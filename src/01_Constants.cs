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

// ===== Settings: edit the values below =====
// (each script only lists the settings it uses; details in the README)

const String pamTag = "[PAM]"; //LCDs of cockpits and other blocks: [PAM:<lcdIndex>] e.g. [PAM:1]
const String controllerTag = "[PAM-Controller]";
const String dataTag = "[PAM-Data]"; //EXPORT/IMPORT use the Custom Data of the block with this tag
const String dockPoolTag = "[PAM-Dock]"; //base connectors ships may use when their own home dock is taken

//Station dock control ([PAM-Controller] PB at the station): one ship at a time in the docking area
const String zoneTag = "[PAM-Zone]"; //sensor over the docking area: nobody cleared while another ship moves in it
const String laneTag = "[PAM-Lane]"; //camera at a connector, facing out along its approach: lane must be clear
const float dockLaneLength = 40f;//[m] how far the lane cameras check
const float dockRequestDistance = 300f;//[m] ships ask for clearance this close to home, then wait outside the zone

const int gyroSpeedSmall = 15; //[RPM] small ship
const int gyroSpeedLarge = 5; //[RPM] large ship
const int generalSpeedLimit = 100; //[m/s] 0 = no limit (if set to 0 ignore unreachable code warning)
const float dockingSpeed = 0.5f; //[m/s]
const float jobAreaSpeed = 10f;//[m/s] near the job area (between holes, backing out, arriving, leaving)

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
const float followPathDock = 2f; //stop following path, fly direct to target
const float followPathJob = 1f; //same as dock
const float useDockDirectionDist = 1f; //Override waypoint direction, use docking dir
const float useJobDirectionDist = 0f; //same as dock

//other distances
const float wpReachedDist = 2f;//[m]
const float drillRadius = 1.4f;//[m] grinders and modded drills (vanilla drills use their real cut radius)

//grinding
const float sensorRange = 2f;//fly slow when blocks found in this range
const float fastSpeed = 10f;//speed when no blocks are detected

//minimum acceleration in space before ship becomes too heavy
const float minAccelerationSmall = 0.2f;//[m/s²] small ship
const float minAccelerationLarge = 0.1f;//[m/s²] large ship

//stone ejection
const float minEjection = 25f;//[%] Min amount of ejected cargo to continue job

//LCD
const bool setLCDFontAndSize = true;

//Obstacle brake: the ship's cameras scan where it flies and stop it when something is in the way.
//A camera scans within 45° of where it faces: add cameras facing every direction the ship travels.
const bool obstacleBrake = true;
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size
const float obstacleTimeout = 10f;//[s] show "Path blocked" after holding this long

//Traffic between PAM ships (needs an antenna): lanes, spacing, right of way, co-op holes (IMPORT JOB)
const bool trafficEnabled = true;
const float laneOffset = 0.75f;//x ship size: keep right of the path; 0 = fly on the path
const float followGap = 1f;//x ship size: free space behind a PAM ship ahead

const bool resumeAfterRestart = true;//continue a running job after a server restart / world load

//Check if blocks are connected with conveyors
const bool checkConveyorSystem = false;//temporarily disabled because of a SE bug

//3D debug drawing (offline world + Digi's "(DevTool) Programmable Block DebugAPI" mod), see README
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
