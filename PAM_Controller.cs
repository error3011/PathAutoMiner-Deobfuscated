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
public enum Ν{ў,Ƒ,Ȁ,Б,λ}public enum Λ{ǜ,Ƅ,ΑЉ,Ɩ,ΒΓ,ȫ,ќ,
ƃ,ΓЅ,Ǽ,Αη,Γί,Αΰ,
Δν,Ȳ,ȱ,Ž,ΕΑ,ΓЀ,
ΔА,ΔБ,ΓЁ,ΓЂ,ȳ}public enum Σ{ы,Ɠ,Ǜ,Αθ,İ,ΑЊ,ΑЋ,Ĭ,ĭ}public enum ј{None,Ž,ǜ,Γφ}public enum Ι{Main,ΒΑ,ȭ,Ȭ,Ɲ,į,
Ź,Αό,Αύ,Αϊ,Ȱ,ΒΖ,
Ĺ,Ƙ,ƛ,ΒΙ,Ľ}public enum ѝ{ΒΔ,Γς,Δώ,ΔΡ,λ}public enum О{Ŀ,Γν}public enum ћ{ΒΒ,ΓЄ,ΔЀ,ƈ}public enum Э{ΑЍ,Γυ,Γτ,Γό,Γϋ,ΔΟ,ΔΞ}public enum ч{ƀ,Γγ,Αα}public enum Ĵ{ΔΝ,Ζβ,Γΰ}public enum Ѓ{ΑЎ,ΑЏ,ΑА,Δγ,Δβ,
Δέ,Δή,Δά,Δΰ,
Δα,Δί}public enum Τ{з,ь,ж,Į,ź,Ż}public enum Ζδ{Ζα,Ζά,Ζί}public enum и{ΒΜ,Αι,ΔΫ,ΑЌ}public enum ζ{None,ȁ,Ȯ,ķ}public enum π{Ơ,Ÿ,Ǝ,ƈ,ΑЇ,Ɛ,ΑЈ,ΔЉ,ƒ}public enum φ{ў,ΑЅ,ΑЄ,Ə,Ɖ,ſ,ΑІ}class Ω{public bool ι=false;public Vector3 Υ=new Vector3();
public Vector3 Ϗ=new Vector3();public Vector3 Є=new Vector3();public Vector3 Й=new Vector3();public Vector3 μ=new Vector3();public Vector3 Т=new Vector3();public float Εσ=0;public float Ην=0;public float[]Ы=null;public Ω(){}public Ω(Ω Г){ι=Г.ι;Υ=Г.Υ;Ϗ=Г.Ϗ;Є=Г.Є;Й=Г.Й;μ=Г.μ;
Т=Г.Т;Ы=Г.Ы;}public Ω(Vector3 Υ,Vector3 Є,Vector3 Ϗ,Vector3 Й,Vector3 μ){this.Υ=Υ;this.Ϗ=Ϗ;this.Є=Є;this.Й=Й;this.Εσ=0;this.μ=μ;}public void Ζή(List<IMyThrust>Ń,List<string>ї){Ы=new float[ї.Count];for(int i=0;i<Ы.Length;i++)
Ы[i]=-1;for(int i=0;i<Ń.Count;i++){string Ǹ=ΔΕ(Ń[i]);int Α=ї.IndexOf(Ǹ);if(Α!=-1)
Ы[Α]=М(Ń[i].MaxEffectiveThrust,Ń[i].MaxThrust);}}}class Δζ{public Ω ί=null;public List<Vector3>Ηά=new List<Vector3>();public float ΖЀ=0;public float Ηρ=0;public float Ζμ=0;public float ΖЉ=0;public Vector3 ΖЊ=new Vector3();}class ŀ{public Ѓ К=Ѓ.ΑЎ;public float Я=0;public float Ǧ=0;
public string м="";public string п="";DateTime ΒЄ;public bool ΕЁ=false;private bool ƫ=false;public bool Γξ(bool ΑΑ){if(ΑΑ){Ǧ=0;ƫ=false;return false;}ƫ=true;return Ǧ>Я;}public void Ζγ(){Γξ(true);ΕЁ=false;}public void Ζθ(){if(ƫ)
if((DateTime.Now-ΒЄ).TotalSeconds>1){Ǧ++;ΒЄ=DateTime.Now;}}public bool Αψ(){switch(К){case Ѓ.ΑЏ:return true;case Ѓ.ΑА:return true;}return false;}}class щ{public String θ="";public String Њ="";public int І=0;public Ĵ ǀ=Ĵ.Γΰ;public щ(String θ,String Њ,int І,Ĵ ǀ){this.θ=θ;this.Њ=Њ;
this.І=І;this.ǀ=ǀ;}}class Γχ{public Γχ(Vector3 β,float В){this.β=β;this.В=В;}public Vector3 β;public float В;}class ε{public DateTime ƽ;public DateTime Ββ;public String Ο="";public String θ="";public String ų="";public String Ψ="";public String ΑΠ="";public String Ť="";public String ǋ="";
public Vector3 Υ=new Vector3();public List<щ>ё=new List<щ>();public Ν Π=Ν.ў;public ζ Ǎ;public float ǭ;public float Ǣ;public int ξ;public int Ʈ=0;public int ΑΧ=0;public bool ΑЃ(){return(DateTime.Now-ƽ).TotalSeconds>10;}public bool Ɗ(){if(Ǎ!=ζ.ȁ) return false;return(DateTime.Now-Ββ).TotalSeconds>=4;
}public ε(String Ο){this.Ο=Ο;}public ζ ǝ(String ρ,bool ΕЍ,bool ю,bool ΕΚ){if(ρ==""&&!ю) return ζ.None;if(Ǎ==ζ.ȁ&&Ɗ())
Ǎ=ζ.ķ;if(ΕЍ){ǋ=ρ;Ǎ=ζ.ȁ;Ββ=DateTime.Now;return Ǎ;}else if(ю){ǋ="";Ǎ=ζ.None;}else if(ǋ==ρ){if(ΕΚ) Ǎ=ζ.Ȯ;return Ǎ;}return ζ.None;}}public String ΓЍ(String Ελ){switch(Ελ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem И;Vector3 ΓΗ=new Vector3();Vector3 е=new Vector3();Vector3 Εά=new Vector3();Vector3 Εξ=new Vector3();Vector3 ΕΦ=new Vector3();DateTime ΒЃ=new DateTime();
bool Ď=true;int ΑΤ=0;bool Ė=false;bool ȏ=false;bool Εχ=true;bool Ησ=false;bool ΖЎ=false;bool Εη=false;float ΕЏ=0;float ΓΞ=0;int ΕЇ=0;int Ηξ=0;int Ū=0;int Ζς=0;float ȓ=0;float Ȇ=0;double ǅ=0;float ā=0;List<int>Ȏ=new List<int>();List<int>Ƞ=new List<int>();Ν Π=Ν.ў;
int ΑΗ=0;float Ƿ=0;float Ƕ=0;float Ȥ=0;float ΓΝ=0,ΓΜ=0;IMyRemoteControl Ј;IMySensorBlock ϊ;List<IMyTimerBlock>ΓΧ=new List<IMyTimerBlock>();List<IMyShipConnector>ć=new List<IMyShipConnector>();List<IMyThrust>Ĥ=new List<IMyThrust>();List<IMyGyro>Βψ=new List<IMyGyro>();List<IMyTerminalBlock>Ь=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ƽ=new List<IMyLandingGear>();List<IMyReactor>ǌ=new List<IMyReactor>();List<IMyConveyorSorter>ΖΑ=new List<IMyConveyorSorter>();List<IMyGasTank>ē=new List<IMyGasTank>();List<IMyTerminalBlock>Ā=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ζυ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ζλ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΒΨ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ї=new List<IMyTextPanel>();List<IMyTextSurface>ф=new List<IMyTextSurface>();List<IMyTextPanel>Ǥ=new List<IMyTextPanel>();IMyTerminalBlock ǖ=null;Ω Ρ=new Ω();Ω Βό=new Ω();
Ω ΒЀ=new Ω();bool Ǚ=false;bool Ĩ=false;bool ĩ=false;bool Βώ=false;List<String>ї=new List<string>();List<Ω>υ=new List<Ω>();List<Ω>Ŵ=new List<Ω>();int ΖЁ=0;double ΖΣ=0;Ω Φ=new Ω();Vector3 ě;Vector3 Ȕ;Σ ς=Σ.ы;О Ȑ=О.Ŀ;int г=0;int в=0;double Ş=0;bool Βθ=false;ŀ σ=new ŀ();
ŀ τ=new ŀ();ћ Ǖ=ћ.ΒΒ;bool Ī=true;О Ǵ=О.Ŀ;int С=3;int Р=3;int Ю=30;ч ѐ=ч.ƀ;bool Ǻ=true;bool ĕ=true;bool Č=true;bool ǵ=false;Э ċ=Э.ΑЍ;bool ǘ=true;float р=90;float с=20;float Ч=5;float т=20;float џ=0.70f;float Ŷ=1.50f;float ŵ=2.50f;float ī=0f;float đ=0f;Λ Ŏ;
Σ Ǔ;Λ Ģ;Δζ ΗΙ=null;int Ř=0;int ř=0;int л=0;int ę=0;int ΑΔ=30;int ΗΛ=0;bool ΖΒ=true;Vector3 ΗΕ;double Βν=0;bool ΓΙ=false;int ΖΤ=0;int ΗΧ=0;int Ζν=0;int ΗΘ=0;int ΗΖ=0;Vector3 ΗΚ=new Vector3();float ΗΒ=0;float ΗΨ=0;int ǹ=0;Ω ū=null;float ΗΓ=0;float Ζξ=0;float Βη=0;
float Ǆ=0;bool ΖЏ=false;bool Ημ=false;bool Ηπ=false;bool ΖЌ=false;bool Ζπ=false;DateTime ΖЈ=new DateTime();Ω Ζώ=null;Ω ΕΤ=null;float ΒΩ=0;ј Ă;float Βύ=0;float ΓΪ=0;String Ερ="";float ƿ=0;float Ǣ=0;float ǭ=0;List<щ>ΖА=new List<щ>();float Ѐ=0;bool ΑΚ=false;bool ΖΗ=false;
bool ΕА=false;bool ΓΦ=false;bool ΕБ=false;float Ες=0;float ΕϏ=0;Vector3 ΕΪ=new Vector3();Vector3 ΒЏ=new Vector3();Vector3 Ηβ=new Vector3(1,1,1);float Ηΰ=1;Vector3 Ηα=new Vector3();bool ΖЍ=false;bool Εΰ=false;bool ΗΔ=false;bool Ζκ=false;float Ζύ=0;float Ζι=2;Vector3 ΗΫ;
Vector3 ΗΪ;Vector3 ΗΩ;float[,]ħ=new float[3,2];Dictionary<String,float[,]>і=new Dictionary<string,float[,]>();Dictionary<String,float>Ης=new Dictionary<String,float>();int Ζο=0;Dictionary<String,float[]>ũ=new Dictionary<string,float[]>();float ΗΜ;bool Βκ=false;int ń=0;int Ǐ=0;
int ś=0;int ǩ=0;String Ψ="";int[]ΒЌ=new int[Enum.GetValues(typeof(Ι)).Length];bool Ǉ=false;int Ǯ=0;int ξ=0;Ι ή=Ι.Main;int ΖΘ=0,ΖΙ=0,ΖΚ=0,ΖΛ=0;int ΕЈ=0,ΕЉ=0;bool Ą=false;bool ΒΤ=false;String Ġ="";String Ʀ="";IMyBroadcastListener э=null;bool Βσ=true;List<ε>ό=null;
ε ο=null;int Ŋ=0,ŋ=0;int Βδ=0,Βε=0;int ΕΠ=0,ΕΡ=0,ΕΣ=0;String Εμ="";Exception ȑ=null;bool ΓΘ=false;Σ ΑΕ=Σ.ы;bool Βι=false;bool ΗΑ=true;bool ΗΞ=false;String ΖΠ="";bool ΓΩ=false;bool ΑΞ=true;int ƺ=0;int[]ΗΠ=null;Vector3 ƨ=new Vector3();bool Βφ=false;bool ƌ(IMyTerminalBlock ά)=>ά.CubeGrid==Me.CubeGrid;
Program(){И=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Αω(Me,controllerTag,true))
Π=Ν.Б;Χ(Ι.Main);if(Π!=Ν.Б){Ψ="Welcome to [PAM]!";и ΑΒ=Δλ();if(Π==Ν.ў){List<IMyShipDrill>Βρ=new List<IMyShipDrill>();List<IMyShipGrinder>Βχ=new List<IMyShipGrinder>();И.GetBlocksOfType(Βρ,q=>q.CubeGrid==Me.CubeGrid);И.GetBlocksOfType(Βχ,q=>q.CubeGrid==Me.CubeGrid);if(Βρ.Count>0)
{Π=Ν.Ƒ;Ψ="Miner mode enabled!";}else if(Βχ.Count>0){Π=Ν.Ȁ;Ψ="Grinder mode enabled!";}else{Π=Ν.λ;ΒΘ(ѝ.λ);}}if(ΑΒ==и.ΒΜ) Εχ=false;if(ΑΒ==и.Αι) Ψ="Data restore failed!";if(ΑΒ==и.ΑЌ) Ψ="New version, wipe data";}}void Main(string Ѝ,UpdateType ΖΝ){try{if(ȑ!=null){if(Ѝ.Trim().ToUpper()=="RETRY")
{ȑ=null;Ď=true;ΕЇ=0;if(Π!=Ν.Б) ǟ();Ψ="Restarted after error";}else{ΔΙ();Δθ();return;}}ȏ=(ΖΝ&UpdateType.Update10)!=0;if(ȏ) ΑΤ++;Ė=ΑΤ>=10;if(Ė) ΑΤ=0;if(ȏ){ń++;if(ń>4) ń=0;Ū=(int) Math.Max(0,10-(DateTime.Now-ΒЃ).TotalSeconds);}if(Ѝ!="") Εμ=Ѝ;if(Π!=Ν.Б)
Δύ(Ѝ);else
Γσ(Ѝ);Ǉ=false;try{int Βζ=Runtime.CurrentInstructionCount;float Ǳ=М(Βζ,Runtime.MaxInstructionCount);if(Ǳ>0.90) Ψ="Max. instructions >90%";if(Ǳ>ȓ) ȓ=Ǳ;if(Βκ){Ȏ.Add(Βζ);while(Ȏ.Count>10) Ȏ.RemoveAt(0);Ȇ=0;for(int i=0;i<Ȏ.Count;i++) Ȇ+=Ȏ[i];Ȇ=М(М(Ȇ,Ȏ.Count),Runtime.MaxInstructionCount);
double ƾ=Runtime.LastRunTimeMs;if(Εη&&ƾ>ǅ) ǅ=ƾ;Ƞ.Add((int)(ƾ*1000f));while(Ƞ.Count>10) Ƞ.RemoveAt(0);ā=0;for(int i=0;i<Ƞ.Count;i++) ā+=Ƞ[i];ā=М(ā,Ƞ.Count)/1000f;}}catch{ȓ=0;}}catch(Exception e){ȑ=e;}}String ΓЎ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Χ(Ι Ћ){ΒЌ[(int) ή]=ξ;ξ=ΒЌ[(int) Ћ];if(Ћ==Ι.Ȱ) ξ=0;ή=Ћ;if(Π!=Ν.Б)
ł(ή==Ι.ȭ,false,0,0);Ǉ=true;}void ΔΩ(bool ǁ){if(ξ>0) ξ--;else if(ǁ) ξ=Ǯ-1;}void ΔΨ(bool ǁ){if(ξ<Ǯ-1) ξ++;else if(ǁ) ξ=0;}String ΒΘ(ѝ γ){switch(γ){case ѝ.ΒΔ:Ψ="Job is running";break;case ѝ.Γς:Ψ="Connector not ready!";break;case ѝ.Δώ:Ψ="Ship modified, path outdated!";break;
case ѝ.ΔΡ:Ψ="Interrupted by player!";break;case ѝ.λ:Ψ="Shuttle mode enabled!";break;}return "";}String ΔΔ(О β){switch(β){case О.Ŀ:return "Top-Left";case О.Γν:return "Center";default:return "";}}String ΓЊ(ч η){switch(η){case ч.Γγ:return "Auto"+(Π==Ν.Ƒ?" (Ore)":"");case ч.Αα:return "Auto (+Stone)";
case ч.ƀ:return "Default";default:return "";}}String ΔΑ(Σ ώ){switch(ώ){case Σ.ы:return "No job";case Σ.Ɠ:return "Job paused";case Σ.Ǜ:return "Job active";case Σ.Ĭ:return "Job active";case Σ.ĭ:return "Job active";case Σ.Αθ:return "Job done";case Σ.İ:return "Job changed";case Σ.ΑЊ:return "Move home";
case Σ.ΑЋ:return "Move to job";}return "";}String Αξ(ћ ΕΘ){switch(ΕΘ){case ћ.ΒΒ:return "Return home";case ћ.ΓЄ:return "Fly to job pos";case ћ.ΔЀ:return "Stop";case ћ.ƈ:return "Ignore";}return "";}String ΓЌ(Э η){switch(η){case Э.ΑЍ:return "Off";case Э.Γό:return "Drop pos (Stone) ";case Э.Γϋ:return "Drop pos (Sto.+Ice)";
case Э.Γυ:return "Cur. pos (Stone)";case Э.Γτ:return "Cur. pos (Sto.+Ice)";case Э.ΔΟ:return "In motion (Stone)";case Э.ΔΞ:return "In motion (Sto.+Ice)";}return "";}String Αμ(ј ώ){switch(ώ){case ј.None:return "No batteries";case ј.Ž:return "Charging";case ј.Γφ:return "Discharging";}return "";}
String Ας(Λ ώ){String ΖБ=Π==Ν.λ?"target":"job";switch(ώ){case Λ.ǜ:return "Idle";case Λ.Ƅ:return "Flying to XY position";case Λ.ΑЉ:return Π==Ν.Ȁ?"Grinding":"Mining";case Λ.Ɩ:return "Returning";case Λ.ΓЁ:return "Flying to drop pos";case Λ.ΒΓ:return "Returning to dock";case Λ.ȫ:return "Flying to dock area";
case Λ.ќ:return "Flying to job area";case Λ.ƃ:return "Flying to path";case Λ.ΓЅ:return "Flying to job position";case Λ.Ǽ:return "Approaching dock";case Λ.Αη:return "Docking";case Λ.Γί:return "Aligning";case Λ.Αΰ:return "Aligning";case Λ.Δν:return "Retry docking";case Λ.Ȳ:return "Unloading";case Λ.ȳ:return ΖΠ;
case Λ.ȱ:return "Undocking";case Λ.Ž:return "Charging batteries";case Λ.ΕΑ:return "Waiting for uranium";case Λ.ΓЀ:return "Filling up hydrogen";case Λ.ΔА:return "Waiting for ejection";case Λ.ΔБ:return "Waiting for ejection";case Λ.ΓЂ:return "Flying to drop pos";}return "";}String Αχ(Ѓ К){switch(К)
{case Ѓ.ΑЎ:return "On \"Undock\" command";case Ѓ.ΑЏ:return "On player entered cockpit";case Ѓ.Δγ:return "On ship is full";case Ѓ.Δβ:return "On ship is empty";case Ѓ.ΑА:return "On time delay";case Ѓ.Δή:return "On batteries empty(<25%)";case Ѓ.Δά:return "On batteries empty(=0%)";case Ѓ.Δέ:return "On batteries full";
case Ѓ.Δα:return "On hydrogen empty(<25%)";case Ѓ.Δί:return "On hydrogen empty(=0%)";case Ѓ.Δΰ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int ΓΛ,int ǎ,bool Ε,String Ǫ){Ǯ+=1;if(ΓΛ==ǎ) Ǫ=">"+Ǫ+(ń>=2?" .":"");else Ǫ=" "+Ǫ;Β+=Ǫ+"\n";return ΓΛ==ǎ&&Ε;}String Γε(bool Ε)
{int Α=0;int Δ=ξ;Ǯ=0;String ȣ="———————————————\n";String Ȧ="--------------------------------------------\n";String Β="";Β+=ΔΑ(ς)+" | "+(Ρ.ι?"Ready to dock":"No dock")+"\n";Β+=ȣ;double ΓΣ=Math.Max(Math.Round(this.Βν),0);if(ή==Ι.Main){bool ǐ=Π==Ν.λ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΔϏ();
if(Π==Ν.Ƒ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Χ(Ι.ȭ);if(Π==Ν.Ȁ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Χ(Ι.ȭ);if(Π==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Χ(Ι.ƛ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) Αε();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΓЃ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΓІ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ǐ) Χ(Ι.Ľ);else Χ(Ι.į);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Χ(Ι.Αό);if(Π!=Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Χ(Ι.Αϊ);}else if(ή==Ι.ȭ){double ΖΥ=Math.Round(С*Ƿ,1);double Εγ=Math.Round(Р*Ƕ,1);String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Start new job!")) ΒΚ();if(Ζ(ref κ,Δ,Α++,Ε," Change current job")){Ɵ(false,false);Χ(Ι.Main);}if(Ζ(ref κ,Δ,Α++,Ε," Width + (Width: "+С+" = "+ΖΥ+"m)")){ǻ(ref С,5,20,1);Ȅ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Width -")){ǻ(ref С,-5,20,-1);Ȅ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height + (Height: "+Р+" = "+Εγ+"m)")){ǻ(ref Р,5,20,1);Ȅ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height -")){ǻ(ref Р,-5,20,-1);Ȅ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth + ("+(ѐ==ч.ƀ?"Depth":"Min")+": "+Ю+"m)")){ǻ(ref Ю,5,50,2);Ȅ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Depth -")){ǻ(ref Ю,-5,50,-2);Ȅ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth mode: "+ΓЊ(ѐ))){ѐ=њ(ѐ);}if(Ζ(ref κ,Δ,Α++,Ε," Start pos: "+ΔΔ(Ǵ))){Ǵ=њ(Ǵ);}if(Π==Ν.Ȁ&&ѐ==ч.Αα) ѐ=њ(ѐ);Β+=Ɨ(8,κ,Δ,ref ΕЈ);}else if(ή==Ι.ƛ){float[]Βλ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.ΒΙ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αχ(σ.К))) σ.К=њ(σ.К);if(!σ.Αψ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƅ((int) σ.Я))) σ.Я=Αζ(σ.Я,Βλ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αχ(τ.К))) τ.К=њ(τ.К);
if(!τ.Αψ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+ƅ((int) τ.Я))) τ.Я=Αζ(τ.Я,Βλ);}else if(ή==Ι.ΒΙ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ΒΚ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.ƛ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.м!=""?σ.м:"-"))) σ.м=Ķ(ref ΖΘ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.п!=""?σ.п:"-"))) σ.п=Ķ(ref ΖΚ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.м!=""?τ.м:"-"))) τ.м=Ķ(ref ΖΙ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.п!=""?τ.п:"-"))) τ.п=Ķ(ref ΖΛ);
}else if(ή==Ι.Ȭ){String ΒЇ=р+" %";if(Ė) Ǐ++;if(Ǐ>1){Ǐ=0;ś++;if(ś>1) ś=0;}bool[]ΑΛ=new bool[]{ǌ.Count==0,Ă==ј.None,ē.Count==0};int ΓΨ=0;while(true){ΓΨ++;ǩ++;if(ǩ>ΑΛ.Length-1) ǩ=0;if(ΓΨ>=ΑΛ.Length) break;if(!ΑΛ[ǩ]) break;}bool ǐ=Π==Ν.λ;if(!ǐ&&Ǻ&&Ѐ!=-1&&ś==0)
ΒЇ=Ѐ<1000000?Math.Round(Ѐ)+" Kg":Math.Round(Ѐ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){ǟ();Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ǐ) Χ(Ι.į);else Χ(Ι.Ľ);if(!ǐ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) Γή(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΓΩ=true;Β+=Ȧ;if(!ǐ) Β+="Progress: "+Math.Round(Ş,1)+" %\n";Β+="State: "+Ας(Ŏ)+" "+ΓΣ+"m \n";Β+="Load: "+ƿ+" % Max: "+ΒЇ+" \n";if(ǩ==0) Β+="Uranium: "+(ǌ.Count==0?"No reactors":Math.Round(ΓΪ,1)+"Kg "+(Ч==-1?"":" Min: "+Ч+" Kg"))+"\n";
if(ǩ==1) Β+="Battery: "+(Ă==ј.None?Αμ(Ă):ΒΩ+"% "+(с==-1||ǐ?"":" Min: "+с+" %"))+"\n";if(ǩ==2) Β+="Hydrogen: "+(ē.Count==0?"No tanks":Math.Round(Βύ,1)+"% "+(т==-1||ǐ?"":" Min: "+т+" %"))+"\n";}else if(ή==Ι.į){String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Back")){if(ς==Σ.Ǜ) Χ(Ι.Ȭ);else Χ(Ι.Main);}
if(Ζ(ref κ,Δ,Α++,Ε," Max load: "+р+"%")) А(ref р,р<=80?-10:-5,π.Ǝ,false);if(Ζ(ref κ,Δ,Α++,Ε," Weight limit: "+(Ǻ?"On":"Off"))) Ǻ=!Ǻ;if(Ζ(ref κ,Δ,Α++,Ε," Ejection: "+ΓЌ(ċ))) ċ=њ(ċ);if(Ζ(ref κ,Δ,Α++,Ε," Toggle sorters: "+(ǵ?"On":"Off"))){ǵ=!ǵ;if(ǵ) ƚ(ΑΞ);}
if(Ζ(ref κ,Δ,Α++,Ε," Unload ice: "+(ǘ?"On":"Off"))) ǘ=!ǘ;if(Ζ(ref κ,Δ,Α++,Ε," Uranium: "+(Ч==-1?"Ignore":"Min "+Ч+"Kg"))) А(ref Ч,(Ч>5?-5:-1),π.Ɛ,true);if(Ζ(ref κ,Δ,Α++,Ε," Battery: "+(с==-1?"Ignore":"Min "+с+"%"))) А(ref с,-5,π.ΑЇ,true);if(Ζ(ref κ,Δ,Α++,Ε," Hydrogen: "+(т==-1?"Ignore":"Min "+т+"%"))) А(ref т,-10,π.ΑЈ,true);
if(Ζ(ref κ,Δ,Α++,Ε," When done: "+(Ī?"Return home":"Stop"))) Ī=!Ī;if(Ζ(ref κ,Δ,Α++,Ε," On damage: "+Αξ(Ǖ))) Ǖ=њ(Ǖ);if(Ζ(ref κ,Δ,Α++,Ε," Advanced...")) Χ(Ι.Ź);Β+=Ɨ(8,κ,Δ,ref ΕЉ);}else if(ή==Ι.Ź){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Σ.Ǜ) Χ(Ι.Ȭ);else Χ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Π==Ν.Ȁ?" Grinder":" Drill")+" inv. balancing: "+(ĕ?"On":"Off"))) ĕ=!ĕ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Π==Ν.Ȁ?" grinders":" drills")+": "+(Č?"Fwd + Bwd":"Fwd"))) Č=!Č;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ŷ+"m/s")) А(ref Ŷ,0.5f,π.Ơ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ŵ+"m/s")) А(ref ŵ,0.5f,π.Ơ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(џ*100f)+"%"+(џ>0.80f?" (risky)":""))) А(ref џ,0.1f,π.Ÿ,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ī*100f+"%")) Αδ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+đ*100f+"%")) Αδ(false,0.05f);
}else if(ή==Ι.Ľ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Σ.Ǜ) Χ(Ι.Ȭ);else Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+р+"%")) А(ref р,р<=80?-10:-5,π.Ǝ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ǘ?"On":"Off"))) ǘ=!ǘ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Ч==-1?"Ignore":"Min "+Ч+"Kg"))) А(ref Ч,(Ч>5?-5:-1),π.Ɛ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(с==-1?"Ignore":"Charge up"))) с=(с==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(т==-1?"Ignore":"Fill up"))) т=(т==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+Αξ(Ǖ))) Ǖ=њ(Ǖ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(џ*100f)+"%"+(џ>0.80f?" (risky)":""))) А(ref џ,0.1f,π.Ÿ,false);
}else if(ή==Ι.ΒΑ){double Εν=0;if(υ.Count>0) Εν=Vector3.Distance(υ.Last().Υ,е);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΔЃ();if(Π!=Ν.λ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(Ǚ?"Use old home":(Ρ.ι?"Was set! ":"none ")))) Ǚ=!Ǚ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(Ǚ?"Use old connector":(Ρ.ι?"Was set! ":"none ")))) Ǚ=!Ǚ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ĩ?"Use old connector":(Φ.ι?"Was set! ":"none ")))) Ĩ=!Ĩ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ĩ?"Use old path":(υ.Count>1?"Count: "+υ.Count:"none ")))) ĩ=!ĩ;Β+=Ȧ;Β+="Wp spacing: "+Math.Round(ΖΣ)+"m\n";}else if(ή==Ι.Ɲ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){ǟ();Χ(Ι.Main);}
Β+=Ȧ;Β+="State: "+Ας(Ŏ)+" \n";Β+="Speed: "+Math.Round(ΕЏ,1)+"m/s\n";Β+="Target dist: "+ΓΣ+"m\n";Β+="Wp count: "+υ.Count+"\n";Β+="Wp left: "+ΖΤ+"\n";}else if(ή==Ι.Αό){List<IMyTerminalBlock>ǣ=ΓЉ();if(Ė) Ǐ++;if(Ǐ>=ǣ.Count) Ǐ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.Αύ);
Β+=Ȧ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ƿ,1)+"% "+Math.Round(Ǣ,1)+" / "+Math.Round(ǭ,1)+"\n";Β+="Uranium: "+(ǌ.Count==0?"No reactors":Math.Round(ΓΪ,1)+"Kg "+Ερ)+"\n";Β+="Battery: "+(Ă==ј.None?"":ΒΩ+"% ")+Αμ(Ă)+"\n";Β+="Hydrogen: "+(ē.Count==0?"No tanks":Math.Round(Βύ,1)+"% ")+"\n";
Β+="Damage: "+(ǣ.Count==0?"None":""+(Ǐ+1)+"/"+ǣ.Count+" "+ǣ[Ǐ].CustomName)+"\n";}else if(ή==Ι.Αύ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȧ;Β+="Next scan: "+Ū+"s\n";Β+="Drill area: "+Math.Round(Ƿ,1)+"x"+Math.Round(Ƕ,1)+"m Ship: "+Math.Round(ΓΝ,1)+"x"+Math.Round(ΓΜ,1)+"m ("+Math.Round(Ȥ,1)+"m)\n";
Β+="Broadcast: "+(Ą?"Online - "+Ġ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȓ*100f,1)+"% \n";}else if(ή==Ι.Αϊ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȧ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ΑΗ==2) Β="Fatal setup error\nNext scan: "+Ū+"s\n";if(ΓΘ) Β="Recompile script now";int Εο=Β.Split('\n').Length;for(int i=Εο;i<=10;i++) Β+="\n";Β+=ȣ;Β+="Last: "+Ψ+"\n";
return Β;}void Αδ(bool Εκ,float ŭ){ǟ();Ɵ(true,false);if(Εκ) А(ref ī,ŭ,π.ƒ,false);else А(ref đ,ŭ,π.ƒ,false);Γμ();ł(true,true,0,0);}float Αζ(float Ȉ,float[]Ų){float Ϊ=Ų[0];for(int i=Ų.Length-1;i>=0;i--)
if(Ȉ<Ų[i]) Ϊ=Ų[i];return Ϊ;}String Ķ(ref int ǎ){String Ϊ="";if(ǎ>=ΓΧ.Count) ǎ=-1;if(ǎ>=0) Ϊ=ΓΧ[ǎ].CustomName;ǎ++;return Ϊ;}void ǻ(ref int value,int ă,int ů,int ΑΜ){if(ă==0) return;if(value<ů&&ΑΜ>0||value<=ů&&ΑΜ<0){value+=ΑΜ;return;}int ΕΓ=Math.Abs(ă);int ơ=0;
int ǈ=1;while(true){ơ+=ǈ*ΕΓ*10;if(ă<0&&value-ů<=ơ) break;if(ă>0&&value-ů<ơ) break;ǈ++;}value+=ǈ*ă;}void Ȅ(bool Ηο){С=Math.Max(С,1);Р=Math.Max(Р,1);Ю=Math.Max(Ю,0);ł(ή==Ι.ȭ,false,0,0);}void А(ref float value,float ŭ,π Њ,bool Ħ){А(ref value,value+ŭ,Њ,Ħ,true);
}void А(ref float value,float ȕ,π Њ,bool Ħ,bool ΖΨ){float Ш=0,Ц=0;if(Њ==π.Ơ){Ш=0.5f;Ц=10f;}if(Њ==π.Ÿ){Ш=0.1f;Ц=1f;}if(Њ==π.ƈ){Ш=50f;Ц=100f;}if(Њ==π.ΑЇ){Ш=5f;Ц=30f;}if(Њ==π.Ɛ){Ш=1f;Ц=25f;}if(Њ==π.Ǝ){Ш=10f;Ц=95f;}if(Њ==π.ΑЈ){Ш=10f;Ц=90f;}
if(Њ==π.ΔЉ){Ш=10f;Ц=1800;}if(Њ==π.ƒ){Ш=0.0f;Ц=0.75f;}if(ȕ==-1&&Ħ){value=-1;return;}if(value==-1) Ħ=false;bool ΒБ=ȕ<Ш||ȕ>Ц;if(ΒБ&&ΖΨ){if(ȕ<value) value=Ц;else if(ȕ>value) value=Ш;}else value=ȕ;if(ΒБ&&Ħ) value=-1;else value=Math.Max(Ш,Math.Min(value,Ц));
value=(float) Math.Round(value,2);}void Γή(bool ΕЃ){if(ΕЃ) л=Math.Max(0,л-1);else л++;Γώ();Γύ();for(int k=0;k<ƹ.Count&&k<а.Length;k++) Δω(ƹ[k],k<л);ǯ=-1;ΒΣ(true);}T њ<T>(T Ȉ){int Α=Array.IndexOf(Enum.GetValues(Ȉ.GetType()),Ȉ);Α++;if(Α>=ΓϏ(Ȉ)) Α=0;return(T) Enum.GetValues(Ȉ.GetType()).GetValue(Α);
}int ΓϏ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔϏ(){Ŵ.Clear();for(int i=0;i<υ.Count;i++) Ŵ.Add(υ[i]);υ.Clear();Βώ=true;Βό=new Ω(Ρ);ΒЀ=new Ω(Φ);Ρ.ι=false;if(Π==Ν.λ) Φ.ι=false;for(int i=0;i<і.Count;i++)
if(!ї.Contains(і.Keys.ElementAt(i)))
ї.Add(і.Keys.ElementAt(i));Ǚ=false;Ĩ=false;ĩ=false;Χ(Ι.ΒΑ);}void ΔЃ(){if(Ǚ) Ρ=Βό;if(Ĩ) Φ=ΒЀ;if(ĩ){υ.Clear();for(int i=0;i<Ŵ.Count;i++) υ.Add(Ŵ[i]);}Βώ=false;ǟ();Χ(Ι.Main);}void Ļ(Ω У,Σ ώ){ΕΤ=У;if(ς==Σ.Ǜ) Ǔ=ώ;}bool Αο(Ω У,float В,bool ΖΞ,out Vector3 β){if(ΖΞ){Vector3I Εί=new Vector3I((int) У.Т.X,(int) У.Т.Y,(int) У.Т.Z);
IMySlimBlock ά=Me.CubeGrid.GetCubeBlock(Εί);if(ά==null||!(ά.FatBlock is IMyShipConnector)){β=new Vector3();return false;}Vector3 ΕΟ=ĵ(Ј,ά.FatBlock.GetPosition()-е);Vector3 ΕΞ=ĵ(Ј,ά.FatBlock.WorldMatrix.Forward);β=У.Υ-ΒΟ(У.Є,У.Ϗ*-1,ΕΟ)-ΒΟ(У.Є,У.Ϗ*-1,ΕΞ)*В;return true;}
else{β=У.Υ;return true;}}Vector3 ΓА(int ΕΜ,int ȟ,bool ΕЄ){if(!ΕЄ&&Βφ) return ƨ;float ΕΝ=((г-1f)/2f)-ΕΜ;float ΕІ=((в-1f)/2f)-ȟ;ƨ=Φ.Υ+Φ.Й*ΕΝ*Ƿ+Ȕ*-1*ΕІ*Ƕ;Βφ=true;return ƨ;}Τ Ɓ(){float П=-1;Τ Ϊ=Τ.з;if(Π!=Ν.λ){ΔΧ();if(ς!=Σ.ы){Vector3 ω=ΒΠ(ě,Ȕ*-1,е-Φ.Υ);
if(Math.Abs(ω.X)<=(float)(г*Ƿ)/2f&&Math.Abs(ω.Y)<=(float)(в*Ƕ)/2f){if(ω.Z<=-1&&ω.Z>=-ΑΔ*2) return Τ.Į;if(ω.Z>-1&&ω.Z<Ȥ*2) return Τ.з;}if(љ(Φ.Υ,ref П)) Ϊ=Τ.з;}if(Ρ.ι){if(љ(Ρ.Υ,ref П)) Ϊ=Τ.ж;for(int i=0;i<υ.Count;i++)
if(љ(υ[i].Υ,ref П)) Ϊ=Τ.ь;if(Vector3.Distance(е,Ρ.Υ)<dockDist*Ȥ) Ϊ=Τ.ж;if(Ǿ(MyShipConnectorStatus.Connectable)!=null||Ǿ(MyShipConnectorStatus.Connected)!=null)
Ϊ=Τ.ж;}}else{Vector3 β=new Vector3();IMyShipConnector ŉ=Ǿ(MyShipConnectorStatus.Connected);if(Ρ.ι){if(љ(Ρ.Υ,ref П)) Ϊ=Τ.ж;if(Αο(Ρ,dockDist,true,out β))
if(љ(β,ref П)) Ϊ=Τ.ж;if(ŉ!=null&&Vector3.Distance(ŉ.GetPosition(),Ρ.Υ)<5)
return Τ.ź;}for(int i=0;i<υ.Count;i++)
if(Vector3.Distance(υ[i].Υ,Ρ.Υ)>dockDist*Ȥ&&Vector3.Distance(υ[i].Υ,Φ.Υ)>dockDist*Ȥ)
if(љ(υ[i].Υ,ref П)) Ϊ=Τ.ь;if(Φ.ι){if(љ(Φ.Υ,ref П)) Ϊ=Τ.з;if(Αο(Φ,dockDist,true,out β))
if(љ(β,ref П)) Ϊ=Τ.з;if(ŉ!=null&&Vector3.Distance(ŉ.GetPosition(),Φ.Υ)<5)
return Τ.Ż;}}return Ϊ;}bool љ(Vector3 β,ref float П){float В=Vector3.Distance(β,е);if(В<П||П==-1){П=В;return true;}return false;}void ΒΚ(){if(ΑΗ>0){Ψ="Setup error! Can't start";return;}if(Π==Ν.λ){Αε();return;}Φ.Υ=е;Φ.μ=Ј.GetNaturalGravity();Φ.Є=Εά;Φ.Ϗ=ΕΦ;Φ.Й=Εξ;
ě=ǖ.WorldMatrix.Forward;Ȕ=Φ.Ϗ;if(ě==Ј.WorldMatrix.Down) Ȕ=Ј.WorldMatrix.Backward;Ɵ(true,true);ΰ(Λ.Ƅ);Ɯ();}void Ɵ(bool Ф,bool ΕЅ){if(ς==Σ.ы&&!Ф) return;bool Εψ=Ф||ς==Σ.Αθ||г!=С||в!=Р||Ȑ!=Ǵ;if(Εψ){if(ς!=Σ.ы){ς=Σ.İ;ΓА(Ř,ř,ΕЅ);Ψ="Job changed, lost progress";}Ȑ=Ǵ;
г=С;в=Р;ř=0;Ř=0;а="";ǯ=-1;Ǆ=0;ę=0;Βη=0;л=0;ΒΣ(true);}}void Ɣ(){Δχ(е,0);ĺ(Ĥ,true);}void Ɯ(){ΒΘ(ѝ.ΒΔ);Ɣ();ƙ(Ƽ,false);ƚ(ΑΞ);ς=Σ.Ǜ;Γλ(true);Ǔ=ς;Χ(Ι.Ȭ);Δπ();Βθ=true;ƺ=0;for(int i=Ā.Count-1;i>=0;i--)
if(ΑϏ(Ā[i],false)) ƺ++;if(ƺ>0) Ψ="Started with damage";}void ǟ(){if(ς==Σ.Ǜ){ς=Σ.Ɠ;Ψ="Job paused";}ΰ(Λ.ǜ);Ǔ=ς;Δψ(false,0,0,0,0);ΔЁ();Γα(new Vector3(),false);ΔЂ();Δτ(ChargeMode.Auto);Δϊ(false);Δφ(true);Ǟ(Λ.ǜ);ł(false,false,0,0);ĺ(Ь,false);ĺ(ć,true);ƚ(true);ΓΙ=false;Γο();
ΕΥ=false;Δκ();Βθ=false;ΑΚ=false;ΓΩ=false;if(ή!=Ι.Main&&ή!=Ι.į&&ή!=Ι.Ź&&ή!=Ι.Ľ)
Χ(Ι.Main);}void Αε(){Τ ђ=Ɓ();if(Π==Ν.λ){if(!Φ.ι||!Ρ.ι) return;Ɯ();bool Ƶ=Vector3.Distance(е,Ρ.Υ)<Vector3.Distance(е,Φ.Υ);if(ΑΕ==Σ.Ĭ) Ƶ=true;if(ΑΕ==Σ.ĭ) Ƶ=false;if(Ƶ){Ļ(Ρ,Σ.Ĭ);switch(ђ){case Τ.ź:ΰ(Λ.ȳ);break;case Τ.ь:ΰ(Λ.ќ);break;case Τ.ж:ΰ(Λ.Ǽ);break;default:ΰ(Λ.ȱ);break;
}}else{Ļ(Φ,Σ.ĭ);switch(ђ){case Τ.Ż:ΰ(Λ.ȳ);break;case Τ.з:ΰ(Λ.Ǽ);break;case Τ.ь:ΰ(Λ.ќ);break;default:ΰ(Λ.ȱ);break;}}}else{if(ς!=Σ.Ɠ&&ς!=Σ.İ) return;bool ΖΡ=ς==Σ.İ;Ɯ();bool Ʒ=ΔΣ(false)&&Ρ.ι;switch(ђ){case Τ.з:ΰ(Ʒ?Λ.ƃ:Λ.Ƅ);break;case Τ.ь:ΰ(Ʒ?Λ.ȫ:Λ.ќ);break;case Τ.ж:ΰ(Ʒ?Λ.Ǽ:Λ.Ȳ);break;
case Τ.Į:{if(ę!=л||ΖΡ) ΰ(Λ.Ɩ);else ΰ(Λ.ΑЉ);}break;default:break;}}}void ΓІ(){if(ς==Σ.ы&&!Ρ.ι) return;if(Π==Ν.λ&&(!Φ.ι||!Ρ.ι)) return;Ψ="Move to job";Τ ђ=Ɓ();if(Π==Ν.λ){Ļ(Φ,Σ.ĭ);switch(ђ){case Τ.з:ΰ(Λ.Ǽ);break;case Τ.ь:ΰ(Λ.ќ);break;case Τ.Ż:return;default:ΰ(Λ.ȱ);break;
}Ǟ(Λ.ȳ);}else{switch(ђ){case Τ.з:ΰ(Λ.ќ);break;case Τ.ь:ΰ(Λ.ќ);break;case Τ.ж:ΰ(Λ.Ȳ);break;case Τ.Į:ΰ(Λ.Ɩ);break;default:break;}if(ς==Σ.ы) Ǟ(Λ.ќ);else Ǟ(Λ.Αΰ);ΓΙ=true;}Ɣ();Χ(Ι.Ɲ);ƙ(Ƽ,false);Ǔ=Σ.ΑЋ;}void ΓЃ(){if(!Ρ.ι) return;Ψ="Move home";Τ ђ=Ɓ();if(Π==Ν.λ){Ļ(Ρ,Σ.Ĭ);switch(ђ)
{case Τ.ь:ΰ(Λ.ȫ);break;case Τ.ж:ΰ(Λ.Ǽ);break;case Τ.ź:return;default:ΰ(Λ.ȱ);break;}Ǟ(Λ.ȳ);}else{if(Ǿ(MyShipConnectorStatus.Connected)!=null) return;if(Ǿ(MyShipConnectorStatus.Connectable)!=null){ΰ(Λ.Αη);Ǟ(Λ.Ȳ);return;}switch(ђ){case Τ.з:ΰ(Λ.ƃ);break;case Τ.ь:ΰ(Λ.ȫ);break;case Τ.ж:ΰ(Λ.ȫ);break;
case Τ.Į:ΰ(Λ.ΒΓ);break;default:break;}Ǟ(Λ.Ȳ);}Ɣ();Χ(Ι.Ɲ);ƙ(Ƽ,false);Ǔ=Σ.ΑЊ;}void ΰ(Λ ǉ){if(ǉ==Λ.ǜ) Ģ=Λ.ǜ;if(Ģ!=Λ.ǜ&&Ŏ==Ģ&&ǉ!=Ģ){ǟ();return;}ΖΒ=true;Ŏ=ǉ;}void Ǟ(Λ ώ){this.Ģ=ώ;}void ΒΣ(bool ΑΑ){if(ΑΑ){Ş=0;return;}if(Π==Ν.λ) return;float ψ=л*Math.Max(1,Ю);if(ę==л) ψ+=Math.Min(Ю,Βη);
float ΖΜ=г*в*Math.Max(1,Ю);Ş=Math.Max(Ş,(float) Math.Min(ψ/ΖΜ*100.0,100));}List<IMyTerminalBlock>ΓЉ(){List<IMyTerminalBlock>ǣ=new List<IMyTerminalBlock>();for(int i=0;i<Ā.Count;i++){IMyTerminalBlock ά=Ā[i];if(ΑϏ(ά,false)) ǣ.Add(ά);}return ǣ;}bool ΑϏ(IMyTerminalBlock ά,bool ΑΡ)
{return(!ΑЀ(ά,ΑΡ)||!ά.IsFunctional);}bool ΑЀ(IMyTerminalBlock ά,bool ΑΡ){if(ά==null) return false;try{IMyCubeBlock Βυ=Me.CubeGrid.GetCubeBlock(ά.Position).FatBlock;if(ΑΡ) return Βυ==ά;else return Βυ.GetType()==ά.GetType();}catch{return false;}}bool ΔΣ(bool ΗΟ){if(Ǻ&&Π!=Ν.λ)
if(Ѐ!=-1&&ΓΞ>=Ѐ){Ψ="Ship too heavy";return true;}if(ƿ>=р||ΑΚ){ΑΚ=false;Ψ="Ship is full";return true;}return false;}static String ΔΕ(IMyThrust х){return х.BlockDefinition.SubtypeId;}Vector3 ƍ(Vector3 Е,float[,]Ȓ){return new Vector3(
Е.X>=0?Ȓ[0,0]:Ȓ[0,1],
Е.Y>=0?Ȓ[1,0]:Ȓ[1,1],
Е.Z>=0?Ȓ[2,0]:Ȓ[2,1]);}bool Αφ(Ω Ϋ,String Ǹ,out float Ʊ){Ʊ=0;int Α=ї.IndexOf(Ǹ);if(Α==-1||Ϋ.Ы==null||Α>=Ϋ.Ы.Length)
return false;Ʊ=Ϋ.Ы[Α];if(Ʊ==-1) return false;return true;}Vector3 Αυ(Vector3 Е,Ω Ϋ){if(Ϋ!=null){Vector3 Ϊ=new Vector3();for(int i=0;i<і.Keys.Count;i++){String Ǹ=і.Keys.ElementAt(i);float Ċ=0;if(!Αφ(Ϋ,Ǹ,out Ċ))
return ƍ(Е,ħ);Ϊ+=ƍ(Е,і.Values.ElementAt(i))*Ċ;}return Ϊ;}return ƍ(Е,ħ);}float Ƈ(Vector3 Е,Ω Ϋ){return Ƈ(Е,new Vector3(),Ϋ);}float Ƈ(Vector3 Е,Vector3 μ,Ω Ϋ){Vector3 ΖΖ=Αυ(Е,Ϋ);Vector3 ΕΨ=ΖΖ+μ*ΓΞ;float Ǳ=(ΕΨ/Е).AbsMin();return(float)(Е*Ǳ).Length();}float ĳ(Vector3 ď,Vector3 Ё,Vector3 μ,Ω Ϋ)
{if(μ.Length()==0f) return 0;Vector3 Επ=ΒΠ(ď,Ё,Vector3.Normalize(μ));float Ф=Ƈ(-Επ,Ϋ);return Ф/μ.Length();}void Γλ(bool Εζ){float Л=0;float Ƴ=0.9f;if(Εζ){Ѐ=-1;ǹ=0;ū=null;if(ς!=Σ.ы&&Φ.μ.Length()!=0){Л=Ƴ*ĳ(Φ.Є,Φ.Ϗ*-1,Φ.μ,null);if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}if(Ρ.ι&&Ρ.μ.Length()!=0)
{Л=Ƴ*ĳ(Ρ.Є,Ρ.Ϗ*-1,Ρ.μ,null);if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}return;}if(ǹ==-1) return;if(ǹ>=0){int ΓΕ=0;while(ǹ<υ.Count){if(ΓΕ>100) return;ΓΕ++;Ω Ϋ=υ[ǹ];if(Ϋ.μ.Length()!=0f){Л=Ƴ*Math.Min(
ĳ(Ϋ.Є,Ϋ.Ϗ*-1,Ϋ.μ,Ϋ),
ĳ(Ϋ.Є*-1,Ϋ.Ϗ*-1,Ϋ.μ,Ϋ));if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}else ū=Ϋ;ǹ++;}ǹ=-1;}bool ƶ=true;float ǔ=0;if(ū!=null){for(int i=0;i<і.Count;i++){String Ǹ=і.Keys.ElementAt(i);float[,]ΑΨ=і.Values.ElementAt(i);float Ċ=0;if(!Αφ(ū,Ǹ,out Ċ)){ƶ=false;break;}for(int a=0;a<ΑΨ.GetLength(0);a++)
for(int b=0;b<ΑΨ.GetLength(1);b++){float Ǡ=Math.Abs(ΑΨ[a,b]*Ċ);if(Ǡ==0) continue;ƶ=true;if(ǔ==0||Ǡ<ǔ) ǔ=Ǡ;}}}if(!ƶ){for(int a=0;a<ħ.GetLength(0);a++)
for(int b=0;b<ħ.GetLength(1);b++){float Ǡ=Math.Abs(ħ[a,b]);if(Ǡ==0) continue;if(ǔ==0||Ǡ<ǔ) ǔ=Ǡ;}}if(ǔ>0){float Ετ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Л=М(ǔ,Ετ);if(Л>0)
if(Л<Ѐ||Ѐ==-1) Ѐ=Л;}}void Γα(Vector3 Ф,bool ύ){if(!ύ){for(int i=0;i<Ĥ.Count;i++)
Ĥ[i].SetValueFloat("Override",0.0f);return;}Vector3 ƣ=Αυ(Ф,null);float ΖΩ=Math.Min(1,Math.Abs(М(Ф.X,ƣ.X)));float ΖΪ=Math.Min(1,Math.Abs(М(Ф.Y,ƣ.Y)));float ΖΫ=Math.Min(1,Math.Abs(М(Ф.Z,ƣ.Z)));for(int i=0;i<Ĥ.Count;i++){IMyThrust х=Ĥ[i];Vector3 ȉ=Δξ(ĵ(Ј,х.WorldMatrix.Backward),1);
if(ȉ.X!=0&&Math.Sign(ȉ.X)==Math.Sign(Ф.X))
х.SetValueFloat("Override",х.MaxThrust*ΖΩ);else if(ȉ.Y!=0&&Math.Sign(ȉ.Y)==Math.Sign(Ф.Y))
х.SetValueFloat("Override",х.MaxThrust*ΖΪ);else if(ȉ.Z!=0&&Math.Sign(ȉ.Z)==Math.Sign(Ф.Z))
х.SetValueFloat("Override",х.MaxThrust*ΖΫ);else
х.SetValueFloat("Override",0.0f);}}void Δψ(bool ύ,float ΕЂ,float ș,float Ȟ,float Ȫ){for(int i=0;i<Βψ.Count;i++){IMyGyro ν=Βψ[i];ν.GyroOverride=ύ;if(!ύ) ν.GyroPower=100;else ν.GyroPower=ΕЂ;if(!ύ) continue;Vector3 З=Ј.WorldMatrix.Forward;Vector3 ȝ=Ј.WorldMatrix.Right;Vector3 Ё=Ј.WorldMatrix.Up;
Vector3 Ȋ=ν.WorldMatrix.Forward;Vector3 Ȍ=ν.WorldMatrix.Up;Vector3 ȋ=ν.WorldMatrix.Left*-1;if(Ȋ==З) ν.SetValueFloat("Roll",Ȫ);else if(Ȋ==(З*-1)) ν.SetValueFloat("Roll",Ȫ*-1);else if(Ȍ==(З*-1)) ν.SetValueFloat("Yaw",Ȫ);else if(Ȍ==З) ν.SetValueFloat("Yaw",Ȫ*-1);else if(ȋ==З) ν.SetValueFloat("Pitch",Ȫ);
else if(ȋ==(З*-1)) ν.SetValueFloat("Pitch",Ȫ*-1);if(ȋ==(ȝ*-1)) ν.SetValueFloat("Pitch",ș);else if(ȋ==ȝ) ν.SetValueFloat("Pitch",ș*-1);else if(Ȍ==ȝ) ν.SetValueFloat("Yaw",ș);else if(Ȍ==(ȝ*-1)) ν.SetValueFloat("Yaw",ș*-1);else if(Ȋ==(ȝ*-1)) ν.SetValueFloat("Roll",ș);
else if(Ȋ==ȝ) ν.SetValueFloat("Roll",ș*-1);if(Ȍ==(Ё*-1)) ν.SetValueFloat("Yaw",Ȟ);else if(Ȍ==Ё) ν.SetValueFloat("Yaw",Ȟ*-1);else if(ȋ==Ё) ν.SetValueFloat("Pitch",Ȟ);else if(ȋ==(Ё*-1)) ν.SetValueFloat("Pitch",Ȟ*-1);else if(Ȋ==Ё) ν.SetValueFloat("Roll",Ȟ);else if(Ȋ==(Ё*-1)) ν.SetValueFloat("Roll",Ȟ*-1);
}}void ΔЂ(){this.Εΰ=false;}void ΔЁ(){ļ(false,false,false,ΒЏ,0);ΓΦ=false;}void Δχ(Vector3 ί,float ġ){ļ(true,false,false,ί,ġ);}void ļ(bool ύ,bool ΑΝ,bool ΑΩ,Vector3 ί,float ġ){ļ(ύ,ΑΝ,ΑΩ,ί,ί-е,0.0f,ġ);}void ļ(bool ύ,bool ΑΝ,bool ΑΩ,Vector3 ί,Vector3 ΕЋ,float ΕЌ,float ġ)
{ΓΦ=true;this.ΖΗ=ύ;ΒЏ=ί;this.Ες=ġ;this.ΕϏ=ΕЌ;this.ΕБ=ΑΝ;this.ΕА=ΑΩ;this.ΕΪ=ΕЋ;Βν=Vector3.Distance(ί,е);}void Δπ(){И.GetBlocksOfType(Ā,ƌ);}void Γμ(){if(Ј==null) return;ǖ=null;float ť=0,š=0,Ŧ=0,Ţ=0,ŧ=0,ţ=0;List<IMyTerminalBlock>ΑΣ=Ακ(Ь,pamTag,true);bool Εϊ=ΑΣ.Count==0;
if(ΑΣ.Count>0) ǖ=ΑΣ[0];else if(Ь.Count>0) ǖ=Ь[0];int ō=0;float ț=0;for(int i=0;i<Ь.Count;i++){if(Ь[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward){if(Εϊ){ΑΗ=2;Ψ="Mining direction is unclear!";return;}continue;}ō++;ț=Math.Max(ț,ΔЊ(Ь[i]));Vector3 ω=Ɔ(Ј,Ь[i].GetPosition());
if(i==0){ť=ω.X;š=ω.X;Ŧ=ω.Y;Ţ=ω.Y;ŧ=ω.Z;ţ=ω.Z;}š=Math.Max(ω.X,š);ť=Math.Min(ω.X,ť);Ţ=Math.Max(ω.Y,Ţ);Ŧ=Math.Min(ω.Y,Ŧ);ţ=Math.Max(ω.Z,ţ);ŧ=Math.Min(ω.Z,ŧ);}if(ț==0) ț=drillRadius;bool Βπ=ǖ!=null&&ǖ.WorldMatrix.Forward==Ј.WorldMatrix.Down;Ƿ=(š-ť)*(1-ī)+ț*2;
Ƕ=(Βπ?ţ-ŧ:Ţ-Ŧ)*(1-đ)+ț*2;IMyCubeGrid g=Ј.CubeGrid;float Αά=0,Αέ=0,Αή=0,Αί=0;for(int c=0;c<8;c++){Vector3 p=Ɔ(Ј,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Βπ?p.Z:p.Y;if(c==0||p.X<Αά) Αά=p.X;if(c==0||p.X>Αέ) Αέ=p.X;if(c==0||y<Αή) Αή=y;if(c==0||y>Αί) Αί=y;}ΓΝ=Αέ-Αά+g.GridSize;ΓΜ=Αί-Αή+g.GridSize;}float ΔЊ(IMyTerminalBlock b)
{if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ακ<T>(List<T>Ѕ,String Ъ,bool Ś){List<T>Ϊ=new List<T>();
for(int i=0;i<Ѕ.Count;i++)
if(Αω(Ѕ[i],Ъ,Ś)) Ϊ.Add(Ѕ[i]);return Ϊ;}bool Αω<T>(T ά,String Ъ,bool Ś){IMyTerminalBlock b=(IMyTerminalBlock) ά;if(Ś&&b.CustomName.ToUpper().Contains(Ъ.ToUpper())) return true;if(!Ś&&b.BlockDefinition.ToString().ToUpper().Contains(Ъ.ToUpper())) return true;return false;}void ĺ<T>(List<T>Ѕ,bool ύ)
{for(int i=0;i<Ѕ.Count;i++)
Δυ((IMyTerminalBlock) Ѕ[i],ύ);}void Δυ(IMyTerminalBlock ά,bool ύ){if(ά==null) return;String ΕΔ=ύ?"OnOff_On":"OnOff_Off";var a=ά.GetActionWithName(ΕΔ);a.Apply(ά);}void ƚ(bool ύ){ΑΞ=ύ;if(!ǵ) return;ĺ(ΖΑ,ύ);}void Δτ(ChargeMode η){for(int i=0;i<ΒΨ.Count;i++) ΒΨ[i].ChargeMode=η;}void Δϊ(bool ΖΔ)
{for(int i=0;i<ē.Count;i++) ē[i].Stockpile=ΖΔ;}void ƙ(List<IMyLandingGear>ƴ,bool ΒЈ){for(int i=0;i<ƴ.Count;i++){if(ΒЈ) ƴ[i].Lock();if(!ΒЈ) ƴ[i].Unlock();}}void Δφ(bool ύ){if(Βι==ύ) return;List<IMyShipController>Ō=new List<IMyShipController>();И.GetBlocksOfType(Ō,ƌ);if(Ō.Count==0) return;
for(int i=0;i<Ō.Count;i++)
Ō[i].DampenersOverride=ύ;Βι=ύ;}IMyShipConnector Ǿ(MyShipConnectorStatus ΖΓ){for(int i=0;i<ć.Count;i++){if(!ΑЀ(ć[i],true)) continue;if(ć[i].Status==ΖΓ) return ć[i];}return null;}void ł(bool ΑΘ,bool ŏ,float Εέ,float ΕΗ){if(ϊ==null||Ь.Count==0) return;Vector3 Ň=new Vector3();
int ō=0;for(int i=0;i<Ь.Count;i++){if(Ь[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward) continue;ō++;Ň+=Ь[i].GetPosition();}Ň=Ň/ō;Vector3 Ȗ=Ɔ(ϊ,Ň);ϊ.Enabled=true;ϊ.ShowOnHUD=ΑΘ;ϊ.LeftExtend=(ŏ?1:С)/2f*Ƿ-Ȗ.X;ϊ.RightExtend=(ŏ?1:С)/2f*Ƿ+Ȗ.X;ϊ.TopExtend=(ŏ?1:Р)/2f*Ƕ+Ȗ.Y;
ϊ.BottomExtend=(ŏ?1:Р)/2f*Ƕ-Ȗ.Y;ϊ.FrontExtend=(ΑΘ?Ю:Εέ)-Ȗ.Z;ϊ.BackExtend=ΑΘ?0:ΕΗ+Ȥ*0.75f+Ȗ.Z;ϊ.DetectFloatingObjects=true;ϊ.DetectAsteroids=false;ϊ.DetectLargeShips=true;ϊ.DetectSmallShips=true;ϊ.DetectStations=true;ϊ.DetectOwner=true;ϊ.DetectSubgrids=false;
ϊ.DetectPlayers=false;ϊ.DetectEnemy=true;ϊ.DetectFriendly=true;ϊ.DetectNeutral=true;}void ȯ<T>(List<T>Ѕ,bool ΕЎ,float ΕΫ,bool Ει){for(int i=0;i<Ѕ.Count;i++){IMyTextSurface ģ=null;if(Ѕ[i]is IMyTextSurface) ģ=(IMyTextSurface) Ѕ[i];if(ģ!=null){ģ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΕЎ) continue;ģ.Font="Debug";if(Ει) continue;ģ.FontSize=ΕΫ;}}}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 Δξ(Vector3 v,int Ƭ){return new Vector3(Math.Round(v.X,Ƭ),Math.Round(v.Y,Ƭ),Math.Round(v.Z,Ƭ));}Vector3 Ɔ(IMyTerminalBlock ά,Vector3 ΖΧ){return ĵ(ά,ΖΧ-ά.GetPosition());
}Vector3 ĵ(IMyTerminalBlock ά,Vector3 ΑΫ){return Vector3D.TransformNormal(ΑΫ,MatrixD.Transpose(ά.WorldMatrix));}Vector3 ΒΠ(Vector3 ď,Vector3 Ё,Vector3 ΑΫ){MatrixD ǃ=MatrixD.CreateFromDir(ď,Ё);return Vector3D.TransformNormal(ΑΫ,MatrixD.Transpose(ǃ));}Vector3 ΒΟ(Vector3 ď,Vector3 Ё,Vector3 ω)
{MatrixD ǃ=MatrixD.CreateFromDir(ď,Ё);return Vector3D.Transform(ω,ǃ);}String έ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 α(String s){String[]δ=s.Split('|');return new Vector3(float.Parse(Θ(δ,0)),float.Parse(Θ(δ,1)),float.Parse(Θ(δ,2)));}String ΕΒ(Ω Ϋ){String ǳ=":";String Ϊ=έ(Ϋ.Υ)+ǳ+έ(Ϋ.Є)+ǳ+
έ(Ϋ.Ϗ)+ǳ+έ(Ϋ.Й)+ǳ+έ(Ϋ.μ);for(int i=0;i<Ϋ.Ы.Length;i++)
Ϊ+=ǳ+Math.Round(Ϋ.Ы[i],3);return Ϊ;}Ω ΔЄ(String s){String[]δ=s.Split(':');Ω Ϋ=new Ω(
α(Θ(δ,0)),α(Θ(δ,1)),
α(Θ(δ,2)),α(Θ(δ,3)),
α(Θ(δ,4)));int Α=5;List<float>Βς=new List<float>();while(Α<δ.Length){String ű=Θ(δ,Α);float f=0;if(!float.TryParse(ű,out f)) break;Βς.Add(f);Α++;}Ϋ.Ы=Βς.ToArray();return Ϋ;}System.Text.StringBuilder ȡ=new System.Text.StringBuilder();void Κ<T>(T value,bool Εω){if(Εω) ȡ.Append('\n');ȡ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String ı(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ł(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΕΕ,int Α){String ű=ΕΕ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ű)) return "";
return ű;}void Save(){if(ΓΘ||Π==Ν.Б){Storage="";return;}Storage=Γζ();}String Γζ(){ȡ.Clear();ȡ.Append(DATAREV+";");Κ(έ(ΓΗ),false);Κ(έ(Ρ.Є));Κ(έ(Ρ.Й));Κ(έ(Ρ.Ϗ));Κ(έ(Ρ.μ));Κ(έ(Ρ.Υ));Κ(έ(Ρ.Т));Κ(Ρ.ι);Κ(έ(Φ.Υ));Κ(έ(Φ.μ));Κ(έ(Φ.Є));Κ(έ(Φ.Ϗ));Κ(έ(Φ.Й));Κ(έ(Φ.Т));Κ(Φ.ι);Κ(έ(ě));Κ(έ(Ȕ));Κ(";");
Κ((int) Π,false);Κ((int) ς);Κ((int) Ǔ);Κ(р);Κ(с);Κ(Ч);Κ(т);Κ(џ);Κ(Ǻ);Κ(ǘ);if(Π==Ν.λ){Κ((int) σ.К);Κ(σ.Я);Κ(σ.Ǧ);Κ(ı(σ.м));Κ(ı(σ.п));Κ((int) τ.К);Κ(τ.Я);Κ(τ.Ǧ);Κ(ı(τ.м));Κ(ı(τ.п));}else{Κ((int) Ǵ);Κ((int) Ǖ);Κ((int) ċ);Κ((int) ѐ);Κ((int) Ȑ);Κ(Ī);Κ(ǵ);Κ(ĕ);Κ(Č);Κ(С);Κ(Р);Κ(Ю);Κ(Ŷ);Κ(ŵ);Κ(ī);Κ(đ);
Κ(г);Κ(в);Κ(Ř);Κ(ř);Κ(ę);Κ(л);Κ(ΑΔ);Κ(Ǆ);}Κ(";");for(int i=0;i<ї.Count;i++)
Κ((i>0?"|":"")+ї[i],false);Κ(";");for(int i=0;i<υ.Count;i++)
Κ(ΕΒ(υ[i]),i>0);Κ(";",false);Κ(а+"|"+ǯ,false);Κ(";",false);Κ(Γω(),false);String Ξ=ȡ.ToString();ȡ.Clear();return Ξ;}и Δλ(){return Δμ(Storage);}и Δμ(String Ξ){if(Ξ=="") return и.ΔΫ;String[]ǲ=Ξ.Split(';');if(Θ(ǲ,0)!=DATAREV) return и.ΑЌ;int Α=0;try{String[]Η=Θ(ǲ,1).Split('\n');
ΓΗ=α(Θ(Η,Α++));Ρ.Є=α(Θ(Η,Α++));Ρ.Й=α(Θ(Η,Α++));Ρ.Ϗ=α(Θ(Η,Α++));Ρ.μ=α(Θ(Η,Α++));Ρ.Υ=α(Θ(Η,Α++));Ρ.Т=α(Θ(Η,Α++));Ρ.ι=bool.Parse(Θ(Η,Α++));Φ.Υ=α(Θ(Η,Α++));Φ.μ=α(Θ(Η,Α++));Φ.Є=α(Θ(Η,Α++));Φ.Ϗ=α(Θ(Η,Α++));Φ.Й=α(Θ(Η,Α++));Φ.Т=α(Θ(Η,Α++));Φ.ι=bool.Parse(Θ(Η,Α++));
ě=α(Θ(Η,Α++));Ȕ=α(Θ(Η,Α++));Η=Θ(ǲ,2).Split('\n');Α=0;Π=(Ν) int.Parse(Θ(Η,Α++));ς=(Σ) int.Parse(Θ(Η,Α++));Ǔ=(Σ) int.Parse(Θ(Η,Α++));р=int.Parse(Θ(Η,Α++));с=int.Parse(Θ(Η,Α++));Ч=int.Parse(Θ(Η,Α++));т=int.Parse(Θ(Η,Α++));џ=float.Parse(Θ(Η,Α++));Ǻ=bool.Parse(Θ(Η,Α++));
ǘ=bool.Parse(Θ(Η,Α++));if(Π==Ν.λ){σ.К=(Ѓ) int.Parse(Θ(Η,Α++));σ.Я=float.Parse(Θ(Η,Α++));σ.Ǧ=float.Parse(Θ(Η,Α++));σ.м=Ł(Θ(Η,Α++));σ.п=Ł(Θ(Η,Α++));τ.К=(Ѓ) int.Parse(Θ(Η,Α++));τ.Я=float.Parse(Θ(Η,Α++));τ.Ǧ=float.Parse(Θ(Η,Α++));τ.м=Ł(Θ(Η,Α++));τ.п=Ł(Θ(Η,Α++));}else{Ǵ=(О) int.Parse(Θ(Η,Α++));
Ǖ=(ћ) int.Parse(Θ(Η,Α++));ċ=(Э) int.Parse(Θ(Η,Α++));ѐ=(ч) int.Parse(Θ(Η,Α++));Ȑ=(О) int.Parse(Θ(Η,Α++));Ī=bool.Parse(Θ(Η,Α++));ǵ=bool.Parse(Θ(Η,Α++));ĕ=bool.Parse(Θ(Η,Α++));Č=bool.Parse(Θ(Η,Α++));С=int.Parse(Θ(Η,Α++));Р=int.Parse(Θ(Η,Α++));Ю=int.Parse(Θ(Η,Α++));Ŷ=float.Parse(Θ(Η,Α++));
ŵ=float.Parse(Θ(Η,Α++));ī=float.Parse(Θ(Η,Α++));đ=float.Parse(Θ(Η,Α++));г=int.Parse(Θ(Η,Α++));в=int.Parse(Θ(Η,Α++));Ř=int.Parse(Θ(Η,Α++));ř=int.Parse(Θ(Η,Α++));ę=int.Parse(Θ(Η,Α++));л=int.Parse(Θ(Η,Α++));ΑΔ=int.Parse(Θ(Η,Α++));Ǆ=float.Parse(Θ(Η,Α++));}Η=Θ(ǲ,3).Replace("\n","").Split('|');
ї=Η.ToList();Η=Θ(ǲ,4).Split('\n');υ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
υ.Add(ΔЄ(Θ(Η,i)));Η=Θ(ǲ,5).Split('|');а=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ǯ)) ǯ=-1;if(а.Length!=г*в){а="";ǯ=-1;}Γψ(Θ(ǲ,6));}catch{return и.Αι;}ΑΕ=Ǔ;ǟ();return и.ΒΜ;}const int Ζέ=64000;void Δϋ(){Ǥ.Clear();for(int i=Ї.Count-1;i>=0;i--){String Ξ=Ї[i].CustomData.ToUpper();
bool ƻ=false;if(Ξ=="INSTRUCTIONS"){ƻ=true;Βκ=true;}if(Ξ=="DEBUG") ƻ=true;if(ƻ){Ǥ.Add(Ї[i]);Ї.RemoveAt(i);}}ȯ(Ǥ,false,1,false);}void Δό(List<IMyTerminalBlock>ΒΫ){ф.Clear();for(int i=0;i<ΒΫ.Count;i++){IMyTerminalBlock ά=ΒΫ[i];try{String ΓΔ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=ά.CustomName.IndexOf(ΓΔ);int ȥ=-1;if(Α<0||!int.TryParse(ά.CustomName.Substring(Α+ΓΔ.Length,1),out ȥ)) continue;if(ȥ==-1) continue;ȥ--;IMyTextSurfaceProvider ΓΖ=(IMyTextSurfaceProvider) ά;if(ȥ<ΓΖ.SurfaceCount&&ȥ>=0)
ф.Add(ΓΖ.GetSurface(ȥ));}catch{}}}String ǆ="";void Δθ(){String ǂ="";String ĉ="";ǆ=Γε(false);ǂ+=ǆ;ĉ+=ǆ;ĉ+=ΓЎ();for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(ǂ);for(int i=0;i<ф.Count;i++) ф[i].WriteText(ǂ);Echo(ĉ);for(int i=0;i<Ǥ.Count;i++){IMyTextPanel Ќ=Ǥ[i];String Ξ=Ќ.CustomData.ToUpper();
if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Απ());}}string Απ(){String Β="";try{float ΒЉ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ȇ*ΒЉ)+" / "+(int)(ȓ*ΒЉ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ȇ*100f,1)+"% / "+Math.Round(ȓ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(ā,2)+"ms / "+Math.Round(ǅ,2)+"ms \n";}catch{}for(int i=0;i<ũ.Count;i++)
Β+=""+ũ.Keys.ElementAt(i)+" = "+Math.Round(ũ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ũ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΔΙ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ȯ(Ї,setLCDFontAndSize,0.9f,false);ȯ(ф,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(Β+ȑ.ToString());for(int i=0;i<ф.Count;i++) ф[i].WriteText(Β+ȑ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ΒΕ(String γ,String ί,String ǧ){try{if(!Ą) return;γ=BROADCAST_TAG+BROADCAST_SEP+Αν()+BROADCAST_SEP+ί+BROADCAST_SEP+Ʀ+BROADCAST_SEP+ǧ+BROADCAST_SEP+γ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,γ);}catch(Exception e){ȑ=e;}}String Αν(){
if(Π!=Ν.Б) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΑЁ(String γ){return γ.StartsWith(BROADCAST_TAG);}bool Ђ(ref String Ξ,out string č,Char ǳ){int Α=Ξ.IndexOf(ǳ);č="";if(Α<0) return false;č=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool Δδ(ref String γ,out String Ǘ,out String ǧ)
{Ǘ="";ǧ="";if(!Ą) return false;String Ъ="";if(!Ђ(ref γ,out Ъ,BROADCAST_SEP)||!ΑЁ(Ъ)) return false;if(!Ђ(ref γ,out Ǘ,BROADCAST_SEP)) return false;if(!Ђ(ref γ,out Ъ,BROADCAST_SEP)||(Ъ!=Αν()&&(Ъ!="*"&&Π!=Ν.Б))) return false;if(!Ђ(ref γ,out Ъ,BROADCAST_SEP)||(Ъ!=Ʀ)) return false;
if(!Ђ(ref γ,out ǧ,BROADCAST_SEP)) return false;return true;}void ΔΠ(){bool Ć=true;if(Βσ){Βσ=false;if(Me.CustomData.Contains("Antenna_Name")){Ψ="Update custom data";Me.CustomData="";}}String Đ=(Π!=Ν.Б?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Đ)
Αώ(Đ);String[]Η=Me.CustomData.Split('\n');Ą=bool.Parse(ĸ(Ĳ(Η,"Enable_Broadcast",ref Ć)));bool ΑΪ=false;bool Βμ=true;if(Ą){if(Π!=Ν.Б)
Ġ=ĸ(Ĳ(Η,"Ship_Name",ref Ć)).Replace(BROADCAST_SELF_ID,"");Ʀ=ĸ(Ĳ(Η,"Broadcast_Channel",ref Ć)).ToLower();Βμ=false;if(э==null){э=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);э.SetMessageCallback("");}List<IMyRadioAntenna>ȅ=new List<IMyRadioAntenna>();И.GetBlocksOfType(ȅ);bool Ƣ=false;
for(int i=0;i<ȅ.Count;i++)
if(ȅ[i].EnableBroadcasting&&ȅ[i].Enabled){Ƣ=true;break;}if(ȅ.Count==0) Ψ="No Antenna found";else if(!Ƣ) Ψ="Antenna not ready";ΑΪ=ȅ.Count==0||!Ƣ;if(ΒΤ&&!ΑΪ&&Π!=Ν.Б) Ψ="Antenna ok";}else if(Π==Ν.Б)
Ψ="Offline - Enable in PB custom data";ΒΤ=ΑΪ;if(Βμ){if(э!=null) this.IGC.DisableBroadcastListener(э);э=null;}}catch{Ć=false;}if(!Ć){Ψ="Reset custom data";Αώ(Đ);}}void Αώ(String Đ){String Ξ=Đ+"\n\n"+"Enable_Broadcast="+(Π==Ν.Б?"true":"false")+" \n";Ξ+=Π!=Ν.Б?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ĸ(String Ǭ){int Α=Ǭ.IndexOf("//");if(Α!=-1) Ǭ=Ǭ.Substring(0,Α);String[]δ=Ǭ.Split('=');if(δ.Length<=1) return "";return δ[1].Trim();}String Ĳ(String[]Η,String Ǒ,ref bool ȗ){foreach(String Ǭ in Η)
if(Ǭ.StartsWith(Ǒ)) return Ǭ;ȗ=false;return "";}void Γσ(string Ѝ){if(Ď) ό=new List<ε>();String Ǘ="";String ǧ="";if(!ΑЁ(Ѝ)) Δι(Ѝ);bool ΓΟ=false;int Ευ=50;while(э!=null&&э.HasPendingMessage&&Ευ-->0){MyIGCMessage γ=э.AcceptMessage();String Ξ=γ.Data as String;if(Ξ==null) continue;
if(Δδ(ref Ξ,out Ǘ,out ǧ)&&Ǘ!=""&&Ǘ!=BROADCAST_SELF_ID){ε Γ=Ƃ(Ǘ,"");if(Γ==null){Γ=new ε(Ǘ);ό.Add(Γ);}Γ.ǝ(ǧ,false,false,true);ΔЎ(Γ,Ξ);ΓΟ=true;}}if(ΓΟ)
ό.Sort(delegate(ε a,ε b){if(a.θ==null&&b.θ==null) return 0;else if(a.θ==null) return-1;else if(b.θ==null) return 1;else return a.θ.CompareTo(b.θ);});if(Ė||Ď){if(Ū<=0||Ď){Ψ="";Ď=false;Δρ();ΔΠ();ΒЃ=DateTime.Now;}}if(ȏ) ΔЌ();if(ȏ) ΔЍ();if(ȏ) ΔЏ();ΔЋ();}void ΔЎ(ε Γ,String Ξ)
{Γ.ƽ=DateTime.Now;String č="";Ђ(ref Ξ,out Γ.ų,BROADCAST_SEP);Ђ(ref Ξ,out Γ.θ,BROADCAST_SEP);if(Γ.ų!=VERSION) return;String ĝ="";String[]Ț=new string[3];Ђ(ref Ξ,out ĝ,BROADCAST_SEP);Ђ(ref Ξ,out Ț[0],BROADCAST_SEP);Ђ(ref Ξ,out Ț[1],BROADCAST_SEP);Ђ(ref Ξ,out Ț[2],BROADCAST_SEP);
Ђ(ref Ξ,out Γ.ΑΠ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ψ,BROADCAST_SEP);Ђ(ref Ξ,out č,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ť,BROADCAST_SEP);String ΓΫ="",ΒЋ="";Ђ(ref Ξ,out ΓΫ,BROADCAST_SEP);Ђ(ref Ξ,out ΒЋ,BROADCAST_SEP);Γ.ё.Clear();while(true){String ΒϏ;if(!Ђ(ref Ξ,out ΒϏ,BROADCAST_SEP)) break;String[]ŝ=ΒϏ.Split('/');
if(ŝ.Count()<3) continue;int І=0;if(!int.TryParse(ŝ[2],out І)) continue;Γ.ё.Add(new щ(ŝ[0],ŝ[1],І,Ĵ.ΔΝ));}int ΒЍ=0;if(!int.TryParse(č,out Γ.ξ)) Γ.ξ=0;if(!int.TryParse(ĝ,out ΒЍ)) Γ.Π=Ν.ў;if(!float.TryParse(Ț[0],out Γ.Υ.X)) Γ.Υ.X=0;if(!float.TryParse(Ț[1],out Γ.Υ.Y)) Γ.Υ.Y=0;
if(!float.TryParse(Ț[2],out Γ.Υ.Z)) Γ.Υ.Z=0;if(!float.TryParse(ΓΫ,out Γ.Ǣ)) Γ.Ǣ=0;if(!float.TryParse(ΒЋ,out Γ.ǭ)) Γ.ǭ=0;Γ.Π=(Ν) ΒЍ;Γ.Ʈ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Υ));Γ.ΑΧ=0;for(int i=0;i<Γ.ё.Count();i++) Γ.ΑΧ+=Γ.ё[i].І;}ε Ƃ(String Ο,String θ){Ο=Ο.ToUpper();θ=θ.ToUpper();
for(int i=0;i<ό.Count;i++){if(Ο!=""&&ό[i].Ο.ToUpper()==Ο) return ό[i];if(θ!=""&&ό[i].θ!=null&&ό[i].θ.ToUpper()==θ) return ό[i];}return null;}String ȇ="";ε Βί=null;DateTime Βΰ=DateTime.MinValue;List<ε>ǡ=new List<ε>();const float Γκ=20f;bool ΒΗ(ε ί,String ρ){return ρ==ȇ&&ί==Βί&&(DateTime.Now-Βΰ).TotalSeconds<Γκ;
}String ΓЇ(ε ί){if(ȇ==""||!ΒΗ(ί,ȇ)) return "";String s="";int ȗ=0,ΑΙ=0;for(int i=0;i<ǡ.Count;i++){ζ Ŭ=ǡ[i].ǝ(ȇ,false,false,false);if(Ŭ==ζ.Ȯ) ȗ++;if(ΑΙ++<8) s+="  "+ǡ[i].θ+": "+(Ŭ==ζ.Ȯ?"received":Ŭ==ζ.ȁ?"pending...":Ŭ==ζ.ķ?"no answer!":"-")+"\n";
}if(ΑΙ>8) s+="  +"+(ΑΙ-8)+" more\n";return "--------------------------------------------\n"+ȇ+": "+ȗ+"/"+ǡ.Count+" received\n"+s;}void Н(ε Γ,String ρ){ȇ=ρ;Βί=Γ;Βΰ=DateTime.Now;ǡ.Clear();if(Γ==null) ǡ.AddRange(ό);else ǡ.Add(Γ);if(Γ==null){for(int i=0;i<ό.Count;i++)
ό[i].ǝ(ρ,true,false,false);ΒΕ(ρ,"*",ρ);}else{Γ.ǝ(ρ,true,false,false);ΒΕ(ρ,Γ.Ο,ρ);}}void Δι(string Ѝ){if(Ѝ=="") return;var δ=Ѝ.ToUpper().Split(' ');δ.DefaultIfEmpty("");var ρ=δ.ElementAtOrDefault(0);var ΒΦ=δ.ElementAtOrDefault(1);String Εθ="Invalid argument: "+Ѝ;if(ή==Ι.Ȱ){if(ο!=null)
{switch(ρ){case "UP":{if(ξ<2) break;if(ο.ξ==0){ξ=1;return;}Н(ο,"UP");return;}case "DOWN":{if(ξ<1) break;if(ξ==1){Н(ο,"MRES");break;}Н(ο,"DOWN");break;}case "APPLY":{if(ξ<2) break;Н(ο,"APPLY");return;}}}}switch(ρ){case "UP":this.ΔΩ(false);return;case "DOWN":this.ΔΨ(false);return;
case "APPLY":this.ǽ(true);return;}switch(ρ){case "CLEAR":Γπ();return;case "SENDALL":
if(String.IsNullOrEmpty(ΒΦ)){Ψ="Usage: SENDALL <cmd>";return;}Н(null,ΒΦ);return;case "SEND":
if(Ѝ.Length<="SEND".Length+1){Ψ="Usage: SEND <ship>:<cmd>";return;}Δσ(Ѝ.Remove(0,"SEND".Length+1));return;}Ψ=Εθ;}void Δσ(String ΒΥ){if(ΒΥ=="") return;var δ=ΒΥ.Split(':');if(δ.Length!=2){Ψ="Missing separator \":\"";return;}δ.DefaultIfEmpty("");String Ġ=δ.ElementAtOrDefault(0).Trim();
ε Γ=Ƃ("",Ġ);if(Γ!=null) Н(Γ,δ.ElementAtOrDefault(1).Trim());else Ψ="Unknown ship: "+Ġ;}void Γπ(){ό.Clear();ο=null;Χ(Ι.Main);ξ=0;}void Δρ(){List<IMyTerminalBlock>ΓΡ=new List<IMyTerminalBlock>();И.GetBlocksOfType(Ї,ƌ);И.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΓΡ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ї=Ακ(Ї,pamTag,true);Δό(ΓΡ);Δϋ();Δς();ȯ(Ї,setLCDFontAndSize,1.15f,false);ȯ(ф,setLCDFontAndSize,1.15f,true);String Εδ=Αρ(false);String ΒА="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ќ in Ї){if(Ќ.CustomData==""){Ќ.CustomData=Αρ(true);continue;}
if(!Ќ.CustomData.Contains(Εδ))
if(!Ќ.CustomData.Contains(ΒА)) Ќ.CustomData=ΒА+Ќ.CustomData;}}void ΔЋ(){String ĉ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ф.Count;i++)
ф[i].WriteText(Αβ(φ.ΑЄ,"0",false));for(int i=0;i<Ї.Count;i++){φ η=φ.ў;String Щ="";Δε(Ї[i],out η,out Щ);Ї[i].WriteText(Αβ(η,Щ,i==0));}Echo(ĉ);for(int i=0;i<Ǥ.Count;i++){IMyTextPanel Ќ=Ǥ[i];String Ξ=Ќ.CustomData.ToUpper();if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Απ());
}}String ǽ(bool Ε){int ΕΧ=0;return ǽ(Ε,0,ref ΕΧ,false,1);}String ǽ(bool Ε,int Εύ,ref int ΕЊ,bool Βγ,int Ğ){String Β="";String ȣ="——————————————————\n";String ΖΕ="--------------------------------------------\n";if(ο==null||ή==Ι.Main||ή==Ι.Ƙ)
Β+="[PAM]-Controller | "+ό.Count+" Connected ships"+"\n"+ΓЋ()+ΓБ()+ΔΖ();else Β+=Ατ(ο)+"\n";Β+=ȣ;int Α=0;int Δ=ξ;Ǯ=0;if(Ψ!=""){Ζ(ref Β,Δ,Α++,Ε,Ψ);}else if(ή==Ι.Main){if(ο==null&&ό.Count>=1) ο=ό[0];String Ѕ="";if(Ζ(ref Ѕ,Δ,Α++,Ε," Send command to all")) Χ(Ι.Ƙ);
int Ȣ=0;for(int s=0;s<ό.Count;s++){if(Βγ) Ѕ+="\n";ε Γ=ό[s];if(Δ==Α||Δ==Α+1) ο=Γ;if(Α==Δ) Ȣ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+Ατ(Γ))){ο=Γ;Χ(Ι.Ȱ);ο.ǝ("",false,true,false);}if(Α==Δ) Ȣ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+(Βγ?"":"  ")+ΔΒ(Γ)))
{ο=Γ;Χ(Ι.ΒΖ);}}int ȩ=Ğ-2;ȩ+=Math.Max(0,(Εύ-1))*Ğ;Β+=Ɨ(ȩ,Ѕ,Ȣ,ref ΕЊ);}else if(ή==Ι.Ĺ||ή==Ι.Ƙ){Ι ΕΖ=ή==Ι.Ĺ?Ι.Ȱ:Ι.Main;ε ί=null;if(ή==Ι.Ĺ) ί=ο;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(ΕΖ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǿ(ί,"STOP"))){Н(ί,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǿ(ί,"CONT"))){Н(ί,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǿ(ί,"HOMEPOS"))){Н(ί,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǿ(ί,"JOBPOS"))){Н(ί,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǿ(ί,"FULL"))){Н(ί,"FULL");}if(ί!=null&&ί.Π==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǿ(ί,"UNDOCK"))){Н(ί,"UNDOCK");}Β+=ΓЇ(ί);}else if(ή==Ι.Ȱ){String Ƥ="";if(Ζ(ref Ƥ,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=Ƥ.Substring(0,Ƥ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Χ(Ι.Ĺ);}if(!ο.ΑЃ()&&!ο.Ɗ()) Ǯ++;if(ο.Ɗ()) Β+="No answer received...";
else Β+=ȃ(ο,ο.Ť);}else if(ή==Ι.ΒΖ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=ΖΕ;Β+=ȃ(ο,Ασ(ο));}if(!Ǉ) ξ=Math.Min(Ǯ-1,ξ);return Β;}String Ατ(ε Γ){if(Γ.ų!=VERSION) return Γ.θ+": Different version!";return Γ.θ+": "+ȃ(Γ,Γ.ΑΠ)+" "+Γ.Ʈ+"m";}String ΔΒ(ε Γ){String s=ƕ("",М(Γ.Ǣ,Γ.ǭ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.ё.Count;i++){if(i>=5) break;щ о=Γ.ё[i];s+=ΒΡ(ż(ΓЏ(о)),3)+" "+Αλ(о.І)+" ";}return s;}String ΓЏ(щ о){if(о.Њ.ToUpper()=="ORE"||о.Њ.ToUpper()=="INGOT"){String Βα=ΓЍ(о.θ.ToUpper());if(Βα!="") return Βα;}return о.θ;}String ȃ(ε Γ,String Β){if(Γ.ΑЃ()) return "No signal...("+ƅ((int)(DateTime.Now-Γ.ƽ).TotalSeconds)+")";
return Β;}String Αρ(bool ΖΦ){String Β="";if(ΖΦ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Δε(IMyTerminalBlock Ќ,out φ η,out String Щ){bool ȗ=true;String ĝ=ĸ(Ĳ(Ќ.CustomData.Split('\n'),"mode",ref ȗ)).ToUpper();String[]δ=ĝ.Split(':');String Ĝ=δ.Length>0?δ[0].Trim():"";Щ=δ.Length>1?δ[1].Trim():"";
η=φ.ў;if(Ĝ=="MAIN") η=φ.ΑЅ;else if(Ĝ=="MAINX") η=φ.ΑІ;else if(Ĝ=="MENU") η=φ.Ə;else if(Ĝ=="INVENTORY") η=φ.Ɖ;else if(Ĝ=="DEBUG") η=φ.ſ;if(η==φ.ў) Щ=ĝ;}String Αβ(φ η,String Щ,bool ΕΩ){int Ě=15;if(ΕΩ){Βδ=Ŋ;Ŋ=0;Βε=ŋ;ŋ=0;}String Β="";if(η==φ.ΑЅ){int Ћ=0;
Β=ǽ(false,Βδ,ref ΕΠ,true,Ě);if(!int.TryParse(Щ,out Ћ)) return Β;Ŋ=Math.Max(Ћ,Ŋ);Ћ--;return ΑБ(Ћ,Ě,Β);}else if(η==φ.ΑІ){int Ћ=0;Β=ǽ(false,Βε,ref ΕΡ,false,Ě);if(!int.TryParse(Щ,out Ћ)) return Β;ŋ=Math.Max(Ћ,ŋ);Ћ--;return ΑБ(Ћ,Ě,Β);}if(η==φ.ΑЄ){return ǽ(false,0,ref ΕΣ,true,Ě);
}else if(η==φ.Ɖ||η==φ.Ə||η==φ.ſ){ε Γ=ο;if(Щ!=""){Γ=Ƃ("",Щ);if(Γ==null) return "Unknown ship: "+Щ;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ȣ="—————————————————————\n";String ΑΥ="";String ƥ="";if(η==φ.Ə){ΑΥ="Menu";ƥ=ȃ(Γ,Γ.Ť);}
else if(η==φ.Ɖ){ΑΥ="Inventory";ƥ=ȃ(Γ,Ασ(Γ));}else if(η==φ.ſ){return ȃ(Γ,ΔΓ(Γ));}Β+=Γ.θ+" - "+Γ.Ʈ+"m | "+ż(""+ΑΥ)+"\n"+ȣ;Β+=ƥ;return Β;}return "Unknown command: "+Щ;}string ΔΓ(ε Γ){String s="";s+=Γ.ų+"\n";s+=Γ.θ+"\n";s+=Γ.Υ.X+"\n";s+=Γ.Υ.Y+"\n";
s+=Γ.Υ.Z+"\n";s+=Γ.ΑΠ+"\n";s+=Γ.Ψ+"\n";s+=Γ.ξ+"\n";s+=Γ.Ť.Length+"\n";s+=Γ.Ǣ+"\n";s+=Γ.ǭ+"\n";s+=Γ.ё.Count()+"\n";return s;}string Ασ(ε Γ){String s="";s+=ƕ("All",М(Γ.Ǣ,Γ.ǭ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.ё.Count();i++){щ о=Γ.ё[i];
s+=ƕ(о.θ,о.І,Γ.ΑΧ,30,8,10)+"\n";}return s;}ζ ΓЈ(ε Γ,bool ю,string ρ){ζ Ϊ=ζ.None;if(Γ==null){for(int i=0;i<ό.Count;i++){ζ ώ=ό[i].ǝ(ρ,false,ю,false);if(ώ==ζ.Ȯ) Ϊ=ώ;if(ώ==ζ.ķ) return ώ;if(ώ==ζ.ȁ) return ώ;}return Ϊ;}else return Γ.ǝ(ρ,false,ю,false);}String ǿ(ε Γ,String ρ)
{if(!ΒΗ(Γ,ρ)) return "";ζ ώ=ΓЈ(Γ,false,ρ);if(ώ==ζ.Ȯ) return "received!";if(ώ==ζ.ȁ) return "pending...";if(ώ==ζ.ķ) return "no answer!";return "";}String Ɨ(int ȩ,String Β,int Ȣ,ref int ѓ){String[]Η=Β.Split('\n');if(Ȣ>=ѓ+ȩ-1) ѓ++;ѓ=Math.Min(Η.Count()-1-ȩ,ѓ);if(Ȣ<ѓ+1) ѓ--;
ѓ=Math.Max(0,ѓ);String Ϊ="";for(int i=0;i<ȩ;i++){int ΒІ=i+ѓ;if(ΒІ>=Η.Count()) break;Ϊ+=Η[ΒІ]+"\n";}return Ϊ;}String ΑБ(int Ћ,int Ğ,String Β){String[]Η=Β.Split('\n');int ΑΟ=Ћ*Ğ;int ŕ=(Ћ+1)*Ğ;String Ϊ="";for(int i=ΑΟ;i<ŕ;i++){if(i>=Η.Count()) break;Ϊ+=Η[i]+"\n";
}return Ϊ;}String ż(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΒΡ(String s,int ΒЊ){if(s=="") return s;if(s.Length>ΒЊ) s=s.Substring(0,ΒЊ-1)+".";return s;}string ƕ(String Ǫ,float value,float Š,int ΒΧ,int ΒЂ,int Εό){float Ǳ=М(value,Š)*ΒΧ;
String Ņ="[";for(int i=0;i<ΒΧ;i++){if(i<=Ǳ) Ņ+="|";else Ņ+="'";}Ņ+="]";return Ņ+" "+ΒΡ(ż(Ǫ),ΒЂ).PadRight(ΒЂ)+"".PadRight(Εό)+Αλ(value);}String Αλ(float І){if(І>=1000000) return Math.Round(І/1000000f,І/1000000f<100?1:0)+"M";if(І>=1000) return Math.Round(І/1000f,І/1000f<100?1:0)+"K";
return ""+Math.Round(І);}String ƅ(int ğ){if(ğ>=60*60) return Math.Round(ğ/(60f*60f),1)+" h";if(ğ>=60) return Math.Round(ğ/60f,1)+" min";return ""+ğ+" s";}List<IMyCameraBlock>Ζτ=new List<IMyCameraBlock>();List<long>ΗΗ=new List<long>();bool Εώ=false;int ΕΛ=0;int Ζσ=0;
DateTime Ζρ=DateTime.Now;String ΕΙ="";void Γο(){Εώ=false;ΕΛ=0;ΕΙ="";}const String ΔЈ="[PAMTFC]";const float Ζη=5f;const float Ζΰ=2f;class ΒΞ{public long Ο;public Vector3 β,ǚ;public Vector3 Ŗ;public float ΖϏ;public String Μ="";public int ȍ=-1;public bool ΗΝ;public bool Ĕ;public float Х,н;
public long У=0;public int ΖЄ=0;public DateTime ĥ;}IMyBroadcastListener Ηη=null;long Εφ=0;List<ΒΞ>Ηγ=new List<ΒΞ>();Vector3 Ηι=new Vector3();float Ηκ=-1;Vector3 Ηδ=new Vector3();bool Ηζ=false;DateTime Ηε=DateTime.Now;const float Ζζ=15f;const float Ζε=10f;int Ηλ=0;String а="";
int ǯ=-1;bool Ζψ=false,Ζφ=false;Vector3 Ζχ=new Vector3();Vector3 Ηθ=new Vector3();IMyBroadcastListener Ηή=null;float Ηέ=-1;DateTime Ηί=DateTime.MinValue;List<int>ƹ=new List<int>();String Βϋ="";void Γώ(){String Ǒ=г+"|"+в+"|"+(int) Ȑ;if(Ǒ==Βϋ) return;Βϋ=Ǒ;Αγ(г,в,Ȑ,ƹ);
}void Αγ(int w,int h,О ΑΟ,List<int>Ʃ){Ʃ.Clear();if(ΑΟ==О.Ŀ){for(int k=0;k<w*h;k++){int ȟ=k/w;Ʃ.Add(ȟ*w+(ȟ%2==0?k-ȟ*w:w-1-(k-ȟ*w)));}return;}int Βϊ=(int) Math.Ceiling(w/2f),Βω=(int) Math.Ceiling(h/2f);int Εβ=w/2,Εα=h/2;int x=0,y=0,Ư=0,ư=-1,Š=Math.Max(w,h);
for(int i=0;i<Š*Š;i++){if(-Βϊ<x&&x<=Εβ&&-Βω<y&&y<=Εα) Ʃ.Add((y-1+Βω)*w+(x-1+Βϊ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ư;Ư=-ư;ư=t;}x+=Ư;y+=ư;}}bool ΔΜ(int a,int b,int w,float Ű,float Ů,float Х,float н){return a==b||(Math.Abs(a%w-b%w)*Ű<Х+0.5f&&Math.Abs(a/w-b/w)*Ů<н+0.5f);
}bool Γύ(){int n=Math.Max(0,г*в);if(а.Length==n) return false;а=new String('0',n);ǯ=-1;return true;}void Δω(int Ў,bool ψ){if(Ў<0||Ў>=а.Length) return;char[]h=а.ToCharArray();h[Ў]=ψ?'1':'0';а=new String(h);}const String ΔΦ="[PAMJOB]";const String ΔІ="[PAMTCC]";IMyBroadcastListener Ζϊ=null;
DateTime Ζϋ=DateTime.MinValue;DateTime Ζό=DateTime.Now;int Ζω=-1;bool ΖЋ=false;class Ȃ{public long Ο;public Vector3 β,З,Ё;}List<Ȃ>Џ=new List<Ȃ>();bool Ĉ=false;Vector3 Œ,ő,œ,Ŕ;long Ē=0;long ΓΓ=0;Ω ΓΒ=null;bool ΕΥ=false;Ω Γϊ(Vector3 β,Vector3 З,Vector3 Ё){MatrixD m=MatrixD.CreateWorld(β,З,Ё);
Ω w=new Ω(Ρ);w.Є=Vector3D.TransformNormal(Œ,m);w.Ϗ=Vector3D.TransformNormal(ő,m);w.Й=Vector3D.TransformNormal(œ,m);w.Υ=β+(Vector3) Vector3D.TransformNormal(Ŕ,m);w.ι=true;return w;}void ΔΧ(){if(Π==Ν.λ||!Ρ.ι) return;IMyShipConnector я=Ǿ(MyShipConnectorStatus.Connected);if(я==null||я.OtherConnector==null) return;
IMyShipConnector Г=я.OtherConnector;Vector3I Εε=new Vector3I((int) Ρ.Т.X,(int) Ρ.Т.Y,(int) Ρ.Т.Z);if(я.Position==Εε){MatrixD Ŝ=MatrixD.Transpose(Г.WorldMatrix);Œ=Vector3D.TransformNormal(Ј.WorldMatrix.Forward,Ŝ);ő=Vector3D.TransformNormal(Ј.WorldMatrix.Down,Ŝ);œ=Vector3D.TransformNormal(Ј.WorldMatrix.Left,Ŝ);
Ŕ=Vector3D.TransformNormal(я.GetPosition()-Г.GetPosition(),Ŝ);Ĉ=true;}if(Vector3.Distance(я.GetPosition(),Ρ.Υ)<5) Ē=Г.EntityId;List<IMyShipConnector>Ǩ=new List<IMyShipConnector>();И.GetBlocksOfType(Ǩ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ǩ.Count>0){Џ.Clear();for(int i=0;i<Ǩ.Count;i++){Ȃ p=new Ȃ();p.Ο=Ǩ[i].EntityId;p.β=Ǩ[i].GetPosition();p.З=Ǩ[i].WorldMatrix.Forward;p.Ё=Ǩ[i].WorldMatrix.Up;Џ.Add(p);}}ΓΒ=null;ΓΓ=0;if(Г.EntityId==Ē||!Ĉ) return;for(int i=0;i<Џ.Count;i++)
if(Џ[i].Ο==Г.EntityId){ΓΓ=Џ[i].Ο;ΓΒ=Γϊ(Џ[i].β,Џ[i].З,Џ[i].Ё);}}String Γω(){String s=(Ĉ?"1":"0")+":"+έ(Œ)+":"+έ(ő)+":"+
έ(œ)+":"+έ(Ŕ)+":"+Ē;for(int i=0;i<Џ.Count;i++)
s+=":"+Џ[i].Ο+","+έ(Џ[i].β)+","+έ(Џ[i].З)+","+έ(Џ[i].Ё);return s;}void Γψ(String s){Џ.Clear();Ĉ=false;Ē=0;String[]f=s.Split(':');if(f.Length<6) return;Ĉ=f[0]=="1";Œ=α(f[1]);ő=α(f[2]);œ=α(f[3]);Ŕ=α(f[4]);long.TryParse(f[5],out Ē);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ȃ p=new Ȃ();if(!long.TryParse(e[0],out p.Ο)) continue;p.β=α(e[1]);p.З=α(e[2]);p.Ё=α(e[3]);Џ.Add(p);}}const String ž="[PAMDCK]";const float Γι=120f;class ш{public long Γ;public int ǒ;public long б;public bool Βά;public DateTime Ж,Ę;
}IMyBroadcastListener Ő=null;List<ш>χ=new List<ш>();List<IMyShipConnector>є=new List<IMyShipConnector>();List<IMySensorBlock>ц=new List<IMySensorBlock>();List<IMyCameraBlock>ė=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ŷ=new List<MyDetectedEntityInfo>();long Д=0;int ƪ=0;
long Βέ=0;DateTime Βή=DateTime.Now;String ǥ="";int Βο=0;IMyCubeGrid ѕ=null;void Δς(){ѕ=Me.CubeGrid;И.GetBlocksOfType(є,q=>q.CubeGrid.IsSameConstructAs(ѕ)&&q.CustomName.Contains(dockPoolTag));И.GetBlocksOfType(ц,q=>q.CubeGrid.IsSameConstructAs(ѕ)&&q.CustomName.Contains(zoneTag));
И.GetBlocksOfType(ė,q=>q.CubeGrid.IsSameConstructAs(ѕ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ц.Count;i++){ц[i].DetectSmallShips=true;ц[i].DetectLargeShips=true;}for(int i=0;i<ė.Count;i++) ė[i].EnableRaycast=true;}IMyShipConnector ΒΛ(long Ο){if(Ο==0) return null;
IMyShipConnector c=И.GetBlockWithId(Ο) as IMyShipConnector;return c!=null&&ѕ!=null&&c.CubeGrid.IsSameConstructAs(ѕ)?c:null;}bool Ƌ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΑЂ(IMyShipConnector c,long Γ){for(int i=0;i<ė.Count;i++)
{IMyCameraBlock ą=ė[i];if(!ą.IsWorking||Vector3D.Distance(ą.GetPosition(),c.GetPosition())>10) continue;Vector3D ŕ=ą.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ą.CanScan(ŕ)) continue;MyDetectedEntityInfo ŗ=ą.Raycast(ŕ);if(!ŗ.IsEmpty()&&ŗ.EntityId!=ѕ.EntityId&&ŗ.EntityId!=Γ&&ŗ.Type!=MyDetectedEntityType.FloatingObject)
{ǥ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΔΤ(){for(int s=0;s<ц.Count;s++){if(!ц[s].IsWorking) continue;ŷ.Clear();ц[s].DetectedEntities(ŷ);for(int i=0;i<ŷ.Count;i++){MyDetectedEntityInfo e=ŷ[i];bool Εή=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Εή&&e.EntityId!=Д&&e.Velocity.Length()>0.5f){ǥ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Δη(ш r){IMyShipConnector б=ΒΛ(r.б);if(Ƌ(б)&&ΑЂ(б,r.Γ)) return б.EntityId;if(!r.Βά||б==null) return 0;IMyShipConnector ņ=null;double ΒΪ=0;for(int i=0;i<є.Count;i++)
{IMyShipConnector c=є[i];if(!Ƌ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),б.GetPosition());if(ņ==null||d<ΒΪ){if(ΑЂ(c,r.Γ)){ņ=c;ΒΪ=d;}}}return ņ==null?0:ņ.EntityId;}void ΔΗ(){ш Ș=null;long я=0;for(int Ũ=2;Ũ>=1&&Ș==null;Ũ--){ш ǰ=null;for(int i=0;i<χ.Count;i++)
{ш r=χ[i];if(r.ǒ!=Ũ) continue;if(Ũ==1){long c=Δη(r);if(c==0) continue;if(ǰ==null||r.Ж<ǰ.Ж){ǰ=r;я=c;}}else if(ǰ==null||r.Ж<ǰ.Ж) ǰ=r;}Ș=ǰ;}if(Ș==null){if(χ.Count>0&&ǥ=="") ǥ="No free dock";return;}Д=Ș.Γ;ƪ=Ș.ǒ;Βέ=Ș.ǒ==1?я:0;Βή=DateTime.Now;
}void ΔЌ(){if(ѕ==null) return;if(Ő==null) Ő=IGC.RegisterBroadcastListener(ž);int Ƨ=30;while(Ő.HasPendingMessage&&Ƨ-->0){String Ξ=Ő.AcceptMessage().Data as String;if(Ξ!=null) ΔΘ(Ξ);}Δο();if(++Βο<3) return;Βο=0;IGC.SendBroadcastMessage(ž,Γη());}void ΔΘ(String Ξ){try{String[]f=Ξ.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=χ.Count-1;i>=0;i--) if(χ[i].Γ==Γ) χ.RemoveAt(i);if(Д==Γ) Д=0;}else if(f[0]=="R"&&f.Length>=5){long б=long.Parse(f[3]);if(ΒΛ(б)==null) return;ш r=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Γ) r=χ[i];if(r==null){r=new ш();r.Γ=Γ;r.Ж=DateTime.Now;χ.Add(r);}
int ǒ=int.Parse(f[2]);if(r.ǒ!=ǒ) r.Ж=DateTime.Now;r.ǒ=ǒ;r.б=б;r.Βά=f[4]=="1";r.Ę=DateTime.Now;}}catch{}}void Δο(){for(int i=χ.Count-1;i>=0;i--)
if((DateTime.Now-χ[i].Ę).TotalSeconds>3) χ.RemoveAt(i);ш ň=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Д) ň=χ[i];if(Д!=0&&ň==null) Д=0;if(ň!=null&&(DateTime.Now-Βή).TotalSeconds>Γι){Д=0;ň.Ж=DateTime.Now;}ǥ="";if(Д==0&&χ.Count>0&&!ΔΤ()) ΔΗ();}String Γη()
{String γ="S;"+ѕ.EntityId+";"+Д+";"+ƪ+";"+Βέ+";";for(int i=0;i<χ.Count;i++) γ+=(i>0?",":"")+χ[i].Γ;γ+=";";for(int i=0;i<є.Count;i++){IMyShipConnector c=є[i];γ+=(i>0?":":"")+c.EntityId+","+έ(c.GetPosition())+","+έ(c.WorldMatrix.Forward)+","+έ(c.WorldMatrix.Up);
}γ+=";";for(int i=0;i<ц.Count;i++){IMySensorBlock z=ц[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();γ+=(i>0?":":"")+έ(z.GetPosition())+","+Math.Round(r,1);}return γ;}String ΓЋ()
{if(є.Count==0&&ц.Count==0) return "";int Βτ=0;for(int i=0;i<є.Count;i++) if(Ƌ(є[i])) Βτ++;return "Docks: "+Βτ+"/"+є.Count+" free"+(Д!=0?(ƪ==1?" | docking":" | undocking"):"")+
(χ.Count>1?" | "+(χ.Count-1)+" waiting":"")+(ǥ!=""?"\n"+ǥ:"")+"\n";}IMyBroadcastListener ΗΦ=null;int Βξ=0;DateTime ΖІ=DateTime.Now;DateTime ΗΡ=DateTime.MinValue;long ΗΣ=0;int ΗΤ=0;long ΗΥ=0;bool ΖΟ=false;int ΖЇ=0;List<Vector3>Ητ=new List<Vector3>();List<float>Ηυ=new List<float>();
bool ΖЅ=false;bool ΖЃ=false;Vector3 ΖЂ=new Vector3();void Δκ(){if(Βξ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ž,"D;"+Εφ);Βξ=0;ΖΟ=false;}const float ΔΥ=600f;class ъ{public long Ο;public int Ў=-1;public bool Γά;public float Х,н;public DateTime ΑΖ;}class й{public String Ǒ;public int w,h;
public float Ű,Ů;public String ψ;public List<int>Ǌ=new List<int>();public List<ъ>Љ=new List<ъ>();}List<й>д=new List<й>();IMyBroadcastListener ş=null;int ΒЁ=0;void ΔЍ(){if(ş==null) ş=IGC.RegisterBroadcastListener(ΔЈ);int Ƨ=50;while(ş.HasPendingMessage&&Ƨ-->0){String Ξ=ş.AcceptMessage().Data as String;
if(Ξ==null) continue;try{ΔΚ(Ξ);}catch{}try{ΔΛ(Ξ);}catch{}}for(int j=д.Count-1;j>=0;j--){й Μ=д[j];for(int i=Μ.Љ.Count-1;i>=0;i--)
if((DateTime.Now-Μ.Љ[i].ΑΖ).TotalSeconds>ΔΥ) Μ.Љ.RemoveAt(i);if(Μ.Љ.Count==0){д.RemoveAt(j);continue;}Γβ(Μ);}if(++ΒЁ<3) return;ΒЁ=0;for(int j=0;j<д.Count;j++) IGC.SendBroadcastMessage(ΔΦ,Γδ(д[j]));}void Γβ(й Μ){for(int i=0;i<Μ.Љ.Count;i++){ъ s=Μ.Љ[i];if(s.Γά&&(s.Ў<0||Μ.ψ[s.Ў]=='1')) s.Ў=ΔΪ(Μ,s);
}}String Γδ(й Μ){String γ="J;"+Μ.Ǒ+";"+Μ.ψ+";";bool Ж=true;for(int i=0;i<Μ.Љ.Count;i++){if((DateTime.Now-Μ.Љ[i].ΑΖ).TotalSeconds>5) continue;γ+=(Ж?"":",")+Μ.Љ[i].Ο+":"+Μ.Љ[i].Ў;Ж=false;}return γ;}void ΔΚ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;й Μ=null;for(int i=0;i<д.Count;i++) if(д[i].Ǒ==f[4]) Μ=д[i];if(Μ==null){Μ=new й();Μ.Ǒ=f[4];Μ.w=int.Parse(k[3]);Μ.h=int.Parse(k[4]);Μ.Ű=float.Parse(k[6]);Μ.Ů=float.Parse(k[7]);if(Μ.w<1||Μ.h<1) return;Αγ(Μ.w,Μ.h,(О) int.Parse(k[5]),Μ.Ǌ);
Μ.ψ=new String('0',Μ.w*Μ.h);д.Add(Μ);}String d=f[6];if(d.Length==Μ.ψ.Length){char[]c=Μ.ψ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Μ.ψ=new String(c);}long Ο=long.Parse(f[0]);ъ s=null;for(int i=0;i<Μ.Љ.Count;i++) if(Μ.Љ[i].Ο==Ο) s=Μ.Љ[i];
if(s==null){s=new ъ();s.Ο=Ο;Μ.Љ.Add(s);}s.ΑΖ=DateTime.Now;String[]w=f[10].Split(',');s.Γά=w[0]=="1";if(w.Length>=3){s.Х=float.Parse(w[1]);s.н=float.Parse(w[2]);}int ȍ=int.Parse(f[5]);if(ȍ>=0&&ȍ<Μ.ψ.Length&&(s.Ў<0||Μ.ψ[s.Ў]=='1')&&Μ.ψ[ȍ]!='1'&&Αϋ(Μ,ȍ,s)==null)
s.Ў=ȍ;}ъ Αϋ(й Μ,int Ў,ъ Ʋ){for(int i=0;i<Μ.Љ.Count;i++){ъ o=Μ.Љ[i];if(o!=Ʋ&&o.Ў>=0&&ΔΜ(Ў,o.Ў,Μ.w,Μ.Ű,Μ.Ů,Math.Max(o.Х,Ʋ.Х),Math.Max(o.н,Ʋ.н)))
return o;}return null;}int ΔΪ(й Μ,ъ s){for(int k=0;k<Μ.Ǌ.Count;k++){int c=Μ.Ǌ[k];if(Μ.ψ[c]!='1'&&Αϋ(Μ,c,s)==null) return c;}return-2;}String ΓБ(){String s="";for(int j=0;j<д.Count;j++){й Μ=д[j];int ψ=0;for(int i=0;i<Μ.ψ.Length;i++) if(Μ.ψ[i]=='1') ψ++;s+="Job "+(j+1)+": "+ψ+"/"+Μ.ψ.Length+" holes, "+Μ.Љ.Count+" ships\n";
}return s;}const float ΔЅ=8f;class к{public long Ο;public Vector3 β,ǚ,Ŗ;public float ƭ,Х,н,ǫ=-1;public bool Ĕ;public String Μ="";public int ΓΑ;public DateTime ĥ;}class ľ{public long a,b,Γέ;public DateTime Ę;}List<к>ϋ=new List<к>();List<ľ>у=new List<ľ>();int ΓΥ=0;void ΔΛ(String Ξ)
{String[]f=Ξ.Split(';');if(f.Length<9) return;long Ο=long.Parse(f[0]);к s=null;for(int i=0;i<ϋ.Count;i++) if(ϋ[i].Ο==Ο) s=ϋ[i];if(s==null){s=new к();s.Ο=Ο;ϋ.Add(s);}s.β=α(f[1]);s.ǚ=α(f[2]);s.ƭ=float.Parse(f[3]);s.Μ=f[4];s.ΓΑ=int.Parse(f[7].Split(',')[1]);s.Ŗ=α(f[8]);
s.Ĕ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.Х=float.Parse(w[1]);s.н=float.Parse(w[2]);s.Ĕ=w[3]=="1";}}s.ĥ=DateTime.Now;}bool ƞ(к s){return s.ǚ.LengthSquared()<0.25f&&(s.ΓΑ==3||s.Ŗ.LengthSquared()<0.25f);}Vector3 ΒΝ(к s){return s.ǚ.LengthSquared()>=1f?s.ǚ:s.Ŗ*2f;}
void ΔЏ(){Γρ();if(++ΓΥ<3) return;ΓΥ=0;IGC.SendBroadcastMessage(ΔІ,Γθ());}void Γρ(){for(int i=ϋ.Count-1;i>=0;i--)
if((DateTime.Now-ϋ[i].ĥ).TotalSeconds>3) ϋ.RemoveAt(i);for(int i=у.Count-1;i>=0;i--)
if((DateTime.Now-у[i].Ę).TotalSeconds>2) у.RemoveAt(i);for(int i=0;i<ϋ.Count;i++) ϋ[i].ǫ=-1;for(int i=0;i<ϋ.Count;i++)
for(int j=i+1;j<ϋ.Count;j++){к a=ϋ[i],b=ϋ[j];if(ƞ(a)||ƞ(b)) continue;Vector3 Εϋ=a.β+a.ǚ*(float)(DateTime.Now-a.ĥ).TotalSeconds;Vector3 ΕЀ=b.β+b.ǚ*(float)(DateTime.Now-b.ĥ).TotalSeconds;Vector3 ȧ=ΒΝ(a),Ȩ=ΒΝ(b);Vector3 Ȝ=ΕЀ-Εϋ;float В=Ȝ.Length();float ю=(a.ƭ+b.ƭ)*0.55f;
if(a.Ĕ&&b.Ĕ&&a.Μ!=""&&a.Μ==b.Μ&&a.Х>0&&b.Х>0)
ю=Math.Max(a.Х+b.Х,a.н+b.н)*0.5f+0.5f;Vector3 ΑΓ=Ȩ-ȧ;float ΓΚ=ΑΓ.LengthSquared();float t=ΓΚ>0.01f?-Vector3.Dot(Ȝ,ΑΓ)/ΓΚ:0;bool ΑΦ=В<ю;if(!ΑΦ&&(t<=0||t>ΔЅ||(Ȝ+ΑΓ*t).Length()>=ю)) continue;if(!ΑΦ&&ȧ.LengthSquared()>0.25f&&Ȩ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ȧ),Vector3.Normalize(Ȩ))<-0.7f)
continue;ľ p=null;for(int k=0;k<у.Count;k++)
if((у[k].a==a.Ο&&у[k].b==b.Ο)||(у[k].a==b.Ο&&у[k].b==a.Ο)) p=у[k];if(p==null){p=new ľ();p.a=a.Ο;p.b=b.Ο;p.Γέ=ΔЇ(a,b,ȧ,Ȩ,Ȝ,В);у.Add(p);}p.Ę=DateTime.Now;к y=p.Γέ==a.Ο?a:b;float ΒЅ=ΑΦ||t<4?0:Math.Max(1,y.ǚ.Length()*0.5f);if(y.ǫ<0||ΒЅ<y.ǫ) y.ǫ=ΒЅ;
}}long ΔЇ(к a,к b,Vector3 ȧ,Vector3 Ȩ,Vector3 Ȝ,float В){if(a.Ο==Д) return b.Ο;if(b.Ο==Д) return a.Ο;Vector3 n=В>0.01f?Ȝ/В:new Vector3(1,0,0);float ΓΠ=Vector3.Dot(ȧ,n),ΓΤ=Vector3.Dot(Ȩ,-n);if(ΓΠ-ΓΤ>1) return a.Ο;if(ΓΤ-ΓΠ>1) return b.Ο;return a.Ο>b.Ο?a.Ο:b.Ο;
}String Γθ(){String γ="C;";bool Ж=true;for(int i=0;i<ϋ.Count;i++){if(ϋ[i].ǫ<0) continue;γ+=(Ж?"":",")+ϋ[i].Ο+":"+Math.Round(ϋ[i].ǫ,1);Ж=false;}return γ;}String ΔΖ(){int ΒЎ=0,Ƹ=0;for(int i=0;i<ϋ.Count;i++){if(!ƞ(ϋ[i])) ΒЎ++;if(ϋ[i].ǫ>=0) Ƹ++;}if(ϋ.Count==0) return "";
return "Traffic: "+ΒЎ+" moving"+(Ƹ>0?", "+Ƹ+" giving way":"")+"\n";}void Δύ(string Ѝ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}