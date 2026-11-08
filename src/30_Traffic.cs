// ========================== TRAFFIC & CO-OP MINING ==========================
// PAM ships share a small, frequent traffic message (own IGC tag, independent of the controller
// protocol and of Broadcast_Channel): position, velocity, size, job and the hole they
// work on. It is used for:
//  - lanes: on the recorded path, ships keep right of it by laneOffset (tapering to the path
//    near both ends), so ships going opposite ways pass side by side
//  - spacing: slow down / hold behind a PAM ship ahead; keep right when one comes head-on
//  - co-op mining: ships on the same job (IMPORT JOB) claim single holes, skip holes other
//    ships have or are too close to, and share which holes are finished

const String TRAFFIC_TAG = "[PAMTFC]";
const float TRAFFIC_TIMEOUT = 5f; //[s] forget ships not heard from for this long
const float FOLLOW_TIME = 2f;     //[s] time to close the gap to a ship ahead

class TrafficShip
{
    public long id;
    public Vector3 pos, vel;
    public Vector3 heading;  // direction to its flight target (zero when it isn't avoiding traffic)
    public float diameter;
    public String job = "";
    public int hole = -1;  // co-op: grid cell it claims
    public bool queued;    // holding outside the docking area for station clearance
    public bool parked;    // out of service (can't dock), parked beside the path: pass it
    public int jobFlag;    // 1: working in the job area, aligned to it; 2: deep in its hole (inside the rock)
    public int prio;       // right of way at the job site (TrafficPrio)
    public float metric = -1; // distance it still has to go, whole metres (-1 unknown)
    public float fw, fh;   // its size across the drill face
    public long dock = 0;  // dock pool: connector this ship uses or heads for
    public int dockPhase = 0;
    public DateTime time;
}

IMyBroadcastListener trafficListener = null;
long myGridId = 0;
List<TrafficShip> traffic = new List<TrafficShip>();
Vector3 trafficShift = new Vector3();
float trafficSpeedLimit = -1; // -1 = no limit
Vector3 trafficHeading = new Vector3();
bool trafficHolding = false;
DateTime trafficHoldSince = TimeNow();
const float TRAFFIC_STUCK_TIME = 15f;  //[s] held by traffic this long -> stop waiting and pass on the right
const float TRAFFIC_ESCAPE_TIME = 10f; //[s] for this long
int trafficTick = 0;
String doneHoles = "";        // one char per hole (grid cell): '1' = finished (by any co-op ship)
int myHole = -1;              // grid cell of the hole this ship works on
bool coopWaiting = false, coopWaitAside = false, exitRise = false;
Vector3 coopWaitPos = new Vector3();

// hole spacing of the job: the drill area it was set up with (or imported with), so a ship that
// loses or gains a drill keeps the same grid as the other ships
float JTW() { return jobToolW > 0 ? jobToolW : toolWidth; }
float JTH() { return jobToolH > 0 ? jobToolH : toolHeight; }

String JobKey()
{
    if (jobState == JobState.NoJob || shipMode == ShipMode.Shuttle) return "";
    return Math.Round(jobPosition.position.X) + "|" + Math.Round(jobPosition.position.Y) + "|" + Math.Round(jobPosition.position.Z) +
        "|" + jobWidth + "|" + jobHeight + "|" + (int)jobStartPos + "|" + Math.Round(JTW(), 1) + "|" + Math.Round(JTH(), 1) + "|" + jobSerial;
}

