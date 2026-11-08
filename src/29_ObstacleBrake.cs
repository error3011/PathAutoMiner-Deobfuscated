// ========================== OBSTACLE BRAKE (cameras) ==========================
// Cameras on the ship raycast along the flight direction. If something is in the way the ship
// brakes and holds position (UpdateFlight uses maxSpeed = 0) until the way is clear.
// The scan is a sweep, not a single line: every run one ray along the centre of the flight line
// and one to the next of 8 edge points (the four edges of the ship, its size, at the scan
// distance and at half of it), each from a camera that faces that point and has charge. A full
// sweep (8 runs, 16 rays) without a hit clears the hold.
// The edge rays look for blocks only: terrain is the centre ray's job (paths may run low).
// The sweep follows where the ship is actually going (its velocity, once it moves), and each ray
// is cast by the camera that faces that point best (with charge left).
// Also on the way from the path end to the connector (ApproachDock), where anything right at the
// approach point (the station around the connector) doesn't count. Blocked there for 4 s, the
// ship tries to go around: one ship size up, right, left, down from the straight line, one after
// the other; if none works it stays where it is ("Docking blocked").
// Active only for free flight: path flights and hole-to-hole moves. Off while mining, backing
// out of a hole and docking, where the ship is meant to be close to voxels and blocks. Terrain
// (planet/asteroid) doesn't count between holes (the hole tops sit on the
// ground) or right at the flight target (recorded spots close to the ground).

List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
List<long> ownGridIds = new List<long>();
bool pathBlocked = false;
int clearScans = 0, scanStep = 0, blindRuns = 0;
const int SCAN_PATTERN = 16;
int cameraIndex = 0;
DateTime blockedSince = TimeNow();
String blockedBy = "";
String cameraCoverage = "";
Vector3 detourShift = new Vector3(); // docking approach: way around an obstacle
Vector3 laneEntry; bool toLane, laneTurn; // straight connector lane: entry point, flying to it, turning round there
int detourStep = 0;
Vector3 gridHalf = new Vector3(); // half size of the ship's grid box (grid axes)

