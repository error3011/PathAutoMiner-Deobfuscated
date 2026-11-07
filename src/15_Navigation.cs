// ========================== HOLE SPIRAL PATTERN ==========================

SpiralResult GetNextHolePosition(int holeNumber, bool reset)
{
    if (reset) { spiralState = null; holeCol = 0; holeRow = 0; }

    if (startPosition == StartPosition.TopLeft)
    {
        int next = holeNumber + 1;
        holeRow = (int)Math.Floor(SafeDiv(holeNumber, jobWidth));
        if (holeRow % 2 == 0) holeCol = holeNumber - (holeRow * jobWidth);
        else holeCol = jobWidth - 1 - (holeNumber - (holeRow * jobWidth));
        if (holeRow >= jobHeight) return SpiralResult.AllDone;
        else return SpiralResult.Found;
    }
    else if (startPosition == StartPosition.Center)
    {
        if (spiralState == null) spiralState = new int[] { 0, -1, 0, 0 };
        int halfW = (int)Math.Ceiling(jobWidth / 2f);
        int halfH = (int)Math.Ceiling(jobHeight / 2f);
        int halfW2 = (int)Math.Floor(jobWidth / 2f);
        int halfH2 = (int)Math.Floor(jobHeight / 2f);
        int iterations = 0;
        while (spiralState[2] < Math.Pow(Math.Max(jobWidth, jobHeight), 2))
        {
            if (iterations > 200) return SpiralResult.Error;
            iterations++;
            spiralState[2]++;
            if (-halfW < holeCol && holeCol <= halfW2 && -halfH < holeRow && holeRow <= halfH2)
            {
                if (spiralState[3] == holeNumber)
                {
                    this.holeCol = holeCol - 1 + halfW;
                    this.holeRow = holeRow - 1 + halfH;
                    return SpiralResult.Found;
                }
                spiralState[3]++;
            }
            if (holeCol == holeRow || (holeCol < 0 && holeCol == -holeRow) || (holeCol > 0 && holeCol == 1 - holeRow))
            {
                int temp = spiralState[0];
                spiralState[0] = -spiralState[1];
                spiralState[1] = temp;
            }
            holeCol += spiralState[0];
            holeRow += spiralState[1];
        }
    }
    return SpiralResult.AllDone;
}


// ========================== NAVIGATION STATE MACHINE ==========================
// (UpdateNavigation is the core state machine - extremely large)
// See the original logic flow for all NavState transitions.
// The full method has been preserved with readable variable names.

