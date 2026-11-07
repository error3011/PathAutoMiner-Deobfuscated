// ========================== DOCK CLEARANCE (ship side) ==========================
// At the end of the path, and before undocking, a ship asks its station for clearance. If the
// station's controller handles the request (its home connector is on that station), the ship
// waits until it is cleared and docks at the connector the station assigns. If no station answers
// within 3 s, the ship falls back to the ship-to-ship dock pool / own home dock as before.

IMyBroadcastListener stationListener = null;
int dockRequestKind = 0;        // 0 none, 1 dock, 2 undock
DateTime dockRequestSince = DateTime.Now;
DateTime stationAckTime = DateTime.MinValue;
long stationCleared = 0;
int stationClearedKind = 0;
long stationConn = 0;
bool waitUndockClearance = false;
int dockRequestTick = 0;
List<Vector3> zoneCenters = new List<Vector3>(); // the station's [PAM-Zone] sensor areas (bounding spheres)
List<float> zoneRadii = new List<float>();
bool dockQueued = false;  // holding outside the docking area for clearance (sent in traffic messages)
bool outOfService = false; // can't dock (connector damaged): parked beside the path, others pass it
bool dockHoldSet = false;
Vector3 dockHoldPos = new Vector3();

bool StationHandlesMe() { return dockRequestKind != 0 && (DateTime.Now - stationAckTime).TotalSeconds < 3; }
bool StationCleared(int kind) { return StationHandlesMe() && stationCleared == myGridId && stationClearedKind == kind; }
// still waiting for a first answer, or the station is handling this ship
bool StationDeciding() { return StationHandlesMe() || (DateTime.Now - dockRequestSince).TotalSeconds < 3; }

void RequestDock(int kind)
{
    if (dockRequestKind == kind) return;
    dockRequestKind = kind;
    dockRequestSince = DateTime.Now;
    dockRequestTick = 3; // send right away
}

void ReleaseDock()
{
    if (dockRequestKind != 0 && trafficEnabled) IGC.SendBroadcastMessage(DOCK_TAG, "D;" + myGridId);
    dockRequestKind = 0;
    waitUndockClearance = false;
}

void UpdateDockClearance()
{
    if (!trafficEnabled || shipMode == ShipMode.Shuttle) return;
    if (stationListener == null) stationListener = IGC.RegisterBroadcastListener(DOCK_TAG);
    int budget = 20;
    while (stationListener.HasPendingMessage && budget-- > 0)
    {
        String data = stationListener.AcceptMessage().Data as String;
        if (data == null || !data.StartsWith("S;")) continue;
        try { ParseStation(data); } catch { }
    }

    if (currentNavState != NavState.FlyToDockArea) { dockQueued = false; dockHoldSet = false; }

    // finished?
    if (dockRequestKind == 1 && FindConnector(MyShipConnectorStatus.Connected) != null) ReleaseDock();
    if (dockRequestKind == 2 && !waitUndockClearance && currentNavState != NavState.Undocking &&
        Vector3.Distance(shipPosition, GetCurrentDock().position) > shipDiameter * 3) ReleaseDock();

    if (dockRequestKind != 0 && ++dockRequestTick >= 3)
    {
        dockRequestTick = 0;
        IGC.SendBroadcastMessage(DOCK_TAG, "R;" + myGridId + ";" + dockRequestKind + ";" + homeConnId + ";" + (dockRelSet ? 1 : 0));
    }
}

