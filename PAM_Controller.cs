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
//[m] ships ask the station for clearance this close to their home dock, and wait outside the [PAM-Zone] sensor area
//Tag for LCD's of cockpits and other blocks: [PAM:<lcdIndex>] e.g: [PAM:1]
 //[RPM] small ship
 //[RPM] large ship
 //[m/s] 0 = no limit (if set to 0 ignore unreachable code warning)
 //[m/s]

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
 //stop following path, fly direct to target
 //same as dock
 //Override waypoint direction, use docking dir
 //same as dock

//other distances
//[m]
const float drillRadius = 1.4f;//[m] grinders and modded drills; vanilla drills use their real cut radius (small 1.3, large 1.9, prototech 6)

//grinding
//fly slow when blocks found in this range
//speed when no blocks are detected

//minimum acceleration in space before ship becomes too heavy
const float minAccelerationSmall = 0.2f;//[m/s²] small ship
const float minAccelerationLarge = 0.1f;//[m/s²] large ship

//stone ejection
//[%] Min amount of ejected cargo to continue job

//LCD
const bool setLCDFontAndSize = true;

//Obstacle brake: cameras on the ship raycast towards the flight target and stop the ship when
//something is in the way (path flights and hole-to-hole moves; off while mining and docking).
//A camera only scans within 45° of where it faces: add cameras facing every direction the ship travels.
//[s] scan distance = speed * this + ship size (the sweep covers the ship's width)
//[s] show "Path blocked" after holding this long

//Traffic between PAM ships (needs a working antenna; independent of Broadcast_Channel)
//Ships keep right of the recorded path, keep distance to a PAM ship ahead, and ships on the
//same job (IMPORT JOB) mine different rows of holes.
const bool trafficEnabled = true;//multiplied with ship size: how far to keep right of the path; 0 = fly on the path
//multiplied with ship size: free space kept behind a PAM ship ahead

//Server restart / world load
//continue a job that was running when the world was saved (false = stay paused, like the original)