void UpdateTraffic()
{
    if (!trafficEnabled) return;
    if (trafficListener == null) trafficListener = IGC.RegisterBroadcastListener(TRAFFIC_TAG);
    myGridId = Me.CubeGrid.EntityId;
    int budget = 30;
    while (trafficListener.HasPendingMessage && budget-- > 0)
    {
        String data = trafficListener.AcceptMessage().Data as String;
        if (data != null) ParseTraffic(data);
    }
    for (int i = traffic.Count - 1; i >= 0; i--)
        if ((TimeNow() - traffic[i].time).TotalSeconds > TRAFFIC_TIMEOUT) traffic.RemoveAt(i);
    if (tccListener == null) tccListener = IGC.RegisterBroadcastListener(TCC_TAG);
    budget = 5;
    while (tccListener.HasPendingMessage && budget-- > 0)
    {
        String data = tccListener.AcceptMessage().Data as String;
        if (data != null) try { ParseTrafficControl(data); } catch { }
    }
    if (coordListener == null) coordListener = IGC.RegisterBroadcastListener(JOB_TAG);
    budget = 10;
    while (coordListener.HasPendingMessage && budget-- > 0)
    {
        String data = coordListener.AcceptMessage().Data as String;
        if (data != null) try { ParseJobState(data); } catch { }
    }

    // idle ships send too: docked (their connector counts as taken) or hovering (an obstacle)
    if (++trafficTick < 3) return; // ~2 messages per second
    trafficTick = 0;
    if (debugDraw) // for the controller's debug drawing: id;jobPos;left;down;target;rays;forward;path (every ~10 s)
    {
        IGC.SendBroadcastMessage("[PAMDBG]", myGridId + ";" + VectorToString(jobPosition.position) + ";" +
            VectorToString(jobState == JobState.NoJob ? new Vector3() : jobPosition.leftDir) + ";" + VectorToString(miningDown) + ";" +
            VectorToString(navTargetPos + trafficShift) + ";" + String.Join(":", debugRays) + ";" + VectorToString(miningForward) + ";" + (++dbgPathTick % 20 == 1 ? DbgPath() : ""));
        debugRays.Clear();
    }
    IGC.SendBroadcastMessage(TRAFFIC_TAG, myGridId + ";" + VectorToString(shipPosition) + ";" +
        VectorToString(remoteControl.GetShipVelocities().LinearVelocity) + ";" + shipDiameter + ";" + JobKey() + ";" +
        (jobState == JobState.Active ? myHole : -1) + ";" + doneHoles + ";" + ClaimedDockId() + "," + DockPhase() + ";" + VectorToString(trafficHeading) +
        ";" + (outOfService || emgStage == 1 || emgStage == 2 ? 2 : dockQueued ? 1 : 0) + ";" + (holeWanted && jobState == JobState.Active ? 1 : 0) + "," + shipFootWidth + "," + shipFootHeight + "," + JobFlag() + "," + (sentPrio = TrafficPrio()) + "," + (sentDest = (float)Math.Round(DestDist())) + ";" + Me.GetId());
}

void ParseTraffic(String data)
{
    try
    {
        String[] f = data.Split(';');
        if (f.Length < 7) return;
        long id = long.Parse(f[0]);
        if (id == myGridId) return;
        TrafficShip ship = null;
        for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) ship = traffic[i];
        if (ship == null) { ship = new TrafficShip(); ship.id = id; traffic.Add(ship); }
        ship.pos = StringToVector(f[1]);
        ship.vel = StringToVector(f[2]);
        ship.diameter = float.Parse(f[3]);
        ship.job = f[4];
        ship.hole = int.Parse(f[5]);
        ship.time = TimeNow();
        if (ship.job != "" && ship.job == JobKey()) MergeDoneHoles(f[6]);
        ship.dock = 0; ship.dockPhase = 0;
        if (f.Length >= 8)
        {
            String[] d = f[7].Split(',');
            ship.dock = long.Parse(d[0]);
            ship.dockPhase = int.Parse(d[1]);
        }
        ship.heading = f.Length >= 9 ? StringToVector(f[8]) : new Vector3();
        ship.queued = f.Length >= 10 && f[9] == "1";
        ship.parked = f.Length >= 10 && f[9] == "2";
        ship.jobFlag = 0; ship.prio = 0;
        if (f.Length >= 11)
        {
            String[] w = f[10].Split(',');
            if (w.Length >= 4) { ship.fw = float.Parse(w[1]); ship.fh = float.Parse(w[2]); ship.jobFlag = int.Parse(w[3]); if (w.Length >= 5) ship.prio = int.Parse(w[4]); ship.metric = w.Length >= 6 ? float.Parse(w[5]) : -1; }
        }
    }
    catch { } // malformed message from another script: ignore
}

