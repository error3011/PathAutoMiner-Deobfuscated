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
const float drillRadius = 1.4f;//[m]

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
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size
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

// ===== End of settings: don't edit below this line =====
public enum Μ{ф,Œ,Ǡ,Ћ,ζ}public enum Λ{є,Ň,ǅ,ř,ΑΔ,Ȃ,т,
ņ,Βγ,ǜ,Ƥ,ΒΕ,ƞ,
ΒЌ,ȉ,Ȉ,Ł,ΓΦ,Βή,
ΓΤ,ΓΥ,Βί,Βΰ,Ȋ}public enum Ο{в,ŕ,ѓ,ƥ,Ă,ǆ,Ǉ,Ȳ,ȳ}public enum о{None,Ł,є,ΒΦ}public enum Ι{Main,ΑΒ,Ȅ,ȃ,Š,ā,
Ľ,ƹ,ƺ,Ƹ,ȇ,ΑΗ,
Ċ,ś,Ş,ΑΙ,Ď}public enum у{ΑΕ,ΒΡ,ΓΙ,Βχ,ζ}public enum д{š,ΒΝ}public enum с{ΑΓ,Ββ,ΓΛ,Ŋ}public enum Р{ǉ,ΒΥ,ΒΤ,Βά,ΒΫ,Βυ,Βτ}public enum а{Ń,ΒΘ,Ɵ}public enum Ć{Βσ,Δτ,ΒΖ}public enum ύ{Ǌ,ǋ,ǌ,ΒЄ,ΒЃ,
Βώ,ΒϏ,Βύ,ΒЁ,
ΒЂ,ΒЀ}public enum Π{Ц,г,Х,Ā,ľ,Ŀ}public enum Δφ{Δσ,Δξ,Δρ}public enum Ч{ΑΛ,Ʀ,Βό,ǈ}public enum ρ{None,ȅ,Ř,œ}public enum λ{ţ,ļ,ŏ,Ŋ,ǃ,ő,Ǆ,ΓΠ,Ŕ}public enum ς{ф,ǁ,ǀ,Ő,ŋ,ł,ǂ}class Χ{public bool β=false;public Vector3 Ρ=new Vector3();public Vector3 ω=new Vector3();
public Vector3 ώ=new Vector3();public Vector3 Џ=new Vector3();public Vector3 η=new Vector3();public Vector3 З=new Vector3();public float ΓЈ=0;public float Εθ=0;public float[]П=null;public Χ(){}public Χ(Χ Ќ){β=Ќ.β;Ρ=Ќ.Ρ;ω=Ќ.ω;ώ=Ќ.ώ;Џ=Ќ.Џ;η=Ќ.η;З=Ќ.З;П=Ќ.П;}public Χ(Vector3 Ρ,Vector3 ώ,Vector3 ω,Vector3 Џ,Vector3 η)
{this.Ρ=Ρ;this.ω=ω;this.ώ=ώ;this.Џ=Џ;this.ΓЈ=0;this.η=η;}public void Δπ(List<IMyThrust>Ē,List<string>н){П=new float[н.Count];for(int i=0;i<П.Length;i++)
П[i]=-1;for(int i=0;i<Ē.Count;i++){string ǘ=Βρ(Ē[i]);int Α=н.IndexOf(ǘ);if(Α!=-1)
П[Α]=В(Ē[i].MaxEffectiveThrust,Ē[i].MaxThrust);}}}class ΒЇ{public Χ α=null;public List<Vector3>Εέ=new List<Vector3>();public float ΔЋ=0;public float Εμ=0;public float Δύ=0;public float ΔЏ=0;public Vector3 ΔА=new Vector3();}class ď{public ύ А=ύ.Ǌ;public float Т=0;public float ћ=0;
public string Ш="";public string Ъ="";DateTime Αυ;public bool ΔΒ=false;private bool ū=false;public bool ΒΞ(bool Ɖ){if(Ɖ){ћ=0;ū=false;return false;}ū=true;return ћ>Т;}public void Δυ(){ΒΞ(true);ΔΒ=false;}public void Δω(){if(ū)
if((DateTime.Now-Αυ).TotalSeconds>1){ћ++;Αυ=DateTime.Now;}}public bool ƶ(){switch(А){case ύ.ǋ:return true;case ύ.ǌ:return true;}return false;}}class б{public String γ="";public String Ѓ="";public int Ѐ=0;public Ć ż=Ć.ΒΖ;public б(String γ,String Ѓ,int Ѐ,Ć ż){this.γ=γ;this.Ѓ=Ѓ;
this.Ѐ=Ѐ;this.ż=ż;}}class ΒΧ{public ΒΧ(Vector3 δ,float ч){this.δ=δ;this.ч=ч;}public Vector3 δ;public float ч;}class ο{public DateTime Ź;public DateTime ΑΪ;public String Ё="";public String γ="";public String ĸ="";public String Φ="";public String Ɣ="";public String Ĭ="";public String Ƈ="";
public Vector3 Ρ=new Vector3();public List<б>й=new List<б>();public Μ Ν=Μ.ф;public ρ ц;public float ǐ;public float ј;public int ι;public int ŭ=0;public int ƙ=0;public bool ƿ(){return(DateTime.Now-Ź).TotalSeconds>10;}public bool Ō(){if(ц!=ρ.ȅ) return false;return(DateTime.Now-ΑΪ).TotalSeconds>=4;
}public ο(String Ё){this.Ё=Ё;}public ρ Ǣ(String χ,bool ΔΞ,bool Ȑ,bool Γί){if(χ==""&&!Ȑ) return ρ.None;if(ц==ρ.ȅ&&Ō())
ц=ρ.œ;if(ΔΞ){Ƈ=χ;ц=ρ.ȅ;ΑΪ=DateTime.Now;return ц;}else if(Ȑ){Ƈ="";ц=ρ.None;}else if(Ƈ==χ){if(Γί) ц=ρ.Ř;return ц;}return ρ.None;}}public String Βι(String Γώ){switch(Γώ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem и;Vector3 ΑІ=new Vector3();Vector3 У=new Vector3();Vector3 Γς=new Vector3();Vector3 ΓЂ=new Vector3();Vector3 Γλ=new Vector3();DateTime Ατ=new DateTime();
bool ș=true;int Ɨ=0;bool ȟ=false;bool ŷ=false;bool ΓЋ=true;bool Εο=false;bool ΕΒ=false;bool Γϊ=false;float ΔΠ=0;float ΑЋ=0;int ΔΘ=0;int Ει=0;int Ĳ=0;int ΔЃ=0;float Ǳ=0;float Ǧ=0;double Ɓ=0;float Ȍ=0;List<int>Ǯ=new List<int>();List<int>ǹ=new List<int>();Μ Ν=Μ.ф;
int ƍ=0;float Ǘ=0;float ѐ=0;float ǽ=0;IMyRemoteControl Љ;IMySensorBlock τ;List<IMyTimerBlock>ΑА=new List<IMyTimerBlock>();List<IMyShipConnector>Ȓ=new List<IMyShipConnector>();List<IMyThrust>ȫ=new List<IMyThrust>();List<IMyGyro>Αν=new List<IMyGyro>();List<IMyTerminalBlock>Ф=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ÿ=new List<IMyLandingGear>();List<IMyReactor>ƈ=new List<IMyReactor>();List<IMyConveyorSorter>ΔΤ=new List<IMyConveyorSorter>();List<IMyGasTank>ȝ=new List<IMyGasTank>();List<IMyTerminalBlock>ȋ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ΔІ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Δό=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΑΦ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ђ=new List<IMyTextPanel>();List<IMyTextSurface>Ю=new List<IMyTextSurface>();List<IMyTextPanel>њ=new List<IMyTextPanel>();IMyTerminalBlock ю=null;Χ Ξ=new Χ();Χ Αξ=new Χ();
Χ Ας=new Χ();bool ђ=false;bool Ȯ=false;bool ȯ=false;bool Απ=false;List<String>н=new List<string>();List<Χ>π=new List<Χ>();List<Χ>Ĺ=new List<Χ>();int ΔЌ=0;double Δε=0;Χ Σ=new Χ();Vector3 Ȣ;Vector3 ǲ;Ο μ=Ο.в;д Ĩ=д.š;int Ǎ=0;int џ=0;double ħ=0;bool Αΰ=false;ď ν=new ď();
ď ξ=new ď();с э=с.ΑΓ;bool Ȱ=true;д Ǖ=д.š;int Ж=3;int Е=3;int С=30;а з=а.Ń;bool ǚ=true;bool Ȟ=true;bool ȗ=true;bool ǖ=false;Р Ȗ=Р.ǉ;bool ё=true;float Ы=90;float Ь=20;float Л=5;float Э=20;float х=0.70f;float Ļ=1.50f;float ĺ=2.50f;float ȱ=0f;float ǭ=0f;Λ ě;Ο ы;
Λ ȩ;ΒЇ ΕΠ=null;int ġ=0;int Ģ=0;int ж=0;int Ƞ=0;int Ƌ=30;int ΕΣ=0;bool ΔΦ=true;Vector3 ΕΛ;double Αε=0;bool ΑЈ=false;int Δζ=0;int ΕΨ=0;int Δώ=0;int ΕΟ=0;int ΕΝ=0;Vector3 ΕΡ=new Vector3();float ΕΘ=0;float ΕΩ=0;int Ǚ=0;Χ ĳ=null;float ΕΙ=0;float ΔϏ=0;float Αί=0;
float ƀ=0;bool ΕΓ=false;bool Εη=false;bool Ελ=false;bool ΔБ=false;bool ΔЁ=false;DateTime ΔЎ=new DateTime();Χ ΔЉ=null;Χ Γι=null;float ΑΧ=0;о ȍ;float Αο=0;float ΒΒ=0;String ΓЅ="";float Ż=0;float ј=0;float ǐ=0;List<б>ΕΔ=new List<б>();float ϊ=0;bool Ə=false;bool ΔΫ=false;
bool ΔΡ=false;bool ΑЏ=false;bool ΔΣ=false;float ΓЇ=0;float ΔΑ=0;Vector3 Γπ=new Vector3();Vector3 Αώ=new Vector3();Vector3 Εΰ=new Vector3(1,1,1);float Εή=1;Vector3 Εί=new Vector3();bool ΕΑ=false;bool Γυ=false;bool ΕΚ=false;bool Δϋ=false;float ΔЈ=0;float Δϊ=2;Vector3 Εά;
Vector3 ΕΫ;Vector3 ΕΪ;float[,]ȭ=new float[3,2];Dictionary<String,float[,]>м=new Dictionary<string,float[,]>();Dictionary<String,float>Εξ=new Dictionary<String,float>();int ΔЀ=0;Dictionary<String,float[]>ı=new Dictionary<string,float[]>();float ΕΤ;bool Αβ=false;int ē=0;int щ=0;
int Ĥ=0;int ў=0;String Φ="";int[]Αό=new int[Enum.GetValues(typeof(Ι)).Length];bool ƃ=false;int Ǒ=0;int ι=0;Ι Ϋ=Ι.Main;int Δά=0,Δέ=0,Δή=0,Δί=0;int ΔΙ=0,ΔΚ=0;bool ȏ=false;bool ΑΡ=false;String ȧ="";String ũ="";IMyBroadcastListener е=null;bool Αι=true;List<ο>ψ=null;
ο κ=null;int ė=0,Ę=0;int Αά=0,Αέ=0;int Γε=0,Γζ=0,Γη=0;String ΓЀ="";Exception ǯ=null;bool ΑЇ=false;Ο ƌ=Ο.в;bool Αα=false;bool ΕΗ=true;bool ΕΥ=false;String Δγ="";bool ΒΑ=false;bool Ɠ=true;int ŵ=0;int[]ΕΧ=null;Vector3 Ū=new Vector3();bool Αλ=false;bool ō(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;
Program(){и=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Ʒ(Me,controllerTag,true))
Ν=Μ.Ћ;Τ(Ι.Main);if(Ν!=Μ.Ћ){Φ="Welcome to [PAM]!";Ч Ɗ=ΒЊ();if(Ν==Μ.ф){List<IMyShipDrill>Αη=new List<IMyShipDrill>();List<IMyShipGrinder>Αμ=new List<IMyShipGrinder>();и.GetBlocksOfType(Αη,q=>q.CubeGrid==Me.CubeGrid);и.GetBlocksOfType(Αμ,q=>q.CubeGrid==Me.CubeGrid);if(Αη.Count>0)
{Ν=Μ.Œ;Φ="Miner mode enabled!";}else if(Αμ.Count>0){Ν=Μ.Ǡ;Φ="Grinder mode enabled!";}else{Ν=Μ.ζ;ΑΘ(у.ζ);}}if(Ɗ==Ч.ΑΛ) ΓЋ=false;if(Ɗ==Ч.Ʀ) Φ="Data restore failed!";if(Ɗ==Ч.ǈ) Φ="New version, wipe data";}}void Main(string І,UpdateType Δα){try{if(ǯ!=null){if(І.Trim().ToUpper()=="RETRY")
{ǯ=null;ș=true;ΔΘ=0;if(Ν!=Μ.Ћ) і();Φ="Restarted after error";}else{Βς();ΒЈ();return;}}ŷ=(Δα&UpdateType.Update10)!=0;if(ŷ) Ɨ++;ȟ=Ɨ>=10;if(ȟ) Ɨ=0;if(ŷ){ē++;if(ē>4) ē=0;Ĳ=(int) Math.Max(0,10-(DateTime.Now-Ατ).TotalSeconds);}if(І!="") ΓЀ=І;if(Ν!=Μ.Ћ)
ΓΘ(І);else
ΒΣ(І);ƃ=false;try{int Αή=Runtime.CurrentInstructionCount;float ǒ=В(Αή,Runtime.MaxInstructionCount);if(ǒ>0.90) Φ="Max. instructions >90%";if(ǒ>Ǳ) Ǳ=ǒ;if(Αβ){Ǯ.Add(Αή);while(Ǯ.Count>10) Ǯ.RemoveAt(0);Ǧ=0;for(int i=0;i<Ǯ.Count;i++) Ǧ+=Ǯ[i];Ǧ=В(В(Ǧ,Ǯ.Count),Runtime.MaxInstructionCount);
double ź=Runtime.LastRunTimeMs;if(Γϊ&&ź>Ɓ) Ɓ=ź;ǹ.Add((int)(ź*1000f));while(ǹ.Count>10) ǹ.RemoveAt(0);Ȍ=0;for(int i=0;i<ǹ.Count;i++) Ȍ+=ǹ[i];Ȍ=В(Ȍ,ǹ.Count)/1000f;}}catch{Ǳ=0;}}catch(Exception e){ǯ=e;}}String Βκ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ν!=Μ.ζ)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Τ(Ι Є){Αό[(int) Ϋ]=ι;ι=Αό[(int) Є];if(Є==Ι.ȇ) ι=0;Ϋ=Є;if(Ν!=Μ.Ћ)
đ(Ϋ==Ι.Ȅ,false,0,0);ƃ=true;}void Βϋ(bool Ž){if(ι>0) ι--;else if(Ž) ι=Ǒ-1;}void Βϊ(bool Ž){if(ι<Ǒ-1) ι++;else if(Ž) ι=0;}String ΑΘ(у Ј){switch(Ј){case у.ΑΕ:Φ="Job is running";break;case у.ΒΡ:Φ="Connector not ready!";break;case у.ΓΙ:Φ="Ship modified, path outdated!";break;
case у.Βχ:Φ="Interrupted by player!";break;case у.ζ:Φ="Shuttle mode enabled!";break;}return "";}String Βπ(д δ){switch(δ){case д.š:return "Top-Left";case д.ΒΝ:return "Center";default:return "";}}String Βη(а ΰ){switch(ΰ){case а.ΒΘ:return "Auto"+(Ν==Μ.Œ?" (Ore)":"");case а.Ɵ:return "Auto (+Stone)";
case а.Ń:return "Default";default:return "";}}String Βν(Ο φ){switch(φ){case Ο.в:return "No job";case Ο.ŕ:return "Job paused";case Ο.ѓ:return "Job active";case Ο.Ȳ:return "Job active";case Ο.ȳ:return "Job active";case Ο.ƥ:return "Job done";case Ο.Ă:return "Job changed";case Ο.ǆ:return "Move home";
case Ο.Ǉ:return "Move to job";}return "";}String ƫ(с Γέ){switch(Γέ){case с.ΑΓ:return "Return home";case с.Ββ:return "Fly to job pos";case с.ΓΛ:return "Stop";case с.Ŋ:return "Ignore";}return "";}String Βθ(Р ΰ){switch(ΰ){case Р.ǉ:return "Off";case Р.Βά:return "Drop pos (Stone) ";case Р.ΒΫ:return "Drop pos (Sto.+Ice)";
case Р.ΒΥ:return "Cur. pos (Stone)";case Р.ΒΤ:return "Cur. pos (Sto.+Ice)";case Р.Βυ:return "In motion (Stone)";case Р.Βτ:return "In motion (Sto.+Ice)";}return "";}String Ʃ(о φ){switch(φ){case о.None:return "No batteries";case о.Ł:return "Charging";case о.ΒΦ:return "Discharging";}return "";}String ư(Λ φ)
{String ΕΖ=Ν==Μ.ζ?"target":"job";switch(φ){case Λ.є:return "Idle";case Λ.Ň:return "Flying to XY position";case Λ.ǅ:return Ν==Μ.Ǡ?"Grinding":"Mining";case Λ.ř:return "Returning";case Λ.Βί:return "Flying to drop pos";case Λ.ΑΔ:return "Returning to dock";case Λ.Ȃ:return "Flying to dock area";
case Λ.т:return "Flying to job area";case Λ.ņ:return "Flying to path";case Λ.Βγ:return "Flying to job position";case Λ.ǜ:return "Approaching dock";case Λ.Ƥ:return "Docking";case Λ.ΒΕ:return "Aligning";case Λ.ƞ:return "Aligning";case Λ.ΒЌ:return "Retry docking";case Λ.ȉ:return "Unloading";case Λ.Ȋ:return Δγ;
case Λ.Ȉ:return "Undocking";case Λ.Ł:return "Charging batteries";case Λ.ΓΦ:return "Waiting for uranium";case Λ.Βή:return "Filling up hydrogen";case Λ.ΓΤ:return "Waiting for ejection";case Λ.ΓΥ:return "Waiting for ejection";case Λ.Βΰ:return "Flying to drop pos";}return "";}String Ƶ(ύ А){switch(А)
{case ύ.Ǌ:return "On \"Undock\" command";case ύ.ǋ:return "On player entered cockpit";case ύ.ΒЄ:return "On ship is full";case ύ.ΒЃ:return "On ship is empty";case ύ.ǌ:return "On time delay";case ύ.ΒϏ:return "On batteries empty(<25%)";case ύ.Βύ:return "On batteries empty(=0%)";case ύ.Βώ:return "On batteries full";
case ύ.ΒЂ:return "On hydrogen empty(<25%)";case ύ.ΒЀ:return "On hydrogen empty(=0%)";case ύ.ΒЁ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int ΑЊ,int ш,bool Ε,String ǎ){Ǒ+=1;if(ΑЊ==ш) ǎ=">"+ǎ+(ē>=2?" .":"");else ǎ=" "+ǎ;Β+=ǎ+"\n";return ΑЊ==ш&&Ε;}String ΒΙ(bool Ε)
{int Α=0;int Δ=ι;Ǒ=0;String Ǽ="———————————————\n";String ǿ="--------------------------------------------\n";String Β="";Β+=Βν(μ)+" | "+(Ξ.β?"Ready to dock":"No dock")+"\n";Β+=Ǽ;double ΑЎ=Math.Max(Math.Round(this.Αε),0);if(Ϋ==Ι.Main){bool ъ=Ν==Μ.ζ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΓΚ();
if(Ν==Μ.Œ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Τ(Ι.Ȅ);if(Ν==Μ.Ǡ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Τ(Ι.Ȅ);if(Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Τ(Ι.Ş);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) Ƣ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) Βα();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) Βδ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ъ) Τ(Ι.Ď);else Τ(Ι.ā);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Τ(Ι.ƹ);if(Ν!=Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Τ(Ι.Ƹ);}else if(Ϋ==Ι.Ȅ){double Δη=Math.Round(Ж*Ǘ,1);double Γφ=Math.Round(Е*ѐ,1);String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Start new job!")) ΑΚ();if(Ζ(ref ε,Δ,Α++,Ε," Change current job")){Ţ(false,false);Τ(Ι.Main);}if(Ζ(ref ε,Δ,Α++,Ε," Width + (Width: "+Ж+" = "+Δη+"m)")){Ǜ(ref Ж,5,20,1);Ǥ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Width -")){Ǜ(ref Ж,-5,20,-1);Ǥ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height + (Height: "+Е+" = "+Γφ+"m)")){Ǜ(ref Е,5,20,1);Ǥ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height -")){Ǜ(ref Е,-5,20,-1);Ǥ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth + ("+(з==а.Ń?"Depth":"Min")+": "+С+"m)")){Ǜ(ref С,5,50,2);Ǥ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Depth -")){Ǜ(ref С,-5,50,-2);Ǥ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth mode: "+Βη(з))){з=р(з);}if(Ζ(ref ε,Δ,Α++,Ε," Start pos: "+Βπ(Ǖ))){Ǖ=р(Ǖ);}if(Ν==Μ.Ǡ&&з==а.Ɵ) з=р(з);Β+=Ś(8,ε,Δ,ref ΔΙ);}else if(Ϋ==Ι.Ş){float[]Αγ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Τ(Ι.ΑΙ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Ƶ(ν.А))) ν.А=р(ν.А);if(!ν.ƶ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ň((int) ν.Т))) ν.Т=ƣ(ν.Т,Αγ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Ƶ(ξ.А))) ξ.А=р(ξ.А);
if(!ξ.ƶ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ň((int) ξ.Т))) ξ.Т=ƣ(ξ.Т,Αγ);}else if(Ϋ==Ι.ΑΙ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ΑΚ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Ş);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.Ш!=""?ν.Ш:"-"))) ν.Ш=Ĉ(ref Δά);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.Ъ!=""?ν.Ъ:"-"))) ν.Ъ=Ĉ(ref Δή);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.Ш!=""?ξ.Ш:"-"))) ξ.Ш=Ĉ(ref Δέ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.Ъ!=""?ξ.Ъ:"-"))) ξ.Ъ=Ĉ(ref Δί);
}else if(Ϋ==Ι.ȃ){String Αχ=Ы+" %";if(ȟ) щ++;if(щ>1){щ=0;Ĥ++;if(Ĥ>1) Ĥ=0;}bool[]Ɛ=new bool[]{ƈ.Count==0,ȍ==о.None,ȝ.Count==0};int ΑБ=0;while(true){ΑБ++;ў++;if(ў>Ɛ.Length-1) ў=0;if(ΑБ>=Ɛ.Length) break;if(!Ɛ[ў]) break;}bool ъ=Ν==Μ.ζ;if(!ъ&&ǚ&&ϊ!=-1&&Ĥ==0)
Αχ=ϊ<1000000?Math.Round(ϊ)+" Kg":Math.Round(ϊ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){і();Τ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ъ) Τ(Ι.ā);else Τ(Ι.Ď);if(!ъ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΒΔ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΒΑ=true;Β+=ǿ;if(!ъ) Β+="Progress: "+Math.Round(ħ,1)+" %\n";Β+="State: "+ư(ě)+" "+ΑЎ+"m \n";Β+="Load: "+Ż+" % Max: "+Αχ+" \n";if(ў==0) Β+="Uranium: "+(ƈ.Count==0?"No reactors":Math.Round(ΒΒ,1)+"Kg "+(Л==-1?"":" Min: "+Л+" Kg"))+"\n";
if(ў==1) Β+="Battery: "+(ȍ==о.None?Ʃ(ȍ):ΑΧ+"% "+(Ь==-1||ъ?"":" Min: "+Ь+" %"))+"\n";if(ў==2) Β+="Hydrogen: "+(ȝ.Count==0?"No tanks":Math.Round(Αο,1)+"% "+(Э==-1||ъ?"":" Min: "+Э+" %"))+"\n";}else if(Ϋ==Ι.ā){String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Back")){if(μ==Ο.ѓ) Τ(Ι.ȃ);else Τ(Ι.Main);}
if(Ζ(ref ε,Δ,Α++,Ε," Max load: "+Ы+"%")) Њ(ref Ы,Ы<=80?-10:-5,λ.ŏ,false);if(Ζ(ref ε,Δ,Α++,Ε," Weight limit: "+(ǚ?"On":"Off"))) ǚ=!ǚ;if(Ζ(ref ε,Δ,Α++,Ε," Ejection: "+Βθ(Ȗ))) Ȗ=р(Ȗ);if(Ζ(ref ε,Δ,Α++,Ε," Toggle sorters: "+(ǖ?"On":"Off"))){ǖ=!ǖ;if(ǖ) ŝ(Ɠ);}
if(Ζ(ref ε,Δ,Α++,Ε," Unload ice: "+(ё?"On":"Off"))) ё=!ё;if(Ζ(ref ε,Δ,Α++,Ε," Uranium: "+(Л==-1?"Ignore":"Min "+Л+"Kg"))) Њ(ref Л,(Л>5?-5:-1),λ.ő,true);if(Ζ(ref ε,Δ,Α++,Ε," Battery: "+(Ь==-1?"Ignore":"Min "+Ь+"%"))) Њ(ref Ь,-5,λ.ǃ,true);if(Ζ(ref ε,Δ,Α++,Ε," Hydrogen: "+(Э==-1?"Ignore":"Min "+Э+"%"))) Њ(ref Э,-10,λ.Ǆ,true);
if(Ζ(ref ε,Δ,Α++,Ε," When done: "+(Ȱ?"Return home":"Stop"))) Ȱ=!Ȱ;if(Ζ(ref ε,Δ,Α++,Ε," On damage: "+ƫ(э))) э=р(э);if(Ζ(ref ε,Δ,Α++,Ε," Advanced...")) Τ(Ι.Ľ);Β+=Ś(8,ε,Δ,ref ΔΚ);}else if(Ϋ==Ι.Ľ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.ѓ) Τ(Ι.ȃ);else Τ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ν==Μ.Ǡ?" Grinder":" Drill")+" inv. balancing: "+(Ȟ?"On":"Off"))) Ȟ=!Ȟ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ν==Μ.Ǡ?" grinders":" drills")+": "+(ȗ?"Fwd + Bwd":"Fwd"))) ȗ=!ȗ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ļ+"m/s")) Њ(ref Ļ,0.5f,λ.ţ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ĺ+"m/s")) Њ(ref ĺ,0.5f,λ.ţ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(х*100f)+"%"+(х>0.80f?" (risky)":""))) Њ(ref х,0.1f,λ.ļ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ȱ*100f+"%")) ơ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ǭ*100f+"%")) ơ(false,0.05f);
}else if(Ϋ==Ι.Ď){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.ѓ) Τ(Ι.ȃ);else Τ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+Ы+"%")) Њ(ref Ы,Ы<=80?-10:-5,λ.ŏ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ё?"On":"Off"))) ё=!ё;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Л==-1?"Ignore":"Min "+Л+"Kg"))) Њ(ref Л,(Л>5?-5:-1),λ.ő,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(Ь==-1?"Ignore":"Charge up"))) Ь=(Ь==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(Э==-1?"Ignore":"Fill up"))) Э=(Э==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ƫ(э))) э=р(э);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(х*100f)+"%"+(х>0.80f?" (risky)":""))) Њ(ref х,0.1f,λ.ļ,false);
}else if(Ϋ==Ι.ΑΒ){double ΓЁ=0;if(π.Count>0) ΓЁ=Vector3.Distance(π.Last().Ρ,У);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΓΞ();if(Ν!=Μ.ζ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ђ?"Use old home":(Ξ.β?"Was set! ":"none ")))) ђ=!ђ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ђ?"Use old connector":(Ξ.β?"Was set! ":"none ")))) ђ=!ђ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ȯ?"Use old connector":(Σ.β?"Was set! ":"none ")))) Ȯ=!Ȯ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ȯ?"Use old path":(π.Count>1?"Count: "+π.Count:"none ")))) ȯ=!ȯ;Β+=ǿ;Β+="Wp spacing: "+Math.Round(Δε)+"m\n";}else if(Ϋ==Ι.Š){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){і();Τ(Ι.Main);}
Β+=ǿ;Β+="State: "+ư(ě)+" \n";Β+="Speed: "+Math.Round(ΔΠ,1)+"m/s\n";Β+="Target dist: "+ΑЎ+"m\n";Β+="Wp count: "+π.Count+"\n";Β+="Wp left: "+Δζ+"\n";}else if(Ϋ==Ι.ƹ){List<IMyTerminalBlock>љ=Βζ();if(ȟ) щ++;if(щ>=љ.Count) щ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Τ(Ι.ƺ);
Β+=ǿ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(Ż,1)+"% "+Math.Round(ј,1)+" / "+Math.Round(ǐ,1)+"\n";Β+="Uranium: "+(ƈ.Count==0?"No reactors":Math.Round(ΒΒ,1)+"Kg "+ΓЅ)+"\n";Β+="Battery: "+(ȍ==о.None?"":ΑΧ+"% ")+Ʃ(ȍ)+"\n";Β+="Hydrogen: "+(ȝ.Count==0?"No tanks":Math.Round(Αο,1)+"% ")+"\n";
Β+="Damage: "+(љ.Count==0?"None":""+(щ+1)+"/"+љ.Count+" "+љ[щ].CustomName)+"\n";}else if(Ϋ==Ι.ƺ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=ǿ;Β+="Next scan: "+Ĳ+"s\n";Β+="Ship size: "+Math.Round(Ǘ,1)+"m "+Math.Round(ѐ,1)+"m "+Math.Round(ǽ,1)+"m \n";
Β+="Broadcast: "+(ȏ?"Online - "+ȧ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ǳ*100f,1)+"% \n";}else if(Ϋ==Ι.Ƹ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=ǿ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ƍ==2) Β="Fatal setup error\nNext scan: "+Ĳ+"s\n";if(ΑЇ) Β="Recompile script now";int ΓЃ=Β.Split('\n').Length;for(int i=ΓЃ;i<=10;i++) Β+="\n";Β+=Ǽ;Β+="Last: "+Φ+"\n";
return Β;}void ơ(bool Γύ,float Ĵ){і();Ţ(true,false);if(Γύ) Њ(ref ȱ,Ĵ,λ.Ŕ,false);else Њ(ref ǭ,Ĵ,λ.Ŕ,false);ΒΜ();đ(true,true,0,0);}float ƣ(float Ǩ,float[]ķ){float Ψ=ķ[0];for(int i=ķ.Length-1;i>=0;i--)
if(Ǩ<ķ[i]) Ψ=ķ[i];return Ψ;}String Ĉ(ref int ш){String Ψ="";if(ш>=ΑА.Count) ш=-1;if(ш>=0) Ψ=ΑА[ш].CustomName;ш++;return Ψ;}void Ǜ(ref int value,int Ȏ,int ĵ,int Ƒ){if(Ȏ==0) return;if(value<ĵ&&Ƒ>0||value<=ĵ&&Ƒ<0){value+=Ƒ;return;}int ΓΨ=Math.Abs(Ȏ);int Ť=0;int Ƅ=1;
while(true){Ť+=Ƅ*ΓΨ*10;if(Ȏ<0&&value-ĵ<=Ť) break;if(Ȏ>0&&value-ĵ<Ť) break;Ƅ++;}value+=Ƅ*Ȏ;}void Ǥ(bool Εκ){Ж=Math.Max(Ж,1);Е=Math.Max(Е,1);С=Math.Max(С,0);đ(Ϋ==Ι.Ȅ,false,0,0);}void Њ(ref float value,float Ĵ,λ Ѓ,bool Ȭ){Њ(ref value,value+Ĵ,Ѓ,Ȭ,true);
}void Њ(ref float value,float ǳ,λ Ѓ,bool Ȭ,bool Δκ){float М=0,К=0;if(Ѓ==λ.ţ){М=0.5f;К=10f;}if(Ѓ==λ.ļ){М=0.1f;К=1f;}if(Ѓ==λ.Ŋ){М=50f;К=100f;}if(Ѓ==λ.ǃ){М=5f;К=30f;}if(Ѓ==λ.ő){М=1f;К=25f;}if(Ѓ==λ.ŏ){М=10f;К=95f;}if(Ѓ==λ.Ǆ){М=10f;К=90f;}
if(Ѓ==λ.ΓΠ){М=10f;К=1800;}if(Ѓ==λ.Ŕ){М=0.0f;К=0.75f;}if(ǳ==-1&&Ȭ){value=-1;return;}if(value==-1) Ȭ=false;bool ΑЀ=ǳ<М||ǳ>К;if(ΑЀ&&Δκ){if(ǳ<value) value=К;else if(ǳ>value) value=М;}else value=ǳ;if(ΑЀ&&Ȭ) value=-1;else value=Math.Max(М,Math.Min(value,К));
value=(float) Math.Round(value,2);}void ΒΔ(bool ΔΔ){if(ΔΔ) ж=Math.Max(0,ж-1);else ж++;ΑΠ(true);}T р<T>(T Ǩ){int Α=Array.IndexOf(Enum.GetValues(Ǩ.GetType()),Ǩ);Α++;if(Α>=Βέ(Ǩ)) Α=0;return(T) Enum.GetValues(Ǩ.GetType()).GetValue(Α);}int Βέ<T>(T value){return Enum.GetValues(value.GetType()).Length;
}void ΓΚ(){Ĺ.Clear();for(int i=0;i<π.Count;i++) Ĺ.Add(π[i]);π.Clear();Απ=true;Αξ=new Χ(Ξ);Ας=new Χ(Σ);Ξ.β=false;if(Ν==Μ.ζ) Σ.β=false;for(int i=0;i<м.Count;i++)
if(!н.Contains(м.Keys.ElementAt(i)))
н.Add(м.Keys.ElementAt(i));ђ=false;Ȯ=false;ȯ=false;Τ(Ι.ΑΒ);}void ΓΞ(){if(ђ) Ξ=Αξ;if(Ȯ) Σ=Ας;if(ȯ){π.Clear();for(int i=0;i<Ĺ.Count;i++) π.Add(Ĺ[i]);}Απ=false;і();Τ(Ι.Main);}void Č(Χ И,Ο φ){Γι=И;if(μ==Ο.ѓ) ы=φ;}bool Ƭ(Χ И,float ч,bool Δβ,out Vector3 δ){if(Δβ){Vector3I Γτ=new Vector3I((int) И.З.X,(int) И.З.Y,(int) И.З.Z);
IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(Γτ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){δ=new Vector3();return false;}Vector3 Γδ=ć(Љ,Ϊ.FatBlock.GetPosition()-У);Vector3 Γγ=ć(Љ,Ϊ.FatBlock.WorldMatrix.Forward);δ=И.Ρ-ΑΝ(И.ώ,И.ω*-1,Γδ)-ΑΝ(И.ώ,И.ω*-1,Γγ)*ч;return true;}
else{δ=И.Ρ;return true;}}Vector3 Βμ(int Γα,int ΑЉ,bool ΔΕ){if(!ΔΕ&&Αλ) return Ū;float Γβ=((Ǎ-1f)/2f)-Γα;float ΔΗ=((џ-1f)/2f)-ΑЉ;Ū=Σ.Ρ+Σ.Џ*Γβ*Ǘ+ǲ*-1*ΔΗ*ѐ;Αλ=true;return Ū;}Π ń(){float Д=-1;Π Ψ=Π.Ц;if(Ν!=Μ.ζ){Βω();if(μ!=Ο.в){Vector3 σ=ΑΞ(Ȣ,ǲ*-1,У-Σ.Ρ);
if(Math.Abs(σ.X)<=(float)(Ǎ*Ǘ)/2f&&Math.Abs(σ.Y)<=(float)(џ*ѐ)/2f){if(σ.Z<=-1&&σ.Z>=-Ƌ*2) return Π.Ā;if(σ.Z>-1&&σ.Z<ǽ*2) return Π.Ц;}if(п(Σ.Ρ,ref Д)) Ψ=Π.Ц;}if(Ξ.β){if(п(Ξ.Ρ,ref Д)) Ψ=Π.Х;for(int i=0;i<π.Count;i++)
if(п(π[i].Ρ,ref Д)) Ψ=Π.г;if(Vector3.Distance(У,Ξ.Ρ)<dockDist*ǽ) Ψ=Π.Х;if(Ǟ(MyShipConnectorStatus.Connectable)!=null||Ǟ(MyShipConnectorStatus.Connected)!=null)
Ψ=Π.Х;}}else{Vector3 δ=new Vector3();IMyShipConnector Ė=Ǟ(MyShipConnectorStatus.Connected);if(Ξ.β){if(п(Ξ.Ρ,ref Д)) Ψ=Π.Х;if(Ƭ(Ξ,dockDist,true,out δ))
if(п(δ,ref Д)) Ψ=Π.Х;if(Ė!=null&&Vector3.Distance(Ė.GetPosition(),Ξ.Ρ)<5)
return Π.ľ;}for(int i=0;i<π.Count;i++)
if(Vector3.Distance(π[i].Ρ,Ξ.Ρ)>dockDist*ǽ&&Vector3.Distance(π[i].Ρ,Σ.Ρ)>dockDist*ǽ)
if(п(π[i].Ρ,ref Д)) Ψ=Π.г;if(Σ.β){if(п(Σ.Ρ,ref Д)) Ψ=Π.Ц;if(Ƭ(Σ,dockDist,true,out δ))
if(п(δ,ref Д)) Ψ=Π.Ц;if(Ė!=null&&Vector3.Distance(Ė.GetPosition(),Σ.Ρ)<5)
return Π.Ŀ;}}return Ψ;}bool п(Vector3 δ,ref float Д){float ч=Vector3.Distance(δ,У);if(ч<Д||Д==-1){Д=ч;return true;}return false;}void ΑΚ(){if(ƍ>0){Φ="Setup error! Can't start";return;}if(Ν==Μ.ζ){Ƣ();return;}Σ.Ρ=У;Σ.η=Љ.GetNaturalGravity();Σ.ώ=Γς;Σ.ω=Γλ;Σ.Џ=ΓЂ;
Ȣ=ю.WorldMatrix.Forward;ǲ=Σ.ω;if(Ȣ==Љ.WorldMatrix.Down) ǲ=Љ.WorldMatrix.Backward;Ţ(true,true);ά(Λ.Ň);ş();}void Ţ(bool Й,bool ΔΖ){if(μ==Ο.в&&!Й) return;bool ΓЌ=Й||μ==Ο.ƥ||Ǎ!=Ж||џ!=Е||Ĩ!=Ǖ;if(ΓЌ){if(μ!=Ο.в){μ=Ο.Ă;Βμ(ġ,Ģ,ΔΖ);Φ="Job changed, lost progress";}Ĩ=Ǖ;
Ǎ=Ж;џ=Е;Ģ=0;ġ=0;Ů="";İ=-1;ƀ=0;Ƞ=0;Αί=0;ж=0;ΑΠ(true);}}void Ŗ(){ΓΓ(У,0);ċ(ȫ,true);}void ş(){ΑΘ(у.ΑΕ);Ŗ();Ŝ(Ÿ,false);ŝ(Ɠ);μ=Ο.ѓ;ΒΛ(true);ы=μ;Τ(Ι.ȃ);ΒЎ();Αΰ=true;ŵ=0;for(int i=ȋ.Count-1;i>=0;i--)
if(Ƽ(ȋ[i],false)) ŵ++;if(ŵ>0) Φ="Started with damage";}void і(){if(μ==Ο.ѓ){μ=Ο.ŕ;Φ="Job paused";}ά(Λ.є);ы=μ;ΓΔ(false,0,0,0,0);ΓΜ();ΒΗ(new Vector3(),false);ΓΝ();ΒБ(ChargeMode.Auto);ΓΕ(false);ΓΒ(true);ѕ(Λ.є);đ(false,false,0,0);ċ(Ф,false);ċ(Ȓ,true);ŝ(true);ΑЈ=false;ΒΟ();Γθ=false;
Γκ=false;Αΰ=false;Ə=false;ΒΑ=false;if(Ϋ!=Ι.Main&&Ϋ!=Ι.ā&&Ϋ!=Ι.Ľ&&Ϋ!=Ι.Ď)
Τ(Ι.Main);}void Ƣ(){Π к=ń();if(Ν==Μ.ζ){if(!Σ.β||!Ξ.β) return;ş();bool Ų=Vector3.Distance(У,Ξ.Ρ)<Vector3.Distance(У,Σ.Ρ);if(ƌ==Ο.Ȳ) Ų=true;if(ƌ==Ο.ȳ) Ų=false;if(Ų){Č(Ξ,Ο.Ȳ);switch(к){case Π.ľ:ά(Λ.Ȋ);break;case Π.г:ά(Λ.т);break;case Π.Х:ά(Λ.ǜ);break;default:ά(Λ.Ȉ);break;
}}else{Č(Σ,Ο.ȳ);switch(к){case Π.Ŀ:ά(Λ.Ȋ);break;case Π.Ц:ά(Λ.ǜ);break;case Π.г:ά(Λ.т);break;default:ά(Λ.Ȉ);break;}}}else{if(μ!=Ο.ŕ&&μ!=Ο.Ă) return;bool Δδ=μ==Ο.Ă;ş();bool Ŵ=Βψ(false)&&Ξ.β;switch(к){case Π.Ц:ά(Ŵ?Λ.ņ:Λ.Ň);break;case Π.г:ά(Ŵ?Λ.Ȃ:Λ.т);break;case Π.Х:ά(Ŵ?Λ.ǜ:Λ.ȉ);break;
case Π.Ā:{if(Ƞ!=ж||Δδ) ά(Λ.ř);else ά(Λ.ǅ);}break;default:break;}}}void Βδ(){if(μ==Ο.в&&!Ξ.β) return;if(Ν==Μ.ζ&&(!Σ.β||!Ξ.β)) return;Φ="Move to job";Π к=ń();if(Ν==Μ.ζ){Č(Σ,Ο.ȳ);switch(к){case Π.Ц:ά(Λ.ǜ);break;case Π.г:ά(Λ.т);break;case Π.Ŀ:return;default:ά(Λ.Ȉ);break;
}ѕ(Λ.Ȋ);}else{switch(к){case Π.Ц:ά(Λ.т);break;case Π.г:ά(Λ.т);break;case Π.Х:ά(Λ.ȉ);break;case Π.Ā:ά(Λ.ř);break;default:break;}if(μ==Ο.в) ѕ(Λ.т);else ѕ(Λ.ƞ);ΑЈ=true;}Ŗ();Τ(Ι.Š);Ŝ(Ÿ,false);ы=Ο.Ǉ;}void Βα(){if(!Ξ.β) return;Φ="Move home";Π к=ń();if(Ν==Μ.ζ){Č(Ξ,Ο.Ȳ);switch(к){
case Π.г:ά(Λ.Ȃ);break;case Π.Х:ά(Λ.ǜ);break;case Π.ľ:return;default:ά(Λ.Ȉ);break;}ѕ(Λ.Ȋ);}else{if(Ǟ(MyShipConnectorStatus.Connected)!=null) return;if(Ǟ(MyShipConnectorStatus.Connectable)!=null){ά(Λ.Ƥ);ѕ(Λ.ȉ);return;}switch(к){case Π.Ц:ά(Λ.ņ);break;case Π.г:ά(Λ.Ȃ);break;case Π.Х:ά(Λ.Ȃ);break;
case Π.Ā:ά(Λ.ΑΔ);break;default:break;}ѕ(Λ.ȉ);}Ŗ();Τ(Ι.Š);Ŝ(Ÿ,false);ы=Ο.ǆ;}void ά(Λ ƅ){if(ƅ==Λ.є) ȩ=Λ.є;if(ȩ!=Λ.є&&ě==ȩ&&ƅ!=ȩ){і();return;}ΔΦ=true;ě=ƅ;}void ѕ(Λ φ){this.ȩ=φ;}void ΑΠ(bool Ɖ){if(Ɖ){ħ=0;return;}if(Ν==Μ.ζ) return;float Αζ=ж*Math.Max(1,С);if(Ƞ==ж) Αζ+=Math.Min(С,Αί);
float Δΰ=Ǎ*џ*Math.Max(1,С);ħ=Math.Max(ħ,(float) Math.Min(Αζ/Δΰ*100.0,100));}List<IMyTerminalBlock>Βζ(){List<IMyTerminalBlock>љ=new List<IMyTerminalBlock>();for(int i=0;i<ȋ.Count;i++){IMyTerminalBlock Ϊ=ȋ[i];if(Ƽ(Ϊ,false)) љ.Add(Ϊ);}return љ;}bool Ƽ(IMyTerminalBlock Ϊ,bool ƕ)
{return(!ƽ(Ϊ,ƕ)||!Ϊ.IsFunctional);}bool ƽ(IMyTerminalBlock Ϊ,bool ƕ){if(Ϊ==null) return false;try{IMyCubeBlock Ακ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(ƕ) return Ακ==Ϊ;else return Ακ.GetType()==Ϊ.GetType();}catch{return false;}}bool Βψ(bool ΕΦ){if(ǚ&&Ν!=Μ.ζ)
if(ϊ!=-1&&ΑЋ>=ϊ){Φ="Ship too heavy";return true;}if(Ż>=Ы||Ə){Ə=false;Φ="Ship is full";return true;}return false;}static String Βρ(IMyThrust Я){return Я.BlockDefinition.SubtypeId;}Vector3 Ŏ(Vector3 Ѝ,float[,]ǰ){return new Vector3(
Ѝ.X>=0?ǰ[0,0]:ǰ[0,1],
Ѝ.Y>=0?ǰ[1,0]:ǰ[1,1],
Ѝ.Z>=0?ǰ[2,0]:ǰ[2,1]);}bool ƴ(Χ Ω,String ǘ,out float ů){ů=0;int Α=н.IndexOf(ǘ);if(Α==-1||Ω.П==null||Α>=Ω.П.Length)
return false;ů=Ω.П[Α];if(ů==-1) return false;return true;}Vector3 Ƴ(Vector3 Ѝ,Χ Ω){if(Ω!=null){Vector3 Ψ=new Vector3();for(int i=0;i<м.Keys.Count;i++){String ǘ=м.Keys.ElementAt(i);float ȕ=0;if(!ƴ(Ω,ǘ,out ȕ))
return Ŏ(Ѝ,ȭ);Ψ+=Ŏ(Ѝ,м.Values.ElementAt(i))*ȕ;}return Ψ;}return Ŏ(Ѝ,ȭ);}float ŉ(Vector3 Ѝ,Χ Ω){return ŉ(Ѝ,new Vector3(),Ω);}float ŉ(Vector3 Ѝ,Vector3 η,Χ Ω){Vector3 ΔΪ=Ƴ(Ѝ,Ω);Vector3 Γν=ΔΪ+η*ΑЋ;float ǒ=(Γν/Ѝ).AbsMin();return(float)(Ѝ*ǒ).Length();}float ą(Vector3 Ț,Vector3 ϋ,Vector3 η,Χ Ω)
{if(η.Length()==0f) return 0;Vector3 ΓЄ=ΑΞ(Ț,ϋ,Vector3.Normalize(η));float Й=ŉ(-ΓЄ,Ω);return Й/η.Length();}void ΒΛ(bool Γω){float Б=0;float Ű=0.9f;if(Γω){ϊ=-1;Ǚ=0;ĳ=null;if(μ!=Ο.в&&Σ.η.Length()!=0){Б=Ű*ą(Σ.ώ,Σ.ω*-1,Σ.η,null);if(Б<ϊ||ϊ==-1) ϊ=Б;}if(Ξ.β&&Ξ.η.Length()!=0)
{Б=Ű*ą(Ξ.ώ,Ξ.ω*-1,Ξ.η,null);if(Б<ϊ||ϊ==-1) ϊ=Б;}return;}if(Ǚ==-1) return;if(Ǚ>=0){int ΑЄ=0;while(Ǚ<π.Count){if(ΑЄ>100) return;ΑЄ++;Χ Ω=π[Ǚ];if(Ω.η.Length()!=0f){Б=Ű*Math.Min(
ą(Ω.ώ,Ω.ω*-1,Ω.η,Ω),
ą(Ω.ώ*-1,Ω.ω*-1,Ω.η,Ω));if(Б<ϊ||ϊ==-1) ϊ=Б;}else ĳ=Ω;Ǚ++;}Ǚ=-1;}bool ų=true;float ь=0;if(ĳ!=null){for(int i=0;i<м.Count;i++){String ǘ=м.Keys.ElementAt(i);float[,]ƚ=м.Values.ElementAt(i);float ȕ=0;if(!ƴ(ĳ,ǘ,out ȕ)){ų=false;break;}for(int a=0;a<ƚ.GetLength(0);a++)
for(int b=0;b<ƚ.GetLength(1);b++){float ї=Math.Abs(ƚ[a,b]*ȕ);if(ї==0) continue;ų=true;if(ь==0||ї<ь) ь=ї;}}}if(!ų){for(int a=0;a<ȭ.GetLength(0);a++)
for(int b=0;b<ȭ.GetLength(1);b++){float ї=Math.Abs(ȭ[a,b]);if(ї==0) continue;if(ь==0||ї<ь) ь=ї;}}if(ь>0){float ΓЉ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Б=В(ь,ΓЉ);if(Б>0)
if(Б<ϊ||ϊ==-1) ϊ=Б;}}void ΒΗ(Vector3 Й,bool υ){if(!υ){for(int i=0;i<ȫ.Count;i++)
ȫ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ŧ=Ƴ(Й,null);float Δλ=Math.Min(1,Math.Abs(В(Й.X,Ŧ.X)));float Δμ=Math.Min(1,Math.Abs(В(Й.Y,Ŧ.Y)));float Δν=Math.Min(1,Math.Abs(В(Й.Z,Ŧ.Z)));for(int i=0;i<ȫ.Count;i++){IMyThrust Я=ȫ[i];Vector3 ǩ=ΒЍ(ć(Љ,Я.WorldMatrix.Backward),1);
if(ǩ.X!=0&&Math.Sign(ǩ.X)==Math.Sign(Й.X))
Я.SetValueFloat("Override",Я.MaxThrust*Δλ);else if(ǩ.Y!=0&&Math.Sign(ǩ.Y)==Math.Sign(Й.Y))
Я.SetValueFloat("Override",Я.MaxThrust*Δμ);else if(ǩ.Z!=0&&Math.Sign(ǩ.Z)==Math.Sign(Й.Z))
Я.SetValueFloat("Override",Я.MaxThrust*Δν);else
Я.SetValueFloat("Override",0.0f);}}void ΓΔ(bool υ,float ΔΓ,float ǵ,float Ǹ,float ȁ){for(int i=0;i<Αν.Count;i++){IMyGyro θ=Αν[i];θ.GyroOverride=υ;if(!υ) θ.GyroPower=100;else θ.GyroPower=ΔΓ;if(!υ) continue;Vector3 Ў=Љ.WorldMatrix.Forward;Vector3 Ƿ=Љ.WorldMatrix.Right;Vector3 ϋ=Љ.WorldMatrix.Up;
Vector3 Ǫ=θ.WorldMatrix.Forward;Vector3 Ǭ=θ.WorldMatrix.Up;Vector3 ǫ=θ.WorldMatrix.Left*-1;if(Ǫ==Ў) θ.SetValueFloat("Roll",ȁ);else if(Ǫ==(Ў*-1)) θ.SetValueFloat("Roll",ȁ*-1);else if(Ǭ==(Ў*-1)) θ.SetValueFloat("Yaw",ȁ);else if(Ǭ==Ў) θ.SetValueFloat("Yaw",ȁ*-1);else if(ǫ==Ў) θ.SetValueFloat("Pitch",ȁ);
else if(ǫ==(Ў*-1)) θ.SetValueFloat("Pitch",ȁ*-1);if(ǫ==(Ƿ*-1)) θ.SetValueFloat("Pitch",ǵ);else if(ǫ==Ƿ) θ.SetValueFloat("Pitch",ǵ*-1);else if(Ǭ==Ƿ) θ.SetValueFloat("Yaw",ǵ);else if(Ǭ==(Ƿ*-1)) θ.SetValueFloat("Yaw",ǵ*-1);else if(Ǫ==(Ƿ*-1)) θ.SetValueFloat("Roll",ǵ);
else if(Ǫ==Ƿ) θ.SetValueFloat("Roll",ǵ*-1);if(Ǭ==(ϋ*-1)) θ.SetValueFloat("Yaw",Ǹ);else if(Ǭ==ϋ) θ.SetValueFloat("Yaw",Ǹ*-1);else if(ǫ==ϋ) θ.SetValueFloat("Pitch",Ǹ);else if(ǫ==(ϋ*-1)) θ.SetValueFloat("Pitch",Ǹ*-1);else if(Ǫ==ϋ) θ.SetValueFloat("Roll",Ǹ);else if(Ǫ==(ϋ*-1)) θ.SetValueFloat("Roll",Ǹ*-1);
}}void ΓΝ(){this.Γυ=false;}void ΓΜ(){č(false,false,false,Αώ,0);ΑЏ=false;}void ΓΓ(Vector3 α,float Ȩ){č(true,false,false,α,Ȩ);}void č(bool υ,bool ƒ,bool ƛ,Vector3 α,float Ȩ){č(υ,ƒ,ƛ,α,α-У,0.0f,Ȩ);}void č(bool υ,bool ƒ,bool ƛ,Vector3 α,Vector3 ΔΜ,float ΔΝ,float Ȩ){ΑЏ=true;
this.ΔΫ=υ;Αώ=α;this.ΓЇ=Ȩ;this.ΔΑ=ΔΝ;this.ΔΣ=ƒ;this.ΔΡ=ƛ;this.Γπ=ΔΜ;Αε=Vector3.Distance(α,У);}void ΒЎ(){и.GetBlocksOfType(ȋ,ō);}void ΒΜ(){if(Љ==null) return;ю=null;float ĭ=0,ĩ=0,Į=0,Ī=0,į=0,ī=0;List<IMyTerminalBlock>Ɩ=Ƨ(Ф,pamTag,true);bool ΓЎ=Ɩ.Count==0;
if(Ɩ.Count>0) ю=Ɩ[0];else if(Ф.Count>0) ю=Ф[0];int Ě=0;for(int i=0;i<Ф.Count;i++){if(Ф[i].WorldMatrix.Forward!=ю.WorldMatrix.Forward){if(ΓЎ){ƍ=2;Φ="Mining direction is unclear!";return;}continue;}Ě++;Vector3 σ=Ư(Љ,Ф[i].GetPosition());if(i==0){ĭ=σ.X;ĩ=σ.X;Į=σ.Y;Ī=σ.Y;į=σ.Z;ī=σ.Z;}
ĩ=Math.Max(σ.X,ĩ);ĭ=Math.Min(σ.X,ĭ);Ī=Math.Max(σ.Y,Ī);Į=Math.Min(σ.Y,Į);ī=Math.Max(σ.Z,ī);į=Math.Min(σ.Z,į);}Ǘ=(ĩ-ĭ)*(1-ȱ)+drillRadius*2;ѐ=(Ī-Į)*(1-ǭ)+drillRadius*2;if(ю!=null&&ю.WorldMatrix.Forward==Љ.WorldMatrix.Down)
ѐ=(ī-į)*(1-ǭ)+drillRadius*2;}List<T>Ƨ<T>(List<T>Ϗ,String О,bool ģ){List<T>Ψ=new List<T>();for(int i=0;i<Ϗ.Count;i++)
if(Ʒ(Ϗ[i],О,ģ)) Ψ.Add(Ϗ[i]);return Ψ;}bool Ʒ<T>(T Ϊ,String О,bool ģ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(ģ&&b.CustomName.ToUpper().Contains(О.ToUpper())) return true;if(!ģ&&b.BlockDefinition.ToString().ToUpper().Contains(О.ToUpper())) return true;return false;}void ċ<T>(List<T>Ϗ,bool υ)
{for(int i=0;i<Ϗ.Count;i++)
ΓΑ((IMyTerminalBlock) Ϗ[i],υ);}void ΓΑ(IMyTerminalBlock Ϊ,bool υ){if(Ϊ==null) return;String ΓΩ=υ?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(ΓΩ);a.Apply(Ϊ);}void ŝ(bool υ){Ɠ=υ;if(!ǖ) return;ċ(ΔΤ,υ);}void ΒБ(ChargeMode ΰ){for(int i=0;i<ΑΦ.Count;i++) ΑΦ[i].ChargeMode=ΰ;}void ΓΕ(bool ΔΨ)
{for(int i=0;i<ȝ.Count;i++) ȝ[i].Stockpile=ΔΨ;}void Ŝ(List<IMyLandingGear>ű,bool Αψ){for(int i=0;i<ű.Count;i++){if(Αψ) ű[i].Lock();if(!Αψ) ű[i].Unlock();}}void ΓΒ(bool υ){if(Αα==υ) return;List<IMyShipController>ę=new List<IMyShipController>();и.GetBlocksOfType(ę,ō);if(ę.Count==0) return;
for(int i=0;i<ę.Count;i++)
ę[i].DampenersOverride=υ;Αα=υ;}IMyShipConnector Ǟ(MyShipConnectorStatus ΔΧ){for(int i=0;i<Ȓ.Count;i++){if(!ƽ(Ȓ[i],true)) continue;if(Ȓ[i].Status==ΔΧ) return Ȓ[i];}return null;}void đ(bool Ǝ,bool Ĝ,float Γσ,float Γά){if(τ==null||Ф.Count==0) return;Vector3 ĕ=new Vector3();int Ě=0;
for(int i=0;i<Ф.Count;i++){if(Ф[i].WorldMatrix.Forward!=ю.WorldMatrix.Forward) continue;Ě++;ĕ+=Ф[i].GetPosition();}ĕ=ĕ/Ě;Vector3 Ǵ=Ư(τ,ĕ);τ.Enabled=true;τ.ShowOnHUD=Ǝ;τ.LeftExtend=(Ĝ?1:Ж)/2f*Ǘ-Ǵ.X;τ.RightExtend=(Ĝ?1:Ж)/2f*Ǘ+Ǵ.X;τ.TopExtend=(Ĝ?1:Е)/2f*ѐ+Ǵ.Y;
τ.BottomExtend=(Ĝ?1:Е)/2f*ѐ-Ǵ.Y;τ.FrontExtend=(Ǝ?С:Γσ)-Ǵ.Z;τ.BackExtend=Ǝ?0:Γά+ǽ*0.75f+Ǵ.Z;τ.DetectFloatingObjects=true;τ.DetectAsteroids=false;τ.DetectLargeShips=true;τ.DetectSmallShips=true;τ.DetectStations=true;τ.DetectOwner=true;τ.DetectSubgrids=false;
τ.DetectPlayers=false;τ.DetectEnemy=true;τ.DetectFriendly=true;τ.DetectNeutral=true;}void Ȇ<T>(List<T>Ϗ,bool ΔΟ,float Γρ,bool Γό){for(int i=0;i<Ϗ.Count;i++){IMyTextSurface Ȫ=null;if(Ϗ[i]is IMyTextSurface) Ȫ=(IMyTextSurface) Ϗ[i];if(Ȫ!=null){Ȫ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΔΟ) continue;Ȫ.Font="Debug";if(Γό) continue;Ȫ.FontSize=Γρ;}}}static float В(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΒЍ(Vector3 v,int Ŭ){return new Vector3(Math.Round(v.X,Ŭ),Math.Round(v.Y,Ŭ),Math.Round(v.Z,Ŭ));}Vector3 Ư(IMyTerminalBlock Ϊ,Vector3 Δι){return ć(Ϊ,Δι-Ϊ.GetPosition());
}Vector3 ć(IMyTerminalBlock Ϊ,Vector3 Ɲ){return Vector3D.TransformNormal(Ɲ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 ΑΞ(Vector3 Ț,Vector3 ϋ,Vector3 Ɲ){MatrixD ſ=MatrixD.CreateFromDir(Ț,ϋ);return Vector3D.TransformNormal(Ɲ,MatrixD.Transpose(ſ));}Vector3 ΑΝ(Vector3 Ț,Vector3 ϋ,Vector3 σ){MatrixD ſ=MatrixD.CreateFromDir(Ț,ϋ);
return Vector3D.Transform(σ,ſ);}String ί(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ή(String s){String[]έ=s.Split('|');return new Vector3(float.Parse(Θ(έ,0)),float.Parse(Θ(έ,1)),float.Parse(Θ(έ,2)));}String ΓΧ(Χ Ω){String ǔ=":";String Ψ=ί(Ω.Ρ)+ǔ+ί(Ω.ώ)+ǔ+
ί(Ω.ω)+ǔ+ί(Ω.Џ)+ǔ+ί(Ω.η);for(int i=0;i<Ω.П.Length;i++)
Ψ+=ǔ+Math.Round(Ω.П[i],3);return Ψ;}Χ ΓΟ(String s){String[]έ=s.Split(':');Χ Ω=new Χ(
ή(Θ(έ,0)),ή(Θ(έ,1)),
ή(Θ(έ,2)),ή(Θ(έ,3)),
ή(Θ(έ,4)));int Α=5;List<float>Αθ=new List<float>();while(Α<έ.Length){String Ķ=Θ(έ,Α);float f=0;if(!float.TryParse(Ķ,out f)) break;Αθ.Add(f);Α++;}Ω.П=Αθ.ToArray();return Ω;}System.Text.StringBuilder Ǻ=new System.Text.StringBuilder();void Κ<T>(T value,bool ΓЍ){if(ΓЍ) Ǻ.Append('\n');Ǻ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String ă(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Đ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΓΪ,int Α){String Ķ=ΓΪ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Ķ)) return "";
return Ķ;}void Save(){if(ΑЇ||Ν==Μ.Ћ){Storage="";return;}Storage=ΒΚ();}String ΒΚ(){Ǻ.Clear();Ǻ.Append(DATAREV+";");Κ(ί(ΑІ),false);Κ(ί(Ξ.ώ));Κ(ί(Ξ.Џ));Κ(ί(Ξ.ω));Κ(ί(Ξ.η));Κ(ί(Ξ.Ρ));Κ(ί(Ξ.З));Κ(Ξ.β);Κ(ί(Σ.Ρ));Κ(ί(Σ.η));Κ(ί(Σ.ώ));Κ(ί(Σ.ω));Κ(ί(Σ.Џ));Κ(ί(Σ.З));Κ(Σ.β);Κ(ί(Ȣ));Κ(ί(ǲ));Κ(";");
Κ((int) Ν,false);Κ((int) μ);Κ((int) ы);Κ(Ы);Κ(Ь);Κ(Л);Κ(Э);Κ(х);Κ(ǚ);Κ(ё);if(Ν==Μ.ζ){Κ((int) ν.А);Κ(ν.Т);Κ(ν.ћ);Κ(ă(ν.Ш));Κ(ă(ν.Ъ));Κ((int) ξ.А);Κ(ξ.Т);Κ(ξ.ћ);Κ(ă(ξ.Ш));Κ(ă(ξ.Ъ));}else{Κ((int) Ǖ);Κ((int) э);Κ((int) Ȗ);Κ((int) з);Κ((int) Ĩ);Κ(Ȱ);Κ(ǖ);Κ(Ȟ);Κ(ȗ);Κ(Ж);Κ(Е);Κ(С);Κ(Ļ);Κ(ĺ);Κ(ȱ);Κ(ǭ);
Κ(Ǎ);Κ(џ);Κ(ġ);Κ(Ģ);Κ(Ƞ);Κ(ж);Κ(Ƌ);Κ(ƀ);}Κ(";");for(int i=0;i<н.Count;i++)
Κ((i>0?"|":"")+н[i],false);Κ(";");for(int i=0;i<π.Count;i++)
Κ(ΓΧ(π[i]),i>0);Κ(";",false);Κ(Ů+"|"+İ,false);Κ(";",false);Κ(ΒΩ(),false);String Υ=Ǻ.ToString();Ǻ.Clear();return Υ;}Ч ΒЊ(){return ΒЋ(Storage);}Ч ΒЋ(String Υ){if(Υ=="") return Ч.Βό;String[]Ǔ=Υ.Split(';');if(Θ(Ǔ,0)!=DATAREV) return Ч.ǈ;int Α=0;try{String[]Η=Θ(Ǔ,1).Split('\n');
ΑІ=ή(Θ(Η,Α++));Ξ.ώ=ή(Θ(Η,Α++));Ξ.Џ=ή(Θ(Η,Α++));Ξ.ω=ή(Θ(Η,Α++));Ξ.η=ή(Θ(Η,Α++));Ξ.Ρ=ή(Θ(Η,Α++));Ξ.З=ή(Θ(Η,Α++));Ξ.β=bool.Parse(Θ(Η,Α++));Σ.Ρ=ή(Θ(Η,Α++));Σ.η=ή(Θ(Η,Α++));Σ.ώ=ή(Θ(Η,Α++));Σ.ω=ή(Θ(Η,Α++));Σ.Џ=ή(Θ(Η,Α++));Σ.З=ή(Θ(Η,Α++));Σ.β=bool.Parse(Θ(Η,Α++));
Ȣ=ή(Θ(Η,Α++));ǲ=ή(Θ(Η,Α++));Η=Θ(Ǔ,2).Split('\n');Α=0;Ν=(Μ) int.Parse(Θ(Η,Α++));μ=(Ο) int.Parse(Θ(Η,Α++));ы=(Ο) int.Parse(Θ(Η,Α++));Ы=int.Parse(Θ(Η,Α++));Ь=int.Parse(Θ(Η,Α++));Л=int.Parse(Θ(Η,Α++));Э=int.Parse(Θ(Η,Α++));х=float.Parse(Θ(Η,Α++));ǚ=bool.Parse(Θ(Η,Α++));
ё=bool.Parse(Θ(Η,Α++));if(Ν==Μ.ζ){ν.А=(ύ) int.Parse(Θ(Η,Α++));ν.Т=float.Parse(Θ(Η,Α++));ν.ћ=float.Parse(Θ(Η,Α++));ν.Ш=Đ(Θ(Η,Α++));ν.Ъ=Đ(Θ(Η,Α++));ξ.А=(ύ) int.Parse(Θ(Η,Α++));ξ.Т=float.Parse(Θ(Η,Α++));ξ.ћ=float.Parse(Θ(Η,Α++));ξ.Ш=Đ(Θ(Η,Α++));ξ.Ъ=Đ(Θ(Η,Α++));}else{Ǖ=(д) int.Parse(Θ(Η,Α++));
э=(с) int.Parse(Θ(Η,Α++));Ȗ=(Р) int.Parse(Θ(Η,Α++));з=(а) int.Parse(Θ(Η,Α++));Ĩ=(д) int.Parse(Θ(Η,Α++));Ȱ=bool.Parse(Θ(Η,Α++));ǖ=bool.Parse(Θ(Η,Α++));Ȟ=bool.Parse(Θ(Η,Α++));ȗ=bool.Parse(Θ(Η,Α++));Ж=int.Parse(Θ(Η,Α++));Е=int.Parse(Θ(Η,Α++));С=int.Parse(Θ(Η,Α++));Ļ=float.Parse(Θ(Η,Α++));
ĺ=float.Parse(Θ(Η,Α++));ȱ=float.Parse(Θ(Η,Α++));ǭ=float.Parse(Θ(Η,Α++));Ǎ=int.Parse(Θ(Η,Α++));џ=int.Parse(Θ(Η,Α++));ġ=int.Parse(Θ(Η,Α++));Ģ=int.Parse(Θ(Η,Α++));Ƞ=int.Parse(Θ(Η,Α++));ж=int.Parse(Θ(Η,Α++));Ƌ=int.Parse(Θ(Η,Α++));ƀ=float.Parse(Θ(Η,Α++));}Η=Θ(Ǔ,3).Replace("\n","").Split('|');
н=Η.ToList();Η=Θ(Ǔ,4).Split('\n');π.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
π.Add(ΓΟ(Θ(Η,i)));Η=Θ(Ǔ,5).Split('|');Ů=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out İ)) İ=-1;ΒΨ(Θ(Ǔ,6));}catch{return Ч.Ʀ;}ƌ=ы;і();return Ч.ΑΛ;}const int Δο=64000;void ΓΖ(){њ.Clear();for(int i=Ђ.Count-1;i>=0;i--){String Υ=Ђ[i].CustomData.ToUpper();bool Ŷ=false;if(Υ=="INSTRUCTIONS"){Ŷ=true;Αβ=true;}
if(Υ=="DEBUG") Ŷ=true;if(Ŷ){њ.Add(Ђ[i]);Ђ.RemoveAt(i);}}Ȇ(њ,false,1,false);}void ΓΗ(List<IMyTerminalBlock>ΑΨ){Ю.Clear();for(int i=0;i<ΑΨ.Count;i++){IMyTerminalBlock Ϊ=ΑΨ[i];try{String ΑЃ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Ϊ.CustomName.IndexOf(ΑЃ);int Ǿ=-1;
if(Α<0||!int.TryParse(Ϊ.CustomName.Substring(Α+ΑЃ.Length,1),out Ǿ)) continue;if(Ǿ==-1) continue;Ǿ--;IMyTextSurfaceProvider ΑЅ=(IMyTextSurfaceProvider) Ϊ;if(Ǿ<ΑЅ.SurfaceCount&&Ǿ>=0)
Ю.Add(ΑЅ.GetSurface(Ǿ));}catch{}}}String Ƃ="";void ΒЈ(){String ž="";String Ȕ="";Ƃ=ΒΙ(false);ž+=Ƃ;Ȕ+=Ƃ;Ȕ+=Βκ();for(int i=0;i<Ђ.Count;i++) Ђ[i].WriteText(ž);for(int i=0;i<Ю.Count;i++) Ю[i].WriteText(ž);Echo(Ȕ);for(int i=0;i<њ.Count;i++){IMyTextPanel Ѕ=њ[i];String Υ=Ѕ.CustomData.ToUpper();
if(Υ=="DEBUG") Ѕ.WriteText(""+"\n"+"");if(Υ=="INSTRUCTIONS") Ѕ.WriteText(ƭ());}}string ƭ(){String Β="";try{float Αω=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ǧ*Αω)+" / "+(int)(Ǳ*Αω)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ǧ*100f,1)+"% / "+Math.Round(Ǳ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ȍ,2)+"ms / "+Math.Round(Ɓ,2)+"ms \n";}catch{}for(int i=0;i<ı.Count;i++)
Β+=""+ı.Keys.ElementAt(i)+" = "+Math.Round(ı.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ı.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Βς(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ȇ(Ђ,setLCDFontAndSize,0.9f,false);Ȇ(Ю,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ђ.Count;i++) Ђ[i].WriteText(Β+ǯ.ToString());for(int i=0;i<Ю.Count;i++) Ю[i].WriteText(Β+ǯ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ΑΖ(String Ј,String α,String ќ){try{if(!ȏ) return;Ј=BROADCAST_TAG+BROADCAST_SEP+ƪ()+BROADCAST_SEP+α+BROADCAST_SEP+ũ+BROADCAST_SEP+ќ+BROADCAST_SEP+Ј;this.IGC.SendBroadcastMessage(BROADCAST_TAG,Ј);}catch(Exception e){ǯ=e;}}String ƪ(){if(Ν!=Μ.Ћ) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ƾ(String Ј){return Ј.StartsWith(BROADCAST_TAG);}bool ό(ref String Υ,out string Ș,Char ǔ){int Α=Υ.IndexOf(ǔ);Ș="";if(Α<0) return false;Ș=Υ.Substring(0,Α);Υ=Υ.Remove(0,Α+1);return true;}bool ΒЅ(ref String Ј,out String я,out String ќ){я="";ќ="";if(!ȏ) return false;
String О="";if(!ό(ref Ј,out О,BROADCAST_SEP)||!ƾ(О)) return false;if(!ό(ref Ј,out я,BROADCAST_SEP)) return false;if(!ό(ref Ј,out О,BROADCAST_SEP)||(О!=ƪ()&&(О!="*"&&Ν!=Μ.Ћ))) return false;if(!ό(ref Ј,out О,BROADCAST_SEP)||(О!=ũ)) return false;if(!ό(ref Ј,out ќ,BROADCAST_SEP)) return false;
return true;}void Βφ(){bool ȑ=true;if(Αι){Αι=false;if(Me.CustomData.Contains("Antenna_Name")){Φ="Update custom data";Me.CustomData="";}}String ț=(Ν!=Μ.Ћ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ț)
ƻ(ț);String[]Η=Me.CustomData.Split('\n');ȏ=bool.Parse(ĉ(Ą(Η,"Enable_Broadcast",ref ȑ)));bool Ɯ=false;bool Αδ=true;if(ȏ){if(Ν!=Μ.Ћ)
ȧ=ĉ(Ą(Η,"Ship_Name",ref ȑ)).Replace(BROADCAST_SELF_ID,"");ũ=ĉ(Ą(Η,"Broadcast_Channel",ref ȑ)).ToLower();Αδ=false;if(е==null){е=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);е.SetMessageCallback("");}List<IMyRadioAntenna>ǥ=new List<IMyRadioAntenna>();и.GetBlocksOfType(ǥ);bool ť=false;
for(int i=0;i<ǥ.Count;i++)
if(ǥ[i].EnableBroadcasting&&ǥ[i].Enabled){ť=true;break;}if(ǥ.Count==0) Φ="No Antenna found";else if(!ť) Φ="Antenna not ready";Ɯ=ǥ.Count==0||!ť;if(ΑΡ&&!Ɯ&&Ν!=Μ.Ћ) Φ="Antenna ok";}else if(Ν==Μ.Ћ)
Φ="Offline - Enable in PB custom data";ΑΡ=Ɯ;if(Αδ){if(е!=null) this.IGC.DisableBroadcastListener(е);е=null;}}catch{ȑ=false;}if(!ȑ){Φ="Reset custom data";ƻ(ț);}}void ƻ(String ț){String Υ=ț+"\n\n"+"Enable_Broadcast="+(Ν==Μ.Ћ?"true":"false")+" \n";Υ+=Ν!=Μ.Ћ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Υ+"Broadcast_Channel=#default";}String ĉ(String Ǐ){int Α=Ǐ.IndexOf("//");if(Α!=-1) Ǐ=Ǐ.Substring(0,Α);String[]έ=Ǐ.Split('=');if(έ.Length<=1) return "";return έ[1].Trim();}String Ą(String[]Η,String ΓϏ,ref bool Ɔ){foreach(String Ǐ in Η)
if(Ǐ.StartsWith(ΓϏ)) return Ǐ;Ɔ=false;return "";}void ΒΣ(string І){if(ș) ψ=new List<ο>();String я="";String ќ="";if(!ƾ(І)) ΒЉ(І);bool ΑЌ=false;int ΓЊ=50;while(е!=null&&е.HasPendingMessage&&ΓЊ-->0){MyIGCMessage Ј=е.AcceptMessage();String Υ=Ј.Data as String;if(Υ==null) continue;
if(ΒЅ(ref Υ,out я,out ќ)&&я!=""&&я!=BROADCAST_SELF_ID){ο Γ=Ņ(я,"");if(Γ==null){Γ=new ο(я);ψ.Add(Γ);}Γ.Ǣ(ќ,false,false,true);ΓΣ(Γ,Υ);ΑЌ=true;}}if(ΑЌ)
ψ.Sort(delegate(ο a,ο b){if(a.γ==null&&b.γ==null) return 0;else if(a.γ==null) return-1;else if(b.γ==null) return 1;else return a.γ.CompareTo(b.γ);});if(ȟ||ș){if(Ĳ<=0||ș){Φ="";ș=false;ΒЏ();Βφ();Ατ=DateTime.Now;}}ΓΡ();}void ΓΣ(ο Γ,String Υ){Γ.Ź=DateTime.Now;String Ș="";
ό(ref Υ,out Γ.ĸ,BROADCAST_SEP);ό(ref Υ,out Γ.γ,BROADCAST_SEP);if(Γ.ĸ!=VERSION) return;String Ȥ="";String[]Ƕ=new string[3];ό(ref Υ,out Ȥ,BROADCAST_SEP);ό(ref Υ,out Ƕ[0],BROADCAST_SEP);ό(ref Υ,out Ƕ[1],BROADCAST_SEP);ό(ref Υ,out Ƕ[2],BROADCAST_SEP);ό(ref Υ,out Γ.Ɣ,BROADCAST_SEP);ό(ref Υ,out Γ.Φ,BROADCAST_SEP);
ό(ref Υ,out Ș,BROADCAST_SEP);ό(ref Υ,out Γ.Ĭ,BROADCAST_SEP);String ΒΓ="",Αϋ="";ό(ref Υ,out ΒΓ,BROADCAST_SEP);ό(ref Υ,out Αϋ,BROADCAST_SEP);Γ.й.Clear();while(true){String Αρ;if(!ό(ref Υ,out Αρ,BROADCAST_SEP)) break;String[]Ħ=Αρ.Split('/');if(Ħ.Count()<3) continue;int Ѐ=0;if(!int.TryParse(Ħ[2],out Ѐ)) continue;
Γ.й.Add(new б(Ħ[0],Ħ[1],Ѐ,Ć.Βσ));}int Αύ=0;if(!int.TryParse(Ș,out Γ.ι)) Γ.ι=0;if(!int.TryParse(Ȥ,out Αύ)) Γ.Ν=Μ.ф;if(!float.TryParse(Ƕ[0],out Γ.Ρ.X)) Γ.Ρ.X=0;if(!float.TryParse(Ƕ[1],out Γ.Ρ.Y)) Γ.Ρ.Y=0;if(!float.TryParse(Ƕ[2],out Γ.Ρ.Z)) Γ.Ρ.Z=0;if(!float.TryParse(ΒΓ,out Γ.ј)) Γ.ј=0;
if(!float.TryParse(Αϋ,out Γ.ǐ)) Γ.ǐ=0;Γ.Ν=(Μ) Αύ;Γ.ŭ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Ρ));Γ.ƙ=0;for(int i=0;i<Γ.й.Count();i++) Γ.ƙ+=Γ.й[i].Ѐ;}ο Ņ(String Ё,String γ){Ё=Ё.ToUpper();γ=γ.ToUpper();for(int i=0;i<ψ.Count;i++){if(Ё!=""&&ψ[i].Ё.ToUpper()==Ё) return ψ[i];
if(γ!=""&&ψ[i].γ!=null&&ψ[i].γ.ToUpper()==γ) return ψ[i];}return null;}void Г(ο Γ,String χ){if(Γ==null){for(int i=0;i<ψ.Count;i++)
ψ[i].Ǣ(χ,true,false,false);ΑΖ(χ,"*",χ);}else{Γ.Ǣ(χ,true,false,false);ΑΖ(χ,Γ.Ё,χ);}}void ΒЉ(string І){if(І=="") return;var έ=І.ToUpper().Split(' ');έ.DefaultIfEmpty("");var χ=έ.ElementAtOrDefault(0);var ΑΤ=έ.ElementAtOrDefault(1);String Γϋ="Invalid argument: "+І;if(Ϋ==Ι.ȇ){if(κ!=null)
{switch(χ){case "UP":{if(ι<2) break;if(κ.ι==0){ι=1;return;}Г(κ,"UP");return;}case "DOWN":{if(ι<1) break;if(ι==1){Г(κ,"MRES");break;}Г(κ,"DOWN");break;}case "APPLY":{if(ι<2) break;Г(κ,"APPLY");return;}}}}switch(χ){case "UP":this.Βϋ(false);return;case "DOWN":this.Βϊ(false);return;
case "APPLY":this.ǝ(true);return;}switch(χ){case "CLEAR":ΒΠ();return;case "SENDALL":
if(String.IsNullOrEmpty(ΑΤ)){Φ="Usage: SENDALL <cmd>";return;}Г(null,ΑΤ);return;case "SEND":
if(І.Length<="SEND".Length+1){Φ="Usage: SEND <ship>:<cmd>";return;}ΒА(І.Remove(0,"SEND".Length+1));return;}Φ=Γϋ;}void ΒА(String ΑΣ){if(ΑΣ=="") return;var έ=ΑΣ.Split(':');if(έ.Length!=2){Φ="Missing separator \":\"";return;}έ.DefaultIfEmpty("");String ȧ=έ.ElementAtOrDefault(0).Trim();
ο Γ=Ņ("",ȧ);if(Γ!=null) Г(Γ,έ.ElementAtOrDefault(1).Trim());else Φ="Unknown ship: "+ȧ;}void ΒΠ(){ψ.Clear();κ=null;Τ(Ι.Main);ι=0;}void ΒЏ(){List<IMyTerminalBlock>ΑЍ=new List<IMyTerminalBlock>();и.GetBlocksOfType(Ђ,ō);и.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑЍ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ђ=Ƨ(Ђ,pamTag,true);ΓΗ(ΑЍ);ΓΖ();Ȇ(Ђ,setLCDFontAndSize,1.15f,false);Ȇ(Ю,setLCDFontAndSize,1.15f,true);String Γχ=Ʈ(false);String ΑϏ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ѕ in Ђ){if(Ѕ.CustomData==""){Ѕ.CustomData=Ʈ(true);continue;}
if(!Ѕ.CustomData.Contains(Γχ))
if(!Ѕ.CustomData.Contains(ΑϏ)) Ѕ.CustomData=ΑϏ+Ѕ.CustomData;}}void ΓΡ(){String Ȕ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ю.Count;i++)
Ю[i].WriteText(Ơ(ς.ǀ,"0",false));for(int i=0;i<Ђ.Count;i++){ς ΰ=ς.ф;String Н="";ΒІ(Ђ[i],out ΰ,out Н);Ђ[i].WriteText(Ơ(ΰ,Н,i==0));}Echo(Ȕ);for(int i=0;i<њ.Count;i++){IMyTextPanel Ѕ=њ[i];String Υ=Ѕ.CustomData.ToUpper();if(Υ=="DEBUG") Ѕ.WriteText(""+"\n"+"");if(Υ=="INSTRUCTIONS") Ѕ.WriteText(ƭ());
}}String ǝ(bool Ε){int Γμ=0;return ǝ(Ε,0,ref Γμ,false,1);}String ǝ(bool Ε,int ΓА,ref int ΔΛ,bool ΑΫ,int ȥ){String Β="";String Ǽ="——————————————————\n";String ΔΩ="--------------------------------------------\n";if(κ==null||Ϋ==Ι.Main||Ϋ==Ι.ś)
Β+="[PAM]-Controller | "+ψ.Count+" Connected ships"+"\n";else Β+=Ʋ(κ)+"\n";Β+=Ǽ;int Α=0;int Δ=ι;Ǒ=0;if(Φ!=""){Ζ(ref Β,Δ,Α++,Ε,Φ);}else if(Ϋ==Ι.Main){if(κ==null&&ψ.Count>=1) κ=ψ[0];String Ϗ="";if(Ζ(ref Ϗ,Δ,Α++,Ε," Send command to all")) Τ(Ι.ś);int ǻ=0;for(int s=0;s<ψ.Count;s++)
{if(ΑΫ) Ϗ+="\n";ο Γ=ψ[s];if(Δ==Α||Δ==Α+1) κ=Γ;if(Α==Δ) ǻ=Ϗ.Split('\n').Length-1;if(Ζ(ref Ϗ,Δ,Α++,Ε," "+Ʋ(Γ))){κ=Γ;Τ(Ι.ȇ);κ.Ǣ("",false,true,false);}if(Α==Δ) ǻ=Ϗ.Split('\n').Length-1;if(Ζ(ref Ϗ,Δ,Α++,Ε," "+(ΑΫ?"":"  ")+Βξ(Γ))){κ=Γ;Τ(Ι.ΑΗ);}}int Ȁ=ȥ-2;
Ȁ+=Math.Max(0,(ΓА-1))*ȥ;Β+=Ś(Ȁ,Ϗ,ǻ,ref ΔΛ);}else if(Ϋ==Ι.Ċ||Ϋ==Ι.ś){Ι ΓΫ=Ϋ==Ι.Ċ?Ι.ȇ:Ι.Main;ο α=null;if(Ϋ==Ι.Ċ) α=κ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Τ(ΓΫ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǟ(α,"STOP"))){Г(α,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǟ(α,"CONT"))){Г(α,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǟ(α,"HOMEPOS"))){Г(α,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǟ(α,"JOBPOS"))){Г(α,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǟ(α,"FULL"))){Г(α,"FULL");}if(α!=null&&α.Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǟ(α,"UNDOCK"))){Г(α,"UNDOCK");}}else if(Ϋ==Ι.ȇ){String ŧ="";if(Ζ(ref ŧ,Δ,Α++,Ε," Back")){Τ(Ι.Main);}Β+=ŧ.Substring(0,ŧ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Τ(Ι.Ċ);}if(!κ.ƿ()&&!κ.Ō()) Ǒ++;if(κ.Ō()) Β+="No answer received...";
else Β+=ǣ(κ,κ.Ĭ);}else if(Ϋ==Ι.ΑΗ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Τ(Ι.Main);}Β+=ΔΩ;Β+=ǣ(κ,Ʊ(κ));}if(!ƃ) ι=Math.Min(Ǒ-1,ι);return Β;}String Ʋ(ο Γ){if(Γ.ĸ!=VERSION) return Γ.γ+": Different version!";return Γ.γ+": "+ǣ(Γ,Γ.Ɣ)+" "+Γ.ŭ+"m";}String Βξ(ο Γ){String s=ŗ("",В(Γ.ј,Γ.ǐ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.й.Count;i++){if(i>=5) break;б Щ=Γ.й[i];s+=ΑΟ(ŀ(Βλ(Щ)),3)+" "+ƨ(Щ.Ѐ)+" ";}return s;}String Βλ(б Щ){if(Щ.Ѓ.ToUpper()=="ORE"||Щ.Ѓ.ToUpper()=="INGOT"){String ΑΩ=Βι(Щ.γ.ToUpper());if(ΑΩ!="") return ΑΩ;}return Щ.γ;}String ǣ(ο Γ,String Β){if(Γ.ƿ()) return "No signal...("+ň((int)(DateTime.Now-Γ.Ź).TotalSeconds)+")";
return Β;}String Ʈ(bool Δθ){String Β="";if(Δθ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΒІ(IMyTerminalBlock Ѕ,out ς ΰ,out String Н){bool Ɔ=true;String Ȥ=ĉ(Ą(Ѕ.CustomData.Split('\n'),"mode",ref Ɔ)).ToUpper();String[]έ=Ȥ.Split(':');String ȣ=έ.Length>0?έ[0].Trim():"";Н=έ.Length>1?έ[1].Trim():"";
ΰ=ς.ф;if(ȣ=="MAIN") ΰ=ς.ǁ;else if(ȣ=="MAINX") ΰ=ς.ǂ;else if(ȣ=="MENU") ΰ=ς.Ő;else if(ȣ=="INVENTORY") ΰ=ς.ŋ;else if(ȣ=="DEBUG") ΰ=ς.ł;if(ΰ==ς.ф) Н=Ȥ;}String Ơ(ς ΰ,String Н,bool Γο){int ȡ=15;if(Γο){Αά=ė;ė=0;Αέ=Ę;Ę=0;}String Β="";if(ΰ==ς.ǁ){int Є=0;Β=ǝ(false,Αά,ref Γε,true,ȡ);
if(!int.TryParse(Н,out Є)) return Β;ė=Math.Max(Є,ė);Є--;return ΑΑ(Є,ȡ,Β);}else if(ΰ==ς.ǂ){int Є=0;Β=ǝ(false,Αέ,ref Γζ,false,ȡ);if(!int.TryParse(Н,out Є)) return Β;Ę=Math.Max(Є,Ę);Є--;return ΑΑ(Є,ȡ,Β);}if(ΰ==ς.ǀ){return ǝ(false,0,ref Γη,true,ȡ);}else if(ΰ==ς.ŋ||ΰ==ς.Ő||ΰ==ς.ł)
{ο Γ=κ;if(Н!=""){Γ=Ņ("",Н);if(Γ==null) return "Unknown ship: "+Н;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ǽ="—————————————————————\n";String Ƙ="";String Ũ="";if(ΰ==ς.Ő){Ƙ="Menu";Ũ=ǣ(Γ,Γ.Ĭ);}else if(ΰ==ς.ŋ){Ƙ="Inventory";Ũ=ǣ(Γ,Ʊ(Γ));}
else if(ΰ==ς.ł){return ǣ(Γ,Βο(Γ));}Β+=Γ.γ+" - "+Γ.ŭ+"m | "+ŀ(""+Ƙ)+"\n"+Ǽ;Β+=Ũ;return Β;}return "Unknown command: "+Н;}string Βο(ο Γ){String s="";s+=Γ.ĸ+"\n";s+=Γ.γ+"\n";s+=Γ.Ρ.X+"\n";s+=Γ.Ρ.Y+"\n";s+=Γ.Ρ.Z+"\n";s+=Γ.Ɣ+"\n";s+=Γ.Φ+"\n";s+=Γ.ι+"\n";
s+=Γ.Ĭ.Length+"\n";s+=Γ.ј+"\n";s+=Γ.ǐ+"\n";s+=Γ.й.Count()+"\n";return s;}string Ʊ(ο Γ){String s="";s+=ŗ("All",В(Γ.ј,Γ.ǐ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.й.Count();i++){б Щ=Γ.й[i];s+=ŗ(Щ.γ,Щ.Ѐ,Γ.ƙ,30,8,10)+"\n";}return s;}ρ Βε(ο Γ,bool Ȑ,string χ)
{ρ Ψ=ρ.None;if(Γ==null){for(int i=0;i<ψ.Count;i++){ρ φ=ψ[i].Ǣ(χ,false,Ȑ,false);if(φ==ρ.Ř) Ψ=φ;if(φ==ρ.œ) return φ;if(φ==ρ.ȅ) return φ;}return Ψ;}else return Γ.Ǣ(χ,false,Ȑ,false);}String ǟ(ο Γ,String χ){ρ φ=Βε(Γ,false,χ);if(φ==ρ.Ř) return "received!";if(φ==ρ.ȅ) return "pending...";
if(φ==ρ.œ) return "no answer!";return "";}String Ś(int Ȁ,String Β,int ǻ,ref int л){String[]Η=Β.Split('\n');if(ǻ>=л+Ȁ-1) л++;л=Math.Min(Η.Count()-1-Ȁ,л);if(ǻ<л+1) л--;л=Math.Max(0,л);String Ψ="";for(int i=0;i<Ȁ;i++){int Αφ=i+л;if(Αφ>=Η.Count()) break;Ψ+=Η[Αφ]+"\n";
}return Ψ;}String ΑΑ(int Є,int ȥ,String Β){String[]Η=Β.Split('\n');int ΔΥ=Є*ȥ;int Γξ=(Є+1)*ȥ;String Ψ="";for(int i=ΔΥ;i<Γξ;i++){if(i>=Η.Count()) break;Ψ+=Η[i]+"\n";}return Ψ;}String ŀ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();
}String ΑΟ(String s,int Αϊ){if(s=="") return s;if(s.Length>Αϊ) s=s.Substring(0,Αϊ-1)+".";return s;}string ŗ(String ǎ,float value,float ΓІ,int ΑΥ,int Ασ,int ΓЏ){float ǒ=В(value,ΓІ)*ΑΥ;String Ĕ="[";for(int i=0;i<ΑΥ;i++){if(i<=ǒ) Ĕ+="|";else Ĕ+="'";}Ĕ+="]";return Ĕ+" "+ΑΟ(ŀ(ǎ),Ασ).PadRight(Ασ)+"".PadRight(ΓЏ)+ƨ(value);
}String ƨ(float Ѐ){if(Ѐ>=1000000) return Math.Round(Ѐ/1000000f,Ѐ/1000000f<100?1:0)+"M";if(Ѐ>=1000) return Math.Round(Ѐ/1000f,Ѐ/1000f<100?1:0)+"K";return ""+Math.Round(Ѐ);}String ň(int Ȧ){if(Ȧ>=60*60) return Math.Round(Ȧ/(60f*60f),1)+" h";if(Ȧ>=60) return Math.Round(Ȧ/60f,1)+" min";
return ""+Ȧ+" s";}List<IMyCameraBlock>ΔЅ=new List<IMyCameraBlock>();List<long>ΕΞ=new List<long>();bool ΓБ=false;int Γΰ=0;int ΔЄ=0;DateTime ΔЂ=DateTime.Now;String Γή="";void ΒΟ(){ΓБ=false;Γΰ=0;Γή="";}const String Δχ="[PAMTFC]";const float Δψ=5f;const float Δς=2f;class ΑΜ
{public long Ё;public Vector3 δ,Εν;public float ΔЊ;public String ΕΕ="";public int ΑЉ=-1;public long И=0;public int ΔЍ=0;public DateTime Εα;}IMyBroadcastListener Εγ=null;long ΕΜ=0;List<ΑΜ>Εβ=new List<ΑΜ>();Vector3 Εδ=new Vector3();float Εε=-1;int Εζ=0;String Ů="";int İ=-1;bool Γθ=false;
Vector3 ΔЇ=new Vector3();class ǡ{public long Ё;public Vector3 δ,Ў,ϋ;}List<ǡ>Ї=new List<ǡ>();bool ȓ=false;Vector3 Ğ,ĝ,ğ,Ġ;long Ȝ=0;long ΑЂ=0;Χ ΑЁ=null;bool Γκ=false;Χ ΒΪ(Vector3 δ,Vector3 Ў,Vector3 ϋ){MatrixD m=MatrixD.CreateWorld(δ,Ў,ϋ);Χ w=new Χ(Ξ);w.ώ=Vector3D.TransformNormal(Ğ,m);
w.ω=Vector3D.TransformNormal(ĝ,m);w.Џ=Vector3D.TransformNormal(ğ,m);w.Ρ=δ+(Vector3) Vector3D.TransformNormal(Ġ,m);w.β=true;return w;}void Βω(){if(Ν==Μ.ζ||!Ξ.β) return;IMyShipConnector ǧ=Ǟ(MyShipConnectorStatus.Connected);if(ǧ==null||ǧ.OtherConnector==null) return;IMyShipConnector Ќ=ǧ.OtherConnector;
Vector3I Γψ=new Vector3I((int) Ξ.З.X,(int) Ξ.З.Y,(int) Ξ.З.Z);if(ǧ.Position==Γψ){MatrixD ĥ=MatrixD.Transpose(Ќ.WorldMatrix);Ğ=Vector3D.TransformNormal(Љ.WorldMatrix.Forward,ĥ);ĝ=Vector3D.TransformNormal(Љ.WorldMatrix.Down,ĥ);ğ=Vector3D.TransformNormal(Љ.WorldMatrix.Left,ĥ);Ġ=Vector3D.TransformNormal(ǧ.GetPosition()-Ќ.GetPosition(),ĥ);
ȓ=true;}if(Vector3.Distance(ǧ.GetPosition(),Ξ.Ρ)<5) Ȝ=Ќ.EntityId;List<IMyShipConnector>ѝ=new List<IMyShipConnector>();и.GetBlocksOfType(ѝ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(ѝ.Count>0){Ї.Clear();for(int i=0;i<ѝ.Count;i++){ǡ p=new ǡ();
p.Ё=ѝ[i].EntityId;p.δ=ѝ[i].GetPosition();p.Ў=ѝ[i].WorldMatrix.Forward;p.ϋ=ѝ[i].WorldMatrix.Up;Ї.Add(p);}}ΑЁ=null;ΑЂ=0;if(Ќ.EntityId==Ȝ||!ȓ) return;for(int i=0;i<Ї.Count;i++)
if(Ї[i].Ё==Ќ.EntityId){ΑЂ=Ї[i].Ё;ΑЁ=ΒΪ(Ї[i].δ,Ї[i].Ў,Ї[i].ϋ);}}String ΒΩ(){String s=(ȓ?"1":"0")+":"+ί(Ğ)+":"+ί(ĝ)+":"+
ί(ğ)+":"+ί(Ġ)+":"+Ȝ;for(int i=0;i<Ї.Count;i++)
s+=":"+Ї[i].Ё+","+ί(Ї[i].δ)+","+ί(Ї[i].Ў)+","+ί(Ї[i].ϋ);return s;}void ΒΨ(String s){Ї.Clear();ȓ=false;Ȝ=0;String[]f=s.Split(':');if(f.Length<6) return;ȓ=f[0]=="1";Ğ=ή(f[1]);ĝ=ή(f[2]);ğ=ή(f[3]);Ġ=ή(f[4]);long.TryParse(f[5],out Ȝ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǡ p=new ǡ();if(!long.TryParse(e[0],out p.Ё)) continue;p.δ=ή(e[1]);p.Ў=ή(e[2]);p.ϋ=ή(e[3]);Ї.Add(p);}}void ΓΘ(string І){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}