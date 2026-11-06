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
public enum Μ{я,ű,Ǯ,Ѝ,ζ}public enum Λ{Ǎ,ť,ΑΫ,Ÿ,Αζ,Ȓ,э,
Ť,ΓΑ,Ǫ,Ǆ,Βρ,ƾ,
Γρ,ș,Ș,Ş,ΓЏ,ΒЍ,
ΓЍ,ΓЎ,ΒЎ,ΒЏ,Ț}public enum Ο{н,Ŵ,џ,ǅ,ĕ,Αά,Αέ,đ,Ē}public enum щ{None,Ş,Ǎ,ΒЃ}public enum Ι{Main,Αδ,Ȕ,ȓ,ſ,Ĕ,
Ś,ΑΝ,ΑΞ,ΑΜ,ȗ,Αι,
ĝ,ź,Ž,Αλ,ġ}public enum ю{Αη,ΒϏ,ΓЂ,ΓΩ,ζ}public enum Я{Ģ,Βϋ}public enum ь{Αε,ΒБ,ΓЄ,Ũ}public enum Ф{Αί,ΒЂ,ΒЁ,ΒЉ,ΒЈ,ΓΧ,ΓΦ}public enum к{š,Βτ,ƿ}public enum ę{ΓΥ,Εΰ,Βς}public enum Ё{Αΰ,Αα,Αβ,Γη,Γζ,
Γα,Γβ,Γΰ,Γδ,
Γε,Γγ}public enum Π{Э,о,Ь,ē,ś,Ŝ}public enum Εβ{Εί,ΕΪ,Εέ}public enum Ю{Αξ,ǆ,Γί,Αή}public enum ς{None,ȕ,ŷ,Ų}public enum λ{Ɓ,ř,Ů,Ũ,ΑΩ,Ű,ΑΪ,ΓЉ,ų}public enum σ{я,ΑΧ,ΑΦ,ů,ũ,Š,ΑΨ}class Χ{public bool β=false;public Vector3 Σ=new Vector3();
public Vector3 ύ=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 Г=new Vector3();public Vector3 η=new Vector3();public Vector3 Л=new Vector3();public float Δς=0;public float Ζζ=0;public float[]У=null;public Χ(){}public Χ(Χ Џ){β=Џ.β;Σ=Џ.Σ;ύ=Џ.ύ;Ђ=Џ.Ђ;Г=Џ.Г;η=Џ.η;
Л=Џ.Л;У=Џ.У;}public Χ(Vector3 Σ,Vector3 Ђ,Vector3 ύ,Vector3 Г,Vector3 η){this.Σ=Σ;this.ύ=ύ;this.Ђ=Ђ;this.Г=Г;this.Δς=0;this.η=η;}public void Εά(List<IMyThrust>Ħ,List<string>ш){У=new float[ш.Count];for(int i=0;i<У.Length;i++)
У[i]=-1;for(int i=0;i<Ħ.Count;i++){string Ǧ=ΓΠ(Ħ[i]);int Α=ш.IndexOf(Ǧ);if(Α!=-1)
У[Α]=Ж(Ħ[i].MaxEffectiveThrust,Ħ[i].MaxThrust);}}}class Γκ{public Χ α=null;public List<Vector3>ΖΧ=new List<Vector3>();public float Εω=0;public float Ζκ=0;public float Ελ=0;public float ΕЂ=0;public Vector3 ΕЃ=new Vector3();}class ģ{public Ё Д=Ё.Αΰ;public float Ч=0;public float Ǖ=0;
public string б="";public string г="";DateTime ΒΠ;public bool Δώ=false;private bool Ƌ=false;public bool Βό(bool Ʃ){if(Ʃ){Ǖ=0;Ƌ=false;return false;}Ƌ=true;return Ǖ>Ч;}public void Εα(){Βό(true);Δώ=false;}public void Εη(){if(Ƌ)
if((DateTime.Now-ΒΠ).TotalSeconds>1){Ǖ++;ΒΠ=DateTime.Now;}}public bool ΑΚ(){switch(Д){case Ё.Αα:return true;case Ё.Αβ:return true;}return false;}}class м{public String γ="";public String І="";public int Є=0;public ę Ɯ=ę.Βς;public м(String γ,String І,int Є,ę Ɯ){this.γ=γ;this.І=І;
this.Є=Є;this.Ɯ=Ɯ;}}class ΒЄ{public ΒЄ(Vector3 δ,float ђ){this.δ=δ;this.ђ=ђ;}public Vector3 δ;public float ђ;}class ο{public DateTime ƙ;public DateTime ΑЀ;public String ψ="";public String γ="";public String Ŕ="";public String Φ="";public String ƴ="";public String ň="";public String Ƨ="";
public Vector3 Σ=new Vector3();public List<м>т=new List<м>();public Μ Ν=Μ.я;public ς ё;public float ǜ;public float Ǒ;public int ι;public int ƍ=0;public int ƹ=0;public bool ΑΥ(){return(DateTime.Now-ƙ).TotalSeconds>10;}public bool Ū(){if(ё!=ς.ȕ) return false;return(DateTime.Now-ΑЀ).TotalSeconds>=4;
}public ο(String ψ){this.ψ=ψ;}public ς ǰ(String ϋ,bool ΔЊ,bool ȡ,bool ΔΘ){if(ϋ==""&&!ȡ) return ς.None;if(ё==ς.ȕ&&Ū())
ё=ς.Ų;if(ΔЊ){Ƨ=ϋ;ё=ς.ȕ;ΑЀ=DateTime.Now;return ё;}else if(ȡ){Ƨ="";ё=ς.None;}else if(Ƨ==ϋ){if(ΔΘ) ё=ς.ŷ;return ё;}return ς.None;}}public String ΓΘ(String Δκ){switch(Δκ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem В;Vector3 Ββ=new Vector3();Vector3 Ъ=new Vector3();Vector3 ΔΫ=new Vector3();Vector3 Δν=new Vector3();Vector3 ΔΥ=new Vector3();DateTime ΒΟ=new DateTime();
bool Ȫ=true;int Ʒ=0;bool ȱ=false;bool ŀ=false;bool Δφ=true;bool Ζν=false;bool ΕЈ=false;bool Δζ=false;float ΔЌ=0;float Βζ=0;int ΔЄ=0;int Ζη=0;int Ŏ=0;int Ερ=0;float ǿ=0;float Ǵ=0;double ơ=0;float Ȝ=0;List<int>ǻ=new List<int>();List<int>ȉ=new List<int>();Μ Ν=Μ.я;
int ƭ=0;float ǥ=0;float ќ=0;float ȍ=0;IMyRemoteControl Ћ;IMySensorBlock φ;List<IMyTimerBlock>Βλ=new List<IMyTimerBlock>();List<IMyShipConnector>ȣ=new List<IMyShipConnector>();List<IMyThrust>Ċ=new List<IMyThrust>();List<IMyGyro>ΒΔ=new List<IMyGyro>();List<IMyTerminalBlock>Ы=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ƙ=new List<IMyLandingGear>();List<IMyReactor>ƨ=new List<IMyReactor>();List<IMyConveyorSorter>ΔЏ=new List<IMyConveyorSorter>();List<IMyGasTank>ȯ=new List<IMyGasTank>();List<IMyTerminalBlock>ț=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ετ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Εκ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Αψ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ѕ=new List<IMyTextPanel>();List<IMyTextSurface>з=new List<IMyTextSurface>();List<IMyTextPanel>Ǔ=new List<IMyTextPanel>();IMyTerminalBlock њ=null;Χ Ξ=new Χ();Χ ΒΘ=new Χ();
Χ ΒΜ=new Χ();bool ў=false;bool č=false;bool Ď=false;bool ΒΚ=false;List<String>ш=new List<string>();List<Χ>ρ=new List<Χ>();List<Χ>ŕ=new List<Χ>();int Εϊ=0;double ΕΠ=0;Χ Τ=new Χ();Vector3 ā;Vector3 Ȁ;Ο μ=Ο.н;Я Ǽ=Я.Ģ;int ω=0;int Ў=0;double ł=0;bool ΑІ=false;ģ ν=new ģ();
ģ ξ=new ģ();ь љ=ь.Αε;bool ď=true;Я ǣ=Я.Ģ;int К=3;int Й=3;int Ц=30;к с=к.š;bool Ǩ=true;bool Ȱ=true;bool Ȩ=true;bool Ǥ=false;Ф ȧ=Ф.Αί;bool ѝ=true;float д=90;float е=20;float П=5;float ж=20;float ѐ=0.70f;float ŗ=1.50f;float Ŗ=2.50f;float Đ=0f;float Ǻ=0f;Λ ı;
Ο ї;Λ Ĉ;Γκ ΖΔ=null;int Ļ=0;int ļ=0;int а=0;int ȳ=0;int ƫ=30;int ΖΖ=0;bool ΔБ=true;Vector3 ΕБ;double ΑЋ=0;bool Βδ=false;int ΕΡ=0;int ΖΡ=0;int Εμ=0;int ΖΓ=0;int ΖΑ=0;Vector3 ΖΕ=new Vector3();float ΕЎ=0;float ΖΣ=0;int ǧ=0;Χ ŏ=null;float ΕЏ=0;float Εν=0;float ΑЅ=0;
float Ơ=0;bool ΕЉ=false;bool Ζε=false;bool Ζι=false;bool ΕІ=false;bool Εο=false;DateTime ΕЁ=new DateTime();Χ Εχ=null;Χ ΔΣ=null;float Αω=0;щ ȝ;float ΒΙ=0;float Βξ=0;String Δπ="";float ƛ=0;float Ǒ=0;float ǜ=0;List<м>ΕЊ=new List<м>();float ώ=0;bool Ư=false;bool ΕΕ=false;
bool ΔЍ=false;bool Βκ=false;bool ΔЎ=false;float Δρ=0;float Δύ=0;Vector3 ΔΩ=new Vector3();Vector3 ΒΪ=new Vector3();Vector3 ΖΪ=new Vector3(1,1,1);float ΖΨ=1;Vector3 ΖΩ=new Vector3();bool ΕЇ=false;bool Δί=false;bool ΕА=false;bool Ει=false;float Εφ=0;float Εθ=2;Vector3 ΖΦ;
Vector3 ΖΥ;Vector3 ΖΤ;float[,]Č=new float[3,2];Dictionary<String,float[,]>ч=new Dictionary<string,float[,]>();Dictionary<String,float>Ζμ=new Dictionary<String,float>();int Εξ=0;Dictionary<String,float[]>ō=new Dictionary<string,float[]>();float ΖΗ;bool ΑЈ=false;int ħ=0;int є=0;
int ľ=0;int Ǚ=0;String Φ="";int[]ΒΨ=new int[Enum.GetValues(typeof(Ι)).Length];bool ƣ=false;int ǝ=0;int ι=0;Ι ά=Ι.Main;int ΕΖ=0,ΕΗ=0,ΕΘ=0,ΕΙ=0;int ΔЅ=0,ΔІ=0;bool ȟ=false;bool Ατ=false;String Ć="";String Ƈ="";IMyBroadcastListener п=null;bool ΑА=true;List<ο>ό=null;
ο κ=null;int ĭ=0,Į=0;int ΑЂ=0,ΑЃ=0;int ΔΞ=0,ΔΟ=0,ΔΠ=0;String Δλ="";Exception ǽ=null;bool Βγ=false;Ο Ƭ=Ο.н;bool ΑЇ=false;bool ΕЍ=true;bool ΖΙ=false;String ΕΞ="";bool Βν=false;bool Ƴ=true;int Ɩ=0;int[]ΖΛ=null;Vector3 ƈ=new Vector3();bool ΒΒ=false;bool Ŭ(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;
Program(){В=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΑΛ(Me,controllerTag,true))
Ν=Μ.Ѝ;Υ(Ι.Main);if(Ν!=Μ.Ѝ){Φ="Welcome to [PAM]!";Ю ƪ=Γο();if(Ν==Μ.я){List<IMyShipDrill>ΑЎ=new List<IMyShipDrill>();List<IMyShipGrinder>ΒΓ=new List<IMyShipGrinder>();В.GetBlocksOfType(ΑЎ,q=>q.CubeGrid==Me.CubeGrid);В.GetBlocksOfType(ΒΓ,q=>q.CubeGrid==Me.CubeGrid);if(ΑЎ.Count>0)
{Ν=Μ.ű;Φ="Miner mode enabled!";}else if(ΒΓ.Count>0){Ν=Μ.Ǯ;Φ="Grinder mode enabled!";}else{Ν=Μ.ζ;Ακ(ю.ζ);}}if(ƪ==Ю.Αξ) Δφ=false;if(ƪ==Ю.ǆ) Φ="Data restore failed!";if(ƪ==Ю.Αή) Φ="New version, wipe data";}}void Main(string Љ,UpdateType ΕΛ){try{if(ǽ!=null){if(Љ.Trim().ToUpper()=="RETRY")
{ǽ=null;Ȫ=true;ΔЄ=0;if(Ν!=Μ.Ѝ) Ǐ();Φ="Restarted after error";}else{ΓΤ();Γμ();return;}}ŀ=(ΕΛ&UpdateType.Update10)!=0;if(ŀ) Ʒ++;ȱ=Ʒ>=10;if(ȱ) Ʒ=0;if(ŀ){ħ++;if(ħ>4) ħ=0;Ŏ=(int) Math.Max(0,10-(DateTime.Now-ΒΟ).TotalSeconds);}if(Љ!="") Δλ=Љ;if(Ν!=Μ.Ѝ)
ΓЁ(Љ);else
ΒЀ(Љ);ƣ=false;try{int ΑЄ=Runtime.CurrentInstructionCount;float Ǡ=Ж(ΑЄ,Runtime.MaxInstructionCount);if(Ǡ>0.90) Φ="Max. instructions >90%";if(Ǡ>ǿ) ǿ=Ǡ;if(ΑЈ){ǻ.Add(ΑЄ);while(ǻ.Count>10) ǻ.RemoveAt(0);Ǵ=0;for(int i=0;i<ǻ.Count;i++) Ǵ+=ǻ[i];Ǵ=Ж(Ж(Ǵ,ǻ.Count),Runtime.MaxInstructionCount);
double ƚ=Runtime.LastRunTimeMs;if(Δζ&&ƚ>ơ) ơ=ƚ;ȉ.Add((int)(ƚ*1000f));while(ȉ.Count>10) ȉ.RemoveAt(0);Ȝ=0;for(int i=0;i<ȉ.Count;i++) Ȝ+=ȉ[i];Ȝ=Ж(Ȝ,ȉ.Count)/1000f;}}catch{ǿ=0;}}catch(Exception e){ǽ=e;}}String ΓΙ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Υ(Ι Ї){ΒΨ[(int) ά]=ι;ι=ΒΨ[(int) Ї];if(Ї==Ι.ȗ) ι=0;ά=Ї;if(Ν!=Μ.Ѝ)
ĥ(ά==Ι.Ȕ,false,0,0);ƣ=true;}void Γή(bool Ɲ){if(ι>0) ι--;else if(Ɲ) ι=ǝ-1;}void Γέ(bool Ɲ){if(ι<ǝ-1) ι++;else if(Ɲ) ι=0;}String Ακ(ю π){switch(π){case ю.Αη:Φ="Job is running";break;case ю.ΒϏ:Φ="Connector not ready!";break;case ю.ΓЂ:Φ="Ship modified, path outdated!";break;
case ю.ΓΩ:Φ="Interrupted by player!";break;case ю.ζ:Φ="Shuttle mode enabled!";break;}return "";}String ΓΟ(Я δ){switch(δ){case Я.Ģ:return "Top-Left";case Я.Βϋ:return "Center";default:return "";}}String ΓΕ(к ΰ){switch(ΰ){case к.Βτ:return "Auto"+(Ν==Μ.ű?" (Ore)":"");case к.ƿ:return "Auto (+Stone)";
case к.š:return "Default";default:return "";}}String ΓΜ(Ο ϊ){switch(ϊ){case Ο.н:return "No job";case Ο.Ŵ:return "Job paused";case Ο.џ:return "Job active";case Ο.đ:return "Job active";case Ο.Ē:return "Job active";case Ο.ǅ:return "Job done";case Ο.ĕ:return "Job changed";case Ο.Αά:return "Move home";
case Ο.Αέ:return "Move to job";}return "";}String ǋ(ь ΔΕ){switch(ΔΕ){case ь.Αε:return "Return home";case ь.ΒБ:return "Fly to job pos";case ь.ΓЄ:return "Stop";case ь.Ũ:return "Ignore";}return "";}String ΓΗ(Ф ΰ){switch(ΰ){case Ф.Αί:return "Off";case Ф.ΒЉ:return "Drop pos (Stone) ";case Ф.ΒЈ:return "Drop pos (Sto.+Ice)";
case Ф.ΒЂ:return "Cur. pos (Stone)";case Ф.ΒЁ:return "Cur. pos (Sto.+Ice)";case Ф.ΓΧ:return "In motion (Stone)";case Ф.ΓΦ:return "In motion (Sto.+Ice)";}return "";}String ǉ(щ ϊ){switch(ϊ){case щ.None:return "No batteries";case щ.Ş:return "Charging";case щ.ΒЃ:return "Discharging";}return "";}String ΑΔ(Λ ϊ)
{String ΕЌ=Ν==Μ.ζ?"target":"job";switch(ϊ){case Λ.Ǎ:return "Idle";case Λ.ť:return "Flying to XY position";case Λ.ΑΫ:return Ν==Μ.Ǯ?"Grinding":"Mining";case Λ.Ÿ:return "Returning";case Λ.ΒЎ:return "Flying to drop pos";case Λ.Αζ:return "Returning to dock";case Λ.Ȓ:return "Flying to dock area";
case Λ.э:return "Flying to job area";case Λ.Ť:return "Flying to path";case Λ.ΓΑ:return "Flying to job position";case Λ.Ǫ:return "Approaching dock";case Λ.Ǆ:return "Docking";case Λ.Βρ:return "Aligning";case Λ.ƾ:return "Aligning";case Λ.Γρ:return "Retry docking";case Λ.ș:return "Unloading";case Λ.Ț:return ΕΞ;
case Λ.Ș:return "Undocking";case Λ.Ş:return "Charging batteries";case Λ.ΓЏ:return "Waiting for uranium";case Λ.ΒЍ:return "Filling up hydrogen";case Λ.ΓЍ:return "Waiting for ejection";case Λ.ΓЎ:return "Waiting for ejection";case Λ.ΒЏ:return "Flying to drop pos";}return "";}String ΑΙ(Ё Д){switch(Д)
{case Ё.Αΰ:return "On \"Undock\" command";case Ё.Αα:return "On player entered cockpit";case Ё.Γη:return "On ship is full";case Ё.Γζ:return "On ship is empty";case Ё.Αβ:return "On time delay";case Ё.Γβ:return "On batteries empty(<25%)";case Ё.Γΰ:return "On batteries empty(=0%)";case Ё.Γα:return "On batteries full";
case Ё.Γε:return "On hydrogen empty(<25%)";case Ё.Γγ:return "On hydrogen empty(=0%)";case Ё.Γδ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Βε,int ѓ,bool Ε,String ǚ){ǝ+=1;if(Βε==ѓ) ǚ=">"+ǚ+(ħ>=2?" .":"");else ǚ=" "+ǚ;Β+=ǚ+"\n";return Βε==ѓ&&Ε;}String Βυ(bool Ε)
{int Α=0;int Δ=ι;ǝ=0;String Ȍ="———————————————\n";String ȏ="--------------------------------------------\n";String Β="";Β+=ΓΜ(μ)+" | "+(Ξ.β?"Ready to dock":"No dock")+"\n";Β+=Ȍ;double Βι=Math.Max(Math.Round(this.ΑЋ),0);if(ά==Ι.Main){bool ѕ=Ν==Μ.ζ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΓЃ();
if(Ν==Μ.ű)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Υ(Ι.Ȕ);if(Ν==Μ.Ǯ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Υ(Ι.Ȕ);if(Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Υ(Ι.Ž);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ǂ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΒА();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΓΒ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ѕ) Υ(Ι.ġ);else Υ(Ι.Ĕ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Υ(Ι.ΑΝ);if(Ν!=Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Υ(Ι.ΑΜ);}else if(ά==Ι.Ȕ){double ΕΣ=Math.Round(К*ǥ,1);double Δβ=Math.Round(Й*ќ,1);String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Start new job!")) Αμ();if(Ζ(ref ε,Δ,Α++,Ε," Change current job")){ƀ(false,false);Υ(Ι.Main);}if(Ζ(ref ε,Δ,Α++,Ε," Width + (Width: "+К+" = "+ΕΣ+"m)")){ǩ(ref К,5,20,1);ǲ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Width -")){ǩ(ref К,-5,20,-1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height + (Height: "+Й+" = "+Δβ+"m)")){ǩ(ref Й,5,20,1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Height -")){ǩ(ref Й,-5,20,-1);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth + ("+(с==к.š?"Depth":"Min")+": "+Ц+"m)")){ǩ(ref Ц,5,50,2);ǲ(true);}
if(Ζ(ref ε,Δ,Α++,Ε," Depth -")){ǩ(ref Ц,-5,50,-2);ǲ(true);}if(Ζ(ref ε,Δ,Α++,Ε," Depth mode: "+ΓΕ(с))){с=ы(с);}if(Ζ(ref ε,Δ,Α++,Ε," Start pos: "+ΓΟ(ǣ))){ǣ=ы(ǣ);}if(Ν==Μ.Ǯ&&с==к.ƿ) с=ы(с);Β+=Ź(8,ε,Δ,ref ΔЅ);}else if(ά==Ι.Ž){float[]ΑЉ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Υ(Ι.Αλ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΑΙ(ν.Д))) ν.Д=ы(ν.Д);if(!ν.ΑΚ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ŧ((int) ν.Ч))) ν.Ч=ǃ(ν.Ч,ΑЉ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ΑΙ(ξ.Д))) ξ.Д=ы(ξ.Д);
if(!ξ.ΑΚ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ŧ((int) ξ.Ч))) ξ.Ч=ǃ(ξ.Ч,ΑЉ);}else if(ά==Ι.Αλ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) Αμ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Ž);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.б!=""?ν.б:"-"))) ν.б=ě(ref ΕΖ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ν.г!=""?ν.г:"-"))) ν.г=ě(ref ΕΘ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.б!=""?ξ.б:"-"))) ξ.б=ě(ref ΕΗ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(ξ.г!=""?ξ.г:"-"))) ξ.г=ě(ref ΕΙ);
}else if(ά==Ι.ȓ){String ΒΣ=д+" %";if(ȱ) є++;if(є>1){є=0;ľ++;if(ľ>1) ľ=0;}bool[]ư=new bool[]{ƨ.Count==0,ȝ==щ.None,ȯ.Count==0};int Βμ=0;while(true){Βμ++;Ǚ++;if(Ǚ>ư.Length-1) Ǚ=0;if(Βμ>=ư.Length) break;if(!ư[Ǚ]) break;}bool ѕ=Ν==Μ.ζ;if(!ѕ&&Ǩ&&ώ!=-1&&ľ==0)
ΒΣ=ώ<1000000?Math.Round(ώ)+" Kg":Math.Round(ώ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){Ǐ();Υ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ѕ) Υ(Ι.Ĕ);else Υ(Ι.ġ);if(!ѕ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) Βπ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) Βν=true;Β+=ȏ;if(!ѕ) Β+="Progress: "+Math.Round(ł,1)+" %\n";Β+="State: "+ΑΔ(ı)+" "+Βι+"m \n";Β+="Load: "+ƛ+" % Max: "+ΒΣ+" \n";if(Ǚ==0) Β+="Uranium: "+(ƨ.Count==0?"No reactors":Math.Round(Βξ,1)+"Kg "+(П==-1?"":" Min: "+П+" Kg"))+"\n";
if(Ǚ==1) Β+="Battery: "+(ȝ==щ.None?ǉ(ȝ):Αω+"% "+(е==-1||ѕ?"":" Min: "+е+" %"))+"\n";if(Ǚ==2) Β+="Hydrogen: "+(ȯ.Count==0?"No tanks":Math.Round(ΒΙ,1)+"% "+(ж==-1||ѕ?"":" Min: "+ж+" %"))+"\n";}else if(ά==Ι.Ĕ){String ε="";if(Ζ(ref ε,Δ,Α++,Ε," Back")){if(μ==Ο.џ) Υ(Ι.ȓ);else Υ(Ι.Main);}
if(Ζ(ref ε,Δ,Α++,Ε," Max load: "+д+"%")) Ќ(ref д,д<=80?-10:-5,λ.Ů,false);if(Ζ(ref ε,Δ,Α++,Ε," Weight limit: "+(Ǩ?"On":"Off"))) Ǩ=!Ǩ;if(Ζ(ref ε,Δ,Α++,Ε," Ejection: "+ΓΗ(ȧ))) ȧ=ы(ȧ);if(Ζ(ref ε,Δ,Α++,Ε," Toggle sorters: "+(Ǥ?"On":"Off"))){Ǥ=!Ǥ;if(Ǥ) ż(Ƴ);}
if(Ζ(ref ε,Δ,Α++,Ε," Unload ice: "+(ѝ?"On":"Off"))) ѝ=!ѝ;if(Ζ(ref ε,Δ,Α++,Ε," Uranium: "+(П==-1?"Ignore":"Min "+П+"Kg"))) Ќ(ref П,(П>5?-5:-1),λ.Ű,true);if(Ζ(ref ε,Δ,Α++,Ε," Battery: "+(е==-1?"Ignore":"Min "+е+"%"))) Ќ(ref е,-5,λ.ΑΩ,true);if(Ζ(ref ε,Δ,Α++,Ε," Hydrogen: "+(ж==-1?"Ignore":"Min "+ж+"%"))) Ќ(ref ж,-10,λ.ΑΪ,true);
if(Ζ(ref ε,Δ,Α++,Ε," When done: "+(ď?"Return home":"Stop"))) ď=!ď;if(Ζ(ref ε,Δ,Α++,Ε," On damage: "+ǋ(љ))) љ=ы(љ);if(Ζ(ref ε,Δ,Α++,Ε," Advanced...")) Υ(Ι.Ś);Β+=Ź(8,ε,Δ,ref ΔІ);}else if(ά==Ι.Ś){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.џ) Υ(Ι.ȓ);else Υ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ν==Μ.Ǯ?" Grinder":" Drill")+" inv. balancing: "+(Ȱ?"On":"Off"))) Ȱ=!Ȱ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ν==Μ.Ǯ?" grinders":" drills")+": "+(Ȩ?"Fwd + Bwd":"Fwd"))) Ȩ=!Ȩ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+ŗ+"m/s")) Ќ(ref ŗ,0.5f,λ.Ɓ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ŗ+"m/s")) Ќ(ref Ŗ,0.5f,λ.Ɓ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѐ*100f)+"%"+(ѐ>0.80f?" (risky)":""))) Ќ(ref ѐ,0.1f,λ.ř,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+Đ*100f+"%")) ǁ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+Ǻ*100f+"%")) ǁ(false,0.05f);
}else if(ά==Ι.ġ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(μ==Ο.џ) Υ(Ι.ȓ);else Υ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+д+"%")) Ќ(ref д,д<=80?-10:-5,λ.Ů,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ѝ?"On":"Off"))) ѝ=!ѝ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(П==-1?"Ignore":"Min "+П+"Kg"))) Ќ(ref П,(П>5?-5:-1),λ.Ű,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(е==-1?"Ignore":"Charge up"))) е=(е==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(ж==-1?"Ignore":"Fill up"))) ж=(ж==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ǋ(љ))) љ=ы(љ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѐ*100f)+"%"+(ѐ>0.80f?" (risky)":""))) Ќ(ref ѐ,0.1f,λ.ř,false);
}else if(ά==Ι.Αδ){double Δμ=0;if(ρ.Count>0) Δμ=Vector3.Distance(ρ.Last().Σ,Ъ);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΓЇ();if(Ν!=Μ.ζ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ў?"Use old home":(Ξ.β?"Was set! ":"none ")))) ў=!ў;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ў?"Use old connector":(Ξ.β?"Was set! ":"none ")))) ў=!ў;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(č?"Use old connector":(Τ.β?"Was set! ":"none ")))) č=!č;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(Ď?"Use old path":(ρ.Count>1?"Count: "+ρ.Count:"none ")))) Ď=!Ď;Β+=ȏ;Β+="Wp spacing: "+Math.Round(ΕΠ)+"m\n";}else if(ά==Ι.ſ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){Ǐ();Υ(Ι.Main);}
Β+=ȏ;Β+="State: "+ΑΔ(ı)+" \n";Β+="Speed: "+Math.Round(ΔЌ,1)+"m/s\n";Β+="Target dist: "+Βι+"m\n";Β+="Wp count: "+ρ.Count+"\n";Β+="Wp left: "+ΕΡ+"\n";}else if(ά==Ι.ΑΝ){List<IMyTerminalBlock>ǒ=ΓΔ();if(ȱ) є++;if(є>=ǒ.Count) є=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Υ(Ι.ΑΞ);
Β+=ȏ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ƛ,1)+"% "+Math.Round(Ǒ,1)+" / "+Math.Round(ǜ,1)+"\n";Β+="Uranium: "+(ƨ.Count==0?"No reactors":Math.Round(Βξ,1)+"Kg "+Δπ)+"\n";Β+="Battery: "+(ȝ==щ.None?"":Αω+"% ")+ǉ(ȝ)+"\n";Β+="Hydrogen: "+(ȯ.Count==0?"No tanks":Math.Round(ΒΙ,1)+"% ")+"\n";
Β+="Damage: "+(ǒ.Count==0?"None":""+(є+1)+"/"+ǒ.Count+" "+ǒ[є].CustomName)+"\n";}else if(ά==Ι.ΑΞ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=ȏ;Β+="Next scan: "+Ŏ+"s\n";Β+="Ship size: "+Math.Round(ǥ,1)+"m "+Math.Round(ќ,1)+"m "+Math.Round(ȍ,1)+"m \n";
Β+="Broadcast: "+(ȟ?"Online - "+Ć:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ǿ*100f,1)+"% \n";}else if(ά==Ι.ΑΜ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Υ(Ι.Main);Β+=ȏ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ƭ==2) Β="Fatal setup error\nNext scan: "+Ŏ+"s\n";if(Βγ) Β="Recompile script now";int Δξ=Β.Split('\n').Length;for(int i=Δξ;i<=10;i++) Β+="\n";Β+=Ȍ;Β+="Last: "+Φ+"\n";
return Β;}void ǁ(bool Δι,float Ő){Ǐ();ƀ(true,false);if(Δι) Ќ(ref Đ,Ő,λ.ų,false);else Ќ(ref Ǻ,Ő,λ.ų,false);Βϊ();ĥ(true,true,0,0);}float ǃ(float ǵ,float[]œ){float Ψ=œ[0];for(int i=œ.Length-1;i>=0;i--)
if(ǵ<œ[i]) Ψ=œ[i];return Ψ;}String ě(ref int ѓ){String Ψ="";if(ѓ>=Βλ.Count) ѓ=-1;if(ѓ>=0) Ψ=Βλ[ѓ].CustomName;ѓ++;return Ψ;}void ǩ(ref int value,int Ȟ,int ő,int Ʊ){if(Ȟ==0) return;if(value<ő&&Ʊ>0||value<=ő&&Ʊ<0){value+=Ʊ;return;}int ΓБ=Math.Abs(Ȟ);int Ƃ=0;int Ƥ=1;
while(true){Ƃ+=Ƥ*ΓБ*10;if(Ȟ<0&&value-ő<=Ƃ) break;if(Ȟ>0&&value-ő<Ƃ) break;Ƥ++;}value+=Ƥ*Ȟ;}void ǲ(bool Ζθ){К=Math.Max(К,1);Й=Math.Max(Й,1);Ц=Math.Max(Ц,0);ĥ(ά==Ι.Ȕ,false,0,0);}void Ќ(ref float value,float Ő,λ І,bool ċ){Ќ(ref value,value+Ő,І,ċ,true);
}void Ќ(ref float value,float ȁ,λ І,bool ċ,bool ΕΦ){float Р=0,О=0;if(І==λ.Ɓ){Р=0.5f;О=10f;}if(І==λ.ř){Р=0.1f;О=1f;}if(І==λ.Ũ){Р=50f;О=100f;}if(І==λ.ΑΩ){Р=5f;О=30f;}if(І==λ.Ű){Р=1f;О=25f;}if(І==λ.Ů){Р=10f;О=95f;}if(І==λ.ΑΪ){Р=10f;О=90f;}
if(І==λ.ΓЉ){Р=10f;О=1800;}if(І==λ.ų){Р=0.0f;О=0.75f;}if(ȁ==-1&&ċ){value=-1;return;}if(value==-1) ċ=false;bool Βά=ȁ<Р||ȁ>О;if(Βά&&ΕΦ){if(ȁ<value) value=О;else if(ȁ>value) value=Р;}else value=ȁ;if(Βά&&ċ) value=-1;else value=Math.Max(Р,Math.Min(value,О));
value=(float) Math.Round(value,2);}void Βπ(bool ΔЀ){if(ΔЀ) а=Math.Max(0,а-1);else а++;ΒЋ();ΒЊ();for(int k=0;k<ȭ.Count&&k<Ш.Length;k++) Γύ(ȭ[k],k<а);Ǟ=-1;Ασ(true);}T ы<T>(T ǵ){int Α=Array.IndexOf(Enum.GetValues(ǵ.GetType()),ǵ);Α++;if(Α>=ΒЌ(ǵ)) Α=0;return(T) Enum.GetValues(ǵ.GetType()).GetValue(Α);
}int ΒЌ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЃ(){ŕ.Clear();for(int i=0;i<ρ.Count;i++) ŕ.Add(ρ[i]);ρ.Clear();ΒΚ=true;ΒΘ=new Χ(Ξ);ΒΜ=new Χ(Τ);Ξ.β=false;if(Ν==Μ.ζ) Τ.β=false;for(int i=0;i<ч.Count;i++)
if(!ш.Contains(ч.Keys.ElementAt(i)))
ш.Add(ч.Keys.ElementAt(i));ў=false;č=false;Ď=false;Υ(Ι.Αδ);}void ΓЇ(){if(ў) Ξ=ΒΘ;if(č) Τ=ΒΜ;if(Ď){ρ.Clear();for(int i=0;i<ŕ.Count;i++) ρ.Add(ŕ[i]);}ΒΚ=false;Ǐ();Υ(Ι.Main);}void ğ(Χ М,Ο ϊ){ΔΣ=М;if(μ==Ο.џ) ї=ϊ;}bool ǌ(Χ М,float ђ,bool ΕΜ,out Vector3 δ){if(ΕΜ){Vector3I Δή=new Vector3I((int) М.Л.X,(int) М.Л.Y,(int) М.Л.Z);
IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(Δή);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){δ=new Vector3();return false;}Vector3 ΔΝ=Ě(Ћ,Ϊ.FatBlock.GetPosition()-Ъ);Vector3 ΔΜ=Ě(Ћ,Ϊ.FatBlock.WorldMatrix.Forward);δ=М.Σ-Απ(М.Ђ,М.ύ*-1,ΔΝ)-Απ(М.Ђ,М.ύ*-1,ΔΜ)*ђ;return true;}
else{δ=М.Σ;return true;}}Vector3 ΓΛ(int ΔΚ,int Ȉ,bool ΔЁ){if(!ΔЁ&&ΒΒ) return ƈ;float ΔΛ=((ω-1f)/2f)-ΔΚ;float ΔЃ=((Ў-1f)/2f)-Ȉ;ƈ=Τ.Σ+Τ.Г*ΔΛ*ǥ+Ȁ*-1*ΔЃ*ќ;ΒΒ=true;return ƈ;}Π Ţ(){float И=-1;Π Ψ=Π.Э;if(Ν!=Μ.ζ){Γά();if(μ!=Ο.н){Vector3 υ=Αρ(ā,Ȁ*-1,Ъ-Τ.Σ);
if(Math.Abs(υ.X)<=(float)(ω*ǥ)/2f&&Math.Abs(υ.Y)<=(float)(Ў*ќ)/2f){if(υ.Z<=-1&&υ.Z>=-ƫ*2) return Π.ē;if(υ.Z>-1&&υ.Z<ȍ*2) return Π.Э;}if(ъ(Τ.Σ,ref И)) Ψ=Π.Э;}if(Ξ.β){if(ъ(Ξ.Σ,ref И)) Ψ=Π.Ь;for(int i=0;i<ρ.Count;i++)
if(ъ(ρ[i].Σ,ref И)) Ψ=Π.о;if(Vector3.Distance(Ъ,Ξ.Σ)<dockDist*ȍ) Ψ=Π.Ь;if(Ǭ(MyShipConnectorStatus.Connectable)!=null||Ǭ(MyShipConnectorStatus.Connected)!=null)
Ψ=Π.Ь;}}else{Vector3 δ=new Vector3();IMyShipConnector Ĭ=Ǭ(MyShipConnectorStatus.Connected);if(Ξ.β){if(ъ(Ξ.Σ,ref И)) Ψ=Π.Ь;if(ǌ(Ξ,dockDist,true,out δ))
if(ъ(δ,ref И)) Ψ=Π.Ь;if(Ĭ!=null&&Vector3.Distance(Ĭ.GetPosition(),Ξ.Σ)<5)
return Π.ś;}for(int i=0;i<ρ.Count;i++)
if(Vector3.Distance(ρ[i].Σ,Ξ.Σ)>dockDist*ȍ&&Vector3.Distance(ρ[i].Σ,Τ.Σ)>dockDist*ȍ)
if(ъ(ρ[i].Σ,ref И)) Ψ=Π.о;if(Τ.β){if(ъ(Τ.Σ,ref И)) Ψ=Π.Э;if(ǌ(Τ,dockDist,true,out δ))
if(ъ(δ,ref И)) Ψ=Π.Э;if(Ĭ!=null&&Vector3.Distance(Ĭ.GetPosition(),Τ.Σ)<5)
return Π.Ŝ;}}return Ψ;}bool ъ(Vector3 δ,ref float И){float ђ=Vector3.Distance(δ,Ъ);if(ђ<И||И==-1){И=ђ;return true;}return false;}void Αμ(){if(ƭ>0){Φ="Setup error! Can't start";return;}if(Ν==Μ.ζ){ǂ();return;}Τ.Σ=Ъ;Τ.η=Ћ.GetNaturalGravity();Τ.Ђ=ΔΫ;Τ.ύ=ΔΥ;Τ.Г=Δν;
ā=њ.WorldMatrix.Forward;Ȁ=Τ.ύ;if(ā==Ћ.WorldMatrix.Down) Ȁ=Ћ.WorldMatrix.Backward;ƀ(true,true);έ(Λ.ť);ž();}void ƀ(bool Н,bool ΔЂ){if(μ==Ο.н&&!Н) return;bool Δχ=Н||μ==Ο.ǅ||ω!=К||Ў!=Й||Ǽ!=ǣ;if(Δχ){if(μ!=Ο.н){μ=Ο.ĕ;ΓΛ(Ļ,ļ,ΔЂ);Φ="Job changed, lost progress";}Ǽ=ǣ;
ω=К;Ў=Й;ļ=0;Ļ=0;Ш="";Ǟ=-1;Ơ=0;ȳ=0;ΑЅ=0;а=0;Ασ(true);}}void ŵ(){Γϋ(Ъ,0);Ğ(Ċ,true);}void ž(){Ακ(ю.Αη);ŵ();Ż(Ƙ,false);ż(Ƴ);μ=Ο.џ;Βω(true);ї=μ;Υ(Ι.ȓ);Γτ();ΑІ=true;Ɩ=0;for(int i=ț.Count-1;i>=0;i--)
if(ΑΠ(ț[i],false)) Ɩ++;if(Ɩ>0) Φ="Started with damage";}void Ǐ(){if(μ==Ο.џ){μ=Ο.Ŵ;Φ="Job paused";}έ(Λ.Ǎ);ї=μ;Γό(false,0,0,0,0);ΓЅ();Βσ(new Vector3(),false);ΓІ();Γψ(ChargeMode.Auto);Γώ(false);Γϊ(true);ǎ(Λ.Ǎ);ĥ(false,false,0,0);Ğ(Ы,false);Ğ(ȣ,true);ż(true);Βδ=false;Βύ();
ΔΡ=false;ΔΤ=false;Γξ();ΑІ=false;Ư=false;Βν=false;if(ά!=Ι.Main&&ά!=Ι.Ĕ&&ά!=Ι.Ś&&ά!=Ι.ġ)
Υ(Ι.Main);}void ǂ(){Π у=Ţ();if(Ν==Μ.ζ){if(!Τ.β||!Ξ.β) return;ž();bool Ɠ=Vector3.Distance(Ъ,Ξ.Σ)<Vector3.Distance(Ъ,Τ.Σ);if(Ƭ==Ο.đ) Ɠ=true;if(Ƭ==Ο.Ē) Ɠ=false;if(Ɠ){ğ(Ξ,Ο.đ);switch(у){case Π.ś:έ(Λ.Ț);break;case Π.о:έ(Λ.э);break;case Π.Ь:έ(Λ.Ǫ);break;default:έ(Λ.Ș);break;
}}else{ğ(Τ,Ο.Ē);switch(у){case Π.Ŝ:έ(Λ.Ț);break;case Π.Э:έ(Λ.Ǫ);break;case Π.о:έ(Λ.э);break;default:έ(Λ.Ș);break;}}}else{if(μ!=Ο.Ŵ&&μ!=Ο.ĕ) return;bool ΕΟ=μ==Ο.ĕ;ž();bool ƕ=ΓΪ(false)&&Ξ.β;switch(у){case Π.Э:έ(ƕ?Λ.Ť:Λ.ť);break;case Π.о:έ(ƕ?Λ.Ȓ:Λ.э);break;case Π.Ь:έ(ƕ?Λ.Ǫ:Λ.ș);break;
case Π.ē:{if(ȳ!=а||ΕΟ) έ(Λ.Ÿ);else έ(Λ.ΑΫ);}break;default:break;}}}void ΓΒ(){if(μ==Ο.н&&!Ξ.β) return;if(Ν==Μ.ζ&&(!Τ.β||!Ξ.β)) return;Φ="Move to job";Π у=Ţ();if(Ν==Μ.ζ){ğ(Τ,Ο.Ē);switch(у){case Π.Э:έ(Λ.Ǫ);break;case Π.о:έ(Λ.э);break;case Π.Ŝ:return;default:έ(Λ.Ș);break;
}ǎ(Λ.Ț);}else{switch(у){case Π.Э:έ(Λ.э);break;case Π.о:έ(Λ.э);break;case Π.Ь:έ(Λ.ș);break;case Π.ē:έ(Λ.Ÿ);break;default:break;}if(μ==Ο.н) ǎ(Λ.э);else ǎ(Λ.ƾ);Βδ=true;}ŵ();Υ(Ι.ſ);Ż(Ƙ,false);ї=Ο.Αέ;}void ΒА(){if(!Ξ.β) return;Φ="Move home";Π у=Ţ();if(Ν==Μ.ζ){ğ(Ξ,Ο.đ);switch(у)
{case Π.о:έ(Λ.Ȓ);break;case Π.Ь:έ(Λ.Ǫ);break;case Π.ś:return;default:έ(Λ.Ș);break;}ǎ(Λ.Ț);}else{if(Ǭ(MyShipConnectorStatus.Connected)!=null) return;if(Ǭ(MyShipConnectorStatus.Connectable)!=null){έ(Λ.Ǆ);ǎ(Λ.ș);return;}switch(у){case Π.Э:έ(Λ.Ť);break;case Π.о:έ(Λ.Ȓ);break;case Π.Ь:έ(Λ.Ȓ);break;
case Π.ē:έ(Λ.Αζ);break;default:break;}ǎ(Λ.ș);}ŵ();Υ(Ι.ſ);Ż(Ƙ,false);ї=Ο.Αά;}void έ(Λ ƥ){if(ƥ==Λ.Ǎ) Ĉ=Λ.Ǎ;if(Ĉ!=Λ.Ǎ&&ı==Ĉ&&ƥ!=Ĉ){Ǐ();return;}ΔБ=true;ı=ƥ;}void ǎ(Λ ϊ){this.Ĉ=ϊ;}void Ασ(bool Ʃ){if(Ʃ){ł=0;return;}if(Ν==Μ.ζ) return;float ĸ=а*Math.Max(1,Ц);if(ȳ==а) ĸ+=Math.Min(Ц,ΑЅ);
float ΕΚ=ω*Ў*Math.Max(1,Ц);ł=Math.Max(ł,(float) Math.Min(ĸ/ΕΚ*100.0,100));}List<IMyTerminalBlock>ΓΔ(){List<IMyTerminalBlock>ǒ=new List<IMyTerminalBlock>();for(int i=0;i<ț.Count;i++){IMyTerminalBlock Ϊ=ț[i];if(ΑΠ(Ϊ,false)) ǒ.Add(Ϊ);}return ǒ;}bool ΑΠ(IMyTerminalBlock Ϊ,bool Ƶ)
{return(!ΑΡ(Ϊ,Ƶ)||!Ϊ.IsFunctional);}bool ΑΡ(IMyTerminalBlock Ϊ,bool Ƶ){if(Ϊ==null) return false;try{IMyCubeBlock ΒΑ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(Ƶ) return ΒΑ==Ϊ;else return ΒΑ.GetType()==Ϊ.GetType();}catch{return false;}}bool ΓΪ(bool ΖΚ){if(Ǩ&&Ν!=Μ.ζ)
if(ώ!=-1&&Βζ>=ώ){Φ="Ship too heavy";return true;}if(ƛ>=д||Ư){Ư=false;Φ="Ship is full";return true;}return false;}static String ΓΠ(IMyThrust и){return и.BlockDefinition.SubtypeId;}Vector3 ŭ(Vector3 А,float[,]Ǿ){return new Vector3(
А.X>=0?Ǿ[0,0]:Ǿ[0,1],
А.Y>=0?Ǿ[1,0]:Ǿ[1,1],
А.Z>=0?Ǿ[2,0]:Ǿ[2,1]);}bool ΑΘ(Χ Ω,String Ǧ,out float Ɛ){Ɛ=0;int Α=ш.IndexOf(Ǧ);if(Α==-1||Ω.У==null||Α>=Ω.У.Length)
return false;Ɛ=Ω.У[Α];if(Ɛ==-1) return false;return true;}Vector3 ΑΗ(Vector3 А,Χ Ω){if(Ω!=null){Vector3 Ψ=new Vector3();for(int i=0;i<ч.Keys.Count;i++){String Ǧ=ч.Keys.ElementAt(i);float Ȧ=0;if(!ΑΘ(Ω,Ǧ,out Ȧ))
return ŭ(А,Č);Ψ+=ŭ(А,ч.Values.ElementAt(i))*Ȧ;}return Ψ;}return ŭ(А,Č);}float ŧ(Vector3 А,Χ Ω){return ŧ(А,new Vector3(),Ω);}float ŧ(Vector3 А,Vector3 η,Χ Ω){Vector3 ΕΔ=ΑΗ(А,Ω);Vector3 ΔΧ=ΕΔ+η*Βζ;float Ǡ=(ΔΧ/А).AbsMin();return(float)(А*Ǡ).Length();}float Ę(Vector3 ȫ,Vector3 Ϗ,Vector3 η,Χ Ω)
{if(η.Length()==0f) return 0;Vector3 Δο=Αρ(ȫ,Ϗ,Vector3.Normalize(η));float Н=ŧ(-Δο,Ω);return Н/η.Length();}void Βω(bool Δε){float Е=0;float Ƒ=0.9f;if(Δε){ώ=-1;ǧ=0;ŏ=null;if(μ!=Ο.н&&Τ.η.Length()!=0){Е=Ƒ*Ę(Τ.Ђ,Τ.ύ*-1,Τ.η,null);if(Е<ώ||ώ==-1) ώ=Е;}if(Ξ.β&&Ξ.η.Length()!=0)
{Е=Ƒ*Ę(Ξ.Ђ,Ξ.ύ*-1,Ξ.η,null);if(Е<ώ||ώ==-1) ώ=Е;}return;}if(ǧ==-1) return;if(ǧ>=0){int Βΰ=0;while(ǧ<ρ.Count){if(Βΰ>100) return;Βΰ++;Χ Ω=ρ[ǧ];if(Ω.η.Length()!=0f){Е=Ƒ*Math.Min(
Ę(Ω.Ђ,Ω.ύ*-1,Ω.η,Ω),
Ę(Ω.Ђ*-1,Ω.ύ*-1,Ω.η,Ω));if(Е<ώ||ώ==-1) ώ=Е;}else ŏ=Ω;ǧ++;}ǧ=-1;}bool Ɣ=true;float ј=0;if(ŏ!=null){for(int i=0;i<ч.Count;i++){String Ǧ=ч.Keys.ElementAt(i);float[,]ƺ=ч.Values.ElementAt(i);float Ȧ=0;if(!ΑΘ(ŏ,Ǧ,out Ȧ)){Ɣ=false;break;}for(int a=0;a<ƺ.GetLength(0);a++)
for(int b=0;b<ƺ.GetLength(1);b++){float ǐ=Math.Abs(ƺ[a,b]*Ȧ);if(ǐ==0) continue;Ɣ=true;if(ј==0||ǐ<ј) ј=ǐ;}}}if(!Ɣ){for(int a=0;a<Č.GetLength(0);a++)
for(int b=0;b<Č.GetLength(1);b++){float ǐ=Math.Abs(Č[a,b]);if(ǐ==0) continue;if(ј==0||ǐ<ј) ј=ǐ;}}if(ј>0){float Δσ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Е=Ж(ј,Δσ);if(Е>0)
if(Е<ώ||ώ==-1) ώ=Е;}}void Βσ(Vector3 Н,bool χ){if(!χ){for(int i=0;i<Ċ.Count;i++)
Ċ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ƅ=ΑΗ(Н,null);float ΕΧ=Math.Min(1,Math.Abs(Ж(Н.X,Ƅ.X)));float ΕΨ=Math.Min(1,Math.Abs(Ж(Н.Y,Ƅ.Y)));float ΕΩ=Math.Min(1,Math.Abs(Ж(Н.Z,Ƅ.Z)));for(int i=0;i<Ċ.Count;i++){IMyThrust и=Ċ[i];Vector3 Ƕ=Γς(Ě(Ћ,и.WorldMatrix.Backward),1);
if(Ƕ.X!=0&&Math.Sign(Ƕ.X)==Math.Sign(Н.X))
и.SetValueFloat("Override",и.MaxThrust*ΕΧ);else if(Ƕ.Y!=0&&Math.Sign(Ƕ.Y)==Math.Sign(Н.Y))
и.SetValueFloat("Override",и.MaxThrust*ΕΨ);else if(Ƕ.Z!=0&&Math.Sign(Ƕ.Z)==Math.Sign(Н.Z))
и.SetValueFloat("Override",и.MaxThrust*ΕΩ);else
и.SetValueFloat("Override",0.0f);}}void Γό(bool χ,float ΔϏ,float Ȅ,float ȇ,float ȑ){for(int i=0;i<ΒΔ.Count;i++){IMyGyro θ=ΒΔ[i];θ.GyroOverride=χ;if(!χ) θ.GyroPower=100;else θ.GyroPower=ΔϏ;if(!χ) continue;Vector3 Б=Ћ.WorldMatrix.Forward;Vector3 Ȇ=Ћ.WorldMatrix.Right;Vector3 Ϗ=Ћ.WorldMatrix.Up;
Vector3 Ƿ=θ.WorldMatrix.Forward;Vector3 ǹ=θ.WorldMatrix.Up;Vector3 Ǹ=θ.WorldMatrix.Left*-1;if(Ƿ==Б) θ.SetValueFloat("Roll",ȑ);else if(Ƿ==(Б*-1)) θ.SetValueFloat("Roll",ȑ*-1);else if(ǹ==(Б*-1)) θ.SetValueFloat("Yaw",ȑ);else if(ǹ==Б) θ.SetValueFloat("Yaw",ȑ*-1);else if(Ǹ==Б) θ.SetValueFloat("Pitch",ȑ);
else if(Ǹ==(Б*-1)) θ.SetValueFloat("Pitch",ȑ*-1);if(Ǹ==(Ȇ*-1)) θ.SetValueFloat("Pitch",Ȅ);else if(Ǹ==Ȇ) θ.SetValueFloat("Pitch",Ȅ*-1);else if(ǹ==Ȇ) θ.SetValueFloat("Yaw",Ȅ);else if(ǹ==(Ȇ*-1)) θ.SetValueFloat("Yaw",Ȅ*-1);else if(Ƿ==(Ȇ*-1)) θ.SetValueFloat("Roll",Ȅ);
else if(Ƿ==Ȇ) θ.SetValueFloat("Roll",Ȅ*-1);if(ǹ==(Ϗ*-1)) θ.SetValueFloat("Yaw",ȇ);else if(ǹ==Ϗ) θ.SetValueFloat("Yaw",ȇ*-1);else if(Ǹ==Ϗ) θ.SetValueFloat("Pitch",ȇ);else if(Ǹ==(Ϗ*-1)) θ.SetValueFloat("Pitch",ȇ*-1);else if(Ƿ==Ϗ) θ.SetValueFloat("Roll",ȇ);else if(Ƿ==(Ϗ*-1)) θ.SetValueFloat("Roll",ȇ*-1);
}}void ΓІ(){this.Δί=false;}void ΓЅ(){Ġ(false,false,false,ΒΪ,0);Βκ=false;}void Γϋ(Vector3 α,float ć){Ġ(true,false,false,α,ć);}void Ġ(bool χ,bool Ʋ,bool ƻ,Vector3 α,float ć){Ġ(χ,Ʋ,ƻ,α,α-Ъ,0.0f,ć);}void Ġ(bool χ,bool Ʋ,bool ƻ,Vector3 α,Vector3 ΔЈ,float ΔЉ,float ć){Βκ=true;
this.ΕΕ=χ;ΒΪ=α;this.Δρ=ć;this.Δύ=ΔЉ;this.ΔЎ=Ʋ;this.ΔЍ=ƻ;this.ΔΩ=ΔЈ;ΑЋ=Vector3.Distance(α,Ъ);}void Γτ(){В.GetBlocksOfType(ț,Ŭ);}void Βϊ(){if(Ћ==null) return;њ=null;float ŉ=0,Ņ=0,Ŋ=0,ņ=0,ŋ=0,Ň=0;List<IMyTerminalBlock>ƶ=Ǉ(Ы,pamTag,true);bool Δω=ƶ.Count==0;
if(ƶ.Count>0) њ=ƶ[0];else if(Ы.Count>0) њ=Ы[0];int İ=0;for(int i=0;i<Ы.Count;i++){if(Ы[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward){if(Δω){ƭ=2;Φ="Mining direction is unclear!";return;}continue;}İ++;Vector3 υ=ΑΓ(Ћ,Ы[i].GetPosition());if(i==0){ŉ=υ.X;Ņ=υ.X;Ŋ=υ.Y;ņ=υ.Y;ŋ=υ.Z;Ň=υ.Z;}
Ņ=Math.Max(υ.X,Ņ);ŉ=Math.Min(υ.X,ŉ);ņ=Math.Max(υ.Y,ņ);Ŋ=Math.Min(υ.Y,Ŋ);Ň=Math.Max(υ.Z,Ň);ŋ=Math.Min(υ.Z,ŋ);}ǥ=(Ņ-ŉ)*(1-Đ)+drillRadius*2;ќ=(ņ-Ŋ)*(1-Ǻ)+drillRadius*2;if(њ!=null&&њ.WorldMatrix.Forward==Ћ.WorldMatrix.Down)
ќ=(Ň-ŋ)*(1-Ǻ)+drillRadius*2;}List<T>Ǉ<T>(List<T>Ѓ,String Т,bool Ľ){List<T>Ψ=new List<T>();for(int i=0;i<Ѓ.Count;i++)
if(ΑΛ(Ѓ[i],Т,Ľ)) Ψ.Add(Ѓ[i]);return Ψ;}bool ΑΛ<T>(T Ϊ,String Т,bool Ľ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(Ľ&&b.CustomName.ToUpper().Contains(Т.ToUpper())) return true;if(!Ľ&&b.BlockDefinition.ToString().ToUpper().Contains(Т.ToUpper())) return true;return false;}void Ğ<T>(List<T>Ѓ,bool χ)
{for(int i=0;i<Ѓ.Count;i++)
Γω((IMyTerminalBlock) Ѓ[i],χ);}void Γω(IMyTerminalBlock Ϊ,bool χ){if(Ϊ==null) return;String ΔΑ=χ?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(ΔΑ);a.Apply(Ϊ);}void ż(bool χ){Ƴ=χ;if(!Ǥ) return;Ğ(ΔЏ,χ);}void Γψ(ChargeMode ΰ){for(int i=0;i<Αψ.Count;i++) Αψ[i].ChargeMode=ΰ;}void Γώ(bool ΕΒ)
{for(int i=0;i<ȯ.Count;i++) ȯ[i].Stockpile=ΕΒ;}void Ż(List<IMyLandingGear>ƒ,bool ΒΤ){for(int i=0;i<ƒ.Count;i++){if(ΒΤ) ƒ[i].Lock();if(!ΒΤ) ƒ[i].Unlock();}}void Γϊ(bool χ){if(ΑЇ==χ) return;List<IMyShipController>į=new List<IMyShipController>();В.GetBlocksOfType(į,Ŭ);if(į.Count==0) return;
for(int i=0;i<į.Count;i++)
į[i].DampenersOverride=χ;ΑЇ=χ;}IMyShipConnector Ǭ(MyShipConnectorStatus ΕΑ){for(int i=0;i<ȣ.Count;i++){if(!ΑΡ(ȣ[i],true)) continue;if(ȣ[i].Status==ΕΑ) return ȣ[i];}return null;}void ĥ(bool Ʈ,bool Ĳ,float Δά,float ΔΔ){if(φ==null||Ы.Count==0) return;Vector3 Ī=new Vector3();int İ=0;
for(int i=0;i<Ы.Count;i++){if(Ы[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward) continue;İ++;Ī+=Ы[i].GetPosition();}Ī=Ī/İ;Vector3 Ȃ=ΑΓ(φ,Ī);φ.Enabled=true;φ.ShowOnHUD=Ʈ;φ.LeftExtend=(Ĳ?1:К)/2f*ǥ-Ȃ.X;φ.RightExtend=(Ĳ?1:К)/2f*ǥ+Ȃ.X;φ.TopExtend=(Ĳ?1:Й)/2f*ќ+Ȃ.Y;
φ.BottomExtend=(Ĳ?1:Й)/2f*ќ-Ȃ.Y;φ.FrontExtend=(Ʈ?Ц:Δά)-Ȃ.Z;φ.BackExtend=Ʈ?0:ΔΔ+ȍ*0.75f+Ȃ.Z;φ.DetectFloatingObjects=true;φ.DetectAsteroids=false;φ.DetectLargeShips=true;φ.DetectSmallShips=true;φ.DetectStations=true;φ.DetectOwner=true;φ.DetectSubgrids=false;
φ.DetectPlayers=false;φ.DetectEnemy=true;φ.DetectFriendly=true;φ.DetectNeutral=true;}void Ȗ<T>(List<T>Ѓ,bool ΔЋ,float ΔΪ,bool Δθ){for(int i=0;i<Ѓ.Count;i++){IMyTextSurface ĉ=null;if(Ѓ[i]is IMyTextSurface) ĉ=(IMyTextSurface) Ѓ[i];if(ĉ!=null){ĉ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΔЋ) continue;ĉ.Font="Debug";if(Δθ) continue;ĉ.FontSize=ΔΪ;}}}static float Ж(float a,float b){if(b==0) return 0;return a/b;}Vector3 Γς(Vector3 v,int ƌ){return new Vector3(Math.Round(v.X,ƌ),Math.Round(v.Y,ƌ),Math.Round(v.Z,ƌ));}Vector3 ΑΓ(IMyTerminalBlock Ϊ,Vector3 ΕΥ){return Ě(Ϊ,ΕΥ-Ϊ.GetPosition());
}Vector3 Ě(IMyTerminalBlock Ϊ,Vector3 ƽ){return Vector3D.TransformNormal(ƽ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 Αρ(Vector3 ȫ,Vector3 Ϗ,Vector3 ƽ){MatrixD Ɵ=MatrixD.CreateFromDir(ȫ,Ϗ);return Vector3D.TransformNormal(ƽ,MatrixD.Transpose(Ɵ));}Vector3 Απ(Vector3 ȫ,Vector3 Ϗ,Vector3 υ){MatrixD Ɵ=MatrixD.CreateFromDir(ȫ,Ϗ);
return Vector3D.Transform(υ,Ɵ);}String Ϋ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ί(String s){String[]ή=s.Split('|');return new Vector3(float.Parse(Θ(ή,0)),float.Parse(Θ(ή,1)),float.Parse(Θ(ή,2)));}String ΓА(Χ Ω){String Ǣ=":";String Ψ=Ϋ(Ω.Σ)+Ǣ+Ϋ(Ω.Ђ)+Ǣ+
Ϋ(Ω.ύ)+Ǣ+Ϋ(Ω.Г)+Ǣ+Ϋ(Ω.η);for(int i=0;i<Ω.У.Length;i++)
Ψ+=Ǣ+Math.Round(Ω.У[i],3);return Ψ;}Χ ΓЈ(String s){String[]ή=s.Split(':');Χ Ω=new Χ(
ί(Θ(ή,0)),ί(Θ(ή,1)),
ί(Θ(ή,2)),ί(Θ(ή,3)),
ί(Θ(ή,4)));int Α=5;List<float>ΑЏ=new List<float>();while(Α<ή.Length){String Œ=Θ(ή,Α);float f=0;if(!float.TryParse(Œ,out f)) break;ΑЏ.Add(f);Α++;}Ω.У=ΑЏ.ToArray();return Ω;}System.Text.StringBuilder Ȋ=new System.Text.StringBuilder();void Κ<T>(T value,bool Δψ){if(Δψ) Ȋ.Append('\n');Ȋ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String Ė(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ĥ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΔΒ,int Α){String Œ=ΔΒ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Œ)) return "";
return Œ;}void Save(){if(Βγ||Ν==Μ.Ѝ){Storage="";return;}Storage=Βφ();}String Βφ(){Ȋ.Clear();Ȋ.Append(DATAREV+";");Κ(Ϋ(Ββ),false);Κ(Ϋ(Ξ.Ђ));Κ(Ϋ(Ξ.Г));Κ(Ϋ(Ξ.ύ));Κ(Ϋ(Ξ.η));Κ(Ϋ(Ξ.Σ));Κ(Ϋ(Ξ.Л));Κ(Ξ.β);Κ(Ϋ(Τ.Σ));Κ(Ϋ(Τ.η));Κ(Ϋ(Τ.Ђ));Κ(Ϋ(Τ.ύ));Κ(Ϋ(Τ.Г));Κ(Ϋ(Τ.Л));Κ(Τ.β);Κ(Ϋ(ā));Κ(Ϋ(Ȁ));Κ(";");
Κ((int) Ν,false);Κ((int) μ);Κ((int) ї);Κ(д);Κ(е);Κ(П);Κ(ж);Κ(ѐ);Κ(Ǩ);Κ(ѝ);if(Ν==Μ.ζ){Κ((int) ν.Д);Κ(ν.Ч);Κ(ν.Ǖ);Κ(Ė(ν.б));Κ(Ė(ν.г));Κ((int) ξ.Д);Κ(ξ.Ч);Κ(ξ.Ǖ);Κ(Ė(ξ.б));Κ(Ė(ξ.г));}else{Κ((int) ǣ);Κ((int) љ);Κ((int) ȧ);Κ((int) с);Κ((int) Ǽ);Κ(ď);Κ(Ǥ);Κ(Ȱ);Κ(Ȩ);Κ(К);Κ(Й);Κ(Ц);Κ(ŗ);Κ(Ŗ);Κ(Đ);Κ(Ǻ);
Κ(ω);Κ(Ў);Κ(Ļ);Κ(ļ);Κ(ȳ);Κ(а);Κ(ƫ);Κ(Ơ);}Κ(";");for(int i=0;i<ш.Count;i++)
Κ((i>0?"|":"")+ш[i],false);Κ(";");for(int i=0;i<ρ.Count;i++)
Κ(ΓА(ρ[i]),i>0);Κ(";",false);Κ(Ш+"|"+Ǟ,false);Κ(";",false);Κ(ΒІ(),false);String Ρ=Ȋ.ToString();Ȋ.Clear();return Ρ;}Ю Γο(){return Γπ(Storage);}Ю Γπ(String Ρ){if(Ρ=="") return Ю.Γί;String[]ǡ=Ρ.Split(';');if(Θ(ǡ,0)!=DATAREV) return Ю.Αή;int Α=0;try{String[]Η=Θ(ǡ,1).Split('\n');
Ββ=ί(Θ(Η,Α++));Ξ.Ђ=ί(Θ(Η,Α++));Ξ.Г=ί(Θ(Η,Α++));Ξ.ύ=ί(Θ(Η,Α++));Ξ.η=ί(Θ(Η,Α++));Ξ.Σ=ί(Θ(Η,Α++));Ξ.Л=ί(Θ(Η,Α++));Ξ.β=bool.Parse(Θ(Η,Α++));Τ.Σ=ί(Θ(Η,Α++));Τ.η=ί(Θ(Η,Α++));Τ.Ђ=ί(Θ(Η,Α++));Τ.ύ=ί(Θ(Η,Α++));Τ.Г=ί(Θ(Η,Α++));Τ.Л=ί(Θ(Η,Α++));Τ.β=bool.Parse(Θ(Η,Α++));
ā=ί(Θ(Η,Α++));Ȁ=ί(Θ(Η,Α++));Η=Θ(ǡ,2).Split('\n');Α=0;Ν=(Μ) int.Parse(Θ(Η,Α++));μ=(Ο) int.Parse(Θ(Η,Α++));ї=(Ο) int.Parse(Θ(Η,Α++));д=int.Parse(Θ(Η,Α++));е=int.Parse(Θ(Η,Α++));П=int.Parse(Θ(Η,Α++));ж=int.Parse(Θ(Η,Α++));ѐ=float.Parse(Θ(Η,Α++));Ǩ=bool.Parse(Θ(Η,Α++));
ѝ=bool.Parse(Θ(Η,Α++));if(Ν==Μ.ζ){ν.Д=(Ё) int.Parse(Θ(Η,Α++));ν.Ч=float.Parse(Θ(Η,Α++));ν.Ǖ=float.Parse(Θ(Η,Α++));ν.б=Ĥ(Θ(Η,Α++));ν.г=Ĥ(Θ(Η,Α++));ξ.Д=(Ё) int.Parse(Θ(Η,Α++));ξ.Ч=float.Parse(Θ(Η,Α++));ξ.Ǖ=float.Parse(Θ(Η,Α++));ξ.б=Ĥ(Θ(Η,Α++));ξ.г=Ĥ(Θ(Η,Α++));}else{ǣ=(Я) int.Parse(Θ(Η,Α++));
љ=(ь) int.Parse(Θ(Η,Α++));ȧ=(Ф) int.Parse(Θ(Η,Α++));с=(к) int.Parse(Θ(Η,Α++));Ǽ=(Я) int.Parse(Θ(Η,Α++));ď=bool.Parse(Θ(Η,Α++));Ǥ=bool.Parse(Θ(Η,Α++));Ȱ=bool.Parse(Θ(Η,Α++));Ȩ=bool.Parse(Θ(Η,Α++));К=int.Parse(Θ(Η,Α++));Й=int.Parse(Θ(Η,Α++));Ц=int.Parse(Θ(Η,Α++));ŗ=float.Parse(Θ(Η,Α++));
Ŗ=float.Parse(Θ(Η,Α++));Đ=float.Parse(Θ(Η,Α++));Ǻ=float.Parse(Θ(Η,Α++));ω=int.Parse(Θ(Η,Α++));Ў=int.Parse(Θ(Η,Α++));Ļ=int.Parse(Θ(Η,Α++));ļ=int.Parse(Θ(Η,Α++));ȳ=int.Parse(Θ(Η,Α++));а=int.Parse(Θ(Η,Α++));ƫ=int.Parse(Θ(Η,Α++));Ơ=float.Parse(Θ(Η,Α++));}Η=Θ(ǡ,3).Replace("\n","").Split('|');
ш=Η.ToList();Η=Θ(ǡ,4).Split('\n');ρ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
ρ.Add(ΓЈ(Θ(Η,i)));Η=Θ(ǡ,5).Split('|');Ш=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out Ǟ)) Ǟ=-1;if(Ш.Length!=ω*Ў){Ш="";Ǟ=-1;}ΒЅ(Θ(ǡ,6));}catch{return Ю.ǆ;}Ƭ=ї;Ǐ();return Ю.Αξ;}const int ΕΫ=64000;void ΓϏ(){Ǔ.Clear();for(int i=Ѕ.Count-1;i>=0;i--){String Ρ=Ѕ[i].CustomData.ToUpper();
bool Ɨ=false;if(Ρ=="INSTRUCTIONS"){Ɨ=true;ΑЈ=true;}if(Ρ=="DEBUG") Ɨ=true;if(Ɨ){Ǔ.Add(Ѕ[i]);Ѕ.RemoveAt(i);}}Ȗ(Ǔ,false,1,false);}void ΓЀ(List<IMyTerminalBlock>Αϋ){з.Clear();for(int i=0;i<Αϋ.Count;i++){IMyTerminalBlock Ϊ=Αϋ[i];try{String Βί=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=Ϊ.CustomName.IndexOf(Βί);int Ȏ=-1;if(Α<0||!int.TryParse(Ϊ.CustomName.Substring(Α+Βί.Length,1),out Ȏ)) continue;if(Ȏ==-1) continue;Ȏ--;IMyTextSurfaceProvider Βα=(IMyTextSurfaceProvider) Ϊ;if(Ȏ<Βα.SurfaceCount&&Ȏ>=0)
з.Add(Βα.GetSurface(Ȏ));}catch{}}}String Ƣ="";void Γμ(){String ƞ="";String ȥ="";Ƣ=Βυ(false);ƞ+=Ƣ;ȥ+=Ƣ;ȥ+=ΓΙ();for(int i=0;i<Ѕ.Count;i++) Ѕ[i].WriteText(ƞ);for(int i=0;i<з.Count;i++) з[i].WriteText(ƞ);Echo(ȥ);for(int i=0;i<Ǔ.Count;i++){IMyTextPanel Ј=Ǔ[i];String Ρ=Ј.CustomData.ToUpper();
if(Ρ=="DEBUG") Ј.WriteText(""+"\n"+"");if(Ρ=="INSTRUCTIONS") Ј.WriteText(ΑΑ());}}string ΑΑ(){String Β="";try{float ΒΥ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ǵ*ΒΥ)+" / "+(int)(ǿ*ΒΥ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ǵ*100f,1)+"% / "+Math.Round(ǿ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ȝ,2)+"ms / "+Math.Round(ơ,2)+"ms \n";}catch{}for(int i=0;i<ō.Count;i++)
Β+=""+ō.Keys.ElementAt(i)+" = "+Math.Round(ō.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ō.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΓΤ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ȗ(Ѕ,setLCDFontAndSize,0.9f,false);Ȗ(з,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ѕ.Count;i++) Ѕ[i].WriteText(Β+ǽ.ToString());for(int i=0;i<з.Count;i++) з[i].WriteText(Β+ǽ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Αθ(String π,String α,String ǖ){try{if(!ȟ) return;π=BROADCAST_TAG+BROADCAST_SEP+Ǌ()+BROADCAST_SEP+α+BROADCAST_SEP+Ƈ+BROADCAST_SEP+ǖ+BROADCAST_SEP+π;this.IGC.SendBroadcastMessage(BROADCAST_TAG,π);}catch(Exception e){ǽ=e;}}String Ǌ(){if(Ν!=Μ.Ѝ) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΑΣ(String π){return π.StartsWith(BROADCAST_TAG);}bool Ѐ(ref String Ρ,out string ȩ,Char Ǣ){int Α=Ρ.IndexOf(Ǣ);ȩ="";if(Α<0) return false;ȩ=Ρ.Substring(0,Α);Ρ=Ρ.Remove(0,Α+1);return true;}bool Γθ(ref String π,out String ћ,out String ǖ){ћ="";ǖ="";if(!ȟ) return false;
String Т="";if(!Ѐ(ref π,out Т,BROADCAST_SEP)||!ΑΣ(Т)) return false;if(!Ѐ(ref π,out ћ,BROADCAST_SEP)) return false;if(!Ѐ(ref π,out Т,BROADCAST_SEP)||(Т!=Ǌ()&&(Т!="*"&&Ν!=Μ.Ѝ))) return false;if(!Ѐ(ref π,out Т,BROADCAST_SEP)||(Т!=Ƈ)) return false;if(!Ѐ(ref π,out ǖ,BROADCAST_SEP)) return false;
return true;}void ΓΨ(){bool Ȣ=true;if(ΑА){ΑА=false;if(Me.CustomData.Contains("Antenna_Name")){Φ="Update custom data";Me.CustomData="";}}String Ȭ=(Ν!=Μ.Ѝ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ȭ)
ΑΟ(Ȭ);String[]Η=Me.CustomData.Split('\n');ȟ=bool.Parse(Ĝ(ė(Η,"Enable_Broadcast",ref Ȣ)));bool Ƽ=false;bool ΑЊ=true;if(ȟ){if(Ν!=Μ.Ѝ)
Ć=Ĝ(ė(Η,"Ship_Name",ref Ȣ)).Replace(BROADCAST_SELF_ID,"");Ƈ=Ĝ(ė(Η,"Broadcast_Channel",ref Ȣ)).ToLower();ΑЊ=false;if(п==null){п=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);п.SetMessageCallback("");}List<IMyRadioAntenna>ǳ=new List<IMyRadioAntenna>();В.GetBlocksOfType(ǳ);bool ƃ=false;
for(int i=0;i<ǳ.Count;i++)
if(ǳ[i].EnableBroadcasting&&ǳ[i].Enabled){ƃ=true;break;}if(ǳ.Count==0) Φ="No Antenna found";else if(!ƃ) Φ="Antenna not ready";Ƽ=ǳ.Count==0||!ƃ;if(Ατ&&!Ƽ&&Ν!=Μ.Ѝ) Φ="Antenna ok";}else if(Ν==Μ.Ѝ)
Φ="Offline - Enable in PB custom data";Ατ=Ƽ;if(ΑЊ){if(п!=null) this.IGC.DisableBroadcastListener(п);п=null;}}catch{Ȣ=false;}if(!Ȣ){Φ="Reset custom data";ΑΟ(Ȭ);}}void ΑΟ(String Ȭ){String Ρ=Ȭ+"\n\n"+"Enable_Broadcast="+(Ν==Μ.Ѝ?"true":"false")+" \n";Ρ+=Ν!=Μ.Ѝ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ρ+"Broadcast_Channel=#default";}String Ĝ(String Ǜ){int Α=Ǜ.IndexOf("//");if(Α!=-1) Ǜ=Ǜ.Substring(0,Α);String[]ή=Ǜ.Split('=');if(ή.Length<=1) return "";return ή[1].Trim();}String ė(String[]Η,String Ń,ref bool Ʀ){foreach(String Ǜ in Η)
if(Ǜ.StartsWith(Ń)) return Ǜ;Ʀ=false;return "";}void ΒЀ(string Љ){if(Ȫ) ό=new List<ο>();String ћ="";String ǖ="";if(!ΑΣ(Љ)) Γν(Љ);bool Βη=false;int Δτ=50;while(п!=null&&п.HasPendingMessage&&Δτ-->0){MyIGCMessage π=п.AcceptMessage();String Ρ=π.Data as String;if(Ρ==null) continue;
if(Γθ(ref Ρ,out ћ,out ǖ)&&ћ!=""&&ћ!=BROADCAST_SELF_ID){ο Γ=ţ(ћ,"");if(Γ==null){Γ=new ο(ћ);ό.Add(Γ);}Γ.ǰ(ǖ,false,false,true);ΓЌ(Γ,Ρ);Βη=true;}}if(Βη)
ό.Sort(delegate(ο a,ο b){if(a.γ==null&&b.γ==null) return 0;else if(a.γ==null) return-1;else if(b.γ==null) return 1;else return a.γ.CompareTo(b.γ);});if(ȱ||Ȫ){if(Ŏ<=0||Ȫ){Φ="";Ȫ=false;Γυ();ΓΨ();ΒΟ=DateTime.Now;}}if(ŀ) ΓЋ();ΓЊ();}void ΓЌ(ο Γ,String Ρ){Γ.ƙ=DateTime.Now;
String ȩ="";Ѐ(ref Ρ,out Γ.Ŕ,BROADCAST_SEP);Ѐ(ref Ρ,out Γ.γ,BROADCAST_SEP);if(Γ.Ŕ!=VERSION) return;String ă="";String[]ȅ=new string[3];Ѐ(ref Ρ,out ă,BROADCAST_SEP);Ѐ(ref Ρ,out ȅ[0],BROADCAST_SEP);Ѐ(ref Ρ,out ȅ[1],BROADCAST_SEP);Ѐ(ref Ρ,out ȅ[2],BROADCAST_SEP);Ѐ(ref Ρ,out Γ.ƴ,BROADCAST_SEP);
Ѐ(ref Ρ,out Γ.Φ,BROADCAST_SEP);Ѐ(ref Ρ,out ȩ,BROADCAST_SEP);Ѐ(ref Ρ,out Γ.ň,BROADCAST_SEP);String Βο="",ΒΧ="";Ѐ(ref Ρ,out Βο,BROADCAST_SEP);Ѐ(ref Ρ,out ΒΧ,BROADCAST_SEP);Γ.т.Clear();while(true){String ΒΛ;if(!Ѐ(ref Ρ,out ΒΛ,BROADCAST_SEP)) break;String[]Ł=ΒΛ.Split('/');if(Ł.Count()<3) continue;
int Є=0;if(!int.TryParse(Ł[2],out Є)) continue;Γ.т.Add(new м(Ł[0],Ł[1],Є,ę.ΓΥ));}int ΒΩ=0;if(!int.TryParse(ȩ,out Γ.ι)) Γ.ι=0;if(!int.TryParse(ă,out ΒΩ)) Γ.Ν=Μ.я;if(!float.TryParse(ȅ[0],out Γ.Σ.X)) Γ.Σ.X=0;if(!float.TryParse(ȅ[1],out Γ.Σ.Y)) Γ.Σ.Y=0;if(!float.TryParse(ȅ[2],out Γ.Σ.Z)) Γ.Σ.Z=0;
if(!float.TryParse(Βο,out Γ.Ǒ)) Γ.Ǒ=0;if(!float.TryParse(ΒΧ,out Γ.ǜ)) Γ.ǜ=0;Γ.Ν=(Μ) ΒΩ;Γ.ƍ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Σ));Γ.ƹ=0;for(int i=0;i<Γ.т.Count();i++) Γ.ƹ+=Γ.т[i].Є;}ο ţ(String ψ,String γ){ψ=ψ.ToUpper();γ=γ.ToUpper();for(int i=0;i<ό.Count;i++)
{if(ψ!=""&&ό[i].ψ.ToUpper()==ψ) return ό[i];if(γ!=""&&ό[i].γ!=null&&ό[i].γ.ToUpper()==γ) return ό[i];}return null;}void З(ο Γ,String ϋ){if(Γ==null){for(int i=0;i<ό.Count;i++)
ό[i].ǰ(ϋ,true,false,false);Αθ(ϋ,"*",ϋ);}else{Γ.ǰ(ϋ,true,false,false);Αθ(ϋ,Γ.ψ,ϋ);}}void Γν(string Љ){if(Љ=="") return;var ή=Љ.ToUpper().Split(' ');ή.DefaultIfEmpty("");var ϋ=ή.ElementAtOrDefault(0);var Αφ=ή.ElementAtOrDefault(1);String Δη="Invalid argument: "+Љ;if(ά==Ι.ȗ){if(κ!=null)
{switch(ϋ){case "UP":{if(ι<2) break;if(κ.ι==0){ι=1;return;}З(κ,"UP");return;}case "DOWN":{if(ι<1) break;if(ι==1){З(κ,"MRES");break;}З(κ,"DOWN");break;}case "APPLY":{if(ι<2) break;З(κ,"APPLY");return;}}}}switch(ϋ){case "UP":this.Γή(false);return;case "DOWN":this.Γέ(false);return;
case "APPLY":this.ǫ(true);return;}switch(ϋ){case "CLEAR":Βώ();return;case "SENDALL":
if(String.IsNullOrEmpty(Αφ)){Φ="Usage: SENDALL <cmd>";return;}З(null,Αφ);return;case "SEND":
if(Љ.Length<="SEND".Length+1){Φ="Usage: SEND <ship>:<cmd>";return;}Γχ(Љ.Remove(0,"SEND".Length+1));return;}Φ=Δη;}void Γχ(String Αυ){if(Αυ=="") return;var ή=Αυ.Split(':');if(ή.Length!=2){Φ="Missing separator \":\"";return;}ή.DefaultIfEmpty("");String Ć=ή.ElementAtOrDefault(0).Trim();
ο Γ=ţ("",Ć);if(Γ!=null) З(Γ,ή.ElementAtOrDefault(1).Trim());else Φ="Unknown ship: "+Ć;}void Βώ(){ό.Clear();κ=null;Υ(Ι.Main);ι=0;}void Γυ(){List<IMyTerminalBlock>Βθ=new List<IMyTerminalBlock>();В.GetBlocksOfType(Ѕ,Ŭ);В.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Βθ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ѕ=Ǉ(Ѕ,pamTag,true);ΓЀ(Βθ);ΓϏ();Γφ();Ȗ(Ѕ,setLCDFontAndSize,1.15f,false);Ȗ(з,setLCDFontAndSize,1.15f,true);String Δγ=ΑΒ(false);String ΒΫ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ј in Ѕ){if(Ј.CustomData==""){Ј.CustomData=ΑΒ(true);continue;}
if(!Ј.CustomData.Contains(Δγ))
if(!Ј.CustomData.Contains(ΒΫ)) Ј.CustomData=ΒΫ+Ј.CustomData;}}void ΓЊ(){String ȥ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<з.Count;i++)
з[i].WriteText(ǀ(σ.ΑΦ,"0",false));for(int i=0;i<Ѕ.Count;i++){σ ΰ=σ.я;String С="";Γι(Ѕ[i],out ΰ,out С);Ѕ[i].WriteText(ǀ(ΰ,С,i==0));}Echo(ȥ);for(int i=0;i<Ǔ.Count;i++){IMyTextPanel Ј=Ǔ[i];String Ρ=Ј.CustomData.ToUpper();if(Ρ=="DEBUG") Ј.WriteText(""+"\n"+"");if(Ρ=="INSTRUCTIONS") Ј.WriteText(ΑΑ());
}}String ǫ(bool Ε){int ΔΦ=0;return ǫ(Ε,0,ref ΔΦ,false,1);}String ǫ(bool Ε,int Δϋ,ref int ΔЇ,bool ΑЁ,int Ą){String Β="";String Ȍ="——————————————————\n";String ΕΓ="--------------------------------------------\n";if(κ==null||ά==Ι.Main||ά==Ι.ź)
Β+="[PAM]-Controller | "+ό.Count+" Connected ships"+"\n"+ΓΖ();else Β+=ΑΖ(κ)+"\n";Β+=Ȍ;int Α=0;int Δ=ι;ǝ=0;if(Φ!=""){Ζ(ref Β,Δ,Α++,Ε,Φ);}else if(ά==Ι.Main){if(κ==null&&ό.Count>=1) κ=ό[0];String Ѓ="";if(Ζ(ref Ѓ,Δ,Α++,Ε," Send command to all")) Υ(Ι.ź);int ȋ=0;
for(int s=0;s<ό.Count;s++){if(ΑЁ) Ѓ+="\n";ο Γ=ό[s];if(Δ==Α||Δ==Α+1) κ=Γ;if(Α==Δ) ȋ=Ѓ.Split('\n').Length-1;if(Ζ(ref Ѓ,Δ,Α++,Ε," "+ΑΖ(Γ))){κ=Γ;Υ(Ι.ȗ);κ.ǰ("",false,true,false);}if(Α==Δ) ȋ=Ѓ.Split('\n').Length-1;if(Ζ(ref Ѓ,Δ,Α++,Ε," "+(ΑЁ?"":"  ")+ΓΝ(Γ)))
{κ=Γ;Υ(Ι.Αι);}}int Ȑ=Ą-2;Ȑ+=Math.Max(0,(Δϋ-1))*Ą;Β+=Ź(Ȑ,Ѓ,ȋ,ref ΔЇ);}else if(ά==Ι.ĝ||ά==Ι.ź){Ι ΔΓ=ά==Ι.ĝ?Ι.ȗ:Ι.Main;ο α=null;if(ά==Ι.ĝ) α=κ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Υ(ΔΓ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǭ(α,"STOP"))){З(α,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǭ(α,"CONT"))){З(α,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǭ(α,"HOMEPOS"))){З(α,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǭ(α,"JOBPOS"))){З(α,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǭ(α,"FULL"))){З(α,"FULL");}if(α!=null&&α.Ν==Μ.ζ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǭ(α,"UNDOCK"))){З(α,"UNDOCK");}}else if(ά==Ι.ȗ){String ƅ="";if(Ζ(ref ƅ,Δ,Α++,Ε," Back")){Υ(Ι.Main);}Β+=ƅ.Substring(0,ƅ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Υ(Ι.ĝ);}if(!κ.ΑΥ()&&!κ.Ū()) ǝ++;if(κ.Ū()) Β+="No answer received...";
else Β+=Ǳ(κ,κ.ň);}else if(ά==Ι.Αι){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Υ(Ι.Main);}Β+=ΕΓ;Β+=Ǳ(κ,ΑΕ(κ));}if(!ƣ) ι=Math.Min(ǝ-1,ι);return Β;}String ΑΖ(ο Γ){if(Γ.Ŕ!=VERSION) return Γ.γ+": Different version!";return Γ.γ+": "+Ǳ(Γ,Γ.ƴ)+" "+Γ.ƍ+"m";}String ΓΝ(ο Γ){String s=Ŷ("",Ж(Γ.Ǒ,Γ.ǜ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.т.Count;i++){if(i>=5) break;м в=Γ.т[i];s+=Ας(ŝ(ΓΚ(в)),3)+" "+ǈ(в.Є)+" ";}return s;}String ΓΚ(м в){if(в.І.ToUpper()=="ORE"||в.І.ToUpper()=="INGOT"){String ΑϏ=ΓΘ(в.γ.ToUpper());if(ΑϏ!="") return ΑϏ;}return в.γ;}String Ǳ(ο Γ,String Β){if(Γ.ΑΥ()) return "No signal...("+Ŧ((int)(DateTime.Now-Γ.ƙ).TotalSeconds)+")";
return Β;}String ΑΒ(bool ΕΤ){String Β="";if(ΕΤ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Γι(IMyTerminalBlock Ј,out σ ΰ,out String С){bool Ʀ=true;String ă=Ĝ(ė(Ј.CustomData.Split('\n'),"mode",ref Ʀ)).ToUpper();String[]ή=ă.Split(':');String Ă=ή.Length>0?ή[0].Trim():"";С=ή.Length>1?ή[1].Trim():"";
ΰ=σ.я;if(Ă=="MAIN") ΰ=σ.ΑΧ;else if(Ă=="MAINX") ΰ=σ.ΑΨ;else if(Ă=="MENU") ΰ=σ.ů;else if(Ă=="INVENTORY") ΰ=σ.ũ;else if(Ă=="DEBUG") ΰ=σ.Š;if(ΰ==σ.я) С=ă;}String ǀ(σ ΰ,String С,bool ΔΨ){int Ā=15;if(ΔΨ){ΑЂ=ĭ;ĭ=0;ΑЃ=Į;Į=0;}String Β="";if(ΰ==σ.ΑΧ){int Ї=0;
Β=ǫ(false,ΑЂ,ref ΔΞ,true,Ā);if(!int.TryParse(С,out Ї)) return Β;ĭ=Math.Max(Ї,ĭ);Ї--;return Αγ(Ї,Ā,Β);}else if(ΰ==σ.ΑΨ){int Ї=0;Β=ǫ(false,ΑЃ,ref ΔΟ,false,Ā);if(!int.TryParse(С,out Ї)) return Β;Į=Math.Max(Ї,Į);Ї--;return Αγ(Ї,Ā,Β);}if(ΰ==σ.ΑΦ){return ǫ(false,0,ref ΔΠ,true,Ā);
}else if(ΰ==σ.ũ||ΰ==σ.ů||ΰ==σ.Š){ο Γ=κ;if(С!=""){Γ=ţ("",С);if(Γ==null) return "Unknown ship: "+С;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ȍ="—————————————————————\n";String Ƹ="";String Ɔ="";if(ΰ==σ.ů){Ƹ="Menu";Ɔ=Ǳ(Γ,Γ.ň);}
else if(ΰ==σ.ũ){Ƹ="Inventory";Ɔ=Ǳ(Γ,ΑΕ(Γ));}else if(ΰ==σ.Š){return Ǳ(Γ,ΓΞ(Γ));}Β+=Γ.γ+" - "+Γ.ƍ+"m | "+ŝ(""+Ƹ)+"\n"+Ȍ;Β+=Ɔ;return Β;}return "Unknown command: "+С;}string ΓΞ(ο Γ){String s="";s+=Γ.Ŕ+"\n";s+=Γ.γ+"\n";s+=Γ.Σ.X+"\n";s+=Γ.Σ.Y+"\n";s+=Γ.Σ.Z+"\n";
s+=Γ.ƴ+"\n";s+=Γ.Φ+"\n";s+=Γ.ι+"\n";s+=Γ.ň.Length+"\n";s+=Γ.Ǒ+"\n";s+=Γ.ǜ+"\n";s+=Γ.т.Count()+"\n";return s;}string ΑΕ(ο Γ){String s="";s+=Ŷ("All",Ж(Γ.Ǒ,Γ.ǜ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.т.Count();i++){м в=Γ.т[i];s+=Ŷ(в.γ,в.Є,Γ.ƹ,30,8,10)+"\n";
}return s;}ς ΓΓ(ο Γ,bool ȡ,string ϋ){ς Ψ=ς.None;if(Γ==null){for(int i=0;i<ό.Count;i++){ς ϊ=ό[i].ǰ(ϋ,false,ȡ,false);if(ϊ==ς.ŷ) Ψ=ϊ;if(ϊ==ς.Ų) return ϊ;if(ϊ==ς.ȕ) return ϊ;}return Ψ;}else return Γ.ǰ(ϋ,false,ȡ,false);}String ǭ(ο Γ,String ϋ){ς ϊ=ΓΓ(Γ,false,ϋ);if(ϊ==ς.ŷ) return "received!";
if(ϊ==ς.ȕ) return "pending...";if(ϊ==ς.Ų) return "no answer!";return "";}String Ź(int Ȑ,String Β,int ȋ,ref int ф){String[]Η=Β.Split('\n');if(ȋ>=ф+Ȑ-1) ф++;ф=Math.Min(Η.Count()-1-Ȑ,ф);if(ȋ<ф+1) ф--;ф=Math.Max(0,ф);String Ψ="";for(int i=0;i<Ȑ;i++){int ΒΡ=i+ф;
if(ΒΡ>=Η.Count()) break;Ψ+=Η[ΒΡ]+"\n";}return Ψ;}String Αγ(int Ї,int Ą,String Β){String[]Η=Β.Split('\n');int ΔА=Ї*Ą;int Ĺ=(Ї+1)*Ą;String Ψ="";for(int i=ΔА;i<Ĺ;i++){if(i>=Η.Count()) break;Ψ+=Η[i]+"\n";}return Ψ;}String ŝ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();
}String Ας(String s,int ΒΦ){if(s=="") return s;if(s.Length>ΒΦ) s=s.Substring(0,ΒΦ-1)+".";return s;}string Ŷ(String ǚ,float value,float ń,int Αχ,int ΒΝ,int Δϊ){float Ǡ=Ж(value,ń)*Αχ;String Ĩ="[";for(int i=0;i<Αχ;i++){if(i<=Ǡ) Ĩ+="|";else Ĩ+="'";}Ĩ+="]";return Ĩ+" "+Ας(ŝ(ǚ),ΒΝ).PadRight(ΒΝ)+"".PadRight(Δϊ)+ǈ(value);
}String ǈ(float Є){if(Є>=1000000) return Math.Round(Є/1000000f,Є/1000000f<100?1:0)+"M";if(Є>=1000) return Math.Round(Є/1000f,Є/1000f<100?1:0)+"K";return ""+Math.Round(Є);}String Ŧ(int ą){if(ą>=60*60) return Math.Round(ą/(60f*60f),1)+" h";if(ą>=60) return Math.Round(ą/60f,1)+" min";
return ""+ą+" s";}List<IMyCameraBlock>Εσ=new List<IMyCameraBlock>();List<long>ΖΒ=new List<long>();bool Δό=false;int ΔΙ=0;int Ες=0;DateTime Επ=DateTime.Now;String ΔΖ="";void Βύ(){Δό=false;ΔΙ=0;ΔΖ="";}const String Εε="[PAMTFC]";const float Εζ=5f;const float Εή=2f;class Αο
{public long ψ;public Vector3 δ,Ζλ;public Vector3 ΕЄ;public float Εψ;public String ΕЋ="";public int ΕЅ=-1;public bool ΖΘ;public long М=0;public int Εύ=0;public DateTime ΖΫ;}IMyBroadcastListener Ζΰ=null;long Δυ=0;List<Αο>Ζά=new List<Αο>();Vector3 Ζβ=new Vector3();float Ζγ=-1;Vector3 Ζέ=new Vector3();
bool Ζί=false;DateTime Ζή=DateTime.Now;const float Εδ=15f;const float Εγ=10f;int Ζδ=0;String Ш="";int Ǟ=-1;bool ΔΡ=false;Vector3 Ευ=new Vector3();Vector3 Ζα=new Vector3();List<int>ȭ=new List<int>();String ΒΗ="";void ΒЋ(){String Ń=ω+"|"+Ў+"|"+(int) Ǽ;if(Ń==ΒΗ) return;
ΒΗ=Ń;ȭ.Clear();int n=ω*Ў;if(Ǽ==Я.Ģ){for(int k=0;k<n;k++){int Ȉ=k/ω;ȭ.Add(Ȉ*ω+(Ȉ%2==0?k-Ȉ*ω:ω-1-(k-Ȉ*ω)));}return;}int ΒΖ=(int) Math.Ceiling(ω/2f),ΒΕ=(int) Math.Ceiling(Ў/2f);int Δα=ω/2,Δΰ=Ў/2;int x=0,y=0,Ǝ=0,Ə=-1,ń=Math.Max(ω,Ў);
for(int i=0;i<ń*ń;i++){if(-ΒΖ<x&&x<=Δα&&-ΒΕ<y&&y<=Δΰ) ȭ.Add((y-1+ΒΕ)*ω+(x-1+ΒΖ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ǝ;Ǝ=-Ə;Ə=t;}x+=Ǝ;y+=Ə;}}bool ΒЊ(){int n=Math.Max(0,ω*Ў);if(Ш.Length==n) return false;Ш=new String('0',n);
Ǟ=-1;return true;}void Γύ(int Ɖ,bool ĸ){if(Ɖ<0||Ɖ>=Ш.Length) return;char[]h=Ш.ToCharArray();h[Ɖ]=ĸ?'1':'0';Ш=new String(h);}class ǯ{public long ψ;public Vector3 δ,Б,Ϗ;}List<ǯ>Њ=new List<ǯ>();bool Ȥ=false;Vector3 ĵ,Ĵ,Ķ,ķ;long Ȯ=0;long Βή=0;Χ Βέ=null;bool ΔΤ=false;
Χ ΒЇ(Vector3 δ,Vector3 Б,Vector3 Ϗ){MatrixD m=MatrixD.CreateWorld(δ,Б,Ϗ);Χ w=new Χ(Ξ);w.Ђ=Vector3D.TransformNormal(ĵ,m);w.ύ=Vector3D.TransformNormal(Ĵ,m);w.Г=Vector3D.TransformNormal(Ķ,m);w.Σ=δ+(Vector3) Vector3D.TransformNormal(ķ,m);w.β=true;return w;}void Γά(){if(Ν==Μ.ζ||!Ξ.β) return;
IMyShipConnector р=Ǭ(MyShipConnectorStatus.Connected);if(р==null||р.OtherConnector==null) return;IMyShipConnector Џ=р.OtherConnector;Vector3I Δδ=new Vector3I((int) Ξ.Л.X,(int) Ξ.Л.Y,(int) Ξ.Л.Z);if(р.Position==Δδ){MatrixD Ŀ=MatrixD.Transpose(Џ.WorldMatrix);ĵ=Vector3D.TransformNormal(Ћ.WorldMatrix.Forward,Ŀ);
Ĵ=Vector3D.TransformNormal(Ћ.WorldMatrix.Down,Ŀ);Ķ=Vector3D.TransformNormal(Ћ.WorldMatrix.Left,Ŀ);ķ=Vector3D.TransformNormal(р.GetPosition()-Џ.GetPosition(),Ŀ);Ȥ=true;}if(Vector3.Distance(р.GetPosition(),Ξ.Σ)<5) Ȯ=Џ.EntityId;List<IMyShipConnector>ǘ=new List<IMyShipConnector>();В.GetBlocksOfType(ǘ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ǘ.Count>0){Њ.Clear();for(int i=0;i<ǘ.Count;i++){ǯ p=new ǯ();p.ψ=ǘ[i].EntityId;p.δ=ǘ[i].GetPosition();p.Б=ǘ[i].WorldMatrix.Forward;p.Ϗ=ǘ[i].WorldMatrix.Up;Њ.Add(p);}}Βέ=null;Βή=0;if(Џ.EntityId==Ȯ||!Ȥ) return;for(int i=0;i<Њ.Count;i++)
if(Њ[i].ψ==Џ.EntityId){Βή=Њ[i].ψ;Βέ=ΒЇ(Њ[i].δ,Њ[i].Б,Њ[i].Ϗ);}}String ΒІ(){String s=(Ȥ?"1":"0")+":"+Ϋ(ĵ)+":"+Ϋ(Ĵ)+":"+
Ϋ(Ķ)+":"+Ϋ(ķ)+":"+Ȯ;for(int i=0;i<Њ.Count;i++)
s+=":"+Њ[i].ψ+","+Ϋ(Њ[i].δ)+","+Ϋ(Њ[i].Б)+","+Ϋ(Њ[i].Ϗ);return s;}void ΒЅ(String s){Њ.Clear();Ȥ=false;Ȯ=0;String[]f=s.Split(':');if(f.Length<6) return;Ȥ=f[0]=="1";ĵ=ί(f[1]);Ĵ=ί(f[2]);Ķ=ί(f[3]);ķ=ί(f[4]);long.TryParse(f[5],out Ȯ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǯ p=new ǯ();if(!long.TryParse(e[0],out p.ψ)) continue;p.δ=ί(e[1]);p.Б=ί(e[2]);p.Ϗ=ί(e[3]);Њ.Add(p);}}const String ş="[PAMDCK]";const float Βψ=120f;class л{public long Γ;public int і;public long Щ;public bool Αό;public DateTime Ǘ,ΒΞ;
}IMyBroadcastListener ĳ=null;List<л>τ=new List<л>();List<IMyShipConnector>х=new List<IMyShipConnector>();List<IMySensorBlock>й=new List<IMySensorBlock>();List<IMyCameraBlock>Ȳ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ř=new List<MyDetectedEntityInfo>();long Х=0;int Ɗ=0;
long Αύ=0;DateTime Αώ=DateTime.Now;String ǔ="";int ΑЍ=0;IMyCubeGrid ц=null;void Γφ(){ц=Me.CubeGrid;В.GetBlocksOfType(х,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(dockPoolTag));В.GetBlocksOfType(й,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(zoneTag));
В.GetBlocksOfType(Ȳ,q=>q.CubeGrid.IsSameConstructAs(ц)&&q.CustomName.Contains(laneTag));for(int i=0;i<й.Count;i++){й[i].DetectSmallShips=true;й[i].DetectLargeShips=true;}for(int i=0;i<Ȳ.Count;i++) Ȳ[i].EnableRaycast=true;}IMyShipConnector Αν(long ψ){if(ψ==0) return null;
IMyShipConnector c=В.GetBlockWithId(ψ) as IMyShipConnector;return c!=null&&ц!=null&&c.CubeGrid.IsSameConstructAs(ц)?c:null;}bool ū(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΑΤ(IMyShipConnector c,long Γ){for(int i=0;i<Ȳ.Count;i++)
{IMyCameraBlock Ƞ=Ȳ[i];if(!Ƞ.IsWorking||Vector3D.Distance(Ƞ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ĺ=Ƞ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ƞ.CanScan(Ĺ)) continue;MyDetectedEntityInfo ĺ=Ƞ.Raycast(Ĺ);if(!ĺ.IsEmpty()&&ĺ.EntityId!=ц.EntityId&&ĺ.EntityId!=Γ&&ĺ.Type!=MyDetectedEntityType.FloatingObject)
{ǔ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΓΫ(){for(int s=0;s<й.Count;s++){if(!й[s].IsWorking) continue;Ř.Clear();й[s].DetectedEntities(Ř);for(int i=0;i<Ř.Count;i++){MyDetectedEntityInfo e=Ř[i];bool Δέ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Δέ&&e.EntityId!=Х&&e.Velocity.Length()>0.5f){ǔ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Γλ(л r){IMyShipConnector Щ=Αν(r.Щ);if(ū(Щ)&&ΑΤ(Щ,r.Γ)) return Щ.EntityId;if(!r.Αό||Щ==null) return 0;IMyShipConnector ĩ=null;double Αϊ=0;for(int i=0;i<х.Count;i++)
{IMyShipConnector c=х[i];if(!ū(c)) continue;double d=Vector3D.Distance(c.GetPosition(),Щ.GetPosition());if(ĩ==null||d<Αϊ){if(ΑΤ(c,r.Γ)){ĩ=c;Αϊ=d;}}}return ĩ==null?0:ĩ.EntityId;}void ΓΡ(){л ȃ=null;long р=0;for(int Ō=2;Ō>=1&&ȃ==null;Ō--){л ǟ=null;for(int i=0;i<τ.Count;i++)
{л r=τ[i];if(r.і!=Ō) continue;if(Ō==1){long c=Γλ(r);if(c==0) continue;if(ǟ==null||r.Ǘ<ǟ.Ǘ){ǟ=r;р=c;}}else if(ǟ==null||r.Ǘ<ǟ.Ǘ) ǟ=r;}ȃ=ǟ;}if(ȃ==null){if(τ.Count>0&&ǔ=="") ǔ="No free dock";return;}Х=ȃ.Γ;Ɗ=ȃ.і;Αύ=ȃ.і==1?р:0;Αώ=DateTime.Now;
}void ΓЋ(){if(ц==null) return;if(ĳ==null) ĳ=IGC.RegisterBroadcastListener(ş);int ΔΗ=30;while(ĳ.HasPendingMessage&&ΔΗ-->0){String Ρ=ĳ.AcceptMessage().Data as String;if(Ρ!=null) ΓΣ(Ρ);}Γσ();if(++ΑЍ<3) return;ΑЍ=0;IGC.SendBroadcastMessage(ş,Βχ());}void ΓΣ(String Ρ){try{String[]f=Ρ.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=τ.Count-1;i>=0;i--) if(τ[i].Γ==Γ) τ.RemoveAt(i);if(Х==Γ) Х=0;}else if(f[0]=="R"&&f.Length>=5){long Щ=long.Parse(f[3]);if(Αν(Щ)==null) return;л r=null;for(int i=0;i<τ.Count;i++) if(τ[i].Γ==Γ) r=τ[i];if(r==null){r=new л();r.Γ=Γ;r.Ǘ=DateTime.Now;τ.Add(r);}
int і=int.Parse(f[2]);if(r.і!=і) r.Ǘ=DateTime.Now;r.і=і;r.Щ=Щ;r.Αό=f[4]=="1";r.ΒΞ=DateTime.Now;}}catch{}}void Γσ(){for(int i=τ.Count-1;i>=0;i--)
if((DateTime.Now-τ[i].ΒΞ).TotalSeconds>3) τ.RemoveAt(i);л ī=null;for(int i=0;i<τ.Count;i++) if(τ[i].Γ==Х) ī=τ[i];if(Х!=0&&ī==null) Х=0;if(ī!=null&&(DateTime.Now-Αώ).TotalSeconds>Βψ){Х=0;ī.Ǘ=DateTime.Now;}ǔ="";if(Х==0&&τ.Count>0&&!ΓΫ()) ΓΡ();}String Βχ()
{String π="S;"+ц.EntityId+";"+Х+";"+Ɗ+";"+Αύ+";";for(int i=0;i<τ.Count;i++) π+=(i>0?",":"")+τ[i].Γ;π+=";";for(int i=0;i<х.Count;i++){IMyShipConnector c=х[i];π+=(i>0?":":"")+c.EntityId+","+Ϋ(c.GetPosition())+","+Ϋ(c.WorldMatrix.Forward)+","+Ϋ(c.WorldMatrix.Up);
}π+=";";for(int i=0;i<й.Count;i++){IMySensorBlock z=й[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();π+=(i>0?":":"")+Ϋ(z.GetPosition())+","+Math.Round(r,1);}return π;}String ΓΖ()
{if(х.Count==0&&й.Count==0) return "";int ΑБ=0;for(int i=0;i<х.Count;i++) if(ū(х[i])) ΑБ++;return "Docks: "+ΑБ+"/"+х.Count+" free"+(Х!=0?(Ɗ==1?" | docking":" | undocking"):"")+
(τ.Count>1?" | "+(τ.Count-1)+" waiting":"")+(ǔ!=""?"\n"+ǔ:"")+"\n";}IMyBroadcastListener ΖΠ=null;int ΑЌ=0;DateTime ΕϏ=DateTime.Now;DateTime ΖΜ=DateTime.MinValue;long ΖΝ=0;int ΖΞ=0;long ΖΟ=0;bool ΕΝ=false;int ΕЀ=0;List<Vector3>Ζξ=new List<Vector3>();List<float>Ζο=new List<float>();
bool Εώ=false;bool Εό=false;Vector3 Εϋ=new Vector3();void Γξ(){if(ΑЌ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ş,"D;"+Δυ);ΑЌ=0;ΕΝ=false;}void ΓЁ(string Љ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}