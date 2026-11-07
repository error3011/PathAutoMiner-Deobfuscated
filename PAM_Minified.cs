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
public enum Ι{Ġ,Ʈ,ǣ,і,έ}public enum Α{ы,ȱ,Ū,Ș,ļ,И,с,
Ğ,ť,Ǹ,ĸ,Ĵ,Ě,
ư,ț,Ǧ,ķ,ΑЌ,Αξ,
ΓЇ,ΑЋ,Ʀ,Απ,ġ}public enum Π{ь,ū,В,ΓΚ,Ακ,Αϋ,Αό,Š,š}public enum ѕ{None,ķ,ы,Εω}public enum Σ{Main,ΖΥ,ƭ,Ƭ,ΓЂ,Αθ,
ΒЊ,ΕЏ,ΕА,ΕЋ,Ʋ,ΖΪ,
ΑЅ,Γω,Γύ,Ζά,ΑЇ}public enum у{ΑЄ,Ƥ,ΙΡ,ΖΒ,έ}public enum э{ƴ,Ες}public enum щ{ΑЃ,ΓΠ,Ζί,ƨ}public enum Г{Γι,Εφ,ΓΕ,ΓΛ,Αμ,ΓΫ,Αφ}public enum Ǒ{ΓΗ,ΒЏ,ΒА}public enum Ш{Ũ,ΘЂ,ĵ}public enum π{Γλ,Γο,Γπ,ΖΟ,ΖΞ,
Γκ,ΑЀ,ΑϏ,Γν,
Γξ,Γμ}public enum ε{ȓ,Ā,Ȓ,Αη,ΒЍ,ΒЎ}public enum ǥ{Ασ,Αζ,Θλ}public enum ї{ΑЈ,Εϋ,ΘЌ,Γθ}public enum Ј{None,Ŭ,Ư,Αώ}public enum ϊ{ƶ,Αε,Αω,ƨ,Γζ,Αϊ,Γη,ΙΧ,Γρ}public enum С{Ġ,ΖΛ,ΖΚ,Γγ,Γέ,ΓΖ,ΖΜ}class Ϋ{public bool κ=false;public Vector3 Ε=new Vector3();
public Vector3 θ=new Vector3();public Vector3 ι=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 φ=new Vector3();public Vector3 м=new Vector3();public float Ч=0;public float ΛΘ=0;public float[]Ƕ=null;public Ϋ(){}public Ϋ(Ϋ γ){κ=γ.κ;Ε=γ.Ε;θ=γ.θ;ι=γ.ι;Ђ=γ.Ђ;φ=γ.φ;
м=γ.м;Ƕ=γ.Ƕ;}public Ϋ(Vector3 Ε,Vector3 ι,Vector3 θ,Vector3 Ђ,Vector3 φ){this.Ε=Ε;this.θ=θ;this.ι=ι;this.Ђ=Ђ;this.Ч=0;this.φ=φ;}public void ΘΧ(List<IMyThrust>ΑЏ,List<string>ȫ){Ƕ=new float[ȫ.Count];for(int i=0;i<Ƕ.Length;i++)
Ƕ[i]=-1;for(int i=0;i<ΑЏ.Count;i++){string ǐ=ΕЉ(ΑЏ[i]);int Β=ȫ.IndexOf(ǐ);if(Β!=-1)
Ƕ[Β]=Й(ΑЏ[i].MaxEffectiveThrust,ΑЏ[i].MaxThrust);}}}class Γς{public Ϋ η=null;public List<Vector3>ƚ=new List<Vector3>();public float ǁ=0;public float Εΰ=0;public float ų=0;public float Δί=0;public Vector3 ΒΥ=new Vector3();}class ů{public π Р=π.Γλ;public float Ǫ=0;public float Ĩ=0;
public string ǫ="";public string Ǯ="";DateTime Ηή;public bool ΔА=false;private bool ΔΡ=false;public bool ΓΒ(bool ǳ){if(ǳ){Ĩ=0;ΔΡ=false;return false;}ΔΡ=true;return Ĩ>Ǫ;}public void ΙΒ(){ΓΒ(true);ΔА=false;}public void Ιη(){if(ΔΡ)
if((DateTime.Now-Ηή).TotalSeconds>1){Ĩ++;Ηή=DateTime.Now;}}public bool ΓΨ(){switch(Р){case π.Γο:return true;case π.Γπ:return true;}return false;}}class к{public String ί="";public String υ="";public int Љ=0;public Ш ƃ=Ш.ĵ;public к(String ί,String υ,int Љ,Ш ƃ){this.ί=ί;this.υ=υ;
this.Љ=Љ;this.ƃ=ƃ;}}class Θζ{public Θζ(Vector3 ν,float Њ){this.ν=ν;this.Њ=Њ;}public Vector3 ν;public float Њ;}class Є{public DateTime Δτ;public DateTime Ζό;public String ΰ="";public String ί="";public String ΒЇ="";public String Λ="";public String ΕΞ="";public String Βμ="";public String ΔЎ="";
public Vector3 Ε=new Vector3();public List<к>Ћ=new List<к>();public Ι Κ=Ι.Ġ;public Ј ģ;public float ȇ;public float ȁ;public int Ў;public int ΔΧ=0;public int ΕΫ=0;public bool ΖΖ(){return(DateTime.Now-Δτ).TotalSeconds>10;}public bool Γή(){if(ģ!=Ј.Ŭ) return false;return(DateTime.Now-Ζό).TotalSeconds>=4;
}public Є(String ΰ){this.ΰ=ΰ;}public Ј Ļ(String Ѕ,bool Κξ,bool ф,bool Ιφ){if(Ѕ==""&&!ф) return Ј.None;if(ģ==Ј.Ŭ&&Γή())
ģ=Ј.Αώ;if(Κξ){ΔЎ=Ѕ;ģ=Ј.Ŭ;Ζό=DateTime.Now;return ģ;}else if(ф){ΔЎ="";ģ=Ј.None;}else if(ΔЎ==Ѕ){if(Ιφ) ģ=Ј.Ư;return ģ;}return Ј.None;}}public String Θσ(String ΚΘ){switch(ΚΘ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Βσ=new Vector3();Vector3 δ=new Vector3();Vector3 ΒΨ=new Vector3();Vector3 ΑΗ=new Vector3();Vector3 Ǆ=new Vector3();
DateTime Δχ=new DateTime();bool ć=true;int ΕΥ=0;bool č=false;bool Ɓ=false;bool ΑΜ=true;bool ΘΝ=false;bool ƀ=false;bool Βέ=false;float Ĕ=0;float Ś=0;int İ=0;int Αή=0;int ΑΠ=0;int ΔΗ=0;float Ɔ=0;float Ŵ=0;double ΔЃ=0;float ƻ=0;List<int>Ž=new List<int>();List<int>Ƒ=new List<int>();
Ι Κ=Ι.Ġ;int А=0;float Ȑ=0;float ȏ=0;float ο=0;float Ɩ=0,ΑΣ=0;IMyRemoteControl Ϊ;IMySensorBlock ψ;List<IMyTimerBlock>ΕΨ=new List<IMyTimerBlock>();List<IMyShipConnector>ј=new List<IMyShipConnector>();List<IMyThrust>Ȭ=new List<IMyThrust>();List<IMyGyro>ΒΩ=new List<IMyGyro>();List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȩ=new List<IMyLandingGear>();List<IMyReactor>į=new List<IMyReactor>();List<IMyConveyorSorter>Βψ=new List<IMyConveyorSorter>();List<IMyGasTank>ǖ=new List<IMyGasTank>();List<IMyTerminalBlock>Ʒ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ŷ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ł=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ģ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ц=new List<IMyTextPanel>();List<IMyTextSurface>Ȍ=new List<IMyTextSurface>();List<IMyTextPanel>ņ=new List<IMyTextPanel>();IMyTerminalBlock ǚ=null;Ϋ Ο=new Ϋ();Ϋ Δν=new Ϋ();
Ϋ Δρ=new Ϋ();bool ȭ=false;bool Ɯ=false;bool Ɲ=false;bool Βί=false;List<String>ȫ=new List<string>();List<Ϋ>Φ=new List<Ϋ>();List<Ϋ>Αγ=new List<Ϋ>();int ΔΨ=0;double ΘΞ=0;Ϋ Ζ=new Ϋ();Vector3 Ǳ;Vector3 đ;Π β=Π.ь;э Ŏ=э.ƴ;int Ќ=0;int Ь=0;double ΑΕ=0;bool ΒΚ=false;ů І=new ů();
ů Ї=new ů();щ ё=щ.ΑЃ;bool Ĳ=true;э Ȋ=э.ƴ;int я=3;int ю=3;int л=30;Ǒ љ=Ǒ.ΓΗ;bool ę=true;bool ſ=true;bool Ź=true;bool ś=false;Г б=Г.Γι;bool Ę=true;float Ȇ=90;float ѝ=20;float н=5;float ў=20;float ǻ=0.70f;float Ƣ=1.50f;float ơ=2.50f;float ş=0f;float Ŋ=0f;Α Ω;
Π ќ;Α ΑΧ;Γς μ=null;int Ϗ=0;int ό=0;int Я=0;int ď=0;int Ř=30;int и=0;bool Е=true;Vector3 џ;double а=0;bool Βυ=false;int Εγ=0;int Ж=0;int ΓЎ=0;int Ȉ=0;int Ηυ=0;Vector3 ΑΞ=new Vector3();float ѐ=0;float р=0;int Ş=0;Ϋ Βω=null;float Δυ=0;float ΑБ=0;float ж=0;float ő=0;
bool Δο=false;bool Ŝ=false;bool Αί=false;bool ċ=false;bool ă=false;DateTime ΙЇ=new DateTime();Ϋ ń=null;Ϋ ΖЄ=null;float ȝ=0;ѕ Ǽ;float Č=0;float ŝ=0;String Δώ="";float Βγ=0;float ȁ=0;float ȇ=0;List<к>ǭ=new List<к>();float в=0;bool ΑΤ=false;bool ΘΒ=false;bool ΗЂ=false;
bool Βώ=false;bool ΗЃ=false;float ΔЁ=0;float ΔЍ=0;Vector3 ΗΖ=new Vector3();Vector3 Ѐ=new Vector3();Vector3 ΒϏ=new Vector3(1,1,1);float ΕΤ=1;Vector3 ΑΫ=new Vector3();bool Βή=false;bool Δε=false;bool Δϋ=false;bool Ζθ=false;float ΒΘ=0;float ΑЎ=2;Vector3 ΘΑ;Vector3 ΕΣ;Vector3 ΕΡ;
float[,]Ƿ=new float[3,2];Dictionary<String,float[,]>Ǟ=new Dictionary<string,float[,]>();Dictionary<String,float>ΘΛ=new Dictionary<String,float>();int ΔΑ=0;Dictionary<String,float[]>Ē=new Dictionary<string,float[]>();float ΔБ;bool ΒΛ=false;int ΑА=0;int Ĭ=0;int ΒΫ=0;int Ō=0;
String Λ="";int[]Ημ=new int[Enum.GetValues(typeof(Σ)).Length];bool ΔЅ=false;int Œ=0;int Ў=0;Σ ϋ=Σ.Main;int Κό=0,Κύ=0,Κώ=0,ΚϏ=0;int Κι=0,Κκ=0;bool Ŷ=false;bool Ζι=false;String Ɨ="";String ΔΖ="";IMyBroadcastListener Ǩ=null;bool ΗΕ=true;List<Є>Ъ=null;Є Л=null;
int ΒΔ=0,ΒΕ=0;int ΖϏ=0,ΖЀ=0;int Ιό=0,Ιύ=0,Ιώ=0;String ΚΚ="";Exception Ď=null;bool Βτ=false;Π Ɠ=Π.ь;bool ΖЇ=false;bool ΗΫ=true;bool ΕΕ=false;String ΘΜ="";bool Αέ=false;bool ΕΝ=true;int Βά=0;int[]ǜ=null;Vector3 ΔΘ=new Vector3();bool ΗΛ=false;bool ǒ(IMyTerminalBlock Ψ)=>Ψ.CubeGrid==Me.CubeGrid;
Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓΩ(Me,controllerTag,true))
Κ=Ι.і;λ(Σ.Main);if(Κ!=Ι.і){Λ="Welcome to [PAM]!";ї ΕΔ=ΙΓ();if(Κ==Ι.Ġ){List<IMyShipDrill>ΗΒ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗΜ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗΒ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗΜ,q=>q.CubeGrid==Me.CubeGrid);if(ΗΒ.Count>0)
{Κ=Ι.Ʈ;Λ="Miner mode enabled!";}else if(ΗΜ.Count>0){Κ=Ι.ǣ;Λ="Grinder mode enabled!";}else{Κ=Ι.έ;ș(у.έ);}}if(ΕΔ==ї.ΑЈ) ΑΜ=false;if(ΕΔ==ї.Εϋ) Λ="Data restore failed!";if(ΕΔ==ї.Γθ) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΚЇ){try{if(Ď!=null){if(Т.Trim().ToUpper()=="RETRY")
{Ď=null;ć=true;İ=0;if(Κ!=Ι.і) Ё();Λ="Restarted after error";}else{Θύ();ΖΣ();return;}}Ɓ=(ΚЇ&UpdateType.Update10)!=0;if(Ɓ) ΕΥ++;č=ΕΥ>=10;if(č) ΕΥ=0;if(Ɓ){ΑА++;if(ΑА>4) ΑА=0;ΑΠ=(int) Math.Max(0,10-(DateTime.Now-Δχ).TotalSeconds);}if(Т!="") ΚΚ=Т;if(Κ!=Ι.і)
ΙΠ(Т);else
Θδ(Т);ΔЅ=false;try{int ΖЅ=Runtime.CurrentInstructionCount;float ŗ=Й(ΖЅ,Runtime.MaxInstructionCount);if(ŗ>0.90) Λ="Max. instructions >90%";if(ŗ>Ɔ) Ɔ=ŗ;if(ΒΛ){Ž.Add(ΖЅ);while(Ž.Count>10) Ž.RemoveAt(0);Ŵ=0;for(int i=0;i<Ž.Count;i++) Ŵ+=Ž[i];Ŵ=Й(Й(Ŵ,Ž.Count),Runtime.MaxInstructionCount);
double Δφ=Runtime.LastRunTimeMs;if(Βέ&&Δφ>ΔЃ) ΔЃ=Δφ;Ƒ.Add((int)(Δφ*1000f));while(Ƒ.Count>10) Ƒ.RemoveAt(0);ƻ=0;for(int i=0;i<Ƒ.Count;i++) ƻ+=Ƒ[i];ƻ=Й(ƻ,Ƒ.Count)/1000f;}}catch{Ɔ=0;}}catch(Exception e){Ď=e;}}void ΙΠ(string Т){bool Βφ=false;String Ζσ="";if(А<=1&&!Γα(Т))
ΖΤ(Т);int ΔІ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔІ-->0){MyIGCMessage ζ=Ǩ.AcceptMessage();String Ξ=ζ.Data as String;if(Ξ==null) continue;String ȉ="";if(А<=1&&ΖΡ(ref Ξ,out ȉ,out Ζσ)&&ȉ==BROADCAST_SELF_ID){ΖΤ(Ξ);Βφ=true;break;}}bool ΗΥ=Βέ&&Ω==Α.ы&&!ƀ&&!Βφ&&İ==0&&!Βί;
if(č&&Ω!=Α.ы) Βφ=true;if((Ɓ&&!ΗΥ)||(č&&ΗΥ)){if(İ==0&&(ΑΠ<=0||ć)){ΘΝ=А>0;А=0;İ=1;Ț();ΙΗ();ΕБ();Ȕ("Scan 1");}else if(İ==1){İ=2;Ț();ΙΘ();Ȕ("Scan 2");}else if(İ==2){İ=0;Ț();ΙΙ();ΙΚ();if(А<=1) ΖΙ();Ȕ("Scan 3");Δχ=DateTime.Now;if(А<=1&&ΑΜ)
Βσ=Ατ(Ϊ,Ϊ.CenterOfMass);ΑΜ=false;if(ć){ć=false;bool ΚЋ=β==Π.В;Ё();if(ΗΫ&&resumeAfterRestart&&ΚЋ&&А<=1) ΕΕ=true;}ΗΫ=false;if(ΘΝ&&А==0) Λ="Setup complete";}else{if(β==Π.В&&Κ!=Ι.έ){Ț();ΘΠ();Ȕ("Inv balance");}Ț();switch(Αή){case 0:ΙΫ();break;case 1:Ια();break;case 2:ΙΪ();break;
case 3:Ικ();break;case 4:Ιΰ();break;case 5:ΘΪ();break;case 6:ΘΩ(Ϊ);break;}Ȕ("Update: "+Αή);Αή++;if(Αή>6){Αή=0;Βέ=true;if(Ɠ!=Π.ь){switch(Ɠ){case Π.Š:Ĝ();break;case Π.š:Ĝ();break;case Π.В:Ĝ();break;case Π.Αϋ:Αρ();break;case Π.Αό:Ĺ();break;}Ɠ=Π.ь;}}}if(!ć){if(!Ȗ(Ϊ,true))
{Ϊ=null;ΑΜ=true;А=2;}if(А>=2&&Ω!=Α.ы)
Ё();if(А<=1){Ś=Ϊ.CalculateShipMass().PhysicalMass;Ĕ=(float) Ϊ.GetShipSpeed();δ=ΙΩ(Ϊ,Βσ);ΒΨ=Ϊ.WorldMatrix.Forward;ΑΗ=Ϊ.WorldMatrix.Left;Ǆ=Ϊ.WorldMatrix.Down;Ιε();Ιθ();Ιέ();if(ΕΕ){ΕΕ=false;Ĝ();if(Ω!=Α.ы) Λ="Job resumed after restart";}if(Ω!=Α.ы){ƀ=false;ΖΧ(false);Επ(false);String Κσ=ΓΤ(Ω)+" "+(int) Ω;
Ț();Ιγ();ΓІ(false);Ȕ(Κσ);Ιι();Ιδ();Ț();Ιί();Ȕ("Thruster");Ț();Ζδ();Ȕ("Gyroscope");}else{if(ƀ){if(Αψ()){й(Ǆ,ΒΨ,ΑΗ,0.25f,true);Ζδ();Λ="Aligning to planet: "+Math.Round(ΒΘ-0.25f,2)+"°";if(Βή) ΓΧ(true,true);}else ΓΧ(true,true);}}Αέ=false;}}}Ț();ΖΣ();Ȕ("Print");if(Βφ||ΔΗ<=0){Ț();ΘΡ(Ζσ);
Ȕ("Broadcast");ΔΗ=4;}else if(č) ΔΗ--;}void ΖΤ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ȝ=τ.ElementAtOrDefault(1);var ƺ=τ.ElementAtOrDefault(2);var Ζλ=τ.ElementAtOrDefault(3);String Ȧ="Invalid argument: "+Т;bool Εί=false;
switch(Ѕ){case "UP":this.Γε(false);break;case "DOWN":this.Γδ(false);break;case "UPLOOP":this.Γε(true);break;case "DOWNLOOP":this.Γδ(true);break;case "APPLY":this.Εξ(true);break;case "MRES":Ў=0;break;case "STOP":this.Ё();break;case "PATHHOME":{this.Ё();this.ΓЁ();}break;case "PATH":{this.Ё();this.ΓЁ();Ο.κ=true;}break;
case "START":{this.Ё();ΓЀ();}break;case "ALIGN":{ΓΧ(!ƀ,false);}break;case "CONT":{this.Ё();this.Ĝ();}break;case "JOBPOS":{this.Ё();this.Ĺ();}break;case "HOMEPOS":{this.Ё();this.Αρ();}break;case "FULL":{ΑΤ=true;}break;case "RESET":{Βτ=true;А=2;}break;case "EXPORT":{Θμ();}break;
case "IMPORT":{ΘЀ(Ȝ=="JOB");}break;default:Εί=true;break;}if(Κ!=Ι.έ){switch(Ѕ){case "SHUTTLE":{Θι();}break;case "CFGS":{if(!Θβ(Ȝ,ƺ,Ζλ)) Λ=Ȧ;}break;case "CFGB":{if(!Θα(Ȝ,ƺ)) Λ=Ȧ;}break;case "CFGL":{if(!ĺ(ref Ȇ,true,ϊ.Αω,Ȝ,"")||!Θγ(ƺ))
Λ=Ȧ;}break;case "CFGE":{if(!ĺ(ref н,true,ϊ.Αϊ,Ȝ,"IG")||
!ĺ(ref ѝ,true,ϊ.Γζ,ƺ,"IG")||
!ĺ(ref ў,true,ϊ.Γη,Ζλ,"IG"))
Λ=Ȧ;}break;case "CFGA":{if(!ĺ(ref ǻ,false,ϊ.Αε,Ȝ,"")) Λ=Ȧ;}break;case "CFGW":{if(!ĺ(ref Ƣ,false,ϊ.ƶ,Ȝ,"")||
!ĺ(ref ơ,false,ϊ.ƶ,ƺ,""))
Λ=Ȧ;}break;case "NEXT":{ΒЉ(false);}break;case "PREV":{ΒЉ(true);}break;default:if(Εί) Λ=Ȧ;break;}}else{switch(Ѕ){case "UNDOCK":{Αέ=true;}break;default:if(Εί) Λ=Ȧ;break;}}}String Θτ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Κ!=Ι.έ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Θι(){Ё();Κ=Ι.έ;ș(у.έ);Ζ.κ=false;Ο.κ=false;ψ=null;ω.Clear();β=Π.ь;}void ΓΧ(bool М,bool ώ){if(!М) Λ="Aligning canceled";if(ώ) Λ="Aligning done";if(ώ||!М){ƀ=false;Ζΰ();ΑІ(false,0,0,0,0);return;}if(Αψ()) ƀ=true;}bool Θβ(String ΚЍ,String ΙБ,String ΙЂ)
{bool ΚЉ=β==Π.В;int w,h,d;if(int.TryParse(ΚЍ,out w)&&int.TryParse(ΙБ,out h)&&int.TryParse(ΙЂ,out d)){this.Ё();я=w;ю=h;л=d;ŀ(false);Ƶ(false,false);if(ΚЉ) Ĝ();return true;}return false;}bool Θγ(String ƹ){if(ƹ=="ON"){ę=true;return true;}if(ƹ=="OFF"){ę=false;return true;}
return false;}bool Θα(String ΗΑ,String ΒΙ){bool ȩ=true;if(ΗΑ=="HOME") Ĳ=true;else if(ΗΑ=="STOP") Ĳ=false;else ȩ=false;if(ΒΙ=="HOME") ё=щ.ΑЃ;else if(ΒΙ=="STOP") ё=щ.Ζί;else if(ΒΙ=="JOB") ё=щ.ΓΠ;else if(ΒΙ=="IG") ё=щ.ƨ;else ȩ=false;return ȩ;}void λ(Σ х){Ημ[(int) ϋ]=Ў;
Ў=Ημ[(int) х];if(х==Σ.Ʋ) Ў=0;ϋ=х;if(Κ!=Ι.і)
Ŀ(ϋ==Σ.ƭ,false,0,0);ΔЅ=true;}void Γε(bool Δό){if(Ў>0) Ў--;else if(Δό) Ў=Œ-1;}void Γδ(bool Δό){if(Ў<Œ-1) Ў++;else if(Δό) Ў=0;}String ș(у ζ){switch(ζ){case у.ΑЄ:Λ="Job is running";break;case у.Ƥ:Λ="Connector not ready!";break;case у.ΙΡ:Λ="Ship modified, path outdated!";break;
case у.ΖΒ:Λ="Interrupted by player!";break;case у.έ:Λ="Shuttle mode enabled!";break;}return "";}String Θϊ(э ν){switch(ν){case э.ƴ:return "Top-Left";case э.Ες:return "Center";default:return "";}}String Θπ(Ǒ Џ){switch(Џ){case Ǒ.ΒЏ:return "Auto"+(Κ==Ι.Ʈ?" (Ore)":"");case Ǒ.ΒА:return "Auto (+Stone)";
case Ǒ.ΓΗ:return "Default";default:return "";}}String ΓΡ(Π Ѓ){switch(Ѓ){case Π.ь:return "No job";case Π.ū:return "Job paused";case Π.В:return "Job active";case Π.Š:return "Job active";case Π.š:return "Job active";case Π.ΓΚ:return "Job done";case Π.Ακ:return "Job changed";case Π.Αϋ:return "Move home";
case Π.Αό:return "Move to job";}return "";}String ΕЀ(щ Ιυ){switch(Ιυ){case щ.ΑЃ:return "Return home";case щ.ΓΠ:return "Fly to job pos";case щ.Ζί:return "Stop";case щ.ƨ:return "Ignore";}return "";}String Θς(Г Џ){switch(Џ){case Г.Γι:return "Off";case Г.ΓΛ:return "Drop pos (Stone) ";case Г.Αμ:return "Drop pos (Sto.+Ice)";
case Г.Εφ:return "Cur. pos (Stone)";case Г.ΓΕ:return "Cur. pos (Sto.+Ice)";case Г.ΓΫ:return "In motion (Stone)";case Г.Αφ:return "In motion (Sto.+Ice)";}return "";}String Εώ(ѕ Ѓ){switch(Ѓ){case ѕ.None:return "No batteries";case ѕ.ķ:return "Charging";case ѕ.Εω:return "Discharging";}return "";}
String ΓΤ(Α Ѓ){String ΛΖ=Κ==Ι.έ?"target":"job";switch(Ѓ){case Α.ы:return "Idle";case Α.ȱ:return "Flying to XY position";case Α.Ū:return Κ==Ι.ǣ?"Grinding":"Mining";case Α.Ș:return "Returning";case Α.Ʀ:return "Flying to drop pos";case Α.ļ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.с:return "Flying to job area";case Α.Ğ:return "Flying to path";case Α.ť:return "Flying to job position";case Α.Ǹ:return "Approaching dock";case Α.ĸ:return "Docking";case Α.Ĵ:return "Aligning";case Α.Ě:return "Aligning";case Α.ư:return "Retry docking";case Α.ț:return "Unloading";case Α.ġ:return ΘΜ;
case Α.Ǧ:return "Undocking";case Α.ķ:return "Charging batteries";case Α.ΑЌ:return "Waiting for uranium";case Α.Αξ:return "Filling up hydrogen";case Α.ΓЇ:return "Waiting for ejection";case Α.ΑЋ:return "Waiting for ejection";case Α.Απ:return "Flying to drop pos";}return "";}String ΕЊ(π Р){switch(Р)
{case π.Γλ:return "On \"Undock\" command";case π.Γο:return "On player entered cockpit";case π.ΖΟ:return "On ship is full";case π.ΖΞ:return "On ship is empty";case π.Γπ:return "On time delay";case π.ΑЀ:return "On batteries empty(<25%)";case π.ΑϏ:return "On batteries empty(=0%)";case π.Γκ:return "On batteries full";
case π.Γξ:return "On hydrogen empty(<25%)";case π.Γμ:return "On hydrogen empty(=0%)";case π.Γν:return "On hydrogen full";}return "";}bool Μ(ref String Γ,int Ηώ,int ī,bool Θ,String ŏ){Œ+=1;if(Ηώ==ī) ŏ=">"+ŏ+(ΑА>=2?" .":"");else ŏ=" "+ŏ;Γ+=ŏ+"\n";return Ηώ==ī&&Θ;}String Εξ(bool Θ)
{int Β=0;int Η=Ў;Œ=0;String ƕ="———————————————\n";String ƛ="--------------------------------------------\n";String Γ="";Γ+=ΓΡ(β)+" | "+(Ο.κ?"Ready to dock":"No dock")+"\n";Γ+=ƕ;double ȋ=Math.Max(Math.Round(this.а),0);if(ϋ==Σ.Main){bool ĭ=Κ==Ι.έ;if(Μ(ref Γ,Η,Β++,Θ," Record path & set home")) ΓЁ();
if(Κ==Ι.Ʈ)
if(Μ(ref Γ,Η,Β++,Θ," Setup mining job")) λ(Σ.ƭ);if(Κ==Ι.ǣ)
if(Μ(ref Γ,Η,Β++,Θ," Setup grinding job")) λ(Σ.ƭ);if(Κ==Ι.έ)
if(Μ(ref Γ,Η,Β++,Θ," Setup shuttle job")) λ(Σ.Γύ);if(Μ(ref Γ,Η,Β++,Θ," Continue job")) Ĝ();if(Μ(ref Γ,Η,Β++,Θ," Fly to home position")) Αρ();if(Μ(ref Γ,Η,Β++,Θ," Fly to job position")) Ĺ();if(Μ(ref Γ,Η,Β++,Θ," Behavior settings"))
if(ĭ) λ(Σ.ΑЇ);else λ(Σ.Αθ);if(Μ(ref Γ,Η,Β++,Θ," Info")) λ(Σ.ΕЏ);if(Κ!=Ι.έ)
if(Μ(ref Γ,Η,Β++,Θ," Help")) λ(Σ.ΕЋ);}else if(ϋ==Σ.ƭ){double ΚЌ=Math.Round(я*Ȑ,1);double ΙА=Math.Round(ю*ȏ,1);String З="";if(Μ(ref З,Η,Β++,Θ," Start new job!")) ΓЀ();if(Μ(ref З,Η,Β++,Θ," Change current job")){Ƶ(false,false);λ(Σ.Main);}if(Μ(ref З,Η,Β++,Θ," Width + (Width: "+я+" = "+ΚЌ+"m)")){Ţ(ref я,5,20,1);ŀ(true);}
if(Μ(ref З,Η,Β++,Θ," Width -")){Ţ(ref я,-5,20,-1);ŀ(true);}if(Μ(ref З,Η,Β++,Θ," Height + (Height: "+ю+" = "+ΙА+"m)")){Ţ(ref ю,5,20,1);ŀ(true);}if(Μ(ref З,Η,Β++,Θ," Height -")){Ţ(ref ю,-5,20,-1);ŀ(true);}if(Μ(ref З,Η,Β++,Θ," Depth + ("+(љ==Ǒ.ΓΗ?"Depth":"Min")+": "+л+"m)")){Ţ(ref л,5,50,2);ŀ(true);}
if(Μ(ref З,Η,Β++,Θ," Depth -")){Ţ(ref л,-5,50,-2);ŀ(true);}if(Μ(ref З,Η,Β++,Θ," Depth mode: "+Θπ(љ))){љ=ĝ(љ);}if(Μ(ref З,Η,Β++,Θ," Start pos: "+Θϊ(Ȋ))){Ȋ=ĝ(Ȋ);}if(Κ==Ι.ǣ&&љ==Ǒ.ΒА) љ=ĝ(љ);Γ+=Γχ(8,З,Η,ref Κι);}else if(ϋ==Σ.Γύ){float[]ΖЉ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Μ(ref Γ,Η,Β++,Θ," Next")) λ(Σ.Ζά);if(Μ(ref Γ,Η,Β++,Θ," Back")) λ(Σ.Main);Γ+=" Leave connector 1:\n";if(Μ(ref Γ,Η,Β++,Θ," - "+ΕЊ(І.Р))) І.Р=ĝ(І.Р);if(!І.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Β++,Θ," - Delay: "+Ας((int) І.Ǫ))) І.Ǫ=Εχ(І.Ǫ,ΖЉ);Γ+=" Leave connector 2:\n";if(Μ(ref Γ,Η,Β++,Θ," - "+ΕЊ(Ї.Р))) Ї.Р=ĝ(Ї.Р);
if(!Ї.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Β++,Θ," - Delay: "+Ας((int) Ї.Ǫ))) Ї.Ǫ=Εχ(Ї.Ǫ,ΖЉ);}else if(ϋ==Σ.Ζά){if(Μ(ref Γ,Η,Β++,Θ," Start job!")) ΓЀ();if(Μ(ref Γ,Η,Β++,Θ," Back")) λ(Σ.Γύ);Γ+=" Timer: \"Docking connector 1\":\n";if(Μ(ref Γ,Η,Β++,Θ," = "+(І.ǫ!=""?І.ǫ:"-"))) І.ǫ=Αύ(ref Κό);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Μ(ref Γ,Η,Β++,Θ," = "+(І.Ǯ!=""?І.Ǯ:"-"))) І.Ǯ=Αύ(ref Κώ);Γ+=" Timer: \"Docking connector 2\":\n";if(Μ(ref Γ,Η,Β++,Θ," = "+(Ї.ǫ!=""?Ї.ǫ:"-"))) Ї.ǫ=Αύ(ref Κύ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Μ(ref Γ,Η,Β++,Θ," = "+(Ї.Ǯ!=""?Ї.Ǯ:"-"))) Ї.Ǯ=Αύ(ref ΚϏ);
}else if(ϋ==Σ.Ƭ){String Ηβ=Ȇ+" %";if(č) Ĭ++;if(Ĭ>1){Ĭ=0;ΒΫ++;if(ΒΫ>1) ΒΫ=0;}bool[]ΕΚ=new bool[]{į.Count==0,Ǽ==ѕ.None,ǖ.Count==0};int ΘΗ=0;while(true){ΘΗ++;Ō++;if(Ō>ΕΚ.Length-1) Ō=0;if(ΘΗ>=ΕΚ.Length) break;if(!ΕΚ[Ō]) break;}bool ĭ=Κ==Ι.έ;if(!ĭ&&ę&&в!=-1&&ΒΫ==0)
Ηβ=в<1000000?Math.Round(в)+" Kg":Math.Round(в/1000)+" t";if(Μ(ref Γ,Η,Β++,Θ," Stop!")){Ё();λ(Σ.Main);}if(Μ(ref Γ,Η,Β++,Θ," Behavior settings"))
if(!ĭ) λ(Σ.Αθ);else λ(Σ.ΑЇ);if(!ĭ){if(Μ(ref Γ,Η,Β++,Θ," Next hole")) ΒЉ(false);}else if(Μ(ref Γ,Η,Β++,Θ," Undock")) Αέ=true;Γ+=ƛ;if(!ĭ) Γ+="Progress: "+Math.Round(ΑΕ,1)+" %\n";Γ+="State: "+ΓΤ(Ω)+" "+ȋ+"m \n";Γ+="Load: "+Βγ+" % Max: "+Ηβ+" \n";if(Ō==0) Γ+="Uranium: "+(į.Count==0?"No reactors":Math.Round(ŝ,1)+"Kg "+(н==-1?"":" Min: "+н+" Kg"))+"\n";
if(Ō==1) Γ+="Battery: "+(Ǽ==ѕ.None?Εώ(Ǽ):ȝ+"% "+(ѝ==-1||ĭ?"":" Min: "+ѝ+" %"))+"\n";if(Ō==2) Γ+="Hydrogen: "+(ǖ.Count==0?"No tanks":Math.Round(Č,1)+"% "+(ў==-1||ĭ?"":" Min: "+ў+" %"))+"\n";}else if(ϋ==Σ.Αθ){String З="";if(Μ(ref З,Η,Β++,Θ," Back")){if(β==Π.В) λ(Σ.Ƭ);else λ(Σ.Main);}
if(Μ(ref З,Η,Β++,Θ," Max load: "+Ȇ+"%")) ш(ref Ȇ,Ȇ<=80?-10:-5,ϊ.Αω,false);if(Μ(ref З,Η,Β++,Θ," Weight limit: "+(ę?"On":"Off"))) ę=!ę;if(Μ(ref З,Η,Β++,Θ," Ejection: "+Θς(б))) б=ĝ(б);if(Μ(ref З,Η,Β++,Θ," Toggle sorters: "+(ś?"On":"Off"))){ś=!ś;if(ś) Ǻ(ΕΝ);}
if(Μ(ref З,Η,Β++,Θ," Unload ice: "+(Ę?"On":"Off"))) Ę=!Ę;if(Μ(ref З,Η,Β++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ш(ref н,(н>5?-5:-1),ϊ.Αϊ,true);if(Μ(ref З,Η,Β++,Θ," Battery: "+(ѝ==-1?"Ignore":"Min "+ѝ+"%"))) ш(ref ѝ,-5,ϊ.Γζ,true);if(Μ(ref З,Η,Β++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Min "+ў+"%"))) ш(ref ў,-10,ϊ.Γη,true);
if(Μ(ref З,Η,Β++,Θ," When done: "+(Ĳ?"Return home":"Stop"))) Ĳ=!Ĳ;if(Μ(ref З,Η,Β++,Θ," On damage: "+ΕЀ(ё))) ё=ĝ(ё);if(Μ(ref З,Η,Β++,Θ," Advanced...")) λ(Σ.ΒЊ);Γ+=Γχ(8,З,Η,ref Κκ);}else if(ϋ==Σ.ΒЊ){if(Μ(ref Γ,Η,Β++,Θ," Back")){if(β==Π.В) λ(Σ.Ƭ);else λ(Σ.Main);}
if(Μ(ref Γ,Η,Β++,Θ,(Κ==Ι.ǣ?" Grinder":" Drill")+" inv. balancing: "+(ſ?"On":"Off"))) ſ=!ſ;if(Μ(ref Γ,Η,Β++,Θ," Enable"+(Κ==Ι.ǣ?" grinders":" drills")+": "+(Ź?"Fwd + Bwd":"Fwd"))) Ź=!Ź;if(Μ(ref Γ,Η,Β++,Θ," Work speed fwd.: "+Ƣ+"m/s")) ш(ref Ƣ,0.5f,ϊ.ƶ,false);
if(Μ(ref Γ,Η,Β++,Θ," Work speed bwd.: "+ơ+"m/s")) ш(ref ơ,0.5f,ϊ.ƶ,false);if(Μ(ref Γ,Η,Β++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ш(ref ǻ,0.1f,ϊ.Αε,false);if(Μ(ref Γ,Η,Β++,Θ," Width overlap: "+ş*100f+"%")) Εσ(true,0.05f);if(Μ(ref Γ,Η,Β++,Θ," Height overlap: "+Ŋ*100f+"%")) Εσ(false,0.05f);
}else if(ϋ==Σ.ΑЇ){if(Μ(ref Γ,Η,Β++,Θ," Back")){if(β==Π.В) λ(Σ.Ƭ);else λ(Σ.Main);}if(Μ(ref Γ,Η,Β++,Θ," Max load: "+Ȇ+"%")) ш(ref Ȇ,Ȇ<=80?-10:-5,ϊ.Αω,false);if(Μ(ref Γ,Η,Β++,Θ," Unload ice: "+(Ę?"On":"Off"))) Ę=!Ę;if(Μ(ref Γ,Η,Β++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ш(ref н,(н>5?-5:-1),ϊ.Αϊ,true);
if(Μ(ref Γ,Η,Β++,Θ," Battery: "+(ѝ==-1?"Ignore":"Charge up"))) ѝ=(ѝ==-1?1:-1);if(Μ(ref Γ,Η,Β++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Fill up"))) ў=(ў==-1?1:-1);if(Μ(ref Γ,Η,Β++,Θ," On damage: "+ΕЀ(ё))) ё=ĝ(ё);if(Μ(ref Γ,Η,Β++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ш(ref ǻ,0.1f,ϊ.Αε,false);
}else if(ϋ==Σ.ΖΥ){double ΚΛ=0;if(Φ.Count>0) ΚΛ=Vector3.Distance(Φ.Last().Ε,δ);if(Μ(ref Γ,Η,Β++,Θ," Stop path recording")) Ζα();if(Κ!=Ι.έ){if(Μ(ref Γ,Η,Β++,Θ," Home: "+(ȭ?"Use old home":(Ο.κ?"Was set! ":"none ")))) ȭ=!ȭ;}else{if(Μ(ref Γ,Η,Β++,Θ," Connector 1: "+(ȭ?"Use old connector":(Ο.κ?"Was set! ":"none ")))) ȭ=!ȭ;
if(Μ(ref Γ,Η,Β++,Θ," Connector 2: "+(Ɯ?"Use old connector":(Ζ.κ?"Was set! ":"none ")))) Ɯ=!Ɯ;}if(Μ(ref Γ,Η,Β++,Θ," Path: "+(Ɲ?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) Ɲ=!Ɲ;Γ+=ƛ;Γ+="Wp spacing: "+Math.Round(ΘΞ)+"m\n";}else if(ϋ==Σ.ΓЂ){if(Μ(ref Γ,Η,Β++,Θ," Stop")){Ё();λ(Σ.Main);}
Γ+=ƛ;Γ+="State: "+ΓΤ(Ω)+" \n";Γ+="Speed: "+Math.Round(Ĕ,1)+"m/s\n";Γ+="Target dist: "+ȋ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+Εγ+"\n";}else if(ϋ==Σ.ΕЏ){List<IMyTerminalBlock>Ņ=ΕЁ();if(č) Ĭ++;if(Ĭ>=Ņ.Count) Ĭ=0;if(Μ(ref Γ,Η,Β++,Θ," Next")) λ(Σ.ΕА);
Γ+=ƛ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βγ,1)+"% "+Math.Round(ȁ,1)+" / "+Math.Round(ȇ,1)+"\n";Γ+="Uranium: "+(į.Count==0?"No reactors":Math.Round(ŝ,1)+"Kg "+Δώ)+"\n";Γ+="Battery: "+(Ǽ==ѕ.None?"":ȝ+"% ")+Εώ(Ǽ)+"\n";Γ+="Hydrogen: "+(ǖ.Count==0?"No tanks":Math.Round(Č,1)+"% ")+"\n";
Γ+="Damage: "+(Ņ.Count==0?"None":""+(Ĭ+1)+"/"+Ņ.Count+" "+Ņ[Ĭ].CustomName)+"\n";}else if(ϋ==Σ.ΕА){if(Μ(ref Γ,Η,Β++,Θ," Back")) λ(Σ.Main);Γ+=ƛ;Γ+="Next scan: "+ΑΠ+"s\n";Γ+="Drill area: "+Math.Round(Ȑ,1)+"x"+Math.Round(ȏ,1)+"m Ship: "+Math.Round(Ɩ,1)+"x"+Math.Round(ΑΣ,1)+"m ("+Math.Round(ο,1)+"m)\n";
Γ+="Broadcast: "+(Ŷ?"Online - "+Ɨ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ɔ*100f,1)+"% \n";}else if(ϋ==Σ.ΕЋ){if(Μ(ref Γ,Η,Β++,Θ," Back")) λ(Σ.Main);Γ+=ƛ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+ΑΠ+"s\n";if(Βτ) Γ="Recompile script now";int ΚΜ=Γ.Split('\n').Length;for(int i=ΚΜ;i<=10;i++) Γ+="\n";Γ+=ƕ;Γ+="Last: "+Λ+"\n";
return Γ;}void Εσ(bool ΚΗ,float ǝ){Ё();Ƶ(true,false);if(ΚΗ) ш(ref ş,ǝ,ϊ.Γρ,false);else ш(ref Ŋ,ǝ,ϊ.Γρ,false);Ερ();Ŀ(true,true,0,0);}float Εχ(float ξ,float[]ΒІ){float ή=ΒІ[0];for(int i=ΒІ.Length-1;i>=0;i--)
if(ξ<ΒІ[i]) ή=ΒІ[i];return ή;}String Αύ(ref int ī){String ή="";if(ī>=ΕΨ.Count) ī=-1;if(ī>=0) ή=ΕΨ[ī].CustomName;ī++;return ή;}void ΑЉ(string ί){if(β!=Π.В) return;if(ί=="") return;IMyTerminalBlock Ψ=χ.GetBlockWithName(ί);if(Ψ==null||!(Ψ is IMyTimerBlock)){Λ="Timerblock "+ί+" not found!";
return;}((IMyTimerBlock) Ψ).Trigger();}void Ţ(ref int value,int ƾ,int ė,int ΕΛ){if(ƾ==0) return;if(value<ė&&ΕΛ>0||value<=ė&&ΕΛ<0){value+=ΕΛ;return;}int Ιμ=Math.Abs(ƾ);int ΓЈ=0;int ΔЇ=1;while(true){ΓЈ+=ΔЇ*Ιμ*10;if(ƾ<0&&value-ė<=ΓЈ) break;if(ƾ>0&&value-ė<ΓЈ) break;
ΔЇ++;}value+=ΔЇ*ƾ;}void ŀ(bool ΛΙ){я=Math.Max(я,1);ю=Math.Max(ю,1);л=Math.Max(л,0);Ŀ(ϋ==Σ.ƭ,false,0,0);}bool ĺ(ref float value,bool Κζ,ϊ υ,String ō,String ΚΔ){if(ō=="") return false;float Ɗ=-1;bool ΗΡ=false;if(ō.ToUpper()==ΚΔ) ΗΡ=true;else if(!float.TryParse(ō,out Ɗ)) return false;
else Ɗ=Math.Max(0,Ɗ);if(Κζ) Ɗ=(float) Math.Round(Ɗ);ш(ref value,Ɗ,υ,ΗΡ,false);return true;}void ш(ref float value,float ǝ,ϊ υ,bool Αά){ш(ref value,value+ǝ,υ,Αά,true);}void ш(ref float value,float Ƈ,ϊ υ,bool Αά,bool ΚБ){float ǰ=0,ǯ=0;if(υ==ϊ.ƶ){ǰ=0.5f;ǯ=10f;}if(υ==ϊ.Αε){ǰ=0.1f;ǯ=1f;}
if(υ==ϊ.ƨ){ǰ=50f;ǯ=100f;}if(υ==ϊ.Γζ){ǰ=5f;ǯ=30f;}if(υ==ϊ.Αϊ){ǰ=1f;ǯ=25f;}if(υ==ϊ.Αω){ǰ=10f;ǯ=95f;}if(υ==ϊ.Γη){ǰ=10f;ǯ=90f;}if(υ==ϊ.ΙΧ){ǰ=10f;ǯ=1800;}if(υ==ϊ.Γρ){ǰ=0.0f;ǯ=0.75f;}if(Ƈ==-1&&Αά){value=-1;return;}if(value==-1) Αά=false;
bool Ηψ=Ƈ<ǰ||Ƈ>ǯ;if(Ηψ&&ΚБ){if(Ƈ<value) value=ǯ;else if(Ƈ>value) value=ǰ;}else value=Ƈ;if(Ηψ&&Αά) value=-1;else value=Math.Max(ǰ,Math.Min(value,ǯ));value=(float) Math.Round(value,2);}void ΒЉ(bool Κα){if(Κα) Я=Math.Max(0,Я-1);else Я++;ΓΝ();ΓΜ();for(int k=0;k<ȥ.Count&&k<з.Length;k++) Γϋ(ȥ[k],k<Я);
Д=-1;ΓІ(true);}T ĝ<T>(T ξ){int Β=Array.IndexOf(Enum.GetValues(ξ.GetType()),ξ);Β++;if(Β>=Θκ(ξ)) Β=0;return(T) Enum.GetValues(ξ.GetType()).GetValue(Β);}int Θκ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЁ(){Αγ.Clear();for(int i=0;i<Φ.Count;i++) Αγ.Add(Φ[i]);Φ.Clear();
Βί=true;Δν=new Ϋ(Ο);Δρ=new Ϋ(Ζ);Ο.κ=false;if(Κ==Ι.έ) Ζ.κ=false;for(int i=0;i<Ǟ.Count;i++)
if(!ȫ.Contains(Ǟ.Keys.ElementAt(i)))
ȫ.Add(Ǟ.Keys.ElementAt(i));ȭ=false;Ɯ=false;Ɲ=false;λ(Σ.ΖΥ);}void Ζα(){if(ȭ) Ο=Δν;if(Ɯ) Ζ=Δρ;if(Ɲ){Φ.Clear();for(int i=0;i<Αγ.Count;i++) Φ.Add(Αγ[i]);}Βί=false;Ё();λ(Σ.Main);}void Ιε(){if(!Βί) return;if(Ω!=Α.ы){Ζα();return;}if(!Δν.κ) ȭ=false;if(!Δρ.κ) Ɯ=false;if(Αγ.Count<=1) Ɲ=false;
IMyShipConnector О=д(MyShipConnectorStatus.Connectable);if(О==null) О=д(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Ĕ,2)<=0.20) ΔΨ++;else ΔΨ=0;if(ΔΨ>=5){if(Κ==Ι.έ&&(Ο.κ||ȭ)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.ι=Ϊ.WorldMatrix.Forward;Ζ.Ђ=Ϊ.WorldMatrix.Left;
Ζ.θ=Ϊ.WorldMatrix.Down;Ζ.φ=Ϊ.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.κ=true;Ζ.м=О.Position;}else{Ο.ι=Ϊ.WorldMatrix.Forward;Ο.Ђ=Ϊ.WorldMatrix.Left;Ο.θ=Ϊ.WorldMatrix.Down;Ο.φ=Ϊ.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.κ=true;Ο.м=О.Position;}}}double Δσ=-1;if(Φ.Count>0) Δσ=Vector3.Distance(δ,Φ.Last().Ε);
double Κρ=Math.Max(1.5,Math.Pow(Ĕ/100.0,2));double Ην=Math.Max(Ĕ*Κρ,2);ΘΞ=Ην;if((Δσ==-1)||Δσ>=Ην){Ϋ ά=new Ϋ(δ,ΒΨ,Ǆ,ΑΗ,Ϊ.GetNaturalGravity());ά.ΘΧ(Ȭ,ȫ);Φ.Add(ά);}}Ϋ ъ(){if(Κ!=Ι.έ) return Ǚ!=null?Ǚ:Ο;return ΖЄ;}void ŭ(Ϋ α,Π Ѓ){ΖЄ=α;if(β==Π.В) ќ=Ѓ;}bool ȕ(Ϋ α,float Њ,bool ΚЈ,out Vector3 ν)
{if(ΚЈ){Vector3I Δδ=new Vector3I((int) α.м.X,(int) α.м.Y,(int) α.м.Z);IMySlimBlock Ψ=Me.CubeGrid.GetCubeBlock(Δδ);if(Ψ==null||!(Ψ.FatBlock is IMyShipConnector)){ν=new Vector3();return false;}Vector3 Ιϋ=т(Ϊ,Ψ.FatBlock.GetPosition()-δ);Vector3 Ιϊ=т(Ϊ,Ψ.FatBlock.WorldMatrix.Forward);ν=α.Ε-ΓЅ(α.ι,α.θ*-1,Ιϋ)-ΓЅ(α.ι,α.θ*-1,Ιϊ)*Њ;
return true;}else{ν=α.Ε;return true;}}Vector3 Ȳ(int Ιψ,int Ɛ,bool Κδ){if(!Κδ&&ΗΛ) return ΔΘ;float Ιω=((Ќ-1f)/2f)-Ιψ;float Κη=((Ь-1f)/2f)-Ɛ;ΔΘ=Ζ.Ε+Ζ.Ђ*Ιω*Ȑ+đ*-1*Κη*ȏ;ΗΛ=true;return ΔΘ;}Vector3 ΓΣ(Vector3 Κυ,float ΔΤ){return Κυ+(Ǳ*ΔΤ);}ε ΓΘ(){float Щ=-1;ε ή=ε.ȓ;
if(Κ!=Ι.έ){ΖΙ();if(β!=Π.ь){Vector3 Н=ľ(Ǳ,đ*-1,δ-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ќ*Ȑ)/2f&&Math.Abs(Н.Y)<=(float)(Ь*ȏ)/2f){if(Н.Z<=-1&&Н.Z>=-Ř*2) return ε.Αη;if(Н.Z>-1&&Н.Z<ο*2) return ε.ȓ;}if(ě(Ζ.Ε,ref Щ)) ή=ε.ȓ;}if(Ο.κ){if(ě(Ο.Ε,ref Щ)) ή=ε.Ȓ;for(int i=0;i<Φ.Count;i++)
if(ě(Φ[i].Ε,ref Щ)) ή=ε.Ā;if(Vector3.Distance(δ,Ο.Ε)<dockDist*ο) ή=ε.Ȓ;if(д(MyShipConnectorStatus.Connectable)!=null||д(MyShipConnectorStatus.Connected)!=null)
ή=ε.Ȓ;}}else{Vector3 ν=new Vector3();IMyShipConnector Ƞ=д(MyShipConnectorStatus.Connected);if(Ο.κ){if(ě(Ο.Ε,ref Щ)) ή=ε.Ȓ;if(ȕ(Ο,dockDist,true,out ν))
if(ě(ν,ref Щ)) ή=ε.Ȓ;if(Ƞ!=null&&Vector3.Distance(Ƞ.GetPosition(),Ο.Ε)<5)
return ε.ΒЍ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ο.Ε)>dockDist*ο&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*ο)
if(ě(Φ[i].Ε,ref Щ)) ή=ε.Ā;if(Ζ.κ){if(ě(Ζ.Ε,ref Щ)) ή=ε.ȓ;if(ȕ(Ζ,dockDist,true,out ν))
if(ě(ν,ref Щ)) ή=ε.ȓ;if(Ƞ!=null&&Vector3.Distance(Ƞ.GetPosition(),Ζ.Ε)<5)
return ε.ΒЎ;}}return ή;}bool ě(Vector3 ν,ref float Щ){float Њ=Vector3.Distance(ν,δ);if(Њ<Щ||Щ==-1){Щ=Њ;return true;}return false;}void ΓЀ(){if(А>0){Λ="Setup error! Can't start";return;}if(Κ==Ι.έ){Ĝ();return;}Ζ.Ε=δ;Ζ.φ=Ϊ.GetNaturalGravity();Ζ.ι=ΒΨ;Ζ.θ=Ǆ;Ζ.Ђ=ΑΗ;
Ǳ=ǚ.WorldMatrix.Forward;đ=Ζ.θ;if(Ǳ==Ϊ.WorldMatrix.Down) đ=Ϊ.WorldMatrix.Backward;Ƶ(true,true);Τ(Α.ȱ);ΓϏ();}void Ƶ(bool Ǭ,bool Κε){if(β==Π.ь&&!Ǭ) return;bool ΚΥ=Ǭ||β==Π.ΓΚ||Ќ!=я||Ь!=ю||Ŏ!=Ȋ;if(ΚΥ){if(β!=Π.ь){β=Π.Ακ;Ȳ(Ϗ,ό,Κε);Λ="Job changed, lost progress";}Ŏ=Ȋ;
Ќ=я;Ь=ю;ό=0;Ϗ=0;з="";Д=-1;ő=0;ď=0;ж=0;Я=0;ΓІ(true);}}void Γσ(){е(δ,0);ǔ(Ȭ,true);}void ΓϏ(){ș(у.ΑЄ);Γσ();Ʊ(Ȩ,false);Ǻ(ΕΝ);β=Π.В;Επ(true);ќ=β;λ(Σ.Ƭ);ΙΖ();ΒΚ=true;Βά=0;for(int i=Ʒ.Count-1;i>=0;i--)
if(Ʃ(Ʒ[i],false)) Βά++;if(Βά>0) Λ="Started with damage";}void Ё(){if(β==Π.В){β=Π.ū;Λ="Job paused";}Τ(Α.ы);ќ=β;ΑІ(false,0,0,0,0);ΓЃ();Ελ(new Vector3(),false);Ζΰ();Γϊ(ChargeMode.Auto);Γό(false);ΖΧ(true);ā(Α.ы);Ŀ(false,false,0,0);ǔ(ω,false);ǔ(ј,true);Ǻ(true);Βυ=false;Αλ();
ǃ=false;Γυ();ΒΚ=false;ΑΤ=false;Αέ=false;if(ϋ!=Σ.Main&&ϋ!=Σ.Αθ&&ϋ!=Σ.ΒЊ&&ϋ!=Σ.ΑЇ)
λ(Σ.Main);}void Ĝ(){ε Э=ΓΘ();if(Κ==Ι.έ){if(!Ζ.κ||!Ο.κ) return;ΓϏ();bool Δβ=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(Ɠ==Π.Š) Δβ=true;if(Ɠ==Π.š) Δβ=false;if(Δβ){ŭ(Ο,Π.Š);switch(Э){case ε.ΒЍ:Τ(Α.ġ);break;case ε.Ā:Τ(Α.с);break;case ε.Ȓ:Τ(Α.Ǹ);break;default:Τ(Α.Ǧ);break;
}}else{ŭ(Ζ,Π.š);switch(Э){case ε.ΒЎ:Τ(Α.ġ);break;case ε.ȓ:Τ(Α.Ǹ);break;case ε.Ā:Τ(Α.с);break;default:Τ(Α.Ǧ);break;}}}else{if(β!=Π.ū&&β!=Π.Ακ) return;bool ΚЊ=β==Π.Ακ;ΓϏ();bool Δι=ƪ(false)&&Ο.κ;switch(Э){case ε.ȓ:Τ(Δι?Α.Ğ:Α.ȱ);break;case ε.Ā:Τ(Δι?Α.И:Α.с);break;case ε.Ȓ:Τ(Δι?Α.Ǹ:Α.ț);break;
case ε.Αη:{if(ď!=Я||ΚЊ) Τ(Α.Ș);else Τ(Α.Ū);}break;default:break;}}}void Ĺ(){if(β==Π.ь&&!Ο.κ) return;if(Κ==Ι.έ&&(!Ζ.κ||!Ο.κ)) return;Λ="Move to job";ε Э=ΓΘ();if(Κ==Ι.έ){ŭ(Ζ,Π.š);switch(Э){case ε.ȓ:Τ(Α.Ǹ);break;case ε.Ā:Τ(Α.с);break;case ε.ΒЎ:return;default:Τ(Α.Ǧ);break;
}ā(Α.ġ);}else{switch(Э){case ε.ȓ:Τ(Α.с);break;case ε.Ā:Τ(Α.с);break;case ε.Ȓ:Τ(Α.ț);break;case ε.Αη:Τ(Α.Ș);break;default:break;}if(β==Π.ь) ā(Α.с);else ā(Α.Ě);Βυ=true;}Γσ();λ(Σ.ΓЂ);Ʊ(Ȩ,false);ќ=Π.Αό;}void Αρ(){if(!Ο.κ) return;Λ="Move home";ε Э=ΓΘ();if(Κ==Ι.έ){ŭ(Ο,Π.Š);switch(Э)
{case ε.Ā:Τ(Α.И);break;case ε.Ȓ:Τ(Α.Ǹ);break;case ε.ΒЍ:return;default:Τ(Α.Ǧ);break;}ā(Α.ġ);}else{if(д(MyShipConnectorStatus.Connected)!=null) return;if(д(MyShipConnectorStatus.Connectable)!=null){Τ(Α.ĸ);ā(Α.ț);return;}switch(Э){case ε.ȓ:Τ(Α.Ğ);break;case ε.Ā:Τ(Α.И);break;case ε.Ȓ:Τ(Α.И);break;
case ε.Αη:Τ(Α.ļ);break;default:break;}ā(Α.ț);}Γσ();λ(Σ.ΓЂ);Ʊ(Ȩ,false);ќ=Π.Αϋ;}void Τ(Α ΔЈ){if(ΔЈ==Α.ы) ΑΧ=Α.ы;if(ΑΧ!=Α.ы&&Ω==ΑΧ&&ΔЈ!=ΑΧ){Ё();return;}Е=true;Ω=ΔЈ;}void ā(Α Ѓ){this.ΑΧ=Ѓ;}ǥ ΛΕ(int ΑΓ,bool ǳ){if(ǳ){ǜ=null;Ϗ=0;ό=0;}if(Ȋ==э.ƴ){int œ=ΑΓ+1;
ό=(int) Math.Floor(Й(ΑΓ,Ќ));if(ό%2==0) Ϗ=ΑΓ-(ό*Ќ);else Ϗ=Ќ-1-(ΑΓ-(ό*Ќ));if(ό>=Ь) return ǥ.Αζ;else return ǥ.Ασ;}else if(Ȋ==э.Ες){if(ǜ==null) ǜ=new int[]{0,-1,0,0};int ǌ=(int) Math.Ceiling(Ќ/2f);int ǋ=(int) Math.Ceiling(Ь/2f);int Δη=(int) Math.Floor(Ќ/2f);
int Δζ=(int) Math.Floor(Ь/2f);int ΑΔ=0;while(ǜ[2]<Math.Pow(Math.Max(Ќ,Ь),2)){if(ΑΔ>200) return ǥ.Θλ;ΑΔ++;ǜ[2]++;if(-ǌ<Ϗ&&Ϗ<=Δη&&-ǋ<ό&&ό<=Δζ){if(ǜ[3]==ΑΓ){this.Ϗ=Ϗ-1+ǌ;this.ό=ό-1+ǋ;return ǥ.Ασ;}ǜ[3]++;}if(Ϗ==ό||(Ϗ<0&&Ϗ==-ό)||(Ϗ>0&&Ϗ==1-ό))
{int Κψ=ǜ[0];ǜ[0]=-ǜ[1];ǜ[1]=Κψ;}Ϗ+=ǜ[0];ό+=ǜ[1];}}return ǥ.Αζ;}void Ιγ(){if(Ω==Α.ȱ){if(Е){Ж=0;if(ď!=Я) ő=0;ď=Я;}if(Ж==0){int ΔΛ=Θέ(Я);if(ΔΛ<0){if(!ΔΠ){ΔΠ=true;ΖЁ=δ;}е(ΖЁ,0);Λ="Waiting for the controller";return;}ΔΠ=false;if(ΔΛ!=Я){Я=ΔΛ;ő=0;ď=Я;}
ǥ ή=ΙΟ(Я);if(ή==ǥ.Αζ){β=Π.ΓΚ;bool Ηχ=з.IndexOf('0')>=0;Λ=Ηχ?"No free hole left for this ship":"Job done";if((Ĳ||Ηχ)&&Ο.κ){Τ(Α.Ğ);ā(Α.ț);ќ=Π.Αϋ;}else{Τ(Α.ť);ā(Α.Ě);ќ=Π.Αό;}return;}if(ή==ǥ.Ασ){Ж=1;ǔ(ω,true);џ=Ȳ(Ϗ,ό,true);е(џ,10);й(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}}else
{if(Д>=0&&(ΖΘ()?Ȁ>=0&&Ȁ!=Д:ΓΪ(Д,true))){Ж=0;return;}if(а<wpReachedDist){Τ(Α.Ū);return;}}}if(Ω==Α.Ū){if(Е){ǔ(ω,true);Ǻ(false);џ=Ȳ(Ϗ,ό,false);е(ΓΣ(џ,0),0);й(Ζ.θ,Ζ.ι,Ζ.Ђ,false);Ж=1;ѐ=0;р=0;ΑБ=0;Δυ=-1;Ř=л;Δο=true;}if(!ΓΓ()){Τ(Α.ļ);return;}if(ƪ(true))
{Ηυ=ȯ("","ORE",Ш.ĵ);if((б==Г.ΓΛ||б==Г.Αμ||
б==Г.ΓΫ||б==Г.Αφ)&&Κ!=Ι.ǣ)
Τ(Α.Ʀ);else if((б==Г.Εφ||б==Г.ΓΕ)&&Κ!=Ι.ǣ)
Τ(Α.ΓЇ);else
Τ(Α.ļ);return;}ж=Vector3.Distance(δ,џ);if(ж>ő){ő=ж;Δο=false;}if(Κ==Ι.ǣ&&ΕЂ()==MyDetectedEntityType.SmallGrid)
р+=2;else р-=2;р=Math.Max(100,Math.Min(400,р));if(Ж>0&&Ж<р){if(ж>ѐ){if(р>150) ѐ=ж;else ѐ=(float) Math.Ceiling(ж);Ж=1;}else Ж++;}else{if(Ж>0){Λ="Ship stuck! Retrying";ѐ=ж;Ж=0;Ŀ(false,true,0,ο*sensorRange);}е(ΓΣ(џ,Math.Max(0,ѐ-ο)),ΓΦ(false));if(а<=wpReachedDist/2){Ж=1;ѐ=0;}
return;}Ŀ(false,true,ο*sensorRange,0);Vector3 Ιξ=џ+Ǳ*ж;bool ΗΦ=false;if(Vector3.Distance(Ιξ,δ)>0.3f){Vector3 ΙϏ=џ+Ǳ*(ж+0.1f);е(ΙϏ,4);ΗΦ=true;}else{float Ǵ=ΓΦ(true);Vector3 ΖЈ=ΓΣ(џ,Math.Max(л+1,ж+1));ǹ(true,false,false,ΖЈ,ΖЈ-џ,Ǵ,Ǵ);}bool ΔΥ=false;if(љ==Ǒ.ΒА||љ==Ǒ.ΒЏ)
{if(!ΗΦ){float ΔЋ=0;foreach(IMyTerminalBlock Ȏ in ω)
ΔЋ+=Θε(Ȏ,"","",љ==Ǒ.ΒЏ?new string[]{"STONE"}:null);if(ΔЋ>Δυ||ж<л||Δο){ΓЎ=0;ΑБ=ж;Ř=(int)(Math.Max(Ř,ΑБ)+ο/2);}else{ΔΥ=ж-ΑБ>2&&ΓЎ>=20;ΓЎ++;}Δυ=ΔЋ;}}else ΔΥ=ж>=Ř;if(ď!=Я){Τ(Α.Ș);ж=0;return;}if(ΔΥ){Я++;Τ(Α.Ș);ж=0;return;}}if(Ω==Α.Ș||Ω==Α.ļ||Ω==Α.Ʀ)
{if(Е){џ=Ȳ(Ϗ,ό,false);й(Ζ.θ,Ζ.ι,Ζ.Ђ,false);ǔ(ω,Ź);Ǻ(false);ѐ=Vector3.Distance(δ,џ);Ŀ(false,true,0,ο*sensorRange);}е(џ,ΓΦ(false));if(Vector3.Distance(δ,џ)>=ѐ+5){ǔ(ω,false);Ǻ(true);Λ="Can´t return!";}if(а<wpReachedDist){if(Ω==Α.Ș&&Βυ) Τ(Α.ť);if(Ω==Α.Ș) Τ(Α.ȱ);
if(Ω==Α.Ʀ) Τ(Α.Απ);if(Ω==Α.ļ){if(Ο.κ) Τ(Α.Ğ);else{Ё();Ĺ();Λ="Can´t return, no dock found";}}return;}}if(Ω==Α.И||Ω==Α.с){if(Ω==Α.с&&β==Π.В&&Κ!=Ι.έ){if(!ΓΓ()||ƪ(true)){Τ(Α.И);return;}}bool ΕΒ=false;bool ΒЄ=false;bool Ζμ=false;float ų=0;bool Ητ=false;Ϋ ά=null;
if(Е){if(Ω==Α.И||Κ==Ι.έ){if(Κ!=Ι.έ){if(Ă(1)) ΒЌ();else{Ǚ=null;Ȫ=0;ǃ=!ΓΔ(1);}}Ϋ α=ъ();μ=new Γς();μ.η=α;μ.ǁ=followPathDock*ο;μ.Εΰ=useDockDirectionDist*ο;μ.ų=10;μ.ƚ.Add(α.Ε);Vector3 Ų=new Vector3();if(ȕ(α,dockDist*ο,true,out Ų))
μ.ƚ.Add(Ų);else μ.ǁ*=1.5f;if(Κ==Ι.έ){if(α==Ο) μ.ΒΥ=Ζ.Ε;if(α==Ζ) μ.ΒΥ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;}}else if(Ω==Α.с){μ=new Γς();μ.η=Ζ;μ.ǁ=followPathJob*ο;μ.Εΰ=useJobDirectionDist*ο;μ.ų=10;μ.ΒΥ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;μ.ƚ.Add(Ζ.Ε);if(β==Π.ь){if(!Ο.κ||Φ.Count==0){Ё();return;}
float ΙЅ=Vector3.Distance(Φ.First().Ε,Ο.Ε);float ΙІ=Vector3.Distance(Φ.Last().Ε,Ο.Ε);if(ΙЅ<ΙІ) μ.η=Φ.Last();else μ.η=Φ.First();}}ΑΞ=new Vector3();Ητ=!ΘЄ(δ);ǔ(ω,false);Ǻ(true);и=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,μ.ΒΥ)<=μ.Δί) continue;
double Њ=Vector3.Distance(Φ[i].Ε,δ);if(ǿ==-1||Њ<ǿ){и=i;ǿ=Њ;}}Ȉ=Θχ(μ.η.Ε,и);ń=null;}if(Ω==Α.И&&Κ!=Ι.έ&&!Е&&!Ƴ()){Ϋ ΔΒ=ъ();ǃ=!ΓΔ(1);if(ъ()!=ΔΒ){Τ(Α.И);return;}}if(Ω==Α.И&&Κ!=Ι.έ&&!Е&&Ă(1)&&ΒЌ()){Τ(Α.И);return;}bool ΗΤ=false;if(Ω==Α.И&&!Е&&и>=0&&и<Φ.Count)
{Vector3 ǀ=Φ[и].Ε+ΕЅ(и,Ȉ);int ΔЉ=и+Ȉ;Vector3 œ=Ȉ!=0&&ΔЉ>=0&&ΔЉ<Φ.Count?Φ[ΔЉ].Ε:μ.η.Ε;if(ΘϏ(ǀ,œ)){if(!ΒΝ){ΒΝ=true;ΖЍ=ũ(ǀ)?δ:ǀ;}е(ΖЍ,0);й(μ.η.θ,μ.η.ι,μ.η.Ђ,false);Λ="Waiting for dock clearance";return;}ΒΝ=false;ΗΤ=Ă(1)&&(ũ(δ)||ũ(œ));}ΘΨ(Φ,Ȉ,μ.ƚ,μ.ǁ,Е,ref Ж);
for(int i=0;i<μ.ƚ.Count;i++){float Њ=Vector3.Distance(δ,μ.ƚ[i]);if(Њ<=μ.ǁ) ΕΒ=true;if(Њ<=μ.Εΰ) ΒЄ=true;}if(ΗΤ) ΒЄ=true;if(ΒЄ) ų=μ.ų;float ΛΚ=ń!=null?ń.Ч:Ĕ;float ΚА=(float) Math.Max(Ĕ*0.1f*ο,wpReachedDist);if((а<ΚА)||Е){if(!Е) и+=Ȉ;if(Ȉ==0||и>Φ.Count-1||и<0)
ΕΒ=true;else{Εγ=Ȉ>0?Φ.Count-1-и:и;ά=Φ[и];ń=ά;if(и>=1&&и<Φ.Count-1)
ΑΞ=ά.Ε-Φ[и-Ȉ].Ε;else ń=null;Ѐ=ά.Ε+ΕЅ(и,Ȉ);Ζμ=true;}}if(ΒЄ)
й(μ.η.θ,μ.η.ι,μ.η.Ђ,false);else if(Ητ)
ΒЋ(μ.η.θ,10,true);else if(Ζμ&&ά!=null){if(Ȉ>0) й(ά.θ,ά.ι,ά.Ђ,90,false);else й(ά.θ,-ά.ι,-ά.Ђ,90,false);}ǹ(true,false,true,Ѐ,ΑΞ,ń==null?0:ń.Ч,ų);if(ΕΒ){Εγ=0;if(Ω==Α.И&&!ΘΫ()){е(Ѐ,0);return;}if(Ω==Α.И&&ǃ){е(Ѐ,0);Λ="Waiting for a free dock";return;}if(Ω==Α.И||Κ==Ι.έ){Τ(Α.Ǹ);return;}
if(Ω==Α.с&&Βυ){Τ(Α.ť);return;}if(Ω==Α.с){Τ(Α.ȱ);return;}}}if(Ω==Α.Ǹ||Ω==Α.ư){if(Κ!=Ι.έ&&Ƴ()){if(!Ă(1)){Τ(Α.И);return;}}else if(Κ!=Ι.έ){Ϋ ΔΒ=ъ();if(!ΓΔ(2)||ъ()!=ΔΒ){Τ(Α.И);return;}}Ϋ α=ъ();if(Е){if(!ȕ(α,dockDist*ο,true,out Ѐ)){ș(у.Ƥ);Ё();return;}
е(Ѐ,0);ΒЋ(α.θ,90,true);}if(а<followPathDock*ο&&а!=-1){е(Ѐ,10);й(α.θ,α.ι,α.Ђ,false);}if(д(MyShipConnectorStatus.Connectable)!=null||д(MyShipConnectorStatus.Connected)!=null){Τ(Α.ĸ);return;}if(а<wpReachedDist/2&&а!=-1){Τ(Α.Ĵ);return;}}if(Ω==Α.Ĵ||Ω==Α.Ě){if(Е)
{if(Ω==Α.Ĵ){Ϋ α=ъ();if(!ȕ(α,dockDist*ο,true,out Ѐ)){ș(у.Ƥ);Ё();return;}ǹ(true,true,false,Ѐ,0);й(α.θ,α.ι,α.Ђ,10,false);}if(Ω==Α.Ě){й(Ζ.θ,Ζ.ι,Ζ.Ђ,0.5f,false);Ѐ=Ζ.Ε;ǹ(true,true,false,Ѐ,0);}}if(Βή){ΑІ(false,0,0,0,0);if(Ω==Α.Ĵ) Τ(Α.ĸ);if(Ω==Α.Ě) Ё();return;}}
if(Ω==Α.ĸ){if(д(MyShipConnectorStatus.Connected)!=null){if(Κ==Ι.έ) Τ(Α.ġ);else Τ(Α.ț);return;}Ϋ α=ъ();if(Е){р=0;ΙЇ=DateTime.Now;Ж=0;й(α.θ,α.ι,α.Ђ,false);}Vector3I Δδ=new Vector3I((int) α.м.X,(int) α.м.Y,(int) α.м.Z);IMySlimBlock Ζώ=Me.CubeGrid.GetCubeBlock(Δδ);float ΖА=dockingSpeed;
float ΚΟ=dockingSpeed*5;float Ζκ=Math.Max(1.5f,Math.Min(5f,ο*0.15f));if(!ȕ(α,0,true,out Ѐ)||!ȕ(α,Ζκ,true,out ΑΞ)||
Ζώ==null||!Ζώ.FatBlock.IsFunctional){ș(у.Ƥ);Ё();return;}if(р==1||(Vector3.Distance(δ,Ѐ)<=Ζκ*1.1f&&!Е))
р=1;else{Vector3 ΚЀ=т(Ϊ,ΑΞ-δ);Vector3 Δγ=т(Ϊ,Ϊ.GetNaturalGravity());float Κθ=Αι(ΚЀ,Δγ,null);ΖА=Math.Min(ΚΟ,Κθ);}ǹ(true,false,false,Ѐ,Ѐ-δ,dockingSpeed,ΖА);if(Е) ѐ=(float) а;IMyShipConnector ΔΟ=д(MyShipConnectorStatus.Connectable);if(ΔΟ!=null){ǹ(false,false,false,Ѐ,0);
if(Ж>0) Ж=0;Ж--;if(Ж<-5){ΔΟ.Connect();if(ΔΟ.Status==MyShipConnectorStatus.Connected){if(Κ==Ι.έ) Τ(Α.ġ);else Τ(Α.ț);ΓЃ();Ʊ(Ȩ,true);return;}}}else{float ΖЌ=(float) Math.Round(а,1);if(ΖЌ<ѐ){Ж=-1;ѐ=ΖЌ;}else Ж++;if(Ж>20){Τ(Α.ư);return;}}}if(Ω==Α.ț||Ω==Α.ġ||
Ω==Α.ΑЌ||Ω==Α.Αξ||Ω==Α.ķ){bool ΓЌ=false,ΓЍ=false;if(Κ==Ι.έ){if(ъ()==Ο) ΓЌ=true;else if(ъ()==Ζ) ΓЍ=true;}if(Е){ΑΤ=false;if(д(MyShipConnectorStatus.Connected)==null){Τ(Α.Ǧ);return;}ΓЃ();if(ΓЌ) ΑЉ(І.ǫ);if(ΓЍ) ΑЉ(Ї.ǫ);Ŝ=false;ă=false;ċ=false;Αί=false;}if(д(MyShipConnectorStatus.Connected)==null){Ё();ș(у.ΖΒ);return;}
if(β!=Π.В||ѝ==-1||Ǽ==ѕ.None) ă=true;else if(ȝ>=100f) ă=true;else if(ȝ<=99f) ă=false;if(β!=Π.В||ў==-1||ǖ.Count==0) ċ=true;else if(Č>=100f) ċ=true;else if(Č<=99) ċ=false;if(β!=Π.В||н==-1||į.Count==0) Αί=true;else Αί=ŝ>=н;ů є=null;if(ΓЌ) є=І;
if(ΓЍ) є=Ї;if(є!=null&&(є.Р==π.ΑЀ||є.Р==π.ΑϏ)) ă=true;if(є!=null&&(є.Р==π.Γκ)) if(!Ŝ) ă=false;if(є!=null&&(є.Р==π.Γξ||є.Р==π.Γμ)) ċ=true;if(є!=null&&(є.Р==π.Γν)) if(!Ŝ) ċ=false;if(č){ChargeMode Ζφ=ă?ChargeMode.Auto:ChargeMode.Recharge;if(є!=null&&(є.Р==π.ΑϏ||є.Р==π.ΑЀ))
Ζφ=ChargeMode.Discharge;Γϊ(Ζφ);Γό(!ċ);}if(!Ŝ){if(Κ==Ι.έ)
Ŝ=β!=Π.В||Θά(Е,true)||Αέ;else
Ŝ=β!=Π.В||ΖΓ();}else{if(!ă) Τ(Α.ķ);if(!ċ) Τ(Α.Αξ);if(!Αί) Τ(Α.ΑЌ);Е=false;}if(Ŝ&&ă&&ċ&&Αί){Γϊ(ChargeMode.Auto);Γό(false);if(β==Π.В){if(Κ==Ι.έ){if(ъ()==Ο) ΑЉ(І.Ǯ);else if(ъ()==Ζ) ΑЉ(Ї.Ǯ);if(ъ()==Ο) ŭ(Ζ,Π.š);else ŭ(Ο,Π.Š);}}Τ(Α.Ǧ);return;}}if(Ω==Α.Ǧ){if(Αβ){if(!Ετ()) return;
Αβ=false;Τ(Α.Ǧ);return;}if(Е){if(!Ετ()){Αβ=true;return;}IMyShipConnector Ƞ=д(MyShipConnectorStatus.Connected);if(Ƞ==null){Τ(Α.с);return;}IMyShipConnector γ=Ƞ.OtherConnector;ΖΦ(Ƞ,false);Ʊ(Ȩ,false);Ϋ α=null;Ϋ њ=Κ==Ι.έ?Ο:ъ();if(Vector3.Distance(Ƞ.GetPosition(),њ.Ε)<5f&&Ο.κ) α=њ;
if(Vector3.Distance(Ƞ.GetPosition(),Ζ.Ε)<5f&&Ζ.κ) α=Ζ;if(α!=null){if(!ȕ(α,dockDist*ο,true,out Ѐ)){ș(у.Ƥ);Ё();return;}е(Ѐ,5);й(α.θ,α.ι,α.Ђ,false);}else е(δ+γ.WorldMatrix.Forward*dockDist*ο,5);if(β==Π.В) ș(у.ΑЄ);}if(а<wpReachedDist){ǔ(ј,true);Τ(Α.с);return;}}if(Ω==Α.ť)
{if(Е){Ǻ(true);ǔ(ω,false);Ѐ=Ζ.Ε;е(Ѐ,20);й(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(а<wpReachedDist/2){Τ(Α.Ě);return;}}if(Ω==Α.Ğ){if(Е){Ǻ(true);ǔ(ω,false);int Щ=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,δ);if(ǿ==-1||Њ<ǿ){Щ=i;ǿ=Њ;}}
if(Щ==-1){Τ(Α.И);return;}Ѐ=Φ[Щ].Ε;е(Ѐ,10);й(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(а<wpReachedDist){Τ(Α.И);return;}}if(Ω==Α.Απ){bool ώ=false;if(Е){Ǻ(true);if((б==Г.ΓΛ||б==Г.Αμ)&&Αψ()&&
ţ(Ǳ,Ϊ.GetNaturalGravity())<25&&Ќ>=2&&Ь>=2){Vector3 ǅ=δ;if(Ϗ>0&&ό<Ь-1) ǅ=Ȳ(Ϗ-1,ό+1,true);else if(Ϗ<Ќ-1&&ό<Ь-1) ǅ=Ȳ(Ϗ+1,ό+1,true);else if(Ϗ<Ќ-1&&ό>0) ǅ=Ȳ(Ϗ+1,ό-1,true);else if(Ϗ>0&&ό>0) ǅ=Ȳ(Ϗ-1,ό-1,true);else ώ=true;if(!ώ) е(ǅ,10);
}else ώ=true;}if(а<wpReachedDist/2) ώ=true;if(ώ){Τ(Α.ΑЋ);return;}}if(Ω==Α.ΓЇ||Ω==Α.ΑЋ){if(Е){ǹ(true,true,false,δ,0);ǔ(ω,false);Ǻ(true);Ж=-1;р=б==Г.ΓΫ||б==Г.Αφ?0:-1;}bool ň=!ΓΓ();int Βό=ȯ("STONE","ORE",Ш.ĵ);if(б==Г.ΓΕ||б==Г.Αφ||б==Г.Αμ)
Βό+=ȯ("ICE","ORE",Ш.ĵ);bool ΙЏ=Βό>0;bool Δέ=false;if(р>=0){float Ű=(float) Math.Sin(Εψ(р))*Ȑ/3f;float Ιπ=(float) Math.Cos(Εψ(р))*ȏ/3f;Vector3 Ιχ=Ȳ(Ϗ,ό,true)+ΓЅ(Ǳ,đ*-1,new Vector3(Ű,Ιπ,0));е(Ιχ,0.3f);if(а<Math.Min(Ȑ,ȏ)/10f) р+=5f;if(р>=360) р=0;}if(Ж==-1||Βό<Ж){Ж=Βό;ѐ=0;}
else{ѐ++;if(ѐ>50) Δέ=true;}if(!ΙЏ||ň||Δέ){if(!ň){int ΚЅ=ȯ("","ORE",Ш.ĵ);if(ƪ(true)) ň=true;else if(100-(Й(ΚЅ,Ηυ)*100)<minEjection) ň=true;else ș(у.ΑЄ);}if(Δέ&&ň) Λ="Ejection failed";if(Ω==Α.ΑЋ){if(ň){if(Ο.κ) Τ(Α.Ğ);else{Ё();Ĺ();Λ="Can´t return, no dock found";}}
else Τ(Α.ȱ);}else if(ň) Τ(Α.ļ);else Τ(Α.Ū);return;}}Е=false;}void ΘΨ(List<Ϋ>ĳ,int ρ,List<Vector3>ΑΪ,float ȋ,bool ǳ,ref int ǩ){if(ǳ){for(int i=0;i<ĳ.Count;i++) ĳ[i].Ч=0;ǩ=-1;return;}if(ρ==0) return;int ǝ=ρ*-1;if(ǩ==-1) ǩ=ǝ>0?1:ĳ.Count-2;int ΑΔ=0;while(ǩ>=1&&ǩ<ĳ.Count-1)
{if(ΑΔ>50) return;ΑΔ++;try{if((ǝ<0&&ǩ>=1)||(ǝ>0&&ǩ<=ĳ.Count-2)){Ϋ ξ=ĳ[ǩ];bool Ηρ=false;for(int j=0;j<ΑΪ.Count;j++)
if(Vector3.Distance(ξ.Ε,ΑΪ[j])<=ȋ){Ηρ=true;break;}if(!Ηρ){Ϋ Βρ=ĳ[ǩ-ǝ];Ϋ œ=ĳ[ǩ+ǝ];Vector3 ΚЁ=ξ.Ε-œ.Ε;Vector3 ΕΪ=Βρ.Ε-ξ.Ε;Vector3 Κβ=ξ.Ε+Vector3.Normalize(ΚЁ)*ΕΪ.Length();Vector3 ΙЄ=Βρ.Ε-Κβ;Vector3 Βδ=ľ(ρ>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ΙЄ);Vector3 ΚΞ=ľ(ρ>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ΕΪ);
Vector3 Βζ=ľ(ρ>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ξ.φ);ξ.Ч=(float) Math.Sqrt(Math.Pow(Βρ.Ч,2)+Math.Pow(Αι(-ΚΞ,Βζ,ξ),2));for(int j=0;j<ΑΪ.Count;j++)
if(Vector3.Distance(Βρ.Ε,ΑΪ[j])<=ȋ){Vector3 Βη=ľ(ρ>0?ξ.ι:ξ.ι*-1,ξ.θ*-1,ΑΪ[j]-ξ.Ε);float Κχ=Αι(-Βη,Βζ,ξ);ξ.Ч=Math.Min(ξ.Ч,Κχ)/2f;}if(Βδ.Length()==0) Βδ=new Vector3(0,0,1);Vector3 ΚΝ=ľ(ξ.ι,ξ.θ*-1,ξ.φ);float Κϋ=Ƨ(-Βδ,ΚΝ,ξ);float ΑЍ=Й(Κϋ,Ś);float ΕΧ=(float) Math.Sqrt(Й(Βδ.Length()*1.0f,0.5f*ΑЍ));
ξ.Ч=Math.Min(ξ.Ч,(ΕΪ.Length()/ΕΧ)*ǻ);}}}catch{return;}ǩ+=ǝ;}ǩ=-1;}void ΓІ(bool ǳ){if(ǳ){ΑΕ=0;return;}if(Κ==Ι.έ) return;float ώ=Я*Math.Max(1,л);if(ď==Я) ώ+=Math.Min(л,ж);float ΚЄ=Ќ*Ь*Math.Max(1,л);ΑΕ=Math.Max(ΑΕ,(float) Math.Min(ώ/ΚЄ*100.0,100));}MyDetectedEntityType ΕЂ()
{try{if(Ȗ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓΦ(bool Ȅ){if(Κ==Ι.ǣ&&ΕЂ()==MyDetectedEntityType.None&&!Ʃ(ψ,true))
return fastSpeed;else return Ȅ?Ƣ:ơ;}void ΙΪ(){float Ηι=0,ΖІ=0,ō=0,ΔЌ=0;for(int i=0;i<Ģ.Count;i++){IMyBatteryBlock Ƽ=Ģ[i];if(!Ȗ(Ƽ,true)) continue;Ηι+=Ƽ.MaxStoredPower;ΖІ+=Ƽ.CurrentStoredPower;ō+=Ƽ.CurrentInput;ΔЌ+=Ƽ.CurrentOutput;}ȝ=(float) Math.Round(Й(ΖІ,Ηι)*100,1);
if(ō>=ΔЌ) Ǽ=ѕ.ķ;else Ǽ=ѕ.Εω;if(ō==0&&ΔЌ==0||ȝ==100.0) Ǽ=ѕ.ы;if(Ģ.Count==0) Ǽ=ѕ.None;}void Ιΰ(){float ΗΔ=0;for(int i=0;i<ǖ.Count;i++){IMyGasTank ΗА=ǖ[i];if(!Ȗ(ΗА,true)) continue;ΗΔ+=(float) ΗА.FilledRatio;}Č=Й(ΗΔ,ǖ.Count)*100f;}void Ικ(){ŝ=0;try{for(int i=0;i<į.Count;i++)
{IMyReactor ΕΓ=į[i];if(!Ȗ(ΕΓ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΓ.GetInventory(0).GetItems(Ћ);float Љ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem ύ=Ћ[j];if(ύ.Type.SubtypeId.ToUpper()=="URANIUM"&&ύ.Type.TypeId.ToUpper().Contains("_INGOT"))
Љ+=(float) ύ.Amount;}if(Љ<ŝ||i==0){ŝ=Љ;Δώ=ΕΓ.CustomName;}}}catch(Exception e){Ď=e;}}void ΙΫ(){ȇ=0;ȁ=0;try{for(int i=0;i<ŷ.Count;i++){IMyTerminalBlock Ψ=ŷ[i];if(!Ȗ(Ψ,true)) continue;ȁ+=(float) Ψ.GetInventory(0).CurrentVolume;ȇ+=(float) Ψ.GetInventory(0).MaxVolume;
}Βγ=(float) Math.Min(Math.Round(Й(ȁ,ȇ)*100,1),100.0);}catch(Exception e){Ď=e;}}void Ια(){try{ǭ.Clear();for(int i=0;i<ŷ.Count;i++){IMyTerminalBlock Ψ=ŷ[i];if(!Ȗ(Ψ,true)) continue;Εκ(Ψ,Ш.Ũ);}if(б!=Г.Γι){for(int i=0;i<ł.Count;i++){IMyTerminalBlock Ψ=ł[i];if(!Ȗ(Ψ,true)) continue;
Εκ(Ψ,Ш.ΘЂ);}}ΙΣ(ǭ);}catch(Exception e){Ď=e;}}void ΘΪ(){if(ΒΚ){if(ΕЁ().Count>Βά){ΒΚ=false;if(ё!=щ.ƨ){Ё();if(ё==щ.ΓΠ) Ĺ();if(ё==щ.ΑЃ)
if(Ο.κ) Αρ();else Ĺ();}Λ="Damage detected";}}}bool ΓΓ(){if(!Βέ) return true;if(β==Π.В){if(ѝ>0&&Ǽ!=ѕ.None)
if(ȝ<=ѝ){Λ="Low energy! Move home";return false;}if(н>0&&į.Count>0)
if(ŝ<=н){Λ="Low fuel: "+Δώ;return false;}if(ў>0&&ǖ.Count>0)
if(Č<=ў){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕЁ(){List<IMyTerminalBlock>Ņ=new List<IMyTerminalBlock>();for(int i=0;i<Ʒ.Count;i++){IMyTerminalBlock Ψ=Ʒ[i];if(Ʃ(Ψ,false)) Ņ.Add(Ψ);}return Ņ;}bool Ʃ(IMyTerminalBlock Ψ,bool ΕΟ){return(!Ȗ(Ψ,ΕΟ)||!Ψ.IsFunctional);
}bool Ȗ(IMyTerminalBlock Ψ,bool ΕΟ){if(Ψ==null) return false;try{IMyCubeBlock ΗΚ=Me.CubeGrid.GetCubeBlock(Ψ.Position).FatBlock;if(ΕΟ) return ΗΚ==Ψ;else return ΗΚ.GetType()==Ψ.GetType();}catch{return false;}}float Θε(IMyTerminalBlock Ψ,String ǐ,String ΗЈ,String[]Ȃ){float ȡ=0;for(int i=0;i<Ψ.InventoryCount;i++)
{IMyInventory ž=Ψ.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ž.GetItems(Ћ);foreach(MyInventoryItem ύ in Ћ){if(Ȃ!=null&&(Ȃ.Contains(ύ.Type.TypeId.ToUpper())||Ȃ.Contains(ύ.Type.SubtypeId.ToUpper())))
continue;if((ǐ==""||ύ.Type.TypeId.ToUpper()==ǐ)&&(ΗЈ==""||ύ.Type.SubtypeId.ToUpper()==ΗЈ))
ȡ+=(float) ύ.Amount;}}return ȡ;}к Θν(String ί,String υ,Ш Э,bool ΙЀ){ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){к ύ=ǭ[i];if(ύ.ί.ToUpper()==ί&&ύ.υ.ToUpper()==υ&&(ύ.ƃ==Э||Э==Ш.ĵ))
return ύ;}к ή=null;if(ΙЀ){ή=new к(ί,υ,0,Э);ǭ.Add(ή);}return ή;}int ȯ(String ί,String υ,Ш Э){return ȯ(ί,υ,Э,null);}int ȯ(String ί,String υ,Ш Э,String[]Ȃ){int ȡ=0;ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){к ύ=ǭ[i];if(Ȃ!=null&&Ȃ.Contains(ύ.ί.ToUpper())) continue;
if((ί==""||ύ.ί.ToUpper()==ί)&&(υ==""||ύ.υ.ToUpper()==υ)&&(ύ.ƃ==Э||Э==Ш.ĵ))
ȡ+=ύ.Љ;}return ȡ;}void Εκ(IMyTerminalBlock Ψ,Ш Э){for(int i=0;i<Ψ.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ψ.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
Θν(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Э,true).Љ+=(int) Ћ[j].Amount;}}void ΙΣ(List<к>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){к a=Ѝ[j];к b=Ѝ[j+1];if(a.Љ<b.Љ) Ѝ.Move(j,j+1);}}bool ƪ(bool ΛΗ){if(ę&&Κ!=Ι.έ)
if(в!=-1&&Ś>=в){Λ="Ship too heavy";return true;}if(Βγ>=Ȇ||ΑΤ){ΑΤ=false;Λ="Ship is full";return true;}return false;}bool ΖΓ(){String[]Ȃ=null;if(!Ę) Ȃ=new string[]{"ICE"};if(Κ==Ι.Ʈ) return ȯ("","ORE",Ш.Ũ,Ȃ)==0;if(Κ==Ι.ǣ) return ȯ("","COMPONENT",Ш.Ũ,Ȃ)==0;else return ȯ("","",Ш.Ũ,Ȃ)==0;
}bool ΘЅ(){List<IMyCockpit>ΔΞ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔΞ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔΞ.Count;i++)
if(ΔΞ[i].IsUnderControl) return true;return false;}bool Θά(bool ǳ,bool Βχ){IMyShipConnector О=д(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return Ευ(І,ǳ,Βχ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return Ευ(Ї,ǳ,Βχ);return false;}bool Ευ(ů ȟ,bool ǳ,bool Βχ){if(ǳ) ȟ.ΙΒ();ȟ.Ιη();bool ѓ=false;String Ǐ="";switch(ȟ.Р){case π.Γλ:{Ǐ="Waiting for command";ѓ=false;break;}case π.ΖΟ:{Ǐ="Waiting for cargo";ѓ=ƪ(true);break;}case π.ΖΞ:{Ǐ="Unloading";ѓ=ΖΓ();break;}case π.Γπ:{ѓ=true;break;}
case π.Γκ:{Ǐ="Charging batteries";ѓ=ȝ>=100f;break;}case π.ΑЀ:{Ǐ="Discharging batteries";ѓ=ȝ<=25f;break;}case π.ΑϏ:{Ǐ="Discharging batteries";ѓ=ȝ<=0f;break;}case π.Γν:{Ǐ="Filling up hydrogen";ѓ=Č>=100f;break;}case π.Γξ:{Ǐ="Unloading hydrogen";ѓ=Č<=25f;break;}
case π.Γμ:{Ǐ="Unloading hydrogen";ѓ=Č<=0f;break;}case π.Γο:{bool ΗΣ=ΘЅ();if(!ΗΣ) ȟ.ΔА=true;ѓ=ȟ.ΔА&&ΗΣ;Ǐ="Waiting for passengers";break;}}if(!ѓ) ȟ.ΓΒ(true);if(ѓ&&ȟ.ΓΨ()){ѓ=ȟ.ΓΒ(false);Ǐ="Undocking in: "+Ας((int) Math.Max(0,ȟ.Ǫ-ȟ.Ĩ));}if(Βχ) ΘΜ=Ǐ;return ѓ;}void ΘΠ()
{if(!ſ) return;if(ω.Count<=1) return;float ΘΔ=0,ΘΓ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȏ=ω[i];if(Ʃ(Ȏ,true)) continue;ΘΔ+=(float) Ȏ.GetInventory(0).MaxVolume;ΘΓ+=(float) Ȏ.GetInventory(0).CurrentVolume;}float Ζν=(float) Math.Round(Й(ΘΓ,ΘΔ),5);for(int ŕ=0;ŕ<Math.Max(1,Math.Floor(ω.Count/10f));ŕ++)
{float Δμ=0,Δύ=0;float ΗΟ=0,Ηζ=0;IMyTerminalBlock Ĉ=null,ĩ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȏ=ω[i];if(Ʃ(Ȏ,true)) continue;float ΔЄ=(float) Ȏ.GetInventory(0).MaxVolume;float ΒΦ=Й((float) Ȏ.GetInventory(0).CurrentVolume,ΔЄ);if(Ĉ==null||ΒΦ>Δμ){Ĉ=Ȏ;Δμ=ΒΦ;ΗΟ=ΔЄ;}
if(ĩ==null||ΒΦ<Δύ){ĩ=Ȏ;Δύ=ΒΦ;Ηζ=ΔЄ;}}if(Ĉ==null||ĩ==null||Ĉ==ĩ) return;if(checkConveyorSystem&&!Ĉ.GetInventory(0).IsConnectedTo(ĩ.GetInventory(0))){if(ΔΑ>20) Λ="Inventory balancing failed";else ΔΑ++;return;}ΔΑ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
Ĉ.GetInventory(0).GetItems(Ћ);float Αα=0;if(Ћ.Count==0) continue;MyInventoryItem ΒΧ=Ћ[0];String ΗΧ=ΒΧ.Type.TypeId+ΒΧ.Type.SubtypeId;if(!ΘΛ.TryGetValue(ΗΧ,out Αα)){if(ΘЊ(Ĉ.GetInventory(0),0,ĩ.GetInventory(0),out Αα))
ΘΛ.Add(ΗΧ,Αα);else return;}float ΚЂ=((Δμ-Ζν)*ΗΟ/Αα);float ΚЃ=((Ζν-Δύ)*Ηζ/Αα);int Εή=(int) Math.Min(ΚЃ,ΚЂ);if(Εή<=0) return;if((float) ΒΧ.Amount<Εή)
Ĉ.GetInventory(0).TransferItemTo(ĩ.GetInventory(0),0,null,null,ΒΧ.Amount);else
Ĉ.GetInventory(0).TransferItemTo(ĩ.GetInventory(0),0,null,null,Εή);}}bool ΘЊ(IMyInventory ΑΥ,int Β,IMyInventory ΙЃ,out float ΘΚ){ΘΚ=0;float Ιτ=(float) ΑΥ.CurrentVolume;List<MyInventoryItem>ΔΓ=new List<MyInventoryItem>();ΑΥ.GetItems(ΔΓ);float Ζρ=0;for(int i=0;i<ΔΓ.Count;i++) Ζρ+=(float) ΔΓ[i].Amount;
ΑΥ.TransferItemTo(ΙЃ,Β,null,null,1);float ΘΘ=Ιτ-(float) ΑΥ.CurrentVolume;List<MyInventoryItem>ΓЉ=new List<MyInventoryItem>();ΑΥ.GetItems(ΓЉ);float Ζζ=0;for(int i=0;i<ΓЉ.Count;i++) Ζζ+=(float) ΓЉ[i].Amount;if(ΘΘ==0f||!Γά(0.9999,Ζρ-Ζζ,1.0001))
return false;ΘΚ=ΘΘ;return true;}void Ιί(){Vector3 ǉ=Ѐ+ı;Vector3 ȍ=ǉ-δ;if(ȍ.Length()==0) ȍ=new Vector3(0,0,-1);Vector3 Βη=т(Ϊ,ȍ);Vector3 ΗБ=Vector3.Normalize(Βη);Vector3 Ƃ=т(Ϊ,Ϊ.GetNaturalGravity());float Κς=ΔЍ>0?Math.Max(0,1-ţ(ȍ,ΗΖ)/5):0;float Ч=(float) Math.Min((ΔЁ>0?ΔЁ:1000f),Math.Max(Αι(-Βη,Ƃ,null),ΔЍ*Κς));
if(!ΘΒ) Ч=0;if(ΗЂ) Ч=Math.Max(0,1-ΒΘ/ΑЎ)*Ч;if(generalSpeedLimit>0) Ч=Math.Min(generalSpeedLimit,Ч);if(Ƌ) Ч=0;if(ȑ>=0) Ч=Math.Min(Ч,ȑ);if(ΗЃ) Ч*=(float) Math.Min(1,Й(ȍ.Length(),wpReachedDist)/2);Vector3 Ηγ=т(Ϊ,Ϊ.GetShipVelocities().LinearVelocity);float Ιο=(float)(Math.Max(0,15-ţ(-ΗБ,-Ηγ))/15)*0.85f+0.15f;
ΕΤ+=Math.Sign(Ιο-ΕΤ)/10f;Vector3 Ĥ=ΗБ*Ч*ΕΤ-(Ηγ);Vector3 ΓА=ΓΥ(Ĥ,null);if(Βώ&&а>0.1f){Ĥ.X*=Γώ(Ĥ.X,ref ΒϏ.X,1f,ΓА.X,20);Ĥ.Y*=Γώ(Ĥ.Y,ref ΒϏ.Y,1f,ΓА.Y,20);Ĥ.Z*=Γώ(Ĥ.Z,ref ΒϏ.Z,1f,ΓА.Z,20);}else ΒϏ=new Vector3(1,1,1);ΑΫ=Ś*Ĥ-Ƃ*Ś;Ελ(ΑΫ,Βώ);а=Vector3.Distance(δ,ǉ);
}float Γώ(float value,ref float Ѓ,float ǝ,float Đ,float ΔЂ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ѓ)) Ѓ+=Math.Sign(Ѓ)*ǝ;else Ѓ=value;if(value==0) Ѓ=1;float ή=Math.Abs(Ѓ);if(ή<ΔЂ||Đ==0) return ή;Ѓ=Math.Min(ΔЂ,Math.Max(-ΔЂ,Ѓ));ή=Math.Abs(Đ);return ή;
}void Ζδ(){float ǎ=90,ǡ=90,Ǜ=90;float Ǵ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȅ,ǘ,ΑΙ,Δϊ;if(Δϋ){Ȅ=Vector3.Normalize(Ѐ-δ);ΑΙ=т(Ϊ,Ȅ);ǘ=т(Ϊ,ΕΡ);ǎ=ţ(ΑΙ,new Vector3(0,-1,0))-90;ǡ=Ķ(ǘ,new Vector3(-1,0,0),ǘ.Y);
Ǜ=Ķ(ΑΙ,new Vector3(-1,0,0),ΑΙ.Z);}else{Ȅ=ΕΣ;ǘ=т(Ϊ,ΕΡ);ΑΙ=т(Ϊ,ΕΣ);Δϊ=т(Ϊ,ΘΑ);ǎ=Ķ(ǘ,new Vector3(0,0,1),ǘ.Y);ǡ=Ķ(ǘ,new Vector3(-1,0,0),ǘ.Y);Ǜ=Ķ(Δϊ,new Vector3(0,0,1),Δϊ.X);}if(Ζθ&&Αψ()){Vector3 Δγ=Ϊ.GetNaturalGravity();ǘ=т(Ϊ,Δγ);ǎ=Ķ(ǘ,new Vector3(0,0,1),ǘ.Y);
ǡ=Ķ(ǘ,new Vector3(-1,0,0),ǘ.Y);}if(!Γά(-45,ǡ,45)){ǎ=0;Ǜ=0;}if(!Γά(-45,Ǜ,45)) ǎ=0;ΑІ(Δε,1,(-ǎ)*Ǵ,(-Ǜ)*Ǵ,(-ǡ)*Ǵ);ΒΘ=Math.Max(Math.Abs(ǎ),Math.Max(Math.Abs(ǡ),Math.Abs(Ǜ)));Βή=ΒΘ<=ΑЎ;}void ΘΩ(IMyTerminalBlock Βς){if(Βς==null) return;Ƿ=new float[3,2];Ǟ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȭ.Count;i++){IMyThrust ч=Ȭ[i];if(!ч.IsFunctional) continue;Vector3 ρ=т(Βς,ч.WorldMatrix.Backward);float ħ=ч.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ƿ[0,0]+=ħ;else Ƿ[0,1]-=ħ;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ƿ[1,0]+=ħ;else Ƿ[1,1]-=ħ;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ƿ[2,0]+=ħ;else Ƿ[2,1]-=ħ;String ǐ=ΕЉ(ч);float[,]ǟ=null;if(Ǟ.ContainsKey(ǐ)) ǟ=Ǟ[ǐ];else{ǟ=new float[3,2];Ǟ.Add(ǐ,ǟ);}float Đ=ч.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ǟ[0,0]+=Đ;else ǟ[0,1]-=Đ;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ǟ[1,0]+=Đ;else ǟ[1,1]-=Đ;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ǟ[2,0]+=Đ;else ǟ[2,1]-=Đ;}}static String ΕЉ(IMyThrust ч){return ч.BlockDefinition.SubtypeId;}Vector3 Γβ(Vector3 Ǖ,float[,]Ƅ){return new Vector3(
Ǖ.X>=0?Ƅ[0,0]:Ƅ[0,1],
Ǖ.Y>=0?Ƅ[1,0]:Ƅ[1,1],
Ǖ.Z>=0?Ƅ[2,0]:Ƅ[2,1]);}bool ΕЈ(Ϋ ά,String ǐ,out float Δά){Δά=0;int Β=ȫ.IndexOf(ǐ);if(Β==-1||ά.Ƕ==null||Β>=ά.Ƕ.Length)
return false;Δά=ά.Ƕ[Β];if(Δά==-1) return false;return true;}Vector3 ΓΥ(Vector3 Ǖ,Ϋ ά){if(ά!=null){Vector3 ή=new Vector3();for(int i=0;i<Ǟ.Keys.Count;i++){String ǐ=Ǟ.Keys.ElementAt(i);float Ǉ=0;if(!ΕЈ(ά,ǐ,out Ǉ))
return Γβ(Ǖ,Ƿ);ή+=Γβ(Ǖ,Ǟ.Values.ElementAt(i))*Ǉ;}return ή;}return Γβ(Ǖ,Ƿ);}float Ƨ(Vector3 Ǖ,Ϋ ά){return Ƨ(Ǖ,new Vector3(),ά);}float Ƨ(Vector3 Ǖ,Vector3 φ,Ϋ ά){Vector3 Κϊ=ΓΥ(Ǖ,ά);Vector3 ħ=Κϊ+φ*Ś;float ŗ=(ħ/Ǖ).AbsMin();return(float)(Ǖ*ŗ).Length();}float Αι(Vector3 Βε,Vector3 Ƃ,Ϋ ά)
{if(Βε.Length()==0) return 0;float Ζη=1;if(Ƃ.Length()>0) Ζη=Math.Min(1,ţ(-Ƃ,Βε)/90)*0.4f+0.6f;float ΑΫ=Ƨ(Βε,Ƃ,ά);if(ΑΫ==0) return 0.1f;float ΑЍ=Й(ΑΫ,Ś);float ΕΧ=(float) Math.Sqrt(Й(Βε.Length(),ΑЍ*0.5f));return ΑЍ*ΕΧ*Ζη*ǻ;}float Αυ(Vector3 Ȅ,Vector3 Б,Vector3 φ,Ϋ ά)
{if(φ.Length()==0f) return 0;Vector3 Βζ=ľ(Ȅ,Б,Vector3.Normalize(φ));float Ǭ=Ƨ(-Βζ,ά);return Ǭ/φ.Length();}void Επ(bool ΚΕ){float Ǡ=0;float Δΰ=0.9f;if(ΚΕ){в=-1;Ş=0;Βω=null;if(β!=Π.ь&&Ζ.φ.Length()!=0){Ǡ=Δΰ*Αυ(Ζ.ι,Ζ.θ*-1,Ζ.φ,null);if(Ǡ<в||в==-1) в=Ǡ;}if(Ο.κ&&Ο.φ.Length()!=0)
{Ǡ=Δΰ*Αυ(Ο.ι,Ο.θ*-1,Ο.φ,null);if(Ǡ<в||в==-1) в=Ǡ;}return;}if(Ş==-1) return;if(Ş>=0){int Ηϋ=0;while(Ş<Φ.Count){if(Ηϋ>100) return;Ηϋ++;Ϋ ά=Φ[Ş];if(ά.φ.Length()!=0f){Ǡ=Δΰ*Math.Min(
Αυ(ά.ι,ά.θ*-1,ά.φ,ά),
Αυ(ά.ι*-1,ά.θ*-1,ά.φ,ά));if(Ǡ<в||в==-1) в=Ǡ;}else Βω=ά;Ş++;}Ş=-1;}bool Δθ=true;float Į=0;if(Βω!=null){for(int i=0;i<Ǟ.Count;i++){String ǐ=Ǟ.Keys.ElementAt(i);float[,]ǟ=Ǟ.Values.ElementAt(i);float Ǉ=0;if(!ΕЈ(Βω,ǐ,out Ǉ)){Δθ=false;break;}for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float Ł=Math.Abs(ǟ[a,b]*Ǉ);if(Ł==0) continue;Δθ=true;if(Į==0||Ł<Į) Į=Ł;}}}if(!Δθ){for(int a=0;a<Ƿ.GetLength(0);a++)
for(int b=0;b<Ƿ.GetLength(1);b++){float Ł=Math.Abs(Ƿ[a,b]);if(Ł==0) continue;if(Į==0||Ł<Į) Į=Ł;}}if(Į>0){float ΚΡ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǡ=Й(Į,ΚΡ);if(Ǡ>0)
if(Ǡ<в||в==-1) в=Ǡ;}}void Ελ(Vector3 Ǭ,bool М){if(!М){for(int i=0;i<Ȭ.Count;i++)
Ȭ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓЏ=ΓΥ(Ǭ,null);float ΛΑ=Math.Min(1,Math.Abs(Й(Ǭ.X,ΓЏ.X)));float ΛΒ=Math.Min(1,Math.Abs(Й(Ǭ.Y,ΓЏ.Y)));float ΛΓ=Math.Min(1,Math.Abs(Й(Ǭ.Z,ΓЏ.Z)));for(int i=0;i<Ȭ.Count;i++){IMyThrust ч=Ȭ[i];Vector3 ρ=ΙΔ(т(Ϊ,ч.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǭ.X))
ч.SetValueFloat("Override",ч.MaxThrust*ΛΑ);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǭ.Y))
ч.SetValueFloat("Override",ч.MaxThrust*ΛΒ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǭ.Z))
ч.SetValueFloat("Override",ч.MaxThrust*ΛΓ);else
ч.SetValueFloat("Override",0.0f);}}void ΑІ(bool М,float Κί,float ǎ,float Ǜ,float ǡ){for(int i=0;i<ΒΩ.Count;i++){IMyGyro К=ΒΩ[i];К.GyroOverride=М;if(!М) К.GyroPower=100;else К.GyroPower=Κί;if(!М) continue;Vector3 Ы=Ϊ.WorldMatrix.Forward;Vector3 о=Ϊ.WorldMatrix.Right;Vector3 Б=Ϊ.WorldMatrix.Up;
Vector3 ź=К.WorldMatrix.Forward;Vector3 ż=К.WorldMatrix.Up;Vector3 Ż=К.WorldMatrix.Left*-1;if(ź==Ы) К.SetValueFloat("Roll",ǡ);else if(ź==(Ы*-1)) К.SetValueFloat("Roll",ǡ*-1);else if(ż==(Ы*-1)) К.SetValueFloat("Yaw",ǡ);else if(ż==Ы) К.SetValueFloat("Yaw",ǡ*-1);else if(Ż==Ы) К.SetValueFloat("Pitch",ǡ);
else if(Ż==(Ы*-1)) К.SetValueFloat("Pitch",ǡ*-1);if(Ż==(о*-1)) К.SetValueFloat("Pitch",ǎ);else if(Ż==о) К.SetValueFloat("Pitch",ǎ*-1);else if(ż==о) К.SetValueFloat("Yaw",ǎ);else if(ż==(о*-1)) К.SetValueFloat("Yaw",ǎ*-1);else if(ź==(о*-1)) К.SetValueFloat("Roll",ǎ);
else if(ź==о) К.SetValueFloat("Roll",ǎ*-1);if(ż==(Б*-1)) К.SetValueFloat("Yaw",Ǜ);else if(ż==Б) К.SetValueFloat("Yaw",Ǜ*-1);else if(Ż==Б) К.SetValueFloat("Pitch",Ǜ);else if(Ż==(Б*-1)) К.SetValueFloat("Pitch",Ǜ*-1);else if(ź==Б) К.SetValueFloat("Roll",Ǜ);else if(ź==(Б*-1)) К.SetValueFloat("Roll",Ǜ*-1);
}}void Ζΰ(){this.Δε=false;}void й(Vector3 Ħ,Vector3 Ы,Vector3 Δψ,float ė,bool Αΰ){ΒЋ(Ħ,ė,Αΰ);ΑЎ=ė;Δϋ=false;this.ΕΣ=Ы;this.ΘΑ=Δψ;}void й(Vector3 Ħ,Vector3 Ы,Vector3 Δψ,bool Αΰ){й(Ħ,Ы,Δψ,2f,Αΰ);}void ΒЋ(Vector3 Ħ,float ė,bool Αΰ){ΑЎ=ė;this.Δε=true;this.Ζθ=Αΰ;Δϋ=true;
Βή=false;this.ΕΡ=Ħ;}void ΓЃ(){ǹ(false,false,false,Ѐ,0);Βώ=false;}void е(Vector3 η,float Ǵ){ǹ(true,false,false,η,Ǵ);}void ǹ(bool М,bool ΕΜ,bool Εα,Vector3 η,float Ǵ){ǹ(М,ΕΜ,Εα,η,η-δ,0.0f,Ǵ);}void ǹ(bool М,bool ΕΜ,bool Εα,Vector3 η,Vector3 Κμ,float Κν,float Ǵ){Βώ=true;
this.ΘΒ=М;Ѐ=η;this.ΔЁ=Ǵ;this.ΔЍ=Κν;this.ΗЃ=ΕΜ;this.ΗЂ=Εα;this.ΗΖ=Κμ;а=Vector3.Distance(η,δ);}void ΙΖ(){χ.GetBlocksOfType(Ʒ,ǒ);}void ΙΗ(){if(Βτ){А=2;return;}List<IMyRemoteControl>ΑΟ=new List<IMyRemoteControl>();List<IMySensorBlock>ΗϏ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑΩ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(ΑΟ,ǒ);χ.GetBlocksOfType(Ц,ǒ);χ.GetBlocksOfType(ΗϏ,ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΩ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΨ();ΖΩ(ΑΩ);Ľ(Ц,setLCDFontAndSize,1.4f,false);Ľ(Ȍ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΗЍ=Ǣ(ΗϏ,pamTag,true);if(ΗЍ.Count>0) ψ=ΗЍ[0];else ψ=null;if(Κ==Ι.Ʈ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Κ==Ι.ǣ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Κ==Ι.έ){χ.GetBlocksOfType(ΕΨ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΗЌ=Ǣ(ΑΟ,pamTag,true);if(ΗЌ.Count>0) ΑΟ=ΗЌ;if(ΑΟ.Count>0) Ϊ=ΑΟ[0];else{Ϊ=null;А=2;Λ="Remote is missing";return;}ο=(float) Ϊ.CubeGrid.WorldVolume.Radius*2;ǚ=null;if(Κ!=Ι.έ){Ερ();if(ω.Count>0&&ǚ!=null){if(ψ!=null&&(ǚ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ϊ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ϊ.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(ǚ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Forward&&
ǚ.WorldMatrix.Forward!=Ϊ.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void ΙΘ(){χ.GetBlocksOfType(Ȩ,ǒ);χ.GetBlocksOfType(ј,ǒ);χ.GetBlocksOfType(Ȭ,ǒ);χ.GetBlocksOfType(ΒΩ,ǒ);χ.GetBlocksOfType(Ģ,ǒ);χ.GetBlocksOfType(į,ǒ);χ.GetBlocksOfType(ǖ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Βψ,ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ј=Ǣ(ј,"ConnectorMedium",false);else
ј=Ǣ(ј,"Connector",false);List<IMyShipConnector>ΗЊ=Ǣ(ј,pamTag,true);if(ΗЊ.Count>0) ј=ΗЊ;if(А<=1){if(ј.Count==0){А=1;Λ="Connector is missing";}if(ΒΩ.Count==0){А=1;Λ="Gyros are missing";}if(Ȭ.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΗЎ=Ǣ(Βψ,pamTag,true);
if(ΗЎ.Count>0) Βψ=ΗЎ;List<IMyLandingGear>ΗЋ=Ǣ(Ȩ,pamTag,true);if(ΗЋ.Count>0) Ȩ=ΗЋ;for(int i=0;i<Ȩ.Count;i++) Ȩ[i].AutoLock=false;List<IMyBatteryBlock>ΗЉ=Ǣ(Ģ,pamTag,true);if(ΗЉ.Count>0) Ģ=ΗЉ;List<IMyGasTank>ΗЏ=Ǣ(ǖ,pamTag,true);if(ΗЏ.Count>0) ǖ=ΗЏ;}void ΙΙ(){χ.GetBlocksOfType(ł,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ŷ.Clear();for(int i=ł.Count-1;i>=0;i--){if(ΘЃ(ł[i])){ŷ.Add(ł[i]);ł.RemoveAt(i);}}}bool ΘЃ(IMyTerminalBlock Ψ){if(Ψ is IMyCargoContainer) return true;if(Ψ is IMyShipDrill) return true;if(Ψ is IMyShipGrinder) return true;if(Ψ is IMyShipConnector){if(((IMyShipConnector) Ψ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓΩ(Ψ,"ConnectorSmall",false))
return false;else return true;}return false;}void Ερ(){if(Ϊ==null) return;ǚ=null;float Βν=0,Βθ=0,Βξ=0,Βι=0,Βο=0,Βκ=0;List<IMyTerminalBlock>ΕΠ=Ǣ(ω,pamTag,true);bool ΚΨ=ΕΠ.Count==0;if(ΕΠ.Count>0) ǚ=ΕΠ[0];else if(ω.Count>0) ǚ=ω[0];int ȡ=0;float Ǝ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward){if(ΚΨ){А=2;Λ="Mining direction is unclear!";return;}continue;}ȡ++;Ǝ=Math.Max(Ǝ,ΙΨ(ω[i]));Vector3 Н=Ατ(Ϊ,ω[i].GetPosition());if(i==0){Βν=Н.X;Βθ=Н.X;Βξ=Н.Y;Βι=Н.Y;Βο=Н.Z;Βκ=Н.Z;}Βθ=Math.Max(Н.X,Βθ);Βν=Math.Min(Н.X,Βν);
Βι=Math.Max(Н.Y,Βι);Βξ=Math.Min(Н.Y,Βξ);Βκ=Math.Max(Н.Z,Βκ);Βο=Math.Min(Н.Z,Βο);}if(Ǝ==0) Ǝ=drillRadius;bool Ħ=ǚ!=null&&ǚ.WorldMatrix.Forward==Ϊ.WorldMatrix.Down;Ȑ=(Βθ-Βν)*(1-ş)+Ǝ*2;ȏ=(Ħ?Βκ-Βο:Βι-Βξ)*(1-Ŋ)+Ǝ*2;IMyCubeGrid g=Ϊ.CubeGrid;float Εε=0,Εζ=0,Εη=0,Εθ=0;
for(int c=0;c<8;c++){Vector3 p=Ατ(Ϊ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ħ?p.Z:p.Y;if(c==0||p.X<Εε) Εε=p.X;if(c==0||p.X>Εζ) Εζ=p.X;if(c==0||y<Εη) Εη=y;if(c==0||y>Εθ) Εθ=y;}Ɩ=Εζ-Εε+g.GridSize;ΑΣ=Εθ-Εη+g.GridSize;}float ΙΨ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǣ<T>(List<T>Ѝ,String ǵ,bool ΒΪ){List<T>ή=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(ΓΩ(Ѝ[i],ǵ,ΒΪ)) ή.Add(Ѝ[i]);return ή;}bool ΓΩ<T>(T Ψ,String ǵ,bool ΒΪ){IMyTerminalBlock b=(IMyTerminalBlock) Ψ;if(ΒΪ&&b.CustomName.ToUpper().Contains(ǵ.ToUpper())) return true;if(!ΒΪ&&b.BlockDefinition.ToString().ToUpper().Contains(ǵ.ToUpper())) return true;return false;}void ǔ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
ΖΦ((IMyTerminalBlock) Ѝ[i],М);}void ΖΦ(IMyTerminalBlock Ψ,bool М){if(Ψ==null) return;String Ιν=М?"OnOff_On":"OnOff_Off";var a=Ψ.GetActionWithName(Ιν);a.Apply(Ψ);}void Ǻ(bool М){ΕΝ=М;if(!ś) return;ǔ(Βψ,М);}void Γϊ(ChargeMode Џ){for(int i=0;i<Ģ.Count;i++) Ģ[i].ChargeMode=Џ;}void Γό(bool Κτ)
{for(int i=0;i<ǖ.Count;i++) ǖ[i].Stockpile=Κτ;}void Ʊ(List<IMyLandingGear>Δα,bool Ηδ){for(int i=0;i<Δα.Count;i++){if(Ηδ) Δα[i].Lock();if(!Ηδ) Δα[i].Unlock();}}void ΖΧ(bool М){if(ΖЇ==М) return;List<IMyShipController>ΒΖ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΖ,ǒ);if(ΒΖ.Count==0) return;
for(int i=0;i<ΒΖ.Count;i++)
ΒΖ[i].DampenersOverride=М;ΖЇ=М;}IMyShipConnector д(MyShipConnectorStatus Ǐ){for(int i=0;i<ј.Count;i++){if(!Ȗ(ј[i],true)) continue;if(ј[i].Status==Ǐ) return ј[i];}return null;}void Ŀ(bool ΕΘ,bool ΒΜ,float ΙЍ,float Ισ){if(ψ==null||ω.Count==0) return;Vector3 ΒΒ=new Vector3();
int ȡ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward) continue;ȡ++;ΒΒ+=ω[i].GetPosition();}ΒΒ=ΒΒ/ȡ;Vector3 ƈ=Ατ(ψ,ΒΒ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΘ;ψ.LeftExtend=(ΒΜ?1:я)/2f*Ȑ-ƈ.X;ψ.RightExtend=(ΒΜ?1:я)/2f*Ȑ+ƈ.X;ψ.TopExtend=(ΒΜ?1:ю)/2f*ȏ+ƈ.Y;
ψ.BottomExtend=(ΒΜ?1:ю)/2f*ȏ-ƈ.Y;ψ.FrontExtend=(ΕΘ?л:ΙЍ)-ƈ.Z;ψ.BackExtend=ΕΘ?0:Ισ+ο*0.75f+ƈ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void Ľ<T>(List<T>Ѝ,bool Κο,float ΙЋ,bool ΚΖ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface ΑΨ=null;if(Ѝ[i]is IMyTextSurface) ΑΨ=(IMyTextSurface) Ѝ[i];if(ΑΨ!=null){ΑΨ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Κο) continue;ΑΨ.Font="Debug";if(ΚΖ) continue;ΑΨ.FontSize=ΙЋ;}}}bool Αψ(){Vector3D ν;return this.Ϊ.TryGetPlanetPosition(out ν);}int Θχ(Vector3 Κφ,int ΗЄ){if(ΗЄ==-1) return 0;double Щ=-1;int ą=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,Κφ);if(Щ==-1||Њ<Щ){ą=i;Щ=Њ;}
}return Math.Sign(ą-ΗЄ);}bool ΘЄ(Vector3 ν){List<Vector3>Ю=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Ю.Add(Φ[i].Ε);if(Ο.κ&&Φ.Count>=1){Vector3 Ų=new Vector3();ȕ(Ο,dockDist*ο,false,out Ų);if(Vector3.Distance(Ο.Ε,Φ.First().Ε)<Vector3.Distance(Ο.Ε,Φ.Last().Ε)){Ю.Insert(0,Ų);Ю.Insert(0,Ο.Ε);}
else{Ю.Add(Ų);Ю.Add(Ο.Ε);}}if(Κ==Ι.έ){if(Ζ.κ&&Φ.Count>=1){Vector3 ΓЋ=new Vector3();ȕ(Ζ,dockDist*ο,false,out ΓЋ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Ю.Insert(0,ΓЋ);Ю.Insert(0,Ζ.Ε);}else{Ю.Add(ΓЋ);Ю.Add(Ζ.Ε);}}}else{if(β!=Π.ь)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Ю.Insert(0,Ζ.Ε);else Ю.Add(Ζ.Ε);}int ą=-1;double ǿ=-1;for(int i=0;i<Ю.Count;i++){double Њ=Vector3.Distance(Ю[i],ν);if(Њ<ǿ||ǿ==-1){ǿ=Њ;ą=i;}}if(Ю.Count==0) return false;double ΔΦ=Vector3.Distance(Ю[ą],ν);double Κΰ=Vector3.Distance(Ю[Math.Max(0,ą-1)],Ю[ą])*1.5f;
double ΚΧ=Vector3.Distance(Ю[Math.Min(Ю.Count-1,ą+1)],Ю[ą])*1.5f;return ΔΦ<Κΰ||ΔΦ<ΚΧ||ΔΦ<laneOffset*ο*1.5f;}static float Й(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΙΔ(Vector3 v,int ΔΣ){return new Vector3(Math.Round(v.X,ΔΣ),Math.Round(v.Y,ΔΣ),Math.Round(v.Z,ΔΣ));
}float ţ(Vector3 a,Vector3 b){if(a==b) return 0;float ΙЈ=(a*b).Sum;float Ηί=a.Length();float Ηΰ=b.Length();if(Ηί==0||Ηΰ==0) return 0;float ή=(float)((180.0f/Math.PI)*Math.Acos(ΙЈ/(Ηί*Ηΰ)));if(float.IsNaN(ή)) return 0;return ή;}float Ķ(Vector3 v,Vector3 Βς,float Κπ){float Ű=ţ(v,Βς);
if(Κπ>0f) Ű*=-1;if(Ű>-90f) return Ű-90f;else return 180f-(-Ű-90f);}double Εψ(float ΙЁ){return(Math.PI/180)*ΙЁ;}bool Γά(double ΚΠ,double value,double ƅ){return(value>=ΚΠ&&value<=ƅ);}Vector3 ΙΩ(IMyTerminalBlock Ψ,Vector3 Н){return Vector3D.Transform(Н,Ψ.WorldMatrix);
}Vector3 Ατ(IMyTerminalBlock Ψ,Vector3 ΚЏ){return т(Ψ,ΚЏ-Ψ.GetPosition());}Vector3 т(IMyTerminalBlock Ψ,Vector3 Εδ){return Vector3D.TransformNormal(Εδ,MatrixD.Transpose(Ψ.WorldMatrix));}Vector3 ľ(Vector3 Ȅ,Vector3 Б,Vector3 Εδ){MatrixD ΔЀ=MatrixD.CreateFromDir(Ȅ,Б);return Vector3D.TransformNormal(Εδ,MatrixD.Transpose(ΔЀ));
}Vector3 ΓЅ(Vector3 Ȅ,Vector3 Б,Vector3 Н){MatrixD ΔЀ=MatrixD.CreateFromDir(Ȅ,Б);return Vector3D.Transform(Н,ΔЀ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ρ(τ,0)),float.Parse(Ρ(τ,1)),float.Parse(Ρ(τ,2)));
}String Ιλ(Ϋ ά){String П=":";String ή=σ(ά.Ε)+П+σ(ά.ι)+П+
σ(ά.θ)+П+σ(ά.Ђ)+П+σ(ά.φ);for(int i=0;i<ά.Ƕ.Length;i++)
ή+=П+Math.Round(ά.Ƕ[i],3);return ή;}Ϋ ΙΤ(String s){String[]τ=s.Split(':');Ϋ ά=new Ϋ(
ς(Ρ(τ,0)),ς(Ρ(τ,1)),
ς(Ρ(τ,2)),ς(Ρ(τ,3)),
ς(Ρ(τ,4)));int Β=5;List<float>ΗΓ=new List<float>();while(Β<τ.Length){String ΒЅ=Ρ(τ,Β);float f=0;if(!float.TryParse(ΒЅ,out f)) break;ΗΓ.Add(f);Β++;}ά.Ƕ=ΗΓ.ToArray();return ά;}System.Text.StringBuilder ƒ=new System.Text.StringBuilder();void Υ<T>(T value,bool ΚΦ){if(ΚΦ) ƒ.Append('\n');ƒ.Append(value);}
void Υ<T>(T value){Υ(value,true);}String Αν(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑЊ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ρ(String[]Ιρ,int Β){String ΒЅ=Ιρ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΒЅ)) return "";
return ΒЅ;}void Save(){if(Βτ||Κ==Ι.і){Storage="";return;}Storage=ΒБ();}String ΒБ(){ƒ.Clear();ƒ.Append(DATAREV+";");Υ(σ(Βσ),false);Υ(σ(Ο.ι));Υ(σ(Ο.Ђ));Υ(σ(Ο.θ));Υ(σ(Ο.φ));Υ(σ(Ο.Ε));Υ(σ(Ο.м));Υ(Ο.κ);Υ(σ(Ζ.Ε));Υ(σ(Ζ.φ));Υ(σ(Ζ.ι));Υ(σ(Ζ.θ));Υ(σ(Ζ.Ђ));Υ(σ(Ζ.м));Υ(Ζ.κ);Υ(σ(Ǳ));Υ(σ(đ));Υ(";");
Υ((int) Κ,false);Υ((int) β);Υ((int) ќ);Υ(Ȇ);Υ(ѝ);Υ(н);Υ(ў);Υ(ǻ);Υ(ę);Υ(Ę);if(Κ==Ι.έ){Υ((int) І.Р);Υ(І.Ǫ);Υ(І.Ĩ);Υ(Αν(І.ǫ));Υ(Αν(І.Ǯ));Υ((int) Ї.Р);Υ(Ї.Ǫ);Υ(Ї.Ĩ);Υ(Αν(Ї.ǫ));Υ(Αν(Ї.Ǯ));}else{Υ((int) Ȋ);Υ((int) ё);Υ((int) б);Υ((int) љ);Υ((int) Ŏ);Υ(Ĳ);Υ(ś);Υ(ſ);Υ(Ź);Υ(я);Υ(ю);Υ(л);Υ(Ƣ);Υ(ơ);
Υ(ş);Υ(Ŋ);Υ(Ќ);Υ(Ь);Υ(Ϗ);Υ(ό);Υ(ď);Υ(Я);Υ(Ř);Υ(ő);}Υ(";");for(int i=0;i<ȫ.Count;i++)
Υ((i>0?"|":"")+ȫ[i],false);Υ(";");for(int i=0;i<Φ.Count;i++)
Υ(Ιλ(Φ[i]),i>0);Υ(";",false);Υ(з+"|"+Д,false);Υ(";",false);Υ(Θθ(),false);String Ξ=ƒ.ToString();ƒ.Clear();return Ξ;}ї ΙΓ(){return ΑЂ(Storage);}ї ΑЂ(String Ξ){if(Ξ=="") return ї.ΘЌ;String[]ř=Ξ.Split(';');if(Ρ(ř,0)!=DATAREV) return ї.Γθ;int Β=0;try{String[]Ν=Ρ(ř,1).Split('\n');
Βσ=ς(Ρ(Ν,Β++));Ο.ι=ς(Ρ(Ν,Β++));Ο.Ђ=ς(Ρ(Ν,Β++));Ο.θ=ς(Ρ(Ν,Β++));Ο.φ=ς(Ρ(Ν,Β++));Ο.Ε=ς(Ρ(Ν,Β++));Ο.м=ς(Ρ(Ν,Β++));Ο.κ=bool.Parse(Ρ(Ν,Β++));Ζ.Ε=ς(Ρ(Ν,Β++));Ζ.φ=ς(Ρ(Ν,Β++));Ζ.ι=ς(Ρ(Ν,Β++));Ζ.θ=ς(Ρ(Ν,Β++));Ζ.Ђ=ς(Ρ(Ν,Β++));Ζ.м=ς(Ρ(Ν,Β++));Ζ.κ=bool.Parse(Ρ(Ν,Β++));
Ǳ=ς(Ρ(Ν,Β++));đ=ς(Ρ(Ν,Β++));Ν=Ρ(ř,2).Split('\n');Β=0;Κ=(Ι) int.Parse(Ρ(Ν,Β++));β=(Π) int.Parse(Ρ(Ν,Β++));ќ=(Π) int.Parse(Ρ(Ν,Β++));Ȇ=int.Parse(Ρ(Ν,Β++));ѝ=int.Parse(Ρ(Ν,Β++));н=int.Parse(Ρ(Ν,Β++));ў=int.Parse(Ρ(Ν,Β++));ǻ=float.Parse(Ρ(Ν,Β++));ę=bool.Parse(Ρ(Ν,Β++));
Ę=bool.Parse(Ρ(Ν,Β++));if(Κ==Ι.έ){І.Р=(π) int.Parse(Ρ(Ν,Β++));І.Ǫ=float.Parse(Ρ(Ν,Β++));І.Ĩ=float.Parse(Ρ(Ν,Β++));І.ǫ=ΑЊ(Ρ(Ν,Β++));І.Ǯ=ΑЊ(Ρ(Ν,Β++));Ї.Р=(π) int.Parse(Ρ(Ν,Β++));Ї.Ǫ=float.Parse(Ρ(Ν,Β++));Ї.Ĩ=float.Parse(Ρ(Ν,Β++));Ї.ǫ=ΑЊ(Ρ(Ν,Β++));Ї.Ǯ=ΑЊ(Ρ(Ν,Β++));}else
{Ȋ=(э) int.Parse(Ρ(Ν,Β++));ё=(щ) int.Parse(Ρ(Ν,Β++));б=(Г) int.Parse(Ρ(Ν,Β++));љ=(Ǒ) int.Parse(Ρ(Ν,Β++));Ŏ=(э) int.Parse(Ρ(Ν,Β++));Ĳ=bool.Parse(Ρ(Ν,Β++));ś=bool.Parse(Ρ(Ν,Β++));ſ=bool.Parse(Ρ(Ν,Β++));Ź=bool.Parse(Ρ(Ν,Β++));я=int.Parse(Ρ(Ν,Β++));ю=int.Parse(Ρ(Ν,Β++));л=int.Parse(Ρ(Ν,Β++));
Ƣ=float.Parse(Ρ(Ν,Β++));ơ=float.Parse(Ρ(Ν,Β++));ş=float.Parse(Ρ(Ν,Β++));Ŋ=float.Parse(Ρ(Ν,Β++));Ќ=int.Parse(Ρ(Ν,Β++));Ь=int.Parse(Ρ(Ν,Β++));Ϗ=int.Parse(Ρ(Ν,Β++));ό=int.Parse(Ρ(Ν,Β++));ď=int.Parse(Ρ(Ν,Β++));Я=int.Parse(Ρ(Ν,Β++));Ř=int.Parse(Ρ(Ν,Β++));ő=float.Parse(Ρ(Ν,Β++));
}Ν=Ρ(ř,3).Replace("\n","").Split('|');ȫ=Ν.ToList();Ν=Ρ(ř,4).Split('\n');Φ.Clear();if(Ν.Count()>=1&&Ν[0]!="")
for(int i=0;i<Ν.Length;i++)
Φ.Add(ΙΤ(Ρ(Ν,i)));Ν=Ρ(ř,5).Split('|');з=Ρ(Ν,0);if(!int.TryParse(Ρ(Ν,1),out Д)) Д=-1;if(з.Length!=Ќ*Ь){з="";Д=-1;}Θη(Ρ(ř,6));}catch{return ї.Εϋ;}Ɠ=ќ;Ё();return ї.ΑЈ;}const int Εο=64000;IMyTerminalBlock Εό(){List<IMyTerminalBlock>ŵ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(ŵ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ŵ.Count>0?ŵ[0]:null;}void Θμ(){IMyTerminalBlock Ψ=Εό();if(Ψ==null){Λ="Export: name a block "+dataTag;return;}String Ξ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒБ();if(Ξ.Length>Εο){Λ="Export too large: "+Ξ.Length+" chars, max "+Εο;return;}Ψ.CustomData=Ξ;Λ="Exported "+Φ.Count+" waypoints";}void ΘЀ(bool Δς){IMyTerminalBlock Ψ=Εό();if(Ψ==null){Λ="Import: name a block "+dataTag;return;}
ΘЁ(Ψ.CustomData,Δς);}void ΘЁ(String Ξ,bool Δς){Ξ=Ξ.Replace("\r","");if(Ξ.StartsWith("//")){int Ης=Ξ.IndexOf('\n');Ξ=Ης<0?"":Ξ.Substring(Ης+1);}Ξ=Ξ.Trim();if(Ξ==""){Λ="Import: no PAM data in "+dataTag;return;}Ё();String Ζο=ΒБ();ї ή=ΑЂ(Ξ);if(ή==ї.ΑЈ&&Δς){Ϋ Χ=new Ϋ(Ζ);
Vector3 ΚΤ=Ǳ,ΚΣ=đ;int w=я,h=ю,d=л;э ΑΦ=Ȋ;Ǒ ΔΤ=љ;float ΚΫ=ş,ΚΪ=Ŋ;ΑЂ(Ζο);Ζ=Χ;Ǳ=ΚΤ;đ=ΚΣ;я=w;ю=h;л=d;Ȋ=ΑΦ;љ=ΔΤ;ş=ΚΫ;Ŋ=ΚΪ;β=Π.ū;Ƶ(true,true);β=Π.ū;ќ=Π.ū;Λ="Imported job, run CONT";return;}if(ή==ї.ΑЈ){ΑΜ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}ΑЂ(Ζο);Λ=ή==ї.Γθ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖΨ(){ņ.Clear();for(int i=Ц.Count-1;i>=0;i--){String Ξ=Ц[i].CustomData.ToUpper();bool Δπ=false;if(Ξ=="INSTRUCTIONS"){Δπ=true;ΒΛ=true;}if(Ξ=="DEBUG") Δπ=true;if(Δπ){ņ.Add(Ц[i]);Ц.RemoveAt(i);}
}Ľ(ņ,false,1,false);}void ΖΩ(List<IMyTerminalBlock>ŵ){Ȍ.Clear();for(int i=0;i<ŵ.Count;i++){IMyTerminalBlock Ψ=ŵ[i];try{String Ηϊ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ψ.CustomName.IndexOf(Ηϊ);int ƙ=-1;if(Β<0||!int.TryParse(Ψ.CustomName.Substring(Β+Ηϊ.Length,1),out ƙ)) continue;
if(ƙ==-1) continue;ƙ--;IMyTextSurfaceProvider Ηό=(IMyTextSurfaceProvider) Ψ;if(ƙ<Ηό.SurfaceCount&&ƙ>=0)
Ȍ.Add(Ηό.GetSurface(ƙ));}catch{}}}String Βλ="";void ΖΣ(){String ΔϏ="";String ǆ="";Βλ=Εξ(false);ΔϏ+=Βλ;ǆ+=Βλ;ǆ+=Θτ();for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(ΔϏ);for(int i=0;i<Ȍ.Count;i++) Ȍ[i].WriteText(ΔϏ);Echo(ǆ);for(int i=0;i<ņ.Count;i++){IMyTextPanel ц=ņ[i];
String Ξ=ц.CustomData.ToUpper();if(Ξ=="DEBUG") ц.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") ц.WriteText(ΕЃ());}}string ΕЃ(){String Γ="";try{float Ηη=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ŵ*Ηη)+" / "+(int)(Ɔ*Ηη)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ŵ*100f,1)+"% / "+Math.Round(Ɔ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ƻ,2)+"ms / "+Math.Round(ΔЃ,2)+"ms \n";}catch{}for(int i=0;i<Ē.Count;i++)
Γ+=""+Ē.Keys.ElementAt(i)+" = "+Math.Round(Ē.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ē.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Θύ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ľ(Ц,setLCDFontAndSize,0.9f,false);Ľ(Ȍ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Γ+Ď.ToString());for(int i=0;i<Ȍ.Count;i++) Ȍ[i].WriteText(Γ+Ď.ToString());}void Ț(){if(!ΒΛ) return;try{ΔБ=Runtime.CurrentInstructionCount;}catch{}
}void Ȕ(String ί){if(!ΒΛ) return;if(ΔБ==0) return;try{float ΒЃ=(Runtime.CurrentInstructionCount-ΔБ)/Runtime.MaxInstructionCount*100;if(!Ē.ContainsKey(ί))
Ē.Add(ί,new float[]{ΒЃ,ΒЃ});else{Ē[ί][0]=ΒЃ;Ē[ί][1]=Math.Max(ΒЃ,Ē[ί][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΘΡ(String ȃ){if(!Ŷ) return;String П=""+BROADCAST_SEP;String ζ=VERSION+П;ζ+=ƣ(Ɨ)+П;
ζ+=(int) Κ+П;ζ+=Ŧ(δ.X)+""+П;ζ+=Ŧ(δ.Y)+П;ζ+=Ŧ(δ.Z)+П;if(Κ!=Ι.έ)
ζ+=ΓΡ(ќ)+(ќ==Π.В?" "+Math.Round(ΑΕ,1)+"%":"")+П;else ζ+=ΓΡ(ќ)+П;ζ+=ƣ(Λ)+П;ζ+=Ў+""+П;ζ+=ƣ(Βλ)+П;ζ+=ȁ+П;ζ+=ȇ+П;for(int i=0;i<ǭ.Count;i++){if(ǭ[i].ƃ!=Ш.Ũ) continue;ζ+=ƣ(ǭ[i].ί)+"/"+ƣ(ǭ[i].υ)+"/"+ǭ[i].Љ+П;}ΙΝ(ζ,ȃ);}String ƣ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΙΝ(String ζ,String ȃ){Γψ(ζ,BROADCAST_SELF_ID,ȃ);}void Γψ(String ζ,String η,String ȃ){try{if(!Ŷ) return;ζ=BROADCAST_TAG+BROADCAST_SEP+ΕϏ()+BROADCAST_SEP+η+BROADCAST_SEP+ΔΖ+BROADCAST_SEP+ȃ+BROADCAST_SEP+ζ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ζ);}catch(Exception e){Ď=e;}
}String ΕϏ(){if(Κ!=Ι.і) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γα(String ζ){return ζ.StartsWith(BROADCAST_TAG);}bool г(ref String Ξ,out string ǈ,Char П){int Β=Ξ.IndexOf(П);ǈ="";if(Β<0) return false;ǈ=Ξ.Substring(0,Β);Ξ=Ξ.Remove(0,Β+1);return true;}bool ΖΡ(ref String ζ,out String ȉ,out String ȃ)
{ȉ="";ȃ="";if(!Ŷ) return false;String ǵ="";if(!г(ref ζ,out ǵ,BROADCAST_SEP)||!Γα(ǵ)) return false;if(!г(ref ζ,out ȉ,BROADCAST_SEP)) return false;if(!г(ref ζ,out ǵ,BROADCAST_SEP)||(ǵ!=ΕϏ()&&(ǵ!="*"&&Κ!=Ι.і))) return false;if(!г(ref ζ,out ǵ,BROADCAST_SEP)||(ǵ!=ΔΖ)) return false;
if(!г(ref ζ,out ȃ,BROADCAST_SEP)) return false;return true;}void ΕБ(){bool ƿ=true;if(ΗΕ){ΗΕ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ΑΑ=(Κ!=Ι.і?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΑ)
ΖΑ(ΑΑ);String[]Ν=Me.CustomData.Split('\n');Ŷ=bool.Parse(ΑЁ(Αο(Ν,"Enable_Broadcast",ref ƿ)));bool Εβ=false;bool ΖЋ=true;if(Ŷ){if(Κ!=Ι.і)
Ɨ=ΑЁ(Αο(Ν,"Ship_Name",ref ƿ)).Replace(BROADCAST_SELF_ID,"");ΔΖ=ΑЁ(Αο(Ν,"Broadcast_Channel",ref ƿ)).ToLower();ΖЋ=false;if(Ǩ==null){Ǩ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǩ.SetMessageCallback("");}List<IMyRadioAntenna>ű=new List<IMyRadioAntenna>();χ.GetBlocksOfType(ű);bool ΓЊ=false;
for(int i=0;i<ű.Count;i++)
if(ű[i].EnableBroadcasting&&ű[i].Enabled){ΓЊ=true;break;}if(ű.Count==0) Λ="No Antenna found";else if(!ΓЊ) Λ="Antenna not ready";Εβ=ű.Count==0||!ΓЊ;if(Ζι&&!Εβ&&Κ!=Ι.і) Λ="Antenna ok";}else if(Κ==Ι.і)
Λ="Offline - Enable in PB custom data";Ζι=Εβ;if(ΖЋ){if(Ǩ!=null) this.IGC.DisableBroadcastListener(Ǩ);Ǩ=null;}}catch{ƿ=false;}if(!ƿ){Λ="Reset custom data";ΖΑ(ΑΑ);}}void ΖΑ(String ΑΑ){String Ξ=ΑΑ+"\n\n"+"Enable_Broadcast="+(Κ==Ι.і?"true":"false")+" \n";Ξ+=Κ!=Ι.і?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ΑЁ(String Ő){int Β=Ő.IndexOf("//");if(Β!=-1) Ő=Ő.Substring(0,Β);String[]τ=Ő.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αο(String[]Ν,String ȧ,ref bool ȩ){foreach(String Ő in Ν)
if(Ő.StartsWith(ȧ)) return Ő;ȩ=false;return "";}void Θδ(string Т){if(ć) Ъ=new List<Є>();String ȉ="";String ȃ="";if(!Γα(Т)) ΙΑ(Т);bool ΗЀ=false;int ΔІ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔІ-->0){MyIGCMessage ζ=Ǩ.AcceptMessage();String Ξ=ζ.Data as String;if(Ξ==null) continue;
if(ΖΡ(ref Ξ,out ȉ,out ȃ)&&ȉ!=""&&ȉ!=BROADCAST_SELF_ID){Є Δ=ΓΟ(ȉ,"");if(Δ==null){Δ=new Є(ȉ);Ъ.Add(Δ);}Δ.Ļ(ȃ,false,false,true);Ιζ(Δ,Ξ);ΗЀ=true;}}if(ΗЀ)
Ъ.Sort(delegate(Є a,Є b){if(a.ί==null&&b.ί==null) return 0;else if(a.ί==null) return-1;else if(b.ί==null) return 1;else return a.ί.CompareTo(b.ί);});if(č||ć){if(ΑΠ<=0||ć){Λ="";ć=false;ΙΛ();ΕБ();Δχ=DateTime.Now;}}if(Ɓ) Ιή();if(Ɓ) Ιβ();Ιά();}void Ιζ(Є Δ,String Ξ){Δ.Δτ=DateTime.Now;
String ǈ="";г(ref Ξ,out Δ.ΒЇ,BROADCAST_SEP);г(ref Ξ,out Δ.ί,BROADCAST_SEP);if(Δ.ΒЇ!=VERSION) return;String ΑΛ="";String[]ƍ=new string[3];г(ref Ξ,out ΑΛ,BROADCAST_SEP);г(ref Ξ,out ƍ[0],BROADCAST_SEP);г(ref Ξ,out ƍ[1],BROADCAST_SEP);г(ref Ξ,out ƍ[2],BROADCAST_SEP);г(ref Ξ,out Δ.ΕΞ,BROADCAST_SEP);
г(ref Ξ,out Δ.Λ,BROADCAST_SEP);г(ref Ξ,out ǈ,BROADCAST_SEP);г(ref Ξ,out Δ.Βμ,BROADCAST_SEP);String ΘΙ="",Ηκ="";г(ref Ξ,out ΘΙ,BROADCAST_SEP);г(ref Ξ,out Ηκ,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String ΗΨ;if(!г(ref Ξ,out ΗΨ,BROADCAST_SEP)) break;String[]Βΰ=ΗΨ.Split('/');if(Βΰ.Count()<3) continue;
int Љ=0;if(!int.TryParse(Βΰ[2],out Љ)) continue;Δ.Ћ.Add(new к(Βΰ[0],Βΰ[1],Љ,Ш.Ũ));}int Ηο=0;if(!int.TryParse(ǈ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΛ,out Ηο)) Δ.Κ=Ι.Ġ;if(!float.TryParse(ƍ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ƍ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(ƍ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(ΘΙ,out Δ.ȁ)) Δ.ȁ=0;if(!float.TryParse(Ηκ,out Δ.ȇ)) Δ.ȇ=0;Δ.Κ=(Ι) Ηο;Δ.ΔΧ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΕΫ=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.ΕΫ+=Δ.Ћ[i].Љ;}Є ΓΟ(String ΰ,String ί){ΰ=ΰ.ToUpper();ί=ί.ToUpper();for(int i=0;i<Ъ.Count;i++)
{if(ΰ!=""&&Ъ[i].ΰ.ToUpper()==ΰ) return Ъ[i];if(ί!=""&&Ъ[i].ί!=null&&Ъ[i].ί.ToUpper()==ί) return Ъ[i];}return null;}String Ÿ="";Є Ζω=null;DateTime Ζϊ=DateTime.MinValue;List<Є>Ń=new List<Є>();const float ΘΦ=20f;bool ΖΫ(Є η,String Ѕ){return Ѕ==Ÿ&&η==Ζω&&(DateTime.Now-Ζϊ).TotalSeconds<ΘΦ;
}String Θξ(Є η){if(Ÿ==""||!ΖΫ(η,Ÿ)) return "";String s="";int ȩ=0,ΕΙ=0;for(int i=0;i<Ń.Count;i++){Ј Βϊ=Ń[i].Ļ(Ÿ,false,false,false);if(Βϊ==Ј.Ư) ȩ++;if(ΕΙ++<8) s+="  "+Ń[i].ί+": "+(Βϊ==Ј.Ư?"received":Βϊ==Ј.Ŭ?"pending...":Βϊ==Ј.Αώ?"no answer!":"-")+"\n";
}if(ΕΙ>8) s+="  +"+(ΕΙ-8)+" more\n";return "--------------------------------------------\n"+Ÿ+": "+ȩ+"/"+Ń.Count+" received\n"+s;}void Ǥ(Є Δ,String Ѕ){Ÿ=Ѕ;Ζω=Δ;Ζϊ=DateTime.Now;Ń.Clear();if(Δ==null) Ń.AddRange(Ъ);else Ń.Add(Δ);if(Δ==null){for(int i=0;i<Ъ.Count;i++)
Ъ[i].Ļ(Ѕ,true,false,false);Γψ(Ѕ,"*",Ѕ);}else{Δ.Ļ(Ѕ,true,false,false);Γψ(Ѕ,Δ.ΰ,Ѕ);}}void ΙΑ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ȝ=τ.ElementAtOrDefault(1);String Ȧ="Invalid argument: "+Т;if(ϋ==Σ.Ʋ){if(Л!=null)
{switch(Ѕ){case "UP":{if(Ў<2) break;if(Л.Ў==0){Ў=1;return;}Ǥ(Л,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){Ǥ(Л,"MRES");break;}Ǥ(Л,"DOWN");break;}case "APPLY":{if(Ў<2) break;Ǥ(Л,"APPLY");return;}}}}switch(Ѕ){case "UP":this.Γε(false);return;case "DOWN":this.Γδ(false);return;
case "APPLY":this.Ť(true);return;}switch(Ѕ){case "CLEAR":Θί();return;case "SENDALL":
if(String.IsNullOrEmpty(Ȝ)){Λ="Usage: SENDALL <cmd>";return;}Ǥ(null,Ȝ);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΙΞ(Т.Remove(0,"SEND".Length+1));return;}Λ=Ȧ;}void ΙΞ(String ƹ){if(ƹ=="") return;var τ=ƹ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ɨ=τ.ElementAtOrDefault(0).Trim();
Є Δ=ΓΟ("",Ɨ);if(Δ!=null) Ǥ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ɨ;}void Θί(){Ъ.Clear();Л=null;λ(Σ.Main);Ў=0;}void ΙΛ(){List<IMyTerminalBlock>ΑΩ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ц,ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΩ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΩ(ΑΩ);ΖΨ();ΙΜ();Ľ(Ц,setLCDFontAndSize,1.15f,false);Ľ(Ȍ,setLCDFontAndSize,1.15f,true);String ΚΒ=ΕЄ(false);String Ησ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ц in Ц){if(ц.CustomData==""){ц.CustomData=ΕЄ(true);continue;}
if(!ц.CustomData.Contains(ΚΒ))
if(!ц.CustomData.Contains(Ησ)) ц.CustomData=Ησ+ц.CustomData;}}void Ιά(){String ǆ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȍ.Count;i++)
Ȍ[i].WriteText(Εμ(С.ΖΚ,"0",false));for(int i=0;i<Ц.Count;i++){С Џ=С.Ġ;String ǲ="";ΘЎ(Ц[i],out Џ,out ǲ);Ц[i].WriteText(Εμ(Џ,ǲ,i==0));}Echo(ǆ);for(int i=0;i<ņ.Count;i++){IMyTextPanel ц=ņ[i];String Ξ=ц.CustomData.ToUpper();if(Ξ=="DEBUG") ц.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") ц.WriteText(ΕЃ());
}}String Ť(bool Θ){int ΙЉ=0;return Ť(Θ,0,ref ΙЉ,false,1);}String Ť(bool Θ,int Κέ,ref int Κλ,bool Ζύ,int ΑΝ){String Γ="";String ƕ="——————————————————\n";String Κω="--------------------------------------------\n";if(Л==null||ϋ==Σ.Main||ϋ==Σ.Γω)
Γ+="[PAM]-Controller | "+Ъ.Count+" Connected ships"+"\n"+Θρ()+Θφ();else Γ+=ΕЇ(Л)+"\n";Γ+=ƕ;int Β=0;int Η=Ў;Œ=0;if(Λ!=""){Μ(ref Γ,Η,Β++,Θ,Λ);}else if(ϋ==Σ.Main){if(Л==null&&Ъ.Count>=1) Л=Ъ[0];String Ѝ="";if(Μ(ref Ѝ,Η,Β++,Θ," Send command to all")) λ(Σ.Γω);
int Ɣ=0;for(int s=0;s<Ъ.Count;s++){if(Ζύ) Ѝ+="\n";Є Δ=Ъ[s];if(Η==Β||Η==Β+1) Л=Δ;if(Β==Η) Ɣ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Β++,Θ," "+ΕЇ(Δ))){Л=Δ;λ(Σ.Ʋ);Л.Ļ("",false,true,false);}if(Β==Η) Ɣ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Β++,Θ," "+(Ζύ?"":"  ")+Θψ(Δ)))
{Л=Δ;λ(Σ.ΖΪ);}}int Ɵ=ΑΝ-2;Ɵ+=Math.Max(0,(Κέ-1))*ΑΝ;Γ+=Γχ(Ɵ,Ѝ,Ɣ,ref Κλ);}else if(ϋ==Σ.ΑЅ||ϋ==Σ.Γω){Σ Ις=ϋ==Σ.ΑЅ?Σ.Ʋ:Σ.Main;Є η=null;if(ϋ==Σ.ΑЅ) η=Л;if(Μ(ref Γ,Η,Β++,Θ," Back")){λ(Ις);}if(Μ(ref Γ,Η,Β++,Θ," [Stop] "+ŧ(η,"STOP"))){Ǥ(η,"STOP");}if(Μ(ref Γ,Η,Β++,Θ," [Continue] "+ŧ(η,"CONT"))){Ǥ(η,"CONT");}
if(Μ(ref Γ,Η,Β++,Θ," [Move home] "+ŧ(η,"HOMEPOS"))){Ǥ(η,"HOMEPOS");}if(Μ(ref Γ,Η,Β++,Θ," [Move to job] "+ŧ(η,"JOBPOS"))){Ǥ(η,"JOBPOS");}if(Μ(ref Γ,Η,Β++,Θ," [Full simulation] "+ŧ(η,"FULL"))){Ǥ(η,"FULL");}if(η!=null&&η.Κ==Ι.έ)
if(Μ(ref Γ,Η,Β++,Θ," [Undock] "+ŧ(η,"UNDOCK"))){Ǥ(η,"UNDOCK");}Γ+=Θξ(η);}else if(ϋ==Σ.Ʋ){String ΓБ="";if(Μ(ref ΓБ,Η,Β++,Θ," Back")){λ(Σ.Main);}Γ+=ΓБ.Substring(0,ΓБ.Length-1).PadRight(36,' ');if(Μ(ref Γ,Η,Β++,Θ," Send cmd")){λ(Σ.ΑЅ);}if(!Л.ΖΖ()&&!Л.Γή()) Œ++;
if(Л.Γή()) Γ+="No answer received...";else Γ+=Ů(Л,Л.Βμ);}else if(ϋ==Σ.ΖΪ){if(Μ(ref Γ,Η,Β++,Θ," Back")){λ(Σ.Main);}Γ+=Κω;Γ+=Ů(Л,ΕІ(Л));}if(!ΔЅ) Ў=Math.Min(Œ-1,Ў);return Γ;}String ΕЇ(Є Δ){if(Δ.ΒЇ!=VERSION) return Δ.ί+": Different version!";return Δ.ί+": "+Ů(Δ,Δ.ΕΞ)+" "+Δ.ΔΧ+"m";
}String Θψ(Є Δ){String s=Γτ("",Й(Δ.ȁ,Δ.ȇ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;к ύ=Δ.Ћ[i];s+=Ζγ(ΓΑ(Θυ(ύ)),3)+" "+Εύ(ύ.Љ)+" ";}return s;}String Θυ(к ύ){if(ύ.υ.ToUpper()=="ORE"||ύ.υ.ToUpper()=="INGOT"){String Ζϋ=Θσ(ύ.ί.ToUpper());if(Ζϋ!="") return Ζϋ;
}return ύ.ί;}String Ů(Є Δ,String Γ){if(Δ.ΖΖ()) return "No signal...("+Ας((int)(DateTime.Now-Δ.Δτ).TotalSeconds)+")";return Γ;}String ΕЄ(bool ΚЎ){String Γ="";if(ΚЎ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΘЎ(IMyTerminalBlock ц,out С Џ,out String ǲ){bool ȩ=true;String ΑΛ=ΑЁ(Αο(ц.CustomData.Split('\n'),"mode",ref ȩ)).ToUpper();String[]τ=ΑΛ.Split(':');String ΑΚ=τ.Length>0?τ[0].Trim():"";ǲ=τ.Length>1?τ[1].Trim():"";
Џ=С.Ġ;if(ΑΚ=="MAIN") Џ=С.ΖΛ;else if(ΑΚ=="MAINX") Џ=С.ΖΜ;else if(ΑΚ=="MENU") Џ=С.Γγ;else if(ΑΚ=="INVENTORY") Џ=С.Γέ;else if(ΑΚ=="DEBUG") Џ=С.ΓΖ;if(Џ==С.Ġ) ǲ=ΑΛ;}String Εμ(С Џ,String ǲ,bool ΙЊ){int ΑΘ=15;if(ΙЊ){ΖϏ=ΒΔ;ΒΔ=0;ΖЀ=ΒΕ;ΒΕ=0;}String Γ="";if(Џ==С.ΖΛ)
{int х=0;Γ=Ť(false,ΖϏ,ref Ιό,true,ΑΘ);if(!int.TryParse(ǲ,out х)) return Γ;ΒΔ=Math.Max(х,ΒΔ);х--;return ΖΠ(х,ΑΘ,Γ);}else if(Џ==С.ΖΜ){int х=0;Γ=Ť(false,ΖЀ,ref Ιύ,false,ΑΘ);if(!int.TryParse(ǲ,out х)) return Γ;ΒΕ=Math.Max(х,ΒΕ);х--;return ΖΠ(х,ΑΘ,Γ);}if(Џ==С.ΖΚ){return Ť(false,0,ref Ιώ,true,ΑΘ);
}else if(Џ==С.Γέ||Џ==С.Γγ||Џ==С.ΓΖ){Є Δ=Л;if(ǲ!=""){Δ=ΓΟ("",ǲ);if(Δ==null) return "Unknown ship: "+ǲ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƕ="—————————————————————\n";String ΕΩ="";String ΔΕ="";if(Џ==С.Γγ){ΕΩ="Menu";ΔΕ=Ů(Δ,Δ.Βμ);}
else if(Џ==С.Γέ){ΕΩ="Inventory";ΔΕ=Ů(Δ,ΕІ(Δ));}else if(Џ==С.ΓΖ){return Ů(Δ,Θω(Δ));}Γ+=Δ.ί+" - "+Δ.ΔΧ+"m | "+ΓΑ(""+ΕΩ)+"\n"+ƕ;Γ+=ΔΕ;return Γ;}return "Unknown command: "+ǲ;}string Θω(Є Δ){String s="";s+=Δ.ΒЇ+"\n";s+=Δ.ί+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕΞ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βμ.Length+"\n";s+=Δ.ȁ+"\n";s+=Δ.ȇ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΕІ(Є Δ){String s="";s+=Γτ("All",Й(Δ.ȁ,Δ.ȇ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){к ύ=Δ.Ћ[i];
s+=Γτ(ύ.ί,ύ.Љ,Δ.ΕΫ,30,8,10)+"\n";}return s;}Ј Θο(Є Δ,bool ф,string Ѕ){Ј ή=Ј.None;if(Δ==null){for(int i=0;i<Ъ.Count;i++){Ј Ѓ=Ъ[i].Ļ(Ѕ,false,ф,false);if(Ѓ==Ј.Ư) ή=Ѓ;if(Ѓ==Ј.Αώ) return Ѓ;if(Ѓ==Ј.Ŭ) return Ѓ;}return ή;}else return Δ.Ļ(Ѕ,false,ф,false);}String ŧ(Є Δ,String Ѕ)
{if(!ΖΫ(Δ,Ѕ)) return "";Ј Ѓ=Θο(Δ,false,Ѕ);if(Ѓ==Ј.Ư) return "received!";if(Ѓ==Ј.Ŭ) return "pending...";if(Ѓ==Ј.Αώ) return "no answer!";return "";}String Γχ(int Ɵ,String Γ,int Ɣ,ref int ē){String[]Ν=Γ.Split('\n');if(Ɣ>=ē+Ɵ-1) ē++;ē=Math.Min(Ν.Count()-1-Ɵ,ē);if(Ɣ<ē+1) ē--;
ē=Math.Max(0,ē);String ή="";for(int i=0;i<Ɵ;i++){int Ηα=i+ē;if(Ηα>=Ν.Count()) break;ή+=Ν[Ηα]+"\n";}return ή;}String ΖΠ(int х,int ΑΝ,String Γ){String[]Ν=Γ.Split('\n');int ΑΦ=х*ΑΝ;int Ć=(х+1)*ΑΝ;String ή="";for(int i=ΑΦ;i<Ć;i++){if(i>=Ν.Count()) break;ή+=Ν[i]+"\n";
}return ή;}String ΓΑ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζγ(String s,int Ηθ){if(s=="") return s;if(s.Length>Ηθ) s=s.Substring(0,Ηθ-1)+".";return s;}string Γτ(String ŏ,float value,float ƅ,int Ζπ,int Ηά,int Κά){float ŗ=Й(value,ƅ)*Ζπ;
String ΒΑ="[";for(int i=0;i<Ζπ;i++){if(i<=ŗ) ΒΑ+="|";else ΒΑ+="'";}ΒΑ+="]";return ΒΑ+" "+Ζγ(ΓΑ(ŏ),Ηά).PadRight(Ηά)+"".PadRight(Κά)+Εύ(value);}String Εύ(float Љ){if(Љ>=1000000) return Math.Round(Љ/1000000f,Љ/1000000f<100?1:0)+"M";if(Љ>=1000) return Math.Round(Љ/1000f,Љ/1000f<100?1:0)+"K";
return ""+Math.Round(Љ);}String Ας(int ΑΡ){if(ΑΡ>=60*60) return Math.Round(ΑΡ/(60f*60f),1)+" h";if(ΑΡ>=60) return Math.Round(ΑΡ/60f,1)+" min";return ""+ΑΡ+" s";}string Ŧ(float v){return Math.Round(v,2)+" ";}string ΛΔ(Vector3 v){return "X"+Ŧ(v.X)+"Y"+Ŧ(v.Y)+"Z"+Ŧ(v.Z);}
List<IMyCameraBlock>Ȟ=new List<IMyCameraBlock>();List<long>Ɖ=new List<long>();bool Ƌ=false;int ΔΜ=0;int ΔΙ=0;DateTime Ζς=DateTime.Now;String ΔΔ="";void ΙΚ(){Ȟ.Clear();Ɖ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȟ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȟ.Count;i++){Ȟ[i].EnableRaycast=true;if(!Ɖ.Contains(Ȟ[i].CubeGrid.EntityId)) Ɖ.Add(Ȟ[i].CubeGrid.EntityId);}if(!Ɖ.Contains(Me.CubeGrid.EntityId)) Ɖ.Add(Me.CubeGrid.EntityId);}bool ΖΔ(Α s){return s==Α.ȱ||s==Α.с||s==Α.И||
s==Α.Ğ||s==Α.ť||s==Α.Ʀ||s==Α.Απ||
(s==Α.Ǹ&&Ǚ!=null);}void Αλ(){Ƌ=false;ΔΜ=0;ΔΔ="";}void Ιδ(){if(!obstacleBrake||Ȟ.Count==0||!ΖΔ(Ω)){Αλ();return;}Vector3 ǉ=Ѐ+ı;Vector3 ȍ=ǉ-δ;float ȋ=ȍ.Length();if(ȋ<1f){Αλ();return;}Vector3 ρ=ȍ/ȋ;float Ηε=Ĕ*obstacleLookAhead+ο;Vector3 Κγ=ǉ+ρ*(ο*0.25f);
for(int n=0;n<Ȟ.Count;n++){ΔΙ=(ΔΙ+1)%Ȟ.Count;IMyCameraBlock ǽ=Ȟ[ΔΙ];if(!ǽ.IsFunctional||!ǽ.IsWorking) continue;Vector3 Ζτ=ǽ.GetPosition();Vector3 Ć=Κγ;if(Vector3.Distance(Ζτ,Ć)>Ηε) Ć=Ζτ+ρ*Ηε;if(!ǽ.CanScan(Ć)) continue;MyDetectedEntityInfo Ȥ=ǽ.Raycast(Ć);bool ΚΩ=!Ȥ.IsEmpty()&&!Ɖ.Contains(Ȥ.EntityId)&&!ΘЇ(Ȥ.EntityId)&&
Ȥ.Type!=MyDetectedEntityType.FloatingObject;if(ΚΩ){if(!Ƌ) Ζς=DateTime.Now;Ƌ=true;ΔΜ=0;ΔΔ=Ȥ.Type.ToString();}else if(Ƌ&&++ΔΜ>=3) Αλ();break;}if(Ƌ&&(DateTime.Now-Ζς).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΔ;}const String ΓЄ="[PAMTFC]";const float ΙΦ=5f;const float ΓΞ=2f;class ğ{public long ΰ;public Vector3 ν,ƞ;public Vector3 ΑΒ;public float ΖЊ;public String Χ="";public int Ċ=-1;public bool ΕΑ;public bool Δξ;public float ĉ,Ī;public long α=0;public int ǂ=0;public DateTime ΕΦ;
}IMyBroadcastListener ΒЁ=null;long Ǎ=0;List<ğ>ђ=new List<ğ>();Vector3 ı=new Vector3();float ȑ=-1;Vector3 Εά=new Vector3();bool ΒЀ=false;DateTime Εέ=DateTime.Now;const float Ζβ=15f;const float ΙΥ=10f;int ΘΖ=0;String з="";int Д=-1;bool ΔΠ=false;Vector3 ΖЁ=new Vector3();String ƫ()
{if(β==Π.ь||Κ==Ι.έ) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Ь+"|"+(int) Ŏ+"|"+Math.Round(Ȑ,1)+"|"+Math.Round(ȏ,1);}void Ιθ(){if(!trafficEnabled) return;if(ΒЁ==null) ΒЁ=IGC.RegisterBroadcastListener(ΓЄ);Ǎ=Me.CubeGrid.EntityId;int Ą=30;while(ΒЁ.HasPendingMessage&&Ą-->0){String Ξ=ΒЁ.AcceptMessage().Data as String;if(Ξ!=null) ΘА(Ξ);
}for(int i=ђ.Count-1;i>=0;i--)
if((DateTime.Now-ђ[i].ΕΦ).TotalSeconds>ΙΦ) ђ.RemoveAt(i);if(ΒΗ==null) ΒΗ=IGC.RegisterBroadcastListener(ΖΗ);Ą=10;while(ΒΗ.HasPendingMessage&&Ą-->0){String Ξ=ΒΗ.AcceptMessage().Data as String;if(Ξ!=null) try{ΘЍ(Ξ);}catch{}}if(++ΘΖ<3) return;ΘΖ=0;IGC.SendBroadcastMessage(ΓЄ,Ǎ+";"+σ(δ)+";"+
σ(Ϊ.GetShipVelocities().LinearVelocity)+";"+ο+";"+ƫ()+";"+
(β==Π.В?Д:-1)+";"+з+";"+Θή()+","+Εϊ()+";"+σ(Εά)+
";"+(ΒΟ?1:0)+";"+(ŋ&&β==Π.В?1:0)+","+Ɩ+","+ΑΣ+","+(ΕЎ()?1:0));}void ΘА(String Ξ){try{String[]f=Ξ.Split(';');if(f.Length<7) return;long ΰ=long.Parse(f[0]);if(ΰ==Ǎ) return;ğ Δ=null;for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) Δ=ђ[i];if(Δ==null){Δ=new ğ();Δ.ΰ=ΰ;ђ.Add(Δ);}
Δ.ν=ς(f[1]);Δ.ƞ=ς(f[2]);Δ.ΖЊ=float.Parse(f[3]);Δ.Χ=f[4];Δ.Ċ=int.Parse(f[5]);Δ.ΕΦ=DateTime.Now;if(Δ.Χ!=""&&Δ.Χ==ƫ()) ΖΝ(f[6]);Δ.α=0;Δ.ǂ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ǂ=int.Parse(d[1]);}Δ.ΑΒ=f.Length>=9?ς(f[8]):new Vector3();
Δ.ΕΑ=f.Length>=10&&f[9]=="1";Δ.Δξ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ĉ=float.Parse(w[1]);Δ.Ī=float.Parse(w[2]);Δ.Δξ=w[3]=="1";}}}catch{}}bool ΘЇ(long ΰ){for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) return true;return false;}Vector3 ΕЅ(int Β,int Κή)
{if(laneOffset<=0||Φ.Count<2||Β<0||Β>=Φ.Count) return new Vector3();Ϋ ά=Φ[Β];Vector3 ΚІ=(Φ[Math.Min(Φ.Count-1,Β+1)].Ε-Φ[Math.Max(0,Β-1)].Ε)*Κή;Vector3 Б=ά.φ.LengthSquared()>0.01f?-Vector3.Normalize(ά.φ):-ά.θ;Vector3 о=Vector3.Cross(ΚІ,Б);if(о.LengthSquared()<0.0001f) return new Vector3();
float Њ=laneOffset*ο;float ΙЌ=Math.Min(Vector3.Distance(ά.Ε,Φ[0].Ε),
Vector3.Distance(ά.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(о)*Њ*Math.Min(1,ΙЌ/(Њ*4));}Vector3 ΘΕ=new Vector3();bool ΕЎ(){return β==Π.В&&(Ω==Α.ȱ||Ω==Α.Ū||Ω==Α.Ș);}bool ΘІ(Α s){return s==Α.Ǹ||s==Α.Ĵ||s==Α.ĸ||s==Α.ư||
s==Α.Ǧ||s==Α.Ș||s==Α.Ě;}void Ιι(){ı=new Vector3();ȑ=-1;Εά=new Vector3();bool Ǌ=ΖΔ(Ω);if(!trafficEnabled||(!Ǌ&&!ΘІ(Ω))){ΒЀ=false;return;}ΘΕ=Ϊ.GetShipVelocities().LinearVelocity;Θΰ(Ϊ.GetNaturalGravity(),!Ǌ);}void Θΰ(Vector3 φ,bool Ηω=false){ı=new Vector3();ȑ=-1;Vector3 ȍ=Ѐ-δ;
float ȋ=ȍ.Length();bool Δκ=ȋ>=1f;Vector3 ρ=Δκ?ȍ/ȋ:new Vector3();Εά=ρ;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-Ǆ;Vector3 о=Vector3.Cross(ρ,Б);о=о.LengthSquared()<0.0001f?-ΑΗ:Vector3.Normalize(о);ğ Ββ=null;bool ΗΙ=Ω==Α.И&&!Ă(1);for(int i=0;i<ђ.Count;i++)
{ğ γ=ђ[i];if(γ.ǂ==3&&γ.ƞ.LengthSquared()<0.25f) continue;Vector3 ν=γ.ν+γ.ƞ*(float)(DateTime.Now-γ.ΕΦ).TotalSeconds;Vector3 Ə=ν-δ;float ф=(ο+γ.ΖЊ)*0.5f;bool ΗЁ=γ.Δξ&&ΕЎ()&&γ.ĉ>0&&Ɩ>0&&γ.Χ==ƫ();if(ΗЁ) ф=Math.Max(Ɩ+γ.ĉ,ΑΣ+γ.Ī)*0.5f;float Ƹ=Δκ?Vector3.Dot(Ə,ρ):0;
if(Ƹ>0&&(Ə-ρ*Ƹ).Length()<=ф*1.2f){float Βπ=Vector3.Dot(γ.ƞ,ρ);bool ΗΝ=γ.ΑΒ.LengthSquared()>0.25f;bool Δλ=Βπ<-1f||(ΗΝ&&Vector3.Dot(γ.ΑΒ,ρ)<-0.5f);if(Ηω){if(Δλ&&Ǎ<γ.ΰ) continue;}else{if(γ.ΕΑ){if(!ΗΙ){ı=о*ф*1.3f;continue;}float Ηξ=Vector3.Distance(δ,Ѐ),ΗΩ=Vector3.Distance(ν,Ѐ);
if(Ηξ<ΗΩ||(Ηξ==ΗΩ&&Ǎ<γ.ΰ)) continue;}if(Δλ){if(Ƹ<(Ĕ-Βπ)*6f+ф*4f) ı=о*ф*1.3f;continue;}if(ΗΝ&&Ǎ<γ.ΰ){Vector3 Ζξ=-Ə;float Ηφ=Vector3.Dot(Ζξ,γ.ΑΒ);if(Ηφ>0&&(Ζξ-γ.ΑΒ*Ηφ).Length()<=ф*1.2f){ı=о*ф*1.3f;continue;}}}float ΗΘ=Ƹ-ф-(ΗЁ?1f:followGap*ο);
float Δω=Math.Max(0,Βπ)+Math.Max(0,ΗΘ)/ΓΞ;if(Δλ) Δω=Math.Max(0,ΗΘ+Math.Min(0,Βπ)*ΓΞ*2)/ΓΞ;if(ȑ<0||Δω<ȑ){ȑ=Δω;Ββ=γ;}continue;}if(γ.ƞ.LengthSquared()<1f) continue;if(Δκ&&(Ə-ρ*Ƹ).Length()<=ф*1.2f) continue;Vector3 ΕΖ=γ.ƞ-ΘΕ;float Ηύ=ΕΖ.LengthSquared();
if(Ηύ<0.25f) continue;float t=-Vector3.Dot(Ə,ΕΖ)/Ηύ;if(t<=0||t>5f||(Ə+ΕΖ*t).Length()>=ф*1.1f) continue;if(Ǎ>γ.ΰ||t<2f){ȑ=0;Ββ=γ;}}bool ΚΑ=ȑ>=0&&ȑ<0.5f;if(!ΚΑ||Ηω||(Ββ!=null&&Ββ.ΕΑ&&ΗΙ)){ΒЀ=false;return;}if(!ΒЀ){ΒЀ=true;Εέ=DateTime.Now;}
double ΗΞ=(DateTime.Now-Εέ).TotalSeconds;if(ΗΞ>Ζβ+ΙΥ) Εέ=DateTime.Now;else if(ΗΞ>Ζβ){ȑ=-1;ı=о*ο*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȥ=new List<int>();String ΗΠ="";void ΓΝ(){String ȧ=Ќ+"|"+Ь+"|"+(int) Ŏ;if(ȧ==ΗΠ) return;ΗΠ=ȧ;Εν(Ќ,Ь,Ŏ,ȥ);
}void Εν(int w,int h,э ΑΦ,List<int>ΔΚ){ΔΚ.Clear();if(ΑΦ==э.ƴ){for(int k=0;k<w*h;k++){int Ɛ=k/w;ΔΚ.Add(Ɛ*w+(Ɛ%2==0?k-Ɛ*w:w-1-(k-Ɛ*w)));}return;}int ǌ=(int) Math.Ceiling(w/2f),ǋ=(int) Math.Ceiling(h/2f);int Δη=w/2,Δζ=h/2;int x=0,y=0,ΔΪ=0,ΔΫ=-1,ƅ=Math.Max(w,h);
for(int i=0;i<ƅ*ƅ;i++){if(-ǌ<x&&x<=Δη&&-ǋ<y&&y<=Δζ) ΔΚ.Add((y-1+ǋ)*w+(x-1+ǌ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔΪ;ΔΪ=-ΔΫ;ΔΫ=t;}x+=ΔΪ;y+=ΔΫ;}}bool ΕЍ(int a,int b,int w,float ΒЂ,float Βύ,float ĉ,float Ī){return a==b||(Math.Abs(a%w-b%w)*ΒЂ<ĉ+0.5f&&Math.Abs(a/w-b/w)*Βύ<Ī+0.5f);
}bool ΓΜ(){int n=Math.Max(0,Ќ*Ь);if(з.Length==n) return false;з=new String('0',n);Д=-1;return true;}void ΖΝ(String γ){ΓΜ();if(γ.Length!=з.Length) return;char[]h=з.ToCharArray();for(int i=0;i<h.Length;i++) if(γ[i]=='1') h[i]='1';з=new String(h);}bool Αχ(int У){return У>=0&&У<з.Length&&з[У]=='1';}
void Γϋ(int У,bool ώ){if(У<0||У>=з.Length) return;char[]h=з.ToCharArray();h[У]=ώ?'1':'0';з=new String(h);}bool ΓΪ(int У,bool ΚΙ){if(!trafficEnabled) return false;String ȧ=ƫ();for(int i=0;i<ђ.Count;i++){ğ t=ђ[i];if(t.Χ!=ȧ||t.Ċ<0||(ΚΙ&&t.ΰ>Ǎ)) continue;if(ΕЍ(У,t.Ċ,Ќ,Ȑ,ȏ,Ɩ,ΑΣ)) return true;
}return false;}int Θέ(int Β){int n=Ќ*Ь;if(n<1) return Β;ΓΝ();if(ΓΜ()) for(int k=0;k<Β&&k<n;k++) Γϋ(ȥ[k],true);if(Д>=0&&(Β>=n||ȥ[Β]!=Д)){Γϋ(Д,true);Д=-1;}if(ΖΘ()){if(Ȁ>=0&&!Αχ(Ȁ)){ŋ=false;Д=Ȁ;return ȥ.IndexOf(Ȁ);}if(Ȁ==-2){ŋ=false;Д=-1;return n;}
if(Ȁ==-1&&Д>=0&&!Αχ(Д)){ŋ=false;return Β;}Д=-1;if(!ŋ){ŋ=true;ΖЃ=DateTime.Now;}if((DateTime.Now-ΖЃ).TotalSeconds<5) return-1;}ŋ=false;if(Д>=0&&!Αχ(Д)&&!ΓΪ(Д,true)) return Β;Д=-1;for(int k=0;k<n;k++){int c=ȥ[k];if(Αχ(c)||ΓΪ(c,false)) continue;Д=c;
return k;}return n;}const String ΖΗ="[PAMJOB]";IMyBroadcastListener ΒΗ=null;DateTime ΖЂ=DateTime.MinValue;DateTime ΖЃ=DateTime.Now;int Ȁ=-1;bool ŋ=false;bool ΖΘ(){return trafficEnabled&&Ȁ!=-3&&(DateTime.Now-ΖЂ).TotalSeconds<3;}void ΘЍ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƫ()) return;
ΖЂ=DateTime.Now;ΖΝ(f[2]);Ȁ=-3;String Ηλ=Ǎ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ηλ)) Ȁ=int.Parse(e[i].Substring(Ηλ.Length));}ǥ ΙΟ(int Β){ΓΝ();if(Β<0||Β>=ȥ.Count) return ǥ.Αζ;Ϗ=ȥ[Β]%Ќ;ό=ȥ[Β]/Ќ;return ǥ.Ασ;}class Ǔ{public long ΰ;
public Vector3 ν,Ы,Б;}List<Ǔ>Ф=new List<Ǔ>();bool ĥ=false;Vector3 ΒΡ,ΒΠ,ΒΣ,ΒΤ;long ћ=0;long Ȫ=0;Ϋ Ǚ=null;bool ǃ=false;Ϋ ΓΙ(Vector3 ν,Vector3 Ы,Vector3 Б){MatrixD m=MatrixD.CreateWorld(ν,Ы,Б);Ϋ w=new Ϋ(Ο);w.ι=Vector3D.TransformNormal(ΒΡ,m);w.θ=Vector3D.TransformNormal(ΒΠ,m);
w.Ђ=Vector3D.TransformNormal(ΒΣ,m);w.Ε=ν+(Vector3) Vector3D.TransformNormal(ΒΤ,m);w.κ=true;return w;}int Εϊ(){Α s=Ω;if(s==Α.ы) return д(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ĸ||s==Α.ț||s==Α.ķ||s==Α.ΑЌ||
s==Α.Αξ||s==Α.Ǧ) return 3;if(s==Α.Ǹ||s==Α.Ĵ||s==Α.ư) return 2;if(s==Α.И) return 1;return 0;}long Θή(){if(Εϊ()==0) return 0;if(Ă(1)&&Ƙ!=0) return Ƙ;return Ǚ!=null?Ȫ:ћ;}void ΖΙ(){if(Κ==Ι.έ||!Ο.κ) return;IMyShipConnector О=д(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector γ=О.OtherConnector;Vector3I ΚΓ=new Vector3I((int) Ο.м.X,(int) Ο.м.Y,(int) Ο.м.Z);if(О.Position==ΚΓ){MatrixD ž=MatrixD.Transpose(γ.WorldMatrix);ΒΡ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Forward,ž);ΒΠ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Down,ž);ΒΣ=Vector3D.TransformNormal(Ϊ.WorldMatrix.Left,ž);
ΒΤ=Vector3D.TransformNormal(О.GetPosition()-γ.GetPosition(),ž);ĥ=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ћ=γ.EntityId;List<IMyShipConnector>ŉ=new List<IMyShipConnector>();χ.GetBlocksOfType(ŉ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ŉ.Count>0){Ф.Clear();for(int i=0;i<ŉ.Count;i++){Ǔ p=new Ǔ();p.ΰ=ŉ[i].EntityId;p.ν=ŉ[i].GetPosition();p.Ы=ŉ[i].WorldMatrix.Forward;p.Б=ŉ[i].WorldMatrix.Up;Ф.Add(p);}}Ǚ=null;Ȫ=0;if(γ.EntityId==ћ||!ĥ) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].ΰ==γ.EntityId){Ȫ=Ф[i].ΰ;Ǚ=ΓΙ(Ф[i].ν,Ф[i].Ы,Ф[i].Б);}}bool Γΰ(long ΰ,int Ηπ){if(ΰ==0) return false;for(int i=0;i<ђ.Count;i++){ğ t=ђ[i];if(t.α==ΰ&&(t.ǂ>Ηπ||(t.ǂ==Ηπ&&t.ΰ<Ǎ))) return true;}return false;}bool ΓΔ(int ΔЏ){if(Κ==Ι.έ||!trafficEnabled){Ǚ=null;Ȫ=0;return true;}
long ξ=Ǚ!=null?Ȫ:ћ;if(!Γΰ(ξ,ΔЏ)) return true;if(!Γΰ(ћ,ΔЏ)){Ǚ=null;Ȫ=0;return true;}if(!ĥ) return false;Ǔ ǧ=null;float ƽ=0;for(int i=0;i<Ф.Count;i++){Ǔ p=Ф[i];if(p.ΰ==ћ||Γΰ(p.ΰ,ΔЏ)) continue;float d=Vector3.Distance(p.ν,Ο.Ε);if(ǧ==null||d<ƽ){ǧ=p;ƽ=d;}
}if(ǧ==null) return false;Ȫ=ǧ.ΰ;Ǚ=ΓΙ(ǧ.ν,ǧ.Ы,ǧ.Б);return true;}String Θθ(){String s=(ĥ?"1":"0")+":"+σ(ΒΡ)+":"+σ(ΒΠ)+":"+
σ(ΒΣ)+":"+σ(ΒΤ)+":"+ћ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].ΰ+","+σ(Ф[i].ν)+","+σ(Ф[i].Ы)+","+σ(Ф[i].Б);return s;}void Θη(String s){Ф.Clear();ĥ=false;ћ=0;String[]f=s.Split(':');if(f.Length<6) return;ĥ=f[0]=="1";ΒΡ=ς(f[1]);ΒΠ=ς(f[2]);ΒΣ=ς(f[3]);ΒΤ=ς(f[4]);long.TryParse(f[5],out ћ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǔ p=new Ǔ();if(!long.TryParse(e[0],out p.ΰ)) continue;p.ν=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ф.Add(p);}}const String ƥ="[PAMDCK]";const float ΘΥ=120f;class Ȱ{public long Δ;public int Ǘ;public long њ;public bool Ζυ;public DateTime ȣ,Ηέ;
}IMyBroadcastListener ΒΞ=null;List<Ȱ>Х=new List<Ȱ>();List<IMyShipConnector>ĕ=new List<IMyShipConnector>();List<IMySensorBlock>Ȯ=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΖ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΒЈ=new List<MyDetectedEntityInfo>();long Ǿ=0;int ΔΝ=0;
long Ζχ=0;DateTime Ζψ=DateTime.Now;String Ň="";int ΖЏ=0;IMyCubeGrid Ė=null;void ΙΜ(){Ė=Me.CubeGrid;χ.GetBlocksOfType(ĕ,q=>q.CubeGrid.IsSameConstructAs(Ė)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȯ,q=>q.CubeGrid.IsSameConstructAs(Ė)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΖ,q=>q.CubeGrid.IsSameConstructAs(Ė)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȯ.Count;i++){Ȯ[i].DetectSmallShips=true;Ȯ[i].DetectLargeShips=true;}for(int i=0;i<ΑΖ.Count;i++) ΑΖ[i].EnableRaycast=true;}IMyShipConnector Ζέ(long ΰ){if(ΰ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(ΰ) as IMyShipConnector;return c!=null&&Ė!=null&&c.CubeGrid.IsSameConstructAs(Ė)?c:null;}bool Γί(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΕ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΖ.Count;i++)
{IMyCameraBlock ǽ=ΑΖ[i];if(!ǽ.IsWorking||Vector3D.Distance(ǽ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ć=ǽ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ǽ.CanScan(Ć)) continue;MyDetectedEntityInfo Ȥ=ǽ.Raycast(Ć);if(!Ȥ.IsEmpty()&&Ȥ.EntityId!=Ė.EntityId&&Ȥ.EntityId!=Δ&&Ȥ.Type!=MyDetectedEntityType.FloatingObject)
{Ň="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΘЈ(){for(int s=0;s<Ȯ.Count;s++){if(!Ȯ[s].IsWorking) continue;ΒЈ.Clear();Ȯ[s].DetectedEntities(ΒЈ);for(int i=0;i<ΒЈ.Count;i++){MyDetectedEntityInfo e=ΒЈ[i];bool ΙЎ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΙЎ&&e.EntityId!=Ǿ&&e.Velocity.Length()>0.5f){Ň="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΘБ(Ȱ r){IMyShipConnector њ=Ζέ(r.њ);if(Γί(њ)&&ΖΕ(њ,r.Δ)) return њ.EntityId;if(!r.Ζυ||њ==null) return 0;IMyShipConnector ǧ=null;double ƽ=0;for(int i=0;i<ĕ.Count;i++)
{IMyShipConnector c=ĕ[i];if(!Γί(c)) continue;double d=Vector3D.Distance(c.GetPosition(),њ.GetPosition());if(ǧ==null||d<ƽ){if(ΖΕ(c,r.Δ)){ǧ=c;ƽ=d;}}}return ǧ==null?0:ǧ.EntityId;}void Θϋ(){Ȱ ƌ=null;long О=0;for(int ŕ=2;ŕ>=1&&ƌ==null;ŕ--){Ȱ Ŕ=null;for(int i=0;i<Х.Count;i++)
{Ȱ r=Х[i];if(r.Ǘ!=ŕ) continue;if(ŕ==1){long c=ΘБ(r);if(c==0) continue;if(Ŕ==null||r.ȣ<Ŕ.ȣ){Ŕ=r;О=c;}}else if(Ŕ==null||r.ȣ<Ŕ.ȣ) Ŕ=r;}ƌ=Ŕ;}if(ƌ==null){if(Х.Count>0&&Ň=="") Ň="No free dock";return;}Ǿ=ƌ.Δ;ΔΝ=ƌ.Ǘ;Ζχ=ƌ.Ǘ==1?О:0;Ζψ=DateTime.Now;
}void Ιή(){if(Ė==null) return;if(ΒΞ==null) ΒΞ=IGC.RegisterBroadcastListener(ƥ);int Ą=30;while(ΒΞ.HasPendingMessage&&Ą-->0){String Ξ=ΒΞ.AcceptMessage().Data as String;if(Ξ!=null) Θό(Ξ);}ΙΕ();if(++ΖЏ<3) return;ΖЏ=0;IGC.SendBroadcastMessage(ƥ,ΘΤ());}void Θό(String Ξ){try{String[]f=Ξ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Х.Count-1;i>=0;i--) if(Х[i].Δ==Δ) Х.RemoveAt(i);if(Ǿ==Δ) Ǿ=0;}else if(f[0]=="R"&&f.Length>=5){long њ=long.Parse(f[3]);if(Ζέ(њ)==null) return;Ȱ r=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Δ) r=Х[i];if(r==null){r=new Ȱ();r.Δ=Δ;r.ȣ=DateTime.Now;Х.Add(r);}
int Ǘ=int.Parse(f[2]);if(r.Ǘ!=Ǘ) r.ȣ=DateTime.Now;r.Ǘ=Ǘ;r.њ=њ;r.Ζυ=f[4]=="1";r.Ηέ=DateTime.Now;}}catch{}}void ΙΕ(){for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].Ηέ).TotalSeconds>3) Х.RemoveAt(i);Ȱ ΒΓ=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Ǿ) ΒΓ=Х[i];if(Ǿ!=0&&ΒΓ==null) Ǿ=0;if(ΒΓ!=null&&(DateTime.Now-Ζψ).TotalSeconds>ΘΥ){Ǿ=0;ΒΓ.ȣ=DateTime.Now;}Ň="";if(Ǿ==0&&Х.Count>0&&!ΘЈ()) Θϋ();
}String ΘΤ(){String ζ="S;"+Ė.EntityId+";"+Ǿ+";"+ΔΝ+";"+Ζχ+";";for(int i=0;i<Х.Count;i++) ζ+=(i>0?",":"")+Х[i].Δ;ζ+=";";for(int i=0;i<ĕ.Count;i++){IMyShipConnector c=ĕ[i];ζ+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}ζ+=";";for(int i=0;i<Ȯ.Count;i++){IMySensorBlock z=Ȯ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ζ+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return ζ;}String Θρ()
{if(ĕ.Count==0&&Ȯ.Count==0) return "";int Ǌ=0;for(int i=0;i<ĕ.Count;i++) if(Γί(ĕ[i])) Ǌ++;return "Docks: "+Ǌ+"/"+ĕ.Count+" free"+(Ǿ!=0?(ΔΝ==1?" | docking":" | undocking"):"")+
(Х.Count>1?" | "+(Х.Count-1)+" waiting":"")+(Ň!=""?"\n"+Ň:"")+"\n";}IMyBroadcastListener Βϋ=null;int Ȣ=0;DateTime ΖЎ=DateTime.Now;DateTime ΗЅ=DateTime.MinValue;long ΗІ=0;int ΗЇ=0;long Ƙ=0;bool Αβ=false;int ΔΩ=0;List<Vector3>Αδ=new List<Vector3>();List<float>Ει=new List<float>();
bool ΒΟ=false;bool ΒΝ=false;Vector3 ΖЍ=new Vector3();bool Ƴ(){return Ȣ!=0&&(DateTime.Now-ΗЅ).TotalSeconds<3;}bool Ă(int Ǘ){return Ƴ()&&ΗІ==Ǎ&&ΗЇ==Ǘ;}bool Ζή(){return Ƴ()||(DateTime.Now-ΖЎ).TotalSeconds<3;}void Γφ(int Ǘ){if(Ȣ==Ǘ) return;Ȣ=Ǘ;ΖЎ=DateTime.Now;ΔΩ=3;
}void Γυ(){if(Ȣ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƥ,"D;"+Ǎ);Ȣ=0;Αβ=false;}void Ιέ(){if(!trafficEnabled||Κ==Ι.έ) return;if(Βϋ==null) Βϋ=IGC.RegisterBroadcastListener(ƥ);int Ą=20;while(Βϋ.HasPendingMessage&&Ą-->0){String Ξ=Βϋ.AcceptMessage().Data as String;if(Ξ==null||!Ξ.StartsWith("S;")) continue;
try{ΘЏ(Ξ);}catch{}}if(Ω!=Α.И){ΒΟ=false;ΒΝ=false;}if(Ȣ==1&&д(MyShipConnectorStatus.Connected)!=null) Γυ();if(Ȣ==2&&!Αβ&&Ω!=Α.Ǧ&&
Vector3.Distance(δ,ъ().Ε)>ο*3) Γυ();if(Ȣ!=0&&++ΔΩ>=3){ΔΩ=0;IGC.SendBroadcastMessage(ƥ,"R;"+Ǎ+";"+Ȣ+";"+ћ+";"+(ĥ?1:0));}}void ΘЏ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<7) return;bool ΗΗ=false;String[]Ζε=f[5].Split(',');for(int i=0;i<Ζε.Length;i++) if(Ζε[i]==Ǎ.ToString()) ΗΗ=true;
if(!ΗΗ) return;ΗЅ=DateTime.Now;ΗІ=long.Parse(f[2]);ΗЇ=int.Parse(f[3]);Ƙ=long.Parse(f[4]);Αδ.Clear();Ει.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αδ.Add(ς(e[0]));Ει.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΖБ=f[6].Split(':');List<Ǔ>Ѝ=new List<Ǔ>();for(int i=0;i<ΖБ.Length;i++){String[]e=ΖБ[i].Split(',');if(e.Length<4) continue;Ǔ p=new Ǔ();p.ΰ=long.Parse(e[0]);p.ν=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ѝ.Add(p);}Ф=Ѝ;}bool ΘΫ(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;
Γφ(1);if(!Ζή()) return true;if(!Ă(1)){Λ="Waiting for dock clearance";return false;}ΒЌ();return true;}bool ΒЌ(){ǃ=false;Ǔ Ŗ=null;if(Ƙ!=0&&Ƙ!=ћ&&ĥ)
for(int i=0;i<Ф.Count;i++) if(Ф[i].ΰ==Ƙ) Ŗ=Ф[i];long Ơ=Ŗ==null?0:Ŗ.ΰ;if(Ơ==Ȫ&&(Ơ==0)==(Ǚ==null)) return false;Ȫ=Ơ;Ǚ=Ŗ==null?null:ΓΙ(Ŗ.ν,Ŗ.Ы,Ŗ.Б);return true;}bool ũ(Vector3 p){for(int i=0;i<Αδ.Count;i++) if(Vector3.Distance(p,Αδ[i])<Ει[i]+ο*0.75f) return true;
return false;}bool ΘϏ(Vector3 ǀ,Vector3 œ){ΒΟ=false;if(!trafficEnabled||Κ==Ι.έ||ћ==0) return false;if(Ȣ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;Γφ(1);if(!Ƴ()||Ă(1)||Αδ.Count==0) return false;if(!ũ(ǀ)&&!ũ(œ)&&!ũ(δ)) return false;ΒΟ=true;return true;
}bool Ετ(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;Γφ(2);if(!Ζή()) return true;if(!Ă(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΘЉ=600f;class ȳ{public long ΰ;public int У=-1;public bool Ơ;public float ĉ,Ī;public DateTime ΕΗ;}class ȗ{public String ȧ;
public int w,h;public float ΒЂ,Βύ;public String ώ;public List<int>ΔЊ=new List<int>();public List<ȳ>п=new List<ȳ>();}List<ȗ>ȅ=new List<ȗ>();IMyBroadcastListener Βα=null;int ΗΪ=0;void Ιβ(){if(Βα==null) Βα=IGC.RegisterBroadcastListener(ΓЄ);int Ą=50;while(Βα.HasPendingMessage&&Ą-->0)
{String Ξ=Βα.AcceptMessage().Data as String;if(Ξ!=null) try{Θώ(Ξ);}catch{}}for(int j=ȅ.Count-1;j>=0;j--){ȗ Χ=ȅ[j];for(int i=Χ.п.Count-1;i>=0;i--)
if((DateTime.Now-Χ.п[i].ΕΗ).TotalSeconds>ΘЉ) Χ.п.RemoveAt(i);if(Χ.п.Count==0){ȅ.RemoveAt(j);continue;}ΘΟ(Χ);}if(++ΗΪ<3) return;ΗΪ=0;for(int j=0;j<ȅ.Count;j++) IGC.SendBroadcastMessage(ΖΗ,ΘΣ(ȅ[j]));}void ΘΟ(ȗ Χ){for(int i=0;i<Χ.п.Count;i++){ȳ s=Χ.п[i];if(s.Ơ&&(s.У<0||Χ.ώ[s.У]=='1')) s.У=ΘЋ(Χ,s);
}}String ΘΣ(ȗ Χ){String ζ="J;"+Χ.ȧ+";"+Χ.ώ+";";bool ȣ=true;for(int i=0;i<Χ.п.Count;i++){if((DateTime.Now-Χ.п[i].ΕΗ).TotalSeconds>5) continue;ζ+=(ȣ?"":",")+Χ.п[i].ΰ+":"+Χ.п[i].У;ȣ=false;}return ζ;}void Θώ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȗ Χ=null;for(int i=0;i<ȅ.Count;i++) if(ȅ[i].ȧ==f[4]) Χ=ȅ[i];if(Χ==null){Χ=new ȗ();Χ.ȧ=f[4];Χ.w=int.Parse(k[3]);Χ.h=int.Parse(k[4]);Χ.ΒЂ=float.Parse(k[6]);Χ.Βύ=float.Parse(k[7]);if(Χ.w<1||Χ.h<1) return;Εν(Χ.w,Χ.h,(э) int.Parse(k[5]),Χ.ΔЊ);
Χ.ώ=new String('0',Χ.w*Χ.h);ȅ.Add(Χ);}String d=f[6];if(d.Length==Χ.ώ.Length){char[]c=Χ.ώ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Χ.ώ=new String(c);}long ΰ=long.Parse(f[0]);ȳ s=null;for(int i=0;i<Χ.п.Count;i++) if(Χ.п[i].ΰ==ΰ) s=Χ.п[i];
if(s==null){s=new ȳ();s.ΰ=ΰ;Χ.п.Add(s);}s.ΕΗ=DateTime.Now;String[]w=f[10].Split(',');s.Ơ=w[0]=="1";if(w.Length>=3){s.ĉ=float.Parse(w[1]);s.Ī=float.Parse(w[2]);}int Ċ=int.Parse(f[5]);if(Ċ>=0&&Ċ<Χ.ώ.Length&&(s.У<0||Χ.ώ[s.У]=='1')&&Χ.ώ[Ċ]!='1'&&ΕЌ(Χ,Ċ,s)==null)
s.У=Ċ;}ȳ ΕЌ(ȗ Χ,int У,ȳ Δή){for(int i=0;i<Χ.п.Count;i++){ȳ o=Χ.п[i];if(o!=Δή&&o.У>=0&&ΕЍ(У,o.У,Χ.w,Χ.ΒЂ,Χ.Βύ,Math.Max(o.ĉ,Δή.ĉ),Math.Max(o.Ī,Δή.Ī)))
return o;}return null;}int ΘЋ(ȗ Χ,ȳ s){for(int k=0;k<Χ.ΔЊ.Count;k++){int c=Χ.ΔЊ[k];if(Χ.ώ[c]!='1'&&ΕЌ(Χ,c,s)==null) return c;}return-2;}String Θφ(){String s="";for(int j=0;j<ȅ.Count;j++){ȗ Χ=ȅ[j];int ώ=0;for(int i=0;i<Χ.ώ.Length;i++) if(Χ.ώ[i]=='1') ώ++;s+="Job "+(j+1)+": "+ώ+"/"+Χ.ώ.Length+" holes, "+Χ.п.Count+" ships\n";
}return s;}