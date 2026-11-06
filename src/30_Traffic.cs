// ========================== TRAFFIC & CO-OP MINING ==========================
// PAM ships share a small, frequent traffic message (own IGC tag, independent of the controller
// protocol and of Broadcast_Channel): position, velocity, size, job and the row of holes they
// work on. It is used for:
//  - lanes: on the recorded path, ships keep right of it by laneOffset (tapering to the path
//    near both ends), so ships going opposite ways pass side by side
//  - spacing: slow down / hold behind a PAM ship ahead; keep right when one comes head-on
//  - co-op mining: ships on the same job (IMPORT JOB) claim rows of holes, skip rows other
//    ships are on or right next to, and share which rows are finished

const String TRAFFIC_TAG = "[PAMTFC]";
const float TRAFFIC_TIMEOUT = 5f; //[s] forget ships not heard from for this long
const float FOLLOW_TIME = 2f;     //[s] time to close the gap to a ship ahead

class TrafficShip
{
    public long id;
    public Vector3 pos, vel;
    public float diameter;
    public String job = "";
    public int row = -1;
    public long dock = 0;  // dock pool: connector this ship uses or heads for
    public int dockPhase = 0;
    public DateTime time;
}

IMyBroadcastListener trafficListener = null;
long myGridId = 0;
List<TrafficShip> traffic = new List<TrafficShip>();
Vector3 trafficShift = new Vector3();
float trafficSpeedLimit = -1; // -1 = no limit
int trafficTick = 0;
String doneRows = "";         // one char per row of holes: '1' = finished (by any co-op ship)
int myRow = -1;               // row this ship is working on
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

    if (currentNavState == NavState.Idle || ++trafficTick < 3) return; // ~2 messages per second
    trafficTick = 0;
    bool inJob = currentNavState == NavState.FlyToXY || currentNavState == NavState.Mining || currentNavState == NavState.Returning;
    IGC.SendBroadcastMessage(TRAFFIC_TAG, myGridId + ";" + VectorToString(shipPosition) + ";" +
        VectorToString(remoteControl.GetShipVelocities().LinearVelocity) + ";" + shipDiameter + ";" + JobKey() + ";" +
        (inJob ? myRow : -1) + ";" + doneRows + ";" + ClaimedDockId() + "," + DockPhase());
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
        ship.row = int.Parse(f[5]);
        ship.time = DateTime.Now;
        if (ship.job != "" && ship.job == JobKey()) MergeDoneRows(f[6]);
        ship.dock = 0; ship.dockPhase = 0;
        if (f.Length >= 8)
        {
            String[] d = f[7].Split(',');
            ship.dock = long.Parse(d[0]);
            ship.dockPhase = int.Parse(d[1]);
        }
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

void UpdateTrafficAvoidance()
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    if (!trafficEnabled || traffic.Count == 0 || !IsFreeFlightState(currentNavState)) return;
    ComputeTrafficAvoidance(remoteControl.GetNaturalGravity());
}

void ComputeTrafficAvoidance(Vector3 gravity)
{
    Vector3 toTarget = navTargetPos - shipPosition;
    float targetDist = toTarget.Length();
    if (targetDist < 1f) return;
    Vector3 dir = toTarget / targetDist;
    Vector3 up = gravity.LengthSquared() > 0.01f ? -Vector3.Normalize(gravity) : -downDirection;
    Vector3 right = Vector3.Cross(dir, up);
    right = right.LengthSquared() < 0.0001f ? -leftDirection : Vector3.Normalize(right);

    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip other = traffic[i];
        Vector3 pos = other.pos + other.vel * (float)(DateTime.Now - other.time).TotalSeconds;
        Vector3 rel = pos - shipPosition;
        float along = Vector3.Dot(rel, dir);
        if (along <= 0) continue; // behind
        float clear = (shipDiameter + other.diameter) * 0.5f;
        if ((rel - dir * along).Length() > clear * 1.2f) continue; // not in my corridor
        float otherAlong = Vector3.Dot(other.vel, dir);
        if (otherAlong < -1f)
        {
            // head-on: both ships see each other as oncoming and both keep right
            if (along < (shipSpeed - otherAlong) * 6f + clear * 4f) trafficShift = right * clear * 1.3f;
        }
        else
        {
            // same direction or stopped: follow at a distance, hold if it stops
            float gap = along - clear - followGap * shipDiameter;
            float limit = Math.Max(0, otherAlong) + Math.Max(0, gap) / FOLLOW_TIME;
            if (trafficSpeedLimit < 0 || limit < trafficSpeedLimit) trafficSpeedLimit = limit;
        }
    }
}

// ---- co-op mining ----

bool EnsureDoneRows()
{
    if (doneRows.Length == Math.Max(0, jobHeight)) return false;
    doneRows = new String('0', Math.Max(0, jobHeight));
    myRow = -1;
    return true;
}

void MergeDoneRows(String other)
{
    EnsureDoneRows();
    if (other.Length != doneRows.Length) return;
    char[] rows = doneRows.ToCharArray();
    for (int i = 0; i < rows.Length; i++) if (other[i] == '1') rows[i] = '1';
    doneRows = new String(rows);
}

bool IsRowDone(int row) { return row >= 0 && row < doneRows.Length && doneRows[row] == '1'; }

void SetRowDone(int row)
{
    if (row < 0 || row >= doneRows.Length) return;
    char[] rows = doneRows.ToCharArray();
    rows[row] = '1';
    doneRows = new String(rows);
}

bool IsRowClaimed(int row)
{
    String key = JobKey();
    for (int i = 0; i < traffic.Count; i++)
        if (traffic[i].job == key && traffic[i].row >= 0 && Math.Abs(traffic[i].row - row) <= 1) return true;
    return false;
}

// Called before choosing hole idx. Returns the hole to mine, -1 to wait (the rows left are taken
// by other ships), or an index past the end when every row is finished. A single ship gets its
// holes in the original order.
int ClaimHole(int idx)
{
    if (jobWidth < 1 || jobHeight < 1) return idx;
    // no record yet (new job, or a job saved by an older version): rows before the current
    // hole were mined in order, so count them as finished
    if (EnsureDoneRows()) for (int r = 0; r < idx / jobWidth && r < jobHeight; r++) SetRowDone(r);
    if (myRow >= 0 && idx >= (myRow + 1) * jobWidth) { SetRowDone(myRow); myRow = -1; }
    int row = idx / jobWidth;
    if (row == myRow && !IsRowDone(row)) return idx; // still inside my row
    bool anyLeft = false;
    for (int r = 0; r < jobHeight; r++)
    {
        if (IsRowDone(r)) continue;
        anyLeft = true;
        if (trafficEnabled && IsRowClaimed(r)) continue;
        myRow = r;
        return r == row ? idx : r * jobWidth;
    }
    return anyLeft ? -1 : jobWidth * jobHeight;
}
