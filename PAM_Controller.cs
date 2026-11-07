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
public enum Ο{Ǘ,ƥ,ȍ,Е,ο}public enum Μ{Ǩ,Ɩ,ΒΫ,ƫ,Βζ,ą,Ǔ,
ƕ,ΔΨ,ȉ,ΑЊ,ΓЂ,ΑЃ,
ΕΒ,č,Č,Ǝ,Ελ,ΔΣ,
Ει,Εκ,ΔΤ,ΔΥ,Ď}public enum Τ{ё,ƨ,ǧ,ΑЋ,ŀ,Βά,Βέ,ļ,Ľ}public enum Ǐ{None,Ǝ,Ǩ,ΔΙ}public enum Κ{Main,Βδ,ć,Ć,ƴ,Ŀ,
Ɗ,ΒΞ,ΒΟ,ΒΝ,Ċ,Βθ,
ŉ,Ʈ,Ʊ,Βλ,ō}public enum ǖ{Βη,ΔΔ,ΕΤ,Δχ,ο}public enum У{Ő,ΓА}public enum ǒ{Βε,ΔΧ,ΕΧ,ƛ}public enum в{Βί,ΔΗ,ΔΖ,ΔΞ,ΔΝ,Δυ,Δτ}public enum я{ƒ,ΓІ,ΑЄ}public enum ń{Δσ,ΖЌ,ΓЃ}public enum Ѕ{Βΰ,Βα,Ββ,ΔЈ,ΔЇ,
ΔЂ,ΔЃ,ΔЁ,ΔЅ,
ΔІ,ΔЄ}public enum Υ{н,ђ,м,ľ,Ƌ,ƌ}public enum о{Βξ,ΑЌ,ΔЀ,Βή}public enum ι{None,ȏ,Ĉ,Ň}public enum τ{ƶ,Ɖ,Ƣ,ƛ,ΒΩ,Ƥ,ΒΪ,Εΰ,Ƨ}public enum ϊ{Ǘ,ΒΧ,ΒΦ,ƣ,Ɯ,Ɛ,ΒΨ}class ά{public bool ν=false;public Vector3 Π=new Vector3();public Vector3 Ѐ=new Vector3();
public Vector3 І=new Vector3();public Vector3 О=new Vector3();public Vector3 π=new Vector3();public Vector3 Ч=new Vector3();public float ΖΞ=0;public float ΗΒ=0;public float[]Я=null;public ά(){}public ά(ά Й){ν=Й.ν;Π=Й.Π;Ѐ=Й.Ѐ;І=Й.І;О=Й.О;π=Й.π;Ч=Й.Ч;Я=Й.Я;}public ά(Vector3 Π,Vector3 І,Vector3 Ѐ,Vector3 О,Vector3 π)
{this.Π=Π;this.Ѐ=Ѐ;this.І=І;this.О=О;this.ΖΞ=0;this.π=π;}public void ΖЋ(List<IMyThrust>Ŕ,List<string>ǎ){Я=new float[ǎ.Count];for(int i=0;i<Я.Length;i++)
Я[i]=-1;for(int i=0;i<Ŕ.Count;i++){string ȅ=Δκ(Ŕ[i]);int Α=ǎ.IndexOf(ȅ);if(Α!=-1)
Я[Α]=С(Ŕ[i].MaxEffectiveThrust,Ŕ[i].MaxThrust);}}}class ő{public Ѕ П=Ѕ.Βΰ;public float е=0;public float Ǳ=0;public string у="";public string ц="";DateTime ΓΨ;public bool Ζέ=false;private bool ǅ=false;public bool ΓБ(bool Αα){if(Αα){Ǳ=0;ǅ=false;return false;}ǅ=true;return Ǳ>е;
}public void ΖЍ(){ΓБ(true);Ζέ=false;}public void ΖЎ(){if(ǅ)
if((DateTime.Now-ΓΨ).TotalSeconds>1){Ǳ++;ΓΨ=DateTime.Now;}}public bool ΒΛ(){switch(П){case Ѕ.Βα:return true;case Ѕ.Ββ:return true;}return false;}}class ǩ{public String ζ="";public String Ќ="";public int Ј=0;public ń ΑΠ=ń.ΓЃ;public ǩ(String ζ,String Ќ,int Ј,ń ΑΠ){this.ζ=ζ;this.Ќ=Ќ;
this.Ј=Ј;this.ΑΠ=ΑΠ;}}class ί{public DateTime ΑΜ;public DateTime ΒЁ;public String Ν="";public String ζ="";public String Ƅ="";public String Ω="";public String Απ="";public String ŵ="";public String Αά="";public Vector3 Π=new Vector3();public List<ǩ>њ=new List<ǩ>();public Ο Ρ=Ο.Ǘ;
public ι Ǚ;public float Ǹ;public float Ǯ;public int ς;public int Ǉ=0;public int Αψ=0;public bool Ơ(){return(DateTime.Now-ΑΜ).TotalSeconds>10;}public bool Ɲ(){if(Ǚ!=ι.ȏ) return false;return(DateTime.Now-ΒЁ).TotalSeconds>=4;}public ί(String Ν){this.Ν=Ν;}public ι Ǖ(String μ,bool Ζλ,bool ѕ,bool Εφ)
{if(μ==""&&!ѕ) return ι.None;if(Ǚ==ι.ȏ&&Ɲ())
Ǚ=ι.Ň;if(Ζλ){Αά=μ;Ǚ=ι.ȏ;ΒЁ=DateTime.Now;return Ǚ;}else if(ѕ){Αά="";Ǚ=ι.None;}else if(Αά==μ){if(Εφ) Ǚ=ι.Ĉ;return Ǚ;}return ι.None;}}public String Δα(String ΖΖ){switch(ΖΖ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Н;Vector3 Γμ=new Vector3();Vector3 л=new Vector3();Vector3 ΕЇ=new Vector3();Vector3 ΖΙ=new Vector3();Vector3 ΕЀ=new Vector3();DateTime ΓΧ=new DateTime();
bool ğ=true;int Αυ=0;bool Ĩ=false;bool ǝ=false;bool ΖΣ=true;bool ΖΒ=false;float Ζν=0;float Γτ=0;int Ζε=0;int Ž=0;float Ȣ=0;float Ȕ=0;double ΑΦ=0;float Đ=0;List<int>ȝ=new List<int>();List<int>ȭ=new List<int>();Ο Ρ=Ο.Ǘ;int Αη=0;float Ȅ=0;float ȃ=0;float ȱ=0;float Γσ=0,Γς=0;
IMyRemoteControl Њ;IMySensorBlock ύ;List<IMyTimerBlock>Γϋ=new List<IMyTimerBlock>();List<IMyShipConnector>ė=new List<IMyShipConnector>();List<IMyThrust>ĵ=new List<IMyThrust>();List<IMyGyro>ΓΖ=new List<IMyGyro>();List<IMyTerminalBlock>а=new List<IMyTerminalBlock>();List<IMyLandingGear>ΑΛ=new List<IMyLandingGear>();
List<IMyReactor>Αΰ=new List<IMyReactor>();List<IMyConveyorSorter>Ζπ=new List<IMyConveyorSorter>();List<IMyGasTank>Ħ=new List<IMyGasTank>();List<IMyTerminalBlock>ď=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Βψ=new List<IMyBatteryBlock>();List<IMyTextPanel>Љ=new List<IMyTextPanel>();
List<IMyTextSurface>ь=new List<IMyTextSurface>();List<IMyTextPanel>ǰ=new List<IMyTextPanel>();IMyTerminalBlock Ǣ=null;ά Σ=new ά();ά ΓΜ=new ά();ά ΓΤ=new ά();bool Ǧ=false;bool ĸ=false;bool Ĺ=false;bool ΓΡ=false;List<String>ǎ=new List<string>();List<ά>ω=new List<ά>();List<ά>ƅ=new List<ά>();
double ΖЂ=0;ά Χ=new ά();Vector3 Ĭ;Vector3 ȣ;Τ φ=Τ.ё;У Ȟ=У.Ő;int к=0;int й=0;double ů=0;bool ΒЇ=false;ő χ=new ő();ő ψ=new ő();ǒ ǡ=ǒ.Βε;bool ĺ=true;У Ȁ=У.Ő;int Ц=3;int Х=3;int д=30;я ј=я.ƒ;bool ȇ=true;bool ħ=true;bool ĝ=true;bool Ȃ=false;в Ĝ=в.Βί;bool ǥ=true;
float ш=90;float щ=20;float Ы=5;float ъ=20;float ǘ=0.70f;float Ƈ=1.50f;float Ɔ=2.50f;float Ļ=0f;float ģ=0f;Μ ş;Τ ǟ;Μ ĳ;int ũ=0;int Ū=0;int р=0;int Ī=0;int Αδ=30;bool Ζρ=true;double ΒЌ=0;bool Γξ=false;int ΖЃ=0;int Ȇ=0;ά ž=null;float ΒІ=0;float ΑΥ=0;ά Εώ=null;
float Βω=0;Ǐ đ;float ΓΠ=0;float Γώ=0;String ΖΜ="";float ΑΟ=0;float Ǯ=0;float Ǹ=0;float Ё=0;bool Ακ=false;bool Ζχ=false;bool Ζξ=false;bool Γϊ=false;bool Ζο=false;float ΖΝ=0;float ΖΫ=0;Vector3 ΕЅ=new Vector3();Vector3 Γγ=new Vector3();bool ΕЌ=false;float[,]ķ=new float[3,2];
Dictionary<String,float[,]>Ǎ=new Dictionary<string,float[,]>();Dictionary<String,float[]>Ż=new Dictionary<string,float[]>();bool ΒЉ=false;int ŕ=0;int Ǜ=0;int Ŭ=0;int Ǵ=0;String Ω="";int[]Γΰ=new int[Enum.GetValues(typeof(Κ)).Length];bool ΑΨ=false;int ǹ=0;int ς=0;Κ γ=Κ.Main;
int Ζψ=0,Ζω=0,Ζϊ=0,Ζϋ=0;int Ζζ=0,Ζη=0;bool ē=false;bool Βτ=false;String ı="";String ǁ="";IMyBroadcastListener є=null;bool ΓΑ=true;List<ί>θ=null;ί σ=null;int ś=0,Ŝ=0;int ΒЃ=0,ΒЄ=0;int Εϋ=0,Εό=0,Εύ=0;String ΖΗ="";Exception Ƞ=null;bool Γν=false;Τ Αε=Τ.ё;
bool ΒЈ=false;String ΖЀ="";bool Γύ=false;bool Αξ=true;int ΑΘ=0;Vector3 ǂ=new Vector3();bool ΓΔ=false;bool Ɵ(IMyTerminalBlock α)=>α.CubeGrid==Me.CubeGrid;Program(){Н=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΜ(Me,controllerTag,true))
Ρ=Ο.Е;Ψ(Κ.Main);if(Ρ!=Ο.Е){Ω="Welcome to [PAM]!";о Αβ=ΔБ();if(Ρ==Ο.Ǘ){List<IMyShipDrill>ΒА=new List<IMyShipDrill>();List<IMyShipGrinder>ΓΕ=new List<IMyShipGrinder>();Н.GetBlocksOfType(ΒА,q=>q.CubeGrid==Me.CubeGrid);Н.GetBlocksOfType(ΓΕ,q=>q.CubeGrid==Me.CubeGrid);if(ΒА.Count>0)
{Ρ=Ο.ƥ;Ω="Miner mode enabled!";}else if(ΓΕ.Count>0){Ρ=Ο.ȍ;Ω="Grinder mode enabled!";}else{Ρ=Ο.ο;Βκ(ǖ.ο);}}if(Αβ==о.Βξ) ΖΣ=false;if(Αβ==о.ΑЌ) Ω="Data restore failed!";if(Αβ==о.Βή) Ω="New version, wipe data";}}void Main(string А,UpdateType Ζύ){try{if(Ƞ!=null){if(А.Trim().ToUpper()=="RETRY")
{Ƞ=null;ğ=true;Ζε=0;if(Ρ!=Ο.Е) ǫ();Ω="Restarted after error";}else{Δο();ΔЍ();return;}}ǝ=(Ζύ&UpdateType.Update10)!=0;if(ǝ) Αυ++;Ĩ=Αυ>=10;if(Ĩ) Αυ=0;if(ǝ){ŕ++;if(ŕ>4) ŕ=0;Ž=(int) Math.Max(0,10-(DateTime.Now-ΓΧ).TotalSeconds);}if(А!="") ΖΗ=А;if(Ρ!=Ο.Е)
ΕΣ(А);else
ΔΕ(А);ΑΨ=false;try{int ΒЅ=Runtime.CurrentInstructionCount;float ǽ=С(ΒЅ,Runtime.MaxInstructionCount);if(ǽ>0.90) Ω="Max. instructions >90%";if(ǽ>Ȣ) Ȣ=ǽ;if(ΒЉ){ȝ.Add(ΒЅ);while(ȝ.Count>10) ȝ.RemoveAt(0);Ȕ=0;for(int i=0;i<ȝ.Count;i++) Ȕ+=ȝ[i];Ȕ=С(С(Ȕ,ȝ.Count),Runtime.MaxInstructionCount);
double ΑΝ=Runtime.LastRunTimeMs;if(ΖΒ&&ΑΝ>ΑΦ) ΑΦ=ΑΝ;ȭ.Add((int)(ΑΝ*1000f));while(ȭ.Count>10) ȭ.RemoveAt(0);Đ=0;for(int i=0;i<ȭ.Count;i++) Đ+=ȭ[i];Đ=С(Đ,ȭ.Count)/1000f;}}catch{Ȣ=0;}}catch(Exception e){Ƞ=e;}}String Δβ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Ψ(Κ Ў){Γΰ[(int) γ]=ς;ς=Γΰ[(int) Ў];if(Ў==Κ.Ċ) ς=0;γ=Ў;if(Ρ!=Ο.Е)
œ(γ==Κ.ć,false,0,0);ΑΨ=true;}void Δώ(bool ΑΡ){if(ς>0) ς--;else if(ΑΡ) ς=ǹ-1;}void Δύ(bool ΑΡ){if(ς<ǹ-1) ς++;else if(ΑΡ) ς=0;}String Βκ(ǖ ε){switch(ε){case ǖ.Βη:Ω="Job is running";break;case ǖ.ΔΔ:Ω="Connector not ready!";break;case ǖ.ΕΤ:Ω="Ship modified, path outdated!";break;
case ǖ.Δχ:Ω="Interrupted by player!";break;case ǖ.ο:Ω="Shuttle mode enabled!";break;}return "";}String Δι(У Φ){switch(Φ){case У.Ő:return "Top-Left";case У.ΓА:return "Center";default:return "";}}String Δή(я λ){switch(λ){case я.ΓІ:return "Auto"+(Ρ==Ο.ƥ?" (Ore)":"");case я.ΑЄ:return "Auto (+Stone)";
case я.ƒ:return "Default";default:return "";}}String Δζ(Τ Ϗ){switch(Ϗ){case Τ.ё:return "No job";case Τ.ƨ:return "Job paused";case Τ.ǧ:return "Job active";case Τ.ļ:return "Job active";case Τ.Ľ:return "Job active";case Τ.ΑЋ:return "Job done";case Τ.ŀ:return "Job changed";case Τ.Βά:return "Move home";
case Τ.Βέ:return "Move to job";}return "";}String ΒΑ(ǒ Ες){switch(Ες){case ǒ.Βε:return "Return home";case ǒ.ΔΧ:return "Fly to job pos";case ǒ.ΕΧ:return "Stop";case ǒ.ƛ:return "Ignore";}return "";}String Δΰ(в λ){switch(λ){case в.Βί:return "Off";case в.ΔΞ:return "Drop pos (Stone) ";case в.ΔΝ:return "Drop pos (Sto.+Ice)";
case в.ΔΗ:return "Cur. pos (Stone)";case в.ΔΖ:return "Cur. pos (Sto.+Ice)";case в.Δυ:return "In motion (Stone)";case в.Δτ:return "In motion (Sto.+Ice)";}return "";}String ΑА(Ǐ Ϗ){switch(Ϗ){case Ǐ.None:return "No batteries";case Ǐ.Ǝ:return "Charging";case Ǐ.ΔΙ:return "Discharging";}return "";}
String ΒΕ(Μ Ϗ){String ΖА=Ρ==Ο.ο?"target":"job";switch(Ϗ){case Μ.Ǩ:return "Idle";case Μ.Ɩ:return "Flying to XY position";case Μ.ΒΫ:return Ρ==Ο.ȍ?"Grinding":"Mining";case Μ.ƫ:return "Returning";case Μ.ΔΤ:return "Flying to drop pos";case Μ.Βζ:return "Returning to dock";case Μ.ą:return "Flying to dock area";
case Μ.Ǔ:return "Flying to job area";case Μ.ƕ:return "Flying to path";case Μ.ΔΨ:return "Flying to job position";case Μ.ȉ:return "Approaching dock";case Μ.ΑЊ:return "Docking";case Μ.ΓЂ:return "Aligning";case Μ.ΑЃ:return "Aligning";case Μ.ΕΒ:return "Retry docking";case Μ.č:return "Unloading";case Μ.Ď:return ΖЀ;
case Μ.Č:return "Undocking";case Μ.Ǝ:return "Charging batteries";case Μ.Ελ:return "Waiting for uranium";case Μ.ΔΣ:return "Filling up hydrogen";case Μ.Ει:return "Waiting for ejection";case Μ.Εκ:return "Waiting for ejection";case Μ.ΔΥ:return "Flying to drop pos";}return "";}String ΒΚ(Ѕ П){switch(П)
{case Ѕ.Βΰ:return "On \"Undock\" command";case Ѕ.Βα:return "On player entered cockpit";case Ѕ.ΔЈ:return "On ship is full";case Ѕ.ΔЇ:return "On ship is empty";case Ѕ.Ββ:return "On time delay";case Ѕ.ΔЃ:return "On batteries empty(<25%)";case Ѕ.ΔЁ:return "On batteries empty(=0%)";case Ѕ.ΔЂ:return "On batteries full";
case Ѕ.ΔІ:return "On hydrogen empty(<25%)";case Ѕ.ΔЄ:return "On hydrogen empty(=0%)";case Ѕ.ΔЅ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Γρ,int ǚ,bool Ε,String Ƕ){ǹ+=1;if(Γρ==ǚ) Ƕ=">"+Ƕ+(ŕ>=2?" .":"");else Ƕ=" "+Ƕ;Β+=Ƕ+"\n";return Γρ==ǚ&&Ε;}String ΓЈ(bool Ε)
{int Α=0;int Δ=ς;ǹ=0;String Ȱ="———————————————\n";String ȳ="--------------------------------------------\n";String Β="";Β+=Δζ(φ)+" | "+(Σ.ν?"Ready to dock":"No dock")+"\n";Β+=Ȱ;double Γψ=Math.Max(Math.Round(this.ΒЌ),0);if(γ==Κ.Main){bool ǜ=Ρ==Ο.ο;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΕΥ();
if(Ρ==Ο.ƥ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Ψ(Κ.ć);if(Ρ==Ο.ȍ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Ψ(Κ.ć);if(Ρ==Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Ψ(Κ.Ʊ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ΑЈ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΔΦ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΔΩ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ǜ) Ψ(Κ.ō);else Ψ(Κ.Ŀ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Ψ(Κ.ΒΞ);if(Ρ!=Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Ψ(Κ.ΒΝ);}else if(γ==Κ.ć){double ΖЄ=Math.Round(Ц*Ȅ,1);double ΕЏ=Math.Round(Х*ȃ,1);String ξ="";if(Ζ(ref ξ,Δ,Α++,Ε," Start new job!")) Βν();if(Ζ(ref ξ,Δ,Α++,Ε," Change current job")){Ƶ(false,false);Ψ(Κ.Main);}if(Ζ(ref ξ,Δ,Α++,Ε," Width + (Width: "+Ц+" = "+ΖЄ+"m)")){Ȉ(ref Ц,5,20,1);Ȓ(true);}
if(Ζ(ref ξ,Δ,Α++,Ε," Width -")){Ȉ(ref Ц,-5,20,-1);Ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Height + (Height: "+Х+" = "+ΕЏ+"m)")){Ȉ(ref Х,5,20,1);Ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Height -")){Ȉ(ref Х,-5,20,-1);Ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Depth + ("+(ј==я.ƒ?"Depth":"Min")+": "+д+"m)")){Ȉ(ref д,5,50,2);Ȓ(true);}
if(Ζ(ref ξ,Δ,Α++,Ε," Depth -")){Ȉ(ref д,-5,50,-2);Ȓ(true);}if(Ζ(ref ξ,Δ,Α++,Ε," Depth mode: "+Δή(ј))){ј=Ǒ(ј);}if(Ζ(ref ξ,Δ,Α++,Ε," Start pos: "+Δι(Ȁ))){Ȁ=Ǒ(Ȁ);}if(Ρ==Ο.ȍ&&ј==я.ΑЄ) ј=Ǒ(ј);Β+=Ƭ(8,ξ,Δ,ref Ζζ);}else if(γ==Κ.Ʊ){float[]ΒЊ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Ψ(Κ.Βλ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΚ(χ.П))) χ.П=Ǒ(χ.П);if(!χ.ΒΛ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ɨ((int) χ.е))) χ.е=ΑЉ(χ.е,ΒЊ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΒΚ(ψ.П))) ψ.П=Ǒ(ψ.П);
if(!ψ.ΒΛ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ɨ((int) ψ.е))) ψ.е=ΑЉ(ψ.е,ΒЊ);}else if(γ==Κ.Βλ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) Βν();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Ʊ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.у!=""?χ.у:"-"))) χ.у=ņ(ref Ζψ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(χ.ц!=""?χ.ц:"-"))) χ.ц=ņ(ref Ζϊ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ψ.у!=""?ψ.у:"-"))) ψ.у=ņ(ref Ζω);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ψ.ц!=""?ψ.ц:"-"))) ψ.ц=ņ(ref Ζϋ);
}else if(γ==Κ.Ć){String ΓΫ=ш+" %";if(Ĩ) Ǜ++;if(Ǜ>1){Ǜ=0;Ŭ++;if(Ŭ>1) Ŭ=0;}bool[]Αλ=new bool[]{Αΰ.Count==0,đ==Ǐ.None,Ħ.Count==0};int Γό=0;while(true){Γό++;Ǵ++;if(Ǵ>Αλ.Length-1) Ǵ=0;if(Γό>=Αλ.Length) break;if(!Αλ[Ǵ]) break;}bool ǜ=Ρ==Ο.ο;if(!ǜ&&ȇ&&Ё!=-1&&Ŭ==0)
ΓΫ=Ё<1000000?Math.Round(Ё)+" Kg":Math.Round(Ё/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){ǫ();Ψ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ǜ) Ψ(Κ.Ŀ);else Ψ(Κ.ō);if(!ǜ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΓЁ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) Γύ=true;Β+=ȳ;if(!ǜ) Β+="Progress: "+Math.Round(ů,1)+" %\n";Β+="State: "+ΒΕ(ş)+" "+Γψ+"m \n";Β+="Load: "+ΑΟ+" % Max: "+ΓΫ+" \n";if(Ǵ==0) Β+="Uranium: "+(Αΰ.Count==0?"No reactors":Math.Round(Γώ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ǵ==1) Β+="Battery: "+(đ==Ǐ.None?ΑА(đ):Βω+"% "+(щ==-1||ǜ?"":" Min: "+щ+" %"))+"\n";if(Ǵ==2) Β+="Hydrogen: "+(Ħ.Count==0?"No tanks":Math.Round(ΓΠ,1)+"% "+(ъ==-1||ǜ?"":" Min: "+ъ+" %"))+"\n";}else if(γ==Κ.Ŀ){String ξ="";if(Ζ(ref ξ,Δ,Α++,Ε," Back")){if(φ==Τ.ǧ) Ψ(Κ.Ć);else Ψ(Κ.Main);}
if(Ζ(ref ξ,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ƣ,false);if(Ζ(ref ξ,Δ,Α++,Ε," Weight limit: "+(ȇ?"On":"Off"))) ȇ=!ȇ;if(Ζ(ref ξ,Δ,Α++,Ε," Ejection: "+Δΰ(Ĝ))) Ĝ=Ǒ(Ĝ);if(Ζ(ref ξ,Δ,Α++,Ε," Toggle sorters: "+(Ȃ?"On":"Off"))){Ȃ=!Ȃ;if(Ȃ) ư(Αξ);}
if(Ζ(ref ξ,Δ,Α++,Ε," Unload ice: "+(ǥ?"On":"Off"))) ǥ=!ǥ;if(Ζ(ref ξ,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.Ƥ,true);if(Ζ(ref ξ,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Min "+щ+"%"))) Д(ref щ,-5,τ.ΒΩ,true);if(Ζ(ref ξ,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) Д(ref ъ,-10,τ.ΒΪ,true);
if(Ζ(ref ξ,Δ,Α++,Ε," When done: "+(ĺ?"Return home":"Stop"))) ĺ=!ĺ;if(Ζ(ref ξ,Δ,Α++,Ε," On damage: "+ΒΑ(ǡ))) ǡ=Ǒ(ǡ);if(Ζ(ref ξ,Δ,Α++,Ε," Advanced...")) Ψ(Κ.Ɗ);Β+=Ƭ(8,ξ,Δ,ref Ζη);}else if(γ==Κ.Ɗ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.ǧ) Ψ(Κ.Ć);else Ψ(Κ.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ρ==Ο.ȍ?" Grinder":" Drill")+" inv. balancing: "+(ħ?"On":"Off"))) ħ=!ħ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ρ==Ο.ȍ?" grinders":" drills")+": "+(ĝ?"Fwd + Bwd":"Fwd"))) ĝ=!ĝ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ƈ+"m/s")) Д(ref Ƈ,0.5f,τ.ƶ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ɔ+"m/s")) Д(ref Ɔ,0.5f,τ.ƶ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǘ*100f)+"%"+(ǘ>0.80f?" (risky)":""))) Д(ref ǘ,0.1f,τ.Ɖ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+Ļ*100f+"%")) ΑЇ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ģ*100f+"%")) ΑЇ(false,0.05f);
}else if(γ==Κ.ō){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.ǧ) Ψ(Κ.Ć);else Ψ(Κ.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ƣ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ǥ?"On":"Off"))) ǥ=!ǥ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.Ƥ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Charge up"))) щ=(щ==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Fill up"))) ъ=(ъ==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ΒΑ(ǡ))) ǡ=Ǒ(ǡ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǘ*100f)+"%"+(ǘ>0.80f?" (risky)":""))) Д(ref ǘ,0.1f,τ.Ɖ,false);
}else if(γ==Κ.Βδ){double ΖΘ=0;if(ω.Count>0) ΖΘ=Vector3.Distance(ω.Last().Π,л);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΕΪ();if(Ρ!=Ο.ο){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(Ǧ?"Use old home":(Σ.ν?"Was set! ":"none ")))) Ǧ=!Ǧ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(Ǧ?"Use old connector":(Σ.ν?"Was set! ":"none ")))) Ǧ=!Ǧ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(ĸ?"Use old connector":(Χ.ν?"Was set! ":"none ")))) ĸ=!ĸ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(Ĺ?"Use old path":(ω.Count>1?"Count: "+ω.Count:"none ")))) Ĺ=!Ĺ;Β+=ȳ;Β+="Wp spacing: "+Math.Round(ΖЂ)+"m\n";}else if(γ==Κ.ƴ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){ǫ();Ψ(Κ.Main);}
Β+=ȳ;Β+="State: "+ΒΕ(ş)+" \n";Β+="Speed: "+Math.Round(Ζν,1)+"m/s\n";Β+="Target dist: "+Γψ+"m\n";Β+="Wp count: "+ω.Count+"\n";Β+="Wp left: "+ΖЃ+"\n";}else if(γ==Κ.ΒΞ){List<IMyTerminalBlock>ǯ=Δά();if(Ĩ) Ǜ++;if(Ǜ>=ǯ.Count) Ǜ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Ψ(Κ.ΒΟ);
Β+=ȳ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΟ,1)+"% "+Math.Round(Ǯ,1)+" / "+Math.Round(Ǹ,1)+"\n";Β+="Uranium: "+(Αΰ.Count==0?"No reactors":Math.Round(Γώ,1)+"Kg "+ΖΜ)+"\n";Β+="Battery: "+(đ==Ǐ.None?"":Βω+"% ")+ΑА(đ)+"\n";Β+="Hydrogen: "+(Ħ.Count==0?"No tanks":Math.Round(ΓΠ,1)+"% ")+"\n";
Β+="Damage: "+(ǯ.Count==0?"None":""+(Ǜ+1)+"/"+ǯ.Count+" "+ǯ[Ǜ].CustomName)+"\n";}else if(γ==Κ.ΒΟ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=ȳ;Β+="Next scan: "+Ž+"s\n";Β+="Drill area: "+Math.Round(Ȅ,1)+"x"+Math.Round(ȃ,1)+"m Ship: "+Math.Round(Γσ,1)+"x"+Math.Round(Γς,1)+"m ("+Math.Round(ȱ,1)+"m)\n"+(Ευ.Count>0?"Cameras: "+Ετ+"\n":"");
Β+="Broadcast: "+(ē?"Online - "+ı:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ȣ*100f,1)+"% \n";}else if(γ==Κ.ΒΝ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Ψ(Κ.Main);Β+=ȳ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Αη==2) Β="Fatal setup error\nNext scan: "+Ž+"s\n";if(Γν) Β="Recompile script now";int ΖΚ=Β.Split('\n').Length;for(int i=ΖΚ;i<=10;i++) Β+="\n";Β+=Ȱ;Β+="Last: "+Ω+"\n";
return Β;}void ΑЇ(bool ΖΕ,float ƀ){ǫ();Ƶ(true,false);if(ΖΕ) Д(ref Ļ,ƀ,τ.Ƨ,false);else Д(ref ģ,ƀ,τ.Ƨ,false);ΓЏ();œ(true,true,0,0);}float ΑЉ(float ȗ,float[]ƃ){float έ=ƃ[0];for(int i=ƃ.Length-1;i>=0;i--)
if(ȗ<ƃ[i]) έ=ƃ[i];return έ;}String ņ(ref int ǚ){String έ="";if(ǚ>=Γϋ.Count) ǚ=-1;if(ǚ>=0) έ=Γϋ[ǚ].CustomName;ǚ++;return έ;}void Ȉ(ref int value,int Ē,int Ɓ,int Αμ){if(Ē==0) return;if(value<Ɓ&&Αμ>0||value<=Ɓ&&Αμ<0){value+=Αμ;return;}int Εν=Math.Abs(Ē);int Ƽ=0;
int ΑΩ=1;while(true){Ƽ+=ΑΩ*Εν*10;if(Ē<0&&value-Ɓ<=Ƽ) break;if(Ē>0&&value-Ɓ<Ƽ) break;ΑΩ++;}value+=ΑΩ*Ē;}void Ȓ(bool ΗΓ){Ц=Math.Max(Ц,1);Х=Math.Max(Х,1);д=Math.Max(д,0);œ(γ==Κ.ć,false,0,0);}void Д(ref float value,float ƀ,τ Ќ,bool Ķ){Д(ref value,value+ƀ,Ќ,Ķ,true);
}void Д(ref float value,float Ȥ,τ Ќ,bool Ķ,bool ΖЇ){float Ь=0,Ъ=0;if(Ќ==τ.ƶ){Ь=0.5f;Ъ=10f;}if(Ќ==τ.Ɖ){Ь=0.1f;Ъ=1f;}if(Ќ==τ.ƛ){Ь=50f;Ъ=100f;}if(Ќ==τ.ΒΩ){Ь=5f;Ъ=30f;}if(Ќ==τ.Ƥ){Ь=1f;Ъ=25f;}if(Ќ==τ.Ƣ){Ь=10f;Ъ=95f;}if(Ќ==τ.ΒΪ){Ь=10f;Ъ=90f;}
if(Ќ==τ.Εΰ){Ь=10f;Ъ=1800;}if(Ќ==τ.Ƨ){Ь=0.0f;Ъ=0.75f;}if(Ȥ==-1&&Ķ){value=-1;return;}if(value==-1) Ķ=false;bool Γε=Ȥ<Ь||Ȥ>Ъ;if(Γε&&ΖЇ){if(Ȥ<value) value=Ъ;else if(Ȥ>value) value=Ь;}else value=Ȥ;if(Γε&&Ķ) value=-1;else value=Math.Max(Ь,Math.Min(value,Ъ));
value=(float) Math.Round(value,2);}void ΓЁ(bool Ζί){if(Ζί) р=Math.Max(0,р-1);else р++;ΔΠ();ΔΟ();for(int k=0;k<ΑΗ.Count&&k<з.Length;k++) ΕΞ(ΑΗ[k],k<р);ǻ=-1;Βσ(true);}T Ǒ<T>(T ȗ){int Α=Array.IndexOf(Enum.GetValues(ȗ.GetType()),ȗ);Α++;if(Α>=ΔΡ(ȗ)) Α=0;return(T) Enum.GetValues(ȗ.GetType()).GetValue(Α);
}int ΔΡ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΕΥ(){ƅ.Clear();for(int i=0;i<ω.Count;i++) ƅ.Add(ω[i]);ω.Clear();ΓΡ=true;ΓΜ=new ά(Σ);ΓΤ=new ά(Χ);Σ.ν=false;if(Ρ==Ο.ο) Χ.ν=false;for(int i=0;i<Ǎ.Count;i++)
if(!ǎ.Contains(Ǎ.Keys.ElementAt(i)))
ǎ.Add(Ǎ.Keys.ElementAt(i));Ǧ=false;ĸ=false;Ĺ=false;Ψ(Κ.Βδ);}void ΕΪ(){if(Ǧ) Σ=ΓΜ;if(ĸ) Χ=ΓΤ;if(Ĺ){ω.Clear();for(int i=0;i<ƅ.Count;i++) ω.Add(ƅ[i]);}ΓΡ=false;ǫ();Ψ(Κ.Main);}void ŋ(ά ж,Τ Ϗ){Εώ=ж;if(φ==Τ.ǧ) ǟ=Ϗ;}bool ΒΒ(ά ж,float т,bool Ζώ,out Vector3 Φ){if(Ζώ){Vector3I ΕЋ=new Vector3I((int) ж.Ч.X,(int) ж.Ч.Y,(int) ж.Ч.Z);
IMySlimBlock α=Me.CubeGrid.GetCubeBlock(ΕЋ);if(α==null||!(α.FatBlock is IMyShipConnector)){Φ=new Vector3();return false;}Vector3 Εϊ=Ņ(Њ,α.FatBlock.GetPosition()-л);Vector3 Εω=Ņ(Њ,α.FatBlock.WorldMatrix.Forward);Φ=ж.Π-Βπ(ж.І,ж.Ѐ*-1,Εϊ)-Βπ(ж.І,ж.Ѐ*-1,Εω)*т;return true;}
else{Φ=ж.Π;return true;}}Vector3 Δδ(int Ǆ,int ǣ,bool Ζβ){if(!Ζβ&&ΓΔ) return ǂ;float Εψ=((к-1f)/2f)-Ǆ;float Ζδ=((й-1f)/2f)-ǣ;ǂ=Χ.Π+Χ.О*Εψ*Ȅ+ȣ*-1*Ζδ*ȃ;ΓΔ=true;return ǂ;}Υ Ɠ(){float Ф=-1;Υ έ=Υ.н;if(Ρ!=Ο.ο){Δό();if(φ!=Τ.ё){Vector3 ό=Βρ(Ĭ,ȣ*-1,л-Χ.Π);
if(Math.Abs(ό.X)<=(float)(к*Ȅ)/2f&&Math.Abs(ό.Y)<=(float)(й*ȃ)/2f){if(ό.Z<=-1&&ό.Z>=-Αδ*2) return Υ.ľ;if(ό.Z>-1&&ό.Z<ȱ*2) return Υ.н;}if(ǐ(Χ.Π,ref Ф)) έ=Υ.н;}if(Σ.ν){if(ǐ(Σ.Π,ref Ф)) έ=Υ.м;for(int i=0;i<ω.Count;i++)
if(ǐ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Vector3.Distance(л,Σ.Π)<dockDist*ȱ) έ=Υ.м;if(ȋ(MyShipConnectorStatus.Connectable)!=null||ȋ(MyShipConnectorStatus.Connected)!=null)
έ=Υ.м;}}else{Vector3 Φ=new Vector3();IMyShipConnector Ś=ȋ(MyShipConnectorStatus.Connected);if(Σ.ν){if(ǐ(Σ.Π,ref Ф)) έ=Υ.м;if(ΒΒ(Σ,dockDist,true,out Φ))
if(ǐ(Φ,ref Ф)) έ=Υ.м;if(Ś!=null&&Vector3.Distance(Ś.GetPosition(),Σ.Π)<5)
return Υ.Ƌ;}for(int i=0;i<ω.Count;i++)
if(Vector3.Distance(ω[i].Π,Σ.Π)>dockDist*ȱ&&Vector3.Distance(ω[i].Π,Χ.Π)>dockDist*ȱ)
if(ǐ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Χ.ν){if(ǐ(Χ.Π,ref Ф)) έ=Υ.н;if(ΒΒ(Χ,dockDist,true,out Φ))
if(ǐ(Φ,ref Ф)) έ=Υ.н;if(Ś!=null&&Vector3.Distance(Ś.GetPosition(),Χ.Π)<5)
return Υ.ƌ;}}return έ;}bool ǐ(Vector3 Φ,ref float Ф){float т=Vector3.Distance(Φ,л);if(т<Ф||Ф==-1){Ф=т;return true;}return false;}void Βν(){if(Αη>0){Ω="Setup error! Can't start";return;}if(Ρ==Ο.ο){ΑЈ();return;}Χ.Π=л;Χ.π=Њ.GetNaturalGravity();Χ.І=ΕЇ;Χ.Ѐ=ΕЀ;Χ.О=ΖΙ;
Ĭ=Ǣ.WorldMatrix.Forward;ȣ=Χ.Ѐ;if(Ĭ==Њ.WorldMatrix.Down) ȣ=Њ.WorldMatrix.Backward;Ƶ(true,true);δ(Μ.Ɩ);Ʋ();}void Ƶ(bool Ш,bool Ζγ){if(φ==Τ.ё&&!Ш) return;bool ΖΤ=Ш||φ==Τ.ΑЋ||к!=Ц||й!=Х||Ȟ!=Ȁ;if(ΖΤ){if(φ!=Τ.ё){φ=Τ.ŀ;Δδ(ũ,Ū,Ζγ);Ω="Job changed, lost progress";}Ȟ=Ȁ;
к=Ц;й=Х;Ū=0;ũ=0;з="";ǻ=-1;ΑΥ=0;Ī=0;ΒІ=0;р=0;Βσ(true);}}void Ʃ(){ΕΜ(л,0);Ŋ(ĵ,true);}void Ʋ(){Βκ(ǖ.Βη);Ʃ();Ư(ΑΛ,false);ư(Αξ);φ=Τ.ǧ;ΓЎ(true);ǟ=φ;Ψ(Κ.Ć);ΕΕ();ΒЇ=true;ΑΘ=0;for(int i=ď.Count-1;i>=0;i--)
if(ΒΡ(ď[i],false)) ΑΘ++;if(ΑΘ>0) Ω="Started with damage";}void ǫ(){if(φ==Τ.ǧ){φ=Τ.ƨ;Ω="Job paused";}δ(Μ.Ǩ);ǟ=φ;ΕΝ(false,0,0,0,0);ΕΨ();ΓЄ(new Vector3(),false);ΕΩ();ΕΙ(ChargeMode.Auto);ΕΟ(false);ΕΛ(true);Ǫ(Μ.Ǩ);œ(false,false,0,0);Ŋ(а,false);Ŋ(ė,true);ư(true);Γξ=false;ΔΑ();
ΕϏ=false;ΔЏ();ΒЇ=false;Ακ=false;Γύ=false;if(γ!=Κ.Main&&γ!=Κ.Ŀ&&γ!=Κ.Ɗ&&γ!=Κ.ō)
Ψ(Κ.Main);}void ΑЈ(){Υ ќ=Ɠ();if(Ρ==Ο.ο){if(!Χ.ν||!Σ.ν) return;Ʋ();bool ΑΓ=Vector3.Distance(л,Σ.Π)<Vector3.Distance(л,Χ.Π);if(Αε==Τ.ļ) ΑΓ=true;if(Αε==Τ.Ľ) ΑΓ=false;if(ΑΓ){ŋ(Σ,Τ.ļ);switch(ќ){case Υ.Ƌ:δ(Μ.Ď);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.м:δ(Μ.ȉ);break;default:δ(Μ.Č);break;
}}else{ŋ(Χ,Τ.Ľ);switch(ќ){case Υ.ƌ:δ(Μ.Ď);break;case Υ.н:δ(Μ.ȉ);break;case Υ.ђ:δ(Μ.Ǔ);break;default:δ(Μ.Č);break;}}}else{if(φ!=Τ.ƨ&&φ!=Τ.ŀ) return;bool ΖЁ=φ==Τ.ŀ;Ʋ();bool ΑΕ=Δψ(false)&&Σ.ν;switch(ќ){case Υ.н:δ(ΑΕ?Μ.ƕ:Μ.Ɩ);break;case Υ.ђ:δ(ΑΕ?Μ.ą:Μ.Ǔ);break;case Υ.м:δ(ΑΕ?Μ.ȉ:Μ.č);break;
case Υ.ľ:{if(Ī!=р||ΖЁ) δ(Μ.ƫ);else δ(Μ.ΒΫ);}break;default:break;}}}void ΔΩ(){if(φ==Τ.ё&&!Σ.ν) return;if(Ρ==Ο.ο&&(!Χ.ν||!Σ.ν)) return;Ω="Move to job";Υ ќ=Ɠ();if(Ρ==Ο.ο){ŋ(Χ,Τ.Ľ);switch(ќ){case Υ.н:δ(Μ.ȉ);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.ƌ:return;default:δ(Μ.Č);break;
}Ǫ(Μ.Ď);}else{switch(ќ){case Υ.н:δ(Μ.Ǔ);break;case Υ.ђ:δ(Μ.Ǔ);break;case Υ.м:δ(Μ.č);break;case Υ.ľ:δ(Μ.ƫ);break;default:break;}if(φ==Τ.ё) Ǫ(Μ.Ǔ);else Ǫ(Μ.ΑЃ);Γξ=true;}Ʃ();Ψ(Κ.ƴ);Ư(ΑΛ,false);ǟ=Τ.Βέ;}void ΔΦ(){if(!Σ.ν) return;Ω="Move home";Υ ќ=Ɠ();if(Ρ==Ο.ο){ŋ(Σ,Τ.ļ);switch(ќ)
{case Υ.ђ:δ(Μ.ą);break;case Υ.м:δ(Μ.ȉ);break;case Υ.Ƌ:return;default:δ(Μ.Č);break;}Ǫ(Μ.Ď);}else{if(ȋ(MyShipConnectorStatus.Connected)!=null) return;if(ȋ(MyShipConnectorStatus.Connectable)!=null){δ(Μ.ΑЊ);Ǫ(Μ.č);return;}switch(ќ){case Υ.н:δ(Μ.ƕ);break;case Υ.ђ:δ(Μ.ą);break;case Υ.м:δ(Μ.ą);break;
case Υ.ľ:δ(Μ.Βζ);break;default:break;}Ǫ(Μ.č);}Ʃ();Ψ(Κ.ƴ);Ư(ΑΛ,false);ǟ=Τ.Βά;}void δ(Μ ΑΪ){if(ΑΪ==Μ.Ǩ) ĳ=Μ.Ǩ;if(ĳ!=Μ.Ǩ&&ş==ĳ&&ΑΪ!=ĳ){ǫ();return;}Ζρ=true;ş=ΑΪ;}void Ǫ(Μ Ϗ){this.ĳ=Ϗ;}void Βσ(bool Αα){if(Αα){ů=0;return;}if(Ρ==Ο.ο) return;float κ=р*Math.Max(1,д);
if(Ī==р) κ+=Math.Min(д,ΒІ);float Ζό=к*й*Math.Max(1,д);ů=Math.Max(ů,(float) Math.Min(κ/Ζό*100.0,100));}List<IMyTerminalBlock>Δά(){List<IMyTerminalBlock>ǯ=new List<IMyTerminalBlock>();for(int i=0;i<ď.Count;i++){IMyTerminalBlock α=ď[i];if(ΒΡ(α,false)) ǯ.Add(α);}return ǯ;
}bool ΒΡ(IMyTerminalBlock α,bool Αρ){return(!ΒΣ(α,Αρ)||!α.IsFunctional);}bool ΒΣ(IMyTerminalBlock α,bool Αρ){if(α==null) return false;try{IMyCubeBlock ΓΓ=Me.CubeGrid.GetCubeBlock(α.Position).FatBlock;if(Αρ) return ΓΓ==α;else return ΓΓ.GetType()==α.GetType();}catch{return false;}}bool Δψ(bool ΗΑ)
{if(ȇ&&Ρ!=Ο.ο)
if(Ё!=-1&&Γτ>=Ё){Ω="Ship too heavy";return true;}if(ΑΟ>=ш||Ακ){Ακ=false;Ω="Ship is full";return true;}return false;}static String Δκ(IMyThrust э){return э.BlockDefinition.SubtypeId;}Vector3 ơ(Vector3 К,float[,]ȡ){return new Vector3(
К.X>=0?ȡ[0,0]:ȡ[0,1],
К.Y>=0?ȡ[1,0]:ȡ[1,1],
К.Z>=0?ȡ[2,0]:ȡ[2,1]);}bool ΒΙ(ά ή,String ȅ,out float Ǌ){Ǌ=0;int Α=ǎ.IndexOf(ȅ);if(Α==-1||ή.Я==null||Α>=ή.Я.Length)
return false;Ǌ=ή.Я[Α];if(Ǌ==-1) return false;return true;}Vector3 ΒΘ(Vector3 К,ά ή){if(ή!=null){Vector3 έ=new Vector3();for(int i=0;i<Ǎ.Keys.Count;i++){String ȅ=Ǎ.Keys.ElementAt(i);float ě=0;if(!ΒΙ(ή,ȅ,out ě))
return ơ(К,ķ);έ+=ơ(К,Ǎ.Values.ElementAt(i))*ě;}return έ;}return ơ(К,ķ);}float ƙ(Vector3 К,ά ή){return ƙ(К,new Vector3(),ή);}float ƙ(Vector3 К,Vector3 π,ά ή){Vector3 Ζφ=ΒΘ(К,ή);Vector3 ΕЂ=Ζφ+π*Γτ;float ǽ=(ΕЂ/К).AbsMin();return(float)(К*ǽ).Length();}float Ń(Vector3 Ġ,Vector3 Ђ,Vector3 π,ά ή)
{if(π.Length()==0f) return 0;Vector3 ΖΛ=Βρ(Ġ,Ђ,Vector3.Normalize(π));float Ш=ƙ(-ΖΛ,ή);return Ш/π.Length();}void ΓЎ(bool ΖΑ){float Р=0;float ǋ=0.9f;if(ΖΑ){Ё=-1;Ȇ=0;ž=null;if(φ!=Τ.ё&&Χ.π.Length()!=0){Р=ǋ*Ń(Χ.І,Χ.Ѐ*-1,Χ.π,null);if(Р<Ё||Ё==-1) Ё=Р;}if(Σ.ν&&Σ.π.Length()!=0)
{Р=ǋ*Ń(Σ.І,Σ.Ѐ*-1,Σ.π,null);if(Р<Ё||Ё==-1) Ё=Р;}return;}if(Ȇ==-1) return;if(Ȇ>=0){int Γι=0;while(Ȇ<ω.Count){if(Γι>100) return;Γι++;ά ή=ω[Ȇ];if(ή.π.Length()!=0f){Р=ǋ*Math.Min(
Ń(ή.І,ή.Ѐ*-1,ή.π,ή),
Ń(ή.І*-1,ή.Ѐ*-1,ή.π,ή));if(Р<Ё||Ё==-1) Ё=Р;}else ž=ή;Ȇ++;}Ȇ=-1;}bool ΑΔ=true;float Ǡ=0;if(ž!=null){for(int i=0;i<Ǎ.Count;i++){String ȅ=Ǎ.Keys.ElementAt(i);float[,]Αϊ=Ǎ.Values.ElementAt(i);float ě=0;if(!ΒΙ(ž,ȅ,out ě)){ΑΔ=false;break;}for(int a=0;a<Αϊ.GetLength(0);a++)
for(int b=0;b<Αϊ.GetLength(1);b++){float Ǭ=Math.Abs(Αϊ[a,b]*ě);if(Ǭ==0) continue;ΑΔ=true;if(Ǡ==0||Ǭ<Ǡ) Ǡ=Ǭ;}}}if(!ΑΔ){for(int a=0;a<ķ.GetLength(0);a++)
for(int b=0;b<ķ.GetLength(1);b++){float Ǭ=Math.Abs(ķ[a,b]);if(Ǭ==0) continue;if(Ǡ==0||Ǭ<Ǡ) Ǡ=Ǭ;}}if(Ǡ>0){float ΖΟ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Р=С(Ǡ,ΖΟ);if(Р>0)
if(Р<Ё||Ё==-1) Ё=Р;}}void ΓЄ(Vector3 Ш,bool ώ){if(!ώ){for(int i=0;i<ĵ.Count;i++)
ĵ[i].SetValueFloat("Override",0.0f);return;}Vector3 ƾ=ΒΘ(Ш,null);float ΖЈ=Math.Min(1,Math.Abs(С(Ш.X,ƾ.X)));float ΖЉ=Math.Min(1,Math.Abs(С(Ш.Y,ƾ.Y)));float ΖЊ=Math.Min(1,Math.Abs(С(Ш.Z,ƾ.Z)));for(int i=0;i<ĵ.Count;i++){IMyThrust э=ĵ[i];Vector3 ș=ΕΓ(Ņ(Њ,э.WorldMatrix.Backward),1);
if(ș.X!=0&&Math.Sign(ș.X)==Math.Sign(Ш.X))
э.SetValueFloat("Override",э.MaxThrust*ΖЈ);else if(ș.Y!=0&&Math.Sign(ș.Y)==Math.Sign(Ш.Y))
э.SetValueFloat("Override",э.MaxThrust*ΖЉ);else if(ș.Z!=0&&Math.Sign(ș.Z)==Math.Sign(Ш.Z))
э.SetValueFloat("Override",э.MaxThrust*ΖЊ);else
э.SetValueFloat("Override",0.0f);}}void ΕΝ(bool ώ,float Ζή,float Ȩ,float Ȭ,float ă){for(int i=0;i<ΓΖ.Count;i++){IMyGyro ρ=ΓΖ[i];ρ.GyroOverride=ώ;if(!ώ) ρ.GyroPower=100;else ρ.GyroPower=Ζή;if(!ώ) continue;Vector3 М=Њ.WorldMatrix.Forward;Vector3 ȫ=Њ.WorldMatrix.Right;Vector3 Ђ=Њ.WorldMatrix.Up;
Vector3 Ț=ρ.WorldMatrix.Forward;Vector3 Ȝ=ρ.WorldMatrix.Up;Vector3 ț=ρ.WorldMatrix.Left*-1;if(Ț==М) ρ.SetValueFloat("Roll",ă);else if(Ț==(М*-1)) ρ.SetValueFloat("Roll",ă*-1);else if(Ȝ==(М*-1)) ρ.SetValueFloat("Yaw",ă);else if(Ȝ==М) ρ.SetValueFloat("Yaw",ă*-1);else if(ț==М) ρ.SetValueFloat("Pitch",ă);
else if(ț==(М*-1)) ρ.SetValueFloat("Pitch",ă*-1);if(ț==(ȫ*-1)) ρ.SetValueFloat("Pitch",Ȩ);else if(ț==ȫ) ρ.SetValueFloat("Pitch",Ȩ*-1);else if(Ȝ==ȫ) ρ.SetValueFloat("Yaw",Ȩ);else if(Ȝ==(ȫ*-1)) ρ.SetValueFloat("Yaw",Ȩ*-1);else if(Ț==(ȫ*-1)) ρ.SetValueFloat("Roll",Ȩ);
else if(Ț==ȫ) ρ.SetValueFloat("Roll",Ȩ*-1);if(Ȝ==(Ђ*-1)) ρ.SetValueFloat("Yaw",Ȭ);else if(Ȝ==Ђ) ρ.SetValueFloat("Yaw",Ȭ*-1);else if(ț==Ђ) ρ.SetValueFloat("Pitch",Ȭ);else if(ț==(Ђ*-1)) ρ.SetValueFloat("Pitch",Ȭ*-1);else if(Ț==Ђ) ρ.SetValueFloat("Roll",Ȭ);else if(Ț==(Ђ*-1)) ρ.SetValueFloat("Roll",Ȭ*-1);
}}void ΕΩ(){this.ΕЌ=false;}void ΕΨ(){Ō(false,false,false,Γγ,0);Γϊ=false;}void ΕΜ(Vector3 ΰ,float Ĳ){Ō(true,false,false,ΰ,Ĳ);}void Ō(bool ώ,bool Αν,bool Αϋ,Vector3 ΰ,float Ĳ){Ō(ώ,Αν,Αϋ,ΰ,ΰ-л,0.0f,Ĳ);}void Ō(bool ώ,bool Αν,bool Αϋ,Vector3 ΰ,Vector3 Ζι,float Ζκ,float Ĳ)
{Γϊ=true;this.Ζχ=ώ;Γγ=ΰ;this.ΖΝ=Ĳ;this.ΖΫ=Ζκ;this.Ζο=Αν;this.Ζξ=Αϋ;this.ΕЅ=Ζι;ΒЌ=Vector3.Distance(ΰ,л);}void ΕΕ(){Н.GetBlocksOfType(ď,Ɵ);}void ΓЏ(){if(Њ==null) return;Ǣ=null;float Ŷ=0,Ų=0,ŷ=0,ų=0,Ÿ=0,Ŵ=0;List<IMyTerminalBlock>Ας=ΑЍ(а,pamTag,true);bool ΖΦ=Ας.Count==0;
if(Ας.Count>0) Ǣ=Ας[0];else if(а.Count>0) Ǣ=а[0];int Ş=0;float ы=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=Ǣ.WorldMatrix.Forward){if(ΖΦ){Αη=2;Ω="Mining direction is unclear!";return;}continue;}Ş++;ы=Math.Max(ы,Εα(а[i]));Vector3 ό=Ƙ(Њ,а[i].GetPosition());
if(i==0){Ŷ=ό.X;Ų=ό.X;ŷ=ό.Y;ų=ό.Y;Ÿ=ό.Z;Ŵ=ό.Z;}Ų=Math.Max(ό.X,Ų);Ŷ=Math.Min(ό.X,Ŷ);ų=Math.Max(ό.Y,ų);ŷ=Math.Min(ό.Y,ŷ);Ŵ=Math.Max(ό.Z,Ŵ);Ÿ=Math.Min(ό.Z,Ÿ);}if(ы==0) ы=drillRadius;bool ę=Ǣ!=null&&Ǣ.WorldMatrix.Forward==Њ.WorldMatrix.Down;Ȅ=(Ų-Ŷ)*(1-Ļ)+ы*2;
ȃ=(ę?Ŵ-Ÿ:ų-ŷ)*(1-ģ)+ы*2;IMyCubeGrid g=Њ.CubeGrid;float Αώ=0,ΑϏ=0,ΑЀ=0,ΑЁ=0;for(int c=0;c<8;c++){Vector3 p=Ƙ(Њ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ę?p.Z:p.Y;if(c==0||p.X<Αώ) Αώ=p.X;if(c==0||p.X>ΑϏ) ΑϏ=p.X;if(c==0||y<ΑЀ) ΑЀ=y;if(c==0||y>ΑЁ) ΑЁ=y;}Γσ=ΑϏ-Αώ+g.GridSize;Γς=ΑЁ-ΑЀ+g.GridSize;}float Εα(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΑЍ<T>(List<T>Ї,String Ю,bool ū){List<T>έ=new List<T>();for(int i=0;i<Ї.Count;i++)
if(ΒΜ(Ї[i],Ю,ū)) έ.Add(Ї[i]);return έ;}bool ΒΜ<T>(T α,String Ю,bool ū){IMyTerminalBlock b=(IMyTerminalBlock) α;if(ū&&b.CustomName.ToUpper().Contains(Ю.ToUpper())) return true;if(!ū&&b.BlockDefinition.ToString().ToUpper().Contains(Ю.ToUpper())) return true;return false;}void Ŋ<T>(List<T>Ї,bool ώ)
{for(int i=0;i<Ї.Count;i++)
ΕΚ((IMyTerminalBlock) Ї[i],ώ);}void ΕΚ(IMyTerminalBlock α,bool ώ){if(α==null) return;String Εξ=ώ?"OnOff_On":"OnOff_Off";var a=α.GetActionWithName(Εξ);a.Apply(α);}void ư(bool ώ){Αξ=ώ;if(!Ȃ) return;Ŋ(Ζπ,ώ);}void ΕΙ(ChargeMode λ){for(int i=0;i<Βψ.Count;i++) Βψ[i].ChargeMode=λ;}void ΕΟ(bool Ζσ)
{for(int i=0;i<Ħ.Count;i++) Ħ[i].Stockpile=Ζσ;}void Ư(List<IMyLandingGear>ΑΒ,bool Γά){for(int i=0;i<ΑΒ.Count;i++){if(Γά) ΑΒ[i].Lock();if(!Γά) ΑΒ[i].Unlock();}}void ΕΛ(bool ώ){if(ΒЈ==ώ) return;List<IMyShipController>ŝ=new List<IMyShipController>();Н.GetBlocksOfType(ŝ,Ɵ);if(ŝ.Count==0) return;
for(int i=0;i<ŝ.Count;i++)
ŝ[i].DampenersOverride=ώ;ΒЈ=ώ;}IMyShipConnector ȋ(MyShipConnectorStatus Ζς){for(int i=0;i<ė.Count;i++){if(!ΒΣ(ė[i],true)) continue;if(ė[i].Status==Ζς) return ė[i];}return null;}void œ(bool Αθ,bool š,float ΕЈ,float Ερ){if(ύ==null||а.Count==0) return;Vector3 ȕ=new Vector3();
int Ş=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=Ǣ.WorldMatrix.Forward) continue;Ş++;ȕ+=а[i].GetPosition();}ȕ=ȕ/Ş;Vector3 ȥ=Ƙ(ύ,ȕ);ύ.Enabled=true;ύ.ShowOnHUD=Αθ;ύ.LeftExtend=(š?1:Ц)/2f*Ȅ-ȥ.X;ύ.RightExtend=(š?1:Ц)/2f*Ȅ+ȥ.X;ύ.TopExtend=(š?1:Х)/2f*ȃ+ȥ.Y;
ύ.BottomExtend=(š?1:Х)/2f*ȃ-ȥ.Y;ύ.FrontExtend=(Αθ?д:ΕЈ)-ȥ.Z;ύ.BackExtend=Αθ?0:Ερ+ȱ*0.75f+ȥ.Z;ύ.DetectFloatingObjects=true;ύ.DetectAsteroids=false;ύ.DetectLargeShips=true;ύ.DetectSmallShips=true;ύ.DetectStations=true;ύ.DetectOwner=true;ύ.DetectSubgrids=false;
ύ.DetectPlayers=false;ύ.DetectEnemy=true;ύ.DetectFriendly=true;ύ.DetectNeutral=true;}void ĉ<T>(List<T>Ї,bool Ζμ,float ΕІ,bool ΖΔ){for(int i=0;i<Ї.Count;i++){IMyTextSurface Ĵ=null;if(Ї[i]is IMyTextSurface) Ĵ=(IMyTextSurface) Ї[i];if(Ĵ!=null){Ĵ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ζμ) continue;Ĵ.Font="Debug";if(ΖΔ) continue;Ĵ.FontSize=ΕІ;}}}static float С(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΕΓ(Vector3 v,int ǆ){return new Vector3(Math.Round(v.X,ǆ),Math.Round(v.Y,ǆ),Math.Round(v.Z,ǆ));}Vector3 Ƙ(IMyTerminalBlock α,Vector3 ΖІ){return Ņ(α,ΖІ-α.GetPosition());
}Vector3 Ņ(IMyTerminalBlock α,Vector3 Αύ){return Vector3D.TransformNormal(Αύ,MatrixD.Transpose(α.WorldMatrix));}Vector3 Βρ(Vector3 Ġ,Vector3 Ђ,Vector3 Αύ){MatrixD ΑΤ=MatrixD.CreateFromDir(Ġ,Ђ);return Vector3D.TransformNormal(Αύ,MatrixD.Transpose(ΑΤ));}Vector3 Βπ(Vector3 Ġ,Vector3 Ђ,Vector3 ό)
{MatrixD ΑΤ=MatrixD.CreateFromDir(Ġ,Ђ);return Vector3D.Transform(ό,ΑΤ);}String β(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ϋ(String s){String[]η=s.Split('|');return new Vector3(float.Parse(Ι(η,0)),float.Parse(Ι(η,1)),float.Parse(Ι(η,2)));}String Εμ(ά ή){String ǿ=":";
String έ=β(ή.Π)+ǿ+β(ή.І)+ǿ+
β(ή.Ѐ)+ǿ+β(ή.О)+ǿ+β(ή.π);for(int i=0;i<ή.Я.Length;i++)
έ+=ǿ+Math.Round(ή.Я[i],3);return έ;}ά ΕΫ(String s){String[]η=s.Split(':');ά ή=new ά(
Ϋ(Ι(η,0)),Ϋ(Ι(η,1)),
Ϋ(Ι(η,2)),Ϋ(Ι(η,3)),
Ϋ(Ι(η,4)));int Α=5;List<float>ΒБ=new List<float>();while(Α<η.Length){String Ƃ=Ι(η,Α);float f=0;if(!float.TryParse(Ƃ,out f)) break;ΒБ.Add(f);Α++;}ή.Я=ΒБ.ToArray();return ή;}System.Text.StringBuilder Ȯ=new System.Text.StringBuilder();void Λ<T>(T value,bool ΖΥ){if(ΖΥ) Ȯ.Append('\n');Ȯ.Append(value);}
void Λ<T>(T value){Λ(value,true);}String Ł(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Œ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Εο,int Α){String Ƃ=Εο.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Ƃ)) return "";
return Ƃ;}void Save(){if(Γν||Ρ==Ο.Е){Storage="";return;}Storage=ΓЉ();}String ΓЉ(){Ȯ.Clear();Ȯ.Append(DATAREV+";");Λ(β(Γμ),false);Λ(β(Σ.І));Λ(β(Σ.О));Λ(β(Σ.Ѐ));Λ(β(Σ.π));Λ(β(Σ.Π));Λ(β(Σ.Ч));Λ(Σ.ν);Λ(β(Χ.Π));Λ(β(Χ.π));Λ(β(Χ.І));Λ(β(Χ.Ѐ));Λ(β(Χ.О));Λ(β(Χ.Ч));Λ(Χ.ν);Λ(β(Ĭ));Λ(β(ȣ));Λ(";");
Λ((int) Ρ,false);Λ((int) φ);Λ((int) ǟ);Λ(ш);Λ(щ);Λ(Ы);Λ(ъ);Λ(ǘ);Λ(ȇ);Λ(ǥ);if(Ρ==Ο.ο){Λ((int) χ.П);Λ(χ.е);Λ(χ.Ǳ);Λ(Ł(χ.у));Λ(Ł(χ.ц));Λ((int) ψ.П);Λ(ψ.е);Λ(ψ.Ǳ);Λ(Ł(ψ.у));Λ(Ł(ψ.ц));}else{Λ((int) Ȁ);Λ((int) ǡ);Λ((int) Ĝ);Λ((int) ј);Λ((int) Ȟ);Λ(ĺ);Λ(Ȃ);Λ(ħ);Λ(ĝ);Λ(Ц);Λ(Х);Λ(д);Λ(Ƈ);Λ(Ɔ);Λ(Ļ);Λ(ģ);
Λ(к);Λ(й);Λ(ũ);Λ(Ū);Λ(Ī);Λ(р);Λ(Αδ);Λ(ΑΥ);}Λ(";");for(int i=0;i<ǎ.Count;i++)
Λ((i>0?"|":"")+ǎ[i],false);Λ(";");for(int i=0;i<ω.Count;i++)
Λ(Εμ(ω[i]),i>0);Λ(";",false);Λ(з+"|"+ǻ,false);Λ(";",false);Λ(ΔΛ(),false);String Ξ=Ȯ.ToString();Ȯ.Clear();return Ξ;}о ΔБ(){return ΕΑ(Storage);}о ΕΑ(String Ξ){if(Ξ=="") return о.ΔЀ;String[]Ǿ=Ξ.Split(';');if(Ι(Ǿ,0)!=DATAREV) return о.Βή;int Α=0;try{String[]Θ=Ι(Ǿ,1).Split('\n');
Γμ=Ϋ(Ι(Θ,Α++));Σ.І=Ϋ(Ι(Θ,Α++));Σ.О=Ϋ(Ι(Θ,Α++));Σ.Ѐ=Ϋ(Ι(Θ,Α++));Σ.π=Ϋ(Ι(Θ,Α++));Σ.Π=Ϋ(Ι(Θ,Α++));Σ.Ч=Ϋ(Ι(Θ,Α++));Σ.ν=bool.Parse(Ι(Θ,Α++));Χ.Π=Ϋ(Ι(Θ,Α++));Χ.π=Ϋ(Ι(Θ,Α++));Χ.І=Ϋ(Ι(Θ,Α++));Χ.Ѐ=Ϋ(Ι(Θ,Α++));Χ.О=Ϋ(Ι(Θ,Α++));Χ.Ч=Ϋ(Ι(Θ,Α++));Χ.ν=bool.Parse(Ι(Θ,Α++));
Ĭ=Ϋ(Ι(Θ,Α++));ȣ=Ϋ(Ι(Θ,Α++));Θ=Ι(Ǿ,2).Split('\n');Α=0;Ρ=(Ο) int.Parse(Ι(Θ,Α++));φ=(Τ) int.Parse(Ι(Θ,Α++));ǟ=(Τ) int.Parse(Ι(Θ,Α++));ш=int.Parse(Ι(Θ,Α++));щ=int.Parse(Ι(Θ,Α++));Ы=int.Parse(Ι(Θ,Α++));ъ=int.Parse(Ι(Θ,Α++));ǘ=float.Parse(Ι(Θ,Α++));ȇ=bool.Parse(Ι(Θ,Α++));
ǥ=bool.Parse(Ι(Θ,Α++));if(Ρ==Ο.ο){χ.П=(Ѕ) int.Parse(Ι(Θ,Α++));χ.е=float.Parse(Ι(Θ,Α++));χ.Ǳ=float.Parse(Ι(Θ,Α++));χ.у=Œ(Ι(Θ,Α++));χ.ц=Œ(Ι(Θ,Α++));ψ.П=(Ѕ) int.Parse(Ι(Θ,Α++));ψ.е=float.Parse(Ι(Θ,Α++));ψ.Ǳ=float.Parse(Ι(Θ,Α++));ψ.у=Œ(Ι(Θ,Α++));ψ.ц=Œ(Ι(Θ,Α++));}else{Ȁ=(У) int.Parse(Ι(Θ,Α++));
ǡ=(ǒ) int.Parse(Ι(Θ,Α++));Ĝ=(в) int.Parse(Ι(Θ,Α++));ј=(я) int.Parse(Ι(Θ,Α++));Ȟ=(У) int.Parse(Ι(Θ,Α++));ĺ=bool.Parse(Ι(Θ,Α++));Ȃ=bool.Parse(Ι(Θ,Α++));ħ=bool.Parse(Ι(Θ,Α++));ĝ=bool.Parse(Ι(Θ,Α++));Ц=int.Parse(Ι(Θ,Α++));Х=int.Parse(Ι(Θ,Α++));д=int.Parse(Ι(Θ,Α++));Ƈ=float.Parse(Ι(Θ,Α++));
Ɔ=float.Parse(Ι(Θ,Α++));Ļ=float.Parse(Ι(Θ,Α++));ģ=float.Parse(Ι(Θ,Α++));к=int.Parse(Ι(Θ,Α++));й=int.Parse(Ι(Θ,Α++));ũ=int.Parse(Ι(Θ,Α++));Ū=int.Parse(Ι(Θ,Α++));Ī=int.Parse(Ι(Θ,Α++));р=int.Parse(Ι(Θ,Α++));Αδ=int.Parse(Ι(Θ,Α++));ΑΥ=float.Parse(Ι(Θ,Α++));}Θ=Ι(Ǿ,3).Replace("\n","").Split('|');
ǎ=Θ.ToList();Θ=Ι(Ǿ,4).Split('\n');ω.Clear();if(Θ.Count()>=1&&Θ[0]!="")
for(int i=0;i<Θ.Length;i++)
ω.Add(ΕΫ(Ι(Θ,i)));Θ=Ι(Ǿ,5).Split('|');з=Ι(Θ,0);if(!int.TryParse(Ι(Θ,1),out ǻ)) ǻ=-1;if(з.Length!=к*й){з="";ǻ=-1;}ΔΚ(Ι(Ǿ,6));}catch{return о.ΑЌ;}Αε=ǟ;ǫ();return о.Βξ;}void ΕΠ(){ǰ.Clear();for(int i=Љ.Count-1;i>=0;i--){String Ξ=Љ[i].CustomData.ToUpper();bool ΑΙ=false;
if(Ξ=="INSTRUCTIONS"){ΑΙ=true;ΒЉ=true;}if(Ξ=="DEBUG") ΑΙ=true;if(ΑΙ){ǰ.Add(Љ[i]);Љ.RemoveAt(i);}}ĉ(ǰ,false,1,false);}void ΕΡ(List<IMyTerminalBlock>Βϋ){ь.Clear();for(int i=0;i<Βϋ.Count;i++){IMyTerminalBlock α=Βϋ[i];try{String Γθ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=α.CustomName.IndexOf(Γθ);int Ȳ=-1;if(Α<0||!int.TryParse(α.CustomName.Substring(Α+Γθ.Length,1),out Ȳ)) continue;if(Ȳ==-1) continue;Ȳ--;IMyTextSurfaceProvider Γκ=(IMyTextSurfaceProvider) α;if(Ȳ<Γκ.SurfaceCount&&Ȳ>=0)
ь.Add(Γκ.GetSurface(Ȳ));}catch{}}}String ΑΧ="";void ΔЍ(){String ΑΣ="";String Ě="";ΑΧ=ΓЈ(false);ΑΣ+=ΑΧ;Ě+=ΑΧ;Ě+=Δβ();for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(ΑΣ);for(int i=0;i<ь.Count;i++) ь[i].WriteText(ΑΣ);Echo(Ě);for(int i=0;i<ǰ.Count;i++){IMyTextPanel Џ=ǰ[i];
String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΓ());}}string ΒΓ(){String Β="";try{float Γέ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ȕ*Γέ)+" / "+(int)(Ȣ*Γέ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ȕ*100f,1)+"% / "+Math.Round(Ȣ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Đ,2)+"ms / "+Math.Round(ΑΦ,2)+"ms \n";}catch{}for(int i=0;i<Ż.Count;i++)
Β+=""+Ż.Keys.ElementAt(i)+" = "+Math.Round(Ż.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ż.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Δο(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ĉ(Љ,setLCDFontAndSize,0.9f,false);ĉ(ь,setLCDFontAndSize,0.9f,true);for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(Β+Ƞ.ToString());for(int i=0;i<ь.Count;i++) ь[i].WriteText(Β+Ƞ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ƭ(String ε,String ΰ,String ǲ){try{if(!ē) return;ε=BROADCAST_TAG+BROADCAST_SEP+ΑБ()+BROADCAST_SEP+ΰ+BROADCAST_SEP+ǁ+BROADCAST_SEP+ǲ+BROADCAST_SEP+ε;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ε);}catch(Exception e){Ƞ=e;}}String ΑБ(){if(Ρ!=Ο.Е) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΒΤ(String ε){return ε.StartsWith(BROADCAST_TAG);}bool Ѓ(ref String Ξ,out string Ğ,Char ǿ){int Α=Ξ.IndexOf(ǿ);Ğ="";if(Α<0) return false;Ğ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΔЉ(ref String ε,out String Ǥ,out String ǲ){Ǥ="";ǲ="";if(!ē) return false;
String Ю="";if(!Ѓ(ref ε,out Ю,BROADCAST_SEP)||!ΒΤ(Ю)) return false;if(!Ѓ(ref ε,out Ǥ,BROADCAST_SEP)) return false;if(!Ѓ(ref ε,out Ю,BROADCAST_SEP)||(Ю!=ΑБ()&&(Ю!="*"&&Ρ!=Ο.Е))) return false;if(!Ѓ(ref ε,out Ю,BROADCAST_SEP)||(Ю!=ǁ)) return false;if(!Ѓ(ref ε,out ǲ,BROADCAST_SEP)) return false;
return true;}void Δφ(){bool Ė=true;if(ΓΑ){ΓΑ=false;if(Me.CustomData.Contains("Antenna_Name")){Ω="Update custom data";Me.CustomData="";}}String ġ=(Ρ!=Ο.Е?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ġ)
ΒΠ(ġ);String[]Θ=Me.CustomData.Split('\n');ē=bool.Parse(ň(ł(Θ,"Enable_Broadcast",ref Ė)));bool Αό=false;bool ΒЋ=true;if(ē){if(Ρ!=Ο.Е)
ı=ň(ł(Θ,"Ship_Name",ref Ė)).Replace(BROADCAST_SELF_ID,"");ǁ=ň(ł(Θ,"Broadcast_Channel",ref Ė)).ToLower();ΒЋ=false;if(є==null){є=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);є.SetMessageCallback("");}List<IMyRadioAntenna>ȓ=new List<IMyRadioAntenna>();Н.GetBlocksOfType(ȓ);bool ƽ=false;
for(int i=0;i<ȓ.Count;i++)
if(ȓ[i].EnableBroadcasting&&ȓ[i].Enabled){ƽ=true;break;}if(ȓ.Count==0) Ω="No Antenna found";else if(!ƽ) Ω="Antenna not ready";Αό=ȓ.Count==0||!ƽ;if(Βτ&&!Αό&&Ρ!=Ο.Е) Ω="Antenna ok";}else if(Ρ==Ο.Е)
Ω="Offline - Enable in PB custom data";Βτ=Αό;if(ΒЋ){if(є!=null) this.IGC.DisableBroadcastListener(є);є=null;}}catch{Ė=false;}if(!Ė){Ω="Reset custom data";ΒΠ(ġ);}}void ΒΠ(String ġ){String Ξ=ġ+"\n\n"+"Enable_Broadcast="+(Ρ==Ο.Е?"true":"false")+" \n";Ξ+=Ρ!=Ο.Е?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ň(String Ƿ){int Α=Ƿ.IndexOf("//");if(Α!=-1) Ƿ=Ƿ.Substring(0,Α);String[]η=Ƿ.Split('=');if(η.Length<=1) return "";return η[1].Trim();}String ł(String[]Θ,String И,ref bool Ȧ){foreach(String Ƿ in Θ)
if(Ƿ.StartsWith(И)) return Ƿ;Ȧ=false;return "";}void ΔΕ(string А){if(ğ) θ=new List<ί>();String Ǥ="";String ǲ="";if(!ΒΤ(А)) ΔЎ(А);bool Γυ=false;int ΖΠ=50;while(є!=null&&є.HasPendingMessage&&ΖΠ-->0){MyIGCMessage ε=є.AcceptMessage();String Ξ=ε.Data as String;if(Ξ==null) continue;
if(ΔЉ(ref Ξ,out Ǥ,out ǲ)&&Ǥ!=""&&Ǥ!=BROADCAST_SELF_ID){ί Γ=Ɣ(Ǥ,"");if(Γ==null){Γ=new ί(Ǥ);θ.Add(Γ);}Γ.Ǖ(ǲ,false,false,true);Εζ(Γ,Ξ);Γυ=true;}}if(Γυ)
θ.Sort(delegate(ί a,ί b){if(a.ζ==null&&b.ζ==null) return 0;else if(a.ζ==null) return-1;else if(b.ζ==null) return 1;else return a.ζ.CompareTo(b.ζ);});if(Ĩ||ğ){if(Ž<=0||ğ){Ω="";ğ=false;ΕΖ();Δφ();ΓΧ=DateTime.Now;}}if(ǝ) Εδ();if(ǝ) Εε();if(ǝ) Εθ();if(ǝ) Εη();if(ǝ&&Ασ==0) Εγ();
Εβ();}void Εζ(ί Γ,String Ξ){Γ.ΑΜ=DateTime.Now;String Ğ="";Ѓ(ref Ξ,out Γ.Ƅ,BROADCAST_SEP);Ѓ(ref Ξ,out Γ.ζ,BROADCAST_SEP);if(Γ.Ƅ!=VERSION) return;String Į="";String[]ȩ=new string[3];Ѓ(ref Ξ,out Į,BROADCAST_SEP);Ѓ(ref Ξ,out ȩ[0],BROADCAST_SEP);Ѓ(ref Ξ,out ȩ[1],BROADCAST_SEP);Ѓ(ref Ξ,out ȩ[2],BROADCAST_SEP);
Ѓ(ref Ξ,out Γ.Απ,BROADCAST_SEP);Ѓ(ref Ξ,out Γ.Ω,BROADCAST_SEP);Ѓ(ref Ξ,out Ğ,BROADCAST_SEP);Ѓ(ref Ξ,out Γ.ŵ,BROADCAST_SEP);String ΓϏ="",Γί="";Ѓ(ref Ξ,out ΓϏ,BROADCAST_SEP);Ѓ(ref Ξ,out Γί,BROADCAST_SEP);Γ.њ.Clear();while(true){String ΓΣ;if(!Ѓ(ref Ξ,out ΓΣ,BROADCAST_SEP)) break;String[]Ů=ΓΣ.Split('/');
if(Ů.Count()<3) continue;int Ј=0;if(!int.TryParse(Ů[2],out Ј)) continue;Γ.њ.Add(new ǩ(Ů[0],Ů[1],Ј,ń.Δσ));}int Γα=0;if(!int.TryParse(Ğ,out Γ.ς)) Γ.ς=0;if(!int.TryParse(Į,out Γα)) Γ.Ρ=Ο.Ǘ;if(!float.TryParse(ȩ[0],out Γ.Π.X)) Γ.Π.X=0;if(!float.TryParse(ȩ[1],out Γ.Π.Y)) Γ.Π.Y=0;
if(!float.TryParse(ȩ[2],out Γ.Π.Z)) Γ.Π.Z=0;if(!float.TryParse(ΓϏ,out Γ.Ǯ)) Γ.Ǯ=0;if(!float.TryParse(Γί,out Γ.Ǹ)) Γ.Ǹ=0;Γ.Ρ=(Ο) Γα;Γ.Ǉ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Π));Γ.Αψ=0;for(int i=0;i<Γ.њ.Count();i++) Γ.Αψ+=Γ.њ[i].Ј;}ί Ɣ(String Ν,String ζ){Ν=Ν.ToUpper();ζ=ζ.ToUpper();
for(int i=0;i<θ.Count;i++){if(Ν!=""&&θ[i].Ν.ToUpper()==Ν) return θ[i];if(ζ!=""&&θ[i].ζ!=null&&θ[i].ζ.ToUpper()==ζ) return θ[i];}return null;}String Ȗ="";ί Βώ=null;DateTime ΒϏ=DateTime.MinValue;List<ί>і=new List<ί>();const float ΓЍ=20f;bool Βι(ί ΰ,String μ){return μ==Ȗ&&ΰ==Βώ&&((DateTime.Now-ΒϏ).TotalSeconds<ΓЍ||
(μ=="HOMEPOS"&&(ф.Count>0||Щ!=null)));}String ΔΪ(ί ΰ){if(Ȗ==""||!Βι(ΰ,Ȗ)) return "";String s="";int Ȧ=0,Αι=0;for(int i=0;i<і.Count;i++){ι ſ=і[i].Ǖ(Ȗ,false,false,false);if(ſ==ι.Ĉ) Ȧ++;bool Ζα=ф.Contains(і[i]);if(Αι++<8) s+="  "+і[i].ζ+": "+(Ζα?"queued ("+(ф.IndexOf(і[i])+1)+")":ſ==ι.Ĉ?"received":
ſ==ι.ȏ?"pending...":ſ==ι.Ň?"no answer!":"-")+"\n";}if(Αι>8) s+="  +"+(Αι-8)+" more\n";return "--------------------------------------------\n"+Ȗ+": "+Ȧ+"/"+і.Count+" received\n"+s;}void Т(ί Γ,String μ){Ȗ=μ;Βώ=Γ;ΒϏ=DateTime.Now;і.Clear();if(Γ==null) і.AddRange(θ);else і.Add(Γ);
if(Γ==null){ф.Clear();Щ=null;}else ф.Remove(Γ);if(Γ==null&&μ=="HOMEPOS"){ΕΦ();return;}if(Γ==null){for(int i=0;i<θ.Count;i++)
θ[i].Ǖ(μ,true,false,false);ƭ(μ,"*",μ);}else{Γ.Ǖ(μ,true,false,false);ƭ(μ,Γ.Ν,μ);}}void ΔЎ(string А){if(А=="") return;var η=А.ToUpper().Split(' ');η.DefaultIfEmpty("");var μ=η.ElementAtOrDefault(0);var Βφ=η.ElementAtOrDefault(1);String ΖΓ="Invalid argument: "+А;if(γ==Κ.Ċ){if(σ!=null)
{switch(μ){case "UP":{if(ς<2) break;if(σ.ς==0){ς=1;return;}Т(σ,"UP");return;}case "DOWN":{if(ς<1) break;if(ς==1){Т(σ,"MRES");break;}Т(σ,"DOWN");break;}case "APPLY":{if(ς<2) break;Т(σ,"APPLY");return;}}}}switch(μ){case "UP":this.Δώ(false);return;case "DOWN":this.Δύ(false);return;
case "APPLY":this.Ȋ(true);return;}switch(μ){case "CLEAR":ΔΒ();return;case "SENDALL":
if(String.IsNullOrEmpty(Βφ)){Ω="Usage: SENDALL <cmd>";return;}Т(null,Βφ);return;case "SEND":
if(А.Length<="SEND".Length+1){Ω="Usage: SEND <ship>:<cmd>";return;}ΕΘ(А.Remove(0,"SEND".Length+1));return;}Ω=ΖΓ;}void ΕΘ(String Βυ){if(Βυ=="") return;var η=Βυ.Split(':');if(η.Length!=2){Ω="Missing separator \":\"";return;}η.DefaultIfEmpty("");String ı=η.ElementAtOrDefault(0).Trim();
ί Γ=Ɣ("",ı);if(Γ!=null) Т(Γ,η.ElementAtOrDefault(1).Trim());else Ω="Unknown ship: "+ı;}void ΔΒ(){θ.Clear();σ=null;Ψ(Κ.Main);ς=0;}void ΕΖ(){List<IMyTerminalBlock>Γχ=new List<IMyTerminalBlock>();Н.GetBlocksOfType(Љ,Ɵ);Н.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Γχ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Љ=ΑЍ(Љ,pamTag,true);ΕΡ(Γχ);ΕΠ();ΕΗ();ĉ(Љ,setLCDFontAndSize,1.15f,false);ĉ(ь,setLCDFontAndSize,1.15f,true);String ΕА=ΒΔ(false);String Γδ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Џ in Љ){if(Џ.CustomData==""){Џ.CustomData=ΒΔ(true);continue;}
if(!Џ.CustomData.Contains(ΕА))
if(!Џ.CustomData.Contains(Γδ)) Џ.CustomData=Γδ+Џ.CustomData;}}void Εβ(){String Ě="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ь.Count;i++)
ь[i].WriteText(ΑЅ(ϊ.ΒΦ,"0",false));for(int i=0;i<Љ.Count;i++){ϊ λ=ϊ.Ǘ;String Э="";ΔЊ(Љ[i],out λ,out Э);Љ[i].WriteText(ΑЅ(λ,Э,i==0));}Echo(Ě+Δέ());for(int i=0;i<ǰ.Count;i++){IMyTextPanel Џ=ǰ[i];String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");
if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΓ());}}String Ȋ(bool Ε){int ΕЁ=0;return Ȋ(Ε,0,ref ΕЁ,false,1);}String Ȋ(bool Ε,int ΖΩ,ref int Ζθ,bool ΒЂ,int į){String Β="";String Ȱ="——————————————————\n";String Ζυ="--------------------------------------------\n";if(σ==null||γ==Κ.Main||γ==Κ.Ʈ)
Β+="[PAM]-Controller | "+θ.Count+" Connected ships"+"\n"+Δί()+Δε()+Δλ();else Β+=ΒΗ(σ)+"\n";Β+=Ȱ;int Α=0;int Δ=ς;ǹ=0;if(Ω!=""){Ζ(ref Β,Δ,Α++,Ε,Ω);}else if(γ==Κ.Main){if(σ==null&&θ.Count>=1) σ=θ[0];String Ї="";if(Ζ(ref Ї,Δ,Α++,Ε," Send command to all")) Ψ(Κ.Ʈ);
int ȯ=0;for(int s=0;s<θ.Count;s++){if(ΒЂ) Ї+="\n";ί Γ=θ[s];if(Δ==Α||Δ==Α+1) σ=Γ;if(Α==Δ) ȯ=Ї.Split('\n').Length-1;if(Ζ(ref Ї,Δ,Α++,Ε," "+ΒΗ(Γ))){σ=Γ;Ψ(Κ.Ċ);σ.Ǖ("",false,true,false);}if(Α==Δ) ȯ=Ї.Split('\n').Length-1;if(Ζ(ref Ї,Δ,Α++,Ε," "+(ΒЂ?"":"  ")+Δη(Γ)))
{σ=Γ;Ψ(Κ.Βθ);}}int Ă=į-2;Ă+=Math.Max(0,(ΖΩ-1))*į;Β+=Ƭ(Ă,Ї,ȯ,ref Ζθ);}else if(γ==Κ.ŉ||γ==Κ.Ʈ){Κ Επ=γ==Κ.ŉ?Κ.Ċ:Κ.Main;ί ΰ=null;if(γ==Κ.ŉ) ΰ=σ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Ψ(Επ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+Ȍ(ΰ,"STOP"))){Т(ΰ,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+Ȍ(ΰ,"CONT"))){Т(ΰ,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+Ȍ(ΰ,"HOMEPOS"))){Т(ΰ,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+Ȍ(ΰ,"JOBPOS"))){Т(ΰ,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+Ȍ(ΰ,"FULL"))){Т(ΰ,"FULL");}if(ΰ!=null&&ΰ.Ρ==Ο.ο)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+Ȍ(ΰ,"UNDOCK"))){Т(ΰ,"UNDOCK");}Β+=ΔΪ(ΰ);}else if(γ==Κ.Ċ){String ƿ="";if(Ζ(ref ƿ,Δ,Α++,Ε," Back")){Ψ(Κ.Main);}Β+=ƿ.Substring(0,ƿ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Ψ(Κ.ŉ);}if(!σ.Ơ()&&!σ.Ɲ()) ǹ++;if(σ.Ɲ()) Β+="No answer received...";
else Β+=ȑ(σ,σ.ŵ);}else if(γ==Κ.Βθ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Ψ(Κ.Main);}Β+=Ζυ;Β+=ȑ(σ,ΒΖ(σ));}if(!ΑΨ) ς=Math.Min(ǹ-1,ς);return Β;}String ΒΗ(ί Γ){if(Γ.Ƅ!=VERSION) return Γ.ζ+": Different version!";return Γ.ζ+": "+ȑ(Γ,Γ.Απ)+" "+Γ.Ǉ+"m";}String Δη(ί Γ){String s=ƪ("",С(Γ.Ǯ,Γ.Ǹ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.њ.Count;i++){if(i>=5) break;ǩ х=Γ.њ[i];s+=Βς(ƍ(Δγ(х)),3)+" "+ΑЏ(х.Ј)+" ";}return s;}String Δγ(ǩ х){if(х.Ќ.ToUpper()=="ORE"||х.Ќ.ToUpper()=="INGOT"){String ΒЀ=Δα(х.ζ.ToUpper());if(ΒЀ!="") return ΒЀ;}return х.ζ;}String ȑ(ί Γ,String Β){if(Γ.Ơ()) return "No signal...("+Ɨ((int)(DateTime.Now-Γ.ΑΜ).TotalSeconds)+")";
return Β;}String ΒΔ(bool ΖЅ){String Β="";if(ΖЅ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΔЊ(IMyTerminalBlock Џ,out ϊ λ,out String Э){bool Ȧ=true;String Į=ň(ł(Џ.CustomData.Split('\n'),"mode",ref Ȧ)).ToUpper();String[]η=Į.Split(':');String ĭ=η.Length>0?η[0].Trim():"";Э=η.Length>1?η[1].Trim():"";
λ=ϊ.Ǘ;if(ĭ=="MAIN") λ=ϊ.ΒΧ;else if(ĭ=="MAINX") λ=ϊ.ΒΨ;else if(ĭ=="MENU") λ=ϊ.ƣ;else if(ĭ=="INVENTORY") λ=ϊ.Ɯ;else if(ĭ=="DEBUG") λ=ϊ.Ɛ;if(λ==ϊ.Ǘ) Э=Į;}String ΑЅ(ϊ λ,String Э,bool ΕЄ){int ī=15;if(ΕЄ){ΒЃ=ś;ś=0;ΒЄ=Ŝ;Ŝ=0;}String Β="";if(λ==ϊ.ΒΧ){int Ў=0;
Β=Ȋ(false,ΒЃ,ref Εϋ,true,ī);if(!int.TryParse(Э,out Ў)) return Β;ś=Math.Max(Ў,ś);Ў--;return Βγ(Ў,ī,Β);}else if(λ==ϊ.ΒΨ){int Ў=0;Β=Ȋ(false,ΒЄ,ref Εό,false,ī);if(!int.TryParse(Э,out Ў)) return Β;Ŝ=Math.Max(Ў,Ŝ);Ў--;return Βγ(Ў,ī,Β);}if(λ==ϊ.ΒΦ){return Ȋ(false,0,ref Εύ,true,ī);
}else if(λ==ϊ.Ɯ||λ==ϊ.ƣ||λ==ϊ.Ɛ){ί Γ=σ;if(Э!=""){Γ=Ɣ("",Э);if(Γ==null) return "Unknown ship: "+Э;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ȱ="—————————————————————\n";String Αφ="";String ǀ="";if(λ==ϊ.ƣ){Αφ="Menu";ǀ=ȑ(Γ,Γ.ŵ);}
else if(λ==ϊ.Ɯ){Αφ="Inventory";ǀ=ȑ(Γ,ΒΖ(Γ));}else if(λ==ϊ.Ɛ){return ȑ(Γ,Δθ(Γ));}Β+=Γ.ζ+" - "+Γ.Ǉ+"m | "+ƍ(""+Αφ)+"\n"+Ȱ;Β+=ǀ;return Β;}return "Unknown command: "+Э;}string Δθ(ί Γ){String s="";s+=Γ.Ƅ+"\n";s+=Γ.ζ+"\n";s+=Γ.Π.X+"\n";s+=Γ.Π.Y+"\n";
s+=Γ.Π.Z+"\n";s+=Γ.Απ+"\n";s+=Γ.Ω+"\n";s+=Γ.ς+"\n";s+=Γ.ŵ.Length+"\n";s+=Γ.Ǯ+"\n";s+=Γ.Ǹ+"\n";s+=Γ.њ.Count()+"\n";return s;}string ΒΖ(ί Γ){String s="";s+=ƪ("All",С(Γ.Ǯ,Γ.Ǹ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.њ.Count();i++){ǩ х=Γ.њ[i];
s+=ƪ(х.ζ,х.Ј,Γ.Αψ,30,8,10)+"\n";}return s;}ι ΔΫ(ί Γ,bool ѕ,string μ){ι έ=ι.None;if(Γ==null){for(int i=0;i<θ.Count;i++){ι Ϗ=θ[i].Ǖ(μ,false,ѕ,false);if(Ϗ==ι.Ĉ) έ=Ϗ;if(Ϗ==ι.Ň) return Ϗ;if(Ϗ==ι.ȏ) return Ϗ;}return έ;}else return Γ.Ǖ(μ,false,ѕ,false);}String Ȍ(ί Γ,String μ)
{if(!Βι(Γ,μ)) return "";ι Ϗ=ΔΫ(Γ,false,μ);if(Ϗ==ι.Ĉ) return "received!";if(Ϗ==ι.ȏ) return "pending...";if(Ϗ==ι.Ň) return "no answer!";return "";}String Ƭ(int Ă,String Β,int ȯ,ref int ѝ){String[]Θ=Β.Split('\n');if(ȯ>=ѝ+Ă-1) ѝ++;ѝ=Math.Min(Θ.Count()-1-Ă,ѝ);if(ȯ<ѝ+1) ѝ--;
ѝ=Math.Max(0,ѝ);String έ="";for(int i=0;i<Ă;i++){int ΓΪ=i+ѝ;if(ΓΪ>=Θ.Count()) break;έ+=Θ[ΓΪ]+"\n";}return έ;}String Βγ(int Ў,int į,String Β){String[]Θ=Β.Split('\n');int Αο=Ў*į;int ŧ=(Ў+1)*į;String έ="";for(int i=Αο;i<ŧ;i++){if(i>=Θ.Count()) break;έ+=Θ[i]+"\n";
}return έ;}String ƍ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Βς(String s,int Γή){if(s=="") return s;if(s.Length>Γή) s=s.Substring(0,Γή-1)+".";return s;}string ƪ(String Ƕ,float value,float ű,int Βχ,int ΓΦ,int ΖΨ){float ǽ=С(value,ű)*Βχ;
String Ŗ="[";for(int i=0;i<Βχ;i++){if(i<=ǽ) Ŗ+="|";else Ŗ+="'";}Ŗ+="]";return Ŗ+" "+Βς(ƍ(Ƕ),ΓΦ).PadRight(ΓΦ)+"".PadRight(ΖΨ)+ΑЏ(value);}String ΑЏ(float Ј){if(Ј>=1000000) return Math.Round(Ј/1000000f,Ј/1000000f<100?1:0)+"M";if(Ј>=1000) return Math.Round(Ј/1000f,Ј/1000f<100?1:0)+"K";
return ""+Math.Round(Ј);}String Ɨ(int İ){if(İ>=60*60) return Math.Round(İ/(60f*60f),1)+" h";if(İ>=60) return Math.Round(İ/60f,1)+" min";return ""+İ+" s";}List<IMyCameraBlock>Ευ=new List<IMyCameraBlock>();bool ΖΪ=false;int Εχ=0,ΖБ=0,ΖЏ=0;String Εσ="";String Ετ="";
void ΔΑ(){ΖΪ=false;Εχ=0;Εσ="";}const String Εί="[PAMTFC]";long ΖΡ=0;String з="";int ǻ=-1;List<int>ΑΗ=new List<int>();String ΓΚ="";void ΔΠ(){String И=к+"|"+й+"|"+(int) Ȟ;if(И==ΓΚ) return;ΓΚ=И;ΑІ(к,й,Ȟ,ΑΗ);}void ΑІ(int w,int h,У Αο,List<int>ǃ){ǃ.Clear();if(Αο==У.Ő)
{for(int k=0;k<w*h;k++){int ǣ=k/w;ǃ.Add(ǣ*w+(ǣ%2==0?k-ǣ*w:w-1-(k-ǣ*w)));}return;}int ΓΘ=(int) Math.Ceiling(w/2f),ΓΗ=(int) Math.Ceiling(h/2f);int ΕЎ=w/2,ΕЍ=h/2;int x=0,y=0,ǈ=0,ǉ=-1,ű=Math.Max(w,h);for(int i=0;i<ű*ű;i++){if(-ΓΘ<x&&x<=ΕЎ&&-ΓΗ<y&&y<=ΕЍ) ǃ.Add((y-1+ΓΗ)*w+(x-1+ΓΘ));
if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ǈ;ǈ=-ǉ;ǉ=t;}x+=ǈ;y+=ǉ;}}bool ΔΟ(){int n=Math.Max(0,к*й);if(з.Length==n) return false;з=new String('0',n);ǻ=-1;return true;}void ΕΞ(int Ж,bool κ){if(Ж<0||Ж>=з.Length) return;char[]h=з.ToCharArray();
h[Ж]=κ?'1':'0';з=new String(h);}const String Δϋ="[PAMJOB]";const String Εέ="[PAMTCC]";class Ȑ{public long Ν;public Vector3 Φ,М,Ђ;}List<Ȑ>В=new List<Ȑ>();bool Ę=false;Vector3 Ť,ţ,ť,Ŧ;long ĥ=0;long Γη=0;ά Γζ=null;bool ΕϏ=false;ά ΔΜ(Vector3 Φ,Vector3 М,Vector3 Ђ){MatrixD m=MatrixD.CreateWorld(Φ,М,Ђ);
ά w=new ά(Σ);w.І=Vector3D.TransformNormal(Ť,m);w.Ѐ=Vector3D.TransformNormal(ţ,m);w.О=Vector3D.TransformNormal(ť,m);w.Π=Φ+(Vector3) Vector3D.TransformNormal(Ŧ,m);w.ν=true;return w;}void Δό(){if(Ρ==Ο.ο||!Σ.ν) return;IMyShipConnector ї=ȋ(MyShipConnectorStatus.Connected);if(ї==null||ї.OtherConnector==null) return;
IMyShipConnector Й=ї.OtherConnector;Vector3I ΕБ=new Vector3I((int) Σ.Ч.X,(int) Σ.Ч.Y,(int) Σ.Ч.Z);if(ї.Position==ΕБ){MatrixD ŭ=MatrixD.Transpose(Й.WorldMatrix);Ť=Vector3D.TransformNormal(Њ.WorldMatrix.Forward,ŭ);ţ=Vector3D.TransformNormal(Њ.WorldMatrix.Down,ŭ);ť=Vector3D.TransformNormal(Њ.WorldMatrix.Left,ŭ);
Ŧ=Vector3D.TransformNormal(ї.GetPosition()-Й.GetPosition(),ŭ);Ę=true;}if(Vector3.Distance(ї.GetPosition(),Σ.Π)<5) ĥ=Й.EntityId;List<IMyShipConnector>ǳ=new List<IMyShipConnector>();Н.GetBlocksOfType(ǳ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ǳ.Count>0){В.Clear();for(int i=0;i<ǳ.Count;i++){Ȑ p=new Ȑ();p.Ν=ǳ[i].EntityId;p.Φ=ǳ[i].GetPosition();p.М=ǳ[i].WorldMatrix.Forward;p.Ђ=ǳ[i].WorldMatrix.Up;В.Add(p);}}Γζ=null;Γη=0;if(Й.EntityId==ĥ||!Ę) return;for(int i=0;i<В.Count;i++)
if(В[i].Ν==Й.EntityId){Γη=В[i].Ν;Γζ=ΔΜ(В[i].Φ,В[i].М,В[i].Ђ);}}String ΔΛ(){String s=(Ę?"1":"0")+":"+β(Ť)+":"+β(ţ)+":"+
β(ť)+":"+β(Ŧ)+":"+ĥ;for(int i=0;i<В.Count;i++)
s+=":"+В[i].Ν+","+β(В[i].Φ)+","+β(В[i].М)+","+β(В[i].Ђ);return s;}void ΔΚ(String s){В.Clear();Ę=false;ĥ=0;String[]f=s.Split(':');if(f.Length<6) return;Ę=f[0]=="1";Ť=Ϋ(f[1]);ţ=Ϋ(f[2]);ť=Ϋ(f[3]);Ŧ=Ϋ(f[4]);long.TryParse(f[5],out ĥ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ȑ p=new Ȑ();if(!long.TryParse(e[0],out p.Ν)) continue;p.Φ=Ϋ(e[1]);p.М=Ϋ(e[2]);p.Ђ=Ϋ(e[3]);В.Add(p);}}const String Ə="[PAMDCK]";const float ΓЌ=120f;class ѐ{public long Γ;public int Ǟ;public long и;public bool Βό;public DateTime Л,ȟ;
}IMyBroadcastListener Ţ=null;List<ѐ>ϋ=new List<ѐ>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMySensorBlock>б=new List<IMySensorBlock>();List<IMyCameraBlock>ћ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ƈ=new List<MyDetectedEntityInfo>();long Б=0;int ř=0;
long Ř=0;DateTime Βύ=DateTime.Now;String љ="";int ΒЎ=0;IMyCubeGrid џ=null;void ΕΗ(){џ=Me.CubeGrid;Н.GetBlocksOfType(ў,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(dockPoolTag));Н.GetBlocksOfType(б,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(zoneTag));Н.GetBlocksOfType(ћ,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(laneTag));
for(int i=0;i<б.Count;i++){б[i].DetectSmallShips=true;б[i].DetectLargeShips=true;}for(int i=0;i<ћ.Count;i++) ћ[i].EnableRaycast=true;}IMyShipConnector Ƴ(long Ν){if(Ν==0) return null;IMyShipConnector c=Н.GetBlockWithId(Ν) as IMyShipConnector;return c!=null&&џ!=null&&c.CubeGrid.IsSameConstructAs(џ)?c:null;
}bool ƞ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΒΥ(IMyShipConnector c,long Γ){for(int i=0;i<ћ.Count;i++){IMyCameraBlock ĕ=ћ[i];if(!ĕ.IsWorking||Vector3D.Distance(ĕ.GetPosition(),c.GetPosition())>10) continue;
Vector3D ŧ=ĕ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ĕ.CanScan(ŧ)) continue;MyDetectedEntityInfo Ũ=ĕ.Raycast(ŧ);if(!Ũ.IsEmpty()&&Ũ.EntityId!=џ.EntityId&&Ũ.EntityId!=Γ&&Ũ.Type!=MyDetectedEntityType.FloatingObject){љ="Lane blocked: "+c.CustomName;return false;}
}return true;}bool Δω(){for(int s=0;s<б.Count;s++){if(!б[s].IsWorking) continue;ƈ.Clear();б[s].DetectedEntities(ƈ);for(int i=0;i<ƈ.Count;i++){MyDetectedEntityInfo e=ƈ[i];bool ΕЊ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;if(ΕЊ&&e.EntityId!=Б&&e.Velocity.Length()>0.5f)
{љ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΔЋ(ѐ r){IMyShipConnector и=Ƴ(r.и);if(ƞ(и)&&ΒΥ(и,r.Γ)) return и.EntityId;if(!r.Βό||и==null) return 0;IMyShipConnector Ѝ=null;double Βϊ=0;for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];if(!ƞ(c)) continue;
double d=Vector3D.Distance(c.GetPosition(),и.GetPosition());if(Ѝ==null||d<Βϊ){if(ΒΥ(c,r.Γ)){Ѝ=c;Βϊ=d;}}}return Ѝ==null?0:Ѝ.EntityId;}void Δμ(){ѐ ȧ=null;long ї=0;for(int ź=2;ź>=1&&ȧ==null;ź--){ѐ Ǽ=null;for(int i=0;i<ϋ.Count;i++){ѐ r=ϋ[i];if(r.Ǟ!=ź) continue;
if(ź==1){long c=ΔЋ(r);if(c==0) continue;if(Ǽ==null||r.Л<Ǽ.Л){Ǽ=r;ї=c;}}else if(Ǽ==null||r.Л<Ǽ.Л) Ǽ=r;}ȧ=Ǽ;}if(ȧ==null){if(ϋ.Count>0&&љ=="") љ="No free dock";return;}Б=ȧ.Γ;ř=ȧ.Ǟ;Ř=ȧ.Ǟ==1?ї:0;Βύ=DateTime.Now;}void Εδ(){if(џ==null) return;
if(Ţ==null) Ţ=IGC.RegisterBroadcastListener(Ə);int Ĕ=30;while(Ţ.HasPendingMessage&&Ĕ-->0){String Ξ=Ţ.AcceptMessage().Data as String;if(Ξ!=null) Δξ(Ξ);}ΕΔ();if(++ΒЎ<3) return;ΒЎ=0;IGC.SendBroadcastMessage(Ə,ΓЊ());}void Δξ(String Ξ){try{String[]f=Ξ.Split(';');long Γ=long.Parse(f[1]);
if(f[0]=="D"){for(int i=ϋ.Count-1;i>=0;i--) if(ϋ[i].Γ==Γ) ϋ.RemoveAt(i);if(Б==Γ) Б=0;}else if(f[0]=="R"&&f.Length>=5){long и=long.Parse(f[3]);if(Ƴ(и)==null) return;ѐ r=null;for(int i=0;i<ϋ.Count;i++) if(ϋ[i].Γ==Γ) r=ϋ[i];if(r==null){r=new ѐ();r.Γ=Γ;r.Л=DateTime.Now;ϋ.Add(r);}
int Ǟ=int.Parse(f[2]);if(r.Ǟ!=Ǟ) r.Л=DateTime.Now;r.Ǟ=Ǟ;r.и=и;r.Βό=f[4]=="1";r.ȟ=DateTime.Now;}}catch{}}void ΕΔ(){for(int i=ϋ.Count-1;i>=0;i--)
if((DateTime.Now-ϋ[i].ȟ).TotalSeconds>3) ϋ.RemoveAt(i);ѐ ŗ=null;for(int i=0;i<ϋ.Count;i++) if(ϋ[i].Γ==Б) ŗ=ϋ[i];if(Б!=0&&ŗ==null) Б=0;if(ŗ!=null&&(DateTime.Now-Βύ).TotalSeconds>ΓЌ){Б=0;ŗ.Л=DateTime.Now;}љ="";if(Б==0&&ϋ.Count>0&&!Δω()) Δμ();}String ΓЊ()
{String ε="S;"+џ.EntityId+";"+Б+";"+ř+";"+Ř+";";for(int i=0;i<ϋ.Count;i++) ε+=(i>0?",":"")+ϋ[i].Γ;ε+=";";for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];ε+=(i>0?":":"")+c.EntityId+","+β(c.GetPosition())+","+β(c.WorldMatrix.Forward)+","+β(c.WorldMatrix.Up);
}ε+=";";for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ε+=(i>0?":":"")+β(z.GetPosition())+","+Math.Round(r,1);}return ε;}String Δί()
{if(ў.Count==0&&б.Count==0) return "";int ΓΒ=0;for(int i=0;i<ў.Count;i++) if(ƞ(ў[i])) ΓΒ++;return "Docks: "+ΓΒ+"/"+ў.Count+" free"+(Б!=0?(ř==1?" | docking":" | undocking"):"")+
(ϋ.Count>1?" | "+(ϋ.Count-1)+" waiting":"")+(љ!=""?"\n"+љ:"")+"\n";}int ΒЍ=0;bool ΖϏ=false;void ΔЏ(){if(ΒЍ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ə,"D;"+ΖΡ);ΒЍ=0;ΖϏ=false;}const float Δϊ=600f;class п{public long Ν;public int Ж=-1;public bool ΓЀ;public float ΑΑ,ǌ;
public DateTime Αζ;}class г{public String И;public int w,h;public float Αω,Ατ;public String κ;public List<int>Ź=new List<int>();public List<п>Ћ=new List<п>();}List<г>З=new List<г>();IMyBroadcastListener Ű=null;int ΓΥ=0;void Εε(){if(Ű==null) Ű=IGC.RegisterBroadcastListener(Εί);int Ĕ=50;
while(Ű.HasPendingMessage&&Ĕ-->0){String Ξ=Ű.AcceptMessage().Data as String;if(Ξ==null) continue;try{Δπ(Ξ);}catch{}try{Δρ(Ξ);}catch{}}for(int j=З.Count-1;j>=0;j--){г Η=З[j];for(int i=Η.Ћ.Count-1;i>=0;i--)
if((DateTime.Now-Η.Ћ[i].Αζ).TotalSeconds>Δϊ) Η.Ћ.RemoveAt(i);if(Η.Ћ.Count==0){З.RemoveAt(j);continue;}ΓЅ(Η);}if(++ΓΥ<3) return;ΓΥ=0;for(int j=0;j<З.Count;j++) IGC.SendBroadcastMessage(Δϋ,ΓЇ(З[j]));}void ΓЅ(г Η){for(int i=0;i<Η.Ћ.Count;i++){п s=Η.Ћ[i];if(s.ΓЀ&&(s.Ж<0||Η.κ[s.Ж]=='1')) s.Ж=ΔϏ(Η,s);
}}String ΓЇ(г Η){String ε="J;"+Η.И+";"+Η.κ+";";bool Л=true;for(int i=0;i<Η.Ћ.Count;i++){if((DateTime.Now-Η.Ћ[i].Αζ).TotalSeconds>5) continue;ε+=(Л?"":",")+Η.Ћ[i].Ν+":"+Η.Ћ[i].Ж;Л=false;}return ε;}void Δπ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;г Η=null;for(int i=0;i<З.Count;i++) if(З[i].И==f[4]) Η=З[i];if(Η==null){Η=new г();Η.И=f[4];Η.w=int.Parse(k[3]);Η.h=int.Parse(k[4]);Η.Αω=float.Parse(k[6]);Η.Ατ=float.Parse(k[7]);if(Η.w<1||Η.h<1) return;ΑІ(Η.w,Η.h,(У) int.Parse(k[5]),Η.Ź);
Η.κ=new String('0',Η.w*Η.h);З.Add(Η);}String d=f[6];if(d.Length==Η.κ.Length){char[]c=Η.κ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Η.κ=new String(c);}long Ν=long.Parse(f[0]);п s=null;for(int i=0;i<Η.Ћ.Count;i++) if(Η.Ћ[i].Ν==Ν) s=Η.Ћ[i];
if(s==null){s=new п();s.Ν=Ν;Η.Ћ.Add(s);}s.Αζ=DateTime.Now;String[]w=f[10].Split(',');s.ΓЀ=w[0]=="1";if(w.Length>=3){s.ΑΑ=float.Parse(w[1]);s.ǌ=float.Parse(w[2]);}int Ĥ=int.Parse(f[5]);if(Ĥ>=0&&Ĥ<Η.κ.Length&&(s.Ж<0||Η.κ[s.Ж]=='1')&&Η.κ[Ĥ]!='1'&&ƚ(Η,Ĥ,s)==null)
s.Ж=Ĥ;}п ƚ(г Η,int Ж,п ΕЃ){for(int i=0;i<Η.Ћ.Count;i++){п o=Η.Ћ[i];if(o!=ΕЃ&&o.Ж==Ж)
return o;}return null;}int ΔϏ(г Η,п s){for(int k=0;k<Η.Ź.Count;k++){int c=Η.Ź[k];if(Η.κ[c]!='1'&&ƚ(Η,c,s)==null) return c;}return-2;}String Δε(){String s="";for(int j=0;j<З.Count;j++){г Η=З[j];int κ=0;for(int i=0;i<Η.κ.Length;i++) if(Η.κ[i]=='1') κ++;s+="Job "+(j+1)+": "+κ+"/"+Η.κ.Length+" holes, "+Η.Ћ.Count+" ships\n";
}return s;}const float Εά=8f;class Є{public long Ν;public Vector3 Φ,ю,Ģ;public float Ș,ΑΑ,ǌ,ч=-1;public int ǵ;public String Η="";public int Αέ;public DateTime ȁ;}class ŏ{public long a,b,ΑЂ;public DateTime ȟ;}List<Є>Ϊ=new List<Є>();List<ŏ>Г=new List<ŏ>();int Ασ=0;void Δρ(String Ξ)
{String[]f=Ξ.Split(';');if(f.Length<9) return;long Ν=long.Parse(f[0]);Є s=null;for(int i=0;i<Ϊ.Count;i++) if(Ϊ[i].Ν==Ν) s=Ϊ[i];if(s==null){s=new Є();s.Ν=Ν;Ϊ.Add(s);}s.Φ=Ϋ(f[1]);s.ю=Ϋ(f[2]);s.Ș=float.Parse(f[3]);s.Η=f[4];s.Αέ=int.Parse(f[7].Split(',')[1]);s.Ģ=Ϋ(f[8]);
s.ǵ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΑ=float.Parse(w[1]);s.ǌ=float.Parse(w[2]);s.ǵ=int.Parse(w[3]);}}s.ȁ=DateTime.Now;}bool ċ(Є s){return s.ю.LengthSquared()<0.25f&&(s.Αέ==3||s.Ģ.LengthSquared()<0.25f);}Vector3 Βο(Є s){return s.ю.LengthSquared()>=1f?s.ю:s.Ģ*2f;}
void Εθ(){ΔΓ();if(++Ασ<3) return;Ασ=0;IGC.SendBroadcastMessage(Εέ,ΓЋ());}void ΔΓ(){for(int i=Ϊ.Count-1;i>=0;i--)
if((DateTime.Now-Ϊ[i].ȁ).TotalSeconds>3) Ϊ.RemoveAt(i);for(int i=Г.Count-1;i>=0;i--)
if((DateTime.Now-Г[i].ȟ).TotalSeconds>2) Г.RemoveAt(i);for(int i=0;i<Ϊ.Count;i++) Ϊ[i].ч=-1;for(int i=0;i<Ϊ.Count;i++)
for(int j=i+1;j<Ϊ.Count;j++){Є a=Ϊ[i],b=Ϊ[j];if(ċ(a)||ċ(b)) continue;Vector3 ΖΧ=a.Φ+a.ю*(float)(DateTime.Now-a.ȁ).TotalSeconds;Vector3 Ζά=b.Φ+b.ю*(float)(DateTime.Now-b.ȁ).TotalSeconds;Vector3 Ā=Βο(a),ā=Βο(b);Vector3 Ȫ=Ζά-ΖΧ;float т=Ȫ.Length();float ѕ=(a.Ș+b.Ș)*0.55f;
bool Γπ=a.Η!=""&&a.Η==b.Η;if(Γπ&&(a.ǵ==2||b.ǵ==2)) continue;if(Γπ&&a.ǵ==1&&b.ǵ==1){String[]k=a.Η.Split('|');if(k.Length>=8) ѕ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Αγ=ā-Ā;float Γο=Αγ.LengthSquared();float t=Γο>0.01f?-Vector3.Dot(Ȫ,Αγ)/Γο:0;
bool Αχ=т<ѕ;if(!Αχ&&(t<=0||t>Εά||(Ȫ+Αγ*t).Length()>=ѕ)) continue;if(!Αχ&&Ā.LengthSquared()>0.25f&&ā.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ā),Vector3.Normalize(ā))<-0.7f)
continue;ŏ p=null;for(int k=0;k<Г.Count;k++)
if((Г[k].a==a.Ν&&Г[k].b==b.Ν)||(Г[k].a==b.Ν&&Г[k].b==a.Ν)) p=Г[k];if(p==null){p=new ŏ();p.a=a.Ν;p.b=b.Ν;p.ΑЂ=Εή(a,b,Ā,ā,Ȫ,т);Г.Add(p);}p.ȟ=DateTime.Now;Є y=p.ΑЂ==a.Ν?a:b;float ΓΩ=Αχ||t<4?0:Math.Max(1,y.ю.Length()*0.5f);if(y.ч<0||ΓΩ<y.ч) y.ч=ΓΩ;
}}long Εή(Є a,Є b,Vector3 Ā,Vector3 ā,Vector3 Ȫ,float т){if(a.Ν==Б) return b.Ν;if(b.Ν==Б) return a.Ν;Vector3 n=т>0.01f?Ȫ/т:new Vector3(1,0,0);float Γφ=Vector3.Dot(Ā,n),Γω=Vector3.Dot(ā,-n);if(Γφ-Γω>1) return a.Ν;if(Γω-Γφ>1) return b.Ν;return a.Ν>b.Ν?a.Ν:b.Ν;
}String ΓЋ(){String ε="C;";bool Л=true;for(int i=0;i<Ϊ.Count;i++){if(Ϊ[i].ч<0) continue;ε+=(Л?"":",")+Ϊ[i].Ν+":"+Math.Round(Ϊ[i].ч,1);Л=false;}return ε;}String Δλ(){int Γβ=0,ΑΖ=0;for(int i=0;i<Ϊ.Count;i++){if(!ċ(Ϊ[i])) Γβ++;if(Ϊ[i].ч>=0) ΑΖ++;}if(Ϊ.Count==0) return "";
return "Traffic: "+Γβ+" moving"+(ΑΖ>0?", "+ΑΖ+" giving way":"")+"\n";}List<ί>ф=new List<ί>();ί Щ=null;Vector3 ΓΟ;DateTime ΓΞ;float ΓΝ=40;bool ΓΛ=false;Є ΑЎ(ί s){Є Ѝ=null;for(int i=0;i<Ϊ.Count;i++)
if(Vector3.Distance(Ϊ[i].Φ,s.Π)<15&&(Ѝ==null||
Vector3.Distance(Ϊ[i].Φ,s.Π)<Vector3.Distance(Ѝ.Φ,s.Π))) Ѝ=Ϊ[i];return Ѝ;}float Δς(ί s){Є t=ΑЎ(s);if(t==null) return 0;float k=t.ǵ*1000000f;String[]И=t.Η.Split('|');if(И.Length>=3) k+=Vector3.Distance(t.Φ,new Vector3(float.Parse(И[0]),float.Parse(И[1]),float.Parse(И[2])));return k;
}void ΕΦ(){ф.Clear();Щ=null;List<float>ΑΚ=new List<float>();for(int i=0;i<θ.Count;i++){float k=Δς(θ[i]);int ǭ=0;while(ǭ<ΑΚ.Count&&ΑΚ[ǭ]<=k) ǭ++;ΑΚ.Insert(ǭ,k);ф.Insert(ǭ,θ[i]);}}void Εη(){if(Щ!=null){bool ΕЉ=ΓΛ||Vector3.Distance(Щ.Π,ΓΟ)>ΓΝ||
(DateTime.Now-ΓΞ).TotalSeconds>45||Щ.Ơ();if(!ΕЉ) return;Щ=null;}if(ф.Count==0) return;Щ=ф[0];ф.RemoveAt(0);ΓΟ=Щ.Π;ΓΞ=DateTime.Now;Є t=ΑЎ(Щ);ΓΝ=t==null?40:Math.Max(40,t.Ș*3);ΓΛ=t!=null&&t.Αέ==3&&t.ю.LengthSquared()<0.25f;Щ.Ǖ("HOMEPOS",true,false,false);ƭ("HOMEPOS",Щ.Ν,"HOMEPOS");
}const String ΔΘ="[PAMDBG]";class Ƒ{public readonly bool Ʀ;IMyProgrammableBlock ѓ;Action<IMyProgrammableBlock>ƺ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>ƹ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Ƹ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>ƻ;
Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>Ʒ;public Ƒ(MyGridProgram Ζΰ){ѓ=Ζΰ.Me;var Ǻ=ѓ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ѓ);if(Ǻ==null) return;ƺ=(Action<IMyProgrammableBlock>) Ǻ["RemoveDraw"];ƹ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) Ǻ["Point"];
Ƹ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) Ǻ["Line"];ƻ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) Ǻ["Sphere"];Ʒ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) Ǻ["GPS"];((Action<IMyProgrammableBlock>) Ǻ["RemoveAll"])(ѓ);
Ʀ=true;}public void ΔА(){if(ƺ!=null) ƺ(ѓ);}public void ǔ(Vector3D a,Vector3D b,Color c,float Ζτ){if(Ƹ!=null) Ƹ(ѓ,a,b,c,Ζτ,-1,false);}public void ΔЌ(Vector3D a,Color c,float ы){if(ƹ!=null) ƹ(ѓ,a,c,ы,-1,false);}public void Ȏ(String Β,Vector3D ǭ,Color c){if(Ʒ!=null) Ʒ(ѓ,Β,ǭ,c,-1);}
public void Βμ(Vector3D ȕ,double ы,Color c){if(ƻ!=null) ƻ(ѓ,new BoundingSphereD(ȕ,ы),c,1,0.05f,30,-1,false);}}class Ą{public long Ν;public Vector3 ĩ,ΑΞ,ę,ΰ;public bool ΓΙ;public List<Vector3>ż=new List<Vector3>(),Αί=new List<Vector3>();public List<bool>Αή=new List<bool>();
public DateTime ȁ;}Ƒ υ=null;IMyBroadcastListener Š=null;List<Ą>с=new List<Ą>();void Δν(String Ξ){String[]f=Ξ.Split(';');if(f.Length<6) return;long Ν=long.Parse(f[0]);Ą s=null;for(int i=0;i<с.Count;i++) if(с[i].Ν==Ν) s=с[i];if(s==null){s=new Ą();s.Ν=Ν;с.Add(s);}
s.ĩ=Ϋ(f[1]);s.ΑΞ=Ϋ(f[2]);s.ę=Ϋ(f[3]);s.ΓΙ=s.ΑΞ.LengthSquared()>0.5f;s.ΰ=Ϋ(f[4]);s.ȁ=DateTime.Now;if(f[5]=="") return;s.ż.Clear();s.Αί.Clear();s.Αή.Clear();String[]Γλ=f[5].Split(':');for(int i=0;i<Γλ.Length;i++){String[]r=Γλ[i].Split(',');if(r.Length<3) continue;s.ż.Add(Ϋ(r[0]));s.Αί.Add(Ϋ(r[1]));s.Αή.Add(r[2]=="1");
}}void Εγ(){if(!debugDraw) return;if(υ==null) υ=new Ƒ(this);if(!υ.Ʀ) return;if(Š==null) Š=IGC.RegisterBroadcastListener(ΔΘ);int Ĕ=20;while(Š.HasPendingMessage&&Ĕ-->0){String Ξ=Š.AcceptMessage().Data as String;if(Ξ!=null) try{Δν(Ξ);}catch{}}for(int i=с.Count-1;i>=0;i--) if((DateTime.Now-с[i].ȁ).TotalSeconds>5) с.RemoveAt(i);
υ.ΔА();for(int i=0;i<Ϊ.Count;i++){Є s=Ϊ[i];Color c=ċ(s)?Color.Gray:s.ч>=0?Color.Yellow:Color.Lime;υ.Βμ(s.Φ,s.Ș/2,c);υ.Ȏ(Ŏ(s)+": "+(ċ(s)?"parked":s.ч==0?"holding (giving way)":s.ч>0?"slowing (giving way)":"moving"),s.Φ,c);if(s.ю.LengthSquared()>0.25f) υ.ǔ(s.Φ,s.Φ+s.ю*3,Color.White,0.1f);
if(s.Ģ.LengthSquared()>0.25f) υ.ǔ(s.Φ,s.Φ+s.Ģ*s.Ș,Color.Cyan,0.1f);}for(int i=0;i<Г.Count;i++){if((DateTime.Now-Г[i].ȟ).TotalSeconds>0.5) continue;Є a=null,b=null;for(int k=0;k<Ϊ.Count;k++){if(Ϊ[k].Ν==Г[i].a) a=Ϊ[k];if(Ϊ[k].Ν==Г[i].b) b=Ϊ[k];}if(a==null||b==null) continue;
υ.ǔ(a.Φ,b.Φ,Color.Red,0.2f);υ.Ȏ("Conflict: "+Ŏ(Г[i].ΑЂ==a.Ν?a:b)+" gives way",(a.Φ+b.Φ)/2,Color.Red);}for(int i=0;i<с.Count;i++){Ą d=с[i];Є t=null;for(int k=0;k<Ϊ.Count;k++) if(Ϊ[k].Ν==d.Ν) t=Ϊ[k];if(t!=null) υ.ǔ(t.Φ,d.ΰ,Color.Blue,0.08f);for(int r=0;r<d.ż.Count;r++) υ.ǔ(d.ż[r],d.Αί[r],d.Αή[r]?Color.Red:Color.Green,0.04f);
if(t==null||!d.ΓΙ) continue;г Η=null;for(int j=0;j<З.Count;j++) if(З[j].И==t.Η) Η=З[j];if(Η==null) continue;int ΒЏ=0;for(int c=0;c<Η.κ.Length;c++) if(Η.κ[c]=='1') ΒЏ++;if(i==0||с[i-1].ĩ!=d.ĩ) υ.Ȏ("Job: "+ΒЏ+"/"+Η.κ.Length+" holes done",d.ĩ,Color.White);
for(int c=0;c<Η.κ.Length;c++){int Ǆ=c%Η.w,ǣ=c/Η.w;Vector3 p=d.ĩ+d.ΑΞ*((Η.w-1f)/2f-Ǆ)*Η.Αω-d.ę*((Η.h-1f)/2f-ǣ)*Η.Ατ;п ΑΫ=ƚ(Η,c,null);υ.ΔЌ(p,Η.κ[c]=='1'?Color.Gray:ΑΫ!=null?Color.Orange:Color.White,Math.Min(Η.Αω,Η.Ατ)*0.2f);if(ΑΫ==null||ΑΫ.Ν!=d.Ν) continue;
υ.ǔ(t.Φ,p,Color.Orange,0.05f);υ.Ȏ("Hole "+(Η.Ź.IndexOf(c)+1)+": "+Ŏ(t),p,Color.Orange);}}for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];υ.Βμ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);
υ.Ȏ("Docking area"+(љ!=""?": "+љ:""),z.GetPosition(),Color.Cyan);}for(int i=0;i<ћ.Count;i++)
υ.ǔ(ћ[i].GetPosition(),ћ[i].GetPosition()+ћ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(Б!=0&&Ř!=0){IMyShipConnector c=Ƴ(Ř);for(int k=0;k<Ϊ.Count;k++)
if(c!=null&&Ϊ[k].Ν==Б){υ.ǔ(Ϊ[k].Φ,c.GetPosition(),Color.Lime,0.15f);υ.Ȏ("Cleared to "+(ř==2?"undock: ":"dock: ")+Ŏ(Ϊ[k]),c.GetPosition(),Color.Lime);}}}String Ŏ(Є t){ί Ѝ=null;for(int i=0;i<θ.Count;i++)
if(Vector3.Distance(θ[i].Π,t.Φ)<30&&(Ѝ==null||
Vector3.Distance(θ[i].Π,t.Φ)<Vector3.Distance(Ѝ.Π,t.Φ))) Ѝ=θ[i];if(Ѝ!=null&&Ѝ.ζ!="") return Ѝ.ζ;String Ν=t.Ν.ToString();return "Ship "+Ν.Substring(Math.Max(0,Ν.Length-4));}String Δέ(){if(!debugDraw) return "";if(υ==null||!υ.Ʀ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}void ΕΣ(string А){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}