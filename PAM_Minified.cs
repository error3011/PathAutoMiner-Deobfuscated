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
public enum Ι{Ĕ,Ɲ,ǟ,ё,ά}public enum Β{о,đ,ƞ,Ȫ,Į,Е,н,
Đ,ŗ,ǵ,ī,ħ,Č,
Ơ,Ȗ,Ǣ,Ī,Αϋ,Αί,
Γς,Αϊ,Ɩ,Αα,ĕ}public enum Π{ч,Ŝ,Д,ΒЄ,ΑΫ,Αλ,Αμ,Œ,œ}public enum ѐ{None,Ī,о,Εί}public enum Ρ{Main,ΕІ,Ɯ,ƛ,Γξ,ΑΩ,
Βσ,Ες,Εσ,Ερ,Ƣ,ΕЋ,
Ατ,Γε,Γι,ΕЌ,Αφ}public enum р{Ασ,Ɣ,Θπ,Εφ,ά}public enum Ǐ{Ƥ,ΕΨ}public enum х{Ας,ΒЊ,ΕЏ,Ƙ}public enum Ѝ{ΓΤ,Εά,ΒϏ,ΒЅ,Αέ,ΓΓ,Αη}public enum ў{ΒЁ,Βψ,Βω}public enum Х{Ś,ΘΡ,Ĩ}public enum π{ΓΦ,ΓΪ,ΓΫ,ΕЁ,ΕЀ,
ΓΥ,Αο,Αξ,ΓΨ,
ΓΩ,ΓΧ}public enum ΰ{Ȑ,ȩ,ȏ,ΑΨ,Βφ,Βχ}public enum ǡ{Αδ,ΑΧ,ΗЎ}public enum ђ{Αχ,Εα,ΘΫ,ΓΡ}public enum О{None,Ɵ,Γΰ,ΓΣ}public enum ϊ{Ʀ,ΑΦ,Αι,Ƙ,ΓΟ,Ακ,ΓΠ,Θυ,Γά}public enum П{Ĕ,Εώ,Εύ,ΓΜ,ΓΕ,ΒЀ,ΕϏ}class Ϊ{public bool ζ=false;public Vector3 Ε=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 Ѐ=new Vector3();public Vector3 φ=new Vector3();public Vector3 и=new Vector3();public float Ф=0;public float Κθ=0;public float[]ǳ=null;public Ϊ(){}public Ϊ(Ϊ ι){ζ=ι.ζ;Ε=ι.Ε;β=ι.β;γ=ι.γ;Ѐ=ι.Ѐ;φ=ι.φ;
и=ι.и;ǳ=ι.ǳ;}public Ϊ(Vector3 Ε,Vector3 γ,Vector3 β,Vector3 Ѐ,Vector3 φ){this.Ε=Ε;this.β=β;this.γ=γ;this.Ѐ=Ѐ;this.Ф=0;this.φ=φ;}public void Ηϊ(List<IMyThrust>Αώ,List<string>Ȣ){ǳ=new float[Ȣ.Count];for(int i=0;i<ǳ.Length;i++)
ǳ[i]=-1;for(int i=0;i<Αώ.Count;i++){string ѝ=Εο(Αώ[i]);int Α=Ȣ.IndexOf(ѝ);if(Α!=-1)
ǳ[Α]=Б(Αώ[i].MaxEffectiveThrust,Αώ[i].MaxThrust);}}}class Γέ{public Ϊ λ=null;public List<Vector3>Ƌ=new List<Vector3>();public float Ʋ=0;public float ΕΕ=0;public float Ť=0;public float ΔΕ=0;public Vector3 ΒΑ=new Vector3();}class Š{public π Н=π.ΓΦ;public float Ǧ=0;public float Ĝ=0;
public string ǧ="";public string Ǫ="";DateTime ΖЈ;public bool Δυ=false;private bool ΓЊ=false;public bool Βό(bool ǰ){if(ǰ){Ĝ=0;ΓЊ=false;return false;}ΓЊ=true;return Ĝ>Ǧ;}public void Θα(){Βό(true);Δυ=false;}public void ΘЄ(){if(ΓЊ)
if((DateTime.Now-ΖЈ).TotalSeconds>1){Ĝ++;ΖЈ=DateTime.Now;}}public bool ΒБ(){switch(Н){case π.ΓΪ:return true;case π.ΓΫ:return true;}return false;}}class е{public String ή="";public String υ="";public int Ѕ=0;public Х ŵ=Х.Ĩ;public е(String ή,String υ,int Ѕ,Х ŵ){this.ή=ή;this.υ=υ;
this.Ѕ=Ѕ;this.ŵ=ŵ;}}class ΗЉ{public ΗЉ(Vector3 ξ,float І){this.ξ=ξ;this.І=І;}public Vector3 ξ;public float І;}class К{public DateTime ΔΪ;public DateTime Ζά;public String δ="";public String ή="";public String Βπ="";public String Λ="";public String ΔЅ="";public String ΒΨ="";public String Δσ="";
public Vector3 Ε=new Vector3();public List<е>Ї=new List<е>();public Ι Κ=Ι.Ĕ;public О ė;public float Ȅ;public float Ǿ;public int Љ;public int ΓЏ=0;public int ΔБ=0;public bool Εϋ(){return(DateTime.Now-ΔΪ).TotalSeconds>10;}public bool ΓΖ(){if(ė!=О.Ɵ) return false;return(DateTime.Now-Ζά).TotalSeconds>=4;
}public К(String δ){this.δ=δ;}public О ŝ(String Ж,bool ΙЍ,bool ш,bool ΙΒ){if(Ж==""&&!ш) return О.None;if(ė==О.Ɵ&&ΓΖ())
ė=О.ΓΣ;if(ΙЍ){Δσ=Ж;ė=О.Ɵ;Ζά=DateTime.Now;return ė;}else if(ш){Δσ="";ė=О.None;}else if(Δσ==Ж){if(ΙΒ) ė=О.Γΰ;return ė;}return О.None;}}public String ΘΔ(String Ιθ){switch(Ιθ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Βί=new Vector3();Vector3 ε=new Vector3();Vector3 ΒΔ=new Vector3();Vector3 ǆ=new Vector3();Vector3 Ƶ=new Vector3();
DateTime Δέ=new DateTime();bool ȯ=true;int ΔЋ=0;bool ȳ=false;bool ǂ=false;bool ǋ=true;bool Ητ=false;bool ű=false;bool ΒΙ=false;float Ć=0;float Ō=0;int ģ=0;int ΑΞ=0;int ΑΓ=0;int ΓЃ=0;float Ÿ=0;float ť=0;double Δι=0;float ƫ=0;List<int>Ů=new List<int>();List<int>ƃ=new List<int>();
Ι Κ=Ι.Ĕ;int Ћ=0;float ȍ=0;float Ȍ=0;float ο=0;float Δώ=0,Δύ=0;IMyRemoteControl Ψ;IMySensorBlock ψ;List<IMyTimerBlock>ΔЎ=new List<IMyTimerBlock>();List<IMyShipConnector>ѓ=new List<IMyShipConnector>();List<IMyThrust>ȣ=new List<IMyThrust>();List<IMyGyro>ΒΕ=new List<IMyGyro>();
List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();List<IMyLandingGear>Ƞ=new List<IMyLandingGear>();List<IMyReactor>Ģ=new List<IMyReactor>();List<IMyConveyorSorter>Βδ=new List<IMyConveyorSorter>();List<IMyGasTank>ǒ=new List<IMyGasTank>();List<IMyTerminalBlock>Ƨ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ũ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ĵ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ė=new List<IMyBatteryBlock>();List<IMyTextPanel>У=new List<IMyTextPanel>();List<IMyTextSurface>ȉ=new List<IMyTextSurface>();List<IMyTextPanel>ķ=new List<IMyTextPanel>();
IMyTerminalBlock ǖ=null;Ϊ Ξ=new Ϊ();Ϊ ΔΤ=new Ϊ();Ϊ ΔΧ=new Ϊ();bool Ȥ=false;bool ƍ=false;bool Ǝ=false;bool ΒΛ=false;List<String>Ȣ=new List<string>();List<Ϊ>Φ=new List<Ϊ>();List<Ϊ>ΑΤ=new List<Ϊ>();int ΓА=0;double Ηυ=0;Ϊ Ζ=new Ϊ();Vector3 ǭ;Vector3 ă;Π α=Π.ч;Ǐ Ŀ=Ǐ.Ƥ;int ν=0;
int И=0;double Ǆ=0;bool ΑЉ=false;Š Ѓ=new Š();Š Є=new Š();х ь=х.Ας;bool ĥ=true;Ǐ ȇ=Ǐ.Ƥ;int ъ=3;int щ=3;int з=30;ў є=ў.ΒЁ;bool ċ=true;bool Ű=true;bool Ū=true;bool ō=false;Ѝ Ю=Ѝ.ΓΤ;bool Ċ=true;float ȃ=90;float ј=20;float к=5;float љ=20;float Ǹ=0.70f;float ƒ=1.50f;
float Ƒ=2.50f;float ő=0f;float ļ=0f;Β Ω;Π ї;Β ΑΗ;Γέ κ=null;int ώ=0;int ό=0;int Ь=0;int ā=0;int Ŋ=30;int г=0;bool Ў=true;Vector3 њ;double Э=0;bool Βα=false;int ΕΙ=0;int Џ=0;int Γω=0;int ȅ=0;int ΗΜ=0;Vector3 ΑΑ=new Vector3();float ы=0;float м=0;int Ő=0;Ϊ Βε=null;
float ΔΫ=0;float ΑЀ=0;float в=0;float ł=0;bool ΔΥ=false;bool Ŏ=false;bool ΑΟ=false;bool ȱ=false;bool Ȭ=false;DateTime ΙΥ=new DateTime();Ϊ ĵ=null;Ϊ Ζα=null;float Ș=0;ѐ ǹ;float Ȳ=0;float ŏ=0;String Δδ="";float ΒΞ=0;float Ǿ=0;float Ȅ=0;List<е>ǩ=new List<е>();float Я=0;
bool ΑΕ=false;bool Ηι=false;bool ΗΨ=false;bool Βθ=false;bool ΗΩ=false;float Δη=0;float Δς=0;Vector3 Ζτ=new Vector3();Vector3 Ђ=new Vector3();Vector3 Βι=new Vector3(1,1,1);float ΔЊ=1;Vector3 ΑΛ=new Vector3();bool ΒΚ=false;bool ΔΛ=false;bool Δα=false;bool ΖΘ=false;float ΑЇ=0;
float Αύ=2;Vector3 Ηθ;Vector3 ΔЉ;Vector3 ΔЈ;float[,]Ǵ=new float[3,2];Dictionary<String,float[,]>ǚ=new Dictionary<string,float[,]>();Dictionary<String,float>Ης=new Dictionary<String,float>();int Γύ=0;Dictionary<String,float[]>Ą=new Dictionary<string,float[]>();float Δφ;bool ΑЊ=false;
int ΑϏ=0;int ğ=0;int ΒΗ=0;int Ľ=0;String Λ="";int[]ΗΔ=new int[Enum.GetValues(typeof(Ρ)).Length];bool Δλ=false;int Ń=0;int Љ=0;Ρ ϋ=Ρ.Main;int ΚΚ=0,ΚΛ=0,ΚΜ=0,ΚΝ=0;int ΙЈ=0,ΙЉ=0;bool ŧ=false;bool ΖΙ=false;String ƈ="";String ΓЂ="";IMyBroadcastListener Ǥ=null;
bool Ζσ=true;List<К>Ы=null;К Г=null;int ΑЄ=0,ΑЅ=0;int Ζί=0,Ζΰ=0;int ΙΘ=0,ΙΙ=0,ΙΚ=0;String Ικ="";Exception Ā=null;bool Βΰ=false;Π ƅ=Π.ч;bool Ζδ=false;bool ΖЅ=true;bool Δϋ=false;String Ησ="";bool ΑΝ=false;bool ΔЃ=true;int ΒΘ=0;int[]ǘ=null;Vector3 ΓЄ=new Vector3();
bool Ζψ=false;bool џ(IMyTerminalBlock Χ)=>Χ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓΑ(Me,controllerTag,true))
Κ=Ι.ё;θ(Ρ.Main);if(Κ!=Ι.ё){Λ="Welcome to [PAM]!";ђ Δϊ=Θβ();if(Κ==Ι.Ĕ){List<IMyShipDrill>Ζπ=new List<IMyShipDrill>();List<IMyShipGrinder>Ζω=new List<IMyShipGrinder>();χ.GetBlocksOfType(Ζπ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(Ζω,q=>q.CubeGrid==Me.CubeGrid);if(Ζπ.Count>0)
{Κ=Ι.Ɲ;Λ="Miner mode enabled!";}else if(Ζω.Count>0){Κ=Ι.ǟ;Λ="Grinder mode enabled!";}else{Κ=Ι.ά;Ȕ(р.ά);}}if(Δϊ==ђ.Αχ) ǋ=false;if(Δϊ==ђ.Εα) Λ="Data restore failed!";if(Δϊ==ђ.ΓΡ) Λ="New version, wipe data";}}void Main(string Р,UpdateType ΚΦ){try{if(Ā!=null){if(Р.Trim().ToUpper()=="RETRY")
{Ā=null;ȯ=true;ģ=0;if(Κ!=Ι.ё) Ϗ();Λ="Restarted after error";}else{ΘΝ();ΕЄ();return;}}ǂ=(ΚΦ&UpdateType.Update10)!=0;if(ǂ) ΔЋ++;ȳ=ΔЋ>=10;if(ȳ) ΔЋ=0;if(ǂ){ΑϏ++;if(ΑϏ>4) ΑϏ=0;ΑΓ=(int) Math.Max(0,10-(DateTime.Now-Δέ).TotalSeconds);}if(Р!="") Ικ=Р;if(Κ!=Ι.ё)
Θο(Р);else
ΗЇ(Р);Δλ=false;try{int Ζβ=Runtime.CurrentInstructionCount;float ŉ=Б(Ζβ,Runtime.MaxInstructionCount);if(ŉ>0.90) Λ="Max. instructions >90%";if(ŉ>Ÿ) Ÿ=ŉ;if(ΑЊ){Ů.Add(Ζβ);while(Ů.Count>10) Ů.RemoveAt(0);ť=0;for(int i=0;i<Ů.Count;i++) ť+=Ů[i];ť=Б(Б(ť,Ů.Count),Runtime.MaxInstructionCount);
double Δά=Runtime.LastRunTimeMs;if(ΒΙ&&Δά>Δι) Δι=Δά;ƃ.Add((int)(Δά*1000f));while(ƃ.Count>10) ƃ.RemoveAt(0);ƫ=0;for(int i=0;i<ƃ.Count;i++) ƫ+=ƃ[i];ƫ=Б(ƫ,ƃ.Count)/1000f;}}catch{Ÿ=0;}}catch(Exception e){Ā=e;}}void Θο(string Р){bool Ββ=false;String ΖΤ="";if(Ћ<=1&&!ΓΙ(Р))
ΕЅ(Р);int Δμ=50;while(Ǥ!=null&&Ǥ.HasPendingMessage&&Δμ-->0){MyIGCMessage η=Ǥ.AcceptMessage();String Τ=η.Data as String;if(Τ==null) continue;String Ȇ="";if(Ћ<=1&&ΕЃ(ref Τ,out Ȇ,out ΖΤ)&&Ȇ==BROADCAST_SELF_ID){ΕЅ(Τ);Ββ=true;break;}}bool ΖЁ=ΒΙ&&Ω==Β.о&&!ű&&!Ββ&&ģ==0&&!ΒΛ;
if(ȳ&&Ω!=Β.о) Ββ=true;if((ǂ&&!ΖЁ)||(ȳ&&ΖЁ)){if(ģ==0&&(ΑΓ<=0||ȯ)){Ητ=Ћ>0;Ћ=0;ģ=1;ȕ();Θζ();Ετ();ȑ("Scan 1");}else if(ģ==1){ģ=2;ȕ();Θη();ȑ("Scan 2");}else if(ģ==2){ģ=0;ȕ();Θθ();Θι();if(Ћ<=1) Εό();ȑ("Scan 3");Δέ=DateTime.Now;if(Ћ<=1&&ǋ)
Βί=Αε(Ψ,Ψ.CenterOfMass);ǋ=false;if(ȯ){ȯ=false;bool ΚΪ=α==Π.Д;Ϗ();if(ΖЅ&&resumeAfterRestart&&ΚΪ&&Ћ<=1) Δϋ=true;}ΖЅ=false;if(Ητ&&Ћ==0) Λ="Setup complete";}else{if(α==Π.Д&&Κ!=Ι.ά){ȕ();Ηφ();ȑ("Inv balance");}ȕ();switch(ΑΞ){case 0:Θω();break;case 1:ΘϏ();break;case 2:Θψ();break;
case 3:ΘЇ();break;case 4:Θώ();break;case 5:Ηύ();break;case 6:Ηό(Ψ);break;}ȑ("Update: "+ΑΞ);ΑΞ++;if(ΑΞ>6){ΑΞ=0;ΒΙ=true;if(ƅ!=Π.ч){switch(ƅ){case Π.Œ:Ď();break;case Π.œ:Ď();break;case Π.Д:Ď();break;case Π.Αλ:Αβ();break;case Π.Αμ:Ĭ();break;}ƅ=Π.ч;}}}if(!ȯ){if(!ȓ(Ψ,true))
{Ψ=null;ǋ=true;Ћ=2;}if(Ћ>=2&&Ω!=Β.о)
Ϗ();if(Ћ<=1){Ō=Ψ.CalculateShipMass().PhysicalMass;Ć=(float) Ψ.GetShipSpeed();ε=Θχ(Ψ,Βί);ΒΔ=Ψ.WorldMatrix.Forward;ǆ=Ψ.WorldMatrix.Left;Ƶ=Ψ.WorldMatrix.Down;ΘЂ();ΘЅ();Θϋ();if(Δϋ){Δϋ=false;Ď();if(Ω!=Β.о) Λ="Job resumed after restart";}if(Ω!=Β.о){ű=false;ΕЈ(false);ΕΦ(false);String ΚΑ=ΒЍ(Ω)+" "+(int) Ω;
ȕ();ΘЀ();Γρ(false);ȑ(ΚΑ);ΘІ();ΘЁ();ȕ();Θύ();ȑ("Thruster");ȕ();ΖΔ();ȑ("Gyroscope");}else{if(ű){if(Αθ()){д(Ƶ,ΒΔ,ǆ,0.25f,true);ΖΔ();Λ="Aligning to planet: "+Math.Round(ΑЇ-0.25f,2)+"°";if(ΒΚ) ΒА(true,true);}else ΒА(true,true);}}ΑΝ=false;}}}ȕ();ΕЄ();ȑ("Print");if(Ββ||ΓЃ<=0){ȕ();Ηχ(ΖΤ);
ȑ("Broadcast");ΓЃ=4;}else if(ȳ) ΓЃ--;}void ΕЅ(string Р){if(Р=="") return;var τ=Р.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ж=τ.ElementAtOrDefault(0);var ȗ=τ.ElementAtOrDefault(1);var ƪ=τ.ElementAtOrDefault(2);var ΖΛ=τ.ElementAtOrDefault(3);String ȟ="Invalid argument: "+Р;bool ΕΔ=false;
switch(Ж){case "UP":this.ΓΞ(false);break;case "DOWN":this.ΓΝ(false);break;case "UPLOOP":this.ΓΞ(true);break;case "DOWNLOOP":this.ΓΝ(true);break;case "APPLY":this.ΕΤ(true);break;case "MRES":Љ=0;break;case "STOP":this.Ϗ();break;case "PATHHOME":{this.Ϗ();this.Γν();}break;case "PATH":{this.Ϗ();this.Γν();Ξ.ζ=true;}break;
case "START":{this.Ϗ();Γμ();}break;case "ALIGN":{ΒА(!ű,false);}break;case "CONT":{this.Ϗ();this.Ď();}break;case "JOBPOS":{this.Ϗ();this.Ĭ();}break;case "HOMEPOS":{this.Ϗ();this.Αβ();}break;case "FULL":{ΑΕ=true;}break;case "RESET":{Βΰ=true;Ћ=2;}break;case "EXPORT":{ΗЏ();}break;
case "IMPORT":{ΘΟ(ȗ=="JOB");}break;default:ΕΔ=true;break;}if(Κ!=Ι.ά){switch(Ж){case "SHUTTLE":{ΗЌ();}break;case "CFGS":{if(!ΗЅ(ȗ,ƪ,ΖΛ)) Λ=ȟ;}break;case "CFGB":{if(!ΗЄ(ȗ,ƪ)) Λ=ȟ;}break;case "CFGL":{if(!ĭ(ref ȃ,true,ϊ.Αι,ȗ,"")||!ΗІ(ƪ))
Λ=ȟ;}break;case "CFGE":{if(!ĭ(ref к,true,ϊ.Ακ,ȗ,"IG")||
!ĭ(ref ј,true,ϊ.ΓΟ,ƪ,"IG")||
!ĭ(ref љ,true,ϊ.ΓΠ,ΖΛ,"IG"))
Λ=ȟ;}break;case "CFGA":{if(!ĭ(ref Ǹ,false,ϊ.ΑΦ,ȗ,"")) Λ=ȟ;}break;case "CFGW":{if(!ĭ(ref ƒ,false,ϊ.Ʀ,ȗ,"")||
!ĭ(ref Ƒ,false,ϊ.Ʀ,ƪ,""))
Λ=ȟ;}break;case "NEXT":{Βς(false);}break;case "PREV":{Βς(true);}break;default:if(ΕΔ) Λ=ȟ;break;}}else{switch(Ж){case "UNDOCK":{ΑΝ=true;}break;default:if(ΕΔ) Λ=ȟ;break;}}}String ΘΕ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Κ!=Ι.ά)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗЌ(){Ϗ();Κ=Ι.ά;Ȕ(р.ά);Ζ.ζ=false;Ξ.ζ=false;ψ=null;ω.Clear();α=Π.ч;}void ΒА(bool З,bool Ǒ){if(!З) Λ="Aligning canceled";if(Ǒ) Λ="Aligning done";if(Ǒ||!З){ű=false;ΕА();Αυ(false,0,0,0,0);return;}if(Αθ()) ű=true;}bool ΗЅ(String Κά,String Ιΰ,String ΙΟ)
{bool ΚΨ=α==Π.Д;int w,h,d;if(int.TryParse(Κά,out w)&&int.TryParse(Ιΰ,out h)&&int.TryParse(ΙΟ,out d)){this.Ϗ();ъ=w;щ=h;з=d;Ĳ(false);ƥ(false,false);if(ΚΨ) Ď();return true;}return false;}bool ΗІ(String Ʃ){if(Ʃ=="ON"){ċ=true;return true;}if(Ʃ=="OFF"){ċ=false;return true;}
return false;}bool ΗЄ(String Ζο,String ΑЈ){bool Ņ=true;if(Ζο=="HOME") ĥ=true;else if(Ζο=="STOP") ĥ=false;else Ņ=false;if(ΑЈ=="HOME") ь=х.Ας;else if(ΑЈ=="STOP") ь=х.ΕЏ;else if(ΑЈ=="JOB") ь=х.ΒЊ;else if(ΑЈ=="IG") ь=х.Ƙ;else Ņ=false;return Ņ;}void θ(Ρ с){ΗΔ[(int) ϋ]=Љ;
Љ=ΗΔ[(int) с];if(с==Ρ.Ƣ) Љ=0;ϋ=с;if(Κ!=Ι.ё)
ı(ϋ==Ρ.Ɯ,false,0,0);Δλ=true;}void ΓΞ(bool Δβ){if(Љ>0) Љ--;else if(Δβ) Љ=Ń-1;}void ΓΝ(bool Δβ){if(Љ<Ń-1) Љ++;else if(Δβ) Љ=0;}String Ȕ(р η){switch(η){case р.Ασ:Λ="Job is running";break;case р.Ɣ:Λ="Connector not ready!";break;case р.Θπ:Λ="Ship modified, path outdated!";break;
case р.Εφ:Λ="Interrupted by player!";break;case р.ά:Λ="Shuttle mode enabled!";break;}return "";}String ΘΚ(Ǐ ξ){switch(ξ){case Ǐ.Ƥ:return "Top-Left";case Ǐ.ΕΨ:return "Center";default:return "";}}String ΘΑ(ў Њ){switch(Њ){case ў.Βψ:return "Auto"+(Κ==Ι.Ɲ?" (Ore)":"");case ў.Βω:return "Auto (+Stone)";
case ў.ΒЁ:return "Default";default:return "";}}String ΒЋ(Π Ё){switch(Ё){case Π.ч:return "No job";case Π.Ŝ:return "Job paused";case Π.Д:return "Job active";case Π.Œ:return "Job active";case Π.œ:return "Job active";case Π.ΒЄ:return "Job done";case Π.ΑΫ:return "Job changed";case Π.Αλ:return "Move home";
case Π.Αμ:return "Move to job";}return "";}String Εζ(х ΙΑ){switch(ΙΑ){case х.Ας:return "Return home";case х.ΒЊ:return "Fly to job pos";case х.ΕЏ:return "Stop";case х.Ƙ:return "Ignore";}return "";}String ΘΓ(Ѝ Њ){switch(Њ){case Ѝ.ΓΤ:return "Off";case Ѝ.ΒЅ:return "Drop pos (Stone) ";case Ѝ.Αέ:return "Drop pos (Sto.+Ice)";
case Ѝ.Εά:return "Cur. pos (Stone)";case Ѝ.ΒϏ:return "Cur. pos (Sto.+Ice)";case Ѝ.ΓΓ:return "In motion (Stone)";case Ѝ.Αη:return "In motion (Sto.+Ice)";}return "";}String Εδ(ѐ Ё){switch(Ё){case ѐ.None:return "No batteries";case ѐ.Ī:return "Charging";case ѐ.Εί:return "Discharging";}return "";}
String ΒЍ(Β Ё){String Κζ=Κ==Ι.ά?"target":"job";switch(Ё){case Β.о:return "Idle";case Β.đ:return "Flying to XY position";case Β.ƞ:return Κ==Ι.ǟ?"Grinding":"Mining";case Β.Ȫ:return "Returning";case Β.Ɩ:return "Flying to drop pos";case Β.Į:return "Returning to dock";case Β.Е:return "Flying to dock area";
case Β.н:return "Flying to job area";case Β.Đ:return "Flying to path";case Β.ŗ:return "Flying to job position";case Β.ǵ:return "Approaching dock";case Β.ī:return "Docking";case Β.ħ:return "Aligning";case Β.Č:return "Aligning";case Β.Ơ:return "Retry docking";case Β.Ȗ:return "Unloading";case Β.ĕ:return Ησ;
case Β.Ǣ:return "Undocking";case Β.Ī:return "Charging batteries";case Β.Αϋ:return "Waiting for uranium";case Β.Αί:return "Filling up hydrogen";case Β.Γς:return "Waiting for ejection";case Β.Αϊ:return "Waiting for ejection";case Β.Αα:return "Flying to drop pos";}return "";}String Επ(π Н){switch(Н)
{case π.ΓΦ:return "On \"Undock\" command";case π.ΓΪ:return "On player entered cockpit";case π.ΕЁ:return "On ship is full";case π.ΕЀ:return "On ship is empty";case π.ΓΫ:return "On time delay";case π.Αο:return "On batteries empty(<25%)";case π.Αξ:return "On batteries empty(=0%)";case π.ΓΥ:return "On batteries full";
case π.ΓΩ:return "On hydrogen empty(<25%)";case π.ΓΧ:return "On hydrogen empty(=0%)";case π.ΓΨ:return "On hydrogen full";}return "";}bool Μ(ref String Γ,int ΗΥ,int Ğ,bool Θ,String ŀ){Ń+=1;if(ΗΥ==Ğ) ŀ=">"+ŀ+(ΑϏ>=2?" .":"");else ŀ=" "+ŀ;Γ+=ŀ+"\n";return ΗΥ==Ğ&&Θ;}String ΕΤ(bool Θ)
{int Α=0;int Η=Љ;Ń=0;String Ƈ="———————————————\n";String ƌ="--------------------------------------------\n";String Γ="";Γ+=ΒЋ(α)+" | "+(Ξ.ζ?"Ready to dock":"No dock")+"\n";Γ+=Ƈ;double Ȉ=Math.Max(Math.Round(this.Э),0);if(ϋ==Ρ.Main){bool Ġ=Κ==Ι.ά;if(Μ(ref Γ,Η,Α++,Θ," Record path & set home")) Γν();
if(Κ==Ι.Ɲ)
if(Μ(ref Γ,Η,Α++,Θ," Setup mining job")) θ(Ρ.Ɯ);if(Κ==Ι.ǟ)
if(Μ(ref Γ,Η,Α++,Θ," Setup grinding job")) θ(Ρ.Ɯ);if(Κ==Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," Setup shuttle job")) θ(Ρ.Γι);if(Μ(ref Γ,Η,Α++,Θ," Continue job")) Ď();if(Μ(ref Γ,Η,Α++,Θ," Fly to home position")) Αβ();if(Μ(ref Γ,Η,Α++,Θ," Fly to job position")) Ĭ();if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(Ġ) θ(Ρ.Αφ);else θ(Ρ.ΑΩ);if(Μ(ref Γ,Η,Α++,Θ," Info")) θ(Ρ.Ες);if(Κ!=Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," Help")) θ(Ρ.Ερ);}else if(ϋ==Ρ.Ɯ){double ΚΫ=Math.Round(ъ*ȍ,1);double Ιί=Math.Round(щ*Ȍ,1);String А="";if(Μ(ref А,Η,Α++,Θ," Start new job!")) Γμ();if(Μ(ref А,Η,Α++,Θ," Change current job")){ƥ(false,false);θ(Ρ.Main);}if(Μ(ref А,Η,Α++,Θ," Width + (Width: "+ъ+" = "+ΚΫ+"m)")){Ŕ(ref ъ,5,20,1);Ĳ(true);}
if(Μ(ref А,Η,Α++,Θ," Width -")){Ŕ(ref ъ,-5,20,-1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Height + (Height: "+щ+" = "+Ιί+"m)")){Ŕ(ref щ,5,20,1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Height -")){Ŕ(ref щ,-5,20,-1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Depth + ("+(є==ў.ΒЁ?"Depth":"Min")+": "+з+"m)")){Ŕ(ref з,5,50,2);Ĳ(true);}
if(Μ(ref А,Η,Α++,Θ," Depth -")){Ŕ(ref з,-5,50,-2);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Depth mode: "+ΘΑ(є))){є=ď(є);}if(Μ(ref А,Η,Α++,Θ," Start pos: "+ΘΚ(ȇ))){ȇ=ď(ȇ);}if(Κ==Ι.ǟ&&є==ў.Βω) є=ď(є);Γ+=Γγ(8,А,Η,ref ΙЈ);}else if(ϋ==Ρ.Γι){float[]Ζζ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Μ(ref Γ,Η,Α++,Θ," Next")) θ(Ρ.ΕЌ);if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=" Leave connector 1:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+Επ(Ѓ.Н))) Ѓ.Н=ď(Ѓ.Н);if(!Ѓ.ΒБ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αγ((int) Ѓ.Ǧ))) Ѓ.Ǧ=Εέ(Ѓ.Ǧ,Ζζ);Γ+=" Leave connector 2:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+Επ(Є.Н))) Є.Н=ď(Є.Н);
if(!Є.ΒБ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αγ((int) Є.Ǧ))) Є.Ǧ=Εέ(Є.Ǧ,Ζζ);}else if(ϋ==Ρ.ΕЌ){if(Μ(ref Γ,Η,Α++,Θ," Start job!")) Γμ();if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Γι);Γ+=" Timer: \"Docking connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ѓ.ǧ!=""?Ѓ.ǧ:"-"))) Ѓ.ǧ=Αν(ref ΚΚ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ѓ.Ǫ!=""?Ѓ.Ǫ:"-"))) Ѓ.Ǫ=Αν(ref ΚΜ);Γ+=" Timer: \"Docking connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Є.ǧ!=""?Є.ǧ:"-"))) Є.ǧ=Αν(ref ΚΛ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Є.Ǫ!=""?Є.Ǫ:"-"))) Є.Ǫ=Αν(ref ΚΝ);
}else if(ϋ==Ρ.ƛ){String ΖЌ=ȃ+" %";if(ȳ) ğ++;if(ğ>1){ğ=0;ΒΗ++;if(ΒΗ>1) ΒΗ=0;}bool[]ΔЀ=new bool[]{Ģ.Count==0,ǹ==ѐ.None,ǒ.Count==0};int Ηξ=0;while(true){Ηξ++;Ľ++;if(Ľ>ΔЀ.Length-1) Ľ=0;if(Ηξ>=ΔЀ.Length) break;if(!ΔЀ[Ľ]) break;}bool Ġ=Κ==Ι.ά;if(!Ġ&&ċ&&Я!=-1&&ΒΗ==0)
ΖЌ=Я<1000000?Math.Round(Я)+" Kg":Math.Round(Я/1000)+" t";if(Μ(ref Γ,Η,Α++,Θ," Stop!")){Ϗ();θ(Ρ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!Ġ) θ(Ρ.ΑΩ);else θ(Ρ.Αφ);if(!Ġ){if(Μ(ref Γ,Η,Α++,Θ," Next hole")) Βς(false);}else if(Μ(ref Γ,Η,Α++,Θ," Undock")) ΑΝ=true;Γ+=ƌ;if(!Ġ) Γ+="Progress: "+Math.Round(Ǆ,1)+" %\n";Γ+="State: "+ΒЍ(Ω)+" "+Ȉ+"m \n";Γ+="Load: "+ΒΞ+" % Max: "+ΖЌ+" \n";if(Ľ==0) Γ+="Uranium: "+(Ģ.Count==0?"No reactors":Math.Round(ŏ,1)+"Kg "+(к==-1?"":" Min: "+к+" Kg"))+"\n";
if(Ľ==1) Γ+="Battery: "+(ǹ==ѐ.None?Εδ(ǹ):Ș+"% "+(ј==-1||Ġ?"":" Min: "+ј+" %"))+"\n";if(Ľ==2) Γ+="Hydrogen: "+(ǒ.Count==0?"No tanks":Math.Round(Ȳ,1)+"% "+(љ==-1||Ġ?"":" Min: "+љ+" %"))+"\n";}else if(ϋ==Ρ.ΑΩ){String А="";if(Μ(ref А,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƛ);else θ(Ρ.Main);}
if(Μ(ref А,Η,Α++,Θ," Max load: "+ȃ+"%")) ф(ref ȃ,ȃ<=80?-10:-5,ϊ.Αι,false);if(Μ(ref А,Η,Α++,Θ," Weight limit: "+(ċ?"On":"Off"))) ċ=!ċ;if(Μ(ref А,Η,Α++,Θ," Ejection: "+ΘΓ(Ю))) Ю=ď(Ю);if(Μ(ref А,Η,Α++,Θ," Toggle sorters: "+(ō?"On":"Off"))){ō=!ō;if(ō) Ƿ(ΔЃ);}
if(Μ(ref А,Η,Α++,Θ," Unload ice: "+(Ċ?"On":"Off"))) Ċ=!Ċ;if(Μ(ref А,Η,Α++,Θ," Uranium: "+(к==-1?"Ignore":"Min "+к+"Kg"))) ф(ref к,(к>5?-5:-1),ϊ.Ακ,true);if(Μ(ref А,Η,Α++,Θ," Battery: "+(ј==-1?"Ignore":"Min "+ј+"%"))) ф(ref ј,-5,ϊ.ΓΟ,true);if(Μ(ref А,Η,Α++,Θ," Hydrogen: "+(љ==-1?"Ignore":"Min "+љ+"%"))) ф(ref љ,-10,ϊ.ΓΠ,true);
if(Μ(ref А,Η,Α++,Θ," When done: "+(ĥ?"Return home":"Stop"))) ĥ=!ĥ;if(Μ(ref А,Η,Α++,Θ," On damage: "+Εζ(ь))) ь=ď(ь);if(Μ(ref А,Η,Α++,Θ," Advanced...")) θ(Ρ.Βσ);Γ+=Γγ(8,А,Η,ref ΙЉ);}else if(ϋ==Ρ.Βσ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƛ);else θ(Ρ.Main);}
if(Μ(ref Γ,Η,Α++,Θ,(Κ==Ι.ǟ?" Grinder":" Drill")+" inv. balancing: "+(Ű?"On":"Off"))) Ű=!Ű;if(Μ(ref Γ,Η,Α++,Θ," Enable"+(Κ==Ι.ǟ?" grinders":" drills")+": "+(Ū?"Fwd + Bwd":"Fwd"))) Ū=!Ū;if(Μ(ref Γ,Η,Α++,Θ," Work speed fwd.: "+ƒ+"m/s")) ф(ref ƒ,0.5f,ϊ.Ʀ,false);
if(Μ(ref Γ,Η,Α++,Θ," Work speed bwd.: "+Ƒ+"m/s")) ф(ref Ƒ,0.5f,ϊ.Ʀ,false);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(Ǹ*100f)+"%"+(Ǹ>0.80f?" (risky)":""))) ф(ref Ǹ,0.1f,ϊ.ΑΦ,false);if(Μ(ref Γ,Η,Α++,Θ," Width overlap: "+ő*100f+"%")) ΕΩ(true,0.05f);if(Μ(ref Γ,Η,Α++,Θ," Height overlap: "+ļ*100f+"%")) ΕΩ(false,0.05f);
}else if(ϋ==Ρ.Αφ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƛ);else θ(Ρ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Max load: "+ȃ+"%")) ф(ref ȃ,ȃ<=80?-10:-5,ϊ.Αι,false);if(Μ(ref Γ,Η,Α++,Θ," Unload ice: "+(Ċ?"On":"Off"))) Ċ=!Ċ;if(Μ(ref Γ,Η,Α++,Θ," Uranium: "+(к==-1?"Ignore":"Min "+к+"Kg"))) ф(ref к,(к>5?-5:-1),ϊ.Ακ,true);
if(Μ(ref Γ,Η,Α++,Θ," Battery: "+(ј==-1?"Ignore":"Charge up"))) ј=(ј==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," Hydrogen: "+(љ==-1?"Ignore":"Fill up"))) љ=(љ==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," On damage: "+Εζ(ь))) ь=ď(ь);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(Ǹ*100f)+"%"+(Ǹ>0.80f?" (risky)":""))) ф(ref Ǹ,0.1f,ϊ.ΑΦ,false);
}else if(ϋ==Ρ.ΕІ){double Ιλ=0;if(Φ.Count>0) Ιλ=Vector3.Distance(Φ.Last().Ε,ε);if(Μ(ref Γ,Η,Α++,Θ," Stop path recording")) ΕБ();if(Κ!=Ι.ά){if(Μ(ref Γ,Η,Α++,Θ," Home: "+(Ȥ?"Use old home":(Ξ.ζ?"Was set! ":"none ")))) Ȥ=!Ȥ;}else{if(Μ(ref Γ,Η,Α++,Θ," Connector 1: "+(Ȥ?"Use old connector":(Ξ.ζ?"Was set! ":"none ")))) Ȥ=!Ȥ;
if(Μ(ref Γ,Η,Α++,Θ," Connector 2: "+(ƍ?"Use old connector":(Ζ.ζ?"Was set! ":"none ")))) ƍ=!ƍ;}if(Μ(ref Γ,Η,Α++,Θ," Path: "+(Ǝ?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) Ǝ=!Ǝ;Γ+=ƌ;Γ+="Wp spacing: "+Math.Round(Ηυ)+"m\n";}else if(ϋ==Ρ.Γξ){if(Μ(ref Γ,Η,Α++,Θ," Stop")){Ϗ();θ(Ρ.Main);}
Γ+=ƌ;Γ+="State: "+ΒЍ(Ω)+" \n";Γ+="Speed: "+Math.Round(Ć,1)+"m/s\n";Γ+="Target dist: "+Ȉ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+ΕΙ+"\n";}else if(ϋ==Ρ.Ες){List<IMyTerminalBlock>Ķ=Εη();if(ȳ) ğ++;if(ğ>=Ķ.Count) ğ=0;if(Μ(ref Γ,Η,Α++,Θ," Next")) θ(Ρ.Εσ);
Γ+=ƌ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΒΞ,1)+"% "+Math.Round(Ǿ,1)+" / "+Math.Round(Ȅ,1)+"\n";Γ+="Uranium: "+(Ģ.Count==0?"No reactors":Math.Round(ŏ,1)+"Kg "+Δδ)+"\n";Γ+="Battery: "+(ǹ==ѐ.None?"":Ș+"% ")+Εδ(ǹ)+"\n";Γ+="Hydrogen: "+(ǒ.Count==0?"No tanks":Math.Round(Ȳ,1)+"% ")+"\n";
Γ+="Damage: "+(Ķ.Count==0?"None":""+(ğ+1)+"/"+Ķ.Count+" "+Ķ[ğ].CustomName)+"\n";}else if(ϋ==Ρ.Εσ){if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=ƌ;Γ+="Next scan: "+ΑΓ+"s\n";Γ+="Drill area: "+Math.Round(ȍ,1)+"x"+Math.Round(Ȍ,1)+"m Ship: "+Math.Round(Δώ,1)+"x"+Math.Round(Δύ,1)+"m ("+Math.Round(ο,1)+"m)\n";
Γ+="Broadcast: "+(ŧ?"Online - "+ƈ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ÿ*100f,1)+"% \n";}else if(ϋ==Ρ.Ερ){if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=ƌ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Ћ==2) Γ="Fatal setup error\nNext scan: "+ΑΓ+"s\n";if(Βΰ) Γ="Recompile script now";int Ιμ=Γ.Split('\n').Length;for(int i=Ιμ;i<=10;i++) Γ+="\n";Γ+=Ƈ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕΩ(bool Ιη,float Ǚ){Ϗ();ƥ(true,false);if(Ιη) ф(ref ő,Ǚ,ϊ.Γά,false);else ф(ref ļ,Ǚ,ϊ.Γά,false);ΕΧ();ı(true,true,0,0);}float Εέ(float μ,float[]Βο){float έ=Βο[0];for(int i=Βο.Length-1;i>=0;i--)
if(μ<Βο[i]) έ=Βο[i];return έ;}String Αν(ref int Ğ){String έ="";if(Ğ>=ΔЎ.Count) Ğ=-1;if(Ğ>=0) έ=ΔЎ[Ğ].CustomName;Ğ++;return έ;}void Αψ(string ή){if(α!=Π.Д) return;if(ή=="") return;IMyTerminalBlock Χ=χ.GetBlockWithName(ή);if(Χ==null||!(Χ is IMyTimerBlock)){Λ="Timerblock "+ή+" not found!";
return;}((IMyTimerBlock) Χ).Trigger();}void Ŕ(ref int value,int Ʈ,int ĉ,int ΔЁ){if(Ʈ==0) return;if(value<ĉ&&ΔЁ>0||value<=ĉ&&ΔЁ<0){value+=ΔЁ;return;}int ΘЉ=Math.Abs(Ʈ);int Γσ=0;int Δν=1;while(true){Γσ+=Δν*ΘЉ*10;if(Ʈ<0&&value-ĉ<=Γσ) break;if(Ʈ>0&&value-ĉ<Γσ) break;
Δν++;}value+=Δν*Ʈ;}void Ĳ(bool Κι){ъ=Math.Max(ъ,1);щ=Math.Max(щ,1);з=Math.Max(з,0);ı(ϋ==Ρ.Ɯ,false,0,0);}bool ĭ(ref float value,bool ΙЅ,ϊ υ,String ľ,String Ιδ){if(ľ=="") return false;float ż=-1;bool Ζώ=false;if(ľ.ToUpper()==Ιδ) Ζώ=true;else if(!float.TryParse(ľ,out ż)) return false;
else ż=Math.Max(0,ż);if(ΙЅ) ż=(float) Math.Round(ż);ф(ref value,ż,υ,Ζώ,false);return true;}void ф(ref float value,float Ǚ,ϊ υ,bool ΑΜ){ф(ref value,value+Ǚ,υ,ΑΜ,true);}void ф(ref float value,float Ź,ϊ υ,bool ΑΜ,bool Κΰ){float Ǭ=0,ǫ=0;if(υ==ϊ.Ʀ){Ǭ=0.5f;ǫ=10f;}if(υ==ϊ.ΑΦ){Ǭ=0.1f;ǫ=1f;}
if(υ==ϊ.Ƙ){Ǭ=50f;ǫ=100f;}if(υ==ϊ.ΓΟ){Ǭ=5f;ǫ=30f;}if(υ==ϊ.Ακ){Ǭ=1f;ǫ=25f;}if(υ==ϊ.Αι){Ǭ=10f;ǫ=95f;}if(υ==ϊ.ΓΠ){Ǭ=10f;ǫ=90f;}if(υ==ϊ.Θυ){Ǭ=10f;ǫ=1800;}if(υ==ϊ.Γά){Ǭ=0.0f;ǫ=0.75f;}if(Ź==-1&&ΑΜ){value=-1;return;}if(value==-1) ΑΜ=false;
bool ΗΞ=Ź<Ǭ||Ź>ǫ;if(ΗΞ&&Κΰ){if(Ź<value) value=ǫ;else if(Ź>value) value=Ǭ;}else value=Ź;if(ΗΞ&&ΑΜ) value=-1;else value=Math.Max(Ǭ,Math.Min(value,ǫ));value=(float) Math.Round(value,2);}void Βς(bool ΙЀ){if(ΙЀ) Ь=Math.Max(0,Ь-1);else Ь++;ΒЇ();ΒІ();for(int k=0;k<Ȃ.Count&&k<й.Length;k++) Γη(Ȃ[k],k<Ь);
а=-1;Γρ(true);}T ď<T>(T μ){int Α=Array.IndexOf(Enum.GetValues(μ.GetType()),μ);Α++;if(Α>=ΗЍ(μ)) Α=0;return(T) Enum.GetValues(μ.GetType()).GetValue(Α);}int ΗЍ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Γν(){ΑΤ.Clear();for(int i=0;i<Φ.Count;i++) ΑΤ.Add(Φ[i]);Φ.Clear();
ΒΛ=true;ΔΤ=new Ϊ(Ξ);ΔΧ=new Ϊ(Ζ);Ξ.ζ=false;if(Κ==Ι.ά) Ζ.ζ=false;for(int i=0;i<ǚ.Count;i++)
if(!Ȣ.Contains(ǚ.Keys.ElementAt(i)))
Ȣ.Add(ǚ.Keys.ElementAt(i));Ȥ=false;ƍ=false;Ǝ=false;θ(Ρ.ΕІ);}void ΕБ(){if(Ȥ) Ξ=ΔΤ;if(ƍ) Ζ=ΔΧ;if(Ǝ){Φ.Clear();for(int i=0;i<ΑΤ.Count;i++) Φ.Add(ΑΤ[i]);}ΒΛ=false;Ϗ();θ(Ρ.Main);}void ΘЂ(){if(!ΒΛ) return;if(Ω!=Β.о){ΕБ();return;}if(!ΔΤ.ζ) Ȥ=false;if(!ΔΧ.ζ) ƍ=false;if(ΑΤ.Count<=1) Ǝ=false;
IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connectable);if(Л==null) Л=Ъ(MyShipConnectorStatus.Connected);if(Л!=null){if(Math.Round(Ć,2)<=0.20) ΓА++;else ΓА=0;if(ΓА>=5){if(Κ==Ι.ά&&(Ξ.ζ||Ȥ)&&Vector3.Distance(Ξ.Ε,Л.GetPosition())>5){Ζ.γ=Ψ.WorldMatrix.Forward;Ζ.Ѐ=Ψ.WorldMatrix.Left;
Ζ.β=Ψ.WorldMatrix.Down;Ζ.φ=Ψ.GetNaturalGravity();Ζ.Ε=Л.GetPosition();Ζ.ζ=true;Ζ.и=Л.Position;}else{Ξ.γ=Ψ.WorldMatrix.Forward;Ξ.Ѐ=Ψ.WorldMatrix.Left;Ξ.β=Ψ.WorldMatrix.Down;Ξ.φ=Ψ.GetNaturalGravity();Ξ.Ε=Л.GetPosition();Ξ.ζ=true;Ξ.и=Л.Position;}}}double ΔΩ=-1;if(Φ.Count>0) ΔΩ=Vector3.Distance(ε,Φ.Last().Ε);
double ΙА=Math.Max(1.5,Math.Pow(Ć/100.0,2));double ΗΕ=Math.Max(Ć*ΙА,2);Ηυ=ΗΕ;if((ΔΩ==-1)||ΔΩ>=ΗΕ){Ϊ Ϋ=new Ϊ(ε,ΒΔ,Ƶ,ǆ,Ψ.GetNaturalGravity());Ϋ.Ηϊ(ȣ,Ȣ);Φ.Add(Ϋ);}}Ϊ ц(){if(Κ!=Ι.ά) return Ǖ!=null?Ǖ:Ξ;return Ζα;}void Ş(Ϊ ί,Π Ё){Ζα=ί;if(α==Π.Д) ї=Ё;}bool Ȓ(Ϊ ί,float І,bool ΚΧ,out Vector3 ξ)
{if(ΚΧ){Vector3I ΔΚ=new Vector3I((int) ί.и.X,(int) ί.и.Y,(int) ί.и.Z);IMySlimBlock Χ=Me.CubeGrid.GetCubeBlock(ΔΚ);if(Χ==null||!(Χ.FatBlock is IMyShipConnector)){ξ=new Vector3();return false;}Vector3 ΙΗ=п(Ψ,Χ.FatBlock.GetPosition()-ε);Vector3 ΙΖ=п(Ψ,Χ.FatBlock.WorldMatrix.Forward);ξ=ί.Ε-Γπ(ί.γ,ί.β*-1,ΙΗ)-Γπ(ί.γ,ί.β*-1,ΙΖ)*І;
return true;}else{ξ=ί.Ε;return true;}}Vector3 Ȩ(int ΙΔ,int Ƃ,bool ΙЃ){if(!ΙЃ&&Ζψ) return ΓЄ;float ΙΕ=((ν-1f)/2f)-ΙΔ;float ΙІ=((И-1f)/2f)-Ƃ;ΓЄ=Ζ.Ε+Ζ.Ѐ*ΙΕ*ȍ+ă*-1*ΙІ*Ȍ;Ζψ=true;return ΓЄ;}Vector3 ΒЌ(Vector3 ΚΓ,float ΓЌ){return ΚΓ+(ǭ*ΓЌ);}ΰ ΒЂ(){float Ц=-1;ΰ έ=ΰ.Ȑ;
if(Κ!=Ι.ά){Εό();if(α!=Π.ч){Vector3 Й=İ(ǭ,ă*-1,ε-Ζ.Ε);if(Math.Abs(Й.X)<=(float)(ν*ȍ)/2f&&Math.Abs(Й.Y)<=(float)(И*Ȍ)/2f){if(Й.Z<=-1&&Й.Z>=-Ŋ*2) return ΰ.ΑΨ;if(Й.Z>-1&&Й.Z<ο*2) return ΰ.Ȑ;}if(č(Ζ.Ε,ref Ц)) έ=ΰ.Ȑ;}if(Ξ.ζ){if(č(Ξ.Ε,ref Ц)) έ=ΰ.ȏ;for(int i=0;i<Φ.Count;i++)
if(č(Φ[i].Ε,ref Ц)) έ=ΰ.ȩ;if(Vector3.Distance(ε,Ξ.Ε)<dockDist*ο) έ=ΰ.ȏ;if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ȏ;}}else{Vector3 ξ=new Vector3();IMyShipConnector ț=Ъ(MyShipConnectorStatus.Connected);if(Ξ.ζ){if(č(Ξ.Ε,ref Ц)) έ=ΰ.ȏ;if(Ȓ(Ξ,dockDist,true,out ξ))
if(č(ξ,ref Ц)) έ=ΰ.ȏ;if(ț!=null&&Vector3.Distance(ț.GetPosition(),Ξ.Ε)<5)
return ΰ.Βφ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ξ.Ε)>dockDist*ο&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*ο)
if(č(Φ[i].Ε,ref Ц)) έ=ΰ.ȩ;if(Ζ.ζ){if(č(Ζ.Ε,ref Ц)) έ=ΰ.Ȑ;if(Ȓ(Ζ,dockDist,true,out ξ))
if(č(ξ,ref Ц)) έ=ΰ.Ȑ;if(ț!=null&&Vector3.Distance(ț.GetPosition(),Ζ.Ε)<5)
return ΰ.Βχ;}}return έ;}bool č(Vector3 ξ,ref float Ц){float І=Vector3.Distance(ξ,ε);if(І<Ц||Ц==-1){Ц=І;return true;}return false;}void Γμ(){if(Ћ>0){Λ="Setup error! Can't start";return;}if(Κ==Ι.ά){Ď();return;}Ζ.Ε=ε;Ζ.φ=Ψ.GetNaturalGravity();Ζ.γ=ΒΔ;Ζ.β=Ƶ;Ζ.Ѐ=ǆ;
ǭ=ǖ.WorldMatrix.Forward;ă=Ζ.β;if(ǭ==Ψ.WorldMatrix.Down) ă=Ψ.WorldMatrix.Backward;ƥ(true,true);Σ(Β.đ);Γλ();}void ƥ(bool Ǩ,bool ΙЄ){if(α==Π.ч&&!Ǩ) return;bool Ιτ=Ǩ||α==Π.ΒЄ||ν!=ъ||И!=щ||Ŀ!=ȇ;if(Ιτ){if(α!=Π.ч){α=Π.ΑΫ;Ȩ(ώ,ό,ΙЄ);Λ="Job changed, lost progress";}Ŀ=ȇ;
ν=ъ;И=щ;ό=0;ώ=0;й="";а=-1;ł=0;ā=0;в=0;Ь=0;Γρ(true);}}void Γή(){ж(ε,0);ǎ(ȣ,true);}void Γλ(){Ȕ(р.Ασ);Γή();ơ(Ƞ,false);Ƿ(ΔЃ);α=Π.Д;ΕΦ(true);ї=α;θ(Ρ.ƛ);Θε();ΑЉ=true;ΒΘ=0;for(int i=Ƨ.Count-1;i>=0;i--)
if(ƙ(Ƨ[i],false)) ΒΘ++;if(ΒΘ>0) Λ="Started with damage";}void Ϗ(){if(α==Π.Д){α=Π.Ŝ;Λ="Job paused";}Σ(Β.о);ї=α;Αυ(false,0,0,0,0);Γο();ΕΡ(new Vector3(),false);ΕА();Γζ(ChargeMode.Auto);Γθ(false);ΕЈ(true);ȫ(Β.о);ı(false,false,0,0);ǎ(ω,false);ǎ(ѓ,true);Ƿ(true);Βα=false;Αά();
ƴ=false;Γα();ΑЉ=false;ΑΕ=false;ΑΝ=false;if(ϋ!=Ρ.Main&&ϋ!=Ρ.ΑΩ&&ϋ!=Ρ.Βσ&&ϋ!=Ρ.Αφ)
θ(Ρ.Main);}void Ď(){ΰ Ш=ΒЂ();if(Κ==Ι.ά){if(!Ζ.ζ||!Ξ.ζ) return;Γλ();bool ΔΘ=Vector3.Distance(ε,Ξ.Ε)<Vector3.Distance(ε,Ζ.Ε);if(ƅ==Π.Œ) ΔΘ=true;if(ƅ==Π.œ) ΔΘ=false;if(ΔΘ){Ş(Ξ,Π.Œ);switch(Ш){case ΰ.Βφ:Σ(Β.ĕ);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.ȏ:Σ(Β.ǵ);break;default:Σ(Β.Ǣ);break;
}}else{Ş(Ζ,Π.œ);switch(Ш){case ΰ.Βχ:Σ(Β.ĕ);break;case ΰ.Ȑ:Σ(Β.ǵ);break;case ΰ.ȩ:Σ(Β.н);break;default:Σ(Β.Ǣ);break;}}}else{if(α!=Π.Ŝ&&α!=Π.ΑΫ) return;bool ΚΩ=α==Π.ΑΫ;Γλ();bool ΔΟ=ƚ(false)&&Ξ.ζ;switch(Ш){case ΰ.Ȑ:Σ(ΔΟ?Β.Đ:Β.đ);break;case ΰ.ȩ:Σ(ΔΟ?Β.Е:Β.н);break;case ΰ.ȏ:Σ(ΔΟ?Β.ǵ:Β.Ȗ);break;
case ΰ.ΑΨ:{if(ā!=Ь||ΚΩ) Σ(Β.Ȫ);else Σ(Β.ƞ);}break;default:break;}}}void Ĭ(){if(α==Π.ч&&!Ξ.ζ) return;if(Κ==Ι.ά&&(!Ζ.ζ||!Ξ.ζ)) return;Λ="Move to job";ΰ Ш=ΒЂ();if(Κ==Ι.ά){Ş(Ζ,Π.œ);switch(Ш){case ΰ.Ȑ:Σ(Β.ǵ);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.Βχ:return;default:Σ(Β.Ǣ);break;
}ȫ(Β.ĕ);}else{switch(Ш){case ΰ.Ȑ:Σ(Β.н);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.ȏ:Σ(Β.Ȗ);break;case ΰ.ΑΨ:Σ(Β.Ȫ);break;default:break;}if(α==Π.ч) ȫ(Β.н);else ȫ(Β.Č);Βα=true;}Γή();θ(Ρ.Γξ);ơ(Ƞ,false);ї=Π.Αμ;}void Αβ(){if(!Ξ.ζ) return;Λ="Move home";ΰ Ш=ΒЂ();if(Κ==Ι.ά){Ş(Ξ,Π.Œ);switch(Ш)
{case ΰ.ȩ:Σ(Β.Е);break;case ΰ.ȏ:Σ(Β.ǵ);break;case ΰ.Βφ:return;default:Σ(Β.Ǣ);break;}ȫ(Β.ĕ);}else{if(Ъ(MyShipConnectorStatus.Connected)!=null) return;if(Ъ(MyShipConnectorStatus.Connectable)!=null){Σ(Β.ī);ȫ(Β.Ȗ);return;}switch(Ш){case ΰ.Ȑ:Σ(Β.Đ);break;case ΰ.ȩ:Σ(Β.Е);break;case ΰ.ȏ:Σ(Β.Е);break;
case ΰ.ΑΨ:Σ(Β.Į);break;default:break;}ȫ(Β.Ȗ);}Γή();θ(Ρ.Γξ);ơ(Ƞ,false);ї=Π.Αλ;}void Σ(Β Δξ){if(Δξ==Β.о) ΑΗ=Β.о;if(ΑΗ!=Β.о&&Ω==ΑΗ&&Δξ!=ΑΗ){Ϗ();return;}Ў=true;Ω=Δξ;}void ȫ(Β Ё){this.ΑΗ=Ё;}ǡ Κε(int ǁ,bool ǰ){if(ǰ){ǘ=null;ώ=0;ό=0;}if(ȇ==Ǐ.Ƥ){int ń=ǁ+1;ό=(int) Math.Floor(Б(ǁ,ν));
if(ό%2==0) ώ=ǁ-(ό*ν);else ώ=ν-1-(ǁ-(ό*ν));if(ό>=И) return ǡ.ΑΧ;else return ǡ.Αδ;}else if(ȇ==Ǐ.ΕΨ){if(ǘ==null) ǘ=new int[]{0,-1,0,0};int ƽ=(int) Math.Ceiling(ν/2f);int Ƽ=(int) Math.Ceiling(И/2f);int ΔΝ=(int) Math.Floor(ν/2f);int ΔΜ=(int) Math.Floor(И/2f);
int ǃ=0;while(ǘ[2]<Math.Pow(Math.Max(ν,И),2)){if(ǃ>200) return ǡ.ΗЎ;ǃ++;ǘ[2]++;if(-ƽ<ώ&&ώ<=ΔΝ&&-Ƽ<ό&&ό<=ΔΜ){if(ǘ[3]==ǁ){this.ώ=ώ-1+ƽ;this.ό=ό-1+Ƽ;return ǡ.Αδ;}ǘ[3]++;}if(ώ==ό||(ώ<0&&ώ==-ό)||(ώ>0&&ώ==1-ό)){int ΚΖ=ǘ[0];ǘ[0]=-ǘ[1];ǘ[1]=ΚΖ;
}ώ+=ǘ[0];ό+=ǘ[1];}}return ǡ.ΑΧ;}void ΘЀ(){if(Ω==Β.đ){if(Ў){Џ=0;if(ā!=Ь) ł=0;ā=Ь;}if(Џ==0){int ΖΨ=ΗЀ(Ь);if(ΖΨ!=Ь){Ь=ΖΨ;ł=0;ā=Ь;}ǡ έ=Θξ(Ь);if(έ==ǡ.ΑΧ){α=Π.ΒЄ;Λ="Job done";if(ĥ&&Ξ.ζ){Σ(Β.Đ);ȫ(Β.Ȗ);ї=Π.Αλ;}else{Σ(Β.ŗ);ȫ(Β.Č);ї=Π.Αμ;}return;}if(έ==ǡ.Αδ)
{Џ=1;ǎ(ω,true);њ=Ȩ(ώ,ό,true);ж(њ,10);д(Ζ.β,Ζ.γ,Ζ.Ѐ,false);}}else{if(а>=0&&ΓΒ(а,true)){Џ=0;return;}if(Э<wpReachedDist){Σ(Β.ƞ);return;}}}if(Ω==Β.ƞ){if(Ў){ǎ(ω,true);Ƿ(false);њ=Ȩ(ώ,ό,false);ж(ΒЌ(њ,0),0);д(Ζ.β,Ζ.γ,Ζ.Ѐ,false);Џ=1;ы=0;м=0;ΑЀ=0;ΔΫ=-1;Ŋ=з;
ΔΥ=true;}if(!Βύ()){Σ(Β.Į);return;}if(ƚ(true)){ΗΜ=Ȧ("","ORE",Х.Ĩ);if((Ю==Ѝ.ΒЅ||Ю==Ѝ.Αέ||
Ю==Ѝ.ΓΓ||Ю==Ѝ.Αη)&&Κ!=Ι.ǟ)
Σ(Β.Ɩ);else if((Ю==Ѝ.Εά||Ю==Ѝ.ΒϏ)&&Κ!=Ι.ǟ)
Σ(Β.Γς);else
Σ(Β.Į);return;}в=Vector3.Distance(ε,њ);if(в>ł){ł=в;ΔΥ=false;}if(Κ==Ι.ǟ&&Εθ()==MyDetectedEntityType.SmallGrid)
м+=2;else м-=2;м=Math.Max(100,Math.Min(400,м));if(Џ>0&&Џ<м){if(в>ы){if(м>150) ы=в;else ы=(float) Math.Ceiling(в);Џ=1;}else Џ++;}else{if(Џ>0){Λ="Ship stuck! Retrying";ы=в;Џ=0;ı(false,true,0,ο*sensorRange);}ж(ΒЌ(њ,Math.Max(0,ы-ο)),ΒЏ(false));if(Э<=wpReachedDist/2){Џ=1;ы=0;}
return;}ı(false,true,ο*sensorRange,0);Vector3 ΘЋ=њ+ǭ*в;bool ΖЂ=false;if(Vector3.Distance(ΘЋ,ε)>0.3f){Vector3 ΙΛ=њ+ǭ*(в+0.1f);ж(ΙΛ,4);ΖЂ=true;}else{float Ǳ=ΒЏ(true);Vector3 Ζε=ΒЌ(њ,Math.Max(з+1,в+1));Ƕ(true,false,false,Ζε,Ζε-њ,Ǳ,Ǳ);}bool ΓЍ=false;if(є==ў.Βω||є==ў.Βψ)
{if(!ΖЂ){float Δπ=0;foreach(IMyTerminalBlock ȋ in ω)
Δπ+=ΗЈ(ȋ,"","",є==ў.Βψ?new string[]{"STONE"}:null);if(Δπ>ΔΫ||в<з||ΔΥ){Γω=0;ΑЀ=в;Ŋ=(int)(Math.Max(Ŋ,ΑЀ)+ο/2);}else{ΓЍ=в-ΑЀ>2&&Γω>=20;Γω++;}ΔΫ=Δπ;}}else ΓЍ=в>=Ŋ;if(ā!=Ь){Σ(Β.Ȫ);в=0;return;}if(ΓЍ){Ь++;Σ(Β.Ȫ);в=0;return;}}if(Ω==Β.Ȫ||Ω==Β.Į||Ω==Β.Ɩ)
{if(Ў){њ=Ȩ(ώ,ό,false);д(Ζ.β,Ζ.γ,Ζ.Ѐ,false);ǎ(ω,Ū);Ƿ(false);ы=Vector3.Distance(ε,њ);ı(false,true,0,ο*sensorRange);}ж(њ,ΒЏ(false));if(Vector3.Distance(ε,њ)>=ы+5){ǎ(ω,false);Ƿ(true);Λ="Can´t return!";}if(Э<wpReachedDist){if(Ω==Β.Ȫ&&Βα) Σ(Β.ŗ);if(Ω==Β.Ȫ) Σ(Β.đ);
if(Ω==Β.Ɩ) Σ(Β.Αα);if(Ω==Β.Į){if(Ξ.ζ) Σ(Β.Đ);else{Ϗ();Ĭ();Λ="Can´t return, no dock found";}}return;}}if(Ω==Β.Е||Ω==Β.н){if(Ω==Β.н&&α==Π.Д&&Κ!=Ι.ά){if(!Βύ()||ƚ(true)){Σ(Β.Е);return;}}bool Δψ=false;bool Βν=false;bool ΖΜ=false;float Ť=0;bool ΗΛ=false;Ϊ Ϋ=null;
if(Ў){if(Ω==Β.Е||Κ==Ι.ά){if(Κ!=Ι.ά){if(Ē(1)) Βυ();else{Ǖ=null;ȡ=0;ƴ=!Βώ(1);}}Ϊ ί=ц();κ=new Γέ();κ.λ=ί;κ.Ʋ=followPathDock*ο;κ.ΕΕ=useDockDirectionDist*ο;κ.Ť=10;κ.Ƌ.Add(ί.Ε);Vector3 ţ=new Vector3();if(Ȓ(ί,dockDist*ο,true,out ţ))
κ.Ƌ.Add(ţ);else κ.Ʋ*=1.5f;if(Κ==Ι.ά){if(ί==Ξ) κ.ΒΑ=Ζ.Ε;if(ί==Ζ) κ.ΒΑ=Ξ.Ε;κ.ΔΕ=dockDist*ο*1.1f;}}else if(Ω==Β.н){κ=new Γέ();κ.λ=Ζ;κ.Ʋ=followPathJob*ο;κ.ΕΕ=useJobDirectionDist*ο;κ.Ť=10;κ.ΒΑ=Ξ.Ε;κ.ΔΕ=dockDist*ο*1.1f;κ.Ƌ.Add(Ζ.Ε);if(α==Π.ч){if(!Ξ.ζ||Φ.Count==0){Ϗ();return;}
float ΙΣ=Vector3.Distance(Φ.First().Ε,Ξ.Ε);float ΙΤ=Vector3.Distance(Φ.Last().Ε,Ξ.Ε);if(ΙΣ<ΙΤ) κ.λ=Φ.Last();else κ.λ=Φ.First();}}ΑΑ=new Vector3();ΗΛ=!ΘΤ(ε);ǎ(ω,false);Ƿ(true);г=-1;double ǽ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,κ.ΒΑ)<=κ.ΔΕ) continue;
double І=Vector3.Distance(Φ[i].Ε,ε);if(ǽ==-1||І<ǽ){г=i;ǽ=І;}}ȅ=ΘΗ(κ.λ.Ε,г);ĵ=null;}if(Ω==Β.Е&&Κ!=Ι.ά&&!Ў&&!ƣ()){Ϊ Γώ=ц();ƴ=!Βώ(1);if(ц()!=Γώ){Σ(Β.Е);return;}}if(Ω==Β.Е&&Κ!=Ι.ά&&!Ў&&Ē(1)&&Βυ()){Σ(Β.Е);return;}bool ΖЀ=false;if(Ω==Β.Е&&!Ў&&г>=0&&г<Φ.Count)
{Vector3 Ʊ=Φ[г].Ε+Ελ(г,ȅ);int Δο=г+ȅ;Vector3 ń=ȅ!=0&&Δο>=0&&Δο<Φ.Count?Φ[Δο].Ε:κ.λ.Ε;if(ΘΞ(Ʊ,ń)){if(!ΑЌ){ΑЌ=true;Ζκ=ś(Ʊ)?ε:Ʊ;}ж(Ζκ,0);д(κ.λ.β,κ.λ.γ,κ.λ.Ѐ,false);Λ="Waiting for dock clearance";return;}ΑЌ=false;ΖЀ=Ē(1)&&(ś(ε)||ś(ń));}Ηϋ(Φ,ȅ,κ.Ƌ,κ.Ʋ,Ў,ref Џ);
for(int i=0;i<κ.Ƌ.Count;i++){float І=Vector3.Distance(ε,κ.Ƌ[i]);if(І<=κ.Ʋ) Δψ=true;if(І<=κ.ΕΕ) Βν=true;}if(ΖЀ) Βν=true;if(Βν) Ť=κ.Ť;float Κκ=ĵ!=null?ĵ.Ф:Ć;float Κί=(float) Math.Max(Ć*0.1f*ο,wpReachedDist);if((Э<Κί)||Ў){if(!Ў) г+=ȅ;if(ȅ==0||г>Φ.Count-1||г<0)
Δψ=true;else{ΕΙ=ȅ>0?Φ.Count-1-г:г;Ϋ=Φ[г];ĵ=Ϋ;if(г>=1&&г<Φ.Count-1)
ΑΑ=Ϋ.Ε-Φ[г-ȅ].Ε;else ĵ=null;Ђ=Ϋ.Ε+Ελ(г,ȅ);ΖΜ=true;}}if(Βν)
д(κ.λ.β,κ.λ.γ,κ.λ.Ѐ,false);else if(ΗΛ)
Βτ(κ.λ.β,10,true);else if(ΖΜ&&Ϋ!=null){if(ȅ>0) д(Ϋ.β,Ϋ.γ,Ϋ.Ѐ,90,false);else д(Ϋ.β,-Ϋ.γ,-Ϋ.Ѐ,90,false);}Ƕ(true,false,true,Ђ,ΑΑ,ĵ==null?0:ĵ.Ф,Ť);if(Δψ){ΕΙ=0;if(Ω==Β.Е&&!Ηώ()){ж(Ђ,0);return;}if(Ω==Β.Е&&ƴ){ж(Ђ,0);Λ="Waiting for a free dock";return;}if(Ω==Β.Е||Κ==Ι.ά){Σ(Β.ǵ);return;}
if(Ω==Β.н&&Βα){Σ(Β.ŗ);return;}if(Ω==Β.н){Σ(Β.đ);return;}}}if(Ω==Β.ǵ||Ω==Β.Ơ){if(Κ!=Ι.ά&&ƣ()){if(!Ē(1)){Σ(Β.Е);return;}}else if(Κ!=Ι.ά){Ϊ Γώ=ц();if(!Βώ(2)||ц()!=Γώ){Σ(Β.Е);return;}}Ϊ ί=ц();if(Ў){if(!Ȓ(ί,dockDist*ο,true,out Ђ)){Ȕ(р.Ɣ);Ϗ();return;}
ж(Ђ,0);Βτ(ί.β,90,true);}if(Э<followPathDock*ο&&Э!=-1){ж(Ђ,10);д(ί.β,ί.γ,ί.Ѐ,false);}if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null){Σ(Β.ī);return;}if(Э<wpReachedDist/2&&Э!=-1){Σ(Β.ħ);return;}}if(Ω==Β.ħ||Ω==Β.Č){if(Ў)
{if(Ω==Β.ħ){Ϊ ί=ц();if(!Ȓ(ί,dockDist*ο,true,out Ђ)){Ȕ(р.Ɣ);Ϗ();return;}Ƕ(true,true,false,Ђ,0);д(ί.β,ί.γ,ί.Ѐ,10,false);}if(Ω==Β.Č){д(Ζ.β,Ζ.γ,Ζ.Ѐ,0.5f,false);Ђ=Ζ.Ε;Ƕ(true,true,false,Ђ,0);}}if(ΒΚ){Αυ(false,0,0,0,0);if(Ω==Β.ħ) Σ(Β.ī);if(Ω==Β.Č) Ϗ();return;}}
if(Ω==Β.ī){if(Ъ(MyShipConnectorStatus.Connected)!=null){if(Κ==Ι.ά) Σ(Β.ĕ);else Σ(Β.Ȗ);return;}Ϊ ί=ц();if(Ў){м=0;ΙΥ=DateTime.Now;Џ=0;д(ί.β,ί.γ,ί.Ѐ,false);}Vector3I ΔΚ=new Vector3I((int) ί.и.X,(int) ί.и.Y,(int) ί.и.Z);IMySlimBlock Ζή=Me.CubeGrid.GetCubeBlock(ΔΚ);float Ζν=dockingSpeed;
float Ιο=dockingSpeed*5;float ΖΚ=Math.Max(1.5f,Math.Min(5f,ο*0.15f));if(!Ȓ(ί,0,true,out Ђ)||!Ȓ(ί,ΖΚ,true,out ΑΑ)||
Ζή==null||!Ζή.FatBlock.IsFunctional){Ȕ(р.Ɣ);Ϗ();return;}if(м==1||(Vector3.Distance(ε,Ђ)<=ΖΚ*1.1f&&!Ў))
м=1;else{Vector3 ΚΞ=п(Ψ,ΑΑ-ε);Vector3 ΔΙ=п(Ψ,Ψ.GetNaturalGravity());float ΙЇ=ΑΪ(ΚΞ,ΔΙ,null);Ζν=Math.Min(Ιο,ΙЇ);}Ƕ(true,false,false,Ђ,Ђ-ε,dockingSpeed,Ζν);if(Ў) ы=(float) Э;IMyShipConnector ΓЉ=Ъ(MyShipConnectorStatus.Connectable);if(ΓЉ!=null){Ƕ(false,false,false,Ђ,0);
if(Џ>0) Џ=0;Џ--;if(Џ<-5){ΓЉ.Connect();if(ΓЉ.Status==MyShipConnectorStatus.Connected){if(Κ==Ι.ά) Σ(Β.ĕ);else Σ(Β.Ȗ);Γο();ơ(Ƞ,true);return;}}}else{float Ζι=(float) Math.Round(Э,1);if(Ζι<ы){Џ=-1;ы=Ζι;}else Џ++;if(Џ>20){Σ(Β.Ơ);return;}}}if(Ω==Β.Ȗ||Ω==Β.ĕ||
Ω==Β.Αϋ||Ω==Β.Αί||Ω==Β.Ī){bool Γχ=false,Γψ=false;if(Κ==Ι.ά){if(ц()==Ξ) Γχ=true;else if(ц()==Ζ) Γψ=true;}if(Ў){ΑΕ=false;if(Ъ(MyShipConnectorStatus.Connected)==null){Σ(Β.Ǣ);return;}Γο();if(Γχ) Αψ(Ѓ.ǧ);if(Γψ) Αψ(Є.ǧ);Ŏ=false;Ȭ=false;ȱ=false;ΑΟ=false;}if(Ъ(MyShipConnectorStatus.Connected)==null){Ϗ();Ȕ(р.Εφ);return;}
if(α!=Π.Д||ј==-1||ǹ==ѐ.None) Ȭ=true;else if(Ș>=100f) Ȭ=true;else if(Ș<=99f) Ȭ=false;if(α!=Π.Д||љ==-1||ǒ.Count==0) ȱ=true;else if(Ȳ>=100f) ȱ=true;else if(Ȳ<=99) ȱ=false;if(α!=Π.Д||к==-1||Ģ.Count==0) ΑΟ=true;else ΑΟ=ŏ>=к;Š я=null;if(Γχ) я=Ѓ;
if(Γψ) я=Є;if(я!=null&&(я.Н==π.Αο||я.Н==π.Αξ)) Ȭ=true;if(я!=null&&(я.Н==π.ΓΥ)) if(!Ŏ) Ȭ=false;if(я!=null&&(я.Н==π.ΓΩ||я.Н==π.ΓΧ)) ȱ=true;if(я!=null&&(я.Н==π.ΓΨ)) if(!Ŏ) ȱ=false;if(ȳ){ChargeMode ΖΧ=Ȭ?ChargeMode.Auto:ChargeMode.Recharge;if(я!=null&&(я.Н==π.Αξ||я.Н==π.Αο))
ΖΧ=ChargeMode.Discharge;Γζ(ΖΧ);Γθ(!ȱ);}if(!Ŏ){if(Κ==Ι.ά)
Ŏ=α!=Π.Д||ΗϏ(Ў,true)||ΑΝ;else
Ŏ=α!=Π.Д||Εχ();}else{if(!Ȭ) Σ(Β.Ī);if(!ȱ) Σ(Β.Αί);if(!ΑΟ) Σ(Β.Αϋ);Ў=false;}if(Ŏ&&Ȭ&&ȱ&&ΑΟ){Γζ(ChargeMode.Auto);Γθ(false);if(α==Π.Д){if(Κ==Ι.ά){if(ц()==Ξ) Αψ(Ѓ.Ǫ);else if(ц()==Ζ) Αψ(Є.Ǫ);if(ц()==Ξ) Ş(Ζ,Π.œ);else Ş(Ξ,Π.Œ);}}Σ(Β.Ǣ);return;}}if(Ω==Β.Ǣ){if(ΑΣ){if(!ΕΪ()) return;
ΑΣ=false;Σ(Β.Ǣ);return;}if(Ў){if(!ΕΪ()){ΑΣ=true;return;}IMyShipConnector ț=Ъ(MyShipConnectorStatus.Connected);if(ț==null){Σ(Β.н);return;}IMyShipConnector ι=ț.OtherConnector;ΕЇ(ț,false);ơ(Ƞ,false);Ϊ ί=null;Ϊ ѕ=Κ==Ι.ά?Ξ:ц();if(Vector3.Distance(ț.GetPosition(),ѕ.Ε)<5f&&Ξ.ζ) ί=ѕ;
if(Vector3.Distance(ț.GetPosition(),Ζ.Ε)<5f&&Ζ.ζ) ί=Ζ;if(ί!=null){if(!Ȓ(ί,dockDist*ο,true,out Ђ)){Ȕ(р.Ɣ);Ϗ();return;}ж(Ђ,5);д(ί.β,ί.γ,ί.Ѐ,false);}else ж(ε+ι.WorldMatrix.Forward*dockDist*ο,5);if(α==Π.Д) Ȕ(р.Ασ);}if(Э<wpReachedDist){ǎ(ѓ,true);Σ(Β.н);return;}}if(Ω==Β.ŗ)
{if(Ў){Ƿ(true);ǎ(ω,false);Ђ=Ζ.Ε;ж(Ђ,20);д(Ζ.β,Ζ.γ,Ζ.Ѐ,false);}if(Э<wpReachedDist/2){Σ(Β.Č);return;}}if(Ω==Β.Đ){if(Ў){Ƿ(true);ǎ(ω,false);int Ц=-1;double ǽ=-1;for(int i=Φ.Count-1;i>=0;i--){double І=Vector3.Distance(Φ[i].Ε,ε);if(ǽ==-1||І<ǽ){Ц=i;ǽ=І;}}
if(Ц==-1){Σ(Β.Е);return;}Ђ=Φ[Ц].Ε;ж(Ђ,10);д(Ζ.β,Ζ.γ,Ζ.Ѐ,false);}if(Э<wpReachedDist){Σ(Β.Е);return;}}if(Ω==Β.Αα){bool Ǒ=false;if(Ў){Ƿ(true);if((Ю==Ѝ.ΒЅ||Ю==Ѝ.Αέ)&&Αθ()&&
ŕ(ǭ,Ψ.GetNaturalGravity())<25&&ν>=2&&И>=2){Vector3 ƶ=ε;if(ώ>0&&ό<И-1) ƶ=Ȩ(ώ-1,ό+1,true);else if(ώ<ν-1&&ό<И-1) ƶ=Ȩ(ώ+1,ό+1,true);else if(ώ<ν-1&&ό>0) ƶ=Ȩ(ώ+1,ό-1,true);else if(ώ>0&&ό>0) ƶ=Ȩ(ώ-1,ό-1,true);else Ǒ=true;if(!Ǒ) ж(ƶ,10);
}else Ǒ=true;}if(Э<wpReachedDist/2) Ǒ=true;if(Ǒ){Σ(Β.Αϊ);return;}}if(Ω==Β.Γς||Ω==Β.Αϊ){if(Ў){Ƕ(true,true,false,ε,0);ǎ(ω,false);Ƿ(true);Џ=-1;м=Ю==Ѝ.ΓΓ||Ю==Ѝ.Αη?0:-1;}bool Ĺ=!Βύ();int Βη=Ȧ("STONE","ORE",Х.Ĩ);if(Ю==Ѝ.ΒϏ||Ю==Ѝ.Αη||Ю==Ѝ.Αέ)
Βη+=Ȧ("ICE","ORE",Х.Ĩ);bool Ιή=Βη>0;bool ΔΔ=false;if(м>=0){float š=(float) Math.Sin(Εή(м))*ȍ/3f;float ΘЍ=(float) Math.Cos(Εή(м))*Ȍ/3f;Vector3 ΙΓ=Ȩ(ώ,ό,true)+Γπ(ǭ,ă*-1,new Vector3(š,ΘЍ,0));ж(ΙΓ,0.3f);if(Э<Math.Min(ȍ,Ȍ)/10f) м+=5f;if(м>=360) м=0;}if(Џ==-1||Βη<Џ){Џ=Βη;ы=0;}
else{ы++;if(ы>50) ΔΔ=true;}if(!Ιή||Ĺ||ΔΔ){if(!Ĺ){int ΚΤ=Ȧ("","ORE",Х.Ĩ);if(ƚ(true)) Ĺ=true;else if(100-(Б(ΚΤ,ΗΜ)*100)<minEjection) Ĺ=true;else Ȕ(р.Ασ);}if(ΔΔ&&Ĺ) Λ="Ejection failed";if(Ω==Β.Αϊ){if(Ĺ){if(Ξ.ζ) Σ(Β.Đ);else{Ϗ();Ĭ();Λ="Can´t return, no dock found";}}
else Σ(Β.đ);}else if(Ĺ) Σ(Β.Į);else Σ(Β.ƞ);return;}}Ў=false;}void Ηϋ(List<Ϊ>Ħ,int ρ,List<Vector3>ΑΚ,float Ȉ,bool ǰ,ref int ǥ){if(ǰ){for(int i=0;i<Ħ.Count;i++) Ħ[i].Ф=0;ǥ=-1;return;}if(ρ==0) return;int Ǚ=ρ*-1;if(ǥ==-1) ǥ=Ǚ>0?1:Ħ.Count-2;int ǃ=0;while(ǥ>=1&&ǥ<Ħ.Count-1)
{if(ǃ>50) return;ǃ++;try{if((Ǚ<0&&ǥ>=1)||(Ǚ>0&&ǥ<=Ħ.Count-2)){Ϊ μ=Ħ[ǥ];bool ΗΘ=false;for(int j=0;j<ΑΚ.Count;j++)
if(Vector3.Distance(μ.Ε,ΑΚ[j])<=Ȉ){ΗΘ=true;break;}if(!ΗΘ){Ϊ Βέ=Ħ[ǥ-Ǚ];Ϊ ń=Ħ[ǥ+Ǚ];Vector3 ΚΟ=μ.Ε-ń.Ε;Vector3 ΔА=Βέ.Ε-μ.Ε;Vector3 ΙЁ=μ.Ε+Vector3.Normalize(ΚΟ)*ΔА.Length();Vector3 ΙΡ=Βέ.Ε-ΙЁ;Vector3 ΒΟ=İ(ρ>0?μ.γ:μ.γ*-1,μ.β*-1,ΙΡ);Vector3 Ιξ=İ(ρ>0?μ.γ:μ.γ*-1,μ.β*-1,ΔА);
Vector3 ΒΡ=İ(ρ>0?μ.γ:μ.γ*-1,μ.β*-1,μ.φ);μ.Ф=(float) Math.Sqrt(Math.Pow(Βέ.Ф,2)+Math.Pow(ΑΪ(-Ιξ,ΒΡ,μ),2));for(int j=0;j<ΑΚ.Count;j++)
if(Vector3.Distance(Βέ.Ε,ΑΚ[j])<=Ȉ){Vector3 ΒΣ=İ(ρ>0?μ.γ:μ.γ*-1,μ.β*-1,ΑΚ[j]-μ.Ε);float ΚΕ=ΑΪ(-ΒΣ,ΒΡ,μ);μ.Ф=Math.Min(μ.Ф,ΚΕ)/2f;}if(ΒΟ.Length()==0) ΒΟ=new Vector3(0,0,1);Vector3 Ιν=İ(μ.γ,μ.β*-1,μ.φ);float ΚΙ=Ɨ(-ΒΟ,Ιν,μ);float Αό=Б(ΚΙ,Ō);float ΔЍ=(float) Math.Sqrt(Б(ΒΟ.Length()*1.0f,0.5f*Αό));
μ.Ф=Math.Min(μ.Ф,(ΔА.Length()/ΔЍ)*Ǹ);}}}catch{return;}ǥ+=Ǚ;}ǥ=-1;}void Γρ(bool ǰ){if(ǰ){Ǆ=0;return;}if(Κ==Ι.ά) return;float Ǒ=Ь*Math.Max(1,з);if(ā==Ь) Ǒ+=Math.Min(з,в);float ΚΣ=ν*И*Math.Max(1,з);Ǆ=Math.Max(Ǆ,(float) Math.Min(Ǒ/ΚΣ*100.0,100));}MyDetectedEntityType Εθ()
{try{if(ȓ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒЏ(bool ȁ){if(Κ==Ι.ǟ&&Εθ()==MyDetectedEntityType.None&&!ƙ(ψ,true))
return fastSpeed;else return ȁ?ƒ:Ƒ;}void Θψ(){float ΗΒ=0,Ζγ=0,ľ=0,Δρ=0;for(int i=0;i<Ė.Count;i++){IMyBatteryBlock Ƭ=Ė[i];if(!ȓ(Ƭ,true)) continue;ΗΒ+=Ƭ.MaxStoredPower;Ζγ+=Ƭ.CurrentStoredPower;ľ+=Ƭ.CurrentInput;Δρ+=Ƭ.CurrentOutput;}Ș=(float) Math.Round(Б(Ζγ,ΗΒ)*100,1);
if(ľ>=Δρ) ǹ=ѐ.Ī;else ǹ=ѐ.Εί;if(ľ==0&&Δρ==0||Ș==100.0) ǹ=ѐ.о;if(Ė.Count==0) ǹ=ѐ.None;}void Θώ(){float Ζς=0;for(int i=0;i<ǒ.Count;i++){IMyGasTank Ηζ=ǒ[i];if(!ȓ(Ηζ,true)) continue;Ζς+=(float) Ηζ.FilledRatio;}Ȳ=Б(Ζς,ǒ.Count)*100f;}void ΘЇ(){ŏ=0;try{for(int i=0;i<Ģ.Count;i++)
{IMyReactor Δω=Ģ[i];if(!ȓ(Δω,true)) continue;List<MyInventoryItem>Ї=new List<MyInventoryItem>();Δω.GetInventory(0).GetItems(Ї);float Ѕ=0;for(int j=0;j<Ї.Count;j++){MyInventoryItem ύ=Ї[j];if(ύ.Type.SubtypeId.ToUpper()=="URANIUM"&&ύ.Type.TypeId.ToUpper().Contains("_INGOT"))
Ѕ+=(float) ύ.Amount;}if(Ѕ<ŏ||i==0){ŏ=Ѕ;Δδ=Δω.CustomName;}}}catch(Exception e){Ā=e;}}void Θω(){Ȅ=0;Ǿ=0;try{for(int i=0;i<Ũ.Count;i++){IMyTerminalBlock Χ=Ũ[i];if(!ȓ(Χ,true)) continue;Ǿ+=(float) Χ.GetInventory(0).CurrentVolume;Ȅ+=(float) Χ.GetInventory(0).MaxVolume;
}ΒΞ=(float) Math.Min(Math.Round(Б(Ǿ,Ȅ)*100,1),100.0);}catch(Exception e){Ā=e;}}void ΘϏ(){try{ǩ.Clear();for(int i=0;i<Ũ.Count;i++){IMyTerminalBlock Χ=Ũ[i];if(!ȓ(Χ,true)) continue;ΕΠ(Χ,Х.Ś);}if(Ю!=Ѝ.ΓΤ){for(int i=0;i<Ĵ.Count;i++){IMyTerminalBlock Χ=Ĵ[i];if(!ȓ(Χ,true)) continue;
ΕΠ(Χ,Х.ΘΡ);}}Θρ(ǩ);}catch(Exception e){Ā=e;}}void Ηύ(){if(ΑЉ){if(Εη().Count>ΒΘ){ΑЉ=false;if(ь!=х.Ƙ){Ϗ();if(ь==х.ΒЊ) Ĭ();if(ь==х.Ας)
if(Ξ.ζ) Αβ();else Ĭ();}Λ="Damage detected";}}}bool Βύ(){if(!ΒΙ) return true;if(α==Π.Д){if(ј>0&&ǹ!=ѐ.None)
if(Ș<=ј){Λ="Low energy! Move home";return false;}if(к>0&&Ģ.Count>0)
if(ŏ<=к){Λ="Low fuel: "+Δδ;return false;}if(љ>0&&ǒ.Count>0)
if(Ȳ<=љ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Εη(){List<IMyTerminalBlock>Ķ=new List<IMyTerminalBlock>();for(int i=0;i<Ƨ.Count;i++){IMyTerminalBlock Χ=Ƨ[i];if(ƙ(Χ,false)) Ķ.Add(Χ);}return Ķ;}bool ƙ(IMyTerminalBlock Χ,bool ΔІ){return(!ȓ(Χ,ΔІ)||!Χ.IsFunctional);
}bool ȓ(IMyTerminalBlock Χ,bool ΔІ){if(Χ==null) return false;try{IMyCubeBlock Ζχ=Me.CubeGrid.GetCubeBlock(Χ.Position).FatBlock;if(ΔІ) return Ζχ==Χ;else return Ζχ.GetType()==Χ.GetType();}catch{return false;}}float ΗЈ(IMyTerminalBlock Χ,String ѝ,String Ηή,String[]ǿ){float Ȝ=0;for(int i=0;i<Χ.InventoryCount;i++)
{IMyInventory ů=Χ.GetInventory(i);List<MyInventoryItem>Ї=new List<MyInventoryItem>();ů.GetItems(Ї);foreach(MyInventoryItem ύ in Ї){if(ǿ!=null&&(ǿ.Contains(ύ.Type.TypeId.ToUpper())||ǿ.Contains(ύ.Type.SubtypeId.ToUpper())))
continue;if((ѝ==""||ύ.Type.TypeId.ToUpper()==ѝ)&&(Ηή==""||ύ.Type.SubtypeId.ToUpper()==Ηή))
Ȝ+=(float) ύ.Amount;}}return Ȝ;}е ΗА(String ή,String υ,Х Ш,bool ΙΜ){ή=ή.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǩ.Count;i++){е ύ=ǩ[i];if(ύ.ή.ToUpper()==ή&&ύ.υ.ToUpper()==υ&&(ύ.ŵ==Ш||Ш==Х.Ĩ))
return ύ;}е έ=null;if(ΙΜ){έ=new е(ή,υ,0,Ш);ǩ.Add(έ);}return έ;}int Ȧ(String ή,String υ,Х Ш){return Ȧ(ή,υ,Ш,null);}int Ȧ(String ή,String υ,Х Ш,String[]ǿ){int Ȝ=0;ή=ή.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǩ.Count;i++){е ύ=ǩ[i];if(ǿ!=null&&ǿ.Contains(ύ.ή.ToUpper())) continue;
if((ή==""||ύ.ή.ToUpper()==ή)&&(υ==""||ύ.υ.ToUpper()==υ)&&(ύ.ŵ==Ш||Ш==Х.Ĩ))
Ȝ+=ύ.Ѕ;}return Ȝ;}void ΕΠ(IMyTerminalBlock Χ,Х Ш){for(int i=0;i<Χ.InventoryCount;i++){List<MyInventoryItem>Ї=new List<MyInventoryItem>();Χ.GetInventory(i).GetItems(Ї);for(int j=0;j<Ї.Count;j++)
ΗА(Ї[j].Type.SubtypeId,Ї[j].Type.TypeId.Replace("MyObjectBuilder_",""),Ш,true).Ѕ+=(int) Ї[j].Amount;}}void Θρ(List<е>Ј){for(int i=Ј.Count-1;i>0;i--)
for(int j=0;j<i;j++){е a=Ј[j];е b=Ј[j+1];if(a.Ѕ<b.Ѕ) Ј.Move(j,j+1);}}bool ƚ(bool Κη){if(ċ&&Κ!=Ι.ά)
if(Я!=-1&&Ō>=Я){Λ="Ship too heavy";return true;}if(ΒΞ>=ȃ||ΑΕ){ΑΕ=false;Λ="Ship is full";return true;}return false;}bool Εχ(){String[]ǿ=null;if(!Ċ) ǿ=new string[]{"ICE"};if(Κ==Ι.Ɲ) return Ȧ("","ORE",Х.Ś,ǿ)==0;if(Κ==Ι.ǟ) return Ȧ("","COMPONENT",Х.Ś,ǿ)==0;else return Ȧ("","",Х.Ś,ǿ)==0;
}bool ΘΥ(){List<IMyCockpit>ΓЈ=new List<IMyCockpit>();χ.GetBlocksOfType(ΓЈ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓЈ.Count;i++)
if(ΓЈ[i].IsUnderControl) return true;return false;}bool ΗϏ(bool ǰ,bool Βγ){IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connected);if(Л==null) return false;if(Vector3.Distance(Ξ.Ε,Л.GetPosition())<5)
return ΕΫ(Ѓ,ǰ,Βγ);if(Vector3.Distance(Ζ.Ε,Л.GetPosition())<5)
return ΕΫ(Є,ǰ,Βγ);return false;}bool ΕΫ(Š Ț,bool ǰ,bool Βγ){if(ǰ) Ț.Θα();Ț.ΘЄ();bool ю=false;String ќ="";switch(Ț.Н){case π.ΓΦ:{ќ="Waiting for command";ю=false;break;}case π.ΕЁ:{ќ="Waiting for cargo";ю=ƚ(true);break;}case π.ΕЀ:{ќ="Unloading";ю=Εχ();break;}case π.ΓΫ:{ю=true;break;}
case π.ΓΥ:{ќ="Charging batteries";ю=Ș>=100f;break;}case π.Αο:{ќ="Discharging batteries";ю=Ș<=25f;break;}case π.Αξ:{ќ="Discharging batteries";ю=Ș<=0f;break;}case π.ΓΨ:{ќ="Filling up hydrogen";ю=Ȳ>=100f;break;}case π.ΓΩ:{ќ="Unloading hydrogen";ю=Ȳ<=25f;break;}
case π.ΓΧ:{ќ="Unloading hydrogen";ю=Ȳ<=0f;break;}case π.ΓΪ:{bool ΖϏ=ΘΥ();if(!ΖϏ) Ț.Δυ=true;ю=Ț.Δυ&&ΖϏ;ќ="Waiting for passengers";break;}}if(!ю) Ț.Βό(true);if(ю&&Ț.ΒБ()){ю=Ț.Βό(false);ќ="Undocking in: "+Αγ((int) Math.Max(0,Ț.Ǧ-Ț.Ĝ));}if(Βγ) Ησ=ќ;return ю;}void Ηφ()
{if(!Ű) return;if(ω.Count<=1) return;float Ηλ=0,Ηκ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȋ=ω[i];if(ƙ(ȋ,true)) continue;Ηλ+=(float) ȋ.GetInventory(0).MaxVolume;Ηκ+=(float) ȋ.GetInventory(0).CurrentVolume;}float ΖΝ=(float) Math.Round(Б(Ηκ,Ηλ),5);for(int Ň=0;Ň<Math.Max(1,Math.Floor(ω.Count/10f));Ň++)
{float ΔΣ=0,Δγ=0;float Ζό=0,ΖА=0;IMyTerminalBlock Ȱ=null,ĝ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȋ=ω[i];if(ƙ(ȋ,true)) continue;float Δκ=(float) ȋ.GetInventory(0).MaxVolume;float ΒΒ=Б((float) ȋ.GetInventory(0).CurrentVolume,Δκ);if(Ȱ==null||ΒΒ>ΔΣ){Ȱ=ȋ;ΔΣ=ΒΒ;Ζό=Δκ;}
if(ĝ==null||ΒΒ<Δγ){ĝ=ȋ;Δγ=ΒΒ;ΖА=Δκ;}}if(Ȱ==null||ĝ==null||Ȱ==ĝ) return;if(checkConveyorSystem&&!Ȱ.GetInventory(0).IsConnectedTo(ĝ.GetInventory(0))){if(Γύ>20) Λ="Inventory balancing failed";else Γύ++;return;}Γύ=0;List<MyInventoryItem>Ї=new List<MyInventoryItem>();
Ȱ.GetInventory(0).GetItems(Ї);float ΑΡ=0;if(Ї.Count==0) continue;MyInventoryItem ΒΓ=Ї[0];String ΖЃ=ΒΓ.Type.TypeId+ΒΓ.Type.SubtypeId;if(!Ης.TryGetValue(ΖЃ,out ΑΡ)){if(ΘΩ(Ȱ.GetInventory(0),0,ĝ.GetInventory(0),out ΑΡ))
Ης.Add(ΖЃ,ΑΡ);else return;}float ΚΠ=((ΔΣ-ΖΝ)*Ζό/ΑΡ);float ΚΡ=((ΖΝ-Δγ)*ΖА/ΑΡ);int ΕΓ=(int) Math.Min(ΚΡ,ΚΠ);if(ΕΓ<=0) return;if((float) ΒΓ.Amount<ΕΓ)
Ȱ.GetInventory(0).TransferItemTo(ĝ.GetInventory(0),0,null,null,ΒΓ.Amount);else
Ȱ.GetInventory(0).TransferItemTo(ĝ.GetInventory(0),0,null,null,ΕΓ);}}bool ΘΩ(IMyInventory ΑΖ,int Α,IMyInventory ΙΠ,out float Ηρ){Ηρ=0;float ΘБ=(float) ΑΖ.CurrentVolume;List<MyInventoryItem>ΓϏ=new List<MyInventoryItem>();ΑΖ.GetItems(ΓϏ);float ΖΡ=0;for(int i=0;i<ΓϏ.Count;i++) ΖΡ+=(float) ΓϏ[i].Amount;
ΑΖ.TransferItemTo(ΙΠ,Α,null,null,1);float Ηο=ΘБ-(float) ΑΖ.CurrentVolume;List<MyInventoryItem>Γτ=new List<MyInventoryItem>();ΑΖ.GetItems(Γτ);float ΖΖ=0;for(int i=0;i<Γτ.Count;i++) ΖΖ+=(float) Γτ[i].Amount;if(Ηο==0f||!ΓΔ(0.9999,ΖΡ-ΖΖ,1.0001))
return false;Ηρ=Ηο;return true;}void Θύ(){Vector3 ƺ=Ђ+Ĥ;Vector3 Ȋ=ƺ-ε;if(Ȋ.Length()==0) Ȋ=new Vector3(0,0,-1);Vector3 ΒΣ=п(Ψ,Ȋ);Vector3 Ηη=Vector3.Normalize(ΒΣ);Vector3 Ŵ=п(Ψ,Ψ.GetNaturalGravity());float ΙБ=Δς>0?Math.Max(0,1-ŕ(Ȋ,Ζτ)/5):0;float Ф=(float) Math.Min((Δη>0?Δη:1000f),Math.Max(ΑΪ(-ΒΣ,Ŵ,null),Δς*ΙБ));
if(!Ηι) Ф=0;if(ΗΨ) Ф=Math.Max(0,1-ΑЇ/Αύ)*Ф;if(generalSpeedLimit>0) Ф=Math.Min(generalSpeedLimit,Ф);if(Ž) Ф=0;if(Ȏ>=0) Ф=Math.Min(Ф,Ȏ);if(ΗΩ) Ф*=(float) Math.Min(1,Б(Ȋ.Length(),wpReachedDist)/2);Vector3 ΖЍ=п(Ψ,Ψ.GetShipVelocities().LinearVelocity);float ΘЌ=(float)(Math.Max(0,15-ŕ(-Ηη,-ΖЍ))/15)*0.85f+0.15f;
ΔЊ+=Math.Sign(ΘЌ-ΔЊ)/10f;Vector3 Ę=Ηη*Ф*ΔЊ-(ΖЍ);Vector3 Γϋ=ΒЎ(Ę,null);if(Βθ&&Э>0.1f){Ę.X*=Γκ(Ę.X,ref Βι.X,1f,Γϋ.X,20);Ę.Y*=Γκ(Ę.Y,ref Βι.Y,1f,Γϋ.Y,20);Ę.Z*=Γκ(Ę.Z,ref Βι.Z,1f,Γϋ.Z,20);}else Βι=new Vector3(1,1,1);ΑΛ=Ō*Ę-Ŵ*Ō;ΕΡ(ΑΛ,Βθ);Э=Vector3.Distance(ε,ƺ);
}float Γκ(float value,ref float Ё,float Ǚ,float Ă,float Δθ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ё)) Ё+=Math.Sign(Ё)*Ǚ;else Ё=value;if(value==0) Ё=1;float έ=Math.Abs(Ё);if(έ<Δθ||Ă==0) return έ;Ё=Math.Min(Δθ,Math.Max(-Δθ,Ё));έ=Math.Abs(Ă);return έ;
}void ΖΔ(){float ћ=90,ǝ=90,Ǘ=90;float Ǳ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȁ,ǔ,ǈ,Δΰ;if(Δα){ȁ=Vector3.Normalize(Ђ-ε);ǈ=п(Ψ,ȁ);ǔ=п(Ψ,ΔЈ);ћ=ŕ(ǈ,new Vector3(0,-1,0))-90;ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);
Ǘ=ĩ(ǈ,new Vector3(-1,0,0),ǈ.Z);}else{ȁ=ΔЉ;ǔ=п(Ψ,ΔЈ);ǈ=п(Ψ,ΔЉ);Δΰ=п(Ψ,Ηθ);ћ=ĩ(ǔ,new Vector3(0,0,1),ǔ.Y);ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);Ǘ=ĩ(Δΰ,new Vector3(0,0,1),Δΰ.X);}if(ΖΘ&&Αθ()){Vector3 ΔΙ=Ψ.GetNaturalGravity();ǔ=п(Ψ,ΔΙ);ћ=ĩ(ǔ,new Vector3(0,0,1),ǔ.Y);
ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);}if(!ΓΔ(-45,ǝ,45)){ћ=0;Ǘ=0;}if(!ΓΔ(-45,Ǘ,45)) ћ=0;Αυ(ΔΛ,1,(-ћ)*Ǳ,(-Ǘ)*Ǳ,(-ǝ)*Ǳ);ΑЇ=Math.Max(Math.Abs(ћ),Math.Max(Math.Abs(ǝ),Math.Abs(Ǘ)));ΒΚ=ΑЇ<=Αύ;}void Ηό(IMyTerminalBlock Βή){if(Βή==null) return;Ǵ=new float[3,2];ǚ=new Dictionary<string,float[,]>();
for(int i=0;i<ȣ.Count;i++){IMyThrust у=ȣ[i];if(!у.IsFunctional) continue;Vector3 ρ=п(Βή,у.WorldMatrix.Backward);float ě=у.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ǵ[0,0]+=ě;else Ǵ[0,1]-=ě;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ǵ[1,0]+=ě;else Ǵ[1,1]-=ě;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ǵ[2,0]+=ě;else Ǵ[2,1]-=ě;String ѝ=Εο(у);float[,]Ǜ=null;if(ǚ.ContainsKey(ѝ)) Ǜ=ǚ[ѝ];else{Ǜ=new float[3,2];ǚ.Add(ѝ,Ǜ);}float Ă=у.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ǜ[0,0]+=Ă;else Ǜ[0,1]-=Ă;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ǜ[1,0]+=Ă;else Ǜ[1,1]-=Ă;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ǜ[2,0]+=Ă;else Ǜ[2,1]-=Ă;}}static String Εο(IMyThrust у){return у.BlockDefinition.SubtypeId;}Vector3 ΓΛ(Vector3 ǐ,float[,]Ŷ){return new Vector3(
ǐ.X>=0?Ŷ[0,0]:Ŷ[0,1],
ǐ.Y>=0?Ŷ[1,0]:Ŷ[1,1],
ǐ.Z>=0?Ŷ[2,0]:Ŷ[2,1]);}bool Εξ(Ϊ Ϋ,String ѝ,out float ΔΓ){ΔΓ=0;int Α=Ȣ.IndexOf(ѝ);if(Α==-1||Ϋ.ǳ==null||Α>=Ϋ.ǳ.Length)
return false;ΔΓ=Ϋ.ǳ[Α];if(ΔΓ==-1) return false;return true;}Vector3 ΒЎ(Vector3 ǐ,Ϊ Ϋ){if(Ϋ!=null){Vector3 έ=new Vector3();for(int i=0;i<ǚ.Keys.Count;i++){String ѝ=ǚ.Keys.ElementAt(i);float Ƹ=0;if(!Εξ(Ϋ,ѝ,out Ƹ))
return ΓΛ(ǐ,Ǵ);έ+=ΓΛ(ǐ,ǚ.Values.ElementAt(i))*Ƹ;}return έ;}return ΓΛ(ǐ,Ǵ);}float Ɨ(Vector3 ǐ,Ϊ Ϋ){return Ɨ(ǐ,new Vector3(),Ϋ);}float Ɨ(Vector3 ǐ,Vector3 φ,Ϊ Ϋ){Vector3 ΚΘ=ΒЎ(ǐ,Ϋ);Vector3 ě=ΚΘ+φ*Ō;float ŉ=(ě/ǐ).AbsMin();return(float)(ǐ*ŉ).Length();}float ΑΪ(Vector3 ΒΠ,Vector3 Ŵ,Ϊ Ϋ)
{if(ΒΠ.Length()==0) return 0;float ΖΗ=1;if(Ŵ.Length()>0) ΖΗ=Math.Min(1,ŕ(-Ŵ,ΒΠ)/90)*0.4f+0.6f;float ΑΛ=Ɨ(ΒΠ,Ŵ,Ϋ);if(ΑΛ==0) return 0.1f;float Αό=Б(ΑΛ,Ō);float ΔЍ=(float) Math.Sqrt(Б(ΒΠ.Length(),Αό*0.5f));return Αό*ΔЍ*ΖΗ*Ǹ;}float Αζ(Vector3 ȁ,Vector3 Ќ,Vector3 φ,Ϊ Ϋ)
{if(φ.Length()==0f) return 0;Vector3 ΒΡ=İ(ȁ,Ќ,Vector3.Normalize(φ));float Ǩ=Ɨ(-ΒΡ,Ϋ);return Ǩ/φ.Length();}void ΕΦ(bool Ιε){float ǜ=0;float ΔΖ=0.9f;if(Ιε){Я=-1;Ő=0;Βε=null;if(α!=Π.ч&&Ζ.φ.Length()!=0){ǜ=ΔΖ*Αζ(Ζ.γ,Ζ.β*-1,Ζ.φ,null);if(ǜ<Я||Я==-1) Я=ǜ;}if(Ξ.ζ&&Ξ.φ.Length()!=0)
{ǜ=ΔΖ*Αζ(Ξ.γ,Ξ.β*-1,Ξ.φ,null);if(ǜ<Я||Я==-1) Я=ǜ;}return;}if(Ő==-1) return;if(Ő>=0){int ΗΡ=0;while(Ő<Φ.Count){if(ΗΡ>100) return;ΗΡ++;Ϊ Ϋ=Φ[Ő];if(Ϋ.φ.Length()!=0f){ǜ=ΔΖ*Math.Min(
Αζ(Ϋ.γ,Ϋ.β*-1,Ϋ.φ,Ϋ),
Αζ(Ϋ.γ*-1,Ϋ.β*-1,Ϋ.φ,Ϋ));if(ǜ<Я||Я==-1) Я=ǜ;}else Βε=Ϋ;Ő++;}Ő=-1;}bool ΔΞ=true;float ġ=0;if(Βε!=null){for(int i=0;i<ǚ.Count;i++){String ѝ=ǚ.Keys.ElementAt(i);float[,]Ǜ=ǚ.Values.ElementAt(i);float Ƹ=0;if(!Εξ(Βε,ѝ,out Ƹ)){ΔΞ=false;break;}for(int a=0;a<Ǜ.GetLength(0);a++)
for(int b=0;b<Ǜ.GetLength(1);b++){float ĳ=Math.Abs(Ǜ[a,b]*Ƹ);if(ĳ==0) continue;ΔΞ=true;if(ġ==0||ĳ<ġ) ġ=ĳ;}}}if(!ΔΞ){for(int a=0;a<Ǵ.GetLength(0);a++)
for(int b=0;b<Ǵ.GetLength(1);b++){float ĳ=Math.Abs(Ǵ[a,b]);if(ĳ==0) continue;if(ġ==0||ĳ<ġ) ġ=ĳ;}}if(ġ>0){float Ιρ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǜ=Б(ġ,Ιρ);if(ǜ>0)
if(ǜ<Я||Я==-1) Я=ǜ;}}void ΕΡ(Vector3 Ǩ,bool З){if(!З){for(int i=0;i<ȣ.Count;i++)
ȣ[i].SetValueFloat("Override",0.0f);return;}Vector3 Γϊ=ΒЎ(Ǩ,null);float Κα=Math.Min(1,Math.Abs(Б(Ǩ.X,Γϊ.X)));float Κβ=Math.Min(1,Math.Abs(Б(Ǩ.Y,Γϊ.Y)));float Κγ=Math.Min(1,Math.Abs(Б(Ǩ.Z,Γϊ.Z)));for(int i=0;i<ȣ.Count;i++){IMyThrust у=ȣ[i];Vector3 ρ=Θγ(п(Ψ,у.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǩ.X))
у.SetValueFloat("Override",у.MaxThrust*Κα);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǩ.Y))
у.SetValueFloat("Override",у.MaxThrust*Κβ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǩ.Z))
у.SetValueFloat("Override",у.MaxThrust*Κγ);else
у.SetValueFloat("Override",0.0f);}}void Αυ(bool З,float Ιώ,float ћ,float Ǘ,float ǝ){for(int i=0;i<ΒΕ.Count;i++){IMyGyro В=ΒΕ[i];В.GyroOverride=З;if(!З) В.GyroPower=100;else В.GyroPower=Ιώ;if(!З) continue;Vector3 Ч=Ψ.WorldMatrix.Forward;Vector3 л=Ψ.WorldMatrix.Right;Vector3 Ќ=Ψ.WorldMatrix.Up;
Vector3 ū=В.WorldMatrix.Forward;Vector3 ŭ=В.WorldMatrix.Up;Vector3 Ŭ=В.WorldMatrix.Left*-1;if(ū==Ч) В.SetValueFloat("Roll",ǝ);else if(ū==(Ч*-1)) В.SetValueFloat("Roll",ǝ*-1);else if(ŭ==(Ч*-1)) В.SetValueFloat("Yaw",ǝ);else if(ŭ==Ч) В.SetValueFloat("Yaw",ǝ*-1);else if(Ŭ==Ч) В.SetValueFloat("Pitch",ǝ);
else if(Ŭ==(Ч*-1)) В.SetValueFloat("Pitch",ǝ*-1);if(Ŭ==(л*-1)) В.SetValueFloat("Pitch",ћ);else if(Ŭ==л) В.SetValueFloat("Pitch",ћ*-1);else if(ŭ==л) В.SetValueFloat("Yaw",ћ);else if(ŭ==(л*-1)) В.SetValueFloat("Yaw",ћ*-1);else if(ū==(л*-1)) В.SetValueFloat("Roll",ћ);
else if(ū==л) В.SetValueFloat("Roll",ћ*-1);if(ŭ==(Ќ*-1)) В.SetValueFloat("Yaw",Ǘ);else if(ŭ==Ќ) В.SetValueFloat("Yaw",Ǘ*-1);else if(Ŭ==Ќ) В.SetValueFloat("Pitch",Ǘ);else if(Ŭ==(Ќ*-1)) В.SetValueFloat("Pitch",Ǘ*-1);else if(ū==Ќ) В.SetValueFloat("Roll",Ǘ);else if(ū==(Ќ*-1)) В.SetValueFloat("Roll",Ǘ*-1);
}}void ΕА(){this.ΔΛ=false;}void д(Vector3 Ě,Vector3 Ч,Vector3 Δή,float ĉ,bool ΑΠ){Βτ(Ě,ĉ,ΑΠ);Αύ=ĉ;Δα=false;this.ΔЉ=Ч;this.Ηθ=Δή;}void д(Vector3 Ě,Vector3 Ч,Vector3 Δή,bool ΑΠ){д(Ě,Ч,Δή,2f,ΑΠ);}void Βτ(Vector3 Ě,float ĉ,bool ΑΠ){Αύ=ĉ;this.ΔΛ=true;this.ΖΘ=ΑΠ;Δα=true;
ΒΚ=false;this.ΔЈ=Ě;}void Γο(){Ƕ(false,false,false,Ђ,0);Βθ=false;}void ж(Vector3 λ,float Ǳ){Ƕ(true,false,false,λ,Ǳ);}void Ƕ(bool З,bool ΔЂ,bool ΕΖ,Vector3 λ,float Ǳ){Ƕ(З,ΔЂ,ΕΖ,λ,λ-ε,0.0f,Ǳ);}void Ƕ(bool З,bool ΔЂ,bool ΕΖ,Vector3 λ,Vector3 ΙЋ,float ΙЌ,float Ǳ){Βθ=true;
this.Ηι=З;Ђ=λ;this.Δη=Ǳ;this.Δς=ΙЌ;this.ΗΩ=ΔЂ;this.ΗΨ=ΕΖ;this.Ζτ=ΙЋ;Э=Vector3.Distance(λ,ε);}void Θε(){χ.GetBlocksOfType(Ƨ,џ);}void Θζ(){if(Βΰ){Ћ=2;return;}List<IMyRemoteControl>ΑΒ=new List<IMyRemoteControl>();List<IMySensorBlock>ΗΦ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑΙ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(ΑΒ,џ);χ.GetBlocksOfType(У,џ);χ.GetBlocksOfType(ΗΦ,џ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΙ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);У=Ǟ(У,pamTag,true);ΕЉ();ΕЊ(ΑΙ);į(У,setLCDFontAndSize,1.4f,false);į(ȉ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ηγ=Ǟ(ΗΦ,pamTag,true);if(Ηγ.Count>0) ψ=Ηγ[0];else ψ=null;if(Κ==Ι.Ɲ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){Ћ=1;Λ="Drills are missing";}}else if(Κ==Ι.ǟ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){Ћ=1;Λ="Grinders are missing";}if(ψ==null){Ћ=1;Λ="Sensor is missing";}}else if(Κ==Ι.ά){χ.GetBlocksOfType(ΔЎ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ηβ=Ǟ(ΑΒ,pamTag,true);if(Ηβ.Count>0) ΑΒ=Ηβ;if(ΑΒ.Count>0) Ψ=ΑΒ[0];else{Ψ=null;Ћ=2;Λ="Remote is missing";return;}ο=(float) Ψ.CubeGrid.WorldVolume.Radius*2;ǖ=null;if(Κ!=Ι.ά){ΕΧ();if(ω.Count>0&&ǖ!=null){if(ψ!=null&&(ǖ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ψ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ψ.WorldMatrix.Down==ψ.WorldMatrix.Down))){Ћ=1;Λ="Wrong sensor direction";}if(ǖ.WorldMatrix.Forward!=Ψ.WorldMatrix.Forward&&
ǖ.WorldMatrix.Forward!=Ψ.WorldMatrix.Down){Ћ=2;Λ="Wrong remote direction";}}}}void Θη(){χ.GetBlocksOfType(Ƞ,џ);χ.GetBlocksOfType(ѓ,џ);χ.GetBlocksOfType(ȣ,џ);χ.GetBlocksOfType(ΒΕ,џ);χ.GetBlocksOfType(Ė,џ);χ.GetBlocksOfType(Ģ,џ);χ.GetBlocksOfType(ǒ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Βδ,џ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ѓ=Ǟ(ѓ,"ConnectorMedium",false);else
ѓ=Ǟ(ѓ,"Connector",false);List<IMyShipConnector>Ηΰ=Ǟ(ѓ,pamTag,true);if(Ηΰ.Count>0) ѓ=Ηΰ;if(Ћ<=1){if(ѓ.Count==0){Ћ=1;Λ="Connector is missing";}if(ΒΕ.Count==0){Ћ=1;Λ="Gyros are missing";}if(ȣ.Count==0){Ћ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Ηδ=Ǟ(Βδ,pamTag,true);
if(Ηδ.Count>0) Βδ=Ηδ;List<IMyLandingGear>Ηα=Ǟ(Ƞ,pamTag,true);if(Ηα.Count>0) Ƞ=Ηα;for(int i=0;i<Ƞ.Count;i++) Ƞ[i].AutoLock=false;List<IMyBatteryBlock>Ηί=Ǟ(Ė,pamTag,true);if(Ηί.Count>0) Ė=Ηί;List<IMyGasTank>Ηε=Ǟ(ǒ,pamTag,true);if(Ηε.Count>0) ǒ=Ηε;}void Θθ(){χ.GetBlocksOfType(Ĵ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ũ.Clear();for(int i=Ĵ.Count-1;i>=0;i--){if(ΘΣ(Ĵ[i])){Ũ.Add(Ĵ[i]);Ĵ.RemoveAt(i);}}}bool ΘΣ(IMyTerminalBlock Χ){if(Χ is IMyCargoContainer) return true;if(Χ is IMyShipDrill) return true;if(Χ is IMyShipGrinder) return true;if(Χ is IMyShipConnector){if(((IMyShipConnector) Χ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓΑ(Χ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕΧ(){if(Ψ==null) return;ǖ=null;float ΒΩ=0,ΒΤ=0,ΒΪ=0,ΒΥ=0,ΒΫ=0,ΒΦ=0;List<IMyTerminalBlock>ΔЇ=Ǟ(ω,pamTag,true);bool Ιχ=ΔЇ.Count==0;if(ΔЇ.Count>0) ǖ=ΔЇ[0];else if(ω.Count>0) ǖ=ω[0];int Ȝ=0;float ƀ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward){if(Ιχ){Ћ=2;Λ="Mining direction is unclear!";return;}continue;}Ȝ++;ƀ=Math.Max(ƀ,Θφ(ω[i]));Vector3 Й=Αε(Ψ,ω[i].GetPosition());if(i==0){ΒΩ=Й.X;ΒΤ=Й.X;ΒΪ=Й.Y;ΒΥ=Й.Y;ΒΫ=Й.Z;ΒΦ=Й.Z;}ΒΤ=Math.Max(Й.X,ΒΤ);ΒΩ=Math.Min(Й.X,ΒΩ);
ΒΥ=Math.Max(Й.Y,ΒΥ);ΒΪ=Math.Min(Й.Y,ΒΪ);ΒΦ=Math.Max(Й.Z,ΒΦ);ΒΫ=Math.Min(Й.Z,ΒΫ);}if(ƀ==0) ƀ=drillRadius;bool Ě=ǖ!=null&&ǖ.WorldMatrix.Forward==Ψ.WorldMatrix.Down;ȍ=(ΒΤ-ΒΩ)*(1-ő)+ƀ*2;Ȍ=(Ě?ΒΦ-ΒΫ:ΒΥ-ΒΪ)*(1-ļ)+ƀ*2;IMyCubeGrid g=Ψ.CubeGrid;float ΕΛ=0,ΕΜ=0,ΕΝ=0,ΕΞ=0;
for(int c=0;c<8;c++){Vector3 p=Αε(Ψ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ě?p.Z:p.Y;if(c==0||p.X<ΕΛ) ΕΛ=p.X;if(c==0||p.X>ΕΜ) ΕΜ=p.X;if(c==0||y<ΕΝ) ΕΝ=y;if(c==0||y>ΕΞ) ΕΞ=y;}Δώ=ΕΜ-ΕΛ+g.GridSize;Δύ=ΕΞ-ΕΝ+g.GridSize;}float Θφ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǟ<T>(List<T>Ј,String ǲ,bool ΒΖ){List<T>έ=new List<T>();for(int i=0;i<Ј.Count;i++)
if(ΓΑ(Ј[i],ǲ,ΒΖ)) έ.Add(Ј[i]);return έ;}bool ΓΑ<T>(T Χ,String ǲ,bool ΒΖ){IMyTerminalBlock b=(IMyTerminalBlock) Χ;if(ΒΖ&&b.CustomName.ToUpper().Contains(ǲ.ToUpper())) return true;if(!ΒΖ&&b.BlockDefinition.ToString().ToUpper().Contains(ǲ.ToUpper())) return true;return false;}void ǎ<T>(List<T>Ј,bool З)
{for(int i=0;i<Ј.Count;i++)
ΕЇ((IMyTerminalBlock) Ј[i],З);}void ΕЇ(IMyTerminalBlock Χ,bool З){if(Χ==null) return;String ΘЊ=З?"OnOff_On":"OnOff_Off";var a=Χ.GetActionWithName(ΘЊ);a.Apply(Χ);}void Ƿ(bool З){ΔЃ=З;if(!ō) return;ǎ(Βδ,З);}void Γζ(ChargeMode Њ){for(int i=0;i<Ė.Count;i++) Ė[i].ChargeMode=Њ;}void Γθ(bool ΚΒ)
{for(int i=0;i<ǒ.Count;i++) ǒ[i].Stockpile=ΚΒ;}void ơ(List<IMyLandingGear>ΔΗ,bool ΖЎ){for(int i=0;i<ΔΗ.Count;i++){if(ΖЎ) ΔΗ[i].Lock();if(!ΖЎ) ΔΗ[i].Unlock();}}void ΕЈ(bool З){if(Ζδ==З) return;List<IMyShipController>ΑІ=new List<IMyShipController>();χ.GetBlocksOfType(ΑІ,џ);if(ΑІ.Count==0) return;
for(int i=0;i<ΑІ.Count;i++)
ΑІ[i].DampenersOverride=З;Ζδ=З;}IMyShipConnector Ъ(MyShipConnectorStatus ќ){for(int i=0;i<ѓ.Count;i++){if(!ȓ(ѓ[i],true)) continue;if(ѓ[i].Status==ќ) return ѓ[i];}return null;}void ı(bool ΔϏ,bool ΑЋ,float Ιά,float ΘА){if(ψ==null||ω.Count==0) return;Vector3 ΑЂ=new Vector3();
int Ȝ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward) continue;Ȝ++;ΑЂ+=ω[i].GetPosition();}ΑЂ=ΑЂ/Ȝ;Vector3 ź=Αε(ψ,ΑЂ);ψ.Enabled=true;ψ.ShowOnHUD=ΔϏ;ψ.LeftExtend=(ΑЋ?1:ъ)/2f*ȍ-ź.X;ψ.RightExtend=(ΑЋ?1:ъ)/2f*ȍ+ź.X;ψ.TopExtend=(ΑЋ?1:щ)/2f*Ȍ+ź.Y;
ψ.BottomExtend=(ΑЋ?1:щ)/2f*Ȍ-ź.Y;ψ.FrontExtend=(ΔϏ?з:Ιά)-ź.Z;ψ.BackExtend=ΔϏ?0:ΘА+ο*0.75f+ź.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void į<T>(List<T>Ј,bool ΙЎ,float ΙΪ,bool Ιζ){for(int i=0;i<Ј.Count;i++){IMyTextSurface ΑΘ=null;if(Ј[i]is IMyTextSurface) ΑΘ=(IMyTextSurface) Ј[i];if(ΑΘ!=null){ΑΘ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΙЎ) continue;ΑΘ.Font="Debug";if(Ιζ) continue;ΑΘ.FontSize=ΙΪ;}}}bool Αθ(){Vector3D ξ;return this.Ψ.TryGetPlanetPosition(out ξ);}int ΘΗ(Vector3 ΚΔ,int ΗΪ){if(ΗΪ==-1) return 0;double Ц=-1;int ȭ=-1;for(int i=Φ.Count-1;i>=0;i--){double І=Vector3.Distance(Φ[i].Ε,ΚΔ);if(Ц==-1||І<Ц){ȭ=i;Ц=І;}
}return Math.Sign(ȭ-ΗΪ);}bool ΘΤ(Vector3 ξ){List<Vector3>Щ=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Щ.Add(Φ[i].Ε);if(Ξ.ζ&&Φ.Count>=1){Vector3 ţ=new Vector3();Ȓ(Ξ,dockDist*ο,false,out ţ);if(Vector3.Distance(Ξ.Ε,Φ.First().Ε)<Vector3.Distance(Ξ.Ε,Φ.Last().Ε)){Щ.Insert(0,ţ);Щ.Insert(0,Ξ.Ε);}
else{Щ.Add(ţ);Щ.Add(Ξ.Ε);}}if(Κ==Ι.ά){if(Ζ.ζ&&Φ.Count>=1){Vector3 Γφ=new Vector3();Ȓ(Ζ,dockDist*ο,false,out Γφ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Щ.Insert(0,Γφ);Щ.Insert(0,Ζ.Ε);}else{Щ.Add(Γφ);Щ.Add(Ζ.Ε);}}}else{if(α!=Π.ч)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Щ.Insert(0,Ζ.Ε);else Щ.Add(Ζ.Ε);}int ȭ=-1;double ǽ=-1;for(int i=0;i<Щ.Count;i++){double І=Vector3.Distance(Щ[i],ξ);if(І<ǽ||ǽ==-1){ǽ=І;ȭ=i;}}if(Щ.Count==0) return false;double ΓЎ=Vector3.Distance(Щ[ȭ],ξ);double ΙϏ=Vector3.Distance(Щ[Math.Max(0,ȭ-1)],Щ[ȭ])*1.5f;
double Ιφ=Vector3.Distance(Щ[Math.Min(Щ.Count-1,ȭ+1)],Щ[ȭ])*1.5f;return ΓЎ<ΙϏ||ΓЎ<Ιφ||ΓЎ<laneOffset*ο*1.5f;}static float Б(float a,float b){if(b==0) return 0;return a/b;}Vector3 Θγ(Vector3 v,int ΓЋ){return new Vector3(Math.Round(v.X,ΓЋ),Math.Round(v.Y,ΓЋ),Math.Round(v.Z,ΓЋ));
}float ŕ(Vector3 a,Vector3 b){if(a==b) return 0;float ΙΦ=(a*b).Sum;float ΖЉ=a.Length();float ΖЊ=b.Length();if(ΖЉ==0||ΖЊ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΙΦ/(ΖЉ*ΖЊ)));if(float.IsNaN(έ)) return 0;return έ;}float ĩ(Vector3 v,Vector3 Βή,float ΙЏ){float š=ŕ(v,Βή);
if(ΙЏ>0f) š*=-1;if(š>-90f) return š-90f;else return 180f-(-š-90f);}double Εή(float ΙΞ){return(Math.PI/180)*ΙΞ;}bool ΓΔ(double Ιπ,double value,double ŷ){return(value>=Ιπ&&value<=ŷ);}Vector3 Θχ(IMyTerminalBlock Χ,Vector3 Й){return Vector3D.Transform(Й,Χ.WorldMatrix);
}Vector3 Αε(IMyTerminalBlock Χ,Vector3 Κή){return п(Χ,Κή-Χ.GetPosition());}Vector3 п(IMyTerminalBlock Χ,Vector3 ΕΚ){return Vector3D.TransformNormal(ΕΚ,MatrixD.Transpose(Χ.WorldMatrix));}Vector3 İ(Vector3 ȁ,Vector3 Ќ,Vector3 ΕΚ){MatrixD Δζ=MatrixD.CreateFromDir(ȁ,Ќ);return Vector3D.TransformNormal(ΕΚ,MatrixD.Transpose(Δζ));
}Vector3 Γπ(Vector3 ȁ,Vector3 Ќ,Vector3 Й){MatrixD Δζ=MatrixD.CreateFromDir(ȁ,Ќ);return Vector3D.Transform(Й,Δζ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ο(τ,0)),float.Parse(Ο(τ,1)),float.Parse(Ο(τ,2)));
}String ΘЈ(Ϊ Ϋ){String М=":";String έ=σ(Ϋ.Ε)+М+σ(Ϋ.γ)+М+
σ(Ϋ.β)+М+σ(Ϋ.Ѐ)+М+σ(Ϋ.φ);for(int i=0;i<Ϋ.ǳ.Length;i++)
έ+=М+Math.Round(Ϋ.ǳ[i],3);return έ;}Ϊ Θς(String s){String[]τ=s.Split(':');Ϊ Ϋ=new Ϊ(
ς(Ο(τ,0)),ς(Ο(τ,1)),
ς(Ο(τ,2)),ς(Ο(τ,3)),
ς(Ο(τ,4)));int Α=5;List<float>Ζρ=new List<float>();while(Α<τ.Length){String Βξ=Ο(τ,Α);float f=0;if(!float.TryParse(Βξ,out f)) break;Ζρ.Add(f);Α++;}Ϋ.ǳ=Ζρ.ToArray();return Ϋ;}System.Text.StringBuilder Ƅ=new System.Text.StringBuilder();void Υ<T>(T value,bool Ιυ){if(Ιυ) Ƅ.Append('\n');Ƅ.Append(value);}
void Υ<T>(T value){Υ(value,true);}String Αή(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Αω(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ο(String[]ΘЎ,int Α){String Βξ=ΘЎ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Βξ)) return "";
return Βξ;}void Save(){if(Βΰ||Κ==Ι.ё){Storage="";return;}Storage=Βϊ();}String Βϊ(){Ƅ.Clear();Ƅ.Append(DATAREV+";");Υ(σ(Βί),false);Υ(σ(Ξ.γ));Υ(σ(Ξ.Ѐ));Υ(σ(Ξ.β));Υ(σ(Ξ.φ));Υ(σ(Ξ.Ε));Υ(σ(Ξ.и));Υ(Ξ.ζ);Υ(σ(Ζ.Ε));Υ(σ(Ζ.φ));Υ(σ(Ζ.γ));Υ(σ(Ζ.β));Υ(σ(Ζ.Ѐ));Υ(σ(Ζ.и));Υ(Ζ.ζ);Υ(σ(ǭ));Υ(σ(ă));Υ(";");
Υ((int) Κ,false);Υ((int) α);Υ((int) ї);Υ(ȃ);Υ(ј);Υ(к);Υ(љ);Υ(Ǹ);Υ(ċ);Υ(Ċ);if(Κ==Ι.ά){Υ((int) Ѓ.Н);Υ(Ѓ.Ǧ);Υ(Ѓ.Ĝ);Υ(Αή(Ѓ.ǧ));Υ(Αή(Ѓ.Ǫ));Υ((int) Є.Н);Υ(Є.Ǧ);Υ(Є.Ĝ);Υ(Αή(Є.ǧ));Υ(Αή(Є.Ǫ));}else{Υ((int) ȇ);Υ((int) ь);Υ((int) Ю);Υ((int) є);Υ((int) Ŀ);Υ(ĥ);Υ(ō);Υ(Ű);Υ(Ū);Υ(ъ);Υ(щ);Υ(з);Υ(ƒ);Υ(Ƒ);
Υ(ő);Υ(ļ);Υ(ν);Υ(И);Υ(ώ);Υ(ό);Υ(ā);Υ(Ь);Υ(Ŋ);Υ(ł);}Υ(";");for(int i=0;i<Ȣ.Count;i++)
Υ((i>0?"|":"")+Ȣ[i],false);Υ(";");for(int i=0;i<Φ.Count;i++)
Υ(ΘЈ(Φ[i]),i>0);Υ(";",false);Υ(й+"|"+а,false);Υ(";",false);Υ(ΗЋ(),false);String Τ=Ƅ.ToString();Ƅ.Clear();return Τ;}ђ Θβ(){return Αρ(Storage);}ђ Αρ(String Τ){if(Τ=="") return ђ.ΘΫ;String[]ŋ=Τ.Split(';');if(Ο(ŋ,0)!=DATAREV) return ђ.ΓΡ;int Α=0;try{String[]Ν=Ο(ŋ,1).Split('\n');
Βί=ς(Ο(Ν,Α++));Ξ.γ=ς(Ο(Ν,Α++));Ξ.Ѐ=ς(Ο(Ν,Α++));Ξ.β=ς(Ο(Ν,Α++));Ξ.φ=ς(Ο(Ν,Α++));Ξ.Ε=ς(Ο(Ν,Α++));Ξ.и=ς(Ο(Ν,Α++));Ξ.ζ=bool.Parse(Ο(Ν,Α++));Ζ.Ε=ς(Ο(Ν,Α++));Ζ.φ=ς(Ο(Ν,Α++));Ζ.γ=ς(Ο(Ν,Α++));Ζ.β=ς(Ο(Ν,Α++));Ζ.Ѐ=ς(Ο(Ν,Α++));Ζ.и=ς(Ο(Ν,Α++));Ζ.ζ=bool.Parse(Ο(Ν,Α++));
ǭ=ς(Ο(Ν,Α++));ă=ς(Ο(Ν,Α++));Ν=Ο(ŋ,2).Split('\n');Α=0;Κ=(Ι) int.Parse(Ο(Ν,Α++));α=(Π) int.Parse(Ο(Ν,Α++));ї=(Π) int.Parse(Ο(Ν,Α++));ȃ=int.Parse(Ο(Ν,Α++));ј=int.Parse(Ο(Ν,Α++));к=int.Parse(Ο(Ν,Α++));љ=int.Parse(Ο(Ν,Α++));Ǹ=float.Parse(Ο(Ν,Α++));ċ=bool.Parse(Ο(Ν,Α++));
Ċ=bool.Parse(Ο(Ν,Α++));if(Κ==Ι.ά){Ѓ.Н=(π) int.Parse(Ο(Ν,Α++));Ѓ.Ǧ=float.Parse(Ο(Ν,Α++));Ѓ.Ĝ=float.Parse(Ο(Ν,Α++));Ѓ.ǧ=Αω(Ο(Ν,Α++));Ѓ.Ǫ=Αω(Ο(Ν,Α++));Є.Н=(π) int.Parse(Ο(Ν,Α++));Є.Ǧ=float.Parse(Ο(Ν,Α++));Є.Ĝ=float.Parse(Ο(Ν,Α++));Є.ǧ=Αω(Ο(Ν,Α++));Є.Ǫ=Αω(Ο(Ν,Α++));}else
{ȇ=(Ǐ) int.Parse(Ο(Ν,Α++));ь=(х) int.Parse(Ο(Ν,Α++));Ю=(Ѝ) int.Parse(Ο(Ν,Α++));є=(ў) int.Parse(Ο(Ν,Α++));Ŀ=(Ǐ) int.Parse(Ο(Ν,Α++));ĥ=bool.Parse(Ο(Ν,Α++));ō=bool.Parse(Ο(Ν,Α++));Ű=bool.Parse(Ο(Ν,Α++));Ū=bool.Parse(Ο(Ν,Α++));ъ=int.Parse(Ο(Ν,Α++));щ=int.Parse(Ο(Ν,Α++));з=int.Parse(Ο(Ν,Α++));
ƒ=float.Parse(Ο(Ν,Α++));Ƒ=float.Parse(Ο(Ν,Α++));ő=float.Parse(Ο(Ν,Α++));ļ=float.Parse(Ο(Ν,Α++));ν=int.Parse(Ο(Ν,Α++));И=int.Parse(Ο(Ν,Α++));ώ=int.Parse(Ο(Ν,Α++));ό=int.Parse(Ο(Ν,Α++));ā=int.Parse(Ο(Ν,Α++));Ь=int.Parse(Ο(Ν,Α++));Ŋ=int.Parse(Ο(Ν,Α++));ł=float.Parse(Ο(Ν,Α++));
}Ν=Ο(ŋ,3).Replace("\n","").Split('|');Ȣ=Ν.ToList();Ν=Ο(ŋ,4).Split('\n');Φ.Clear();if(Ν.Count()>=1&&Ν[0]!="")
for(int i=0;i<Ν.Length;i++)
Φ.Add(Θς(Ο(Ν,i)));Ν=Ο(ŋ,5).Split('|');й=Ο(Ν,0);if(!int.TryParse(Ο(Ν,1),out а)) а=-1;if(й.Length!=ν*И){й="";а=-1;}ΗЊ(Ο(ŋ,6));}catch{return ђ.Εα;}ƅ=ї;Ϗ();return ђ.Αχ;}const int ΕΥ=64000;IMyTerminalBlock Εβ(){List<IMyTerminalBlock>Ŧ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ŧ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ŧ.Count>0?Ŧ[0]:null;}void ΗЏ(){IMyTerminalBlock Χ=Εβ();if(Χ==null){Λ="Export: name a block "+dataTag;return;}String Τ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Βϊ();if(Τ.Length>ΕΥ){Λ="Export too large: "+Τ.Length+" chars, max "+ΕΥ;return;}Χ.CustomData=Τ;Λ="Exported "+Φ.Count+" waypoints";}void ΘΟ(bool ΔΨ){IMyTerminalBlock Χ=Εβ();if(Χ==null){Λ="Import: name a block "+dataTag;return;}
ΘΠ(Χ.CustomData,ΔΨ);}void ΘΠ(String Τ,bool ΔΨ){Τ=Τ.Replace("\r","");if(Τ.StartsWith("//")){int ΗΙ=Τ.IndexOf('\n');Τ=ΗΙ<0?"":Τ.Substring(ΗΙ+1);}Τ=Τ.Trim();if(Τ==""){Λ="Import: no PAM data in "+dataTag;return;}Ϗ();String ΖΟ=Βϊ();ђ έ=Αρ(Τ);if(έ==ђ.Αχ&&ΔΨ){Ϊ Ų=new Ϊ(Ζ);
Vector3 Ισ=ǭ,Ις=ă;int w=ъ,h=щ,d=з;Ǐ ΔЄ=ȇ;ў ΓЌ=є;float Ιϊ=ő,Ιω=ļ;Αρ(ΖΟ);Ζ=Ų;ǭ=Ισ;ă=Ις;ъ=w;щ=h;з=d;ȇ=ΔЄ;є=ΓЌ;ő=Ιϊ;ļ=Ιω;α=Π.Ŝ;ƥ(true,true);α=Π.Ŝ;ї=Π.Ŝ;Λ="Imported job, run CONT";return;}if(έ==ђ.Αχ){ǋ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}Αρ(ΖΟ);Λ=έ==ђ.ΓΡ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΕЉ(){ķ.Clear();for(int i=У.Count-1;i>=0;i--){String Τ=У[i].CustomData.ToUpper();bool ΔΦ=false;if(Τ=="INSTRUCTIONS"){ΔΦ=true;ΑЊ=true;}if(Τ=="DEBUG") ΔΦ=true;if(ΔΦ){ķ.Add(У[i]);У.RemoveAt(i);}
}į(ķ,false,1,false);}void ΕЊ(List<IMyTerminalBlock>Ŧ){ȉ.Clear();for(int i=0;i<Ŧ.Count;i++){IMyTerminalBlock Χ=Ŧ[i];try{String ΗΠ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Χ.CustomName.IndexOf(ΗΠ);int Ɗ=-1;if(Α<0||!int.TryParse(Χ.CustomName.Substring(Α+ΗΠ.Length,1),out Ɗ)) continue;
if(Ɗ==-1) continue;Ɗ--;IMyTextSurfaceProvider ΗΣ=(IMyTextSurfaceProvider) Χ;if(Ɗ<ΗΣ.SurfaceCount&&Ɗ>=0)
ȉ.Add(ΗΣ.GetSurface(Ɗ));}catch{}}}String ΒΧ="";void ΕЄ(){String Δε="";String Ʒ="";ΒΧ=ΕΤ(false);Δε+=ΒΧ;Ʒ+=ΒΧ;Ʒ+=ΘΕ();for(int i=0;i<У.Count;i++) У[i].WriteText(Δε);for(int i=0;i<ȉ.Count;i++) ȉ[i].WriteText(Δε);Echo(Ʒ);for(int i=0;i<ķ.Count;i++){IMyTextPanel т=ķ[i];
String Τ=т.CustomData.ToUpper();if(Τ=="DEBUG") т.WriteText(""+"\n"+"");if(Τ=="INSTRUCTIONS") т.WriteText(Ει());}}string Ει(){String Γ="";try{float ΖБ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ť*ΖБ)+" / "+(int)(Ÿ*ΖБ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ť*100f,1)+"% / "+Math.Round(Ÿ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ƫ,2)+"ms / "+Math.Round(Δι,2)+"ms \n";}catch{}for(int i=0;i<Ą.Count;i++)
Γ+=""+Ą.Keys.ElementAt(i)+" = "+Math.Round(Ą.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ą.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΘΝ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";į(У,setLCDFontAndSize,0.9f,false);į(ȉ,setLCDFontAndSize,0.9f,true);for(int i=0;i<У.Count;i++) У[i].WriteText(Γ+Ā.ToString());for(int i=0;i<ȉ.Count;i++) ȉ[i].WriteText(Γ+Ā.ToString());}void ȕ(){if(!ΑЊ) return;try{Δφ=Runtime.CurrentInstructionCount;}catch{}
}void ȑ(String ή){if(!ΑЊ) return;if(Δφ==0) return;try{float Βμ=(Runtime.CurrentInstructionCount-Δφ)/Runtime.MaxInstructionCount*100;if(!Ą.ContainsKey(ή))
Ą.Add(ή,new float[]{Βμ,Βμ});else{Ą[ή][0]=Βμ;Ą[ή][1]=Math.Max(Βμ,Ą[ή][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ηχ(String Ȁ){if(!ŧ) return;String М=""+BROADCAST_SEP;String η=VERSION+М;η+=Ɠ(ƈ)+М;
η+=(int) Κ+М;η+=Ř(ε.X)+""+М;η+=Ř(ε.Y)+М;η+=Ř(ε.Z)+М;if(Κ!=Ι.ά)
η+=ΒЋ(ї)+(ї==Π.Д?" "+Math.Round(Ǆ,1)+"%":"")+М;else η+=ΒЋ(ї)+М;η+=Ɠ(Λ)+М;η+=Љ+""+М;η+=Ɠ(ΒΧ)+М;η+=Ǿ+М;η+=Ȅ+М;for(int i=0;i<ǩ.Count;i++){if(ǩ[i].ŵ!=Х.Ś) continue;η+=Ɠ(ǩ[i].ή)+"/"+Ɠ(ǩ[i].υ)+"/"+ǩ[i].Ѕ+М;}Θμ(η,Ȁ);}String Ɠ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Θμ(String η,String Ȁ){Γδ(η,BROADCAST_SELF_ID,Ȁ);}void Γδ(String η,String λ,String Ȁ){try{if(!ŧ) return;η=BROADCAST_TAG+BROADCAST_SEP+Εε()+BROADCAST_SEP+λ+BROADCAST_SEP+ΓЂ+BROADCAST_SEP+Ȁ+BROADCAST_SEP+η;this.IGC.SendBroadcastMessage(BROADCAST_TAG,η);}catch(Exception e){Ā=e;}
}String Εε(){if(Κ!=Ι.ё) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΓΙ(String η){return η.StartsWith(BROADCAST_TAG);}bool б(ref String Τ,out string ƹ,Char М){int Α=Τ.IndexOf(М);ƹ="";if(Α<0) return false;ƹ=Τ.Substring(0,Α);Τ=Τ.Remove(0,Α+1);return true;}bool ΕЃ(ref String η,out String Ȇ,out String Ȁ)
{Ȇ="";Ȁ="";if(!ŧ) return false;String ǲ="";if(!б(ref η,out ǲ,BROADCAST_SEP)||!ΓΙ(ǲ)) return false;if(!б(ref η,out Ȇ,BROADCAST_SEP)) return false;if(!б(ref η,out ǲ,BROADCAST_SEP)||(ǲ!=Εε()&&(ǲ!="*"&&Κ!=Ι.ё))) return false;if(!б(ref η,out ǲ,BROADCAST_SEP)||(ǲ!=ΓЂ)) return false;
if(!б(ref η,out Ȁ,BROADCAST_SEP)) return false;return true;}void Ετ(){bool ư=true;if(Ζσ){Ζσ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ƾ=(Κ!=Ι.ё?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ƾ)
Ευ(ƾ);String[]Ν=Me.CustomData.Split('\n');ŧ=bool.Parse(Απ(Αΰ(Ν,"Enable_Broadcast",ref ư)));bool ΕΘ=false;bool Ζθ=true;if(ŧ){if(Κ!=Ι.ё)
ƈ=Απ(Αΰ(Ν,"Ship_Name",ref ư)).Replace(BROADCAST_SELF_ID,"");ΓЂ=Απ(Αΰ(Ν,"Broadcast_Channel",ref ư)).ToLower();Ζθ=false;if(Ǥ==null){Ǥ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǥ.SetMessageCallback("");}List<IMyRadioAntenna>Ţ=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ţ);bool Γυ=false;
for(int i=0;i<Ţ.Count;i++)
if(Ţ[i].EnableBroadcasting&&Ţ[i].Enabled){Γυ=true;break;}if(Ţ.Count==0) Λ="No Antenna found";else if(!Γυ) Λ="Antenna not ready";ΕΘ=Ţ.Count==0||!Γυ;if(ΖΙ&&!ΕΘ&&Κ!=Ι.ё) Λ="Antenna ok";}else if(Κ==Ι.ё)
Λ="Offline - Enable in PB custom data";ΖΙ=ΕΘ;if(Ζθ){if(Ǥ!=null) this.IGC.DisableBroadcastListener(Ǥ);Ǥ=null;}}catch{ư=false;}if(!ư){Λ="Reset custom data";Ευ(ƾ);}}void Ευ(String ƾ){String Τ=ƾ+"\n\n"+"Enable_Broadcast="+(Κ==Ι.ё?"true":"false")+" \n";Τ+=Κ!=Ι.ё?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Τ+"Broadcast_Channel=#default";}String Απ(String Ł){int Α=Ł.IndexOf("//");if(Α!=-1) Ł=Ł.Substring(0,Α);String[]τ=Ł.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αΰ(String[]Ν,String ų,ref bool Ņ){foreach(String Ł in Ν)
if(Ł.StartsWith(ų)) return Ł;Ņ=false;return "";}void ΗЇ(string Р){if(ȯ) Ы=new List<К>();String Ȇ="";String Ȁ="";if(!ΓΙ(Р)) Θΰ(Р);bool ΗΧ=false;int Δμ=50;while(Ǥ!=null&&Ǥ.HasPendingMessage&&Δμ-->0){MyIGCMessage η=Ǥ.AcceptMessage();String Τ=η.Data as String;if(Τ==null) continue;
if(ΕЃ(ref Τ,out Ȇ,out Ȁ)&&Ȇ!=""&&Ȇ!=BROADCAST_SELF_ID){К Δ=ΒЉ(Ȇ,"");if(Δ==null){Δ=new К(Ȇ);Ы.Add(Δ);}Δ.ŝ(Ȁ,false,false,true);ΘЃ(Δ,Τ);ΗΧ=true;}}if(ΗΧ)
Ы.Sort(delegate(К a,К b){if(a.ή==null&&b.ή==null) return 0;else if(a.ή==null) return-1;else if(b.ή==null) return 1;else return a.ή.CompareTo(b.ή);});if(ȳ||ȯ){if(ΑΓ<=0||ȯ){Λ="";ȯ=false;Θκ();Ετ();Δέ=DateTime.Now;}}if(ǂ) Θό();Θϊ();}void ΘЃ(К Δ,String Τ){Δ.ΔΪ=DateTime.Now;
String ƹ="";б(ref Τ,out Δ.Βπ,BROADCAST_SEP);б(ref Τ,out Δ.ή,BROADCAST_SEP);if(Δ.Βπ!=VERSION) return;String Ǌ="";String[]ſ=new string[3];б(ref Τ,out Ǌ,BROADCAST_SEP);б(ref Τ,out ſ[0],BROADCAST_SEP);б(ref Τ,out ſ[1],BROADCAST_SEP);б(ref Τ,out ſ[2],BROADCAST_SEP);б(ref Τ,out Δ.ΔЅ,BROADCAST_SEP);
б(ref Τ,out Δ.Λ,BROADCAST_SEP);б(ref Τ,out ƹ,BROADCAST_SEP);б(ref Τ,out Δ.ΒΨ,BROADCAST_SEP);String Ηπ="",ΗΓ="";б(ref Τ,out Ηπ,BROADCAST_SEP);б(ref Τ,out ΗΓ,BROADCAST_SEP);Δ.Ї.Clear();while(true){String ΖЄ;if(!б(ref Τ,out ΖЄ,BROADCAST_SEP)) break;String[]ΒΜ=ΖЄ.Split('/');if(ΒΜ.Count()<3) continue;
int Ѕ=0;if(!int.TryParse(ΒΜ[2],out Ѕ)) continue;Δ.Ї.Add(new е(ΒΜ[0],ΒΜ[1],Ѕ,Х.Ś));}int ΗΖ=0;if(!int.TryParse(ƹ,out Δ.Љ)) Δ.Љ=0;if(!int.TryParse(Ǌ,out ΗΖ)) Δ.Κ=Ι.Ĕ;if(!float.TryParse(ſ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ſ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(ſ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(Ηπ,out Δ.Ǿ)) Δ.Ǿ=0;if(!float.TryParse(ΗΓ,out Δ.Ȅ)) Δ.Ȅ=0;Δ.Κ=(Ι) ΗΖ;Δ.ΓЏ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΔБ=0;for(int i=0;i<Δ.Ї.Count();i++) Δ.ΔБ+=Δ.Ї[i].Ѕ;}К ΒЉ(String δ,String ή){δ=δ.ToUpper();ή=ή.ToUpper();for(int i=0;i<Ы.Count;i++)
{if(δ!=""&&Ы[i].δ.ToUpper()==δ) return Ы[i];if(ή!=""&&Ы[i].ή!=null&&Ы[i].ή.ToUpper()==ή) return Ы[i];}return null;}void Ǡ(К Δ,String Ж){if(Δ==null){for(int i=0;i<Ы.Count;i++)
Ы[i].ŝ(Ж,true,false,false);Γδ(Ж,"*",Ж);}else{Δ.ŝ(Ж,true,false,false);Γδ(Ж,Δ.δ,Ж);}}void Θΰ(string Р){if(Р=="") return;var τ=Р.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ж=τ.ElementAtOrDefault(0);var ȗ=τ.ElementAtOrDefault(1);String ȟ="Invalid argument: "+Р;if(ϋ==Ρ.Ƣ){if(Г!=null)
{switch(Ж){case "UP":{if(Љ<2) break;if(Г.Љ==0){Љ=1;return;}Ǡ(Г,"UP");return;}case "DOWN":{if(Љ<1) break;if(Љ==1){Ǡ(Г,"MRES");break;}Ǡ(Г,"DOWN");break;}case "APPLY":{if(Љ<2) break;Ǡ(Г,"APPLY");return;}}}}switch(Ж){case "UP":this.ΓΞ(false);return;case "DOWN":this.ΓΝ(false);return;
case "APPLY":this.Ŗ(true);return;}switch(Ж){case "CLEAR":ΗЂ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȗ)){Λ="Usage: SENDALL <cmd>";return;}Ǡ(null,ȗ);return;case "SEND":
if(Р.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Θν(Р.Remove(0,"SEND".Length+1));return;}Λ=ȟ;}void Θν(String Ʃ){if(Ʃ=="") return;var τ=Ʃ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String ƈ=τ.ElementAtOrDefault(0).Trim();
К Δ=ΒЉ("",ƈ);if(Δ!=null) Ǡ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+ƈ;}void ΗЂ(){Ы.Clear();Г=null;θ(Ρ.Main);Љ=0;}void Θκ(){List<IMyTerminalBlock>ΑΙ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(У,џ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΙ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);У=Ǟ(У,pamTag,true);ΕЊ(ΑΙ);ΕЉ();Θλ();į(У,setLCDFontAndSize,1.15f,false);į(ȉ,setLCDFontAndSize,1.15f,true);String Ιβ=Εκ(false);String ΗΚ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock т in У){if(т.CustomData==""){т.CustomData=Εκ(true);continue;}
if(!т.CustomData.Contains(Ιβ))
if(!т.CustomData.Contains(ΗΚ)) т.CustomData=ΗΚ+т.CustomData;}}void Θϊ(){String Ʒ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȉ.Count;i++)
ȉ[i].WriteText(ΕΣ(П.Εύ,"0",false));for(int i=0;i<У.Count;i++){П Њ=П.Ĕ;String ǯ="";Θά(У[i],out Њ,out ǯ);У[i].WriteText(ΕΣ(Њ,ǯ,i==0));}Echo(Ʒ);for(int i=0;i<ķ.Count;i++){IMyTextPanel т=ķ[i];String Τ=т.CustomData.ToUpper();if(Τ=="DEBUG") т.WriteText(""+"\n"+"");if(Τ=="INSTRUCTIONS") т.WriteText(Ει());
}}String Ŗ(bool Θ){int ΙΨ=0;return Ŗ(Θ,0,ref ΙΨ,false,1);}String Ŗ(bool Θ,int Ιό,ref int ΙЊ,bool Ζέ,int ǌ){String Γ="";String Ƈ="——————————————————\n";String ΚΗ="--------------------------------------------\n";if(Г==null||ϋ==Ρ.Main||ϋ==Ρ.Γε)
Γ+="[PAM]-Controller | "+Ы.Count+" Connected ships"+"\n"+ΘΒ();else Γ+=Εν(Г)+"\n";Γ+=Ƈ;int Α=0;int Η=Љ;Ń=0;if(Λ!=""){Μ(ref Γ,Η,Α++,Θ,Λ);}else if(ϋ==Ρ.Main){if(Г==null&&Ы.Count>=1) Г=Ы[0];String Ј="";if(Μ(ref Ј,Η,Α++,Θ," Send command to all")) θ(Ρ.Γε);int Ɔ=0;
for(int s=0;s<Ы.Count;s++){if(Ζέ) Ј+="\n";К Δ=Ы[s];if(Η==Α||Η==Α+1) Г=Δ;if(Α==Η) Ɔ=Ј.Split('\n').Length-1;if(Μ(ref Ј,Η,Α++,Θ," "+Εν(Δ))){Г=Δ;θ(Ρ.Ƣ);Г.ŝ("",false,true,false);}if(Α==Η) Ɔ=Ј.Split('\n').Length-1;if(Μ(ref Ј,Η,Α++,Θ," "+(Ζέ?"":"  ")+ΘΘ(Δ)))
{Г=Δ;θ(Ρ.ΕЋ);}}int Ɛ=ǌ-2;Ɛ+=Math.Max(0,(Ιό-1))*ǌ;Γ+=Γγ(Ɛ,Ј,Ɔ,ref ΙЊ);}else if(ϋ==Ρ.Ατ||ϋ==Ρ.Γε){Ρ ΘЏ=ϋ==Ρ.Ατ?Ρ.Ƣ:Ρ.Main;К λ=null;if(ϋ==Ρ.Ατ) λ=Г;if(Μ(ref Γ,Η,Α++,Θ," Back")){θ(ΘЏ);}if(Μ(ref Γ,Η,Α++,Θ," [Stop] "+ř(λ,"STOP"))){Ǡ(λ,"STOP");}if(Μ(ref Γ,Η,Α++,Θ," [Continue] "+ř(λ,"CONT"))){Ǡ(λ,"CONT");}
if(Μ(ref Γ,Η,Α++,Θ," [Move home] "+ř(λ,"HOMEPOS"))){Ǡ(λ,"HOMEPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Move to job] "+ř(λ,"JOBPOS"))){Ǡ(λ,"JOBPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Full simulation] "+ř(λ,"FULL"))){Ǡ(λ,"FULL");}if(λ!=null&&λ.Κ==Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," [Undock] "+ř(λ,"UNDOCK"))){Ǡ(λ,"UNDOCK");}}else if(ϋ==Ρ.Ƣ){String Γό="";if(Μ(ref Γό,Η,Α++,Θ," Back")){θ(Ρ.Main);}Γ+=Γό.Substring(0,Γό.Length-1).PadRight(36,' ');if(Μ(ref Γ,Η,Α++,Θ," Send cmd")){θ(Ρ.Ατ);}if(!Г.Εϋ()&&!Г.ΓΖ()) Ń++;if(Г.ΓΖ()) Γ+="No answer received...";
else Γ+=ş(Г,Г.ΒΨ);}else if(ϋ==Ρ.ΕЋ){if(Μ(ref Γ,Η,Α++,Θ," Back")){θ(Ρ.Main);}Γ+=ΚΗ;Γ+=ş(Г,Εμ(Г));}if(!Δλ) Љ=Math.Min(Ń-1,Љ);return Γ;}String Εν(К Δ){if(Δ.Βπ!=VERSION) return Δ.ή+": Different version!";return Δ.ή+": "+ş(Δ,Δ.ΔЅ)+" "+Δ.ΓЏ+"m";}String ΘΘ(К Δ){String s=Γί("",Б(Δ.Ǿ,Δ.Ȅ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ї.Count;i++){if(i>=5) break;е ύ=Δ.Ї[i];s+=ΖΓ(Βϋ(ΘΖ(ύ)),3)+" "+Εγ(ύ.Ѕ)+" ";}return s;}String ΘΖ(е ύ){if(ύ.υ.ToUpper()=="ORE"||ύ.υ.ToUpper()=="INGOT"){String ΖΫ=ΘΔ(ύ.ή.ToUpper());if(ΖΫ!="") return ΖΫ;}return ύ.ή;}String ş(К Δ,String Γ){if(Δ.Εϋ()) return "No signal...("+Αγ((int)(DateTime.Now-Δ.ΔΪ).TotalSeconds)+")";
return Γ;}String Εκ(bool Κέ){String Γ="";if(Κέ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void Θά(IMyTerminalBlock т,out П Њ,out String ǯ){bool Ņ=true;String Ǌ=Απ(Αΰ(т.CustomData.Split('\n'),"mode",ref Ņ)).ToUpper();String[]τ=Ǌ.Split(':');String ǉ=τ.Length>0?τ[0].Trim():"";ǯ=τ.Length>1?τ[1].Trim():"";
Њ=П.Ĕ;if(ǉ=="MAIN") Њ=П.Εώ;else if(ǉ=="MAINX") Њ=П.ΕϏ;else if(ǉ=="MENU") Њ=П.ΓΜ;else if(ǉ=="INVENTORY") Њ=П.ΓΕ;else if(ǉ=="DEBUG") Њ=П.ΒЀ;if(Њ==П.Ĕ) ǯ=Ǌ;}String ΕΣ(П Њ,String ǯ,bool ΙΩ){int Ǉ=15;if(ΙΩ){Ζί=ΑЄ;ΑЄ=0;Ζΰ=ΑЅ;ΑЅ=0;}String Γ="";if(Њ==П.Εώ){int с=0;
Γ=Ŗ(false,Ζί,ref ΙΘ,true,Ǉ);if(!int.TryParse(ǯ,out с)) return Γ;ΑЄ=Math.Max(с,ΑЄ);с--;return ΕЂ(с,Ǉ,Γ);}else if(Њ==П.ΕϏ){int с=0;Γ=Ŗ(false,Ζΰ,ref ΙΙ,false,Ǉ);if(!int.TryParse(ǯ,out с)) return Γ;ΑЅ=Math.Max(с,ΑЅ);с--;return ΕЂ(с,Ǉ,Γ);}if(Њ==П.Εύ){return Ŗ(false,0,ref ΙΚ,true,Ǉ);
}else if(Њ==П.ΓΕ||Њ==П.ΓΜ||Њ==П.ΒЀ){К Δ=Г;if(ǯ!=""){Δ=ΒЉ("",ǯ);if(Δ==null) return "Unknown ship: "+ǯ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ƈ="—————————————————————\n";String ΔЏ="";String ΓЁ="";if(Њ==П.ΓΜ){ΔЏ="Menu";ΓЁ=ş(Δ,Δ.ΒΨ);}
else if(Њ==П.ΓΕ){ΔЏ="Inventory";ΓЁ=ş(Δ,Εμ(Δ));}else if(Њ==П.ΒЀ){return ş(Δ,ΘΙ(Δ));}Γ+=Δ.ή+" - "+Δ.ΓЏ+"m | "+Βϋ(""+ΔЏ)+"\n"+Ƈ;Γ+=ΓЁ;return Γ;}return "Unknown command: "+ǯ;}string ΘΙ(К Δ){String s="";s+=Δ.Βπ+"\n";s+=Δ.ή+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΔЅ+"\n";s+=Δ.Λ+"\n";s+=Δ.Љ+"\n";s+=Δ.ΒΨ.Length+"\n";s+=Δ.Ǿ+"\n";s+=Δ.Ȅ+"\n";s+=Δ.Ї.Count()+"\n";return s;}string Εμ(К Δ){String s="";s+=Γί("All",Б(Δ.Ǿ,Δ.Ȅ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ї.Count();i++){е ύ=Δ.Ї[i];
s+=Γί(ύ.ή,ύ.Ѕ,Δ.ΔБ,30,8,10)+"\n";}return s;}О ΗБ(К Δ,bool ш,string Ж){О έ=О.None;if(Δ==null){for(int i=0;i<Ы.Count;i++){О Ё=Ы[i].ŝ(Ж,false,ш,false);if(Ё==О.Γΰ) έ=Ё;if(Ё==О.ΓΣ) return Ё;if(Ё==О.Ɵ) return Ё;}return έ;}else return Δ.ŝ(Ж,false,ш,false);}String ř(К Δ,String Ж)
{О Ё=ΗБ(Δ,false,Ж);if(Ё==О.Γΰ) return "received!";if(Ё==О.Ɵ) return "pending...";if(Ё==О.ΓΣ) return "no answer!";return "";}String Γγ(int Ɛ,String Γ,int Ɔ,ref int ą){String[]Ν=Γ.Split('\n');if(Ɔ>=ą+Ɛ-1) ą++;ą=Math.Min(Ν.Count()-1-Ɛ,ą);if(Ɔ<ą+1) ą--;ą=Math.Max(0,ą);
String έ="";for(int i=0;i<Ɛ;i++){int ΖЋ=i+ą;if(ΖЋ>=Ν.Count()) break;έ+=Ν[ΖЋ]+"\n";}return έ;}String ΕЂ(int с,int ǌ,String Γ){String[]Ν=Γ.Split('\n');int ΔЄ=с*ǌ;int Ȯ=(с+1)*ǌ;String έ="";for(int i=ΔЄ;i<Ȯ;i++){if(i>=Ν.Count()) break;έ+=Ν[i]+"\n";}return έ;
}String Βϋ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΖΓ(String s,int ΗΑ){if(s=="") return s;if(s.Length>ΗΑ) s=s.Substring(0,ΗΑ-1)+".";return s;}string Γί(String ŀ,float value,float ŷ,int ΖΠ,int ΖІ,int Ιϋ){float ŉ=Б(value,ŷ)*ΖΠ;
String ΑЁ="[";for(int i=0;i<ΖΠ;i++){if(i<=ŉ) ΑЁ+="|";else ΑЁ+="'";}ΑЁ+="]";return ΑЁ+" "+ΖΓ(Βϋ(ŀ),ΖІ).PadRight(ΖІ)+"".PadRight(Ιϋ)+Εγ(value);}String Εγ(float Ѕ){if(Ѕ>=1000000) return Math.Round(Ѕ/1000000f,Ѕ/1000000f<100?1:0)+"M";if(Ѕ>=1000) return Math.Round(Ѕ/1000f,Ѕ/1000f<100?1:0)+"K";
return ""+Math.Round(Ѕ);}String Αγ(int ΑΔ){if(ΑΔ>=60*60) return Math.Round(ΑΔ/(60f*60f),1)+" h";if(ΑΔ>=60) return Math.Round(ΑΔ/60f,1)+" min";return ""+ΑΔ+" s";}string Ř(float v){return Math.Round(v,2)+" ";}string Κδ(Vector3 v){return "X"+Ř(v.X)+"Y"+Ř(v.Y)+"Z"+Ř(v.Z);}
List<IMyCameraBlock>ș=new List<IMyCameraBlock>();List<long>Ż=new List<long>();bool Ž=false;int ΓІ=0;int ΓЅ=0;DateTime ΖΣ=DateTime.Now;String ΓЀ="";void Θι(){ș.Clear();Ż.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ș,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ș.Count;i++){ș[i].EnableRaycast=true;if(!Ż.Contains(ș[i].CubeGrid.EntityId)) Ż.Add(ș[i].CubeGrid.EntityId);}if(!Ż.Contains(Me.CubeGrid.EntityId)) Ż.Add(Me.CubeGrid.EntityId);}bool Εψ(Β s){return s==Β.đ||s==Β.н||s==Β.Е||
s==Β.Đ||s==Β.ŗ||s==Β.Ɩ||s==Β.Αα||
(s==Β.ǵ&&Ǖ!=null);}void Αά(){Ž=false;ΓІ=0;ΓЀ="";}void ΘЁ(){if(!obstacleBrake||ș.Count==0||!Εψ(Ω)){Αά();return;}Vector3 ƺ=Ђ+Ĥ;Vector3 Ȋ=ƺ-ε;float Ȉ=Ȋ.Length();if(Ȉ<1f){Αά();return;}Vector3 ρ=Ȋ/Ȉ;float ΖЏ=Ć*obstacleLookAhead+ο;Vector3 ΙЂ=ƺ+ρ*(ο*0.25f);
for(int n=0;n<ș.Count;n++){ΓЅ=(ΓЅ+1)%ș.Count;IMyCameraBlock Ǻ=ș[ΓЅ];if(!Ǻ.IsFunctional||!Ǻ.IsWorking) continue;Vector3 ΖΥ=Ǻ.GetPosition();Vector3 Ȯ=ΙЂ;if(Vector3.Distance(ΖΥ,Ȯ)>ΖЏ) Ȯ=ΖΥ+ρ*ΖЏ;if(!Ǻ.CanScan(Ȯ)) continue;MyDetectedEntityInfo Ȟ=Ǻ.Raycast(Ȯ);bool Ιψ=!Ȟ.IsEmpty()&&!Ż.Contains(Ȟ.EntityId)&&!ΘΧ(Ȟ.EntityId)&&
Ȟ.Type!=MyDetectedEntityType.FloatingObject;if(Ιψ){if(!Ž) ΖΣ=DateTime.Now;Ž=true;ΓІ=0;ΓЀ=Ȟ.Type.ToString();}else if(Ž&&++ΓІ>=3) Αά();break;}if(Ž&&(DateTime.Now-ΖΣ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΓЀ;}const String ΖΒ="[PAMTFC]";const float Θτ=5f;const float ΒЈ=2f;class ē{public long δ;public Vector3 ξ,Ə;public Vector3 ƿ;public float Ζη;public String Ų="";public int ǀ=-1;public bool Δχ;public long ί=0;public int Ƴ=0;public DateTime ΔЌ;}IMyBroadcastListener Βλ=null;
long Ǯ=0;List<ē>э=new List<ē>();Vector3 Ĥ=new Vector3();float Ȏ=-1;Vector3 ΕΑ=new Vector3();bool Βκ=false;DateTime ΕΒ=DateTime.Now;const float ΖΑ=15f;const float Θσ=10f;int Ην=0;String й="";int а=-1;String ΓΚ(){if(α==Π.ч||Κ==Ι.ά) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+ν+"|"+И+"|"+(int) Ŀ+"|"+Math.Round(ȍ,1)+"|"+Math.Round(Ȍ,1);}void ΘЅ(){if(!trafficEnabled) return;if(Βλ==null) Βλ=IGC.RegisterBroadcastListener(ΖΒ);Ǯ=Me.CubeGrid.EntityId;int Ư=30;while(Βλ.HasPendingMessage&&Ư-->0){String Τ=Βλ.AcceptMessage().Data as String;if(Τ!=null) Θή(Τ);
}for(int i=э.Count-1;i>=0;i--)
if((DateTime.Now-э[i].ΔЌ).TotalSeconds>Θτ) э.RemoveAt(i);if(++Ην<3) return;if(Ω==Β.о&&Ъ(MyShipConnectorStatus.Connected)==null) return;Ην=0;IGC.SendBroadcastMessage(ΖΒ,Ǯ+";"+σ(ε)+";"+
σ(Ψ.GetShipVelocities().LinearVelocity)+";"+ο+";"+ΓΚ()+";"+
(α==Π.Д?а:-1)+";"+й+";"+ΗЁ()+","+Εΰ()+";"+σ(ΕΑ)+
";"+(ũ?1:0));}void Θή(String Τ){try{String[]f=Τ.Split(';');if(f.Length<7) return;long δ=long.Parse(f[0]);if(δ==Ǯ) return;ē Δ=null;for(int i=0;i<э.Count;i++) if(э[i].δ==δ) Δ=э[i];if(Δ==null){Δ=new ē();Δ.δ=δ;э.Add(Δ);}Δ.ξ=ς(f[1]);Δ.Ə=ς(f[2]);Δ.Ζη=float.Parse(f[3]);
Δ.Ų=f[4];Δ.ǀ=int.Parse(f[5]);Δ.ΔЌ=DateTime.Now;if(Δ.Ų!=""&&Δ.Ų==ΓΚ()) ΘΪ(f[6]);Δ.ί=0;Δ.Ƴ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.ί=long.Parse(d[0]);Δ.Ƴ=int.Parse(d[1]);}Δ.ƿ=f.Length>=9?ς(f[8]):new Vector3();Δ.Δχ=f.Length>=10&&f[9]=="1";}catch{}}bool ΘΧ(long δ)
{for(int i=0;i<э.Count;i++) if(э[i].δ==δ) return true;return false;}Vector3 Ελ(int Α,int Ιύ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();Ϊ Ϋ=Φ[Α];Vector3 ΚΥ=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*Ιύ;Vector3 Ќ=Ϋ.φ.LengthSquared()>0.01f?-Vector3.Normalize(Ϋ.φ):-Ϋ.β;
Vector3 л=Vector3.Cross(ΚΥ,Ќ);if(л.LengthSquared()<0.0001f) return new Vector3();float І=laneOffset*ο;float ΙΫ=Math.Min(Vector3.Distance(Ϋ.Ε,Φ[0].Ε),
Vector3.Distance(Ϋ.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(л)*І*Math.Min(1,ΙΫ/(І*4));}Vector3 Ημ=new Vector3();bool ΘΦ(Β s){return s==Β.ǵ||s==Β.ħ||s==Β.ī||s==Β.Ơ||
s==Β.Ǣ||s==Β.Ȫ||s==Β.Č;}void ΘІ(){Ĥ=new Vector3();Ȏ=-1;ΕΑ=new Vector3();bool ƻ=Εψ(Ω);if(!trafficEnabled||(!ƻ&&!ΘΦ(Ω))){Βκ=false;return;}Ημ=Ψ.GetShipVelocities().LinearVelocity;ΗЃ(Ψ.GetNaturalGravity(),!ƻ);}void ΗЃ(Vector3 φ,bool ΗΟ=false){Ĥ=new Vector3();Ȏ=-1;Vector3 Ȋ=Ђ-ε;
float Ȉ=Ȋ.Length();bool ΔΠ=Ȉ>=1f;Vector3 ρ=ΔΠ?Ȋ/Ȉ:new Vector3();ΕΑ=ρ;Vector3 Ќ=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-Ƶ;Vector3 л=Vector3.Cross(ρ,Ќ);л=л.LengthSquared()<0.0001f?-ǆ:Vector3.Normalize(л);ē ΒΝ=null;for(int i=0;i<э.Count;i++){ē ι=э[i];
if(ι.Ƴ==3&&ι.Ə.LengthSquared()<0.25f) continue;Vector3 ξ=ι.ξ+ι.Ə*(float)(DateTime.Now-ι.ΔЌ).TotalSeconds;Vector3 Ɓ=ξ-ε;float ш=(ο+ι.Ζη)*0.5f;float ƨ=ΔΠ?Vector3.Dot(Ɓ,ρ):0;if(ƨ>0&&(Ɓ-ρ*ƨ).Length()<=ш*1.2f){float Βά=Vector3.Dot(ι.Ə,ρ);bool Ζϊ=ι.ƿ.LengthSquared()>0.25f;
bool ΔΡ=Βά<-1f||(Ζϊ&&Vector3.Dot(ι.ƿ,ρ)<-0.5f);if(ΗΟ){if(ΔΡ&&Ǯ<ι.δ) continue;}else{if(ι.Δχ&&!ũ){Ĥ=л*ш*1.3f;continue;}if(ΔΡ){if(ƨ<(Ć-Βά)*6f+ш*4f) Ĥ=л*ш*1.3f;continue;}if(Ζϊ&&Ǯ<ι.δ){Vector3 ΖΞ=-Ɓ;float ΗΝ=Vector3.Dot(ΖΞ,ι.ƿ);if(ΗΝ>0&&(ΖΞ-ι.ƿ*ΗΝ).Length()<=ш*1.2f)
{Ĥ=л*ш*1.3f;continue;}}}float Ζφ=ƨ-ш-followGap*ο;float Δί=Math.Max(0,Βά)+Math.Max(0,Ζφ)/ΒЈ;if(ΔΡ) Δί=Math.Max(0,Ζφ+Math.Min(0,Βά)*ΒЈ*2)/ΒЈ;if(Ȏ<0||Δί<Ȏ){Ȏ=Δί;ΒΝ=ι;}continue;}if(ι.Ə.LengthSquared()<1f) continue;if(ΔΠ&&(Ɓ-ρ*ƨ).Length()<=ш*1.2f) continue;
Vector3 Δό=ι.Ə-Ημ;float ΗΤ=Δό.LengthSquared();if(ΗΤ<0.25f) continue;float t=-Vector3.Dot(Ɓ,Δό)/ΗΤ;if(t<=0||t>5f||(Ɓ+Δό*t).Length()>=ш*1.1f) continue;if(Ǯ>ι.δ||t<2f){Ȏ=0;ΒΝ=ι;}}bool Ια=Ȏ>=0&&Ȏ<0.5f;if(!Ια||ΗΟ||(ΒΝ!=null&&ΒΝ.Δχ&&ũ)){Βκ=false;return;}
if(!Βκ){Βκ=true;ΕΒ=DateTime.Now;}double Ζϋ=(DateTime.Now-ΕΒ).TotalSeconds;if(Ζϋ>ΖΑ+Θσ) ΕΒ=DateTime.Now;else if(Ζϋ>ΖΑ){Ȏ=-1;Ĥ=л*ο*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>Ȃ=new List<int>();String Ζύ="";void ΒЇ(){String ų=ν+"|"+И+"|"+(int) Ŀ;
if(ų==Ζύ) return;Ζύ=ų;Ȃ.Clear();int n=ν*И;if(Ŀ==Ǐ.Ƥ){for(int k=0;k<n;k++){int Ƃ=k/ν;Ȃ.Add(Ƃ*ν+(Ƃ%2==0?k-Ƃ*ν:ν-1-(k-Ƃ*ν)));}return;}int ƽ=(int) Math.Ceiling(ν/2f),Ƽ=(int) Math.Ceiling(И/2f);int ΔΝ=ν/2,ΔΜ=И/2;int x=0,y=0,ΔΑ=0,ΔΒ=-1,ŷ=Math.Max(ν,И);
for(int i=0;i<ŷ*ŷ;i++){if(-ƽ<x&&x<=ΔΝ&&-Ƽ<y&&y<=ΔΜ) Ȃ.Add((y-1+Ƽ)*ν+(x-1+ƽ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔΑ;ΔΑ=-ΔΒ;ΔΒ=t;}x+=ΔΑ;y+=ΔΒ;}}bool ΒІ(){int n=Math.Max(0,ν*И);if(й.Length==n) return false;й=new String('0',n);
а=-1;return true;}void ΘΪ(String ι){ΒІ();if(ι.Length!=й.Length) return;char[]h=й.ToCharArray();for(int i=0;i<h.Length;i++) if(ι[i]=='1') h[i]='1';й=new String(h);}bool Εω(int ǻ){return ǻ>=0&&ǻ<й.Length&&й[ǻ]=='1';}void Γη(int ǻ,bool Ǒ){if(ǻ<0||ǻ>=й.Length) return;
char[]h=й.ToCharArray();h[ǻ]=Ǒ?'1':'0';й=new String(h);}bool ΓΒ(int ǻ,bool Ιι){if(!trafficEnabled) return false;String ų=ΓΚ();for(int i=0;i<э.Count;i++){ē t=э[i];if(t.Ų!=ų||t.ǀ<0||(Ιι&&t.δ>Ǯ)) continue;float ΙΝ=Math.Abs(ǻ%ν-t.ǀ%ν)*ȍ;float ΙΧ=Math.Abs(ǻ/ν-t.ǀ/ν)*Ȍ;
if(t.ǀ==ǻ||(ΙΝ<Δώ+0.5f&&ΙΧ<Δύ+0.5f)) return true;}return false;}int ΗЀ(int Α){int n=ν*И;if(n<1) return Α;ΒЇ();if(ΒІ()) for(int k=0;k<Α&&k<n;k++) Γη(Ȃ[k],true);if(а>=0&&(Α>=n||Ȃ[Α]!=а)){Γη(а,true);а=-1;}if(а>=0&&!Εω(а)&&!ΓΒ(а,true)) return Α;
а=-1;for(int k=0;k<n;k++){int c=Ȃ[k];if(Εω(c)||ΓΒ(c,false)) continue;а=c;return k;}return n;}ǡ Θξ(int Α){ΒЇ();if(Α<0||Α>=Ȃ.Count) return ǡ.ΑΧ;ώ=Ȃ[Α]%ν;ό=Ȃ[Α]/ν;return ǡ.Αδ;}class Ǎ{public long δ;public Vector3 ξ,Ч,Ќ;}List<Ǎ>С=new List<Ǎ>();bool ę=false;Vector3 ΑЏ,ΑЎ,ΑА,ΑБ;
long і=0;long ȡ=0;Ϊ Ǖ=null;bool ƴ=false;Ϊ ΒЃ(Vector3 ξ,Vector3 Ч,Vector3 Ќ){MatrixD m=MatrixD.CreateWorld(ξ,Ч,Ќ);Ϊ w=new Ϊ(Ξ);w.γ=Vector3D.TransformNormal(ΑЏ,m);w.β=Vector3D.TransformNormal(ΑЎ,m);w.Ѐ=Vector3D.TransformNormal(ΑА,m);w.Ε=ξ+(Vector3) Vector3D.TransformNormal(ΑБ,m);
w.ζ=true;return w;}int Εΰ(){Β s=Ω;if(s==Β.о) return Ъ(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Β.ī||s==Β.Ȗ||s==Β.Ī||s==Β.Αϋ||
s==Β.Αί||s==Β.Ǣ) return 3;if(s==Β.ǵ||s==Β.ħ||s==Β.Ơ) return 2;if(s==Β.Е) return 1;return 0;}long ΗЁ(){if(Εΰ()==0) return 0;if(Ē(1)&&Ɖ!=0) return Ɖ;return Ǖ!=null?ȡ:і;}void Εό(){if(Κ==Ι.ά||!Ξ.ζ) return;IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connected);if(Л==null||Л.OtherConnector==null) return;
IMyShipConnector ι=Л.OtherConnector;Vector3I Ιγ=new Vector3I((int) Ξ.и.X,(int) Ξ.и.Y,(int) Ξ.и.Z);if(Л.Position==Ιγ){MatrixD ů=MatrixD.Transpose(ι.WorldMatrix);ΑЏ=Vector3D.TransformNormal(Ψ.WorldMatrix.Forward,ů);ΑЎ=Vector3D.TransformNormal(Ψ.WorldMatrix.Down,ů);ΑА=Vector3D.TransformNormal(Ψ.WorldMatrix.Left,ů);
ΑБ=Vector3D.TransformNormal(Л.GetPosition()-ι.GetPosition(),ů);ę=true;}if(Vector3.Distance(Л.GetPosition(),Ξ.Ε)<5) і=ι.EntityId;List<IMyShipConnector>Ļ=new List<IMyShipConnector>();χ.GetBlocksOfType(Ļ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ļ.Count>0){С.Clear();for(int i=0;i<Ļ.Count;i++){Ǎ p=new Ǎ();p.δ=Ļ[i].EntityId;p.ξ=Ļ[i].GetPosition();p.Ч=Ļ[i].WorldMatrix.Forward;p.Ќ=Ļ[i].WorldMatrix.Up;С.Add(p);}}Ǖ=null;ȡ=0;if(ι.EntityId==і||!ę) return;for(int i=0;i<С.Count;i++)
if(С[i].δ==ι.EntityId){ȡ=С[i].δ;Ǖ=ΒЃ(С[i].ξ,С[i].Ч,С[i].Ќ);}}bool ΓΘ(long δ,int ΗΗ){if(δ==0) return false;for(int i=0;i<э.Count;i++){ē t=э[i];if(t.ί==δ&&(t.Ƴ>ΗΗ||(t.Ƴ==ΗΗ&&t.δ<Ǯ))) return true;}return false;}bool Βώ(int Δτ){if(Κ==Ι.ά||!trafficEnabled){Ǖ=null;ȡ=0;return true;}
long μ=Ǖ!=null?ȡ:і;if(!ΓΘ(μ,Δτ)) return true;if(!ΓΘ(і,Δτ)){Ǖ=null;ȡ=0;return true;}if(!ę) return false;Ǎ ǣ=null;float ƭ=0;for(int i=0;i<С.Count;i++){Ǎ p=С[i];if(p.δ==і||ΓΘ(p.δ,Δτ)) continue;float d=Vector3.Distance(p.ξ,Ξ.Ε);if(ǣ==null||d<ƭ){ǣ=p;ƭ=d;}
}if(ǣ==null) return false;ȡ=ǣ.δ;Ǖ=ΒЃ(ǣ.ξ,ǣ.Ч,ǣ.Ќ);return true;}String ΗЋ(){String s=(ę?"1":"0")+":"+σ(ΑЏ)+":"+σ(ΑЎ)+":"+
σ(ΑА)+":"+σ(ΑБ)+":"+і;for(int i=0;i<С.Count;i++)
s+=":"+С[i].δ+","+σ(С[i].ξ)+","+σ(С[i].Ч)+","+σ(С[i].Ќ);return s;}void ΗЊ(String s){С.Clear();ę=false;і=0;String[]f=s.Split(':');if(f.Length<6) return;ę=f[0]=="1";ΑЏ=ς(f[1]);ΑЎ=ς(f[2]);ΑА=ς(f[3]);ΑБ=ς(f[4]);long.TryParse(f[5],out і);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǎ p=new Ǎ();if(!long.TryParse(e[0],out p.δ)) continue;p.ξ=ς(e[1]);p.Ч=ς(e[2]);p.Ќ=ς(e[3]);С.Add(p);}}const String ƕ="[PAMDCK]";const float Ηω=120f;class ȧ{public long Δ;public int Ǔ;public long ѕ;public bool ΖΦ;public DateTime ĺ,ΖЇ;
}IMyBroadcastListener ΑЍ=null;List<ȧ>Т=new List<ȧ>();List<IMyShipConnector>ć=new List<IMyShipConnector>();List<IMySensorBlock>ȥ=new List<IMySensorBlock>();List<IMyCameraBlock>ǅ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Βρ=new List<MyDetectedEntityInfo>();long Ǽ=0;int ΓЇ=0;
long ΖΩ=0;DateTime ΖΪ=DateTime.Now;String ĸ="";int Ζμ=0;IMyCubeGrid Ĉ=null;void Θλ(){Ĉ=Me.CubeGrid;χ.GetBlocksOfType(ć,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ȥ,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ǅ,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ȥ.Count;i++){ȥ[i].DetectSmallShips=true;ȥ[i].DetectLargeShips=true;}for(int i=0;i<ǅ.Count;i++) ǅ[i].EnableRaycast=true;}IMyShipConnector ΕЍ(long δ){if(δ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(δ) as IMyShipConnector;return c!=null&&Ĉ!=null&&c.CubeGrid.IsSameConstructAs(Ĉ)?c:null;}bool ΓΗ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Εϊ(IMyShipConnector c,long Δ){for(int i=0;i<ǅ.Count;i++)
{IMyCameraBlock Ǻ=ǅ[i];if(!Ǻ.IsWorking||Vector3D.Distance(Ǻ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ȯ=Ǻ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ǻ.CanScan(Ȯ)) continue;MyDetectedEntityInfo Ȟ=Ǻ.Raycast(Ȯ);if(!Ȟ.IsEmpty()&&Ȟ.EntityId!=Ĉ.EntityId&&Ȟ.EntityId!=Δ&&Ȟ.Type!=MyDetectedEntityType.FloatingObject)
{ĸ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΘΨ(){for(int s=0;s<ȥ.Count;s++){if(!ȥ[s].IsWorking) continue;Βρ.Clear();ȥ[s].DetectedEntities(Βρ);for(int i=0;i<Βρ.Count;i++){MyDetectedEntityInfo e=Βρ[i];bool Ιέ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Ιέ&&e.EntityId!=Ǽ&&e.Velocity.Length()>0.5f){ĸ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Θί(ȧ r){IMyShipConnector ѕ=ΕЍ(r.ѕ);if(ΓΗ(ѕ)&&Εϊ(ѕ,r.Δ)) return ѕ.EntityId;if(!r.ΖΦ||ѕ==null) return 0;IMyShipConnector ǣ=null;double ƭ=0;for(int i=0;i<ć.Count;i++)
{IMyShipConnector c=ć[i];if(!ΓΗ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ѕ.GetPosition());if(ǣ==null||d<ƭ){if(Εϊ(c,r.Δ)){ǣ=c;ƭ=d;}}}return ǣ==null?0:ǣ.EntityId;}void ΘΛ(){ȧ ž=null;long Л=0;for(int Ň=2;Ň>=1&&ž==null;Ň--){ȧ ņ=null;for(int i=0;i<Т.Count;i++)
{ȧ r=Т[i];if(r.Ǔ!=Ň) continue;if(Ň==1){long c=Θί(r);if(c==0) continue;if(ņ==null||r.ĺ<ņ.ĺ){ņ=r;Л=c;}}else if(ņ==null||r.ĺ<ņ.ĺ) ņ=r;}ž=ņ;}if(ž==null){if(Т.Count>0&&ĸ=="") ĸ="No free dock";return;}Ǽ=ž.Δ;ΓЇ=ž.Ǔ;ΖΩ=ž.Ǔ==1?Л:0;ΖΪ=DateTime.Now;
}void Θό(){if(Ĉ==null) return;if(ΑЍ==null) ΑЍ=IGC.RegisterBroadcastListener(ƕ);int Ư=30;while(ΑЍ.HasPendingMessage&&Ư-->0){String Τ=ΑЍ.AcceptMessage().Data as String;if(Τ!=null) ΘΜ(Τ);}Θδ();if(++Ζμ<3) return;Ζμ=0;IGC.SendBroadcastMessage(ƕ,Ηψ());}void ΘΜ(String Τ){try{String[]f=Τ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Т.Count-1;i>=0;i--) if(Т[i].Δ==Δ) Т.RemoveAt(i);if(Ǽ==Δ) Ǽ=0;}else if(f[0]=="R"&&f.Length>=5){long ѕ=long.Parse(f[3]);if(ΕЍ(ѕ)==null) return;ȧ r=null;for(int i=0;i<Т.Count;i++) if(Т[i].Δ==Δ) r=Т[i];if(r==null){r=new ȧ();r.Δ=Δ;r.ĺ=DateTime.Now;Т.Add(r);}
int Ǔ=int.Parse(f[2]);if(r.Ǔ!=Ǔ) r.ĺ=DateTime.Now;r.Ǔ=Ǔ;r.ѕ=ѕ;r.ΖΦ=f[4]=="1";r.ΖЇ=DateTime.Now;}}catch{}}void Θδ(){for(int i=Т.Count-1;i>=0;i--)
if((DateTime.Now-Т[i].ΖЇ).TotalSeconds>3) Т.RemoveAt(i);ȧ ΑЃ=null;for(int i=0;i<Т.Count;i++) if(Т[i].Δ==Ǽ) ΑЃ=Т[i];if(Ǽ!=0&&ΑЃ==null) Ǽ=0;if(ΑЃ!=null&&(DateTime.Now-ΖΪ).TotalSeconds>Ηω){Ǽ=0;ΑЃ.ĺ=DateTime.Now;}ĸ="";if(Ǽ==0&&Т.Count>0&&!ΘΨ()) ΘΛ();
}String Ηψ(){String η="S;"+Ĉ.EntityId+";"+Ǽ+";"+ΓЇ+";"+ΖΩ+";";for(int i=0;i<Т.Count;i++) η+=(i>0?",":"")+Т[i].Δ;η+=";";for(int i=0;i<ć.Count;i++){IMyShipConnector c=ć[i];η+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}η+=";";for(int i=0;i<ȥ.Count;i++){IMySensorBlock z=ȥ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();η+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return η;}String ΘΒ()
{if(ć.Count==0&&ȥ.Count==0) return "";int ƻ=0;for(int i=0;i<ć.Count;i++) if(ΓΗ(ć[i])) ƻ++;return "Docks: "+ƻ+"/"+ć.Count+" free"+(Ǽ!=0?(ΓЇ==1?" | docking":" | undocking"):"")+
(Т.Count>1?" | "+(Т.Count-1)+" waiting":"")+(ĸ!=""?"\n"+ĸ:"")+"\n";}IMyBroadcastListener Βζ=null;int ȝ=0;DateTime Ζλ=DateTime.Now;DateTime ΗΫ=DateTime.MinValue;long Ηά=0;int Ηέ=0;long Ɖ=0;bool ΑΣ=false;int ΓБ=0;List<Vector3>ΑΥ=new List<Vector3>();List<float>ΕΟ=new List<float>();
bool ũ=false;bool ΑЌ=false;Vector3 Ζκ=new Vector3();bool ƣ(){return ȝ!=0&&(DateTime.Now-ΗΫ).TotalSeconds<3;}bool Ē(int Ǔ){return ƣ()&&Ηά==Ǯ&&Ηέ==Ǔ;}bool ΕЎ(){return ƣ()||(DateTime.Now-Ζλ).TotalSeconds<3;}void Γβ(int Ǔ){if(ȝ==Ǔ) return;ȝ=Ǔ;Ζλ=DateTime.Now;ΓБ=3;
}void Γα(){if(ȝ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƕ,"D;"+Ǯ);ȝ=0;ΑΣ=false;}void Θϋ(){if(!trafficEnabled||Κ==Ι.ά) return;if(Βζ==null) Βζ=IGC.RegisterBroadcastListener(ƕ);int Ư=20;while(Βζ.HasPendingMessage&&Ư-->0){String Τ=Βζ.AcceptMessage().Data as String;if(Τ==null||!Τ.StartsWith("S;")) continue;
try{Θέ(Τ);}catch{}}if(Ω!=Β.Е){ũ=false;ΑЌ=false;}if(ȝ==1&&Ъ(MyShipConnectorStatus.Connected)!=null) Γα();if(ȝ==2&&!ΑΣ&&Ω!=Β.Ǣ&&
Vector3.Distance(ε,ц().Ε)>ο*3) Γα();if(ȝ!=0&&++ΓБ>=3){ΓБ=0;IGC.SendBroadcastMessage(ƕ,"R;"+Ǯ+";"+ȝ+";"+і+";"+(ę?1:0));}}void Θέ(String Τ){String[]f=Τ.Split(';');if(f.Length<7) return;bool Ζυ=false;String[]ΖΕ=f[5].Split(',');for(int i=0;i<ΖΕ.Length;i++) if(ΖΕ[i]==Ǯ.ToString()) Ζυ=true;
if(!Ζυ) return;ΗΫ=DateTime.Now;Ηά=long.Parse(f[2]);Ηέ=int.Parse(f[3]);Ɖ=long.Parse(f[4]);ΑΥ.Clear();ΕΟ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑΥ.Add(ς(e[0]));ΕΟ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ζξ=f[6].Split(':');List<Ǎ>Ј=new List<Ǎ>();for(int i=0;i<Ζξ.Length;i++){String[]e=Ζξ[i].Split(',');if(e.Length<4) continue;Ǎ p=new Ǎ();p.δ=long.Parse(e[0]);p.ξ=ς(e[1]);p.Ч=ς(e[2]);p.Ќ=ς(e[3]);Ј.Add(p);}С=Ј;}bool Ηώ(){if(!trafficEnabled||Κ==Ι.ά||і==0) return true;
Γβ(1);if(!ΕЎ()) return true;if(!Ē(1)){Λ="Waiting for dock clearance";return false;}Βυ();return true;}bool Βυ(){ƴ=false;Ǎ ň=null;if(Ɖ!=0&&Ɖ!=і&&ę)
for(int i=0;i<С.Count;i++) if(С[i].δ==Ɖ) ň=С[i];long ΕΗ=ň==null?0:ň.δ;if(ΕΗ==ȡ&&(ΕΗ==0)==(Ǖ==null)) return false;ȡ=ΕΗ;Ǖ=ň==null?null:ΒЃ(ň.ξ,ň.Ч,ň.Ќ);return true;}bool ś(Vector3 p){for(int i=0;i<ΑΥ.Count;i++) if(Vector3.Distance(p,ΑΥ[i])<ΕΟ[i]+ο*0.75f) return true;
return false;}bool ΘΞ(Vector3 Ʊ,Vector3 ń){ũ=false;if(!trafficEnabled||Κ==Ι.ά||і==0) return false;if(ȝ!=1&&Vector3.Distance(ε,Ξ.Ε)>dockRequestDistance) return false;Γβ(1);if(!ƣ()||Ē(1)||ΑΥ.Count==0) return false;if(!ś(Ʊ)&&!ś(ń)&&!ś(ε)) return false;ũ=true;return true;
}bool ΕΪ(){if(!trafficEnabled||Κ==Ι.ά||і==0) return true;Γβ(2);if(!ΕЎ()) return true;if(!Ē(2)){Λ="Waiting for undock clearance";return false;}return true;}