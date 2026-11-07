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
// (planet/asteroid) doesn't count between holes (the drills are on and the hole tops sit on the
// ground) or right at the flight target (recorded spots close to the ground).

List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
List<long> ownGridIds = new List<long>();
bool pathBlocked = false;
int clearScans = 0, scanStep = 0, blindRuns = 0;
const int SCAN_PATTERN = 16;
int cameraIndex = 0;
DateTime blockedSince = DateTime.Now;
String blockedBy = "";
String cameraCoverage = "";
Vector3 detourShift = new Vector3(); // docking approach: way around an obstacle
int detourStep = 0; // ship directions some camera can scan (info page)

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
    bool approach = currentNavState == NavState.ApproachDock;
    if (!approach) { detourShift = new Vector3(); detourStep = 0; }
    if (!obstacleBrake || cameras.Count == 0 || !(IsFreeFlightState(currentNavState) || approach)) { ClearBlocked(); return; }
    Vector3 flyTarget = navTargetPos + trafficShift + detourShift;
    Vector3 toTarget = flyTarget - shipPosition;
    float targetDist = toTarget.Length();
    if (targetDist < 1f) { ClearBlocked(); return; }
    Vector3 dir = toTarget / targetDist;
    // where the ship is really going in the next seconds: its velocity once it moves
    Vector3 vel = remoteControl != null ? (Vector3)remoteControl.GetShipVelocities().LinearVelocity : new Vector3();
    if (vel.Length() > 2) dir = Vector3.Normalize(vel);
    Vector3 side = Vector3.Cross(dir, Math.Abs(dir.Y) < 0.9f ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0));
    side.Normalize();
    Vector3 up = Vector3.Cross(side, dir);
    // up to a little past the target (obstacles behind it don't matter), no further than the
    // look-ahead distance for the current speed
    float dist = Math.Min(targetDist + shipDiameter * 0.25f, shipSpeed * obstacleLookAhead + shipDiameter);
    int cast = 0;
    for (int k = 0; k < 2; k++)
    {
        int p = k == 0 ? 0 : scanStep++ % 8, e = p % 4; // k 0: centre, 1: next edge point (4-7: at half the distance)
        Vector3 off = k == 0 ? new Vector3() : (e == 0 ? side : e == 1 ? -side : e == 2 ? up : -up) * shipDiameter * 0.5f;
        Vector3 end = shipPosition + dir * (p >= 4 ? dist * 0.5f : dist) + off;
        IMyCameraBlock cam = CameraFor(end);
        if (cam == null) { CountClear(); continue; } // no camera faces that point (or all charging)
        cast++;
        MyDetectedEntityInfo hit = cam.Raycast(end);
        // PAM ships that send traffic messages are handled by the spacing logic, not by braking
        bool voxel = hit.Type == MyDetectedEntityType.Planet || hit.Type == MyDetectedEntityType.Asteroid;
        bool atTarget = hit.HitPosition.HasValue && Vector3D.Distance(hit.HitPosition.Value, navTargetPos) < shipDiameter;
        bool obstacle = !hit.IsEmpty() && !ownGridIds.Contains(hit.EntityId) && !IsTrafficShip(hit.EntityId) &&
            hit.Type != MyDetectedEntityType.FloatingObject && !(approach && atTarget) &&
            !(voxel && (k > 0 || currentNavState == NavState.FlyToXY || atTarget));
        if (obstacle)
        {
            if (!pathBlocked) blockedSince = DateTime.Now;
            pathBlocked = true;
            clearScans = 0;
            blockedBy = hit.Type.ToString();
        }
        else CountClear();
        if (debugDraw && debugRays.Count < 16) debugRays.Add(VectorToString(cam.GetPosition()) + "," + VectorToString(end) + "," + (obstacle ? 1 : 0));
    }
    // no camera could look ahead for 2 s: say so (the ship keeps flying, as without cameras)
    blindRuns = cast > 0 ? 0 : blindRuns + 1;
    if (blindRuns > 12) statusMessage = "Cameras can't see ahead";

    double blockedFor = (DateTime.Now - blockedSince).TotalSeconds;
    if (approach && pathBlocked && blockedFor > 4)
    {
        if (detourStep < 4)
        {
            // try the next way around: up, right, left, down from the straight line to the connector
            detourStep++;
            detourShift = (detourStep == 1 ? up : detourStep == 2 ? side : detourStep == 3 ? -side : -up) * shipDiameter;
            blockedSince = DateTime.Now;
            ClearBlocked();
            statusMessage = "Docking: going around an obstacle";
        }
        else statusMessage = "Docking blocked: " + blockedBy + ", stopped";
        return;
    }
    if (pathBlocked && blockedFor > obstacleTimeout)
        statusMessage = "Path blocked: " + blockedBy;
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
