// ========================== OBSTACLE BRAKE (cameras) ==========================
// Cameras on the ship raycast towards the current flight target. If something is in the way
// the ship brakes and holds position (UpdateFlight uses maxSpeed = 0) until the way is clear.
// Active only for free flight: path flights and hole-to-hole moves. Off while mining, backing
// out of a hole and docking, where the ship is meant to be close to voxels and blocks.

List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
List<long> ownGridIds = new List<long>();
bool pathBlocked = false;
int clearScans = 0;
int cameraIndex = 0;
DateTime blockedSince = DateTime.Now;
String blockedBy = "";

void ScanCameras()
{
    cameras.Clear();
    ownGridIds.Clear();
    if (!obstacleBrake) return;
    gridTerminalSystem.GetBlocksOfType(cameras, q => q.CubeGrid.IsSameConstructAs(Me.CubeGrid) && q.IsFunctional);
    for (int i = 0; i < cameras.Count; i++)
    {
        cameras[i].EnableRaycast = true;
        if (!ownGridIds.Contains(cameras[i].CubeGrid.EntityId)) ownGridIds.Add(cameras[i].CubeGrid.EntityId);
    }
    if (!ownGridIds.Contains(Me.CubeGrid.EntityId)) ownGridIds.Add(Me.CubeGrid.EntityId);
}

bool IsFreeFlightState(NavState s)
{
    return s == NavState.FlyToXY || s == NavState.FlyToJobArea || s == NavState.FlyToDockArea ||
        s == NavState.FlyToPath || s == NavState.FlyToJobPos || s == NavState.FlyToDropPos || s == NavState.FlyToDropPos2 ||
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
    if (!obstacleBrake || cameras.Count == 0 || !IsFreeFlightState(currentNavState)) { ClearBlocked(); return; }
    Vector3 flyTarget = navTargetPos + trafficShift;
    Vector3 toTarget = flyTarget - shipPosition;
    float targetDist = toTarget.Length();
    if (targetDist < 1f) { ClearBlocked(); return; }
    Vector3 dir = toTarget / targetDist;

    // Scan up to a little past the target (obstacles behind the target don't matter),
    // and no further than the look-ahead distance for the current speed.
    float lookDist = shipSpeed * obstacleLookAhead + shipDiameter;
    Vector3 rayEnd = flyTarget + dir * (shipDiameter * 0.25f);

    for (int n = 0; n < cameras.Count; n++)
    {
        cameraIndex = (cameraIndex + 1) % cameras.Count;
        IMyCameraBlock cam = cameras[cameraIndex];
        if (!cam.IsFunctional || !cam.IsWorking) continue;
        Vector3 camPos = cam.GetPosition();
        Vector3 end = rayEnd;
        if (Vector3.Distance(camPos, end) > lookDist) end = camPos + dir * lookDist;
        if (!cam.CanScan(end)) continue; // outside this camera's cone, or still charging

        MyDetectedEntityInfo hit = cam.Raycast(end);
        // PAM ships that send traffic messages are handled by the spacing logic, not by braking
        bool obstacle = !hit.IsEmpty() && !ownGridIds.Contains(hit.EntityId) && !IsTrafficShip(hit.EntityId) &&
            hit.Type != MyDetectedEntityType.FloatingObject;
        if (obstacle)
        {
            if (!pathBlocked) blockedSince = DateTime.Now;
            pathBlocked = true;
            clearScans = 0;
            blockedBy = hit.Type.ToString();
        }
        else if (pathBlocked && ++clearScans >= 3) ClearBlocked();
        break; // one raycast per run is enough at 6 runs per second
    }

    if (pathBlocked && (DateTime.Now - blockedSince).TotalSeconds > obstacleTimeout)
        statusMessage = "Path blocked: " + blockedBy;
}
