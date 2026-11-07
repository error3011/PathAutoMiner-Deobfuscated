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

const String pamTag = "[PAM]";
const String controllerTag = "[PAM-Controller]";
const String dataTag = "[PAM-Data]"; //EXPORT/IMPORT use the Custom Data of the block with this tag
const String dockPoolTag = "[PAM-Dock]"; //base connectors in the dock pool (used when the ship's own home dock is taken)

//Station dock control (on the [PAM-Controller] PB at the station): one ship at a time is cleared to
//move in the docking area. Ships wait at the end of their path until cleared.
const String zoneTag = "[PAM-Zone]"; //sensor(s) covering the docking area: no clearance while another ship moves inside
const String laneTag = "[PAM-Lane]"; //camera next to a connector, facing out along its approach: lane must be clear
const float dockLaneLength = 40f;//[m] how far the lane cameras check in front of their connector
const float dockRequestDistance = 300f;//[m] ships ask the station for clearance this close to their home dock, and wait outside the [PAM-Zone] sensor area
//Tag for LCD's of cockpits and other blocks: [PAM:<lcdIndex>] e.g: [PAM:1]

const int gyroSpeedSmall = 15; //[RPM] small ship
const int gyroSpeedLarge = 5; //[RPM] large ship
const int generalSpeedLimit = 100; //[m/s] 0 = no limit (if set to 0 ignore unreachable code warning)
const float dockingSpeed = 0.5f; //[m/s]

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
const float followPathDock = 2f; //stop following path, fly direct to target
const float followPathJob = 1f; //same as dock
const float useDockDirectionDist = 1f; //Override waypoint direction, use docking dir
const float useJobDirectionDist = 0f; //same as dock

//other distances
const float wpReachedDist = 2f;//[m]
const float drillRadius = 1.4f;//[m] grinders and modded drills; vanilla drills use their real cut radius (small 1.3, large 1.9, prototech 6)

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