void UpdateNavigation()
{
    // Due to extreme length (500+ lines), this method preserves the original
    // state machine logic. Each NavState case handles transitions.
    // The full implementation follows the same flow as the obfuscated version
    // with meaningful variable names applied throughout.

    // [FlyToXY] -> determine next hole, fly to surface position
    if (currentNavState == NavState.FlyToXY)
    {
        if (stateFirstTick)
        {
            stuckCounter = 0;
            if (lastHoleIndex != currentHoleIndex) maxMineDepth = 0;
            lastHoleIndex = currentHoleIndex;
        }
        if (stuckCounter == 0)
        {
            // co-op: take a row no other ship on this job is on (a single ship keeps the original order)
            int claimed = ClaimHole(currentHoleIndex);
            if (claimed < 0)
            {
                if (!CheckEnergy()) { SetNavState(NavState.ReturnToDock); return; }
                // -1: short wait for the controller's answer, where it is. -2: every hole left is taken:
                // wait beside the job area, backed off from the surface, out of the other ships' way
                if (!coopWaiting || (claimed == -2) != coopWaitAside)
                {
                    coopWaiting = true; coopWaitAside = claimed == -2;
                    // waiting spot: on a ring 2.5 ship sizes above the job centre, above everyone working
                    // or passing (always clear of terrain), each ship on its own place of the ring
                    double a = (myGridId % 6) * Math.PI / 3;
                    coopWaitPos = !coopWaitAside ? shipPosition : jobPosition.position - miningForward * shipDiameter * 2.5f +
                        (jobPosition.leftDir * (float)Math.Cos(a) + miningDown * (float)Math.Sin(a)) * shipDiameter * 1.2f;
                }
                SetFlightTarget(coopWaitPos, 10);
                statusMessage = coopWaitAside ? "Waiting for a free hole" : "Waiting for the controller";
                return;
            }
            if (coopWaiting) statusMessage = ""; // got a hole: drop "Waiting for ..."
            coopWaiting = false;
            if (claimed != currentHoleIndex) { currentHoleIndex = claimed; maxMineDepth = 0; lastHoleIndex = currentHoleIndex; }
            SpiralResult result = SetHolePosition(currentHoleIndex);
            if (result == SpiralResult.AllDone)
            {
                jobState = JobState.Done;
                statusMessage = "Job done";
                if (whenDoneReturnHome && homeDock.isSet) { SetNavState(NavState.FlyToPath); SetStopAtState(NavState.Unloading); lastJobState = JobState.MoveHome; }
                else { SetNavState(NavState.FlyToJobPos); SetStopAtState(NavState.AlignJob); lastJobState = JobState.MoveToJob; }
                return;
            }
            if (result == SpiralResult.Found)
            {
                stuckCounter = 1;
                SetBlocksEnabled(workTools, true);
                miningTargetPos = GetJobGridPosition(holeCol, holeRow, true);
                // over other holes (not the next one; also when arriving from the path or the waiting
                // spot): first over the hole, at least one ship size above the job surface, then straight
                // down to it
                float h = Vector3.Dot(shipPosition - miningTargetPos, miningForward);
                if ((shipPosition - miningTargetPos - miningForward * h).Length() > Math.Max(JTW(), JTH()) * 1.5f) h = Math.Min(h, -shipDiameter);
                SetFlightTarget(h < -1 ? miningTargetPos + miningForward * h : miningTargetPos, 10);
                AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, false);
            }
        }
        else
        {
            // co-op: a ship with a lower id claimed this hole (or one next to it) at the same moment -> pick again
            if (myHole >= 0 && (JobCoordinated() ? (coordCell >= 0 && coordCell != myHole) || coordOthers.Contains(myHole) : HoleConflict(myHole, true))) { stuckCounter = 0; return; }
            if (distToTarget < wpReachedDist)
            {
                if (Vector3.Distance(navTargetPos, miningTargetPos) > 1) { SetFlightTarget(miningTargetPos, 10); return; } // over the hole: down
                SetNavState(NavState.Mining);
                return;
            }
            // no progress for 5 s (not held by traffic or the cameras): over the hole, the ground is in
            // the way down (terrain above the job surface) -> start the hole here (drills are on);
            // elsewhere a hill is in the way -> go over it, one ship size higher, then down
            if (shipSpeed < 0.3f && trafficSpeedLimit < 0 && !pathBlocked)
            {
                if (++stuckCounter < 30) return;
                stuckCounter = 1;
                Vector3 off = miningTargetPos - shipPosition;
                float along = Vector3.Dot(off, miningForward);
                if (along > 0 && (off - miningForward * along).Length() < wpReachedDist) { cachedGridPos = shipPosition; SetNavState(NavState.Mining); }
                else SetFlightTarget(miningTargetPos + miningForward * (Math.Min(0, -along) - shipDiameter), 10);
                return;
            }
            stuckCounter = 1;
        }
    }

    // [Mining] -> drill downward
    if (currentNavState == NavState.Mining)
    {
        if (stateFirstTick)
        {
            SetBlocksEnabled(workTools, true);
            SetSortersEnabled(false);
            miningTargetPos = GetJobGridPosition(holeCol, holeRow, false);
            SetFlightTarget(GetMiningDepthPosition(miningTargetPos, 0), 0);
            AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, false);
            stuckCounter = 1;
            lastDistToTarget = 0;
            stuckSensitivity = 0;
            autoDepthRef = 0;
            lastOreAmount = -1;
            runtimeMaxDepth = configDepth;
            isFirstDescent = true;
        }

        if (!CheckEnergy()) { SetNavState(NavState.ReturnToDock); return; }
        if (IsShipFull(true))
        {
            oreCountBeforeEject = CountItems("", "ORE", ItemLocation.All);
            if ((ejectionMode == EjectionMode.DropPosStone || ejectionMode == EjectionMode.DropPosStoIce ||
                 ejectionMode == EjectionMode.InMotionStone || ejectionMode == EjectionMode.InMotionStoIce) && shipMode != ShipMode.Grinder)
                SetNavState(NavState.FlyToDropPos);
            else if ((ejectionMode == EjectionMode.CurPosStone || ejectionMode == EjectionMode.CurPosStoIce) && shipMode != ShipMode.Grinder)
                SetNavState(NavState.WaitEjection);
            else
                SetNavState(NavState.ReturnToDock);
            return;
        }

        currentMineDepth = Vector3.Distance(shipPosition, miningTargetPos);
        if (currentMineDepth > maxMineDepth) { maxMineDepth = currentMineDepth; isFirstDescent = false; }

        if (shipMode == ShipMode.Grinder && GetDetectedEntityType() == MyDetectedEntityType.SmallGrid)
            stuckSensitivity += 2;
        else stuckSensitivity -= 2;
        stuckSensitivity = Math.Max(100, Math.Min(400, stuckSensitivity));

        if (stuckCounter > 0 && stuckCounter < stuckSensitivity)
        {
            if (currentMineDepth > lastDistToTarget) { if (stuckSensitivity > 150) lastDistToTarget = currentMineDepth; else lastDistToTarget = (float)Math.Ceiling(currentMineDepth); stuckCounter = 1; }
            else stuckCounter++;
        }
        else
        {
            if (stuckCounter > 0)
            {
                statusMessage = "Ship stuck! Retrying";
                lastDistToTarget = currentMineDepth;
                stuckCounter = 0;
                UpdateSensor(false, true, 0, shipDiameter * sensorRange);
            }
            SetFlightTarget(GetMiningDepthPosition(miningTargetPos, Math.Max(0, lastDistToTarget - shipDiameter)), GetWorkSpeed(false));
            if (distToTarget <= wpReachedDist / 2) { stuckCounter = 1; lastDistToTarget = 0; }
            return;
        }

        UpdateSensor(false, true, shipDiameter * sensorRange, 0);
        Vector3 alignedPos = miningTargetPos + miningForward * currentMineDepth;
        bool isOffCenter = false;
        if (Vector3.Distance(alignedPos, shipPosition) > 0.3f)
        {
            Vector3 corrected = miningTargetPos + miningForward * (currentMineDepth + 0.1f);
            SetFlightTarget(corrected, 4);
            isOffCenter = true;
        }
        else
        {
            float speed = GetWorkSpeed(true);
            Vector3 deepTarget = GetMiningDepthPosition(miningTargetPos, Math.Max(configDepth + 1, currentMineDepth + 1));
            SetFlightTargetFull(true, false, false, deepTarget, deepTarget - miningTargetPos, speed, speed);
        }

        bool depthReached = false;
        if (depthMode == DepthMode.AutoStone || depthMode == DepthMode.AutoOre)
        {
            if (!isOffCenter)
            {
                float oreCount = 0;
                foreach (IMyTerminalBlock tool in workTools)
                    oreCount += CountBlockItems(tool, "", "", depthMode == DepthMode.AutoOre ? new string[] { "STONE" } : null);
                if (oreCount > lastOreAmount || currentMineDepth < configDepth || isFirstDescent) { autoDepthCounter = 0; autoDepthRef = currentMineDepth; runtimeMaxDepth = (int)(Math.Max(runtimeMaxDepth, autoDepthRef) + shipDiameter / 2); }
                else { depthReached = currentMineDepth - autoDepthRef > 2 && autoDepthCounter >= 20; autoDepthCounter++; }
                lastOreAmount = oreCount;
            }
        }
        else depthReached = currentMineDepth >= runtimeMaxDepth;

        if (lastHoleIndex != currentHoleIndex) { SetNavState(NavState.Returning); currentMineDepth = 0; return; }
        if (depthReached) { currentHoleIndex++; SetNavState(NavState.Returning); currentMineDepth = 0; return; }
    }

    // [Returning / ReturnToDock / FlyToDropPos] -> fly back to surface
    if (currentNavState == NavState.Returning || currentNavState == NavState.ReturnToDock || currentNavState == NavState.FlyToDropPos)
    {
        if (stateFirstTick)
        {
            miningTargetPos = GetJobGridPosition(holeCol, holeRow, false);
            // heading home but not in a hole (e.g. low on energy while waiting): straight to the path
            Vector3 rel = shipPosition - miningTargetPos;
            if (currentNavState == NavState.ReturnToDock && homeDock.isSet &&
                (rel - miningForward * Vector3.Dot(rel, miningForward)).Length() > Math.Max(JTW(), JTH())) { SetNavState(NavState.FlyToPath); return; }
            AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, false);
            SetBlocksEnabled(workTools, enableDrillsBothWays);
            SetSortersEnabled(false);
            lastDistToTarget = Vector3.Distance(shipPosition, miningTargetPos);
            UpdateSensor(false, true, 0, shipDiameter * sensorRange);
        }
        if (stateFirstTick) exitRise = false;
        SetFlightTarget(exitRise ? miningTargetPos - miningForward * shipDiameter : miningTargetPos, GetWorkSpeed(false));

        if (!exitRise && Vector3.Distance(shipPosition, miningTargetPos) >= lastDistToTarget + 5) { SetBlocksEnabled(workTools, false); SetSortersEnabled(true); statusMessage = "Can´t return!"; }

        if (distToTarget < wpReachedDist)
        {
            // leaving: first one ship size above the job plane, clear of the ships at the surface, before turning
            if (currentNavState == NavState.ReturnToDock && !exitRise) { exitRise = true; return; }
            if (currentNavState == NavState.Returning && returnToJobAfter) SetNavState(NavState.FlyToJobPos);
            if (currentNavState == NavState.Returning) SetNavState(NavState.FlyToXY);
            if (currentNavState == NavState.FlyToDropPos) SetNavState(NavState.FlyToDropPos2);
            if (currentNavState == NavState.ReturnToDock)
            {
                if (homeDock.isSet) SetNavState(NavState.FlyToPath);
                else { StopAll(); FlyToJobPosition(); statusMessage = "Can´t return, no dock found"; }
            }
            return;
        }
    }

    // [FlyToPath -> FlyToDockArea -> FlyToJobArea] follow recorded path
    if (currentNavState == NavState.FlyToDockArea || currentNavState == NavState.FlyToJobArea)
    {
        if (currentNavState == NavState.FlyToJobArea && jobState == JobState.Active && shipMode != ShipMode.Shuttle)
        {
            if (!CheckEnergy() || IsShipFull(true)) { SetNavState(NavState.FlyToDockArea); return; }
        }

        bool reachedEnd = false;
        bool useDockDir = false;
        bool atWaypoint = false;
        float approachSpeed = 0;
        bool offPath = false;
        WaypointInfo wp = null;

        if (stateFirstTick)
        {
            if (currentNavState == NavState.FlyToDockArea || shipMode == ShipMode.Shuttle)
            {
                // dock pool: own home dock if free, else a free pool connector, else wait at the path end
                if (shipMode != ShipMode.Shuttle)
                {
                    if (StationCleared(1)) ApplyStationDock(); // station dock control: the dock it assigned
                    else { poolDock = null; poolDockId = 0; dockWaiting = !ChooseDock(1); }
                }
                WaypointInfo dock = GetCurrentDock();
                pathFollowData = new PathFollowData();
                pathFollowData.target = dock;
                pathFollowData.directFlyDist = followPathDock * shipDiameter;
                pathFollowData.useDockDirDist = useDockDirectionDist * shipDiameter;
                pathFollowData.approachSpeed = 10;
                pathFollowData.targetPositions.Add(dock.position);
                Vector3 approachPos = new Vector3();
                if (GetDockApproachPosition(dock, dockDist * shipDiameter, true, out approachPos))
                    pathFollowData.targetPositions.Add(approachPos);
                else pathFollowData.directFlyDist *= 1.5f;
                if (shipMode == ShipMode.Shuttle)
                {
                    if (dock == homeDock) pathFollowData.excludePos = jobPosition.position;
                    if (dock == jobPosition) pathFollowData.excludePos = homeDock.position;
                    pathFollowData.excludeDist = dockDist * shipDiameter * 1.1f;
                }
            }
            else if (currentNavState == NavState.FlyToJobArea)
            {
                pathFollowData = new PathFollowData();
                pathFollowData.target = jobPosition;
                pathFollowData.directFlyDist = followPathJob * shipDiameter;
                pathFollowData.useDockDirDist = useJobDirectionDist * shipDiameter;
                pathFollowData.approachSpeed = 10;
                pathFollowData.excludePos = homeDock.position;
                pathFollowData.excludeDist = dockDist * shipDiameter * 1.1f;
                pathFollowData.targetPositions.Add(jobPosition.position);
                if (jobState == JobState.NoJob)
                {
                    if (!homeDock.isSet || waypoints.Count == 0) { StopAll(); return; }
                    float distFirst = Vector3.Distance(waypoints.First().position, homeDock.position);
                    float distLast = Vector3.Distance(waypoints.Last().position, homeDock.position);
                    if (distFirst < distLast) pathFollowData.target = waypoints.Last();
                    else pathFollowData.target = waypoints.First();
                }
            }

            pathSegmentDir = new Vector3();
            offPath = !IsOnPath(shipPosition);
            SetBlocksEnabled(workTools, false);
            SetSortersEnabled(true);
            pathWaypointIndex = -1;
            double closestDist = -1;
            for (int i = waypoints.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(waypoints[i].position, pathFollowData.excludePos) <= pathFollowData.excludeDist) continue;
                double dist = Vector3.Distance(waypoints[i].position, shipPosition);
                if (closestDist == -1 || dist < closestDist) { pathWaypointIndex = i; closestDist = dist; }
            }
            pathDirection = GetPathDirection(pathFollowData.target.position, pathWaypointIndex);
            currentPathWaypoint = null;
        }

        if (currentNavState == NavState.FlyToDockArea && shipMode != ShipMode.Shuttle && !stateFirstTick && !StationHandlesMe())
        {
            WaypointInfo before = GetCurrentDock();
            dockWaiting = !ChooseDock(1);
            if (GetCurrentDock() != before) { SetNavState(NavState.FlyToDockArea); return; }
        }

        // station dock control: cleared -> fly to the connector it assigned
        if (currentNavState == NavState.FlyToDockArea && shipMode != ShipMode.Shuttle && !stateFirstTick && StationCleared(1) && ApplyStationDock())
        { SetNavState(NavState.FlyToDockArea); return; }

        // station dock control: wait outside the docking area until cleared (facing the way it flew in)
        bool inDockZone = false;
        if (currentNavState == NavState.FlyToDockArea && !stateFirstTick && pathWaypointIndex >= 0 && pathWaypointIndex < waypoints.Count)
        {
            Vector3 cur = waypoints[pathWaypointIndex].position + GetLaneOffset(pathWaypointIndex, pathDirection);
            int nextIdx = pathWaypointIndex + pathDirection;
            Vector3 next = pathDirection != 0 && nextIdx >= 0 && nextIdx < waypoints.Count ? waypoints[nextIdx].position : pathFollowData.target.position;
            if (HoldForDockArea(cur, next))
            {
                if (!dockHoldSet) { dockHoldSet = true; dockHoldPos = InDockZone(cur) ? shipPosition : cur; }
                SetFlightTarget(dockHoldPos, 0); // keeps facing the way it flew in; turns to the dock once cleared
                if (!outOfService) statusMessage = "Waiting for dock clearance";
                return;
            }
            dockHoldSet = false;
            inDockZone = StationCleared(1) && (InDockZone(shipPosition) || InDockZone(next));
        }

        CalculatePathSpeeds(waypoints, pathDirection, pathFollowData.targetPositions, pathFollowData.directFlyDist, stateFirstTick, ref stuckCounter);

        for (int i = 0; i < pathFollowData.targetPositions.Count; i++)
        {
            float dist = Vector3.Distance(shipPosition, pathFollowData.targetPositions[i]);
            if (dist <= pathFollowData.directFlyDist) reachedEnd = true;
            if (dist <= pathFollowData.useDockDirDist) useDockDir = true;
        }
        if (inDockZone) useDockDir = true; // cleared: keep the dock orientation and approach speed inside the docking area
        if (useDockDir) approachSpeed = pathFollowData.approachSpeed;

        float wpSpeed = currentPathWaypoint != null ? currentPathWaypoint.maxSpeed : shipSpeed;
        float wpReachDist = (float)Math.Max(shipSpeed * 0.1f * shipDiameter, wpReachedDist);

        if ((distToTarget < wpReachDist) || stateFirstTick)
        {
            if (!stateFirstTick) pathWaypointIndex += pathDirection;
            if (pathDirection == 0 || pathWaypointIndex > waypoints.Count - 1 || pathWaypointIndex < 0)
                reachedEnd = true;
            else
            {
                waypointsRemaining = pathDirection > 0 ? waypoints.Count - 1 - pathWaypointIndex : pathWaypointIndex;
                wp = waypoints[pathWaypointIndex];
                currentPathWaypoint = wp;
                if (pathWaypointIndex >= 1 && pathWaypointIndex < waypoints.Count - 1)
                    pathSegmentDir = wp.position - waypoints[pathWaypointIndex - pathDirection].position;
                else currentPathWaypoint = null;
                navTargetPos = wp.position + GetLaneOffset(pathWaypointIndex, pathDirection);
                atWaypoint = true;
            }
        }

        if (useDockDir)
            AlignToDown(pathFollowData.target.downDir, pathFollowData.target.forwardDir, pathFollowData.target.leftDir, false);
        else if (offPath)
            AlignToGravity(pathFollowData.target.downDir, 10, true);
        else if (atWaypoint && wp != null)
        {
            if (pathDirection > 0) AlignToDown(wp.downDir, wp.forwardDir, wp.leftDir, 90, false);
            else AlignToDown(wp.downDir, -wp.forwardDir, -wp.leftDir, 90, false);
        }

        SetFlightTargetFull(true, false, true, navTargetPos, pathSegmentDir, currentPathWaypoint == null ? 0 : currentPathWaypoint.maxSpeed, approachSpeed);

        if (reachedEnd)
        {
            waypointsRemaining = 0;
            // station dock control: wait at the path end until the station clears this ship
            if (currentNavState == NavState.FlyToDockArea && !CheckDockClearance()) { SetFlightTarget(navTargetPos, 0); return; }
            if (currentNavState == NavState.FlyToDockArea && dockWaiting)
            { SetFlightTarget(navTargetPos, 0); statusMessage = "Waiting for a free dock"; return; }
            if (currentNavState == NavState.FlyToDockArea || shipMode == ShipMode.Shuttle) { SetNavState(NavState.ApproachDock); return; }
            if (currentNavState == NavState.FlyToJobArea && returnToJobAfter) { SetNavState(NavState.FlyToJobPos); return; }
            if (currentNavState == NavState.FlyToJobArea) { SetNavState(NavState.FlyToXY); return; }
        }
    }

    // [ApproachDock / RetryDocking] -> approach connector
    if (currentNavState == NavState.ApproachDock || currentNavState == NavState.RetryDocking)
    {
        if (shipMode != ShipMode.Shuttle && StationHandlesMe())
        {
            // station dock control: lost the clearance (timeout) -> back to the path end and wait
            if (!StationCleared(1)) { SetNavState(NavState.FlyToDockArea); return; }
        }
        else if (shipMode != ShipMode.Shuttle)
        {
            // dock pool: a ship already docking/docked there has priority -> back to the path end
            WaypointInfo before = GetCurrentDock();
            if (!ChooseDock(2) || GetCurrentDock() != before) { SetNavState(NavState.FlyToDockArea); return; }
        }
        WaypointInfo dock = GetCurrentDock();
        if (stateFirstTick)
        {
            if (!GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos) || !GetDockApproachPosition(dock, (dockDist + 1) * shipDiameter, true, out laneEntry))
            { ShowStatusMessage(StatusMsg.ConnectorNotReady); StopAll(); return; }
            // straight lane in front of the connector: off its line -> first to the lane entry, then straight in (no diagonal past docked ships)
            toLane = OffLane(navTargetPos);
            SetFlightTarget(toLane ? laneEntry : navTargetPos, 0);
            AlignToGravity(dock.downDir, 90, true);
        }
        if (toLane && distToTarget < wpReachedDist && distToTarget != -1)
        {
            toLane = false;
            GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos);
            SetFlightTarget(navTargetPos, 0);
            return;
        }
        if (distToTarget < followPathDock * shipDiameter && distToTarget != -1)
        {
            SetFlightTarget(navTargetPos, 10);
            AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false);
        }
        if (FindConnector(MyShipConnectorStatus.Connectable) != null || FindConnector(MyShipConnectorStatus.Connected) != null)
        { SetNavState(NavState.Docking); return; }
        // reached the point beside the obstacle: from here straight to the connector again
        if (detourShift.LengthSquared() > 0 && distToTarget < wpReachedDist && distToTarget != -1) { detourShift = new Vector3(); return; }
        if (distToTarget < wpReachedDist / 2 && distToTarget != -1)
        { SetNavState(NavState.AlignDock); return; }
    }

    // [AlignDock / AlignJob] -> precise alignment
    if (currentNavState == NavState.AlignDock || currentNavState == NavState.AlignJob)
    {
        if (stateFirstTick)
        {
            if (currentNavState == NavState.AlignDock)
            {
                WaypointInfo dock = GetCurrentDock();
                if (!GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos))
                { ShowStatusMessage(StatusMsg.ConnectorNotReady); StopAll(); return; }
                SetFlightTargetFull(true, true, false, navTargetPos, 0);
                AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, 10, false);
            }
            if (currentNavState == NavState.AlignJob)
            {
                AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, 0.5f, false);
                navTargetPos = jobPosition.position;
                SetFlightTargetFull(true, true, false, navTargetPos, 0);
            }
        }
        if (isAligned)
        {
            SetGyroOverride(false, 0, 0, 0, 0);
            if (currentNavState == NavState.AlignDock) SetNavState(NavState.Docking);
            if (currentNavState == NavState.AlignJob) StopAll();
            return;
        }
    }

    // [Docking] -> final approach and connect
    if (currentNavState == NavState.Docking)
    {
        if (FindConnector(MyShipConnectorStatus.Connected) != null)
        {
            if (shipMode == ShipMode.Shuttle) SetNavState(NavState.WaitForCommand);
            else SetNavState(NavState.Unloading);
            return;
        }

        WaypointInfo dock = GetCurrentDock();
        if (stateFirstTick)
        {
            stuckSensitivity = 0;
            dockStartTime = DateTime.Now;
            stuckCounter = 0;
            AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false);
        }

        Vector3I gridPos = new Vector3I((int)dock.connectorGridPos.X, (int)dock.connectorGridPos.Y, (int)dock.connectorGridPos.Z);
        IMySlimBlock connBlock = Me.CubeGrid.GetCubeBlock(gridPos);
        float dockSpeed2 = dockingSpeed;
        float maxDockSpeed = dockingSpeed * 5;
        float approachDist = Math.Max(1.5f, Math.Min(5f, shipDiameter * 0.15f));

        if (!GetDockApproachPosition(dock, 0, true, out navTargetPos) || !GetDockApproachPosition(dock, approachDist, true, out pathSegmentDir) ||
            connBlock == null || !connBlock.FatBlock.IsFunctional)
        { ShowStatusMessage(StatusMsg.ConnectorNotReady); StopAll(); return; }

        if (stuckSensitivity == 1 || (Vector3.Distance(shipPosition, navTargetPos) <= approachDist * 1.1f && !stateFirstTick))
            stuckSensitivity = 1;
        else
        {
            Vector3 toApproach = LocalTransformDirection(remoteControl, pathSegmentDir - shipPosition);
            Vector3 gravDir = LocalTransformDirection(remoteControl, remoteControl.GetNaturalGravity());
            float safeSpeed = CalculateMaxSpeed(toApproach, gravDir, null);
            dockSpeed2 = Math.Min(maxDockSpeed, safeSpeed);
        }

        SetFlightTargetFull(true, false, false, navTargetPos, navTargetPos - shipPosition, dockingSpeed, dockSpeed2);
        if (stateFirstTick) lastDistToTarget = (float)distToTarget;

        IMyShipConnector connectable = FindConnector(MyShipConnectorStatus.Connectable);
        if (connectable != null)
        {
            SetFlightTargetFull(false, false, false, navTargetPos, 0);
            if (stuckCounter > 0) stuckCounter = 0;
            stuckCounter--;
            if (stuckCounter < -5)
            {
                connectable.Connect();
                if (connectable.Status == MyShipConnectorStatus.Connected)
                {
                    if (shipMode == ShipMode.Shuttle) SetNavState(NavState.WaitForCommand);
                    else SetNavState(NavState.Unloading);
                    StopFlight();
                    SetLandingGears(landingGears, true);
                    return;
                }
            }
        }
        else
        {
            float distRounded = (float)Math.Round(distToTarget, 1);
            if (distRounded < lastDistToTarget) { stuckCounter = -1; lastDistToTarget = distRounded; }
            else stuckCounter++;
            if (stuckCounter > 20) { SetNavState(NavState.RetryDocking); return; }
        }
    }

    // [Unloading / WaitForCommand / WaitUranium / FillHydrogen / Charging] -> docked operations
    if (currentNavState == NavState.Unloading || currentNavState == NavState.WaitForCommand ||
        currentNavState == NavState.WaitUranium || currentNavState == NavState.FillHydrogen || currentNavState == NavState.Charging)
    {
        bool atHome = false, atJob = false;
        if (shipMode == ShipMode.Shuttle)
        {
            if (GetCurrentDock() == homeDock) atHome = true;
            else if (GetCurrentDock() == jobPosition) atJob = true;
        }

        if (stateFirstTick)
        {
            simulateShipFull = false;
            if (FindConnector(MyShipConnectorStatus.Connected) == null) { SetNavState(NavState.Undocking); return; }
            StopFlight();
            if (atHome) TriggerTimerBlock(undockConfig1.dockTimerName);
            if (atJob) TriggerTimerBlock(undockConfig2.dockTimerName);
            unloadComplete = false;
            batteryOk = false;
            hydrogenOk = false;
            uraniumOk = false;
        }

        if (FindConnector(MyShipConnectorStatus.Connected) == null) { StopAll(); ShowStatusMessage(StatusMsg.Interrupted); return; }

        // Check energy states
        if (jobState != JobState.Active || minBatteryPercent == -1 || batteryState == BatteryState.None) batteryOk = true;
        else if (batteryPercent >= 100f) batteryOk = true;
        else if (batteryPercent <= 99f) batteryOk = false;

        if (jobState != JobState.Active || minHydrogenPercent == -1 || hydrogenTanks.Count == 0) hydrogenOk = true;
        else if (hydrogenPercent >= 100f) hydrogenOk = true;
        else if (hydrogenPercent <= 99) hydrogenOk = false;

        if (jobState != JobState.Active || minUraniumKg == -1 || reactors.Count == 0) uraniumOk = true;
        else uraniumOk = uraniumAmount >= minUraniumKg;

        UndockConfig undockCfg = null;
        if (atHome) undockCfg = undockConfig1;
        if (atJob) undockCfg = undockConfig2;
        if (undockCfg != null && (undockCfg.trigger == UndockTrigger.OnBatteriesLow25 || undockCfg.trigger == UndockTrigger.OnBatteriesEmpty)) batteryOk = true;
        if (undockCfg != null && (undockCfg.trigger == UndockTrigger.OnBatteriesFull)) if (!unloadComplete) batteryOk = false;
        if (undockCfg != null && (undockCfg.trigger == UndockTrigger.OnHydrogenLow25 || undockCfg.trigger == UndockTrigger.OnHydrogenEmpty)) hydrogenOk = true;
        if (undockCfg != null && (undockCfg.trigger == UndockTrigger.OnHydrogenFull)) if (!unloadComplete) hydrogenOk = false;

        if (isSlowTick)
        {
            ChargeMode chargeMode = batteryOk ? ChargeMode.Auto : ChargeMode.Recharge;
            if (undockCfg != null && (undockCfg.trigger == UndockTrigger.OnBatteriesEmpty || undockCfg.trigger == UndockTrigger.OnBatteriesLow25))
                chargeMode = ChargeMode.Discharge;
            SetBatteryChargeMode(chargeMode);
            SetHydrogenStockpile(!hydrogenOk);
        }

        if (!unloadComplete)
        {
            if (shipMode == ShipMode.Shuttle)
                unloadComplete = jobState != JobState.Active || CheckShuttleUndock(stateFirstTick, true) || undockRequested;
            else
                unloadComplete = jobState != JobState.Active || IsCargoEmpty();
        }
        else
        {
            if (!batteryOk) SetNavState(NavState.Charging);
            if (!hydrogenOk) SetNavState(NavState.FillHydrogen);
            if (!uraniumOk) SetNavState(NavState.WaitUranium);
            stateFirstTick = false;
        }

        if (unloadComplete && batteryOk && hydrogenOk && uraniumOk)
        {
            SetBatteryChargeMode(ChargeMode.Auto);
            SetHydrogenStockpile(false);
            if (jobState == JobState.Active)
            {
                if (shipMode == ShipMode.Shuttle)
                {
                    if (GetCurrentDock() == homeDock) TriggerTimerBlock(undockConfig1.leaveTimerName);
                    else if (GetCurrentDock() == jobPosition) TriggerTimerBlock(undockConfig2.leaveTimerName);
                    if (GetCurrentDock() == homeDock) SetDockTarget(jobPosition, JobState.ActiveJob);
                    else SetDockTarget(homeDock, JobState.ActiveHome);
                }
            }
            SetNavState(NavState.Undocking);
            return;
        }
    }

    // [Undocking] -> disconnect and fly away from dock
    if (currentNavState == NavState.Undocking)
    {
        // station dock control: stay docked until the station clears the undock
        if (waitUndockClearance)
        {
            if (!CheckUndockClearance()) return;
            waitUndockClearance = false;
            SetNavState(NavState.Undocking);
            return;
        }
        if (stateFirstTick)
        {
            if (!CheckUndockClearance()) { waitUndockClearance = true; return; }
            IMyShipConnector connected = FindConnector(MyShipConnectorStatus.Connected);
            if (connected == null) { SetNavState(NavState.FlyToJobArea); return; }
            IMyShipConnector other = connected.OtherConnector;
            SetBlockEnabled(connected, false);
            SetLandingGears(landingGears, false);

            WaypointInfo dock = null;
            WaypointInfo home = shipMode == ShipMode.Shuttle ? homeDock : GetCurrentDock(); // dock pool
            if (Vector3.Distance(connected.GetPosition(), home.position) < 5f && homeDock.isSet) dock = home;
            if (Vector3.Distance(connected.GetPosition(), jobPosition.position) < 5f && jobPosition.isSet) dock = jobPosition;

            if (dock != null)
            {
                if (!GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos) || !GetDockApproachPosition(dock, (dockDist + 1) * shipDiameter, true, out laneEntry))
                { ShowStatusMessage(StatusMsg.ConnectorNotReady); StopAll(); return; }
                SetFlightTarget(navTargetPos, 5);
                AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false);
            }
            else SetFlightTarget(shipPosition + other.WorldMatrix.Forward * dockDist * shipDiameter, 5);
            toLane = dock != null;

            if (jobState == JobState.Active) ShowStatusMessage(StatusMsg.Running);
        }
        if (distToTarget < wpReachedDist)
        {
            // back out straight along the connector lane to its entry, then on
            if (toLane) { toLane = false; SetFlightTarget(laneEntry, 5); return; }
            SetBlocksEnabled(connectors, true);
            SetNavState(NavState.FlyToJobArea);
            return;
        }
    }

    // [FlyToJobPos] -> fly directly to job area
    if (currentNavState == NavState.FlyToJobPos)
    {
        if (stateFirstTick)
        {
            SetSortersEnabled(true);
            SetBlocksEnabled(workTools, false);
            navTargetPos = jobPosition.position;
            SetFlightTarget(navTargetPos, 20);
            AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, false);
        }
        if (distToTarget < wpReachedDist / 2) { SetNavState(NavState.AlignJob); return; }
    }

    // [FlyToPath] -> fly to nearest path waypoint
    if (currentNavState == NavState.FlyToPath)
    {
        if (stateFirstTick)
        {
            SetSortersEnabled(true);
            SetBlocksEnabled(workTools, false);
            int closest = -1;
            double closestDist = -1;
            // leaving the job site: the nearest waypoint outside the job area (don't go back down
            // among the other ships to the path end, turn towards the path out there)
            bool skipJob = NearJobArea();
            for (int pass = 0; pass < 2 && closest == -1; pass++, skipJob = false)
                for (int i = waypoints.Count - 1; i >= 0; i--)
                {
                    if (skipJob && Vector3.Distance(waypoints[i].position, jobPosition.position) < JobRadius() + shipDiameter) continue;
                    double dist = Vector3.Distance(waypoints[i].position, shipPosition);
                    if (closestDist == -1 || dist < closestDist) { closest = i; closestDist = dist; }
                }
            if (closest == -1) { SetNavState(NavState.FlyToDockArea); return; }
            navTargetPos = waypoints[closest].position;
            SetFlightTarget(navTargetPos, 10);
            AlignToDown(jobPosition.downDir, jobPosition.forwardDir, jobPosition.leftDir, false);
        }
        if (distToTarget < wpReachedDist) { SetNavState(NavState.FlyToDockArea); return; }
    }

    // [WaitEjection / WaitEjectionDrop / FlyToDropPos2] -> stone ejection
    if (currentNavState == NavState.FlyToDropPos2)
    {
        bool done = false;
        if (stateFirstTick)
        {
            SetSortersEnabled(true);
            if ((ejectionMode == EjectionMode.DropPosStone || ejectionMode == EjectionMode.DropPosStoIce) && IsNearPlanet() &&
                AngleBetween(miningForward, remoteControl.GetNaturalGravity()) < 25 && jobWidth >= 2 && jobHeight >= 2)
            {
                Vector3 dropPos = shipPosition;
                if (holeCol > 0 && holeRow < jobHeight - 1) dropPos = GetJobGridPosition(holeCol - 1, holeRow + 1, true);
                else if (holeCol < jobWidth - 1 && holeRow < jobHeight - 1) dropPos = GetJobGridPosition(holeCol + 1, holeRow + 1, true);
                else if (holeCol < jobWidth - 1 && holeRow > 0) dropPos = GetJobGridPosition(holeCol + 1, holeRow - 1, true);
                else if (holeCol > 0 && holeRow > 0) dropPos = GetJobGridPosition(holeCol - 1, holeRow - 1, true);
                else done = true;
                if (!done) SetFlightTarget(dropPos, 10);
            }
            else done = true;
        }
        if (distToTarget < wpReachedDist / 2) done = true;
        if (done) { SetNavState(NavState.WaitEjectionDrop); return; }
    }

    if (currentNavState == NavState.WaitEjection || currentNavState == NavState.WaitEjectionDrop)
    {
        if (stateFirstTick)
        {
            SetFlightTargetFull(true, true, false, shipPosition, 0);
            SetBlocksEnabled(workTools, false);
            SetSortersEnabled(true);
            stuckCounter = -1;
            stuckSensitivity = ejectionMode == EjectionMode.InMotionStone || ejectionMode == EjectionMode.InMotionStoIce ? 0 : -1;
        }

        bool energyLow = !CheckEnergy();
        int stoneCount = CountItems("STONE", "ORE", ItemLocation.All);
        if (ejectionMode == EjectionMode.CurPosStoIce || ejectionMode == EjectionMode.InMotionStoIce || ejectionMode == EjectionMode.DropPosStoIce)
            stoneCount += CountItems("ICE", "ORE", ItemLocation.All);
        bool hasStone = stoneCount > 0;

        bool ejectionStuck = false;
        if (stuckSensitivity >= 0)
        {
            float angle = (float)Math.Sin(DegToRad(stuckSensitivity)) * toolWidth / 3f;
            float angle2 = (float)Math.Cos(DegToRad(stuckSensitivity)) * toolHeight / 3f;
            Vector3 circlePos = GetJobGridPosition(holeCol, holeRow, true) + TransformLocalToWorld(miningForward, miningDown * -1, new Vector3(angle, angle2, 0));
            SetFlightTarget(circlePos, 0.3f);
            if (distToTarget < Math.Min(toolWidth, toolHeight) / 10f) stuckSensitivity += 5f;
            if (stuckSensitivity >= 360) stuckSensitivity = 0;
        }

        if (stuckCounter == -1 || stoneCount < stuckCounter) { stuckCounter = stoneCount; lastDistToTarget = 0; }
        else { lastDistToTarget++; if (lastDistToTarget > 50) ejectionStuck = true; }

        if (!hasStone || energyLow || ejectionStuck)
        {
            if (!energyLow)
            {
                int totalOre = CountItems("", "ORE", ItemLocation.All);
                if (IsShipFull(true)) energyLow = true;
                else if (100 - (SafeDiv(totalOre, oreCountBeforeEject) * 100) < minEjection) energyLow = true;
                else ShowStatusMessage(StatusMsg.Running);
            }
            if (ejectionStuck && energyLow) statusMessage = "Ejection failed";
            if (currentNavState == NavState.WaitEjectionDrop)
            {
                if (energyLow) { if (homeDock.isSet) SetNavState(NavState.FlyToPath); else { StopAll(); FlyToJobPosition(); statusMessage = "Can´t return, no dock found"; } }
                else SetNavState(NavState.FlyToXY);
            }
            else if (energyLow) SetNavState(NavState.ReturnToDock);
            else SetNavState(NavState.Mining);
            return;
        }
    }

    stateFirstTick = false;
}


