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
public enum Ο{Ǚ,ƫ,ȑ,Е,ο}public enum Ν{ǫ,Ɯ,Βα,Ʊ,Βμ,ĉ,Ǖ,
ƛ,Δή,ȍ,ΑБ,ΓЈ,ΑЊ,
ΕΛ,đ,Đ,Ɠ,Ετ,ΔΩ,
Ες,Εσ,ΔΪ,ΔΫ,Ē}public enum Τ{ё,Ʈ,Ǫ,ΒΑ,ń,Ββ,Βγ,ŀ,Ł}public enum Ǒ{None,Ɠ,ǫ,ΔΟ}public enum Κ{Main,Βκ,ċ,Ċ,ƺ,Ń,
Ə,ΒΥ,ΒΦ,ΒΤ,Ď,Βξ,
ō,ƴ,Ʒ,Βρ,ő}public enum ǘ{Βν,ΔΚ,Εέ,Δώ,ο}public enum У{Ŕ,ΔΕ}public enum ǔ{Βλ,Δέ,Εΰ,ơ}public enum в{Βε,ΔΝ,ΔΜ,ΔΥ,ΔΤ,Δό,Δϋ}public enum я{Ɨ,ΓЌ,ΑЋ}public enum ň{Δϊ,ΗΓ,ΓЉ}public enum Ѕ{Βζ,Βη,Βθ,ΔБ,ΔА,
ΔЋ,ΔЌ,ΔЊ,ΔЎ,
ΔЏ,ΔЍ}public enum Υ{о,ђ,н,ł,Ɛ,Ƒ}public enum п{Βτ,ΒΒ,ΔЉ,Βδ}public enum ι{None,ȓ,Č,ŋ}public enum τ{Ƽ,Ǝ,ƨ,ơ,Βί,ƪ,Βΰ,Ει,ƭ}public enum ϊ{Ǚ,Βέ,Βά,Ʃ,Ƣ,ƕ,Βή}class ά{public bool ν=false;public Vector3 Π=new Vector3();public Vector3 Ё=new Vector3();
public Vector3 І=new Vector3();public Vector3 О=new Vector3();public Vector3 π=new Vector3();public Vector3 Ч=new Vector3();public float ΖΨ=0;public float ΗΚ=0;public float[]Я=null;public ά(){}public ά(ά Й){ν=Й.ν;Π=Й.Π;Ё=Й.Ё;І=Й.І;О=Й.О;π=Й.π;Ч=Й.Ч;Я=Й.Я;}public ά(Vector3 Π,Vector3 І,Vector3 Ё,Vector3 О,Vector3 π)
{this.Π=Π;this.Ё=Ё;this.І=І;this.О=О;this.ΖΨ=0;this.π=π;}public void ΗΒ(List<IMyThrust>Ř,List<string>ǎ){Я=new float[ǎ.Count];for(int i=0;i<Я.Length;i++)
Я[i]=-1;for(int i=0;i<Ř.Count;i++){string ȉ=Δπ(Ř[i]);int Α=ǎ.IndexOf(ȉ);if(Α!=-1)
Я[Α]=С(Ř[i].MaxEffectiveThrust,Ř[i].MaxThrust);}}}class ŕ{public Ѕ П=Ѕ.Βζ;public float е=0;public float Ǵ=0;public string ф="";public string ч="";DateTime Γή;public bool Ζε=false;private bool ǋ=false;public bool ΔΖ(bool Αθ){if(Αθ){Ǵ=0;ǋ=false;return false;}ǋ=true;return Ǵ>е;
}public void ΗΔ(){ΔΖ(true);Ζε=false;}public void ΗΕ(){if(ǋ)
if((DateTime.Now-Γή).TotalSeconds>1){Ǵ++;Γή=DateTime.Now;}}public bool ΒΡ(){switch(П){case Ѕ.Βη:return true;case Ѕ.Βθ:return true;}return false;}}class Ǭ{public String ζ="";public String Ќ="";public int Ј=0;public ň ΑΨ=ň.ΓЉ;public Ǭ(String ζ,String Ќ,int Ј,ň ΑΨ){this.ζ=ζ;this.Ќ=Ќ;
this.Ј=Ј;this.ΑΨ=ΑΨ;}}class ί{public DateTime ΑΤ;public DateTime ΒЇ;public String Λ="";public String ζ="";public String Ɖ="";public String Ϊ="";public String Αχ="";public String ź="";public String Αγ="";public Vector3 Π=new Vector3();public List<Ǭ>њ=new List<Ǭ>();public Ο Ρ=Ο.Ǚ;
public ι Ǜ;public float ǽ;public float Ǳ;public int ς;public int ΑΑ=0;public int ΑϏ=0;public bool Ʀ(){return(DateTime.Now-ΑΤ).TotalSeconds>10;}public bool ƣ(){if(Ǜ!=ι.ȓ) return false;return(DateTime.Now-ΒЇ).TotalSeconds>=4;}public ί(String Λ){this.Λ=Λ;}public ι Ǘ(String μ,bool Ζσ,bool ѕ,bool ΕϏ)
{if(μ==""&&!ѕ) return ι.None;if(Ǜ==ι.ȓ&&ƣ())
Ǜ=ι.ŋ;if(Ζσ){Αγ=μ;Ǜ=ι.ȓ;ΒЇ=DateTime.Now;return Ǜ;}else if(ѕ){Αγ="";Ǜ=ι.None;}else if(Αγ==μ){if(ΕϏ) Ǜ=ι.Č;return Ǜ;}return ι.None;}}public String Δη(String ΖΟ){switch(ΖΟ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Н;Vector3 Γς=new Vector3();Vector3 м=new Vector3();Vector3 ΕА=new Vector3();Vector3 ΖΣ=new Vector3();Vector3 ΕЉ=new Vector3();DateTime Γέ=new DateTime();
bool ģ=true;int Αό=0;bool Ĭ=false;bool ǟ=false;bool Ζά=true;bool ΖΛ=false;float Ζυ=0;float Γϊ=0;int Ζν=0;int Ƃ=0;float Ȧ=0;float Ș=0;double Αέ=0;float Ĕ=0;List<int>ȡ=new List<int>();List<int>ȱ=new List<int>();Ο Ρ=Ο.Ǚ;int Αξ=0;float ǐ=0;float Ǐ=0;int Ŵ=0;float Ǻ=0,ǹ=0;
float ā=0;float Γω=0,Γψ=0;IMyRemoteControl Њ;IMySensorBlock ώ;List<IMyTimerBlock>ΓЁ=new List<IMyTimerBlock>();List<IMyShipConnector>ě=new List<IMyShipConnector>();List<IMyThrust>Ĺ=new List<IMyThrust>();List<IMyGyro>ΓΜ=new List<IMyGyro>();List<IMyTerminalBlock>а=new List<IMyTerminalBlock>();
List<IMyLandingGear>ΑΣ=new List<IMyLandingGear>();List<IMyReactor>Αη=new List<IMyReactor>();List<IMyConveyorSorter>Ζψ=new List<IMyConveyorSorter>();List<IMyGasTank>Ī=new List<IMyGasTank>();List<IMyTerminalBlock>ē=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Βώ=new List<IMyBatteryBlock>();
List<IMyTextPanel>Љ=new List<IMyTextPanel>();List<IMyTextSurface>ь=new List<IMyTextSurface>();List<IMyTextPanel>ǳ=new List<IMyTextPanel>();IMyTerminalBlock ǥ=null;ά Σ=new ά();ά ΓΣ=new ά();ά ΓΪ=new ά();bool ǩ=false;bool ļ=false;bool Ľ=false;bool ΓΨ=false;List<String>ǎ=new List<string>();
List<ά>ω=new List<ά>();List<ά>Ɗ=new List<ά>();double ΖЊ=0;ά Ψ=new ά();Vector3 İ;Vector3 ȧ;Τ φ=Τ.ё;У Ȣ=У.Ŕ;int к=0;int й=0;double ų=0;bool ΒЍ=false;ŕ χ=new ŕ();ŕ ψ=new ŕ();ǔ ǣ=ǔ.Βλ;bool ľ=true;У Ȇ=У.Ŕ;int Ц=3;int Х=3;int д=30;я ј=я.Ɨ;bool ȋ=true;bool ī=true;
bool ġ=true;bool Ȉ=false;в Ġ=в.Βε;bool Ǩ=true;float ш=90;float щ=20;float Ы=5;float ъ=20;float ǚ=0.70f;float ƌ=1.50f;float Ƌ=2.50f;float Ŀ=0f;float ħ=0f;Ν ţ;Τ ǡ;Ν ķ;int ŭ=0;int Ů=0;int с=0;int Į=0;int Αλ=30;bool Ζω=true;double ΓΑ=0;bool Γτ=false;int ΖЋ=0;int Ȋ=0;
ά ƃ=null;float ΒЌ=0;float Αά=0;ά ΕЇ=null;float ΒϏ=0;Ǒ ĕ;float ΓΧ=0;float ΓЄ=0;String ΖΦ="";float ΑΧ=0;float Ǳ=0;float ǽ=0;float Ђ=0;bool Αρ=false;bool ΖϏ=false;bool Ζφ=false;bool ΓЀ=false;bool Ζχ=false;float ΖΧ=0;float Ζδ=0;Vector3 ΕЎ=new Vector3();Vector3 Γι=new Vector3();
bool ΖΔ=false;float[,]Ļ=new float[3,2];Dictionary<String,float[,]>Ǎ=new Dictionary<string,float[,]>();Dictionary<String,float[]>ƀ=new Dictionary<string,float[]>();bool ΒЏ=false;int ř=0;int ǝ=0;int Ű=0;int Ƿ=0;String Ϊ="";int[]Γζ=new int[Enum.GetValues(typeof(Κ)).Length];
bool Αί=false;int Ǿ=0;int ς=0;Κ δ=Κ.Main;int ΖЀ=0,ΖЁ=0,ΖЂ=0,ΖЃ=0;int Ζξ=0,Ζο=0;bool ė=false;bool Βϊ=false;String ĵ="";String Ǉ="";IMyBroadcastListener є=null;bool ΓΗ=true;List<ί>β=null;ί σ=null;int ş=0,Š=0;int ΒЉ=0,ΒЊ=0;int ΕЄ=0,ΕЅ=0,ΕІ=0;String ΖΠ="";
Exception Ȥ=null;bool Γσ=false;Τ Αμ=Τ.ё;bool ΒЎ=false;String ΖЈ="";bool ΓЃ=false;bool Αυ=true;int ΑΟ=0;Vector3 ǈ=new Vector3();bool ΓΚ=false;bool ƥ(IMyTerminalBlock α)=>α.CubeGrid==Me.CubeGrid;Program(){Н=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;
if(ΒΣ(Me,controllerTag,true))
Ρ=Ο.Е;Ω(Κ.Main);if(Ρ!=Ο.Е){Ϊ="Welcome to [PAM]!";п Αι=ΕΙ();if(Ρ==Ο.Ǚ){List<IMyShipDrill>ΓΕ=new List<IMyShipDrill>();List<IMyShipGrinder>ΓΛ=new List<IMyShipGrinder>();Н.GetBlocksOfType(ΓΕ,q=>q.CubeGrid==Me.CubeGrid);Н.GetBlocksOfType(ΓΛ,q=>q.CubeGrid==Me.CubeGrid);if(ΓΕ.Count>0)
{Ρ=Ο.ƫ;Ϊ="Miner mode enabled!";}else if(ΓΛ.Count>0){Ρ=Ο.ȑ;Ϊ="Grinder mode enabled!";}else{Ρ=Ο.ο;Βπ(ǘ.ο);}}if(Αι==п.Βτ) Ζά=false;if(Αι==п.ΒΒ) Ϊ="Data restore failed!";if(Αι==п.Βδ) Ϊ="New version, wipe data";}}void Main(string А,UpdateType ΖЅ){try{if(Ȥ!=null){if(А.Trim().ToUpper()=="RETRY")
{Ȥ=null;ģ=true;Ζν=0;if(Ρ!=Ο.Е) Ǯ();Ϊ="Restarted after error";}else{Δυ();ΕΕ();return;}}ǟ=(ΖЅ&UpdateType.Update10)!=0;if(ǟ) Αό++;Ĭ=Αό>=10;if(Ĭ) Αό=0;if(ǟ){ř++;if(ř>4) ř=0;Ƃ=(int) Math.Max(0,10-(DateTime.Now-Γέ).TotalSeconds);}if(А!="") ΖΠ=А;if(Ρ!=Ο.Е)
Εά(А);else
ΔΛ(А);Αί=false;try{int ΒЋ=Runtime.CurrentInstructionCount;float ȃ=С(ΒЋ,Runtime.MaxInstructionCount);if(ȃ>0.90) Ϊ="Max. instructions >90%";if(ȃ>Ȧ) Ȧ=ȃ;if(ΒЏ){ȡ.Add(ΒЋ);while(ȡ.Count>10) ȡ.RemoveAt(0);Ș=0;for(int i=0;i<ȡ.Count;i++) Ș+=ȡ[i];Ș=С(С(Ș,ȡ.Count),Runtime.MaxInstructionCount);
double ΑΥ=Runtime.LastRunTimeMs;if(ΖΛ&&ΑΥ>Αέ) Αέ=ΑΥ;ȱ.Add((int)(ΑΥ*1000f));while(ȱ.Count>10) ȱ.RemoveAt(0);Ĕ=0;for(int i=0;i<ȱ.Count;i++) Ĕ+=ȱ[i];Ĕ=С(Ĕ,ȱ.Count)/1000f;}}catch{Ȧ=0;}}catch(Exception e){Ȥ=e;}}String Δθ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Ω(Κ Ў){Γζ[(int) δ]=ς;ς=Γζ[(int) Ў];if(Ў==Κ.Ď) ς=0;δ=Ў;if(Ρ!=Ο.Е)
ŗ(δ==Κ.ċ,false,0,0);Αί=true;}void ΔЇ(bool ΑΩ){if(ς>0) ς--;else if(ΑΩ) ς=Ǿ-1;}void ΔІ(bool ΑΩ){if(ς<Ǿ-1) ς++;else if(ΑΩ) ς=0;}String Βπ(ǘ η){switch(η){case ǘ.Βν:Ϊ="Job is running";break;case ǘ.ΔΚ:Ϊ="Connector not ready!";break;case ǘ.Εέ:Ϊ="Ship modified, path outdated!";break;
case ǘ.Δώ:Ϊ="Interrupted by player!";break;case ǘ.ο:Ϊ="Shuttle mode enabled!";break;}return "";}String Δο(У Φ){switch(Φ){case У.Ŕ:return "Top-Left";case У.ΔΕ:return "Center";default:return "";}}String Δδ(я λ){switch(λ){case я.ΓЌ:return "Auto"+(Ρ==Ο.ƫ?" (Ore)":"");case я.ΑЋ:return "Auto (+Stone)";
case я.Ɨ:return "Default";default:return "";}}String Δμ(Τ Ѐ){switch(Ѐ){case Τ.ё:return "No job";case Τ.Ʈ:return "Job paused";case Τ.Ǫ:return "Job active";case Τ.ŀ:return "Job active";case Τ.Ł:return "Job active";case Τ.ΒΑ:return "Job done";case Τ.ń:return "Job changed";case Τ.Ββ:return "Move home";
case Τ.Βγ:return "Move to job";}return "";}String ΒΗ(ǔ Εϋ){switch(Εϋ){case ǔ.Βλ:return "Return home";case ǔ.Δέ:return "Fly to job pos";case ǔ.Εΰ:return "Stop";case ǔ.ơ:return "Ignore";}return "";}String Δζ(в λ){switch(λ){case в.Βε:return "Off";case в.ΔΥ:return "Drop pos (Stone) ";case в.ΔΤ:return "Drop pos (Sto.+Ice)";
case в.ΔΝ:return "Cur. pos (Stone)";case в.ΔΜ:return "Cur. pos (Sto.+Ice)";case в.Δό:return "In motion (Stone)";case в.Δϋ:return "In motion (Sto.+Ice)";}return "";}String ΒΕ(Ǒ Ѐ){switch(Ѐ){case Ǒ.None:return "No batteries";case Ǒ.Ɠ:return "Charging";case Ǒ.ΔΟ:return "Discharging";}return "";}
String ΒΛ(Ν Ѐ){String ΗΗ=Ρ==Ο.ο?"target":"job";switch(Ѐ){case Ν.ǫ:return "Idle";case Ν.Ɯ:return "Flying to XY position";case Ν.Βα:return Ρ==Ο.ȑ?"Grinding":"Mining";case Ν.Ʊ:return "Returning";case Ν.ΔΪ:return "Flying to drop pos";case Ν.Βμ:return "Returning to dock";case Ν.ĉ:return "Flying to dock area";
case Ν.Ǖ:return "Flying to job area";case Ν.ƛ:return "Flying to path";case Ν.Δή:return "Flying to job position";case Ν.ȍ:return "Approaching dock";case Ν.ΑБ:return "Docking";case Ν.ΓЈ:return "Aligning";case Ν.ΑЊ:return "Aligning";case Ν.ΕΛ:return "Retry docking";case Ν.đ:return "Unloading";case Ν.Ē:return ΖЈ;
case Ν.Đ:return "Undocking";case Ν.Ɠ:return "Charging batteries";case Ν.Ετ:return "Waiting for uranium";case Ν.ΔΩ:return "Filling up hydrogen";case Ν.Ες:return "Waiting for ejection";case Ν.Εσ:return "Waiting for ejection";case Ν.ΔΫ:return "Flying to drop pos";}return "";}String ΒΠ(Ѕ П){switch(П)
{case Ѕ.Βζ:return "On \"Undock\" command";case Ѕ.Βη:return "On player entered cockpit";case Ѕ.ΔБ:return "On ship is full";case Ѕ.ΔА:return "On ship is empty";case Ѕ.Βθ:return "On time delay";case Ѕ.ΔЌ:return "On batteries empty(<25%)";case Ѕ.ΔЊ:return "On batteries empty(=0%)";case Ѕ.ΔЋ:return "On batteries full";
case Ѕ.ΔЏ:return "On hydrogen empty(<25%)";case Ѕ.ΔЍ:return "On hydrogen empty(=0%)";case Ѕ.ΔЎ:return "On hydrogen full";}return "";}bool Η(ref String Β,int Γχ,int ǜ,bool Ε,String ǻ){Ǿ+=1;if(Γχ==ǜ) ǻ=">"+ǻ+(ř>=2?" .":"");else ǻ=" "+ǻ;Β+=ǻ+"\n";return Γχ==ǜ&&Ε;}String ΓЎ(bool Ε)
{int Α=0;int Δ=ς;Ǿ=0;String Ā="———————————————\n";String ă="--------------------------------------------\n";String Β="";Β+=Δμ(φ)+" | "+(Σ.ν?"Ready to dock":"No dock")+"\n";Β+=Ā;double Γώ=Math.Max(Math.Round(this.ΓΑ),0);if(δ==Κ.Main){bool Ǟ=Ρ==Ο.ο;if(Η(ref Β,Δ,Α++,Ε," Record path & set home")) Εή();
if(Ρ==Ο.ƫ)
if(Η(ref Β,Δ,Α++,Ε," Setup mining job")) Ω(Κ.ċ);if(Ρ==Ο.ȑ)
if(Η(ref Β,Δ,Α++,Ε," Setup grinding job")) Ω(Κ.ċ);if(Ρ==Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," Setup shuttle job")) Ω(Κ.Ʒ);if(Η(ref Β,Δ,Α++,Ε," Continue job")) ΑЏ();if(Η(ref Β,Δ,Α++,Ε," Fly to home position")) Δά();if(Η(ref Β,Δ,Α++,Ε," Fly to job position")) Δί();if(Η(ref Β,Δ,Α++,Ε," Behavior settings"))
if(Ǟ) Ω(Κ.ő);else Ω(Κ.Ń);if(Η(ref Β,Δ,Α++,Ε," Info")) Ω(Κ.ΒΥ);if(Ρ!=Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," Help")) Ω(Κ.ΒΤ);}else if(δ==Κ.ċ){double ΖЌ=Math.Round(Ц*ǐ,1);double ΖΗ=Math.Round(Х*Ǐ,1);String ξ="";if(Η(ref ξ,Δ,Α++,Ε," Start new job!")) Βσ();if(Η(ref ξ,Δ,Α++,Ε," Change current job")){ƻ(false,false);Ω(Κ.Main);}if(Η(ref ξ,Δ,Α++,Ε," Width + (Width: "+Ц+" = "+ΖЌ+"m)")){Ȍ(ref Ц,5,20,1);Ȗ(true);}
if(Η(ref ξ,Δ,Α++,Ε," Width -")){Ȍ(ref Ц,-5,20,-1);Ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Height + (Height: "+Х+" = "+ΖΗ+"m)")){Ȍ(ref Х,5,20,1);Ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Height -")){Ȍ(ref Х,-5,20,-1);Ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Depth + ("+(ј==я.Ɨ?"Depth":"Min")+": "+д+"m)")){Ȍ(ref д,5,50,2);Ȗ(true);}
if(Η(ref ξ,Δ,Α++,Ε," Depth -")){Ȍ(ref д,-5,50,-2);Ȗ(true);}if(Η(ref ξ,Δ,Α++,Ε," Depth mode: "+Δδ(ј))){ј=Ǔ(ј);}if(Η(ref ξ,Δ,Α++,Ε," Start pos: "+Δο(Ȇ))){Ȇ=Ǔ(Ȇ);}if(Ρ==Ο.ȑ&&ј==я.ΑЋ) ј=Ǔ(ј);Β+=Ʋ(8,ξ,Δ,ref Ζξ);}else if(δ==Κ.Ʒ){float[]ΒА=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Η(ref Β,Δ,Α++,Ε," Next")) Ω(Κ.Βρ);if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=" Leave connector 1:\n";if(Η(ref Β,Δ,Α++,Ε," - "+ΒΠ(χ.П))) χ.П=Ǔ(χ.П);if(!χ.ΒΡ()) Β+="\n";else if(Η(ref Β,Δ,Α++,Ε," - Delay: "+Ɲ((int) χ.е))) χ.е=ΑА(χ.е,ΒА);Β+=" Leave connector 2:\n";if(Η(ref Β,Δ,Α++,Ε," - "+ΒΠ(ψ.П))) ψ.П=Ǔ(ψ.П);
if(!ψ.ΒΡ()) Β+="\n";else if(Η(ref Β,Δ,Α++,Ε," - Delay: "+Ɲ((int) ψ.е))) ψ.е=ΑА(ψ.е,ΒА);}else if(δ==Κ.Βρ){if(Η(ref Β,Δ,Α++,Ε," Start job!")) Βσ();if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Ʒ);Β+=" Timer: \"Docking connector 1\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(χ.ф!=""?χ.ф:"-"))) χ.ф=Ŋ(ref ΖЀ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(χ.ч!=""?χ.ч:"-"))) χ.ч=Ŋ(ref ΖЂ);Β+=" Timer: \"Docking connector 2\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(ψ.ф!=""?ψ.ф:"-"))) ψ.ф=Ŋ(ref ΖЁ);Β+=" Timer: \"Leaving connector 2\":\n";if(Η(ref Β,Δ,Α++,Ε," = "+(ψ.ч!=""?ψ.ч:"-"))) ψ.ч=Ŋ(ref ΖЃ);
}else if(δ==Κ.Ċ){String Γα=ш+" %";if(Ĭ) ǝ++;if(ǝ>1){ǝ=0;Ű++;if(Ű>1) Ű=0;}bool[]Ας=new bool[]{Αη.Count==0,ĕ==Ǒ.None,Ī.Count==0};int ΓЂ=0;while(true){ΓЂ++;Ƿ++;if(Ƿ>Ας.Length-1) Ƿ=0;if(ΓЂ>=Ας.Length) break;if(!Ας[Ƿ]) break;}bool Ǟ=Ρ==Ο.ο;if(!Ǟ&&ȋ&&Ђ!=-1&&Ű==0)
Γα=Ђ<1000000?Math.Round(Ђ)+" Kg":Math.Round(Ђ/1000)+" t";if(Η(ref Β,Δ,Α++,Ε," Stop!")){Ǯ();Ω(Κ.Main);}if(Η(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!Ǟ) Ω(Κ.Ń);else Ω(Κ.ő);if(!Ǟ){if(Η(ref Β,Δ,Α++,Ε," Next hole")) ΓЇ(false);}else if(Η(ref Β,Δ,Α++,Ε," Undock")) ΓЃ=true;Β+=ă;if(!Ǟ) Β+="Progress: "+Math.Round(ų,1)+" %\n";Β+="State: "+ΒΛ(ţ)+" "+Γώ+"m \n";Β+="Load: "+ΑΧ+" % Max: "+Γα+" \n";if(Ƿ==0) Β+="Uranium: "+(Αη.Count==0?"No reactors":Math.Round(ΓЄ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ƿ==1) Β+="Battery: "+(ĕ==Ǒ.None?ΒΕ(ĕ):ΒϏ+"% "+(щ==-1||Ǟ?"":" Min: "+щ+" %"))+"\n";if(Ƿ==2) Β+="Hydrogen: "+(Ī.Count==0?"No tanks":Math.Round(ΓΧ,1)+"% "+(ъ==-1||Ǟ?"":" Min: "+ъ+" %"))+"\n";}else if(δ==Κ.Ń){String ξ="";if(Η(ref ξ,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.Ċ);else Ω(Κ.Main);}
if(Η(ref ξ,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.ƨ,false);if(Η(ref ξ,Δ,Α++,Ε," Weight limit: "+(ȋ?"On":"Off"))) ȋ=!ȋ;if(Η(ref ξ,Δ,Α++,Ε," Ejection: "+Δζ(Ġ))) Ġ=Ǔ(Ġ);if(Η(ref ξ,Δ,Α++,Ε," Toggle sorters: "+(Ȉ?"On":"Off"))){Ȉ=!Ȉ;if(Ȉ) ƶ(Αυ);}
if(Η(ref ξ,Δ,Α++,Ε," Unload ice: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Η(ref ξ,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.ƪ,true);if(Η(ref ξ,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Min "+щ+"%"))) Д(ref щ,-5,τ.Βί,true);if(Η(ref ξ,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) Д(ref ъ,-10,τ.Βΰ,true);
if(Η(ref ξ,Δ,Α++,Ε," When done: "+(ľ?"Return home":"Stop"))) ľ=!ľ;if(Η(ref ξ,Δ,Α++,Ε," On damage: "+ΒΗ(ǣ))) ǣ=Ǔ(ǣ);if(Η(ref ξ,Δ,Α++,Ε," Advanced...")) Ω(Κ.Ə);Β+=Ʋ(8,ξ,Δ,ref Ζο);}else if(δ==Κ.Ə){if(Η(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.Ċ);else Ω(Κ.Main);}
if(Η(ref Β,Δ,Α++,Ε,(Ρ==Ο.ȑ?" Grinder":" Drill")+" inv. balancing: "+(ī?"On":"Off"))) ī=!ī;if(Η(ref Β,Δ,Α++,Ε," Enable"+(Ρ==Ο.ȑ?" grinders":" drills")+": "+(ġ?"Fwd + Bwd":"Fwd"))) ġ=!ġ;if(Η(ref Β,Δ,Α++,Ε," Work speed fwd.: "+ƌ+"m/s")) Д(ref ƌ,0.5f,τ.Ƽ,false);
if(Η(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ƌ+"m/s")) Д(ref Ƌ,0.5f,τ.Ƽ,false);if(Η(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǚ*100f)+"%"+(ǚ>0.80f?" (risky)":""))) Д(ref ǚ,0.1f,τ.Ǝ,false);if(Η(ref Β,Δ,Α++,Ε," Width overlap: "+Ŀ*100f+"%")) ΑЎ(true,0.05f);if(Η(ref Β,Δ,Α++,Ε," Height overlap: "+ħ*100f+"%")) ΑЎ(false,0.05f);
}else if(δ==Κ.ő){if(Η(ref Β,Δ,Α++,Ε," Back")){if(φ==Τ.Ǫ) Ω(Κ.Ċ);else Ω(Κ.Main);}if(Η(ref Β,Δ,Α++,Ε," Max load: "+ш+"%")) Д(ref ш,ш<=80?-10:-5,τ.ƨ,false);if(Η(ref Β,Δ,Α++,Ε," Unload ice: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Η(ref Β,Δ,Α++,Ε," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) Д(ref Ы,(Ы>5?-5:-1),τ.ƪ,true);
if(Η(ref Β,Δ,Α++,Ε," Battery: "+(щ==-1?"Ignore":"Charge up"))) щ=(щ==-1?1:-1);if(Η(ref Β,Δ,Α++,Ε," Hydrogen: "+(ъ==-1?"Ignore":"Fill up"))) ъ=(ъ==-1?1:-1);if(Η(ref Β,Δ,Α++,Ε," On damage: "+ΒΗ(ǣ))) ǣ=Ǔ(ǣ);if(Η(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ǚ*100f)+"%"+(ǚ>0.80f?" (risky)":""))) Д(ref ǚ,0.1f,τ.Ǝ,false);
}else if(δ==Κ.Βκ){double ΖΡ=0;if(ω.Count>0) ΖΡ=Vector3.Distance(ω.Last().Π,м);if(Η(ref Β,Δ,Α++,Ε," Stop path recording")) Εγ();if(Ρ!=Ο.ο){if(Η(ref Β,Δ,Α++,Ε," Home: "+(ǩ?"Use old home":(Σ.ν?"Was set! ":"none ")))) ǩ=!ǩ;}else{if(Η(ref Β,Δ,Α++,Ε," Connector 1: "+(ǩ?"Use old connector":(Σ.ν?"Was set! ":"none ")))) ǩ=!ǩ;
if(Η(ref Β,Δ,Α++,Ε," Connector 2: "+(ļ?"Use old connector":(Ψ.ν?"Was set! ":"none ")))) ļ=!ļ;}if(Η(ref Β,Δ,Α++,Ε," Path: "+(Ľ?"Use old path":(ω.Count>1?"Count: "+ω.Count:"none ")))) Ľ=!Ľ;Β+=ă;Β+="Wp spacing: "+Math.Round(ΖЊ)+"m\n";}else if(δ==Κ.ƺ){if(Η(ref Β,Δ,Α++,Ε," Stop")){Ǯ();Ω(Κ.Main);}
Β+=ă;Β+="State: "+ΒΛ(ţ)+" \n";Β+="Speed: "+Math.Round(Ζυ,1)+"m/s\n";Β+="Target dist: "+Γώ+"m\n";Β+="Wp count: "+ω.Count+"\n";Β+="Wp left: "+ΖЋ+"\n";}else if(δ==Κ.ΒΥ){List<IMyTerminalBlock>ǲ=Δβ();if(Ĭ) ǝ++;if(ǝ>=ǲ.Count) ǝ=0;if(Η(ref Β,Δ,Α++,Ε," Next")) Ω(Κ.ΒΦ);
Β+=ă;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΧ,1)+"% "+Math.Round(Ǳ,1)+" / "+Math.Round(ǽ,1)+"\n";Β+="Uranium: "+(Αη.Count==0?"No reactors":Math.Round(ΓЄ,1)+"Kg "+ΖΦ)+"\n";Β+="Battery: "+(ĕ==Ǒ.None?"":ΒϏ+"% ")+ΒΕ(ĕ)+"\n";Β+="Hydrogen: "+(Ī.Count==0?"No tanks":Math.Round(ΓΧ,1)+"% ")+"\n";
Β+="Damage: "+(ǲ.Count==0?"None":""+(ǝ+1)+"/"+ǲ.Count+" "+ǲ[ǝ].CustomName)+"\n";}else if(δ==Κ.ΒΦ){if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=ă;Β+="Next scan: "+Ƃ+"s\n";Β+="Drill area: "+Math.Round(ǐ,1)+"x"+Math.Round(Ǐ,1)+"m Ship: "+Math.Round(Γω,1)+"x"+Math.Round(Γψ,1)+"m ("+Math.Round(ā,1)+"m)\n"+(Εώ.Count>0?"Cameras: "+Εύ+"\n":"");
Β+="Broadcast: "+(ė?"Online - "+ĵ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ȧ*100f,1)+"% \n";}else if(δ==Κ.ΒΤ){if(Η(ref Β,Δ,Α++,Ε," Back")) Ω(Κ.Main);Β+=ă;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Αξ==2) Β="Fatal setup error\nNext scan: "+Ƃ+"s\n";if(Γσ) Β="Recompile script now";int ΖΤ=Β.Split('\n').Length;for(int i=ΖΤ;i<=10;i++) Β+="\n";Β+=Ā;Β+="Last: "+Ϊ+"\n";
return Β;}void ΑЎ(bool ΖΞ,float ƅ){Ǯ();ƻ(true,false);if(ΖΞ) Д(ref Ŀ,ƅ,τ.ƭ,false);else Д(ref ħ,ƅ,τ.ƭ,false);ΔΔ();Ǻ=ǐ;ǹ=Ǐ;ŗ(true,true,0,0);}float ΑА(float ț,float[]ƈ){float έ=ƈ[0];for(int i=ƈ.Length-1;i>=0;i--)
if(ț<ƈ[i]) έ=ƈ[i];return έ;}String Ŋ(ref int ǜ){String έ="";if(ǜ>=ΓЁ.Count) ǜ=-1;if(ǜ>=0) έ=ΓЁ[ǜ].CustomName;ǜ++;return έ;}void Ȍ(ref int value,int Ė,int Ɔ,int Ασ){if(Ė==0) return;if(value<Ɔ&&Ασ>0||value<=Ɔ&&Ασ<0){value+=Ασ;return;}int Εφ=Math.Abs(Ė);int ǂ=0;
int Αΰ=1;while(true){ǂ+=Αΰ*Εφ*10;if(Ė<0&&value-Ɔ<=ǂ) break;if(Ė>0&&value-Ɔ<ǂ) break;Αΰ++;}value+=Αΰ*Ė;}void Ȗ(bool ΗΛ){Ц=Math.Max(Ц,1);Х=Math.Max(Х,1);д=Math.Max(д,0);ŗ(δ==Κ.ċ,false,0,0);}void Д(ref float value,float ƅ,τ Ќ,bool ĺ){Д(ref value,value+ƅ,Ќ,ĺ,true);
}void Д(ref float value,float Ȩ,τ Ќ,bool ĺ,bool ΖЏ){float Ь=0,Ъ=0;if(Ќ==τ.Ƽ){Ь=0.5f;Ъ=10f;}if(Ќ==τ.Ǝ){Ь=0.1f;Ъ=1f;}if(Ќ==τ.ơ){Ь=50f;Ъ=100f;}if(Ќ==τ.Βί){Ь=5f;Ъ=30f;}if(Ќ==τ.ƪ){Ь=1f;Ъ=25f;}if(Ќ==τ.ƨ){Ь=10f;Ъ=95f;}if(Ќ==τ.Βΰ){Ь=10f;Ъ=90f;}
if(Ќ==τ.Ει){Ь=10f;Ъ=1800;}if(Ќ==τ.ƭ){Ь=0.0f;Ъ=0.75f;}if(Ȩ==-1&&ĺ){value=-1;return;}if(value==-1) ĺ=false;bool Γλ=Ȩ<Ь||Ȩ>Ъ;if(Γλ&&ΖЏ){if(Ȩ<value) value=Ъ;else if(Ȩ>value) value=Ь;}else value=Ȩ;if(Γλ&&ĺ) value=-1;else value=Math.Max(Ь,Math.Min(value,Ъ));
value=(float) Math.Round(value,2);}void ΓЇ(bool Ζη){if(Ζη) с=Math.Max(0,с-1);else с++;ΔΧ();ΔΦ();for(int k=0;k<ΑΝ.Count&&k<з.Length;k++) ΕΨ(ΑΝ[k],k<с);Ȁ=-1;Βω(true);}T Ǔ<T>(T ț){int Α=Array.IndexOf(Enum.GetValues(ț.GetType()),ț);Α++;if(Α>=ΔΨ(ț)) Α=0;return(T) Enum.GetValues(ț.GetType()).GetValue(Α);
}int ΔΨ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Εή(){Ɗ.Clear();for(int i=0;i<ω.Count;i++) Ɗ.Add(ω[i]);ω.Clear();ΓΨ=true;ΓΣ=new ά(Σ);ΓΪ=new ά(Ψ);Σ.ν=false;if(Ρ==Ο.ο) Ψ.ν=false;for(int i=0;i<Ǎ.Count;i++)
if(!ǎ.Contains(Ǎ.Keys.ElementAt(i)))
ǎ.Add(Ǎ.Keys.ElementAt(i));ǩ=false;ļ=false;Ľ=false;Ω(Κ.Βκ);}void Εγ(){if(ǩ) Σ=ΓΣ;if(ļ) Ψ=ΓΪ;if(Ľ){ω.Clear();for(int i=0;i<Ɗ.Count;i++) ω.Add(Ɗ[i]);}ΓΨ=false;Ǯ();Ω(Κ.Main);}void ŏ(ά ж,Τ Ѐ){ΕЇ=ж;if(φ==Τ.Ǫ) ǡ=Ѐ;}bool ΒΘ(ά ж,float у,bool ΖІ,out Vector3 Φ){if(ΖІ){Vector3I ΖΓ=new Vector3I((int) ж.Ч.X,(int) ж.Ч.Y,(int) ж.Ч.Z);
IMySlimBlock α=Me.CubeGrid.GetCubeBlock(ΖΓ);if(α==null||!(α.FatBlock is IMyShipConnector)){Φ=new Vector3();return false;}Vector3 ΕЃ=ŉ(Њ,α.FatBlock.GetPosition()-м);Vector3 ΕЂ=ŉ(Њ,α.FatBlock.WorldMatrix.Forward);Φ=ж.Π-Βφ(ж.І,ж.Ё*-1,ΕЃ)-Βφ(ж.І,ж.Ё*-1,ΕЂ)*у;return true;}
else{Φ=ж.Π;return true;}}Vector3 Δκ(int Ǌ,int Ǧ,bool Ζκ){if(!Ζκ&&ΓΚ) return ǈ;float ΕЁ=((к-1f)/2f)-Ǌ;float Ζμ=((й-1f)/2f)-Ǧ;ǈ=Ψ.Π+Ψ.О*ΕЁ*ΔЄ()+ȧ*-1*Ζμ*ΔЃ();ΓΚ=true;return ǈ;}Υ Ƙ(){float Ф=-1;Υ έ=Υ.о;if(Ρ!=Ο.ο){ΔЅ();if(φ!=Τ.ё){Vector3 ύ=Βχ(İ,ȧ*-1,м-Ψ.Π);
if(Math.Abs(ύ.X)<=(float)(к*ǐ)/2f&&Math.Abs(ύ.Y)<=(float)(й*Ǐ)/2f){if(ύ.Z<=-1&&ύ.Z>=-Αλ*2) return Υ.ł;if(ύ.Z>-1&&ύ.Z<ā*2) return Υ.о;}if(ǒ(Ψ.Π,ref Ф)) έ=Υ.о;}if(Σ.ν){if(ǒ(Σ.Π,ref Ф)) έ=Υ.н;for(int i=0;i<ω.Count;i++)
if(ǒ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Vector3.Distance(м,Σ.Π)<dockDist*ā) έ=Υ.н;if(ȏ(MyShipConnectorStatus.Connectable)!=null||ȏ(MyShipConnectorStatus.Connected)!=null)
έ=Υ.н;}}else{Vector3 Φ=new Vector3();IMyShipConnector Ş=ȏ(MyShipConnectorStatus.Connected);if(Σ.ν){if(ǒ(Σ.Π,ref Ф)) έ=Υ.н;if(ΒΘ(Σ,dockDist,true,out Φ))
if(ǒ(Φ,ref Ф)) έ=Υ.н;if(Ş!=null&&Vector3.Distance(Ş.GetPosition(),Σ.Π)<5)
return Υ.Ɛ;}for(int i=0;i<ω.Count;i++)
if(Vector3.Distance(ω[i].Π,Σ.Π)>dockDist*ā&&Vector3.Distance(ω[i].Π,Ψ.Π)>dockDist*ā)
if(ǒ(ω[i].Π,ref Ф)) έ=Υ.ђ;if(Ψ.ν){if(ǒ(Ψ.Π,ref Ф)) έ=Υ.о;if(ΒΘ(Ψ,dockDist,true,out Φ))
if(ǒ(Φ,ref Ф)) έ=Υ.о;if(Ş!=null&&Vector3.Distance(Ş.GetPosition(),Ψ.Π)<5)
return Υ.Ƒ;}}return έ;}bool ǒ(Vector3 Φ,ref float Ф){float у=Vector3.Distance(Φ,м);if(у<Ф||Ф==-1){Ф=у;return true;}return false;}void Βσ(){if(Αξ>0){Ϊ="Setup error! Can't start";return;}if(Ρ==Ο.ο){ΑЏ();return;}Ψ.Π=м;Ψ.π=Њ.GetNaturalGravity();Ψ.І=ΕА;Ψ.Ё=ΕЉ;Ψ.О=ΖΣ;
İ=ǥ.WorldMatrix.Forward;ȧ=Ψ.Ё;if(İ==Њ.WorldMatrix.Down) ȧ=Њ.WorldMatrix.Backward;Ǻ=ǐ;ǹ=Ǐ;ƻ(true,true);ε(Ν.Ɯ);Ƹ();}void ƻ(bool Ш,bool Ζλ){if(φ==Τ.ё&&!Ш) return;bool Ζέ=Ш||φ==Τ.ΒΑ||к!=Ц||й!=Х||Ȣ!=Ȇ;if(Ζέ){if(φ!=Τ.ё){φ=Τ.ń;Δκ(ŭ,Ů,Ζλ);Ϊ="Job changed, lost progress";
}Ȣ=Ȇ;к=Ц;й=Х;Ů=0;ŭ=0;з="";Ȁ=-1;Ŵ=(int)(DateTime.Now.Ticks/10000000%1000000);Αά=0;Į=0;ΒЌ=0;с=0;Βω(true);}}void Ư(){ΕΦ(м,0);Ŏ(Ĺ,true);}void Ƹ(){Βπ(ǘ.Βν);Ư();Ƶ(ΑΣ,false);ƶ(Αυ);φ=Τ.Ǫ;ΔΓ(true);ǡ=φ;Ω(Κ.Ċ);ΕΞ();ΒЍ=true;ΑΟ=0;for(int i=ē.Count-1;i>=0;i--)
if(ΒΨ(ē[i],false)) ΑΟ++;if(ΑΟ>0) Ϊ="Started with damage";}void Ǯ(){if(φ==Τ.Ǫ){φ=Τ.Ʈ;Ϊ="Job paused";}ε(Ν.ǫ);ǡ=φ;ΕΧ(false,0,0,0,0);Εα();ΓЊ(new Vector3(),false);Εβ();ΕΣ(ChargeMode.Auto);ΕΩ(false);ΕΥ(true);ǭ(Ν.ǫ);ŗ(false,false,0,0);Ŏ(а,false);Ŏ(ě,true);ƶ(true);Γτ=false;ΔΗ();
ΕЈ=false;ΕΗ();ΒЍ=false;Αρ=false;ΓЃ=false;if(δ!=Κ.Main&&δ!=Κ.Ń&&δ!=Κ.Ə&&δ!=Κ.ő)
Ω(Κ.Main);}void ΑЏ(){Υ ќ=Ƙ();if(Ρ==Ο.ο){if(!Ψ.ν||!Σ.ν) return;Ƹ();bool ΑΙ=Vector3.Distance(м,Σ.Π)<Vector3.Distance(м,Ψ.Π);if(Αμ==Τ.ŀ) ΑΙ=true;if(Αμ==Τ.Ł) ΑΙ=false;if(ΑΙ){ŏ(Σ,Τ.ŀ);switch(ќ){case Υ.Ɛ:ε(Ν.Ē);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.н:ε(Ν.ȍ);break;default:ε(Ν.Đ);break;
}}else{ŏ(Ψ,Τ.Ł);switch(ќ){case Υ.Ƒ:ε(Ν.Ē);break;case Υ.о:ε(Ν.ȍ);break;case Υ.ђ:ε(Ν.Ǖ);break;default:ε(Ν.Đ);break;}}}else{if(φ!=Τ.Ʈ&&φ!=Τ.ń) return;bool ΖЉ=φ==Τ.ń;Ƹ();bool ΑΛ=ΔϏ(false)&&Σ.ν;switch(ќ){case Υ.о:ε(ΑΛ?Ν.ƛ:Ν.Ɯ);break;case Υ.ђ:ε(ΑΛ?Ν.ĉ:Ν.Ǖ);break;case Υ.н:ε(ΑΛ?Ν.ȍ:Ν.đ);break;
case Υ.ł:{if(Į!=с||ΖЉ) ε(Ν.Ʊ);else ε(Ν.Βα);}break;default:break;}}}void Δί(){if(φ==Τ.ё&&!Σ.ν) return;if(Ρ==Ο.ο&&(!Ψ.ν||!Σ.ν)) return;Ϊ="Move to job";Υ ќ=Ƙ();if(Ρ==Ο.ο){ŏ(Ψ,Τ.Ł);switch(ќ){case Υ.о:ε(Ν.ȍ);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.Ƒ:return;default:ε(Ν.Đ);break;
}ǭ(Ν.Ē);}else{switch(ќ){case Υ.о:ε(Ν.Ǖ);break;case Υ.ђ:ε(Ν.Ǖ);break;case Υ.н:ε(Ν.đ);break;case Υ.ł:ε(Ν.Ʊ);break;default:break;}if(φ==Τ.ё) ǭ(Ν.Ǖ);else ǭ(Ν.ΑЊ);Γτ=true;}Ư();Ω(Κ.ƺ);Ƶ(ΑΣ,false);ǡ=Τ.Βγ;}void Δά(){if(!Σ.ν) return;Ϊ="Move home";Υ ќ=Ƙ();if(Ρ==Ο.ο){ŏ(Σ,Τ.ŀ);switch(ќ)
{case Υ.ђ:ε(Ν.ĉ);break;case Υ.н:ε(Ν.ȍ);break;case Υ.Ɛ:return;default:ε(Ν.Đ);break;}ǭ(Ν.Ē);}else{if(ȏ(MyShipConnectorStatus.Connected)!=null) return;if(ȏ(MyShipConnectorStatus.Connectable)!=null){ε(Ν.ΑБ);ǭ(Ν.đ);return;}switch(ќ){case Υ.о:ε(Ν.ƛ);break;case Υ.ђ:ε(Ν.ĉ);break;case Υ.н:ε(Ν.ĉ);break;
case Υ.ł:ε(Ν.Βμ);break;default:break;}ǭ(Ν.đ);}Ư();Ω(Κ.ƺ);Ƶ(ΑΣ,false);ǡ=Τ.Ββ;}void ε(Ν Αα){if(Αα==Ν.ǫ) ķ=Ν.ǫ;if(ķ!=Ν.ǫ&&ţ==ķ&&Αα!=ķ){Ǯ();return;}Ζω=true;ţ=Αα;}void ǭ(Ν Ѐ){this.ķ=Ѐ;}void Βω(bool Αθ){if(Αθ){ų=0;return;}if(Ρ==Ο.ο) return;float κ=с*Math.Max(1,д);
if(Į==с) κ+=Math.Min(д,ΒЌ);float ΖЄ=к*й*Math.Max(1,д);ų=Math.Max(ų,(float) Math.Min(κ/ΖЄ*100.0,100));}List<IMyTerminalBlock>Δβ(){List<IMyTerminalBlock>ǲ=new List<IMyTerminalBlock>();for(int i=0;i<ē.Count;i++){IMyTerminalBlock α=ē[i];if(ΒΨ(α,false)) ǲ.Add(α);}return ǲ;
}bool ΒΨ(IMyTerminalBlock α,bool Αψ){return(!ΒΩ(α,Αψ)||!α.IsFunctional);}bool ΒΩ(IMyTerminalBlock α,bool Αψ){if(α==null) return false;try{IMyCubeBlock ΓΙ=Me.CubeGrid.GetCubeBlock(α.Position).FatBlock;if(Αψ) return ΓΙ==α;else return ΓΙ.GetType()==α.GetType();}catch{return false;}}bool ΔϏ(bool ΗΙ)
{if(ȋ&&Ρ!=Ο.ο)
if(Ђ!=-1&&Γϊ>=Ђ){Ϊ="Ship too heavy";return true;}if(ΑΧ>=ш||Αρ){Αρ=false;Ϊ="Ship is full";return true;}return false;}static String Δπ(IMyThrust э){return э.BlockDefinition.SubtypeId;}Vector3 Ƨ(Vector3 К,float[,]ȥ){return new Vector3(
К.X>=0?ȥ[0,0]:ȥ[0,1],
К.Y>=0?ȥ[1,0]:ȥ[1,1],
К.Z>=0?ȥ[2,0]:ȥ[2,1]);}bool ΒΟ(ά ή,String ȉ,out float ΑΔ){ΑΔ=0;int Α=ǎ.IndexOf(ȉ);if(Α==-1||ή.Я==null||Α>=ή.Я.Length)
return false;ΑΔ=ή.Я[Α];if(ΑΔ==-1) return false;return true;}Vector3 ΒΞ(Vector3 К,ά ή){if(ή!=null){Vector3 έ=new Vector3();for(int i=0;i<Ǎ.Keys.Count;i++){String ȉ=Ǎ.Keys.ElementAt(i);float ğ=0;if(!ΒΟ(ή,ȉ,out ğ))
return Ƨ(К,Ļ);έ+=Ƨ(К,Ǎ.Values.ElementAt(i))*ğ;}return έ;}return Ƨ(К,Ļ);}float Ɵ(Vector3 К,ά ή){return Ɵ(К,new Vector3(),ή);}float Ɵ(Vector3 К,Vector3 π,ά ή){Vector3 Ζώ=ΒΞ(К,ή);Vector3 ΕЋ=Ζώ+π*Γϊ;float ȃ=(ΕЋ/К).AbsMin();return(float)(К*ȃ).Length();}float Ň(Vector3 Ĥ,Vector3 Ѓ,Vector3 π,ά ή)
{if(π.Length()==0f) return 0;Vector3 ΖΥ=Βχ(Ĥ,Ѓ,Vector3.Normalize(π));float Ш=Ɵ(-ΖΥ,ή);return Ш/π.Length();}void ΔΓ(bool ΖΚ){float Р=0;float ΑΕ=0.9f;if(ΖΚ){Ђ=-1;Ȋ=0;ƃ=null;if(φ!=Τ.ё&&Ψ.π.Length()!=0){Р=ΑΕ*Ň(Ψ.І,Ψ.Ё*-1,Ψ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}if(Σ.ν&&Σ.π.Length()!=0)
{Р=ΑΕ*Ň(Σ.І,Σ.Ё*-1,Σ.π,null);if(Р<Ђ||Ђ==-1) Ђ=Р;}return;}if(Ȋ==-1) return;if(Ȋ>=0){int Γο=0;while(Ȋ<ω.Count){if(Γο>100) return;Γο++;ά ή=ω[Ȋ];if(ή.π.Length()!=0f){Р=ΑΕ*Math.Min(
Ň(ή.І,ή.Ё*-1,ή.π,ή),
Ň(ή.І*-1,ή.Ё*-1,ή.π,ή));if(Р<Ђ||Ђ==-1) Ђ=Р;}else ƃ=ή;Ȋ++;}Ȋ=-1;}bool ΑΚ=true;float Ǣ=0;if(ƃ!=null){for(int i=0;i<Ǎ.Count;i++){String ȉ=Ǎ.Keys.ElementAt(i);float[,]ΑЁ=Ǎ.Values.ElementAt(i);float ğ=0;if(!ΒΟ(ƃ,ȉ,out ğ)){ΑΚ=false;break;}for(int a=0;a<ΑЁ.GetLength(0);a++)
for(int b=0;b<ΑЁ.GetLength(1);b++){float ǯ=Math.Abs(ΑЁ[a,b]*ğ);if(ǯ==0) continue;ΑΚ=true;if(Ǣ==0||ǯ<Ǣ) Ǣ=ǯ;}}}if(!ΑΚ){for(int a=0;a<Ļ.GetLength(0);a++)
for(int b=0;b<Ļ.GetLength(1);b++){float ǯ=Math.Abs(Ļ[a,b]);if(ǯ==0) continue;if(Ǣ==0||ǯ<Ǣ) Ǣ=ǯ;}}if(Ǣ>0){float ΖΩ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Р=С(Ǣ,ΖΩ);if(Р>0)
if(Р<Ђ||Ђ==-1) Ђ=Р;}}void ΓЊ(Vector3 Ш,bool Ϗ){if(!Ϗ){for(int i=0;i<Ĺ.Count;i++)
Ĺ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ǆ=ΒΞ(Ш,null);float ΖА=Math.Min(1,Math.Abs(С(Ш.X,Ǆ.X)));float ΖБ=Math.Min(1,Math.Abs(С(Ш.Y,Ǆ.Y)));float ΗΑ=Math.Min(1,Math.Abs(С(Ш.Z,Ǆ.Z)));for(int i=0;i<Ĺ.Count;i++){IMyThrust э=Ĺ[i];Vector3 ȝ=ΕΜ(ŉ(Њ,э.WorldMatrix.Backward),1);
if(ȝ.X!=0&&Math.Sign(ȝ.X)==Math.Sign(Ш.X))
э.SetValueFloat("Override",э.MaxThrust*ΖА);else if(ȝ.Y!=0&&Math.Sign(ȝ.Y)==Math.Sign(Ш.Y))
э.SetValueFloat("Override",э.MaxThrust*ΖБ);else if(ȝ.Z!=0&&Math.Sign(ȝ.Z)==Math.Sign(Ш.Z))
э.SetValueFloat("Override",э.MaxThrust*ΗΑ);else
э.SetValueFloat("Override",0.0f);}}void ΕΧ(bool Ϗ,float Ζζ,float Ȭ,float Ȱ,float ć){for(int i=0;i<ΓΜ.Count;i++){IMyGyro ρ=ΓΜ[i];ρ.GyroOverride=Ϗ;if(!Ϗ) ρ.GyroPower=100;else ρ.GyroPower=Ζζ;if(!Ϗ) continue;Vector3 М=Њ.WorldMatrix.Forward;Vector3 ȯ=Њ.WorldMatrix.Right;Vector3 Ѓ=Њ.WorldMatrix.Up;
Vector3 Ȟ=ρ.WorldMatrix.Forward;Vector3 Ƞ=ρ.WorldMatrix.Up;Vector3 ȟ=ρ.WorldMatrix.Left*-1;if(Ȟ==М) ρ.SetValueFloat("Roll",ć);else if(Ȟ==(М*-1)) ρ.SetValueFloat("Roll",ć*-1);else if(Ƞ==(М*-1)) ρ.SetValueFloat("Yaw",ć);else if(Ƞ==М) ρ.SetValueFloat("Yaw",ć*-1);else if(ȟ==М) ρ.SetValueFloat("Pitch",ć);
else if(ȟ==(М*-1)) ρ.SetValueFloat("Pitch",ć*-1);if(ȟ==(ȯ*-1)) ρ.SetValueFloat("Pitch",Ȭ);else if(ȟ==ȯ) ρ.SetValueFloat("Pitch",Ȭ*-1);else if(Ƞ==ȯ) ρ.SetValueFloat("Yaw",Ȭ);else if(Ƞ==(ȯ*-1)) ρ.SetValueFloat("Yaw",Ȭ*-1);else if(Ȟ==(ȯ*-1)) ρ.SetValueFloat("Roll",Ȭ);
else if(Ȟ==ȯ) ρ.SetValueFloat("Roll",Ȭ*-1);if(Ƞ==(Ѓ*-1)) ρ.SetValueFloat("Yaw",Ȱ);else if(Ƞ==Ѓ) ρ.SetValueFloat("Yaw",Ȱ*-1);else if(ȟ==Ѓ) ρ.SetValueFloat("Pitch",Ȱ);else if(ȟ==(Ѓ*-1)) ρ.SetValueFloat("Pitch",Ȱ*-1);else if(Ȟ==Ѓ) ρ.SetValueFloat("Roll",Ȱ);else if(Ȟ==(Ѓ*-1)) ρ.SetValueFloat("Roll",Ȱ*-1);
}}void Εβ(){this.ΖΔ=false;}void Εα(){Ő(false,false,false,Γι,0);ΓЀ=false;}void ΕΦ(Vector3 ΰ,float Ķ){Ő(true,false,false,ΰ,Ķ);}void Ő(bool Ϗ,bool Ατ,bool ΑЂ,Vector3 ΰ,float Ķ){Ő(Ϗ,Ατ,ΑЂ,ΰ,ΰ-м,0.0f,Ķ);}void Ő(bool Ϗ,bool Ατ,bool ΑЂ,Vector3 ΰ,Vector3 Ζρ,float Ζς,float Ķ)
{ΓЀ=true;this.ΖϏ=Ϗ;Γι=ΰ;this.ΖΧ=Ķ;this.Ζδ=Ζς;this.Ζχ=Ατ;this.Ζφ=ΑЂ;this.ΕЎ=Ζρ;ΓΑ=Vector3.Distance(ΰ,м);}void ΕΞ(){Н.GetBlocksOfType(ē,ƥ);}void ΔΔ(){if(Њ==null) return;ǥ=null;float Ż=0,ŷ=0,ż=0,Ÿ=0,Ž=0,Ź=0;List<IMyTerminalBlock>Αω=ΒΓ(а,pamTag,true);bool Ζί=Αω.Count==0;
if(Αω.Count>0) ǥ=Αω[0];else if(а.Count>0) ǥ=а[0];int Ţ=0;float ы=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǥ.WorldMatrix.Forward){if(Ζί){Αξ=2;Ϊ="Mining direction is unclear!";return;}continue;}Ţ++;ы=Math.Max(ы,Εκ(а[i]));Vector3 ύ=ƞ(Њ,а[i].GetPosition());
if(i==0){Ż=ύ.X;ŷ=ύ.X;ż=ύ.Y;Ÿ=ύ.Y;Ž=ύ.Z;Ź=ύ.Z;}ŷ=Math.Max(ύ.X,ŷ);Ż=Math.Min(ύ.X,Ż);Ÿ=Math.Max(ύ.Y,Ÿ);ż=Math.Min(ύ.Y,ż);Ź=Math.Max(ύ.Z,Ź);Ž=Math.Min(ύ.Z,Ž);}if(ы==0) ы=drillRadius;bool ĝ=ǥ!=null&&ǥ.WorldMatrix.Forward==Њ.WorldMatrix.Down;ǐ=(ŷ-Ż)*(1-Ŀ)+ы*2;
Ǐ=(ĝ?Ź-Ž:Ÿ-ż)*(1-ħ)+ы*2;IMyCubeGrid g=Њ.CubeGrid;float ΑЅ=0,ΑІ=0,ΑЇ=0,ΑЈ=0;for(int c=0;c<8;c++){Vector3 p=ƞ(Њ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ĝ?p.Z:p.Y;if(c==0||p.X<ΑЅ) ΑЅ=p.X;if(c==0||p.X>ΑІ) ΑІ=p.X;if(c==0||y<ΑЇ) ΑЇ=y;if(c==0||y>ΑЈ) ΑЈ=y;}Γω=ΑІ-ΑЅ+g.GridSize;Γψ=ΑЈ-ΑЇ+g.GridSize;}float Εκ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΒΓ<T>(List<T>Ї,String Ю,bool ů){List<T>έ=new List<T>();for(int i=0;i<Ї.Count;i++)
if(ΒΣ(Ї[i],Ю,ů)) έ.Add(Ї[i]);return έ;}bool ΒΣ<T>(T α,String Ю,bool ů){IMyTerminalBlock b=(IMyTerminalBlock) α;if(ů&&b.CustomName.ToUpper().Contains(Ю.ToUpper())) return true;if(!ů&&b.BlockDefinition.ToString().ToUpper().Contains(Ю.ToUpper())) return true;return false;}void Ŏ<T>(List<T>Ї,bool Ϗ)
{for(int i=0;i<Ї.Count;i++)
ΕΤ((IMyTerminalBlock) Ї[i],Ϗ);}void ΕΤ(IMyTerminalBlock α,bool Ϗ){if(α==null) return;String Εχ=Ϗ?"OnOff_On":"OnOff_Off";var a=α.GetActionWithName(Εχ);a.Apply(α);}void ƶ(bool Ϗ){Αυ=Ϗ;if(!Ȉ) return;Ŏ(Ζψ,Ϗ);}void ΕΣ(ChargeMode λ){for(int i=0;i<Βώ.Count;i++) Βώ[i].ChargeMode=λ;}void ΕΩ(bool Ζϋ)
{for(int i=0;i<Ī.Count;i++) Ī[i].Stockpile=Ζϋ;}void Ƶ(List<IMyLandingGear>ΑΘ,bool Γβ){for(int i=0;i<ΑΘ.Count;i++){if(Γβ) ΑΘ[i].Lock();if(!Γβ) ΑΘ[i].Unlock();}}void ΕΥ(bool Ϗ){if(ΒЎ==Ϗ) return;List<IMyShipController>š=new List<IMyShipController>();Н.GetBlocksOfType(š,ƥ);if(š.Count==0) return;
for(int i=0;i<š.Count;i++)
š[i].DampenersOverride=Ϗ;ΒЎ=Ϗ;}IMyShipConnector ȏ(MyShipConnectorStatus Ζϊ){for(int i=0;i<ě.Count;i++){if(!ΒΩ(ě[i],true)) continue;if(ě[i].Status==Ζϊ) return ě[i];}return null;}void ŗ(bool Αο,bool ť,float ΕБ,float Εϊ){if(ώ==null||а.Count==0) return;Vector3 ș=new Vector3();
int Ţ=0;for(int i=0;i<а.Count;i++){if(а[i].WorldMatrix.Forward!=ǥ.WorldMatrix.Forward) continue;Ţ++;ș+=а[i].GetPosition();}ș=ș/Ţ;Vector3 ȩ=ƞ(ώ,ș);ώ.Enabled=true;ώ.ShowOnHUD=Αο;ώ.LeftExtend=(ť?1:Ц)/2f*ǐ-ȩ.X;ώ.RightExtend=(ť?1:Ц)/2f*ǐ+ȩ.X;ώ.TopExtend=(ť?1:Х)/2f*Ǐ+ȩ.Y;
ώ.BottomExtend=(ť?1:Х)/2f*Ǐ-ȩ.Y;ώ.FrontExtend=(Αο?д:ΕБ)-ȩ.Z;ώ.BackExtend=Αο?0:Εϊ+ā*0.75f+ȩ.Z;ώ.DetectFloatingObjects=true;ώ.DetectAsteroids=false;ώ.DetectLargeShips=true;ώ.DetectSmallShips=true;ώ.DetectStations=true;ώ.DetectOwner=true;ώ.DetectSubgrids=false;
ώ.DetectPlayers=false;ώ.DetectEnemy=true;ώ.DetectFriendly=true;ώ.DetectNeutral=true;}void č<T>(List<T>Ї,bool Ζτ,float ΕЏ,bool ΖΝ){for(int i=0;i<Ї.Count;i++){IMyTextSurface ĸ=null;if(Ї[i]is IMyTextSurface) ĸ=(IMyTextSurface) Ї[i];if(ĸ!=null){ĸ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ζτ) continue;ĸ.Font="Debug";if(ΖΝ) continue;ĸ.FontSize=ΕЏ;}}}static float С(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΕΜ(Vector3 v,int ǌ){return new Vector3(Math.Round(v.X,ǌ),Math.Round(v.Y,ǌ),Math.Round(v.Z,ǌ));}Vector3 ƞ(IMyTerminalBlock α,Vector3 ΖЎ){return ŉ(α,ΖЎ-α.GetPosition());
}Vector3 ŉ(IMyTerminalBlock α,Vector3 ΑЄ){return Vector3D.TransformNormal(ΑЄ,MatrixD.Transpose(α.WorldMatrix));}Vector3 Βχ(Vector3 Ĥ,Vector3 Ѓ,Vector3 ΑЄ){MatrixD ΑΫ=MatrixD.CreateFromDir(Ĥ,Ѓ);return Vector3D.TransformNormal(ΑЄ,MatrixD.Transpose(ΑΫ));}Vector3 Βφ(Vector3 Ĥ,Vector3 Ѓ,Vector3 ύ)
{MatrixD ΑΫ=MatrixD.CreateFromDir(Ĥ,Ѓ);return Vector3D.Transform(ύ,ΑΫ);}String γ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ϋ(String s){String[]θ=s.Split('|');return new Vector3(float.Parse(Ι(θ,0)),float.Parse(Ι(θ,1)),float.Parse(Ι(θ,2)));}String Ευ(ά ή){String ȅ=":";
String έ=γ(ή.Π)+ȅ+γ(ή.І)+ȅ+
γ(ή.Ё)+ȅ+γ(ή.О)+ȅ+γ(ή.π);for(int i=0;i<ή.Я.Length;i++)
έ+=ȅ+Math.Round(ή.Я[i],3);return έ;}ά Εδ(String s){String[]θ=s.Split(':');ά ή=new ά(
Ϋ(Ι(θ,0)),Ϋ(Ι(θ,1)),
Ϋ(Ι(θ,2)),Ϋ(Ι(θ,3)),
Ϋ(Ι(θ,4)));int Α=5;List<float>ΓΖ=new List<float>();while(Α<θ.Length){String Ƈ=Ι(θ,Α);float f=0;if(!float.TryParse(Ƈ,out f)) break;ΓΖ.Add(f);Α++;}ή.Я=ΓΖ.ToArray();return ή;}System.Text.StringBuilder Ȳ=new System.Text.StringBuilder();void Μ<T>(T value,bool Ζή){if(Ζή) Ȳ.Append('\n');Ȳ.Append(value);}
void Μ<T>(T value){Μ(value,true);}String Ņ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ŗ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Εψ,int Α){String Ƈ=Εψ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Ƈ)) return "";
return Ƈ;}void Save(){if(Γσ||Ρ==Ο.Е){Storage="";return;}Storage=ΓЏ();}String ΓЏ(){Ȳ.Clear();Ȳ.Append(DATAREV+";");Μ(γ(Γς),false);Μ(γ(Σ.І));Μ(γ(Σ.О));Μ(γ(Σ.Ё));Μ(γ(Σ.π));Μ(γ(Σ.Π));Μ(γ(Σ.Ч));Μ(Σ.ν);Μ(γ(Ψ.Π));Μ(γ(Ψ.π));Μ(γ(Ψ.І));Μ(γ(Ψ.Ё));Μ(γ(Ψ.О));Μ(γ(Ψ.Ч));Μ(Ψ.ν);Μ(γ(İ));Μ(γ(ȧ));Μ(";");
Μ((int) Ρ,false);Μ((int) φ);Μ((int) ǡ);Μ(ш);Μ(щ);Μ(Ы);Μ(ъ);Μ(ǚ);Μ(ȋ);Μ(Ǩ);if(Ρ==Ο.ο){Μ((int) χ.П);Μ(χ.е);Μ(χ.Ǵ);Μ(Ņ(χ.ф));Μ(Ņ(χ.ч));Μ((int) ψ.П);Μ(ψ.е);Μ(ψ.Ǵ);Μ(Ņ(ψ.ф));Μ(Ņ(ψ.ч));}else{Μ((int) Ȇ);Μ((int) ǣ);Μ((int) Ġ);Μ((int) ј);Μ((int) Ȣ);Μ(ľ);Μ(Ȉ);Μ(ī);Μ(ġ);Μ(Ц);Μ(Х);Μ(д);Μ(ƌ);Μ(Ƌ);Μ(Ŀ);Μ(ħ);
Μ(к);Μ(й);Μ(ŭ);Μ(Ů);Μ(Į);Μ(с);Μ(Αλ);Μ(Αά);}Μ(";");for(int i=0;i<ǎ.Count;i++)
Μ((i>0?"|":"")+ǎ[i],false);Μ(";");for(int i=0;i<ω.Count;i++)
Μ(Ευ(ω[i]),i>0);Μ(";",false);Μ(з+"|"+Ȁ+"|"+Ǻ+"|"+ǹ+"|"+Ŵ,false);Μ(";",false);Μ(ΔΡ(),false);String Ξ=Ȳ.ToString();Ȳ.Clear();return Ξ;}п ΕΙ(){return ΕΚ(Storage);}п ΕΚ(String Ξ){if(Ξ=="") return п.ΔЉ;String[]Ȅ=Ξ.Split(';');if(Ι(Ȅ,0)!=DATAREV) return п.Βδ;int Α=0;try
{String[]Ζ=Ι(Ȅ,1).Split('\n');Γς=Ϋ(Ι(Ζ,Α++));Σ.І=Ϋ(Ι(Ζ,Α++));Σ.О=Ϋ(Ι(Ζ,Α++));Σ.Ё=Ϋ(Ι(Ζ,Α++));Σ.π=Ϋ(Ι(Ζ,Α++));Σ.Π=Ϋ(Ι(Ζ,Α++));Σ.Ч=Ϋ(Ι(Ζ,Α++));Σ.ν=bool.Parse(Ι(Ζ,Α++));Ψ.Π=Ϋ(Ι(Ζ,Α++));Ψ.π=Ϋ(Ι(Ζ,Α++));Ψ.І=Ϋ(Ι(Ζ,Α++));Ψ.Ё=Ϋ(Ι(Ζ,Α++));Ψ.О=Ϋ(Ι(Ζ,Α++));Ψ.Ч=Ϋ(Ι(Ζ,Α++));
Ψ.ν=bool.Parse(Ι(Ζ,Α++));İ=Ϋ(Ι(Ζ,Α++));ȧ=Ϋ(Ι(Ζ,Α++));Ζ=Ι(Ȅ,2).Split('\n');Α=0;Ρ=(Ο) int.Parse(Ι(Ζ,Α++));φ=(Τ) int.Parse(Ι(Ζ,Α++));ǡ=(Τ) int.Parse(Ι(Ζ,Α++));ш=int.Parse(Ι(Ζ,Α++));щ=int.Parse(Ι(Ζ,Α++));Ы=int.Parse(Ι(Ζ,Α++));ъ=int.Parse(Ι(Ζ,Α++));ǚ=float.Parse(Ι(Ζ,Α++));
ȋ=bool.Parse(Ι(Ζ,Α++));Ǩ=bool.Parse(Ι(Ζ,Α++));if(Ρ==Ο.ο){χ.П=(Ѕ) int.Parse(Ι(Ζ,Α++));χ.е=float.Parse(Ι(Ζ,Α++));χ.Ǵ=float.Parse(Ι(Ζ,Α++));χ.ф=Ŗ(Ι(Ζ,Α++));χ.ч=Ŗ(Ι(Ζ,Α++));ψ.П=(Ѕ) int.Parse(Ι(Ζ,Α++));ψ.е=float.Parse(Ι(Ζ,Α++));ψ.Ǵ=float.Parse(Ι(Ζ,Α++));ψ.ф=Ŗ(Ι(Ζ,Α++));ψ.ч=Ŗ(Ι(Ζ,Α++));
}else{Ȇ=(У) int.Parse(Ι(Ζ,Α++));ǣ=(ǔ) int.Parse(Ι(Ζ,Α++));Ġ=(в) int.Parse(Ι(Ζ,Α++));ј=(я) int.Parse(Ι(Ζ,Α++));Ȣ=(У) int.Parse(Ι(Ζ,Α++));ľ=bool.Parse(Ι(Ζ,Α++));Ȉ=bool.Parse(Ι(Ζ,Α++));ī=bool.Parse(Ι(Ζ,Α++));ġ=bool.Parse(Ι(Ζ,Α++));Ц=int.Parse(Ι(Ζ,Α++));Х=int.Parse(Ι(Ζ,Α++));д=int.Parse(Ι(Ζ,Α++));
ƌ=float.Parse(Ι(Ζ,Α++));Ƌ=float.Parse(Ι(Ζ,Α++));Ŀ=float.Parse(Ι(Ζ,Α++));ħ=float.Parse(Ι(Ζ,Α++));к=int.Parse(Ι(Ζ,Α++));й=int.Parse(Ι(Ζ,Α++));ŭ=int.Parse(Ι(Ζ,Α++));Ů=int.Parse(Ι(Ζ,Α++));Į=int.Parse(Ι(Ζ,Α++));с=int.Parse(Ι(Ζ,Α++));Αλ=int.Parse(Ι(Ζ,Α++));Αά=float.Parse(Ι(Ζ,Α++));
}Ζ=Ι(Ȅ,3).Replace("\n","").Split('|');ǎ=Ζ.ToList();Ζ=Ι(Ȅ,4).Split('\n');ω.Clear();if(Ζ.Count()>=1&&Ζ[0]!="")
for(int i=0;i<Ζ.Length;i++)
ω.Add(Εδ(Ι(Ζ,i)));Ζ=Ι(Ȅ,5).Split('|');з=Ι(Ζ,0);if(!int.TryParse(Ι(Ζ,1),out Ȁ)) Ȁ=-1;if(!float.TryParse(Ι(Ζ,2),out Ǻ)) Ǻ=0;if(!float.TryParse(Ι(Ζ,3),out ǹ)) ǹ=0;if(!int.TryParse(Ι(Ζ,4),out Ŵ)) Ŵ=0;if(з.Length!=к*й){з="";Ȁ=-1;}ΔΠ(Ι(Ȅ,6));}catch{return п.ΒΒ;}Αμ=ǡ;
Ǯ();return п.Βτ;}void ΕΪ(){ǳ.Clear();for(int i=Љ.Count-1;i>=0;i--){String Ξ=Љ[i].CustomData.ToUpper();bool ΑΠ=false;if(Ξ=="INSTRUCTIONS"){ΑΠ=true;ΒЏ=true;}if(Ξ=="DEBUG") ΑΠ=true;if(ΑΠ){ǳ.Add(Љ[i]);Љ.RemoveAt(i);}}č(ǳ,false,1,false);}void ΕΫ(List<IMyTerminalBlock>ΒЁ)
{ь.Clear();for(int i=0;i<ΒЁ.Count;i++){IMyTerminalBlock α=ΒЁ[i];try{String Γξ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=α.CustomName.IndexOf(Γξ);int Ă=-1;if(Α<0||!int.TryParse(α.CustomName.Substring(Α+Γξ.Length,1),out Ă)) continue;if(Ă==-1) continue;Ă--;IMyTextSurfaceProvider Γπ=(IMyTextSurfaceProvider) α;
if(Ă<Γπ.SurfaceCount&&Ă>=0)
ь.Add(Γπ.GetSurface(Ă));}catch{}}}String Αή="";void ΕΕ(){String ΑΪ="";String Ğ="";Αή=ΓЎ(false);ΑΪ+=Αή;Ğ+=Αή;Ğ+=Δθ();for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(ΑΪ);for(int i=0;i<ь.Count;i++) ь[i].WriteText(ΑΪ);Echo(Ğ);for(int i=0;i<ǳ.Count;i++){IMyTextPanel Џ=ǳ[i];
String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΙ());}}string ΒΙ(){String Β="";try{float Γγ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ș*Γγ)+" / "+(int)(Ȧ*Γγ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ș*100f,1)+"% / "+Math.Round(Ȧ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ĕ,2)+"ms / "+Math.Round(Αέ,2)+"ms \n";}catch{}for(int i=0;i<ƀ.Count;i++)
Β+=""+ƀ.Keys.ElementAt(i)+" = "+Math.Round(ƀ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ƀ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Δυ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";č(Љ,setLCDFontAndSize,0.9f,false);č(ь,setLCDFontAndSize,0.9f,true);for(int i=0;i<Љ.Count;i++) Љ[i].WriteText(Β+Ȥ.ToString());for(int i=0;i<ь.Count;i++) ь[i].WriteText(Β+Ȥ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Ƴ(String η,String ΰ,String ǵ){try{if(!ė) return;η=BROADCAST_TAG+BROADCAST_SEP+ΒΖ()+BROADCAST_SEP+ΰ+BROADCAST_SEP+Ǉ+BROADCAST_SEP+ǵ+BROADCAST_SEP+η;this.IGC.SendBroadcastMessage(BROADCAST_TAG,η);}catch(Exception e){Ȥ=e;}}String ΒΖ(){if(Ρ!=Ο.Е) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΒΪ(String η){return η.StartsWith(BROADCAST_TAG);}bool Є(ref String Ξ,out string Ģ,Char ȅ){int Α=Ξ.IndexOf(ȅ);Ģ="";if(Α<0) return false;Ģ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΕΑ(ref String η,out String ǧ,out String ǵ){ǧ="";ǵ="";if(!ė) return false;
String Ю="";if(!Є(ref η,out Ю,BROADCAST_SEP)||!ΒΪ(Ю)) return false;if(!Є(ref η,out ǧ,BROADCAST_SEP)) return false;if(!Є(ref η,out Ю,BROADCAST_SEP)||(Ю!=ΒΖ()&&(Ю!="*"&&Ρ!=Ο.Е))) return false;if(!Є(ref η,out Ю,BROADCAST_SEP)||(Ю!=Ǉ)) return false;if(!Є(ref η,out ǵ,BROADCAST_SEP)) return false;
return true;}void Δύ(){bool Ě=true;if(ΓΗ){ΓΗ=false;if(Me.CustomData.Contains("Antenna_Name")){Ϊ="Update custom data";Me.CustomData="";}}String ĥ=(Ρ!=Ο.Е?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ĥ)
ΒΧ(ĥ);String[]Ζ=Me.CustomData.Split('\n');ė=bool.Parse(Ō(ņ(Ζ,"Enable_Broadcast",ref Ě)));bool ΑЃ=false;bool ΒБ=true;if(ė){if(Ρ!=Ο.Е)
ĵ=Ō(ņ(Ζ,"Ship_Name",ref Ě)).Replace(BROADCAST_SELF_ID,"");Ǉ=Ō(ņ(Ζ,"Broadcast_Channel",ref Ě)).ToLower();ΒБ=false;if(є==null){є=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);є.SetMessageCallback("");}List<IMyRadioAntenna>ȗ=new List<IMyRadioAntenna>();Н.GetBlocksOfType(ȗ);bool ǃ=false;
for(int i=0;i<ȗ.Count;i++)
if(ȗ[i].EnableBroadcasting&&ȗ[i].Enabled){ǃ=true;break;}if(ȗ.Count==0) Ϊ="No Antenna found";else if(!ǃ) Ϊ="Antenna not ready";ΑЃ=ȗ.Count==0||!ǃ;if(Βϊ&&!ΑЃ&&Ρ!=Ο.Е) Ϊ="Antenna ok";}else if(Ρ==Ο.Е)
Ϊ="Offline - Enable in PB custom data";Βϊ=ΑЃ;if(ΒБ){if(є!=null) this.IGC.DisableBroadcastListener(є);є=null;}}catch{Ě=false;}if(!Ě){Ϊ="Reset custom data";ΒΧ(ĥ);}}void ΒΧ(String ĥ){String Ξ=ĥ+"\n\n"+"Enable_Broadcast="+(Ρ==Ο.Е?"true":"false")+" \n";Ξ+=Ρ!=Ο.Е?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String Ō(String Ǽ){int Α=Ǽ.IndexOf("//");if(Α!=-1) Ǽ=Ǽ.Substring(0,Α);String[]θ=Ǽ.Split('=');if(θ.Length<=1) return "";return θ[1].Trim();}String ņ(String[]Ζ,String И,ref bool Ȫ){foreach(String Ǽ in Ζ)
if(Ǽ.StartsWith(И)) return Ǽ;Ȫ=false;return "";}void ΔΛ(string А){if(ģ) β=new List<ί>();String ǧ="";String ǵ="";if(!ΒΪ(А)) ΕΖ(А);bool Γϋ=false;int ΖΪ=50;while(є!=null&&є.HasPendingMessage&&ΖΪ-->0){MyIGCMessage η=є.AcceptMessage();String Ξ=η.Data as String;if(Ξ==null) continue;
if(ΕΑ(ref Ξ,out ǧ,out ǵ)&&ǧ!=""&&ǧ!=BROADCAST_SELF_ID){ί Γ=ƙ(ǧ,"");if(Γ==null){Γ=new ί(ǧ);β.Add(Γ);}Γ.Ǘ(ǵ,false,false,true);Εο(Γ,Ξ);Γϋ=true;}}if(Γϋ)
β.Sort(delegate(ί a,ί b){if(a.ζ==null&&b.ζ==null) return 0;else if(a.ζ==null) return-1;else if(b.ζ==null) return 1;else return a.ζ.CompareTo(b.ζ);});if(Ĭ||ģ){if(Ƃ<=0||ģ){Ϊ="";ģ=false;ΕΟ();Δύ();Γέ=DateTime.Now;}}if(ǟ) Εν();if(ǟ) Εξ();if(ǟ) Ερ();if(ǟ) Επ();if(ǟ&&Αϊ==0) Εμ();
Ελ();}void Εο(ί Γ,String Ξ){Γ.ΑΤ=DateTime.Now;String Ģ="";Є(ref Ξ,out Γ.Ɖ,BROADCAST_SEP);Є(ref Ξ,out Γ.ζ,BROADCAST_SEP);if(Γ.Ɖ!=VERSION) return;String Ĳ="";String[]ȭ=new string[3];Є(ref Ξ,out Ĳ,BROADCAST_SEP);Є(ref Ξ,out ȭ[0],BROADCAST_SEP);Є(ref Ξ,out ȭ[1],BROADCAST_SEP);Є(ref Ξ,out ȭ[2],BROADCAST_SEP);
Є(ref Ξ,out Γ.Αχ,BROADCAST_SEP);Є(ref Ξ,out Γ.Ϊ,BROADCAST_SEP);Є(ref Ξ,out Ģ,BROADCAST_SEP);Є(ref Ξ,out Γ.ź,BROADCAST_SEP);String ΓЅ="",Γε="";Є(ref Ξ,out ΓЅ,BROADCAST_SEP);Є(ref Ξ,out Γε,BROADCAST_SEP);Γ.њ.Clear();while(true){String ΓΩ;if(!Є(ref Ξ,out ΓΩ,BROADCAST_SEP)) break;String[]Ų=ΓΩ.Split('/');
if(Ų.Count()<3) continue;int Ј=0;if(!int.TryParse(Ų[2],out Ј)) continue;Γ.њ.Add(new Ǭ(Ų[0],Ų[1],Ј,ň.Δϊ));}int Γη=0;if(!int.TryParse(Ģ,out Γ.ς)) Γ.ς=0;if(!int.TryParse(Ĳ,out Γη)) Γ.Ρ=Ο.Ǚ;if(!float.TryParse(ȭ[0],out Γ.Π.X)) Γ.Π.X=0;if(!float.TryParse(ȭ[1],out Γ.Π.Y)) Γ.Π.Y=0;
if(!float.TryParse(ȭ[2],out Γ.Π.Z)) Γ.Π.Z=0;if(!float.TryParse(ΓЅ,out Γ.Ǳ)) Γ.Ǳ=0;if(!float.TryParse(Γε,out Γ.ǽ)) Γ.ǽ=0;Γ.Ρ=(Ο) Γη;Γ.ΑΑ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Π));Γ.ΑϏ=0;for(int i=0;i<Γ.њ.Count();i++) Γ.ΑϏ+=Γ.њ[i].Ј;}ί ƙ(String Λ,String ζ){Λ=Λ.ToUpper();ζ=ζ.ToUpper();
for(int i=0;i<β.Count;i++){if(Λ!=""&&β[i].Λ.ToUpper()==Λ) return β[i];if(ζ!=""&&β[i].ζ!=null&&β[i].ζ.ToUpper()==ζ) return β[i];}return null;}String Ț="";ί ΒЄ=null;DateTime ΒЅ=DateTime.MinValue;List<ί>і=new List<ί>();const float ΔΒ=20f;bool Βο(ί ΰ,String μ){return μ==Ț&&ΰ==ΒЄ&&((DateTime.Now-ΒЅ).TotalSeconds<ΔΒ||
(μ=="HOMEPOS"&&(Щ.Count>0||х!=null)));}String Δΰ(ί ΰ){if(Ț==""||!Βο(ΰ,Ț)) return "";String s="";int Ȫ=0,Απ=0;for(int i=0;i<і.Count;i++){ι Ƅ=і[i].Ǘ(Ț,false,false,false);if(Ƅ==ι.Č) Ȫ++;bool Ζι=Щ.Contains(і[i]);if(Απ++<8) s+="  "+і[i].ζ+": "+(Ζι?"waiting to leave ("+(Щ.IndexOf(і[i])+1)+")":Ƅ==ι.Č?"received":
Ƅ==ι.ȓ?"pending...":Ƅ==ι.ŋ?"no answer!":"-")+"\n";}if(Απ>8) s+="  +"+(Απ-8)+" more\n";return "--------------------------------------------\n"+Ț+": "+Ȫ+"/"+і.Count+" received\n"+s;}void Т(ί Γ,String μ){Ț=μ;ΒЄ=Γ;ΒЅ=DateTime.Now;і.Clear();if(Γ==null) і.AddRange(β);else і.Add(Γ);
if(Γ==null){Щ.Clear();х=null;}else Щ.Remove(Γ);if(Γ==null&&μ=="HOMEPOS"){Εί();return;}if(Γ==null){for(int i=0;i<β.Count;i++)
β[i].Ǘ(μ,true,false,false);Ƴ(μ,"*",μ);}else{Γ.Ǘ(μ,true,false,false);Ƴ(μ,Γ.Λ,μ);}}void ΕΖ(string А){if(А=="") return;var θ=А.ToUpper().Split(' ');θ.DefaultIfEmpty("");var μ=θ.ElementAtOrDefault(0);var Βό=θ.ElementAtOrDefault(1);String ΖΜ="Invalid argument: "+А;if(δ==Κ.Ď){if(σ!=null)
{switch(μ){case "UP":{if(ς<2) break;if(σ.ς==0){ς=1;return;}Т(σ,"UP");return;}case "DOWN":{if(ς<1) break;if(ς==1){Т(σ,"MRES");break;}Т(σ,"DOWN");break;}case "APPLY":{if(ς<2) break;Т(σ,"APPLY");return;}}}}switch(μ){case "UP":this.ΔЇ(false);return;case "DOWN":this.ΔІ(false);return;
case "APPLY":this.Ȏ(true);return;}switch(μ){case "CLEAR":ΔΘ();return;case "SENDALL":
if(String.IsNullOrEmpty(Βό)){Ϊ="Usage: SENDALL <cmd>";return;}Т(null,Βό);return;case "SEND":
if(А.Length<="SEND".Length+1){Ϊ="Usage: SEND <ship>:<cmd>";return;}ΕΡ(А.Remove(0,"SEND".Length+1));return;}Ϊ=ΖΜ;}void ΕΡ(String Βϋ){if(Βϋ=="") return;var θ=Βϋ.Split(':');if(θ.Length!=2){Ϊ="Missing separator \":\"";return;}θ.DefaultIfEmpty("");String ĵ=θ.ElementAtOrDefault(0).Trim();
ί Γ=ƙ("",ĵ);if(Γ!=null) Т(Γ,θ.ElementAtOrDefault(1).Trim());else Ϊ="Unknown ship: "+ĵ;}void ΔΘ(){β.Clear();σ=null;Ω(Κ.Main);ς=0;}void ΕΟ(){List<IMyTerminalBlock>Γύ=new List<IMyTerminalBlock>();Н.GetBlocksOfType(Љ,ƥ);Н.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Γύ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Љ=ΒΓ(Љ,pamTag,true);ΕΫ(Γύ);ΕΪ();ΕΠ();č(Љ,setLCDFontAndSize,1.15f,false);č(ь,setLCDFontAndSize,1.15f,true);String ΖΘ=ΒΚ(false);String Γκ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Џ in Љ){if(Џ.CustomData==""){Џ.CustomData=ΒΚ(true);continue;}
if(!Џ.CustomData.Contains(ΖΘ))
if(!Џ.CustomData.Contains(Γκ)) Џ.CustomData=Γκ+Џ.CustomData;}}void Ελ(){String Ğ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ь.Count;i++)
ь[i].WriteText(ΑЌ(ϊ.Βά,"0",false));for(int i=0;i<Љ.Count;i++){ϊ λ=ϊ.Ǚ;String Э="";ΕΒ(Љ[i],out λ,out Э);Љ[i].WriteText(ΑЌ(λ,Э,i==0));}Echo(Ğ+Δγ());for(int i=0;i<ǳ.Count;i++){IMyTextPanel Џ=ǳ[i];String Ξ=Џ.CustomData.ToUpper();if(Ξ=="DEBUG") Џ.WriteText(""+"\n"+"");
if(Ξ=="INSTRUCTIONS") Џ.WriteText(ΒΙ());}}String Ȏ(bool Ε){int ΕЊ=0;return Ȏ(Ε,0,ref ΕЊ,false,1);}String Ȏ(bool Ε,int Ζβ,ref int Ζπ,bool ΒЈ,int ĳ){String Β="";String Ā="——————————————————\n";String Ζύ="--------------------------------------------\n";if(σ==null||δ==Κ.Main||δ==Κ.ƴ)
Β+="[PAM]-Controller | "+β.Count+" Connected ships"+"\n"+Δε()+Δλ()+Δρ();else Β+=ΒΝ(σ)+"\n";Β+=Ā;int Α=0;int Δ=ς;Ǿ=0;if(Ϊ!=""){Η(ref Β,Δ,Α++,Ε,Ϊ);}else if(δ==Κ.Main){if(σ==null&&β.Count>=1) σ=β[0];String Ї="";if(Η(ref Ї,Δ,Α++,Ε," Send command to all")) Ω(Κ.ƴ);
int ȳ=0;for(int s=0;s<β.Count;s++){if(ΒЈ) Ї+="\n";ί Γ=β[s];if(Δ==Α||Δ==Α+1) σ=Γ;if(Α==Δ) ȳ=Ї.Split('\n').Length-1;if(Η(ref Ї,Δ,Α++,Ε," "+ΒΝ(Γ))){σ=Γ;Ω(Κ.Ď);σ.Ǘ("",false,true,false);}if(Α==Δ) ȳ=Ї.Split('\n').Length-1;if(Η(ref Ї,Δ,Α++,Ε," "+(ΒЈ?"":"  ")+Δν(Γ)))
{σ=Γ;Ω(Κ.Βξ);}}int Ć=ĳ-2;Ć+=Math.Max(0,(Ζβ-1))*ĳ;Β+=Ʋ(Ć,Ї,ȳ,ref Ζπ);}else if(δ==Κ.ō||δ==Κ.ƴ){Κ Εω=δ==Κ.ō?Κ.Ď:Κ.Main;ί ΰ=null;if(δ==Κ.ō) ΰ=σ;if(Η(ref Β,Δ,Α++,Ε," Back")){Ω(Εω);}if(Η(ref Β,Δ,Α++,Ε," [Stop] "+Ȑ(ΰ,"STOP"))){Т(ΰ,"STOP");}if(Η(ref Β,Δ,Α++,Ε," [Continue] "+Ȑ(ΰ,"CONT"))){Т(ΰ,"CONT");}
if(Η(ref Β,Δ,Α++,Ε," [Move home] "+Ȑ(ΰ,"HOMEPOS"))){Т(ΰ,"HOMEPOS");}if(Η(ref Β,Δ,Α++,Ε," [Move to job] "+Ȑ(ΰ,"JOBPOS"))){Т(ΰ,"JOBPOS");}if(Η(ref Β,Δ,Α++,Ε," [Full simulation] "+Ȑ(ΰ,"FULL"))){Т(ΰ,"FULL");}if(ΰ!=null&&ΰ.Ρ==Ο.ο)
if(Η(ref Β,Δ,Α++,Ε," [Undock] "+Ȑ(ΰ,"UNDOCK"))){Т(ΰ,"UNDOCK");}Β+=Δΰ(ΰ);}else if(δ==Κ.Ď){String ǅ="";if(Η(ref ǅ,Δ,Α++,Ε," Back")){Ω(Κ.Main);}Β+=ǅ.Substring(0,ǅ.Length-1).PadRight(36,' ');if(Η(ref Β,Δ,Α++,Ε," Send cmd")){Ω(Κ.ō);}if(!σ.Ʀ()&&!σ.ƣ()) Ǿ++;if(σ.ƣ()) Β+="No answer received...";
else Β+=ȕ(σ,σ.ź);}else if(δ==Κ.Βξ){if(Η(ref Β,Δ,Α++,Ε," Back")){Ω(Κ.Main);}Β+=Ζύ;Β+=ȕ(σ,ΒΜ(σ));}if(!Αί) ς=Math.Min(Ǿ-1,ς);return Β;}String ΒΝ(ί Γ){if(Γ.Ɖ!=VERSION) return Γ.ζ+": Different version!";return Γ.ζ+": "+ȕ(Γ,Γ.Αχ)+" "+Γ.ΑΑ+"m";}String Δν(ί Γ){String s=ư("",С(Γ.Ǳ,Γ.ǽ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.њ.Count;i++){if(i>=5) break;Ǭ ц=Γ.њ[i];s+=Βψ(ƒ(Δι(ц)),3)+" "+ΒΔ(ц.Ј)+" ";}return s;}String Δι(Ǭ ц){if(ц.Ќ.ToUpper()=="ORE"||ц.Ќ.ToUpper()=="INGOT"){String ΒІ=Δη(ц.ζ.ToUpper());if(ΒІ!="") return ΒІ;}return ц.ζ;}String ȕ(ί Γ,String Β){if(Γ.Ʀ()) return "No signal...("+Ɲ((int)(DateTime.Now-Γ.ΑΤ).TotalSeconds)+")";
return Β;}String ΒΚ(bool ΖЍ){String Β="";if(ΖЍ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΕΒ(IMyTerminalBlock Џ,out ϊ λ,out String Э){bool Ȫ=true;String Ĳ=Ō(ņ(Џ.CustomData.Split('\n'),"mode",ref Ȫ)).ToUpper();String[]θ=Ĳ.Split(':');String ı=θ.Length>0?θ[0].Trim():"";Э=θ.Length>1?θ[1].Trim():"";
λ=ϊ.Ǚ;if(ı=="MAIN") λ=ϊ.Βέ;else if(ı=="MAINX") λ=ϊ.Βή;else if(ı=="MENU") λ=ϊ.Ʃ;else if(ı=="INVENTORY") λ=ϊ.Ƣ;else if(ı=="DEBUG") λ=ϊ.ƕ;if(λ==ϊ.Ǚ) Э=Ĳ;}String ΑЌ(ϊ λ,String Э,bool ΕЍ){int į=15;if(ΕЍ){ΒЉ=ş;ş=0;ΒЊ=Š;Š=0;}String Β="";if(λ==ϊ.Βέ){int Ў=0;
Β=Ȏ(false,ΒЉ,ref ΕЄ,true,į);if(!int.TryParse(Э,out Ў)) return Β;ş=Math.Max(Ў,ş);Ў--;return Βι(Ў,į,Β);}else if(λ==ϊ.Βή){int Ў=0;Β=Ȏ(false,ΒЊ,ref ΕЅ,false,į);if(!int.TryParse(Э,out Ў)) return Β;Š=Math.Max(Ў,Š);Ў--;return Βι(Ў,į,Β);}if(λ==ϊ.Βά){return Ȏ(false,0,ref ΕІ,true,į);
}else if(λ==ϊ.Ƣ||λ==ϊ.Ʃ||λ==ϊ.ƕ){ί Γ=σ;if(Э!=""){Γ=ƙ("",Э);if(Γ==null) return "Unknown ship: "+Э;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ā="—————————————————————\n";String Αύ="";String ǆ="";if(λ==ϊ.Ʃ){Αύ="Menu";ǆ=ȕ(Γ,Γ.ź);}
else if(λ==ϊ.Ƣ){Αύ="Inventory";ǆ=ȕ(Γ,ΒΜ(Γ));}else if(λ==ϊ.ƕ){return ȕ(Γ,Δξ(Γ));}Β+=Γ.ζ+" - "+Γ.ΑΑ+"m | "+ƒ(""+Αύ)+"\n"+Ā;Β+=ǆ;return Β;}return "Unknown command: "+Э;}string Δξ(ί Γ){String s="";s+=Γ.Ɖ+"\n";s+=Γ.ζ+"\n";s+=Γ.Π.X+"\n";s+=Γ.Π.Y+"\n";
s+=Γ.Π.Z+"\n";s+=Γ.Αχ+"\n";s+=Γ.Ϊ+"\n";s+=Γ.ς+"\n";s+=Γ.ź.Length+"\n";s+=Γ.Ǳ+"\n";s+=Γ.ǽ+"\n";s+=Γ.њ.Count()+"\n";return s;}string ΒΜ(ί Γ){String s="";s+=ư("All",С(Γ.Ǳ,Γ.ǽ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.њ.Count();i++){Ǭ ц=Γ.њ[i];
s+=ư(ц.ζ,ц.Ј,Γ.ΑϏ,30,8,10)+"\n";}return s;}ι Δα(ί Γ,bool ѕ,string μ){ι έ=ι.None;if(Γ==null){for(int i=0;i<β.Count;i++){ι Ѐ=β[i].Ǘ(μ,false,ѕ,false);if(Ѐ==ι.Č) έ=Ѐ;if(Ѐ==ι.ŋ) return Ѐ;if(Ѐ==ι.ȓ) return Ѐ;}return έ;}else return Γ.Ǘ(μ,false,ѕ,false);}String Ȑ(ί Γ,String μ)
{if(!Βο(Γ,μ)) return "";ι Ѐ=Δα(Γ,false,μ);if(Ѐ==ι.Č) return "received!";if(Ѐ==ι.ȓ) return "pending...";if(Ѐ==ι.ŋ) return "no answer!";return "";}String Ʋ(int Ć,String Β,int ȳ,ref int ѝ){String[]Ζ=Β.Split('\n');if(ȳ>=ѝ+Ć-1) ѝ++;ѝ=Math.Min(Ζ.Count()-1-Ć,ѝ);if(ȳ<ѝ+1) ѝ--;
ѝ=Math.Max(0,ѝ);String έ="";for(int i=0;i<Ć;i++){int Γΰ=i+ѝ;if(Γΰ>=Ζ.Count()) break;έ+=Ζ[Γΰ]+"\n";}return έ;}String Βι(int Ў,int ĳ,String Β){String[]Ζ=Β.Split('\n');int Αφ=Ў*ĳ;int ū=(Ў+1)*ĳ;String έ="";for(int i=Αφ;i<ū;i++){if(i>=Ζ.Count()) break;έ+=Ζ[i]+"\n";
}return έ;}String ƒ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Βψ(String s,int Γδ){if(s=="") return s;if(s.Length>Γδ) s=s.Substring(0,Γδ-1)+".";return s;}string ư(String ǻ,float value,float Ŷ,int Βύ,int Γά,int Ζα){float ȃ=С(value,Ŷ)*Βύ;
String Ś="[";for(int i=0;i<Βύ;i++){if(i<=ȃ) Ś+="|";else Ś+="'";}Ś+="]";return Ś+" "+Βψ(ƒ(ǻ),Γά).PadRight(Γά)+"".PadRight(Ζα)+ΒΔ(value);}String ΒΔ(float Ј){if(Ј>=1000000) return Math.Round(Ј/1000000f,Ј/1000000f<100?1:0)+"M";if(Ј>=1000) return Math.Round(Ј/1000f,Ј/1000f<100?1:0)+"K";
return ""+Math.Round(Ј);}String Ɲ(int Ĵ){if(Ĵ>=60*60) return Math.Round(Ĵ/(60f*60f),1)+" h";if(Ĵ>=60) return Math.Round(Ĵ/60f,1)+" min";return ""+Ĵ+" s";}List<IMyCameraBlock>Εώ=new List<IMyCameraBlock>();bool Ζγ=false;int ΕЀ=0,ΗΘ=0,ΗΖ=0;String Εό="";String Εύ="";
void ΔΗ(){Ζγ=false;ΕЀ=0;Εό="";}const String Εθ="[PAMTFC]";long ΖΫ=0;String з="";int Ȁ=-1;float ΔЄ(){return Ǻ>0?Ǻ:ǐ;}float ΔЃ(){return ǹ>0?ǹ:Ǐ;}List<int>ΑΝ=new List<int>();String ΓΠ="";void ΔΧ(){String И=к+"|"+й+"|"+(int) Ȣ;if(И==ΓΠ) return;ΓΠ=И;ΑЍ(к,й,Ȣ,ΑΝ);
}void ΑЍ(int w,int h,У Αφ,List<int>ǉ){ǉ.Clear();if(Αφ==У.Ŕ){for(int k=0;k<w*h;k++){int Ǧ=k/w;ǉ.Add(Ǧ*w+(Ǧ%2==0?k-Ǧ*w:w-1-(k-Ǧ*w)));}return;}int ΓΞ=(int) Math.Ceiling(w/2f),ΓΝ=(int) Math.Ceiling(h/2f);int ΖΖ=w/2,ΖΕ=h/2;int x=0,y=0,ΑΒ=0,ΑΓ=-1,Ŷ=Math.Max(w,h);
for(int i=0;i<Ŷ*Ŷ;i++){if(-ΓΞ<x&&x<=ΖΖ&&-ΓΝ<y&&y<=ΖΕ) ǉ.Add((y-1+ΓΝ)*w+(x-1+ΓΞ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΑΒ;ΑΒ=-ΑΓ;ΑΓ=t;}x+=ΑΒ;y+=ΑΓ;}}bool ΔΦ(){int n=Math.Max(0,к*й);if(з.Length==n) return false;з=new String('0',n);
Ȁ=-1;return true;}void ΕΨ(int Ж,bool κ){if(Ж<0||Ж>=з.Length) return;char[]h=з.ToCharArray();h[Ж]=κ?'1':'0';з=new String(h);}const String ΔЂ="[PAMJOB]";const String Εζ="[PAMTCC]";class Ȕ{public long Λ;public Vector3 Φ,М,Ѓ;}List<Ȕ>В=new List<Ȕ>();bool Ĝ=false;Vector3 Ũ,ŧ,ũ,Ū;
long ĩ=0;long Γν=0;ά Γμ=null;bool ΕЈ=false;ά ΔΣ(Vector3 Φ,Vector3 М,Vector3 Ѓ){MatrixD m=MatrixD.CreateWorld(Φ,М,Ѓ);ά w=new ά(Σ);w.І=Vector3D.TransformNormal(Ũ,m);w.Ё=Vector3D.TransformNormal(ŧ,m);w.О=Vector3D.TransformNormal(ũ,m);w.Π=Φ+(Vector3) Vector3D.TransformNormal(Ū,m);
w.ν=true;return w;}void ΔЅ(){if(Ρ==Ο.ο||!Σ.ν) return;IMyShipConnector ї=ȏ(MyShipConnectorStatus.Connected);if(ї==null||ї.OtherConnector==null) return;IMyShipConnector Й=ї.OtherConnector;Vector3I ΖΙ=new Vector3I((int) Σ.Ч.X,(int) Σ.Ч.Y,(int) Σ.Ч.Z);if(ї.Position==ΖΙ){MatrixD ű=MatrixD.Transpose(Й.WorldMatrix);
Ũ=Vector3D.TransformNormal(Њ.WorldMatrix.Forward,ű);ŧ=Vector3D.TransformNormal(Њ.WorldMatrix.Down,ű);ũ=Vector3D.TransformNormal(Њ.WorldMatrix.Left,ű);Ū=Vector3D.TransformNormal(ї.GetPosition()-Й.GetPosition(),ű);Ĝ=true;}if(Vector3.Distance(ї.GetPosition(),Σ.Π)<5) ĩ=Й.EntityId;List<IMyShipConnector>Ƕ=new List<IMyShipConnector>();
Н.GetBlocksOfType(Ƕ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(Ƕ.Count>0){В.Clear();for(int i=0;i<Ƕ.Count;i++){Ȕ p=new Ȕ();p.Λ=Ƕ[i].EntityId;p.Φ=Ƕ[i].GetPosition();p.М=Ƕ[i].WorldMatrix.Forward;p.Ѓ=Ƕ[i].WorldMatrix.Up;В.Add(p);}}Γμ=null;Γν=0;
if(Й.EntityId==ĩ||!Ĝ) return;for(int i=0;i<В.Count;i++)
if(В[i].Λ==Й.EntityId){Γν=В[i].Λ;Γμ=ΔΣ(В[i].Φ,В[i].М,В[i].Ѓ);}}String ΔΡ(){String s=(Ĝ?"1":"0")+":"+γ(Ũ)+":"+γ(ŧ)+":"+
γ(ũ)+":"+γ(Ū)+":"+ĩ;for(int i=0;i<В.Count;i++)
s+=":"+В[i].Λ+","+γ(В[i].Φ)+","+γ(В[i].М)+","+γ(В[i].Ѓ);return s;}void ΔΠ(String s){В.Clear();Ĝ=false;ĩ=0;String[]f=s.Split(':');if(f.Length<6) return;Ĝ=f[0]=="1";Ũ=Ϋ(f[1]);ŧ=Ϋ(f[2]);ũ=Ϋ(f[3]);Ū=Ϋ(f[4]);long.TryParse(f[5],out ĩ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ȕ p=new Ȕ();if(!long.TryParse(e[0],out p.Λ)) continue;p.Φ=Ϋ(e[1]);p.М=Ϋ(e[2]);p.Ѓ=Ϋ(e[3]);В.Add(p);}}const String Ɣ="[PAMDCK]";const float ΔΑ=120f;class ѐ{public long Γ;public int Ǡ;public long и;public bool ΒЂ;public DateTime Л,ȣ;
}IMyBroadcastListener Ŧ=null;List<ѐ>ό=new List<ѐ>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMySensorBlock>б=new List<IMySensorBlock>();List<IMyCameraBlock>ћ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ƍ=new List<MyDetectedEntityInfo>();long Б=0;int ŝ=0;
long Ŝ=0;DateTime ΒЃ=DateTime.Now;String љ="";int ΓΓ=0;IMyCubeGrid џ=null;void ΕΠ(){џ=Me.CubeGrid;Н.GetBlocksOfType(ў,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(dockPoolTag));Н.GetBlocksOfType(б,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(zoneTag));Н.GetBlocksOfType(ћ,q=>q.CubeGrid.IsSameConstructAs(џ)&&q.CustomName.Contains(laneTag));
for(int i=0;i<б.Count;i++){б[i].DetectSmallShips=true;б[i].DetectLargeShips=true;}for(int i=0;i<ћ.Count;i++) ћ[i].EnableRaycast=true;}IMyShipConnector ƹ(long Λ){if(Λ==0) return null;IMyShipConnector c=Н.GetBlockWithId(Λ) as IMyShipConnector;return c!=null&&џ!=null&&c.CubeGrid.IsSameConstructAs(џ)?c:null;
}bool Ƥ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΒΫ(IMyShipConnector c,long Γ){for(int i=0;i<ћ.Count;i++){IMyCameraBlock ę=ћ[i];if(!ę.IsWorking||Vector3D.Distance(ę.GetPosition(),c.GetPosition())>10) continue;
Vector3D ū=ę.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ę.CanScan(ū)) continue;MyDetectedEntityInfo Ŭ=ę.Raycast(ū);if(!Ŭ.IsEmpty()&&Ŭ.EntityId!=џ.EntityId&&Ŭ.EntityId!=Γ&&Ŭ.Type!=MyDetectedEntityType.FloatingObject){љ="Lane blocked: "+c.CustomName;return false;}
}return true;}bool ΔЀ(){for(int s=0;s<б.Count;s++){if(!б[s].IsWorking) continue;ƍ.Clear();б[s].DetectedEntities(ƍ);for(int i=0;i<ƍ.Count;i++){MyDetectedEntityInfo e=ƍ[i];bool ΖΒ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;if(ΖΒ&&e.EntityId!=Б&&e.Velocity.Length()>0.5f)
{љ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΕΓ(ѐ r){IMyShipConnector и=ƹ(r.и);if(Ƥ(и)&&ΒΫ(и,r.Γ)) return и.EntityId;if(!r.ΒЂ||и==null) return 0;IMyShipConnector Ѝ=null;double ΒЀ=0;for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];if(!Ƥ(c)) continue;
double d=Vector3D.Distance(c.GetPosition(),и.GetPosition());if(Ѝ==null||d<ΒЀ){if(ΒΫ(c,r.Γ)){Ѝ=c;ΒЀ=d;}}}return Ѝ==null?0:Ѝ.EntityId;}void Δς(){ѐ ȫ=null;long ї=0;for(int ſ=2;ſ>=1&&ȫ==null;ſ--){ѐ ȁ=null;for(int i=0;i<ό.Count;i++){ѐ r=ό[i];if(r.Ǡ!=ſ) continue;
if(ſ==1){long c=ΕΓ(r);if(c==0) continue;if(ȁ==null||r.Л<ȁ.Л){ȁ=r;ї=c;}}else if(ȁ==null||r.Л<ȁ.Л) ȁ=r;}ȫ=ȁ;}if(ȫ==null){if(ό.Count>0&&љ=="") љ="No free dock";return;}Б=ȫ.Γ;ŝ=ȫ.Ǡ;Ŝ=ȫ.Ǡ==1?ї:0;ΒЃ=DateTime.Now;}void Εν(){if(џ==null) return;
if(Ŧ==null) Ŧ=IGC.RegisterBroadcastListener(Ɣ);int Ę=30;while(Ŧ.HasPendingMessage&&Ę-->0){String Ξ=Ŧ.AcceptMessage().Data as String;if(Ξ!=null) Δτ(Ξ);}ΕΝ();if(++ΓΓ<3) return;ΓΓ=0;IGC.SendBroadcastMessage(Ɣ,ΓА());}void Δτ(String Ξ){try{String[]f=Ξ.Split(';');long Γ=long.Parse(f[1]);
if(f[0]=="D"){for(int i=ό.Count-1;i>=0;i--) if(ό[i].Γ==Γ) ό.RemoveAt(i);if(Б==Γ) Б=0;}else if(f[0]=="R"&&f.Length>=5){long и=long.Parse(f[3]);if(ƹ(и)==null) return;ѐ r=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Γ) r=ό[i];if(r==null){r=new ѐ();r.Γ=Γ;r.Л=DateTime.Now;ό.Add(r);}
int Ǡ=int.Parse(f[2]);if(r.Ǡ!=Ǡ) r.Л=DateTime.Now;r.Ǡ=Ǡ;r.и=и;r.ΒЂ=f[4]=="1";r.ȣ=DateTime.Now;}}catch{}}void ΕΝ(){for(int i=ό.Count-1;i>=0;i--)
if((DateTime.Now-ό[i].ȣ).TotalSeconds>3) ό.RemoveAt(i);ѐ ś=null;for(int i=0;i<ό.Count;i++) if(ό[i].Γ==Б) ś=ό[i];if(Б!=0&&ś==null) Б=0;if(ś!=null&&(DateTime.Now-ΒЃ).TotalSeconds>ΔΑ){Б=0;ś.Л=DateTime.Now;}љ="";if(Б==0&&ό.Count>0&&!ΔЀ()) Δς();}String ΓА()
{String η="S;"+џ.EntityId+";"+Б+";"+ŝ+";"+Ŝ+";";for(int i=0;i<ό.Count;i++) η+=(i>0?",":"")+ό[i].Γ;η+=";";for(int i=0;i<ў.Count;i++){IMyShipConnector c=ў[i];η+=(i>0?":":"")+c.EntityId+","+γ(c.GetPosition())+","+γ(c.WorldMatrix.Forward)+","+γ(c.WorldMatrix.Up);
}η+=";";for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();η+=(i>0?":":"")+γ(z.GetPosition())+","+Math.Round(r,1);}return η;}String Δε()
{if(ў.Count==0&&б.Count==0) return "";int ΓΘ=0;for(int i=0;i<ў.Count;i++) if(Ƥ(ў[i])) ΓΘ++;return "Docks: "+ΓΘ+"/"+ў.Count+" free"+(Б!=0?(ŝ==1?" | docking":" | undocking"):"")+
(ό.Count>1?" | "+(ό.Count-1)+" waiting":"")+(љ!=""?"\n"+љ:"")+"\n";}int ΓΒ=0;bool ΖЇ=false;void ΕΗ(){if(ΓΒ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ɣ,"D;"+ΖΫ);ΓΒ=0;ΖЇ=false;}const float ΔЁ=600f;class р{public long Λ;public int Ж=-1;public bool ΓІ;public float ΑΗ,ΑΖ;
public DateTime Αν;}class г{public String И;public int w,h;public float ΑЀ,Αϋ;public String κ;public List<int>ž=new List<int>();public List<р>Ћ=new List<р>();}List<г>З=new List<г>();IMyBroadcastListener ŵ=null;int ΓΫ=0;void Εξ(){if(ŵ==null) ŵ=IGC.RegisterBroadcastListener(Εθ);int Ę=50;
while(ŵ.HasPendingMessage&&Ę-->0){String Ξ=ŵ.AcceptMessage().Data as String;if(Ξ==null) continue;try{Δφ(Ξ);}catch{}try{Δχ(Ξ);}catch{}}for(int j=З.Count-1;j>=0;j--){г Θ=З[j];for(int i=Θ.Ћ.Count-1;i>=0;i--)
if((DateTime.Now-Θ.Ћ[i].Αν).TotalSeconds>ΔЁ) Θ.Ћ.RemoveAt(i);if(Θ.Ћ.Count==0){З.RemoveAt(j);continue;}ΓЋ(Θ);}if(++ΓΫ<3) return;ΓΫ=0;for(int j=0;j<З.Count;j++) IGC.SendBroadcastMessage(ΔЂ,ΓЍ(З[j]));}void ΓЋ(г Θ){for(int i=0;i<Θ.Ћ.Count;i++){р s=Θ.Ћ[i];if(s.ΓІ&&(s.Ж<0||Θ.κ[s.Ж]=='1')) s.Ж=ΔЈ(Θ,s);
}}String ΓЍ(г Θ){String η="J;"+Θ.И+";"+Θ.κ+";";bool Л=true;for(int i=0;i<Θ.Ћ.Count;i++){if((DateTime.Now-Θ.Ћ[i].Αν).TotalSeconds>5) continue;η+=(Л?"":",")+Θ.Ћ[i].Λ+":"+Θ.Ћ[i].Ж;Л=false;}return η;}void Δφ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;г Θ=null;for(int i=0;i<З.Count;i++) if(З[i].И==f[4]) Θ=З[i];if(Θ==null){Θ=new г();Θ.И=f[4];Θ.w=int.Parse(k[3]);Θ.h=int.Parse(k[4]);Θ.ΑЀ=float.Parse(k[6]);Θ.Αϋ=float.Parse(k[7]);if(Θ.w<1||Θ.h<1) return;ΑЍ(Θ.w,Θ.h,(У) int.Parse(k[5]),Θ.ž);
Θ.κ=new String('0',Θ.w*Θ.h);З.Add(Θ);}String d=f[6];if(d.Length==Θ.κ.Length){char[]c=Θ.κ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Θ.κ=new String(c);}long Λ=long.Parse(f[0]);р s=null;for(int i=0;i<Θ.Ћ.Count;i++) if(Θ.Ћ[i].Λ==Λ) s=Θ.Ћ[i];
if(s==null){s=new р();s.Λ=Λ;Θ.Ћ.Add(s);}s.Αν=DateTime.Now;String[]w=f[10].Split(',');s.ΓІ=w[0]=="1";if(w.Length>=3){s.ΑΗ=float.Parse(w[1]);s.ΑΖ=float.Parse(w[2]);}int Ĩ=int.Parse(f[5]);if(Ĩ>=0&&Ĩ<Θ.κ.Length&&(s.Ж<0||Θ.κ[s.Ж]=='1')&&Θ.κ[Ĩ]!='1'&&Ơ(Θ,Ĩ,s)==null)
s.Ж=Ĩ;}р Ơ(г Θ,int Ж,р ΕЌ){for(int i=0;i<Θ.Ћ.Count;i++){р o=Θ.Ћ[i];if(o!=ΕЌ&&o.Ж==Ж)
return o;}return null;}int ΔЈ(г Θ,р s){for(int k=0;k<Θ.ž.Count;k++){int c=Θ.ž[k];if(Θ.κ[c]!='1'&&Ơ(Θ,c,s)==null) return c;}return-2;}String Δλ(){String s="";for(int j=0;j<З.Count;j++){г Θ=З[j];int κ=0;for(int i=0;i<Θ.κ.Length;i++) if(Θ.κ[i]=='1') κ++;s+="Job "+(j+1)+": "+κ+"/"+Θ.κ.Length+" holes, "+Θ.Ћ.Count+" ships\n";
}return s;}const float Εε=8f;class ϋ{public long Λ,Ȃ;public Vector3 Φ,ю,Ħ;public float Ȝ,ΑΗ,ΑΖ,л=-1;public int Ǹ;public int Ǥ;public String Θ="";public int Αδ;public DateTime ȇ;}class œ{public long a,b,ΑЉ;public DateTime ȣ;}List<ϋ>Χ=new List<ϋ>();List<œ>Г=new List<œ>();int Αϊ=0;
void Δχ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<9) return;long Λ=long.Parse(f[0]);ϋ s=null;for(int i=0;i<Χ.Count;i++) if(Χ[i].Λ==Λ) s=Χ[i];if(s==null){s=new ϋ();s.Λ=Λ;Χ.Add(s);}s.Φ=Ϋ(f[1]);s.ю=Ϋ(f[2]);s.Ȝ=float.Parse(f[3]);s.Θ=f[4];s.Αδ=int.Parse(f[7].Split(',')[1]);
s.Ħ=Ϋ(f[8]);s.Ǹ=0;s.Ǥ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΗ=float.Parse(w[1]);s.ΑΖ=float.Parse(w[2]);s.Ǹ=int.Parse(w[3]);if(w.Length>=5) s.Ǥ=int.Parse(w[4]);}}s.Ȃ=f.Length>=12?long.Parse(f[11]):0;s.ȇ=DateTime.Now;}bool ď(ϋ s){return s.ю.LengthSquared()<0.25f&&(s.Αδ==3||s.Ħ.LengthSquared()<0.25f);}
Vector3 Βυ(ϋ s){return s.ю.LengthSquared()>=1f?s.ю:s.Ħ*2f;}void Ερ(){ΔΙ();if(++Αϊ<3) return;Αϊ=0;IGC.SendBroadcastMessage(Εζ,ΓБ());}void ΔΙ(){for(int i=Χ.Count-1;i>=0;i--)
if((DateTime.Now-Χ[i].ȇ).TotalSeconds>3) Χ.RemoveAt(i);for(int i=Г.Count-1;i>=0;i--)
if((DateTime.Now-Г[i].ȣ).TotalSeconds>2) Г.RemoveAt(i);for(int i=0;i<Χ.Count;i++) Χ[i].л=-1;for(int i=0;i<Χ.Count;i++)
for(int j=i+1;j<Χ.Count;j++){ϋ a=Χ[i],b=Χ[j];if(ď(a)||ď(b)) continue;Vector3 Ζΰ=a.Φ+a.ю*(float)(DateTime.Now-a.ȇ).TotalSeconds;Vector3 Ȃ=b.Φ+b.ю*(float)(DateTime.Now-b.ȇ).TotalSeconds;Vector3 Ą=Βυ(a),ą=Βυ(b);Vector3 Ȯ=Ȃ-Ζΰ;float у=Ȯ.Length();float ѕ=(a.Ȝ+b.Ȝ)*0.55f;
bool Γφ=a.Θ!=""&&a.Θ==b.Θ;if(Γφ&&(a.Ǹ==2||b.Ǹ==2)) continue;if(Γφ&&a.Ǹ==1&&b.Ǹ==1){String[]k=a.Θ.Split('|');if(k.Length>=8) ѕ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Ακ=ą-Ą;float Γυ=Ακ.LengthSquared();float t=Γυ>0.01f?-Vector3.Dot(Ȯ,Ακ)/Γυ:0;
bool Αώ=у<ѕ;if(!Αώ&&(t<=0||t>Εε||(Ȯ+Ακ*t).Length()>=ѕ)) continue;if(!Αώ&&a.Ǥ==b.Ǥ&&Ą.LengthSquared()>0.25f&&ą.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ą),Vector3.Normalize(ą))<-0.7f)
continue;œ p=null;for(int k=0;k<Г.Count;k++)
if((Г[k].a==a.Λ&&Г[k].b==b.Λ)||(Г[k].a==b.Λ&&Г[k].b==a.Λ)) p=Г[k];if(p==null){p=new œ();p.a=a.Λ;p.b=b.Λ;p.ΑЉ=Εη(a,b,Ą,ą,Ȯ,у);Г.Add(p);}p.ȣ=DateTime.Now;ϋ y=p.ΑЉ==a.Λ?a:b;float Γί=Αώ||t<4?0:Math.Max(1,y.ю.Length()*0.5f);if(y.л<0||Γί<y.л) y.л=Γί;
}Δψ();}long Εη(ϋ a,ϋ b,Vector3 Ą,Vector3 ą,Vector3 Ȯ,float у){if(a.Λ==Б) return b.Λ;if(b.Λ==Б) return a.Λ;if(a.Ǥ!=b.Ǥ) return a.Ǥ<b.Ǥ?a.Λ:b.Λ;Vector3 n=у>0.01f?Ȯ/у:new Vector3(1,0,0);float Γό=Vector3.Dot(Ą,n),ΓϏ=Vector3.Dot(ą,-n);if(Γό-ΓϏ>1) return a.Λ;if(ΓϏ-Γό>1) return b.Λ;
return a.Λ>b.Λ?a.Λ:b.Λ;}String ΓБ(){String η="C;";bool Л=true;for(int i=0;i<Χ.Count;i++){if(Χ[i].л<0) continue;η+=(Л?"":",")+Χ[i].Λ+":"+Math.Round(Χ[i].л,1);Л=false;}return η;}String Δρ(){int Γθ=0,ΑΜ=0;for(int i=0;i<Χ.Count;i++){if(!ď(Χ[i])) Γθ++;if(Χ[i].л>=0) ΑΜ++;
}if(Χ.Count==0) return "";return "Traffic: "+Γθ+" moving"+(ΑΜ>0?", "+ΑΜ+" giving way":"")+"\n";}List<ί>Щ=new List<ί>();ί х=null;Vector3 ΓΦ;DateTime ΓΥ;float ΓΤ=40;bool ΓΡ=false;ϋ ƚ(ί s){ϋ Ѝ=null;for(int i=0;i<Χ.Count;i++) if(Χ[i].Ȃ!=0&&""+Χ[i].Ȃ==s.Λ) return Χ[i];
for(int i=0;i<Χ.Count;i++)
if(Vector3.Distance(Χ[i].Φ,s.Π)<15&&(Ѝ==null||
Vector3.Distance(Χ[i].Φ,s.Π)<Vector3.Distance(Ѝ.Φ,s.Π))) Ѝ=Χ[i];return Ѝ;}float Δω(ί s){ϋ t=ƚ(s);if(t==null) return 0;float k=t.Ǹ*1000000f;String[]И=t.Θ.Split('|');if(И.Length>=3) k+=Vector3.Distance(t.Φ,new Vector3(float.Parse(И[0]),float.Parse(И[1]),float.Parse(И[2])));return k;
}void Εί(){Щ.Clear();х=null;ΑΞ.Clear();List<float>ΑΡ=new List<float>();for(int i=0;i<β.Count;i++){float k=Δω(β[i]);int ǰ=0;while(ǰ<ΑΡ.Count&&ΑΡ[ǰ]<=k) ǰ++;ΑΡ.Insert(ǰ,k);Щ.Insert(ǰ,β[i]);β[i].Ǘ("HOMEPOS",true,false,false);Ƴ("HOMEPOS",β[i].Λ,"HOMEPOS");}}List<ϋ>ΑΞ=new List<ϋ>();
void Δψ(){ΑΞ.Clear();for(int i=0;i<Щ.Count;i++){ϋ t=ƚ(Щ[i]);if(t==null) continue;t.л=0;ΑΞ.Add(t);}}void Επ(){if(х!=null){bool ΖΑ=ΓΡ||Vector3.Distance(х.Π,ΓΦ)>ΓΤ||
(DateTime.Now-ΓΥ).TotalSeconds>45||х.Ʀ();if(!ΖΑ) return;х=null;}if(Щ.Count==0) return;х=Щ[0];Щ.RemoveAt(0);ΓΦ=х.Π;ΓΥ=DateTime.Now;ϋ t=ƚ(х);ΓΤ=t==null?40:Math.Max(40,t.Ȝ*3);ΓΡ=t!=null&&t.Αδ==3&&t.ю.LengthSquared()<0.25f;}const String ΔΞ="[PAMDBG]";class Ɩ{public readonly bool Ƭ;
IMyProgrammableBlock ѓ;Action<IMyProgrammableBlock>ǀ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>ƿ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>ƾ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>ǁ;Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>ƽ;
public Ɩ(MyGridProgram Ζθ){ѓ=Ζθ.Me;var ǿ=ѓ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ѓ);if(ǿ==null) return;ǀ=(Action<IMyProgrammableBlock>) ǿ["RemoveDraw"];ƿ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ǿ["Point"];ƾ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ǿ["Line"];
ǁ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ǿ["Sphere"];ƽ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) ǿ["GPS"];((Action<IMyProgrammableBlock>) ǿ["RemoveAll"])(ѓ);Ƭ=true;}public void ΕΘ(){if(ǀ!=null) ǀ(ѓ);}public void ǖ(Vector3D a,Vector3D b,Color c,float Ζό){if(ƾ!=null) ƾ(ѓ,a,b,c,Ζό,-1,false);}
public void ΕΔ(Vector3D a,Color c,float ы){if(ƿ!=null) ƿ(ѓ,a,c,ы,-1,false);}public void Ȓ(String Β,Vector3D ǰ,Color c){if(ƽ!=null) ƽ(ѓ,Β,ǰ,c,-1);}public void Βς(Vector3D ș,double ы,Color c){if(ǁ!=null) ǁ(ѓ,new BoundingSphereD(ș,ы),c,1,0.05f,30,-1,false);}}class Ĉ
{public long Λ;public Vector3 ĭ,ΑΦ,ĝ,ΰ;public bool ΓΟ;public List<Vector3>Ɓ=new List<Vector3>(),Αζ=new List<Vector3>();public List<bool>Αε=new List<bool>();public DateTime ȇ;}Ɩ υ=null;IMyBroadcastListener Ť=null;List<Ĉ>т=new List<Ĉ>();void Δσ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<6) return;
long Λ=long.Parse(f[0]);Ĉ s=null;for(int i=0;i<т.Count;i++) if(т[i].Λ==Λ) s=т[i];if(s==null){s=new Ĉ();s.Λ=Λ;т.Add(s);}s.ĭ=Ϋ(f[1]);s.ΑΦ=Ϋ(f[2]);s.ĝ=Ϋ(f[3]);s.ΓΟ=s.ΑΦ.LengthSquared()>0.5f;s.ΰ=Ϋ(f[4]);s.ȇ=DateTime.Now;if(f[5]=="") return;s.Ɓ.Clear();s.Αζ.Clear();s.Αε.Clear();
String[]Γρ=f[5].Split(':');for(int i=0;i<Γρ.Length;i++){String[]r=Γρ[i].Split(',');if(r.Length<3) continue;s.Ɓ.Add(Ϋ(r[0]));s.Αζ.Add(Ϋ(r[1]));s.Αε.Add(r[2]=="1");}}void Εμ(){if(!debugDraw) return;if(υ==null) υ=new Ɩ(this);if(!υ.Ƭ) return;if(Ť==null) Ť=IGC.RegisterBroadcastListener(ΔΞ);
int Ę=20;while(Ť.HasPendingMessage&&Ę-->0){String Ξ=Ť.AcceptMessage().Data as String;if(Ξ!=null) try{Δσ(Ξ);}catch{}}for(int i=т.Count-1;i>=0;i--) if((DateTime.Now-т[i].ȇ).TotalSeconds>5) т.RemoveAt(i);υ.ΕΘ();for(int i=0;i<Χ.Count;i++){ϋ s=Χ[i];Color c=ď(s)?Color.Gray:s.л>=0?Color.Yellow:Color.Lime;
υ.Βς(s.Φ,s.Ȝ/2,c);υ.Ȓ(Œ(s)+": "+(ď(s)?"parked":s.л==0?"holding (giving way)":s.л>0?"slowing (giving way)":"moving"),s.Φ,c);if(s.ю.LengthSquared()>0.25f) υ.ǖ(s.Φ,s.Φ+s.ю*3,Color.White,0.1f);if(s.Ħ.LengthSquared()>0.25f) υ.ǖ(s.Φ,s.Φ+s.Ħ*s.Ȝ,Color.Cyan,0.1f);
}for(int i=0;i<Г.Count;i++){if((DateTime.Now-Г[i].ȣ).TotalSeconds>0.5) continue;ϋ a=null,b=null;for(int k=0;k<Χ.Count;k++){if(Χ[k].Λ==Г[i].a) a=Χ[k];if(Χ[k].Λ==Г[i].b) b=Χ[k];}if(a==null||b==null) continue;υ.ǖ(a.Φ,b.Φ,Color.Red,0.2f);υ.Ȓ("Conflict: "+Œ(Г[i].ΑЉ==a.Λ?a:b)+" gives way",(a.Φ+b.Φ)/2,Color.Red);
}for(int i=0;i<т.Count;i++){Ĉ d=т[i];ϋ t=null;for(int k=0;k<Χ.Count;k++) if(Χ[k].Λ==d.Λ) t=Χ[k];if(t!=null) υ.ǖ(t.Φ,d.ΰ,Color.Blue,0.08f);for(int r=0;r<d.Ɓ.Count;r++) υ.ǖ(d.Ɓ[r],d.Αζ[r],d.Αε[r]?Color.Red:Color.Green,0.04f);if(t==null||!d.ΓΟ) continue;г Θ=null;
for(int j=0;j<З.Count;j++) if(З[j].И==t.Θ) Θ=З[j];if(Θ==null) continue;int ΓΔ=0;for(int c=0;c<Θ.κ.Length;c++) if(Θ.κ[c]=='1') ΓΔ++;if(i==0||т[i-1].ĭ!=d.ĭ) υ.Ȓ("Job: "+ΓΔ+"/"+Θ.κ.Length+" holes done",d.ĭ,Color.White);for(int c=0;c<Θ.κ.Length;c++){int Ǌ=c%Θ.w,Ǧ=c/Θ.w;
Vector3 p=d.ĭ+d.ΑΦ*((Θ.w-1f)/2f-Ǌ)*Θ.ΑЀ-d.ĝ*((Θ.h-1f)/2f-Ǧ)*Θ.Αϋ;р Αβ=Ơ(Θ,c,null);υ.ΕΔ(p,Θ.κ[c]=='1'?Color.Gray:Αβ!=null?Color.Orange:Color.White,Math.Min(Θ.ΑЀ,Θ.Αϋ)*0.2f);if(Αβ==null||Αβ.Λ!=d.Λ) continue;υ.ǖ(t.Φ,p,Color.Orange,0.05f);υ.Ȓ("Hole "+(Θ.ž.IndexOf(c)+1)+": "+Œ(t),p,Color.Orange);
}}for(int i=0;i<б.Count;i++){IMySensorBlock z=б[i];υ.Βς(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);υ.Ȓ("Docking area"+(љ!=""?": "+љ:""),z.GetPosition(),Color.Cyan);
}for(int i=0;i<ћ.Count;i++)
υ.ǖ(ћ[i].GetPosition(),ћ[i].GetPosition()+ћ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(Б!=0&&Ŝ!=0){IMyShipConnector c=ƹ(Ŝ);for(int k=0;k<Χ.Count;k++)
if(c!=null&&Χ[k].Λ==Б){υ.ǖ(Χ[k].Φ,c.GetPosition(),Color.Lime,0.15f);υ.Ȓ("Cleared to "+(ŝ==2?"undock: ":"dock: ")+Œ(Χ[k]),c.GetPosition(),Color.Lime);}}}String Œ(ϋ t){ί Ѝ=null;for(int i=0;i<β.Count;i++) if(t.Ȃ!=0&&β[i].Λ==""+t.Ȃ) return β[i].ζ;for(int i=0;i<β.Count;i++)
if(Vector3.Distance(β[i].Π,t.Φ)<30&&(Ѝ==null||
Vector3.Distance(β[i].Π,t.Φ)<Vector3.Distance(Ѝ.Π,t.Φ))) Ѝ=β[i];if(Ѝ!=null&&Ѝ.ζ!="") return Ѝ.ζ;String Λ=t.Λ.ToString();return "Ship "+Λ.Substring(Math.Max(0,Λ.Length-4));}String Δγ(){if(!debugDraw) return "";if(υ==null||!υ.Ƭ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}void Εά(string А){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}