//Check if blocks are connected with conveyors
//temporarily disabled because of a SE bug

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ο{ǔ,Ơ,ȋ,Г,ν}public enum Μ{ǧ,Ƒ,ΒΥ,ƥ,Βα,ă,ǐ,
Ɛ,ΔΤ,ȇ,ΑЄ,Γώ,Αύ,
ΔЎ,Ċ,ĉ,Ɖ,Εζ,ΔΞ,
Εδ,Εε,ΔΟ,ΔΠ,ċ}public enum Τ{ю,Ƣ,Ǧ,ΑЅ,Ľ,ΒΧ,ΒΨ,Ĺ,ĺ}public enum џ{None,Ɖ,ǧ,ΔΕ}public enum Κ{Main,Βί,ą,Ą,Ʈ,ļ,
ƅ,ΒΘ,ΒΙ,ΒΗ,Ĉ,Βγ,
ņ,ƨ,ƫ,Βζ,Ŋ}public enum Ǔ{Ββ,ΓБ,ΕΞ,Δς,ν}public enum Т{ō,ΓЌ}public enum Ǐ{Βΰ,ΔΣ,ΕΡ,Ɩ}public enum б{ΒΪ,ΔΓ,ΔΒ,ΔΚ,ΔΙ,Δπ,Δο}public enum ь{ƍ,ΓЂ,Αώ}public enum Ł{Δξ,ΖЇ,ΓϏ}public enum Ѓ{ΒΫ,Βά,Βέ,ΔЃ,ΔЂ,
Δύ,Δώ,Δό,ΔЀ,
ΔЁ,ΔϏ}public enum Υ{м,я,л,Ļ,Ɔ,Ƈ}public enum н{Βι,ΑІ,Δϋ,ΒΩ}public enum η{None,Ȍ,Ć,ń}public enum ς{ư,Ƅ,Ɲ,Ɩ,ΒΣ,Ɵ,ΒΤ,ΕΫ,ơ}public enum ω{ǔ,ΒΠ,ΒΟ,ƞ,Ɨ,Ƌ,ΒΡ}class ά{public bool λ=false;public Vector3 Ρ=new Vector3();public Vector3 Ϗ=new Vector3();
public Vector3 Є=new Vector3();public Vector3 Н=new Vector3();public Vector3 ξ=new Vector3();public Vector3 Ц=new Vector3();public float ΖΙ=0;public float ΖЎ=0;public float[]Ю=null;public ά(){}public ά(ά З){λ=З.λ;Ρ=З.Ρ;Ϗ=З.Ϗ;Є=З.Є;Н=З.Н;ξ=З.ξ;Ц=З.Ц;Ю=З.Ю;}public ά(Vector3 Ρ,Vector3 Є,Vector3 Ϗ,Vector3 Н,Vector3 ξ)
{this.Ρ=Ρ;this.Ϗ=Ϗ;this.Є=Є;this.Н=Н;this.ΖΙ=0;this.ξ=ξ;}public void ΖІ(List<IMyThrust>ő,List<string>ў){Ю=new float[ў.Count];for(int i=0;i<Ю.Length;i++)
Ю[i]=-1;for(int i=0;i<ő.Count;i++){string ȃ=Δε(ő[i]);int Α=ў.IndexOf(ȃ);if(Α!=-1)
Ю[Α]=Р(ő[i].MaxEffectiveThrust,ő[i].MaxThrust);}}}class Ŏ{public Ѓ О=Ѓ.ΒΫ;public float д=0;public float ǰ=0;public string с="";public string ф="";DateTime ΓΣ;public bool ΖΨ=false;private bool ƿ=false;public bool ΓЍ(bool Αά){if(Αά){ǰ=0;ƿ=false;return false;}ƿ=true;return ǰ>д;
}public void ΖЈ(){ΓЍ(true);ΖΨ=false;}public void ΖЉ(){if(ƿ)
if((DateTime.Now-ΓΣ).TotalSeconds>1){ǰ++;ΓΣ=DateTime.Now;}}public bool ΒΕ(){switch(О){case Ѓ.Βά:return true;case Ѓ.Βέ:return true;}return false;}}class Ǩ{public String ι="";public String Ћ="";public int Ї=0;public Ł ΑΚ=Ł.ΓϏ;public Ǩ(String ι,String Ћ,int Ї,Ł ΑΚ){this.ι=ι;this.Ћ=Ћ;
this.Ї=Ї;this.ΑΚ=ΑΚ;}}class ΰ{public DateTime ΑΖ;public DateTime Βό;public String Ν="";public String ι="";public String ſ="";public String Ψ="";public String Αλ="";public String ű="";public String ΑΧ="";public Vector3 Ρ=new Vector3();public List<Ǩ>ї=new List<Ǩ>();public Ο Π=Ο.ǔ;
public η Ǘ;public float Ƿ;public float Ǭ;public int π;public int ǁ=0;public int Ασ=0;public bool ƛ(){return(DateTime.Now-ΑΖ).TotalSeconds>10;}public bool Ƙ(){if(Ǘ!=η.Ȍ) return false;return(DateTime.Now-Βό).TotalSeconds>=4;}public ΰ(String Ν){this.Ν=Ν;}public η ǒ(String κ,bool Ζζ,bool ђ,bool Ερ)
{if(κ==""&&!ђ) return η.None;if(Ǘ==η.Ȍ&&Ƙ())
Ǘ=η.ń;if(Ζζ){ΑΧ=κ;Ǘ=η.Ȍ;Βό=DateTime.Now;return Ǘ;}else if(ђ){ΑΧ="";Ǘ=η.None;}else if(ΑΧ==κ){if(Ερ) Ǘ=η.Ć;return Ǘ;}return η.None;}}public String Δά(String ΖΑ){switch(ΖΑ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem М;Vector3 Γη=new Vector3();Vector3 к=new Vector3();Vector3 ΕЂ=new Vector3();Vector3 ΖΔ=new Vector3();Vector3 Εϋ=new Vector3();DateTime ΓΡ=new DateTime();
bool ĝ=true;int Απ=0;bool Ħ=false;bool Ǜ=false;bool ΖΝ=true;bool ΕЎ=false;float Ζθ=0;float Γο=0;int Ζΰ=0;int Ÿ=0;float ȟ=0;float ȑ=0;double ΑΟ=0;float Ď=0;List<int>Ț=new List<int>();List<int>ȫ=new List<int>();Ο Π=Ο.ǔ;int Αβ=0;float Ȃ=0;float ȁ=0;float ȯ=0;float Γξ=0,Γν=0;
IMyRemoteControl Љ;IMySensorBlock ό;List<IMyTimerBlock>Γφ=new List<IMyTimerBlock>();List<IMyShipConnector>ĕ=new List<IMyShipConnector>();List<IMyThrust>Ĳ=new List<IMyThrust>();List<IMyGyro>ΒБ=new List<IMyGyro>();List<IMyTerminalBlock>Я=new List<IMyTerminalBlock>();List<IMyLandingGear>ΑΕ=new List<IMyLandingGear>();
List<IMyReactor>ΑΫ=new List<IMyReactor>();List<IMyConveyorSorter>Ζλ=new List<IMyConveyorSorter>();List<IMyGasTank>Ĥ=new List<IMyGasTank>();List<IMyTerminalBlock>Č=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Βσ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ј=new List<IMyTextPanel>();
List<IMyTextSurface>щ=new List<IMyTextSurface>();List<IMyTextPanel>Ǯ=new List<IMyTextPanel>();IMyTerminalBlock ǡ=null;ά Σ=new ά();ά ΓΖ=new ά();ά ΓΝ=new ά();bool ǥ=false;bool ĵ=false;bool Ķ=false;bool ΓΛ=false;List<String>ў=new List<string>();List<ά>ψ=new List<ά>();List<ά>ƀ=new List<ά>();
double Ζύ=0;ά Φ=new ά();Vector3 ĩ;Vector3 ȡ;Τ υ=Τ.ю;Т ț=Т.ō;int й=0;int и=0;double ū=0;bool ΒЂ=false;Ŏ φ=new Ŏ();Ŏ χ=new Ŏ();Ǐ Ǡ=Ǐ.Βΰ;bool ķ=true;Т Ǿ=Т.ō;int Х=3;int Ф=3;int г=30;ь і=ь.ƍ;bool ȅ=true;bool ĥ=true;bool ě=true;bool Ȁ=false;б Ě=б.ΒΪ;bool Ǥ=true;
float х=90;float ц=20;float Ъ=5;float ч=20;float ǖ=0.70f;float Ƃ=1.50f;float Ɓ=2.50f;float ĸ=0f;float ġ=0f;Μ ś;Τ ǝ;Μ İ;int ť=0;int Ŧ=0;int п=0;int ħ=0;int Αί=30;bool Ζμ=true;double ΒЇ=0;bool Γι=false;int Ζώ=0;int Ȅ=0;ά Ź=null;float ΒЁ=0;float ΑΞ=0;ά Εω=null;
float Βτ=0;џ ď;float ΓΚ=0;float Γω=0;String ΖΗ="";float ΑΙ=0;float Ǭ=0;float Ƿ=0;float Ѐ=0;bool Αε=false;bool Ζς=false;bool Ζι=false;bool Γυ=false;bool Ζκ=false;float ΖΘ=0;float ΖΦ=0;Vector3 ΕЀ=new Vector3();Vector3 Γή=new Vector3();bool ΕЇ=false;float[,]Ĵ=new float[3,2];
Dictionary<String,float[,]>ѝ=new Dictionary<string,float[,]>();Dictionary<String,float[]>Ŷ=new Dictionary<string,float[]>();bool ΒЄ=false;int Œ=0;int Ǚ=0;int Ũ=0;int ǳ=0;String Ψ="";int[]ΓΫ=new int[Enum.GetValues(typeof(Κ)).Length];bool ΑΡ=false;int Ǹ=0;int π=0;Κ γ=Κ.Main;
int Ζσ=0,Ζτ=0,Ζυ=0,Ζφ=0;int Ζα=0,Ζβ=0;bool đ=false;bool Βο=false;String Į="";String ƺ="";IMyBroadcastListener ё=null;bool ΒЌ=true;List<ΰ>σ=null;ΰ ρ=null;int ŗ=0,Ř=0;int Βώ=0,ΒϏ=0;int Εφ=0,Εχ=0,Εψ=0;String ΖΒ="";Exception ȝ=null;bool Γθ=false;Τ Αΰ=Τ.ю;
bool ΒЃ=false;String Ζϋ="";bool Γψ=false;bool Αι=true;int ΑΒ=0;Vector3 ƻ=new Vector3();bool ΒЏ=false;bool ƚ(IMyTerminalBlock α)=>α.CubeGrid==Me.CubeGrid;Program(){М=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΖ(Me,controllerTag,true))
Π=Ο.Г;Χ(Κ.Main);if(Π!=Ο.Г){Ψ="Welcome to [PAM]!";н Αέ=ΔЌ();if(Π==Ο.ǔ){List<IMyShipDrill>ΒЊ=new List<IMyShipDrill>();List<IMyShipGrinder>ΒА=new List<IMyShipGrinder>();М.GetBlocksOfType(ΒЊ,q=>q.CubeGrid==Me.CubeGrid);М.GetBlocksOfType(ΒА,q=>q.CubeGrid==Me.CubeGrid);if(ΒЊ.Count>0)
{Π=Ο.Ơ;Ψ="Miner mode enabled!";}else if(ΒА.Count>0){Π=Ο.ȋ;Ψ="Grinder mode enabled!";}else{Π=Ο.ν;Βε(Ǔ.ν);}}if(Αέ==н.Βι) ΖΝ=false;if(Αέ==н.ΑІ) Ψ="Data restore failed!";if(Αέ==н.ΒΩ) Ψ="New version, wipe data";}}void Main(string Ў,UpdateType Ζψ){try{if(ȝ!=null){if(Ў.Trim().ToUpper()=="RETRY")
{ȝ=null;ĝ=true;Ζΰ=0;if(Π!=Ο.Г) Ǫ();Ψ="Restarted after error";}else{Δκ();ΔЈ();return;}}Ǜ=(Ζψ&UpdateType.Update10)!=0;if(Ǜ) Απ++;Ħ=Απ>=10;if(Ħ) Απ=0;if(Ǜ){Œ++;if(Œ>4) Œ=0;Ÿ=(int) Math.Max(0,10-(DateTime.Now-ΓΡ).TotalSeconds);}if(Ў!="") ΖΒ=Ў;if(Π!=Ο.Г)
ΕΝ(Ў);else
ΔΑ(Ў);ΑΡ=false;try{int ΒЀ=Runtime.CurrentInstructionCount;float ǻ=Р(ΒЀ,Runtime.MaxInstructionCount);if(ǻ>0.90) Ψ="Max. instructions >90%";if(ǻ>ȟ) ȟ=ǻ;if(ΒЄ){Ț.Add(ΒЀ);while(Ț.Count>10) Ț.RemoveAt(0);ȑ=0;for(int i=0;i<Ț.Count;i++) ȑ+=Ț[i];ȑ=Р(Р(ȑ,Ț.Count),Runtime.MaxInstructionCount);
double ΑΗ=Runtime.LastRunTimeMs;if(ΕЎ&&ΑΗ>ΑΟ) ΑΟ=ΑΗ;ȫ.Add((int)(ΑΗ*1000f));while(ȫ.Count>10) ȫ.RemoveAt(0);Ď=0;for(int i=0;i<ȫ.Count;i++) Ď+=ȫ[i];Ď=Р(Ď,ȫ.Count)/1000f;}}catch{ȟ=0;}}catch(Exception e){ȝ=e;}}String Δέ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Π!=Ο.ν)
Β+="[SHUTTLE] Enable shuttle mode\n"+"[NEXT] Next hole\n"+"[PREV] Previous hole\n\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Χ(Κ Ќ){ΓΫ[(int) γ]=π;π=ΓΫ[(int) Ќ];if(Ќ==Κ.Ĉ) π=0;γ=Ќ;if(Π!=Ο.Г)
Ő(γ==Κ.ą,false,0,0);ΑΡ=true;}void Δω(bool ΑΛ){if(π>0) π--;else if(ΑΛ) π=Ǹ-1;}void Δψ(bool ΑΛ){if(π<Ǹ-1) π++;else if(ΑΛ) π=0;}String Βε(Ǔ ε){switch(ε){case Ǔ.Ββ:Ψ="Job is running";break;case Ǔ.ΓБ:Ψ="Connector not ready!";break;case Ǔ.ΕΞ:Ψ="Ship modified, path outdated!";break;
case Ǔ.Δς:Ψ="Interrupted by player!";break;case Ǔ.ν:Ψ="Shuttle mode enabled!";break;}return "";}String Δδ(Т Ω){switch(Ω){case Т.ō:return "Top-Left";case Т.ΓЌ:return "Center";default:return "";}}String ΔΩ(ь θ){switch(θ){case ь.ΓЂ:return "Auto"+(Π==Ο.Ơ?" (Ore)":"");case ь.Αώ:return "Auto (+Stone)";
case ь.ƍ:return "Default";default:return "";}}String Δα(Τ ώ){switch(ώ){case Τ.ю:return "No job";case Τ.Ƣ:return "Job paused";case Τ.Ǧ:return "Job active";case Τ.Ĺ:return "Job active";case Τ.ĺ:return "Job active";case Τ.ΑЅ:return "Job done";case Τ.Ľ:return "Job changed";case Τ.ΒΧ:return "Move home";
case Τ.ΒΨ:return "Move to job";}return "";}String ΑЌ(Ǐ Εν){switch(Εν){case Ǐ.Βΰ:return "Return home";case Ǐ.ΔΣ:return "Fly to job pos";case Ǐ.ΕΡ:return "Stop";case Ǐ.Ɩ:return "Ignore";}return "";}String ΔΫ(б θ){switch(θ){case б.ΒΪ:return "Off";case б.ΔΚ:return "Drop pos (Stone) ";case б.ΔΙ:return "Drop pos (Sto.+Ice)";
case б.ΔΓ:return "Cur. pos (Stone)";case б.ΔΒ:return "Cur. pos (Sto.+Ice)";case б.Δπ:return "In motion (Stone)";case б.Δο:return "In motion (Sto.+Ice)";}return "";}String ΑЊ(џ ώ){switch(ώ){case џ.None:return "No batteries";case џ.Ɖ:return "Charging";case џ.ΔΕ:return "Discharging";}return "";}
String ΑА(Μ ώ){String ΖЋ=Π==Ο.ν?"target":"job";switch(ώ){case Μ.ǧ:return "Idle";case Μ.Ƒ:return "Flying to XY position";case Μ.ΒΥ:return Π==Ο.ȋ?"Grinding":"Mining";case Μ.ƥ:return "Returning";case Μ.ΔΟ:return "Flying to drop pos";case Μ.Βα:return "Returning to dock";case Μ.ă:return "Flying to dock area";
case Μ.ǐ:return "Flying to job area";case Μ.Ɛ:return "Flying to path";case Μ.ΔΤ:return "Flying to job position";case Μ.ȇ:return "Approaching dock";case Μ.ΑЄ:return "Docking";case Μ.Γώ:return "Aligning";case Μ.Αύ:return "Aligning";case Μ.ΔЎ:return "Retry docking";case Μ.Ċ:return "Unloading";case Μ.ċ:return Ζϋ;
case Μ.ĉ:return "Undocking";case Μ.Ɖ:return "Charging batteries";case Μ.Εζ:return "Waiting for uranium";case Μ.ΔΞ:return "Filling up hydrogen";case Μ.Εδ:return "Waiting for ejection";case Μ.Εε:return "Waiting for ejection";case Μ.ΔΠ:return "Flying to drop pos";}return "";}String ΒΔ(Ѓ О){switch(О)
{case Ѓ.ΒΫ:return "On \"Undock\" command";case Ѓ.Βά:return "On player entered cockpit";case Ѓ.ΔЃ:return "On ship is full";case Ѓ.ΔЂ:return "On ship is empty";case Ѓ.Βέ:return "On time delay";case Ѓ.Δώ:return "On batteries empty(<25%)";case Ѓ.Δό:return "On batteries empty(=0%)";case Ѓ.Δύ:return "On batteries full";
case Ѓ.ΔЁ:return "On hydrogen empty(<25%)";case Ѓ.ΔϏ:return "On hydrogen empty(=0%)";case Ѓ.ΔЀ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Γμ,int ǘ,bool Ε,String ǵ){Ǹ+=1;if(Γμ==ǘ) ǵ=">"+ǵ+(Œ>=2?" .":"");else ǵ=" "+ǵ;Β+=ǵ+"\n";return Γμ==ǘ&&Ε;}String ΓЄ(bool Ε)
{int Α=0;int Δ=π;Ǹ=0;String Ȯ="———————————————\n";String ȱ="--------------------------------------------\n";String Β="";Β+=Δα(υ)+" | "+(Σ.λ?"Ready to dock":"No dock")+"\n";Β+=Ȯ;double Γσ=Math.Max(Math.Round(this.ΒЇ),0);if(γ==Κ.Main){bool ǚ=Π==Ο.ν;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΕΟ();
if(Π==Ο.Ơ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Χ(Κ.ą);if(Π==Ο.ȋ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Χ(Κ.ą);if(Π==Ο.ν)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Χ(Κ.ƫ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ΑЂ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΔΡ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΔΥ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ǚ) Χ(Κ.Ŋ);else Χ(Κ.ļ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Χ(Κ.ΒΘ);if(Π!=Ο.ν)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Χ(Κ.ΒΗ);}else if(γ==Κ.ą){double ΖϏ=Math.Round(Х*Ȃ,1);double ΕЊ=Math.Round(Ф*ȁ,1);String μ="";if(Ζ(ref μ,Δ,Α++,Ε," Start new job!")) Βθ();if(Ζ(ref μ,Δ,Α++,Ε," Change current job")){Ư(false,false);Χ(Κ.Main);}if(Ζ(ref μ,Δ,Α++,Ε," Width + (Width: "+Х+" = "+ΖϏ+"m)")){Ȇ(ref Х,5,20,1);ȏ(true);}
if(Ζ(ref μ,Δ,Α++,Ε," Width -")){Ȇ(ref Х,-5,20,-1);ȏ(true);}if(Ζ(ref μ,Δ,Α++,Ε," Height + (Height: "+Ф+" = "+ΕЊ+"m)")){Ȇ(ref Ф,5,20,1);ȏ(true);}if(Ζ(ref μ,Δ,Α++,Ε," Height -")){Ȇ(ref Ф,-5,20,-1);ȏ(true);}if(Ζ(ref μ,Δ,Α++,Ε," Depth + ("+(і==ь.ƍ?"Depth":"Min")+": "+г+"m)")){Ȇ(ref г,5,50,2);ȏ(true);}
if(Ζ(ref μ,Δ,Α++,Ε," Depth -")){Ȇ(ref г,-5,50,-2);ȏ(true);}if(Ζ(ref μ,Δ,Α++,Ε," Depth mode: "+ΔΩ(і))){і=ǎ(і);}if(Ζ(ref μ,Δ,Α++,Ε," Start pos: "+Δδ(Ǿ))){Ǿ=ǎ(Ǿ);}if(Π==Ο.ȋ&&і==ь.Αώ) і=ǎ(і);Β+=Ʀ(8,μ,Δ,ref Ζα);}else if(γ==Κ.ƫ){float[]ΒЅ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Κ.Βζ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Κ.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΔ(φ.О))) φ.О=ǎ(φ.О);if(!φ.ΒΕ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƒ((int) φ.д))) φ.д=ΑЃ(φ.д,ΒЅ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΔ(χ.О))) χ.О=ǎ(χ.О);
if(!χ.ΒΕ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƒ((int) χ.д))) χ.д=ΑЃ(χ.д,ΒЅ);}else if(γ==Κ.Βζ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) Βθ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Κ.ƫ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(φ.с!=""?φ.с:"-"))) φ.с=Ń(ref Ζσ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(φ.ф!=""?φ.ф:"-"))) φ.ф=Ń(ref Ζυ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.с!=""?χ.с:"-"))) χ.с=Ń(ref Ζτ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.ф!=""?χ.ф:"-"))) χ.ф=Ń(ref Ζφ);
}else if(γ==Κ.Ą){String ΓΦ=х+" %";if(Ħ) Ǚ++;if(Ǚ>1){Ǚ=0;Ũ++;if(Ũ>1) Ũ=0;}bool[]Αζ=new bool[]{ΑΫ.Count==0,ď==џ.None,Ĥ.Count==0};int Γχ=0;while(true){Γχ++;ǳ++;if(ǳ>Αζ.Length-1) ǳ=0;if(Γχ>=Αζ.Length) break;if(!Αζ[ǳ]) break;}bool ǚ=Π==Ο.ν;if(!ǚ&&ȅ&&Ѐ!=-1&&Ũ==0)
ΓΦ=Ѐ<1000000?Math.Round(Ѐ)+" Kg":Math.Round(Ѐ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){Ǫ();Χ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ǚ) Χ(Κ.ļ);else Χ(Κ.Ŋ);if(!ǚ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) Γύ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) Γψ=true;Β+=ȱ;if(!ǚ) Β+="Progress: "+Math.Round(ū,1)+" %\n";Β+="State: "+ΑА(ś)+" "+Γσ+"m \n";Β+="Load: "+ΑΙ+" % Max: "+ΓΦ+" \n";if(ǳ==0) Β+="Uranium: "+(ΑΫ.Count==0?"No reactors":Math.Round(Γω,1)+"Kg "+(Ъ==-1?"":" Min: "+Ъ+" Kg"))+"\n";
if(ǳ==1) Β+="Battery: "+(ď==џ.None?ΑЊ(ď):Βτ+"% "+(ц==-1||ǚ?"":" Min: "+ц+" %"))+"\n";if(ǳ==2) Β+="Hydrogen: "+(Ĥ.Count==0?"No tanks":Math.Round(ΓΚ,1)+"% "+(ч==-1||ǚ?"":" Min: "+ч+" %"))+"\n";}else if(γ==Κ.ļ){String μ="";if(Ζ(ref μ,Δ,Α++,Ε," Back")){if(υ==Τ.Ǧ) Χ(Κ.Ą);else Χ(Κ.Main);}
if(Ζ(ref μ,Δ,Α++,Ε," Max load: "+х+"%")) В(ref х,х<=80?-10:-5,ς.Ɲ,false);if(Ζ(ref μ,Δ,Α++,Ε," Weight limit: "+(ȅ?"On":"Off"))) ȅ=!ȅ;if(Ζ(ref μ,Δ,Α++,Ε," Ejection: "+ΔΫ(Ě))) Ě=ǎ(Ě);if(Ζ(ref μ,Δ,Α++,Ε," Toggle sorters: "+(Ȁ?"On":"Off"))){Ȁ=!Ȁ;if(Ȁ) ƪ(Αι);}
if(Ζ(ref μ,Δ,Α++,Ε," Unload ice: "+(Ǥ?"On":"Off"))) Ǥ=!Ǥ;if(Ζ(ref μ,Δ,Α++,Ε," Uranium: "+(Ъ==-1?"Ignore":"Min "+Ъ+"Kg"))) В(ref Ъ,(Ъ>5?-5:-1),ς.Ɵ,true);if(Ζ(ref μ,Δ,Α++,Ε," Battery: "+(ц==-1?"Ignore":"Min "+ц+"%"))) В(ref ц,-5,ς.ΒΣ,true);if(Ζ(ref μ,Δ,Α++,Ε," Hydrogen: "+(ч==-1?"Ignore":"Min "+ч+"%"))) В(ref ч,-10,ς.ΒΤ,true);
if(Ζ(ref μ,Δ,Α++,Ε," When done: "+(ķ?"Return home":"Stop"))) ķ=!ķ;if(Ζ(ref μ,Δ,Α++,Ε," On damage: "+ΑЌ(Ǡ))) Ǡ=ǎ(Ǡ);if(Ζ(ref μ,Δ,Α++,Ε," Advanced...")) Χ(Κ.ƅ);Β+=Ʀ(8,μ,Δ,ref Ζβ);}else if(γ==Κ.ƅ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(υ==Τ.Ǧ) Χ(Κ.Ą);else Χ(Κ.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Π==Ο.ȋ?" Grinder":" Drill")+" inv. balancing: "+(ĥ?"On":"Off"))) ĥ=!ĥ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Π==Ο.ȋ?" grinders":" drills")+": "+(ě?"Fwd + Bwd":"Fwd"))) ě=!ě;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ƃ+"m/s")) В(ref Ƃ,0.5f,ς.ư,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ɓ+"m/s")) В(ref Ɓ,0.5f,ς.ư,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǖ*100f)+"%"+(ǖ>0.80f?" (risky)":""))) В(ref ǖ,0.1f,ς.Ƅ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ĸ*100f+"%")) ΑЁ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ġ*100f+"%")) ΑЁ(false,0.05f);
}else if(γ==Κ.Ŋ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(υ==Τ.Ǧ) Χ(Κ.Ą);else Χ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+х+"%")) В(ref х,х<=80?-10:-5,ς.Ɲ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(Ǥ?"On":"Off"))) Ǥ=!Ǥ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Ъ==-1?"Ignore":"Min "+Ъ+"Kg"))) В(ref Ъ,(Ъ>5?-5:-1),ς.Ɵ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(ц==-1?"Ignore":"Charge up"))) ц=(ц==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(ч==-1?"Ignore":"Fill up"))) ч=(ч==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ΑЌ(Ǡ))) Ǡ=ǎ(Ǡ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǖ*100f)+"%"+(ǖ>0.80f?" (risky)":""))) В(ref ǖ,0.1f,ς.Ƅ,false);
}else if(γ==Κ.Βί){double ΖΓ=0;if(ψ.Count>0) ΖΓ=Vector3.Distance(ψ.Last().Ρ,к);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΕΥ();if(Π!=Ο.ν){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ǥ?"Use old home":(Σ.λ?"Was set! ":"none ")))) ǥ=!ǥ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ǥ?"Use old connector":(Σ.λ?"Was set! ":"none ")))) ǥ=!ǥ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(ĵ?"Use old connector":(Φ.λ?"Was set! ":"none ")))) ĵ=!ĵ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(Ķ?"Use old path":(ψ.Count>1?"Count: "+ψ.Count:"none ")))) Ķ=!Ķ;Β+=ȱ;Β+="Wp spacing: "+Math.Round(Ζύ)+"m\n";}else if(γ==Κ.Ʈ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){Ǫ();Χ(Κ.Main);}
Β+=ȱ;Β+="State: "+ΑА(ś)+" \n";Β+="Speed: "+Math.Round(Ζθ,1)+"m/s\n";Β+="Target dist: "+Γσ+"m\n";Β+="Wp count: "+ψ.Count+"\n";Β+="Wp left: "+Ζώ+"\n";}else if(γ==Κ.ΒΘ){List<IMyTerminalBlock>ǭ=ΔΨ();if(Ħ) Ǚ++;if(Ǚ>=ǭ.Count) Ǚ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Κ.ΒΙ);
Β+=ȱ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΙ,1)+"% "+Math.Round(Ǭ,1)+" / "+Math.Round(Ƿ,1)+"\n";Β+="Uranium: "+(ΑΫ.Count==0?"No reactors":Math.Round(Γω,1)+"Kg "+ΖΗ)+"\n";Β+="Battery: "+(ď==џ.None?"":Βτ+"% ")+ΑЊ(ď)+"\n";Β+="Hydrogen: "+(Ĥ.Count==0?"No tanks":Math.Round(ΓΚ,1)+"% ")+"\n";
Β+="Damage: "+(ǭ.Count==0?"None":""+(Ǚ+1)+"/"+ǭ.Count+" "+ǭ[Ǚ].CustomName)+"\n";}else if(γ==Κ.ΒΙ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Κ.Main);Β+=ȱ;Β+="Next scan: "+Ÿ+"s\n";Β+="Drill area: "+Math.Round(Ȃ,1)+"x"+Math.Round(ȁ,1)+"m Ship: "+Math.Round(Γξ,1)+"x"+Math.Round(Γν,1)+"m ("+Math.Round(ȯ,1)+"m)\n"+(Επ.Count>0?"Cameras: "+Εο+"\n":"");
Β+="Broadcast: "+(đ?"Online - "+Į:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȟ*100f,1)+"% \n";}else if(γ==Κ.ΒΗ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Κ.Main);Β+=ȱ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Αβ==2) Β="Fatal setup error\nNext scan: "+Ÿ+"s\n";if(Γθ) Β="Recompile script now";int ΖΕ=Β.Split('\n').Length;for(int i=ΖΕ;i<=10;i++) Β+="\n";Β+=Ȯ;Β+="Last: "+Ψ+"\n";
return Β;}void ΑЁ(bool ΕБ,float Ż){Ǫ();Ư(true,false);if(ΕБ) В(ref ĸ,Ż,ς.ơ,false);else В(ref ġ,Ż,ς.ơ,false);ΓЋ();Ő(true,true,0,0);}float ΑЃ(float Ȕ,float[]ž){float έ=ž[0];for(int i=ž.Length-1;i>=0;i--)
if(Ȕ<ž[i]) έ=ž[i];return έ;}String Ń(ref int ǘ){String έ="";if(ǘ>=Γφ.Count) ǘ=-1;if(ǘ>=0) έ=Γφ[ǘ].CustomName;ǘ++;return έ;}void Ȇ(ref int value,int Đ,int ż,int Αη){if(Đ==0) return;if(value<ż&&Αη>0||value<=ż&&Αη<0){value+=Αη;return;}int Εθ=Math.Abs(Đ);int Ƶ=0;
int ΑΣ=1;while(true){Ƶ+=ΑΣ*Εθ*10;if(Đ<0&&value-ż<=Ƶ) break;if(Đ>0&&value-ż<Ƶ) break;ΑΣ++;}value+=ΑΣ*Đ;}void ȏ(bool ΖЏ){Х=Math.Max(Х,1);Ф=Math.Max(Ф,1);г=Math.Max(г,0);Ő(γ==Κ.ą,false,0,0);}void В(ref float value,float Ż,ς Ћ,bool ĳ){В(ref value,value+Ż,Ћ,ĳ,true);
}void В(ref float value,float Ȣ,ς Ћ,bool ĳ,bool ΖЂ){float Ы=0,Щ=0;if(Ћ==ς.ư){Ы=0.5f;Щ=10f;}if(Ћ==ς.Ƅ){Ы=0.1f;Щ=1f;}if(Ћ==ς.Ɩ){Ы=50f;Щ=100f;}if(Ћ==ς.ΒΣ){Ы=5f;Щ=30f;}if(Ћ==ς.Ɵ){Ы=1f;Щ=25f;}if(Ћ==ς.Ɲ){Ы=10f;Щ=95f;}if(Ћ==ς.ΒΤ){Ы=10f;Щ=90f;}
if(Ћ==ς.ΕΫ){Ы=10f;Щ=1800;}if(Ћ==ς.ơ){Ы=0.0f;Щ=0.75f;}if(Ȣ==-1&&ĳ){value=-1;return;}if(value==-1) ĳ=false;bool Γΰ=Ȣ<Ы||Ȣ>Щ;if(Γΰ&&ΖЂ){if(Ȣ<value) value=Щ;else if(Ȣ>value) value=Ы;}else value=Ȣ;if(Γΰ&&ĳ) value=-1;else value=Math.Max(Ы,Math.Min(value,Щ));
value=(float) Math.Round(value,2);}void Γύ(bool ΖΪ){if(ΖΪ) п=Math.Max(0,п-1);else п++;ΔΜ();ΔΛ();for(int k=0;k<ΑΑ.Count&&k<ж.Length;k++) ΕΙ(ΑΑ[k],k<п);ǹ=-1;Βξ(true);}T ǎ<T>(T Ȕ){int Α=Array.IndexOf(Enum.GetValues(Ȕ.GetType()),Ȕ);Α++;if(Α>=ΔΝ(Ȕ)) Α=0;return(T) Enum.GetValues(Ȕ.GetType()).GetValue(Α);
}int ΔΝ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΕΟ(){ƀ.Clear();for(int i=0;i<ψ.Count;i++) ƀ.Add(ψ[i]);ψ.Clear();ΓΛ=true;ΓΖ=new ά(Σ);ΓΝ=new ά(Φ);Σ.λ=false;if(Π==Ο.ν) Φ.λ=false;for(int i=0;i<ѝ.Count;i++)
if(!ў.Contains(ѝ.Keys.ElementAt(i)))
ў.Add(ѝ.Keys.ElementAt(i));ǥ=false;ĵ=false;Ķ=false;Χ(Κ.Βί);}void ΕΥ(){if(ǥ) Σ=ΓΖ;if(ĵ) Φ=ΓΝ;if(Ķ){ψ.Clear();for(int i=0;i<ƀ.Count;i++) ψ.Add(ƀ[i]);}ΓΛ=false;Ǫ();Χ(Κ.Main);}void ň(ά е,Τ ώ){Εω=е;if(υ==Τ.Ǧ) ǝ=ώ;}bool ΑЍ(ά е,float р,bool Ζω,out Vector3 Ω){if(Ζω){Vector3I ΕІ=new Vector3I((int) е.Ц.X,(int) е.Ц.Y,(int) е.Ц.Z);
IMySlimBlock α=Me.CubeGrid.GetCubeBlock(ΕІ);if(α==null||!(α.FatBlock is IMyShipConnector)){Ω=new Vector3();return false;}Vector3 Ευ=ł(Љ,α.FatBlock.GetPosition()-к);Vector3 Ετ=ł(Љ,α.FatBlock.WorldMatrix.Forward);Ω=е.Ρ-Βλ(е.Є,е.Ϗ*-1,Ευ)-Βλ(е.Є,е.Ϗ*-1,Ετ)*р;return true;}
else{Ω=е.Ρ;return true;}}Vector3 Δί(int ƾ,int Ǣ,bool Ζέ){if(!Ζέ&&ΒЏ) return ƻ;float Εσ=((й-1f)/2f)-ƾ;float Ζί=((и-1f)/2f)-Ǣ;ƻ=Φ.Ρ+Φ.Н*Εσ*Ȃ+ȡ*-1*Ζί*ȁ;ΒЏ=true;return ƻ;}Υ Ǝ(){float У=-1;Υ έ=Υ.м;if(Π!=Ο.ν){Δχ();if(υ!=Τ.ю){Vector3 ϋ=Βμ(ĩ,ȡ*-1,к-Φ.Ρ);
if(Math.Abs(ϋ.X)<=(float)(й*Ȃ)/2f&&Math.Abs(ϋ.Y)<=(float)(и*ȁ)/2f){if(ϋ.Z<=-1&&ϋ.Z>=-Αί*2) return Υ.Ļ;if(ϋ.Z>-1&&ϋ.Z<ȯ*2) return Υ.м;}if(Ǎ(Φ.Ρ,ref У)) έ=Υ.м;}if(Σ.λ){if(Ǎ(Σ.Ρ,ref У)) έ=Υ.л;for(int i=0;i<ψ.Count;i++)
if(Ǎ(ψ[i].Ρ,ref У)) έ=Υ.я;if(Vector3.Distance(к,Σ.Ρ)<dockDist*ȯ) έ=Υ.л;if(ȉ(MyShipConnectorStatus.Connectable)!=null||ȉ(MyShipConnectorStatus.Connected)!=null)
έ=Υ.л;}}else{Vector3 Ω=new Vector3();IMyShipConnector Ŗ=ȉ(MyShipConnectorStatus.Connected);if(Σ.λ){if(Ǎ(Σ.Ρ,ref У)) έ=Υ.л;if(ΑЍ(Σ,dockDist,true,out Ω))
if(Ǎ(Ω,ref У)) έ=Υ.л;if(Ŗ!=null&&Vector3.Distance(Ŗ.GetPosition(),Σ.Ρ)<5)
return Υ.Ɔ;}for(int i=0;i<ψ.Count;i++)
if(Vector3.Distance(ψ[i].Ρ,Σ.Ρ)>dockDist*ȯ&&Vector3.Distance(ψ[i].Ρ,Φ.Ρ)>dockDist*ȯ)
if(Ǎ(ψ[i].Ρ,ref У)) έ=Υ.я;if(Φ.λ){if(Ǎ(Φ.Ρ,ref У)) έ=Υ.м;if(ΑЍ(Φ,dockDist,true,out Ω))
if(Ǎ(Ω,ref У)) έ=Υ.м;if(Ŗ!=null&&Vector3.Distance(Ŗ.GetPosition(),Φ.Ρ)<5)
return Υ.Ƈ;}}return έ;}bool Ǎ(Vector3 Ω,ref float У){float р=Vector3.Distance(Ω,к);if(р<У||У==-1){У=р;return true;}return false;}void Βθ(){if(Αβ>0){Ψ="Setup error! Can't start";return;}if(Π==Ο.ν){ΑЂ();return;}Φ.Ρ=к;Φ.ξ=Љ.GetNaturalGravity();Φ.Є=ΕЂ;Φ.Ϗ=Εϋ;Φ.Н=ΖΔ;
ĩ=ǡ.WorldMatrix.Forward;ȡ=Φ.Ϗ;if(ĩ==Љ.WorldMatrix.Down) ȡ=Љ.WorldMatrix.Backward;Ư(true,true);δ(Μ.Ƒ);Ƭ();}void Ư(bool Ч,bool Ζή){if(υ==Τ.ю&&!Ч) return;bool ΖΞ=Ч||υ==Τ.ΑЅ||й!=Х||и!=Ф||ț!=Ǿ;if(ΖΞ){if(υ!=Τ.ю){υ=Τ.Ľ;Δί(ť,Ŧ,Ζή);Ψ="Job changed, lost progress";}ț=Ǿ;
й=Х;и=Ф;Ŧ=0;ť=0;ж="";ǹ=-1;ΑΞ=0;ħ=0;ΒЁ=0;п=0;Βξ(true);}}void ƣ(){ΕΗ(к,0);Ň(Ĳ,true);}void Ƭ(){Βε(Ǔ.Ββ);ƣ();Ʃ(ΑΕ,false);ƪ(Αι);υ=Τ.Ǧ;ΓЊ(true);ǝ=υ;Χ(Κ.Ą);ΔБ();ΒЂ=true;ΑΒ=0;for(int i=Č.Count-1;i>=0;i--)
if(ΒΛ(Č[i],false)) ΑΒ++;if(ΑΒ>0) Ψ="Started with damage";}void Ǫ(){if(υ==Τ.Ǧ){υ=Τ.Ƣ;Ψ="Job paused";}δ(Μ.ǧ);ǝ=υ;ΕΘ(false,0,0,0,0);ΕΣ();ΓЀ(new Vector3(),false);ΕΤ();ΕΔ(ChargeMode.Auto);ΕΚ(false);ΕΖ(true);ǩ(Μ.ǧ);Ő(false,false,0,0);Ň(Я,false);Ň(ĕ,true);ƪ(true);Γι=false;ΓЎ();
Εϊ=false;ΔЊ();ΒЂ=false;Αε=false;Γψ=false;if(γ!=Κ.Main&&γ!=Κ.ļ&&γ!=Κ.ƅ&&γ!=Κ.Ŋ)
Χ(Κ.Main);}void ΑЂ(){Υ љ=Ǝ();if(Π==Ο.ν){if(!Φ.λ||!Σ.λ) return;Ƭ();bool ǉ=Vector3.Distance(к,Σ.Ρ)<Vector3.Distance(к,Φ.Ρ);if(Αΰ==Τ.Ĺ) ǉ=true;if(Αΰ==Τ.ĺ) ǉ=false;if(ǉ){ň(Σ,Τ.Ĺ);switch(љ){case Υ.Ɔ:δ(Μ.ċ);break;case Υ.я:δ(Μ.ǐ);break;case Υ.л:δ(Μ.ȇ);break;default:δ(Μ.ĉ);break;
}}else{ň(Φ,Τ.ĺ);switch(љ){case Υ.Ƈ:δ(Μ.ċ);break;case Υ.м:δ(Μ.ȇ);break;case Υ.я:δ(Μ.ǐ);break;default:δ(Μ.ĉ);break;}}}else{if(υ!=Τ.Ƣ&&υ!=Τ.Ľ) return;bool Ζό=υ==Τ.Ľ;Ƭ();bool ǋ=Δσ(false)&&Σ.λ;switch(љ){case Υ.м:δ(ǋ?Μ.Ɛ:Μ.Ƒ);break;case Υ.я:δ(ǋ?Μ.ă:Μ.ǐ);break;case Υ.л:δ(ǋ?Μ.ȇ:Μ.Ċ);break;
case Υ.Ļ:{if(ħ!=п||Ζό) δ(Μ.ƥ);else δ(Μ.ΒΥ);}break;default:break;}}}void ΔΥ(){if(υ==Τ.ю&&!Σ.λ) return;if(Π==Ο.ν&&(!Φ.λ||!Σ.λ)) return;Ψ="Move to job";Υ љ=Ǝ();if(Π==Ο.ν){ň(Φ,Τ.ĺ);switch(љ){case Υ.м:δ(Μ.ȇ);break;case Υ.я:δ(Μ.ǐ);break;case Υ.Ƈ:return;default:δ(Μ.ĉ);break;
}ǩ(Μ.ċ);}else{switch(љ){case Υ.м:δ(Μ.ǐ);break;case Υ.я:δ(Μ.ǐ);break;case Υ.л:δ(Μ.Ċ);break;case Υ.Ļ:δ(Μ.ƥ);break;default:break;}if(υ==Τ.ю) ǩ(Μ.ǐ);else ǩ(Μ.Αύ);Γι=true;}ƣ();Χ(Κ.Ʈ);Ʃ(ΑΕ,false);ǝ=Τ.ΒΨ;}void ΔΡ(){if(!Σ.λ) return;Ψ="Move home";Υ љ=Ǝ();if(Π==Ο.ν){ň(Σ,Τ.Ĺ);switch(љ)
{case Υ.я:δ(Μ.ă);break;case Υ.л:δ(Μ.ȇ);break;case Υ.Ɔ:return;default:δ(Μ.ĉ);break;}ǩ(Μ.ċ);}else{if(ȉ(MyShipConnectorStatus.Connected)!=null) return;if(ȉ(MyShipConnectorStatus.Connectable)!=null){δ(Μ.ΑЄ);ǩ(Μ.Ċ);return;}switch(љ){case Υ.м:δ(Μ.Ɛ);break;case Υ.я:δ(Μ.ă);break;case Υ.л:δ(Μ.ă);break;
case Υ.Ļ:δ(Μ.Βα);break;default:break;}ǩ(Μ.Ċ);}ƣ();Χ(Κ.Ʈ);Ʃ(ΑΕ,false);ǝ=Τ.ΒΧ;}void δ(Μ ΑΤ){if(ΑΤ==Μ.ǧ) İ=Μ.ǧ;if(İ!=Μ.ǧ&&ś==İ&&ΑΤ!=İ){Ǫ();return;}Ζμ=true;ś=ΑΤ;}void ǩ(Μ ώ){this.İ=ώ;}void Βξ(bool Αά){if(Αά){ū=0;return;}if(Π==Ο.ν) return;float τ=п*Math.Max(1,г);
if(ħ==п) τ+=Math.Min(г,ΒЁ);float Ζχ=й*и*Math.Max(1,г);ū=Math.Max(ū,(float) Math.Min(τ/Ζχ*100.0,100));}List<IMyTerminalBlock>ΔΨ(){List<IMyTerminalBlock>ǭ=new List<IMyTerminalBlock>();for(int i=0;i<Č.Count;i++){IMyTerminalBlock α=Č[i];if(ΒΛ(α,false)) ǭ.Add(α);}return ǭ;
}bool ΒΛ(IMyTerminalBlock α,bool Αμ){return(!ΒΜ(α,Αμ)||!α.IsFunctional);}bool ΒΜ(IMyTerminalBlock α,bool Αμ){if(α==null) return false;try{IMyCubeBlock ΒЎ=Me.CubeGrid.GetCubeBlock(α.Position).FatBlock;if(Αμ) return ΒЎ==α;else return ΒЎ.GetType()==α.GetType();}catch{return false;}}bool Δσ(bool ΖЍ)
{if(ȅ&&Π!=Ο.ν)
if(Ѐ!=-1&&Γο>=Ѐ){Ψ="Ship too heavy";return true;}if(ΑΙ>=х||Αε){Αε=false;Ψ="Ship is full";return true;}return false;}static String Δε(IMyThrust ъ){return ъ.BlockDefinition.SubtypeId;}Vector3 Ɯ(Vector3 Й,float[,]Ȟ){return new Vector3(
Й.X>=0?Ȟ[0,0]:Ȟ[0,1],
Й.Y>=0?Ȟ[1,0]:Ȟ[1,1],
Й.Z>=0?Ȟ[2,0]:Ȟ[2,1]);}bool ΒΓ(ά ή,String ȃ,out float Ǆ){Ǆ=0;int Α=ў.IndexOf(ȃ);if(Α==-1||ή.Ю==null||Α>=ή.Ю.Length)
return false;Ǆ=ή.Ю[Α];if(Ǆ==-1) return false;return true;}Vector3 ΒΒ(Vector3 Й,ά ή){if(ή!=null){Vector3 έ=new Vector3();for(int i=0;i<ѝ.Keys.Count;i++){String ȃ=ѝ.Keys.ElementAt(i);float ę=0;if(!ΒΓ(ή,ȃ,out ę))
return Ɯ(Й,Ĵ);έ+=Ɯ(Й,ѝ.Values.ElementAt(i))*ę;}return έ;}return Ɯ(Й,Ĵ);}float Ɣ(Vector3 Й,ά ή){return Ɣ(Й,new Vector3(),ή);}float Ɣ(Vector3 Й,Vector3 ξ,ά ή){Vector3 Ζρ=ΒΒ(Й,ή);Vector3 Εύ=Ζρ+ξ*Γο;float ǻ=(Εύ/Й).AbsMin();return(float)(Й*ǻ).Length();}float ŀ(Vector3 Ğ,Vector3 Ё,Vector3 ξ,ά ή)
{if(ξ.Length()==0f) return 0;Vector3 ΖΖ=Βμ(Ğ,Ё,Vector3.Normalize(ξ));float Ч=Ɣ(-ΖΖ,ή);return Ч/ξ.Length();}void ΓЊ(bool ΕЍ){float П=0;float ǅ=0.9f;if(ΕЍ){Ѐ=-1;Ȅ=0;Ź=null;if(υ!=Τ.ю&&Φ.ξ.Length()!=0){П=ǅ*ŀ(Φ.Є,Φ.Ϗ*-1,Φ.ξ,null);if(П<Ѐ||Ѐ==-1) Ѐ=П;}if(Σ.λ&&Σ.ξ.Length()!=0)
{П=ǅ*ŀ(Σ.Є,Σ.Ϗ*-1,Σ.ξ,null);if(П<Ѐ||Ѐ==-1) Ѐ=П;}return;}if(Ȅ==-1) return;if(Ȅ>=0){int Γδ=0;while(Ȅ<ψ.Count){if(Γδ>100) return;Γδ++;ά ή=ψ[Ȅ];if(ή.ξ.Length()!=0f){П=ǅ*Math.Min(
ŀ(ή.Є,ή.Ϗ*-1,ή.ξ,ή),
ŀ(ή.Є*-1,ή.Ϗ*-1,ή.ξ,ή));if(П<Ѐ||Ѐ==-1) Ѐ=П;}else Ź=ή;Ȅ++;}Ȅ=-1;}bool Ǌ=true;float ǟ=0;if(Ź!=null){for(int i=0;i<ѝ.Count;i++){String ȃ=ѝ.Keys.ElementAt(i);float[,]Αυ=ѝ.Values.ElementAt(i);float ę=0;if(!ΒΓ(Ź,ȃ,out ę)){Ǌ=false;break;}for(int a=0;a<Αυ.GetLength(0);a++)
for(int b=0;b<Αυ.GetLength(1);b++){float ǫ=Math.Abs(Αυ[a,b]*ę);if(ǫ==0) continue;Ǌ=true;if(ǟ==0||ǫ<ǟ) ǟ=ǫ;}}}if(!Ǌ){for(int a=0;a<Ĵ.GetLength(0);a++)
for(int b=0;b<Ĵ.GetLength(1);b++){float ǫ=Math.Abs(Ĵ[a,b]);if(ǫ==0) continue;if(ǟ==0||ǫ<ǟ) ǟ=ǫ;}}if(ǟ>0){float ΖΚ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;П=Р(ǟ,ΖΚ);if(П>0)
if(П<Ѐ||Ѐ==-1) Ѐ=П;}}void ΓЀ(Vector3 Ч,bool ύ){if(!ύ){for(int i=0;i<Ĳ.Count;i++)
Ĳ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ʒ=ΒΒ(Ч,null);float ΖЃ=Math.Min(1,Math.Abs(Р(Ч.X,Ʒ.X)));float ΖЄ=Math.Min(1,Math.Abs(Р(Ч.Y,Ʒ.Y)));float ΖЅ=Math.Min(1,Math.Abs(Р(Ч.Z,Ʒ.Z)));for(int i=0;i<Ĳ.Count;i++){IMyThrust ъ=Ĳ[i];Vector3 Ȗ=ΔЏ(ł(Љ,ъ.WorldMatrix.Backward),1);
if(Ȗ.X!=0&&Math.Sign(Ȗ.X)==Math.Sign(Ч.X))
ъ.SetValueFloat("Override",ъ.MaxThrust*ΖЃ);else if(Ȗ.Y!=0&&Math.Sign(Ȗ.Y)==Math.Sign(Ч.Y))
ъ.SetValueFloat("Override",ъ.MaxThrust*ΖЄ);else if(Ȗ.Z!=0&&Math.Sign(Ȗ.Z)==Math.Sign(Ч.Z))
ъ.SetValueFloat("Override",ъ.MaxThrust*ΖЅ);else
ъ.SetValueFloat("Override",0.0f);}}void ΕΘ(bool ύ,float ΖΩ,float Ȧ,float Ȫ,float ā){for(int i=0;i<ΒБ.Count;i++){IMyGyro ο=ΒБ[i];ο.GyroOverride=ύ;if(!ύ) ο.GyroPower=100;else ο.GyroPower=ΖΩ;if(!ύ) continue;Vector3 Л=Љ.WorldMatrix.Forward;Vector3 ȩ=Љ.WorldMatrix.Right;Vector3 Ё=Љ.WorldMatrix.Up;
Vector3 ȗ=ο.WorldMatrix.Forward;Vector3 ș=ο.WorldMatrix.Up;Vector3 Ș=ο.WorldMatrix.Left*-1;if(ȗ==Л) ο.SetValueFloat("Roll",ā);else if(ȗ==(Л*-1)) ο.SetValueFloat("Roll",ā*-1);else if(ș==(Л*-1)) ο.SetValueFloat("Yaw",ā);else if(ș==Л) ο.SetValueFloat("Yaw",ā*-1);else if(Ș==Л) ο.SetValueFloat("Pitch",ā);
else if(Ș==(Л*-1)) ο.SetValueFloat("Pitch",ā*-1);if(Ș==(ȩ*-1)) ο.SetValueFloat("Pitch",Ȧ);else if(Ș==ȩ) ο.SetValueFloat("Pitch",Ȧ*-1);else if(ș==ȩ) ο.SetValueFloat("Yaw",Ȧ);else if(ș==(ȩ*-1)) ο.SetValueFloat("Yaw",Ȧ*-1);else if(ȗ==(ȩ*-1)) ο.SetValueFloat("Roll",Ȧ);
else if(ȗ==ȩ) ο.SetValueFloat("Roll",Ȧ*-1);if(ș==(Ё*-1)) ο.SetValueFloat("Yaw",Ȫ);else if(ș==Ё) ο.SetValueFloat("Yaw",Ȫ*-1);else if(Ș==Ё) ο.SetValueFloat("Pitch",Ȫ);else if(Ș==(Ё*-1)) ο.SetValueFloat("Pitch",Ȫ*-1);else if(ȗ==Ё) ο.SetValueFloat("Roll",Ȫ);else if(ȗ==(Ё*-1)) ο.SetValueFloat("Roll",Ȫ*-1);
}}void ΕΤ(){this.ΕЇ=false;}void ΕΣ(){ŉ(false,false,false,Γή,0);Γυ=false;}void ΕΗ(Vector3 ί,float į){ŉ(true,false,false,ί,į);}void ŉ(bool ύ,bool Αθ,bool Αφ,Vector3 ί,float į){ŉ(ύ,Αθ,Αφ,ί,ί-к,0.0f,į);}void ŉ(bool ύ,bool Αθ,bool Αφ,Vector3 ί,Vector3 Ζδ,float Ζε,float į)
{Γυ=true;this.Ζς=ύ;Γή=ί;this.ΖΘ=į;this.ΖΦ=Ζε;this.Ζκ=Αθ;this.Ζι=Αφ;this.ΕЀ=Ζδ;ΒЇ=Vector3.Distance(ί,к);}void ΔБ(){М.GetBlocksOfType(Č,ƚ);}void ΓЋ(){if(Љ==null) return;ǡ=null;float Ų=0,Ů=0,ų=0,ů=0,Ŵ=0,Ű=0;List<IMyTerminalBlock>Αν=ΑЇ(Я,pamTag,true);bool ΖΠ=Αν.Count==0;
if(Αν.Count>0) ǡ=Αν[0];else if(Я.Count>0) ǡ=Я[0];int Ś=0;float ш=0;for(int i=0;i<Я.Count;i++){if(Я[i].WorldMatrix.Forward!=ǡ.WorldMatrix.Forward){if(ΖΠ){Αβ=2;Ψ="Mining direction is unclear!";return;}continue;}Ś++;ш=Math.Max(ш,Εά(Я[i]));Vector3 ϋ=Ɠ(Љ,Я[i].GetPosition());
if(i==0){Ų=ϋ.X;Ů=ϋ.X;ų=ϋ.Y;ů=ϋ.Y;Ŵ=ϋ.Z;Ű=ϋ.Z;}Ů=Math.Max(ϋ.X,Ů);Ų=Math.Min(ϋ.X,Ų);ů=Math.Max(ϋ.Y,ů);ų=Math.Min(ϋ.Y,ų);Ű=Math.Max(ϋ.Z,Ű);Ŵ=Math.Min(ϋ.Z,Ŵ);}if(ш==0) ш=drillRadius;bool ė=ǡ!=null&&ǡ.WorldMatrix.Forward==Љ.WorldMatrix.Down;Ȃ=(Ů-Ų)*(1-ĸ)+ш*2;
ȁ=(ė?Ű-Ŵ:ů-ų)*(1-ġ)+ш*2;IMyCubeGrid g=Љ.CubeGrid;float Αω=0,Αϊ=0,Αϋ=0,Αό=0;for(int c=0;c<8;c++){Vector3 p=Ɠ(Љ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ė?p.Z:p.Y;if(c==0||p.X<Αω) Αω=p.X;if(c==0||p.X>Αϊ) Αϊ=p.X;if(c==0||y<Αϋ) Αϋ=y;if(c==0||y>Αό) Αό=y;}Γξ=Αϊ-Αω+g.GridSize;Γν=Αό-Αϋ+g.GridSize;}float Εά(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΑЇ<T>(List<T>Ѕ,String Э,bool ŧ){List<T>έ=new List<T>();for(int i=0;i<Ѕ.Count;i++)
if(ΒΖ(Ѕ[i],Э,ŧ)) έ.Add(Ѕ[i]);return έ;}bool ΒΖ<T>(T α,String Э,bool ŧ){IMyTerminalBlock b=(IMyTerminalBlock) α;if(ŧ&&b.CustomName.ToUpper().Contains(Э.ToUpper())) return true;if(!ŧ&&b.BlockDefinition.ToString().ToUpper().Contains(Э.ToUpper())) return true;return false;}void Ň<T>(List<T>Ѕ,bool ύ)
{for(int i=0;i<Ѕ.Count;i++)
ΕΕ((IMyTerminalBlock) Ѕ[i],ύ);}void ΕΕ(IMyTerminalBlock α,bool ύ){if(α==null) return;String Ει=ύ?"OnOff_On":"OnOff_Off";var a=α.GetActionWithName(Ει);a.Apply(α);}void ƪ(bool ύ){Αι=ύ;if(!Ȁ) return;Ň(Ζλ,ύ);}void ΕΔ(ChargeMode θ){for(int i=0;i<Βσ.Count;i++) Βσ[i].ChargeMode=θ;}void ΕΚ(bool Ζξ)
{for(int i=0;i<Ĥ.Count;i++) Ĥ[i].Stockpile=Ζξ;}void Ʃ(List<IMyLandingGear>ǈ,bool ΓΧ){for(int i=0;i<ǈ.Count;i++){if(ΓΧ) ǈ[i].Lock();if(!ΓΧ) ǈ[i].Unlock();}}void ΕΖ(bool ύ){if(ΒЃ==ύ) return;List<IMyShipController>ř=new List<IMyShipController>();М.GetBlocksOfType(ř,ƚ);if(ř.Count==0) return;
for(int i=0;i<ř.Count;i++)
ř[i].DampenersOverride=ύ;ΒЃ=ύ;}IMyShipConnector ȉ(MyShipConnectorStatus Ζν){for(int i=0;i<ĕ.Count;i++){if(!ΒΜ(ĕ[i],true)) continue;if(ĕ[i].Status==Ζν) return ĕ[i];}return null;}void Ő(bool Αγ,bool ŝ,float ΕЃ,float Εμ){if(ό==null||Я.Count==0) return;Vector3 Ȓ=new Vector3();
int Ś=0;for(int i=0;i<Я.Count;i++){if(Я[i].WorldMatrix.Forward!=ǡ.WorldMatrix.Forward) continue;Ś++;Ȓ+=Я[i].GetPosition();}Ȓ=Ȓ/Ś;Vector3 ȣ=Ɠ(ό,Ȓ);ό.Enabled=true;ό.ShowOnHUD=Αγ;ό.LeftExtend=(ŝ?1:Х)/2f*Ȃ-ȣ.X;ό.RightExtend=(ŝ?1:Х)/2f*Ȃ+ȣ.X;ό.TopExtend=(ŝ?1:Ф)/2f*ȁ+ȣ.Y;
ό.BottomExtend=(ŝ?1:Ф)/2f*ȁ-ȣ.Y;ό.FrontExtend=(Αγ?г:ΕЃ)-ȣ.Z;ό.BackExtend=Αγ?0:Εμ+ȯ*0.75f+ȣ.Z;ό.DetectFloatingObjects=true;ό.DetectAsteroids=false;ό.DetectLargeShips=true;ό.DetectSmallShips=true;ό.DetectStations=true;ό.DetectOwner=true;ό.DetectSubgrids=false;
ό.DetectPlayers=false;ό.DetectEnemy=true;ό.DetectFriendly=true;ό.DetectNeutral=true;}void ć<T>(List<T>Ѕ,bool Ζη,float ΕЁ,bool ΕА){for(int i=0;i<Ѕ.Count;i++){IMyTextSurface ı=null;if(Ѕ[i]is IMyTextSurface) ı=(IMyTextSurface) Ѕ[i];if(ı!=null){ı.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ζη) continue;ı.Font="Debug";if(ΕА) continue;ı.FontSize=ΕЁ;}}}static float Р(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΔЏ(Vector3 v,int ǀ){return new Vector3(Math.Round(v.X,ǀ),Math.Round(v.Y,ǀ),Math.Round(v.Z,ǀ));}Vector3 Ɠ(IMyTerminalBlock α,Vector3 ΖЁ){return ł(α,ΖЁ-α.GetPosition());
}Vector3 ł(IMyTerminalBlock α,Vector3 Αψ){return Vector3D.TransformNormal(Αψ,MatrixD.Transpose(α.WorldMatrix));}Vector3 Βμ(Vector3 Ğ,Vector3 Ё,Vector3 Αψ){MatrixD ΑΝ=MatrixD.CreateFromDir(Ğ,Ё);return Vector3D.TransformNormal(Αψ,MatrixD.Transpose(ΑΝ));}Vector3 Βλ(Vector3 Ğ,Vector3 Ё,Vector3 ϋ)
{MatrixD ΑΝ=MatrixD.CreateFromDir(Ğ,Ё);return Vector3D.Transform(ϋ,ΑΝ);}String β(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ϋ(String s){String[]ζ=s.Split('|');return new Vector3(float.Parse(Ι(ζ,0)),float.Parse(Ι(ζ,1)),float.Parse(Ι(ζ,2)));}String Εη(ά ή){String ǽ=":";
String έ=β(ή.Ρ)+ǽ+β(ή.Є)+ǽ+
β(ή.Ϗ)+ǽ+β(ή.Н)+ǽ+β(ή.ξ);for(int i=0;i<ή.Ю.Length;i++)
έ+=ǽ+Math.Round(ή.Ю[i],3);return έ;}ά ΕΦ(String s){String[]ζ=s.Split(':');ά ή=new ά(
Ϋ(Ι(ζ,0)),Ϋ(Ι(ζ,1)),
Ϋ(Ι(ζ,2)),Ϋ(Ι(ζ,3)),
Ϋ(Ι(ζ,4)));int Α=5;List<float>ΒЋ=new List<float>();while(Α<ζ.Length){String Ž=Ι(ζ,Α);float f=0;if(!float.TryParse(Ž,out f)) break;ΒЋ.Add(f);Α++;}ή.Ю=ΒЋ.ToArray();return ή;}System.Text.StringBuilder Ȭ=new System.Text.StringBuilder();void Λ<T>(T value,bool ΖΟ){if(ΖΟ) Ȭ.Append('\n');Ȭ.Append(value);}
void Λ<T>(T value){Λ(value,true);}String ľ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ŏ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Εκ,int Α){String Ž=Εκ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Ž)) return "";
return Ž;}void Save(){if(Γθ||Π==Ο.Г){Storage="";return;}Storage=ΓЅ();}String ΓЅ(){Ȭ.Clear();Ȭ.Append(DATAREV+";");Λ(β(Γη),false);Λ(β(Σ.Є));Λ(β(Σ.Н));Λ(β(Σ.Ϗ));Λ(β(Σ.ξ));Λ(β(Σ.Ρ));Λ(β(Σ.Ц));Λ(Σ.λ);Λ(β(Φ.Ρ));Λ(β(Φ.ξ));Λ(β(Φ.Є));Λ(β(Φ.Ϗ));Λ(β(Φ.Н));Λ(β(Φ.Ц));Λ(Φ.λ);Λ(β(ĩ));Λ(β(ȡ));Λ(";");
Λ((int) Π,false);Λ((int) υ);Λ((int) ǝ);Λ(х);Λ(ц);Λ(Ъ);Λ(ч);Λ(ǖ);Λ(ȅ);Λ(Ǥ);if(Π==Ο.ν){Λ((int) φ.О);Λ(φ.д);Λ(φ.ǰ);Λ(ľ(φ.с));Λ(ľ(φ.ф));Λ((int) χ.О);Λ(χ.д);Λ(χ.ǰ);Λ(ľ(χ.с));Λ(ľ(χ.ф));}else{Λ((int) Ǿ);Λ((int) Ǡ);Λ((int) Ě);Λ((int) і);Λ((int) ț);Λ(ķ);Λ(Ȁ);Λ(ĥ);Λ(ě);Λ(Х);Λ(Ф);Λ(г);Λ(Ƃ);Λ(Ɓ);Λ(ĸ);Λ(ġ);
Λ(й);Λ(и);Λ(ť);Λ(Ŧ);Λ(ħ);Λ(п);Λ(Αί);Λ(ΑΞ);}Λ(";");for(int i=0;i<ў.Count;i++)
Λ((i>0?"|":"")+ў[i],false);Λ(";");for(int i=0;i<ψ.Count;i++)
Λ(Εη(ψ[i]),i>0);Λ(";",false);Λ(ж+"|"+ǹ,false);Λ(";",false);Λ(ΔΗ(),false);String Ξ=Ȭ.ToString();Ȭ.Clear();return Ξ;}н ΔЌ(){return ΔЍ(Storage);}н ΔЍ(String Ξ){if(Ξ=="") return н.Δϋ;String[]Ǽ=Ξ.Split(';');if(Ι(Ǽ,0)!=DATAREV) return н.ΒΩ;int Α=0;try{String[]Η=Ι(Ǽ,1).Split('\n');
Γη=Ϋ(Ι(Η,Α++));Σ.Є=Ϋ(Ι(Η,Α++));Σ.Н=Ϋ(Ι(Η,Α++));Σ.Ϗ=Ϋ(Ι(Η,Α++));Σ.ξ=Ϋ(Ι(Η,Α++));Σ.Ρ=Ϋ(Ι(Η,Α++));Σ.Ц=Ϋ(Ι(Η,Α++));Σ.λ=bool.Parse(Ι(Η,Α++));Φ.Ρ=Ϋ(Ι(Η,Α++));Φ.ξ=Ϋ(Ι(Η,Α++));Φ.Є=Ϋ(Ι(Η,Α++));Φ.Ϗ=Ϋ(Ι(Η,Α++));Φ.Н=Ϋ(Ι(Η,Α++));Φ.Ц=Ϋ(Ι(Η,Α++));Φ.λ=bool.Parse(Ι(Η,Α++));
ĩ=Ϋ(Ι(Η,Α++));ȡ=Ϋ(Ι(Η,Α++));Η=Ι(Ǽ,2).Split('\n');Α=0;Π=(Ο) int.Parse(Ι(Η,Α++));υ=(Τ) int.Parse(Ι(Η,Α++));ǝ=(Τ) int.Parse(Ι(Η,Α++));х=int.Parse(Ι(Η,Α++));ц=int.Parse(Ι(Η,Α++));Ъ=int.Parse(Ι(Η,Α++));ч=int.Parse(Ι(Η,Α++));ǖ=float.Parse(Ι(Η,Α++));ȅ=bool.Parse(Ι(Η,Α++));
Ǥ=bool.Parse(Ι(Η,Α++));if(Π==Ο.ν){φ.О=(Ѓ) int.Parse(Ι(Η,Α++));φ.д=float.Parse(Ι(Η,Α++));φ.ǰ=float.Parse(Ι(Η,Α++));φ.с=ŏ(Ι(Η,Α++));φ.ф=ŏ(Ι(Η,Α++));χ.О=(Ѓ) int.Parse(Ι(Η,Α++));χ.д=float.Parse(Ι(Η,Α++));χ.ǰ=float.Parse(Ι(Η,Α++));χ.с=ŏ(Ι(Η,Α++));χ.ф=ŏ(Ι(Η,Α++));}else{Ǿ=(Т) int.Parse(Ι(Η,Α++));
Ǡ=(Ǐ) int.Parse(Ι(Η,Α++));Ě=(б) int.Parse(Ι(Η,Α++));і=(ь) int.Parse(Ι(Η,Α++));ț=(Т) int.Parse(Ι(Η,Α++));ķ=bool.Parse(Ι(Η,Α++));Ȁ=bool.Parse(Ι(Η,Α++));ĥ=bool.Parse(Ι(Η,Α++));ě=bool.Parse(Ι(Η,Α++));Х=int.Parse(Ι(Η,Α++));Ф=int.Parse(Ι(Η,Α++));г=int.Parse(Ι(Η,Α++));Ƃ=float.Parse(Ι(Η,Α++));
Ɓ=float.Parse(Ι(Η,Α++));ĸ=float.Parse(Ι(Η,Α++));ġ=float.Parse(Ι(Η,Α++));й=int.Parse(Ι(Η,Α++));и=int.Parse(Ι(Η,Α++));ť=int.Parse(Ι(Η,Α++));Ŧ=int.Parse(Ι(Η,Α++));ħ=int.Parse(Ι(Η,Α++));п=int.Parse(Ι(Η,Α++));Αί=int.Parse(Ι(Η,Α++));ΑΞ=float.Parse(Ι(Η,Α++));}Η=Ι(Ǽ,3).Replace("\n","").Split('|');
ў=Η.ToList();Η=Ι(Ǽ,4).Split('\n');ψ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
ψ.Add(ΕΦ(Ι(Η,i)));Η=Ι(Ǽ,5).Split('|');ж=Ι(Η,0);if(!int.TryParse(Ι(Η,1),out ǹ)) ǹ=-1;if(ж.Length!=й*и){ж="";ǹ=-1;}ΔΖ(Ι(Ǽ,6));}catch{return н.ΑІ;}Αΰ=ǝ;Ǫ();return н.Βι;}void ΕΛ(){Ǯ.Clear();for(int i=Ј.Count-1;i>=0;i--){String Ξ=Ј[i].CustomData.ToUpper();bool ΑΓ=false;
if(Ξ=="INSTRUCTIONS"){ΑΓ=true;ΒЄ=true;}if(Ξ=="DEBUG") ΑΓ=true;if(ΑΓ){Ǯ.Add(Ј[i]);Ј.RemoveAt(i);}}ć(Ǯ,false,1,false);}void ΕΜ(List<IMyTerminalBlock>Βφ){щ.Clear();for(int i=0;i<Βφ.Count;i++){IMyTerminalBlock α=Βφ[i];try{String Γγ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=α.CustomName.IndexOf(Γγ);int Ȱ=-1;if(Α<0||!int.TryParse(α.CustomName.Substring(Α+Γγ.Length,1),out Ȱ)) continue;if(Ȱ==-1) continue;Ȱ--;IMyTextSurfaceProvider Γε=(IMyTextSurfaceProvider) α;if(Ȱ<Γε.SurfaceCount&&Ȱ>=0)
щ.Add(Γε.GetSurface(Ȱ));}catch{}}}String ΑΠ="";void ΔЈ(){String ΑΜ="";String Ę="";ΑΠ=ΓЄ(false);ΑΜ+=ΑΠ;Ę+=ΑΠ;Ę+=Δέ();for(int i=0;i<Ј.Count;i++) Ј[i].WriteText(ΑΜ);for(int i=0;i<щ.Count;i++) щ[i].WriteText(ΑΜ);Echo(Ę);for(int i=0;i<Ǯ.Count;i++){IMyTextPanel Ѝ=Ǯ[i];
String Ξ=Ѝ.CustomData.ToUpper();if(Ξ=="DEBUG") Ѝ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ѝ.WriteText(ΑЎ());}}string ΑЎ(){String Β="";try{float ΓΨ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ȑ*ΓΨ)+" / "+(int)(ȟ*ΓΨ)+"\n";
Β+="Inst. avg/max: "+Math.Round(ȑ*100f,1)+"% / "+Math.Round(ȟ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ď,2)+"ms / "+Math.Round(ΑΟ,2)+"ms \n";}catch{}for(int i=0;i<Ŷ.Count;i++)
Β+=""+Ŷ.Keys.ElementAt(i)+" = "+Math.Round(Ŷ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ŷ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Δκ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ć(Ј,setLCDFontAndSize,0.9f,false);ć(щ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ј.Count;i++) Ј[i].WriteText(Β+ȝ.ToString());for(int i=0;i<щ.Count;i++) щ[i].WriteText(Β+ȝ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Ƨ(String ε,String ί,String Ǳ){try{if(!đ) return;ε=BROADCAST_TAG+BROADCAST_SEP+ΑЋ()+BROADCAST_SEP+ί+BROADCAST_SEP+ƺ+BROADCAST_SEP+Ǳ+BROADCAST_SEP+ε;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ε);}catch(Exception e){ȝ=e;}}String ΑЋ(){if(Π!=Ο.Г) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΒΝ(String ε){return ε.StartsWith(BROADCAST_TAG);}bool Ђ(ref String Ξ,out string Ĝ,Char ǽ){int Α=Ξ.IndexOf(ǽ);Ĝ="";if(Α<0) return false;Ĝ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΔЄ(ref String ε,out String ǣ,out String Ǳ){ǣ="";Ǳ="";if(!đ) return false;
String Э="";if(!Ђ(ref ε,out Э,BROADCAST_SEP)||!ΒΝ(Э)) return false;if(!Ђ(ref ε,out ǣ,BROADCAST_SEP)) return false;if(!Ђ(ref ε,out Э,BROADCAST_SEP)||(Э!=ΑЋ()&&(Э!="*"&&Π!=Ο.Г))) return false;if(!Ђ(ref ε,out Э,BROADCAST_SEP)||(Э!=ƺ)) return false;if(!Ђ(ref ε,out Ǳ,BROADCAST_SEP)) return false;
return true;}void Δρ(){bool Ĕ=true;if(ΒЌ){ΒЌ=false;if(Me.CustomData.Contains("Antenna_Name")){Ψ="Update custom data";Me.CustomData="";}}String ğ=(Π!=Ο.Г?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ğ)
ΒΚ(ğ);String[]Η=Me.CustomData.Split('\n');đ=bool.Parse(Ņ(Ŀ(Η,"Enable_Broadcast",ref Ĕ)));bool Αχ=false;bool ΒІ=true;if(đ){if(Π!=Ο.Г)
Į=Ņ(Ŀ(Η,"Ship_Name",ref Ĕ)).Replace(BROADCAST_SELF_ID,"");ƺ=Ņ(Ŀ(Η,"Broadcast_Channel",ref Ĕ)).ToLower();ΒІ=false;if(ё==null){ё=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ё.SetMessageCallback("");}List<IMyRadioAntenna>Ȑ=new List<IMyRadioAntenna>();М.GetBlocksOfType(Ȑ);bool ƶ=false;
for(int i=0;i<Ȑ.Count;i++)
if(Ȑ[i].EnableBroadcasting&&Ȑ[i].Enabled){ƶ=true;break;}if(Ȑ.Count==0) Ψ="No Antenna found";else if(!ƶ) Ψ="Antenna not ready";Αχ=Ȑ.Count==0||!ƶ;if(Βο&&!Αχ&&Π!=Ο.Г) Ψ="Antenna ok";}else if(Π==Ο.Г)
Ψ="Offline - Enable in PB custom data";Βο=Αχ;if(ΒІ){if(ё!=null) this.IGC.DisableBroadcastListener(ё);ё=null;}}catch{Ĕ=false;}if(!Ĕ){Ψ="Reset custom data";ΒΚ(ğ);}}void ΒΚ(String ğ){String Ξ=ğ+"\n\n"+"Enable_Broadcast="+(Π==Ο.Г?"true":"false")+" \n";Ξ+=Π!=Ο.Г?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String Ņ(String Ƕ){int Α=Ƕ.IndexOf("//");if(Α!=-1) Ƕ=Ƕ.Substring(0,Α);String[]ζ=Ƕ.Split('=');if(ζ.Length<=1) return "";return ζ[1].Trim();}String Ŀ(String[]Η,String Ж,ref bool Ȥ){foreach(String Ƕ in Η)
if(Ƕ.StartsWith(Ж)) return Ƕ;Ȥ=false;return "";}void ΔΑ(string Ў){if(ĝ) σ=new List<ΰ>();String ǣ="";String Ǳ="";if(!ΒΝ(Ў)) ΔЉ(Ў);bool Γπ=false;int ΖΛ=50;while(ё!=null&&ё.HasPendingMessage&&ΖΛ-->0){MyIGCMessage ε=ё.AcceptMessage();String Ξ=ε.Data as String;if(Ξ==null) continue;
if(ΔЄ(ref Ξ,out ǣ,out Ǳ)&&ǣ!=""&&ǣ!=BROADCAST_SELF_ID){ΰ Γ=Ə(ǣ,"");if(Γ==null){Γ=new ΰ(ǣ);σ.Add(Γ);}Γ.ǒ(Ǳ,false,false,true);Εα(Γ,Ξ);Γπ=true;}}if(Γπ)
σ.Sort(delegate(ΰ a,ΰ b){if(a.ι==null&&b.ι==null) return 0;else if(a.ι==null) return-1;else if(b.ι==null) return 1;else return a.ι.CompareTo(b.ι);});if(Ħ||ĝ){if(Ÿ<=0||ĝ){Ψ="";ĝ=false;ΕΑ();Δρ();ΓΡ=DateTime.Now;}}if(Ǜ) Εί();if(Ǜ) Εΰ();if(Ǜ) Εγ();if(Ǜ) Εβ();if(Ǜ&&Αξ==0) Εή();
Εέ();}void Εα(ΰ Γ,String Ξ){Γ.ΑΖ=DateTime.Now;String Ĝ="";Ђ(ref Ξ,out Γ.ſ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.ι,BROADCAST_SEP);if(Γ.ſ!=VERSION) return;String ī="";String[]ȧ=new string[3];Ђ(ref Ξ,out ī,BROADCAST_SEP);Ђ(ref Ξ,out ȧ[0],BROADCAST_SEP);Ђ(ref Ξ,out ȧ[1],BROADCAST_SEP);Ђ(ref Ξ,out ȧ[2],BROADCAST_SEP);
Ђ(ref Ξ,out Γ.Αλ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ψ,BROADCAST_SEP);Ђ(ref Ξ,out Ĝ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.ű,BROADCAST_SEP);String Γϊ="",ΓΪ="";Ђ(ref Ξ,out Γϊ,BROADCAST_SEP);Ђ(ref Ξ,out ΓΪ,BROADCAST_SEP);Γ.ї.Clear();while(true){String ΓΜ;if(!Ђ(ref Ξ,out ΓΜ,BROADCAST_SEP)) break;String[]Ū=ΓΜ.Split('/');
if(Ū.Count()<3) continue;int Ї=0;if(!int.TryParse(Ū[2],out Ї)) continue;Γ.ї.Add(new Ǩ(Ū[0],Ū[1],Ї,Ł.Δξ));}int Γά=0;if(!int.TryParse(Ĝ,out Γ.π)) Γ.π=0;if(!int.TryParse(ī,out Γά)) Γ.Π=Ο.ǔ;if(!float.TryParse(ȧ[0],out Γ.Ρ.X)) Γ.Ρ.X=0;if(!float.TryParse(ȧ[1],out Γ.Ρ.Y)) Γ.Ρ.Y=0;
if(!float.TryParse(ȧ[2],out Γ.Ρ.Z)) Γ.Ρ.Z=0;if(!float.TryParse(Γϊ,out Γ.Ǭ)) Γ.Ǭ=0;if(!float.TryParse(ΓΪ,out Γ.Ƿ)) Γ.Ƿ=0;Γ.Π=(Ο) Γά;Γ.ǁ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Ρ));Γ.Ασ=0;for(int i=0;i<Γ.ї.Count();i++) Γ.Ασ+=Γ.ї[i].Ї;}ΰ Ə(String Ν,String ι){Ν=Ν.ToUpper();ι=ι.ToUpper();
for(int i=0;i<σ.Count;i++){if(Ν!=""&&σ[i].Ν.ToUpper()==Ν) return σ[i];if(ι!=""&&σ[i].ι!=null&&σ[i].ι.ToUpper()==ι) return σ[i];}return null;}String ȓ="";ΰ Βω=null;DateTime Βϊ=DateTime.MinValue;List<ΰ>ѓ=new List<ΰ>();const float ΓЉ=20f;bool Βδ(ΰ ί,String κ){return κ==ȓ&&ί==Βω&&((DateTime.Now-Βϊ).TotalSeconds<ΓЉ||
(κ=="HOMEPOS"&&(т.Count>0||Ш!=null)));}String ΔΦ(ΰ ί){if(ȓ==""||!Βδ(ί,ȓ)) return "";String s="";int Ȥ=0,Αδ=0;for(int i=0;i<ѓ.Count;i++){η ź=ѓ[i].ǒ(ȓ,false,false,false);if(ź==η.Ć) Ȥ++;bool Ζά=т.Contains(ѓ[i]);if(Αδ++<8) s+="  "+ѓ[i].ι+": "+(Ζά?"queued ("+(т.IndexOf(ѓ[i])+1)+")":ź==η.Ć?"received":
ź==η.Ȍ?"pending...":ź==η.ń?"no answer!":"-")+"\n";}if(Αδ>8) s+="  +"+(Αδ-8)+" more\n";return "--------------------------------------------\n"+ȓ+": "+Ȥ+"/"+ѓ.Count+" received\n"+s;}void С(ΰ Γ,String κ){ȓ=κ;Βω=Γ;Βϊ=DateTime.Now;ѓ.Clear();if(Γ==null) ѓ.AddRange(σ);else ѓ.Add(Γ);
if(Γ==null){т.Clear();Ш=null;}else т.Remove(Γ);if(Γ==null&&κ=="HOMEPOS"){ΕΠ();return;}if(Γ==null){for(int i=0;i<σ.Count;i++)
σ[i].ǒ(κ,true,false,false);Ƨ(κ,"*",κ);}else{Γ.ǒ(κ,true,false,false);Ƨ(κ,Γ.Ν,κ);}}void ΔЉ(string Ў){if(Ў=="") return;var ζ=Ў.ToUpper().Split(' ');ζ.DefaultIfEmpty("");var κ=ζ.ElementAtOrDefault(0);var Βρ=ζ.ElementAtOrDefault(1);String ΕЏ="Invalid argument: "+Ў;if(γ==Κ.Ĉ){if(ρ!=null)
{switch(κ){case "UP":{if(π<2) break;if(ρ.π==0){π=1;return;}С(ρ,"UP");return;}case "DOWN":{if(π<1) break;if(π==1){С(ρ,"MRES");break;}С(ρ,"DOWN");break;}case "APPLY":{if(π<2) break;С(ρ,"APPLY");return;}}}}switch(κ){case "UP":this.Δω(false);return;case "DOWN":this.Δψ(false);return;
case "APPLY":this.Ȉ(true);return;}switch(κ){case "CLEAR":ΓЏ();return;case "SENDALL":
if(String.IsNullOrEmpty(Βρ)){Ψ="Usage: SENDALL <cmd>";return;}С(null,Βρ);return;case "SEND":
if(Ў.Length<="SEND".Length+1){Ψ="Usage: SEND <ship>:<cmd>";return;}ΕΓ(Ў.Remove(0,"SEND".Length+1));return;}Ψ=ΕЏ;}void ΕΓ(String Βπ){if(Βπ=="") return;var ζ=Βπ.Split(':');if(ζ.Length!=2){Ψ="Missing separator \":\"";return;}ζ.DefaultIfEmpty("");String Į=ζ.ElementAtOrDefault(0).Trim();
ΰ Γ=Ə("",Į);if(Γ!=null) С(Γ,ζ.ElementAtOrDefault(1).Trim());else Ψ="Unknown ship: "+Į;}void ΓЏ(){σ.Clear();ρ=null;Χ(Κ.Main);π=0;}void ΕΑ(){List<IMyTerminalBlock>Γς=new List<IMyTerminalBlock>();М.GetBlocksOfType(Ј,ƚ);М.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Γς,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ј=ΑЇ(Ј,pamTag,true);ΕΜ(Γς);ΕΛ();ΕΒ();ć(Ј,setLCDFontAndSize,1.15f,false);ć(щ,setLCDFontAndSize,1.15f,true);String ΕЋ=ΑЏ(false);String Γί="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ѝ in Ј){if(Ѝ.CustomData==""){Ѝ.CustomData=ΑЏ(true);continue;}
if(!Ѝ.CustomData.Contains(ΕЋ))
if(!Ѝ.CustomData.Contains(Γί)) Ѝ.CustomData=Γί+Ѝ.CustomData;}}void Εέ(){String Ę="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<щ.Count;i++)
щ[i].WriteText(ΑϏ(ω.ΒΟ,"0",false));for(int i=0;i<Ј.Count;i++){ω θ=ω.ǔ;String Ь="";ΔЅ(Ј[i],out θ,out Ь);Ј[i].WriteText(ΑϏ(θ,Ь,i==0));}Echo(Ę);for(int i=0;i<Ǯ.Count;i++){IMyTextPanel Ѝ=Ǯ[i];String Ξ=Ѝ.CustomData.ToUpper();if(Ξ=="DEBUG") Ѝ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ѝ.WriteText(ΑЎ());
}}String Ȉ(bool Ε){int Εό=0;return Ȉ(Ε,0,ref Εό,false,1);}String Ȉ(bool Ε,int ΖΤ,ref int Ζγ,bool Βύ,int Ĭ){String Β="";String Ȯ="——————————————————\n";String Ζπ="--------------------------------------------\n";if(ρ==null||γ==Κ.Main||γ==Κ.ƨ)
Β+="[PAM]-Controller | "+σ.Count+" Connected ships"+"\n"+ΔΪ()+Δΰ()+Δζ();else Β+=ΒΑ(ρ)+"\n";Β+=Ȯ;int Α=0;int Δ=π;Ǹ=0;if(Ψ!=""){Ζ(ref Β,Δ,Α++,Ε,Ψ);}else if(γ==Κ.Main){if(ρ==null&&σ.Count>=1) ρ=σ[0];String Ѕ="";if(Ζ(ref Ѕ,Δ,Α++,Ε," Send command to all")) Χ(Κ.ƨ);
int ȭ=0;for(int s=0;s<σ.Count;s++){if(Βύ) Ѕ+="\n";ΰ Γ=σ[s];if(Δ==Α||Δ==Α+1) ρ=Γ;if(Α==Δ) ȭ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+ΒΑ(Γ))){ρ=Γ;Χ(Κ.Ĉ);ρ.ǒ("",false,true,false);}if(Α==Δ) ȭ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+(Βύ?"":"  ")+Δβ(Γ)))
{ρ=Γ;Χ(Κ.Βγ);}}int Ā=Ĭ-2;Ā+=Math.Max(0,(ΖΤ-1))*Ĭ;Β+=Ʀ(Ā,Ѕ,ȭ,ref Ζγ);}else if(γ==Κ.ņ||γ==Κ.ƨ){Κ Ελ=γ==Κ.ņ?Κ.Ĉ:Κ.Main;ΰ ί=null;if(γ==Κ.ņ) ί=ρ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(Ελ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+Ȋ(ί,"STOP"))){С(ί,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+Ȋ(ί,"CONT"))){С(ί,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+Ȋ(ί,"HOMEPOS"))){С(ί,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+Ȋ(ί,"JOBPOS"))){С(ί,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+Ȋ(ί,"FULL"))){С(ί,"FULL");}if(ί!=null&&ί.Π==Ο.ν)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+Ȋ(ί,"UNDOCK"))){С(ί,"UNDOCK");}Β+=ΔΦ(ί);}else if(γ==Κ.Ĉ){String Ƹ="";if(Ζ(ref Ƹ,Δ,Α++,Ε," Back")){Χ(Κ.Main);}Β+=Ƹ.Substring(0,Ƹ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Χ(Κ.ņ);}if(!ρ.ƛ()&&!ρ.Ƙ()) Ǹ++;if(ρ.Ƙ()) Β+="No answer received...";
else Β+=Ȏ(ρ,ρ.ű);}else if(γ==Κ.Βγ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(Κ.Main);}Β+=Ζπ;Β+=Ȏ(ρ,ΑБ(ρ));}if(!ΑΡ) π=Math.Min(Ǹ-1,π);return Β;}String ΒΑ(ΰ Γ){if(Γ.ſ!=VERSION) return Γ.ι+": Different version!";return Γ.ι+": "+Ȏ(Γ,Γ.Αλ)+" "+Γ.ǁ+"m";}String Δβ(ΰ Γ){String s=Ƥ("",Р(Γ.Ǭ,Γ.Ƿ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.ї.Count;i++){if(i>=5) break;Ǩ у=Γ.ї[i];s+=Βν(ƈ(Δή(у)),3)+" "+ΑЉ(у.Ї)+" ";}return s;}String Δή(Ǩ у){if(у.Ћ.ToUpper()=="ORE"||у.Ћ.ToUpper()=="INGOT"){String Βϋ=Δά(у.ι.ToUpper());if(Βϋ!="") return Βϋ;}return у.ι;}String Ȏ(ΰ Γ,String Β){if(Γ.ƛ()) return "No signal...("+ƒ((int)(DateTime.Now-Γ.ΑΖ).TotalSeconds)+")";
return Β;}String ΑЏ(bool ΖЀ){String Β="";if(ΖЀ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΔЅ(IMyTerminalBlock Ѝ,out ω θ,out String Ь){bool Ȥ=true;String ī=Ņ(Ŀ(Ѝ.CustomData.Split('\n'),"mode",ref Ȥ)).ToUpper();String[]ζ=ī.Split(':');String Ī=ζ.Length>0?ζ[0].Trim():"";Ь=ζ.Length>1?ζ[1].Trim():"";
θ=ω.ǔ;if(Ī=="MAIN") θ=ω.ΒΠ;else if(Ī=="MAINX") θ=ω.ΒΡ;else if(Ī=="MENU") θ=ω.ƞ;else if(Ī=="INVENTORY") θ=ω.Ɨ;else if(Ī=="DEBUG") θ=ω.Ƌ;if(θ==ω.ǔ) Ь=ī;}String ΑϏ(ω θ,String Ь,bool ΕϏ){int Ĩ=15;if(ΕϏ){Βώ=ŗ;ŗ=0;ΒϏ=Ř;Ř=0;}String Β="";if(θ==ω.ΒΠ){int Ќ=0;
Β=Ȉ(false,Βώ,ref Εφ,true,Ĩ);if(!int.TryParse(Ь,out Ќ)) return Β;ŗ=Math.Max(Ќ,ŗ);Ќ--;return Βή(Ќ,Ĩ,Β);}else if(θ==ω.ΒΡ){int Ќ=0;Β=Ȉ(false,ΒϏ,ref Εχ,false,Ĩ);if(!int.TryParse(Ь,out Ќ)) return Β;Ř=Math.Max(Ќ,Ř);Ќ--;return Βή(Ќ,Ĩ,Β);}if(θ==ω.ΒΟ){return Ȉ(false,0,ref Εψ,true,Ĩ);
}else if(θ==ω.Ɨ||θ==ω.ƞ||θ==ω.Ƌ){ΰ Γ=ρ;if(Ь!=""){Γ=Ə("",Ь);if(Γ==null) return "Unknown ship: "+Ь;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ȯ="—————————————————————\n";String Αρ="";String ƹ="";if(θ==ω.ƞ){Αρ="Menu";ƹ=Ȏ(Γ,Γ.ű);}
else if(θ==ω.Ɨ){Αρ="Inventory";ƹ=Ȏ(Γ,ΑБ(Γ));}else if(θ==ω.Ƌ){return Ȏ(Γ,Δγ(Γ));}Β+=Γ.ι+" - "+Γ.ǁ+"m | "+ƈ(""+Αρ)+"\n"+Ȯ;Β+=ƹ;return Β;}return "Unknown command: "+Ь;}string Δγ(ΰ Γ){String s="";s+=Γ.ſ+"\n";s+=Γ.ι+"\n";s+=Γ.Ρ.X+"\n";s+=Γ.Ρ.Y+"\n";
s+=Γ.Ρ.Z+"\n";s+=Γ.Αλ+"\n";s+=Γ.Ψ+"\n";s+=Γ.π+"\n";s+=Γ.ű.Length+"\n";s+=Γ.Ǭ+"\n";s+=Γ.Ƿ+"\n";s+=Γ.ї.Count()+"\n";return s;}string ΑБ(ΰ Γ){String s="";s+=Ƥ("All",Р(Γ.Ǭ,Γ.Ƿ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.ї.Count();i++){Ǩ у=Γ.ї[i];
s+=Ƥ(у.ι,у.Ї,Γ.Ασ,30,8,10)+"\n";}return s;}η ΔΧ(ΰ Γ,bool ђ,string κ){η έ=η.None;if(Γ==null){for(int i=0;i<σ.Count;i++){η ώ=σ[i].ǒ(κ,false,ђ,false);if(ώ==η.Ć) έ=ώ;if(ώ==η.ń) return ώ;if(ώ==η.Ȍ) return ώ;}return έ;}else return Γ.ǒ(κ,false,ђ,false);}String Ȋ(ΰ Γ,String κ)
{if(!Βδ(Γ,κ)) return "";η ώ=ΔΧ(Γ,false,κ);if(ώ==η.Ć) return "received!";if(ώ==η.Ȍ) return "pending...";if(ώ==η.ń) return "no answer!";return "";}String Ʀ(int Ā,String Β,int ȭ,ref int њ){String[]Η=Β.Split('\n');if(ȭ>=њ+Ā-1) њ++;њ=Math.Min(Η.Count()-1-Ā,њ);if(ȭ<њ+1) њ--;
њ=Math.Max(0,њ);String έ="";for(int i=0;i<Ā;i++){int ΓΥ=i+њ;if(ΓΥ>=Η.Count()) break;έ+=Η[ΓΥ]+"\n";}return έ;}String Βή(int Ќ,int Ĭ,String Β){String[]Η=Β.Split('\n');int Ακ=Ќ*Ĭ;int ţ=(Ќ+1)*Ĭ;String έ="";for(int i=Ακ;i<ţ;i++){if(i>=Η.Count()) break;έ+=Η[i]+"\n";
}return έ;}String ƈ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Βν(String s,int ΓΩ){if(s=="") return s;if(s.Length>ΓΩ) s=s.Substring(0,ΓΩ-1)+".";return s;}string Ƥ(String ǵ,float value,float ŭ,int Βς,int ΓΠ,int ΖΣ){float ǻ=Р(value,ŭ)*Βς;
String œ="[";for(int i=0;i<Βς;i++){if(i<=ǻ) œ+="|";else œ+="'";}œ+="]";return œ+" "+Βν(ƈ(ǵ),ΓΠ).PadRight(ΓΠ)+"".PadRight(ΖΣ)+ΑЉ(value);}String ΑЉ(float Ї){if(Ї>=1000000) return Math.Round(Ї/1000000f,Ї/1000000f<100?1:0)+"M";if(Ї>=1000) return Math.Round(Ї/1000f,Ї/1000f<100?1:0)+"K";
return ""+Math.Round(Ї);}String ƒ(int ĭ){if(ĭ>=60*60) return Math.Round(ĭ/(60f*60f),1)+" h";if(ĭ>=60) return Math.Round(ĭ/60f,1)+" min";return ""+ĭ+" s";}List<IMyCameraBlock>Επ=new List<IMyCameraBlock>();bool ΖΥ=false;int Ες=0,ΖЌ=0,ΖЊ=0;String Εξ="";String Εο="";
void ΓЎ(){ΖΥ=false;Ες=0;Εξ="";}const String ΕΪ="[PAMTFC]";long ΖΜ=0;String ж="";int ǹ=-1;List<int>ΑΑ=new List<int>();String ΓΔ="";void ΔΜ(){String Ж=й+"|"+и+"|"+(int) ț;if(Ж==ΓΔ) return;ΓΔ=Ж;ΑЀ(й,и,ț,ΑΑ);}void ΑЀ(int w,int h,Т Ακ,List<int>Ƽ){Ƽ.Clear();if(Ακ==Т.ō)
{for(int k=0;k<w*h;k++){int Ǣ=k/w;Ƽ.Add(Ǣ*w+(Ǣ%2==0?k-Ǣ*w:w-1-(k-Ǣ*w)));}return;}int ΓΒ=(int) Math.Ceiling(w/2f),ΓΑ=(int) Math.Ceiling(h/2f);int ΕЉ=w/2,ΕЈ=h/2;int x=0,y=0,ǂ=0,ǃ=-1,ŭ=Math.Max(w,h);for(int i=0;i<ŭ*ŭ;i++){if(-ΓΒ<x&&x<=ΕЉ&&-ΓΑ<y&&y<=ΕЈ) Ƽ.Add((y-1+ΓΑ)*w+(x-1+ΓΒ));
if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ǂ;ǂ=-ǃ;ǃ=t;}x+=ǂ;y+=ǃ;}}bool ΔΛ(){int n=Math.Max(0,й*и);if(ж.Length==n) return false;ж=new String('0',n);ǹ=-1;return true;}void ΕΙ(int Д,bool τ){if(Д<0||Д>=ж.Length) return;char[]h=ж.ToCharArray();
h[Д]=τ?'1':'0';ж=new String(h);}const String Δφ="[PAMJOB]";const String ΕΨ="[PAMTCC]";class ȍ{public long Ν;public Vector3 Ω,Л,Ё;}List<ȍ>Б=new List<ȍ>();bool Ė=false;Vector3 Š,ş,š,Ţ;long ģ=0;long Γβ=0;ά Γα=null;bool Εϊ=false;ά ΔΘ(Vector3 Ω,Vector3 Л,Vector3 Ё){MatrixD m=MatrixD.CreateWorld(Ω,Л,Ё);
ά w=new ά(Σ);w.Є=Vector3D.TransformNormal(Š,m);w.Ϗ=Vector3D.TransformNormal(ş,m);w.Н=Vector3D.TransformNormal(š,m);w.Ρ=Ω+(Vector3) Vector3D.TransformNormal(Ţ,m);w.λ=true;return w;}void Δχ(){if(Π==Ο.ν||!Σ.λ) return;IMyShipConnector є=ȉ(MyShipConnectorStatus.Connected);if(є==null||є.OtherConnector==null) return;
IMyShipConnector З=є.OtherConnector;Vector3I ΕЌ=new Vector3I((int) Σ.Ц.X,(int) Σ.Ц.Y,(int) Σ.Ц.Z);if(є.Position==ΕЌ){MatrixD ũ=MatrixD.Transpose(З.WorldMatrix);Š=Vector3D.TransformNormal(Љ.WorldMatrix.Forward,ũ);ş=Vector3D.TransformNormal(Љ.WorldMatrix.Down,ũ);š=Vector3D.TransformNormal(Љ.WorldMatrix.Left,ũ);
Ţ=Vector3D.TransformNormal(є.GetPosition()-З.GetPosition(),ũ);Ė=true;}if(Vector3.Distance(є.GetPosition(),Σ.Ρ)<5) ģ=З.EntityId;List<IMyShipConnector>ǲ=new List<IMyShipConnector>();М.GetBlocksOfType(ǲ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ǲ.Count>0){Б.Clear();for(int i=0;i<ǲ.Count;i++){ȍ p=new ȍ();p.Ν=ǲ[i].EntityId;p.Ω=ǲ[i].GetPosition();p.Л=ǲ[i].WorldMatrix.Forward;p.Ё=ǲ[i].WorldMatrix.Up;Б.Add(p);}}Γα=null;Γβ=0;if(З.EntityId==ģ||!Ė) return;for(int i=0;i<Б.Count;i++)
if(Б[i].Ν==З.EntityId){Γβ=Б[i].Ν;Γα=ΔΘ(Б[i].Ω,Б[i].Л,Б[i].Ё);}}String ΔΗ(){String s=(Ė?"1":"0")+":"+β(Š)+":"+β(ş)+":"+
β(š)+":"+β(Ţ)+":"+ģ;for(int i=0;i<Б.Count;i++)
s+=":"+Б[i].Ν+","+β(Б[i].Ω)+","+β(Б[i].Л)+","+β(Б[i].Ё);return s;}void ΔΖ(String s){Б.Clear();Ė=false;ģ=0;String[]f=s.Split(':');if(f.Length<6) return;Ė=f[0]=="1";Š=Ϋ(f[1]);ş=Ϋ(f[2]);š=Ϋ(f[3]);Ţ=Ϋ(f[4]);long.TryParse(f[5],out ģ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ȍ p=new ȍ();if(!long.TryParse(e[0],out p.Ν)) continue;p.Ω=Ϋ(e[1]);p.Л=Ϋ(e[2]);p.Ё=Ϋ(e[3]);Б.Add(p);}}const String Ɗ="[PAMDCK]";const float ΓЈ=120f;class э{public long Γ;public int ǜ;public long з;public bool Βχ;public DateTime К,Ȝ;
}IMyBroadcastListener Ş=null;List<э>ϊ=new List<э>();List<IMyShipConnector>ћ=new List<IMyShipConnector>();List<IMySensorBlock>а=new List<IMySensorBlock>();List<IMyCameraBlock>ј=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ƃ=new List<MyDetectedEntityInfo>();long Џ=0;int ƽ=0;
long ŕ=0;DateTime Βψ=DateTime.Now;String ǯ="";int ΒЉ=0;IMyCubeGrid ќ=null;void ΕΒ(){ќ=Me.CubeGrid;М.GetBlocksOfType(ћ,q=>q.CubeGrid.IsSameConstructAs(ќ)&&q.CustomName.Contains(dockPoolTag));М.GetBlocksOfType(а,q=>q.CubeGrid.IsSameConstructAs(ќ)&&q.CustomName.Contains(zoneTag));М.GetBlocksOfType(ј,q=>q.CubeGrid.IsSameConstructAs(ќ)&&q.CustomName.Contains(laneTag));
for(int i=0;i<а.Count;i++){а[i].DetectSmallShips=true;а[i].DetectLargeShips=true;}for(int i=0;i<ј.Count;i++) ј[i].EnableRaycast=true;}IMyShipConnector ƭ(long Ν){if(Ν==0) return null;IMyShipConnector c=М.GetBlockWithId(Ν) as IMyShipConnector;return c!=null&&ќ!=null&&c.CubeGrid.IsSameConstructAs(ќ)?c:null;
}bool ƙ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΒΞ(IMyShipConnector c,long Γ){for(int i=0;i<ј.Count;i++){IMyCameraBlock ē=ј[i];if(!ē.IsWorking||Vector3D.Distance(ē.GetPosition(),c.GetPosition())>10) continue;
Vector3D ţ=ē.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ē.CanScan(ţ)) continue;MyDetectedEntityInfo Ť=ē.Raycast(ţ);if(!Ť.IsEmpty()&&Ť.EntityId!=ќ.EntityId&&Ť.EntityId!=Γ&&Ť.Type!=MyDetectedEntityType.FloatingObject){ǯ="Lane blocked: "+c.CustomName;return false;}
}return true;}bool Δτ(){for(int s=0;s<а.Count;s++){if(!а[s].IsWorking) continue;ƃ.Clear();а[s].DetectedEntities(ƃ);for(int i=0;i<ƃ.Count;i++){MyDetectedEntityInfo e=ƃ[i];bool ΕЅ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;if(ΕЅ&&e.EntityId!=Џ&&e.Velocity.Length()>0.5f)
{ǯ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΔІ(э r){IMyShipConnector з=ƭ(r.з);if(ƙ(з)&&ΒΞ(з,r.Γ)) return з.EntityId;if(!r.Βχ||з==null) return 0;IMyShipConnector ѐ=null;double Βυ=0;for(int i=0;i<ћ.Count;i++){IMyShipConnector c=ћ[i];if(!ƙ(c)) continue;
double d=Vector3D.Distance(c.GetPosition(),з.GetPosition());if(ѐ==null||d<Βυ){if(ΒΞ(c,r.Γ)){ѐ=c;Βυ=d;}}}return ѐ==null?0:ѐ.EntityId;}void Δη(){э ȥ=null;long є=0;for(int ŵ=2;ŵ>=1&&ȥ==null;ŵ--){э Ǻ=null;for(int i=0;i<ϊ.Count;i++){э r=ϊ[i];if(r.ǜ!=ŵ) continue;
if(ŵ==1){long c=ΔІ(r);if(c==0) continue;if(Ǻ==null||r.К<Ǻ.К){Ǻ=r;є=c;}}else if(Ǻ==null||r.К<Ǻ.К) Ǻ=r;}ȥ=Ǻ;}if(ȥ==null){if(ϊ.Count>0&&ǯ=="") ǯ="No free dock";return;}Џ=ȥ.Γ;ƽ=ȥ.ǜ;ŕ=ȥ.ǜ==1?є:0;Βψ=DateTime.Now;}void Εί(){if(ќ==null) return;
if(Ş==null) Ş=IGC.RegisterBroadcastListener(Ɗ);int Ē=30;while(Ş.HasPendingMessage&&Ē-->0){String Ξ=Ş.AcceptMessage().Data as String;if(Ξ!=null) Δι(Ξ);}ΔА();if(++ΒЉ<3) return;ΒЉ=0;IGC.SendBroadcastMessage(Ɗ,ΓІ());}void Δι(String Ξ){try{String[]f=Ξ.Split(';');long Γ=long.Parse(f[1]);
if(f[0]=="D"){for(int i=ϊ.Count-1;i>=0;i--) if(ϊ[i].Γ==Γ) ϊ.RemoveAt(i);if(Џ==Γ) Џ=0;}else if(f[0]=="R"&&f.Length>=5){long з=long.Parse(f[3]);if(ƭ(з)==null) return;э r=null;for(int i=0;i<ϊ.Count;i++) if(ϊ[i].Γ==Γ) r=ϊ[i];if(r==null){r=new э();r.Γ=Γ;r.К=DateTime.Now;ϊ.Add(r);}
int ǜ=int.Parse(f[2]);if(r.ǜ!=ǜ) r.К=DateTime.Now;r.ǜ=ǜ;r.з=з;r.Βχ=f[4]=="1";r.Ȝ=DateTime.Now;}}catch{}}void ΔА(){for(int i=ϊ.Count-1;i>=0;i--)
if((DateTime.Now-ϊ[i].Ȝ).TotalSeconds>3) ϊ.RemoveAt(i);э Ŕ=null;for(int i=0;i<ϊ.Count;i++) if(ϊ[i].Γ==Џ) Ŕ=ϊ[i];if(Џ!=0&&Ŕ==null) Џ=0;if(Ŕ!=null&&(DateTime.Now-Βψ).TotalSeconds>ΓЈ){Џ=0;Ŕ.К=DateTime.Now;}ǯ="";if(Џ==0&&ϊ.Count>0&&!Δτ()) Δη();}String ΓІ()
{String ε="S;"+ќ.EntityId+";"+Џ+";"+ƽ+";"+ŕ+";";for(int i=0;i<ϊ.Count;i++) ε+=(i>0?",":"")+ϊ[i].Γ;ε+=";";for(int i=0;i<ћ.Count;i++){IMyShipConnector c=ћ[i];ε+=(i>0?":":"")+c.EntityId+","+β(c.GetPosition())+","+β(c.WorldMatrix.Forward)+","+β(c.WorldMatrix.Up);
}ε+=";";for(int i=0;i<а.Count;i++){IMySensorBlock z=а[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ε+=(i>0?":":"")+β(z.GetPosition())+","+Math.Round(r,1);}return ε;}String ΔΪ()
{if(ћ.Count==0&&а.Count==0) return "";int ΒЍ=0;for(int i=0;i<ћ.Count;i++) if(ƙ(ћ[i])) ΒЍ++;return "Docks: "+ΒЍ+"/"+ћ.Count+" free"+(Џ!=0?(ƽ==1?" | docking":" | undocking"):"")+
(ϊ.Count>1?" | "+(ϊ.Count-1)+" waiting":"")+(ǯ!=""?"\n"+ǯ:"")+"\n";}int ΒЈ=0;bool Ζϊ=false;void ΔЊ(){if(ΒЈ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ɗ,"D;"+ΖΜ);ΒЈ=0;Ζϊ=false;}const float Δυ=600f;class о{public long Ν;public int Д=-1;public bool Γϋ;public float Ǉ,ǆ;
public DateTime Αα;}class в{public String Ж;public int w,h;public float Ατ,Αο;public String τ;public List<int>ΑΥ=new List<int>();public List<о>Њ=new List<о>();}List<в>Е=new List<в>();IMyBroadcastListener Ŭ=null;int ΓΟ=0;void Εΰ(){if(Ŭ==null) Ŭ=IGC.RegisterBroadcastListener(ΕΪ);int Ē=50;
while(Ŭ.HasPendingMessage&&Ē-->0){String Ξ=Ŭ.AcceptMessage().Data as String;if(Ξ==null) continue;try{Δλ(Ξ);}catch{}try{Δμ(Ξ);}catch{}}for(int j=Е.Count-1;j>=0;j--){в Θ=Е[j];for(int i=Θ.Њ.Count-1;i>=0;i--)
if((DateTime.Now-Θ.Њ[i].Αα).TotalSeconds>Δυ) Θ.Њ.RemoveAt(i);if(Θ.Њ.Count==0){Е.RemoveAt(j);continue;}ΓЁ(Θ);}if(++ΓΟ<3) return;ΓΟ=0;for(int j=0;j<Е.Count;j++) IGC.SendBroadcastMessage(Δφ,ΓЃ(Е[j]));}void ΓЁ(в Θ){for(int i=0;i<Θ.Њ.Count;i++){о s=Θ.Њ[i];if(s.Γϋ&&(s.Д<0||Θ.τ[s.Д]=='1')) s.Д=Δϊ(Θ,s);
}}String ΓЃ(в Θ){String ε="J;"+Θ.Ж+";"+Θ.τ+";";bool К=true;for(int i=0;i<Θ.Њ.Count;i++){if((DateTime.Now-Θ.Њ[i].Αα).TotalSeconds>5) continue;ε+=(К?"":",")+Θ.Њ[i].Ν+":"+Θ.Њ[i].Д;К=false;}return ε;}void Δλ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;в Θ=null;for(int i=0;i<Е.Count;i++) if(Е[i].Ж==f[4]) Θ=Е[i];if(Θ==null){Θ=new в();Θ.Ж=f[4];Θ.w=int.Parse(k[3]);Θ.h=int.Parse(k[4]);Θ.Ατ=float.Parse(k[6]);Θ.Αο=float.Parse(k[7]);if(Θ.w<1||Θ.h<1) return;ΑЀ(Θ.w,Θ.h,(Т) int.Parse(k[5]),Θ.ΑΥ);
Θ.τ=new String('0',Θ.w*Θ.h);Е.Add(Θ);}String d=f[6];if(d.Length==Θ.τ.Length){char[]c=Θ.τ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Θ.τ=new String(c);}long Ν=long.Parse(f[0]);о s=null;for(int i=0;i<Θ.Њ.Count;i++) if(Θ.Њ[i].Ν==Ν) s=Θ.Њ[i];
if(s==null){s=new о();s.Ν=Ν;Θ.Њ.Add(s);}s.Αα=DateTime.Now;String[]w=f[10].Split(',');s.Γϋ=w[0]=="1";if(w.Length>=3){s.Ǉ=float.Parse(w[1]);s.ǆ=float.Parse(w[2]);}int Ģ=int.Parse(f[5]);if(Ģ>=0&&Ģ<Θ.τ.Length&&(s.Д<0||Θ.τ[s.Д]=='1')&&Θ.τ[Ģ]!='1'&&ƕ(Θ,Ģ,s)==null)
s.Д=Ģ;}о ƕ(в Θ,int Д,о Εώ){for(int i=0;i<Θ.Њ.Count;i++){о o=Θ.Њ[i];if(o!=Εώ&&o.Д==Д)
return o;}return null;}int Δϊ(в Θ,о s){for(int k=0;k<Θ.ΑΥ.Count;k++){int c=Θ.ΑΥ[k];if(Θ.τ[c]!='1'&&ƕ(Θ,c,s)==null) return c;}return-2;}String Δΰ(){String s="";for(int j=0;j<Е.Count;j++){в Θ=Е[j];int τ=0;for(int i=0;i<Θ.τ.Length;i++) if(Θ.τ[i]=='1') τ++;s+="Job "+(j+1)+": "+τ+"/"+Θ.τ.Length+" holes, "+Θ.Њ.Count+" ships\n";
}return s;}const float ΕΧ=8f;class І{public long Ν;public Vector3 Ω,ы,Ġ;public float ȕ,Ǉ,ǆ,Ǟ=-1;public int Ǵ;public String Θ="";public int ΑΨ;public DateTime ǿ;}class ŋ{public long a,b,Γό;public DateTime Ȝ;}List<І>Ϊ=new List<І>();List<ŋ>И=new List<ŋ>();int Αξ=0;void Δμ(String Ξ){
String[]f=Ξ.Split(';');if(f.Length<9) return;long Ν=long.Parse(f[0]);І s=null;for(int i=0;i<Ϊ.Count;i++) if(Ϊ[i].Ν==Ν) s=Ϊ[i];if(s==null){s=new І();s.Ν=Ν;Ϊ.Add(s);}s.Ω=Ϋ(f[1]);s.ы=Ϋ(f[2]);s.ȕ=float.Parse(f[3]);s.Θ=f[4];s.ΑΨ=int.Parse(f[7].Split(',')[1]);s.Ġ=Ϋ(f[8]);
s.Ǵ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.Ǉ=float.Parse(w[1]);s.ǆ=float.Parse(w[2]);s.Ǵ=int.Parse(w[3]);}}s.ǿ=DateTime.Now;}bool Ō(І s){return s.ы.LengthSquared()<0.25f&&(s.ΑΨ==3||s.Ġ.LengthSquared()<0.25f);}Vector3 Βκ(І s){return s.ы.LengthSquared()>=1f?s.ы:s.Ġ*2f;}
void Εγ(){ΓА();if(++Αξ<3) return;Αξ=0;IGC.SendBroadcastMessage(ΕΨ,ΓЇ());}void ΓА(){for(int i=Ϊ.Count-1;i>=0;i--)
if((DateTime.Now-Ϊ[i].ǿ).TotalSeconds>3) Ϊ.RemoveAt(i);for(int i=И.Count-1;i>=0;i--)
if((DateTime.Now-И[i].Ȝ).TotalSeconds>2) И.RemoveAt(i);for(int i=0;i<Ϊ.Count;i++) Ϊ[i].Ǟ=-1;for(int i=0;i<Ϊ.Count;i++)
for(int j=i+1;j<Ϊ.Count;j++){І a=Ϊ[i],b=Ϊ[j];if(Ō(a)||Ō(b)) continue;Vector3 ΖΡ=a.Ω+a.ы*(float)(DateTime.Now-a.ǿ).TotalSeconds;Vector3 ΖΧ=b.Ω+b.ы*(float)(DateTime.Now-b.ǿ).TotalSeconds;Vector3 Ȳ=Βκ(a),ȳ=Βκ(b);Vector3 Ȩ=ΖΧ-ΖΡ;float р=Ȩ.Length();float ђ=(a.ȕ+b.ȕ)*0.55f;
bool Γλ=a.Θ!=""&&a.Θ==b.Θ;if(Γλ&&(a.Ǵ==2||b.Ǵ==2)) continue;if(Γλ&&a.Ǵ==1&&b.Ǵ==1){String[]k=a.Θ.Split('|');if(k.Length>=8) ђ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Αή=ȳ-Ȳ;float Γκ=Αή.LengthSquared();float t=Γκ>0.01f?-Vector3.Dot(Ȩ,Αή)/Γκ:0;
bool Ας=р<ђ;if(!Ας&&(t<=0||t>ΕΧ||(Ȩ+Αή*t).Length()>=ђ)) continue;if(!Ας&&Ȳ.LengthSquared()>0.25f&&ȳ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ȳ),Vector3.Normalize(ȳ))<-0.7f)
continue;ŋ p=null;for(int k=0;k<И.Count;k++)
if((И[k].a==a.Ν&&И[k].b==b.Ν)||(И[k].a==b.Ν&&И[k].b==a.Ν)) p=И[k];if(p==null){p=new ŋ();p.a=a.Ν;p.b=b.Ν;p.Γό=ΕΩ(a,b,Ȳ,ȳ,Ȩ,р);И.Add(p);}p.Ȝ=DateTime.Now;І y=p.Γό==a.Ν?a:b;float ΓΤ=Ας||t<4?0:Math.Max(1,y.ы.Length()*0.5f);if(y.Ǟ<0||ΓΤ<y.Ǟ) y.Ǟ=ΓΤ;
}}long ΕΩ(І a,І b,Vector3 Ȳ,Vector3 ȳ,Vector3 Ȩ,float р){if(a.Ν==Џ) return b.Ν;if(b.Ν==Џ) return a.Ν;Vector3 n=р>0.01f?Ȩ/р:new Vector3(1,0,0);float Γρ=Vector3.Dot(Ȳ,n),Γτ=Vector3.Dot(ȳ,-n);if(Γρ-Γτ>1) return a.Ν;if(Γτ-Γρ>1) return b.Ν;return a.Ν>b.Ν?a.Ν:b.Ν;
}String ΓЇ(){String ε="C;";bool К=true;for(int i=0;i<Ϊ.Count;i++){if(Ϊ[i].Ǟ<0) continue;ε+=(К?"":",")+Ϊ[i].Ν+":"+Math.Round(Ϊ[i].Ǟ,1);К=false;}return ε;}String Δζ(){int Γέ=0,ǌ=0;for(int i=0;i<Ϊ.Count;i++){if(!Ō(Ϊ[i])) Γέ++;if(Ϊ[i].Ǟ>=0) ǌ++;}if(Ϊ.Count==0) return "";
return "Traffic: "+Γέ+" moving"+(ǌ>0?", "+ǌ+" giving way":"")+"\n";}List<ΰ>т=new List<ΰ>();ΰ Ш=null;Vector3 ΓΙ;DateTime ΓΘ;float ΓΗ=40;bool ΓΕ=false;І ΑЈ(ΰ s){І ѐ=null;for(int i=0;i<Ϊ.Count;i++)
if(Vector3.Distance(Ϊ[i].Ω,s.Ρ)<15&&(ѐ==null||
Vector3.Distance(Ϊ[i].Ω,s.Ρ)<Vector3.Distance(ѐ.Ω,s.Ρ))) ѐ=Ϊ[i];return ѐ;}float Δν(ΰ s){І t=ΑЈ(s);if(t==null) return 0;float k=t.Ǵ*1000000f;String[]Ж=t.Θ.Split('|');if(Ж.Length>=3) k+=Vector3.Distance(t.Ω,new Vector3(float.Parse(Ж[0]),float.Parse(Ж[1]),float.Parse(Ж[2])));return k;
}void ΕΠ(){т.Clear();Ш=null;List<float>ΑΔ=new List<float>();for(int i=0;i<σ.Count;i++){float k=Δν(σ[i]);int č=0;while(č<ΑΔ.Count&&ΑΔ[č]<=k) č++;ΑΔ.Insert(č,k);т.Insert(č,σ[i]);}}void Εβ(){if(Ш!=null){bool ΕЄ=ΓΕ||Vector3.Distance(Ш.Ρ,ΓΙ)>ΓΗ||
(DateTime.Now-ΓΘ).TotalSeconds>45||Ш.ƛ();if(!ΕЄ) return;Ш=null;}if(т.Count==0) return;Ш=т[0];т.RemoveAt(0);ΓΙ=Ш.Ρ;ΓΘ=DateTime.Now;І t=ΑЈ(Ш);ΓΗ=t==null?40:Math.Max(40,t.ȕ*3);ΓΕ=t!=null&&t.ΑΨ==3&&t.ы.LengthSquared()<0.25f;Ш.ǒ("HOMEPOS",true,false,false);Ƨ("HOMEPOS",Ш.Ν,"HOMEPOS");
}const String ΔΔ="[PAMDBG]";class ƌ{public readonly bool ΒΦ;IMyProgrammableBlock Ǖ;Action<IMyProgrammableBlock>Ƴ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Ʋ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Ʊ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>ƴ;
public ƌ(MyGridProgram ΖΫ){Ǖ=ΖΫ.Me;var Ƞ=Ǖ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(Ǖ);if(Ƞ==null) return;Ƴ=(Action<IMyProgrammableBlock>) Ƞ["RemoveDraw"];Ʋ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) Ƞ["Point"];Ʊ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) Ƞ["Line"];
ƴ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) Ƞ["Sphere"];((Action<IMyProgrammableBlock>) Ƞ["RemoveAll"])(Ǖ);ΒΦ=true;}public void ΔЋ(){if(Ƴ!=null) Ƴ(Ǖ);}public void Ǒ(Vector3D a,Vector3D b,Color c,float Ζο){if(Ʊ!=null) Ʊ(Ǖ,a,b,c,Ζο,-1,false);}
public void ΔЇ(Vector3D a,Color c,float ш){if(Ʋ!=null) Ʋ(Ǖ,a,c,ш,-1,false);}public void Βη(Vector3D Ȓ,double ш,Color c){if(ƴ!=null) ƴ(Ǖ,new BoundingSphereD(Ȓ,ш),c,1,0.05f,30,-1,false);}}class Ă{public long Ν;public Vector3 ΓΞ,ΑΘ,ė,ί;public bool ΓΓ;public List<Vector3>ŷ=new List<Vector3>(),ΑΪ=new List<Vector3>();
public List<bool>ΑΩ=new List<bool>();public DateTime ǿ;}ƌ А=null;IMyBroadcastListener Ŝ=null;List<Ă>ѕ=new List<Ă>();void Δθ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<6) return;long Ν=long.Parse(f[0]);Ă s=null;for(int i=0;i<ѕ.Count;i++) if(ѕ[i].Ν==Ν) s=ѕ[i];if(s==null){s=new Ă();s.Ν=Ν;ѕ.Add(s);}
s.ΓΞ=Ϋ(f[1]);s.ΑΘ=Ϋ(f[2]);s.ė=Ϋ(f[3]);s.ΓΓ=s.ΑΘ.LengthSquared()>0.5f;s.ί=Ϋ(f[4]);s.ǿ=DateTime.Now;if(f[5]=="") return;s.ŷ.Clear();s.ΑΪ.Clear();s.ΑΩ.Clear();String[]Γζ=f[5].Split(':');for(int i=0;i<Γζ.Length;i++){String[]r=Γζ[i].Split(',');if(r.Length<3) continue;s.ŷ.Add(Ϋ(r[0]));s.ΑΪ.Add(Ϋ(r[1]));s.ΑΩ.Add(r[2]=="1");
}}void Εή(){if(!debugDraw) return;if(А==null) А=new ƌ(this);if(!А.ΒΦ) return;if(Ŝ==null) Ŝ=IGC.RegisterBroadcastListener(ΔΔ);int Ē=20;while(Ŝ.HasPendingMessage&&Ē-->0){String Ξ=Ŝ.AcceptMessage().Data as String;if(Ξ!=null) try{Δθ(Ξ);}catch{}}for(int i=ѕ.Count-1;i>=0;i--) if((DateTime.Now-ѕ[i].ǿ).TotalSeconds>5) ѕ.RemoveAt(i);
А.ΔЋ();for(int i=0;i<Ϊ.Count;i++){І s=Ϊ[i];Color c=Ō(s)?Color.Gray:s.Ǟ>=0?Color.Yellow:Color.Lime;А.Βη(s.Ω,s.ȕ/2,c);if(s.ы.LengthSquared()>0.25f) А.Ǒ(s.Ω,s.Ω+s.ы*3,Color.White,0.1f);if(s.Ġ.LengthSquared()>0.25f) А.Ǒ(s.Ω,s.Ω+s.Ġ*s.ȕ,Color.Cyan,0.1f);}for(int i=0;i<И.Count;i++)
{if((DateTime.Now-И[i].Ȝ).TotalSeconds>0.5) continue;І a=null,b=null;for(int k=0;k<Ϊ.Count;k++){if(Ϊ[k].Ν==И[i].a) a=Ϊ[k];if(Ϊ[k].Ν==И[i].b) b=Ϊ[k];}if(a!=null&&b!=null) А.Ǒ(a.Ω,b.Ω,Color.Red,0.2f);}for(int i=0;i<ѕ.Count;i++){Ă d=ѕ[i];І t=null;for(int k=0;k<Ϊ.Count;k++) if(Ϊ[k].Ν==d.Ν) t=Ϊ[k];
if(t!=null) А.Ǒ(t.Ω,d.ί,Color.Blue,0.08f);for(int r=0;r<d.ŷ.Count;r++) А.Ǒ(d.ŷ[r],d.ΑΪ[r],d.ΑΩ[r]?Color.Red:Color.Green,0.04f);if(t==null||!d.ΓΓ) continue;в Θ=null;for(int j=0;j<Е.Count;j++) if(Е[j].Ж==t.Θ) Θ=Е[j];if(Θ==null) continue;for(int c=0;c<Θ.τ.Length;c++)
{int ƾ=c%Θ.w,Ǣ=c/Θ.w;Vector3 p=d.ΓΞ+d.ΑΘ*((Θ.w-1f)/2f-ƾ)*Θ.Ατ-d.ė*((Θ.h-1f)/2f-Ǣ)*Θ.Αο;о ΑΦ=ƕ(Θ,c,null);А.ΔЇ(p,Θ.τ[c]=='1'?Color.Gray:ΑΦ!=null?Color.Orange:Color.White,Math.Min(Θ.Ατ,Θ.Αο)*0.2f);if(ΑΦ!=null&&ΑΦ.Ν==d.Ν) А.Ǒ(t.Ω,p,Color.Orange,0.05f);
}}for(int i=0;i<а.Count;i++){IMySensorBlock z=а[i];А.Βη(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);}for(int i=0;i<ј.Count;i++)
А.Ǒ(ј[i].GetPosition(),ј[i].GetPosition()+ј[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(Џ!=0&&ŕ!=0){IMyShipConnector c=ƭ(ŕ);for(int k=0;k<Ϊ.Count;k++)
if(c!=null&&Ϊ[k].Ν==Џ) А.Ǒ(Ϊ[k].Ω,c.GetPosition(),Color.Lime,0.15f);}}void ΕΝ(string Ў){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}