bool DockedPam(long id)
{
    for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) return traffic[i].dockPhase == 3 && traffic[i].vel.LengthSquared() < 0.25f;
    return false;
}

bool IsTrafficShip(long id)
{
    for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) return true;
    return false;
}

// ---- lanes ----

// Keep-right offset for path waypoint idx when travelling in pathDir (+1/-1): perpendicular to
// the path and to "up", so opposite directions end up on opposite sides whatever way the path
// was recorded. Tapers to 0 near both path ends (dock and job).
Vector3 GetLaneOffset(int idx, int pathDir)
{
    if (laneOffset <= 0 || waypoints.Count < 2 || idx < 0 || idx >= waypoints.Count) return new Vector3();
    WaypointInfo wp = waypoints[idx];
    Vector3 travel = (waypoints[Math.Min(waypoints.Count - 1, idx + 1)].position - waypoints[Math.Max(0, idx - 1)].position) * pathDir;
    Vector3 up = wp.gravity.LengthSquared() > 0.01f ? -Vector3.Normalize(wp.gravity) : -wp.downDir;
    Vector3 right = Vector3.Cross(travel, up);
    if (right.LengthSquared() < 0.0001f) return new Vector3();
    float dist = laneOffset * shipDiameter;
    float fromEnds = Math.Min(Vector3.Distance(wp.position, waypoints[0].position),
        Vector3.Distance(wp.position, waypoints[waypoints.Count - 1].position));
    return Vector3.Normalize(right) * dist * Math.Min(1, fromEnds / (dist * 4));
}

// ---- spacing ----

Vector3 trafficMyVel = new Vector3();
bool sidestep = false; // moving out of the way of a passing ship
List<String> debugRays = new List<String>();
int dbgPathTick = 0;

// recorded path for the controller's debug drawing, whole metres
String DbgPath()
{
    String s = "";
    for (int i = 0; i < waypoints.Count; i++) { Vector3 w = waypoints[i].position; s += (i > 0 ? ":" : "") + (int)w.X + "|" + (int)w.Y + "|" + (int)w.Z; }
    return s;
}

// Docking, undocking and backing out of a hole: traffic only slows/holds the ship, no side step
// (the ship has to stay on its line to the connector or hole).
bool InJobArea()
{
    NavState s = currentNavState;
    return JobKey() != "" && (s == NavState.FlyToXY || s == NavState.Mining || s == NavState.Returning ||
        s == NavState.ReturnToDock || s == NavState.FlyToDropPos);
}

float JobRadius() { return 0.5f * (float)Math.Sqrt(Math.Pow(jobWidth * JTW(), 2) + Math.Pow(jobHeight * JTH(), 2)); }

bool NearJobArea() { return JobKey() != "" && Vector3.Distance(shipPosition, jobPosition.position) < JobRadius() + shipDiameter * 2; }

// right of way: 3 emergency (low on energy, on the way to dock) or docking / undocking, 2 heading home (anywhere: backing out
// to go home, flying to the path, on the path home), 1 moving between holes or arriving at the job
// area, 0 other (e.g. on the way to the job), -1 waiting where it is (for a hole, clearance, a dock)
bool HeadingHome() { NavState s = currentNavState; return s == NavState.ReturnToDock || s == NavState.FlyToPath || s == NavState.FlyToDockArea; }

int TrafficPrio()
{
    NavState s = currentNavState;
    if (waitingHere) return -1; // waiting (for a hole, dock clearance, a free dock): everyone else goes first
    // low on energy on its way to dock, or cleared and docking / undocking in the docking area
    if (emergency || s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.Docking || s == NavState.RetryDocking || s == NavState.Undocking) return 3;
    if (HeadingHome()) return 2;
    return s == NavState.FlyToXY || s == NavState.Returning || s == NavState.FlyToDropPos || (s == NavState.FlyToJobArea && NearJobArea()) ? 1 : 0;
}

