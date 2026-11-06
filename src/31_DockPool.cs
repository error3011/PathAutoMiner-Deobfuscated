// ========================== DOCK POOL ==========================
// Miners/grinders only (shuttles keep their two docks). A ship still has its own recorded home
// dock and uses it whenever it's free, so nothing changes without tagged connectors. Base
// connectors named with dockPoolTag form a pool: when the home dock is taken by another PAM ship
// (e.g. several ships sharing one recorded path via IMPORT), the ship docks at a free pool
// connector instead, or waits at the end of the path until one is free.
// How: while docked, the ship stores its pose relative to the base connector it is locked to
// (same for every connector of that type) and the positions of all pool connectors. Claims
// travel in the traffic messages: docked > final approach > on the way; ties go to the lower id.

class PoolDock
{
    public long id;
    public Vector3 pos, fwd, up;
}

List<PoolDock> dockPool = new List<PoolDock>();
bool dockRelSet = false;
Vector3 dockRelFwd, dockRelDown, dockRelLeft, dockRelOffset;
long homeConnId = 0;          // base connector of the recorded home dock (0 = not seen yet)
long poolDockId = 0;          // pool connector in use (0 = own home dock)
WaypointInfo poolDock = null; // ship pose at that connector (null = own home dock)
bool dockWaiting = false;

// Ship pose (as a dock waypoint) when locked to a base connector with this position/orientation.
WaypointInfo DockPose(Vector3 pos, Vector3 fwd, Vector3 up)
{
    MatrixD m = MatrixD.CreateWorld(pos, fwd, up);
    WaypointInfo w = new WaypointInfo(homeDock); // keeps this ship's connector and the gravity
    w.forwardDir = Vector3D.TransformNormal(dockRelFwd, m);
    w.downDir = Vector3D.TransformNormal(dockRelDown, m);
    w.leftDir = Vector3D.TransformNormal(dockRelLeft, m);
    w.position = pos + (Vector3)Vector3D.TransformNormal(dockRelOffset, m);
    w.isSet = true;
    return w;
}

int DockPhase()
{
    NavState s = currentNavState;
    if (s == NavState.Idle) return FindConnector(MyShipConnectorStatus.Connected) != null ? 3 : 0; // idle but docked still holds the connector
    if (s == NavState.Docking || s == NavState.Unloading || s == NavState.Charging || s == NavState.WaitUranium ||
        s == NavState.FillHydrogen || s == NavState.Undocking) return 3;
    if (s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.RetryDocking) return 2;
    if (s == NavState.FlyToDockArea) return 1;
    return 0;
}

long ClaimedDockId()
{
    if (DockPhase() == 0) return 0;
    if (StationCleared(1) && stationConn != 0) return stationConn;
    return poolDock != null ? poolDockId : homeConnId;
}

