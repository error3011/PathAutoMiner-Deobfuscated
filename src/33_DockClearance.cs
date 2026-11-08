// ========================== DOCK CLEARANCE (ship side) ==========================
// At the end of the path, and before undocking, a ship asks its station for clearance. If the
// station's controller handles the request (its home connector is on that station), the ship
// waits until it is cleared and docks at the connector the station assigns. If no station answers
// within 3 s, the ship falls back to the ship-to-ship dock pool / own home dock as before.

IMyBroadcastListener stationListener = null;
int dockRequestKind = 0;        // 0 none, 1 dock, 2 undock
DateTime dockRequestSince = TimeNow();
DateTime stationAckTime = DateTime.MinValue;
long stationCleared = 0;
int stationClearedKind = 0;
long stationConn = 0;
bool waitUndockClearance = false;
int dockRequestTick = 0;
List<Vector3> zoneCenters = new List<Vector3>(); // the station's [PAM-Zone] sensor areas: boxes, as the sensor sees
List<Vector3> zoneBox = new List<Vector3>();      // 4 per area: forward, up, extents left/bottom/back, right/top/front
bool dockQueued = false;  // holding outside the docking area for clearance (sent in traffic messages)
bool outOfService = false; // can't dock (connector damaged): parked beside the path, others pass it
bool dockHoldSet = false;
Vector3 dockHoldPos = new Vector3();
bool emergency = false; // heading home low on energy: lands while it has to wait to dock, docks first
int emgStage = 0;       // 1 coming down, 2 landed (thrusters off), 3 taking off again
int emgTicks = 0, emgStill = 0, emgFree = 0;
Vector3 emgStart = new Vector3();

bool StationHandlesMe() { return dockRequestKind != 0 && (TimeNow() - stationAckTime).TotalSeconds < 3; }
bool StationCleared(int kind) { return StationHandlesMe() && stationCleared == myGridId && stationClearedKind == kind && !badDocks.Contains(stationConn); }
// still waiting for a first answer, or the station is handling this ship
bool StationDeciding() { return StationHandlesMe() || (TimeNow() - dockRequestSince).TotalSeconds < 3; }