// How far this ship still has to go, along the road: to the path waypoint it is on (or joins:
// the nearest one, when it is beside the path or still at the job site), then along the path to
// its end, home or job. So a ship already on the road goes before one that still has to join it.
float DestDist()
{
    bool home = HeadingHome() && homeDock.isSet;
    bool onPath = currentNavState == NavState.FlyToDockArea || currentNavState == NavState.FlyToJobArea;
    if (waypoints.Count == 0 || (!home && !onPath)) return Vector3.Distance(shipPosition, navTargetPos);
    int k = pathWaypointIndex, dir = pathDirection;
    if (!onPath || dir == 0 || k < 0 || k >= waypoints.Count)
    {
        // joining the road: nearest waypoint, towards the home end of the path
        k = 0;
        for (int i = 1; i < waypoints.Count; i++)
            if (Vector3.Distance(waypoints[i].position, shipPosition) < Vector3.Distance(waypoints[k].position, shipPosition)) k = i;
        dir = Vector3.Distance(waypoints[0].position, homeDock.position) < Vector3.Distance(waypoints[waypoints.Count - 1].position, homeDock.position) ? -1 : 1;
    }
    float d = Vector3.Distance(shipPosition, waypoints[k].position);
    for (int i = k; i + dir >= 0 && i + dir < waypoints.Count; i += dir) { d += Vector3.Distance(waypoints[i].position, waypoints[i + dir].position); k = i + dir; }
    return home ? d + Vector3.Distance(waypoints[k].position, homeDock.position) : d;
}

// right of way as last sent: the other ships compare against these same numbers (a value fresher
// than the one they have would make both ships think they go first)
float sentDest = -1;
int sentPrio = 0;
bool waitingHere = false;

// between ships with the same right of way: the one closer to where it is going goes first (whole
// metres; equal: lower id). One number per ship, so the order is the same for every ship: no circle
// of ships all giving way to each other.
bool GoesFirst(float myDist, TrafficShip o)
{
    float mine = (float)Math.Round(myDist);
    if (o.metric < 0 || mine == o.metric) return myGridId < o.id;
    return mine < o.metric;
}

// 0: not working the job; 1: in the job area, aligned to it; 2: deeper in its hole than half its size
int JobFlag()
{
    if (!InJobArea()) return 0;
    return currentNavState != NavState.FlyToXY && Vector3.Dot(shipPosition - miningTargetPos, miningForward) > shipDiameter * 0.5f ? 2 : 1;
}

bool IsPreciseFlightState(NavState s)
{
    return s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.Docking || s == NavState.RetryDocking ||
        s == NavState.Undocking || s == NavState.Returning || s == NavState.ReturnToDock || s == NavState.FlyToDropPos || s == NavState.AlignJob;
}

void UpdateTrafficAvoidance()
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    trafficHeading = new Vector3();
    // in a connector's lane (pool connector too): precise, holds only (no side steps, no moving aside)
    bool free = IsFreeFlightState(currentNavState) && !(currentNavState == NavState.ApproachDock && !toLane);
    if (!trafficEnabled || (!free && !IsPreciseFlightState(currentNavState))) { trafficHolding = false; return; }
    trafficMyVel = remoteControl.GetShipVelocities().LinearVelocity;
    ComputeTrafficAvoidance(remoteControl.GetNaturalGravity(), !free);
    ApplyTrafficControl();
}

// [PAM-Controller] traffic control: "C;ship:limit,..." lists the ships that have to give way
IMyBroadcastListener tccListener = null;
float tccLimit = -1;
DateTime tccTime = DateTime.MinValue;

void ParseTrafficControl(String data)
{
    if (!data.StartsWith("C;")) return;
    tccTime = TimeNow();
    tccLimit = -1;
    String me = myGridId + ":";
    String[] e = data.Substring(2).Split(',');
    for (int i = 0; i < e.Length; i++) if (e[i].StartsWith(me)) tccLimit = float.Parse(e[i].Substring(me.Length));
}

// on top of the ship's own avoidance, after its 15 s escape: the controller sees all ships
void ApplyTrafficControl()
{
    if (tccLimit < 0 || (TimeNow() - tccTime).TotalSeconds > 1.5 || sidestep) return; // moving aside resolves it
    trafficSpeedLimit = trafficSpeedLimit < 0 ? tccLimit : Math.Min(trafficSpeedLimit, tccLimit);
    if (tccLimit < 0.5f) statusMessage = "Traffic: giving way (controller)";
}

