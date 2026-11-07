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
DateTime trafficHoldSince = DateTime.Now;
const float TRAFFIC_STUCK_TIME = 15f;  //[s] held by traffic this long -> stop waiting and pass on the right
const float TRAFFIC_ESCAPE_TIME = 10f; //[s] for this long
int trafficTick = 0;
String doneHoles = "";        // one char per hole (grid cell): '1' = finished (by any co-op ship)
int myHole = -1;              // grid cell of the hole this ship works on
bool coopWaiting = false;
Vector3 coopWaitPos = new Vector3();

String JobKey()
{
    if (jobState == JobState.NoJob || shipMode == ShipMode.Shuttle) return "";
    return Math.Round(jobPosition.position.X) + "|" + Math.Round(jobPosition.position.Y) + "|" + Math.Round(jobPosition.position.Z) +
        "|" + jobWidth + "|" + jobHeight + "|" + (int)jobStartPos + "|" + Math.Round(toolWidth, 1) + "|" + Math.Round(toolHeight, 1);
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
        if ((DateTime.Now - traffic[i].time).TotalSeconds > TRAFFIC_TIMEOUT) traffic.RemoveAt(i);
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
    IGC.SendBroadcastMessage(TRAFFIC_TAG, myGridId + ";" + VectorToString(shipPosition) + ";" +
        VectorToString(remoteControl.GetShipVelocities().LinearVelocity) + ";" + shipDiameter + ";" + JobKey() + ";" +
        (jobState == JobState.Active ? myHole : -1) + ";" + doneHoles + ";" + ClaimedDockId() + "," + DockPhase() + ";" + VectorToString(trafficHeading) +
        ";" + (dockQueued ? 1 : 0) + ";" + (holeWanted && jobState == JobState.Active ? 1 : 0) + "," + shipFootWidth + "," + shipFootHeight);
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
        ship.time = DateTime.Now;
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
    }
    catch { } // malformed message from another script: ignore
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

// Docking, undocking and backing out of a hole: traffic only slows/holds the ship, no side step
// (the ship has to stay on its line to the connector or hole).
bool IsPreciseFlightState(NavState s)
{
    return s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.Docking || s == NavState.RetryDocking ||
        s == NavState.Undocking || s == NavState.Returning || s == NavState.AlignJob;
}

void UpdateTrafficAvoidance()
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    trafficHeading = new Vector3();
    bool free = IsFreeFlightState(currentNavState);
    if (!trafficEnabled || (!free && !IsPreciseFlightState(currentNavState))) { trafficHolding = false; return; }
    trafficMyVel = remoteControl.GetShipVelocities().LinearVelocity;
    ComputeTrafficAvoidance(remoteControl.GetNaturalGravity(), !free);
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
    bool goingHome = currentNavState == NavState.FlyToDockArea && !StationCleared(1); // lines up in the dock queue

    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip other = traffic[i];
        if (other.dockPhase == 3 && other.vel.LengthSquared() < 0.25f) continue; // parked at a connector
        Vector3 pos = other.pos + other.vel * (float)(DateTime.Now - other.time).TotalSeconds;
        Vector3 rel = pos - shipPosition;
        float clear = (shipDiameter + other.diameter) * 0.5f;
        float along = haveDir ? Vector3.Dot(rel, dir) : 0;
        if (along > 0 && (rel - dir * along).Length() <= clear * 1.2f)
        {
            // in my corridor, ahead
            float otherAlong = Vector3.Dot(other.vel, dir);
            bool hasHeading = other.heading.LengthSquared() > 0.25f;
            bool headOn = otherAlong < -1f || (hasHeading && Vector3.Dot(other.heading, dir) < -0.5f);
            if (precise)
            {
                if (headOn && myGridId < other.id) continue; // the other ship gives way
            }
            else
            {
                if (other.queued)
                {
                    // a ship waiting outside the docking area: leaving or cleared ships pass it on the right
                    if (!goingHome) { trafficShift = right * clear * 1.3f; continue; }
                    // both queueing: the one closer to the waiting point is ahead (tie: lower id)
                    float mine = Vector3.Distance(shipPosition, navTargetPos), its = Vector3.Distance(pos, navTargetPos);
                    if (mine < its || (mine == its && myGridId < other.id)) continue;
                }
                if (headOn)
                {
                    // head-on (also when both are slow or stopped): both keep right and pass
                    if (along < (shipSpeed - otherAlong) * 6f + clear * 4f) trafficShift = right * clear * 1.3f;
                    continue;
                }
                if (hasHeading && myGridId < other.id)
                {
                    // does the other ship have me ahead in its corridor too (crossing paths)? then one
                    // of us has to go: the lower id goes, keeping right, the other one waits
                    Vector3 back = -rel;
                    float otherAhead = Vector3.Dot(back, other.heading);
                    if (otherAhead > 0 && (back - other.heading * otherAhead).Length() <= clear * 1.2f)
                    { trafficShift = right * clear * 1.3f; continue; }
                }
            }
            // same direction, or a ship that isn't moving on its own: follow at a distance, hold if it stops
            float gap = along - clear - followGap * shipDiameter;
            float limit = Math.Max(0, otherAlong) + Math.Max(0, gap) / FOLLOW_TIME;
            if (headOn) limit = Math.Max(0, gap + Math.Min(0, otherAlong) * FOLLOW_TIME * 2) / FOLLOW_TIME; // it comes towards me: stop early
            if (trafficSpeedLimit < 0 || limit < trafficSpeedLimit) { trafficSpeedLimit = limit; limiter = other; }
            continue;
        }
        // any other moving ship on a collision course (crossing, from the side or from behind):
        // the higher id stops; both stop when it's close
        if (other.vel.LengthSquared() < 1f) continue;
        if (haveDir && (rel - dir * along).Length() <= clear * 1.2f) continue; // behind me in my lane: it has to slow down, not me
        Vector3 rv = other.vel - trafficMyVel;
        float rv2 = rv.LengthSquared();
        if (rv2 < 0.25f) continue;
        float t = -Vector3.Dot(rel, rv) / rv2;
        if (t <= 0 || t > 5f || (rel + rv * t).Length() >= clear * 1.1f) continue;
        if (myGridId > other.id || t < 2f) { trafficSpeedLimit = 0; limiter = other; }
    }

    // safety net: held by traffic for too long -> stop waiting for a while and pass on the right
    // (not while docking or in a hole, and not in the queue outside the docking area)
    bool held = trafficSpeedLimit >= 0 && trafficSpeedLimit < 0.5f;
    if (!held || precise || (limiter != null && limiter.queued && goingHome)) { trafficHolding = false; return; }
    if (!trafficHolding) { trafficHolding = true; trafficHoldSince = DateTime.Now; }
    double heldFor = (DateTime.Now - trafficHoldSince).TotalSeconds;
    if (heldFor > TRAFFIC_STUCK_TIME + TRAFFIC_ESCAPE_TIME) trafficHoldSince = DateTime.Now;
    else if (heldFor > TRAFFIC_STUCK_TIME) { trafficSpeedLimit = -1; trafficShift = right * shipDiameter * 1.3f; statusMessage = "Traffic: passing a stuck ship"; }
}

