// ========================== MENU NAVIGATION ==========================

void NavigateToMenu(MenuPage page)
{
    menuIndexPerPage[(int)currentMenu] = menuIndex;
    menuIndex = menuIndexPerPage[(int)page];
    if (page == MenuPage.ShipList) menuIndex = 0;
    currentMenu = page;
    if (shipMode != ShipMode.Controller)
        UpdateSensor(currentMenu == MenuPage.JobSetup, false, 0, 0);
    menuJustNavigated = true;
}

void MenuUp(bool loop)
{
    if (menuIndex > 0) menuIndex--;
    else if (loop) menuIndex = menuItemCount - 1;
}

void MenuDown(bool loop)
{
    if (menuIndex < menuItemCount - 1) menuIndex++;
    else if (loop) menuIndex = 0;
}


// ========================== NAME/STATUS HELPERS ==========================

String ShowStatusMessage(StatusMsg msg)
{
    statusMessage = Pick("Job is running|Connector not ready!|Ship modified, path outdated!|Interrupted by player!|Shuttle mode enabled!", (int)msg);
    return "";
}

String GetStartPosName(StartPosition pos) { return Pick("Top-Left|Center", (int)pos); }

String GetDepthModeName(DepthMode mode)
{
    switch (mode)
    {
        case DepthMode.AutoOre: return "Auto" + (shipMode == ShipMode.Miner ? " (Ore)" : "");
        case DepthMode.AutoStone: return "Auto (+Stone)";
        case DepthMode.Default: return "Default";
        default: return "";
    }
}

String GetJobStateName(JobState state) { return Pick("No job|Job paused|Job active|Job done|Job changed|Move home|Move to job|Job active|Job active", (int)state); }

String GetDamageBehaviorName(DamageBehavior behavior) { return Pick("Return home|Fly to job pos|Stop|Ignore", (int)behavior); }

String GetEjectionModeName(EjectionMode mode) { return Pick("Off|Cur. pos (Stone)|Cur. pos (Sto.+Ice)|Drop pos (Stone) |Drop pos (Sto.+Ice)|In motion (Stone)|In motion (Sto.+Ice)", (int)mode); }

String GetBatteryStateName(BatteryState state) { return Pick("No batteries|Charging||Discharging", (int)state); }

String GetNavStateName(NavState state)
{
    if (state == NavState.Mining && shipMode == ShipMode.Grinder) return "Grinding";
    if (state == NavState.WaitForCommand) return waitingStatusText;
    return Pick("Idle|Flying to XY position|Mining|Returning|Returning to dock|Flying to dock area|Flying to job area|Flying to path|" +
        "Flying to job position|Approaching dock|Docking|Aligning|Aligning|Retry docking|Unloading|Undocking|Charging batteries|" +
        "Waiting for uranium|Filling up hydrogen|Waiting for ejection|Waiting for ejection|Flying to drop pos|Flying to drop pos", (int)state);
}

String GetUndockTriggerName(UndockTrigger trigger)
{
    return Pick("On \"Undock\" command|On player entered cockpit|On time delay|On ship is full|On ship is empty|On batteries full|" +
        "On batteries empty(<25%)|On batteries empty(=0%)|On hydrogen full|On hydrogen empty(<25%)|On hydrogen empty(=0%)", (int)trigger);
}

// names in enum order, '|' separated (shorter than a switch)
String Pick(String list, int i)
{
    String[] a = list.Split('|');
    return i >= 0 && i < a.Length ? a[i] : "";
}
