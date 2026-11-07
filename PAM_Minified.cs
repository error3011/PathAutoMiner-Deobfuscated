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
public enum Ν{Ľ,ΑΖ,Ǹ,Ǒ,ί}public enum Α{ѕ,Ƿ,ř,Ȑ,Ǥ,И,т,
Ȯ,ƅ,ǵ,ŕ,ő,ķ,
ΑΘ,Ā,ǻ,Ŕ,Βι,ΒΖ,
Δυ,Βθ,Ŗ,ΒΘ,ľ}public enum Τ{ы,ƍ,Л,ΓЅ,ΒΒ,ΒΤ,ΒΥ,ƀ,Ɓ}public enum ǐ{None,Ŕ,ѕ,Ζς}public enum Φ{Main,ΗΜ,ΑΕ,ΑΔ,Δπ,ΑБ,
Γτ,ΖЇ,ΖЈ,ΖЅ,ΑΚ,ΗΡ,
Βΰ,Δζ,Δκ,ΗΤ,Ββ}public enum ь{Βή,ǉ,Κλ,ΖЋ,ί}public enum і{ΑΝ,Ζκ}public enum ѓ{Βέ,ΓЌ,ΗΧ,ΑΑ}public enum З{ΔΧ,Ζο,ΓϏ,ΓІ,ΒΔ,ΔΖ,ΒΞ}public enum ǡ{ΓЂ,Γω,Γϊ}public enum б{ƈ,ΚΕ,Œ}public enum τ{ΔΩ,Δέ,Δή,ΗΗ,ΗΖ,
ΔΨ,ΒΪ,ΒΩ,ΔΫ,
Δά,ΔΪ}public enum ι{Ȭ,ė,ȫ,ΑА,Γχ,Γψ}public enum Ǻ{ΒΛ,ΑЏ,Ιω}public enum ǒ{Βγ,Ζτ,ΚΠ,ΔΦ}public enum Ў{None,Ǝ,ΑΗ,ΒΨ}public enum Ѐ{ΑΟ,ΑЎ,ΒΡ,ΑΑ,ΔΣ,ΒΣ,ΔΤ,Κσ,Δί}public enum Ш{Ľ,ΗΓ,ΗΒ,ΔΟ,ΔΘ,ΓЀ,ΗΔ}class έ{public bool ν=false;public Vector3 Ε=new Vector3();
public Vector3 λ=new Vector3();public Vector3 μ=new Vector3();public Vector3 Є=new Vector3();public Vector3 ό=new Vector3();public Vector3 х=new Vector3();public float У=0;public float Μπ=0;public float[]Ȍ=null;public έ(){}public έ(έ θ){ν=θ.ν;Ε=θ.Ε;λ=θ.λ;μ=θ.μ;Є=θ.Є;ό=θ.ό;
х=θ.х;Ȍ=θ.Ȍ;}public έ(Vector3 Ε,Vector3 μ,Vector3 λ,Vector3 Є,Vector3 ό){this.Ε=Ε;this.λ=λ;this.μ=μ;this.Є=Є;this.У=0;this.ό=ό;}public void Ιβ(List<IMyThrust>Βν,List<string>đ){Ȍ=new float[đ.Count];for(int i=0;i<Ȍ.Length;i++)
Ȍ[i]=-1;for(int i=0;i<Βν.Count;i++){string Ǡ=ΖЂ(Βν[i]);int Β=đ.IndexOf(Ǡ);if(Β!=-1)
Ȍ[Β]=М(Βν[i].MaxEffectiveThrust,Βν[i].MaxThrust);}}}class Δΰ{public έ ζ=null;public List<Vector3>Ƽ=new List<Vector3>();public float ΑΫ=0;public float ΖΧ=0;public float ƕ=0;public float ΕΠ=0;public Vector3 ΒЋ=new Vector3();}class Ƒ{public τ Ч=τ.ΔΩ;public float Ǿ=0;public float ń=0;
public string ǿ="";public string Ȅ="";DateTime Θΰ;public bool ΕЀ=false;private bool ΕΔ=false;public bool Γύ(bool ȉ){if(ȉ){ń=0;ΕΔ=false;return false;}ΕΔ=true;return ń>Ǿ;}public void ΚΫ(){Γύ(true);ΕЀ=false;}public void ΚЅ(){if(ΕΔ)
if((DateTime.Now-Θΰ).TotalSeconds>1){ń++;Θΰ=DateTime.Now;}}public bool ΔΒ(){switch(Ч){case τ.Δέ:return true;case τ.Δή:return true;}return false;}}class у{public String ΰ="";public String ϊ="";public int Џ=0;public б Ʃ=б.Œ;public у(String ΰ,String ϊ,int Џ,б Ʃ){this.ΰ=ΰ;this.ϊ=ϊ;
this.Џ=Џ;this.Ʃ=Ʃ;}}class Ιτ{public Ιτ(Vector3 ά,float υ){this.ά=ά;this.υ=υ;}public Vector3 ά;public float υ;}class ϋ{public DateTime Εζ;public DateTime Ηψ;public String Θ="";public String ΰ="";public String Γρ="";public String Λ="";public String ΖΒ="";public String ΓΣ="";public String ΕϏ="";
public Vector3 Ε=new Vector3();public List<у>А=new List<у>();public Ν Ξ=Ν.Ľ;public Ў ŀ;public float Ƞ;public float Ș;public int Г;public int ΕΙ=0;public int ΖΠ=0;public bool ΔΝ(){return(DateTime.Now-Εζ).TotalSeconds>10;}public bool ΔΙ(){if(ŀ!=Ў.Ǝ) return false;return(DateTime.Now-Ηψ).TotalSeconds>=4;
}public ϋ(String Θ){this.Θ=Θ;}public Ў ļ(String Ј,bool ΜΑ,bool и,bool ΛΔ){if(Ј==""&&!и) return Ў.None;if(ŀ==Ў.Ǝ&&ΔΙ())
ŀ=Ў.ΒΨ;if(ΜΑ){ΕϏ=Ј;ŀ=Ў.Ǝ;Ηψ=DateTime.Now;return ŀ;}else if(и){ΕϏ="";ŀ=Ў.None;}else if(ΕϏ==Ј){if(ΛΔ) ŀ=Ў.ΑΗ;return ŀ;}return Ў.None;}}public String ΙЂ(String Λι){switch(Λι){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Γέ=new Vector3();Vector3 β=new Vector3();Vector3 ΒЏ=new Vector3();Vector3 Αο=new Vector3();Vector3 Αή=new Vector3();
DateTime Ει=new DateTime();bool Ģ=true;int ΖΚ=0;bool ħ=false;bool Ĩ=false;bool Αυ=true;bool ΙΧ=false;bool Ʀ=false;bool ΓΓ=false;float Đ=0;float ź=0;int Ŏ=0;int ΑЇ=0;int Αϋ=0;int ΔЌ=0;float Ƭ=0;float Ɩ=0;double Εσ=0;float ΑΤ=0;List<int>ƣ=new List<int>();List<int>Ƴ=new List<int>();
Ν Ξ=Ν.Ľ;int Е=0;float Ȫ=0;float ȩ=0;int Ū=0;float Ī=0,ĩ=0;float δ=0;float ΕЌ=0,ΕЋ=0;IMyRemoteControl Ϊ;IMySensorBlock ύ;List<IMyTimerBlock>ΖΜ=new List<IMyTimerBlock>();List<IMyShipConnector>Ǔ=new List<IMyShipConnector>();List<IMyThrust>Ē=new List<IMyThrust>();List<IMyGyro>ΒА=new List<IMyGyro>();
List<IMyTerminalBlock>ώ=new List<IMyTerminalBlock>();List<IMyLandingGear>ċ=new List<IMyLandingGear>();List<IMyReactor>Ō=new List<IMyReactor>();List<IMyConveyorSorter>Γγ=new List<IMyConveyorSorter>();List<IMyGasTank>ǩ=new List<IMyGasTank>();List<IMyTerminalBlock>ΑΠ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ƙ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Š=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ŀ=new List<IMyBatteryBlock>();List<IMyTextPanel>а=new List<IMyTextPanel>();List<IMyTextSurface>ȥ=new List<IMyTextSurface>();List<IMyTextPanel>Ť=new List<IMyTextPanel>();
IMyTerminalBlock ǭ=null;έ Ρ=new έ();έ Εή=new έ();έ Εβ=new έ();bool ē=false;bool ǁ=false;bool ǂ=false;bool ΓΕ=false;List<String>đ=new List<string>();List<έ>Ψ=new List<έ>();List<έ>ΑЌ=new List<έ>();int ΕΚ=0;double ΙΨ=0;έ Ζ=new έ();Vector3 Ф;Vector3 ȡ;Τ η=Τ.ы;і ū=і.ΑΝ;int Б=0;
int Я=0;double Αξ=0;bool ΒϏ=false;Ƒ Ќ=new Ƒ();Ƒ Ѝ=new Ƒ();ѓ ѝ=ѓ.Βέ;bool ŏ=true;і Ȥ=і.ΑΝ;int љ=3;int ј=3;int ф=30;ǡ ǔ=ǡ.ΓЂ;bool Ķ=true;bool ƥ=true;bool Ɵ=true;bool Ż=false;З л=З.ΔΧ;bool ĵ=true;float ȟ=90;float ǚ=20;float ц=5;float Ǜ=20;float ȓ=0.70f;float ǆ=1.50f;
float ǅ=2.50f;float ſ=0f;float ŧ=0f;Α Ω;Τ Ǚ;Α ΑЀ;Δΰ ς=null;int І=0;int Ђ=0;int й=0;int ĭ=0;int Ÿ=30;int р=0;bool Ж=true;Vector3 О;double Ь=0;bool Γί=false;int ΖΪ=0;int Њ=0;int ΔЁ=0;int Ȣ=0;int Θω=0;Vector3 Αχ=new Vector3();float њ=0;float щ=0;int ž=0;έ Γδ=null;
float Εη=0;float Βπ=0;float п=0;float ů=0;bool Εΰ=false;bool ż=false;bool ΑЈ=false;bool ĥ=false;bool ĝ=false;DateTime ΛΦ=new DateTime();έ Ţ=null;έ ΗЀ=null;float Ă=0;ǐ Ȕ;float Ħ=0;float Ž=0;String Εξ="";float ΓΙ=0;float Ș=0;float Ƞ=0;List<у>Ȃ=new List<у>();float м=0;
bool Αύ=false;bool ΙΛ=false;bool ΘЈ=false;bool Γι=false;bool ΘЉ=false;float Ερ=0;float Εώ=0;Vector3 ΘΓ=new Vector3();Vector3 ψ=new Vector3();Vector3 Γκ=new Vector3(1,1,1);float ΖΙ=1;Vector3 ΑЄ=new Vector3();bool ΓΔ=false;bool ΕΧ=false;bool Ελ=false;bool Ηγ=false;float Βύ=0;
float Βλ=2;Vector3 ΙΘ;Vector3 ΖΖ;Vector3 ΖΕ;float[,]ȍ=new float[3,2];Dictionary<String,float[,]>Ǳ=new Dictionary<string,float[,]>();Dictionary<String,float>ΙΥ=new Dictionary<String,float>();int ΔЅ=0;Dictionary<String,float[]>į=new Dictionary<string,float[]>();float ΕЁ;bool ΒЁ=false;
int Βξ=0;int ň=0;int ΓΑ=0;int Ũ=0;String Λ="";int[]Θν=new int[Enum.GetValues(typeof(Φ)).Length];bool Ευ=false;int Ű=0;int Г=0;Φ Ё=Φ.Main;int ΜΡ=0,ΜΣ=0,ΜΤ=0,ΜΥ=0;int ΛЍ=0,ΛЎ=0;bool Ƙ=false;bool Ηδ=false;String Ƹ="";String ΔЋ="";IMyBroadcastListener Ǽ=null;
bool ΘΒ=true;List<ϋ>Ϗ=null;ϋ П=null;int Βχ=0,Βψ=0;int Ηϋ=0,Ηό=0;int ΛΙ=0,ΛΚ=0,ΛΛ=0;String Λν="";Exception Ĭ=null;bool Γή=false;Τ Ƶ=Τ.ы;bool ΗЃ=false;bool Θή=true;bool ΕЇ=false;String ΙΦ="";bool ΑІ=false;bool ΖΑ=true;int ΓΒ=0;int[]ǯ=null;Vector3 Βσ=new Vector3();
bool ΘΘ=false;bool Ǣ(IMyTerminalBlock Ϋ)=>Ϋ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΔΓ(Me,controllerTag,true))
Ξ=Ν.Ǒ;ο(Φ.Main);if(Ξ!=Ν.Ǒ){Λ="Welcome to [PAM]!";ǒ ΕІ=Κά();if(Ξ==Ν.Ľ){List<IMyShipDrill>ΗА=new List<IMyShipDrill>();List<IMyShipGrinder>ΘΙ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗА,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΘΙ,q=>q.CubeGrid==Me.CubeGrid);if(ΗА.Count>0)
{Ξ=Ν.ΑΖ;Λ="Miner mode enabled!";}else if(ΘΙ.Count>0){Ξ=Ν.Ǹ;Λ="Grinder mode enabled!";}else{Ξ=Ν.ί;Ȳ(ь.ί);}}if(ΕІ==ǒ.Βγ) Αυ=false;if(ΕІ==ǒ.Ζτ) Λ="Data restore failed!";if(ΕІ==ǒ.ΔΦ) Λ="New version, wipe data";}}void Main(string Ъ,UpdateType Μέ){try{if(Ĭ!=null){if(Ъ.Trim().ToUpper()=="RETRY")
{Ĭ=null;Ģ=true;Ŏ=0;if(Ξ!=Ν.Ǒ) Ї();Λ="Restarted after error";}else{ΙЎ();ΗΚ();return;}}Ĩ=(Μέ&UpdateType.Update10)!=0;if(Ĩ) ΖΚ++;ħ=ΖΚ>=10;if(ħ) ΖΚ=0;if(Ĩ){Βξ++;if(Βξ>4) Βξ=0;Αϋ=(int) Math.Max(0,10-(DateTime.Now-Ει).TotalSeconds);}if(Ъ!="") Λν=Ъ;if(Ξ!=Ν.Ǒ)
Κκ(Ъ);else
Ιρ(Ъ);Ευ=false;try{int ΗЁ=Runtime.CurrentInstructionCount;float ŷ=М(ΗЁ,Runtime.MaxInstructionCount);if(ŷ>0.90) Λ="Max. instructions >90%";if(ŷ>Ƭ) Ƭ=ŷ;if(ΒЁ){ƣ.Add(ΗЁ);while(ƣ.Count>10) ƣ.RemoveAt(0);Ɩ=0;for(int i=0;i<ƣ.Count;i++) Ɩ+=ƣ[i];Ɩ=М(М(Ɩ,ƣ.Count),Runtime.MaxInstructionCount);
double Εθ=Runtime.LastRunTimeMs;if(ΓΓ&&Εθ>Εσ) Εσ=Εθ;Ƴ.Add((int)(Εθ*1000f));while(Ƴ.Count>10) Ƴ.RemoveAt(0);ΑΤ=0;for(int i=0;i<Ƴ.Count;i++) ΑΤ+=Ƴ[i];ΑΤ=М(ΑΤ,Ƴ.Count)/1000f;}}catch{Ƭ=0;}}catch(Exception e){Ĭ=e;}}void Κκ(string Ъ){bool Γΰ=false;String Ηπ="";if(Е<=1&&!ΔΜ(Ъ))
ΗΛ(Ъ);int Εφ=50;while(Ǽ!=null&&Ǽ.HasPendingMessage&&Εφ-->0){MyIGCMessage ε=Ǽ.AcceptMessage();String Η=ε.Data as String;if(Η==null) continue;String ȣ="";if(Е<=1&&ΗΙ(ref Η,out ȣ,out Ηπ)&&ȣ==BROADCAST_SELF_ID){ΗΛ(Η);Γΰ=true;break;}}bool ΘΨ=ΓΓ&&Ω==Α.ѕ&&!Ʀ&&!Γΰ&&Ŏ==0&&!ΓΕ;
if(ħ&&Ω!=Α.ѕ) Γΰ=true;if((Ĩ&&!ΘΨ)||(ħ&&ΘΨ)){if(Ŏ==0&&(Αϋ<=0||Ģ)){ΙΧ=Е>0;Е=0;Ŏ=1;ȳ();Κα();ΖЉ();ȭ("Scan 1");}else if(Ŏ==1){Ŏ=2;ȳ();Κβ();ȭ("Scan 2");}else if(Ŏ==2){Ŏ=0;ȳ();Κγ();Κδ();if(Е<=1) ΗΑ();ȭ("Scan 3");Ει=DateTime.Now;if(Е<=1&&Αυ)
Γέ=ΒΜ(Ϊ,Ϊ.CenterOfMass);Αυ=false;if(Ģ){Ģ=false;bool Μβ=η==Τ.Л;Ї();if(Θή&&resumeAfterRestart&&Μβ&&Е<=1) ΕЇ=true;}Θή=false;if(ΙΧ&&Е==0) Λ="Setup complete";}else{if(η==Τ.Л&&Ξ!=Ν.ί){ȳ();ΙΫ();ȭ("Inv balance");}ȳ();switch(ΑЇ){case 0:Κχ();break;case 1:Κώ();break;case 2:Κφ();break;
case 3:ΚЉ();break;case 4:Κύ();break;case 5:Ιζ();break;case 6:Ιδ(Ϊ);break;}ȭ("Update: "+ΑЇ);ΑЇ++;if(ΑЇ>6){ΑЇ=0;ΓΓ=true;if(Ƶ!=Τ.ы){switch(Ƶ){case Τ.ƀ:Ĺ();break;case Τ.Ɓ:Ĺ();break;case Τ.Л:Ĺ();break;case Τ.ΒΤ:ΒΙ();break;case Τ.ΒΥ:ŗ();break;}Ƶ=Τ.ы;}}}if(!Ģ){if(!Ȱ(Ϊ,true))
{Ϊ=null;Αυ=true;Е=2;}if(Е>=2&&Ω!=Α.ѕ)
Ї();if(Е<=1){ź=Ϊ.CalculateShipMass().PhysicalMass;Đ=(float) Ϊ.GetShipSpeed();β=Κυ(Ϊ,Γέ);ΒЏ=Ϊ.WorldMatrix.Forward;Αο=Ϊ.WorldMatrix.Left;Αή=Ϊ.WorldMatrix.Down;ΚЂ();ΚІ();Κϊ();if(ΕЇ){ΕЇ=false;Ĺ();if(Ω!=Α.ѕ) Λ="Job resumed after restart";}if(Ω!=Α.ѕ){Ʀ=false;ΗΞ(false);Ζθ(false);
String ΜΗ=ΓЏ(Ω)+" "+(int) Ω;ȳ();ΚЀ();Δτ(false);ȭ(ΜΗ);ΚЇ();ΚЁ();ȳ();Κό();ȭ("Thruster");ȳ();Ηί();ȭ("Gyroscope");}else{if(Ʀ){if(ΒΠ()){с(Αή,ΒЏ,Αο,0.25f,true);Ηί();Λ="Aligning to planet: "+Math.Round(Βύ-0.25f,2)+"°";if(ΓΔ) ΔΑ(true,true);}else ΔΑ(true,true);}}ΑІ=false;}}}ȳ();ΗΚ();ȭ("Print");
if(Γΰ||ΔЌ<=0){ȳ();Ιά(Ηπ);ȭ("Broadcast");ΔЌ=4;}else if(ħ) ΔЌ--;}void ΗΛ(string Ъ){if(Ъ=="") return;var ω=Ъ.ToUpper().Split(' ');ω.DefaultIfEmpty("");var Ј=ω.ElementAtOrDefault(0);var ā=ω.ElementAtOrDefault(1);var ΑΣ=ω.ElementAtOrDefault(2);var Ηζ=ω.ElementAtOrDefault(3);String Ċ="Invalid argument: "+Ъ;
bool ΖΦ=false;switch(Ј){case "UP":this.ΔΡ(false);break;case "DOWN":this.ΔΠ(false);break;case "UPLOOP":this.ΔΡ(true);break;case "DOWNLOOP":this.ΔΠ(true);break;case "APPLY":this.Ζζ(true);break;case "MRES":Г=0;break;case "STOP":this.Ї();break;case "PATHHOME":{this.Ї();this.Δξ();}break;
case "PATH":{this.Ї();this.Δξ();Ρ.ν=true;}break;case "START":{this.Ї();Δν();}break;case "ALIGN":{ΔΑ(!Ʀ,false);}break;case "CONT":{this.Ї();this.Ĺ();}break;case "JOBPOS":{this.Ї();this.ŗ();}break;case "HOMEPOS":{this.Ї();this.ΒΙ();}break;case "FULL":{Αύ=true;}break;
case "RESET":{Γή=true;Е=2;}break;case "EXPORT":{Ιϊ();}break;case "IMPORT":{ΚΓ(ā=="JOB");}break;default:ΖΦ=true;break;}if(Ξ!=Ν.ί){switch(Ј){case "SHUTTLE":{Ιχ();}break;case "CFGS":{if(!Ιο(ā,ΑΣ,Ηζ)) Λ=Ċ;}break;case "CFGB":{if(!Ιξ(ā,ΑΣ)) Λ=Ċ;}break;case "CFGL":
{if(!Ś(ref ȟ,true,Ѐ.ΒΡ,ā,"")||!Ιπ(ΑΣ))
Λ=Ċ;}break;case "CFGE":{if(!Ś(ref ц,true,Ѐ.ΒΣ,ā,"IG")||
!Ś(ref ǚ,true,Ѐ.ΔΣ,ΑΣ,"IG")||
!Ś(ref Ǜ,true,Ѐ.ΔΤ,Ηζ,"IG"))
Λ=Ċ;}break;case "CFGA":{if(!Ś(ref ȓ,false,Ѐ.ΑЎ,ā,"")) Λ=Ċ;}break;case "CFGW":{if(!Ś(ref ǆ,false,Ѐ.ΑΟ,ā,"")||
!Ś(ref ǅ,false,Ѐ.ΑΟ,ΑΣ,""))
Λ=Ċ;}break;case "NEXT":{Γσ(false);}break;case "PREV":{Γσ(true);}break;default:if(ΖΦ) Λ=Ċ;break;}}else{switch(Ј){case "UNDOCK":{ΑІ=true;}break;default:if(ΖΦ) Λ=Ċ;break;}}}String ΙЃ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ιχ(){Ї();Ξ=Ν.ί;Ȳ(ь.ί);Ζ.ν=false;Ρ.ν=false;ύ=null;ώ.Clear();η=Τ.ы;}void ΔΑ(bool С,bool φ){if(!С) Λ="Aligning canceled";if(φ) Λ="Aligning done";if(φ||!С){Ʀ=false;ΗΨ();Βα(false,0,0,0,0);return;}if(ΒΠ()) Ʀ=true;}bool Ιο(String Μδ,String Λβ,String ΛΟ)
{bool Μΰ=η==Τ.Л;int w,h,d;if(int.TryParse(Μδ,out w)&&int.TryParse(Λβ,out h)&&int.TryParse(ΛΟ,out d)){this.Ї();љ=w;ј=h;ф=d;Ş(false);ΑΞ(false,false);if(Μΰ) Ĺ();return true;}return false;}bool Ιπ(String ΑΡ){if(ΑΡ=="ON"){Ķ=true;return true;}if(ΑΡ=="OFF"){Ķ=false;return true;}
return false;}bool Ιξ(String ΗЎ,String Βώ){bool Č=true;if(ΗЎ=="HOME") ŏ=true;else if(ΗЎ=="STOP") ŏ=false;else Č=false;if(Βώ=="HOME") ѝ=ѓ.Βέ;else if(Βώ=="STOP") ѝ=ѓ.ΗΧ;else if(Βώ=="JOB") ѝ=ѓ.ΓЌ;else if(Βώ=="IG") ѝ=ѓ.ΑΑ;else Č=false;return Č;}void ο(Φ ю){Θν[(int) Ё]=Г;
Г=Θν[(int) ю];if(ю==Φ.ΑΚ) Г=0;Ё=ю;if(Ξ!=Ν.Ǒ)
ŝ(Ё==Φ.ΑΕ,false,0,0);Ευ=true;}void ΔΡ(bool Εμ){if(Г>0) Г--;else if(Εμ) Г=Ű-1;}void ΔΠ(bool Εμ){if(Г<Ű-1) Г++;else if(Εμ) Г=0;}String Ȳ(ь ε){switch(ε){case ь.Βή:Λ="Job is running";break;case ь.ǉ:Λ="Connector not ready!";break;case ь.Κλ:Λ="Ship modified, path outdated!";break;
case ь.ΖЋ:Λ="Interrupted by player!";break;case ь.ί:Λ="Shuttle mode enabled!";break;}return "";}String ΙЉ(і ά){switch(ά){case і.ΑΝ:return "Top-Left";case і.Ζκ:return "Center";default:return "";}}String ΙϏ(ǡ Д){switch(Д){case ǡ.Γω:return "Auto"+(Ξ==Ν.ΑΖ?" (Ore)":"");case ǡ.Γϊ:return "Auto (+Stone)";
case ǡ.ΓЂ:return "Default";default:return "";}}String ΓЍ(Τ Љ){switch(Љ){case Τ.ы:return "No job";case Τ.ƍ:return "Job paused";case Τ.Л:return "Job active";case Τ.ƀ:return "Job active";case Τ.Ɓ:return "Job active";case Τ.ΓЅ:return "Job done";case Τ.ΒΒ:return "Job changed";case Τ.ΒΤ:return "Move home";
case Τ.ΒΥ:return "Move to job";}return "";}String Ζω(ѓ ΛΓ){switch(ΛΓ){case ѓ.Βέ:return "Return home";case ѓ.ΓЌ:return "Fly to job pos";case ѓ.ΗΧ:return "Stop";case ѓ.ΑΑ:return "Ignore";}return "";}String ΙЁ(З Д){switch(Д){case З.ΔΧ:return "Off";case З.ΓІ:return "Drop pos (Stone) ";case З.ΒΔ:return "Drop pos (Sto.+Ice)";
case З.Ζο:return "Cur. pos (Stone)";case З.ΓϏ:return "Cur. pos (Sto.+Ice)";case З.ΔΖ:return "In motion (Stone)";case З.ΒΞ:return "In motion (Sto.+Ice)";}return "";}String Ζχ(ǐ Љ){switch(Љ){case ǐ.None:return "No batteries";case ǐ.Ŕ:return "Charging";case ǐ.Ζς:return "Discharging";}return "";}
String ΓЏ(Α Љ){String Μξ=Ξ==Ν.ί?"target":"job";switch(Љ){case Α.ѕ:return "Idle";case Α.Ƿ:return "Flying to XY position";case Α.ř:return Ξ==Ν.Ǹ?"Grinding":"Mining";case Α.Ȑ:return "Returning";case Α.Ŗ:return "Flying to drop pos";case Α.Ǥ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.т:return "Flying to job area";case Α.Ȯ:return "Flying to path";case Α.ƅ:return "Flying to job position";case Α.ǵ:return "Approaching dock";case Α.ŕ:return "Docking";case Α.ő:return "Aligning";case Α.ķ:return "Aligning";case Α.ΑΘ:return "Retry docking";case Α.Ā:return "Unloading";case Α.ľ:return ΙΦ;
case Α.ǻ:return "Undocking";case Α.Ŕ:return "Charging batteries";case Α.Βι:return "Waiting for uranium";case Α.ΒΖ:return "Filling up hydrogen";case Α.Δυ:return "Waiting for ejection";case Α.Βθ:return "Waiting for ejection";case Α.ΒΘ:return "Flying to drop pos";}return "";}String ΖЃ(τ Ч){switch(Ч)
{case τ.ΔΩ:return "On \"Undock\" command";case τ.Δέ:return "On player entered cockpit";case τ.ΗΗ:return "On ship is full";case τ.ΗΖ:return "On ship is empty";case τ.Δή:return "On time delay";case τ.ΒΪ:return "On batteries empty(<25%)";case τ.ΒΩ:return "On batteries empty(=0%)";case τ.ΔΨ:return "On batteries full";
case τ.Δά:return "On hydrogen empty(<25%)";case τ.ΔΪ:return "On hydrogen empty(=0%)";case τ.ΔΫ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΘЄ,int Ň,bool Μ,String Ŭ){Ű+=1;if(ΘЄ==Ň) Ŭ=">"+Ŭ+(Βξ>=2?" .":"");else Ŭ=" "+Ŭ;Γ+=Ŭ+"\n";return ΘЄ==Ň&&Μ;}String Ζζ(bool Μ)
{int Β=0;int Κ=Г;Ű=0;String Ʒ="———————————————\n";String ƾ="--------------------------------------------\n";String Γ="";Γ+=ΓЍ(η)+" | "+(Ρ.ν?"Ready to dock":"No dock")+"\n";Γ+=Ʒ;double ǟ=Math.Max(Math.Round(this.Ь),0);if(Ё==Φ.Main){bool ŉ=Ξ==Ν.ί;if(Π(ref Γ,Κ,Β++,Μ," Record path & set home")) Δξ();
if(Ξ==Ν.ΑΖ)
if(Π(ref Γ,Κ,Β++,Μ," Setup mining job")) ο(Φ.ΑΕ);if(Ξ==Ν.Ǹ)
if(Π(ref Γ,Κ,Β++,Μ," Setup grinding job")) ο(Φ.ΑΕ);if(Ξ==Ν.ί)
if(Π(ref Γ,Κ,Β++,Μ," Setup shuttle job")) ο(Φ.Δκ);if(Π(ref Γ,Κ,Β++,Μ," Continue job")) Ĺ();if(Π(ref Γ,Κ,Β++,Μ," Fly to home position")) ΒΙ();if(Π(ref Γ,Κ,Β++,Μ," Fly to job position")) ŗ();if(Π(ref Γ,Κ,Β++,Μ," Behavior settings"))
if(ŉ) ο(Φ.Ββ);else ο(Φ.ΑБ);if(Π(ref Γ,Κ,Β++,Μ," Info")) ο(Φ.ΖЇ);if(Ξ!=Ν.ί)
if(Π(ref Γ,Κ,Β++,Μ," Help")) ο(Φ.ΖЅ);}else if(Ё==Φ.ΑΕ){double Μγ=Math.Round(љ*Ȫ,1);double Λα=Math.Round(ј*ȩ,1);String К="";if(Π(ref К,Κ,Β++,Μ," Start new job!")) Δν();if(Π(ref К,Κ,Β++,Μ," Change current job")){ΑΞ(false,false);ο(Φ.Main);}if(Π(ref К,Κ,Β++,Μ," Width + (Width: "+љ+" = "+Μγ+"m)")){Ƃ(ref љ,5,20,1);Ş(true);}
if(Π(ref К,Κ,Β++,Μ," Width -")){Ƃ(ref љ,-5,20,-1);Ş(true);}if(Π(ref К,Κ,Β++,Μ," Height + (Height: "+ј+" = "+Λα+"m)")){Ƃ(ref ј,5,20,1);Ş(true);}if(Π(ref К,Κ,Β++,Μ," Height -")){Ƃ(ref ј,-5,20,-1);Ş(true);}if(Π(ref К,Κ,Β++,Μ," Depth + ("+(ǔ==ǡ.ΓЂ?"Depth":"Min")+": "+ф+"m)")){Ƃ(ref ф,5,50,2);Ş(true);}
if(Π(ref К,Κ,Β++,Μ," Depth -")){Ƃ(ref ф,-5,50,-2);Ş(true);}if(Π(ref К,Κ,Β++,Μ," Depth mode: "+ΙϏ(ǔ))){ǔ=ĺ(ǔ);}if(Π(ref К,Κ,Β++,Μ," Start pos: "+ΙЉ(Ȥ))){Ȥ=ĺ(Ȥ);}if(Ξ==Ν.Ǹ&&ǔ==ǡ.Γϊ) ǔ=ĺ(ǔ);Γ+=Δε(8,К,Κ,ref ΛЍ);}else if(Ё==Φ.Δκ){float[]ΗЅ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Κ,Β++,Μ," Next")) ο(Φ.ΗΤ);if(Π(ref Γ,Κ,Β++,Μ," Back")) ο(Φ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Κ,Β++,Μ," - "+ΖЃ(Ќ.Ч))) Ќ.Ч=ĺ(Ќ.Ч);if(!Ќ.ΔΒ()) Γ+="\n";else if(Π(ref Γ,Κ,Β++,Μ," - Delay: "+ΒΚ((int) Ќ.Ǿ))) Ќ.Ǿ=Ζπ(Ќ.Ǿ,ΗЅ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Κ,Β++,Μ," - "+ΖЃ(Ѝ.Ч))) Ѝ.Ч=ĺ(Ѝ.Ч);
if(!Ѝ.ΔΒ()) Γ+="\n";else if(Π(ref Γ,Κ,Β++,Μ," - Delay: "+ΒΚ((int) Ѝ.Ǿ))) Ѝ.Ǿ=Ζπ(Ѝ.Ǿ,ΗЅ);}else if(Ё==Φ.ΗΤ){if(Π(ref Γ,Κ,Β++,Μ," Start job!")) Δν();if(Π(ref Γ,Κ,Β++,Μ," Back")) ο(Φ.Δκ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Κ,Β++,Μ," = "+(Ќ.ǿ!=""?Ќ.ǿ:"-"))) Ќ.ǿ=ΒΧ(ref ΜΡ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Κ,Β++,Μ," = "+(Ќ.Ȅ!=""?Ќ.Ȅ:"-"))) Ќ.Ȅ=ΒΧ(ref ΜΤ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Κ,Β++,Μ," = "+(Ѝ.ǿ!=""?Ѝ.ǿ:"-"))) Ѝ.ǿ=ΒΧ(ref ΜΣ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Κ,Β++,Μ," = "+(Ѝ.Ȅ!=""?Ѝ.Ȅ:"-"))) Ѝ.Ȅ=ΒΧ(ref ΜΥ);
}else if(Ё==Φ.ΑΔ){String Θε=ȟ+" %";if(ħ) ň++;if(ň>1){ň=0;ΓΑ++;if(ΓΑ>1) ΓΑ=0;}bool[]ΕЏ=new bool[]{Ō.Count==0,Ȕ==ǐ.None,ǩ.Count==0};int ΙΠ=0;while(true){ΙΠ++;Ũ++;if(Ũ>ΕЏ.Length-1) Ũ=0;if(ΙΠ>=ΕЏ.Length) break;if(!ΕЏ[Ũ]) break;}bool ŉ=Ξ==Ν.ί;if(!ŉ&&Ķ&&м!=-1&&ΓΑ==0)
Θε=м<1000000?Math.Round(м)+" Kg":Math.Round(м/1000)+" t";if(Π(ref Γ,Κ,Β++,Μ," Stop!")){Ї();ο(Φ.Main);}if(Π(ref Γ,Κ,Β++,Μ," Behavior settings"))
if(!ŉ) ο(Φ.ΑБ);else ο(Φ.Ββ);if(!ŉ){if(Π(ref Γ,Κ,Β++,Μ," Next hole")) Γσ(false);}else if(Π(ref Γ,Κ,Β++,Μ," Undock")) ΑІ=true;Γ+=ƾ;if(!ŉ) Γ+="Progress: "+Math.Round(Αξ,1)+" %\n";Γ+="State: "+ΓЏ(Ω)+" "+ǟ+"m \n";Γ+="Load: "+ΓΙ+" % Max: "+Θε+" \n";if(Ũ==0) Γ+="Uranium: "+(Ō.Count==0?"No reactors":Math.Round(Ž,1)+"Kg "+(ц==-1?"":" Min: "+ц+" Kg"))+"\n";
if(Ũ==1) Γ+="Battery: "+(Ȕ==ǐ.None?Ζχ(Ȕ):Ă+"% "+(ǚ==-1||ŉ?"":" Min: "+ǚ+" %"))+"\n";if(Ũ==2) Γ+="Hydrogen: "+(ǩ.Count==0?"No tanks":Math.Round(Ħ,1)+"% "+(Ǜ==-1||ŉ?"":" Min: "+Ǜ+" %"))+"\n";}else if(Ё==Φ.ΑБ){String К="";if(Π(ref К,Κ,Β++,Μ," Back")){if(η==Τ.Л) ο(Φ.ΑΔ);else ο(Φ.Main);}
if(Π(ref К,Κ,Β++,Μ," Max load: "+ȟ+"%")) ђ(ref ȟ,ȟ<=80?-10:-5,Ѐ.ΒΡ,false);if(Π(ref К,Κ,Β++,Μ," Weight limit: "+(Ķ?"On":"Off"))) Ķ=!Ķ;if(Π(ref К,Κ,Β++,Μ," Ejection: "+ΙЁ(л))) л=ĺ(л);if(Π(ref К,Κ,Β++,Μ," Toggle sorters: "+(Ż?"On":"Off"))){Ż=!Ż;if(Ż) Ȓ(ΖΑ);}
if(Π(ref К,Κ,Β++,Μ," Unload ice: "+(ĵ?"On":"Off"))) ĵ=!ĵ;if(Π(ref К,Κ,Β++,Μ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ђ(ref ц,(ц>5?-5:-1),Ѐ.ΒΣ,true);if(Π(ref К,Κ,Β++,Μ," Battery: "+(ǚ==-1?"Ignore":"Min "+ǚ+"%"))) ђ(ref ǚ,-5,Ѐ.ΔΣ,true);if(Π(ref К,Κ,Β++,Μ," Hydrogen: "+(Ǜ==-1?"Ignore":"Min "+Ǜ+"%"))) ђ(ref Ǜ,-10,Ѐ.ΔΤ,true);
if(Π(ref К,Κ,Β++,Μ," When done: "+(ŏ?"Return home":"Stop"))) ŏ=!ŏ;if(Π(ref К,Κ,Β++,Μ," On damage: "+Ζω(ѝ))) ѝ=ĺ(ѝ);if(Π(ref К,Κ,Β++,Μ," Advanced...")) ο(Φ.Γτ);Γ+=Δε(8,К,Κ,ref ΛЎ);}else if(Ё==Φ.Γτ){if(Π(ref Γ,Κ,Β++,Μ," Back")){if(η==Τ.Л) ο(Φ.ΑΔ);else ο(Φ.Main);}
if(Π(ref Γ,Κ,Β++,Μ,(Ξ==Ν.Ǹ?" Grinder":" Drill")+" inv. balancing: "+(ƥ?"On":"Off"))) ƥ=!ƥ;if(Π(ref Γ,Κ,Β++,Μ," Enable"+(Ξ==Ν.Ǹ?" grinders":" drills")+": "+(Ɵ?"Fwd + Bwd":"Fwd"))) Ɵ=!Ɵ;if(Π(ref Γ,Κ,Β++,Μ," Work speed fwd.: "+ǆ+"m/s")) ђ(ref ǆ,0.5f,Ѐ.ΑΟ,false);
if(Π(ref Γ,Κ,Β++,Μ," Work speed bwd.: "+ǅ+"m/s")) ђ(ref ǅ,0.5f,Ѐ.ΑΟ,false);if(Π(ref Γ,Κ,Β++,Μ," Acceleration: "+Math.Round(ȓ*100f)+"%"+(ȓ>0.80f?" (risky)":""))) ђ(ref ȓ,0.1f,Ѐ.ΑЎ,false);if(Π(ref Γ,Κ,Β++,Μ," Width overlap: "+ſ*100f+"%")) Ζλ(true,0.05f);if(Π(ref Γ,Κ,Β++,Μ," Height overlap: "+ŧ*100f+"%")) Ζλ(false,0.05f);
}else if(Ё==Φ.Ββ){if(Π(ref Γ,Κ,Β++,Μ," Back")){if(η==Τ.Л) ο(Φ.ΑΔ);else ο(Φ.Main);}if(Π(ref Γ,Κ,Β++,Μ," Max load: "+ȟ+"%")) ђ(ref ȟ,ȟ<=80?-10:-5,Ѐ.ΒΡ,false);if(Π(ref Γ,Κ,Β++,Μ," Unload ice: "+(ĵ?"On":"Off"))) ĵ=!ĵ;if(Π(ref Γ,Κ,Β++,Μ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ђ(ref ц,(ц>5?-5:-1),Ѐ.ΒΣ,true);
if(Π(ref Γ,Κ,Β++,Μ," Battery: "+(ǚ==-1?"Ignore":"Charge up"))) ǚ=(ǚ==-1?1:-1);if(Π(ref Γ,Κ,Β++,Μ," Hydrogen: "+(Ǜ==-1?"Ignore":"Fill up"))) Ǜ=(Ǜ==-1?1:-1);if(Π(ref Γ,Κ,Β++,Μ," On damage: "+Ζω(ѝ))) ѝ=ĺ(ѝ);if(Π(ref Γ,Κ,Β++,Μ," Acceleration: "+Math.Round(ȓ*100f)+"%"+(ȓ>0.80f?" (risky)":""))) ђ(ref ȓ,0.1f,Ѐ.ΑЎ,false);
}else if(Ё==Φ.ΗΜ){double Λξ=0;if(Ψ.Count>0) Λξ=Vector3.Distance(Ψ.Last().Ε,β);if(Π(ref Γ,Κ,Β++,Μ," Stop path recording")) ΗΩ();if(Ξ!=Ν.ί){if(Π(ref Γ,Κ,Β++,Μ," Home: "+(ē?"Use old home":(Ρ.ν?"Was set! ":"none ")))) ē=!ē;}else{if(Π(ref Γ,Κ,Β++,Μ," Connector 1: "+(ē?"Use old connector":(Ρ.ν?"Was set! ":"none ")))) ē=!ē;
if(Π(ref Γ,Κ,Β++,Μ," Connector 2: "+(ǁ?"Use old connector":(Ζ.ν?"Was set! ":"none ")))) ǁ=!ǁ;}if(Π(ref Γ,Κ,Β++,Μ," Path: "+(ǂ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ǂ=!ǂ;Γ+=ƾ;Γ+="Wp spacing: "+Math.Round(ΙΨ)+"m\n";}else if(Ё==Φ.Δπ){if(Π(ref Γ,Κ,Β++,Μ," Stop")){Ї();ο(Φ.Main);}
Γ+=ƾ;Γ+="State: "+ΓЏ(Ω)+" \n";Γ+="Speed: "+Math.Round(Đ,1)+"m/s\n";Γ+="Target dist: "+ǟ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+ΖΪ+"\n";}else if(Ё==Φ.ΖЇ){List<IMyTerminalBlock>ţ=Ζϊ();if(ħ) ň++;if(ň>=ţ.Count) ň=0;if(Π(ref Γ,Κ,Β++,Μ," Next")) ο(Φ.ΖЈ);
Γ+=ƾ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΓΙ,1)+"% "+Math.Round(Ș,1)+" / "+Math.Round(Ƞ,1)+"\n";Γ+="Uranium: "+(Ō.Count==0?"No reactors":Math.Round(Ž,1)+"Kg "+Εξ)+"\n";Γ+="Battery: "+(Ȕ==ǐ.None?"":Ă+"% ")+Ζχ(Ȕ)+"\n";Γ+="Hydrogen: "+(ǩ.Count==0?"No tanks":Math.Round(Ħ,1)+"% ")+"\n";
Γ+="Damage: "+(ţ.Count==0?"None":""+(ň+1)+"/"+ţ.Count+" "+ţ[ň].CustomName)+"\n";}else if(Ё==Φ.ΖЈ){if(Π(ref Γ,Κ,Β++,Μ," Back")) ο(Φ.Main);Γ+=ƾ;Γ+="Next scan: "+Αϋ+"s\n";Γ+="Drill area: "+Math.Round(Ȫ,1)+"x"+Math.Round(ȩ,1)+"m Ship: "+Math.Round(ΕЌ,1)+"x"+Math.Round(ΕЋ,1)+"m ("+Math.Round(δ,1)+"m)\n"+(ȕ.Count>0?"Cameras: "+ΔЍ+"\n":"");
Γ+="Broadcast: "+(Ƙ?"Online - "+Ƹ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ƭ*100f,1)+"% \n";}else if(Ё==Φ.ΖЅ){if(Π(ref Γ,Κ,Β++,Μ," Back")) ο(Φ.Main);Γ+=ƾ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Е==2) Γ="Fatal setup error\nNext scan: "+Αϋ+"s\n";if(Γή) Γ="Recompile script now";int Λο=Γ.Split('\n').Length;for(int i=Λο;i<=10;i++) Γ+="\n";Γ+=Ʒ;Γ+="Last: "+Λ+"\n";
return Γ;}void Ζλ(bool Λθ,float ǰ){Ї();ΑΞ(true,false);if(Λθ) ђ(ref ſ,ǰ,Ѐ.Δί,false);else ђ(ref ŧ,ǰ,Ѐ.Δί,false);Ζι();Ī=Ȫ;ĩ=ȩ;ŝ(true,true,0,0);}float Ζπ(float σ,float[]Γπ){float α=Γπ[0];for(int i=Γπ.Length-1;i>=0;i--)
if(σ<Γπ[i]) α=Γπ[i];return α;}String ΒΧ(ref int Ň){String α="";if(Ň>=ΖΜ.Count) Ň=-1;if(Ň>=0) α=ΖΜ[Ň].CustomName;Ň++;return α;}void Βζ(string ΰ){if(η!=Τ.Л) return;if(ΰ=="") return;IMyTerminalBlock Ϋ=χ.GetBlockWithName(ΰ);if(Ϋ==null||!(Ϋ is IMyTimerBlock)){Λ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ϋ).Trigger();}void Ƃ(ref int value,int ΑΧ,int ĳ,int ΕА){if(ΑΧ==0) return;if(value<ĳ&&ΕА>0||value<=ĳ&&ΕА<0){value+=ΕА;return;}int ΚЋ=Math.Abs(ΑΧ);int Δϋ=0;int Εχ=1;while(true){Δϋ+=Εχ*ΚЋ*10;if(ΑΧ<0&&value-ĳ<=Δϋ) break;if(ΑΧ>0&&value-ĳ<Δϋ) break;
Εχ++;}value+=Εχ*ΑΧ;}void Ş(bool Μρ){љ=Math.Max(љ,1);ј=Math.Max(ј,1);ф=Math.Max(ф,0);ŝ(Ё==Φ.ΑΕ,false,0,0);}bool Ś(ref float value,bool ΛЉ,Ѐ ϊ,String ũ,String Λε){if(ũ=="") return false;float ư=-1;bool ΘΥ=false;if(ũ.ToUpper()==Λε) ΘΥ=true;else if(!float.TryParse(ũ,out ư)) return false;
else ư=Math.Max(0,ư);if(ΛЉ) ư=(float) Math.Round(ư);ђ(ref value,ư,ϊ,ΘΥ,false);return true;}void ђ(ref float value,float ǰ,Ѐ ϊ,bool ΑЅ){ђ(ref value,value+ǰ,ϊ,ΑЅ,true);}void ђ(ref float value,float ƭ,Ѐ ϊ,bool ΑЅ,bool Μθ){float ȇ=0,ȅ=0;if(ϊ==Ѐ.ΑΟ){ȇ=0.5f;ȅ=10f;}if(ϊ==Ѐ.ΑЎ){ȇ=0.1f;ȅ=1f;}
if(ϊ==Ѐ.ΑΑ){ȇ=50f;ȅ=100f;}if(ϊ==Ѐ.ΔΣ){ȇ=5f;ȅ=30f;}if(ϊ==Ѐ.ΒΣ){ȇ=1f;ȅ=25f;}if(ϊ==Ѐ.ΒΡ){ȇ=10f;ȅ=95f;}if(ϊ==Ѐ.ΔΤ){ȇ=10f;ȅ=90f;}if(ϊ==Ѐ.Κσ){ȇ=10f;ȅ=1800;}if(ϊ==Ѐ.Δί){ȇ=0.0f;ȅ=0.75f;}if(ƭ==-1&&ΑЅ){value=-1;return;}if(value==-1) ΑЅ=false;
bool Θϋ=ƭ<ȇ||ƭ>ȅ;if(Θϋ&&Μθ){if(ƭ<value) value=ȅ;else if(ƭ>value) value=ȇ;}else value=ƭ;if(Θϋ&&ΑЅ) value=-1;else value=Math.Max(ȇ,Math.Min(value,ȅ));value=(float) Math.Round(value,2);}void Γσ(bool ΛЄ){if(ΛЄ) й=Math.Max(0,й-1);else й++;ΓЈ();ΓЇ();for(int k=0;k<Ĉ.Count&&k<к.Length;k++) Δθ(Ĉ[k],k<й);
Й=-1;Δτ(true);}T ĺ<T>(T σ){int Β=Array.IndexOf(Enum.GetValues(σ.GetType()),σ);Β++;if(Β>=Ιψ(σ)) Β=0;return(T) Enum.GetValues(σ.GetType()).GetValue(Β);}int Ιψ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δξ(){ΑЌ.Clear();for(int i=0;i<Ψ.Count;i++) ΑЌ.Add(Ψ[i]);Ψ.Clear();
ΓΕ=true;Εή=new έ(Ρ);Εβ=new έ(Ζ);Ρ.ν=false;if(Ξ==Ν.ί) Ζ.ν=false;for(int i=0;i<Ǳ.Count;i++)
if(!đ.Contains(Ǳ.Keys.ElementAt(i)))
đ.Add(Ǳ.Keys.ElementAt(i));ē=false;ǁ=false;ǂ=false;ο(Φ.ΗΜ);}void ΗΩ(){if(ē) Ρ=Εή;if(ǁ) Ζ=Εβ;if(ǂ){Ψ.Clear();for(int i=0;i<ΑЌ.Count;i++) Ψ.Add(ΑЌ[i]);}ΓΕ=false;Ї();ο(Φ.Main);}void ΚЂ(){if(!ΓΕ) return;if(Ω!=Α.ѕ){ΗΩ();return;}if(!Εή.ν) ē=false;if(!Εβ.ν) ǁ=false;if(ΑЌ.Count<=1) ǂ=false;
IMyShipConnector Х=о(MyShipConnectorStatus.Connectable);if(Х==null) Х=о(MyShipConnectorStatus.Connected);if(Х!=null){if(Math.Round(Đ,2)<=0.20) ΕΚ++;else ΕΚ=0;if(ΕΚ>=5){if(Ξ==Ν.ί&&(Ρ.ν||ē)&&Vector3.Distance(Ρ.Ε,Х.GetPosition())>5){Ζ.μ=Ϊ.WorldMatrix.Forward;Ζ.Є=Ϊ.WorldMatrix.Left;
Ζ.λ=Ϊ.WorldMatrix.Down;Ζ.ό=Ϊ.GetNaturalGravity();Ζ.Ε=Х.GetPosition();Ζ.ν=true;Ζ.х=Х.Position;}else{Ρ.μ=Ϊ.WorldMatrix.Forward;Ρ.Є=Ϊ.WorldMatrix.Left;Ρ.λ=Ϊ.WorldMatrix.Down;Ρ.ό=Ϊ.GetNaturalGravity();Ρ.Ε=Х.GetPosition();Ρ.ν=true;Ρ.х=Х.Position;}}}double Εε=-1;if(Ψ.Count>0) Εε=Vector3.Distance(β,Ψ.Last().Ε);
double ΜΕ=Math.Max(1.5,Math.Pow(Đ/100.0,2));double Θξ=Math.Max(Đ*ΜΕ,2);ΙΨ=Θξ;if((Εε==-1)||Εε>=Θξ){έ ή=new έ(β,ΒЏ,Αή,Αο,Ϊ.GetNaturalGravity());ή.Ιβ(Ē,đ);Ψ.Add(ή);}}έ є(){if(Ξ!=Ν.ί) return Ǭ!=null?Ǭ:Ρ;return ΗЀ;}void Ə(έ γ,Τ Љ){ΗЀ=γ;if(η==Τ.Л) Ǚ=Љ;}bool ȯ(έ γ,float υ,bool Μή,out Vector3 ά)
{if(Μή){Vector3I ΕΦ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);IMySlimBlock Ϋ=Me.CubeGrid.GetCubeBlock(ΕΦ);if(Ϋ==null||!(Ϋ.FatBlock is IMyShipConnector)){ά=new Vector3();return false;}Vector3 ΛΘ=ъ(Ϊ,Ϋ.FatBlock.GetPosition()-β);Vector3 ΛΗ=ъ(Ϊ,Ϋ.FatBlock.WorldMatrix.Forward);ά=γ.Ε-Δσ(γ.μ,γ.λ*-1,ΛΘ)-Δσ(γ.μ,γ.λ*-1,ΛΗ)*υ;
return true;}else{ά=γ.Ε;return true;}}Vector3 Ė(int ΕΑ,int ō,bool ΛЇ){if(!ΛЇ&&ΘΘ) return Βσ;float ΛΖ=((Б-1f)/2f)-ΕΑ;float ΛЊ=((Я-1f)/2f)-ō;Βσ=Ζ.Ε+Ζ.Є*ΛΖ*Ƌ()+ȡ*-1*ΛЊ*Ɗ();ΘΘ=true;return Βσ;}Vector3 ΓЎ(Vector3 ΜΙ,float ΕΖ){return ΜΙ+(Ф*ΕΖ);}ι ΓЃ(){float Ы=-1;
ι α=ι.Ȭ;if(Ξ!=Ν.ί){ΗΑ();if(η!=Τ.ы){Vector3 Т=Ŝ(Ф,ȡ*-1,β-Ζ.Ε);if(Math.Abs(Т.X)<=(float)(Б*Ȫ)/2f&&Math.Abs(Т.Y)<=(float)(Я*ȩ)/2f){if(Т.Z<=-1&&Т.Z>=-Ÿ*2) return ι.ΑА;if(Т.Z>-1&&Т.Z<δ*2) return ι.Ȭ;}if(ĸ(Ζ.Ε,ref Ы)) α=ι.Ȭ;}if(Ρ.ν){if(ĸ(Ρ.Ε,ref Ы)) α=ι.ȫ;
for(int i=0;i<Ψ.Count;i++)
if(ĸ(Ψ[i].Ε,ref Ы)) α=ι.ė;if(Vector3.Distance(β,Ρ.Ε)<dockDist*δ) α=ι.ȫ;if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null)
α=ι.ȫ;}}else{Vector3 ά=new Vector3();IMyShipConnector Ą=о(MyShipConnectorStatus.Connected);if(Ρ.ν){if(ĸ(Ρ.Ε,ref Ы)) α=ι.ȫ;if(ȯ(Ρ,dockDist,true,out ά))
if(ĸ(ά,ref Ы)) α=ι.ȫ;if(Ą!=null&&Vector3.Distance(Ą.GetPosition(),Ρ.Ε)<5)
return ι.Γχ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ρ.Ε)>dockDist*δ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*δ)
if(ĸ(Ψ[i].Ε,ref Ы)) α=ι.ė;if(Ζ.ν){if(ĸ(Ζ.Ε,ref Ы)) α=ι.Ȭ;if(ȯ(Ζ,dockDist,true,out ά))
if(ĸ(ά,ref Ы)) α=ι.Ȭ;if(Ą!=null&&Vector3.Distance(Ą.GetPosition(),Ζ.Ε)<5)
return ι.Γψ;}}return α;}bool ĸ(Vector3 ά,ref float Ы){float υ=Vector3.Distance(ά,β);if(υ<Ы||Ы==-1){Ы=υ;return true;}return false;}void Δν(){if(Е>0){Λ="Setup error! Can't start";return;}if(Ξ==Ν.ί){Ĺ();return;}Ζ.Ε=β;Ζ.ό=Ϊ.GetNaturalGravity();Ζ.μ=ΒЏ;Ζ.λ=Αή;Ζ.Є=Αο;
Ф=ǭ.WorldMatrix.Forward;ȡ=Ζ.λ;if(Ф==Ϊ.WorldMatrix.Down) ȡ=Ϊ.WorldMatrix.Backward;Ī=Ȫ;ĩ=ȩ;ΑΞ(true,true);Υ(Α.Ƿ);Δμ();}void ΑΞ(bool Ȁ,bool ΛЈ){if(η==Τ.ы&&!Ȁ) return;bool Λψ=Ȁ||η==Τ.ΓЅ||Б!=љ||Я!=ј||ū!=Ȥ;if(Λψ){if(η!=Τ.ы){η=Τ.ΒΒ;Ė(І,Ђ,ΛЈ);Λ="Job changed, lost progress";
}ū=Ȥ;Б=љ;Я=ј;Ђ=0;І=0;к="";Й=-1;Ū=(int)(DateTime.Now.Ticks/10000000%1000000);ů=0;ĭ=0;п=0;й=0;Δτ(true);}}void Δα(){в(β,0);ǥ(Ē,true);}void Δμ(){Ȳ(ь.Βή);Δα();ΑΙ(ċ,false);Ȓ(ΖΑ);η=Τ.Л;Ζθ(true);Ǚ=η;ο(Φ.ΑΔ);Κΰ();ΒϏ=true;ΓΒ=0;for(int i=ΑΠ.Count-1;i>=0;i--)
if(ΑΒ(ΑΠ[i],false)) ΓΒ++;if(ΓΒ>0) Λ="Started with damage";}void Ї(){if(η==Τ.Л){η=Τ.ƍ;Λ="Job paused";}Υ(Α.ѕ);Ǚ=η;Βα(false,0,0,0,0);Δρ();Ζγ(new Vector3(),false);ΗΨ();Δη(ChargeMode.Auto);Δι(false);ΗΞ(true);Ę(Α.ѕ);ŝ(false,false,0,0);ǥ(ώ,false);ǥ(Ǔ,true);Ȓ(true);Γί=false;ǈ();
Αέ=false;Δγ();ΒϏ=false;Αύ=false;ΑІ=false;if(Ё!=Φ.Main&&Ё!=Φ.ΑБ&&Ё!=Φ.Γτ&&Ё!=Φ.Ββ)
ο(Φ.Main);}void Ĺ(){ι е=ΓЃ();if(Ξ==Ν.ί){if(!Ζ.ν||!Ρ.ν) return;Δμ();bool ΕΤ=Vector3.Distance(β,Ρ.Ε)<Vector3.Distance(β,Ζ.Ε);if(Ƶ==Τ.ƀ) ΕΤ=true;if(Ƶ==Τ.Ɓ) ΕΤ=false;if(ΕΤ){Ə(Ρ,Τ.ƀ);switch(е){case ι.Γχ:Υ(Α.ľ);break;case ι.ė:Υ(Α.т);break;case ι.ȫ:Υ(Α.ǵ);break;default:Υ(Α.ǻ);break;
}}else{Ə(Ζ,Τ.Ɓ);switch(е){case ι.Γψ:Υ(Α.ľ);break;case ι.Ȭ:Υ(Α.ǵ);break;case ι.ė:Υ(Α.т);break;default:Υ(Α.ǻ);break;}}}else{if(η!=Τ.ƍ&&η!=Τ.ΒΒ) return;bool Μα=η==Τ.ΒΒ;Δμ();bool ΕΫ=ΑΓ(false)&&Ρ.ν;switch(е){case ι.Ȭ:Υ(ΕΫ?Α.Ȯ:Α.Ƿ);break;case ι.ė:Υ(ΕΫ?Α.И:Α.т);break;case ι.ȫ:Υ(ΕΫ?Α.ǵ:Α.Ā);break;
case ι.ΑА:{if(ĭ!=й||Μα) Υ(Α.Ȑ);else Υ(Α.ř);}break;default:break;}}}void ŗ(){if(η==Τ.ы&&!Ρ.ν) return;if(Ξ==Ν.ί&&(!Ζ.ν||!Ρ.ν)) return;Λ="Move to job";ι е=ΓЃ();if(Ξ==Ν.ί){Ə(Ζ,Τ.Ɓ);switch(е){case ι.Ȭ:Υ(Α.ǵ);break;case ι.ė:Υ(Α.т);break;case ι.Γψ:return;default:Υ(Α.ǻ);break;
}Ę(Α.ľ);}else{switch(е){case ι.Ȭ:Υ(Α.т);break;case ι.ė:Υ(Α.т);break;case ι.ȫ:Υ(Α.Ā);break;case ι.ΑА:Υ(Α.Ȑ);break;default:break;}if(η==Τ.ы) Ę(Α.т);else Ę(Α.ķ);Γί=true;}Δα();ο(Φ.Δπ);ΑΙ(ċ,false);Ǚ=Τ.ΒΥ;}void ΒΙ(){if(!Ρ.ν) return;Λ="Move home";ι е=ΓЃ();if(Ξ==Ν.ί){Ə(Ρ,Τ.ƀ);switch(е)
{case ι.ė:Υ(Α.И);break;case ι.ȫ:Υ(Α.ǵ);break;case ι.Γχ:return;default:Υ(Α.ǻ);break;}Ę(Α.ľ);}else{if(о(MyShipConnectorStatus.Connected)!=null) return;if(о(MyShipConnectorStatus.Connectable)!=null){Υ(Α.ŕ);Ę(Α.Ā);return;}switch(е){case ι.Ȭ:Υ(Α.Ȯ);break;case ι.ė:Υ(Α.И);break;case ι.ȫ:Υ(Α.И);break;
case ι.ΑА:Υ(Α.Ǥ);break;default:break;}Ę(Α.Ā);}Δα();ο(Φ.Δπ);ΑΙ(ċ,false);Ǚ=Τ.ΒΤ;}void Υ(Α Εω){if(Εω==Α.ѕ) ΑЀ=Α.ѕ;if(ΑЀ!=Α.ѕ&&Ω==ΑЀ&&Εω!=ΑЀ){Ї();return;}Ж=true;Ω=Εω;}void Ę(Α Љ){this.ΑЀ=Љ;}Ǻ Μν(int Αλ,bool ȉ){if(ȉ){ǯ=null;І=0;Ђ=0;}if(Ȥ==і.ΑΝ){int Ų=Αλ+1;
Ђ=(int) Math.Floor(М(Αλ,Б));if(Ђ%2==0) І=Αλ-(Ђ*Б);else І=Б-1-(Αλ-(Ђ*Б));if(Ђ>=Я) return Ǻ.ΑЏ;else return Ǻ.ΒΛ;}else if(Ȥ==і.Ζκ){if(ǯ==null) ǯ=new int[]{0,-1,0,0};int Αθ=(int) Math.Ceiling(Б/2f);int Αη=(int) Math.Ceiling(Я/2f);int ΕΩ=(int) Math.Floor(Б/2f);
int ΕΨ=(int) Math.Floor(Я/2f);int Αμ=0;while(ǯ[2]<Math.Pow(Math.Max(Б,Я),2)){if(Αμ>200) return Ǻ.Ιω;Αμ++;ǯ[2]++;if(-Αθ<І&&І<=ΕΩ&&-Αη<Ђ&&Ђ<=ΕΨ){if(ǯ[3]==Αλ){this.І=І-1+Αθ;this.Ђ=Ђ-1+Αη;return Ǻ.ΒΛ;}ǯ[3]++;}if(І==Ђ||(І<0&&І==-Ђ)||(І>0&&І==1-Ђ))
{int ΜΜ=ǯ[0];ǯ[0]=-ǯ[1];ǯ[1]=ΜΜ;}І+=ǯ[0];Ђ+=ǯ[1];}}return Ǻ.ΑЏ;}void ΚЀ(){if(Ω==Α.Ƿ){if(Ж){Њ=0;if(ĭ!=й) ů=0;ĭ=й;}if(Њ==0){int ΑΨ=Ιι(й);if(ΑΨ<0){if(!ΒΓ()){Υ(Α.Ǥ);return;}if(!Βϋ||(ΑΨ==-2)!=Βϊ){Βϋ=true;Βϊ=ΑΨ==-2;double a=(ќ%6)*Math.PI/3;Ηύ=!Βϊ?β:Ζ.Ε-Ф*δ*2.5f+
(Ζ.Є*(float) Math.Cos(a)+ȡ*(float) Math.Sin(a))*δ*1.2f;}в(Ηύ,10);Λ=Βϊ?"Waiting for a free hole":"Waiting for the controller";return;}if(Βϋ) Λ="";Βϋ=false;if(ΑΨ!=й){й=ΑΨ;ů=0;ĭ=й;}Ǻ α=Κι(й);if(α==Ǻ.ΑЏ){η=Τ.ΓЅ;Λ="Job done";if(ŏ&&Ρ.ν){Υ(Α.Ȯ);Ę(Α.Ā);Ǚ=Τ.ΒΤ;}
else{Υ(Α.ƅ);Ę(Α.ķ);Ǚ=Τ.ΒΥ;}return;}if(α==Ǻ.ΒΛ){Њ=1;ǥ(ώ,true);О=Ė(І,Ђ,true);float h=Vector3.Dot(β-О,Ф);if((β-О-Ф*h).Length()>Math.Max(Ƌ(),Ɗ())*1.5f) h=Math.Min(h,-δ);в(h<-1?О+Ф*h:О,10);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}}else{if(Й>=0&&(ΖА()?ȗ>=0&&ȗ!=Й:ΔΔ(Й,true))){Њ=0;return;}
if(Ь<wpReachedDist){if(Vector3.Distance(ψ,О)>1){в(О,10);return;}Υ(Α.ř);return;}if(Đ<0.3f&&Ǎ<0&&!ŋ){if(++Њ<30) return;Њ=1;Vector3 ΓΧ=О-β;float Ĝ=Vector3.Dot(ΓΧ,Ф);if(Ĝ>0&&(ΓΧ-Ф*Ĝ).Length()<wpReachedDist){Βσ=β;Υ(Α.ř);}else в(О+Ф*(Math.Min(0,-Ĝ)-δ),10);
return;}Њ=1;}}if(Ω==Α.ř){if(Ж){ǥ(ώ,true);Ȓ(false);О=Ė(І,Ђ,false);в(ΓЎ(О,0),0);с(Ζ.λ,Ζ.μ,Ζ.Є,false);Њ=1;њ=0;щ=0;Βπ=0;Εη=-1;Ÿ=ф;Εΰ=true;}if(!ΒΓ()){Υ(Α.Ǥ);return;}if(ΑΓ(true)){Θω=Ĕ("","ORE",б.Œ);if((л==З.ΓІ||л==З.ΒΔ||
л==З.ΔΖ||л==З.ΒΞ)&&Ξ!=Ν.Ǹ)
Υ(Α.Ŗ);else if((л==З.Ζο||л==З.ΓϏ)&&Ξ!=Ν.Ǹ)
Υ(Α.Δυ);else
Υ(Α.Ǥ);return;}п=Vector3.Distance(β,О);if(п>ů){ů=п;Εΰ=false;}if(Ξ==Ν.Ǹ&&Ζϋ()==MyDetectedEntityType.SmallGrid)
щ+=2;else щ-=2;щ=Math.Max(100,Math.Min(400,щ));if(Њ>0&&Њ<щ){if(п>њ){if(щ>150) њ=п;else њ=(float) Math.Ceiling(п);Њ=1;}else Њ++;}else{if(Њ>0){Λ="Ship stuck! Retrying";њ=п;Њ=0;ŝ(false,true,0,δ*sensorRange);}в(ΓЎ(О,Math.Max(0,њ-δ)),ΓБ(false));if(Ь<=wpReachedDist/2){Њ=1;њ=0;}
return;}ŝ(false,true,δ*sensorRange,0);Vector3 ΚЍ=О+Ф*п;bool ΘΩ=false;if(Vector3.Distance(ΚЍ,β)>0.3f){Vector3 ΛΜ=О+Ф*(п+0.1f);в(ΛΜ,4);ΘΩ=true;}else{float Ȋ=ΓБ(true);Vector3 ΗЄ=ΓЎ(О,Math.Max(ф+1,п+1));ȑ(true,false,false,ΗЄ,ΗЄ-О,Ȋ,Ȋ);}bool ΕΗ=false;if(ǔ==ǡ.Γϊ||ǔ==ǡ.Γω)
{if(!ΘΩ){float Εϋ=0;foreach(IMyTerminalBlock Ȩ in ώ)
Εϋ+=Ις(Ȩ,"","",ǔ==ǡ.Γω?new string[]{"STONE"}:null);if(Εϋ>Εη||п<ф||Εΰ){ΔЁ=0;Βπ=п;Ÿ=(int)(Math.Max(Ÿ,Βπ)+δ/2);}else{ΕΗ=п-Βπ>2&&ΔЁ>=20;ΔЁ++;}Εη=Εϋ;}}else ΕΗ=п>=Ÿ;if(ĭ!=й){Υ(Α.Ȑ);п=0;return;}if(ΕΗ){й++;Υ(Α.Ȑ);п=0;return;}}if(Ω==Α.Ȑ||Ω==Α.Ǥ||Ω==Α.Ŗ)
{if(Ж){О=Ė(І,Ђ,false);Vector3 ѐ=β-О;if(Ω==Α.Ǥ&&Ρ.ν&&
(ѐ-Ф*Vector3.Dot(ѐ,Ф)).Length()>Math.Max(Ƌ(),Ɗ())){Υ(Α.Ȯ);return;}с(Ζ.λ,Ζ.μ,Ζ.Є,false);ǥ(ώ,Ɵ);Ȓ(false);њ=Vector3.Distance(β,О);ŝ(false,true,0,δ*sensorRange);}if(Ж) Αβ=false;в(Αβ?О-Ф*δ:О,ΓБ(false));if(!Αβ&&Vector3.Distance(β,О)>=њ+5){ǥ(ώ,false);Ȓ(true);Λ="Can´t return!";}
if(Ь<wpReachedDist){if(Ω==Α.Ǥ&&!Αβ){Αβ=true;return;}if(Ω==Α.Ȑ&&Γί) Υ(Α.ƅ);if(Ω==Α.Ȑ) Υ(Α.Ƿ);if(Ω==Α.Ŗ) Υ(Α.ΒΘ);if(Ω==Α.Ǥ){if(Ρ.ν) Υ(Α.Ȯ);else{Ї();ŗ();Λ="Can´t return, no dock found";}}return;}}if(Ω==Α.И||Ω==Α.т){if(Ω==Α.т&&η==Τ.Л&&Ξ!=Ν.ί){if(!ΒΓ()||ΑΓ(true)){Υ(Α.И);return;}
}bool ΕЄ=false;bool Γξ=false;bool Ηθ=false;float ƕ=0;bool Θψ=false;έ ή=null;if(Ж){if(Ω==Α.И||Ξ==Ν.ί){if(Ξ!=Ν.ί){if(ę(1)) Γφ();else{Ǭ=null;Ď=0;Αέ=!Γώ(1);}}έ γ=є();ς=new Δΰ();ς.ζ=γ;ς.ΑΫ=followPathDock*δ;ς.ΖΧ=useDockDirectionDist*δ;ς.ƕ=10;ς.Ƽ.Add(γ.Ε);Vector3 Ɣ=new Vector3();
if(ȯ(γ,dockDist*δ,true,out Ɣ))
ς.Ƽ.Add(Ɣ);else ς.ΑΫ*=1.5f;if(Ξ==Ν.ί){if(γ==Ρ) ς.ΒЋ=Ζ.Ε;if(γ==Ζ) ς.ΒЋ=Ρ.Ε;ς.ΕΠ=dockDist*δ*1.1f;}}else if(Ω==Α.т){ς=new Δΰ();ς.ζ=Ζ;ς.ΑΫ=followPathJob*δ;ς.ΖΧ=useJobDirectionDist*δ;ς.ƕ=10;ς.ΒЋ=Ρ.Ε;ς.ΕΠ=dockDist*δ*1.1f;ς.Ƽ.Add(Ζ.Ε);if(η==Τ.ы){if(!Ρ.ν||Ψ.Count==0){Ї();return;}
float ΛΤ=Vector3.Distance(Ψ.First().Ε,Ρ.Ε);float ΛΥ=Vector3.Distance(Ψ.Last().Ε,Ρ.Ε);if(ΛΤ<ΛΥ) ς.ζ=Ψ.Last();else ς.ζ=Ψ.First();}}Αχ=new Vector3();Θψ=!ΚΗ(β);ǥ(ώ,false);Ȓ(true);р=-1;double Ȗ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ς.ΒЋ)<=ς.ΕΠ) continue;
double υ=Vector3.Distance(Ψ[i].Ε,β);if(Ȗ==-1||υ<Ȗ){р=i;Ȗ=υ;}}Ȣ=ΙІ(ς.ζ.Ε,р);Ţ=null;}if(Ω==Α.И&&Ξ!=Ν.ί&&!Ж&&!ΑΛ()){έ ΔІ=є();Αέ=!Γώ(1);if(є()!=ΔІ){Υ(Α.И);return;}}if(Ω==Α.И&&Ξ!=Ν.ί&&!Ж&&ę(1)&&Γφ()){Υ(Α.И);return;}bool ΘΧ=false;if(Ω==Α.И&&!Ж&&р>=0&&р<Ψ.Count)
{Vector3 ΑΪ=Ψ[р].Ε+Ζώ(р,Ȣ);int Εϊ=р+Ȣ;Vector3 Ų=Ȣ!=0&&Εϊ>=0&&Εϊ<Ψ.Count?Ψ[Εϊ].Ε:ς.ζ.Ε;if(ΙБ(ΑΪ,Ų)){if(!ΒЄ){ΒЄ=true;ΗЉ=Ɖ(ΑΪ)?β:ΑΪ;}в(ΗЉ,0);с(ς.ζ.λ,ς.ζ.μ,ς.ζ.Є,false);Λ="Waiting for dock clearance";return;}ΒЄ=false;ΘΧ=ę(1)&&(Ɖ(β)||Ɖ(Ų));}Ιγ(Ψ,Ȣ,ς.Ƽ,ς.ΑΫ,Ж,ref Њ);
for(int i=0;i<ς.Ƽ.Count;i++){float υ=Vector3.Distance(β,ς.Ƽ[i]);if(υ<=ς.ΑΫ) ΕЄ=true;if(υ<=ς.ΖΧ) Γξ=true;}if(ΘΧ) Γξ=true;if(Γξ) ƕ=ς.ƕ;float Μς=Ţ!=null?Ţ.У:Đ;float Μη=(float) Math.Max(Đ*0.1f*δ,wpReachedDist);if((Ь<Μη)||Ж){if(!Ж) р+=Ȣ;if(Ȣ==0||р>Ψ.Count-1||р<0)
ΕЄ=true;else{ΖΪ=Ȣ>0?Ψ.Count-1-р:р;ή=Ψ[р];Ţ=ή;if(р>=1&&р<Ψ.Count-1)
Αχ=ή.Ε-Ψ[р-Ȣ].Ε;else Ţ=null;ψ=ή.Ε+Ζώ(р,Ȣ);Ηθ=true;}}if(Γξ)
с(ς.ζ.λ,ς.ζ.μ,ς.ζ.Є,false);else if(Θψ)
Γυ(ς.ζ.λ,10,true);else if(Ηθ&&ή!=null){if(Ȣ>0) с(ή.λ,ή.μ,ή.Є,90,false);else с(ή.λ,-ή.μ,-ή.Є,90,false);}ȑ(true,false,true,ψ,Αχ,Ţ==null?0:Ţ.У,ƕ);if(ΕЄ){ΖΪ=0;if(Ω==Α.И&&!Ιη()){в(ψ,0);return;}if(Ω==Α.И&&Αέ){в(ψ,0);Λ="Waiting for a free dock";return;}
if(Ω==Α.И||Ξ==Ν.ί){Υ(Α.ǵ);return;}if(Ω==Α.т&&Γί){Υ(Α.ƅ);return;}if(Ω==Α.т){Υ(Α.Ƿ);return;}}}if(Ω==Α.ǵ||Ω==Α.ΑΘ){if(Ξ!=Ν.ί&&ΑΛ()){if(!ę(1)){Υ(Α.И);return;}}else if(Ξ!=Ν.ί){έ ΔІ=є();if(!Γώ(2)||є()!=ΔІ){Υ(Α.И);return;}}έ γ=є();if(Ж){if(!ȯ(γ,dockDist*δ,true,out ψ))
{Ȳ(ь.ǉ);Ї();return;}в(ψ,0);Γυ(γ.λ,90,true);}if(Ь<followPathDock*δ&&Ь!=-1){в(ψ,10);с(γ.λ,γ.μ,γ.Є,false);}if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null){Υ(Α.ŕ);return;}if(Ɯ.LengthSquared()>0&&Ь<wpReachedDist&&Ь!=-1){Ɯ=new Vector3();return;}
if(Ь<wpReachedDist/2&&Ь!=-1){Υ(Α.ő);return;}}if(Ω==Α.ő||Ω==Α.ķ){if(Ж){if(Ω==Α.ő){έ γ=є();if(!ȯ(γ,dockDist*δ,true,out ψ)){Ȳ(ь.ǉ);Ї();return;}ȑ(true,true,false,ψ,0);с(γ.λ,γ.μ,γ.Є,10,false);}if(Ω==Α.ķ){с(Ζ.λ,Ζ.μ,Ζ.Є,0.5f,false);ψ=Ζ.Ε;ȑ(true,true,false,ψ,0);
}}if(ΓΔ){Βα(false,0,0,0,0);if(Ω==Α.ő) Υ(Α.ŕ);if(Ω==Α.ķ) Ї();return;}}if(Ω==Α.ŕ){if(о(MyShipConnectorStatus.Connected)!=null){if(Ξ==Ν.ί) Υ(Α.ľ);else Υ(Α.Ā);return;}έ γ=є();if(Ж){щ=0;ΛΦ=DateTime.Now;Њ=0;с(γ.λ,γ.μ,γ.Є,false);}Vector3I ΕΦ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);
IMySlimBlock Ηϊ=Me.CubeGrid.GetCubeBlock(ΕΦ);float ΗЌ=dockingSpeed;float Λς=dockingSpeed*5;float Ηε=Math.Max(1.5f,Math.Min(5f,δ*0.15f));if(!ȯ(γ,0,true,out ψ)||!ȯ(γ,Ηε,true,out Αχ)||
Ηϊ==null||!Ηϊ.FatBlock.IsFunctional){Ȳ(ь.ǉ);Ї();return;}if(щ==1||(Vector3.Distance(β,ψ)<=Ηε*1.1f&&!Ж))
щ=1;else{Vector3 ΜΦ=ъ(Ϊ,Αχ-β);Vector3 ΕΥ=ъ(Ϊ,Ϊ.GetNaturalGravity());float ΛЋ=ΒΑ(ΜΦ,ΕΥ,null);ΗЌ=Math.Min(Λς,ΛЋ);}ȑ(true,false,false,ψ,ψ-β,dockingSpeed,ΗЌ);if(Ж) њ=(float) Ь;IMyShipConnector ΕΒ=о(MyShipConnectorStatus.Connectable);if(ΕΒ!=null){ȑ(false,false,false,ψ,0);
if(Њ>0) Њ=0;Њ--;if(Њ<-5){ΕΒ.Connect();if(ΕΒ.Status==MyShipConnectorStatus.Connected){if(Ξ==Ν.ί) Υ(Α.ľ);else Υ(Α.Ā);Δρ();ΑΙ(ċ,true);return;}}}else{float ΗЈ=(float) Math.Round(Ь,1);if(ΗЈ<њ){Њ=-1;њ=ΗЈ;}else Њ++;if(Њ>20){Υ(Α.ΑΘ);return;}}}if(Ω==Α.Ā||Ω==Α.ľ||
Ω==Α.Βι||Ω==Α.ΒΖ||Ω==Α.Ŕ){bool ΔϏ=false,ΔЀ=false;if(Ξ==Ν.ί){if(є()==Ρ) ΔϏ=true;else if(є()==Ζ) ΔЀ=true;}if(Ж){Αύ=false;if(о(MyShipConnectorStatus.Connected)==null){Υ(Α.ǻ);return;}Δρ();if(ΔϏ) Βζ(Ќ.ǿ);if(ΔЀ) Βζ(Ѝ.ǿ);ż=false;ĝ=false;ĥ=false;ΑЈ=false;}if(о(MyShipConnectorStatus.Connected)==null){Ї();Ȳ(ь.ΖЋ);return;}
if(η!=Τ.Л||ǚ==-1||Ȕ==ǐ.None) ĝ=true;else if(Ă>=100f) ĝ=true;else if(Ă<=99f) ĝ=false;if(η!=Τ.Л||Ǜ==-1||ǩ.Count==0) ĥ=true;else if(Ħ>=100f) ĥ=true;else if(Ħ<=99) ĥ=false;if(η!=Τ.Л||ц==-1||Ō.Count==0) ΑЈ=true;else ΑЈ=Ž>=ц;Ƒ Ǐ=null;if(ΔϏ) Ǐ=Ќ;
if(ΔЀ) Ǐ=Ѝ;if(Ǐ!=null&&(Ǐ.Ч==τ.ΒΪ||Ǐ.Ч==τ.ΒΩ)) ĝ=true;if(Ǐ!=null&&(Ǐ.Ч==τ.ΔΨ)) if(!ż) ĝ=false;if(Ǐ!=null&&(Ǐ.Ч==τ.Δά||Ǐ.Ч==τ.ΔΪ)) ĥ=true;if(Ǐ!=null&&(Ǐ.Ч==τ.ΔΫ)) if(!ż) ĥ=false;if(ħ){ChargeMode Ησ=ĝ?ChargeMode.Auto:ChargeMode.Recharge;if(Ǐ!=null&&(Ǐ.Ч==τ.ΒΩ||Ǐ.Ч==τ.ΒΪ))
Ησ=ChargeMode.Discharge;Δη(Ησ);Δι(!ĥ);}if(!ż){if(Ξ==Ν.ί)
ż=η!=Τ.Л||Ιθ(Ж,true)||ΑІ;else
ż=η!=Τ.Л||ΖЌ();}else{if(!ĝ) Υ(Α.Ŕ);if(!ĥ) Υ(Α.ΒΖ);if(!ΑЈ) Υ(Α.Βι);Ж=false;}if(ż&&ĝ&&ĥ&&ΑЈ){Δη(ChargeMode.Auto);Δι(false);if(η==Τ.Л){if(Ξ==Ν.ί){if(є()==Ρ) Βζ(Ќ.Ȅ);else if(є()==Ζ) Βζ(Ѝ.Ȅ);if(є()==Ρ) Ə(Ζ,Τ.Ɓ);else Ə(Ρ,Τ.ƀ);}}Υ(Α.ǻ);return;}}if(Ω==Α.ǻ){if(ΑЋ){if(!Ζμ()) return;
ΑЋ=false;Υ(Α.ǻ);return;}if(Ж){if(!Ζμ()){ΑЋ=true;return;}IMyShipConnector Ą=о(MyShipConnectorStatus.Connected);if(Ą==null){Υ(Α.т);return;}IMyShipConnector θ=Ą.OtherConnector;ΗΝ(Ą,false);ΑΙ(ċ,false);έ γ=null;έ ǖ=Ξ==Ν.ί?Ρ:є();if(Vector3.Distance(Ą.GetPosition(),ǖ.Ε)<5f&&Ρ.ν) γ=ǖ;
if(Vector3.Distance(Ą.GetPosition(),Ζ.Ε)<5f&&Ζ.ν) γ=Ζ;if(γ!=null){if(!ȯ(γ,dockDist*δ,true,out ψ)){Ȳ(ь.ǉ);Ї();return;}в(ψ,5);с(γ.λ,γ.μ,γ.Є,false);}else в(β+θ.WorldMatrix.Forward*dockDist*δ,5);if(η==Τ.Л) Ȳ(ь.Βή);}if(Ь<wpReachedDist){ǥ(Ǔ,true);Υ(Α.т);return;}}if(Ω==Α.ƅ)
{if(Ж){Ȓ(true);ǥ(ώ,false);ψ=Ζ.Ε;в(ψ,20);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}if(Ь<wpReachedDist/2){Υ(Α.ķ);return;}}if(Ω==Α.Ȯ){if(Ж){Ȓ(true);ǥ(ώ,false);int Ы=-1;double Ȗ=-1;bool ΘЇ=ΒΦ();for(int č=0;č<2&&Ы==-1;č++,ΘЇ=false)
for(int i=Ψ.Count-1;i>=0;i--){if(ΘЇ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)<ΖБ()+δ) continue;double υ=Vector3.Distance(Ψ[i].Ε,β);if(Ȗ==-1||υ<Ȗ){Ы=i;Ȗ=υ;}}if(Ы==-1){Υ(Α.И);return;}ψ=Ψ[Ы].Ε;в(ψ,10);с(Ζ.λ,Ζ.μ,Ζ.Є,false);}if(Ь<wpReachedDist){Υ(Α.И);return;}}if(Ω==Α.ΒΘ)
{bool φ=false;if(Ж){Ȓ(true);if((л==З.ΓІ||л==З.ΒΔ)&&ΒΠ()&&
ƃ(Ф,Ϊ.GetNaturalGravity())<25&&Б>=2&&Я>=2){Vector3 Αί=β;if(І>0&&Ђ<Я-1) Αί=Ė(І-1,Ђ+1,true);else if(І<Б-1&&Ђ<Я-1) Αί=Ė(І+1,Ђ+1,true);else if(І<Б-1&&Ђ>0) Αί=Ė(І+1,Ђ-1,true);else if(І>0&&Ђ>0) Αί=Ė(І-1,Ђ-1,true);else φ=true;
if(!φ) в(Αί,10);}else φ=true;}if(Ь<wpReachedDist/2) φ=true;if(φ){Υ(Α.Βθ);return;}}if(Ω==Α.Δυ||Ω==Α.Βθ){if(Ж){ȑ(true,true,false,β,0);ǥ(ώ,false);Ȓ(true);Њ=-1;щ=л==З.ΔΖ||л==З.ΒΞ?0:-1;}bool ť=!ΒΓ();int Γη=Ĕ("STONE","ORE",б.Œ);if(л==З.ΓϏ||л==З.ΒΞ||л==З.ΒΔ)
Γη+=Ĕ("ICE","ORE",б.Œ);bool Λΰ=Γη>0;bool ΕΟ=false;if(щ>=0){float ƒ=(float) Math.Sin(Ζρ(щ))*Ȫ/3f;float ΚЏ=(float) Math.Cos(Ζρ(щ))*ȩ/3f;Vector3 ΛΕ=Ė(І,Ђ,true)+Δσ(Ф,ȡ*-1,new Vector3(ƒ,ΚЏ,0));в(ΛΕ,0.3f);if(Ь<Math.Min(Ȫ,ȩ)/10f) щ+=5f;if(щ>=360) щ=0;}if(Њ==-1||Γη<Њ){Њ=Γη;њ=0;}
else{њ++;if(њ>50) ΕΟ=true;}if(!Λΰ||ť||ΕΟ){if(!ť){int ΜΫ=Ĕ("","ORE",б.Œ);if(ΑΓ(true)) ť=true;else if(100-(М(ΜΫ,Θω)*100)<minEjection) ť=true;else Ȳ(ь.Βή);}if(ΕΟ&&ť) Λ="Ejection failed";if(Ω==Α.Βθ){if(ť){if(Ρ.ν) Υ(Α.Ȯ);else{Ї();ŗ();Λ="Can´t return, no dock found";}}
else Υ(Α.Ƿ);}else if(ť) Υ(Α.Ǥ);else Υ(Α.ř);return;}}Ж=false;}void Ιγ(List<έ>Ő,int ρ,List<Vector3>ΑЃ,float ǟ,bool ȉ,ref int ǽ){if(ȉ){for(int i=0;i<Ő.Count;i++) Ő[i].У=0;ǽ=-1;return;}if(ρ==0) return;int ǰ=ρ*-1;if(ǽ==-1) ǽ=ǰ>0?1:Ő.Count-2;int Αμ=0;while(ǽ>=1&&ǽ<Ő.Count-1)
{if(Αμ>50) return;Αμ++;try{if((ǰ<0&&ǽ>=1)||(ǰ>0&&ǽ<=Ő.Count-2)){έ σ=Ő[ǽ];bool Θτ=false;for(int j=0;j<ΑЃ.Count;j++)
if(Vector3.Distance(σ.Ε,ΑЃ[j])<=ǟ){Θτ=true;break;}if(!Θτ){έ ΓΪ=Ő[ǽ-ǰ];έ Ų=Ő[ǽ+ǰ];Vector3 ΜΧ=σ.Ε-Ų.Ε;Vector3 ΖΞ=ΓΪ.Ε-σ.Ε;Vector3 ΛІ=σ.Ε+Vector3.Normalize(ΜΧ)*ΖΞ.Length();Vector3 ΛΡ=ΓΪ.Ε-ΛІ;Vector3 ΓΚ=Ŝ(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΛΡ);Vector3 Λρ=Ŝ(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΖΞ);
Vector3 ΓΜ=Ŝ(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,σ.ό);σ.У=(float) Math.Sqrt(Math.Pow(ΓΪ.У,2)+Math.Pow(ΒΑ(-Λρ,ΓΜ,σ),2));for(int j=0;j<ΑЃ.Count;j++)
if(Vector3.Distance(ΓΪ.Ε,ΑЃ[j])<=ǟ){Vector3 ΓΝ=Ŝ(ρ>0?σ.μ:σ.μ*-1,σ.λ*-1,ΑЃ[j]-σ.Ε);float ΜΛ=ΒΑ(-ΓΝ,ΓΜ,σ);σ.У=Math.Min(σ.У,ΜΛ)/2f;}if(ΓΚ.Length()==0) ΓΚ=new Vector3(0,0,1);Vector3 Λπ=Ŝ(σ.μ,σ.λ*-1,σ.ό);float ΜΠ=ǌ(-ΓΚ,Λπ,σ);float Βκ=М(ΜΠ,ź);float ΖΛ=(float) Math.Sqrt(М(ΓΚ.Length()*1.0f,0.5f*Βκ));
σ.У=Math.Min(σ.У,(ΖΞ.Length()/ΖΛ)*ȓ);}}}catch{return;}ǽ+=ǰ;}ǽ=-1;}void Δτ(bool ȉ){if(ȉ){Αξ=0;return;}if(Ξ==Ν.ί) return;float φ=й*Math.Max(1,ф);if(ĭ==й) φ+=Math.Min(ф,п);float ΜΪ=Б*Я*Math.Max(1,ф);Αξ=Math.Max(Αξ,(float) Math.Min(φ/ΜΪ*100.0,100));}MyDetectedEntityType Ζϋ()
{try{if(Ȱ(ύ,true)&&!ύ.LastDetectedEntity.IsEmpty()) return ύ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓБ(bool ȝ){if(Ξ==Ν.Ǹ&&Ζϋ()==MyDetectedEntityType.None&&!ΑΒ(ύ,true))
return fastSpeed;else return ȝ?ǆ:ǅ;}void Κφ(){float Θλ=0,ΗЂ=0,ũ=0,Εό=0;for(int i=0;i<Ŀ.Count;i++){IMyBatteryBlock ΑΥ=Ŀ[i];if(!Ȱ(ΑΥ,true)) continue;Θλ+=ΑΥ.MaxStoredPower;ΗЂ+=ΑΥ.CurrentStoredPower;ũ+=ΑΥ.CurrentInput;Εό+=ΑΥ.CurrentOutput;}Ă=(float) Math.Round(М(ΗЂ,Θλ)*100,1);
if(ũ>=Εό) Ȕ=ǐ.Ŕ;else Ȕ=ǐ.Ζς;if(ũ==0&&Εό==0||Ă==100.0) Ȕ=ǐ.ѕ;if(Ŀ.Count==0) Ȕ=ǐ.None;}void Κύ(){float ΘΑ=0;for(int i=0;i<ǩ.Count;i++){IMyGasTank ΙΖ=ǩ[i];if(!Ȱ(ΙΖ,true)) continue;ΘΑ+=(float) ΙΖ.FilledRatio;}Ħ=М(ΘΑ,ǩ.Count)*100f;}void ΚЉ(){Ž=0;try{for(int i=0;i<Ō.Count;i++)
{IMyReactor ΕЅ=Ō[i];if(!Ȱ(ΕЅ,true)) continue;List<MyInventoryItem>А=new List<MyInventoryItem>();ΕЅ.GetInventory(0).GetItems(А);float Џ=0;for(int j=0;j<А.Count;j++){MyInventoryItem Ѓ=А[j];if(Ѓ.Type.SubtypeId.ToUpper()=="URANIUM"&&Ѓ.Type.TypeId.ToUpper().Contains("_INGOT"))
Џ+=(float) Ѓ.Amount;}if(Џ<Ž||i==0){Ž=Џ;Εξ=ΕЅ.CustomName;}}}catch(Exception e){Ĭ=e;}}void Κχ(){Ƞ=0;Ș=0;try{for(int i=0;i<ƙ.Count;i++){IMyTerminalBlock Ϋ=ƙ[i];if(!Ȱ(Ϋ,true)) continue;Ș+=(float) Ϋ.GetInventory(0).CurrentVolume;Ƞ+=(float) Ϋ.GetInventory(0).MaxVolume;
}ΓΙ=(float) Math.Min(Math.Round(М(Ș,Ƞ)*100,1),100.0);}catch(Exception e){Ĭ=e;}}void Κώ(){try{Ȃ.Clear();for(int i=0;i<ƙ.Count;i++){IMyTerminalBlock Ϋ=ƙ[i];if(!Ȱ(Ϋ,true)) continue;Ζβ(Ϋ,б.ƈ);}if(л!=З.ΔΧ){for(int i=0;i<Š.Count;i++){IMyTerminalBlock Ϋ=Š[i];if(!Ȱ(Ϋ,true)) continue;
Ζβ(Ϋ,б.ΚΕ);}}Κμ(Ȃ);}catch(Exception e){Ĭ=e;}}void Ιζ(){if(ΒϏ){if(Ζϊ().Count>ΓΒ){ΒϏ=false;if(ѝ!=ѓ.ΑΑ){Ї();if(ѝ==ѓ.ΓЌ) ŗ();if(ѝ==ѓ.Βέ)
if(Ρ.ν) ΒΙ();else ŗ();}Λ="Damage detected";}}}bool ΒΓ(){if(!ΓΓ) return true;if(η==Τ.Л){if(ǚ>0&&Ȕ!=ǐ.None)
if(Ă<=ǚ){Λ="Low energy! Move home";return false;}if(ц>0&&Ō.Count>0)
if(Ž<=ц){Λ="Low fuel: "+Εξ;return false;}if(Ǜ>0&&ǩ.Count>0)
if(Ħ<=Ǜ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Ζϊ(){List<IMyTerminalBlock>ţ=new List<IMyTerminalBlock>();for(int i=0;i<ΑΠ.Count;i++){IMyTerminalBlock Ϋ=ΑΠ[i];if(ΑΒ(Ϋ,false)) ţ.Add(Ϋ);}return ţ;}bool ΑΒ(IMyTerminalBlock Ϋ,bool ΖΓ){return(!Ȱ(Ϋ,ΖΓ)||!Ϋ.IsFunctional);
}bool Ȱ(IMyTerminalBlock Ϋ,bool ΖΓ){if(Ϋ==null) return false;try{IMyCubeBlock ΘΗ=Me.CubeGrid.GetCubeBlock(Ϋ.Position).FatBlock;if(ΖΓ) return ΘΗ==Ϋ;else return ΘΗ.GetType()==Ϋ.GetType();}catch{return false;}}float Ις(IMyTerminalBlock Ϋ,String Ǡ,String ΘЎ,String[]ț){float ą=0;for(int i=0;i<Ϋ.InventoryCount;i++)
{IMyInventory Ƥ=Ϋ.GetInventory(i);List<MyInventoryItem>А=new List<MyInventoryItem>();Ƥ.GetItems(А);foreach(MyInventoryItem Ѓ in А){if(ț!=null&&(ț.Contains(Ѓ.Type.TypeId.ToUpper())||ț.Contains(Ѓ.Type.SubtypeId.ToUpper())))
continue;if((Ǡ==""||Ѓ.Type.TypeId.ToUpper()==Ǡ)&&(ΘЎ==""||Ѓ.Type.SubtypeId.ToUpper()==ΘЎ))
ą+=(float) Ѓ.Amount;}}return ą;}у Ιϋ(String ΰ,String ϊ,б е,bool ΛΝ){ΰ=ΰ.ToUpper();ϊ=ϊ.ToUpper();for(int i=0;i<Ȃ.Count;i++){у Ѓ=Ȃ[i];if(Ѓ.ΰ.ToUpper()==ΰ&&Ѓ.ϊ.ToUpper()==ϊ&&(Ѓ.Ʃ==е||е==б.Œ))
return Ѓ;}у α=null;if(ΛΝ){α=new у(ΰ,ϊ,0,е);Ȃ.Add(α);}return α;}int Ĕ(String ΰ,String ϊ,б е){return Ĕ(ΰ,ϊ,е,null);}int Ĕ(String ΰ,String ϊ,б е,String[]ț){int ą=0;ΰ=ΰ.ToUpper();ϊ=ϊ.ToUpper();for(int i=0;i<Ȃ.Count;i++){у Ѓ=Ȃ[i];if(ț!=null&&ț.Contains(Ѓ.ΰ.ToUpper())) continue;
if((ΰ==""||Ѓ.ΰ.ToUpper()==ΰ)&&(ϊ==""||Ѓ.ϊ.ToUpper()==ϊ)&&(Ѓ.Ʃ==е||е==б.Œ))
ą+=Ѓ.Џ;}return ą;}void Ζβ(IMyTerminalBlock Ϋ,б е){for(int i=0;i<Ϋ.InventoryCount;i++){List<MyInventoryItem>А=new List<MyInventoryItem>();Ϋ.GetInventory(i).GetItems(А);for(int j=0;j<А.Count;j++)
Ιϋ(А[j].Type.SubtypeId,А[j].Type.TypeId.Replace("MyObjectBuilder_",""),е,true).Џ+=(int) А[j].Amount;}}void Κμ(List<у>В){for(int i=В.Count-1;i>0;i--)
for(int j=0;j<i;j++){у a=В[j];у b=В[j+1];if(a.Џ<b.Џ) В.Move(j,j+1);}}bool ΑΓ(bool Μο){if(Ķ&&Ξ!=Ν.ί)
if(м!=-1&&ź>=м){Λ="Ship too heavy";return true;}if(ΓΙ>=ȟ||Αύ){Αύ=false;Λ="Ship is full";return true;}return false;}bool ΖЌ(){String[]ț=null;if(!ĵ) ț=new string[]{"ICE"};if(Ξ==Ν.ΑΖ) return Ĕ("","ORE",б.ƈ,ț)==0;if(Ξ==Ν.Ǹ) return Ĕ("","COMPONENT",б.ƈ,ț)==0;
else return Ĕ("","",б.ƈ,ț)==0;}bool ΚΘ(){List<IMyCockpit>ΔБ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔБ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔБ.Count;i++)
if(ΔБ[i].IsUnderControl) return true;return false;}bool Ιθ(bool ȉ,bool Γα){IMyShipConnector Х=о(MyShipConnectorStatus.Connected);if(Х==null) return false;if(Vector3.Distance(Ρ.Ε,Х.GetPosition())<5)
return Ζν(Ќ,ȉ,Γα);if(Vector3.Distance(Ζ.Ε,Х.GetPosition())<5)
return Ζν(Ѝ,ȉ,Γα);return false;}bool Ζν(Ƒ ă,bool ȉ,bool Γα){if(ȉ) ă.ΚΫ();ă.ΚЅ();bool ǎ=false;String Ǟ="";switch(ă.Ч){case τ.ΔΩ:{Ǟ="Waiting for command";ǎ=false;break;}case τ.ΗΗ:{Ǟ="Waiting for cargo";ǎ=ΑΓ(true);break;}case τ.ΗΖ:{Ǟ="Unloading";ǎ=ΖЌ();break;}case τ.Δή:{ǎ=true;break;}
case τ.ΔΨ:{Ǟ="Charging batteries";ǎ=Ă>=100f;break;}case τ.ΒΪ:{Ǟ="Discharging batteries";ǎ=Ă<=25f;break;}case τ.ΒΩ:{Ǟ="Discharging batteries";ǎ=Ă<=0f;break;}case τ.ΔΫ:{Ǟ="Filling up hydrogen";ǎ=Ħ>=100f;break;}case τ.Δά:{Ǟ="Unloading hydrogen";ǎ=Ħ<=25f;break;}
case τ.ΔΪ:{Ǟ="Unloading hydrogen";ǎ=Ħ<=0f;break;}case τ.Δέ:{bool ΘΦ=ΚΘ();if(!ΘΦ) ă.ΕЀ=true;ǎ=ă.ΕЀ&&ΘΦ;Ǟ="Waiting for passengers";break;}}if(!ǎ) ă.Γύ(true);if(ǎ&&ă.ΔΒ()){ǎ=ă.Γύ(false);Ǟ="Undocking in: "+ΒΚ((int) Math.Max(0,ă.Ǿ-ă.ń));}if(Γα) ΙΦ=Ǟ;return ǎ;}void ΙΫ()
{if(!ƥ) return;if(ώ.Count<=1) return;float ΙΝ=0,ΙΜ=0;for(int i=0;i<ώ.Count;i++){IMyTerminalBlock Ȩ=ώ[i];if(ΑΒ(Ȩ,true)) continue;ΙΝ+=(float) Ȩ.GetInventory(0).MaxVolume;ΙΜ+=(float) Ȩ.GetInventory(0).CurrentVolume;}float Ηι=(float) Math.Round(М(ΙΜ,ΙΝ),5);for(int č=0;č<Math.Max(1,Math.Floor(ώ.Count/10f));č++)
{float Εέ=0,Εν=0;float ΘΞ=0,Θθ=0;IMyTerminalBlock ģ=null,Ņ=null;for(int i=0;i<ώ.Count;i++){IMyTerminalBlock Ȩ=ώ[i];if(ΑΒ(Ȩ,true)) continue;float Ετ=(float) Ȩ.GetInventory(0).MaxVolume;float ΒЌ=М((float) Ȩ.GetInventory(0).CurrentVolume,Ετ);if(ģ==null||ΒЌ>Εέ){ģ=Ȩ;Εέ=ΒЌ;ΘΞ=Ετ;}
if(Ņ==null||ΒЌ<Εν){Ņ=Ȩ;Εν=ΒЌ;Θθ=Ετ;}}if(ģ==null||Ņ==null||ģ==Ņ) return;if(checkConveyorSystem&&!ģ.GetInventory(0).IsConnectedTo(Ņ.GetInventory(0))){if(ΔЅ>20) Λ="Inventory balancing failed";else ΔЅ++;return;}ΔЅ=0;List<MyInventoryItem>А=new List<MyInventoryItem>();
ģ.GetInventory(0).GetItems(А);float ΑЊ=0;if(А.Count==0) continue;MyInventoryItem ΒЍ=А[0];String ΘΪ=ΒЍ.Type.TypeId+ΒЍ.Type.SubtypeId;if(!ΙΥ.TryGetValue(ΘΪ,out ΑЊ)){if(ΚΞ(ģ.GetInventory(0),0,Ņ.GetInventory(0),out ΑЊ))
ΙΥ.Add(ΘΪ,ΑЊ);else return;}float ΜΨ=((Εέ-Ηι)*ΘΞ/ΑЊ);float ΜΩ=((Ηι-Εν)*Θθ/ΑЊ);int ΖΤ=(int) Math.Min(ΜΩ,ΜΨ);if(ΖΤ<=0) return;if((float) ΒЍ.Amount<ΖΤ)
ģ.GetInventory(0).TransferItemTo(Ņ.GetInventory(0),0,null,null,ΒЍ.Amount);else
ģ.GetInventory(0).TransferItemTo(Ņ.GetInventory(0),0,null,null,ΖΤ);}}bool ΚΞ(IMyInventory Αώ,int Β,IMyInventory ΛΠ,out float ΙΤ){ΙΤ=0;float ΛΒ=(float) Αώ.CurrentVolume;List<MyInventoryItem>ΔЇ=new List<MyInventoryItem>();Αώ.GetItems(ΔЇ);float Ην=0;for(int i=0;i<ΔЇ.Count;i++) Ην+=(float) ΔЇ[i].Amount;
Αώ.TransferItemTo(ΛΠ,Β,null,null,1);float ΙΡ=ΛΒ-(float) Αώ.CurrentVolume;List<MyInventoryItem>Δό=new List<MyInventoryItem>();Αώ.GetItems(Δό);float Ηα=0;for(int i=0;i<Δό.Count;i++) Ηα+=(float) Δό[i].Amount;if(ΙΡ==0f||!ΔΗ(0.9999,Ην-Ηα,1.0001))
return false;ΙΤ=ΙΡ;return true;}void Κό(){Vector3 ΒЎ=ψ+Ĵ+Ɯ;Vector3 ȧ=ΒЎ-β;if(ȧ.Length()==0) ȧ=new Vector3(0,0,-1);Vector3 ΓΝ=ъ(Ϊ,ȧ);Vector3 ΙΗ=Vector3.Normalize(ΓΝ);Vector3 ƨ=ъ(Ϊ,Ϊ.GetNaturalGravity());float ΜΖ=Εώ>0?Math.Max(0,1-ƃ(ȧ,ΘΓ)/5):0;float У=(float) Math.Min((Ερ>0?Ερ:1000f),Math.Max(ΒΑ(-ΓΝ,ƨ,null),Εώ*ΜΖ));
if(!ΙΛ) У=0;if(ΘЈ) У=Math.Max(0,1-Βύ/Βλ)*У;if(generalSpeedLimit>0) У=Math.Min(generalSpeedLimit,У);if(ŋ) У=0;if(Ǎ>=0) У=Math.Min(У,Ǎ);if(ΒΦ()) У=Math.Min(У,jobAreaSpeed);if(ΘЉ) У*=(float) Math.Min(1,М(ȧ.Length(),wpReachedDist)/2);Vector3 Θζ=ъ(Ϊ,Ϊ.GetShipVelocities().LinearVelocity);
float ΚЎ=(float)(Math.Max(0,15-ƃ(-ΙΗ,-Θζ))/15)*0.85f+0.15f;ΖΙ+=Math.Sign(ΚЎ-ΖΙ)/10f;Vector3 Ł=ΙΗ*У*ΖΙ-(Θζ);Vector3 ΔЃ=ΓА(Ł,null);if(Γι&&Ь>0.1f){Ł.X*=Δλ(Ł.X,ref Γκ.X,1f,ΔЃ.X,20);Ł.Y*=Δλ(Ł.Y,ref Γκ.Y,1f,ΔЃ.Y,20);Ł.Z*=Δλ(Ł.Z,ref Γκ.Z,1f,ΔЃ.Z,20);}else Γκ=new Vector3(1,1,1);
ΑЄ=ź*Ł-ƨ*ź;Ζγ(ΑЄ,Γι);Ь=Vector3.Distance(β,ΒЎ);}float Δλ(float value,ref float Љ,float ǰ,float Į,float Ες){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Љ)) Љ+=Math.Sign(Љ)*ǰ;else Љ=value;if(value==0) Љ=1;float α=Math.Abs(Љ);if(α<Ες||Į==0) return α;
Љ=Math.Min(Ες,Math.Max(-Ες,Љ));α=Math.Abs(Į);return α;}void Ηί(){float ǜ=90,Ǵ=90,Ǯ=90;float Ȋ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȝ,ǫ,Αρ,Εκ;if(Ελ){ȝ=Vector3.Normalize(ψ-β);Αρ=ъ(Ϊ,ȝ);ǫ=ъ(Ϊ,ΖΕ);ǜ=ƃ(Αρ,new Vector3(0,-1,0))-90;
Ǵ=œ(ǫ,new Vector3(-1,0,0),ǫ.Y);Ǯ=œ(Αρ,new Vector3(-1,0,0),Αρ.Z);}else{ȝ=ΖΖ;ǫ=ъ(Ϊ,ΖΕ);Αρ=ъ(Ϊ,ΖΖ);Εκ=ъ(Ϊ,ΙΘ);ǜ=œ(ǫ,new Vector3(0,0,1),ǫ.Y);Ǵ=œ(ǫ,new Vector3(-1,0,0),ǫ.Y);Ǯ=œ(Εκ,new Vector3(0,0,1),Εκ.X);}if(Ηγ&&ΒΠ()){Vector3 ΕΥ=Ϊ.GetNaturalGravity();ǫ=ъ(Ϊ,ΕΥ);
ǜ=œ(ǫ,new Vector3(0,0,1),ǫ.Y);Ǵ=œ(ǫ,new Vector3(-1,0,0),ǫ.Y);}if(!ΔΗ(-45,Ǵ,45)){ǜ=0;Ǯ=0;}if(!ΔΗ(-45,Ǯ,45)) ǜ=0;Βα(ΕΧ,1,(-ǜ)*Ȋ,(-Ǯ)*Ȋ,(-Ǵ)*Ȋ);Βύ=Math.Max(Math.Abs(ǜ),Math.Max(Math.Abs(Ǵ),Math.Abs(Ǯ)));ΓΔ=Βύ<=Βλ;}void Ιδ(IMyTerminalBlock Γά){if(Γά==null) return;
ȍ=new float[3,2];Ǳ=new Dictionary<string,float[,]>();for(int i=0;i<Ē.Count;i++){IMyThrust ё=Ē[i];if(!ё.IsFunctional) continue;Vector3 ρ=ъ(Γά,ё.WorldMatrix.Backward);float Ń=ё.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ȍ[0,0]+=Ń;else ȍ[0,1]-=Ń;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ȍ[1,0]+=Ń;else ȍ[1,1]-=Ń;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ȍ[2,0]+=Ń;else ȍ[2,1]-=Ń;String Ǡ=ΖЂ(ё);float[,]ǲ=null;if(Ǳ.ContainsKey(Ǡ)) ǲ=Ǳ[Ǡ];else{ǲ=new float[3,2];Ǳ.Add(Ǡ,ǲ);}float Į=ё.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ǲ[0,0]+=Į;else ǲ[0,1]-=Į;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ǲ[1,0]+=Į;else ǲ[1,1]-=Į;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ǲ[2,0]+=Į;else ǲ[2,1]-=Į;}}static String ΖЂ(IMyThrust ё){return ё.BlockDefinition.SubtypeId;}Vector3 ΔΞ(Vector3 ǧ,float[,]ƪ){return new Vector3(
ǧ.X>=0?ƪ[0,0]:ƪ[0,1],
ǧ.Y>=0?ƪ[1,0]:ƪ[1,1],
ǧ.Z>=0?ƪ[2,0]:ƪ[2,1]);}bool ΖЁ(έ ή,String Ǡ,out float ΕΞ){ΕΞ=0;int Β=đ.IndexOf(Ǡ);if(Β==-1||ή.Ȍ==null||Β>=ή.Ȍ.Length)
return false;ΕΞ=ή.Ȍ[Β];if(ΕΞ==-1) return false;return true;}Vector3 ΓА(Vector3 ǧ,έ ή){if(ή!=null){Vector3 α=new Vector3();for(int i=0;i<Ǳ.Keys.Count;i++){String Ǡ=Ǳ.Keys.ElementAt(i);float Αα=0;if(!ΖЁ(ή,Ǡ,out Αα))
return ΔΞ(ǧ,ȍ);α+=ΔΞ(ǧ,Ǳ.Values.ElementAt(i))*Αα;}return α;}return ΔΞ(ǧ,ȍ);}float ǌ(Vector3 ǧ,έ ή){return ǌ(ǧ,new Vector3(),ή);}float ǌ(Vector3 ǧ,Vector3 ό,έ ή){Vector3 ΜΟ=ΓА(ǧ,ή);Vector3 Ń=ΜΟ+ό*ź;float ŷ=(Ń/ǧ).AbsMin();return(float)(ǧ*ŷ).Length();}float ΒΑ(Vector3 ΓΛ,Vector3 ƨ,έ ή)
{if(ΓΛ.Length()==0) return 0;float Ηβ=1;if(ƨ.Length()>0) Ηβ=Math.Min(1,ƃ(-ƨ,ΓΛ)/90)*0.4f+0.6f;float ΑЄ=ǌ(ΓΛ,ƨ,ή);if(ΑЄ==0) return 0.1f;float Βκ=М(ΑЄ,ź);float ΖΛ=(float) Math.Sqrt(М(ΓΛ.Length(),Βκ*0.5f));return Βκ*ΖΛ*Ηβ*ȓ;}float ΒΝ(Vector3 ȝ,Vector3 Ѕ,Vector3 ό,έ ή)
{if(ό.Length()==0f) return 0;Vector3 ΓΜ=Ŝ(ȝ,Ѕ,Vector3.Normalize(ό));float Ȁ=ǌ(-ΓΜ,ή);return Ȁ/ό.Length();}void Ζθ(bool Λζ){float ǳ=0;float ΕΡ=0.9f;if(Λζ){м=-1;ž=0;Γδ=null;if(η!=Τ.ы&&Ζ.ό.Length()!=0){ǳ=ΕΡ*ΒΝ(Ζ.μ,Ζ.λ*-1,Ζ.ό,null);if(ǳ<м||м==-1) м=ǳ;}if(Ρ.ν&&Ρ.ό.Length()!=0)
{ǳ=ΕΡ*ΒΝ(Ρ.μ,Ρ.λ*-1,Ρ.ό,null);if(ǳ<м||м==-1) м=ǳ;}return;}if(ž==-1) return;if(ž>=0){int Θώ=0;while(ž<Ψ.Count){if(Θώ>100) return;Θώ++;έ ή=Ψ[ž];if(ή.ό.Length()!=0f){ǳ=ΕΡ*Math.Min(
ΒΝ(ή.μ,ή.λ*-1,ή.ό,ή),
ΒΝ(ή.μ*-1,ή.λ*-1,ή.ό,ή));if(ǳ<м||м==-1) м=ǳ;}else Γδ=ή;ž++;}ž=-1;}bool ΕΪ=true;float Ŋ=0;if(Γδ!=null){for(int i=0;i<Ǳ.Count;i++){String Ǡ=Ǳ.Keys.ElementAt(i);float[,]ǲ=Ǳ.Values.ElementAt(i);float Αα=0;if(!ΖЁ(Γδ,Ǡ,out Αα)){ΕΪ=false;break;}for(int a=0;a<ǲ.GetLength(0);a++)
for(int b=0;b<ǲ.GetLength(1);b++){float ş=Math.Abs(ǲ[a,b]*Αα);if(ş==0) continue;ΕΪ=true;if(Ŋ==0||ş<Ŋ) Ŋ=ş;}}}if(!ΕΪ){for(int a=0;a<ȍ.GetLength(0);a++)
for(int b=0;b<ȍ.GetLength(1);b++){float ş=Math.Abs(ȍ[a,b]);if(ş==0) continue;if(Ŋ==0||ş<Ŋ) Ŋ=ş;}}if(Ŋ>0){float Λτ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǳ=М(Ŋ,Λτ);if(ǳ>0)
if(ǳ<м||м==-1) м=ǳ;}}void Ζγ(Vector3 Ȁ,bool С){if(!С){for(int i=0;i<Ē.Count;i++)
Ē[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔЂ=ΓА(Ȁ,null);float Μι=Math.Min(1,Math.Abs(М(Ȁ.X,ΔЂ.X)));float Μκ=Math.Min(1,Math.Abs(М(Ȁ.Y,ΔЂ.Y)));float Μλ=Math.Min(1,Math.Abs(М(Ȁ.Z,ΔЂ.Z)));for(int i=0;i<Ē.Count;i++){IMyThrust ё=Ē[i];Vector3 ρ=Κέ(ъ(Ϊ,ё.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ȁ.X))
ё.SetValueFloat("Override",ё.MaxThrust*Μι);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ȁ.Y))
ё.SetValueFloat("Override",ё.MaxThrust*Μκ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ȁ.Z))
ё.SetValueFloat("Override",ё.MaxThrust*Μλ);else
ё.SetValueFloat("Override",0.0f);}}void Βα(bool С,float ΛЂ,float ǜ,float Ǯ,float Ǵ){for(int i=0;i<ΒА.Count;i++){IMyGyro Н=ΒА[i];Н.GyroOverride=С;if(!С) Н.GyroPower=100;else Н.GyroPower=ΛЂ;if(!С) continue;Vector3 д=Ϊ.WorldMatrix.Forward;Vector3 ч=Ϊ.WorldMatrix.Right;Vector3 Ѕ=Ϊ.WorldMatrix.Up;
Vector3 Ơ=Н.WorldMatrix.Forward;Vector3 Ƣ=Н.WorldMatrix.Up;Vector3 ơ=Н.WorldMatrix.Left*-1;if(Ơ==д) Н.SetValueFloat("Roll",Ǵ);else if(Ơ==(д*-1)) Н.SetValueFloat("Roll",Ǵ*-1);else if(Ƣ==(д*-1)) Н.SetValueFloat("Yaw",Ǵ);else if(Ƣ==д) Н.SetValueFloat("Yaw",Ǵ*-1);else if(ơ==д) Н.SetValueFloat("Pitch",Ǵ);
else if(ơ==(д*-1)) Н.SetValueFloat("Pitch",Ǵ*-1);if(ơ==(ч*-1)) Н.SetValueFloat("Pitch",ǜ);else if(ơ==ч) Н.SetValueFloat("Pitch",ǜ*-1);else if(Ƣ==ч) Н.SetValueFloat("Yaw",ǜ);else if(Ƣ==(ч*-1)) Н.SetValueFloat("Yaw",ǜ*-1);else if(Ơ==(ч*-1)) Н.SetValueFloat("Roll",ǜ);
else if(Ơ==ч) Н.SetValueFloat("Roll",ǜ*-1);if(Ƣ==(Ѕ*-1)) Н.SetValueFloat("Yaw",Ǯ);else if(Ƣ==Ѕ) Н.SetValueFloat("Yaw",Ǯ*-1);else if(ơ==Ѕ) Н.SetValueFloat("Pitch",Ǯ);else if(ơ==(Ѕ*-1)) Н.SetValueFloat("Pitch",Ǯ*-1);else if(Ơ==Ѕ) Н.SetValueFloat("Roll",Ǯ);else if(Ơ==(Ѕ*-1)) Н.SetValueFloat("Roll",Ǯ*-1);
}}void ΗΨ(){this.ΕΧ=false;}void с(Vector3 ș,Vector3 д,Vector3 ŭ,float ĳ,bool ΑЉ){Γυ(ș,ĳ,ΑЉ);Βλ=ĳ;Ελ=false;this.ΖΖ=д;this.ΙΘ=ŭ;}void с(Vector3 ș,Vector3 д,Vector3 ŭ,bool ΑЉ){с(ș,д,ŭ,2f,ΑЉ);}void Γυ(Vector3 ș,float ĳ,bool ΑЉ){Βλ=ĳ;this.ΕΧ=true;this.Ηγ=ΑЉ;Ελ=true;ΓΔ=false;
this.ΖΕ=ș;}void Δρ(){ȑ(false,false,false,ψ,0);Γι=false;}void в(Vector3 ζ,float Ȋ){ȑ(true,false,false,ζ,Ȋ);}void ȑ(bool С,bool ΕБ,bool ΖΨ,Vector3 ζ,float Ȋ){ȑ(С,ΕБ,ΖΨ,ζ,ζ-β,0.0f,Ȋ);}void ȑ(bool С,bool ΕБ,bool ΖΨ,Vector3 ζ,Vector3 ΛА,float ΛБ,float Ȋ){Γι=true;this.ΙΛ=С;
ψ=ζ;this.Ερ=Ȋ;this.Εώ=ΛБ;this.ΘЉ=ΕБ;this.ΘЈ=ΖΨ;this.ΘΓ=ΛА;Ь=Vector3.Distance(ζ,β);}void Κΰ(){χ.GetBlocksOfType(ΑΠ,Ǣ);}void Κα(){if(Γή){Е=2;return;}List<IMyRemoteControl>Αω=new List<IMyRemoteControl>();List<IMySensorBlock>ΘЅ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑЂ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αω,Ǣ);χ.GetBlocksOfType(а,Ǣ);χ.GetBlocksOfType(ΘЅ,Ǣ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑЂ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);а=Ƕ(а,pamTag,true);ΗΟ();ΗΠ(ΑЂ);ś(а,setLCDFontAndSize,1.4f,false);ś(ȥ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΙΓ=Ƕ(ΘЅ,pamTag,true);if(ΙΓ.Count>0) ύ=ΙΓ[0];else ύ=null;if(Ξ==Ν.ΑΖ){χ.GetBlocksOfType(ώ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ώ.Count==0){Е=1;Λ="Drills are missing";}}else if(Ξ==Ν.Ǹ){χ.GetBlocksOfType(ώ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ώ.Count==0){Е=1;Λ="Grinders are missing";}if(ύ==null){Е=1;Λ="Sensor is missing";}}else if(Ξ==Ν.ί){χ.GetBlocksOfType(ΖΜ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΙΒ=Ƕ(Αω,pamTag,true);if(ΙΒ.Count>0) Αω=ΙΒ;if(Αω.Count>0) Ϊ=Αω[0];else{Ϊ=null;Е=2;Λ="Remote is missing";return;}δ=(float) Ϊ.CubeGrid.WorldVolume.Radius*2;ǭ=null;if(Ξ!=Ν.ί){Ζι();if(ώ.Count>0&&ǭ!=null){if(ύ!=null&&(ǭ.WorldMatrix.Forward!=ύ.WorldMatrix.Forward||
!(Ϊ.WorldMatrix.Forward==ύ.WorldMatrix.Up||Ϊ.WorldMatrix.Down==ύ.WorldMatrix.Down))){Е=1;Λ="Wrong sensor direction";}if(ǭ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Forward&&
ǭ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Down){Е=2;Λ="Wrong remote direction";}}}}void Κβ(){χ.GetBlocksOfType(ċ,Ǣ);χ.GetBlocksOfType(Ǔ,Ǣ);χ.GetBlocksOfType(Ē,Ǣ);χ.GetBlocksOfType(ΒА,Ǣ);χ.GetBlocksOfType(Ŀ,Ǣ);χ.GetBlocksOfType(Ō,Ǣ);χ.GetBlocksOfType(ǩ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Γγ,Ǣ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
Ǔ=Ƕ(Ǔ,"ConnectorMedium",false);else
Ǔ=Ƕ(Ǔ,"Connector",false);List<IMyShipConnector>ΘБ=Ƕ(Ǔ,pamTag,true);if(ΘБ.Count>0) Ǔ=ΘБ;if(Е<=1){if(Ǔ.Count==0){Е=1;Λ="Connector is missing";}if(ΒА.Count==0){Е=1;Λ="Gyros are missing";}if(Ē.Count==0){Е=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΙΔ=Ƕ(Γγ,pamTag,true);
if(ΙΔ.Count>0) Γγ=ΙΔ;List<IMyLandingGear>ΙΑ=Ƕ(ċ,pamTag,true);if(ΙΑ.Count>0) ċ=ΙΑ;for(int i=0;i<ċ.Count;i++) ċ[i].AutoLock=false;List<IMyBatteryBlock>ΘА=Ƕ(Ŀ,pamTag,true);if(ΘА.Count>0) Ŀ=ΘА;List<IMyGasTank>ΙΕ=Ƕ(ǩ,pamTag,true);if(ΙΕ.Count>0) ǩ=ΙΕ;}void Κγ(){χ.GetBlocksOfType(Š,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ƙ.Clear();for(int i=Š.Count-1;i>=0;i--){if(ΚΖ(Š[i])){ƙ.Add(Š[i]);Š.RemoveAt(i);}}}bool ΚΖ(IMyTerminalBlock Ϋ){if(Ϋ is IMyCargoContainer) return true;if(Ϋ is IMyShipDrill) return true;if(Ϋ is IMyShipGrinder) return true;if(Ϋ is IMyShipConnector){if(((IMyShipConnector) Ϋ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΔΓ(Ϋ,"ConnectorSmall",false))
return false;else return true;}return false;}void Ζι(){if(Ϊ==null) return;ǭ=null;float ΓΤ=0,ΓΞ=0,ΓΥ=0,ΓΟ=0,ΓΦ=0,ΓΠ=0;List<IMyTerminalBlock>ΖΔ=Ƕ(ώ,pamTag,true);bool Λϋ=ΖΔ.Count==0;if(ΖΔ.Count>0) ǭ=ΖΔ[0];else if(ώ.Count>0) ǭ=ώ[0];int ą=0;float ď=0;for(int i=0;i<ώ.Count;i++)
{if(ώ[i].WorldMatrix.Forward!=ǭ.WorldMatrix.Forward){if(Λϋ){Е=2;Λ="Mining direction is unclear!";return;}continue;}ą++;ď=Math.Max(ď,Κτ(ώ[i]));Vector3 Т=ΒΜ(Ϊ,ώ[i].GetPosition());if(i==0){ΓΤ=Т.X;ΓΞ=Т.X;ΓΥ=Т.Y;ΓΟ=Т.Y;ΓΦ=Т.Z;ΓΠ=Т.Z;}ΓΞ=Math.Max(Т.X,ΓΞ);ΓΤ=Math.Min(Т.X,ΓΤ);
ΓΟ=Math.Max(Т.Y,ΓΟ);ΓΥ=Math.Min(Т.Y,ΓΥ);ΓΠ=Math.Max(Т.Z,ΓΠ);ΓΦ=Math.Min(Т.Z,ΓΦ);}if(ď==0) ď=drillRadius;bool ș=ǭ!=null&&ǭ.WorldMatrix.Forward==Ϊ.WorldMatrix.Down;Ȫ=(ΓΞ-ΓΤ)*(1-ſ)+ď*2;ȩ=(ș?ΓΠ-ΓΦ:ΓΟ-ΓΥ)*(1-ŧ)+ď*2;IMyCubeGrid g=Ϊ.CubeGrid;float Ζά=0,Ζέ=0,Ζή=0,Ζί=0;
for(int c=0;c<8;c++){Vector3 p=ΒΜ(Ϊ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ș?p.Z:p.Y;if(c==0||p.X<Ζά) Ζά=p.X;if(c==0||p.X>Ζέ) Ζέ=p.X;if(c==0||y<Ζή) Ζή=y;if(c==0||y>Ζί) Ζί=y;}ΕЌ=Ζέ-Ζά+g.GridSize;ΕЋ=Ζί-Ζή+g.GridSize;}float Κτ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ƕ<T>(List<T>В,String ȋ,bool ΒБ){List<T>α=new List<T>();for(int i=0;i<В.Count;i++)
if(ΔΓ(В[i],ȋ,ΒБ)) α.Add(В[i]);return α;}bool ΔΓ<T>(T Ϋ,String ȋ,bool ΒБ){IMyTerminalBlock b=(IMyTerminalBlock) Ϋ;if(ΒБ&&b.CustomName.ToUpper().Contains(ȋ.ToUpper())) return true;if(!ΒБ&&b.BlockDefinition.ToString().ToUpper().Contains(ȋ.ToUpper())) return true;return false;}void ǥ<T>(List<T>В,bool С)
{for(int i=0;i<В.Count;i++)
ΗΝ((IMyTerminalBlock) В[i],С);}void ΗΝ(IMyTerminalBlock Ϋ,bool С){if(Ϋ==null) return;String ΚЌ=С?"OnOff_On":"OnOff_Off";var a=Ϋ.GetActionWithName(ΚЌ);a.Apply(Ϋ);}void Ȓ(bool С){ΖΑ=С;if(!Ż) return;ǥ(Γγ,С);}void Δη(ChargeMode Д){for(int i=0;i<Ŀ.Count;i++) Ŀ[i].ChargeMode=Д;}void Δι(bool ΜΘ)
{for(int i=0;i<ǩ.Count;i++) ǩ[i].Stockpile=ΜΘ;}void ΑΙ(List<IMyLandingGear>ΕΣ,bool Θη){for(int i=0;i<ΕΣ.Count;i++){if(Θη) ΕΣ[i].Lock();if(!Θη) ΕΣ[i].Unlock();}}void ΗΞ(bool С){if(ΗЃ==С) return;List<IMyShipController>Βω=new List<IMyShipController>();χ.GetBlocksOfType(Βω,Ǣ);if(Βω.Count==0) return;
for(int i=0;i<Βω.Count;i++)
Βω[i].DampenersOverride=С;ΗЃ=С;}IMyShipConnector о(MyShipConnectorStatus Ǟ){for(int i=0;i<Ǔ.Count;i++){if(!Ȱ(Ǔ[i],true)) continue;if(Ǔ[i].Status==Ǟ) return Ǔ[i];}return null;}void ŝ(bool ΕЍ,bool ΒЃ,float Λέ,float ΛΑ){if(ύ==null||ώ.Count==0) return;Vector3 ƚ=new Vector3();int ą=0;
for(int i=0;i<ώ.Count;i++){if(ώ[i].WorldMatrix.Forward!=ǭ.WorldMatrix.Forward) continue;ą++;ƚ+=ώ[i].GetPosition();}ƚ=ƚ/ą;Vector3 Ʈ=ΒΜ(ύ,ƚ);ύ.Enabled=true;ύ.ShowOnHUD=ΕЍ;ύ.LeftExtend=(ΒЃ?1:љ)/2f*Ȫ-Ʈ.X;ύ.RightExtend=(ΒЃ?1:љ)/2f*Ȫ+Ʈ.X;ύ.TopExtend=(ΒЃ?1:ј)/2f*ȩ+Ʈ.Y;
ύ.BottomExtend=(ΒЃ?1:ј)/2f*ȩ-Ʈ.Y;ύ.FrontExtend=(ΕЍ?ф:Λέ)-Ʈ.Z;ύ.BackExtend=ΕЍ?0:ΛΑ+δ*0.75f+Ʈ.Z;ύ.DetectFloatingObjects=true;ύ.DetectAsteroids=false;ύ.DetectLargeShips=true;ύ.DetectSmallShips=true;ύ.DetectStations=true;ύ.DetectOwner=true;ύ.DetectSubgrids=false;
ύ.DetectPlayers=false;ύ.DetectEnemy=true;ύ.DetectFriendly=true;ύ.DetectNeutral=true;}void ś<T>(List<T>В,bool ΜΓ,float ΛΫ,bool Λη){for(int i=0;i<В.Count;i++){IMyTextSurface ΑЁ=null;if(В[i]is IMyTextSurface) ΑЁ=(IMyTextSurface) В[i];if(ΑЁ!=null){ΑЁ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΜΓ) continue;ΑЁ.Font="Debug";if(Λη) continue;ΑЁ.FontSize=ΛΫ;}}}bool ΒΠ(){Vector3D ά;return this.Ϊ.TryGetPlanetPosition(out ά);}int ΙІ(Vector3 ΜΚ,int ΘЊ){if(ΘЊ==-1) return 0;double Ы=-1;int ğ=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,ΜΚ);if(Ы==-1||υ<Ы){ğ=i;Ы=υ;}
}return Math.Sign(ğ-ΘЊ);}bool ΚΗ(Vector3 ά){List<Vector3>ж=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) ж.Add(Ψ[i].Ε);if(Ρ.ν&&Ψ.Count>=1){Vector3 Ɣ=new Vector3();ȯ(Ρ,dockDist*δ,false,out Ɣ);if(Vector3.Distance(Ρ.Ε,Ψ.First().Ε)<Vector3.Distance(Ρ.Ε,Ψ.Last().Ε)){ж.Insert(0,Ɣ);ж.Insert(0,Ρ.Ε);}
else{ж.Add(Ɣ);ж.Add(Ρ.Ε);}}if(Ξ==Ν.ί){if(Ζ.ν&&Ψ.Count>=1){Vector3 Δώ=new Vector3();ȯ(Ζ,dockDist*δ,false,out Δώ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){ж.Insert(0,Δώ);ж.Insert(0,Ζ.Ε);}else{ж.Add(Δώ);ж.Add(Ζ.Ε);}}}else{if(η!=Τ.ы)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
ж.Insert(0,Ζ.Ε);else ж.Add(Ζ.Ε);}int ğ=-1;double Ȗ=-1;for(int i=0;i<ж.Count;i++){double υ=Vector3.Distance(ж[i],ά);if(υ<Ȗ||Ȗ==-1){Ȗ=υ;ğ=i;}}if(ж.Count==0) return false;double ΕΘ=Vector3.Distance(ж[ğ],ά);double ΛЃ=Vector3.Distance(ж[Math.Max(0,ğ-1)],ж[ğ])*1.5f;
double Λϊ=Vector3.Distance(ж[Math.Min(ж.Count-1,ğ+1)],ж[ğ])*1.5f;return ΕΘ<ΛЃ||ΕΘ<Λϊ||ΕΘ<laneOffset*δ*1.5f;}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 Κέ(Vector3 v,int ΕΕ){return new Vector3(Math.Round(v.X,ΕΕ),Math.Round(v.Y,ΕΕ),Math.Round(v.Z,ΕΕ));
}float ƃ(Vector3 a,Vector3 b){if(a==b) return 0;float ΛΧ=(a*b).Sum;float Θα=a.Length();float Θβ=b.Length();if(Θα==0||Θβ==0) return 0;float α=(float)((180.0f/Math.PI)*Math.Acos(ΛΧ/(Θα*Θβ)));if(float.IsNaN(α)) return 0;return α;}float œ(Vector3 v,Vector3 Γά,float ΜΔ){float ƒ=ƃ(v,Γά);
if(ΜΔ>0f) ƒ*=-1;if(ƒ>-90f) return ƒ-90f;else return 180f-(-ƒ-90f);}double Ζρ(float ΛΞ){return(Math.PI/180)*ΛΞ;}bool ΔΗ(double Λσ,double value,double ƫ){return(value>=Λσ&&value<=ƫ);}Vector3 Κυ(IMyTerminalBlock Ϋ,Vector3 Т){return Vector3D.Transform(Т,Ϋ.WorldMatrix);
}Vector3 ΒΜ(IMyTerminalBlock Ϋ,Vector3 Μζ){return ъ(Ϋ,Μζ-Ϋ.GetPosition());}Vector3 ъ(IMyTerminalBlock Ϋ,Vector3 ΖΫ){return Vector3D.TransformNormal(ΖΫ,MatrixD.Transpose(Ϋ.WorldMatrix));}Vector3 Ŝ(Vector3 ȝ,Vector3 Ѕ,Vector3 ΖΫ){MatrixD Επ=MatrixD.CreateFromDir(ȝ,Ѕ);return Vector3D.TransformNormal(ΖΫ,MatrixD.Transpose(Επ));
}Vector3 Δσ(Vector3 ȝ,Vector3 Ѕ,Vector3 Т){MatrixD Επ=MatrixD.CreateFromDir(ȝ,Ѕ);return Vector3D.Transform(Т,Επ);}String π(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 κ(String s){String[]ω=s.Split('|');return new Vector3(float.Parse(Σ(ω,0)),float.Parse(Σ(ω,1)),float.Parse(Σ(ω,2)));
}String ΚЊ(έ ή){String Ц=":";String α=π(ή.Ε)+Ц+π(ή.μ)+Ц+
π(ή.λ)+Ц+π(ή.Є)+Ц+π(ή.ό);for(int i=0;i<ή.Ȍ.Length;i++)
α+=Ц+Math.Round(ή.Ȍ[i],3);return α;}έ Κξ(String s){String[]ω=s.Split(':');έ ή=new έ(
κ(Σ(ω,0)),κ(Σ(ω,1)),
κ(Σ(ω,2)),κ(Σ(ω,3)),
κ(Σ(ω,4)));int Β=5;List<float>ΗБ=new List<float>();while(Β<ω.Length){String Γο=Σ(ω,Β);float f=0;if(!float.TryParse(Γο,out f)) break;ΗБ.Add(f);Β++;}ή.Ȍ=ΗБ.ToArray();return ή;}System.Text.StringBuilder ƴ=new System.Text.StringBuilder();void Χ<T>(T value,bool Λω){if(Λω) ƴ.Append('\n');ƴ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΒΕ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Βη(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Σ(String[]ΚА,int Β){String Γο=ΚА.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Γο)) return "";
return Γο;}void Save(){if(Γή||Ξ==Ν.Ǒ){Storage="";return;}Storage=Γϋ();}String Γϋ(){ƴ.Clear();ƴ.Append(DATAREV+";");Χ(π(Γέ),false);Χ(π(Ρ.μ));Χ(π(Ρ.Є));Χ(π(Ρ.λ));Χ(π(Ρ.ό));Χ(π(Ρ.Ε));Χ(π(Ρ.х));Χ(Ρ.ν);Χ(π(Ζ.Ε));Χ(π(Ζ.ό));Χ(π(Ζ.μ));Χ(π(Ζ.λ));Χ(π(Ζ.Є));Χ(π(Ζ.х));Χ(Ζ.ν);Χ(π(Ф));Χ(π(ȡ));Χ(";");
Χ((int) Ξ,false);Χ((int) η);Χ((int) Ǚ);Χ(ȟ);Χ(ǚ);Χ(ц);Χ(Ǜ);Χ(ȓ);Χ(Ķ);Χ(ĵ);if(Ξ==Ν.ί){Χ((int) Ќ.Ч);Χ(Ќ.Ǿ);Χ(Ќ.ń);Χ(ΒΕ(Ќ.ǿ));Χ(ΒΕ(Ќ.Ȅ));Χ((int) Ѝ.Ч);Χ(Ѝ.Ǿ);Χ(Ѝ.ń);Χ(ΒΕ(Ѝ.ǿ));Χ(ΒΕ(Ѝ.Ȅ));}else{Χ((int) Ȥ);Χ((int) ѝ);Χ((int) л);Χ((int) ǔ);Χ((int) ū);Χ(ŏ);Χ(Ż);Χ(ƥ);Χ(Ɵ);Χ(љ);Χ(ј);Χ(ф);Χ(ǆ);Χ(ǅ);
Χ(ſ);Χ(ŧ);Χ(Б);Χ(Я);Χ(І);Χ(Ђ);Χ(ĭ);Χ(й);Χ(Ÿ);Χ(ů);}Χ(";");for(int i=0;i<đ.Count;i++)
Χ((i>0?"|":"")+đ[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΚЊ(Ψ[i]),i>0);Χ(";",false);Χ(к+"|"+Й+"|"+Ī+"|"+ĩ+"|"+Ū,false);Χ(";",false);Χ(Ιφ(),false);String Η=ƴ.ToString();ƴ.Clear();return Η;}ǒ Κά(){return Βά(Storage);}ǒ Βά(String Η){if(Η=="") return ǒ.ΚΠ;String[]Ź=Η.Split(';');if(Σ(Ź,0)!=DATAREV) return ǒ.ΔΦ;int Β=0;try
{String[]Ο=Σ(Ź,1).Split('\n');Γέ=κ(Σ(Ο,Β++));Ρ.μ=κ(Σ(Ο,Β++));Ρ.Є=κ(Σ(Ο,Β++));Ρ.λ=κ(Σ(Ο,Β++));Ρ.ό=κ(Σ(Ο,Β++));Ρ.Ε=κ(Σ(Ο,Β++));Ρ.х=κ(Σ(Ο,Β++));Ρ.ν=bool.Parse(Σ(Ο,Β++));Ζ.Ε=κ(Σ(Ο,Β++));Ζ.ό=κ(Σ(Ο,Β++));Ζ.μ=κ(Σ(Ο,Β++));Ζ.λ=κ(Σ(Ο,Β++));Ζ.Є=κ(Σ(Ο,Β++));Ζ.х=κ(Σ(Ο,Β++));
Ζ.ν=bool.Parse(Σ(Ο,Β++));Ф=κ(Σ(Ο,Β++));ȡ=κ(Σ(Ο,Β++));Ο=Σ(Ź,2).Split('\n');Β=0;Ξ=(Ν) int.Parse(Σ(Ο,Β++));η=(Τ) int.Parse(Σ(Ο,Β++));Ǚ=(Τ) int.Parse(Σ(Ο,Β++));ȟ=int.Parse(Σ(Ο,Β++));ǚ=int.Parse(Σ(Ο,Β++));ц=int.Parse(Σ(Ο,Β++));Ǜ=int.Parse(Σ(Ο,Β++));ȓ=float.Parse(Σ(Ο,Β++));
Ķ=bool.Parse(Σ(Ο,Β++));ĵ=bool.Parse(Σ(Ο,Β++));if(Ξ==Ν.ί){Ќ.Ч=(τ) int.Parse(Σ(Ο,Β++));Ќ.Ǿ=float.Parse(Σ(Ο,Β++));Ќ.ń=float.Parse(Σ(Ο,Β++));Ќ.ǿ=Βη(Σ(Ο,Β++));Ќ.Ȅ=Βη(Σ(Ο,Β++));Ѝ.Ч=(τ) int.Parse(Σ(Ο,Β++));Ѝ.Ǿ=float.Parse(Σ(Ο,Β++));Ѝ.ń=float.Parse(Σ(Ο,Β++));Ѝ.ǿ=Βη(Σ(Ο,Β++));
Ѝ.Ȅ=Βη(Σ(Ο,Β++));}else{Ȥ=(і) int.Parse(Σ(Ο,Β++));ѝ=(ѓ) int.Parse(Σ(Ο,Β++));л=(З) int.Parse(Σ(Ο,Β++));ǔ=(ǡ) int.Parse(Σ(Ο,Β++));ū=(і) int.Parse(Σ(Ο,Β++));ŏ=bool.Parse(Σ(Ο,Β++));Ż=bool.Parse(Σ(Ο,Β++));ƥ=bool.Parse(Σ(Ο,Β++));Ɵ=bool.Parse(Σ(Ο,Β++));љ=int.Parse(Σ(Ο,Β++));ј=int.Parse(Σ(Ο,Β++));
ф=int.Parse(Σ(Ο,Β++));ǆ=float.Parse(Σ(Ο,Β++));ǅ=float.Parse(Σ(Ο,Β++));ſ=float.Parse(Σ(Ο,Β++));ŧ=float.Parse(Σ(Ο,Β++));Б=int.Parse(Σ(Ο,Β++));Я=int.Parse(Σ(Ο,Β++));І=int.Parse(Σ(Ο,Β++));Ђ=int.Parse(Σ(Ο,Β++));ĭ=int.Parse(Σ(Ο,Β++));й=int.Parse(Σ(Ο,Β++));Ÿ=int.Parse(Σ(Ο,Β++));
ů=float.Parse(Σ(Ο,Β++));}Ο=Σ(Ź,3).Replace("\n","").Split('|');đ=Ο.ToList();Ο=Σ(Ź,4).Split('\n');Ψ.Clear();if(Ο.Count()>=1&&Ο[0]!="")
for(int i=0;i<Ο.Length;i++)
Ψ.Add(Κξ(Σ(Ο,i)));Ο=Σ(Ź,5).Split('|');к=Σ(Ο,0);if(!int.TryParse(Σ(Ο,1),out Й)) Й=-1;if(!float.TryParse(Σ(Ο,2),out Ī)) Ī=0;if(!float.TryParse(Σ(Ο,3),out ĩ)) ĩ=0;if(!int.TryParse(Σ(Ο,4),out Ū)) Ū=0;if(к.Length!=Б*Я){к="";Й=-1;}Ιυ(Σ(Ź,6));}catch{return ǒ.Ζτ;}Ƶ=Ǚ;
Ї();return ǒ.Βγ;}const int Ζη=64000;IMyTerminalBlock Ζυ(){List<IMyTerminalBlock>Ɨ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ɨ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));return Ɨ.Count>0?Ɨ[0]:null;}void Ιϊ(){IMyTerminalBlock Ϋ=Ζυ();if(Ϋ==null){Λ="Export: name a block "+dataTag;return;}
String Η="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Γϋ();if(Η.Length>Ζη){Λ="Export too large: "+Η.Length+" chars, max "+Ζη;return;}Ϋ.CustomData=Η;Λ="Exported "+Ψ.Count+" waypoints";}void ΚΓ(bool Εγ){IMyTerminalBlock Ϋ=Ζυ();if(Ϋ==null){Λ="Import: name a block "+dataTag;return;}
ΚΔ(Ϋ.CustomData,Εγ);}void ΚΔ(String Η,bool Εγ){Η=Η.Replace("\r","");if(Η.StartsWith("//")){int Θυ=Η.IndexOf('\n');Η=Θυ<0?"":Η.Substring(Θυ+1);}Η=Η.Trim();if(Η==""){Λ="Import: no PAM data in "+dataTag;return;}Ї();String Ηλ=Γϋ();ǒ α=Βά(Η);if(α==ǒ.Βγ&&Εγ){έ Ι=new έ(Ζ);
Vector3 Λφ=Ф,Λυ=ȡ;int w=љ,h=ј,d=ф;і ΑϏ=Ȥ;ǡ ΕΖ=ǔ;float Λύ=ſ,Λό=ŧ,Λλ=Ī,Λκ=ĩ;int ΜΒ=Ū;Βά(Ηλ);Ζ=Ι;Ф=Λφ;ȡ=Λυ;љ=w;ј=h;ф=d;Ȥ=ΑϏ;ǔ=ΕΖ;ſ=Λύ;ŧ=Λό;η=Τ.ƍ;ΑΞ(true,true);Ī=Λλ;ĩ=Λκ;Ū=ΜΒ;η=Τ.ƍ;Ǚ=Τ.ƍ;Λ="Imported job, run CONT";return;}if(α==ǒ.Βγ)
{Αυ=false;Λ="Imported "+Ψ.Count+" waypoints";return;}Βά(Ηλ);Λ=α==ǒ.ΔΦ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗΟ(){Ť.Clear();for(int i=а.Count-1;i>=0;i--){String Η=а[i].CustomData.ToUpper();bool Εα=false;if(Η=="INSTRUCTIONS"){Εα=true;ΒЁ=true;}
if(Η=="DEBUG") Εα=true;if(Εα){Ť.Add(а[i]);а.RemoveAt(i);}}ś(Ť,false,1,false);}void ΗΠ(List<IMyTerminalBlock>Ɨ){ȥ.Clear();for(int i=0;i<Ɨ.Count;i++){IMyTerminalBlock Ϋ=Ɨ[i];try{String Θύ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϋ.CustomName.IndexOf(Θύ);int ƻ=-1;if(Β<0||!int.TryParse(Ϋ.CustomName.Substring(Β+Θύ.Length,1),out ƻ)) continue;
if(ƻ==-1) continue;ƻ--;IMyTextSurfaceProvider ΘϏ=(IMyTextSurfaceProvider) Ϋ;if(ƻ<ΘϏ.SurfaceCount&&ƻ>=0)
ȥ.Add(ΘϏ.GetSurface(ƻ));}catch{}}}String ΓΡ="";void ΗΚ(){String Εο="";String Αΰ="";ΓΡ=Ζζ(false);Εο+=ΓΡ;Αΰ+=ΓΡ;Αΰ+=ΙЃ();for(int i=0;i<а.Count;i++) а[i].WriteText(Εο);for(int i=0;i<ȥ.Count;i++) ȥ[i].WriteText(Εο);Echo(Αΰ);for(int i=0;i<Ť.Count;i++){IMyTextPanel я=Ť[i];
String Η=я.CustomData.ToUpper();if(Η=="DEBUG") я.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") я.WriteText(Ζό());}}string Ζό(){String Γ="";try{float Θι=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ɩ*Θι)+" / "+(int)(Ƭ*Θι)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ɩ*100f,1)+"% / "+Math.Round(Ƭ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ΑΤ,2)+"ms / "+Math.Round(Εσ,2)+"ms \n";}catch{}for(int i=0;i<į.Count;i++)
Γ+=""+į.Keys.ElementAt(i)+" = "+Math.Round(į.Values.ElementAt(i)[0],2)+" / "+
Math.Round(į.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙЎ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ś(а,setLCDFontAndSize,0.9f,false);ś(ȥ,setLCDFontAndSize,0.9f,true);for(int i=0;i<а.Count;i++) а[i].WriteText(Γ+Ĭ.ToString());for(int i=0;i<ȥ.Count;i++) ȥ[i].WriteText(Γ+Ĭ.ToString());}void ȳ(){if(!ΒЁ) return;try{ΕЁ=Runtime.CurrentInstructionCount;}catch{}
}void ȭ(String ΰ){if(!ΒЁ) return;if(ΕЁ==0) return;try{float Γν=(Runtime.CurrentInstructionCount-ΕЁ)/Runtime.MaxInstructionCount*100;if(!į.ContainsKey(ΰ))
į.Add(ΰ,new float[]{Γν,Γν});else{į[ΰ][0]=Γν;į[ΰ][1]=Math.Max(Γν,į[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ιά(String Ȝ){if(!Ƙ) return;String Ц=""+BROADCAST_SEP;String ε=VERSION+Ц;ε+=Ǉ(Ƹ)+Ц;
ε+=(int) Ξ+Ц;ε+=Ɔ(β.X)+""+Ц;ε+=Ɔ(β.Y)+Ц;ε+=Ɔ(β.Z)+Ц;if(Ξ!=Ν.ί)
ε+=ΓЍ(Ǚ)+(Ǚ==Τ.Л?" "+Math.Round(Αξ,1)+"%":"")+Ц;else ε+=ΓЍ(Ǚ)+Ц;ε+=Ǉ(Λ)+Ц;ε+=Г+""+Ц;ε+=Ǉ(ΓΡ)+Ц;ε+=Ș+Ц;ε+=Ƞ+Ц;for(int i=0;i<Ȃ.Count;i++){if(Ȃ[i].Ʃ!=б.ƈ) continue;ε+=Ǉ(Ȃ[i].ΰ)+"/"+Ǉ(Ȃ[i].ϊ)+"/"+Ȃ[i].Џ+Ц;}Κη(ε,Ȝ);}String Ǉ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Κη(String ε,String Ȝ){Βί(ε,BROADCAST_SELF_ID,Ȝ);}void Βί(String ε,String ζ,String Ȝ){try{if(!Ƙ) return;ε=BROADCAST_TAG+BROADCAST_SEP+Ζψ()+BROADCAST_SEP+ζ+BROADCAST_SEP+ΔЋ+BROADCAST_SEP+Ȝ+BROADCAST_SEP+ε;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ε);}catch(Exception e){Ĭ=e;}
}String Ζψ(){if(Ξ!=Ν.Ǒ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔΜ(String ε){return ε.StartsWith(BROADCAST_TAG);}bool н(ref String Η,out string Αδ,Char Ц){int Β=Η.IndexOf(Ц);Αδ="";if(Β<0) return false;Αδ=Η.Substring(0,Β);Η=Η.Remove(0,Β+1);return true;}bool ΗΙ(ref String ε,out String ȣ,out String Ȝ)
{ȣ="";Ȝ="";if(!Ƙ) return false;String ȋ="";if(!н(ref ε,out ȋ,BROADCAST_SEP)||!ΔΜ(ȋ)) return false;if(!н(ref ε,out ȣ,BROADCAST_SEP)) return false;if(!н(ref ε,out ȋ,BROADCAST_SEP)||(ȋ!=Ζψ()&&(ȋ!="*"&&Ξ!=Ν.Ǒ))) return false;if(!н(ref ε,out ȋ,BROADCAST_SEP)||(ȋ!=ΔЋ)) return false;
if(!н(ref ε,out Ȝ,BROADCAST_SEP)) return false;return true;}void ΖЉ(){bool ΑΩ=true;if(ΘΒ){ΘΒ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Αι=(Ξ!=Ν.Ǒ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Αι)
ΖЊ(Αι);String[]Ο=Me.CustomData.Split('\n');Ƙ=bool.Parse(ΒΫ(ΒΗ(Ο,"Enable_Broadcast",ref ΑΩ)));bool ΖΩ=false;bool ΗЇ=true;if(Ƙ){if(Ξ!=Ν.Ǒ)
Ƹ=ΒΫ(ΒΗ(Ο,"Ship_Name",ref ΑΩ)).Replace(BROADCAST_SELF_ID,"");ΔЋ=ΒΫ(ΒΗ(Ο,"Broadcast_Channel",ref ΑΩ)).ToLower();ΗЇ=false;if(Ǽ==null){Ǽ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǽ.SetMessageCallback("");}List<IMyRadioAntenna>Ɠ=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ɠ);
bool Δύ=false;for(int i=0;i<Ɠ.Count;i++)
if(Ɠ[i].EnableBroadcasting&&Ɠ[i].Enabled){Δύ=true;break;}if(Ɠ.Count==0) Λ="No Antenna found";else if(!Δύ) Λ="Antenna not ready";ΖΩ=Ɠ.Count==0||!Δύ;if(Ηδ&&!ΖΩ&&Ξ!=Ν.Ǒ) Λ="Antenna ok";}else if(Ξ==Ν.Ǒ)
Λ="Offline - Enable in PB custom data";Ηδ=ΖΩ;if(ΗЇ){if(Ǽ!=null) this.IGC.DisableBroadcastListener(Ǽ);Ǽ=null;}}catch{ΑΩ=false;}if(!ΑΩ){Λ="Reset custom data";ΖЊ(Αι);}}void ΖЊ(String Αι){String Η=Αι+"\n\n"+"Enable_Broadcast="+(Ξ==Ν.Ǒ?"true":"false")+" \n";Η+=Ξ!=Ν.Ǒ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Η+"Broadcast_Channel=#default";}String ΒΫ(String Ů){int Β=Ů.IndexOf("//");if(Β!=-1) Ů=Ů.Substring(0,Β);String[]ω=Ů.Split('=');if(ω.Length<=1) return "";return ω[1].Trim();}String ΒΗ(String[]Ο,String э,ref bool Č){foreach(String Ů in Ο)
if(Ů.StartsWith(э)) return Ů;Č=false;return "";}void Ιρ(string Ъ){if(Ģ) Ϗ=new List<ϋ>();String ȣ="";String Ȝ="";if(!ΔΜ(Ъ)) ΚΩ(Ъ);bool ΘІ=false;int Εφ=50;while(Ǽ!=null&&Ǽ.HasPendingMessage&&Εφ-->0){MyIGCMessage ε=Ǽ.AcceptMessage();String Η=ε.Data as String;if(Η==null) continue;
if(ΗΙ(ref Η,out ȣ,out Ȝ)&&ȣ!=""&&ȣ!=BROADCAST_SELF_ID){ϋ Δ=ΓЊ(ȣ,"");if(Δ==null){Δ=new ϋ(ȣ);Ϗ.Add(Δ);}Δ.ļ(Ȝ,false,false,true);ΚЃ(Δ,Η);ΘІ=true;}}if(ΘІ)
Ϗ.Sort(delegate(ϋ a,ϋ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ħ||Ģ){if(Αϋ<=0||Ģ){Λ="";Ģ=false;Κε();ΖЉ();Ει=DateTime.Now;}}if(Ĩ) Κϋ();if(Ĩ) ΚϏ();if(Ĩ) ΚЈ();if(Ĩ) ΚЄ();if(Ĩ&&ΖΗ==0) Κω();
Κψ();}void ΚЃ(ϋ Δ,String Η){Δ.Εζ=DateTime.Now;String Αδ="";н(ref Η,out Δ.Γρ,BROADCAST_SEP);н(ref Η,out Δ.ΰ,BROADCAST_SEP);if(Δ.Γρ!=VERSION) return;String Ατ="";String[]Ʋ=new string[3];н(ref Η,out Ατ,BROADCAST_SEP);н(ref Η,out Ʋ[0],BROADCAST_SEP);н(ref Η,out Ʋ[1],BROADCAST_SEP);
н(ref Η,out Ʋ[2],BROADCAST_SEP);н(ref Η,out Δ.ΖΒ,BROADCAST_SEP);н(ref Η,out Δ.Λ,BROADCAST_SEP);н(ref Η,out Αδ,BROADCAST_SEP);н(ref Η,out Δ.ΓΣ,BROADCAST_SEP);String ΙΣ="",Θμ="";н(ref Η,out ΙΣ,BROADCAST_SEP);н(ref Η,out Θμ,BROADCAST_SEP);Δ.А.Clear();while(true){String ΘΫ;if(!н(ref Η,out ΘΫ,BROADCAST_SEP)) break;
String[]ΓΖ=ΘΫ.Split('/');if(ΓΖ.Count()<3) continue;int Џ=0;if(!int.TryParse(ΓΖ[2],out Џ)) continue;Δ.А.Add(new у(ΓΖ[0],ΓΖ[1],Џ,б.ƈ));}int Θπ=0;if(!int.TryParse(Αδ,out Δ.Г)) Δ.Г=0;if(!int.TryParse(Ατ,out Θπ)) Δ.Ξ=Ν.Ľ;if(!float.TryParse(Ʋ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ʋ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ʋ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(ΙΣ,out Δ.Ș)) Δ.Ș=0;if(!float.TryParse(Θμ,out Δ.Ƞ)) Δ.Ƞ=0;Δ.Ξ=(Ν) Θπ;Δ.ΕΙ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΖΠ=0;for(int i=0;i<Δ.А.Count();i++) Δ.ΖΠ+=Δ.А[i].Џ;}ϋ ΓЊ(String Θ,String ΰ){Θ=Θ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Ϗ.Count;i++){if(Θ!=""&&Ϗ[i].Θ.ToUpper()==Θ) return Ϗ[i];if(ΰ!=""&&Ϗ[i].ΰ!=null&&Ϗ[i].ΰ.ToUpper()==ΰ) return Ϗ[i];}return null;}String ƛ="";ϋ Ηυ=null;DateTime Ηφ=DateTime.MinValue;List<ϋ>Ġ=new List<ϋ>();const float Ια=20f;bool ΗΣ(ϋ ζ,String Ј){return Ј==ƛ&&ζ==Ηυ&&((DateTime.Now-Ηφ).TotalSeconds<Ια||
(Ј=="HOMEPOS"&&(ȁ.Count>0||ĉ!=null)));}String Ιό(ϋ ζ){if(ƛ==""||!ΗΣ(ζ,ƛ)) return "";String s="";int Č=0,ΕЎ=0;for(int i=0;i<Ġ.Count;i++){Ў Γε=Ġ[i].ļ(ƛ,false,false,false);if(Γε==Ў.ΑΗ) Č++;bool Αψ=ȁ.Contains(Ġ[i]);if(ΕЎ++<8) s+="  "+Ġ[i].ΰ+": "+(Αψ?"waiting to leave ("+(ȁ.IndexOf(Ġ[i])+1)+")":Γε==Ў.ΑΗ?"received":
Γε==Ў.Ǝ?"pending...":Γε==Ў.ΒΨ?"no answer!":"-")+"\n";}if(ΕЎ>8) s+="  +"+(ΕЎ-8)+" more\n";return "--------------------------------------------\n"+ƛ+": "+Č+"/"+Ġ.Count+" received\n"+s;}void ǹ(ϋ Δ,String Ј){ƛ=Ј;Ηυ=Δ;Ηφ=DateTime.Now;Ġ.Clear();if(Δ==null) Ġ.AddRange(Ϗ);else Ġ.Add(Δ);
if(Δ==null){ȁ.Clear();ĉ=null;}else ȁ.Remove(Δ);if(Δ==null&&Ј=="HOMEPOS"){Κν();return;}if(Δ==null){for(int i=0;i<Ϗ.Count;i++)
Ϗ[i].ļ(Ј,true,false,false);Βί(Ј,"*",Ј);}else{Δ.ļ(Ј,true,false,false);Βί(Ј,Δ.Θ,Ј);}}void ΚΩ(string Ъ){if(Ъ=="") return;var ω=Ъ.ToUpper().Split(' ');ω.DefaultIfEmpty("");var Ј=ω.ElementAtOrDefault(0);var ā=ω.ElementAtOrDefault(1);String Ċ="Invalid argument: "+Ъ;if(Ё==Φ.ΑΚ){if(П!=null)
{switch(Ј){case "UP":{if(Г<2) break;if(П.Г==0){Г=1;return;}ǹ(П,"UP");return;}case "DOWN":{if(Г<1) break;if(Г==1){ǹ(П,"MRES");break;}ǹ(П,"DOWN");break;}case "APPLY":{if(Г<2) break;ǹ(П,"APPLY");return;}}}}switch(Ј){case "UP":this.ΔΡ(false);return;case "DOWN":this.ΔΠ(false);return;
case "APPLY":this.Ƅ(true);return;}switch(Ј){case "CLEAR":Ιλ();return;case "SENDALL":
if(String.IsNullOrEmpty(ā)){Λ="Usage: SENDALL <cmd>";return;}ǹ(null,ā);return;case "SEND":
if(Ъ.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Κθ(Ъ.Remove(0,"SEND".Length+1));return;}Λ=Ċ;}void Κθ(String ΑΡ){if(ΑΡ=="") return;var ω=ΑΡ.Split(':');if(ω.Length!=2){Λ="Missing separator \":\"";return;}ω.DefaultIfEmpty("");String Ƹ=ω.ElementAtOrDefault(0).Trim();
ϋ Δ=ΓЊ("",Ƹ);if(Δ!=null) ǹ(Δ,ω.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ƹ;}void Ιλ(){Ϗ.Clear();П=null;ο(Φ.Main);Г=0;}void Κε(){List<IMyTerminalBlock>ΑЂ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(а,Ǣ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑЂ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);а=Ƕ(а,pamTag,true);ΗΠ(ΑЂ);ΗΟ();Κζ();ś(а,setLCDFontAndSize,1.15f,false);ś(ȥ,setLCDFontAndSize,1.15f,true);String Λγ=Ζύ(false);String Θφ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock я in а){if(я.CustomData==""){я.CustomData=Ζύ(true);continue;}
if(!я.CustomData.Contains(Λγ))
if(!я.CustomData.Contains(Θφ)) я.CustomData=Θφ+я.CustomData;}}void Κψ(){String Αΰ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȥ.Count;i++)
ȥ[i].WriteText(Ζδ(Ш.ΗΒ,"0",false));for(int i=0;i<а.Count;i++){Ш Д=Ш.Ľ;String Ȉ="";ΚΣ(а[i],out Д,out Ȉ);а[i].WriteText(Ζδ(Д,Ȉ,i==0));}Echo(Αΰ+Ιώ());for(int i=0;i<Ť.Count;i++){IMyTextPanel я=Ť[i];String Η=я.CustomData.ToUpper();if(Η=="DEBUG") я.WriteText(""+"\n"+"");
if(Η=="INSTRUCTIONS") я.WriteText(Ζό());}}String Ƅ(bool Μ){int ΛΨ=0;return Ƅ(Μ,0,ref ΛΨ,false,1);}String Ƅ(bool Μ,int ΛЀ,ref int ΛЏ,bool Ηω,int Αφ){String Γ="";String Ʒ="——————————————————\n";String ΜΞ="--------------------------------------------\n";if(П==null||Ё==Φ.Main||Ё==Φ.Δζ)
Γ+="[PAM]-Controller | "+Ϗ.Count+" Connected ships"+"\n"+ΙЀ()+ΙЅ()+ΙЊ();else Γ+=ΖЀ(П)+"\n";Γ+=Ʒ;int Β=0;int Κ=Г;Ű=0;if(Λ!=""){Π(ref Γ,Κ,Β++,Μ,Λ);}else if(Ё==Φ.Main){if(П==null&&Ϗ.Count>=1) П=Ϗ[0];String В="";if(Π(ref В,Κ,Β++,Μ," Send command to all")) ο(Φ.Δζ);
int ƶ=0;for(int s=0;s<Ϗ.Count;s++){if(Ηω) В+="\n";ϋ Δ=Ϗ[s];if(Κ==Β||Κ==Β+1) П=Δ;if(Β==Κ) ƶ=В.Split('\n').Length-1;if(Π(ref В,Κ,Β++,Μ," "+ΖЀ(Δ))){П=Δ;ο(Φ.ΑΚ);П.ļ("",false,true,false);}if(Β==Κ) ƶ=В.Split('\n').Length-1;if(Π(ref В,Κ,Β++,Μ," "+(Ηω?"":"  ")+ΙЇ(Δ)))
{П=Δ;ο(Φ.ΗΡ);}}int ǃ=Αφ-2;ǃ+=Math.Max(0,(ΛЀ-1))*Αφ;Γ+=Δε(ǃ,В,ƶ,ref ΛЏ);}else if(Ё==Φ.Βΰ||Ё==Φ.Δζ){Φ ΚБ=Ё==Φ.Βΰ?Φ.ΑΚ:Φ.Main;ϋ ζ=null;if(Ё==Φ.Βΰ) ζ=П;if(Π(ref Γ,Κ,Β++,Μ," Back")){ο(ΚБ);}if(Π(ref Γ,Κ,Β++,Μ," [Stop] "+Ƈ(ζ,"STOP"))){ǹ(ζ,"STOP");}
if(Π(ref Γ,Κ,Β++,Μ," [Continue] "+Ƈ(ζ,"CONT"))){ǹ(ζ,"CONT");}if(Π(ref Γ,Κ,Β++,Μ," [Move home] "+Ƈ(ζ,"HOMEPOS"))){ǹ(ζ,"HOMEPOS");}if(Π(ref Γ,Κ,Β++,Μ," [Move to job] "+Ƈ(ζ,"JOBPOS"))){ǹ(ζ,"JOBPOS");}if(Π(ref Γ,Κ,Β++,Μ," [Full simulation] "+Ƈ(ζ,"FULL"))){ǹ(ζ,"FULL");}
if(ζ!=null&&ζ.Ξ==Ν.ί)
if(Π(ref Γ,Κ,Β++,Μ," [Undock] "+Ƈ(ζ,"UNDOCK"))){ǹ(ζ,"UNDOCK");}Γ+=Ιό(ζ);}else if(Ё==Φ.ΑΚ){String ΔЄ="";if(Π(ref ΔЄ,Κ,Β++,Μ," Back")){ο(Φ.Main);}Γ+=ΔЄ.Substring(0,ΔЄ.Length-1).PadRight(36,' ');if(Π(ref Γ,Κ,Β++,Μ," Send cmd")){ο(Φ.Βΰ);}if(!П.ΔΝ()&&!П.ΔΙ()) Ű++;
if(П.ΔΙ()) Γ+="No answer received...";else Γ+=Ɛ(П,П.ΓΣ);}else if(Ё==Φ.ΗΡ){if(Π(ref Γ,Κ,Β++,Μ," Back")){ο(Φ.Main);}Γ+=ΜΞ;Γ+=Ɛ(П,ΖϏ(П));}if(!Ευ) Г=Math.Min(Ű-1,Г);return Γ;}String ΖЀ(ϋ Δ){if(Δ.Γρ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ɛ(Δ,Δ.ΖΒ)+" "+Δ.ΕΙ+"m";
}String ΙЇ(ϋ Δ){String s=Δβ("",М(Δ.Ș,Δ.Ƞ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.А.Count;i++){if(i>=5) break;у Ѓ=Δ.А[i];s+=Ηή(Γό(ΙЄ(Ѓ)),3)+" "+Ζφ(Ѓ.Џ)+" ";}return s;}String ΙЄ(у Ѓ){if(Ѓ.ϊ.ToUpper()=="ORE"||Ѓ.ϊ.ToUpper()=="INGOT"){String Ηχ=ΙЂ(Ѓ.ΰ.ToUpper());if(Ηχ!="") return Ηχ;
}return Ѓ.ΰ;}String Ɛ(ϋ Δ,String Γ){if(Δ.ΔΝ()) return "No signal...("+ΒΚ((int)(DateTime.Now-Δ.Εζ).TotalSeconds)+")";return Γ;}String Ζύ(bool Με){String Γ="";if(Με) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΚΣ(IMyTerminalBlock я,out Ш Д,out String Ȉ){bool Č=true;String Ατ=ΒΫ(ΒΗ(я.CustomData.Split('\n'),"mode",ref Č)).ToUpper();String[]ω=Ατ.Split(':');String Ασ=ω.Length>0?ω[0].Trim():"";Ȉ=ω.Length>1?ω[1].Trim():"";
Д=Ш.Ľ;if(Ασ=="MAIN") Д=Ш.ΗΓ;else if(Ασ=="MAINX") Д=Ш.ΗΔ;else if(Ασ=="MENU") Д=Ш.ΔΟ;else if(Ασ=="INVENTORY") Д=Ш.ΔΘ;else if(Ασ=="DEBUG") Д=Ш.ΓЀ;if(Д==Ш.Ľ) Ȉ=Ατ;}String Ζδ(Ш Д,String Ȉ,bool ΛΪ){int Απ=15;if(ΛΪ){Ηϋ=Βχ;Βχ=0;Ηό=Βψ;Βψ=0;}String Γ="";if(Д==Ш.ΗΓ)
{int ю=0;Γ=Ƅ(false,Ηϋ,ref ΛΙ,true,Απ);if(!int.TryParse(Ȉ,out ю)) return Γ;Βχ=Math.Max(ю,Βχ);ю--;return ΗΘ(ю,Απ,Γ);}else if(Д==Ш.ΗΔ){int ю=0;Γ=Ƅ(false,Ηό,ref ΛΚ,false,Απ);if(!int.TryParse(Ȉ,out ю)) return Γ;Βψ=Math.Max(ю,Βψ);ю--;return ΗΘ(ю,Απ,Γ);}if(Д==Ш.ΗΒ){return Ƅ(false,0,ref ΛΛ,true,Απ);
}else if(Д==Ш.ΔΘ||Д==Ш.ΔΟ||Д==Ш.ΓЀ){ϋ Δ=П;if(Ȉ!=""){Δ=ΓЊ("",Ȉ);if(Δ==null) return "Unknown ship: "+Ȉ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ʒ="—————————————————————\n";String ΖΝ="";String ΔЊ="";if(Д==Ш.ΔΟ){ΖΝ="Menu";ΔЊ=Ɛ(Δ,Δ.ΓΣ);}
else if(Д==Ш.ΔΘ){ΖΝ="Inventory";ΔЊ=Ɛ(Δ,ΖϏ(Δ));}else if(Д==Ш.ΓЀ){return Ɛ(Δ,ΙЈ(Δ));}Γ+=Δ.ΰ+" - "+Δ.ΕΙ+"m | "+Γό(""+ΖΝ)+"\n"+Ʒ;Γ+=ΔЊ;return Γ;}return "Unknown command: "+Ȉ;}string ΙЈ(ϋ Δ){String s="";s+=Δ.Γρ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΖΒ+"\n";s+=Δ.Λ+"\n";s+=Δ.Г+"\n";s+=Δ.ΓΣ.Length+"\n";s+=Δ.Ș+"\n";s+=Δ.Ƞ+"\n";s+=Δ.А.Count()+"\n";return s;}string ΖϏ(ϋ Δ){String s="";s+=Δβ("All",М(Δ.Ș,Δ.Ƞ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.А.Count();i++){у Ѓ=Δ.А[i];
s+=Δβ(Ѓ.ΰ,Ѓ.Џ,Δ.ΖΠ,30,8,10)+"\n";}return s;}Ў Ιύ(ϋ Δ,bool и,string Ј){Ў α=Ў.None;if(Δ==null){for(int i=0;i<Ϗ.Count;i++){Ў Љ=Ϗ[i].ļ(Ј,false,и,false);if(Љ==Ў.ΑΗ) α=Љ;if(Љ==Ў.ΒΨ) return Љ;if(Љ==Ў.Ǝ) return Љ;}return α;}else return Δ.ļ(Ј,false,и,false);}String Ƈ(ϋ Δ,String Ј)
{if(!ΗΣ(Δ,Ј)) return "";Ў Љ=Ιύ(Δ,false,Ј);if(Љ==Ў.ΑΗ) return "received!";if(Љ==Ў.Ǝ) return "pending...";if(Љ==Ў.ΒΨ) return "no answer!";return "";}String Δε(int ǃ,String Γ,int ƶ,ref int İ){String[]Ο=Γ.Split('\n');if(ƶ>=İ+ǃ-1) İ++;İ=Math.Min(Ο.Count()-1-ǃ,İ);if(ƶ<İ+1) İ--;
İ=Math.Max(0,İ);String α="";for(int i=0;i<ǃ;i++){int Θδ=i+İ;if(Θδ>=Ο.Count()) break;α+=Ο[Θδ]+"\n";}return α;}String ΗΘ(int ю,int Αφ,String Γ){String[]Ο=Γ.Split('\n');int ΑϏ=ю*Αφ;int Ț=(ю+1)*Αφ;String α="";for(int i=ΑϏ;i<Ț;i++){if(i>=Ο.Count()) break;α+=Ο[i]+"\n";
}return α;}String Γό(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ηή(String s,int Θκ){if(s=="") return s;if(s.Length>Θκ) s=s.Substring(0,Θκ-1)+".";return s;}string Δβ(String Ŭ,float value,float ƫ,int Ημ,int Θί,int ΛϏ){float ŷ=М(value,ƫ)*Ημ;
String Βρ="[";for(int i=0;i<Ημ;i++){if(i<=ŷ) Βρ+="|";else Βρ+="'";}Βρ+="]";return Βρ+" "+Ηή(Γό(Ŭ),Θί).PadRight(Θί)+"".PadRight(ΛϏ)+Ζφ(value);}String Ζφ(float Џ){if(Џ>=1000000) return Math.Round(Џ/1000000f,Џ/1000000f<100?1:0)+"M";if(Џ>=1000) return Math.Round(Џ/1000f,Џ/1000f<100?1:0)+"K";
return ""+Math.Round(Џ);}String ΒΚ(int Αό){if(Αό>=60*60) return Math.Round(Αό/(60f*60f),1)+" h";if(Αό>=60) return Math.Round(Αό/60f,1)+" min";return ""+Αό+" s";}string Ɔ(float v){return Math.Round(v,2)+" ";}string Μμ(Vector3 v){return "X"+Ɔ(v.X)+"Y"+Ɔ(v.Y)+"Z"+Ɔ(v.Z);}
List<IMyCameraBlock>ȕ=new List<IMyCameraBlock>();List<long>Ư=new List<long>();bool ŋ=false;int ΔА=0,ΛЌ=0,ΔЈ=0;const int Κί=16;int ΔЎ=0;DateTime ΔЉ=DateTime.Now;String Βς="";String ΔЍ="";Vector3 Ɯ=new Vector3();int Ɲ=0;void Κδ(){ȕ.Clear();Ư.Clear();if(!obstacleBrake) return;
χ.GetBlocksOfType(ȕ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<ȕ.Count;i++) ȕ[i].EnableRaycast=true;List<IMyTerminalBlock>Βμ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Βμ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<Βμ.Count;i++) if(!Ư.Contains(Βμ[i].CubeGrid.EntityId)) Ư.Add(Βμ[i].CubeGrid.EntityId);
if(!Ư.Contains(Me.CubeGrid.EntityId)) Ư.Add(Me.CubeGrid.EntityId);ΔЍ="";if(Ϊ==null) return;MatrixD m=Ϊ.WorldMatrix;Vector3[]ΛΣ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Λχ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<ȕ.Count;i++)
if(Vector3.Dot(ȕ[i].WorldMatrix.Forward,ΛΣ[d])>0.7f){ΔЍ+=Λχ[d]+" ";break;}}bool ΖЍ(Α s){return s==Α.Ƿ||s==Α.т||s==Α.И||
s==Α.Ȯ||s==Α.ƅ||s==Α.ΒΘ||
(s==Α.ǵ&&Ǭ!=null);}void ǈ(){ŋ=false;ΔА=0;Βς="";}void ΚЁ(){bool Βο=Ω==Α.ǵ;if(!Βο){Ɯ=new Vector3();Ɲ=0;}if(!obstacleBrake||ȕ.Count==0||!(ΖЍ(Ω)||Βο)){ǈ();return;}Vector3 ΒЎ=ψ+Ĵ+Ɯ;Vector3 ȧ=ΒЎ-β;float ǟ=ȧ.Length();if(ǟ<1f){ǈ();return;}Vector3 ρ=ȧ/ǟ;
Vector3 з=Ϊ!=null?(Vector3) Ϊ.GetShipVelocities().LinearVelocity:new Vector3();if(з.Length()>2) ρ=Vector3.Normalize(з);Vector3 ƹ=Vector3.Cross(ρ,Math.Abs(ρ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ƹ.Normalize();Vector3 Ѕ=Vector3.Cross(ƹ,ρ);float υ=Math.Min(ǟ+δ*0.25f,Đ*obstacleLookAhead+δ);
int Ης=0;for(int k=0;k<2;k++){int p=k==0?0:ΛЌ++%8,e=p%4;Vector3 ΓΧ=k==0?new Vector3():(e==0?ƹ:e==1?-ƹ:e==2?Ѕ:-Ѕ)*δ*0.5f;Vector3 Ț=β+ρ*(p>=4?υ*0.5f:υ)+ΓΧ;IMyCameraBlock Ğ=Ιε(Ț);if(Ğ==null){Ζξ();continue;}Ης++;MyDetectedEntityInfo Ǖ=Ğ.Raycast(Ț);
bool Μί=Ǖ.Type==MyDetectedEntityType.Planet||Ǖ.Type==MyDetectedEntityType.Asteroid;bool Ηη=Ǖ.HitPosition.HasValue&&Vector3D.Distance(Ǖ.HitPosition.Value,ψ)<δ;bool Θχ=!Ǖ.IsEmpty()&&!Ư.Contains(Ǖ.EntityId)&&!ΚΚ(Ǖ.EntityId)&&
Ǖ.Type!=MyDetectedEntityType.FloatingObject&&!(Βο&&Ηη)&&
!(Μί&&(k>0||Ω==Α.Ƿ||Ηη));if(Θχ){if(!ŋ) ΔЉ=DateTime.Now;ŋ=true;ΔА=0;Βς=Ǖ.Type.ToString();}else Ζξ();if(debugDraw&&ΒЂ.Count<16) ΒЂ.Add(π(Ğ.GetPosition())+","+π(Ț)+","+(Θχ?1:0));}ΔЈ=Ης>0?0:ΔЈ+1;if(ΔЈ>12) Λ="Cameras can't see ahead";double Ηο=(DateTime.Now-ΔЉ).TotalSeconds;
if(Βο&&ŋ&&Ηο>4){if(Ɲ<4){Ɲ++;Ɯ=(Ɲ==1?Ѕ:Ɲ==2?ƹ:Ɲ==3?-ƹ:-Ѕ)*δ;ΔЉ=DateTime.Now;ǈ();Λ="Docking: going around an obstacle";}else Λ="Docking blocked: "+Βς+", stopped";return;}if(ŋ&&Ηο>obstacleTimeout)
Λ="Path blocked: "+Βς;}void Ζξ(){if(ŋ&&++ΔА>=Κί) ǈ();}IMyCameraBlock Ιε(Vector3 Ț){IMyCameraBlock Ћ=null;float Ηξ=-2;for(int n=0;n<ȕ.Count;n++){ΔЎ=(ΔЎ+1)%ȕ.Count;IMyCameraBlock c=ȕ[ΔЎ];if(!c.IsFunctional||!c.IsWorking) continue;float ΘЃ=Vector3.Dot(Vector3.Normalize(Ț-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(ΘЃ>Ηξ+0.05f&&c.CanScan(Ț)){Ћ=c;Ηξ=ΘЃ;}}return Ћ;}const String Δς="[PAMTFC]";const float Κς=5f;const float ΓЉ=2f;class Ě{public long Θ;public Vector3 ά,з;public Vector3 Ȟ;public float ΗІ;public String Ι="";public int Ĥ=-1;public bool Αψ;public int ȃ;public int ǝ;public float Ȇ=-1;
public float Αζ,Αγ;public long γ=0;public int Αά=0;public DateTime Ȧ;}IMyBroadcastListener Γμ=null;long ќ=0;List<Ě>џ=new List<Ě>();Vector3 Ĵ=new Vector3();float Ǎ=-1;Vector3 ΖΡ=new Vector3();bool Γλ=false;DateTime ΖΣ=DateTime.Now;const float Ηά=15f;const float Κρ=10f;int ΙΟ=0;
String к="";int Й=-1;bool Βϋ=false,Βϊ=false,Αβ=false;Vector3 Ηύ=new Vector3();float Ƌ(){return Ī>0?Ī:Ȫ;}float Ɗ(){return ĩ>0?ĩ:ȩ;}String Ř(){if(η==Τ.ы||Ξ==Ν.ί) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Б+"|"+Я+"|"+(int) ū+"|"+Math.Round(Ƌ(),1)+"|"+Math.Round(Ɗ(),1)+"|"+Ū;}void ΚІ(){if(!trafficEnabled) return;if(Γμ==null) Γμ=IGC.RegisterBroadcastListener(Δς);ќ=Me.CubeGrid.EntityId;int Ǧ=30;while(Γμ.HasPendingMessage&&Ǧ-->0){String Η=Γμ.AcceptMessage().Data as String;
if(Η!=null) ΚΥ(Η);}for(int i=џ.Count-1;i>=0;i--)
if((DateTime.Now-џ[i].Ȧ).TotalSeconds>Κς) џ.RemoveAt(i);if(Γθ==null) Γθ=IGC.RegisterBroadcastListener(ΗΪ);Ǧ=5;while(Γθ.HasPendingMessage&&Ǧ-->0){String Η=Γθ.AcceptMessage().Data as String;if(Η!=null) try{ΚΦ(Η);}catch{}}if(Βό==null) Βό=IGC.RegisterBroadcastListener(ΖЏ);
Ǧ=10;while(Βό.HasPendingMessage&&Ǧ-->0){String Η=Βό.AcceptMessage().Data as String;if(Η!=null) try{ΚΡ(Η);}catch{}}if(++ΙΟ<3) return;ΙΟ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",ќ+";"+π(Ζ.Ε)+";"+
π(η==Τ.ы?new Vector3():Ζ.Є)+";"+π(ȡ)+";"+
π(ψ+Ĵ)+";"+String.Join(":",ΒЂ));ΒЂ.Clear();}IGC.SendBroadcastMessage(Δς,ќ+";"+π(β)+";"+
π(Ϊ.GetShipVelocities().LinearVelocity)+";"+δ+";"+Ř()+";"+
(η==Τ.Л?Й:-1)+";"+к+";"+Ικ()+","+Ζσ()+";"+π(ΖΡ)+
";"+(ΒІ?1:0)+";"+(ņ&&η==Τ.Л?1:0)+","+ΕЌ+","+ΕЋ+","+ΚΝ()+","+Ηέ()+","+Math.Round(Vector3.Distance(β,ψ))+";"+Me.GetId());}void ΚΥ(String Η){try{String[]f=Η.Split(';');if(f.Length<7) return;long Θ=long.Parse(f[0]);if(Θ==ќ) return;Ě Δ=null;for(int i=0;i<џ.Count;i++) if(џ[i].Θ==Θ) Δ=џ[i];
if(Δ==null){Δ=new Ě();Δ.Θ=Θ;џ.Add(Δ);}Δ.ά=κ(f[1]);Δ.з=κ(f[2]);Δ.ΗІ=float.Parse(f[3]);Δ.Ι=f[4];Δ.Ĥ=int.Parse(f[5]);Δ.Ȧ=DateTime.Now;if(Δ.Ι!=""&&Δ.Ι==Ř()) ΗΕ(f[6]);Δ.γ=0;Δ.Αά=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.γ=long.Parse(d[0]);Δ.Αά=int.Parse(d[1]);
}Δ.Ȟ=f.Length>=9?κ(f[8]):new Vector3();Δ.Αψ=f.Length>=10&&f[9]=="1";Δ.ȃ=0;Δ.ǝ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.Αζ=float.Parse(w[1]);Δ.Αγ=float.Parse(w[2]);Δ.ȃ=int.Parse(w[3]);if(w.Length>=5) Δ.ǝ=int.Parse(w[4]);Δ.Ȇ=w.Length>=6?float.Parse(w[5]):-1;}
}}catch{}}bool ΚΚ(long Θ){for(int i=0;i<џ.Count;i++) if(џ[i].Θ==Θ) return true;return false;}Vector3 Ζώ(int Β,int ΛЁ){if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();έ ή=Ψ[Β];Vector3 Μά=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*ΛЁ;
Vector3 Ѕ=ή.ό.LengthSquared()>0.01f?-Vector3.Normalize(ή.ό):-ή.λ;Vector3 ч=Vector3.Cross(Μά,Ѕ);if(ч.LengthSquared()<0.0001f) return new Vector3();float υ=laneOffset*δ;float Λά=Math.Min(Vector3.Distance(ή.Ε,Ψ[0].Ε),
Vector3.Distance(ή.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(ч)*υ*Math.Min(1,Λά/(υ*4));}Vector3 ΙΞ=new Vector3();List<String>ΒЂ=new List<String>();bool ΖІ(){Α s=Ω;return Ř()!=""&&(s==Α.Ƿ||s==Α.ř||s==Α.Ȑ||
s==Α.Ǥ||s==Α.Ŗ);}float ΖБ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Б*Ƌ(),2)+Math.Pow(Я*Ɗ(),2));}bool ΒΦ(){return Ř()!=""&&Vector3.Distance(β,Ζ.Ε)<ΖБ()+δ*2;}int Ηέ(){Α s=Ω;if(s==Α.Ǥ||s==Α.Ȯ||(s==Α.И&&ΒΦ())) return 2;return s==Α.Ƿ||s==Α.Ȑ||s==Α.Ŗ||(s==Α.т&&ΒΦ())?1:0;
}bool ΖЄ(float Θς,Ě o){if(o.Ȇ<0||Math.Abs(Θς-o.Ȇ)<=2) return ќ<o.Θ;return Θς<o.Ȇ;}int ΚΝ(){if(!ΖІ()) return 0;return Ω!=Α.Ƿ&&Vector3.Dot(β-О,Ф)>δ*0.5f?2:1;}bool ΚΙ(Α s){return s==Α.ǵ||s==Α.ő||s==Α.ŕ||s==Α.ΑΘ||
s==Α.ǻ||s==Α.Ȑ||s==Α.Ǥ||s==Α.Ŗ||s==Α.ķ;}void ΚЇ(){Ĵ=new Vector3();Ǎ=-1;ΖΡ=new Vector3();bool Αε=ΖЍ(Ω);if(!trafficEnabled||(!Αε&&!ΚΙ(Ω))){Γλ=false;return;}ΙΞ=Ϊ.GetShipVelocities().LinearVelocity;Ιμ(Ϊ.GetNaturalGravity(),!Αε);ΙΩ();}IMyBroadcastListener Γθ=null;
float ƽ=-1;DateTime ΙΚ=DateTime.MinValue;void ΚΦ(String Η){if(!Η.StartsWith("C;")) return;ΙΚ=DateTime.Now;ƽ=-1;String Ας=ќ+":";String[]e=Η.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ας)) ƽ=float.Parse(e[i].Substring(Ας.Length));}void ΙΩ(){if(ƽ<0||(DateTime.Now-ΙΚ).TotalSeconds>1.5) return;
Ǎ=Ǎ<0?ƽ:Math.Min(Ǎ,ƽ);if(ƽ<0.5f) Λ="Traffic: giving way (controller)";}void Ιμ(Vector3 ό,bool Θό=false){Ĵ=new Vector3();Ǎ=-1;Vector3 ȧ=ψ-β;float ǟ=ȧ.Length();bool Εά=ǟ>=1f;Vector3 ρ=Εά?ȧ/ǟ:new Vector3();ΖΡ=ρ;Vector3 Ѕ=ό.LengthSquared()>0.01f?-Vector3.Normalize(ό):-Αή;
Vector3 ч=Vector3.Cross(ρ,Ѕ);ч=ч.LengthSquared()<0.0001f?-Αο:Vector3.Normalize(ч);Ě ΓΘ=null;bool ΘΖ=Ω==Α.И&&!ę(1);int Εψ=Ηέ();for(int i=0;i<џ.Count;i++){Ě θ=џ[i];if(θ.Αά==3&&θ.з.LengthSquared()<0.25f) continue;Vector3 ά=θ.ά+θ.з*(float)(DateTime.Now-θ.Ȧ).TotalSeconds;
Vector3 ѐ=ά-β;float и=(δ+θ.ΗІ)*0.5f;bool Αϊ=θ.Ι!=""&&θ.Ι==Ř();if(Αϊ&&θ.ȃ==2) continue;bool Γβ=Αϊ&&θ.ȃ==1&&ΖІ();if(Γβ) и=Math.Min(Ƌ(),Ɗ())*0.9f;float ΕΓ=и*(Γβ?1f:1.2f);float Ĝ=Εά?Vector3.Dot(ѐ,ρ):0;if(Ĝ>0&&(ѐ-ρ*Ĝ).Length()<=ΕΓ){float ΓΩ=Vector3.Dot(θ.з,ρ);
bool ΘΚ=θ.Ȟ.LengthSquared()>0.25f;bool ΘΜ=ΓΩ<-1f||(ΘΚ&&Vector3.Dot(θ.Ȟ,ρ)<-0.5f);if(!Θό&&θ.ǝ<=Εψ){if(θ.Αψ){if(!ΘΖ){Ĵ=ч*и*1.3f;continue;}float Θο=Vector3.Distance(β,ψ),Θά=Vector3.Distance(ά,ψ);if(Θο<Θά||(Θο==Θά&&ќ<θ.Θ)) continue;}if(ΘΜ){if(Ĝ<(Đ-ΓΩ)*6f+и*4f) Ĵ=ч*и*1.3f;
continue;}if(ΘΚ&&ΖЄ(ǟ,θ)){Vector3 Ηκ=-ѐ;float Θϊ=Vector3.Dot(Ηκ,θ.Ȟ);if(Θϊ>0&&(Ηκ-θ.Ȟ*Θϊ).Length()<=ΕΓ){Ĵ=ч*и*1.3f;continue;}}}float ΘΕ=Ĝ-и-(Γβ?1f:followGap*δ);float ћ=Math.Max(0,ΓΩ)+Math.Max(0,ΘΕ)/ΓЉ;if(ΘΜ) ћ=Math.Max(0,ΘΕ+Math.Min(0,ΓΩ)*ΓЉ*2)/ΓЉ;
if(Ǎ<0||ћ<Ǎ){Ǎ=ћ;ΓΘ=θ;}continue;}if(θ.з.LengthSquared()<1f) continue;if(Εά&&(ѐ-ρ*Ĝ).Length()<=ΕΓ) continue;Vector3 ΕЉ=θ.з-ΙΞ;float ΘЂ=ΕЉ.LengthSquared();if(ΘЂ<0.25f) continue;float t=-Vector3.Dot(ѐ,ΕЉ)/ΘЂ;if(t<=0||t>5f||(ѐ+ΕЉ*t).Length()>=(Γβ?и:и*1.1f)) continue;
if(θ.ǝ>Εψ||(θ.ǝ==Εψ&&!ΖЄ(ǟ,θ))||t<2f){Ǎ=0;ΓΘ=θ;}}bool Ακ=Ǎ>=0&&Ǎ<0.5f;if(!Ακ||Θό||(ΓΘ!=null&&ΓΘ.Αψ&&ΘΖ)){Γλ=false;return;}if(!Γλ){Γλ=true;ΖΣ=DateTime.Now;}double ΘΝ=(DateTime.Now-ΖΣ).TotalSeconds;if(ΘΝ>Ηά+Κρ) ΖΣ=DateTime.Now;else if(ΘΝ>Ηά){Ǎ=-1;Ĵ=ч*δ*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>Ĉ=new List<int>();String ΘΟ="";void ΓЈ(){String э=Б+"|"+Я+"|"+(int) ū;if(э==ΘΟ) return;ΘΟ=э;Ζε(Б,Я,ū,Ĉ);}void Ζε(int w,int h,і ΑϏ,List<int>ΔЏ){ΔЏ.Clear();if(ΑϏ==і.ΑΝ){for(int k=0;k<w*h;k++){int ō=k/w;ΔЏ.Add(ō*w+(ō%2==0?k-ō*w:w-1-(k-ō*w)));
}return;}int Αθ=(int) Math.Ceiling(w/2f),Αη=(int) Math.Ceiling(h/2f);int ΕΩ=w/2,ΕΨ=h/2;int x=0,y=0,ΕΜ=0,ΕΝ=-1,ƫ=Math.Max(w,h);for(int i=0;i<ƫ*ƫ;i++){if(-Αθ<x&&x<=ΕΩ&&-Αη<y&&y<=ΕΨ) ΔЏ.Add((y-1+Αη)*w+(x-1+Αθ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΕΜ;ΕΜ=-ΕΝ;ΕΝ=t;}
x+=ΕΜ;y+=ΕΝ;}}bool ΓЇ(){int n=Math.Max(0,Б*Я);if(к.Length==n) return false;к=new String('0',n);Й=-1;return true;}void ΗΕ(String θ){ΓЇ();if(θ.Length!=к.Length) return;char[]h=к.ToCharArray();for(int i=0;i<h.Length;i++) if(θ[i]=='1') h[i]='1';к=new String(h);}bool ΒΟ(int г){return г>=0&&г<к.Length&&к[г]=='1';}
void Δθ(int г,bool φ){if(г<0||г>=к.Length) return;char[]h=к.ToCharArray();h[г]=φ?'1':'0';к=new String(h);}bool ΔΔ(int г,bool Λμ){if(!trafficEnabled) return false;String э=Ř();for(int i=0;i<џ.Count;i++){Ě t=џ[i];if(t.Ι!=э||t.Ĥ<0||(Λμ&&t.Θ>ќ)) continue;if(t.Ĥ==г) return true;
}return false;}int Ιι(int Β){int n=Б*Я;if(n<1) return Β;ΓЈ();if(ΓЇ()) for(int k=0;k<Β&&k<n;k++) Δθ(Ĉ[k],true);if(Й>=0&&(Β>=n||Ĉ[Β]!=Й)){Δθ(Й,true);Й=-1;}if(ΖА()){if(ȗ>=0&&!ΒΟ(ȗ)){ņ=false;Й=ȗ;return Ĉ.IndexOf(ȗ);}if(ȗ==-2){Й=-1;ņ=к.IndexOf('0')>=0;return ņ?-2:n;}
if(ȗ==-1&&Й>=0&&!ΒΟ(Й)){ņ=false;return Β;}Й=-1;if(!ņ){ņ=true;ΗϏ=DateTime.Now;}if((DateTime.Now-ΗϏ).TotalSeconds<5) return-1;}ņ=false;if(Й>=0&&!ΒΟ(Й)&&!ΔΔ(Й,true)) return Β;Й=-1;for(int k=0;k<n;k++){int c=Ĉ[k];if(ΒΟ(c)||ΔΔ(c,false)) continue;Й=c;
return k;}return к.IndexOf('0')>=0?-2:n;}const String ΖЏ="[PAMJOB]";const String ΗΪ="[PAMTCC]";IMyBroadcastListener Βό=null;DateTime Ηώ=DateTime.MinValue;DateTime ΗϏ=DateTime.Now;int ȗ=-1;bool ņ=false;bool ΖА(){return trafficEnabled&&ȗ!=-3&&(DateTime.Now-Ηώ).TotalSeconds<3;}
void ΚΡ(String Η){String[]f=Η.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ř()) return;Ηώ=DateTime.Now;ΗΕ(f[2]);ȗ=-3;String Ας=ќ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ας)) ȗ=int.Parse(e[i].Substring(Ας.Length));}Ǻ Κι(int Β)
{ΓЈ();if(Β<0||Β>=Ĉ.Count) return Ǻ.ΑЏ;І=Ĉ[Β]%Б;Ђ=Ĉ[Β]/Б;return Ǻ.ΒΛ;}class ǣ{public long Θ;public Vector3 ά,д,Ѕ;}List<ǣ>Э=new List<ǣ>();bool ł=false;Vector3 ΒЈ,ΒЇ,ΒЉ,ΒЊ;long Ǘ=0;long Ď=0;έ Ǭ=null;bool Αέ=false;έ ΓЄ(Vector3 ά,Vector3 д,Vector3 Ѕ){MatrixD m=MatrixD.CreateWorld(ά,д,Ѕ);
έ w=new έ(Ρ);w.μ=Vector3D.TransformNormal(ΒЈ,m);w.λ=Vector3D.TransformNormal(ΒЇ,m);w.Є=Vector3D.TransformNormal(ΒЉ,m);w.Ε=ά+(Vector3) Vector3D.TransformNormal(ΒЊ,m);w.ν=true;return w;}int Ζσ(){Α s=Ω;if(s==Α.ѕ) return о(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ŕ||s==Α.Ā||s==Α.Ŕ||s==Α.Βι||
s==Α.ΒΖ||s==Α.ǻ) return 3;if(s==Α.ǵ||s==Α.ő||s==Α.ΑΘ) return 2;if(s==Α.И) return 1;return 0;}long Ικ(){if(Ζσ()==0) return 0;if(ę(1)&&ƺ!=0) return ƺ;return Ǭ!=null?Ď:Ǘ;}void ΗΑ(){if(Ξ==Ν.ί||!Ρ.ν) return;IMyShipConnector Х=о(MyShipConnectorStatus.Connected);if(Х==null||Х.OtherConnector==null) return;
IMyShipConnector θ=Х.OtherConnector;Vector3I Λδ=new Vector3I((int) Ρ.х.X,(int) Ρ.х.Y,(int) Ρ.х.Z);if(Х.Position==Λδ){MatrixD Ƥ=MatrixD.Transpose(θ.WorldMatrix);ΒЈ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Forward,Ƥ);ΒЇ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Down,Ƥ);ΒЉ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Left,Ƥ);
ΒЊ=Vector3D.TransformNormal(Х.GetPosition()-θ.GetPosition(),Ƥ);ł=true;}if(Vector3.Distance(Х.GetPosition(),Ρ.Ε)<5) Ǘ=θ.EntityId;List<IMyShipConnector>Ŧ=new List<IMyShipConnector>();χ.GetBlocksOfType(Ŧ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ŧ.Count>0){Э.Clear();for(int i=0;i<Ŧ.Count;i++){ǣ p=new ǣ();p.Θ=Ŧ[i].EntityId;p.ά=Ŧ[i].GetPosition();p.д=Ŧ[i].WorldMatrix.Forward;p.Ѕ=Ŧ[i].WorldMatrix.Up;Э.Add(p);}}Ǭ=null;Ď=0;if(θ.EntityId==Ǘ||!ł) return;for(int i=0;i<Э.Count;i++)
if(Э[i].Θ==θ.EntityId){Ď=Э[i].Θ;Ǭ=ΓЄ(Э[i].ά,Э[i].д,Э[i].Ѕ);}}bool ΔΛ(long Θ,int Θσ){if(Θ==0) return false;for(int i=0;i<џ.Count;i++){Ě t=џ[i];if(t.γ==Θ&&(t.Αά>Θσ||(t.Αά==Θσ&&t.Θ<ќ))) return true;}return false;}bool Γώ(int Ŷ){if(Ξ==Ν.ί||!trafficEnabled){Ǭ=null;Ď=0;return true;}
long σ=Ǭ!=null?Ď:Ǘ;if(!ΔΛ(σ,Ŷ)) return true;if(!ΔΛ(Ǘ,Ŷ)){Ǭ=null;Ď=0;return true;}if(!ł) return false;ǣ Ћ=null;float ΑΦ=0;for(int i=0;i<Э.Count;i++){ǣ p=Э[i];if(p.Θ==Ǘ||ΔΛ(p.Θ,Ŷ)) continue;float d=Vector3.Distance(p.ά,Ρ.Ε);if(Ћ==null||d<ΑΦ){Ћ=p;ΑΦ=d;}
}if(Ћ==null) return false;Ď=Ћ.Θ;Ǭ=ΓЄ(Ћ.ά,Ћ.д,Ћ.Ѕ);return true;}String Ιφ(){String s=(ł?"1":"0")+":"+π(ΒЈ)+":"+π(ΒЇ)+":"+
π(ΒЉ)+":"+π(ΒЊ)+":"+Ǘ;for(int i=0;i<Э.Count;i++)
s+=":"+Э[i].Θ+","+π(Э[i].ά)+","+π(Э[i].д)+","+π(Э[i].Ѕ);return s;}void Ιυ(String s){Э.Clear();ł=false;Ǘ=0;String[]f=s.Split(':');if(f.Length<6) return;ł=f[0]=="1";ΒЈ=κ(f[1]);ΒЇ=κ(f[2]);ΒЉ=κ(f[3]);ΒЊ=κ(f[4]);long.TryParse(f[5],out Ǘ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǣ p=new ǣ();if(!long.TryParse(e[0],out p.Θ)) continue;p.ά=κ(e[1]);p.д=κ(e[2]);p.Ѕ=κ(e[3]);Э.Add(p);}}const String Ǌ="[PAMDCK]";const float Ιΰ=120f;class ĕ{public long Δ;public int Ǫ;public long ǖ;public bool Ηρ;public DateTime Ǩ,Ƨ;
}IMyBroadcastListener ΒЅ=null;List<ĕ>Ю=new List<ĕ>();List<IMyShipConnector>ı=new List<IMyShipConnector>();List<IMySensorBlock>Ȏ=new List<IMySensorBlock>();List<IMyCameraBlock>ī=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Γς=new List<MyDetectedEntityInfo>();long ї=0;int Βφ=0;
long Βυ=0;DateTime Ητ=DateTime.Now;String ġ="";int ΗЋ=0;IMyCubeGrid Ĳ=null;void Κζ(){Ĳ=Me.CubeGrid;χ.GetBlocksOfType(ı,q=>q.CubeGrid.IsSameConstructAs(Ĳ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȏ,q=>q.CubeGrid.IsSameConstructAs(Ĳ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ī,q=>q.CubeGrid.IsSameConstructAs(Ĳ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȏ.Count;i++){Ȏ[i].DetectSmallShips=true;Ȏ[i].DetectLargeShips=true;}for(int i=0;i<ī.Count;i++) ī[i].EnableRaycast=true;}IMyShipConnector Δο(long Θ){if(Θ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Θ) as IMyShipConnector;return c!=null&&Ĳ!=null&&c.CubeGrid.IsSameConstructAs(Ĳ)?c:null;}bool ΔΚ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖЎ(IMyShipConnector c,long Δ){for(int i=0;i<ī.Count;i++)
{IMyCameraBlock Ğ=ī[i];if(!Ğ.IsWorking||Vector3D.Distance(Ğ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ț=Ğ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ğ.CanScan(Ț)) continue;MyDetectedEntityInfo Ǖ=Ğ.Raycast(Ț);if(!Ǖ.IsEmpty()&&Ǖ.EntityId!=Ĳ.EntityId&&Ǖ.EntityId!=Δ&&Ǖ.Type!=MyDetectedEntityType.FloatingObject)
{ġ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΚΛ(){for(int s=0;s<Ȏ.Count;s++){if(!Ȏ[s].IsWorking) continue;Γς.Clear();Ȏ[s].DetectedEntities(Γς);for(int i=0;i<Γς.Count;i++){MyDetectedEntityInfo e=Γς[i];bool Λί=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Λί&&e.EntityId!=ї&&e.Velocity.Length()>0.5f){ġ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΚΧ(ĕ r){IMyShipConnector ǖ=Δο(r.ǖ);if(ΔΚ(ǖ)&&ΖЎ(ǖ,r.Δ)) return ǖ.EntityId;if(!r.Ηρ||ǖ==null) return 0;IMyShipConnector Ћ=null;double ΑΦ=0;for(int i=0;i<ı.Count;i++)
{IMyShipConnector c=ı[i];if(!ΔΚ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ǖ.GetPosition());if(Ћ==null||d<ΑΦ){if(ΖЎ(c,r.Δ)){Ћ=c;ΑΦ=d;}}}return Ћ==null?0:Ћ.EntityId;}void ΙЋ(){ĕ Ʊ=null;long Х=0;for(int č=2;č>=1&&Ʊ==null;č--){ĕ ų=null;for(int i=0;i<Ю.Count;i++)
{ĕ r=Ю[i];if(r.Ǫ!=č) continue;if(č==1){long c=ΚΧ(r);if(c==0) continue;if(ų==null||r.Ǩ<ų.Ǩ){ų=r;Х=c;}}else if(ų==null||r.Ǩ<ų.Ǩ) ų=r;}Ʊ=ų;}if(Ʊ==null){if(Ю.Count>0&&ġ=="") ġ="No free dock";return;}ї=Ʊ.Δ;Βφ=Ʊ.Ǫ;Βυ=Ʊ.Ǫ==1?Х:0;Ητ=DateTime.Now;
}void Κϋ(){if(Ĳ==null) return;if(ΒЅ==null) ΒЅ=IGC.RegisterBroadcastListener(Ǌ);int Ǧ=30;while(ΒЅ.HasPendingMessage&&Ǧ-->0){String Η=ΒЅ.AcceptMessage().Data as String;if(Η!=null) ΙЍ(Η);}Κή();if(++ΗЋ<3) return;ΗЋ=0;IGC.SendBroadcastMessage(Ǌ,Ιή());}void ΙЍ(String Η){try{String[]f=Η.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ю.Count-1;i>=0;i--) if(Ю[i].Δ==Δ) Ю.RemoveAt(i);if(ї==Δ) ї=0;}else if(f[0]=="R"&&f.Length>=5){long ǖ=long.Parse(f[3]);if(Δο(ǖ)==null) return;ĕ r=null;for(int i=0;i<Ю.Count;i++) if(Ю[i].Δ==Δ) r=Ю[i];if(r==null){r=new ĕ();r.Δ=Δ;r.Ǩ=DateTime.Now;Ю.Add(r);}
int Ǫ=int.Parse(f[2]);if(r.Ǫ!=Ǫ) r.Ǩ=DateTime.Now;r.Ǫ=Ǫ;r.ǖ=ǖ;r.Ηρ=f[4]=="1";r.Ƨ=DateTime.Now;}}catch{}}void Κή(){for(int i=Ю.Count-1;i>=0;i--)
if((DateTime.Now-Ю[i].Ƨ).TotalSeconds>3) Ю.RemoveAt(i);ĕ Βτ=null;for(int i=0;i<Ю.Count;i++) if(Ю[i].Δ==ї) Βτ=Ю[i];if(ї!=0&&Βτ==null) ї=0;if(Βτ!=null&&(DateTime.Now-Ητ).TotalSeconds>Ιΰ){ї=0;Βτ.Ǩ=DateTime.Now;}ġ="";if(ї==0&&Ю.Count>0&&!ΚΛ()) ΙЋ();}
String Ιή(){String ε="S;"+Ĳ.EntityId+";"+ї+";"+Βφ+";"+Βυ+";";for(int i=0;i<Ю.Count;i++) ε+=(i>0?",":"")+Ю[i].Δ;ε+=";";for(int i=0;i<ı.Count;i++){IMyShipConnector c=ı[i];ε+=(i>0?":":"")+c.EntityId+","+π(c.GetPosition())+","+π(c.WorldMatrix.Forward)+","+π(c.WorldMatrix.Up);
}ε+=";";for(int i=0;i<Ȏ.Count;i++){IMySensorBlock z=Ȏ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ε+=(i>0?":":"")+π(z.GetPosition())+","+Math.Round(r,1);}return ε;}String ΙЀ()
{if(ı.Count==0&&Ȏ.Count==0) return "";int Αε=0;for(int i=0;i<ı.Count;i++) if(ΔΚ(ı[i])) Αε++;return "Docks: "+Αε+"/"+ı.Count+" free"+(ї!=0?(Βφ==1?" | docking":" | undocking"):"")+
(Ю.Count>1?" | "+(Ю.Count-1)+" waiting":"")+(ġ!=""?"\n"+ġ:"")+"\n";}IMyBroadcastListener Γζ=null;int ć=0;DateTime ΗЊ=DateTime.Now;DateTime ΘЋ=DateTime.MinValue;long ΘЌ=0;int ΘЍ=0;long ƺ=0;bool ΑЋ=false;int ΕΛ=0;List<Vector3>ΑЍ=new List<Vector3>();List<float>Ζα=new List<float>();
bool ΒІ=false;bool ΒЄ=false;Vector3 ΗЉ=new Vector3();bool ΑΛ(){return ć!=0&&(DateTime.Now-ΘЋ).TotalSeconds<3;}bool ę(int Ǫ){return ΑΛ()&&ΘЌ==ќ&&ΘЍ==Ǫ;}bool ΗΦ(){return ΑΛ()||(DateTime.Now-ΗЊ).TotalSeconds<3;}void Δδ(int Ǫ){if(ć==Ǫ) return;ć=Ǫ;ΗЊ=DateTime.Now;
ΕΛ=3;}void Δγ(){if(ć!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ǌ,"D;"+ќ);ć=0;ΑЋ=false;}void Κϊ(){if(!trafficEnabled||Ξ==Ν.ί) return;if(Γζ==null) Γζ=IGC.RegisterBroadcastListener(Ǌ);int Ǧ=20;while(Γζ.HasPendingMessage&&Ǧ-->0){String Η=Γζ.AcceptMessage().Data as String;
if(Η==null||!Η.StartsWith("S;")) continue;try{ΚΤ(Η);}catch{}}if(Ω!=Α.И){ΒІ=false;ΒЄ=false;}if(ć==1&&о(MyShipConnectorStatus.Connected)!=null) Δγ();if(ć==2&&!ΑЋ&&Ω!=Α.ǻ&&
Vector3.Distance(β,є().Ε)>δ*3) Δγ();if(ć!=0&&++ΕΛ>=3){ΕΛ=0;IGC.SendBroadcastMessage(Ǌ,"R;"+ќ+";"+ć+";"+Ǘ+";"+(ł?1:0));}}void ΚΤ(String Η){String[]f=Η.Split(';');if(f.Length<7) return;bool ΘΔ=false;String[]Ηΰ=f[5].Split(',');for(int i=0;i<Ηΰ.Length;i++) if(Ηΰ[i]==ќ.ToString()) ΘΔ=true;
if(!ΘΔ) return;ΘЋ=DateTime.Now;ΘЌ=long.Parse(f[2]);ΘЍ=int.Parse(f[3]);ƺ=long.Parse(f[4]);ΑЍ.Clear();Ζα.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑЍ.Add(κ(e[0]));Ζα.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗЍ=f[6].Split(':');List<ǣ>В=new List<ǣ>();for(int i=0;i<ΗЍ.Length;i++){String[]e=ΗЍ[i].Split(',');if(e.Length<4) continue;ǣ p=new ǣ();p.Θ=long.Parse(e[0]);p.ά=κ(e[1]);p.д=κ(e[2]);p.Ѕ=κ(e[3]);В.Add(p);}Э=В;}bool Ιη(){if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return true;
Δδ(1);if(!ΗΦ()) return true;if(!ę(1)){Λ="Waiting for dock clearance";return false;}Γφ();return true;}bool Γφ(){Αέ=false;ǣ ŵ=null;if(ƺ!=0&&ƺ!=Ǘ&&ł)
for(int i=0;i<Э.Count;i++) if(Э[i].Θ==ƺ) ŵ=Э[i];long Ǆ=ŵ==null?0:ŵ.Θ;if(Ǆ==Ď&&(Ǆ==0)==(Ǭ==null)) return false;Ď=Ǆ;Ǭ=ŵ==null?null:ΓЄ(ŵ.ά,ŵ.д,ŵ.Ѕ);return true;}bool Ɖ(Vector3 p){for(int i=0;i<ΑЍ.Count;i++) if(Vector3.Distance(p,ΑЍ[i])<Ζα[i]+δ*0.75f) return true;
return false;}bool ΙБ(Vector3 ΑΪ,Vector3 Ų){ΒІ=false;if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return false;if(ć!=1&&Vector3.Distance(β,Ρ.Ε)>dockRequestDistance) return false;Δδ(1);if(!ΑΛ()||ę(1)||ΑЍ.Count==0) return false;if(!Ɖ(ΑΪ)&&!Ɖ(Ų)&&!Ɖ(β)) return false;ΒІ=true;return true;
}bool Ζμ(){if(!trafficEnabled||Ξ==Ν.ί||Ǘ==0) return true;Δδ(2);if(!ΗΦ()) return true;if(!ę(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΚΜ=600f;class ȱ{public long Θ;public int г=-1;public bool Ǆ;public float Αζ,Αγ;public DateTime ΕЊ;}class ȏ{public String э;
public int w,h;public float ΖΥ,ΖΘ;public String φ;public List<int>ΓΨ=new List<int>();public List<ȱ>ш=new List<ȱ>();}List<ȏ>ǘ=new List<ȏ>();IMyBroadcastListener ΓΗ=null;int Θέ=0;void ΚϏ(){if(ΓΗ==null) ΓΗ=IGC.RegisterBroadcastListener(Δς);int Ǧ=50;while(ΓΗ.HasPendingMessage&&Ǧ-->0)
{String Η=ΓΗ.AcceptMessage().Data as String;if(Η==null) continue;try{ΙЏ(Η);}catch{}try{ΙА(Η);}catch{}}for(int j=ǘ.Count-1;j>=0;j--){ȏ Ι=ǘ[j];for(int i=Ι.ш.Count-1;i>=0;i--)
if((DateTime.Now-Ι.ш[i].ΕЊ).TotalSeconds>ΚΜ) Ι.ш.RemoveAt(i);if(Ι.ш.Count==0){ǘ.RemoveAt(j);continue;}ΙΪ(Ι);}if(++Θέ<3) return;Θέ=0;for(int j=0;j<ǘ.Count;j++) IGC.SendBroadcastMessage(ΖЏ,Ιέ(ǘ[j]));}void ΙΪ(ȏ Ι){for(int i=0;i<Ι.ш.Count;i++){ȱ s=Ι.ш[i];if(s.Ǆ&&(s.г<0||Ι.φ[s.г]=='1')) s.г=ΚΟ(Ι,s);
}}String Ιέ(ȏ Ι){String ε="J;"+Ι.э+";"+Ι.φ+";";bool Ǩ=true;for(int i=0;i<Ι.ш.Count;i++){if((DateTime.Now-Ι.ш[i].ΕЊ).TotalSeconds>5) continue;ε+=(Ǩ?"":",")+Ι.ш[i].Θ+":"+Ι.ш[i].г;Ǩ=false;}return ε;}void ΙЏ(String Η){String[]f=Η.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȏ Ι=null;for(int i=0;i<ǘ.Count;i++) if(ǘ[i].э==f[4]) Ι=ǘ[i];if(Ι==null){Ι=new ȏ();Ι.э=f[4];Ι.w=int.Parse(k[3]);Ι.h=int.Parse(k[4]);Ι.ΖΥ=float.Parse(k[6]);Ι.ΖΘ=float.Parse(k[7]);if(Ι.w<1||Ι.h<1) return;Ζε(Ι.w,Ι.h,(і) int.Parse(k[5]),Ι.ΓΨ);
Ι.φ=new String('0',Ι.w*Ι.h);ǘ.Add(Ι);}String d=f[6];if(d.Length==Ι.φ.Length){char[]c=Ι.φ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Ι.φ=new String(c);}long Θ=long.Parse(f[0]);ȱ s=null;for(int i=0;i<Ι.ш.Count;i++) if(Ι.ш[i].Θ==Θ) s=Ι.ш[i];
if(s==null){s=new ȱ();s.Θ=Θ;Ι.ш.Add(s);}s.ΕЊ=DateTime.Now;String[]w=f[10].Split(',');s.Ǆ=w[0]=="1";if(w.Length>=3){s.Αζ=float.Parse(w[1]);s.Αγ=float.Parse(w[2]);}int Ĥ=int.Parse(f[5]);if(Ĥ>=0&&Ĥ<Ι.φ.Length&&(s.г<0||Ι.φ[s.г]=='1')&&Ι.φ[Ĥ]!='1'&&ΔΕ(Ι,Ĥ,s)==null)
s.г=Ĥ;}ȱ ΔΕ(ȏ Ι,int г,ȱ ΛΩ){for(int i=0;i<Ι.ш.Count;i++){ȱ o=Ι.ш[i];if(o!=ΛΩ&&o.г==г)
return o;}return null;}int ΚΟ(ȏ Ι,ȱ s){for(int k=0;k<Ι.ΓΨ.Count;k++){int c=Ι.ΓΨ[k];if(Ι.φ[c]!='1'&&ΔΕ(Ι,c,s)==null) return c;}return-2;}String ΙЅ(){String s="";for(int j=0;j<ǘ.Count;j++){ȏ Ι=ǘ[j];int φ=0;for(int i=0;i<Ι.φ.Length;i++) if(Ι.φ[i]=='1') φ++;s+="Job "+(j+1)+": "+φ+"/"+Ι.φ.Length+" holes, "+Ι.ш.Count+" ships\n";
}return s;}const float Κο=8f;class Щ{public long Θ,Ŵ;public Vector3 ά,з,Ȟ;public float ƞ,Αζ,Αγ,ћ=-1;public int ȃ;public int ǝ;public float Ȇ=-1;public String Ι="";public int Ŷ;public DateTime Ȧ;}class Βε{public long a,b,Ζΰ;public DateTime Ƨ;}List<Щ>ξ=new List<Щ>();List<Βε>ў=new List<Βε>();
int ΖΗ=0;void ΙА(String Η){String[]f=Η.Split(';');if(f.Length<9) return;long Θ=long.Parse(f[0]);Щ s=null;for(int i=0;i<ξ.Count;i++) if(ξ[i].Θ==Θ) s=ξ[i];if(s==null){s=new Щ();s.Θ=Θ;ξ.Add(s);}s.ά=κ(f[1]);s.з=κ(f[2]);s.ƞ=float.Parse(f[3]);s.Ι=f[4];s.Ŷ=int.Parse(f[7].Split(',')[1]);
s.Ȟ=κ(f[8]);s.ȃ=0;s.ǝ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.Αζ=float.Parse(w[1]);s.Αγ=float.Parse(w[2]);s.ȃ=int.Parse(w[3]);if(w.Length>=5) s.ǝ=int.Parse(w[4]);s.Ȇ=w.Length>=6?float.Parse(w[5]):-1;}}s.Ŵ=f.Length>=12?long.Parse(f[11]):0;
s.Ȧ=DateTime.Now;}bool ΑΜ(Щ s){return s.з.LengthSquared()<0.25f&&(s.Ŷ==3||s.Ȟ.LengthSquared()<0.25f);}Vector3 ΗΫ(Щ s){return s.з.LengthSquared()>=1f?s.з:s.Ȟ*2f;}void ΚЈ(){Ιν();if(++ΖΗ<3) return;ΖΗ=0;IGC.SendBroadcastMessage(ΗΪ,Ιί());}void Ιν(){for(int i=ξ.Count-1;i>=0;i--)
if((DateTime.Now-ξ[i].Ȧ).TotalSeconds>3) ξ.RemoveAt(i);for(int i=ў.Count-1;i>=0;i--)
if((DateTime.Now-ў[i].Ƨ).TotalSeconds>2) ў.RemoveAt(i);for(int i=0;i<ξ.Count;i++) ξ[i].ћ=-1;for(int i=0;i<ξ.Count;i++)
for(int j=i+1;j<ξ.Count;j++){Щ a=ξ[i],b=ξ[j];if(ΑΜ(a)||ΑΜ(b)) continue;Vector3 Λώ=a.ά+a.з*(float)(DateTime.Now-a.Ȧ).TotalSeconds;Vector3 Ŵ=b.ά+b.з*(float)(DateTime.Now-b.Ȧ).TotalSeconds;Vector3 ƿ=ΗΫ(a),ǀ=ΗΫ(b);Vector3 ѐ=Ŵ-Λώ;float υ=ѐ.Length();float и=(a.ƞ+b.ƞ)*0.55f;
bool Αϊ=a.Ι!=""&&a.Ι==b.Ι;if(Αϊ&&(a.ȃ==2||b.ȃ==2)) continue;if(Αϊ&&a.ȃ==1&&b.ȃ==1){String[]k=a.Ι.Split('|');if(k.Length>=8) и=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ΕЈ=ǀ-ƿ;float ΘЁ=ΕЈ.LengthSquared();float t=ΘЁ>0.01f?-Vector3.Dot(ѐ,ΕЈ)/ΘЁ:0;
bool ΖΟ=υ<и;if(!ΖΟ&&(t<=0||t>Κο||(ѐ+ΕЈ*t).Length()>=и)) continue;if(!ΖΟ&&a.ǝ==b.ǝ&&ƿ.LengthSquared()>0.25f&&ǀ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ƿ),Vector3.Normalize(ǀ))<-0.7f)
continue;Βε p=null;for(int k=0;k<ў.Count;k++)
if((ў[k].a==a.Θ&&ў[k].b==b.Θ)||(ў[k].a==b.Θ&&ў[k].b==a.Θ)) p=ў[k];if(p==null){p=new Βε();p.a=a.Θ;p.b=b.Θ;p.Ζΰ=Κπ(a,b,ƿ,ǀ,ѐ,υ);ў.Add(p);}p.Ƨ=DateTime.Now;Щ y=p.Ζΰ==a.Θ?a:b;float Θγ=ΖΟ||t<4?0:Math.Max(1,y.з.Length()*0.5f);if(y.ћ<0||Θγ<y.ћ) y.ћ=Θγ;
}ΚΑ();}long Κπ(Щ a,Щ b,Vector3 ƿ,Vector3 ǀ,Vector3 ѐ,float υ){if(a.Θ==ї) return b.Θ;if(b.Θ==ї) return a.Θ;if(a.ǝ!=b.ǝ) return a.ǝ<b.ǝ?a.Θ:b.Θ;Vector3 n=υ>0.01f?ѐ/υ:new Vector3(1,0,0);float ΘЏ=Vector3.Dot(ƿ,n),ΙΙ=Vector3.Dot(ǀ,-n);if(ΘЏ-ΙΙ>1) return a.Θ;if(ΙΙ-ΘЏ>1) return b.Θ;
if(a.Ȇ>=0&&b.Ȇ>=0&&Math.Abs(a.Ȇ-b.Ȇ)>2) return a.Ȇ>b.Ȇ?a.Θ:b.Θ;return a.Θ>b.Θ?a.Θ:b.Θ;}String Ιί(){String ε="C;";bool Ǩ=true;for(int i=0;i<ξ.Count;i++){if(ξ[i].ћ<0) continue;ε+=(Ǩ?"":",")+ξ[i].Θ+":"+Math.Round(ξ[i].ћ,1);Ǩ=false;}return ε;}String ΙЊ()
{int Θρ=0,Ακ=0;for(int i=0;i<ξ.Count;i++){if(!ΑΜ(ξ[i])) Θρ++;if(ξ[i].ћ>=0) Ακ++;}if(ξ.Count==0) return "";return "Traffic: "+Θρ+" moving"+(Ακ>0?", "+Ακ+" giving way":"")+"\n";}List<ϋ>ȁ=new List<ϋ>();ϋ ĉ=null;Vector3 ΘΤ;DateTime ΘΣ;float ΘΡ=40;bool ΘΠ=false;
Щ ΓЋ(ϋ s){Щ Ћ=null;for(int i=0;i<ξ.Count;i++) if(ξ[i].Ŵ!=0&&""+ξ[i].Ŵ==s.Θ) return ξ[i];for(int i=0;i<ξ.Count;i++)
if(Vector3.Distance(ξ[i].ά,s.Ε)<15&&(Ћ==null||
Vector3.Distance(ξ[i].ά,s.Ε)<Vector3.Distance(Ћ.ά,s.Ε))) Ћ=ξ[i];return Ћ;}float ΚΒ(ϋ s){Щ t=ΓЋ(s);if(t==null) return 0;float k=t.ȃ*1000000f;String[]э=t.Ι.Split('|');if(э.Length>=3) k+=Vector3.Distance(t.ά,new Vector3(float.Parse(э[0]),float.Parse(э[1]),float.Parse(э[2])));return k;
}void Κν(){ȁ.Clear();ĉ=null;Εί.Clear();List<float>Εδ=new List<float>();for(int i=0;i<Ϗ.Count;i++){float k=ΚΒ(Ϗ[i]);int š=0;while(š<Εδ.Count&&Εδ[š]<=k) š++;Εδ.Insert(š,k);ȁ.Insert(š,Ϗ[i]);Ϗ[i].ļ("HOMEPOS",true,false,false);Βί("HOMEPOS",Ϗ[i].Θ,"HOMEPOS");}}List<Щ>Εί=new List<Щ>();
void ΚΑ(){Εί.Clear();for(int i=0;i<ȁ.Count;i++){Щ t=ΓЋ(ȁ[i]);if(t==null) continue;t.ћ=0;Εί.Add(t);}}void ΚЄ(){if(ĉ!=null){bool Λή=ΘΠ||Vector3.Distance(ĉ.Ε,ΘΤ)>ΘΡ||
(DateTime.Now-ΘΣ).TotalSeconds>45||ĉ.ΔΝ();if(!Λή) return;ĉ=null;}if(ȁ.Count==0) return;ĉ=ȁ[0];ȁ.RemoveAt(0);ΘΤ=ĉ.Ε;ΘΣ=DateTime.Now;Щ t=ΓЋ(ĉ);ΘΡ=t==null?40:Math.Max(40,t.ƞ*3);ΘΠ=t!=null&&t.Ŷ==3&&t.з.LengthSquared()<0.25f;}const String Ισ="[PAMDBG]";class ΓЁ
{public readonly bool ΔΥ;IMyProgrammableBlock ě;Action<IMyProgrammableBlock>Δω;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Δψ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Δχ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>Δϊ;
Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>Δφ;public ΓЁ(MyGridProgram ΛЅ){ě=ΛЅ.Me;var ű=ě.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ě);if(ű==null) return;Δω=(Action<IMyProgrammableBlock>) ű["RemoveDraw"];Δψ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ű["Point"];
Δχ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ű["Line"];Δϊ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ű["Sphere"];Δφ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) ű["GPS"];((Action<IMyProgrammableBlock>) ű["RemoveAll"])(ě);
ΔΥ=true;}public void ΚΪ(){if(Δω!=null) Δω(ě);}public void Ļ(Vector3D a,Vector3D b,Color c,float ΜΝ){if(Δχ!=null) Δχ(ě,a,b,c,ΜΝ,-1,false);}public void ΚΨ(Vector3D a,Color c,float ď){if(Δψ!=null) Δψ(ě,a,c,ď,-1,false);}public void ƌ(String Γ,Vector3D š,Color c){if(Δφ!=null) Δφ(ě,Γ,š,c,-1);}
public void ΗΥ(Vector3D ƚ,double ď,Color c){if(Δϊ!=null) Δϊ(ě,new BoundingSphereD(ƚ,ď),c,1,0.05f,30,-1,false);}}class ǋ{public long Θ;public Vector3 Αν,ŭ,ș,ζ;public bool ΘΛ;public List<Vector3>ΓΫ=new List<Vector3>(),ΕЃ=new List<Vector3>();public List<bool>ΕЂ=new List<bool>();
public DateTime Ȧ;}ΓЁ Р=null;IMyBroadcastListener ΒЀ=null;List<ǋ>Ć=new List<ǋ>();void ΙЌ(String Η){String[]f=Η.Split(';');if(f.Length<6) return;long Θ=long.Parse(f[0]);ǋ s=null;for(int i=0;i<Ć.Count;i++) if(Ć[i].Θ==Θ) s=Ć[i];if(s==null){s=new ǋ();s.Θ=Θ;Ć.Add(s);}
s.Αν=κ(f[1]);s.ŭ=κ(f[2]);s.ș=κ(f[3]);s.ΘΛ=s.ŭ.LengthSquared()>0.5f;s.ζ=κ(f[4]);s.Ȧ=DateTime.Now;if(f[5]=="") return;s.ΓΫ.Clear();s.ΕЃ.Clear();s.ΕЂ.Clear();String[]ΘЀ=f[5].Split(':');for(int i=0;i<ΘЀ.Length;i++){String[]r=ΘЀ[i].Split(',');if(r.Length<3) continue;s.ΓΫ.Add(κ(r[0]));s.ΕЃ.Add(κ(r[1]));s.ΕЂ.Add(r[2]=="1");
}}void Κω(){if(!debugDraw) return;if(Р==null) Р=new ΓЁ(this);if(!Р.ΔΥ) return;if(ΒЀ==null) ΒЀ=IGC.RegisterBroadcastListener(Ισ);int Ǧ=20;while(ΒЀ.HasPendingMessage&&Ǧ-->0){String Η=ΒЀ.AcceptMessage().Data as String;if(Η!=null) try{ΙЌ(Η);}catch{}}for(int i=Ć.Count-1;i>=0;i--) if((DateTime.Now-Ć[i].Ȧ).TotalSeconds>5) Ć.RemoveAt(i);
Р.ΚΪ();for(int i=0;i<ξ.Count;i++){Щ s=ξ[i];Color c=ΑΜ(s)?Color.Gray:s.ћ>=0?Color.Yellow:Color.Lime;Р.ΗΥ(s.ά,s.ƞ/2,c);Р.ƌ(Βδ(s)+": "+(ΑΜ(s)?"parked":s.ћ==0?"holding (giving way)":s.ћ>0?"slowing (giving way)":"moving"),s.ά,c);if(s.з.LengthSquared()>0.25f) Р.Ļ(s.ά,s.ά+s.з*3,Color.White,0.1f);
if(s.Ȟ.LengthSquared()>0.25f) Р.Ļ(s.ά,s.ά+s.Ȟ*s.ƞ,Color.Cyan,0.1f);}for(int i=0;i<ў.Count;i++){if((DateTime.Now-ў[i].Ƨ).TotalSeconds>0.5) continue;Щ a=null,b=null;for(int k=0;k<ξ.Count;k++){if(ξ[k].Θ==ў[i].a) a=ξ[k];if(ξ[k].Θ==ў[i].b) b=ξ[k];}if(a==null||b==null) continue;
Р.Ļ(a.ά,b.ά,Color.Red,0.2f);Р.ƌ("Conflict: "+Βδ(ў[i].Ζΰ==a.Θ?a:b)+" gives way",(a.ά+b.ά)/2,Color.Red);}for(int i=0;i<Ć.Count;i++){ǋ d=Ć[i];Щ t=null;for(int k=0;k<ξ.Count;k++) if(ξ[k].Θ==d.Θ) t=ξ[k];if(t!=null) Р.Ļ(t.ά,d.ζ,Color.Blue,0.08f);for(int r=0;r<d.ΓΫ.Count;r++) Р.Ļ(d.ΓΫ[r],d.ΕЃ[r],d.ΕЂ[r]?Color.Red:Color.Green,0.04f);
if(t==null||!d.ΘΛ) continue;ȏ Ι=null;for(int j=0;j<ǘ.Count;j++) if(ǘ[j].э==t.Ι) Ι=ǘ[j];if(Ι==null) continue;int ΗЏ=0;for(int c=0;c<Ι.φ.Length;c++) if(Ι.φ[c]=='1') ΗЏ++;if(i==0||Ć[i-1].Αν!=d.Αν) Р.ƌ("Job: "+ΗЏ+"/"+Ι.φ.Length+" holes done",d.Αν,Color.White);
for(int c=0;c<Ι.φ.Length;c++){int ΕΑ=c%Ι.w,ō=c/Ι.w;Vector3 p=d.Αν+d.ŭ*((Ι.w-1f)/2f-ΕΑ)*Ι.ΖΥ-d.ș*((Ι.h-1f)/2f-ō)*Ι.ΖΘ;ȱ Εύ=ΔΕ(Ι,c,null);Р.ΚΨ(p,Ι.φ[c]=='1'?Color.Gray:Εύ!=null?Color.Orange:Color.White,Math.Min(Ι.ΖΥ,Ι.ΖΘ)*0.2f);if(Εύ==null||Εύ.Θ!=d.Θ) continue;
Р.Ļ(t.ά,p,Color.Orange,0.05f);Р.ƌ("Hole "+(Ι.ΓΨ.IndexOf(c)+1)+": "+Βδ(t),p,Color.Orange);}}for(int i=0;i<Ȏ.Count;i++){IMySensorBlock z=Ȏ[i];Р.ΗΥ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);
Р.ƌ("Docking area"+(ġ!=""?": "+ġ:""),z.GetPosition(),Color.Cyan);}for(int i=0;i<ī.Count;i++)
Р.Ļ(ī[i].GetPosition(),ī[i].GetPosition()+ī[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(ї!=0&&Βυ!=0){IMyShipConnector c=Δο(Βυ);for(int k=0;k<ξ.Count;k++)
if(c!=null&&ξ[k].Θ==ї){Р.Ļ(ξ[k].ά,c.GetPosition(),Color.Lime,0.15f);Р.ƌ("Cleared to "+(Βφ==2?"undock: ":"dock: ")+Βδ(ξ[k]),c.GetPosition(),Color.Lime);}}}String Βδ(Щ t){ϋ Ћ=null;for(int i=0;i<Ϗ.Count;i++) if(t.Ŵ!=0&&Ϗ[i].Θ==""+t.Ŵ) return Ϗ[i].ΰ;for(int i=0;i<Ϗ.Count;i++)
if(Vector3.Distance(Ϗ[i].Ε,t.ά)<30&&(Ћ==null||
Vector3.Distance(Ϗ[i].Ε,t.ά)<Vector3.Distance(Ћ.Ε,t.ά))) Ћ=Ϗ[i];if(Ћ!=null&&Ћ.ΰ!="") return Ћ.ΰ;String Θ=t.Θ.ToString();return "Ship "+Θ.Substring(Math.Max(0,Θ.Length-4));}String Ιώ(){if(!debugDraw) return "";if(Р==null||!Р.ΔΥ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}