// While docked: learn the pose relative to the base connector and the list of pool connectors.
void LearnDocks()
{
    if (shipMode == ShipMode.Shuttle || !homeDock.isSet) return;
    IMyShipConnector conn = FindConnector(MyShipConnectorStatus.Connected);
    if (conn == null || conn.OtherConnector == null) return;
    IMyShipConnector other = conn.OtherConnector;
    Vector3I homeConn = new Vector3I((int)homeDock.connectorGridPos.X, (int)homeDock.connectorGridPos.Y, (int)homeDock.connectorGridPos.Z);
    if (conn.Position == homeConn)
    {
        MatrixD inv = MatrixD.Transpose(other.WorldMatrix); // rotation part only is used
        dockRelFwd = Vector3D.TransformNormal(remoteControl.WorldMatrix.Forward, inv);
        dockRelDown = Vector3D.TransformNormal(remoteControl.WorldMatrix.Down, inv);
        dockRelLeft = Vector3D.TransformNormal(remoteControl.WorldMatrix.Left, inv);
        dockRelOffset = Vector3D.TransformNormal(conn.GetPosition() - other.GetPosition(), inv);
        dockRelSet = true;
    }
    if (Vector3.Distance(conn.GetPosition(), homeDock.position) < 5) homeConnId = other.EntityId;

    List<IMyShipConnector> found = new List<IMyShipConnector>();
    gridTerminalSystem.GetBlocksOfType(found, q => q.CustomName.Contains(dockPoolTag) && !q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
    if (found.Count > 0)
    {
        dockPool.Clear();
        for (int i = 0; i < found.Count; i++)
        {
            PoolDock p = new PoolDock();
            p.id = found[i].EntityId; p.pos = found[i].GetPosition(); p.fwd = found[i].WorldMatrix.Forward; p.up = found[i].WorldMatrix.Up;
            dockPool.Add(p);
        }
    }
    // which dock are we at?
    poolDock = null; poolDockId = 0;
    if (other.EntityId == homeConnId || !dockRelSet) return;
    for (int i = 0; i < dockPool.Count; i++)
        if (dockPool[i].id == other.EntityId) { poolDockId = dockPool[i].id; poolDock = DockPose(dockPool[i].pos, dockPool[i].fwd, dockPool[i].up); }
}

bool IsDockTaken(long id, int myPhase)
{
    if (id == 0) return false;
    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip t = traffic[i];
        if (t.dock == id && (t.dockPhase > myPhase || (t.dockPhase == myPhase && t.id < myGridId))) return true;
    }
    return false;
}

// Picks the dock to use: the current one while it's ours, else own home, else the nearest free
// pool connector. Returns false when every dock is taken (wait).
bool ChooseDock(int phase)
{
    if (shipMode == ShipMode.Shuttle || !trafficEnabled) { poolDock = null; poolDockId = 0; return true; }
    long current = poolDock != null ? poolDockId : homeConnId;
    if (!IsDockTaken(current, phase)) return true;
    if (!IsDockTaken(homeConnId, phase)) { poolDock = null; poolDockId = 0; return true; }
    if (!dockRelSet) return false;
    PoolDock best = null;
    float bestDist = 0;
    for (int i = 0; i < dockPool.Count; i++)
    {
        PoolDock p = dockPool[i];
        if (p.id == homeConnId || IsDockTaken(p.id, phase)) continue;
        float d = Vector3.Distance(p.pos, homeDock.position);
        if (best == null || d < bestDist) { best = p; bestDist = d; }
    }
    if (best == null) return false;
    poolDockId = best.id;
    poolDock = DockPose(best.pos, best.fwd, best.up);
    return true;
}

// Save section 6: dock pose relation, home connector id and the pool list.
String DockPoolToString()
{
    String s = (dockRelSet ? "1" : "0") + ":" + VectorToString(dockRelFwd) + ":" + VectorToString(dockRelDown) + ":" +
        VectorToString(dockRelLeft) + ":" + VectorToString(dockRelOffset) + ":" + homeConnId;
    for (int i = 0; i < dockPool.Count; i++)
        s += ":" + dockPool[i].id + "," + VectorToString(dockPool[i].pos) + "," + VectorToString(dockPool[i].fwd) + "," + VectorToString(dockPool[i].up);
    return s;
}

void DockPoolFromString(String s)
{
    dockPool.Clear();
    dockRelSet = false;
    homeConnId = 0;
    String[] f = s.Split(':');
    if (f.Length < 6) return;
    dockRelSet = f[0] == "1";
    dockRelFwd = StringToVector(f[1]); dockRelDown = StringToVector(f[2]); dockRelLeft = StringToVector(f[3]); dockRelOffset = StringToVector(f[4]);
    long.TryParse(f[5], out homeConnId);
    for (int i = 6; i < f.Length; i++)
    {
        String[] e = f[i].Split(',');
        if (e.Length < 4) continue;
        PoolDock p = new PoolDock();
        if (!long.TryParse(e[0], out p.id)) continue;
        p.pos = StringToVector(e[1]); p.fwd = StringToVector(e[2]); p.up = StringToVector(e[3]);
        dockPool.Add(p);
    }
}