void ComputeTrafficAvoidance(Vector3 gravity, bool precise = false)
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    Vector3 toTarget = navTargetPos - shipPosition;
    float targetDist = toTarget.Length();
    bool haveDir = targetDist >= 1f;
    Vector3 dir = haveDir ? toTarget / targetDist : new Vector3();
    trafficHeading = dir; // sent to the other ships: lets them tell head-on from following, even when stopped
    Vector3 up = gravity.LengthSquared() > 0.01f ? -Vector3.Normalize(gravity) : -downDirection;
    Vector3 right = Vector3.Cross(dir, up);
    right = right.LengthSquared() < 0.0001f ? -leftDirection : Vector3.Normalize(right);
    TrafficShip limiter = null;
    bool cpaHold = false; // held for a collision course: stop, don't move aside
    bool goingHome = currentNavState == NavState.FlyToDockArea && !StationCleared(1); // lines up in the dock queue
    int myPrio = sentDest < 0 ? TrafficPrio() : sentPrio;
    float myDest = sentDest < 0 ? DestDist() : sentDest;
    bool cpaClose = false; // collision course less than 2 s away

    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip other = traffic[i];
        if (other.dockPhase == 3 && other.vel.LengthSquared() < 0.25f) continue; // parked at a connector
        Vector3 pos = other.pos + other.vel * (float)(TimeNow() - other.time).TotalSeconds;
        Vector3 rel = pos - shipPosition;
        float clear = (shipDiameter + other.diameter) * 0.5f;
        // two ships working the same job are aligned to it and drill different holes, so a ship at
        // another hole, even the next one, is never in the way: only closer than one hole spacing counts
        bool sameJob = other.job != "" && other.job == JobKey();
        if (sameJob && other.jobFlag == 2) continue; // inside the rock in its own hole: can't be in anyone's way
        bool sideBySide = sameJob && other.jobFlag == 1 && InJobArea();
        if (sideBySide) clear = Math.Min(JTW(), JTH()) * 0.9f;
        float corridor = clear * (sideBySide ? 1f : 1.2f); // side by side: a ship at the next hole is outside it
        float along = haveDir ? Vector3.Dot(rel, dir) : 0;
        if (along > 0 && (rel - dir * along).Length() <= corridor)
        {
            // in my corridor, ahead
            float otherAlong = Vector3.Dot(other.vel, dir);
            bool hasHeading = other.heading.LengthSquared() > 0.25f;
            bool headOn = otherAlong < -1f || (hasHeading && Vector3.Dot(other.heading, dir) < -0.5f);
            // out of service, or waiting (for a hole, a dock) and not where I'm going: pass it
            if (!precise && (other.parked || other.prio < 0 && !other.queued && other.vel.LengthSquared() < 1 && Vector3.Distance(pos, navTargetPos) > clear))
            { trafficShift = PassRight(dir, right, clear, along); continue; }
            // a ship with right of way: give way, don't pass it
            if (!precise && other.prio <= myPrio) // docking, undocking, in a hole: no side step, hold for every ship in the way
            {
                if (other.queued)
                {
                    // a ship waiting outside the docking area: leaving or cleared ships pass it on the right
                    if (!goingHome) { trafficShift = PassRight(dir, right, clear, along); continue; }
                    // both queueing: the one closer to the waiting point is ahead (tie: lower id)
                    float mine = Vector3.Distance(shipPosition, navTargetPos), its = Vector3.Distance(pos, navTargetPos);
                    if (mine < its || (mine == its && myGridId < other.id)) continue;
                }
                if (headOn)
                {
                    // head-on (also when both are slow or stopped): both keep right and pass
                    if (along < (shipSpeed - otherAlong) * 6f + clear * 4f)
                    {
                        trafficShift = PassRight(dir, right, clear, along);
                        // not beside it yet: slow down, so there's time to get out of its way
                        if ((rel - dir * along).Length() < clear) Limit(Math.Max(1, (along - clear) / 4), other, ref limiter);
                    }
                    continue;
                }
                if (hasHeading && GoesFirst(myDest, other))
                {
                    // does the other ship have me ahead in its corridor too (crossing paths)? then one
                    // of us has to go: the one with right of way goes, keeping right, the other one waits
                    Vector3 back = -rel;
                    float otherAhead = Vector3.Dot(back, other.heading);
                    if (otherAhead > 0 && (back - other.heading * otherAhead).Length() <= corridor)
                    { trafficShift = PassRight(dir, right, clear, along); continue; }
                }
            }
            // same direction, or a ship that isn't moving on its own: follow at a distance, hold if it stops
            float gap = along - clear - (sideBySide ? 1f : followGap * shipDiameter);
            float limit = Math.Max(0, otherAlong) + Math.Max(0, gap) / FOLLOW_TIME;
            if (headOn) limit = Math.Max(0, gap + Math.Min(0, otherAlong) * FOLLOW_TIME * 2) / FOLLOW_TIME; // it comes towards me: stop early
            Limit(limit, other, ref limiter);
            continue;
        }
        // any other moving ship on a collision course (crossing, from the side or from behind):
        // the higher id stops; both stop when it's close
        if (other.vel.LengthSquared() < 1f) continue;
        if (haveDir && (rel - dir * along).Length() <= corridor) continue; // behind me in my lane: it has to slow down, not me
        Vector3 rv = other.vel - trafficMyVel;
        float rv2 = rv.LengthSquared();
        if (rv2 < 0.25f) continue;
        float t = -Vector3.Dot(rel, rv) / rv2;
        if (t <= 0 || t > 5f || (rel + rv * t).Length() >= (sideBySide ? clear : clear * 1.1f)) continue;
        if (other.prio > myPrio || (other.prio == myPrio && !GoesFirst(myDest, other)) || t < 2f) { trafficSpeedLimit = 0; limiter = other; cpaHold = true; cpaClose |= t < 2f; }
    }

    // giving way: a ship with right of way that is coming through where I am (or wants to: its
    // direction when stopped) -> move slowly out of its line, sideways (only in free flight; the
    // camera brake stops the ship if there's no room there)
    bool wasAside = sidestep; // once moving aside, keep going until out of its line (it starts only when stopped)
    sidestep = false;
    if (!precise && !cpaHold && (shipSpeed < 1 || wasAside))
        for (int i = 0; i < traffic.Count; i++)
        {
            TrafficShip o = traffic[i];
            if (o.parked || o.jobFlag == 2 || !(o.prio > myPrio || (o.prio == myPrio && !GoesFirst(myDest, o)))) continue;
            Vector3 oi = o.vel.LengthSquared() > 1 ? o.vel : o.heading;
            if (oi.LengthSquared() < 0.25f) continue;
            Vector3 od = Vector3.Normalize(oi);
            // a ship going my way (behind me, following) never makes me move aside: it slows down
            if (haveDir && Vector3.Dot(od, dir) > 0.3f) continue;
            Vector3 rel = shipPosition - o.pos;
            float along = Vector3.Dot(rel, od);
            Vector3 lat = rel - od * along;
            float clear = (shipDiameter + o.diameter) * 0.6f;
            if (along <= 0 || along > clear * 4 || lat.Length() >= clear) continue;
            Vector3 away = lat.Length() > 0.5f ? Vector3.Normalize(lat) : Vector3.Normalize(Vector3.Cross(od, up));
            trafficShift = shipPosition + away * clear - navTargetPos; // fly to the point beside its line
            trafficSpeedLimit = 2;
            sidestep = true;
            statusMessage = "Traffic: moving aside";
            return;
        }

    // safety net: held by traffic for too long -> stop waiting for a while and pass on the right
    // (not while docking or in a hole, and not in the queue outside the docking area)
    bool held = trafficSpeedLimit >= 0 && trafficSpeedLimit < 0.5f;
    if (!held || precise || (limiter != null && limiter.queued && goingHome)) { trafficHolding = false; return; }
    if (!trafficHolding) { trafficHolding = true; trafficHoldSince = TimeNow(); }
    double heldFor = (TimeNow() - trafficHoldSince).TotalSeconds;
    if (heldFor > TRAFFIC_STUCK_TIME + TRAFFIC_ESCAPE_TIME) trafficHoldSince = TimeNow();
    else if (heldFor > TRAFFIC_STUCK_TIME) { trafficSpeedLimit = cpaClose ? 0 : -1; trafficShift = PassRight(dir, right, shipDiameter, shipDiameter * 4); statusMessage = "Traffic: passing a stuck ship"; }
}

