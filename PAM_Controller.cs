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
public enum Ν{ќ,Ə,Ǿ,А,λ}public enum Λ{ǚ,Ƃ,ΑІ,Ɣ,ΑБ,ȩ,њ,
Ɓ,ΓЂ,Ǻ,Αδ,Γά,Αέ,
Δι,Ȱ,ȯ,Ż,ΔЎ,Γύ,
ΔЌ,ΔЍ,Γώ,ΓϏ,ȱ}public enum Σ{щ,Ƒ,Ǚ,Αε,Į,ΑЇ,ΑЈ,Ī,ī}public enum і{None,Ż,ǚ,Γσ}public enum Ι{Main,ΑЏ,ȫ,Ȫ,ƛ,ĭ,
ŷ,Αω,Αϊ,Αχ,Ȯ,ΒΓ,
ķ,Ɩ,ƙ,ΒΖ,Ļ}public enum ћ{ΒΑ,Γο,Δϊ,ΔΝ,λ}public enum О{Ľ,Γκ}public enum љ{ΑА,ΓЁ,Δό,Ɔ}public enum Ь{ΑЊ,Γς,Γρ,Γω,Γψ,ΔΛ,ΔΚ}public enum х{ž,Γΰ,Αή}public enum Ĳ{ΔΙ,Ζα,Γέ}public enum Ѓ{ΑЋ,ΑЌ,ΑЍ,Δί,Δή,
ΔΩ,ΔΪ,ΔΨ,Δά,
Δέ,ΔΫ}public enum Τ{ж,ъ,е,Ĭ,Ÿ,Ź}public enum Ζγ{Ζΰ,ΖΫ,Ζή}public enum з{ΒΙ,Αζ,ΔΧ,ΑЉ}public enum ζ{None,ǿ,Ȭ,ĵ}public enum π{ƞ,Ŷ,ƌ,Ɔ,ΑЄ,Ǝ,ΑЅ,ΔЅ,Ɛ}public enum φ{ќ,ΑЂ,ΑЁ,ƍ,Ƈ,Ž,ΑЃ}class Ω{public bool ι=false;public Vector3 Υ=new Vector3();
public Vector3 Ϗ=new Vector3();public Vector3 Є=new Vector3();public Vector3 Й=new Vector3();public Vector3 μ=new Vector3();public Vector3 Т=new Vector3();public float Επ=0;public float Ημ=0;public float[]Ъ=null;public Ω(){}public Ω(Ω Г){ι=Г.ι;Υ=Г.Υ;Ϗ=Г.Ϗ;Є=Г.Є;Й=Г.Й;μ=Г.μ;
Т=Г.Т;Ъ=Г.Ъ;}public Ω(Vector3 Υ,Vector3 Є,Vector3 Ϗ,Vector3 Й,Vector3 μ){this.Υ=Υ;this.Ϗ=Ϗ;this.Є=Є;this.Й=Й;this.Επ=0;this.μ=μ;}public void Ζέ(List<IMyThrust>Ł,List<string>ѕ){Ъ=new float[ѕ.Count];for(int i=0;i<Ъ.Length;i++)
Ъ[i]=-1;for(int i=0;i<Ł.Count;i++){string Ƕ=ΔΒ(Ł[i]);int Α=ѕ.IndexOf(Ƕ);if(Α!=-1)
Ъ[Α]=М(Ł[i].MaxEffectiveThrust,Ł[i].MaxThrust);}}}class Δβ{public Ω ί=null;public List<Vector3>ΗΫ=new List<Vector3>();public float ΖϏ=0;public float Ηπ=0;public float Ζλ=0;public float ΖЈ=0;public Vector3 ΖЉ=new Vector3();}class ľ{public Ѓ К=Ѓ.ΑЋ;public float Ю=0;public float Ǥ=0;
public string л="";public string н="";DateTime ΒЁ;public bool Εώ=false;private bool Ʃ=false;public bool Γλ(bool Ǌ){if(Ǌ){Ǥ=0;Ʃ=false;return false;}Ʃ=true;return Ǥ>Ю;}public void Ζβ(){Γλ(true);Εώ=false;}public void Ζη(){if(Ʃ)
if((DateTime.Now-ΒЁ).TotalSeconds>1){Ǥ++;ΒЁ=DateTime.Now;}}public bool Αυ(){switch(К){case Ѓ.ΑЌ:return true;case Ѓ.ΑЍ:return true;}return false;}}class ч{public String θ="";public String Њ="";public int І=0;public Ĳ ƽ=Ĳ.Γέ;public ч(String θ,String Њ,int І,Ĳ ƽ){this.θ=θ;this.Њ=Њ;
this.І=І;this.ƽ=ƽ;}}class Γτ{public Γτ(Vector3 β,float В){this.β=β;this.В=В;}public Vector3 β;public float В;}class ε{public DateTime ƺ;public DateTime Βί;public String Ο="";public String θ="";public String ű="";public String Ψ="";public String ΑΝ="";public String Ť="";public String ǈ="";
public Vector3 Υ=new Vector3();public List<ч>я=new List<ч>();public Ν Π=Ν.ќ;public ζ ў;public float ǫ;public float Ǡ;public int ξ;public int Ƭ=0;public int ΑΤ=0;public bool ΑЀ(){return(DateTime.Now-ƺ).TotalSeconds>10;}public bool ƈ(){if(ў!=ζ.ǿ) return false;return(DateTime.Now-Βί).TotalSeconds>=4;
}public ε(String Ο){this.Ο=Ο;}public ζ Ǜ(String ρ,bool ΕЊ,bool ь,bool ΕΖ){if(ρ==""&&!ь) return ζ.None;if(ў==ζ.ǿ&&ƈ())
ў=ζ.ĵ;if(ΕЊ){ǈ=ρ;ў=ζ.ǿ;Βί=DateTime.Now;return ў;}else if(ь){ǈ="";ў=ζ.None;}else if(ǈ==ρ){if(ΕΖ) ў=ζ.Ȭ;return ў;}return ζ.None;}}public String ΓЊ(String Εθ){switch(Εθ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem И;Vector3 ΓΔ=new Vector3();Vector3 д=new Vector3();Vector3 ΕΩ=new Vector3();Vector3 Ελ=new Vector3();Vector3 ΕΡ=new Vector3();DateTime ΒЀ=new DateTime();
bool Č=true;int ΑΠ=0;bool Ĕ=false;bool ȍ=false;bool Ετ=true;bool Ης=false;bool ΖЍ=false;bool Εδ=false;float ΕЌ=0;float ΓΛ=0;int ΕЄ=0;int Ην=0;int Ū=0;int Ζρ=0;float ȑ=0;float Ȅ=0;double ǂ=0;float ȳ=0;List<int>Ȍ=new List<int>();List<int>Ȟ=new List<int>();Ν Π=Ν.ќ;
int ΑΔ=0;float ǵ=0;float Ǵ=0;float Ȣ=0;float ΓΚ=0,ΓΙ=0;IMyRemoteControl Ј;IMySensorBlock ϊ;List<IMyTimerBlock>ΓΤ=new List<IMyTimerBlock>();List<IMyShipConnector>ą=new List<IMyShipConnector>();List<IMyThrust>Ģ=new List<IMyThrust>();List<IMyGyro>Βυ=new List<IMyGyro>();List<IMyTerminalBlock>Ы=new List<IMyTerminalBlock>();
List<IMyLandingGear>ƹ=new List<IMyLandingGear>();List<IMyReactor>ǉ=new List<IMyReactor>();List<IMyConveyorSorter>ΕЏ=new List<IMyConveyorSorter>();List<IMyGasTank>đ=new List<IMyGasTank>();List<IMyTerminalBlock>Ȳ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ζτ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ζκ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΒΥ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ї=new List<IMyTextPanel>();List<IMyTextSurface>т=new List<IMyTextSurface>();List<IMyTextPanel>Ǣ=new List<IMyTextPanel>();IMyTerminalBlock ǔ=null;Ω Ρ=new Ω();Ω Βω=new Ω();
Ω Βύ=new Ω();bool Ǘ=false;bool Ħ=false;bool ħ=false;bool Βϋ=false;List<String>ѕ=new List<string>();List<Ω>υ=new List<Ω>();List<Ω>Ų=new List<Ω>();int ΖЀ=0;double ΖΡ=0;Ω Φ=new Ω();Vector3 ę;Vector3 Ȓ;Σ ς=Σ.щ;О Ȏ=О.Ľ;int в=0;int б=0;double Ş=0;bool Βε=false;ľ σ=new ľ();
ľ τ=new ľ();љ Ǔ=љ.ΑА;bool Ĩ=true;О ǲ=О.Ľ;int С=3;int Р=3;int Э=30;х ю=х.ž;bool Ǹ=true;bool ē=true;bool Ċ=true;bool ǳ=false;Ь ĉ=Ь.ΑЊ;bool ǖ=true;float о=90;float п=20;float Ц=5;float р=20;float ѝ=0.70f;float Ŵ=1.50f;float ų=2.50f;float ĩ=0f;float ď=0f;Λ Ō;
Σ Ǒ;Λ Ġ;Δβ ΗΘ=null;int Ř=0;int ř=0;int к=0;int ė=0;int ΑΑ=30;int ΗΚ=0;bool ΕА=true;Vector3 ΗΔ;double Βκ=0;bool ΓΖ=false;int ΖΣ=0;int ΗΦ=0;int Ζμ=0;int ΗΗ=0;int ΗΕ=0;Vector3 ΗΙ=new Vector3();float ΗΑ=0;float ΗΧ=0;int Ƿ=0;Ω ū=null;float ΗΒ=0;float Ζν=0;float Βδ=0;
float ǁ=0;bool ΖЎ=false;bool Ηλ=false;bool Ηο=false;bool ΖЋ=false;bool Ζο=false;DateTime ΖЇ=new DateTime();Ω Ζύ=null;Ω ΕΟ=null;float ΒΦ=0;і Ā;float Βϊ=0;float ΓΧ=0;String Εξ="";float Ƽ=0;float Ǡ=0;float ǫ=0;List<ч>ΖЏ=new List<ч>();float Ѐ=0;bool ΑΗ=false;bool ΖΕ=false;
bool ΕЍ=false;bool ΓΣ=false;bool ΕЎ=false;float Εο=0;float Εό=0;Vector3 ΕΧ=new Vector3();Vector3 ΒЌ=new Vector3();Vector3 Ηα=new Vector3(1,1,1);float Ηί=1;Vector3 Ηΰ=new Vector3();bool ΖЌ=false;bool Εέ=false;bool ΗΓ=false;bool Ζι=false;float Ζό=0;float Ζθ=2;Vector3 ΗΪ;
Vector3 ΗΩ;Vector3 ΗΨ;float[,]ĥ=new float[3,2];Dictionary<String,float[,]>є=new Dictionary<string,float[,]>();Dictionary<String,float>Ηρ=new Dictionary<String,float>();int Ζξ=0;Dictionary<String,float[]>ũ=new Dictionary<string,float[]>();float ΗΛ;bool Βη=false;int ł=0;int Ǎ=0;
int ś=0;int ǧ=0;String Ψ="";int[]ΒЉ=new int[Enum.GetValues(typeof(Ι)).Length];bool Ǆ=false;int Ǭ=0;int ξ=0;Ι ή=Ι.Main;int ΖΖ=0,ΖΗ=0,ΖΘ=0,ΖΙ=0;int ΕЅ=0,ΕІ=0;bool Ă=false;bool ΒΠ=false;String Ğ="";String Ƥ="";IMyBroadcastListener ы=null;bool Βπ=true;List<ε>ό=null;
ε ο=null;int ň=0,ŉ=0;int Βα=0,Ββ=0;int ΕΜ=0,ΕΝ=0,ΕΞ=0;String Ει="";Exception ȏ=null;bool ΓΕ=false;Σ ΑΒ=Σ.щ;bool Βζ=false;bool ΖБ=true;bool ΗΝ=false;String ΖΟ="";bool ΓΦ=false;bool ΑΛ=true;int Ʒ=0;int[]ΗΟ=null;Vector3 Ʀ=new Vector3();bool Βσ=false;bool Ɗ(IMyTerminalBlock ά)=>ά.CubeGrid==Me.CubeGrid;
Program(){И=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Αφ(Me,controllerTag,true))
Π=Ν.А;Χ(Ι.Main);if(Π!=Ν.А){Ψ="Welcome to [PAM]!";з ǋ=Δη();if(Π==Ν.ќ){List<IMyShipDrill>Βξ=new List<IMyShipDrill>();List<IMyShipGrinder>Βτ=new List<IMyShipGrinder>();И.GetBlocksOfType(Βξ,q=>q.CubeGrid==Me.CubeGrid);И.GetBlocksOfType(Βτ,q=>q.CubeGrid==Me.CubeGrid);if(Βξ.Count>0)
{Π=Ν.Ə;Ψ="Miner mode enabled!";}else if(Βτ.Count>0){Π=Ν.Ǿ;Ψ="Grinder mode enabled!";}else{Π=Ν.λ;ΒΕ(ћ.λ);}}if(ǋ==з.ΒΙ) Ετ=false;if(ǋ==з.Αζ) Ψ="Data restore failed!";if(ǋ==з.ΑЉ) Ψ="New version, wipe data";}}void Main(string Ѝ,UpdateType ΖΜ){try{if(ȏ!=null){if(Ѝ.Trim().ToUpper()=="RETRY")
{ȏ=null;Č=true;ΕЄ=0;if(Π!=Ν.А) ǝ();Ψ="Restarted after error";}else{ΔΖ();Δδ();return;}}ȍ=(ΖΜ&UpdateType.Update10)!=0;if(ȍ) ΑΠ++;Ĕ=ΑΠ>=10;if(Ĕ) ΑΠ=0;if(ȍ){ł++;if(ł>4) ł=0;Ū=(int) Math.Max(0,10-(DateTime.Now-ΒЀ).TotalSeconds);}if(Ѝ!="") Ει=Ѝ;if(Π!=Ν.А)
Δω(Ѝ);else
Γπ(Ѝ);Ǆ=false;try{int Βγ=Runtime.CurrentInstructionCount;float ǯ=М(Βγ,Runtime.MaxInstructionCount);if(ǯ>0.90) Ψ="Max. instructions >90%";if(ǯ>ȑ) ȑ=ǯ;if(Βη){Ȍ.Add(Βγ);while(Ȍ.Count>10) Ȍ.RemoveAt(0);Ȅ=0;for(int i=0;i<Ȍ.Count;i++) Ȅ+=Ȍ[i];Ȅ=М(М(Ȅ,Ȍ.Count),Runtime.MaxInstructionCount);
double ƻ=Runtime.LastRunTimeMs;if(Εδ&&ƻ>ǂ) ǂ=ƻ;Ȟ.Add((int)(ƻ*1000f));while(Ȟ.Count>10) Ȟ.RemoveAt(0);ȳ=0;for(int i=0;i<Ȟ.Count;i++) ȳ+=Ȟ[i];ȳ=М(ȳ,Ȟ.Count)/1000f;}}catch{ȑ=0;}}catch(Exception e){ȏ=e;}}String ΓЋ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Π!=Ν.λ)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Χ(Ι Ћ){ΒЉ[(int) ή]=ξ;ξ=ΒЉ[(int) Ћ];if(Ћ==Ι.Ȯ) ξ=0;ή=Ћ;if(Π!=Ν.А)
ŀ(ή==Ι.ȫ,false,0,0);Ǆ=true;}void ΔΥ(bool ƾ){if(ξ>0) ξ--;else if(ƾ) ξ=Ǭ-1;}void ΔΤ(bool ƾ){if(ξ<Ǭ-1) ξ++;else if(ƾ) ξ=0;}String ΒΕ(ћ γ){switch(γ){case ћ.ΒΑ:Ψ="Job is running";break;case ћ.Γο:Ψ="Connector not ready!";break;case ћ.Δϊ:Ψ="Ship modified, path outdated!";break;
case ћ.ΔΝ:Ψ="Interrupted by player!";break;case ћ.λ:Ψ="Shuttle mode enabled!";break;}return "";}String ΔΑ(О β){switch(β){case О.Ľ:return "Top-Left";case О.Γκ:return "Center";default:return "";}}String ΓЇ(х η){switch(η){case х.Γΰ:return "Auto"+(Π==Ν.Ə?" (Ore)":"");case х.Αή:return "Auto (+Stone)";
case х.ž:return "Default";default:return "";}}String ΓЏ(Σ ώ){switch(ώ){case Σ.щ:return "No job";case Σ.Ƒ:return "Job paused";case Σ.Ǚ:return "Job active";case Σ.Ī:return "Job active";case Σ.ī:return "Job active";case Σ.Αε:return "Job done";case Σ.Į:return "Job changed";case Σ.ΑЇ:return "Move home";
case Σ.ΑЈ:return "Move to job";}return "";}String Αλ(љ ΕΔ){switch(ΕΔ){case љ.ΑА:return "Return home";case љ.ΓЁ:return "Fly to job pos";case љ.Δό:return "Stop";case љ.Ɔ:return "Ignore";}return "";}String ΓЉ(Ь η){switch(η){case Ь.ΑЊ:return "Off";case Ь.Γω:return "Drop pos (Stone) ";case Ь.Γψ:return "Drop pos (Sto.+Ice)";
case Ь.Γς:return "Cur. pos (Stone)";case Ь.Γρ:return "Cur. pos (Sto.+Ice)";case Ь.ΔΛ:return "In motion (Stone)";case Ь.ΔΚ:return "In motion (Sto.+Ice)";}return "";}String Αι(і ώ){switch(ώ){case і.None:return "No batteries";case і.Ż:return "Charging";case і.Γσ:return "Discharging";}return "";}
String Αο(Λ ώ){String ΖА=Π==Ν.λ?"target":"job";switch(ώ){case Λ.ǚ:return "Idle";case Λ.Ƃ:return "Flying to XY position";case Λ.ΑІ:return Π==Ν.Ǿ?"Grinding":"Mining";case Λ.Ɣ:return "Returning";case Λ.Γώ:return "Flying to drop pos";case Λ.ΑБ:return "Returning to dock";case Λ.ȩ:return "Flying to dock area";
case Λ.њ:return "Flying to job area";case Λ.Ɓ:return "Flying to path";case Λ.ΓЂ:return "Flying to job position";case Λ.Ǻ:return "Approaching dock";case Λ.Αδ:return "Docking";case Λ.Γά:return "Aligning";case Λ.Αέ:return "Aligning";case Λ.Δι:return "Retry docking";case Λ.Ȱ:return "Unloading";case Λ.ȱ:return ΖΟ;
case Λ.ȯ:return "Undocking";case Λ.Ż:return "Charging batteries";case Λ.ΔЎ:return "Waiting for uranium";case Λ.Γύ:return "Filling up hydrogen";case Λ.ΔЌ:return "Waiting for ejection";case Λ.ΔЍ:return "Waiting for ejection";case Λ.ΓϏ:return "Flying to drop pos";}return "";}String Ατ(Ѓ К){switch(К)
{case Ѓ.ΑЋ:return "On \"Undock\" command";case Ѓ.ΑЌ:return "On player entered cockpit";case Ѓ.Δί:return "On ship is full";case Ѓ.Δή:return "On ship is empty";case Ѓ.ΑЍ:return "On time delay";case Ѓ.ΔΪ:return "On batteries empty(<25%)";case Ѓ.ΔΨ:return "On batteries empty(=0%)";case Ѓ.ΔΩ:return "On batteries full";
case Ѓ.Δέ:return "On hydrogen empty(<25%)";case Ѓ.ΔΫ:return "On hydrogen empty(=0%)";case Ѓ.Δά:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int ΓΘ,int џ,bool Ε,String Ǩ){Ǭ+=1;if(ΓΘ==џ) Ǩ=">"+Ǩ+(ł>=2?" .":"");else Ǩ=" "+Ǩ;Β+=Ǩ+"\n";return ΓΘ==џ&&Ε;}String Γβ(bool Ε)
{int Α=0;int Δ=ξ;Ǭ=0;String ȡ="———————————————\n";String Ȥ="--------------------------------------------\n";String Β="";Β+=ΓЏ(ς)+" | "+(Ρ.ι?"Ready to dock":"No dock")+"\n";Β+=ȡ;double ΓΟ=Math.Max(Math.Round(this.Βκ),0);if(ή==Ι.Main){bool ǎ=Π==Ν.λ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) Δϋ();
if(Π==Ν.Ə)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Χ(Ι.ȫ);if(Π==Ν.Ǿ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Χ(Ι.ȫ);if(Π==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Χ(Ι.ƙ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) Αβ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΓЀ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΓЃ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ǎ) Χ(Ι.Ļ);else Χ(Ι.ĭ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Χ(Ι.Αω);if(Π!=Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Χ(Ι.Αχ);}else if(ή==Ι.ȫ){double ΖΤ=Math.Round(С*ǵ,1);double Εΰ=Math.Round(Р*Ǵ,1);String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Start new job!")) ΒΗ();if(Ζ(ref κ,Δ,Α++,Ε," Change current job")){Ɲ(false,false);Χ(Ι.Main);}if(Ζ(ref κ,Δ,Α++,Ε," Width + (Width: "+С+" = "+ΖΤ+"m)")){ǹ(ref С,5,20,1);Ȃ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Width -")){ǹ(ref С,-5,20,-1);Ȃ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height + (Height: "+Р+" = "+Εΰ+"m)")){ǹ(ref Р,5,20,1);Ȃ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height -")){ǹ(ref Р,-5,20,-1);Ȃ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth + ("+(ю==х.ž?"Depth":"Min")+": "+Э+"m)")){ǹ(ref Э,5,50,2);Ȃ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Depth -")){ǹ(ref Э,-5,50,-2);Ȃ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth mode: "+ΓЇ(ю))){ю=ј(ю);}if(Ζ(ref κ,Δ,Α++,Ε," Start pos: "+ΔΑ(ǲ))){ǲ=ј(ǲ);}if(Π==Ν.Ǿ&&ю==х.Αή) ю=ј(ю);Β+=ƕ(8,κ,Δ,ref ΕЅ);}else if(ή==Ι.ƙ){float[]Βθ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.ΒΖ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Ατ(σ.К))) σ.К=ј(σ.К);if(!σ.Αυ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƃ((int) σ.Ю))) σ.Ю=Αγ(σ.Ю,Βθ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Ατ(τ.К))) τ.К=ј(τ.К);
if(!τ.Αυ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƃ((int) τ.Ю))) τ.Ю=Αγ(τ.Ю,Βθ);}else if(ή==Ι.ΒΖ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ΒΗ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.ƙ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.л!=""?σ.л:"-"))) σ.л=Ĵ(ref ΖΖ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.н!=""?σ.н:"-"))) σ.н=Ĵ(ref ΖΘ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.л!=""?τ.л:"-"))) τ.л=Ĵ(ref ΖΗ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.н!=""?τ.н:"-"))) τ.н=Ĵ(ref ΖΙ);
}else if(ή==Ι.Ȫ){String ΒЄ=о+" %";if(Ĕ) Ǎ++;if(Ǎ>1){Ǎ=0;ś++;if(ś>1) ś=0;}bool[]ΑΘ=new bool[]{ǉ.Count==0,Ā==і.None,đ.Count==0};int ΓΥ=0;while(true){ΓΥ++;ǧ++;if(ǧ>ΑΘ.Length-1) ǧ=0;if(ΓΥ>=ΑΘ.Length) break;if(!ΑΘ[ǧ]) break;}bool ǎ=Π==Ν.λ;if(!ǎ&&Ǹ&&Ѐ!=-1&&ś==0)
ΒЄ=Ѐ<1000000?Math.Round(Ѐ)+" Kg":Math.Round(Ѐ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){ǝ();Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ǎ) Χ(Ι.ĭ);else Χ(Ι.Ļ);if(!ǎ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΓΫ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΓΦ=true;Β+=Ȥ;if(!ǎ) Β+="Progress: "+Math.Round(Ş,1)+" %\n";Β+="State: "+Αο(Ō)+" "+ΓΟ+"m \n";Β+="Load: "+Ƽ+" % Max: "+ΒЄ+" \n";if(ǧ==0) Β+="Uranium: "+(ǉ.Count==0?"No reactors":Math.Round(ΓΧ,1)+"Kg "+(Ц==-1?"":" Min: "+Ц+" Kg"))+"\n";
if(ǧ==1) Β+="Battery: "+(Ā==і.None?Αι(Ā):ΒΦ+"% "+(п==-1||ǎ?"":" Min: "+п+" %"))+"\n";if(ǧ==2) Β+="Hydrogen: "+(đ.Count==0?"No tanks":Math.Round(Βϊ,1)+"% "+(р==-1||ǎ?"":" Min: "+р+" %"))+"\n";}else if(ή==Ι.ĭ){String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Back")){if(ς==Σ.Ǚ) Χ(Ι.Ȫ);else Χ(Ι.Main);}
if(Ζ(ref κ,Δ,Α++,Ε," Max load: "+о+"%")) Џ(ref о,о<=80?-10:-5,π.ƌ,false);if(Ζ(ref κ,Δ,Α++,Ε," Weight limit: "+(Ǹ?"On":"Off"))) Ǹ=!Ǹ;if(Ζ(ref κ,Δ,Α++,Ε," Ejection: "+ΓЉ(ĉ))) ĉ=ј(ĉ);if(Ζ(ref κ,Δ,Α++,Ε," Toggle sorters: "+(ǳ?"On":"Off"))){ǳ=!ǳ;if(ǳ) Ƙ(ΑΛ);}
if(Ζ(ref κ,Δ,Α++,Ε," Unload ice: "+(ǖ?"On":"Off"))) ǖ=!ǖ;if(Ζ(ref κ,Δ,Α++,Ε," Uranium: "+(Ц==-1?"Ignore":"Min "+Ц+"Kg"))) Џ(ref Ц,(Ц>5?-5:-1),π.Ǝ,true);if(Ζ(ref κ,Δ,Α++,Ε," Battery: "+(п==-1?"Ignore":"Min "+п+"%"))) Џ(ref п,-5,π.ΑЄ,true);if(Ζ(ref κ,Δ,Α++,Ε," Hydrogen: "+(р==-1?"Ignore":"Min "+р+"%"))) Џ(ref р,-10,π.ΑЅ,true);
if(Ζ(ref κ,Δ,Α++,Ε," When done: "+(Ĩ?"Return home":"Stop"))) Ĩ=!Ĩ;if(Ζ(ref κ,Δ,Α++,Ε," On damage: "+Αλ(Ǔ))) Ǔ=ј(Ǔ);if(Ζ(ref κ,Δ,Α++,Ε," Advanced...")) Χ(Ι.ŷ);Β+=ƕ(8,κ,Δ,ref ΕІ);}else if(ή==Ι.ŷ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Σ.Ǚ) Χ(Ι.Ȫ);else Χ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Π==Ν.Ǿ?" Grinder":" Drill")+" inv. balancing: "+(ē?"On":"Off"))) ē=!ē;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Π==Ν.Ǿ?" grinders":" drills")+": "+(Ċ?"Fwd + Bwd":"Fwd"))) Ċ=!Ċ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ŵ+"m/s")) Џ(ref Ŵ,0.5f,π.ƞ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ų+"m/s")) Џ(ref ų,0.5f,π.ƞ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѝ*100f)+"%"+(ѝ>0.80f?" (risky)":""))) Џ(ref ѝ,0.1f,π.Ŷ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ĩ*100f+"%")) Αα(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ď*100f+"%")) Αα(false,0.05f);
}else if(ή==Ι.Ļ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Σ.Ǚ) Χ(Ι.Ȫ);else Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+о+"%")) Џ(ref о,о<=80?-10:-5,π.ƌ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ǖ?"On":"Off"))) ǖ=!ǖ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Ц==-1?"Ignore":"Min "+Ц+"Kg"))) Џ(ref Ц,(Ц>5?-5:-1),π.Ǝ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(п==-1?"Ignore":"Charge up"))) п=(п==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(р==-1?"Ignore":"Fill up"))) р=(р==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+Αλ(Ǔ))) Ǔ=ј(Ǔ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ѝ*100f)+"%"+(ѝ>0.80f?" (risky)":""))) Џ(ref ѝ,0.1f,π.Ŷ,false);
}else if(ή==Ι.ΑЏ){double Εκ=0;if(υ.Count>0) Εκ=Vector3.Distance(υ.Last().Υ,д);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΔϏ();if(Π!=Ν.λ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(Ǘ?"Use old home":(Ρ.ι?"Was set! ":"none ")))) Ǘ=!Ǘ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(Ǘ?"Use old connector":(Ρ.ι?"Was set! ":"none ")))) Ǘ=!Ǘ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ħ?"Use old connector":(Φ.ι?"Was set! ":"none ")))) Ħ=!Ħ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ħ?"Use old path":(υ.Count>1?"Count: "+υ.Count:"none ")))) ħ=!ħ;Β+=Ȥ;Β+="Wp spacing: "+Math.Round(ΖΡ)+"m\n";}else if(ή==Ι.ƛ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){ǝ();Χ(Ι.Main);}
Β+=Ȥ;Β+="State: "+Αο(Ō)+" \n";Β+="Speed: "+Math.Round(ΕЌ,1)+"m/s\n";Β+="Target dist: "+ΓΟ+"m\n";Β+="Wp count: "+υ.Count+"\n";Β+="Wp left: "+ΖΣ+"\n";}else if(ή==Ι.Αω){List<IMyTerminalBlock>ǡ=ΓІ();if(Ĕ) Ǎ++;if(Ǎ>=ǡ.Count) Ǎ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.Αϊ);
Β+=Ȥ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(Ƽ,1)+"% "+Math.Round(Ǡ,1)+" / "+Math.Round(ǫ,1)+"\n";Β+="Uranium: "+(ǉ.Count==0?"No reactors":Math.Round(ΓΧ,1)+"Kg "+Εξ)+"\n";Β+="Battery: "+(Ā==і.None?"":ΒΦ+"% ")+Αι(Ā)+"\n";Β+="Hydrogen: "+(đ.Count==0?"No tanks":Math.Round(Βϊ,1)+"% ")+"\n";
Β+="Damage: "+(ǡ.Count==0?"None":""+(Ǎ+1)+"/"+ǡ.Count+" "+ǡ[Ǎ].CustomName)+"\n";}else if(ή==Ι.Αϊ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȥ;Β+="Next scan: "+Ū+"s\n";Β+="Drill area: "+Math.Round(ǵ,1)+"x"+Math.Round(Ǵ,1)+"m Ship: "+Math.Round(ΓΚ,1)+"x"+Math.Round(ΓΙ,1)+"m ("+Math.Round(Ȣ,1)+"m)\n";
Β+="Broadcast: "+(Ă?"Online - "+Ğ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȑ*100f,1)+"% \n";}else if(ή==Ι.Αχ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȥ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ΑΔ==2) Β="Fatal setup error\nNext scan: "+Ū+"s\n";if(ΓΕ) Β="Recompile script now";int Εμ=Β.Split('\n').Length;for(int i=Εμ;i<=10;i++) Β+="\n";Β+=ȡ;Β+="Last: "+Ψ+"\n";
return Β;}void Αα(bool Εη,float ŭ){ǝ();Ɲ(true,false);if(Εη) Џ(ref ĩ,ŭ,π.Ɛ,false);else Џ(ref ď,ŭ,π.Ɛ,false);Γι();ŀ(true,true,0,0);}float Αγ(float Ȇ,float[]Ű){float Ϊ=Ű[0];for(int i=Ű.Length-1;i>=0;i--)
if(Ȇ<Ű[i]) Ϊ=Ű[i];return Ϊ;}String Ĵ(ref int џ){String Ϊ="";if(џ>=ΓΤ.Count) џ=-1;if(џ>=0) Ϊ=ΓΤ[џ].CustomName;џ++;return Ϊ;}void ǹ(ref int value,int ā,int Ů,int ΑΙ){if(ā==0) return;if(value<Ů&&ΑΙ>0||value<=Ů&&ΑΙ<0){value+=ΑΙ;return;}int ΔА=Math.Abs(ā);int Ɵ=0;
int ǅ=1;while(true){Ɵ+=ǅ*ΔА*10;if(ā<0&&value-Ů<=Ɵ) break;if(ā>0&&value-Ů<Ɵ) break;ǅ++;}value+=ǅ*ā;}void Ȃ(bool Ηξ){С=Math.Max(С,1);Р=Math.Max(Р,1);Э=Math.Max(Э,0);ŀ(ή==Ι.ȫ,false,0,0);}void Џ(ref float value,float ŭ,π Њ,bool Ĥ){Џ(ref value,value+ŭ,Њ,Ĥ,true);
}void Џ(ref float value,float ȓ,π Њ,bool Ĥ,bool ΖΧ){float Ч=0,Х=0;if(Њ==π.ƞ){Ч=0.5f;Х=10f;}if(Њ==π.Ŷ){Ч=0.1f;Х=1f;}if(Њ==π.Ɔ){Ч=50f;Х=100f;}if(Њ==π.ΑЄ){Ч=5f;Х=30f;}if(Њ==π.Ǝ){Ч=1f;Х=25f;}if(Њ==π.ƌ){Ч=10f;Х=95f;}if(Њ==π.ΑЅ){Ч=10f;Х=90f;}
if(Њ==π.ΔЅ){Ч=10f;Х=1800;}if(Њ==π.Ɛ){Ч=0.0f;Х=0.75f;}if(ȓ==-1&&Ĥ){value=-1;return;}if(value==-1) Ĥ=false;bool ΒЎ=ȓ<Ч||ȓ>Х;if(ΒЎ&&ΖΧ){if(ȓ<value) value=Х;else if(ȓ>value) value=Ч;}else value=ȓ;if(ΒЎ&&Ĥ) value=-1;else value=Math.Max(Ч,Math.Min(value,Х));
value=(float) Math.Round(value,2);}void ΓΫ(bool ΕЀ){if(ΕЀ) к=Math.Max(0,к-1);else к++;Γϋ();Γϊ();for(int k=0;k<ƶ.Count&&k<Я.Length;k++) Δυ(ƶ[k],k<к);ǭ=-1;ΒΟ(true);}T ј<T>(T Ȇ){int Α=Array.IndexOf(Enum.GetValues(Ȇ.GetType()),Ȇ);Α++;if(Α>=Γό(Ȇ)) Α=0;return(T) Enum.GetValues(Ȇ.GetType()).GetValue(Α);
}int Γό<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δϋ(){Ų.Clear();for(int i=0;i<υ.Count;i++) Ų.Add(υ[i]);υ.Clear();Βϋ=true;Βω=new Ω(Ρ);Βύ=new Ω(Φ);Ρ.ι=false;if(Π==Ν.λ) Φ.ι=false;for(int i=0;i<є.Count;i++)
if(!ѕ.Contains(є.Keys.ElementAt(i)))
ѕ.Add(є.Keys.ElementAt(i));Ǘ=false;Ħ=false;ħ=false;Χ(Ι.ΑЏ);}void ΔϏ(){if(Ǘ) Ρ=Βω;if(Ħ) Φ=Βύ;if(ħ){υ.Clear();for(int i=0;i<Ų.Count;i++) υ.Add(Ų[i]);}Βϋ=false;ǝ();Χ(Ι.Main);}void Ĺ(Ω У,Σ ώ){ΕΟ=У;if(ς==Σ.Ǚ) Ǒ=ώ;}bool Αμ(Ω У,float В,bool ΖΝ,out Vector3 β){if(ΖΝ){Vector3I Εά=new Vector3I((int) У.Т.X,(int) У.Т.Y,(int) У.Т.Z);
IMySlimBlock ά=Me.CubeGrid.GetCubeBlock(Εά);if(ά==null||!(ά.FatBlock is IMyShipConnector)){β=new Vector3();return false;}Vector3 ΕΛ=ĳ(Ј,ά.FatBlock.GetPosition()-д);Vector3 ΕΚ=ĳ(Ј,ά.FatBlock.WorldMatrix.Forward);β=У.Υ-ΒΜ(У.Є,У.Ϗ*-1,ΕΛ)-ΒΜ(У.Є,У.Ϗ*-1,ΕΚ)*В;return true;}
else{β=У.Υ;return true;}}Vector3 ΓЍ(int ΕΘ,int ȝ,bool ΕЁ){if(!ΕЁ&&Βσ) return Ʀ;float ΕΙ=((в-1f)/2f)-ΕΘ;float ΕЃ=((б-1f)/2f)-ȝ;Ʀ=Φ.Υ+Φ.Й*ΕΙ*ǵ+Ȓ*-1*ΕЃ*Ǵ;Βσ=true;return Ʀ;}Τ ſ(){float П=-1;Τ Ϊ=Τ.ж;if(Π!=Ν.λ){ΔΣ();if(ς!=Σ.щ){Vector3 ω=ΒΝ(ę,Ȓ*-1,д-Φ.Υ);
if(Math.Abs(ω.X)<=(float)(в*ǵ)/2f&&Math.Abs(ω.Y)<=(float)(б*Ǵ)/2f){if(ω.Z<=-1&&ω.Z>=-ΑΑ*2) return Τ.Ĭ;if(ω.Z>-1&&ω.Z<Ȣ*2) return Τ.ж;}if(ї(Φ.Υ,ref П)) Ϊ=Τ.ж;}if(Ρ.ι){if(ї(Ρ.Υ,ref П)) Ϊ=Τ.е;for(int i=0;i<υ.Count;i++)
if(ї(υ[i].Υ,ref П)) Ϊ=Τ.ъ;if(Vector3.Distance(д,Ρ.Υ)<dockDist*Ȣ) Ϊ=Τ.е;if(Ǽ(MyShipConnectorStatus.Connectable)!=null||Ǽ(MyShipConnectorStatus.Connected)!=null)
Ϊ=Τ.е;}}else{Vector3 β=new Vector3();IMyShipConnector Ň=Ǽ(MyShipConnectorStatus.Connected);if(Ρ.ι){if(ї(Ρ.Υ,ref П)) Ϊ=Τ.е;if(Αμ(Ρ,dockDist,true,out β))
if(ї(β,ref П)) Ϊ=Τ.е;if(Ň!=null&&Vector3.Distance(Ň.GetPosition(),Ρ.Υ)<5)
return Τ.Ÿ;}for(int i=0;i<υ.Count;i++)
if(Vector3.Distance(υ[i].Υ,Ρ.Υ)>dockDist*Ȣ&&Vector3.Distance(υ[i].Υ,Φ.Υ)>dockDist*Ȣ)
if(ї(υ[i].Υ,ref П)) Ϊ=Τ.ъ;if(Φ.ι){if(ї(Φ.Υ,ref П)) Ϊ=Τ.ж;if(Αμ(Φ,dockDist,true,out β))
if(ї(β,ref П)) Ϊ=Τ.ж;if(Ň!=null&&Vector3.Distance(Ň.GetPosition(),Φ.Υ)<5)
return Τ.Ź;}}return Ϊ;}bool ї(Vector3 β,ref float П){float В=Vector3.Distance(β,д);if(В<П||П==-1){П=В;return true;}return false;}void ΒΗ(){if(ΑΔ>0){Ψ="Setup error! Can't start";return;}if(Π==Ν.λ){Αβ();return;}Φ.Υ=д;Φ.μ=Ј.GetNaturalGravity();Φ.Є=ΕΩ;Φ.Ϗ=ΕΡ;Φ.Й=Ελ;
ę=ǔ.WorldMatrix.Forward;Ȓ=Φ.Ϗ;if(ę==Ј.WorldMatrix.Down) Ȓ=Ј.WorldMatrix.Backward;Ɲ(true,true);ΰ(Λ.Ƃ);ƚ();}void Ɲ(bool Ф,bool ΕЂ){if(ς==Σ.щ&&!Ф) return;bool Ευ=Ф||ς==Σ.Αε||в!=С||б!=Р||Ȏ!=ǲ;if(Ευ){if(ς!=Σ.щ){ς=Σ.Į;ΓЍ(Ř,ř,ΕЂ);Ψ="Job changed, lost progress";}Ȏ=ǲ;
в=С;б=Р;ř=0;Ř=0;Я="";ǭ=-1;ǁ=0;ė=0;Βδ=0;к=0;ΒΟ(true);}}void ƒ(){Δσ(д,0);ĸ(Ģ,true);}void ƚ(){ΒΕ(ћ.ΒΑ);ƒ();Ɨ(ƹ,false);Ƙ(ΑΛ);ς=Σ.Ǚ;Γθ(true);Ǒ=ς;Χ(Ι.Ȫ);Δμ();Βε=true;Ʒ=0;for(int i=Ȳ.Count-1;i>=0;i--)
if(Αό(Ȳ[i],false)) Ʒ++;if(Ʒ>0) Ψ="Started with damage";}void ǝ(){if(ς==Σ.Ǚ){ς=Σ.Ƒ;Ψ="Job paused";}ΰ(Λ.ǚ);Ǒ=ς;Δτ(false,0,0,0,0);Δύ();Γή(new Vector3(),false);Δώ();Δπ(ChargeMode.Auto);Δφ(false);Δς(true);ǜ(Λ.ǚ);ŀ(false,false,0,0);ĸ(Ы,false);ĸ(ą,true);Ƙ(true);ΓΖ=false;Γμ();
ΕΠ=false;Δζ();Βε=false;ΑΗ=false;ΓΦ=false;if(ή!=Ι.Main&&ή!=Ι.ĭ&&ή!=Ι.ŷ&&ή!=Ι.Ļ)
Χ(Ι.Main);}void Αβ(){Τ ѐ=ſ();if(Π==Ν.λ){if(!Φ.ι||!Ρ.ι) return;ƚ();bool Ʋ=Vector3.Distance(д,Ρ.Υ)<Vector3.Distance(д,Φ.Υ);if(ΑΒ==Σ.Ī) Ʋ=true;if(ΑΒ==Σ.ī) Ʋ=false;if(Ʋ){Ĺ(Ρ,Σ.Ī);switch(ѐ){case Τ.Ÿ:ΰ(Λ.ȱ);break;case Τ.ъ:ΰ(Λ.њ);break;case Τ.е:ΰ(Λ.Ǻ);break;default:ΰ(Λ.ȯ);break;
}}else{Ĺ(Φ,Σ.ī);switch(ѐ){case Τ.Ź:ΰ(Λ.ȱ);break;case Τ.ж:ΰ(Λ.Ǻ);break;case Τ.ъ:ΰ(Λ.њ);break;default:ΰ(Λ.ȯ);break;}}}else{if(ς!=Σ.Ƒ&&ς!=Σ.Į) return;bool ΖΠ=ς==Σ.Į;ƚ();bool ƴ=ΔΞ(false)&&Ρ.ι;switch(ѐ){case Τ.ж:ΰ(ƴ?Λ.Ɓ:Λ.Ƃ);break;case Τ.ъ:ΰ(ƴ?Λ.ȩ:Λ.њ);break;case Τ.е:ΰ(ƴ?Λ.Ǻ:Λ.Ȱ);break;
case Τ.Ĭ:{if(ė!=к||ΖΠ) ΰ(Λ.Ɣ);else ΰ(Λ.ΑІ);}break;default:break;}}}void ΓЃ(){if(ς==Σ.щ&&!Ρ.ι) return;if(Π==Ν.λ&&(!Φ.ι||!Ρ.ι)) return;Ψ="Move to job";Τ ѐ=ſ();if(Π==Ν.λ){Ĺ(Φ,Σ.ī);switch(ѐ){case Τ.ж:ΰ(Λ.Ǻ);break;case Τ.ъ:ΰ(Λ.њ);break;case Τ.Ź:return;default:ΰ(Λ.ȯ);break;
}ǜ(Λ.ȱ);}else{switch(ѐ){case Τ.ж:ΰ(Λ.њ);break;case Τ.ъ:ΰ(Λ.њ);break;case Τ.е:ΰ(Λ.Ȱ);break;case Τ.Ĭ:ΰ(Λ.Ɣ);break;default:break;}if(ς==Σ.щ) ǜ(Λ.њ);else ǜ(Λ.Αέ);ΓΖ=true;}ƒ();Χ(Ι.ƛ);Ɨ(ƹ,false);Ǒ=Σ.ΑЈ;}void ΓЀ(){if(!Ρ.ι) return;Ψ="Move home";Τ ѐ=ſ();if(Π==Ν.λ){Ĺ(Ρ,Σ.Ī);switch(ѐ)
{case Τ.ъ:ΰ(Λ.ȩ);break;case Τ.е:ΰ(Λ.Ǻ);break;case Τ.Ÿ:return;default:ΰ(Λ.ȯ);break;}ǜ(Λ.ȱ);}else{if(Ǽ(MyShipConnectorStatus.Connected)!=null) return;if(Ǽ(MyShipConnectorStatus.Connectable)!=null){ΰ(Λ.Αδ);ǜ(Λ.Ȱ);return;}switch(ѐ){case Τ.ж:ΰ(Λ.Ɓ);break;case Τ.ъ:ΰ(Λ.ȩ);break;case Τ.е:ΰ(Λ.ȩ);break;
case Τ.Ĭ:ΰ(Λ.ΑБ);break;default:break;}ǜ(Λ.Ȱ);}ƒ();Χ(Ι.ƛ);Ɨ(ƹ,false);Ǒ=Σ.ΑЇ;}void ΰ(Λ ǆ){if(ǆ==Λ.ǚ) Ġ=Λ.ǚ;if(Ġ!=Λ.ǚ&&Ō==Ġ&&ǆ!=Ġ){ǝ();return;}ΕА=true;Ō=ǆ;}void ǜ(Λ ώ){this.Ġ=ώ;}void ΒΟ(bool Ǌ){if(Ǌ){Ş=0;return;}if(Π==Ν.λ) return;float ψ=к*Math.Max(1,Э);if(ė==к) ψ+=Math.Min(Э,Βδ);
float ΖΚ=в*б*Math.Max(1,Э);Ş=Math.Max(Ş,(float) Math.Min(ψ/ΖΚ*100.0,100));}List<IMyTerminalBlock>ΓІ(){List<IMyTerminalBlock>ǡ=new List<IMyTerminalBlock>();for(int i=0;i<Ȳ.Count;i++){IMyTerminalBlock ά=Ȳ[i];if(Αό(ά,false)) ǡ.Add(ά);}return ǡ;}bool Αό(IMyTerminalBlock ά,bool ΑΞ)
{return(!Αύ(ά,ΑΞ)||!ά.IsFunctional);}bool Αύ(IMyTerminalBlock ά,bool ΑΞ){if(ά==null) return false;try{IMyCubeBlock Βς=Me.CubeGrid.GetCubeBlock(ά.Position).FatBlock;if(ΑΞ) return Βς==ά;else return Βς.GetType()==ά.GetType();}catch{return false;}}bool ΔΞ(bool ΗΞ){if(Ǹ&&Π!=Ν.λ)
if(Ѐ!=-1&&ΓΛ>=Ѐ){Ψ="Ship too heavy";return true;}if(Ƽ>=о||ΑΗ){ΑΗ=false;Ψ="Ship is full";return true;}return false;}static String ΔΒ(IMyThrust у){return у.BlockDefinition.SubtypeId;}Vector3 Ƌ(Vector3 Е,float[,]Ȑ){return new Vector3(
Е.X>=0?Ȑ[0,0]:Ȑ[0,1],
Е.Y>=0?Ȑ[1,0]:Ȑ[1,1],
Е.Z>=0?Ȑ[2,0]:Ȑ[2,1]);}bool Ασ(Ω Ϋ,String Ƕ,out float Ư){Ư=0;int Α=ѕ.IndexOf(Ƕ);if(Α==-1||Ϋ.Ъ==null||Α>=Ϋ.Ъ.Length)
return false;Ư=Ϋ.Ъ[Α];if(Ư==-1) return false;return true;}Vector3 Ας(Vector3 Е,Ω Ϋ){if(Ϋ!=null){Vector3 Ϊ=new Vector3();for(int i=0;i<є.Keys.Count;i++){String Ƕ=є.Keys.ElementAt(i);float Ĉ=0;if(!Ασ(Ϋ,Ƕ,out Ĉ))
return Ƌ(Е,ĥ);Ϊ+=Ƌ(Е,є.Values.ElementAt(i))*Ĉ;}return Ϊ;}return Ƌ(Е,ĥ);}float ƅ(Vector3 Е,Ω Ϋ){return ƅ(Е,new Vector3(),Ϋ);}float ƅ(Vector3 Е,Vector3 μ,Ω Ϋ){Vector3 ΖΔ=Ας(Е,Ϋ);Vector3 ΕΤ=ΖΔ+μ*ΓΛ;float ǯ=(ΕΤ/Е).AbsMin();return(float)(Е*ǯ).Length();}float ı(Vector3 č,Vector3 Ё,Vector3 μ,Ω Ϋ)
{if(μ.Length()==0f) return 0;Vector3 Εν=ΒΝ(č,Ё,Vector3.Normalize(μ));float Ф=ƅ(-Εν,Ϋ);return Ф/μ.Length();}void Γθ(bool Εγ){float Л=0;float ư=0.9f;if(Εγ){Ѐ=-1;Ƿ=0;ū=null;if(ς!=Σ.щ&&Φ.μ.Length()!=0){Л=ư*ı(Φ.Є,Φ.Ϗ*-1,Φ.μ,null);if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}if(Ρ.ι&&Ρ.μ.Length()!=0)
{Л=ư*ı(Ρ.Є,Ρ.Ϗ*-1,Ρ.μ,null);if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}return;}if(Ƿ==-1) return;if(Ƿ>=0){int ΓΒ=0;while(Ƿ<υ.Count){if(ΓΒ>100) return;ΓΒ++;Ω Ϋ=υ[Ƿ];if(Ϋ.μ.Length()!=0f){Л=ư*Math.Min(
ı(Ϋ.Є,Ϋ.Ϗ*-1,Ϋ.μ,Ϋ),
ı(Ϋ.Є*-1,Ϋ.Ϗ*-1,Ϋ.μ,Ϋ));if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}else ū=Ϋ;Ƿ++;}Ƿ=-1;}bool Ƴ=true;float ǒ=0;if(ū!=null){for(int i=0;i<є.Count;i++){String Ƕ=є.Keys.ElementAt(i);float[,]ΑΥ=є.Values.ElementAt(i);float Ĉ=0;if(!Ασ(ū,Ƕ,out Ĉ)){Ƴ=false;break;}for(int a=0;a<ΑΥ.GetLength(0);a++)
for(int b=0;b<ΑΥ.GetLength(1);b++){float Ǟ=Math.Abs(ΑΥ[a,b]*Ĉ);if(Ǟ==0) continue;Ƴ=true;if(ǒ==0||Ǟ<ǒ) ǒ=Ǟ;}}}if(!Ƴ){for(int a=0;a<ĥ.GetLength(0);a++)
for(int b=0;b<ĥ.GetLength(1);b++){float Ǟ=Math.Abs(ĥ[a,b]);if(Ǟ==0) continue;if(ǒ==0||Ǟ<ǒ) ǒ=Ǟ;}}if(ǒ>0){float Ερ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Л=М(ǒ,Ερ);if(Л>0)
if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}}void Γή(Vector3 Ф,bool ύ){if(!ύ){for(int i=0;i<Ģ.Count;i++)
Ģ[i].SetValueFloat("Override",0.0f);return;}Vector3 ơ=Ας(Ф,null);float ΖΨ=Math.Min(1,Math.Abs(М(Ф.X,ơ.X)));float ΖΩ=Math.Min(1,Math.Abs(М(Ф.Y,ơ.Y)));float ΖΪ=Math.Min(1,Math.Abs(М(Ф.Z,ơ.Z)));for(int i=0;i<Ģ.Count;i++){IMyThrust у=Ģ[i];Vector3 ȇ=Δκ(ĳ(Ј,у.WorldMatrix.Backward),1);
if(ȇ.X!=0&&Math.Sign(ȇ.X)==Math.Sign(Ф.X))
у.SetValueFloat("Override",у.MaxThrust*ΖΨ);else if(ȇ.Y!=0&&Math.Sign(ȇ.Y)==Math.Sign(Ф.Y))
у.SetValueFloat("Override",у.MaxThrust*ΖΩ);else if(ȇ.Z!=0&&Math.Sign(ȇ.Z)==Math.Sign(Ф.Z))
у.SetValueFloat("Override",у.MaxThrust*ΖΪ);else
у.SetValueFloat("Override",0.0f);}}void Δτ(bool ύ,float ΕϏ,float ȗ,float Ȝ,float Ȩ){for(int i=0;i<Βυ.Count;i++){IMyGyro ν=Βυ[i];ν.GyroOverride=ύ;if(!ύ) ν.GyroPower=100;else ν.GyroPower=ΕϏ;if(!ύ) continue;Vector3 З=Ј.WorldMatrix.Forward;Vector3 ț=Ј.WorldMatrix.Right;Vector3 Ё=Ј.WorldMatrix.Up;
Vector3 Ȉ=ν.WorldMatrix.Forward;Vector3 Ȋ=ν.WorldMatrix.Up;Vector3 ȉ=ν.WorldMatrix.Left*-1;if(Ȉ==З) ν.SetValueFloat("Roll",Ȩ);else if(Ȉ==(З*-1)) ν.SetValueFloat("Roll",Ȩ*-1);else if(Ȋ==(З*-1)) ν.SetValueFloat("Yaw",Ȩ);else if(Ȋ==З) ν.SetValueFloat("Yaw",Ȩ*-1);else if(ȉ==З) ν.SetValueFloat("Pitch",Ȩ);
else if(ȉ==(З*-1)) ν.SetValueFloat("Pitch",Ȩ*-1);if(ȉ==(ț*-1)) ν.SetValueFloat("Pitch",ȗ);else if(ȉ==ț) ν.SetValueFloat("Pitch",ȗ*-1);else if(Ȋ==ț) ν.SetValueFloat("Yaw",ȗ);else if(Ȋ==(ț*-1)) ν.SetValueFloat("Yaw",ȗ*-1);else if(Ȉ==(ț*-1)) ν.SetValueFloat("Roll",ȗ);
else if(Ȉ==ț) ν.SetValueFloat("Roll",ȗ*-1);if(Ȋ==(Ё*-1)) ν.SetValueFloat("Yaw",Ȝ);else if(Ȋ==Ё) ν.SetValueFloat("Yaw",Ȝ*-1);else if(ȉ==Ё) ν.SetValueFloat("Pitch",Ȝ);else if(ȉ==(Ё*-1)) ν.SetValueFloat("Pitch",Ȝ*-1);else if(Ȉ==Ё) ν.SetValueFloat("Roll",Ȝ);else if(Ȉ==(Ё*-1)) ν.SetValueFloat("Roll",Ȝ*-1);
}}void Δώ(){this.Εέ=false;}void Δύ(){ĺ(false,false,false,ΒЌ,0);ΓΣ=false;}void Δσ(Vector3 ί,float ğ){ĺ(true,false,false,ί,ğ);}void ĺ(bool ύ,bool ΑΚ,bool ΑΦ,Vector3 ί,float ğ){ĺ(ύ,ΑΚ,ΑΦ,ί,ί-д,0.0f,ğ);}void ĺ(bool ύ,bool ΑΚ,bool ΑΦ,Vector3 ί,Vector3 ΕЈ,float ΕЉ,float ğ)
{ΓΣ=true;this.ΖΕ=ύ;ΒЌ=ί;this.Εο=ğ;this.Εό=ΕЉ;this.ΕЎ=ΑΚ;this.ΕЍ=ΑΦ;this.ΕΧ=ΕЈ;Βκ=Vector3.Distance(ί,д);}void Δμ(){И.GetBlocksOfType(Ȳ,Ɗ);}void Γι(){if(Ј==null) return;ǔ=null;float ť=0,š=0,Ŧ=0,Ţ=0,ŧ=0,ţ=0;List<IMyTerminalBlock>ΑΟ=Αη(Ы,pamTag,true);bool Εχ=ΑΟ.Count==0;
if(ΑΟ.Count>0) ǔ=ΑΟ[0];else if(Ы.Count>0) ǔ=Ы[0];int ŋ=0;float ș=0;for(int i=0;i<Ы.Count;i++){if(Ы[i].WorldMatrix.Forward!=ǔ.WorldMatrix.Forward){if(Εχ){ΑΔ=2;Ψ="Mining direction is unclear!";return;}continue;}ŋ++;ș=Math.Max(ș,ΔІ(Ы[i]));Vector3 ω=Ƅ(Ј,Ы[i].GetPosition());
if(i==0){ť=ω.X;š=ω.X;Ŧ=ω.Y;Ţ=ω.Y;ŧ=ω.Z;ţ=ω.Z;}š=Math.Max(ω.X,š);ť=Math.Min(ω.X,ť);Ţ=Math.Max(ω.Y,Ţ);Ŧ=Math.Min(ω.Y,Ŧ);ţ=Math.Max(ω.Z,ţ);ŧ=Math.Min(ω.Z,ŧ);}if(ș==0) ș=drillRadius;bool Βν=ǔ!=null&&ǔ.WorldMatrix.Forward==Ј.WorldMatrix.Down;ǵ=(š-ť)*(1-ĩ)+ș*2;
Ǵ=(Βν?ţ-ŧ:Ţ-Ŧ)*(1-ď)+ș*2;IMyCubeGrid g=Ј.CubeGrid;float ΑΩ=0,ΑΪ=0,ΑΫ=0,Αά=0;for(int c=0;c<8;c++){Vector3 p=Ƅ(Ј,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Βν?p.Z:p.Y;if(c==0||p.X<ΑΩ) ΑΩ=p.X;if(c==0||p.X>ΑΪ) ΑΪ=p.X;if(c==0||y<ΑΫ) ΑΫ=y;if(c==0||y>Αά) Αά=y;}ΓΚ=ΑΪ-ΑΩ+g.GridSize;ΓΙ=Αά-ΑΫ+g.GridSize;}float ΔІ(IMyTerminalBlock b)
{if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Αη<T>(List<T>Ѕ,String Щ,bool Ś){List<T>Ϊ=new List<T>();
for(int i=0;i<Ѕ.Count;i++)
if(Αφ(Ѕ[i],Щ,Ś)) Ϊ.Add(Ѕ[i]);return Ϊ;}bool Αφ<T>(T ά,String Щ,bool Ś){IMyTerminalBlock b=(IMyTerminalBlock) ά;if(Ś&&b.CustomName.ToUpper().Contains(Щ.ToUpper())) return true;if(!Ś&&b.BlockDefinition.ToString().ToUpper().Contains(Щ.ToUpper())) return true;return false;}void ĸ<T>(List<T>Ѕ,bool ύ)
{for(int i=0;i<Ѕ.Count;i++)
Δρ((IMyTerminalBlock) Ѕ[i],ύ);}void Δρ(IMyTerminalBlock ά,bool ύ){if(ά==null) return;String ΔБ=ύ?"OnOff_On":"OnOff_Off";var a=ά.GetActionWithName(ΔБ);a.Apply(ά);}void Ƙ(bool ύ){ΑΛ=ύ;if(!ǳ) return;ĸ(ΕЏ,ύ);}void Δπ(ChargeMode η){for(int i=0;i<ΒΥ.Count;i++) ΒΥ[i].ChargeMode=η;}void Δφ(bool ΖΑ)
{for(int i=0;i<đ.Count;i++) đ[i].Stockpile=ΖΑ;}void Ɨ(List<IMyLandingGear>Ʊ,bool ΒЅ){for(int i=0;i<Ʊ.Count;i++){if(ΒЅ) Ʊ[i].Lock();if(!ΒЅ) Ʊ[i].Unlock();}}void Δς(bool ύ){if(Βζ==ύ) return;List<IMyShipController>Ŋ=new List<IMyShipController>();И.GetBlocksOfType(Ŋ,Ɗ);if(Ŋ.Count==0) return;
for(int i=0;i<Ŋ.Count;i++)
Ŋ[i].DampenersOverride=ύ;Βζ=ύ;}IMyShipConnector Ǽ(MyShipConnectorStatus ΕБ){for(int i=0;i<ą.Count;i++){if(!Αύ(ą[i],true)) continue;if(ą[i].Status==ΕБ) return ą[i];}return null;}void ŀ(bool ΑΕ,bool ō,float ΕΪ,float ΕΓ){if(ϊ==null||Ы.Count==0) return;Vector3 Ņ=new Vector3();
int ŋ=0;for(int i=0;i<Ы.Count;i++){if(Ы[i].WorldMatrix.Forward!=ǔ.WorldMatrix.Forward) continue;ŋ++;Ņ+=Ы[i].GetPosition();}Ņ=Ņ/ŋ;Vector3 Ȕ=Ƅ(ϊ,Ņ);ϊ.Enabled=true;ϊ.ShowOnHUD=ΑΕ;ϊ.LeftExtend=(ō?1:С)/2f*ǵ-Ȕ.X;ϊ.RightExtend=(ō?1:С)/2f*ǵ+Ȕ.X;ϊ.TopExtend=(ō?1:Р)/2f*Ǵ+Ȕ.Y;
ϊ.BottomExtend=(ō?1:Р)/2f*Ǵ-Ȕ.Y;ϊ.FrontExtend=(ΑΕ?Э:ΕΪ)-Ȕ.Z;ϊ.BackExtend=ΑΕ?0:ΕΓ+Ȣ*0.75f+Ȕ.Z;ϊ.DetectFloatingObjects=true;ϊ.DetectAsteroids=false;ϊ.DetectLargeShips=true;ϊ.DetectSmallShips=true;ϊ.DetectStations=true;ϊ.DetectOwner=true;ϊ.DetectSubgrids=false;
ϊ.DetectPlayers=false;ϊ.DetectEnemy=true;ϊ.DetectFriendly=true;ϊ.DetectNeutral=true;}void ȭ<T>(List<T>Ѕ,bool ΕЋ,float ΕΨ,bool Εζ){for(int i=0;i<Ѕ.Count;i++){IMyTextSurface ġ=null;if(Ѕ[i]is IMyTextSurface) ġ=(IMyTextSurface) Ѕ[i];if(ġ!=null){ġ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΕЋ) continue;ġ.Font="Debug";if(Εζ) continue;ġ.FontSize=ΕΨ;}}}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 Δκ(Vector3 v,int ƪ){return new Vector3(Math.Round(v.X,ƪ),Math.Round(v.Y,ƪ),Math.Round(v.Z,ƪ));}Vector3 Ƅ(IMyTerminalBlock ά,Vector3 ΖΦ){return ĳ(ά,ΖΦ-ά.GetPosition());
}Vector3 ĳ(IMyTerminalBlock ά,Vector3 ΑΨ){return Vector3D.TransformNormal(ΑΨ,MatrixD.Transpose(ά.WorldMatrix));}Vector3 ΒΝ(Vector3 č,Vector3 Ё,Vector3 ΑΨ){MatrixD ǀ=MatrixD.CreateFromDir(č,Ё);return Vector3D.TransformNormal(ΑΨ,MatrixD.Transpose(ǀ));}Vector3 ΒΜ(Vector3 č,Vector3 Ё,Vector3 ω)
{MatrixD ǀ=MatrixD.CreateFromDir(č,Ё);return Vector3D.Transform(ω,ǀ);}String έ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 α(String s){String[]δ=s.Split('|');return new Vector3(float.Parse(Θ(δ,0)),float.Parse(Θ(δ,1)),float.Parse(Θ(δ,2)));}String ΔЏ(Ω Ϋ){String Ǳ=":";String Ϊ=έ(Ϋ.Υ)+Ǳ+έ(Ϋ.Є)+Ǳ+
έ(Ϋ.Ϗ)+Ǳ+έ(Ϋ.Й)+Ǳ+έ(Ϋ.μ);for(int i=0;i<Ϋ.Ъ.Length;i++)
Ϊ+=Ǳ+Math.Round(Ϋ.Ъ[i],3);return Ϊ;}Ω ΔЀ(String s){String[]δ=s.Split(':');Ω Ϋ=new Ω(
α(Θ(δ,0)),α(Θ(δ,1)),
α(Θ(δ,2)),α(Θ(δ,3)),
α(Θ(δ,4)));int Α=5;List<float>Βο=new List<float>();while(Α<δ.Length){String ů=Θ(δ,Α);float f=0;if(!float.TryParse(ů,out f)) break;Βο.Add(f);Α++;}Ϋ.Ъ=Βο.ToArray();return Ϋ;}System.Text.StringBuilder ȟ=new System.Text.StringBuilder();void Κ<T>(T value,bool Εφ){if(Εφ) ȟ.Append('\n');ȟ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String į(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ŀ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΕΑ,int Α){String ů=ΕΑ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ů)) return "";
return ů;}void Save(){if(ΓΕ||Π==Ν.А){Storage="";return;}Storage=Γγ();}String Γγ(){ȟ.Clear();ȟ.Append(DATAREV+";");Κ(έ(ΓΔ),false);Κ(έ(Ρ.Є));Κ(έ(Ρ.Й));Κ(έ(Ρ.Ϗ));Κ(έ(Ρ.μ));Κ(έ(Ρ.Υ));Κ(έ(Ρ.Т));Κ(Ρ.ι);Κ(έ(Φ.Υ));Κ(έ(Φ.μ));Κ(έ(Φ.Є));Κ(έ(Φ.Ϗ));Κ(έ(Φ.Й));Κ(έ(Φ.Т));Κ(Φ.ι);Κ(έ(ę));Κ(έ(Ȓ));Κ(";");
Κ((int) Π,false);Κ((int) ς);Κ((int) Ǒ);Κ(о);Κ(п);Κ(Ц);Κ(р);Κ(ѝ);Κ(Ǹ);Κ(ǖ);if(Π==Ν.λ){Κ((int) σ.К);Κ(σ.Ю);Κ(σ.Ǥ);Κ(į(σ.л));Κ(į(σ.н));Κ((int) τ.К);Κ(τ.Ю);Κ(τ.Ǥ);Κ(į(τ.л));Κ(į(τ.н));}else{Κ((int) ǲ);Κ((int) Ǔ);Κ((int) ĉ);Κ((int) ю);Κ((int) Ȏ);Κ(Ĩ);Κ(ǳ);Κ(ē);Κ(Ċ);Κ(С);Κ(Р);Κ(Э);Κ(Ŵ);Κ(ų);Κ(ĩ);Κ(ď);
Κ(в);Κ(б);Κ(Ř);Κ(ř);Κ(ė);Κ(к);Κ(ΑΑ);Κ(ǁ);}Κ(";");for(int i=0;i<ѕ.Count;i++)
Κ((i>0?"|":"")+ѕ[i],false);Κ(";");for(int i=0;i<υ.Count;i++)
Κ(ΔЏ(υ[i]),i>0);Κ(";",false);Κ(Я+"|"+ǭ,false);Κ(";",false);Κ(Γφ(),false);String Ξ=ȟ.ToString();ȟ.Clear();return Ξ;}з Δη(){return Δθ(Storage);}з Δθ(String Ξ){if(Ξ=="") return з.ΔΧ;String[]ǰ=Ξ.Split(';');if(Θ(ǰ,0)!=DATAREV) return з.ΑЉ;int Α=0;try{String[]Η=Θ(ǰ,1).Split('\n');
ΓΔ=α(Θ(Η,Α++));Ρ.Є=α(Θ(Η,Α++));Ρ.Й=α(Θ(Η,Α++));Ρ.Ϗ=α(Θ(Η,Α++));Ρ.μ=α(Θ(Η,Α++));Ρ.Υ=α(Θ(Η,Α++));Ρ.Т=α(Θ(Η,Α++));Ρ.ι=bool.Parse(Θ(Η,Α++));Φ.Υ=α(Θ(Η,Α++));Φ.μ=α(Θ(Η,Α++));Φ.Є=α(Θ(Η,Α++));Φ.Ϗ=α(Θ(Η,Α++));Φ.Й=α(Θ(Η,Α++));Φ.Т=α(Θ(Η,Α++));Φ.ι=bool.Parse(Θ(Η,Α++));
ę=α(Θ(Η,Α++));Ȓ=α(Θ(Η,Α++));Η=Θ(ǰ,2).Split('\n');Α=0;Π=(Ν) int.Parse(Θ(Η,Α++));ς=(Σ) int.Parse(Θ(Η,Α++));Ǒ=(Σ) int.Parse(Θ(Η,Α++));о=int.Parse(Θ(Η,Α++));п=int.Parse(Θ(Η,Α++));Ц=int.Parse(Θ(Η,Α++));р=int.Parse(Θ(Η,Α++));ѝ=float.Parse(Θ(Η,Α++));Ǹ=bool.Parse(Θ(Η,Α++));
ǖ=bool.Parse(Θ(Η,Α++));if(Π==Ν.λ){σ.К=(Ѓ) int.Parse(Θ(Η,Α++));σ.Ю=float.Parse(Θ(Η,Α++));σ.Ǥ=float.Parse(Θ(Η,Α++));σ.л=Ŀ(Θ(Η,Α++));σ.н=Ŀ(Θ(Η,Α++));τ.К=(Ѓ) int.Parse(Θ(Η,Α++));τ.Ю=float.Parse(Θ(Η,Α++));τ.Ǥ=float.Parse(Θ(Η,Α++));τ.л=Ŀ(Θ(Η,Α++));τ.н=Ŀ(Θ(Η,Α++));}else{ǲ=(О) int.Parse(Θ(Η,Α++));
Ǔ=(љ) int.Parse(Θ(Η,Α++));ĉ=(Ь) int.Parse(Θ(Η,Α++));ю=(х) int.Parse(Θ(Η,Α++));Ȏ=(О) int.Parse(Θ(Η,Α++));Ĩ=bool.Parse(Θ(Η,Α++));ǳ=bool.Parse(Θ(Η,Α++));ē=bool.Parse(Θ(Η,Α++));Ċ=bool.Parse(Θ(Η,Α++));С=int.Parse(Θ(Η,Α++));Р=int.Parse(Θ(Η,Α++));Э=int.Parse(Θ(Η,Α++));Ŵ=float.Parse(Θ(Η,Α++));
ų=float.Parse(Θ(Η,Α++));ĩ=float.Parse(Θ(Η,Α++));ď=float.Parse(Θ(Η,Α++));в=int.Parse(Θ(Η,Α++));б=int.Parse(Θ(Η,Α++));Ř=int.Parse(Θ(Η,Α++));ř=int.Parse(Θ(Η,Α++));ė=int.Parse(Θ(Η,Α++));к=int.Parse(Θ(Η,Α++));ΑΑ=int.Parse(Θ(Η,Α++));ǁ=float.Parse(Θ(Η,Α++));}Η=Θ(ǰ,3).Replace("\n","").Split('|');
ѕ=Η.ToList();Η=Θ(ǰ,4).Split('\n');υ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
υ.Add(ΔЀ(Θ(Η,i)));Η=Θ(ǰ,5).Split('|');Я=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ǭ)) ǭ=-1;if(Я.Length!=в*б){Я="";ǭ=-1;}Γυ(Θ(ǰ,6));}catch{return з.Αζ;}ΑΒ=Ǒ;ǝ();return з.ΒΙ;}const int Ζά=64000;void Δχ(){Ǣ.Clear();for(int i=Ї.Count-1;i>=0;i--){String Ξ=Ї[i].CustomData.ToUpper();
bool Ƹ=false;if(Ξ=="INSTRUCTIONS"){Ƹ=true;Βη=true;}if(Ξ=="DEBUG") Ƹ=true;if(Ƹ){Ǣ.Add(Ї[i]);Ї.RemoveAt(i);}}ȭ(Ǣ,false,1,false);}void Δψ(List<IMyTerminalBlock>ΒΨ){т.Clear();for(int i=0;i<ΒΨ.Count;i++){IMyTerminalBlock ά=ΒΨ[i];try{String ΓΑ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=ά.CustomName.IndexOf(ΓΑ);int ȣ=-1;if(Α<0||!int.TryParse(ά.CustomName.Substring(Α+ΓΑ.Length,1),out ȣ)) continue;if(ȣ==-1) continue;ȣ--;IMyTextSurfaceProvider ΓΓ=(IMyTextSurfaceProvider) ά;if(ȣ<ΓΓ.SurfaceCount&&ȣ>=0)
т.Add(ΓΓ.GetSurface(ȣ));}catch{}}}String ǃ="";void Δδ(){String ƿ="";String ć="";ǃ=Γβ(false);ƿ+=ǃ;ć+=ǃ;ć+=ΓЋ();for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(ƿ);for(int i=0;i<т.Count;i++) т[i].WriteText(ƿ);Echo(ć);for(int i=0;i<Ǣ.Count;i++){IMyTextPanel Ќ=Ǣ[i];String Ξ=Ќ.CustomData.ToUpper();
if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Αν());}}string Αν(){String Β="";try{float ΒІ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ȅ*ΒІ)+" / "+(int)(ȑ*ΒІ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ȅ*100f,1)+"% / "+Math.Round(ȑ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(ȳ,2)+"ms / "+Math.Round(ǂ,2)+"ms \n";}catch{}for(int i=0;i<ũ.Count;i++)
Β+=""+ũ.Keys.ElementAt(i)+" = "+Math.Round(ũ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ũ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΔΖ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ȭ(Ї,setLCDFontAndSize,0.9f,false);ȭ(т,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(Β+ȏ.ToString());for(int i=0;i<т.Count;i++) т[i].WriteText(Β+ȏ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ΒΒ(String γ,String ί,String ǥ){try{if(!Ă) return;γ=BROADCAST_TAG+BROADCAST_SEP+Ακ()+BROADCAST_SEP+ί+BROADCAST_SEP+Ƥ+BROADCAST_SEP+ǥ+BROADCAST_SEP+γ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,γ);}catch(Exception e){ȏ=e;}}String Ακ(){
if(Π!=Ν.А) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Αώ(String γ){return γ.StartsWith(BROADCAST_TAG);}bool Ђ(ref String Ξ,out string ċ,Char Ǳ){int Α=Ξ.IndexOf(Ǳ);ċ="";if(Α<0) return false;ċ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool Δΰ(ref String γ,out String Ǖ,out String ǥ)
{Ǖ="";ǥ="";if(!Ă) return false;String Щ="";if(!Ђ(ref γ,out Щ,BROADCAST_SEP)||!Αώ(Щ)) return false;if(!Ђ(ref γ,out Ǖ,BROADCAST_SEP)) return false;if(!Ђ(ref γ,out Щ,BROADCAST_SEP)||(Щ!=Ακ()&&(Щ!="*"&&Π!=Ν.А))) return false;if(!Ђ(ref γ,out Щ,BROADCAST_SEP)||(Щ!=Ƥ)) return false;
if(!Ђ(ref γ,out ǥ,BROADCAST_SEP)) return false;return true;}void ΔΜ(){bool Ą=true;if(Βπ){Βπ=false;if(Me.CustomData.Contains("Antenna_Name")){Ψ="Update custom data";Me.CustomData="";}}String Ď=(Π!=Ν.А?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ď)
Αϋ(Ď);String[]Η=Me.CustomData.Split('\n');Ă=bool.Parse(Ķ(İ(Η,"Enable_Broadcast",ref Ą)));bool ΑΧ=false;bool Βι=true;if(Ă){if(Π!=Ν.А)
Ğ=Ķ(İ(Η,"Ship_Name",ref Ą)).Replace(BROADCAST_SELF_ID,"");Ƥ=Ķ(İ(Η,"Broadcast_Channel",ref Ą)).ToLower();Βι=false;if(ы==null){ы=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ы.SetMessageCallback("");}List<IMyRadioAntenna>ȃ=new List<IMyRadioAntenna>();И.GetBlocksOfType(ȃ);bool Ơ=false;
for(int i=0;i<ȃ.Count;i++)
if(ȃ[i].EnableBroadcasting&&ȃ[i].Enabled){Ơ=true;break;}if(ȃ.Count==0) Ψ="No Antenna found";else if(!Ơ) Ψ="Antenna not ready";ΑΧ=ȃ.Count==0||!Ơ;if(ΒΠ&&!ΑΧ&&Π!=Ν.А) Ψ="Antenna ok";}else if(Π==Ν.А)
Ψ="Offline - Enable in PB custom data";ΒΠ=ΑΧ;if(Βι){if(ы!=null) this.IGC.DisableBroadcastListener(ы);ы=null;}}catch{Ą=false;}if(!Ą){Ψ="Reset custom data";Αϋ(Ď);}}void Αϋ(String Ď){String Ξ=Ď+"\n\n"+"Enable_Broadcast="+(Π==Ν.А?"true":"false")+" \n";Ξ+=Π!=Ν.А?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String Ķ(String Ǫ){int Α=Ǫ.IndexOf("//");if(Α!=-1) Ǫ=Ǫ.Substring(0,Α);String[]δ=Ǫ.Split('=');if(δ.Length<=1) return "";return δ[1].Trim();}String İ(String[]Η,String Ǐ,ref bool ȕ){foreach(String Ǫ in Η)
if(Ǫ.StartsWith(Ǐ)) return Ǫ;ȕ=false;return "";}void Γπ(string Ѝ){if(Č) ό=new List<ε>();String Ǖ="";String ǥ="";if(!Αώ(Ѝ)) Δε(Ѝ);bool ΓΜ=false;int Ες=50;while(ы!=null&&ы.HasPendingMessage&&Ες-->0){MyIGCMessage γ=ы.AcceptMessage();String Ξ=γ.Data as String;if(Ξ==null) continue;
if(Δΰ(ref Ξ,out Ǖ,out ǥ)&&Ǖ!=""&&Ǖ!=BROADCAST_SELF_ID){ε Γ=ƀ(Ǖ,"");if(Γ==null){Γ=new ε(Ǖ);ό.Add(Γ);}Γ.Ǜ(ǥ,false,false,true);ΔЊ(Γ,Ξ);ΓΜ=true;}}if(ΓΜ)
ό.Sort(delegate(ε a,ε b){if(a.θ==null&&b.θ==null) return 0;else if(a.θ==null) return-1;else if(b.θ==null) return 1;else return a.θ.CompareTo(b.θ);});if(Ĕ||Č){if(Ū<=0||Č){Ψ="";Č=false;Δν();ΔΜ();ΒЀ=DateTime.Now;}}if(ȍ) ΔЈ();if(ȍ) ΔЉ();if(ȍ) ΔЋ();ΔЇ();}void ΔЊ(ε Γ,String Ξ)
{Γ.ƺ=DateTime.Now;String ċ="";Ђ(ref Ξ,out Γ.ű,BROADCAST_SEP);Ђ(ref Ξ,out Γ.θ,BROADCAST_SEP);if(Γ.ű!=VERSION) return;String ě="";String[]Ș=new string[3];Ђ(ref Ξ,out ě,BROADCAST_SEP);Ђ(ref Ξ,out Ș[0],BROADCAST_SEP);Ђ(ref Ξ,out Ș[1],BROADCAST_SEP);Ђ(ref Ξ,out Ș[2],BROADCAST_SEP);
Ђ(ref Ξ,out Γ.ΑΝ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ψ,BROADCAST_SEP);Ђ(ref Ξ,out ċ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ť,BROADCAST_SEP);String ΓΨ="",ΒЈ="";Ђ(ref Ξ,out ΓΨ,BROADCAST_SEP);Ђ(ref Ξ,out ΒЈ,BROADCAST_SEP);Γ.я.Clear();while(true){String Βό;if(!Ђ(ref Ξ,out Βό,BROADCAST_SEP)) break;String[]ŝ=Βό.Split('/');
if(ŝ.Count()<3) continue;int І=0;if(!int.TryParse(ŝ[2],out І)) continue;Γ.я.Add(new ч(ŝ[0],ŝ[1],І,Ĳ.ΔΙ));}int ΒЊ=0;if(!int.TryParse(ċ,out Γ.ξ)) Γ.ξ=0;if(!int.TryParse(ě,out ΒЊ)) Γ.Π=Ν.ќ;if(!float.TryParse(Ș[0],out Γ.Υ.X)) Γ.Υ.X=0;if(!float.TryParse(Ș[1],out Γ.Υ.Y)) Γ.Υ.Y=0;
if(!float.TryParse(Ș[2],out Γ.Υ.Z)) Γ.Υ.Z=0;if(!float.TryParse(ΓΨ,out Γ.Ǡ)) Γ.Ǡ=0;if(!float.TryParse(ΒЈ,out Γ.ǫ)) Γ.ǫ=0;Γ.Π=(Ν) ΒЊ;Γ.Ƭ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Υ));Γ.ΑΤ=0;for(int i=0;i<Γ.я.Count();i++) Γ.ΑΤ+=Γ.я[i].І;}ε ƀ(String Ο,String θ){Ο=Ο.ToUpper();θ=θ.ToUpper();
for(int i=0;i<ό.Count;i++){if(Ο!=""&&ό[i].Ο.ToUpper()==Ο) return ό[i];if(θ!=""&&ό[i].θ!=null&&ό[i].θ.ToUpper()==θ) return ό[i];}return null;}String ȅ="";ε Βά=null;DateTime Βέ=DateTime.MinValue;List<ε>ǟ=new List<ε>();const float Γη=20f;bool ΒΔ(ε ί,String ρ){return ρ==ȅ&&ί==Βά&&(DateTime.Now-Βέ).TotalSeconds<Γη;
}String ΓЄ(ε ί){if(ȅ==""||!ΒΔ(ί,ȅ)) return "";String s="";int ȕ=0,ΑΖ=0;for(int i=0;i<ǟ.Count;i++){ζ Ŭ=ǟ[i].Ǜ(ȅ,false,false,false);if(Ŭ==ζ.Ȭ) ȕ++;if(ΑΖ++<8) s+="  "+ǟ[i].θ+": "+(Ŭ==ζ.Ȭ?"received":Ŭ==ζ.ǿ?"pending...":Ŭ==ζ.ĵ?"no answer!":"-")+"\n";
}if(ΑΖ>8) s+="  +"+(ΑΖ-8)+" more\n";return "--------------------------------------------\n"+ȅ+": "+ȕ+"/"+ǟ.Count+" received\n"+s;}void Н(ε Γ,String ρ){ȅ=ρ;Βά=Γ;Βέ=DateTime.Now;ǟ.Clear();if(Γ==null) ǟ.AddRange(ό);else ǟ.Add(Γ);if(Γ==null){for(int i=0;i<ό.Count;i++)
ό[i].Ǜ(ρ,true,false,false);ΒΒ(ρ,"*",ρ);}else{Γ.Ǜ(ρ,true,false,false);ΒΒ(ρ,Γ.Ο,ρ);}}void Δε(string Ѝ){if(Ѝ=="") return;var δ=Ѝ.ToUpper().Split(' ');δ.DefaultIfEmpty("");var ρ=δ.ElementAtOrDefault(0);var ΒΣ=δ.ElementAtOrDefault(1);String Εε="Invalid argument: "+Ѝ;if(ή==Ι.Ȯ){if(ο!=null)
{switch(ρ){case "UP":{if(ξ<2) break;if(ο.ξ==0){ξ=1;return;}Н(ο,"UP");return;}case "DOWN":{if(ξ<1) break;if(ξ==1){Н(ο,"MRES");break;}Н(ο,"DOWN");break;}case "APPLY":{if(ξ<2) break;Н(ο,"APPLY");return;}}}}switch(ρ){case "UP":this.ΔΥ(false);return;case "DOWN":this.ΔΤ(false);return;
case "APPLY":this.ǻ(true);return;}switch(ρ){case "CLEAR":Γν();return;case "SENDALL":
if(String.IsNullOrEmpty(ΒΣ)){Ψ="Usage: SENDALL <cmd>";return;}Н(null,ΒΣ);return;case "SEND":
if(Ѝ.Length<="SEND".Length+1){Ψ="Usage: SEND <ship>:<cmd>";return;}Δο(Ѝ.Remove(0,"SEND".Length+1));return;}Ψ=Εε;}void Δο(String ΒΡ){if(ΒΡ=="") return;var δ=ΒΡ.Split(':');if(δ.Length!=2){Ψ="Missing separator \":\"";return;}δ.DefaultIfEmpty("");String Ğ=δ.ElementAtOrDefault(0).Trim();
ε Γ=ƀ("",Ğ);if(Γ!=null) Н(Γ,δ.ElementAtOrDefault(1).Trim());else Ψ="Unknown ship: "+Ğ;}void Γν(){ό.Clear();ο=null;Χ(Ι.Main);ξ=0;}void Δν(){List<IMyTerminalBlock>ΓΞ=new List<IMyTerminalBlock>();И.GetBlocksOfType(Ї,Ɗ);И.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΓΞ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ї=Αη(Ї,pamTag,true);Δψ(ΓΞ);Δχ();Δξ();ȭ(Ї,setLCDFontAndSize,1.15f,false);ȭ(т,setLCDFontAndSize,1.15f,true);String Εα=Αξ(false);String ΒЍ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ќ in Ї){if(Ќ.CustomData==""){Ќ.CustomData=Αξ(true);continue;}
if(!Ќ.CustomData.Contains(Εα))
if(!Ќ.CustomData.Contains(ΒЍ)) Ќ.CustomData=ΒЍ+Ќ.CustomData;}}void ΔЇ(){String ć="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<т.Count;i++)
т[i].WriteText(Αί(φ.ΑЁ,"0",false));for(int i=0;i<Ї.Count;i++){φ η=φ.ќ;String Ш="";Δα(Ї[i],out η,out Ш);Ї[i].WriteText(Αί(η,Ш,i==0));}Echo(ć);for(int i=0;i<Ǣ.Count;i++){IMyTextPanel Ќ=Ǣ[i];String Ξ=Ќ.CustomData.ToUpper();if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Αν());
}}String ǻ(bool Ε){int ΕΣ=0;return ǻ(Ε,0,ref ΕΣ,false,1);}String ǻ(bool Ε,int Εϊ,ref int ΕЇ,bool Βΰ,int Ĝ){String Β="";String ȡ="——————————————————\n";String ΖΓ="--------------------------------------------\n";if(ο==null||ή==Ι.Main||ή==Ι.Ɩ)
Β+="[PAM]-Controller | "+ό.Count+" Connected ships"+"\n"+ΓЈ()+ΓЎ()+ΔΓ();else Β+=Αρ(ο)+"\n";Β+=ȡ;int Α=0;int Δ=ξ;Ǭ=0;if(Ψ!=""){Ζ(ref Β,Δ,Α++,Ε,Ψ);}else if(ή==Ι.Main){if(ο==null&&ό.Count>=1) ο=ό[0];String Ѕ="";if(Ζ(ref Ѕ,Δ,Α++,Ε," Send command to all")) Χ(Ι.Ɩ);
int Ƞ=0;for(int s=0;s<ό.Count;s++){if(Βΰ) Ѕ+="\n";ε Γ=ό[s];if(Δ==Α||Δ==Α+1) ο=Γ;if(Α==Δ) Ƞ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+Αρ(Γ))){ο=Γ;Χ(Ι.Ȯ);ο.Ǜ("",false,true,false);}if(Α==Δ) Ƞ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+(Βΰ?"":"  ")+ΓА(Γ)))
{ο=Γ;Χ(Ι.ΒΓ);}}int ȧ=Ĝ-2;ȧ+=Math.Max(0,(Εϊ-1))*Ĝ;Β+=ƕ(ȧ,Ѕ,Ƞ,ref ΕЇ);}else if(ή==Ι.ķ||ή==Ι.Ɩ){Ι ΕΒ=ή==Ι.ķ?Ι.Ȯ:Ι.Main;ε ί=null;if(ή==Ι.ķ) ί=ο;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(ΕΒ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǽ(ί,"STOP"))){Н(ί,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǽ(ί,"CONT"))){Н(ί,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǽ(ί,"HOMEPOS"))){Н(ί,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǽ(ί,"JOBPOS"))){Н(ί,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǽ(ί,"FULL"))){Н(ί,"FULL");}if(ί!=null&&ί.Π==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǽ(ί,"UNDOCK"))){Н(ί,"UNDOCK");}Β+=ΓЄ(ί);}else if(ή==Ι.Ȯ){String Ƣ="";if(Ζ(ref Ƣ,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=Ƣ.Substring(0,Ƣ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Χ(Ι.ķ);}if(!ο.ΑЀ()&&!ο.ƈ()) Ǭ++;if(ο.ƈ()) Β+="No answer received...";
else Β+=ȁ(ο,ο.Ť);}else if(ή==Ι.ΒΓ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=ΖΓ;Β+=ȁ(ο,Απ(ο));}if(!Ǆ) ξ=Math.Min(Ǭ-1,ξ);return Β;}String Αρ(ε Γ){if(Γ.ű!=VERSION) return Γ.θ+": Different version!";return Γ.θ+": "+ȁ(Γ,Γ.ΑΝ)+" "+Γ.Ƭ+"m";}String ΓА(ε Γ){String s=Ɠ("",М(Γ.Ǡ,Γ.ǫ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.я.Count;i++){if(i>=5) break;ч м=Γ.я[i];s+=ΒΞ(ź(ΓЌ(м)),3)+" "+Αθ(м.І)+" ";}return s;}String ΓЌ(ч м){if(м.Њ.ToUpper()=="ORE"||м.Њ.ToUpper()=="INGOT"){String Βή=ΓЊ(м.θ.ToUpper());if(Βή!="") return Βή;}return м.θ;}String ȁ(ε Γ,String Β){if(Γ.ΑЀ()) return "No signal...("+ƃ((int)(DateTime.Now-Γ.ƺ).TotalSeconds)+")";
return Β;}String Αξ(bool ΖΥ){String Β="";if(ΖΥ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Δα(IMyTerminalBlock Ќ,out φ η,out String Ш){bool ȕ=true;String ě=Ķ(İ(Ќ.CustomData.Split('\n'),"mode",ref ȕ)).ToUpper();String[]δ=ě.Split(':');String Ě=δ.Length>0?δ[0].Trim():"";Ш=δ.Length>1?δ[1].Trim():"";
η=φ.ќ;if(Ě=="MAIN") η=φ.ΑЂ;else if(Ě=="MAINX") η=φ.ΑЃ;else if(Ě=="MENU") η=φ.ƍ;else if(Ě=="INVENTORY") η=φ.Ƈ;else if(Ě=="DEBUG") η=φ.Ž;if(η==φ.ќ) Ш=ě;}String Αί(φ η,String Ш,bool ΕΦ){int Ę=15;if(ΕΦ){Βα=ň;ň=0;Ββ=ŉ;ŉ=0;}String Β="";if(η==φ.ΑЂ){int Ћ=0;
Β=ǻ(false,Βα,ref ΕΜ,true,Ę);if(!int.TryParse(Ш,out Ћ)) return Β;ň=Math.Max(Ћ,ň);Ћ--;return ΑЎ(Ћ,Ę,Β);}else if(η==φ.ΑЃ){int Ћ=0;Β=ǻ(false,Ββ,ref ΕΝ,false,Ę);if(!int.TryParse(Ш,out Ћ)) return Β;ŉ=Math.Max(Ћ,ŉ);Ћ--;return ΑЎ(Ћ,Ę,Β);}if(η==φ.ΑЁ){return ǻ(false,0,ref ΕΞ,true,Ę);
}else if(η==φ.Ƈ||η==φ.ƍ||η==φ.Ž){ε Γ=ο;if(Ш!=""){Γ=ƀ("",Ш);if(Γ==null) return "Unknown ship: "+Ш;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ȡ="—————————————————————\n";String ΑΡ="";String ƣ="";if(η==φ.ƍ){ΑΡ="Menu";ƣ=ȁ(Γ,Γ.Ť);}
else if(η==φ.Ƈ){ΑΡ="Inventory";ƣ=ȁ(Γ,Απ(Γ));}else if(η==φ.Ž){return ȁ(Γ,ΓБ(Γ));}Β+=Γ.θ+" - "+Γ.Ƭ+"m | "+ź(""+ΑΡ)+"\n"+ȡ;Β+=ƣ;return Β;}return "Unknown command: "+Ш;}string ΓБ(ε Γ){String s="";s+=Γ.ű+"\n";s+=Γ.θ+"\n";s+=Γ.Υ.X+"\n";s+=Γ.Υ.Y+"\n";
s+=Γ.Υ.Z+"\n";s+=Γ.ΑΝ+"\n";s+=Γ.Ψ+"\n";s+=Γ.ξ+"\n";s+=Γ.Ť.Length+"\n";s+=Γ.Ǡ+"\n";s+=Γ.ǫ+"\n";s+=Γ.я.Count()+"\n";return s;}string Απ(ε Γ){String s="";s+=Ɠ("All",М(Γ.Ǡ,Γ.ǫ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.я.Count();i++){ч м=Γ.я[i];
s+=Ɠ(м.θ,м.І,Γ.ΑΤ,30,8,10)+"\n";}return s;}ζ ΓЅ(ε Γ,bool ь,string ρ){ζ Ϊ=ζ.None;if(Γ==null){for(int i=0;i<ό.Count;i++){ζ ώ=ό[i].Ǜ(ρ,false,ь,false);if(ώ==ζ.Ȭ) Ϊ=ώ;if(ώ==ζ.ĵ) return ώ;if(ώ==ζ.ǿ) return ώ;}return Ϊ;}else return Γ.Ǜ(ρ,false,ь,false);}String ǽ(ε Γ,String ρ)
{if(!ΒΔ(Γ,ρ)) return "";ζ ώ=ΓЅ(Γ,false,ρ);if(ώ==ζ.Ȭ) return "received!";if(ώ==ζ.ǿ) return "pending...";if(ώ==ζ.ĵ) return "no answer!";return "";}String ƕ(int ȧ,String Β,int Ƞ,ref int ё){String[]Η=Β.Split('\n');if(Ƞ>=ё+ȧ-1) ё++;ё=Math.Min(Η.Count()-1-ȧ,ё);if(Ƞ<ё+1) ё--;
ё=Math.Max(0,ё);String Ϊ="";for(int i=0;i<ȧ;i++){int ΒЃ=i+ё;if(ΒЃ>=Η.Count()) break;Ϊ+=Η[ΒЃ]+"\n";}return Ϊ;}String ΑЎ(int Ћ,int Ĝ,String Β){String[]Η=Β.Split('\n');int ΑΜ=Ћ*Ĝ;int œ=(Ћ+1)*Ĝ;String Ϊ="";for(int i=ΑΜ;i<œ;i++){if(i>=Η.Count()) break;Ϊ+=Η[i]+"\n";
}return Ϊ;}String ź(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΒΞ(String s,int ΒЇ){if(s=="") return s;if(s.Length>ΒЇ) s=s.Substring(0,ΒЇ-1)+".";return s;}string Ɠ(String Ǩ,float value,float Š,int ΒΤ,int ΒϏ,int Εω){float ǯ=М(value,Š)*ΒΤ;
String Ń="[";for(int i=0;i<ΒΤ;i++){if(i<=ǯ) Ń+="|";else Ń+="'";}Ń+="]";return Ń+" "+ΒΞ(ź(Ǩ),ΒϏ).PadRight(ΒϏ)+"".PadRight(Εω)+Αθ(value);}String Αθ(float І){if(І>=1000000) return Math.Round(І/1000000f,І/1000000f<100?1:0)+"M";if(І>=1000) return Math.Round(І/1000f,І/1000f<100?1:0)+"K";
return ""+Math.Round(І);}String ƃ(int ĝ){if(ĝ>=60*60) return Math.Round(ĝ/(60f*60f),1)+" h";if(ĝ>=60) return Math.Round(ĝ/60f,1)+" min";return ""+ĝ+" s";}List<IMyCameraBlock>Ζσ=new List<IMyCameraBlock>();List<long>ΗΖ=new List<long>();bool Εϋ=false;int ΕΗ=0;int Ζς=0;
DateTime Ζπ=DateTime.Now;String ΕΕ="";void Γμ(){Εϋ=false;ΕΗ=0;ΕΕ="";}const String ΔЄ="[PAMTFC]";const float Ζζ=5f;const float Ζί=2f;class ΒΛ{public long Ο;public Vector3 β,ǘ;public Vector3 Ŗ;public float Ζώ;public String Μ="";public int ȋ=-1;public bool ΗΜ;public bool Ē;public float ŕ,Ŕ;
public long У=0;public int ΖЃ=0;public DateTime ģ;}IMyBroadcastListener Ηζ=null;long Εσ=0;List<ΒΛ>Ηβ=new List<ΒΛ>();Vector3 Ηθ=new Vector3();float Ηι=-1;Vector3 Ηγ=new Vector3();bool Ηε=false;DateTime Ηδ=DateTime.Now;const float Ζε=15f;const float Ζδ=10f;int Ηκ=0;String Я="";
int ǭ=-1;bool Ζχ=false,Ζυ=false;Vector3 Ζφ=new Vector3();Vector3 Ηη=new Vector3();IMyBroadcastListener Ηέ=null;float Ηά=-1;DateTime Ηή=DateTime.MinValue;List<int>ƶ=new List<int>();String Βψ="";void Γϋ(){String Ǐ=в+"|"+б+"|"+(int) Ȏ;if(Ǐ==Βψ) return;Βψ=Ǐ;Αΰ(в,б,Ȏ,ƶ);
}void Αΰ(int w,int h,О ΑΜ,List<int>Ƨ){Ƨ.Clear();if(ΑΜ==О.Ľ){for(int k=0;k<w*h;k++){int ȝ=k/w;Ƨ.Add(ȝ*w+(ȝ%2==0?k-ȝ*w:w-1-(k-ȝ*w)));}return;}int Βχ=(int) Math.Ceiling(w/2f),Βφ=(int) Math.Ceiling(h/2f);int Εί=w/2,Εή=h/2;int x=0,y=0,ƭ=0,Ʈ=-1,Š=Math.Max(w,h);
for(int i=0;i<Š*Š;i++){if(-Βχ<x&&x<=Εί&&-Βφ<y&&y<=Εή) Ƨ.Add((y-1+Βφ)*w+(x-1+Βχ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ƭ;ƭ=-Ʈ;Ʈ=t;}x+=ƭ;y+=Ʈ;}}bool Γϊ(){int n=Math.Max(0,в*б);if(Я.Length==n) return false;Я=new String('0',n);
ǭ=-1;return true;}void Δυ(int Б,bool ψ){if(Б<0||Б>=Я.Length) return;char[]h=Я.ToCharArray();h[Б]=ψ?'1':'0';Я=new String(h);}const String ΔΡ="[PAMJOB]";const String ΔЂ="[PAMTCC]";IMyBroadcastListener Ζω=null;DateTime Ζϊ=DateTime.MinValue;DateTime Ζϋ=DateTime.Now;int Ζψ=-1;
bool ΖЊ=false;class Ȁ{public long Ο;public Vector3 β,З,Ё;}List<Ȁ>Ў=new List<Ȁ>();bool Ć=false;Vector3 Ő,ŏ,ő,Œ;long Đ=0;long ΒБ=0;Ω ΒА=null;bool ΕΠ=false;Ω Γχ(Vector3 β,Vector3 З,Vector3 Ё){MatrixD m=MatrixD.CreateWorld(β,З,Ё);Ω w=new Ω(Ρ);w.Є=Vector3D.TransformNormal(Ő,m);
w.Ϗ=Vector3D.TransformNormal(ŏ,m);w.Й=Vector3D.TransformNormal(ő,m);w.Υ=β+(Vector3) Vector3D.TransformNormal(Œ,m);w.ι=true;return w;}void ΔΣ(){if(Π==Ν.λ||!Ρ.ι) return;IMyShipConnector э=Ǽ(MyShipConnectorStatus.Connected);if(э==null||э.OtherConnector==null) return;IMyShipConnector Г=э.OtherConnector;
Vector3I Εβ=new Vector3I((int) Ρ.Т.X,(int) Ρ.Т.Y,(int) Ρ.Т.Z);if(э.Position==Εβ){MatrixD Ŝ=MatrixD.Transpose(Г.WorldMatrix);Ő=Vector3D.TransformNormal(Ј.WorldMatrix.Forward,Ŝ);ŏ=Vector3D.TransformNormal(Ј.WorldMatrix.Down,Ŝ);ő=Vector3D.TransformNormal(Ј.WorldMatrix.Left,Ŝ);Œ=Vector3D.TransformNormal(э.GetPosition()-Г.GetPosition(),Ŝ);
Ć=true;}if(Vector3.Distance(э.GetPosition(),Ρ.Υ)<5) Đ=Г.EntityId;List<IMyShipConnector>Ǧ=new List<IMyShipConnector>();И.GetBlocksOfType(Ǧ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(Ǧ.Count>0){Ў.Clear();for(int i=0;i<Ǧ.Count;i++){Ȁ p=new Ȁ();
p.Ο=Ǧ[i].EntityId;p.β=Ǧ[i].GetPosition();p.З=Ǧ[i].WorldMatrix.Forward;p.Ё=Ǧ[i].WorldMatrix.Up;Ў.Add(p);}}ΒА=null;ΒБ=0;if(Г.EntityId==Đ||!Ć) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].Ο==Г.EntityId){ΒБ=Ў[i].Ο;ΒА=Γχ(Ў[i].β,Ў[i].З,Ў[i].Ё);}}String Γφ(){String s=(Ć?"1":"0")+":"+έ(Ő)+":"+έ(ŏ)+":"+
έ(ő)+":"+έ(Œ)+":"+Đ;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].Ο+","+έ(Ў[i].β)+","+έ(Ў[i].З)+","+έ(Ў[i].Ё);return s;}void Γυ(String s){Ў.Clear();Ć=false;Đ=0;String[]f=s.Split(':');if(f.Length<6) return;Ć=f[0]=="1";Ő=α(f[1]);ŏ=α(f[2]);ő=α(f[3]);Œ=α(f[4]);long.TryParse(f[5],out Đ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ȁ p=new Ȁ();if(!long.TryParse(e[0],out p.Ο)) continue;p.β=α(e[1]);p.З=α(e[2]);p.Ё=α(e[3]);Ў.Add(p);}}const String ż="[PAMDCK]";const float Γζ=120f;class ц{public long Γ;public int ǐ;public long а;public bool ΒΩ;public DateTime Ж,Ė;
}IMyBroadcastListener Ŏ=null;List<ц>χ=new List<ц>();List<IMyShipConnector>ђ=new List<IMyShipConnector>();List<IMySensorBlock>ф=new List<IMySensorBlock>();List<IMyCameraBlock>ĕ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ŵ=new List<MyDetectedEntityInfo>();long Д=0;int ƨ=0;
long ΒΪ=0;DateTime ΒΫ=DateTime.Now;String ǣ="";int Βμ=0;IMyCubeGrid ѓ=null;void Δξ(){ѓ=Me.CubeGrid;И.GetBlocksOfType(ђ,q=>q.CubeGrid.IsSameConstructAs(ѓ)&&q.CustomName.Contains(dockPoolTag));И.GetBlocksOfType(ф,q=>q.CubeGrid.IsSameConstructAs(ѓ)&&q.CustomName.Contains(zoneTag));
И.GetBlocksOfType(ĕ,q=>q.CubeGrid.IsSameConstructAs(ѓ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ф.Count;i++){ф[i].DetectSmallShips=true;ф[i].DetectLargeShips=true;}for(int i=0;i<ĕ.Count;i++) ĕ[i].EnableRaycast=true;}IMyShipConnector ΒΘ(long Ο){if(Ο==0) return null;
IMyShipConnector c=И.GetBlockWithId(Ο) as IMyShipConnector;return c!=null&&ѓ!=null&&c.CubeGrid.IsSameConstructAs(ѓ)?c:null;}bool Ɖ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΑϏ(IMyShipConnector c,long Γ){for(int i=0;i<ĕ.Count;i++)
{IMyCameraBlock ă=ĕ[i];if(!ă.IsWorking||Vector3D.Distance(ă.GetPosition(),c.GetPosition())>10) continue;Vector3D œ=ă.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ă.CanScan(œ)) continue;MyDetectedEntityInfo ŗ=ă.Raycast(œ);if(!ŗ.IsEmpty()&&ŗ.EntityId!=ѓ.EntityId&&ŗ.EntityId!=Γ&&ŗ.Type!=MyDetectedEntityType.FloatingObject)
{ǣ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΔΟ(){for(int s=0;s<ф.Count;s++){if(!ф[s].IsWorking) continue;ŵ.Clear();ф[s].DetectedEntities(ŵ);for(int i=0;i<ŵ.Count;i++){MyDetectedEntityInfo e=ŵ[i];bool ΕΫ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΕΫ&&e.EntityId!=Д&&e.Velocity.Length()>0.5f){ǣ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Δγ(ц r){IMyShipConnector а=ΒΘ(r.а);if(Ɖ(а)&&ΑϏ(а,r.Γ)) return а.EntityId;if(!r.ΒΩ||а==null) return 0;IMyShipConnector ń=null;double ΒΧ=0;for(int i=0;i<ђ.Count;i++)
{IMyShipConnector c=ђ[i];if(!Ɖ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),а.GetPosition());if(ń==null||d<ΒΧ){if(ΑϏ(c,r.Γ)){ń=c;ΒΧ=d;}}}return ń==null?0:ń.EntityId;}void ΔΔ(){ц Ȗ=null;long э=0;for(int Ũ=2;Ũ>=1&&Ȗ==null;Ũ--){ц Ǯ=null;for(int i=0;i<χ.Count;i++)
{ц r=χ[i];if(r.ǐ!=Ũ) continue;if(Ũ==1){long c=Δγ(r);if(c==0) continue;if(Ǯ==null||r.Ж<Ǯ.Ж){Ǯ=r;э=c;}}else if(Ǯ==null||r.Ж<Ǯ.Ж) Ǯ=r;}Ȗ=Ǯ;}if(Ȗ==null){if(χ.Count>0&&ǣ=="") ǣ="No free dock";return;}Д=Ȗ.Γ;ƨ=Ȗ.ǐ;ΒΪ=Ȗ.ǐ==1?э:0;ΒΫ=DateTime.Now;
}void ΔЈ(){if(ѓ==null) return;if(Ŏ==null) Ŏ=IGC.RegisterBroadcastListener(ż);int ƥ=30;while(Ŏ.HasPendingMessage&&ƥ-->0){String Ξ=Ŏ.AcceptMessage().Data as String;if(Ξ!=null) ΔΕ(Ξ);}Δλ();if(++Βμ<3) return;Βμ=0;IGC.SendBroadcastMessage(ż,Γδ());}void ΔΕ(String Ξ){try{String[]f=Ξ.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=χ.Count-1;i>=0;i--) if(χ[i].Γ==Γ) χ.RemoveAt(i);if(Д==Γ) Д=0;}else if(f[0]=="R"&&f.Length>=5){long а=long.Parse(f[3]);if(ΒΘ(а)==null) return;ц r=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Γ) r=χ[i];if(r==null){r=new ц();r.Γ=Γ;r.Ж=DateTime.Now;χ.Add(r);}
int ǐ=int.Parse(f[2]);if(r.ǐ!=ǐ) r.Ж=DateTime.Now;r.ǐ=ǐ;r.а=а;r.ΒΩ=f[4]=="1";r.Ė=DateTime.Now;}}catch{}}void Δλ(){for(int i=χ.Count-1;i>=0;i--)
if((DateTime.Now-χ[i].Ė).TotalSeconds>3) χ.RemoveAt(i);ц ņ=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Д) ņ=χ[i];if(Д!=0&&ņ==null) Д=0;if(ņ!=null&&(DateTime.Now-ΒΫ).TotalSeconds>Γζ){Д=0;ņ.Ж=DateTime.Now;}ǣ="";if(Д==0&&χ.Count>0&&!ΔΟ()) ΔΔ();}String Γδ()
{String γ="S;"+ѓ.EntityId+";"+Д+";"+ƨ+";"+ΒΪ+";";for(int i=0;i<χ.Count;i++) γ+=(i>0?",":"")+χ[i].Γ;γ+=";";for(int i=0;i<ђ.Count;i++){IMyShipConnector c=ђ[i];γ+=(i>0?":":"")+c.EntityId+","+έ(c.GetPosition())+","+έ(c.WorldMatrix.Forward)+","+έ(c.WorldMatrix.Up);
}γ+=";";for(int i=0;i<ф.Count;i++){IMySensorBlock z=ф[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();γ+=(i>0?":":"")+έ(z.GetPosition())+","+Math.Round(r,1);}return γ;}String ΓЈ()
{if(ђ.Count==0&&ф.Count==0) return "";int Βρ=0;for(int i=0;i<ђ.Count;i++) if(Ɖ(ђ[i])) Βρ++;return "Docks: "+Βρ+"/"+ђ.Count+" free"+(Д!=0?(ƨ==1?" | docking":" | undocking"):"")+
(χ.Count>1?" | "+(χ.Count-1)+" waiting":"")+(ǣ!=""?"\n"+ǣ:"")+"\n";}IMyBroadcastListener ΗΥ=null;int Βλ=0;DateTime ΖЅ=DateTime.Now;DateTime ΗΠ=DateTime.MinValue;long ΗΡ=0;int ΗΣ=0;long ΗΤ=0;bool ΖΞ=false;int ΖІ=0;List<Vector3>Ησ=new List<Vector3>();List<float>Ητ=new List<float>();
bool ΖЄ=false;bool ΖЂ=false;Vector3 ΖЁ=new Vector3();void Δζ(){if(Βλ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ż,"D;"+Εσ);Βλ=0;ΖΞ=false;}const float ΔΠ=600f;class ш{public long Ο;public int Б=-1;public bool ΓΩ;public float ŕ,Ŕ;public DateTime ΑΓ;}class и{public String Ǐ;public int w,h;
public float ΖΛ,ΖΒ;public String ψ;public List<int>Ǉ=new List<int>();public List<ш>Љ=new List<ш>();}List<и>г=new List<и>();IMyBroadcastListener ş=null;int Βώ=0;void ΔЉ(){if(ş==null) ş=IGC.RegisterBroadcastListener(ΔЄ);int ƥ=50;while(ş.HasPendingMessage&&ƥ-->0){String Ξ=ş.AcceptMessage().Data as String;
if(Ξ==null) continue;try{ΔΗ(Ξ);}catch{}try{ΔΘ(Ξ);}catch{}}for(int j=г.Count-1;j>=0;j--){и Μ=г[j];for(int i=Μ.Љ.Count-1;i>=0;i--)
if((DateTime.Now-Μ.Љ[i].ΑΓ).TotalSeconds>ΔΠ) Μ.Љ.RemoveAt(i);if(Μ.Љ.Count==0){г.RemoveAt(j);continue;}Γί(Μ);}if(++Βώ<3) return;Βώ=0;for(int j=0;j<г.Count;j++) IGC.SendBroadcastMessage(ΔΡ,Γα(г[j]));}void Γί(и Μ){for(int i=0;i<Μ.Љ.Count;i++){ш s=Μ.Љ[i];if(s.ΓΩ&&(s.Б<0||Μ.ψ[s.Б]=='1')) s.Б=ΔΦ(Μ,s);
}}String Γα(и Μ){String γ="J;"+Μ.Ǐ+";"+Μ.ψ+";";bool Ж=true;for(int i=0;i<Μ.Љ.Count;i++){if((DateTime.Now-Μ.Љ[i].ΑΓ).TotalSeconds>5) continue;γ+=(Ж?"":",")+Μ.Љ[i].Ο+":"+Μ.Љ[i].Б;Ж=false;}return γ;}void ΔΗ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;и Μ=null;for(int i=0;i<г.Count;i++) if(г[i].Ǐ==f[4]) Μ=г[i];if(Μ==null){Μ=new и();Μ.Ǐ=f[4];Μ.w=int.Parse(k[3]);Μ.h=int.Parse(k[4]);Μ.ΖΛ=float.Parse(k[6]);Μ.ΖΒ=float.Parse(k[7]);if(Μ.w<1||Μ.h<1) return;Αΰ(Μ.w,Μ.h,(О) int.Parse(k[5]),Μ.Ǉ);
Μ.ψ=new String('0',Μ.w*Μ.h);г.Add(Μ);}String d=f[6];if(d.Length==Μ.ψ.Length){char[]c=Μ.ψ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Μ.ψ=new String(c);}long Ο=long.Parse(f[0]);ш s=null;for(int i=0;i<Μ.Љ.Count;i++) if(Μ.Љ[i].Ο==Ο) s=Μ.Љ[i];
if(s==null){s=new ш();s.Ο=Ο;Μ.Љ.Add(s);}s.ΑΓ=DateTime.Now;String[]w=f[10].Split(',');s.ΓΩ=w[0]=="1";if(w.Length>=3){s.ŕ=float.Parse(w[1]);s.Ŕ=float.Parse(w[2]);}int ȋ=int.Parse(f[5]);if(ȋ>=0&&ȋ<Μ.ψ.Length&&(s.Б<0||Μ.ψ[s.Б]=='1')&&Μ.ψ[ȋ]!='1'&&Αψ(Μ,ȋ,s)==null)
s.Б=ȋ;}ш Αψ(и Μ,int Б,ш ΕΥ){for(int i=0;i<Μ.Љ.Count;i++){ш o=Μ.Љ[i];if(o!=ΕΥ&&o.Б==Б)
return o;}return null;}int ΔΦ(и Μ,ш s){for(int k=0;k<Μ.Ǉ.Count;k++){int c=Μ.Ǉ[k];if(Μ.ψ[c]!='1'&&Αψ(Μ,c,s)==null) return c;}return-2;}String ΓЎ(){String s="";for(int j=0;j<г.Count;j++){и Μ=г[j];int ψ=0;for(int i=0;i<Μ.ψ.Length;i++) if(Μ.ψ[i]=='1') ψ++;s+="Job "+(j+1)+": "+ψ+"/"+Μ.ψ.Length+" holes, "+Μ.Љ.Count+" ships\n";
}return s;}const float ΔЁ=8f;class й{public long Ο;public Vector3 β,ǘ,Ŗ;public float ƫ,ŕ,Ŕ,ǩ=-1;public bool Ē;public String Μ="";public int ΒЏ;public DateTime ģ;}class ļ{public long a,b,ΓΪ;public DateTime Ė;}List<й>ϋ=new List<й>();List<ļ>с=new List<ļ>();int ΓΡ=0;void ΔΘ(String Ξ)
{String[]f=Ξ.Split(';');if(f.Length<9) return;long Ο=long.Parse(f[0]);й s=null;for(int i=0;i<ϋ.Count;i++) if(ϋ[i].Ο==Ο) s=ϋ[i];if(s==null){s=new й();s.Ο=Ο;ϋ.Add(s);}s.β=α(f[1]);s.ǘ=α(f[2]);s.ƫ=float.Parse(f[3]);s.Μ=f[4];s.ΒЏ=int.Parse(f[7].Split(',')[1]);s.Ŗ=α(f[8]);
s.Ē=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ŕ=float.Parse(w[1]);s.Ŕ=float.Parse(w[2]);s.Ē=w[3]=="1";}}s.ģ=DateTime.Now;}bool Ɯ(й s){return s.ǘ.LengthSquared()<0.25f&&(s.ΒЏ==3||s.Ŗ.LengthSquared()<0.25f);}Vector3 ΒΚ(й s){return s.ǘ.LengthSquared()>=1f?s.ǘ:s.Ŗ*2f;}
void ΔЋ(){Γξ();if(++ΓΡ<3) return;ΓΡ=0;IGC.SendBroadcastMessage(ΔЂ,Γε());}void Γξ(){for(int i=ϋ.Count-1;i>=0;i--)
if((DateTime.Now-ϋ[i].ģ).TotalSeconds>3) ϋ.RemoveAt(i);for(int i=с.Count-1;i>=0;i--)
if((DateTime.Now-с[i].Ė).TotalSeconds>2) с.RemoveAt(i);for(int i=0;i<ϋ.Count;i++) ϋ[i].ǩ=-1;for(int i=0;i<ϋ.Count;i++)
for(int j=i+1;j<ϋ.Count;j++){й a=ϋ[i],b=ϋ[j];if(Ɯ(a)||Ɯ(b)) continue;Vector3 Εψ=a.β+a.ǘ*(float)(DateTime.Now-a.ģ).TotalSeconds;Vector3 Εύ=b.β+b.ǘ*(float)(DateTime.Now-b.ģ).TotalSeconds;Vector3 ȥ=ΒΚ(a),Ȧ=ΒΚ(b);Vector3 Ț=Εύ-Εψ;float В=Ț.Length();float ь=(a.ƫ+b.ƫ)*0.55f;
if(a.Ē&&b.Ē&&a.Μ!=""&&a.Μ==b.Μ){String[]k=a.Μ.Split('|');if(k.Length>=8) ь=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ǌ=Ȧ-ȥ;float ΓΗ=ǌ.LengthSquared();float t=ΓΗ>0.01f?-Vector3.Dot(Ț,ǌ)/ΓΗ:0;bool ΑΣ=В<ь;if(!ΑΣ&&(t<=0||t>ΔЁ||(Ț+ǌ*t).Length()>=ь)) continue;
if(!ΑΣ&&ȥ.LengthSquared()>0.25f&&Ȧ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ȥ),Vector3.Normalize(Ȧ))<-0.7f)
continue;ļ p=null;for(int k=0;k<с.Count;k++)
if((с[k].a==a.Ο&&с[k].b==b.Ο)||(с[k].a==b.Ο&&с[k].b==a.Ο)) p=с[k];if(p==null){p=new ļ();p.a=a.Ο;p.b=b.Ο;p.ΓΪ=ΔЃ(a,b,ȥ,Ȧ,Ț,В);с.Add(p);}p.Ė=DateTime.Now;й y=p.ΓΪ==a.Ο?a:b;float ΒЂ=ΑΣ||t<4?0:Math.Max(1,y.ǘ.Length()*0.5f);if(y.ǩ<0||ΒЂ<y.ǩ) y.ǩ=ΒЂ;
}}long ΔЃ(й a,й b,Vector3 ȥ,Vector3 Ȧ,Vector3 Ț,float В){if(a.Ο==Д) return b.Ο;if(b.Ο==Д) return a.Ο;Vector3 n=В>0.01f?Ț/В:new Vector3(1,0,0);float ΓΝ=Vector3.Dot(ȥ,n),ΓΠ=Vector3.Dot(Ȧ,-n);if(ΓΝ-ΓΠ>1) return a.Ο;if(ΓΠ-ΓΝ>1) return b.Ο;return a.Ο>b.Ο?a.Ο:b.Ο;
}String Γε(){String γ="C;";bool Ж=true;for(int i=0;i<ϋ.Count;i++){if(ϋ[i].ǩ<0) continue;γ+=(Ж?"":",")+ϋ[i].Ο+":"+Math.Round(ϋ[i].ǩ,1);Ж=false;}return γ;}String ΔΓ(){int ΒЋ=0,Ƶ=0;for(int i=0;i<ϋ.Count;i++){if(!Ɯ(ϋ[i])) ΒЋ++;if(ϋ[i].ǩ>=0) Ƶ++;}if(ϋ.Count==0) return "";
return "Traffic: "+ΒЋ+" moving"+(Ƶ>0?", "+Ƶ+" giving way":"")+"\n";}void Δω(string Ѝ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}