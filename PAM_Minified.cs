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
const float dockRequestDistance = 300f;//[m] ships ask for clearance this close to home, then wait outside the zone

const int gyroSpeedSmall = 15; //[RPM] small ship
const int gyroSpeedLarge = 5; //[RPM] large ship
const int generalSpeedLimit = 100; //[m/s] 0 = no limit (if set to 0 ignore unreachable code warning)
const float dockingSpeed = 0.5f; //[m/s]
const float jobAreaSpeed = 10f;//[m/s] near the job area (between holes, backing out, arriving, leaving)

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
const float followPathDock = 2f; //stop following path, fly direct to target
const float followPathJob = 1f; //same as dock
const float useDockDirectionDist = 1f; //Override waypoint direction, use docking dir
const float useJobDirectionDist = 0f; //same as dock

//other distances
const float wpReachedDist = 2f;//[m]
const float drillRadius = 1.4f;//[m] grinders and modded drills (vanilla drills use their real cut radius)

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

//Obstacle brake: the ship's cameras scan where it flies and stop it when something is in the way.
//A camera scans within 45° of where it faces: add cameras facing every direction the ship travels.
const bool obstacleBrake = true;
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size
const float obstacleTimeout = 10f;//[s] show "Path blocked" after holding this long

//Traffic between PAM ships (needs an antenna): lanes, spacing, right of way, co-op holes (IMPORT JOB)
const bool trafficEnabled = true;
const float laneOffset = 0.75f;//x ship size: keep right of the path; 0 = fly on the path
const float followGap = 1f;//x ship size: free space behind a PAM ship ahead

const bool resumeAfterRestart = true;//continue a running job after a server restart / world load

//Check if blocks are connected with conveyors
const bool checkConveyorSystem = false;//temporarily disabled because of a SE bug