void Limit(float v, TrafficShip o, ref TrafficShip limiter) { if (trafficSpeedLimit < 0 || v < trafficSpeedLimit) { trafficSpeedLimit = v; limiter = o; } }

// passing on the right: steer to a point beside the other ship where we meet it, not to the far
// target with an offset (that would hardly move the ship sideways by the time they meet)
Vector3 PassRight(Vector3 dir, Vector3 right, float clear, float along)
{
    return dir * Math.Min((navTargetPos - shipPosition).Length(), Math.Max(along * 0.5f, clear)) + right * clear * 1.3f + shipPosition - navTargetPos;
}

// ---- co-op mining ----
// Ships on the same job claim single holes, in the normal mining order. A claim is kept while the
// job runs (also while the ship is away unloading), so a half-mined hole is never taken over.
// The next ship takes the next hole, right beside the other one. Finished holes are shared.

List<int> holeCells = new List<int>(); // mining order: grid cell (row * jobWidth + col) for each hole index
String holeOrderKey = "";

// Same order as GetNextHolePosition (top-left zigzag or centre spiral), all holes at once.
void EnsureHoleOrder()
{
    String key = jobWidth + "|" + jobHeight + "|" + (int)jobStartPos;
    if (key == holeOrderKey) return;
    holeOrderKey = key;
    BuildHoleOrder(jobWidth, jobHeight, jobStartPos, holeCells);
}