// ========================== PATH SPEED CALCULATION ==========================

void CalculatePathSpeeds(List<WaypointInfo> wps, int dir, List<Vector3> targets, float targetDist, bool reset, ref int calcIdx)
{
    if (reset) { for (int i = 0; i < wps.Count; i++) wps[i].maxSpeed = 0; calcIdx = -1; return; }
    if (dir == 0) return;
    int step = dir * -1;
    if (calcIdx == -1) calcIdx = step > 0 ? 1 : wps.Count - 2;

    int iterations = 0;
    while (calcIdx >= 1 && calcIdx < wps.Count - 1)
    {
        if (iterations > 50) return;
        iterations++;
        try
        {
            if ((step < 0 && calcIdx >= 1) || (step > 0 && calcIdx <= wps.Count - 2))
            {
                WaypointInfo current = wps[calcIdx];
                bool nearTarget = false;
                for (int j = 0; j < targets.Count; j++)
                    if (Vector3.Distance(current.position, targets[j]) <= targetDist) { nearTarget = true; break; }

                if (!nearTarget)
                {
                    WaypointInfo prev = wps[calcIdx - step];
                    WaypointInfo next = wps[calcIdx + step];
                    Vector3 toNext = current.position - next.position;
                    Vector3 toPrev = prev.position - current.position;
                    Vector3 projected = current.position + Vector3.Normalize(toNext) * toPrev.Length();
                    Vector3 deviation = prev.position - projected;
                    Vector3 localDev = TransformToLocal(dir > 0 ? current.forwardDir : current.forwardDir * -1, current.downDir * -1, deviation);
                    Vector3 localPrev = TransformToLocal(dir > 0 ? current.forwardDir : current.forwardDir * -1, current.downDir * -1, toPrev);
                    Vector3 localGrav = TransformToLocal(dir > 0 ? current.forwardDir : current.forwardDir * -1, current.downDir * -1, current.gravity);

                    current.maxSpeed = (float)Math.Sqrt(Math.Pow(prev.maxSpeed, 2) + Math.Pow(CalculateMaxSpeed(-localPrev, localGrav, current), 2));

                    for (int j = 0; j < targets.Count; j++)
                        if (Vector3.Distance(prev.position, targets[j]) <= targetDist)
                        {
                            Vector3 localTarget = TransformToLocal(dir > 0 ? current.forwardDir : current.forwardDir * -1, current.downDir * -1, targets[j] - current.position);
                            float targetSpeed = CalculateMaxSpeed(-localTarget, localGrav, current);
                            current.maxSpeed = Math.Min(current.maxSpeed, targetSpeed) / 2f;
                        }

                    if (localDev.Length() == 0) localDev = new Vector3(0, 0, 1);
                    Vector3 localGravDir = TransformToLocal(current.forwardDir, current.downDir * -1, current.gravity);
                    float thrustForDev = GetThrustForDirection(-localDev, localGravDir, current);
                    float accel = SafeDiv(thrustForDev, shipMass);
                    float timeToStop = (float)Math.Sqrt(SafeDiv(localDev.Length() * 1.0f, 0.5f * accel));
                    current.maxSpeed = Math.Min(current.maxSpeed, (toPrev.Length() / timeToStop) * accelerationFactor);
                }
            }
        }
        catch { return; }
        calcIdx += step;
    }
    calcIdx = -1;
}
