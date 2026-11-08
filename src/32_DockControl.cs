// ========================== DOCK CONTROL (station, [PAM-Controller] PB) ==========================
// The controller PB on the station runs the docking area: it reads the real status of the
// station's connectors (catches idle PAM ships and non-PAM ships), watches the docking area with
// [PAM-Zone] sensors and each connector's approach lane with a [PAM-Lane] camera, and clears one
// ship at a time to move in the area. Once that ship is on its last leg to its connector
// (approach, align, docking) it keeps its clearance and the next ship may come in, to another
// connector. Undocking is cleared before docking (when no ship is docking), so a full dock area
// can't jam. A station only answers ships whose home connector is on its own grid.

const String DOCK_TAG = "[PAMDCK]";
const float CLEARANCE_TIMEOUT = 120f; //[s] a cleared ship that doesn't finish loses its clearance

class DockRequest
{
    public long ship;
    public int kind; // 1 = dock, 2 = undock
    public long home;
    public bool canUsePool;
    public String usable; // ",id,id," pool connectors the ship can dock at in its normal attitude (null: older build)
    public String bad = ""; // ",id,id," connectors that didn't lock for this ship
    public bool emg;      // low on energy: docks first
    public DateTime first, last;
}

IMyBroadcastListener dockListener = null;
List<DockRequest> dockRequests = new List<DockRequest>();
List<IMyShipConnector> stationDocks = new List<IMyShipConnector>();
List<IMySensorBlock> zoneSensors = new List<IMySensorBlock>();
List<IMyCameraBlock> laneCameras = new List<IMyCameraBlock>();
List<MyDetectedEntityInfo> zoneSeen = new List<MyDetectedEntityInfo>();
long clearedShip = 0;
int clearedKind = 0;
long clearedConn = 0;
List<long> dockingShips = new List<long>(), dockingConns = new List<long>(); // cleared, on the last leg
DateTime clearedSince = TimeNow();
String dockAreaStatus = "";
int dockSendTick = 0;
IMyCubeGrid stationGrid = null;

void ScanDockArea()
{
    stationGrid = Me.CubeGrid;
    gridTerminalSystem.GetBlocksOfType(stationDocks, q => q.CubeGrid.IsSameConstructAs(stationGrid) && q.CustomName.Contains(dockPoolTag));
    gridTerminalSystem.GetBlocksOfType(zoneSensors, q => q.CubeGrid.IsSameConstructAs(stationGrid) && q.CustomName.Contains(zoneTag));
    gridTerminalSystem.GetBlocksOfType(laneCameras, q => q.CubeGrid.IsSameConstructAs(stationGrid) && q.CustomName.Contains(laneTag));
    for (int i = 0; i < zoneSensors.Count; i++) { zoneSensors[i].DetectSmallShips = true; zoneSensors[i].DetectLargeShips = true; }
    for (int i = 0; i < laneCameras.Count; i++) laneCameras[i].EnableRaycast = true;
}

IMyShipConnector StationConnector(long id)
{
    if (id == 0) return null;
    IMyShipConnector c = gridTerminalSystem.GetBlockWithId(id) as IMyShipConnector;
    return c != null && stationGrid != null && c.CubeGrid.IsSameConstructAs(stationGrid) ? c : null;
}

bool IsDockFree(IMyShipConnector c)
{
    return c != null && c.IsFunctional && c.Enabled && c.Status == MyShipConnectorStatus.Unconnected;
}

// Raycast the approach lane of a connector with the [PAM-Lane] camera next to it (within 10 m).
bool IsLaneClear(IMyShipConnector c, long ship)
{
    for (int i = 0; i < laneCameras.Count; i++)
    {
        IMyCameraBlock cam = laneCameras[i];
        if (!cam.IsWorking || Vector3D.Distance(cam.GetPosition(), c.GetPosition()) > 10) continue;
        Vector3D end = cam.GetPosition() + c.WorldMatrix.Forward * dockLaneLength;
        if (!cam.CanScan(end)) continue; // camera not facing along the lane, or charging
        MyDetectedEntityInfo hit = cam.Raycast(end);
        if (!hit.IsEmpty() && hit.EntityId != stationGrid.EntityId && hit.EntityId != ship && hit.Type != MyDetectedEntityType.FloatingObject)
        { dockAreaStatus = "Lane blocked: " + c.CustomName; return false; }
    }
    return true;
}

