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
public enum Μ{я,ų,Ǯ,Ѝ,ζ}public enum Λ{џ,Ŧ,Αΰ,ź,Αλ,Ȓ,э,
ť,ΓΙ,Ǫ,Ǌ,Βω,Ǆ,
Γω,ș,Ș,ş,ΔΗ,ΓΔ,
ΔΕ,ΔΖ,ΓΕ,ΓΖ,Ț}public enum Ο{н,Ŷ,ў,ǋ,Ė,Αα,Αβ,Ē,ē}public enum щ{None,ş,џ,ΒЋ}public enum Ι{Main,Αι,Ȕ,ȓ,Ɓ,ĕ,
ś,ΑΣ,ΑΤ,ΑΡ,ȗ,Αξ,
Ğ,ż,ſ,Απ,Ģ}public enum ю{Αμ,ΒЇ,ΓЊ,Γα,ζ}public enum Я{ģ,ΒЃ}public enum ь{Ακ,ΓΘ,ΓЌ,Ū}public enum Х{Αδ,ΒЊ,ΒЉ,ΒБ,ΒА,Γί,Γή}public enum к{Ţ,Βό,ǅ}public enum Ě{Γέ,Εθ,Βϊ}public enum Ё{Αε,Αζ,Αη,Γο,Γξ,
Γι,Γκ,Γθ,Γμ,
Γν,Γλ}public enum Π{Э,о,Ь,Ĕ,Ŝ,ŝ}public enum Εκ{Εη,Εβ,Εε}public enum Ю{Ασ,ǌ,Γη,Αγ}public enum ς{None,ȕ,Ź,Ŵ}public enum λ{ƃ,Ś,Ű,Ū,Αή,Ų,Αί,ΓБ,ŵ}public enum σ{я,Αά,ΑΫ,ű,ū,š,Αέ}class Χ{public bool β=false;public Vector3 Σ=new Vector3();
public Vector3 ύ=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 Г=new Vector3();public Vector3 η=new Vector3();public Vector3 Л=new Vector3();public float Δϊ=0;public float Ζν=0;public float[]У=null;public Χ(){}public Χ(Χ Џ){β=Џ.β;Σ=Џ.Σ;ύ=Џ.ύ;Ђ=Џ.Ђ;Г=Џ.Г;η=Џ.η;
Л=Џ.Л;У=Џ.У;}public Χ(Vector3 Σ,Vector3 Ђ,Vector3 ύ,Vector3 Г,Vector3 η){this.Σ=Σ;this.ύ=ύ;this.Ђ=Ђ;this.Г=Г;this.Δϊ=0;this.η=η;}public void Εδ(List<IMyThrust>ħ,List<string>ш){У=new float[ш.Count];for(int i=0;i<У.Length;i++)
У[i]=-1;for(int i=0;i<ħ.Count;i++){string Ǧ=ΓΩ(ħ[i]);int Α=ш.IndexOf(Ǧ);if(Α!=-1)
У[Α]=Ж(ħ[i].MaxEffectiveThrust,ħ[i].MaxThrust);}}}class Γς{public Χ α=null;public List<Vector3>Ζή=new List<Vector3>();public float ΕЀ=0;public float Ζρ=0;public float Εσ=0;public float ΕЉ=0;public Vector3 ΕЊ=new Vector3();}class Ĥ{public Ё Д=Ё.Αε;public float Ш=0;public float ǔ=0;
public string б="";public string г="";DateTime ΒΧ;public bool ΔІ=false;private bool ƍ=false;public bool ΒЄ(bool ƫ){if(ƫ){ǔ=0;ƍ=false;return false;}ƍ=true;return ǔ>Ш;}public void Ει(){ΒЄ(true);ΔІ=false;}public void Εο(){if(ƍ)
if((DateTime.Now-ΒΧ).TotalSeconds>1){ǔ++;ΒΧ=DateTime.Now;}}public bool ΑΟ(){switch(Д){case Ё.Αζ:return true;case Ё.Αη:return true;}return false;}}class м{public String γ="";public String Ї="";public int Є=0;public Ě ƞ=Ě.Βϊ;public м(String γ,String Ї,int Є,Ě ƞ){this.γ=γ;this.Ї=Ї;
this.Є=Є;this.ƞ=ƞ;}}class ΒЌ{public ΒЌ(Vector3 δ,float ђ){this.δ=δ;this.ђ=ђ;}public Vector3 δ;public float ђ;}class ο{public DateTime ƛ;public DateTime ΑЅ;public String ψ="";public String γ="";public String ŕ="";public String Φ="";public String ƶ="";public String ŉ="";public String Ʃ="";
public Vector3 Σ=new Vector3();public List<м>т=new List<м>();public Μ Ν=Μ.я;public ς ё;public float Ǜ;public float ǐ;public int ι;public int Ə=0;public int ƻ=0;public bool ΑΪ(){return(DateTime.Now-ƛ).TotalSeconds>10;}public bool Ŭ(){if(ё!=ς.ȕ) return false;return(DateTime.Now-ΑЅ).TotalSeconds>=4;
}public ο(String ψ){this.ψ=ψ;}public ς ǰ(String ϋ,bool ΕΑ,bool ȡ,bool ΔΡ){if(ϋ==""&&!ȡ) return ς.None;if(ё==ς.ȕ&&Ŭ())
ё=ς.Ŵ;if(ΕΑ){Ʃ=ϋ;ё=ς.ȕ;ΑЅ=DateTime.Now;return ё;}else if(ȡ){Ʃ="";ё=ς.None;}else if(Ʃ==ϋ){if(ΔΡ) ё=ς.Ź;return ё;}return ς.None;}}public String ΓΠ(String Δς){switch(Δς){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem В;Vector3 Βθ=new Vector3();Vector3 Ы=new Vector3();Vector3 Δγ=new Vector3();Vector3 Δυ=new Vector3();Vector3 Δέ=new Vector3();DateTime ΒΦ=new DateTime();
bool Ȫ=true;int ƹ=0;bool Ȳ=false;bool Ł=false;bool Δώ=true;bool Ζτ=false;bool ΕЏ=false;bool Δξ=false;float ΕΓ=0;float Βξ=0;int ΔЌ=0;int Ζξ=0;int ŏ=0;int Εω=0;float Ǿ=0;float Ǵ=0;double ƣ=0;float Ȝ=0;List<int>Ǻ=new List<int>();List<int>ȉ=new List<int>();Μ Ν=Μ.я;
int Ư=0;float ǥ=0;float Ǥ=0;float ȍ=0;float Βν=0,Βμ=0;IMyRemoteControl І;IMySensorBlock φ;List<IMyTimerBlock>Βσ=new List<IMyTimerBlock>();List<IMyShipConnector>ȣ=new List<IMyShipConnector>();List<IMyThrust>ċ=new List<IMyThrust>();List<IMyGyro>ΒΚ=new List<IMyGyro>();List<IMyTerminalBlock>Ф=new List<IMyTerminalBlock>();
List<IMyLandingGear>ƚ=new List<IMyLandingGear>();List<IMyReactor>ƪ=new List<IMyReactor>();List<IMyConveyorSorter>ΕΖ=new List<IMyConveyorSorter>();List<IMyGasTank>Ȱ=new List<IMyGasTank>();List<IMyTerminalBlock>ț=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Εό=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ες=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Αύ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ѕ=new List<IMyTextPanel>();List<IMyTextSurface>з=new List<IMyTextSurface>();List<IMyTextPanel>ǒ=new List<IMyTextPanel>();IMyTerminalBlock њ=null;Χ Ξ=new Χ();Χ ΒΞ=new Χ();
Χ ΒΣ=new Χ();bool ѝ=false;bool Ď=false;bool ď=false;bool ΒΠ=false;List<String>ш=new List<string>();List<Χ>ρ=new List<Χ>();List<Χ>Ŗ=new List<Χ>();int ΕЁ=0;double ΕΩ=0;Χ Τ=new Χ();Vector3 Ă;Vector3 ǿ;Ο μ=Ο.н;Я ǻ=Я.ģ;int ω=0;int Ў=0;double Ń=0;bool ΑЋ=false;Ĥ ν=new Ĥ();
Ĥ ξ=new Ĥ();ь љ=ь.Ακ;bool Đ=true;Я Ǣ=Я.ģ;int К=3;int Й=3;int Ч=30;к с=к.Ţ;bool Ǩ=true;bool ȱ=true;bool Ȩ=true;bool ǣ=false;Х ȧ=Х.Αδ;bool ќ=true;float д=90;float е=20;float П=5;float ж=20;float ѐ=0.70f;float Ř=1.50f;float ŗ=2.50f;float đ=0f;float ȭ=0f;Λ Ĳ;
Ο ї;Λ ĉ;Γς ΖΛ=null;int ļ=0;int Ľ=0;int а=0;int Ā=0;int ƭ=30;int ΖΝ=0;bool ΕΘ=true;Vector3 ΖΗ;double ΑА=0;bool Βκ=false;int ΕΪ=0;int ΖΩ=0;int Ετ=0;int ΖΚ=0;int ΖΘ=0;Vector3 ΖΜ=new Vector3();float ΖΔ=0;float ΖΪ=0;int ǧ=0;Χ Ő=null;float ΖΕ=0;float Ευ=0;float ΑЊ=0;
float Ƣ=0;bool ΕА=false;bool Ζμ=false;bool Ζπ=false;bool ΕЍ=false;bool Εχ=false;DateTime ΕЈ=new DateTime();Χ Εώ=null;Χ ΔΫ=null;float Αώ=0;щ ȝ;float ΒΟ=0;float Βφ=0;String Δψ="";float Ɲ=0;float ǐ=0;float Ǜ=0;List<м>ΕБ=new List<м>();float ώ=0;bool Ʊ=false;bool ΕΝ=false;
bool ΕΔ=false;bool Βς=false;bool ΕΕ=false;float Δω=0;float ΔЅ=0;Vector3 Δα=new Vector3();Vector3 Βΰ=new Vector3();Vector3 Ζα=new Vector3(1,1,1);float Ζί=1;Vector3 Ζΰ=new Vector3();bool ΕЎ=false;bool Δη=false;bool ΖΖ=false;bool Ερ=false;float Εύ=0;float Επ=2;Vector3 Ζέ;
Vector3 Ζά;Vector3 ΖΫ;float[,]č=new float[3,2];Dictionary<String,float[,]>ч=new Dictionary<string,float[,]>();Dictionary<String,float>Ζσ=new Dictionary<String,float>();int Εφ=0;Dictionary<String,float[]>Ŏ=new Dictionary<string,float[]>();float ΖΞ;bool ΑЍ=false;int Ĩ=0;int є=0;
int Ŀ=0;int ǘ=0;String Φ="";int[]Βή=new int[Enum.GetValues(typeof(Ι)).Length];bool ƥ=false;int ǜ=0;int ι=0;Ι ά=Ι.Main;int ΕΞ=0,ΕΟ=0,ΕΠ=0,ΕΡ=0;int ΔЍ=0,ΔЎ=0;bool ȟ=false;bool Αω=false;String ć="";String Ɖ="";IMyBroadcastListener п=null;bool ΒΕ=true;List<ο>ό=null;
ο κ=null;int Į=0,į=0;int ΑЇ=0,ΑЈ=0;int ΔΨ=0,ΔΩ=0,ΔΪ=0;String Δσ="";Exception Ǽ=null;bool Βι=false;Ο Ʈ=Ο.н;bool ΑЌ=false;bool ΖΓ=true;bool ΖΠ=false;String ΕΧ="";bool Βυ=false;bool Ƶ=true;int Ƙ=0;int[]ΖΣ=null;Vector3 Ɗ=new Vector3();bool ΒΘ=false;bool Ů(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;
Program(){В=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΑΠ(Me,controllerTag,true))
Ν=Μ.Ѝ;Υ(Ι.Main);if(Ν!=Μ.Ѝ){Φ="Welcome to [PAM]!";Ю Ƭ=Γχ();if(Ν==Μ.я){List<IMyShipDrill>ΒΓ=new List<IMyShipDrill>();List<IMyShipGrinder>ΒΙ=new List<IMyShipGrinder>();В.GetBlocksOfType(ΒΓ,q=>q.CubeGrid==Me.CubeGrid);В.GetBlocksOfType(ΒΙ,q=>q.CubeGrid==Me.CubeGrid);if(ΒΓ.Count>0)
{Ν=Μ.ų;Φ="Miner mode enabled!";}else if(ΒΙ.Count>0){Ν=Μ.Ǯ;Φ="Grinder mode enabled!";}else{Ν=Μ.ζ;Αο(ю.ζ);}}if(Ƭ==Ю.Ασ) Δώ=false;if(Ƭ==Ю.ǌ) Φ="Data restore failed!";if(Ƭ==Ю.Αγ) Φ="New version, wipe data";}}void Main(string Њ,UpdateType ΕΤ){try{if(Ǽ!=null){if(Њ.Trim().ToUpper()=="RETRY")
{Ǽ=null;Ȫ=true;ΔЌ=0;if(Ν!=Μ.Ѝ) ǎ();Φ="Restarted after error";}else{Γά();Γτ();return;}}Ł=(ΕΤ&UpdateType.Update10)!=0;if(Ł) ƹ++;Ȳ=ƹ>=10;if(Ȳ) ƹ=0;if(Ł){Ĩ++;if(Ĩ>4) Ĩ=0;ŏ=(int) Math.Max(0,10-(DateTime.Now-ΒΦ).TotalSeconds);}if(Њ!="") Δσ=Њ;if(Ν!=Μ.Ѝ)
ΓЉ(Њ);else
ΒЈ(Њ);ƥ=false;try{int ΑЉ=Runtime.CurrentInstructionCount;float ǟ=Ж(ΑЉ,Runtime.MaxInstructionCount);if(ǟ>0.90) Φ="Max. instructions >90%";if(ǟ>Ǿ) Ǿ=ǟ;if(ΑЍ){Ǻ.Add(ΑЉ);while(Ǻ.Count>10) Ǻ.RemoveAt(0);Ǵ=0;for(int i=0;i<Ǻ.Count;i++) Ǵ+=Ǻ[i];Ǵ=Ж(Ж(Ǵ,Ǻ.Count),Runtime.MaxInstructionCount);
double Ɯ=Runtime.LastRunTimeMs;if(Δξ&&Ɯ>ƣ) ƣ=Ɯ;ȉ.Add((int)(Ɯ*1000f));while(ȉ.Count>10) ȉ.RemoveAt(0);Ȝ=0;for(int i=0;i<ȉ.Count;i++) Ȝ+=ȉ[i];Ȝ=Ж(Ȝ,ȉ.Count)/1000f;}}catch{Ǿ=0;}}catch(Exception e){Ǽ=e;}}String ΓΡ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Υ(Ι Ј){Βή[(int) ά]=ι;ι=Βή[(int) Ј];if(Ј==Ι.ȗ) ι=0;ά=Ј;if(Ν!=Μ.Ѝ)
Ħ(ά==Ι.Ȕ,false,0,0);ƥ=true;}void Γζ(bool Ɵ){if(ι>0) ι--;else if(Ɵ) ι=ǜ-1;}void Γε(bool Ɵ){if(ι<ǜ-1) ι++;else if(Ɵ) ι=0;}String Αο(ю π){switch(π){case ю.Αμ:Φ="Job is running";break;case ю.ΒЇ:Φ="Connector not ready!";break;case ю.ΓЊ:Φ="Ship modified, path outdated!";break;
case ю.Γα:Φ="Interrupted by player!";break;case ю.ζ:Φ="Shuttle mode enabled!";break;}return "";}String ΓΨ(Я δ){switch(δ){case Я.ģ:return "Top-Left";case Я.ΒЃ:return "Center";default:return "";}}String ΓΝ(к ΰ){switch(ΰ){case к.Βό:return "Auto"+(Ν==Μ.ų?" (Ore)":"");case к.ǅ:return "Auto (+Stone)";
case к.Ţ:return "Default";default:return "";}}String ΓΥ(Ο ϊ){switch(ϊ){case Ο.н:return "No job";case Ο.Ŷ:return "Job paused";case Ο.ў:return "Job active";case Ο.Ē:return "Job active";case Ο.ē:return "Job active";case Ο.ǋ:return "Job done";case Ο.Ė:return "Job changed";case Ο.Αα:return "Move home";
case Ο.Αβ:return "Move to job";}return "";}String ΑΕ(ь ΔΞ){switch(ΔΞ){case ь.Ακ:return "Return home";case ь.ΓΘ:return "Fly to job pos";case ь.ΓЌ:return "Stop";case ь.Ū:return "Ignore";}return "";}String ΓΟ(Х ΰ){switch(ΰ){case Х.Αδ:return "Off";case Х.ΒБ:return "Drop pos (Stone) ";case Х.ΒА:return "Drop pos (Sto.+Ice)";
case Х.ΒЊ:return "Cur. pos (Stone)";case Х.ΒЉ:return "Cur. pos (Sto.+Ice)";case Х.Γί:return "In motion (Stone)";case Х.Γή:return "In motion (Sto.+Ice)";}return "";}String ΑΓ(щ ϊ){switch(ϊ){case щ.None:return "No batteries";case щ.ş:return "Charging";case щ.ΒЋ:return "Discharging";}return "";}
String ΑΙ(Λ ϊ){String ΖΒ=Ν==Μ.ζ?"target":"job";switch(ϊ){case Λ.џ:return "Idle";case Λ.Ŧ:return "Flying to XY position";case Λ.Αΰ:return Ν==Μ.Ǯ?"Grinding":"Mining";case Λ.ź:return "Returning";case Λ.ΓΕ:return "Flying to drop pos";case Λ.Αλ:return "Returning to dock";case Λ.Ȓ:return "Flying to dock area";
case Λ.э:return "Flying to job area";case Λ.ť:return "Flying to path";case Λ.ΓΙ:return "Flying to job position";case Λ.Ǫ:return "Approaching dock";case Λ.Ǌ:return "Docking";case Λ.Βω:return "Aligning";case Λ.Ǆ:return "Aligning";case Λ.Γω:return "Retry docking";case Λ.ș:return "Unloading";case Λ.Ț:return ΕΧ;
case Λ.Ș:return "Undocking";case Λ.ş:return "Charging batteries";case Λ.ΔΗ:return "Waiting for uranium";case Λ.ΓΔ:return "Filling up hydrogen";case Λ.ΔΕ:return "Waiting for ejection";case Λ.ΔΖ:return "Waiting for ejection";case Λ.ΓΖ:return "Flying to drop pos";}return "";}String ΑΞ(Ё Д){switch(Д)
{case Ё.Αε:return "On \"Undock\" command";case Ё.Αζ:return "On player entered cockpit";case Ё.Γο:return "On ship is full";case Ё.Γξ:return "On ship is empty";case Ё.Αη:return "On time delay";case Ё.Γκ:return "On batteries empty(<25%)";case Ё.Γθ:return "On batteries empty(=0%)";case Ё.Γι:return "On batteries full";
case Ё.Γν:return "On hydrogen empty(<25%)";case Ё.Γλ:return "On hydrogen empty(=0%)";case Ё.Γμ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Βλ,int ѓ,bool Ε,String Ǚ){ǜ+=1;if(Βλ==ѓ) Ǚ=">"+Ǚ+(Ĩ>=2?" .":"");else Ǚ=" "+Ǚ;Β+=Ǚ+"\n";return Βλ==ѓ&&Ε;}String Βύ(bool Ε)
{int Α=0;int Δ=ι;ǜ=0;String Ȍ="———————————————\n";String ȏ="--------------------------------------------\n";String Β="";Β+=ΓΥ(μ)+" | "+(Ξ.β?"Ready to dock":"No dock")+"\n";Β+=Ȍ;double Βρ=Math.Max(Math.Round(this.ΑА),0);if(ά==Ι.Main){bool ѕ=Ν==Μ.ζ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΓЋ();
if(Ν==Μ.ų)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Υ(Ι.Ȕ);if(Ν==Μ.Ǯ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Υ(Ι.Ȕ);if(Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Υ(Ι.ſ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ǈ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΓΗ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΓΚ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ѕ) Υ(Ι.Ģ);else Υ(Ι.ĕ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Υ(Ι.ΑΣ);if(Ν!=Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Υ(Ι.ΑΡ);}else if(ά==Ι.Ȕ){double ΕΫ=Math.Round(К*ǥ,1);double Δκ=Math.Round(Й*Ǥ,1);String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Start new job!")) Αρ();if(Ζ(ref ε,Δ,Α++,Ε," Change current job")){Ƃ(false,false);Υ(Ι.Main);}if(Ζ(ref ε,Δ,Α++,Ε," Width + (Width: "+К+" = "+ΕΫ+"m)")){ǩ(ref К,5,20,1);ǲ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Width -")){ǩ(ref К,-5,20,-1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height + (Height: "+Й+" = "+Δκ+"m)")){ǩ(ref Й,5,20,1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height -")){ǩ(ref Й,-5,20,-1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth + ("+(с==к.Ţ?"Depth":"Min")+": "+Ч+"m)")){ǩ(ref Ч,5,50,2);ǲ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Depth -")){ǩ(ref Ч,-5,50,-2);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth mode: "+ΓΝ(с))){с=ы(с);}if(Ζ(ref ε,Δ,Α++,Ε," Start pos: "+ΓΨ(Ǣ))){Ǣ=ы(Ǣ);}if(Ν==Μ.Ǯ&&с==к.ǅ) с=ы(с);Β+=Ż(8,ε,Δ,ref ΔЍ);}else if(ά==Ι.ſ){float[]ΑЎ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Υ(Ι.Απ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΑΞ(ν.Д))) ν.Д=ы(ν.Д);if(!ν.ΑΟ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ŧ((int) ν.Ш))) ν.Ш=ǉ(ν.Ш,ΑЎ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΑΞ(ξ.Д))) ξ.Д=ы(ξ.Д);
if(!ξ.ΑΟ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ŧ((int) ξ.Ш))) ξ.Ш=ǉ(ξ.Ш,ΑЎ);}else if(ά==Ι.Απ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) Αρ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.ſ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.б!=""?ν.б:"-"))) ν.б=Ĝ(ref ΕΞ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.г!=""?ν.г:"-"))) ν.г=Ĝ(ref ΕΠ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.б!=""?ξ.б:"-"))) ξ.б=Ĝ(ref ΕΟ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.г!=""?ξ.г:"-"))) ξ.г=Ĝ(ref ΕΡ);
}else if(ά==Ι.ȓ){String ΒΩ=д+" %";if(Ȳ) є++;if(є>1){є=0;Ŀ++;if(Ŀ>1) Ŀ=0;}bool[]Ʋ=new bool[]{ƪ.Count==0,ȝ==щ.None,Ȱ.Count==0};int Βτ=0;while(true){Βτ++;ǘ++;if(ǘ>Ʋ.Length-1) ǘ=0;if(Βτ>=Ʋ.Length) break;if(!Ʋ[ǘ]) break;}bool ѕ=Ν==Μ.ζ;if(!ѕ&&Ǩ&&ώ!=-1&&Ŀ==0)
ΒΩ=ώ<1000000?Math.Round(ώ)+" Kg":Math.Round(ώ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){ǎ();Υ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ѕ) Υ(Ι.ĕ);else Υ(Ι.Ģ);if(!ѕ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) Βψ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) Βυ=true;Β+=ȏ;if(!ѕ) Β+="Progress: "+Math.Round(Ń,1)+" %\n";Β+="State: "+ΑΙ(Ĳ)+" "+Βρ+"m \n";Β+="Load: "+Ɲ+" % Max: "+ΒΩ+" \n";if(ǘ==0) Β+="Uranium: "+(ƪ.Count==0?"No reactors":Math.Round(Βφ,1)+"Kg "+(П==-1?"":" Min: "+П+" Kg"))+"\n";
if(ǘ==1) Β+="Battery: "+(ȝ==щ.None?ΑΓ(ȝ):Αώ+"% "+(е==-1||ѕ?"":" Min: "+е+" %"))+"\n";if(ǘ==2) Β+="Hydrogen: "+(Ȱ.Count==0?"No tanks":Math.Round(ΒΟ,1)+"% "+(ж==-1||ѕ?"":" Min: "+ж+" %"))+"\n";}else if(ά==Ι.ĕ){String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Back")){if(μ==Ο.ў) Υ(Ι.ȓ);else Υ(Ι.Main);}
if(Ζ(ref ε,Δ,Α++,Ε," Max load: "+д+"%")) Ќ(ref д,д<=80?-10:-5,λ.Ű,false);if(Ζ(ref ε,Δ,Α++,Ε," Weight limit: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Ζ(ref ε,Δ,Α++,Ε," Ejection: "+ΓΟ(ȧ))) ȧ=ы(ȧ);if(Ζ(ref ε,Δ,Α++,Ε," Toggle sorters: "+(ǣ?"On":"Off"))){ǣ=!ǣ;if(ǣ) ž(Ƶ);}
if(Ζ(ref ε,Δ,Α++,Ε," Unload ice: "+(ќ?"On":"Off"))) ќ=!ќ;if(Ζ(ref ε,Δ,Α++,Ε," Uranium: "+(П==-1?"Ignore":"Min "+П+"Kg"))) Ќ(ref П,(П>5?-5:-1),λ.Ų,true);if(Ζ(ref ε,Δ,Α++,Ε," Battery: "+(е==-1?"Ignore":"Min "+е+"%"))) Ќ(ref е,-5,λ.Αή,true);if(Ζ(ref ε,Δ,Α++,Ε," Hydrogen: "+(ж==-1?"Ignore":"Min "+ж+"%"))) Ќ(ref ж,-10,λ.Αί,true);
if(Ζ(ref ε,Δ,Α++,Ε," When done: "+(Đ?"Return home":"Stop"))) Đ=!Đ;if(Ζ(ref ε,Δ,Α++,Ε," On damage: "+ΑΕ(љ))) љ=ы(љ);if(Ζ(ref ε,Δ,Α++,Ε," Advanced...")) Υ(Ι.ś);Β+=Ż(8,ε,Δ,ref ΔЎ);}else if(ά==Ι.ś){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.ў) Υ(Ι.ȓ);else Υ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ν==Μ.Ǯ?" Grinder":" Drill")+" inv. balancing: "+(ȱ?"On":"Off"))) ȱ=!ȱ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ν==Μ.Ǯ?" grinders":" drills")+": "+(Ȩ?"Fwd + Bwd":"Fwd"))) Ȩ=!Ȩ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ř+"m/s")) Ќ(ref Ř,0.5f,λ.ƃ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ŗ+"m/s")) Ќ(ref ŗ,0.5f,λ.ƃ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѐ*100f)+"%"+(ѐ>0.80f?" (risky)":""))) Ќ(ref ѐ,0.1f,λ.Ś,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+đ*100f+"%")) Ǉ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ȭ*100f+"%")) Ǉ(false,0.05f);
}else if(ά==Ι.Ģ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.ў) Υ(Ι.ȓ);else Υ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+д+"%")) Ќ(ref д,д<=80?-10:-5,λ.Ű,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ќ?"On":"Off"))) ќ=!ќ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(П==-1?"Ignore":"Min "+П+"Kg"))) Ќ(ref П,(П>5?-5:-1),λ.Ų,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(е==-1?"Ignore":"Charge up"))) е=(е==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(ж==-1?"Ignore":"Fill up"))) ж=(ж==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ΑΕ(љ))) љ=ы(љ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѐ*100f)+"%"+(ѐ>0.80f?" (risky)":""))) Ќ(ref ѐ,0.1f,λ.Ś,false);
}else if(ά==Ι.Αι){double Δτ=0;if(ρ.Count>0) Δτ=Vector3.Distance(ρ.Last().Σ,Ы);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΓЏ();if(Ν!=Μ.ζ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ѝ?"Use old home":(Ξ.β?"Was set! ":"none ")))) ѝ=!ѝ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ѝ?"Use old connector":(Ξ.β?"Was set! ":"none ")))) ѝ=!ѝ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ď?"Use old connector":(Τ.β?"Was set! ":"none ")))) Ď=!Ď;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ď?"Use old path":(ρ.Count>1?"Count: "+ρ.Count:"none ")))) ď=!ď;Β+=ȏ;Β+="Wp spacing: "+Math.Round(ΕΩ)+"m\n";}else if(ά==Ι.Ɓ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){ǎ();Υ(Ι.Main);}
Β+=ȏ;Β+="State: "+ΑΙ(Ĳ)+" \n";Β+="Speed: "+Math.Round(ΕΓ,1)+"m/s\n";Β+="Target dist: "+Βρ+"m\n";Β+="Wp count: "+ρ.Count+"\n";Β+="Wp left: "+ΕΪ+"\n";}else if(ά==Ι.ΑΣ){List<IMyTerminalBlock>Ǒ=ΓΜ();if(Ȳ) є++;if(є>=Ǒ.Count) є=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Υ(Ι.ΑΤ);
Β+=ȏ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(Ɲ,1)+"% "+Math.Round(ǐ,1)+" / "+Math.Round(Ǜ,1)+"\n";Β+="Uranium: "+(ƪ.Count==0?"No reactors":Math.Round(Βφ,1)+"Kg "+Δψ)+"\n";Β+="Battery: "+(ȝ==щ.None?"":Αώ+"% ")+ΑΓ(ȝ)+"\n";Β+="Hydrogen: "+(Ȱ.Count==0?"No tanks":Math.Round(ΒΟ,1)+"% ")+"\n";
Β+="Damage: "+(Ǒ.Count==0?"None":""+(є+1)+"/"+Ǒ.Count+" "+Ǒ[є].CustomName)+"\n";}else if(ά==Ι.ΑΤ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=ȏ;Β+="Next scan: "+ŏ+"s\n";Β+="Drill area: "+Math.Round(ǥ,1)+"x"+Math.Round(Ǥ,1)+"m Ship: "+Math.Round(Βν,1)+"x"+Math.Round(Βμ,1)+"m ("+Math.Round(ȍ,1)+"m)\n";
Β+="Broadcast: "+(ȟ?"Online - "+ć:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ǿ*100f,1)+"% \n";}else if(ά==Ι.ΑΡ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=ȏ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Ư==2) Β="Fatal setup error\nNext scan: "+ŏ+"s\n";if(Βι) Β="Recompile script now";int Δφ=Β.Split('\n').Length;for(int i=Δφ;i<=10;i++) Β+="\n";Β+=Ȍ;Β+="Last: "+Φ+"\n";
return Β;}void Ǉ(bool Δρ,float ő){ǎ();Ƃ(true,false);if(Δρ) Ќ(ref đ,ő,λ.ŵ,false);else Ќ(ref ȭ,ő,λ.ŵ,false);ΒЂ();Ħ(true,true,0,0);}float ǉ(float ǵ,float[]Ŕ){float Ψ=Ŕ[0];for(int i=Ŕ.Length-1;i>=0;i--)
if(ǵ<Ŕ[i]) Ψ=Ŕ[i];return Ψ;}String Ĝ(ref int ѓ){String Ψ="";if(ѓ>=Βσ.Count) ѓ=-1;if(ѓ>=0) Ψ=Βσ[ѓ].CustomName;ѓ++;return Ψ;}void ǩ(ref int value,int Ȟ,int Œ,int Ƴ){if(Ȟ==0) return;if(value<Œ&&Ƴ>0||value<=Œ&&Ƴ<0){value+=Ƴ;return;}int ΔΙ=Math.Abs(Ȟ);int Ƅ=0;int Ʀ=1;
while(true){Ƅ+=Ʀ*ΔΙ*10;if(Ȟ<0&&value-Œ<=Ƅ) break;if(Ȟ>0&&value-Œ<Ƅ) break;Ʀ++;}value+=Ʀ*Ȟ;}void ǲ(bool Ζο){К=Math.Max(К,1);Й=Math.Max(Й,1);Ч=Math.Max(Ч,0);Ħ(ά==Ι.Ȕ,false,0,0);}void Ќ(ref float value,float ő,λ Ї,bool Č){Ќ(ref value,value+ő,Ї,Č,true);
}void Ќ(ref float value,float Ȁ,λ Ї,bool Č,bool Εή){float Р=0,О=0;if(Ї==λ.ƃ){Р=0.5f;О=10f;}if(Ї==λ.Ś){Р=0.1f;О=1f;}if(Ї==λ.Ū){Р=50f;О=100f;}if(Ї==λ.Αή){Р=5f;О=30f;}if(Ї==λ.Ų){Р=1f;О=25f;}if(Ї==λ.Ű){Р=10f;О=95f;}if(Ї==λ.Αί){Р=10f;О=90f;}
if(Ї==λ.ΓБ){Р=10f;О=1800;}if(Ї==λ.ŵ){Р=0.0f;О=0.75f;}if(Ȁ==-1&&Č){value=-1;return;}if(value==-1) Č=false;bool Ββ=Ȁ<Р||Ȁ>О;if(Ββ&&Εή){if(Ȁ<value) value=О;else if(Ȁ>value) value=Р;}else value=Ȁ;if(Ββ&&Č) value=-1;else value=Math.Max(Р,Math.Min(value,О));
value=(float) Math.Round(value,2);}void Βψ(bool ΔЈ){if(ΔЈ) а=Math.Max(0,а-1);else а++;ΓΒ();ΓΑ();for(int k=0;k<Ȯ.Count&&k<Щ.Length;k++) ΓЅ(Ȯ[k],k<а);ǝ=-1;Αψ(true);}T ы<T>(T ǵ){int Α=Array.IndexOf(Enum.GetValues(ǵ.GetType()),ǵ);Α++;if(Α>=ΓΓ(ǵ)) Α=0;return(T) Enum.GetValues(ǵ.GetType()).GetValue(Α);
}int ΓΓ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЋ(){Ŗ.Clear();for(int i=0;i<ρ.Count;i++) Ŗ.Add(ρ[i]);ρ.Clear();ΒΠ=true;ΒΞ=new Χ(Ξ);ΒΣ=new Χ(Τ);Ξ.β=false;if(Ν==Μ.ζ) Τ.β=false;for(int i=0;i<ч.Count;i++)
if(!ш.Contains(ч.Keys.ElementAt(i)))
ш.Add(ч.Keys.ElementAt(i));ѝ=false;Ď=false;ď=false;Υ(Ι.Αι);}void ΓЏ(){if(ѝ) Ξ=ΒΞ;if(Ď) Τ=ΒΣ;if(ď){ρ.Clear();for(int i=0;i<Ŗ.Count;i++) ρ.Add(Ŗ[i]);}ΒΠ=false;ǎ();Υ(Ι.Main);}void Ġ(Χ М,Ο ϊ){ΔΫ=М;if(μ==Ο.ў) ї=ϊ;}bool ΑΖ(Χ М,float ђ,bool ΕΥ,out Vector3 δ){if(ΕΥ){Vector3I Δζ=new Vector3I((int) М.Л.X,(int) М.Л.Y,(int) М.Л.Z);
IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(Δζ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){δ=new Vector3();return false;}Vector3 ΔΧ=ě(І,Ϊ.FatBlock.GetPosition()-Ы);Vector3 ΔΦ=ě(І,Ϊ.FatBlock.WorldMatrix.Forward);δ=М.Σ-Αυ(М.Ђ,М.ύ*-1,ΔΧ)-Αυ(М.Ђ,М.ύ*-1,ΔΦ)*ђ;return true;}
else{δ=М.Σ;return true;}}Vector3 ΓΤ(int ΔΤ,int Ȉ,bool ΔЉ){if(!ΔЉ&&ΒΘ) return Ɗ;float ΔΥ=((ω-1f)/2f)-ΔΤ;float ΔЋ=((Ў-1f)/2f)-Ȉ;Ɗ=Τ.Σ+Τ.Г*ΔΥ*ǥ+ǿ*-1*ΔЋ*Ǥ;ΒΘ=true;return Ɗ;}Π ţ(){float И=-1;Π Ψ=Π.Э;if(Ν!=Μ.ζ){Γδ();if(μ!=Ο.н){Vector3 υ=Αφ(Ă,ǿ*-1,Ы-Τ.Σ);
if(Math.Abs(υ.X)<=(float)(ω*ǥ)/2f&&Math.Abs(υ.Y)<=(float)(Ў*Ǥ)/2f){if(υ.Z<=-1&&υ.Z>=-ƭ*2) return Π.Ĕ;if(υ.Z>-1&&υ.Z<ȍ*2) return Π.Э;}if(ъ(Τ.Σ,ref И)) Ψ=Π.Э;}if(Ξ.β){if(ъ(Ξ.Σ,ref И)) Ψ=Π.Ь;for(int i=0;i<ρ.Count;i++)
if(ъ(ρ[i].Σ,ref И)) Ψ=Π.о;if(Vector3.Distance(Ы,Ξ.Σ)<dockDist*ȍ) Ψ=Π.Ь;if(Ǭ(MyShipConnectorStatus.Connectable)!=null||Ǭ(MyShipConnectorStatus.Connected)!=null)
Ψ=Π.Ь;}}else{Vector3 δ=new Vector3();IMyShipConnector ĭ=Ǭ(MyShipConnectorStatus.Connected);if(Ξ.β){if(ъ(Ξ.Σ,ref И)) Ψ=Π.Ь;if(ΑΖ(Ξ,dockDist,true,out δ))
if(ъ(δ,ref И)) Ψ=Π.Ь;if(ĭ!=null&&Vector3.Distance(ĭ.GetPosition(),Ξ.Σ)<5)
return Π.Ŝ;}for(int i=0;i<ρ.Count;i++)
if(Vector3.Distance(ρ[i].Σ,Ξ.Σ)>dockDist*ȍ&&Vector3.Distance(ρ[i].Σ,Τ.Σ)>dockDist*ȍ)
if(ъ(ρ[i].Σ,ref И)) Ψ=Π.о;if(Τ.β){if(ъ(Τ.Σ,ref И)) Ψ=Π.Э;if(ΑΖ(Τ,dockDist,true,out δ))
if(ъ(δ,ref И)) Ψ=Π.Э;if(ĭ!=null&&Vector3.Distance(ĭ.GetPosition(),Τ.Σ)<5)
return Π.ŝ;}}return Ψ;}bool ъ(Vector3 δ,ref float И){float ђ=Vector3.Distance(δ,Ы);if(ђ<И||И==-1){И=ђ;return true;}return false;}void Αρ(){if(Ư>0){Φ="Setup error! Can't start";return;}if(Ν==Μ.ζ){ǈ();return;}Τ.Σ=Ы;Τ.η=І.GetNaturalGravity();Τ.Ђ=Δγ;Τ.ύ=Δέ;Τ.Г=Δυ;
Ă=њ.WorldMatrix.Forward;ǿ=Τ.ύ;if(Ă==І.WorldMatrix.Down) ǿ=І.WorldMatrix.Backward;Ƃ(true,true);έ(Λ.Ŧ);ƀ();}void Ƃ(bool Н,bool ΔЊ){if(μ==Ο.н&&!Н) return;bool ΔϏ=Н||μ==Ο.ǋ||ω!=К||Ў!=Й||ǻ!=Ǣ;if(ΔϏ){if(μ!=Ο.н){μ=Ο.Ė;ΓΤ(ļ,Ľ,ΔЊ);Φ="Job changed, lost progress";}ǻ=Ǣ;
ω=К;Ў=Й;Ľ=0;ļ=0;Щ="";ǝ=-1;Ƣ=0;Ā=0;ΑЊ=0;а=0;Αψ(true);}}void ŷ(){ΓЃ(Ы,0);ğ(ċ,true);}void ƀ(){Αο(ю.Αμ);ŷ();Ž(ƚ,false);ž(Ƶ);μ=Ο.ў;ΒЁ(true);ї=μ;Υ(Ι.ȓ);Γό();ΑЋ=true;Ƙ=0;for(int i=ț.Count-1;i>=0;i--)
if(ΑΦ(ț[i],false)) Ƙ++;if(Ƙ>0) Φ="Started with damage";}void ǎ(){if(μ==Ο.ў){μ=Ο.Ŷ;Φ="Job paused";}έ(Λ.џ);ї=μ;ΓЄ(false,0,0,0,0);ΓЍ();Βϋ(new Vector3(),false);ΓЎ();ΓЀ(ChargeMode.Auto);ΓІ(false);ΓЂ(true);Ǎ(Λ.џ);Ħ(false,false,0,0);ğ(Ф,false);ğ(ȣ,true);ž(true);Βκ=false;ΒЅ();
Δά=false;Γφ();ΑЋ=false;Ʊ=false;Βυ=false;if(ά!=Ι.Main&&ά!=Ι.ĕ&&ά!=Ι.ś&&ά!=Ι.Ģ)
Υ(Ι.Main);}void ǈ(){Π у=ţ();if(Ν==Μ.ζ){if(!Τ.β||!Ξ.β) return;ƀ();bool ƕ=Vector3.Distance(Ы,Ξ.Σ)<Vector3.Distance(Ы,Τ.Σ);if(Ʈ==Ο.Ē) ƕ=true;if(Ʈ==Ο.ē) ƕ=false;if(ƕ){Ġ(Ξ,Ο.Ē);switch(у){case Π.Ŝ:έ(Λ.Ț);break;case Π.о:έ(Λ.э);break;case Π.Ь:έ(Λ.Ǫ);break;default:έ(Λ.Ș);break;
}}else{Ġ(Τ,Ο.ē);switch(у){case Π.ŝ:έ(Λ.Ț);break;case Π.Э:έ(Λ.Ǫ);break;case Π.о:έ(Λ.э);break;default:έ(Λ.Ș);break;}}}else{if(μ!=Ο.Ŷ&&μ!=Ο.Ė) return;bool ΕΨ=μ==Ο.Ė;ƀ();bool Ɨ=Γβ(false)&&Ξ.β;switch(у){case Π.Э:έ(Ɨ?Λ.ť:Λ.Ŧ);break;case Π.о:έ(Ɨ?Λ.Ȓ:Λ.э);break;case Π.Ь:έ(Ɨ?Λ.Ǫ:Λ.ș);break;
case Π.Ĕ:{if(Ā!=а||ΕΨ) έ(Λ.ź);else έ(Λ.Αΰ);}break;default:break;}}}void ΓΚ(){if(μ==Ο.н&&!Ξ.β) return;if(Ν==Μ.ζ&&(!Τ.β||!Ξ.β)) return;Φ="Move to job";Π у=ţ();if(Ν==Μ.ζ){Ġ(Τ,Ο.ē);switch(у){case Π.Э:έ(Λ.Ǫ);break;case Π.о:έ(Λ.э);break;case Π.ŝ:return;default:έ(Λ.Ș);break;
}Ǎ(Λ.Ț);}else{switch(у){case Π.Э:έ(Λ.э);break;case Π.о:έ(Λ.э);break;case Π.Ь:έ(Λ.ș);break;case Π.Ĕ:έ(Λ.ź);break;default:break;}if(μ==Ο.н) Ǎ(Λ.э);else Ǎ(Λ.Ǆ);Βκ=true;}ŷ();Υ(Ι.Ɓ);Ž(ƚ,false);ї=Ο.Αβ;}void ΓΗ(){if(!Ξ.β) return;Φ="Move home";Π у=ţ();if(Ν==Μ.ζ){Ġ(Ξ,Ο.Ē);switch(у)
{case Π.о:έ(Λ.Ȓ);break;case Π.Ь:έ(Λ.Ǫ);break;case Π.Ŝ:return;default:έ(Λ.Ș);break;}Ǎ(Λ.Ț);}else{if(Ǭ(MyShipConnectorStatus.Connected)!=null) return;if(Ǭ(MyShipConnectorStatus.Connectable)!=null){έ(Λ.Ǌ);Ǎ(Λ.ș);return;}switch(у){case Π.Э:έ(Λ.ť);break;case Π.о:έ(Λ.Ȓ);break;case Π.Ь:έ(Λ.Ȓ);break;
case Π.Ĕ:έ(Λ.Αλ);break;default:break;}Ǎ(Λ.ș);}ŷ();Υ(Ι.Ɓ);Ž(ƚ,false);ї=Ο.Αα;}void έ(Λ Ƨ){if(Ƨ==Λ.џ) ĉ=Λ.џ;if(ĉ!=Λ.џ&&Ĳ==ĉ&&Ƨ!=ĉ){ǎ();return;}ΕΘ=true;Ĳ=Ƨ;}void Ǎ(Λ ϊ){this.ĉ=ϊ;}void Αψ(bool ƫ){if(ƫ){Ń=0;return;}if(Ν==Μ.ζ) return;float Ĺ=а*Math.Max(1,Ч);if(Ā==а) Ĺ+=Math.Min(Ч,ΑЊ);
float ΕΣ=ω*Ў*Math.Max(1,Ч);Ń=Math.Max(Ń,(float) Math.Min(Ĺ/ΕΣ*100.0,100));}List<IMyTerminalBlock>ΓΜ(){List<IMyTerminalBlock>Ǒ=new List<IMyTerminalBlock>();for(int i=0;i<ț.Count;i++){IMyTerminalBlock Ϊ=ț[i];if(ΑΦ(Ϊ,false)) Ǒ.Add(Ϊ);}return Ǒ;}bool ΑΦ(IMyTerminalBlock Ϊ,bool Ʒ)
{return(!ΑΧ(Ϊ,Ʒ)||!Ϊ.IsFunctional);}bool ΑΧ(IMyTerminalBlock Ϊ,bool Ʒ){if(Ϊ==null) return false;try{IMyCubeBlock ΒΗ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(Ʒ) return ΒΗ==Ϊ;else return ΒΗ.GetType()==Ϊ.GetType();}catch{return false;}}bool Γβ(bool ΖΡ){if(Ǩ&&Ν!=Μ.ζ)
if(ώ!=-1&&Βξ>=ώ){Φ="Ship too heavy";return true;}if(Ɲ>=д||Ʊ){Ʊ=false;Φ="Ship is full";return true;}return false;}static String ΓΩ(IMyThrust и){return и.BlockDefinition.SubtypeId;}Vector3 ů(Vector3 А,float[,]ǽ){return new Vector3(
А.X>=0?ǽ[0,0]:ǽ[0,1],
А.Y>=0?ǽ[1,0]:ǽ[1,1],
А.Z>=0?ǽ[2,0]:ǽ[2,1]);}bool ΑΝ(Χ Ω,String Ǧ,out float ƒ){ƒ=0;int Α=ш.IndexOf(Ǧ);if(Α==-1||Ω.У==null||Α>=Ω.У.Length)
return false;ƒ=Ω.У[Α];if(ƒ==-1) return false;return true;}Vector3 ΑΜ(Vector3 А,Χ Ω){if(Ω!=null){Vector3 Ψ=new Vector3();for(int i=0;i<ч.Keys.Count;i++){String Ǧ=ч.Keys.ElementAt(i);float Ȧ=0;if(!ΑΝ(Ω,Ǧ,out Ȧ))
return ů(А,č);Ψ+=ů(А,ч.Values.ElementAt(i))*Ȧ;}return Ψ;}return ů(А,č);}float ũ(Vector3 А,Χ Ω){return ũ(А,new Vector3(),Ω);}float ũ(Vector3 А,Vector3 η,Χ Ω){Vector3 ΕΜ=ΑΜ(А,Ω);Vector3 Δί=ΕΜ+η*Βξ;float ǟ=(Δί/А).AbsMin();return(float)(А*ǟ).Length();}float ę(Vector3 ȫ,Vector3 Ϗ,Vector3 η,Χ Ω)
{if(η.Length()==0f) return 0;Vector3 Δχ=Αφ(ȫ,Ϗ,Vector3.Normalize(η));float Н=ũ(-Δχ,Ω);return Н/η.Length();}void ΒЁ(bool Δν){float Е=0;float Ɠ=0.9f;if(Δν){ώ=-1;ǧ=0;Ő=null;if(μ!=Ο.н&&Τ.η.Length()!=0){Е=Ɠ*ę(Τ.Ђ,Τ.ύ*-1,Τ.η,null);if(Е<ώ||ώ==-1) ώ=Е;}if(Ξ.β&&Ξ.η.Length()!=0)
{Е=Ɠ*ę(Ξ.Ђ,Ξ.ύ*-1,Ξ.η,null);if(Е<ώ||ώ==-1) ώ=Е;}return;}if(ǧ==-1) return;if(ǧ>=0){int Βζ=0;while(ǧ<ρ.Count){if(Βζ>100) return;Βζ++;Χ Ω=ρ[ǧ];if(Ω.η.Length()!=0f){Е=Ɠ*Math.Min(
ę(Ω.Ђ,Ω.ύ*-1,Ω.η,Ω),
ę(Ω.Ђ*-1,Ω.ύ*-1,Ω.η,Ω));if(Е<ώ||ώ==-1) ώ=Е;}else Ő=Ω;ǧ++;}ǧ=-1;}bool Ɩ=true;float ј=0;if(Ő!=null){for(int i=0;i<ч.Count;i++){String Ǧ=ч.Keys.ElementAt(i);float[,]Ƽ=ч.Values.ElementAt(i);float Ȧ=0;if(!ΑΝ(Ő,Ǧ,out Ȧ)){Ɩ=false;break;}for(int a=0;a<Ƽ.GetLength(0);a++)
for(int b=0;b<Ƽ.GetLength(1);b++){float Ǐ=Math.Abs(Ƽ[a,b]*Ȧ);if(Ǐ==0) continue;Ɩ=true;if(ј==0||Ǐ<ј) ј=Ǐ;}}}if(!Ɩ){for(int a=0;a<č.GetLength(0);a++)
for(int b=0;b<č.GetLength(1);b++){float Ǐ=Math.Abs(č[a,b]);if(Ǐ==0) continue;if(ј==0||Ǐ<ј) ј=Ǐ;}}if(ј>0){float Δϋ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Е=Ж(ј,Δϋ);if(Е>0)
if(Е<ώ||ώ==-1) ώ=Е;}}void Βϋ(Vector3 Н,bool χ){if(!χ){for(int i=0;i<ċ.Count;i++)
ċ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ɔ=ΑΜ(Н,null);float Εί=Math.Min(1,Math.Abs(Ж(Н.X,Ɔ.X)));float Εΰ=Math.Min(1,Math.Abs(Ж(Н.Y,Ɔ.Y)));float Εα=Math.Min(1,Math.Abs(Ж(Н.Z,Ɔ.Z)));for(int i=0;i<ċ.Count;i++){IMyThrust и=ċ[i];Vector3 Ƕ=Γϊ(ě(І,и.WorldMatrix.Backward),1);
if(Ƕ.X!=0&&Math.Sign(Ƕ.X)==Math.Sign(Н.X))
и.SetValueFloat("Override",и.MaxThrust*Εί);else if(Ƕ.Y!=0&&Math.Sign(Ƕ.Y)==Math.Sign(Н.Y))
и.SetValueFloat("Override",и.MaxThrust*Εΰ);else if(Ƕ.Z!=0&&Math.Sign(Ƕ.Z)==Math.Sign(Н.Z))
и.SetValueFloat("Override",и.MaxThrust*Εα);else
и.SetValueFloat("Override",0.0f);}}void ΓЄ(bool χ,float ΔЇ,float ȃ,float ȇ,float ȑ){for(int i=0;i<ΒΚ.Count;i++){IMyGyro θ=ΒΚ[i];θ.GyroOverride=χ;if(!χ) θ.GyroPower=100;else θ.GyroPower=ΔЇ;if(!χ) continue;Vector3 Б=І.WorldMatrix.Forward;Vector3 Ȇ=І.WorldMatrix.Right;Vector3 Ϗ=І.WorldMatrix.Up;
Vector3 Ƿ=θ.WorldMatrix.Forward;Vector3 ǹ=θ.WorldMatrix.Up;Vector3 Ǹ=θ.WorldMatrix.Left*-1;if(Ƿ==Б) θ.SetValueFloat("Roll",ȑ);else if(Ƿ==(Б*-1)) θ.SetValueFloat("Roll",ȑ*-1);else if(ǹ==(Б*-1)) θ.SetValueFloat("Yaw",ȑ);else if(ǹ==Б) θ.SetValueFloat("Yaw",ȑ*-1);else if(Ǹ==Б) θ.SetValueFloat("Pitch",ȑ);
else if(Ǹ==(Б*-1)) θ.SetValueFloat("Pitch",ȑ*-1);if(Ǹ==(Ȇ*-1)) θ.SetValueFloat("Pitch",ȃ);else if(Ǹ==Ȇ) θ.SetValueFloat("Pitch",ȃ*-1);else if(ǹ==Ȇ) θ.SetValueFloat("Yaw",ȃ);else if(ǹ==(Ȇ*-1)) θ.SetValueFloat("Yaw",ȃ*-1);else if(Ƿ==(Ȇ*-1)) θ.SetValueFloat("Roll",ȃ);
else if(Ƿ==Ȇ) θ.SetValueFloat("Roll",ȃ*-1);if(ǹ==(Ϗ*-1)) θ.SetValueFloat("Yaw",ȇ);else if(ǹ==Ϗ) θ.SetValueFloat("Yaw",ȇ*-1);else if(Ǹ==Ϗ) θ.SetValueFloat("Pitch",ȇ);else if(Ǹ==(Ϗ*-1)) θ.SetValueFloat("Pitch",ȇ*-1);else if(Ƿ==Ϗ) θ.SetValueFloat("Roll",ȇ);else if(Ƿ==(Ϗ*-1)) θ.SetValueFloat("Roll",ȇ*-1);
}}void ΓЎ(){this.Δη=false;}void ΓЍ(){ġ(false,false,false,Βΰ,0);Βς=false;}void ΓЃ(Vector3 α,float Ĉ){ġ(true,false,false,α,Ĉ);}void ġ(bool χ,bool ƴ,bool ƽ,Vector3 α,float Ĉ){ġ(χ,ƴ,ƽ,α,α-Ы,0.0f,Ĉ);}void ġ(bool χ,bool ƴ,bool ƽ,Vector3 α,Vector3 ΔА,float ΔБ,float Ĉ){Βς=true;
this.ΕΝ=χ;Βΰ=α;this.Δω=Ĉ;this.ΔЅ=ΔБ;this.ΕΕ=ƴ;this.ΕΔ=ƽ;this.Δα=ΔА;ΑА=Vector3.Distance(α,Ы);}void Γό(){В.GetBlocksOfType(ț,Ů);}void ΒЂ(){if(І==null) return;њ=null;float Ŋ=0,ņ=0,ŋ=0,Ň=0,Ō=0,ň=0;List<IMyTerminalBlock>Ƹ=ΑΑ(Ф,pamTag,true);bool ΔЁ=Ƹ.Count==0;
if(Ƹ.Count>0) њ=Ƹ[0];else if(Ф.Count>0) њ=Ф[0];int ı=0;float ȅ=0;for(int i=0;i<Ф.Count;i++){if(Ф[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward){if(ΔЁ){Ư=2;Φ="Mining direction is unclear!";return;}continue;}ı++;ȅ=Math.Max(ȅ,ΔΑ(Ф[i]));Vector3 υ=Ũ(І,Ф[i].GetPosition());
if(i==0){Ŋ=υ.X;ņ=υ.X;ŋ=υ.Y;Ň=υ.Y;Ō=υ.Z;ň=υ.Z;}ņ=Math.Max(υ.X,ņ);Ŋ=Math.Min(υ.X,Ŋ);Ň=Math.Max(υ.Y,Ň);ŋ=Math.Min(υ.Y,ŋ);ň=Math.Max(υ.Z,ň);Ō=Math.Min(υ.Z,Ō);}if(ȅ==0) ȅ=drillRadius;bool ΒΒ=њ!=null&&њ.WorldMatrix.Forward==І.WorldMatrix.Down;ǥ=(ņ-Ŋ)*(1-đ)+ȅ*2;
Ǥ=(ΒΒ?ň-Ō:Ň-ŋ)*(1-ȭ)+ȅ*2;IMyCubeGrid g=І.CubeGrid;float ǀ=0,ǁ=0,ǂ=0,ǃ=0;for(int c=0;c<8;c++){Vector3 p=Ũ(І,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ΒΒ?p.Z:p.Y;if(c==0||p.X<ǀ) ǀ=p.X;if(c==0||p.X>ǁ) ǁ=p.X;if(c==0||y<ǂ) ǂ=y;if(c==0||y>ǃ) ǃ=y;}Βν=ǁ-ǀ+g.GridSize;Βμ=ǃ-ǂ+g.GridSize;}float ΔΑ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΑΑ<T>(List<T>Ѓ,String Т,bool ľ){List<T>Ψ=new List<T>();for(int i=0;i<Ѓ.Count;i++)
if(ΑΠ(Ѓ[i],Т,ľ)) Ψ.Add(Ѓ[i]);return Ψ;}bool ΑΠ<T>(T Ϊ,String Т,bool ľ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(ľ&&b.CustomName.ToUpper().Contains(Т.ToUpper())) return true;if(!ľ&&b.BlockDefinition.ToString().ToUpper().Contains(Т.ToUpper())) return true;return false;}void ğ<T>(List<T>Ѓ,bool χ)
{for(int i=0;i<Ѓ.Count;i++)
ΓЁ((IMyTerminalBlock) Ѓ[i],χ);}void ΓЁ(IMyTerminalBlock Ϊ,bool χ){if(Ϊ==null) return;String ΔΚ=χ?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(ΔΚ);a.Apply(Ϊ);}void ž(bool χ){Ƶ=χ;if(!ǣ) return;ğ(ΕΖ,χ);}void ΓЀ(ChargeMode ΰ){for(int i=0;i<Αύ.Count;i++) Αύ[i].ChargeMode=ΰ;}void ΓІ(bool ΕΚ)
{for(int i=0;i<Ȱ.Count;i++) Ȱ[i].Stockpile=ΕΚ;}void Ž(List<IMyLandingGear>Ɣ,bool ΒΪ){for(int i=0;i<Ɣ.Count;i++){if(ΒΪ) Ɣ[i].Lock();if(!ΒΪ) Ɣ[i].Unlock();}}void ΓЂ(bool χ){if(ΑЌ==χ) return;List<IMyShipController>İ=new List<IMyShipController>();В.GetBlocksOfType(İ,Ů);if(İ.Count==0) return;
for(int i=0;i<İ.Count;i++)
İ[i].DampenersOverride=χ;ΑЌ=χ;}IMyShipConnector Ǭ(MyShipConnectorStatus ΕΙ){for(int i=0;i<ȣ.Count;i++){if(!ΑΧ(ȣ[i],true)) continue;if(ȣ[i].Status==ΕΙ) return ȣ[i];}return null;}void Ħ(bool ư,bool ĳ,float Δδ,float ΔΝ){if(φ==null||Ф.Count==0) return;Vector3 ī=new Vector3();int ı=0;
for(int i=0;i<Ф.Count;i++){if(Ф[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward) continue;ı++;ī+=Ф[i].GetPosition();}ī=ī/ı;Vector3 ȁ=Ũ(φ,ī);φ.Enabled=true;φ.ShowOnHUD=ư;φ.LeftExtend=(ĳ?1:К)/2f*ǥ-ȁ.X;φ.RightExtend=(ĳ?1:К)/2f*ǥ+ȁ.X;φ.TopExtend=(ĳ?1:Й)/2f*Ǥ+ȁ.Y;
φ.BottomExtend=(ĳ?1:Й)/2f*Ǥ-ȁ.Y;φ.FrontExtend=(ư?Ч:Δδ)-ȁ.Z;φ.BackExtend=ư?0:ΔΝ+ȍ*0.75f+ȁ.Z;φ.DetectFloatingObjects=true;φ.DetectAsteroids=false;φ.DetectLargeShips=true;φ.DetectSmallShips=true;φ.DetectStations=true;φ.DetectOwner=true;φ.DetectSubgrids=false;
φ.DetectPlayers=false;φ.DetectEnemy=true;φ.DetectFriendly=true;φ.DetectNeutral=true;}void Ȗ<T>(List<T>Ѓ,bool ΕΒ,float Δβ,bool Δπ){for(int i=0;i<Ѓ.Count;i++){IMyTextSurface Ċ=null;if(Ѓ[i]is IMyTextSurface) Ċ=(IMyTextSurface) Ѓ[i];if(Ċ!=null){Ċ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΕΒ) continue;Ċ.Font="Debug";if(Δπ) continue;Ċ.FontSize=Δβ;}}}static float Ж(float a,float b){if(b==0) return 0;return a/b;}Vector3 Γϊ(Vector3 v,int Ǝ){return new Vector3(Math.Round(v.X,Ǝ),Math.Round(v.Y,Ǝ),Math.Round(v.Z,Ǝ));}Vector3 Ũ(IMyTerminalBlock Ϊ,Vector3 Εέ){return ě(Ϊ,Εέ-Ϊ.GetPosition());
}Vector3 ě(IMyTerminalBlock Ϊ,Vector3 ƿ){return Vector3D.TransformNormal(ƿ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 Αφ(Vector3 ȫ,Vector3 Ϗ,Vector3 ƿ){MatrixD ơ=MatrixD.CreateFromDir(ȫ,Ϗ);return Vector3D.TransformNormal(ƿ,MatrixD.Transpose(ơ));}Vector3 Αυ(Vector3 ȫ,Vector3 Ϗ,Vector3 υ){MatrixD ơ=MatrixD.CreateFromDir(ȫ,Ϗ);
return Vector3D.Transform(υ,ơ);}String Ϋ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ί(String s){String[]ή=s.Split('|');return new Vector3(float.Parse(Θ(ή,0)),float.Parse(Θ(ή,1)),float.Parse(Θ(ή,2)));}String ΔΘ(Χ Ω){String ǡ=":";String Ψ=Ϋ(Ω.Σ)+ǡ+Ϋ(Ω.Ђ)+ǡ+
Ϋ(Ω.ύ)+ǡ+Ϋ(Ω.Г)+ǡ+Ϋ(Ω.η);for(int i=0;i<Ω.У.Length;i++)
Ψ+=ǡ+Math.Round(Ω.У[i],3);return Ψ;}Χ ΓА(String s){String[]ή=s.Split(':');Χ Ω=new Χ(
ί(Θ(ή,0)),ί(Θ(ή,1)),
ί(Θ(ή,2)),ί(Θ(ή,3)),
ί(Θ(ή,4)));int Α=5;List<float>ΒΔ=new List<float>();while(Α<ή.Length){String œ=Θ(ή,Α);float f=0;if(!float.TryParse(œ,out f)) break;ΒΔ.Add(f);Α++;}Ω.У=ΒΔ.ToArray();return Ω;}System.Text.StringBuilder Ȋ=new System.Text.StringBuilder();void Κ<T>(T value,bool ΔЀ){if(ΔЀ) Ȋ.Append('\n');Ȋ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String ė(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ĥ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΔΛ,int Α){String œ=ΔΛ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(œ)) return "";
return œ;}void Save(){if(Βι||Ν==Μ.Ѝ){Storage="";return;}Storage=Βώ();}String Βώ(){Ȋ.Clear();Ȋ.Append(DATAREV+";");Κ(Ϋ(Βθ),false);Κ(Ϋ(Ξ.Ђ));Κ(Ϋ(Ξ.Г));Κ(Ϋ(Ξ.ύ));Κ(Ϋ(Ξ.η));Κ(Ϋ(Ξ.Σ));Κ(Ϋ(Ξ.Л));Κ(Ξ.β);Κ(Ϋ(Τ.Σ));Κ(Ϋ(Τ.η));Κ(Ϋ(Τ.Ђ));Κ(Ϋ(Τ.ύ));Κ(Ϋ(Τ.Г));Κ(Ϋ(Τ.Л));Κ(Τ.β);Κ(Ϋ(Ă));Κ(Ϋ(ǿ));Κ(";");
Κ((int) Ν,false);Κ((int) μ);Κ((int) ї);Κ(д);Κ(е);Κ(П);Κ(ж);Κ(ѐ);Κ(Ǩ);Κ(ќ);if(Ν==Μ.ζ){Κ((int) ν.Д);Κ(ν.Ш);Κ(ν.ǔ);Κ(ė(ν.б));Κ(ė(ν.г));Κ((int) ξ.Д);Κ(ξ.Ш);Κ(ξ.ǔ);Κ(ė(ξ.б));Κ(ė(ξ.г));}else{Κ((int) Ǣ);Κ((int) љ);Κ((int) ȧ);Κ((int) с);Κ((int) ǻ);Κ(Đ);Κ(ǣ);Κ(ȱ);Κ(Ȩ);Κ(К);Κ(Й);Κ(Ч);Κ(Ř);Κ(ŗ);Κ(đ);Κ(ȭ);
Κ(ω);Κ(Ў);Κ(ļ);Κ(Ľ);Κ(Ā);Κ(а);Κ(ƭ);Κ(Ƣ);}Κ(";");for(int i=0;i<ш.Count;i++)
Κ((i>0?"|":"")+ш[i],false);Κ(";");for(int i=0;i<ρ.Count;i++)
Κ(ΔΘ(ρ[i]),i>0);Κ(";",false);Κ(Щ+"|"+ǝ,false);Κ(";",false);Κ(ΒЎ(),false);String Ρ=Ȋ.ToString();Ȋ.Clear();return Ρ;}Ю Γχ(){return Γψ(Storage);}Ю Γψ(String Ρ){if(Ρ=="") return Ю.Γη;String[]Ǡ=Ρ.Split(';');if(Θ(Ǡ,0)!=DATAREV) return Ю.Αγ;int Α=0;try{String[]Η=Θ(Ǡ,1).Split('\n');
Βθ=ί(Θ(Η,Α++));Ξ.Ђ=ί(Θ(Η,Α++));Ξ.Г=ί(Θ(Η,Α++));Ξ.ύ=ί(Θ(Η,Α++));Ξ.η=ί(Θ(Η,Α++));Ξ.Σ=ί(Θ(Η,Α++));Ξ.Л=ί(Θ(Η,Α++));Ξ.β=bool.Parse(Θ(Η,Α++));Τ.Σ=ί(Θ(Η,Α++));Τ.η=ί(Θ(Η,Α++));Τ.Ђ=ί(Θ(Η,Α++));Τ.ύ=ί(Θ(Η,Α++));Τ.Г=ί(Θ(Η,Α++));Τ.Л=ί(Θ(Η,Α++));Τ.β=bool.Parse(Θ(Η,Α++));
Ă=ί(Θ(Η,Α++));ǿ=ί(Θ(Η,Α++));Η=Θ(Ǡ,2).Split('\n');Α=0;Ν=(Μ) int.Parse(Θ(Η,Α++));μ=(Ο) int.Parse(Θ(Η,Α++));ї=(Ο) int.Parse(Θ(Η,Α++));д=int.Parse(Θ(Η,Α++));е=int.Parse(Θ(Η,Α++));П=int.Parse(Θ(Η,Α++));ж=int.Parse(Θ(Η,Α++));ѐ=float.Parse(Θ(Η,Α++));Ǩ=bool.Parse(Θ(Η,Α++));
ќ=bool.Parse(Θ(Η,Α++));if(Ν==Μ.ζ){ν.Д=(Ё) int.Parse(Θ(Η,Α++));ν.Ш=float.Parse(Θ(Η,Α++));ν.ǔ=float.Parse(Θ(Η,Α++));ν.б=ĥ(Θ(Η,Α++));ν.г=ĥ(Θ(Η,Α++));ξ.Д=(Ё) int.Parse(Θ(Η,Α++));ξ.Ш=float.Parse(Θ(Η,Α++));ξ.ǔ=float.Parse(Θ(Η,Α++));ξ.б=ĥ(Θ(Η,Α++));ξ.г=ĥ(Θ(Η,Α++));}else{Ǣ=(Я) int.Parse(Θ(Η,Α++));
љ=(ь) int.Parse(Θ(Η,Α++));ȧ=(Х) int.Parse(Θ(Η,Α++));с=(к) int.Parse(Θ(Η,Α++));ǻ=(Я) int.Parse(Θ(Η,Α++));Đ=bool.Parse(Θ(Η,Α++));ǣ=bool.Parse(Θ(Η,Α++));ȱ=bool.Parse(Θ(Η,Α++));Ȩ=bool.Parse(Θ(Η,Α++));К=int.Parse(Θ(Η,Α++));Й=int.Parse(Θ(Η,Α++));Ч=int.Parse(Θ(Η,Α++));Ř=float.Parse(Θ(Η,Α++));
ŗ=float.Parse(Θ(Η,Α++));đ=float.Parse(Θ(Η,Α++));ȭ=float.Parse(Θ(Η,Α++));ω=int.Parse(Θ(Η,Α++));Ў=int.Parse(Θ(Η,Α++));ļ=int.Parse(Θ(Η,Α++));Ľ=int.Parse(Θ(Η,Α++));Ā=int.Parse(Θ(Η,Α++));а=int.Parse(Θ(Η,Α++));ƭ=int.Parse(Θ(Η,Α++));Ƣ=float.Parse(Θ(Η,Α++));}Η=Θ(Ǡ,3).Replace("\n","").Split('|');
ш=Η.ToList();Η=Θ(Ǡ,4).Split('\n');ρ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
ρ.Add(ΓА(Θ(Η,i)));Η=Θ(Ǡ,5).Split('|');Щ=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ǝ)) ǝ=-1;if(Щ.Length!=ω*Ў){Щ="";ǝ=-1;}ΒЍ(Θ(Ǡ,6));}catch{return Ю.ǌ;}Ʈ=ї;ǎ();return Ю.Ασ;}const int Εγ=64000;void ΓЇ(){ǒ.Clear();for(int i=Ѕ.Count-1;i>=0;i--){String Ρ=Ѕ[i].CustomData.ToUpper();
bool ƙ=false;if(Ρ=="INSTRUCTIONS"){ƙ=true;ΑЍ=true;}if(Ρ=="DEBUG") ƙ=true;if(ƙ){ǒ.Add(Ѕ[i]);Ѕ.RemoveAt(i);}}Ȗ(ǒ,false,1,false);}void ΓЈ(List<IMyTerminalBlock>ΑЀ){з.Clear();for(int i=0;i<ΑЀ.Count;i++){IMyTerminalBlock Ϊ=ΑЀ[i];try{String Βε=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=Ϊ.CustomName.IndexOf(Βε);int Ȏ=-1;if(Α<0||!int.TryParse(Ϊ.CustomName.Substring(Α+Βε.Length,1),out Ȏ)) continue;if(Ȏ==-1) continue;Ȏ--;IMyTextSurfaceProvider Βη=(IMyTextSurfaceProvider) Ϊ;if(Ȏ<Βη.SurfaceCount&&Ȏ>=0)
з.Add(Βη.GetSurface(Ȏ));}catch{}}}String Ƥ="";void Γτ(){String Ơ="";String ȥ="";Ƥ=Βύ(false);Ơ+=Ƥ;ȥ+=Ƥ;ȥ+=ΓΡ();for(int i=0;i<Ѕ.Count;i++) Ѕ[i].WriteText(Ơ);for(int i=0;i<з.Count;i++) з[i].WriteText(Ơ);Echo(ȥ);for(int i=0;i<ǒ.Count;i++){IMyTextPanel Љ=ǒ[i];String Ρ=Љ.CustomData.ToUpper();
if(Ρ=="DEBUG") Љ.WriteText(""+"\n"+"");if(Ρ=="INSTRUCTIONS") Љ.WriteText(ΑΗ());}}string ΑΗ(){String Β="";try{float ΒΫ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ǵ*ΒΫ)+" / "+(int)(Ǿ*ΒΫ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ǵ*100f,1)+"% / "+Math.Round(Ǿ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ȝ,2)+"ms / "+Math.Round(ƣ,2)+"ms \n";}catch{}for(int i=0;i<Ŏ.Count;i++)
Β+=""+Ŏ.Keys.ElementAt(i)+" = "+Math.Round(Ŏ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ŏ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Γά(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ȗ(Ѕ,setLCDFontAndSize,0.9f,false);Ȗ(з,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ѕ.Count;i++) Ѕ[i].WriteText(Β+Ǽ.ToString());for(int i=0;i<з.Count;i++) з[i].WriteText(Β+Ǽ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Αν(String π,String α,String Ǖ){try{if(!ȟ) return;π=BROADCAST_TAG+BROADCAST_SEP+ΑΔ()+BROADCAST_SEP+α+BROADCAST_SEP+Ɖ+BROADCAST_SEP+Ǖ+BROADCAST_SEP+π;this.IGC.SendBroadcastMessage(BROADCAST_TAG,π);}catch(Exception e){Ǽ=e;}}String ΑΔ(){
if(Ν!=Μ.Ѝ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΑΨ(String π){return π.StartsWith(BROADCAST_TAG);}bool Ѐ(ref String Ρ,out string ȩ,Char ǡ){int Α=Ρ.IndexOf(ǡ);ȩ="";if(Α<0) return false;ȩ=Ρ.Substring(0,Α);Ρ=Ρ.Remove(0,Α+1);return true;}bool Γπ(ref String π,out String ћ,out String Ǖ)
{ћ="";Ǖ="";if(!ȟ) return false;String Т="";if(!Ѐ(ref π,out Т,BROADCAST_SEP)||!ΑΨ(Т)) return false;if(!Ѐ(ref π,out ћ,BROADCAST_SEP)) return false;if(!Ѐ(ref π,out Т,BROADCAST_SEP)||(Т!=ΑΔ()&&(Т!="*"&&Ν!=Μ.Ѝ))) return false;if(!Ѐ(ref π,out Т,BROADCAST_SEP)||(Т!=Ɖ)) return false;
if(!Ѐ(ref π,out Ǖ,BROADCAST_SEP)) return false;return true;}void Γΰ(){bool Ȣ=true;if(ΒΕ){ΒΕ=false;if(Me.CustomData.Contains("Antenna_Name")){Φ="Update custom data";Me.CustomData="";}}String Ȭ=(Ν!=Μ.Ѝ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ȭ)
ΑΥ(Ȭ);String[]Η=Me.CustomData.Split('\n');ȟ=bool.Parse(ĝ(Ę(Η,"Enable_Broadcast",ref Ȣ)));bool ƾ=false;bool ΑЏ=true;if(ȟ){if(Ν!=Μ.Ѝ)
ć=ĝ(Ę(Η,"Ship_Name",ref Ȣ)).Replace(BROADCAST_SELF_ID,"");Ɖ=ĝ(Ę(Η,"Broadcast_Channel",ref Ȣ)).ToLower();ΑЏ=false;if(п==null){п=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);п.SetMessageCallback("");}List<IMyRadioAntenna>ǳ=new List<IMyRadioAntenna>();В.GetBlocksOfType(ǳ);bool ƅ=false;
for(int i=0;i<ǳ.Count;i++)
if(ǳ[i].EnableBroadcasting&&ǳ[i].Enabled){ƅ=true;break;}if(ǳ.Count==0) Φ="No Antenna found";else if(!ƅ) Φ="Antenna not ready";ƾ=ǳ.Count==0||!ƅ;if(Αω&&!ƾ&&Ν!=Μ.Ѝ) Φ="Antenna ok";}else if(Ν==Μ.Ѝ)
Φ="Offline - Enable in PB custom data";Αω=ƾ;if(ΑЏ){if(п!=null) this.IGC.DisableBroadcastListener(п);п=null;}}catch{Ȣ=false;}if(!Ȣ){Φ="Reset custom data";ΑΥ(Ȭ);}}void ΑΥ(String Ȭ){String Ρ=Ȭ+"\n\n"+"Enable_Broadcast="+(Ν==Μ.Ѝ?"true":"false")+" \n";Ρ+=Ν!=Μ.Ѝ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ρ+"Broadcast_Channel=#default";}String ĝ(String ǚ){int Α=ǚ.IndexOf("//");if(Α!=-1) ǚ=ǚ.Substring(0,Α);String[]ή=ǚ.Split('=');if(ή.Length<=1) return "";return ή[1].Trim();}String Ę(String[]Η,String ń,ref bool ƨ){foreach(String ǚ in Η)
if(ǚ.StartsWith(ń)) return ǚ;ƨ=false;return "";}void ΒЈ(string Њ){if(Ȫ) ό=new List<ο>();String ћ="";String Ǖ="";if(!ΑΨ(Њ)) Γυ(Њ);bool Βο=false;int Δό=50;while(п!=null&&п.HasPendingMessage&&Δό-->0){MyIGCMessage π=п.AcceptMessage();String Ρ=π.Data as String;if(Ρ==null) continue;
if(Γπ(ref Ρ,out ћ,out Ǖ)&&ћ!=""&&ћ!=BROADCAST_SELF_ID){ο Γ=Ť(ћ,"");if(Γ==null){Γ=new ο(ћ);ό.Add(Γ);}Γ.ǰ(Ǖ,false,false,true);ΔΔ(Γ,Ρ);Βο=true;}}if(Βο)
ό.Sort(delegate(ο a,ο b){if(a.γ==null&&b.γ==null) return 0;else if(a.γ==null) return-1;else if(b.γ==null) return 1;else return a.γ.CompareTo(b.γ);});if(Ȳ||Ȫ){if(ŏ<=0||Ȫ){Φ="";Ȫ=false;Γύ();Γΰ();ΒΦ=DateTime.Now;}}if(Ł) ΔΓ();ΔΒ();}void ΔΔ(ο Γ,String Ρ){Γ.ƛ=DateTime.Now;
String ȩ="";Ѐ(ref Ρ,out Γ.ŕ,BROADCAST_SEP);Ѐ(ref Ρ,out Γ.γ,BROADCAST_SEP);if(Γ.ŕ!=VERSION) return;String Ą="";String[]Ȅ=new string[3];Ѐ(ref Ρ,out Ą,BROADCAST_SEP);Ѐ(ref Ρ,out Ȅ[0],BROADCAST_SEP);Ѐ(ref Ρ,out Ȅ[1],BROADCAST_SEP);Ѐ(ref Ρ,out Ȅ[2],BROADCAST_SEP);Ѐ(ref Ρ,out Γ.ƶ,BROADCAST_SEP);
Ѐ(ref Ρ,out Γ.Φ,BROADCAST_SEP);Ѐ(ref Ρ,out ȩ,BROADCAST_SEP);Ѐ(ref Ρ,out Γ.ŉ,BROADCAST_SEP);String Βχ="",Βέ="";Ѐ(ref Ρ,out Βχ,BROADCAST_SEP);Ѐ(ref Ρ,out Βέ,BROADCAST_SEP);Γ.т.Clear();while(true){String ΒΡ;if(!Ѐ(ref Ρ,out ΒΡ,BROADCAST_SEP)) break;String[]ł=ΒΡ.Split('/');if(ł.Count()<3) continue;
int Є=0;if(!int.TryParse(ł[2],out Є)) continue;Γ.т.Add(new м(ł[0],ł[1],Є,Ě.Γέ));}int Βί=0;if(!int.TryParse(ȩ,out Γ.ι)) Γ.ι=0;if(!int.TryParse(Ą,out Βί)) Γ.Ν=Μ.я;if(!float.TryParse(Ȅ[0],out Γ.Σ.X)) Γ.Σ.X=0;if(!float.TryParse(Ȅ[1],out Γ.Σ.Y)) Γ.Σ.Y=0;if(!float.TryParse(Ȅ[2],out Γ.Σ.Z)) Γ.Σ.Z=0;
if(!float.TryParse(Βχ,out Γ.ǐ)) Γ.ǐ=0;if(!float.TryParse(Βέ,out Γ.Ǜ)) Γ.Ǜ=0;Γ.Ν=(Μ) Βί;Γ.Ə=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Σ));Γ.ƻ=0;for(int i=0;i<Γ.т.Count();i++) Γ.ƻ+=Γ.т[i].Є;}ο Ť(String ψ,String γ){ψ=ψ.ToUpper();γ=γ.ToUpper();for(int i=0;i<ό.Count;i++)
{if(ψ!=""&&ό[i].ψ.ToUpper()==ψ) return ό[i];if(γ!=""&&ό[i].γ!=null&&ό[i].γ.ToUpper()==γ) return ό[i];}return null;}void З(ο Γ,String ϋ){if(Γ==null){for(int i=0;i<ό.Count;i++)
ό[i].ǰ(ϋ,true,false,false);Αν(ϋ,"*",ϋ);}else{Γ.ǰ(ϋ,true,false,false);Αν(ϋ,Γ.ψ,ϋ);}}void Γυ(string Њ){if(Њ=="") return;var ή=Њ.ToUpper().Split(' ');ή.DefaultIfEmpty("");var ϋ=ή.ElementAtOrDefault(0);var Αϋ=ή.ElementAtOrDefault(1);String Δο="Invalid argument: "+Њ;if(ά==Ι.ȗ){if(κ!=null)
{switch(ϋ){case "UP":{if(ι<2) break;if(κ.ι==0){ι=1;return;}З(κ,"UP");return;}case "DOWN":{if(ι<1) break;if(ι==1){З(κ,"MRES");break;}З(κ,"DOWN");break;}case "APPLY":{if(ι<2) break;З(κ,"APPLY");return;}}}}switch(ϋ){case "UP":this.Γζ(false);return;case "DOWN":this.Γε(false);return;
case "APPLY":this.ǫ(true);return;}switch(ϋ){case "CLEAR":ΒІ();return;case "SENDALL":
if(String.IsNullOrEmpty(Αϋ)){Φ="Usage: SENDALL <cmd>";return;}З(null,Αϋ);return;case "SEND":
if(Њ.Length<="SEND".Length+1){Φ="Usage: SEND <ship>:<cmd>";return;}ΓϏ(Њ.Remove(0,"SEND".Length+1));return;}Φ=Δο;}void ΓϏ(String Αϊ){if(Αϊ=="") return;var ή=Αϊ.Split(':');if(ή.Length!=2){Φ="Missing separator \":\"";return;}ή.DefaultIfEmpty("");String ć=ή.ElementAtOrDefault(0).Trim();
ο Γ=Ť("",ć);if(Γ!=null) З(Γ,ή.ElementAtOrDefault(1).Trim());else Φ="Unknown ship: "+ć;}void ΒІ(){ό.Clear();κ=null;Υ(Ι.Main);ι=0;}void Γύ(){List<IMyTerminalBlock>Βπ=new List<IMyTerminalBlock>();В.GetBlocksOfType(Ѕ,Ů);В.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Βπ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ѕ=ΑΑ(Ѕ,pamTag,true);ΓЈ(Βπ);ΓЇ();Γώ();Ȗ(Ѕ,setLCDFontAndSize,1.15f,false);Ȗ(з,setLCDFontAndSize,1.15f,true);String Δλ=ΑΘ(false);String Βα="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Љ in Ѕ){if(Љ.CustomData==""){Љ.CustomData=ΑΘ(true);continue;}
if(!Љ.CustomData.Contains(Δλ))
if(!Љ.CustomData.Contains(Βα)) Љ.CustomData=Βα+Љ.CustomData;}}void ΔΒ(){String ȥ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<з.Count;i++)
з[i].WriteText(ǆ(σ.ΑΫ,"0",false));for(int i=0;i<Ѕ.Count;i++){σ ΰ=σ.я;String С="";Γρ(Ѕ[i],out ΰ,out С);Ѕ[i].WriteText(ǆ(ΰ,С,i==0));}Echo(ȥ);for(int i=0;i<ǒ.Count;i++){IMyTextPanel Љ=ǒ[i];String Ρ=Љ.CustomData.ToUpper();if(Ρ=="DEBUG") Љ.WriteText(""+"\n"+"");if(Ρ=="INSTRUCTIONS") Љ.WriteText(ΑΗ());
}}String ǫ(bool Ε){int Δή=0;return ǫ(Ε,0,ref Δή,false,1);}String ǫ(bool Ε,int ΔЃ,ref int ΔЏ,bool ΑІ,int ą){String Β="";String Ȍ="——————————————————\n";String ΕΛ="--------------------------------------------\n";if(κ==null||ά==Ι.Main||ά==Ι.ż)
Β+="[PAM]-Controller | "+ό.Count+" Connected ships"+"\n"+ΓΞ();else Β+=ΑΛ(κ)+"\n";Β+=Ȍ;int Α=0;int Δ=ι;ǜ=0;if(Φ!=""){Ζ(ref Β,Δ,Α++,Ε,Φ);}else if(ά==Ι.Main){if(κ==null&&ό.Count>=1) κ=ό[0];String Ѓ="";if(Ζ(ref Ѓ,Δ,Α++,Ε," Send command to all")) Υ(Ι.ż);int ȋ=0;
for(int s=0;s<ό.Count;s++){if(ΑІ) Ѓ+="\n";ο Γ=ό[s];if(Δ==Α||Δ==Α+1) κ=Γ;if(Α==Δ) ȋ=Ѓ.Split('\n').Length-1;if(Ζ(ref Ѓ,Δ,Α++,Ε," "+ΑΛ(Γ))){κ=Γ;Υ(Ι.ȗ);κ.ǰ("",false,true,false);}if(Α==Δ) ȋ=Ѓ.Split('\n').Length-1;if(Ζ(ref Ѓ,Δ,Α++,Ε," "+(ΑІ?"":"  ")+ΓΦ(Γ)))
{κ=Γ;Υ(Ι.Αξ);}}int Ȑ=ą-2;Ȑ+=Math.Max(0,(ΔЃ-1))*ą;Β+=Ż(Ȑ,Ѓ,ȋ,ref ΔЏ);}else if(ά==Ι.Ğ||ά==Ι.ż){Ι ΔΜ=ά==Ι.Ğ?Ι.ȗ:Ι.Main;ο α=null;if(ά==Ι.Ğ) α=κ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Υ(ΔΜ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǭ(α,"STOP"))){З(α,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǭ(α,"CONT"))){З(α,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǭ(α,"HOMEPOS"))){З(α,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǭ(α,"JOBPOS"))){З(α,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǭ(α,"FULL"))){З(α,"FULL");}if(α!=null&&α.Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǭ(α,"UNDOCK"))){З(α,"UNDOCK");}}else if(ά==Ι.ȗ){String Ƈ="";if(Ζ(ref Ƈ,Δ,Α++,Ε," Back")){Υ(Ι.Main);}Β+=Ƈ.Substring(0,Ƈ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Υ(Ι.Ğ);}if(!κ.ΑΪ()&&!κ.Ŭ()) ǜ++;if(κ.Ŭ()) Β+="No answer received...";
else Β+=Ǳ(κ,κ.ŉ);}else if(ά==Ι.Αξ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Υ(Ι.Main);}Β+=ΕΛ;Β+=Ǳ(κ,ΑΚ(κ));}if(!ƥ) ι=Math.Min(ǜ-1,ι);return Β;}String ΑΛ(ο Γ){if(Γ.ŕ!=VERSION) return Γ.γ+": Different version!";return Γ.γ+": "+Ǳ(Γ,Γ.ƶ)+" "+Γ.Ə+"m";}String ΓΦ(ο Γ){String s=Ÿ("",Ж(Γ.ǐ,Γ.Ǜ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.т.Count;i++){if(i>=5) break;м в=Γ.т[i];s+=Αχ(Ş(ΓΣ(в)),3)+" "+ΑΒ(в.Є)+" ";}return s;}String ΓΣ(м в){if(в.Ї.ToUpper()=="ORE"||в.Ї.ToUpper()=="INGOT"){String ΑЄ=ΓΠ(в.γ.ToUpper());if(ΑЄ!="") return ΑЄ;}return в.γ;}String Ǳ(ο Γ,String Β){if(Γ.ΑΪ()) return "No signal...("+ŧ((int)(DateTime.Now-Γ.ƛ).TotalSeconds)+")";
return Β;}String ΑΘ(bool Εά){String Β="";if(Εά) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Γρ(IMyTerminalBlock Љ,out σ ΰ,out String С){bool ƨ=true;String Ą=ĝ(Ę(Љ.CustomData.Split('\n'),"mode",ref ƨ)).ToUpper();String[]ή=Ą.Split(':');String ă=ή.Length>0?ή[0].Trim():"";С=ή.Length>1?ή[1].Trim():"";
ΰ=σ.я;if(ă=="MAIN") ΰ=σ.Αά;else if(ă=="MAINX") ΰ=σ.Αέ;else if(ă=="MENU") ΰ=σ.ű;else if(ă=="INVENTORY") ΰ=σ.ū;else if(ă=="DEBUG") ΰ=σ.š;if(ΰ==σ.я) С=Ą;}String ǆ(σ ΰ,String С,bool Δΰ){int ā=15;if(Δΰ){ΑЇ=Į;Į=0;ΑЈ=į;į=0;}String Β="";if(ΰ==σ.Αά){int Ј=0;
Β=ǫ(false,ΑЇ,ref ΔΨ,true,ā);if(!int.TryParse(С,out Ј)) return Β;Į=Math.Max(Ј,Į);Ј--;return Αθ(Ј,ā,Β);}else if(ΰ==σ.Αέ){int Ј=0;Β=ǫ(false,ΑЈ,ref ΔΩ,false,ā);if(!int.TryParse(С,out Ј)) return Β;į=Math.Max(Ј,į);Ј--;return Αθ(Ј,ā,Β);}if(ΰ==σ.ΑΫ){return ǫ(false,0,ref ΔΪ,true,ā);
}else if(ΰ==σ.ū||ΰ==σ.ű||ΰ==σ.š){ο Γ=κ;if(С!=""){Γ=Ť("",С);if(Γ==null) return "Unknown ship: "+С;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ȍ="—————————————————————\n";String ƺ="";String ƈ="";if(ΰ==σ.ű){ƺ="Menu";ƈ=Ǳ(Γ,Γ.ŉ);}
else if(ΰ==σ.ū){ƺ="Inventory";ƈ=Ǳ(Γ,ΑΚ(Γ));}else if(ΰ==σ.š){return Ǳ(Γ,ΓΧ(Γ));}Β+=Γ.γ+" - "+Γ.Ə+"m | "+Ş(""+ƺ)+"\n"+Ȍ;Β+=ƈ;return Β;}return "Unknown command: "+С;}string ΓΧ(ο Γ){String s="";s+=Γ.ŕ+"\n";s+=Γ.γ+"\n";s+=Γ.Σ.X+"\n";s+=Γ.Σ.Y+"\n";s+=Γ.Σ.Z+"\n";
s+=Γ.ƶ+"\n";s+=Γ.Φ+"\n";s+=Γ.ι+"\n";s+=Γ.ŉ.Length+"\n";s+=Γ.ǐ+"\n";s+=Γ.Ǜ+"\n";s+=Γ.т.Count()+"\n";return s;}string ΑΚ(ο Γ){String s="";s+=Ÿ("All",Ж(Γ.ǐ,Γ.Ǜ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.т.Count();i++){м в=Γ.т[i];s+=Ÿ(в.γ,в.Є,Γ.ƻ,30,8,10)+"\n";
}return s;}ς ΓΛ(ο Γ,bool ȡ,string ϋ){ς Ψ=ς.None;if(Γ==null){for(int i=0;i<ό.Count;i++){ς ϊ=ό[i].ǰ(ϋ,false,ȡ,false);if(ϊ==ς.Ź) Ψ=ϊ;if(ϊ==ς.Ŵ) return ϊ;if(ϊ==ς.ȕ) return ϊ;}return Ψ;}else return Γ.ǰ(ϋ,false,ȡ,false);}String ǭ(ο Γ,String ϋ){ς ϊ=ΓΛ(Γ,false,ϋ);if(ϊ==ς.Ź) return "received!";
if(ϊ==ς.ȕ) return "pending...";if(ϊ==ς.Ŵ) return "no answer!";return "";}String Ż(int Ȑ,String Β,int ȋ,ref int ф){String[]Η=Β.Split('\n');if(ȋ>=ф+Ȑ-1) ф++;ф=Math.Min(Η.Count()-1-Ȑ,ф);if(ȋ<ф+1) ф--;ф=Math.Max(0,ф);String Ψ="";for(int i=0;i<Ȑ;i++){int ΒΨ=i+ф;
if(ΒΨ>=Η.Count()) break;Ψ+=Η[ΒΨ]+"\n";}return Ψ;}String Αθ(int Ј,int ą,String Β){String[]Η=Β.Split('\n');int ΕΗ=Ј*ą;int ĺ=(Ј+1)*ą;String Ψ="";for(int i=ΕΗ;i<ĺ;i++){if(i>=Η.Count()) break;Ψ+=Η[i]+"\n";}return Ψ;}String Ş(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();
}String Αχ(String s,int Βά){if(s=="") return s;if(s.Length>Βά) s=s.Substring(0,Βά-1)+".";return s;}string Ÿ(String Ǚ,float value,float Ņ,int Αό,int ΒΤ,int ΔЂ){float ǟ=Ж(value,Ņ)*Αό;String ĩ="[";for(int i=0;i<Αό;i++){if(i<=ǟ) ĩ+="|";else ĩ+="'";}ĩ+="]";return ĩ+" "+Αχ(Ş(Ǚ),ΒΤ).PadRight(ΒΤ)+"".PadRight(ΔЂ)+ΑΒ(value);
}String ΑΒ(float Є){if(Є>=1000000) return Math.Round(Є/1000000f,Є/1000000f<100?1:0)+"M";if(Є>=1000) return Math.Round(Є/1000f,Є/1000f<100?1:0)+"K";return ""+Math.Round(Є);}String ŧ(int Ć){if(Ć>=60*60) return Math.Round(Ć/(60f*60f),1)+" h";if(Ć>=60) return Math.Round(Ć/60f,1)+" min";
return ""+Ć+" s";}List<IMyCameraBlock>Εϋ=new List<IMyCameraBlock>();List<long>ΖΙ=new List<long>();bool ΔЄ=false;int ΔΣ=0;int Εϊ=0;DateTime Εψ=DateTime.Now;String ΔΟ="";void ΒЅ(){ΔЄ=false;ΔΣ=0;ΔΟ="";}const String Εν="[PAMTFC]";const float Εξ=5f;const float Εζ=2f;class Ατ
{public long ψ;public Vector3 δ,Ζς;public Vector3 ΕЋ;public float ΕϏ;public String ΖΑ="";public int ΕЌ=-1;public bool ΖΟ;public long М=0;public int ΕЄ=0;public DateTime Ζβ;}IMyBroadcastListener Ζη=null;long Δύ=0;List<Ατ>Ζγ=new List<Ατ>();Vector3 Ζι=new Vector3();float Ζκ=-1;Vector3 Ζδ=new Vector3();
bool Ζζ=false;DateTime Ζε=DateTime.Now;const float Εμ=15f;const float Ελ=10f;int Ζλ=0;String Щ="";int ǝ=-1;Vector3 Ζθ=new Vector3();List<int>Ȯ=new List<int>();String ΒΝ="";void ΓΒ(){String ń=ω+"|"+Ў+"|"+(int) ǻ;if(ń==ΒΝ) return;ΒΝ=ń;Ȯ.Clear();int n=ω*Ў;if(ǻ==Я.ģ)
{for(int k=0;k<n;k++){int Ȉ=k/ω;Ȯ.Add(Ȉ*ω+(Ȉ%2==0?k-Ȉ*ω:ω-1-(k-Ȉ*ω)));}return;}int ΒΜ=(int) Math.Ceiling(ω/2f),ΒΛ=(int) Math.Ceiling(Ў/2f);int Δι=ω/2,Δθ=Ў/2;int x=0,y=0,Ɛ=0,Ƒ=-1,Ņ=Math.Max(ω,Ў);for(int i=0;i<Ņ*Ņ;i++){if(-ΒΜ<x&&x<=Δι&&-ΒΛ<y&&y<=Δθ) Ȯ.Add((y-1+ΒΛ)*ω+(x-1+ΒΜ));
if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ɛ;Ɛ=-Ƒ;Ƒ=t;}x+=Ɛ;y+=Ƒ;}}bool ΓΑ(){int n=Math.Max(0,ω*Ў);if(Щ.Length==n) return false;Щ=new String('0',n);ǝ=-1;return true;}void ΓЅ(int Ƌ,bool Ĺ){if(Ƌ<0||Ƌ>=Щ.Length) return;char[]h=Щ.ToCharArray();
h[Ƌ]=Ĺ?'1':'0';Щ=new String(h);}class ǯ{public long ψ;public Vector3 δ,Б,Ϗ;}List<ǯ>Ћ=new List<ǯ>();bool Ȥ=false;Vector3 Ķ,ĵ,ķ,ĸ;long ȯ=0;long Βδ=0;Χ Βγ=null;bool Δά=false;Χ ΒЏ(Vector3 δ,Vector3 Б,Vector3 Ϗ){MatrixD m=MatrixD.CreateWorld(δ,Б,Ϗ);Χ w=new Χ(Ξ);w.Ђ=Vector3D.TransformNormal(Ķ,m);
w.ύ=Vector3D.TransformNormal(ĵ,m);w.Г=Vector3D.TransformNormal(ķ,m);w.Σ=δ+(Vector3) Vector3D.TransformNormal(ĸ,m);w.β=true;return w;}void Γδ(){if(Ν==Μ.ζ||!Ξ.β) return;IMyShipConnector р=Ǭ(MyShipConnectorStatus.Connected);if(р==null||р.OtherConnector==null) return;IMyShipConnector Џ=р.OtherConnector;
Vector3I Δμ=new Vector3I((int) Ξ.Л.X,(int) Ξ.Л.Y,(int) Ξ.Л.Z);if(р.Position==Δμ){MatrixD ŀ=MatrixD.Transpose(Џ.WorldMatrix);Ķ=Vector3D.TransformNormal(І.WorldMatrix.Forward,ŀ);ĵ=Vector3D.TransformNormal(І.WorldMatrix.Down,ŀ);ķ=Vector3D.TransformNormal(І.WorldMatrix.Left,ŀ);ĸ=Vector3D.TransformNormal(р.GetPosition()-Џ.GetPosition(),ŀ);
Ȥ=true;}if(Vector3.Distance(р.GetPosition(),Ξ.Σ)<5) ȯ=Џ.EntityId;List<IMyShipConnector>Ǘ=new List<IMyShipConnector>();В.GetBlocksOfType(Ǘ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(Ǘ.Count>0){Ћ.Clear();for(int i=0;i<Ǘ.Count;i++){ǯ p=new ǯ();
p.ψ=Ǘ[i].EntityId;p.δ=Ǘ[i].GetPosition();p.Б=Ǘ[i].WorldMatrix.Forward;p.Ϗ=Ǘ[i].WorldMatrix.Up;Ћ.Add(p);}}Βγ=null;Βδ=0;if(Џ.EntityId==ȯ||!Ȥ) return;for(int i=0;i<Ћ.Count;i++)
if(Ћ[i].ψ==Џ.EntityId){Βδ=Ћ[i].ψ;Βγ=ΒЏ(Ћ[i].δ,Ћ[i].Б,Ћ[i].Ϗ);}}String ΒЎ(){String s=(Ȥ?"1":"0")+":"+Ϋ(Ķ)+":"+Ϋ(ĵ)+":"+
Ϋ(ķ)+":"+Ϋ(ĸ)+":"+ȯ;for(int i=0;i<Ћ.Count;i++)
s+=":"+Ћ[i].ψ+","+Ϋ(Ћ[i].δ)+","+Ϋ(Ћ[i].Б)+","+Ϋ(Ћ[i].Ϗ);return s;}void ΒЍ(String s){Ћ.Clear();Ȥ=false;ȯ=0;String[]f=s.Split(':');if(f.Length<6) return;Ȥ=f[0]=="1";Ķ=ί(f[1]);ĵ=ί(f[2]);ķ=ί(f[3]);ĸ=ί(f[4]);long.TryParse(f[5],out ȯ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǯ p=new ǯ();if(!long.TryParse(e[0],out p.ψ)) continue;p.δ=ί(e[1]);p.Б=ί(e[2]);p.Ϗ=ί(e[3]);Ћ.Add(p);}}const String Š="[PAMDCK]";const float ΒЀ=120f;class л{public long Γ;public int і;public long Ъ;public bool ΑЁ;public DateTime ǖ,ΒΥ;
}IMyBroadcastListener Ĵ=null;List<л>τ=new List<л>();List<IMyShipConnector>х=new List<IMyShipConnector>();List<IMySensorBlock>й=new List<IMySensorBlock>();List<IMyCameraBlock>ȳ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ř=new List<MyDetectedEntityInfo>();long Ц=0;int ƌ=0;
long ΑЂ=0;DateTime ΑЃ=DateTime.Now;String Ǔ="";int ΒΑ=0;IMyCubeGrid ц=null;void Γώ(){ц=Me.CubeGrid;В.GetBlocksOfType(х,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(dockPoolTag));В.GetBlocksOfType(й,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(zoneTag));
В.GetBlocksOfType(ȳ,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(laneTag));for(int i=0;i<й.Count;i++){й[i].DetectSmallShips=true;й[i].DetectLargeShips=true;}for(int i=0;i<ȳ.Count;i++) ȳ[i].EnableRaycast=true;}IMyShipConnector Ας(long ψ){if(ψ==0) return null;
IMyShipConnector c=В.GetBlockWithId(ψ) as IMyShipConnector;return c!=null&&ц!=null&&c.CubeGrid.IsSameConstructAs(ц)?c:null;}bool ŭ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΑΩ(IMyShipConnector c,long Γ){for(int i=0;i<ȳ.Count;i++)
{IMyCameraBlock Ƞ=ȳ[i];if(!Ƞ.IsWorking||Vector3D.Distance(Ƞ.GetPosition(),c.GetPosition())>10) continue;Vector3D ĺ=Ƞ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ƞ.CanScan(ĺ)) continue;MyDetectedEntityInfo Ļ=Ƞ.Raycast(ĺ);if(!Ļ.IsEmpty()&&Ļ.EntityId!=ц.EntityId&&Ļ.EntityId!=Γ&&Ļ.Type!=MyDetectedEntityType.FloatingObject)
{Ǔ="Lane blocked: "+c.CustomName;return false;}}return true;}bool Γγ(){for(int s=0;s<й.Count;s++){if(!й[s].IsWorking) continue;ř.Clear();й[s].DetectedEntities(ř);for(int i=0;i<ř.Count;i++){MyDetectedEntityInfo e=ř[i];bool Δε=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Δε&&e.EntityId!=Ц&&e.Velocity.Length()>0.5f){Ǔ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Γσ(л r){IMyShipConnector Ъ=Ας(r.Ъ);if(ŭ(Ъ)&&ΑΩ(Ъ,r.Γ)) return Ъ.EntityId;if(!r.ΑЁ||Ъ==null) return 0;IMyShipConnector Ī=null;double ΑϏ=0;for(int i=0;i<х.Count;i++)
{IMyShipConnector c=х[i];if(!ŭ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),Ъ.GetPosition());if(Ī==null||d<ΑϏ){if(ΑΩ(c,r.Γ)){Ī=c;ΑϏ=d;}}}return Ī==null?0:Ī.EntityId;}void ΓΪ(){л Ȃ=null;long р=0;for(int ō=2;ō>=1&&Ȃ==null;ō--){л Ǟ=null;for(int i=0;i<τ.Count;i++)
{л r=τ[i];if(r.і!=ō) continue;if(ō==1){long c=Γσ(r);if(c==0) continue;if(Ǟ==null||r.ǖ<Ǟ.ǖ){Ǟ=r;р=c;}}else if(Ǟ==null||r.ǖ<Ǟ.ǖ) Ǟ=r;}Ȃ=Ǟ;}if(Ȃ==null){if(τ.Count>0&&Ǔ=="") Ǔ="No free dock";return;}Ц=Ȃ.Γ;ƌ=Ȃ.і;ΑЂ=Ȃ.і==1?р:0;ΑЃ=DateTime.Now;
}void ΔΓ(){if(ц==null) return;if(Ĵ==null) Ĵ=IGC.RegisterBroadcastListener(Š);int ΔΠ=30;while(Ĵ.HasPendingMessage&&ΔΠ-->0){String Ρ=Ĵ.AcceptMessage().Data as String;if(Ρ!=null) ΓΫ(Ρ);}Γϋ();if(++ΒΑ<3) return;ΒΑ=0;IGC.SendBroadcastMessage(Š,ΒϏ());}void ΓΫ(String Ρ){try{String[]f=Ρ.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=τ.Count-1;i>=0;i--) if(τ[i].Γ==Γ) τ.RemoveAt(i);if(Ц==Γ) Ц=0;}else if(f[0]=="R"&&f.Length>=5){long Ъ=long.Parse(f[3]);if(Ας(Ъ)==null) return;л r=null;for(int i=0;i<τ.Count;i++) if(τ[i].Γ==Γ) r=τ[i];if(r==null){r=new л();r.Γ=Γ;r.ǖ=DateTime.Now;τ.Add(r);}
int і=int.Parse(f[2]);if(r.і!=і) r.ǖ=DateTime.Now;r.і=і;r.Ъ=Ъ;r.ΑЁ=f[4]=="1";r.ΒΥ=DateTime.Now;}}catch{}}void Γϋ(){for(int i=τ.Count-1;i>=0;i--)
if((DateTime.Now-τ[i].ΒΥ).TotalSeconds>3) τ.RemoveAt(i);л Ĭ=null;for(int i=0;i<τ.Count;i++) if(τ[i].Γ==Ц) Ĭ=τ[i];if(Ц!=0&&Ĭ==null) Ц=0;if(Ĭ!=null&&(DateTime.Now-ΑЃ).TotalSeconds>ΒЀ){Ц=0;Ĭ.ǖ=DateTime.Now;}Ǔ="";if(Ц==0&&τ.Count>0&&!Γγ()) ΓΪ();}String ΒϏ()
{String π="S;"+ц.EntityId+";"+Ц+";"+ƌ+";"+ΑЂ+";";for(int i=0;i<τ.Count;i++) π+=(i>0?",":"")+τ[i].Γ;π+=";";for(int i=0;i<х.Count;i++){IMyShipConnector c=х[i];π+=(i>0?":":"")+c.EntityId+","+Ϋ(c.GetPosition())+","+Ϋ(c.WorldMatrix.Forward)+","+Ϋ(c.WorldMatrix.Up);
}π+=";";for(int i=0;i<й.Count;i++){IMySensorBlock z=й[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();π+=(i>0?":":"")+Ϋ(z.GetPosition())+","+Math.Round(r,1);}return π;}String ΓΞ()
{if(х.Count==0&&й.Count==0) return "";int ΒΖ=0;for(int i=0;i<х.Count;i++) if(ŭ(х[i])) ΒΖ++;return "Docks: "+ΒΖ+"/"+х.Count+" free"+(Ц!=0?(ƌ==1?" | docking":" | undocking"):"")+
(τ.Count>1?" | "+(τ.Count-1)+" waiting":"")+(Ǔ!=""?"\n"+Ǔ:"")+"\n";}IMyBroadcastListener ΖΨ=null;int ΑБ=0;DateTime ΕІ=DateTime.Now;DateTime ΖΤ=DateTime.MinValue;long ΖΥ=0;int ΖΦ=0;long ΖΧ=0;bool ΕΦ=false;int ΕЇ=0;List<Vector3>Ζυ=new List<Vector3>();List<float>Ζφ=new List<float>();
bool ΕЅ=false;bool ΕЃ=false;Vector3 ΕЂ=new Vector3();void Γφ(){if(ΑБ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Š,"D;"+Δύ);ΑБ=0;ΕΦ=false;}void ΓЉ(string Њ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}