void ParseStation(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 7) return;
    bool forMe = false;
    String[] acks = f[5].Split(',');
    for (int i = 0; i < acks.Length; i++) if (acks[i] == myGridId.ToString()) forMe = true;
    if (!forMe) return;
    stationAckTime = DateTime.Now;
    // cleared ships: ship:kind:connector,... (several may be on their way to different connectors)
    stationCleared = 0; stationClearedKind = 0; stationConn = 0;
    String[] cl = f[2].Split(',');
    for (int i = 0; i < cl.Length; i++)
    {
        String[] c = cl[i].Split(':');
        if (c.Length == 3 && c[0] == myGridId.ToString()) { stationCleared = myGridId; stationClearedKind = int.Parse(c[1]); stationConn = long.Parse(c[2]); }
    }
    zoneCenters.Clear(); zoneRadii.Clear();
    if (f.Length >= 8 && f[7] != "")
    {
        String[] z = f[7].Split(':');
        for (int i = 0; i < z.Length; i++)
        {
            String[] e = z[i].Split(',');
            if (e.Length < 2) continue;
            zoneCenters.Add(StringToVector(e[0])); zoneRadii.Add(float.Parse(e[1]));
        }
    }
    // the station's own list of pool connectors: lets ships use the pool before docking there once
    if (f[6] == "") return;
    String[] docks = f[6].Split(':');
    List<PoolDock> list = new List<PoolDock>();
    for (int i = 0; i < docks.Length; i++)
    {
        String[] e = docks[i].Split(',');
        if (e.Length < 4) continue;
        PoolDock p = new PoolDock();
        p.id = long.Parse(e[0]); p.pos = StringToVector(e[1]); p.fwd = StringToVector(e[2]); p.up = StringToVector(e[3]);
        list.Add(p);
    }
    dockPool = list;
}

// Called at the end of the path. Returns true when the ship may continue to ApproachDock.
bool CheckDockClearance()
{
    if (!trafficEnabled || shipMode == ShipMode.Shuttle || homeConnId == 0) return true;
    RequestDock(1);
    if (!StationDeciding()) return true; // no station: ship-to-ship rules
    if (!StationCleared(1)) { statusMessage = "Waiting for dock clearance"; return false; }
    ApplyStationDock();
    return true;
}

// Cleared: dock where the station says. Returns true when that changed the dock.
bool ApplyStationDock()
{
    dockWaiting = false;
    PoolDock pd = null;
    if (stationConn != 0 && stationConn != homeConnId && dockRelSet)
        for (int i = 0; i < dockPool.Count; i++) if (dockPool[i].id == stationConn) pd = dockPool[i];
    long want = pd == null ? 0 : pd.id;
    if (want == poolDockId && (want == 0) == (poolDock == null)) return false;
    poolDockId = want;
    poolDock = pd == null ? null : DockPose(pd.pos, pd.fwd, pd.up);
    return true;
}

bool InDockZone(Vector3 p)
{
    for (int i = 0; i < zoneCenters.Count; i++) if (Vector3.Distance(p, zoneCenters[i]) < zoneRadii[i] + shipDiameter * 0.75f) return true;
    return false;
}

// FlyToDockArea, every tick: ask for clearance when close to the home dock. Returns true while the
// ship has to wait outside the docking area (the waypoint it flies to, or the next one, is inside).
bool HoldForDockArea(Vector3 cur, Vector3 next)
{
    dockQueued = false;
    outOfService = false;
    if (!trafficEnabled || shipMode == ShipMode.Shuttle || homeConnId == 0) return false;
    // no working connector: can't dock. Don't queue or enter the docking area: stop near home, beside
    // the path (the others pass it), and say so
    bool broken = connectors.Count > 0;
    for (int i = 0; i < connectors.Count; i++) if (IsBlockValid(connectors[i], true) && connectors[i].IsFunctional) broken = false;
    if (broken && Vector3.Distance(shipPosition, homeDock.position) < dockRequestDistance)
    {
        if (dockRequestKind != 0) ReleaseDock();
        if (!dockHoldSet) { dockHoldSet = true; dockHoldPos = shipPosition - leftDirection * shipDiameter * 1.5f; } // move off the lane, to the right
        outOfService = true;
        statusMessage = "Connector damaged: can't dock";
        return true;
    }
    if (dockRequestKind != 1 && Vector3.Distance(shipPosition, homeDock.position) > dockRequestDistance) return false;
    RequestDock(1);
    if (!StationHandlesMe() || StationCleared(1) || zoneCenters.Count == 0) return false;
    if (!InDockZone(cur) && !InDockZone(next) && !InDockZone(shipPosition)) return false;
    dockQueued = true;
    return true;
}

// Called before undocking. Returns true when the ship may undock now.
bool CheckUndockClearance()
{
    if (!trafficEnabled || shipMode == ShipMode.Shuttle || homeConnId == 0) return true;
    RequestDock(2);
    if (!StationDeciding()) return true;
    if (!StationCleared(2)) { statusMessage = "Waiting for undock clearance"; return false; }
    return true;
}
