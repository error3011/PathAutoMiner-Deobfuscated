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
public enum Κ{Ĩ,ƹ,Ǫ,ј,ή}public enum Α{ь,Ć,Ŵ,Ƞ,Ņ,И,у,
Ħ,ů,Ȁ,Ł,Ľ,Ģ,
ƻ,Ȥ,ǭ,ŀ,ΒΙ,Αϋ,
ΔΖ,ΒΘ,Ʊ,Αύ,ĩ}public enum Π{э,ŵ,В,ΓΪ,Αχ,ΑЈ,ΑЉ,Ū,ū}public enum ї{None,ŀ,ь,ΕЇ}public enum Σ{Main,Ζγ,Ƹ,Ʒ,ΓБ,Αυ,
ΓΘ,ΖΜ,ΖΝ,ΖΘ,ƽ,Ζθ,
ΒΑ,ΓЈ,ΓЌ,Ζκ,ΒΓ}public enum х{ΑБ,Ư,Ιο,ΖΠ,ή}public enum ю{ƿ,ΕЀ}public enum ъ{ΑА,Γΰ,Ζν,Ƴ}public enum Г{Γψ,ΕЄ,ΓΥ,ΓΫ,Αω,Γκ,ΑЃ}public enum Ǖ{ΓΧ,ΓΝ,ΓΞ}public enum Щ{Ų,ΙΝ,ľ}public enum ρ{Γϊ,Γώ,ΓϏ,Ζή,Ζέ,
Γω,ΑЍ,ΑЌ,Γό,
Γύ,Γϋ}public enum ε{ț,ĉ,Ț,Ατ,ΓΛ,ΓΜ}public enum Ǭ{ΑЀ,Ασ,ΘЅ}public enum љ{ΒΔ,ΕЉ,ΙΨ,Γχ}public enum Љ{None,Ŷ,ƺ,ΑЋ}public enum ϋ{ǁ,Ας,ΑІ,Ƴ,Γυ,ΑЇ,Γφ,Ιφ,ΓЀ}public enum С{Ĩ,ΖΪ,ΖΩ,Γς,Γμ,ΓΦ,ΖΫ}class ά{public bool λ=false;public Vector3 Ε=new Vector3();
public Vector3 θ=new Vector3();public Vector3 ι=new Vector3();public Vector3 Ѓ=new Vector3();public Vector3 φ=new Vector3();public Vector3 о=new Vector3();public float Ч=0;public float Λκ=0;public float[]Ǿ=null;public ά(){}public ά(ά ζ){λ=ζ.λ;Ε=ζ.Ε;θ=ζ.θ;ι=ζ.ι;Ѓ=ζ.Ѓ;φ=ζ.φ;
о=ζ.о;Ǿ=ζ.Ǿ;}public ά(Vector3 Ε,Vector3 ι,Vector3 θ,Vector3 Ѓ,Vector3 φ){this.Ε=Ε;this.θ=θ;this.ι=ι;this.Ѓ=Ѓ;this.Ч=0;this.φ=φ;}public void Θπ(List<IMyThrust>ΒΜ,List<string>Ā){Ǿ=new float[Ā.Count];for(int i=0;i<Ǿ.Length;i++)
Ǿ[i]=-1;for(int i=0;i<ΒΜ.Count;i++){string Ǔ=ΖΖ(ΒΜ[i]);int Β=Ā.IndexOf(Ǔ);if(Β!=-1)
Ǿ[Β]=Й(ΒΜ[i].MaxEffectiveThrust,ΒΜ[i].MaxThrust);}}}class ΓЁ{public ά η=null;public List<Vector3>ƣ=new List<Vector3>();public float ǌ=0;public float Εξ=0;public float Ž=0;public float Δπ=0;public Vector3 Βγ=new Vector3();}class Ź{public ρ Р=ρ.Γϊ;public float Ǳ=0;public float İ=0;
public string ǲ="";public string Ƕ="";DateTime Ηξ;public bool ΕΜ=false;private bool Δβ=false;public bool ΓΡ(bool ǻ){if(ǻ){İ=0;Δβ=false;return false;}Δβ=true;return İ>Ǳ;}public void Ιΰ(){ΓΡ(true);ΕΜ=false;}public void ΙІ(){if(Δβ)
if((DateTime.Now-Ηξ).TotalSeconds>1){İ++;Ηξ=DateTime.Now;}}public bool Γη(){switch(Р){case ρ.Γώ:return true;case ρ.ΓϏ:return true;}return false;}}class м{public String ΰ="";public String υ="";public int Њ=0;public Щ ƌ=Щ.ľ;public м(String ΰ,String υ,int Њ,Щ ƌ){this.ΰ=ΰ;this.υ=υ;
this.Њ=Њ;this.ƌ=ƌ;}}class ΘЀ{public ΘЀ(Vector3 κ,float ϊ){this.κ=κ;this.ϊ=ϊ;}public Vector3 κ;public float ϊ;}class Ѕ{public DateTime ΔЃ;public DateTime ΖЌ;public String Φ="";public String ΰ="";public String ΓΕ="";public String Λ="";public String Εέ="";public String Βϊ="";public String ΕΛ="";
public Vector3 Ε=new Vector3();public List<м>Ћ=new List<м>();public Κ Μ=Κ.Ĩ;public Љ ī;public float Ȑ;public float ȉ;public int Ў;public int Δθ=0;public int Ει=0;public bool ΖΥ(){return(DateTime.Now-ΔЃ).TotalSeconds>10;}public bool Γν(){if(ī!=Љ.Ŷ) return false;return(DateTime.Now-ΖЌ).TotalSeconds>=4;
}public Ѕ(String Φ){this.Φ=Φ;}public Љ ń(String І,bool ΚЏ,bool Ъ,bool ΚΕ){if(І==""&&!Ъ) return Љ.None;if(ī==Љ.Ŷ&&Γν())
ī=Љ.ΑЋ;if(ΚЏ){ΕΛ=І;ī=Љ.Ŷ;ΖЌ=DateTime.Now;return ī;}else if(Ъ){ΕΛ="";ī=Љ.None;}else if(ΕΛ==І){if(ΚΕ) ī=Љ.ƺ;return ī;}return Љ.None;}}public String ΘЍ(String Κθ){switch(Κθ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΒЁ=new Vector3();Vector3 δ=new Vector3();Vector3 Βζ=new Vector3();Vector3 ΑΤ=new Vector3();Vector3 ΑΓ=new Vector3();
DateTime ΔІ=new DateTime();bool ď=true;int Εγ=0;bool ĕ=false;bool ŗ=false;bool ΑΪ=true;bool Θδ=false;bool Ɗ=false;bool Βλ=false;float Ĝ=0;float Ť=0;int ĸ=0;int Αλ=0;int Αή=0;int ΔΨ=0;float Ə=0;float ž=0;double ΔБ=0;float ǆ=0;List<int>Ƈ=new List<int>();List<int>ƚ=new List<int>();
Κ Μ=Κ.Ĩ;int А=0;float ș=0;float Ș=0;float ο=0;float Ɵ=0,Αΰ=0;IMyRemoteControl Ϋ;IMySensorBlock ψ;List<IMyTimerBlock>Εε=new List<IMyTimerBlock>();List<IMyShipConnector>њ=new List<IMyShipConnector>();List<IMyThrust>ā=new List<IMyThrust>();List<IMyGyro>Βη=new List<IMyGyro>();List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȱ=new List<IMyLandingGear>();List<IMyReactor>ķ=new List<IMyReactor>();List<IMyConveyorSorter>ΒІ=new List<IMyConveyorSorter>();List<IMyGasTank>ǜ=new List<IMyGasTank>();List<IMyTerminalBlock>ǂ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ɓ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ŋ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ī=new List<IMyBatteryBlock>();List<IMyTextPanel>Ц=new List<IMyTextPanel>();List<IMyTextSurface>ȕ=new List<IMyTextSurface>();List<IMyTextPanel>ŏ=new List<IMyTextPanel>();IMyTerminalBlock Ǡ=null;ά Ο=new ά();ά Δύ=new ά();
ά ΔЀ=new ά();bool Ă=false;bool ƨ=false;bool Ʃ=false;bool Βν=false;List<String>Ā=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Απ=new List<ά>();int Δι=0;double Θε=0;ά Ζ=new ά();Vector3 ǹ;Vector3 ę;Π γ=Π.э;ю Ř=ю.ƿ;int Ќ=0;int Ю=0;double ΑΠ=0;bool ΒΩ=false;Ź Ї=new Ź();
Ź Ј=new Ź();ъ ѓ=ъ.ΑА;bool Ļ=true;ю ȓ=ю.ƿ;int ѐ=3;int я=3;int н=30;Ǖ ћ=Ǖ.ΓΧ;bool ġ=true;bool Ɖ=true;bool ƃ=true;bool ť=false;Г г=Г.Γψ;bool Ġ=true;float ȏ=90;float џ=20;float п=5;float Ǎ=20;float ȃ=0.70f;float ƭ=1.50f;float Ƭ=2.50f;float ũ=0f;float œ=0f;Α Ϊ;
Π ў;Α Αδ;ΓЁ ν=null;int Ѐ=0;int ύ=0;int б=0;int ė=0;int Ţ=30;int к=0;bool Е=true;Vector3 ǎ;double в=0;bool ΒЃ=false;int Ερ=0;int Ж=0;int ΔΝ=0;int ȑ=0;int ΗІ=0;Vector3 Αά=new Vector3();float ђ=0;float т=0;int Ũ=0;ά ΒЇ=null;float ΔЄ=0;float ΒΞ=0;float и=0;float ś=0;
bool Δώ=false;bool Ŧ=false;bool Αμ=false;bool ē=false;bool Č=false;DateTime ΚΧ=new DateTime();ά ō=null;ά ΗΓ=null;float Ȧ=0;ї Ȅ;float Ĕ=0;float ŧ=0;String ΔЌ="";float Βρ=0;float ȉ=0;float Ȑ=0;List<м>ǵ=new List<м>();float д=0;bool Αα=false;bool ΘΩ=false;bool ΘΓ=false;
bool ΒЍ=false;bool ΘΔ=false;float ΔЏ=0;float ΕΚ=0;Vector3 ΗΧ=new Vector3();Vector3 Ё=new Vector3();Vector3 ΒЎ=new Vector3(1,1,1);float Εβ=1;Vector3 Αθ=new Vector3();bool Βμ=false;bool Δφ=false;bool ΔЉ=false;bool Ζψ=false;float ΒΧ=0;float ΒΛ=2;Vector3 ΘΥ;Vector3 Εα;Vector3 Εΰ;
float[,]ǿ=new float[3,2];Dictionary<String,float[,]>ǥ=new Dictionary<string,float[,]>();Dictionary<String,float>Θβ=new Dictionary<String,float>();int ΔΡ=0;Dictionary<String,float[]>Ě=new Dictionary<string,float[]>();float ΕΝ;bool ΒΪ=false;int ΒΝ=0;int Ĵ=0;int Βι=0;int ŕ=0;
String Λ="";int[]Ηό=new int[Enum.GetValues(typeof(Σ)).Length];bool ΕΒ=false;int Ŝ=0;int Ў=0;Σ ό=Σ.Main;int ΛΜ=0,ΛΝ=0,ΛΞ=0,ΛΟ=0;int ΚЊ=0,ΚЋ=0;bool ƀ=false;bool Ζω=false;String Ơ="";String ΔΧ="";IMyBroadcastListener ǯ=null;bool ΗΦ=true;List<Ѕ>Ь=null;Ѕ Л=null;
int ΒΣ=0,ΒΤ=0;int ΖЏ=0,ΖА=0;int ΚΛ=0,ΚΜ=0,ΚΝ=0;String Κκ="";Exception Ė=null;bool ΒЂ=false;Π Ɯ=Π.э;bool ΗΖ=false;bool Ημ=true;bool ΕΣ=false;String Θγ="";bool Ακ=false;bool Εά=true;int Βκ=0;int[]ǣ=null;Vector3 ΔΩ=new Vector3();bool Ηά=false;bool ǖ(IMyTerminalBlock Ω)=>Ω.CubeGrid==Me.CubeGrid;
Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Γθ(Me,controllerTag,true))
Μ=Κ.ј;μ(Σ.Main);if(Μ!=Κ.ј){Λ="Welcome to [PAM]!";љ ΕΡ=Ια();if(Μ==Κ.Ĩ){List<IMyShipDrill>ΗΣ=new List<IMyShipDrill>();List<IMyShipGrinder>Ηέ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗΣ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(Ηέ,q=>q.CubeGrid==Me.CubeGrid);if(ΗΣ.Count>0)
{Μ=Κ.ƹ;Λ="Miner mode enabled!";}else if(Ηέ.Count>0){Μ=Κ.Ǫ;Λ="Grinder mode enabled!";}else{Μ=Κ.ή;ȡ(х.ή);}}if(ΕΡ==љ.ΒΔ) ΑΪ=false;if(ΕΡ==љ.ΕЉ) Λ="Data restore failed!";if(ΕΡ==љ.Γχ) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΛΨ){try{if(Ė!=null){if(Т.Trim().ToUpper()=="RETRY")
{Ė=null;ď=true;ĸ=0;if(Μ!=Κ.ј) Ђ();Λ="Restarted after error";}else{ΙΗ();Ζα();return;}}ŗ=(ΛΨ&UpdateType.Update10)!=0;if(ŗ) Εγ++;ĕ=Εγ>=10;if(ĕ) Εγ=0;if(ŗ){ΒΝ++;if(ΒΝ>4) ΒΝ=0;Αή=(int) Math.Max(0,10-(DateTime.Now-ΔІ).TotalSeconds);}if(Т!="") Κκ=Т;if(Μ!=Κ.ј)
Ιξ(Т);else
Θώ(Т);ΕΒ=false;try{int ΗΔ=Runtime.CurrentInstructionCount;float š=Й(ΗΔ,Runtime.MaxInstructionCount);if(š>0.90) Λ="Max. instructions >90%";if(š>Ə) Ə=š;if(ΒΪ){Ƈ.Add(ΗΔ);while(Ƈ.Count>10) Ƈ.RemoveAt(0);ž=0;for(int i=0;i<Ƈ.Count;i++) ž+=Ƈ[i];ž=Й(Й(ž,Ƈ.Count),Runtime.MaxInstructionCount);
double ΔЅ=Runtime.LastRunTimeMs;if(Βλ&&ΔЅ>ΔБ) ΔБ=ΔЅ;ƚ.Add((int)(ΔЅ*1000f));while(ƚ.Count>10) ƚ.RemoveAt(0);ǆ=0;for(int i=0;i<ƚ.Count;i++) ǆ+=ƚ[i];ǆ=Й(ǆ,ƚ.Count)/1000f;}}catch{Ə=0;}}catch(Exception e){Ė=e;}}void Ιξ(string Т){bool ΒЄ=false;String ΖЃ="";if(А<=1&&!Γπ(Т))
Ζβ(Т);int ΕΓ=50;while(ǯ!=null&&ǯ.HasPendingMessage&&ΕΓ-->0){MyIGCMessage β=ǯ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;String Ȓ="";if(А<=1&&Ζΰ(ref Θ,out Ȓ,out ΖЃ)&&Ȓ==BROADCAST_SELF_ID){Ζβ(Θ);ΒЄ=true;break;}}bool Ηζ=Βλ&&Ϊ==Α.ь&&!Ɗ&&!ΒЄ&&ĸ==0&&!Βν;
if(ĕ&&Ϊ!=Α.ь) ΒЄ=true;if((ŗ&&!Ηζ)||(ĕ&&Ηζ)){if(ĸ==0&&(Αή<=0||ď)){Θδ=А>0;А=0;ĸ=1;Ȣ();Ιε();ΖΞ();Ȝ("Scan 1");}else if(ĸ==1){ĸ=2;Ȣ();Ιζ();Ȝ("Scan 2");}else if(ĸ==2){ĸ=0;Ȣ();Ιη();Ιθ();if(А<=1) ΖΨ();Ȝ("Scan 3");ΔІ=DateTime.Now;if(А<=1&&ΑΪ)
ΒЁ=ΑЁ(Ϋ,Ϋ.CenterOfMass);ΑΪ=false;if(ď){ď=false;bool Λά=γ==Π.В;Ђ();if(Ημ&&resumeAfterRestart&&Λά&&А<=1) ΕΣ=true;}Ημ=false;if(Θδ&&А==0) Λ="Setup complete";}else{if(γ==Π.В&&Μ!=Κ.ή){Ȣ();Θι();Ȝ("Inv balance");}Ȣ();switch(Αλ){case 0:Ιϊ();break;case 1:ΙЀ();break;case 2:Ιω();break;
case 3:ΙЊ();break;case 4:ΙϏ();break;case 5:Θσ();break;case 6:Θς(Ϋ);break;}Ȝ("Update: "+Αλ);Αλ++;if(Αλ>6){Αλ=0;Βλ=true;if(Ɯ!=Π.э){switch(Ɯ){case Π.Ū:Ĥ();break;case Π.ū:Ĥ();break;case Π.В:Ĥ();break;case Π.ΑЈ:Αώ();break;case Π.ΑЉ:ł();break;}Ɯ=Π.э;}}}if(!ď){if(!Ȟ(Ϋ,true))
{Ϋ=null;ΑΪ=true;А=2;}if(А>=2&&Ϊ!=Α.ь)
Ђ();if(А<=1){Ť=Ϋ.CalculateShipMass().PhysicalMass;Ĝ=(float) Ϋ.GetShipSpeed();δ=Ιψ(Ϋ,ΒЁ);Βζ=Ϋ.WorldMatrix.Forward;ΑΤ=Ϋ.WorldMatrix.Left;ΑΓ=Ϋ.WorldMatrix.Down;ΙЄ();ΙЇ();Ιό();if(ΕΣ){ΕΣ=false;Ĥ();if(Ϊ!=Α.ь) Λ="Job resumed after restart";}if(Ϊ!=Α.ь){Ɗ=false;Ζε(false);Εώ(false);
String ΛΓ=Γγ(Ϊ)+" "+(int) Ϊ;Ȣ();ΙЂ();ΔΕ(false);Ȝ(ΛΓ);ΙЈ();ΙЃ();Ȣ();Ιώ();Ȝ("Thruster");Ȣ();Ζτ();Ȝ("Gyroscope");}else{if(Ɗ){if(ΑЅ()){л(ΑΓ,Βζ,ΑΤ,0.25f,true);Ζτ();Λ="Aligning to planet: "+Math.Round(ΒΧ-0.25f,2)+"°";if(Βμ) Γζ(true,true);}else Γζ(true,true);}}Ακ=false;}}}Ȣ();Ζα();Ȝ("Print");
if(ΒЄ||ΔΨ<=0){Ȣ();Θκ(ΖЃ);Ȝ("Broadcast");ΔΨ=4;}else if(ĕ) ΔΨ--;}void Ζβ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȥ=τ.ElementAtOrDefault(1);var ǅ=τ.ElementAtOrDefault(2);var Ζϋ=τ.ElementAtOrDefault(3);String Ȯ="Invalid argument: "+Т;
bool Εν=false;switch(І){case "UP":this.Γτ(false);break;case "DOWN":this.Γσ(false);break;case "UPLOOP":this.Γτ(true);break;case "DOWNLOOP":this.Γσ(true);break;case "APPLY":this.Εό(true);break;case "MRES":Ў=0;break;case "STOP":this.Ђ();break;case "PATHHOME":{this.Ђ();this.ΓА();}break;
case "PATH":{this.Ђ();this.ΓА();Ο.λ=true;}break;case "START":{this.Ђ();ΓЏ();}break;case "ALIGN":{Γζ(!Ɗ,false);}break;case "CONT":{this.Ђ();this.Ĥ();}break;case "JOBPOS":{this.Ђ();this.ł();}break;case "HOMEPOS":{this.Ђ();this.Αώ();}break;case "FULL":{Αα=true;}break;
case "RESET":{ΒЂ=true;А=2;}break;case "EXPORT":{ΘІ();}break;case "IMPORT":{ΙΛ(ȥ=="JOB");}break;default:Εν=true;break;}if(Μ!=Κ.ή){switch(І){case "SHUTTLE":{ΘЃ();}break;case "CFGS":{if(!Θό(ȥ,ǅ,Ζϋ)) Λ=Ȯ;}break;case "CFGB":{if(!Θϋ(ȥ,ǅ)) Λ=Ȯ;}break;case "CFGL":
{if(!Ń(ref ȏ,true,ϋ.ΑІ,ȥ,"")||!Θύ(ǅ))
Λ=Ȯ;}break;case "CFGE":{if(!Ń(ref п,true,ϋ.ΑЇ,ȥ,"IG")||
!Ń(ref џ,true,ϋ.Γυ,ǅ,"IG")||
!Ń(ref Ǎ,true,ϋ.Γφ,Ζϋ,"IG"))
Λ=Ȯ;}break;case "CFGA":{if(!Ń(ref ȃ,false,ϋ.Ας,ȥ,"")) Λ=Ȯ;}break;case "CFGW":{if(!Ń(ref ƭ,false,ϋ.ǁ,ȥ,"")||
!Ń(ref Ƭ,false,ϋ.ǁ,ǅ,""))
Λ=Ȯ;}break;case "NEXT":{ΓΗ(false);}break;case "PREV":{ΓΗ(true);}break;default:if(Εν) Λ=Ȯ;break;}}else{switch(І){case "UNDOCK":{Ακ=true;}break;default:if(Εν) Λ=Ȯ;break;}}}String ΘЎ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Μ!=Κ.ή)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΘЃ(){Ђ();Μ=Κ.ή;ȡ(х.ή);Ζ.λ=false;Ο.λ=false;ψ=null;ω.Clear();γ=Π.э;}void Γζ(bool М,bool Ϗ){if(!М) Λ="Aligning canceled";if(Ϗ) Λ="Aligning done";if(Ϗ||!М){Ɗ=false;Ζξ();ΒΒ(false,0,0,0,0);return;}if(ΑЅ()) Ɗ=true;}bool Θό(String Λή,String Κα,String ΚΡ)
{bool ΛΪ=γ==Π.В;int w,h,d;if(int.TryParse(Λή,out w)&&int.TryParse(Κα,out h)&&int.TryParse(ΚΡ,out d)){this.Ђ();ѐ=w;я=h;н=d;ŉ(false);ǀ(false,false);if(ΛΪ) Ĥ();return true;}return false;}bool Θύ(String Ǆ){if(Ǆ=="ON"){ġ=true;return true;}if(Ǆ=="OFF"){ġ=false;return true;}
return false;}bool Θϋ(String ΗΡ,String ΒΨ){bool ȱ=true;if(ΗΡ=="HOME") Ļ=true;else if(ΗΡ=="STOP") Ļ=false;else ȱ=false;if(ΒΨ=="HOME") ѓ=ъ.ΑА;else if(ΒΨ=="STOP") ѓ=ъ.Ζν;else if(ΒΨ=="JOB") ѓ=ъ.Γΰ;else if(ΒΨ=="IG") ѓ=ъ.Ƴ;else ȱ=false;return ȱ;}void μ(Σ ц){Ηό[(int) ό]=Ў;
Ў=Ηό[(int) ц];if(ц==Σ.ƽ) Ў=0;ό=ц;if(Μ!=Κ.ј)
ň(ό==Σ.Ƹ,false,0,0);ΕΒ=true;}void Γτ(bool ΔЊ){if(Ў>0) Ў--;else if(ΔЊ) Ў=Ŝ-1;}void Γσ(bool ΔЊ){if(Ў<Ŝ-1) Ў++;else if(ΔЊ) Ў=0;}String ȡ(х β){switch(β){case х.ΑБ:Λ="Job is running";break;case х.Ư:Λ="Connector not ready!";break;case х.Ιο:Λ="Ship modified, path outdated!";break;
case х.ΖΠ:Λ="Interrupted by player!";break;case х.ή:Λ="Shuttle mode enabled!";break;}return "";}String ΙΓ(ю κ){switch(κ){case ю.ƿ:return "Top-Left";case ю.ΕЀ:return "Center";default:return "";}}String ΘЊ(Ǖ Џ){switch(Џ){case Ǖ.ΓΝ:return "Auto"+(Μ==Κ.ƹ?" (Ore)":"");case Ǖ.ΓΞ:return "Auto (+Stone)";
case Ǖ.ΓΧ:return "Default";default:return "";}}String Γα(Π Є){switch(Є){case Π.э:return "No job";case Π.ŵ:return "Job paused";case Π.В:return "Job active";case Π.Ū:return "Job active";case Π.ū:return "Job active";case Π.ΓΪ:return "Job done";case Π.Αχ:return "Job changed";case Π.ΑЈ:return "Move home";
case Π.ΑЉ:return "Move to job";}return "";}String ΕЎ(ъ ΚΔ){switch(ΚΔ){case ъ.ΑА:return "Return home";case ъ.Γΰ:return "Fly to job pos";case ъ.Ζν:return "Stop";case ъ.Ƴ:return "Ignore";}return "";}String ΘЌ(Г Џ){switch(Џ){case Г.Γψ:return "Off";case Г.ΓΫ:return "Drop pos (Stone) ";case Г.Αω:return "Drop pos (Sto.+Ice)";
case Г.ΕЄ:return "Cur. pos (Stone)";case Г.ΓΥ:return "Cur. pos (Sto.+Ice)";case Г.Γκ:return "In motion (Stone)";case Г.ΑЃ:return "In motion (Sto.+Ice)";}return "";}String ΕЌ(ї Є){switch(Є){case ї.None:return "No batteries";case ї.ŀ:return "Charging";case ї.ΕЇ:return "Discharging";}return "";}
String Γγ(Α Є){String Λθ=Μ==Κ.ή?"target":"job";switch(Є){case Α.ь:return "Idle";case Α.Ć:return "Flying to XY position";case Α.Ŵ:return Μ==Κ.Ǫ?"Grinding":"Mining";case Α.Ƞ:return "Returning";case Α.Ʊ:return "Flying to drop pos";case Α.Ņ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.у:return "Flying to job area";case Α.Ħ:return "Flying to path";case Α.ů:return "Flying to job position";case Α.Ȁ:return "Approaching dock";case Α.Ł:return "Docking";case Α.Ľ:return "Aligning";case Α.Ģ:return "Aligning";case Α.ƻ:return "Retry docking";case Α.Ȥ:return "Unloading";case Α.ĩ:return Θγ;
case Α.ǭ:return "Undocking";case Α.ŀ:return "Charging batteries";case Α.ΒΙ:return "Waiting for uranium";case Α.Αϋ:return "Filling up hydrogen";case Α.ΔΖ:return "Waiting for ejection";case Α.ΒΘ:return "Waiting for ejection";case Α.Αύ:return "Flying to drop pos";}return "";}String ΖΗ(ρ Р){switch(Р)
{case ρ.Γϊ:return "On \"Undock\" command";case ρ.Γώ:return "On player entered cockpit";case ρ.Ζή:return "On ship is full";case ρ.Ζέ:return "On ship is empty";case ρ.ΓϏ:return "On time delay";case ρ.ΑЍ:return "On batteries empty(<25%)";case ρ.ΑЌ:return "On batteries empty(=0%)";case ρ.Γω:return "On batteries full";
case ρ.Γύ:return "On hydrogen empty(<25%)";case ρ.Γϋ:return "On hydrogen empty(=0%)";case ρ.Γό:return "On hydrogen full";}return "";}bool Ν(ref String Γ,int ΗА,int ĳ,bool Ι,String ř){Ŝ+=1;if(ΗА==ĳ) ř=">"+ř+(ΒΝ>=2?" .":"");else ř=" "+ř;Γ+=ř+"\n";return ΗА==ĳ&&Ι;}String Εό(bool Ι)
{int Β=0;int Η=Ў;Ŝ=0;String ƞ="———————————————\n";String ƥ="--------------------------------------------\n";String Γ="";Γ+=Γα(γ)+" | "+(Ο.λ?"Ready to dock":"No dock")+"\n";Γ+=ƞ;double Ȕ=Math.Max(Math.Round(this.в),0);if(ό==Σ.Main){bool ĵ=Μ==Κ.ή;if(Ν(ref Γ,Η,Β++,Ι," Record path & set home")) ΓА();
if(Μ==Κ.ƹ)
if(Ν(ref Γ,Η,Β++,Ι," Setup mining job")) μ(Σ.Ƹ);if(Μ==Κ.Ǫ)
if(Ν(ref Γ,Η,Β++,Ι," Setup grinding job")) μ(Σ.Ƹ);if(Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Setup shuttle job")) μ(Σ.ΓЌ);if(Ν(ref Γ,Η,Β++,Ι," Continue job")) Ĥ();if(Ν(ref Γ,Η,Β++,Ι," Fly to home position")) Αώ();if(Ν(ref Γ,Η,Β++,Ι," Fly to job position")) ł();if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(ĵ) μ(Σ.ΒΓ);else μ(Σ.Αυ);if(Ν(ref Γ,Η,Β++,Ι," Info")) μ(Σ.ΖΜ);if(Μ!=Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Help")) μ(Σ.ΖΘ);}else if(ό==Σ.Ƹ){double Λέ=Math.Round(ѐ*ș,1);double Κΰ=Math.Round(я*Ș,1);String З="";if(Ν(ref З,Η,Β++,Ι," Start new job!")) ΓЏ();if(Ν(ref З,Η,Β++,Ι," Change current job")){ǀ(false,false);μ(Σ.Main);}if(Ν(ref З,Η,Β++,Ι," Width + (Width: "+ѐ+" = "+Λέ+"m)")){Ŭ(ref ѐ,5,20,1);ŉ(true);}
if(Ν(ref З,Η,Β++,Ι," Width -")){Ŭ(ref ѐ,-5,20,-1);ŉ(true);}if(Ν(ref З,Η,Β++,Ι," Height + (Height: "+я+" = "+Κΰ+"m)")){Ŭ(ref я,5,20,1);ŉ(true);}if(Ν(ref З,Η,Β++,Ι," Height -")){Ŭ(ref я,-5,20,-1);ŉ(true);}if(Ν(ref З,Η,Β++,Ι," Depth + ("+(ћ==Ǖ.ΓΧ?"Depth":"Min")+": "+н+"m)")){Ŭ(ref н,5,50,2);ŉ(true);}
if(Ν(ref З,Η,Β++,Ι," Depth -")){Ŭ(ref н,-5,50,-2);ŉ(true);}if(Ν(ref З,Η,Β++,Ι," Depth mode: "+ΘЊ(ћ))){ћ=ĥ(ћ);}if(Ν(ref З,Η,Β++,Ι," Start pos: "+ΙΓ(ȓ))){ȓ=ĥ(ȓ);}if(Μ==Κ.Ǫ&&ћ==Ǖ.ΓΞ) ћ=ĥ(ћ);Γ+=ΓІ(8,З,Η,ref ΚЊ);}else if(ό==Σ.ΓЌ){float[]ΗΘ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.Ζκ);if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=" Leave connector 1:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΗ(Ї.Р))) Ї.Р=ĥ(Ї.Р);if(!Ї.Γη()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑϏ((int) Ї.Ǳ))) Ї.Ǳ=ΕЅ(Ї.Ǳ,ΗΘ);Γ+=" Leave connector 2:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΗ(Ј.Р))) Ј.Р=ĥ(Ј.Р);
if(!Ј.Γη()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑϏ((int) Ј.Ǳ))) Ј.Ǳ=ΕЅ(Ј.Ǳ,ΗΘ);}else if(ό==Σ.Ζκ){if(Ν(ref Γ,Η,Β++,Ι," Start job!")) ΓЏ();if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.ΓЌ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.ǲ!=""?Ї.ǲ:"-"))) Ї.ǲ=ΑЊ(ref ΛΜ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.Ƕ!=""?Ї.Ƕ:"-"))) Ї.Ƕ=ΑЊ(ref ΛΞ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǲ!=""?Ј.ǲ:"-"))) Ј.ǲ=ΑЊ(ref ΛΝ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.Ƕ!=""?Ј.Ƕ:"-"))) Ј.Ƕ=ΑЊ(ref ΛΟ);
}else if(ό==Σ.Ʒ){String Ησ=ȏ+" %";if(ĕ) Ĵ++;if(Ĵ>1){Ĵ=0;Βι++;if(Βι>1) Βι=0;}bool[]ΕΩ=new bool[]{ķ.Count==0,Ȅ==ї.None,ǜ.Count==0};int Θή=0;while(true){Θή++;ŕ++;if(ŕ>ΕΩ.Length-1) ŕ=0;if(Θή>=ΕΩ.Length) break;if(!ΕΩ[ŕ]) break;}bool ĵ=Μ==Κ.ή;if(!ĵ&&ġ&&д!=-1&&Βι==0)
Ησ=д<1000000?Math.Round(д)+" Kg":Math.Round(д/1000)+" t";if(Ν(ref Γ,Η,Β++,Ι," Stop!")){Ђ();μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(!ĵ) μ(Σ.Αυ);else μ(Σ.ΒΓ);if(!ĵ){if(Ν(ref Γ,Η,Β++,Ι," Next hole")) ΓΗ(false);}else if(Ν(ref Γ,Η,Β++,Ι," Undock")) Ακ=true;Γ+=ƥ;if(!ĵ) Γ+="Progress: "+Math.Round(ΑΠ,1)+" %\n";Γ+="State: "+Γγ(Ϊ)+" "+Ȕ+"m \n";Γ+="Load: "+Βρ+" % Max: "+Ησ+" \n";if(ŕ==0) Γ+="Uranium: "+(ķ.Count==0?"No reactors":Math.Round(ŧ,1)+"Kg "+(п==-1?"":" Min: "+п+" Kg"))+"\n";
if(ŕ==1) Γ+="Battery: "+(Ȅ==ї.None?ΕЌ(Ȅ):Ȧ+"% "+(џ==-1||ĵ?"":" Min: "+џ+" %"))+"\n";if(ŕ==2) Γ+="Hydrogen: "+(ǜ.Count==0?"No tanks":Math.Round(Ĕ,1)+"% "+(Ǎ==-1||ĵ?"":" Min: "+Ǎ+" %"))+"\n";}else if(ό==Σ.Αυ){String З="";if(Ν(ref З,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}
if(Ν(ref З,Η,Β++,Ι," Max load: "+ȏ+"%")) щ(ref ȏ,ȏ<=80?-10:-5,ϋ.ΑІ,false);if(Ν(ref З,Η,Β++,Ι," Weight limit: "+(ġ?"On":"Off"))) ġ=!ġ;if(Ν(ref З,Η,Β++,Ι," Ejection: "+ΘЌ(г))) г=ĥ(г);if(Ν(ref З,Η,Β++,Ι," Toggle sorters: "+(ť?"On":"Off"))){ť=!ť;if(ť) Ȃ(Εά);}
if(Ν(ref З,Η,Β++,Ι," Unload ice: "+(Ġ?"On":"Off"))) Ġ=!Ġ;if(Ν(ref З,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЇ,true);if(Ν(ref З,Η,Β++,Ι," Battery: "+(џ==-1?"Ignore":"Min "+џ+"%"))) щ(ref џ,-5,ϋ.Γυ,true);if(Ν(ref З,Η,Β++,Ι," Hydrogen: "+(Ǎ==-1?"Ignore":"Min "+Ǎ+"%"))) щ(ref Ǎ,-10,ϋ.Γφ,true);
if(Ν(ref З,Η,Β++,Ι," When done: "+(Ļ?"Return home":"Stop"))) Ļ=!Ļ;if(Ν(ref З,Η,Β++,Ι," On damage: "+ΕЎ(ѓ))) ѓ=ĥ(ѓ);if(Ν(ref З,Η,Β++,Ι," Advanced...")) μ(Σ.ΓΘ);Γ+=ΓІ(8,З,Η,ref ΚЋ);}else if(ό==Σ.ΓΘ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}
if(Ν(ref Γ,Η,Β++,Ι,(Μ==Κ.Ǫ?" Grinder":" Drill")+" inv. balancing: "+(Ɖ?"On":"Off"))) Ɖ=!Ɖ;if(Ν(ref Γ,Η,Β++,Ι," Enable"+(Μ==Κ.Ǫ?" grinders":" drills")+": "+(ƃ?"Fwd + Bwd":"Fwd"))) ƃ=!ƃ;if(Ν(ref Γ,Η,Β++,Ι," Work speed fwd.: "+ƭ+"m/s")) щ(ref ƭ,0.5f,ϋ.ǁ,false);
if(Ν(ref Γ,Η,Β++,Ι," Work speed bwd.: "+Ƭ+"m/s")) щ(ref Ƭ,0.5f,ϋ.ǁ,false);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(ȃ*100f)+"%"+(ȃ>0.80f?" (risky)":""))) щ(ref ȃ,0.1f,ϋ.Ας,false);if(Ν(ref Γ,Η,Β++,Ι," Width overlap: "+ũ*100f+"%")) ΕЁ(true,0.05f);if(Ν(ref Γ,Η,Β++,Ι," Height overlap: "+œ*100f+"%")) ΕЁ(false,0.05f);
}else if(ό==Σ.ΒΓ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Max load: "+ȏ+"%")) щ(ref ȏ,ȏ<=80?-10:-5,ϋ.ΑІ,false);if(Ν(ref Γ,Η,Β++,Ι," Unload ice: "+(Ġ?"On":"Off"))) Ġ=!Ġ;if(Ν(ref Γ,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЇ,true);
if(Ν(ref Γ,Η,Β++,Ι," Battery: "+(џ==-1?"Ignore":"Charge up"))) џ=(џ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," Hydrogen: "+(Ǎ==-1?"Ignore":"Fill up"))) Ǎ=(Ǎ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," On damage: "+ΕЎ(ѓ))) ѓ=ĥ(ѓ);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(ȃ*100f)+"%"+(ȃ>0.80f?" (risky)":""))) щ(ref ȃ,0.1f,ϋ.Ας,false);
}else if(ό==Σ.Ζγ){double Κλ=0;if(Ψ.Count>0) Κλ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ν(ref Γ,Η,Β++,Ι," Stop path recording")) Ζο();if(Μ!=Κ.ή){if(Ν(ref Γ,Η,Β++,Ι," Home: "+(Ă?"Use old home":(Ο.λ?"Was set! ":"none ")))) Ă=!Ă;}else{if(Ν(ref Γ,Η,Β++,Ι," Connector 1: "+(Ă?"Use old connector":(Ο.λ?"Was set! ":"none ")))) Ă=!Ă;
if(Ν(ref Γ,Η,Β++,Ι," Connector 2: "+(ƨ?"Use old connector":(Ζ.λ?"Was set! ":"none ")))) ƨ=!ƨ;}if(Ν(ref Γ,Η,Β++,Ι," Path: "+(Ʃ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) Ʃ=!Ʃ;Γ+=ƥ;Γ+="Wp spacing: "+Math.Round(Θε)+"m\n";}else if(ό==Σ.ΓБ){if(Ν(ref Γ,Η,Β++,Ι," Stop")){Ђ();μ(Σ.Main);}
Γ+=ƥ;Γ+="State: "+Γγ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(Ĝ,1)+"m/s\n";Γ+="Target dist: "+Ȕ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+Ερ+"\n";}else if(ό==Σ.ΖΜ){List<IMyTerminalBlock>Ŏ=ΕЏ();if(ĕ) Ĵ++;if(Ĵ>=Ŏ.Count) Ĵ=0;if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.ΖΝ);
Γ+=ƥ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βρ,1)+"% "+Math.Round(ȉ,1)+" / "+Math.Round(Ȑ,1)+"\n";Γ+="Uranium: "+(ķ.Count==0?"No reactors":Math.Round(ŧ,1)+"Kg "+ΔЌ)+"\n";Γ+="Battery: "+(Ȅ==ї.None?"":Ȧ+"% ")+ΕЌ(Ȅ)+"\n";Γ+="Hydrogen: "+(ǜ.Count==0?"No tanks":Math.Round(Ĕ,1)+"% ")+"\n";
Γ+="Damage: "+(Ŏ.Count==0?"None":""+(Ĵ+1)+"/"+Ŏ.Count+" "+Ŏ[Ĵ].CustomName)+"\n";}else if(ό==Σ.ΖΝ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=ƥ;Γ+="Next scan: "+Αή+"s\n";Γ+="Drill area: "+Math.Round(ș,1)+"x"+Math.Round(Ș,1)+"m Ship: "+Math.Round(Ɵ,1)+"x"+Math.Round(Αΰ,1)+"m ("+Math.Round(ο,1)+"m)\n";
Γ+="Broadcast: "+(ƀ?"Online - "+Ơ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ə*100f,1)+"% \n";}else if(ό==Σ.ΖΘ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=ƥ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+Αή+"s\n";if(ΒЂ) Γ="Recompile script now";int Κμ=Γ.Split('\n').Length;for(int i=Κμ;i<=10;i++) Γ+="\n";Γ+=ƞ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕЁ(bool Κη,float Ǥ){Ђ();ǀ(true,false);if(Κη) щ(ref ũ,Ǥ,ϋ.ΓЀ,false);else щ(ref œ,Ǥ,ϋ.ΓЀ,false);ΕϏ();ň(true,true,0,0);}float ΕЅ(float ξ,float[]ΓΔ){float ί=ΓΔ[0];for(int i=ΓΔ.Length-1;i>=0;i--)
if(ξ<ΓΔ[i]) ί=ΓΔ[i];return ί;}String ΑЊ(ref int ĳ){String ί="";if(ĳ>=Εε.Count) ĳ=-1;if(ĳ>=0) ί=Εε[ĳ].CustomName;ĳ++;return ί;}void ΒΖ(string ΰ){if(γ!=Π.В) return;if(ΰ=="") return;IMyTerminalBlock Ω=χ.GetBlockWithName(ΰ);if(Ω==null||!(Ω is IMyTimerBlock)){Λ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ω).Trigger();}void Ŭ(ref int value,int ǉ,int ğ,int ΕΪ){if(ǉ==0) return;if(value<ğ&&ΕΪ>0||value<=ğ&&ΕΪ<0){value+=ΕΪ;return;}int ΙЌ=Math.Abs(ǉ);int ΔΗ=0;int ΕΔ=1;while(true){ΔΗ+=ΕΔ*ΙЌ*10;if(ǉ<0&&value-ğ<=ΔΗ) break;if(ǉ>0&&value-ğ<ΔΗ) break;
ΕΔ++;}value+=ΕΔ*ǉ;}void ŉ(bool Λλ){ѐ=Math.Max(ѐ,1);я=Math.Max(я,1);н=Math.Max(н,0);ň(ό==Σ.Ƹ,false,0,0);}bool Ń(ref float value,bool ΚЇ,ϋ υ,String Ŗ,String Κδ){if(Ŗ=="") return false;float Ɠ=-1;bool Ηγ=false;if(Ŗ.ToUpper()==Κδ) Ηγ=true;else if(!float.TryParse(Ŗ,out Ɠ)) return false;
else Ɠ=Math.Max(0,Ɠ);if(ΚЇ) Ɠ=(float) Math.Round(Ɠ);щ(ref value,Ɠ,υ,Ηγ,false);return true;}void щ(ref float value,float Ǥ,ϋ υ,bool Αι){щ(ref value,value+Ǥ,υ,Αι,true);}void щ(ref float value,float Ɛ,ϋ υ,bool Αι,bool Λβ){float Ǹ=0,Ƿ=0;if(υ==ϋ.ǁ){Ǹ=0.5f;Ƿ=10f;}if(υ==ϋ.Ας){Ǹ=0.1f;Ƿ=1f;}
if(υ==ϋ.Ƴ){Ǹ=50f;Ƿ=100f;}if(υ==ϋ.Γυ){Ǹ=5f;Ƿ=30f;}if(υ==ϋ.ΑЇ){Ǹ=1f;Ƿ=25f;}if(υ==ϋ.ΑІ){Ǹ=10f;Ƿ=95f;}if(υ==ϋ.Γφ){Ǹ=10f;Ƿ=90f;}if(υ==ϋ.Ιφ){Ǹ=10f;Ƿ=1800;}if(υ==ϋ.ΓЀ){Ǹ=0.0f;Ƿ=0.75f;}if(Ɛ==-1&&Αι){value=-1;return;}if(value==-1) Αι=false;
bool ΗЉ=Ɛ<Ǹ||Ɛ>Ƿ;if(ΗЉ&&Λβ){if(Ɛ<value) value=Ƿ;else if(Ɛ>value) value=Ǹ;}else value=Ɛ;if(ΗЉ&&Αι) value=-1;else value=Math.Max(Ǹ,Math.Min(value,Ƿ));value=(float) Math.Round(value,2);}void ΓΗ(bool ΚЂ){if(ΚЂ) б=Math.Max(0,б-1);else б++;Γέ();Γά();for(int k=0;k<ȭ.Count&&k<й.Length;k++) ΓЊ(ȭ[k],k<б);
Д=-1;ΔΕ(true);}T ĥ<T>(T ξ){int Β=Array.IndexOf(Enum.GetValues(ξ.GetType()),ξ);Β++;if(Β>=ΘЄ(ξ)) Β=0;return(T) Enum.GetValues(ξ.GetType()).GetValue(Β);}int ΘЄ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓА(){Απ.Clear();for(int i=0;i<Ψ.Count;i++) Απ.Add(Ψ[i]);Ψ.Clear();
Βν=true;Δύ=new ά(Ο);ΔЀ=new ά(Ζ);Ο.λ=false;if(Μ==Κ.ή) Ζ.λ=false;for(int i=0;i<ǥ.Count;i++)
if(!Ā.Contains(ǥ.Keys.ElementAt(i)))
Ā.Add(ǥ.Keys.ElementAt(i));Ă=false;ƨ=false;Ʃ=false;μ(Σ.Ζγ);}void Ζο(){if(Ă) Ο=Δύ;if(ƨ) Ζ=ΔЀ;if(Ʃ){Ψ.Clear();for(int i=0;i<Απ.Count;i++) Ψ.Add(Απ[i]);}Βν=false;Ђ();μ(Σ.Main);}void ΙЄ(){if(!Βν) return;if(Ϊ!=Α.ь){Ζο();return;}if(!Δύ.λ) Ă=false;if(!ΔЀ.λ) ƨ=false;if(Απ.Count<=1) Ʃ=false;
IMyShipConnector О=ж(MyShipConnectorStatus.Connectable);if(О==null) О=ж(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Ĝ,2)<=0.20) Δι++;else Δι=0;if(Δι>=5){if(Μ==Κ.ή&&(Ο.λ||Ă)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.ι=Ϋ.WorldMatrix.Forward;Ζ.Ѓ=Ϋ.WorldMatrix.Left;
Ζ.θ=Ϋ.WorldMatrix.Down;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.λ=true;Ζ.о=О.Position;}else{Ο.ι=Ϋ.WorldMatrix.Forward;Ο.Ѓ=Ϋ.WorldMatrix.Left;Ο.θ=Ϋ.WorldMatrix.Down;Ο.φ=Ϋ.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.λ=true;Ο.о=О.Position;}}}double ΔЂ=-1;if(Ψ.Count>0) ΔЂ=Vector3.Distance(δ,Ψ.Last().Ε);
double ΛΑ=Math.Max(1.5,Math.Pow(Ĝ/100.0,2));double Ηύ=Math.Max(Ĝ*ΛΑ,2);Θε=Ηύ;if((ΔЂ==-1)||ΔЂ>=Ηύ){ά έ=new ά(δ,Βζ,ΑΓ,ΑΤ,Ϋ.GetNaturalGravity());έ.Θπ(ā,Ā);Ψ.Add(έ);}}ά ы(){if(Μ!=Κ.ή) return ǟ!=null?ǟ:Ο;return ΗΓ;}void ŷ(ά α,Π Є){ΗΓ=α;if(γ==Π.В) ў=Є;}bool ȝ(ά α,float ϊ,bool ΛΩ,out Vector3 κ)
{if(ΛΩ){Vector3I Δυ=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock Ω=Me.CubeGrid.GetCubeBlock(Δυ);if(Ω==null||!(Ω.FatBlock is IMyShipConnector)){κ=new Vector3();return false;}Vector3 ΚΚ=ф(Ϋ,Ω.FatBlock.GetPosition()-δ);Vector3 ΚΙ=ф(Ϋ,Ω.FatBlock.WorldMatrix.Forward);κ=α.Ε-ΔΔ(α.ι,α.θ*-1,ΚΚ)-ΔΔ(α.ι,α.θ*-1,ΚΙ)*ϊ;
return true;}else{κ=α.Ε;return true;}}Vector3 ć(int ΚΗ,int ƙ,bool ΚЅ){if(!ΚЅ&&Ηά) return ΔΩ;float ΚΘ=((Ќ-1f)/2f)-ΚΗ;float ΚЈ=((Ю-1f)/2f)-ƙ;ΔΩ=Ζ.Ε+Ζ.Ѓ*ΚΘ*ș+ę*-1*ΚЈ*Ș;Ηά=true;return ΔΩ;}Vector3 Γβ(Vector3 ΛΕ,float Δδ){return ΛΕ+(ǹ*Δδ);}ε ΓΨ(){float Ы=-1;ε ί=ε.ț;
if(Μ!=Κ.ή){ΖΨ();if(γ!=Π.э){Vector3 Н=Ň(ǹ,ę*-1,δ-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ќ*ș)/2f&&Math.Abs(Н.Y)<=(float)(Ю*Ș)/2f){if(Н.Z<=-1&&Н.Z>=-Ţ*2) return ε.Ατ;if(Н.Z>-1&&Н.Z<ο*2) return ε.ț;}if(ģ(Ζ.Ε,ref Ы)) ί=ε.ț;}if(Ο.λ){if(ģ(Ο.Ε,ref Ы)) ί=ε.Ț;for(int i=0;i<Ψ.Count;i++)
if(ģ(Ψ[i].Ε,ref Ы)) ί=ε.ĉ;if(Vector3.Distance(δ,Ο.Ε)<dockDist*ο) ί=ε.Ț;if(ж(MyShipConnectorStatus.Connectable)!=null||ж(MyShipConnectorStatus.Connected)!=null)
ί=ε.Ț;}}else{Vector3 κ=new Vector3();IMyShipConnector ȩ=ж(MyShipConnectorStatus.Connected);if(Ο.λ){if(ģ(Ο.Ε,ref Ы)) ί=ε.Ț;if(ȝ(Ο,dockDist,true,out κ))
if(ģ(κ,ref Ы)) ί=ε.Ț;if(ȩ!=null&&Vector3.Distance(ȩ.GetPosition(),Ο.Ε)<5)
return ε.ΓΛ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ο.Ε)>dockDist*ο&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*ο)
if(ģ(Ψ[i].Ε,ref Ы)) ί=ε.ĉ;if(Ζ.λ){if(ģ(Ζ.Ε,ref Ы)) ί=ε.ț;if(ȝ(Ζ,dockDist,true,out κ))
if(ģ(κ,ref Ы)) ί=ε.ț;if(ȩ!=null&&Vector3.Distance(ȩ.GetPosition(),Ζ.Ε)<5)
return ε.ΓΜ;}}return ί;}bool ģ(Vector3 κ,ref float Ы){float ϊ=Vector3.Distance(κ,δ);if(ϊ<Ы||Ы==-1){Ы=ϊ;return true;}return false;}void ΓЏ(){if(А>0){Λ="Setup error! Can't start";return;}if(Μ==Κ.ή){Ĥ();return;}Ζ.Ε=δ;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.ι=Βζ;Ζ.θ=ΑΓ;Ζ.Ѓ=ΑΤ;
ǹ=Ǡ.WorldMatrix.Forward;ę=Ζ.θ;if(ǹ==Ϋ.WorldMatrix.Down) ę=Ϋ.WorldMatrix.Backward;ǀ(true,true);Τ(Α.Ć);ΓЎ();}void ǀ(bool Ǵ,bool ΚІ){if(γ==Π.э&&!Ǵ) return;bool Κτ=Ǵ||γ==Π.ΓΪ||Ќ!=ѐ||Ю!=я||Ř!=ȓ;if(Κτ){if(γ!=Π.э){γ=Π.Αχ;ć(Ѐ,ύ,ΚІ);Λ="Job changed, lost progress";}Ř=ȓ;
Ќ=ѐ;Ю=я;ύ=0;Ѐ=0;й="";Д=-1;ś=0;ė=0;и=0;б=0;ΔΕ(true);}}void ΓЂ(){з(δ,0);ǘ(ā,true);}void ΓЎ(){ȡ(х.ΑБ);ΓЂ();Ƽ(Ȱ,false);Ȃ(Εά);γ=Π.В;Εώ(true);ў=γ;μ(Σ.Ʒ);Ιδ();ΒΩ=true;Βκ=0;for(int i=ǂ.Count-1;i>=0;i--)
if(ƴ(ǂ[i],false)) Βκ++;if(Βκ>0) Λ="Started with damage";}void Ђ(){if(γ==Π.В){γ=Π.ŵ;Λ="Job paused";}Τ(Α.ь);ў=γ;ΒΒ(false,0,0,0,0);ΔΑ();Εω(new Vector3(),false);Ζξ();ΓЉ(ChargeMode.Auto);ΓЋ(false);Ζε(true);Ċ(Α.ь);ň(false,false,0,0);ǘ(ω,false);ǘ(њ,true);Ȃ(true);ΒЃ=false;Αψ();
ΑΒ=false;ΓЄ();ΒΩ=false;Αα=false;Ακ=false;if(ό!=Σ.Main&&ό!=Σ.Αυ&&ό!=Σ.ΓΘ&&ό!=Σ.ΒΓ)
μ(Σ.Main);}void Ĥ(){ε Я=ΓΨ();if(Μ==Κ.ή){if(!Ζ.λ||!Ο.λ) return;ΓЎ();bool Δσ=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(Ɯ==Π.Ū) Δσ=true;if(Ɯ==Π.ū) Δσ=false;if(Δσ){ŷ(Ο,Π.Ū);switch(Я){case ε.ΓΛ:Τ(Α.ĩ);break;case ε.ĉ:Τ(Α.у);break;case ε.Ț:Τ(Α.Ȁ);break;default:Τ(Α.ǭ);break;
}}else{ŷ(Ζ,Π.ū);switch(Я){case ε.ΓΜ:Τ(Α.ĩ);break;case ε.ț:Τ(Α.Ȁ);break;case ε.ĉ:Τ(Α.у);break;default:Τ(Α.ǭ);break;}}}else{if(γ!=Π.ŵ&&γ!=Π.Αχ) return;bool ΛΫ=γ==Π.Αχ;ΓЎ();bool Δϊ=Ƶ(false)&&Ο.λ;switch(Я){case ε.ț:Τ(Δϊ?Α.Ħ:Α.Ć);break;case ε.ĉ:Τ(Δϊ?Α.И:Α.у);break;case ε.Ț:Τ(Δϊ?Α.Ȁ:Α.Ȥ);break;
case ε.Ατ:{if(ė!=б||ΛΫ) Τ(Α.Ƞ);else Τ(Α.Ŵ);}break;default:break;}}}void ł(){if(γ==Π.э&&!Ο.λ) return;if(Μ==Κ.ή&&(!Ζ.λ||!Ο.λ)) return;Λ="Move to job";ε Я=ΓΨ();if(Μ==Κ.ή){ŷ(Ζ,Π.ū);switch(Я){case ε.ț:Τ(Α.Ȁ);break;case ε.ĉ:Τ(Α.у);break;case ε.ΓΜ:return;default:Τ(Α.ǭ);break;
}Ċ(Α.ĩ);}else{switch(Я){case ε.ț:Τ(Α.у);break;case ε.ĉ:Τ(Α.у);break;case ε.Ț:Τ(Α.Ȥ);break;case ε.Ατ:Τ(Α.Ƞ);break;default:break;}if(γ==Π.э) Ċ(Α.у);else Ċ(Α.Ģ);ΒЃ=true;}ΓЂ();μ(Σ.ΓБ);Ƽ(Ȱ,false);ў=Π.ΑЉ;}void Αώ(){if(!Ο.λ) return;Λ="Move home";ε Я=ΓΨ();if(Μ==Κ.ή){ŷ(Ο,Π.Ū);switch(Я)
{case ε.ĉ:Τ(Α.И);break;case ε.Ț:Τ(Α.Ȁ);break;case ε.ΓΛ:return;default:Τ(Α.ǭ);break;}Ċ(Α.ĩ);}else{if(ж(MyShipConnectorStatus.Connected)!=null) return;if(ж(MyShipConnectorStatus.Connectable)!=null){Τ(Α.Ł);Ċ(Α.Ȥ);return;}switch(Я){case ε.ț:Τ(Α.Ħ);break;case ε.ĉ:Τ(Α.И);break;case ε.Ț:Τ(Α.И);break;
case ε.Ατ:Τ(Α.Ņ);break;default:break;}Ċ(Α.Ȥ);}ΓЂ();μ(Σ.ΓБ);Ƽ(Ȱ,false);ў=Π.ΑЈ;}void Τ(Α ΕΕ){if(ΕΕ==Α.ь) Αδ=Α.ь;if(Αδ!=Α.ь&&Ϊ==Αδ&&ΕΕ!=Αδ){Ђ();return;}Е=true;Ϊ=ΕΕ;}void Ċ(Α Є){this.Αδ=Є;}Ǭ Λη(int ΑΞ,bool ǻ){if(ǻ){ǣ=null;Ѐ=0;ύ=0;}if(ȓ==ю.ƿ){int ŝ=ΑΞ+1;
ύ=(int) Math.Floor(Й(ΑΞ,Ќ));if(ύ%2==0) Ѐ=ΑΞ-(ύ*Ќ);else Ѐ=Ќ-1-(ΑΞ-(ύ*Ќ));if(ύ>=Ю) return Ǭ.Ασ;else return Ǭ.ΑЀ;}else if(ȓ==ю.ΕЀ){if(ǣ==null) ǣ=new int[]{0,-1,0,0};int ΑΛ=(int) Math.Ceiling(Ќ/2f);int ΑΚ=(int) Math.Ceiling(Ю/2f);int Δψ=(int) Math.Floor(Ќ/2f);
int Δχ=(int) Math.Floor(Ю/2f);int ΑΟ=0;while(ǣ[2]<Math.Pow(Math.Max(Ќ,Ю),2)){if(ΑΟ>200) return Ǭ.ΘЅ;ΑΟ++;ǣ[2]++;if(-ΑΛ<Ѐ&&Ѐ<=Δψ&&-ΑΚ<ύ&&ύ<=Δχ){if(ǣ[3]==ΑΞ){this.Ѐ=Ѐ-1+ΑΛ;this.ύ=ύ-1+ΑΚ;return Ǭ.ΑЀ;}ǣ[3]++;}if(Ѐ==ύ||(Ѐ<0&&Ѐ==-ύ)||(Ѐ>0&&Ѐ==1-ύ))
{int ΛΘ=ǣ[0];ǣ[0]=-ǣ[1];ǣ[1]=ΛΘ;}Ѐ+=ǣ[0];ύ+=ǣ[1];}}return Ǭ.Ασ;}void ΙЂ(){if(Ϊ==Α.Ć){if(Е){Ж=0;if(ė!=б) ś=0;ė=б;}if(Ж==0){int Δά=Θφ(б);if(Δά<0){if(!Δα){Δα=true;ΖБ=δ;}з(ΖБ,0);Λ="Waiting for the controller";return;}Δα=false;if(Δά!=б){б=Δά;ś=0;ė=б;}
Ǭ ί=Ιν(б);if(ί==Ǭ.Ασ){γ=Π.ΓΪ;bool ΗЈ=й.IndexOf('0')>=0;Λ=ΗЈ?"No free hole left for this ship":"Job done";if((Ļ||ΗЈ)&&Ο.λ){Τ(Α.Ħ);Ċ(Α.Ȥ);ў=Π.ΑЈ;}else{Τ(Α.ů);Ċ(Α.Ģ);ў=Π.ΑЉ;}return;}if(ί==Ǭ.ΑЀ){Ж=1;ǘ(ω,true);ǎ=ć(Ѐ,ύ,true);з(ǎ,10);л(Ζ.θ,Ζ.ι,Ζ.Ѓ,false);}}else
{if(Д>=0&&(ΖΧ()?Ȉ>=0&&Ȉ!=Д:Γι(Д,true))){Ж=0;return;}if(в<wpReachedDist){Τ(Α.Ŵ);return;}}}if(Ϊ==Α.Ŵ){if(Е){ǘ(ω,true);Ȃ(false);ǎ=ć(Ѐ,ύ,false);з(Γβ(ǎ,0),0);л(Ζ.θ,Ζ.ι,Ζ.Ѓ,false);Ж=1;ђ=0;т=0;ΒΞ=0;ΔЄ=-1;Ţ=н;Δώ=true;}if(!ΓΣ()){Τ(Α.Ņ);return;}if(Ƶ(true))
{ΗІ=Ą("","ORE",Щ.ľ);if((г==Г.ΓΫ||г==Г.Αω||
г==Г.Γκ||г==Г.ΑЃ)&&Μ!=Κ.Ǫ)
Τ(Α.Ʊ);else if((г==Г.ΕЄ||г==Г.ΓΥ)&&Μ!=Κ.Ǫ)
Τ(Α.ΔΖ);else
Τ(Α.Ņ);return;}и=Vector3.Distance(δ,ǎ);if(и>ś){ś=и;Δώ=false;}if(Μ==Κ.Ǫ&&ΕА()==MyDetectedEntityType.SmallGrid)
т+=2;else т-=2;т=Math.Max(100,Math.Min(400,т));if(Ж>0&&Ж<т){if(и>ђ){if(т>150) ђ=и;else ђ=(float) Math.Ceiling(и);Ж=1;}else Ж++;}else{if(Ж>0){Λ="Ship stuck! Retrying";ђ=и;Ж=0;ň(false,true,0,ο*sensorRange);}з(Γβ(ǎ,Math.Max(0,ђ-ο)),Γε(false));if(в<=wpReachedDist/2){Ж=1;ђ=0;}
return;}ň(false,true,ο*sensorRange,0);Vector3 ΙЎ=ǎ+ǹ*и;bool Ηη=false;if(Vector3.Distance(ΙЎ,δ)>0.3f){Vector3 ΚΞ=ǎ+ǹ*(и+0.1f);з(ΚΞ,4);Ηη=true;}else{float Ǽ=Γε(true);Vector3 ΗΗ=Γβ(ǎ,Math.Max(н+1,и+1));ȁ(true,false,false,ΗΗ,ΗΗ-ǎ,Ǽ,Ǽ);}bool Δε=false;if(ћ==Ǖ.ΓΞ||ћ==Ǖ.ΓΝ)
{if(!Ηη){float ΕΘ=0;foreach(IMyTerminalBlock ȗ in ω)
ΕΘ+=ΘϏ(ȗ,"","",ћ==Ǖ.ΓΝ?new string[]{"STONE"}:null);if(ΕΘ>ΔЄ||и<н||Δώ){ΔΝ=0;ΒΞ=и;Ţ=(int)(Math.Max(Ţ,ΒΞ)+ο/2);}else{Δε=и-ΒΞ>2&&ΔΝ>=20;ΔΝ++;}ΔЄ=ΕΘ;}}else Δε=и>=Ţ;if(ė!=б){Τ(Α.Ƞ);и=0;return;}if(Δε){б++;Τ(Α.Ƞ);и=0;return;}}if(Ϊ==Α.Ƞ||Ϊ==Α.Ņ||Ϊ==Α.Ʊ)
{if(Е){ǎ=ć(Ѐ,ύ,false);л(Ζ.θ,Ζ.ι,Ζ.Ѓ,false);ǘ(ω,ƃ);Ȃ(false);ђ=Vector3.Distance(δ,ǎ);ň(false,true,0,ο*sensorRange);}з(ǎ,Γε(false));if(Vector3.Distance(δ,ǎ)>=ђ+5){ǘ(ω,false);Ȃ(true);Λ="Can´t return!";}if(в<wpReachedDist){if(Ϊ==Α.Ƞ&&ΒЃ) Τ(Α.ů);if(Ϊ==Α.Ƞ) Τ(Α.Ć);
if(Ϊ==Α.Ʊ) Τ(Α.Αύ);if(Ϊ==Α.Ņ){if(Ο.λ) Τ(Α.Ħ);else{Ђ();ł();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Α.И||Ϊ==Α.у){if(Ϊ==Α.у&&γ==Π.В&&Μ!=Κ.ή){if(!ΓΣ()||Ƶ(true)){Τ(Α.И);return;}}bool ΕΟ=false;bool ΓΒ=false;bool Ζό=false;float Ž=0;bool ΗЅ=false;ά έ=null;
if(Е){if(Ϊ==Α.И||Μ==Κ.ή){if(Μ!=Κ.ή){if(ċ(1)) ΓΚ();else{ǟ=null;Ȳ=0;ΑΒ=!ΓΤ(1);}}ά α=ы();ν=new ΓЁ();ν.η=α;ν.ǌ=followPathDock*ο;ν.Εξ=useDockDirectionDist*ο;ν.Ž=10;ν.ƣ.Add(α.Ε);Vector3 ż=new Vector3();if(ȝ(α,dockDist*ο,true,out ż))
ν.ƣ.Add(ż);else ν.ǌ*=1.5f;if(Μ==Κ.ή){if(α==Ο) ν.Βγ=Ζ.Ε;if(α==Ζ) ν.Βγ=Ο.Ε;ν.Δπ=dockDist*ο*1.1f;}}else if(Ϊ==Α.у){ν=new ΓЁ();ν.η=Ζ;ν.ǌ=followPathJob*ο;ν.Εξ=useJobDirectionDist*ο;ν.Ž=10;ν.Βγ=Ο.Ε;ν.Δπ=dockDist*ο*1.1f;ν.ƣ.Add(Ζ.Ε);if(γ==Π.э){if(!Ο.λ||Ψ.Count==0){Ђ();return;}
float ΚΥ=Vector3.Distance(Ψ.First().Ε,Ο.Ε);float ΚΦ=Vector3.Distance(Ψ.Last().Ε,Ο.Ε);if(ΚΥ<ΚΦ) ν.η=Ψ.Last();else ν.η=Ψ.First();}}Αά=new Vector3();ΗЅ=!ΙΟ(δ);ǘ(ω,false);Ȃ(true);к=-1;double ȇ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ν.Βγ)<=ν.Δπ) continue;
double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(ȇ==-1||ϊ<ȇ){к=i;ȇ=ϊ;}}ȑ=ΘБ(ν.η.Ε,к);ō=null;}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&!ƾ()){ά ΔΣ=ы();ΑΒ=!ΓΤ(1);if(ы()!=ΔΣ){Τ(Α.И);return;}}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&ċ(1)&&ΓΚ()){Τ(Α.И);return;}bool Ηε=false;if(Ϊ==Α.И&&!Е&&к>=0&&к<Ψ.Count)
{Vector3 ǋ=Ψ[к].Ε+ΖΒ(к,ȑ);int ΕΖ=к+ȑ;Vector3 ŝ=ȑ!=0&&ΕΖ>=0&&ΕΖ<Ψ.Count?Ψ[ΕΖ].Ε:ν.η.Ε;if(ΙΚ(ǋ,ŝ)){if(!Βά){Βά=true;ΗΜ=ų(ǋ)?δ:ǋ;}з(ΗΜ,0);л(ν.η.θ,ν.η.ι,ν.η.Ѓ,false);Λ="Waiting for dock clearance";return;}Βά=false;Ηε=ċ(1)&&(ų(δ)||ų(ŝ));}Θρ(Ψ,ȑ,ν.ƣ,ν.ǌ,Е,ref Ж);
for(int i=0;i<ν.ƣ.Count;i++){float ϊ=Vector3.Distance(δ,ν.ƣ[i]);if(ϊ<=ν.ǌ) ΕΟ=true;if(ϊ<=ν.Εξ) ΓΒ=true;}if(Ηε) ΓΒ=true;if(ΓΒ) Ž=ν.Ž;float Λμ=ō!=null?ō.Ч:Ĝ;float Λα=(float) Math.Max(Ĝ*0.1f*ο,wpReachedDist);if((в<Λα)||Е){if(!Е) к+=ȑ;if(ȑ==0||к>Ψ.Count-1||к<0)
ΕΟ=true;else{Ερ=ȑ>0?Ψ.Count-1-к:к;έ=Ψ[к];ō=έ;if(к>=1&&к<Ψ.Count-1)
Αά=έ.Ε-Ψ[к-ȑ].Ε;else ō=null;Ё=έ.Ε+ΖΒ(к,ȑ);Ζό=true;}}if(ΓΒ)
л(ν.η.θ,ν.η.ι,ν.η.Ѓ,false);else if(ΗЅ)
ΓΙ(ν.η.θ,10,true);else if(Ζό&&έ!=null){if(ȑ>0) л(έ.θ,έ.ι,έ.Ѓ,90,false);else л(έ.θ,-έ.ι,-έ.Ѓ,90,false);}ȁ(true,false,true,Ё,Αά,ō==null?0:ō.Ч,Ž);if(ΕΟ){Ερ=0;if(Ϊ==Α.И&&!Θτ()){з(Ё,0);return;}if(Ϊ==Α.И&&ΑΒ){з(Ё,0);Λ="Waiting for a free dock";return;}
if(Ϊ==Α.И||Μ==Κ.ή){Τ(Α.Ȁ);return;}if(Ϊ==Α.у&&ΒЃ){Τ(Α.ů);return;}if(Ϊ==Α.у){Τ(Α.Ć);return;}}}if(Ϊ==Α.Ȁ||Ϊ==Α.ƻ){if(Μ!=Κ.ή&&ƾ()){if(!ċ(1)){Τ(Α.И);return;}}else if(Μ!=Κ.ή){ά ΔΣ=ы();if(!ΓΤ(2)||ы()!=ΔΣ){Τ(Α.И);return;}}ά α=ы();if(Е){if(!ȝ(α,dockDist*ο,true,out Ё))
{ȡ(х.Ư);Ђ();return;}з(Ё,0);ΓΙ(α.θ,90,true);}if(в<followPathDock*ο&&в!=-1){з(Ё,10);л(α.θ,α.ι,α.Ѓ,false);}if(ж(MyShipConnectorStatus.Connectable)!=null||ж(MyShipConnectorStatus.Connected)!=null){Τ(Α.Ł);return;}if(в<wpReachedDist/2&&в!=-1){Τ(Α.Ľ);return;}}if(Ϊ==Α.Ľ||Ϊ==Α.Ģ)
{if(Е){if(Ϊ==Α.Ľ){ά α=ы();if(!ȝ(α,dockDist*ο,true,out Ё)){ȡ(х.Ư);Ђ();return;}ȁ(true,true,false,Ё,0);л(α.θ,α.ι,α.Ѓ,10,false);}if(Ϊ==Α.Ģ){л(Ζ.θ,Ζ.ι,Ζ.Ѓ,0.5f,false);Ё=Ζ.Ε;ȁ(true,true,false,Ё,0);}}if(Βμ){ΒΒ(false,0,0,0,0);if(Ϊ==Α.Ľ) Τ(Α.Ł);if(Ϊ==Α.Ģ) Ђ();return;
}}if(Ϊ==Α.Ł){if(ж(MyShipConnectorStatus.Connected)!=null){if(Μ==Κ.ή) Τ(Α.ĩ);else Τ(Α.Ȥ);return;}ά α=ы();if(Е){т=0;ΚΧ=DateTime.Now;Ж=0;л(α.θ,α.ι,α.Ѓ,false);}Vector3I Δυ=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock ΖЎ=Me.CubeGrid.GetCubeBlock(Δυ);float ΗΟ=dockingSpeed;
float Κο=dockingSpeed*5;float Ζϊ=Math.Max(1.5f,Math.Min(5f,ο*0.15f));if(!ȝ(α,0,true,out Ё)||!ȝ(α,Ζϊ,true,out Αά)||
ΖЎ==null||!ΖЎ.FatBlock.IsFunctional){ȡ(х.Ư);Ђ();return;}if(т==1||(Vector3.Distance(δ,Ё)<=Ζϊ*1.1f&&!Е))
т=1;else{Vector3 ΛΠ=ф(Ϋ,Αά-δ);Vector3 Δτ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΚЉ=Αφ(ΛΠ,Δτ,null);ΗΟ=Math.Min(Κο,ΚЉ);}ȁ(true,false,false,Ё,Ё-δ,dockingSpeed,ΗΟ);if(Е) ђ=(float) в;IMyShipConnector Δΰ=ж(MyShipConnectorStatus.Connectable);if(Δΰ!=null){ȁ(false,false,false,Ё,0);
if(Ж>0) Ж=0;Ж--;if(Ж<-5){Δΰ.Connect();if(Δΰ.Status==MyShipConnectorStatus.Connected){if(Μ==Κ.ή) Τ(Α.ĩ);else Τ(Α.Ȥ);ΔΑ();Ƽ(Ȱ,true);return;}}}else{float ΗΛ=(float) Math.Round(в,1);if(ΗΛ<ђ){Ж=-1;ђ=ΗΛ;}else Ж++;if(Ж>20){Τ(Α.ƻ);return;}}}if(Ϊ==Α.Ȥ||Ϊ==Α.ĩ||
Ϊ==Α.ΒΙ||Ϊ==Α.Αϋ||Ϊ==Α.ŀ){bool ΔΛ=false,ΔΜ=false;if(Μ==Κ.ή){if(ы()==Ο) ΔΛ=true;else if(ы()==Ζ) ΔΜ=true;}if(Е){Αα=false;if(ж(MyShipConnectorStatus.Connected)==null){Τ(Α.ǭ);return;}ΔΑ();if(ΔΛ) ΒΖ(Ї.ǲ);if(ΔΜ) ΒΖ(Ј.ǲ);Ŧ=false;Č=false;ē=false;Αμ=false;}if(ж(MyShipConnectorStatus.Connected)==null){Ђ();ȡ(х.ΖΠ);return;}
if(γ!=Π.В||џ==-1||Ȅ==ї.None) Č=true;else if(Ȧ>=100f) Č=true;else if(Ȧ<=99f) Č=false;if(γ!=Π.В||Ǎ==-1||ǜ.Count==0) ē=true;else if(Ĕ>=100f) ē=true;else if(Ĕ<=99) ē=false;if(γ!=Π.В||п==-1||ķ.Count==0) Αμ=true;else Αμ=ŧ>=п;Ź і=null;if(ΔΛ) і=Ї;
if(ΔΜ) і=Ј;if(і!=null&&(і.Р==ρ.ΑЍ||і.Р==ρ.ΑЌ)) Č=true;if(і!=null&&(і.Р==ρ.Γω)) if(!Ŧ) Č=false;if(і!=null&&(і.Р==ρ.Γύ||і.Р==ρ.Γϋ)) ē=true;if(і!=null&&(і.Р==ρ.Γό)) if(!Ŧ) ē=false;if(ĕ){ChargeMode ΖІ=Č?ChargeMode.Auto:ChargeMode.Recharge;if(і!=null&&(і.Р==ρ.ΑЌ||і.Р==ρ.ΑЍ))
ΖІ=ChargeMode.Discharge;ΓЉ(ΖІ);ΓЋ(!ē);}if(!Ŧ){if(Μ==Κ.ή)
Ŧ=γ!=Π.В||Θυ(Е,true)||Ακ;else
Ŧ=γ!=Π.В||ΖΡ();}else{if(!Č) Τ(Α.ŀ);if(!ē) Τ(Α.Αϋ);if(!Αμ) Τ(Α.ΒΙ);Е=false;}if(Ŧ&&Č&&ē&&Αμ){ΓЉ(ChargeMode.Auto);ΓЋ(false);if(γ==Π.В){if(Μ==Κ.ή){if(ы()==Ο) ΒΖ(Ї.Ƕ);else if(ы()==Ζ) ΒΖ(Ј.Ƕ);if(ы()==Ο) ŷ(Ζ,Π.ū);else ŷ(Ο,Π.Ū);}}Τ(Α.ǭ);return;}}if(Ϊ==Α.ǭ){if(Αο){if(!ΕЂ()) return;
Αο=false;Τ(Α.ǭ);return;}if(Е){if(!ΕЂ()){Αο=true;return;}IMyShipConnector ȩ=ж(MyShipConnectorStatus.Connected);if(ȩ==null){Τ(Α.у);return;}IMyShipConnector ζ=ȩ.OtherConnector;Ζδ(ȩ,false);Ƽ(Ȱ,false);ά α=null;ά ќ=Μ==Κ.ή?Ο:ы();if(Vector3.Distance(ȩ.GetPosition(),ќ.Ε)<5f&&Ο.λ) α=ќ;
if(Vector3.Distance(ȩ.GetPosition(),Ζ.Ε)<5f&&Ζ.λ) α=Ζ;if(α!=null){if(!ȝ(α,dockDist*ο,true,out Ё)){ȡ(х.Ư);Ђ();return;}з(Ё,5);л(α.θ,α.ι,α.Ѓ,false);}else з(δ+ζ.WorldMatrix.Forward*dockDist*ο,5);if(γ==Π.В) ȡ(х.ΑБ);}if(в<wpReachedDist){ǘ(њ,true);Τ(Α.у);return;}}if(Ϊ==Α.ů)
{if(Е){Ȃ(true);ǘ(ω,false);Ё=Ζ.Ε;з(Ё,20);л(Ζ.θ,Ζ.ι,Ζ.Ѓ,false);}if(в<wpReachedDist/2){Τ(Α.Ģ);return;}}if(Ϊ==Α.Ħ){if(Е){Ȃ(true);ǘ(ω,false);int Ы=-1;double ȇ=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(ȇ==-1||ϊ<ȇ){Ы=i;ȇ=ϊ;}}
if(Ы==-1){Τ(Α.И);return;}Ё=Ψ[Ы].Ε;з(Ё,10);л(Ζ.θ,Ζ.ι,Ζ.Ѓ,false);}if(в<wpReachedDist){Τ(Α.И);return;}}if(Ϊ==Α.Αύ){bool Ϗ=false;if(Е){Ȃ(true);if((г==Г.ΓΫ||г==Г.Αω)&&ΑЅ()&&
ŭ(ǹ,Ϋ.GetNaturalGravity())<25&&Ќ>=2&&Ю>=2){Vector3 ΑΔ=δ;if(Ѐ>0&&ύ<Ю-1) ΑΔ=ć(Ѐ-1,ύ+1,true);else if(Ѐ<Ќ-1&&ύ<Ю-1) ΑΔ=ć(Ѐ+1,ύ+1,true);else if(Ѐ<Ќ-1&&ύ>0) ΑΔ=ć(Ѐ+1,ύ-1,true);else if(Ѐ>0&&ύ>0) ΑΔ=ć(Ѐ-1,ύ-1,true);else Ϗ=true;
if(!Ϗ) з(ΑΔ,10);}else Ϗ=true;}if(в<wpReachedDist/2) Ϗ=true;if(Ϗ){Τ(Α.ΒΘ);return;}}if(Ϊ==Α.ΔΖ||Ϊ==Α.ΒΘ){if(Е){ȁ(true,true,false,δ,0);ǘ(ω,false);Ȃ(true);Ж=-1;т=г==Г.Γκ||г==Г.ΑЃ?0:-1;}bool ő=!ΓΣ();int ΒЊ=Ą("STONE","ORE",Щ.ľ);if(г==Г.ΓΥ||г==Г.ΑЃ||г==Г.Αω)
ΒЊ+=Ą("ICE","ORE",Щ.ľ);bool Κί=ΒЊ>0;bool Δξ=false;if(т>=0){float ź=(float) Math.Sin(ΕІ(т))*ș/3f;float ΙА=(float) Math.Cos(ΕІ(т))*Ș/3f;Vector3 ΚΖ=ć(Ѐ,ύ,true)+ΔΔ(ǹ,ę*-1,new Vector3(ź,ΙА,0));з(ΚΖ,0.3f);if(в<Math.Min(ș,Ș)/10f) т+=5f;if(т>=360) т=0;}if(Ж==-1||ΒЊ<Ж){Ж=ΒЊ;ђ=0;}
else{ђ++;if(ђ>50) Δξ=true;}if(!Κί||ő||Δξ){if(!ő){int ΛΦ=Ą("","ORE",Щ.ľ);if(Ƶ(true)) ő=true;else if(100-(Й(ΛΦ,ΗІ)*100)<minEjection) ő=true;else ȡ(х.ΑБ);}if(Δξ&&ő) Λ="Ejection failed";if(Ϊ==Α.ΒΘ){if(ő){if(Ο.λ) Τ(Α.Ħ);else{Ђ();ł();Λ="Can´t return, no dock found";}}
else Τ(Α.Ć);}else if(ő) Τ(Α.Ņ);else Τ(Α.Ŵ);return;}}Е=false;}void Θρ(List<ά>ļ,int ς,List<Vector3>Αη,float Ȕ,bool ǻ,ref int ǰ){if(ǻ){for(int i=0;i<ļ.Count;i++) ļ[i].Ч=0;ǰ=-1;return;}if(ς==0) return;int Ǥ=ς*-1;if(ǰ==-1) ǰ=Ǥ>0?1:ļ.Count-2;int ΑΟ=0;while(ǰ>=1&&ǰ<ļ.Count-1)
{if(ΑΟ>50) return;ΑΟ++;try{if((Ǥ<0&&ǰ>=1)||(Ǥ>0&&ǰ<=ļ.Count-2)){ά ξ=ļ[ǰ];bool ΗЂ=false;for(int j=0;j<Αη.Count;j++)
if(Vector3.Distance(ξ.Ε,Αη[j])<=Ȕ){ΗЂ=true;break;}if(!ΗЂ){ά ΒϏ=ļ[ǰ-Ǥ];ά ŝ=ļ[ǰ+Ǥ];Vector3 ΛΡ=ξ.Ε-ŝ.Ε;Vector3 Εη=ΒϏ.Ε-ξ.Ε;Vector3 ΚЃ=ξ.Ε+Vector3.Normalize(ΛΡ)*Εη.Length();Vector3 ΚΤ=ΒϏ.Ε-ΚЃ;Vector3 Βς=Ň(ς>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ΚΤ);Vector3 Κξ=Ň(ς>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,Εη);
Vector3 Βτ=Ň(ς>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ξ.φ);ξ.Ч=(float) Math.Sqrt(Math.Pow(ΒϏ.Ч,2)+Math.Pow(Αφ(-Κξ,Βτ,ξ),2));for(int j=0;j<Αη.Count;j++)
if(Vector3.Distance(ΒϏ.Ε,Αη[j])<=Ȕ){Vector3 Βυ=Ň(ς>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,Αη[j]-ξ.Ε);float ΛΗ=Αφ(-Βυ,Βτ,ξ);ξ.Ч=Math.Min(ξ.Ч,ΛΗ)/2f;}if(Βς.Length()==0) Βς=new Vector3(0,0,1);Vector3 Κν=Ň(ξ.ι,ξ.θ*-1,ξ.φ);float ΛΛ=Ʋ(-Βς,Κν,ξ);float ΒΚ=Й(ΛΛ,Ť);float Εδ=(float) Math.Sqrt(Й(Βς.Length()*1.0f,0.5f*ΒΚ));
ξ.Ч=Math.Min(ξ.Ч,(Εη.Length()/Εδ)*ȃ);}}}catch{return;}ǰ+=Ǥ;}ǰ=-1;}void ΔΕ(bool ǻ){if(ǻ){ΑΠ=0;return;}if(Μ==Κ.ή) return;float Ϗ=б*Math.Max(1,н);if(ė==б) Ϗ+=Math.Min(н,и);float ΛΥ=Ќ*Ю*Math.Max(1,н);ΑΠ=Math.Max(ΑΠ,(float) Math.Min(Ϗ/ΛΥ*100.0,100));}MyDetectedEntityType ΕА()
{try{if(Ȟ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γε(bool Ȍ){if(Μ==Κ.Ǫ&&ΕА()==MyDetectedEntityType.None&&!ƴ(ψ,true))
return fastSpeed;else return Ȍ?ƭ:Ƭ;}void Ιω(){float Ηϊ=0,ΗΕ=0,Ŗ=0,ΕΙ=0;for(int i=0;i<Ī.Count;i++){IMyBatteryBlock Ǉ=Ī[i];if(!Ȟ(Ǉ,true)) continue;Ηϊ+=Ǉ.MaxStoredPower;ΗΕ+=Ǉ.CurrentStoredPower;Ŗ+=Ǉ.CurrentInput;ΕΙ+=Ǉ.CurrentOutput;}Ȧ=(float) Math.Round(Й(ΗΕ,Ηϊ)*100,1);
if(Ŗ>=ΕΙ) Ȅ=ї.ŀ;else Ȅ=ї.ΕЇ;if(Ŗ==0&&ΕΙ==0||Ȧ==100.0) Ȅ=ї.ь;if(Ī.Count==0) Ȅ=ї.None;}void ΙϏ(){float ΗΥ=0;for(int i=0;i<ǜ.Count;i++){IMyGasTank ΘΣ=ǜ[i];if(!Ȟ(ΘΣ,true)) continue;ΗΥ+=(float) ΘΣ.FilledRatio;}Ĕ=Й(ΗΥ,ǜ.Count)*100f;}void ΙЊ(){ŧ=0;try{for(int i=0;i<ķ.Count;i++)
{IMyReactor ΕΠ=ķ[i];if(!Ȟ(ΕΠ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΠ.GetInventory(0).GetItems(Ћ);float Њ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem ώ=Ћ[j];if(ώ.Type.SubtypeId.ToUpper()=="URANIUM"&&ώ.Type.TypeId.ToUpper().Contains("_INGOT"))
Њ+=(float) ώ.Amount;}if(Њ<ŧ||i==0){ŧ=Њ;ΔЌ=ΕΠ.CustomName;}}}catch(Exception e){Ė=e;}}void Ιϊ(){Ȑ=0;ȉ=0;try{for(int i=0;i<Ɓ.Count;i++){IMyTerminalBlock Ω=Ɓ[i];if(!Ȟ(Ω,true)) continue;ȉ+=(float) Ω.GetInventory(0).CurrentVolume;Ȑ+=(float) Ω.GetInventory(0).MaxVolume;
}Βρ=(float) Math.Min(Math.Round(Й(ȉ,Ȑ)*100,1),100.0);}catch(Exception e){Ė=e;}}void ΙЀ(){try{ǵ.Clear();for(int i=0;i<Ɓ.Count;i++){IMyTerminalBlock Ω=Ɓ[i];if(!Ȟ(Ω,true)) continue;Εψ(Ω,Щ.Ų);}if(г!=Г.Γψ){for(int i=0;i<ŋ.Count;i++){IMyTerminalBlock Ω=ŋ[i];if(!Ȟ(Ω,true)) continue;
Εψ(Ω,Щ.ΙΝ);}}Ιπ(ǵ);}catch(Exception e){Ė=e;}}void Θσ(){if(ΒΩ){if(ΕЏ().Count>Βκ){ΒΩ=false;if(ѓ!=ъ.Ƴ){Ђ();if(ѓ==ъ.Γΰ) ł();if(ѓ==ъ.ΑА)
if(Ο.λ) Αώ();else ł();}Λ="Damage detected";}}}bool ΓΣ(){if(!Βλ) return true;if(γ==Π.В){if(џ>0&&Ȅ!=ї.None)
if(Ȧ<=џ){Λ="Low energy! Move home";return false;}if(п>0&&ķ.Count>0)
if(ŧ<=п){Λ="Low fuel: "+ΔЌ;return false;}if(Ǎ>0&&ǜ.Count>0)
if(Ĕ<=Ǎ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕЏ(){List<IMyTerminalBlock>Ŏ=new List<IMyTerminalBlock>();for(int i=0;i<ǂ.Count;i++){IMyTerminalBlock Ω=ǂ[i];if(ƴ(Ω,false)) Ŏ.Add(Ω);}return Ŏ;}bool ƴ(IMyTerminalBlock Ω,bool Εή){return(!Ȟ(Ω,Εή)||!Ω.IsFunctional);
}bool Ȟ(IMyTerminalBlock Ω,bool Εή){if(Ω==null) return false;try{IMyCubeBlock ΗΫ=Me.CubeGrid.GetCubeBlock(Ω.Position).FatBlock;if(Εή) return ΗΫ==Ω;else return ΗΫ.GetType()==Ω.GetType();}catch{return false;}}float ΘϏ(IMyTerminalBlock Ω,String Ǔ,String ΘΙ,String[]Ȋ){float Ȫ=0;for(int i=0;i<Ω.InventoryCount;i++)
{IMyInventory ƈ=Ω.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ƈ.GetItems(Ћ);foreach(MyInventoryItem ώ in Ћ){if(Ȋ!=null&&(Ȋ.Contains(ώ.Type.TypeId.ToUpper())||Ȋ.Contains(ώ.Type.SubtypeId.ToUpper())))
continue;if((Ǔ==""||ώ.Type.TypeId.ToUpper()==Ǔ)&&(ΘΙ==""||ώ.Type.SubtypeId.ToUpper()==ΘΙ))
Ȫ+=(float) ώ.Amount;}}return Ȫ;}м ΘЇ(String ΰ,String υ,Щ Я,bool ΚΟ){ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǵ.Count;i++){м ώ=ǵ[i];if(ώ.ΰ.ToUpper()==ΰ&&ώ.υ.ToUpper()==υ&&(ώ.ƌ==Я||Я==Щ.ľ))
return ώ;}м ί=null;if(ΚΟ){ί=new м(ΰ,υ,0,Я);ǵ.Add(ί);}return ί;}int Ą(String ΰ,String υ,Щ Я){return Ą(ΰ,υ,Я,null);}int Ą(String ΰ,String υ,Щ Я,String[]Ȋ){int Ȫ=0;ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǵ.Count;i++){м ώ=ǵ[i];if(Ȋ!=null&&Ȋ.Contains(ώ.ΰ.ToUpper())) continue;
if((ΰ==""||ώ.ΰ.ToUpper()==ΰ)&&(υ==""||ώ.υ.ToUpper()==υ)&&(ώ.ƌ==Я||Я==Щ.ľ))
Ȫ+=ώ.Њ;}return Ȫ;}void Εψ(IMyTerminalBlock Ω,Щ Я){for(int i=0;i<Ω.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ω.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
ΘЇ(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Я,true).Њ+=(int) Ћ[j].Amount;}}void Ιπ(List<м>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){м a=Ѝ[j];м b=Ѝ[j+1];if(a.Њ<b.Њ) Ѝ.Move(j,j+1);}}bool Ƶ(bool Λι){if(ġ&&Μ!=Κ.ή)
if(д!=-1&&Ť>=д){Λ="Ship too heavy";return true;}if(Βρ>=ȏ||Αα){Αα=false;Λ="Ship is full";return true;}return false;}bool ΖΡ(){String[]Ȋ=null;if(!Ġ) Ȋ=new string[]{"ICE"};if(Μ==Κ.ƹ) return Ą("","ORE",Щ.Ų,Ȋ)==0;if(Μ==Κ.Ǫ) return Ą("","COMPONENT",Щ.Ų,Ȋ)==0;else return Ą("","",Щ.Ų,Ȋ)==0;
}bool ΙΠ(){List<IMyCockpit>Δί=new List<IMyCockpit>();χ.GetBlocksOfType(Δί,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δί.Count;i++)
if(Δί[i].IsUnderControl) return true;return false;}bool Θυ(bool ǻ,bool ΒЅ){IMyShipConnector О=ж(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return ΕЃ(Ї,ǻ,ΒЅ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return ΕЃ(Ј,ǻ,ΒЅ);return false;}bool ΕЃ(Ź Ȩ,bool ǻ,bool ΒЅ){if(ǻ) Ȩ.Ιΰ();Ȩ.ΙІ();bool ѕ=false;String Ǒ="";switch(Ȩ.Р){case ρ.Γϊ:{Ǒ="Waiting for command";ѕ=false;break;}case ρ.Ζή:{Ǒ="Waiting for cargo";ѕ=Ƶ(true);break;}case ρ.Ζέ:{Ǒ="Unloading";ѕ=ΖΡ();break;}case ρ.ΓϏ:{ѕ=true;break;}
case ρ.Γω:{Ǒ="Charging batteries";ѕ=Ȧ>=100f;break;}case ρ.ΑЍ:{Ǒ="Discharging batteries";ѕ=Ȧ<=25f;break;}case ρ.ΑЌ:{Ǒ="Discharging batteries";ѕ=Ȧ<=0f;break;}case ρ.Γό:{Ǒ="Filling up hydrogen";ѕ=Ĕ>=100f;break;}case ρ.Γύ:{Ǒ="Unloading hydrogen";ѕ=Ĕ<=25f;break;}
case ρ.Γϋ:{Ǒ="Unloading hydrogen";ѕ=Ĕ<=0f;break;}case ρ.Γώ:{bool Ηδ=ΙΠ();if(!Ηδ) Ȩ.ΕΜ=true;ѕ=Ȩ.ΕΜ&&Ηδ;Ǒ="Waiting for passengers";break;}}if(!ѕ) Ȩ.ΓΡ(true);if(ѕ&&Ȩ.Γη()){ѕ=Ȩ.ΓΡ(false);Ǒ="Undocking in: "+ΑϏ((int) Math.Max(0,Ȩ.Ǳ-Ȩ.İ));}if(ΒЅ) Θγ=Ǒ;return ѕ;}void Θι()
{if(!Ɖ) return;if(ω.Count<=1) return;float ΘΫ=0,ΘΪ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȗ=ω[i];if(ƴ(ȗ,true)) continue;ΘΫ+=(float) ȗ.GetInventory(0).MaxVolume;ΘΪ+=(float) ȗ.GetInventory(0).CurrentVolume;}float Ζύ=(float) Math.Round(Й(ΘΪ,ΘΫ),5);for(int ş=0;ş<Math.Max(1,Math.Floor(ω.Count/10f));ş++)
{float Δό=0,ΔЋ=0;float Ηα=0,Ηχ=0;IMyTerminalBlock Đ=null,ı=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȗ=ω[i];if(ƴ(ȗ,true)) continue;float ΕΑ=(float) ȗ.GetInventory(0).MaxVolume;float Βδ=Й((float) ȗ.GetInventory(0).CurrentVolume,ΕΑ);if(Đ==null||Βδ>Δό){Đ=ȗ;Δό=Βδ;Ηα=ΕΑ;}
if(ı==null||Βδ<ΔЋ){ı=ȗ;ΔЋ=Βδ;Ηχ=ΕΑ;}}if(Đ==null||ı==null||Đ==ı) return;if(checkConveyorSystem&&!Đ.GetInventory(0).IsConnectedTo(ı.GetInventory(0))){if(ΔΡ>20) Λ="Inventory balancing failed";else ΔΡ++;return;}ΔΡ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
Đ.GetInventory(0).GetItems(Ћ);float Αξ=0;if(Ћ.Count==0) continue;MyInventoryItem Βε=Ћ[0];String Ηθ=Βε.Type.TypeId+Βε.Type.SubtypeId;if(!Θβ.TryGetValue(Ηθ,out Αξ)){if(ΙΦ(Đ.GetInventory(0),0,ı.GetInventory(0),out Αξ))
Θβ.Add(Ηθ,Αξ);else return;}float ΛΣ=((Δό-Ζύ)*Ηα/Αξ);float ΛΤ=((Ζύ-ΔЋ)*Ηχ/Αξ);int Εμ=(int) Math.Min(ΛΤ,ΛΣ);if(Εμ<=0) return;if((float) Βε.Amount<Εμ)
Đ.GetInventory(0).TransferItemTo(ı.GetInventory(0),0,null,null,Βε.Amount);else
Đ.GetInventory(0).TransferItemTo(ı.GetInventory(0),0,null,null,Εμ);}}bool ΙΦ(IMyInventory Αβ,int Β,IMyInventory ΚΣ,out float Θα){Θα=0;float ΚΓ=(float) Αβ.CurrentVolume;List<MyInventoryItem>ΔΤ=new List<MyInventoryItem>();Αβ.GetItems(ΔΤ);float ΖЁ=0;for(int i=0;i<ΔΤ.Count;i++) ΖЁ+=(float) ΔΤ[i].Amount;
Αβ.TransferItemTo(ΚΣ,Β,null,null,1);float Θί=ΚΓ-(float) Αβ.CurrentVolume;List<MyInventoryItem>ΔΘ=new List<MyInventoryItem>();Αβ.GetItems(ΔΘ);float Ζφ=0;for(int i=0;i<ΔΘ.Count;i++) Ζφ+=(float) ΔΘ[i].Amount;if(Θί==0f||!Γλ(0.9999,ΖЁ-Ζφ,1.0001))
return false;Θα=Θί;return true;}void Ιώ(){Vector3 ΑΘ=Ё+ĺ;Vector3 Ȗ=ΑΘ-δ;if(Ȗ.Length()==0) Ȗ=new Vector3(0,0,-1);Vector3 Βυ=ф(Ϋ,Ȗ);Vector3 ΘΤ=Vector3.Normalize(Βυ);Vector3 Ƌ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΛΒ=ΕΚ>0?Math.Max(0,1-ŭ(Ȗ,ΗΧ)/5):0;float Ч=(float) Math.Min((ΔЏ>0?ΔЏ:1000f),Math.Max(Αφ(-Βυ,Ƌ,null),ΕΚ*ΛΒ));
if(!ΘΩ) Ч=0;if(ΘΓ) Ч=Math.Max(0,1-ΒΧ/ΒΛ)*Ч;if(generalSpeedLimit>0) Ч=Math.Min(generalSpeedLimit,Ч);if(Ɣ) Ч=0;if(ǒ>=0) Ч=Math.Min(Ч,ǒ);if(ΘΔ) Ч*=(float) Math.Min(1,Й(Ȗ.Length(),wpReachedDist)/2);Vector3 Ητ=ф(Ϋ,Ϋ.GetShipVelocities().LinearVelocity);float ΙЏ=(float)(Math.Max(0,15-ŭ(-ΘΤ,-Ητ))/15)*0.85f+0.15f;
Εβ+=Math.Sign(ΙЏ-Εβ)/10f;Vector3 Ĭ=ΘΤ*Ч*Εβ-(Ητ);Vector3 ΔΟ=Γδ(Ĭ,null);if(ΒЍ&&в>0.1f){Ĭ.X*=ΓЍ(Ĭ.X,ref ΒЎ.X,1f,ΔΟ.X,20);Ĭ.Y*=ΓЍ(Ĭ.Y,ref ΒЎ.Y,1f,ΔΟ.Y,20);Ĭ.Z*=ΓЍ(Ĭ.Z,ref ΒЎ.Z,1f,ΔΟ.Z,20);}else ΒЎ=new Vector3(1,1,1);Αθ=Ť*Ĭ-Ƌ*Ť;Εω(Αθ,ΒЍ);в=Vector3.Distance(δ,ΑΘ);
}float ΓЍ(float value,ref float Є,float Ǥ,float Ę,float ΔА){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Є)) Є+=Math.Sign(Є)*Ǥ;else Є=value;if(value==0) Є=1;float ί=Math.Abs(Є);if(ί<ΔА||Ę==0) return ί;Є=Math.Min(ΔА,Math.Max(-ΔА,Є));ί=Math.Abs(Ę);return ί;
}void Ζτ(){float ǐ=90,Ǩ=90,Ǣ=90;float Ǽ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȍ,Ǟ,ΑΦ,ΔЈ;if(ΔЉ){Ȍ=Vector3.Normalize(Ё-δ);ΑΦ=ф(Ϋ,Ȍ);Ǟ=ф(Ϋ,Εΰ);ǐ=ŭ(ΑΦ,new Vector3(0,-1,0))-90;Ǩ=Ŀ(Ǟ,new Vector3(-1,0,0),Ǟ.Y);
Ǣ=Ŀ(ΑΦ,new Vector3(-1,0,0),ΑΦ.Z);}else{Ȍ=Εα;Ǟ=ф(Ϋ,Εΰ);ΑΦ=ф(Ϋ,Εα);ΔЈ=ф(Ϋ,ΘΥ);ǐ=Ŀ(Ǟ,new Vector3(0,0,1),Ǟ.Y);Ǩ=Ŀ(Ǟ,new Vector3(-1,0,0),Ǟ.Y);Ǣ=Ŀ(ΔЈ,new Vector3(0,0,1),ΔЈ.X);}if(Ζψ&&ΑЅ()){Vector3 Δτ=Ϋ.GetNaturalGravity();Ǟ=ф(Ϋ,Δτ);ǐ=Ŀ(Ǟ,new Vector3(0,0,1),Ǟ.Y);
Ǩ=Ŀ(Ǟ,new Vector3(-1,0,0),Ǟ.Y);}if(!Γλ(-45,Ǩ,45)){ǐ=0;Ǣ=0;}if(!Γλ(-45,Ǣ,45)) ǐ=0;ΒΒ(Δφ,1,(-ǐ)*Ǽ,(-Ǣ)*Ǽ,(-Ǩ)*Ǽ);ΒΧ=Math.Max(Math.Abs(ǐ),Math.Max(Math.Abs(Ǩ),Math.Abs(Ǣ)));Βμ=ΒΧ<=ΒΛ;}void Θς(IMyTerminalBlock ΒЀ){if(ΒЀ==null) return;ǿ=new float[3,2];ǥ=new Dictionary<string,float[,]>();
for(int i=0;i<ā.Count;i++){IMyThrust ш=ā[i];if(!ш.IsFunctional) continue;Vector3 ς=ф(ΒЀ,ш.WorldMatrix.Backward);float į=ш.MaxEffectiveThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) ǿ[0,0]+=į;else ǿ[0,1]-=į;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) ǿ[1,0]+=į;else ǿ[1,1]-=į;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) ǿ[2,0]+=į;else ǿ[2,1]-=į;String Ǔ=ΖΖ(ш);float[,]Ǧ=null;if(ǥ.ContainsKey(Ǔ)) Ǧ=ǥ[Ǔ];else{Ǧ=new float[3,2];ǥ.Add(Ǔ,Ǧ);}float Ę=ш.MaxThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) Ǧ[0,0]+=Ę;else Ǧ[0,1]-=Ę;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) Ǧ[1,0]+=Ę;else Ǧ[1,1]-=Ę;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) Ǧ[2,0]+=Ę;else Ǧ[2,1]-=Ę;}}static String ΖΖ(IMyThrust ш){return ш.BlockDefinition.SubtypeId;}Vector3 Γρ(Vector3 ǚ,float[,]ƍ){return new Vector3(
ǚ.X>=0?ƍ[0,0]:ƍ[0,1],
ǚ.Y>=0?ƍ[1,0]:ƍ[1,1],
ǚ.Z>=0?ƍ[2,0]:ƍ[2,1]);}bool ΖΕ(ά έ,String Ǔ,out float Δν){Δν=0;int Β=Ā.IndexOf(Ǔ);if(Β==-1||έ.Ǿ==null||Β>=έ.Ǿ.Length)
return false;Δν=έ.Ǿ[Β];if(Δν==-1) return false;return true;}Vector3 Γδ(Vector3 ǚ,ά έ){if(έ!=null){Vector3 ί=new Vector3();for(int i=0;i<ǥ.Keys.Count;i++){String Ǔ=ǥ.Keys.ElementAt(i);float ΑΖ=0;if(!ΖΕ(έ,Ǔ,out ΑΖ))
return Γρ(ǚ,ǿ);ί+=Γρ(ǚ,ǥ.Values.ElementAt(i))*ΑΖ;}return ί;}return Γρ(ǚ,ǿ);}float Ʋ(Vector3 ǚ,ά έ){return Ʋ(ǚ,new Vector3(),έ);}float Ʋ(Vector3 ǚ,Vector3 φ,ά έ){Vector3 ΛΚ=Γδ(ǚ,έ);Vector3 į=ΛΚ+φ*Ť;float š=(į/ǚ).AbsMin();return(float)(ǚ*š).Length();}float Αφ(Vector3 Βσ,Vector3 Ƌ,ά έ)
{if(Βσ.Length()==0) return 0;float Ζχ=1;if(Ƌ.Length()>0) Ζχ=Math.Min(1,ŭ(-Ƌ,Βσ)/90)*0.4f+0.6f;float Αθ=Ʋ(Βσ,Ƌ,έ);if(Αθ==0) return 0.1f;float ΒΚ=Й(Αθ,Ť);float Εδ=(float) Math.Sqrt(Й(Βσ.Length(),ΒΚ*0.5f));return ΒΚ*Εδ*Ζχ*ȃ;}float ΑЂ(Vector3 Ȍ,Vector3 Б,Vector3 φ,ά έ)
{if(φ.Length()==0f) return 0;Vector3 Βτ=Ň(Ȍ,Б,Vector3.Normalize(φ));float Ǵ=Ʋ(-Βτ,έ);return Ǵ/φ.Length();}void Εώ(bool Κε){float ǧ=0;float Δρ=0.9f;if(Κε){д=-1;Ũ=0;ΒЇ=null;if(γ!=Π.э&&Ζ.φ.Length()!=0){ǧ=Δρ*ΑЂ(Ζ.ι,Ζ.θ*-1,Ζ.φ,null);if(ǧ<д||д==-1) д=ǧ;}if(Ο.λ&&Ο.φ.Length()!=0)
{ǧ=Δρ*ΑЂ(Ο.ι,Ο.θ*-1,Ο.φ,null);if(ǧ<д||д==-1) д=ǧ;}return;}if(Ũ==-1) return;if(Ũ>=0){int ΗЌ=0;while(Ũ<Ψ.Count){if(ΗЌ>100) return;ΗЌ++;ά έ=Ψ[Ũ];if(έ.φ.Length()!=0f){ǧ=Δρ*Math.Min(
ΑЂ(έ.ι,έ.θ*-1,έ.φ,έ),
ΑЂ(έ.ι*-1,έ.θ*-1,έ.φ,έ));if(ǧ<д||д==-1) д=ǧ;}else ΒЇ=έ;Ũ++;}Ũ=-1;}bool Δω=true;float Ķ=0;if(ΒЇ!=null){for(int i=0;i<ǥ.Count;i++){String Ǔ=ǥ.Keys.ElementAt(i);float[,]Ǧ=ǥ.Values.ElementAt(i);float ΑΖ=0;if(!ΖΕ(ΒЇ,Ǔ,out ΑΖ)){Δω=false;break;}for(int a=0;a<Ǧ.GetLength(0);a++)
for(int b=0;b<Ǧ.GetLength(1);b++){float Ŋ=Math.Abs(Ǧ[a,b]*ΑΖ);if(Ŋ==0) continue;Δω=true;if(Ķ==0||Ŋ<Ķ) Ķ=Ŋ;}}}if(!Δω){for(int a=0;a<ǿ.GetLength(0);a++)
for(int b=0;b<ǿ.GetLength(1);b++){float Ŋ=Math.Abs(ǿ[a,b]);if(Ŋ==0) continue;if(Ķ==0||Ŋ<Ķ) Ķ=Ŋ;}}if(Ķ>0){float Κρ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǧ=Й(Ķ,Κρ);if(ǧ>0)
if(ǧ<д||д==-1) д=ǧ;}}void Εω(Vector3 Ǵ,bool М){if(!М){for(int i=0;i<ā.Count;i++)
ā[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔΞ=Γδ(Ǵ,null);float Λγ=Math.Min(1,Math.Abs(Й(Ǵ.X,ΔΞ.X)));float Λδ=Math.Min(1,Math.Abs(Й(Ǵ.Y,ΔΞ.Y)));float Λε=Math.Min(1,Math.Abs(Й(Ǵ.Z,ΔΞ.Z)));for(int i=0;i<ā.Count;i++){IMyThrust ш=ā[i];Vector3 ς=Ιβ(ф(Ϋ,ш.WorldMatrix.Backward),1);
if(ς.X!=0&&Math.Sign(ς.X)==Math.Sign(Ǵ.X))
ш.SetValueFloat("Override",ш.MaxThrust*Λγ);else if(ς.Y!=0&&Math.Sign(ς.Y)==Math.Sign(Ǵ.Y))
ш.SetValueFloat("Override",ш.MaxThrust*Λδ);else if(ς.Z!=0&&Math.Sign(ς.Z)==Math.Sign(Ǵ.Z))
ш.SetValueFloat("Override",ш.MaxThrust*Λε);else
ш.SetValueFloat("Override",0.0f);}}void ΒΒ(bool М,float ΚЀ,float ǐ,float Ǣ,float Ǩ){for(int i=0;i<Βη.Count;i++){IMyGyro К=Βη[i];К.GyroOverride=М;if(!М) К.GyroPower=100;else К.GyroPower=ΚЀ;if(!М) continue;Vector3 Э=Ϋ.WorldMatrix.Forward;Vector3 р=Ϋ.WorldMatrix.Right;Vector3 Б=Ϋ.WorldMatrix.Up;
Vector3 Ƅ=К.WorldMatrix.Forward;Vector3 Ɔ=К.WorldMatrix.Up;Vector3 ƅ=К.WorldMatrix.Left*-1;if(Ƅ==Э) К.SetValueFloat("Roll",Ǩ);else if(Ƅ==(Э*-1)) К.SetValueFloat("Roll",Ǩ*-1);else if(Ɔ==(Э*-1)) К.SetValueFloat("Yaw",Ǩ);else if(Ɔ==Э) К.SetValueFloat("Yaw",Ǩ*-1);else if(ƅ==Э) К.SetValueFloat("Pitch",Ǩ);
else if(ƅ==(Э*-1)) К.SetValueFloat("Pitch",Ǩ*-1);if(ƅ==(р*-1)) К.SetValueFloat("Pitch",ǐ);else if(ƅ==р) К.SetValueFloat("Pitch",ǐ*-1);else if(Ɔ==р) К.SetValueFloat("Yaw",ǐ);else if(Ɔ==(р*-1)) К.SetValueFloat("Yaw",ǐ*-1);else if(Ƅ==(р*-1)) К.SetValueFloat("Roll",ǐ);
else if(Ƅ==р) К.SetValueFloat("Roll",ǐ*-1);if(Ɔ==(Б*-1)) К.SetValueFloat("Yaw",Ǣ);else if(Ɔ==Б) К.SetValueFloat("Yaw",Ǣ*-1);else if(ƅ==Б) К.SetValueFloat("Pitch",Ǣ);else if(ƅ==(Б*-1)) К.SetValueFloat("Pitch",Ǣ*-1);else if(Ƅ==Б) К.SetValueFloat("Roll",Ǣ);else if(Ƅ==(Б*-1)) К.SetValueFloat("Roll",Ǣ*-1);
}}void Ζξ(){this.Δφ=false;}void л(Vector3 Į,Vector3 Э,Vector3 ΔЇ,float ğ,bool Αν){ΓΙ(Į,ğ,Αν);ΒΛ=ğ;ΔЉ=false;this.Εα=Э;this.ΘΥ=ΔЇ;}void л(Vector3 Į,Vector3 Э,Vector3 ΔЇ,bool Αν){л(Į,Э,ΔЇ,2f,Αν);}void ΓΙ(Vector3 Į,float ğ,bool Αν){ΒΛ=ğ;this.Δφ=true;this.Ζψ=Αν;ΔЉ=true;
Βμ=false;this.Εΰ=Į;}void ΔΑ(){ȁ(false,false,false,Ё,0);ΒЍ=false;}void з(Vector3 η,float Ǽ){ȁ(true,false,false,η,Ǽ);}void ȁ(bool М,bool ΕΫ,bool Εο,Vector3 η,float Ǽ){ȁ(М,ΕΫ,Εο,η,η-δ,0.0f,Ǽ);}void ȁ(bool М,bool ΕΫ,bool Εο,Vector3 η,Vector3 ΚЍ,float ΚЎ,float Ǽ){ΒЍ=true;
this.ΘΩ=М;Ё=η;this.ΔЏ=Ǽ;this.ΕΚ=ΚЎ;this.ΘΔ=ΕΫ;this.ΘΓ=Εο;this.ΗΧ=ΚЍ;в=Vector3.Distance(η,δ);}void Ιδ(){χ.GetBlocksOfType(ǂ,ǖ);}void Ιε(){if(ΒЂ){А=2;return;}List<IMyRemoteControl>Αέ=new List<IMyRemoteControl>();List<IMySensorBlock>ΗБ=new List<IMySensorBlock>();List<IMyTerminalBlock>Αζ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αέ,ǖ);χ.GetBlocksOfType(Ц,ǖ);χ.GetBlocksOfType(ΗБ,ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αζ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=ǩ(Ц,pamTag,true);Ζζ();Ζη(Αζ);ņ(Ц,setLCDFontAndSize,1.4f,false);ņ(ȕ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘΟ=ǩ(ΗБ,pamTag,true);if(ΘΟ.Count>0) ψ=ΘΟ[0];else ψ=null;if(Μ==Κ.ƹ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Μ==Κ.Ǫ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Μ==Κ.ή){χ.GetBlocksOfType(Εε,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘΞ=ǩ(Αέ,pamTag,true);if(ΘΞ.Count>0) Αέ=ΘΞ;if(Αέ.Count>0) Ϋ=Αέ[0];else{Ϋ=null;А=2;Λ="Remote is missing";return;}ο=(float) Ϋ.CubeGrid.WorldVolume.Radius*2;Ǡ=null;if(Μ!=Κ.ή){ΕϏ();if(ω.Count>0&&Ǡ!=null){if(ψ!=null&&(Ǡ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ϋ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ϋ.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(Ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Forward&&
Ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void Ιζ(){χ.GetBlocksOfType(Ȱ,ǖ);χ.GetBlocksOfType(њ,ǖ);χ.GetBlocksOfType(ā,ǖ);χ.GetBlocksOfType(Βη,ǖ);χ.GetBlocksOfType(Ī,ǖ);χ.GetBlocksOfType(ķ,ǖ);χ.GetBlocksOfType(ǜ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΒІ,ǖ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
њ=ǩ(њ,"ConnectorMedium",false);else
њ=ǩ(њ,"Connector",false);List<IMyShipConnector>ΘΜ=ǩ(њ,pamTag,true);if(ΘΜ.Count>0) њ=ΘΜ;if(А<=1){if(њ.Count==0){А=1;Λ="Connector is missing";}if(Βη.Count==0){А=1;Λ="Gyros are missing";}if(ā.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΘΠ=ǩ(ΒІ,pamTag,true);
if(ΘΠ.Count>0) ΒІ=ΘΠ;List<IMyLandingGear>ΘΝ=ǩ(Ȱ,pamTag,true);if(ΘΝ.Count>0) Ȱ=ΘΝ;for(int i=0;i<Ȱ.Count;i++) Ȱ[i].AutoLock=false;List<IMyBatteryBlock>ΘΛ=ǩ(Ī,pamTag,true);if(ΘΛ.Count>0) Ī=ΘΛ;List<IMyGasTank>ΘΡ=ǩ(ǜ,pamTag,true);if(ΘΡ.Count>0) ǜ=ΘΡ;}void Ιη(){χ.GetBlocksOfType(ŋ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ɓ.Clear();for(int i=ŋ.Count-1;i>=0;i--){if(ΙΞ(ŋ[i])){Ɓ.Add(ŋ[i]);ŋ.RemoveAt(i);}}}bool ΙΞ(IMyTerminalBlock Ω){if(Ω is IMyCargoContainer) return true;if(Ω is IMyShipDrill) return true;if(Ω is IMyShipGrinder) return true;if(Ω is IMyShipConnector){if(((IMyShipConnector) Ω).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Γθ(Ω,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕϏ(){if(Ϋ==null) return;Ǡ=null;float Βϋ=0,Βφ=0,Βό=0,Βχ=0,Βύ=0,Βψ=0;List<IMyTerminalBlock>Εί=ǩ(ω,pamTag,true);bool Κχ=Εί.Count==0;if(Εί.Count>0) Ǡ=Εί[0];else if(ω.Count>0) Ǡ=ω[0];int Ȫ=0;float Ƙ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=Ǡ.WorldMatrix.Forward){if(Κχ){А=2;Λ="Mining direction is unclear!";return;}continue;}Ȫ++;Ƙ=Math.Max(Ƙ,Ιχ(ω[i]));Vector3 Н=ΑЁ(Ϋ,ω[i].GetPosition());if(i==0){Βϋ=Н.X;Βφ=Н.X;Βό=Н.Y;Βχ=Н.Y;Βύ=Н.Z;Βψ=Н.Z;}Βφ=Math.Max(Н.X,Βφ);Βϋ=Math.Min(Н.X,Βϋ);
Βχ=Math.Max(Н.Y,Βχ);Βό=Math.Min(Н.Y,Βό);Βψ=Math.Max(Н.Z,Βψ);Βύ=Math.Min(Н.Z,Βύ);}if(Ƙ==0) Ƙ=drillRadius;bool Į=Ǡ!=null&&Ǡ.WorldMatrix.Forward==Ϋ.WorldMatrix.Down;ș=(Βφ-Βϋ)*(1-ũ)+Ƙ*2;Ș=(Į?Βψ-Βύ:Βχ-Βό)*(1-œ)+Ƙ*2;IMyCubeGrid g=Ϋ.CubeGrid;float Εσ=0,Ετ=0,Ευ=0,Εφ=0;
for(int c=0;c<8;c++){Vector3 p=ΑЁ(Ϋ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Į?p.Z:p.Y;if(c==0||p.X<Εσ) Εσ=p.X;if(c==0||p.X>Ετ) Ετ=p.X;if(c==0||y<Ευ) Ευ=y;if(c==0||y>Εφ) Εφ=y;}Ɵ=Ετ-Εσ+g.GridSize;Αΰ=Εφ-Ευ+g.GridSize;}float Ιχ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǩ<T>(List<T>Ѝ,String ǽ,bool Βθ){List<T>ί=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(Γθ(Ѝ[i],ǽ,Βθ)) ί.Add(Ѝ[i]);return ί;}bool Γθ<T>(T Ω,String ǽ,bool Βθ){IMyTerminalBlock b=(IMyTerminalBlock) Ω;if(Βθ&&b.CustomName.ToUpper().Contains(ǽ.ToUpper())) return true;if(!Βθ&&b.BlockDefinition.ToString().ToUpper().Contains(ǽ.ToUpper())) return true;return false;}void ǘ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
Ζδ((IMyTerminalBlock) Ѝ[i],М);}void Ζδ(IMyTerminalBlock Ω,bool М){if(Ω==null) return;String ΙЍ=М?"OnOff_On":"OnOff_Off";var a=Ω.GetActionWithName(ΙЍ);a.Apply(Ω);}void Ȃ(bool М){Εά=М;if(!ť) return;ǘ(ΒІ,М);}void ΓЉ(ChargeMode Џ){for(int i=0;i<Ī.Count;i++) Ī[i].ChargeMode=Џ;}void ΓЋ(bool ΛΔ)
{for(int i=0;i<ǜ.Count;i++) ǜ[i].Stockpile=ΛΔ;}void Ƽ(List<IMyLandingGear>Δς,bool Ηυ){for(int i=0;i<Δς.Count;i++){if(Ηυ) Δς[i].Lock();if(!Ηυ) Δς[i].Unlock();}}void Ζε(bool М){if(ΗΖ==М) return;List<IMyShipController>ΒΥ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΥ,ǖ);if(ΒΥ.Count==0) return;
for(int i=0;i<ΒΥ.Count;i++)
ΒΥ[i].DampenersOverride=М;ΗΖ=М;}IMyShipConnector ж(MyShipConnectorStatus Ǒ){for(int i=0;i<њ.Count;i++){if(!Ȟ(њ[i],true)) continue;if(њ[i].Status==Ǒ) return њ[i];}return null;}void ň(bool ΕΧ,bool ΒΫ,float Κέ,float ΚΒ){if(ψ==null||ω.Count==0) return;Vector3 ΒΠ=new Vector3();
int Ȫ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=Ǡ.WorldMatrix.Forward) continue;Ȫ++;ΒΠ+=ω[i].GetPosition();}ΒΠ=ΒΠ/Ȫ;Vector3 Ƒ=ΑЁ(ψ,ΒΠ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΧ;ψ.LeftExtend=(ΒΫ?1:ѐ)/2f*ș-Ƒ.X;ψ.RightExtend=(ΒΫ?1:ѐ)/2f*ș+Ƒ.X;ψ.TopExtend=(ΒΫ?1:я)/2f*Ș+Ƒ.Y;
ψ.BottomExtend=(ΒΫ?1:я)/2f*Ș-Ƒ.Y;ψ.FrontExtend=(ΕΧ?н:Κέ)-Ƒ.Z;ψ.BackExtend=ΕΧ?0:ΚΒ+ο*0.75f+Ƒ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void ņ<T>(List<T>Ѝ,bool ΚА,float ΚΫ,bool Κζ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Αε=null;if(Ѝ[i]is IMyTextSurface) Αε=(IMyTextSurface) Ѝ[i];if(Αε!=null){Αε.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΚА) continue;Αε.Font="Debug";if(Κζ) continue;Αε.FontSize=ΚΫ;}}}bool ΑЅ(){Vector3D κ;return this.Ϋ.TryGetPlanetPosition(out κ);}int ΘБ(Vector3 ΛΖ,int ΘΕ){if(ΘΕ==-1) return 0;double Ы=-1;int č=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,ΛΖ);if(Ы==-1||ϊ<Ы){č=i;Ы=ϊ;}
}return Math.Sign(č-ΘΕ);}bool ΙΟ(Vector3 κ){List<Vector3>а=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) а.Add(Ψ[i].Ε);if(Ο.λ&&Ψ.Count>=1){Vector3 ż=new Vector3();ȝ(Ο,dockDist*ο,false,out ż);if(Vector3.Distance(Ο.Ε,Ψ.First().Ε)<Vector3.Distance(Ο.Ε,Ψ.Last().Ε)){а.Insert(0,ż);а.Insert(0,Ο.Ε);}
else{а.Add(ż);а.Add(Ο.Ε);}}if(Μ==Κ.ή){if(Ζ.λ&&Ψ.Count>=1){Vector3 ΔΚ=new Vector3();ȝ(Ζ,dockDist*ο,false,out ΔΚ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){а.Insert(0,ΔΚ);а.Insert(0,Ζ.Ε);}else{а.Add(ΔΚ);а.Add(Ζ.Ε);}}}else{if(γ!=Π.э)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
а.Insert(0,Ζ.Ε);else а.Add(Ζ.Ε);}int č=-1;double ȇ=-1;for(int i=0;i<а.Count;i++){double ϊ=Vector3.Distance(а[i],κ);if(ϊ<ȇ||ȇ==-1){ȇ=ϊ;č=i;}}if(а.Count==0) return false;double Δη=Vector3.Distance(а[č],κ);double ΚЁ=Vector3.Distance(а[Math.Max(0,č-1)],а[č])*1.5f;
double Κφ=Vector3.Distance(а[Math.Min(а.Count-1,č+1)],а[č])*1.5f;return Δη<ΚЁ||Δη<Κφ||Δη<laneOffset*ο*1.5f;}static float Й(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ιβ(Vector3 v,int Δγ){return new Vector3(Math.Round(v.X,Δγ),Math.Round(v.Y,Δγ),Math.Round(v.Z,Δγ));
}float ŭ(Vector3 a,Vector3 b){if(a==b) return 0;float ΚΨ=(a*b).Sum;float Ηο=a.Length();float Ηπ=b.Length();if(Ηο==0||Ηπ==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(ΚΨ/(Ηο*Ηπ)));if(float.IsNaN(ί)) return 0;return ί;}float Ŀ(Vector3 v,Vector3 ΒЀ,float ΚБ){float ź=ŭ(v,ΒЀ);
if(ΚБ>0f) ź*=-1;if(ź>-90f) return ź-90f;else return 180f-(-ź-90f);}double ΕІ(float ΚΠ){return(Math.PI/180)*ΚΠ;}bool Γλ(double Κπ,double value,double Ǝ){return(value>=Κπ&&value<=Ǝ);}Vector3 Ιψ(IMyTerminalBlock Ω,Vector3 Н){return Vector3D.Transform(Н,Ω.WorldMatrix);
}Vector3 ΑЁ(IMyTerminalBlock Ω,Vector3 Λΰ){return ф(Ω,Λΰ-Ω.GetPosition());}Vector3 ф(IMyTerminalBlock Ω,Vector3 Ες){return Vector3D.TransformNormal(Ες,MatrixD.Transpose(Ω.WorldMatrix));}Vector3 Ň(Vector3 Ȍ,Vector3 Б,Vector3 Ες){MatrixD ΔЎ=MatrixD.CreateFromDir(Ȍ,Б);return Vector3D.TransformNormal(Ες,MatrixD.Transpose(ΔЎ));
}Vector3 ΔΔ(Vector3 Ȍ,Vector3 Б,Vector3 Н){MatrixD ΔЎ=MatrixD.CreateFromDir(Ȍ,Б);return Vector3D.Transform(Н,ΔЎ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 π(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ρ(τ,0)),float.Parse(Ρ(τ,1)),float.Parse(Ρ(τ,2)));
}String ΙЋ(ά έ){String П=":";String ί=σ(έ.Ε)+П+σ(έ.ι)+П+
σ(έ.θ)+П+σ(έ.Ѓ)+П+σ(έ.φ);for(int i=0;i<έ.Ǿ.Length;i++)
ί+=П+Math.Round(έ.Ǿ[i],3);return ί;}ά Ιρ(String s){String[]τ=s.Split(':');ά έ=new ά(
π(Ρ(τ,0)),π(Ρ(τ,1)),
π(Ρ(τ,2)),π(Ρ(τ,3)),
π(Ρ(τ,4)));int Β=5;List<float>ΗΤ=new List<float>();while(Β<τ.Length){String ΓΓ=Ρ(τ,Β);float f=0;if(!float.TryParse(ΓΓ,out f)) break;ΗΤ.Add(f);Β++;}έ.Ǿ=ΗΤ.ToArray();return έ;}System.Text.StringBuilder ƛ=new System.Text.StringBuilder();void Χ<T>(T value,bool Κυ){if(Κυ) ƛ.Append('\n');ƛ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String Αϊ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΗ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ρ(String[]ΙБ,int Β){String ΓΓ=ΙБ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΓ)) return "";
return ΓΓ;}void Save(){if(ΒЂ||Μ==Κ.ј){Storage="";return;}Storage=ΓΟ();}String ΓΟ(){ƛ.Clear();ƛ.Append(DATAREV+";");Χ(σ(ΒЁ),false);Χ(σ(Ο.ι));Χ(σ(Ο.Ѓ));Χ(σ(Ο.θ));Χ(σ(Ο.φ));Χ(σ(Ο.Ε));Χ(σ(Ο.о));Χ(Ο.λ);Χ(σ(Ζ.Ε));Χ(σ(Ζ.φ));Χ(σ(Ζ.ι));Χ(σ(Ζ.θ));Χ(σ(Ζ.Ѓ));Χ(σ(Ζ.о));Χ(Ζ.λ);Χ(σ(ǹ));Χ(σ(ę));Χ(";");
Χ((int) Μ,false);Χ((int) γ);Χ((int) ў);Χ(ȏ);Χ(џ);Χ(п);Χ(Ǎ);Χ(ȃ);Χ(ġ);Χ(Ġ);if(Μ==Κ.ή){Χ((int) Ї.Р);Χ(Ї.Ǳ);Χ(Ї.İ);Χ(Αϊ(Ї.ǲ));Χ(Αϊ(Ї.Ƕ));Χ((int) Ј.Р);Χ(Ј.Ǳ);Χ(Ј.İ);Χ(Αϊ(Ј.ǲ));Χ(Αϊ(Ј.Ƕ));}else{Χ((int) ȓ);Χ((int) ѓ);Χ((int) г);Χ((int) ћ);Χ((int) Ř);Χ(Ļ);Χ(ť);Χ(Ɖ);Χ(ƃ);Χ(ѐ);Χ(я);Χ(н);Χ(ƭ);Χ(Ƭ);
Χ(ũ);Χ(œ);Χ(Ќ);Χ(Ю);Χ(Ѐ);Χ(ύ);Χ(ė);Χ(б);Χ(Ţ);Χ(ś);}Χ(";");for(int i=0;i<Ā.Count;i++)
Χ((i>0?"|":"")+Ā[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΙЋ(Ψ[i]),i>0);Χ(";",false);Χ(й+"|"+Д,false);Χ(";",false);Χ(ΘЂ(),false);String Θ=ƛ.ToString();ƛ.Clear();return Θ;}љ Ια(){return ΑЏ(Storage);}љ ΑЏ(String Θ){if(Θ=="") return љ.ΙΨ;String[]ţ=Θ.Split(';');if(Ρ(ţ,0)!=DATAREV) return љ.Γχ;int Β=0;try{String[]Ξ=Ρ(ţ,1).Split('\n');
ΒЁ=π(Ρ(Ξ,Β++));Ο.ι=π(Ρ(Ξ,Β++));Ο.Ѓ=π(Ρ(Ξ,Β++));Ο.θ=π(Ρ(Ξ,Β++));Ο.φ=π(Ρ(Ξ,Β++));Ο.Ε=π(Ρ(Ξ,Β++));Ο.о=π(Ρ(Ξ,Β++));Ο.λ=bool.Parse(Ρ(Ξ,Β++));Ζ.Ε=π(Ρ(Ξ,Β++));Ζ.φ=π(Ρ(Ξ,Β++));Ζ.ι=π(Ρ(Ξ,Β++));Ζ.θ=π(Ρ(Ξ,Β++));Ζ.Ѓ=π(Ρ(Ξ,Β++));Ζ.о=π(Ρ(Ξ,Β++));Ζ.λ=bool.Parse(Ρ(Ξ,Β++));
ǹ=π(Ρ(Ξ,Β++));ę=π(Ρ(Ξ,Β++));Ξ=Ρ(ţ,2).Split('\n');Β=0;Μ=(Κ) int.Parse(Ρ(Ξ,Β++));γ=(Π) int.Parse(Ρ(Ξ,Β++));ў=(Π) int.Parse(Ρ(Ξ,Β++));ȏ=int.Parse(Ρ(Ξ,Β++));џ=int.Parse(Ρ(Ξ,Β++));п=int.Parse(Ρ(Ξ,Β++));Ǎ=int.Parse(Ρ(Ξ,Β++));ȃ=float.Parse(Ρ(Ξ,Β++));ġ=bool.Parse(Ρ(Ξ,Β++));
Ġ=bool.Parse(Ρ(Ξ,Β++));if(Μ==Κ.ή){Ї.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Ї.Ǳ=float.Parse(Ρ(Ξ,Β++));Ї.İ=float.Parse(Ρ(Ξ,Β++));Ї.ǲ=ΒΗ(Ρ(Ξ,Β++));Ї.Ƕ=ΒΗ(Ρ(Ξ,Β++));Ј.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Ј.Ǳ=float.Parse(Ρ(Ξ,Β++));Ј.İ=float.Parse(Ρ(Ξ,Β++));Ј.ǲ=ΒΗ(Ρ(Ξ,Β++));Ј.Ƕ=ΒΗ(Ρ(Ξ,Β++));}else
{ȓ=(ю) int.Parse(Ρ(Ξ,Β++));ѓ=(ъ) int.Parse(Ρ(Ξ,Β++));г=(Г) int.Parse(Ρ(Ξ,Β++));ћ=(Ǖ) int.Parse(Ρ(Ξ,Β++));Ř=(ю) int.Parse(Ρ(Ξ,Β++));Ļ=bool.Parse(Ρ(Ξ,Β++));ť=bool.Parse(Ρ(Ξ,Β++));Ɖ=bool.Parse(Ρ(Ξ,Β++));ƃ=bool.Parse(Ρ(Ξ,Β++));ѐ=int.Parse(Ρ(Ξ,Β++));я=int.Parse(Ρ(Ξ,Β++));н=int.Parse(Ρ(Ξ,Β++));
ƭ=float.Parse(Ρ(Ξ,Β++));Ƭ=float.Parse(Ρ(Ξ,Β++));ũ=float.Parse(Ρ(Ξ,Β++));œ=float.Parse(Ρ(Ξ,Β++));Ќ=int.Parse(Ρ(Ξ,Β++));Ю=int.Parse(Ρ(Ξ,Β++));Ѐ=int.Parse(Ρ(Ξ,Β++));ύ=int.Parse(Ρ(Ξ,Β++));ė=int.Parse(Ρ(Ξ,Β++));б=int.Parse(Ρ(Ξ,Β++));Ţ=int.Parse(Ρ(Ξ,Β++));ś=float.Parse(Ρ(Ξ,Β++));
}Ξ=Ρ(ţ,3).Replace("\n","").Split('|');Ā=Ξ.ToList();Ξ=Ρ(ţ,4).Split('\n');Ψ.Clear();if(Ξ.Count()>=1&&Ξ[0]!="")
for(int i=0;i<Ξ.Length;i++)
Ψ.Add(Ιρ(Ρ(Ξ,i)));Ξ=Ρ(ţ,5).Split('|');й=Ρ(Ξ,0);if(!int.TryParse(Ρ(Ξ,1),out Д)) Д=-1;if(й.Length!=Ќ*Ю){й="";Д=-1;}ΘЁ(Ρ(ţ,6));}catch{return љ.ΕЉ;}Ɯ=ў;Ђ();return љ.ΒΔ;}const int Εύ=64000;IMyTerminalBlock ΕЊ(){List<IMyTerminalBlock>ſ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(ſ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ſ.Count>0?ſ[0]:null;}void ΘІ(){IMyTerminalBlock Ω=ΕЊ();if(Ω==null){Λ="Export: name a block "+dataTag;return;}String Θ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΓΟ();if(Θ.Length>Εύ){Λ="Export too large: "+Θ.Length+" chars, max "+Εύ;return;}Ω.CustomData=Θ;Λ="Exported "+Ψ.Count+" waypoints";}void ΙΛ(bool ΔЁ){IMyTerminalBlock Ω=ΕЊ();if(Ω==null){Λ="Import: name a block "+dataTag;return;}
ΙΜ(Ω.CustomData,ΔЁ);}void ΙΜ(String Θ,bool ΔЁ){Θ=Θ.Replace("\r","");if(Θ.StartsWith("//")){int ΗЃ=Θ.IndexOf('\n');Θ=ΗЃ<0?"":Θ.Substring(ΗЃ+1);}Θ=Θ.Trim();if(Θ==""){Λ="Import: no PAM data in "+dataTag;return;}Ђ();String ΖϏ=ΓΟ();љ ί=ΑЏ(Θ);if(ί==љ.ΒΔ&&ΔЁ){ά Υ=new ά(Ζ);
Vector3 Κσ=ǹ,Κς=ę;int w=ѐ,h=я,d=н;ю Αγ=ȓ;Ǖ Δδ=ћ;float Κϊ=ũ,Κω=œ;ΑЏ(ΖϏ);Ζ=Υ;ǹ=Κσ;ę=Κς;ѐ=w;я=h;н=d;ȓ=Αγ;ћ=Δδ;ũ=Κϊ;œ=Κω;γ=Π.ŵ;ǀ(true,true);γ=Π.ŵ;ў=Π.ŵ;Λ="Imported job, run CONT";return;}if(ί==љ.ΒΔ){ΑΪ=false;Λ="Imported "+Ψ.Count+" waypoints";
return;}ΑЏ(ΖϏ);Λ=ί==љ.Γχ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζζ(){ŏ.Clear();for(int i=Ц.Count-1;i>=0;i--){String Θ=Ц[i].CustomData.ToUpper();bool ΔϏ=false;if(Θ=="INSTRUCTIONS"){ΔϏ=true;ΒΪ=true;}if(Θ=="DEBUG") ΔϏ=true;if(ΔϏ){ŏ.Add(Ц[i]);Ц.RemoveAt(i);}
}ņ(ŏ,false,1,false);}void Ζη(List<IMyTerminalBlock>ſ){ȕ.Clear();for(int i=0;i<ſ.Count;i++){IMyTerminalBlock Ω=ſ[i];try{String ΗЋ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ω.CustomName.IndexOf(ΗЋ);int Ƣ=-1;if(Β<0||!int.TryParse(Ω.CustomName.Substring(Β+ΗЋ.Length,1),out Ƣ)) continue;
if(Ƣ==-1) continue;Ƣ--;IMyTextSurfaceProvider ΗЍ=(IMyTextSurfaceProvider) Ω;if(Ƣ<ΗЍ.SurfaceCount&&Ƣ>=0)
ȕ.Add(ΗЍ.GetSurface(Ƣ));}catch{}}}String Βω="";void Ζα(){String ΔЍ="";String ΑΕ="";Βω=Εό(false);ΔЍ+=Βω;ΑΕ+=Βω;ΑΕ+=ΘЎ();for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(ΔЍ);for(int i=0;i<ȕ.Count;i++) ȕ[i].WriteText(ΔЍ);Echo(ΑΕ);for(int i=0;i<ŏ.Count;i++){IMyTextPanel ч=ŏ[i];
String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΕБ());}}string ΕБ(){String Γ="";try{float Ηψ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ž*Ηψ)+" / "+(int)(Ə*Ηψ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ž*100f,1)+"% / "+Math.Round(Ə*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ǆ,2)+"ms / "+Math.Round(ΔБ,2)+"ms \n";}catch{}for(int i=0;i<Ě.Count;i++)
Γ+=""+Ě.Keys.ElementAt(i)+" = "+Math.Round(Ě.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ě.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙΗ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ņ(Ц,setLCDFontAndSize,0.9f,false);ņ(ȕ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Γ+Ė.ToString());for(int i=0;i<ȕ.Count;i++) ȕ[i].WriteText(Γ+Ė.ToString());}void Ȣ(){if(!ΒΪ) return;try{ΕΝ=Runtime.CurrentInstructionCount;}catch{}
}void Ȝ(String ΰ){if(!ΒΪ) return;if(ΕΝ==0) return;try{float ΓΑ=(Runtime.CurrentInstructionCount-ΕΝ)/Runtime.MaxInstructionCount*100;if(!Ě.ContainsKey(ΰ))
Ě.Add(ΰ,new float[]{ΓΑ,ΓΑ});else{Ě[ΰ][0]=ΓΑ;Ě[ΰ][1]=Math.Max(ΓΑ,Ě[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Θκ(String ȋ){if(!ƀ) return;String П=""+BROADCAST_SEP;String β=VERSION+П;β+=Ʈ(Ơ)+П;
β+=(int) Μ+П;β+=Ű(δ.X)+""+П;β+=Ű(δ.Y)+П;β+=Ű(δ.Z)+П;if(Μ!=Κ.ή)
β+=Γα(ў)+(ў==Π.В?" "+Math.Round(ΑΠ,1)+"%":"")+П;else β+=Γα(ў)+П;β+=Ʈ(Λ)+П;β+=Ў+""+П;β+=Ʈ(Βω)+П;β+=ȉ+П;β+=Ȑ+П;for(int i=0;i<ǵ.Count;i++){if(ǵ[i].ƌ!=Щ.Ų) continue;β+=Ʈ(ǵ[i].ΰ)+"/"+Ʈ(ǵ[i].υ)+"/"+ǵ[i].Њ+П;}Ιλ(β,ȋ);}String Ʈ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ιλ(String β,String ȋ){ΓЇ(β,BROADCAST_SELF_ID,ȋ);}void ΓЇ(String β,String η,String ȋ){try{if(!ƀ) return;β=BROADCAST_TAG+BROADCAST_SEP+ΕЍ()+BROADCAST_SEP+η+BROADCAST_SEP+ΔΧ+BROADCAST_SEP+ȋ+BROADCAST_SEP+β;this.IGC.SendBroadcastMessage(BROADCAST_TAG,β);}catch(Exception e){Ė=e;}
}String ΕЍ(){if(Μ!=Κ.ј) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γπ(String β){return β.StartsWith(BROADCAST_TAG);}bool е(ref String Θ,out string ΑΗ,Char П){int Β=Θ.IndexOf(П);ΑΗ="";if(Β<0) return false;ΑΗ=Θ.Substring(0,Β);Θ=Θ.Remove(0,Β+1);return true;}bool Ζΰ(ref String β,out String Ȓ,out String ȋ)
{Ȓ="";ȋ="";if(!ƀ) return false;String ǽ="";if(!е(ref β,out ǽ,BROADCAST_SEP)||!Γπ(ǽ)) return false;if(!е(ref β,out Ȓ,BROADCAST_SEP)) return false;if(!е(ref β,out ǽ,BROADCAST_SEP)||(ǽ!=ΕЍ()&&(ǽ!="*"&&Μ!=Κ.ј))) return false;if(!е(ref β,out ǽ,BROADCAST_SEP)||(ǽ!=ΔΧ)) return false;
if(!е(ref β,out ȋ,BROADCAST_SEP)) return false;return true;}void ΖΞ(){bool Ǌ=true;if(ΗΦ){ΗΦ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ΑΜ=(Μ!=Κ.ј?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΜ)
ΖΟ(ΑΜ);String[]Ξ=Me.CustomData.Split('\n');ƀ=bool.Parse(ΑЎ(Αό(Ξ,"Enable_Broadcast",ref Ǌ)));bool Επ=false;bool ΗΚ=true;if(ƀ){if(Μ!=Κ.ј)
Ơ=ΑЎ(Αό(Ξ,"Ship_Name",ref Ǌ)).Replace(BROADCAST_SELF_ID,"");ΔΧ=ΑЎ(Αό(Ξ,"Broadcast_Channel",ref Ǌ)).ToLower();ΗΚ=false;if(ǯ==null){ǯ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ǯ.SetMessageCallback("");}List<IMyRadioAntenna>Ż=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ż);bool ΔΙ=false;
for(int i=0;i<Ż.Count;i++)
if(Ż[i].EnableBroadcasting&&Ż[i].Enabled){ΔΙ=true;break;}if(Ż.Count==0) Λ="No Antenna found";else if(!ΔΙ) Λ="Antenna not ready";Επ=Ż.Count==0||!ΔΙ;if(Ζω&&!Επ&&Μ!=Κ.ј) Λ="Antenna ok";}else if(Μ==Κ.ј)
Λ="Offline - Enable in PB custom data";Ζω=Επ;if(ΗΚ){if(ǯ!=null) this.IGC.DisableBroadcastListener(ǯ);ǯ=null;}}catch{Ǌ=false;}if(!Ǌ){Λ="Reset custom data";ΖΟ(ΑΜ);}}void ΖΟ(String ΑΜ){String Θ=ΑΜ+"\n\n"+"Enable_Broadcast="+(Μ==Κ.ј?"true":"false")+" \n";Θ+=Μ!=Κ.ј?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Θ+"Broadcast_Channel=#default";}String ΑЎ(String Ś){int Β=Ś.IndexOf("//");if(Β!=-1) Ś=Ś.Substring(0,Β);String[]τ=Ś.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αό(String[]Ξ,String ȯ,ref bool ȱ){foreach(String Ś in Ξ)
if(Ś.StartsWith(ȯ)) return Ś;ȱ=false;return "";}void Θώ(string Т){if(ď) Ь=new List<Ѕ>();String Ȓ="";String ȋ="";if(!Γπ(Т)) Ιί(Т);bool ΘΑ=false;int ΕΓ=50;while(ǯ!=null&&ǯ.HasPendingMessage&&ΕΓ-->0){MyIGCMessage β=ǯ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;
if(Ζΰ(ref Θ,out Ȓ,out ȋ)&&Ȓ!=""&&Ȓ!=BROADCAST_SELF_ID){Ѕ Δ=Γί(Ȓ,"");if(Δ==null){Δ=new Ѕ(Ȓ);Ь.Add(Δ);}Δ.ń(ȋ,false,false,true);ΙЅ(Δ,Θ);ΘΑ=true;}}if(ΘΑ)
Ь.Sort(delegate(Ѕ a,Ѕ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ĕ||ď){if(Αή<=0||ď){Λ="";ď=false;Ιι();ΖΞ();ΔІ=DateTime.Now;}}if(ŗ) Ιύ();if(ŗ) ΙЁ();if(ŗ) ΙЉ();Ιϋ();}void ΙЅ(Ѕ Δ,String Θ)
{Δ.ΔЃ=DateTime.Now;String ΑΗ="";е(ref Θ,out Δ.ΓΕ,BROADCAST_SEP);е(ref Θ,out Δ.ΰ,BROADCAST_SEP);if(Δ.ΓΕ!=VERSION) return;String ΑΩ="";String[]Ɨ=new string[3];е(ref Θ,out ΑΩ,BROADCAST_SEP);е(ref Θ,out Ɨ[0],BROADCAST_SEP);е(ref Θ,out Ɨ[1],BROADCAST_SEP);е(ref Θ,out Ɨ[2],BROADCAST_SEP);
е(ref Θ,out Δ.Εέ,BROADCAST_SEP);е(ref Θ,out Δ.Λ,BROADCAST_SEP);е(ref Θ,out ΑΗ,BROADCAST_SEP);е(ref Θ,out Δ.Βϊ,BROADCAST_SEP);String Θΰ="",Ηϋ="";е(ref Θ,out Θΰ,BROADCAST_SEP);е(ref Θ,out Ηϋ,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String Ηι;if(!е(ref Θ,out Ηι,BROADCAST_SEP)) break;String[]Βξ=Ηι.Split('/');
if(Βξ.Count()<3) continue;int Њ=0;if(!int.TryParse(Βξ[2],out Њ)) continue;Δ.Ћ.Add(new м(Βξ[0],Βξ[1],Њ,Щ.Ų));}int ΗϏ=0;if(!int.TryParse(ΑΗ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΩ,out ΗϏ)) Δ.Μ=Κ.Ĩ;if(!float.TryParse(Ɨ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ɨ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ɨ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(Θΰ,out Δ.ȉ)) Δ.ȉ=0;if(!float.TryParse(Ηϋ,out Δ.Ȑ)) Δ.Ȑ=0;Δ.Μ=(Κ) ΗϏ;Δ.Δθ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Ει=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.Ει+=Δ.Ћ[i].Њ;}Ѕ Γί(String Φ,String ΰ){Φ=Φ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Ь.Count;i++){if(Φ!=""&&Ь[i].Φ.ToUpper()==Φ) return Ь[i];if(ΰ!=""&&Ь[i].ΰ!=null&&Ь[i].ΰ.ToUpper()==ΰ) return Ь[i];}return null;}String Ƃ="";Ѕ ΖЉ=null;DateTime ΖЊ=DateTime.MinValue;List<Ѕ>Ō=new List<Ѕ>();const float Θο=20f;bool Ζι(Ѕ η,String І){return І==Ƃ&&η==ΖЉ&&(DateTime.Now-ΖЊ).TotalSeconds<Θο;
}String ΘЈ(Ѕ η){if(Ƃ==""||!Ζι(η,Ƃ)) return "";String s="";int ȱ=0,ΕΨ=0;for(int i=0;i<Ō.Count;i++){Љ ΒЈ=Ō[i].ń(Ƃ,false,false,false);if(ΒЈ==Љ.ƺ) ȱ++;if(ΕΨ++<8) s+="  "+Ō[i].ΰ+": "+(ΒЈ==Љ.ƺ?"received":ΒЈ==Љ.Ŷ?"pending...":ΒЈ==Љ.ΑЋ?"no answer!":"-")+"\n";
}if(ΕΨ>8) s+="  +"+(ΕΨ-8)+" more\n";return "--------------------------------------------\n"+Ƃ+": "+ȱ+"/"+Ō.Count+" received\n"+s;}void ǫ(Ѕ Δ,String І){Ƃ=І;ΖЉ=Δ;ΖЊ=DateTime.Now;Ō.Clear();if(Δ==null) Ō.AddRange(Ь);else Ō.Add(Δ);if(Δ==null){for(int i=0;i<Ь.Count;i++)
Ь[i].ń(І,true,false,false);ΓЇ(І,"*",І);}else{Δ.ń(І,true,false,false);ΓЇ(І,Δ.Φ,І);}}void Ιί(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȥ=τ.ElementAtOrDefault(1);String Ȯ="Invalid argument: "+Т;if(ό==Σ.ƽ){if(Л!=null)
{switch(І){case "UP":{if(Ў<2) break;if(Л.Ў==0){Ў=1;return;}ǫ(Л,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){ǫ(Л,"MRES");break;}ǫ(Л,"DOWN");break;}case "APPLY":{if(Ў<2) break;ǫ(Л,"APPLY");return;}}}}switch(І){case "UP":this.Γτ(false);return;case "DOWN":this.Γσ(false);return;
case "APPLY":this.Ů(true);return;}switch(І){case "CLEAR":Θψ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȥ)){Λ="Usage: SENDALL <cmd>";return;}ǫ(null,ȥ);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Ιμ(Т.Remove(0,"SEND".Length+1));return;}Λ=Ȯ;}void Ιμ(String Ǆ){if(Ǆ=="") return;var τ=Ǆ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ơ=τ.ElementAtOrDefault(0).Trim();
Ѕ Δ=Γί("",Ơ);if(Δ!=null) ǫ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ơ;}void Θψ(){Ь.Clear();Л=null;μ(Σ.Main);Ў=0;}void Ιι(){List<IMyTerminalBlock>Αζ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ц,ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αζ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=ǩ(Ц,pamTag,true);Ζη(Αζ);Ζζ();Ικ();ņ(Ц,setLCDFontAndSize,1.15f,false);ņ(ȕ,setLCDFontAndSize,1.15f,true);String Κβ=ΖΑ(false);String ΗЄ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ч in Ц){if(ч.CustomData==""){ч.CustomData=ΖΑ(true);continue;}
if(!ч.CustomData.Contains(Κβ))
if(!ч.CustomData.Contains(ΗЄ)) ч.CustomData=ΗЄ+ч.CustomData;}}void Ιϋ(){String ΑΕ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȕ.Count;i++)
ȕ[i].WriteText(Εϊ(С.ΖΩ,"0",false));for(int i=0;i<Ц.Count;i++){С Џ=С.Ĩ;String Ǻ="";ΙΪ(Ц[i],out Џ,out Ǻ);Ц[i].WriteText(Εϊ(Џ,Ǻ,i==0));}Echo(ΑΕ);for(int i=0;i<ŏ.Count;i++){IMyTextPanel ч=ŏ[i];String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΕБ());
}}String Ů(bool Ι){int ΚΩ=0;return Ů(Ι,0,ref ΚΩ,false,1);}String Ů(bool Ι,int Κύ,ref int ΚЌ,bool ΖЍ,int ΑΫ){String Γ="";String ƞ="——————————————————\n";String ΛΙ="--------------------------------------------\n";if(Л==null||ό==Σ.Main||ό==Σ.ΓЈ)
Γ+="[PAM]-Controller | "+Ь.Count+" Connected ships"+"\n"+ΘЋ()+ΘА()+ΙΔ();else Γ+=ΖΔ(Л)+"\n";Γ+=ƞ;int Β=0;int Η=Ў;Ŝ=0;if(Λ!=""){Ν(ref Γ,Η,Β++,Ι,Λ);}else if(ό==Σ.Main){if(Л==null&&Ь.Count>=1) Л=Ь[0];String Ѝ="";if(Ν(ref Ѝ,Η,Β++,Ι," Send command to all")) μ(Σ.ΓЈ);
int Ɲ=0;for(int s=0;s<Ь.Count;s++){if(ΖЍ) Ѝ+="\n";Ѕ Δ=Ь[s];if(Η==Β||Η==Β+1) Л=Δ;if(Β==Η) Ɲ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+ΖΔ(Δ))){Л=Δ;μ(Σ.ƽ);Л.ń("",false,true,false);}if(Β==Η) Ɲ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+(ΖЍ?"":"  ")+ΙΑ(Δ)))
{Л=Δ;μ(Σ.Ζθ);}}int ƪ=ΑΫ-2;ƪ+=Math.Max(0,(Κύ-1))*ΑΫ;Γ+=ΓІ(ƪ,Ѝ,Ɲ,ref ΚЌ);}else if(ό==Σ.ΒΑ||ό==Σ.ΓЈ){Σ ΚΑ=ό==Σ.ΒΑ?Σ.ƽ:Σ.Main;Ѕ η=null;if(ό==Σ.ΒΑ) η=Л;if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(ΚΑ);}if(Ν(ref Γ,Η,Β++,Ι," [Stop] "+ű(η,"STOP"))){ǫ(η,"STOP");}if(Ν(ref Γ,Η,Β++,Ι," [Continue] "+ű(η,"CONT"))){ǫ(η,"CONT");}
if(Ν(ref Γ,Η,Β++,Ι," [Move home] "+ű(η,"HOMEPOS"))){ǫ(η,"HOMEPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Move to job] "+ű(η,"JOBPOS"))){ǫ(η,"JOBPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Full simulation] "+ű(η,"FULL"))){ǫ(η,"FULL");}if(η!=null&&η.Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," [Undock] "+ű(η,"UNDOCK"))){ǫ(η,"UNDOCK");}Γ+=ΘЈ(η);}else if(ό==Σ.ƽ){String ΔΠ="";if(Ν(ref ΔΠ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΔΠ.Substring(0,ΔΠ.Length-1).PadRight(36,' ');if(Ν(ref Γ,Η,Β++,Ι," Send cmd")){μ(Σ.ΒΑ);}if(!Л.ΖΥ()&&!Л.Γν()) Ŝ++;
if(Л.Γν()) Γ+="No answer received...";else Γ+=Ÿ(Л,Л.Βϊ);}else if(ό==Σ.Ζθ){if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΛΙ;Γ+=Ÿ(Л,ΖΓ(Л));}if(!ΕΒ) Ў=Math.Min(Ŝ-1,Ў);return Γ;}String ΖΔ(Ѕ Δ){if(Δ.ΓΕ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ÿ(Δ,Δ.Εέ)+" "+Δ.Δθ+"m";
}String ΙΑ(Ѕ Δ){String s=ΓЃ("",Й(Δ.ȉ,Δ.Ȑ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;м ώ=Δ.Ћ[i];s+=Ζσ(ΓΠ(ΘЏ(ώ)),3)+" "+ΕЋ(ώ.Њ)+" ";}return s;}String ΘЏ(м ώ){if(ώ.υ.ToUpper()=="ORE"||ώ.υ.ToUpper()=="INGOT"){String ΖЋ=ΘЍ(ώ.ΰ.ToUpper());if(ΖЋ!="") return ΖЋ;
}return ώ.ΰ;}String Ÿ(Ѕ Δ,String Γ){if(Δ.ΖΥ()) return "No signal...("+ΑϏ((int)(DateTime.Now-Δ.ΔЃ).TotalSeconds)+")";return Γ;}String ΖΑ(bool Λί){String Γ="";if(Λί) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΙΪ(IMyTerminalBlock ч,out С Џ,out String Ǻ){bool ȱ=true;String ΑΩ=ΑЎ(Αό(ч.CustomData.Split('\n'),"mode",ref ȱ)).ToUpper();String[]τ=ΑΩ.Split(':');String ΑΨ=τ.Length>0?τ[0].Trim():"";Ǻ=τ.Length>1?τ[1].Trim():"";
Џ=С.Ĩ;if(ΑΨ=="MAIN") Џ=С.ΖΪ;else if(ΑΨ=="MAINX") Џ=С.ΖΫ;else if(ΑΨ=="MENU") Џ=С.Γς;else if(ΑΨ=="INVENTORY") Џ=С.Γμ;else if(ΑΨ=="DEBUG") Џ=С.ΓΦ;if(Џ==С.Ĩ) Ǻ=ΑΩ;}String Εϊ(С Џ,String Ǻ,bool ΚΪ){int ΑΥ=15;if(ΚΪ){ΖЏ=ΒΣ;ΒΣ=0;ΖА=ΒΤ;ΒΤ=0;}String Γ="";if(Џ==С.ΖΪ)
{int ц=0;Γ=Ů(false,ΖЏ,ref ΚΛ,true,ΑΥ);if(!int.TryParse(Ǻ,out ц)) return Γ;ΒΣ=Math.Max(ц,ΒΣ);ц--;return Ζί(ц,ΑΥ,Γ);}else if(Џ==С.ΖΫ){int ц=0;Γ=Ů(false,ΖА,ref ΚΜ,false,ΑΥ);if(!int.TryParse(Ǻ,out ц)) return Γ;ΒΤ=Math.Max(ц,ΒΤ);ц--;return Ζί(ц,ΑΥ,Γ);}if(Џ==С.ΖΩ){return Ů(false,0,ref ΚΝ,true,ΑΥ);
}else if(Џ==С.Γμ||Џ==С.Γς||Џ==С.ΓΦ){Ѕ Δ=Л;if(Ǻ!=""){Δ=Γί("",Ǻ);if(Δ==null) return "Unknown ship: "+Ǻ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƞ="—————————————————————\n";String Εζ="";String ΔΦ="";if(Џ==С.Γς){Εζ="Menu";ΔΦ=Ÿ(Δ,Δ.Βϊ);}
else if(Џ==С.Γμ){Εζ="Inventory";ΔΦ=Ÿ(Δ,ΖΓ(Δ));}else if(Џ==С.ΓΦ){return Ÿ(Δ,ΙΒ(Δ));}Γ+=Δ.ΰ+" - "+Δ.Δθ+"m | "+ΓΠ(""+Εζ)+"\n"+ƞ;Γ+=ΔΦ;return Γ;}return "Unknown command: "+Ǻ;}string ΙΒ(Ѕ Δ){String s="";s+=Δ.ΓΕ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Εέ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βϊ.Length+"\n";s+=Δ.ȉ+"\n";s+=Δ.Ȑ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΖΓ(Ѕ Δ){String s="";s+=ΓЃ("All",Й(Δ.ȉ,Δ.Ȑ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){м ώ=Δ.Ћ[i];
s+=ΓЃ(ώ.ΰ,ώ.Њ,Δ.Ει,30,8,10)+"\n";}return s;}Љ ΘЉ(Ѕ Δ,bool Ъ,string І){Љ ί=Љ.None;if(Δ==null){for(int i=0;i<Ь.Count;i++){Љ Є=Ь[i].ń(І,false,Ъ,false);if(Є==Љ.ƺ) ί=Є;if(Є==Љ.ΑЋ) return Є;if(Є==Љ.Ŷ) return Є;}return ί;}else return Δ.ń(І,false,Ъ,false);}String ű(Ѕ Δ,String І)
{if(!Ζι(Δ,І)) return "";Љ Є=ΘЉ(Δ,false,І);if(Є==Љ.ƺ) return "received!";if(Є==Љ.Ŷ) return "pending...";if(Є==Љ.ΑЋ) return "no answer!";return "";}String ΓІ(int ƪ,String Γ,int Ɲ,ref int ě){String[]Ξ=Γ.Split('\n');if(Ɲ>=ě+ƪ-1) ě++;ě=Math.Min(Ξ.Count()-1-ƪ,ě);if(Ɲ<ě+1) ě--;
ě=Math.Max(0,ě);String ί="";for(int i=0;i<ƪ;i++){int Ης=i+ě;if(Ης>=Ξ.Count()) break;ί+=Ξ[Ης]+"\n";}return ί;}String Ζί(int ц,int ΑΫ,String Γ){String[]Ξ=Γ.Split('\n');int Αγ=ц*ΑΫ;int Ď=(ц+1)*ΑΫ;String ί="";for(int i=Αγ;i<Ď;i++){if(i>=Ξ.Count()) break;ί+=Ξ[i]+"\n";
}return ί;}String ΓΠ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζσ(String s,int Ηω){if(s=="") return s;if(s.Length>Ηω) s=s.Substring(0,Ηω-1)+".";return s;}string ΓЃ(String ř,float value,float Ǝ,int ΖЀ,int Ην,int Κό){float š=Й(value,Ǝ)*ΖЀ;
String ΒΟ="[";for(int i=0;i<ΖЀ;i++){if(i<=š) ΒΟ+="|";else ΒΟ+="'";}ΒΟ+="]";return ΒΟ+" "+Ζσ(ΓΠ(ř),Ην).PadRight(Ην)+"".PadRight(Κό)+ΕЋ(value);}String ΕЋ(float Њ){if(Њ>=1000000) return Math.Round(Њ/1000000f,Њ/1000000f<100?1:0)+"M";if(Њ>=1000) return Math.Round(Њ/1000f,Њ/1000f<100?1:0)+"K";
return ""+Math.Round(Њ);}String ΑϏ(int Αί){if(Αί>=60*60) return Math.Round(Αί/(60f*60f),1)+" h";if(Αί>=60) return Math.Round(Αί/60f,1)+" min";return ""+Αί+" s";}string Ű(float v){return Math.Round(v,2)+" ";}string Λζ(Vector3 v){return "X"+Ű(v.X)+"Y"+Ű(v.Y)+"Z"+Ű(v.Z);}
List<IMyCameraBlock>ȧ=new List<IMyCameraBlock>();List<long>ƒ=new List<long>();bool Ɣ=false;int Δέ=0;int ΔΪ=0;DateTime ΖЂ=DateTime.Now;String ΔΥ="";void Ιθ(){ȧ.Clear();ƒ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ȧ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȧ.Count;i++){ȧ[i].EnableRaycast=true;if(!ƒ.Contains(ȧ[i].CubeGrid.EntityId)) ƒ.Add(ȧ[i].CubeGrid.EntityId);}if(!ƒ.Contains(Me.CubeGrid.EntityId)) ƒ.Add(Me.CubeGrid.EntityId);}bool ΖΣ(Α s){return s==Α.Ć||s==Α.у||s==Α.И||
s==Α.Ħ||s==Α.ů||s==Α.Ʊ||s==Α.Αύ||
(s==Α.Ȁ&&ǟ!=null);}void Αψ(){Ɣ=false;Δέ=0;ΔΥ="";}void ΙЃ(){if(!obstacleBrake||ȧ.Count==0||!ΖΣ(Ϊ)){Αψ();return;}Vector3 ΑΘ=Ё+ĺ;Vector3 Ȗ=ΑΘ-δ;float Ȕ=Ȗ.Length();if(Ȕ<1f){Αψ();return;}Vector3 ς=Ȗ/Ȕ;float Ηφ=Ĝ*obstacleLookAhead+ο;Vector3 ΚЄ=ΑΘ+ς*(ο*0.25f);
for(int n=0;n<ȧ.Count;n++){ΔΪ=(ΔΪ+1)%ȧ.Count;IMyCameraBlock Ȇ=ȧ[ΔΪ];if(!Ȇ.IsFunctional||!Ȇ.IsWorking) continue;Vector3 ΖЄ=Ȇ.GetPosition();Vector3 Ď=ΚЄ;if(Vector3.Distance(ΖЄ,Ď)>Ηφ) Ď=ΖЄ+ς*Ηφ;if(!Ȇ.CanScan(Ď)) continue;MyDetectedEntityInfo Ȭ=Ȇ.Raycast(Ď);bool Κψ=!Ȭ.IsEmpty()&&!ƒ.Contains(Ȭ.EntityId)&&!ΙΣ(Ȭ.EntityId)&&
Ȭ.Type!=MyDetectedEntityType.FloatingObject;if(Κψ){if(!Ɣ) ΖЂ=DateTime.Now;Ɣ=true;Δέ=0;ΔΥ=Ȭ.Type.ToString();}else if(Ɣ&&++Δέ>=3) Αψ();break;}if(Ɣ&&(DateTime.Now-ΖЂ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΥ;}const String ΔΓ="[PAMTFC]";const float Ιυ=5f;const float Γή=2f;class ħ{public long Φ;public Vector3 κ,ǔ;public Vector3 đ;public float ΗΙ;public String Υ="";public int Ē=-1;public bool ΕΞ;public bool Ĳ;public float ё,ǳ;public long α=0;public int ΑΑ=0;public DateTime Ĺ;
}IMyBroadcastListener ΒА=null;long Ǐ=0;List<ħ>є=new List<ħ>();Vector3 ĺ=new Vector3();float ǒ=-1;Vector3 Εκ=new Vector3();bool ΒЏ=false;DateTime Ελ=DateTime.Now;const float Ζς=15f;const float Ιτ=10f;int Θέ=0;String й="";int Д=-1;bool Δα=false;Vector3 ΖБ=new Vector3();String ƶ()
{if(γ==Π.э||Μ==Κ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Ю+"|"+(int) Ř+"|"+Math.Round(ș,1)+"|"+Math.Round(Ș,1);}void ΙЇ(){if(!trafficEnabled) return;if(ΒА==null) ΒА=IGC.RegisterBroadcastListener(ΔΓ);Ǐ=Me.CubeGrid.EntityId;int ȅ=30;while(ΒА.HasPendingMessage&&ȅ-->0){String Θ=ΒА.AcceptMessage().Data as String;if(Θ!=null) Ιά(Θ);
}for(int i=є.Count-1;i>=0;i--)
if((DateTime.Now-є[i].Ĺ).TotalSeconds>Ιυ) є.RemoveAt(i);if(ΒЋ==null) ΒЋ=IGC.RegisterBroadcastListener(Ζπ);ȅ=5;while(ΒЋ.HasPendingMessage&&ȅ-->0){String Θ=ΒЋ.AcceptMessage().Data as String;if(Θ!=null) try{Ιέ(Θ);}catch{}}if(ΒΦ==null) ΒΦ=IGC.RegisterBroadcastListener(ΖΦ);
ȅ=10;while(ΒΦ.HasPendingMessage&&ȅ-->0){String Θ=ΒΦ.AcceptMessage().Data as String;if(Θ!=null) try{ΙΩ(Θ);}catch{}}if(++Θέ<3) return;Θέ=0;IGC.SendBroadcastMessage(ΔΓ,Ǐ+";"+σ(δ)+";"+
σ(Ϋ.GetShipVelocities().LinearVelocity)+";"+ο+";"+ƶ()+";"+
(γ==Π.В?Д:-1)+";"+й+";"+Θχ()+","+ΕЈ()+";"+σ(Εκ)+
";"+(Βή?1:0)+";"+(Ŕ&&γ==Π.В?1:0)+","+Ɵ+","+Αΰ+","+(ΖΛ()?1:0));}void Ιά(String Θ){try{String[]f=Θ.Split(';');if(f.Length<7) return;long Φ=long.Parse(f[0]);if(Φ==Ǐ) return;ħ Δ=null;for(int i=0;i<є.Count;i++) if(є[i].Φ==Φ) Δ=є[i];if(Δ==null){Δ=new ħ();Δ.Φ=Φ;є.Add(Δ);}
Δ.κ=π(f[1]);Δ.ǔ=π(f[2]);Δ.ΗΙ=float.Parse(f[3]);Δ.Υ=f[4];Δ.Ē=int.Parse(f[5]);Δ.Ĺ=DateTime.Now;if(Δ.Υ!=""&&Δ.Υ==ƶ()) Ζά(f[6]);Δ.α=0;Δ.ΑΑ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ΑΑ=int.Parse(d[1]);}Δ.đ=f.Length>=9?π(f[8]):new Vector3();
Δ.ΕΞ=f.Length>=10&&f[9]=="1";Δ.Ĳ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ё=float.Parse(w[1]);Δ.ǳ=float.Parse(w[2]);Δ.Ĳ=w[3]=="1";}}}catch{}}bool ΙΣ(long Φ){for(int i=0;i<є.Count;i++) if(є[i].Φ==Φ) return true;return false;}Vector3 ΖΒ(int Β,int Κώ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();ά έ=Ψ[Β];Vector3 ΛΧ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Κώ;Vector3 Б=έ.φ.LengthSquared()>0.01f?-Vector3.Normalize(έ.φ):-έ.θ;Vector3 р=Vector3.Cross(ΛΧ,Б);if(р.LengthSquared()<0.0001f) return new Vector3();
float ϊ=laneOffset*ο;float Κά=Math.Min(Vector3.Distance(έ.Ε,Ψ[0].Ε),
Vector3.Distance(έ.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(р)*ϊ*Math.Min(1,Κά/(ϊ*4));}Vector3 Θά=new Vector3();bool ΖΛ(){return γ==Π.В&&(Ϊ==Α.Ć||Ϊ==Α.Ŵ||Ϊ==Α.Ƞ);}bool ΙΡ(Α s){return s==Α.Ȁ||s==Α.Ľ||s==Α.Ł||s==Α.ƻ||
s==Α.ǭ||s==Α.Ƞ||s==Α.Ģ;}void ΙЈ(){ĺ=new Vector3();ǒ=-1;Εκ=new Vector3();bool ΑΙ=ΖΣ(Ϊ);if(!trafficEnabled||(!ΑΙ&&!ΙΡ(Ϊ))){ΒЏ=false;return;}Θά=Ϋ.GetShipVelocities().LinearVelocity;Θω(Ϋ.GetNaturalGravity(),!ΑΙ);Θη();}IMyBroadcastListener ΒЋ=null;float Ƥ=-1;DateTime ΘΨ=DateTime.MinValue;
void Ιέ(String Θ){if(!Θ.StartsWith("C;")) return;ΘΨ=DateTime.Now;Ƥ=-1;String ΑΧ=Ǐ+":";String[]e=Θ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΧ)) Ƥ=float.Parse(e[i].Substring(ΑΧ.Length));}void Θη(){if(Ƥ<0||(DateTime.Now-ΘΨ).TotalSeconds>1.5) return;
ǒ=ǒ<0?Ƥ:Math.Min(ǒ,Ƥ);if(Ƥ<0.5f) Λ="Traffic: giving way (controller)";}void Θω(Vector3 φ,bool ΗЊ=false){ĺ=new Vector3();ǒ=-1;Vector3 Ȗ=Ё-δ;float Ȕ=Ȗ.Length();bool Δϋ=Ȕ>=1f;Vector3 ς=Δϋ?Ȗ/Ȕ:new Vector3();Εκ=ς;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ΑΓ;
Vector3 р=Vector3.Cross(ς,Б);р=р.LengthSquared()<0.0001f?-ΑΤ:Vector3.Normalize(р);ħ Βπ=null;bool ΗΪ=Ϊ==Α.И&&!ċ(1);for(int i=0;i<є.Count;i++){ħ ζ=є[i];if(ζ.ΑΑ==3&&ζ.ǔ.LengthSquared()<0.25f) continue;Vector3 κ=ζ.κ+ζ.ǔ*(float)(DateTime.Now-ζ.Ĺ).TotalSeconds;Vector3 ǡ=κ-δ;
float Ъ=(ο+ζ.ΗΙ)*0.5f;bool ΘΒ=ζ.Ĳ&&ΖΛ()&&ζ.ё>0&&Ɵ>0&&ζ.Υ==ƶ();if(ΘΒ) Ъ=Math.Max(Ɵ+ζ.ё,Αΰ+ζ.ǳ)*0.5f;float ǃ=Δϋ?Vector3.Dot(ǡ,ς):0;if(ǃ>0&&(ǡ-ς*ǃ).Length()<=Ъ*1.2f){float Βώ=Vector3.Dot(ζ.ǔ,ς);bool Ηή=ζ.đ.LengthSquared()>0.25f;bool Ηί=Βώ<-1f||(Ηή&&Vector3.Dot(ζ.đ,ς)<-0.5f);
if(!ΗЊ){if(ζ.ΕΞ){if(!ΗΪ){ĺ=р*Ъ*1.3f;continue;}float Ηώ=Vector3.Distance(δ,Ё),Ηκ=Vector3.Distance(κ,Ё);if(Ηώ<Ηκ||(Ηώ==Ηκ&&Ǐ<ζ.Φ)) continue;}if(Ηί){if(ǃ<(Ĝ-Βώ)*6f+Ъ*4f) ĺ=р*Ъ*1.3f;continue;}if(Ηή&&Ǐ<ζ.Φ){Vector3 Ζώ=-ǡ;float ΗЇ=Vector3.Dot(Ζώ,ζ.đ);
if(ΗЇ>0&&(Ζώ-ζ.đ*ΗЇ).Length()<=Ъ*1.2f){ĺ=р*Ъ*1.3f;continue;}}}float ΗΩ=ǃ-Ъ-(ΘΒ?1f:followGap*ο);float Ȏ=Math.Max(0,Βώ)+Math.Max(0,ΗΩ)/Γή;if(Ηί) Ȏ=Math.Max(0,ΗΩ+Math.Min(0,Βώ)*Γή*2)/Γή;if(ǒ<0||Ȏ<ǒ){ǒ=Ȏ;Βπ=ζ;}continue;}if(ζ.ǔ.LengthSquared()<1f) continue;
if(Δϋ&&(ǡ-ς*ǃ).Length()<=Ъ*1.2f) continue;Vector3 ΕΥ=ζ.ǔ-Θά;float ΗЏ=ΕΥ.LengthSquared();if(ΗЏ<0.25f) continue;float t=-Vector3.Dot(ǡ,ΕΥ)/ΗЏ;if(t<=0||t>5f||(ǡ+ΕΥ*t).Length()>=Ъ*1.1f) continue;if(Ǐ>ζ.Φ||t<2f){ǒ=0;Βπ=ζ;}}bool ΑΝ=ǒ>=0&&ǒ<0.5f;
if(!ΑΝ||ΗЊ||(Βπ!=null&&Βπ.ΕΞ&&ΗΪ)){ΒЏ=false;return;}if(!ΒЏ){ΒЏ=true;Ελ=DateTime.Now;}double Ηΰ=(DateTime.Now-Ελ).TotalSeconds;if(Ηΰ>Ζς+Ιτ) Ελ=DateTime.Now;else if(Ηΰ>Ζς){ǒ=-1;ĺ=р*ο*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȭ=new List<int>();
String Ηβ="";void Γέ(){String ȯ=Ќ+"|"+Ю+"|"+(int) Ř;if(ȯ==Ηβ) return;Ηβ=ȯ;Εϋ(Ќ,Ю,Ř,ȭ);}void Εϋ(int w,int h,ю Αγ,List<int>ΔΫ){ΔΫ.Clear();if(Αγ==ю.ƿ){for(int k=0;k<w*h;k++){int ƙ=k/w;ΔΫ.Add(ƙ*w+(ƙ%2==0?k-ƙ*w:w-1-(k-ƙ*w)));}return;}int ΑΛ=(int) Math.Ceiling(w/2f),ΑΚ=(int) Math.Ceiling(h/2f);
int Δψ=w/2,Δχ=h/2;int x=0,y=0,Δλ=0,Δμ=-1,Ǝ=Math.Max(w,h);for(int i=0;i<Ǝ*Ǝ;i++){if(-ΑΛ<x&&x<=Δψ&&-ΑΚ<y&&y<=Δχ) ΔΫ.Add((y-1+ΑΚ)*w+(x-1+ΑΛ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Δλ;Δλ=-Δμ;Δμ=t;}x+=Δλ;y+=Δμ;
}}bool ΖΚ(int a,int b,int w,float ΒБ,float ΒЌ,float ё,float ǳ){return a==b||(Math.Abs(a%w-b%w)*ΒБ<ё+0.5f&&Math.Abs(a/w-b/w)*ΒЌ<ǳ+0.5f);}bool Γά(){int n=Math.Max(0,Ќ*Ю);if(й.Length==n) return false;й=new String('0',n);Д=-1;return true;}void Ζά(String ζ)
{Γά();if(ζ.Length!=й.Length) return;char[]h=й.ToCharArray();for(int i=0;i<h.Length;i++) if(ζ[i]=='1') h[i]='1';й=new String(h);}bool ΑЄ(int У){return У>=0&&У<й.Length&&й[У]=='1';}void ΓЊ(int У,bool Ϗ){if(У<0||У>=й.Length) return;char[]h=й.ToCharArray();h[У]=Ϗ?'1':'0';
й=new String(h);}bool Γι(int У,bool Κι){if(!trafficEnabled) return false;String ȯ=ƶ();for(int i=0;i<є.Count;i++){ħ t=є[i];if(t.Υ!=ȯ||t.Ē<0||(Κι&&t.Φ>Ǐ)) continue;if(ΖΚ(У,t.Ē,Ќ,ș,Ș,Ɵ,Αΰ)) return true;}return false;}int Θφ(int Β){int n=Ќ*Ю;if(n<1) return Β;Γέ();
if(Γά()) for(int k=0;k<Β&&k<n;k++) ΓЊ(ȭ[k],true);if(Д>=0&&(Β>=n||ȭ[Β]!=Д)){ΓЊ(Д,true);Д=-1;}if(ΖΧ()){if(Ȉ>=0&&!ΑЄ(Ȉ)){Ŕ=false;Д=Ȉ;return ȭ.IndexOf(Ȉ);}if(Ȉ==-2){Ŕ=false;Д=-1;return n;}if(Ȉ==-1&&Д>=0&&!ΑЄ(Д)){Ŕ=false;return Β;}Д=-1;
if(!Ŕ){Ŕ=true;ΗΒ=DateTime.Now;}if((DateTime.Now-ΗΒ).TotalSeconds<5) return-1;}Ŕ=false;if(Д>=0&&!ΑЄ(Д)&&!Γι(Д,true)) return Β;Д=-1;for(int k=0;k<n;k++){int c=ȭ[k];if(ΑЄ(c)||Γι(c,false)) continue;Д=c;return k;}return n;}const String ΖΦ="[PAMJOB]";const String Ζπ="[PAMTCC]";
IMyBroadcastListener ΒΦ=null;DateTime ΗΑ=DateTime.MinValue;DateTime ΗΒ=DateTime.Now;int Ȉ=-1;bool Ŕ=false;bool ΖΧ(){return trafficEnabled&&Ȉ!=-3&&(DateTime.Now-ΗΑ).TotalSeconds<3;}void ΙΩ(String Θ){String[]f=Θ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƶ()) return;
ΗΑ=DateTime.Now;Ζά(f[2]);Ȉ=-3;String ΑΧ=Ǐ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΧ)) Ȉ=int.Parse(e[i].Substring(ΑΧ.Length));}Ǭ Ιν(int Β){Γέ();if(Β<0||Β>=ȭ.Count) return Ǭ.Ασ;Ѐ=ȭ[Β]%Ќ;ύ=ȭ[Β]/Ќ;return Ǭ.ΑЀ;}class Ǘ{public long Φ;
public Vector3 κ,Э,Б;}List<Ǘ>Ф=new List<Ǘ>();bool ĭ=false;Vector3 Βΰ,Βί,Βα,Ββ;long ѝ=0;long Ȳ=0;ά ǟ=null;bool ΑΒ=false;ά ΓΩ(Vector3 κ,Vector3 Э,Vector3 Б){MatrixD m=MatrixD.CreateWorld(κ,Э,Б);ά w=new ά(Ο);w.ι=Vector3D.TransformNormal(Βΰ,m);w.θ=Vector3D.TransformNormal(Βί,m);
w.Ѓ=Vector3D.TransformNormal(Βα,m);w.Ε=κ+(Vector3) Vector3D.TransformNormal(Ββ,m);w.λ=true;return w;}int ΕЈ(){Α s=Ϊ;if(s==Α.ь) return ж(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ł||s==Α.Ȥ||s==Α.ŀ||s==Α.ΒΙ||
s==Α.Αϋ||s==Α.ǭ) return 3;if(s==Α.Ȁ||s==Α.Ľ||s==Α.ƻ) return 2;if(s==Α.И) return 1;return 0;}long Θχ(){if(ΕЈ()==0) return 0;if(ċ(1)&&ơ!=0) return ơ;return ǟ!=null?Ȳ:ѝ;}void ΖΨ(){if(Μ==Κ.ή||!Ο.λ) return;IMyShipConnector О=ж(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector ζ=О.OtherConnector;Vector3I Κγ=new Vector3I((int) Ο.о.X,(int) Ο.о.Y,(int) Ο.о.Z);if(О.Position==Κγ){MatrixD ƈ=MatrixD.Transpose(ζ.WorldMatrix);Βΰ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Forward,ƈ);Βί=Vector3D.TransformNormal(Ϋ.WorldMatrix.Down,ƈ);Βα=Vector3D.TransformNormal(Ϋ.WorldMatrix.Left,ƈ);
Ββ=Vector3D.TransformNormal(О.GetPosition()-ζ.GetPosition(),ƈ);ĭ=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ѝ=ζ.EntityId;List<IMyShipConnector>Œ=new List<IMyShipConnector>();χ.GetBlocksOfType(Œ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Œ.Count>0){Ф.Clear();for(int i=0;i<Œ.Count;i++){Ǘ p=new Ǘ();p.Φ=Œ[i].EntityId;p.κ=Œ[i].GetPosition();p.Э=Œ[i].WorldMatrix.Forward;p.Б=Œ[i].WorldMatrix.Up;Ф.Add(p);}}ǟ=null;Ȳ=0;if(ζ.EntityId==ѝ||!ĭ) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].Φ==ζ.EntityId){Ȳ=Ф[i].Φ;ǟ=ΓΩ(Ф[i].κ,Ф[i].Э,Ф[i].Б);}}bool Γο(long Φ,int ΗЁ){if(Φ==0) return false;for(int i=0;i<є.Count;i++){ħ t=є[i];if(t.α==Φ&&(t.ΑΑ>ΗЁ||(t.ΑΑ==ΗЁ&&t.Φ<Ǐ))) return true;}return false;}bool ΓΤ(int ƕ){if(Μ==Κ.ή||!trafficEnabled){ǟ=null;Ȳ=0;return true;}
long ξ=ǟ!=null?Ȳ:ѝ;if(!Γο(ξ,ƕ)) return true;if(!Γο(ѝ,ƕ)){ǟ=null;Ȳ=0;return true;}if(!ĭ) return false;Ǘ Ǯ=null;float ǈ=0;for(int i=0;i<Ф.Count;i++){Ǘ p=Ф[i];if(p.Φ==ѝ||Γο(p.Φ,ƕ)) continue;float d=Vector3.Distance(p.κ,Ο.Ε);if(Ǯ==null||d<ǈ){Ǯ=p;ǈ=d;}
}if(Ǯ==null) return false;Ȳ=Ǯ.Φ;ǟ=ΓΩ(Ǯ.κ,Ǯ.Э,Ǯ.Б);return true;}String ΘЂ(){String s=(ĭ?"1":"0")+":"+σ(Βΰ)+":"+σ(Βί)+":"+
σ(Βα)+":"+σ(Ββ)+":"+ѝ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].Φ+","+σ(Ф[i].κ)+","+σ(Ф[i].Э)+","+σ(Ф[i].Б);return s;}void ΘЁ(String s){Ф.Clear();ĭ=false;ѝ=0;String[]f=s.Split(':');if(f.Length<6) return;ĭ=f[0]=="1";Βΰ=π(f[1]);Βί=π(f[2]);Βα=π(f[3]);Ββ=π(f[4]);long.TryParse(f[5],out ѝ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǘ p=new Ǘ();if(!long.TryParse(e[0],out p.Φ)) continue;p.κ=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);Ф.Add(p);}}const String ư="[PAMDCK]";const float Θξ=120f;class ą{public long Δ;public int ǝ;public long ќ;public bool ΖЅ;public DateTime Ǜ,ΑΣ;
}IMyBroadcastListener Βέ=null;List<ą>Х=new List<ą>();List<IMyShipConnector>ĝ=new List<IMyShipConnector>();List<IMySensorBlock>ă=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΡ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΓΖ=new List<MyDetectedEntityInfo>();long Ǚ=0;int Δή=0;
long ΖЇ=0;DateTime ΖЈ=DateTime.Now;String Ő="";int ΗΞ=0;IMyCubeGrid Ğ=null;void Ικ(){Ğ=Me.CubeGrid;χ.GetBlocksOfType(ĝ,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ă,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΡ,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ă.Count;i++){ă[i].DetectSmallShips=true;ă[i].DetectLargeShips=true;}for(int i=0;i<ΑΡ.Count;i++) ΑΡ[i].EnableRaycast=true;}IMyShipConnector Ζλ(long Φ){if(Φ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Φ) as IMyShipConnector;return c!=null&&Ğ!=null&&c.CubeGrid.IsSameConstructAs(Ğ)?c:null;}bool Γξ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΤ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΡ.Count;i++)
{IMyCameraBlock Ȇ=ΑΡ[i];if(!Ȇ.IsWorking||Vector3D.Distance(Ȇ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ď=Ȇ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ȇ.CanScan(Ď)) continue;MyDetectedEntityInfo Ȭ=Ȇ.Raycast(Ď);if(!Ȭ.IsEmpty()&&Ȭ.EntityId!=Ğ.EntityId&&Ȭ.EntityId!=Δ&&Ȭ.Type!=MyDetectedEntityType.FloatingObject)
{Ő="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙΤ(){for(int s=0;s<ă.Count;s++){if(!ă[s].IsWorking) continue;ΓΖ.Clear();ă[s].DetectedEntities(ΓΖ);for(int i=0;i<ΓΖ.Count;i++){MyDetectedEntityInfo e=ΓΖ[i];bool Κή=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Κή&&e.EntityId!=Ǚ&&e.Velocity.Length()>0.5f){Ő="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Ιή(ą r){IMyShipConnector ќ=Ζλ(r.ќ);if(Γξ(ќ)&&ΖΤ(ќ,r.Δ)) return ќ.EntityId;if(!r.ΖЅ||ќ==null) return 0;IMyShipConnector Ǯ=null;double ǈ=0;for(int i=0;i<ĝ.Count;i++)
{IMyShipConnector c=ĝ[i];if(!Γξ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ќ.GetPosition());if(Ǯ==null||d<ǈ){if(ΖΤ(c,r.Δ)){Ǯ=c;ǈ=d;}}}return Ǯ==null?0:Ǯ.EntityId;}void ΙΕ(){ą Ɩ=null;long О=0;for(int ş=2;ş>=1&&Ɩ==null;ş--){ą Ş=null;for(int i=0;i<Х.Count;i++)
{ą r=Х[i];if(r.ǝ!=ş) continue;if(ş==1){long c=Ιή(r);if(c==0) continue;if(Ş==null||r.Ǜ<Ş.Ǜ){Ş=r;О=c;}}else if(Ş==null||r.Ǜ<Ş.Ǜ) Ş=r;}Ɩ=Ş;}if(Ɩ==null){if(Х.Count>0&&Ő=="") Ő="No free dock";return;}Ǚ=Ɩ.Δ;Δή=Ɩ.ǝ;ΖЇ=Ɩ.ǝ==1?О:0;ΖЈ=DateTime.Now;
}void Ιύ(){if(Ğ==null) return;if(Βέ==null) Βέ=IGC.RegisterBroadcastListener(ư);int ȅ=30;while(Βέ.HasPendingMessage&&ȅ-->0){String Θ=Βέ.AcceptMessage().Data as String;if(Θ!=null) ΙΖ(Θ);}Ιγ();if(++ΗΞ<3) return;ΗΞ=0;IGC.SendBroadcastMessage(ư,Θμ());}void ΙΖ(String Θ){try{String[]f=Θ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Х.Count-1;i>=0;i--) if(Х[i].Δ==Δ) Х.RemoveAt(i);if(Ǚ==Δ) Ǚ=0;}else if(f[0]=="R"&&f.Length>=5){long ќ=long.Parse(f[3]);if(Ζλ(ќ)==null) return;ą r=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Δ) r=Х[i];if(r==null){r=new ą();r.Δ=Δ;r.Ǜ=DateTime.Now;Х.Add(r);}
int ǝ=int.Parse(f[2]);if(r.ǝ!=ǝ) r.Ǜ=DateTime.Now;r.ǝ=ǝ;r.ќ=ќ;r.ΖЅ=f[4]=="1";r.ΑΣ=DateTime.Now;}}catch{}}void Ιγ(){for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].ΑΣ).TotalSeconds>3) Х.RemoveAt(i);ą ΒΡ=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Ǚ) ΒΡ=Х[i];if(Ǚ!=0&&ΒΡ==null) Ǚ=0;if(ΒΡ!=null&&(DateTime.Now-ΖЈ).TotalSeconds>Θξ){Ǚ=0;ΒΡ.Ǜ=DateTime.Now;}Ő="";if(Ǚ==0&&Х.Count>0&&!ΙΤ()) ΙΕ();
}String Θμ(){String β="S;"+Ğ.EntityId+";"+Ǚ+";"+Δή+";"+ΖЇ+";";for(int i=0;i<Х.Count;i++) β+=(i>0?",":"")+Х[i].Δ;β+=";";for(int i=0;i<ĝ.Count;i++){IMyShipConnector c=ĝ[i];β+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}β+=";";for(int i=0;i<ă.Count;i++){IMySensorBlock z=ă[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();β+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return β;}String ΘЋ()
{if(ĝ.Count==0&&ă.Count==0) return "";int ΑΙ=0;for(int i=0;i<ĝ.Count;i++) if(Γξ(ĝ[i])) ΑΙ++;return "Docks: "+ΑΙ+"/"+ĝ.Count+" free"+(Ǚ!=0?(Δή==1?" | docking":" | undocking"):"")+
(Х.Count>1?" | "+(Х.Count-1)+" waiting":"")+(Ő!=""?"\n"+Ő:"")+"\n";}IMyBroadcastListener ΒЉ=null;int ȫ=0;DateTime ΗΝ=DateTime.Now;DateTime ΘΖ=DateTime.MinValue;long ΘΗ=0;int ΘΘ=0;long ơ=0;bool Αο=false;int Δκ=0;List<Vector3>Αρ=new List<Vector3>();List<float>Εχ=new List<float>();
bool Βή=false;bool Βά=false;Vector3 ΗΜ=new Vector3();bool ƾ(){return ȫ!=0&&(DateTime.Now-ΘΖ).TotalSeconds<3;}bool ċ(int ǝ){return ƾ()&&ΘΗ==Ǐ&&ΘΘ==ǝ;}bool Ζμ(){return ƾ()||(DateTime.Now-ΗΝ).TotalSeconds<3;}void ΓЅ(int ǝ){if(ȫ==ǝ) return;ȫ=ǝ;ΗΝ=DateTime.Now;Δκ=3;
}void ΓЄ(){if(ȫ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ư,"D;"+Ǐ);ȫ=0;Αο=false;}void Ιό(){if(!trafficEnabled||Μ==Κ.ή) return;if(ΒЉ==null) ΒЉ=IGC.RegisterBroadcastListener(ư);int ȅ=20;while(ΒЉ.HasPendingMessage&&ȅ-->0){String Θ=ΒЉ.AcceptMessage().Data as String;if(Θ==null||!Θ.StartsWith("S;")) continue;
try{ΙΫ(Θ);}catch{}}if(Ϊ!=Α.И){Βή=false;Βά=false;}if(ȫ==1&&ж(MyShipConnectorStatus.Connected)!=null) ΓЄ();if(ȫ==2&&!Αο&&Ϊ!=Α.ǭ&&
Vector3.Distance(δ,ы().Ε)>ο*3) ΓЄ();if(ȫ!=0&&++Δκ>=3){Δκ=0;IGC.SendBroadcastMessage(ư,"R;"+Ǐ+";"+ȫ+";"+ѝ+";"+(ĭ?1:0));}}void ΙΫ(String Θ){String[]f=Θ.Split(';');if(f.Length<7) return;bool ΗΨ=false;String[]Ζυ=f[5].Split(',');for(int i=0;i<Ζυ.Length;i++) if(Ζυ[i]==Ǐ.ToString()) ΗΨ=true;
if(!ΗΨ) return;ΘΖ=DateTime.Now;ΘΗ=long.Parse(f[2]);ΘΘ=int.Parse(f[3]);ơ=long.Parse(f[4]);Αρ.Clear();Εχ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αρ.Add(π(e[0]));Εχ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗΠ=f[6].Split(':');List<Ǘ>Ѝ=new List<Ǘ>();for(int i=0;i<ΗΠ.Length;i++){String[]e=ΗΠ[i].Split(',');if(e.Length<4) continue;Ǘ p=new Ǘ();p.Φ=long.Parse(e[0]);p.κ=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);Ѝ.Add(p);}Ф=Ѝ;}bool Θτ(){if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return true;
ΓЅ(1);if(!Ζμ()) return true;if(!ċ(1)){Λ="Waiting for dock clearance";return false;}ΓΚ();return true;}bool ΓΚ(){ΑΒ=false;Ǘ Š=null;if(ơ!=0&&ơ!=ѝ&&ĭ)
for(int i=0;i<Ф.Count;i++) if(Ф[i].Φ==ơ) Š=Ф[i];long ƫ=Š==null?0:Š.Φ;if(ƫ==Ȳ&&(ƫ==0)==(ǟ==null)) return false;Ȳ=ƫ;ǟ=Š==null?null:ΓΩ(Š.κ,Š.Э,Š.Б);return true;}bool ų(Vector3 p){for(int i=0;i<Αρ.Count;i++) if(Vector3.Distance(p,Αρ[i])<Εχ[i]+ο*0.75f) return true;
return false;}bool ΙΚ(Vector3 ǋ,Vector3 ŝ){Βή=false;if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return false;if(ȫ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;ΓЅ(1);if(!ƾ()||ċ(1)||Αρ.Count==0) return false;if(!ų(ǋ)&&!ų(ŝ)&&!ų(δ)) return false;Βή=true;return true;
}bool ΕЂ(){if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return true;ΓЅ(2);if(!Ζμ()) return true;if(!ċ(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΙΥ=600f;class Ĉ{public long Φ;public int У=-1;public bool ƫ;public float ё,ǳ;public DateTime ΕΦ;}class ȟ{public String ȯ;
public int w,h;public float ΒБ,ΒЌ;public String Ϗ;public List<int>ΕΗ=new List<int>();public List<Ĉ>с=new List<Ĉ>();}List<ȟ>ȍ=new List<ȟ>();IMyBroadcastListener Βο=null;int Ηλ=0;void ΙЁ(){if(Βο==null) Βο=IGC.RegisterBroadcastListener(ΔΓ);int ȅ=50;while(Βο.HasPendingMessage&&ȅ-->0)
{String Θ=Βο.AcceptMessage().Data as String;if(Θ==null) continue;try{ΙΘ(Θ);}catch{}try{ΙΙ(Θ);}catch{}}for(int j=ȍ.Count-1;j>=0;j--){ȟ Υ=ȍ[j];for(int i=Υ.с.Count-1;i>=0;i--)
if((DateTime.Now-Υ.с[i].ΕΦ).TotalSeconds>ΙΥ) Υ.с.RemoveAt(i);if(Υ.с.Count==0){ȍ.RemoveAt(j);continue;}Θθ(Υ);}if(++Ηλ<3) return;Ηλ=0;for(int j=0;j<ȍ.Count;j++) IGC.SendBroadcastMessage(ΖΦ,Θλ(ȍ[j]));}void Θθ(ȟ Υ){for(int i=0;i<Υ.с.Count;i++){Ĉ s=Υ.с[i];if(s.ƫ&&(s.У<0||Υ.Ϗ[s.У]=='1')) s.У=ΙΧ(Υ,s);
}}String Θλ(ȟ Υ){String β="J;"+Υ.ȯ+";"+Υ.Ϗ+";";bool Ǜ=true;for(int i=0;i<Υ.с.Count;i++){if((DateTime.Now-Υ.с[i].ΕΦ).TotalSeconds>5) continue;β+=(Ǜ?"":",")+Υ.с[i].Φ+":"+Υ.с[i].У;Ǜ=false;}return β;}void ΙΘ(String Θ){String[]f=Θ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȟ Υ=null;for(int i=0;i<ȍ.Count;i++) if(ȍ[i].ȯ==f[4]) Υ=ȍ[i];if(Υ==null){Υ=new ȟ();Υ.ȯ=f[4];Υ.w=int.Parse(k[3]);Υ.h=int.Parse(k[4]);Υ.ΒБ=float.Parse(k[6]);Υ.ΒЌ=float.Parse(k[7]);if(Υ.w<1||Υ.h<1) return;Εϋ(Υ.w,Υ.h,(ю) int.Parse(k[5]),Υ.ΕΗ);
Υ.Ϗ=new String('0',Υ.w*Υ.h);ȍ.Add(Υ);}String d=f[6];if(d.Length==Υ.Ϗ.Length){char[]c=Υ.Ϗ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Υ.Ϗ=new String(c);}long Φ=long.Parse(f[0]);Ĉ s=null;for(int i=0;i<Υ.с.Count;i++) if(Υ.с[i].Φ==Φ) s=Υ.с[i];
if(s==null){s=new Ĉ();s.Φ=Φ;Υ.с.Add(s);}s.ΕΦ=DateTime.Now;String[]w=f[10].Split(',');s.ƫ=w[0]=="1";if(w.Length>=3){s.ё=float.Parse(w[1]);s.ǳ=float.Parse(w[2]);}int Ē=int.Parse(f[5]);if(Ē>=0&&Ē<Υ.Ϗ.Length&&(s.У<0||Υ.Ϗ[s.У]=='1')&&Υ.Ϗ[Ē]!='1'&&ΖΙ(Υ,Ē,s)==null)
s.У=Ē;}Ĉ ΖΙ(ȟ Υ,int У,Ĉ Δο){for(int i=0;i<Υ.с.Count;i++){Ĉ o=Υ.с[i];if(o!=Δο&&o.У>=0&&ΖΚ(У,o.У,Υ.w,Υ.ΒБ,Υ.ΒЌ,Math.Max(o.ё,Δο.ё),Math.Max(o.ǳ,Δο.ǳ)))
return o;}return null;}int ΙΧ(ȟ Υ,Ĉ s){for(int k=0;k<Υ.ΕΗ.Count;k++){int c=Υ.ΕΗ[k];if(Υ.Ϗ[c]!='1'&&ΖΙ(Υ,c,s)==null) return c;}return-2;}String ΘА(){String s="";for(int j=0;j<ȍ.Count;j++){ȟ Υ=ȍ[j];int Ϗ=0;for(int i=0;i<Υ.Ϗ.Length;i++) if(Υ.Ϗ[i]=='1') Ϗ++;s+="Job "+(j+1)+": "+Ϗ+"/"+Υ.Ϗ.Length+" holes, "+Υ.с.Count+" ships\n";
}return s;}const float Ις=8f;class ȣ{public long Φ;public Vector3 κ,ǔ,đ;public float Δζ,ё,ǳ,Ȏ=-1;public bool Ĳ;public String Υ="";public int ƕ;public DateTime Ĺ;}class ΒΕ{public long a,b,Θζ;public DateTime ΑΣ;}List<ȣ>Ш=new List<ȣ>();List<ΒΕ>ȳ=new List<ΒΕ>();int ΘΧ=0;void ΙΙ(String Θ)
{String[]f=Θ.Split(';');if(f.Length<9) return;long Φ=long.Parse(f[0]);ȣ s=null;for(int i=0;i<Ш.Count;i++) if(Ш[i].Φ==Φ) s=Ш[i];if(s==null){s=new ȣ();s.Φ=Φ;Ш.Add(s);}s.κ=π(f[1]);s.ǔ=π(f[2]);s.Δζ=float.Parse(f[3]);s.Υ=f[4];s.ƕ=int.Parse(f[7].Split(',')[1]);s.đ=π(f[8]);
s.Ĳ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ё=float.Parse(w[1]);s.ǳ=float.Parse(w[2]);s.Ĳ=w[3]=="1";}}s.Ĺ=DateTime.Now;}bool ΔΒ(ȣ s){return s.ǔ.LengthSquared()<0.25f&&(s.ƕ==3||s.đ.LengthSquared()<0.25f);}Vector3 Ζρ(ȣ s){return s.ǔ.LengthSquared()>=1f?s.ǔ:s.đ*2f;}
void ΙЉ(){Θϊ();if(++ΘΧ<3) return;ΘΧ=0;IGC.SendBroadcastMessage(Ζπ,Θν());}void Θϊ(){for(int i=Ш.Count-1;i>=0;i--)
if((DateTime.Now-Ш[i].Ĺ).TotalSeconds>3) Ш.RemoveAt(i);for(int i=ȳ.Count-1;i>=0;i--)
if((DateTime.Now-ȳ[i].ΑΣ).TotalSeconds>2) ȳ.RemoveAt(i);for(int i=0;i<Ш.Count;i++) Ш[i].Ȏ=-1;for(int i=0;i<Ш.Count;i++)
for(int j=i+1;j<Ш.Count;j++){ȣ a=Ш[i],b=Ш[j];if(ΔΒ(a)||ΔΒ(b)) continue;Vector3 Κϋ=a.κ+a.ǔ*(float)(DateTime.Now-a.Ĺ).TotalSeconds;Vector3 ΚϏ=b.κ+b.ǔ*(float)(DateTime.Now-b.Ĺ).TotalSeconds;Vector3 Ʀ=Ζρ(a),Ƨ=Ζρ(b);Vector3 ǡ=ΚϏ-Κϋ;float ϊ=ǡ.Length();float Ъ=(a.Δζ+b.Δζ)*0.55f;
if(a.Ĳ&&b.Ĳ&&a.Υ!=""&&a.Υ==b.Υ&&a.ё>0&&b.ё>0)
Ъ=Math.Max(a.ё+b.ё,a.ǳ+b.ǳ)*0.5f+0.5f;Vector3 ΕΤ=Ƨ-Ʀ;float ΗЎ=ΕΤ.LengthSquared();float t=ΗЎ>0.01f?-Vector3.Dot(ǡ,ΕΤ)/ΗЎ:0;bool Εθ=ϊ<Ъ;if(!Εθ&&(t<=0||t>Ις||(ǡ+ΕΤ*t).Length()>=Ъ)) continue;if(!Εθ&&Ʀ.LengthSquared()>0.25f&&Ƨ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ʀ),Vector3.Normalize(Ƨ))<-0.7f)
continue;ΒΕ p=null;for(int k=0;k<ȳ.Count;k++)
if((ȳ[k].a==a.Φ&&ȳ[k].b==b.Φ)||(ȳ[k].a==b.Φ&&ȳ[k].b==a.Φ)) p=ȳ[k];if(p==null){p=new ΒΕ();p.a=a.Φ;p.b=b.Φ;p.Θζ=Ισ(a,b,Ʀ,Ƨ,ǡ,ϊ);ȳ.Add(p);}p.ΑΣ=DateTime.Now;ȣ y=p.Θζ==a.Φ?a:b;float Ηρ=Εθ||t<4?0:Math.Max(1,y.ǔ.Length()*0.5f);if(y.Ȏ<0||Ηρ<y.Ȏ) y.Ȏ=Ηρ;
}}long Ισ(ȣ a,ȣ b,Vector3 Ʀ,Vector3 Ƨ,Vector3 ǡ,float ϊ){if(a.Φ==Ǚ) return b.Φ;if(b.Φ==Ǚ) return a.Φ;Vector3 n=ϊ>0.01f?ǡ/ϊ:new Vector3(1,0,0);float ΘΚ=Vector3.Dot(Ʀ,n),ΘΦ=Vector3.Dot(Ƨ,-n);if(ΘΚ-ΘΦ>1) return a.Φ;if(ΘΦ-ΘΚ>1) return b.Φ;return a.Φ>b.Φ?a.Φ:b.Φ;
}String Θν(){String β="C;";bool Ǜ=true;for(int i=0;i<Ш.Count;i++){if(Ш[i].Ȏ<0) continue;β+=(Ǜ?"":",")+Ш[i].Φ+":"+Math.Round(Ш[i].Ȏ,1);Ǜ=false;}return β;}String ΙΔ(){int ΗЀ=0,ΑΝ=0;for(int i=0;i<Ш.Count;i++){if(!ΔΒ(Ш[i])) ΗЀ++;if(Ш[i].Ȏ>=0) ΑΝ++;}if(Ш.Count==0) return "";
return "Traffic: "+ΗЀ+" moving"+(ΑΝ>0?", "+ΑΝ+" giving way":"")+"\n";}