void BuildHoleOrder(int w, int h, StartPosition start, List<int> cells)
{
    cells.Clear();
    if (start == StartPosition.TopLeft)
    {
        for (int k = 0; k < w * h; k++)
        {
            int row = k / w;
            cells.Add(row * w + (row % 2 == 0 ? k - row * w : w - 1 - (k - row * w)));
        }
        return;
    }
    int halfW = (int)Math.Ceiling(w / 2f), halfH = (int)Math.Ceiling(h / 2f);
    int halfW2 = w / 2, halfH2 = h / 2;
    int x = 0, y = 0, dx = 0, dy = -1, max = Math.Max(w, h);
    for (int i = 0; i < max * max; i++)
    {
        if (-halfW < x && x <= halfW2 && -halfH < y && y <= halfH2) cells.Add((y - 1 + halfH) * w + (x - 1 + halfW));
        if (x == y || (x < 0 && x == -y) || (x > 0 && x == 1 - y)) { int t = dx; dx = -dy; dy = t; }
        x += dx; y += dy;
    }
}

bool EnsureDoneHoles()
{
    int n = Math.Max(0, jobWidth * jobHeight);
    if (doneHoles.Length == n) return false;
    doneHoles = new String('0', n);
    myHole = -1;
    return true;
}

void MergeDoneHoles(String other)
{
    EnsureDoneHoles();
    if (other.Length != doneHoles.Length) return;
    char[] h = doneHoles.ToCharArray();
    for (int i = 0; i < h.Length; i++) if (other[i] == '1') h[i] = '1';
    doneHoles = new String(h);
}

bool IsHoleDone(int cell) { return cell >= 0 && cell < doneHoles.Length && doneHoles[cell] == '1'; }

void SetHoleDone(int cell, bool done)
{
    if (cell < 0 || cell >= doneHoles.Length) return;
    char[] h = doneHoles.ToCharArray();
    h[cell] = done ? '1' : '0';
    doneHoles = new String(h);
}

// Another ship on this job has this hole. keeping: the hole is already ours, so only a ship with a
// lower id makes us give it up (both claimed it at once).
bool HoleConflict(int cell, bool keeping)
{
    if (!trafficEnabled) return false;
    String key = JobKey();
    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip t = traffic[i];
        if (t.job != key || t.hole < 0 || (keeping && t.id > myGridId)) continue;
        if (t.hole == cell) return true;
    }
    return false;
}

