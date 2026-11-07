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

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
const float drillRadius = 1.4f;//[m] grinders and modded drills (vanilla drills use their real cut radius)

//minimum acceleration in space before ship becomes too heavy
const float minAccelerationSmall = 0.2f;//[m/s²] small ship
const float minAccelerationLarge = 0.1f;//[m/s²] large ship

//LCD
const bool setLCDFontAndSize = true;

//Traffic between PAM ships (needs an antenna): lanes, spacing, right of way, co-op holes (IMPORT JOB)
const bool trafficEnabled = true;

//3D debug drawing (offline world + Digi's "(DevTool) Programmable Block DebugAPI" mod), see README
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ο{Ǚ,Ƭ,Ȓ,Е,ο}public enum Ν{ǫ,Ɲ,Ββ,Ʋ,Βν,Ċ,Ǖ,
Ɯ,Δί,Ȏ,ΒΑ,ΓЉ,ΑЋ,
ΕΜ,Ē,đ,Ɣ,Ευ,ΔΪ,
Εσ,Ετ,ΔΫ,Δά,ē}public enum Τ{ё,Ư,Ǫ,ΒΒ,Ņ,Βγ,Βδ,Ł,ł}public enum Ǒ{None,Ɣ,ǫ,ΔΠ}public enum Κ{Main,Βλ,Č,ċ,ƻ,ń,
Ɛ,ΒΦ,ΒΧ,ΒΥ,ď,Βο,
Ŏ,Ƶ,Ƹ,Βς,Œ}public enum ǘ{Βξ,ΔΛ,Εή,ΔϏ,ο}public enum У{ŕ,ΔΖ}public enum ǔ{Βμ,Δή,Εα,Ƣ}public enum в{Βζ,ΔΞ,ΔΝ,ΔΦ,ΔΥ,Δύ,Δό}public enum я{Ƙ,ΓЍ,ΑЌ}public enum ŉ{Δϋ,ΗΔ,ΓЊ}public enum Ѕ{Βη,Βθ,Βι,ΕΑ,ΔБ,
ΔЌ,ΔЍ,ΔЋ,ΔЏ,
ΔА,ΔЎ}public enum Υ{о,ђ,н,Ń,Ƒ,ƒ}public enum п{Βυ,ΒΓ,ΔЊ,Βε}public enum ι{None,Ȕ,č,Ō}public enum τ{ƽ,Ə,Ʃ,Ƣ,Βΰ,ƫ,Βα,Εκ,Ʈ}public enum ϊ{Ǚ,Βή,Βέ,ƪ,ƣ,Ɩ,Βί}class ά{public bool ν=false;public Vector3 Π=new Vector3();public Vector3 Ё=new Vector3();
public Vector3 І=new Vector3();public Vector3 О=new Vector3();public Vector3 π=new Vector3();public Vector3 Ч=new Vector3();public float ΖΩ=0;public float ΗΛ=0;public float[]Я=null;public ά(){}public ά(ά Й){ν=Й.ν;Π=Й.Π;Ё=Й.Ё;І=Й.І;О=Й.О;π=Й.π;Ч=Й.Ч;Я=Й.Я;}public ά(Vector3 Π,Vector3 І,Vector3 Ё,Vector3 О,Vector3 π)
{this.Π=Π;this.Ё=Ё;this.І=І;this.О=О;this.ΖΩ=0;this.π=π;}public void ΗΓ(List<IMyThrust>ř,List<string>ǎ){Я=new float[ǎ.Count];for(int i=0;i<Я.Length;i++)
Я[i]=-1;for(int i=0;i<ř.Count;i++){string Ȋ=Δρ(ř[i]);int Α=ǎ.IndexOf(Ȋ);if(Α!=-1)
Я[Α]=С(ř[i].MaxEffectiveThrust,ř[i].MaxThrust);}}}class Ŗ{public Ѕ П=Ѕ.Βη;public float е=0;public float Ǵ=0;public string ф="";public string ч="";DateTime Γί;public bool Ζζ=false;private bool ǌ=false;public bool ΔΗ(bool Αι){if(Αι){Ǵ=0;ǌ=false;return false;}ǌ=true;return Ǵ>е;
}public void ΗΕ(){ΔΗ(true);Ζζ=false;}public void ΗΖ(){if(ǌ)
if((DateTime.Now-Γί).TotalSeconds>1){Ǵ++;Γί=DateTime.Now;}}public bool ΒΣ(){switch(П){case Ѕ.Βθ:return true;case Ѕ.Βι:return true;}return false;}}class Ǭ{public String ζ="";public String Ќ="";public int Ј=0;public ŉ ΑΩ=ŉ.ΓЊ;public Ǭ(String ζ,String Ќ,int Ј,ŉ ΑΩ){this.ζ=ζ;this.Ќ=Ќ;
this.Ј=Ј;this.ΑΩ=ΑΩ;}}class ί{public DateTime ΑΥ;public DateTime ΒЈ;public String Λ="";public String ζ="";public String Ɗ="";public String Ϊ="";public String Αψ="";public String Ż="";public String Αδ="";public Vector3 Π=new Vector3();public List<Ǭ>њ=new List<Ǭ>();public Ο Ρ=Ο.Ǚ;
public ι Ǜ;public float ǽ;public float Ǳ;public int ς;public int ΑΒ=0;public int ΑЀ=0;public bool Ƨ(){return(DateTime.Now-ΑΥ).TotalSeconds>10;}public bool Ƥ(){if(Ǜ!=ι.Ȕ) return false;return(DateTime.Now-ΒЈ).TotalSeconds>=4;}public ί(String Λ){this.Λ=Λ;}public ι Ǘ(String μ,bool Ζτ,bool ѕ,bool ΕЀ)
{if(μ==""&&!ѕ) return ι.None;if(Ǜ==ι.Ȕ&&Ƥ())
Ǜ=ι.Ō;if(Ζτ){Αδ=μ;Ǜ=ι.Ȕ;ΒЈ=DateTime.Now;return Ǜ;}else if(ѕ){Αδ="";Ǜ=ι.None;}else if(Αδ==μ){if(ΕЀ) Ǜ=ι.č;return Ǜ;}return ι.None;}}public String Δθ(String ΖΠ){switch(ΖΠ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Н;Vector3 Γσ=new Vector3();Vector3 м=new Vector3();Vector3 ΕБ=new Vector3();Vector3 ΖΤ=new Vector3();Vector3 ΕЊ=new Vector3();DateTime Γή=new DateTime();
bool Ĥ=true;int Αύ=0;bool ĭ=false;bool ǟ=false;bool Ζέ=true;bool ΖΜ=false;float Ζφ=0;float Γϋ=0;int Ζξ=0;int ƃ=0;float ȧ=0;float ș=0;double Αή=0;float ĕ=0;List<int>Ȣ=new List<int>();List<int>Ȳ=new List<int>();Ο Ρ=Ο.Ǚ;int Αο=0;float ǐ=0;float Ǐ=0;int ŵ=0;float Ǻ=0,ǹ=0;
float Ă=0;float Γϊ=0,Γω=0;IMyRemoteControl Њ;IMySensorBlock ώ;List<IMyTimerBlock>ΓЂ=new List<IMyTimerBlock>();List<IMyShipConnector>Ĝ=new List<IMyShipConnector>();List<IMyThrust>ĺ=new List<IMyThrust>();List<IMyGyro>ΓΝ=new List<IMyGyro>();List<IMyTerminalBlock>а=new List<IMyTerminalBlock>();
List<IMyLandingGear>ΑΤ=new List<IMyLandingGear>();List<IMyReactor>Αθ=new List<IMyReactor>();List<IMyConveyorSorter>Ζω=new List<IMyConveyorSorter>();List<IMyGasTank>ī=new List<IMyGasTank>();List<IMyTerminalBlock>Ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΒϏ=new List<IMyBatteryBlock>();
List<IMyTextPanel>Љ=new List<IMyTextPanel>();List<IMyTextSurface>ь=new List<IMyTextSurface>();List<IMyTextPanel>ǳ=new List<IMyTextPanel>();IMyTerminalBlock ǥ=null;ά Σ=new ά();ά ΓΤ=new ά();ά ΓΫ=new ά();bool ǩ=false;bool Ľ=false;bool ľ=false;bool ΓΩ=false;List<String>ǎ=new List<string>();
List<ά>ω=new List<ά>();List<ά>Ƌ=new List<ά>();double ΖЋ=0;ά Ψ=new ά();Vector3 ı;Vector3 Ȩ;Τ φ=Τ.ё;У ȣ=У.ŕ;int к=0;int й=0;double Ŵ=0;bool ΒЎ=false;Ŗ χ=new Ŗ();Ŗ ψ=new Ŗ();ǔ ǣ=ǔ.Βμ;bool Ŀ=true;У ȇ=У.ŕ;int Ц=3;int Х=3;int д=30;я ј=я.Ƙ;bool Ȍ=true;bool Ĭ=true;
bool Ģ=true;bool ȉ=false;в ġ=в.Βζ;bool Ǩ=true;float ш=90;float щ=20;float Ы=5;float ъ=20;float ǚ=0.70f;float ƍ=1.50f;float ƌ=2.50f;float ŀ=0f;float Ĩ=0f;Ν Ť;Τ ǡ;Ν ĸ;int Ů=0;int ů=0;int с=0;int į=0;int Αμ=30;bool Ζϊ=true;double ΓΒ=0;bool Γυ=false;int ΖЌ=0;int ȋ=0;
ά Ƅ=null;float ΒЍ=0;float Αέ=0;ά ΕЈ=null;float ΒЀ=0;Ǒ Ė;float ΓΨ=0;float ΓЅ=0;String ΖΧ="";float ΑΨ=0;float Ǳ=0;float ǽ=0;float Ђ=0;bool Ας=false;bool ΖЀ=false;bool Ζχ=false;bool ΓЁ=false;bool Ζψ=false;float ΖΨ=0;float Ζε=0;Vector3 ΕЏ=new Vector3();Vector3 Γκ=new Vector3();
bool ΖΕ=false;float[,]ļ=new float[3,2];Dictionary<String,float[,]>Ǎ=new Dictionary<string,float[,]>();Dictionary<String,float[]>Ɓ=new Dictionary<string,float[]>();bool ΒА=false;int Ś=0;int ǝ=0;int ű=0;int Ƿ=0;String Ϊ="";int[]Γη=new int[Enum.GetValues(typeof(Κ)).Length];
bool Αΰ=false;int Ǿ=0;int ς=0;Κ δ=Κ.Main;int ΖЁ=0,ΖЂ=0,ΖЃ=0,ΖЄ=0;int Ζο=0,Ζπ=0;bool Ę=false;bool Βϋ=false;String Ķ="";String ǈ="";IMyBroadcastListener є=null;bool ΓΘ=true;List<ί>β=null;ί σ=null;int Š=0,š=0;int ΒЊ=0,ΒЋ=0;int ΕЅ=0,ΕІ=0,ΕЇ=0;String ΖΡ="";
Exception ȥ=null;bool Γτ=false;Τ Αν=Τ.ё;bool ΒЏ=false;String ΖЉ="";bool ΓЄ=false;bool Αφ=true;int ΑΠ=0;Vector3 ǉ=new Vector3();bool ΓΛ=false;bool Ʀ(IMyTerminalBlock α)=>α.CubeGrid==Me.CubeGrid;Program(){Н=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;
if(ΒΤ(Me,controllerTag,true))
Ρ=Ο.Е;Ω(Κ.Main);if(Ρ!=Ο.Е){Ϊ="Welcome to [PAM]!";п Ακ=ΕΚ();if(Ρ==Ο.Ǚ){List<IMyShipDrill>ΓΖ=new List<IMyShipDrill>();List<IMyShipGrinder>ΓΜ=new List<IMyShipGrinder>();Н.GetBlocksOfType(ΓΖ,q=>q.CubeGrid==Me.CubeGrid);Н.GetBlocksOfType(ΓΜ,q=>q.CubeGrid==Me.CubeGrid);if(ΓΖ.Count>0)
{Ρ=Ο.Ƭ;Ϊ="Miner mode enabled!";}else if(ΓΜ.Count>0){Ρ=Ο.Ȓ;Ϊ="Grinder mode enabled!";}else{Ρ=Ο.ο;Βρ(ǘ.ο);}}if(Ακ==п.Βυ) Ζέ=false;if(Ακ==п.ΒΓ) Ϊ="Data restore failed!";if(Ακ==п.Βε) Ϊ="New version, wipe data";}}void Main(string А,UpdateType ΖІ){try{if(ȥ!=null){if(А.Trim().ToUpper()=="RETRY")
{ȥ=null;Ĥ=true;Ζξ=0;if(Ρ!=Ο.Е) Ǯ();Ϊ="Restarted after error";}else{Δφ();ΕΖ();return;}}ǟ=(ΖІ&UpdateType.Update10)!=0;if(ǟ) Αύ++;ĭ=Αύ>=10;if(ĭ) Αύ=0;if(ǟ){Ś++;if(Ś>4) Ś=0;ƃ=(int) Math.Max(0,10-(DateTime.Now-Γή).TotalSeconds);}if(А!="") ΖΡ=А;if(Ρ!=Ο.Е)
Εέ(А);else
ΔΜ(А);Αΰ=false;try{int ΒЌ=Runtime.CurrentInstructionCount;float Ȅ=С(ΒЌ,Runtime.MaxInstructionCount);if(Ȅ>0.90) Ϊ="Max. instructions >90%";if(Ȅ>ȧ) ȧ=Ȅ;if(ΒА){Ȣ.Add(ΒЌ);while(Ȣ.Count>10) Ȣ.RemoveAt(0);ș=0;for(int i=0;i<Ȣ.Count;i++) ș+=Ȣ[i];ș=С(С(ș,Ȣ.Count),Runtime.MaxInstructionCount);
double ΑΦ=Runtime.LastRunTimeMs;if(ΖΜ&&ΑΦ>Αή) Αή=ΑΦ;Ȳ.Add((int)(ΑΦ*1000f));while(Ȳ.Count>10) Ȳ.RemoveAt(0);ĕ=0;for(int i=0;i<Ȳ.Count;i++) ĕ+=Ȳ[i];ĕ=С(ĕ,Ȳ.Count)/1000f;}}catch{ȧ=0;}}catch(Exception e){ȥ=e;}}String Δι(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Ω(Κ Ў){Γη[(int) δ]=ς;ς=Γη[(int) Ў];if(Ў==Κ.ď) ς=0;δ=Ў;if(Ρ!=Ο.Е)
Ř(δ==Κ.Č,false,0,0);Αΰ=true;}void ΔЈ(bool ΑΪ){if(ς>0) ς--;else if(ΑΪ) ς=Ǿ-1;}void ΔЇ(bool ΑΪ){if(ς<Ǿ-1) ς++;else if(ΑΪ) ς=0;}String Βρ(ǘ η){switch(η){case ǘ.Βξ:Ϊ="Job is running";break;case ǘ.ΔΛ:Ϊ="Connector not ready!";break;case ǘ.Εή:Ϊ="Ship modified, path outdated!";break;
case ǘ.ΔϏ:Ϊ="Interrupted by player!";break;case ǘ.ο:Ϊ="Shuttle mode enabled!";break;}return "";}String Δπ(У Φ){switch(Φ){case У.ŕ:return "Top-Left";case У.ΔΖ:return "Center";default:return "";}}String Δε(я λ){switch(λ){case я.ΓЍ:return "Auto"+(Ρ==Ο.Ƭ?" (Ore)":"");case я.ΑЌ:return "Auto (+Stone)";
case я.Ƙ:return "Default";default:return "";}}String Δν(Τ Ѐ){switch(Ѐ){case Τ.ё:return "No job";case Τ.Ư:return "Job paused";case Τ.Ǫ:return "Job active";case Τ.Ł:return "Job active";case Τ.ł:return "Job active";case Τ.ΒΒ:return "Job done";case Τ.Ņ:return "Job changed";case Τ.Βγ:return "Move home";
case Τ.Βδ:return "Move to job";}return "";}String ΒΘ(ǔ Εό){switch(Εό){case ǔ.Βμ:return "Return home";case ǔ.Δή:return "Fly to job pos";case ǔ.Εα:return "Stop";case ǔ.Ƣ:return "Ignore";}return "";}String Δη(в λ){switch(λ){case в.Βζ:return "Off";case в.ΔΦ:return "Drop pos (Stone) ";case в.ΔΥ:return "Drop pos (Sto.+Ice)";
case в.ΔΞ:return "Cur. pos (Stone)";case в.ΔΝ:return "Cur. pos (Sto.+Ice)";case в.Δύ:return "In motion (Stone)";case в.Δό:return "In motion (Sto.+Ice)";}return "";}String ΒΖ(Ǒ Ѐ){switch(Ѐ){case Ǒ.None:return "No batteries";case Ǒ.Ɣ:return "Charging";case Ǒ.ΔΠ:return "Discharging";}return "";}
String ΒΜ(Ν Ѐ){String ΗΘ=Ρ==Ο.ο?"target":"job";switch(Ѐ){case Ν.ǫ:return "Idle";case Ν.Ɲ:return "Flying to XY position";case Ν.Ββ:return Ρ==Ο.Ȓ?"Grinding":"Mining";case Ν.Ʋ:return "Returning";case Ν.ΔΫ:return "Flying to drop pos";case Ν.Βν:return "Returning to dock";case Ν.Ċ:return "Flying to dock area";
case Ν.Ǖ:return "Flying to job area";case Ν.Ɯ:return "Flying to path";case Ν.Δί:return "Flying to job position";case Ν.Ȏ:return "Approaching dock";case Ν.ΒΑ:return "Docking";case Ν.ΓЉ:return "Aligning";case Ν.ΑЋ:return "Aligning";case Ν.ΕΜ:return "Retry docking";case Ν.Ē:return "Unloading";case Ν.ē:return ΖЉ;
case Ν.đ:return "Undocking";case Ν.Ɣ:return "Charging batteries";case Ν.Ευ:return "Waiting for uranium";case Ν.ΔΪ:return "Filling up hydrogen";case Ν.Εσ:return "Waiting for ejection";case Ν.Ετ:return "Waiting for ejection";case Ν.Δά:return "Flying to drop pos";}return "";}String ΒΡ(Ѕ П){switch(П)
{case Ѕ.Βη:return "On \"Undock\" command";case Ѕ.Βθ:return "On player entered cockpit";case Ѕ.ΕΑ:return "On ship is full";case Ѕ.ΔБ:return "On ship is empty";case Ѕ.Βι:return "On time delay";case Ѕ.ΔЍ:return "On batteries empty(<25%)";case Ѕ.ΔЋ:return "On batteries empty(=0%)";case Ѕ.ΔЌ:return "On batteries full";
case Ѕ.ΔА:return "On hydrogen empty(<25%)";case Ѕ.ΔЎ:return "On hydrogen empty(=0%)";case Ѕ.ΔЏ:return "On hydrogen full";}return "";}bool Η(ref String Β,int Γψ,int ǜ,bool Ε,String ǻ){Ǿ+=1;if(Γψ==ǜ) ǻ=">"+ǻ+(Ś>=2?" .":"");else ǻ=" "+ǻ;Β+=ǻ+"\n";return Γψ==ǜ&&Ε;}String ΓЏ(bool Ε)
{int Α=0;int Δ=ς;Ǿ=0;String ā="———————————————\n";String Ą="--------------------------------------------\n";String Β="";Β+=Δν(φ)+" | "+(Σ.ν?"Ready to dock":"No dock")+"\n";Β+=ā;double ΓϏ=Math.Max(Math.Round(this.ΓΒ),0);if(δ==Κ.Main){bool Ǟ=Ρ==Ο.ο;if(Η(ref Β,Δ,Α++,Ε," Record path & set home")) Εί();
if(Ρ==Ο.Ƭ)
if(Η(ref Β,Δ,Α++,Ε," Setup mining job")) Ω(Κ.Č);if(Ρ==Ο.Ȓ)
if(Η(ref Β,Δ,Α++,Ε," Setup grinding job")) Ω(Κ.Č);if(Ρ==Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," Setup shuttle job")) Ω(Κ.Ƹ);if(Η(ref Β,Δ,Α++,Ε," Continue job")) ΑА();if(Η(ref Β,Δ,Α++,Ε," Fly to home position")) Δέ();if(Η(ref Β,Δ,Α++,Ε," Fly to job position")) Δΰ();if(Η(ref Β,Δ,Α++,Ε," Behavior settings"))
if(Ǟ) Ω(Κ.Œ);else Ω(Κ.ń);if(Η(ref Β,Δ,Α++,Ε," Info")) Ω(Κ.ΒΦ);if(Ρ!=Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," Help")) Ω(Κ.ΒΥ);}else if(δ==Κ.Č){double ΖЍ=Math.Round(Ц*ǐ,1);double ΖΘ=Math.Round(Х*Ǐ,1);String ξ="";if(Η(ref ξ,Δ,Α++,Ε," Start new job!")) Βτ();if(Η(ref ξ,Δ,Α++,Ε," Change current job")){Ƽ(false,false);Ω(Κ.Main);}if(Η(ref ξ,Δ,Α++,Ε," Width + (Width: "+Ц+" = "+ΖЍ+"m)")){ȍ(ref Ц,5,20,1);ȗ(true);}
if(Η(ref ξ,Δ,Α++,Ε," Width -")){ȍ(ref Ц,-5,20,-1);ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Height + (Height: "+Х+" = "+ΖΘ+"m)")){ȍ(ref Х,5,20,1);ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Height -")){ȍ(ref Х,-5,20,-1);ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Depth + ("+(ј==я.Ƙ?"Depth":"Min")+": "+д+"m)")){ȍ(ref д,5,50,2);ȗ(true);}
if(Η(ref ξ,Δ,Α++,Ε," Depth -")){ȍ(ref д,-5,50,-2);ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Depth mode: "+Δε(ј))){ј=Ǔ(ј);}if(Η(ref ξ,Δ,Α++,Ε," Start pos: "+Δπ(ȇ))){ȇ=Ǔ(ȇ);}if(Ρ==Ο.Ȓ&&ј==я.ΑЌ) ј=Ǔ(ј);Β+=Ƴ(8,ξ,Δ,ref Ζο);}else if(δ==Κ.Ƹ){float[]ΒБ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Η(ref Β,Δ,Α++,Ε," Next")) Ω(Κ.Βς);if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=" Leave connector 1:\n";if(Η(ref Β,Δ,Α++,Ε," - "+ΒΡ(χ.П))) χ.П=Ǔ(χ.П);if(!χ.ΒΣ()) Β+="\n";else if(Η(ref Β,Δ,Α++,Ε," - Delay: "+ƞ((int) χ.е))) χ.е=ΑБ(χ.е,ΒБ);Β+=" Leave connector 2:\n";if(Η(ref Β,Δ,Α++,Ε," - "+ΒΡ(ψ.П))) ψ.П=Ǔ(ψ.П);
if(!ψ.ΒΣ()) Β+="\n";else if(Η(ref Β,Δ,Α++,Ε," - Delay: "+ƞ((int) ψ.е))) ψ.е=ΑБ(ψ.е,ΒБ);}else if(δ==Κ.Βς){if(Η(ref Β,Δ,Α++,Ε," Start job!")) Βτ();if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Ƹ);Β+=" Timer: \"Docking connector 1\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(χ.ф!=""?χ.ф:"-"))) χ.ф=ŋ(ref ΖЁ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(χ.ч!=""?χ.ч:"-"))) χ.ч=ŋ(ref ΖЃ);Β+=" Timer: \"Docking connector 2\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(ψ.ф!=""?ψ.ф:"-"))) ψ.ф=ŋ(ref ΖЂ);Β+=" Timer: \"Leaving connector 2\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(ψ.ч!=""?ψ.ч:"-"))) ψ.ч=ŋ(ref ΖЄ);
}else if(δ==Κ.ċ){String Γβ=ш+" %";if(ĭ) ǝ++;if(ǝ>1){ǝ=0;ű++;if(ű>1) ű=0;}bool[]Ασ=new bool[]{Αθ.Count==0,Ė==Ǒ.None,ī.Count==0};int ΓЃ=0;while(true){ΓЃ++;Ƿ++;if(Ƿ>Ασ.Length-1) Ƿ=0;if(ΓЃ>=Ασ.Length) break;if(!Ασ[Ƿ]) break;}bool Ǟ=Ρ==Ο.ο;if(!Ǟ&&Ȍ&&Ђ!=-1&&ű==0)
Γβ=Ђ<1000000?Math.Round(Ђ)+" Kg":Math.Round(Ђ/1000)+" t";if(Η(ref Β,Δ,Α++,Ε," Stop!")){Ǯ();Ω(Κ.Main);}if(Η(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!Ǟ) Ω(Κ.ń);else Ω(Κ.Œ);if(!Ǟ){if(Η(ref Β,Δ,Α++,Ε," Next hole")) ΓЈ(false);}else if(Η(ref Β,Δ,Α++,Ε," Undock")) ΓЄ=true;Β+=Ą;if(!Ǟ) Β+="Progress: "+Math.Round(Ŵ,1)+" %\n";Β+="State: "+ΒΜ(Ť)+" "+ΓϏ+"m \n";Β+="Load: "+ΑΨ+" % Max: "+Γβ+" \n";if(Ƿ==0) Β+="Uranium: "+(Αθ.Count==0?"No reactors":Math.Round(ΓЅ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ƿ==1) Β+="Battery: "+(Ė==Ǒ.None?ΒΖ(Ė):ΒЀ+"% "+(щ==-1||Ǟ?"":" Min: "+щ+" %"))+"\n";if(Ƿ==2) Β+="Hydrogen: "+(ī.Count==0?"No tanks":Math.Round(ΓΨ,1)+"% "+(ъ==-1||Ǟ?"":" Min: "+ъ+" %"))+"\n";}else if(δ==Κ.ń){String ξ="";if(Η(ref ξ,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.ċ);else Ω(Κ.Main);}
if(Η(ref ξ,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ʃ,false);if(Η(ref ξ,Δ,Α++,Ε," Weight limit: "+(Ȍ?"On":"Off"))) Ȍ=!Ȍ;if(Η(ref ξ,Δ,Α++,Ε," Ejection: "+Δη(ġ))) ġ=Ǔ(ġ);if(Η(ref ξ,Δ,Α++,Ε," Toggle sorters: "+(ȉ?"On":"Off"))){ȉ=!ȉ;if(ȉ) Ʒ(Αφ);}
if(Η(ref ξ,Δ,Α++,Ε," Unload ice: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Η(ref ξ,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.ƫ,true);if(Η(ref ξ,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Min "+щ+"%"))) Д(ref щ,-5,τ.Βΰ,true);if(Η(ref ξ,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) Д(ref ъ,-10,τ.Βα,true);
if(Η(ref ξ,Δ,Α++,Ε," When done: "+(Ŀ?"Return home":"Stop"))) Ŀ=!Ŀ;if(Η(ref ξ,Δ,Α++,Ε," On damage: "+ΒΘ(ǣ))) ǣ=Ǔ(ǣ);if(Η(ref ξ,Δ,Α++,Ε," Advanced...")) Ω(Κ.Ɛ);Β+=Ƴ(8,ξ,Δ,ref Ζπ);}else if(δ==Κ.Ɛ){if(Η(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.ċ);else Ω(Κ.Main);}
if(Η(ref Β,Δ,Α++,Ε,(Ρ==Ο.Ȓ?" Grinder":" Drill")+" inv. balancing: "+(Ĭ?"On":"Off"))) Ĭ=!Ĭ;if(Η(ref Β,Δ,Α++,Ε," Enable"+(Ρ==Ο.Ȓ?" grinders":" drills")+": "+(Ģ?"Fwd + Bwd":"Fwd"))) Ģ=!Ģ;if(Η(ref Β,Δ,Α++,Ε," Work speed fwd.: "+ƍ+"m/s")) Д(ref ƍ,0.5f,τ.ƽ,false);
if(Η(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ƌ+"m/s")) Д(ref ƌ,0.5f,τ.ƽ,false);if(Η(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǚ*100f)+"%"+(ǚ>0.80f?" (risky)":""))) Д(ref ǚ,0.1f,τ.Ə,false);if(Η(ref Β,Δ,Α++,Ε," Width overlap: "+ŀ*100f+"%")) ΑЏ(true,0.05f);if(Η(ref Β,Δ,Α++,Ε," Height overlap: "+Ĩ*100f+"%")) ΑЏ(false,0.05f);
}else if(δ==Κ.Œ){if(Η(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.ċ);else Ω(Κ.Main);}if(Η(ref Β,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.Ʃ,false);if(Η(ref Β,Δ,Α++,Ε," Unload ice: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Η(ref Β,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.ƫ,true);
if(Η(ref Β,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Charge up"))) щ=(щ==-1?1:-1);if(Η(ref Β,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Fill up"))) ъ=(ъ==-1?1:-1);if(Η(ref Β,Δ,Α++,Ε," On damage: "+ΒΘ(ǣ))) ǣ=Ǔ(ǣ);if(Η(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǚ*100f)+"%"+(ǚ>0.80f?" (risky)":""))) Д(ref ǚ,0.1f,τ.Ə,false);
}else if(δ==Κ.Βλ){double ΖΣ=0;if(ω.Count>0) ΖΣ=Vector3.Distance(ω.Last().Π,м);if(Η(ref Β,Δ,Α++,Ε," Stop path recording")) Εδ();if(Ρ!=Ο.ο){if(Η(ref Β,Δ,Α++,Ε," Home: "+(ǩ?"Use old home":(Σ.ν?"Was set! ":"none ")))) ǩ=!ǩ;}else{if(Η(ref Β,Δ,Α++,Ε," Connector 1: "+(ǩ?"Use old connector":(Σ.ν?"Was set! ":"none ")))) ǩ=!ǩ;
if(Η(ref Β,Δ,Α++,Ε," Connector 2: "+(Ľ?"Use old connector":(Ψ.ν?"Was set! ":"none ")))) Ľ=!Ľ;}if(Η(ref Β,Δ,Α++,Ε," Path: "+(ľ?"Use old path":(ω.Count>1?"Count: "+ω.Count:"none ")))) ľ=!ľ;Β+=Ą;Β+="Wp spacing: "+Math.Round(ΖЋ)+"m\n";}else if(δ==Κ.ƻ){if(Η(ref Β,Δ,Α++,Ε," Stop")){Ǯ();Ω(Κ.Main);}
Β+=Ą;Β+="State: "+ΒΜ(Ť)+" \n";Β+="Speed: "+Math.Round(Ζφ,1)+"m/s\n";Β+="Target dist: "+ΓϏ+"m\n";Β+="Wp count: "+ω.Count+"\n";Β+="Wp left: "+ΖЌ+"\n";}else if(δ==Κ.ΒΦ){List<IMyTerminalBlock>ǲ=Δγ();if(ĭ) ǝ++;if(ǝ>=ǲ.Count) ǝ=0;if(Η(ref Β,Δ,Α++,Ε," Next")) Ω(Κ.ΒΧ);
Β+=Ą;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΨ,1)+"% "+Math.Round(Ǳ,1)+" / "+Math.Round(ǽ,1)+"\n";Β+="Uranium: "+(Αθ.Count==0?"No reactors":Math.Round(ΓЅ,1)+"Kg "+ΖΧ)+"\n";Β+="Battery: "+(Ė==Ǒ.None?"":ΒЀ+"% ")+ΒΖ(Ė)+"\n";Β+="Hydrogen: "+(ī.Count==0?"No tanks":Math.Round(ΓΨ,1)+"% ")+"\n";
Β+="Damage: "+(ǲ.Count==0?"None":""+(ǝ+1)+"/"+ǲ.Count+" "+ǲ[ǝ].CustomName)+"\n";}else if(δ==Κ.ΒΧ){if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=Ą;Β+="Next scan: "+ƃ+"s\n";Β+="Drill area: "+Math.Round(ǐ,1)+"x"+Math.Round(Ǐ,1)+"m Ship: "+Math.Round(Γϊ,1)+"x"+Math.Round(Γω,1)+"m ("+Math.Round(Ă,1)+"m)\n"+(ΕϏ.Count>0?"Cameras: "+Εώ+"\n":"");
Β+="Broadcast: "+(Ę?"Online - "+Ķ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȧ*100f,1)+"% \n";}else if(δ==Κ.ΒΥ){if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=Ą;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Αο==2) Β="Fatal setup error\nNext scan: "+ƃ+"s\n";if(Γτ) Β="Recompile script now";int ΖΥ=Β.Split('\n').Length;for(int i=ΖΥ;i<=10;i++) Β+="\n";Β+=ā;Β+="Last: "+Ϊ+"\n";
return Β;}void ΑЏ(bool ΖΟ,float Ɔ){Ǯ();Ƽ(true,false);if(ΖΟ) Д(ref ŀ,Ɔ,τ.Ʈ,false);else Д(ref Ĩ,Ɔ,τ.Ʈ,false);ΔΕ();Ǻ=ǐ;ǹ=Ǐ;Ř(true,true,0,0);}float ΑБ(float Ȝ,float[]Ɖ){float έ=Ɖ[0];for(int i=Ɖ.Length-1;i>=0;i--)
if(Ȝ<Ɖ[i]) έ=Ɖ[i];return έ;}String ŋ(ref int ǜ){String έ="";if(ǜ>=ΓЂ.Count) ǜ=-1;if(ǜ>=0) έ=ΓЂ[ǜ].CustomName;ǜ++;return έ;}void ȍ(ref int value,int ė,int Ƈ,int Ατ){if(ė==0) return;if(value<Ƈ&&Ατ>0||value<=Ƈ&&Ατ<0){value+=Ατ;return;}int Εχ=Math.Abs(ė);int ǃ=0;
int Αα=1;while(true){ǃ+=Αα*Εχ*10;if(ė<0&&value-Ƈ<=ǃ) break;if(ė>0&&value-Ƈ<ǃ) break;Αα++;}value+=Αα*ė;}void ȗ(bool ΗΜ){Ц=Math.Max(Ц,1);Х=Math.Max(Х,1);д=Math.Max(д,0);Ř(δ==Κ.Č,false,0,0);}void Д(ref float value,float Ɔ,τ Ќ,bool Ļ){Д(ref value,value+Ɔ,Ќ,Ļ,true);
}void Д(ref float value,float ȩ,τ Ќ,bool Ļ,bool ΖА){float Ь=0,Ъ=0;if(Ќ==τ.ƽ){Ь=0.5f;Ъ=10f;}if(Ќ==τ.Ə){Ь=0.1f;Ъ=1f;}if(Ќ==τ.Ƣ){Ь=50f;Ъ=100f;}if(Ќ==τ.Βΰ){Ь=5f;Ъ=30f;}if(Ќ==τ.ƫ){Ь=1f;Ъ=25f;}if(Ќ==τ.Ʃ){Ь=10f;Ъ=95f;}if(Ќ==τ.Βα){Ь=10f;Ъ=90f;}
if(Ќ==τ.Εκ){Ь=10f;Ъ=1800;}if(Ќ==τ.Ʈ){Ь=0.0f;Ъ=0.75f;}if(ȩ==-1&&Ļ){value=-1;return;}if(value==-1) Ļ=false;bool Γμ=ȩ<Ь||ȩ>Ъ;if(Γμ&&ΖА){if(ȩ<value) value=Ъ;else if(ȩ>value) value=Ь;}else value=ȩ;if(Γμ&&Ļ) value=-1;else value=Math.Max(Ь,Math.Min(value,Ъ));
value=(float) Math.Round(value,2);}void ΓЈ(bool Ζθ){if(Ζθ) с=Math.Max(0,с-1);else с++;ΔΨ();ΔΧ();for(int k=0;k<ΑΞ.Count&&k<з.Length;k++) ΕΩ(ΑΞ[k],k<с);ȁ=-1;Βϊ(true);}T Ǔ<T>(T Ȝ){int Α=Array.IndexOf(Enum.GetValues(Ȝ.GetType()),Ȝ);Α++;if(Α>=ΔΩ(Ȝ)) Α=0;return(T) Enum.GetValues(Ȝ.GetType()).GetValue(Α);
}int ΔΩ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Εί(){Ƌ.Clear();for(int i=0;i<ω.Count;i++) Ƌ.Add(ω[i]);ω.Clear();ΓΩ=true;ΓΤ=new ά(Σ);ΓΫ=new ά(Ψ);Σ.ν=false;if(Ρ==Ο.ο) Ψ.ν=false;for(int i=0;i<Ǎ.Count;i++)
if(!ǎ.Contains(Ǎ.Keys.ElementAt(i)))
ǎ.Add(Ǎ.Keys.ElementAt(i));ǩ=false;Ľ=false;ľ=false;Ω(Κ.Βλ);}void Εδ(){if(ǩ) Σ=ΓΤ;if(Ľ) Ψ=ΓΫ;if(ľ){ω.Clear();for(int i=0;i<Ƌ.Count;i++) ω.Add(Ƌ[i]);}ΓΩ=false;Ǯ();Ω(Κ.Main);}void Ő(ά ж,Τ Ѐ){ΕЈ=ж;if(φ==Τ.Ǫ) ǡ=Ѐ;}bool ΒΙ(ά ж,float у,bool ΖЇ,out Vector3 Φ){if(ΖЇ){Vector3I ΖΔ=new Vector3I((int) ж.Ч.X,(int) ж.Ч.Y,(int) ж.Ч.Z);
IMySlimBlock α=Me.CubeGrid.GetCubeBlock(ΖΔ);if(α==null||!(α.FatBlock is IMyShipConnector)){Φ=new Vector3();return false;}Vector3 ΕЄ=Ŋ(Њ,α.FatBlock.GetPosition()-м);Vector3 ΕЃ=Ŋ(Њ,α.FatBlock.WorldMatrix.Forward);Φ=ж.Π-Βχ(ж.І,ж.Ё*-1,ΕЄ)-Βχ(ж.І,ж.Ё*-1,ΕЃ)*у;return true;}
else{Φ=ж.Π;return true;}}Vector3 Δλ(int ǋ,int Ǧ,bool Ζλ){if(!Ζλ&&ΓΛ) return ǉ;float ΕЂ=((к-1f)/2f)-ǋ;float Ζν=((й-1f)/2f)-Ǧ;ǉ=Ψ.Π+Ψ.О*ΕЂ*ΔЅ()+Ȩ*-1*Ζν*ΔЄ();ΓΛ=true;return ǉ;}Υ ƙ(){float Ф=-1;Υ έ=Υ.о;if(Ρ!=Ο.ο){ΔІ();if(φ!=Τ.ё){Vector3 ύ=Βψ(ı,Ȩ*-1,м-Ψ.Π);
if(Math.Abs(ύ.X)<=(float)(к*ǐ)/2f&&Math.Abs(ύ.Y)<=(float)(й*Ǐ)/2f){if(ύ.Z<=-1&&ύ.Z>=-Αμ*2) return Υ.Ń;if(ύ.Z>-1&&ύ.Z<Ă*2) return Υ.о;}if(ǒ(Ψ.Π,ref Ф)) έ=Υ.о;}if(Σ.ν){if(ǒ(Σ.Π,ref Ф)) έ=Υ.н;for(int i=0;i<ω.Count;i++)
if(ǒ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Vector3.Distance(м,Σ.Π)<dockDist*Ă) έ=Υ.н;if(Ȑ(MyShipConnectorStatus.Connectable)!=null||Ȑ(MyShipConnectorStatus.Connected)!=null)
έ=Υ.н;}}else{Vector3 Φ=new Vector3();IMyShipConnector ş=Ȑ(MyShipConnectorStatus.Connected);if(Σ.ν){if(ǒ(Σ.Π,ref Ф)) έ=Υ.н;if(ΒΙ(Σ,dockDist,true,out Φ))
if(ǒ(Φ,ref Ф)) έ=Υ.н;if(ş!=null&&Vector3.Distance(ş.GetPosition(),Σ.Π)<5)
return Υ.Ƒ;}for(int i=0;i<ω.Count;i++)
if(Vector3.Distance(ω[i].Π,Σ.Π)>dockDist*Ă&&Vector3.Distance(ω[i].Π,Ψ.Π)>dockDist*Ă)
if(ǒ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Ψ.ν){if(ǒ(Ψ.Π,ref Ф)) έ=Υ.о;if(ΒΙ(Ψ,dockDist,true,out Φ))
if(ǒ(Φ,ref Ф)) έ=Υ.о;if(ş!=null&&Vector3.Distance(ş.GetPosition(),Ψ.Π)<5)
return Υ.ƒ;}}return έ;}bool ǒ(Vector3 Φ,ref float Ф){float у=Vector3.Distance(Φ,м);if(у<Ф||Ф==-1){Ф=у;return true;}return false;}void Βτ(){if(Αο>0){Ϊ="Setup error! Can't start";return;}if(Ρ==Ο.ο){ΑА();return;}Ψ.Π=м;Ψ.π=Њ.GetNaturalGravity();Ψ.І=ΕБ;Ψ.Ё=ΕЊ;Ψ.О=ΖΤ;
ı=ǥ.WorldMatrix.Forward;Ȩ=Ψ.Ё;if(ı==Њ.WorldMatrix.Down) Ȩ=Њ.WorldMatrix.Backward;Ǻ=ǐ;ǹ=Ǐ;Ƽ(true,true);ε(Ν.Ɲ);ƹ();}void Ƽ(bool Ш,bool Ζμ){if(φ==Τ.ё&&!Ш) return;bool Ζή=Ш||φ==Τ.ΒΒ||к!=Ц||й!=Х||ȣ!=ȇ;if(Ζή){if(φ!=Τ.ё){φ=Τ.Ņ;Δλ(Ů,ů,Ζμ);Ϊ="Job changed, lost progress";
}ȣ=ȇ;к=Ц;й=Х;ů=0;Ů=0;з="";ȁ=-1;ŵ=(int)(DateTime.Now.Ticks/10000000%1000000);Αέ=0;į=0;ΒЍ=0;с=0;Βϊ(true);}}void ư(){ΕΧ(м,0);ŏ(ĺ,true);}void ƹ(){Βρ(ǘ.Βξ);ư();ƶ(ΑΤ,false);Ʒ(Αφ);φ=Τ.Ǫ;ΔΔ(true);ǡ=φ;Ω(Κ.ċ);ΕΟ();ΒЎ=true;ΑΠ=0;for(int i=Ĕ.Count-1;i>=0;i--)
if(ΒΩ(Ĕ[i],false)) ΑΠ++;if(ΑΠ>0) Ϊ="Started with damage";}void Ǯ(){if(φ==Τ.Ǫ){φ=Τ.Ư;Ϊ="Job paused";}ε(Ν.ǫ);ǡ=φ;ΕΨ(false,0,0,0,0);Εβ();ΓЋ(new Vector3(),false);Εγ();ΕΤ(ChargeMode.Auto);ΕΪ(false);ΕΦ(true);ǭ(Ν.ǫ);Ř(false,false,0,0);ŏ(а,false);ŏ(Ĝ,true);Ʒ(true);Γυ=false;ΔΘ();
ΕЉ=false;ΕΘ();ΒЎ=false;Ας=false;ΓЄ=false;if(δ!=Κ.Main&&δ!=Κ.ń&&δ!=Κ.Ɛ&&δ!=Κ.Œ)
Ω(Κ.Main);}void ΑА(){Υ ќ=ƙ();if(Ρ==Ο.ο){if(!Ψ.ν||!Σ.ν) return;ƹ();bool ΑΚ=Vector3.Distance(м,Σ.Π)<Vector3.Distance(м,Ψ.Π);if(Αν==Τ.Ł) ΑΚ=true;if(Αν==Τ.ł) ΑΚ=false;if(ΑΚ){Ő(Σ,Τ.Ł);switch(ќ){case Υ.Ƒ:ε(Ν.ē);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.н:ε(Ν.Ȏ);break;default:ε(Ν.đ);break;
}}else{Ő(Ψ,Τ.ł);switch(ќ){case Υ.ƒ:ε(Ν.ē);break;case Υ.о:ε(Ν.Ȏ);break;case Υ.ђ:ε(Ν.Ǖ);break;default:ε(Ν.đ);break;}}}else{if(φ!=Τ.Ư&&φ!=Τ.Ņ) return;bool ΖЊ=φ==Τ.Ņ;ƹ();bool ΑΜ=ΔЀ(false)&&Σ.ν;switch(ќ){case Υ.о:ε(ΑΜ?Ν.Ɯ:Ν.Ɲ);break;case Υ.ђ:ε(ΑΜ?Ν.Ċ:Ν.Ǖ);break;case Υ.н:ε(ΑΜ?Ν.Ȏ:Ν.Ē);break;
case Υ.Ń:{if(į!=с||ΖЊ) ε(Ν.Ʋ);else ε(Ν.Ββ);}break;default:break;}}}void Δΰ(){if(φ==Τ.ё&&!Σ.ν) return;if(Ρ==Ο.ο&&(!Ψ.ν||!Σ.ν)) return;Ϊ="Move to job";Υ ќ=ƙ();if(Ρ==Ο.ο){Ő(Ψ,Τ.ł);switch(ќ){case Υ.о:ε(Ν.Ȏ);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.ƒ:return;default:ε(Ν.đ);break;
}ǭ(Ν.ē);}else{switch(ќ){case Υ.о:ε(Ν.Ǖ);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.н:ε(Ν.Ē);break;case Υ.Ń:ε(Ν.Ʋ);break;default:break;}if(φ==Τ.ё) ǭ(Ν.Ǖ);else ǭ(Ν.ΑЋ);Γυ=true;}ư();Ω(Κ.ƻ);ƶ(ΑΤ,false);ǡ=Τ.Βδ;}void Δέ(){if(!Σ.ν) return;Ϊ="Move home";Υ ќ=ƙ();if(Ρ==Ο.ο){Ő(Σ,Τ.Ł);switch(ќ)
{case Υ.ђ:ε(Ν.Ċ);break;case Υ.н:ε(Ν.Ȏ);break;case Υ.Ƒ:return;default:ε(Ν.đ);break;}ǭ(Ν.ē);}else{if(Ȑ(MyShipConnectorStatus.Connected)!=null) return;if(Ȑ(MyShipConnectorStatus.Connectable)!=null){ε(Ν.ΒΑ);ǭ(Ν.Ē);return;}switch(ќ){case Υ.о:ε(Ν.Ɯ);break;case Υ.ђ:ε(Ν.Ċ);break;case Υ.н:ε(Ν.Ċ);break;
case Υ.Ń:ε(Ν.Βν);break;default:break;}ǭ(Ν.Ē);}ư();Ω(Κ.ƻ);ƶ(ΑΤ,false);ǡ=Τ.Βγ;}void ε(Ν Αβ){if(Αβ==Ν.ǫ) ĸ=Ν.ǫ;if(ĸ!=Ν.ǫ&&Ť==ĸ&&Αβ!=ĸ){Ǯ();return;}Ζϊ=true;Ť=Αβ;}void ǭ(Ν Ѐ){this.ĸ=Ѐ;}void Βϊ(bool Αι){if(Αι){Ŵ=0;return;}if(Ρ==Ο.ο) return;float κ=с*Math.Max(1,д);
if(į==с) κ+=Math.Min(д,ΒЍ);float ΖЅ=к*й*Math.Max(1,д);Ŵ=Math.Max(Ŵ,(float) Math.Min(κ/ΖЅ*100.0,100));}List<IMyTerminalBlock>Δγ(){List<IMyTerminalBlock>ǲ=new List<IMyTerminalBlock>();for(int i=0;i<Ĕ.Count;i++){IMyTerminalBlock α=Ĕ[i];if(ΒΩ(α,false)) ǲ.Add(α);}return ǲ;
}bool ΒΩ(IMyTerminalBlock α,bool Αω){return(!ΒΪ(α,Αω)||!α.IsFunctional);}bool ΒΪ(IMyTerminalBlock α,bool Αω){if(α==null) return false;try{IMyCubeBlock ΓΚ=Me.CubeGrid.GetCubeBlock(α.Position).FatBlock;if(Αω) return ΓΚ==α;else return ΓΚ.GetType()==α.GetType();}catch{return false;}}bool ΔЀ(bool ΗΚ)
{if(Ȍ&&Ρ!=Ο.ο)
if(Ђ!=-1&&Γϋ>=Ђ){Ϊ="Ship too heavy";return true;}if(ΑΨ>=ш||Ας){Ας=false;Ϊ="Ship is full";return true;}return false;}static String Δρ(IMyThrust э){return э.BlockDefinition.SubtypeId;}Vector3 ƨ(Vector3 К,float[,]Ȧ){return new Vector3(
К.X>=0?Ȧ[0,0]:Ȧ[0,1],
К.Y>=0?Ȧ[1,0]:Ȧ[1,1],
К.Z>=0?Ȧ[2,0]:Ȧ[2,1]);}bool ΒΠ(ά ή,String Ȋ,out float ΑΕ){ΑΕ=0;int Α=ǎ.IndexOf(Ȋ);if(Α==-1||ή.Я==null||Α>=ή.Я.Length)
return false;ΑΕ=ή.Я[Α];if(ΑΕ==-1) return false;return true;}Vector3 ΒΟ(Vector3 К,ά ή){if(ή!=null){Vector3 έ=new Vector3();for(int i=0;i<Ǎ.Keys.Count;i++){String Ȋ=Ǎ.Keys.ElementAt(i);float Ġ=0;if(!ΒΠ(ή,Ȋ,out Ġ))
return ƨ(К,ļ);έ+=ƨ(К,Ǎ.Values.ElementAt(i))*Ġ;}return έ;}return ƨ(К,ļ);}float Ơ(Vector3 К,ά ή){return Ơ(К,new Vector3(),ή);}float Ơ(Vector3 К,Vector3 π,ά ή){Vector3 ΖϏ=ΒΟ(К,ή);Vector3 ΕЌ=ΖϏ+π*Γϋ;float Ȅ=(ΕЌ/К).AbsMin();return(float)(К*Ȅ).Length();}float ň(Vector3 ĥ,Vector3 Ѓ,Vector3 π,ά ή)
{if(π.Length()==0f) return 0;Vector3 ΖΦ=Βψ(ĥ,Ѓ,Vector3.Normalize(π));float Ш=Ơ(-ΖΦ,ή);return Ш/π.Length();}void ΔΔ(bool ΖΛ){float Р=0;float ΑΖ=0.9f;if(ΖΛ){Ђ=-1;ȋ=0;Ƅ=null;if(φ!=Τ.ё&&Ψ.π.Length()!=0){Р=ΑΖ*ň(Ψ.І,Ψ.Ё*-1,Ψ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}if(Σ.ν&&Σ.π.Length()!=0)
{Р=ΑΖ*ň(Σ.І,Σ.Ё*-1,Σ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}return;}if(ȋ==-1) return;if(ȋ>=0){int Γπ=0;while(ȋ<ω.Count){if(Γπ>100) return;Γπ++;ά ή=ω[ȋ];if(ή.π.Length()!=0f){Р=ΑΖ*Math.Min(
ň(ή.І,ή.Ё*-1,ή.π,ή),
ň(ή.І*-1,ή.Ё*-1,ή.π,ή));if(Р<Ђ||Ђ==-1) Ђ=Р;}else Ƅ=ή;ȋ++;}ȋ=-1;}bool ΑΛ=true;float Ǣ=0;if(Ƅ!=null){for(int i=0;i<Ǎ.Count;i++){String Ȋ=Ǎ.Keys.ElementAt(i);float[,]ΑЂ=Ǎ.Values.ElementAt(i);float Ġ=0;if(!ΒΠ(Ƅ,Ȋ,out Ġ)){ΑΛ=false;break;}for(int a=0;a<ΑЂ.GetLength(0);a++)
for(int b=0;b<ΑЂ.GetLength(1);b++){float ǯ=Math.Abs(ΑЂ[a,b]*Ġ);if(ǯ==0) continue;ΑΛ=true;if(Ǣ==0||ǯ<Ǣ) Ǣ=ǯ;}}}if(!ΑΛ){for(int a=0;a<ļ.GetLength(0);a++)
for(int b=0;b<ļ.GetLength(1);b++){float ǯ=Math.Abs(ļ[a,b]);if(ǯ==0) continue;if(Ǣ==0||ǯ<Ǣ) Ǣ=ǯ;}}if(Ǣ>0){float ΖΪ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Р=С(Ǣ,ΖΪ);if(Р>0)
if(Р<Ђ||Ђ==-1) Ђ=Р;}}void ΓЋ(Vector3 Ш,bool Ϗ){if(!Ϗ){for(int i=0;i<ĺ.Count;i++)
ĺ[i].SetValueFloat("Override",0.0f);return;}Vector3 ǅ=ΒΟ(Ш,null);float ΖБ=Math.Min(1,Math.Abs(С(Ш.X,ǅ.X)));float ΗΑ=Math.Min(1,Math.Abs(С(Ш.Y,ǅ.Y)));float ΗΒ=Math.Min(1,Math.Abs(С(Ш.Z,ǅ.Z)));for(int i=0;i<ĺ.Count;i++){IMyThrust э=ĺ[i];Vector3 Ȟ=ΕΝ(Ŋ(Њ,э.WorldMatrix.Backward),1);
if(Ȟ.X!=0&&Math.Sign(Ȟ.X)==Math.Sign(Ш.X))
э.SetValueFloat("Override",э.MaxThrust*ΖБ);else if(Ȟ.Y!=0&&Math.Sign(Ȟ.Y)==Math.Sign(Ш.Y))
э.SetValueFloat("Override",э.MaxThrust*ΗΑ);else if(Ȟ.Z!=0&&Math.Sign(Ȟ.Z)==Math.Sign(Ш.Z))
э.SetValueFloat("Override",э.MaxThrust*ΗΒ);else
э.SetValueFloat("Override",0.0f);}}void ΕΨ(bool Ϗ,float Ζη,float ȭ,float ȱ,float Ĉ){for(int i=0;i<ΓΝ.Count;i++){IMyGyro ρ=ΓΝ[i];ρ.GyroOverride=Ϗ;if(!Ϗ) ρ.GyroPower=100;else ρ.GyroPower=Ζη;if(!Ϗ) continue;Vector3 М=Њ.WorldMatrix.Forward;Vector3 Ȱ=Њ.WorldMatrix.Right;Vector3 Ѓ=Њ.WorldMatrix.Up;
Vector3 ȟ=ρ.WorldMatrix.Forward;Vector3 ȡ=ρ.WorldMatrix.Up;Vector3 Ƞ=ρ.WorldMatrix.Left*-1;if(ȟ==М) ρ.SetValueFloat("Roll",Ĉ);else if(ȟ==(М*-1)) ρ.SetValueFloat("Roll",Ĉ*-1);else if(ȡ==(М*-1)) ρ.SetValueFloat("Yaw",Ĉ);else if(ȡ==М) ρ.SetValueFloat("Yaw",Ĉ*-1);else if(Ƞ==М) ρ.SetValueFloat("Pitch",Ĉ);
else if(Ƞ==(М*-1)) ρ.SetValueFloat("Pitch",Ĉ*-1);if(Ƞ==(Ȱ*-1)) ρ.SetValueFloat("Pitch",ȭ);else if(Ƞ==Ȱ) ρ.SetValueFloat("Pitch",ȭ*-1);else if(ȡ==Ȱ) ρ.SetValueFloat("Yaw",ȭ);else if(ȡ==(Ȱ*-1)) ρ.SetValueFloat("Yaw",ȭ*-1);else if(ȟ==(Ȱ*-1)) ρ.SetValueFloat("Roll",ȭ);
else if(ȟ==Ȱ) ρ.SetValueFloat("Roll",ȭ*-1);if(ȡ==(Ѓ*-1)) ρ.SetValueFloat("Yaw",ȱ);else if(ȡ==Ѓ) ρ.SetValueFloat("Yaw",ȱ*-1);else if(Ƞ==Ѓ) ρ.SetValueFloat("Pitch",ȱ);else if(Ƞ==(Ѓ*-1)) ρ.SetValueFloat("Pitch",ȱ*-1);else if(ȟ==Ѓ) ρ.SetValueFloat("Roll",ȱ);else if(ȟ==(Ѓ*-1)) ρ.SetValueFloat("Roll",ȱ*-1);
}}void Εγ(){this.ΖΕ=false;}void Εβ(){ő(false,false,false,Γκ,0);ΓЁ=false;}void ΕΧ(Vector3 ΰ,float ķ){ő(true,false,false,ΰ,ķ);}void ő(bool Ϗ,bool Αυ,bool ΑЃ,Vector3 ΰ,float ķ){ő(Ϗ,Αυ,ΑЃ,ΰ,ΰ-м,0.0f,ķ);}void ő(bool Ϗ,bool Αυ,bool ΑЃ,Vector3 ΰ,Vector3 Ζς,float Ζσ,float ķ)
{ΓЁ=true;this.ΖЀ=Ϗ;Γκ=ΰ;this.ΖΨ=ķ;this.Ζε=Ζσ;this.Ζψ=Αυ;this.Ζχ=ΑЃ;this.ΕЏ=Ζς;ΓΒ=Vector3.Distance(ΰ,м);}void ΕΟ(){Н.GetBlocksOfType(Ĕ,Ʀ);}void ΔΕ(){if(Њ==null) return;ǥ=null;float ż=0,Ÿ=0,Ž=0,Ź=0,ž=0,ź=0;List<IMyTerminalBlock>Αϊ=ΒΔ(а,pamTag,true);bool Ζΰ=Αϊ.Count==0;
if(Αϊ.Count>0) ǥ=Αϊ[0];else if(а.Count>0) ǥ=а[0];int ţ=0;float ы=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǥ.WorldMatrix.Forward){if(Ζΰ){Αο=2;Ϊ="Mining direction is unclear!";return;}continue;}ţ++;ы=Math.Max(ы,Ελ(а[i]));Vector3 ύ=Ɵ(Њ,а[i].GetPosition());
if(i==0){ż=ύ.X;Ÿ=ύ.X;Ž=ύ.Y;Ź=ύ.Y;ž=ύ.Z;ź=ύ.Z;}Ÿ=Math.Max(ύ.X,Ÿ);ż=Math.Min(ύ.X,ż);Ź=Math.Max(ύ.Y,Ź);Ž=Math.Min(ύ.Y,Ž);ź=Math.Max(ύ.Z,ź);ž=Math.Min(ύ.Z,ž);}if(ы==0) ы=drillRadius;bool Ğ=ǥ!=null&&ǥ.WorldMatrix.Forward==Њ.WorldMatrix.Down;ǐ=(Ÿ-ż)*(1-ŀ)+ы*2;
Ǐ=(Ğ?ź-ž:Ź-Ž)*(1-Ĩ)+ы*2;IMyCubeGrid g=Њ.CubeGrid;float ΑІ=0,ΑЇ=0,ΑЈ=0,ΑЉ=0;for(int c=0;c<8;c++){Vector3 p=Ɵ(Њ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ğ?p.Z:p.Y;if(c==0||p.X<ΑІ) ΑІ=p.X;if(c==0||p.X>ΑЇ) ΑЇ=p.X;if(c==0||y<ΑЈ) ΑЈ=y;if(c==0||y>ΑЉ) ΑЉ=y;}Γϊ=ΑЇ-ΑІ+g.GridSize;Γω=ΑЉ-ΑЈ+g.GridSize;}float Ελ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΒΔ<T>(List<T>Ї,String Ю,bool Ű){List<T>έ=new List<T>();for(int i=0;i<Ї.Count;i++)
if(ΒΤ(Ї[i],Ю,Ű)) έ.Add(Ї[i]);return έ;}bool ΒΤ<T>(T α,String Ю,bool Ű){IMyTerminalBlock b=(IMyTerminalBlock) α;if(Ű&&b.CustomName.ToUpper().Contains(Ю.ToUpper())) return true;if(!Ű&&b.BlockDefinition.ToString().ToUpper().Contains(Ю.ToUpper())) return true;return false;}void ŏ<T>(List<T>Ї,bool Ϗ)
{for(int i=0;i<Ї.Count;i++)
ΕΥ((IMyTerminalBlock) Ї[i],Ϗ);}void ΕΥ(IMyTerminalBlock α,bool Ϗ){if(α==null) return;String Εψ=Ϗ?"OnOff_On":"OnOff_Off";var a=α.GetActionWithName(Εψ);a.Apply(α);}void Ʒ(bool Ϗ){Αφ=Ϗ;if(!ȉ) return;ŏ(Ζω,Ϗ);}void ΕΤ(ChargeMode λ){for(int i=0;i<ΒϏ.Count;i++) ΒϏ[i].ChargeMode=λ;}void ΕΪ(bool Ζό)
{for(int i=0;i<ī.Count;i++) ī[i].Stockpile=Ζό;}void ƶ(List<IMyLandingGear>ΑΙ,bool Γγ){for(int i=0;i<ΑΙ.Count;i++){if(Γγ) ΑΙ[i].Lock();if(!Γγ) ΑΙ[i].Unlock();}}void ΕΦ(bool Ϗ){if(ΒЏ==Ϗ) return;List<IMyShipController>Ţ=new List<IMyShipController>();Н.GetBlocksOfType(Ţ,Ʀ);if(Ţ.Count==0) return;
for(int i=0;i<Ţ.Count;i++)
Ţ[i].DampenersOverride=Ϗ;ΒЏ=Ϗ;}IMyShipConnector Ȑ(MyShipConnectorStatus Ζϋ){for(int i=0;i<Ĝ.Count;i++){if(!ΒΪ(Ĝ[i],true)) continue;if(Ĝ[i].Status==Ζϋ) return Ĝ[i];}return null;}void Ř(bool Απ,bool Ŧ,float ΖΑ,float Εϋ){if(ώ==null||а.Count==0) return;Vector3 Ț=new Vector3();
int ţ=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǥ.WorldMatrix.Forward) continue;ţ++;Ț+=а[i].GetPosition();}Ț=Ț/ţ;Vector3 Ȫ=Ɵ(ώ,Ț);ώ.Enabled=true;ώ.ShowOnHUD=Απ;ώ.LeftExtend=(Ŧ?1:Ц)/2f*ǐ-Ȫ.X;ώ.RightExtend=(Ŧ?1:Ц)/2f*ǐ+Ȫ.X;ώ.TopExtend=(Ŧ?1:Х)/2f*Ǐ+Ȫ.Y;
ώ.BottomExtend=(Ŧ?1:Х)/2f*Ǐ-Ȫ.Y;ώ.FrontExtend=(Απ?д:ΖΑ)-Ȫ.Z;ώ.BackExtend=Απ?0:Εϋ+Ă*0.75f+Ȫ.Z;ώ.DetectFloatingObjects=true;ώ.DetectAsteroids=false;ώ.DetectLargeShips=true;ώ.DetectSmallShips=true;ώ.DetectStations=true;ώ.DetectOwner=true;ώ.DetectSubgrids=false;
ώ.DetectPlayers=false;ώ.DetectEnemy=true;ώ.DetectFriendly=true;ώ.DetectNeutral=true;}void Ď<T>(List<T>Ї,bool Ζυ,float ΕА,bool ΖΞ){for(int i=0;i<Ї.Count;i++){IMyTextSurface Ĺ=null;if(Ї[i]is IMyTextSurface) Ĺ=(IMyTextSurface) Ї[i];if(Ĺ!=null){Ĺ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ζυ) continue;Ĺ.Font="Debug";if(ΖΞ) continue;Ĺ.FontSize=ΕА;}}}static float С(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΕΝ(Vector3 v,int ΑΑ){return new Vector3(Math.Round(v.X,ΑΑ),Math.Round(v.Y,ΑΑ),Math.Round(v.Z,ΑΑ));}Vector3 Ɵ(IMyTerminalBlock α,Vector3 ΖЏ){return Ŋ(α,ΖЏ-α.GetPosition());
}Vector3 Ŋ(IMyTerminalBlock α,Vector3 ΑЅ){return Vector3D.TransformNormal(ΑЅ,MatrixD.Transpose(α.WorldMatrix));}Vector3 Βψ(Vector3 ĥ,Vector3 Ѓ,Vector3 ΑЅ){MatrixD Αά=MatrixD.CreateFromDir(ĥ,Ѓ);return Vector3D.TransformNormal(ΑЅ,MatrixD.Transpose(Αά));}Vector3 Βχ(Vector3 ĥ,Vector3 Ѓ,Vector3 ύ)
{MatrixD Αά=MatrixD.CreateFromDir(ĥ,Ѓ);return Vector3D.Transform(ύ,Αά);}String γ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ϋ(String s){String[]θ=s.Split('|');return new Vector3(float.Parse(Ι(θ,0)),float.Parse(Ι(θ,1)),float.Parse(Ι(θ,2)));}String Εφ(ά ή){String Ȇ=":";
String έ=γ(ή.Π)+Ȇ+γ(ή.І)+Ȇ+
γ(ή.Ё)+Ȇ+γ(ή.О)+Ȇ+γ(ή.π);for(int i=0;i<ή.Я.Length;i++)
έ+=Ȇ+Math.Round(ή.Я[i],3);return έ;}ά Εε(String s){String[]θ=s.Split(':');ά ή=new ά(
Ϋ(Ι(θ,0)),Ϋ(Ι(θ,1)),
Ϋ(Ι(θ,2)),Ϋ(Ι(θ,3)),
Ϋ(Ι(θ,4)));int Α=5;List<float>ΓΗ=new List<float>();while(Α<θ.Length){String ƈ=Ι(θ,Α);float f=0;if(!float.TryParse(ƈ,out f)) break;ΓΗ.Add(f);Α++;}ή.Я=ΓΗ.ToArray();return ή;}System.Text.StringBuilder ȳ=new System.Text.StringBuilder();void Μ<T>(T value,bool Ζί){if(Ζί) ȳ.Append('\n');ȳ.Append(value);}
void Μ<T>(T value){Μ(value,true);}String ņ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ŗ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Εω,int Α){String ƈ=Εω.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ƈ)) return "";
return ƈ;}void Save(){if(Γτ||Ρ==Ο.Е){Storage="";return;}Storage=ΓА();}String ΓА(){ȳ.Clear();ȳ.Append(DATAREV+";");Μ(γ(Γσ),false);Μ(γ(Σ.І));Μ(γ(Σ.О));Μ(γ(Σ.Ё));Μ(γ(Σ.π));Μ(γ(Σ.Π));Μ(γ(Σ.Ч));Μ(Σ.ν);Μ(γ(Ψ.Π));Μ(γ(Ψ.π));Μ(γ(Ψ.І));Μ(γ(Ψ.Ё));Μ(γ(Ψ.О));Μ(γ(Ψ.Ч));Μ(Ψ.ν);Μ(γ(ı));Μ(γ(Ȩ));Μ(";");
Μ((int) Ρ,false);Μ((int) φ);Μ((int) ǡ);Μ(ш);Μ(щ);Μ(Ы);Μ(ъ);Μ(ǚ);Μ(Ȍ);Μ(Ǩ);if(Ρ==Ο.ο){Μ((int) χ.П);Μ(χ.е);Μ(χ.Ǵ);Μ(ņ(χ.ф));Μ(ņ(χ.ч));Μ((int) ψ.П);Μ(ψ.е);Μ(ψ.Ǵ);Μ(ņ(ψ.ф));Μ(ņ(ψ.ч));}else{Μ((int) ȇ);Μ((int) ǣ);Μ((int) ġ);Μ((int) ј);Μ((int) ȣ);Μ(Ŀ);Μ(ȉ);Μ(Ĭ);Μ(Ģ);Μ(Ц);Μ(Х);Μ(д);Μ(ƍ);Μ(ƌ);Μ(ŀ);Μ(Ĩ);
Μ(к);Μ(й);Μ(Ů);Μ(ů);Μ(į);Μ(с);Μ(Αμ);Μ(Αέ);}Μ(";");for(int i=0;i<ǎ.Count;i++)
Μ((i>0?"|":"")+ǎ[i],false);Μ(";");for(int i=0;i<ω.Count;i++)
Μ(Εφ(ω[i]),i>0);Μ(";",false);Μ(з+"|"+ȁ+"|"+Ǻ+"|"+ǹ+"|"+ŵ,false);Μ(";",false);Μ(ΔΣ(),false);String Ξ=ȳ.ToString();ȳ.Clear();return Ξ;}п ΕΚ(){return ΕΛ(Storage);}п ΕΛ(String Ξ){if(Ξ=="") return п.ΔЊ;String[]ȅ=Ξ.Split(';');if(Ι(ȅ,0)!=DATAREV) return п.Βε;int Α=0;try
{String[]Ζ=Ι(ȅ,1).Split('\n');Γσ=Ϋ(Ι(Ζ,Α++));Σ.І=Ϋ(Ι(Ζ,Α++));Σ.О=Ϋ(Ι(Ζ,Α++));Σ.Ё=Ϋ(Ι(Ζ,Α++));Σ.π=Ϋ(Ι(Ζ,Α++));Σ.Π=Ϋ(Ι(Ζ,Α++));Σ.Ч=Ϋ(Ι(Ζ,Α++));Σ.ν=bool.Parse(Ι(Ζ,Α++));Ψ.Π=Ϋ(Ι(Ζ,Α++));Ψ.π=Ϋ(Ι(Ζ,Α++));Ψ.І=Ϋ(Ι(Ζ,Α++));Ψ.Ё=Ϋ(Ι(Ζ,Α++));Ψ.О=Ϋ(Ι(Ζ,Α++));Ψ.Ч=Ϋ(Ι(Ζ,Α++));
Ψ.ν=bool.Parse(Ι(Ζ,Α++));ı=Ϋ(Ι(Ζ,Α++));Ȩ=Ϋ(Ι(Ζ,Α++));Ζ=Ι(ȅ,2).Split('\n');Α=0;Ρ=(Ο) int.Parse(Ι(Ζ,Α++));φ=(Τ) int.Parse(Ι(Ζ,Α++));ǡ=(Τ) int.Parse(Ι(Ζ,Α++));ш=int.Parse(Ι(Ζ,Α++));щ=int.Parse(Ι(Ζ,Α++));Ы=int.Parse(Ι(Ζ,Α++));ъ=int.Parse(Ι(Ζ,Α++));ǚ=float.Parse(Ι(Ζ,Α++));
Ȍ=bool.Parse(Ι(Ζ,Α++));Ǩ=bool.Parse(Ι(Ζ,Α++));if(Ρ==Ο.ο){χ.П=(Ѕ) int.Parse(Ι(Ζ,Α++));χ.е=float.Parse(Ι(Ζ,Α++));χ.Ǵ=float.Parse(Ι(Ζ,Α++));χ.ф=ŗ(Ι(Ζ,Α++));χ.ч=ŗ(Ι(Ζ,Α++));ψ.П=(Ѕ) int.Parse(Ι(Ζ,Α++));ψ.е=float.Parse(Ι(Ζ,Α++));ψ.Ǵ=float.Parse(Ι(Ζ,Α++));ψ.ф=ŗ(Ι(Ζ,Α++));ψ.ч=ŗ(Ι(Ζ,Α++));
}else{ȇ=(У) int.Parse(Ι(Ζ,Α++));ǣ=(ǔ) int.Parse(Ι(Ζ,Α++));ġ=(в) int.Parse(Ι(Ζ,Α++));ј=(я) int.Parse(Ι(Ζ,Α++));ȣ=(У) int.Parse(Ι(Ζ,Α++));Ŀ=bool.Parse(Ι(Ζ,Α++));ȉ=bool.Parse(Ι(Ζ,Α++));Ĭ=bool.Parse(Ι(Ζ,Α++));Ģ=bool.Parse(Ι(Ζ,Α++));Ц=int.Parse(Ι(Ζ,Α++));Х=int.Parse(Ι(Ζ,Α++));д=int.Parse(Ι(Ζ,Α++));
ƍ=float.Parse(Ι(Ζ,Α++));ƌ=float.Parse(Ι(Ζ,Α++));ŀ=float.Parse(Ι(Ζ,Α++));Ĩ=float.Parse(Ι(Ζ,Α++));к=int.Parse(Ι(Ζ,Α++));й=int.Parse(Ι(Ζ,Α++));Ů=int.Parse(Ι(Ζ,Α++));ů=int.Parse(Ι(Ζ,Α++));į=int.Parse(Ι(Ζ,Α++));с=int.Parse(Ι(Ζ,Α++));Αμ=int.Parse(Ι(Ζ,Α++));Αέ=float.Parse(Ι(Ζ,Α++));
}Ζ=Ι(ȅ,3).Replace("\n","").Split('|');ǎ=Ζ.ToList();Ζ=Ι(ȅ,4).Split('\n');ω.Clear();if(Ζ.Count()>=1&&Ζ[0]!="")
for(int i=0;i<Ζ.Length;i++)
ω.Add(Εε(Ι(Ζ,i)));Ζ=Ι(ȅ,5).Split('|');з=Ι(Ζ,0);if(!int.TryParse(Ι(Ζ,1),out ȁ)) ȁ=-1;if(!float.TryParse(Ι(Ζ,2),out Ǻ)) Ǻ=0;if(!float.TryParse(Ι(Ζ,3),out ǹ)) ǹ=0;if(!int.TryParse(Ι(Ζ,4),out ŵ)) ŵ=0;if(з.Length!=к*й){з="";ȁ=-1;}ΔΡ(Ι(ȅ,6));}catch{return п.ΒΓ;}Αν=ǡ;
Ǯ();return п.Βυ;}void ΕΫ(){ǳ.Clear();for(int i=Љ.Count-1;i>=0;i--){String Ξ=Љ[i].CustomData.ToUpper();bool ΑΡ=false;if(Ξ=="INSTRUCTIONS"){ΑΡ=true;ΒА=true;}if(Ξ=="DEBUG") ΑΡ=true;if(ΑΡ){ǳ.Add(Љ[i]);Љ.RemoveAt(i);}}Ď(ǳ,false,1,false);}void Εά(List<IMyTerminalBlock>ΒЂ)
{ь.Clear();for(int i=0;i<ΒЂ.Count;i++){IMyTerminalBlock α=ΒЂ[i];try{String Γο=pamTag.Substring(0,pamTag.Length-1)+":";int Α=α.CustomName.IndexOf(Γο);int ă=-1;if(Α<0||!int.TryParse(α.CustomName.Substring(Α+Γο.Length,1),out ă)) continue;if(ă==-1) continue;ă--;IMyTextSurfaceProvider Γρ=(IMyTextSurfaceProvider) α;
if(ă<Γρ.SurfaceCount&&ă>=0)
ь.Add(Γρ.GetSurface(ă));}catch{}}}String Αί="";void ΕΖ(){String ΑΫ="";String ğ="";Αί=ΓЏ(false);ΑΫ+=Αί;ğ+=Αί;ğ+=Δι();for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(ΑΫ);for(int i=0;i<ь.Count;i++) ь[i].WriteText(ΑΫ);Echo(ğ);for(int i=0;i<ǳ.Count;i++){IMyTextPanel Џ=ǳ[i];
String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΚ());}}string ΒΚ(){String Β="";try{float Γδ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ș*Γδ)+" / "+(int)(ȧ*Γδ)+"\n";
Β+="Inst. avg/max: "+Math.Round(ș*100f,1)+"% / "+Math.Round(ȧ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(ĕ,2)+"ms / "+Math.Round(Αή,2)+"ms \n";}catch{}for(int i=0;i<Ɓ.Count;i++)
Β+=""+Ɓ.Keys.ElementAt(i)+" = "+Math.Round(Ɓ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ɓ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Δφ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ď(Љ,setLCDFontAndSize,0.9f,false);Ď(ь,setLCDFontAndSize,0.9f,true);for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(Β+ȥ.ToString());for(int i=0;i<ь.Count;i++) ь[i].WriteText(Β+ȥ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ƴ(String η,String ΰ,String ǵ){try{if(!Ę) return;η=BROADCAST_TAG+BROADCAST_SEP+ΒΗ()+BROADCAST_SEP+ΰ+BROADCAST_SEP+ǈ+BROADCAST_SEP+ǵ+BROADCAST_SEP+η;this.IGC.SendBroadcastMessage(BROADCAST_TAG,η);}catch(Exception e){ȥ=e;}}String ΒΗ(){if(Ρ!=Ο.Е) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΒΫ(String η){return η.StartsWith(BROADCAST_TAG);}bool Є(ref String Ξ,out string ģ,Char Ȇ){int Α=Ξ.IndexOf(Ȇ);ģ="";if(Α<0) return false;ģ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΕΒ(ref String η,out String ǧ,out String ǵ){ǧ="";ǵ="";if(!Ę) return false;
String Ю="";if(!Є(ref η,out Ю,BROADCAST_SEP)||!ΒΫ(Ю)) return false;if(!Є(ref η,out ǧ,BROADCAST_SEP)) return false;if(!Є(ref η,out Ю,BROADCAST_SEP)||(Ю!=ΒΗ()&&(Ю!="*"&&Ρ!=Ο.Е))) return false;if(!Є(ref η,out Ю,BROADCAST_SEP)||(Ю!=ǈ)) return false;if(!Є(ref η,out ǵ,BROADCAST_SEP)) return false;
return true;}void Δώ(){bool ě=true;if(ΓΘ){ΓΘ=false;if(Me.CustomData.Contains("Antenna_Name")){Ϊ="Update custom data";Me.CustomData="";}}String Ħ=(Ρ!=Ο.Е?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ħ)
ΒΨ(Ħ);String[]Ζ=Me.CustomData.Split('\n');Ę=bool.Parse(ō(Ň(Ζ,"Enable_Broadcast",ref ě)));bool ΑЄ=false;bool ΓΑ=true;if(Ę){if(Ρ!=Ο.Е)
Ķ=ō(Ň(Ζ,"Ship_Name",ref ě)).Replace(BROADCAST_SELF_ID,"");ǈ=ō(Ň(Ζ,"Broadcast_Channel",ref ě)).ToLower();ΓΑ=false;if(є==null){є=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);є.SetMessageCallback("");}List<IMyRadioAntenna>Ș=new List<IMyRadioAntenna>();Н.GetBlocksOfType(Ș);bool Ǆ=false;
for(int i=0;i<Ș.Count;i++)
if(Ș[i].EnableBroadcasting&&Ș[i].Enabled){Ǆ=true;break;}if(Ș.Count==0) Ϊ="No Antenna found";else if(!Ǆ) Ϊ="Antenna not ready";ΑЄ=Ș.Count==0||!Ǆ;if(Βϋ&&!ΑЄ&&Ρ!=Ο.Е) Ϊ="Antenna ok";}else if(Ρ==Ο.Е)
Ϊ="Offline - Enable in PB custom data";Βϋ=ΑЄ;if(ΓΑ){if(є!=null) this.IGC.DisableBroadcastListener(є);є=null;}}catch{ě=false;}if(!ě){Ϊ="Reset custom data";ΒΨ(Ħ);}}void ΒΨ(String Ħ){String Ξ=Ħ+"\n\n"+"Enable_Broadcast="+(Ρ==Ο.Е?"true":"false")+" \n";Ξ+=Ρ!=Ο.Е?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ō(String Ǽ){int Α=Ǽ.IndexOf("//");if(Α!=-1) Ǽ=Ǽ.Substring(0,Α);String[]θ=Ǽ.Split('=');if(θ.Length<=1) return "";return θ[1].Trim();}String Ň(String[]Ζ,String И,ref bool ȫ){foreach(String Ǽ in Ζ)
if(Ǽ.StartsWith(И)) return Ǽ;ȫ=false;return "";}void ΔΜ(string А){if(Ĥ) β=new List<ί>();String ǧ="";String ǵ="";if(!ΒΫ(А)) ΕΗ(А);bool Γό=false;int ΖΫ=50;while(є!=null&&є.HasPendingMessage&&ΖΫ-->0){MyIGCMessage η=є.AcceptMessage();String Ξ=η.Data as String;if(Ξ==null) continue;
if(ΕΒ(ref Ξ,out ǧ,out ǵ)&&ǧ!=""&&ǧ!=BROADCAST_SELF_ID){ί Γ=ƚ(ǧ,"");if(Γ==null){Γ=new ί(ǧ);β.Add(Γ);}Γ.Ǘ(ǵ,false,false,true);Επ(Γ,Ξ);Γό=true;}}if(Γό)
β.Sort(delegate(ί a,ί b){if(a.ζ==null&&b.ζ==null) return 0;else if(a.ζ==null) return-1;else if(b.ζ==null) return 1;else return a.ζ.CompareTo(b.ζ);});if(ĭ||Ĥ){if(ƃ<=0||Ĥ){Ϊ="";Ĥ=false;ΕΠ();Δώ();Γή=DateTime.Now;}}if(ǟ) Εξ();if(ǟ) Εο();if(ǟ) Ες();if(ǟ) Ερ();if(ǟ&&Αϋ==0) Εν();
Εμ();}void Επ(ί Γ,String Ξ){Γ.ΑΥ=DateTime.Now;String ģ="";Є(ref Ξ,out Γ.Ɗ,BROADCAST_SEP);Є(ref Ξ,out Γ.ζ,BROADCAST_SEP);if(Γ.Ɗ!=VERSION) return;String ĳ="";String[]Ȯ=new string[3];Є(ref Ξ,out ĳ,BROADCAST_SEP);Є(ref Ξ,out Ȯ[0],BROADCAST_SEP);Є(ref Ξ,out Ȯ[1],BROADCAST_SEP);Є(ref Ξ,out Ȯ[2],BROADCAST_SEP);
Є(ref Ξ,out Γ.Αψ,BROADCAST_SEP);Є(ref Ξ,out Γ.Ϊ,BROADCAST_SEP);Є(ref Ξ,out ģ,BROADCAST_SEP);Є(ref Ξ,out Γ.Ż,BROADCAST_SEP);String ΓІ="",Γζ="";Є(ref Ξ,out ΓІ,BROADCAST_SEP);Є(ref Ξ,out Γζ,BROADCAST_SEP);Γ.њ.Clear();while(true){String ΓΪ;if(!Є(ref Ξ,out ΓΪ,BROADCAST_SEP)) break;String[]ų=ΓΪ.Split('/');
if(ų.Count()<3) continue;int Ј=0;if(!int.TryParse(ų[2],out Ј)) continue;Γ.њ.Add(new Ǭ(ų[0],ų[1],Ј,ŉ.Δϋ));}int Γθ=0;if(!int.TryParse(ģ,out Γ.ς)) Γ.ς=0;if(!int.TryParse(ĳ,out Γθ)) Γ.Ρ=Ο.Ǚ;if(!float.TryParse(Ȯ[0],out Γ.Π.X)) Γ.Π.X=0;if(!float.TryParse(Ȯ[1],out Γ.Π.Y)) Γ.Π.Y=0;
if(!float.TryParse(Ȯ[2],out Γ.Π.Z)) Γ.Π.Z=0;if(!float.TryParse(ΓІ,out Γ.Ǳ)) Γ.Ǳ=0;if(!float.TryParse(Γζ,out Γ.ǽ)) Γ.ǽ=0;Γ.Ρ=(Ο) Γθ;Γ.ΑΒ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Π));Γ.ΑЀ=0;for(int i=0;i<Γ.њ.Count();i++) Γ.ΑЀ+=Γ.њ[i].Ј;}ί ƚ(String Λ,String ζ){Λ=Λ.ToUpper();ζ=ζ.ToUpper();
for(int i=0;i<β.Count;i++){if(Λ!=""&&β[i].Λ.ToUpper()==Λ) return β[i];if(ζ!=""&&β[i].ζ!=null&&β[i].ζ.ToUpper()==ζ) return β[i];}return null;}String ț="";ί ΒЅ=null;DateTime ΒІ=DateTime.MinValue;List<ί>і=new List<ί>();const float ΔΓ=20f;bool Βπ(ί ΰ,String μ){return μ==ț&&ΰ==ΒЅ&&((DateTime.Now-ΒІ).TotalSeconds<ΔΓ||
(μ=="HOMEPOS"&&(Щ.Count>0||х!=null)));}String Δα(ί ΰ){if(ț==""||!Βπ(ΰ,ț)) return "";String s="";int ȫ=0,Αρ=0;for(int i=0;i<і.Count;i++){ι ƅ=і[i].Ǘ(ț,false,false,false);if(ƅ==ι.č) ȫ++;bool Ζκ=Щ.Contains(і[i]);if(Αρ++<8) s+="  "+і[i].ζ+": "+(Ζκ?"waiting to leave ("+(Щ.IndexOf(і[i])+1)+")":ƅ==ι.č?"received":
ƅ==ι.Ȕ?"pending...":ƅ==ι.Ō?"no answer!":"-")+"\n";}if(Αρ>8) s+="  +"+(Αρ-8)+" more\n";return "--------------------------------------------\n"+ț+": "+ȫ+"/"+і.Count+" received\n"+s;}void Т(ί Γ,String μ){ț=μ;ΒЅ=Γ;ΒІ=DateTime.Now;і.Clear();if(Γ==null) і.AddRange(β);else і.Add(Γ);
if(Γ==null){Щ.Clear();х=null;}else Щ.Remove(Γ);if(Γ==null&&μ=="HOMEPOS"){Εΰ();return;}if(Γ==null){for(int i=0;i<β.Count;i++)
β[i].Ǘ(μ,true,false,false);ƴ(μ,"*",μ);}else{Γ.Ǘ(μ,true,false,false);ƴ(μ,Γ.Λ,μ);}}void ΕΗ(string А){if(А=="") return;var θ=А.ToUpper().Split(' ');θ.DefaultIfEmpty("");var μ=θ.ElementAtOrDefault(0);var Βύ=θ.ElementAtOrDefault(1);String ΖΝ="Invalid argument: "+А;if(δ==Κ.ď){if(σ!=null)
{switch(μ){case "UP":{if(ς<2) break;if(σ.ς==0){ς=1;return;}Т(σ,"UP");return;}case "DOWN":{if(ς<1) break;if(ς==1){Т(σ,"MRES");break;}Т(σ,"DOWN");break;}case "APPLY":{if(ς<2) break;Т(σ,"APPLY");return;}}}}switch(μ){case "UP":this.ΔЈ(false);return;case "DOWN":this.ΔЇ(false);return;
case "APPLY":this.ȏ(true);return;}switch(μ){case "CLEAR":ΔΙ();return;case "SENDALL":
if(String.IsNullOrEmpty(Βύ)){Ϊ="Usage: SENDALL <cmd>";return;}Т(null,Βύ);return;case "SEND":
if(А.Length<="SEND".Length+1){Ϊ="Usage: SEND <ship>:<cmd>";return;}ΕΣ(А.Remove(0,"SEND".Length+1));return;}Ϊ=ΖΝ;}void ΕΣ(String Βό){if(Βό=="") return;var θ=Βό.Split(':');if(θ.Length!=2){Ϊ="Missing separator \":\"";return;}θ.DefaultIfEmpty("");String Ķ=θ.ElementAtOrDefault(0).Trim();
ί Γ=ƚ("",Ķ);if(Γ!=null) Т(Γ,θ.ElementAtOrDefault(1).Trim());else Ϊ="Unknown ship: "+Ķ;}void ΔΙ(){β.Clear();σ=null;Ω(Κ.Main);ς=0;}void ΕΠ(){List<IMyTerminalBlock>Γώ=new List<IMyTerminalBlock>();Н.GetBlocksOfType(Љ,Ʀ);Н.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Γώ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Љ=ΒΔ(Љ,pamTag,true);Εά(Γώ);ΕΫ();ΕΡ();Ď(Љ,setLCDFontAndSize,1.15f,false);Ď(ь,setLCDFontAndSize,1.15f,true);String ΖΙ=ΒΛ(false);String Γλ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Џ in Љ){if(Џ.CustomData==""){Џ.CustomData=ΒΛ(true);continue;}
if(!Џ.CustomData.Contains(ΖΙ))
if(!Џ.CustomData.Contains(Γλ)) Џ.CustomData=Γλ+Џ.CustomData;}}void Εμ(){String ğ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ь.Count;i++)
ь[i].WriteText(ΑЍ(ϊ.Βέ,"0",false));for(int i=0;i<Љ.Count;i++){ϊ λ=ϊ.Ǚ;String Э="";ΕΓ(Љ[i],out λ,out Э);Љ[i].WriteText(ΑЍ(λ,Э,i==0));}Echo(ğ+Δδ());for(int i=0;i<ǳ.Count;i++){IMyTextPanel Џ=ǳ[i];String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");
if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΚ());}}String ȏ(bool Ε){int ΕЋ=0;return ȏ(Ε,0,ref ΕЋ,false,1);}String ȏ(bool Ε,int Ζγ,ref int Ζρ,bool ΒЉ,int Ĵ){String Β="";String ā="——————————————————\n";String Ζώ="--------------------------------------------\n";if(σ==null||δ==Κ.Main||δ==Κ.Ƶ)
Β+="[PAM]-Controller | "+β.Count+" Connected ships"+"\n"+Δζ()+Δμ()+Δς();else Β+=ΒΞ(σ)+"\n";Β+=ā;int Α=0;int Δ=ς;Ǿ=0;if(Ϊ!=""){Η(ref Β,Δ,Α++,Ε,Ϊ);}else if(δ==Κ.Main){if(σ==null&&β.Count>=1) σ=β[0];String Ї="";if(Η(ref Ї,Δ,Α++,Ε," Send command to all")) Ω(Κ.Ƶ);
int Ā=0;for(int s=0;s<β.Count;s++){if(ΒЉ) Ї+="\n";ί Γ=β[s];if(Δ==Α||Δ==Α+1) σ=Γ;if(Α==Δ) Ā=Ї.Split('\n').Length-1;if(Η(ref Ї,Δ,Α++,Ε," "+ΒΞ(Γ))){σ=Γ;Ω(Κ.ď);σ.Ǘ("",false,true,false);}if(Α==Δ) Ā=Ї.Split('\n').Length-1;if(Η(ref Ї,Δ,Α++,Ε," "+(ΒЉ?"":"  ")+Δξ(Γ)))
{σ=Γ;Ω(Κ.Βο);}}int ć=Ĵ-2;ć+=Math.Max(0,(Ζγ-1))*Ĵ;Β+=Ƴ(ć,Ї,Ā,ref Ζρ);}else if(δ==Κ.Ŏ||δ==Κ.Ƶ){Κ Εϊ=δ==Κ.Ŏ?Κ.ď:Κ.Main;ί ΰ=null;if(δ==Κ.Ŏ) ΰ=σ;if(Η(ref Β,Δ,Α++,Ε," Back")){Ω(Εϊ);}if(Η(ref Β,Δ,Α++,Ε," [Stop] "+ȑ(ΰ,"STOP"))){Т(ΰ,"STOP");}if(Η(ref Β,Δ,Α++,Ε," [Continue] "+ȑ(ΰ,"CONT"))){Т(ΰ,"CONT");}
if(Η(ref Β,Δ,Α++,Ε," [Move home] "+ȑ(ΰ,"HOMEPOS"))){Т(ΰ,"HOMEPOS");}if(Η(ref Β,Δ,Α++,Ε," [Move to job] "+ȑ(ΰ,"JOBPOS"))){Т(ΰ,"JOBPOS");}if(Η(ref Β,Δ,Α++,Ε," [Full simulation] "+ȑ(ΰ,"FULL"))){Т(ΰ,"FULL");}if(ΰ!=null&&ΰ.Ρ==Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," [Undock] "+ȑ(ΰ,"UNDOCK"))){Т(ΰ,"UNDOCK");}Β+=Δα(ΰ);}else if(δ==Κ.ď){String ǆ="";if(Η(ref ǆ,Δ,Α++,Ε," Back")){Ω(Κ.Main);}Β+=ǆ.Substring(0,ǆ.Length-1).PadRight(36,' ');if(Η(ref Β,Δ,Α++,Ε," Send cmd")){Ω(Κ.Ŏ);}if(!σ.Ƨ()&&!σ.Ƥ()) Ǿ++;if(σ.Ƥ()) Β+="No answer received...";
else Β+=Ȗ(σ,σ.Ż);}else if(δ==Κ.Βο){if(Η(ref Β,Δ,Α++,Ε," Back")){Ω(Κ.Main);}Β+=Ζώ;Β+=Ȗ(σ,ΒΝ(σ));}if(!Αΰ) ς=Math.Min(Ǿ-1,ς);return Β;}String ΒΞ(ί Γ){if(Γ.Ɗ!=VERSION) return Γ.ζ+": Different version!";return Γ.ζ+": "+Ȗ(Γ,Γ.Αψ)+" "+Γ.ΑΒ+"m";}String Δξ(ί Γ){String s=Ʊ("",С(Γ.Ǳ,Γ.ǽ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.њ.Count;i++){if(i>=5) break;Ǭ ц=Γ.њ[i];s+=Βω(Ɠ(Δκ(ц)),3)+" "+ΒΕ(ц.Ј)+" ";}return s;}String Δκ(Ǭ ц){if(ц.Ќ.ToUpper()=="ORE"||ц.Ќ.ToUpper()=="INGOT"){String ΒЇ=Δθ(ц.ζ.ToUpper());if(ΒЇ!="") return ΒЇ;}return ц.ζ;}String Ȗ(ί Γ,String Β){if(Γ.Ƨ()) return "No signal...("+ƞ((int)(DateTime.Now-Γ.ΑΥ).TotalSeconds)+")";
return Β;}String ΒΛ(bool ΖЎ){String Β="";if(ΖЎ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΕΓ(IMyTerminalBlock Џ,out ϊ λ,out String Э){bool ȫ=true;String ĳ=ō(Ň(Џ.CustomData.Split('\n'),"mode",ref ȫ)).ToUpper();String[]θ=ĳ.Split(':');String Ĳ=θ.Length>0?θ[0].Trim():"";Э=θ.Length>1?θ[1].Trim():"";
λ=ϊ.Ǚ;if(Ĳ=="MAIN") λ=ϊ.Βή;else if(Ĳ=="MAINX") λ=ϊ.Βί;else if(Ĳ=="MENU") λ=ϊ.ƪ;else if(Ĳ=="INVENTORY") λ=ϊ.ƣ;else if(Ĳ=="DEBUG") λ=ϊ.Ɩ;if(λ==ϊ.Ǚ) Э=ĳ;}String ΑЍ(ϊ λ,String Э,bool ΕЎ){int İ=15;if(ΕЎ){ΒЊ=Š;Š=0;ΒЋ=š;š=0;}String Β="";if(λ==ϊ.Βή){int Ў=0;
Β=ȏ(false,ΒЊ,ref ΕЅ,true,İ);if(!int.TryParse(Э,out Ў)) return Β;Š=Math.Max(Ў,Š);Ў--;return Βκ(Ў,İ,Β);}else if(λ==ϊ.Βί){int Ў=0;Β=ȏ(false,ΒЋ,ref ΕІ,false,İ);if(!int.TryParse(Э,out Ў)) return Β;š=Math.Max(Ў,š);Ў--;return Βκ(Ў,İ,Β);}if(λ==ϊ.Βέ){return ȏ(false,0,ref ΕЇ,true,İ);
}else if(λ==ϊ.ƣ||λ==ϊ.ƪ||λ==ϊ.Ɩ){ί Γ=σ;if(Э!=""){Γ=ƚ("",Э);if(Γ==null) return "Unknown ship: "+Э;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ā="—————————————————————\n";String Αώ="";String Ǉ="";if(λ==ϊ.ƪ){Αώ="Menu";Ǉ=Ȗ(Γ,Γ.Ż);}
else if(λ==ϊ.ƣ){Αώ="Inventory";Ǉ=Ȗ(Γ,ΒΝ(Γ));}else if(λ==ϊ.Ɩ){return Ȗ(Γ,Δο(Γ));}Β+=Γ.ζ+" - "+Γ.ΑΒ+"m | "+Ɠ(""+Αώ)+"\n"+ā;Β+=Ǉ;return Β;}return "Unknown command: "+Э;}string Δο(ί Γ){String s="";s+=Γ.Ɗ+"\n";s+=Γ.ζ+"\n";s+=Γ.Π.X+"\n";s+=Γ.Π.Y+"\n";
s+=Γ.Π.Z+"\n";s+=Γ.Αψ+"\n";s+=Γ.Ϊ+"\n";s+=Γ.ς+"\n";s+=Γ.Ż.Length+"\n";s+=Γ.Ǳ+"\n";s+=Γ.ǽ+"\n";s+=Γ.њ.Count()+"\n";return s;}string ΒΝ(ί Γ){String s="";s+=Ʊ("All",С(Γ.Ǳ,Γ.ǽ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.њ.Count();i++){Ǭ ц=Γ.њ[i];
s+=Ʊ(ц.ζ,ц.Ј,Γ.ΑЀ,30,8,10)+"\n";}return s;}ι Δβ(ί Γ,bool ѕ,string μ){ι έ=ι.None;if(Γ==null){for(int i=0;i<β.Count;i++){ι Ѐ=β[i].Ǘ(μ,false,ѕ,false);if(Ѐ==ι.č) έ=Ѐ;if(Ѐ==ι.Ō) return Ѐ;if(Ѐ==ι.Ȕ) return Ѐ;}return έ;}else return Γ.Ǘ(μ,false,ѕ,false);}String ȑ(ί Γ,String μ)
{if(!Βπ(Γ,μ)) return "";ι Ѐ=Δβ(Γ,false,μ);if(Ѐ==ι.č) return "received!";if(Ѐ==ι.Ȕ) return "pending...";if(Ѐ==ι.Ō) return "no answer!";return "";}String Ƴ(int ć,String Β,int Ā,ref int ѝ){String[]Ζ=Β.Split('\n');if(Ā>=ѝ+ć-1) ѝ++;ѝ=Math.Min(Ζ.Count()-1-ć,ѝ);if(Ā<ѝ+1) ѝ--;
ѝ=Math.Max(0,ѝ);String έ="";for(int i=0;i<ć;i++){int Γα=i+ѝ;if(Γα>=Ζ.Count()) break;έ+=Ζ[Γα]+"\n";}return έ;}String Βκ(int Ў,int Ĵ,String Β){String[]Ζ=Β.Split('\n');int Αχ=Ў*Ĵ;int Ŭ=(Ў+1)*Ĵ;String έ="";for(int i=Αχ;i<Ŭ;i++){if(i>=Ζ.Count()) break;έ+=Ζ[i]+"\n";
}return έ;}String Ɠ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Βω(String s,int Γε){if(s=="") return s;if(s.Length>Γε) s=s.Substring(0,Γε-1)+".";return s;}string Ʊ(String ǻ,float value,float ŷ,int Βώ,int Γέ,int Ζβ){float Ȅ=С(value,ŷ)*Βώ;
String ś="[";for(int i=0;i<Βώ;i++){if(i<=Ȅ) ś+="|";else ś+="'";}ś+="]";return ś+" "+Βω(Ɠ(ǻ),Γέ).PadRight(Γέ)+"".PadRight(Ζβ)+ΒΕ(value);}String ΒΕ(float Ј){if(Ј>=1000000) return Math.Round(Ј/1000000f,Ј/1000000f<100?1:0)+"M";if(Ј>=1000) return Math.Round(Ј/1000f,Ј/1000f<100?1:0)+"K";
return ""+Math.Round(Ј);}String ƞ(int ĵ){if(ĵ>=60*60) return Math.Round(ĵ/(60f*60f),1)+" h";if(ĵ>=60) return Math.Round(ĵ/60f,1)+" min";return ""+ĵ+" s";}List<IMyCameraBlock>ΕϏ=new List<IMyCameraBlock>();bool Ζδ=false;int ΕЁ=0,ΗΙ=0,ΗΗ=0;String Εύ="";String Εώ="";
void ΔΘ(){Ζδ=false;ΕЁ=0;Εύ="";}const String Ει="[PAMTFC]";long Ζά=0;String з="";int ȁ=-1;float ΔЅ(){return Ǻ>0?Ǻ:ǐ;}float ΔЄ(){return ǹ>0?ǹ:Ǐ;}List<int>ΑΞ=new List<int>();String ΓΡ="";void ΔΨ(){String И=к+"|"+й+"|"+(int) ȣ;if(И==ΓΡ) return;ΓΡ=И;ΑЎ(к,й,ȣ,ΑΞ);
}void ΑЎ(int w,int h,У Αχ,List<int>Ǌ){Ǌ.Clear();if(Αχ==У.ŕ){for(int k=0;k<w*h;k++){int Ǧ=k/w;Ǌ.Add(Ǧ*w+(Ǧ%2==0?k-Ǧ*w:w-1-(k-Ǧ*w)));}return;}int ΓΟ=(int) Math.Ceiling(w/2f),ΓΞ=(int) Math.Ceiling(h/2f);int ΖΗ=w/2,ΖΖ=h/2;int x=0,y=0,ΑΓ=0,ΑΔ=-1,ŷ=Math.Max(w,h);
for(int i=0;i<ŷ*ŷ;i++){if(-ΓΟ<x&&x<=ΖΗ&&-ΓΞ<y&&y<=ΖΖ) Ǌ.Add((y-1+ΓΞ)*w+(x-1+ΓΟ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΑΓ;ΑΓ=-ΑΔ;ΑΔ=t;}x+=ΑΓ;y+=ΑΔ;}}bool ΔΧ(){int n=Math.Max(0,к*й);if(з.Length==n) return false;з=new String('0',n);
ȁ=-1;return true;}void ΕΩ(int Ж,bool κ){if(Ж<0||Ж>=з.Length) return;char[]h=з.ToCharArray();h[Ж]=κ?'1':'0';з=new String(h);}const String ΔЃ="[PAMJOB]";const String Εη="[PAMTCC]";class ȕ{public long Λ;public Vector3 Φ,М,Ѓ;}List<ȕ>В=new List<ȕ>();bool ĝ=false;Vector3 ũ,Ũ,Ū,ū;
long Ī=0;long Γξ=0;ά Γν=null;bool ΕЉ=false;ά ΔΤ(Vector3 Φ,Vector3 М,Vector3 Ѓ){MatrixD m=MatrixD.CreateWorld(Φ,М,Ѓ);ά w=new ά(Σ);w.І=Vector3D.TransformNormal(ũ,m);w.Ё=Vector3D.TransformNormal(Ũ,m);w.О=Vector3D.TransformNormal(Ū,m);w.Π=Φ+(Vector3) Vector3D.TransformNormal(ū,m);
w.ν=true;return w;}void ΔІ(){if(Ρ==Ο.ο||!Σ.ν) return;IMyShipConnector ї=Ȑ(MyShipConnectorStatus.Connected);if(ї==null||ї.OtherConnector==null) return;IMyShipConnector Й=ї.OtherConnector;Vector3I ΖΚ=new Vector3I((int) Σ.Ч.X,(int) Σ.Ч.Y,(int) Σ.Ч.Z);if(ї.Position==ΖΚ){MatrixD Ų=MatrixD.Transpose(Й.WorldMatrix);
ũ=Vector3D.TransformNormal(Њ.WorldMatrix.Forward,Ų);Ũ=Vector3D.TransformNormal(Њ.WorldMatrix.Down,Ų);Ū=Vector3D.TransformNormal(Њ.WorldMatrix.Left,Ų);ū=Vector3D.TransformNormal(ї.GetPosition()-Й.GetPosition(),Ų);ĝ=true;}if(Vector3.Distance(ї.GetPosition(),Σ.Π)<5) Ī=Й.EntityId;List<IMyShipConnector>Ƕ=new List<IMyShipConnector>();
Н.GetBlocksOfType(Ƕ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(Ƕ.Count>0){В.Clear();for(int i=0;i<Ƕ.Count;i++){ȕ p=new ȕ();p.Λ=Ƕ[i].EntityId;p.Φ=Ƕ[i].GetPosition();p.М=Ƕ[i].WorldMatrix.Forward;p.Ѓ=Ƕ[i].WorldMatrix.Up;В.Add(p);}}Γν=null;Γξ=0;
if(Й.EntityId==Ī||!ĝ) return;for(int i=0;i<В.Count;i++)
if(В[i].Λ==Й.EntityId){Γξ=В[i].Λ;Γν=ΔΤ(В[i].Φ,В[i].М,В[i].Ѓ);}}String ΔΣ(){String s=(ĝ?"1":"0")+":"+γ(ũ)+":"+γ(Ũ)+":"+
γ(Ū)+":"+γ(ū)+":"+Ī;for(int i=0;i<В.Count;i++)
s+=":"+В[i].Λ+","+γ(В[i].Φ)+","+γ(В[i].М)+","+γ(В[i].Ѓ);return s;}void ΔΡ(String s){В.Clear();ĝ=false;Ī=0;String[]f=s.Split(':');if(f.Length<6) return;ĝ=f[0]=="1";ũ=Ϋ(f[1]);Ũ=Ϋ(f[2]);Ū=Ϋ(f[3]);ū=Ϋ(f[4]);long.TryParse(f[5],out Ī);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ȕ p=new ȕ();if(!long.TryParse(e[0],out p.Λ)) continue;p.Φ=Ϋ(e[1]);p.М=Ϋ(e[2]);p.Ѓ=Ϋ(e[3]);В.Add(p);}}const String ƕ="[PAMDCK]";const float ΔΒ=120f;class ѐ{public long Γ;public int Ǡ;public long и;public bool ΒЃ;public DateTime Л,Ȥ;
}IMyBroadcastListener ŧ=null;List<ѐ>ό=new List<ѐ>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMySensorBlock>б=new List<IMySensorBlock>();List<IMyCameraBlock>ћ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ǝ=new List<MyDetectedEntityInfo>();long Б=0;int Ş=0;
long ŝ=0;DateTime ΒЄ=DateTime.Now;String љ="";int ΓΔ=0;IMyCubeGrid џ=null;void ΕΡ(){џ=Me.CubeGrid;Н.GetBlocksOfType(ў,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(dockPoolTag));Н.GetBlocksOfType(б,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(zoneTag));Н.GetBlocksOfType(ћ,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(laneTag));
for(int i=0;i<б.Count;i++){б[i].DetectSmallShips=true;б[i].DetectLargeShips=true;}for(int i=0;i<ћ.Count;i++) ћ[i].EnableRaycast=true;}IMyShipConnector ƺ(long Λ){if(Λ==0) return null;IMyShipConnector c=Н.GetBlockWithId(Λ) as IMyShipConnector;return c!=null&&џ!=null&&c.CubeGrid.IsSameConstructAs(џ)?c:null;
}bool ƥ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Βά(IMyShipConnector c,long Γ){for(int i=0;i<ћ.Count;i++){IMyCameraBlock Ě=ћ[i];if(!Ě.IsWorking||Vector3D.Distance(Ě.GetPosition(),c.GetPosition())>10) continue;
Vector3D Ŭ=Ě.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ě.CanScan(Ŭ)) continue;MyDetectedEntityInfo ŭ=Ě.Raycast(Ŭ);if(!ŭ.IsEmpty()&&ŭ.EntityId!=џ.EntityId&&ŭ.EntityId!=Γ&&ŭ.Type!=MyDetectedEntityType.FloatingObject){љ="Lane blocked: "+c.CustomName;return false;}
}return true;}bool ΔЁ(){for(int s=0;s<б.Count;s++){if(!б[s].IsWorking) continue;Ǝ.Clear();б[s].DetectedEntities(Ǝ);for(int i=0;i<Ǝ.Count;i++){MyDetectedEntityInfo e=Ǝ[i];bool ΖΓ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;if(ΖΓ&&e.EntityId!=Б&&e.Velocity.Length()>0.5f)
{љ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΕΔ(ѐ r){IMyShipConnector и=ƺ(r.и);if(ƥ(и)&&Βά(и,r.Γ)) return и.EntityId;if(!r.ΒЃ||и==null) return 0;IMyShipConnector Ѝ=null;double ΒЁ=0;for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];if(!ƥ(c)) continue;
double d=Vector3D.Distance(c.GetPosition(),и.GetPosition());if(Ѝ==null||d<ΒЁ){if(Βά(c,r.Γ)){Ѝ=c;ΒЁ=d;}}}return Ѝ==null?0:Ѝ.EntityId;}void Δσ(){ѐ Ȭ=null;long ї=0;for(int ƀ=2;ƀ>=1&&Ȭ==null;ƀ--){ѐ Ȃ=null;for(int i=0;i<ό.Count;i++){ѐ r=ό[i];if(r.Ǡ!=ƀ) continue;
if(ƀ==1){long c=ΕΔ(r);if(c==0) continue;if(Ȃ==null||r.Л<Ȃ.Л){Ȃ=r;ї=c;}}else if(Ȃ==null||r.Л<Ȃ.Л) Ȃ=r;}Ȭ=Ȃ;}if(Ȭ==null){if(ό.Count>0&&љ=="") љ="No free dock";return;}Б=Ȭ.Γ;Ş=Ȭ.Ǡ;ŝ=Ȭ.Ǡ==1?ї:0;ΒЄ=DateTime.Now;}void Εξ(){if(џ==null) return;
if(ŧ==null) ŧ=IGC.RegisterBroadcastListener(ƕ);int ę=30;while(ŧ.HasPendingMessage&&ę-->0){String Ξ=ŧ.AcceptMessage().Data as String;if(Ξ!=null) Δυ(Ξ);}ΕΞ();if(++ΓΔ<3) return;ΓΔ=0;IGC.SendBroadcastMessage(ƕ,ΓБ());}void Δυ(String Ξ){try{String[]f=Ξ.Split(';');long Γ=long.Parse(f[1]);
if(f[0]=="D"){for(int i=ό.Count-1;i>=0;i--) if(ό[i].Γ==Γ) ό.RemoveAt(i);if(Б==Γ) Б=0;}else if(f[0]=="R"&&f.Length>=5){long и=long.Parse(f[3]);if(ƺ(и)==null) return;ѐ r=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Γ) r=ό[i];if(r==null){r=new ѐ();r.Γ=Γ;r.Л=DateTime.Now;ό.Add(r);}
int Ǡ=int.Parse(f[2]);if(r.Ǡ!=Ǡ) r.Л=DateTime.Now;r.Ǡ=Ǡ;r.и=и;r.ΒЃ=f[4]=="1";r.Ȥ=DateTime.Now;}}catch{}}void ΕΞ(){for(int i=ό.Count-1;i>=0;i--)
if((DateTime.Now-ό[i].Ȥ).TotalSeconds>3) ό.RemoveAt(i);ѐ Ŝ=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Б) Ŝ=ό[i];if(Б!=0&&Ŝ==null) Б=0;if(Ŝ!=null&&(DateTime.Now-ΒЄ).TotalSeconds>ΔΒ){Б=0;Ŝ.Л=DateTime.Now;}љ="";if(Б==0&&ό.Count>0&&!ΔЁ()) Δσ();}String ΓБ()
{String η="S;"+џ.EntityId+";"+Б+";"+Ş+";"+ŝ+";";for(int i=0;i<ό.Count;i++) η+=(i>0?",":"")+ό[i].Γ;η+=";";for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];η+=(i>0?":":"")+c.EntityId+","+γ(c.GetPosition())+","+γ(c.WorldMatrix.Forward)+","+γ(c.WorldMatrix.Up);
}η+=";";for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();η+=(i>0?":":"")+γ(z.GetPosition())+","+Math.Round(r,1);}return η;}String Δζ()
{if(ў.Count==0&&б.Count==0) return "";int ΓΙ=0;for(int i=0;i<ў.Count;i++) if(ƥ(ў[i])) ΓΙ++;return "Docks: "+ΓΙ+"/"+ў.Count+" free"+(Б!=0?(Ş==1?" | docking":" | undocking"):"")+
(ό.Count>1?" | "+(ό.Count-1)+" waiting":"")+(љ!=""?"\n"+љ:"")+"\n";}int ΓΓ=0;bool ΖЈ=false;void ΕΘ(){if(ΓΓ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƕ,"D;"+Ζά);ΓΓ=0;ΖЈ=false;}const float ΔЂ=600f;class р{public long Λ;public int Ж=-1;public bool ΓЇ;public float ΑΘ,ΑΗ;
public DateTime Αξ;}class г{public String И;public int w,h;public float ΑЁ,Αό;public String κ;public List<int>ſ=new List<int>();public List<р>Ћ=new List<р>();}List<г>З=new List<г>();IMyBroadcastListener Ŷ=null;int Γά=0;void Εο(){if(Ŷ==null) Ŷ=IGC.RegisterBroadcastListener(Ει);int ę=50;
while(Ŷ.HasPendingMessage&&ę-->0){String Ξ=Ŷ.AcceptMessage().Data as String;if(Ξ==null) continue;try{Δχ(Ξ);}catch{}try{Δψ(Ξ);}catch{}}for(int j=З.Count-1;j>=0;j--){г Θ=З[j];for(int i=Θ.Ћ.Count-1;i>=0;i--)
if((DateTime.Now-Θ.Ћ[i].Αξ).TotalSeconds>ΔЂ) Θ.Ћ.RemoveAt(i);if(Θ.Ћ.Count==0){З.RemoveAt(j);continue;}ΓЌ(Θ);}if(++Γά<3) return;Γά=0;for(int j=0;j<З.Count;j++) IGC.SendBroadcastMessage(ΔЃ,ΓЎ(З[j]));}void ΓЌ(г Θ){for(int i=0;i<Θ.Ћ.Count;i++){р s=Θ.Ћ[i];if(s.ΓЇ&&(s.Ж<0||Θ.κ[s.Ж]=='1')) s.Ж=ΔЉ(Θ,s);
}}String ΓЎ(г Θ){String η="J;"+Θ.И+";"+Θ.κ+";";bool Л=true;for(int i=0;i<Θ.Ћ.Count;i++){if((DateTime.Now-Θ.Ћ[i].Αξ).TotalSeconds>5) continue;η+=(Л?"":",")+Θ.Ћ[i].Λ+":"+Θ.Ћ[i].Ж;Л=false;}return η;}void Δχ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;г Θ=null;for(int i=0;i<З.Count;i++) if(З[i].И==f[4]) Θ=З[i];if(Θ==null){Θ=new г();Θ.И=f[4];Θ.w=int.Parse(k[3]);Θ.h=int.Parse(k[4]);Θ.ΑЁ=float.Parse(k[6]);Θ.Αό=float.Parse(k[7]);if(Θ.w<1||Θ.h<1) return;ΑЎ(Θ.w,Θ.h,(У) int.Parse(k[5]),Θ.ſ);
Θ.κ=new String('0',Θ.w*Θ.h);З.Add(Θ);}String d=f[6];if(d.Length==Θ.κ.Length){char[]c=Θ.κ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Θ.κ=new String(c);}long Λ=long.Parse(f[0]);р s=null;for(int i=0;i<Θ.Ћ.Count;i++) if(Θ.Ћ[i].Λ==Λ) s=Θ.Ћ[i];
if(s==null){s=new р();s.Λ=Λ;Θ.Ћ.Add(s);}s.Αξ=DateTime.Now;String[]w=f[10].Split(',');s.ΓЇ=w[0]=="1";if(w.Length>=3){s.ΑΘ=float.Parse(w[1]);s.ΑΗ=float.Parse(w[2]);}int ĩ=int.Parse(f[5]);if(ĩ>=0&&ĩ<Θ.κ.Length&&(s.Ж<0||Θ.κ[s.Ж]=='1')&&Θ.κ[ĩ]!='1'&&ơ(Θ,ĩ,s)==null)
s.Ж=ĩ;}р ơ(г Θ,int Ж,р ΕЍ){for(int i=0;i<Θ.Ћ.Count;i++){р o=Θ.Ћ[i];if(o!=ΕЍ&&o.Ж==Ж)
return o;}return null;}int ΔЉ(г Θ,р s){for(int k=0;k<Θ.ſ.Count;k++){int c=Θ.ſ[k];if(Θ.κ[c]!='1'&&ơ(Θ,c,s)==null) return c;}return-2;}String Δμ(){String s="";for(int j=0;j<З.Count;j++){г Θ=З[j];int κ=0;for(int i=0;i<Θ.κ.Length;i++) if(Θ.κ[i]=='1') κ++;s+="Job "+(j+1)+": "+κ+"/"+Θ.κ.Length+" holes, "+Θ.Ћ.Count+" ships\n";
}return s;}const float Εζ=8f;class ϋ{public long Λ,ȃ;public Vector3 Φ,ю,ħ;public float ȝ,ΑΘ,ΑΗ,л=-1;public int Ǹ;public int Ǥ;public float Ȁ=-1;public String Θ="";public int Αε;public DateTime Ȉ;}class Ŕ{public long a,b,ΑЊ;public DateTime Ȥ;}List<ϋ>Χ=new List<ϋ>();List<Ŕ>Г=new List<Ŕ>();
int Αϋ=0;void Δψ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<9) return;long Λ=long.Parse(f[0]);ϋ s=null;for(int i=0;i<Χ.Count;i++) if(Χ[i].Λ==Λ) s=Χ[i];if(s==null){s=new ϋ();s.Λ=Λ;Χ.Add(s);}s.Φ=Ϋ(f[1]);s.ю=Ϋ(f[2]);s.ȝ=float.Parse(f[3]);s.Θ=f[4];s.Αε=int.Parse(f[7].Split(',')[1]);
s.ħ=Ϋ(f[8]);s.Ǹ=0;s.Ǥ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΘ=float.Parse(w[1]);s.ΑΗ=float.Parse(w[2]);s.Ǹ=int.Parse(w[3]);if(w.Length>=5) s.Ǥ=int.Parse(w[4]);s.Ȁ=w.Length>=6?float.Parse(w[5]):-1;}}s.ȃ=f.Length>=12?long.Parse(f[11]):0;
s.Ȉ=DateTime.Now;}bool Đ(ϋ s){return s.ю.LengthSquared()<0.25f&&(s.Αε==3||s.ħ.LengthSquared()<0.25f);}Vector3 Βφ(ϋ s){return s.ю.LengthSquared()>=1f?s.ю:s.ħ*2f;}void Ες(){ΔΚ();if(++Αϋ<3) return;Αϋ=0;IGC.SendBroadcastMessage(Εη,ΔΑ());}void ΔΚ(){for(int i=Χ.Count-1;i>=0;i--)
if((DateTime.Now-Χ[i].Ȉ).TotalSeconds>3) Χ.RemoveAt(i);for(int i=Г.Count-1;i>=0;i--)
if((DateTime.Now-Г[i].Ȥ).TotalSeconds>2) Г.RemoveAt(i);for(int i=0;i<Χ.Count;i++) Χ[i].л=-1;for(int i=0;i<Χ.Count;i++)
for(int j=i+1;j<Χ.Count;j++){ϋ a=Χ[i],b=Χ[j];if(Đ(a)||Đ(b)) continue;Vector3 Ζα=a.Φ+a.ю*(float)(DateTime.Now-a.Ȉ).TotalSeconds;Vector3 ȃ=b.Φ+b.ю*(float)(DateTime.Now-b.Ȉ).TotalSeconds;Vector3 ą=Βφ(a),Ć=Βφ(b);Vector3 ȯ=ȃ-Ζα;float у=ȯ.Length();float ѕ=(a.ȝ+b.ȝ)*0.55f;
bool Γχ=a.Θ!=""&&a.Θ==b.Θ;if(Γχ&&(a.Ǹ==2||b.Ǹ==2)) continue;if(Γχ&&a.Ǹ==1&&b.Ǹ==1){String[]k=a.Θ.Split('|');if(k.Length>=8) ѕ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Αλ=Ć-ą;float Γφ=Αλ.LengthSquared();float t=Γφ>0.01f?-Vector3.Dot(ȯ,Αλ)/Γφ:0;
bool ΑϏ=у<ѕ;if(!ΑϏ&&(t<=0||t>Εζ||(ȯ+Αλ*t).Length()>=ѕ)) continue;if(!ΑϏ&&a.Ǥ==b.Ǥ&&ą.LengthSquared()>0.25f&&Ć.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ą),Vector3.Normalize(Ć))<-0.7f)
continue;Ŕ p=null;for(int k=0;k<Г.Count;k++)
if((Г[k].a==a.Λ&&Г[k].b==b.Λ)||(Г[k].a==b.Λ&&Г[k].b==a.Λ)) p=Г[k];if(p==null){p=new Ŕ();p.a=a.Λ;p.b=b.Λ;p.ΑЊ=Εθ(a,b,ą,Ć,ȯ,у);Г.Add(p);}p.Ȥ=DateTime.Now;ϋ y=p.ΑЊ==a.Λ?a:b;float Γΰ=ΑϏ||t<4?0:Math.Max(1,y.ю.Length()*0.5f);if(y.л<0||Γΰ<y.л) y.л=Γΰ;
}Δω();}long Εθ(ϋ a,ϋ b,Vector3 ą,Vector3 Ć,Vector3 ȯ,float у){if(a.Λ==Б) return b.Λ;if(b.Λ==Б) return a.Λ;if(a.Ǥ!=b.Ǥ) return a.Ǥ<b.Ǥ?a.Λ:b.Λ;Vector3 n=у>0.01f?ȯ/у:new Vector3(1,0,0);float Γύ=Vector3.Dot(ą,n),ΓЀ=Vector3.Dot(Ć,-n);if(Γύ-ΓЀ>1) return a.Λ;if(ΓЀ-Γύ>1) return b.Λ;
if(a.Ȁ>=0&&b.Ȁ>=0&&Math.Abs(a.Ȁ-b.Ȁ)>2) return a.Ȁ>b.Ȁ?a.Λ:b.Λ;return a.Λ>b.Λ?a.Λ:b.Λ;}String ΔΑ(){String η="C;";bool Л=true;for(int i=0;i<Χ.Count;i++){if(Χ[i].л<0) continue;η+=(Л?"":",")+Χ[i].Λ+":"+Math.Round(Χ[i].л,1);Л=false;}return η;}String Δς()
{int Γι=0,ΑΝ=0;for(int i=0;i<Χ.Count;i++){if(!Đ(Χ[i])) Γι++;if(Χ[i].л>=0) ΑΝ++;}if(Χ.Count==0) return "";return "Traffic: "+Γι+" moving"+(ΑΝ>0?", "+ΑΝ+" giving way":"")+"\n";}List<ί>Щ=new List<ί>();ί х=null;Vector3 ΓΧ;DateTime ΓΦ;float ΓΥ=40;bool ΓΣ=false;
ϋ ƛ(ί s){ϋ Ѝ=null;for(int i=0;i<Χ.Count;i++) if(Χ[i].ȃ!=0&&""+Χ[i].ȃ==s.Λ) return Χ[i];for(int i=0;i<Χ.Count;i++)
if(Vector3.Distance(Χ[i].Φ,s.Π)<15&&(Ѝ==null||
Vector3.Distance(Χ[i].Φ,s.Π)<Vector3.Distance(Ѝ.Φ,s.Π))) Ѝ=Χ[i];return Ѝ;}float Δϊ(ί s){ϋ t=ƛ(s);if(t==null) return 0;float k=t.Ǹ*1000000f;String[]И=t.Θ.Split('|');if(И.Length>=3) k+=Vector3.Distance(t.Φ,new Vector3(float.Parse(И[0]),float.Parse(И[1]),float.Parse(И[2])));return k;
}void Εΰ(){Щ.Clear();х=null;ΑΟ.Clear();List<float>ΑΣ=new List<float>();for(int i=0;i<β.Count;i++){float k=Δϊ(β[i]);int ǰ=0;while(ǰ<ΑΣ.Count&&ΑΣ[ǰ]<=k) ǰ++;ΑΣ.Insert(ǰ,k);Щ.Insert(ǰ,β[i]);β[i].Ǘ("HOMEPOS",true,false,false);ƴ("HOMEPOS",β[i].Λ,"HOMEPOS");}}List<ϋ>ΑΟ=new List<ϋ>();
void Δω(){ΑΟ.Clear();for(int i=0;i<Щ.Count;i++){ϋ t=ƛ(Щ[i]);if(t==null) continue;t.л=0;ΑΟ.Add(t);}}void Ερ(){if(х!=null){bool ΖΒ=ΓΣ||Vector3.Distance(х.Π,ΓΧ)>ΓΥ||
(DateTime.Now-ΓΦ).TotalSeconds>45||х.Ƨ();if(!ΖΒ) return;х=null;}if(Щ.Count==0) return;х=Щ[0];Щ.RemoveAt(0);ΓΧ=х.Π;ΓΦ=DateTime.Now;ϋ t=ƛ(х);ΓΥ=t==null?40:Math.Max(40,t.ȝ*3);ΓΣ=t!=null&&t.Αε==3&&t.ю.LengthSquared()<0.25f;}const String ΔΟ="[PAMDBG]";class Ɨ{public readonly bool ƭ;
IMyProgrammableBlock ѓ;Action<IMyProgrammableBlock>ǁ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>ǀ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>ƿ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>ǂ;Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>ƾ;
public Ɨ(MyGridProgram Ζι){ѓ=Ζι.Me;var ǿ=ѓ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ѓ);if(ǿ==null) return;ǁ=(Action<IMyProgrammableBlock>) ǿ["RemoveDraw"];ǀ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ǿ["Point"];ƿ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ǿ["Line"];
ǂ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ǿ["Sphere"];ƾ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) ǿ["GPS"];((Action<IMyProgrammableBlock>) ǿ["RemoveAll"])(ѓ);ƭ=true;}public void ΕΙ(){if(ǁ!=null) ǁ(ѓ);}public void ǖ(Vector3D a,Vector3D b,Color c,float Ζύ){if(ƿ!=null) ƿ(ѓ,a,b,c,Ζύ,-1,false);}
public void ΕΕ(Vector3D a,Color c,float ы){if(ǀ!=null) ǀ(ѓ,a,c,ы,-1,false);}public void ȓ(String Β,Vector3D ǰ,Color c){if(ƾ!=null) ƾ(ѓ,Β,ǰ,c,-1);}public void Βσ(Vector3D Ț,double ы,Color c){if(ǂ!=null) ǂ(ѓ,new BoundingSphereD(Ț,ы),c,1,0.05f,30,-1,false);}}class ĉ
{public long Λ;public Vector3 Į,ΑΧ,Ğ,ΰ;public bool ΓΠ;public List<Vector3>Ƃ=new List<Vector3>(),Αη=new List<Vector3>();public List<bool>Αζ=new List<bool>();public DateTime Ȉ;}Ɨ υ=null;IMyBroadcastListener ť=null;List<ĉ>т=new List<ĉ>();void Δτ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<6) return;
long Λ=long.Parse(f[0]);ĉ s=null;for(int i=0;i<т.Count;i++) if(т[i].Λ==Λ) s=т[i];if(s==null){s=new ĉ();s.Λ=Λ;т.Add(s);}s.Į=Ϋ(f[1]);s.ΑΧ=Ϋ(f[2]);s.Ğ=Ϋ(f[3]);s.ΓΠ=s.ΑΧ.LengthSquared()>0.5f;s.ΰ=Ϋ(f[4]);s.Ȉ=DateTime.Now;if(f[5]=="") return;s.Ƃ.Clear();s.Αη.Clear();s.Αζ.Clear();
String[]Γς=f[5].Split(':');for(int i=0;i<Γς.Length;i++){String[]r=Γς[i].Split(',');if(r.Length<3) continue;s.Ƃ.Add(Ϋ(r[0]));s.Αη.Add(Ϋ(r[1]));s.Αζ.Add(r[2]=="1");}}void Εν(){if(!debugDraw) return;if(υ==null) υ=new Ɨ(this);if(!υ.ƭ) return;if(ť==null) ť=IGC.RegisterBroadcastListener(ΔΟ);
int ę=20;while(ť.HasPendingMessage&&ę-->0){String Ξ=ť.AcceptMessage().Data as String;if(Ξ!=null) try{Δτ(Ξ);}catch{}}for(int i=т.Count-1;i>=0;i--) if((DateTime.Now-т[i].Ȉ).TotalSeconds>5) т.RemoveAt(i);υ.ΕΙ();for(int i=0;i<Χ.Count;i++){ϋ s=Χ[i];Color c=Đ(s)?Color.Gray:s.л>=0?Color.Yellow:Color.Lime;
υ.Βσ(s.Φ,s.ȝ/2,c);υ.ȓ(œ(s)+": "+(Đ(s)?"parked":s.л==0?"holding (giving way)":s.л>0?"slowing (giving way)":"moving"),s.Φ,c);if(s.ю.LengthSquared()>0.25f) υ.ǖ(s.Φ,s.Φ+s.ю*3,Color.White,0.1f);if(s.ħ.LengthSquared()>0.25f) υ.ǖ(s.Φ,s.Φ+s.ħ*s.ȝ,Color.Cyan,0.1f);
}for(int i=0;i<Г.Count;i++){if((DateTime.Now-Г[i].Ȥ).TotalSeconds>0.5) continue;ϋ a=null,b=null;for(int k=0;k<Χ.Count;k++){if(Χ[k].Λ==Г[i].a) a=Χ[k];if(Χ[k].Λ==Г[i].b) b=Χ[k];}if(a==null||b==null) continue;υ.ǖ(a.Φ,b.Φ,Color.Red,0.2f);υ.ȓ("Conflict: "+œ(Г[i].ΑЊ==a.Λ?a:b)+" gives way",(a.Φ+b.Φ)/2,Color.Red);
}for(int i=0;i<т.Count;i++){ĉ d=т[i];ϋ t=null;for(int k=0;k<Χ.Count;k++) if(Χ[k].Λ==d.Λ) t=Χ[k];if(t!=null) υ.ǖ(t.Φ,d.ΰ,Color.Blue,0.08f);for(int r=0;r<d.Ƃ.Count;r++) υ.ǖ(d.Ƃ[r],d.Αη[r],d.Αζ[r]?Color.Red:Color.Green,0.04f);if(t==null||!d.ΓΠ) continue;г Θ=null;
for(int j=0;j<З.Count;j++) if(З[j].И==t.Θ) Θ=З[j];if(Θ==null) continue;int ΓΕ=0;for(int c=0;c<Θ.κ.Length;c++) if(Θ.κ[c]=='1') ΓΕ++;if(i==0||т[i-1].Į!=d.Į) υ.ȓ("Job: "+ΓΕ+"/"+Θ.κ.Length+" holes done",d.Į,Color.White);for(int c=0;c<Θ.κ.Length;c++){int ǋ=c%Θ.w,Ǧ=c/Θ.w;
Vector3 p=d.Į+d.ΑΧ*((Θ.w-1f)/2f-ǋ)*Θ.ΑЁ-d.Ğ*((Θ.h-1f)/2f-Ǧ)*Θ.Αό;р Αγ=ơ(Θ,c,null);υ.ΕΕ(p,Θ.κ[c]=='1'?Color.Gray:Αγ!=null?Color.Orange:Color.White,Math.Min(Θ.ΑЁ,Θ.Αό)*0.2f);if(Αγ==null||Αγ.Λ!=d.Λ) continue;υ.ǖ(t.Φ,p,Color.Orange,0.05f);υ.ȓ("Hole "+(Θ.ſ.IndexOf(c)+1)+": "+œ(t),p,Color.Orange);
}}for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];υ.Βσ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);υ.ȓ("Docking area"+(љ!=""?": "+љ:""),z.GetPosition(),Color.Cyan);
}for(int i=0;i<ћ.Count;i++)
υ.ǖ(ћ[i].GetPosition(),ћ[i].GetPosition()+ћ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(Б!=0&&ŝ!=0){IMyShipConnector c=ƺ(ŝ);for(int k=0;k<Χ.Count;k++)
if(c!=null&&Χ[k].Λ==Б){υ.ǖ(Χ[k].Φ,c.GetPosition(),Color.Lime,0.15f);υ.ȓ("Cleared to "+(Ş==2?"undock: ":"dock: ")+œ(Χ[k]),c.GetPosition(),Color.Lime);}}}String œ(ϋ t){ί Ѝ=null;for(int i=0;i<β.Count;i++) if(t.ȃ!=0&&β[i].Λ==""+t.ȃ) return β[i].ζ;for(int i=0;i<β.Count;i++)
if(Vector3.Distance(β[i].Π,t.Φ)<30&&(Ѝ==null||
Vector3.Distance(β[i].Π,t.Φ)<Vector3.Distance(Ѝ.Π,t.Φ))) Ѝ=β[i];if(Ѝ!=null&&Ѝ.ζ!="") return Ѝ.ζ;String Λ=t.Λ.ToString();return "Ship "+Λ.Substring(Math.Max(0,Λ.Length-4));}String Δδ(){if(!debugDraw) return "";if(υ==null||!υ.ƭ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}void Εέ(string А){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}