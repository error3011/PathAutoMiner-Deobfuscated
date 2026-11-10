// ========================== SETTINGS (PB Custom Data) ==========================
// The settings live in the [PAM settings] section of this PB's Custom Data, below the broadcast
// settings. Read when the script compiles (Recompile after editing). Missing settings are written
// with their defaults, so a new version adds its new settings by itself; a value that can't be
// read keeps the default and is reported.

const String CFG_MARK = "[PAM settings]";
String[] cfgIn;
String cfgOut = "", cfgBad = "";

void LoadSettings()
{
    String nd = ApplySettings(Me.CustomData);
    if (nd != Me.CustomData) Me.CustomData = nd;
}

// reads the settings from this Custom Data text, returns it with the settings section updated
String ApplySettings(String cd)
{
    int at = cd.IndexOf(CFG_MARK);
    cfgIn = at < 0 ? new String[0] : cd.Substring(at).Split('\n');
    cfgOut = CFG_MARK + " edit, then Recompile. Delete a line to get its default. See README.\n";
    cfgBad = "";
    pamTag = CS("pamTag", pamTag);
    controllerTag = CS("controllerTag", controllerTag);
    dataTag = CS("dataTag", dataTag);
    dockPoolTag = CS("dockPoolTag", dockPoolTag);
    dockDist = CF("dockDist", dockDist);
    drillRadius = CF("drillRadius", drillRadius);
    minAccelerationSmall = CF("minAccelerationSmall", minAccelerationSmall);
    minAccelerationLarge = CF("minAccelerationLarge", minAccelerationLarge);
    setLCDFontAndSize = CB("setLCDFontAndSize", setLCDFontAndSize);
    trafficEnabled = CB("trafficEnabled", trafficEnabled);
    debugDraw = CB("debugDraw", debugDraw);
    if (Me != null && HasTag(Me, controllerTag, true)) LoadControllerSettings(); else LoadShipSettings();
    return (at < 0 ? (cd.Trim() == "" ? "" : cd.TrimEnd() + "\n\n") : cd.Substring(0, at)) + cfgOut;
}

void LoadShipSettings()
{
    dockRequestDistance = CF("dockRequestDistance", dockRequestDistance);
    gyroSpeedSmall = (int)CF("gyroSpeedSmall", gyroSpeedSmall);
    gyroSpeedLarge = (int)CF("gyroSpeedLarge", gyroSpeedLarge);
    generalSpeedLimit = (int)CF("generalSpeedLimit", generalSpeedLimit);
    dockingSpeed = CF("dockingSpeed", dockingSpeed);
    jobAreaSpeed = CF("jobAreaSpeed", jobAreaSpeed);
    dockAreaSpeed = CF("dockAreaSpeed", dockAreaSpeed);
    followPathDock = CF("followPathDock", followPathDock);
    followPathJob = CF("followPathJob", followPathJob);
    useDockDirectionDist = CF("useDockDirectionDist", useDockDirectionDist);
    useJobDirectionDist = CF("useJobDirectionDist", useJobDirectionDist);
    wpReachedDist = CF("wpReachedDist", wpReachedDist);
    sensorRange = CF("sensorRange", sensorRange);
    fastSpeed = CF("fastSpeed", fastSpeed);
    minEjection = CF("minEjection", minEjection);
    obstacleBrake = CB("obstacleBrake", obstacleBrake);
    obstacleLookAhead = CF("obstacleLookAhead", obstacleLookAhead);
    obstacleTimeout = CF("obstacleTimeout", obstacleTimeout);
    laneOffset = CF("laneOffset", laneOffset);
    followGap = CF("followGap", followGap);
    trafficHullCover = CB("trafficHullCover", trafficHullCover);
    trafficPadding = CF("trafficPadding", trafficPadding);
    trafficStuckTime = CF("trafficStuckTime", trafficStuckTime);
    trafficEscapeTime = CF("trafficEscapeTime", trafficEscapeTime);
    followTime = Math.Max(0.1f, CF("followTime", followTime));
    trafficTimeout = CF("trafficTimeout", trafficTimeout);
    dockRetries = Math.Max(1, (int)CF("dockRetries", dockRetries));
    moveAsideSpeed = CF("moveAsideSpeed", moveAsideSpeed);
    emergencySpeed = CF("emergencySpeed", emergencySpeed);
    laneTurnTime = CF("laneTurnTime", laneTurnTime);
    holeStuckTime = CF("holeStuckTime", holeStuckTime);
    coopWaitHeight = CF("coopWaitHeight", coopWaitHeight);
    resumeAfterRestart = CB("resumeAfterRestart", resumeAfterRestart);
}

void LoadControllerSettings()
{
    zoneTag = CS("zoneTag", zoneTag);
    laneTag = CS("laneTag", laneTag);
    dockLaneLength = CF("dockLaneLength", dockLaneLength);
    lcdLineHeight = CF("lcdLineHeight", lcdLineHeight);
    tccHorizon = CF("tccHorizon", tccHorizon);
    clearanceTimeout = CF("clearanceTimeout", clearanceTimeout);
    jobShipTimeout = CF("jobShipTimeout", jobShipTimeout);
    laneClearTime = CF("laneClearTime", laneClearTime);
}

// the setting's value from Custom Data (or the default), written back to the section
String CS(String key, String def)
{
    String v = def;
    for (int i = 1; i < cfgIn.Length; i++)
        if (cfgIn[i].StartsWith(key + "=")) v = cfgIn[i].Substring(key.Length + 1).Trim();
    cfgOut += key + "=" + v + "\n";
    return v;
}

float CF(String key, float def)
{
    float f;
    if (float.TryParse(CS(key, "" + def), out f)) return f;
    cfgBad += key + " ";
    return def;
}

bool CB(String key, bool def)
{
    String v = CS(key, def ? "true" : "false").ToLower();
    if (v != "true" && v != "false") cfgBad += key + " ";
    return v == "true" || (v != "false" && def);
}

// the settings section, kept when the broadcast settings are rewritten
String SettingsBlock()
{
    int at = Me.CustomData.IndexOf(CFG_MARK);
    return at < 0 ? "" : "\n\n" + Me.CustomData.Substring(at);
}