// Called before flying to hole idx (index in the mining order). Returns the next free hole index,
// -1 while waiting for the controller's answer, -2 when every hole left is taken by other ships
// (wait for one), or jobWidth * jobHeight when every hole is finished. A single ship gets its
// holes in the original order.
int ClaimHole(int idx)
{
    int n = jobWidth * jobHeight;
    if (n < 1) return idx;
    EnsureHoleOrder();
    // no record yet (new job, or a job saved by an older version): holes before the current one
    // were mined in order
    if (EnsureDoneHoles()) for (int k = 0; k < idx && k < n; k++) SetHoleDone(holeCells[k], true);
    if (myHole >= 0 && (idx >= n || holeCells[idx] != myHole)) { SetHoleDone(myHole, true); myHole = -1; } // moved on: finished
    if (JobCoordinated())
    {
        // the [PAM-Controller] assigns the holes
        if (coordCell >= 0 && !IsHoleDone(coordCell)) { holeWanted = false; myHole = coordCell; return holeCells.IndexOf(coordCell); }
        if (coordCell == -2) { myHole = -1; holeWanted = doneHoles.IndexOf('0') >= 0; return holeWanted ? -2 : n; } // all taken: wait (keep asking), all done: done
        // controller doesn't know this ship's hole yet: keep it, unless it gave that hole to another ship
        if (coordCell == -1 && myHole >= 0 && !IsHoleDone(myHole) && !coordOthers.Contains(myHole)) { holeWanted = false; return idx; }
        myHole = -1;
        if (!holeWanted) { holeWanted = true; coordWaitSince = TimeNow(); }
        if ((TimeNow() - coordWaitSince).TotalSeconds < 5) return -1; // wait for the answer
    }
    holeWanted = false;
    if (myHole >= 0 && !IsHoleDone(myHole) && !HoleConflict(myHole, true)) return idx;
    myHole = -1;
    for (int k = 0; k < n; k++)
    {
        int c = holeCells[k];
        if (IsHoleDone(c) || HoleConflict(c, false)) continue;
        myHole = c;
        return k;
    }
    return doneHoles.IndexOf('0') >= 0 ? -2 : n; // holes left, all taken by other ships: wait for one
}

// ---- job coordination by the [PAM-Controller] ----
// When a controller hears this job it keeps the master record (finished holes, which ship has
// which hole) and assigns the next hole when a ship asks (want flag in the traffic message). It
// broadcasts "J;job;doneHoles;ship:cell,..." on JOB_TAG. Without a controller (or if it doesn't
// answer within 5 s) the ships use the peer rules above.

const String JOB_TAG = "[PAMJOB]";
const String TCC_TAG = "[PAMTCC]";
IMyBroadcastListener coordListener = null;
DateTime coordTime = DateTime.MinValue;
DateTime coordWaitSince = TimeNow();
int coordCell = -1; // -1 none assigned (yet), -2 nothing left for this ship
List<int> coordOthers = new List<int>(); // holes the controller has given to other ships
bool holeWanted = false;

// coordCell -3: the controller's job state doesn't list this ship (it can't hear it): peer rules
bool JobCoordinated() { return trafficEnabled && coordCell != -3 && (TimeNow() - coordTime).TotalSeconds < 3; }

void ParseJobState(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 4 || f[0] != "J" || f[1] == "" || f[1] != JobKey()) return;
    coordTime = TimeNow();
    MergeDoneHoles(f[2]);
    coordCell = -3;
    coordOthers.Clear();
    String me = myGridId + ":";
    String[] e = f[3].Split(',');
    for (int i = 0; i < e.Length; i++)
    {
        int c = int.Parse(e[i].Substring(e[i].IndexOf(':') + 1));
        if (e[i].StartsWith(me)) coordCell = c; else coordOthers.Add(c);
    }
}

// hole index -> holeCol/holeRow, like GetNextHolePosition
SpiralResult SetHolePosition(int idx)
{
    EnsureHoleOrder();
    if (idx < 0 || idx >= holeCells.Count) return SpiralResult.AllDone;
    holeCol = holeCells[idx] % jobWidth;
    holeRow = holeCells[idx] / jobWidth;
    return SpiralResult.Found;
}