// Something moving in the docking area that isn't the cleared ship?
bool IsZoneBusy()
{
    for (int s = 0; s < zoneSensors.Count; s++)
    {
        if (!zoneSensors[s].IsWorking) continue;
        zoneSeen.Clear();
        zoneSensors[s].DetectedEntities(zoneSeen);
        for (int i = 0; i < zoneSeen.Count; i++)
        {
            MyDetectedEntityInfo e = zoneSeen[i];
            bool grid = e.Type == MyDetectedEntityType.SmallGrid || e.Type == MyDetectedEntityType.LargeGrid;
            if (grid && e.EntityId != clearedShip && !dockingShips.Contains(e.EntityId) && e.Velocity.Length() > 0.5f)
            { dockAreaStatus = "Ship moving in dock area: " + e.Name; return true; }
        }
    }
    return false;
}

long PickDock(DockRequest r)
{
    IMyShipConnector home = StationConnector(r.home);
    if (IsDockFree(home) && !DockTaken(home.EntityId) && !r.bad.Contains("," + home.EntityId + ",") && IsLaneClear(home, r.ship)) return home.EntityId;
    if (!r.canUsePool || home == null) return 0; // home removed: the pool poses are relative to it
    IMyShipConnector best = null;
    double bestDist = 0;
    for (int i = 0; i < stationDocks.Count; i++)
    {
        IMyShipConnector c = stationDocks[i];
        if (!IsDockFree(c) || DockTaken(c.EntityId) || (r.usable != null && !r.usable.Contains("," + c.EntityId + ",")) || r.bad.Contains("," + c.EntityId + ",")) continue;
        double d = Vector3D.Distance(c.GetPosition(), home.GetPosition());
        if (best == null || d < bestDist) { if (IsLaneClear(c, r.ship)) { best = c; bestDist = d; } }
    }
    return best == null ? 0 : best.EntityId;
}

void GrantNext()
{
    DockRequest pick = null;
    long conn = 0;
    // pass 3: emergency (low on energy) docking first, 2: undocking (frees a connector), 1: docking
    for (int pass = 3; pass >= 1 && pick == null; pass--)
    {
        DockRequest oldest = null;
        for (int i = 0; i < dockRequests.Count; i++)
        {
            DockRequest r = dockRequests[i];
            if (r.kind != (pass == 2 ? 2 : 1) || (pass == 3 && !r.emg) || dockingShips.Contains(r.ship) || (pass == 2 && dockingShips.Count > 0)) continue; // undock once nobody is docking
            if (pass != 2)
            {
                long c = PickDock(r);
                if (c == 0) continue;
                if (oldest == null || r.first < oldest.first) { oldest = r; conn = c; }
            }
            else if (oldest == null || r.first < oldest.first) oldest = r;
        }
        pick = oldest;
    }
    if (pick == null) { if (dockRequests.Count > 0 && dockAreaStatus == "") dockAreaStatus = "No free dock"; return; }
    clearedShip = pick.ship;
    clearedKind = pick.kind;
    clearedConn = pick.kind == 1 ? conn : 0;
    clearedSince = TimeNow();
}

void UpdateDockControl()
{
    if (stationGrid == null) return; // first block scan not done yet
    if (dockListener == null) dockListener = IGC.RegisterBroadcastListener(DOCK_TAG);
    int budget = 30;
    while (dockListener.HasPendingMessage && budget-- > 0)
    {
        String data = dockListener.AcceptMessage().Data as String;
        if (data != null) HandleDockMessage(data);
    }
    RunDockControl();
    if (++dockSendTick < 3) return; // ~2 per second
    dockSendTick = 0;
    IGC.SendBroadcastMessage(DOCK_TAG, BuildStationMessage());
}

void HandleDockMessage(String data)
{
    try
    {
        String[] f = data.Split(';');
        long ship = long.Parse(f[1]);
        if (f[0] == "D")
        {
            for (int i = dockRequests.Count - 1; i >= 0; i--) if (dockRequests[i].ship == ship) dockRequests.RemoveAt(i);
            if (clearedShip == ship) clearedShip = 0;
            int d = dockingShips.IndexOf(ship);
            if (d >= 0) { dockingShips.RemoveAt(d); dockingConns.RemoveAt(d); }
        }
        else if (f[0] == "R" && f.Length >= 5)
        {
            long home = long.Parse(f[3]);
            if (StationConnector(home) == null) return; // that ship's home is not on this station
            DockRequest r = null;
            for (int i = 0; i < dockRequests.Count; i++) if (dockRequests[i].ship == ship) r = dockRequests[i];
            if (r == null) { r = new DockRequest(); r.ship = ship; r.first = TimeNow(); dockRequests.Add(r); }
            int kind = int.Parse(f[2]);
            if (r.kind != kind) r.first = TimeNow();
            r.kind = kind; r.home = home; r.canUsePool = f[4] == "1"; r.last = TimeNow();
            r.usable = f.Length >= 6 ? "," + f[5] + "," : null;
            r.bad = f.Length >= 7 ? "," + f[6] + "," : "";
            r.emg = f.Length >= 8 && f[7] == "1";
            // its connector didn't lock: drop that clearance, the request (and its place in the queue) stays
            int d = dockingShips.IndexOf(r.ship);
            if (d >= 0 && r.bad.Contains("," + dockingConns[d] + ",")) { dockingShips.RemoveAt(d); dockingConns.RemoveAt(d); }
            if (clearedShip == r.ship && r.bad.Contains("," + clearedConn + ",")) clearedShip = 0;
        }
    }
    catch { }
}