// ---- co-op mining ----
// Ships on the same job claim single holes, in the normal mining order. A claim is kept while the
// job runs (also while the ship is away unloading), so a half-mined hole is never taken over.
// A neighbouring hole is skipped only when the ship is wider than the area it drills (the two
// ships would touch). Finished holes are shared.

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

// Two holes so close that ships drilling them would touch (or the same hole).
bool HolesTouch(int a, int b, int w, float tw, float th, float fw, float fh)
{
    return a == b || (Math.Abs(a % w - b % w) * tw < fw + 0.5f && Math.Abs(a / w - b / w) * th < fh + 0.5f);
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

// Another ship on this job has this hole, or one so close that the ships would touch. keeping:
// the hole is already ours, so only a ship with a lower id makes us give it up (both claimed at once).
bool HoleConflict(int cell, bool keeping)
{
    if (!trafficEnabled) return false;
    String key = JobKey();
    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip t = traffic[i];
        if (t.job != key || t.hole < 0 || (keeping && t.id > myGridId)) continue;
        if (HolesTouch(cell, t.hole, jobWidth, toolWidth, toolHeight, shipFootWidth, shipFootHeight)) return true;
    }
    return false;
}

// Called before flying to hole idx (index in the mining order). Returns the next free hole index,
// or jobWidth * jobHeight when every hole is finished or being mined by another ship (this ship's
// part of the job is done). A single ship gets its holes in the original order.
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
        if (coordCell == -2) { holeWanted = false; myHole = -1; return n; } // nothing left for this ship
        if (coordCell == -1 && myHole >= 0 && !IsHoleDone(myHole)) { holeWanted = false; return idx; } // controller doesn't know it yet: keep it
        myHole = -1;
        if (!holeWanted) { holeWanted = true; coordWaitSince = DateTime.Now; }
        if ((DateTime.Now - coordWaitSince).TotalSeconds < 5) return -1; // wait for the answer
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
    return n;
}

// ---- job coordination by the [PAM-Controller] ----
// When a controller hears this job it keeps the master record (finished holes, which ship has
// which hole) and assigns the next hole when a ship asks (want flag in the traffic message). It
// broadcasts "J;job;doneHoles;ship:cell,..." on JOB_TAG. Without a controller (or if it doesn't
// answer within 5 s) the ships use the peer rules above.

const String JOB_TAG = "[PAMJOB]";
IMyBroadcastListener coordListener = null;
DateTime coordTime = DateTime.MinValue;
DateTime coordWaitSince = DateTime.Now;
int coordCell = -1; // -1 none assigned (yet), -2 nothing left for this ship
bool holeWanted = false;

// coordCell -3: the controller's job state doesn't list this ship (it can't hear it): peer rules
bool JobCoordinated() { return trafficEnabled && coordCell != -3 && (DateTime.Now - coordTime).TotalSeconds < 3; }

void ParseJobState(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 4 || f[0] != "J" || f[1] == "" || f[1] != JobKey()) return;
    coordTime = DateTime.Now;
    MergeDoneHoles(f[2]);
    coordCell = -3;
    String me = myGridId + ":";
    String[] e = f[3].Split(',');
    for (int i = 0; i < e.Length; i++) if (e[i].StartsWith(me)) coordCell = int.Parse(e[i].Substring(me.Length));
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
