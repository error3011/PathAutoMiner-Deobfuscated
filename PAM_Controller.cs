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

//Speed limit near the job area (moving between holes, backing out, arriving and leaving), so ships
//working next to each other don't change direction fast or bump into each other.
//[m/s]

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ο{Ǘ,Ƨ,Ȏ,Е,ο}public enum Μ{ǩ,Ƙ,Βέ,ƭ,Βθ,Ć,Ǔ,
Ɨ,ΔΪ,Ȋ,ΑЍ,ΓЄ,ΑІ,
ΕΕ,Ď,č,Ə,Εξ,ΔΥ,
Εμ,Εν,ΔΦ,ΔΧ,ď}public enum Τ{ё,ƪ,Ǩ,ΑЎ,Ł,Βή,Βί,Ľ,ľ}public enum Ǐ{None,Ə,ǩ,ΔΛ}public enum Κ{Main,Βζ,Ĉ,ć,ƶ,ŀ,
Ƌ,ΒΠ,ΒΡ,ΒΟ,ċ,Βκ,
Ŋ,ư,Ƴ,Βν,Ŏ}public enum ǖ{Βι,ΔΖ,ΕΧ,Δϊ,ο}public enum У{ő,ΔΑ}public enum ǒ{Βη,ΔΩ,ΕΪ,Ɲ}public enum в{Βα,ΔΙ,ΔΘ,ΔΠ,ΔΟ,Δψ,Δχ}public enum я{Ɠ,ΓЈ,ΑЇ}public enum Ņ{Δφ,ΖЏ,ΓЅ}public enum Ѕ{Ββ,Βγ,Βδ,ΔЋ,ΔЊ,
ΔЅ,ΔІ,ΔЄ,ΔЈ,
ΔЉ,ΔЇ}public enum Υ{о,ђ,н,Ŀ,ƌ,ƍ}public enum п{Βπ,ΑЏ,ΔЃ,Βΰ}public enum ι{None,Ȑ,ĉ,ň}public enum τ{Ƹ,Ɗ,Ƥ,Ɲ,ΒΫ,Ʀ,Βά,Εγ,Ʃ}public enum ϊ{Ǘ,ΒΩ,ΒΨ,ƥ,ƞ,Ƒ,ΒΪ}class ά{public bool ν=false;public Vector3 Π=new Vector3();public Vector3 Ё=new Vector3();
public Vector3 І=new Vector3();public Vector3 О=new Vector3();public Vector3 π=new Vector3();public Vector3 Ч=new Vector3();public float ΖΡ=0;public float ΗΕ=0;public float[]Я=null;public ά(){}public ά(ά Й){ν=Й.ν;Π=Й.Π;Ё=Й.Ё;І=Й.І;О=Й.О;π=Й.π;Ч=Й.Ч;Я=Й.Я;}public ά(Vector3 Π,Vector3 І,Vector3 Ё,Vector3 О,Vector3 π)
{this.Π=Π;this.Ё=Ё;this.І=І;this.О=О;this.ΖΡ=0;this.π=π;}public void ΖЎ(List<IMyThrust>ŕ,List<string>ǎ){Я=new float[ǎ.Count];for(int i=0;i<Я.Length;i++)
Я[i]=-1;for(int i=0;i<ŕ.Count;i++){string Ȇ=Δμ(ŕ[i]);int Α=ǎ.IndexOf(Ȇ);if(Α!=-1)
Я[Α]=С(ŕ[i].MaxEffectiveThrust,ŕ[i].MaxThrust);}}}class Œ{public Ѕ П=Ѕ.Ββ;public float е=0;public float ǲ=0;public string ф="";public string ч="";DateTime ΓΪ;public bool Ζΰ=false;private bool Ǉ=false;public bool ΔΒ(bool Αδ){if(Αδ){ǲ=0;Ǉ=false;return false;}Ǉ=true;return ǲ>е;
}public void ΖА(){ΔΒ(true);Ζΰ=false;}public void ΖБ(){if(Ǉ)
if((DateTime.Now-ΓΪ).TotalSeconds>1){ǲ++;ΓΪ=DateTime.Now;}}public bool ΒΝ(){switch(П){case Ѕ.Βγ:return true;case Ѕ.Βδ:return true;}return false;}}class Ǫ{public String η="";public String Ќ="";public int Ј=0;public Ņ ΑΤ=Ņ.ΓЅ;public Ǫ(String η,String Ќ,int Ј,Ņ ΑΤ){this.η=η;this.Ќ=Ќ;
this.Ј=Ј;this.ΑΤ=ΑΤ;}}class ί{public DateTime ΑΟ;public DateTime ΒЃ;public String Ν="";public String η="";public String ƅ="";public String Ω="";public String Ασ="";public String Ŷ="";public String Αί="";public Vector3 Π=new Vector3();public List<Ǫ>њ=new List<Ǫ>();public Ο Ρ=Ο.Ǘ;
public ι Ǚ;public float ǹ;public float ǯ;public int ς;public int ǉ=0;public int Αϋ=0;public bool Ƣ(){return(DateTime.Now-ΑΟ).TotalSeconds>10;}public bool Ɵ(){if(Ǚ!=ι.Ȑ) return false;return(DateTime.Now-ΒЃ).TotalSeconds>=4;}public ί(String Ν){this.Ν=Ν;}public ι Ǖ(String μ,bool Ζξ,bool ѕ,bool Εω)
{if(μ==""&&!ѕ) return ι.None;if(Ǚ==ι.Ȑ&&Ɵ())
Ǚ=ι.ň;if(Ζξ){Αί=μ;Ǚ=ι.Ȑ;ΒЃ=DateTime.Now;return Ǚ;}else if(ѕ){Αί="";Ǚ=ι.None;}else if(Αί==μ){if(Εω) Ǚ=ι.ĉ;return Ǚ;}return ι.None;}}public String Δγ(String ΖΙ){switch(ΖΙ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Н;Vector3 Γξ=new Vector3();Vector3 м=new Vector3();Vector3 ΕЊ=new Vector3();Vector3 ΖΜ=new Vector3();Vector3 ΕЃ=new Vector3();DateTime ΓΩ=new DateTime();
bool Ġ=true;int Αψ=0;bool ĩ=false;bool ǝ=false;bool ΖΦ=true;bool ΖΕ=false;float Ζπ=0;float Γφ=0;int Ζθ=0;int ž=0;float ȣ=0;float ȕ=0;double ΑΩ=0;float đ=0;List<int>Ȟ=new List<int>();List<int>Ȯ=new List<int>();Ο Ρ=Ο.Ǘ;int Ακ=0;float ȅ=0;float Ȅ=0;float Ȳ=0;float Γυ=0,Γτ=0;
IMyRemoteControl Њ;IMySensorBlock ώ;List<IMyTimerBlock>Γύ=new List<IMyTimerBlock>();List<IMyShipConnector>Ę=new List<IMyShipConnector>();List<IMyThrust>Ķ=new List<IMyThrust>();List<IMyGyro>ΓΘ=new List<IMyGyro>();List<IMyTerminalBlock>а=new List<IMyTerminalBlock>();List<IMyLandingGear>ΑΞ=new List<IMyLandingGear>();
List<IMyReactor>Αγ=new List<IMyReactor>();List<IMyConveyorSorter>Ζσ=new List<IMyConveyorSorter>();List<IMyGasTank>ħ=new List<IMyGasTank>();List<IMyTerminalBlock>Đ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Βϊ=new List<IMyBatteryBlock>();List<IMyTextPanel>Љ=new List<IMyTextPanel>();
List<IMyTextSurface>ь=new List<IMyTextSurface>();List<IMyTextPanel>Ǳ=new List<IMyTextPanel>();IMyTerminalBlock ǣ=null;ά Σ=new ά();ά ΓΞ=new ά();ά ΓΦ=new ά();bool ǧ=false;bool Ĺ=false;bool ĺ=false;bool ΓΤ=false;List<String>ǎ=new List<string>();List<ά>ω=new List<ά>();List<ά>Ɔ=new List<ά>();
double ΖЅ=0;ά Χ=new ά();Vector3 ĭ;Vector3 Ȥ;Τ φ=Τ.ё;У ȟ=У.ő;int к=0;int й=0;double Ű=0;bool ΒЉ=false;Œ χ=new Œ();Œ ψ=new Œ();ǒ ǡ=ǒ.Βη;bool Ļ=true;У ȁ=У.ő;int Ц=3;int Х=3;int д=30;я ј=я.Ɠ;bool Ȉ=true;bool Ĩ=true;bool Ğ=true;bool ȃ=false;в ĝ=в.Βα;bool Ǧ=true;
float ш=90;float щ=20;float Ы=5;float ъ=20;float ǘ=0.70f;float ƈ=1.50f;float Ƈ=2.50f;float ļ=0f;float Ĥ=0f;Μ Š;Τ ǟ;Μ Ĵ;int Ū=0;int ū=0;int с=0;int ī=0;int Αη=30;bool Ζτ=true;double ΒЎ=0;bool Γπ=false;int ΖІ=0;int ȇ=0;ά ſ=null;float ΒЈ=0;float ΑΨ=0;ά ΕЁ=null;
float Βϋ=0;Ǐ Ē;float ΓΣ=0;float ΓЀ=0;String ΖΟ="";float ΑΣ=0;float ǯ=0;float ǹ=0;float Ђ=0;bool Αν=false;bool Ζϊ=false;bool Ζρ=false;bool Γό=false;bool Ζς=false;float ΖΠ=0;float Ζή=0;Vector3 ΕЈ=new Vector3();Vector3 Γε=new Vector3();bool ΕЏ=false;float[,]ĸ=new float[3,2];
Dictionary<String,float[,]>Ǎ=new Dictionary<string,float[,]>();Dictionary<String,float[]>ż=new Dictionary<string,float[]>();bool ΒЋ=false;int Ŗ=0;int Ǜ=0;int ŭ=0;int ǵ=0;String Ω="";int[]Γβ=new int[Enum.GetValues(typeof(Κ)).Length];bool ΑΫ=false;int Ǻ=0;int ς=0;Κ γ=Κ.Main;
int Ζϋ=0,Ζό=0,Ζύ=0,Ζώ=0;int Ζι=0,Ζκ=0;bool Ĕ=false;bool Βφ=false;String Ĳ="";String ǃ="";IMyBroadcastListener є=null;bool ΓΓ=true;List<ί>ε=null;ί σ=null;int Ŝ=0,ŝ=0;int ΒЅ=0,ΒІ=0;int Εώ=0,ΕϏ=0,ΕЀ=0;String ΖΚ="";Exception ȡ=null;bool Γο=false;Τ Αθ=Τ.ё;
bool ΒЊ=false;String ΖЃ="";bool ΓϏ=false;bool Αρ=true;int ΑΛ=0;Vector3 Ǆ=new Vector3();bool ΓΖ=false;bool ơ(IMyTerminalBlock α)=>α.CubeGrid==Me.CubeGrid;Program(){Н=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΞ(Me,controllerTag,true))
Ρ=Ο.Е;Ψ(Κ.Main);if(Ρ!=Ο.Е){Ω="Welcome to [PAM]!";п Αε=ΕΓ();if(Ρ==Ο.Ǘ){List<IMyShipDrill>ΓΑ=new List<IMyShipDrill>();List<IMyShipGrinder>ΓΗ=new List<IMyShipGrinder>();Н.GetBlocksOfType(ΓΑ,q=>q.CubeGrid==Me.CubeGrid);Н.GetBlocksOfType(ΓΗ,q=>q.CubeGrid==Me.CubeGrid);if(ΓΑ.Count>0)
{Ρ=Ο.Ƨ;Ω="Miner mode enabled!";}else if(ΓΗ.Count>0){Ρ=Ο.Ȏ;Ω="Grinder mode enabled!";}else{Ρ=Ο.ο;Βμ(ǖ.ο);}}if(Αε==п.Βπ) ΖΦ=false;if(Αε==п.ΑЏ) Ω="Data restore failed!";if(Αε==п.Βΰ) Ω="New version, wipe data";}}void Main(string А,UpdateType ΖЀ){try{if(ȡ!=null){if(А.Trim().ToUpper()=="RETRY")
{ȡ=null;Ġ=true;Ζθ=0;if(Ρ!=Ο.Е) Ǭ();Ω="Restarted after error";}else{Δρ();ΔА();return;}}ǝ=(ΖЀ&UpdateType.Update10)!=0;if(ǝ) Αψ++;ĩ=Αψ>=10;if(ĩ) Αψ=0;if(ǝ){Ŗ++;if(Ŗ>4) Ŗ=0;ž=(int) Math.Max(0,10-(DateTime.Now-ΓΩ).TotalSeconds);}if(А!="") ΖΚ=А;if(Ρ!=Ο.Е)
ΕΦ(А);else
ΔΗ(А);ΑΫ=false;try{int ΒЇ=Runtime.CurrentInstructionCount;float Ǿ=С(ΒЇ,Runtime.MaxInstructionCount);if(Ǿ>0.90) Ω="Max. instructions >90%";if(Ǿ>ȣ) ȣ=Ǿ;if(ΒЋ){Ȟ.Add(ΒЇ);while(Ȟ.Count>10) Ȟ.RemoveAt(0);ȕ=0;for(int i=0;i<Ȟ.Count;i++) ȕ+=Ȟ[i];ȕ=С(С(ȕ,Ȟ.Count),Runtime.MaxInstructionCount);
double ΑΠ=Runtime.LastRunTimeMs;if(ΖΕ&&ΑΠ>ΑΩ) ΑΩ=ΑΠ;Ȯ.Add((int)(ΑΠ*1000f));while(Ȯ.Count>10) Ȯ.RemoveAt(0);đ=0;for(int i=0;i<Ȯ.Count;i++) đ+=Ȯ[i];đ=С(đ,Ȯ.Count)/1000f;}}catch{ȣ=0;}}catch(Exception e){ȡ=e;}}String Δδ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ρ!=Ο.ο)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Ψ(Κ Ў){Γβ[(int) γ]=ς;ς=Γβ[(int) Ў];if(Ў==Κ.ċ) ς=0;γ=Ў;if(Ρ!=Ο.Е)
Ŕ(γ==Κ.Ĉ,false,0,0);ΑΫ=true;}void ΔЁ(bool ΑΥ){if(ς>0) ς--;else if(ΑΥ) ς=Ǻ-1;}void ΔЀ(bool ΑΥ){if(ς<Ǻ-1) ς++;else if(ΑΥ) ς=0;}String Βμ(ǖ ζ){switch(ζ){case ǖ.Βι:Ω="Job is running";break;case ǖ.ΔΖ:Ω="Connector not ready!";break;case ǖ.ΕΧ:Ω="Ship modified, path outdated!";break;
case ǖ.Δϊ:Ω="Interrupted by player!";break;case ǖ.ο:Ω="Shuttle mode enabled!";break;}return "";}String Δλ(У Φ){switch(Φ){case У.ő:return "Top-Left";case У.ΔΑ:return "Center";default:return "";}}String Δΰ(я λ){switch(λ){case я.ΓЈ:return "Auto"+(Ρ==Ο.Ƨ?" (Ore)":"");case я.ΑЇ:return "Auto (+Stone)";
case я.Ɠ:return "Default";default:return "";}}String Δθ(Τ Ѐ){switch(Ѐ){case Τ.ё:return "No job";case Τ.ƪ:return "Job paused";case Τ.Ǩ:return "Job active";case Τ.Ľ:return "Job active";case Τ.ľ:return "Job active";case Τ.ΑЎ:return "Job done";case Τ.Ł:return "Job changed";case Τ.Βή:return "Move home";
case Τ.Βί:return "Move to job";}return "";}String ΒΓ(ǒ Ευ){switch(Ευ){case ǒ.Βη:return "Return home";case ǒ.ΔΩ:return "Fly to job pos";case ǒ.ΕΪ:return "Stop";case ǒ.Ɲ:return "Ignore";}return "";}String Δβ(в λ){switch(λ){case в.Βα:return "Off";case в.ΔΠ:return "Drop pos (Stone) ";case в.ΔΟ:return "Drop pos (Sto.+Ice)";
case в.ΔΙ:return "Cur. pos (Stone)";case в.ΔΘ:return "Cur. pos (Sto.+Ice)";case в.Δψ:return "In motion (Stone)";case в.Δχ:return "In motion (Sto.+Ice)";}return "";}String ΒΑ(Ǐ Ѐ){switch(Ѐ){case Ǐ.None:return "No batteries";case Ǐ.Ə:return "Charging";case Ǐ.ΔΛ:return "Discharging";}return "";}
String ΒΗ(Μ Ѐ){String ΗΒ=Ρ==Ο.ο?"target":"job";switch(Ѐ){case Μ.ǩ:return "Idle";case Μ.Ƙ:return "Flying to XY position";case Μ.Βέ:return Ρ==Ο.Ȏ?"Grinding":"Mining";case Μ.ƭ:return "Returning";case Μ.ΔΦ:return "Flying to drop pos";case Μ.Βθ:return "Returning to dock";case Μ.Ć:return "Flying to dock area";
case Μ.Ǔ:return "Flying to job area";case Μ.Ɨ:return "Flying to path";case Μ.ΔΪ:return "Flying to job position";case Μ.Ȋ:return "Approaching dock";case Μ.ΑЍ:return "Docking";case Μ.ΓЄ:return "Aligning";case Μ.ΑІ:return "Aligning";case Μ.ΕΕ:return "Retry docking";case Μ.Ď:return "Unloading";case Μ.ď:return ΖЃ;
case Μ.č:return "Undocking";case Μ.Ə:return "Charging batteries";case Μ.Εξ:return "Waiting for uranium";case Μ.ΔΥ:return "Filling up hydrogen";case Μ.Εμ:return "Waiting for ejection";case Μ.Εν:return "Waiting for ejection";case Μ.ΔΧ:return "Flying to drop pos";}return "";}String ΒΜ(Ѕ П){switch(П)
{case Ѕ.Ββ:return "On \"Undock\" command";case Ѕ.Βγ:return "On player entered cockpit";case Ѕ.ΔЋ:return "On ship is full";case Ѕ.ΔЊ:return "On ship is empty";case Ѕ.Βδ:return "On time delay";case Ѕ.ΔІ:return "On batteries empty(<25%)";case Ѕ.ΔЄ:return "On batteries empty(=0%)";case Ѕ.ΔЅ:return "On batteries full";
case Ѕ.ΔЉ:return "On hydrogen empty(<25%)";case Ѕ.ΔЇ:return "On hydrogen empty(=0%)";case Ѕ.ΔЈ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Γσ,int ǚ,bool Ε,String Ƿ){Ǻ+=1;if(Γσ==ǚ) Ƿ=">"+Ƿ+(Ŗ>=2?" .":"");else Ƿ=" "+Ƿ;Β+=Ƿ+"\n";return Γσ==ǚ&&Ε;}String ΓЊ(bool Ε)
{int Α=0;int Δ=ς;Ǻ=0;String ȱ="———————————————\n";String Ā="--------------------------------------------\n";String Β="";Β+=Δθ(φ)+" | "+(Σ.ν?"Ready to dock":"No dock")+"\n";Β+=ȱ;double Γϊ=Math.Max(Math.Round(this.ΒЎ),0);if(γ==Κ.Main){bool ǜ=Ρ==Ο.ο;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΕΨ();
if(Ρ==Ο.Ƨ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Ψ(Κ.Ĉ);if(Ρ==Ο.Ȏ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Ψ(Κ.Ĉ);if(Ρ==Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Ψ(Κ.Ƴ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ΑЋ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΔΨ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΔΫ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ǜ) Ψ(Κ.Ŏ);else Ψ(Κ.ŀ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Ψ(Κ.ΒΠ);if(Ρ!=Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Ψ(Κ.ΒΟ);}else if(γ==Κ.Ĉ){double ΖЇ=Math.Round(Ц*ȅ,1);double ΖΑ=Math.Round(Х*Ȅ,1);String ξ="";if(Ζ(ref ξ,Δ,Α++,Ε," Start new job!")) Βο();if(Ζ(ref ξ,Δ,Α++,Ε," Change current job")){Ʒ(false,false);Ψ(Κ.Main);}if(Ζ(ref ξ,Δ,Α++,Ε," Width + (Width: "+Ц+" = "+ΖЇ+"m)")){ȉ(ref Ц,5,20,1);ȓ(true);}
if(Ζ(ref ξ,Δ,Α++,Ε," Width -")){ȉ(ref Ц,-5,20,-1);ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Height + (Height: "+Х+" = "+ΖΑ+"m)")){ȉ(ref Х,5,20,1);ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Height -")){ȉ(ref Х,-5,20,-1);ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Depth + ("+(ј==я.Ɠ?"Depth":"Min")+": "+д+"m)")){ȉ(ref д,5,50,2);ȓ(true);}
if(Ζ(ref ξ,Δ,Α++,Ε," Depth -")){ȉ(ref д,-5,50,-2);ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Depth mode: "+Δΰ(ј))){ј=Ǒ(ј);}if(Ζ(ref ξ,Δ,Α++,Ε," Start pos: "+Δλ(ȁ))){ȁ=Ǒ(ȁ);}if(Ρ==Ο.Ȏ&&ј==я.ΑЇ) ј=Ǒ(ј);Β+=Ʈ(8,ξ,Δ,ref Ζι);}else if(γ==Κ.Ƴ){float[]ΒЌ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Ψ(Κ.Βν);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΜ(χ.П))) χ.П=Ǒ(χ.П);if(!χ.ΒΝ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƙ((int) χ.е))) χ.е=ΑЌ(χ.е,ΒЌ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΜ(ψ.П))) ψ.П=Ǒ(ψ.П);
if(!ψ.ΒΝ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƙ((int) ψ.е))) ψ.е=ΑЌ(ψ.е,ΒЌ);}else if(γ==Κ.Βν){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) Βο();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Ƴ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.ф!=""?χ.ф:"-"))) χ.ф=Ň(ref Ζϋ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.ч!=""?χ.ч:"-"))) χ.ч=Ň(ref Ζύ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ψ.ф!=""?ψ.ф:"-"))) ψ.ф=Ň(ref Ζό);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ψ.ч!=""?ψ.ч:"-"))) ψ.ч=Ň(ref Ζώ);
}else if(γ==Κ.ć){String Γέ=ш+" %";if(ĩ) Ǜ++;if(Ǜ>1){Ǜ=0;ŭ++;if(ŭ>1) ŭ=0;}bool[]Αξ=new bool[]{Αγ.Count==0,Ē==Ǐ.None,ħ.Count==0};int Γώ=0;while(true){Γώ++;ǵ++;if(ǵ>Αξ.Length-1) ǵ=0;if(Γώ>=Αξ.Length) break;if(!Αξ[ǵ]) break;}bool ǜ=Ρ==Ο.ο;if(!ǜ&&Ȉ&&Ђ!=-1&&ŭ==0)
Γέ=Ђ<1000000?Math.Round(Ђ)+" Kg":Math.Round(Ђ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){Ǭ();Ψ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ǜ) Ψ(Κ.ŀ);else Ψ(Κ.Ŏ);if(!ǜ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΓЃ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΓϏ=true;Β+=Ā;if(!ǜ) Β+="Progress: "+Math.Round(Ű,1)+" %\n";Β+="State: "+ΒΗ(Š)+" "+Γϊ+"m \n";Β+="Load: "+ΑΣ+" % Max: "+Γέ+" \n";if(ǵ==0) Β+="Uranium: "+(Αγ.Count==0?"No reactors":Math.Round(ΓЀ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ǵ==1) Β+="Battery: "+(Ē==Ǐ.None?ΒΑ(Ē):Βϋ+"% "+(щ==-1||ǜ?"":" Min: "+щ+" %"))+"\n";if(ǵ==2) Β+="Hydrogen: "+(ħ.Count==0?"No tanks":Math.Round(ΓΣ,1)+"% "+(ъ==-1||ǜ?"":" Min: "+ъ+" %"))+"\n";}else if(γ==Κ.ŀ){String ξ="";if(Ζ(ref ξ,Δ,Α++,Ε," Back")){if(φ==Τ.Ǩ) Ψ(Κ.ć);else Ψ(Κ.Main);}
if(Ζ(ref ξ,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ƥ,false);if(Ζ(ref ξ,Δ,Α++,Ε," Weight limit: "+(Ȉ?"On":"Off"))) Ȉ=!Ȉ;if(Ζ(ref ξ,Δ,Α++,Ε," Ejection: "+Δβ(ĝ))) ĝ=Ǒ(ĝ);if(Ζ(ref ξ,Δ,Α++,Ε," Toggle sorters: "+(ȃ?"On":"Off"))){ȃ=!ȃ;if(ȃ) Ʋ(Αρ);}
if(Ζ(ref ξ,Δ,Α++,Ε," Unload ice: "+(Ǧ?"On":"Off"))) Ǧ=!Ǧ;if(Ζ(ref ξ,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.Ʀ,true);if(Ζ(ref ξ,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Min "+щ+"%"))) Д(ref щ,-5,τ.ΒΫ,true);if(Ζ(ref ξ,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) Д(ref ъ,-10,τ.Βά,true);
if(Ζ(ref ξ,Δ,Α++,Ε," When done: "+(Ļ?"Return home":"Stop"))) Ļ=!Ļ;if(Ζ(ref ξ,Δ,Α++,Ε," On damage: "+ΒΓ(ǡ))) ǡ=Ǒ(ǡ);if(Ζ(ref ξ,Δ,Α++,Ε," Advanced...")) Ψ(Κ.Ƌ);Β+=Ʈ(8,ξ,Δ,ref Ζκ);}else if(γ==Κ.Ƌ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǩ) Ψ(Κ.ć);else Ψ(Κ.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ρ==Ο.Ȏ?" Grinder":" Drill")+" inv. balancing: "+(Ĩ?"On":"Off"))) Ĩ=!Ĩ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ρ==Ο.Ȏ?" grinders":" drills")+": "+(Ğ?"Fwd + Bwd":"Fwd"))) Ğ=!Ğ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+ƈ+"m/s")) Д(ref ƈ,0.5f,τ.Ƹ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ƈ+"m/s")) Д(ref Ƈ,0.5f,τ.Ƹ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǘ*100f)+"%"+(ǘ>0.80f?" (risky)":""))) Д(ref ǘ,0.1f,τ.Ɗ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ļ*100f+"%")) ΑЊ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+Ĥ*100f+"%")) ΑЊ(false,0.05f);
}else if(γ==Κ.Ŏ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǩ) Ψ(Κ.ć);else Ψ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ƥ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(Ǧ?"On":"Off"))) Ǧ=!Ǧ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.Ʀ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Charge up"))) щ=(щ==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Fill up"))) ъ=(ъ==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ΒΓ(ǡ))) ǡ=Ǒ(ǡ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǘ*100f)+"%"+(ǘ>0.80f?" (risky)":""))) Д(ref ǘ,0.1f,τ.Ɗ,false);
}else if(γ==Κ.Βζ){double ΖΛ=0;if(ω.Count>0) ΖΛ=Vector3.Distance(ω.Last().Π,м);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) Εέ();if(Ρ!=Ο.ο){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ǧ?"Use old home":(Σ.ν?"Was set! ":"none ")))) ǧ=!ǧ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ǧ?"Use old connector":(Σ.ν?"Was set! ":"none ")))) ǧ=!ǧ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ĺ?"Use old connector":(Χ.ν?"Was set! ":"none ")))) Ĺ=!Ĺ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ĺ?"Use old path":(ω.Count>1?"Count: "+ω.Count:"none ")))) ĺ=!ĺ;Β+=Ā;Β+="Wp spacing: "+Math.Round(ΖЅ)+"m\n";}else if(γ==Κ.ƶ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){Ǭ();Ψ(Κ.Main);}
Β+=Ā;Β+="State: "+ΒΗ(Š)+" \n";Β+="Speed: "+Math.Round(Ζπ,1)+"m/s\n";Β+="Target dist: "+Γϊ+"m\n";Β+="Wp count: "+ω.Count+"\n";Β+="Wp left: "+ΖІ+"\n";}else if(γ==Κ.ΒΠ){List<IMyTerminalBlock>ǰ=Δή();if(ĩ) Ǜ++;if(Ǜ>=ǰ.Count) Ǜ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Ψ(Κ.ΒΡ);
Β+=Ā;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΣ,1)+"% "+Math.Round(ǯ,1)+" / "+Math.Round(ǹ,1)+"\n";Β+="Uranium: "+(Αγ.Count==0?"No reactors":Math.Round(ΓЀ,1)+"Kg "+ΖΟ)+"\n";Β+="Battery: "+(Ē==Ǐ.None?"":Βϋ+"% ")+ΒΑ(Ē)+"\n";Β+="Hydrogen: "+(ħ.Count==0?"No tanks":Math.Round(ΓΣ,1)+"% ")+"\n";
Β+="Damage: "+(ǰ.Count==0?"None":""+(Ǜ+1)+"/"+ǰ.Count+" "+ǰ[Ǜ].CustomName)+"\n";}else if(γ==Κ.ΒΡ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=Ā;Β+="Next scan: "+ž+"s\n";Β+="Drill area: "+Math.Round(ȅ,1)+"x"+Math.Round(Ȅ,1)+"m Ship: "+Math.Round(Γυ,1)+"x"+Math.Round(Γτ,1)+"m ("+Math.Round(Ȳ,1)+"m)\n"+(Εψ.Count>0?"Cameras: "+Εχ+"\n":"");
Β+="Broadcast: "+(Ĕ?"Online - "+Ĳ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȣ*100f,1)+"% \n";}else if(γ==Κ.ΒΟ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=Ā;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Ακ==2) Β="Fatal setup error\nNext scan: "+ž+"s\n";if(Γο) Β="Recompile script now";int ΖΝ=Β.Split('\n').Length;for(int i=ΖΝ;i<=10;i++) Β+="\n";Β+=ȱ;Β+="Last: "+Ω+"\n";
return Β;}void ΑЊ(bool ΖΘ,float Ɓ){Ǭ();Ʒ(true,false);if(ΖΘ) Д(ref ļ,Ɓ,τ.Ʃ,false);else Д(ref Ĥ,Ɓ,τ.Ʃ,false);ΓБ();Ŕ(true,true,0,0);}float ΑЌ(float Ș,float[]Ƅ){float έ=Ƅ[0];for(int i=Ƅ.Length-1;i>=0;i--)
if(Ș<Ƅ[i]) έ=Ƅ[i];return έ;}String Ň(ref int ǚ){String έ="";if(ǚ>=Γύ.Count) ǚ=-1;if(ǚ>=0) έ=Γύ[ǚ].CustomName;ǚ++;return έ;}void ȉ(ref int value,int ē,int Ƃ,int Αο){if(ē==0) return;if(value<Ƃ&&Αο>0||value<=Ƃ&&Αο<0){value+=Αο;return;}int Επ=Math.Abs(ē);int ƾ=0;
int Αά=1;while(true){ƾ+=Αά*Επ*10;if(ē<0&&value-Ƃ<=ƾ) break;if(ē>0&&value-Ƃ<ƾ) break;Αά++;}value+=Αά*ē;}void ȓ(bool ΗΖ){Ц=Math.Max(Ц,1);Х=Math.Max(Х,1);д=Math.Max(д,0);Ŕ(γ==Κ.Ĉ,false,0,0);}void Д(ref float value,float Ɓ,τ Ќ,bool ķ){Д(ref value,value+Ɓ,Ќ,ķ,true);
}void Д(ref float value,float ȥ,τ Ќ,bool ķ,bool ΖЊ){float Ь=0,Ъ=0;if(Ќ==τ.Ƹ){Ь=0.5f;Ъ=10f;}if(Ќ==τ.Ɗ){Ь=0.1f;Ъ=1f;}if(Ќ==τ.Ɲ){Ь=50f;Ъ=100f;}if(Ќ==τ.ΒΫ){Ь=5f;Ъ=30f;}if(Ќ==τ.Ʀ){Ь=1f;Ъ=25f;}if(Ќ==τ.Ƥ){Ь=10f;Ъ=95f;}if(Ќ==τ.Βά){Ь=10f;Ъ=90f;}
if(Ќ==τ.Εγ){Ь=10f;Ъ=1800;}if(Ќ==τ.Ʃ){Ь=0.0f;Ъ=0.75f;}if(ȥ==-1&&ķ){value=-1;return;}if(value==-1) ķ=false;bool Γη=ȥ<Ь||ȥ>Ъ;if(Γη&&ΖЊ){if(ȥ<value) value=Ъ;else if(ȥ>value) value=Ь;}else value=ȥ;if(Γη&&ķ) value=-1;else value=Math.Max(Ь,Math.Min(value,Ъ));
value=(float) Math.Round(value,2);}void ΓЃ(bool Ζβ){if(Ζβ) с=Math.Max(0,с-1);else с++;ΔΣ();ΔΡ();for(int k=0;k<ΑΙ.Count&&k<з.Length;k++) ΕΡ(ΑΙ[k],k<с);Ǽ=-1;Βυ(true);}T Ǒ<T>(T Ș){int Α=Array.IndexOf(Enum.GetValues(Ș.GetType()),Ș);Α++;if(Α>=ΔΤ(Ș)) Α=0;return(T) Enum.GetValues(Ș.GetType()).GetValue(Α);
}int ΔΤ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΕΨ(){Ɔ.Clear();for(int i=0;i<ω.Count;i++) Ɔ.Add(ω[i]);ω.Clear();ΓΤ=true;ΓΞ=new ά(Σ);ΓΦ=new ά(Χ);Σ.ν=false;if(Ρ==Ο.ο) Χ.ν=false;for(int i=0;i<Ǎ.Count;i++)
if(!ǎ.Contains(Ǎ.Keys.ElementAt(i)))
ǎ.Add(Ǎ.Keys.ElementAt(i));ǧ=false;Ĺ=false;ĺ=false;Ψ(Κ.Βζ);}void Εέ(){if(ǧ) Σ=ΓΞ;if(Ĺ) Χ=ΓΦ;if(ĺ){ω.Clear();for(int i=0;i<Ɔ.Count;i++) ω.Add(Ɔ[i]);}ΓΤ=false;Ǭ();Ψ(Κ.Main);}void Ō(ά ж,Τ Ѐ){ΕЁ=ж;if(φ==Τ.Ǩ) ǟ=Ѐ;}bool ΒΔ(ά ж,float у,bool ΖЁ,out Vector3 Φ){if(ΖЁ){Vector3I ΕЎ=new Vector3I((int) ж.Ч.X,(int) ж.Ч.Y,(int) ж.Ч.Z);
IMySlimBlock α=Me.CubeGrid.GetCubeBlock(ΕЎ);if(α==null||!(α.FatBlock is IMyShipConnector)){Φ=new Vector3();return false;}Vector3 Εύ=ņ(Њ,α.FatBlock.GetPosition()-м);Vector3 Εό=ņ(Њ,α.FatBlock.WorldMatrix.Forward);Φ=ж.Π-Βς(ж.І,ж.Ё*-1,Εύ)-Βς(ж.І,ж.Ё*-1,Εό)*у;return true;}
else{Φ=ж.Π;return true;}}Vector3 Δζ(int ǆ,int Ǥ,bool Ζε){if(!Ζε&&ΓΖ) return Ǆ;float Εϋ=((к-1f)/2f)-ǆ;float Ζη=((й-1f)/2f)-Ǥ;Ǆ=Χ.Π+Χ.О*Εϋ*ȅ+Ȥ*-1*Ζη*Ȅ;ΓΖ=true;return Ǆ;}Υ Ɣ(){float Ф=-1;Υ έ=Υ.о;if(Ρ!=Ο.ο){ΔϏ();if(φ!=Τ.ё){Vector3 ύ=Βσ(ĭ,Ȥ*-1,м-Χ.Π);
if(Math.Abs(ύ.X)<=(float)(к*ȅ)/2f&&Math.Abs(ύ.Y)<=(float)(й*Ȅ)/2f){if(ύ.Z<=-1&&ύ.Z>=-Αη*2) return Υ.Ŀ;if(ύ.Z>-1&&ύ.Z<Ȳ*2) return Υ.о;}if(ǐ(Χ.Π,ref Ф)) έ=Υ.о;}if(Σ.ν){if(ǐ(Σ.Π,ref Ф)) έ=Υ.н;for(int i=0;i<ω.Count;i++)
if(ǐ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Vector3.Distance(м,Σ.Π)<dockDist*Ȳ) έ=Υ.н;if(Ȍ(MyShipConnectorStatus.Connectable)!=null||Ȍ(MyShipConnectorStatus.Connected)!=null)
έ=Υ.н;}}else{Vector3 Φ=new Vector3();IMyShipConnector ś=Ȍ(MyShipConnectorStatus.Connected);if(Σ.ν){if(ǐ(Σ.Π,ref Ф)) έ=Υ.н;if(ΒΔ(Σ,dockDist,true,out Φ))
if(ǐ(Φ,ref Ф)) έ=Υ.н;if(ś!=null&&Vector3.Distance(ś.GetPosition(),Σ.Π)<5)
return Υ.ƌ;}for(int i=0;i<ω.Count;i++)
if(Vector3.Distance(ω[i].Π,Σ.Π)>dockDist*Ȳ&&Vector3.Distance(ω[i].Π,Χ.Π)>dockDist*Ȳ)
if(ǐ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Χ.ν){if(ǐ(Χ.Π,ref Ф)) έ=Υ.о;if(ΒΔ(Χ,dockDist,true,out Φ))
if(ǐ(Φ,ref Ф)) έ=Υ.о;if(ś!=null&&Vector3.Distance(ś.GetPosition(),Χ.Π)<5)
return Υ.ƍ;}}return έ;}bool ǐ(Vector3 Φ,ref float Ф){float у=Vector3.Distance(Φ,м);if(у<Ф||Ф==-1){Ф=у;return true;}return false;}void Βο(){if(Ακ>0){Ω="Setup error! Can't start";return;}if(Ρ==Ο.ο){ΑЋ();return;}Χ.Π=м;Χ.π=Њ.GetNaturalGravity();Χ.І=ΕЊ;Χ.Ё=ΕЃ;Χ.О=ΖΜ;
ĭ=ǣ.WorldMatrix.Forward;Ȥ=Χ.Ё;if(ĭ==Њ.WorldMatrix.Down) Ȥ=Њ.WorldMatrix.Backward;Ʒ(true,true);δ(Μ.Ƙ);ƴ();}void Ʒ(bool Ш,bool Ζζ){if(φ==Τ.ё&&!Ш) return;bool ΖΧ=Ш||φ==Τ.ΑЎ||к!=Ц||й!=Х||ȟ!=ȁ;if(ΖΧ){if(φ!=Τ.ё){φ=Τ.Ł;Δζ(Ū,ū,Ζζ);Ω="Job changed, lost progress";}ȟ=ȁ;
к=Ц;й=Х;ū=0;Ū=0;з="";Ǽ=-1;ΑΨ=0;ī=0;ΒЈ=0;с=0;Βυ(true);}}void ƫ(){ΕΟ(м,0);ŋ(Ķ,true);}void ƴ(){Βμ(ǖ.Βι);ƫ();Ʊ(ΑΞ,false);Ʋ(Αρ);φ=Τ.Ǩ;ΓА(true);ǟ=φ;Ψ(Κ.ć);ΕΘ();ΒЉ=true;ΑΛ=0;for(int i=Đ.Count-1;i>=0;i--)
if(ΒΤ(Đ[i],false)) ΑΛ++;if(ΑΛ>0) Ω="Started with damage";}void Ǭ(){if(φ==Τ.Ǩ){φ=Τ.ƪ;Ω="Job paused";}δ(Μ.ǩ);ǟ=φ;ΕΠ(false,0,0,0,0);ΕΫ();ΓІ(new Vector3(),false);Εά();ΕΜ(ChargeMode.Auto);ΕΣ(false);ΕΞ(true);ǫ(Μ.ǩ);Ŕ(false,false,0,0);ŋ(а,false);ŋ(Ę,true);Ʋ(true);Γπ=false;ΔΓ();
ΕЂ=false;ΕΑ();ΒЉ=false;Αν=false;ΓϏ=false;if(γ!=Κ.Main&&γ!=Κ.ŀ&&γ!=Κ.Ƌ&&γ!=Κ.Ŏ)
Ψ(Κ.Main);}void ΑЋ(){Υ ќ=Ɣ();if(Ρ==Ο.ο){if(!Χ.ν||!Σ.ν) return;ƴ();bool ΑΕ=Vector3.Distance(м,Σ.Π)<Vector3.Distance(м,Χ.Π);if(Αθ==Τ.Ľ) ΑΕ=true;if(Αθ==Τ.ľ) ΑΕ=false;if(ΑΕ){Ō(Σ,Τ.Ľ);switch(ќ){case Υ.ƌ:δ(Μ.ď);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.н:δ(Μ.Ȋ);break;default:δ(Μ.č);break;
}}else{Ō(Χ,Τ.ľ);switch(ќ){case Υ.ƍ:δ(Μ.ď);break;case Υ.о:δ(Μ.Ȋ);break;case Υ.ђ:δ(Μ.Ǔ);break;default:δ(Μ.č);break;}}}else{if(φ!=Τ.ƪ&&φ!=Τ.Ł) return;bool ΖЄ=φ==Τ.Ł;ƴ();bool ΑΗ=Δϋ(false)&&Σ.ν;switch(ќ){case Υ.о:δ(ΑΗ?Μ.Ɨ:Μ.Ƙ);break;case Υ.ђ:δ(ΑΗ?Μ.Ć:Μ.Ǔ);break;case Υ.н:δ(ΑΗ?Μ.Ȋ:Μ.Ď);break;
case Υ.Ŀ:{if(ī!=с||ΖЄ) δ(Μ.ƭ);else δ(Μ.Βέ);}break;default:break;}}}void ΔΫ(){if(φ==Τ.ё&&!Σ.ν) return;if(Ρ==Ο.ο&&(!Χ.ν||!Σ.ν)) return;Ω="Move to job";Υ ќ=Ɣ();if(Ρ==Ο.ο){Ō(Χ,Τ.ľ);switch(ќ){case Υ.о:δ(Μ.Ȋ);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.ƍ:return;default:δ(Μ.č);break;
}ǫ(Μ.ď);}else{switch(ќ){case Υ.о:δ(Μ.Ǔ);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.н:δ(Μ.Ď);break;case Υ.Ŀ:δ(Μ.ƭ);break;default:break;}if(φ==Τ.ё) ǫ(Μ.Ǔ);else ǫ(Μ.ΑІ);Γπ=true;}ƫ();Ψ(Κ.ƶ);Ʊ(ΑΞ,false);ǟ=Τ.Βί;}void ΔΨ(){if(!Σ.ν) return;Ω="Move home";Υ ќ=Ɣ();if(Ρ==Ο.ο){Ō(Σ,Τ.Ľ);switch(ќ)
{case Υ.ђ:δ(Μ.Ć);break;case Υ.н:δ(Μ.Ȋ);break;case Υ.ƌ:return;default:δ(Μ.č);break;}ǫ(Μ.ď);}else{if(Ȍ(MyShipConnectorStatus.Connected)!=null) return;if(Ȍ(MyShipConnectorStatus.Connectable)!=null){δ(Μ.ΑЍ);ǫ(Μ.Ď);return;}switch(ќ){case Υ.о:δ(Μ.Ɨ);break;case Υ.ђ:δ(Μ.Ć);break;case Υ.н:δ(Μ.Ć);break;
case Υ.Ŀ:δ(Μ.Βθ);break;default:break;}ǫ(Μ.Ď);}ƫ();Ψ(Κ.ƶ);Ʊ(ΑΞ,false);ǟ=Τ.Βή;}void δ(Μ Αέ){if(Αέ==Μ.ǩ) Ĵ=Μ.ǩ;if(Ĵ!=Μ.ǩ&&Š==Ĵ&&Αέ!=Ĵ){Ǭ();return;}Ζτ=true;Š=Αέ;}void ǫ(Μ Ѐ){this.Ĵ=Ѐ;}void Βυ(bool Αδ){if(Αδ){Ű=0;return;}if(Ρ==Ο.ο) return;float κ=с*Math.Max(1,д);
if(ī==с) κ+=Math.Min(д,ΒЈ);float ΖϏ=к*й*Math.Max(1,д);Ű=Math.Max(Ű,(float) Math.Min(κ/ΖϏ*100.0,100));}List<IMyTerminalBlock>Δή(){List<IMyTerminalBlock>ǰ=new List<IMyTerminalBlock>();for(int i=0;i<Đ.Count;i++){IMyTerminalBlock α=Đ[i];if(ΒΤ(α,false)) ǰ.Add(α);}return ǰ;
}bool ΒΤ(IMyTerminalBlock α,bool Ατ){return(!ΒΥ(α,Ατ)||!α.IsFunctional);}bool ΒΥ(IMyTerminalBlock α,bool Ατ){if(α==null) return false;try{IMyCubeBlock ΓΕ=Me.CubeGrid.GetCubeBlock(α.Position).FatBlock;if(Ατ) return ΓΕ==α;else return ΓΕ.GetType()==α.GetType();}catch{return false;}}bool Δϋ(bool ΗΔ)
{if(Ȉ&&Ρ!=Ο.ο)
if(Ђ!=-1&&Γφ>=Ђ){Ω="Ship too heavy";return true;}if(ΑΣ>=ш||Αν){Αν=false;Ω="Ship is full";return true;}return false;}static String Δμ(IMyThrust э){return э.BlockDefinition.SubtypeId;}Vector3 ƣ(Vector3 К,float[,]Ȣ){return new Vector3(
К.X>=0?Ȣ[0,0]:Ȣ[0,1],
К.Y>=0?Ȣ[1,0]:Ȣ[1,1],
К.Z>=0?Ȣ[2,0]:Ȣ[2,1]);}bool ΒΛ(ά ή,String Ȇ,out float ǌ){ǌ=0;int Α=ǎ.IndexOf(Ȇ);if(Α==-1||ή.Я==null||Α>=ή.Я.Length)
return false;ǌ=ή.Я[Α];if(ǌ==-1) return false;return true;}Vector3 ΒΚ(Vector3 К,ά ή){if(ή!=null){Vector3 έ=new Vector3();for(int i=0;i<Ǎ.Keys.Count;i++){String Ȇ=Ǎ.Keys.ElementAt(i);float Ĝ=0;if(!ΒΛ(ή,Ȇ,out Ĝ))
return ƣ(К,ĸ);έ+=ƣ(К,Ǎ.Values.ElementAt(i))*Ĝ;}return έ;}return ƣ(К,ĸ);}float ƛ(Vector3 К,ά ή){return ƛ(К,new Vector3(),ή);}float ƛ(Vector3 К,Vector3 π,ά ή){Vector3 Ζω=ΒΚ(К,ή);Vector3 ΕЅ=Ζω+π*Γφ;float Ǿ=(ΕЅ/К).AbsMin();return(float)(К*Ǿ).Length();}float ń(Vector3 ġ,Vector3 Ѓ,Vector3 π,ά ή)
{if(π.Length()==0f) return 0;Vector3 ΖΞ=Βσ(ġ,Ѓ,Vector3.Normalize(π));float Ш=ƛ(-ΖΞ,ή);return Ш/π.Length();}void ΓА(bool ΖΔ){float Р=0;float ΑΑ=0.9f;if(ΖΔ){Ђ=-1;ȇ=0;ſ=null;if(φ!=Τ.ё&&Χ.π.Length()!=0){Р=ΑΑ*ń(Χ.І,Χ.Ё*-1,Χ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}if(Σ.ν&&Σ.π.Length()!=0)
{Р=ΑΑ*ń(Σ.І,Σ.Ё*-1,Σ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}return;}if(ȇ==-1) return;if(ȇ>=0){int Γλ=0;while(ȇ<ω.Count){if(Γλ>100) return;Γλ++;ά ή=ω[ȇ];if(ή.π.Length()!=0f){Р=ΑΑ*Math.Min(
ń(ή.І,ή.Ё*-1,ή.π,ή),
ń(ή.І*-1,ή.Ё*-1,ή.π,ή));if(Р<Ђ||Ђ==-1) Ђ=Р;}else ſ=ή;ȇ++;}ȇ=-1;}bool ΑΖ=true;float Ǡ=0;if(ſ!=null){for(int i=0;i<Ǎ.Count;i++){String Ȇ=Ǎ.Keys.ElementAt(i);float[,]Αύ=Ǎ.Values.ElementAt(i);float Ĝ=0;if(!ΒΛ(ſ,Ȇ,out Ĝ)){ΑΖ=false;break;}for(int a=0;a<Αύ.GetLength(0);a++)
for(int b=0;b<Αύ.GetLength(1);b++){float ǭ=Math.Abs(Αύ[a,b]*Ĝ);if(ǭ==0) continue;ΑΖ=true;if(Ǡ==0||ǭ<Ǡ) Ǡ=ǭ;}}}if(!ΑΖ){for(int a=0;a<ĸ.GetLength(0);a++)
for(int b=0;b<ĸ.GetLength(1);b++){float ǭ=Math.Abs(ĸ[a,b]);if(ǭ==0) continue;if(Ǡ==0||ǭ<Ǡ) Ǡ=ǭ;}}if(Ǡ>0){float ΖΣ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Р=С(Ǡ,ΖΣ);if(Р>0)
if(Р<Ђ||Ђ==-1) Ђ=Р;}}void ΓІ(Vector3 Ш,bool Ϗ){if(!Ϗ){for(int i=0;i<Ķ.Count;i++)
Ķ[i].SetValueFloat("Override",0.0f);return;}Vector3 ǀ=ΒΚ(Ш,null);float ΖЋ=Math.Min(1,Math.Abs(С(Ш.X,ǀ.X)));float ΖЌ=Math.Min(1,Math.Abs(С(Ш.Y,ǀ.Y)));float ΖЍ=Math.Min(1,Math.Abs(С(Ш.Z,ǀ.Z)));for(int i=0;i<Ķ.Count;i++){IMyThrust э=Ķ[i];Vector3 Ț=ΕΖ(ņ(Њ,э.WorldMatrix.Backward),1);
if(Ț.X!=0&&Math.Sign(Ț.X)==Math.Sign(Ш.X))
э.SetValueFloat("Override",э.MaxThrust*ΖЋ);else if(Ț.Y!=0&&Math.Sign(Ț.Y)==Math.Sign(Ш.Y))
э.SetValueFloat("Override",э.MaxThrust*ΖЌ);else if(Ț.Z!=0&&Math.Sign(Ț.Z)==Math.Sign(Ш.Z))
э.SetValueFloat("Override",э.MaxThrust*ΖЍ);else
э.SetValueFloat("Override",0.0f);}}void ΕΠ(bool Ϗ,float Ζα,float ȩ,float ȭ,float Ą){for(int i=0;i<ΓΘ.Count;i++){IMyGyro ρ=ΓΘ[i];ρ.GyroOverride=Ϗ;if(!Ϗ) ρ.GyroPower=100;else ρ.GyroPower=Ζα;if(!Ϗ) continue;Vector3 М=Њ.WorldMatrix.Forward;Vector3 Ȭ=Њ.WorldMatrix.Right;Vector3 Ѓ=Њ.WorldMatrix.Up;
Vector3 ț=ρ.WorldMatrix.Forward;Vector3 ȝ=ρ.WorldMatrix.Up;Vector3 Ȝ=ρ.WorldMatrix.Left*-1;if(ț==М) ρ.SetValueFloat("Roll",Ą);else if(ț==(М*-1)) ρ.SetValueFloat("Roll",Ą*-1);else if(ȝ==(М*-1)) ρ.SetValueFloat("Yaw",Ą);else if(ȝ==М) ρ.SetValueFloat("Yaw",Ą*-1);else if(Ȝ==М) ρ.SetValueFloat("Pitch",Ą);
else if(Ȝ==(М*-1)) ρ.SetValueFloat("Pitch",Ą*-1);if(Ȝ==(Ȭ*-1)) ρ.SetValueFloat("Pitch",ȩ);else if(Ȝ==Ȭ) ρ.SetValueFloat("Pitch",ȩ*-1);else if(ȝ==Ȭ) ρ.SetValueFloat("Yaw",ȩ);else if(ȝ==(Ȭ*-1)) ρ.SetValueFloat("Yaw",ȩ*-1);else if(ț==(Ȭ*-1)) ρ.SetValueFloat("Roll",ȩ);
else if(ț==Ȭ) ρ.SetValueFloat("Roll",ȩ*-1);if(ȝ==(Ѓ*-1)) ρ.SetValueFloat("Yaw",ȭ);else if(ȝ==Ѓ) ρ.SetValueFloat("Yaw",ȭ*-1);else if(Ȝ==Ѓ) ρ.SetValueFloat("Pitch",ȭ);else if(Ȝ==(Ѓ*-1)) ρ.SetValueFloat("Pitch",ȭ*-1);else if(ț==Ѓ) ρ.SetValueFloat("Roll",ȭ);else if(ț==(Ѓ*-1)) ρ.SetValueFloat("Roll",ȭ*-1);
}}void Εά(){this.ΕЏ=false;}void ΕΫ(){ō(false,false,false,Γε,0);Γό=false;}void ΕΟ(Vector3 ΰ,float ĳ){ō(true,false,false,ΰ,ĳ);}void ō(bool Ϗ,bool Απ,bool Αώ,Vector3 ΰ,float ĳ){ō(Ϗ,Απ,Αώ,ΰ,ΰ-м,0.0f,ĳ);}void ō(bool Ϗ,bool Απ,bool Αώ,Vector3 ΰ,Vector3 Ζμ,float Ζν,float ĳ)
{Γό=true;this.Ζϊ=Ϗ;Γε=ΰ;this.ΖΠ=ĳ;this.Ζή=Ζν;this.Ζς=Απ;this.Ζρ=Αώ;this.ΕЈ=Ζμ;ΒЎ=Vector3.Distance(ΰ,м);}void ΕΘ(){Н.GetBlocksOfType(Đ,ơ);}void ΓБ(){if(Њ==null) return;ǣ=null;float ŷ=0,ų=0,Ÿ=0,Ŵ=0,Ź=0,ŵ=0;List<IMyTerminalBlock>Αυ=ΑА(а,pamTag,true);bool ΖΩ=Αυ.Count==0;
if(Αυ.Count>0) ǣ=Αυ[0];else if(а.Count>0) ǣ=а[0];int ş=0;float ы=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǣ.WorldMatrix.Forward){if(ΖΩ){Ακ=2;Ω="Mining direction is unclear!";return;}continue;}ş++;ы=Math.Max(ы,Εδ(а[i]));Vector3 ύ=ƚ(Њ,а[i].GetPosition());
if(i==0){ŷ=ύ.X;ų=ύ.X;Ÿ=ύ.Y;Ŵ=ύ.Y;Ź=ύ.Z;ŵ=ύ.Z;}ų=Math.Max(ύ.X,ų);ŷ=Math.Min(ύ.X,ŷ);Ŵ=Math.Max(ύ.Y,Ŵ);Ÿ=Math.Min(ύ.Y,Ÿ);ŵ=Math.Max(ύ.Z,ŵ);Ź=Math.Min(ύ.Z,Ź);}if(ы==0) ы=drillRadius;bool Ě=ǣ!=null&&ǣ.WorldMatrix.Forward==Њ.WorldMatrix.Down;ȅ=(ų-ŷ)*(1-ļ)+ы*2;
Ȅ=(Ě?ŵ-Ź:Ŵ-Ÿ)*(1-Ĥ)+ы*2;IMyCubeGrid g=Њ.CubeGrid;float ΑЁ=0,ΑЂ=0,ΑЃ=0,ΑЄ=0;for(int c=0;c<8;c++){Vector3 p=ƚ(Њ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ě?p.Z:p.Y;if(c==0||p.X<ΑЁ) ΑЁ=p.X;if(c==0||p.X>ΑЂ) ΑЂ=p.X;if(c==0||y<ΑЃ) ΑЃ=y;if(c==0||y>ΑЄ) ΑЄ=y;}Γυ=ΑЂ-ΑЁ+g.GridSize;Γτ=ΑЄ-ΑЃ+g.GridSize;}float Εδ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΑА<T>(List<T>Ї,String Ю,bool Ŭ){List<T>έ=new List<T>();for(int i=0;i<Ї.Count;i++)
if(ΒΞ(Ї[i],Ю,Ŭ)) έ.Add(Ї[i]);return έ;}bool ΒΞ<T>(T α,String Ю,bool Ŭ){IMyTerminalBlock b=(IMyTerminalBlock) α;if(Ŭ&&b.CustomName.ToUpper().Contains(Ю.ToUpper())) return true;if(!Ŭ&&b.BlockDefinition.ToString().ToUpper().Contains(Ю.ToUpper())) return true;return false;}void ŋ<T>(List<T>Ї,bool Ϗ)
{for(int i=0;i<Ї.Count;i++)
ΕΝ((IMyTerminalBlock) Ї[i],Ϗ);}void ΕΝ(IMyTerminalBlock α,bool Ϗ){if(α==null) return;String Ερ=Ϗ?"OnOff_On":"OnOff_Off";var a=α.GetActionWithName(Ερ);a.Apply(α);}void Ʋ(bool Ϗ){Αρ=Ϗ;if(!ȃ) return;ŋ(Ζσ,Ϗ);}void ΕΜ(ChargeMode λ){for(int i=0;i<Βϊ.Count;i++) Βϊ[i].ChargeMode=λ;}void ΕΣ(bool Ζφ)
{for(int i=0;i<ħ.Count;i++) ħ[i].Stockpile=Ζφ;}void Ʊ(List<IMyLandingGear>ΑΔ,bool Γή){for(int i=0;i<ΑΔ.Count;i++){if(Γή) ΑΔ[i].Lock();if(!Γή) ΑΔ[i].Unlock();}}void ΕΞ(bool Ϗ){if(ΒЊ==Ϗ) return;List<IMyShipController>Ş=new List<IMyShipController>();Н.GetBlocksOfType(Ş,ơ);if(Ş.Count==0) return;
for(int i=0;i<Ş.Count;i++)
Ş[i].DampenersOverride=Ϗ;ΒЊ=Ϗ;}IMyShipConnector Ȍ(MyShipConnectorStatus Ζυ){for(int i=0;i<Ę.Count;i++){if(!ΒΥ(Ę[i],true)) continue;if(Ę[i].Status==Ζυ) return Ę[i];}return null;}void Ŕ(bool Αλ,bool Ţ,float ΕЋ,float Ετ){if(ώ==null||а.Count==0) return;Vector3 Ȗ=new Vector3();
int ş=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǣ.WorldMatrix.Forward) continue;ş++;Ȗ+=а[i].GetPosition();}Ȗ=Ȗ/ş;Vector3 Ȧ=ƚ(ώ,Ȗ);ώ.Enabled=true;ώ.ShowOnHUD=Αλ;ώ.LeftExtend=(Ţ?1:Ц)/2f*ȅ-Ȧ.X;ώ.RightExtend=(Ţ?1:Ц)/2f*ȅ+Ȧ.X;ώ.TopExtend=(Ţ?1:Х)/2f*Ȅ+Ȧ.Y;
ώ.BottomExtend=(Ţ?1:Х)/2f*Ȅ-Ȧ.Y;ώ.FrontExtend=(Αλ?д:ΕЋ)-Ȧ.Z;ώ.BackExtend=Αλ?0:Ετ+Ȳ*0.75f+Ȧ.Z;ώ.DetectFloatingObjects=true;ώ.DetectAsteroids=false;ώ.DetectLargeShips=true;ώ.DetectSmallShips=true;ώ.DetectStations=true;ώ.DetectOwner=true;ώ.DetectSubgrids=false;
ώ.DetectPlayers=false;ώ.DetectEnemy=true;ώ.DetectFriendly=true;ώ.DetectNeutral=true;}void Ċ<T>(List<T>Ї,bool Ζο,float ΕЉ,bool ΖΗ){for(int i=0;i<Ї.Count;i++){IMyTextSurface ĵ=null;if(Ї[i]is IMyTextSurface) ĵ=(IMyTextSurface) Ї[i];if(ĵ!=null){ĵ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ζο) continue;ĵ.Font="Debug";if(ΖΗ) continue;ĵ.FontSize=ΕЉ;}}}static float С(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΕΖ(Vector3 v,int ǈ){return new Vector3(Math.Round(v.X,ǈ),Math.Round(v.Y,ǈ),Math.Round(v.Z,ǈ));}Vector3 ƚ(IMyTerminalBlock α,Vector3 ΖЉ){return ņ(α,ΖЉ-α.GetPosition());
}Vector3 ņ(IMyTerminalBlock α,Vector3 ΑЀ){return Vector3D.TransformNormal(ΑЀ,MatrixD.Transpose(α.WorldMatrix));}Vector3 Βσ(Vector3 ġ,Vector3 Ѓ,Vector3 ΑЀ){MatrixD ΑΧ=MatrixD.CreateFromDir(ġ,Ѓ);return Vector3D.TransformNormal(ΑЀ,MatrixD.Transpose(ΑΧ));}Vector3 Βς(Vector3 ġ,Vector3 Ѓ,Vector3 ύ)
{MatrixD ΑΧ=MatrixD.CreateFromDir(ġ,Ѓ);return Vector3D.Transform(ύ,ΑΧ);}String β(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ϋ(String s){String[]θ=s.Split('|');return new Vector3(float.Parse(Ι(θ,0)),float.Parse(Ι(θ,1)),float.Parse(Ι(θ,2)));}String Εο(ά ή){String Ȁ=":";
String έ=β(ή.Π)+Ȁ+β(ή.І)+Ȁ+
β(ή.Ё)+Ȁ+β(ή.О)+Ȁ+β(ή.π);for(int i=0;i<ή.Я.Length;i++)
έ+=Ȁ+Math.Round(ή.Я[i],3);return έ;}ά Εή(String s){String[]θ=s.Split(':');ά ή=new ά(
Ϋ(Ι(θ,0)),Ϋ(Ι(θ,1)),
Ϋ(Ι(θ,2)),Ϋ(Ι(θ,3)),
Ϋ(Ι(θ,4)));int Α=5;List<float>ΓΒ=new List<float>();while(Α<θ.Length){String ƃ=Ι(θ,Α);float f=0;if(!float.TryParse(ƃ,out f)) break;ΓΒ.Add(f);Α++;}ή.Я=ΓΒ.ToArray();return ή;}System.Text.StringBuilder ȯ=new System.Text.StringBuilder();void Λ<T>(T value,bool ΖΨ){if(ΖΨ) ȯ.Append('\n');ȯ.Append(value);}
void Λ<T>(T value){Λ(value,true);}String ł(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String œ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Ες,int Α){String ƃ=Ες.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ƃ)) return "";
return ƃ;}void Save(){if(Γο||Ρ==Ο.Е){Storage="";return;}Storage=ΓЋ();}String ΓЋ(){ȯ.Clear();ȯ.Append(DATAREV+";");Λ(β(Γξ),false);Λ(β(Σ.І));Λ(β(Σ.О));Λ(β(Σ.Ё));Λ(β(Σ.π));Λ(β(Σ.Π));Λ(β(Σ.Ч));Λ(Σ.ν);Λ(β(Χ.Π));Λ(β(Χ.π));Λ(β(Χ.І));Λ(β(Χ.Ё));Λ(β(Χ.О));Λ(β(Χ.Ч));Λ(Χ.ν);Λ(β(ĭ));Λ(β(Ȥ));Λ(";");
Λ((int) Ρ,false);Λ((int) φ);Λ((int) ǟ);Λ(ш);Λ(щ);Λ(Ы);Λ(ъ);Λ(ǘ);Λ(Ȉ);Λ(Ǧ);if(Ρ==Ο.ο){Λ((int) χ.П);Λ(χ.е);Λ(χ.ǲ);Λ(ł(χ.ф));Λ(ł(χ.ч));Λ((int) ψ.П);Λ(ψ.е);Λ(ψ.ǲ);Λ(ł(ψ.ф));Λ(ł(ψ.ч));}else{Λ((int) ȁ);Λ((int) ǡ);Λ((int) ĝ);Λ((int) ј);Λ((int) ȟ);Λ(Ļ);Λ(ȃ);Λ(Ĩ);Λ(Ğ);Λ(Ц);Λ(Х);Λ(д);Λ(ƈ);Λ(Ƈ);Λ(ļ);Λ(Ĥ);
Λ(к);Λ(й);Λ(Ū);Λ(ū);Λ(ī);Λ(с);Λ(Αη);Λ(ΑΨ);}Λ(";");for(int i=0;i<ǎ.Count;i++)
Λ((i>0?"|":"")+ǎ[i],false);Λ(";");for(int i=0;i<ω.Count;i++)
Λ(Εο(ω[i]),i>0);Λ(";",false);Λ(з+"|"+Ǽ,false);Λ(";",false);Λ(ΔΝ(),false);String Ξ=ȯ.ToString();ȯ.Clear();return Ξ;}п ΕΓ(){return ΕΔ(Storage);}п ΕΔ(String Ξ){if(Ξ=="") return п.ΔЃ;String[]ǿ=Ξ.Split(';');if(Ι(ǿ,0)!=DATAREV) return п.Βΰ;int Α=0;try{String[]Θ=Ι(ǿ,1).Split('\n');
Γξ=Ϋ(Ι(Θ,Α++));Σ.І=Ϋ(Ι(Θ,Α++));Σ.О=Ϋ(Ι(Θ,Α++));Σ.Ё=Ϋ(Ι(Θ,Α++));Σ.π=Ϋ(Ι(Θ,Α++));Σ.Π=Ϋ(Ι(Θ,Α++));Σ.Ч=Ϋ(Ι(Θ,Α++));Σ.ν=bool.Parse(Ι(Θ,Α++));Χ.Π=Ϋ(Ι(Θ,Α++));Χ.π=Ϋ(Ι(Θ,Α++));Χ.І=Ϋ(Ι(Θ,Α++));Χ.Ё=Ϋ(Ι(Θ,Α++));Χ.О=Ϋ(Ι(Θ,Α++));Χ.Ч=Ϋ(Ι(Θ,Α++));Χ.ν=bool.Parse(Ι(Θ,Α++));
ĭ=Ϋ(Ι(Θ,Α++));Ȥ=Ϋ(Ι(Θ,Α++));Θ=Ι(ǿ,2).Split('\n');Α=0;Ρ=(Ο) int.Parse(Ι(Θ,Α++));φ=(Τ) int.Parse(Ι(Θ,Α++));ǟ=(Τ) int.Parse(Ι(Θ,Α++));ш=int.Parse(Ι(Θ,Α++));щ=int.Parse(Ι(Θ,Α++));Ы=int.Parse(Ι(Θ,Α++));ъ=int.Parse(Ι(Θ,Α++));ǘ=float.Parse(Ι(Θ,Α++));Ȉ=bool.Parse(Ι(Θ,Α++));
Ǧ=bool.Parse(Ι(Θ,Α++));if(Ρ==Ο.ο){χ.П=(Ѕ) int.Parse(Ι(Θ,Α++));χ.е=float.Parse(Ι(Θ,Α++));χ.ǲ=float.Parse(Ι(Θ,Α++));χ.ф=œ(Ι(Θ,Α++));χ.ч=œ(Ι(Θ,Α++));ψ.П=(Ѕ) int.Parse(Ι(Θ,Α++));ψ.е=float.Parse(Ι(Θ,Α++));ψ.ǲ=float.Parse(Ι(Θ,Α++));ψ.ф=œ(Ι(Θ,Α++));ψ.ч=œ(Ι(Θ,Α++));}else{ȁ=(У) int.Parse(Ι(Θ,Α++));
ǡ=(ǒ) int.Parse(Ι(Θ,Α++));ĝ=(в) int.Parse(Ι(Θ,Α++));ј=(я) int.Parse(Ι(Θ,Α++));ȟ=(У) int.Parse(Ι(Θ,Α++));Ļ=bool.Parse(Ι(Θ,Α++));ȃ=bool.Parse(Ι(Θ,Α++));Ĩ=bool.Parse(Ι(Θ,Α++));Ğ=bool.Parse(Ι(Θ,Α++));Ц=int.Parse(Ι(Θ,Α++));Х=int.Parse(Ι(Θ,Α++));д=int.Parse(Ι(Θ,Α++));ƈ=float.Parse(Ι(Θ,Α++));
Ƈ=float.Parse(Ι(Θ,Α++));ļ=float.Parse(Ι(Θ,Α++));Ĥ=float.Parse(Ι(Θ,Α++));к=int.Parse(Ι(Θ,Α++));й=int.Parse(Ι(Θ,Α++));Ū=int.Parse(Ι(Θ,Α++));ū=int.Parse(Ι(Θ,Α++));ī=int.Parse(Ι(Θ,Α++));с=int.Parse(Ι(Θ,Α++));Αη=int.Parse(Ι(Θ,Α++));ΑΨ=float.Parse(Ι(Θ,Α++));}Θ=Ι(ǿ,3).Replace("\n","").Split('|');
ǎ=Θ.ToList();Θ=Ι(ǿ,4).Split('\n');ω.Clear();if(Θ.Count()>=1&&Θ[0]!="")
for(int i=0;i<Θ.Length;i++)
ω.Add(Εή(Ι(Θ,i)));Θ=Ι(ǿ,5).Split('|');з=Ι(Θ,0);if(!int.TryParse(Ι(Θ,1),out Ǽ)) Ǽ=-1;if(з.Length!=к*й){з="";Ǽ=-1;}ΔΜ(Ι(ǿ,6));}catch{return п.ΑЏ;}Αθ=ǟ;Ǭ();return п.Βπ;}void ΕΤ(){Ǳ.Clear();for(int i=Љ.Count-1;i>=0;i--){String Ξ=Љ[i].CustomData.ToUpper();bool ΑΜ=false;
if(Ξ=="INSTRUCTIONS"){ΑΜ=true;ΒЋ=true;}if(Ξ=="DEBUG") ΑΜ=true;if(ΑΜ){Ǳ.Add(Љ[i]);Љ.RemoveAt(i);}}Ċ(Ǳ,false,1,false);}void ΕΥ(List<IMyTerminalBlock>Βύ){ь.Clear();for(int i=0;i<Βύ.Count;i++){IMyTerminalBlock α=Βύ[i];try{String Γκ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=α.CustomName.IndexOf(Γκ);int ȳ=-1;if(Α<0||!int.TryParse(α.CustomName.Substring(Α+Γκ.Length,1),out ȳ)) continue;if(ȳ==-1) continue;ȳ--;IMyTextSurfaceProvider Γμ=(IMyTextSurfaceProvider) α;if(ȳ<Γμ.SurfaceCount&&ȳ>=0)
ь.Add(Γμ.GetSurface(ȳ));}catch{}}}String ΑΪ="";void ΔА(){String ΑΦ="";String ě="";ΑΪ=ΓЊ(false);ΑΦ+=ΑΪ;ě+=ΑΪ;ě+=Δδ();for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(ΑΦ);for(int i=0;i<ь.Count;i++) ь[i].WriteText(ΑΦ);Echo(ě);for(int i=0;i<Ǳ.Count;i++){IMyTextPanel Џ=Ǳ[i];
String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΕ());}}string ΒΕ(){String Β="";try{float Γί=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ȕ*Γί)+" / "+(int)(ȣ*Γί)+"\n";
Β+="Inst. avg/max: "+Math.Round(ȕ*100f,1)+"% / "+Math.Round(ȣ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(đ,2)+"ms / "+Math.Round(ΑΩ,2)+"ms \n";}catch{}for(int i=0;i<ż.Count;i++)
Β+=""+ż.Keys.ElementAt(i)+" = "+Math.Round(ż.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ż.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Δρ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ċ(Љ,setLCDFontAndSize,0.9f,false);Ċ(ь,setLCDFontAndSize,0.9f,true);for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(Β+ȡ.ToString());for(int i=0;i<ь.Count;i++) ь[i].WriteText(Β+ȡ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Ư(String ζ,String ΰ,String ǳ){try{if(!Ĕ) return;ζ=BROADCAST_TAG+BROADCAST_SEP+ΒΒ()+BROADCAST_SEP+ΰ+BROADCAST_SEP+ǃ+BROADCAST_SEP+ǳ+BROADCAST_SEP+ζ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ζ);}catch(Exception e){ȡ=e;}}String ΒΒ(){if(Ρ!=Ο.Е) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΒΦ(String ζ){return ζ.StartsWith(BROADCAST_TAG);}bool Є(ref String Ξ,out string ğ,Char Ȁ){int Α=Ξ.IndexOf(Ȁ);ğ="";if(Α<0) return false;ğ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΔЌ(ref String ζ,out String ǥ,out String ǳ){ǥ="";ǳ="";if(!Ĕ) return false;
String Ю="";if(!Є(ref ζ,out Ю,BROADCAST_SEP)||!ΒΦ(Ю)) return false;if(!Є(ref ζ,out ǥ,BROADCAST_SEP)) return false;if(!Є(ref ζ,out Ю,BROADCAST_SEP)||(Ю!=ΒΒ()&&(Ю!="*"&&Ρ!=Ο.Е))) return false;if(!Є(ref ζ,out Ю,BROADCAST_SEP)||(Ю!=ǃ)) return false;if(!Є(ref ζ,out ǳ,BROADCAST_SEP)) return false;
return true;}void Δω(){bool ė=true;if(ΓΓ){ΓΓ=false;if(Me.CustomData.Contains("Antenna_Name")){Ω="Update custom data";Me.CustomData="";}}String Ģ=(Ρ!=Ο.Е?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ģ)
ΒΣ(Ģ);String[]Θ=Me.CustomData.Split('\n');Ĕ=bool.Parse(ŉ(Ń(Θ,"Enable_Broadcast",ref ė)));bool ΑϏ=false;bool ΒЍ=true;if(Ĕ){if(Ρ!=Ο.Е)
Ĳ=ŉ(Ń(Θ,"Ship_Name",ref ė)).Replace(BROADCAST_SELF_ID,"");ǃ=ŉ(Ń(Θ,"Broadcast_Channel",ref ė)).ToLower();ΒЍ=false;if(є==null){є=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);є.SetMessageCallback("");}List<IMyRadioAntenna>Ȕ=new List<IMyRadioAntenna>();Н.GetBlocksOfType(Ȕ);bool ƿ=false;
for(int i=0;i<Ȕ.Count;i++)
if(Ȕ[i].EnableBroadcasting&&Ȕ[i].Enabled){ƿ=true;break;}if(Ȕ.Count==0) Ω="No Antenna found";else if(!ƿ) Ω="Antenna not ready";ΑϏ=Ȕ.Count==0||!ƿ;if(Βφ&&!ΑϏ&&Ρ!=Ο.Е) Ω="Antenna ok";}else if(Ρ==Ο.Е)
Ω="Offline - Enable in PB custom data";Βφ=ΑϏ;if(ΒЍ){if(є!=null) this.IGC.DisableBroadcastListener(є);є=null;}}catch{ė=false;}if(!ė){Ω="Reset custom data";ΒΣ(Ģ);}}void ΒΣ(String Ģ){String Ξ=Ģ+"\n\n"+"Enable_Broadcast="+(Ρ==Ο.Е?"true":"false")+" \n";Ξ+=Ρ!=Ο.Е?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ŉ(String Ǹ){int Α=Ǹ.IndexOf("//");if(Α!=-1) Ǹ=Ǹ.Substring(0,Α);String[]θ=Ǹ.Split('=');if(θ.Length<=1) return "";return θ[1].Trim();}String Ń(String[]Θ,String И,ref bool ȧ){foreach(String Ǹ in Θ)
if(Ǹ.StartsWith(И)) return Ǹ;ȧ=false;return "";}void ΔΗ(string А){if(Ġ) ε=new List<ί>();String ǥ="";String ǳ="";if(!ΒΦ(А)) ΔБ(А);bool Γχ=false;int ΖΤ=50;while(є!=null&&є.HasPendingMessage&&ΖΤ-->0){MyIGCMessage ζ=є.AcceptMessage();String Ξ=ζ.Data as String;if(Ξ==null) continue;
if(ΔЌ(ref Ξ,out ǥ,out ǳ)&&ǥ!=""&&ǥ!=BROADCAST_SELF_ID){ί Γ=ƕ(ǥ,"");if(Γ==null){Γ=new ί(ǥ);ε.Add(Γ);}Γ.Ǖ(ǳ,false,false,true);Ει(Γ,Ξ);Γχ=true;}}if(Γχ)
ε.Sort(delegate(ί a,ί b){if(a.η==null&&b.η==null) return 0;else if(a.η==null) return-1;else if(b.η==null) return 1;else return a.η.CompareTo(b.η);});if(ĩ||Ġ){if(ž<=0||Ġ){Ω="";Ġ=false;ΕΙ();Δω();ΓΩ=DateTime.Now;}}if(ǝ) Εη();if(ǝ) Εθ();if(ǝ) Ελ();if(ǝ) Εκ();if(ǝ&&Αφ==0) Εζ();
Εε();}void Ει(ί Γ,String Ξ){Γ.ΑΟ=DateTime.Now;String ğ="";Є(ref Ξ,out Γ.ƅ,BROADCAST_SEP);Є(ref Ξ,out Γ.η,BROADCAST_SEP);if(Γ.ƅ!=VERSION) return;String į="";String[]Ȫ=new string[3];Є(ref Ξ,out į,BROADCAST_SEP);Є(ref Ξ,out Ȫ[0],BROADCAST_SEP);Є(ref Ξ,out Ȫ[1],BROADCAST_SEP);Є(ref Ξ,out Ȫ[2],BROADCAST_SEP);
Є(ref Ξ,out Γ.Ασ,BROADCAST_SEP);Є(ref Ξ,out Γ.Ω,BROADCAST_SEP);Є(ref Ξ,out ğ,BROADCAST_SEP);Є(ref Ξ,out Γ.Ŷ,BROADCAST_SEP);String ΓЁ="",Γα="";Є(ref Ξ,out ΓЁ,BROADCAST_SEP);Є(ref Ξ,out Γα,BROADCAST_SEP);Γ.њ.Clear();while(true){String ΓΥ;if(!Є(ref Ξ,out ΓΥ,BROADCAST_SEP)) break;String[]ů=ΓΥ.Split('/');
if(ů.Count()<3) continue;int Ј=0;if(!int.TryParse(ů[2],out Ј)) continue;Γ.њ.Add(new Ǫ(ů[0],ů[1],Ј,Ņ.Δφ));}int Γγ=0;if(!int.TryParse(ğ,out Γ.ς)) Γ.ς=0;if(!int.TryParse(į,out Γγ)) Γ.Ρ=Ο.Ǘ;if(!float.TryParse(Ȫ[0],out Γ.Π.X)) Γ.Π.X=0;if(!float.TryParse(Ȫ[1],out Γ.Π.Y)) Γ.Π.Y=0;
if(!float.TryParse(Ȫ[2],out Γ.Π.Z)) Γ.Π.Z=0;if(!float.TryParse(ΓЁ,out Γ.ǯ)) Γ.ǯ=0;if(!float.TryParse(Γα,out Γ.ǹ)) Γ.ǹ=0;Γ.Ρ=(Ο) Γγ;Γ.ǉ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Π));Γ.Αϋ=0;for(int i=0;i<Γ.њ.Count();i++) Γ.Αϋ+=Γ.њ[i].Ј;}ί ƕ(String Ν,String η){Ν=Ν.ToUpper();η=η.ToUpper();
for(int i=0;i<ε.Count;i++){if(Ν!=""&&ε[i].Ν.ToUpper()==Ν) return ε[i];if(η!=""&&ε[i].η!=null&&ε[i].η.ToUpper()==η) return ε[i];}return null;}String ȗ="";ί ΒЀ=null;DateTime ΒЁ=DateTime.MinValue;List<ί>і=new List<ί>();const float ΓЏ=20f;bool Βλ(ί ΰ,String μ){return μ==ȗ&&ΰ==ΒЀ&&((DateTime.Now-ΒЁ).TotalSeconds<ΓЏ||
(μ=="HOMEPOS"&&(Щ.Count>0||х!=null)));}String Δά(ί ΰ){if(ȗ==""||!Βλ(ΰ,ȗ)) return "";String s="";int ȧ=0,Αμ=0;for(int i=0;i<і.Count;i++){ι ƀ=і[i].Ǖ(ȗ,false,false,false);if(ƀ==ι.ĉ) ȧ++;bool Ζδ=Щ.Contains(і[i]);if(Αμ++<8) s+="  "+і[i].η+": "+(Ζδ?"waiting to leave ("+(Щ.IndexOf(і[i])+1)+")":ƀ==ι.ĉ?"received":
ƀ==ι.Ȑ?"pending...":ƀ==ι.ň?"no answer!":"-")+"\n";}if(Αμ>8) s+="  +"+(Αμ-8)+" more\n";return "--------------------------------------------\n"+ȗ+": "+ȧ+"/"+і.Count+" received\n"+s;}void Т(ί Γ,String μ){ȗ=μ;ΒЀ=Γ;ΒЁ=DateTime.Now;і.Clear();if(Γ==null) і.AddRange(ε);else і.Add(Γ);
if(Γ==null){Щ.Clear();х=null;}else Щ.Remove(Γ);if(Γ==null&&μ=="HOMEPOS"){ΕΩ();return;}if(Γ==null){for(int i=0;i<ε.Count;i++)
ε[i].Ǖ(μ,true,false,false);Ư(μ,"*",μ);}else{Γ.Ǖ(μ,true,false,false);Ư(μ,Γ.Ν,μ);}}void ΔБ(string А){if(А=="") return;var θ=А.ToUpper().Split(' ');θ.DefaultIfEmpty("");var μ=θ.ElementAtOrDefault(0);var Βψ=θ.ElementAtOrDefault(1);String ΖΖ="Invalid argument: "+А;if(γ==Κ.ċ){if(σ!=null)
{switch(μ){case "UP":{if(ς<2) break;if(σ.ς==0){ς=1;return;}Т(σ,"UP");return;}case "DOWN":{if(ς<1) break;if(ς==1){Т(σ,"MRES");break;}Т(σ,"DOWN");break;}case "APPLY":{if(ς<2) break;Т(σ,"APPLY");return;}}}}switch(μ){case "UP":this.ΔЁ(false);return;case "DOWN":this.ΔЀ(false);return;
case "APPLY":this.ȋ(true);return;}switch(μ){case "CLEAR":ΔΔ();return;case "SENDALL":
if(String.IsNullOrEmpty(Βψ)){Ω="Usage: SENDALL <cmd>";return;}Т(null,Βψ);return;case "SEND":
if(А.Length<="SEND".Length+1){Ω="Usage: SEND <ship>:<cmd>";return;}ΕΛ(А.Remove(0,"SEND".Length+1));return;}Ω=ΖΖ;}void ΕΛ(String Βχ){if(Βχ=="") return;var θ=Βχ.Split(':');if(θ.Length!=2){Ω="Missing separator \":\"";return;}θ.DefaultIfEmpty("");String Ĳ=θ.ElementAtOrDefault(0).Trim();
ί Γ=ƕ("",Ĳ);if(Γ!=null) Т(Γ,θ.ElementAtOrDefault(1).Trim());else Ω="Unknown ship: "+Ĳ;}void ΔΔ(){ε.Clear();σ=null;Ψ(Κ.Main);ς=0;}void ΕΙ(){List<IMyTerminalBlock>Γω=new List<IMyTerminalBlock>();Н.GetBlocksOfType(Љ,ơ);Н.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Γω,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Љ=ΑА(Љ,pamTag,true);ΕΥ(Γω);ΕΤ();ΕΚ();Ċ(Љ,setLCDFontAndSize,1.15f,false);Ċ(ь,setLCDFontAndSize,1.15f,true);String ΖΒ=ΒΖ(false);String Γζ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Џ in Љ){if(Џ.CustomData==""){Џ.CustomData=ΒΖ(true);continue;}
if(!Џ.CustomData.Contains(ΖΒ))
if(!Џ.CustomData.Contains(Γζ)) Џ.CustomData=Γζ+Џ.CustomData;}}void Εε(){String ě="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ь.Count;i++)
ь[i].WriteText(ΑЈ(ϊ.ΒΨ,"0",false));for(int i=0;i<Љ.Count;i++){ϊ λ=ϊ.Ǘ;String Э="";ΔЍ(Љ[i],out λ,out Э);Љ[i].WriteText(ΑЈ(λ,Э,i==0));}Echo(ě+Δί());for(int i=0;i<Ǳ.Count;i++){IMyTextPanel Џ=Ǳ[i];String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");
if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΕ());}}String ȋ(bool Ε){int ΕЄ=0;return ȋ(Ε,0,ref ΕЄ,false,1);}String ȋ(bool Ε,int Ζά,ref int Ζλ,bool ΒЄ,int İ){String Β="";String ȱ="——————————————————\n";String Ζψ="--------------------------------------------\n";if(σ==null||γ==Κ.Main||γ==Κ.ư)
Β+="[PAM]-Controller | "+ε.Count+" Connected ships"+"\n"+Δα()+Δη()+Δν();else Β+=ΒΙ(σ)+"\n";Β+=ȱ;int Α=0;int Δ=ς;Ǻ=0;if(Ω!=""){Ζ(ref Β,Δ,Α++,Ε,Ω);}else if(γ==Κ.Main){if(σ==null&&ε.Count>=1) σ=ε[0];String Ї="";if(Ζ(ref Ї,Δ,Α++,Ε," Send command to all")) Ψ(Κ.ư);
int Ȱ=0;for(int s=0;s<ε.Count;s++){if(ΒЄ) Ї+="\n";ί Γ=ε[s];if(Δ==Α||Δ==Α+1) σ=Γ;if(Α==Δ) Ȱ=Ї.Split('\n').Length-1;if(Ζ(ref Ї,Δ,Α++,Ε," "+ΒΙ(Γ))){σ=Γ;Ψ(Κ.ċ);σ.Ǖ("",false,true,false);}if(Α==Δ) Ȱ=Ї.Split('\n').Length-1;if(Ζ(ref Ї,Δ,Α++,Ε," "+(ΒЄ?"":"  ")+Δι(Γ)))
{σ=Γ;Ψ(Κ.Βκ);}}int ă=İ-2;ă+=Math.Max(0,(Ζά-1))*İ;Β+=Ʈ(ă,Ї,Ȱ,ref Ζλ);}else if(γ==Κ.Ŋ||γ==Κ.ư){Κ Εσ=γ==Κ.Ŋ?Κ.ċ:Κ.Main;ί ΰ=null;if(γ==Κ.Ŋ) ΰ=σ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Ψ(Εσ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ȍ(ΰ,"STOP"))){Т(ΰ,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ȍ(ΰ,"CONT"))){Т(ΰ,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ȍ(ΰ,"HOMEPOS"))){Т(ΰ,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ȍ(ΰ,"JOBPOS"))){Т(ΰ,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ȍ(ΰ,"FULL"))){Т(ΰ,"FULL");}if(ΰ!=null&&ΰ.Ρ==Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ȍ(ΰ,"UNDOCK"))){Т(ΰ,"UNDOCK");}Β+=Δά(ΰ);}else if(γ==Κ.ċ){String ǁ="";if(Ζ(ref ǁ,Δ,Α++,Ε," Back")){Ψ(Κ.Main);}Β+=ǁ.Substring(0,ǁ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Ψ(Κ.Ŋ);}if(!σ.Ƣ()&&!σ.Ɵ()) Ǻ++;if(σ.Ɵ()) Β+="No answer received...";
else Β+=Ȓ(σ,σ.Ŷ);}else if(γ==Κ.Βκ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Ψ(Κ.Main);}Β+=Ζψ;Β+=Ȓ(σ,ΒΘ(σ));}if(!ΑΫ) ς=Math.Min(Ǻ-1,ς);return Β;}String ΒΙ(ί Γ){if(Γ.ƅ!=VERSION) return Γ.η+": Different version!";return Γ.η+": "+Ȓ(Γ,Γ.Ασ)+" "+Γ.ǉ+"m";}String Δι(ί Γ){String s=Ƭ("",С(Γ.ǯ,Γ.ǹ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.њ.Count;i++){if(i>=5) break;Ǫ ц=Γ.њ[i];s+=Βτ(Ǝ(Δε(ц)),3)+" "+ΑБ(ц.Ј)+" ";}return s;}String Δε(Ǫ ц){if(ц.Ќ.ToUpper()=="ORE"||ц.Ќ.ToUpper()=="INGOT"){String ΒЂ=Δγ(ц.η.ToUpper());if(ΒЂ!="") return ΒЂ;}return ц.η;}String Ȓ(ί Γ,String Β){if(Γ.Ƣ()) return "No signal...("+ƙ((int)(DateTime.Now-Γ.ΑΟ).TotalSeconds)+")";
return Β;}String ΒΖ(bool ΖЈ){String Β="";if(ΖЈ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΔЍ(IMyTerminalBlock Џ,out ϊ λ,out String Э){bool ȧ=true;String į=ŉ(Ń(Џ.CustomData.Split('\n'),"mode",ref ȧ)).ToUpper();String[]θ=į.Split(':');String Į=θ.Length>0?θ[0].Trim():"";Э=θ.Length>1?θ[1].Trim():"";
λ=ϊ.Ǘ;if(Į=="MAIN") λ=ϊ.ΒΩ;else if(Į=="MAINX") λ=ϊ.ΒΪ;else if(Į=="MENU") λ=ϊ.ƥ;else if(Į=="INVENTORY") λ=ϊ.ƞ;else if(Į=="DEBUG") λ=ϊ.Ƒ;if(λ==ϊ.Ǘ) Э=į;}String ΑЈ(ϊ λ,String Э,bool ΕЇ){int Ĭ=15;if(ΕЇ){ΒЅ=Ŝ;Ŝ=0;ΒІ=ŝ;ŝ=0;}String Β="";if(λ==ϊ.ΒΩ){int Ў=0;
Β=ȋ(false,ΒЅ,ref Εώ,true,Ĭ);if(!int.TryParse(Э,out Ў)) return Β;Ŝ=Math.Max(Ў,Ŝ);Ў--;return Βε(Ў,Ĭ,Β);}else if(λ==ϊ.ΒΪ){int Ў=0;Β=ȋ(false,ΒІ,ref ΕϏ,false,Ĭ);if(!int.TryParse(Э,out Ў)) return Β;ŝ=Math.Max(Ў,ŝ);Ў--;return Βε(Ў,Ĭ,Β);}if(λ==ϊ.ΒΨ){return ȋ(false,0,ref ΕЀ,true,Ĭ);
}else if(λ==ϊ.ƞ||λ==ϊ.ƥ||λ==ϊ.Ƒ){ί Γ=σ;if(Э!=""){Γ=ƕ("",Э);if(Γ==null) return "Unknown ship: "+Э;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ȱ="—————————————————————\n";String Αω="";String ǂ="";if(λ==ϊ.ƥ){Αω="Menu";ǂ=Ȓ(Γ,Γ.Ŷ);}
else if(λ==ϊ.ƞ){Αω="Inventory";ǂ=Ȓ(Γ,ΒΘ(Γ));}else if(λ==ϊ.Ƒ){return Ȓ(Γ,Δκ(Γ));}Β+=Γ.η+" - "+Γ.ǉ+"m | "+Ǝ(""+Αω)+"\n"+ȱ;Β+=ǂ;return Β;}return "Unknown command: "+Э;}string Δκ(ί Γ){String s="";s+=Γ.ƅ+"\n";s+=Γ.η+"\n";s+=Γ.Π.X+"\n";s+=Γ.Π.Y+"\n";
s+=Γ.Π.Z+"\n";s+=Γ.Ασ+"\n";s+=Γ.Ω+"\n";s+=Γ.ς+"\n";s+=Γ.Ŷ.Length+"\n";s+=Γ.ǯ+"\n";s+=Γ.ǹ+"\n";s+=Γ.њ.Count()+"\n";return s;}string ΒΘ(ί Γ){String s="";s+=Ƭ("All",С(Γ.ǯ,Γ.ǹ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.њ.Count();i++){Ǫ ц=Γ.њ[i];
s+=Ƭ(ц.η,ц.Ј,Γ.Αϋ,30,8,10)+"\n";}return s;}ι Δέ(ί Γ,bool ѕ,string μ){ι έ=ι.None;if(Γ==null){for(int i=0;i<ε.Count;i++){ι Ѐ=ε[i].Ǖ(μ,false,ѕ,false);if(Ѐ==ι.ĉ) έ=Ѐ;if(Ѐ==ι.ň) return Ѐ;if(Ѐ==ι.Ȑ) return Ѐ;}return έ;}else return Γ.Ǖ(μ,false,ѕ,false);}String ȍ(ί Γ,String μ)
{if(!Βλ(Γ,μ)) return "";ι Ѐ=Δέ(Γ,false,μ);if(Ѐ==ι.ĉ) return "received!";if(Ѐ==ι.Ȑ) return "pending...";if(Ѐ==ι.ň) return "no answer!";return "";}String Ʈ(int ă,String Β,int Ȱ,ref int ѝ){String[]Θ=Β.Split('\n');if(Ȱ>=ѝ+ă-1) ѝ++;ѝ=Math.Min(Θ.Count()-1-ă,ѝ);if(Ȱ<ѝ+1) ѝ--;
ѝ=Math.Max(0,ѝ);String έ="";for(int i=0;i<ă;i++){int Γά=i+ѝ;if(Γά>=Θ.Count()) break;έ+=Θ[Γά]+"\n";}return έ;}String Βε(int Ў,int İ,String Β){String[]Θ=Β.Split('\n');int Ας=Ў*İ;int Ũ=(Ў+1)*İ;String έ="";for(int i=Ας;i<Ũ;i++){if(i>=Θ.Count()) break;έ+=Θ[i]+"\n";
}return έ;}String Ǝ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Βτ(String s,int Γΰ){if(s=="") return s;if(s.Length>Γΰ) s=s.Substring(0,Γΰ-1)+".";return s;}string Ƭ(String Ƿ,float value,float Ų,int Βω,int ΓΨ,int ΖΫ){float Ǿ=С(value,Ų)*Βω;
String ŗ="[";for(int i=0;i<Βω;i++){if(i<=Ǿ) ŗ+="|";else ŗ+="'";}ŗ+="]";return ŗ+" "+Βτ(Ǝ(Ƿ),ΓΨ).PadRight(ΓΨ)+"".PadRight(ΖΫ)+ΑБ(value);}String ΑБ(float Ј){if(Ј>=1000000) return Math.Round(Ј/1000000f,Ј/1000000f<100?1:0)+"M";if(Ј>=1000) return Math.Round(Ј/1000f,Ј/1000f<100?1:0)+"K";
return ""+Math.Round(Ј);}String ƙ(int ı){if(ı>=60*60) return Math.Round(ı/(60f*60f),1)+" h";if(ı>=60) return Math.Round(ı/60f,1)+" min";return ""+ı+" s";}List<IMyCameraBlock>Εψ=new List<IMyCameraBlock>();bool Ζέ=false;int Εϊ=0,ΗΓ=0,ΗΑ=0;String Εφ="";String Εχ="";
void ΔΓ(){Ζέ=false;Εϊ=0;Εφ="";}const String Εβ="[PAMTFC]";long ΖΥ=0;String з="";int Ǽ=-1;List<int>ΑΙ=new List<int>();String ΓΜ="";void ΔΣ(){String И=к+"|"+й+"|"+(int) ȟ;if(И==ΓΜ) return;ΓΜ=И;ΑЉ(к,й,ȟ,ΑΙ);}void ΑЉ(int w,int h,У Ας,List<int>ǅ){ǅ.Clear();if(Ας==У.ő)
{for(int k=0;k<w*h;k++){int Ǥ=k/w;ǅ.Add(Ǥ*w+(Ǥ%2==0?k-Ǥ*w:w-1-(k-Ǥ*w)));}return;}int ΓΚ=(int) Math.Ceiling(w/2f),ΓΙ=(int) Math.Ceiling(h/2f);int ΕБ=w/2,ΕА=h/2;int x=0,y=0,Ǌ=0,ǋ=-1,Ų=Math.Max(w,h);for(int i=0;i<Ų*Ų;i++){if(-ΓΚ<x&&x<=ΕБ&&-ΓΙ<y&&y<=ΕА) ǅ.Add((y-1+ΓΙ)*w+(x-1+ΓΚ));
if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ǌ;Ǌ=-ǋ;ǋ=t;}x+=Ǌ;y+=ǋ;}}bool ΔΡ(){int n=Math.Max(0,к*й);if(з.Length==n) return false;з=new String('0',n);Ǽ=-1;return true;}void ΕΡ(int Ж,bool κ){if(Ж<0||Ж>=з.Length) return;char[]h=з.ToCharArray();
h[Ж]=κ?'1':'0';з=new String(h);}const String Δώ="[PAMJOB]";const String Εΰ="[PAMTCC]";class ȑ{public long Ν;public Vector3 Φ,М,Ѓ;}List<ȑ>В=new List<ȑ>();bool ę=false;Vector3 ť,Ť,Ŧ,ŧ;long Ħ=0;long Γι=0;ά Γθ=null;bool ΕЂ=false;ά ΔΞ(Vector3 Φ,Vector3 М,Vector3 Ѓ){MatrixD m=MatrixD.CreateWorld(Φ,М,Ѓ);
ά w=new ά(Σ);w.І=Vector3D.TransformNormal(ť,m);w.Ё=Vector3D.TransformNormal(Ť,m);w.О=Vector3D.TransformNormal(Ŧ,m);w.Π=Φ+(Vector3) Vector3D.TransformNormal(ŧ,m);w.ν=true;return w;}void ΔϏ(){if(Ρ==Ο.ο||!Σ.ν) return;IMyShipConnector ї=Ȍ(MyShipConnectorStatus.Connected);if(ї==null||ї.OtherConnector==null) return;
IMyShipConnector Й=ї.OtherConnector;Vector3I ΖΓ=new Vector3I((int) Σ.Ч.X,(int) Σ.Ч.Y,(int) Σ.Ч.Z);if(ї.Position==ΖΓ){MatrixD Ů=MatrixD.Transpose(Й.WorldMatrix);ť=Vector3D.TransformNormal(Њ.WorldMatrix.Forward,Ů);Ť=Vector3D.TransformNormal(Њ.WorldMatrix.Down,Ů);Ŧ=Vector3D.TransformNormal(Њ.WorldMatrix.Left,Ů);
ŧ=Vector3D.TransformNormal(ї.GetPosition()-Й.GetPosition(),Ů);ę=true;}if(Vector3.Distance(ї.GetPosition(),Σ.Π)<5) Ħ=Й.EntityId;List<IMyShipConnector>Ǵ=new List<IMyShipConnector>();Н.GetBlocksOfType(Ǵ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ǵ.Count>0){В.Clear();for(int i=0;i<Ǵ.Count;i++){ȑ p=new ȑ();p.Ν=Ǵ[i].EntityId;p.Φ=Ǵ[i].GetPosition();p.М=Ǵ[i].WorldMatrix.Forward;p.Ѓ=Ǵ[i].WorldMatrix.Up;В.Add(p);}}Γθ=null;Γι=0;if(Й.EntityId==Ħ||!ę) return;for(int i=0;i<В.Count;i++)
if(В[i].Ν==Й.EntityId){Γι=В[i].Ν;Γθ=ΔΞ(В[i].Φ,В[i].М,В[i].Ѓ);}}String ΔΝ(){String s=(ę?"1":"0")+":"+β(ť)+":"+β(Ť)+":"+
β(Ŧ)+":"+β(ŧ)+":"+Ħ;for(int i=0;i<В.Count;i++)
s+=":"+В[i].Ν+","+β(В[i].Φ)+","+β(В[i].М)+","+β(В[i].Ѓ);return s;}void ΔΜ(String s){В.Clear();ę=false;Ħ=0;String[]f=s.Split(':');if(f.Length<6) return;ę=f[0]=="1";ť=Ϋ(f[1]);Ť=Ϋ(f[2]);Ŧ=Ϋ(f[3]);ŧ=Ϋ(f[4]);long.TryParse(f[5],out Ħ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ȑ p=new ȑ();if(!long.TryParse(e[0],out p.Ν)) continue;p.Φ=Ϋ(e[1]);p.М=Ϋ(e[2]);p.Ѓ=Ϋ(e[3]);В.Add(p);}}const String Ɛ="[PAMDCK]";const float ΓЎ=120f;class ѐ{public long Γ;public int Ǟ;public long и;public bool Βώ;public DateTime Л,Ƞ;
}IMyBroadcastListener ţ=null;List<ѐ>ό=new List<ѐ>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMySensorBlock>б=new List<IMySensorBlock>();List<IMyCameraBlock>ћ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ɖ=new List<MyDetectedEntityInfo>();long Б=0;int Ś=0;
long ř=0;DateTime ΒϏ=DateTime.Now;String љ="";int ΒА=0;IMyCubeGrid џ=null;void ΕΚ(){џ=Me.CubeGrid;Н.GetBlocksOfType(ў,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(dockPoolTag));Н.GetBlocksOfType(б,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(zoneTag));Н.GetBlocksOfType(ћ,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(laneTag));
for(int i=0;i<б.Count;i++){б[i].DetectSmallShips=true;б[i].DetectLargeShips=true;}for(int i=0;i<ћ.Count;i++) ћ[i].EnableRaycast=true;}IMyShipConnector Ƶ(long Ν){if(Ν==0) return null;IMyShipConnector c=Н.GetBlockWithId(Ν) as IMyShipConnector;return c!=null&&џ!=null&&c.CubeGrid.IsSameConstructAs(џ)?c:null;
}bool Ơ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΒΧ(IMyShipConnector c,long Γ){for(int i=0;i<ћ.Count;i++){IMyCameraBlock Ė=ћ[i];if(!Ė.IsWorking||Vector3D.Distance(Ė.GetPosition(),c.GetPosition())>10) continue;
Vector3D Ũ=Ė.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ė.CanScan(Ũ)) continue;MyDetectedEntityInfo ũ=Ė.Raycast(Ũ);if(!ũ.IsEmpty()&&ũ.EntityId!=џ.EntityId&&ũ.EntityId!=Γ&&ũ.Type!=MyDetectedEntityType.FloatingObject){љ="Lane blocked: "+c.CustomName;return false;}
}return true;}bool Δό(){for(int s=0;s<б.Count;s++){if(!б[s].IsWorking) continue;Ɖ.Clear();б[s].DetectedEntities(Ɖ);for(int i=0;i<Ɖ.Count;i++){MyDetectedEntityInfo e=Ɖ[i];bool ΕЍ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;if(ΕЍ&&e.EntityId!=Б&&e.Velocity.Length()>0.5f)
{љ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΔЎ(ѐ r){IMyShipConnector и=Ƶ(r.и);if(Ơ(и)&&ΒΧ(и,r.Γ)) return и.EntityId;if(!r.Βώ||и==null) return 0;IMyShipConnector Ѝ=null;double Βό=0;for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];if(!Ơ(c)) continue;
double d=Vector3D.Distance(c.GetPosition(),и.GetPosition());if(Ѝ==null||d<Βό){if(ΒΧ(c,r.Γ)){Ѝ=c;Βό=d;}}}return Ѝ==null?0:Ѝ.EntityId;}void Δξ(){ѐ Ȩ=null;long ї=0;for(int Ż=2;Ż>=1&&Ȩ==null;Ż--){ѐ ǽ=null;for(int i=0;i<ό.Count;i++){ѐ r=ό[i];if(r.Ǟ!=Ż) continue;
if(Ż==1){long c=ΔЎ(r);if(c==0) continue;if(ǽ==null||r.Л<ǽ.Л){ǽ=r;ї=c;}}else if(ǽ==null||r.Л<ǽ.Л) ǽ=r;}Ȩ=ǽ;}if(Ȩ==null){if(ό.Count>0&&љ=="") љ="No free dock";return;}Б=Ȩ.Γ;Ś=Ȩ.Ǟ;ř=Ȩ.Ǟ==1?ї:0;ΒϏ=DateTime.Now;}void Εη(){if(џ==null) return;
if(ţ==null) ţ=IGC.RegisterBroadcastListener(Ɛ);int ĕ=30;while(ţ.HasPendingMessage&&ĕ-->0){String Ξ=ţ.AcceptMessage().Data as String;if(Ξ!=null) Δπ(Ξ);}ΕΗ();if(++ΒА<3) return;ΒА=0;IGC.SendBroadcastMessage(Ɛ,ΓЌ());}void Δπ(String Ξ){try{String[]f=Ξ.Split(';');long Γ=long.Parse(f[1]);
if(f[0]=="D"){for(int i=ό.Count-1;i>=0;i--) if(ό[i].Γ==Γ) ό.RemoveAt(i);if(Б==Γ) Б=0;}else if(f[0]=="R"&&f.Length>=5){long и=long.Parse(f[3]);if(Ƶ(и)==null) return;ѐ r=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Γ) r=ό[i];if(r==null){r=new ѐ();r.Γ=Γ;r.Л=DateTime.Now;ό.Add(r);}
int Ǟ=int.Parse(f[2]);if(r.Ǟ!=Ǟ) r.Л=DateTime.Now;r.Ǟ=Ǟ;r.и=и;r.Βώ=f[4]=="1";r.Ƞ=DateTime.Now;}}catch{}}void ΕΗ(){for(int i=ό.Count-1;i>=0;i--)
if((DateTime.Now-ό[i].Ƞ).TotalSeconds>3) ό.RemoveAt(i);ѐ Ř=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Б) Ř=ό[i];if(Б!=0&&Ř==null) Б=0;if(Ř!=null&&(DateTime.Now-ΒϏ).TotalSeconds>ΓЎ){Б=0;Ř.Л=DateTime.Now;}љ="";if(Б==0&&ό.Count>0&&!Δό()) Δξ();}String ΓЌ()
{String ζ="S;"+џ.EntityId+";"+Б+";"+Ś+";"+ř+";";for(int i=0;i<ό.Count;i++) ζ+=(i>0?",":"")+ό[i].Γ;ζ+=";";for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];ζ+=(i>0?":":"")+c.EntityId+","+β(c.GetPosition())+","+β(c.WorldMatrix.Forward)+","+β(c.WorldMatrix.Up);
}ζ+=";";for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ζ+=(i>0?":":"")+β(z.GetPosition())+","+Math.Round(r,1);}return ζ;}String Δα()
{if(ў.Count==0&&б.Count==0) return "";int ΓΔ=0;for(int i=0;i<ў.Count;i++) if(Ơ(ў[i])) ΓΔ++;return "Docks: "+ΓΔ+"/"+ў.Count+" free"+(Б!=0?(Ś==1?" | docking":" | undocking"):"")+
(ό.Count>1?" | "+(ό.Count-1)+" waiting":"")+(љ!=""?"\n"+љ:"")+"\n";}int ΒЏ=0;bool ΖЂ=false;void ΕΑ(){if(ΒЏ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ɛ,"D;"+ΖΥ);ΒЏ=0;ΖЂ=false;}const float Δύ=600f;class р{public long Ν;public int Ж=-1;public bool ΓЂ;public float ΑΓ,ΑΒ;
public DateTime Αι;}class г{public String И;public int w,h;public float Αό,Αχ;public String κ;public List<int>ź=new List<int>();public List<р>Ћ=new List<р>();}List<г>З=new List<г>();IMyBroadcastListener ű=null;int ΓΧ=0;void Εθ(){if(ű==null) ű=IGC.RegisterBroadcastListener(Εβ);int ĕ=50;
while(ű.HasPendingMessage&&ĕ-->0){String Ξ=ű.AcceptMessage().Data as String;if(Ξ==null) continue;try{Δς(Ξ);}catch{}try{Δσ(Ξ);}catch{}}for(int j=З.Count-1;j>=0;j--){г Η=З[j];for(int i=Η.Ћ.Count-1;i>=0;i--)
if((DateTime.Now-Η.Ћ[i].Αι).TotalSeconds>Δύ) Η.Ћ.RemoveAt(i);if(Η.Ћ.Count==0){З.RemoveAt(j);continue;}ΓЇ(Η);}if(++ΓΧ<3) return;ΓΧ=0;for(int j=0;j<З.Count;j++) IGC.SendBroadcastMessage(Δώ,ΓЉ(З[j]));}void ΓЇ(г Η){for(int i=0;i<Η.Ћ.Count;i++){р s=Η.Ћ[i];if(s.ΓЂ&&(s.Ж<0||Η.κ[s.Ж]=='1')) s.Ж=ΔЂ(Η,s);
}}String ΓЉ(г Η){String ζ="J;"+Η.И+";"+Η.κ+";";bool Л=true;for(int i=0;i<Η.Ћ.Count;i++){if((DateTime.Now-Η.Ћ[i].Αι).TotalSeconds>5) continue;ζ+=(Л?"":",")+Η.Ћ[i].Ν+":"+Η.Ћ[i].Ж;Л=false;}return ζ;}void Δς(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;г Η=null;for(int i=0;i<З.Count;i++) if(З[i].И==f[4]) Η=З[i];if(Η==null){Η=new г();Η.И=f[4];Η.w=int.Parse(k[3]);Η.h=int.Parse(k[4]);Η.Αό=float.Parse(k[6]);Η.Αχ=float.Parse(k[7]);if(Η.w<1||Η.h<1) return;ΑЉ(Η.w,Η.h,(У) int.Parse(k[5]),Η.ź);
Η.κ=new String('0',Η.w*Η.h);З.Add(Η);}String d=f[6];if(d.Length==Η.κ.Length){char[]c=Η.κ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Η.κ=new String(c);}long Ν=long.Parse(f[0]);р s=null;for(int i=0;i<Η.Ћ.Count;i++) if(Η.Ћ[i].Ν==Ν) s=Η.Ћ[i];
if(s==null){s=new р();s.Ν=Ν;Η.Ћ.Add(s);}s.Αι=DateTime.Now;String[]w=f[10].Split(',');s.ΓЂ=w[0]=="1";if(w.Length>=3){s.ΑΓ=float.Parse(w[1]);s.ΑΒ=float.Parse(w[2]);}int ĥ=int.Parse(f[5]);if(ĥ>=0&&ĥ<Η.κ.Length&&(s.Ж<0||Η.κ[s.Ж]=='1')&&Η.κ[ĥ]!='1'&&Ɯ(Η,ĥ,s)==null)
s.Ж=ĥ;}р Ɯ(г Η,int Ж,р ΕІ){for(int i=0;i<Η.Ћ.Count;i++){р o=Η.Ћ[i];if(o!=ΕІ&&o.Ж==Ж)
return o;}return null;}int ΔЂ(г Η,р s){for(int k=0;k<Η.ź.Count;k++){int c=Η.ź[k];if(Η.κ[c]!='1'&&Ɯ(Η,c,s)==null) return c;}return-2;}String Δη(){String s="";for(int j=0;j<З.Count;j++){г Η=З[j];int κ=0;for(int i=0;i<Η.κ.Length;i++) if(Η.κ[i]=='1') κ++;s+="Job "+(j+1)+": "+κ+"/"+Η.κ.Length+" holes, "+Η.Ћ.Count+" ships\n";
}return s;}const float Εί=8f;class ϋ{public long Ν;public Vector3 Φ,ю,ģ;public float ș,ΑΓ,ΑΒ,л=-1;public int Ƕ;public int Ǣ;public String Η="";public int Αΰ;public DateTime Ȃ;}class Ő{public long a,b,ΑЅ;public DateTime Ƞ;}List<ϋ>Ϊ=new List<ϋ>();List<Ő>Г=new List<Ő>();int Αφ=0;void Δσ(String Ξ)
{String[]f=Ξ.Split(';');if(f.Length<9) return;long Ν=long.Parse(f[0]);ϋ s=null;for(int i=0;i<Ϊ.Count;i++) if(Ϊ[i].Ν==Ν) s=Ϊ[i];if(s==null){s=new ϋ();s.Ν=Ν;Ϊ.Add(s);}s.Φ=Ϋ(f[1]);s.ю=Ϋ(f[2]);s.ș=float.Parse(f[3]);s.Η=f[4];s.Αΰ=int.Parse(f[7].Split(',')[1]);s.ģ=Ϋ(f[8]);
s.Ƕ=0;s.Ǣ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΓ=float.Parse(w[1]);s.ΑΒ=float.Parse(w[2]);s.Ƕ=int.Parse(w[3]);if(w.Length>=5) s.Ǣ=int.Parse(w[4]);}}s.Ȃ=DateTime.Now;}bool Č(ϋ s){return s.ю.LengthSquared()<0.25f&&(s.Αΰ==3||s.ģ.LengthSquared()<0.25f);}
Vector3 Βρ(ϋ s){return s.ю.LengthSquared()>=1f?s.ю:s.ģ*2f;}void Ελ(){ΔΕ();if(++Αφ<3) return;Αφ=0;IGC.SendBroadcastMessage(Εΰ,ΓЍ());}void ΔΕ(){for(int i=Ϊ.Count-1;i>=0;i--)
if((DateTime.Now-Ϊ[i].Ȃ).TotalSeconds>3) Ϊ.RemoveAt(i);for(int i=Г.Count-1;i>=0;i--)
if((DateTime.Now-Г[i].Ƞ).TotalSeconds>2) Г.RemoveAt(i);for(int i=0;i<Ϊ.Count;i++) Ϊ[i].л=-1;for(int i=0;i<Ϊ.Count;i++)
for(int j=i+1;j<Ϊ.Count;j++){ϋ a=Ϊ[i],b=Ϊ[j];if(Č(a)||Č(b)) continue;Vector3 ΖΪ=a.Φ+a.ю*(float)(DateTime.Now-a.Ȃ).TotalSeconds;Vector3 Ζί=b.Φ+b.ю*(float)(DateTime.Now-b.Ȃ).TotalSeconds;Vector3 ā=Βρ(a),Ă=Βρ(b);Vector3 ȫ=Ζί-ΖΪ;float у=ȫ.Length();float ѕ=(a.ș+b.ș)*0.55f;
bool Γς=a.Η!=""&&a.Η==b.Η;if(Γς&&(a.Ƕ==2||b.Ƕ==2)) continue;if(Γς&&a.Ƕ==1&&b.Ƕ==1){String[]k=a.Η.Split('|');if(k.Length>=8) ѕ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Αζ=Ă-ā;float Γρ=Αζ.LengthSquared();float t=Γρ>0.01f?-Vector3.Dot(ȫ,Αζ)/Γρ:0;
bool Αϊ=у<ѕ;if(!Αϊ&&(t<=0||t>Εί||(ȫ+Αζ*t).Length()>=ѕ)) continue;if(!Αϊ&&a.Ǣ==b.Ǣ&&ā.LengthSquared()>0.25f&&Ă.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ā),Vector3.Normalize(Ă))<-0.7f)
continue;Ő p=null;for(int k=0;k<Г.Count;k++)
if((Г[k].a==a.Ν&&Г[k].b==b.Ν)||(Г[k].a==b.Ν&&Г[k].b==a.Ν)) p=Г[k];if(p==null){p=new Ő();p.a=a.Ν;p.b=b.Ν;p.ΑЅ=Εα(a,b,ā,Ă,ȫ,у);Г.Add(p);}p.Ƞ=DateTime.Now;ϋ y=p.ΑЅ==a.Ν?a:b;float ΓΫ=Αϊ||t<4?0:Math.Max(1,y.ю.Length()*0.5f);if(y.л<0||ΓΫ<y.л) y.л=ΓΫ;
}Δτ();}long Εα(ϋ a,ϋ b,Vector3 ā,Vector3 Ă,Vector3 ȫ,float у){if(a.Ν==Б) return b.Ν;if(b.Ν==Б) return a.Ν;if(a.Ǣ!=b.Ǣ) return a.Ǣ<b.Ǣ?a.Ν:b.Ν;Vector3 n=у>0.01f?ȫ/у:new Vector3(1,0,0);float Γψ=Vector3.Dot(ā,n),Γϋ=Vector3.Dot(Ă,-n);if(Γψ-Γϋ>1) return a.Ν;if(Γϋ-Γψ>1) return b.Ν;
return a.Ν>b.Ν?a.Ν:b.Ν;}String ΓЍ(){String ζ="C;";bool Л=true;for(int i=0;i<Ϊ.Count;i++){if(Ϊ[i].л<0) continue;ζ+=(Л?"":",")+Ϊ[i].Ν+":"+Math.Round(Ϊ[i].л,1);Л=false;}return ζ;}String Δν(){int Γδ=0,ΑΘ=0;for(int i=0;i<Ϊ.Count;i++){if(!Č(Ϊ[i])) Γδ++;if(Ϊ[i].л>=0) ΑΘ++;
}if(Ϊ.Count==0) return "";return "Traffic: "+Γδ+" moving"+(ΑΘ>0?", "+ΑΘ+" giving way":"")+"\n";}List<ί>Щ=new List<ί>();ί х=null;Vector3 ΓΡ;DateTime ΓΠ;float ΓΟ=40;bool ΓΝ=false;ϋ Ɩ(ί s){ϋ Ѝ=null;for(int i=0;i<Ϊ.Count;i++)
if(Vector3.Distance(Ϊ[i].Φ,s.Π)<15&&(Ѝ==null||
Vector3.Distance(Ϊ[i].Φ,s.Π)<Vector3.Distance(Ѝ.Φ,s.Π))) Ѝ=Ϊ[i];return Ѝ;}float Δυ(ί s){ϋ t=Ɩ(s);if(t==null) return 0;float k=t.Ƕ*1000000f;String[]И=t.Η.Split('|');if(И.Length>=3) k+=Vector3.Distance(t.Φ,new Vector3(float.Parse(И[0]),float.Parse(И[1]),float.Parse(И[2])));return k;
}void ΕΩ(){Щ.Clear();х=null;ΑΚ.Clear();List<float>ΑΝ=new List<float>();for(int i=0;i<ε.Count;i++){float k=Δυ(ε[i]);int Ǯ=0;while(Ǯ<ΑΝ.Count&&ΑΝ[Ǯ]<=k) Ǯ++;ΑΝ.Insert(Ǯ,k);Щ.Insert(Ǯ,ε[i]);ε[i].Ǖ("HOMEPOS",true,false,false);Ư("HOMEPOS",ε[i].Ν,"HOMEPOS");}}List<ϋ>ΑΚ=new List<ϋ>();
void Δτ(){ΑΚ.Clear();for(int i=0;i<Щ.Count;i++){ϋ t=Ɩ(Щ[i]);if(t==null) continue;t.л=0;ΑΚ.Add(t);}}void Εκ(){if(х!=null){bool ΕЌ=ΓΝ||Vector3.Distance(х.Π,ΓΡ)>ΓΟ||
(DateTime.Now-ΓΠ).TotalSeconds>45||х.Ƣ();if(!ΕЌ) return;х=null;}if(Щ.Count==0) return;х=Щ[0];Щ.RemoveAt(0);ΓΡ=х.Π;ΓΠ=DateTime.Now;ϋ t=Ɩ(х);ΓΟ=t==null?40:Math.Max(40,t.ș*3);ΓΝ=t!=null&&t.Αΰ==3&&t.ю.LengthSquared()<0.25f;}const String ΔΚ="[PAMDBG]";class ƒ{public readonly bool ƨ;
IMyProgrammableBlock ѓ;Action<IMyProgrammableBlock>Ƽ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>ƻ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>ƺ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>ƽ;Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>ƹ;
public ƒ(MyGridProgram Ζγ){ѓ=Ζγ.Me;var ǻ=ѓ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ѓ);if(ǻ==null) return;Ƽ=(Action<IMyProgrammableBlock>) ǻ["RemoveDraw"];ƻ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ǻ["Point"];ƺ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ǻ["Line"];
ƽ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ǻ["Sphere"];ƹ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) ǻ["GPS"];((Action<IMyProgrammableBlock>) ǻ["RemoveAll"])(ѓ);ƨ=true;}public void ΕΒ(){if(Ƽ!=null) Ƽ(ѓ);}public void ǔ(Vector3D a,Vector3D b,Color c,float Ζχ){if(ƺ!=null) ƺ(ѓ,a,b,c,Ζχ,-1,false);}
public void ΔЏ(Vector3D a,Color c,float ы){if(ƻ!=null) ƻ(ѓ,a,c,ы,-1,false);}public void ȏ(String Β,Vector3D Ǯ,Color c){if(ƹ!=null) ƹ(ѓ,Β,Ǯ,c,-1);}public void Βξ(Vector3D Ȗ,double ы,Color c){if(ƽ!=null) ƽ(ѓ,new BoundingSphereD(Ȗ,ы),c,1,0.05f,30,-1,false);}}class ą
{public long Ν;public Vector3 Ī,ΑΡ,Ě,ΰ;public bool ΓΛ;public List<Vector3>Ž=new List<Vector3>(),Αβ=new List<Vector3>();public List<bool>Αα=new List<bool>();public DateTime Ȃ;}ƒ υ=null;IMyBroadcastListener š=null;List<ą>т=new List<ą>();void Δο(String Ξ){String[]f=Ξ.Split(';');if(f.Length<6) return;
long Ν=long.Parse(f[0]);ą s=null;for(int i=0;i<т.Count;i++) if(т[i].Ν==Ν) s=т[i];if(s==null){s=new ą();s.Ν=Ν;т.Add(s);}s.Ī=Ϋ(f[1]);s.ΑΡ=Ϋ(f[2]);s.Ě=Ϋ(f[3]);s.ΓΛ=s.ΑΡ.LengthSquared()>0.5f;s.ΰ=Ϋ(f[4]);s.Ȃ=DateTime.Now;if(f[5]=="") return;s.Ž.Clear();s.Αβ.Clear();s.Αα.Clear();
String[]Γν=f[5].Split(':');for(int i=0;i<Γν.Length;i++){String[]r=Γν[i].Split(',');if(r.Length<3) continue;s.Ž.Add(Ϋ(r[0]));s.Αβ.Add(Ϋ(r[1]));s.Αα.Add(r[2]=="1");}}void Εζ(){if(!debugDraw) return;if(υ==null) υ=new ƒ(this);if(!υ.ƨ) return;if(š==null) š=IGC.RegisterBroadcastListener(ΔΚ);
int ĕ=20;while(š.HasPendingMessage&&ĕ-->0){String Ξ=š.AcceptMessage().Data as String;if(Ξ!=null) try{Δο(Ξ);}catch{}}for(int i=т.Count-1;i>=0;i--) if((DateTime.Now-т[i].Ȃ).TotalSeconds>5) т.RemoveAt(i);υ.ΕΒ();for(int i=0;i<Ϊ.Count;i++){ϋ s=Ϊ[i];Color c=Č(s)?Color.Gray:s.л>=0?Color.Yellow:Color.Lime;
υ.Βξ(s.Φ,s.ș/2,c);υ.ȏ(ŏ(s)+": "+(Č(s)?"parked":s.л==0?"holding (giving way)":s.л>0?"slowing (giving way)":"moving"),s.Φ,c);if(s.ю.LengthSquared()>0.25f) υ.ǔ(s.Φ,s.Φ+s.ю*3,Color.White,0.1f);if(s.ģ.LengthSquared()>0.25f) υ.ǔ(s.Φ,s.Φ+s.ģ*s.ș,Color.Cyan,0.1f);
}for(int i=0;i<Г.Count;i++){if((DateTime.Now-Г[i].Ƞ).TotalSeconds>0.5) continue;ϋ a=null,b=null;for(int k=0;k<Ϊ.Count;k++){if(Ϊ[k].Ν==Г[i].a) a=Ϊ[k];if(Ϊ[k].Ν==Г[i].b) b=Ϊ[k];}if(a==null||b==null) continue;υ.ǔ(a.Φ,b.Φ,Color.Red,0.2f);υ.ȏ("Conflict: "+ŏ(Г[i].ΑЅ==a.Ν?a:b)+" gives way",(a.Φ+b.Φ)/2,Color.Red);
}for(int i=0;i<т.Count;i++){ą d=т[i];ϋ t=null;for(int k=0;k<Ϊ.Count;k++) if(Ϊ[k].Ν==d.Ν) t=Ϊ[k];if(t!=null) υ.ǔ(t.Φ,d.ΰ,Color.Blue,0.08f);for(int r=0;r<d.Ž.Count;r++) υ.ǔ(d.Ž[r],d.Αβ[r],d.Αα[r]?Color.Red:Color.Green,0.04f);if(t==null||!d.ΓΛ) continue;г Η=null;
for(int j=0;j<З.Count;j++) if(З[j].И==t.Η) Η=З[j];if(Η==null) continue;int ΒБ=0;for(int c=0;c<Η.κ.Length;c++) if(Η.κ[c]=='1') ΒБ++;if(i==0||т[i-1].Ī!=d.Ī) υ.ȏ("Job: "+ΒБ+"/"+Η.κ.Length+" holes done",d.Ī,Color.White);for(int c=0;c<Η.κ.Length;c++){int ǆ=c%Η.w,Ǥ=c/Η.w;
Vector3 p=d.Ī+d.ΑΡ*((Η.w-1f)/2f-ǆ)*Η.Αό-d.Ě*((Η.h-1f)/2f-Ǥ)*Η.Αχ;р Αή=Ɯ(Η,c,null);υ.ΔЏ(p,Η.κ[c]=='1'?Color.Gray:Αή!=null?Color.Orange:Color.White,Math.Min(Η.Αό,Η.Αχ)*0.2f);if(Αή==null||Αή.Ν!=d.Ν) continue;υ.ǔ(t.Φ,p,Color.Orange,0.05f);υ.ȏ("Hole "+(Η.ź.IndexOf(c)+1)+": "+ŏ(t),p,Color.Orange);
}}for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];υ.Βξ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);υ.ȏ("Docking area"+(љ!=""?": "+љ:""),z.GetPosition(),Color.Cyan);
}for(int i=0;i<ћ.Count;i++)
υ.ǔ(ћ[i].GetPosition(),ћ[i].GetPosition()+ћ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(Б!=0&&ř!=0){IMyShipConnector c=Ƶ(ř);for(int k=0;k<Ϊ.Count;k++)
if(c!=null&&Ϊ[k].Ν==Б){υ.ǔ(Ϊ[k].Φ,c.GetPosition(),Color.Lime,0.15f);υ.ȏ("Cleared to "+(Ś==2?"undock: ":"dock: ")+ŏ(Ϊ[k]),c.GetPosition(),Color.Lime);}}}String ŏ(ϋ t){ί Ѝ=null;for(int i=0;i<ε.Count;i++)
if(Vector3.Distance(ε[i].Π,t.Φ)<30&&(Ѝ==null||
Vector3.Distance(ε[i].Π,t.Φ)<Vector3.Distance(Ѝ.Π,t.Φ))) Ѝ=ε[i];if(Ѝ!=null&&Ѝ.η!="") return Ѝ.η;String Ν=t.Ν.ToString();return "Ship "+Ν.Substring(Math.Max(0,Ν.Length-4));}String Δί(){if(!debugDraw) return "";if(υ==null||!υ.ƨ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}void ΕΦ(string А){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}