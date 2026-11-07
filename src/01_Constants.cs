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
// The settings are in this PB's Custom Data, in the [PAM settings] section (written with the
// defaults on the first run; each script lists only the settings it uses). Edit them there, then
// press Recompile. Delete a line to get its default back. They are kept when you paste a new
// version of the script. Descriptions: README, section Settings.

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
bool resumeAfterRestart = true;
const bool checkConveyorSystem = false; //temporarily disabled because of a SE bug
bool debugDraw = false;
