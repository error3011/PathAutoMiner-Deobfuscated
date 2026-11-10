// ========================== 01_Constants.cs ==========================
/*//////////////////////////
 * Thank you for using:
 * [PAM] - Path Auto Miner
 * ————————————
 * Author:  Keks
 * Last update: 2019-12-20
 * ————————————
 * Guide: https://steamcommunity.com/sharedfiles/filedetails/?id=1553126390
 * Script: https://steamcommunity.com/sharedfiles/filedetails/?id=1507646929
 * Youtube: https://youtu.be/ne_i5U2Y8Fk
 * ————————————
 * Deobfuscated and patched version (see README in the repository).
 * Bugs in this version are not the original author's: don't report them in the Workshop thread.
 *//////////////////////////

const string VERSION = "1.3.1";
const string DATAREV = "14";

// ===== Settings =====
// In this PB's Custom Data, section [PAM settings], then Recompile. See README, Settings.

// ===== End of settings: don't edit below this line =====

// ---- setting defaults (the values in Custom Data override them, see LoadSettings) ----
String pamTag = "[PAM]";
String controllerTag = "[PAM-Controller]";
String dataTag = "[PAM-Data]";
String dockPoolTag = "[PAM-Dock]";
String zoneTag = "[PAM-Zone]";
String laneTag = "[PAM-Lane]";
float dockLaneLength = 40f;
float dockRequestDistance = 300f;
int gyroSpeedSmall = 15;
int gyroSpeedLarge = 5;
int generalSpeedLimit = 100;
float dockingSpeed = 0.5f;
float jobAreaSpeed = 10f;
float dockAreaSpeed = 5f;
float dockDist = 0.6f;
float followPathDock = 2f;
float followPathJob = 1f;
float useDockDirectionDist = 1f;
float useJobDirectionDist = 0f;
float wpReachedDist = 2f;
float drillRadius = 1.4f;
float sensorRange = 2f;
float fastSpeed = 10f;
float minAccelerationSmall = 0.2f;
float minAccelerationLarge = 0.1f;
float minEjection = 25f;
bool setLCDFontAndSize = true;
bool obstacleBrake = true;
float obstacleLookAhead = 3f;
float obstacleTimeout = 10f;
bool trafficEnabled = true;
float laneOffset = 0.75f;
float followGap = 1f;
bool trafficHullCover = true;
float trafficPadding = 0f;
float trafficStuckTime = 15f;
float trafficEscapeTime = 10f;
float followTime = 2f;
float trafficTimeout = 5f;
int dockRetries = 3;
float moveAsideSpeed = 2f;
float emergencySpeed = 3f;
float laneTurnTime = 8f;
float holeStuckTime = 5f;
float coopWaitHeight = 2.5f;
float tccHorizon = 8f;
float clearanceTimeout = 120f;
float jobShipTimeout = 600f;
float laneClearTime = 3f;
bool resumeAfterRestart = true;
const bool checkConveyorSystem = false; //temporarily disabled because of a SE bug
bool debugDraw = false;
float lcdLineHeight = 30f;

// ========================== 02_Enums.cs ==========================
// ========================== ENUMS ==========================

public enum ShipMode       { Unknown, Miner, Grinder, Controller, Shuttle }
public enum NavState       { Idle, FlyToXY, Mining, Returning, ReturnToDock, FlyToDockArea, FlyToJobArea,
                             FlyToPath, FlyToJobPos, ApproachDock, Docking, AlignDock, AlignJob,
                             RetryDocking, Unloading, Undocking, Charging, WaitUranium, FillHydrogen,
                             WaitEjection, WaitEjectionDrop, FlyToDropPos, FlyToDropPos2, WaitForCommand }
public enum JobState       { NoJob, Paused, Active, Done, Changed, MoveHome, MoveToJob, ActiveHome, ActiveJob }
public enum BatteryState   { None, Charging, Idle, Discharging }
public enum MenuPage       { Main, Recording, JobSetup, JobRunning, StatusOverview, BehaviorSettings,
                             AdvancedSettings, InfoPage, InfoPage2, HelpPage, ShipList, ShipInventory,
                             SendCommand, SendCommandAll, ShuttlePage1, ShuttlePage2, ShuttleBehavior }
public enum StatusMsg      { Running, ConnectorNotReady, ShipModified, Interrupted, Shuttle }
public enum StartPosition  { TopLeft, Center }
public enum DamageBehavior { ReturnHome, FlyToJob, Stop, Ignore }
public enum EjectionMode   { Off, CurPosStone, CurPosStoIce, DropPosStone, DropPosStoIce, InMotionStone, InMotionStoIce }
public enum DepthMode      { Default, AutoOre, AutoStone }
public enum ItemLocation   { InCargo, InOther, All }
public enum UndockTrigger  { OnCommand, OnPlayerEntered, OnTimerDelay, OnShipFull, OnShipEmpty,
                             OnBatteriesFull, OnBatteriesLow25, OnBatteriesEmpty, OnHydrogenFull,
                             OnHydrogenLow25, OnHydrogenEmpty }
public enum LocationType   { AtJob, OnPath, AtHomeDock, AtMine, AtDock1, AtDock2 }
public enum SpiralResult   { Found, AllDone, Error }
public enum DataResult     { Success, Failed, NoData, NewVersion }
public enum CommandState   { None, Pending, Received, NoAnswer }
public enum ConfigType     { WorkSpeed, Acceleration, MaxLoad, Ignore, MinBattery, MinUranium, MinHydrogen, TimerDelay, Overlap }
public enum LCDMode        { Unknown, MainPaged, MainCompact, Menu, Inventory, Debug, MainPagedCompact, Fleet, Ship, Log, Job, Map, Docks }

// ========================== 03_Types.cs ==========================
// ========================== CLASSES ==========================

class WaypointInfo
{
    public bool isSet = false;
    public Vector3 position = new Vector3();
    public Vector3 downDir = new Vector3();
    public Vector3 forwardDir = new Vector3();
    public Vector3 leftDir = new Vector3();
    public Vector3 gravity = new Vector3();
    public Vector3 connectorGridPos = new Vector3();
    public float maxSpeed = 0;
    public float unused = 0;
    public float[] thrusterEfficiency = null;

    public WaypointInfo() { }

    public WaypointInfo(WaypointInfo other)
    {
        isSet = other.isSet;
        position = other.position;
        downDir = other.downDir;
        forwardDir = other.forwardDir;
        leftDir = other.leftDir;
        gravity = other.gravity;
        connectorGridPos = other.connectorGridPos;
        thrusterEfficiency = other.thrusterEfficiency;
    }

    public WaypointInfo(Vector3 position, Vector3 forwardDir, Vector3 downDir, Vector3 leftDir, Vector3 gravity)
    {
        this.position = position;
        this.downDir = downDir;
        this.forwardDir = forwardDir;
        this.leftDir = leftDir;
        this.maxSpeed = 0;
        this.gravity = gravity;
    }

    public void CalcThrusterEfficiency(List<IMyThrust> allThrusters, List<string> thrusterTypeNames)
    {
        thrusterEfficiency = new float[thrusterTypeNames.Count];
        for (int i = 0; i < thrusterEfficiency.Length; i++)
            thrusterEfficiency[i] = -1;

        for (int i = 0; i < allThrusters.Count; i++)
        {
            string typeName = GetThrusterType(allThrusters[i]);
            int idx = thrusterTypeNames.IndexOf(typeName);
            if (idx != -1)
                thrusterEfficiency[idx] = SafeDiv(allThrusters[i].MaxEffectiveThrust, allThrusters[i].MaxThrust);
        }
    }
}

class PathFollowData
{
    public WaypointInfo target = null;
    public List<Vector3> targetPositions = new List<Vector3>();
    public float directFlyDist = 0;
    public float useDockDirDist = 0;
    public float approachSpeed = 0;
    public float excludeDist = 0;
    public Vector3 excludePos = new Vector3();
}

class UndockConfig
{
    public UndockTrigger trigger = UndockTrigger.OnCommand;
    public float delay = 0;
    public float elapsedDelay = 0;
    public string dockTimerName = "";
    public string leaveTimerName = "";
    DateTime lastTimerTick;
    public bool playerWasInCockpit = false;
    private bool counting = false;

    public bool CheckDelay(bool reset)
    {
        if (reset) { elapsedDelay = 0; counting = false; return false; }
        counting = true;
        return elapsedDelay > delay;
    }

    public void Reset()
    {
        CheckDelay(true);
        playerWasInCockpit = false;
    }

    public void UpdateTimer()
    {
        if (counting)
            if ((TimeNow() - lastTimerTick).TotalSeconds > 1)
            {
                elapsedDelay++;
                lastTimerTick = TimeNow();
            }
    }

    public bool HasDelay()
    {
        switch (trigger)
        {
            case UndockTrigger.OnPlayerEntered: return true;
            case UndockTrigger.OnTimerDelay: return true;
        }
        return false;
    }
}

class ItemInfo
{
    public String name = "";
    public String type = "";
    public int amount = 0;
    public ItemLocation location = ItemLocation.All;

    public ItemInfo(String name, String type, int amount, ItemLocation location)
    {
        this.name = name;
        this.type = type;
        this.amount = amount;
        this.location = location;
    }
}

class DistanceEntry
{
    public DistanceEntry(Vector3 pos, float dist)
    {
        this.pos = pos;
        this.dist = dist;
    }
    public Vector3 pos;
    public float dist;
}

class ShipInfo
{
    public DateTime lastMessageTime;
    public DateTime commandSentTime;
    public String id = "";
    public String name = "";
    public String version = "";
    public String statusMessage = "";
    public String stateText = "";
    public String menuText = "";
    public String pendingCommand = "";
    public Vector3 position = new Vector3();
    public List<ItemInfo> items = new List<ItemInfo>();
    public ShipMode shipMode = ShipMode.Unknown;
    public CommandState commandState;
    public float maxVolume;
    public float currentVolume;
    public int menuIndex;
    public int distance = 0;
    public int totalItems = 0;

    public bool IsTimedOut()
    {
        return (TimeNow() - lastMessageTime).TotalSeconds > 10;
    }

    public bool IsCommandTimedOut()
    {
        if (commandState != CommandState.Pending) return false;
        return (TimeNow() - commandSentTime).TotalSeconds >= 4;
    }

    public ShipInfo(String id) { this.id = id; }

    public CommandState ProcessCommand(String cmd, bool send, bool clear, bool checkReceived)
    {
        if (cmd == "" && !clear) return CommandState.None;
        if (commandState == CommandState.Pending && IsCommandTimedOut())
            commandState = CommandState.NoAnswer;

        if (send)
        {
            pendingCommand = cmd;
            commandState = CommandState.Pending;
            commandSentTime = TimeNow();
            return commandState;
        }
        else if (clear)
        {
            pendingCommand = "";
            commandState = CommandState.None;
        }
        else if (pendingCommand == cmd)
        {
            if (checkReceived) commandState = CommandState.Received;
            return commandState;
        }
        return CommandState.None;
    }
}


// ========================== ELEMENT CODE (user-customizable) ==========================

public String GetElementCode(String itemName)
{
    switch (itemName)
    {
        case "IRON": return "Fe";
        case "NICKEL": return "Ni";
        case "COBALT": return "Co";
        case "MAGNESIUM": return "Mg";
        case "SILICON": return "Si";
        case "SILVER": return "Ag";
        case "GOLD": return "Au";
        case "PLATINUM": return "Pt";
        case "URANIUM": return "U";
        default: return "";
    }
}

// ========================== 04_Fields.cs ==========================
// ========================== FIELDS ==========================

IMyGridTerminalSystem gridTerminalSystem;

// Ship state
Vector3 referencePosition = new Vector3();
Vector3 shipPosition = new Vector3();
Vector3 forwardDirection = new Vector3();
Vector3 leftDirection = new Vector3();
Vector3 downDirection = new Vector3();
DateTime lastScanTime = new DateTime();
bool firstRun = true;
int tickCounter = 0;
bool isSlowTick = false;
bool isTick = false;
bool needPositionUpdate = true;
bool wasSetupError = false;
bool isAligning = false;
bool initialized = false;
float shipSpeed = 0;
float shipMass = 0;
int scanPhase = 0;
int updatePhase = 0;
int scanCountdown = 0;
int broadcastCountdown = 0;
float maxInstructionPercent = 0;
float avgInstructionCount = 0;
double maxRuntime = 0;
float avgRuntime = 0;
List<int> instructionSamples = new List<int>();
List<int> runtimeSamples = new List<int>();

// Ship configuration
ShipMode shipMode = ShipMode.Unknown;
int setupErrorLevel = 0;
float toolWidth = 0;
float toolHeight = 0;
int jobSerial = 0; // new number each time a job is set up: a job set up again at the same spot is a new co-op job
float jobToolW = 0, jobToolH = 0; // drill area the job was set up with (hole spacing); 0: older save, use the ship's
float shipDiameter = 0;
float shipFootWidth = 0, shipFootHeight = 0; // ship size across the drill face

// Block references
IMyRemoteControl remoteControl;
IMySensorBlock sensor;
List<IMyTimerBlock> timerBlocks = new List<IMyTimerBlock>();
List<IMyShipConnector> connectors = new List<IMyShipConnector>();
List<IMyThrust> thrusters = new List<IMyThrust>();
List<IMyGyro> gyros = new List<IMyGyro>();
List<IMyTerminalBlock> workTools = new List<IMyTerminalBlock>();
List<IMyLandingGear> landingGears = new List<IMyLandingGear>();
List<IMyReactor> reactors = new List<IMyReactor>();
List<IMyConveyorSorter> sorters = new List<IMyConveyorSorter>();
List<IMyGasTank> hydrogenTanks = new List<IMyGasTank>();
List<IMyTerminalBlock> allGridBlocks = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> cargoBlocks = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> allInventoryBlocks = new List<IMyTerminalBlock>();
List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
List<IMyTextPanel> lcdPanels = new List<IMyTextPanel>();
List<IMyTextSurface> textSurfaces = new List<IMyTextSurface>();
List<IMyTextPanel> debugPanels = new List<IMyTextPanel>();
IMyTerminalBlock referenceWorkTool = null;

// Dock and job positions
WaypointInfo homeDock = new WaypointInfo();
WaypointInfo homeDockBackup = new WaypointInfo();
WaypointInfo jobDockBackup = new WaypointInfo();
bool useOldHome = false;
bool useOldConnector2 = false;
bool useOldPath = false;

// Path recording
bool isRecording = false;
List<String> thrusterTypeNames = new List<string>();
List<WaypointInfo> waypoints = new List<WaypointInfo>();
List<WaypointInfo> waypointsBackup = new List<WaypointInfo>();
int dockDetectCounter = 0;
double waypointSpacing = 0;

// Job state
WaypointInfo jobPosition = new WaypointInfo();
Vector3 miningForward;
Vector3 miningDown;
JobState jobState = JobState.NoJob;
StartPosition jobStartPos = StartPosition.TopLeft;
int jobWidth = 0;
int jobHeight = 0;
double jobProgress = 0;
bool damageCheckActive = false;

// Job config
UndockConfig undockConfig1 = new UndockConfig();
UndockConfig undockConfig2 = new UndockConfig();
DamageBehavior onDamageBehavior = DamageBehavior.ReturnHome;
bool whenDoneReturnHome = true;
StartPosition startPosition = StartPosition.TopLeft;
int configWidth = 3;
int configHeight = 3;
int configDepth = 30;
DepthMode depthMode = DepthMode.Default;
bool weightLimitEnabled = true;
bool invBalancingEnabled = true;
bool enableDrillsBothWays = true;
bool toggleSortersEnabled = false;
EjectionMode ejectionMode = EjectionMode.Off;
bool unloadIce = true;
float maxLoadPercent = 90;
float minBatteryPercent = 20;
float minUraniumKg = 5;
float minHydrogenPercent = 20;
float accelerationFactor = 0.70f;
float workSpeedForward = 1.50f;
float workSpeedBackward = 2.50f;
float widthOverlap = 0f;
float heightOverlap = 0f;

// Navigation state
NavState currentNavState;
JobState lastJobState;
NavState stopAtState;
PathFollowData pathFollowData = null;

// Hole tracking
int holeCol = 0;
int holeRow = 0;
int currentHoleIndex = 0;
int lastHoleIndex = 0;
int runtimeMaxDepth = 30;
int pathWaypointIndex = 0;
bool stateFirstTick = true;
Vector3 miningTargetPos;
double distToTarget = 0;
bool returnToJobAfter = false;
int waypointsRemaining = 0;
int stuckCounter = 0;
int autoDepthCounter = 0;
int pathDirection = 0;
int oreCountBeforeEject = 0;
Vector3 pathSegmentDir = new Vector3();
float lastDistToTarget = 0;
float stuckSensitivity = 0;
int waypointCalcIndex = 0;
WaypointInfo spaceWaypoint = null; // zero-gravity waypoint used by CalculateMaxWeight (separate from path-following's currentPathWaypoint)
float lastOreAmount = 0;
float autoDepthRef = 0;
float currentMineDepth = 0;
float maxMineDepth = 0;

// Mining state
bool isFirstDescent = false;
bool unloadComplete = false;
bool uraniumOk = false;
bool hydrogenOk = false;
bool batteryOk = false;
DateTime dockStartTime = new DateTime();
WaypointInfo currentPathWaypoint = null;

// Navigation target
WaypointInfo currentDockTarget = null;

// Energy
float batteryPercent = 0;
BatteryState batteryState;
float hydrogenPercent = 0;
float uraniumAmount = 0;
String lowestUraniumReactor = "";

// Cargo
float loadPercent = 0;
float currentVolume = 0;
float maxVolume = 0;
List<ItemInfo> itemList = new List<ItemInfo>();

// Weight limit
float maxWeight = 0;
bool simulateShipFull = false;

// Flight control
bool thrustEnabled = false;
bool slowOnMisalign = false;
bool thrustActive = false;
bool softApproach = false;
float maxApproachSpeed = 0;
float pathSpeed = 0;
Vector3 flightPathDir = new Vector3();
Vector3 navTargetPos = new Vector3();
Vector3 thrustMultiplier = new Vector3(1, 1, 1);
float thrustEfficiency = 1;
Vector3 thrustForce = new Vector3();

// Gyro
bool isAligned = false;
bool gyroActive = false;
bool lookAtTarget = false;
bool alignToGravity = false;
float currentAngleError = 0;
float alignThreshold = 2;
Vector3 targetLeft;
Vector3 targetForward;
Vector3 targetDown;

// Thrust maps
float[,] totalThrustMap = new float[3, 2];
Dictionary<String, float[,]> thrustByType = new Dictionary<string, float[,]>();

// Inventory balance
Dictionary<String, float> volumePerItem = new Dictionary<String, float>();
int balanceFailCount = 0;

// Profiling
Dictionary<String, float[]> profilingData = new Dictionary<string, float[]>();
float profilingStart;
bool debugMode = false;

// Menu
int animCounter = 0;
int infoRotateIndex = 0;
int infoToggle = 0;
int infoSection = 0;
String statusMessage = "";
int[] menuIndexPerPage = new int[Enum.GetValues(typeof(MenuPage)).Length];
bool menuJustNavigated = false;
int menuItemCount = 0;
int menuIndex = 0;
MenuPage currentMenu = MenuPage.Main;

// UI scroll state
int timerIdx1 = 0, timerIdx2 = 0, timerIdx3 = 0, timerIdx4 = 0;
int scrollOffset1 = 0, scrollOffset2 = 0;

// Broadcast
bool broadcastEnabled = false;
bool antennaError = false;
String shipName = "";
String broadcastChannel = "";
IMyBroadcastListener broadcastListener = null;
bool firstBroadcastInit = true;

// Controller
List<ShipInfo> connectedShips = null;
ShipInfo selectedShip = null;
int controllerPageCount1 = 0, controllerPageCount2 = 0;
int controllerPageIdx1 = 0, controllerPageIdx2 = 0;
int controllerScrollState1 = 0, controllerScrollState2 = 0, controllerScrollState3 = 0;

// Misc
String lastArgument = "";
Exception lastError = null;
bool resetRequested = false;
JobState savedJobState = JobState.NoJob;
bool dampenerState = false; // as in the original: unknown at load, so the first SetDampeners(true) always applies
bool justLoaded = true;      // true until the first block scan after the script (re)starts with the world
bool resumePending = false;  // resume the saved Active job once the ship's position is known
String waitingStatusText = "";
bool undockRequested = false;
bool sorterState = true;
int initialDamageCount = 0;
int[] spiralState = null;
Vector3 cachedGridPos = new Vector3();
bool gridPosCached = false;

bool IsOnSameGrid(IMyTerminalBlock block) => block.CubeGrid == Me.CubeGrid;

// ========================== 05_Constructor.cs ==========================
// ========================== CONSTRUCTOR ==========================

Program()
{
    gridTerminalSystem = GridTerminalSystem;
    Runtime.UpdateFrequency = UpdateFrequency.Update10;
    LoadSettings(); // PB Custom Data, before anything uses a setting

    if (HasTag(Me, controllerTag, true))
        shipMode = ShipMode.Controller;

    NavigateToMenu(MenuPage.Main);

    if (shipMode != ShipMode.Controller)
    {
        statusMessage = "Welcome to [PAM]!";
        DataResult restoreResult = RestoreData();

        if (shipMode == ShipMode.Unknown)
        {
            List<IMyShipDrill> drills = new List<IMyShipDrill>();
            List<IMyShipGrinder> grinders = new List<IMyShipGrinder>();
            gridTerminalSystem.GetBlocksOfType(drills, q => q.CubeGrid == Me.CubeGrid);
            gridTerminalSystem.GetBlocksOfType(grinders, q => q.CubeGrid == Me.CubeGrid);

            if (drills.Count > 0)
            {
                shipMode = ShipMode.Miner;
                statusMessage = "Miner mode enabled!";
            }
            else if (grinders.Count > 0)
            {
                shipMode = ShipMode.Grinder;
                statusMessage = "Grinder mode enabled!";
            }
            else
            {
                shipMode = ShipMode.Shuttle;
                ShowStatusMessage(StatusMsg.Shuttle);
            }
        }

        if (restoreResult == DataResult.Success) needPositionUpdate = false;
        if (restoreResult == DataResult.Failed) statusMessage = "Data restore failed!";
        if (restoreResult == DataResult.NewVersion) statusMessage = "New version, wipe data";
    }
    if (cfgBad != "") statusMessage = "Bad setting, default used: " + cfgBad;
}

// ========================== 06_Main.cs ==========================
// ========================== MAIN ==========================

// run time of the script (Runtime.LastRunTimeMs = the previous run): average, and the highest run
// in the last 10 s
double execAvg = 0, execPeak = 0;
DateTime execPeakTime = DateTime.MinValue;

void Main(string argument, UpdateType updateSource)
{
    double ms = Runtime.LastRunTimeMs;
    execAvg = execAvg * 0.95 + ms * 0.05;
    if (ms > execPeak || (TimeNow() - execPeakTime).TotalSeconds > 10) { execPeak = ms; execPeakTime = TimeNow(); }
    try
    {
        if (lastError != null)
        {
            // "RETRY" restarts the script after an exception without recompiling: stop all overrides and rescan.
            if (argument.Trim().ToUpper() == "RETRY")
            {
                lastError = null;
                firstRun = true;
                scanPhase = 0;
                if (shipMode != ShipMode.Controller) StopAll();
                statusMessage = "Restarted after error";
            }
            else { HandleError(); PrintDisplays(); return; }
        }

        isTick = (updateSource & UpdateType.Update10) != 0;
        if (isTick) tickCounter++;
        isSlowTick = tickCounter >= 10;
        if (isSlowTick) tickCounter = 0;

        if (isTick)
        {
            animCounter++;
            if (animCounter > 4) animCounter = 0;
            scanCountdown = (int)Math.Max(0, 10 - (TimeNow() - lastScanTime).TotalSeconds);
        }

        if (argument != "") lastArgument = argument;

        if (shipMode != ShipMode.Controller)
            ShipMainLoop(argument);
        else
            ControllerMainLoop(argument);

        menuJustNavigated = false;

        try
        {
            int currentInstructions = Runtime.CurrentInstructionCount;
            float ratio = SafeDiv(currentInstructions, Runtime.MaxInstructionCount);
            if (ratio > 0.90) statusMessage = "Max. instructions >90%";
            if (ratio > maxInstructionPercent) maxInstructionPercent = ratio;

            if (debugMode)
            {
                instructionSamples.Add(currentInstructions);
                while (instructionSamples.Count > 10) instructionSamples.RemoveAt(0);
                avgInstructionCount = 0;
                for (int i = 0; i < instructionSamples.Count; i++) avgInstructionCount += instructionSamples[i];
                avgInstructionCount = SafeDiv(SafeDiv(avgInstructionCount, instructionSamples.Count), Runtime.MaxInstructionCount);

                double lastRunMs = Runtime.LastRunTimeMs;
                if (initialized && lastRunMs > maxRuntime) maxRuntime = lastRunMs;
                runtimeSamples.Add((int)(lastRunMs * 1000f));
                while (runtimeSamples.Count > 10) runtimeSamples.RemoveAt(0);
                avgRuntime = 0;
                for (int i = 0; i < runtimeSamples.Count; i++) avgRuntime += runtimeSamples[i];
                avgRuntime = SafeDiv(avgRuntime, runtimeSamples.Count) / 1000f;
            }
        }
        catch { maxInstructionPercent = 0; }
    }
    catch (Exception e) { lastError = e; }
}

// ========================== 07_ShipLoop.cs ==========================
// ========================== SHIP MAIN LOOP ==========================

void ShipMainLoop(string argument)
{
    bool shouldBroadcast = false;
    String broadcastExtra = "";

    if (setupErrorLevel <= 1 && !IsInternalMessage(argument))
        ProcessShipArgument(argument);

    // Drain other ships' status broadcasts; handle at most one controller command per run
    // (its echo in broadcastExtra is what the controller uses as the acknowledgement).
    int msgBudget = 50;
    while (broadcastListener != null && broadcastListener.HasPendingMessage && msgBudget-- > 0)
    {
        MyIGCMessage msg = broadcastListener.AcceptMessage();
        String data = msg.Data as String;
        if (data == null) continue;
        String senderId = "";
        if (setupErrorLevel <= 1 && ParseBroadcastMessage(ref data, out senderId, out broadcastExtra) && senderId == BROADCAST_SELF_ID)
        {
            ProcessShipArgument(data);
            shouldBroadcast = true;
            break;
        }
    }

    bool isIdleOptimize = initialized && currentNavState == NavState.Idle && !isAligning && !shouldBroadcast && scanPhase == 0 && !isRecording;
    if (isSlowTick && currentNavState != NavState.Idle) shouldBroadcast = true;

    if ((isTick && !isIdleOptimize) || (isSlowTick && isIdleOptimize))
    {
        // Scan phase 1
        if (scanPhase == 0 && (scanCountdown <= 0 || firstRun))
        {
            wasSetupError = setupErrorLevel > 0;
            setupErrorLevel = 0;
            scanPhase = 1;
            StartProfile();
            ScanBlocks1();
            InitBroadcast();
            EndProfile("Scan 1");
        }
        // Scan phase 2
        else if (scanPhase == 1)
        {
            scanPhase = 2;
            StartProfile();
            ScanBlocks2();
            EndProfile("Scan 2");
        }
        // Scan phase 3
        else if (scanPhase == 2)
        {
            scanPhase = 0;
            StartProfile();
            ScanBlocks3();
            ScanCameras();
            if (setupErrorLevel <= 1) LearnDocks();
            EndProfile("Scan 3");
            lastScanTime = TimeNow();

            if (setupErrorLevel <= 1 && needPositionUpdate)
                referencePosition = GetLocalPosition(remoteControl, remoteControl.CenterOfMass);
            needPositionUpdate = false;

            if (firstRun)
            {
                firstRun = false;
                // A job that was running when the world was saved (server restart/crash) is resumed
                // instead of left paused. RETRY after an error also sets firstRun but not justLoaded.
                bool wasRunning = jobState == JobState.Active;
                StopAll();
                if (justLoaded && resumeAfterRestart && wasRunning && setupErrorLevel <= 1) resumePending = true;
            }
            justLoaded = false;
            if (wasSetupError && setupErrorLevel == 0) statusMessage = "Setup complete";
        }
        else
        {
            // Inventory balancing
            if (jobState == JobState.Active && shipMode != ShipMode.Shuttle)
            {
                StartProfile();
                BalanceInventory();
                EndProfile("Inv balance");
            }

            // Cycled updates
            StartProfile();
            switch (updatePhase)
            {
                case 0: UpdateCargoLoad(); break;
                case 1: UpdateInventory(); break;
                case 2: UpdateBatteryState(); break;
                case 3: UpdateUranium(); break;
                case 4: UpdateHydrogen(); break;
                case 5: CheckDamage(); break;
                case 6: CalculateThrustVectors(remoteControl); break;
            }
            EndProfile("Update: " + updatePhase);

            updatePhase++;
            if (updatePhase > 6)
            {
                updatePhase = 0;
                initialized = true;

                if (savedJobState != JobState.NoJob)
                {
                    switch (savedJobState)
                    {
                        case JobState.ActiveHome: ContinueJob(); break;
                        case JobState.ActiveJob: ContinueJob(); break;
                        case JobState.Active: ContinueJob(); break;
                        case JobState.MoveHome: FlyToHomePosition(); break;
                        case JobState.MoveToJob: FlyToJobPosition(); break;
                    }
                    savedJobState = JobState.NoJob;
                }
            }
        }

        if (!firstRun)
        {
            if (!IsBlockValid(remoteControl, true))
            {
                remoteControl = null;
                needPositionUpdate = true;
                setupErrorLevel = 2;
            }

            if (setupErrorLevel >= 2 && currentNavState != NavState.Idle)
                StopAll();

            if (setupErrorLevel <= 1)
            {
                shipMass = remoteControl.CalculateShipMass().PhysicalMass;
                shipSpeed = (float)remoteControl.GetShipSpeed();
                shipPosition = TransformToWorld(remoteControl, referencePosition);
                forwardDirection = remoteControl.WorldMatrix.Forward;
                leftDirection = remoteControl.WorldMatrix.Left;
                downDirection = remoteControl.WorldMatrix.Down;
                UpdatePathRecording();
                UpdateTraffic();
                UpdateDockClearance();

                if (resumePending)
                {
                    resumePending = false;
                    ContinueJob();
                    if (currentNavState != NavState.Idle) statusMessage = "Job resumed after restart";
                }

                if (currentNavState != NavState.Idle)
                {
                    isAligning = false;
                    SetDampeners(false);
                    CalculateMaxWeight(false);

                    String stateName = GetNavStateName(currentNavState) + " " + (int)currentNavState;
                    StartProfile();
                    UpdateNavigation();
                    UpdateProgress(false);
                    EndProfile(stateName);
                    UpdateTrafficAvoidance();
                    UpdateObstacleCheck();

                    StartProfile();
                    UpdateFlight();
                    EndProfile("Thruster");

                    StartProfile();
                    UpdateGyros();
                    EndProfile("Gyroscope");
                }
                else
                {
                    if (isAligning)
                    {
                        if (IsNearPlanet())
                        {
                            AlignToDown(downDirection, forwardDirection, leftDirection, 0.25f, true);
                            UpdateGyros();
                            statusMessage = "Aligning to planet: " + Math.Round(currentAngleError - 0.25f, 2) + "°";
                            if (isAligned) HandleAlignment(true, true);
                        }
                        else HandleAlignment(true, true);
                    }
                }
                undockRequested = false;
            }
        }
    }

    StartProfile();
    PrintDisplays();
    EndProfile("Print");

    if (shouldBroadcast || broadcastCountdown <= 0)
    {
        StartProfile();
        BroadcastShipState(broadcastExtra);
        EndProfile("Broadcast");
        broadcastCountdown = 4;
    }
    else if (isSlowTick) broadcastCountdown--;
}

// ========================== 08_Commands.cs ==========================
// ========================== ARGUMENT PROCESSING ==========================

void ProcessShipArgument(string argument)
{
    if (argument == "") return;
    var parts = argument.ToUpper().Split(' ');
    parts.DefaultIfEmpty("");
    var cmd = parts.ElementAtOrDefault(0);
    var arg1 = parts.ElementAtOrDefault(1);
    var arg2 = parts.ElementAtOrDefault(2);
    var arg3 = parts.ElementAtOrDefault(3);
    String invalidMsg = "Invalid argument: " + argument;
    bool unhandled = false;

    switch (cmd)
    {
        case "UP": this.MenuUp(false); break;
        case "DOWN": this.MenuDown(false); break;
        case "UPLOOP": this.MenuUp(true); break;
        case "DOWNLOOP": this.MenuDown(true); break;
        case "APPLY": this.BuildMenuScreen(true); break;
        case "MRES": menuIndex = 0; break;
        case "STOP": this.StopAll(); break;
        case "PATHHOME": { this.StopAll(); this.StartPathRecording(); } break;
        case "PATH": { this.StopAll(); this.StartPathRecording(); homeDock.isSet = true; } break;
        case "START": { this.StopAll(); StartNewJob(); } break;
        case "ALIGN": { HandleAlignment(!isAligning, false); } break;
        case "CONT": { this.StopAll(); this.ContinueJob(); } break;
        case "JOBPOS": { this.StopAll(); this.FlyToJobPosition(); } break;
        case "HOMEPOS": { this.StopAll(); this.FlyToHomePosition(); } break;
        case "FULL": { simulateShipFull = true; } break;
        case "RESET": { resetRequested = true; setupErrorLevel = 2; } break;
        case "EXPORT": { ExportData(); } break;
        case "IMPORT": { ImportData(arg1 == "JOB"); } break;
        default: unhandled = true; break;
    }

    if (shipMode != ShipMode.Shuttle)
    {
        switch (cmd)
        {
            case "SHUTTLE": { EnableShuttleMode(); } break;
            case "CFGS": { if (!ConfigSize(arg1, arg2, arg3)) statusMessage = invalidMsg; } break;
            case "CFGB": { if (!ConfigBehavior(arg1, arg2)) statusMessage = invalidMsg; } break;
            case "CFGL":
            {
                if (!ParseConfig(ref maxLoadPercent, true, ConfigType.MaxLoad, arg1, "") || !ConfigWeightLimit(arg2))
                    statusMessage = invalidMsg;
            } break;
            case "CFGE":
            {
                if (!ParseConfig(ref minUraniumKg, true, ConfigType.MinUranium, arg1, "IG") ||
                    !ParseConfig(ref minBatteryPercent, true, ConfigType.MinBattery, arg2, "IG") ||
                    !ParseConfig(ref minHydrogenPercent, true, ConfigType.MinHydrogen, arg3, "IG"))
                    statusMessage = invalidMsg;
            } break;
            case "CFGA": { if (!ParseConfig(ref accelerationFactor, false, ConfigType.Acceleration, arg1, "")) statusMessage = invalidMsg; } break;
            case "CFGW":
            {
                if (!ParseConfig(ref workSpeedForward, false, ConfigType.WorkSpeed, arg1, "") ||
                    !ParseConfig(ref workSpeedBackward, false, ConfigType.WorkSpeed, arg2, ""))
                    statusMessage = invalidMsg;
            } break;
            case "NEXT": { AdvanceHole(false); } break;
            case "PREV": { AdvanceHole(true); } break;
            default: if (unhandled) statusMessage = invalidMsg; break;
        }
    }
    else
    {
        switch (cmd)
        {
            case "UNDOCK": { undockRequested = true; } break;
            default: if (unhandled) statusMessage = invalidMsg; break;
        }
    }
}

// ========================== 09_ShuttleConfig.cs ==========================
// ========================== SHUTTLE / ALIGNMENT / CONFIG ==========================

void EnableShuttleMode()
{
    StopAll();
    shipMode = ShipMode.Shuttle;
    ShowStatusMessage(StatusMsg.Shuttle);
    jobPosition.isSet = false;
    homeDock.isSet = false;
    sensor = null;
    workTools.Clear();
    jobState = JobState.NoJob;
}

void HandleAlignment(bool enable, bool done)
{
    if (!enable) statusMessage = "Aligning canceled";
    if (done) statusMessage = "Aligning done";
    if (done || !enable)
    {
        isAligning = false;
        StopGyroOverride();
        SetGyroOverride(false, 0, 0, 0, 0);
        return;
    }
    if (IsNearPlanet()) isAligning = true;
}

bool ConfigSize(String widthStr, String heightStr, String depthStr)
{
    bool wasActive = jobState == JobState.Active;
    int w, h, d;
    if (int.TryParse(widthStr, out w) && int.TryParse(heightStr, out h) && int.TryParse(depthStr, out d))
    {
        this.StopAll();
        configWidth = w; configHeight = h; configDepth = d;
        ValidateJobConfig(false);
        UpdateJobConfig(false, false);
        if (wasActive) ContinueJob();
        return true;
    }
    return false;
}

bool ConfigWeightLimit(String arg)
{
    if (arg == "ON") { weightLimitEnabled = true; return true; }
    if (arg == "OFF") { weightLimitEnabled = false; return true; }
    return false;
}

bool ConfigBehavior(String doneArg, String damageArg)
{
    bool ok = true;
    if (doneArg == "HOME") whenDoneReturnHome = true;
    else if (doneArg == "STOP") whenDoneReturnHome = false;
    else ok = false;

    if (damageArg == "HOME") onDamageBehavior = DamageBehavior.ReturnHome;
    else if (damageArg == "STOP") onDamageBehavior = DamageBehavior.Stop;
    else if (damageArg == "JOB") onDamageBehavior = DamageBehavior.FlyToJob;
    else if (damageArg == "IG") onDamageBehavior = DamageBehavior.Ignore;
    else ok = false;
    return ok;
}

// ========================== 10_MenuNav.cs ==========================
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

// ========================== 11_MenuBuilder.cs ==========================
// ========================== MENU SCREEN BUILDER ==========================
// (BuildMenuScreen is extremely large - handles all menu pages)

bool AddMenuItem(ref String text, int selected, int index, bool applyAction, String label)
{
    menuItemCount += 1;
    if (selected == index) label = ">" + label + (animCounter >= 2 ? " ." : "");
    else label = " " + label;
    text += label + "\n";
    return selected == index && applyAction;
}

String BuildMenuScreen(bool applyAction)
{
    int idx = 0;
    int sel = menuIndex;
    menuItemCount = 0;
    String separator = "———————————————\n";
    String thinSep = "--------------------------------------------\n";
    String text = "";

    text += GetJobStateName(jobState) + " | " + (homeDock.isSet ? "Ready to dock" : "No dock") + "\n";
    text += separator;

    double targetDist = Math.Max(Math.Round(this.distToTarget), 0);

    // ---- MAIN MENU ----
    if (currentMenu == MenuPage.Main)
    {
        bool isShuttle = shipMode == ShipMode.Shuttle;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Record path & set home")) StartPathRecording();
        if (shipMode == ShipMode.Miner)
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Setup mining job")) NavigateToMenu(MenuPage.JobSetup);
        if (shipMode == ShipMode.Grinder)
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Setup grinding job")) NavigateToMenu(MenuPage.JobSetup);
        if (shipMode == ShipMode.Shuttle)
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Setup shuttle job")) NavigateToMenu(MenuPage.ShuttlePage1);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Continue job")) ContinueJob();
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Fly to home position")) FlyToHomePosition();
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Fly to job position")) FlyToJobPosition();
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Behavior settings"))
            if (isShuttle) NavigateToMenu(MenuPage.ShuttleBehavior); else NavigateToMenu(MenuPage.BehaviorSettings);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Info")) NavigateToMenu(MenuPage.InfoPage);
    }
    // ---- JOB SETUP ----
    else if (currentMenu == MenuPage.JobSetup)
    {
        double widthMeters = Math.Round(configWidth * toolWidth, 1);
        double heightMeters = Math.Round(configHeight * toolHeight, 1);
        String subText = "";

        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Start new job!")) StartNewJob();
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Change current job")) { UpdateJobConfig(false, false); NavigateToMenu(MenuPage.Main); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Width + (Width: " + configWidth + " = " + widthMeters + "m)")) { AdjustValue(ref configWidth, 5, 20, 1); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Width -")) { AdjustValue(ref configWidth, -5, 20, -1); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Height + (Height: " + configHeight + " = " + heightMeters + "m)")) { AdjustValue(ref configHeight, 5, 20, 1); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Height -")) { AdjustValue(ref configHeight, -5, 20, -1); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Depth + (" + (depthMode == DepthMode.Default ? "Depth" : "Min") + ": " + configDepth + "m)")) { AdjustValue(ref configDepth, 5, 50, 2); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Depth -")) { AdjustValue(ref configDepth, -5, 50, -2); ValidateJobConfig(true); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Depth mode: " + GetDepthModeName(depthMode))) { depthMode = CycleEnum(depthMode); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Start pos: " + GetStartPosName(startPosition))) { startPosition = CycleEnum(startPosition); }
        if (shipMode == ShipMode.Grinder && depthMode == DepthMode.AutoStone) depthMode = CycleEnum(depthMode);
        text += ScrollText(8, subText, sel, ref scrollOffset1);
    }
    // ---- SHUTTLE PAGE 1 ----
    else if (currentMenu == MenuPage.ShuttlePage1)
    {
        float[] delayValues = new float[] { 0, 3, 10, 30, 60, 300, 600, 1200, 1800 };
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Next")) NavigateToMenu(MenuPage.ShuttlePage2);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) NavigateToMenu(MenuPage.Main);
        text += " Leave connector 1:\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " - " + GetUndockTriggerName(undockConfig1.trigger))) undockConfig1.trigger = CycleEnum(undockConfig1.trigger);
        if (!undockConfig1.HasDelay()) text += "\n";
        else if (AddMenuItem(ref text, sel, idx++, applyAction, " - Delay: " + FormatTime((int)undockConfig1.delay))) undockConfig1.delay = CycleArrayValue(undockConfig1.delay, delayValues);
        text += " Leave connector 2:\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " - " + GetUndockTriggerName(undockConfig2.trigger))) undockConfig2.trigger = CycleEnum(undockConfig2.trigger);
        if (!undockConfig2.HasDelay()) text += "\n";
        else if (AddMenuItem(ref text, sel, idx++, applyAction, " - Delay: " + FormatTime((int)undockConfig2.delay))) undockConfig2.delay = CycleArrayValue(undockConfig2.delay, delayValues);
    }
    // ---- SHUTTLE PAGE 2 ----
    else if (currentMenu == MenuPage.ShuttlePage2)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Start job!")) StartNewJob();
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) NavigateToMenu(MenuPage.ShuttlePage1);
        text += " Timer: \"Docking connector 1\":\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " = " + (undockConfig1.dockTimerName != "" ? undockConfig1.dockTimerName : "-"))) undockConfig1.dockTimerName = NextTimerBlock(ref timerIdx1);
        text += " Timer: \"Leaving connector 1\":\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " = " + (undockConfig1.leaveTimerName != "" ? undockConfig1.leaveTimerName : "-"))) undockConfig1.leaveTimerName = NextTimerBlock(ref timerIdx3);
        text += " Timer: \"Docking connector 2\":\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " = " + (undockConfig2.dockTimerName != "" ? undockConfig2.dockTimerName : "-"))) undockConfig2.dockTimerName = NextTimerBlock(ref timerIdx2);
        text += " Timer: \"Leaving connector 2\":\n";
        if (AddMenuItem(ref text, sel, idx++, applyAction, " = " + (undockConfig2.leaveTimerName != "" ? undockConfig2.leaveTimerName : "-"))) undockConfig2.leaveTimerName = NextTimerBlock(ref timerIdx4);
    }
    // ---- JOB RUNNING ----
    else if (currentMenu == MenuPage.JobRunning)
    {
        String loadText = maxLoadPercent + " %";
        if (isSlowTick) infoRotateIndex++;
        if (infoRotateIndex > 1) { infoRotateIndex = 0; infoToggle++; if (infoToggle > 1) infoToggle = 0; }
        bool[] skipSection = new bool[] { reactors.Count == 0, batteryState == BatteryState.None, hydrogenTanks.Count == 0 };
        int tries = 0;
        while (true)
        {
            tries++;
            infoSection++;
            if (infoSection > skipSection.Length - 1) infoSection = 0;
            if (tries >= skipSection.Length) break;
            if (!skipSection[infoSection]) break;
        }

        bool isShuttle = shipMode == ShipMode.Shuttle;
        if (!isShuttle && weightLimitEnabled && maxWeight != -1 && infoToggle == 0)
            loadText = maxWeight < 1000000 ? Math.Round(maxWeight) + " Kg" : Math.Round(maxWeight / 1000) + " t";

        if (AddMenuItem(ref text, sel, idx++, applyAction, " Stop!")) { StopAll(); NavigateToMenu(MenuPage.Main); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Behavior settings"))
            if (!isShuttle) NavigateToMenu(MenuPage.BehaviorSettings); else NavigateToMenu(MenuPage.ShuttleBehavior);
        if (!isShuttle) { if (AddMenuItem(ref text, sel, idx++, applyAction, " Next hole")) AdvanceHole(false); }
        else if (AddMenuItem(ref text, sel, idx++, applyAction, " Undock")) undockRequested = true;

        text += thinSep;
        if (!isShuttle) text += "Progress: " + Math.Round(jobProgress, 1) + " %\n";
        text += "State: " + GetNavStateName(currentNavState) + " " + targetDist + "m \n";
        text += "Load: " + loadPercent + " % Max: " + loadText + " \n";

        if (infoSection == 0) text += "Uranium: " + (reactors.Count == 0 ? "No reactors" : Math.Round(uraniumAmount, 1) + "Kg " + (minUraniumKg == -1 ? "" : " Min: " + minUraniumKg + " Kg")) + "\n";
        if (infoSection == 1) text += "Battery: " + (batteryState == BatteryState.None ? GetBatteryStateName(batteryState) : batteryPercent + "% " + (minBatteryPercent == -1 || isShuttle ? "" : " Min: " + minBatteryPercent + " %")) + "\n";
        if (infoSection == 2) text += "Hydrogen: " + (hydrogenTanks.Count == 0 ? "No tanks" : Math.Round(hydrogenPercent, 1) + "% " + (minHydrogenPercent == -1 || isShuttle ? "" : " Min: " + minHydrogenPercent + " %")) + "\n";
    }
    // ---- BEHAVIOR SETTINGS ----
    else if (currentMenu == MenuPage.BehaviorSettings)
    {
        String subText = "";
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Back")) { if (jobState == JobState.Active) NavigateToMenu(MenuPage.JobRunning); else NavigateToMenu(MenuPage.Main); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Max load: " + maxLoadPercent + "%")) AdjustConfig(ref maxLoadPercent, maxLoadPercent <= 80 ? -10 : -5, ConfigType.MaxLoad, false);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Weight limit: " + (weightLimitEnabled ? "On" : "Off"))) weightLimitEnabled = !weightLimitEnabled;
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Ejection: " + GetEjectionModeName(ejectionMode))) ejectionMode = CycleEnum(ejectionMode);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Toggle sorters: " + (toggleSortersEnabled ? "On" : "Off"))) { toggleSortersEnabled = !toggleSortersEnabled; if (toggleSortersEnabled) SetSortersEnabled(sorterState); }
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Unload ice: " + (unloadIce ? "On" : "Off"))) unloadIce = !unloadIce;
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Uranium: " + (minUraniumKg == -1 ? "Ignore" : "Min " + minUraniumKg + "Kg"))) AdjustConfig(ref minUraniumKg, (minUraniumKg > 5 ? -5 : -1), ConfigType.MinUranium, true);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Battery: " + (minBatteryPercent == -1 ? "Ignore" : "Min " + minBatteryPercent + "%"))) AdjustConfig(ref minBatteryPercent, -5, ConfigType.MinBattery, true);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Hydrogen: " + (minHydrogenPercent == -1 ? "Ignore" : "Min " + minHydrogenPercent + "%"))) AdjustConfig(ref minHydrogenPercent, -10, ConfigType.MinHydrogen, true);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " When done: " + (whenDoneReturnHome ? "Return home" : "Stop"))) whenDoneReturnHome = !whenDoneReturnHome;
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " On damage: " + GetDamageBehaviorName(onDamageBehavior))) onDamageBehavior = CycleEnum(onDamageBehavior);
        if (AddMenuItem(ref subText, sel, idx++, applyAction, " Advanced...")) NavigateToMenu(MenuPage.AdvancedSettings);
        text += ScrollText(8, subText, sel, ref scrollOffset2);
    }
    // ---- ADVANCED SETTINGS ----
    else if (currentMenu == MenuPage.AdvancedSettings)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { if (jobState == JobState.Active) NavigateToMenu(MenuPage.JobRunning); else NavigateToMenu(MenuPage.Main); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, (shipMode == ShipMode.Grinder ? " Grinder" : " Drill") + " inv. balancing: " + (invBalancingEnabled ? "On" : "Off"))) invBalancingEnabled = !invBalancingEnabled;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Enable" + (shipMode == ShipMode.Grinder ? " grinders" : " drills") + ": " + (enableDrillsBothWays ? "Fwd + Bwd" : "Fwd"))) enableDrillsBothWays = !enableDrillsBothWays;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Work speed fwd.: " + workSpeedForward + "m/s")) AdjustConfig(ref workSpeedForward, 0.5f, ConfigType.WorkSpeed, false);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Work speed bwd.: " + workSpeedBackward + "m/s")) AdjustConfig(ref workSpeedBackward, 0.5f, ConfigType.WorkSpeed, false);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Acceleration: " + Math.Round(accelerationFactor * 100f) + "%" + (accelerationFactor > 0.80f ? " (risky)" : ""))) AdjustConfig(ref accelerationFactor, 0.1f, ConfigType.Acceleration, false);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Width overlap: " + widthOverlap * 100f + "%")) ChangeOverlap(true, 0.05f);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Height overlap: " + heightOverlap * 100f + "%")) ChangeOverlap(false, 0.05f);
    }
    // ---- SHUTTLE BEHAVIOR ----
    else if (currentMenu == MenuPage.ShuttleBehavior)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { if (jobState == JobState.Active) NavigateToMenu(MenuPage.JobRunning); else NavigateToMenu(MenuPage.Main); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Max load: " + maxLoadPercent + "%")) AdjustConfig(ref maxLoadPercent, maxLoadPercent <= 80 ? -10 : -5, ConfigType.MaxLoad, false);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Unload ice: " + (unloadIce ? "On" : "Off"))) unloadIce = !unloadIce;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Uranium: " + (minUraniumKg == -1 ? "Ignore" : "Min " + minUraniumKg + "Kg"))) AdjustConfig(ref minUraniumKg, (minUraniumKg > 5 ? -5 : -1), ConfigType.MinUranium, true);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Battery: " + (minBatteryPercent == -1 ? "Ignore" : "Charge up"))) minBatteryPercent = (minBatteryPercent == -1 ? 1 : -1);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Hydrogen: " + (minHydrogenPercent == -1 ? "Ignore" : "Fill up"))) minHydrogenPercent = (minHydrogenPercent == -1 ? 1 : -1);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " On damage: " + GetDamageBehaviorName(onDamageBehavior))) onDamageBehavior = CycleEnum(onDamageBehavior);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Acceleration: " + Math.Round(accelerationFactor * 100f) + "%" + (accelerationFactor > 0.80f ? " (risky)" : ""))) AdjustConfig(ref accelerationFactor, 0.1f, ConfigType.Acceleration, false);
    }
    // ---- RECORDING ----
    else if (currentMenu == MenuPage.Recording)
    {
        double lastWpDist = 0;
        if (waypoints.Count > 0) lastWpDist = Vector3.Distance(waypoints.Last().position, shipPosition);
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Stop path recording")) StopPathRecording();
        if (shipMode != ShipMode.Shuttle)
        {
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Home: " + (useOldHome ? "Use old home" : (homeDock.isSet ? "Was set! " : "none ")))) useOldHome = !useOldHome;
        }
        else
        {
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Connector 1: " + (useOldHome ? "Use old connector" : (homeDock.isSet ? "Was set! " : "none ")))) useOldHome = !useOldHome;
            if (AddMenuItem(ref text, sel, idx++, applyAction, " Connector 2: " + (useOldConnector2 ? "Use old connector" : (jobPosition.isSet ? "Was set! " : "none ")))) useOldConnector2 = !useOldConnector2;
        }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Path: " + (useOldPath ? "Use old path" : (waypoints.Count > 1 ? "Count: " + waypoints.Count : "none ")))) useOldPath = !useOldPath;
        text += thinSep;
        text += "Wp spacing: " + Math.Round(waypointSpacing) + "m\n";
    }
    // ---- FLYING STATUS ----
    else if (currentMenu == MenuPage.StatusOverview)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Stop")) { StopAll(); NavigateToMenu(MenuPage.Main); }
        text += thinSep;
        text += "State: " + GetNavStateName(currentNavState) + " \n";
        text += "Speed: " + Math.Round(shipSpeed, 1) + "m/s\n";
        text += "Target dist: " + targetDist + "m\n";
        text += "Wp count: " + waypoints.Count + "\n";
        text += "Wp left: " + waypointsRemaining + "\n";
    }
    // ---- INFO PAGE ----
    else if (currentMenu == MenuPage.InfoPage)
    {
        List<IMyTerminalBlock> damaged = GetDamagedBlocks();
        if (isSlowTick) infoRotateIndex++;
        if (infoRotateIndex >= damaged.Count) infoRotateIndex = 0;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Next")) NavigateToMenu(MenuPage.InfoPage2);
        text += thinSep;
        text += "Version: " + VERSION + "\n";
        text += "Ship load: " + Math.Round(loadPercent, 1) + "% " + Math.Round(currentVolume, 1) + " / " + Math.Round(maxVolume, 1) + "\n";
        text += "Uranium: " + (reactors.Count == 0 ? "No reactors" : Math.Round(uraniumAmount, 1) + "Kg " + lowestUraniumReactor) + "\n";
        text += "Battery: " + (batteryState == BatteryState.None ? "" : batteryPercent + "% ") + GetBatteryStateName(batteryState) + "\n";
        text += "Hydrogen: " + (hydrogenTanks.Count == 0 ? "No tanks" : Math.Round(hydrogenPercent, 1) + "% ") + "\n";
        text += "Damage: " + (damaged.Count == 0 ? "None" : "" + (infoRotateIndex + 1) + "/" + damaged.Count + " " + damaged[infoRotateIndex].CustomName) + "\n";
    }
    // ---- INFO PAGE 2 ----
    else if (currentMenu == MenuPage.InfoPage2)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) NavigateToMenu(MenuPage.Main);
        text += thinSep;
        text += "Next scan: " + scanCountdown + "s\n";
        text += "Drill area: " + Math.Round(toolWidth, 1) + "x" + Math.Round(toolHeight, 1) + "m Ship: " + Math.Round(shipFootWidth, 1) + "x" + Math.Round(shipFootHeight, 1) + "m (" + Math.Round(shipDiameter, 1) + "m)\n" + (cameras.Count > 0 ? "Cameras: " + cameraCoverage + "\n" : "");
        text += "Broadcast: " + (broadcastEnabled ? "Online - " + shipName : "Offline") + "\n";
        text += "Max Instructions: " + Math.Round(maxInstructionPercent * 100f, 1) + "% | " + Math.Round(execAvg, 2) + " ms\n";
    }

    if (setupErrorLevel == 2) text = "Fatal setup error\nNext scan: " + scanCountdown + "s\n";
    if (resetRequested) text = "Recompile script now";

    int lineCount = text.Split('\n').Length;
    for (int i = lineCount; i <= 10; i++) text += "\n";
    text += separator;
    text += "Last: " + statusMessage + "\n";
    return text;
}

// ========================== 12_ConfigHelpers.cs ==========================
// ========================== CONFIG HELPERS ==========================

void ChangeOverlap(bool isWidth, float step)
{
    StopAll();
    UpdateJobConfig(true, false);
    if (isWidth) AdjustConfig(ref widthOverlap, step, ConfigType.Overlap, false);
    else AdjustConfig(ref heightOverlap, step, ConfigType.Overlap, false);
    CalculateToolDimensions();
    jobToolW = toolWidth; jobToolH = toolHeight; // new overlap: new hole spacing for the job
    UpdateSensor(true, true, 0, 0);
}

float CycleArrayValue(float current, float[] values)
{
    float result = values[0];
    for (int i = values.Length - 1; i >= 0; i--)
        if (current < values[i]) result = values[i];
    return result;
}

String NextTimerBlock(ref int index)
{
    String result = "";
    if (index >= timerBlocks.Count) index = -1;
    if (index >= 0) result = timerBlocks[index].CustomName;
    index++;
    return result;
}

void TriggerTimerBlock(string name)
{
    if (jobState != JobState.Active) return;
    if (name == "") return;
    IMyTerminalBlock block = gridTerminalSystem.GetBlockWithName(name);
    if (block == null || !(block is IMyTimerBlock))
    {
        statusMessage = "Timerblock " + name + " not found!";
        return;
    }
    ((IMyTimerBlock)block).Trigger();
}

void AdjustValue(ref int value, int bigStep, int threshold, int smallStep)
{
    if (bigStep == 0) return;
    if (value < threshold && smallStep > 0 || value <= threshold && smallStep < 0)
    {
        value += smallStep;
        return;
    }
    int absStep = Math.Abs(bigStep);
    int accumulated = 0;
    int multiplier = 1;
    while (true)
    {
        accumulated += multiplier * absStep * 10;
        if (bigStep < 0 && value - threshold <= accumulated) break;
        if (bigStep > 0 && value - threshold < accumulated) break;
        multiplier++;
    }
    value += multiplier * bigStep;
}

void ValidateJobConfig(bool updateSensor)
{
    configWidth = Math.Max(configWidth, 1);
    configHeight = Math.Max(configHeight, 1);
    configDepth = Math.Max(configDepth, 0);
    UpdateSensor(currentMenu == MenuPage.JobSetup, false, 0, 0);
}

bool ParseConfig(ref float value, bool round, ConfigType type, String input, String ignoreKeyword)
{
    if (input == "") return false;
    float parsed = -1;
    bool ignore = false;
    if (input.ToUpper() == ignoreKeyword) ignore = true;
    else if (!float.TryParse(input, out parsed)) return false;
    else parsed = Math.Max(0, parsed);
    if (round) parsed = (float)Math.Round(parsed);
    AdjustConfig(ref value, parsed, type, ignore, false);
    return true;
}

void AdjustConfig(ref float value, float step, ConfigType type, bool toggleIgnore)
{
    AdjustConfig(ref value, value + step, type, toggleIgnore, true);
}

void AdjustConfig(ref float value, float newValue, ConfigType type, bool toggleIgnore, bool wrap)
{
    float minVal = 0, maxVal = 0;
    if (type == ConfigType.WorkSpeed) { minVal = 0.5f; maxVal = 10f; }
    if (type == ConfigType.Acceleration) { minVal = 0.1f; maxVal = 1f; }
    if (type == ConfigType.Ignore) { minVal = 50f; maxVal = 100f; }
    if (type == ConfigType.MinBattery) { minVal = 5f; maxVal = 30f; }
    if (type == ConfigType.MinUranium) { minVal = 1f; maxVal = 25f; }
    if (type == ConfigType.MaxLoad) { minVal = 10f; maxVal = 95f; }
    if (type == ConfigType.MinHydrogen) { minVal = 10f; maxVal = 90f; }
    if (type == ConfigType.TimerDelay) { minVal = 10f; maxVal = 1800; }
    if (type == ConfigType.Overlap) { minVal = 0.0f; maxVal = 0.75f; }

    if (newValue == -1 && toggleIgnore) { value = -1; return; }
    if (value == -1) toggleIgnore = false;

    bool outOfRange = newValue < minVal || newValue > maxVal;
    if (outOfRange && wrap)
    {
        if (newValue < value) value = maxVal;
        else if (newValue > value) value = minVal;
    }
    else value = newValue;

    if (outOfRange && toggleIgnore) value = -1;
    else value = Math.Max(minVal, Math.Min(value, maxVal));
    value = (float)Math.Round(value, 2);
}

void AdvanceHole(bool previous)
{
    if (previous) currentHoleIndex = Math.Max(0, currentHoleIndex - 1);
    else currentHoleIndex++;
    // co-op record: holes before the new one count as finished, the rest as not finished
    EnsureHoleOrder(); EnsureDoneHoles();
    for (int k = 0; k < holeCells.Count && k < doneHoles.Length; k++) SetHoleDone(holeCells[k], k < currentHoleIndex);
    if (previous && currentHoleIndex < holeCells.Count) Reopen(holeCells[currentHoleIndex], true); // co-op: for the others too
    myHole = -1;
    UpdateProgress(true);
}

T CycleEnum<T>(T current)
{
    int idx = Array.IndexOf(Enum.GetValues(current.GetType()), current);
    idx++;
    if (idx >= EnumCount(current)) idx = 0;
    return (T)Enum.GetValues(current.GetType()).GetValue(idx);
}

int EnumCount<T>(T value)
{
    return Enum.GetValues(value.GetType()).Length;
}

// ========================== 13_PathRecording.cs ==========================
// ========================== PATH RECORDING ==========================

void StartPathRecording()
{
    waypointsBackup.Clear();
    for (int i = 0; i < waypoints.Count; i++) waypointsBackup.Add(waypoints[i]);
    waypoints.Clear();
    isRecording = true;
    homeDockBackup = new WaypointInfo(homeDock);
    jobDockBackup = new WaypointInfo(jobPosition);
    homeDock.isSet = false;
    if (shipMode == ShipMode.Shuttle) jobPosition.isSet = false;

    for (int i = 0; i < thrustByType.Count; i++)
        if (!thrusterTypeNames.Contains(thrustByType.Keys.ElementAt(i)))
            thrusterTypeNames.Add(thrustByType.Keys.ElementAt(i));

    useOldHome = false;
    useOldConnector2 = false;
    useOldPath = false;
    NavigateToMenu(MenuPage.Recording);
}

void StopPathRecording()
{
    if (useOldHome) homeDock = homeDockBackup;
    if (useOldConnector2) jobPosition = jobDockBackup;
    if (useOldPath)
    {
        waypoints.Clear();
        for (int i = 0; i < waypointsBackup.Count; i++) waypoints.Add(waypointsBackup[i]);
    }
    isRecording = false;
    StopAll();
    NavigateToMenu(MenuPage.Main);
}

void UpdatePathRecording()
{
    if (!isRecording) return;
    if (currentNavState != NavState.Idle) { StopPathRecording(); return; }

    if (!homeDockBackup.isSet) useOldHome = false;
    if (!jobDockBackup.isSet) useOldConnector2 = false;
    if (waypointsBackup.Count <= 1) useOldPath = false;

    IMyShipConnector conn = FindConnector(MyShipConnectorStatus.Connectable);
    if (conn == null) conn = FindConnector(MyShipConnectorStatus.Connected);

    if (conn != null)
    {
        if (Math.Round(shipSpeed, 2) <= 0.20) dockDetectCounter++;
        else dockDetectCounter = 0;

        if (dockDetectCounter >= 5)
        {
            if (shipMode == ShipMode.Shuttle && (homeDock.isSet || useOldHome) && Vector3.Distance(homeDock.position, conn.GetPosition()) > 5)
            {
                jobPosition.forwardDir = remoteControl.WorldMatrix.Forward;
                jobPosition.leftDir = remoteControl.WorldMatrix.Left;
                jobPosition.downDir = remoteControl.WorldMatrix.Down;
                jobPosition.gravity = remoteControl.GetNaturalGravity();
                jobPosition.position = conn.GetPosition();
                jobPosition.isSet = true;
                jobPosition.connectorGridPos = conn.Position;
            }
            else
            {
                homeDock.forwardDir = remoteControl.WorldMatrix.Forward;
                homeDock.leftDir = remoteControl.WorldMatrix.Left;
                homeDock.downDir = remoteControl.WorldMatrix.Down;
                homeDock.gravity = remoteControl.GetNaturalGravity();
                homeDock.position = conn.GetPosition();
                homeDock.isSet = true;
                homeDock.connectorGridPos = conn.Position;
            }
        }
    }

    double lastDist = -1;
    if (waypoints.Count > 0) lastDist = Vector3.Distance(shipPosition, waypoints.Last().position);

    double speedFactor = Math.Max(1.5, Math.Pow(shipSpeed / 100.0, 2));
    double minSpacing = Math.Max(shipSpeed * speedFactor, 2);
    waypointSpacing = minSpacing;

    if ((lastDist == -1) || lastDist >= minSpacing)
    {
        WaypointInfo wp = new WaypointInfo(shipPosition, forwardDirection, downDirection, leftDirection, remoteControl.GetNaturalGravity());
        wp.CalcThrusterEfficiency(thrusters, thrusterTypeNames);
        waypoints.Add(wp);
    }
}


// ========================== DOCK HELPERS ==========================

WaypointInfo GetCurrentDock()
{
    if (shipMode != ShipMode.Shuttle) return poolDock != null ? poolDock : homeDock; // dock pool
    return currentDockTarget;
}

void SetDockTarget(WaypointInfo dock, JobState state)
{
    currentDockTarget = dock;
    if (jobState == JobState.Active) lastJobState = state;
}

// docking lane: is the ship off the straight line from approach point p out to laneEntry?
bool OffLane(Vector3 p)
{
    Vector3 d = Vector3.Normalize(laneEntry - p), v = shipPosition - p;
    return (v - d * Vector3.Dot(v, d)).Length() > wpReachedDist;
}

bool GetDockApproachPosition(WaypointInfo dock, float dist, bool useConnector, out Vector3 pos)
{
    if (useConnector)
    {
        Vector3I gridPos = new Vector3I((int)dock.connectorGridPos.X, (int)dock.connectorGridPos.Y, (int)dock.connectorGridPos.Z);
        IMySlimBlock block = Me.CubeGrid.GetCubeBlock(gridPos);
        if (block == null || !(block.FatBlock is IMyShipConnector))
        {
            pos = new Vector3();
            return false;
        }
        Vector3 connOffset = LocalTransformDirection(remoteControl, block.FatBlock.GetPosition() - shipPosition);
        Vector3 connForward = LocalTransformDirection(remoteControl, block.FatBlock.WorldMatrix.Forward);
        pos = dock.position - TransformLocalToWorld(dock.forwardDir, dock.downDir * -1, connOffset) - TransformLocalToWorld(dock.forwardDir, dock.downDir * -1, connForward) * dist;
        return true;
    }
    else
    {
        pos = dock.position;
        return true;
    }
}


// ========================== JOB POSITION CALCULATION ==========================

Vector3 GetJobGridPosition(int col, int row, bool recalc)
{
    if (!recalc && gridPosCached) return cachedGridPos;
    float colOffset = ((jobWidth - 1f) / 2f) - col;
    float rowOffset = ((jobHeight - 1f) / 2f) - row;
    cachedGridPos = jobPosition.position + jobPosition.leftDir * colOffset * JTW() + miningDown * -1 * rowOffset * JTH();
    gridPosCached = true;
    return cachedGridPos;
}

Vector3 GetMiningDepthPosition(Vector3 surfacePos, float depth)
{
    return surfacePos + (miningForward * depth);
}

// ========================== 14_JobControl.cs ==========================
// ========================== LOCATION DETERMINATION ==========================

LocationType DetermineLocation()
{
    float closest = -1;
    LocationType result = LocationType.AtJob;

    if (shipMode != ShipMode.Shuttle)
    {
        LearnDocks(); // docked at a pool connector? then that's the current dock
        if (jobState != JobState.NoJob)
        {
            Vector3 localPos = TransformToLocal(miningForward, miningDown * -1, shipPosition - jobPosition.position);
            if (Math.Abs(localPos.X) <= (float)(jobWidth * toolWidth) / 2f && Math.Abs(localPos.Y) <= (float)(jobHeight * toolHeight) / 2f)
            {
                if (localPos.Z <= -1 && localPos.Z >= -runtimeMaxDepth * 2) return LocationType.AtMine;
                if (localPos.Z > -1 && localPos.Z < shipDiameter * 2) return LocationType.AtJob;
            }
            if (CheckCloser(jobPosition.position, ref closest)) result = LocationType.AtJob;
        }
        if (homeDock.isSet)
        {
            if (CheckCloser(homeDock.position, ref closest)) result = LocationType.AtHomeDock;
            for (int i = 0; i < waypoints.Count; i++)
                if (CheckCloser(waypoints[i].position, ref closest)) result = LocationType.OnPath;
            if (Vector3.Distance(shipPosition, homeDock.position) < dockDist * shipDiameter) result = LocationType.AtHomeDock;
            if (FindConnector(MyShipConnectorStatus.Connectable) != null || FindConnector(MyShipConnectorStatus.Connected) != null)
                result = LocationType.AtHomeDock;
        }
    }
    else
    {
        Vector3 pos = new Vector3();
        IMyShipConnector connected = FindConnector(MyShipConnectorStatus.Connected);
        if (homeDock.isSet)
        {
            if (CheckCloser(homeDock.position, ref closest)) result = LocationType.AtHomeDock;
            if (GetDockApproachPosition(homeDock, dockDist, true, out pos))
                if (CheckCloser(pos, ref closest)) result = LocationType.AtHomeDock;
            if (connected != null && Vector3.Distance(connected.GetPosition(), homeDock.position) < 5)
                return LocationType.AtDock1;
        }
        for (int i = 0; i < waypoints.Count; i++)
            if (Vector3.Distance(waypoints[i].position, homeDock.position) > dockDist * shipDiameter && Vector3.Distance(waypoints[i].position, jobPosition.position) > dockDist * shipDiameter)
                if (CheckCloser(waypoints[i].position, ref closest)) result = LocationType.OnPath;
        if (jobPosition.isSet)
        {
            if (CheckCloser(jobPosition.position, ref closest)) result = LocationType.AtJob;
            if (GetDockApproachPosition(jobPosition, dockDist, true, out pos))
                if (CheckCloser(pos, ref closest)) result = LocationType.AtJob;
            if (connected != null && Vector3.Distance(connected.GetPosition(), jobPosition.position) < 5)
                return LocationType.AtDock2;
        }
    }
    return result;
}

bool CheckCloser(Vector3 pos, ref float closest)
{
    float dist = Vector3.Distance(pos, shipPosition);
    if (dist < closest || closest == -1) { closest = dist; return true; }
    return false;
}


// ========================== JOB MANAGEMENT ==========================

void StartNewJob()
{
    if (setupErrorLevel > 0) { statusMessage = "Setup error! Can't start"; return; }
    if (shipMode == ShipMode.Shuttle) { ContinueJob(); return; }

    jobPosition.position = shipPosition;
    jobPosition.gravity = remoteControl.GetNaturalGravity();
    jobPosition.forwardDir = forwardDirection;
    jobPosition.downDir = downDirection;
    jobPosition.leftDir = leftDirection;
    miningForward = referenceWorkTool.WorldMatrix.Forward;
    miningDown = jobPosition.downDir;
    if (miningForward == remoteControl.WorldMatrix.Down) miningDown = remoteControl.WorldMatrix.Backward;
    jobToolW = toolWidth; jobToolH = toolHeight;
    UpdateJobConfig(true, true);
    SetNavState(NavState.FlyToXY);
    StartJobCommon();
}

void UpdateJobConfig(bool force, bool resetProgress)
{
    if (jobState == JobState.NoJob && !force) return;
    bool needsReset = force || jobState == JobState.Done || jobWidth != configWidth || jobHeight != configHeight || jobStartPos != startPosition;
    if (needsReset)
    {
        if (jobState != JobState.NoJob)
        {
            jobState = JobState.Changed;
            GetJobGridPosition(holeCol, holeRow, resetProgress);
            statusMessage = "Job changed, lost progress";
        }
        jobStartPos = startPosition;
        jobWidth = configWidth;
        jobHeight = configHeight;
        holeRow = 0; holeCol = 0;
        doneHoles = ""; myHole = -1;
        jobSerial = (int)(DateTime.Now.Ticks / 10000000 % 1000000);
        maxMineDepth = 0; lastHoleIndex = 0;
        currentMineDepth = 0; currentHoleIndex = 0;
        UpdateProgress(true);
    }
}

void PrepareForFlight()
{
    SetFlightTarget(shipPosition, 0);
    SetBlocksEnabled(thrusters, true);
}

void StartJobCommon()
{
    ShowStatusMessage(StatusMsg.Running);
    PrepareForFlight();
    SetLandingGears(landingGears, false);
    SetSortersEnabled(sorterState);
    jobState = JobState.Active;
    CalculateMaxWeight(true);
    lastJobState = jobState;
    NavigateToMenu(MenuPage.JobRunning);
    ScanAllBlocks();
    damageCheckActive = true;
    initialDamageCount = 0;
    for (int i = allGridBlocks.Count - 1; i >= 0; i--)
        if (IsBlockDamaged(allGridBlocks[i], false)) initialDamageCount++;
    if (initialDamageCount > 0) statusMessage = "Started with damage";
}

void StopAll()
{
    if (jobState == JobState.Active) { jobState = JobState.Paused; statusMessage = "Job paused"; }
    SetNavState(NavState.Idle);
    lastJobState = jobState;
    SetGyroOverride(false, 0, 0, 0, 0);
    StopFlight();
    ApplyThrustOverride(new Vector3(), false);
    StopGyroOverride();
    SetBatteryChargeMode(ChargeMode.Auto);
    SetHydrogenStockpile(false);
    SetDampeners(true);
    SetStopAtState(NavState.Idle);
    UpdateSensor(false, false, 0, 0);
    SetBlocksEnabled(workTools, false);
    SetBlocksEnabled(connectors, true);
    SetSortersEnabled(true);
    returnToJobAfter = false;
    ClearBlocked();
    dockWaiting = false;
    ReleaseDock();
    damageCheckActive = false;
    simulateShipFull = false;
    undockRequested = false;
    if (currentMenu != MenuPage.Main && currentMenu != MenuPage.BehaviorSettings && currentMenu != MenuPage.AdvancedSettings && currentMenu != MenuPage.ShuttleBehavior)
        NavigateToMenu(MenuPage.Main);
}

void ContinueJob()
{
    LocationType loc = DetermineLocation();

    if (shipMode == ShipMode.Shuttle)
    {
        if (!jobPosition.isSet || !homeDock.isSet) return;
        StartJobCommon();
        bool goToHome = Vector3.Distance(shipPosition, homeDock.position) < Vector3.Distance(shipPosition, jobPosition.position);
        if (savedJobState == JobState.ActiveHome) goToHome = true;
        if (savedJobState == JobState.ActiveJob) goToHome = false;

        if (goToHome)
        {
            SetDockTarget(homeDock, JobState.ActiveHome);
            switch (loc)
            {
                case LocationType.AtDock1: SetNavState(NavState.WaitForCommand); break;
                case LocationType.OnPath: SetNavState(NavState.FlyToJobArea); break;
                case LocationType.AtHomeDock: SetNavState(NavState.ApproachDock); break;
                default: SetNavState(NavState.Undocking); break;
            }
        }
        else
        {
            SetDockTarget(jobPosition, JobState.ActiveJob);
            switch (loc)
            {
                case LocationType.AtDock2: SetNavState(NavState.WaitForCommand); break;
                case LocationType.AtJob: SetNavState(NavState.ApproachDock); break;
                case LocationType.OnPath: SetNavState(NavState.FlyToJobArea); break;
                default: SetNavState(NavState.Undocking); break;
            }
        }
    }
    else
    {
        if (jobState != JobState.Paused && jobState != JobState.Changed && jobState != JobState.Active) return; // active: re-plans from where it is (e.g. stuck)
        bool wasChanged = jobState == JobState.Changed;
        StartJobCommon();
        bool hasDock = IsShipFull(false) && homeDock.isSet;

        switch (loc)
        {
            case LocationType.AtJob: SetNavState(hasDock ? NavState.FlyToPath : NavState.FlyToXY); break;
            case LocationType.OnPath: SetNavState(hasDock ? NavState.FlyToDockArea : NavState.FlyToJobArea); break;
            // near home but not at the connector: to the docking area (asks the station, docks) instead of unloading
            case LocationType.AtHomeDock:
                SetNavState(FindConnector(MyShipConnectorStatus.Connected) == null && FindConnector(MyShipConnectorStatus.Connectable) == null ? NavState.FlyToDockArea :
                    hasDock ? NavState.ApproachDock : NavState.Unloading);
                break;
            case LocationType.AtMine:
            {
                if (lastHoleIndex != currentHoleIndex || wasChanged) SetNavState(NavState.Returning);
                else SetNavState(NavState.Mining);
            }
            break;
            default: break;
        }
    }
}

void FlyToJobPosition()
{
    if (jobState == JobState.NoJob && !homeDock.isSet) return;
    if (shipMode == ShipMode.Shuttle && (!jobPosition.isSet || !homeDock.isSet)) return;
    statusMessage = "Move to job";

    LocationType loc = DetermineLocation();
    if (shipMode == ShipMode.Shuttle)
    {
        SetDockTarget(jobPosition, JobState.ActiveJob);
        switch (loc)
        {
            case LocationType.AtJob: SetNavState(NavState.ApproachDock); break;
            case LocationType.OnPath: SetNavState(NavState.FlyToJobArea); break;
            case LocationType.AtDock2: return;
            default: SetNavState(NavState.Undocking); break;
        }
        SetStopAtState(NavState.WaitForCommand);
    }
    else
    {
        switch (loc)
        {
            case LocationType.AtJob: SetNavState(NavState.FlyToJobArea); break;
            case LocationType.OnPath: SetNavState(NavState.FlyToJobArea); break;
            case LocationType.AtHomeDock: SetNavState(NavState.Unloading); break;
            case LocationType.AtMine: SetNavState(NavState.Returning); break;
            default: break;
        }
        if (jobState == JobState.NoJob) SetStopAtState(NavState.FlyToJobArea);
        else SetStopAtState(NavState.AlignJob);
        returnToJobAfter = true;
    }
    PrepareForFlight();
    NavigateToMenu(MenuPage.StatusOverview);
    SetLandingGears(landingGears, false);
    lastJobState = JobState.MoveToJob;
}

void FlyToHomePosition()
{
    if (!homeDock.isSet) return;
    statusMessage = "Move home";

    LocationType loc = DetermineLocation();
    if (shipMode == ShipMode.Shuttle)
    {
        SetDockTarget(homeDock, JobState.ActiveHome);
        switch (loc)
        {
            case LocationType.OnPath: SetNavState(NavState.FlyToDockArea); break;
            case LocationType.AtHomeDock: SetNavState(NavState.ApproachDock); break;
            case LocationType.AtDock1: return;
            default: SetNavState(NavState.Undocking); break;
        }
        SetStopAtState(NavState.WaitForCommand);
    }
    else
    {
        if (FindConnector(MyShipConnectorStatus.Connected) != null) return;
        if (FindConnector(MyShipConnectorStatus.Connectable) != null)
        {
            SetNavState(NavState.Docking);
            SetStopAtState(NavState.Unloading);
            return;
        }
        switch (loc)
        {
            case LocationType.AtJob: SetNavState(NavState.FlyToPath); break;
            case LocationType.OnPath: SetNavState(NavState.FlyToDockArea); break;
            case LocationType.AtHomeDock: SetNavState(NavState.FlyToDockArea); break;
            case LocationType.AtMine: SetNavState(NavState.ReturnToDock); break;
            default: break;
        }
        SetStopAtState(NavState.Unloading);
    }
    PrepareForFlight();
    NavigateToMenu(MenuPage.StatusOverview);
    SetLandingGears(landingGears, false);
    lastJobState = JobState.MoveHome;
}

void SetNavState(NavState newState)
{
    if (newState == NavState.Idle) stopAtState = NavState.Idle;
    if (stopAtState != NavState.Idle && currentNavState == stopAtState && newState != stopAtState) { StopAll(); return; }
    stateFirstTick = true;
    currentNavState = newState;
}

void SetStopAtState(NavState state)
{
    this.stopAtState = state;
}

// ========================== 15_Navigation.cs ==========================
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


// Docking at a pool connector: the recorded path ends at the home connector, among the docked ships.
// Leave it before the waypoint that gets closer to home than the pool connector is (plus one ship
// size), and fly to that connector's lane from there.
bool PoolPathEnd(int idx)
{
    return poolDock != null && idx >= 0 && idx < waypoints.Count &&
        Vector3.Distance(waypoints[idx].position, homeDock.position) < Vector3.Distance(poolDock.position, homeDock.position) + shipDiameter;
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

    waitingHere = false; // set again below by every branch that holds the ship waiting

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
                // -2: every hole left is being worked by another ship (they keep their holes, none will
                // free up): nothing to do here -> home, dock, and stop there (job paused; CONT resumes)
                if (claimed == -2 && homeDock.isSet)
                {
                    coopWaiting = false;
                    SetNavState(NavState.FlyToPath); SetStopAtState(NavState.Unloading); lastJobState = JobState.MoveHome;
                    statusMessage = "No free hole left: going home";
                    return;
                }
                // -1: short wait for the controller's answer, where it is. -2 without a home: wait beside
                // the job area, backed off from the surface, out of the other ships' way
                if (!coopWaiting || (claimed == -2) != coopWaitAside)
                {
                    coopWaiting = true; coopWaitAside = claimed == -2;
                    // waiting spot: on a ring coopWaitHeight (2.5) ship sizes above the job centre, above everyone working
                    // or passing (always clear of terrain), each ship on its own place of the ring
                    int slot = 0; // rank among the ships on this job
                    for (int i = 0; i < traffic.Count; i++) if (traffic[i].job == JobKey() && traffic[i].id < myGridId) slot++;
                    double a = slot % 6 * Math.PI / 3;
                    coopWaitPos = !coopWaitAside ? shipPosition : jobPosition.position - miningForward * shipDiameter * coopWaitHeight +
                        (jobPosition.leftDir * (float)Math.Cos(a) + miningDown * (float)Math.Sin(a)) * shipDiameter * 1.2f;
                }
                SetFlightTarget(coopWaitPos, 10);
                waitingHere = true;
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
                SetBlocksEnabled(workTools, false); // off on the way to the hole: on when it starts the hole (Mining)
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
            // no progress for holeStuckTime (5 s, not held by traffic or the cameras): over the hole, the ground is in
            // the way down (terrain above the job surface) -> start the hole here (Mining turns the drills on);
            // elsewhere a hill is in the way -> go over it, one ship size higher, then down
            if (shipSpeed < 0.3f && trafficSpeedLimit < 0 && !pathBlocked)
            {
                if (++stuckCounter < holeStuckTime * 6) return;
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

        // emergency landing: no longer told to wait (cleared, or a dock is free) -> power up, climb back
        // to where it came down from, then on
        if (currentNavState == NavState.FlyToDockArea && emgStage > 0 && ++emgFree > 3)
        {
            if (emgStage < 3)
            {
                emgStage = 3; SetBlocksEnabled(thrusters, true); SetLandingGears(landingGears, false); AlignToDown(downDirection, forwardDirection, leftDirection, true);
                // came down where it was (e.g. after a restart): climb at least one ship size first
                Vector3 g = remoteControl.GetNaturalGravity();
                if (Vector3.Distance(shipPosition, emgStart) < shipDiameter && g.LengthSquared() > 0.01f) emgStart = shipPosition - Vector3.Normalize(g) * shipDiameter;
            }
            SetFlightTarget(emgStart, 5);
            if (distToTarget > wpReachedDist) { statusMessage = "Emergency: taking off"; return; }
            emgStage = 0;
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
                waitingHere = true;
                if (emergency && !outOfService) { EmergencyLand(); return; }
                if (!dockHoldSet) { dockHoldSet = true; dockHoldPos = InDockZone(cur) ? shipPosition : cur; }
                SetFlightTarget(dockHoldPos, 0); // keeps facing the way it flew in; turns to the dock once cleared
                if (!outOfService) statusMessage = WaitText();
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

        if (useDockDir && currentNavState == NavState.FlyToDockArea)
            AlignToGravity(pathFollowData.target.downDir, 90, true); // facing the way it flies: it turns round at the connector's lane
        else if (useDockDir)
            AlignToDown(pathFollowData.target.downDir, pathFollowData.target.forwardDir, pathFollowData.target.leftDir, false);
        else if (offPath)
            AlignToGravity(pathFollowData.target.downDir, 10, true);
        else if (atWaypoint && wp != null)
        {
            if (pathDirection > 0) AlignToDown(wp.downDir, wp.forwardDir, wp.leftDir, 90, false);
            else AlignToDown(wp.downDir, -wp.forwardDir, -wp.leftDir, 90, false);
        }

        SetFlightTargetFull(true, false, true, navTargetPos, pathSegmentDir, currentPathWaypoint == null ? 0 : currentPathWaypoint.maxSpeed, approachSpeed);

        if (currentNavState == NavState.FlyToDockArea && PoolPathEnd(pathWaypointIndex)) reachedEnd = true;
        if (reachedEnd)
        {
            waypointsRemaining = 0;
            // station dock control: wait at the path end until the station clears this ship
            bool noClear = currentNavState == NavState.FlyToDockArea && !CheckDockClearance();
            if (noClear || currentNavState == NavState.FlyToDockArea && dockWaiting)
            {
                waitingHere = true;
                if (emergency) EmergencyLand();
                else { SetFlightTarget(navTargetPos, 0); if (!noClear) statusMessage = "Waiting for a free dock"; }
                return;
            }
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
            if (!ChooseDock(Math.Max(1, DockPhase())) || GetCurrentDock() != before) { SetNavState(NavState.FlyToDockArea); return; }
        }
        WaypointInfo dock = GetCurrentDock();
        if (stateFirstTick)
        {
            if (!GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos) || !GetDockApproachPosition(dock, (dockDist + 1) * shipDiameter, true, out laneEntry))
            { ShowStatusMessage(StatusMsg.ConnectorNotReady); StopAll(); return; }
            // straight lane in front of the connector: off its line -> first to the lane entry, then straight in (no diagonal past docked ships)
            // facing the way it flies to the lane entry; turns round there (already on the lane: where it
            // is), then backs straight in
            toLane = OffLane(navTargetPos);
            laneTurn = !toLane;
            if (toLane) { SetFlightTarget(laneEntry, 0); AlignToGravity(dock.downDir, 90, true); }
            else { SetFlightTarget(shipPosition, 0); AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false); }
            stuckCounter = 0;
        }
        // already at (or docked to) the connector: dock, whatever else is going on (e.g. job started while docked)
        if (FindConnector(MyShipConnectorStatus.Connectable) != null || FindConnector(MyShipConnectorStatus.Connected) != null)
        { SetNavState(NavState.Docking); return; }
        // close to the lane entry: hold the heading (the direction to a point that close swings about)
        if (toLane && lookAtTarget && distToTarget < 5 && distToTarget != -1) AlignToDown(downDirection, forwardDirection, leftDirection, true);
        if (toLane && distToTarget < wpReachedDist && distToTarget != -1)
        {
            toLane = false; laneTurn = true; stuckCounter = 0;
            SetFlightTarget(laneEntry, 0);
            AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false);
            return;
        }
        if (laneTurn)
        {
            statusMessage = "Docking: turning at the lane";
            if (!isAligned && ++stuckCounter < laneTurnTime * 6) return; // at most laneTurnTime (8 s) (weak gyros, heavy cargo): then on anyway
            laneTurn = false; statusMessage = "";
            GetDockApproachPosition(dock, dockDist * shipDiameter, true, out navTargetPos);
            SetFlightTarget(navTargetPos, 0);
            return;
        }
        if (!toLane && distToTarget < followPathDock * shipDiameter && distToTarget != -1)
        {
            SetFlightTarget(navTargetPos, 10);
            AlignToDown(dock.downDir, dock.forwardDir, dock.leftDir, false);
        }
        // reached the point beside the obstacle: from here straight to the connector again
        if (detourShift.LengthSquared() > 0 && distToTarget < wpReachedDist && distToTarget != -1) { detourShift = new Vector3(); return; }
        if (distToTarget < wpReachedDist / 2 && distToTarget != -1)
        {
            // the connector didn't lock dockRetries (3) times: leave it, ask for another one
            if (dockTries >= dockRetries && shipMode != ShipMode.Shuttle)
            {
                badDocks.Add(poolDock != null ? poolDockId : homeConnId); // keeps asking: the station drops that clearance, the ship keeps its place
                dockTries = 0;
                SetNavState(NavState.FlyToDockArea);
                statusMessage = "Docking failed: trying another connector";
                return;
            }
            SetNavState(NavState.AlignDock);
            return;
        }
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
            dockStartTime = TimeNow();
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
            if (stuckCounter > 20) { dockTries++; SetNavState(NavState.RetryDocking); return; }
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
            dockTries = 0; badDocks.Clear();
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

// ========================== 16_MiningProgress.cs ==========================
// ========================== PROGRESS / DETECTION / SPEED ==========================

void UpdateProgress(bool reset)
{
    if (reset) { jobProgress = 0; return; }
    if (shipMode == ShipMode.Shuttle) return;
    float done = currentHoleIndex * Math.Max(1, configDepth);
    if (lastHoleIndex == currentHoleIndex) done += Math.Min(configDepth, currentMineDepth);
    float total = jobWidth * jobHeight * Math.Max(1, configDepth);
    jobProgress = Math.Max(jobProgress, (float)Math.Min(done / total * 100.0, 100));
}

MyDetectedEntityType GetDetectedEntityType()
{
    try { if (IsBlockValid(sensor, true) && !sensor.LastDetectedEntity.IsEmpty()) return sensor.LastDetectedEntity.Type; }
    catch { }
    return MyDetectedEntityType.None;
}

float GetWorkSpeed(bool forward)
{
    if (shipMode == ShipMode.Grinder && GetDetectedEntityType() == MyDetectedEntityType.None && !IsBlockDamaged(sensor, true))
        return fastSpeed;
    else return forward ? workSpeedForward : workSpeedBackward;
}

// ========================== 17_Resources.cs ==========================
// ========================== ENERGY / CARGO / DAMAGE ==========================

void UpdateBatteryState()
{
    float maxPower = 0, currentPower = 0, input = 0, output = 0;
    for (int i = 0; i < batteries.Count; i++)
    {
        IMyBatteryBlock bat = batteries[i];
        if (!IsBlockValid(bat, true)) continue;
        maxPower += bat.MaxStoredPower;
        currentPower += bat.CurrentStoredPower;
        input += bat.CurrentInput;
        output += bat.CurrentOutput;
    }
    batteryPercent = (float)Math.Round(SafeDiv(currentPower, maxPower) * 100, 1);
    if (input >= output) batteryState = BatteryState.Charging;
    else batteryState = BatteryState.Discharging;
    if (input == 0 && output == 0 || batteryPercent == 100.0) batteryState = BatteryState.Idle;
    if (batteries.Count == 0) batteryState = BatteryState.None;
}

void UpdateHydrogen()
{
    float filled = 0;
    for (int i = 0; i < hydrogenTanks.Count; i++)
    {
        IMyGasTank tank = hydrogenTanks[i];
        if (!IsBlockValid(tank, true)) continue;
        filled += (float)tank.FilledRatio;
    }
    hydrogenPercent = SafeDiv(filled, hydrogenTanks.Count) * 100f;
}

void UpdateUranium()
{
    uraniumAmount = 0;
    try
    {
        for (int i = 0; i < reactors.Count; i++)
        {
            IMyReactor reactor = reactors[i];
            if (!IsBlockValid(reactor, true)) continue;
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            reactor.GetInventory(0).GetItems(items);
            float amount = 0;
            for (int j = 0; j < items.Count; j++)
            {
                MyInventoryItem item = items[j];
                if (item.Type.SubtypeId.ToUpper() == "URANIUM" && item.Type.TypeId.ToUpper().Contains("_INGOT"))
                    amount += (float)item.Amount;
            }
            if (amount < uraniumAmount || i == 0) { uraniumAmount = amount; lowestUraniumReactor = reactor.CustomName; }
        }
    }
    catch (Exception e) { lastError = e; }
}

void UpdateCargoLoad()
{
    maxVolume = 0;
    currentVolume = 0;
    try
    {
        for (int i = 0; i < cargoBlocks.Count; i++)
        {
            IMyTerminalBlock block = cargoBlocks[i];
            if (!IsBlockValid(block, true)) continue;
            currentVolume += (float)block.GetInventory(0).CurrentVolume;
            maxVolume += (float)block.GetInventory(0).MaxVolume;
        }
        loadPercent = (float)Math.Min(Math.Round(SafeDiv(currentVolume, maxVolume) * 100, 1), 100.0);
    }
    catch (Exception e) { lastError = e; }
}

void UpdateInventory()
{
    try
    {
        itemList.Clear();
        for (int i = 0; i < cargoBlocks.Count; i++)
        {
            IMyTerminalBlock block = cargoBlocks[i];
            if (!IsBlockValid(block, true)) continue;
            AddBlockInventory(block, ItemLocation.InCargo);
        }
        if (ejectionMode != EjectionMode.Off)
        {
            for (int i = 0; i < allInventoryBlocks.Count; i++)
            {
                IMyTerminalBlock block = allInventoryBlocks[i];
                if (!IsBlockValid(block, true)) continue;
                AddBlockInventory(block, ItemLocation.InOther);
            }
        }
        SortItemList(itemList);
    }
    catch (Exception e) { lastError = e; }
}

void CheckDamage()
{
    if (damageCheckActive)
    {
        if (GetDamagedBlocks().Count > initialDamageCount)
        {
            damageCheckActive = false;
            if (onDamageBehavior != DamageBehavior.Ignore)
            {
                StopAll();
                if (onDamageBehavior == DamageBehavior.FlyToJob) FlyToJobPosition();
                if (onDamageBehavior == DamageBehavior.ReturnHome)
                    if (homeDock.isSet) FlyToHomePosition(); else FlyToJobPosition();
            }
            statusMessage = "Damage detected";
        }
    }
}

bool CheckEnergy()
{
    if (!initialized) return true;
    if (jobState == JobState.Active)
    {
        if (minBatteryPercent > 0 && batteryState != BatteryState.None)
            if (batteryPercent <= minBatteryPercent) { statusMessage = "Low energy! Move home"; return false; }
        if (minUraniumKg > 0 && reactors.Count > 0)
            if (uraniumAmount <= minUraniumKg) { statusMessage = "Low fuel: " + lowestUraniumReactor; return false; }
        if (minHydrogenPercent > 0 && hydrogenTanks.Count > 0)
            if (hydrogenPercent <= minHydrogenPercent) { statusMessage = "Low hydrogen"; return false; }
    }
    return true;
}

List<IMyTerminalBlock> GetDamagedBlocks()
{
    List<IMyTerminalBlock> damaged = new List<IMyTerminalBlock>();
    for (int i = 0; i < allGridBlocks.Count; i++)
    {
        IMyTerminalBlock block = allGridBlocks[i];
        if (IsBlockDamaged(block, false)) damaged.Add(block);
    }
    return damaged;
}

bool IsBlockDamaged(IMyTerminalBlock block, bool strict)
{
    return (!IsBlockValid(block, strict) || !block.IsFunctional);
}

bool IsBlockValid(IMyTerminalBlock block, bool strict)
{
    if (block == null) return false;
    try
    {
        IMyCubeBlock gridBlock = Me.CubeGrid.GetCubeBlock(block.Position).FatBlock;
        if (strict) return gridBlock == block;
        else return gridBlock.GetType() == block.GetType();
    }
    catch { return false; }
}


// ========================== INVENTORY HELPERS ==========================

float CountBlockItems(IMyTerminalBlock block, String typeName, String subtypeName, String[] exclude)
{
    float count = 0;
    for (int i = 0; i < block.InventoryCount; i++)
    {
        IMyInventory inv = block.GetInventory(i);
        List<MyInventoryItem> items = new List<MyInventoryItem>();
        inv.GetItems(items);
        foreach (MyInventoryItem item in items)
        {
            if (exclude != null && (exclude.Contains(item.Type.TypeId.ToUpper()) || exclude.Contains(item.Type.SubtypeId.ToUpper())))
                continue;
            if ((typeName == "" || item.Type.TypeId.ToUpper() == typeName) && (subtypeName == "" || item.Type.SubtypeId.ToUpper() == subtypeName))
                count += (float)item.Amount;
        }
    }
    return count;
}

ItemInfo FindItem(String name, String type, ItemLocation loc, bool create)
{
    name = name.ToUpper();
    type = type.ToUpper();
    for (int i = 0; i < itemList.Count; i++)
    {
        ItemInfo item = itemList[i];
        if (item.name.ToUpper() == name && item.type.ToUpper() == type && (item.location == loc || loc == ItemLocation.All))
            return item;
    }
    ItemInfo result = null;
    if (create) { result = new ItemInfo(name, type, 0, loc); itemList.Add(result); }
    return result;
}

int CountItems(String name, String type, ItemLocation loc)
{
    return CountItems(name, type, loc, null);
}

int CountItems(String name, String type, ItemLocation loc, String[] exclude)
{
    int count = 0;
    name = name.ToUpper();
    type = type.ToUpper();
    for (int i = 0; i < itemList.Count; i++)
    {
        ItemInfo item = itemList[i];
        if (exclude != null && exclude.Contains(item.name.ToUpper())) continue;
        if ((name == "" || item.name.ToUpper() == name) && (type == "" || item.type.ToUpper() == type) && (item.location == loc || loc == ItemLocation.All))
            count += item.amount;
    }
    return count;
}

void AddBlockInventory(IMyTerminalBlock block, ItemLocation loc)
{
    for (int i = 0; i < block.InventoryCount; i++)
    {
        List<MyInventoryItem> items = new List<MyInventoryItem>();
        block.GetInventory(i).GetItems(items);
        for (int j = 0; j < items.Count; j++)
            FindItem(items[j].Type.SubtypeId, items[j].Type.TypeId.Replace("MyObjectBuilder_", ""), loc, true).amount += (int)items[j].Amount;
    }
}

void SortItemList(List<ItemInfo> list)
{
    for (int i = list.Count - 1; i > 0; i--)
        for (int j = 0; j < i; j++)
        {
            ItemInfo a = list[j];
            ItemInfo b = list[j + 1];
            if (a.amount < b.amount) list.Move(j, j + 1);
        }
}

bool IsShipFull(bool showMsg)
{
    if (weightLimitEnabled && shipMode != ShipMode.Shuttle)
        if (maxWeight != -1 && shipMass >= maxWeight) { statusMessage = "Ship too heavy"; return true; }
    if (loadPercent >= maxLoadPercent || simulateShipFull)
    {
        simulateShipFull = false;
        statusMessage = "Ship is full";
        return true;
    }
    return false;
}

bool IsCargoEmpty()
{
    String[] exclude = null;
    if (!unloadIce) exclude = new string[] { "ICE" };
    if (shipMode == ShipMode.Miner) return CountItems("", "ORE", ItemLocation.InCargo, exclude) == 0;
    if (shipMode == ShipMode.Grinder) return CountItems("", "COMPONENT", ItemLocation.InCargo, exclude) == 0;
    else return CountItems("", "", ItemLocation.InCargo, exclude) == 0;
}

bool IsPlayerInCockpit()
{
    List<IMyCockpit> cockpits = new List<IMyCockpit>();
    gridTerminalSystem.GetBlocksOfType(cockpits, q => q.CubeGrid == Me.CubeGrid);
    for (int i = 0; i < cockpits.Count; i++)
        if (cockpits[i].IsUnderControl) return true;
    return false;
}

bool CheckShuttleUndock(bool reset, bool showStatus)
{
    IMyShipConnector conn = FindConnector(MyShipConnectorStatus.Connected);
    if (conn == null) return false;
    if (Vector3.Distance(homeDock.position, conn.GetPosition()) < 5)
        return CheckUndockTrigger(undockConfig1, reset, showStatus);
    if (Vector3.Distance(jobPosition.position, conn.GetPosition()) < 5)
        return CheckUndockTrigger(undockConfig2, reset, showStatus);
    return false;
}

bool CheckUndockTrigger(UndockConfig cfg, bool reset, bool showStatus)
{
    if (reset) cfg.Reset();
    cfg.UpdateTimer();
    bool triggered = false;
    String status = "";
    switch (cfg.trigger)
    {
        case UndockTrigger.OnCommand: { status = "Waiting for command"; triggered = false; break; }
        case UndockTrigger.OnShipFull: { status = "Waiting for cargo"; triggered = IsShipFull(true); break; }
        case UndockTrigger.OnShipEmpty: { status = "Unloading"; triggered = IsCargoEmpty(); break; }
        case UndockTrigger.OnTimerDelay: { triggered = true; break; }
        case UndockTrigger.OnBatteriesFull: { status = "Charging batteries"; triggered = batteryPercent >= 100f; break; }
        case UndockTrigger.OnBatteriesLow25: { status = "Discharging batteries"; triggered = batteryPercent <= 25f; break; }
        case UndockTrigger.OnBatteriesEmpty: { status = "Discharging batteries"; triggered = batteryPercent <= 0f; break; }
        case UndockTrigger.OnHydrogenFull: { status = "Filling up hydrogen"; triggered = hydrogenPercent >= 100f; break; }
        case UndockTrigger.OnHydrogenLow25: { status = "Unloading hydrogen"; triggered = hydrogenPercent <= 25f; break; }
        case UndockTrigger.OnHydrogenEmpty: { status = "Unloading hydrogen"; triggered = hydrogenPercent <= 0f; break; }
        case UndockTrigger.OnPlayerEntered:
        {
            bool inCockpit = IsPlayerInCockpit();
            if (!inCockpit) cfg.playerWasInCockpit = true;
            triggered = cfg.playerWasInCockpit && inCockpit;
            status = "Waiting for passengers";
            break;
        }
    }
    if (!triggered) cfg.CheckDelay(true);
    if (triggered && cfg.HasDelay())
    {
        triggered = cfg.CheckDelay(false);
        status = "Undocking in: " + FormatTime((int)Math.Max(0, cfg.delay - cfg.elapsedDelay));
    }
    if (showStatus) waitingStatusText = status;
    return triggered;
}

// ========================== 18_InventoryBalance.cs ==========================
// ========================== INVENTORY BALANCING ==========================

void BalanceInventory()
{
    if (!invBalancingEnabled) return;
    if (workTools.Count <= 1) return;

    float totalMax = 0, totalCurrent = 0;
    for (int i = 0; i < workTools.Count; i++)
    {
        IMyTerminalBlock tool = workTools[i];
        if (IsBlockDamaged(tool, true)) continue;
        totalMax += (float)tool.GetInventory(0).MaxVolume;
        totalCurrent += (float)tool.GetInventory(0).CurrentVolume;
    }
    float avgFill = (float)Math.Round(SafeDiv(totalCurrent, totalMax), 5);

    for (int pass = 0; pass < Math.Max(1, Math.Floor(workTools.Count / 10f)); pass++)
    {
        float highestFill = 0, lowestFill = 0;
        float highestMax = 0, lowestMax = 0;
        IMyTerminalBlock fullest = null, emptiest = null;

        for (int i = 0; i < workTools.Count; i++)
        {
            IMyTerminalBlock tool = workTools[i];
            if (IsBlockDamaged(tool, true)) continue;
            float maxVol = (float)tool.GetInventory(0).MaxVolume;
            float fillRatio = SafeDiv((float)tool.GetInventory(0).CurrentVolume, maxVol);

            if (fullest == null || fillRatio > highestFill) { fullest = tool; highestFill = fillRatio; highestMax = maxVol; }
            if (emptiest == null || fillRatio < lowestFill) { emptiest = tool; lowestFill = fillRatio; lowestMax = maxVol; }
        }

        if (fullest == null || emptiest == null || fullest == emptiest) return;

        if (checkConveyorSystem && !fullest.GetInventory(0).IsConnectedTo(emptiest.GetInventory(0)))
        {
            if (balanceFailCount > 20) statusMessage = "Inventory balancing failed";
            else balanceFailCount++;
            return;
        }
        balanceFailCount = 0;

        List<MyInventoryItem> items = new List<MyInventoryItem>();
        fullest.GetInventory(0).GetItems(items);
        float volPerUnit = 0;
        if (items.Count == 0) continue;
        MyInventoryItem firstItem = items[0];
        String itemKey = firstItem.Type.TypeId + firstItem.Type.SubtypeId;

        if (!volumePerItem.TryGetValue(itemKey, out volPerUnit))
        {
            if (MeasureItemVolume(fullest.GetInventory(0), 0, emptiest.GetInventory(0), out volPerUnit))
                volumePerItem.Add(itemKey, volPerUnit);
            else return;
        }

        float toTransferFromFull = ((highestFill - avgFill) * highestMax / volPerUnit);
        float toTransferToEmpty = ((avgFill - lowestFill) * lowestMax / volPerUnit);
        int transferAmount = (int)Math.Min(toTransferToEmpty, toTransferFromFull);
        if (transferAmount <= 0) return;

        if ((float)firstItem.Amount < transferAmount)
            fullest.GetInventory(0).TransferItemTo(emptiest.GetInventory(0), 0, null, null, firstItem.Amount);
        else
            fullest.GetInventory(0).TransferItemTo(emptiest.GetInventory(0), 0, null, null, transferAmount);
    }
}

bool MeasureItemVolume(IMyInventory source, int idx, IMyInventory dest, out float volume)
{
    volume = 0;
    float beforeVol = (float)source.CurrentVolume;
    List<MyInventoryItem> beforeItems = new List<MyInventoryItem>();
    source.GetItems(beforeItems);
    float beforeCount = 0;
    for (int i = 0; i < beforeItems.Count; i++) beforeCount += (float)beforeItems[i].Amount;

    source.TransferItemTo(dest, idx, null, null, 1);
    float volDiff = beforeVol - (float)source.CurrentVolume;

    List<MyInventoryItem> afterItems = new List<MyInventoryItem>();
    source.GetItems(afterItems);
    float afterCount = 0;
    for (int i = 0; i < afterItems.Count; i++) afterCount += (float)afterItems[i].Amount;

    if (volDiff == 0f || !InRange(0.9999, beforeCount - afterCount, 1.0001))
        return false;
    volume = volDiff;
    return true;
}

// ========================== 19_Flight.cs ==========================
// ========================== FLIGHT CONTROL ==========================

void UpdateFlight()
{
    Vector3 flyTarget = navTargetPos + trafficShift + detourShift; // traffic: keep right of an oncoming PAM ship; docking: way around an obstacle
    Vector3 toTarget = flyTarget - shipPosition;
    if (toTarget.Length() == 0) toTarget = new Vector3(0, 0, -1);
    Vector3 localTarget = LocalTransformDirection(remoteControl, toTarget);
    Vector3 targetDir = Vector3.Normalize(localTarget);
    Vector3 localGravity = LocalTransformDirection(remoteControl, remoteControl.GetNaturalGravity());

    float speedFactorPath = pathSpeed > 0 ? Math.Max(0, 1 - AngleBetween(toTarget, flightPathDir) / 5) : 0;
    float maxSpeed = (float)Math.Min((maxApproachSpeed > 0 ? maxApproachSpeed : 1000f), Math.Max(CalculateMaxSpeed(-localTarget, localGravity, null), pathSpeed * speedFactorPath));

    if (!thrustEnabled) maxSpeed = 0;
    if (slowOnMisalign) maxSpeed = Math.Max(0, 1 - currentAngleError / alignThreshold) * maxSpeed;
    if (generalSpeedLimit > 0) maxSpeed = Math.Min(generalSpeedLimit, maxSpeed);
    if (pathBlocked) maxSpeed = 0; // obstacle brake: cancel velocity and hold position
    if (trafficSpeedLimit >= 0) maxSpeed = Math.Min(maxSpeed, trafficSpeedLimit); // keep distance to a PAM ship ahead
    if (NearJobArea()) maxSpeed = Math.Min(maxSpeed, jobAreaSpeed); // slow and steady next to the other ships
    NavState n = currentNavState; // docking area: from the queue to the connector and back out
    if (n == NavState.ApproachDock || n == NavState.AlignDock || n == NavState.RetryDocking || n == NavState.Undocking ||
        (n == NavState.FlyToDockArea && InDockZone(shipPosition))) maxSpeed = Math.Min(maxSpeed, dockAreaSpeed);
    if (softApproach) maxSpeed *= (float)Math.Min(1, SafeDiv(toTarget.Length(), wpReachedDist) / 2);

    Vector3 localVelocity = LocalTransformDirection(remoteControl, remoteControl.GetShipVelocities().LinearVelocity);
    float alignmentFactor = (float)(Math.Max(0, 15 - AngleBetween(-targetDir, -localVelocity)) / 15) * 0.85f + 0.15f;
    thrustEfficiency += Math.Sign(alignmentFactor - thrustEfficiency) / 10f;

    Vector3 desiredVelocity = targetDir * maxSpeed * thrustEfficiency - (localVelocity);
    Vector3 availableThrust = GetThrustMap(desiredVelocity, null);

    if (thrustActive && distToTarget > 0.1f)
    {
        desiredVelocity.X *= SmoothThrust(desiredVelocity.X, ref thrustMultiplier.X, 1f, availableThrust.X, 20);
        desiredVelocity.Y *= SmoothThrust(desiredVelocity.Y, ref thrustMultiplier.Y, 1f, availableThrust.Y, 20);
        desiredVelocity.Z *= SmoothThrust(desiredVelocity.Z, ref thrustMultiplier.Z, 1f, availableThrust.Z, 20);
    }
    else thrustMultiplier = new Vector3(1, 1, 1);

    thrustForce = shipMass * desiredVelocity - localGravity * shipMass;
    ApplyThrustOverride(thrustForce, thrustActive);
    distToTarget = Vector3.Distance(shipPosition, navTargetPos + detourShift); // reached the target, not the point traffic steers to
}

float SmoothThrust(float value, ref float state, float step, float maxThrust, float maxMultiplier)
{
    value = Math.Sign(Math.Round(value, 2));
    if (value == Math.Sign(state)) state += Math.Sign(state) * step;
    else state = value;
    if (value == 0) state = 1;
    float result = Math.Abs(state);
    if (result < maxMultiplier || maxThrust == 0) return result;
    state = Math.Min(maxMultiplier, Math.Max(-maxMultiplier, state));
    result = Math.Abs(maxThrust);
    return result;
}

void UpdateGyros()
{
    float pitch = 90, yaw = 90, roll = 90;
    float speed = (float)(Me.CubeGrid.GridSizeEnum == MyCubeSize.Small ? gyroSpeedSmall : gyroSpeedLarge) / 100f;

    Vector3 forward, localDown, localForward, localLeft;

    if (lookAtTarget)
    {
        forward = Vector3.Normalize(navTargetPos - shipPosition);
        localForward = LocalTransformDirection(remoteControl, forward);
        localDown = LocalTransformDirection(remoteControl, targetDown);
        pitch = AngleBetween(localForward, new Vector3(0, -1, 0)) - 90;
        yaw = AngleWithSign(localDown, new Vector3(-1, 0, 0), localDown.Y);
        roll = AngleWithSign(localForward, new Vector3(-1, 0, 0), localForward.Z);
    }
    else
    {
        forward = targetForward;
        localDown = LocalTransformDirection(remoteControl, targetDown);
        localForward = LocalTransformDirection(remoteControl, targetForward);
        localLeft = LocalTransformDirection(remoteControl, targetLeft);
        pitch = AngleWithSign(localDown, new Vector3(0, 0, 1), localDown.Y);
        yaw = AngleWithSign(localDown, new Vector3(-1, 0, 0), localDown.Y);
        roll = AngleWithSign(localLeft, new Vector3(0, 0, 1), localLeft.X);
    }

    if (alignToGravity && IsNearPlanet())
    {
        Vector3 gravDir = remoteControl.GetNaturalGravity();
        localDown = LocalTransformDirection(remoteControl, gravDir);
        pitch = AngleWithSign(localDown, new Vector3(0, 0, 1), localDown.Y);
        yaw = AngleWithSign(localDown, new Vector3(-1, 0, 0), localDown.Y);
    }

    if (!InRange(-45, yaw, 45)) { pitch = 0; roll = 0; }
    if (!InRange(-45, roll, 45)) pitch = 0;

    SetGyroOverride(gyroActive, 1, (-pitch) * speed, (-roll) * speed, (-yaw) * speed);
    currentAngleError = Math.Max(Math.Abs(pitch), Math.Max(Math.Abs(yaw), Math.Abs(roll)));
    isAligned = currentAngleError <= alignThreshold;
}

// ========================== 20_ThrustGyro.cs ==========================
// ========================== THRUST SYSTEM ==========================

void CalculateThrustVectors(IMyTerminalBlock reference)
{
    if (reference == null) return;
    totalThrustMap = new float[3, 2];
    thrustByType = new Dictionary<string, float[,]>();

    for (int i = 0; i < thrusters.Count; i++)
    {
        IMyThrust thruster = thrusters[i];
        if (!thruster.IsFunctional) continue;
        Vector3 dir = LocalTransformDirection(reference, thruster.WorldMatrix.Backward);
        float effective = thruster.MaxEffectiveThrust;

        if (Math.Round(dir.X, 2) != 0.0)
            if (dir.X >= 0) totalThrustMap[0, 0] += effective; else totalThrustMap[0, 1] -= effective;
        if (Math.Round(dir.Y, 2) != 0.0)
            if (dir.Y >= 0) totalThrustMap[1, 0] += effective; else totalThrustMap[1, 1] -= effective;
        if (Math.Round(dir.Z, 2) != 0.0)
            if (dir.Z >= 0) totalThrustMap[2, 0] += effective; else totalThrustMap[2, 1] -= effective;

        String typeName = GetThrusterType(thruster);
        float[,] typeMap = null;
        if (thrustByType.ContainsKey(typeName)) typeMap = thrustByType[typeName];
        else { typeMap = new float[3, 2]; thrustByType.Add(typeName, typeMap); }

        float maxThrust = thruster.MaxThrust;
        if (Math.Round(dir.X, 2) != 0.0)
            if (dir.X >= 0) typeMap[0, 0] += maxThrust; else typeMap[0, 1] -= maxThrust;
        if (Math.Round(dir.Y, 2) != 0.0)
            if (dir.Y >= 0) typeMap[1, 0] += maxThrust; else typeMap[1, 1] -= maxThrust;
        if (Math.Round(dir.Z, 2) != 0.0)
            if (dir.Z >= 0) typeMap[2, 0] += maxThrust; else typeMap[2, 1] -= maxThrust;
    }
}

static String GetThrusterType(IMyThrust thruster)
{
    return thruster.BlockDefinition.SubtypeId;
}

Vector3 LookupThrustMap(Vector3 direction, float[,] map)
{
    return new Vector3(
        direction.X >= 0 ? map[0, 0] : map[0, 1],
        direction.Y >= 0 ? map[1, 0] : map[1, 1],
        direction.Z >= 0 ? map[2, 0] : map[2, 1]);
}

bool GetThrusterEfficiency(WaypointInfo wp, String typeName, out float efficiency)
{
    efficiency = 0;
    int idx = thrusterTypeNames.IndexOf(typeName);
    if (idx == -1 || wp.thrusterEfficiency == null || idx >= wp.thrusterEfficiency.Length)
        return false;
    efficiency = wp.thrusterEfficiency[idx];
    if (efficiency == -1) return false;
    return true;
}

Vector3 GetThrustMap(Vector3 direction, WaypointInfo wp)
{
    if (wp != null)
    {
        Vector3 result = new Vector3();
        for (int i = 0; i < thrustByType.Keys.Count; i++)
        {
            String typeName = thrustByType.Keys.ElementAt(i);
            float eff = 0;
            if (!GetThrusterEfficiency(wp, typeName, out eff))
                return LookupThrustMap(direction, totalThrustMap);
            result += LookupThrustMap(direction, thrustByType.Values.ElementAt(i)) * eff;
        }
        return result;
    }
    return LookupThrustMap(direction, totalThrustMap);
}

float GetThrustForDirection(Vector3 direction, WaypointInfo wp)
{
    return GetThrustForDirection(direction, new Vector3(), wp);
}

float GetThrustForDirection(Vector3 direction, Vector3 gravity, WaypointInfo wp)
{
    Vector3 thrust = GetThrustMap(direction, wp);
    Vector3 effective = thrust + gravity * shipMass;
    float ratio = (effective / direction).AbsMin();
    return (float)(direction * ratio).Length();
}

float CalculateMaxSpeed(Vector3 localDir, Vector3 localGravity, WaypointInfo wp)
{
    if (localDir.Length() == 0) return 0;
    float alignFactor = 1;
    if (localGravity.Length() > 0) alignFactor = Math.Min(1, AngleBetween(-localGravity, localDir) / 90) * 0.4f + 0.6f;
    float thrustForce = GetThrustForDirection(localDir, localGravity, wp);
    if (thrustForce == 0) return 0.1f;
    float accel = SafeDiv(thrustForce, shipMass);
    float timeToStop = (float)Math.Sqrt(SafeDiv(localDir.Length(), accel * 0.5f));
    return accel * timeToStop * alignFactor * accelerationFactor;
}

float GravityThrustRatio(Vector3 forward, Vector3 up, Vector3 gravity, WaypointInfo wp)
{
    if (gravity.Length() == 0f) return 0;
    Vector3 localGrav = TransformToLocal(forward, up, Vector3.Normalize(gravity));
    float force = GetThrustForDirection(-localGrav, wp);
    return force / gravity.Length();
}

void CalculateMaxWeight(bool init)
{
    float weight = 0;
    float factor = 0.9f;
    if (init)
    {
        maxWeight = -1;
        waypointCalcIndex = 0;
        spaceWaypoint = null;

        if (jobState != JobState.NoJob && jobPosition.gravity.Length() != 0)
        {
            weight = factor * GravityThrustRatio(jobPosition.forwardDir, jobPosition.downDir * -1, jobPosition.gravity, null);
            if (weight < maxWeight || maxWeight == -1) maxWeight = weight;
        }
        if (homeDock.isSet && homeDock.gravity.Length() != 0)
        {
            weight = factor * GravityThrustRatio(homeDock.forwardDir, homeDock.downDir * -1, homeDock.gravity, null);
            if (weight < maxWeight || maxWeight == -1) maxWeight = weight;
        }
        return;
    }

    // Process waypoints incrementally
    if (waypointCalcIndex == -1) return;
    if (waypointCalcIndex >= 0)
    {
        int processed = 0;
        while (waypointCalcIndex < waypoints.Count)
        {
            if (processed > 100) return;
            processed++;
            WaypointInfo wp = waypoints[waypointCalcIndex];
            if (wp.gravity.Length() != 0f)
            {
                weight = factor * Math.Min(
                    GravityThrustRatio(wp.forwardDir, wp.downDir * -1, wp.gravity, wp),
                    GravityThrustRatio(wp.forwardDir * -1, wp.downDir * -1, wp.gravity, wp));
                if (weight < maxWeight || maxWeight == -1) maxWeight = weight;
            }
            else spaceWaypoint = wp;
            waypointCalcIndex++;
        }
        waypointCalcIndex = -1;
    }

    bool hasAtmoThrusters = true;
    float minThrust = 0;
    if (spaceWaypoint != null)
    {
        for (int i = 0; i < thrustByType.Count; i++)
        {
            String typeName = thrustByType.Keys.ElementAt(i);
            float[,] typeMap = thrustByType.Values.ElementAt(i);
            float eff = 0;
            if (!GetThrusterEfficiency(spaceWaypoint, typeName, out eff))
            {
                hasAtmoThrusters = false;
                break;
            }
            for (int a = 0; a < typeMap.GetLength(0); a++)
                for (int b = 0; b < typeMap.GetLength(1); b++)
                {
                    float absThrust = Math.Abs(typeMap[a, b] * eff);
                    if (absThrust == 0) continue;
                    hasAtmoThrusters = true;
                    if (minThrust == 0 || absThrust < minThrust) minThrust = absThrust;
                }
        }
    }

    if (!hasAtmoThrusters)
    {
        for (int a = 0; a < totalThrustMap.GetLength(0); a++)
            for (int b = 0; b < totalThrustMap.GetLength(1); b++)
            {
                float absThrust = Math.Abs(totalThrustMap[a, b]);
                if (absThrust == 0) continue;
                if (minThrust == 0 || absThrust < minThrust) minThrust = absThrust;
            }
    }

    if (minThrust > 0)
    {
        float minAccel = Me.CubeGrid.GridSizeEnum == MyCubeSize.Small ? minAccelerationSmall : minAccelerationLarge;
        weight = SafeDiv(minThrust, minAccel);
        if (weight > 0)
            if (weight < maxWeight || maxWeight == -1) maxWeight = weight;
    }
}

void ApplyThrustOverride(Vector3 force, bool enable)
{
    if (!enable)
    {
        for (int i = 0; i < thrusters.Count; i++)
            thrusters[i].SetValueFloat("Override", 0.0f);
        return;
    }

    Vector3 available = GetThrustMap(force, null);
    float xRatio = Math.Min(1, Math.Abs(SafeDiv(force.X, available.X)));
    float yRatio = Math.Min(1, Math.Abs(SafeDiv(force.Y, available.Y)));
    float zRatio = Math.Min(1, Math.Abs(SafeDiv(force.Z, available.Z)));

    for (int i = 0; i < thrusters.Count; i++)
    {
        IMyThrust thruster = thrusters[i];
        Vector3 dir = RoundVector(LocalTransformDirection(remoteControl, thruster.WorldMatrix.Backward), 1);
        if (dir.X != 0 && Math.Sign(dir.X) == Math.Sign(force.X))
            thruster.SetValueFloat("Override", thruster.MaxThrust * xRatio);
        else if (dir.Y != 0 && Math.Sign(dir.Y) == Math.Sign(force.Y))
            thruster.SetValueFloat("Override", thruster.MaxThrust * yRatio);
        else if (dir.Z != 0 && Math.Sign(dir.Z) == Math.Sign(force.Z))
            thruster.SetValueFloat("Override", thruster.MaxThrust * zRatio);
        else
            thruster.SetValueFloat("Override", 0.0f);
    }
}

void SetGyroOverride(bool enable, float power, float pitch, float roll, float yaw)
{
    for (int i = 0; i < gyros.Count; i++)
    {
        IMyGyro gyro = gyros[i];
        gyro.GyroOverride = enable;
        if (!enable) gyro.GyroPower = 100;
        else gyro.GyroPower = power;
        if (!enable) continue;

        Vector3 fwd = remoteControl.WorldMatrix.Forward;
        Vector3 right = remoteControl.WorldMatrix.Right;
        Vector3 up = remoteControl.WorldMatrix.Up;
        Vector3 gFwd = gyro.WorldMatrix.Forward;
        Vector3 gUp = gyro.WorldMatrix.Up;
        Vector3 gRight = gyro.WorldMatrix.Left * -1;

        if (gFwd == fwd) gyro.SetValueFloat("Roll", yaw);
        else if (gFwd == (fwd * -1)) gyro.SetValueFloat("Roll", yaw * -1);
        else if (gUp == (fwd * -1)) gyro.SetValueFloat("Yaw", yaw);
        else if (gUp == fwd) gyro.SetValueFloat("Yaw", yaw * -1);
        else if (gRight == fwd) gyro.SetValueFloat("Pitch", yaw);
        else if (gRight == (fwd * -1)) gyro.SetValueFloat("Pitch", yaw * -1);

        if (gRight == (right * -1)) gyro.SetValueFloat("Pitch", pitch);
        else if (gRight == right) gyro.SetValueFloat("Pitch", pitch * -1);
        else if (gUp == right) gyro.SetValueFloat("Yaw", pitch);
        else if (gUp == (right * -1)) gyro.SetValueFloat("Yaw", pitch * -1);
        else if (gFwd == (right * -1)) gyro.SetValueFloat("Roll", pitch);
        else if (gFwd == right) gyro.SetValueFloat("Roll", pitch * -1);

        if (gUp == (up * -1)) gyro.SetValueFloat("Yaw", roll);
        else if (gUp == up) gyro.SetValueFloat("Yaw", roll * -1);
        else if (gRight == up) gyro.SetValueFloat("Pitch", roll);
        else if (gRight == (up * -1)) gyro.SetValueFloat("Pitch", roll * -1);
        else if (gFwd == up) gyro.SetValueFloat("Roll", roll);
        else if (gFwd == (up * -1)) gyro.SetValueFloat("Roll", roll * -1);
    }
}

// ========================== 21_NavTargets.cs ==========================
// ========================== NAV TARGET SETTERS ==========================

void StopGyroOverride() { this.gyroActive = false; }

void AlignToDown(Vector3 down, Vector3 fwd, Vector3 left, float threshold, bool useGravity)
{
    AlignToGravity(down, threshold, useGravity);
    alignThreshold = threshold;
    lookAtTarget = false;
    this.targetForward = fwd;
    this.targetLeft = left;
}

void AlignToDown(Vector3 down, Vector3 fwd, Vector3 left, bool useGravity)
{
    AlignToDown(down, fwd, left, 2f, useGravity);
}

void AlignToGravity(Vector3 down, float threshold, bool useGravity)
{
    alignThreshold = threshold;
    this.gyroActive = true;
    this.alignToGravity = useGravity;
    lookAtTarget = true;
    isAligned = false;
    this.targetDown = down;
}

void StopFlight()
{
    SetFlightTargetFull(false, false, false, navTargetPos, 0);
    thrustActive = false;
}

void SetFlightTarget(Vector3 target, float speed)
{
    SetFlightTargetFull(true, false, false, target, speed);
}

void SetFlightTargetFull(bool enable, bool soft, bool useSegment, Vector3 target, float speed)
{
    SetFlightTargetFull(enable, soft, useSegment, target, target - shipPosition, 0.0f, speed);
}

void SetFlightTargetFull(bool enable, bool soft, bool useSegment, Vector3 target, Vector3 segDir, float segSpeed, float speed)
{
    thrustActive = true;
    this.thrustEnabled = enable;
    navTargetPos = target;
    this.maxApproachSpeed = speed;
    this.pathSpeed = segSpeed;
    this.softApproach = soft;
    this.slowOnMisalign = useSegment;
    this.flightPathDir = segDir;
    distToTarget = Vector3.Distance(target, shipPosition);
}

// ========================== 22_BlockScan.cs ==========================
// ========================== BLOCK MANAGEMENT ==========================

void ScanAllBlocks() { gridTerminalSystem.GetBlocksOfType(allGridBlocks, IsOnSameGrid); }

void ScanBlocks1()
{
    if (resetRequested) { setupErrorLevel = 2; return; }

    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
    List<IMySensorBlock> sensors = new List<IMySensorBlock>();
    List<IMyTerminalBlock> taggedBlocks = new List<IMyTerminalBlock>();

    gridTerminalSystem.GetBlocksOfType(remotes, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(lcdPanels, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(sensors, IsOnSameGrid);
    gridTerminalSystem.SearchBlocksOfName(pamTag.Substring(0, pamTag.Length - 1) + ":", taggedBlocks,
        q => q.CubeGrid == Me.CubeGrid && q is IMyTextSurfaceProvider);

    lcdPanels = FilterByTag(lcdPanels, pamTag, true);
    SetupDebugPanels();
    SetupTaggedSurfaces(taggedBlocks);
    SetLCDFormat(lcdPanels, setLCDFontAndSize, 1.4f, false);
    SetLCDFormat(textSurfaces, setLCDFontAndSize, 1.4f, true);

    List<IMySensorBlock> taggedSensors = FilterByTag(sensors, pamTag, true);
    if (taggedSensors.Count > 0) sensor = taggedSensors[0]; else sensor = null;

    if (shipMode == ShipMode.Miner)
    {
        gridTerminalSystem.GetBlocksOfType(workTools, q => q.CubeGrid == Me.CubeGrid && q is IMyShipDrill);
        if (workTools.Count == 0) { setupErrorLevel = 1; statusMessage = "Drills are missing"; }
    }
    else if (shipMode == ShipMode.Grinder)
    {
        gridTerminalSystem.GetBlocksOfType(workTools, q => q.CubeGrid == Me.CubeGrid && q is IMyShipGrinder);
        if (workTools.Count == 0) { setupErrorLevel = 1; statusMessage = "Grinders are missing"; }
        if (sensor == null) { setupErrorLevel = 1; statusMessage = "Sensor is missing"; }
    }
    else if (shipMode == ShipMode.Shuttle)
    {
        gridTerminalSystem.GetBlocksOfType(timerBlocks, q => q.CubeGrid == Me.CubeGrid);
    }

    List<IMyRemoteControl> taggedRemotes = FilterByTag(remotes, pamTag, true);
    if (taggedRemotes.Count > 0) remotes = taggedRemotes;
    if (remotes.Count > 0) remoteControl = remotes[0];
    else { remoteControl = null; setupErrorLevel = 2; statusMessage = "Remote is missing"; return; }

    shipDiameter = (float)remoteControl.CubeGrid.WorldVolume.Radius * 2;
    referenceWorkTool = null;

    if (shipMode != ShipMode.Shuttle)
    {
        CalculateToolDimensions();
        if (workTools.Count > 0 && referenceWorkTool != null)
        {
            if (sensor != null && (referenceWorkTool.WorldMatrix.Forward != sensor.WorldMatrix.Forward ||
                !(remoteControl.WorldMatrix.Forward == sensor.WorldMatrix.Up || remoteControl.WorldMatrix.Down == sensor.WorldMatrix.Down)))
            { setupErrorLevel = 1; statusMessage = "Wrong sensor direction"; }
            if (referenceWorkTool.WorldMatrix.Forward != remoteControl.WorldMatrix.Forward &&
                referenceWorkTool.WorldMatrix.Forward != remoteControl.WorldMatrix.Down)
            { setupErrorLevel = 2; statusMessage = "Wrong remote direction"; }
        }
    }
}

void ScanBlocks2()
{
    gridTerminalSystem.GetBlocksOfType(landingGears, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(connectors, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(thrusters, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(gyros, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(batteries, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(reactors, IsOnSameGrid);
    gridTerminalSystem.GetBlocksOfType(hydrogenTanks,
        q => q.CubeGrid == Me.CubeGrid && q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));
    gridTerminalSystem.GetBlocksOfType(sorters, IsOnSameGrid);

    if (Me.CubeGrid.GridSizeEnum == MyCubeSize.Small)
        connectors = FilterByTag(connectors, "ConnectorMedium", false);
    else
        connectors = FilterByTag(connectors, "Connector", false);

    List<IMyShipConnector> taggedConns = FilterByTag(connectors, pamTag, true);
    if (taggedConns.Count > 0) connectors = taggedConns;

    if (setupErrorLevel <= 1)
    {
        if (connectors.Count == 0) { setupErrorLevel = 1; statusMessage = "Connector is missing"; }
        if (gyros.Count == 0) { setupErrorLevel = 1; statusMessage = "Gyros are missing"; }
        if (thrusters.Count == 0) { setupErrorLevel = 1; statusMessage = "Thrusters are missing"; }
    }

    List<IMyConveyorSorter> taggedSorters = FilterByTag(sorters, pamTag, true);
    if (taggedSorters.Count > 0) sorters = taggedSorters;
    List<IMyLandingGear> taggedGears = FilterByTag(landingGears, pamTag, true);
    if (taggedGears.Count > 0) landingGears = taggedGears;
    for (int i = 0; i < landingGears.Count; i++) landingGears[i].AutoLock = false;
    List<IMyBatteryBlock> taggedBatteries = FilterByTag(batteries, pamTag, true);
    if (taggedBatteries.Count > 0) batteries = taggedBatteries;
    List<IMyGasTank> taggedTanks = FilterByTag(hydrogenTanks, pamTag, true);
    if (taggedTanks.Count > 0) hydrogenTanks = taggedTanks;
}

void ScanBlocks3()
{
    gridTerminalSystem.GetBlocksOfType(allInventoryBlocks, q => q.CubeGrid == Me.CubeGrid && q.InventoryCount > 0);
    cargoBlocks.Clear();
    for (int i = allInventoryBlocks.Count - 1; i >= 0; i--)
    {
        if (IsCargoBlock(allInventoryBlocks[i]))
        {
            cargoBlocks.Add(allInventoryBlocks[i]);
            allInventoryBlocks.RemoveAt(i);
        }
    }
}

bool IsCargoBlock(IMyTerminalBlock block)
{
    if (block is IMyCargoContainer) return true;
    if (block is IMyShipDrill) return true;
    if (block is IMyShipGrinder) return true;
    if (block is IMyShipConnector)
    {
        if (((IMyShipConnector)block).ThrowOut) return false;
        if (Me.CubeGrid.GridSizeEnum != MyCubeSize.Large && HasTag(block, "ConnectorSmall", false))
            return false;
        else return true;
    }
    return false;
}

void CalculateToolDimensions()
{
    if (remoteControl == null) return;
    referenceWorkTool = null;

    float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;
    List<IMyTerminalBlock> tagged = FilterByTag(workTools, pamTag, true);
    bool noTagged = tagged.Count == 0;
    if (tagged.Count > 0) referenceWorkTool = tagged[0];
    else if (workTools.Count > 0) referenceWorkTool = workTools[0];

    int count = 0;
    float radius = 0;
    for (int i = 0; i < workTools.Count; i++)
    {
        if (workTools[i].WorldMatrix.Forward != referenceWorkTool.WorldMatrix.Forward)
        {
            if (noTagged) { setupErrorLevel = 2; statusMessage = "Mining direction is unclear!"; return; }
            continue;
        }
        count++;
        radius = Math.Max(radius, ToolRadius(workTools[i]));
        Vector3 localPos = GetLocalPosition(remoteControl, workTools[i].GetPosition());
        if (i == 0) { minX = localPos.X; maxX = localPos.X; minY = localPos.Y; maxY = localPos.Y; minZ = localPos.Z; maxZ = localPos.Z; }
        maxX = Math.Max(localPos.X, maxX); minX = Math.Min(localPos.X, minX);
        maxY = Math.Max(localPos.Y, maxY); minY = Math.Min(localPos.Y, minY);
        maxZ = Math.Max(localPos.Z, maxZ); minZ = Math.Min(localPos.Z, minZ);
    }
    if (radius == 0) radius = drillRadius;
    bool down = referenceWorkTool != null && referenceWorkTool.WorldMatrix.Forward == remoteControl.WorldMatrix.Down;
    toolWidth = (maxX - minX) * (1 - widthOverlap) + radius * 2;
    toolHeight = (down ? maxZ - minZ : maxY - minY) * (1 - heightOverlap) + radius * 2;

    // ship footprint across the drill face (same axes as the hole grid): co-op ships don't take holes
    // where their hulls would touch (HolesTouch), so a ship wider than the area it drills skips neighbours
    IMyCubeGrid g = remoteControl.CubeGrid;
    float x0 = 0, x1 = 0, y0 = 0, y1 = 0;
    for (int c = 0; c < 8; c++)
    {
        Vector3 p = GetLocalPosition(remoteControl, g.GridIntegerToWorld(new Vector3I((c & 1) == 0 ? g.Min.X : g.Max.X,
            (c & 2) == 0 ? g.Min.Y : g.Max.Y, (c & 4) == 0 ? g.Min.Z : g.Max.Z)));
        float y = down ? p.Z : p.Y;
        if (c == 0 || p.X < x0) x0 = p.X;
        if (c == 0 || p.X > x1) x1 = p.X;
        if (c == 0 || y < y0) y0 = y;
        if (c == 0 || y > y1) y1 = y;
    }
    gridHalf = new Vector3(g.Max - g.Min + Vector3I.One) * g.GridSize * 0.5f;
    shipFootWidth = x1 - x0 + g.GridSize;
    shipFootHeight = y1 - y0 + g.GridSize;
}

// cut radius of a drill (from the game's block definitions)
float ToolRadius(IMyTerminalBlock b)
{
    if (!(b is IMyShipDrill)) return drillRadius;
    String s = b.BlockDefinition.SubtypeId;
    if (s == "SmallBlockDrill") return 1.3f;
    if (s == "LargeBlockDrill") return 1.9f;
    if (s == "LargeBlockPrototechDrill") return 6f;
    return drillRadius;
}

List<T> FilterByTag<T>(List<T> list, String tag, bool inName)
{
    List<T> result = new List<T>();
    for (int i = 0; i < list.Count; i++)
        if (HasTag(list[i], tag, inName)) result.Add(list[i]);
    return result;
}

bool HasTag<T>(T block, String tag, bool inName)
{
    IMyTerminalBlock b = (IMyTerminalBlock)block;
    if (inName && b.CustomName.ToUpper().Contains(tag.ToUpper())) return true;
    if (!inName && b.BlockDefinition.ToString().ToUpper().Contains(tag.ToUpper())) return true;
    return false;
}


// ========================== BLOCK CONTROL HELPERS ==========================

void SetBlocksEnabled<T>(List<T> list, bool enable)
{
    for (int i = 0; i < list.Count; i++)
        SetBlockEnabled((IMyTerminalBlock)list[i], enable);
}

void SetBlockEnabled(IMyTerminalBlock block, bool enable)
{
    if (block == null) return;
    String action = enable ? "OnOff_On" : "OnOff_Off";
    var a = block.GetActionWithName(action);
    a.Apply(block);
}

void SetSortersEnabled(bool enable)
{
    sorterState = enable;
    if (!toggleSortersEnabled) return;
    SetBlocksEnabled(sorters, enable);
}

void SetBatteryChargeMode(ChargeMode mode)
{
    for (int i = 0; i < batteries.Count; i++) batteries[i].ChargeMode = mode;
}

void SetHydrogenStockpile(bool stockpile)
{
    for (int i = 0; i < hydrogenTanks.Count; i++) hydrogenTanks[i].Stockpile = stockpile;
}

void SetLandingGears(List<IMyLandingGear> gears, bool lockState)
{
    for (int i = 0; i < gears.Count; i++)
    {
        if (lockState) gears[i].Lock();
        if (!lockState) gears[i].Unlock();
    }
}

void SetDampeners(bool enable)
{
    if (dampenerState == enable) return;
    List<IMyShipController> controllers = new List<IMyShipController>();
    gridTerminalSystem.GetBlocksOfType(controllers, IsOnSameGrid);
    if (controllers.Count == 0) return;
    for (int i = 0; i < controllers.Count; i++)
        controllers[i].DampenersOverride = enable;
    dampenerState = enable;
}

IMyShipConnector FindConnector(MyShipConnectorStatus status)
{
    for (int i = 0; i < connectors.Count; i++)
    {
        if (!IsBlockValid(connectors[i], true)) continue;
        if (connectors[i].Status == status) return connectors[i];
    }
    return null;
}

void UpdateSensor(bool showOnHUD, bool detect, float frontRange, float backRange)
{
    if (sensor == null || workTools.Count == 0) return;

    Vector3 center = new Vector3();
    int count = 0;
    for (int i = 0; i < workTools.Count; i++)
    {
        if (workTools[i].WorldMatrix.Forward != referenceWorkTool.WorldMatrix.Forward) continue;
        count++;
        center += workTools[i].GetPosition();
    }
    center = center / count;

    Vector3 offset = GetLocalPosition(sensor, center);
    sensor.Enabled = true;
    sensor.ShowOnHUD = showOnHUD;
    sensor.LeftExtend = (detect ? 1 : configWidth) / 2f * toolWidth - offset.X;
    sensor.RightExtend = (detect ? 1 : configWidth) / 2f * toolWidth + offset.X;
    sensor.TopExtend = (detect ? 1 : configHeight) / 2f * toolHeight + offset.Y;
    sensor.BottomExtend = (detect ? 1 : configHeight) / 2f * toolHeight - offset.Y;
    sensor.FrontExtend = (showOnHUD ? configDepth : frontRange) - offset.Z;
    sensor.BackExtend = showOnHUD ? 0 : backRange + shipDiameter * 0.75f + offset.Z;
    sensor.DetectFloatingObjects = true;
    sensor.DetectAsteroids = false;
    sensor.DetectLargeShips = true;
    sensor.DetectSmallShips = true;
    sensor.DetectStations = true;
    sensor.DetectOwner = true;
    sensor.DetectSubgrids = false;
    sensor.DetectPlayers = false;
    sensor.DetectEnemy = true;
    sensor.DetectFriendly = true;
    sensor.DetectNeutral = true;
}

HashSet<IMyTextSurface> lcdFormatted = new HashSet<IMyTextSurface>();

void SetLCDFormat<T>(List<T> list, bool setFont, float fontSize, bool isSurface)
{
    for (int i = 0; i < list.Count; i++)
    {
        IMyTextSurface surface = null;
        if (list[i] is IMyTextSurface) surface = (IMyTextSurface)list[i];
        if (surface != null)
        {
            surface.ContentType = ContentType.TEXT_AND_IMAGE;
            if (!setFont || !lcdFormatted.Add(surface)) continue; // font / size only the first time: then your own settings stay
            surface.Font = "Debug";
            if (isSurface) continue;
            surface.FontSize = fontSize;
        }
    }
}

bool IsNearPlanet()
{
    Vector3D pos;
    return this.remoteControl.TryGetPlanetPosition(out pos);
}

int GetPathDirection(Vector3 targetPos, int startIndex)
{
    if (startIndex == -1) return 0;
    double closest = -1;
    int closestIdx = -1;
    for (int i = waypoints.Count - 1; i >= 0; i--)
    {
        double dist = Vector3.Distance(waypoints[i].position, targetPos);
        if (closest == -1 || dist < closest) { closestIdx = i; closest = dist; }
    }
    return Math.Sign(closestIdx - startIndex);
}

bool IsOnPath(Vector3 pos)
{
    List<Vector3> points = new List<Vector3>();
    for (int i = 0; i < waypoints.Count; i++) points.Add(waypoints[i].position);

    if (homeDock.isSet && waypoints.Count >= 1)
    {
        Vector3 approachPos = new Vector3();
        GetDockApproachPosition(homeDock, dockDist * shipDiameter, false, out approachPos);
        if (Vector3.Distance(homeDock.position, waypoints.First().position) < Vector3.Distance(homeDock.position, waypoints.Last().position))
        { points.Insert(0, approachPos); points.Insert(0, homeDock.position); }
        else { points.Add(approachPos); points.Add(homeDock.position); }
    }

    if (shipMode == ShipMode.Shuttle)
    {
        if (jobPosition.isSet && waypoints.Count >= 1)
        {
            Vector3 approachPos2 = new Vector3();
            GetDockApproachPosition(jobPosition, dockDist * shipDiameter, false, out approachPos2);
            if (Vector3.Distance(jobPosition.position, waypoints.First().position) < Vector3.Distance(jobPosition.position, waypoints.Last().position))
            { points.Insert(0, approachPos2); points.Insert(0, jobPosition.position); }
            else { points.Add(approachPos2); points.Add(jobPosition.position); }
        }
    }
    else
    {
        if (jobState != JobState.NoJob)
            if (waypoints.Count > 0 && Vector3.Distance(jobPosition.position, waypoints.First().position) < Vector3.Distance(jobPosition.position, waypoints.Last().position))
                points.Insert(0, jobPosition.position);
            else points.Add(jobPosition.position);
    }

    int closestIdx = -1;
    double closestDist = -1;
    for (int i = 0; i < points.Count; i++)
    {
        double dist = Vector3.Distance(points[i], pos);
        if (dist < closestDist || closestDist == -1) { closestDist = dist; closestIdx = i; }
    }

    if (points.Count == 0) return false;
    double distToClosest = Vector3.Distance(points[closestIdx], pos);
    double prevSpacing = Vector3.Distance(points[Math.Max(0, closestIdx - 1)], points[closestIdx]) * 1.5f;
    double nextSpacing = Vector3.Distance(points[Math.Min(points.Count - 1, closestIdx + 1)], points[closestIdx]) * 1.5f;
    // ships fly up to laneOffset * ship size beside the path (traffic lanes)
    return distToClosest < prevSpacing || distToClosest < nextSpacing || distToClosest < laneOffset * shipDiameter * 1.5f;
}

// ========================== 23_MathAndSerial.cs ==========================
// ========================== MATH UTILITIES ==========================

static float SafeDiv(float a, float b) { if (b == 0) return 0; return a / b; }

// all timers use UTC: local time jumps an hour at the daylight saving change (timeouts would stop
// expiring for that hour)
static DateTime TimeNow() { return DateTime.UtcNow; }

Vector3 RoundVector(Vector3 v, int decimals)
{
    return new Vector3(Math.Round(v.X, decimals), Math.Round(v.Y, decimals), Math.Round(v.Z, decimals));
}

float AngleBetween(Vector3 a, Vector3 b)
{
    if (a == b) return 0;
    float dot = (a * b).Sum;
    float lenA = a.Length();
    float lenB = b.Length();
    if (lenA == 0 || lenB == 0) return 0;
    float result = (float)((180.0f / Math.PI) * Math.Acos(dot / (lenA * lenB)));
    if (float.IsNaN(result)) return 0;
    return result;
}

float AngleWithSign(Vector3 v, Vector3 reference, float signValue)
{
    float angle = AngleBetween(v, reference);
    if (signValue > 0f) angle *= -1;
    if (angle > -90f) return angle - 90f;
    else return 180f - (-angle - 90f);
}

double DegToRad(float degrees) { return (Math.PI / 180) * degrees; }

bool InRange(double min, double value, double max) { return (value >= min && value <= max); }

Vector3 TransformToWorld(IMyTerminalBlock block, Vector3 localPos)
{
    return Vector3D.Transform(localPos, block.WorldMatrix);
}

Vector3 GetLocalPosition(IMyTerminalBlock block, Vector3 worldPos)
{
    return LocalTransformDirection(block, worldPos - block.GetPosition());
}

Vector3 LocalTransformDirection(IMyTerminalBlock block, Vector3 worldDir)
{
    return Vector3D.TransformNormal(worldDir, MatrixD.Transpose(block.WorldMatrix));
}

Vector3 TransformToLocal(Vector3 forward, Vector3 up, Vector3 worldDir)
{
    MatrixD matrix = MatrixD.CreateFromDir(forward, up);
    return Vector3D.TransformNormal(worldDir, MatrixD.Transpose(matrix));
}

Vector3 TransformLocalToWorld(Vector3 forward, Vector3 up, Vector3 localPos)
{
    MatrixD matrix = MatrixD.CreateFromDir(forward, up);
    return Vector3D.Transform(localPos, matrix);
}


// ========================== SERIALIZATION ==========================

String VectorToString(Vector3 v) { return "" + v.X + "|" + v.Y + "|" + v.Z; }

Vector3 StringToVector(String s)
{
    String[] parts = s.Split('|');
    return new Vector3(float.Parse(SafeElement(parts, 0)), float.Parse(SafeElement(parts, 1)), float.Parse(SafeElement(parts, 2)));
}

String WaypointToString(WaypointInfo wp)
{
    String sep = ":";
    String result = VectorToString(wp.position) + sep + VectorToString(wp.forwardDir) + sep +
        VectorToString(wp.downDir) + sep + VectorToString(wp.leftDir) + sep + VectorToString(wp.gravity);
    for (int i = 0; i < wp.thrusterEfficiency.Length; i++)
        result += sep + Math.Round(wp.thrusterEfficiency[i], 3);
    return result;
}

WaypointInfo StringToWaypoint(String s)
{
    String[] parts = s.Split(':');
    WaypointInfo wp = new WaypointInfo(
        StringToVector(SafeElement(parts, 0)), StringToVector(SafeElement(parts, 1)),
        StringToVector(SafeElement(parts, 2)), StringToVector(SafeElement(parts, 3)),
        StringToVector(SafeElement(parts, 4)));
    int idx = 5;
    List<float> effList = new List<float>();
    while (idx < parts.Length)
    {
        String val = SafeElement(parts, idx);
        float f = 0;
        if (!float.TryParse(val, out f)) break;
        effList.Add(f);
        idx++;
    }
    wp.thrusterEfficiency = effList.ToArray();
    return wp;
}

// Save() builds into this buffer and assigns Storage once (Storage += per value was quadratic on long paths).
System.Text.StringBuilder saveBuffer = new System.Text.StringBuilder();
void AppendStorage<T>(T value, bool newLine) { if (newLine) saveBuffer.Append('\n'); saveBuffer.Append(value); }
void AppendStorage<T>(T value) { AppendStorage(value, true); }

// Block names can contain the Storage separators (';' between sections, '\n' between values).
// Escape them so a timer called e.g. "Dock;1" can't corrupt the save. The escapes themselves must
// not contain ';' '\n' '|' or ':' (an earlier "&#59;" version did, which broke the save).
String EscapeStored(String s) { return s.Replace("%", "%25").Replace(";", "%3B").Replace("\n", "%0A"); }
String UnescapeStored(String s) { return s.Replace("%0A", "\n").Replace("%3B", ";").Replace("%25", "%"); }

String SafeElement(String[] arr, int idx)
{
    String val = arr.ElementAtOrDefault(idx);
    if (String.IsNullOrEmpty(val)) return "";
    return val;
}

// ========================== 24_Persistence.cs ==========================
// ========================== SAVE / LOAD ==========================

void Save()
{
    if (resetRequested || shipMode == ShipMode.Controller) { Storage = ""; return; }
    Storage = BuildSaveData();
}

// Everything PAM persists (path, home dock, job position, mining area, settings) as one string.
// Used by Save() and by EXPORT.
String BuildSaveData()
{
    saveBuffer.Clear();
    saveBuffer.Append(DATAREV + ";");
    AppendStorage(VectorToString(referencePosition), false);
    AppendStorage(VectorToString(homeDock.forwardDir));
    AppendStorage(VectorToString(homeDock.leftDir));
    AppendStorage(VectorToString(homeDock.downDir));
    AppendStorage(VectorToString(homeDock.gravity));
    AppendStorage(VectorToString(homeDock.position));
    AppendStorage(VectorToString(homeDock.connectorGridPos));
    AppendStorage(homeDock.isSet);
    AppendStorage(VectorToString(jobPosition.position));
    AppendStorage(VectorToString(jobPosition.gravity));
    AppendStorage(VectorToString(jobPosition.forwardDir));
    AppendStorage(VectorToString(jobPosition.downDir));
    AppendStorage(VectorToString(jobPosition.leftDir));
    AppendStorage(VectorToString(jobPosition.connectorGridPos));
    AppendStorage(jobPosition.isSet);
    AppendStorage(VectorToString(miningForward));
    AppendStorage(VectorToString(miningDown));
    AppendStorage(";");
    AppendStorage((int)shipMode, false);
    AppendStorage((int)jobState); AppendStorage((int)lastJobState);
    AppendStorage(maxLoadPercent); AppendStorage(minBatteryPercent); AppendStorage(minUraniumKg);
    AppendStorage(minHydrogenPercent); AppendStorage(accelerationFactor);
    AppendStorage(weightLimitEnabled); AppendStorage(unloadIce);

    if (shipMode == ShipMode.Shuttle)
    {
        AppendStorage((int)undockConfig1.trigger); AppendStorage(undockConfig1.delay);
        AppendStorage(undockConfig1.elapsedDelay); AppendStorage(EscapeStored(undockConfig1.dockTimerName)); AppendStorage(EscapeStored(undockConfig1.leaveTimerName));
        AppendStorage((int)undockConfig2.trigger); AppendStorage(undockConfig2.delay);
        AppendStorage(undockConfig2.elapsedDelay); AppendStorage(EscapeStored(undockConfig2.dockTimerName)); AppendStorage(EscapeStored(undockConfig2.leaveTimerName));
    }
    else
    {
        AppendStorage((int)startPosition); AppendStorage((int)onDamageBehavior); AppendStorage((int)ejectionMode);
        AppendStorage((int)depthMode); AppendStorage((int)jobStartPos); AppendStorage(whenDoneReturnHome);
        AppendStorage(toggleSortersEnabled); AppendStorage(invBalancingEnabled); AppendStorage(enableDrillsBothWays);
        AppendStorage(configWidth); AppendStorage(configHeight); AppendStorage(configDepth);
        AppendStorage(workSpeedForward); AppendStorage(workSpeedBackward);
        AppendStorage(widthOverlap); AppendStorage(heightOverlap);
        AppendStorage(jobWidth); AppendStorage(jobHeight);
        AppendStorage(holeCol); AppendStorage(holeRow);
        AppendStorage(lastHoleIndex); AppendStorage(currentHoleIndex);
        AppendStorage(runtimeMaxDepth); AppendStorage(maxMineDepth);
    }
    AppendStorage(";");
    for (int i = 0; i < thrusterTypeNames.Count; i++)
        AppendStorage((i > 0 ? "|" : "") + thrusterTypeNames[i], false);
    AppendStorage(";");
    for (int i = 0; i < waypoints.Count; i++)
        AppendStorage(WaypointToString(waypoints[i]), i > 0);
    // section 5 (co-op holes) was added after 1.3.1; older saves simply don't have it
    AppendStorage(";", false);
    AppendStorage(doneHoles + "|" + myHole + "|" + jobToolW + "|" + jobToolH + "|" + jobSerial, false);
    // section 6 (dock pool)
    AppendStorage(";", false);
    AppendStorage(DockPoolToString(), false);
    String data = saveBuffer.ToString();
    saveBuffer.Clear();
    return data;
}

DataResult RestoreData() { return RestoreFrom(Storage); }

// Parses data written by BuildSaveData(). Used at load and by IMPORT.
DataResult RestoreFrom(String data)
{
    if (data == "") return DataResult.NoData;
    String[] sections = data.Split(';');
    if (SafeElement(sections, 0) != DATAREV) return DataResult.NewVersion;

    int idx = 0;
    try
    {
        String[] lines = SafeElement(sections, 1).Split('\n');
        referencePosition = StringToVector(SafeElement(lines, idx++));
        homeDock.forwardDir = StringToVector(SafeElement(lines, idx++));
        homeDock.leftDir = StringToVector(SafeElement(lines, idx++));
        homeDock.downDir = StringToVector(SafeElement(lines, idx++));
        homeDock.gravity = StringToVector(SafeElement(lines, idx++));
        homeDock.position = StringToVector(SafeElement(lines, idx++));
        homeDock.connectorGridPos = StringToVector(SafeElement(lines, idx++));
        homeDock.isSet = bool.Parse(SafeElement(lines, idx++));
        jobPosition.position = StringToVector(SafeElement(lines, idx++));
        jobPosition.gravity = StringToVector(SafeElement(lines, idx++));
        jobPosition.forwardDir = StringToVector(SafeElement(lines, idx++));
        jobPosition.downDir = StringToVector(SafeElement(lines, idx++));
        jobPosition.leftDir = StringToVector(SafeElement(lines, idx++));
        jobPosition.connectorGridPos = StringToVector(SafeElement(lines, idx++));
        jobPosition.isSet = bool.Parse(SafeElement(lines, idx++));
        miningForward = StringToVector(SafeElement(lines, idx++));
        miningDown = StringToVector(SafeElement(lines, idx++));

        lines = SafeElement(sections, 2).Split('\n'); idx = 0;
        shipMode = (ShipMode)int.Parse(SafeElement(lines, idx++));
        jobState = (JobState)int.Parse(SafeElement(lines, idx++));
        lastJobState = (JobState)int.Parse(SafeElement(lines, idx++));
        maxLoadPercent = int.Parse(SafeElement(lines, idx++));
        minBatteryPercent = int.Parse(SafeElement(lines, idx++));
        minUraniumKg = int.Parse(SafeElement(lines, idx++));
        minHydrogenPercent = int.Parse(SafeElement(lines, idx++));
        accelerationFactor = float.Parse(SafeElement(lines, idx++));
        weightLimitEnabled = bool.Parse(SafeElement(lines, idx++));
        unloadIce = bool.Parse(SafeElement(lines, idx++));

        if (shipMode == ShipMode.Shuttle)
        {
            undockConfig1.trigger = (UndockTrigger)int.Parse(SafeElement(lines, idx++));
            undockConfig1.delay = float.Parse(SafeElement(lines, idx++));
            undockConfig1.elapsedDelay = float.Parse(SafeElement(lines, idx++));
            undockConfig1.dockTimerName = UnescapeStored(SafeElement(lines, idx++));
            undockConfig1.leaveTimerName = UnescapeStored(SafeElement(lines, idx++));
            undockConfig2.trigger = (UndockTrigger)int.Parse(SafeElement(lines, idx++));
            undockConfig2.delay = float.Parse(SafeElement(lines, idx++));
            undockConfig2.elapsedDelay = float.Parse(SafeElement(lines, idx++));
            undockConfig2.dockTimerName = UnescapeStored(SafeElement(lines, idx++));
            undockConfig2.leaveTimerName = UnescapeStored(SafeElement(lines, idx++));
        }
        else
        {
            startPosition = (StartPosition)int.Parse(SafeElement(lines, idx++));
            onDamageBehavior = (DamageBehavior)int.Parse(SafeElement(lines, idx++));
            ejectionMode = (EjectionMode)int.Parse(SafeElement(lines, idx++));
            depthMode = (DepthMode)int.Parse(SafeElement(lines, idx++));
            jobStartPos = (StartPosition)int.Parse(SafeElement(lines, idx++));
            whenDoneReturnHome = bool.Parse(SafeElement(lines, idx++));
            toggleSortersEnabled = bool.Parse(SafeElement(lines, idx++));
            invBalancingEnabled = bool.Parse(SafeElement(lines, idx++));
            enableDrillsBothWays = bool.Parse(SafeElement(lines, idx++));
            configWidth = int.Parse(SafeElement(lines, idx++));
            configHeight = int.Parse(SafeElement(lines, idx++));
            configDepth = int.Parse(SafeElement(lines, idx++));
            workSpeedForward = float.Parse(SafeElement(lines, idx++));
            workSpeedBackward = float.Parse(SafeElement(lines, idx++));
            widthOverlap = float.Parse(SafeElement(lines, idx++));
            heightOverlap = float.Parse(SafeElement(lines, idx++));
            jobWidth = int.Parse(SafeElement(lines, idx++));
            jobHeight = int.Parse(SafeElement(lines, idx++));
            holeCol = int.Parse(SafeElement(lines, idx++));
            holeRow = int.Parse(SafeElement(lines, idx++));
            lastHoleIndex = int.Parse(SafeElement(lines, idx++));
            currentHoleIndex = int.Parse(SafeElement(lines, idx++));
            runtimeMaxDepth = int.Parse(SafeElement(lines, idx++));
            maxMineDepth = float.Parse(SafeElement(lines, idx++));
        }

        lines = SafeElement(sections, 3).Replace("\n", "").Split('|');
        thrusterTypeNames = lines.ToList();

        lines = SafeElement(sections, 4).Split('\n');
        waypoints.Clear();
        if (lines.Count() >= 1 && lines[0] != "")
            for (int i = 0; i < lines.Length; i++)
                waypoints.Add(StringToWaypoint(SafeElement(lines, i)));

        lines = SafeElement(sections, 5).Split('|');
        doneHoles = SafeElement(lines, 0);
        if (!int.TryParse(SafeElement(lines, 1), out myHole)) myHole = -1;
        if (!float.TryParse(SafeElement(lines, 2), out jobToolW)) jobToolW = 0;
        if (!float.TryParse(SafeElement(lines, 3), out jobToolH)) jobToolH = 0;
        if (!int.TryParse(SafeElement(lines, 4), out jobSerial)) jobSerial = 0;
        if (doneHoles.Length != jobWidth * jobHeight) { doneHoles = ""; myHole = -1; } // older save: rows, not holes
        DockPoolFromString(SafeElement(sections, 6));
    }
    catch { return DataResult.Failed; }

    savedJobState = lastJobState;
    StopAll();
    return DataResult.Success;
}

// ========================== EXPORT / IMPORT ==========================
// EXPORT copies the saved data (path, home dock, job position, mining area, settings) into the
// Custom Data of a block named with dataTag; IMPORT loads it back. Meant for backups and for
// identical copies of the same ship. Custom Data is limited to 64000 characters by the game.

const int CUSTOM_DATA_LIMIT = 64000;

IMyTerminalBlock FindDataBlock()
{
    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    gridTerminalSystem.GetBlocksOfType(blocks, q => q.CubeGrid == Me.CubeGrid && q.CustomName.Contains(dataTag));
    return blocks.Count > 0 ? blocks[0] : null;
}

void ExportData()
{
    IMyTerminalBlock block = FindDataBlock();
    if (block == null) { statusMessage = "Export: name a block " + dataTag; return; }
    String data = "// PAM export " + VERSION + " | " + Me.CubeGrid.CustomName + " | " + waypoints.Count +
        " waypoints | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\n" + BuildSaveData();
    if (data.Length > CUSTOM_DATA_LIMIT)
    {
        statusMessage = "Export too large: " + data.Length + " chars, max " + CUSTOM_DATA_LIMIT;
        return;
    }
    block.CustomData = data;
    statusMessage = "Exported " + waypoints.Count + " waypoints";
}

void ImportData(bool jobOnly)
{
    IMyTerminalBlock block = FindDataBlock();
    if (block == null) { statusMessage = "Import: name a block " + dataTag; return; }
    ImportText(block.CustomData, jobOnly);
}

void ImportText(String data, bool jobOnly)
{
    data = data.Replace("\r", "");
    if (data.StartsWith("//"))
    {
        int nl = data.IndexOf('\n');
        data = nl < 0 ? "" : data.Substring(nl + 1);
    }
    data = data.Trim();
    if (data == "") { statusMessage = "Import: no PAM data in " + dataTag; return; }

    StopAll();
    String backup = BuildSaveData();
    DataResult result = RestoreFrom(data);
    if (result == DataResult.Success && jobOnly)
    {
        // keep this ship's own path, home dock and settings; take only the job from the export,
        // with fresh progress, so several ships can mine the same job (co-op)
        WaypointInfo job = new WaypointInfo(jobPosition);
        Vector3 mineForward = miningForward, mineDown = miningDown;
        int w = configWidth, h = configHeight, d = configDepth;
        StartPosition start = startPosition;
        DepthMode depth = depthMode;
        float overlapW = widthOverlap, overlapH = heightOverlap, jtw = jobToolW, jth = jobToolH;
        int serial = jobSerial;
        RestoreFrom(backup);
        jobPosition = job; miningForward = mineForward; miningDown = mineDown;
        configWidth = w; configHeight = h; configDepth = d;
        startPosition = start; depthMode = depth; widthOverlap = overlapW; heightOverlap = overlapH;
        jobState = JobState.Paused;
        UpdateJobConfig(true, true);
        jobToolW = jtw; jobToolH = jth; jobSerial = serial; // the exporting ship's job: same grid and key on every ship
        jobState = JobState.Paused; lastJobState = JobState.Paused;
        statusMessage = "Imported job, run CONT";
        return;
    }
    if (result == DataResult.Success)
    {
        needPositionUpdate = false;
        myHole = -1; // a claimed hole belongs to the exporting ship, never copied
        statusMessage = "Imported " + waypoints.Count + " waypoints";
        return;
    }
    RestoreFrom(backup); // undo a partial import
    statusMessage = result == DataResult.NewVersion ? "Import: data is from another PAM version" : "Import failed, nothing changed";
}

// ========================== 25_Display.cs ==========================
// ========================== DISPLAY ==========================

void SetupDebugPanels()
{
    debugPanels.Clear();
    for (int i = lcdPanels.Count - 1; i >= 0; i--)
    {
        String data = lcdPanels[i].CustomData.ToUpper();
        bool isSpecial = false;
        if (data == "INSTRUCTIONS") { isSpecial = true; debugMode = true; }
        if (data == "DEBUG") isSpecial = true;
        if (isSpecial) { debugPanels.Add(lcdPanels[i]); lcdPanels.RemoveAt(i); }
    }
    SetLCDFormat(debugPanels, false, 1, false);
}

void SetupTaggedSurfaces(List<IMyTerminalBlock> blocks)
{
    textSurfaces.Clear();
    for (int i = 0; i < blocks.Count; i++)
    {
        IMyTerminalBlock block = blocks[i];
        try
        {
            String prefix = pamTag.Substring(0, pamTag.Length - 1) + ":";
            int idx = block.CustomName.IndexOf(prefix);
            int surfaceIdx = -1;
            if (idx < 0 || !int.TryParse(block.CustomName.Substring(idx + prefix.Length, 1), out surfaceIdx)) continue;
            if (surfaceIdx == -1) continue;
            surfaceIdx--;
            IMyTextSurfaceProvider provider = (IMyTextSurfaceProvider)block;
            if (surfaceIdx < provider.SurfaceCount && surfaceIdx >= 0)
                textSurfaces.Add(provider.GetSurface(surfaceIdx));
        }
        catch { }
    }
}

String menuCacheText = "";
void PrintDisplays()
{
    String mainText = "";
    String echoText = "";
    menuCacheText = BuildMenuScreen(false);
    mainText += menuCacheText;
    echoText += menuCacheText;

    for (int i = 0; i < lcdPanels.Count; i++) lcdPanels[i].WriteText(mainText);
    for (int i = 0; i < textSurfaces.Count; i++) textSurfaces[i].WriteText(mainText);
    Echo(echoText);

    for (int i = 0; i < debugPanels.Count; i++)
    {
        IMyTextPanel panel = debugPanels[i];
        String data = panel.CustomData.ToUpper();
        if (data == "DEBUG") panel.WriteText("" + "\n" + "");
        if (data == "INSTRUCTIONS") panel.WriteText(GetInstructionInfo());
    }
}

string GetInstructionInfo()
{
    String text = "";
    try
    {
        float maxInst = Runtime.MaxInstructionCount;
        text += "Inst: " + Runtime.CurrentInstructionCount + " Time: " + Math.Round(Runtime.LastRunTimeMs, 3) + "\n";
        text += "Inst. avg/max: " + (int)(avgInstructionCount * maxInst) + " / " + (int)(maxInstructionPercent * maxInst) + "\n";
        text += "Inst. avg/max: " + Math.Round(avgInstructionCount * 100f, 1) + "% / " + Math.Round(maxInstructionPercent * 100f, 1) + "% \n";
        text += "Time avg/max: " + Math.Round(avgRuntime, 2) + "ms / " + Math.Round(maxRuntime, 2) + "ms \n";
    }
    catch { }
    for (int i = 0; i < profilingData.Count; i++)
        text += "" + profilingData.Keys.ElementAt(i) + " = " + Math.Round(profilingData.Values.ElementAt(i)[0], 2) + " / " +
            Math.Round(profilingData.Values.ElementAt(i)[1], 2) + "%\n";
    return text;
}

void HandleError()
{
    String text = "Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n" +
        "Run with argument RETRY to restart.\n" +
        "Version: " + VERSION + "\n";
    SetLCDFormat(lcdPanels, setLCDFontAndSize, 0.9f, false);
    SetLCDFormat(textSurfaces, setLCDFontAndSize, 0.9f, true);
    for (int i = 0; i < lcdPanels.Count; i++) lcdPanels[i].WriteText(text + lastError.ToString());
    for (int i = 0; i < textSurfaces.Count; i++) textSurfaces[i].WriteText(text + lastError.ToString());
}


// ========================== PROFILING ==========================

void StartProfile()
{
    if (!debugMode) return;
    try { profilingStart = Runtime.CurrentInstructionCount; } catch { }
}

void EndProfile(String name)
{
    if (!debugMode) return;
    if (profilingStart == 0) return;
    try
    {
        float usage = (Runtime.CurrentInstructionCount - profilingStart) / Runtime.MaxInstructionCount * 100;
        if (!profilingData.ContainsKey(name))
            profilingData.Add(name, new float[] { usage, usage });
        else { profilingData[name][0] = usage; profilingData[name][1] = Math.Max(usage, profilingData[name][1]); }
    }
    catch { }
}

// ========================== 26_Broadcast.cs ==========================
// ========================== BROADCAST ==========================

const String BROADCAST_TAG = "[PAMCMD]";
const String BROADCAST_SELF_ID = "#";
const Char BROADCAST_SEP = ';';

void BroadcastShipState(String extra)
{
    if (!broadcastEnabled) return;
    String sep = "" + BROADCAST_SEP;
    String msg = VERSION + sep;
    msg += CleanField(shipName) + sep;
    msg += (int)shipMode + sep;
    msg += FormatFloat(shipPosition.X) + "" + sep;
    msg += FormatFloat(shipPosition.Y) + sep;
    msg += FormatFloat(shipPosition.Z) + sep;
    if (shipMode != ShipMode.Shuttle)
        msg += GetJobStateName(lastJobState) + (lastJobState == JobState.Active ? " " + Math.Round(jobProgress, 1) + "%" : "") + sep;
    else msg += GetJobStateName(lastJobState) + sep;
    msg += CleanField(statusMessage) + sep;
    msg += menuIndex + "" + sep;
    msg += CleanField(menuCacheText) + sep;
    msg += currentVolume + sep;
    msg += maxVolume + sep;
    for (int i = 0; i < itemList.Count; i++)
    {
        if (itemList[i].location != ItemLocation.InCargo) continue;
        msg += CleanField(itemList[i].name) + "/" + CleanField(itemList[i].type) + "/" + itemList[i].amount + sep;
    }
    SendBroadcast(msg, extra);
}

// Fields are ';'-separated with no escaping, so a ';' in a ship name, status or menu text would
// shift every following field on the controller. Replace it before sending.
String CleanField(String s) { return s == null ? "" : s.Replace(BROADCAST_SEP, ','); }

void SendBroadcast(String msg, String extra) { SendBroadcastTo(msg, BROADCAST_SELF_ID, extra); }

void SendBroadcastTo(String msg, String target, String extra)
{
    try
    {
        if (!broadcastEnabled) return;
        msg = BROADCAST_TAG + BROADCAST_SEP + GetBroadcastId() + BROADCAST_SEP + target + BROADCAST_SEP + broadcastChannel + BROADCAST_SEP + extra + BROADCAST_SEP + msg;
        this.IGC.SendBroadcastMessage(BROADCAST_TAG, msg);
    }
    catch (Exception e) { lastError = e; }
}

String GetBroadcastId()
{
    if (shipMode != ShipMode.Controller) return "" + Me.GetId();
    return BROADCAST_SELF_ID;
}

bool IsInternalMessage(String msg) { return msg.StartsWith(BROADCAST_TAG); }

bool ExtractField(ref String data, out string field, Char sep)
{
    int idx = data.IndexOf(sep);
    field = "";
    if (idx < 0) return false;
    field = data.Substring(0, idx);
    data = data.Remove(0, idx + 1);
    return true;
}

bool ParseBroadcastMessage(ref String msg, out String senderId, out String extra)
{
    senderId = "";
    extra = "";
    if (!broadcastEnabled) return false;
    String tag = "";
    if (!ExtractField(ref msg, out tag, BROADCAST_SEP) || !IsInternalMessage(tag)) return false;
    if (!ExtractField(ref msg, out senderId, BROADCAST_SEP)) return false;
    if (!ExtractField(ref msg, out tag, BROADCAST_SEP) || (tag != GetBroadcastId() && (tag != "*" && shipMode != ShipMode.Controller))) return false;
    if (!ExtractField(ref msg, out tag, BROADCAST_SEP) || (tag != broadcastChannel)) return false;
    if (!ExtractField(ref msg, out extra, BROADCAST_SEP)) return false;
    return true;
}

void InitBroadcast()
{
    bool configOk = true;
    if (firstBroadcastInit)
    {
        firstBroadcastInit = false;
        if (Me.CustomData.Contains("Antenna_Name"))
        {
            statusMessage = "Update custom data";
            Me.CustomData = SettingsBlock().Trim();
        }
    }

    String header = (shipMode != ShipMode.Controller ? "[PAM-Ship]" : "[PAM-Controller]") + " Broadcast-settings";
    try
    {
        if (Me.CustomData.Length == 0 || Me.CustomData.Split('\n')[0] != header)
            InitBroadcastCustomData(header);

        String[] lines = Me.CustomData.Split('\n');
        broadcastEnabled = bool.Parse(ParseCustomDataValue(FindCustomDataLine(lines, "Enable_Broadcast", ref configOk)));
        bool wasError = false;
        bool disableListener = true;

        if (broadcastEnabled)
        {
            if (shipMode != ShipMode.Controller)
                shipName = ParseCustomDataValue(FindCustomDataLine(lines, "Ship_Name", ref configOk)).Replace(BROADCAST_SELF_ID, "");
            broadcastChannel = ParseCustomDataValue(FindCustomDataLine(lines, "Broadcast_Channel", ref configOk)).ToLower();
            disableListener = false;

            if (broadcastListener == null)
            {
                broadcastListener = this.IGC.RegisterBroadcastListener(BROADCAST_TAG);
                broadcastListener.SetMessageCallback("");
            }

            List<IMyRadioAntenna> antennas = new List<IMyRadioAntenna>();
            gridTerminalSystem.GetBlocksOfType(antennas);
            bool antennaReady = false;
            for (int i = 0; i < antennas.Count; i++)
                if (antennas[i].EnableBroadcasting && antennas[i].Enabled) { antennaReady = true; break; }

            if (antennas.Count == 0) statusMessage = "No Antenna found";
            else if (!antennaReady) statusMessage = "Antenna not ready";
            wasError = antennas.Count == 0 || !antennaReady;
            if (antennaError && !wasError && shipMode != ShipMode.Controller) statusMessage = "Antenna ok";
        }
        else if (shipMode == ShipMode.Controller)
            statusMessage = "Offline - Enable in PB custom data";

        antennaError = wasError;
        if (disableListener) { if (broadcastListener != null) this.IGC.DisableBroadcastListener(broadcastListener); broadcastListener = null; }
    }
    catch { configOk = false; }
    if (!configOk) { statusMessage = "Reset custom data"; InitBroadcastCustomData(header); }
}

void InitBroadcastCustomData(String header)
{
    String data = header + "\n\n" + "Enable_Broadcast=" + (shipMode == ShipMode.Controller ? "true" : "false") + " \n";
    data += shipMode != ShipMode.Controller ? "Ship_Name=Your_ship_name_here\n" : "";
    Me.CustomData = data + "Broadcast_Channel=#default" + SettingsBlock(); // keep the [PAM settings] section
}

String ParseCustomDataValue(String line)
{
    int idx = line.IndexOf("//");
    if (idx != -1) line = line.Substring(0, idx);
    String[] parts = line.Split('=');
    if (parts.Length <= 1) return "";
    return parts[1].Trim();
}

String FindCustomDataLine(String[] lines, String key, ref bool ok)
{
    foreach (String line in lines)
        if (line.StartsWith(key)) return line;
    ok = false;
    return "";
}

// ========================== 27_Controller.cs ==========================
// ========================== CONTROLLER MODE ==========================
// Controller PB: lists PAM ships in antenna range and sends them commands (ported from PAM 1.3.1)

void ControllerMainLoop(string argument)
{
    if (firstRun) connectedShips = new List<ShipInfo>();

    String senderId = "";
    String extra = "";

    if (!IsInternalMessage(argument)) ProcessControllerArgument(argument);

    // Process every queued ship broadcast (one per run fell behind with many ships), sort once.
    bool shipsChanged = false;
    int msgBudget = 50;
    while (broadcastListener != null && broadcastListener.HasPendingMessage && msgBudget-- > 0)
    {
        MyIGCMessage msg = broadcastListener.AcceptMessage();
        String data = msg.Data as String;
        if (data == null) continue;
        if (ParseBroadcastMessage(ref data, out senderId, out extra) && senderId != "" && senderId != BROADCAST_SELF_ID)
        {
            ShipInfo ship = FindShip(senderId, "");
            if (ship == null) { ship = new ShipInfo(senderId); connectedShips.Add(ship); }
            ship.ProcessCommand(extra, false, false, true);
            UpdateShipInfo(ship, data);
            shipsChanged = true;
        }
    }
    // a ship script not heard from for 2 minutes (its PB rebuilt or gone, ship destroyed): off the list,
    // it comes back with its next message
    connectedShips.RemoveAll(q => (TimeNow() - q.lastMessageTime).TotalSeconds > 120);
    if (selectedShip != null && !connectedShips.Contains(selectedShip)) selectedShip = null;
    if (shipsChanged)
        connectedShips.Sort(delegate (ShipInfo a, ShipInfo b)
        {
            if (a.name == null && b.name == null) return 0;
            else if (a.name == null) return -1;
            else if (b.name == null) return 1;
            else return a.name.CompareTo(b.name);
        });

    // block scan, spread over 3 runs (a big station grid made one run spike): screens, docking area, broadcast
    if (firstRun) { statusMessage = ""; firstRun = false; ScanControllerBlocks(); ScanDockArea(); InitBroadcast(); lastScanTime = TimeNow(); }
    else if (isTick && ctrlScanStep > 0)
    {
        if (ctrlScanStep == 1) ScanDockArea();
        else { InitBroadcast(); lastScanTime = TimeNow(); }
        ctrlScanStep = ctrlScanStep == 1 ? 2 : 0;
    }
    else if (isSlowTick && scanCountdown <= 0) { statusMessage = ""; ScanControllerBlocks(); ctrlScanStep = 1; }

    if (isTick) UpdateDockControl(); // station dock control (clearance for one ship at a time)
    if (isTick) UpdateJobCoordination(); // co-op jobs: hand out holes
    if (isTick) UpdateTrafficControl();  // all ships at once: who gives way
    if (isTick) UpdateStagedHome();      // "Move home" for all: one ship after the other
    if (isTick) UpdateScreens();         // recorded paths for the map
    if (isTick && tcSendTick == 0) UpdateDebugDraw(); // 3D debug drawing (DebugAPI mod), twice a second
    UpdateControllerDisplays();
}

void UpdateShipInfo(ShipInfo ship, String data)
{
    ship.lastMessageTime = TimeNow();
    String field = "";
    ExtractField(ref data, out ship.version, BROADCAST_SEP);
    ExtractField(ref data, out ship.name, BROADCAST_SEP);
    if (ship.version != VERSION) return;
    String modeStr = "";
    String[] posStr = new string[3];
    ExtractField(ref data, out modeStr, BROADCAST_SEP);
    ExtractField(ref data, out posStr[0], BROADCAST_SEP);
    ExtractField(ref data, out posStr[1], BROADCAST_SEP);
    ExtractField(ref data, out posStr[2], BROADCAST_SEP);
    ExtractField(ref data, out ship.stateText, BROADCAST_SEP);
    ExtractField(ref data, out ship.statusMessage, BROADCAST_SEP);
    ExtractField(ref data, out field, BROADCAST_SEP);
    ExtractField(ref data, out ship.menuText, BROADCAST_SEP);
    String volStr = "", maxVolStr = "";
    ExtractField(ref data, out volStr, BROADCAST_SEP);
    ExtractField(ref data, out maxVolStr, BROADCAST_SEP);

    ship.items.Clear();
    while (true)
    {
        String itemStr;
        if (!ExtractField(ref data, out itemStr, BROADCAST_SEP)) break;
        String[] itemParts = itemStr.Split('/');
        if (itemParts.Count() < 3) continue;
        int amount = 0;
        if (!int.TryParse(itemParts[2], out amount)) continue;
        ship.items.Add(new ItemInfo(itemParts[0], itemParts[1], amount, ItemLocation.InCargo));
    }

    int modeInt = 0;
    if (!int.TryParse(field, out ship.menuIndex)) ship.menuIndex = 0;
    if (!int.TryParse(modeStr, out modeInt)) ship.shipMode = ShipMode.Unknown;
    if (!float.TryParse(posStr[0], out ship.position.X)) ship.position.X = 0;
    if (!float.TryParse(posStr[1], out ship.position.Y)) ship.position.Y = 0;
    if (!float.TryParse(posStr[2], out ship.position.Z)) ship.position.Z = 0;
    if (!float.TryParse(volStr, out ship.currentVolume)) ship.currentVolume = 0;
    if (!float.TryParse(maxVolStr, out ship.maxVolume)) ship.maxVolume = 0;
    ship.shipMode = (ShipMode)modeInt;
    ship.distance = (int)Math.Round(Vector3.Distance(Me.GetPosition(), ship.position));
    ship.totalItems = 0;
    for (int i = 0; i < ship.items.Count(); i++) ship.totalItems += ship.items[i].amount;
}

ShipInfo FindShip(String id, String name)
{
    id = id.ToUpper(); name = name.ToUpper();
    for (int i = 0; i < connectedShips.Count; i++)
    {
        if (id != "" && connectedShips[i].id.ToUpper() == id) return connectedShips[i];
        if (name != "" && connectedShips[i].name != null && connectedShips[i].name.ToUpper() == name) return connectedShips[i];
    }
    return null;
}

// last command sent from the menus: its result is shown only on the page it was sent from, for a while
String cmdSent = "";
ShipInfo cmdTarget = null;
DateTime cmdTime = DateTime.MinValue;
List<ShipInfo> cmdShips = new List<ShipInfo>();
const float CMD_RESULT_TIME = 20f; //[s]

bool ShowCmdResult(ShipInfo target, String cmd)
{
    return cmd == cmdSent && target == cmdTarget && ((TimeNow() - cmdTime).TotalSeconds < CMD_RESULT_TIME ||
        (cmd == "HOMEPOS" && (homeQueue.Count > 0 || homeCurrent != null))); // staged "Move home" still running
}

// one line per ship the last command went to: received / pending / no answer
String GetCmdResultLines(ShipInfo target)
{
    if (cmdSent == "" || !ShowCmdResult(target, cmdSent)) return "";
    String s = "";
    int ok = 0, shown = 0;
    for (int i = 0; i < cmdShips.Count; i++)
    {
        CommandState st = cmdShips[i].ProcessCommand(cmdSent, false, false, false);
        if (st == CommandState.Received) ok++;
        bool queued = homeQueue.Contains(cmdShips[i]);
        if (shown++ < 8) s += "  " + cmdShips[i].name + ": " + (queued ? "waiting to leave (" + (homeQueue.IndexOf(cmdShips[i]) + 1) + ")" : st == CommandState.Received ? "received" :
            st == CommandState.Pending ? "pending..." : st == CommandState.NoAnswer ? "no answer!" : "-") + "\n";
    }
    if (shown > 8) s += "  +" + (shown - 8) + " more\n";
    return "--------------------------------------------\n" + cmdSent + ": " + ok + "/" + cmdShips.Count + " received\n" + s;
}

void SendCommandToShip(ShipInfo ship, String cmd)
{
    cmdSent = cmd; cmdTarget = ship; cmdTime = TimeNow();
    cmdShips.Clear();
    if (ship == null) cmdShips.AddRange(connectedShips); else cmdShips.Add(ship);
    // a new command replaces a staged "Move home" (for that ship, or for all)
    if (ship == null) { homeQueue.Clear(); homeCurrent = null; } else homeQueue.Remove(ship);
    if (ship == null && cmd == "HOMEPOS") { StartStagedHome(); return; } // one at a time, see UpdateStagedHome
    if (ship == null)
    {
        for (int i = 0; i < connectedShips.Count; i++)
            connectedShips[i].ProcessCommand(cmd, true, false, false);
        SendBroadcastTo(cmd, "*", cmd);
    }
    else
    {
        ship.ProcessCommand(cmd, true, false, false);
        SendBroadcastTo(cmd, ship.id, cmd);
    }
}

void ProcessControllerArgument(string argument)
{
    if (argument == "") return;
    var parts = argument.ToUpper().Split(' ');
    parts.DefaultIfEmpty("");
    var cmd = parts.ElementAtOrDefault(0);
    var arg1 = parts.ElementAtOrDefault(1);
    String invalidMsg = "Invalid argument: " + argument;

    if (currentMenu == MenuPage.ShipList)
    {
        if (selectedShip != null)
        {
            switch (cmd)
            {
                case "UP": { if (menuIndex < 2) break; if (selectedShip.menuIndex == 0) { menuIndex = 1; return; } SendCommandToShip(selectedShip, "UP"); return; }
                case "DOWN": { if (menuIndex < 1) break; if (menuIndex == 1) { SendCommandToShip(selectedShip, "MRES"); break; } SendCommandToShip(selectedShip, "DOWN"); break; }
                case "APPLY": { if (menuIndex < 2) break; SendCommandToShip(selectedShip, "APPLY"); return; }
            }
        }
    }

    switch (cmd)
    {
        case "UP": this.MenuUp(false); return;
        case "DOWN": this.MenuDown(false); return;
        case "APPLY": this.BuildControllerScreen(true); return;
    }

    switch (cmd)
    {
        case "CLEAR": ClearShipList(); return;
        case "SENDALL":
            if (String.IsNullOrEmpty(arg1)) { statusMessage = "Usage: SENDALL <cmd>"; return; }
            SendCommandToShip(null, arg1); return;
        case "SEND":
            if (argument.Length <= "SEND".Length + 1) { statusMessage = "Usage: SEND <ship>:<cmd>"; return; }
            SendToNamedShip(argument.Remove(0, "SEND".Length + 1)); return;
    }
    statusMessage = invalidMsg;
}

void SendToNamedShip(String arg)
{
    if (arg == "") return;
    var parts = arg.Split(':');
    if (parts.Length != 2) { statusMessage = "Missing separator \":\""; return; }
    parts.DefaultIfEmpty("");
    String shipName = parts.ElementAtOrDefault(0).Trim();
    ShipInfo ship = FindShip("", shipName);
    if (ship != null) SendCommandToShip(ship, parts.ElementAtOrDefault(1).Trim());
    else statusMessage = "Unknown ship: " + shipName;
}

void ClearShipList() { connectedShips.Clear(); selectedShip = null; NavigateToMenu(MenuPage.Main); menuIndex = 0; }

void ScanControllerBlocks()
{
    List<IMyTerminalBlock> taggedBlocks = new List<IMyTerminalBlock>();
    gridTerminalSystem.GetBlocksOfType(lcdPanels, IsOnSameGrid);
    gridTerminalSystem.SearchBlocksOfName(pamTag.Substring(0, pamTag.Length - 1) + ":", taggedBlocks,
        q => q.CubeGrid == Me.CubeGrid && q is IMyTextSurfaceProvider);
    lcdPanels = FilterByTag(lcdPanels, pamTag, true);
    SetupTaggedSurfaces(taggedBlocks);
    SetupDebugPanels();
    // controller LCDs are drawn (coloured): only their first font size, for the original screens
    foreach (IMyTextPanel q in lcdPanels) if (setLCDFontAndSize && ModeOf(q) < LCDMode.Fleet && lcdFormatted.Add(q)) q.FontSize = 1.15f;

    // Write the LCD mode template into empty Custom Data; flag outdated Custom Data.
    String helpText = GetLCDModeHelp(false);
    String obsoleteNote = "//Custom Data is obsolete, please delete it.\n\n";
    foreach (IMyTerminalBlock panel in lcdPanels)
    {
        if (panel.CustomData == "") { panel.CustomData = GetLCDModeHelp(true); continue; }
        if (!panel.CustomData.Contains(helpText))
            if (!panel.CustomData.Contains(obsoleteNote)) panel.CustomData = obsoleteNote + panel.CustomData;
    }
}

void UpdateControllerDisplays()
{
    String echoText = "[PAM]-Controller\n\n" + "Run-arguments: (Type without:[ ])\n" + "———————————————\n" +
        "[UP] Menu navigation up\n" + "[DOWN] Menu navigation down\n" + "[APPLY] Apply menu point\n" +
        "[CLEAR] Clear miner list\n" + "[SEND ship:cmd] Send to a ship*\n" + "[SENDALL cmd] Send to all ships*\n" +
        "———————————————\n\n" + "*[SEND] = Cmd to one ship:\n" + " e.g.: \"SEND Miner 1:homepos\"\n\n" +
        "*[SENDALL] = Cmd to all ships:\n" + " e.g.: \"SENDALL homepos\"\n\n";

    for (int i = 0; i < textSurfaces.Count; i++)
        DrawTextScreen(textSurfaces[i], BuildControllerScreenForMode(LCDMode.MainCompact, "0", false, LinesFor(textSurfaces[i])), LCDMode.MainCompact, LinesFor(textSurfaces[i]));
    // drawn screens (map, job, log): one per run, in turn, every other tick: each about once a second with 3;
    // the tiles of an LCD wall (map:<view>:<C>x<R>:<n>) together, so the picture doesn't tear between them
    List<String> turns = new List<String>();
    String[] keys = new String[lcdPanels.Count];
    for (int i = 0; i < lcdPanels.Count; i++)
    {
        LCDMode m; String pr;
        ParseLCDMode(lcdPanels[i], out m, out pr);
        if (!SpriteMode(m)) continue;
        keys[i] = m == LCDMode.Map && pr.Split(':').Length > 2 ? pr.Substring(0, pr.LastIndexOf(':')) : "" + lcdPanels[i].EntityId;
        if (!turns.Contains(keys[i])) turns.Add(keys[i]);
    }
    bool draw = isTick && ++screenTick % 2 == 0;
    if (draw) spriteTurn = turns.Count > 0 ? (spriteTurn + 1) % turns.Count : 0;
    for (int i = 0; i < lcdPanels.Count; i++)
    {
        LCDMode mode = LCDMode.Unknown;
        String param = "";
        IMyTextPanel p = lcdPanels[i];
        ParseLCDMode(p, out mode, out param);
        lcdFontSet = ApplyLcdFont(p);
        if (SpriteMode(mode)) { if (draw && turns.IndexOf(keys[i]) == spriteTurn && !Heavy()) DrawSpriteScreen(p, mode, param); continue; }
        int lines = LinesFor(p);
        DrawTextScreen(p, BuildControllerScreenForMode(mode, param, i == 0, lines), mode, lines); // coloured lines
    }
    Echo(echoText + ExecLine() + GetDebugDrawLegend());
    for (int i = 0; i < debugPanels.Count; i++)
    {
        IMyTextPanel panel = debugPanels[i];
        String data = panel.CustomData.ToUpper();
        if (data == "DEBUG") panel.WriteText("" + "\n" + "");
        if (data == "INSTRUCTIONS") panel.WriteText(GetInstructionInfo());
    }
}

String BuildControllerScreen(bool applyAction)
{
    int dummy = 0;
    return BuildControllerScreen(applyAction, 0, ref dummy, false, 1);
}

// Controller menu. pageOffset/scrollState/compact/pageSize are used by the paged LCD modes.
String BuildControllerScreen(bool applyAction, int pageOffset, ref int scrollState, bool compact, int pageSize)
{
    String text = "";
    String separator = "——————————————————\n";
    String thinSeparator = "--------------------------------------------\n";

    if (selectedShip == null || currentMenu == MenuPage.Main || currentMenu == MenuPage.SendCommandAll)
        text += "[PAM]-Controller | " + connectedShips.Count + " Connected ships | " + Math.Round(execAvg, 2) + " ms" + "\n" + GetDockAreaLine() + GetJobLines() + GetTrafficControlLine();
    else text += GetShipStatusLine(selectedShip) + "\n";
    text += separator;
    int headerLines = text.Split('\n').Length - 1; // title, docking area, jobs, traffic, separator: the list gets the rest

    int idx = 0;
    int sel = menuIndex;
    menuItemCount = 0;

    if (statusMessage != "")
    {
        AddMenuItem(ref text, sel, idx++, applyAction, statusMessage);
    }
    else if (currentMenu == MenuPage.Main)
    {
        if (selectedShip == null && connectedShips.Count >= 1) selectedShip = connectedShips[0];
        String list = "";
        if (AddMenuItem(ref list, sel, idx++, applyAction, " Send command to all")) NavigateToMenu(MenuPage.SendCommandAll);
        int selectedLine = 0;
        int visibleLines = Math.Max(3, pageSize - headerLines) + Math.Max(0, pageOffset - 1) * pageSize;
        bool gap = compact && 1 + connectedShips.Count * 3 <= visibleLines; // blank line between ships only when they all fit
        for (int s = 0; s < connectedShips.Count; s++)
        {
            if (gap) list += "\n";
            ShipInfo ship = connectedShips[s];
            if (sel == idx || sel == idx + 1) selectedShip = ship;
            if (idx == sel) selectedLine = list.Split('\n').Length - 1;
            if (AddMenuItem(ref list, sel, idx++, applyAction, " " + GetShipStatusLine(ship)))
            {
                selectedShip = ship;
                NavigateToMenu(MenuPage.ShipList);
                selectedShip.ProcessCommand("", false, true, false);
            }
            if (idx == sel) selectedLine = list.Split('\n').Length - 1;
            if (AddMenuItem(ref list, sel, idx++, applyAction, " " + (compact ? "" : "  ") + GetShipCargoLine(ship)))
            {
                selectedShip = ship;
                NavigateToMenu(MenuPage.ShipInventory);
            }
        }
        text += ScrollText(visibleLines, list, selectedLine, ref scrollState);
    }
    else if (currentMenu == MenuPage.SendCommand || currentMenu == MenuPage.SendCommandAll)
    {
        MenuPage backPage = currentMenu == MenuPage.SendCommand ? MenuPage.ShipList : MenuPage.Main;
        ShipInfo target = null; // null = all ships
        if (currentMenu == MenuPage.SendCommand) target = selectedShip;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { NavigateToMenu(backPage); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Stop] " + GetCommandStatusText(target, "STOP"))) { SendCommandToShip(target, "STOP"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Continue] " + GetCommandStatusText(target, "CONT"))) { SendCommandToShip(target, "CONT"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Move home] " + GetCommandStatusText(target, "HOMEPOS"))) { SendCommandToShip(target, "HOMEPOS"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Move to job] " + GetCommandStatusText(target, "JOBPOS"))) { SendCommandToShip(target, "JOBPOS"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Full simulation] " + GetCommandStatusText(target, "FULL"))) { SendCommandToShip(target, "FULL"); }
        if (target != null && target.shipMode == ShipMode.Shuttle)
            if (AddMenuItem(ref text, sel, idx++, applyAction, " [Undock] " + GetCommandStatusText(target, "UNDOCK"))) { SendCommandToShip(target, "UNDOCK"); }
        text += GetCmdResultLines(target);
    }
    else if (currentMenu == MenuPage.ShipList)
    {
        // Ship detail: mirrors the remote ship's own menu, navigated via UP/DOWN/APPLY forwarding.
        String backLine = "";
        if (AddMenuItem(ref backLine, sel, idx++, applyAction, " Back")) { NavigateToMenu(MenuPage.Main); }
        text += backLine.Substring(0, backLine.Length - 1).PadRight(36, ' ');
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Send cmd")) { NavigateToMenu(MenuPage.SendCommand); }
        if (!selectedShip.IsTimedOut() && !selectedShip.IsCommandTimedOut()) menuItemCount++;
        if (selectedShip.IsCommandTimedOut()) text += "No answer received...";
        else text += SignalOrText(selectedShip, selectedShip.menuText);
    }
    else if (currentMenu == MenuPage.ShipInventory)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { NavigateToMenu(MenuPage.Main); }
        text += thinSeparator;
        text += SignalOrText(selectedShip, GetShipInventoryText(selectedShip));
    }

    if (!menuJustNavigated) menuIndex = Math.Min(menuItemCount - 1, menuIndex);
    return text;
}

String GetShipStatusLine(ShipInfo ship)
{
    if (ship.version != VERSION) return ship.name + ": Different version!";
    return ship.name + ": " + SignalOrText(ship, ship.stateText) + " " + ship.distance + "m";
}

String GetShipCargoLine(ShipInfo ship)
{
    String s = ProgressBar("", SafeDiv(ship.currentVolume, ship.maxVolume) * 100f, 100f, 8, 0, 0) + "% ";
    for (int i = 0; i < ship.items.Count; i++)
    {
        if (i >= 5) break;
        ItemInfo item = ship.items[i];
        s += Truncate(Capitalize(GetItemShortName(item)), 3) + " " + FormatAmount(item.amount) + " ";
    }
    return s;
}

String GetItemShortName(ItemInfo item)
{
    if (item.type.ToUpper() == "ORE" || item.type.ToUpper() == "INGOT")
    {
        String code = GetElementCode(item.name.ToUpper());
        if (code != "") return code;
    }
    return item.name;
}

// Replaces the text with "No signal..." once the ship hasn't reported for a while.
String SignalOrText(ShipInfo ship, String text)
{
    if (ship.IsTimedOut()) return "No signal...(" + FormatTime((int)(TimeNow() - ship.lastMessageTime).TotalSeconds) + ")";
    return text;
}

String GetLCDModeHelp(bool withDefault)
{
    String text = "";
    if (withDefault) text += "mode=main:1\n\n";
    text += "//Available modes:\n" + "//main:<Page>\n" + "//mainX:<Page>  (no empty lines)\n" + "//menu\n" +
        "//inventory\n" + "//menu:<shipname>\n" + "//inventory:<shipname>";
    // (the check for outdated Custom Data looks for the text above: new modes only in the template)
    if (withDefault) text += "\n//fleet\n//ship:<shipname>\n//log\n//job:<number>\n//map  (side view, wide LCD)\n//map:top  (top view)\n//map:dock  (docking area)\n//map:all  (all maps)\n//map:all:3x3:1  (tile 1 of a 3x3 wall of LCDs, sharp)\n//docks" +
        "\n\n//Font and size of this screen (remove the // to use):\n//font=Debug   (or Monospace, White, ...)\n//size=1.0";
    return text;
}

int ctrlScanStep = 0, spriteTurn = 0;

// per LCD (its Custom Data): font=<name> and size=<number>, set on the LCD; all screens draw with them
bool lcdFontSet = false;
bool ApplyLcdFont(IMyTextPanel p)
{
    String[] lines = p.CustomData.Split('\n');
    bool ok = true;
    String f = ParseCustomDataValue(FindCustomDataLine(lines, "font=", ref ok)), s = ParseCustomDataValue(FindCustomDataLine(lines, "size=", ref ok));
    float v;
    if (float.TryParse(s, out v) && v > 0.05f && Math.Abs(v - p.FontSize) > 0.001f) p.FontSize = v;
    if (f != "" && f != p.Font) p.Font = f;
    return f != "";
}

LCDMode ModeOf(IMyTerminalBlock panel) { LCDMode m; String s; ParseLCDMode(panel, out m, out s); return m; }

void ParseLCDMode(IMyTerminalBlock panel, out LCDMode mode, out String param)
{
    bool ok = true;
    String modeStr = ParseCustomDataValue(FindCustomDataLine(panel.CustomData.Split('\n'), "mode", ref ok)).ToUpper();
    String[] parts = modeStr.Split(':');
    String modeKey = parts.Length > 0 ? parts[0].Trim() : "";
    param = parts.Length > 1 ? modeStr.Substring(modeStr.IndexOf(':') + 1).Trim() : ""; // the rest (map:all:3x3:1)
    mode = LCDMode.Unknown;
    if (modeKey == "MAIN") mode = LCDMode.MainPaged;
    else if (modeKey == "MAINX") mode = LCDMode.MainPagedCompact;
    else if (modeKey == "MENU") mode = LCDMode.Menu;
    else if (modeKey == "INVENTORY") mode = LCDMode.Inventory;
    else if (modeKey == "DEBUG") mode = LCDMode.Debug;
    else if (modeKey == "FLEET") mode = LCDMode.Fleet;
    else if (modeKey == "SHIP") mode = LCDMode.Ship;
    else if (modeKey == "LOG") mode = LCDMode.Log;
    else if (modeKey == "JOB") mode = LCDMode.Job;
    else if (modeKey == "MAP") mode = LCDMode.Map;
    else if (modeKey == "DOCKS") mode = LCDMode.Docks;
    if (mode == LCDMode.Unknown) param = modeStr;
}

// Renders one LCD. "main:<page>" pages share one scrolling list; firstPanel resets the page bookkeeping.
// text lines that fit on a screen: its height (minus padding) over the line height at its font size
// (setting lcdLineHeight: pixels per line at size 1.0 on a 512 px LCD, ~30)
int LinesFor(IMyTextSurface s) { return Math.Max(5, (int)(s.SurfaceSize.Y * (1 - s.TextPadding / 50f) / (Math.Max(5, lcdLineHeight) * Math.Max(0.1f, s.FontSize)))); }

String BuildControllerScreenForMode(LCDMode mode, String param, bool firstPanel, int linesPerPage)
{
    if (firstPanel)
    {
        controllerPageIdx1 = controllerPageCount1;
        controllerPageCount1 = 0;
        controllerPageIdx2 = controllerPageCount2;
        controllerPageCount2 = 0;
    }
    String text = "";
    if (mode == LCDMode.Fleet) return BuildFleetScreen();
    if (mode == LCDMode.Ship) return BuildShipScreen(param);
    if (mode == LCDMode.Docks) return BuildDocksScreen();
    if (mode == LCDMode.MainPaged)
    {
        int page = 0;
        text = BuildControllerScreen(false, controllerPageIdx1, ref controllerScrollState1, true, linesPerPage);
        if (!int.TryParse(param, out page)) return text;
        controllerPageCount1 = Math.Max(page, controllerPageCount1);
        page--;
        return PageText(page, linesPerPage, text);
    }
    else if (mode == LCDMode.MainPagedCompact)
    {
        int page = 0;
        text = BuildControllerScreen(false, controllerPageIdx2, ref controllerScrollState2, false, linesPerPage);
        if (!int.TryParse(param, out page)) return text;
        controllerPageCount2 = Math.Max(page, controllerPageCount2);
        page--;
        return PageText(page, linesPerPage, text);
    }
    if (mode == LCDMode.MainCompact)
    {
        return BuildControllerScreen(false, 0, ref controllerScrollState3, true, linesPerPage);
    }
    else if (mode == LCDMode.Inventory || mode == LCDMode.Menu || mode == LCDMode.Debug)
    {
        ShipInfo ship = selectedShip;
        if (param != "")
        {
            ship = FindShip("", param);
            if (ship == null) return "Unknown ship: " + param;
        }
        else
        {
            if (ship == null) return "No ship on main screen selected.";
            text = "Selected: ";
        }
        String separator = "—————————————————————\n";
        String title = "";
        String body = "";
        if (mode == LCDMode.Menu) { title = "Menu"; body = SignalOrText(ship, ship.menuText); }
        else if (mode == LCDMode.Inventory) { title = "Inventory"; body = SignalOrText(ship, GetShipInventoryText(ship)); }
        else if (mode == LCDMode.Debug) { return SignalOrText(ship, GetShipDebugText(ship)); }
        text += ship.name + " - " + ship.distance + "m | " + Capitalize("" + title) + "\n" + separator;
        text += body;
        return text;
    }
    return "Unknown command: " + param;
}

string GetShipDebugText(ShipInfo ship)
{
    String s = "";
    s += ship.version + "\n";
    s += ship.name + "\n";
    s += ship.position.X + "\n";
    s += ship.position.Y + "\n";
    s += ship.position.Z + "\n";
    s += ship.stateText + "\n";
    s += ship.statusMessage + "\n";
    s += ship.menuIndex + "\n";
    s += ship.menuText.Length + "\n";
    s += ship.currentVolume + "\n";
    s += ship.maxVolume + "\n";
    s += ship.items.Count() + "\n";
    return s;
}

string GetShipInventoryText(ShipInfo ship)
{
    String s = "";
    s += ProgressBar("All", SafeDiv(ship.currentVolume, ship.maxVolume) * 100f, 100, 30, 8, 12) + "%\n";
    s += "\n";
    for (int i = 0; i < ship.items.Count(); i++)
    {
        ItemInfo item = ship.items[i];
        s += ProgressBar(item.name, item.amount, ship.totalItems, 30, 8, 10) + "\n";
    }
    return s;
}

// Combined command state for one ship, or for all ships when ship == null.
CommandState GetCommandState(ShipInfo ship, bool clear, string cmd)
{
    CommandState result = CommandState.None;
    if (ship == null)
    {
        for (int i = 0; i < connectedShips.Count; i++)
        {
            CommandState state = connectedShips[i].ProcessCommand(cmd, false, clear, false);
            if (state == CommandState.Received) result = state;
            if (state == CommandState.NoAnswer) return state;
            if (state == CommandState.Pending) return state;
        }
        return result;
    }
    else return ship.ProcessCommand(cmd, false, clear, false);
}

String GetCommandStatusText(ShipInfo ship, String cmd)
{
    if (!ShowCmdResult(ship, cmd)) return ""; // sent from another page, to another ship, or long ago
    CommandState state = GetCommandState(ship, false, cmd);
    if (state == CommandState.Received) return "received!";
    if (state == CommandState.Pending) return "pending...";
    if (state == CommandState.NoAnswer) return "no answer!";
    return "";
}

// ========================== 28_UIHelpers.cs ==========================
// ========================== UI STRING HELPERS ==========================

String ScrollText(int visibleLines, String text, int selectedLine, ref int scrollOffset)
{
    String[] lines = text.Split('\n');
    if (selectedLine >= scrollOffset + visibleLines - 1) scrollOffset++;
    scrollOffset = Math.Min(lines.Count() - 1 - visibleLines, scrollOffset);
    if (selectedLine < scrollOffset + 1) scrollOffset--;
    scrollOffset = Math.Max(0, scrollOffset);
    String result = "";
    for (int i = 0; i < visibleLines; i++)
    {
        int lineIdx = i + scrollOffset;
        if (lineIdx >= lines.Count()) break;
        result += lines[lineIdx] + "\n";
    }
    return result;
}

String PageText(int page, int pageSize, String text)
{
    String[] lines = text.Split('\n');
    int start = page * pageSize;
    int end = (page + 1) * pageSize;
    String result = "";
    for (int i = start; i < end; i++)
    {
        if (i >= lines.Count()) break;
        result += lines[i] + "\n";
    }
    return result;
}

String Capitalize(String s)
{
    if (s == "") return s;
    return s.First().ToString().ToUpper() + s.Substring(1).ToLower();
}

String Truncate(String s, int maxLen)
{
    if (s == "") return s;
    if (s.Length > maxLen) s = s.Substring(0, maxLen - 1) + ".";
    return s;
}

string ProgressBar(String label, float value, float max, int barWidth, int labelWidth, int padding)
{
    float ratio = SafeDiv(value, max) * barWidth;
    String bar = "[";
    for (int i = 0; i < barWidth; i++) { if (i <= ratio) bar += "|"; else bar += "'"; }
    bar += "]";
    return bar + " " + Truncate(Capitalize(label), labelWidth).PadRight(labelWidth) + "".PadRight(padding) + FormatAmount(value);
}

String FormatAmount(float amount)
{
    if (amount >= 1000000) return Math.Round(amount / 1000000f, amount / 1000000f < 100 ? 1 : 0) + "M";
    if (amount >= 1000) return Math.Round(amount / 1000f, amount / 1000f < 100 ? 1 : 0) + "K";
    return "" + Math.Round(amount);
}

String FormatTime(int seconds)
{
    if (seconds >= 60 * 60) return Math.Round(seconds / (60f * 60f), 1) + " h";
    if (seconds >= 60) return Math.Round(seconds / 60f, 1) + " min";
    return "" + seconds + " s";
}

string FormatFloat(float v) { return Math.Round(v, 2) + " "; }
string FormatVector(Vector3 v) { return "X" + FormatFloat(v.X) + "Y" + FormatFloat(v.Y) + "Z" + FormatFloat(v.Z); }

// ========================== 29_ObstacleBrake.cs ==========================
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

// ========================== 30_Traffic.cs ==========================
// ========================== TRAFFIC & CO-OP MINING ==========================
// PAM ships share a small, frequent traffic message (own IGC tag, independent of the controller
// protocol and of Broadcast_Channel): position, velocity, size, job and the hole they
// work on. It is used for:
//  - lanes: on the recorded path, ships keep right of it by laneOffset (tapering to the path
//    near both ends), so ships going opposite ways pass side by side
//  - spacing: slow down / hold behind a PAM ship ahead; keep right when one comes head-on
//  - co-op mining: ships on the same job (IMPORT JOB) claim single holes, skip holes other
//    ships have or are too close to, and share which holes are finished

const String TRAFFIC_TAG = "[PAMTFC]";
// trafficTimeout [s]: forget ships not heard from for this long (stopped ships: at least 12 s)
// followTime [s]: time to close the gap to a ship ahead

class TrafficShip
{
    public long id;
    public Vector3 pos, vel;
    public Vector3 heading;  // direction to its flight target (zero when it isn't avoiding traffic)
    public float diameter;
    public String job = "";
    public int hole = -1;  // co-op: grid cell it claims
    public bool queued;    // holding outside the docking area for station clearance
    public bool parked;    // out of service (can't dock), parked beside the path: pass it
    public int jobFlag;    // 1: working in the job area, aligned to it; 2: deep in its hole (inside the rock)
    public int prio;       // right of way at the job site (TrafficPrio)
    public float metric = -1; // distance it still has to go, whole metres (-1 unknown)
    public float fw, fh;   // its size across the drill face
    public long dock = 0;  // dock pool: connector this ship uses or heads for
    public int dockPhase = 0;
    public DateTime time;
}

IMyBroadcastListener trafficListener = null;
long myGridId = 0;
List<TrafficShip> traffic = new List<TrafficShip>();
Vector3 trafficShift = new Vector3();
float trafficSpeedLimit = -1; // -1 = no limit
Vector3 trafficHeading = new Vector3();
bool trafficHolding = false;
DateTime trafficHoldSince = TimeNow();
// trafficStuckTime / trafficEscapeTime (settings): held by traffic this long -> stop waiting and pass on the right, for this long
int trafficTick = 0;
String doneHoles = "";        // one char per hole (grid cell): '1' = finished (by any co-op ship)
int myHole = -1;              // grid cell of the hole this ship works on
bool coopWaiting = false, coopWaitAside = false, exitRise = false;
Vector3 coopWaitPos = new Vector3();

// hole spacing of the job: the drill area it was set up with (or imported with), so a ship that
// loses or gains a drill keeps the same grid as the other ships
float JTW() { return jobToolW > 0 ? jobToolW : toolWidth; }
float JTH() { return jobToolH > 0 ? jobToolH : toolHeight; }

String JobKey()
{
    if (jobState == JobState.NoJob || shipMode == ShipMode.Shuttle) return "";
    return Math.Round(jobPosition.position.X) + "|" + Math.Round(jobPosition.position.Y) + "|" + Math.Round(jobPosition.position.Z) +
        "|" + jobWidth + "|" + jobHeight + "|" + (int)jobStartPos + "|" + Math.Round(JTW(), 1) + "|" + Math.Round(JTH(), 1) + "|" + jobSerial;
}

void UpdateTraffic()
{
    if (!trafficEnabled) return;
    if (trafficListener == null) trafficListener = IGC.RegisterBroadcastListener(TRAFFIC_TAG);
    myGridId = Me.CubeGrid.EntityId;
    int budget = 30;
    while (trafficListener.HasPendingMessage && budget-- > 0)
    {
        String data = trafficListener.AcceptMessage().Data as String;
        if (data != null) ParseTraffic(data);
    }
    for (int i = traffic.Count - 1; i >= 0; i--)
        // stopped ships (docked, idle: they run and send less often) are kept longer
        if ((TimeNow() - traffic[i].time).TotalSeconds > (traffic[i].vel.LengthSquared() < 0.25f ? Math.Max(12, trafficTimeout) : trafficTimeout)) traffic.RemoveAt(i);
    if (tccListener == null) tccListener = IGC.RegisterBroadcastListener(TCC_TAG);
    budget = 5;
    while (tccListener.HasPendingMessage && budget-- > 0)
    {
        String data = tccListener.AcceptMessage().Data as String;
        if (data != null) try { ParseTrafficControl(data); } catch { }
    }
    if (coordListener == null) coordListener = IGC.RegisterBroadcastListener(JOB_TAG);
    budget = 10;
    while (coordListener.HasPendingMessage && budget-- > 0)
    {
        String data = coordListener.AcceptMessage().Data as String;
        if (data != null) try { ParseJobState(data); } catch { }
    }

    // idle ships send too: docked (their connector counts as taken) or hovering (an obstacle)
    if (++trafficTick < (currentNavState == NavState.Idle ? 1 : 3)) return; // ~2 messages per second (idle: runs every ~1.7 s, sends each time)
    trafficTick = 0;
    Vector3 g = remoteControl.GetNaturalGravity();
    if (g.LengthSquared() > 0.01f) g.Normalize();
    // recorded path (+ planet centre: real heights) for the controller's map / debug drawing, every ~15 s
    Vector3D pc;
    if (++dbgPathTick % 30 == 1) IGC.SendBroadcastMessage("[PAMPTH]", myGridId + ";" + DbgPath() + ";" +
        (remoteControl.TryGetPlanetPosition(out pc) ? (long)pc.X + "|" + (long)pc.Y + "|" + (long)pc.Z : ""));
    if (debugDraw) // for the controller's debug drawing: id;jobPos;left;down;target;rays;forward
    {
        IGC.SendBroadcastMessage("[PAMDBG]", myGridId + ";" + VectorToString(jobPosition.position) + ";" +
            VectorToString(jobState == JobState.NoJob ? new Vector3() : jobPosition.leftDir) + ";" + VectorToString(miningDown) + ";" +
            VectorToString(navTargetPos + trafficShift) + ";" + String.Join(":", debugRays) + ";" + VectorToString(miningForward));
        debugRays.Clear();
    }
    IGC.SendBroadcastMessage(TRAFFIC_TAG, myGridId + ";" + VectorToString(shipPosition) + ";" +
        VectorToString(remoteControl.GetShipVelocities().LinearVelocity) + ";" + TrafficDiameter() + ";" + JobKey() + ";" +
        (jobState == JobState.Active ? myHole : -1) + ";" + doneHoles + ";" + ClaimedDockId() + "," + DockPhase() + ";" + VectorToString(trafficHeading) +
        ";" + (outOfService || emgStage == 1 || emgStage == 2 ? 2 : dockQueued ? 1 : 0) + ";" + (holeWanted && jobState == JobState.Active ? 1 : 0) + "," + shipFootWidth + "," + shipFootHeight + "," + JobFlag() + "," + (sentPrio = TrafficPrio()) + "," + (sentDest = (float)Math.Round(DestDist())) + ";" + Me.GetId() +
        // for the controller's screens: battery %, hydrogen %, uranium kg, emergency, flight state, run time ms, up (x10)
        ";" + (batteryState == BatteryState.None ? -1 : (int)batteryPercent) + "," + (int)hydrogenPercent + "," + (int)uraniumAmount + "," + (emergency ? 1 : 0) + "," + (int)currentNavState + "," + Math.Round(execAvg, 3) + "," +
        (int)(-g.X * 10) + "|" + (int)(-g.Y * 10) + "|" + (int)(-g.Z * 10) +
        ";" + (TimeNow() < reopenSend ? reopenCell : -1)); // hole reopened with PREV (co-op)
}

// PREV in co-op: the hole moved back to counts as not finished for everyone (sent for 15 s); during
// that time a "finished" mark for it from the others doesn't count
int reopenCell = -1;
DateTime reopenUntil = DateTime.MinValue, reopenSend = DateTime.MinValue;

void Reopen(int cell, bool mine)
{
    reopenCell = cell; reopenUntil = TimeNow().AddSeconds(15);
    if (mine) reopenSend = reopenUntil;
    SetHoleDone(cell, false);
}

void ParseTraffic(String data)
{
    try
    {
        String[] f = data.Split(';');
        if (f.Length < 7) return;
        long id = long.Parse(f[0]);
        if (id == myGridId) return;
        TrafficShip ship = null;
        for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) ship = traffic[i];
        if (ship == null) { ship = new TrafficShip(); ship.id = id; traffic.Add(ship); }
        ship.pos = StringToVector(f[1]);
        ship.vel = StringToVector(f[2]);
        ship.diameter = float.Parse(f[3]);
        ship.job = f[4];
        ship.hole = int.Parse(f[5]);
        ship.time = TimeNow();
        if (ship.job != "" && ship.job == JobKey())
        {
            if (f.Length >= 14 && f[13] != "-1") Reopen(int.Parse(f[13]), false);
            MergeDoneHoles(f[6]);
        }
        ship.dock = 0; ship.dockPhase = 0;
        if (f.Length >= 8)
        {
            String[] d = f[7].Split(',');
            ship.dock = long.Parse(d[0]);
            ship.dockPhase = int.Parse(d[1]);
        }
        ship.heading = f.Length >= 9 ? StringToVector(f[8]) : new Vector3();
        ship.queued = f.Length >= 10 && f[9] == "1";
        ship.parked = f.Length >= 10 && f[9] == "2";
        ship.jobFlag = 0; ship.prio = 0;
        if (f.Length >= 11)
        {
            String[] w = f[10].Split(',');
            if (w.Length >= 4) { ship.fw = float.Parse(w[1]); ship.fh = float.Parse(w[2]); ship.jobFlag = int.Parse(w[3]); if (w.Length >= 5) ship.prio = int.Parse(w[4]); ship.metric = w.Length >= 6 ? float.Parse(w[5]) : -1; }
        }
    }
    catch { } // malformed message from another script: ignore
}

bool DockedPam(long id)
{
    for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) return traffic[i].dockPhase == 3 && traffic[i].vel.LengthSquared() < 0.25f;
    return false;
}

bool IsTrafficShip(long id)
{
    for (int i = 0; i < traffic.Count; i++) if (traffic[i].id == id) return true;
    return false;
}

// ---- lanes ----

// Keep-right offset for path waypoint idx when travelling in pathDir (+1/-1): perpendicular to
// the path and to "up", so opposite directions end up on opposite sides whatever way the path
// was recorded. Tapers to 0 near both path ends (dock and job).
Vector3 GetLaneOffset(int idx, int pathDir)
{
    if (laneOffset <= 0 || waypoints.Count < 2 || idx < 0 || idx >= waypoints.Count) return new Vector3();
    WaypointInfo wp = waypoints[idx];
    Vector3 travel = (waypoints[Math.Min(waypoints.Count - 1, idx + 1)].position - waypoints[Math.Max(0, idx - 1)].position) * pathDir;
    Vector3 up = wp.gravity.LengthSquared() > 0.01f ? -Vector3.Normalize(wp.gravity) : -wp.downDir;
    Vector3 right = Vector3.Cross(travel, up);
    if (right.LengthSquared() < 0.0001f) return new Vector3();
    float dist = laneOffset * shipDiameter;
    float fromEnds = Math.Min(Vector3.Distance(wp.position, waypoints[0].position),
        Vector3.Distance(wp.position, waypoints[waypoints.Count - 1].position));
    return Vector3.Normalize(right) * dist * Math.Min(1, fromEnds / (dist * 4));
}

// ---- spacing ----

Vector3 trafficMyVel = new Vector3();
bool sidestep = false; // moving out of the way of a passing ship
List<String> debugRays = new List<String>();
int dbgPathTick = 0;

// recorded path for the controller's debug drawing, whole metres
String DbgPath()
{
    String s = "";
    for (int i = 0; i < waypoints.Count; i++) { Vector3 w = waypoints[i].position; s += (i > 0 ? ":" : "") + (int)w.X + "|" + (int)w.Y + "|" + (int)w.Z; }
    return s;
}

// Docking, undocking and backing out of a hole: traffic only slows/holds the ship, no side step
// (the ship has to stay on its line to the connector or hole).
bool InJobArea()
{
    NavState s = currentNavState;
    return JobKey() != "" && (s == NavState.FlyToXY || s == NavState.Mining || s == NavState.Returning ||
        s == NavState.ReturnToDock || s == NavState.FlyToDropPos);
}

float JobRadius() { return 0.5f * (float)Math.Sqrt(Math.Pow(jobWidth * JTW(), 2) + Math.Pow(jobHeight * JTH(), 2)); }

bool NearJobArea() { return JobKey() != "" && Vector3.Distance(shipPosition, jobPosition.position) < JobRadius() + shipDiameter * 2; }

// right of way: 3 emergency (low on energy, on the way to dock) or docking / undocking, 2 heading home (anywhere: backing out
// to go home, flying to the path, on the path home), 1 moving between holes or arriving at the job
// area, 0 other (e.g. on the way to the job), -1 waiting where it is (for a hole, clearance, a dock)
bool HeadingHome() { NavState s = currentNavState; return s == NavState.ReturnToDock || s == NavState.FlyToPath || s == NavState.FlyToDockArea; }

int TrafficPrio()
{
    NavState s = currentNavState;
    if (waitingHere) return -1; // waiting (for a hole, dock clearance, a free dock): everyone else goes first
    // low on energy on its way to dock, or cleared and docking / undocking in the docking area
    if (emergency || s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.Docking || s == NavState.RetryDocking || s == NavState.Undocking) return 3;
    if (HeadingHome()) return 2;
    return s == NavState.FlyToXY || s == NavState.Returning || s == NavState.FlyToDropPos || (s == NavState.FlyToJobArea && NearJobArea()) ? 1 : 0;
}

// How far this ship still has to go, along the road: to the path waypoint it is on (or joins:
// the nearest one, when it is beside the path or still at the job site), then along the path to
// its end, home or job. So a ship already on the road goes before one that still has to join it.
float DestDist()
{
    bool home = HeadingHome() && homeDock.isSet;
    bool onPath = currentNavState == NavState.FlyToDockArea || currentNavState == NavState.FlyToJobArea;
    if (waypoints.Count == 0 || (!home && !onPath)) return Vector3.Distance(shipPosition, navTargetPos);
    int k = pathWaypointIndex, dir = pathDirection;
    if (!onPath || dir == 0 || k < 0 || k >= waypoints.Count)
    {
        // joining the road: nearest waypoint, towards the home end of the path
        k = 0;
        for (int i = 1; i < waypoints.Count; i++)
            if (Vector3.Distance(waypoints[i].position, shipPosition) < Vector3.Distance(waypoints[k].position, shipPosition)) k = i;
        dir = Vector3.Distance(waypoints[0].position, homeDock.position) < Vector3.Distance(waypoints[waypoints.Count - 1].position, homeDock.position) ? -1 : 1;
    }
    float d = Vector3.Distance(shipPosition, waypoints[k].position);
    for (int i = k; i + dir >= 0 && i + dir < waypoints.Count; i += dir) { d += Vector3.Distance(waypoints[i].position, waypoints[i + dir].position); k = i + dir; }
    return home ? d + Vector3.Distance(waypoints[k].position, homeDock.position) : d;
}

// right of way as last sent: the other ships compare against these same numbers (a value fresher
// than the one they have would make both ships think they go first)
float sentDest = -1;
int sentPrio = 0;
bool waitingHere = false;

// between ships with the same right of way: the one closer to where it is going goes first (whole
// metres; equal: lower id). One number per ship, so the order is the same for every ship: no circle
// of ships all giving way to each other.
bool GoesFirst(float myDist, TrafficShip o)
{
    float mine = (float)Math.Round(myDist);
    if (o.metric < 0 || mine == o.metric) return myGridId < o.id;
    return mine < o.metric;
}

// 0: not working the job; 1: in the job area, aligned to it; 2: deeper in its hole than half its size
int JobFlag()
{
    if (!InJobArea()) return 0;
    return currentNavState != NavState.FlyToXY && Vector3.Dot(shipPosition - miningTargetPos, miningForward) > shipDiameter * 0.5f ? 2 : 1;
}

bool IsPreciseFlightState(NavState s)
{
    return s == NavState.ApproachDock || s == NavState.AlignDock || s == NavState.Docking || s == NavState.RetryDocking ||
        s == NavState.Undocking || s == NavState.Returning || s == NavState.ReturnToDock || s == NavState.FlyToDropPos || s == NavState.AlignJob;
}

void UpdateTrafficAvoidance()
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    trafficHeading = new Vector3();
    // in a connector's lane (pool connector too): precise, holds only (no side steps, no moving aside).
    // Out of its hole, rising above the job to leave for home: free flight again, so a ship waiting or
    // stuck in the way is passed, and it doesn't hold there for ever (an emergency stood still)
    bool free = (IsFreeFlightState(currentNavState) || currentNavState == NavState.ReturnToDock && exitRise) && !(currentNavState == NavState.ApproachDock && !toLane);
    if (!trafficEnabled || (!free && !IsPreciseFlightState(currentNavState))) { trafficHolding = false; return; }
    trafficMyVel = remoteControl.GetShipVelocities().LinearVelocity;
    ComputeTrafficAvoidance(remoteControl.GetNaturalGravity(), !free);
    ApplyTrafficControl();
}

// [PAM-Controller] traffic control: "C;ship:limit,..." lists the ships that have to give way
IMyBroadcastListener tccListener = null;
float tccLimit = -1;
DateTime tccTime = DateTime.MinValue;

void ParseTrafficControl(String data)
{
    if (!data.StartsWith("C;")) return;
    tccTime = TimeNow();
    tccLimit = -1;
    String me = myGridId + ":";
    String[] e = data.Substring(2).Split(',');
    for (int i = 0; i < e.Length; i++) if (e[i].StartsWith(me)) tccLimit = float.Parse(e[i].Substring(me.Length));
}

// on top of the ship's own avoidance, after its 15 s escape: the controller sees all ships
void ApplyTrafficControl()
{
    if (tccLimit < 0 || (TimeNow() - tccTime).TotalSeconds > 1.5 || sidestep) return; // moving aside resolves it
    trafficSpeedLimit = trafficSpeedLimit < 0 ? tccLimit : Math.Min(trafficSpeedLimit, tccLimit);
    if (tccLimit < 0.5f) statusMessage = "Traffic: giving way (controller)";
}

void ComputeTrafficAvoidance(Vector3 gravity, bool precise = false)
{
    trafficShift = new Vector3();
    trafficSpeedLimit = -1;
    Vector3 toTarget = navTargetPos - shipPosition;
    float targetDist = toTarget.Length();
    bool haveDir = targetDist >= 1f;
    Vector3 dir = haveDir ? toTarget / targetDist : new Vector3();
    trafficHeading = dir; // sent to the other ships: lets them tell head-on from following, even when stopped
    Vector3 up = gravity.LengthSquared() > 0.01f ? -Vector3.Normalize(gravity) : -downDirection;
    Vector3 right = Vector3.Cross(dir, up);
    right = right.LengthSquared() < 0.0001f ? -leftDirection : Vector3.Normalize(right);
    TrafficShip limiter = null;
    bool cpaHold = false; // held for a collision course: stop, don't move aside
    bool goingHome = currentNavState == NavState.FlyToDockArea && !StationCleared(1); // lines up in the dock queue
    int myPrio = sentDest < 0 ? TrafficPrio() : sentPrio;
    float myDest = sentDest < 0 ? DestDist() : sentDest;
    bool cpaClose = false; // collision course less than 2 s away

    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip other = traffic[i];
        if (other.dockPhase == 3 && other.vel.LengthSquared() < 0.25f) continue; // parked at a connector
        Vector3 pos = other.pos + other.vel * (float)(TimeNow() - other.time).TotalSeconds;
        Vector3 rel = pos - shipPosition;
        float clear = (TrafficDiameter() + other.diameter) * 0.5f;
        // two ships working the same job are aligned to it and drill different holes, so a ship at
        // another hole, even the next one, is never in the way: only closer than one hole spacing counts
        bool sameJob = other.job != "" && other.job == JobKey();
        if (sameJob && other.jobFlag == 2) continue; // inside the rock in its own hole: can't be in anyone's way
        bool sideBySide = sameJob && other.jobFlag == 1 && InJobArea();
        if (sideBySide) clear = Math.Min(JTW(), JTH()) * 0.9f;
        float corridor = clear * (sideBySide ? 1f : 1.2f); // side by side: a ship at the next hole is outside it
        float along = haveDir ? Vector3.Dot(rel, dir) : 0;
        if (along > 0 && (rel - dir * along).Length() <= corridor)
        {
            // in my corridor, ahead
            float otherAlong = Vector3.Dot(other.vel, dir);
            bool hasHeading = other.heading.LengthSquared() > 0.25f;
            bool headOn = otherAlong < -1f || (hasHeading && Vector3.Dot(other.heading, dir) < -0.5f);
            // out of service, or waiting (for a hole, a dock) and not where I'm going: pass it
            if (!precise && (other.parked || other.prio < 0 && !other.queued && other.vel.LengthSquared() < 1 && Vector3.Distance(pos, navTargetPos) > clear))
            { trafficShift = PassRight(dir, right, clear, along); continue; }
            // a ship with right of way: give way, don't pass it
            if (!precise && other.prio <= myPrio) // docking, undocking, in a hole: no side step, hold for every ship in the way
            {
                if (other.queued)
                {
                    // a ship waiting outside the docking area: leaving or cleared ships pass it on the right
                    if (!goingHome) { trafficShift = PassRight(dir, right, clear, along); continue; }
                    // both queueing: the one closer to the waiting point is ahead (tie: lower id)
                    float mine = Vector3.Distance(shipPosition, navTargetPos), its = Vector3.Distance(pos, navTargetPos);
                    if (mine < its || (mine == its && myGridId < other.id)) continue;
                }
                if (headOn)
                {
                    // head-on (also when both are slow or stopped): both keep right and pass
                    if (along < (shipSpeed - otherAlong) * 6f + clear * 4f)
                    {
                        trafficShift = PassRight(dir, right, clear, along);
                        // not beside it yet: slow down, so there's time to get out of its way
                        if ((rel - dir * along).Length() < clear) Limit(Math.Max(1, (along - clear) / 4), other, ref limiter);
                    }
                    continue;
                }
                if (hasHeading && GoesFirst(myDest, other))
                {
                    // does the other ship have me ahead in its corridor too (crossing paths)? then one
                    // of us has to go: the one with right of way goes, keeping right, the other one waits
                    Vector3 back = -rel;
                    float otherAhead = Vector3.Dot(back, other.heading);
                    if (otherAhead > 0 && (back - other.heading * otherAhead).Length() <= corridor)
                    { trafficShift = PassRight(dir, right, clear, along); continue; }
                }
            }
            // same direction, or a ship that isn't moving on its own: follow at a distance, hold if it stops
            float gap = along - clear - (sideBySide ? 1f : followGap * shipDiameter);
            float limit = Math.Max(0, otherAlong) + Math.Max(0, gap) / followTime;
            if (headOn) limit = Math.Max(0, gap + Math.Min(0, otherAlong) * followTime * 2) / followTime; // it comes towards me: stop early
            Limit(limit, other, ref limiter);
            continue;
        }
        // any other moving ship on a collision course (crossing, from the side or from behind):
        // the higher id stops; both stop when it's close
        if (other.vel.LengthSquared() < 1f) continue;
        if (haveDir && (rel - dir * along).Length() <= corridor) continue; // behind me in my lane: it has to slow down, not me
        Vector3 rv = other.vel - trafficMyVel;
        float rv2 = rv.LengthSquared();
        if (rv2 < 0.25f) continue;
        float t = -Vector3.Dot(rel, rv) / rv2;
        if (t <= 0 || t > 5f || (rel + rv * t).Length() >= (sideBySide ? clear : clear * 1.1f)) continue;
        if (other.prio > myPrio || (other.prio == myPrio && !GoesFirst(myDest, other)) || t < 2f) { trafficSpeedLimit = 0; limiter = other; cpaHold = true; cpaClose |= t < 2f; }
    }

    // giving way: a ship with right of way that is coming through where I am (or wants to: its
    // direction when stopped) -> move slowly out of its line, sideways (only in free flight; the
    // camera brake stops the ship if there's no room there)
    bool wasAside = sidestep; // once moving aside, keep going until out of its line (it starts only when stopped)
    sidestep = false;
    if (!precise && !cpaHold && (shipSpeed < 1 || wasAside))
        for (int i = 0; i < traffic.Count; i++)
        {
            TrafficShip o = traffic[i];
            if (o.parked || o.jobFlag == 2 || !(o.prio > myPrio || (o.prio == myPrio && !GoesFirst(myDest, o)))) continue;
            Vector3 oi = o.vel.LengthSquared() > 1 ? o.vel : o.heading;
            if (oi.LengthSquared() < 0.25f) continue;
            Vector3 od = Vector3.Normalize(oi);
            // a ship going my way (behind me, following) never makes me move aside: it slows down
            if (haveDir && Vector3.Dot(od, dir) > 0.3f) continue;
            Vector3 rel = shipPosition - o.pos;
            float along = Vector3.Dot(rel, od);
            Vector3 lat = rel - od * along;
            float clear = (TrafficDiameter() + o.diameter) * 0.6f;
            if (along <= 0 || along > clear * 4 || lat.Length() >= clear) continue;
            Vector3 away = lat.Length() > 0.5f ? Vector3.Normalize(lat) : Vector3.Normalize(Vector3.Cross(od, up));
            trafficShift = shipPosition + away * clear - navTargetPos; // fly to the point beside its line
            trafficSpeedLimit = moveAsideSpeed;
            sidestep = true;
            statusMessage = "Traffic: moving aside";
            return;
        }

    // safety net: held by traffic for too long -> stop waiting for a while and pass on the right
    // (not while docking or in a hole, and not in the queue outside the docking area)
    bool held = trafficSpeedLimit >= 0 && trafficSpeedLimit < 0.5f;
    if (!held || precise || (limiter != null && limiter.queued && goingHome)) { trafficHolding = false; return; }
    if (!trafficHolding) { trafficHolding = true; trafficHoldSince = TimeNow(); }
    double heldFor = (TimeNow() - trafficHoldSince).TotalSeconds;
    if (heldFor > trafficStuckTime + trafficEscapeTime) trafficHoldSince = TimeNow();
    else if (heldFor > trafficStuckTime) { float td = TrafficDiameter(); trafficSpeedLimit = cpaClose ? 0 : -1; trafficShift = PassRight(dir, right, td, td * 4); statusMessage = "Traffic: passing a stuck ship"; }
}

void Limit(float v, TrafficShip o, ref TrafficShip limiter) { if (trafficSpeedLimit < 0 || v < trafficSpeedLimit) { trafficSpeedLimit = v; limiter = o; } }

// passing on the right: steer to a point beside the other ship where we meet it, not to the far
// target with an offset (that would hardly move the ship sideways by the time they meet)
// Ship size for traffic (what other ships and the controller see, and the clearances it keeps).
// The broadcast position is the ship's reference point (centre of mass at setup), but shipDiameter is
// the grid's bounding sphere around the middle of its box: on a lopsided ship the hull sticks out of a
// sphere of that size around the reference point. trafficHullCover adds the offset between the two
// centres so the sphere covers the whole hull; trafficPadding adds extra metres on every side.
float TrafficDiameter()
{
    float d = shipDiameter;
    if (trafficHullCover && remoteControl != null)
    {
        // middle of the grid's box (the minifier renames ".Center", so it is built from the corners)
        IMyCubeGrid g = remoteControl.CubeGrid;
        d += 2f * Vector3.Distance(shipPosition, (Vector3)((g.GridIntegerToWorld(g.Min) + g.GridIntegerToWorld(g.Max)) * 0.5));
    }
    return d + 2f * Math.Max(0f, trafficPadding);
}

Vector3 PassRight(Vector3 dir, Vector3 right, float clear, float along)
{
    return dir * Math.Min((navTargetPos - shipPosition).Length(), Math.Max(along * 0.5f, clear)) + right * clear * 1.3f + shipPosition - navTargetPos;
}

// ---- co-op mining ----
// Ships on the same job claim single holes, in the normal mining order. A claim is kept while the
// job runs (also while the ship is away unloading), so a half-mined hole is never taken over.
// The next ship takes the next hole, right beside the other one. Finished holes are shared.

List<int> holeCells = new List<int>(); // mining order: grid cell (row * jobWidth + col) for each hole index
String holeOrderKey = "";

// Same order as GetNextHolePosition (top-left zigzag or centre spiral), all holes at once.
void EnsureHoleOrder()
{
    String key = jobWidth + "|" + jobHeight + "|" + (int)jobStartPos;
    if (key == holeOrderKey) return;
    holeOrderKey = key;
    BuildHoleOrder(jobWidth, jobHeight, jobStartPos, holeCells);
}

void BuildHoleOrder(int w, int h, StartPosition start, List<int> cells)
{
    cells.Clear();
    if (start == StartPosition.TopLeft)
    {
        for (int k = 0; k < w * h; k++)
        {
            int row = k / w;
            cells.Add(row * w + (row % 2 == 0 ? k - row * w : w - 1 - (k - row * w)));
        }
        return;
    }
    int halfW = (int)Math.Ceiling(w / 2f), halfH = (int)Math.Ceiling(h / 2f);
    int halfW2 = w / 2, halfH2 = h / 2;
    int x = 0, y = 0, dx = 0, dy = -1, max = Math.Max(w, h);
    for (int i = 0; i < max * max; i++)
    {
        if (-halfW < x && x <= halfW2 && -halfH < y && y <= halfH2) cells.Add((y - 1 + halfH) * w + (x - 1 + halfW));
        if (x == y || (x < 0 && x == -y) || (x > 0 && x == 1 - y)) { int t = dx; dx = -dy; dy = t; }
        x += dx; y += dy;
    }
}

bool EnsureDoneHoles()
{
    int n = Math.Max(0, jobWidth * jobHeight);
    if (doneHoles.Length == n) return false;
    doneHoles = new String('0', n);
    myHole = -1;
    return true;
}

void MergeDoneHoles(String other)
{
    EnsureDoneHoles();
    if (other.Length != doneHoles.Length) return;
    char[] h = doneHoles.ToCharArray();
    for (int i = 0; i < h.Length; i++) if (other[i] == '1' && (i != reopenCell || TimeNow() > reopenUntil)) h[i] = '1';
    doneHoles = new String(h);
}

bool IsHoleDone(int cell) { return cell >= 0 && cell < doneHoles.Length && doneHoles[cell] == '1'; }

void SetHoleDone(int cell, bool done)
{
    if (cell < 0 || cell >= doneHoles.Length) return;
    char[] h = doneHoles.ToCharArray();
    h[cell] = done ? '1' : '0';
    doneHoles = new String(h);
}

// Ships at holes a and b (cells, row * w + col) would touch: their footprints across the drill face
// (fw x fh, the grid's box) overlap when centred on the holes (tw x th apart per hole). A ship no
// bigger than the area it drills can work right beside another one.
static bool HolesTouch(int a, int b, int w, float tw, float th, float fw1, float fh1, float fw2, float fh2)
{
    return a == b || w > 0 && Math.Abs(a % w - b % w) * tw < (fw1 + fw2) / 2 && Math.Abs(a / w - b / w) * th < (fh1 + fh2) / 2;
}

// Another ship on this job has this hole, or one so close that the two hulls would touch. keeping: the
// hole is already ours, so only a ship with a lower id makes us give it up (both claimed at once).
bool HoleConflict(int cell, bool keeping)
{
    if (!trafficEnabled) return false;
    String key = JobKey();
    for (int i = 0; i < traffic.Count; i++)
    {
        TrafficShip t = traffic[i];
        if (t.job != key || t.hole < 0 || (keeping && t.id > myGridId)) continue;
        if (HolesTouch(t.hole, cell, jobWidth, JTW(), JTH(), shipFootWidth, shipFootHeight, t.fw, t.fh)) return true;
    }
    return false;
}

// Called before flying to hole idx (index in the mining order). Returns the next free hole index,
// -1 while waiting for the controller's answer, -2 when every hole left is taken by other ships
// (wait for one), or jobWidth * jobHeight when every hole is finished. A single ship gets its
// holes in the original order.
int ClaimHole(int idx)
{
    int n = jobWidth * jobHeight;
    if (n < 1) return idx;
    EnsureHoleOrder();
    // no record yet (new job, or a job saved by an older version): holes before the current one
    // were mined in order
    if (EnsureDoneHoles()) for (int k = 0; k < idx && k < n; k++) SetHoleDone(holeCells[k], true);
    if (myHole >= 0 && (idx >= n || holeCells[idx] != myHole)) { SetHoleDone(myHole, true); myHole = -1; } // moved on: finished
    if (JobCoordinated())
    {
        // the [PAM-Controller] assigns the holes
        if (coordCell >= 0 && !IsHoleDone(coordCell)) { holeWanted = false; myHole = coordCell; return holeCells.IndexOf(coordCell); }
        if (coordCell == -2) { myHole = -1; holeWanted = doneHoles.IndexOf('0') >= 0; return holeWanted ? -2 : n; } // all taken: wait (keep asking), all done: done
        // controller doesn't know this ship's hole yet: keep it, unless it gave that hole to another ship
        if (coordCell == -1 && myHole >= 0 && !IsHoleDone(myHole) && !coordOthers.Contains(myHole)) { holeWanted = false; return idx; }
        myHole = -1;
        if (!holeWanted) { holeWanted = true; coordWaitSince = TimeNow(); }
        if ((TimeNow() - coordWaitSince).TotalSeconds < 5) return -1; // wait for the answer
    }
    holeWanted = false;
    if (myHole >= 0 && !IsHoleDone(myHole) && !HoleConflict(myHole, true)) return idx;
    myHole = -1;
    for (int k = 0; k < n; k++)
    {
        int c = holeCells[k];
        if (IsHoleDone(c) || HoleConflict(c, false)) continue;
        myHole = c;
        return k;
    }
    return doneHoles.IndexOf('0') >= 0 ? -2 : n; // holes left, all taken by other ships: wait for one
}

// ---- job coordination by the [PAM-Controller] ----
// When a controller hears this job it keeps the master record (finished holes, which ship has
// which hole) and assigns the next hole when a ship asks (want flag in the traffic message). It
// broadcasts "J;job;doneHoles;ship:cell,..." on JOB_TAG. Without a controller (or if it doesn't
// answer within 5 s) the ships use the peer rules above.

const String JOB_TAG = "[PAMJOB]";
const String TCC_TAG = "[PAMTCC]";
IMyBroadcastListener coordListener = null;
DateTime coordTime = DateTime.MinValue;
DateTime coordWaitSince = TimeNow();
int coordCell = -1; // -1 none assigned (yet), -2 nothing left for this ship
List<int> coordOthers = new List<int>(); // holes the controller has given to other ships
bool holeWanted = false;

// coordCell -3: the controller's job state doesn't list this ship (it can't hear it): peer rules
bool JobCoordinated() { return trafficEnabled && coordCell != -3 && (TimeNow() - coordTime).TotalSeconds < 3; }

void ParseJobState(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 4 || f[0] != "J" || f[1] == "" || f[1] != JobKey()) return;
    coordTime = TimeNow();
    MergeDoneHoles(f[2]);
    coordCell = -3;
    coordOthers.Clear();
    String me = myGridId + ":";
    String[] e = f[3].Split(',');
    for (int i = 0; i < e.Length; i++)
    {
        int c = int.Parse(e[i].Substring(e[i].IndexOf(':') + 1));
        if (e[i].StartsWith(me)) coordCell = c; else coordOthers.Add(c);
    }
}

// hole index -> holeCol/holeRow, like GetNextHolePosition
SpiralResult SetHolePosition(int idx)
{
    EnsureHoleOrder();
    if (idx < 0 || idx >= holeCells.Count) return SpiralResult.AllDone;
    holeCol = holeCells[idx] % jobWidth;
    holeRow = holeCells[idx] / jobWidth;
    return SpiralResult.Found;
}

// ========================== 31_DockPool.cs ==========================
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
List<long> badDocks = new List<long>(); // connectors that didn't lock 3 times: not used again until docked somewhere
int dockTries = 0;

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

// Can this ship dock at that pool connector in its normal attitude (the one it has at home)? A
// connector turned another way (on a wall, upside down) would need the ship turned too: in
// gravity it can't fly like that. Also rejects broken data (zero / not perpendicular vectors).
bool PoseOk(PoolDock p)
{
    if (!dockRelSet || Math.Abs(p.fwd.Length() - 1) > 0.05f || Math.Abs(p.up.Length() - 1) > 0.05f || Math.Abs(Vector3.Dot(p.fwd, p.up)) > 0.05f) return false;
    return Vector3.Dot(DockPose(p.pos, p.fwd, p.up).downDir, homeDock.downDir) > 0.9f; // within ~25 degrees
}

// pool connectors this ship can use (sent to the station with the dock request)
String UsablePool()
{
    String s = "";
    for (int i = 0; i < dockPool.Count; i++) if (PoseOk(dockPool[i])) s += (s == "" ? "" : ",") + dockPool[i].id;
    return s;
}

int DockPhase()
{
    NavState s = currentNavState;
    if (s == NavState.Idle) return FindConnector(MyShipConnectorStatus.Connected) != null ? 3 : 0; // idle but docked still holds the connector
    if (s == NavState.Docking || s == NavState.Unloading || s == NavState.Charging || s == NavState.WaitUranium ||
        s == NavState.FillHydrogen || s == NavState.Undocking) return 3;
    // last leg: turned round in the connector's lane (the station clears the next ship from here)
    if ((s == NavState.ApproachDock && !toLane && !laneTurn) || s == NavState.AlignDock || s == NavState.RetryDocking) return 2; // after the turn at the lane
    if (s == NavState.FlyToDockArea || s == NavState.ApproachDock) return 1;
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
    if (!IsDockTaken(current, phase) && !badDocks.Contains(current)) return true;
    if (!IsDockTaken(homeConnId, phase) && !badDocks.Contains(homeConnId)) { poolDock = null; poolDockId = 0; return true; }
    if (!dockRelSet) return false;
    PoolDock best = null;
    float bestDist = 0;
    for (int i = 0; i < dockPool.Count; i++)
    {
        PoolDock p = dockPool[i];
        if (p.id == homeConnId || IsDockTaken(p.id, phase) || !PoseOk(p) || badDocks.Contains(p.id)) continue;
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

// ========================== 32_DockControl.cs ==========================
// ========================== DOCK CONTROL (station, [PAM-Controller] PB) ==========================
// The controller PB on the station runs the docking area: it reads the real status of the
// station's connectors (catches idle PAM ships and non-PAM ships), watches the docking area with
// [PAM-Zone] sensors and each connector's approach lane with a [PAM-Lane] camera, and clears one
// ship at a time to move in the area. Once that ship is on its last leg to its connector
// (approach, align, docking) it keeps its clearance and the next ship may come in, to another
// connector. Undocking is cleared before docking (when no ship is docking), so a full dock area
// can't jam. A station only answers ships whose home connector is on its own grid.

const String DOCK_TAG = "[PAMDCK]";
// clearanceTimeout [s]: a cleared ship that doesn't finish loses its clearance

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

// Raycast the approach lane of a connector with the [PAM-Lane] camera next to it (within 10 m). Anything
// in it but the station, the ship coming in (also its parts: hits inside its size) and PAM ships passing
// through (traffic handles them) blocks the lane;
// the block is kept 3 s after the last hit (no flapping while the camera charges).
class LaneBlock { public long conn, ship; public String what; public Vector3D cam, hit; public DateTime last; }
List<LaneBlock> laneBlocks = new List<LaneBlock>();
HashSet<long> homeConns = new HashSet<long>(); // ships' home connectors seen in requests (connector screen)

bool IsLaneClear(IMyShipConnector c, long ship)
{
    TCShip t = tcShips.Find(q => q.id == ship);
    for (int i = 0; i < laneCameras.Count; i++)
    {
        IMyCameraBlock cam = laneCameras[i];
        if (!cam.IsWorking || Vector3D.Distance(cam.GetPosition(), c.GetPosition()) > 10) continue;
        Vector3D end = cam.GetPosition() + c.WorldMatrix.Forward * dockLaneLength;
        if (!cam.CanScan(end)) continue; // camera not facing along the lane, or charging
        MyDetectedEntityInfo hit = cam.Raycast(end);
        if (hit.IsEmpty() || hit.EntityId == stationGrid.EntityId || hit.EntityId == ship || hit.Type == MyDetectedEntityType.FloatingObject) continue;
        if (t != null && hit.HitPosition.HasValue && Vector3D.Distance(hit.HitPosition.Value, t.pos) < t.dia * 0.6f) continue;
        TCShip other = tcShips.Find(q => q.id == hit.EntityId);
        if (other != null && other.vel.LengthSquared() > 0.25f) continue; // a PAM ship passing through: traffic rules handle that
        LaneBlock b = LaneBlockOf(c.EntityId);
        if (b == null) { b = new LaneBlock(); b.conn = c.EntityId; laneBlocks.Add(b); AddLog("Dock: " + c.CustomName + " blocked (" + hit.Name + " in its lane)"); }
        b.what = hit.Name; b.cam = cam.GetPosition(); b.hit = hit.HitPosition.HasValue ? hit.HitPosition.Value : end; b.last = TimeNow();
    }
    LaneBlock lb = LaneBlockOf(c.EntityId);
    if (lb == null) return true;
    dockAreaStatus = "Lane blocked: " + c.CustomName;
    return false;
}

LaneBlock LaneBlockOf(long conn) { return laneBlocks.Find(q => q.conn == conn); }

String blockedShips = ""; // "ship:connector," ships told their connector's lane is blocked

// the lane to the ship's connector is clear; if not, the ship is told (and the block remembers it)
bool LaneOk(long conn, long ship)
{
    IMyShipConnector c = StationConnector(conn);
    if (c == null || IsLaneClear(c, ship)) return true;
    LaneBlock b = LaneBlockOf(conn);
    if (b.ship != ship) AddLog("Dock: " + ShipName(ship) + " can't dock at " + c.CustomName + " (lane blocked), waits for another");
    b.ship = ship;
    return false;
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
    if (!r.canUsePool) return 0; // home removed: any usable pool connector (nearest to the first one)
    IMyShipConnector best = null;
    double bestDist = 0;
    for (int i = 0; i < stationDocks.Count; i++)
    {
        IMyShipConnector c = stationDocks[i];
        if (!IsDockFree(c) || StationConnector(c.EntityId) == null || DockTaken(c.EntityId) || (r.usable != null && !r.usable.Contains("," + c.EntityId + ",")) || r.bad.Contains("," + c.EntityId + ",")) continue;
        double d = Vector3D.Distance(c.GetPosition(), (home ?? stationDocks[0]).GetPosition());
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
    IMyShipConnector cc = StationConnector(conn);
    AddLog("Dock: " + ShipName(pick.ship) + (pick.kind == 2 ? " cleared to undock" : " cleared to " + (cc != null ? cc.CustomName : "dock")) + (pick.emg ? " (emergency)" : ""));
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
            DockRequest q = dockRequests.Find(x => x.ship == ship);
            if (q != null) AddLog("Dock: " + ShipName(ship) + (q.kind == 2 ? " left" : " docked"));
            for (int i = dockRequests.Count - 1; i >= 0; i--) if (dockRequests[i].ship == ship) dockRequests.RemoveAt(i);
            if (clearedShip == ship) clearedShip = 0;
            int d = dockingShips.IndexOf(ship);
            if (d >= 0) { dockingShips.RemoveAt(d); dockingConns.RemoveAt(d); }
        }
        else if (f[0] == "R" && f.Length >= 5)
        {
            long home = long.Parse(f[3]);
            // that ship's home is not on this station (or was removed: then its usable pool connectors say so)
            bool mine = StationConnector(home) != null;
            if (!mine && f.Length >= 6) foreach (String u in f[5].Split(',')) { long uid; if (long.TryParse(u, out uid) && StationConnector(uid) != null) mine = true; }
            if (!mine) return;
            homeConns.Add(home);
            DockRequest r = null;
            for (int i = 0; i < dockRequests.Count; i++) if (dockRequests[i].ship == ship) r = dockRequests[i];
            if (r == null) { r = new DockRequest(); r.ship = ship; r.first = TimeNow(); dockRequests.Add(r); }
            int kind = int.Parse(f[2]);
            if (r.kind != kind) r.first = TimeNow();
            r.kind = kind; r.home = home; r.canUsePool = f[4] == "1"; r.last = TimeNow();
            r.usable = f.Length >= 6 ? "," + f[5] + "," : null;
            String badWas = r.bad;
            bool emgWas = r.emg;
            r.bad = f.Length >= 7 ? "," + f[6] + "," : "";
            r.emg = f.Length >= 8 && f[7] == "1";
            if (badWas != "" && r.bad.Length > badWas.Length) AddLog("Dock: " + ShipName(ship) + " couldn't lock its connector, gets another");
            if (r.emg && !emgWas) AddLog("EMERGENCY: " + ShipName(ship) + " low on energy, docks first");
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
    if (cleared != null && (TimeNow() - clearedSince).TotalSeconds > clearanceTimeout)
    { AddLog("Dock: " + ShipName(clearedShip) + " too slow, back of the queue"); clearedShip = 0; cleared.first = TimeNow(); } // too slow: back of the queue
    // ships on their last leg that stopped asking (docked, or gave up): done
    for (int i = dockingShips.Count - 1; i >= 0; i--)
    {
        bool asking = false;
        for (int k = 0; k < dockRequests.Count; k++) if (dockRequests[k].ship == dockingShips[i]) asking = true;
        if (!asking) { dockingShips.RemoveAt(i); dockingConns.RemoveAt(i); }
    }
    // lane blocks gone laneClearTime (3 s) ago: connector usable again
    for (int i = laneBlocks.Count - 1; i >= 0; i--)
        if ((TimeNow() - laneBlocks[i].last).TotalSeconds > laneClearTime)
        {
            IMyShipConnector bc = StationConnector(laneBlocks[i].conn);
            AddLog("Dock: " + (bc != null ? bc.CustomName : "connector") + " lane clear again");
            laneBlocks.RemoveAt(i);
        }
    // something got into the lane of a connector a ship is coming to: that ship can't dock there ->
    // drop its clearance (it keeps its request and place, gets another connector or waits)
    blockedShips = "";
    if (clearedShip != 0 && clearedKind == 1 && !LaneOk(clearedConn, clearedShip)) clearedShip = 0;
    for (int i = dockingShips.Count - 1; i >= 0; i--)
        if (!LaneOk(dockingConns[i], dockingShips[i])) { dockingShips.RemoveAt(i); dockingConns.RemoveAt(i); }
    for (int i = 0; i < dockRequests.Count; i++)
        foreach (LaneBlock b in laneBlocks) if (b.ship == dockRequests[i].ship) blockedShips += dockRequests[i].ship + ":" + b.conn + ",";
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
    return msg + ";" + blockedShips; // ships whose connector's lane is blocked: ship:connector,...
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

// ========================== 33_DockClearance.cs ==========================
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
    dockBlockedMe = f.Length >= 9 && ("," + f[8]).Contains("," + myGridId + ":"); // its connector's lane is blocked
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
    if (!StationCleared(1)) { statusMessage = WaitText(); return false; }
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

bool dockBlockedMe = false;
String WaitText() { return dockBlockedMe ? "Connector blocked (obstacle in its lane): waiting" : "Waiting for dock clearance"; }

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
        SetFlightTarget(space ? emgStart : shipPosition + Vector3.Normalize(g) * 50, space ? 0 : emergencySpeed);
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

// ========================== 34_JobCoordinator.cs ==========================
// ========================== JOB COORDINATION (station, [PAM-Controller] PB) ==========================
// The controller listens to the ships' traffic messages and keeps the master record of each co-op
// job: finished holes and which ship has which hole. A ship that needs a hole sets a want flag;
// the controller assigns the next free hole in the mining order and broadcasts the job state twice a second. Claims are kept while a ship is out of
// range (e.g. unloading far away) and dropped after jobShipTimeout (600 s) without a message.


class JobShip
{
    public long id;
    public int cell = -1; // -1 none, -2 nothing left for it
    public bool want;
    public float fw, fh;
    public DateTime seen;
}

class JobRec
{
    public String key;
    public int w, h;
    public float tw, th;
    public String done;
    public List<int> order = new List<int>();
    public List<JobShip> ships = new List<JobShip>();
    public int reopen = -1;        // hole reopened with PREV: "finished" marks for it don't count until
    public DateTime reopenUntil;
}

List<JobRec> jobs = new List<JobRec>();
IMyBroadcastListener jobTrafficListener = null;
int jobSendTick = 0;

void UpdateJobCoordination()
{
    if (jobTrafficListener == null) jobTrafficListener = IGC.RegisterBroadcastListener(TRAFFIC_TAG);
    int budget = 50;
    while (jobTrafficListener.HasPendingMessage && budget-- > 0)
    {
        String data = jobTrafficListener.AcceptMessage().Data as String;
        if (data == null) continue;
        try { HandleJobTraffic(data); } catch { }
        try { HandleTrafficControl(data); } catch { }
    }
    for (int j = jobs.Count - 1; j >= 0; j--)
    {
        JobRec job = jobs[j];
        for (int i = job.ships.Count - 1; i >= 0; i--)
            if ((TimeNow() - job.ships[i].seen).TotalSeconds > jobShipTimeout) job.ships.RemoveAt(i);
        if (job.ships.Count == 0) { jobs.RemoveAt(j); continue; }
        AssignJobHoles(job);
    }
    if (++jobSendTick < 3) return; // ~2 per second
    jobSendTick = 0;
    for (int j = 0; j < jobs.Count; j++) IGC.SendBroadcastMessage(JOB_TAG, BuildJobMessage(jobs[j]));
}

void AssignJobHoles(JobRec job)
{
    for (int i = 0; i < job.ships.Count; i++)
    {
        JobShip s = job.ships[i];
        int was = s.cell;
        if (s.want && (s.cell < 0 || job.done[s.cell] == '1')) s.cell = NextFreeHole(job, s);
        if (s.cell != was && s.cell >= 0) AddLog("Job: " + ShipName(s.id) + " -> hole " + (job.order.IndexOf(s.cell) + 1));
    }
}

String BuildJobMessage(JobRec job)
{
    String msg = "J;" + job.key + ";" + job.done + ";";
    // only ships heard just now: a ship missing from the list knows the controller can't hear it
    // (it may still hear the controller) and picks its holes itself instead of waiting
    bool first = true;
    for (int i = 0; i < job.ships.Count; i++)
    {
        if ((TimeNow() - job.ships[i].seen).TotalSeconds > 5) continue;
        msg += (first ? "" : ",") + job.ships[i].id + ":" + job.ships[i].cell;
        first = false;
    }
    return msg;
}

// traffic message: id;pos;vel;dia;job;hole;doneHoles;dock,phase;heading;queued;want,footW,footH
void HandleJobTraffic(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 11 || f[4] == "") return; // no job, or a build without coordination
    String[] k = f[4].Split('|');
    if (k.Length < 8) return;
    JobRec job = null;
    for (int i = 0; i < jobs.Count; i++) if (jobs[i].key == f[4]) job = jobs[i];
    bool fresh = job == null; // just heard of this job (e.g. controller restarted): no log line per finished hole
    if (job == null)
    {
        job = new JobRec();
        job.key = f[4];
        job.w = int.Parse(k[3]); job.h = int.Parse(k[4]);
        job.tw = float.Parse(k[6]); job.th = float.Parse(k[7]);
        if (job.w < 1 || job.h < 1) return;
        BuildHoleOrder(job.w, job.h, (StartPosition)int.Parse(k[5]), job.order);
        job.done = new String('0', job.w * job.h);
        jobs.Add(job);
    }
    // hole reopened by the ship (PREV): not finished any more, for everyone
    int ro = f.Length >= 14 ? int.Parse(f[13]) : -1;
    if (ro >= 0 && ro < job.done.Length)
    {
        if (job.done[ro] == '1') AddLog("Job: hole " + (job.order.IndexOf(ro) + 1) + " reopened by " + ShipName(long.Parse(f[0])));
        char[] r = job.done.ToCharArray(); r[ro] = '0'; job.done = new String(r);
        job.reopen = ro; job.reopenUntil = TimeNow().AddSeconds(15);
    }
    // finished holes reported by the ship
    String d = f[6];
    if (d.Length == job.done.Length)
    {
        char[] c = job.done.ToCharArray();
        for (int i = 0; i < c.Length; i++)
            if (d[i] == '1' && c[i] != '1' && (i != job.reopen || TimeNow() > job.reopenUntil))
            {
                c[i] = '1';
                if (fresh) continue;
                int n = 0;
                for (int q = 0; q < c.Length; q++) if (c[q] == '1') n++;
                AddLog(n == c.Length ? "Job: all " + n + " holes done" : "Job: hole " + (job.order.IndexOf(i) + 1) + " done (" + n + "/" + c.Length + ")");
            }
        job.done = new String(c);
    }
    long id = long.Parse(f[0]);
    // the ship works this job now: drop it from any other job record (an old job then disappears)
    for (int j = 0; j < jobs.Count; j++) if (jobs[j] != job) jobs[j].ships.RemoveAll(o => o.id == id);
    JobShip s = null;
    for (int i = 0; i < job.ships.Count; i++) if (job.ships[i].id == id) s = job.ships[i];
    if (s == null) { s = new JobShip(); s.id = id; job.ships.Add(s); }
    s.seen = TimeNow();
    String[] w = f[10].Split(',');
    s.want = w[0] == "1";
    if (w.Length >= 3) { s.fw = float.Parse(w[1]); s.fh = float.Parse(w[2]); }
    // the ship's own claim (picked without the controller, or after a controller restart): keep it
    // unless another ship already has that hole
    int hole = int.Parse(f[5]);
    if (hole >= 0 && hole < job.done.Length && (s.cell < 0 || job.done[s.cell] == '1') && job.done[hole] != '1' && !HoleBlocked(job, hole, s))
        s.cell = hole;
}

JobShip HoleOwner(JobRec job, int cell, JobShip except)
{
    for (int i = 0; i < job.ships.Count; i++)
    {
        JobShip o = job.ships[i];
        if (o != except && o.cell == cell)
            return o;
    }
    return null;
}

// another ship has this hole, or one so close that their hulls would touch (HolesTouch)
bool HoleBlocked(JobRec job, int cell, JobShip s)
{
    foreach (JobShip o in job.ships)
        if (o != s && o.cell >= 0 && o.cell < job.done.Length && job.done[o.cell] != '1' && HolesTouch(o.cell, cell, job.w, job.tw, job.th, s.fw, s.fh, o.fw, o.fh)) return true;
    return false;
}

int NextFreeHole(JobRec job, JobShip s)
{
    for (int k = 0; k < job.order.Count; k++)
    {
        int c = job.order[k];
        if (job.done[c] != '1' && !HoleBlocked(job, c, s)) return c;
    }
    return -2;
}

String GetJobLines()
{
    String s = "";
    for (int j = 0; j < jobs.Count; j++)
    {
        JobRec job = jobs[j];
        int done = 0;
        for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') done++;
        s += "Job " + (j + 1) + ": " + done + "/" + job.done.Length + " holes, " + job.ships.Count + " ships\n";
    }
    return s;
}

// ========================== 35_TrafficControl.cs ==========================
// ========================== TRAFFIC CONTROL (station, [PAM-Controller] PB) ==========================
// The controller hears every PAM ship's traffic message, so it sees all of them at once, also
// ships that are out of each other's radio range. Every tick it predicts each pair a few seconds
// ahead (velocity, or the direction a stopped ship wants to go) and, when two would come closer
// than their size, tells one of them to slow down or hold. Who gives way, the same rules the ships
// use themselves: the ship cleared to dock goes; then the higher right of way; going the same way,
// the one catching up gives way; otherwise the one farther from where it is going; otherwise the
// higher id. Ships meeting head-on pass each other on the right by themselves. The choice is kept
// while the conflict lasts (unless a right of way changes), so it can't flip back and forth. Ships parked or
// hovering without wanting to move are obstacles for the ships' own avoidance, not for this.

// tccHorizon [s]: how far ahead conflicts are predicted

class TCShip
{
    public long id, pb; // grid id (traffic), PB id (the controller's ship list)
    public Vector3 pos, vel, heading;
    public float dia, fw, fh, limit = -1;
    public int jobFlag; // 1 in the job area, 2 deep in its hole
    public int prio;    // right of way: 2 leaving the job site, 1 moving between holes / arriving
    public float metric = -1; // distance it still has to go (whole metres)
    public String job = "";
    public int phase;
    public DateTime time;
    public int hole = -1, flag, bat = -1, h2, ur, nav = -1; // claimed hole, 1 queued / 2 parked, energy, flight state (screens)
    public bool emg;
    public Vector3 up;
    public float ms = -1; // its script's average run time
}

class TCPair
{
    public long a, b, yielder;
    public int sig; // both priorities when decided: decide again when one changes (e.g. a ship turns home)
    public DateTime last;
}

List<TCShip> tcShips = new List<TCShip>();
List<TCPair> tcPairs = new List<TCPair>();
int tcSendTick = 0;

// traffic message: id;pos;vel;dia;job;hole;doneHoles;dock,phase;heading;queued;want,footW,footH,jobFlag
void HandleTrafficControl(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 9) return;
    long id = long.Parse(f[0]);
    TCShip s = null;
    for (int i = 0; i < tcShips.Count; i++) if (tcShips[i].id == id) s = tcShips[i];
    bool isNew = s == null;
    if (isNew) { s = new TCShip(); s.id = id; tcShips.Add(s); }
    s.pos = StringToVector(f[1]);
    s.vel = StringToVector(f[2]);
    s.dia = float.Parse(f[3]);
    s.job = f[4];
    s.phase = int.Parse(f[7].Split(',')[1]);
    s.heading = StringToVector(f[8]);
    s.jobFlag = 0; s.prio = 0;
    if (f.Length >= 11)
    {
        String[] w = f[10].Split(',');
        if (w.Length >= 4) { s.fw = float.Parse(w[1]); s.fh = float.Parse(w[2]); s.jobFlag = int.Parse(w[3]); if (w.Length >= 5) s.prio = int.Parse(w[4]); s.metric = w.Length >= 6 ? float.Parse(w[5]) : -1; }
    }
    s.pb = f.Length >= 12 ? long.Parse(f[11]) : 0;
    s.time = TimeNow();
    s.hole = int.Parse(f[5]);
    s.flag = f.Length >= 10 ? int.Parse(f[9]) : 0;
    if (f.Length >= 13)
    {
        String[] e = f[12].Split(',');
        s.bat = int.Parse(e[0]); s.h2 = int.Parse(e[1]); s.ur = int.Parse(e[2]); s.emg = e[3] == "1";
        s.nav = e.Length >= 6 ? int.Parse(e[4]) : -1; s.up = StringToVector(e[e.Length - 1]) / 10;
        s.ms = e.Length >= 7 ? float.Parse(e[5]) : -1;
    }
    if (isNew) AddLog("Contact: " + TCName(s));
}

// not moving and not wanting to move (docked, parked, waiting in the dock queue, idle)
bool TCParked(TCShip s) { return s.vel.LengthSquared() < 0.25f && (s.phase == 3 || s.heading.LengthSquared() < 0.25f); }

// where the ship is going: its velocity, or for a stopped ship the direction it wants to go
Vector3 TCIntent(TCShip s) { return s.vel.LengthSquared() >= 1f ? s.vel : s.heading * 2f; }

void UpdateTrafficControl()
{
    ComputeTrafficControl();
    if (++tcSendTick < 3) return; // ~2 per second
    tcSendTick = 0;
    IGC.SendBroadcastMessage(TCC_TAG, BuildTrafficControlMessage());
}

void ComputeTrafficControl()
{
    for (int i = tcShips.Count - 1; i >= 0; i--)
        // stopped ships (docked, idle) run and send less often: kept longer
        if ((TimeNow() - tcShips[i].time).TotalSeconds > (tcShips[i].vel.LengthSquared() < 0.25f ? 12 : 3)) { AddLog("Contact lost: " + TCName(tcShips[i])); tcShips.RemoveAt(i); }
    for (int i = tcPairs.Count - 1; i >= 0; i--)
        if ((TimeNow() - tcPairs[i].last).TotalSeconds > 2) tcPairs.RemoveAt(i); // conflict over: forget who gave way
    for (int i = 0; i < tcShips.Count; i++) tcShips[i].limit = -1;

    for (int i = 0; i < tcShips.Count; i++)
        for (int j = i + 1; j < tcShips.Count; j++)
        {
            TCShip a = tcShips[i], b = tcShips[j];
            if (TCParked(a) || TCParked(b)) continue;
            Vector3 pa = a.pos + a.vel * (float)(TimeNow() - a.time).TotalSeconds;
            Vector3 pb = b.pos + b.vel * (float)(TimeNow() - b.time).TotalSeconds;
            Vector3 ua = TCIntent(a), ub = TCIntent(b);
            Vector3 rel = pb - pa;
            float dist = rel.Length();
            float clear = (a.dia + b.dia) * 0.55f;
            // two ships working the same job drill different holes: only closer than one hole spacing counts
            bool sameJob = a.job != "" && a.job == b.job;
            if (sameJob && (a.jobFlag == 2 || b.jobFlag == 2)) continue; // one is inside the rock in its hole
            if (sameJob && a.jobFlag == 1 && b.jobFlag == 1)
            {
                String[] k = a.job.Split('|');
                if (k.Length >= 8) clear = Math.Min(float.Parse(k[6]), float.Parse(k[7])) * 0.9f;
            }
            Vector3 ru = ub - ua;
            float ru2 = ru.LengthSquared();
            float t = ru2 > 0.01f ? -Vector3.Dot(rel, ru) / ru2 : 0;
            bool tooClose = dist < clear;
            if (!tooClose && (t <= 0 || t > tccHorizon || (rel + ru * t).Length() >= clear)) continue;
            // head-on: the ships pass each other on the right themselves (holding one would stop it from
            // stepping aside); the controller handles ships catching up and crossing paths
            if (!tooClose && a.prio == b.prio && ua.LengthSquared() > 0.25f && ub.LengthSquared() > 0.25f && Vector3.Dot(Vector3.Normalize(ua), Vector3.Normalize(ub)) < -0.7f)
                continue;

            TCPair p = null;
            for (int k = 0; k < tcPairs.Count; k++)
                if ((tcPairs[k].a == a.id && tcPairs[k].b == b.id) || (tcPairs[k].a == b.id && tcPairs[k].b == a.id)) p = tcPairs[k];
            int sig = a.id < b.id ? a.prio * 8 + b.prio : b.prio * 8 + a.prio;
            if (p == null) { p = new TCPair(); p.a = a.id; p.b = b.id; p.sig = sig + 1; tcPairs.Add(p); }
            if (p.sig != sig)
            {
                long was = p.yielder;
                p.sig = sig; p.yielder = TCYielder(a, b, ua, ub, rel, dist);
                if (p.yielder != was) AddLog("Traffic: " + (p.yielder == a.id ? TCName(a) + " gives way to " + TCName(b) : TCName(b) + " gives way to " + TCName(a)));
            }
            p.last = TimeNow();
            TCShip y = p.yielder == a.id ? a : b;
            float lim = tooClose || t < 4 ? 0 : Math.Max(1, y.vel.Length() * 0.5f); // close: hold, later: slow down
            if (y.limit < 0 || lim < y.limit) y.limit = lim;
        }
    HoldQueuedShips();
}

long TCYielder(TCShip a, TCShip b, Vector3 ua, Vector3 ub, Vector3 rel, float dist)
{
    if (a.id == clearedShip || dockingShips.Contains(a.id)) return b.id; // cleared to dock by this station
    if (b.id == clearedShip || dockingShips.Contains(b.id)) return a.id;
    if (a.prio != b.prio) return a.prio < b.prio ? a.id : b.id; // ships leaving the job site go first
    // going the same way: the one catching up with the other gives way (the ships do that too: they follow)
    if (Vector3.Dot(ua, ub) > 0)
    {
        Vector3 n = dist > 0.01f ? rel / dist : new Vector3(1, 0, 0);
        float ta = Vector3.Dot(ua, n), tb = Vector3.Dot(ub, -n); // speed towards the other ship
        if (ta - tb > 1) return a.id;
        if (tb - ta > 1) return b.id;
    }
    // otherwise the one closer to where it is going goes first (same order as the ships use)
    if (a.metric >= 0 && b.metric >= 0 && a.metric != b.metric) return a.metric > b.metric ? a.id : b.id;
    return a.id > b.id ? a.id : b.id;
}

String BuildTrafficControlMessage()
{
    String msg = "C;";
    bool first = true;
    for (int i = 0; i < tcShips.Count; i++)
    {
        if (tcShips[i].limit < 0) continue;
        msg += (first ? "" : ",") + tcShips[i].id + ":" + Math.Round(tcShips[i].limit, 1);
        first = false;
    }
    return msg;
}

String GetTrafficControlLine()
{
    int moving = 0, held = 0;
    for (int i = 0; i < tcShips.Count; i++)
    {
        if (!TCParked(tcShips[i])) moving++;
        if (tcShips[i].limit >= 0) held++;
    }
    if (tcShips.Count == 0) return "";
    return "Traffic: " + moving + " moving" + (held > 0 ? ", " + held + " giving way" : "") + "\n";
}

// ---- "Move home" for all ships: one at a time ----
// Every ship gets the order at once, so none of them starts a new hole; traffic control then holds
// all but one (in their holes, or where they are). Order: ships already out of the job area first,
// then ships at the surface of the job area, then ships still deep in their holes; within each
// group the ship closest to the job position (where the path starts) goes first. The next ship
// may go when the previous one has moved away (3 ship sizes, at least 40 m), at once if the
// previous one is docked, or after 45 s at the latest.
List<ShipInfo> homeQueue = new List<ShipInfo>();
ShipInfo homeCurrent = null;
Vector3 homeStartPos;
DateTime homeSentTime;
float homeReleaseDist = 40;
bool homeCurrentDocked = false;

TCShip FindTCShip(ShipInfo s)
{
    TCShip best = null;
    for (int i = 0; i < tcShips.Count; i++) if (tcShips[i].pb != 0 && "" + tcShips[i].pb == s.id) return tcShips[i];
    for (int i = 0; i < tcShips.Count; i++)
        if (Vector3.Distance(tcShips[i].pos, s.position) < 15 && (best == null ||
            Vector3.Distance(tcShips[i].pos, s.position) < Vector3.Distance(best.pos, s.position))) best = tcShips[i];
    return best;
}

float HomeOrderKey(ShipInfo s)
{
    TCShip t = FindTCShip(s);
    if (t == null) return 0;
    float k = t.jobFlag * 1000000f;
    String[] key = t.job.Split('|');
    if (key.Length >= 3) k += Vector3.Distance(t.pos, new Vector3(float.Parse(key[0]), float.Parse(key[1]), float.Parse(key[2])));
    return k;
}

void StartStagedHome()
{
    homeQueue.Clear();
    homeCurrent = null;
    homeHeld.Clear();
    List<float> keys = new List<float>();
    for (int i = 0; i < connectedShips.Count; i++)
    {
        float k = HomeOrderKey(connectedShips[i]);
        int at = 0;
        while (at < keys.Count && keys[at] <= k) at++;
        keys.Insert(at, k);
        homeQueue.Insert(at, connectedShips[i]);
        connectedShips[i].ProcessCommand("HOMEPOS", true, false, false);
        SendBroadcastTo("HOMEPOS", connectedShips[i].id, "HOMEPOS");
    }
}

// ships of the staged "Move home" that have to wait: hold them (called after the pair checks)
List<TCShip> homeHeld = new List<TCShip>();
void HoldQueuedShips()
{
    homeHeld.Clear();
    for (int i = 0; i < homeQueue.Count; i++)
    {
        TCShip t = FindTCShip(homeQueue[i]);
        if (t == null) continue;
        t.limit = 0;
        homeHeld.Add(t);
    }
}

void UpdateStagedHome()
{
    if (homeCurrent != null)
    {
        bool gone = homeCurrentDocked || Vector3.Distance(homeCurrent.position, homeStartPos) > homeReleaseDist ||
            (TimeNow() - homeSentTime).TotalSeconds > 45 || homeCurrent.IsTimedOut();
        if (!gone) return;
        homeCurrent = null;
    }
    if (homeQueue.Count == 0) return;
    homeCurrent = homeQueue[0];
    homeQueue.RemoveAt(0);
    AddLog("Move home: " + homeCurrent.name + " goes (" + homeQueue.Count + " waiting)");
    homeStartPos = homeCurrent.position;
    homeSentTime = TimeNow();
    TCShip t = FindTCShip(homeCurrent);
    homeReleaseDist = t == null ? 40 : Math.Max(40, t.dia * 3);
    homeCurrentDocked = t != null && t.phase == 3 && t.vel.LengthSquared() < 0.25f;
}

// ========================== 36_DebugDraw.cs ==========================
// ========================== DEBUG DRAW (station, [PAM-Controller] PB) ==========================
// With debugDraw on and Digi's "(DevTool) Programmable Block DebugAPI" mod in the world (offline
// worlds only), the controller draws what it knows in 3D, twice a second:
//  - every PAM ship: wire sphere of its size (green moving, yellow giving way, grey parked),
//    white line where it will be in 3 s, cyan line the way it wants to go
//  - red line between two ships the controller sees on a collision course
//  - with debugDraw on the ships too: blue line to each ship's flight target, its camera rays
//    (green clear, red hit), and the hole grid of each job (grey done, orange claimed with a line
//    from the ship to its hole, white free)
//  - docking area: [PAM-Zone] sensor boxes, [PAM-Lane] camera rays, green line from the ship cleared
//    to dock to its connector
// Text labels (HUD markers) name the ships with their state, the conflicts (who gives way), the
// claimed holes, the docking area and the cleared ship. The colour key is in the PB's output.
// Without the mod nothing is drawn and nothing else changes.

const String DBG_TAG = "[PAMDBG]";

// trimmed copy of the DebugAPI class from the mod's ForPB.cs (github.com/THDigi/PBDrawAPI)
class DebugAPI
{
    public readonly bool ModDetected;
    IMyProgrammableBlock _pb;
    Action<IMyProgrammableBlock> _rmd;
    Func<IMyProgrammableBlock, Vector3D, Color, float, float, bool, int> _p;
    Func<IMyProgrammableBlock, Vector3D, Vector3D, Color, float, float, bool, int> _ln;
    Func<IMyProgrammableBlock, BoundingSphereD, Color, int, float, int, float, bool, int> _sph;
    Func<IMyProgrammableBlock, string, Vector3D, Color?, float, int> _gps;

    public DebugAPI(MyGridProgram program)
    {
        _pb = program.Me;
        var methods = _pb.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string, Delegate>>()?.GetValue(_pb);
        if (methods == null) return;
        _rmd = (Action<IMyProgrammableBlock>)methods["RemoveDraw"];
        _p = (Func<IMyProgrammableBlock, Vector3D, Color, float, float, bool, int>)methods["Point"];
        _ln = (Func<IMyProgrammableBlock, Vector3D, Vector3D, Color, float, float, bool, int>)methods["Line"];
        _sph = (Func<IMyProgrammableBlock, BoundingSphereD, Color, int, float, int, float, bool, int>)methods["Sphere"];
        _gps = (Func<IMyProgrammableBlock, string, Vector3D, Color?, float, int>)methods["GPS"];
        ((Action<IMyProgrammableBlock>)methods["RemoveAll"])(_pb);
        ModDetected = true;
    }

    public void RemoveDraw() { if (_rmd != null) _rmd(_pb); }
    public void Line(Vector3D a, Vector3D b, Color c, float thickness) { if (_ln != null) _ln(_pb, a, b, c, thickness, -1, false); }
    public void Point(Vector3D a, Color c, float radius) { if (_p != null) _p(_pb, a, c, radius, -1, false); }
    public void Label(String text, Vector3D at, Color c) { if (_gps != null) _gps(_pb, text, at, c, -1); }
    public void Sphere(Vector3D center, double radius, Color c) { if (_sph != null) _sph(_pb, new BoundingSphereD(center, radius), c, 1, 0.05f, 30, -1, false); }
}

class DbgShip
{
    public long id;
    public Vector3 jobPos, left, down, target, fwd;
    public bool hasJob;
    public List<Vector3> rayFrom = new List<Vector3>(), rayTo = new List<Vector3>();
    public List<bool> rayHit = new List<bool>();
    public DateTime time;
}

DebugAPI dbg = null;
int dbgTick = 0;
IMyBroadcastListener dbgListener = null;
List<DbgShip> dbgShips = new List<DbgShip>();

// ship debug message: id;jobPos;left;down;target;rays (from,to,hit:...)
void HandleDebugMessage(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 6) return;
    long id = long.Parse(f[0]);
    DbgShip s = null;
    for (int i = 0; i < dbgShips.Count; i++) if (dbgShips[i].id == id) s = dbgShips[i];
    if (s == null) { s = new DbgShip(); s.id = id; dbgShips.Add(s); }
    s.jobPos = StringToVector(f[1]); s.left = StringToVector(f[2]); s.down = StringToVector(f[3]);
    s.hasJob = s.left.LengthSquared() > 0.5f;
    s.target = StringToVector(f[4]);
    s.time = TimeNow();
    if (f.Length >= 7) s.fwd = StringToVector(f[6]);
    if (f[5] == "") return; // keep the last rays until new ones come
    s.rayFrom.Clear(); s.rayTo.Clear(); s.rayHit.Clear();
    String[] rays = f[5].Split(':');
    for (int i = 0; i < rays.Length; i++)
    {
        String[] r = rays[i].Split(',');
        if (r.Length < 3) continue;
        s.rayFrom.Add(StringToVector(r[0])); s.rayTo.Add(StringToVector(r[1])); s.rayHit.Add(r[2] == "1");
    }
}

void UpdateDebugDraw()
{
    if (!debugDraw) return;
    if (dbg == null) dbg = new DebugAPI(this);
    if (!dbg.ModDetected) return;
    if (dbgListener == null) dbgListener = IGC.RegisterBroadcastListener(DBG_TAG);
    int budget = 20;
    while (dbgListener.HasPendingMessage && budget-- > 0)
    {
        String data = dbgListener.AcceptMessage().Data as String;
        if (data != null) try { HandleDebugMessage(data); } catch { }
    }
    for (int i = dbgShips.Count - 1; i >= 0; i--) if ((TimeNow() - dbgShips[i].time).TotalSeconds > 5) dbgShips.RemoveAt(i);
    if (++dbgTick % 3 != 0) return; // redraw ~2x a second (the drawings stay until removed)

    dbg.RemoveDraw();
    // many ships with debugDraw on: everything below stops near the instruction limit instead of the
    // script being stopped ("Script Too Complex"); the rest is drawn on the next redraw
    // recorded paths (ships sharing a path: drawn once)
    List<String> drawn = new List<String>();
    foreach (String ps in shipPaths.Values)
    {
        if (ps == "" || drawn.Contains(ps) || Heavy()) continue;
        drawn.Add(ps);
        String[] pts = ps.Split(':');
        for (int k = 1; k < pts.Length; k++) dbg.Line(StringToVector(pts[k - 1]), StringToVector(pts[k]), Color.SkyBlue, 0.1f);
        dbg.Label("Path start", StringToVector(pts[0]), Color.SkyBlue);
        dbg.Label("Path end", StringToVector(pts[pts.Length - 1]), Color.SkyBlue);
    }
    // ships
    for (int i = 0; i < tcShips.Count && !Heavy(); i++)
    {
        TCShip s = tcShips[i];
        Color c = TCParked(s) ? Color.Gray : s.limit >= 0 ? Color.Yellow : Color.Lime;
        dbg.Sphere(s.pos, s.dia / 2, c);
        dbg.Label(TCName(s) + ": " + (TCParked(s) ? "parked" : s.limit == 0 ? "holding (giving way)" : s.limit > 0 ? "slowing (giving way)" : "moving"), s.pos, c);
        if (s.vel.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.vel * 3, Color.White, 0.1f);
        if (s.heading.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.heading * s.dia, Color.Cyan, 0.1f);
    }
    // ships on a collision course
    for (int i = 0; i < tcPairs.Count; i++)
    {
        if ((TimeNow() - tcPairs[i].last).TotalSeconds > 0.5) continue;
        TCShip a = null, b = null;
        for (int k = 0; k < tcShips.Count; k++)
        {
            if (tcShips[k].id == tcPairs[i].a) a = tcShips[k];
            if (tcShips[k].id == tcPairs[i].b) b = tcShips[k];
        }
        if (a == null || b == null) continue;
        dbg.Line(a.pos, b.pos, Color.Red, 0.2f);
        dbg.Label("Conflict: " + TCName(tcPairs[i].yielder == a.id ? a : b) + " gives way", (a.pos + b.pos) / 2, Color.Red);
    }
    // what the ships send: flight target, camera rays, hole grid
    drawn.Clear(); // jobs whose hole grid is drawn
    for (int i = 0; i < dbgShips.Count && !Heavy(); i++)
    {
        DbgShip d = dbgShips[i];
        TCShip t = null;
        for (int k = 0; k < tcShips.Count; k++) if (tcShips[k].id == d.id) t = tcShips[k];
        if (t != null) dbg.Line(t.pos, d.target, Color.Blue, 0.08f);
        for (int r = 0; r < d.rayFrom.Count; r++) dbg.Line(d.rayFrom[r], d.rayTo[r], d.rayHit[r] ? Color.Red : Color.Green, 0.04f);
        if (t == null || !d.hasJob) continue;
        JobRec job = null;
        for (int j = 0; j < jobs.Count; j++) if (jobs[j].key == t.job) job = jobs[j];
        if (job == null) continue;
        int doneCount = 0;
        for (int c = 0; c < job.done.Length; c++) if (job.done[c] == '1') doneCount++;
        bool firstOfJob = !drawn.Contains(job.key);
        if (firstOfJob) drawn.Add(job.key);
        if (firstOfJob)
        {
            dbg.Label("Job: " + doneCount + "/" + job.done.Length + " holes done", d.jobPos, Color.White);
            // waiting ring (as the ships compute it, for this ship's size): 6 spots
            for (int k = 0; k < 6; k++)
            {
                double a = k * Math.PI / 3;
                dbg.Point(d.jobPos - d.fwd * t.dia * 2.5f + (d.left * (float)Math.Cos(a) + d.down * (float)Math.Sin(a)) * t.dia * 1.2f, Color.Violet, t.dia * 0.15f);
            }
            dbg.Label("Waiting ring", d.jobPos - d.fwd * t.dia * 2.5f, Color.Violet);
        }
        // the grid once per job; for the other ships only their own hole
        JobShip own = job.ships.Find(q => q.id == d.id);
        for (int c = 0; c < job.done.Length; c++)
        {
            if (!firstOfJob && (own == null || own.cell != c)) continue;
            int col = c % job.w, row = c / job.w;
            Vector3 p = d.jobPos + d.left * ((job.w - 1f) / 2f - col) * job.tw - d.down * ((job.h - 1f) / 2f - row) * job.th;
            JobShip owner = HoleOwner(job, c, null);
            Color hc = job.done[c] == '1' ? Color.Gray : owner != null ? Color.Orange : Color.White;
            dbg.Point(p, hc, Math.Min(job.tw, job.th) * 0.2f);
            // where ships line up over the hole (one ship size above the surface) and come straight down
            if (firstOfJob) dbg.Line(p, p - d.fwd * t.dia, hc, 0.05f);
            if (owner == null || owner.id != d.id) continue;
            dbg.Line(t.pos, p, Color.Orange, 0.05f);
            dbg.Label("Hole " + (job.order.IndexOf(c) + 1) + ": " + TCName(t), p, Color.Orange);
        }
    }
    // docking area
    for (int i = 0; i < zoneSensors.Count; i++)
    {
        IMySensorBlock z = zoneSensors[i];
        // the sensor box: 12 edges
        MatrixD m = z.WorldMatrix;
        Vector3D[] c = new Vector3D[8];
        for (int k = 0; k < 8; k++)
            c[k] = z.GetPosition() + m.Right * ((k & 1) == 0 ? -z.LeftExtend : z.RightExtend) + m.Up * ((k & 2) == 0 ? -z.BottomExtend : z.TopExtend) + m.Forward * ((k & 4) == 0 ? -z.BackExtend : z.FrontExtend);
        for (int k = 0; k < 8; k++)
            for (int b = 1; b < 8; b <<= 1)
                if ((k & b) == 0) dbg.Line(c[k], c[k | b], Color.Cyan, 0.1f);
        dbg.Label("Docking area" + (dockAreaStatus != "" ? ": " + dockAreaStatus : ""), z.GetPosition(), Color.Cyan);
    }
    for (int i = 0; i < laneCameras.Count; i++)
        dbg.Line(laneCameras[i].GetPosition(), laneCameras[i].GetPosition() + laneCameras[i].WorldMatrix.Forward * dockLaneLength, Color.Purple, 0.05f);
    // blocked lanes: red from the lane camera to what's in the way, with what and which connector
    foreach (LaneBlock b in laneBlocks)
    {
        IMyShipConnector bc = StationConnector(b.conn);
        dbg.Line(b.cam, b.hit, Color.Red, 0.2f);
        dbg.Point(b.hit, Color.Red, 0.6f);
        dbg.Label("Lane blocked: " + b.what + (bc != null ? " (" + bc.CustomName + ")" : "") + (b.ship != 0 ? ", " + ShipName(b.ship) + " can't dock" : ""), b.hit, Color.Red);
    }
    if (clearedShip != 0 && clearedConn != 0)
    {
        IMyShipConnector c = StationConnector(clearedConn);
        for (int k = 0; k < tcShips.Count; k++)
            if (c != null && tcShips[k].id == clearedShip)
            {
                dbg.Line(tcShips[k].pos, c.GetPosition(), Color.Lime, 0.15f);
                dbg.Label("Cleared to " + (clearedKind == 2 ? "undock: " : "dock: ") + TCName(tcShips[k]), c.GetPosition(), Color.Lime);
            }
    }
}

// ship name from the controller's ship list (matched by position), else the end of its id
String TCName(TCShip t)
{
    ShipInfo best = null;
    for (int i = 0; i < connectedShips.Count; i++) if (t.pb != 0 && connectedShips[i].id == "" + t.pb) return connectedShips[i].name;
    for (int i = 0; i < connectedShips.Count; i++)
        if (Vector3.Distance(connectedShips[i].position, t.pos) < 30 && (best == null ||
            Vector3.Distance(connectedShips[i].position, t.pos) < Vector3.Distance(best.position, t.pos))) best = connectedShips[i];
    if (best != null && best.name != "") return best.name;
    String id = t.id.ToString();
    return "Ship " + id.Substring(Math.Max(0, id.Length - 4));
}

// colour key and mod status for the PB's output
String GetDebugDrawLegend()
{
    if (!debugDraw) return "";
    if (dbg == null || !dbg.ModDetected) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
    return "\nDebug draw: on\n" +
        "Ship sphere: green moving, yellow giving way, grey parked\n" +
        "White: where it is in 3 s, cyan: where it wants to go\n" +
        "Red line: collision course (label: who gives way)\n" +
        "Blue: flight target; thin green/red: camera rays (clear/hit)\n" +
        "Points: grey hole done, orange claimed, white free\n" +
        "Line up from a hole: where ships line up over it and come down\n" +
        "Violet: waiting ring; sky blue: recorded path\n" +
        "Cyan box: docking area, purple: lane cameras, red: lane blocked\n" +
        "Thick green: ship cleared to its connector\n";
}

// ========================== 37_Settings.cs ==========================
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

// ========================== 38_Screens.cs ==========================
// ========================== CONTROLLER SCREENS ==========================
// Extra LCD modes (Custom Data "mode=..."):
//   fleet          one line per ship: state, cargo, energy, hole, flags; docking area and jobs
//   ship:<name>    one ship in detail
//   log            coordination events (docking, holes, giving way, emergencies, contact), newest first,
//                  coloured by kind (drawn; text size = the LCD's font size)
//   job:<n>        hole matrix of job n (drawn, as seen behind a ship drilling it)
//   map[:side|top|dock|all]  side view / top view / docking area / all three (drawn, see the maps below)
//   docks          the station's connectors: free / cleared / docking / docked / BLOCKED lane / damaged
// The drawn ones redraw about once a second.

List<String> logLines = new List<String>();
List<DateTime> logTimes = new List<DateTime>();
Dictionary<long, String> shipPaths = new Dictionary<long, String>(); // grid id -> recorded path "x|y|z:..."
IMyBroadcastListener pathListener = null;
Vector3 planetCenter = new Vector3();
bool hasPlanet = false;
int screenTick = 0;

void AddLog(String msg)
{
    // the same event again within 30 s (a conflict that comes and goes): once is enough
    for (int i = 0; i < logLines.Count && i < 10; i++) if (logLines[i].EndsWith(msg) && (TimeNow() - logTimes[i]).TotalSeconds < 30) return;
    logLines.Insert(0, DateTime.Now.ToString("HH:mm:ss ") + msg);
    logTimes.Insert(0, TimeNow());
    if (logLines.Count > 40) { logLines.RemoveAt(40); logTimes.RemoveAt(40); }
}

String ShipName(long gridId)
{
    TCShip t = tcShips.Find(q => q.id == gridId);
    if (t != null) return TCName(t);
    String id = gridId.ToString();
    return "Ship " + id.Substring(Math.Max(0, id.Length - 4));
}

void UpdateScreens()
{
    if (pathListener == null) pathListener = IGC.RegisterBroadcastListener("[PAMPTH]");
    int budget = 10;
    while (pathListener.HasPendingMessage && budget-- > 0)
    {
        String data = pathListener.AcceptMessage().Data as String;
        if (data == null) continue;
        String[] f = data.Split(';');
        long id;
        if (f.Length < 2 || !long.TryParse(f[0], out id)) continue;
        // at most ~100 points (start and end kept): drawing many long paths stays cheap
        String[] w = f[1].Split(':');
        int st = Math.Max(1, w.Length / 100);
        shipPaths[id] = w.Length <= 100 ? f[1] : String.Join(":", w.Where((q, i) => i % st == 0 || i == w.Length - 1));
        if (f.Length >= 3 && f[2] != "") { planetCenter = StringToVector(f[2]); hasPlanet = true; } // id;path;planet centre
    }
}

bool SpriteMode(LCDMode m) { return m == LCDMode.Map || m == LCDMode.Job || m == LCDMode.Log; }

String ShortState(String s)
{
    return s.Replace("Flying to ", "To ").Replace(" position", " pos").Replace("Returning to dock", "Going home").Replace("Waiting for ", "Wait ")
        .Replace("Approaching dock", "Approach").Replace("Charging batteries", "Charging");
}

String Pad(String s, int n) { return s.Length >= n ? s.Substring(0, n) : s + new String(' ', n - s.Length); }

String Pct(int v) { return v < 0 ? "  - " : (v + "%").PadLeft(4); }

// flags for the fleet list / ship page
String ShipFlags(TCShip t)
{
    if (t == null) return "no traffic";
    String f = "";
    if (t.emg) f += "EMG ";
    if (t.flag == 2) f += "PARKED ";
    else if (t.flag == 1) f += "QUEUE ";
    if (t.prio < 0) f += "WAIT ";
    if (t.limit >= 0) f += "GIVE ";
    if (clearedShip == t.id || dockingShips.Contains(t.id)) f += "CLEARED ";
    return f.Trim();
}

// this PB's run time (server performance)
String ExecLine() { return "Run time: avg " + Math.Round(execAvg, 3) + " ms, max " + Math.Round(execPeak, 2) + " ms (10 s)\n"; }

String BuildFleetScreen()
{
    String text = "PAM fleet | " + connectedShips.Count + " ships | controller " + Math.Round(execAvg, 2) + " ms (max " + Math.Round(execPeak, 2) + ")\n" + GetDockAreaLine() + GetJobLines() + GetTrafficControlLine();
    int nw = 6; // name column: as wide as the longest name (max 20 characters)
    for (int i = 0; i < connectedShips.Count; i++) nw = Math.Min(21, Math.Max(nw, connectedShips[i].name.Length + 1));
    text += Pad("Ship", nw) + Pad("State", 15) + " Crg  Bat  H2  Hole   ms  Flags\n";
    for (int i = 0; i < connectedShips.Count; i++)
    {
        ShipInfo s = connectedShips[i];
        TCShip t = FindTCShip(s);
        String hole = "  -";
        if (t != null && t.hole >= 0)
        {
            JobRec job = jobs.Find(q => q.key == t.job);
            hole = ("" + (job != null ? job.order.IndexOf(t.hole) + 1 : t.hole + 1)).PadLeft(3);
        }
        // two ship scripts with the same name: the end of each one's PB id tells them apart
        String nm = connectedShips.Count(q => q.name == s.name) > 1 ? s.name + " #" + s.id.Substring(Math.Max(0, s.id.Length - 4)) : s.name;
        text += Pad(nm, nw - 1) + " " + Pad(s.IsTimedOut() ? "NO CONTACT" : ShortState(t != null && t.nav >= 0 ? GetNavStateName((NavState)t.nav) : s.stateText), 14) + " " +
            Pct((int)(SafeDiv(s.currentVolume, s.maxVolume) * 100)) + " " + Pct(t != null ? t.bat : -1) + " " + Pct(t != null ? t.h2 : -1) + " " + hole + " " + (t != null && t.ms >= 0 ? t.ms.ToString("0.00") : "   -").PadLeft(5) + "  " + ShipFlags(t) + "\n";
    }
    return text;
}

String BuildShipScreen(String name)
{
    ShipInfo s = FindShip("", name);
    if (s == null) return "Unknown ship: " + name;
    TCShip t = FindTCShip(s);
    String text = s.name + " | " + s.distance + " m from here" + (s.IsTimedOut() ? " | NO CONTACT" : "") + "\n" + "—————————————————————\n";
    text += "Job:    " + s.stateText + "\n" + "Status: " + s.statusMessage + "\n";
    text += ProgressBar("Cargo", SafeDiv(s.currentVolume, s.maxVolume) * 100f, 100, 20, 8, 7) + "%\n";
    if (t == null) return text + "(no traffic data: traffic off or out of range)\n";
    if (t.nav >= 0) text += "Doing:  " + GetNavStateName((NavState)t.nav) + "\n";
    text += "Battery " + Pct(t.bat).Trim() + "  H2 " + Pct(t.h2).Trim() + "  U " + t.ur + " kg\n";
    text += "Speed " + Math.Round(t.vel.Length(), 1) + " m/s" + (t.ms >= 0 ? "  Script " + t.ms.ToString("0.00") + " ms" : "") + "\n";
    JobRec job = jobs.Find(q => q.key == t.job);
    if (job != null)
    {
        int n = 0;
        for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') n++;
        text += "Job " + (jobs.IndexOf(job) + 1) + ": " + n + "/" + job.done.Length + " done" + (t.hole >= 0 ? ", hole " + (job.order.IndexOf(t.hole) + 1) : "") + "\n";
    }
    text += "Right of way: " + Pick("waiting|normal|between holes|heading home|docking / emergency", t.prio + 1) + (t.metric >= 0 ? ", " + t.metric + " m to go" : "") + "\n";
    text += "Docking: " + Pick("-|to the docking area|last leg|docked / undocking", t.phase) +
        (clearedShip == t.id ? ", cleared" : dockingShips.Contains(t.id) ? ", cleared (on its last leg)" : "") + "\n";
    String f = ShipFlags(t);
    text += "Flags: " + (f == "" ? "-" : f) + "\n";
    text += "Last contact: " + Math.Round((TimeNow() - t.time).TotalSeconds, 1) + " s\n";
    return text;
}

// ---- text screens drawn in colour (main, menus, inventory, fleet, ship) ----
Dictionary<IMyTextSurface, String> screenCache = new Dictionary<IMyTextSurface, String>();

// connectors of the station (pool + ships' homes): state, ship, lane
String BuildDocksScreen()
{
    String text = "Connectors" + (dockAreaStatus != "" ? " | " + dockAreaStatus : "") + "\n";
    List<IMyShipConnector> list = DockList();
    if (list.Count == 0) return text + "(no [PAM-Dock] connectors, no ship asked yet)\n";
    foreach (IMyShipConnector c in list)
    {
        bool cam = laneCameras.Exists(q => Vector3D.Distance(q.GetPosition(), c.GetPosition()) <= 10);
        text += c.CustomName + (homeConns.Contains(c.EntityId) ? " (home)" : "") + ": " + DockSt(c) + (cam ? "" : " | no lane camera") + "\n";
    }
    return text;
}

String GridName(IMyCubeGrid g)
{
    TCShip t = tcShips.Find(q => q.id == g.EntityId) ?? tcShips.Find(q => Vector3D.Distance(q.pos, g.GetPosition()) < q.dia); // ids differ: by position
    return t != null ? TCName(t) : g.CustomName;
}

// colour of a text line by what it says (default: the LCD's font colour)
Color LineColor(String l, LCDMode mode, Color def)
{
    String t = l.Trim();
    if (mode == LCDMode.Docks)
    {
        if (l.Contains(": BLOCKED")) return Color.Red;
        if (l.Contains(": DAMAGED") || l.Contains(": OFF")) return Color.Coral;
        if (l.Contains(": docked")) return Color.Gray;
        if (l.Contains(": cleared") || l.Contains(": docking") || l.Contains(": ready")) return Color.SkyBlue;
        if (l.Contains(": free")) return Color.LimeGreen;
    }
    if (t.StartsWith("—") || t.StartsWith("---")) return Color.Gray;
    if (mode == LCDMode.Fleet && t.StartsWith("Ship ")) return Color.Gray; // table header
    if (l.Contains("EMERGENCY") || l.Contains(" EMG") || l.Contains("no answer") || l.Contains("NO CONTACT") || l.Contains("No signal") || l.Contains("Different version")) return Color.Red;
    if (t.StartsWith("Battery"))
    {
        int b = -1;
        int.TryParse(new String(t.Substring(7).Trim().TakeWhile(char.IsDigit).ToArray()), out b);
        return b < 0 ? def : b >= 50 ? Color.LimeGreen : b >= 20 ? Color.Yellow : Color.Red;
    }
    if (l.Contains(" GIVE") || l.Contains("giving way") || l.Contains("pending") || t.StartsWith("Traffic:")) return Color.Yellow;
    if (l.Contains(" WAIT") || l.Contains("waiting to leave")) return Color.Violet;
    if (l.Contains("PARKED")) return Color.Gray;
    if (l.Contains("CLEARED") || l.Contains("received") || l.Contains("Job active")) return Color.LimeGreen;
    if (l.Contains("QUEUE") || t.StartsWith("Dock")) return Color.SkyBlue;
    if (t.StartsWith("Job")) return Color.Orange;
    if (t.StartsWith("[") || t.Contains("[|")) return new Color(200, 160, 80); // cargo / inventory bars
    return def;
}

// a text screen as coloured lines; the LCD's font, size, padding and colours; the selected menu line
// highlighted; redrawn only when something changed
void DrawTextScreen(IMyTextSurface p, String text, LCDMode mode, int lines)
{
    String font = mode == LCDMode.Fleet && !lcdFontSet ? "Monospace" : p.Font; // table columns (unless font= is set)
    String key = text + p.FontSize + font + p.FontColor + p.BackgroundColor + p.TextPadding, old;
    if (screenCache.TryGetValue(p, out old) && old == key && p.ContentType == ContentType.SCRIPT) return;
    screenCache[p] = key;
    if (p.ContentType != ContentType.SCRIPT) { p.ContentType = ContentType.SCRIPT; p.Script = ""; }
    p.ScriptBackgroundColor = p.BackgroundColor;
    Vector2 size = p.SurfaceSize, at = (p.TextureSize - size) / 2;
    float sc = Math.Max(0.1f, p.FontSize), pad = size.Y * p.TextPadding / 100f;
    float lh = Math.Min(p.MeasureStringInPixels(new StringBuilder("Ag"), font, sc).Y, (size.Y - 2 * pad) / Math.Max(1, lines));
    drawFrame = p.DrawFrame();
    String[] ls = text.Split('\n');
    float y = at.Y + pad;
    for (int i = 0; i < ls.Length && y + lh <= at.Y + size.Y - pad + 1; i++, y += lh)
    {
        if (ls[i].StartsWith(">")) DBox(new Vector2(at.X + size.X / 2, y + lh / 2), new Vector2(size.X, lh), new Color(25, 45, 80), "SquareSimple");
        DText(ls[i], new Vector2(at.X + pad, y), LineColor(ls[i], mode, p.FontColor), sc, TextAlignment.LEFT, font);
    }
    drawFrame.Dispose();
}

// colour of a log event by its kind
Color LogColor(String msg)
{
    if (msg.StartsWith("EMERGENCY")) return Color.Red;
    if (msg.Contains("couldn't lock") || msg.Contains("too slow")) return Color.Coral;
    if (msg.StartsWith("Dock:")) return Color.SkyBlue;
    if (msg.StartsWith("Job:")) return Color.Orange;
    if (msg.StartsWith("Traffic:")) return Color.Yellow;
    if (msg.StartsWith("Move home:")) return Color.Violet;
    if (msg.StartsWith("Contact lost")) return Color.IndianRed;
    if (msg.StartsWith("Contact")) return Color.LimeGreen;
    return Color.White;
}

// log: time in grey, the event in its colour, long events wrapped; text size = the LCD's font size
void DrawLog(IMyTextPanel p, Vector2 at, Vector2 size)
{
    float sc = Math.Max(0.3f, p.FontSize), pad = 6, bottom = at.Y + size.Y;
    StringBuilder sb = new StringBuilder();
    String fn = p.Font;
    Func<String, float> W = s => p.MeasureStringInPixels(sb.Clear().Append(s), fn, sc).X;
    float lh = p.MeasureStringInPixels(sb.Clear().Append("Ag"), fn, sc).Y, y = at.Y + pad;
    DText("Coordination log (newest first)", new Vector2(at.X + pad, y), Color.White, sc, TextAlignment.LEFT, fn);
    y += lh * 1.3f;
    if (logLines.Count == 0) DText("(nothing yet)", new Vector2(at.X + pad, y), Color.Gray, sc, TextAlignment.LEFT, fn);
    for (int i = 0; i < logLines.Count && y + lh <= bottom; i++)
    {
        String time = logLines[i].Substring(0, 9), msg = logLines[i].Substring(9), line = "";
        Color c = LogColor(msg);
        DText(time, new Vector2(at.X + pad, y), Color.Gray, sc, TextAlignment.LEFT, fn);
        float x = at.X + pad + W(time), width = at.X + size.X - pad - x;
        foreach (String w in msg.Split(' '))
        {
            String t = line == "" ? w : line + " " + w;
            if (line == "" || W(t) <= width) { line = t; continue; }
            DText(line, new Vector2(x, y), c, sc, TextAlignment.LEFT, fn);
            y += lh;
            line = w;
            if (y + lh > bottom) { line = ""; break; }
        }
        if (line != "") DText(line, new Vector2(x, y), c, sc, TextAlignment.LEFT, fn);
        y += lh;
    }
}

// ---- drawing ----
MySpriteDrawFrame drawFrame;

void DLine(Vector2 a, Vector2 b, Color c, float w)
{
    Vector2 d = b - a;
    float len = d.Length();
    if (len < 0.5f) return;
    drawFrame.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", (a + b) / 2, new Vector2(len, w), c, null, TextAlignment.CENTER, (float)Math.Atan2(d.Y, d.X)));
}

void DBox(Vector2 center, Vector2 size, Color c, String shape) { drawFrame.Add(new MySprite(SpriteType.TEXTURE, shape, center, size, c)); }

// text sprite; without a font given (map, job): the LCD's font, sizes scaled by its font size
float drawScale = 1;
String drawFont = "Debug";
void DText(String s, Vector2 at, Color c, float scale, TextAlignment align, String font = null)
{
    MySprite t = MySprite.CreateText(s, font ?? drawFont, c, font == null ? scale * drawScale : scale, align);
    t.Position = at;
    drawFrame.Add(t);
}

void DrawSpriteScreen(IMyTextPanel p, LCDMode mode, String param)
{
    // log and job: only when something changed (the drawing stays on the LCD until the next one)
    String key = null;
    if (mode == LCDMode.Log) key = "L" + logLines.Count + (logLines.Count > 0 ? logLines[0] : "");
    if (mode == LCDMode.Job)
    {
        int n = 1;
        int.TryParse(param, out n);
        JobRec jr = n >= 1 && n <= jobs.Count ? jobs[n - 1] : null;
        key = "J" + param + (jr == null ? "" : jr.done + jr.ships.Count + String.Join(",", jr.ships.Select(q => q.cell)));
    }
    if (key != null)
    {
        key += p.FontSize + p.Font + p.BackgroundColor + p.SurfaceSize;
        String old;
        if (screenCache.TryGetValue(p, out old) && old == key && p.ContentType == ContentType.SCRIPT) return;
        screenCache[p] = key;
    }
    if (p.ContentType != ContentType.SCRIPT) { p.ContentType = ContentType.SCRIPT; p.Script = ""; }
    p.ScriptBackgroundColor = new Color(0, 0, 0);
    Vector2 size = p.SurfaceSize, at = (p.TextureSize - size) / 2;
    drawScale = p.FontSize; drawFont = p.Font;
    drawFrame = p.DrawFrame();
    if (mode == LCDMode.Job) DrawJobMatrix(param, at, size);
    else if (mode == LCDMode.Log) DrawLog(p, at, size);
    else DrawMap(param, at, size);
    drawFrame.Dispose();
}

// hole matrix: columns left to right, rows top to bottom, as seen from behind a ship drilling it
void DrawJobMatrix(String param, Vector2 at, Vector2 size)
{
    int n = 1;
    int.TryParse(param, out n);
    if (n < 1 || n > jobs.Count) { DText(jobs.Count == 0 ? "No job" : "Job " + param + ": no such job (" + jobs.Count + ")", at + size / 2, Color.White, 1, TextAlignment.CENTER); return; }
    JobRec job = jobs[n - 1];
    int done = 0;
    for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') done++;
    DText("Job " + n + ": " + done + "/" + job.done.Length + " holes done", at + new Vector2(size.X / 2, 4), Color.White, 0.9f, TextAlignment.CENTER);
    int keyLines = (job.ships.Count + 1) / 2; // key under the grid: 2 ships per line
    float top = 40, bottom = 30 + 20 * keyLines;
    float cell = Math.Min(size.X / job.w, (size.Y - top - bottom) / job.h);
    Vector2 origin = at + new Vector2((size.X - cell * job.w) / 2, top + (size.Y - top - bottom - cell * job.h) / 2);
    for (int c = 0; c < job.done.Length; c++)
    {
        Vector2 ctr = origin + new Vector2((c % job.w + 0.5f) * cell, (c / job.w + 0.5f) * cell);
        JobShip owner = HoleOwner(job, c, null);
        Color col = job.done[c] == '1' ? new Color(90, 90, 90) : owner != null ? Color.Orange : new Color(30, 60, 90);
        DBox(ctr, new Vector2(cell * 0.9f, cell * 0.9f), col, "SquareSimple");
        if (owner == null) continue;
        DText("" + (job.ships.IndexOf(owner) + 1), ctr - new Vector2(0, cell * 0.3f), Color.Black, Math.Min(1.2f, cell / 40), TextAlignment.CENTER);
    }
    // key: the number in a square = the ship working it
    for (int i = 0; i < job.ships.Count; i += 2)
        DText((i + 1) + " " + ShipName(job.ships[i].id) + (i + 1 < job.ships.Count ? "    " + (i + 2) + " " + ShipName(job.ships[i + 1].id) : ""),
            at + new Vector2(size.X / 2, size.Y - 26 - 20 * (keyLines - i / 2)), Color.Orange, 0.55f, TextAlignment.CENTER);
    DText("grey done | orange: ship working (number) | blue free", at + new Vector2(size.X / 2, size.Y - 24), Color.Gray, 0.6f, TextAlignment.CENTER);
}

// ---- maps (mode=map:<view>) ----
//   map / map:side  side view station -> job: real heights, labelled in metres (stretched at most 5x)
//   map:top         top view, the way to the first job pointing up, scale bar; heights as labels
//   map:dock        the docking area zoomed in: connectors (numbered, coloured by state), their lanes,
//                   [PAM-Zone] boxes, blocked lanes, ships near the docks, a line from a cleared ship to its connector
//   map:all         all three on one screen
//   map:<view>:<C>x<R>:<n>  one tile of a wall of C x R LCDs (n = 1.. left to right, top to bottom) that
//                   together show the map in C x R times the pixels (a 3x3 / 5x5 LCD has only 512 x 512)
// Markers and text scale with the view (its smaller side / 512 px) times the LCD's font size.
Vector3 mO, mUp, mAlong, mRight;
Vector2 mAt, mSize;
float mK = 1, mFont = 1;
List<List<Vector3>> mPaths = new List<List<Vector3>>();
String mPathKey = "";

void DrawMap(String view, Vector2 at, Vector2 size)
{
    mFont = drawScale;
    // a tile of an LCD wall: draw the whole map on a surface C x R times as big, shifted to this tile
    String[] vs = view.Split(':');
    view = vs[0];
    if (vs.Length > 2)
    {
        String[] g = vs[1].Split('X');
        int c = 1, r = 1, n = 1;
        int.TryParse(g[0], out c);
        if (g.Length > 1) int.TryParse(g[1], out r);
        int.TryParse(vs[2], out n);
        c = Math.Max(1, c); r = Math.Max(1, r); n = Math.Max(1, Math.Min(n, c * r)) - 1;
        at -= new Vector2(n % c * size.X, n / c * size.Y);
        size *= new Vector2(c, r);
    }
    drawFrameSize = size;
    // frame of all views: station (the middle of its connectors, else this PB), up (planet: away from its
    // centre), the way to the first job
    List<IMyShipConnector> docks = DockList();
    mO = Me.GetPosition();
    if (docks.Count > 0) { mO = new Vector3(); foreach (IMyShipConnector d in docks) mO += d.GetPosition() / docks.Count; }
    mUp = new Vector3();
    if (hasPlanet) mUp = mO - planetCenter;
    else foreach (TCShip t in tcShips) mUp += t.up;
    if (mUp.LengthSquared() < 0.01f) mUp = Me.WorldMatrix.Up;
    mUp.Normalize();
    // every recorded path once (ships sharing a path: drawn once), parsed again only when one changed
    List<String> ps = shipPaths.Values.Distinct().ToList();
    String key = String.Join(";", ps);
    if (key != mPathKey)
    {
        mPathKey = key; mPaths.Clear();
        foreach (String p in ps) { List<Vector3> l = new List<Vector3>(); foreach (String w in p.Split(':')) if (w != "") l.Add(StringToVector(w)); mPaths.Add(l); }
    }
    // towards the first job (else the path end farthest from the station, else the first ship)
    Vector3 far = jobs.Count > 0 ? JobPos(jobs[0]) : mO;
    if (far == mO) foreach (List<Vector3> l in mPaths) foreach (Vector3 v in l) if (Vector3.Distance(v, mO) > Vector3.Distance(far, mO)) far = v;
    if (far == mO && tcShips.Count > 0) far = tcShips[0].pos;
    mAlong = far - mO - mUp * Vector3.Dot(far - mO, mUp);
    if (mAlong.LengthSquared() < 1) mAlong = Vector3.Cross(mUp, Me.WorldMatrix.Right);
    mAlong.Normalize();
    mRight = Vector3.Cross(mAlong, mUp);

    if (view == "ALL")
    {
        // wide: top view left, docks and side view right; square (3x3, 5x5): top view and docks above, side view below
        float w = size.X, h = size.Y * 0.93f;
        bool wide = w > h * 1.4f;
        Vector2 a = wide ? new Vector2(w / 2, h) : new Vector2(w * 0.62f, h * 0.66f);
        MapView("TOP", at, a);
        MapView("DOCK", at + new Vector2(a.X, 0), wide ? new Vector2(w / 2, h / 2) : new Vector2(w - a.X, a.Y));
        MapView("SIDE", at + (wide ? new Vector2(w / 2, h / 2) : new Vector2(0, a.Y)), wide ? new Vector2(w / 2, h / 2) : new Vector2(w, h - a.Y));
        mK = Math.Min(size.X, size.Y) / 1024; drawScale = mFont * mK;
        MapKey(at + new Vector2(size.X / 2, h + 22 * mK), true);
    }
    else MapView(view == "TOP" || view == "DOCK" ? view : "SIDE", at, size);
}

void MapView(String view, Vector2 at, Vector2 size)
{
    mAt = at; mSize = size;
    mK = Math.Min(size.X, size.Y) / 512; drawScale = mFont * mK;
    bool all = size != drawFrameSize;
    if (all) // frame around each view of map:all
    {
        Vector2 b = at + size;
        Color f = new Color(60, 60, 60);
        DLine(at, new Vector2(b.X, at.Y), f, 2); DLine(new Vector2(b.X, at.Y), b, f, 2); DLine(b, new Vector2(at.X, b.Y), f, 2); DLine(new Vector2(at.X, b.Y), at, f, 2);
    }
    if (view == "DOCK") DrawDockMap(at, size);
    else if (view == "TOP") DrawTopMap(at, size);
    else DrawSideMap(at, size);
    if (!all) MapKey(at + new Vector2(size.X / 2, size.Y - 22 * mK), view == "DOCK");
}

Vector2 drawFrameSize;

// colour key at the bottom of a view
void MapKey(Vector2 at, bool docks)
{
    if (docks) at.Y -= 18 * mK;
    DText("green moving | yellow giving way | violet waiting | grey parked | red emergency", at, Color.Gray, 0.5f, TextAlignment.CENTER);
    if (docks) DText("docks: green free | grey docked | sky blue cleared, docking | red lane blocked | coral damaged", at + new Vector2(0, 18 * mK), Color.Gray, 0.5f, TextAlignment.CENTER);
}

float Ht(Vector3 v) { return hasPlanet ? Vector3.Distance(v, planetCenter) - Vector3.Distance(mO, planetCenter) : Vector3.Dot(v - mO, mUp); }
String HtS(Vector3 v) { float h = Ht(v); return (h >= 0 ? "+" : "") + Math.Round(h) + " m"; }

Color ShipCol(TCShip t) { return t.emg ? Color.Red : t.limit >= 0 ? Color.Yellow : t.prio < 0 ? Color.Violet : TCParked(t) ? Color.Gray : Color.Lime; }

// the connector a ship is cleared to / docking at (null: none)
IMyShipConnector ShipDock(long id)
{
    int d = dockingShips.IndexOf(id);
    return StationConnector(id == clearedShip && clearedKind == 1 ? clearedConn : d >= 0 ? dockingConns[d] : 0);
}

// a ship: arrow the way it flies (or wants to), a line to its connector when cleared (else where it is
// in 5 s), its name and height
void DShip(TCShip t, Func<Vector3, Vector2> P)
{
    if (Heavy()) return;
    Vector2 q = P(t.pos), d = P(t.pos + (t.vel.LengthSquared() > 0.25f ? t.vel : t.heading)) - q;
    if (q.X < mAt.X || q.Y < mAt.Y || q.X > mAt.X + mSize.X || q.Y > mAt.Y + mSize.Y) return;
    Color c = ShipCol(t);
    IMyShipConnector sc = ShipDock(t.id);
    if (sc != null) DLine(q, P(sc.GetPosition()), Color.SkyBlue, 2 * mK);
    else if (t.vel.LengthSquared() > 0.25f) DLine(q, P(t.pos + t.vel * 5), c, 2 * mK);
    if (d.LengthSquared() > 0.01f) drawFrame.Add(new MySprite(SpriteType.TEXTURE, "Triangle", q, new Vector2(14, 16) * mK, c, null, TextAlignment.CENTER, (float)Math.Atan2(d.X, -d.Y)));
    else DBox(q, new Vector2(12, 12) * mK, c, "Circle");
    if (!TCParked(t)) DText(TCName(t) + " " + HtS(t.pos), q - new Vector2(0, 28 * mK), c, 0.55f, TextAlignment.CENTER); // parked: named on the dock map's list
}

// recorded paths (fewer segments when they'd be shorter than ~3 px)
void DPaths(Func<Vector3, Vector2> P)
{
    foreach (List<Vector3> l in mPaths)
    {
        if (Heavy()) return;
        Vector2 last = new Vector2();
        for (int i = 0; i < l.Count; i++)
        {
            Vector2 q = P(l[i]);
            if (i > 0 && (q - last).Length() < 3 && i < l.Count - 1) continue;
            if (i > 0) DLine(last, q, Color.SkyBlue, 2 * mK);
            last = q;
        }
    }
}

// near the instruction limit (60 %): drawing stops for this run instead of the script being stopped
bool Heavy() { return Runtime.CurrentInstructionCount > Runtime.MaxInstructionCount * 0.6; }

// the station's connectors (pool + ships' homes)
List<IMyShipConnector> DockList()
{
    List<IMyShipConnector> list = new List<IMyShipConnector>(stationDocks);
    foreach (long h in homeConns) { IMyShipConnector hc = StationConnector(h); if (hc != null && !list.Contains(hc)) list.Add(hc); }
    return list;
}

String DockSt(IMyShipConnector c)
{
    LaneBlock b = LaneBlockOf(c.EntityId);
    int d = dockingConns.IndexOf(c.EntityId);
    return b != null ? "BLOCKED: " + b.what + " in its lane" : !c.IsFunctional ? "DAMAGED" : !c.Enabled ? "OFF" :
        c.Status == MyShipConnectorStatus.Connected ? "docked: " + GridName(c.OtherConnector.CubeGrid) :
        c.Status == MyShipConnectorStatus.Connectable ? "ready to lock" :
        clearedShip != 0 && clearedKind == 1 && clearedConn == c.EntityId ? "cleared: " + ShipName(clearedShip) :
        d >= 0 ? "docking: " + ShipName(dockingShips[d]) : "free";
}

Color DockCol(IMyShipConnector c) { return LineColor(": " + DockSt(c), LCDMode.Docks, Color.White); }

// grid lines / scale bar step: 1, 2, 5 x 10^n m, about n of them over range metres
float MapStep(float range, int n) { float s = 1; while (range / s > n) s *= s.ToString().StartsWith("2") ? 2.5f : 2; return s; }

// side view: horizontal = distance from the station towards the job, vertical = real height above the
// station (on a planet: distance from its centre, so the curve of the planet doesn't tilt it)
void DrawSideMap(Vector2 at, Vector2 size)
{
    float x0 = 0, x1 = 20, y0 = 0, y1 = 10;
    List<Vector3> all = new List<Vector3>();
    foreach (List<Vector3> l in mPaths) all.AddRange(l);
    foreach (JobRec j in jobs) all.Add(JobPos(j));
    foreach (TCShip t in tcShips) all.Add(t.pos);
    foreach (Vector3 v in all)
    {
        float x = Vector3.Dot(v - mO, mAlong), y = Ht(v);
        x0 = Math.Min(x0, x); x1 = Math.Max(x1, x); y0 = Math.Min(y0, y); y1 = Math.Max(y1, y);
    }
    float mx = (x1 - x0) * 0.06f + 5, my = (y1 - y0) * 0.15f + 5;
    x0 -= mx; x1 += mx; y0 -= my; y1 += my;
    float top = 30 * mK, bottom = 40 * mK, left = 52 * mK;
    Vector2 area = new Vector2(size.X - left, size.Y - top - bottom);
    float sx = area.X / (x1 - x0), sy = Math.Min(area.Y / (y1 - y0), sx * 5), yMid = (y0 + y1) / 2;
    Func<float, float, Vector2> P2 = (x, y) => at + new Vector2(left + (x - x0) * sx, top + area.Y / 2 - (y - yMid) * sy);
    Func<Vector3, Vector2> P = v => P2(Vector3.Dot(v - mO, mAlong), Ht(v));

    DText("Side view: station -> job | heights x" + Math.Round(sy / sx, 1), at + new Vector2(size.X / 2, 4 * mK), Color.White, 0.7f, TextAlignment.CENTER);
    // height lines every 1/2/5 x 10^n m, labelled (height above the station)
    float range = area.Y / sy, step = MapStep(range, 6);
    for (float h = (float)Math.Ceiling((yMid - range / 2) / step) * step; h <= yMid + range / 2; h += step)
    {
        Vector2 a = P2(x0, h);
        if (a.Y < at.Y + top || a.Y > at.Y + size.Y - bottom) continue;
        DLine(a, P2(x1, h), h == 0 ? new Color(70, 70, 70) : new Color(35, 35, 35), mK);
        DText((h > 0 ? "+" : "") + h + " m", new Vector2(at.X + 2, a.Y - 9 * mK), Color.Gray, 0.5f, TextAlignment.LEFT);
    }
    DPaths(P);
    // station: its connectors coloured by state (above each other at their heights)
    DText("Station", P(mO) + new Vector2(0, 10 * mK), Color.White, 0.6f, TextAlignment.CENTER);
    List<IMyShipConnector> docks = DockList();
    if (docks.Count == 0) DBox(P(mO), new Vector2(14, 14) * mK, Color.White, "SquareSimple");
    foreach (IMyShipConnector c in docks) DBox(P(c.GetPosition()), new Vector2(8, 8) * mK, DockCol(c), "SquareSimple");
    for (int j = 0; j < jobs.Count; j++)
    {
        Vector3 jp = JobPos(jobs[j]);
        float half = jobs[j].w * jobs[j].tw / 2;
        DLine(P(jp) - new Vector2(half * sx, 0), P(jp) + new Vector2(half * sx, 0), Color.Orange, 4 * mK);
        DText(JobLabel(j) + " " + HtS(jp), P(jp) + new Vector2(0, 6 * mK), Color.Orange, 0.6f, TextAlignment.CENTER);
    }
    foreach (TCShip t in tcShips) DShip(t, P);
}

String JobLabel(int j)
{
    int n = 0;
    foreach (char c in jobs[j].done) if (c == '1') n++;
    return "Job " + (j + 1) + " " + n + "/" + jobs[j].done.Length;
}

// a top-down view of the points (equal scale both ways, the way to the job up); returns the projection
Func<Vector3, Vector2> TopFrame(List<Vector3> pts, Vector2 at, Vector2 size, float top, float bottom, float minRange, out float s)
{
    float x0 = 0, x1 = 0, y0 = 0, y1 = 0;
    foreach (Vector3 v in pts)
    {
        float x = Vector3.Dot(v - mO, mRight), y = Vector3.Dot(v - mO, mAlong);
        x0 = Math.Min(x0, x); x1 = Math.Max(x1, x); y0 = Math.Min(y0, y); y1 = Math.Max(y1, y);
    }
    float m = Math.Max(Math.Max(x1 - x0, y1 - y0) * 0.08f, minRange), cx = (x0 + x1) / 2, cy = (y0 + y1) / 2;
    Vector2 area = new Vector2(size.X, size.Y - top - bottom), ctr = at + new Vector2(size.X / 2, top + area.Y / 2);
    float sc = Math.Min(area.X / (x1 - x0 + 2 * m), area.Y / (y1 - y0 + 2 * m));
    s = sc;
    return v => ctr + new Vector2(Vector3.Dot(v - mO, mRight) - cx, cy - Vector3.Dot(v - mO, mAlong)) * sc;
}

// scale bar, its right end at b
void ScaleBar(Vector2 b, float width, float s)
{
    float st = MapStep(width / 4 / s, 1);
    Vector2 a = b - new Vector2(st * s, 0);
    DLine(a, b, Color.Gray, 2 * mK);
    DText(st >= 1000 ? st / 1000 + " km" : st + " m", a - new Vector2(4 * mK, 9 * mK), Color.Gray, 0.5f, TextAlignment.RIGHT);
}

// top view: path, job sites (their size, with progress and height), station connectors, ships
void DrawTopMap(Vector2 at, Vector2 size)
{
    List<Vector3> pts = new List<Vector3>();
    foreach (List<Vector3> l in mPaths) pts.AddRange(l);
    foreach (JobRec j in jobs) pts.Add(JobPos(j));
    foreach (TCShip t in tcShips) pts.Add(t.pos);
    float s;
    Func<Vector3, Vector2> P = TopFrame(pts, at, size, 30 * mK, 60 * mK, 10, out s);
    DText("Top view: station -> job (up)", at + new Vector2(10 * mK, 4 * mK), Color.White, 0.7f, TextAlignment.LEFT);
    ScaleBar(at + size - new Vector2(10, 46) * mK, size.X, s);
    DPaths(P);
    for (int j = 0; j < jobs.Count; j++)
    {
        Vector2 q = P(JobPos(jobs[j]));
        DBox(q, Vector2.Max(new Vector2(jobs[j].w * jobs[j].tw, jobs[j].h * jobs[j].th) * s, new Vector2(10, 10) * mK), Color.Orange, "SquareHollow");
        DText(JobLabel(j) + " " + HtS(JobPos(jobs[j])), q + new Vector2(0, 10 * mK), Color.Orange, 0.6f, TextAlignment.CENTER);
    }
    List<IMyShipConnector> docks = DockList();
    DBox(P(mO), new Vector2(16, 16) * mK, Color.White, "SquareHollow");
    foreach (IMyShipConnector c in docks) DBox(P(c.GetPosition()), new Vector2(6, 6) * mK, DockCol(c), "SquareSimple");
    DText("Station", P(mO) + new Vector2(0, 10 * mK), Color.White, 0.6f, TextAlignment.CENTER);
    foreach (TCShip t in tcShips) DShip(t, P);
}

// docking area zoomed in: [PAM-Zone] boxes, connectors with their lanes (dockLaneLength), blocked
// lanes from the camera to what is in them, ships near the docks or in the dock queue
void DrawDockMap(Vector2 at, Vector2 size)
{
    List<IMyShipConnector> docks = DockList();
    DText("Dock area" + (dockAreaStatus != "" ? ": " + dockAreaStatus : ""), at + new Vector2(10 * mK, 4 * mK), Color.White, 0.6f, TextAlignment.LEFT);
    if (docks.Count == 0) { DText("No [PAM-Dock] connectors, no ship asked yet", at + size / 2, Color.Gray, 0.6f, TextAlignment.CENTER); return; }
    List<Vector3> pts = new List<Vector3>(), box = new List<Vector3>();
    foreach (IMyShipConnector c in docks) { pts.Add(c.GetPosition()); pts.Add(c.GetPosition() + c.WorldMatrix.Forward * dockLaneLength); }
    foreach (IMySensorBlock z in zoneSensors)
    {
        MatrixD m = z.WorldMatrix;
        for (int i = 0; i < 8; i++)
            box.Add(m.Translation + (i % 2 == 0 ? m.Left * z.LeftExtend : m.Right * z.RightExtend) + (i / 2 % 2 == 0 ? m.Up * z.TopExtend : m.Down * z.BottomExtend) +
                (i / 4 == 0 ? m.Forward * z.FrontExtend : m.Backward * z.BackExtend));
    }
    pts.AddRange(box);
    // ships in the queue, cleared or docking, or within the area so far
    float r = 0;
    foreach (Vector3 v in pts) r = Math.Max(r, Vector3.Distance(v, mO));
    foreach (TCShip t in tcShips)
        if (t.flag == 1 || ShipDock(t.id) != null || t.id == clearedShip || Vector3.Distance(t.pos, mO) < r * 1.5f) pts.Add(t.pos);
    int cols = docks.Count > 6 ? 2 : 1, lines = (docks.Count + cols - 1) / cols;
    float s, bottom = (58 + 16 * lines) * mK;
    Func<Vector3, Vector2> P = TopFrame(pts, at, size, 30 * mK, bottom, 5, out s);
    ScaleBar(at + new Vector2(size.X - 10 * mK, size.Y - bottom), size.X, s);
    // zone boxes (all 12 edges, seen from above)
    for (int k = 0; k < box.Count; k += 8)
        for (int i = 0; i < 8; i++)
            for (int b = 1; b < 8; b <<= 1)
                if ((i & b) == 0) DLine(P(box[k + i]), P(box[k + (i | b)]), Color.Violet, mK);
    foreach (TCShip t in tcShips) DShip(t, P);
    for (int i = 0; i < docks.Count; i++)
    {
        IMyShipConnector c = docks[i];
        Color col = DockCol(c);
        Vector2 q = P(c.GetPosition());
        DLine(q, P(c.GetPosition() + c.WorldMatrix.Forward * dockLaneLength), new Color(col.R / 2, col.G / 2, col.B / 2), 6 * mK);
        DBox(q, new Vector2(12, 12) * mK, col, "SquareSimple");
        DText("" + (i + 1), q + new Vector2(0, 6 * mK), col, 0.5f, TextAlignment.CENTER);
        LaneBlock lb = LaneBlockOf(c.EntityId);
        if (lb != null) { DLine(P(lb.cam), P(lb.hit), Color.Red, 2 * mK); DText(lb.what, P(lb.hit), Color.Red, 0.5f, TextAlignment.LEFT); }
        // key under the map: number, name, state
        String nm = c.CustomName.Replace(dockPoolTag, "").Trim();
        DText((i + 1) + " " + (nm.Length > 20 ? nm.Substring(0, 20) : nm) + ": " + DockSt(c),
            at + new Vector2(10 * mK + i / lines * size.X / 2, size.Y - bottom + (10 + i % lines * 16) * mK), col, 0.5f, TextAlignment.LEFT);
    }
}

Vector3 JobPos(JobRec job) { String[] k = job.key.Split('|'); return new Vector3(float.Parse(k[0]), float.Parse(k[1]), float.Parse(k[2])); }