void RequestDock(int kind)
{
    if (dockRequestKind == kind) return;
    dockRequestKind = kind;
    dockRequestSince = TimeNow();
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
    // emergency: heading home low on energy, until it docks
    int ph = DockPhase();
    if (HeadingHome() && LowEnergy()) emergency = true;
    if (!HeadingHome() && ph != 1 && ph != 2) emergency = false;
    // stopped or sent elsewhere while landed: power up again
    if (emgStage > 0 && currentNavState != NavState.FlyToDockArea) { emgStage = 0; SetBlocksEnabled(thrusters, true); SetLandingGears(landingGears, false); }

    // finished?
    if (dockRequestKind == 1 && FindConnector(MyShipConnectorStatus.Connected) != null) ReleaseDock();
    if (dockRequestKind == 2 && !waitUndockClearance && currentNavState != NavState.Undocking &&
        Vector3.Distance(shipPosition, GetCurrentDock().position) > shipDiameter * 3) ReleaseDock();

    if (dockRequestKind != 0 && ++dockRequestTick >= 3)
    {
        dockRequestTick = 0;
        IGC.SendBroadcastMessage(DOCK_TAG, "R;" + myGridId + ";" + dockRequestKind + ";" + homeConnId + ";" + (dockRelSet ? 1 : 0) + ";" + UsablePool() +
            ";" + String.Join(",", badDocks) + ";" + (emergency ? 1 : 0));
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
    stationAckTime = TimeNow();
    // cleared ships: ship:kind:connector,... (several may be on their way to different connectors)
    stationCleared = 0; stationClearedKind = 0; stationConn = 0;
    String[] cl = f[2].Split(',');
    for (int i = 0; i < cl.Length; i++)
    {
        String[] c = cl[i].Split(':');
        if (c.Length == 3 && c[0] == myGridId.ToString()) { stationCleared = myGridId; stationClearedKind = int.Parse(c[1]); stationConn = long.Parse(c[2]); }
    }
    zoneCenters.Clear(); zoneBox.Clear();
    if (f.Length >= 8 && f[7] != "")
    {
        String[] z = f[7].Split(':');
        for (int i = 0; i < z.Length; i++)
        {
            // pos,forward,up,left,right,top,bottom,front,back (older controller: pos,radius -> a box around that sphere)
            String[] e = z[i].Split(',');
            if (e.Length >= 9) AddZone(StringToVector(e[0]), StringToVector(e[1]), StringToVector(e[2]),
                new Vector3(float.Parse(e[3]), float.Parse(e[6]), float.Parse(e[8])), new Vector3(float.Parse(e[4]), float.Parse(e[5]), float.Parse(e[7])));
            else if (e.Length >= 2) { float r = float.Parse(e[1]); AddZone(StringToVector(e[0]), Vector3.Forward, Vector3.Up, new Vector3(r), new Vector3(r)); }
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
        for (int i = 0; i < dockPool.Count; i++) if (dockPool[i].id == stationConn && PoseOk(dockPool[i])) pd = dockPool[i];
    long want = pd == null ? 0 : pd.id;
    if (want == poolDockId && (want == 0) == (poolDock == null)) return false;
    poolDockId = want;
    poolDock = pd == null ? null : DockPose(pd.pos, pd.fwd, pd.up);
    return true;
}

void AddZone(Vector3 pos, Vector3 fwd, Vector3 up, Vector3 lo, Vector3 hi) { zoneCenters.Add(pos); zoneBox.Add(fwd); zoneBox.Add(up); zoneBox.Add(lo); zoneBox.Add(hi); }

// inside a docking area's sensor box, or closer to it than about half the ship's size
bool InDockZone(Vector3 p)
{
    float m = shipDiameter * 0.6f;
    for (int i = 0; i < zoneCenters.Count; i++)
    {
        Vector3 d = p - zoneCenters[i], f = zoneBox[i * 4], u = zoneBox[i * 4 + 1], lo = zoneBox[i * 4 + 2], hi = zoneBox[i * 4 + 3];
        float x = Vector3.Dot(d, Vector3.Cross(f, u)), y = Vector3.Dot(d, u), z = Vector3.Dot(d, f);
        if (x > -lo.X - m && x < hi.X + m && y > -lo.Y - m && y < hi.Y + m && z > -lo.Z - m && z < hi.Z + m) return true;
    }
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
    bool broken = true; // also when it's gone altogether (no connector left on the ship)
    for (int i = 0; i < connectors.Count; i++) if (!connectors[i].Closed && connectors[i].IsFunctional) broken = false;
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

bool LowEnergy() { String m = statusMessage; bool low = !CheckEnergy(); statusMessage = m; return low; }

// Emergency (low on energy) while it has to wait to dock: come down slowly straight below, land and
// switch the thrusters off (no energy spent hovering) until the station clears it (an emergency
// goes first) or a dock is free. In space: stop, then thrusters off.
void EmergencyLand()
{
    emgFree = 0;
    if (emgStage == 0) { emgStage = 1; emgStart = shipPosition; emgTicks = 0; emgStill = 0; }
    if (emgStage == 1)
    {
        Vector3 g = remoteControl.GetNaturalGravity();
        bool space = g.LengthSquared() < 0.01f;
        SetFlightTarget(space ? emgStart : shipPosition + Vector3.Normalize(g) * 50, space ? 0 : 3);
        statusMessage = "Emergency: landing";
        // on the ground: stopped for 2 s (not by traffic or the cameras), or a landing gear touches
        emgTicks++;
        emgStill = shipSpeed < 0.3f && trafficSpeedLimit < 0 && !pathBlocked ? emgStill + 1 : 0;
        bool gear = false;
        for (int i = 0; i < landingGears.Count; i++) if (landingGears[i].LockMode != LandingGearMode.Unlocked) gear = true;
        if ((emgTicks < 18 || emgStill < 12) && !gear) return;
        emgStage = 2;
        SetLandingGears(landingGears, true);
        SetBlocksEnabled(thrusters, false);
    }
    StopFlight();
    StopGyroOverride();
    statusMessage = "Emergency: landed, waiting to dock";
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