void ScanCameras()
{
    cameras.Clear();
    ownGridIds.Clear();
    if (!obstacleBrake) return;
    gridTerminalSystem.GetBlocksOfType(cameras, q => q.CubeGrid.IsSameConstructAs(Me.CubeGrid) && q.IsFunctional);
    for (int i = 0; i < cameras.Count; i++) cameras[i].EnableRaycast = true;
    // every grid of this ship (rotors, pistons, hinges): a ray hitting its own drill head isn't an obstacle
    List<IMyTerminalBlock> all = new List<IMyTerminalBlock>();
    gridTerminalSystem.GetBlocksOfType(all, q => q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
    for (int i = 0; i < all.Count; i++) if (!ownGridIds.Contains(all[i].CubeGrid.EntityId)) ownGridIds.Add(all[i].CubeGrid.EntityId);
    if (!ownGridIds.Contains(Me.CubeGrid.EntityId)) ownGridIds.Add(Me.CubeGrid.EntityId);
    // which ship directions are covered (a camera scans up to 45 deg from where it faces)
    cameraCoverage = "";
    if (remoteControl == null) return;
    MatrixD m = remoteControl.WorldMatrix;
    Vector3[] dirs = { m.Forward, m.Backward, m.Left, m.Right, m.Up, m.Down };
    String[] names = { "Fwd", "Back", "Left", "Right", "Up", "Down" };
    for (int d = 0; d < 6; d++)
        for (int i = 0; i < cameras.Count; i++)
            if (Vector3.Dot(cameras[i].WorldMatrix.Forward, dirs[d]) > 0.7f) { cameraCoverage += names[d] + " "; break; }
}

bool IsFreeFlightState(NavState s)
{
    return s == NavState.FlyToXY || s == NavState.FlyToJobArea || s == NavState.FlyToDockArea ||
        s == NavState.FlyToPath || s == NavState.FlyToJobPos || s == NavState.FlyToDropPos2 ||
        (s == NavState.ApproachDock && poolDock != null); // straight leg from the path end to a pool connector
}

void ClearBlocked()
{
    pathBlocked = false;
    clearScans = 0;
    blockedBy = "";
}

void UpdateObstacleCheck()
{
    NavState ns = currentNavState;
    bool approach = ns == NavState.ApproachDock || ns == NavState.RetryDocking;
    // also backing out of a hole and undocking (blocks only: the hole walls / ground don't count)
    bool backing = ns == NavState.Returning || ns == NavState.ReturnToDock || ns == NavState.FlyToDropPos || ns == NavState.Undocking || emgStage == 1; // emergency landing: the ground is the goal
    if (!approach) { detourShift = new Vector3(); detourStep = 0; }
    if (!obstacleBrake || cameras.Count == 0 || !(IsFreeFlightState(ns) || approach || backing)) { ClearBlocked(); return; }
    Vector3 flyTarget = navTargetPos + trafficShift + detourShift;
    Vector3 toTarget = flyTarget - shipPosition;
    float targetDist = toTarget.Length();
    if (targetDist < 1f) { ClearBlocked(); return; }
    Vector3 dir = toTarget / targetDist;
    // where the ship is really going in the next seconds: its velocity once it moves
    Vector3 vel = remoteControl != null ? (Vector3)remoteControl.GetShipVelocities().LinearVelocity : new Vector3();
    if (vel.Length() > 2) dir = Vector3.Normalize(vel);
    // real up (from gravity) for the ways over / under an obstacle; in space any perpendicular
    Vector3 g = remoteControl != null ? -(Vector3)remoteControl.GetNaturalGravity() : new Vector3();
    if (g.LengthSquared() < 0.01f) g = new Vector3(0, 1, 0);
    Vector3 side = Vector3.Cross(dir, g);
    if (side.LengthSquared() < 0.01f * g.LengthSquared()) side = Vector3.Cross(dir, leftDirection);
    side.Normalize();
    Vector3 up = Vector3.Cross(side, dir);
    // up to a little past the target (obstacles behind it don't matter), no further than the
    // look-ahead distance for the current speed
    float dist = Math.Min(targetDist + shipDiameter * 0.25f, shipSpeed * obstacleLookAhead + shipDiameter);
    int cast = 0;
    for (int k = 0; k < 2; k++)
    {
        int p = k == 0 ? 0 : scanStep++ % 8, e = p % 4; // k 0: centre, 1: next edge point (4-7: at half the distance)
        Vector3 off = k == 0 ? new Vector3() : (e == 0 ? side : e == 1 ? -side : e == 2 ? up : -up) * HalfSize(e < 2 ? side : up);
        Vector3 end = shipPosition + dir * (p >= 4 ? dist * 0.5f : dist) + off;
        IMyCameraBlock cam = CameraFor(end);
        if (cam == null) { CountClear(); continue; } // no camera faces that point (or all charging)
        cast++;
        MyDetectedEntityInfo hit = cam.Raycast(end);
        // PAM ships that send traffic messages are handled by the spacing logic, not by braking
        bool voxel = hit.Type == MyDetectedEntityType.Planet || hit.Type == MyDetectedEntityType.Asteroid;
        bool atTarget = hit.HitPosition.HasValue && Vector3D.Distance(hit.HitPosition.Value, navTargetPos) < shipDiameter;
        // PAM ships are left to the traffic logic, unless one is right in front (centre ray, closer
        // than one ship size): then brake anyway, whatever the traffic rules say
        bool closePam = k == 0 && hit.HitPosition.HasValue && Vector3D.Distance(hit.HitPosition.Value, shipPosition) < shipDiameter;
        // a PAM ship parked at a connector is left out by the traffic logic: an obstacle here (not when
        // backing out of / flying straight into the own connector, right beside the ships docked next to it)
        bool obstacle = !hit.IsEmpty() && !ownGridIds.Contains(hit.EntityId) &&
            (!IsTrafficShip(hit.EntityId) || closePam || DockedPam(hit.EntityId) && !backing && !(approach && !toLane)) &&
            hit.Type != MyDetectedEntityType.FloatingObject && !(approach && atTarget) &&
            !(voxel && (k > 0 || backing || ns == NavState.FlyToXY || atTarget));
        if (obstacle)
        {
            if (!pathBlocked) blockedSince = TimeNow();
            pathBlocked = true;
            clearScans = 0;
            blockedBy = hit.Type.ToString();
        }
        else CountClear();
        if (debugDraw && debugRays.Count < 16) debugRays.Add(VectorToString(cam.GetPosition()) + "," + VectorToString(end) + "," + (obstacle ? 1 : 0));
    }
    // no camera could look ahead for 2 s: say so (the ship keeps flying, as without cameras)
    blindRuns = cast > 0 || backing ? 0 : blindRuns + 1; // backing out: usually no camera that way, fine
    if (blindRuns > 12) statusMessage = "Cameras can't see ahead";

    double blockedFor = (TimeNow() - blockedSince).TotalSeconds;
    if (approach && pathBlocked && blockedFor > 4)
    {
        if (detourStep < 4)
        {
            // try the next way around: up, right, left, down from the straight line to the connector
            detourStep++;
            detourShift = (detourStep == 1 ? up : detourStep == 2 ? side : detourStep == 3 ? -side : -up) * shipDiameter;
            blockedSince = TimeNow();
            ClearBlocked();
            statusMessage = "Docking: going around an obstacle";
        }
        else statusMessage = "Docking blocked: " + blockedBy + ", stopped";
        return;
    }
    if (pathBlocked && blockedFor > obstacleTimeout)
        statusMessage = "Path blocked: " + blockedBy;
}

// half the ship's size along u (its grid box): the edge rays at its real edges, not the bounding
// sphere's (much wider than a long ship: false blocks in tight places)
float HalfSize(Vector3 u)
{
    if (gridHalf.X <= 0) return shipDiameter * 0.5f;
    MatrixD m = remoteControl.CubeGrid.WorldMatrix;
    return Math.Abs(Vector3.Dot(u, m.Right)) * gridHalf.X + Math.Abs(Vector3.Dot(u, m.Up)) * gridHalf.Y + Math.Abs(Vector3.Dot(u, m.Backward)) * gridHalf.Z;
}

void CountClear() { if (pathBlocked && ++clearScans >= SCAN_PATTERN) ClearBlocked(); }

// the camera that faces this point best and can raycast to it now (inside its cone, charged);
// equally good cameras take turns, so they share the charge
IMyCameraBlock CameraFor(Vector3 end)
{
    IMyCameraBlock best = null;
    float bestScore = -2;
    for (int n = 0; n < cameras.Count; n++)
    {
        cameraIndex = (cameraIndex + 1) % cameras.Count;
        IMyCameraBlock c = cameras[cameraIndex];
        if (!c.IsFunctional || !c.IsWorking) continue;
        float score = Vector3.Dot(Vector3.Normalize(end - (Vector3)c.GetPosition()), c.WorldMatrix.Forward);
        if (score > bestScore + 0.05f && c.CanScan(end)) { best = c; bestScore = score; }
    }
    return best;
}