//3D debug drawing (offline world + Digi's "(DevTool) Programmable Block DebugAPI" mod), see README
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ν{ļ,ΑΑ,Ƕ,Ǒ,ί}public enum Α{і,ǵ,ŗ,ȏ,ǣ,И,т,
ȭ,Ƅ,ȍ,œ,ŏ,ĵ,
ΑΓ,ȳ,ǹ,Œ,Βε,ΒΓ,
Δπ,Βδ,Ŕ,ΒΕ,Ľ}public enum Τ{ы,ƌ,Л,ΓϏ,ΑЏ,ΒΠ,ΒΡ,ſ,ƀ}public enum ǐ{None,Œ,і,Ζν}public enum Φ{Main,ΗΖ,ǌ,ǋ,Δλ,ΑЍ,
Γξ,ΖЁ,ΖЂ,ΖϏ,ΑΕ,ΗΛ,
Βά,Δα,Δε,ΗΝ,Βή}public enum ь{ΒΪ,Ǆ,Κγ,ΖЅ,ί}public enum ї{ΑΘ,Ζε}public enum є{ΒΩ,ΓІ,ΗΠ,ǈ}public enum З{ΔΡ,Ζκ,Γω,ΓЀ,ΒΑ,ΓБ,ΒΛ}public enum Ǡ{Γό,Γσ,Γτ}public enum а{Ƈ,ΙЎ,Ő}public enum τ{ΔΤ,ΔΨ,ΔΩ,ΗΑ,ΖБ,
ΔΣ,ΒΦ,ΒΥ,ΔΦ,
ΔΧ,ΔΥ}public enum ι{ȫ,Ė,Ȫ,ΑЌ,Γρ,Γς}public enum Ǹ{ΒΘ,ΑЋ,Ιρ}public enum ǒ{Βί,Ζο,ΚΘ,ΔΠ}public enum Ў{None,ƍ,ΑΒ,ΒΤ}public enum Ѐ{ΑΚ,ΑЊ,ΒΞ,ǈ,ΔΜ,ΒΟ,ΔΝ,Κλ,ΔΪ}public enum Ш{ļ,ΖЎ,ΖЍ,ΔΙ,ΔΒ,Γϊ,ΖЏ}class έ{public bool ν=false;public Vector3 Ε=new Vector3();
public Vector3 λ=new Vector3();public Vector3 μ=new Vector3();public Vector3 Є=new Vector3();public Vector3 ϋ=new Vector3();public Vector3 х=new Vector3();public float У=0;public float Μθ=0;public float[]Ȋ=null;public έ(){}public έ(έ θ){ν=θ.ν;Ε=θ.Ε;λ=θ.λ;μ=θ.μ;Є=θ.Є;ϋ=θ.ϋ;
х=θ.х;Ȋ=θ.Ȋ;}public έ(Vector3 Ε,Vector3 μ,Vector3 λ,Vector3 Є,Vector3 ϋ){this.Ε=Ε;this.λ=λ;this.μ=μ;this.Є=Є;this.У=0;this.ϋ=ϋ;}public void ΙΪ(List<IMyThrust>Βι,List<string>Đ){Ȋ=new float[Đ.Count];for(int i=0;i<Ȋ.Length;i++)
Ȋ[i]=-1;for(int i=0;i<Βι.Count;i++){string ǟ=Ζύ(Βι[i]);int Β=Đ.IndexOf(ǟ);if(Β!=-1)
Ȋ[Β]=М(Βι[i].MaxEffectiveThrust,Βι[i].MaxThrust);}}}class ΔΫ{public έ ζ=null;public List<Vector3>Ƹ=new List<Vector3>();public float ΑΦ=0;public float ΖΡ=0;public float Ɣ=0;public float ΕΛ=0;public Vector3 ΒЅ=new Vector3();}class Ɛ{public τ Ч=τ.ΔΤ;public float Ǽ=0;public float Ń=0;
public string ǽ="";public string Ȃ="";DateTime ΘΩ;public bool Εϋ=false;private bool ΔА=false;public bool Γχ(bool Ȇ){if(Ȇ){Ń=0;ΔА=false;return false;}ΔА=true;return Ń>Ǽ;}public void ΚΣ(){Γχ(true);Εϋ=false;}public void Κύ(){if(ΔА)
if((DateTime.Now-ΘΩ).TotalSeconds>1){Ń++;ΘΩ=DateTime.Now;}}public bool ΓЍ(){switch(Ч){case τ.ΔΨ:return true;case τ.ΔΩ:return true;}return false;}}class у{public String ΰ="";public String ω="";public int Џ=0;public а Ʀ=а.Ő;public у(String ΰ,String ω,int Џ,а Ʀ){this.ΰ=ΰ;this.ω=ω;
this.Џ=Џ;this.Ʀ=Ʀ;}}class Ιμ{public Ιμ(Vector3 ά,float υ){this.ά=ά;this.υ=υ;}public Vector3 ά;public float υ;}class ϊ{public DateTime Εα;public DateTime Ηρ;public String Θ="";public String ΰ="";public String Γλ="";public String Μ="";public String ΕЎ="";public String ΓΛ="";public String Εϊ="";
public Vector3 Ε=new Vector3();public List<у>А=new List<у>();public Ν Ξ=Ν.ļ;public Ў Ŀ;public float ȟ;public float ȗ;public int Г;public int ΕΔ=0;public int ΖΛ=0;public bool ΔΗ(){return(DateTime.Now-Εα).TotalSeconds>10;}public bool ΔΓ(){if(Ŀ!=Ў.ƍ) return false;return(DateTime.Now-Ηρ).TotalSeconds>=4;
}public ϊ(String Θ){this.Θ=Θ;}public Ў ĺ(String Ї,bool ΛЊ,bool з,bool ΚЍ){if(Ї==""&&!з) return Ў.None;if(Ŀ==Ў.ƍ&&ΔΓ())
Ŀ=Ў.ΒΤ;if(ΛЊ){Εϊ=Ї;Ŀ=Ў.ƍ;Ηρ=DateTime.Now;return Ŀ;}else if(з){Εϊ="";Ŀ=Ў.None;}else if(Εϊ==Ї){if(ΚЍ) Ŀ=Ў.ΑΒ;return Ŀ;}return Ў.None;}}public String Ιϊ(String Λα){switch(Λα){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΓΦ=new Vector3();Vector3 β=new Vector3();Vector3 ΒЈ=new Vector3();Vector3 Αλ=new Vector3();Vector3 ΑΩ=new Vector3();
DateTime Εδ=new DateTime();bool Ġ=true;int ΖΕ=0;bool ĥ=false;bool Ħ=false;bool Αρ=true;bool ΙΞ=false;bool ƣ=false;bool ΒЍ=false;float ď=0;float Ź=0;int Ō=0;int ΑЃ=0;int Αχ=0;int ΔЇ=0;float Ʃ=0;float ƕ=0;double Εξ=0;float ΑΞ=0;List<int>Ơ=new List<int>();List<int>ư=new List<int>();
Ν Ξ=Ν.ļ;int Е=0;float ȩ=0;float Ȩ=0;int Ũ=0;float Ĩ=0,ħ=0;float δ=0;float ΕЇ=0,ΕІ=0;IMyRemoteControl Ϊ;IMySensorBlock ό;List<IMyTimerBlock>ΖΗ=new List<IMyTimerBlock>();List<IMyShipConnector>Ǔ=new List<IMyShipConnector>();List<IMyThrust>đ=new List<IMyThrust>();List<IMyGyro>ΒЉ=new List<IMyGyro>();
List<IMyTerminalBlock>ύ=new List<IMyTerminalBlock>();List<IMyLandingGear>Ċ=new List<IMyLandingGear>();List<IMyReactor>Ŋ=new List<IMyReactor>();List<IMyConveyorSorter>Γέ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǩ=new List<IMyGasTank>();List<IMyTerminalBlock>ΑΛ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ƙ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ş=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ľ=new List<IMyBatteryBlock>();List<IMyTextPanel>Я=new List<IMyTextPanel>();List<IMyTextSurface>Ȥ=new List<IMyTextSurface>();List<IMyTextPanel>Ţ=new List<IMyTextPanel>();
IMyTerminalBlock Ǭ=null;έ Ρ=new έ();έ ΕΩ=new έ();έ Εέ=new έ();bool Ē=false;bool ƽ=false;bool ƾ=false;bool ΒЏ=false;List<String>Đ=new List<string>();List<έ>Ψ=new List<έ>();List<έ>ΑЈ=new List<έ>();int ΕΕ=0;double ΙΟ=0;έ Ζ=new έ();Vector3 Ф;Vector3 Ƞ;Τ η=Τ.ы;ї ũ=ї.ΑΘ;int Б=0;
int Ю=0;double Ακ=0;bool Βω=false;Ɛ Ќ=new Ɛ();Ɛ Ѝ=new Ɛ();є ѝ=є.ΒΩ;bool ō=true;ї ȣ=ї.ΑΘ;int њ=3;int љ=3;int ф=30;Ǡ ǔ=Ǡ.Γό;bool Ĵ=true;bool Ƣ=true;bool Ɯ=true;bool ź=false;З л=З.ΔΡ;bool ĳ=true;float Ȟ=90;float ǚ=20;float ц=5;float Ǜ=20;float Ȓ=0.70f;float ǂ=1.50f;
float ǁ=2.50f;float ž=0f;float ť=0f;Α Ω;Τ Ǚ;Α Αό;ΔΫ ς=null;int Ѕ=0;int Ђ=0;int и=0;int ī=0;int ŷ=30;int р=0;bool Ж=true;Vector3 О;double й=0;bool ΓΨ=false;int ΖΥ=0;int Њ=0;int Δό=0;int ȡ=0;int Θρ=0;Vector3 Ασ=new Vector3();float ћ=0;float щ=0;int Ž=0;έ Γή=null;
float Εβ=0;float Βλ=0;float п=0;float ŭ=0;bool ΕΫ=false;bool Ż=false;bool ΑЄ=false;bool ģ=false;bool ě=false;DateTime ΛΝ=new DateTime();έ Š=null;έ Ηω=null;float ā=0;ǐ ȓ;float Ĥ=0;float ż=0;String Ει="";float ΓΒ=0;float ȗ=0;float ȟ=0;List<у>Ȁ=new List<у>();float м=0;
bool Αω=false;bool ΙΓ=false;bool ΘЀ=false;bool Γγ=false;bool ΘЁ=false;float Εμ=0;float Εω=0;Vector3 ΗЍ=new Vector3();Vector3 Ϗ=new Vector3();Vector3 Γδ=new Vector3(1,1,1);float ΖΔ=1;Vector3 ΑЀ=new Vector3();bool ΒЎ=false;bool ΕΡ=false;bool Εζ=false;bool Ηέ=false;float Βχ=0;
float Βη=2;Vector3 ΘБ;Vector3 ΖΑ;Vector3 ΕБ;float[,]ȋ=new float[3,2];Dictionary<String,float[,]>ǰ=new Dictionary<string,float[,]>();Dictionary<String,float>ΙΜ=new Dictionary<String,float>();int ΔЀ=0;Dictionary<String,float[]>ĭ=new Dictionary<string,float[]>();float Εό;bool Βϋ=false;
int Βκ=0;int Ň=0;int ΒЋ=0;int Ŧ=0;String Μ="";int[]Θζ=new int[Enum.GetValues(typeof(Φ)).Length];bool Επ=false;int Ů=0;int Г=0;Φ Ё=Φ.Main;int ΜΙ=0,ΜΚ=0,ΜΛ=0,ΜΜ=0;int ΛЅ=0,ΛІ=0;bool Ɨ=false;bool Ηή=false;String Ƶ="";String ΔІ="";IMyBroadcastListener Ǻ=null;
bool ΗЌ=true;List<ϊ>ώ=null;ϊ П=null;int Βρ=0,Βς=0;int Ητ=0,Ηυ=0;int ΛΑ=0,ΛΒ=0,ΛΓ=0;String Λε="";Exception Ī=null;bool ΓΧ=false;Τ Ʋ=Τ.ы;bool Ηό=false;bool ΘΧ=true;bool ΕЂ=false;String ΙΝ="";bool ΑЂ=false;bool ΕЍ=true;int ΒЌ=0;int[]Ǯ=null;Vector3 Βν=new Vector3();
bool ΘΑ=false;bool ǡ(IMyTerminalBlock Ϋ)=>Ϋ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓЎ(Me,controllerTag,true))
Ξ=Ν.Ǒ;ο(Φ.Main);if(Ξ!=Ν.Ǒ){Μ="Welcome to [PAM]!";ǒ ΕЁ=ΚΤ();if(Ξ==Ν.ļ){List<IMyShipDrill>ΗЉ=new List<IMyShipDrill>();List<IMyShipGrinder>ΘΒ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗЉ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΘΒ,q=>q.CubeGrid==Me.CubeGrid);if(ΗЉ.Count>0)
{Ξ=Ν.ΑΑ;Μ="Miner mode enabled!";}else if(ΘΒ.Count>0){Ξ=Ν.Ƕ;Μ="Grinder mode enabled!";}else{Ξ=Ν.ί;ȱ(ь.ί);}}if(ΕЁ==ǒ.Βί) Αρ=false;if(ΕЁ==ǒ.Ζο) Μ="Data restore failed!";if(ΕЁ==ǒ.ΔΠ) Μ="New version, wipe data";}}void Main(string Ъ,UpdateType ΜΥ){try{if(Ī!=null){if(Ъ.Trim().ToUpper()=="RETRY")
{Ī=null;Ġ=true;Ō=0;if(Ξ!=Ν.Ǒ) І();Μ="Restarted after error";}else{ΙІ();ΗΔ();return;}}Ħ=(ΜΥ&UpdateType.Update10)!=0;if(Ħ) ΖΕ++;ĥ=ΖΕ>=10;if(ĥ) ΖΕ=0;if(Ħ){Βκ++;if(Βκ>4) Βκ=0;Αχ=(int) Math.Max(0,10-(DateTime.Now-Εδ).TotalSeconds);}if(Ъ!="") Λε=Ъ;if(Ξ!=Ν.Ǒ)
Κβ(Ъ);else
Ιι(Ъ);Επ=false;try{int Ηϊ=Runtime.CurrentInstructionCount;float Ŷ=М(Ηϊ,Runtime.MaxInstructionCount);if(Ŷ>0.90) Μ="Max. instructions >90%";if(Ŷ>Ʃ) Ʃ=Ŷ;if(Βϋ){Ơ.Add(Ηϊ);while(Ơ.Count>10) Ơ.RemoveAt(0);ƕ=0;for(int i=0;i<Ơ.Count;i++) ƕ+=Ơ[i];ƕ=М(М(ƕ,Ơ.Count),Runtime.MaxInstructionCount);
double Εγ=Runtime.LastRunTimeMs;if(ΒЍ&&Εγ>Εξ) Εξ=Εγ;ư.Add((int)(Εγ*1000f));while(ư.Count>10) ư.RemoveAt(0);ΑΞ=0;for(int i=0;i<ư.Count;i++) ΑΞ+=ư[i];ΑΞ=М(ΑΞ,ư.Count)/1000f;}}catch{Ʃ=0;}}catch(Exception e){Ī=e;}}void Κβ(string Ъ){bool ΓΩ=false;String Ηι="";if(Е<=1&&!ΔΖ(Ъ))
ΗΕ(Ъ);int Ερ=50;while(Ǻ!=null&&Ǻ.HasPendingMessage&&Ερ-->0){MyIGCMessage ε=Ǻ.AcceptMessage();String Η=ε.Data as String;if(Η==null) continue;String Ȣ="";if(Е<=1&&ΗΓ(ref Η,out Ȣ,out Ηι)&&Ȣ==BROADCAST_SELF_ID){ΗΕ(Η);ΓΩ=true;break;}}bool ΘΠ=ΒЍ&&Ω==Α.і&&!ƣ&&!ΓΩ&&Ō==0&&!ΒЏ;
if(ĥ&&Ω!=Α.і) ΓΩ=true;if((Ħ&&!ΘΠ)||(ĥ&&ΘΠ)){if(Ō==0&&(Αχ<=0||Ġ)){ΙΞ=Е>0;Е=0;Ō=1;Ȳ();ΚΩ();ΖЃ();Ȭ("Scan 1");}else if(Ō==1){Ō=2;Ȳ();ΚΪ();Ȭ("Scan 2");}else if(Ō==2){Ō=0;Ȳ();ΚΫ();Κά();if(Е<=1) ΖЌ();Ȭ("Scan 3");Εδ=DateTime.Now;if(Е<=1&&Αρ)
ΓΦ=ΒΙ(Ϊ,Ϊ.CenterOfMass);Αρ=false;if(Ġ){Ġ=false;bool ΜΪ=η==Τ.Л;І();if(ΘΧ&&resumeAfterRestart&&ΜΪ&&Е<=1) ΕЂ=true;}ΘΧ=false;if(ΙΞ&&Е==0) Μ="Setup complete";}else{if(η==Τ.Л&&Ξ!=Ν.ί){Ȳ();ΙΣ();Ȭ("Inv balance");}Ȳ();switch(ΑЃ){case 0:Κο();break;case 1:Κφ();break;case 2:Κξ();break;
case 3:ΚЁ();break;case 4:Κυ();break;case 5:Ιή();break;case 6:Ιά(Ϊ);break;}Ȭ("Update: "+ΑЃ);ΑЃ++;if(ΑЃ>6){ΑЃ=0;ΒЍ=true;if(Ʋ!=Τ.ы){switch(Ʋ){case Τ.ſ:ķ();break;case Τ.ƀ:ķ();break;case Τ.Л:ķ();break;case Τ.ΒΠ:ΒΖ();break;case Τ.ΒΡ:ŕ();break;}Ʋ=Τ.ы;}}}if(!Ġ){if(!ȯ(Ϊ,true))
{Ϊ=null;Αρ=true;Е=2;}if(Е>=2&&Ω!=Α.і)
І();if(Е<=1){Ź=Ϊ.CalculateShipMass().PhysicalMass;ď=(float) Ϊ.GetShipSpeed();β=Κν(Ϊ,ΓΦ);ΒЈ=Ϊ.WorldMatrix.Forward;Αλ=Ϊ.WorldMatrix.Left;ΑΩ=Ϊ.WorldMatrix.Down;Κϊ();Κώ();Κς();if(ΕЂ){ΕЂ=false;ķ();if(Ω!=Α.і) Μ="Job resumed after restart";}if(Ω!=Α.і){ƣ=false;ΗΘ(false);Ζγ(false);
String ΛА=ΓЉ(Ω)+" "+(int) Ω;Ȳ();Κψ();Δο(false);Ȭ(ΛА);ΚϏ();Κω();Ȳ();Κτ();Ȭ("Thruster");Ȳ();ΗΩ();Ȭ("Gyroscope");}else{if(ƣ){if(ΒΝ()){с(ΑΩ,ΒЈ,Αλ,0.25f,true);ΗΩ();Μ="Aligning to planet: "+Math.Round(Βχ-0.25f,2)+"°";if(ΒЎ) ΓЌ(true,true);}else ΓЌ(true,true);}}ΑЂ=false;}}}Ȳ();ΗΔ();Ȭ("Print");
if(ΓΩ||ΔЇ<=0){Ȳ();ΙΤ(Ηι);Ȭ("Broadcast");ΔЇ=4;}else if(ĥ) ΔЇ--;}void ΗΕ(string Ъ){if(Ъ=="") return;var ψ=Ъ.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var Ї=ψ.ElementAtOrDefault(0);var Ā=ψ.ElementAtOrDefault(1);var ΑΝ=ψ.ElementAtOrDefault(2);var Ηΰ=ψ.ElementAtOrDefault(3);String ĉ="Invalid argument: "+Ъ;
bool ΖΠ=false;switch(Ї){case "UP":this.ΔΛ(false);break;case "DOWN":this.ΔΚ(false);break;case "UPLOOP":this.ΔΛ(true);break;case "DOWNLOOP":this.ΔΚ(true);break;case "APPLY":this.Ζα(true);break;case "MRES":Г=0;break;case "STOP":this.І();break;case "PATHHOME":{this.І();this.Δι();}break;
case "PATH":{this.І();this.Δι();Ρ.ν=true;}break;case "START":{this.І();Δθ();}break;case "ALIGN":{ΓЌ(!ƣ,false);}break;case "CONT":{this.І();this.ķ();}break;case "JOBPOS":{this.І();this.ŕ();}break;case "HOMEPOS":{this.І();this.ΒΖ();}break;case "FULL":{Αω=true;}break;
case "RESET":{ΓΧ=true;Е=2;}break;case "EXPORT":{Ις();}break;case "IMPORT":{ΙЌ(Ā=="JOB");}break;default:ΖΠ=true;break;}if(Ξ!=Ν.ί){switch(Ї){case "SHUTTLE":{Ιο();}break;case "CFGS":{if(!Ιη(Ā,ΑΝ,Ηΰ)) Μ=ĉ;}break;case "CFGB":{if(!Ιζ(Ā,ΑΝ)) Μ=ĉ;}break;case "CFGL":
{if(!Ř(ref Ȟ,true,Ѐ.ΒΞ,Ā,"")||!Ιθ(ΑΝ))
Μ=ĉ;}break;case "CFGE":{if(!Ř(ref ц,true,Ѐ.ΒΟ,Ā,"IG")||
!Ř(ref ǚ,true,Ѐ.ΔΜ,ΑΝ,"IG")||
!Ř(ref Ǜ,true,Ѐ.ΔΝ,Ηΰ,"IG"))
Μ=ĉ;}break;case "CFGA":{if(!Ř(ref Ȓ,false,Ѐ.ΑЊ,Ā,"")) Μ=ĉ;}break;case "CFGW":{if(!Ř(ref ǂ,false,Ѐ.ΑΚ,Ā,"")||
!Ř(ref ǁ,false,Ѐ.ΑΚ,ΑΝ,""))
Μ=ĉ;}break;case "NEXT":{Γν(false);}break;case "PREV":{Γν(true);}break;default:if(ΖΠ) Μ=ĉ;break;}}else{switch(Ї){case "UNDOCK":{ΑЂ=true;}break;default:if(ΖΠ) Μ=ĉ;break;}}}String Ιϋ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ξ!=Ν.ί)
Γ+="[SHUTTLE] Enable shuttle mode\n"+"[NEXT] Next hole\n"+"[PREV] Previous hole\n\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ιο(){І();Ξ=Ν.ί;ȱ(ь.ί);Ζ.ν=false;Ρ.ν=false;ό=null;ύ.Clear();η=Τ.ы;}void ΓЌ(bool С,bool φ){if(!С) Μ="Aligning canceled";if(φ) Μ="Aligning done";if(φ||!С){ƣ=false;ΗΡ();Βέ(false,0,0,0,0);return;}if(ΒΝ()) ƣ=true;}bool Ιη(String Μά,String ΛΪ,String ΛΗ)
{bool ΜΨ=η==Τ.Л;int w,h,d;if(int.TryParse(Μά,out w)&&int.TryParse(ΛΪ,out h)&&int.TryParse(ΛΗ,out d)){this.І();њ=w;љ=h;ф=d;Ŝ(false);ΑΙ(false,false);if(ΜΨ) ķ();return true;}return false;}bool Ιθ(String ΑΜ){if(ΑΜ=="ON"){Ĵ=true;return true;}if(ΑΜ=="OFF"){Ĵ=false;return true;}
return false;}bool Ιζ(String ΗЇ,String Βψ){bool ċ=true;if(ΗЇ=="HOME") ō=true;else if(ΗЇ=="STOP") ō=false;else ċ=false;if(Βψ=="HOME") ѝ=є.ΒΩ;else if(Βψ=="STOP") ѝ=є.ΗΠ;else if(Βψ=="JOB") ѝ=є.ΓІ;else if(Βψ=="IG") ѝ=є.ǈ;else ċ=false;return ċ;}void ο(Φ я){Θζ[(int) Ё]=Г;
Г=Θζ[(int) я];if(я==Φ.ΑΕ) Г=0;Ё=я;if(Ξ!=Ν.Ǒ)
ś(Ё==Φ.ǌ,false,0,0);Επ=true;}void ΔΛ(bool Εη){if(Г>0) Г--;else if(Εη) Г=Ů-1;}void ΔΚ(bool Εη){if(Г<Ů-1) Г++;else if(Εη) Г=0;}String ȱ(ь ε){switch(ε){case ь.ΒΪ:Μ="Job is running";break;case ь.Ǆ:Μ="Connector not ready!";break;case ь.Κγ:Μ="Ship modified, path outdated!";break;
case ь.ΖЅ:Μ="Interrupted by player!";break;case ь.ί:Μ="Shuttle mode enabled!";break;}return "";}String ΙЁ(ї ά){switch(ά){case ї.ΑΘ:return "Top-Left";case ї.Ζε:return "Center";default:return "";}}String Ιχ(Ǡ Д){switch(Д){case Ǡ.Γσ:return "Auto"+(Ξ==Ν.ΑΑ?" (Ore)":"");case Ǡ.Γτ:return "Auto (+Stone)";
case Ǡ.Γό:return "Default";default:return "";}}String ΓЇ(Τ Ј){switch(Ј){case Τ.ы:return "No job";case Τ.ƌ:return "Job paused";case Τ.Л:return "Job active";case Τ.ſ:return "Job active";case Τ.ƀ:return "Job active";case Τ.ΓϏ:return "Job done";case Τ.ΑЏ:return "Job changed";case Τ.ΒΠ:return "Move home";
case Τ.ΒΡ:return "Move to job";}return "";}String Ζτ(є ΚЌ){switch(ΚЌ){case є.ΒΩ:return "Return home";case є.ΓІ:return "Fly to job pos";case є.ΗΠ:return "Stop";case є.ǈ:return "Ignore";}return "";}String Ιω(З Д){switch(Д){case З.ΔΡ:return "Off";case З.ΓЀ:return "Drop pos (Stone) ";case З.ΒΑ:return "Drop pos (Sto.+Ice)";
case З.Ζκ:return "Cur. pos (Stone)";case З.Γω:return "Cur. pos (Sto.+Ice)";case З.ΓБ:return "In motion (Stone)";case З.ΒΛ:return "In motion (Sto.+Ice)";}return "";}String Ζς(ǐ Ј){switch(Ј){case ǐ.None:return "No batteries";case ǐ.Œ:return "Charging";case ǐ.Ζν:return "Discharging";}return "";}
String ΓЉ(Α Ј){String Μζ=Ξ==Ν.ί?"target":"job";switch(Ј){case Α.і:return "Idle";case Α.ǵ:return "Flying to XY position";case Α.ŗ:return Ξ==Ν.Ƕ?"Grinding":"Mining";case Α.ȏ:return "Returning";case Α.Ŕ:return "Flying to drop pos";case Α.ǣ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.т:return "Flying to job area";case Α.ȭ:return "Flying to path";case Α.Ƅ:return "Flying to job position";case Α.ȍ:return "Approaching dock";case Α.œ:return "Docking";case Α.ŏ:return "Aligning";case Α.ĵ:return "Aligning";case Α.ΑΓ:return "Retry docking";case Α.ȳ:return "Unloading";case Α.Ľ:return ΙΝ;
case Α.ǹ:return "Undocking";case Α.Œ:return "Charging batteries";case Α.Βε:return "Waiting for uranium";case Α.ΒΓ:return "Filling up hydrogen";case Α.Δπ:return "Waiting for ejection";case Α.Βδ:return "Waiting for ejection";case Α.ΒΕ:return "Flying to drop pos";}return "";}String Ζώ(τ Ч){switch(Ч)
{case τ.ΔΤ:return "On \"Undock\" command";case τ.ΔΨ:return "On player entered cockpit";case τ.ΗΑ:return "On ship is full";case τ.ΖБ:return "On ship is empty";case τ.ΔΩ:return "On time delay";case τ.ΒΦ:return "On batteries empty(<25%)";case τ.ΒΥ:return "On batteries empty(=0%)";case τ.ΔΣ:return "On batteries full";
case τ.ΔΧ:return "On hydrogen empty(<25%)";case τ.ΔΥ:return "On hydrogen empty(=0%)";case τ.ΔΦ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Θό,int ņ,bool Λ,String Ū){Ů+=1;if(Θό==ņ) Ū=">"+Ū+(Βκ>=2?" .":"");else Ū=" "+Ū;Γ+=Ū+"\n";return Θό==ņ&&Λ;}String Ζα(bool Λ)
{int Β=0;int Κ=Г;Ů=0;String ƴ="———————————————\n";String ƺ="--------------------------------------------\n";String Γ="";Γ+=ΓЇ(η)+" | "+(Ρ.ν?"Ready to dock":"No dock")+"\n";Γ+=ƴ;double ȉ=Math.Max(Math.Round(this.й),0);if(Ё==Φ.Main){bool ň=Ξ==Ν.ί;if(Π(ref Γ,Κ,Β++,Λ," Record path & set home")) Δι();
if(Ξ==Ν.ΑΑ)
if(Π(ref Γ,Κ,Β++,Λ," Setup mining job")) ο(Φ.ǌ);if(Ξ==Ν.Ƕ)
if(Π(ref Γ,Κ,Β++,Λ," Setup grinding job")) ο(Φ.ǌ);if(Ξ==Ν.ί)
if(Π(ref Γ,Κ,Β++,Λ," Setup shuttle job")) ο(Φ.Δε);if(Π(ref Γ,Κ,Β++,Λ," Continue job")) ķ();if(Π(ref Γ,Κ,Β++,Λ," Fly to home position")) ΒΖ();if(Π(ref Γ,Κ,Β++,Λ," Fly to job position")) ŕ();if(Π(ref Γ,Κ,Β++,Λ," Behavior settings"))
if(ň) ο(Φ.Βή);else ο(Φ.ΑЍ);if(Π(ref Γ,Κ,Β++,Λ," Info")) ο(Φ.ΖЁ);if(Ξ!=Ν.ί)
if(Π(ref Γ,Κ,Β++,Λ," Help")) ο(Φ.ΖϏ);}else if(Ё==Φ.ǌ){double ΜΫ=Math.Round(њ*ȩ,1);double ΛΩ=Math.Round(љ*Ȩ,1);String К="";if(Π(ref К,Κ,Β++,Λ," Start new job!")) Δθ();if(Π(ref К,Κ,Β++,Λ," Change current job")){ΑΙ(false,false);ο(Φ.Main);}if(Π(ref К,Κ,Β++,Λ," Width + (Width: "+њ+" = "+ΜΫ+"m)")){Ɓ(ref њ,5,20,1);Ŝ(true);}
if(Π(ref К,Κ,Β++,Λ," Width -")){Ɓ(ref њ,-5,20,-1);Ŝ(true);}if(Π(ref К,Κ,Β++,Λ," Height + (Height: "+љ+" = "+ΛΩ+"m)")){Ɓ(ref љ,5,20,1);Ŝ(true);}if(Π(ref К,Κ,Β++,Λ," Height -")){Ɓ(ref љ,-5,20,-1);Ŝ(true);}if(Π(ref К,Κ,Β++,Λ," Depth + ("+(ǔ==Ǡ.Γό?"Depth":"Min")+": "+ф+"m)")){Ɓ(ref ф,5,50,2);Ŝ(true);}
if(Π(ref К,Κ,Β++,Λ," Depth -")){Ɓ(ref ф,-5,50,-2);Ŝ(true);}if(Π(ref К,Κ,Β++,Λ," Depth mode: "+Ιχ(ǔ))){ǔ=ĸ(ǔ);}if(Π(ref К,Κ,Β++,Λ," Start pos: "+ΙЁ(ȣ))){ȣ=ĸ(ȣ);}if(Ξ==Ν.Ƕ&&ǔ==Ǡ.Γτ) ǔ=ĸ(ǔ);Γ+=Δΰ(8,К,Κ,ref ΛЅ);}else if(Ё==Φ.Δε){float[]Ηώ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Κ,Β++,Λ," Next")) ο(Φ.ΗΝ);if(Π(ref Γ,Κ,Β++,Λ," Back")) ο(Φ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Κ,Β++,Λ," - "+Ζώ(Ќ.Ч))) Ќ.Ч=ĸ(Ќ.Ч);if(!Ќ.ΓЍ()) Γ+="\n";else if(Π(ref Γ,Κ,Β++,Λ," - Delay: "+ΒΗ((int) Ќ.Ǽ))) Ќ.Ǽ=Ζλ(Ќ.Ǽ,Ηώ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Κ,Β++,Λ," - "+Ζώ(Ѝ.Ч))) Ѝ.Ч=ĸ(Ѝ.Ч);
if(!Ѝ.ΓЍ()) Γ+="\n";else if(Π(ref Γ,Κ,Β++,Λ," - Delay: "+ΒΗ((int) Ѝ.Ǽ))) Ѝ.Ǽ=Ζλ(Ѝ.Ǽ,Ηώ);}else if(Ё==Φ.ΗΝ){if(Π(ref Γ,Κ,Β++,Λ," Start job!")) Δθ();if(Π(ref Γ,Κ,Β++,Λ," Back")) ο(Φ.Δε);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Κ,Β++,Λ," = "+(Ќ.ǽ!=""?Ќ.ǽ:"-"))) Ќ.ǽ=ΒΣ(ref ΜΙ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Κ,Β++,Λ," = "+(Ќ.Ȃ!=""?Ќ.Ȃ:"-"))) Ќ.Ȃ=ΒΣ(ref ΜΛ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Κ,Β++,Λ," = "+(Ѝ.ǽ!=""?Ѝ.ǽ:"-"))) Ѝ.ǽ=ΒΣ(ref ΜΚ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Κ,Β++,Λ," = "+(Ѝ.Ȃ!=""?Ѝ.Ȃ:"-"))) Ѝ.Ȃ=ΒΣ(ref ΜΜ);
}else if(Ё==Φ.ǋ){String Θή=Ȟ+" %";if(ĥ) Ň++;if(Ň>1){Ň=0;ΒЋ++;if(ΒЋ>1) ΒЋ=0;}bool[]ΕЊ=new bool[]{Ŋ.Count==0,ȓ==ǐ.None,Ǩ.Count==0};int ΙΘ=0;while(true){ΙΘ++;Ŧ++;if(Ŧ>ΕЊ.Length-1) Ŧ=0;if(ΙΘ>=ΕЊ.Length) break;if(!ΕЊ[Ŧ]) break;}bool ň=Ξ==Ν.ί;if(!ň&&Ĵ&&м!=-1&&ΒЋ==0)
Θή=м<1000000?Math.Round(м)+" Kg":Math.Round(м/1000)+" t";if(Π(ref Γ,Κ,Β++,Λ," Stop!")){І();ο(Φ.Main);}if(Π(ref Γ,Κ,Β++,Λ," Behavior settings"))
if(!ň) ο(Φ.ΑЍ);else ο(Φ.Βή);if(!ň){if(Π(ref Γ,Κ,Β++,Λ," Next hole")) Γν(false);}else if(Π(ref Γ,Κ,Β++,Λ," Undock")) ΑЂ=true;Γ+=ƺ;if(!ň) Γ+="Progress: "+Math.Round(Ακ,1)+" %\n";Γ+="State: "+ΓЉ(Ω)+" "+ȉ+"m \n";Γ+="Load: "+ΓΒ+" % Max: "+Θή+" \n";if(Ŧ==0) Γ+="Uranium: "+(Ŋ.Count==0?"No reactors":Math.Round(ż,1)+"Kg "+(ц==-1?"":" Min: "+ц+" Kg"))+"\n";
if(Ŧ==1) Γ+="Battery: "+(ȓ==ǐ.None?Ζς(ȓ):ā+"% "+(ǚ==-1||ň?"":" Min: "+ǚ+" %"))+"\n";if(Ŧ==2) Γ+="Hydrogen: "+(Ǩ.Count==0?"No tanks":Math.Round(Ĥ,1)+"% "+(Ǜ==-1||ň?"":" Min: "+Ǜ+" %"))+"\n";}else if(Ё==Φ.ΑЍ){String К="";if(Π(ref К,Κ,Β++,Λ," Back")){if(η==Τ.Л) ο(Φ.ǋ);else ο(Φ.Main);}
if(Π(ref К,Κ,Β++,Λ," Max load: "+Ȟ+"%")) ѓ(ref Ȟ,Ȟ<=80?-10:-5,Ѐ.ΒΞ,false);if(Π(ref К,Κ,Β++,Λ," Weight limit: "+(Ĵ?"On":"Off"))) Ĵ=!Ĵ;if(Π(ref К,Κ,Β++,Λ," Ejection: "+Ιω(л))) л=ĸ(л);if(Π(ref К,Κ,Β++,Λ," Toggle sorters: "+(ź?"On":"Off"))){ź=!ź;if(ź) ȑ(ΕЍ);}
if(Π(ref К,Κ,Β++,Λ," Unload ice: "+(ĳ?"On":"Off"))) ĳ=!ĳ;if(Π(ref К,Κ,Β++,Λ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ѓ(ref ц,(ц>5?-5:-1),Ѐ.ΒΟ,true);if(Π(ref К,Κ,Β++,Λ," Battery: "+(ǚ==-1?"Ignore":"Min "+ǚ+"%"))) ѓ(ref ǚ,-5,Ѐ.ΔΜ,true);if(Π(ref К,Κ,Β++,Λ," Hydrogen: "+(Ǜ==-1?"Ignore":"Min "+Ǜ+"%"))) ѓ(ref Ǜ,-10,Ѐ.ΔΝ,true);
if(Π(ref К,Κ,Β++,Λ," When done: "+(ō?"Return home":"Stop"))) ō=!ō;if(Π(ref К,Κ,Β++,Λ," On damage: "+Ζτ(ѝ))) ѝ=ĸ(ѝ);if(Π(ref К,Κ,Β++,Λ," Advanced...")) ο(Φ.Γξ);Γ+=Δΰ(8,К,Κ,ref ΛІ);}else if(Ё==Φ.Γξ){if(Π(ref Γ,Κ,Β++,Λ," Back")){if(η==Τ.Л) ο(Φ.ǋ);else ο(Φ.Main);}
if(Π(ref Γ,Κ,Β++,Λ,(Ξ==Ν.Ƕ?" Grinder":" Drill")+" inv. balancing: "+(Ƣ?"On":"Off"))) Ƣ=!Ƣ;if(Π(ref Γ,Κ,Β++,Λ," Enable"+(Ξ==Ν.Ƕ?" grinders":" drills")+": "+(Ɯ?"Fwd + Bwd":"Fwd"))) Ɯ=!Ɯ;if(Π(ref Γ,Κ,Β++,Λ," Work speed fwd.: "+ǂ+"m/s")) ѓ(ref ǂ,0.5f,Ѐ.ΑΚ,false);
if(Π(ref Γ,Κ,Β++,Λ," Work speed bwd.: "+ǁ+"m/s")) ѓ(ref ǁ,0.5f,Ѐ.ΑΚ,false);if(Π(ref Γ,Κ,Β++,Λ," Acceleration: "+Math.Round(Ȓ*100f)+"%"+(Ȓ>0.80f?" (risky)":""))) ѓ(ref Ȓ,0.1f,Ѐ.ΑЊ,false);if(Π(ref Γ,Κ,Β++,Λ," Width overlap: "+ž*100f+"%")) Ζζ(true,0.05f);if(Π(ref Γ,Κ,Β++,Λ," Height overlap: "+ť*100f+"%")) Ζζ(false,0.05f);
}else if(Ё==Φ.Βή){if(Π(ref Γ,Κ,Β++,Λ," Back")){if(η==Τ.Л) ο(Φ.ǋ);else ο(Φ.Main);}if(Π(ref Γ,Κ,Β++,Λ," Max load: "+Ȟ+"%")) ѓ(ref Ȟ,Ȟ<=80?-10:-5,Ѐ.ΒΞ,false);if(Π(ref Γ,Κ,Β++,Λ," Unload ice: "+(ĳ?"On":"Off"))) ĳ=!ĳ;if(Π(ref Γ,Κ,Β++,Λ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ѓ(ref ц,(ц>5?-5:-1),Ѐ.ΒΟ,true);
if(Π(ref Γ,Κ,Β++,Λ," Battery: "+(ǚ==-1?"Ignore":"Charge up"))) ǚ=(ǚ==-1?1:-1);if(Π(ref Γ,Κ,Β++,Λ," Hydrogen: "+(Ǜ==-1?"Ignore":"Fill up"))) Ǜ=(Ǜ==-1?1:-1);if(Π(ref Γ,Κ,Β++,Λ," On damage: "+Ζτ(ѝ))) ѝ=ĸ(ѝ);if(Π(ref Γ,Κ,Β++,Λ," Acceleration: "+Math.Round(Ȓ*100f)+"%"+(Ȓ>0.80f?" (risky)":""))) ѓ(ref Ȓ,0.1f,Ѐ.ΑЊ,false);
}else if(Ё==Φ.ΗΖ){double Λζ=0;if(Ψ.Count>0) Λζ=Vector3.Distance(Ψ.Last().Ε,β);if(Π(ref Γ,Κ,Β++,Λ," Stop path recording")) ΗΣ();if(Ξ!=Ν.ί){if(Π(ref Γ,Κ,Β++,Λ," Home: "+(Ē?"Use old home":(Ρ.ν?"Was set! ":"none ")))) Ē=!Ē;}else{if(Π(ref Γ,Κ,Β++,Λ," Connector 1: "+(Ē?"Use old connector":(Ρ.ν?"Was set! ":"none ")))) Ē=!Ē;
if(Π(ref Γ,Κ,Β++,Λ," Connector 2: "+(ƽ?"Use old connector":(Ζ.ν?"Was set! ":"none ")))) ƽ=!ƽ;}if(Π(ref Γ,Κ,Β++,Λ," Path: "+(ƾ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ƾ=!ƾ;Γ+=ƺ;Γ+="Wp spacing: "+Math.Round(ΙΟ)+"m\n";}else if(Ё==Φ.Δλ){if(Π(ref Γ,Κ,Β++,Λ," Stop")){І();ο(Φ.Main);}
Γ+=ƺ;Γ+="State: "+ΓЉ(Ω)+" \n";Γ+="Speed: "+Math.Round(ď,1)+"m/s\n";Γ+="Target dist: "+ȉ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+ΖΥ+"\n";}else if(Ё==Φ.ΖЁ){List<IMyTerminalBlock>š=Ζυ();if(ĥ) Ň++;if(Ň>=š.Count) Ň=0;if(Π(ref Γ,Κ,Β++,Λ," Next")) ο(Φ.ΖЂ);
Γ+=ƺ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΓΒ,1)+"% "+Math.Round(ȗ,1)+" / "+Math.Round(ȟ,1)+"\n";Γ+="Uranium: "+(Ŋ.Count==0?"No reactors":Math.Round(ż,1)+"Kg "+Ει)+"\n";Γ+="Battery: "+(ȓ==ǐ.None?"":ā+"% ")+Ζς(ȓ)+"\n";Γ+="Hydrogen: "+(Ǩ.Count==0?"No tanks":Math.Round(Ĥ,1)+"% ")+"\n";
Γ+="Damage: "+(š.Count==0?"None":""+(Ň+1)+"/"+š.Count+" "+š[Ň].CustomName)+"\n";}else if(Ё==Φ.ΖЂ){if(Π(ref Γ,Κ,Β++,Λ," Back")) ο(Φ.Main);Γ+=ƺ;Γ+="Next scan: "+Αχ+"s\n";Γ+="Drill area: "+Math.Round(ȩ,1)+"x"+Math.Round(Ȩ,1)+"m Ship: "+Math.Round(ΕЇ,1)+"x"+Math.Round(ΕІ,1)+"m ("+Math.Round(δ,1)+"m)\n"+(Ȕ.Count>0?"Cameras: "+ΔЈ+"\n":"");
Γ+="Broadcast: "+(Ɨ?"Online - "+Ƶ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ʃ*100f,1)+"% \n";}else if(Ё==Φ.ΖϏ){if(Π(ref Γ,Κ,Β++,Λ," Back")) ο(Φ.Main);Γ+=ƺ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Е==2) Γ="Fatal setup error\nNext scan: "+Αχ+"s\n";if(ΓΧ) Γ="Recompile script now";int Λη=Γ.Split('\n').Length;for(int i=Λη;i<=10;i++) Γ+="\n";Γ+=ƴ;Γ+="Last: "+Μ+"\n";
return Γ;}void Ζζ(bool Λΰ,float ǯ){І();ΑΙ(true,false);if(Λΰ) ѓ(ref ž,ǯ,Ѐ.ΔΪ,false);else ѓ(ref ť,ǯ,Ѐ.ΔΪ,false);Ζδ();Ĩ=ȩ;ħ=Ȩ;ś(true,true,0,0);}float Ζλ(float σ,float[]Γκ){float α=Γκ[0];for(int i=Γκ.Length-1;i>=0;i--)
if(σ<Γκ[i]) α=Γκ[i];return α;}String ΒΣ(ref int ņ){String α="";if(ņ>=ΖΗ.Count) ņ=-1;if(ņ>=0) α=ΖΗ[ņ].CustomName;ņ++;return α;}void Ββ(string ΰ){if(η!=Τ.Л) return;if(ΰ=="") return;IMyTerminalBlock Ϋ=χ.GetBlockWithName(ΰ);if(Ϋ==null||!(Ϋ is IMyTimerBlock)){Μ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ϋ).Trigger();}void Ɓ(ref int value,int ΑΡ,int ı,int ΕЋ){if(ΑΡ==0) return;if(value<ı&&ΕЋ>0||value<=ı&&ΕЋ<0){value+=ΕЋ;return;}int ΚЃ=Math.Abs(ΑΡ);int Δφ=0;int Ες=1;while(true){Δφ+=Ες*ΚЃ*10;if(ΑΡ<0&&value-ı<=Δφ) break;if(ΑΡ>0&&value-ı<Δφ) break;
Ες++;}value+=Ες*ΑΡ;}void Ŝ(bool Μι){њ=Math.Max(њ,1);љ=Math.Max(љ,1);ф=Math.Max(ф,0);ś(Ё==Φ.ǌ,false,0,0);}bool Ř(ref float value,bool ΛЁ,Ѐ ω,String ŧ,String Λέ){if(ŧ=="") return false;float ƭ=-1;bool ΘΝ=false;if(ŧ.ToUpper()==Λέ) ΘΝ=true;else if(!float.TryParse(ŧ,out ƭ)) return false;
else ƭ=Math.Max(0,ƭ);if(ΛЁ) ƭ=(float) Math.Round(ƭ);ѓ(ref value,ƭ,ω,ΘΝ,false);return true;}void ѓ(ref float value,float ǯ,Ѐ ω,bool ΑЁ){ѓ(ref value,value+ǯ,ω,ΑЁ,true);}void ѓ(ref float value,float ƪ,Ѐ ω,bool ΑЁ,bool Μΰ){float Ȅ=0,ȃ=0;if(ω==Ѐ.ΑΚ){Ȅ=0.5f;ȃ=10f;}if(ω==Ѐ.ΑЊ){Ȅ=0.1f;ȃ=1f;}
if(ω==Ѐ.ǈ){Ȅ=50f;ȃ=100f;}if(ω==Ѐ.ΔΜ){Ȅ=5f;ȃ=30f;}if(ω==Ѐ.ΒΟ){Ȅ=1f;ȃ=25f;}if(ω==Ѐ.ΒΞ){Ȅ=10f;ȃ=95f;}if(ω==Ѐ.ΔΝ){Ȅ=10f;ȃ=90f;}if(ω==Ѐ.Κλ){Ȅ=10f;ȃ=1800;}if(ω==Ѐ.ΔΪ){Ȅ=0.0f;ȃ=0.75f;}if(ƪ==-1&&ΑЁ){value=-1;return;}if(value==-1) ΑЁ=false;
bool Θσ=ƪ<Ȅ||ƪ>ȃ;if(Θσ&&Μΰ){if(ƪ<value) value=ȃ;else if(ƪ>value) value=Ȅ;}else value=ƪ;if(Θσ&&ΑЁ) value=-1;else value=Math.Max(Ȅ,Math.Min(value,ȃ));value=(float) Math.Round(value,2);}void Γν(bool Λό){if(Λό) и=Math.Max(0,и-1);else и++;ΓЂ();ΓЁ();for(int k=0;k<ć.Count&&k<к.Length;k++) Δγ(ć[k],k<и);
Й=-1;Δο(true);}T ĸ<T>(T σ){int Β=Array.IndexOf(Enum.GetValues(σ.GetType()),σ);Β++;if(Β>=Ιπ(σ)) Β=0;return(T) Enum.GetValues(σ.GetType()).GetValue(Β);}int Ιπ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δι(){ΑЈ.Clear();for(int i=0;i<Ψ.Count;i++) ΑЈ.Add(Ψ[i]);Ψ.Clear();
ΒЏ=true;ΕΩ=new έ(Ρ);Εέ=new έ(Ζ);Ρ.ν=false;if(Ξ==Ν.ί) Ζ.ν=false;for(int i=0;i<ǰ.Count;i++)
if(!Đ.Contains(ǰ.Keys.ElementAt(i)))
Đ.Add(ǰ.Keys.ElementAt(i));Ē=false;ƽ=false;ƾ=false;ο(Φ.ΗΖ);}void ΗΣ(){if(Ē) Ρ=ΕΩ;if(ƽ) Ζ=Εέ;if(ƾ){Ψ.Clear();for(int i=0;i<ΑЈ.Count;i++) Ψ.Add(ΑЈ[i]);}ΒЏ=false;І();ο(Φ.Main);}void Κϊ(){if(!ΒЏ) return;if(Ω!=Α.і){ΗΣ();return;}if(!ΕΩ.ν) Ē=false;if(!Εέ.ν) ƽ=false;if(ΑЈ.Count<=1) ƾ=false;
IMyShipConnector Х=о(MyShipConnectorStatus.Connectable);if(Х==null) Х=о(MyShipConnectorStatus.Connected);if(Х!=null){if(Math.Round(ď,2)<=0.20) ΕΕ++;else ΕΕ=0;if(ΕΕ>=5){if(Ξ==Ν.ί&&(Ρ.ν||Ē)&&Vector3.Distance(Ρ.Ε,Х.GetPosition())>5){Ζ.μ=Ϊ.WorldMatrix.Forward;Ζ.Є=Ϊ.WorldMatrix.Left;
Ζ.λ=Ϊ.WorldMatrix.Down;Ζ.ϋ=Ϊ.GetNaturalGravity();Ζ.Ε=Х.GetPosition();Ζ.ν=true;Ζ.х=Х.Position;}else{Ρ.μ=Ϊ.WorldMatrix.Forward;Ρ.Є=Ϊ.WorldMatrix.Left;Ρ.λ=Ϊ.WorldMatrix.Down;Ρ.ϋ=Ϊ.GetNaturalGravity();Ρ.Ε=Х.GetPosition();Ρ.ν=true;Ρ.х=Х.Position;}}}double Εΰ=-1;if(Ψ.Count>0) Εΰ=Vector3.Distance(β,Ψ.Last().Ε);
double ΛЎ=Math.Max(1.5,Math.Pow(ď/100.0,2));double Θη=Math.Max(ď*ΛЎ,2);ΙΟ=Θη;if((Εΰ==-1)||Εΰ>=Θη){έ ή=new έ(β,ΒЈ,ΑΩ,Αλ,Ϊ.GetNaturalGravity());ή.ΙΪ(đ,Đ);Ψ.Add(ή);}}έ ѕ(){if(Ξ!=Ν.ί) return ǫ!=null?ǫ:Ρ;return Ηω;}void Ǝ(έ γ,Τ Ј){Ηω=γ;if(η==Τ.Л) Ǚ=Ј;}bool Ȯ(έ γ,float υ,bool ΜΦ,out Vector3 ά)
{if(ΜΦ){Vector3I ΕΠ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);IMySlimBlock Ϋ=Me.CubeGrid.GetCubeBlock(ΕΠ);if(Ϋ==null||!(Ϋ.FatBlock is IMyShipConnector)){ά=new Vector3();return false;}Vector3 ΚБ=ъ(Ϊ,Ϋ.FatBlock.GetPosition()-β);Vector3 ΚА=ъ(Ϊ,Ϋ.FatBlock.WorldMatrix.Forward);ά=γ.Ε-Δξ(γ.μ,γ.λ*-1,ΚБ)-Δξ(γ.μ,γ.λ*-1,ΚА)*υ;
return true;}else{ά=γ.Ε;return true;}}Vector3 ĕ(int ΔЍ,int ŋ,bool ΛϏ){if(!ΛϏ&&ΘΑ) return Βν;float ΚЏ=((Б-1f)/2f)-ΔЍ;float ΛЂ=((Ю-1f)/2f)-ŋ;Βν=Ζ.Ε+Ζ.Є*ΚЏ*Ɗ()+Ƞ*-1*ΛЂ*Ɖ();ΘΑ=true;return Βν;}Vector3 ΓЈ(Vector3 ΜΑ,float ΕΑ){return ΜΑ+(Ф*ΕΑ);}ι Γύ(){float Ы=-1;
ι α=ι.ȫ;if(Ξ!=Ν.ί){ΖЌ();if(η!=Τ.ы){Vector3 Т=Ś(Ф,Ƞ*-1,β-Ζ.Ε);if(Math.Abs(Т.X)<=(float)(Б*ȩ)/2f&&Math.Abs(Т.Y)<=(float)(Ю*Ȩ)/2f){if(Т.Z<=-1&&Т.Z>=-ŷ*2) return ι.ΑЌ;if(Т.Z>-1&&Т.Z<δ*2) return ι.ȫ;}if(Ķ(Ζ.Ε,ref Ы)) α=ι.ȫ;}if(Ρ.ν){if(Ķ(Ρ.Ε,ref Ы)) α=ι.Ȫ;
for(int i=0;i<Ψ.Count;i++)
if(Ķ(Ψ[i].Ε,ref Ы)) α=ι.Ė;if(Vector3.Distance(β,Ρ.Ε)<dockDist*δ) α=ι.Ȫ;if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null)
α=ι.Ȫ;}}else{Vector3 ά=new Vector3();IMyShipConnector ă=о(MyShipConnectorStatus.Connected);if(Ρ.ν){if(Ķ(Ρ.Ε,ref Ы)) α=ι.Ȫ;if(Ȯ(Ρ,dockDist,true,out ά))
if(Ķ(ά,ref Ы)) α=ι.Ȫ;if(ă!=null&&Vector3.Distance(ă.GetPosition(),Ρ.Ε)<5)
return ι.Γρ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ρ.Ε)>dockDist*δ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*δ)
if(Ķ(Ψ[i].Ε,ref Ы)) α=ι.Ė;if(Ζ.ν){if(Ķ(Ζ.Ε,ref Ы)) α=ι.ȫ;if(Ȯ(Ζ,dockDist,true,out ά))
if(Ķ(ά,ref Ы)) α=ι.ȫ;if(ă!=null&&Vector3.Distance(ă.GetPosition(),Ζ.Ε)<5)
return ι.Γς;}}return α;}bool Ķ(Vector3 ά,ref float Ы){float υ=Vector3.Distance(ά,β);if(υ<Ы||Ы==-1){Ы=υ;return true;}return false;}void Δθ(){if(Е>0){Μ="Setup error! Can't start";return;}if(Ξ==Ν.ί){ķ();return;}Ζ.Ε=β;Ζ.ϋ=Ϊ.GetNaturalGravity();Ζ.μ=ΒЈ;Ζ.λ=ΑΩ;Ζ.Є=Αλ;
Ф=Ǭ.WorldMatrix.Forward;Ƞ=Ζ.λ;if(Ф==Ϊ.WorldMatrix.Down) Ƞ=Ϊ.WorldMatrix.Backward;Ĩ=ȩ;ħ=Ȩ;ΑΙ(true,true);Υ(Α.ǵ);Δη();}void ΑΙ(bool Ǿ,bool ΛЀ){if(η==Τ.ы&&!Ǿ) return;bool Λπ=Ǿ||η==Τ.ΓϏ||Б!=њ||Ю!=љ||ũ!=ȣ;if(Λπ){if(η!=Τ.ы){η=Τ.ΑЏ;ĕ(Ѕ,Ђ,ΛЀ);Μ="Job changed, lost progress";
}ũ=ȣ;Б=њ;Ю=љ;Ђ=0;Ѕ=0;к="";Й=-1;Ũ=(int)(DateTime.Now.Ticks/10000000%1000000);ŭ=0;ī=0;п=0;и=0;Δο(true);}}void Δά(){б(β,0);Ǥ(đ,true);}void Δη(){ȱ(ь.ΒΪ);Δά();ΑΔ(Ċ,false);ȑ(ΕЍ);η=Τ.Л;Ζγ(true);Ǚ=η;ο(Φ.ǋ);ΚΨ();Βω=true;ΒЌ=0;for(int i=ΑΛ.Count-1;i>=0;i--)
if(ǉ(ΑΛ[i],false)) ΒЌ++;if(ΒЌ>0) Μ="Started with damage";}void І(){if(η==Τ.Л){η=Τ.ƌ;Μ="Job paused";}Υ(Α.і);Ǚ=η;Βέ(false,0,0,0,0);Δμ();Ζή(new Vector3(),false);ΗΡ();Δβ(ChargeMode.Auto);Δδ(false);ΗΘ(true);ė(Α.і);ś(false,false,0,0);Ǥ(ύ,false);Ǥ(Ǔ,true);ȑ(true);ΓΨ=false;ΑБ();
ΑΨ=false;Δή();Βω=false;Αω=false;ΑЂ=false;if(Ё!=Φ.Main&&Ё!=Φ.ΑЍ&&Ё!=Φ.Γξ&&Ё!=Φ.Βή)
ο(Φ.Main);}void ķ(){ι д=Γύ();if(Ξ==Ν.ί){if(!Ζ.ν||!Ρ.ν) return;Δη();bool ΕΞ=Vector3.Distance(β,Ρ.Ε)<Vector3.Distance(β,Ζ.Ε);if(Ʋ==Τ.ſ) ΕΞ=true;if(Ʋ==Τ.ƀ) ΕΞ=false;if(ΕΞ){Ǝ(Ρ,Τ.ſ);switch(д){case ι.Γρ:Υ(Α.Ľ);break;case ι.Ė:Υ(Α.т);break;case ι.Ȫ:Υ(Α.ȍ);break;default:Υ(Α.ǹ);break;
}}else{Ǝ(Ζ,Τ.ƀ);switch(д){case ι.Γς:Υ(Α.Ľ);break;case ι.ȫ:Υ(Α.ȍ);break;case ι.Ė:Υ(Α.т);break;default:Υ(Α.ǹ);break;}}}else{if(η!=Τ.ƌ&&η!=Τ.ΑЏ) return;bool ΜΩ=η==Τ.ΑЏ;Δη();bool ΕΦ=Ǌ(false)&&Ρ.ν;switch(д){case ι.ȫ:Υ(ΕΦ?Α.ȭ:Α.ǵ);break;case ι.Ė:Υ(ΕΦ?Α.И:Α.т);break;case ι.Ȫ:Υ(ΕΦ?Α.ȍ:Α.ȳ);break;
case ι.ΑЌ:{if(ī!=и||ΜΩ) Υ(Α.ȏ);else Υ(Α.ŗ);}break;default:break;}}}void ŕ(){if(η==Τ.ы&&!Ρ.ν) return;if(Ξ==Ν.ί&&(!Ζ.ν||!Ρ.ν)) return;Μ="Move to job";ι д=Γύ();if(Ξ==Ν.ί){Ǝ(Ζ,Τ.ƀ);switch(д){case ι.ȫ:Υ(Α.ȍ);break;case ι.Ė:Υ(Α.т);break;case ι.Γς:return;default:Υ(Α.ǹ);break;
}ė(Α.Ľ);}else{switch(д){case ι.ȫ:Υ(Α.т);break;case ι.Ė:Υ(Α.т);break;case ι.Ȫ:Υ(Α.ȳ);break;case ι.ΑЌ:Υ(Α.ȏ);break;default:break;}if(η==Τ.ы) ė(Α.т);else ė(Α.ĵ);ΓΨ=true;}Δά();ο(Φ.Δλ);ΑΔ(Ċ,false);Ǚ=Τ.ΒΡ;}void ΒΖ(){if(!Ρ.ν) return;Μ="Move home";ι д=Γύ();if(Ξ==Ν.ί){Ǝ(Ρ,Τ.ſ);switch(д)
{case ι.Ė:Υ(Α.И);break;case ι.Ȫ:Υ(Α.ȍ);break;case ι.Γρ:return;default:Υ(Α.ǹ);break;}ė(Α.Ľ);}else{if(о(MyShipConnectorStatus.Connected)!=null) return;if(о(MyShipConnectorStatus.Connectable)!=null){Υ(Α.œ);ė(Α.ȳ);return;}switch(д){case ι.ȫ:Υ(Α.ȭ);break;case ι.Ė:Υ(Α.И);break;case ι.Ȫ:Υ(Α.И);break;
case ι.ΑЌ:Υ(Α.ǣ);break;default:break;}ė(Α.ȳ);}Δά();ο(Φ.Δλ);ΑΔ(Ċ,false);Ǚ=Τ.ΒΠ;}void Υ(Α Ετ){if(Ετ==Α.і) Αό=Α.і;if(Αό!=Α.і&&Ω==Αό&&Ετ!=Αό){І();return;}Ж=true;Ω=Ετ;}void ė(Α Ј){this.Αό=Ј;}Ǹ Με(int Αη,bool Ȇ){if(Ȇ){Ǯ=null;Ѕ=0;Ђ=0;}if(ȣ==ї.ΑΘ){int Ű=Αη+1;
Ђ=(int) Math.Floor(М(Αη,Б));if(Ђ%2==0) Ѕ=Αη-(Ђ*Б);else Ѕ=Б-1-(Αη-(Ђ*Б));if(Ђ>=Ю) return Ǹ.ΑЋ;else return Ǹ.ΒΘ;}else if(ȣ==ї.Ζε){if(Ǯ==null) Ǯ=new int[]{0,-1,0,0};int Αδ=(int) Math.Ceiling(Б/2f);int Αγ=(int) Math.Ceiling(Ю/2f);int ΕΤ=(int) Math.Floor(Б/2f);
int ΕΣ=(int) Math.Floor(Ю/2f);int Αθ=0;while(Ǯ[2]<Math.Pow(Math.Max(Б,Ю),2)){if(Αθ>200) return Ǹ.Ιρ;Αθ++;Ǯ[2]++;if(-Αδ<Ѕ&&Ѕ<=ΕΤ&&-Αγ<Ђ&&Ђ<=ΕΣ){if(Ǯ[3]==Αη){this.Ѕ=Ѕ-1+Αδ;this.Ђ=Ђ-1+Αγ;return Ǹ.ΒΘ;}Ǯ[3]++;}if(Ѕ==Ђ||(Ѕ<0&&Ѕ==-Ђ)||(Ѕ>0&&Ѕ==1-Ђ))
{int ΜΔ=Ǯ[0];Ǯ[0]=-Ǯ[1];Ǯ[1]=ΜΔ;}Ѕ+=Ǯ[0];Ђ+=Ǯ[1];}}return Ǹ.ΑЋ;}void Κψ(){if(Ω==Α.ǵ){if(Ж){Њ=0;if(ī!=и) ŭ=0;ī=и;}if(Њ==0){int ΑΣ=Ια(и);if(ΑΣ<0){if(!ΑА()){Υ(Α.ǣ);return;}if(!Βυ||(ΑΣ==-2)!=Βτ){Βυ=true;Βτ=ΑΣ==-2;double a=(ю%6)*Math.PI/3;Ηφ=!Βτ?β:Ζ.Ε-Ф*δ*2.5f+
(Ζ.Є*(float) Math.Cos(a)+Ƞ*(float) Math.Sin(a))*δ*1.2f;}б(Ηφ,10);Μ=Βτ?"Waiting for a free hole":"Waiting for the controller";return;}if(Βυ) Μ="";Βυ=false;if(ΑΣ!=и){и=ΑΣ;ŭ=0;ī=и;}Ǹ α=Κα(и);if(α==Ǹ.ΑЋ){η=Τ.ΓϏ;Μ="Job done";if(ō&&Ρ.ν){Υ(Α.ȭ);ė(Α.ȳ);Ǚ=Τ.ΒΠ;}
else{Υ(Α.Ƅ);ė(Α.ĵ);Ǚ=Τ.ΒΡ;}return;}if(α==Ǹ.ΒΘ){Њ=1;Ǥ(ύ,true);О=ĕ(Ѕ,Ђ,true);float h=Vector3.Dot(β-О,Ф);if((β-О-Ф*h).Length()>Math.Max(Ɗ(),Ɖ())*1.5f) h=Math.Min(h,-δ);б(h<-1?О+Ф*h:О,10);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}}else{if(Й>=0&&(ΖЊ()?Ȗ>=0&&Ȗ!=Й:ΓЏ(Й,true))){Њ=0;return;}
if(й<wpReachedDist){if(Vector3.Distance(Ϗ,О)>1){б(О,10);return;}Υ(Α.ŗ);return;}if(ď<0.3f&&Ǎ<0&&!Ų){if(++Њ<30) return;Њ=1;Vector3 ΓΟ=О-β;float Ě=Vector3.Dot(ΓΟ,Ф);if(Ě>0&&(ΓΟ-Ф*Ě).Length()<wpReachedDist){Βν=β;Υ(Α.ŗ);}else б(О+Ф*(Math.Min(0,-Ě)-δ),10);
return;}Њ=1;}}if(Ω==Α.ŗ){if(Ж){Ǥ(ύ,true);ȑ(false);О=ĕ(Ѕ,Ђ,false);б(ΓЈ(О,0),0);с(Ζ.λ,Ζ.μ,Ζ.Є,false);Њ=1;ћ=0;щ=0;Βλ=0;Εβ=-1;ŷ=ф;ΕΫ=true;}if(!ΑА()){Υ(Α.ǣ);return;}if(Ǌ(true)){Θρ=ē("","ORE",а.Ő);if((л==З.ΓЀ||л==З.ΒΑ||
л==З.ΓБ||л==З.ΒΛ)&&Ξ!=Ν.Ƕ)
Υ(Α.Ŕ);else if((л==З.Ζκ||л==З.Γω)&&Ξ!=Ν.Ƕ)
Υ(Α.Δπ);else
Υ(Α.ǣ);return;}п=Vector3.Distance(β,О);if(п>ŭ){ŭ=п;ΕΫ=false;}if(Ξ==Ν.Ƕ&&Ζφ()==MyDetectedEntityType.SmallGrid)
щ+=2;else щ-=2;щ=Math.Max(100,Math.Min(400,щ));if(Њ>0&&Њ<щ){if(п>ћ){if(щ>150) ћ=п;else ћ=(float) Math.Ceiling(п);Њ=1;}else Њ++;}else{if(Њ>0){Μ="Ship stuck! Retrying";ћ=п;Њ=0;ś(false,true,0,δ*sensorRange);}б(ΓЈ(О,Math.Max(0,ћ-δ)),ΓЋ(false));if(й<=wpReachedDist/2){Њ=1;ћ=0;}
return;}ś(false,true,δ*sensorRange,0);Vector3 ΚЅ=О+Ф*п;bool ΘΡ=false;if(Vector3.Distance(ΚЅ,β)>0.3f){Vector3 ΛΔ=О+Ф*(п+0.1f);б(ΛΔ,4);ΘΡ=true;}else{float ȇ=ΓЋ(true);Vector3 Ηύ=ΓЈ(О,Math.Max(ф+1,п+1));Ȑ(true,false,false,Ηύ,Ηύ-О,ȇ,ȇ);}bool ΕΒ=false;if(ǔ==Ǡ.Γτ||ǔ==Ǡ.Γσ)
{if(!ΘΡ){float Εφ=0;foreach(IMyTerminalBlock ȧ in ύ)
Εφ+=Ικ(ȧ,"","",ǔ==Ǡ.Γσ?new string[]{"STONE"}:null);if(Εφ>Εβ||п<ф||ΕΫ){Δό=0;Βλ=п;ŷ=(int)(Math.Max(ŷ,Βλ)+δ/2);}else{ΕΒ=п-Βλ>2&&Δό>=20;Δό++;}Εβ=Εφ;}}else ΕΒ=п>=ŷ;if(ī!=и){Υ(Α.ȏ);п=0;return;}if(ΕΒ){и++;Υ(Α.ȏ);п=0;return;}}if(Ω==Α.ȏ||Ω==Α.ǣ||Ω==Α.Ŕ)
{if(Ж){О=ĕ(Ѕ,Ђ,false);Vector3 ё=β-О;if(Ω==Α.ǣ&&Ρ.ν&&
(ё-Ф*Vector3.Dot(ё,Ф)).Length()>Math.Max(Ɗ(),Ɖ())){Υ(Α.ȭ);return;}с(Ζ.λ,Ζ.μ,Ζ.Є,false);Ǥ(ύ,Ɯ);ȑ(false);ћ=Vector3.Distance(β,О);ś(false,true,0,δ*sensorRange);}if(Ж) Αέ=false;б(Αέ?О-Ф*δ:О,ΓЋ(false));if(!Αέ&&Vector3.Distance(β,О)>=ћ+5){Ǥ(ύ,false);ȑ(true);Μ="Can´t return!";}
if(й<wpReachedDist){if(Ω==Α.ǣ&&!Αέ){Αέ=true;return;}if(Ω==Α.ȏ&&ΓΨ) Υ(Α.Ƅ);if(Ω==Α.ȏ) Υ(Α.ǵ);if(Ω==Α.Ŕ) Υ(Α.ΒΕ);if(Ω==Α.ǣ){if(Ρ.ν) Υ(Α.ȭ);else{І();ŕ();Μ="Can´t return, no dock found";}}return;}}if(Ω==Α.И||Ω==Α.т){if(Ω==Α.т&&η==Τ.Л&&Ξ!=Ν.ί){if(!ΑА()||Ǌ(true)){Υ(Α.И);return;}
}bool ΕϏ=false;bool Γθ=false;bool Ηα=false;float Ɣ=0;bool Θπ=false;έ ή=null;if(Ж){if(Ω==Α.И||Ξ==Ν.ί){if(Ξ!=Ν.ί){if(Ę(1)) Γπ();else{ǫ=null;č=0;ΑΨ=!Γψ(1);}}έ γ=ѕ();ς=new ΔΫ();ς.ζ=γ;ς.ΑΦ=followPathDock*δ;ς.ΖΡ=useDockDirectionDist*δ;ς.Ɣ=10;ς.Ƹ.Add(γ.Ε);Vector3 Ɠ=new Vector3();
if(Ȯ(γ,dockDist*δ,true,out Ɠ))
ς.Ƹ.Add(Ɠ);else ς.ΑΦ*=1.5f;if(Ξ==Ν.ί){if(γ==Ρ) ς.ΒЅ=Ζ.Ε;if(γ==Ζ) ς.ΒЅ=Ρ.Ε;ς.ΕΛ=dockDist*δ*1.1f;}}else if(Ω==Α.т){ς=new ΔΫ();ς.ζ=Ζ;ς.ΑΦ=followPathJob*δ;ς.ΖΡ=useJobDirectionDist*δ;ς.Ɣ=10;ς.ΒЅ=Ρ.Ε;ς.ΕΛ=dockDist*δ*1.1f;ς.Ƹ.Add(Ζ.Ε);if(η==Τ.ы){if(!Ρ.ν||Ψ.Count==0){І();return;}
float ΛΛ=Vector3.Distance(Ψ.First().Ε,Ρ.Ε);float ΛΜ=Vector3.Distance(Ψ.Last().Ε,Ρ.Ε);if(ΛΛ<ΛΜ) ς.ζ=Ψ.Last();else ς.ζ=Ψ.First();}}Ασ=new Vector3();Θπ=!ΙА(β);Ǥ(ύ,false);ȑ(true);р=-1;double ȕ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ς.ΒЅ)<=ς.ΕΛ) continue;
double υ=Vector3.Distance(Ψ[i].Ε,β);if(ȕ==-1||υ<ȕ){р=i;ȕ=υ;}}ȡ=Ιώ(ς.ζ.Ε,р);Š=null;}if(Ω==Α.И&&Ξ!=Ν.ί&&!Ж&&!ΑΖ()){έ ΔЁ=ѕ();ΑΨ=!Γψ(1);if(ѕ()!=ΔЁ){Υ(Α.И);return;}}if(Ω==Α.И&&Ξ!=Ν.ί&&!Ж&&Ę(1)&&Γπ()){Υ(Α.И);return;}bool ΘΟ=false;if(Ω==Α.И&&!Ж&&р>=0&&р<Ψ.Count)
{Vector3 ΑΥ=Ψ[р].Ε+Ζω(р,ȡ);int Ευ=р+ȡ;Vector3 Ű=ȡ!=0&&Ευ>=0&&Ευ<Ψ.Count?Ψ[Ευ].Ε:ς.ζ.Ε;if(ΙЉ(ΑΥ,Ű)){if(!Βώ){Βώ=true;ΗЂ=ƈ(ΑΥ)?β:ΑΥ;}б(ΗЂ,0);с(ς.ζ.λ,ς.ζ.μ,ς.ζ.Є,false);Μ="Waiting for dock clearance";return;}Βώ=false;ΘΟ=Ę(1)&&(ƈ(β)||ƈ(Ű));}ΙΫ(Ψ,ȡ,ς.Ƹ,ς.ΑΦ,Ж,ref Њ);
for(int i=0;i<ς.Ƹ.Count;i++){float υ=Vector3.Distance(β,ς.Ƹ[i]);if(υ<=ς.ΑΦ) ΕϏ=true;if(υ<=ς.ΖΡ) Γθ=true;}if(ΘΟ) Γθ=true;if(Γθ) Ɣ=ς.Ɣ;float Μκ=Š!=null?Š.У:ď;float Μί=(float) Math.Max(ď*0.1f*δ,wpReachedDist);if((й<Μί)||Ж){if(!Ж) р+=ȡ;if(ȡ==0||р>Ψ.Count-1||р<0)
ΕϏ=true;else{ΖΥ=ȡ>0?Ψ.Count-1-р:р;ή=Ψ[р];Š=ή;if(р>=1&&р<Ψ.Count-1)
Ασ=ή.Ε-Ψ[р-ȡ].Ε;else Š=null;Ϗ=ή.Ε+Ζω(р,ȡ);Ηα=true;}}if(Γθ)
с(ς.ζ.λ,ς.ζ.μ,ς.ζ.Є,false);else if(Θπ)
Γο(ς.ζ.λ,10,true);else if(Ηα&&ή!=null){if(ȡ>0) с(ή.λ,ή.μ,ή.Є,90,false);else с(ή.λ,-ή.μ,-ή.Є,90,false);}Ȑ(true,false,true,Ϗ,Ασ,Š==null?0:Š.У,Ɣ);if(ΕϏ){ΖΥ=0;if(Ω==Α.И&&!Ιί()){б(Ϗ,0);return;}if(Ω==Α.И&&ΑΨ){б(Ϗ,0);Μ="Waiting for a free dock";return;}
if(Ω==Α.И||Ξ==Ν.ί){Υ(Α.ȍ);return;}if(Ω==Α.т&&ΓΨ){Υ(Α.Ƅ);return;}if(Ω==Α.т){Υ(Α.ǵ);return;}}}if(Ω==Α.ȍ||Ω==Α.ΑΓ){if(Ξ!=Ν.ί&&ΑΖ()){if(!Ę(1)){Υ(Α.И);return;}}else if(Ξ!=Ν.ί){έ ΔЁ=ѕ();if(!Γψ(2)||ѕ()!=ΔЁ){Υ(Α.И);return;}}έ γ=ѕ();if(Ж){if(!Ȯ(γ,dockDist*δ,true,out Ϗ))
{ȱ(ь.Ǆ);І();return;}б(Ϗ,0);Γο(γ.λ,90,true);}if(й<followPathDock*δ&&й!=-1){б(Ϗ,10);с(γ.λ,γ.μ,γ.Є,false);}if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null){Υ(Α.œ);return;}if(й<wpReachedDist/2&&й!=-1){Υ(Α.ŏ);return;}}if(Ω==Α.ŏ||Ω==Α.ĵ)
{if(Ж){if(Ω==Α.ŏ){έ γ=ѕ();if(!Ȯ(γ,dockDist*δ,true,out Ϗ)){ȱ(ь.Ǆ);І();return;}Ȑ(true,true,false,Ϗ,0);с(γ.λ,γ.μ,γ.Є,10,false);}if(Ω==Α.ĵ){с(Ζ.λ,Ζ.μ,Ζ.Є,0.5f,false);Ϗ=Ζ.Ε;Ȑ(true,true,false,Ϗ,0);}}if(ΒЎ){Βέ(false,0,0,0,0);if(Ω==Α.ŏ) Υ(Α.œ);if(Ω==Α.ĵ) І();return;
}}if(Ω==Α.œ){if(о(MyShipConnectorStatus.Connected)!=null){if(Ξ==Ν.ί) Υ(Α.Ľ);else Υ(Α.ȳ);return;}έ γ=ѕ();if(Ж){щ=0;ΛΝ=DateTime.Now;Њ=0;с(γ.λ,γ.μ,γ.Є,false);}Vector3I ΕΠ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);IMySlimBlock Ησ=Me.CubeGrid.GetCubeBlock(ΕΠ);float ΗЅ=dockingSpeed;
float Λκ=dockingSpeed*5;float Ηί=Math.Max(1.5f,Math.Min(5f,δ*0.15f));if(!Ȯ(γ,0,true,out Ϗ)||!Ȯ(γ,Ηί,true,out Ασ)||
Ησ==null||!Ησ.FatBlock.IsFunctional){ȱ(ь.Ǆ);І();return;}if(щ==1||(Vector3.Distance(β,Ϗ)<=Ηί*1.1f&&!Ж))
щ=1;else{Vector3 ΜΝ=ъ(Ϊ,Ασ-β);Vector3 ΕΟ=ъ(Ϊ,Ϊ.GetNaturalGravity());float ΛЃ=ΑЎ(ΜΝ,ΕΟ,null);ΗЅ=Math.Min(Λκ,ΛЃ);}Ȑ(true,false,false,Ϗ,Ϗ-β,dockingSpeed,ΗЅ);if(Ж) ћ=(float) й;IMyShipConnector ΔЎ=о(MyShipConnectorStatus.Connectable);if(ΔЎ!=null){Ȑ(false,false,false,Ϗ,0);
if(Њ>0) Њ=0;Њ--;if(Њ<-5){ΔЎ.Connect();if(ΔЎ.Status==MyShipConnectorStatus.Connected){if(Ξ==Ν.ί) Υ(Α.Ľ);else Υ(Α.ȳ);Δμ();ΑΔ(Ċ,true);return;}}}else{float ΗЁ=(float) Math.Round(й,1);if(ΗЁ<ћ){Њ=-1;ћ=ΗЁ;}else Њ++;if(Њ>20){Υ(Α.ΑΓ);return;}}}if(Ω==Α.ȳ||Ω==Α.Ľ||
Ω==Α.Βε||Ω==Α.ΒΓ||Ω==Α.Œ){bool Δϊ=false,Δϋ=false;if(Ξ==Ν.ί){if(ѕ()==Ρ) Δϊ=true;else if(ѕ()==Ζ) Δϋ=true;}if(Ж){Αω=false;if(о(MyShipConnectorStatus.Connected)==null){Υ(Α.ǹ);return;}Δμ();if(Δϊ) Ββ(Ќ.ǽ);if(Δϋ) Ββ(Ѝ.ǽ);Ż=false;ě=false;ģ=false;ΑЄ=false;}if(о(MyShipConnectorStatus.Connected)==null){І();ȱ(ь.ΖЅ);return;}
if(η!=Τ.Л||ǚ==-1||ȓ==ǐ.None) ě=true;else if(ā>=100f) ě=true;else if(ā<=99f) ě=false;if(η!=Τ.Л||Ǜ==-1||Ǩ.Count==0) ģ=true;else if(Ĥ>=100f) ģ=true;else if(Ĥ<=99) ģ=false;if(η!=Τ.Л||ц==-1||Ŋ.Count==0) ΑЄ=true;else ΑЄ=ż>=ц;Ɛ Ǐ=null;if(Δϊ) Ǐ=Ќ;
if(Δϋ) Ǐ=Ѝ;if(Ǐ!=null&&(Ǐ.Ч==τ.ΒΦ||Ǐ.Ч==τ.ΒΥ)) ě=true;if(Ǐ!=null&&(Ǐ.Ч==τ.ΔΣ)) if(!Ż) ě=false;if(Ǐ!=null&&(Ǐ.Ч==τ.ΔΧ||Ǐ.Ч==τ.ΔΥ)) ģ=true;if(Ǐ!=null&&(Ǐ.Ч==τ.ΔΦ)) if(!Ż) ģ=false;if(ĥ){ChargeMode Ημ=ě?ChargeMode.Auto:ChargeMode.Recharge;if(Ǐ!=null&&(Ǐ.Ч==τ.ΒΥ||Ǐ.Ч==τ.ΒΦ))
Ημ=ChargeMode.Discharge;Δβ(Ημ);Δδ(!ģ);}if(!Ż){if(Ξ==Ν.ί)
Ż=η!=Τ.Л||Ιΰ(Ж,true)||ΑЂ;else
Ż=η!=Τ.Л||ΖІ();}else{if(!ě) Υ(Α.Œ);if(!ģ) Υ(Α.ΒΓ);if(!ΑЄ) Υ(Α.Βε);Ж=false;}if(Ż&&ě&&ģ&&ΑЄ){Δβ(ChargeMode.Auto);Δδ(false);if(η==Τ.Л){if(Ξ==Ν.ί){if(ѕ()==Ρ) Ββ(Ќ.Ȃ);else if(ѕ()==Ζ) Ββ(Ѝ.Ȃ);if(ѕ()==Ρ) Ǝ(Ζ,Τ.ƀ);else Ǝ(Ρ,Τ.ſ);}}Υ(Α.ǹ);return;}}if(Ω==Α.ǹ){if(ΑЇ){if(!Ζη()) return;
ΑЇ=false;Υ(Α.ǹ);return;}if(Ж){if(!Ζη()){ΑЇ=true;return;}IMyShipConnector ă=о(MyShipConnectorStatus.Connected);if(ă==null){Υ(Α.т);return;}IMyShipConnector θ=ă.OtherConnector;ΗΗ(ă,false);ΑΔ(Ċ,false);έ γ=null;έ ǖ=Ξ==Ν.ί?Ρ:ѕ();if(Vector3.Distance(ă.GetPosition(),ǖ.Ε)<5f&&Ρ.ν) γ=ǖ;
if(Vector3.Distance(ă.GetPosition(),Ζ.Ε)<5f&&Ζ.ν) γ=Ζ;if(γ!=null){if(!Ȯ(γ,dockDist*δ,true,out Ϗ)){ȱ(ь.Ǆ);І();return;}б(Ϗ,5);с(γ.λ,γ.μ,γ.Є,false);}else б(β+θ.WorldMatrix.Forward*dockDist*δ,5);if(η==Τ.Л) ȱ(ь.ΒΪ);}if(й<wpReachedDist){Ǥ(Ǔ,true);Υ(Α.т);return;}}if(Ω==Α.Ƅ)
{if(Ж){ȑ(true);Ǥ(ύ,false);Ϗ=Ζ.Ε;б(Ϗ,20);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}if(й<wpReachedDist/2){Υ(Α.ĵ);return;}}if(Ω==Α.ȭ){if(Ж){ȑ(true);Ǥ(ύ,false);int Ы=-1;double ȕ=-1;bool ΘϏ=ΔΟ();for(int Č=0;Č<2&&Ы==-1;Č++,ΘϏ=false)
for(int i=Ψ.Count-1;i>=0;i--){if(ΘϏ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)<ΖЋ()+δ) continue;double υ=Vector3.Distance(Ψ[i].Ε,β);if(ȕ==-1||υ<ȕ){Ы=i;ȕ=υ;}}if(Ы==-1){Υ(Α.И);return;}Ϗ=Ψ[Ы].Ε;б(Ϗ,10);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}if(й<wpReachedDist){Υ(Α.И);return;}}if(Ω==Α.ΒΕ)
{bool φ=false;if(Ж){ȑ(true);if((л==З.ΓЀ||л==З.ΒΑ)&&ΒΝ()&&
Ƃ(Ф,Ϊ.GetNaturalGravity())<25&&Б>=2&&Ю>=2){Vector3 ΑΪ=β;if(Ѕ>0&&Ђ<Ю-1) ΑΪ=ĕ(Ѕ-1,Ђ+1,true);else if(Ѕ<Б-1&&Ђ<Ю-1) ΑΪ=ĕ(Ѕ+1,Ђ+1,true);else if(Ѕ<Б-1&&Ђ>0) ΑΪ=ĕ(Ѕ+1,Ђ-1,true);else if(Ѕ>0&&Ђ>0) ΑΪ=ĕ(Ѕ-1,Ђ-1,true);else φ=true;
if(!φ) б(ΑΪ,10);}else φ=true;}if(й<wpReachedDist/2) φ=true;if(φ){Υ(Α.Βδ);return;}}if(Ω==Α.Δπ||Ω==Α.Βδ){if(Ж){Ȑ(true,true,false,β,0);Ǥ(ύ,false);ȑ(true);Њ=-1;щ=л==З.ΓБ||л==З.ΒΛ?0:-1;}bool ţ=!ΑА();int Γα=ē("STONE","ORE",а.Ő);if(л==З.Γω||л==З.ΒΛ||л==З.ΒΑ)
Γα+=ē("ICE","ORE",а.Ő);bool ΛΨ=Γα>0;bool ΕΚ=false;if(щ>=0){float Ƒ=(float) Math.Sin(Ζμ(щ))*ȩ/3f;float ΚЇ=(float) Math.Cos(Ζμ(щ))*Ȩ/3f;Vector3 ΚЎ=ĕ(Ѕ,Ђ,true)+Δξ(Ф,Ƞ*-1,new Vector3(Ƒ,ΚЇ,0));б(ΚЎ,0.3f);if(й<Math.Min(ȩ,Ȩ)/10f) щ+=5f;if(щ>=360) щ=0;}if(Њ==-1||Γα<Њ){Њ=Γα;ћ=0;}
else{ћ++;if(ћ>50) ΕΚ=true;}if(!ΛΨ||ţ||ΕΚ){if(!ţ){int ΜΣ=ē("","ORE",а.Ő);if(Ǌ(true)) ţ=true;else if(100-(М(ΜΣ,Θρ)*100)<minEjection) ţ=true;else ȱ(ь.ΒΪ);}if(ΕΚ&&ţ) Μ="Ejection failed";if(Ω==Α.Βδ){if(ţ){if(Ρ.ν) Υ(Α.ȭ);else{І();ŕ();Μ="Can´t return, no dock found";}}
else Υ(Α.ǵ);}else if(ţ) Υ(Α.ǣ);else Υ(Α.ŗ);return;}}Ж=false;}void ΙΫ(List<έ>Ŏ,int ρ,List<Vector3>ΑϏ,float ȉ,bool Ȇ,ref int ǻ){if(Ȇ){for(int i=0;i<Ŏ.Count;i++) Ŏ[i].У=0;ǻ=-1;return;}if(ρ==0) return;int ǯ=ρ*-1;if(ǻ==-1) ǻ=ǯ>0?1:Ŏ.Count-2;int Αθ=0;while(ǻ>=1&&ǻ<Ŏ.Count-1)
{if(Αθ>50) return;Αθ++;try{if((ǯ<0&&ǻ>=1)||(ǯ>0&&ǻ<=Ŏ.Count-2)){έ σ=Ŏ[ǻ];bool Θμ=false;for(int j=0;j<ΑϏ.Count;j++)
if(Vector3.Distance(σ.Ε,ΑϏ[j])<=ȉ){Θμ=true;break;}if(!Θμ){έ ΓΣ=Ŏ[ǻ-ǯ];έ Ű=Ŏ[ǻ+ǯ];Vector3 ΜΞ=σ.Ε-Ű.Ε;Vector3 ΖΙ=ΓΣ.Ε-σ.Ε;Vector3 Λώ=σ.Ε+Vector3.Normalize(ΜΞ)*ΖΙ.Length();Vector3 ΛΙ=ΓΣ.Ε-Λώ;Vector3 ΓΓ=Ś(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΛΙ);Vector3 Λι=Ś(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΖΙ);
Vector3 ΓΕ=Ś(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,σ.ϋ);σ.У=(float) Math.Sqrt(Math.Pow(ΓΣ.У,2)+Math.Pow(ΑЎ(-Λι,ΓΕ,σ),2));for(int j=0;j<ΑϏ.Count;j++)
if(Vector3.Distance(ΓΣ.Ε,ΑϏ[j])<=ȉ){Vector3 ΓΖ=Ś(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΑϏ[j]-σ.Ε);float ΜΓ=ΑЎ(-ΓΖ,ΓΕ,σ);σ.У=Math.Min(σ.У,ΜΓ)/2f;}if(ΓΓ.Length()==0) ΓΓ=new Vector3(0,0,1);Vector3 Λθ=Ś(σ.μ,σ.λ*-1,σ.ϋ);float ΜΘ=Ǉ(-ΓΓ,Λθ,σ);float Βζ=М(ΜΘ,Ź);float ΖΖ=(float) Math.Sqrt(М(ΓΓ.Length()*1.0f,0.5f*Βζ));
σ.У=Math.Min(σ.У,(ΖΙ.Length()/ΖΖ)*Ȓ);}}}catch{return;}ǻ+=ǯ;}ǻ=-1;}void Δο(bool Ȇ){if(Ȇ){Ακ=0;return;}if(Ξ==Ν.ί) return;float φ=и*Math.Max(1,ф);if(ī==и) φ+=Math.Min(ф,п);float ΜΡ=Б*Ю*Math.Max(1,ф);Ακ=Math.Max(Ακ,(float) Math.Min(φ/ΜΡ*100.0,100));}MyDetectedEntityType Ζφ()
{try{if(ȯ(ό,true)&&!ό.LastDetectedEntity.IsEmpty()) return ό.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓЋ(bool Ȝ){if(Ξ==Ν.Ƕ&&Ζφ()==MyDetectedEntityType.None&&!ǉ(ό,true))
return fastSpeed;else return Ȝ?ǂ:ǁ;}void Κξ(){float Θδ=0,Ηϋ=0,ŧ=0,Εχ=0;for(int i=0;i<ľ.Count;i++){IMyBatteryBlock ΑΟ=ľ[i];if(!ȯ(ΑΟ,true)) continue;Θδ+=ΑΟ.MaxStoredPower;Ηϋ+=ΑΟ.CurrentStoredPower;ŧ+=ΑΟ.CurrentInput;Εχ+=ΑΟ.CurrentOutput;}ā=(float) Math.Round(М(Ηϋ,Θδ)*100,1);
if(ŧ>=Εχ) ȓ=ǐ.Œ;else ȓ=ǐ.Ζν;if(ŧ==0&&Εχ==0||ā==100.0) ȓ=ǐ.і;if(ľ.Count==0) ȓ=ǐ.None;}void Κυ(){float ΗЋ=0;for(int i=0;i<Ǩ.Count;i++){IMyGasTank ΘЏ=Ǩ[i];if(!ȯ(ΘЏ,true)) continue;ΗЋ+=(float) ΘЏ.FilledRatio;}Ĥ=М(ΗЋ,Ǩ.Count)*100f;}void ΚЁ(){ż=0;try{for(int i=0;i<Ŋ.Count;i++)
{IMyReactor ΕЀ=Ŋ[i];if(!ȯ(ΕЀ,true)) continue;List<MyInventoryItem>А=new List<MyInventoryItem>();ΕЀ.GetInventory(0).GetItems(А);float Џ=0;for(int j=0;j<А.Count;j++){MyInventoryItem Ѓ=А[j];if(Ѓ.Type.SubtypeId.ToUpper()=="URANIUM"&&Ѓ.Type.TypeId.ToUpper().Contains("_INGOT"))
Џ+=(float) Ѓ.Amount;}if(Џ<ż||i==0){ż=Џ;Ει=ΕЀ.CustomName;}}}catch(Exception e){Ī=e;}}void Κο(){ȟ=0;ȗ=0;try{for(int i=0;i<Ƙ.Count;i++){IMyTerminalBlock Ϋ=Ƙ[i];if(!ȯ(Ϋ,true)) continue;ȗ+=(float) Ϋ.GetInventory(0).CurrentVolume;ȟ+=(float) Ϋ.GetInventory(0).MaxVolume;
}ΓΒ=(float) Math.Min(Math.Round(М(ȗ,ȟ)*100,1),100.0);}catch(Exception e){Ī=e;}}void Κφ(){try{Ȁ.Clear();for(int i=0;i<Ƙ.Count;i++){IMyTerminalBlock Ϋ=Ƙ[i];if(!ȯ(Ϋ,true)) continue;Ζέ(Ϋ,а.Ƈ);}if(л!=З.ΔΡ){for(int i=0;i<Ş.Count;i++){IMyTerminalBlock Ϋ=Ş[i];if(!ȯ(Ϋ,true)) continue;
Ζέ(Ϋ,а.ΙЎ);}}Κδ(Ȁ);}catch(Exception e){Ī=e;}}void Ιή(){if(Βω){if(Ζυ().Count>ΒЌ){Βω=false;if(ѝ!=є.ǈ){І();if(ѝ==є.ΓІ) ŕ();if(ѝ==є.ΒΩ)
if(Ρ.ν) ΒΖ();else ŕ();}Μ="Damage detected";}}}bool ΑА(){if(!ΒЍ) return true;if(η==Τ.Л){if(ǚ>0&&ȓ!=ǐ.None)
if(ā<=ǚ){Μ="Low energy! Move home";return false;}if(ц>0&&Ŋ.Count>0)
if(ż<=ц){Μ="Low fuel: "+Ει;return false;}if(Ǜ>0&&Ǩ.Count>0)
if(Ĥ<=Ǜ){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Ζυ(){List<IMyTerminalBlock>š=new List<IMyTerminalBlock>();for(int i=0;i<ΑΛ.Count;i++){IMyTerminalBlock Ϋ=ΑΛ[i];if(ǉ(Ϋ,false)) š.Add(Ϋ);}return š;}bool ǉ(IMyTerminalBlock Ϋ,bool ΕЏ){return(!ȯ(Ϋ,ΕЏ)||!Ϋ.IsFunctional);
}bool ȯ(IMyTerminalBlock Ϋ,bool ΕЏ){if(Ϋ==null) return false;try{IMyCubeBlock ΗБ=Me.CubeGrid.GetCubeBlock(Ϋ.Position).FatBlock;if(ΕЏ) return ΗБ==Ϋ;else return ΗБ.GetType()==Ϋ.GetType();}catch{return false;}}float Ικ(IMyTerminalBlock Ϋ,String ǟ,String ΘІ,String[]Ț){float Ą=0;for(int i=0;i<Ϋ.InventoryCount;i++)
{IMyInventory ơ=Ϋ.GetInventory(i);List<MyInventoryItem>А=new List<MyInventoryItem>();ơ.GetItems(А);foreach(MyInventoryItem Ѓ in А){if(Ț!=null&&(Ț.Contains(Ѓ.Type.TypeId.ToUpper())||Ț.Contains(Ѓ.Type.SubtypeId.ToUpper())))
continue;if((ǟ==""||Ѓ.Type.TypeId.ToUpper()==ǟ)&&(ΘІ==""||Ѓ.Type.SubtypeId.ToUpper()==ΘІ))
Ą+=(float) Ѓ.Amount;}}return Ą;}у Ισ(String ΰ,String ω,а д,bool ΛΕ){ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<Ȁ.Count;i++){у Ѓ=Ȁ[i];if(Ѓ.ΰ.ToUpper()==ΰ&&Ѓ.ω.ToUpper()==ω&&(Ѓ.Ʀ==д||д==а.Ő))
return Ѓ;}у α=null;if(ΛΕ){α=new у(ΰ,ω,0,д);Ȁ.Add(α);}return α;}int ē(String ΰ,String ω,а д){return ē(ΰ,ω,д,null);}int ē(String ΰ,String ω,а д,String[]Ț){int Ą=0;ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<Ȁ.Count;i++){у Ѓ=Ȁ[i];if(Ț!=null&&Ț.Contains(Ѓ.ΰ.ToUpper())) continue;
if((ΰ==""||Ѓ.ΰ.ToUpper()==ΰ)&&(ω==""||Ѓ.ω.ToUpper()==ω)&&(Ѓ.Ʀ==д||д==а.Ő))
Ą+=Ѓ.Џ;}return Ą;}void Ζέ(IMyTerminalBlock Ϋ,а д){for(int i=0;i<Ϋ.InventoryCount;i++){List<MyInventoryItem>А=new List<MyInventoryItem>();Ϋ.GetInventory(i).GetItems(А);for(int j=0;j<А.Count;j++)
Ισ(А[j].Type.SubtypeId,А[j].Type.TypeId.Replace("MyObjectBuilder_",""),д,true).Џ+=(int) А[j].Amount;}}void Κδ(List<у>В){for(int i=В.Count-1;i>0;i--)
for(int j=0;j<i;j++){у a=В[j];у b=В[j+1];if(a.Џ<b.Џ) В.Move(j,j+1);}}bool Ǌ(bool Μη){if(Ĵ&&Ξ!=Ν.ί)
if(м!=-1&&Ź>=м){Μ="Ship too heavy";return true;}if(ΓΒ>=Ȟ||Αω){Αω=false;Μ="Ship is full";return true;}return false;}bool ΖІ(){String[]Ț=null;if(!ĳ) Ț=new string[]{"ICE"};if(Ξ==Ν.ΑΑ) return ē("","ORE",а.Ƈ,Ț)==0;if(Ξ==Ν.Ƕ) return ē("","COMPONENT",а.Ƈ,Ț)==0;
else return ē("","",а.Ƈ,Ț)==0;}bool ΙБ(){List<IMyCockpit>ΔЌ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔЌ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔЌ.Count;i++)
if(ΔЌ[i].IsUnderControl) return true;return false;}bool Ιΰ(bool Ȇ,bool ΓΪ){IMyShipConnector Х=о(MyShipConnectorStatus.Connected);if(Х==null) return false;if(Vector3.Distance(Ρ.Ε,Х.GetPosition())<5)
return Ζθ(Ќ,Ȇ,ΓΪ);if(Vector3.Distance(Ζ.Ε,Х.GetPosition())<5)
return Ζθ(Ѝ,Ȇ,ΓΪ);return false;}bool Ζθ(Ɛ Ă,bool Ȇ,bool ΓΪ){if(Ȇ) Ă.ΚΣ();Ă.Κύ();bool ǎ=false;String Ǟ="";switch(Ă.Ч){case τ.ΔΤ:{Ǟ="Waiting for command";ǎ=false;break;}case τ.ΗΑ:{Ǟ="Waiting for cargo";ǎ=Ǌ(true);break;}case τ.ΖБ:{Ǟ="Unloading";ǎ=ΖІ();break;}case τ.ΔΩ:{ǎ=true;break;}
case τ.ΔΣ:{Ǟ="Charging batteries";ǎ=ā>=100f;break;}case τ.ΒΦ:{Ǟ="Discharging batteries";ǎ=ā<=25f;break;}case τ.ΒΥ:{Ǟ="Discharging batteries";ǎ=ā<=0f;break;}case τ.ΔΦ:{Ǟ="Filling up hydrogen";ǎ=Ĥ>=100f;break;}case τ.ΔΧ:{Ǟ="Unloading hydrogen";ǎ=Ĥ<=25f;break;}
case τ.ΔΥ:{Ǟ="Unloading hydrogen";ǎ=Ĥ<=0f;break;}case τ.ΔΨ:{bool ΘΞ=ΙБ();if(!ΘΞ) Ă.Εϋ=true;ǎ=Ă.Εϋ&&ΘΞ;Ǟ="Waiting for passengers";break;}}if(!ǎ) Ă.Γχ(true);if(ǎ&&Ă.ΓЍ()){ǎ=Ă.Γχ(false);Ǟ="Undocking in: "+ΒΗ((int) Math.Max(0,Ă.Ǽ-Ă.Ń));}if(ΓΪ) ΙΝ=Ǟ;return ǎ;}void ΙΣ()
{if(!Ƣ) return;if(ύ.Count<=1) return;float ΙΕ=0,ΙΔ=0;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock ȧ=ύ[i];if(ǉ(ȧ,true)) continue;ΙΕ+=(float) ȧ.GetInventory(0).MaxVolume;ΙΔ+=(float) ȧ.GetInventory(0).CurrentVolume;}float Ηβ=(float) Math.Round(М(ΙΔ,ΙΕ),5);for(int Č=0;Č<Math.Max(1,Math.Floor(ύ.Count/10f));Č++)
{float ΕΨ=0,Εθ=0;float ΘΗ=0,Θα=0;IMyTerminalBlock ġ=null,ń=null;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock ȧ=ύ[i];if(ǉ(ȧ,true)) continue;float Εο=(float) ȧ.GetInventory(0).MaxVolume;float ΒІ=М((float) ȧ.GetInventory(0).CurrentVolume,Εο);if(ġ==null||ΒІ>ΕΨ){ġ=ȧ;ΕΨ=ΒІ;ΘΗ=Εο;}
if(ń==null||ΒІ<Εθ){ń=ȧ;Εθ=ΒІ;Θα=Εο;}}if(ġ==null||ń==null||ġ==ń) return;if(checkConveyorSystem&&!ġ.GetInventory(0).IsConnectedTo(ń.GetInventory(0))){if(ΔЀ>20) Μ="Inventory balancing failed";else ΔЀ++;return;}ΔЀ=0;List<MyInventoryItem>А=new List<MyInventoryItem>();
ġ.GetInventory(0).GetItems(А);float ΑІ=0;if(А.Count==0) continue;MyInventoryItem ΒЇ=А[0];String ΘΣ=ΒЇ.Type.TypeId+ΒЇ.Type.SubtypeId;if(!ΙΜ.TryGetValue(ΘΣ,out ΑІ)){if(ΚΖ(ġ.GetInventory(0),0,ń.GetInventory(0),out ΑІ))
ΙΜ.Add(ΘΣ,ΑІ);else return;}float ΜΟ=((ΕΨ-Ηβ)*ΘΗ/ΑІ);float ΜΠ=((Ηβ-Εθ)*Θα/ΑІ);int ΖΞ=(int) Math.Min(ΜΠ,ΜΟ);if(ΖΞ<=0) return;if((float) ΒЇ.Amount<ΖΞ)
ġ.GetInventory(0).TransferItemTo(ń.GetInventory(0),0,null,null,ΒЇ.Amount);else
ġ.GetInventory(0).TransferItemTo(ń.GetInventory(0),0,null,null,ΖΞ);}}bool ΚΖ(IMyInventory Αϊ,int Β,IMyInventory ΛΘ,out float ΙΛ){ΙΛ=0;float ΚЋ=(float) Αϊ.CurrentVolume;List<MyInventoryItem>ΔЂ=new List<MyInventoryItem>();Αϊ.GetItems(ΔЂ);float Ηζ=0;for(int i=0;i<ΔЂ.Count;i++) Ηζ+=(float) ΔЂ[i].Amount;
Αϊ.TransferItemTo(ΛΘ,Β,null,null,1);float ΙΙ=ΚЋ-(float) Αϊ.CurrentVolume;List<MyInventoryItem>Δχ=new List<MyInventoryItem>();Αϊ.GetItems(Δχ);float ΗΫ=0;for(int i=0;i<Δχ.Count;i++) ΗΫ+=(float) Δχ[i].Amount;if(ΙΙ==0f||!ΔΑ(0.9999,Ηζ-ΗΫ,1.0001))
return false;ΙΛ=ΙΙ;return true;}void Κτ(){Vector3 Αΰ=Ϗ+Ĳ;Vector3 Ȧ=Αΰ-β;if(Ȧ.Length()==0) Ȧ=new Vector3(0,0,-1);Vector3 ΓΖ=ъ(Ϊ,Ȧ);Vector3 ΘА=Vector3.Normalize(ΓΖ);Vector3 ƥ=ъ(Ϊ,Ϊ.GetNaturalGravity());float ΛЏ=Εω>0?Math.Max(0,1-Ƃ(Ȧ,ΗЍ)/5):0;float У=(float) Math.Min((Εμ>0?Εμ:1000f),Math.Max(ΑЎ(-ΓΖ,ƥ,null),Εω*ΛЏ));
if(!ΙΓ) У=0;if(ΘЀ) У=Math.Max(0,1-Βχ/Βη)*У;if(generalSpeedLimit>0) У=Math.Min(generalSpeedLimit,У);if(Ų) У=0;if(Ǎ>=0) У=Math.Min(У,Ǎ);if(ΔΟ()) У=Math.Min(У,jobAreaSpeed);if(ΘЁ) У*=(float) Math.Min(1,М(Ȧ.Length(),wpReachedDist)/2);Vector3 Θί=ъ(Ϊ,Ϊ.GetShipVelocities().LinearVelocity);
float ΚІ=(float)(Math.Max(0,15-Ƃ(-ΘА,-Θί))/15)*0.85f+0.15f;ΖΔ+=Math.Sign(ΚІ-ΖΔ)/10f;Vector3 ŀ=ΘА*У*ΖΔ-(Θί);Vector3 Δώ=ΓЊ(ŀ,null);if(Γγ&&й>0.1f){ŀ.X*=Δζ(ŀ.X,ref Γδ.X,1f,Δώ.X,20);ŀ.Y*=Δζ(ŀ.Y,ref Γδ.Y,1f,Δώ.Y,20);ŀ.Z*=Δζ(ŀ.Z,ref Γδ.Z,1f,Δώ.Z,20);}else Γδ=new Vector3(1,1,1);
ΑЀ=Ź*ŀ-ƥ*Ź;Ζή(ΑЀ,Γγ);й=Vector3.Distance(β,Αΰ);}float Δζ(float value,ref float Ј,float ǯ,float Ĭ,float Εν){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ј)) Ј+=Math.Sign(Ј)*ǯ;else Ј=value;if(value==0) Ј=1;float α=Math.Abs(Ј);if(α<Εν||Ĭ==0) return α;
Ј=Math.Min(Εν,Math.Max(-Εν,Ј));α=Math.Abs(Ĭ);return α;}void ΗΩ(){float ǜ=90,ǳ=90,ǭ=90;float ȇ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȝ,Ǫ,Αν,Εε;if(Εζ){Ȝ=Vector3.Normalize(Ϗ-β);Αν=ъ(Ϊ,Ȝ);Ǫ=ъ(Ϊ,ΕБ);ǜ=Ƃ(Αν,new Vector3(0,-1,0))-90;
ǳ=ő(Ǫ,new Vector3(-1,0,0),Ǫ.Y);ǭ=ő(Αν,new Vector3(-1,0,0),Αν.Z);}else{Ȝ=ΖΑ;Ǫ=ъ(Ϊ,ΕБ);Αν=ъ(Ϊ,ΖΑ);Εε=ъ(Ϊ,ΘБ);ǜ=ő(Ǫ,new Vector3(0,0,1),Ǫ.Y);ǳ=ő(Ǫ,new Vector3(-1,0,0),Ǫ.Y);ǭ=ő(Εε,new Vector3(0,0,1),Εε.X);}if(Ηέ&&ΒΝ()){Vector3 ΕΟ=Ϊ.GetNaturalGravity();Ǫ=ъ(Ϊ,ΕΟ);
ǜ=ő(Ǫ,new Vector3(0,0,1),Ǫ.Y);ǳ=ő(Ǫ,new Vector3(-1,0,0),Ǫ.Y);}if(!ΔΑ(-45,ǳ,45)){ǜ=0;ǭ=0;}if(!ΔΑ(-45,ǭ,45)) ǜ=0;Βέ(ΕΡ,1,(-ǜ)*ȇ,(-ǭ)*ȇ,(-ǳ)*ȇ);Βχ=Math.Max(Math.Abs(ǜ),Math.Max(Math.Abs(ǳ),Math.Abs(ǭ)));ΒЎ=Βχ<=Βη;}void Ιά(IMyTerminalBlock ΓΥ){if(ΓΥ==null) return;
ȋ=new float[3,2];ǰ=new Dictionary<string,float[,]>();for(int i=0;i<đ.Count;i++){IMyThrust ђ=đ[i];if(!ђ.IsFunctional) continue;Vector3 ρ=ъ(ΓΥ,ђ.WorldMatrix.Backward);float ł=ђ.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ȋ[0,0]+=ł;else ȋ[0,1]-=ł;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ȋ[1,0]+=ł;else ȋ[1,1]-=ł;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ȋ[2,0]+=ł;else ȋ[2,1]-=ł;String ǟ=Ζύ(ђ);float[,]Ǳ=null;if(ǰ.ContainsKey(ǟ)) Ǳ=ǰ[ǟ];else{Ǳ=new float[3,2];ǰ.Add(ǟ,Ǳ);}float Ĭ=ђ.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ǳ[0,0]+=Ĭ;else Ǳ[0,1]-=Ĭ;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ǳ[1,0]+=Ĭ;else Ǳ[1,1]-=Ĭ;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ǳ[2,0]+=Ĭ;else Ǳ[2,1]-=Ĭ;}}static String Ζύ(IMyThrust ђ){return ђ.BlockDefinition.SubtypeId;}Vector3 ΔΘ(Vector3 Ǧ,float[,]Ƨ){return new Vector3(
Ǧ.X>=0?Ƨ[0,0]:Ƨ[0,1],
Ǧ.Y>=0?Ƨ[1,0]:Ƨ[1,1],
Ǧ.Z>=0?Ƨ[2,0]:Ƨ[2,1]);}bool Ζό(έ ή,String ǟ,out float ΕΙ){ΕΙ=0;int Β=Đ.IndexOf(ǟ);if(Β==-1||ή.Ȋ==null||Β>=ή.Ȋ.Length)
return false;ΕΙ=ή.Ȋ[Β];if(ΕΙ==-1) return false;return true;}Vector3 ΓЊ(Vector3 Ǧ,έ ή){if(ή!=null){Vector3 α=new Vector3();for(int i=0;i<ǰ.Keys.Count;i++){String ǟ=ǰ.Keys.ElementAt(i);float Αά=0;if(!Ζό(ή,ǟ,out Αά))
return ΔΘ(Ǧ,ȋ);α+=ΔΘ(Ǧ,ǰ.Values.ElementAt(i))*Αά;}return α;}return ΔΘ(Ǧ,ȋ);}float Ǉ(Vector3 Ǧ,έ ή){return Ǉ(Ǧ,new Vector3(),ή);}float Ǉ(Vector3 Ǧ,Vector3 ϋ,έ ή){Vector3 ΜΗ=ΓЊ(Ǧ,ή);Vector3 ł=ΜΗ+ϋ*Ź;float Ŷ=(ł/Ǧ).AbsMin();return(float)(Ǧ*Ŷ).Length();}float ΑЎ(Vector3 ΓΔ,Vector3 ƥ,έ ή)
{if(ΓΔ.Length()==0) return 0;float Ηά=1;if(ƥ.Length()>0) Ηά=Math.Min(1,Ƃ(-ƥ,ΓΔ)/90)*0.4f+0.6f;float ΑЀ=Ǉ(ΓΔ,ƥ,ή);if(ΑЀ==0) return 0.1f;float Βζ=М(ΑЀ,Ź);float ΖΖ=(float) Math.Sqrt(М(ΓΔ.Length(),Βζ*0.5f));return Βζ*ΖΖ*Ηά*Ȓ;}float ΒΚ(Vector3 Ȝ,Vector3 Љ,Vector3 ϋ,έ ή)
{if(ϋ.Length()==0f) return 0;Vector3 ΓΕ=Ś(Ȝ,Љ,Vector3.Normalize(ϋ));float Ǿ=Ǉ(-ΓΕ,ή);return Ǿ/ϋ.Length();}void Ζγ(bool Λή){float ǲ=0;float ΕΜ=0.9f;if(Λή){м=-1;Ž=0;Γή=null;if(η!=Τ.ы&&Ζ.ϋ.Length()!=0){ǲ=ΕΜ*ΒΚ(Ζ.μ,Ζ.λ*-1,Ζ.ϋ,null);if(ǲ<м||м==-1) м=ǲ;}if(Ρ.ν&&Ρ.ϋ.Length()!=0)
{ǲ=ΕΜ*ΒΚ(Ρ.μ,Ρ.λ*-1,Ρ.ϋ,null);if(ǲ<м||м==-1) м=ǲ;}return;}if(Ž==-1) return;if(Ž>=0){int Θφ=0;while(Ž<Ψ.Count){if(Θφ>100) return;Θφ++;έ ή=Ψ[Ž];if(ή.ϋ.Length()!=0f){ǲ=ΕΜ*Math.Min(
ΒΚ(ή.μ,ή.λ*-1,ή.ϋ,ή),
ΒΚ(ή.μ*-1,ή.λ*-1,ή.ϋ,ή));if(ǲ<м||м==-1) м=ǲ;}else Γή=ή;Ž++;}Ž=-1;}bool ΕΥ=true;float ŉ=0;if(Γή!=null){for(int i=0;i<ǰ.Count;i++){String ǟ=ǰ.Keys.ElementAt(i);float[,]Ǳ=ǰ.Values.ElementAt(i);float Αά=0;if(!Ζό(Γή,ǟ,out Αά)){ΕΥ=false;break;}for(int a=0;a<Ǳ.GetLength(0);a++)
for(int b=0;b<Ǳ.GetLength(1);b++){float ŝ=Math.Abs(Ǳ[a,b]*Αά);if(ŝ==0) continue;ΕΥ=true;if(ŉ==0||ŝ<ŉ) ŉ=ŝ;}}}if(!ΕΥ){for(int a=0;a<ȋ.GetLength(0);a++)
for(int b=0;b<ȋ.GetLength(1);b++){float ŝ=Math.Abs(ȋ[a,b]);if(ŝ==0) continue;if(ŉ==0||ŝ<ŉ) ŉ=ŝ;}}if(ŉ>0){float Λμ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǲ=М(ŉ,Λμ);if(ǲ>0)
if(ǲ<м||м==-1) м=ǲ;}}void Ζή(Vector3 Ǿ,bool С){if(!С){for(int i=0;i<đ.Count;i++)
đ[i].SetValueFloat("Override",0.0f);return;}Vector3 Δύ=ΓЊ(Ǿ,null);float Μα=Math.Min(1,Math.Abs(М(Ǿ.X,Δύ.X)));float Μβ=Math.Min(1,Math.Abs(М(Ǿ.Y,Δύ.Y)));float Μγ=Math.Min(1,Math.Abs(М(Ǿ.Z,Δύ.Z)));for(int i=0;i<đ.Count;i++){IMyThrust ђ=đ[i];Vector3 ρ=ΚΥ(ъ(Ϊ,ђ.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǿ.X))
ђ.SetValueFloat("Override",ђ.MaxThrust*Μα);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǿ.Y))
ђ.SetValueFloat("Override",ђ.MaxThrust*Μβ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǿ.Z))
ђ.SetValueFloat("Override",ђ.MaxThrust*Μγ);else
ђ.SetValueFloat("Override",0.0f);}}void Βέ(bool С,float Λϊ,float ǜ,float ǭ,float ǳ){for(int i=0;i<ΒЉ.Count;i++){IMyGyro Н=ΒЉ[i];Н.GyroOverride=С;if(!С) Н.GyroPower=100;else Н.GyroPower=Λϊ;if(!С) continue;Vector3 г=Ϊ.WorldMatrix.Forward;Vector3 ч=Ϊ.WorldMatrix.Right;Vector3 Љ=Ϊ.WorldMatrix.Up;
Vector3 Ɲ=Н.WorldMatrix.Forward;Vector3 Ɵ=Н.WorldMatrix.Up;Vector3 ƞ=Н.WorldMatrix.Left*-1;if(Ɲ==г) Н.SetValueFloat("Roll",ǳ);else if(Ɲ==(г*-1)) Н.SetValueFloat("Roll",ǳ*-1);else if(Ɵ==(г*-1)) Н.SetValueFloat("Yaw",ǳ);else if(Ɵ==г) Н.SetValueFloat("Yaw",ǳ*-1);else if(ƞ==г) Н.SetValueFloat("Pitch",ǳ);
else if(ƞ==(г*-1)) Н.SetValueFloat("Pitch",ǳ*-1);if(ƞ==(ч*-1)) Н.SetValueFloat("Pitch",ǜ);else if(ƞ==ч) Н.SetValueFloat("Pitch",ǜ*-1);else if(Ɵ==ч) Н.SetValueFloat("Yaw",ǜ);else if(Ɵ==(ч*-1)) Н.SetValueFloat("Yaw",ǜ*-1);else if(Ɲ==(ч*-1)) Н.SetValueFloat("Roll",ǜ);
else if(Ɲ==ч) Н.SetValueFloat("Roll",ǜ*-1);if(Ɵ==(Љ*-1)) Н.SetValueFloat("Yaw",ǭ);else if(Ɵ==Љ) Н.SetValueFloat("Yaw",ǭ*-1);else if(ƞ==Љ) Н.SetValueFloat("Pitch",ǭ);else if(ƞ==(Љ*-1)) Н.SetValueFloat("Pitch",ǭ*-1);else if(Ɲ==Љ) Н.SetValueFloat("Roll",ǭ);else if(Ɲ==(Љ*-1)) Н.SetValueFloat("Roll",ǭ*-1);
}}void ΗΡ(){this.ΕΡ=false;}void с(Vector3 Ș,Vector3 г,Vector3 ū,float ı,bool ΑЅ){Γο(Ș,ı,ΑЅ);Βη=ı;Εζ=false;this.ΖΑ=г;this.ΘБ=ū;}void с(Vector3 Ș,Vector3 г,Vector3 ū,bool ΑЅ){с(Ș,г,ū,2f,ΑЅ);}void Γο(Vector3 Ș,float ı,bool ΑЅ){Βη=ı;this.ΕΡ=true;this.Ηέ=ΑЅ;Εζ=true;ΒЎ=false;
this.ΕБ=Ș;}void Δμ(){Ȑ(false,false,false,Ϗ,0);Γγ=false;}void б(Vector3 ζ,float ȇ){Ȑ(true,false,false,ζ,ȇ);}void Ȑ(bool С,bool ΕЌ,bool ΖΣ,Vector3 ζ,float ȇ){Ȑ(С,ΕЌ,ΖΣ,ζ,ζ-β,0.0f,ȇ);}void Ȑ(bool С,bool ΕЌ,bool ΖΣ,Vector3 ζ,Vector3 ΛЈ,float ΛЉ,float ȇ){Γγ=true;this.ΙΓ=С;
Ϗ=ζ;this.Εμ=ȇ;this.Εω=ΛЉ;this.ΘЁ=ΕЌ;this.ΘЀ=ΖΣ;this.ΗЍ=ΛЈ;й=Vector3.Distance(ζ,β);}void ΚΨ(){χ.GetBlocksOfType(ΑΛ,ǡ);}void ΚΩ(){if(ΓΧ){Е=2;return;}List<IMyRemoteControl>Αυ=new List<IMyRemoteControl>();List<IMySensorBlock>Θύ=new List<IMySensorBlock>();List<IMyTerminalBlock>Αώ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αυ,ǡ);χ.GetBlocksOfType(Я,ǡ);χ.GetBlocksOfType(Θύ,ǡ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αώ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Я=Ǵ(Я,pamTag,true);ΗΙ();ΗΚ(Αώ);ř(Я,setLCDFontAndSize,1.4f,false);ř(Ȥ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘЌ=Ǵ(Θύ,pamTag,true);if(ΘЌ.Count>0) ό=ΘЌ[0];else ό=null;if(Ξ==Ν.ΑΑ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ύ.Count==0){Е=1;Μ="Drills are missing";}}else if(Ξ==Ν.Ƕ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ύ.Count==0){Е=1;Μ="Grinders are missing";}if(ό==null){Е=1;Μ="Sensor is missing";}}else if(Ξ==Ν.ί){χ.GetBlocksOfType(ΖΗ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘЋ=Ǵ(Αυ,pamTag,true);if(ΘЋ.Count>0) Αυ=ΘЋ;if(Αυ.Count>0) Ϊ=Αυ[0];else{Ϊ=null;Е=2;Μ="Remote is missing";return;}δ=(float) Ϊ.CubeGrid.WorldVolume.Radius*2;Ǭ=null;if(Ξ!=Ν.ί){Ζδ();if(ύ.Count>0&&Ǭ!=null){if(ό!=null&&(Ǭ.WorldMatrix.Forward!=ό.WorldMatrix.Forward||
!(Ϊ.WorldMatrix.Forward==ό.WorldMatrix.Up||Ϊ.WorldMatrix.Down==ό.WorldMatrix.Down))){Е=1;Μ="Wrong sensor direction";}if(Ǭ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Forward&&
Ǭ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Down){Е=2;Μ="Wrong remote direction";}}}}void ΚΪ(){χ.GetBlocksOfType(Ċ,ǡ);χ.GetBlocksOfType(Ǔ,ǡ);χ.GetBlocksOfType(đ,ǡ);χ.GetBlocksOfType(ΒЉ,ǡ);χ.GetBlocksOfType(ľ,ǡ);χ.GetBlocksOfType(Ŋ,ǡ);χ.GetBlocksOfType(Ǩ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Γέ,ǡ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
Ǔ=Ǵ(Ǔ,"ConnectorMedium",false);else
Ǔ=Ǵ(Ǔ,"Connector",false);List<IMyShipConnector>ΘЉ=Ǵ(Ǔ,pamTag,true);if(ΘЉ.Count>0) Ǔ=ΘЉ;if(Е<=1){if(Ǔ.Count==0){Е=1;Μ="Connector is missing";}if(ΒЉ.Count==0){Е=1;Μ="Gyros are missing";}if(đ.Count==0){Е=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>ΘЍ=Ǵ(Γέ,pamTag,true);
if(ΘЍ.Count>0) Γέ=ΘЍ;List<IMyLandingGear>ΘЊ=Ǵ(Ċ,pamTag,true);if(ΘЊ.Count>0) Ċ=ΘЊ;for(int i=0;i<Ċ.Count;i++) Ċ[i].AutoLock=false;List<IMyBatteryBlock>ΘЈ=Ǵ(ľ,pamTag,true);if(ΘЈ.Count>0) ľ=ΘЈ;List<IMyGasTank>ΘЎ=Ǵ(Ǩ,pamTag,true);if(ΘЎ.Count>0) Ǩ=ΘЎ;}void ΚΫ(){χ.GetBlocksOfType(Ş,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ƙ.Clear();for(int i=Ş.Count-1;i>=0;i--){if(ΙЏ(Ş[i])){Ƙ.Add(Ş[i]);Ş.RemoveAt(i);}}}bool ΙЏ(IMyTerminalBlock Ϋ){if(Ϋ is IMyCargoContainer) return true;if(Ϋ is IMyShipDrill) return true;if(Ϋ is IMyShipGrinder) return true;if(Ϋ is IMyShipConnector){if(((IMyShipConnector) Ϋ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓЎ(Ϋ,"ConnectorSmall",false))
return false;else return true;}return false;}void Ζδ(){if(Ϊ==null) return;Ǭ=null;float ΓΜ=0,ΓΗ=0,ΓΝ=0,ΓΘ=0,ΓΞ=0,ΓΙ=0;List<IMyTerminalBlock>ΕА=Ǵ(ύ,pamTag,true);bool Λσ=ΕА.Count==0;if(ΕА.Count>0) Ǭ=ΕА[0];else if(ύ.Count>0) Ǭ=ύ[0];int Ą=0;float Ď=0;for(int i=0;i<ύ.Count;i++)
{if(ύ[i].WorldMatrix.Forward!=Ǭ.WorldMatrix.Forward){if(Λσ){Е=2;Μ="Mining direction is unclear!";return;}continue;}Ą++;Ď=Math.Max(Ď,Κμ(ύ[i]));Vector3 Т=ΒΙ(Ϊ,ύ[i].GetPosition());if(i==0){ΓΜ=Т.X;ΓΗ=Т.X;ΓΝ=Т.Y;ΓΘ=Т.Y;ΓΞ=Т.Z;ΓΙ=Т.Z;}ΓΗ=Math.Max(Т.X,ΓΗ);ΓΜ=Math.Min(Т.X,ΓΜ);
ΓΘ=Math.Max(Т.Y,ΓΘ);ΓΝ=Math.Min(Т.Y,ΓΝ);ΓΙ=Math.Max(Т.Z,ΓΙ);ΓΞ=Math.Min(Т.Z,ΓΞ);}if(Ď==0) Ď=drillRadius;bool Ș=Ǭ!=null&&Ǭ.WorldMatrix.Forward==Ϊ.WorldMatrix.Down;ȩ=(ΓΗ-ΓΜ)*(1-ž)+Ď*2;Ȩ=(Ș?ΓΙ-ΓΞ:ΓΘ-ΓΝ)*(1-ť)+Ď*2;IMyCubeGrid g=Ϊ.CubeGrid;float ΖΧ=0,ΖΨ=0,ΖΩ=0,ΖΪ=0;
for(int c=0;c<8;c++){Vector3 p=ΒΙ(Ϊ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ș?p.Z:p.Y;if(c==0||p.X<ΖΧ) ΖΧ=p.X;if(c==0||p.X>ΖΨ) ΖΨ=p.X;if(c==0||y<ΖΩ) ΖΩ=y;if(c==0||y>ΖΪ) ΖΪ=y;}ΕЇ=ΖΨ-ΖΧ+g.GridSize;ΕІ=ΖΪ-ΖΩ+g.GridSize;}float Κμ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǵ<T>(List<T>В,String Ȉ,bool ΒЊ){List<T>α=new List<T>();for(int i=0;i<В.Count;i++)
if(ΓЎ(В[i],Ȉ,ΒЊ)) α.Add(В[i]);return α;}bool ΓЎ<T>(T Ϋ,String Ȉ,bool ΒЊ){IMyTerminalBlock b=(IMyTerminalBlock) Ϋ;if(ΒЊ&&b.CustomName.ToUpper().Contains(Ȉ.ToUpper())) return true;if(!ΒЊ&&b.BlockDefinition.ToString().ToUpper().Contains(Ȉ.ToUpper())) return true;return false;}void Ǥ<T>(List<T>В,bool С)
{for(int i=0;i<В.Count;i++)
ΗΗ((IMyTerminalBlock) В[i],С);}void ΗΗ(IMyTerminalBlock Ϋ,bool С){if(Ϋ==null) return;String ΚЄ=С?"OnOff_On":"OnOff_Off";var a=Ϋ.GetActionWithName(ΚЄ);a.Apply(Ϋ);}void ȑ(bool С){ΕЍ=С;if(!ź) return;Ǥ(Γέ,С);}void Δβ(ChargeMode Д){for(int i=0;i<ľ.Count;i++) ľ[i].ChargeMode=Д;}void Δδ(bool ΛБ)
{for(int i=0;i<Ǩ.Count;i++) Ǩ[i].Stockpile=ΛБ;}void ΑΔ(List<IMyLandingGear>ΕΝ,bool Θΰ){for(int i=0;i<ΕΝ.Count;i++){if(Θΰ) ΕΝ[i].Lock();if(!Θΰ) ΕΝ[i].Unlock();}}void ΗΘ(bool С){if(Ηό==С) return;List<IMyShipController>Βσ=new List<IMyShipController>();χ.GetBlocksOfType(Βσ,ǡ);if(Βσ.Count==0) return;
for(int i=0;i<Βσ.Count;i++)
Βσ[i].DampenersOverride=С;Ηό=С;}IMyShipConnector о(MyShipConnectorStatus Ǟ){for(int i=0;i<Ǔ.Count;i++){if(!ȯ(Ǔ[i],true)) continue;if(Ǔ[i].Status==Ǟ) return Ǔ[i];}return null;}void ś(bool ΕЈ,bool Βύ,float ΛΥ,float ΚЊ){if(ό==null||ύ.Count==0) return;Vector3 ƙ=new Vector3();int Ą=0;
for(int i=0;i<ύ.Count;i++){if(ύ[i].WorldMatrix.Forward!=Ǭ.WorldMatrix.Forward) continue;Ą++;ƙ+=ύ[i].GetPosition();}ƙ=ƙ/Ą;Vector3 ƫ=ΒΙ(ό,ƙ);ό.Enabled=true;ό.ShowOnHUD=ΕЈ;ό.LeftExtend=(Βύ?1:њ)/2f*ȩ-ƫ.X;ό.RightExtend=(Βύ?1:њ)/2f*ȩ+ƫ.X;ό.TopExtend=(Βύ?1:љ)/2f*Ȩ+ƫ.Y;
ό.BottomExtend=(Βύ?1:љ)/2f*Ȩ-ƫ.Y;ό.FrontExtend=(ΕЈ?ф:ΛΥ)-ƫ.Z;ό.BackExtend=ΕЈ?0:ΚЊ+δ*0.75f+ƫ.Z;ό.DetectFloatingObjects=true;ό.DetectAsteroids=false;ό.DetectLargeShips=true;ό.DetectSmallShips=true;ό.DetectStations=true;ό.DetectOwner=true;ό.DetectSubgrids=false;
ό.DetectPlayers=false;ό.DetectEnemy=true;ό.DetectFriendly=true;ό.DetectNeutral=true;}void ř<T>(List<T>В,bool ΛЌ,float ΛΣ,bool Λί){for(int i=0;i<В.Count;i++){IMyTextSurface Αύ=null;if(В[i]is IMyTextSurface) Αύ=(IMyTextSurface) В[i];if(Αύ!=null){Αύ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΛЌ) continue;Αύ.Font="Debug";if(Λί) continue;Αύ.FontSize=ΛΣ;}}}bool ΒΝ(){Vector3D ά;return this.Ϊ.TryGetPlanetPosition(out ά);}int Ιώ(Vector3 ΜΒ,int ΘЂ){if(ΘЂ==-1) return 0;double Ы=-1;int ĝ=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,ΜΒ);if(Ы==-1||υ<Ы){ĝ=i;Ы=υ;}
}return Math.Sign(ĝ-ΘЂ);}bool ΙА(Vector3 ά){List<Vector3>е=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) е.Add(Ψ[i].Ε);if(Ρ.ν&&Ψ.Count>=1){Vector3 Ɠ=new Vector3();Ȯ(Ρ,dockDist*δ,false,out Ɠ);if(Vector3.Distance(Ρ.Ε,Ψ.First().Ε)<Vector3.Distance(Ρ.Ε,Ψ.Last().Ε)){е.Insert(0,Ɠ);е.Insert(0,Ρ.Ε);}
else{е.Add(Ɠ);е.Add(Ρ.Ε);}}if(Ξ==Ν.ί){if(Ζ.ν&&Ψ.Count>=1){Vector3 Δω=new Vector3();Ȯ(Ζ,dockDist*δ,false,out Δω);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){е.Insert(0,Δω);е.Insert(0,Ζ.Ε);}else{е.Add(Δω);е.Add(Ζ.Ε);}}}else{if(η!=Τ.ы)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
е.Insert(0,Ζ.Ε);else е.Add(Ζ.Ε);}int ĝ=-1;double ȕ=-1;for(int i=0;i<е.Count;i++){double υ=Vector3.Distance(е[i],ά);if(υ<ȕ||ȕ==-1){ȕ=υ;ĝ=i;}}if(е.Count==0) return false;double ΕΓ=Vector3.Distance(е[ĝ],ά);double Λϋ=Vector3.Distance(е[Math.Max(0,ĝ-1)],е[ĝ])*1.5f;
double Λς=Vector3.Distance(е[Math.Min(е.Count-1,ĝ+1)],е[ĝ])*1.5f;return ΕΓ<Λϋ||ΕΓ<Λς||ΕΓ<laneOffset*δ*1.5f;}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΚΥ(Vector3 v,int ΔБ){return new Vector3(Math.Round(v.X,ΔБ),Math.Round(v.Y,ΔБ),Math.Round(v.Z,ΔБ));
}float Ƃ(Vector3 a,Vector3 b){if(a==b) return 0;float ΛΞ=(a*b).Sum;float ΘΪ=a.Length();float ΘΫ=b.Length();if(ΘΪ==0||ΘΫ==0) return 0;float α=(float)((180.0f/Math.PI)*Math.Acos(ΛΞ/(ΘΪ*ΘΫ)));if(float.IsNaN(α)) return 0;return α;}float ő(Vector3 v,Vector3 ΓΥ,float ΛЍ){float Ƒ=Ƃ(v,ΓΥ);
if(ΛЍ>0f) Ƒ*=-1;if(Ƒ>-90f) return Ƒ-90f;else return 180f-(-Ƒ-90f);}double Ζμ(float ΛΖ){return(Math.PI/180)*ΛΖ;}bool ΔΑ(double Λλ,double value,double ƨ){return(value>=Λλ&&value<=ƨ);}Vector3 Κν(IMyTerminalBlock Ϋ,Vector3 Т){return Vector3D.Transform(Т,Ϋ.WorldMatrix);
}Vector3 ΒΙ(IMyTerminalBlock Ϋ,Vector3 Μή){return ъ(Ϋ,Μή-Ϋ.GetPosition());}Vector3 ъ(IMyTerminalBlock Ϋ,Vector3 ΖΦ){return Vector3D.TransformNormal(ΖΦ,MatrixD.Transpose(Ϋ.WorldMatrix));}Vector3 Ś(Vector3 Ȝ,Vector3 Љ,Vector3 ΖΦ){MatrixD Ελ=MatrixD.CreateFromDir(Ȝ,Љ);return Vector3D.TransformNormal(ΖΦ,MatrixD.Transpose(Ελ));
}Vector3 Δξ(Vector3 Ȝ,Vector3 Љ,Vector3 Т){MatrixD Ελ=MatrixD.CreateFromDir(Ȝ,Љ);return Vector3D.Transform(Т,Ελ);}String π(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 κ(String s){String[]ψ=s.Split('|');return new Vector3(float.Parse(Σ(ψ,0)),float.Parse(Σ(ψ,1)),float.Parse(Σ(ψ,2)));
}String ΚЂ(έ ή){String Ц=":";String α=π(ή.Ε)+Ц+π(ή.μ)+Ц+
π(ή.λ)+Ц+π(ή.Є)+Ц+π(ή.ϋ);for(int i=0;i<ή.Ȋ.Length;i++)
α+=Ц+Math.Round(ή.Ȋ[i],3);return α;}έ Κζ(String s){String[]ψ=s.Split(':');έ ή=new έ(
κ(Σ(ψ,0)),κ(Σ(ψ,1)),
κ(Σ(ψ,2)),κ(Σ(ψ,3)),
κ(Σ(ψ,4)));int Β=5;List<float>ΗЊ=new List<float>();while(Β<ψ.Length){String Γι=Σ(ψ,Β);float f=0;if(!float.TryParse(Γι,out f)) break;ΗЊ.Add(f);Β++;}ή.Ȋ=ΗЊ.ToArray();return ή;}System.Text.StringBuilder Ʊ=new System.Text.StringBuilder();void Χ<T>(T value,bool Λρ){if(Λρ) Ʊ.Append('\n');Ʊ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΒΒ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Βγ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Σ(String[]ΚЈ,int Β){String Γι=ΚЈ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Γι)) return "";
return Γι;}void Save(){if(ΓΧ||Ξ==Ν.Ǒ){Storage="";return;}Storage=Γυ();}String Γυ(){Ʊ.Clear();Ʊ.Append(DATAREV+";");Χ(π(ΓΦ),false);Χ(π(Ρ.μ));Χ(π(Ρ.Є));Χ(π(Ρ.λ));Χ(π(Ρ.ϋ));Χ(π(Ρ.Ε));Χ(π(Ρ.х));Χ(Ρ.ν);Χ(π(Ζ.Ε));Χ(π(Ζ.ϋ));Χ(π(Ζ.μ));Χ(π(Ζ.λ));Χ(π(Ζ.Є));Χ(π(Ζ.х));Χ(Ζ.ν);Χ(π(Ф));Χ(π(Ƞ));Χ(";");
Χ((int) Ξ,false);Χ((int) η);Χ((int) Ǚ);Χ(Ȟ);Χ(ǚ);Χ(ц);Χ(Ǜ);Χ(Ȓ);Χ(Ĵ);Χ(ĳ);if(Ξ==Ν.ί){Χ((int) Ќ.Ч);Χ(Ќ.Ǽ);Χ(Ќ.Ń);Χ(ΒΒ(Ќ.ǽ));Χ(ΒΒ(Ќ.Ȃ));Χ((int) Ѝ.Ч);Χ(Ѝ.Ǽ);Χ(Ѝ.Ń);Χ(ΒΒ(Ѝ.ǽ));Χ(ΒΒ(Ѝ.Ȃ));}else{Χ((int) ȣ);Χ((int) ѝ);Χ((int) л);Χ((int) ǔ);Χ((int) ũ);Χ(ō);Χ(ź);Χ(Ƣ);Χ(Ɯ);Χ(њ);Χ(љ);Χ(ф);Χ(ǂ);Χ(ǁ);
Χ(ž);Χ(ť);Χ(Б);Χ(Ю);Χ(Ѕ);Χ(Ђ);Χ(ī);Χ(и);Χ(ŷ);Χ(ŭ);}Χ(";");for(int i=0;i<Đ.Count;i++)
Χ((i>0?"|":"")+Đ[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΚЂ(Ψ[i]),i>0);Χ(";",false);Χ(к+"|"+Й+"|"+Ĩ+"|"+ħ+"|"+Ũ,false);Χ(";",false);Χ(Ιξ(),false);String Η=Ʊ.ToString();Ʊ.Clear();return Η;}ǒ ΚΤ(){return ΒΨ(Storage);}ǒ ΒΨ(String Η){if(Η=="") return ǒ.ΚΘ;String[]Ÿ=Η.Split(';');if(Σ(Ÿ,0)!=DATAREV) return ǒ.ΔΠ;int Β=0;try
{String[]Ο=Σ(Ÿ,1).Split('\n');ΓΦ=κ(Σ(Ο,Β++));Ρ.μ=κ(Σ(Ο,Β++));Ρ.Є=κ(Σ(Ο,Β++));Ρ.λ=κ(Σ(Ο,Β++));Ρ.ϋ=κ(Σ(Ο,Β++));Ρ.Ε=κ(Σ(Ο,Β++));Ρ.х=κ(Σ(Ο,Β++));Ρ.ν=bool.Parse(Σ(Ο,Β++));Ζ.Ε=κ(Σ(Ο,Β++));Ζ.ϋ=κ(Σ(Ο,Β++));Ζ.μ=κ(Σ(Ο,Β++));Ζ.λ=κ(Σ(Ο,Β++));Ζ.Є=κ(Σ(Ο,Β++));Ζ.х=κ(Σ(Ο,Β++));
Ζ.ν=bool.Parse(Σ(Ο,Β++));Ф=κ(Σ(Ο,Β++));Ƞ=κ(Σ(Ο,Β++));Ο=Σ(Ÿ,2).Split('\n');Β=0;Ξ=(Ν) int.Parse(Σ(Ο,Β++));η=(Τ) int.Parse(Σ(Ο,Β++));Ǚ=(Τ) int.Parse(Σ(Ο,Β++));Ȟ=int.Parse(Σ(Ο,Β++));ǚ=int.Parse(Σ(Ο,Β++));ц=int.Parse(Σ(Ο,Β++));Ǜ=int.Parse(Σ(Ο,Β++));Ȓ=float.Parse(Σ(Ο,Β++));
Ĵ=bool.Parse(Σ(Ο,Β++));ĳ=bool.Parse(Σ(Ο,Β++));if(Ξ==Ν.ί){Ќ.Ч=(τ) int.Parse(Σ(Ο,Β++));Ќ.Ǽ=float.Parse(Σ(Ο,Β++));Ќ.Ń=float.Parse(Σ(Ο,Β++));Ќ.ǽ=Βγ(Σ(Ο,Β++));Ќ.Ȃ=Βγ(Σ(Ο,Β++));Ѝ.Ч=(τ) int.Parse(Σ(Ο,Β++));Ѝ.Ǽ=float.Parse(Σ(Ο,Β++));Ѝ.Ń=float.Parse(Σ(Ο,Β++));Ѝ.ǽ=Βγ(Σ(Ο,Β++));
Ѝ.Ȃ=Βγ(Σ(Ο,Β++));}else{ȣ=(ї) int.Parse(Σ(Ο,Β++));ѝ=(є) int.Parse(Σ(Ο,Β++));л=(З) int.Parse(Σ(Ο,Β++));ǔ=(Ǡ) int.Parse(Σ(Ο,Β++));ũ=(ї) int.Parse(Σ(Ο,Β++));ō=bool.Parse(Σ(Ο,Β++));ź=bool.Parse(Σ(Ο,Β++));Ƣ=bool.Parse(Σ(Ο,Β++));Ɯ=bool.Parse(Σ(Ο,Β++));њ=int.Parse(Σ(Ο,Β++));љ=int.Parse(Σ(Ο,Β++));
ф=int.Parse(Σ(Ο,Β++));ǂ=float.Parse(Σ(Ο,Β++));ǁ=float.Parse(Σ(Ο,Β++));ž=float.Parse(Σ(Ο,Β++));ť=float.Parse(Σ(Ο,Β++));Б=int.Parse(Σ(Ο,Β++));Ю=int.Parse(Σ(Ο,Β++));Ѕ=int.Parse(Σ(Ο,Β++));Ђ=int.Parse(Σ(Ο,Β++));ī=int.Parse(Σ(Ο,Β++));и=int.Parse(Σ(Ο,Β++));ŷ=int.Parse(Σ(Ο,Β++));
ŭ=float.Parse(Σ(Ο,Β++));}Ο=Σ(Ÿ,3).Replace("\n","").Split('|');Đ=Ο.ToList();Ο=Σ(Ÿ,4).Split('\n');Ψ.Clear();if(Ο.Count()>=1&&Ο[0]!="")
for(int i=0;i<Ο.Length;i++)
Ψ.Add(Κζ(Σ(Ο,i)));Ο=Σ(Ÿ,5).Split('|');к=Σ(Ο,0);if(!int.TryParse(Σ(Ο,1),out Й)) Й=-1;if(!float.TryParse(Σ(Ο,2),out Ĩ)) Ĩ=0;if(!float.TryParse(Σ(Ο,3),out ħ)) ħ=0;if(!int.TryParse(Σ(Ο,4),out Ũ)) Ũ=0;if(к.Length!=Б*Ю){к="";Й=-1;}Ιν(Σ(Ÿ,6));}catch{return ǒ.Ζο;}Ʋ=Ǚ;
І();return ǒ.Βί;}const int Ζβ=64000;IMyTerminalBlock Ζπ(){List<IMyTerminalBlock>Ɩ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ɩ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));return Ɩ.Count>0?Ɩ[0]:null;}void Ις(){IMyTerminalBlock Ϋ=Ζπ();if(Ϋ==null){Μ="Export: name a block "+dataTag;return;}
String Η="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Γυ();if(Η.Length>Ζβ){Μ="Export too large: "+Η.Length+" chars, max "+Ζβ;return;}Ϋ.CustomData=Η;Μ="Exported "+Ψ.Count+" waypoints";}void ΙЌ(bool Εή){IMyTerminalBlock Ϋ=Ζπ();if(Ϋ==null){Μ="Import: name a block "+dataTag;return;}
ΙЍ(Ϋ.CustomData,Εή);}void ΙЍ(String Η,bool Εή){Η=Η.Replace("\r","");if(Η.StartsWith("//")){int Θν=Η.IndexOf('\n');Η=Θν<0?"":Η.Substring(Θν+1);}Η=Η.Trim();if(Η==""){Μ="Import: no PAM data in "+dataTag;return;}І();String Ηδ=Γυ();ǒ α=ΒΨ(Η);if(α==ǒ.Βί&&Εή){έ Ι=new έ(Ζ);
Vector3 Λξ=Ф,Λν=Ƞ;int w=њ,h=љ,d=ф;ї Αϋ=ȣ;Ǡ ΕΑ=ǔ;float Λυ=ž,Λτ=ť,Λγ=Ĩ,Λβ=ħ;int ΛЋ=Ũ;ΒΨ(Ηδ);Ζ=Ι;Ф=Λξ;Ƞ=Λν;њ=w;љ=h;ф=d;ȣ=Αϋ;ǔ=ΕΑ;ž=Λυ;ť=Λτ;η=Τ.ƌ;ΑΙ(true,true);Ĩ=Λγ;ħ=Λβ;Ũ=ΛЋ;η=Τ.ƌ;Ǚ=Τ.ƌ;Μ="Imported job, run CONT";return;}if(α==ǒ.Βί)
{Αρ=false;Μ="Imported "+Ψ.Count+" waypoints";return;}ΒΨ(Ηδ);Μ=α==ǒ.ΔΠ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗΙ(){Ţ.Clear();for(int i=Я.Count-1;i>=0;i--){String Η=Я[i].CustomData.ToUpper();bool Εά=false;if(Η=="INSTRUCTIONS"){Εά=true;Βϋ=true;}
if(Η=="DEBUG") Εά=true;if(Εά){Ţ.Add(Я[i]);Я.RemoveAt(i);}}ř(Ţ,false,1,false);}void ΗΚ(List<IMyTerminalBlock>Ɩ){Ȥ.Clear();for(int i=0;i<Ɩ.Count;i++){IMyTerminalBlock Ϋ=Ɩ[i];try{String Θυ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϋ.CustomName.IndexOf(Θυ);int Ʒ=-1;if(Β<0||!int.TryParse(Ϋ.CustomName.Substring(Β+Θυ.Length,1),out Ʒ)) continue;
if(Ʒ==-1) continue;Ʒ--;IMyTextSurfaceProvider Θχ=(IMyTextSurfaceProvider) Ϋ;if(Ʒ<Θχ.SurfaceCount&&Ʒ>=0)
Ȥ.Add(Θχ.GetSurface(Ʒ));}catch{}}}String ΓΚ="";void ΗΔ(){String Εκ="";String ΑΫ="";ΓΚ=Ζα(false);Εκ+=ΓΚ;ΑΫ+=ΓΚ;ΑΫ+=Ιϋ();for(int i=0;i<Я.Count;i++) Я[i].WriteText(Εκ);for(int i=0;i<Ȥ.Count;i++) Ȥ[i].WriteText(Εκ);Echo(ΑΫ);for(int i=0;i<Ţ.Count;i++){IMyTextPanel ѐ=Ţ[i];
String Η=ѐ.CustomData.ToUpper();if(Η=="DEBUG") ѐ.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") ѐ.WriteText(Ζχ());}}string Ζχ(){String Γ="";try{float Θβ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ƕ*Θβ)+" / "+(int)(Ʃ*Θβ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ƕ*100f,1)+"% / "+Math.Round(Ʃ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ΑΞ,2)+"ms / "+Math.Round(Εξ,2)+"ms \n";}catch{}for(int i=0;i<ĭ.Count;i++)
Γ+=""+ĭ.Keys.ElementAt(i)+" = "+Math.Round(ĭ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ĭ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙІ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ř(Я,setLCDFontAndSize,0.9f,false);ř(Ȥ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Я.Count;i++) Я[i].WriteText(Γ+Ī.ToString());for(int i=0;i<Ȥ.Count;i++) Ȥ[i].WriteText(Γ+Ī.ToString());}void Ȳ(){if(!Βϋ) return;try{Εό=Runtime.CurrentInstructionCount;}catch{}
}void Ȭ(String ΰ){if(!Βϋ) return;if(Εό==0) return;try{float Γη=(Runtime.CurrentInstructionCount-Εό)/Runtime.MaxInstructionCount*100;if(!ĭ.ContainsKey(ΰ))
ĭ.Add(ΰ,new float[]{Γη,Γη});else{ĭ[ΰ][0]=Γη;ĭ[ΰ][1]=Math.Max(Γη,ĭ[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΙΤ(String ț){if(!Ɨ) return;String Ц=""+BROADCAST_SEP;String ε=VERSION+Ц;ε+=ǃ(Ƶ)+Ц;
ε+=(int) Ξ+Ц;ε+=ƅ(β.X)+""+Ц;ε+=ƅ(β.Y)+Ц;ε+=ƅ(β.Z)+Ц;if(Ξ!=Ν.ί)
ε+=ΓЇ(Ǚ)+(Ǚ==Τ.Л?" "+Math.Round(Ακ,1)+"%":"")+Ц;else ε+=ΓЇ(Ǚ)+Ц;ε+=ǃ(Μ)+Ц;ε+=Г+""+Ц;ε+=ǃ(ΓΚ)+Ц;ε+=ȗ+Ц;ε+=ȟ+Ц;for(int i=0;i<Ȁ.Count;i++){if(Ȁ[i].Ʀ!=а.Ƈ) continue;ε+=ǃ(Ȁ[i].ΰ)+"/"+ǃ(Ȁ[i].ω)+"/"+Ȁ[i].Џ+Ц;}Κί(ε,ț);}String ǃ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Κί(String ε,String ț){ΒΫ(ε,BROADCAST_SELF_ID,ț);}void ΒΫ(String ε,String ζ,String ț){try{if(!Ɨ) return;ε=BROADCAST_TAG+BROADCAST_SEP+Ζσ()+BROADCAST_SEP+ζ+BROADCAST_SEP+ΔІ+BROADCAST_SEP+ț+BROADCAST_SEP+ε;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ε);}catch(Exception e){Ī=e;}
}String Ζσ(){if(Ξ!=Ν.Ǒ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔΖ(String ε){return ε.StartsWith(BROADCAST_TAG);}bool н(ref String Η,out string Αί,Char Ц){int Β=Η.IndexOf(Ц);Αί="";if(Β<0) return false;Αί=Η.Substring(0,Β);Η=Η.Remove(0,Β+1);return true;}bool ΗΓ(ref String ε,out String Ȣ,out String ț)
{Ȣ="";ț="";if(!Ɨ) return false;String Ȉ="";if(!н(ref ε,out Ȉ,BROADCAST_SEP)||!ΔΖ(Ȉ)) return false;if(!н(ref ε,out Ȣ,BROADCAST_SEP)) return false;if(!н(ref ε,out Ȉ,BROADCAST_SEP)||(Ȉ!=Ζσ()&&(Ȉ!="*"&&Ξ!=Ν.Ǒ))) return false;if(!н(ref ε,out Ȉ,BROADCAST_SEP)||(Ȉ!=ΔІ)) return false;
if(!н(ref ε,out ț,BROADCAST_SEP)) return false;return true;}void ΖЃ(){bool ΑΤ=true;if(ΗЌ){ΗЌ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Αε=(Ξ!=Ν.Ǒ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Αε)
ΖЄ(Αε);String[]Ο=Me.CustomData.Split('\n');Ɨ=bool.Parse(ΒΧ(ΒΔ(Ο,"Enable_Broadcast",ref ΑΤ)));bool ΖΤ=false;bool ΗЀ=true;if(Ɨ){if(Ξ!=Ν.Ǒ)
Ƶ=ΒΧ(ΒΔ(Ο,"Ship_Name",ref ΑΤ)).Replace(BROADCAST_SELF_ID,"");ΔІ=ΒΧ(ΒΔ(Ο,"Broadcast_Channel",ref ΑΤ)).ToLower();ΗЀ=false;if(Ǻ==null){Ǻ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǻ.SetMessageCallback("");}List<IMyRadioAntenna>ƒ=new List<IMyRadioAntenna>();χ.GetBlocksOfType(ƒ);
bool Δψ=false;for(int i=0;i<ƒ.Count;i++)
if(ƒ[i].EnableBroadcasting&&ƒ[i].Enabled){Δψ=true;break;}if(ƒ.Count==0) Μ="No Antenna found";else if(!Δψ) Μ="Antenna not ready";ΖΤ=ƒ.Count==0||!Δψ;if(Ηή&&!ΖΤ&&Ξ!=Ν.Ǒ) Μ="Antenna ok";}else if(Ξ==Ν.Ǒ)
Μ="Offline - Enable in PB custom data";Ηή=ΖΤ;if(ΗЀ){if(Ǻ!=null) this.IGC.DisableBroadcastListener(Ǻ);Ǻ=null;}}catch{ΑΤ=false;}if(!ΑΤ){Μ="Reset custom data";ΖЄ(Αε);}}void ΖЄ(String Αε){String Η=Αε+"\n\n"+"Enable_Broadcast="+(Ξ==Ν.Ǒ?"true":"false")+" \n";Η+=Ξ!=Ν.Ǒ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Η+"Broadcast_Channel=#default";}String ΒΧ(String Ŭ){int Β=Ŭ.IndexOf("//");if(Β!=-1) Ŭ=Ŭ.Substring(0,Β);String[]ψ=Ŭ.Split('=');if(ψ.Length<=1) return "";return ψ[1].Trim();}String ΒΔ(String[]Ο,String э,ref bool ċ){foreach(String Ŭ in Ο)
if(Ŭ.StartsWith(э)) return Ŭ;ċ=false;return "";}void Ιι(string Ъ){if(Ġ) ώ=new List<ϊ>();String Ȣ="";String ț="";if(!ΔΖ(Ъ)) ΚΠ(Ъ);bool Θώ=false;int Ερ=50;while(Ǻ!=null&&Ǻ.HasPendingMessage&&Ερ-->0){MyIGCMessage ε=Ǻ.AcceptMessage();String Η=ε.Data as String;if(Η==null) continue;
if(ΗΓ(ref Η,out Ȣ,out ț)&&Ȣ!=""&&Ȣ!=BROADCAST_SELF_ID){ϊ Δ=ΓЄ(Ȣ,"");if(Δ==null){Δ=new ϊ(Ȣ);ώ.Add(Δ);}Δ.ĺ(ț,false,false,true);Κϋ(Δ,Η);Θώ=true;}}if(Θώ)
ώ.Sort(delegate(ϊ a,ϊ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ĥ||Ġ){if(Αχ<=0||Ġ){Μ="";Ġ=false;Κέ();ΖЃ();Εδ=DateTime.Now;}}if(Ħ) Κσ();if(Ħ) Κχ();if(Ħ) ΚЀ();if(Ħ) Κό();if(Ħ&&ΖΒ==0) Κρ();
Κπ();}void Κϋ(ϊ Δ,String Η){Δ.Εα=DateTime.Now;String Αί="";н(ref Η,out Δ.Γλ,BROADCAST_SEP);н(ref Η,out Δ.ΰ,BROADCAST_SEP);if(Δ.Γλ!=VERSION) return;String Απ="";String[]Ư=new string[3];н(ref Η,out Απ,BROADCAST_SEP);н(ref Η,out Ư[0],BROADCAST_SEP);н(ref Η,out Ư[1],BROADCAST_SEP);
н(ref Η,out Ư[2],BROADCAST_SEP);н(ref Η,out Δ.ΕЎ,BROADCAST_SEP);н(ref Η,out Δ.Μ,BROADCAST_SEP);н(ref Η,out Αί,BROADCAST_SEP);н(ref Η,out Δ.ΓΛ,BROADCAST_SEP);String ΙΚ="",Θε="";н(ref Η,out ΙΚ,BROADCAST_SEP);н(ref Η,out Θε,BROADCAST_SEP);Δ.А.Clear();while(true){String ΘΤ;if(!н(ref Η,out ΘΤ,BROADCAST_SEP)) break;
String[]ΒА=ΘΤ.Split('/');if(ΒА.Count()<3) continue;int Џ=0;if(!int.TryParse(ΒА[2],out Џ)) continue;Δ.А.Add(new у(ΒА[0],ΒА[1],Џ,а.Ƈ));}int Θι=0;if(!int.TryParse(Αί,out Δ.Г)) Δ.Г=0;if(!int.TryParse(Απ,out Θι)) Δ.Ξ=Ν.ļ;if(!float.TryParse(Ư[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ư[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ư[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(ΙΚ,out Δ.ȗ)) Δ.ȗ=0;if(!float.TryParse(Θε,out Δ.ȟ)) Δ.ȟ=0;Δ.Ξ=(Ν) Θι;Δ.ΕΔ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΖΛ=0;for(int i=0;i<Δ.А.Count();i++) Δ.ΖΛ+=Δ.А[i].Џ;}ϊ ΓЄ(String Θ,String ΰ){Θ=Θ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<ώ.Count;i++){if(Θ!=""&&ώ[i].Θ.ToUpper()==Θ) return ώ[i];if(ΰ!=""&&ώ[i].ΰ!=null&&ώ[i].ΰ.ToUpper()==ΰ) return ώ[i];}return null;}String ƚ="";ϊ Ηξ=null;DateTime Ηο=DateTime.MinValue;List<ϊ>Ğ=new List<ϊ>();const float ΙΩ=20f;bool ΗΜ(ϊ ζ,String Ї){return Ї==ƚ&&ζ==Ηξ&&((DateTime.Now-Ηο).TotalSeconds<ΙΩ||
(Ї=="HOMEPOS"&&(ǿ.Count>0||Ĉ!=null)));}String Ιτ(ϊ ζ){if(ƚ==""||!ΗΜ(ζ,ƚ)) return "";String s="";int ċ=0,ΕЉ=0;for(int i=0;i<Ğ.Count;i++){Ў Γί=Ğ[i].ĺ(ƚ,false,false,false);if(Γί==Ў.ΑΒ) ċ++;bool Ατ=ǿ.Contains(Ğ[i]);if(ΕЉ++<8) s+="  "+Ğ[i].ΰ+": "+(Ατ?"waiting to leave ("+(ǿ.IndexOf(Ğ[i])+1)+")":Γί==Ў.ΑΒ?"received":
Γί==Ў.ƍ?"pending...":Γί==Ў.ΒΤ?"no answer!":"-")+"\n";}if(ΕЉ>8) s+="  +"+(ΕЉ-8)+" more\n";return "--------------------------------------------\n"+ƚ+": "+ċ+"/"+Ğ.Count+" received\n"+s;}void Ƿ(ϊ Δ,String Ї){ƚ=Ї;Ηξ=Δ;Ηο=DateTime.Now;Ğ.Clear();if(Δ==null) Ğ.AddRange(ώ);else Ğ.Add(Δ);
if(Δ==null){ǿ.Clear();Ĉ=null;}else ǿ.Remove(Δ);if(Δ==null&&Ї=="HOMEPOS"){Κε();return;}if(Δ==null){for(int i=0;i<ώ.Count;i++)
ώ[i].ĺ(Ї,true,false,false);ΒΫ(Ї,"*",Ї);}else{Δ.ĺ(Ї,true,false,false);ΒΫ(Ї,Δ.Θ,Ї);}}void ΚΠ(string Ъ){if(Ъ=="") return;var ψ=Ъ.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var Ї=ψ.ElementAtOrDefault(0);var Ā=ψ.ElementAtOrDefault(1);String ĉ="Invalid argument: "+Ъ;if(Ё==Φ.ΑΕ){if(П!=null)
{switch(Ї){case "UP":{if(Г<2) break;if(П.Г==0){Г=1;return;}Ƿ(П,"UP");return;}case "DOWN":{if(Г<1) break;if(Г==1){Ƿ(П,"MRES");break;}Ƿ(П,"DOWN");break;}case "APPLY":{if(Г<2) break;Ƿ(П,"APPLY");return;}}}}switch(Ї){case "UP":this.ΔΛ(false);return;case "DOWN":this.ΔΚ(false);return;
case "APPLY":this.ƃ(true);return;}switch(Ї){case "CLEAR":Ιγ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ā)){Μ="Usage: SENDALL <cmd>";return;}Ƿ(null,Ā);return;case "SEND":
if(Ъ.Length<="SEND".Length+1){Μ="Usage: SEND <ship>:<cmd>";return;}Κΰ(Ъ.Remove(0,"SEND".Length+1));return;}Μ=ĉ;}void Κΰ(String ΑΜ){if(ΑΜ=="") return;var ψ=ΑΜ.Split(':');if(ψ.Length!=2){Μ="Missing separator \":\"";return;}ψ.DefaultIfEmpty("");String Ƶ=ψ.ElementAtOrDefault(0).Trim();
ϊ Δ=ΓЄ("",Ƶ);if(Δ!=null) Ƿ(Δ,ψ.ElementAtOrDefault(1).Trim());else Μ="Unknown ship: "+Ƶ;}void Ιγ(){ώ.Clear();П=null;ο(Φ.Main);Г=0;}void Κέ(){List<IMyTerminalBlock>Αώ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Я,ǡ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αώ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Я=Ǵ(Я,pamTag,true);ΗΚ(Αώ);ΗΙ();Κή();ř(Я,setLCDFontAndSize,1.15f,false);ř(Ȥ,setLCDFontAndSize,1.15f,true);String ΛΫ=Ζψ(false);String Θξ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ѐ in Я){if(ѐ.CustomData==""){ѐ.CustomData=Ζψ(true);continue;}
if(!ѐ.CustomData.Contains(ΛΫ))
if(!ѐ.CustomData.Contains(Θξ)) ѐ.CustomData=Θξ+ѐ.CustomData;}}void Κπ(){String ΑΫ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȥ.Count;i++)
Ȥ[i].WriteText(Ζί(Ш.ΖЍ,"0",false));for(int i=0;i<Я.Count;i++){Ш Д=Ш.ļ;String ȅ="";ΚΚ(Я[i],out Д,out ȅ);Я[i].WriteText(Ζί(Д,ȅ,i==0));}Echo(ΑΫ+Ιφ());for(int i=0;i<Ţ.Count;i++){IMyTextPanel ѐ=Ţ[i];String Η=ѐ.CustomData.ToUpper();if(Η=="DEBUG") ѐ.WriteText(""+"\n"+"");
if(Η=="INSTRUCTIONS") ѐ.WriteText(Ζχ());}}String ƃ(bool Λ){int ΛΟ=0;return ƃ(Λ,0,ref ΛΟ,false,1);}String ƃ(bool Λ,int Λψ,ref int ΛЇ,bool Ης,int Ας){String Γ="";String ƴ="——————————————————\n";String ΜΖ="--------------------------------------------\n";if(П==null||Ё==Φ.Main||Ё==Φ.Δα)
Γ+="[PAM]-Controller | "+ώ.Count+" Connected ships"+"\n"+Ιψ()+Ιύ()+ΙЂ();else Γ+=Ζϋ(П)+"\n";Γ+=ƴ;int Β=0;int Κ=Г;Ů=0;if(Μ!=""){Π(ref Γ,Κ,Β++,Λ,Μ);}else if(Ё==Φ.Main){if(П==null&&ώ.Count>=1) П=ώ[0];String В="";if(Π(ref В,Κ,Β++,Λ," Send command to all")) ο(Φ.Δα);
int Ƴ=0;for(int s=0;s<ώ.Count;s++){if(Ης) В+="\n";ϊ Δ=ώ[s];if(Κ==Β||Κ==Β+1) П=Δ;if(Β==Κ) Ƴ=В.Split('\n').Length-1;if(Π(ref В,Κ,Β++,Λ," "+Ζϋ(Δ))){П=Δ;ο(Φ.ΑΕ);П.ĺ("",false,true,false);}if(Β==Κ) Ƴ=В.Split('\n').Length-1;if(Π(ref В,Κ,Β++,Λ," "+(Ης?"":"  ")+ΙϏ(Δ)))
{П=Δ;ο(Φ.ΗΛ);}}int ƿ=Ας-2;ƿ+=Math.Max(0,(Λψ-1))*Ας;Γ+=Δΰ(ƿ,В,Ƴ,ref ΛЇ);}else if(Ё==Φ.Βά||Ё==Φ.Δα){Φ ΚЉ=Ё==Φ.Βά?Φ.ΑΕ:Φ.Main;ϊ ζ=null;if(Ё==Φ.Βά) ζ=П;if(Π(ref Γ,Κ,Β++,Λ," Back")){ο(ΚЉ);}if(Π(ref Γ,Κ,Β++,Λ," [Stop] "+Ɔ(ζ,"STOP"))){Ƿ(ζ,"STOP");}
if(Π(ref Γ,Κ,Β++,Λ," [Continue] "+Ɔ(ζ,"CONT"))){Ƿ(ζ,"CONT");}if(Π(ref Γ,Κ,Β++,Λ," [Move home] "+Ɔ(ζ,"HOMEPOS"))){Ƿ(ζ,"HOMEPOS");}if(Π(ref Γ,Κ,Β++,Λ," [Move to job] "+Ɔ(ζ,"JOBPOS"))){Ƿ(ζ,"JOBPOS");}if(Π(ref Γ,Κ,Β++,Λ," [Full simulation] "+Ɔ(ζ,"FULL"))){Ƿ(ζ,"FULL");}
if(ζ!=null&&ζ.Ξ==Ν.ί)
if(Π(ref Γ,Κ,Β++,Λ," [Undock] "+Ɔ(ζ,"UNDOCK"))){Ƿ(ζ,"UNDOCK");}Γ+=Ιτ(ζ);}else if(Ё==Φ.ΑΕ){String ΔϏ="";if(Π(ref ΔϏ,Κ,Β++,Λ," Back")){ο(Φ.Main);}Γ+=ΔϏ.Substring(0,ΔϏ.Length-1).PadRight(36,' ');if(Π(ref Γ,Κ,Β++,Λ," Send cmd")){ο(Φ.Βά);}if(!П.ΔΗ()&&!П.ΔΓ()) Ů++;
if(П.ΔΓ()) Γ+="No answer received...";else Γ+=Ə(П,П.ΓΛ);}else if(Ё==Φ.ΗΛ){if(Π(ref Γ,Κ,Β++,Λ," Back")){ο(Φ.Main);}Γ+=ΜΖ;Γ+=Ə(П,Ζϊ(П));}if(!Επ) Г=Math.Min(Ů-1,Г);return Γ;}String Ζϋ(ϊ Δ){if(Δ.Γλ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ə(Δ,Δ.ΕЎ)+" "+Δ.ΕΔ+"m";
}String ΙϏ(ϊ Δ){String s=Δέ("",М(Δ.ȗ,Δ.ȟ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.А.Count;i++){if(i>=5) break;у Ѓ=Δ.А[i];s+=ΗΨ(Γφ(Ιό(Ѓ)),3)+" "+Ζρ(Ѓ.Џ)+" ";}return s;}String Ιό(у Ѓ){if(Ѓ.ω.ToUpper()=="ORE"||Ѓ.ω.ToUpper()=="INGOT"){String Ηπ=Ιϊ(Ѓ.ΰ.ToUpper());if(Ηπ!="") return Ηπ;
}return Ѓ.ΰ;}String Ə(ϊ Δ,String Γ){if(Δ.ΔΗ()) return "No signal...("+ΒΗ((int)(DateTime.Now-Δ.Εα).TotalSeconds)+")";return Γ;}String Ζψ(bool Μέ){String Γ="";if(Μέ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΚΚ(IMyTerminalBlock ѐ,out Ш Д,out String ȅ){bool ċ=true;String Απ=ΒΧ(ΒΔ(ѐ.CustomData.Split('\n'),"mode",ref ċ)).ToUpper();String[]ψ=Απ.Split(':');String Αο=ψ.Length>0?ψ[0].Trim():"";ȅ=ψ.Length>1?ψ[1].Trim():"";
Д=Ш.ļ;if(Αο=="MAIN") Д=Ш.ΖЎ;else if(Αο=="MAINX") Д=Ш.ΖЏ;else if(Αο=="MENU") Д=Ш.ΔΙ;else if(Αο=="INVENTORY") Д=Ш.ΔΒ;else if(Αο=="DEBUG") Д=Ш.Γϊ;if(Д==Ш.ļ) ȅ=Απ;}String Ζί(Ш Д,String ȅ,bool ΛΡ){int Αμ=15;if(ΛΡ){Ητ=Βρ;Βρ=0;Ηυ=Βς;Βς=0;}String Γ="";if(Д==Ш.ΖЎ)
{int я=0;Γ=ƃ(false,Ητ,ref ΛΑ,true,Αμ);if(!int.TryParse(ȅ,out я)) return Γ;Βρ=Math.Max(я,Βρ);я--;return ΗΒ(я,Αμ,Γ);}else if(Д==Ш.ΖЏ){int я=0;Γ=ƃ(false,Ηυ,ref ΛΒ,false,Αμ);if(!int.TryParse(ȅ,out я)) return Γ;Βς=Math.Max(я,Βς);я--;return ΗΒ(я,Αμ,Γ);}if(Д==Ш.ΖЍ){return ƃ(false,0,ref ΛΓ,true,Αμ);
}else if(Д==Ш.ΔΒ||Д==Ш.ΔΙ||Д==Ш.Γϊ){ϊ Δ=П;if(ȅ!=""){Δ=ΓЄ("",ȅ);if(Δ==null) return "Unknown ship: "+ȅ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƴ="—————————————————————\n";String ΖΘ="";String ΔЅ="";if(Д==Ш.ΔΙ){ΖΘ="Menu";ΔЅ=Ə(Δ,Δ.ΓΛ);}
else if(Д==Ш.ΔΒ){ΖΘ="Inventory";ΔЅ=Ə(Δ,Ζϊ(Δ));}else if(Д==Ш.Γϊ){return Ə(Δ,ΙЀ(Δ));}Γ+=Δ.ΰ+" - "+Δ.ΕΔ+"m | "+Γφ(""+ΖΘ)+"\n"+ƴ;Γ+=ΔЅ;return Γ;}return "Unknown command: "+ȅ;}string ΙЀ(ϊ Δ){String s="";s+=Δ.Γλ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕЎ+"\n";s+=Δ.Μ+"\n";s+=Δ.Г+"\n";s+=Δ.ΓΛ.Length+"\n";s+=Δ.ȗ+"\n";s+=Δ.ȟ+"\n";s+=Δ.А.Count()+"\n";return s;}string Ζϊ(ϊ Δ){String s="";s+=Δέ("All",М(Δ.ȗ,Δ.ȟ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.А.Count();i++){у Ѓ=Δ.А[i];
s+=Δέ(Ѓ.ΰ,Ѓ.Џ,Δ.ΖΛ,30,8,10)+"\n";}return s;}Ў Ιυ(ϊ Δ,bool з,string Ї){Ў α=Ў.None;if(Δ==null){for(int i=0;i<ώ.Count;i++){Ў Ј=ώ[i].ĺ(Ї,false,з,false);if(Ј==Ў.ΑΒ) α=Ј;if(Ј==Ў.ΒΤ) return Ј;if(Ј==Ў.ƍ) return Ј;}return α;}else return Δ.ĺ(Ї,false,з,false);}String Ɔ(ϊ Δ,String Ї)
{if(!ΗΜ(Δ,Ї)) return "";Ў Ј=Ιυ(Δ,false,Ї);if(Ј==Ў.ΑΒ) return "received!";if(Ј==Ў.ƍ) return "pending...";if(Ј==Ў.ΒΤ) return "no answer!";return "";}String Δΰ(int ƿ,String Γ,int Ƴ,ref int Į){String[]Ο=Γ.Split('\n');if(Ƴ>=Į+ƿ-1) Į++;Į=Math.Min(Ο.Count()-1-ƿ,Į);if(Ƴ<Į+1) Į--;
Į=Math.Max(0,Į);String α="";for(int i=0;i<ƿ;i++){int Θέ=i+Į;if(Θέ>=Ο.Count()) break;α+=Ο[Θέ]+"\n";}return α;}String ΗΒ(int я,int Ας,String Γ){String[]Ο=Γ.Split('\n');int Αϋ=я*Ας;int ș=(я+1)*Ας;String α="";for(int i=Αϋ;i<ș;i++){if(i>=Ο.Count()) break;α+=Ο[i]+"\n";
}return α;}String Γφ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΗΨ(String s,int Θγ){if(s=="") return s;if(s.Length>Θγ) s=s.Substring(0,Θγ-1)+".";return s;}string Δέ(String Ū,float value,float ƨ,int Ηε,int ΘΨ,int Λχ){float Ŷ=М(value,ƨ)*Ηε;
String Βμ="[";for(int i=0;i<Ηε;i++){if(i<=Ŷ) Βμ+="|";else Βμ+="'";}Βμ+="]";return Βμ+" "+ΗΨ(Γφ(Ū),ΘΨ).PadRight(ΘΨ)+"".PadRight(Λχ)+Ζρ(value);}String Ζρ(float Џ){if(Џ>=1000000) return Math.Round(Џ/1000000f,Џ/1000000f<100?1:0)+"M";if(Џ>=1000) return Math.Round(Џ/1000f,Џ/1000f<100?1:0)+"K";
return ""+Math.Round(Џ);}String ΒΗ(int Αψ){if(Αψ>=60*60) return Math.Round(Αψ/(60f*60f),1)+" h";if(Αψ>=60) return Math.Round(Αψ/60f,1)+" min";return ""+Αψ+" s";}string ƅ(float v){return Math.Round(v,2)+" ";}string Μδ(Vector3 v){return "X"+ƅ(v.X)+"Y"+ƅ(v.Y)+"Z"+ƅ(v.Z);}
List<IMyCameraBlock>Ȕ=new List<IMyCameraBlock>();List<long>Ƭ=new List<long>();bool Ų=false;int ΔЋ=0,ΛЄ=0,ΔЃ=0;const int ΚΧ=16;int ΔЉ=0;DateTime Ηθ=DateTime.Now;String ΔЄ="";String ΔЈ="";void Κά(){Ȕ.Clear();Ƭ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȕ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȕ.Count;i++) Ȕ[i].EnableRaycast=true;List<IMyTerminalBlock>Βθ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Βθ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<Βθ.Count;i++) if(!Ƭ.Contains(Βθ[i].CubeGrid.EntityId)) Ƭ.Add(Βθ[i].CubeGrid.EntityId);if(!Ƭ.Contains(Me.CubeGrid.EntityId)) Ƭ.Add(Me.CubeGrid.EntityId);
ΔЈ="";if(Ϊ==null) return;MatrixD m=Ϊ.WorldMatrix;Vector3[]ΛΚ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Λο={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ȕ.Count;i++)
if(Vector3.Dot(Ȕ[i].WorldMatrix.Forward,ΛΚ[d])>0.7f){ΔЈ+=Λο[d]+" ";break;}}bool ΖЇ(Α s){return s==Α.ǵ||s==Α.т||s==Α.И||
s==Α.ȭ||s==Α.Ƅ||s==Α.ΒΕ||
(s==Α.ȍ&&ǫ!=null);}void ΑБ(){Ų=false;ΔЋ=0;ΔЄ="";}void Κω(){if(!obstacleBrake||Ȕ.Count==0||!ΖЇ(Ω)){ΑБ();return;}Vector3 Αΰ=Ϗ+Ĳ;Vector3 Ȧ=Αΰ-β;float ȉ=Ȧ.Length();if(ȉ<1f){ΑБ();return;}Vector3 ρ=Ȧ/ȉ;Vector3 ж=Ϊ!=null?(Vector3) Ϊ.GetShipVelocities().LinearVelocity:new Vector3();
if(ж.Length()>2) ρ=Vector3.Normalize(ж);Vector3 ΓΫ=Vector3.Cross(ρ,Math.Abs(ρ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΓΫ.Normalize();Vector3 Љ=Vector3.Cross(ΓΫ,ρ);float υ=Math.Min(ȉ+δ*0.25f,ď*obstacleLookAhead+δ);int Ηλ=0;for(int k=0;k<2;k++){int p=k==0?0:ΛЄ++%8,e=p%4;
Vector3 ΓΟ=k==0?new Vector3():(e==0?ΓΫ:e==1?-ΓΫ:e==2?Љ:-Љ)*δ*0.5f;Vector3 ș=β+ρ*(p>=4?υ*0.5f:υ)+ΓΟ;IMyCameraBlock Ĝ=Ιέ(ș);if(Ĝ==null){Ζι();continue;}Ηλ++;MyDetectedEntityInfo Ǖ=Ĝ.Raycast(ș);bool ΜΧ=Ǖ.Type==MyDetectedEntityType.Planet||Ǖ.Type==MyDetectedEntityType.Asteroid;
bool Θο=!Ǖ.IsEmpty()&&!Ƭ.Contains(Ǖ.EntityId)&&!ΚΒ(Ǖ.EntityId)&&
Ǖ.Type!=MyDetectedEntityType.FloatingObject&&!(ΜΧ&&(k>0||Ω==Α.ǵ||
(Ǖ.HitPosition.HasValue&&Vector3D.Distance(Ǖ.HitPosition.Value,Αΰ)<δ)));if(Θο){if(!Ų) Ηθ=DateTime.Now;Ų=true;ΔЋ=0;ΔЄ=Ǖ.Type.ToString();}else Ζι();if(debugDraw&&Βό.Count<16) Βό.Add(π(Ĝ.GetPosition())+","+π(ș)+","+(Θο?1:0));}ΔЃ=Ηλ>0?0:ΔЃ+1;if(ΔЃ>12) Μ="Cameras can't see ahead";
if(Ų&&(DateTime.Now-Ηθ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΔЄ;}void Ζι(){if(Ų&&++ΔЋ>=ΚΧ) ΑБ();}IMyCameraBlock Ιέ(Vector3 ș){IMyCameraBlock Ћ=null;float Ηη=-2;for(int n=0;n<Ȕ.Count;n++){ΔЉ=(ΔЉ+1)%Ȕ.Count;IMyCameraBlock c=Ȕ[ΔЉ];if(!c.IsFunctional||!c.IsWorking) continue;float Θϋ=Vector3.Dot(Vector3.Normalize(ș-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Θϋ>Ηη+0.05f&&c.CanScan(ș)){Ћ=c;Ηη=Θϋ;}}return Ћ;}const String Δν="[PAMTFC]";const float Κκ=5f;const float ΓЃ=2f;class Ļ{public long Θ;public Vector3 ά,ж;public Vector3 ȝ;public float ΗϏ;public String Ι="";public int Ģ=-1;public bool Ατ;public int ȁ;public int ǝ;public float Αβ,Αή;
public long γ=0;public int ΑΧ=0;public DateTime ȥ;}IMyBroadcastListener Γζ=null;long ю=0;List<Ļ>џ=new List<Ļ>();Vector3 Ĳ=new Vector3();float Ǎ=-1;Vector3 ΖΜ=new Vector3();bool Γε=false;DateTime ΖΝ=DateTime.Now;const float ΗΦ=15f;const float Κι=10f;int ΙΗ=0;String к="";int Й=-1;
bool Βυ=false,Βτ=false,Αέ=false;Vector3 Ηφ=new Vector3();float Ɗ(){return Ĩ>0?Ĩ:ȩ;}float Ɖ(){return ħ>0?ħ:Ȩ;}String Ŗ(){if(η==Τ.ы||Ξ==Ν.ί) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Б+"|"+Ю+"|"+(int) ũ+"|"+Math.Round(Ɗ(),1)+"|"+Math.Round(Ɖ(),1)+"|"+Ũ;}void Κώ(){if(!trafficEnabled) return;if(Γζ==null) Γζ=IGC.RegisterBroadcastListener(Δν);ю=Me.CubeGrid.EntityId;int ǥ=30;while(Γζ.HasPendingMessage&&ǥ-->0){String Η=Γζ.AcceptMessage().Data as String;
if(Η!=null) ΚΜ(Η);}for(int i=џ.Count-1;i>=0;i--)
if((DateTime.Now-џ[i].ȥ).TotalSeconds>Κκ) џ.RemoveAt(i);if(Γβ==null) Γβ=IGC.RegisterBroadcastListener(ΗΤ);ǥ=5;while(Γβ.HasPendingMessage&&ǥ-->0){String Η=Γβ.AcceptMessage().Data as String;if(Η!=null) try{ΚΝ(Η);}catch{}}if(Βφ==null) Βφ=IGC.RegisterBroadcastListener(ΖЉ);
ǥ=10;while(Βφ.HasPendingMessage&&ǥ-->0){String Η=Βφ.AcceptMessage().Data as String;if(Η!=null) try{ΚΙ(Η);}catch{}}if(++ΙΗ<3) return;ΙΗ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",ю+";"+π(Ζ.Ε)+";"+
π(η==Τ.ы?new Vector3():Ζ.Є)+";"+π(Ƞ)+";"+
π(Ϗ+Ĳ)+";"+String.Join(":",Βό));Βό.Clear();}IGC.SendBroadcastMessage(Δν,ю+";"+π(β)+";"+
π(Ϊ.GetShipVelocities().LinearVelocity)+";"+δ+";"+Ŗ()+";"+
(η==Τ.Л?Й:-1)+";"+к+";"+Ιβ()+","+Ζξ()+";"+π(ΖΜ)+
";"+(ΒЀ?1:0)+";"+(Ņ&&η==Τ.Л?1:0)+","+ΕЇ+","+ΕІ+","+ΚΕ()+","+ΗΧ()+";"+Me.GetId());}void ΚΜ(String Η){try{String[]f=Η.Split(';');if(f.Length<7) return;long Θ=long.Parse(f[0]);if(Θ==ю) return;Ļ Δ=null;for(int i=0;i<џ.Count;i++) if(џ[i].Θ==Θ) Δ=џ[i];
if(Δ==null){Δ=new Ļ();Δ.Θ=Θ;џ.Add(Δ);}Δ.ά=κ(f[1]);Δ.ж=κ(f[2]);Δ.ΗϏ=float.Parse(f[3]);Δ.Ι=f[4];Δ.Ģ=int.Parse(f[5]);Δ.ȥ=DateTime.Now;if(Δ.Ι!=""&&Δ.Ι==Ŗ()) ΖА(f[6]);Δ.γ=0;Δ.ΑΧ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.γ=long.Parse(d[0]);Δ.ΑΧ=int.Parse(d[1]);
}Δ.ȝ=f.Length>=9?κ(f[8]):new Vector3();Δ.Ατ=f.Length>=10&&f[9]=="1";Δ.ȁ=0;Δ.ǝ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.Αβ=float.Parse(w[1]);Δ.Αή=float.Parse(w[2]);Δ.ȁ=int.Parse(w[3]);if(w.Length>=5) Δ.ǝ=int.Parse(w[4]);}}}catch{}
}bool ΚΒ(long Θ){for(int i=0;i<џ.Count;i++) if(џ[i].Θ==Θ) return true;return false;}Vector3 Ζω(int Β,int Λω){if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();έ ή=Ψ[Β];Vector3 ΜΤ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Λω;Vector3 Љ=ή.ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ή.ϋ):-ή.λ;
Vector3 ч=Vector3.Cross(ΜΤ,Љ);if(ч.LengthSquared()<0.0001f) return new Vector3();float υ=laneOffset*δ;float ΛΤ=Math.Min(Vector3.Distance(ή.Ε,Ψ[0].Ε),
Vector3.Distance(ή.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(ч)*υ*Math.Min(1,ΛΤ/(υ*4));}Vector3 ΙΖ=new Vector3();List<String>Βό=new List<String>();bool ΖЀ(){Α s=Ω;return Ŗ()!=""&&(s==Α.ǵ||s==Α.ŗ||s==Α.ȏ||
s==Α.ǣ||s==Α.Ŕ);}float ΖЋ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Б*Ɗ(),2)+Math.Pow(Ю*Ɖ(),2));}bool ΔΟ(){return Ŗ()!=""&&Vector3.Distance(β,Ζ.Ε)<ΖЋ()+δ*2;}int ΗΧ(){Α s=Ω;if(s==Α.ǣ||s==Α.ȭ||(s==Α.И&&ΔΟ())) return 2;return s==Α.ǵ||s==Α.ȏ||s==Α.Ŕ||s==Α.т?1:0;
}int ΚΕ(){if(!ΖЀ()) return 0;return Ω!=Α.ǵ&&Vector3.Dot(β-О,Ф)>δ*0.5f?2:1;}bool ΚΑ(Α s){return s==Α.ȍ||s==Α.ŏ||s==Α.œ||s==Α.ΑΓ||
s==Α.ǹ||s==Α.ȏ||s==Α.ǣ||s==Α.Ŕ||s==Α.ĵ;}void ΚϏ(){Ĳ=new Vector3();Ǎ=-1;ΖΜ=new Vector3();bool Αα=ΖЇ(Ω);if(!trafficEnabled||(!Αα&&!ΚΑ(Ω))){Γε=false;return;}ΙΖ=Ϊ.GetShipVelocities().LinearVelocity;Ιδ(Ϊ.GetNaturalGravity(),!Αα);ΙΠ();}IMyBroadcastListener Γβ=null;
float ƹ=-1;DateTime ΙΒ=DateTime.MinValue;void ΚΝ(String Η){if(!Η.StartsWith("C;")) return;ΙΒ=DateTime.Now;ƹ=-1;String Αξ=ю+":";String[]e=Η.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αξ)) ƹ=float.Parse(e[i].Substring(Αξ.Length));}void ΙΠ(){if(ƹ<0||(DateTime.Now-ΙΒ).TotalSeconds>1.5) return;
Ǎ=Ǎ<0?ƹ:Math.Min(Ǎ,ƹ);if(ƹ<0.5f) Μ="Traffic: giving way (controller)";}void Ιδ(Vector3 ϋ,bool Θτ=false){Ĳ=new Vector3();Ǎ=-1;Vector3 Ȧ=Ϗ-β;float ȉ=Ȧ.Length();bool ΕΧ=ȉ>=1f;Vector3 ρ=ΕΧ?Ȧ/ȉ:new Vector3();ΖΜ=ρ;Vector3 Љ=ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ϋ):-ΑΩ;
Vector3 ч=Vector3.Cross(ρ,Љ);ч=ч.LengthSquared()<0.0001f?-Αλ:Vector3.Normalize(ч);Ļ ΓΑ=null;bool ΗА=Ω==Α.И&&!Ę(1);int Εσ=ΗΧ();for(int i=0;i<џ.Count;i++){Ļ θ=џ[i];if(θ.ΑΧ==3&&θ.ж.LengthSquared()<0.25f) continue;Vector3 ά=θ.ά+θ.ж*(float)(DateTime.Now-θ.ȥ).TotalSeconds;
Vector3 ё=ά-β;float з=(δ+θ.ΗϏ)*0.5f;bool Αφ=θ.Ι!=""&&θ.Ι==Ŗ();if(Αφ&&θ.ȁ==2) continue;bool Γά=Αφ&&θ.ȁ==1&&ΖЀ();if(Γά) з=Math.Min(Ɗ(),Ɖ())*0.9f;float ΔЏ=з*(Γά?1f:1.2f);float Ě=ΕΧ?Vector3.Dot(ё,ρ):0;if(Ě>0&&(ё-ρ*Ě).Length()<=ΔЏ){float ΓΡ=Vector3.Dot(θ.ж,ρ);
bool ΘΓ=θ.ȝ.LengthSquared()>0.25f;bool ΘΕ=ΓΡ<-1f||(ΘΓ&&Vector3.Dot(θ.ȝ,ρ)<-0.5f);if(!Θτ&&θ.ǝ<=Εσ){if(θ.Ατ){if(!ΗА){Ĳ=ч*з*1.3f;continue;}float Θθ=Vector3.Distance(β,Ϗ),ΘΥ=Vector3.Distance(ά,Ϗ);if(Θθ<ΘΥ||(Θθ==ΘΥ&&ю<θ.Θ)) continue;}if(ΘΕ){if(Ě<(ď-ΓΡ)*6f+з*4f) Ĳ=ч*з*1.3f;
continue;}if(ΘΓ&&ю<θ.Θ){Vector3 Ηγ=-ё;float Θς=Vector3.Dot(Ηγ,θ.ȝ);if(Θς>0&&(Ηγ-θ.ȝ*Θς).Length()<=ΔЏ){Ĳ=ч*з*1.3f;continue;}}}float ΗЏ=Ě-з-(Γά?1f:followGap*δ);float ќ=Math.Max(0,ΓΡ)+Math.Max(0,ΗЏ)/ΓЃ;if(ΘΕ) ќ=Math.Max(0,ΗЏ+Math.Min(0,ΓΡ)*ΓЃ*2)/ΓЃ;
if(Ǎ<0||ќ<Ǎ){Ǎ=ќ;ΓΑ=θ;}continue;}if(θ.ж.LengthSquared()<1f) continue;if(ΕΧ&&(ё-ρ*Ě).Length()<=ΔЏ) continue;Vector3 ΕЄ=θ.ж-ΙΖ;float Θϊ=ΕЄ.LengthSquared();if(Θϊ<0.25f) continue;float t=-Vector3.Dot(ё,ΕЄ)/Θϊ;if(t<=0||t>5f||(ё+ΕЄ*t).Length()>=(Γά?з:з*1.1f)) continue;
if(θ.ǝ>Εσ||(θ.ǝ==Εσ&&ю>θ.Θ)||t<2f){Ǎ=0;ΓΑ=θ;}}bool Αζ=Ǎ>=0&&Ǎ<0.5f;if(!Αζ||Θτ||(ΓΑ!=null&&ΓΑ.Ατ&&ΗА)){Γε=false;return;}if(!Γε){Γε=true;ΖΝ=DateTime.Now;}double ΘΖ=(DateTime.Now-ΖΝ).TotalSeconds;if(ΘΖ>ΗΦ+Κι) ΖΝ=DateTime.Now;else if(ΘΖ>ΗΦ){Ǎ=-1;Ĳ=ч*δ*1.3f;Μ="Traffic: passing a stuck ship";}
}List<int>ć=new List<int>();String ΘΘ="";void ΓЂ(){String э=Б+"|"+Ю+"|"+(int) ũ;if(э==ΘΘ) return;ΘΘ=э;Ζΰ(Б,Ю,ũ,ć);}void Ζΰ(int w,int h,ї Αϋ,List<int>ΔЊ){ΔЊ.Clear();if(Αϋ==ї.ΑΘ){for(int k=0;k<w*h;k++){int ŋ=k/w;ΔЊ.Add(ŋ*w+(ŋ%2==0?k-ŋ*w:w-1-(k-ŋ*w)));
}return;}int Αδ=(int) Math.Ceiling(w/2f),Αγ=(int) Math.Ceiling(h/2f);int ΕΤ=w/2,ΕΣ=h/2;int x=0,y=0,ΕΗ=0,ΕΘ=-1,ƨ=Math.Max(w,h);for(int i=0;i<ƨ*ƨ;i++){if(-Αδ<x&&x<=ΕΤ&&-Αγ<y&&y<=ΕΣ) ΔЊ.Add((y-1+Αγ)*w+(x-1+Αδ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΕΗ;ΕΗ=-ΕΘ;ΕΘ=t;}
x+=ΕΗ;y+=ΕΘ;}}bool ΓЁ(){int n=Math.Max(0,Б*Ю);if(к.Length==n) return false;к=new String('0',n);Й=-1;return true;}void ΖА(String θ){ΓЁ();if(θ.Length!=к.Length) return;char[]h=к.ToCharArray();for(int i=0;i<h.Length;i++) if(θ[i]=='1') h[i]='1';к=new String(h);}bool ΒΜ(int в){return в>=0&&в<к.Length&&к[в]=='1';}
void Δγ(int в,bool φ){if(в<0||в>=к.Length) return;char[]h=к.ToCharArray();h[в]=φ?'1':'0';к=new String(h);}bool ΓЏ(int в,bool Λδ){if(!trafficEnabled) return false;String э=Ŗ();for(int i=0;i<џ.Count;i++){Ļ t=џ[i];if(t.Ι!=э||t.Ģ<0||(Λδ&&t.Θ>ю)) continue;if(t.Ģ==в) return true;
}return false;}int Ια(int Β){int n=Б*Ю;if(n<1) return Β;ΓЂ();if(ΓЁ()) for(int k=0;k<Β&&k<n;k++) Δγ(ć[k],true);if(Й>=0&&(Β>=n||ć[Β]!=Й)){Δγ(Й,true);Й=-1;}if(ΖЊ()){if(Ȗ>=0&&!ΒΜ(Ȗ)){Ņ=false;Й=Ȗ;return ć.IndexOf(Ȗ);}if(Ȗ==-2){Й=-1;Ņ=к.IndexOf('0')>=0;return Ņ?-2:n;}
if(Ȗ==-1&&Й>=0&&!ΒΜ(Й)){Ņ=false;return Β;}Й=-1;if(!Ņ){Ņ=true;Ηψ=DateTime.Now;}if((DateTime.Now-Ηψ).TotalSeconds<5) return-1;}Ņ=false;if(Й>=0&&!ΒΜ(Й)&&!ΓЏ(Й,true)) return Β;Й=-1;for(int k=0;k<n;k++){int c=ć[k];if(ΒΜ(c)||ΓЏ(c,false)) continue;Й=c;
return k;}return к.IndexOf('0')>=0?-2:n;}const String ΖЉ="[PAMJOB]";const String ΗΤ="[PAMTCC]";IMyBroadcastListener Βφ=null;DateTime Ηχ=DateTime.MinValue;DateTime Ηψ=DateTime.Now;int Ȗ=-1;bool Ņ=false;bool ΖЊ(){return trafficEnabled&&Ȗ!=-3&&(DateTime.Now-Ηχ).TotalSeconds<3;}
void ΚΙ(String Η){String[]f=Η.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ŗ()) return;Ηχ=DateTime.Now;ΖА(f[2]);Ȗ=-3;String Αξ=ю+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αξ)) Ȗ=int.Parse(e[i].Substring(Αξ.Length));}Ǹ Κα(int Β)
{ΓЂ();if(Β<0||Β>=ć.Count) return Ǹ.ΑЋ;Ѕ=ć[Β]%Б;Ђ=ć[Β]/Б;return Ǹ.ΒΘ;}class Ǣ{public long Θ;public Vector3 ά,г,Љ;}List<Ǣ>Ь=new List<Ǣ>();bool Ł=false;Vector3 ΒЂ,ΒЁ,ΒЃ,ΒЄ;long Ǘ=0;long č=0;έ ǫ=null;bool ΑΨ=false;έ Γώ(Vector3 ά,Vector3 г,Vector3 Љ){MatrixD m=MatrixD.CreateWorld(ά,г,Љ);
έ w=new έ(Ρ);w.μ=Vector3D.TransformNormal(ΒЂ,m);w.λ=Vector3D.TransformNormal(ΒЁ,m);w.Є=Vector3D.TransformNormal(ΒЃ,m);w.Ε=ά+(Vector3) Vector3D.TransformNormal(ΒЄ,m);w.ν=true;return w;}int Ζξ(){Α s=Ω;if(s==Α.і) return о(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.œ||s==Α.ȳ||s==Α.Œ||s==Α.Βε||
s==Α.ΒΓ||s==Α.ǹ) return 3;if(s==Α.ȍ||s==Α.ŏ||s==Α.ΑΓ) return 2;if(s==Α.И) return 1;return 0;}long Ιβ(){if(Ζξ()==0) return 0;if(Ę(1)&&ƶ!=0) return ƶ;return ǫ!=null?č:Ǘ;}void ΖЌ(){if(Ξ==Ν.ί||!Ρ.ν) return;IMyShipConnector Х=о(MyShipConnectorStatus.Connected);if(Х==null||Х.OtherConnector==null) return;
IMyShipConnector θ=Х.OtherConnector;Vector3I Λά=new Vector3I((int) Ρ.х.X,(int) Ρ.х.Y,(int) Ρ.х.Z);if(Х.Position==Λά){MatrixD ơ=MatrixD.Transpose(θ.WorldMatrix);ΒЂ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Forward,ơ);ΒЁ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Down,ơ);ΒЃ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Left,ơ);
ΒЄ=Vector3D.TransformNormal(Х.GetPosition()-θ.GetPosition(),ơ);Ł=true;}if(Vector3.Distance(Х.GetPosition(),Ρ.Ε)<5) Ǘ=θ.EntityId;List<IMyShipConnector>Ť=new List<IMyShipConnector>();χ.GetBlocksOfType(Ť,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ť.Count>0){Ь.Clear();for(int i=0;i<Ť.Count;i++){Ǣ p=new Ǣ();p.Θ=Ť[i].EntityId;p.ά=Ť[i].GetPosition();p.г=Ť[i].WorldMatrix.Forward;p.Љ=Ť[i].WorldMatrix.Up;Ь.Add(p);}}ǫ=null;č=0;if(θ.EntityId==Ǘ||!Ł) return;for(int i=0;i<Ь.Count;i++)
if(Ь[i].Θ==θ.EntityId){č=Ь[i].Θ;ǫ=Γώ(Ь[i].ά,Ь[i].г,Ь[i].Љ);}}bool ΔΕ(long Θ,int Θλ){if(Θ==0) return false;for(int i=0;i<џ.Count;i++){Ļ t=џ[i];if(t.γ==Θ&&(t.ΑΧ>Θλ||(t.ΑΧ==Θλ&&t.Θ<ю))) return true;}return false;}bool Γψ(int ŵ){if(Ξ==Ν.ί||!trafficEnabled){ǫ=null;č=0;return true;}
long σ=ǫ!=null?č:Ǘ;if(!ΔΕ(σ,ŵ)) return true;if(!ΔΕ(Ǘ,ŵ)){ǫ=null;č=0;return true;}if(!Ł) return false;Ǣ Ћ=null;float ΑΠ=0;for(int i=0;i<Ь.Count;i++){Ǣ p=Ь[i];if(p.Θ==Ǘ||ΔΕ(p.Θ,ŵ)) continue;float d=Vector3.Distance(p.ά,Ρ.Ε);if(Ћ==null||d<ΑΠ){Ћ=p;ΑΠ=d;}
}if(Ћ==null) return false;č=Ћ.Θ;ǫ=Γώ(Ћ.ά,Ћ.г,Ћ.Љ);return true;}String Ιξ(){String s=(Ł?"1":"0")+":"+π(ΒЂ)+":"+π(ΒЁ)+":"+
π(ΒЃ)+":"+π(ΒЄ)+":"+Ǘ;for(int i=0;i<Ь.Count;i++)
s+=":"+Ь[i].Θ+","+π(Ь[i].ά)+","+π(Ь[i].г)+","+π(Ь[i].Љ);return s;}void Ιν(String s){Ь.Clear();Ł=false;Ǘ=0;String[]f=s.Split(':');if(f.Length<6) return;Ł=f[0]=="1";ΒЂ=κ(f[1]);ΒЁ=κ(f[2]);ΒЃ=κ(f[3]);ΒЄ=κ(f[4]);long.TryParse(f[5],out Ǘ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǣ p=new Ǣ();if(!long.TryParse(e[0],out p.Θ)) continue;p.ά=κ(e[1]);p.г=κ(e[2]);p.Љ=κ(e[3]);Ь.Add(p);}}const String ǅ="[PAMDCK]";const float ΙΨ=120f;class Ĕ{public long Δ;public int ǩ;public long ǖ;public bool Ηκ;public DateTime ǧ,Ƥ;
}IMyBroadcastListener ΒϏ=null;List<Ĕ>Э=new List<Ĕ>();List<IMyShipConnector>į=new List<IMyShipConnector>();List<IMySensorBlock>Ȍ=new List<IMySensorBlock>();List<IMyCameraBlock>ĩ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Γμ=new List<MyDetectedEntityInfo>();long ј=0;int Βπ=0;
long Βο=0;DateTime Ην=DateTime.Now;String ğ="";int ΗЄ=0;IMyCubeGrid İ=null;void Κή(){İ=Me.CubeGrid;χ.GetBlocksOfType(į,q=>q.CubeGrid.IsSameConstructAs(İ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȍ,q=>q.CubeGrid.IsSameConstructAs(İ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ĩ,q=>q.CubeGrid.IsSameConstructAs(İ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȍ.Count;i++){Ȍ[i].DetectSmallShips=true;Ȍ[i].DetectLargeShips=true;}for(int i=0;i<ĩ.Count;i++) ĩ[i].EnableRaycast=true;}IMyShipConnector Δκ(long Θ){if(Θ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Θ) as IMyShipConnector;return c!=null&&İ!=null&&c.CubeGrid.IsSameConstructAs(İ)?c:null;}bool ΔΔ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖЈ(IMyShipConnector c,long Δ){for(int i=0;i<ĩ.Count;i++)
{IMyCameraBlock Ĝ=ĩ[i];if(!Ĝ.IsWorking||Vector3D.Distance(Ĝ.GetPosition(),c.GetPosition())>10) continue;Vector3D ș=Ĝ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ĝ.CanScan(ș)) continue;MyDetectedEntityInfo Ǖ=Ĝ.Raycast(ș);if(!Ǖ.IsEmpty()&&Ǖ.EntityId!=İ.EntityId&&Ǖ.EntityId!=Δ&&Ǖ.Type!=MyDetectedEntityType.FloatingObject)
{ğ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΚΓ(){for(int s=0;s<Ȍ.Count;s++){if(!Ȍ[s].IsWorking) continue;Γμ.Clear();Ȍ[s].DetectedEntities(Γμ);for(int i=0;i<Γμ.Count;i++){MyDetectedEntityInfo e=Γμ[i];bool ΛΧ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΛΧ&&e.EntityId!=ј&&e.Velocity.Length()>0.5f){ğ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΚΞ(Ĕ r){IMyShipConnector ǖ=Δκ(r.ǖ);if(ΔΔ(ǖ)&&ΖЈ(ǖ,r.Δ)) return ǖ.EntityId;if(!r.Ηκ||ǖ==null) return 0;IMyShipConnector Ћ=null;double ΑΠ=0;for(int i=0;i<į.Count;i++)
{IMyShipConnector c=į[i];if(!ΔΔ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ǖ.GetPosition());if(Ћ==null||d<ΑΠ){if(ΖЈ(c,r.Δ)){Ћ=c;ΑΠ=d;}}}return Ћ==null?0:Ћ.EntityId;}void ΙЃ(){Ĕ Ʈ=null;long Х=0;for(int Č=2;Č>=1&&Ʈ==null;Č--){Ĕ ű=null;for(int i=0;i<Э.Count;i++)
{Ĕ r=Э[i];if(r.ǩ!=Č) continue;if(Č==1){long c=ΚΞ(r);if(c==0) continue;if(ű==null||r.ǧ<ű.ǧ){ű=r;Х=c;}}else if(ű==null||r.ǧ<ű.ǧ) ű=r;}Ʈ=ű;}if(Ʈ==null){if(Э.Count>0&&ğ=="") ğ="No free dock";return;}ј=Ʈ.Δ;Βπ=Ʈ.ǩ;Βο=Ʈ.ǩ==1?Х:0;Ην=DateTime.Now;
}void Κσ(){if(İ==null) return;if(ΒϏ==null) ΒϏ=IGC.RegisterBroadcastListener(ǅ);int ǥ=30;while(ΒϏ.HasPendingMessage&&ǥ-->0){String Η=ΒϏ.AcceptMessage().Data as String;if(Η!=null) ΙЅ(Η);}ΚΦ();if(++ΗЄ<3) return;ΗЄ=0;IGC.SendBroadcastMessage(ǅ,ΙΦ());}void ΙЅ(String Η){try{String[]f=Η.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Э.Count-1;i>=0;i--) if(Э[i].Δ==Δ) Э.RemoveAt(i);if(ј==Δ) ј=0;}else if(f[0]=="R"&&f.Length>=5){long ǖ=long.Parse(f[3]);if(Δκ(ǖ)==null) return;Ĕ r=null;for(int i=0;i<Э.Count;i++) if(Э[i].Δ==Δ) r=Э[i];if(r==null){r=new Ĕ();r.Δ=Δ;r.ǧ=DateTime.Now;Э.Add(r);}
int ǩ=int.Parse(f[2]);if(r.ǩ!=ǩ) r.ǧ=DateTime.Now;r.ǩ=ǩ;r.ǖ=ǖ;r.Ηκ=f[4]=="1";r.Ƥ=DateTime.Now;}}catch{}}void ΚΦ(){for(int i=Э.Count-1;i>=0;i--)
if((DateTime.Now-Э[i].Ƥ).TotalSeconds>3) Э.RemoveAt(i);Ĕ Βξ=null;for(int i=0;i<Э.Count;i++) if(Э[i].Δ==ј) Βξ=Э[i];if(ј!=0&&Βξ==null) ј=0;if(Βξ!=null&&(DateTime.Now-Ην).TotalSeconds>ΙΨ){ј=0;Βξ.ǧ=DateTime.Now;}ğ="";if(ј==0&&Э.Count>0&&!ΚΓ()) ΙЃ();}
String ΙΦ(){String ε="S;"+İ.EntityId+";"+ј+";"+Βπ+";"+Βο+";";for(int i=0;i<Э.Count;i++) ε+=(i>0?",":"")+Э[i].Δ;ε+=";";for(int i=0;i<į.Count;i++){IMyShipConnector c=į[i];ε+=(i>0?":":"")+c.EntityId+","+π(c.GetPosition())+","+π(c.WorldMatrix.Forward)+","+π(c.WorldMatrix.Up);
}ε+=";";for(int i=0;i<Ȍ.Count;i++){IMySensorBlock z=Ȍ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ε+=(i>0?":":"")+π(z.GetPosition())+","+Math.Round(r,1);}return ε;}String Ιψ()
{if(į.Count==0&&Ȍ.Count==0) return "";int Αα=0;for(int i=0;i<į.Count;i++) if(ΔΔ(į[i])) Αα++;return "Docks: "+Αα+"/"+į.Count+" free"+(ј!=0?(Βπ==1?" | docking":" | undocking"):"")+
(Э.Count>1?" | "+(Э.Count-1)+" waiting":"")+(ğ!=""?"\n"+ğ:"")+"\n";}IMyBroadcastListener Γΰ=null;int Ć=0;DateTime ΗЃ=DateTime.Now;DateTime ΘЃ=DateTime.MinValue;long ΘЄ=0;int ΘЅ=0;long ƶ=0;bool ΑЇ=false;int ΕΖ=0;List<Vector3>ΑЉ=new List<Vector3>();List<float>Ζά=new List<float>();
bool ΒЀ=false;bool Βώ=false;Vector3 ΗЂ=new Vector3();bool ΑΖ(){return Ć!=0&&(DateTime.Now-ΘЃ).TotalSeconds<3;}bool Ę(int ǩ){return ΑΖ()&&ΘЄ==ю&&ΘЅ==ǩ;}bool ΗΟ(){return ΑΖ()||(DateTime.Now-ΗЃ).TotalSeconds<3;}void Δί(int ǩ){if(Ć==ǩ) return;Ć=ǩ;ΗЃ=DateTime.Now;
ΕΖ=3;}void Δή(){if(Ć!=0&&trafficEnabled) IGC.SendBroadcastMessage(ǅ,"D;"+ю);Ć=0;ΑЇ=false;}void Κς(){if(!trafficEnabled||Ξ==Ν.ί) return;if(Γΰ==null) Γΰ=IGC.RegisterBroadcastListener(ǅ);int ǥ=20;while(Γΰ.HasPendingMessage&&ǥ-->0){String Η=Γΰ.AcceptMessage().Data as String;
if(Η==null||!Η.StartsWith("S;")) continue;try{ΚΛ(Η);}catch{}}if(Ω!=Α.И){ΒЀ=false;Βώ=false;}if(Ć==1&&о(MyShipConnectorStatus.Connected)!=null) Δή();if(Ć==2&&!ΑЇ&&Ω!=Α.ǹ&&
Vector3.Distance(β,ѕ().Ε)>δ*3) Δή();if(Ć!=0&&++ΕΖ>=3){ΕΖ=0;IGC.SendBroadcastMessage(ǅ,"R;"+ю+";"+Ć+";"+Ǘ+";"+(Ł?1:0));}}void ΚΛ(String Η){String[]f=Η.Split(';');if(f.Length<7) return;bool ΗЎ=false;String[]ΗΪ=f[5].Split(',');for(int i=0;i<ΗΪ.Length;i++) if(ΗΪ[i]==ю.ToString()) ΗЎ=true;
if(!ΗЎ) return;ΘЃ=DateTime.Now;ΘЄ=long.Parse(f[2]);ΘЅ=int.Parse(f[3]);ƶ=long.Parse(f[4]);ΑЉ.Clear();Ζά.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑЉ.Add(κ(e[0]));Ζά.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗІ=f[6].Split(':');List<Ǣ>В=new List<Ǣ>();for(int i=0;i<ΗІ.Length;i++){String[]e=ΗІ[i].Split(',');if(e.Length<4) continue;Ǣ p=new Ǣ();p.Θ=long.Parse(e[0]);p.ά=κ(e[1]);p.г=κ(e[2]);p.Љ=κ(e[3]);В.Add(p);}Ь=В;}bool Ιί(){if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return true;
Δί(1);if(!ΗΟ()) return true;if(!Ę(1)){Μ="Waiting for dock clearance";return false;}Γπ();return true;}bool Γπ(){ΑΨ=false;Ǣ Ŵ=null;if(ƶ!=0&&ƶ!=Ǘ&&Ł)
for(int i=0;i<Ь.Count;i++) if(Ь[i].Θ==ƶ) Ŵ=Ь[i];long ǀ=Ŵ==null?0:Ŵ.Θ;if(ǀ==č&&(ǀ==0)==(ǫ==null)) return false;č=ǀ;ǫ=Ŵ==null?null:Γώ(Ŵ.ά,Ŵ.г,Ŵ.Љ);return true;}bool ƈ(Vector3 p){for(int i=0;i<ΑЉ.Count;i++) if(Vector3.Distance(p,ΑЉ[i])<Ζά[i]+δ*0.75f) return true;
return false;}bool ΙЉ(Vector3 ΑΥ,Vector3 Ű){ΒЀ=false;if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return false;if(Ć!=1&&Vector3.Distance(β,Ρ.Ε)>dockRequestDistance) return false;Δί(1);if(!ΑΖ()||Ę(1)||ΑЉ.Count==0) return false;if(!ƈ(ΑΥ)&&!ƈ(Ű)&&!ƈ(β)) return false;ΒЀ=true;return true;
}bool Ζη(){if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return true;Δί(2);if(!ΗΟ()) return true;if(!Ę(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΚΔ=600f;class Ȱ{public long Θ;public int в=-1;public bool ǀ;public float Αβ,Αή;public DateTime ΕЅ;}class Ȏ{public String э;
public int w,h;public float ΖΟ,ΖΓ;public String φ;public List<int>ΓΠ=new List<int>();public List<Ȱ>ш=new List<Ȱ>();}List<Ȏ>ǘ=new List<Ȏ>();IMyBroadcastListener ΒБ=null;int ΘΦ=0;void Κχ(){if(ΒБ==null) ΒБ=IGC.RegisterBroadcastListener(Δν);int ǥ=50;while(ΒБ.HasPendingMessage&&ǥ-->0)
{String Η=ΒБ.AcceptMessage().Data as String;if(Η==null) continue;try{ΙЇ(Η);}catch{}try{ΙЈ(Η);}catch{}}for(int j=ǘ.Count-1;j>=0;j--){Ȏ Ι=ǘ[j];for(int i=Ι.ш.Count-1;i>=0;i--)
if((DateTime.Now-Ι.ш[i].ΕЅ).TotalSeconds>ΚΔ) Ι.ш.RemoveAt(i);if(Ι.ш.Count==0){ǘ.RemoveAt(j);continue;}ΙΡ(Ι);}if(++ΘΦ<3) return;ΘΦ=0;for(int j=0;j<ǘ.Count;j++) IGC.SendBroadcastMessage(ΖЉ,ΙΥ(ǘ[j]));}void ΙΡ(Ȏ Ι){for(int i=0;i<Ι.ш.Count;i++){Ȱ s=Ι.ш[i];if(s.ǀ&&(s.в<0||Ι.φ[s.в]=='1')) s.в=ΚΗ(Ι,s);
}}String ΙΥ(Ȏ Ι){String ε="J;"+Ι.э+";"+Ι.φ+";";bool ǧ=true;for(int i=0;i<Ι.ш.Count;i++){if((DateTime.Now-Ι.ш[i].ΕЅ).TotalSeconds>5) continue;ε+=(ǧ?"":",")+Ι.ш[i].Θ+":"+Ι.ш[i].в;ǧ=false;}return ε;}void ΙЇ(String Η){String[]f=Η.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;Ȏ Ι=null;for(int i=0;i<ǘ.Count;i++) if(ǘ[i].э==f[4]) Ι=ǘ[i];if(Ι==null){Ι=new Ȏ();Ι.э=f[4];Ι.w=int.Parse(k[3]);Ι.h=int.Parse(k[4]);Ι.ΖΟ=float.Parse(k[6]);Ι.ΖΓ=float.Parse(k[7]);if(Ι.w<1||Ι.h<1) return;Ζΰ(Ι.w,Ι.h,(ї) int.Parse(k[5]),Ι.ΓΠ);
Ι.φ=new String('0',Ι.w*Ι.h);ǘ.Add(Ι);}String d=f[6];if(d.Length==Ι.φ.Length){char[]c=Ι.φ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Ι.φ=new String(c);}long Θ=long.Parse(f[0]);Ȱ s=null;for(int i=0;i<Ι.ш.Count;i++) if(Ι.ш[i].Θ==Θ) s=Ι.ш[i];
if(s==null){s=new Ȱ();s.Θ=Θ;Ι.ш.Add(s);}s.ΕЅ=DateTime.Now;String[]w=f[10].Split(',');s.ǀ=w[0]=="1";if(w.Length>=3){s.Αβ=float.Parse(w[1]);s.Αή=float.Parse(w[2]);}int Ģ=int.Parse(f[5]);if(Ģ>=0&&Ģ<Ι.φ.Length&&(s.в<0||Ι.φ[s.в]=='1')&&Ι.φ[Ģ]!='1'&&ΓА(Ι,Ģ,s)==null)
s.в=Ģ;}Ȱ ΓА(Ȏ Ι,int в,Ȱ ΛΠ){for(int i=0;i<Ι.ш.Count;i++){Ȱ o=Ι.ш[i];if(o!=ΛΠ&&o.в==в)
return o;}return null;}int ΚΗ(Ȏ Ι,Ȱ s){for(int k=0;k<Ι.ΓΠ.Count;k++){int c=Ι.ΓΠ[k];if(Ι.φ[c]!='1'&&ΓА(Ι,c,s)==null) return c;}return-2;}String Ιύ(){String s="";for(int j=0;j<ǘ.Count;j++){Ȏ Ι=ǘ[j];int φ=0;for(int i=0;i<Ι.φ.Length;i++) if(Ι.φ[i]=='1') φ++;s+="Job "+(j+1)+": "+φ+"/"+Ι.φ.Length+" holes, "+Ι.ш.Count+" ships\n";
}return s;}const float Κη=8f;class Щ{public long Θ,ų;public Vector3 ά,ж,ȝ;public float ƛ,Αβ,Αή,ќ=-1;public int ȁ;public int ǝ;public String Ι="";public int ŵ;public DateTime ȥ;}class Βα{public long a,b,ΖΫ;public DateTime Ƥ;}List<Щ>ξ=new List<Щ>();List<Βα>ў=new List<Βα>();int ΖΒ=0;
void ΙЈ(String Η){String[]f=Η.Split(';');if(f.Length<9) return;long Θ=long.Parse(f[0]);Щ s=null;for(int i=0;i<ξ.Count;i++) if(ξ[i].Θ==Θ) s=ξ[i];if(s==null){s=new Щ();s.Θ=Θ;ξ.Add(s);}s.ά=κ(f[1]);s.ж=κ(f[2]);s.ƛ=float.Parse(f[3]);s.Ι=f[4];s.ŵ=int.Parse(f[7].Split(',')[1]);
s.ȝ=κ(f[8]);s.ȁ=0;s.ǝ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.Αβ=float.Parse(w[1]);s.Αή=float.Parse(w[2]);s.ȁ=int.Parse(w[3]);if(w.Length>=5) s.ǝ=int.Parse(w[4]);}}s.ų=f.Length>=12?long.Parse(f[11]):0;s.ȥ=DateTime.Now;}bool ΑΗ(Щ s){return s.ж.LengthSquared()<0.25f&&(s.ŵ==3||s.ȝ.LengthSquared()<0.25f);}
Vector3 ΗΥ(Щ s){return s.ж.LengthSquared()>=1f?s.ж:s.ȝ*2f;}void ΚЀ(){Ιε();if(++ΖΒ<3) return;ΖΒ=0;IGC.SendBroadcastMessage(ΗΤ,ΙΧ());}void Ιε(){for(int i=ξ.Count-1;i>=0;i--)
if((DateTime.Now-ξ[i].ȥ).TotalSeconds>3) ξ.RemoveAt(i);for(int i=ў.Count-1;i>=0;i--)
if((DateTime.Now-ў[i].Ƥ).TotalSeconds>2) ў.RemoveAt(i);for(int i=0;i<ξ.Count;i++) ξ[i].ќ=-1;for(int i=0;i<ξ.Count;i++)
for(int j=i+1;j<ξ.Count;j++){Щ a=ξ[i],b=ξ[j];if(ΑΗ(a)||ΑΗ(b)) continue;Vector3 Λφ=a.ά+a.ж*(float)(DateTime.Now-a.ȥ).TotalSeconds;Vector3 ų=b.ά+b.ж*(float)(DateTime.Now-b.ȥ).TotalSeconds;Vector3 ƻ=ΗΥ(a),Ƽ=ΗΥ(b);Vector3 ё=ų-Λφ;float υ=ё.Length();float з=(a.ƛ+b.ƛ)*0.55f;
bool Αφ=a.Ι!=""&&a.Ι==b.Ι;if(Αφ&&(a.ȁ==2||b.ȁ==2)) continue;if(Αφ&&a.ȁ==1&&b.ȁ==1){String[]k=a.Ι.Split('|');if(k.Length>=8) з=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ΕЃ=Ƽ-ƻ;float Θω=ΕЃ.LengthSquared();float t=Θω>0.01f?-Vector3.Dot(ё,ΕЃ)/Θω:0;
bool ΖΚ=υ<з;if(!ΖΚ&&(t<=0||t>Κη||(ё+ΕЃ*t).Length()>=з)) continue;if(!ΖΚ&&a.ǝ==b.ǝ&&ƻ.LengthSquared()>0.25f&&Ƽ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ƻ),Vector3.Normalize(Ƽ))<-0.7f)
continue;Βα p=null;for(int k=0;k<ў.Count;k++)
if((ў[k].a==a.Θ&&ў[k].b==b.Θ)||(ў[k].a==b.Θ&&ў[k].b==a.Θ)) p=ў[k];if(p==null){p=new Βα();p.a=a.Θ;p.b=b.Θ;p.ΖΫ=Κθ(a,b,ƻ,Ƽ,ё,υ);ў.Add(p);}p.Ƥ=DateTime.Now;Щ y=p.ΖΫ==a.Θ?a:b;float Θά=ΖΚ||t<4?0:Math.Max(1,y.ж.Length()*0.5f);if(y.ќ<0||Θά<y.ќ) y.ќ=Θά;
}ΙЊ();}long Κθ(Щ a,Щ b,Vector3 ƻ,Vector3 Ƽ,Vector3 ё,float υ){if(a.Θ==ј) return b.Θ;if(b.Θ==ј) return a.Θ;if(a.ǝ!=b.ǝ) return a.ǝ<b.ǝ?a.Θ:b.Θ;Vector3 n=υ>0.01f?ё/υ:new Vector3(1,0,0);float ΘЇ=Vector3.Dot(ƻ,n),ΙΑ=Vector3.Dot(Ƽ,-n);if(ΘЇ-ΙΑ>1) return a.Θ;if(ΙΑ-ΘЇ>1) return b.Θ;
return a.Θ>b.Θ?a.Θ:b.Θ;}String ΙΧ(){String ε="C;";bool ǧ=true;for(int i=0;i<ξ.Count;i++){if(ξ[i].ќ<0) continue;ε+=(ǧ?"":",")+ξ[i].Θ+":"+Math.Round(ξ[i].ќ,1);ǧ=false;}return ε;}String ΙЂ(){int Θκ=0,Αζ=0;for(int i=0;i<ξ.Count;i++){if(!ΑΗ(ξ[i])) Θκ++;if(ξ[i].ќ>=0) Αζ++;
}if(ξ.Count==0) return "";return "Traffic: "+Θκ+" moving"+(Αζ>0?", "+Αζ+" giving way":"")+"\n";}List<ϊ>ǿ=new List<ϊ>();ϊ Ĉ=null;Vector3 ΘΜ;DateTime ΘΛ;float ΘΚ=40;bool ΘΙ=false;Щ ΓЅ(ϊ s){Щ Ћ=null;for(int i=0;i<ξ.Count;i++) if(ξ[i].ų!=0&&""+ξ[i].ų==s.Θ) return ξ[i];
for(int i=0;i<ξ.Count;i++)
if(Vector3.Distance(ξ[i].ά,s.Ε)<15&&(Ћ==null||
Vector3.Distance(ξ[i].ά,s.Ε)<Vector3.Distance(Ћ.ά,s.Ε))) Ћ=ξ[i];return Ћ;}float ΙЋ(ϊ s){Щ t=ΓЅ(s);if(t==null) return 0;float k=t.ȁ*1000000f;String[]э=t.Ι.Split('|');if(э.Length>=3) k+=Vector3.Distance(t.ά,new Vector3(float.Parse(э[0]),float.Parse(э[1]),float.Parse(э[2])));return k;
}void Κε(){ǿ.Clear();Ĉ=null;ΕΪ.Clear();List<float>Εί=new List<float>();for(int i=0;i<ώ.Count;i++){float k=ΙЋ(ώ[i]);int ş=0;while(ş<Εί.Count&&Εί[ş]<=k) ş++;Εί.Insert(ş,k);ǿ.Insert(ş,ώ[i]);ώ[i].ĺ("HOMEPOS",true,false,false);ΒΫ("HOMEPOS",ώ[i].Θ,"HOMEPOS");}}List<Щ>ΕΪ=new List<Щ>();
void ΙЊ(){ΕΪ.Clear();for(int i=0;i<ǿ.Count;i++){Щ t=ΓЅ(ǿ[i]);if(t==null) continue;t.ќ=0;ΕΪ.Add(t);}}void Κό(){if(Ĉ!=null){bool ΛΦ=ΘΙ||Vector3.Distance(Ĉ.Ε,ΘΜ)>ΘΚ||
(DateTime.Now-ΘΛ).TotalSeconds>45||Ĉ.ΔΗ();if(!ΛΦ) return;Ĉ=null;}if(ǿ.Count==0) return;Ĉ=ǿ[0];ǿ.RemoveAt(0);ΘΜ=Ĉ.Ε;ΘΛ=DateTime.Now;Щ t=ΓЅ(Ĉ);ΘΚ=t==null?40:Math.Max(40,t.ƛ*3);ΘΙ=t!=null&&t.ŵ==3&&t.ж.LengthSquared()<0.25f;}const String Ιλ="[PAMDBG]";class Γϋ
{public readonly bool ΔΞ;IMyProgrammableBlock ę;Action<IMyProgrammableBlock>Δτ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Δσ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Δς;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>Δυ;
Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>Δρ;public Γϋ(MyGridProgram Λύ){ę=Λύ.Me;var ů=ę.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ę);if(ů==null) return;Δτ=(Action<IMyProgrammableBlock>) ů["RemoveDraw"];Δσ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ů["Point"];
Δς=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ů["Line"];Δυ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ů["Sphere"];Δρ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) ů["GPS"];((Action<IMyProgrammableBlock>) ů["RemoveAll"])(ę);
ΔΞ=true;}public void ΚΡ(){if(Δτ!=null) Δτ(ę);}public void Ĺ(Vector3D a,Vector3D b,Color c,float ΜΕ){if(Δς!=null) Δς(ę,a,b,c,ΜΕ,-1,false);}public void ΚΟ(Vector3D a,Color c,float Ď){if(Δσ!=null) Δσ(ę,a,c,Ď,-1,false);}public void Ƌ(String Γ,Vector3D ş,Color c){if(Δρ!=null) Δρ(ę,Γ,ş,c,-1);}
public void ΗΞ(Vector3D ƙ,double Ď,Color c){if(Δυ!=null) Δυ(ę,new BoundingSphereD(ƙ,Ď),c,1,0.05f,30,-1,false);}}class ǆ{public long Θ;public Vector3 Αι,ū,Ș,ζ;public bool ΘΔ;public List<Vector3>ΓΤ=new List<Vector3>(),Εώ=new List<Vector3>();public List<bool>Εύ=new List<bool>();
public DateTime ȥ;}Γϋ Р=null;IMyBroadcastListener Βϊ=null;List<ǆ>ą=new List<ǆ>();void ΙЄ(String Η){String[]f=Η.Split(';');if(f.Length<6) return;long Θ=long.Parse(f[0]);ǆ s=null;for(int i=0;i<ą.Count;i++) if(ą[i].Θ==Θ) s=ą[i];if(s==null){s=new ǆ();s.Θ=Θ;ą.Add(s);}
s.Αι=κ(f[1]);s.ū=κ(f[2]);s.Ș=κ(f[3]);s.ΘΔ=s.ū.LengthSquared()>0.5f;s.ζ=κ(f[4]);s.ȥ=DateTime.Now;if(f[5]=="") return;s.ΓΤ.Clear();s.Εώ.Clear();s.Εύ.Clear();String[]Θψ=f[5].Split(':');for(int i=0;i<Θψ.Length;i++){String[]r=Θψ[i].Split(',');if(r.Length<3) continue;s.ΓΤ.Add(κ(r[0]));s.Εώ.Add(κ(r[1]));s.Εύ.Add(r[2]=="1");
}}void Κρ(){if(!debugDraw) return;if(Р==null) Р=new Γϋ(this);if(!Р.ΔΞ) return;if(Βϊ==null) Βϊ=IGC.RegisterBroadcastListener(Ιλ);int ǥ=20;while(Βϊ.HasPendingMessage&&ǥ-->0){String Η=Βϊ.AcceptMessage().Data as String;if(Η!=null) try{ΙЄ(Η);}catch{}}for(int i=ą.Count-1;i>=0;i--) if((DateTime.Now-ą[i].ȥ).TotalSeconds>5) ą.RemoveAt(i);
Р.ΚΡ();for(int i=0;i<ξ.Count;i++){Щ s=ξ[i];Color c=ΑΗ(s)?Color.Gray:s.ќ>=0?Color.Yellow:Color.Lime;Р.ΗΞ(s.ά,s.ƛ/2,c);Р.Ƌ(Βΰ(s)+": "+(ΑΗ(s)?"parked":s.ќ==0?"holding (giving way)":s.ќ>0?"slowing (giving way)":"moving"),s.ά,c);if(s.ж.LengthSquared()>0.25f) Р.Ĺ(s.ά,s.ά+s.ж*3,Color.White,0.1f);
if(s.ȝ.LengthSquared()>0.25f) Р.Ĺ(s.ά,s.ά+s.ȝ*s.ƛ,Color.Cyan,0.1f);}for(int i=0;i<ў.Count;i++){if((DateTime.Now-ў[i].Ƥ).TotalSeconds>0.5) continue;Щ a=null,b=null;for(int k=0;k<ξ.Count;k++){if(ξ[k].Θ==ў[i].a) a=ξ[k];if(ξ[k].Θ==ў[i].b) b=ξ[k];}if(a==null||b==null) continue;
Р.Ĺ(a.ά,b.ά,Color.Red,0.2f);Р.Ƌ("Conflict: "+Βΰ(ў[i].ΖΫ==a.Θ?a:b)+" gives way",(a.ά+b.ά)/2,Color.Red);}for(int i=0;i<ą.Count;i++){ǆ d=ą[i];Щ t=null;for(int k=0;k<ξ.Count;k++) if(ξ[k].Θ==d.Θ) t=ξ[k];if(t!=null) Р.Ĺ(t.ά,d.ζ,Color.Blue,0.08f);for(int r=0;r<d.ΓΤ.Count;r++) Р.Ĺ(d.ΓΤ[r],d.Εώ[r],d.Εύ[r]?Color.Red:Color.Green,0.04f);
if(t==null||!d.ΘΔ) continue;Ȏ Ι=null;for(int j=0;j<ǘ.Count;j++) if(ǘ[j].э==t.Ι) Ι=ǘ[j];if(Ι==null) continue;int ΗЈ=0;for(int c=0;c<Ι.φ.Length;c++) if(Ι.φ[c]=='1') ΗЈ++;if(i==0||ą[i-1].Αι!=d.Αι) Р.Ƌ("Job: "+ΗЈ+"/"+Ι.φ.Length+" holes done",d.Αι,Color.White);
for(int c=0;c<Ι.φ.Length;c++){int ΔЍ=c%Ι.w,ŋ=c/Ι.w;Vector3 p=d.Αι+d.ū*((Ι.w-1f)/2f-ΔЍ)*Ι.ΖΟ-d.Ș*((Ι.h-1f)/2f-ŋ)*Ι.ΖΓ;Ȱ Εψ=ΓА(Ι,c,null);Р.ΚΟ(p,Ι.φ[c]=='1'?Color.Gray:Εψ!=null?Color.Orange:Color.White,Math.Min(Ι.ΖΟ,Ι.ΖΓ)*0.2f);if(Εψ==null||Εψ.Θ!=d.Θ) continue;
Р.Ĺ(t.ά,p,Color.Orange,0.05f);Р.Ƌ("Hole "+(Ι.ΓΠ.IndexOf(c)+1)+": "+Βΰ(t),p,Color.Orange);}}for(int i=0;i<Ȍ.Count;i++){IMySensorBlock z=Ȍ[i];Р.ΗΞ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);
Р.Ƌ("Docking area"+(ğ!=""?": "+ğ:""),z.GetPosition(),Color.Cyan);}for(int i=0;i<ĩ.Count;i++)
Р.Ĺ(ĩ[i].GetPosition(),ĩ[i].GetPosition()+ĩ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(ј!=0&&Βο!=0){IMyShipConnector c=Δκ(Βο);for(int k=0;k<ξ.Count;k++)
if(c!=null&&ξ[k].Θ==ј){Р.Ĺ(ξ[k].ά,c.GetPosition(),Color.Lime,0.15f);Р.Ƌ("Cleared to "+(Βπ==2?"undock: ":"dock: ")+Βΰ(ξ[k]),c.GetPosition(),Color.Lime);}}}String Βΰ(Щ t){ϊ Ћ=null;for(int i=0;i<ώ.Count;i++) if(t.ų!=0&&ώ[i].Θ==""+t.ų) return ώ[i].ΰ;for(int i=0;i<ώ.Count;i++)
if(Vector3.Distance(ώ[i].Ε,t.ά)<30&&(Ћ==null||
Vector3.Distance(ώ[i].Ε,t.ά)<Vector3.Distance(Ћ.Ε,t.ά))) Ћ=ώ[i];if(Ћ!=null&&Ћ.ΰ!="") return Ћ.ΰ;String Θ=t.Θ.ToString();return "Ship "+Θ.Substring(Math.Max(0,Θ.Length-4));}String Ιφ(){if(!debugDraw) return "";if(Р==null||!Р.ΔΞ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}