void RunDockControl()
{
    for (int i = dockRequests.Count - 1; i >= 0; i--)
        if ((TimeNow() - dockRequests[i].last).TotalSeconds > 3) dockRequests.RemoveAt(i);
    DockRequest cleared = null;
    for (int i = 0; i < dockRequests.Count; i++) if (dockRequests[i].ship == clearedShip) cleared = dockRequests[i];
    if (clearedShip != 0 && cleared == null) clearedShip = 0;
    if (cleared != null && (TimeNow() - clearedSince).TotalSeconds > CLEARANCE_TIMEOUT)
    { clearedShip = 0; cleared.first = TimeNow(); } // too slow: back of the queue
    // ships on their last leg that stopped asking (docked, or gave up): done
    for (int i = dockingShips.Count - 1; i >= 0; i--)
    {
        bool asking = false;
        for (int k = 0; k < dockRequests.Count; k++) if (dockRequests[k].ship == dockingShips[i]) asking = true;
        if (!asking) { dockingShips.RemoveAt(i); dockingConns.RemoveAt(i); }
    }
    // the cleared ship is on its last leg to its connector: it keeps the clearance, the next may come
    if (clearedShip != 0 && clearedKind == 1)
        for (int i = 0; i < tcShips.Count; i++)
            if (tcShips[i].id == clearedShip && tcShips[i].phase >= 2) { dockingShips.Add(clearedShip); dockingConns.Add(clearedConn); clearedShip = 0; break; }
    dockAreaStatus = "";
    if (clearedShip == 0 && dockRequests.Count > 0 && !IsZoneBusy()) GrantNext();
}

String BuildStationMessage()
{
    // cleared ships: ship:kind:connector,...
    String msg = "S;" + stationGrid.EntityId + ";" + (clearedShip != 0 ? clearedShip + ":" + clearedKind + ":" + clearedConn : "");
    for (int i = 0; i < dockingShips.Count; i++) msg += (msg.EndsWith(";") ? "" : ",") + dockingShips[i] + ":1:" + dockingConns[i];
    msg += ";;;";
    for (int i = 0; i < dockRequests.Count; i++) msg += (i > 0 ? "," : "") + dockRequests[i].ship;
    msg += ";";
    for (int i = 0; i < stationDocks.Count; i++)
    {
        IMyShipConnector c = stationDocks[i];
        msg += (i > 0 ? ":" : "") + c.EntityId + "," + VectorToString(c.GetPosition()) + "," + VectorToString(c.WorldMatrix.Forward) + "," + VectorToString(c.WorldMatrix.Up);
    }
    // docking area: each [PAM-Zone] sensor's detection box (position, orientation, extents); ships wait outside it
    msg += ";";
    for (int i = 0; i < zoneSensors.Count; i++)
    {
        IMySensorBlock z = zoneSensors[i];
        msg += (i > 0 ? ":" : "") + VectorToString(z.GetPosition()) + "," + VectorToString(z.WorldMatrix.Forward) + "," + VectorToString(z.WorldMatrix.Up) + "," +
            z.LeftExtend + "," + z.RightExtend + "," + z.TopExtend + "," + z.BottomExtend + "," + z.FrontExtend + "," + z.BackExtend;
    }
    return msg;
}

String GetDockAreaLine()
{
    if (stationDocks.Count == 0 && zoneSensors.Count == 0) return "";
    int free = 0;
    for (int i = 0; i < stationDocks.Count; i++) if (IsDockFree(stationDocks[i])) free++;
    return "Docks: " + free + "/" + stationDocks.Count + " free" + (clearedShip != 0 ? (clearedKind == 1 ? " | coming in" : " | undocking") : "") +
        (dockingShips.Count > 0 ? " | docking: " + dockingShips.Count : "") +
        (dockRequests.Count > 1 ? " | " + (dockRequests.Count - 1) + " waiting" : "") + (dockRequests.Exists(q => q.emg) ? " | EMERGENCY" : "") + (dockAreaStatus != "" ? "\n" + dockAreaStatus : "") + "\n";
}

// connector already given to a cleared ship that hasn't connected yet
bool DockTaken(long conn) { return (clearedShip != 0 && clearedConn == conn) || dockingConns.Contains(conn); }