//Obstacle brake: cameras on the ship raycast towards the flight target and stop the ship when
//something is in the way (path flights and hole-to-hole moves; off while mining and docking).
//A camera only scans within 45° of where it faces: add cameras facing every direction the ship travels.
const bool obstacleBrake = true;
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size (the sweep covers the ship's width)
const float obstacleTimeout = 10f;//[s] show "Path blocked" after holding this long

//Traffic between PAM ships (needs a working antenna; independent of Broadcast_Channel)
//Ships keep right of the recorded path, keep distance to a PAM ship ahead, and ships on the
//same job (IMPORT JOB) mine different rows of holes.
const bool trafficEnabled = true;
const float laneOffset = 0.75f;//multiplied with ship size: how far to keep right of the path; 0 = fly on the path
const float followGap = 1f;//multiplied with ship size: free space kept behind a PAM ship ahead

//Server restart / world load
const bool resumeAfterRestart = true;//continue a job that was running when the world was saved (false = stay paused, like the original)

//Check if blocks are connected with conveyors
const bool checkConveyorSystem = false;//temporarily disabled because of a SE bug

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ν{ķ,ǅ,ǳ,Ǎ,ί}public enum Α{ђ,Ȍ,Ƃ,ȭ,Ȭ,Л,ч,
ĳ,Ż,ȋ,Ŏ,Ŋ,į,
Ǉ,Ȱ,Ƕ,ō,Βέ,ΑЌ,
Δε,Βά,ź,ΑЎ,ĸ}public enum Σ{щ,ƃ,К,Γφ,ΑЇ,ΒΘ,ΒΙ,ŵ,Ŷ}public enum џ{None,ō,ђ,Ζΰ}public enum Υ{Main,ΖЊ,Ǆ,ǃ,Δΰ,ΑЅ,
Γε,Ζυ,Ζφ,Ζσ,ǉ,ΖЏ,
ΒΤ,ΔΦ,ΔΪ,ΖБ,ΒΦ}public enum ъ{ΒΡ,Ƽ,ΚΣ,Ζω,ί}public enum ѓ{ǌ,ΖΨ}public enum ѐ{ΒΠ,Γό,ΗΓ,ǀ}public enum Е{ΔΖ,Ζέ,Γπ,Γχ,ΑЊ,ΓЇ,ΒΓ}public enum Ǟ{Γσ,Γκ,Γλ}public enum Ы{ž,Ιώ,ŋ}public enum τ{ΔΘ,ΔΜ,ΔΝ,ΖЅ,ΖЄ,
ΔΗ,ΒΝ,ΒΜ,ΔΚ,
ΔΛ,ΔΙ}public enum η{ȧ,Ē,Ȧ,ΑЄ,Γθ,Γι}public enum ǵ{ΑБ,ΑЃ,Ιβ}public enum ǎ{ΒΧ,Ζβ,ΙЉ,ΔΕ}public enum Ѝ{None,Ƅ,ǆ,ΒΛ}public enum ώ{ΑΒ,ΑЂ,ΒΖ,ǀ,ΔΒ,ΒΗ,ΔΓ,ΚΫ,ΔΞ}public enum Х{ķ,ΖЁ,ΖЀ,ΓА,ΓЉ,Γρ,ΖЂ}class έ{public bool ν=false;public Vector3 Ε=new Vector3();
public Vector3 ι=new Vector3();public Vector3 κ=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 ϋ=new Vector3();public Vector3 т=new Vector3();public float Ъ=0;public float ΜΧ=0;public float[]ȇ=null;public έ(){}public έ(έ λ){ν=λ.ν;Ε=λ.Ε;ι=λ.ι;κ=λ.κ;Ђ=λ.Ђ;ϋ=λ.ϋ;
т=λ.т;ȇ=λ.ȇ;}public έ(Vector3 Ε,Vector3 κ,Vector3 ι,Vector3 Ђ,Vector3 ϋ){this.Ε=Ε;this.ι=ι;this.κ=κ;this.Ђ=Ђ;this.Ъ=0;this.ϋ=ϋ;}public void ΙΚ(List<IMyThrust>Βα,List<string>Č){ȇ=new float[Č.Count];for(int i=0;i<ȇ.Length;i++)
ȇ[i]=-1;for(int i=0;i<Βα.Count;i++){string ǝ=Ζρ(Βα[i]);int Β=Č.IndexOf(ǝ);if(Β!=-1)
ȇ[Β]=М(Βα[i].MaxEffectiveThrust,Βα[i].MaxThrust);}}}class ΔΟ{public έ ε=null;public List<Vector3>ư=new List<Vector3>();public float ΑΞ=0;public float ΖΔ=0;public float Ƌ=0;public float ΕΑ=0;public Vector3 Βό=new Vector3();}class Ƈ{public τ Ф=τ.ΔΘ;public float ǹ=0;public float ľ=0;
public string Ǻ="";public string ǿ="";DateTime ΘΚ;public bool Εο=false;private bool ΔІ=false;public bool Γξ(bool ȃ){if(ȃ){ľ=0;ΔІ=false;return false;}ΔІ=true;return ľ>ǹ;}public void ΚΒ(){Γξ(true);Εο=false;}public void Κν(){if(ΔІ)
if((DateTime.Now-ΘΚ).TotalSeconds>1){ľ++;ΘΚ=DateTime.Now;}}public bool ΓЃ(){switch(Ф){case τ.ΔΜ:return true;case τ.ΔΝ:return true;}return false;}}class р{public String ΰ="";public String ω="";public int Ў=0;public Ы Ɲ=Ы.ŋ;public р(String ΰ,String ω,int Ў,Ы Ɲ){this.ΰ=ΰ;this.ω=ω;
this.Ў=Ў;this.Ɲ=Ɲ;}}class Ιέ{public Ιέ(Vector3 ά,float υ){this.ά=ά;this.υ=υ;}public Vector3 ά;public float υ;}class ϊ{public DateTime ΕΦ;public DateTime Ηγ;public String Κ="";public String ΰ="";public String Γβ="";public String Μ="";public String ΕЁ="";public String ΓΒ="";public String Εξ="";
public Vector3 Ε=new Vector3();public List<р>Џ=new List<р>();public Ν Ξ=Ν.ķ;public Ѝ ĺ;public float ȝ;public float ȕ;public int В;public int ΔЋ=0;public int ΕЏ=0;public bool ΓЎ(){return(DateTime.Now-ΕΦ).TotalSeconds>10;}public bool ΓЊ(){if(ĺ!=Ѝ.Ƅ) return false;return(DateTime.Now-Ηγ).TotalSeconds>=4;
}public ϊ(String Κ){this.Κ=Κ;}public Ѝ ĵ(String І,bool Λϊ,bool г,bool Κύ){if(І==""&&!г) return Ѝ.None;if(ĺ==Ѝ.Ƅ&&ΓЊ())
ĺ=Ѝ.ΒΛ;if(Λϊ){Εξ=І;ĺ=Ѝ.Ƅ;Ηγ=DateTime.Now;return ĺ;}else if(г){Εξ="";ĺ=Ѝ.None;}else if(Εξ==І){if(Κύ) ĺ=Ѝ.ǆ;return ĺ;}return Ѝ.None;}}public String Ιλ(String ΛΠ){switch(ΛΠ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΓΛ=new Vector3();Vector3 δ=new Vector3();Vector3 ΒϏ=new Vector3();Vector3 Αγ=new Vector3();Vector3 ΑΡ=new Vector3();
DateTime ΕΩ=new DateTime();bool ě=true;int ΕЉ=0;bool Ġ=false;bool ġ=false;bool Αι=true;bool ΘА=false;bool ƚ=false;bool ΒЄ=false;float Ĩ=0;float ů=0;int Ň=0;int Αϋ=0;int Αο=0;int Δό=0;float Ơ=0;float ƌ=0;double Εγ=0;float ΑΗ=0;List<int>Ɨ=new List<int>();List<int>ƨ=new List<int>();
Ν Ξ=Ν.ķ;int Д=0;float Ȉ=0;float ȥ=0;float μ=0;float ΓΞ=0,Εϊ=0;IMyRemoteControl Ω;IMySensorBlock ό;List<IMyTimerBlock>ΕЋ=new List<IMyTimerBlock>();List<IMyShipConnector>Ǐ=new List<IMyShipConnector>();List<IMyThrust>č=new List<IMyThrust>();List<IMyGyro>ΒЀ=new List<IMyGyro>();
List<IMyTerminalBlock>ύ=new List<IMyTerminalBlock>();List<IMyLandingGear>ć=new List<IMyLandingGear>();List<IMyReactor>Ņ=new List<IMyReactor>();List<IMyConveyorSorter>ΓΤ=new List<IMyConveyorSorter>();List<IMyGasTank>ǥ=new List<IMyGasTank>();List<IMyTerminalBlock>ΑΓ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ə=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŗ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ĺ=new List<IMyBatteryBlock>();List<IMyTextPanel>Щ=new List<IMyTextPanel>();List<IMyTextSurface>ȡ=new List<IMyTextSurface>();List<IMyTextPanel>Ś=new List<IMyTextPanel>();
IMyTerminalBlock ǩ=null;έ Ρ=new έ();έ ΕΞ=new έ();έ ΕΡ=new έ();bool Ď=false;bool Ƶ=false;bool ƶ=false;bool ΒІ=false;List<String>Č=new List<string>();List<έ>Ψ=new List<έ>();List<έ>ΑЀ=new List<έ>();int ΔЌ=0;double ΘБ=0;έ Ζ=new έ();Vector3 Ǚ;Vector3 Ĉ;Σ ζ=Σ.щ;ѓ Š=ѓ.ǌ;int А=0;
int Я=0;double Αβ=0;bool Βπ=false;Ƈ Ћ=new Ƈ();Ƈ Ќ=new Ƈ();ѐ њ=ѐ.ΒΠ;bool ň=true;ѓ Ƞ=ѓ.ǌ;int і=3;int ѕ=3;int с=30;Ǟ ǐ=Ǟ.Γσ;bool Į=true;bool ƙ=true;bool Ɠ=true;bool Ű=false;Е з=Е.ΔΖ;bool ĭ=true;float Ȝ=90;float Ǘ=20;float у=5;float ǘ=20;float Ȑ=0.70f;float ƺ=1.50f;
float ƹ=2.50f;float Ŵ=0f;float ŝ=0f;Α Ϋ;Σ Ǖ;Α Ατ;ΔΟ ρ=null;int Є=0;int Ѐ=0;int д=0;int Ĥ=0;int ŭ=30;int о=0;bool З=true;Vector3 ј;double е=0;bool ΓΝ=false;int ΖΗ=0;int И=0;int Δρ=0;int Ȟ=0;int Θγ=0;Vector3 Αλ=new Vector3();float ї=0;float ц=0;int ų=0;έ ΓΥ=null;
float ΕΧ=0;float Βγ=0;float н=0;float Ť=0;bool ΕΟ=false;bool ű=false;bool Αό=false;bool Ğ=false;bool Ė=false;DateTime ΚЎ=new DateTime();έ Ř=null;έ Ηλ=null;float Ȳ=0;џ ȑ;float ğ=0;float Ų=0;String Εή="";float ΒЊ=0;float ȕ=0;float ȝ=0;List<р>ǽ=new List<р>();float и=0;
bool Αρ=false;bool ΘЅ=false;bool Θρ=false;bool ΓΪ=false;bool Θς=false;float Εα=0;float Εν=0;Vector3 ΗϏ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 ΓΫ=new Vector3(1,1,1);float ΕЈ=1;Vector3 Αψ=new Vector3();bool ΒЅ=false;bool ΕΗ=false;bool ΕΫ=false;bool ΗΞ=false;float Βξ=0;
float Βί=2;Vector3 ΘЂ;Vector3 ΕЅ;Vector3 ΕЄ;float[,]ȉ=new float[3,2];Dictionary<String,float[,]>Ǯ=new Dictionary<string,float[,]>();Dictionary<String,float>ΘЎ=new Dictionary<String,float>();int Δυ=0;Dictionary<String,float[]>Ħ=new Dictionary<string,float[]>();float Επ;bool Βς=false;
int Ββ=0;int ł=0;int ΒЂ=0;int Ş=0;String Μ="";int[]ΘΨ=new int[Enum.GetValues(typeof(Υ)).Length];bool Εε=false;int ť=0;int В=0;Υ Ϗ=Υ.Main;int ΛЉ=0,ΛЊ=0,ΛЋ=0,ΛЌ=0;int Λυ=0,Λφ=0;bool Ǝ=false;bool ΗΟ=false;String ƭ="";String Δϋ="";IMyBroadcastListener Ƿ=null;
bool Ηώ=true;List<ϊ>Њ=null;ϊ О=null;int Βθ=0,Βι=0;int Ηζ=0,Ηη=0;int ΚЂ=0,ΚЃ=0,ΚЄ=0;String ΛΣ="";Exception ģ=null;bool ΓΜ=false;Σ ƪ=Σ.щ;bool Ηξ=false;bool ΘΘ=true;bool Εφ=false;String ΘЏ="";bool Αϊ=false;bool ΕЀ=true;int ΒЃ=0;int[]Ǭ=null;Vector3 Δύ=new Vector3();
bool ΗЄ=false;bool ǟ(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓЄ(Me,controllerTag,true))
Ξ=Ν.Ǎ;ξ(Υ.Main);if(Ξ!=Ν.Ǎ){Μ="Welcome to [PAM]!";ǎ Ευ=ΚΓ();if(Ξ==Ν.ķ){List<IMyShipDrill>Ηϋ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗЅ=new List<IMyShipGrinder>();χ.GetBlocksOfType(Ηϋ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗЅ,q=>q.CubeGrid==Me.CubeGrid);if(Ηϋ.Count>0)
{Ξ=Ν.ǅ;Μ="Miner mode enabled!";}else if(ΗЅ.Count>0){Ξ=Ν.ǳ;Μ="Grinder mode enabled!";}else{Ξ=Ν.ί;Ȯ(ъ.ί);}}if(Ευ==ǎ.ΒΧ) Αι=false;if(Ευ==ǎ.Ζβ) Μ="Data restore failed!";if(Ευ==ǎ.ΔΕ) Μ="New version, wipe data";}}void Main(string Ц,UpdateType ΜΓ){try{if(ģ!=null){if(Ц.Trim().ToUpper()=="RETRY")
{ģ=null;ě=true;Ň=0;if(Ξ!=Ν.Ǎ) Ѕ();Μ="Restarted after error";}else{Ιχ();ΖЈ();return;}}ġ=(ΜΓ&UpdateType.Update10)!=0;if(ġ) ΕЉ++;Ġ=ΕЉ>=10;if(Ġ) ΕЉ=0;if(ġ){Ββ++;if(Ββ>4) Ββ=0;Αο=(int) Math.Max(0,10-(DateTime.Now-ΕΩ).TotalSeconds);}if(Ц!="") ΛΣ=Ц;if(Ξ!=Ν.Ǎ)
ΚΡ(Ц);else
ΙΪ(Ц);Εε=false;try{int Ημ=Runtime.CurrentInstructionCount;float Ŭ=М(Ημ,Runtime.MaxInstructionCount);if(Ŭ>0.90) Μ="Max. instructions >90%";if(Ŭ>Ơ) Ơ=Ŭ;if(Βς){Ɨ.Add(Ημ);while(Ɨ.Count>10) Ɨ.RemoveAt(0);ƌ=0;for(int i=0;i<Ɨ.Count;i++) ƌ+=Ɨ[i];ƌ=М(М(ƌ,Ɨ.Count),Runtime.MaxInstructionCount);
double ΕΨ=Runtime.LastRunTimeMs;if(ΒЄ&&ΕΨ>Εγ) Εγ=ΕΨ;ƨ.Add((int)(ΕΨ*1000f));while(ƨ.Count>10) ƨ.RemoveAt(0);ΑΗ=0;for(int i=0;i<ƨ.Count;i++) ΑΗ+=ƨ[i];ΑΗ=М(ΑΗ,ƨ.Count)/1000f;}}catch{Ơ=0;}}catch(Exception e){ģ=e;}}void ΚΡ(string Ц){bool ΓΟ=false;String ΗΫ="";if(Д<=1&&!ΓЍ(Ц))
ΖЉ(Ц);int Εζ=50;while(Ƿ!=null&&Ƿ.HasPendingMessage&&Εζ-->0){MyIGCMessage γ=Ƿ.AcceptMessage();String Η=γ.Data as String;if(Η==null) continue;String ȟ="";if(Д<=1&&ΖЇ(ref Η,out ȟ,out ΗΫ)&&ȟ==BROADCAST_SELF_ID){ΖЉ(Η);ΓΟ=true;break;}}bool ΘΒ=ΒЄ&&Ϋ==Α.ђ&&!ƚ&&!ΓΟ&&Ň==0&&!ΒІ;
if(Ġ&&Ϋ!=Α.ђ) ΓΟ=true;if((ġ&&!ΘΒ)||(Ġ&&ΘΒ)){if(Ň==0&&(Αο<=0||ě)){ΘА=Д>0;Д=0;Ň=1;ȯ();ΚΘ();Ζχ();Ȩ("Scan 1");}else if(Ň==1){Ň=2;ȯ();ΚΙ();Ȩ("Scan 2");}else if(Ň==2){Ň=0;ȯ();ΚΚ();ΚΛ();if(Д<=1) ΖϏ();Ȩ("Scan 3");ΕΩ=DateTime.Now;if(Д<=1&&Αι)
ΓΛ=ΒΑ(Ω,Ω.CenterOfMass);Αι=false;if(ě){ě=false;bool ΜΘ=ζ==Σ.К;Ѕ();if(ΘΘ&&resumeAfterRestart&&ΜΘ&&Д<=1) Εφ=true;}ΘΘ=false;if(ΘА&&Д==0) Μ="Setup complete";}else{if(ζ==Σ.К&&Ξ!=Ν.ί){ȯ();ΙΓ();Ȩ("Inv balance");}ȯ();switch(Αϋ){case 0:Κί();break;case 1:Κζ();break;case 2:Κή();break;
case 3:Κρ();break;case 4:Κε();break;case 5:ΙΞ();break;case 6:ΙΜ(Ω);break;}Ȩ("Update: "+Αϋ);Αϋ++;if(Αϋ>6){Αϋ=0;ΒЄ=true;if(ƪ!=Σ.щ){switch(ƪ){case Σ.ŵ:ı();break;case Σ.Ŷ:ı();break;case Σ.К:ı();break;case Σ.ΒΘ:ΑЏ();break;case Σ.ΒΙ:ŏ();break;}ƪ=Σ.щ;}}}if(!ě){if(!Ȫ(Ω,true))
{Ω=null;Αι=true;Д=2;}if(Д>=2&&Ϋ!=Α.ђ)
Ѕ();if(Д<=1){ů=Ω.CalculateShipMass().PhysicalMass;Ĩ=(float) Ω.GetShipSpeed();δ=Κέ(Ω,ΓΛ);ΒϏ=Ω.WorldMatrix.Forward;Αγ=Ω.WorldMatrix.Left;ΑΡ=Ω.WorldMatrix.Down;Κκ();Κξ();Κβ();if(Εφ){Εφ=false;ı();if(Ϋ!=Α.ђ) Μ="Job resumed after restart";}if(Ϋ!=Α.ђ){ƚ=false;ΖЌ(false);ΖΦ(false);
String ΛϏ=ΓϏ(Ϋ)+" "+(int) Ϋ;ȯ();Κθ();Δδ(false);Ȩ(ΛϏ);Κο();Κι();ȯ();Κδ();Ȩ("Thruster");ȯ();ΗΚ();Ȩ("Gyroscope");}else{if(ƚ){if(ΒΕ()){п(ΑΡ,ΒϏ,Αγ,0.25f,true);ΗΚ();Μ="Aligning to planet: "+Math.Round(Βξ-0.25f,2)+"°";if(ΒЅ) ΓЂ(true,true);}else ΓЂ(true,true);}}Αϊ=false;}}}ȯ();ΖЈ();Ȩ("Print");
if(ΓΟ||Δό<=0){ȯ();ΙΔ(ΗΫ);Ȩ("Broadcast");Δό=4;}else if(Ġ) Δό--;}void ΖЉ(string Ц){if(Ц=="") return;var ψ=Ц.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var І=ψ.ElementAtOrDefault(0);var ȱ=ψ.ElementAtOrDefault(1);var ΑΖ=ψ.ElementAtOrDefault(2);var ΗΡ=ψ.ElementAtOrDefault(3);String Ć="Invalid argument: "+Ц;
bool ΖΓ=false;switch(І){case "UP":this.ΔΑ(false);break;case "DOWN":this.ΓБ(false);break;case "UPLOOP":this.ΔΑ(true);break;case "DOWNLOOP":this.ΓБ(true);break;case "APPLY":this.ΖΤ(true);break;case "MRES":В=0;break;case "STOP":this.Ѕ();break;case "PATHHOME":{this.Ѕ();this.Δή();}break;
case "PATH":{this.Ѕ();this.Δή();Ρ.ν=true;}break;case "START":{this.Ѕ();Δέ();}break;case "ALIGN":{ΓЂ(!ƚ,false);}break;case "CONT":{this.Ѕ();this.ı();}break;case "JOBPOS":{this.Ѕ();this.ŏ();}break;case "HOMEPOS":{this.Ѕ();this.ΑЏ();}break;case "FULL":{Αρ=true;}break;
case "RESET":{ΓΜ=true;Д=2;}break;case "EXPORT":{Ιγ();}break;case "IMPORT":{Ιό(ȱ=="JOB");}break;default:ΖΓ=true;break;}if(Ξ!=Ν.ί){switch(І){case "SHUTTLE":{Ιΰ();}break;case "CFGS":{if(!ΙΨ(ȱ,ΑΖ,ΗΡ)) Μ=Ć;}break;case "CFGB":{if(!ΙΧ(ȱ,ΑΖ)) Μ=Ć;}break;case "CFGL":
{if(!Ő(ref Ȝ,true,ώ.ΒΖ,ȱ,"")||!ΙΩ(ΑΖ))
Μ=Ć;}break;case "CFGE":{if(!Ő(ref у,true,ώ.ΒΗ,ȱ,"IG")||
!Ő(ref Ǘ,true,ώ.ΔΒ,ΑΖ,"IG")||
!Ő(ref ǘ,true,ώ.ΔΓ,ΗΡ,"IG"))
Μ=Ć;}break;case "CFGA":{if(!Ő(ref Ȑ,false,ώ.ΑЂ,ȱ,"")) Μ=Ć;}break;case "CFGW":{if(!Ő(ref ƺ,false,ώ.ΑΒ,ȱ,"")||
!Ő(ref ƹ,false,ώ.ΑΒ,ΑΖ,""))
Μ=Ć;}break;case "NEXT":{Γδ(false);}break;case "PREV":{Γδ(true);}break;default:if(ΖΓ) Μ=Ć;break;}}else{switch(І){case "UNDOCK":{Αϊ=true;}break;default:if(ΖΓ) Μ=Ć;break;}}}String Ιμ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ξ!=Ν.ί)
Γ+="[SHUTTLE] Enable shuttle mode\n"+"[NEXT] Next hole\n"+"[PREV] Previous hole\n\n"+
"[CFGS width height depth]*\n"+"[CFGB done damage]*\n"+"[CFGL maxload weightLimit]*\n"+
"[CFGE minUr minBat minHyd]*\n"+"[CFGW forward backward]*\n"+"[CFGA acceleration]*\n"+
"———————————————\n"+
"*[CFGS] = Config Size:\n"+" e.g.: \"CFGS 5 3 20\"\n\n"+
"*[CFGB] = Config Behaviour:\n"+" When done: [HOME,STOP]\n"+" On Damage: [HOME,JOB,STOP,IG]\n"+
" e.g.: \"CFGB HOME IG\"\n\n"+
"*[CFGL] = Config max load:\n"+" maxload: [10..95]\n"+" weight limit: [On/Off]\n"+
" e.g.: \"CFGL 70 on\"\n\n"+
"*[CFGE] = Config energy:\n"+" minUr (Uranium): [1..25, IG]\n"+" minBat (Battery): [5..30, IG]\n"+
" minHyd (Hydrogen): [10..90, IG]\n"+" e.g.: \"CFGE 20 10 IG\"\n\n"+
"*[CFGW] = Config work speed:\n"+" fwd: [0.5..10]\n"+" bwd: [0.5..10]\n"+
" e.g.: \"CFGW 1.5 2\"\n\n"+
"*[CFGA] = Config acceleration:\n"+" acceleration: [10..100]\n"+" e.g.: \"CFGA 80\"\n";else
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ιΰ(){Ѕ();Ξ=Ν.ί;Ȯ(ъ.ί);Ζ.ν=false;Ρ.ν=false;ό=null;ύ.Clear();ζ=Σ.щ;}void ΓЂ(bool Р,bool φ){if(!Р) Μ="Aligning canceled";if(φ) Μ="Aligning done";if(φ||!Р){ƚ=false;ΗΔ();ΒΥ(false,0,0,0,0);return;}if(ΒΕ()) ƚ=true;}bool ΙΨ(String ΜΚ,String ΛΙ,String ΚЈ)
{bool ΜΖ=ζ==Σ.К;int w,h,d;if(int.TryParse(ΜΚ,out w)&&int.TryParse(ΛΙ,out h)&&int.TryParse(ΚЈ,out d)){this.Ѕ();і=w;ѕ=h;с=d;Ŕ(false);ΑΑ(false,false);if(ΜΖ) ı();return true;}return false;}bool ΙΩ(String ΑΕ){if(ΑΕ=="ON"){Į=true;return true;}if(ΑΕ=="OFF"){Į=false;return true;}
return false;}bool ΙΧ(String Ηω,String Βο){bool ĉ=true;if(Ηω=="HOME") ň=true;else if(Ηω=="STOP") ň=false;else ĉ=false;if(Βο=="HOME") њ=ѐ.ΒΠ;else if(Βο=="STOP") њ=ѐ.ΗΓ;else if(Βο=="JOB") њ=ѐ.Γό;else if(Βο=="IG") њ=ѐ.ǀ;else ĉ=false;return ĉ;}void ξ(Υ ь){ΘΨ[(int) Ϗ]=В;
В=ΘΨ[(int) ь];if(ь==Υ.ǉ) В=0;Ϗ=ь;if(Ξ!=Ν.Ǎ)
œ(Ϗ==Υ.Ǆ,false,0,0);Εε=true;}void ΔΑ(bool Εά){if(В>0) В--;else if(Εά) В=ť-1;}void ΓБ(bool Εά){if(В<ť-1) В++;else if(Εά) В=0;}String Ȯ(ъ γ){switch(γ){case ъ.ΒΡ:Μ="Job is running";break;case ъ.Ƽ:Μ="Connector not ready!";break;case ъ.ΚΣ:Μ="Ship modified, path outdated!";break;
case ъ.Ζω:Μ="Interrupted by player!";break;case ъ.ί:Μ="Shuttle mode enabled!";break;}return "";}String Ις(ѓ ά){switch(ά){case ѓ.ǌ:return "Top-Left";case ѓ.ΖΨ:return "Center";default:return "";}}String Ιθ(Ǟ Г){switch(Г){case Ǟ.Γκ:return "Auto"+(Ξ==Ν.ǅ?" (Ore)":"");case Ǟ.Γλ:return "Auto (+Stone)";
case Ǟ.Γσ:return "Default";default:return "";}}String Γύ(Σ Ї){switch(Ї){case Σ.щ:return "No job";case Σ.ƃ:return "Job paused";case Σ.К:return "Job active";case Σ.ŵ:return "Job active";case Σ.Ŷ:return "Job active";case Σ.Γφ:return "Job done";case Σ.ΑЇ:return "Job changed";case Σ.ΒΘ:return "Move home";
case Σ.ΒΙ:return "Move to job";}return "";}String Ζθ(ѐ Κό){switch(Κό){case ѐ.ΒΠ:return "Return home";case ѐ.Γό:return "Fly to job pos";case ѐ.ΗΓ:return "Stop";case ѐ.ǀ:return "Ignore";}return "";}String Ικ(Е Г){switch(Г){case Е.ΔΖ:return "Off";case Е.Γχ:return "Drop pos (Stone) ";case Е.ΑЊ:return "Drop pos (Sto.+Ice)";
case Е.Ζέ:return "Cur. pos (Stone)";case Е.Γπ:return "Cur. pos (Sto.+Ice)";case Е.ΓЇ:return "In motion (Stone)";case Е.ΒΓ:return "In motion (Sto.+Ice)";}return "";}String Ζζ(џ Ї){switch(Ї){case џ.None:return "No batteries";case џ.ō:return "Charging";case џ.Ζΰ:return "Discharging";}return "";}
String ΓϏ(Α Ї){String ΜΥ=Ξ==Ν.ί?"target":"job";switch(Ї){case Α.ђ:return "Idle";case Α.Ȍ:return "Flying to XY position";case Α.Ƃ:return Ξ==Ν.ǳ?"Grinding":"Mining";case Α.ȭ:return "Returning";case Α.ź:return "Flying to drop pos";case Α.Ȭ:return "Returning to dock";case Α.Л:return "Flying to dock area";
case Α.ч:return "Flying to job area";case Α.ĳ:return "Flying to path";case Α.Ż:return "Flying to job position";case Α.ȋ:return "Approaching dock";case Α.Ŏ:return "Docking";case Α.Ŋ:return "Aligning";case Α.į:return "Aligning";case Α.Ǉ:return "Retry docking";case Α.Ȱ:return "Unloading";case Α.ĸ:return ΘЏ;
case Α.Ƕ:return "Undocking";case Α.ō:return "Charging batteries";case Α.Βέ:return "Waiting for uranium";case Α.ΑЌ:return "Filling up hydrogen";case Α.Δε:return "Waiting for ejection";case Α.Βά:return "Waiting for ejection";case Α.ΑЎ:return "Flying to drop pos";}return "";}String Ζς(τ Ф){switch(Ф)
{case τ.ΔΘ:return "On \"Undock\" command";case τ.ΔΜ:return "On player entered cockpit";case τ.ΖЅ:return "On ship is full";case τ.ΖЄ:return "On ship is empty";case τ.ΔΝ:return "On time delay";case τ.ΒΝ:return "On batteries empty(<25%)";case τ.ΒΜ:return "On batteries empty(=0%)";case τ.ΔΗ:return "On batteries full";
case τ.ΔΛ:return "On hydrogen empty(<25%)";case τ.ΔΙ:return "On hydrogen empty(=0%)";case τ.ΔΚ:return "On hydrogen full";}return "";}bool Ο(ref String Γ,int Θξ,int Ł,bool Λ,String š){ť+=1;if(Θξ==Ł) š=">"+š+(Ββ>=2?" .":"");else š=" "+š;Γ+=š+"\n";return Θξ==Ł&&Λ;}String ΖΤ(bool Λ)
{int Β=0;int Ι=В;ť=0;String Ƭ="———————————————\n";String Ʋ="--------------------------------------------\n";String Γ="";Γ+=Γύ(ζ)+" | "+(Ρ.ν?"Ready to dock":"No dock")+"\n";Γ+=Ƭ;double Ȇ=Math.Max(Math.Round(this.е),0);if(Ϗ==Υ.Main){bool Ń=Ξ==Ν.ί;if(Ο(ref Γ,Ι,Β++,Λ," Record path & set home")) Δή();
if(Ξ==Ν.ǅ)
if(Ο(ref Γ,Ι,Β++,Λ," Setup mining job")) ξ(Υ.Ǆ);if(Ξ==Ν.ǳ)
if(Ο(ref Γ,Ι,Β++,Λ," Setup grinding job")) ξ(Υ.Ǆ);if(Ξ==Ν.ί)
if(Ο(ref Γ,Ι,Β++,Λ," Setup shuttle job")) ξ(Υ.ΔΪ);if(Ο(ref Γ,Ι,Β++,Λ," Continue job")) ı();if(Ο(ref Γ,Ι,Β++,Λ," Fly to home position")) ΑЏ();if(Ο(ref Γ,Ι,Β++,Λ," Fly to job position")) ŏ();if(Ο(ref Γ,Ι,Β++,Λ," Behavior settings"))
if(Ń) ξ(Υ.ΒΦ);else ξ(Υ.ΑЅ);if(Ο(ref Γ,Ι,Β++,Λ," Info")) ξ(Υ.Ζυ);if(Ξ!=Ν.ί)
if(Ο(ref Γ,Ι,Β++,Λ," Help")) ξ(Υ.Ζσ);}else if(Ϗ==Υ.Ǆ){double ΜΙ=Math.Round(і*Ȉ,1);double ΛΘ=Math.Round(ѕ*ȥ,1);String Й="";if(Ο(ref Й,Ι,Β++,Λ," Start new job!")) Δέ();if(Ο(ref Й,Ι,Β++,Λ," Change current job")){ΑΑ(false,false);ξ(Υ.Main);}if(Ο(ref Й,Ι,Β++,Λ," Width + (Width: "+і+" = "+ΜΙ+"m)")){ŷ(ref і,5,20,1);Ŕ(true);}
if(Ο(ref Й,Ι,Β++,Λ," Width -")){ŷ(ref і,-5,20,-1);Ŕ(true);}if(Ο(ref Й,Ι,Β++,Λ," Height + (Height: "+ѕ+" = "+ΛΘ+"m)")){ŷ(ref ѕ,5,20,1);Ŕ(true);}if(Ο(ref Й,Ι,Β++,Λ," Height -")){ŷ(ref ѕ,-5,20,-1);Ŕ(true);}if(Ο(ref Й,Ι,Β++,Λ," Depth + ("+(ǐ==Ǟ.Γσ?"Depth":"Min")+": "+с+"m)")){ŷ(ref с,5,50,2);Ŕ(true);}
if(Ο(ref Й,Ι,Β++,Λ," Depth -")){ŷ(ref с,-5,50,-2);Ŕ(true);}if(Ο(ref Й,Ι,Β++,Λ," Depth mode: "+Ιθ(ǐ))){ǐ=Ĳ(ǐ);}if(Ο(ref Й,Ι,Β++,Λ," Start pos: "+Ις(Ƞ))){Ƞ=Ĳ(Ƞ);}if(Ξ==Ν.ǳ&&ǐ==Ǟ.Γλ) ǐ=Ĳ(ǐ);Γ+=ΔΥ(8,Й,Ι,ref Λυ);}else if(Ϗ==Υ.ΔΪ){float[]Ηπ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ο(ref Γ,Ι,Β++,Λ," Next")) ξ(Υ.ΖБ);if(Ο(ref Γ,Ι,Β++,Λ," Back")) ξ(Υ.Main);Γ+=" Leave connector 1:\n";if(Ο(ref Γ,Ι,Β++,Λ," - "+Ζς(Ћ.Ф))) Ћ.Ф=Ĳ(Ћ.Ф);if(!Ћ.ΓЃ()) Γ+="\n";else if(Ο(ref Γ,Ι,Β++,Λ," - Delay: "+ΑА((int) Ћ.ǹ))) Ћ.ǹ=Ζή(Ћ.ǹ,Ηπ);Γ+=" Leave connector 2:\n";if(Ο(ref Γ,Ι,Β++,Λ," - "+Ζς(Ќ.Ф))) Ќ.Ф=Ĳ(Ќ.Ф);
if(!Ќ.ΓЃ()) Γ+="\n";else if(Ο(ref Γ,Ι,Β++,Λ," - Delay: "+ΑА((int) Ќ.ǹ))) Ќ.ǹ=Ζή(Ќ.ǹ,Ηπ);}else if(Ϗ==Υ.ΖБ){if(Ο(ref Γ,Ι,Β++,Λ," Start job!")) Δέ();if(Ο(ref Γ,Ι,Β++,Λ," Back")) ξ(Υ.ΔΪ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ο(ref Γ,Ι,Β++,Λ," = "+(Ћ.Ǻ!=""?Ћ.Ǻ:"-"))) Ћ.Ǻ=ΒΚ(ref ΛЉ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ο(ref Γ,Ι,Β++,Λ," = "+(Ћ.ǿ!=""?Ћ.ǿ:"-"))) Ћ.ǿ=ΒΚ(ref ΛЋ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ο(ref Γ,Ι,Β++,Λ," = "+(Ќ.Ǻ!=""?Ќ.Ǻ:"-"))) Ќ.Ǻ=ΒΚ(ref ΛЊ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ο(ref Γ,Ι,Β++,Λ," = "+(Ќ.ǿ!=""?Ќ.ǿ:"-"))) Ќ.ǿ=ΒΚ(ref ΛЌ);
}else if(Ϗ==Υ.ǃ){String ΘΟ=Ȝ+" %";if(Ġ) ł++;if(ł>1){ł=0;ΒЂ++;if(ΒЂ>1) ΒЂ=0;}bool[]Εύ=new bool[]{Ņ.Count==0,ȑ==џ.None,ǥ.Count==0};int ΘЊ=0;while(true){ΘЊ++;Ş++;if(Ş>Εύ.Length-1) Ş=0;if(ΘЊ>=Εύ.Length) break;if(!Εύ[Ş]) break;}bool Ń=Ξ==Ν.ί;if(!Ń&&Į&&и!=-1&&ΒЂ==0)
ΘΟ=и<1000000?Math.Round(и)+" Kg":Math.Round(и/1000)+" t";if(Ο(ref Γ,Ι,Β++,Λ," Stop!")){Ѕ();ξ(Υ.Main);}if(Ο(ref Γ,Ι,Β++,Λ," Behavior settings"))
if(!Ń) ξ(Υ.ΑЅ);else ξ(Υ.ΒΦ);if(!Ń){if(Ο(ref Γ,Ι,Β++,Λ," Next hole")) Γδ(false);}else if(Ο(ref Γ,Ι,Β++,Λ," Undock")) Αϊ=true;Γ+=Ʋ;if(!Ń) Γ+="Progress: "+Math.Round(Αβ,1)+" %\n";Γ+="State: "+ΓϏ(Ϋ)+" "+Ȇ+"m \n";Γ+="Load: "+ΒЊ+" % Max: "+ΘΟ+" \n";if(Ş==0) Γ+="Uranium: "+(Ņ.Count==0?"No reactors":Math.Round(Ų,1)+"Kg "+(у==-1?"":" Min: "+у+" Kg"))+"\n";
if(Ş==1) Γ+="Battery: "+(ȑ==џ.None?Ζζ(ȑ):Ȳ+"% "+(Ǘ==-1||Ń?"":" Min: "+Ǘ+" %"))+"\n";if(Ş==2) Γ+="Hydrogen: "+(ǥ.Count==0?"No tanks":Math.Round(ğ,1)+"% "+(ǘ==-1||Ń?"":" Min: "+ǘ+" %"))+"\n";}else if(Ϗ==Υ.ΑЅ){String Й="";if(Ο(ref Й,Ι,Β++,Λ," Back")){if(ζ==Σ.К) ξ(Υ.ǃ);else ξ(Υ.Main);}
if(Ο(ref Й,Ι,Β++,Λ," Max load: "+Ȝ+"%")) я(ref Ȝ,Ȝ<=80?-10:-5,ώ.ΒΖ,false);if(Ο(ref Й,Ι,Β++,Λ," Weight limit: "+(Į?"On":"Off"))) Į=!Į;if(Ο(ref Й,Ι,Β++,Λ," Ejection: "+Ικ(з))) з=Ĳ(з);if(Ο(ref Й,Ι,Β++,Λ," Toggle sorters: "+(Ű?"On":"Off"))){Ű=!Ű;if(Ű) ȏ(ΕЀ);}
if(Ο(ref Й,Ι,Β++,Λ," Unload ice: "+(ĭ?"On":"Off"))) ĭ=!ĭ;if(Ο(ref Й,Ι,Β++,Λ," Uranium: "+(у==-1?"Ignore":"Min "+у+"Kg"))) я(ref у,(у>5?-5:-1),ώ.ΒΗ,true);if(Ο(ref Й,Ι,Β++,Λ," Battery: "+(Ǘ==-1?"Ignore":"Min "+Ǘ+"%"))) я(ref Ǘ,-5,ώ.ΔΒ,true);if(Ο(ref Й,Ι,Β++,Λ," Hydrogen: "+(ǘ==-1?"Ignore":"Min "+ǘ+"%"))) я(ref ǘ,-10,ώ.ΔΓ,true);
if(Ο(ref Й,Ι,Β++,Λ," When done: "+(ň?"Return home":"Stop"))) ň=!ň;if(Ο(ref Й,Ι,Β++,Λ," On damage: "+Ζθ(њ))) њ=Ĳ(њ);if(Ο(ref Й,Ι,Β++,Λ," Advanced...")) ξ(Υ.Γε);Γ+=ΔΥ(8,Й,Ι,ref Λφ);}else if(Ϗ==Υ.Γε){if(Ο(ref Γ,Ι,Β++,Λ," Back")){if(ζ==Σ.К) ξ(Υ.ǃ);else ξ(Υ.Main);}
if(Ο(ref Γ,Ι,Β++,Λ,(Ξ==Ν.ǳ?" Grinder":" Drill")+" inv. balancing: "+(ƙ?"On":"Off"))) ƙ=!ƙ;if(Ο(ref Γ,Ι,Β++,Λ," Enable"+(Ξ==Ν.ǳ?" grinders":" drills")+": "+(Ɠ?"Fwd + Bwd":"Fwd"))) Ɠ=!Ɠ;if(Ο(ref Γ,Ι,Β++,Λ," Work speed fwd.: "+ƺ+"m/s")) я(ref ƺ,0.5f,ώ.ΑΒ,false);
if(Ο(ref Γ,Ι,Β++,Λ," Work speed bwd.: "+ƹ+"m/s")) я(ref ƹ,0.5f,ώ.ΑΒ,false);if(Ο(ref Γ,Ι,Β++,Λ," Acceleration: "+Math.Round(Ȑ*100f)+"%"+(Ȑ>0.80f?" (risky)":""))) я(ref Ȑ,0.1f,ώ.ΑЂ,false);if(Ο(ref Γ,Ι,Β++,Λ," Width overlap: "+Ŵ*100f+"%")) ΖΩ(true,0.05f);if(Ο(ref Γ,Ι,Β++,Λ," Height overlap: "+ŝ*100f+"%")) ΖΩ(false,0.05f);
}else if(Ϗ==Υ.ΒΦ){if(Ο(ref Γ,Ι,Β++,Λ," Back")){if(ζ==Σ.К) ξ(Υ.ǃ);else ξ(Υ.Main);}if(Ο(ref Γ,Ι,Β++,Λ," Max load: "+Ȝ+"%")) я(ref Ȝ,Ȝ<=80?-10:-5,ώ.ΒΖ,false);if(Ο(ref Γ,Ι,Β++,Λ," Unload ice: "+(ĭ?"On":"Off"))) ĭ=!ĭ;if(Ο(ref Γ,Ι,Β++,Λ," Uranium: "+(у==-1?"Ignore":"Min "+у+"Kg"))) я(ref у,(у>5?-5:-1),ώ.ΒΗ,true);
if(Ο(ref Γ,Ι,Β++,Λ," Battery: "+(Ǘ==-1?"Ignore":"Charge up"))) Ǘ=(Ǘ==-1?1:-1);if(Ο(ref Γ,Ι,Β++,Λ," Hydrogen: "+(ǘ==-1?"Ignore":"Fill up"))) ǘ=(ǘ==-1?1:-1);if(Ο(ref Γ,Ι,Β++,Λ," On damage: "+Ζθ(њ))) њ=Ĳ(њ);if(Ο(ref Γ,Ι,Β++,Λ," Acceleration: "+Math.Round(Ȑ*100f)+"%"+(Ȑ>0.80f?" (risky)":""))) я(ref Ȑ,0.1f,ώ.ΑЂ,false);
}else if(Ϗ==Υ.ΖЊ){double ΛΤ=0;if(Ψ.Count>0) ΛΤ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ο(ref Γ,Ι,Β++,Λ," Stop path recording")) ΗΕ();if(Ξ!=Ν.ί){if(Ο(ref Γ,Ι,Β++,Λ," Home: "+(Ď?"Use old home":(Ρ.ν?"Was set! ":"none ")))) Ď=!Ď;}else{if(Ο(ref Γ,Ι,Β++,Λ," Connector 1: "+(Ď?"Use old connector":(Ρ.ν?"Was set! ":"none ")))) Ď=!Ď;
if(Ο(ref Γ,Ι,Β++,Λ," Connector 2: "+(Ƶ?"Use old connector":(Ζ.ν?"Was set! ":"none ")))) Ƶ=!Ƶ;}if(Ο(ref Γ,Ι,Β++,Λ," Path: "+(ƶ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ƶ=!ƶ;Γ+=Ʋ;Γ+="Wp spacing: "+Math.Round(ΘБ)+"m\n";}else if(Ϗ==Υ.Δΰ){if(Ο(ref Γ,Ι,Β++,Λ," Stop")){Ѕ();ξ(Υ.Main);}
Γ+=Ʋ;Γ+="State: "+ΓϏ(Ϋ)+" \n";Γ+="Speed: "+Math.Round(Ĩ,1)+"m/s\n";Γ+="Target dist: "+Ȇ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+ΖΗ+"\n";}else if(Ϗ==Υ.Ζυ){List<IMyTerminalBlock>ř=Ζι();if(Ġ) ł++;if(ł>=ř.Count) ł=0;if(Ο(ref Γ,Ι,Β++,Λ," Next")) ξ(Υ.Ζφ);
Γ+=Ʋ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΒЊ,1)+"% "+Math.Round(ȕ,1)+" / "+Math.Round(ȝ,1)+"\n";Γ+="Uranium: "+(Ņ.Count==0?"No reactors":Math.Round(Ų,1)+"Kg "+Εή)+"\n";Γ+="Battery: "+(ȑ==џ.None?"":Ȳ+"% ")+Ζζ(ȑ)+"\n";Γ+="Hydrogen: "+(ǥ.Count==0?"No tanks":Math.Round(ğ,1)+"% ")+"\n";
Γ+="Damage: "+(ř.Count==0?"None":""+(ł+1)+"/"+ř.Count+" "+ř[ł].CustomName)+"\n";}else if(Ϗ==Υ.Ζφ){if(Ο(ref Γ,Ι,Β++,Λ," Back")) ξ(Υ.Main);Γ+=Ʋ;Γ+="Next scan: "+Αο+"s\n";Γ+="Drill area: "+Math.Round(Ȉ,1)+"x"+Math.Round(ȥ,1)+"m Ship: "+Math.Round(ΓΞ,1)+"x"+Math.Round(Εϊ,1)+"m ("+Math.Round(μ,1)+"m)\n"+(Ȓ.Count>0?"Cameras: "+Δώ+"\n":"");
Γ+="Broadcast: "+(Ǝ?"Online - "+ƭ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ơ*100f,1)+"% \n";}else if(Ϗ==Υ.Ζσ){if(Ο(ref Γ,Ι,Β++,Λ," Back")) ξ(Υ.Main);Γ+=Ʋ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Д==2) Γ="Fatal setup error\nNext scan: "+Αο+"s\n";if(ΓΜ) Γ="Recompile script now";int ΛΥ=Γ.Split('\n').Length;for(int i=ΛΥ;i<=10;i++) Γ+="\n";Γ+=Ƭ;Γ+="Last: "+Μ+"\n";
return Γ;}void ΖΩ(bool ΛΟ,float ǭ){Ѕ();ΑΑ(true,false);if(ΛΟ) я(ref Ŵ,ǭ,ώ.ΔΞ,false);else я(ref ŝ,ǭ,ώ.ΔΞ,false);ΖΧ();œ(true,true,0,0);}float Ζή(float σ,float[]Γα){float α=Γα[0];for(int i=Γα.Length-1;i>=0;i--)
if(σ<Γα[i]) α=Γα[i];return α;}String ΒΚ(ref int Ł){String α="";if(Ł>=ΕЋ.Count) Ł=-1;if(Ł>=0) α=ΕЋ[Ł].CustomName;Ł++;return α;}void ΒΪ(string ΰ){if(ζ!=Σ.К) return;if(ΰ=="") return;IMyTerminalBlock Ϊ=χ.GetBlockWithName(ΰ);if(Ϊ==null||!(Ϊ is IMyTimerBlock)){Μ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ϊ).Trigger();}void ŷ(ref int value,int ΑΚ,int ī,int Εώ){if(ΑΚ==0) return;if(value<ī&&Εώ>0||value<=ī&&Εώ<0){value+=Εώ;return;}int Κσ=Math.Abs(ΑΚ);int Δλ=0;int Εη=1;while(true){Δλ+=Εη*Κσ*10;if(ΑΚ<0&&value-ī<=Δλ) break;if(ΑΚ>0&&value-ī<Δλ) break;
Εη++;}value+=Εη*ΑΚ;}void Ŕ(bool ΜΨ){і=Math.Max(і,1);ѕ=Math.Max(ѕ,1);с=Math.Max(с,0);œ(Ϗ==Υ.Ǆ,false,0,0);}bool Ő(ref float value,bool Λρ,ώ ω,String ş,String ΛΜ){if(ş=="") return false;float Ƥ=-1;bool ΗА=false;if(ş.ToUpper()==ΛΜ) ΗА=true;else if(!float.TryParse(ş,out Ƥ)) return false;
else Ƥ=Math.Max(0,Ƥ);if(Λρ) Ƥ=(float) Math.Round(Ƥ);я(ref value,Ƥ,ω,ΗА,false);return true;}void я(ref float value,float ǭ,ώ ω,bool Αω){я(ref value,value+ǭ,ω,Αω,true);}void я(ref float value,float ơ,ώ ω,bool Αω,bool ΜΞ){float ȁ=0,Ȁ=0;if(ω==ώ.ΑΒ){ȁ=0.5f;Ȁ=10f;}if(ω==ώ.ΑЂ){ȁ=0.1f;Ȁ=1f;}
if(ω==ώ.ǀ){ȁ=50f;Ȁ=100f;}if(ω==ώ.ΔΒ){ȁ=5f;Ȁ=30f;}if(ω==ώ.ΒΗ){ȁ=1f;Ȁ=25f;}if(ω==ώ.ΒΖ){ȁ=10f;Ȁ=95f;}if(ω==ώ.ΔΓ){ȁ=10f;Ȁ=90f;}if(ω==ώ.ΚΫ){ȁ=10f;Ȁ=1800;}if(ω==ώ.ΔΞ){ȁ=0.0f;Ȁ=0.75f;}if(ơ==-1&&Αω){value=-1;return;}if(value==-1) Αω=false;
bool Θε=ơ<ȁ||ơ>Ȁ;if(Θε&&ΜΞ){if(ơ<value) value=Ȁ;else if(ơ>value) value=ȁ;}else value=ơ;if(Θε&&Αω) value=-1;else value=Math.Max(ȁ,Math.Min(value,Ȁ));value=(float) Math.Round(value,2);}void Γδ(bool Λμ){if(Λμ) д=Math.Max(0,д-1);else д++;Γω();Γψ();for(int k=0;k<Ą.Count&&k<ж.Length;k++) ΔΨ(Ą[k],k<д);
Ж=-1;Δδ(true);}T Ĳ<T>(T σ){int Β=Array.IndexOf(Enum.GetValues(σ.GetType()),σ);Β++;if(Β>=Ια(σ)) Β=0;return(T) Enum.GetValues(σ.GetType()).GetValue(Β);}int Ια<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δή(){ΑЀ.Clear();for(int i=0;i<Ψ.Count;i++) ΑЀ.Add(Ψ[i]);Ψ.Clear();
ΒІ=true;ΕΞ=new έ(Ρ);ΕΡ=new έ(Ζ);Ρ.ν=false;if(Ξ==Ν.ί) Ζ.ν=false;for(int i=0;i<Ǯ.Count;i++)
if(!Č.Contains(Ǯ.Keys.ElementAt(i)))
Č.Add(Ǯ.Keys.ElementAt(i));Ď=false;Ƶ=false;ƶ=false;ξ(Υ.ΖЊ);}void ΗΕ(){if(Ď) Ρ=ΕΞ;if(Ƶ) Ζ=ΕΡ;if(ƶ){Ψ.Clear();for(int i=0;i<ΑЀ.Count;i++) Ψ.Add(ΑЀ[i]);}ΒІ=false;Ѕ();ξ(Υ.Main);}void Κκ(){if(!ΒІ) return;if(Ϋ!=Α.ђ){ΗΕ();return;}if(!ΕΞ.ν) Ď=false;if(!ΕΡ.ν) Ƶ=false;if(ΑЀ.Count<=1) ƶ=false;
IMyShipConnector Т=к(MyShipConnectorStatus.Connectable);if(Т==null) Т=к(MyShipConnectorStatus.Connected);if(Т!=null){if(Math.Round(Ĩ,2)<=0.20) ΔЌ++;else ΔЌ=0;if(ΔЌ>=5){if(Ξ==Ν.ί&&(Ρ.ν||Ď)&&Vector3.Distance(Ρ.Ε,Т.GetPosition())>5){Ζ.κ=Ω.WorldMatrix.Forward;Ζ.Ђ=Ω.WorldMatrix.Left;
Ζ.ι=Ω.WorldMatrix.Down;Ζ.ϋ=Ω.GetNaturalGravity();Ζ.Ε=Т.GetPosition();Ζ.ν=true;Ζ.т=Т.Position;}else{Ρ.κ=Ω.WorldMatrix.Forward;Ρ.Ђ=Ω.WorldMatrix.Left;Ρ.ι=Ω.WorldMatrix.Down;Ρ.ϋ=Ω.GetNaturalGravity();Ρ.Ε=Т.GetPosition();Ρ.ν=true;Ρ.т=Т.Position;}}}double ΕΥ=-1;if(Ψ.Count>0) ΕΥ=Vector3.Distance(δ,Ψ.Last().Ε);
double Λύ=Math.Max(1.5,Math.Pow(Ĩ/100.0,2));double ΘΩ=Math.Max(Ĩ*Λύ,2);ΘБ=ΘΩ;if((ΕΥ==-1)||ΕΥ>=ΘΩ){έ ή=new έ(δ,ΒϏ,ΑΡ,Αγ,Ω.GetNaturalGravity());ή.ΙΚ(č,Č);Ψ.Add(ή);}}έ ё(){if(Ξ!=Ν.ί) return Ǩ!=null?Ǩ:Ρ;return Ηλ;}void ƅ(έ β,Σ Ї){Ηλ=β;if(ζ==Σ.К) Ǖ=Ї;}bool ȩ(έ β,float υ,bool ΜΔ,out Vector3 ά)
{if(ΜΔ){Vector3I ΕΖ=new Vector3I((int) β.т.X,(int) β.т.Y,(int) β.т.Z);IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(ΕΖ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){ά=new Vector3();return false;}Vector3 ΚЁ=ш(Ω,Ϊ.FatBlock.GetPosition()-δ);Vector3 ΚЀ=ш(Ω,Ϊ.FatBlock.WorldMatrix.Forward);ά=β.Ε-Δγ(β.κ,β.ι*-1,ΚЁ)-Δγ(β.κ,β.ι*-1,ΚЀ)*υ;
return true;}else{ά=β.Ε;return true;}}Vector3 đ(int ΔЃ,int ņ,bool Λο){if(!Λο&&ΗЄ) return Δύ;float ΚϏ=((А-1f)/2f)-ΔЃ;float Λς=((Я-1f)/2f)-ņ;Δύ=Ζ.Ε+Ζ.Ђ*ΚϏ*Ȉ+Ĉ*-1*Λς*ȥ;ΗЄ=true;return Δύ;}Vector3 Γώ(Vector3 ΛЁ,float ΔЈ){return ΛЁ+(Ǚ*ΔЈ);}η Γτ(){float Э=-1;η α=η.ȧ;
if(Ξ!=Ν.ί){ΖϏ();if(ζ!=Σ.щ){Vector3 С=Œ(Ǚ,Ĉ*-1,δ-Ζ.Ε);if(Math.Abs(С.X)<=(float)(А*Ȉ)/2f&&Math.Abs(С.Y)<=(float)(Я*ȥ)/2f){if(С.Z<=-1&&С.Z>=-ŭ*2) return η.ΑЄ;if(С.Z>-1&&С.Z<μ*2) return η.ȧ;}if(İ(Ζ.Ε,ref Э)) α=η.ȧ;}if(Ρ.ν){if(İ(Ρ.Ε,ref Э)) α=η.Ȧ;for(int i=0;i<Ψ.Count;i++)
if(İ(Ψ[i].Ε,ref Э)) α=η.Ē;if(Vector3.Distance(δ,Ρ.Ε)<dockDist*μ) α=η.Ȧ;if(к(MyShipConnectorStatus.Connectable)!=null||к(MyShipConnectorStatus.Connected)!=null)
α=η.Ȧ;}}else{Vector3 ά=new Vector3();IMyShipConnector Ā=к(MyShipConnectorStatus.Connected);if(Ρ.ν){if(İ(Ρ.Ε,ref Э)) α=η.Ȧ;if(ȩ(Ρ,dockDist,true,out ά))
if(İ(ά,ref Э)) α=η.Ȧ;if(Ā!=null&&Vector3.Distance(Ā.GetPosition(),Ρ.Ε)<5)
return η.Γθ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ρ.Ε)>dockDist*μ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*μ)
if(İ(Ψ[i].Ε,ref Э)) α=η.Ē;if(Ζ.ν){if(İ(Ζ.Ε,ref Э)) α=η.ȧ;if(ȩ(Ζ,dockDist,true,out ά))
if(İ(ά,ref Э)) α=η.ȧ;if(Ā!=null&&Vector3.Distance(Ā.GetPosition(),Ζ.Ε)<5)
return η.Γι;}}return α;}bool İ(Vector3 ά,ref float Э){float υ=Vector3.Distance(ά,δ);if(υ<Э||Э==-1){Э=υ;return true;}return false;}void Δέ(){if(Д>0){Μ="Setup error! Can't start";return;}if(Ξ==Ν.ί){ı();return;}Ζ.Ε=δ;Ζ.ϋ=Ω.GetNaturalGravity();Ζ.κ=ΒϏ;Ζ.ι=ΑΡ;Ζ.Ђ=Αγ;
Ǚ=ǩ.WorldMatrix.Forward;Ĉ=Ζ.ι;if(Ǚ==Ω.WorldMatrix.Down) Ĉ=Ω.WorldMatrix.Backward;ΑΑ(true,true);Φ(Α.Ȍ);Δά();}void ΑΑ(bool ǻ,bool Λπ){if(ζ==Σ.щ&&!ǻ) return;bool Λή=ǻ||ζ==Σ.Γφ||А!=і||Я!=ѕ||Š!=Ƞ;if(Λή){if(ζ!=Σ.щ){ζ=Σ.ΑЇ;đ(Є,Ѐ,Λπ);Μ="Job changed, lost progress";
}Š=Ƞ;А=і;Я=ѕ;Ѐ=0;Є=0;ж="";Ж=-1;Ť=0;Ĥ=0;н=0;д=0;Δδ(true);}}void ΔΠ(){л(δ,0);ǡ(č,true);}void Δά(){Ȯ(ъ.ΒΡ);ΔΠ();ǈ(ć,false);ȏ(ΕЀ);ζ=Σ.К;ΖΦ(true);Ǖ=ζ;ξ(Υ.ǃ);ΚΗ();Βπ=true;ΒЃ=0;for(int i=ΑΓ.Count-1;i>=0;i--)
if(ǁ(ΑΓ[i],false)) ΒЃ++;if(ΒЃ>0) Μ="Started with damage";}void Ѕ(){if(ζ==Σ.К){ζ=Σ.ƃ;Μ="Job paused";}Φ(Α.ђ);Ǖ=ζ;ΒΥ(false,0,0,0,0);Δα();ΖΠ(new Vector3(),false);ΗΔ();ΔΧ(ChargeMode.Auto);ΔΩ(false);ΖЌ(true);ē(Α.ђ);œ(false,false,0,0);ǡ(ύ,false);ǡ(Ǐ,true);ȏ(true);ΓΝ=false;ΑЉ();
ΑΠ=false;ΔΣ();Βπ=false;Αρ=false;Αϊ=false;if(Ϗ!=Υ.Main&&Ϗ!=Υ.ΑЅ&&Ϗ!=Υ.Γε&&Ϗ!=Υ.ΒΦ)
ξ(Υ.Main);}void ı(){η а=Γτ();if(Ξ==Ν.ί){if(!Ζ.ν||!Ρ.ν) return;Δά();bool ΕΔ=Vector3.Distance(δ,Ρ.Ε)<Vector3.Distance(δ,Ζ.Ε);if(ƪ==Σ.ŵ) ΕΔ=true;if(ƪ==Σ.Ŷ) ΕΔ=false;if(ΕΔ){ƅ(Ρ,Σ.ŵ);switch(а){case η.Γθ:Φ(Α.ĸ);break;case η.Ē:Φ(Α.ч);break;case η.Ȧ:Φ(Α.ȋ);break;default:Φ(Α.Ƕ);break;
}}else{ƅ(Ζ,Σ.Ŷ);switch(а){case η.Γι:Φ(Α.ĸ);break;case η.ȧ:Φ(Α.ȋ);break;case η.Ē:Φ(Α.ч);break;default:Φ(Α.Ƕ);break;}}}else{if(ζ!=Σ.ƃ&&ζ!=Σ.ΑЇ) return;bool ΜΗ=ζ==Σ.ΑЇ;Δά();bool ΕΛ=ǂ(false)&&Ρ.ν;switch(а){case η.ȧ:Φ(ΕΛ?Α.ĳ:Α.Ȍ);break;case η.Ē:Φ(ΕΛ?Α.Л:Α.ч);break;case η.Ȧ:Φ(ΕΛ?Α.ȋ:Α.Ȱ);break;
case η.ΑЄ:{if(Ĥ!=д||ΜΗ) Φ(Α.ȭ);else Φ(Α.Ƃ);}break;default:break;}}}void ŏ(){if(ζ==Σ.щ&&!Ρ.ν) return;if(Ξ==Ν.ί&&(!Ζ.ν||!Ρ.ν)) return;Μ="Move to job";η а=Γτ();if(Ξ==Ν.ί){ƅ(Ζ,Σ.Ŷ);switch(а){case η.ȧ:Φ(Α.ȋ);break;case η.Ē:Φ(Α.ч);break;case η.Γι:return;default:Φ(Α.Ƕ);break;
}ē(Α.ĸ);}else{switch(а){case η.ȧ:Φ(Α.ч);break;case η.Ē:Φ(Α.ч);break;case η.Ȧ:Φ(Α.Ȱ);break;case η.ΑЄ:Φ(Α.ȭ);break;default:break;}if(ζ==Σ.щ) ē(Α.ч);else ē(Α.į);ΓΝ=true;}ΔΠ();ξ(Υ.Δΰ);ǈ(ć,false);Ǖ=Σ.ΒΙ;}void ΑЏ(){if(!Ρ.ν) return;Μ="Move home";η а=Γτ();if(Ξ==Ν.ί){ƅ(Ρ,Σ.ŵ);switch(а)
{case η.Ē:Φ(Α.Л);break;case η.Ȧ:Φ(Α.ȋ);break;case η.Γθ:return;default:Φ(Α.Ƕ);break;}ē(Α.ĸ);}else{if(к(MyShipConnectorStatus.Connected)!=null) return;if(к(MyShipConnectorStatus.Connectable)!=null){Φ(Α.Ŏ);ē(Α.Ȱ);return;}switch(а){case η.ȧ:Φ(Α.ĳ);break;case η.Ē:Φ(Α.Л);break;case η.Ȧ:Φ(Α.Л);break;
case η.ΑЄ:Φ(Α.Ȭ);break;default:break;}ē(Α.Ȱ);}ΔΠ();ξ(Υ.Δΰ);ǈ(ć,false);Ǖ=Σ.ΒΘ;}void Φ(Α Εθ){if(Εθ==Α.ђ) Ατ=Α.ђ;if(Ατ!=Α.ђ&&Ϋ==Ατ&&Εθ!=Ατ){Ѕ();return;}З=true;Ϋ=Εθ;}void ē(Α Ї){this.Ατ=Ї;}ǵ ΜΤ(int Αί,bool ȃ){if(ȃ){Ǭ=null;Є=0;Ѐ=0;}if(Ƞ==ѓ.ǌ){int ŧ=Αί+1;
Ѐ=(int) Math.Floor(М(Αί,А));if(Ѐ%2==0) Є=Αί-(Ѐ*А);else Є=А-1-(Αί-(Ѐ*А));if(Ѐ>=Я) return ǵ.ΑЃ;else return ǵ.ΑБ;}else if(Ƞ==ѓ.ΖΨ){if(Ǭ==null) Ǭ=new int[]{0,-1,0,0};int Αά=(int) Math.Ceiling(А/2f);int ΑΫ=(int) Math.Ceiling(Я/2f);int ΕΙ=(int) Math.Floor(А/2f);
int ΕΘ=(int) Math.Floor(Я/2f);int Αΰ=0;while(Ǭ[2]<Math.Pow(Math.Max(А,Я),2)){if(Αΰ>200) return ǵ.Ιβ;Αΰ++;Ǭ[2]++;if(-Αά<Є&&Є<=ΕΙ&&-ΑΫ<Ѐ&&Ѐ<=ΕΘ){if(Ǭ[3]==Αί){this.Є=Є-1+Αά;this.Ѐ=Ѐ-1+ΑΫ;return ǵ.ΑБ;}Ǭ[3]++;}if(Є==Ѐ||(Є<0&&Є==-Ѐ)||(Є>0&&Є==1-Ѐ))
{int ΛЄ=Ǭ[0];Ǭ[0]=-Ǭ[1];Ǭ[1]=ΛЄ;}Є+=Ǭ[0];Ѐ+=Ǭ[1];}}return ǵ.ΑЃ;}void Κθ(){if(Ϋ==Α.Ȍ){if(З){И=0;if(Ĥ!=д) Ť=0;Ĥ=д;}if(И==0){int ΑΛ=ΙΡ(д);if(ΑΛ<0){if(!ΑЈ()){Φ(Α.Ȭ);return;}if(!Βμ||(ΑΛ==-2)!=Βλ){Βμ=true;Βλ=ΑΛ==-2;Ηθ=!Βλ?δ:Ζ.Ε-Ǚ*μ+
Ζ.Ђ*(А*Ȉ/2+ΓΞ/2+μ/2);}л(Ηθ,10);Μ=Βλ?"Waiting for a free hole":"Waiting for the controller";return;}if(Βμ) Μ="";Βμ=false;if(ΑΛ!=д){д=ΑΛ;Ť=0;Ĥ=д;}ǵ α=ΚΠ(д);if(α==ǵ.ΑЃ){ζ=Σ.Γφ;Μ="Job done";if(ň&&Ρ.ν){Φ(Α.ĳ);ē(Α.Ȱ);Ǖ=Σ.ΒΘ;}else{Φ(Α.Ż);ē(Α.į);Ǖ=Σ.ΒΙ;}
return;}if(α==ǵ.ΑБ){И=1;ǡ(ύ,true);ј=đ(Є,Ѐ,true);л(ј,10);п(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}}else{if(Ж>=0&&(Ζώ()?Ȕ>=0&&Ȕ!=Ж:ΓЅ(Ж,true))){И=0;return;}if(е<wpReachedDist){Φ(Α.Ƃ);return;}}}if(Ϋ==Α.Ƃ){if(З){ǡ(ύ,true);ȏ(false);ј=đ(Є,Ѐ,false);л(Γώ(ј,0),0);п(Ζ.ι,Ζ.κ,Ζ.Ђ,false);
И=1;ї=0;ц=0;Βγ=0;ΕΧ=-1;ŭ=с;ΕΟ=true;}if(!ΑЈ()){Φ(Α.Ȭ);return;}if(ǂ(true)){Θγ=ď("","ORE",Ы.ŋ);if((з==Е.Γχ||з==Е.ΑЊ||
з==Е.ΓЇ||з==Е.ΒΓ)&&Ξ!=Ν.ǳ)
Φ(Α.ź);else if((з==Е.Ζέ||з==Е.Γπ)&&Ξ!=Ν.ǳ)
Φ(Α.Δε);else
Φ(Α.Ȭ);return;}н=Vector3.Distance(δ,ј);if(н>Ť){Ť=н;ΕΟ=false;}if(Ξ==Ν.ǳ&&Ζκ()==MyDetectedEntityType.SmallGrid)
ц+=2;else ц-=2;ц=Math.Max(100,Math.Min(400,ц));if(И>0&&И<ц){if(н>ї){if(ц>150) ї=н;else ї=(float) Math.Ceiling(н);И=1;}else И++;}else{if(И>0){Μ="Ship stuck! Retrying";ї=н;И=0;œ(false,true,0,μ*sensorRange);}л(Γώ(ј,Math.Max(0,ї-μ)),ΓЁ(false));if(е<=wpReachedDist/2){И=1;ї=0;}
return;}œ(false,true,μ*sensorRange,0);Vector3 Κυ=ј+Ǚ*н;bool ΘΓ=false;if(Vector3.Distance(Κυ,δ)>0.3f){Vector3 ΚЅ=ј+Ǚ*(н+0.1f);л(ΚЅ,4);ΘΓ=true;}else{float Ȅ=ΓЁ(true);Vector3 Ηο=Γώ(ј,Math.Max(с+1,н+1));Ȏ(true,false,false,Ηο,Ηο-ј,Ȅ,Ȅ);}bool ΔЉ=false;if(ǐ==Ǟ.Γλ||ǐ==Ǟ.Γκ)
{if(!ΘΓ){float Εκ=0;foreach(IMyTerminalBlock Ȥ in ύ)
Εκ+=ΙΫ(Ȥ,"","",ǐ==Ǟ.Γκ?new string[]{"STONE"}:null);if(Εκ>ΕΧ||н<с||ΕΟ){Δρ=0;Βγ=н;ŭ=(int)(Math.Max(ŭ,Βγ)+μ/2);}else{ΔЉ=н-Βγ>2&&Δρ>=20;Δρ++;}ΕΧ=Εκ;}}else ΔЉ=н>=ŭ;if(Ĥ!=д){Φ(Α.ȭ);н=0;return;}if(ΔЉ){д++;Φ(Α.ȭ);н=0;return;}}if(Ϋ==Α.ȭ||Ϋ==Α.Ȭ||Ϋ==Α.ź)
{if(З){ј=đ(Є,Ѐ,false);п(Ζ.ι,Ζ.κ,Ζ.Ђ,false);ǡ(ύ,Ɠ);ȏ(false);ї=Vector3.Distance(δ,ј);œ(false,true,0,μ*sensorRange);}л(ј,ΓЁ(false));if(Vector3.Distance(δ,ј)>=ї+5){ǡ(ύ,false);ȏ(true);Μ="Can´t return!";}if(е<wpReachedDist){if(Ϋ==Α.ȭ&&ΓΝ) Φ(Α.Ż);if(Ϋ==Α.ȭ) Φ(Α.Ȍ);
if(Ϋ==Α.ź) Φ(Α.ΑЎ);if(Ϋ==Α.Ȭ){if(Ρ.ν) Φ(Α.ĳ);else{Ѕ();ŏ();Μ="Can´t return, no dock found";}}return;}}if(Ϋ==Α.Л||Ϋ==Α.ч){if(Ϋ==Α.ч&&ζ==Σ.К&&Ξ!=Ν.ί){if(!ΑЈ()||ǂ(true)){Φ(Α.Л);return;}}bool Εσ=false;bool Γί=false;bool ΗΣ=false;float Ƌ=0;bool Θβ=false;έ ή=null;
if(З){if(Ϋ==Α.Л||Ξ==Ν.ί){if(Ξ!=Ν.ί){if(Ĕ(1)) Γη();else{Ǩ=null;Ċ=0;ΑΠ=!Γο(1);}}έ β=ё();ρ=new ΔΟ();ρ.ε=β;ρ.ΑΞ=followPathDock*μ;ρ.ΖΔ=useDockDirectionDist*μ;ρ.Ƌ=10;ρ.ư.Add(β.Ε);Vector3 Ɗ=new Vector3();if(ȩ(β,dockDist*μ,true,out Ɗ))
ρ.ư.Add(Ɗ);else ρ.ΑΞ*=1.5f;if(Ξ==Ν.ί){if(β==Ρ) ρ.Βό=Ζ.Ε;if(β==Ζ) ρ.Βό=Ρ.Ε;ρ.ΕΑ=dockDist*μ*1.1f;}}else if(Ϋ==Α.ч){ρ=new ΔΟ();ρ.ε=Ζ;ρ.ΑΞ=followPathJob*μ;ρ.ΖΔ=useJobDirectionDist*μ;ρ.Ƌ=10;ρ.Βό=Ρ.Ε;ρ.ΕΑ=dockDist*μ*1.1f;ρ.ư.Add(Ζ.Ε);if(ζ==Σ.щ){if(!Ρ.ν||Ψ.Count==0){Ѕ();return;}
float ΚЌ=Vector3.Distance(Ψ.First().Ε,Ρ.Ε);float ΚЍ=Vector3.Distance(Ψ.Last().Ε,Ρ.Ε);if(ΚЌ<ΚЍ) ρ.ε=Ψ.Last();else ρ.ε=Ψ.First();}}Αλ=new Vector3();Θβ=!ΙЀ(δ);ǡ(ύ,false);ȏ(true);о=-1;double ȓ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ρ.Βό)<=ρ.ΕΑ) continue;
double υ=Vector3.Distance(Ψ[i].Ε,δ);if(ȓ==-1||υ<ȓ){о=i;ȓ=υ;}}Ȟ=Ιο(ρ.ε.Ε,о);Ř=null;}if(Ϋ==Α.Л&&Ξ!=Ν.ί&&!З&&!Ǌ()){έ Δφ=ё();ΑΠ=!Γο(1);if(ё()!=Δφ){Φ(Α.Л);return;}}if(Ϋ==Α.Л&&Ξ!=Ν.ί&&!З&&Ĕ(1)&&Γη()){Φ(Α.Л);return;}bool ΘΑ=false;if(Ϋ==Α.Л&&!З&&о>=0&&о<Ψ.Count)
{Vector3 ΑΝ=Ψ[о].Ε+Ζν(о,Ȟ);int Ει=о+Ȟ;Vector3 ŧ=Ȟ!=0&&Ει>=0&&Ει<Ψ.Count?Ψ[Ει].Ε:ρ.ε.Ε;if(Ιϊ(ΑΝ,ŧ)){if(!Βυ){Βυ=true;Ητ=ſ(ΑΝ)?δ:ΑΝ;}л(Ητ,0);п(ρ.ε.ι,ρ.ε.κ,ρ.ε.Ђ,false);Μ="Waiting for dock clearance";return;}Βυ=false;ΘΑ=Ĕ(1)&&(ſ(δ)||ſ(ŧ));}ΙΛ(Ψ,Ȟ,ρ.ư,ρ.ΑΞ,З,ref И);
for(int i=0;i<ρ.ư.Count;i++){float υ=Vector3.Distance(δ,ρ.ư[i]);if(υ<=ρ.ΑΞ) Εσ=true;if(υ<=ρ.ΖΔ) Γί=true;}if(ΘΑ) Γί=true;if(Γί) Ƌ=ρ.Ƌ;float ΜΩ=Ř!=null?Ř.Ъ:Ĩ;float ΜΝ=(float) Math.Max(Ĩ*0.1f*μ,wpReachedDist);if((е<ΜΝ)||З){if(!З) о+=Ȟ;if(Ȟ==0||о>Ψ.Count-1||о<0)
Εσ=true;else{ΖΗ=Ȟ>0?Ψ.Count-1-о:о;ή=Ψ[о];Ř=ή;if(о>=1&&о<Ψ.Count-1)
Αλ=ή.Ε-Ψ[о-Ȟ].Ε;else Ř=null;Ѓ=ή.Ε+Ζν(о,Ȟ);ΗΣ=true;}}if(Γί)
п(ρ.ε.ι,ρ.ε.κ,ρ.ε.Ђ,false);else if(Θβ)
Γζ(ρ.ε.ι,10,true);else if(ΗΣ&&ή!=null){if(Ȟ>0) п(ή.ι,ή.κ,ή.Ђ,90,false);else п(ή.ι,-ή.κ,-ή.Ђ,90,false);}Ȏ(true,false,true,Ѓ,Αλ,Ř==null?0:Ř.Ъ,Ƌ);if(Εσ){ΖΗ=0;if(Ϋ==Α.Л&&!ΙΟ()){л(Ѓ,0);return;}if(Ϋ==Α.Л&&ΑΠ){л(Ѓ,0);Μ="Waiting for a free dock";return;}
if(Ϋ==Α.Л||Ξ==Ν.ί){Φ(Α.ȋ);return;}if(Ϋ==Α.ч&&ΓΝ){Φ(Α.Ż);return;}if(Ϋ==Α.ч){Φ(Α.Ȍ);return;}}}if(Ϋ==Α.ȋ||Ϋ==Α.Ǉ){if(Ξ!=Ν.ί&&Ǌ()){if(!Ĕ(1)){Φ(Α.Л);return;}}else if(Ξ!=Ν.ί){έ Δφ=ё();if(!Γο(2)||ё()!=Δφ){Φ(Α.Л);return;}}έ β=ё();if(З){if(!ȩ(β,dockDist*μ,true,out Ѓ))
{Ȯ(ъ.Ƽ);Ѕ();return;}л(Ѓ,0);Γζ(β.ι,90,true);}if(е<followPathDock*μ&&е!=-1){л(Ѓ,10);п(β.ι,β.κ,β.Ђ,false);}if(к(MyShipConnectorStatus.Connectable)!=null||к(MyShipConnectorStatus.Connected)!=null){Φ(Α.Ŏ);return;}if(е<wpReachedDist/2&&е!=-1){Φ(Α.Ŋ);return;}}if(Ϋ==Α.Ŋ||Ϋ==Α.į)
{if(З){if(Ϋ==Α.Ŋ){έ β=ё();if(!ȩ(β,dockDist*μ,true,out Ѓ)){Ȯ(ъ.Ƽ);Ѕ();return;}Ȏ(true,true,false,Ѓ,0);п(β.ι,β.κ,β.Ђ,10,false);}if(Ϋ==Α.į){п(Ζ.ι,Ζ.κ,Ζ.Ђ,0.5f,false);Ѓ=Ζ.Ε;Ȏ(true,true,false,Ѓ,0);}}if(ΒЅ){ΒΥ(false,0,0,0,0);if(Ϋ==Α.Ŋ) Φ(Α.Ŏ);if(Ϋ==Α.į) Ѕ();return;
}}if(Ϋ==Α.Ŏ){if(к(MyShipConnectorStatus.Connected)!=null){if(Ξ==Ν.ί) Φ(Α.ĸ);else Φ(Α.Ȱ);return;}έ β=ё();if(З){ц=0;ΚЎ=DateTime.Now;И=0;п(β.ι,β.κ,β.Ђ,false);}Vector3I ΕΖ=new Vector3I((int) β.т.X,(int) β.т.Y,(int) β.т.Z);IMySlimBlock Ηε=Me.CubeGrid.GetCubeBlock(ΕΖ);float Ηχ=dockingSpeed;
float ΛΨ=dockingSpeed*5;float ΗΠ=Math.Max(1.5f,Math.Min(5f,μ*0.15f));if(!ȩ(β,0,true,out Ѓ)||!ȩ(β,ΗΠ,true,out Αλ)||
Ηε==null||!Ηε.FatBlock.IsFunctional){Ȯ(ъ.Ƽ);Ѕ();return;}if(ц==1||(Vector3.Distance(δ,Ѓ)<=ΗΠ*1.1f&&!З))
ц=1;else{Vector3 ΛЍ=ш(Ω,Αλ-δ);Vector3 ΕΕ=ш(Ω,Ω.GetNaturalGravity());float Λσ=ΑІ(ΛЍ,ΕΕ,null);Ηχ=Math.Min(ΛΨ,Λσ);}Ȏ(true,false,false,Ѓ,Ѓ-δ,dockingSpeed,Ηχ);if(З) ї=(float) е;IMyShipConnector ΔЄ=к(MyShipConnectorStatus.Connectable);if(ΔЄ!=null){Ȏ(false,false,false,Ѓ,0);
if(И>0) И=0;И--;if(И<-5){ΔЄ.Connect();if(ΔЄ.Status==MyShipConnectorStatus.Connected){if(Ξ==Ν.ί) Φ(Α.ĸ);else Φ(Α.Ȱ);Δα();ǈ(ć,true);return;}}}else{float Ησ=(float) Math.Round(е,1);if(Ησ<ї){И=-1;ї=Ησ;}else И++;if(И>20){Φ(Α.Ǉ);return;}}}if(Ϋ==Α.Ȱ||Ϋ==Α.ĸ||
Ϋ==Α.Βέ||Ϋ==Α.ΑЌ||Ϋ==Α.ō){bool Δο=false,Δπ=false;if(Ξ==Ν.ί){if(ё()==Ρ) Δο=true;else if(ё()==Ζ) Δπ=true;}if(З){Αρ=false;if(к(MyShipConnectorStatus.Connected)==null){Φ(Α.Ƕ);return;}Δα();if(Δο) ΒΪ(Ћ.Ǻ);if(Δπ) ΒΪ(Ќ.Ǻ);ű=false;Ė=false;Ğ=false;Αό=false;}if(к(MyShipConnectorStatus.Connected)==null){Ѕ();Ȯ(ъ.Ζω);return;}
if(ζ!=Σ.К||Ǘ==-1||ȑ==џ.None) Ė=true;else if(Ȳ>=100f) Ė=true;else if(Ȳ<=99f) Ė=false;if(ζ!=Σ.К||ǘ==-1||ǥ.Count==0) Ğ=true;else if(ğ>=100f) Ğ=true;else if(ğ<=99) Ğ=false;if(ζ!=Σ.К||у==-1||Ņ.Count==0) Αό=true;else Αό=Ų>=у;Ƈ ў=null;if(Δο) ў=Ћ;
if(Δπ) ў=Ќ;if(ў!=null&&(ў.Ф==τ.ΒΝ||ў.Ф==τ.ΒΜ)) Ė=true;if(ў!=null&&(ў.Ф==τ.ΔΗ)) if(!ű) Ė=false;if(ў!=null&&(ў.Ф==τ.ΔΛ||ў.Ф==τ.ΔΙ)) Ğ=true;if(ў!=null&&(ў.Ф==τ.ΔΚ)) if(!ű) Ğ=false;if(Ġ){ChargeMode Ηή=Ė?ChargeMode.Auto:ChargeMode.Recharge;if(ў!=null&&(ў.Ф==τ.ΒΜ||ў.Ф==τ.ΒΝ))
Ηή=ChargeMode.Discharge;ΔΧ(Ηή);ΔΩ(!Ğ);}if(!ű){if(Ξ==Ν.ί)
ű=ζ!=Σ.К||ΙΠ(З,true)||Αϊ;else
ű=ζ!=Σ.К||Ζϊ();}else{if(!Ė) Φ(Α.ō);if(!Ğ) Φ(Α.ΑЌ);if(!Αό) Φ(Α.Βέ);З=false;}if(ű&&Ė&&Ğ&&Αό){ΔΧ(ChargeMode.Auto);ΔΩ(false);if(ζ==Σ.К){if(Ξ==Ν.ί){if(ё()==Ρ) ΒΪ(Ћ.ǿ);else if(ё()==Ζ) ΒΪ(Ќ.ǿ);if(ё()==Ρ) ƅ(Ζ,Σ.Ŷ);else ƅ(Ρ,Σ.ŵ);}}Φ(Α.Ƕ);return;}}if(Ϋ==Α.Ƕ){if(ΑϏ){if(!ΖΪ()) return;
ΑϏ=false;Φ(Α.Ƕ);return;}if(З){if(!ΖΪ()){ΑϏ=true;return;}IMyShipConnector Ā=к(MyShipConnectorStatus.Connected);if(Ā==null){Φ(Α.ч);return;}IMyShipConnector λ=Ā.OtherConnector;ΖЋ(Ā,false);ǈ(ć,false);έ β=null;έ ǒ=Ξ==Ν.ί?Ρ:ё();if(Vector3.Distance(Ā.GetPosition(),ǒ.Ε)<5f&&Ρ.ν) β=ǒ;
if(Vector3.Distance(Ā.GetPosition(),Ζ.Ε)<5f&&Ζ.ν) β=Ζ;if(β!=null){if(!ȩ(β,dockDist*μ,true,out Ѓ)){Ȯ(ъ.Ƽ);Ѕ();return;}л(Ѓ,5);п(β.ι,β.κ,β.Ђ,false);}else л(δ+λ.WorldMatrix.Forward*dockDist*μ,5);if(ζ==Σ.К) Ȯ(ъ.ΒΡ);}if(е<wpReachedDist){ǡ(Ǐ,true);Φ(Α.ч);return;}}if(Ϋ==Α.Ż)
{if(З){ȏ(true);ǡ(ύ,false);Ѓ=Ζ.Ε;л(Ѓ,20);п(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}if(е<wpReachedDist/2){Φ(Α.į);return;}}if(Ϋ==Α.ĳ){if(З){ȏ(true);ǡ(ύ,false);int Э=-1;double ȓ=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,δ);if(ȓ==-1||υ<ȓ){Э=i;ȓ=υ;}}
if(Э==-1){Φ(Α.Л);return;}Ѓ=Ψ[Э].Ε;л(Ѓ,10);п(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}if(е<wpReachedDist){Φ(Α.Л);return;}}if(Ϋ==Α.ΑЎ){bool φ=false;if(З){ȏ(true);if((з==Е.Γχ||з==Е.ΑЊ)&&ΒΕ()&&
Ÿ(Ǚ,Ω.GetNaturalGravity())<25&&А>=2&&Я>=2){Vector3 ΑΣ=δ;if(Є>0&&Ѐ<Я-1) ΑΣ=đ(Є-1,Ѐ+1,true);else if(Є<А-1&&Ѐ<Я-1) ΑΣ=đ(Є+1,Ѐ+1,true);else if(Є<А-1&&Ѐ>0) ΑΣ=đ(Є+1,Ѐ-1,true);else if(Є>0&&Ѐ>0) ΑΣ=đ(Є-1,Ѐ-1,true);else φ=true;
if(!φ) л(ΑΣ,10);}else φ=true;}if(е<wpReachedDist/2) φ=true;if(φ){Φ(Α.Βά);return;}}if(Ϋ==Α.Δε||Ϋ==Α.Βά){if(З){Ȏ(true,true,false,δ,0);ǡ(ύ,false);ȏ(true);И=-1;ц=з==Е.ΓЇ||з==Е.ΒΓ?0:-1;}bool ś=!ΑЈ();int ΓΨ=ď("STONE","ORE",Ы.ŋ);if(з==Е.Γπ||з==Е.ΒΓ||з==Е.ΑЊ)
ΓΨ+=ď("ICE","ORE",Ы.ŋ);bool ΛΗ=ΓΨ>0;bool ΔБ=false;if(ц>=0){float ƈ=(float) Math.Sin(Ζί(ц))*Ȉ/3f;float Κχ=(float) Math.Cos(Ζί(ц))*ȥ/3f;Vector3 Κώ=đ(Є,Ѐ,true)+Δγ(Ǚ,Ĉ*-1,new Vector3(ƈ,Κχ,0));л(Κώ,0.3f);if(е<Math.Min(Ȉ,ȥ)/10f) ц+=5f;if(ц>=360) ц=0;}if(И==-1||ΓΨ<И){И=ΓΨ;ї=0;}
else{ї++;if(ї>50) ΔБ=true;}if(!ΛΗ||ś||ΔБ){if(!ś){int ΜΑ=ď("","ORE",Ы.ŋ);if(ǂ(true)) ś=true;else if(100-(М(ΜΑ,Θγ)*100)<minEjection) ś=true;else Ȯ(ъ.ΒΡ);}if(ΔБ&&ś) Μ="Ejection failed";if(Ϋ==Α.Βά){if(ś){if(Ρ.ν) Φ(Α.ĳ);else{Ѕ();ŏ();Μ="Can´t return, no dock found";}}
else Φ(Α.Ȍ);}else if(ś) Φ(Α.Ȭ);else Φ(Α.Ƃ);return;}}З=false;}void ΙΛ(List<έ>ŉ,int π,List<Vector3>Αχ,float Ȇ,bool ȃ,ref int Ǹ){if(ȃ){for(int i=0;i<ŉ.Count;i++) ŉ[i].Ъ=0;Ǹ=-1;return;}if(π==0) return;int ǭ=π*-1;if(Ǹ==-1) Ǹ=ǭ>0?1:ŉ.Count-2;int Αΰ=0;while(Ǹ>=1&&Ǹ<ŉ.Count-1)
{if(Αΰ>50) return;Αΰ++;try{if((ǭ<0&&Ǹ>=1)||(ǭ>0&&Ǹ<=ŉ.Count-2)){έ σ=ŉ[Ǹ];bool Θή=false;for(int j=0;j<Αχ.Count;j++)
if(Vector3.Distance(σ.Ε,Αχ[j])<=Ȇ){Θή=true;break;}if(!Θή){έ ΓΘ=ŉ[Ǹ-ǭ];έ ŧ=ŉ[Ǹ+ǭ];Vector3 ΛЎ=σ.Ε-ŧ.Ε;Vector3 ΕЍ=ΓΘ.Ε-σ.Ε;Vector3 Λξ=σ.Ε+Vector3.Normalize(ΛЎ)*ΕЍ.Length();Vector3 ΚЊ=ΓΘ.Ε-Λξ;Vector3 ΒЋ=Œ(π>0?σ.κ:σ.κ*-1,σ.ι*-1,ΚЊ);Vector3 ΛΧ=Œ(π>0?σ.κ:σ.κ*-1,σ.ι*-1,ΕЍ);
Vector3 ΒЍ=Œ(π>0?σ.κ:σ.κ*-1,σ.ι*-1,σ.ϋ);σ.Ъ=(float) Math.Sqrt(Math.Pow(ΓΘ.Ъ,2)+Math.Pow(ΑІ(-ΛΧ,ΒЍ,σ),2));for(int j=0;j<Αχ.Count;j++)
if(Vector3.Distance(ΓΘ.Ε,Αχ[j])<=Ȇ){Vector3 ΒЎ=Œ(π>0?σ.κ:σ.κ*-1,σ.ι*-1,Αχ[j]-σ.Ε);float ΛЃ=ΑІ(-ΒЎ,ΒЍ,σ);σ.Ъ=Math.Min(σ.Ъ,ΛЃ)/2f;}if(ΒЋ.Length()==0) ΒЋ=new Vector3(0,0,1);Vector3 ΛΦ=Œ(σ.κ,σ.ι*-1,σ.ϋ);float ΛЈ=ƿ(-ΒЋ,ΛΦ,σ);float Βή=М(ΛЈ,ů);float ΕЊ=(float) Math.Sqrt(М(ΒЋ.Length()*1.0f,0.5f*Βή));
σ.Ъ=Math.Min(σ.Ъ,(ΕЍ.Length()/ΕЊ)*Ȑ);}}}catch{return;}Ǹ+=ǭ;}Ǹ=-1;}void Δδ(bool ȃ){if(ȃ){Αβ=0;return;}if(Ξ==Ν.ί) return;float φ=д*Math.Max(1,с);if(Ĥ==д) φ+=Math.Min(с,н);float ΛБ=А*Я*Math.Max(1,с);Αβ=Math.Max(Αβ,(float) Math.Min(φ/ΛБ*100.0,100));}MyDetectedEntityType Ζκ()
{try{if(Ȫ(ό,true)&&!ό.LastDetectedEntity.IsEmpty()) return ό.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓЁ(bool Ț){if(Ξ==Ν.ǳ&&Ζκ()==MyDetectedEntityType.None&&!ǁ(ό,true))
return fastSpeed;else return Ț?ƺ:ƹ;}void Κή(){float ΘΦ=0,Ην=0,ş=0,Ελ=0;for(int i=0;i<Ĺ.Count;i++){IMyBatteryBlock ΑΘ=Ĺ[i];if(!Ȫ(ΑΘ,true)) continue;ΘΦ+=ΑΘ.MaxStoredPower;Ην+=ΑΘ.CurrentStoredPower;ş+=ΑΘ.CurrentInput;Ελ+=ΑΘ.CurrentOutput;}Ȳ=(float) Math.Round(М(Ην,ΘΦ)*100,1);
if(ş>=Ελ) ȑ=џ.ō;else ȑ=џ.Ζΰ;if(ş==0&&Ελ==0||Ȳ==100.0) ȑ=џ.ђ;if(Ĺ.Count==0) ȑ=џ.None;}void Κε(){float Ηύ=0;for(int i=0;i<ǥ.Count;i++){IMyGasTank ΘЀ=ǥ[i];if(!Ȫ(ΘЀ,true)) continue;Ηύ+=(float) ΘЀ.FilledRatio;}ğ=М(Ηύ,ǥ.Count)*100f;}void Κρ(){Ų=0;try{for(int i=0;i<Ņ.Count;i++)
{IMyReactor Ετ=Ņ[i];if(!Ȫ(Ετ,true)) continue;List<MyInventoryItem>Џ=new List<MyInventoryItem>();Ετ.GetInventory(0).GetItems(Џ);float Ў=0;for(int j=0;j<Џ.Count;j++){MyInventoryItem Ё=Џ[j];if(Ё.Type.SubtypeId.ToUpper()=="URANIUM"&&Ё.Type.TypeId.ToUpper().Contains("_INGOT"))
Ў+=(float) Ё.Amount;}if(Ў<Ų||i==0){Ų=Ў;Εή=Ετ.CustomName;}}}catch(Exception e){ģ=e;}}void Κί(){ȝ=0;ȕ=0;try{for(int i=0;i<Ə.Count;i++){IMyTerminalBlock Ϊ=Ə[i];if(!Ȫ(Ϊ,true)) continue;ȕ+=(float) Ϊ.GetInventory(0).CurrentVolume;ȝ+=(float) Ϊ.GetInventory(0).MaxVolume;
}ΒЊ=(float) Math.Min(Math.Round(М(ȕ,ȝ)*100,1),100.0);}catch(Exception e){ģ=e;}}void Κζ(){try{ǽ.Clear();for(int i=0;i<Ə.Count;i++){IMyTerminalBlock Ϊ=Ə[i];if(!Ȫ(Ϊ,true)) continue;ΖΟ(Ϊ,Ы.ž);}if(з!=Е.ΔΖ){for(int i=0;i<Ŗ.Count;i++){IMyTerminalBlock Ϊ=Ŗ[i];if(!Ȫ(Ϊ,true)) continue;
ΖΟ(Ϊ,Ы.Ιώ);}}ΚΤ(ǽ);}catch(Exception e){ģ=e;}}void ΙΞ(){if(Βπ){if(Ζι().Count>ΒЃ){Βπ=false;if(њ!=ѐ.ǀ){Ѕ();if(њ==ѐ.Γό) ŏ();if(њ==ѐ.ΒΠ)
if(Ρ.ν) ΑЏ();else ŏ();}Μ="Damage detected";}}}bool ΑЈ(){if(!ΒЄ) return true;if(ζ==Σ.К){if(Ǘ>0&&ȑ!=џ.None)
if(Ȳ<=Ǘ){Μ="Low energy! Move home";return false;}if(у>0&&Ņ.Count>0)
if(Ų<=у){Μ="Low fuel: "+Εή;return false;}if(ǘ>0&&ǥ.Count>0)
if(ğ<=ǘ){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Ζι(){List<IMyTerminalBlock>ř=new List<IMyTerminalBlock>();for(int i=0;i<ΑΓ.Count;i++){IMyTerminalBlock Ϊ=ΑΓ[i];if(ǁ(Ϊ,false)) ř.Add(Ϊ);}return ř;}bool ǁ(IMyTerminalBlock Ϊ,bool ΕЂ){return(!Ȫ(Ϊ,ΕЂ)||!Ϊ.IsFunctional);
}bool Ȫ(IMyTerminalBlock Ϊ,bool ΕЂ){if(Ϊ==null) return false;try{IMyCubeBlock ΗЃ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(ΕЂ) return ΗЃ==Ϊ;else return ΗЃ.GetType()==Ϊ.GetType();}catch{return false;}}float ΙΫ(IMyTerminalBlock Ϊ,String ǝ,String Θχ,String[]Ș){float ā=0;for(int i=0;i<Ϊ.InventoryCount;i++)
{IMyInventory Ƙ=Ϊ.GetInventory(i);List<MyInventoryItem>Џ=new List<MyInventoryItem>();Ƙ.GetItems(Џ);foreach(MyInventoryItem Ё in Џ){if(Ș!=null&&(Ș.Contains(Ё.Type.TypeId.ToUpper())||Ș.Contains(Ё.Type.SubtypeId.ToUpper())))
continue;if((ǝ==""||Ё.Type.TypeId.ToUpper()==ǝ)&&(Θχ==""||Ё.Type.SubtypeId.ToUpper()==Θχ))
ā+=(float) Ё.Amount;}}return ā;}р Ιδ(String ΰ,String ω,Ы а,bool ΚІ){ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<ǽ.Count;i++){р Ё=ǽ[i];if(Ё.ΰ.ToUpper()==ΰ&&Ё.ω.ToUpper()==ω&&(Ё.Ɲ==а||а==Ы.ŋ))
return Ё;}р α=null;if(ΚІ){α=new р(ΰ,ω,0,а);ǽ.Add(α);}return α;}int ď(String ΰ,String ω,Ы а){return ď(ΰ,ω,а,null);}int ď(String ΰ,String ω,Ы а,String[]Ș){int ā=0;ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<ǽ.Count;i++){р Ё=ǽ[i];if(Ș!=null&&Ș.Contains(Ё.ΰ.ToUpper())) continue;
if((ΰ==""||Ё.ΰ.ToUpper()==ΰ)&&(ω==""||Ё.ω.ToUpper()==ω)&&(Ё.Ɲ==а||а==Ы.ŋ))
ā+=Ё.Ў;}return ā;}void ΖΟ(IMyTerminalBlock Ϊ,Ы а){for(int i=0;i<Ϊ.InventoryCount;i++){List<MyInventoryItem>Џ=new List<MyInventoryItem>();Ϊ.GetInventory(i).GetItems(Џ);for(int j=0;j<Џ.Count;j++)
Ιδ(Џ[j].Type.SubtypeId,Џ[j].Type.TypeId.Replace("MyObjectBuilder_",""),а,true).Ў+=(int) Џ[j].Amount;}}void ΚΤ(List<р>Б){for(int i=Б.Count-1;i>0;i--)
for(int j=0;j<i;j++){р a=Б[j];р b=Б[j+1];if(a.Ў<b.Ў) Б.Move(j,j+1);}}bool ǂ(bool ΜΦ){if(Į&&Ξ!=Ν.ί)
if(и!=-1&&ů>=и){Μ="Ship too heavy";return true;}if(ΒЊ>=Ȝ||Αρ){Αρ=false;Μ="Ship is full";return true;}return false;}bool Ζϊ(){String[]Ș=null;if(!ĭ) Ș=new string[]{"ICE"};if(Ξ==Ν.ǅ) return ď("","ORE",Ы.ž,Ș)==0;if(Ξ==Ν.ǳ) return ď("","COMPONENT",Ы.ž,Ș)==0;else return ď("","",Ы.ž,Ș)==0;
}bool ΙЁ(){List<IMyCockpit>ΔЂ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔЂ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔЂ.Count;i++)
if(ΔЂ[i].IsUnderControl) return true;return false;}bool ΙΠ(bool ȃ,bool ΓΠ){IMyShipConnector Т=к(MyShipConnectorStatus.Connected);if(Т==null) return false;if(Vector3.Distance(Ρ.Ε,Т.GetPosition())<5)
return ΖΫ(Ћ,ȃ,ΓΠ);if(Vector3.Distance(Ζ.Ε,Т.GetPosition())<5)
return ΖΫ(Ќ,ȃ,ΓΠ);return false;}bool ΖΫ(Ƈ ȳ,bool ȃ,bool ΓΠ){if(ȃ) ȳ.ΚΒ();ȳ.Κν();bool ѝ=false;String Ǜ="";switch(ȳ.Ф){case τ.ΔΘ:{Ǜ="Waiting for command";ѝ=false;break;}case τ.ΖЅ:{Ǜ="Waiting for cargo";ѝ=ǂ(true);break;}case τ.ΖЄ:{Ǜ="Unloading";ѝ=Ζϊ();break;}case τ.ΔΝ:{ѝ=true;break;}
case τ.ΔΗ:{Ǜ="Charging batteries";ѝ=Ȳ>=100f;break;}case τ.ΒΝ:{Ǜ="Discharging batteries";ѝ=Ȳ<=25f;break;}case τ.ΒΜ:{Ǜ="Discharging batteries";ѝ=Ȳ<=0f;break;}case τ.ΔΚ:{Ǜ="Filling up hydrogen";ѝ=ğ>=100f;break;}case τ.ΔΛ:{Ǜ="Unloading hydrogen";ѝ=ğ<=25f;break;}
case τ.ΔΙ:{Ǜ="Unloading hydrogen";ѝ=ğ<=0f;break;}case τ.ΔΜ:{bool ΗБ=ΙЁ();if(!ΗБ) ȳ.Εο=true;ѝ=ȳ.Εο&&ΗБ;Ǜ="Waiting for passengers";break;}}if(!ѝ) ȳ.Γξ(true);if(ѝ&&ȳ.ΓЃ()){ѝ=ȳ.Γξ(false);Ǜ="Undocking in: "+ΑА((int) Math.Max(0,ȳ.ǹ-ȳ.ľ));}if(ΓΠ) ΘЏ=Ǜ;return ѝ;}void ΙΓ()
{if(!ƙ) return;if(ύ.Count<=1) return;float ΘЇ=0,ΘІ=0;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock Ȥ=ύ[i];if(ǁ(Ȥ,true)) continue;ΘЇ+=(float) Ȥ.GetInventory(0).MaxVolume;ΘІ+=(float) Ȥ.GetInventory(0).CurrentVolume;}float ΗΤ=(float) Math.Round(М(ΘІ,ΘЇ),5);for(int ũ=0;ũ<Math.Max(1,Math.Floor(ύ.Count/10f));ũ++)
{float ΕΝ=0,Εέ=0;float ΗЊ=0,ΘΣ=0;IMyTerminalBlock Ĝ=null,Ŀ=null;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock Ȥ=ύ[i];if(ǁ(Ȥ,true)) continue;float Εδ=(float) Ȥ.GetInventory(0).MaxVolume;float Βύ=М((float) Ȥ.GetInventory(0).CurrentVolume,Εδ);if(Ĝ==null||Βύ>ΕΝ){Ĝ=Ȥ;ΕΝ=Βύ;ΗЊ=Εδ;}
if(Ŀ==null||Βύ<Εέ){Ŀ=Ȥ;Εέ=Βύ;ΘΣ=Εδ;}}if(Ĝ==null||Ŀ==null||Ĝ==Ŀ) return;if(checkConveyorSystem&&!Ĝ.GetInventory(0).IsConnectedTo(Ŀ.GetInventory(0))){if(Δυ>20) Μ="Inventory balancing failed";else Δυ++;return;}Δυ=0;List<MyInventoryItem>Џ=new List<MyInventoryItem>();
Ĝ.GetInventory(0).GetItems(Џ);float Αώ=0;if(Џ.Count==0) continue;MyInventoryItem Βώ=Џ[0];String ΘΔ=Βώ.Type.TypeId+Βώ.Type.SubtypeId;if(!ΘЎ.TryGetValue(ΘΔ,out Αώ)){if(ΙЇ(Ĝ.GetInventory(0),0,Ŀ.GetInventory(0),out Αώ))
ΘЎ.Add(ΘΔ,Αώ);else return;}float ΛЏ=((ΕΝ-ΗΤ)*ΗЊ/Αώ);float ΛА=((ΗΤ-Εέ)*ΘΣ/Αώ);int ΖΑ=(int) Math.Min(ΛА,ΛЏ);if(ΖΑ<=0) return;if((float) Βώ.Amount<ΖΑ)
Ĝ.GetInventory(0).TransferItemTo(Ŀ.GetInventory(0),0,null,null,Βώ.Amount);else
Ĝ.GetInventory(0).TransferItemTo(Ŀ.GetInventory(0),0,null,null,ΖΑ);}}bool ΙЇ(IMyInventory Ας,int Β,IMyInventory ΚЉ,out float ΘЍ){ΘЍ=0;float Κϋ=(float) Ας.CurrentVolume;List<MyInventoryItem>Δχ=new List<MyInventoryItem>();Ας.GetItems(Δχ);float ΗΨ=0;for(int i=0;i<Δχ.Count;i++) ΗΨ+=(float) Δχ[i].Amount;
Ας.TransferItemTo(ΚЉ,Β,null,null,1);float ΘЋ=Κϋ-(float) Ας.CurrentVolume;List<MyInventoryItem>Δμ=new List<MyInventoryItem>();Ας.GetItems(Δμ);float ΗΜ=0;for(int i=0;i<Δμ.Count;i++) ΗΜ+=(float) Δμ[i].Amount;if(ΘЋ==0f||!ΓЈ(0.9999,ΗΨ-ΗΜ,1.0001))
return false;ΘЍ=ΘЋ;return true;}void Κδ(){Vector3 ΑΨ=Ѓ+Ĭ;Vector3 ȣ=ΑΨ-δ;if(ȣ.Length()==0) ȣ=new Vector3(0,0,-1);Vector3 ΒЎ=ш(Ω,ȣ);Vector3 ΘЁ=Vector3.Normalize(ΒЎ);Vector3 Ɯ=ш(Ω,Ω.GetNaturalGravity());float Λώ=Εν>0?Math.Max(0,1-Ÿ(ȣ,ΗϏ)/5):0;float Ъ=(float) Math.Min((Εα>0?Εα:1000f),Math.Max(ΑІ(-ΒЎ,Ɯ,null),Εν*Λώ));
if(!ΘЅ) Ъ=0;if(Θρ) Ъ=Math.Max(0,1-Βξ/Βί)*Ъ;if(generalSpeedLimit>0) Ъ=Math.Min(generalSpeedLimit,Ъ);if(ƥ) Ъ=0;if(ǜ>=0) Ъ=Math.Min(Ъ,ǜ);if(Θς) Ъ*=(float) Math.Min(1,М(ȣ.Length(),wpReachedDist)/2);Vector3 ΘΠ=ш(Ω,Ω.GetShipVelocities().LinearVelocity);float Κφ=(float)(Math.Max(0,15-Ÿ(-ΘЁ,-ΘΠ))/15)*0.85f+0.15f;
ΕЈ+=Math.Sign(Κφ-ΕЈ)/10f;Vector3 Ļ=ΘЁ*Ъ*ΕЈ-(ΘΠ);Vector3 Δσ=ΓЀ(Ļ,null);if(ΓΪ&&е>0.1f){Ļ.X*=ΔΫ(Ļ.X,ref ΓΫ.X,1f,Δσ.X,20);Ļ.Y*=ΔΫ(Ļ.Y,ref ΓΫ.Y,1f,Δσ.Y,20);Ļ.Z*=ΔΫ(Ļ.Z,ref ΓΫ.Z,1f,Δσ.Z,20);}else ΓΫ=new Vector3(1,1,1);Αψ=ů*Ļ-Ɯ*ů;ΖΠ(Αψ,ΓΪ);е=Vector3.Distance(δ,ΑΨ);
}float ΔΫ(float value,ref float Ї,float ǭ,float ĥ,float Εβ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ї)) Ї+=Math.Sign(Ї)*ǭ;else Ї=value;if(value==0) Ї=1;float α=Math.Abs(Ї);if(α<Εβ||ĥ==0) return α;Ї=Math.Min(Εβ,Math.Max(-Εβ,Ї));α=Math.Abs(ĥ);return α;
}void ΗΚ(){float ǚ=90,Ǳ=90,ǫ=90;float Ȅ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ț,ǧ,Αε,ΕΪ;if(ΕΫ){Ț=Vector3.Normalize(Ѓ-δ);Αε=ш(Ω,Ț);ǧ=ш(Ω,ΕЄ);ǚ=Ÿ(Αε,new Vector3(0,-1,0))-90;Ǳ=Ō(ǧ,new Vector3(-1,0,0),ǧ.Y);
ǫ=Ō(Αε,new Vector3(-1,0,0),Αε.Z);}else{Ț=ΕЅ;ǧ=ш(Ω,ΕЄ);Αε=ш(Ω,ΕЅ);ΕΪ=ш(Ω,ΘЂ);ǚ=Ō(ǧ,new Vector3(0,0,1),ǧ.Y);Ǳ=Ō(ǧ,new Vector3(-1,0,0),ǧ.Y);ǫ=Ō(ΕΪ,new Vector3(0,0,1),ΕΪ.X);}if(ΗΞ&&ΒΕ()){Vector3 ΕΕ=Ω.GetNaturalGravity();ǧ=ш(Ω,ΕΕ);ǚ=Ō(ǧ,new Vector3(0,0,1),ǧ.Y);
Ǳ=Ō(ǧ,new Vector3(-1,0,0),ǧ.Y);}if(!ΓЈ(-45,Ǳ,45)){ǚ=0;ǫ=0;}if(!ΓЈ(-45,ǫ,45)) ǚ=0;ΒΥ(ΕΗ,1,(-ǚ)*Ȅ,(-ǫ)*Ȅ,(-Ǳ)*Ȅ);Βξ=Math.Max(Math.Abs(ǚ),Math.Max(Math.Abs(Ǳ),Math.Abs(ǫ)));ΒЅ=Βξ<=Βί;}void ΙΜ(IMyTerminalBlock ΓΚ){if(ΓΚ==null) return;ȉ=new float[3,2];Ǯ=new Dictionary<string,float[,]>();
for(int i=0;i<č.Count;i++){IMyThrust ю=č[i];if(!ю.IsFunctional) continue;Vector3 π=ш(ΓΚ,ю.WorldMatrix.Backward);float Ľ=ю.MaxEffectiveThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) ȉ[0,0]+=Ľ;else ȉ[0,1]-=Ľ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) ȉ[1,0]+=Ľ;else ȉ[1,1]-=Ľ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) ȉ[2,0]+=Ľ;else ȉ[2,1]-=Ľ;String ǝ=Ζρ(ю);float[,]ǯ=null;if(Ǯ.ContainsKey(ǝ)) ǯ=Ǯ[ǝ];else{ǯ=new float[3,2];Ǯ.Add(ǝ,ǯ);}float ĥ=ю.MaxThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) ǯ[0,0]+=ĥ;else ǯ[0,1]-=ĥ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) ǯ[1,0]+=ĥ;else ǯ[1,1]-=ĥ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) ǯ[2,0]+=ĥ;else ǯ[2,1]-=ĥ;}}static String Ζρ(IMyThrust ю){return ю.BlockDefinition.SubtypeId;}Vector3 ΓЏ(Vector3 ǣ,float[,]ƞ){return new Vector3(
ǣ.X>=0?ƞ[0,0]:ƞ[0,1],
ǣ.Y>=0?ƞ[1,0]:ƞ[1,1],
ǣ.Z>=0?ƞ[2,0]:ƞ[2,1]);}bool Ζπ(έ ή,String ǝ,out float ΔА){ΔА=0;int Β=Č.IndexOf(ǝ);if(Β==-1||ή.ȇ==null||Β>=ή.ȇ.Length)
return false;ΔА=ή.ȇ[Β];if(ΔА==-1) return false;return true;}Vector3 ΓЀ(Vector3 ǣ,έ ή){if(ή!=null){Vector3 α=new Vector3();for(int i=0;i<Ǯ.Keys.Count;i++){String ǝ=Ǯ.Keys.ElementAt(i);float ΑΥ=0;if(!Ζπ(ή,ǝ,out ΑΥ))
return ΓЏ(ǣ,ȉ);α+=ΓЏ(ǣ,Ǯ.Values.ElementAt(i))*ΑΥ;}return α;}return ΓЏ(ǣ,ȉ);}float ƿ(Vector3 ǣ,έ ή){return ƿ(ǣ,new Vector3(),ή);}float ƿ(Vector3 ǣ,Vector3 ϋ,έ ή){Vector3 ΛЇ=ΓЀ(ǣ,ή);Vector3 Ľ=ΛЇ+ϋ*ů;float Ŭ=(Ľ/ǣ).AbsMin();return(float)(ǣ*Ŭ).Length();}float ΑІ(Vector3 ΒЌ,Vector3 Ɯ,έ ή)
{if(ΒЌ.Length()==0) return 0;float ΗΝ=1;if(Ɯ.Length()>0) ΗΝ=Math.Min(1,Ÿ(-Ɯ,ΒЌ)/90)*0.4f+0.6f;float Αψ=ƿ(ΒЌ,Ɯ,ή);if(Αψ==0) return 0.1f;float Βή=М(Αψ,ů);float ΕЊ=(float) Math.Sqrt(М(ΒЌ.Length(),Βή*0.5f));return Βή*ΕЊ*ΗΝ*Ȑ;}float ΒΒ(Vector3 Ț,Vector3 Ј,Vector3 ϋ,έ ή)
{if(ϋ.Length()==0f) return 0;Vector3 ΒЍ=Œ(Ț,Ј,Vector3.Normalize(ϋ));float ǻ=ƿ(-ΒЍ,ή);return ǻ/ϋ.Length();}void ΖΦ(bool ΛΝ){float ǰ=0;float ΕΒ=0.9f;if(ΛΝ){и=-1;ų=0;ΓΥ=null;if(ζ!=Σ.щ&&Ζ.ϋ.Length()!=0){ǰ=ΕΒ*ΒΒ(Ζ.κ,Ζ.ι*-1,Ζ.ϋ,null);if(ǰ<и||и==-1) и=ǰ;}if(Ρ.ν&&Ρ.ϋ.Length()!=0)
{ǰ=ΕΒ*ΒΒ(Ρ.κ,Ρ.ι*-1,Ρ.ϋ,null);if(ǰ<и||и==-1) и=ǰ;}return;}if(ų==-1) return;if(ų>=0){int Θθ=0;while(ų<Ψ.Count){if(Θθ>100) return;Θθ++;έ ή=Ψ[ų];if(ή.ϋ.Length()!=0f){ǰ=ΕΒ*Math.Min(
ΒΒ(ή.κ,ή.ι*-1,ή.ϋ,ή),
ΒΒ(ή.κ*-1,ή.ι*-1,ή.ϋ,ή));if(ǰ<и||и==-1) и=ǰ;}else ΓΥ=ή;ų++;}ų=-1;}bool ΕΚ=true;float ń=0;if(ΓΥ!=null){for(int i=0;i<Ǯ.Count;i++){String ǝ=Ǯ.Keys.ElementAt(i);float[,]ǯ=Ǯ.Values.ElementAt(i);float ΑΥ=0;if(!Ζπ(ΓΥ,ǝ,out ΑΥ)){ΕΚ=false;break;}for(int a=0;a<ǯ.GetLength(0);a++)
for(int b=0;b<ǯ.GetLength(1);b++){float ŕ=Math.Abs(ǯ[a,b]*ΑΥ);if(ŕ==0) continue;ΕΚ=true;if(ń==0||ŕ<ń) ń=ŕ;}}}if(!ΕΚ){for(int a=0;a<ȉ.GetLength(0);a++)
for(int b=0;b<ȉ.GetLength(1);b++){float ŕ=Math.Abs(ȉ[a,b]);if(ŕ==0) continue;if(ń==0||ŕ<ń) ń=ŕ;}}if(ń>0){float ΛΪ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǰ=М(ń,ΛΪ);if(ǰ>0)
if(ǰ<и||и==-1) и=ǰ;}}void ΖΠ(Vector3 ǻ,bool Р){if(!Р){for(int i=0;i<č.Count;i++)
č[i].SetValueFloat("Override",0.0f);return;}Vector3 Δς=ΓЀ(ǻ,null);float ΜΟ=Math.Min(1,Math.Abs(М(ǻ.X,Δς.X)));float ΜΠ=Math.Min(1,Math.Abs(М(ǻ.Y,Δς.Y)));float ΜΡ=Math.Min(1,Math.Abs(М(ǻ.Z,Δς.Z)));for(int i=0;i<č.Count;i++){IMyThrust ю=č[i];Vector3 π=ΚΔ(ш(Ω,ю.WorldMatrix.Backward),1);
if(π.X!=0&&Math.Sign(π.X)==Math.Sign(ǻ.X))
ю.SetValueFloat("Override",ю.MaxThrust*ΜΟ);else if(π.Y!=0&&Math.Sign(π.Y)==Math.Sign(ǻ.Y))
ю.SetValueFloat("Override",ю.MaxThrust*ΜΠ);else if(π.Z!=0&&Math.Sign(π.Z)==Math.Sign(ǻ.Z))
ю.SetValueFloat("Override",ю.MaxThrust*ΜΡ);else
ю.SetValueFloat("Override",0.0f);}}void ΒΥ(bool Р,float Λκ,float ǚ,float ǫ,float Ǳ){for(int i=0;i<ΒЀ.Count;i++){IMyGyro Н=ΒЀ[i];Н.GyroOverride=Р;if(!Р) Н.GyroPower=100;else Н.GyroPower=Λκ;if(!Р) continue;Vector3 Ю=Ω.WorldMatrix.Forward;Vector3 ф=Ω.WorldMatrix.Right;Vector3 Ј=Ω.WorldMatrix.Up;
Vector3 Ɣ=Н.WorldMatrix.Forward;Vector3 Ɩ=Н.WorldMatrix.Up;Vector3 ƕ=Н.WorldMatrix.Left*-1;if(Ɣ==Ю) Н.SetValueFloat("Roll",Ǳ);else if(Ɣ==(Ю*-1)) Н.SetValueFloat("Roll",Ǳ*-1);else if(Ɩ==(Ю*-1)) Н.SetValueFloat("Yaw",Ǳ);else if(Ɩ==Ю) Н.SetValueFloat("Yaw",Ǳ*-1);else if(ƕ==Ю) Н.SetValueFloat("Pitch",Ǳ);
else if(ƕ==(Ю*-1)) Н.SetValueFloat("Pitch",Ǳ*-1);if(ƕ==(ф*-1)) Н.SetValueFloat("Pitch",ǚ);else if(ƕ==ф) Н.SetValueFloat("Pitch",ǚ*-1);else if(Ɩ==ф) Н.SetValueFloat("Yaw",ǚ);else if(Ɩ==(ф*-1)) Н.SetValueFloat("Yaw",ǚ*-1);else if(Ɣ==(ф*-1)) Н.SetValueFloat("Roll",ǚ);
else if(Ɣ==ф) Н.SetValueFloat("Roll",ǚ*-1);if(Ɩ==(Ј*-1)) Н.SetValueFloat("Yaw",ǫ);else if(Ɩ==Ј) Н.SetValueFloat("Yaw",ǫ*-1);else if(ƕ==Ј) Н.SetValueFloat("Pitch",ǫ);else if(ƕ==(Ј*-1)) Н.SetValueFloat("Pitch",ǫ*-1);else if(Ɣ==Ј) Н.SetValueFloat("Roll",ǫ);else if(Ɣ==(Ј*-1)) Н.SetValueFloat("Roll",ǫ*-1);
}}void ΗΔ(){this.ΕΗ=false;}void п(Vector3 Ȗ,Vector3 Ю,Vector3 Ţ,float ī,bool Αύ){Γζ(Ȗ,ī,Αύ);Βί=ī;ΕΫ=false;this.ΕЅ=Ю;this.ΘЂ=Ţ;}void п(Vector3 Ȗ,Vector3 Ю,Vector3 Ţ,bool Αύ){п(Ȗ,Ю,Ţ,2f,Αύ);}void Γζ(Vector3 Ȗ,float ī,bool Αύ){Βί=ī;this.ΕΗ=true;this.ΗΞ=Αύ;ΕΫ=true;ΒЅ=false;
this.ΕЄ=Ȗ;}void Δα(){Ȏ(false,false,false,Ѓ,0);ΓΪ=false;}void л(Vector3 ε,float Ȅ){Ȏ(true,false,false,ε,Ȅ);}void Ȏ(bool Р,bool ΕϏ,bool ΖΕ,Vector3 ε,float Ȅ){Ȏ(Р,ΕϏ,ΖΕ,ε,ε-δ,0.0f,Ȅ);}void Ȏ(bool Р,bool ΕϏ,bool ΖΕ,Vector3 ε,Vector3 Λψ,float Λω,float Ȅ){ΓΪ=true;this.ΘЅ=Р;
Ѓ=ε;this.Εα=Ȅ;this.Εν=Λω;this.Θς=ΕϏ;this.Θρ=ΖΕ;this.ΗϏ=Λψ;е=Vector3.Distance(ε,δ);}void ΚΗ(){χ.GetBlocksOfType(ΑΓ,ǟ);}void ΚΘ(){if(ΓΜ){Д=2;return;}List<IMyRemoteControl>Αν=new List<IMyRemoteControl>();List<IMySensorBlock>Θο=new List<IMySensorBlock>();List<IMyTerminalBlock>Αφ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αν,ǟ);χ.GetBlocksOfType(Щ,ǟ);χ.GetBlocksOfType(Θο,ǟ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αφ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Щ=ǲ(Щ,pamTag,true);ΖЍ();ΖЎ(Αφ);ő(Щ,setLCDFontAndSize,1.4f,false);ő(ȡ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Θύ=ǲ(Θο,pamTag,true);if(Θύ.Count>0) ό=Θύ[0];else ό=null;if(Ξ==Ν.ǅ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ύ.Count==0){Д=1;Μ="Drills are missing";}}else if(Ξ==Ν.ǳ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ύ.Count==0){Д=1;Μ="Grinders are missing";}if(ό==null){Д=1;Μ="Sensor is missing";}}else if(Ξ==Ν.ί){χ.GetBlocksOfType(ΕЋ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Θό=ǲ(Αν,pamTag,true);if(Θό.Count>0) Αν=Θό;if(Αν.Count>0) Ω=Αν[0];else{Ω=null;Д=2;Μ="Remote is missing";return;}μ=(float) Ω.CubeGrid.WorldVolume.Radius*2;ǩ=null;if(Ξ!=Ν.ί){ΖΧ();if(ύ.Count>0&&ǩ!=null){if(ό!=null&&(ǩ.WorldMatrix.Forward!=ό.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ό.WorldMatrix.Up||Ω.WorldMatrix.Down==ό.WorldMatrix.Down))){Д=1;Μ="Wrong sensor direction";}if(ǩ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
ǩ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){Д=2;Μ="Wrong remote direction";}}}}void ΚΙ(){χ.GetBlocksOfType(ć,ǟ);χ.GetBlocksOfType(Ǐ,ǟ);χ.GetBlocksOfType(č,ǟ);χ.GetBlocksOfType(ΒЀ,ǟ);χ.GetBlocksOfType(Ĺ,ǟ);χ.GetBlocksOfType(Ņ,ǟ);χ.GetBlocksOfType(ǥ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΓΤ,ǟ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
Ǐ=ǲ(Ǐ,"ConnectorMedium",false);else
Ǐ=ǲ(Ǐ,"Connector",false);List<IMyShipConnector>Θϊ=ǲ(Ǐ,pamTag,true);if(Θϊ.Count>0) Ǐ=Θϊ;if(Д<=1){if(Ǐ.Count==0){Д=1;Μ="Connector is missing";}if(ΒЀ.Count==0){Д=1;Μ="Gyros are missing";}if(č.Count==0){Д=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Θώ=ǲ(ΓΤ,pamTag,true);
if(Θώ.Count>0) ΓΤ=Θώ;List<IMyLandingGear>Θϋ=ǲ(ć,pamTag,true);if(Θϋ.Count>0) ć=Θϋ;for(int i=0;i<ć.Count;i++) ć[i].AutoLock=false;List<IMyBatteryBlock>Θω=ǲ(Ĺ,pamTag,true);if(Θω.Count>0) Ĺ=Θω;List<IMyGasTank>ΘϏ=ǲ(ǥ,pamTag,true);if(ΘϏ.Count>0) ǥ=ΘϏ;}void ΚΚ(){χ.GetBlocksOfType(Ŗ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ə.Clear();for(int i=Ŗ.Count-1;i>=0;i--){if(ΙϏ(Ŗ[i])){Ə.Add(Ŗ[i]);Ŗ.RemoveAt(i);}}}bool ΙϏ(IMyTerminalBlock Ϊ){if(Ϊ is IMyCargoContainer) return true;if(Ϊ is IMyShipDrill) return true;if(Ϊ is IMyShipGrinder) return true;if(Ϊ is IMyShipConnector){if(((IMyShipConnector) Ϊ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓЄ(Ϊ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΖΧ(){if(Ω==null) return;ǩ=null;float ΓΓ=0,ΒЏ=0,ΓΔ=0,ΒА=0,ΓΕ=0,ΒБ=0;List<IMyTerminalBlock>ΕЃ=ǲ(ύ,pamTag,true);bool Λα=ΕЃ.Count==0;if(ΕЃ.Count>0) ǩ=ΕЃ[0];else if(ύ.Count>0) ǩ=ύ[0];int ā=0;float ċ=0;for(int i=0;i<ύ.Count;i++)
{if(ύ[i].WorldMatrix.Forward!=ǩ.WorldMatrix.Forward){if(Λα){Д=2;Μ="Mining direction is unclear!";return;}continue;}ā++;ċ=Math.Max(ċ,Κά(ύ[i]));Vector3 С=ΒΑ(Ω,ύ[i].GetPosition());if(i==0){ΓΓ=С.X;ΒЏ=С.X;ΓΔ=С.Y;ΒА=С.Y;ΓΕ=С.Z;ΒБ=С.Z;}ΒЏ=Math.Max(С.X,ΒЏ);ΓΓ=Math.Min(С.X,ΓΓ);
ΒА=Math.Max(С.Y,ΒА);ΓΔ=Math.Min(С.Y,ΓΔ);ΒБ=Math.Max(С.Z,ΒБ);ΓΕ=Math.Min(С.Z,ΓΕ);}if(ċ==0) ċ=drillRadius;bool Ȗ=ǩ!=null&&ǩ.WorldMatrix.Forward==Ω.WorldMatrix.Down;Ȉ=(ΒЏ-ΓΓ)*(1-Ŵ)+ċ*2;ȥ=(Ȗ?ΒБ-ΓΕ:ΒА-ΓΔ)*(1-ŝ)+ċ*2;IMyCubeGrid g=Ω.CubeGrid;float ΖΙ=0,ΖΚ=0,ΖΛ=0,ΖΜ=0;
for(int c=0;c<8;c++){Vector3 p=ΒΑ(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȗ?p.Z:p.Y;if(c==0||p.X<ΖΙ) ΖΙ=p.X;if(c==0||p.X>ΖΚ) ΖΚ=p.X;if(c==0||y<ΖΛ) ΖΛ=y;if(c==0||y>ΖΜ) ΖΜ=y;}ΓΞ=ΖΚ-ΖΙ+g.GridSize;Εϊ=ΖΜ-ΖΛ+g.GridSize;}float Κά(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǲ<T>(List<T>Б,String ȅ,bool ΒЁ){List<T>α=new List<T>();for(int i=0;i<Б.Count;i++)
if(ΓЄ(Б[i],ȅ,ΒЁ)) α.Add(Б[i]);return α;}bool ΓЄ<T>(T Ϊ,String ȅ,bool ΒЁ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(ΒЁ&&b.CustomName.ToUpper().Contains(ȅ.ToUpper())) return true;if(!ΒЁ&&b.BlockDefinition.ToString().ToUpper().Contains(ȅ.ToUpper())) return true;return false;}void ǡ<T>(List<T>Б,bool Р)
{for(int i=0;i<Б.Count;i++)
ΖЋ((IMyTerminalBlock) Б[i],Р);}void ΖЋ(IMyTerminalBlock Ϊ,bool Р){if(Ϊ==null) return;String Κτ=Р?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(Κτ);a.Apply(Ϊ);}void ȏ(bool Р){ΕЀ=Р;if(!Ű) return;ǡ(ΓΤ,Р);}void ΔΧ(ChargeMode Г){for(int i=0;i<Ĺ.Count;i++) Ĺ[i].ChargeMode=Г;}void ΔΩ(bool ΛЀ)
{for(int i=0;i<ǥ.Count;i++) ǥ[i].Stockpile=ΛЀ;}void ǈ(List<IMyLandingGear>ΕΓ,bool ΘΡ){for(int i=0;i<ΕΓ.Count;i++){if(ΘΡ) ΕΓ[i].Lock();if(!ΘΡ) ΕΓ[i].Unlock();}}void ΖЌ(bool Р){if(Ηξ==Р) return;List<IMyShipController>Βκ=new List<IMyShipController>();χ.GetBlocksOfType(Βκ,ǟ);if(Βκ.Count==0) return;
for(int i=0;i<Βκ.Count;i++)
Βκ[i].DampenersOverride=Р;Ηξ=Р;}IMyShipConnector к(MyShipConnectorStatus Ǜ){for(int i=0;i<Ǐ.Count;i++){if(!Ȫ(Ǐ[i],true)) continue;if(Ǐ[i].Status==Ǜ) return Ǐ[i];}return null;}void œ(bool Εϋ,bool Βτ,float ΛΔ,float Κϊ){if(ό==null||ύ.Count==0) return;Vector3 Ɛ=new Vector3();int ā=0;
for(int i=0;i<ύ.Count;i++){if(ύ[i].WorldMatrix.Forward!=ǩ.WorldMatrix.Forward) continue;ā++;Ɛ+=ύ[i].GetPosition();}Ɛ=Ɛ/ā;Vector3 Ƣ=ΒΑ(ό,Ɛ);ό.Enabled=true;ό.ShowOnHUD=Εϋ;ό.LeftExtend=(Βτ?1:і)/2f*Ȉ-Ƣ.X;ό.RightExtend=(Βτ?1:і)/2f*Ȉ+Ƣ.X;ό.TopExtend=(Βτ?1:ѕ)/2f*ȥ+Ƣ.Y;
ό.BottomExtend=(Βτ?1:ѕ)/2f*ȥ-Ƣ.Y;ό.FrontExtend=(Εϋ?с:ΛΔ)-Ƣ.Z;ό.BackExtend=Εϋ?0:Κϊ+μ*0.75f+Ƣ.Z;ό.DetectFloatingObjects=true;ό.DetectAsteroids=false;ό.DetectLargeShips=true;ό.DetectSmallShips=true;ό.DetectStations=true;ό.DetectOwner=true;ό.DetectSubgrids=false;
ό.DetectPlayers=false;ό.DetectEnemy=true;ό.DetectFriendly=true;ό.DetectNeutral=true;}void ő<T>(List<T>Б,bool Λϋ,float ΛΒ,bool ΛΞ){for(int i=0;i<Б.Count;i++){IMyTextSurface Αυ=null;if(Б[i]is IMyTextSurface) Αυ=(IMyTextSurface) Б[i];if(Αυ!=null){Αυ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Λϋ) continue;Αυ.Font="Debug";if(ΛΞ) continue;Αυ.FontSize=ΛΒ;}}}bool ΒΕ(){Vector3D ά;return this.Ω.TryGetPlanetPosition(out ά);}int Ιο(Vector3 ΛЂ,int Θσ){if(Θσ==-1) return 0;double Э=-1;int Ę=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,ΛЂ);if(Э==-1||υ<Э){Ę=i;Э=υ;}
}return Math.Sign(Ę-Θσ);}bool ΙЀ(Vector3 ά){List<Vector3>б=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) б.Add(Ψ[i].Ε);if(Ρ.ν&&Ψ.Count>=1){Vector3 Ɗ=new Vector3();ȩ(Ρ,dockDist*μ,false,out Ɗ);if(Vector3.Distance(Ρ.Ε,Ψ.First().Ε)<Vector3.Distance(Ρ.Ε,Ψ.Last().Ε)){б.Insert(0,Ɗ);б.Insert(0,Ρ.Ε);}
else{б.Add(Ɗ);б.Add(Ρ.Ε);}}if(Ξ==Ν.ί){if(Ζ.ν&&Ψ.Count>=1){Vector3 Δξ=new Vector3();ȩ(Ζ,dockDist*μ,false,out Δξ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){б.Insert(0,Δξ);б.Insert(0,Ζ.Ε);}else{б.Add(Δξ);б.Add(Ζ.Ε);}}}else{if(ζ!=Σ.щ)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
б.Insert(0,Ζ.Ε);else б.Add(Ζ.Ε);}int Ę=-1;double ȓ=-1;for(int i=0;i<б.Count;i++){double υ=Vector3.Distance(б[i],ά);if(υ<ȓ||ȓ==-1){ȓ=υ;Ę=i;}}if(б.Count==0) return false;double ΔЊ=Vector3.Distance(б[Ę],ά);double Λλ=Vector3.Distance(б[Math.Max(0,Ę-1)],б[Ę])*1.5f;
double Λΰ=Vector3.Distance(б[Math.Min(б.Count-1,Ę+1)],б[Ę])*1.5f;return ΔЊ<Λλ||ΔЊ<Λΰ||ΔЊ<laneOffset*μ*1.5f;}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΚΔ(Vector3 v,int ΔЇ){return new Vector3(Math.Round(v.X,ΔЇ),Math.Round(v.Y,ΔЇ),Math.Round(v.Z,ΔЇ));
}float Ÿ(Vector3 a,Vector3 b){if(a==b) return 0;float ΚЏ=(a*b).Sum;float ΘΛ=a.Length();float ΘΜ=b.Length();if(ΘΛ==0||ΘΜ==0) return 0;float α=(float)((180.0f/Math.PI)*Math.Acos(ΚЏ/(ΘΛ*ΘΜ)));if(float.IsNaN(α)) return 0;return α;}float Ō(Vector3 v,Vector3 ΓΚ,float Λό){float ƈ=Ÿ(v,ΓΚ);
if(Λό>0f) ƈ*=-1;if(ƈ>-90f) return ƈ-90f;else return 180f-(-ƈ-90f);}double Ζί(float ΚЇ){return(Math.PI/180)*ΚЇ;}bool ΓЈ(double ΛΩ,double value,double Ɵ){return(value>=ΛΩ&&value<=Ɵ);}Vector3 Κέ(IMyTerminalBlock Ϊ,Vector3 С){return Vector3D.Transform(С,Ϊ.WorldMatrix);
}Vector3 ΒΑ(IMyTerminalBlock Ϊ,Vector3 ΜΜ){return ш(Ϊ,ΜΜ-Ϊ.GetPosition());}Vector3 ш(IMyTerminalBlock Ϊ,Vector3 ΖΘ){return Vector3D.TransformNormal(ΖΘ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 Œ(Vector3 Ț,Vector3 Ј,Vector3 ΖΘ){MatrixD Εΰ=MatrixD.CreateFromDir(Ț,Ј);return Vector3D.TransformNormal(ΖΘ,MatrixD.Transpose(Εΰ));
}Vector3 Δγ(Vector3 Ț,Vector3 Ј,Vector3 С){MatrixD Εΰ=MatrixD.CreateFromDir(Ț,Ј);return Vector3D.Transform(С,Εΰ);}String ο(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 θ(String s){String[]ψ=s.Split('|');return new Vector3(float.Parse(Τ(ψ,0)),float.Parse(Τ(ψ,1)),float.Parse(Τ(ψ,2)));
}String Κς(έ ή){String У=":";String α=ο(ή.Ε)+У+ο(ή.κ)+У+
ο(ή.ι)+У+ο(ή.Ђ)+У+ο(ή.ϋ);for(int i=0;i<ή.ȇ.Length;i++)
α+=У+Math.Round(ή.ȇ[i],3);return α;}έ ΚΦ(String s){String[]ψ=s.Split(':');έ ή=new έ(
θ(Τ(ψ,0)),θ(Τ(ψ,1)),
θ(Τ(ψ,2)),θ(Τ(ψ,3)),
θ(Τ(ψ,4)));int Β=5;List<float>Ηό=new List<float>();while(Β<ψ.Length){String Γΰ=Τ(ψ,Β);float f=0;if(!float.TryParse(Γΰ,out f)) break;Ηό.Add(f);Β++;}ή.ȇ=Ηό.ToArray();return ή;}System.Text.StringBuilder Ʃ=new System.Text.StringBuilder();void Χ<T>(T value,bool Λί){if(Λί) Ʃ.Append('\n');Ʃ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΑЋ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΫ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Τ(String[]Κψ,int Β){String Γΰ=Κψ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Γΰ)) return "";
return Γΰ;}void Save(){if(ΓΜ||Ξ==Ν.Ǎ){Storage="";return;}Storage=Γμ();}String Γμ(){Ʃ.Clear();Ʃ.Append(DATAREV+";");Χ(ο(ΓΛ),false);Χ(ο(Ρ.κ));Χ(ο(Ρ.Ђ));Χ(ο(Ρ.ι));Χ(ο(Ρ.ϋ));Χ(ο(Ρ.Ε));Χ(ο(Ρ.т));Χ(Ρ.ν);Χ(ο(Ζ.Ε));Χ(ο(Ζ.ϋ));Χ(ο(Ζ.κ));Χ(ο(Ζ.ι));Χ(ο(Ζ.Ђ));Χ(ο(Ζ.т));Χ(Ζ.ν);Χ(ο(Ǚ));Χ(ο(Ĉ));Χ(";");
Χ((int) Ξ,false);Χ((int) ζ);Χ((int) Ǖ);Χ(Ȝ);Χ(Ǘ);Χ(у);Χ(ǘ);Χ(Ȑ);Χ(Į);Χ(ĭ);if(Ξ==Ν.ί){Χ((int) Ћ.Ф);Χ(Ћ.ǹ);Χ(Ћ.ľ);Χ(ΑЋ(Ћ.Ǻ));Χ(ΑЋ(Ћ.ǿ));Χ((int) Ќ.Ф);Χ(Ќ.ǹ);Χ(Ќ.ľ);Χ(ΑЋ(Ќ.Ǻ));Χ(ΑЋ(Ќ.ǿ));}else{Χ((int) Ƞ);Χ((int) њ);Χ((int) з);Χ((int) ǐ);Χ((int) Š);Χ(ň);Χ(Ű);Χ(ƙ);Χ(Ɠ);Χ(і);Χ(ѕ);Χ(с);Χ(ƺ);Χ(ƹ);
Χ(Ŵ);Χ(ŝ);Χ(А);Χ(Я);Χ(Є);Χ(Ѐ);Χ(Ĥ);Χ(д);Χ(ŭ);Χ(Ť);}Χ(";");for(int i=0;i<Č.Count;i++)
Χ((i>0?"|":"")+Č[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(Κς(Ψ[i]),i>0);Χ(";",false);Χ(ж+"|"+Ж,false);Χ(";",false);Χ(Ιί(),false);String Η=Ʃ.ToString();Ʃ.Clear();return Η;}ǎ ΚΓ(){return ΒΟ(Storage);}ǎ ΒΟ(String Η){if(Η=="") return ǎ.ΙЉ;String[]Ů=Η.Split(';');if(Τ(Ů,0)!=DATAREV) return ǎ.ΔΕ;int Β=0;try{String[]Π=Τ(Ů,1).Split('\n');
ΓΛ=θ(Τ(Π,Β++));Ρ.κ=θ(Τ(Π,Β++));Ρ.Ђ=θ(Τ(Π,Β++));Ρ.ι=θ(Τ(Π,Β++));Ρ.ϋ=θ(Τ(Π,Β++));Ρ.Ε=θ(Τ(Π,Β++));Ρ.т=θ(Τ(Π,Β++));Ρ.ν=bool.Parse(Τ(Π,Β++));Ζ.Ε=θ(Τ(Π,Β++));Ζ.ϋ=θ(Τ(Π,Β++));Ζ.κ=θ(Τ(Π,Β++));Ζ.ι=θ(Τ(Π,Β++));Ζ.Ђ=θ(Τ(Π,Β++));Ζ.т=θ(Τ(Π,Β++));Ζ.ν=bool.Parse(Τ(Π,Β++));
Ǚ=θ(Τ(Π,Β++));Ĉ=θ(Τ(Π,Β++));Π=Τ(Ů,2).Split('\n');Β=0;Ξ=(Ν) int.Parse(Τ(Π,Β++));ζ=(Σ) int.Parse(Τ(Π,Β++));Ǖ=(Σ) int.Parse(Τ(Π,Β++));Ȝ=int.Parse(Τ(Π,Β++));Ǘ=int.Parse(Τ(Π,Β++));у=int.Parse(Τ(Π,Β++));ǘ=int.Parse(Τ(Π,Β++));Ȑ=float.Parse(Τ(Π,Β++));Į=bool.Parse(Τ(Π,Β++));
ĭ=bool.Parse(Τ(Π,Β++));if(Ξ==Ν.ί){Ћ.Ф=(τ) int.Parse(Τ(Π,Β++));Ћ.ǹ=float.Parse(Τ(Π,Β++));Ћ.ľ=float.Parse(Τ(Π,Β++));Ћ.Ǻ=ΒΫ(Τ(Π,Β++));Ћ.ǿ=ΒΫ(Τ(Π,Β++));Ќ.Ф=(τ) int.Parse(Τ(Π,Β++));Ќ.ǹ=float.Parse(Τ(Π,Β++));Ќ.ľ=float.Parse(Τ(Π,Β++));Ќ.Ǻ=ΒΫ(Τ(Π,Β++));Ќ.ǿ=ΒΫ(Τ(Π,Β++));}else
{Ƞ=(ѓ) int.Parse(Τ(Π,Β++));њ=(ѐ) int.Parse(Τ(Π,Β++));з=(Е) int.Parse(Τ(Π,Β++));ǐ=(Ǟ) int.Parse(Τ(Π,Β++));Š=(ѓ) int.Parse(Τ(Π,Β++));ň=bool.Parse(Τ(Π,Β++));Ű=bool.Parse(Τ(Π,Β++));ƙ=bool.Parse(Τ(Π,Β++));Ɠ=bool.Parse(Τ(Π,Β++));і=int.Parse(Τ(Π,Β++));ѕ=int.Parse(Τ(Π,Β++));с=int.Parse(Τ(Π,Β++));
ƺ=float.Parse(Τ(Π,Β++));ƹ=float.Parse(Τ(Π,Β++));Ŵ=float.Parse(Τ(Π,Β++));ŝ=float.Parse(Τ(Π,Β++));А=int.Parse(Τ(Π,Β++));Я=int.Parse(Τ(Π,Β++));Є=int.Parse(Τ(Π,Β++));Ѐ=int.Parse(Τ(Π,Β++));Ĥ=int.Parse(Τ(Π,Β++));д=int.Parse(Τ(Π,Β++));ŭ=int.Parse(Τ(Π,Β++));Ť=float.Parse(Τ(Π,Β++));
}Π=Τ(Ů,3).Replace("\n","").Split('|');Č=Π.ToList();Π=Τ(Ů,4).Split('\n');Ψ.Clear();if(Π.Count()>=1&&Π[0]!="")
for(int i=0;i<Π.Length;i++)
Ψ.Add(ΚΦ(Τ(Π,i)));Π=Τ(Ů,5).Split('|');ж=Τ(Π,0);if(!int.TryParse(Τ(Π,1),out Ж)) Ж=-1;if(ж.Length!=А*Я){ж="";Ж=-1;}Ιή(Τ(Ů,6));}catch{return ǎ.Ζβ;}ƪ=Ǖ;Ѕ();return ǎ.ΒΧ;}const int ΖΥ=64000;IMyTerminalBlock Ζγ(){List<IMyTerminalBlock>ƍ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(ƍ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ƍ.Count>0?ƍ[0]:null;}void Ιγ(){IMyTerminalBlock Ϊ=Ζγ();if(Ϊ==null){Μ="Export: name a block "+dataTag;return;}String Η="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Γμ();if(Η.Length>ΖΥ){Μ="Export too large: "+Η.Length+" chars, max "+ΖΥ;return;}Ϊ.CustomData=Η;Μ="Exported "+Ψ.Count+" waypoints";}void Ιό(bool ΕΣ){IMyTerminalBlock Ϊ=Ζγ();if(Ϊ==null){Μ="Import: name a block "+dataTag;return;}
Ιύ(Ϊ.CustomData,ΕΣ);}void Ιύ(String Η,bool ΕΣ){Η=Η.Replace("\r","");if(Η.StartsWith("//")){int Θί=Η.IndexOf('\n');Η=Θί<0?"":Η.Substring(Θί+1);}Η=Η.Trim();if(Η==""){Μ="Import: no PAM data in "+dataTag;return;}Ѕ();String ΗΦ=Γμ();ǎ α=ΒΟ(Η);if(α==ǎ.ΒΧ&&ΕΣ){έ Θ=new έ(Ζ);
Vector3 Λά=Ǚ,ΛΫ=Ĉ;int w=і,h=ѕ,d=с;ѓ Ασ=Ƞ;Ǟ ΔЈ=ǐ;float Λδ=Ŵ,Λγ=ŝ;ΒΟ(ΗΦ);Ζ=Θ;Ǚ=Λά;Ĉ=ΛΫ;і=w;ѕ=h;с=d;Ƞ=Ασ;ǐ=ΔЈ;Ŵ=Λδ;ŝ=Λγ;ζ=Σ.ƃ;ΑΑ(true,true);ζ=Σ.ƃ;Ǖ=Σ.ƃ;Μ="Imported job, run CONT";return;}if(α==ǎ.ΒΧ){Αι=false;Μ="Imported "+Ψ.Count+" waypoints";
return;}ΒΟ(ΗΦ);Μ=α==ǎ.ΔΕ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖЍ(){Ś.Clear();for(int i=Щ.Count-1;i>=0;i--){String Η=Щ[i].CustomData.ToUpper();bool ΕΠ=false;if(Η=="INSTRUCTIONS"){ΕΠ=true;Βς=true;}if(Η=="DEBUG") ΕΠ=true;if(ΕΠ){Ś.Add(Щ[i]);Щ.RemoveAt(i);}
}ő(Ś,false,1,false);}void ΖЎ(List<IMyTerminalBlock>ƍ){ȡ.Clear();for(int i=0;i<ƍ.Count;i++){IMyTerminalBlock Ϊ=ƍ[i];try{String Θη=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϊ.CustomName.IndexOf(Θη);int Ư=-1;if(Β<0||!int.TryParse(Ϊ.CustomName.Substring(Β+Θη.Length,1),out Ư)) continue;
if(Ư==-1) continue;Ư--;IMyTextSurfaceProvider Θι=(IMyTextSurfaceProvider) Ϊ;if(Ư<Θι.SurfaceCount&&Ư>=0)
ȡ.Add(Θι.GetSurface(Ư));}catch{}}}String ΓΑ="";void ΖЈ(){String Εί="";String ΑΤ="";ΓΑ=ΖΤ(false);Εί+=ΓΑ;ΑΤ+=ΓΑ;ΑΤ+=Ιμ();for(int i=0;i<Щ.Count;i++) Щ[i].WriteText(Εί);for(int i=0;i<ȡ.Count;i++) ȡ[i].WriteText(Εί);Echo(ΑΤ);for(int i=0;i<Ś.Count;i++){IMyTextPanel э=Ś[i];
String Η=э.CustomData.ToUpper();if(Η=="DEBUG") э.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") э.WriteText(Ζλ());}}string Ζλ(){String Γ="";try{float ΘΤ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ƌ*ΘΤ)+" / "+(int)(Ơ*ΘΤ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ƌ*100f,1)+"% / "+Math.Round(Ơ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ΑΗ,2)+"ms / "+Math.Round(Εγ,2)+"ms \n";}catch{}for(int i=0;i<Ħ.Count;i++)
Γ+=""+Ħ.Keys.ElementAt(i)+" = "+Math.Round(Ħ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ħ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Ιχ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ő(Щ,setLCDFontAndSize,0.9f,false);ő(ȡ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Щ.Count;i++) Щ[i].WriteText(Γ+ģ.ToString());for(int i=0;i<ȡ.Count;i++) ȡ[i].WriteText(Γ+ģ.ToString());}void ȯ(){if(!Βς) return;try{Επ=Runtime.CurrentInstructionCount;}catch{}
}void Ȩ(String ΰ){if(!Βς) return;if(Επ==0) return;try{float Γή=(Runtime.CurrentInstructionCount-Επ)/Runtime.MaxInstructionCount*100;if(!Ħ.ContainsKey(ΰ))
Ħ.Add(ΰ,new float[]{Γή,Γή});else{Ħ[ΰ][0]=Γή;Ħ[ΰ][1]=Math.Max(Γή,Ħ[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΙΔ(String ș){if(!Ǝ) return;String У=""+BROADCAST_SEP;String γ=VERSION+У;γ+=ƻ(ƭ)+У;
γ+=(int) Ξ+У;γ+=ż(δ.X)+""+У;γ+=ż(δ.Y)+У;γ+=ż(δ.Z)+У;if(Ξ!=Ν.ί)
γ+=Γύ(Ǖ)+(Ǖ==Σ.К?" "+Math.Round(Αβ,1)+"%":"")+У;else γ+=Γύ(Ǖ)+У;γ+=ƻ(Μ)+У;γ+=В+""+У;γ+=ƻ(ΓΑ)+У;γ+=ȕ+У;γ+=ȝ+У;for(int i=0;i<ǽ.Count;i++){if(ǽ[i].Ɲ!=Ы.ž) continue;γ+=ƻ(ǽ[i].ΰ)+"/"+ƻ(ǽ[i].ω)+"/"+ǽ[i].Ў+У;}ΚΞ(γ,ș);}String ƻ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΚΞ(String γ,String ș){ΒΣ(γ,BROADCAST_SELF_ID,ș);}void ΒΣ(String γ,String ε,String ș){try{if(!Ǝ) return;γ=BROADCAST_TAG+BROADCAST_SEP+Ζη()+BROADCAST_SEP+ε+BROADCAST_SEP+Δϋ+BROADCAST_SEP+ș+BROADCAST_SEP+γ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,γ);}catch(Exception e){ģ=e;}
}String Ζη(){if(Ξ!=Ν.Ǎ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΓЍ(String γ){return γ.StartsWith(BROADCAST_TAG);}bool й(ref String Η,out string ΑΧ,Char У){int Β=Η.IndexOf(У);ΑΧ="";if(Β<0) return false;ΑΧ=Η.Substring(0,Β);Η=Η.Remove(0,Β+1);return true;}bool ΖЇ(ref String γ,out String ȟ,out String ș)
{ȟ="";ș="";if(!Ǝ) return false;String ȅ="";if(!й(ref γ,out ȅ,BROADCAST_SEP)||!ΓЍ(ȅ)) return false;if(!й(ref γ,out ȟ,BROADCAST_SEP)) return false;if(!й(ref γ,out ȅ,BROADCAST_SEP)||(ȅ!=Ζη()&&(ȅ!="*"&&Ξ!=Ν.Ǎ))) return false;if(!й(ref γ,out ȅ,BROADCAST_SEP)||(ȅ!=Δϋ)) return false;
if(!й(ref γ,out ș,BROADCAST_SEP)) return false;return true;}void Ζχ(){bool ΑΜ=true;if(Ηώ){Ηώ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Αέ=(Ξ!=Ν.Ǎ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Αέ)
Ζψ(Αέ);String[]Π=Me.CustomData.Split('\n');Ǝ=bool.Parse(ΒΞ(ΑЍ(Π,"Enable_Broadcast",ref ΑΜ)));bool ΖΖ=false;bool Ης=true;if(Ǝ){if(Ξ!=Ν.Ǎ)
ƭ=ΒΞ(ΑЍ(Π,"Ship_Name",ref ΑΜ)).Replace(BROADCAST_SELF_ID,"");Δϋ=ΒΞ(ΑЍ(Π,"Broadcast_Channel",ref ΑΜ)).ToLower();Ης=false;if(Ƿ==null){Ƿ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ƿ.SetMessageCallback("");}List<IMyRadioAntenna>Ɖ=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ɖ);
bool Δν=false;for(int i=0;i<Ɖ.Count;i++)
if(Ɖ[i].EnableBroadcasting&&Ɖ[i].Enabled){Δν=true;break;}if(Ɖ.Count==0) Μ="No Antenna found";else if(!Δν) Μ="Antenna not ready";ΖΖ=Ɖ.Count==0||!Δν;if(ΗΟ&&!ΖΖ&&Ξ!=Ν.Ǎ) Μ="Antenna ok";}else if(Ξ==Ν.Ǎ)
Μ="Offline - Enable in PB custom data";ΗΟ=ΖΖ;if(Ης){if(Ƿ!=null) this.IGC.DisableBroadcastListener(Ƿ);Ƿ=null;}}catch{ΑΜ=false;}if(!ΑΜ){Μ="Reset custom data";Ζψ(Αέ);}}void Ζψ(String Αέ){String Η=Αέ+"\n\n"+"Enable_Broadcast="+(Ξ==Ν.Ǎ?"true":"false")+" \n";Η+=Ξ!=Ν.Ǎ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Η+"Broadcast_Channel=#default";}String ΒΞ(String ţ){int Β=ţ.IndexOf("//");if(Β!=-1) ţ=ţ.Substring(0,Β);String[]ψ=ţ.Split('=');if(ψ.Length<=1) return "";return ψ[1].Trim();}String ΑЍ(String[]Π,String ы,ref bool ĉ){foreach(String ţ in Π)
if(ţ.StartsWith(ы)) return ţ;ĉ=false;return "";}void ΙΪ(string Ц){if(ě) Њ=new List<ϊ>();String ȟ="";String ș="";if(!ΓЍ(Ц)) ΙБ(Ц);bool Θπ=false;int Εζ=50;while(Ƿ!=null&&Ƿ.HasPendingMessage&&Εζ-->0){MyIGCMessage γ=Ƿ.AcceptMessage();String Η=γ.Data as String;if(Η==null) continue;
if(ΖЇ(ref Η,out ȟ,out ș)&&ȟ!=""&&ȟ!=BROADCAST_SELF_ID){ϊ Δ=Γϋ(ȟ,"");if(Δ==null){Δ=new ϊ(ȟ);Њ.Add(Δ);}Δ.ĵ(ș,false,false,true);Κλ(Δ,Η);Θπ=true;}}if(Θπ)
Њ.Sort(delegate(ϊ a,ϊ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(Ġ||ě){if(Αο<=0||ě){Μ="";ě=false;ΚΜ();Ζχ();ΕΩ=DateTime.Now;}}if(ġ) Κγ();if(ġ) Κη();if(ġ) Κπ();if(ġ) Κμ();if(ġ&&ΕІ==0) Κα();
Κΰ();}void Κλ(ϊ Δ,String Η){Δ.ΕΦ=DateTime.Now;String ΑΧ="";й(ref Η,out Δ.Γβ,BROADCAST_SEP);й(ref Η,out Δ.ΰ,BROADCAST_SEP);if(Δ.Γβ!=VERSION) return;String Αθ="";String[]Ƨ=new string[3];й(ref Η,out Αθ,BROADCAST_SEP);й(ref Η,out Ƨ[0],BROADCAST_SEP);й(ref Η,out Ƨ[1],BROADCAST_SEP);
й(ref Η,out Ƨ[2],BROADCAST_SEP);й(ref Η,out Δ.ΕЁ,BROADCAST_SEP);й(ref Η,out Δ.Μ,BROADCAST_SEP);й(ref Η,out ΑΧ,BROADCAST_SEP);й(ref Η,out Δ.ΓΒ,BROADCAST_SEP);String ΘЌ="",ΘΧ="";й(ref Η,out ΘЌ,BROADCAST_SEP);й(ref Η,out ΘΧ,BROADCAST_SEP);Δ.Џ.Clear();while(true){String ΘΕ;if(!й(ref Η,out ΘΕ,BROADCAST_SEP)) break;
String[]ΒЇ=ΘΕ.Split('/');if(ΒЇ.Count()<3) continue;int Ў=0;if(!int.TryParse(ΒЇ[2],out Ў)) continue;Δ.Џ.Add(new р(ΒЇ[0],ΒЇ[1],Ў,Ы.ž));}int ΘΫ=0;if(!int.TryParse(ΑΧ,out Δ.В)) Δ.В=0;if(!int.TryParse(Αθ,out ΘΫ)) Δ.Ξ=Ν.ķ;if(!float.TryParse(Ƨ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ƨ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ƨ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(ΘЌ,out Δ.ȕ)) Δ.ȕ=0;if(!float.TryParse(ΘΧ,out Δ.ȝ)) Δ.ȝ=0;Δ.Ξ=(Ν) ΘΫ;Δ.ΔЋ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΕЏ=0;for(int i=0;i<Δ.Џ.Count();i++) Δ.ΕЏ+=Δ.Џ[i].Ў;}ϊ Γϋ(String Κ,String ΰ){Κ=Κ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Њ.Count;i++){if(Κ!=""&&Њ[i].Κ.ToUpper()==Κ) return Њ[i];if(ΰ!=""&&Њ[i].ΰ!=null&&Њ[i].ΰ.ToUpper()==ΰ) return Њ[i];}return null;}String Ƒ="";ϊ Ηΰ=null;DateTime Ηα=DateTime.MinValue;List<ϊ>ę=new List<ϊ>();const float ΙΙ=20f;bool ΖА(ϊ ε,String І){return І==Ƒ&&ε==Ηΰ&&((DateTime.Now-Ηα).TotalSeconds<ΙΙ||
(І=="HOMEPOS"&&(ą.Count>0||Ǽ!=null)));}String Ιε(ϊ ε){if(Ƒ==""||!ΖА(ε,Ƒ)) return "";String s="";int ĉ=0,Εό=0;for(int i=0;i<ę.Count;i++){Ѝ ΓΦ=ę[i].ĵ(Ƒ,false,false,false);if(ΓΦ==Ѝ.ǆ) ĉ++;bool Αμ=ą.Contains(ę[i]);if(Εό++<8) s+="  "+ę[i].ΰ+": "+(Αμ?"queued ("+(ą.IndexOf(ę[i])+1)+")":ΓΦ==Ѝ.ǆ?"received":
ΓΦ==Ѝ.Ƅ?"pending...":ΓΦ==Ѝ.ΒΛ?"no answer!":"-")+"\n";}if(Εό>8) s+="  +"+(Εό-8)+" more\n";return "--------------------------------------------\n"+Ƒ+": "+ĉ+"/"+ę.Count+" received\n"+s;}void Ǵ(ϊ Δ,String І){Ƒ=І;Ηΰ=Δ;Ηα=DateTime.Now;ę.Clear();if(Δ==null) ę.AddRange(Њ);else ę.Add(Δ);
if(Δ==null){ą.Clear();Ǽ=null;}else ą.Remove(Δ);if(Δ==null&&І=="HOMEPOS"){ΚΥ();return;}if(Δ==null){for(int i=0;i<Њ.Count;i++)
Њ[i].ĵ(І,true,false,false);ΒΣ(І,"*",І);}else{Δ.ĵ(І,true,false,false);ΒΣ(І,Δ.Κ,І);}}void ΙБ(string Ц){if(Ц=="") return;var ψ=Ц.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var І=ψ.ElementAtOrDefault(0);var ȱ=ψ.ElementAtOrDefault(1);String Ć="Invalid argument: "+Ц;if(Ϗ==Υ.ǉ){if(О!=null)
{switch(І){case "UP":{if(В<2) break;if(О.В==0){В=1;return;}Ǵ(О,"UP");return;}case "DOWN":{if(В<1) break;if(В==1){Ǵ(О,"MRES");break;}Ǵ(О,"DOWN");break;}case "APPLY":{if(В<2) break;Ǵ(О,"APPLY");return;}}}}switch(І){case "UP":this.ΔΑ(false);return;case "DOWN":this.ΓБ(false);return;
case "APPLY":this.Ź(true);return;}switch(І){case "CLEAR":ΙΤ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȱ)){Μ="Usage: SENDALL <cmd>";return;}Ǵ(null,ȱ);return;case "SEND":
if(Ц.Length<="SEND".Length+1){Μ="Usage: SEND <ship>:<cmd>";return;}ΚΟ(Ц.Remove(0,"SEND".Length+1));return;}Μ=Ć;}void ΚΟ(String ΑΕ){if(ΑΕ=="") return;var ψ=ΑΕ.Split(':');if(ψ.Length!=2){Μ="Missing separator \":\"";return;}ψ.DefaultIfEmpty("");String ƭ=ψ.ElementAtOrDefault(0).Trim();
ϊ Δ=Γϋ("",ƭ);if(Δ!=null) Ǵ(Δ,ψ.ElementAtOrDefault(1).Trim());else Μ="Unknown ship: "+ƭ;}void ΙΤ(){Њ.Clear();О=null;ξ(Υ.Main);В=0;}void ΚΜ(){List<IMyTerminalBlock>Αφ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Щ,ǟ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αφ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Щ=ǲ(Щ,pamTag,true);ΖЎ(Αφ);ΖЍ();ΚΝ();ő(Щ,setLCDFontAndSize,1.15f,false);ő(ȡ,setLCDFontAndSize,1.15f,true);String ΛΚ=Ζμ(false);String Θΰ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock э in Щ){if(э.CustomData==""){э.CustomData=Ζμ(true);continue;}
if(!э.CustomData.Contains(ΛΚ))
if(!э.CustomData.Contains(Θΰ)) э.CustomData=Θΰ+э.CustomData;}}void Κΰ(){String ΑΤ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȡ.Count;i++)
ȡ[i].WriteText(ΖΡ(Х.ΖЀ,"0",false));for(int i=0;i<Щ.Count;i++){Х Г=Х.ķ;String Ȃ="";ΙЋ(Щ[i],out Г,out Ȃ);Щ[i].WriteText(ΖΡ(Г,Ȃ,i==0));}Echo(ΑΤ+Ιη());for(int i=0;i<Ś.Count;i++){IMyTextPanel э=Ś[i];String Η=э.CustomData.ToUpper();if(Η=="DEBUG") э.WriteText(""+"\n"+"");
if(Η=="INSTRUCTIONS") э.WriteText(Ζλ());}}String Ź(bool Λ){int ΚА=0;return Ź(Λ,0,ref ΚА,false,1);}String Ź(bool Λ,int Λη,ref int Λχ,bool Ηδ,int Ακ){String Γ="";String Ƭ="——————————————————\n";String ΛІ="--------------------------------------------\n";if(О==null||Ϗ==Υ.Main||Ϗ==Υ.ΔΦ)
Γ+="[PAM]-Controller | "+Њ.Count+" Connected ships"+"\n"+Ιι()+Ιξ()+Ισ();else Γ+=Ζο(О)+"\n";Γ+=Ƭ;int Β=0;int Ι=В;ť=0;if(Μ!=""){Ο(ref Γ,Ι,Β++,Λ,Μ);}else if(Ϗ==Υ.Main){if(О==null&&Њ.Count>=1) О=Њ[0];String Б="";if(Ο(ref Б,Ι,Β++,Λ," Send command to all")) ξ(Υ.ΔΦ);
int ƫ=0;for(int s=0;s<Њ.Count;s++){if(Ηδ) Б+="\n";ϊ Δ=Њ[s];if(Ι==Β||Ι==Β+1) О=Δ;if(Β==Ι) ƫ=Б.Split('\n').Length-1;if(Ο(ref Б,Ι,Β++,Λ," "+Ζο(Δ))){О=Δ;ξ(Υ.ǉ);О.ĵ("",false,true,false);}if(Β==Ι) ƫ=Б.Split('\n').Length-1;if(Ο(ref Б,Ι,Β++,Λ," "+(Ηδ?"":"  ")+Ιπ(Δ)))
{О=Δ;ξ(Υ.ΖЏ);}}int Ʒ=Ακ-2;Ʒ+=Math.Max(0,(Λη-1))*Ακ;Γ+=ΔΥ(Ʒ,Б,ƫ,ref Λχ);}else if(Ϗ==Υ.ΒΤ||Ϗ==Υ.ΔΦ){Υ Κω=Ϗ==Υ.ΒΤ?Υ.ǉ:Υ.Main;ϊ ε=null;if(Ϗ==Υ.ΒΤ) ε=О;if(Ο(ref Γ,Ι,Β++,Λ," Back")){ξ(Κω);}if(Ο(ref Γ,Ι,Β++,Λ," [Stop] "+Ž(ε,"STOP"))){Ǵ(ε,"STOP");}if(Ο(ref Γ,Ι,Β++,Λ," [Continue] "+Ž(ε,"CONT"))){Ǵ(ε,"CONT");}
if(Ο(ref Γ,Ι,Β++,Λ," [Move home] "+Ž(ε,"HOMEPOS"))){Ǵ(ε,"HOMEPOS");}if(Ο(ref Γ,Ι,Β++,Λ," [Move to job] "+Ž(ε,"JOBPOS"))){Ǵ(ε,"JOBPOS");}if(Ο(ref Γ,Ι,Β++,Λ," [Full simulation] "+Ž(ε,"FULL"))){Ǵ(ε,"FULL");}if(ε!=null&&ε.Ξ==Ν.ί)
if(Ο(ref Γ,Ι,Β++,Λ," [Undock] "+Ž(ε,"UNDOCK"))){Ǵ(ε,"UNDOCK");}Γ+=Ιε(ε);}else if(Ϗ==Υ.ǉ){String Δτ="";if(Ο(ref Δτ,Ι,Β++,Λ," Back")){ξ(Υ.Main);}Γ+=Δτ.Substring(0,Δτ.Length-1).PadRight(36,' ');if(Ο(ref Γ,Ι,Β++,Λ," Send cmd")){ξ(Υ.ΒΤ);}if(!О.ΓЎ()&&!О.ΓЊ()) ť++;
if(О.ΓЊ()) Γ+="No answer received...";else Γ+=Ɔ(О,О.ΓΒ);}else if(Ϗ==Υ.ΖЏ){if(Ο(ref Γ,Ι,Β++,Λ," Back")){ξ(Υ.Main);}Γ+=ΛІ;Γ+=Ɔ(О,Ζξ(О));}if(!Εε) В=Math.Min(ť-1,В);return Γ;}String Ζο(ϊ Δ){if(Δ.Γβ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ɔ(Δ,Δ.ΕЁ)+" "+Δ.ΔЋ+"m";
}String Ιπ(ϊ Δ){String s=ΔΡ("",М(Δ.ȕ,Δ.ȝ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Џ.Count;i++){if(i>=5) break;р Ё=Δ.Џ[i];s+=ΗΙ(Γν(Ιν(Ё)),3)+" "+Ζε(Ё.Ў)+" ";}return s;}String Ιν(р Ё){if(Ё.ω.ToUpper()=="ORE"||Ё.ω.ToUpper()=="INGOT"){String Ηβ=Ιλ(Ё.ΰ.ToUpper());if(Ηβ!="") return Ηβ;
}return Ё.ΰ;}String Ɔ(ϊ Δ,String Γ){if(Δ.ΓЎ()) return "No signal...("+ΑА((int)(DateTime.Now-Δ.ΕΦ).TotalSeconds)+")";return Γ;}String Ζμ(bool ΜΛ){String Γ="";if(ΜΛ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΙЋ(IMyTerminalBlock э,out Х Г,out String Ȃ){bool ĉ=true;String Αθ=ΒΞ(ΑЍ(э.CustomData.Split('\n'),"mode",ref ĉ)).ToUpper();String[]ψ=Αθ.Split(':');String Αη=ψ.Length>0?ψ[0].Trim():"";Ȃ=ψ.Length>1?ψ[1].Trim():"";
Г=Х.ķ;if(Αη=="MAIN") Г=Х.ΖЁ;else if(Αη=="MAINX") Г=Х.ΖЂ;else if(Αη=="MENU") Г=Х.ΓА;else if(Αη=="INVENTORY") Г=Х.ΓЉ;else if(Αη=="DEBUG") Г=Х.Γρ;if(Г==Х.ķ) Ȃ=Αθ;}String ΖΡ(Х Г,String Ȃ,bool ΛΑ){int Αδ=15;if(ΛΑ){Ηζ=Βθ;Βθ=0;Ηη=Βι;Βι=0;}String Γ="";if(Г==Х.ΖЁ)
{int ь=0;Γ=Ź(false,Ηζ,ref ΚЂ,true,Αδ);if(!int.TryParse(Ȃ,out ь)) return Γ;Βθ=Math.Max(ь,Βθ);ь--;return ΖІ(ь,Αδ,Γ);}else if(Г==Х.ΖЂ){int ь=0;Γ=Ź(false,Ηη,ref ΚЃ,false,Αδ);if(!int.TryParse(Ȃ,out ь)) return Γ;Βι=Math.Max(ь,Βι);ь--;return ΖІ(ь,Αδ,Γ);}if(Г==Х.ΖЀ){return Ź(false,0,ref ΚЄ,true,Αδ);
}else if(Г==Х.ΓЉ||Г==Х.ΓА||Г==Х.Γρ){ϊ Δ=О;if(Ȃ!=""){Δ=Γϋ("",Ȃ);if(Δ==null) return "Unknown ship: "+Ȃ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ƭ="—————————————————————\n";String ΕЌ="";String Δϊ="";if(Г==Х.ΓА){ΕЌ="Menu";Δϊ=Ɔ(Δ,Δ.ΓΒ);}
else if(Г==Х.ΓЉ){ΕЌ="Inventory";Δϊ=Ɔ(Δ,Ζξ(Δ));}else if(Г==Х.Γρ){return Ɔ(Δ,Ιρ(Δ));}Γ+=Δ.ΰ+" - "+Δ.ΔЋ+"m | "+Γν(""+ΕЌ)+"\n"+Ƭ;Γ+=Δϊ;return Γ;}return "Unknown command: "+Ȃ;}string Ιρ(ϊ Δ){String s="";s+=Δ.Γβ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕЁ+"\n";s+=Δ.Μ+"\n";s+=Δ.В+"\n";s+=Δ.ΓΒ.Length+"\n";s+=Δ.ȕ+"\n";s+=Δ.ȝ+"\n";s+=Δ.Џ.Count()+"\n";return s;}string Ζξ(ϊ Δ){String s="";s+=ΔΡ("All",М(Δ.ȕ,Δ.ȝ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Џ.Count();i++){р Ё=Δ.Џ[i];
s+=ΔΡ(Ё.ΰ,Ё.Ў,Δ.ΕЏ,30,8,10)+"\n";}return s;}Ѝ Ιζ(ϊ Δ,bool г,string І){Ѝ α=Ѝ.None;if(Δ==null){for(int i=0;i<Њ.Count;i++){Ѝ Ї=Њ[i].ĵ(І,false,г,false);if(Ї==Ѝ.ǆ) α=Ї;if(Ї==Ѝ.ΒΛ) return Ї;if(Ї==Ѝ.Ƅ) return Ї;}return α;}else return Δ.ĵ(І,false,г,false);}String Ž(ϊ Δ,String І)
{if(!ΖА(Δ,І)) return "";Ѝ Ї=Ιζ(Δ,false,І);if(Ї==Ѝ.ǆ) return "received!";if(Ї==Ѝ.Ƅ) return "pending...";if(Ї==Ѝ.ΒΛ) return "no answer!";return "";}String ΔΥ(int Ʒ,String Γ,int ƫ,ref int ħ){String[]Π=Γ.Split('\n');if(ƫ>=ħ+Ʒ-1) ħ++;ħ=Math.Min(Π.Count()-1-Ʒ,ħ);if(ƫ<ħ+1) ħ--;
ħ=Math.Max(0,ħ);String α="";for(int i=0;i<Ʒ;i++){int ΘΞ=i+ħ;if(ΘΞ>=Π.Count()) break;α+=Π[ΘΞ]+"\n";}return α;}String ΖІ(int ь,int Ακ,String Γ){String[]Π=Γ.Split('\n');int Ασ=ь*Ακ;int ȗ=(ь+1)*Ακ;String α="";for(int i=Ασ;i<ȗ;i++){if(i>=Π.Count()) break;α+=Π[i]+"\n";
}return α;}String Γν(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΗΙ(String s,int ΘΥ){if(s=="") return s;if(s.Length>ΘΥ) s=s.Substring(0,ΘΥ-1)+".";return s;}string ΔΡ(String š,float value,float Ɵ,int ΗΧ,int ΘΙ,int Λζ){float Ŭ=М(value,Ɵ)*ΗΧ;
String Βδ="[";for(int i=0;i<ΗΧ;i++){if(i<=Ŭ) Βδ+="|";else Βδ+="'";}Βδ+="]";return Βδ+" "+ΗΙ(Γν(š),ΘΙ).PadRight(ΘΙ)+"".PadRight(Λζ)+Ζε(value);}String Ζε(float Ў){if(Ў>=1000000) return Math.Round(Ў/1000000f,Ў/1000000f<100?1:0)+"M";if(Ў>=1000) return Math.Round(Ў/1000f,Ў/1000f<100?1:0)+"K";
return ""+Math.Round(Ў);}String ΑА(int Απ){if(Απ>=60*60) return Math.Round(Απ/(60f*60f),1)+" h";if(Απ>=60) return Math.Round(Απ/60f,1)+" min";return ""+Απ+" s";}string ż(float v){return Math.Round(v,2)+" ";}string ΜΣ(Vector3 v){return "X"+ż(v.X)+"Y"+ż(v.Y)+"Z"+ż(v.Z);}
List<IMyCameraBlock>Ȓ=new List<IMyCameraBlock>();List<long>ƣ=new List<long>();bool ƥ=false;int ΔЁ=0,Λτ=0,Δψ=0;const int ΚΖ=16;int ΔϏ=0;DateTime ΗΪ=DateTime.Now;String Δω="";String Δώ="";void ΚΛ(){Ȓ.Clear();ƣ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȓ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȓ.Count;i++) Ȓ[i].EnableRaycast=true;List<IMyTerminalBlock>Βΰ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Βΰ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<Βΰ.Count;i++) if(!ƣ.Contains(Βΰ[i].CubeGrid.EntityId)) ƣ.Add(Βΰ[i].CubeGrid.EntityId);if(!ƣ.Contains(Me.CubeGrid.EntityId)) ƣ.Add(Me.CubeGrid.EntityId);
Δώ="";if(Ω==null) return;MatrixD m=Ω.WorldMatrix;Vector3[]ΚЋ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Λέ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ȓ.Count;i++)
if(Vector3.Dot(Ȓ[i].WorldMatrix.Forward,ΚЋ[d])>0.7f){Δώ+=Λέ[d]+" ";break;}}bool Ζϋ(Α s){return s==Α.Ȍ||s==Α.ч||s==Α.Л||
s==Α.ĳ||s==Α.Ż||s==Α.ΑЎ||
(s==Α.ȋ&&Ǩ!=null);}void ΑЉ(){ƥ=false;ΔЁ=0;Δω="";}void Κι(){if(!obstacleBrake||Ȓ.Count==0||!Ζϋ(Ϋ)){ΑЉ();return;}Vector3 ΑΨ=Ѓ+Ĭ;Vector3 ȣ=ΑΨ-δ;float Ȇ=ȣ.Length();if(Ȇ<1f){ΑЉ();return;}Vector3 π=ȣ/Ȇ;Vector3 в=Ω!=null?(Vector3) Ω.GetShipVelocities().LinearVelocity:new Vector3();
if(в.Length()>2) π=Vector3.Normalize(в);Vector3 ΓΡ=Vector3.Cross(π,Math.Abs(π.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΓΡ.Normalize();Vector3 Ј=Vector3.Cross(ΓΡ,π);float υ=Math.Min(Ȇ+μ*0.25f,Ĩ*obstacleLookAhead+μ);int Ηέ=0;for(int k=0;k<2;k++){int p=k==0?0:Λτ++%8,e=p%4;
Vector3 Λβ=k==0?new Vector3():(e==0?ΓΡ:e==1?-ΓΡ:e==2?Ј:-Ј)*μ*0.5f;Vector3 ȗ=δ+π*(p>=4?υ*0.5f:υ)+Λβ;IMyCameraBlock ė=ΙΝ(ȗ);if(ė==null){Ζά();continue;}Ηέ++;MyDetectedEntityInfo Ǒ=ė.Raycast(ȗ);bool ΜΕ=Ǒ.Type==MyDetectedEntityType.Planet||Ǒ.Type==MyDetectedEntityType.Asteroid;
bool Θα=!Ǒ.IsEmpty()&&!ƣ.Contains(Ǒ.EntityId)&&!ΙЃ(Ǒ.EntityId)&&
Ǒ.Type!=MyDetectedEntityType.FloatingObject&&!(ΜΕ&&(k>0||Ϋ==Α.Ȍ||
(Ǒ.HitPosition.HasValue&&Vector3D.Distance(Ǒ.HitPosition.Value,ΑΨ)<μ)));if(Θα){if(!ƥ) ΗΪ=DateTime.Now;ƥ=true;ΔЁ=0;Δω=Ǒ.Type.ToString();}else Ζά();if(debugDraw&&Βσ.Count<16) Βσ.Add(ο(ė.GetPosition())+","+ο(ȗ)+","+(Θα?1:0));}Δψ=Ηέ>0?0:Δψ+1;if(Δψ>12) Μ="Cameras can't see ahead";
if(ƥ&&(DateTime.Now-ΗΪ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+Δω;}void Ζά(){if(ƥ&&++ΔЁ>=ΚΖ) ΑЉ();}IMyCameraBlock ΙΝ(Vector3 ȗ){IMyCameraBlock Љ=null;float ΗΩ=-2;for(int n=0;n<Ȓ.Count;n++){ΔϏ=(ΔϏ+1)%Ȓ.Count;IMyCameraBlock c=Ȓ[ΔϏ];if(!c.IsFunctional||!c.IsWorking) continue;float Θν=Vector3.Dot(Vector3.Normalize(ȗ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Θν>ΗΩ+0.05f&&c.CanScan(ȗ)){Љ=c;ΗΩ=Θν;}}return Љ;}const String Δβ="[PAMTFC]";const float ΚΪ=5f;const float Γϊ=2f;class Ķ{public long Κ;public Vector3 ά,в;public Vector3 ț;public float Ηρ;public String Θ="";public int ĝ=-1;public bool Αμ;public int Ǿ;public float ΑΪ,ΑΦ;public long β=0;
public int ΑΟ=0;public DateTime Ȣ;}IMyBroadcastListener Γέ=null;long љ=0;List<Ķ>ќ=new List<Ķ>();Vector3 Ĭ=new Vector3();float ǜ=-1;Vector3 ΕА=new Vector3();bool Γά=false;DateTime ΕБ=DateTime.Now;const float ΗΘ=15f;const float ΚΩ=10f;int ΘЉ=0;String ж="";int Ж=-1;bool Βμ=false,Βλ=false;
Vector3 Ηθ=new Vector3();String ƀ(){if(ζ==Σ.щ||Ξ==Ν.ί) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+А+"|"+Я+"|"+(int) Š+"|"+Math.Round(Ȉ,1)+"|"+Math.Round(ȥ,1);}void Κξ(){if(!trafficEnabled) return;if(Γέ==null) Γέ=IGC.RegisterBroadcastListener(Δβ);љ=Me.CubeGrid.EntityId;int Ǣ=30;while(Γέ.HasPendingMessage&&Ǣ-->0){String Η=Γέ.AcceptMessage().Data as String;if(Η!=null) ΙЍ(Η);
}for(int i=ќ.Count-1;i>=0;i--)
if((DateTime.Now-ќ[i].Ȣ).TotalSeconds>ΚΪ) ќ.RemoveAt(i);if(ΓΩ==null) ΓΩ=IGC.RegisterBroadcastListener(ΗΖ);Ǣ=5;while(ΓΩ.HasPendingMessage&&Ǣ-->0){String Η=ΓΩ.AcceptMessage().Data as String;if(Η!=null) try{ΙЎ(Η);}catch{}}if(Βν==null) Βν=IGC.RegisterBroadcastListener(Ζύ);
Ǣ=10;while(Βν.HasPendingMessage&&Ǣ-->0){String Η=Βν.AcceptMessage().Data as String;if(Η!=null) try{ΙЊ(Η);}catch{}}if(++ΘЉ<3) return;ΘЉ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",љ+";"+ο(Ζ.Ε)+";"+
ο(ζ==Σ.щ?new Vector3():Ζ.Ђ)+";"+ο(Ĉ)+";"+
ο(Ѓ+Ĭ)+";"+String.Join(":",Βσ));Βσ.Clear();}IGC.SendBroadcastMessage(Δβ,љ+";"+ο(δ)+";"+
ο(Ω.GetShipVelocities().LinearVelocity)+";"+μ+";"+ƀ()+";"+
(ζ==Σ.К?Ж:-1)+";"+ж+";"+ΙΣ()+","+Ζα()+";"+ο(ΕА)+
";"+(Βχ?1:0)+";"+(ŀ&&ζ==Σ.К?1:0)+","+ΓΞ+","+Εϊ+","+ΙІ());}void ΙЍ(String Η){try{String[]f=Η.Split(';');if(f.Length<7) return;long Κ=long.Parse(f[0]);if(Κ==љ) return;Ķ Δ=null;for(int i=0;i<ќ.Count;i++) if(ќ[i].Κ==Κ) Δ=ќ[i];if(Δ==null){Δ=new Ķ();Δ.Κ=Κ;ќ.Add(Δ);}
Δ.ά=θ(f[1]);Δ.в=θ(f[2]);Δ.Ηρ=float.Parse(f[3]);Δ.Θ=f[4];Δ.ĝ=int.Parse(f[5]);Δ.Ȣ=DateTime.Now;if(Δ.Θ!=""&&Δ.Θ==ƀ()) ΖЃ(f[6]);Δ.β=0;Δ.ΑΟ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.β=long.Parse(d[0]);Δ.ΑΟ=int.Parse(d[1]);}Δ.ț=f.Length>=9?θ(f[8]):new Vector3();
Δ.Αμ=f.Length>=10&&f[9]=="1";Δ.Ǿ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ΑΪ=float.Parse(w[1]);Δ.ΑΦ=float.Parse(w[2]);Δ.Ǿ=int.Parse(w[3]);}}}catch{}}bool ΙЃ(long Κ){for(int i=0;i<ќ.Count;i++) if(ќ[i].Κ==Κ) return true;return false;}Vector3 Ζν(int Β,int Λθ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();έ ή=Ψ[Β];Vector3 ΜΒ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Λθ;Vector3 Ј=ή.ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ή.ϋ):-ή.ι;Vector3 ф=Vector3.Cross(ΜΒ,Ј);if(ф.LengthSquared()<0.0001f) return new Vector3();
float υ=laneOffset*μ;float ΛΓ=Math.Min(Vector3.Distance(ή.Ε,Ψ[0].Ε),
Vector3.Distance(ή.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(ф)*υ*Math.Min(1,ΛΓ/(υ*4));}Vector3 ΘЈ=new Vector3();List<String>Βσ=new List<String>();bool Ζτ(){Α s=Ϋ;return ƀ()!=""&&(s==Α.Ȍ||s==Α.Ƃ||s==Α.ȭ||
s==Α.Ȭ||s==Α.ź);}int ΙІ(){if(!Ζτ()) return 0;return Ϋ!=Α.Ȍ&&Vector3.Dot(δ-ј,Ǚ)>μ*0.5f?2:1;}bool ΙЂ(Α s){return s==Α.ȋ||s==Α.Ŋ||s==Α.Ŏ||s==Α.Ǉ||
s==Α.Ƕ||s==Α.ȭ||s==Α.Ȭ||s==Α.ź||s==Α.į;}void Κο(){Ĭ=new Vector3();ǜ=-1;ΕА=new Vector3();bool ΑΩ=Ζϋ(Ϋ);if(!trafficEnabled||(!ΑΩ&&!ΙЂ(Ϋ))){Γά=false;return;}ΘЈ=Ω.GetShipVelocities().LinearVelocity;ΙΥ(Ω.GetNaturalGravity(),!ΑΩ);ΙΑ();}IMyBroadcastListener ΓΩ=null;
float Ʊ=-1;DateTime ΘЄ=DateTime.MinValue;void ΙЎ(String Η){if(!Η.StartsWith("C;")) return;ΘЄ=DateTime.Now;Ʊ=-1;String Αζ=љ+":";String[]e=Η.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αζ)) Ʊ=float.Parse(e[i].Substring(Αζ.Length));}void ΙΑ(){if(Ʊ<0||(DateTime.Now-ΘЄ).TotalSeconds>1.5) return;
ǜ=ǜ<0?Ʊ:Math.Min(ǜ,Ʊ);if(Ʊ<0.5f) Μ="Traffic: giving way (controller)";}void ΙΥ(Vector3 ϋ,bool Θζ=false){Ĭ=new Vector3();ǜ=-1;Vector3 ȣ=Ѓ-δ;float Ȇ=ȣ.Length();bool ΕΜ=Ȇ>=1f;Vector3 π=ΕΜ?ȣ/Ȇ:new Vector3();ΕА=π;Vector3 Ј=ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ϋ):-ΑΡ;
Vector3 ф=Vector3.Cross(π,Ј);ф=ф.LengthSquared()<0.0001f?-Αγ:Vector3.Normalize(ф);Ķ ΒЉ=null;bool ΗЂ=Ϋ==Α.Л&&!Ĕ(1);for(int i=0;i<ќ.Count;i++){Ķ λ=ќ[i];if(λ.ΑΟ==3&&λ.в.LengthSquared()<0.25f) continue;Vector3 ά=λ.ά+λ.в*(float)(DateTime.Now-λ.Ȣ).TotalSeconds;Vector3 Ǫ=ά-δ;
float г=(μ+λ.Ηρ)*0.5f;bool Αξ=λ.Θ!=""&&λ.Θ==ƀ();if(Αξ&&λ.Ǿ==2) continue;bool ΓΣ=Αξ&&λ.Ǿ==1&&Ζτ();if(ΓΣ) г=Math.Min(Ȉ,ȥ)*0.9f;float ΔЅ=г*(ΓΣ?1f:1.2f);float ΑΔ=ΕΜ?Vector3.Dot(Ǫ,π):0;if(ΑΔ>0&&(Ǫ-π*ΑΔ).Length()<=ΔЅ){float ΓΗ=Vector3.Dot(λ.в,π);
bool ΗІ=λ.ț.LengthSquared()>0.25f;bool ΗЈ=ΓΗ<-1f||(ΗІ&&Vector3.Dot(λ.ț,π)<-0.5f);if(!Θζ){if(λ.Αμ){if(!ΗЂ){Ĭ=ф*г*1.3f;continue;}float ΘΪ=Vector3.Distance(δ,Ѓ),ΘΖ=Vector3.Distance(ά,Ѓ);if(ΘΪ<ΘΖ||(ΘΪ==ΘΖ&&љ<λ.Κ)) continue;}if(ΗЈ){if(ΑΔ<(Ĩ-ΓΗ)*6f+г*4f) Ĭ=ф*г*1.3f;
continue;}if(ΗІ&&љ<λ.Κ){Vector3 ΗΥ=-Ǫ;float Θδ=Vector3.Dot(ΗΥ,λ.ț);if(Θδ>0&&(ΗΥ-λ.ț*Θδ).Length()<=ΔЅ){Ĭ=ф*г*1.3f;continue;}}}float ΗЁ=ΑΔ-г-(ΓΣ?1f:followGap*μ);float ǖ=Math.Max(0,ΓΗ)+Math.Max(0,ΗЁ)/Γϊ;if(ΗЈ) ǖ=Math.Max(0,ΗЁ+Math.Min(0,ΓΗ)*Γϊ*2)/Γϊ;
if(ǜ<0||ǖ<ǜ){ǜ=ǖ;ΒЉ=λ;}continue;}if(λ.в.LengthSquared()<1f) continue;if(ΕΜ&&(Ǫ-π*ΑΔ).Length()<=ΔЅ) continue;Vector3 Εψ=λ.в-ΘЈ;float Θμ=Εψ.LengthSquared();if(Θμ<0.25f) continue;float t=-Vector3.Dot(Ǫ,Εψ)/Θμ;if(t<=0||t>5f||(Ǫ+Εψ*t).Length()>=(ΓΣ?г:г*1.1f)) continue;
if(љ>λ.Κ||t<2f){ǜ=0;ΒЉ=λ;}}bool Αή=ǜ>=0&&ǜ<0.5f;if(!Αή||Θζ||(ΒЉ!=null&&ΒЉ.Αμ&&ΗЂ)){Γά=false;return;}if(!Γά){Γά=true;ΕБ=DateTime.Now;}double ΗЉ=(DateTime.Now-ΕБ).TotalSeconds;if(ΗЉ>ΗΘ+ΚΩ) ΕБ=DateTime.Now;else if(ΗЉ>ΗΘ){ǜ=-1;Ĭ=ф*μ*1.3f;Μ="Traffic: passing a stuck ship";}
}List<int>Ą=new List<int>();String ΗЋ="";void Γω(){String ы=А+"|"+Я+"|"+(int) Š;if(ы==ΗЋ) return;ΗЋ=ы;ΖΣ(А,Я,Š,Ą);}void ΖΣ(int w,int h,ѓ Ασ,List<int>ΔЀ){ΔЀ.Clear();if(Ασ==ѓ.ǌ){for(int k=0;k<w*h;k++){int ņ=k/w;ΔЀ.Add(ņ*w+(ņ%2==0?k-ņ*w:w-1-(k-ņ*w)));
}return;}int Αά=(int) Math.Ceiling(w/2f),ΑΫ=(int) Math.Ceiling(h/2f);int ΕΙ=w/2,ΕΘ=h/2;int x=0,y=0,ΔЎ=0,ΔЏ=-1,Ɵ=Math.Max(w,h);for(int i=0;i<Ɵ*Ɵ;i++){if(-Αά<x&&x<=ΕΙ&&-ΑΫ<y&&y<=ΕΘ) ΔЀ.Add((y-1+ΑΫ)*w+(x-1+Αά));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔЎ;ΔЎ=-ΔЏ;ΔЏ=t;}
x+=ΔЎ;y+=ΔЏ;}}bool Γψ(){int n=Math.Max(0,А*Я);if(ж.Length==n) return false;ж=new String('0',n);Ж=-1;return true;}void ΖЃ(String λ){Γψ();if(λ.Length!=ж.Length) return;char[]h=ж.ToCharArray();for(int i=0;i<h.Length;i++) if(λ[i]=='1') h[i]='1';ж=new String(h);}bool ΒΔ(int Ь){return Ь>=0&&Ь<ж.Length&&ж[Ь]=='1';}
void ΔΨ(int Ь,bool φ){if(Ь<0||Ь>=ж.Length) return;char[]h=ж.ToCharArray();h[Ь]=φ?'1':'0';ж=new String(h);}bool ΓЅ(int Ь,bool ΛΡ){if(!trafficEnabled) return false;String ы=ƀ();for(int i=0;i<ќ.Count;i++){Ķ t=ќ[i];if(t.Θ!=ы||t.ĝ<0||(ΛΡ&&t.Κ>љ)) continue;if(t.ĝ==Ь) return true;
}return false;}int ΙΡ(int Β){int n=А*Я;if(n<1) return Β;Γω();if(Γψ()) for(int k=0;k<Β&&k<n;k++) ΔΨ(Ą[k],true);if(Ж>=0&&(Β>=n||Ą[Β]!=Ж)){ΔΨ(Ж,true);Ж=-1;}if(Ζώ()){if(Ȕ>=0&&!ΒΔ(Ȕ)){ŀ=false;Ж=Ȕ;return Ą.IndexOf(Ȕ);}if(Ȕ==-2){Ж=-1;ŀ=ж.IndexOf('0')>=0;return ŀ?-2:n;}
if(Ȕ==-1&&Ж>=0&&!ΒΔ(Ж)){ŀ=false;return Β;}Ж=-1;if(!ŀ){ŀ=true;Ηκ=DateTime.Now;}if((DateTime.Now-Ηκ).TotalSeconds<5) return-1;}ŀ=false;if(Ж>=0&&!ΒΔ(Ж)&&!ΓЅ(Ж,true)) return Β;Ж=-1;for(int k=0;k<n;k++){int c=Ą[k];if(ΒΔ(c)||ΓЅ(c,false)) continue;Ж=c;
return k;}return ж.IndexOf('0')>=0?-2:n;}const String Ζύ="[PAMJOB]";const String ΗΖ="[PAMTCC]";IMyBroadcastListener Βν=null;DateTime Ηι=DateTime.MinValue;DateTime Ηκ=DateTime.Now;int Ȕ=-1;bool ŀ=false;bool Ζώ(){return trafficEnabled&&Ȕ!=-3&&(DateTime.Now-Ηι).TotalSeconds<3;}
void ΙЊ(String Η){String[]f=Η.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƀ()) return;Ηι=DateTime.Now;ΖЃ(f[2]);Ȕ=-3;String Αζ=љ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αζ)) Ȕ=int.Parse(e[i].Substring(Αζ.Length));}ǵ ΚΠ(int Β)
{Γω();if(Β<0||Β>=Ą.Count) return ǵ.ΑЃ;Є=Ą[Β]%А;Ѐ=Ą[Β]/А;return ǵ.ΑБ;}class Ǡ{public long Κ;public Vector3 ά,Ю,Ј;}List<Ǡ>Ч=new List<Ǡ>();bool ļ=false;Vector3 Βω,Βψ,Βϊ,Βϋ;long Ǔ=0;long Ċ=0;έ Ǩ=null;bool ΑΠ=false;έ Γυ(Vector3 ά,Vector3 Ю,Vector3 Ј){MatrixD m=MatrixD.CreateWorld(ά,Ю,Ј);
έ w=new έ(Ρ);w.κ=Vector3D.TransformNormal(Βω,m);w.ι=Vector3D.TransformNormal(Βψ,m);w.Ђ=Vector3D.TransformNormal(Βϊ,m);w.Ε=ά+(Vector3) Vector3D.TransformNormal(Βϋ,m);w.ν=true;return w;}int Ζα(){Α s=Ϋ;if(s==Α.ђ) return к(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ŏ||s==Α.Ȱ||s==Α.ō||s==Α.Βέ||
s==Α.ΑЌ||s==Α.Ƕ) return 3;if(s==Α.ȋ||s==Α.Ŋ||s==Α.Ǉ) return 2;if(s==Α.Л) return 1;return 0;}long ΙΣ(){if(Ζα()==0) return 0;if(Ĕ(1)&&Ʈ!=0) return Ʈ;return Ǩ!=null?Ċ:Ǔ;}void ΖϏ(){if(Ξ==Ν.ί||!Ρ.ν) return;IMyShipConnector Т=к(MyShipConnectorStatus.Connected);if(Т==null||Т.OtherConnector==null) return;
IMyShipConnector λ=Т.OtherConnector;Vector3I ΛΛ=new Vector3I((int) Ρ.т.X,(int) Ρ.т.Y,(int) Ρ.т.Z);if(Т.Position==ΛΛ){MatrixD Ƙ=MatrixD.Transpose(λ.WorldMatrix);Βω=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,Ƙ);Βψ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,Ƙ);Βϊ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,Ƙ);
Βϋ=Vector3D.TransformNormal(Т.GetPosition()-λ.GetPosition(),Ƙ);ļ=true;}if(Vector3.Distance(Т.GetPosition(),Ρ.Ε)<5) Ǔ=λ.EntityId;List<IMyShipConnector>Ŝ=new List<IMyShipConnector>();χ.GetBlocksOfType(Ŝ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ŝ.Count>0){Ч.Clear();for(int i=0;i<Ŝ.Count;i++){Ǡ p=new Ǡ();p.Κ=Ŝ[i].EntityId;p.ά=Ŝ[i].GetPosition();p.Ю=Ŝ[i].WorldMatrix.Forward;p.Ј=Ŝ[i].WorldMatrix.Up;Ч.Add(p);}}Ǩ=null;Ċ=0;if(λ.EntityId==Ǔ||!ļ) return;for(int i=0;i<Ч.Count;i++)
if(Ч[i].Κ==λ.EntityId){Ċ=Ч[i].Κ;Ǩ=Γυ(Ч[i].ά,Ч[i].Ю,Ч[i].Ј);}}bool ΓЌ(long Κ,int Θέ){if(Κ==0) return false;for(int i=0;i<ќ.Count;i++){Ķ t=ќ[i];if(t.β==Κ&&(t.ΑΟ>Θέ||(t.ΑΟ==Θέ&&t.Κ<љ))) return true;}return false;}bool Γο(int ū){if(Ξ==Ν.ί||!trafficEnabled){Ǩ=null;Ċ=0;return true;}
long σ=Ǩ!=null?Ċ:Ǔ;if(!ΓЌ(σ,ū)) return true;if(!ΓЌ(Ǔ,ū)){Ǩ=null;Ċ=0;return true;}if(!ļ) return false;Ǡ Љ=null;float ΑΙ=0;for(int i=0;i<Ч.Count;i++){Ǡ p=Ч[i];if(p.Κ==Ǔ||ΓЌ(p.Κ,ū)) continue;float d=Vector3.Distance(p.ά,Ρ.Ε);if(Љ==null||d<ΑΙ){Љ=p;ΑΙ=d;}
}if(Љ==null) return false;Ċ=Љ.Κ;Ǩ=Γυ(Љ.ά,Љ.Ю,Љ.Ј);return true;}String Ιί(){String s=(ļ?"1":"0")+":"+ο(Βω)+":"+ο(Βψ)+":"+
ο(Βϊ)+":"+ο(Βϋ)+":"+Ǔ;for(int i=0;i<Ч.Count;i++)
s+=":"+Ч[i].Κ+","+ο(Ч[i].ά)+","+ο(Ч[i].Ю)+","+ο(Ч[i].Ј);return s;}void Ιή(String s){Ч.Clear();ļ=false;Ǔ=0;String[]f=s.Split(':');if(f.Length<6) return;ļ=f[0]=="1";Βω=θ(f[1]);Βψ=θ(f[2]);Βϊ=θ(f[3]);Βϋ=θ(f[4]);long.TryParse(f[5],out Ǔ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǡ p=new Ǡ();if(!long.TryParse(e[0],out p.Κ)) continue;p.ά=θ(e[1]);p.Ю=θ(e[2]);p.Ј=θ(e[3]);Ч.Add(p);}}const String ƽ="[PAMDCK]";const float ΙΘ=120f;class Đ{public long Δ;public int Ǧ;public long ǒ;public bool Ηά;public DateTime Ǥ,ƛ;
}IMyBroadcastListener Βφ=null;List<Đ>Ш=new List<Đ>();List<IMyShipConnector>ĩ=new List<IMyShipConnector>();List<IMySensorBlock>Ȋ=new List<IMySensorBlock>();List<IMyCameraBlock>Ģ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Γγ=new List<MyDetectedEntityInfo>();long є=0;int Βη=0;
long Βζ=0;DateTime Ηί=DateTime.Now;String Ě="";int Ηφ=0;IMyCubeGrid Ī=null;void ΚΝ(){Ī=Me.CubeGrid;χ.GetBlocksOfType(ĩ,q=>q.CubeGrid.IsSameConstructAs(Ī)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȋ,q=>q.CubeGrid.IsSameConstructAs(Ī)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(Ģ,q=>q.CubeGrid.IsSameConstructAs(Ī)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȋ.Count;i++){Ȋ[i].DetectSmallShips=true;Ȋ[i].DetectLargeShips=true;}for(int i=0;i<Ģ.Count;i++) Ģ[i].EnableRaycast=true;}IMyShipConnector Δί(long Κ){if(Κ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Κ) as IMyShipConnector;return c!=null&&Ī!=null&&c.CubeGrid.IsSameConstructAs(Ī)?c:null;}bool ΓЋ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Ζό(IMyShipConnector c,long Δ){for(int i=0;i<Ģ.Count;i++)
{IMyCameraBlock ė=Ģ[i];if(!ė.IsWorking||Vector3D.Distance(ė.GetPosition(),c.GetPosition())>10) continue;Vector3D ȗ=ė.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ė.CanScan(ȗ)) continue;MyDetectedEntityInfo Ǒ=ė.Raycast(ȗ);if(!Ǒ.IsEmpty()&&Ǒ.EntityId!=Ī.EntityId&&Ǒ.EntityId!=Δ&&Ǒ.Type!=MyDetectedEntityType.FloatingObject)
{Ě="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙЄ(){for(int s=0;s<Ȋ.Count;s++){if(!Ȋ[s].IsWorking) continue;Γγ.Clear();Ȋ[s].DetectedEntities(Γγ);for(int i=0;i<Γγ.Count;i++){MyDetectedEntityInfo e=Γγ[i];bool ΛΖ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΛΖ&&e.EntityId!=є&&e.Velocity.Length()>0.5f){Ě="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΙЏ(Đ r){IMyShipConnector ǒ=Δί(r.ǒ);if(ΓЋ(ǒ)&&Ζό(ǒ,r.Δ)) return ǒ.EntityId;if(!r.Ηά||ǒ==null) return 0;IMyShipConnector Љ=null;double ΑΙ=0;for(int i=0;i<ĩ.Count;i++)
{IMyShipConnector c=ĩ[i];if(!ΓЋ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ǒ.GetPosition());if(Љ==null||d<ΑΙ){if(Ζό(c,r.Δ)){Љ=c;ΑΙ=d;}}}return Љ==null?0:Љ.EntityId;}void Ιτ(){Đ Ʀ=null;long Т=0;for(int ũ=2;ũ>=1&&Ʀ==null;ũ--){Đ Ũ=null;for(int i=0;i<Ш.Count;i++)
{Đ r=Ш[i];if(r.Ǧ!=ũ) continue;if(ũ==1){long c=ΙЏ(r);if(c==0) continue;if(Ũ==null||r.Ǥ<Ũ.Ǥ){Ũ=r;Т=c;}}else if(Ũ==null||r.Ǥ<Ũ.Ǥ) Ũ=r;}Ʀ=Ũ;}if(Ʀ==null){if(Ш.Count>0&&Ě=="") Ě="No free dock";return;}є=Ʀ.Δ;Βη=Ʀ.Ǧ;Βζ=Ʀ.Ǧ==1?Т:0;Ηί=DateTime.Now;
}void Κγ(){if(Ī==null) return;if(Βφ==null) Βφ=IGC.RegisterBroadcastListener(ƽ);int Ǣ=30;while(Βφ.HasPendingMessage&&Ǣ-->0){String Η=Βφ.AcceptMessage().Data as String;if(Η!=null) Ιφ(Η);}ΚΕ();if(++Ηφ<3) return;Ηφ=0;IGC.SendBroadcastMessage(ƽ,ΙΖ());}void Ιφ(String Η){try{String[]f=Η.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ш.Count-1;i>=0;i--) if(Ш[i].Δ==Δ) Ш.RemoveAt(i);if(є==Δ) є=0;}else if(f[0]=="R"&&f.Length>=5){long ǒ=long.Parse(f[3]);if(Δί(ǒ)==null) return;Đ r=null;for(int i=0;i<Ш.Count;i++) if(Ш[i].Δ==Δ) r=Ш[i];if(r==null){r=new Đ();r.Δ=Δ;r.Ǥ=DateTime.Now;Ш.Add(r);}
int Ǧ=int.Parse(f[2]);if(r.Ǧ!=Ǧ) r.Ǥ=DateTime.Now;r.Ǧ=Ǧ;r.ǒ=ǒ;r.Ηά=f[4]=="1";r.ƛ=DateTime.Now;}}catch{}}void ΚΕ(){for(int i=Ш.Count-1;i>=0;i--)
if((DateTime.Now-Ш[i].ƛ).TotalSeconds>3) Ш.RemoveAt(i);Đ Βε=null;for(int i=0;i<Ш.Count;i++) if(Ш[i].Δ==є) Βε=Ш[i];if(є!=0&&Βε==null) є=0;if(Βε!=null&&(DateTime.Now-Ηί).TotalSeconds>ΙΘ){є=0;Βε.Ǥ=DateTime.Now;}Ě="";if(є==0&&Ш.Count>0&&!ΙЄ()) Ιτ();}
String ΙΖ(){String γ="S;"+Ī.EntityId+";"+є+";"+Βη+";"+Βζ+";";for(int i=0;i<Ш.Count;i++) γ+=(i>0?",":"")+Ш[i].Δ;γ+=";";for(int i=0;i<ĩ.Count;i++){IMyShipConnector c=ĩ[i];γ+=(i>0?":":"")+c.EntityId+","+ο(c.GetPosition())+","+ο(c.WorldMatrix.Forward)+","+ο(c.WorldMatrix.Up);
}γ+=";";for(int i=0;i<Ȋ.Count;i++){IMySensorBlock z=Ȋ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();γ+=(i>0?":":"")+ο(z.GetPosition())+","+Math.Round(r,1);}return γ;}String Ιι()
{if(ĩ.Count==0&&Ȋ.Count==0) return "";int ΑΩ=0;for(int i=0;i<ĩ.Count;i++) if(ΓЋ(ĩ[i])) ΑΩ++;return "Docks: "+ΑΩ+"/"+ĩ.Count+" free"+(є!=0?(Βη==1?" | docking":" | undocking"):"")+
(Ш.Count>1?" | "+(Ш.Count-1)+" waiting":"")+(Ě!=""?"\n"+Ě:"")+"\n";}IMyBroadcastListener ΓΧ=null;int ă=0;DateTime Ηυ=DateTime.Now;DateTime Θτ=DateTime.MinValue;long Θυ=0;int Θφ=0;long Ʈ=0;bool ΑϏ=false;int ΔЍ=0;List<Vector3>ΑЁ=new List<Vector3>();List<float>ΖΞ=new List<float>();
bool Βχ=false;bool Βυ=false;Vector3 Ητ=new Vector3();bool Ǌ(){return ă!=0&&(DateTime.Now-Θτ).TotalSeconds<3;}bool Ĕ(int Ǧ){return Ǌ()&&Θυ==љ&&Θφ==Ǧ;}bool ΗΒ(){return Ǌ()||(DateTime.Now-Ηυ).TotalSeconds<3;}void ΔΤ(int Ǧ){if(ă==Ǧ) return;ă=Ǧ;Ηυ=DateTime.Now;ΔЍ=3;
}void ΔΣ(){if(ă!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƽ,"D;"+љ);ă=0;ΑϏ=false;}void Κβ(){if(!trafficEnabled||Ξ==Ν.ί) return;if(ΓΧ==null) ΓΧ=IGC.RegisterBroadcastListener(ƽ);int Ǣ=20;while(ΓΧ.HasPendingMessage&&Ǣ-->0){String Η=ΓΧ.AcceptMessage().Data as String;if(Η==null||!Η.StartsWith("S;")) continue;
try{ΙЌ(Η);}catch{}}if(Ϋ!=Α.Л){Βχ=false;Βυ=false;}if(ă==1&&к(MyShipConnectorStatus.Connected)!=null) ΔΣ();if(ă==2&&!ΑϏ&&Ϋ!=Α.Ƕ&&
Vector3.Distance(δ,ё().Ε)>μ*3) ΔΣ();if(ă!=0&&++ΔЍ>=3){ΔЍ=0;IGC.SendBroadcastMessage(ƽ,"R;"+љ+";"+ă+";"+Ǔ+";"+(ļ?1:0));}}void ΙЌ(String Η){String[]f=Η.Split(';');if(f.Length<7) return;bool ΗЀ=false;String[]ΗΛ=f[5].Split(',');for(int i=0;i<ΗΛ.Length;i++) if(ΗΛ[i]==љ.ToString()) ΗЀ=true;
if(!ΗЀ) return;Θτ=DateTime.Now;Θυ=long.Parse(f[2]);Θφ=int.Parse(f[3]);Ʈ=long.Parse(f[4]);ΑЁ.Clear();ΖΞ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑЁ.Add(θ(e[0]));ΖΞ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ηψ=f[6].Split(':');List<Ǡ>Б=new List<Ǡ>();for(int i=0;i<Ηψ.Length;i++){String[]e=Ηψ[i].Split(',');if(e.Length<4) continue;Ǡ p=new Ǡ();p.Κ=long.Parse(e[0]);p.ά=θ(e[1]);p.Ю=θ(e[2]);p.Ј=θ(e[3]);Б.Add(p);}Ч=Б;}bool ΙΟ(){if(!trafficEnabled||Ξ==Ν.ί||Ǔ==0) return true;
ΔΤ(1);if(!ΗΒ()) return true;if(!Ĕ(1)){Μ="Waiting for dock clearance";return false;}Γη();return true;}bool Γη(){ΑΠ=false;Ǡ Ū=null;if(Ʈ!=0&&Ʈ!=Ǔ&&ļ)
for(int i=0;i<Ч.Count;i++) if(Ч[i].Κ==Ʈ) Ū=Ч[i];long Ƹ=Ū==null?0:Ū.Κ;if(Ƹ==Ċ&&(Ƹ==0)==(Ǩ==null)) return false;Ċ=Ƹ;Ǩ=Ū==null?null:Γυ(Ū.ά,Ū.Ю,Ū.Ј);return true;}bool ſ(Vector3 p){for(int i=0;i<ΑЁ.Count;i++) if(Vector3.Distance(p,ΑЁ[i])<ΖΞ[i]+μ*0.75f) return true;
return false;}bool Ιϊ(Vector3 ΑΝ,Vector3 ŧ){Βχ=false;if(!trafficEnabled||Ξ==Ν.ί||Ǔ==0) return false;if(ă!=1&&Vector3.Distance(δ,Ρ.Ε)>dockRequestDistance) return false;ΔΤ(1);if(!Ǌ()||Ĕ(1)||ΑЁ.Count==0) return false;if(!ſ(ΑΝ)&&!ſ(ŧ)&&!ſ(δ)) return false;Βχ=true;return true;
}bool ΖΪ(){if(!trafficEnabled||Ξ==Ν.ί||Ǔ==0) return true;ΔΤ(2);if(!ΗΒ()) return true;if(!Ĕ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΙЅ=600f;class ȫ{public long Κ;public int Ь=-1;public bool Ƹ;public float ΑΪ,ΑΦ;public DateTime Εω;}class ȍ{public String ы;
public int w,h;public float ΖΒ,ΕЇ;public String φ;public List<int>ΓΖ=new List<int>();public List<ȫ>х=new List<ȫ>();}List<ȍ>ǔ=new List<ȍ>();IMyBroadcastListener ΒЈ=null;int ΘΗ=0;void Κη(){if(ΒЈ==null) ΒЈ=IGC.RegisterBroadcastListener(Δβ);int Ǣ=50;while(ΒЈ.HasPendingMessage&&Ǣ-->0)
{String Η=ΒЈ.AcceptMessage().Data as String;if(Η==null) continue;try{Ιψ(Η);}catch{}try{Ιω(Η);}catch{}}for(int j=ǔ.Count-1;j>=0;j--){ȍ Θ=ǔ[j];for(int i=Θ.х.Count-1;i>=0;i--)
if((DateTime.Now-Θ.х[i].Εω).TotalSeconds>ΙЅ) Θ.х.RemoveAt(i);if(Θ.х.Count==0){ǔ.RemoveAt(j);continue;}ΙΒ(Θ);}if(++ΘΗ<3) return;ΘΗ=0;for(int j=0;j<ǔ.Count;j++) IGC.SendBroadcastMessage(Ζύ,ΙΕ(ǔ[j]));}void ΙΒ(ȍ Θ){for(int i=0;i<Θ.х.Count;i++){ȫ s=Θ.х[i];if(s.Ƹ&&(s.Ь<0||Θ.φ[s.Ь]=='1')) s.Ь=ΙЈ(Θ,s);
}}String ΙΕ(ȍ Θ){String γ="J;"+Θ.ы+";"+Θ.φ+";";bool Ǥ=true;for(int i=0;i<Θ.х.Count;i++){if((DateTime.Now-Θ.х[i].Εω).TotalSeconds>5) continue;γ+=(Ǥ?"":",")+Θ.х[i].Κ+":"+Θ.х[i].Ь;Ǥ=false;}return γ;}void Ιψ(String Η){String[]f=Η.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȍ Θ=null;for(int i=0;i<ǔ.Count;i++) if(ǔ[i].ы==f[4]) Θ=ǔ[i];if(Θ==null){Θ=new ȍ();Θ.ы=f[4];Θ.w=int.Parse(k[3]);Θ.h=int.Parse(k[4]);Θ.ΖΒ=float.Parse(k[6]);Θ.ΕЇ=float.Parse(k[7]);if(Θ.w<1||Θ.h<1) return;ΖΣ(Θ.w,Θ.h,(ѓ) int.Parse(k[5]),Θ.ΓΖ);
Θ.φ=new String('0',Θ.w*Θ.h);ǔ.Add(Θ);}String d=f[6];if(d.Length==Θ.φ.Length){char[]c=Θ.φ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Θ.φ=new String(c);}long Κ=long.Parse(f[0]);ȫ s=null;for(int i=0;i<Θ.х.Count;i++) if(Θ.х[i].Κ==Κ) s=Θ.х[i];
if(s==null){s=new ȫ();s.Κ=Κ;Θ.х.Add(s);}s.Εω=DateTime.Now;String[]w=f[10].Split(',');s.Ƹ=w[0]=="1";if(w.Length>=3){s.ΑΪ=float.Parse(w[1]);s.ΑΦ=float.Parse(w[2]);}int ĝ=int.Parse(f[5]);if(ĝ>=0&&ĝ<Θ.φ.Length&&(s.Ь<0||Θ.φ[s.Ь]=='1')&&Θ.φ[ĝ]!='1'&&ΓІ(Θ,ĝ,s)==null)
s.Ь=ĝ;}ȫ ΓІ(ȍ Θ,int Ь,ȫ ΚБ){for(int i=0;i<Θ.х.Count;i++){ȫ o=Θ.х[i];if(o!=ΚБ&&o.Ь==Ь)
return o;}return null;}int ΙЈ(ȍ Θ,ȫ s){for(int k=0;k<Θ.ΓΖ.Count;k++){int c=Θ.ΓΖ[k];if(Θ.φ[c]!='1'&&ΓІ(Θ,c,s)==null) return c;}return-2;}String Ιξ(){String s="";for(int j=0;j<ǔ.Count;j++){ȍ Θ=ǔ[j];int φ=0;for(int i=0;i<Θ.φ.Length;i++) if(Θ.φ[i]=='1') φ++;s+="Job "+(j+1)+": "+φ+"/"+Θ.φ.Length+" holes, "+Θ.х.Count+" ships\n";
}return s;}const float ΚΧ=8f;class м{public long Κ;public Vector3 ά,в,ț;public float ƒ,ΑΪ,ΑΦ,ǖ=-1;public int Ǿ;public String Θ="";public int ū;public DateTime Ȣ;}class ΒΩ{public long a,b,ΖΝ;public DateTime ƛ;}List<м>ς=new List<м>();List<ΒΩ>ћ=new List<ΒΩ>();int ΕІ=0;void Ιω(String Η)
{String[]f=Η.Split(';');if(f.Length<9) return;long Κ=long.Parse(f[0]);м s=null;for(int i=0;i<ς.Count;i++) if(ς[i].Κ==Κ) s=ς[i];if(s==null){s=new м();s.Κ=Κ;ς.Add(s);}s.ά=θ(f[1]);s.в=θ(f[2]);s.ƒ=float.Parse(f[3]);s.Θ=f[4];s.ū=int.Parse(f[7].Split(',')[1]);s.ț=θ(f[8]);
s.Ǿ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΪ=float.Parse(w[1]);s.ΑΦ=float.Parse(w[2]);s.Ǿ=int.Parse(w[3]);}}s.Ȣ=DateTime.Now;}bool ǋ(м s){return s.в.LengthSquared()<0.25f&&(s.ū==3||s.ț.LengthSquared()<0.25f);}Vector3 ΗΗ(м s){return s.в.LengthSquared()>=1f?s.в:s.ț*2f;}
void Κπ(){ΙΦ();if(++ΕІ<3) return;ΕІ=0;IGC.SendBroadcastMessage(ΗΖ,ΙΗ());}void ΙΦ(){for(int i=ς.Count-1;i>=0;i--)
if((DateTime.Now-ς[i].Ȣ).TotalSeconds>3) ς.RemoveAt(i);for(int i=ћ.Count-1;i>=0;i--)
if((DateTime.Now-ћ[i].ƛ).TotalSeconds>2) ћ.RemoveAt(i);for(int i=0;i<ς.Count;i++) ς[i].ǖ=-1;for(int i=0;i<ς.Count;i++)
for(int j=i+1;j<ς.Count;j++){м a=ς[i],b=ς[j];if(ǋ(a)||ǋ(b)) continue;Vector3 Λε=a.ά+a.в*(float)(DateTime.Now-a.Ȣ).TotalSeconds;Vector3 Λι=b.ά+b.в*(float)(DateTime.Now-b.Ȣ).TotalSeconds;Vector3 Ƴ=ΗΗ(a),ƴ=ΗΗ(b);Vector3 Ǫ=Λι-Λε;float υ=Ǫ.Length();float г=(a.ƒ+b.ƒ)*0.55f;
bool Αξ=a.Θ!=""&&a.Θ==b.Θ;if(Αξ&&(a.Ǿ==2||b.Ǿ==2)) continue;if(Αξ&&a.Ǿ==1&&b.Ǿ==1){String[]k=a.Θ.Split('|');if(k.Length>=8) г=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Εχ=ƴ-Ƴ;float Θλ=Εχ.LengthSquared();float t=Θλ>0.01f?-Vector3.Dot(Ǫ,Εχ)/Θλ:0;
bool ΕЎ=υ<г;if(!ΕЎ&&(t<=0||t>ΚΧ||(Ǫ+Εχ*t).Length()>=г)) continue;if(!ΕЎ&&Ƴ.LengthSquared()>0.25f&&ƴ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ƴ),Vector3.Normalize(ƴ))<-0.7f)
continue;ΒΩ p=null;for(int k=0;k<ћ.Count;k++)
if((ћ[k].a==a.Κ&&ћ[k].b==b.Κ)||(ћ[k].a==b.Κ&&ћ[k].b==a.Κ)) p=ћ[k];if(p==null){p=new ΒΩ();p.a=a.Κ;p.b=b.Κ;p.ΖΝ=ΚΨ(a,b,Ƴ,ƴ,Ǫ,υ);ћ.Add(p);}p.ƛ=DateTime.Now;м y=p.ΖΝ==a.Κ?a:b;float ΘΝ=ΕЎ||t<4?0:Math.Max(1,y.в.Length()*0.5f);if(y.ǖ<0||ΘΝ<y.ǖ) y.ǖ=ΘΝ;
}}long ΚΨ(м a,м b,Vector3 Ƴ,Vector3 ƴ,Vector3 Ǫ,float υ){if(a.Κ==є) return b.Κ;if(b.Κ==є) return a.Κ;Vector3 n=υ>0.01f?Ǫ/υ:new Vector3(1,0,0);float Θψ=Vector3.Dot(Ƴ,n),ΘЃ=Vector3.Dot(ƴ,-n);if(Θψ-ΘЃ>1) return a.Κ;if(ΘЃ-Θψ>1) return b.Κ;return a.Κ>b.Κ?a.Κ:b.Κ;
}String ΙΗ(){String γ="C;";bool Ǥ=true;for(int i=0;i<ς.Count;i++){if(ς[i].ǖ<0) continue;γ+=(Ǥ?"":",")+ς[i].Κ+":"+Math.Round(ς[i].ǖ,1);Ǥ=false;}return γ;}String Ισ(){int Θά=0,Αή=0;for(int i=0;i<ς.Count;i++){if(!ǋ(ς[i])) Θά++;if(ς[i].ǖ>=0) Αή++;}if(ς.Count==0) return "";
return "Traffic: "+Θά+" moving"+(Αή>0?", "+Αή+" giving way":"")+"\n";}List<ϊ>ą=new List<ϊ>();ϊ Ǽ=null;Vector3 ΗЏ;DateTime ΗЎ;float ΗЍ=40;bool ΗЌ=false;м Ζδ(ϊ s){м Љ=null;for(int i=0;i<ς.Count;i++)
if(Vector3.Distance(ς[i].ά,s.Ε)<15&&(Љ==null||
Vector3.Distance(ς[i].ά,s.Ε)<Vector3.Distance(Љ.ά,s.Ε))) Љ=ς[i];return Љ;}float Ιϋ(ϊ s){м t=Ζδ(s);if(t==null) return 0;float k=t.Ǿ*1000000f;String[]ы=t.Θ.Split('|');if(ы.Length>=3) k+=Vector3.Distance(t.ά,new Vector3(float.Parse(ы[0]),float.Parse(ы[1]),float.Parse(ы[2])));return k;
}void ΚΥ(){ą.Clear();Ǽ=null;List<float>ΕΤ=new List<float>();for(int i=0;i<Њ.Count;i++){float k=Ιϋ(Њ[i]);int ŗ=0;while(ŗ<ΕΤ.Count&&ΕΤ[ŗ]<=k) ŗ++;ΕΤ.Insert(ŗ,k);ą.Insert(ŗ,Њ[i]);}}void Κμ(){if(Ǽ!=null){bool ΛΕ=ΗЌ||Vector3.Distance(Ǽ.Ε,ΗЏ)>ΗЍ||
(DateTime.Now-ΗЎ).TotalSeconds>45||Ǽ.ΓЎ();if(!ΛΕ) return;Ǽ=null;}if(ą.Count==0) return;Ǽ=ą[0];ą.RemoveAt(0);ΗЏ=Ǽ.Ε;ΗЎ=DateTime.Now;м t=Ζδ(Ǽ);ΗЍ=t==null?40:Math.Max(40,t.ƒ*3);ΗЌ=t!=null&&t.ū==3&&t.в.LengthSquared()<0.25f;Ǽ.ĵ("HOMEPOS",true,false,false);ΒΣ("HOMEPOS",Ǽ.Κ,"HOMEPOS");
}const String Ιά="[PAMDBG]";class Γς{public readonly bool ΔΔ;IMyProgrammableBlock ĕ;Action<IMyProgrammableBlock>Δι;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Δθ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Δη;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>Δκ;
Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>Δζ;public Γς(MyGridProgram Λν){ĕ=Λν.Me;var Ŧ=ĕ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ĕ);if(Ŧ==null) return;Δι=(Action<IMyProgrammableBlock>) Ŧ["RemoveDraw"];Δθ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) Ŧ["Point"];
Δη=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) Ŧ["Line"];Δκ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) Ŧ["Sphere"];Δζ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) Ŧ["GPS"];((Action<IMyProgrammableBlock>) Ŧ["RemoveAll"])(ĕ);
ΔΔ=true;}public void ΚΑ(){if(Δι!=null) Δι(ĕ);}public void Ĵ(Vector3D a,Vector3D b,Color c,float ΛЅ){if(Δη!=null) Δη(ĕ,a,b,c,ΛЅ,-1,false);}public void ΙА(Vector3D a,Color c,float ċ){if(Δθ!=null) Δθ(ĕ,a,c,ċ,-1,false);}public void Ɓ(String Γ,Vector3D ŗ,Color c){if(Δζ!=null) Δζ(ĕ,Γ,ŗ,c,-1);}
public void ΗΑ(Vector3D Ɛ,double ċ,Color c){if(Δκ!=null) Δκ(ĕ,new BoundingSphereD(Ɛ,ċ),c,1,0.05f,30,-1,false);}}class ƾ{public long Κ;public Vector3 Αα,Ţ,Ȗ,ε;public bool ΗЇ;public List<Vector3>ΓΙ=new List<Vector3>(),Ες=new List<Vector3>();public List<bool>Ερ=new List<bool>();
public DateTime Ȣ;}Γς П=null;IMyBroadcastListener Βρ=null;List<ƾ>Ă=new List<ƾ>();void Ιυ(String Η){String[]f=Η.Split(';');if(f.Length<6) return;long Κ=long.Parse(f[0]);ƾ s=null;for(int i=0;i<Ă.Count;i++) if(Ă[i].Κ==Κ) s=Ă[i];if(s==null){s=new ƾ();s.Κ=Κ;Ă.Add(s);}
s.Αα=θ(f[1]);s.Ţ=θ(f[2]);s.Ȗ=θ(f[3]);s.ΗЇ=s.Ţ.LengthSquared()>0.5f;s.ε=θ(f[4]);s.Ȣ=DateTime.Now;if(f[5]=="") return;s.ΓΙ.Clear();s.Ες.Clear();s.Ερ.Clear();String[]Θκ=f[5].Split(':');for(int i=0;i<Θκ.Length;i++){String[]r=Θκ[i].Split(',');if(r.Length<3) continue;s.ΓΙ.Add(θ(r[0]));s.Ες.Add(θ(r[1]));s.Ερ.Add(r[2]=="1");
}}void Κα(){if(!debugDraw) return;if(П==null) П=new Γς(this);if(!П.ΔΔ) return;if(Βρ==null) Βρ=IGC.RegisterBroadcastListener(Ιά);int Ǣ=20;while(Βρ.HasPendingMessage&&Ǣ-->0){String Η=Βρ.AcceptMessage().Data as String;if(Η!=null) try{Ιυ(Η);}catch{}}for(int i=Ă.Count-1;i>=0;i--) if((DateTime.Now-Ă[i].Ȣ).TotalSeconds>5) Ă.RemoveAt(i);
П.ΚΑ();for(int i=0;i<ς.Count;i++){м s=ς[i];Color c=ǋ(s)?Color.Gray:s.ǖ>=0?Color.Yellow:Color.Lime;П.ΗΑ(s.ά,s.ƒ/2,c);П.Ɓ(ΒΨ(s)+": "+(ǋ(s)?"parked":s.ǖ==0?"holding (giving way)":s.ǖ>0?"slowing (giving way)":"moving"),s.ά,c);if(s.в.LengthSquared()>0.25f) П.Ĵ(s.ά,s.ά+s.в*3,Color.White,0.1f);
if(s.ț.LengthSquared()>0.25f) П.Ĵ(s.ά,s.ά+s.ț*s.ƒ,Color.Cyan,0.1f);}for(int i=0;i<ћ.Count;i++){if((DateTime.Now-ћ[i].ƛ).TotalSeconds>0.5) continue;м a=null,b=null;for(int k=0;k<ς.Count;k++){if(ς[k].Κ==ћ[i].a) a=ς[k];if(ς[k].Κ==ћ[i].b) b=ς[k];}if(a==null||b==null) continue;
П.Ĵ(a.ά,b.ά,Color.Red,0.2f);П.Ɓ("Conflict: "+ΒΨ(ћ[i].ΖΝ==a.Κ?a:b)+" gives way",(a.ά+b.ά)/2,Color.Red);}for(int i=0;i<Ă.Count;i++){ƾ d=Ă[i];м t=null;for(int k=0;k<ς.Count;k++) if(ς[k].Κ==d.Κ) t=ς[k];if(t!=null) П.Ĵ(t.ά,d.ε,Color.Blue,0.08f);for(int r=0;r<d.ΓΙ.Count;r++) П.Ĵ(d.ΓΙ[r],d.Ες[r],d.Ερ[r]?Color.Red:Color.Green,0.04f);
if(t==null||!d.ΗЇ) continue;ȍ Θ=null;for(int j=0;j<ǔ.Count;j++) if(ǔ[j].ы==t.Θ) Θ=ǔ[j];if(Θ==null) continue;int Ηϊ=0;for(int c=0;c<Θ.φ.Length;c++) if(Θ.φ[c]=='1') Ηϊ++;if(i==0||Ă[i-1].Αα!=d.Αα) П.Ɓ("Job: "+Ηϊ+"/"+Θ.φ.Length+" holes done",d.Αα,Color.White);
for(int c=0;c<Θ.φ.Length;c++){int ΔЃ=c%Θ.w,ņ=c/Θ.w;Vector3 p=d.Αα+d.Ţ*((Θ.w-1f)/2f-ΔЃ)*Θ.ΖΒ-d.Ȗ*((Θ.h-1f)/2f-ņ)*Θ.ΕЇ;ȫ Εμ=ΓІ(Θ,c,null);П.ΙА(p,Θ.φ[c]=='1'?Color.Gray:Εμ!=null?Color.Orange:Color.White,Math.Min(Θ.ΖΒ,Θ.ΕЇ)*0.2f);if(Εμ==null||Εμ.Κ!=d.Κ) continue;
П.Ĵ(t.ά,p,Color.Orange,0.05f);П.Ɓ("Hole "+(Θ.ΓΖ.IndexOf(c)+1)+": "+ΒΨ(t),p,Color.Orange);}}for(int i=0;i<Ȋ.Count;i++){IMySensorBlock z=Ȋ[i];П.ΗΑ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);
П.Ɓ("Docking area"+(Ě!=""?": "+Ě:""),z.GetPosition(),Color.Cyan);}for(int i=0;i<Ģ.Count;i++)
П.Ĵ(Ģ[i].GetPosition(),Ģ[i].GetPosition()+Ģ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(є!=0&&Βζ!=0){IMyShipConnector c=Δί(Βζ);for(int k=0;k<ς.Count;k++)
if(c!=null&&ς[k].Κ==є){П.Ĵ(ς[k].ά,c.GetPosition(),Color.Lime,0.15f);П.Ɓ("Cleared to "+(Βη==2?"undock: ":"dock: ")+ΒΨ(ς[k]),c.GetPosition(),Color.Lime);}}}String ΒΨ(м t){ϊ Љ=null;for(int i=0;i<Њ.Count;i++)
if(Vector3.Distance(Њ[i].Ε,t.ά)<30&&(Љ==null||
Vector3.Distance(Њ[i].Ε,t.ά)<Vector3.Distance(Љ.Ε,t.ά))) Љ=Њ[i];if(Љ!=null&&Љ.ΰ!="") return Љ.ΰ;String Κ=t.Κ.ToString();return "Ship "+Κ.Substring(Math.Max(0,Κ.Length-4));}String Ιη(){if(!debugDraw) return "";if(П==null||!П.ΔΔ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}