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
public enum Κ{Ĭ,Ƹ,ǭ,ћ,ή}public enum Α{э,ȟ,ŷ,Ȥ,ȣ,Й,ф,
ĩ,ű,ȃ,ņ,ł,ĥ,
ƺ,ȧ,ǰ,Ņ,ΒΟ,ΑЀ,
ΔΝ,ΒΞ,Ň,ΑЂ,ĭ}public enum Ρ{ю,Ÿ,И,Γα,Αϋ,ΑЍ,ΑЎ,Ŭ,ŭ}public enum њ{None,Ņ,э,ΕЍ}public enum Σ{Main,Ζθ,Ʒ,ƶ,ΔΗ,Αω,
ΓΠ,ΖΣ,ΖΤ,ΖΟ,Ƽ,Ζν,
ΒΗ,ΓЏ,ΔΒ,Ζο,ΒΙ}public enum ц{ΒΕ,ư,Ιψ,ΖΧ,ή}public enum я{ƾ,ΕІ}public enum ы{ΒΔ,Γη,Ζς,Ƴ}public enum В{ΓЀ,ΕЊ,Γά,Γβ,Αώ,Γρ,ΑЈ}public enum ǘ{Γή,ΓΦ,ΓΧ}public enum Щ{Ŵ,ΙΦ,Ń}public enum ρ{ΓЂ,ΓІ,ΓЇ,Ζγ,Ζβ,
ΓЁ,ΒΑ,ΑБ,ΓЄ,
ΓЅ,ΓЃ}public enum ζ{ȝ,ċ,Ȝ,Αψ,ΓΤ,ΓΥ}public enum ǯ{ΑЅ,Αχ,ΘЌ}public enum ќ{ΒΚ,ΕЏ,Ια,ΓϏ}public enum Љ{None,Ź,ƹ,ΑА}public enum ό{ǀ,Αφ,ΑЋ,Ƴ,Γύ,ΑЌ,Γώ,ΙЀ,ΓЈ}public enum У{Ĭ,Ζί,Ζή,Γϊ,Γσ,Γέ,Ζΰ}class ά{public bool λ=false;public Vector3 Ε=new Vector3();
public Vector3 θ=new Vector3();public Vector3 ι=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 φ=new Vector3();public Vector3 п=new Vector3();public float Ш=0;public float Λω=0;public float[]Ȁ=null;public ά(){}public ά(ά κ){λ=κ.λ;Ε=κ.Ε;θ=κ.θ;ι=κ.ι;Ђ=κ.Ђ;φ=κ.φ;
п=κ.п;Ȁ=κ.Ȁ;}public ά(Vector3 Ε,Vector3 ι,Vector3 θ,Vector3 Ђ,Vector3 φ){this.Ε=Ε;this.θ=θ;this.ι=ι;this.Ђ=Ђ;this.Ш=0;this.φ=φ;}public void Θχ(List<IMyThrust>ΒΣ,List<string>ă){Ȁ=new float[ă.Count];for(int i=0;i<Ȁ.Length;i++)
Ȁ[i]=-1;for(int i=0;i<ΒΣ.Count;i++){string Ǘ=ΖΝ(ΒΣ[i]);int Β=ă.IndexOf(Ǘ);if(Β!=-1)
Ȁ[Β]=К(ΒΣ[i].MaxEffectiveThrust,ΒΣ[i].MaxThrust);}}}class ΓЉ{public ά η=null;public List<Vector3>Ƥ=new List<Vector3>();public float ΑΑ=0;public float Ετ=0;public float ƀ=0;public float Δυ=0;public Vector3 Βλ=new Vector3();}class ż{public ρ Т=ρ.ΓЂ;public float ǳ=0;public float Ĵ=0;
public string Ǵ="";public string ǹ="";DateTime Ηχ;public bool ΕΣ=false;private bool Δι=false;public bool ΓΪ(bool ǽ){if(ǽ){Ĵ=0;Δι=false;return false;}Δι=true;return Ĵ>ǳ;}public void Ιι(){ΓΪ(true);ΕΣ=false;}public void ΙБ(){if(Δι)
if((DateTime.Now-Ηχ).TotalSeconds>1){Ĵ++;Ηχ=DateTime.Now;}}public bool Γξ(){switch(Т){case ρ.ΓІ:return true;case ρ.ΓЇ:return true;}return false;}}class м{public String ΰ="";public String υ="";public int Њ=0;public Щ Ə=Щ.Ń;public м(String ΰ,String υ,int Њ,Щ Ə){this.ΰ=ΰ;this.υ=υ;
this.Њ=Њ;this.Ə=Ə;}}class ΘЇ{public ΘЇ(Vector3 γ,float ϋ){this.γ=γ;this.ϋ=ϋ;}public Vector3 γ;public float ϋ;}class ϊ{public DateTime ΔЉ;public DateTime ΖБ;public String Υ="";public String ΰ="";public String ΓΝ="";public String Λ="";public String Εγ="";public String ΒЂ="";public String ΕΡ="";
public Vector3 Ε=new Vector3();public List<м>Ћ=new List<м>();public Κ Μ=Κ.Ĭ;public Љ į;public float ȓ;public float Ȍ;public int Ў;public int Δξ=0;public int Εο=0;public bool Γψ(){return(DateTime.Now-ΔЉ).TotalSeconds>10;}public bool Γτ(){if(į!=Љ.Ź) return false;return(DateTime.Now-ΖБ).TotalSeconds>=4;
}public ϊ(String Υ){this.Υ=Υ;}public Љ Ī(String Ѕ,bool ΛΛ,bool а,bool ΚΠ){if(Ѕ==""&&!а) return Љ.None;if(į==Љ.Ź&&Γτ())
į=Љ.ΑА;if(ΛΛ){ΕΡ=Ѕ;į=Љ.Ź;ΖБ=DateTime.Now;return į;}else if(а){ΕΡ="";į=Љ.None;}else if(ΕΡ==Ѕ){if(ΚΠ) į=Љ.ƹ;return į;}return Љ.None;}}public String ΙΓ(String Κυ){switch(Κυ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΒЉ=new Vector3();Vector3 δ=new Vector3();Vector3 Βξ=new Vector3();Vector3 ΑΧ=new Vector3();Vector3 ΑΔ=new Vector3();
DateTime ΔЌ=new DateTime();bool Ē=true;int Ει=0;bool Ę=false;bool ĺ=false;bool Αέ=true;bool Θλ=false;bool ƍ=false;bool Βσ=false;float ğ=0;float Ŧ=0;int Ľ=0;int Αο=0;int Αγ=0;int Δί=0;float ƒ=0;float Ɓ=0;double ΕΖ=0;float ǆ=0;List<int>Ɗ=new List<int>();List<int>Ɯ=new List<int>();
Κ Μ=Κ.Ĭ;int А=0;float ȁ=0;float ț=0;float μ=0;float ΒЌ=0,Εά=0;IMyRemoteControl Ϋ;IMySensorBlock ψ;List<IMyTimerBlock>Ελ=new List<IMyTimerBlock>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMyThrust>Ą=new List<IMyThrust>();List<IMyGyro>Βο=new List<IMyGyro>();
List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();List<IMyLandingGear>ȳ=new List<IMyLandingGear>();List<IMyReactor>ļ=new List<IMyReactor>();List<IMyConveyorSorter>ΒА=new List<IMyConveyorSorter>();List<IMyGasTank>ǟ=new List<IMyGasTank>();List<IMyTerminalBlock>ǁ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ƅ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ŏ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Į=new List<IMyBatteryBlock>();List<IMyTextPanel>Ч=new List<IMyTextPanel>();List<IMyTextSurface>Ș=new List<IMyTextSurface>();List<IMyTextPanel>Œ=new List<IMyTextPanel>();
IMyTerminalBlock ǣ=null;ά Ο=new ά();ά ΔЂ=new ά();ά ΔЅ=new ά();bool ą=false;bool Ʃ=false;bool ƪ=false;bool Βυ=false;List<String>ă=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Ατ=new List<ά>();int Δο=0;double Θμ=0;ά Ζ=new ά();Vector3 ǒ;Vector3 Ĝ;Ρ ε=Ρ.ю;я ř=я.ƾ;int Ќ=0;
int Э=0;double ΑΤ=0;bool Βΰ=false;ż Ї=new ż();ż Ј=new ż();ы ѕ=ы.ΒΔ;bool ŀ=true;я Ȗ=я.ƾ;int ё=3;int ѐ=3;int о=30;ǘ џ=ǘ.Γή;bool Ĥ=true;bool ƌ=true;bool Ɔ=true;bool ŧ=false;В д=В.ΓЀ;bool ģ=true;float Ȓ=90;float ǐ=20;float р=5;float Ǒ=20;float Ȇ=0.70f;float Ʈ=1.50f;
float ƭ=2.50f;float ū=0f;float Ŗ=0f;Α Ϊ;Ρ Ǐ;Α Αθ;ΓЉ ξ=null;int Ё=0;int ώ=0;int б=0;int Ě=0;int Ť=30;int к=0;bool Д=true;Vector3 є;double в=0;bool ΒЋ=false;int Εχ=0;int Е=0;int ΔΥ=0;int Ȕ=0;int ΗЏ=0;Vector3 Αί=new Vector3();float ѓ=0;float у=0;int Ū=0;ά ΒБ=null;
float ΔЊ=0;float ΒΥ=0;float й=0;float Ŝ=0;bool ΔЃ=false;bool Ũ=false;bool Απ=false;bool Ė=false;bool Ď=false;DateTime Κβ=new DateTime();ά Ő=null;ά ΗΘ=null;float ȩ=0;њ ȇ;float ė=0;float ũ=0;String ΕΑ="";float Βω=0;float Ȍ=0;float ȓ=0;List<м>Ƿ=new List<м>();float е=0;
bool Αε=false;bool Θΰ=false;bool ΘΚ=false;bool ΓΕ=false;bool ΘΛ=false;float ΕΔ=0;float ΕΠ=0;Vector3 Ηά=new Vector3();Vector3 Ѓ=new Vector3();Vector3 ΓΖ=new Vector3(1,1,1);float Εθ=1;Vector3 Αμ=new Vector3();bool Βτ=false;bool Δϋ=false;bool ΔЏ=false;bool Ζύ=false;float Βή=0;
float ΒΡ=2;Vector3 Θά;Vector3 Εη;Vector3 Εζ;float[,]Ȃ=new float[3,2];Dictionary<String,float[,]>Ǩ=new Dictionary<string,float[,]>();Dictionary<String,float>Θι=new Dictionary<String,float>();int ΔΩ=0;Dictionary<String,float[]>ĝ=new Dictionary<string,float[]>();float ΕΤ;bool Βα=false;
int ΒΤ=0;int ĸ=0;int Βρ=0;int ŗ=0;String Λ="";int[]ΗЅ=new int[Enum.GetValues(typeof(Σ)).Length];bool ΕΘ=false;int ŝ=0;int Ў=0;Σ ύ=Σ.Main;int ΛΫ=0,Λά=0,Λέ=0,Λή=0;int ΛΖ=0,ΛΗ=0;bool ƃ=false;bool Ζώ=false;String ơ="";String Δή="";IMyBroadcastListener Ǳ=null;
bool ΗΫ=true;List<ϊ>Н=null;ϊ М=null;int ΒΩ=0,ΒΪ=0;int ΗΓ=0,ΗΔ=0;int ΚΧ=0,ΚΨ=0,ΚΩ=0;String Κχ="";Exception ę=null;bool ΒЊ=false;Ρ ƞ=Ρ.ю;bool ΗΛ=false;bool Ηυ=true;bool ΕΨ=false;String Θκ="";bool Αξ=false;bool Εβ=true;int Βς=0;int[]Ǧ=null;Vector3 Δΰ=new Vector3();
bool Ηα=false;bool Ǚ(IMyTerminalBlock Ω)=>Ω.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Γο(Me,controllerTag,true))
Μ=Κ.ћ;ν(Σ.Main);if(Μ!=Κ.ћ){Λ="Welcome to [PAM]!";ќ ΕΧ=Ικ();if(Μ==Κ.Ĭ){List<IMyShipDrill>ΗΨ=new List<IMyShipDrill>();List<IMyShipGrinder>Ηβ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗΨ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(Ηβ,q=>q.CubeGrid==Me.CubeGrid);if(ΗΨ.Count>0)
{Μ=Κ.Ƹ;Λ="Miner mode enabled!";}else if(Ηβ.Count>0){Μ=Κ.ǭ;Λ="Grinder mode enabled!";}else{Μ=Κ.ή;ȥ(ц.ή);}}if(ΕΧ==ќ.ΒΚ) Αέ=false;if(ΕΧ==ќ.ΕЏ) Λ="Data restore failed!";if(ΕΧ==ќ.ΓϏ) Λ="New version, wipe data";}}void Main(string Ф,UpdateType Λη){try{if(ę!=null){if(Ф.Trim().ToUpper()=="RETRY")
{ę=null;Ē=true;Ľ=0;if(Μ!=Κ.ћ) Є();Λ="Restarted after error";}else{ΙΞ();Ζζ();return;}}ĺ=(Λη&UpdateType.Update10)!=0;if(ĺ) Ει++;Ę=Ει>=10;if(Ę) Ει=0;if(ĺ){ΒΤ++;if(ΒΤ>4) ΒΤ=0;Αγ=(int) Math.Max(0,10-(DateTime.Now-ΔЌ).TotalSeconds);}if(Ф!="") Κχ=Ф;if(Μ!=Κ.ћ)
Ιχ(Ф);else
ΘЅ(Ф);ΕΘ=false;try{int ΗΙ=Runtime.CurrentInstructionCount;float ţ=К(ΗΙ,Runtime.MaxInstructionCount);if(ţ>0.90) Λ="Max. instructions >90%";if(ţ>ƒ) ƒ=ţ;if(Βα){Ɗ.Add(ΗΙ);while(Ɗ.Count>10) Ɗ.RemoveAt(0);Ɓ=0;for(int i=0;i<Ɗ.Count;i++) Ɓ+=Ɗ[i];Ɓ=К(К(Ɓ,Ɗ.Count),Runtime.MaxInstructionCount);
double ΔЋ=Runtime.LastRunTimeMs;if(Βσ&&ΔЋ>ΕΖ) ΕΖ=ΔЋ;Ɯ.Add((int)(ΔЋ*1000f));while(Ɯ.Count>10) Ɯ.RemoveAt(0);ǆ=0;for(int i=0;i<Ɯ.Count;i++) ǆ+=Ɯ[i];ǆ=К(ǆ,Ɯ.Count)/1000f;}}catch{ƒ=0;}}catch(Exception e){ę=e;}}void Ιχ(string Ф){bool ΒЍ=false;String ΖЈ="";if(А<=1&&!Γχ(Ф))
Ζη(Ф);int ΕΙ=50;while(Ǳ!=null&&Ǳ.HasPendingMessage&&ΕΙ-->0){MyIGCMessage β=Ǳ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;String ȕ="";if(А<=1&&Ζε(ref Θ,out ȕ,out ΖЈ)&&ȕ==BROADCAST_SELF_ID){Ζη(Θ);ΒЍ=true;break;}}bool Ηο=Βσ&&Ϊ==Α.э&&!ƍ&&!ΒЍ&&Ľ==0&&!Βυ;
if(Ę&&Ϊ!=Α.э) ΒЍ=true;if((ĺ&&!Ηο)||(Ę&&Ηο)){if(Ľ==0&&(Αγ<=0||Ē)){Θλ=А>0;А=0;Ľ=1;Ȧ();Ιξ();ΖΥ();Ȟ("Scan 1");}else if(Ľ==1){Ľ=2;Ȧ();Ιο();Ȟ("Scan 2");}else if(Ľ==2){Ľ=0;Ȧ();Ιπ();Ιρ();if(А<=1) Ζέ();Ȟ("Scan 3");ΔЌ=DateTime.Now;if(А<=1&&Αέ)
ΒЉ=ΑІ(Ϋ,Ϋ.CenterOfMass);Αέ=false;if(Ē){Ē=false;bool Λλ=ε==Ρ.И;Є();if(Ηυ&&resumeAfterRestart&&Λλ&&А<=1) ΕΨ=true;}Ηυ=false;if(Θλ&&А==0) Λ="Setup complete";}else{if(ε==Ρ.И&&Μ!=Κ.ή){Ȧ();Θπ();Ȟ("Inv balance");}Ȧ();switch(Αο){case 0:ΙЄ();break;case 1:ΙЊ();break;case 2:ΙЃ();break;
case 3:ΚΔ();break;case 4:ΙЉ();break;case 5:Θϊ();break;case 6:Θω(Ϋ);break;}Ȟ("Update: "+Αο);Αο++;if(Αο>6){Αο=0;Βσ=true;if(ƞ!=Ρ.ю){switch(ƞ){case Ρ.Ŭ:ħ();break;case Ρ.ŭ:ħ();break;case Ρ.И:ħ();break;case Ρ.ΑЍ:ΑЃ();break;case Ρ.ΑЎ:ň();break;}ƞ=Ρ.ю;}}}if(!Ē){if(!ȡ(Ϋ,true))
{Ϋ=null;Αέ=true;А=2;}if(А>=2&&Ϊ!=Α.э)
Є();if(А<=1){Ŧ=Ϋ.CalculateShipMass().PhysicalMass;ğ=(float) Ϋ.GetShipSpeed();δ=ΙЂ(Ϋ,ΒЉ);Βξ=Ϋ.WorldMatrix.Forward;ΑΧ=Ϋ.WorldMatrix.Left;ΑΔ=Ϋ.WorldMatrix.Down;ΙЎ();ΚΑ();ΙІ();if(ΕΨ){ΕΨ=false;ħ();if(Ϊ!=Α.э) Λ="Job resumed after restart";}if(Ϊ!=Α.э){ƍ=false;Ζκ(false);ΕЄ(false);
String ΛΠ=Γκ(Ϊ)+" "+(int) Ϊ;Ȧ();ΙЌ();ΔΜ(false);Ȟ(ΛΠ);ΚΒ();ΙЍ();Ȧ();ΙЈ();Ȟ("Thruster");Ȧ();Ζω();Ȟ("Gyroscope");}else{if(ƍ){if(ΑЊ()){л(ΑΔ,Βξ,ΑΧ,0.25f,true);Ζω();Λ="Aligning to planet: "+Math.Round(Βή-0.25f,2)+"°";if(Βτ) Γν(true,true);}else Γν(true,true);}}Αξ=false;}}}Ȧ();Ζζ();Ȟ("Print");
if(ΒЍ||Δί<=0){Ȧ();Θρ(ΖЈ);Ȟ("Broadcast");Δί=4;}else if(Ę) Δί--;}void Ζη(string Ф){if(Ф=="") return;var τ=Ф.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ȩ=τ.ElementAtOrDefault(1);var Ǆ=τ.ElementAtOrDefault(2);var ΖЀ=τ.ElementAtOrDefault(3);String Ȳ="Invalid argument: "+Ф;
bool Εσ=false;switch(Ѕ){case "UP":this.Γό(false);break;case "DOWN":this.Γϋ(false);break;case "UPLOOP":this.Γό(true);break;case "DOWNLOOP":this.Γϋ(true);break;case "APPLY":this.ΕЂ(true);break;case "MRES":Ў=0;break;case "STOP":this.Є();break;case "PATHHOME":{this.Є();this.ΔΖ();}break;
case "PATH":{this.Є();this.ΔΖ();Ο.λ=true;}break;case "START":{this.Є();ΔΕ();}break;case "ALIGN":{Γν(!ƍ,false);}break;case "CONT":{this.Є();this.ħ();}break;case "JOBPOS":{this.Є();this.ň();}break;case "HOMEPOS":{this.Є();this.ΑЃ();}break;case "FULL":{Αε=true;}break;
case "RESET":{ΒЊ=true;А=2;}break;case "EXPORT":{ΘЍ();}break;case "IMPORT":{ΙΤ(Ȩ=="JOB");}break;default:Εσ=true;break;}if(Μ!=Κ.ή){switch(Ѕ){case "SHUTTLE":{ΘЊ();}break;case "CFGS":{if(!ΘЃ(Ȩ,Ǆ,ΖЀ)) Λ=Ȳ;}break;case "CFGB":{if(!ΘЂ(Ȩ,Ǆ)) Λ=Ȳ;}break;case "CFGL":
{if(!ŉ(ref Ȓ,true,ό.ΑЋ,Ȩ,"")||!ΘЄ(Ǆ))
Λ=Ȳ;}break;case "CFGE":{if(!ŉ(ref р,true,ό.ΑЌ,Ȩ,"IG")||
!ŉ(ref ǐ,true,ό.Γύ,Ǆ,"IG")||
!ŉ(ref Ǒ,true,ό.Γώ,ΖЀ,"IG"))
Λ=Ȳ;}break;case "CFGA":{if(!ŉ(ref Ȇ,false,ό.Αφ,Ȩ,"")) Λ=Ȳ;}break;case "CFGW":{if(!ŉ(ref Ʈ,false,ό.ǀ,Ȩ,"")||
!ŉ(ref ƭ,false,ό.ǀ,Ǆ,""))
Λ=Ȳ;}break;case "NEXT":{ΓΟ(false);}break;case "PREV":{ΓΟ(true);}break;default:if(Εσ) Λ=Ȳ;break;}}else{switch(Ѕ){case "UNDOCK":{Αξ=true;}break;default:if(Εσ) Λ=Ȳ;break;}}}String ΙΔ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΘЊ(){Є();Μ=Κ.ή;ȥ(ц.ή);Ζ.λ=false;Ο.λ=false;ψ=null;ω.Clear();ε=Ρ.ю;}void Γν(bool О,bool Ѐ){if(!О) Λ="Aligning canceled";if(Ѐ) Λ="Aligning done";if(Ѐ||!О){ƍ=false;Ζσ();ΒΘ(false,0,0,0,0);return;}if(ΑЊ()) ƍ=true;}bool ΘЃ(String Λν,String Κξ,String Κέ)
{bool Λι=ε==Ρ.И;int w,h,d;if(int.TryParse(Λν,out w)&&int.TryParse(Κξ,out h)&&int.TryParse(Κέ,out d)){this.Є();ё=w;ѐ=h;о=d;ō(false);ƿ(false,false);if(Λι) ħ();return true;}return false;}bool ΘЄ(String ǃ){if(ǃ=="ON"){Ĥ=true;return true;}if(ǃ=="OFF"){Ĥ=false;return true;}
return false;}bool ΘЂ(String ΗΧ,String Βί){bool Ā=true;if(ΗΧ=="HOME") ŀ=true;else if(ΗΧ=="STOP") ŀ=false;else Ā=false;if(Βί=="HOME") ѕ=ы.ΒΔ;else if(Βί=="STOP") ѕ=ы.Ζς;else if(Βί=="JOB") ѕ=ы.Γη;else if(Βί=="IG") ѕ=ы.Ƴ;else Ā=false;return Ā;}void ν(Σ ч){ΗЅ[(int) ύ]=Ў;
Ў=ΗЅ[(int) ч];if(ч==Σ.Ƽ) Ў=0;ύ=ч;if(Μ!=Κ.ћ)
Ō(ύ==Σ.Ʒ,false,0,0);ΕΘ=true;}void Γό(bool ΔА){if(Ў>0) Ў--;else if(ΔА) Ў=ŝ-1;}void Γϋ(bool ΔА){if(Ў<ŝ-1) Ў++;else if(ΔА) Ў=0;}String ȥ(ц β){switch(β){case ц.ΒΕ:Λ="Job is running";break;case ц.ư:Λ="Connector not ready!";break;case ц.Ιψ:Λ="Ship modified, path outdated!";break;
case ц.ΖΧ:Λ="Interrupted by player!";break;case ц.ή:Λ="Shuttle mode enabled!";break;}return "";}String ΙΚ(я γ){switch(γ){case я.ƾ:return "Top-Left";case я.ΕІ:return "Center";default:return "";}}String ΘБ(ǘ Џ){switch(Џ){case ǘ.ΓΦ:return "Auto"+(Μ==Κ.Ƹ?" (Ore)":"");case ǘ.ΓΧ:return "Auto (+Stone)";
case ǘ.Γή:return "Default";default:return "";}}String Γθ(Ρ І){switch(І){case Ρ.ю:return "No job";case Ρ.Ÿ:return "Job paused";case Ρ.И:return "Job active";case Ρ.Ŭ:return "Job active";case Ρ.ŭ:return "Job active";case Ρ.Γα:return "Job done";case Ρ.Αϋ:return "Job changed";case Ρ.ΑЍ:return "Move home";
case Ρ.ΑЎ:return "Move to job";}return "";}String ΖΔ(ы ΚΟ){switch(ΚΟ){case ы.ΒΔ:return "Return home";case ы.Γη:return "Fly to job pos";case ы.Ζς:return "Stop";case ы.Ƴ:return "Ignore";}return "";}String ΙΒ(В Џ){switch(Џ){case В.ΓЀ:return "Off";case В.Γβ:return "Drop pos (Stone) ";case В.Αώ:return "Drop pos (Sto.+Ice)";
case В.ΕЊ:return "Cur. pos (Stone)";case В.Γά:return "Cur. pos (Sto.+Ice)";case В.Γρ:return "In motion (Stone)";case В.ΑЈ:return "In motion (Sto.+Ice)";}return "";}String ΖΒ(њ І){switch(І){case њ.None:return "No batteries";case њ.Ņ:return "Charging";case њ.ΕЍ:return "Discharging";}return "";}
String Γκ(Α І){String Λχ=Μ==Κ.ή?"target":"job";switch(І){case Α.э:return "Idle";case Α.ȟ:return "Flying to XY position";case Α.ŷ:return Μ==Κ.ǭ?"Grinding":"Mining";case Α.Ȥ:return "Returning";case Α.Ň:return "Flying to drop pos";case Α.ȣ:return "Returning to dock";case Α.Й:return "Flying to dock area";
case Α.ф:return "Flying to job area";case Α.ĩ:return "Flying to path";case Α.ű:return "Flying to job position";case Α.ȃ:return "Approaching dock";case Α.ņ:return "Docking";case Α.ł:return "Aligning";case Α.ĥ:return "Aligning";case Α.ƺ:return "Retry docking";case Α.ȧ:return "Unloading";case Α.ĭ:return Θκ;
case Α.ǰ:return "Undocking";case Α.Ņ:return "Charging batteries";case Α.ΒΟ:return "Waiting for uranium";case Α.ΑЀ:return "Filling up hydrogen";case Α.ΔΝ:return "Waiting for ejection";case Α.ΒΞ:return "Waiting for ejection";case Α.ΑЂ:return "Flying to drop pos";}return "";}String ΖΞ(ρ Т){switch(Т)
{case ρ.ΓЂ:return "On \"Undock\" command";case ρ.ΓІ:return "On player entered cockpit";case ρ.Ζγ:return "On ship is full";case ρ.Ζβ:return "On ship is empty";case ρ.ΓЇ:return "On time delay";case ρ.ΒΑ:return "On batteries empty(<25%)";case ρ.ΑБ:return "On batteries empty(=0%)";case ρ.ΓЁ:return "On batteries full";
case ρ.ΓЅ:return "On hydrogen empty(<25%)";case ρ.ΓЃ:return "On hydrogen empty(=0%)";case ρ.ΓЄ:return "On hydrogen full";}return "";}bool Ν(ref String Γ,int ΘΗ,int ķ,bool Ι,String Ś){ŝ+=1;if(ΘΗ==ķ) Ś=">"+Ś+(ΒΤ>=2?" .":"");else Ś=" "+Ś;Γ+=Ś+"\n";return ΘΗ==ķ&&Ι;}String ΕЂ(bool Ι)
{int Β=0;int Η=Ў;ŝ=0;String Ơ="———————————————\n";String Ʀ="--------------------------------------------\n";String Γ="";Γ+=Γθ(ε)+" | "+(Ο.λ?"Ready to dock":"No dock")+"\n";Γ+=Ơ;double ȗ=Math.Max(Math.Round(this.в),0);if(ύ==Σ.Main){bool Ĺ=Μ==Κ.ή;if(Ν(ref Γ,Η,Β++,Ι," Record path & set home")) ΔΖ();
if(Μ==Κ.Ƹ)
if(Ν(ref Γ,Η,Β++,Ι," Setup mining job")) ν(Σ.Ʒ);if(Μ==Κ.ǭ)
if(Ν(ref Γ,Η,Β++,Ι," Setup grinding job")) ν(Σ.Ʒ);if(Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Setup shuttle job")) ν(Σ.ΔΒ);if(Ν(ref Γ,Η,Β++,Ι," Continue job")) ħ();if(Ν(ref Γ,Η,Β++,Ι," Fly to home position")) ΑЃ();if(Ν(ref Γ,Η,Β++,Ι," Fly to job position")) ň();if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(Ĺ) ν(Σ.ΒΙ);else ν(Σ.Αω);if(Ν(ref Γ,Η,Β++,Ι," Info")) ν(Σ.ΖΣ);if(Μ!=Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Help")) ν(Σ.ΖΟ);}else if(ύ==Σ.Ʒ){double Λμ=Math.Round(ё*ȁ,1);double Κν=Math.Round(ѐ*ț,1);String Ж="";if(Ν(ref Ж,Η,Β++,Ι," Start new job!")) ΔΕ();if(Ν(ref Ж,Η,Β++,Ι," Change current job")){ƿ(false,false);ν(Σ.Main);}if(Ν(ref Ж,Η,Β++,Ι," Width + (Width: "+ё+" = "+Λμ+"m)")){Ů(ref ё,5,20,1);ō(true);}
if(Ν(ref Ж,Η,Β++,Ι," Width -")){Ů(ref ё,-5,20,-1);ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Height + (Height: "+ѐ+" = "+Κν+"m)")){Ů(ref ѐ,5,20,1);ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Height -")){Ů(ref ѐ,-5,20,-1);ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Depth + ("+(џ==ǘ.Γή?"Depth":"Min")+": "+о+"m)")){Ů(ref о,5,50,2);ō(true);}
if(Ν(ref Ж,Η,Β++,Ι," Depth -")){Ů(ref о,-5,50,-2);ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Depth mode: "+ΘБ(џ))){џ=Ĩ(џ);}if(Ν(ref Ж,Η,Β++,Ι," Start pos: "+ΙΚ(Ȗ))){Ȗ=Ĩ(Ȗ);}if(Μ==Κ.ǭ&&џ==ǘ.ΓΧ) џ=Ĩ(џ);Γ+=ΓЎ(8,Ж,Η,ref ΛΖ);}else if(ύ==Σ.ΔΒ){float[]ΗΝ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ν(ref Γ,Η,Β++,Ι," Next")) ν(Σ.Ζο);if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=" Leave connector 1:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΞ(Ї.Т))) Ї.Т=Ĩ(Ї.Т);if(!Ї.Γξ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЄ((int) Ї.ǳ))) Ї.ǳ=ΕЋ(Ї.ǳ,ΗΝ);Γ+=" Leave connector 2:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΞ(Ј.Т))) Ј.Т=Ĩ(Ј.Т);
if(!Ј.Γξ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЄ((int) Ј.ǳ))) Ј.ǳ=ΕЋ(Ј.ǳ,ΗΝ);}else if(ύ==Σ.Ζο){if(Ν(ref Γ,Η,Β++,Ι," Start job!")) ΔΕ();if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.ΔΒ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.Ǵ!=""?Ї.Ǵ:"-"))) Ї.Ǵ=ΑЏ(ref ΛΫ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.ǹ!=""?Ї.ǹ:"-"))) Ї.ǹ=ΑЏ(ref Λέ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.Ǵ!=""?Ј.Ǵ:"-"))) Ј.Ǵ=ΑЏ(ref Λά);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǹ!=""?Ј.ǹ:"-"))) Ј.ǹ=ΑЏ(ref Λή);
}else if(ύ==Σ.ƶ){String Ηό=Ȓ+" %";if(Ę) ĸ++;if(ĸ>1){ĸ=0;Βρ++;if(Βρ>1) Βρ=0;}bool[]Εί=new bool[]{ļ.Count==0,ȇ==њ.None,ǟ.Count==0};int Θε=0;while(true){Θε++;ŗ++;if(ŗ>Εί.Length-1) ŗ=0;if(Θε>=Εί.Length) break;if(!Εί[ŗ]) break;}bool Ĺ=Μ==Κ.ή;if(!Ĺ&&Ĥ&&е!=-1&&Βρ==0)
Ηό=е<1000000?Math.Round(е)+" Kg":Math.Round(е/1000)+" t";if(Ν(ref Γ,Η,Β++,Ι," Stop!")){Є();ν(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(!Ĺ) ν(Σ.Αω);else ν(Σ.ΒΙ);if(!Ĺ){if(Ν(ref Γ,Η,Β++,Ι," Next hole")) ΓΟ(false);}else if(Ν(ref Γ,Η,Β++,Ι," Undock")) Αξ=true;Γ+=Ʀ;if(!Ĺ) Γ+="Progress: "+Math.Round(ΑΤ,1)+" %\n";Γ+="State: "+Γκ(Ϊ)+" "+ȗ+"m \n";Γ+="Load: "+Βω+" % Max: "+Ηό+" \n";if(ŗ==0) Γ+="Uranium: "+(ļ.Count==0?"No reactors":Math.Round(ũ,1)+"Kg "+(р==-1?"":" Min: "+р+" Kg"))+"\n";
if(ŗ==1) Γ+="Battery: "+(ȇ==њ.None?ΖΒ(ȇ):ȩ+"% "+(ǐ==-1||Ĺ?"":" Min: "+ǐ+" %"))+"\n";if(ŗ==2) Γ+="Hydrogen: "+(ǟ.Count==0?"No tanks":Math.Round(ė,1)+"% "+(Ǒ==-1||Ĺ?"":" Min: "+Ǒ+" %"))+"\n";}else if(ύ==Σ.Αω){String Ж="";if(Ν(ref Ж,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}
if(Ν(ref Ж,Η,Β++,Ι," Max load: "+Ȓ+"%")) ъ(ref Ȓ,Ȓ<=80?-10:-5,ό.ΑЋ,false);if(Ν(ref Ж,Η,Β++,Ι," Weight limit: "+(Ĥ?"On":"Off"))) Ĥ=!Ĥ;if(Ν(ref Ж,Η,Β++,Ι," Ejection: "+ΙΒ(д))) д=Ĩ(д);if(Ν(ref Ж,Η,Β++,Ι," Toggle sorters: "+(ŧ?"On":"Off"))){ŧ=!ŧ;if(ŧ) ȅ(Εβ);}
if(Ν(ref Ж,Η,Β++,Ι," Unload ice: "+(ģ?"On":"Off"))) ģ=!ģ;if(Ν(ref Ж,Η,Β++,Ι," Uranium: "+(р==-1?"Ignore":"Min "+р+"Kg"))) ъ(ref р,(р>5?-5:-1),ό.ΑЌ,true);if(Ν(ref Ж,Η,Β++,Ι," Battery: "+(ǐ==-1?"Ignore":"Min "+ǐ+"%"))) ъ(ref ǐ,-5,ό.Γύ,true);if(Ν(ref Ж,Η,Β++,Ι," Hydrogen: "+(Ǒ==-1?"Ignore":"Min "+Ǒ+"%"))) ъ(ref Ǒ,-10,ό.Γώ,true);
if(Ν(ref Ж,Η,Β++,Ι," When done: "+(ŀ?"Return home":"Stop"))) ŀ=!ŀ;if(Ν(ref Ж,Η,Β++,Ι," On damage: "+ΖΔ(ѕ))) ѕ=Ĩ(ѕ);if(Ν(ref Ж,Η,Β++,Ι," Advanced...")) ν(Σ.ΓΠ);Γ+=ΓЎ(8,Ж,Η,ref ΛΗ);}else if(ύ==Σ.ΓΠ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}
if(Ν(ref Γ,Η,Β++,Ι,(Μ==Κ.ǭ?" Grinder":" Drill")+" inv. balancing: "+(ƌ?"On":"Off"))) ƌ=!ƌ;if(Ν(ref Γ,Η,Β++,Ι," Enable"+(Μ==Κ.ǭ?" grinders":" drills")+": "+(Ɔ?"Fwd + Bwd":"Fwd"))) Ɔ=!Ɔ;if(Ν(ref Γ,Η,Β++,Ι," Work speed fwd.: "+Ʈ+"m/s")) ъ(ref Ʈ,0.5f,ό.ǀ,false);
if(Ν(ref Γ,Η,Β++,Ι," Work speed bwd.: "+ƭ+"m/s")) ъ(ref ƭ,0.5f,ό.ǀ,false);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȇ*100f)+"%"+(Ȇ>0.80f?" (risky)":""))) ъ(ref Ȇ,0.1f,ό.Αφ,false);if(Ν(ref Γ,Η,Β++,Ι," Width overlap: "+ū*100f+"%")) ΕЇ(true,0.05f);if(Ν(ref Γ,Η,Β++,Ι," Height overlap: "+Ŗ*100f+"%")) ΕЇ(false,0.05f);
}else if(ύ==Σ.ΒΙ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Max load: "+Ȓ+"%")) ъ(ref Ȓ,Ȓ<=80?-10:-5,ό.ΑЋ,false);if(Ν(ref Γ,Η,Β++,Ι," Unload ice: "+(ģ?"On":"Off"))) ģ=!ģ;if(Ν(ref Γ,Η,Β++,Ι," Uranium: "+(р==-1?"Ignore":"Min "+р+"Kg"))) ъ(ref р,(р>5?-5:-1),ό.ΑЌ,true);
if(Ν(ref Γ,Η,Β++,Ι," Battery: "+(ǐ==-1?"Ignore":"Charge up"))) ǐ=(ǐ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," Hydrogen: "+(Ǒ==-1?"Ignore":"Fill up"))) Ǒ=(Ǒ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," On damage: "+ΖΔ(ѕ))) ѕ=Ĩ(ѕ);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȇ*100f)+"%"+(Ȇ>0.80f?" (risky)":""))) ъ(ref Ȇ,0.1f,ό.Αφ,false);
}else if(ύ==Σ.Ζθ){double Κψ=0;if(Ψ.Count>0) Κψ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ν(ref Γ,Η,Β++,Ι," Stop path recording")) Ζτ();if(Μ!=Κ.ή){if(Ν(ref Γ,Η,Β++,Ι," Home: "+(ą?"Use old home":(Ο.λ?"Was set! ":"none ")))) ą=!ą;}else{if(Ν(ref Γ,Η,Β++,Ι," Connector 1: "+(ą?"Use old connector":(Ο.λ?"Was set! ":"none ")))) ą=!ą;
if(Ν(ref Γ,Η,Β++,Ι," Connector 2: "+(Ʃ?"Use old connector":(Ζ.λ?"Was set! ":"none ")))) Ʃ=!Ʃ;}if(Ν(ref Γ,Η,Β++,Ι," Path: "+(ƪ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ƪ=!ƪ;Γ+=Ʀ;Γ+="Wp spacing: "+Math.Round(Θμ)+"m\n";}else if(ύ==Σ.ΔΗ){if(Ν(ref Γ,Η,Β++,Ι," Stop")){Є();ν(Σ.Main);}
Γ+=Ʀ;Γ+="State: "+Γκ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(ğ,1)+"m/s\n";Γ+="Target dist: "+ȗ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+Εχ+"\n";}else if(ύ==Σ.ΖΣ){List<IMyTerminalBlock>ő=ΖΕ();if(Ę) ĸ++;if(ĸ>=ő.Count) ĸ=0;if(Ν(ref Γ,Η,Β++,Ι," Next")) ν(Σ.ΖΤ);
Γ+=Ʀ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βω,1)+"% "+Math.Round(Ȍ,1)+" / "+Math.Round(ȓ,1)+"\n";Γ+="Uranium: "+(ļ.Count==0?"No reactors":Math.Round(ũ,1)+"Kg "+ΕΑ)+"\n";Γ+="Battery: "+(ȇ==њ.None?"":ȩ+"% ")+ΖΒ(ȇ)+"\n";Γ+="Hydrogen: "+(ǟ.Count==0?"No tanks":Math.Round(ė,1)+"% ")+"\n";
Γ+="Damage: "+(ő.Count==0?"None":""+(ĸ+1)+"/"+ő.Count+" "+ő[ĸ].CustomName)+"\n";}else if(ύ==Σ.ΖΤ){if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=Ʀ;Γ+="Next scan: "+Αγ+"s\n";Γ+="Drill area: "+Math.Round(ȁ,1)+"x"+Math.Round(ț,1)+"m Ship: "+Math.Round(ΒЌ,1)+"x"+Math.Round(Εά,1)+"m ("+Math.Round(μ,1)+"m)\n";
Γ+="Broadcast: "+(ƃ?"Online - "+ơ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ƒ*100f,1)+"% \n";}else if(ύ==Σ.ΖΟ){if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=Ʀ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+Αγ+"s\n";if(ΒЊ) Γ="Recompile script now";int Κω=Γ.Split('\n').Length;for(int i=Κω;i<=10;i++) Γ+="\n";Γ+=Ơ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕЇ(bool Κτ,float ǧ){Є();ƿ(true,false);if(Κτ) ъ(ref ū,ǧ,ό.ΓЈ,false);else ъ(ref Ŗ,ǧ,ό.ΓЈ,false);ΕЅ();Ō(true,true,0,0);}float ΕЋ(float ο,float[]ΓΜ){float ί=ΓΜ[0];for(int i=ΓΜ.Length-1;i>=0;i--)
if(ο<ΓΜ[i]) ί=ΓΜ[i];return ί;}String ΑЏ(ref int ķ){String ί="";if(ķ>=Ελ.Count) ķ=-1;if(ķ>=0) ί=Ελ[ķ].CustomName;ķ++;return ί;}void ΒΜ(string ΰ){if(ε!=Ρ.И) return;if(ΰ=="") return;IMyTerminalBlock Ω=χ.GetBlockWithName(ΰ);if(Ω==null||!(Ω is IMyTimerBlock)){Λ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ω).Trigger();}void Ů(ref int value,int ǉ,int Ģ,int Εΰ){if(ǉ==0) return;if(value<Ģ&&Εΰ>0||value<=Ģ&&Εΰ<0){value+=Εΰ;return;}int ΚΖ=Math.Abs(ǉ);int ΔΞ=0;int ΕΚ=1;while(true){ΔΞ+=ΕΚ*ΚΖ*10;if(ǉ<0&&value-Ģ<=ΔΞ) break;if(ǉ>0&&value-Ģ<ΔΞ) break;
ΕΚ++;}value+=ΕΚ*ǉ;}void ō(bool Λϊ){ё=Math.Max(ё,1);ѐ=Math.Max(ѐ,1);о=Math.Max(о,0);Ō(ύ==Σ.Ʒ,false,0,0);}bool ŉ(ref float value,bool ΛΓ,ό υ,String Ř,String Κρ){if(Ř=="") return false;float Ɩ=-1;bool Ημ=false;if(Ř.ToUpper()==Κρ) Ημ=true;else if(!float.TryParse(Ř,out Ɩ)) return false;
else Ɩ=Math.Max(0,Ɩ);if(ΛΓ) Ɩ=(float) Math.Round(Ɩ);ъ(ref value,Ɩ,υ,Ημ,false);return true;}void ъ(ref float value,float ǧ,ό υ,bool Αν){ъ(ref value,value+ǧ,υ,Αν,true);}void ъ(ref float value,float Ɠ,ό υ,bool Αν,bool Λρ){float ǻ=0,Ǻ=0;if(υ==ό.ǀ){ǻ=0.5f;Ǻ=10f;}if(υ==ό.Αφ){ǻ=0.1f;Ǻ=1f;}
if(υ==ό.Ƴ){ǻ=50f;Ǻ=100f;}if(υ==ό.Γύ){ǻ=5f;Ǻ=30f;}if(υ==ό.ΑЌ){ǻ=1f;Ǻ=25f;}if(υ==ό.ΑЋ){ǻ=10f;Ǻ=95f;}if(υ==ό.Γώ){ǻ=10f;Ǻ=90f;}if(υ==ό.ΙЀ){ǻ=10f;Ǻ=1800;}if(υ==ό.ΓЈ){ǻ=0.0f;Ǻ=0.75f;}if(Ɠ==-1&&Αν){value=-1;return;}if(value==-1) Αν=false;
bool ΗБ=Ɠ<ǻ||Ɠ>Ǻ;if(ΗБ&&Λρ){if(Ɠ<value) value=Ǻ;else if(Ɠ>value) value=ǻ;}else value=Ɠ;if(ΗБ&&Αν) value=-1;else value=Math.Max(ǻ,Math.Min(value,Ǻ));value=(float) Math.Round(value,2);}void ΓΟ(bool ΚЏ){if(ΚЏ) б=Math.Max(0,б-1);else б++;Γδ();Γγ();for(int k=0;k<Ȱ.Count&&k<г.Length;k++) ΓБ(Ȱ[k],k<б);
Г=-1;ΔΜ(true);}T Ĩ<T>(T ο){int Β=Array.IndexOf(Enum.GetValues(ο.GetType()),ο);Β++;if(Β>=ΘЋ(ο)) Β=0;return(T) Enum.GetValues(ο.GetType()).GetValue(Β);}int ΘЋ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔΖ(){Ατ.Clear();for(int i=0;i<Ψ.Count;i++) Ατ.Add(Ψ[i]);Ψ.Clear();
Βυ=true;ΔЂ=new ά(Ο);ΔЅ=new ά(Ζ);Ο.λ=false;if(Μ==Κ.ή) Ζ.λ=false;for(int i=0;i<Ǩ.Count;i++)
if(!ă.Contains(Ǩ.Keys.ElementAt(i)))
ă.Add(Ǩ.Keys.ElementAt(i));ą=false;Ʃ=false;ƪ=false;ν(Σ.Ζθ);}void Ζτ(){if(ą) Ο=ΔЂ;if(Ʃ) Ζ=ΔЅ;if(ƪ){Ψ.Clear();for(int i=0;i<Ατ.Count;i++) Ψ.Add(Ατ[i]);}Βυ=false;Є();ν(Σ.Main);}void ΙЎ(){if(!Βυ) return;if(Ϊ!=Α.э){Ζτ();return;}if(!ΔЂ.λ) ą=false;if(!ΔЅ.λ) Ʃ=false;if(Ατ.Count<=1) ƪ=false;
IMyShipConnector Р=з(MyShipConnectorStatus.Connectable);if(Р==null) Р=з(MyShipConnectorStatus.Connected);if(Р!=null){if(Math.Round(ğ,2)<=0.20) Δο++;else Δο=0;if(Δο>=5){if(Μ==Κ.ή&&(Ο.λ||ą)&&Vector3.Distance(Ο.Ε,Р.GetPosition())>5){Ζ.ι=Ϋ.WorldMatrix.Forward;Ζ.Ђ=Ϋ.WorldMatrix.Left;
Ζ.θ=Ϋ.WorldMatrix.Down;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.Ε=Р.GetPosition();Ζ.λ=true;Ζ.п=Р.Position;}else{Ο.ι=Ϋ.WorldMatrix.Forward;Ο.Ђ=Ϋ.WorldMatrix.Left;Ο.θ=Ϋ.WorldMatrix.Down;Ο.φ=Ϋ.GetNaturalGravity();Ο.Ε=Р.GetPosition();Ο.λ=true;Ο.п=Р.Position;}}}double ΔЈ=-1;if(Ψ.Count>0) ΔЈ=Vector3.Distance(δ,Ψ.Last().Ε);
double ΛΞ=Math.Max(1.5,Math.Pow(ğ/100.0,2));double ΗІ=Math.Max(ğ*ΛΞ,2);Θμ=ΗІ;if((ΔЈ==-1)||ΔЈ>=ΗІ){ά έ=new ά(δ,Βξ,ΑΔ,ΑΧ,Ϋ.GetNaturalGravity());έ.Θχ(Ą,ă);Ψ.Add(έ);}}ά ь(){if(Μ!=Κ.ή) return Ǣ!=null?Ǣ:Ο;return ΗΘ;}void ź(ά α,Ρ І){ΗΘ=α;if(ε==Ρ.И) Ǐ=І;}bool Ƞ(ά α,float ϋ,bool Λθ,out Vector3 γ)
{if(Λθ){Vector3I Δϊ=new Vector3I((int) α.п.X,(int) α.п.Y,(int) α.п.Z);IMySlimBlock Ω=Me.CubeGrid.GetCubeBlock(Δϊ);if(Ω==null||!(Ω.FatBlock is IMyShipConnector)){γ=new Vector3();return false;}Vector3 ΚΦ=х(Ϋ,Ω.FatBlock.GetPosition()-δ);Vector3 ΚΥ=х(Ϋ,Ω.FatBlock.WorldMatrix.Forward);γ=α.Ε-ΔΛ(α.ι,α.θ*-1,ΚΦ)-ΔΛ(α.ι,α.θ*-1,ΚΥ)*ϋ;
return true;}else{γ=α.Ε;return true;}}Vector3 ĉ(int ΚΣ,int ƛ,bool ΛΑ){if(!ΛΑ&&Ηα) return Δΰ;float ΚΤ=((Ќ-1f)/2f)-ΚΣ;float ΛΔ=((Э-1f)/2f)-ƛ;Δΰ=Ζ.Ε+Ζ.Ђ*ΚΤ*ȁ+Ĝ*-1*ΛΔ*ț;Ηα=true;return Δΰ;}Vector3 Γι(Vector3 ΛΣ,float Δλ){return ΛΣ+(ǒ*Δλ);}ζ Γί(){float Ы=-1;ζ ί=ζ.ȝ;
if(Μ!=Κ.ή){Ζέ();if(ε!=Ρ.ю){Vector3 П=ŋ(ǒ,Ĝ*-1,δ-Ζ.Ε);if(Math.Abs(П.X)<=(float)(Ќ*ȁ)/2f&&Math.Abs(П.Y)<=(float)(Э*ț)/2f){if(П.Z<=-1&&П.Z>=-Ť*2) return ζ.Αψ;if(П.Z>-1&&П.Z<μ*2) return ζ.ȝ;}if(Ħ(Ζ.Ε,ref Ы)) ί=ζ.ȝ;}if(Ο.λ){if(Ħ(Ο.Ε,ref Ы)) ί=ζ.Ȝ;for(int i=0;i<Ψ.Count;i++)
if(Ħ(Ψ[i].Ε,ref Ы)) ί=ζ.ċ;if(Vector3.Distance(δ,Ο.Ε)<dockDist*μ) ί=ζ.Ȝ;if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null)
ί=ζ.Ȝ;}}else{Vector3 γ=new Vector3();IMyShipConnector Ȭ=з(MyShipConnectorStatus.Connected);if(Ο.λ){if(Ħ(Ο.Ε,ref Ы)) ί=ζ.Ȝ;if(Ƞ(Ο,dockDist,true,out γ))
if(Ħ(γ,ref Ы)) ί=ζ.Ȝ;if(Ȭ!=null&&Vector3.Distance(Ȭ.GetPosition(),Ο.Ε)<5)
return ζ.ΓΤ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ο.Ε)>dockDist*μ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*μ)
if(Ħ(Ψ[i].Ε,ref Ы)) ί=ζ.ċ;if(Ζ.λ){if(Ħ(Ζ.Ε,ref Ы)) ί=ζ.ȝ;if(Ƞ(Ζ,dockDist,true,out γ))
if(Ħ(γ,ref Ы)) ί=ζ.ȝ;if(Ȭ!=null&&Vector3.Distance(Ȭ.GetPosition(),Ζ.Ε)<5)
return ζ.ΓΥ;}}return ί;}bool Ħ(Vector3 γ,ref float Ы){float ϋ=Vector3.Distance(γ,δ);if(ϋ<Ы||Ы==-1){Ы=ϋ;return true;}return false;}void ΔΕ(){if(А>0){Λ="Setup error! Can't start";return;}if(Μ==Κ.ή){ħ();return;}Ζ.Ε=δ;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.ι=Βξ;Ζ.θ=ΑΔ;Ζ.Ђ=ΑΧ;
ǒ=ǣ.WorldMatrix.Forward;Ĝ=Ζ.θ;if(ǒ==Ϋ.WorldMatrix.Down) Ĝ=Ϋ.WorldMatrix.Backward;ƿ(true,true);Τ(Α.ȟ);ΔΔ();}void ƿ(bool ǵ,bool ΛΒ){if(ε==Ρ.ю&&!ǵ) return;bool ΚЁ=ǵ||ε==Ρ.Γα||Ќ!=ё||Э!=ѐ||ř!=Ȗ;if(ΚЁ){if(ε!=Ρ.ю){ε=Ρ.Αϋ;ĉ(Ё,ώ,ΛΒ);Λ="Job changed, lost progress";}ř=Ȗ;
Ќ=ё;Э=ѐ;ώ=0;Ё=0;г="";Г=-1;Ŝ=0;Ě=0;й=0;б=0;ΔΜ(true);}}void ΓЊ(){и(δ,0);Ǜ(Ą,true);}void ΔΔ(){ȥ(ц.ΒΕ);ΓЊ();ƻ(ȳ,false);ȅ(Εβ);ε=Ρ.И;ΕЄ(true);Ǐ=ε;ν(Σ.ƶ);Ιν();Βΰ=true;Βς=0;for(int i=ǁ.Count-1;i>=0;i--)
if(ƴ(ǁ[i],false)) Βς++;if(Βς>0) Λ="Started with damage";}void Є(){if(ε==Ρ.И){ε=Ρ.Ÿ;Λ="Job paused";}Τ(Α.э);Ǐ=ε;ΒΘ(false,0,0,0,0);ΔΘ();ΕϏ(new Vector3(),false);Ζσ();ΓА(ChargeMode.Auto);ΔΑ(false);Ζκ(true);Č(Α.э);Ō(false,false,0,0);Ǜ(ω,false);Ǜ(ў,true);ȅ(true);ΒЋ=false;Αύ();
ΑΓ=false;ΓЌ();Βΰ=false;Αε=false;Αξ=false;if(ύ!=Σ.Main&&ύ!=Σ.Αω&&ύ!=Σ.ΓΠ&&ύ!=Σ.ΒΙ)
ν(Σ.Main);}void ħ(){ζ Ю=Γί();if(Μ==Κ.ή){if(!Ζ.λ||!Ο.λ) return;ΔΔ();bool Δψ=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(ƞ==Ρ.Ŭ) Δψ=true;if(ƞ==Ρ.ŭ) Δψ=false;if(Δψ){ź(Ο,Ρ.Ŭ);switch(Ю){case ζ.ΓΤ:Τ(Α.ĭ);break;case ζ.ċ:Τ(Α.ф);break;case ζ.Ȝ:Τ(Α.ȃ);break;default:Τ(Α.ǰ);break;
}}else{ź(Ζ,Ρ.ŭ);switch(Ю){case ζ.ΓΥ:Τ(Α.ĭ);break;case ζ.ȝ:Τ(Α.ȃ);break;case ζ.ċ:Τ(Α.ф);break;default:Τ(Α.ǰ);break;}}}else{if(ε!=Ρ.Ÿ&&ε!=Ρ.Αϋ) return;bool Λκ=ε==Ρ.Αϋ;ΔΔ();bool ΔϏ=Ƶ(false)&&Ο.λ;switch(Ю){case ζ.ȝ:Τ(ΔϏ?Α.ĩ:Α.ȟ);break;case ζ.ċ:Τ(ΔϏ?Α.Й:Α.ф);break;case ζ.Ȝ:Τ(ΔϏ?Α.ȃ:Α.ȧ);break;
case ζ.Αψ:{if(Ě!=б||Λκ) Τ(Α.Ȥ);else Τ(Α.ŷ);}break;default:break;}}}void ň(){if(ε==Ρ.ю&&!Ο.λ) return;if(Μ==Κ.ή&&(!Ζ.λ||!Ο.λ)) return;Λ="Move to job";ζ Ю=Γί();if(Μ==Κ.ή){ź(Ζ,Ρ.ŭ);switch(Ю){case ζ.ȝ:Τ(Α.ȃ);break;case ζ.ċ:Τ(Α.ф);break;case ζ.ΓΥ:return;default:Τ(Α.ǰ);break;
}Č(Α.ĭ);}else{switch(Ю){case ζ.ȝ:Τ(Α.ф);break;case ζ.ċ:Τ(Α.ф);break;case ζ.Ȝ:Τ(Α.ȧ);break;case ζ.Αψ:Τ(Α.Ȥ);break;default:break;}if(ε==Ρ.ю) Č(Α.ф);else Č(Α.ĥ);ΒЋ=true;}ΓЊ();ν(Σ.ΔΗ);ƻ(ȳ,false);Ǐ=Ρ.ΑЎ;}void ΑЃ(){if(!Ο.λ) return;Λ="Move home";ζ Ю=Γί();if(Μ==Κ.ή){ź(Ο,Ρ.Ŭ);switch(Ю)
{case ζ.ċ:Τ(Α.Й);break;case ζ.Ȝ:Τ(Α.ȃ);break;case ζ.ΓΤ:return;default:Τ(Α.ǰ);break;}Č(Α.ĭ);}else{if(з(MyShipConnectorStatus.Connected)!=null) return;if(з(MyShipConnectorStatus.Connectable)!=null){Τ(Α.ņ);Č(Α.ȧ);return;}switch(Ю){case ζ.ȝ:Τ(Α.ĩ);break;case ζ.ċ:Τ(Α.Й);break;case ζ.Ȝ:Τ(Α.Й);break;
case ζ.Αψ:Τ(Α.ȣ);break;default:break;}Č(Α.ȧ);}ΓЊ();ν(Σ.ΔΗ);ƻ(ȳ,false);Ǐ=Ρ.ΑЍ;}void Τ(Α ΕΛ){if(ΕΛ==Α.э) Αθ=Α.э;if(Αθ!=Α.э&&Ϊ==Αθ&&ΕΛ!=Αθ){Є();return;}Д=true;Ϊ=ΕΛ;}void Č(Α І){this.Αθ=І;}ǯ Λφ(int ΑΡ,bool ǽ){if(ǽ){Ǧ=null;Ё=0;ώ=0;}if(Ȗ==я.ƾ){int Ş=ΑΡ+1;
ώ=(int) Math.Floor(К(ΑΡ,Ќ));if(ώ%2==0) Ё=ΑΡ-(ώ*Ќ);else Ё=Ќ-1-(ΑΡ-(ώ*Ќ));if(ώ>=Э) return ǯ.Αχ;else return ǯ.ΑЅ;}else if(Ȗ==я.ΕІ){if(Ǧ==null) Ǧ=new int[]{0,-1,0,0};int ΑΞ=(int) Math.Ceiling(Ќ/2f);int ΑΝ=(int) Math.Ceiling(Э/2f);int Δύ=(int) Math.Floor(Ќ/2f);
int Δό=(int) Math.Floor(Э/2f);int ΑΣ=0;while(Ǧ[2]<Math.Pow(Math.Max(Ќ,Э),2)){if(ΑΣ>200) return ǯ.ΘЌ;ΑΣ++;Ǧ[2]++;if(-ΑΞ<Ё&&Ё<=Δύ&&-ΑΝ<ώ&&ώ<=Δό){if(Ǧ[3]==ΑΡ){this.Ё=Ё-1+ΑΞ;this.ώ=ώ-1+ΑΝ;return ǯ.ΑЅ;}Ǧ[3]++;}if(Ё==ώ||(Ё<0&&Ё==-ώ)||(Ё>0&&Ё==1-ώ))
{int ΛΦ=Ǧ[0];Ǧ[0]=-Ǧ[1];Ǧ[1]=ΛΦ;}Ё+=Ǧ[0];ώ+=Ǧ[1];}}return ǯ.Αχ;}void ΙЌ(){if(Ϊ==Α.ȟ){if(Д){Е=0;if(Ě!=б) Ŝ=0;Ě=б;}if(Е==0){int Ǌ=Θύ(б);if(Ǌ<0){if(!Αό()){Τ(Α.ȣ);return;}if(!Δη||(Ǌ==-2)!=Βά){Δη=true;Βά=Ǌ==-2;ΗΕ=!Βά?δ:Ζ.Ε-ǒ*μ+
Ζ.Ђ*(Ќ*ȁ/2+ΒЌ/2+μ/2);}и(ΗΕ,10);Λ=Βά?"Waiting for a free hole":"Waiting for the controller";return;}Δη=false;if(Ǌ!=б){б=Ǌ;Ŝ=0;Ě=б;}ǯ ί=Ιφ(б);if(ί==ǯ.Αχ){ε=Ρ.Γα;Λ="Job done";if(ŀ&&Ο.λ){Τ(Α.ĩ);Č(Α.ȧ);Ǐ=Ρ.ΑЍ;}else{Τ(Α.ű);Č(Α.ĥ);Ǐ=Ρ.ΑЎ;}return;
}if(ί==ǯ.ΑЅ){Е=1;Ǜ(ω,true);є=ĉ(Ё,ώ,true);и(є,10);л(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}}else{if(Г>=0&&(Ζά()?ȋ>=0&&ȋ!=Г:Γπ(Г,true))){Е=0;return;}if(в<wpReachedDist){Τ(Α.ŷ);return;}}}if(Ϊ==Α.ŷ){if(Д){Ǜ(ω,true);ȅ(false);є=ĉ(Ё,ώ,false);и(Γι(є,0),0);л(Ζ.θ,Ζ.ι,Ζ.Ђ,false);
Е=1;ѓ=0;у=0;ΒΥ=0;ΔЊ=-1;Ť=о;ΔЃ=true;}if(!Αό()){Τ(Α.ȣ);return;}if(Ƶ(true)){ΗЏ=ć("","ORE",Щ.Ń);if((д==В.Γβ||д==В.Αώ||
д==В.Γρ||д==В.ΑЈ)&&Μ!=Κ.ǭ)
Τ(Α.Ň);else if((д==В.ΕЊ||д==В.Γά)&&Μ!=Κ.ǭ)
Τ(Α.ΔΝ);else
Τ(Α.ȣ);return;}й=Vector3.Distance(δ,є);if(й>Ŝ){Ŝ=й;ΔЃ=false;}if(Μ==Κ.ǭ&&ΖΖ()==MyDetectedEntityType.SmallGrid)
у+=2;else у-=2;у=Math.Max(100,Math.Min(400,у));if(Е>0&&Е<у){if(й>ѓ){if(у>150) ѓ=й;else ѓ=(float) Math.Ceiling(й);Е=1;}else Е++;}else{if(Е>0){Λ="Ship stuck! Retrying";ѓ=й;Е=0;Ō(false,true,0,μ*sensorRange);}и(Γι(є,Math.Max(0,ѓ-μ)),Γμ(false));if(в<=wpReachedDist/2){Е=1;ѓ=0;}
return;}Ō(false,true,μ*sensorRange,0);Vector3 ΚΘ=є+ǒ*й;bool Ηπ=false;if(Vector3.Distance(ΚΘ,δ)>0.3f){Vector3 ΚΪ=є+ǒ*(й+0.1f);и(ΚΪ,4);Ηπ=true;}else{float Ǿ=Γμ(true);Vector3 ΗΜ=Γι(є,Math.Max(о+1,й+1));Ȅ(true,false,false,ΗΜ,ΗΜ-є,Ǿ,Ǿ);}bool Δμ=false;if(џ==ǘ.ΓΧ||џ==ǘ.ΓΦ)
{if(!Ηπ){float ΕΞ=0;foreach(IMyTerminalBlock Ț in ω)
ΕΞ+=ΘІ(Ț,"","",џ==ǘ.ΓΦ?new string[]{"STONE"}:null);if(ΕΞ>ΔЊ||й<о||ΔЃ){ΔΥ=0;ΒΥ=й;Ť=(int)(Math.Max(Ť,ΒΥ)+μ/2);}else{Δμ=й-ΒΥ>2&&ΔΥ>=20;ΔΥ++;}ΔЊ=ΕΞ;}}else Δμ=й>=Ť;if(Ě!=б){Τ(Α.Ȥ);й=0;return;}if(Δμ){б++;Τ(Α.Ȥ);й=0;return;}}if(Ϊ==Α.Ȥ||Ϊ==Α.ȣ||Ϊ==Α.Ň)
{if(Д){є=ĉ(Ё,ώ,false);л(Ζ.θ,Ζ.ι,Ζ.Ђ,false);Ǜ(ω,Ɔ);ȅ(false);ѓ=Vector3.Distance(δ,є);Ō(false,true,0,μ*sensorRange);}и(є,Γμ(false));if(Vector3.Distance(δ,є)>=ѓ+5){Ǜ(ω,false);ȅ(true);Λ="Can´t return!";}if(в<wpReachedDist){if(Ϊ==Α.Ȥ&&ΒЋ) Τ(Α.ű);if(Ϊ==Α.Ȥ) Τ(Α.ȟ);
if(Ϊ==Α.Ň) Τ(Α.ΑЂ);if(Ϊ==Α.ȣ){if(Ο.λ) Τ(Α.ĩ);else{Є();ň();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Α.Й||Ϊ==Α.ф){if(Ϊ==Α.ф&&ε==Ρ.И&&Μ!=Κ.ή){if(!Αό()||Ƶ(true)){Τ(Α.Й);return;}}bool ΕΥ=false;bool ΓΚ=false;bool ΖЁ=false;float ƀ=0;bool ΗЎ=false;ά έ=null;
if(Д){if(Ϊ==Α.Й||Μ==Κ.ή){if(Μ!=Κ.ή){if(č(1)) ΓΣ();else{Ǣ=null;ā=0;ΑΓ=!ΓΫ(1);}}ά α=ь();ξ=new ΓЉ();ξ.η=α;ξ.ΑΑ=followPathDock*μ;ξ.Ετ=useDockDirectionDist*μ;ξ.ƀ=10;ξ.Ƥ.Add(α.Ε);Vector3 ſ=new Vector3();if(Ƞ(α,dockDist*μ,true,out ſ))
ξ.Ƥ.Add(ſ);else ξ.ΑΑ*=1.5f;if(Μ==Κ.ή){if(α==Ο) ξ.Βλ=Ζ.Ε;if(α==Ζ) ξ.Βλ=Ο.Ε;ξ.Δυ=dockDist*μ*1.1f;}}else if(Ϊ==Α.ф){ξ=new ΓЉ();ξ.η=Ζ;ξ.ΑΑ=followPathJob*μ;ξ.Ετ=useJobDirectionDist*μ;ξ.ƀ=10;ξ.Βλ=Ο.Ε;ξ.Δυ=dockDist*μ*1.1f;ξ.Ƥ.Add(Ζ.Ε);if(ε==Ρ.ю){if(!Ο.λ||Ψ.Count==0){Є();return;}
float Κΰ=Vector3.Distance(Ψ.First().Ε,Ο.Ε);float Κα=Vector3.Distance(Ψ.Last().Ε,Ο.Ε);if(Κΰ<Κα) ξ.η=Ψ.Last();else ξ.η=Ψ.First();}}Αί=new Vector3();ΗЎ=!ΙΨ(δ);Ǜ(ω,false);ȅ(true);к=-1;double Ȋ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ξ.Βλ)<=ξ.Δυ) continue;
double ϋ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȋ==-1||ϋ<Ȋ){к=i;Ȋ=ϋ;}}Ȕ=ΙΗ(ξ.η.Ε,к);Ő=null;}if(Ϊ==Α.Й&&Μ!=Κ.ή&&!Д&&!ƽ()){ά ΔΪ=ь();ΑΓ=!ΓΫ(1);if(ь()!=ΔΪ){Τ(Α.Й);return;}}if(Ϊ==Α.Й&&Μ!=Κ.ή&&!Д&&č(1)&&ΓΣ()){Τ(Α.Й);return;}bool Ηξ=false;if(Ϊ==Α.Й&&!Д&&к>=0&&к<Ψ.Count)
{Vector3 ǌ=Ψ[к].Ε+ΖΙ(к,Ȕ);int ΕΜ=к+Ȕ;Vector3 Ş=Ȕ!=0&&ΕΜ>=0&&ΕΜ<Ψ.Count?Ψ[ΕΜ].Ε:ξ.η.Ε;if(ΙΡ(ǌ,Ş)){if(!Βδ){Βδ=true;ΗΡ=ŵ(ǌ)?δ:ǌ;}и(ΗΡ,0);л(ξ.η.θ,ξ.η.ι,ξ.η.Ђ,false);Λ="Waiting for dock clearance";return;}Βδ=false;Ηξ=č(1)&&(ŵ(δ)||ŵ(Ş));}Θψ(Ψ,Ȕ,ξ.Ƥ,ξ.ΑΑ,Д,ref Е);
for(int i=0;i<ξ.Ƥ.Count;i++){float ϋ=Vector3.Distance(δ,ξ.Ƥ[i]);if(ϋ<=ξ.ΑΑ) ΕΥ=true;if(ϋ<=ξ.Ετ) ΓΚ=true;}if(Ηξ) ΓΚ=true;if(ΓΚ) ƀ=ξ.ƀ;float Λϋ=Ő!=null?Ő.Ш:ğ;float Λπ=(float) Math.Max(ğ*0.1f*μ,wpReachedDist);if((в<Λπ)||Д){if(!Д) к+=Ȕ;if(Ȕ==0||к>Ψ.Count-1||к<0)
ΕΥ=true;else{Εχ=Ȕ>0?Ψ.Count-1-к:к;έ=Ψ[к];Ő=έ;if(к>=1&&к<Ψ.Count-1)
Αί=έ.Ε-Ψ[к-Ȕ].Ε;else Ő=null;Ѓ=έ.Ε+ΖΙ(к,Ȕ);ΖЁ=true;}}if(ΓΚ)
л(ξ.η.θ,ξ.η.ι,ξ.η.Ђ,false);else if(ΗЎ)
ΓΡ(ξ.η.θ,10,true);else if(ΖЁ&&έ!=null){if(Ȕ>0) л(έ.θ,έ.ι,έ.Ђ,90,false);else л(έ.θ,-έ.ι,-έ.Ђ,90,false);}Ȅ(true,false,true,Ѓ,Αί,Ő==null?0:Ő.Ш,ƀ);if(ΕΥ){Εχ=0;if(Ϊ==Α.Й&&!Θϋ()){и(Ѓ,0);return;}if(Ϊ==Α.Й&&ΑΓ){и(Ѓ,0);Λ="Waiting for a free dock";return;}
if(Ϊ==Α.Й||Μ==Κ.ή){Τ(Α.ȃ);return;}if(Ϊ==Α.ф&&ΒЋ){Τ(Α.ű);return;}if(Ϊ==Α.ф){Τ(Α.ȟ);return;}}}if(Ϊ==Α.ȃ||Ϊ==Α.ƺ){if(Μ!=Κ.ή&&ƽ()){if(!č(1)){Τ(Α.Й);return;}}else if(Μ!=Κ.ή){ά ΔΪ=ь();if(!ΓΫ(2)||ь()!=ΔΪ){Τ(Α.Й);return;}}ά α=ь();if(Д){if(!Ƞ(α,dockDist*μ,true,out Ѓ))
{ȥ(ц.ư);Є();return;}и(Ѓ,0);ΓΡ(α.θ,90,true);}if(в<followPathDock*μ&&в!=-1){и(Ѓ,10);л(α.θ,α.ι,α.Ђ,false);}if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null){Τ(Α.ņ);return;}if(в<wpReachedDist/2&&в!=-1){Τ(Α.ł);return;}}if(Ϊ==Α.ł||Ϊ==Α.ĥ)
{if(Д){if(Ϊ==Α.ł){ά α=ь();if(!Ƞ(α,dockDist*μ,true,out Ѓ)){ȥ(ц.ư);Є();return;}Ȅ(true,true,false,Ѓ,0);л(α.θ,α.ι,α.Ђ,10,false);}if(Ϊ==Α.ĥ){л(Ζ.θ,Ζ.ι,Ζ.Ђ,0.5f,false);Ѓ=Ζ.Ε;Ȅ(true,true,false,Ѓ,0);}}if(Βτ){ΒΘ(false,0,0,0,0);if(Ϊ==Α.ł) Τ(Α.ņ);if(Ϊ==Α.ĥ) Є();return;
}}if(Ϊ==Α.ņ){if(з(MyShipConnectorStatus.Connected)!=null){if(Μ==Κ.ή) Τ(Α.ĭ);else Τ(Α.ȧ);return;}ά α=ь();if(Д){у=0;Κβ=DateTime.Now;Е=0;л(α.θ,α.ι,α.Ђ,false);}Vector3I Δϊ=new Vector3I((int) α.п.X,(int) α.п.Y,(int) α.п.Z);IMySlimBlock ΗΒ=Me.CubeGrid.GetCubeBlock(Δϊ);float ΗΥ=dockingSpeed;
float Κό=dockingSpeed*5;float ΖϏ=Math.Max(1.5f,Math.Min(5f,μ*0.15f));if(!Ƞ(α,0,true,out Ѓ)||!Ƞ(α,ΖϏ,true,out Αί)||
ΗΒ==null||!ΗΒ.FatBlock.IsFunctional){ȥ(ц.ư);Є();return;}if(у==1||(Vector3.Distance(δ,Ѓ)<=ΖϏ*1.1f&&!Д))
у=1;else{Vector3 Λί=х(Ϋ,Αί-δ);Vector3 Δω=х(Ϋ,Ϋ.GetNaturalGravity());float ΛΕ=Αϊ(Λί,Δω,null);ΗΥ=Math.Min(Κό,ΛΕ);}Ȅ(true,false,false,Ѓ,Ѓ-δ,dockingSpeed,ΗΥ);if(Д) ѓ=(float) в;IMyShipConnector Δζ=з(MyShipConnectorStatus.Connectable);if(Δζ!=null){Ȅ(false,false,false,Ѓ,0);
if(Е>0) Е=0;Е--;if(Е<-5){Δζ.Connect();if(Δζ.Status==MyShipConnectorStatus.Connected){if(Μ==Κ.ή) Τ(Α.ĭ);else Τ(Α.ȧ);ΔΘ();ƻ(ȳ,true);return;}}}else{float ΗΠ=(float) Math.Round(в,1);if(ΗΠ<ѓ){Е=-1;ѓ=ΗΠ;}else Е++;if(Е>20){Τ(Α.ƺ);return;}}}if(Ϊ==Α.ȧ||Ϊ==Α.ĭ||
Ϊ==Α.ΒΟ||Ϊ==Α.ΑЀ||Ϊ==Α.Ņ){bool ΔΣ=false,ΔΤ=false;if(Μ==Κ.ή){if(ь()==Ο) ΔΣ=true;else if(ь()==Ζ) ΔΤ=true;}if(Д){Αε=false;if(з(MyShipConnectorStatus.Connected)==null){Τ(Α.ǰ);return;}ΔΘ();if(ΔΣ) ΒΜ(Ї.Ǵ);if(ΔΤ) ΒΜ(Ј.Ǵ);Ũ=false;Ď=false;Ė=false;Απ=false;}if(з(MyShipConnectorStatus.Connected)==null){Є();ȥ(ц.ΖΧ);return;}
if(ε!=Ρ.И||ǐ==-1||ȇ==њ.None) Ď=true;else if(ȩ>=100f) Ď=true;else if(ȩ<=99f) Ď=false;if(ε!=Ρ.И||Ǒ==-1||ǟ.Count==0) Ė=true;else if(ė>=100f) Ė=true;else if(ė<=99) Ė=false;if(ε!=Ρ.И||р==-1||ļ.Count==0) Απ=true;else Απ=ũ>=р;ż ј=null;if(ΔΣ) ј=Ї;
if(ΔΤ) ј=Ј;if(ј!=null&&(ј.Т==ρ.ΒΑ||ј.Т==ρ.ΑБ)) Ď=true;if(ј!=null&&(ј.Т==ρ.ΓЁ)) if(!Ũ) Ď=false;if(ј!=null&&(ј.Т==ρ.ΓЅ||ј.Т==ρ.ΓЃ)) Ė=true;if(ј!=null&&(ј.Т==ρ.ΓЄ)) if(!Ũ) Ė=false;if(Ę){ChargeMode ΖЋ=Ď?ChargeMode.Auto:ChargeMode.Recharge;if(ј!=null&&(ј.Т==ρ.ΑБ||ј.Т==ρ.ΒΑ))
ΖЋ=ChargeMode.Discharge;ΓА(ΖЋ);ΔΑ(!Ė);}if(!Ũ){if(Μ==Κ.ή)
Ũ=ε!=Ρ.И||Θό(Д,true)||Αξ;else
Ũ=ε!=Ρ.И||ΖΨ();}else{if(!Ď) Τ(Α.Ņ);if(!Ė) Τ(Α.ΑЀ);if(!Απ) Τ(Α.ΒΟ);Д=false;}if(Ũ&&Ď&&Ė&&Απ){ΓА(ChargeMode.Auto);ΔΑ(false);if(ε==Ρ.И){if(Μ==Κ.ή){if(ь()==Ο) ΒΜ(Ї.ǹ);else if(ь()==Ζ) ΒΜ(Ј.ǹ);if(ь()==Ο) ź(Ζ,Ρ.ŭ);else ź(Ο,Ρ.Ŭ);}}Τ(Α.ǰ);return;}}if(Ϊ==Α.ǰ){if(Ασ){if(!ΕЈ()) return;
Ασ=false;Τ(Α.ǰ);return;}if(Д){if(!ΕЈ()){Ασ=true;return;}IMyShipConnector Ȭ=з(MyShipConnectorStatus.Connected);if(Ȭ==null){Τ(Α.ф);return;}IMyShipConnector κ=Ȭ.OtherConnector;Ζι(Ȭ,false);ƻ(ȳ,false);ά α=null;ά Ǎ=Μ==Κ.ή?Ο:ь();if(Vector3.Distance(Ȭ.GetPosition(),Ǎ.Ε)<5f&&Ο.λ) α=Ǎ;
if(Vector3.Distance(Ȭ.GetPosition(),Ζ.Ε)<5f&&Ζ.λ) α=Ζ;if(α!=null){if(!Ƞ(α,dockDist*μ,true,out Ѓ)){ȥ(ц.ư);Є();return;}и(Ѓ,5);л(α.θ,α.ι,α.Ђ,false);}else и(δ+κ.WorldMatrix.Forward*dockDist*μ,5);if(ε==Ρ.И) ȥ(ц.ΒΕ);}if(в<wpReachedDist){Ǜ(ў,true);Τ(Α.ф);return;}}if(Ϊ==Α.ű)
{if(Д){ȅ(true);Ǜ(ω,false);Ѓ=Ζ.Ε;и(Ѓ,20);л(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(в<wpReachedDist/2){Τ(Α.ĥ);return;}}if(Ϊ==Α.ĩ){if(Д){ȅ(true);Ǜ(ω,false);int Ы=-1;double Ȋ=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϋ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȋ==-1||ϋ<Ȋ){Ы=i;Ȋ=ϋ;}}
if(Ы==-1){Τ(Α.Й);return;}Ѓ=Ψ[Ы].Ε;и(Ѓ,10);л(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(в<wpReachedDist){Τ(Α.Й);return;}}if(Ϊ==Α.ΑЂ){bool Ѐ=false;if(Д){ȅ(true);if((д==В.Γβ||д==В.Αώ)&&ΑЊ()&&
ů(ǒ,Ϋ.GetNaturalGravity())<25&&Ќ>=2&&Э>=2){Vector3 ΑΕ=δ;if(Ё>0&&ώ<Э-1) ΑΕ=ĉ(Ё-1,ώ+1,true);else if(Ё<Ќ-1&&ώ<Э-1) ΑΕ=ĉ(Ё+1,ώ+1,true);else if(Ё<Ќ-1&&ώ>0) ΑΕ=ĉ(Ё+1,ώ-1,true);else if(Ё>0&&ώ>0) ΑΕ=ĉ(Ё-1,ώ-1,true);else Ѐ=true;
if(!Ѐ) и(ΑΕ,10);}else Ѐ=true;}if(в<wpReachedDist/2) Ѐ=true;if(Ѐ){Τ(Α.ΒΞ);return;}}if(Ϊ==Α.ΔΝ||Ϊ==Α.ΒΞ){if(Д){Ȅ(true,true,false,δ,0);Ǜ(ω,false);ȅ(true);Е=-1;у=д==В.Γρ||д==В.ΑЈ?0:-1;}bool Ŕ=!Αό();int ΓΓ=ć("STONE","ORE",Щ.Ń);if(д==В.Γά||д==В.ΑЈ||д==В.Αώ)
ΓΓ+=ć("ICE","ORE",Щ.Ń);bool Κμ=ΓΓ>0;bool Δτ=false;if(у>=0){float Ž=(float) Math.Sin(ΕЌ(у))*ȁ/3f;float ΚΚ=(float) Math.Cos(ΕЌ(у))*ț/3f;Vector3 ΚΡ=ĉ(Ё,ώ,true)+ΔΛ(ǒ,Ĝ*-1,new Vector3(Ž,ΚΚ,0));и(ΚΡ,0.3f);if(в<Math.Min(ȁ,ț)/10f) у+=5f;if(у>=360) у=0;}if(Е==-1||ΓΓ<Е){Е=ΓΓ;ѓ=0;}
else{ѓ++;if(ѓ>50) Δτ=true;}if(!Κμ||Ŕ||Δτ){if(!Ŕ){int Λδ=ć("","ORE",Щ.Ń);if(Ƶ(true)) Ŕ=true;else if(100-(К(Λδ,ΗЏ)*100)<minEjection) Ŕ=true;else ȥ(ц.ΒΕ);}if(Δτ&&Ŕ) Λ="Ejection failed";if(Ϊ==Α.ΒΞ){if(Ŕ){if(Ο.λ) Τ(Α.ĩ);else{Є();ň();Λ="Can´t return, no dock found";}}
else Τ(Α.ȟ);}else if(Ŕ) Τ(Α.ȣ);else Τ(Α.ŷ);return;}}Д=false;}void Θψ(List<ά>Ł,int ς,List<Vector3>Αλ,float ȗ,bool ǽ,ref int ǲ){if(ǽ){for(int i=0;i<Ł.Count;i++) Ł[i].Ш=0;ǲ=-1;return;}if(ς==0) return;int ǧ=ς*-1;if(ǲ==-1) ǲ=ǧ>0?1:Ł.Count-2;int ΑΣ=0;while(ǲ>=1&&ǲ<Ł.Count-1)
{if(ΑΣ>50) return;ΑΣ++;try{if((ǧ<0&&ǲ>=1)||(ǧ>0&&ǲ<=Ł.Count-2)){ά ο=Ł[ǲ];bool ΗЋ=false;for(int j=0;j<Αλ.Count;j++)
if(Vector3.Distance(ο.Ε,Αλ[j])<=ȗ){ΗЋ=true;break;}if(!ΗЋ){ά ΒЇ=Ł[ǲ-ǧ];ά Ş=Ł[ǲ+ǧ];Vector3 Λΰ=ο.Ε-Ş.Ε;Vector3 Εν=ΒЇ.Ε-ο.Ε;Vector3 ΚА=ο.Ε+Vector3.Normalize(Λΰ)*Εν.Length();Vector3 Κί=ΒЇ.Ε-ΚА;Vector3 Βϊ=ŋ(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,Κί);Vector3 Κϋ=ŋ(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,Εν);
Vector3 Βό=ŋ(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,ο.φ);ο.Ш=(float) Math.Sqrt(Math.Pow(ΒЇ.Ш,2)+Math.Pow(Αϊ(-Κϋ,Βό,ο),2));for(int j=0;j<Αλ.Count;j++)
if(Vector3.Distance(ΒЇ.Ε,Αλ[j])<=ȗ){Vector3 Βύ=ŋ(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,Αλ[j]-ο.Ε);float ΛΥ=Αϊ(-Βύ,Βό,ο);ο.Ш=Math.Min(ο.Ш,ΛΥ)/2f;}if(Βϊ.Length()==0) Βϊ=new Vector3(0,0,1);Vector3 Κϊ=ŋ(ο.ι,ο.θ*-1,ο.φ);float ΛΪ=Ʋ(-Βϊ,Κϊ,ο);float ΒΠ=К(ΛΪ,Ŧ);float Εκ=(float) Math.Sqrt(К(Βϊ.Length()*1.0f,0.5f*ΒΠ));
ο.Ш=Math.Min(ο.Ш,(Εν.Length()/Εκ)*Ȇ);}}}catch{return;}ǲ+=ǧ;}ǲ=-1;}void ΔΜ(bool ǽ){if(ǽ){ΑΤ=0;return;}if(Μ==Κ.ή) return;float Ѐ=б*Math.Max(1,о);if(Ě==б) Ѐ+=Math.Min(о,й);float Λγ=Ќ*Э*Math.Max(1,о);ΑΤ=Math.Max(ΑΤ,(float) Math.Min(Ѐ/Λγ*100.0,100));}MyDetectedEntityType ΖΖ()
{try{if(ȡ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γμ(bool ȏ){if(Μ==Κ.ǭ&&ΖΖ()==MyDetectedEntityType.None&&!ƴ(ψ,true))
return fastSpeed;else return ȏ?Ʈ:ƭ;}void ΙЃ(){float ΗЃ=0,ΗΚ=0,Ř=0,ΕΟ=0;for(int i=0;i<Į.Count;i++){IMyBatteryBlock Ǉ=Į[i];if(!ȡ(Ǉ,true)) continue;ΗЃ+=Ǉ.MaxStoredPower;ΗΚ+=Ǉ.CurrentStoredPower;Ř+=Ǉ.CurrentInput;ΕΟ+=Ǉ.CurrentOutput;}ȩ=(float) Math.Round(К(ΗΚ,ΗЃ)*100,1);
if(Ř>=ΕΟ) ȇ=њ.Ņ;else ȇ=њ.ΕЍ;if(Ř==0&&ΕΟ==0||ȩ==100.0) ȇ=њ.э;if(Į.Count==0) ȇ=њ.None;}void ΙЉ(){float ΗΪ=0;for(int i=0;i<ǟ.Count;i++){IMyGasTank ΘΪ=ǟ[i];if(!ȡ(ΘΪ,true)) continue;ΗΪ+=(float) ΘΪ.FilledRatio;}ė=К(ΗΪ,ǟ.Count)*100f;}void ΚΔ(){ũ=0;try{for(int i=0;i<ļ.Count;i++)
{IMyReactor ΕΦ=ļ[i];if(!ȡ(ΕΦ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΦ.GetInventory(0).GetItems(Ћ);float Њ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem Ϗ=Ћ[j];if(Ϗ.Type.SubtypeId.ToUpper()=="URANIUM"&&Ϗ.Type.TypeId.ToUpper().Contains("_INGOT"))
Њ+=(float) Ϗ.Amount;}if(Њ<ũ||i==0){ũ=Њ;ΕΑ=ΕΦ.CustomName;}}}catch(Exception e){ę=e;}}void ΙЄ(){ȓ=0;Ȍ=0;try{for(int i=0;i<Ƅ.Count;i++){IMyTerminalBlock Ω=Ƅ[i];if(!ȡ(Ω,true)) continue;Ȍ+=(float) Ω.GetInventory(0).CurrentVolume;ȓ+=(float) Ω.GetInventory(0).MaxVolume;
}Βω=(float) Math.Min(Math.Round(К(Ȍ,ȓ)*100,1),100.0);}catch(Exception e){ę=e;}}void ΙЊ(){try{Ƿ.Clear();for(int i=0;i<Ƅ.Count;i++){IMyTerminalBlock Ω=Ƅ[i];if(!ȡ(Ω,true)) continue;Εώ(Ω,Щ.Ŵ);}if(д!=В.ΓЀ){for(int i=0;i<ŏ.Count;i++){IMyTerminalBlock Ω=ŏ[i];if(!ȡ(Ω,true)) continue;
Εώ(Ω,Щ.ΙΦ);}}Ιω(Ƿ);}catch(Exception e){ę=e;}}void Θϊ(){if(Βΰ){if(ΖΕ().Count>Βς){Βΰ=false;if(ѕ!=ы.Ƴ){Є();if(ѕ==ы.Γη) ň();if(ѕ==ы.ΒΔ)
if(Ο.λ) ΑЃ();else ň();}Λ="Damage detected";}}}bool Αό(){if(!Βσ) return true;if(ε==Ρ.И){if(ǐ>0&&ȇ!=њ.None)
if(ȩ<=ǐ){Λ="Low energy! Move home";return false;}if(р>0&&ļ.Count>0)
if(ũ<=р){Λ="Low fuel: "+ΕΑ;return false;}if(Ǒ>0&&ǟ.Count>0)
if(ė<=Ǒ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΖΕ(){List<IMyTerminalBlock>ő=new List<IMyTerminalBlock>();for(int i=0;i<ǁ.Count;i++){IMyTerminalBlock Ω=ǁ[i];if(ƴ(Ω,false)) ő.Add(Ω);}return ő;}bool ƴ(IMyTerminalBlock Ω,bool Εδ){return(!ȡ(Ω,Εδ)||!Ω.IsFunctional);
}bool ȡ(IMyTerminalBlock Ω,bool Εδ){if(Ω==null) return false;try{IMyCubeBlock Ηΰ=Me.CubeGrid.GetCubeBlock(Ω.Position).FatBlock;if(Εδ) return Ηΰ==Ω;else return Ηΰ.GetType()==Ω.GetType();}catch{return false;}}float ΘІ(IMyTerminalBlock Ω,String Ǘ,String ΘΠ,String[]ȍ){float ȭ=0;for(int i=0;i<Ω.InventoryCount;i++)
{IMyInventory Ƌ=Ω.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ƌ.GetItems(Ћ);foreach(MyInventoryItem Ϗ in Ћ){if(ȍ!=null&&(ȍ.Contains(Ϗ.Type.TypeId.ToUpper())||ȍ.Contains(Ϗ.Type.SubtypeId.ToUpper())))
continue;if((Ǘ==""||Ϗ.Type.TypeId.ToUpper()==Ǘ)&&(ΘΠ==""||Ϗ.Type.SubtypeId.ToUpper()==ΘΠ))
ȭ+=(float) Ϗ.Amount;}}return ȭ;}м ΘЎ(String ΰ,String υ,Щ Ю,bool ΚΫ){ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ƿ.Count;i++){м Ϗ=Ƿ[i];if(Ϗ.ΰ.ToUpper()==ΰ&&Ϗ.υ.ToUpper()==υ&&(Ϗ.Ə==Ю||Ю==Щ.Ń))
return Ϗ;}м ί=null;if(ΚΫ){ί=new м(ΰ,υ,0,Ю);Ƿ.Add(ί);}return ί;}int ć(String ΰ,String υ,Щ Ю){return ć(ΰ,υ,Ю,null);}int ć(String ΰ,String υ,Щ Ю,String[]ȍ){int ȭ=0;ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ƿ.Count;i++){м Ϗ=Ƿ[i];if(ȍ!=null&&ȍ.Contains(Ϗ.ΰ.ToUpper())) continue;
if((ΰ==""||Ϗ.ΰ.ToUpper()==ΰ)&&(υ==""||Ϗ.υ.ToUpper()==υ)&&(Ϗ.Ə==Ю||Ю==Щ.Ń))
ȭ+=Ϗ.Њ;}return ȭ;}void Εώ(IMyTerminalBlock Ω,Щ Ю){for(int i=0;i<Ω.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ω.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
ΘЎ(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Ю,true).Њ+=(int) Ћ[j].Amount;}}void Ιω(List<м>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){м a=Ѝ[j];м b=Ѝ[j+1];if(a.Њ<b.Њ) Ѝ.Move(j,j+1);}}bool Ƶ(bool Λψ){if(Ĥ&&Μ!=Κ.ή)
if(е!=-1&&Ŧ>=е){Λ="Ship too heavy";return true;}if(Βω>=Ȓ||Αε){Αε=false;Λ="Ship is full";return true;}return false;}bool ΖΨ(){String[]ȍ=null;if(!ģ) ȍ=new string[]{"ICE"};if(Μ==Κ.Ƹ) return ć("","ORE",Щ.Ŵ,ȍ)==0;if(Μ==Κ.ǭ) return ć("","COMPONENT",Щ.Ŵ,ȍ)==0;else return ć("","",Щ.Ŵ,ȍ)==0;
}bool ΙΩ(){List<IMyCockpit>Δε=new List<IMyCockpit>();χ.GetBlocksOfType(Δε,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δε.Count;i++)
if(Δε[i].IsUnderControl) return true;return false;}bool Θό(bool ǽ,bool ΒЎ){IMyShipConnector Р=з(MyShipConnectorStatus.Connected);if(Р==null) return false;if(Vector3.Distance(Ο.Ε,Р.GetPosition())<5)
return ΕЉ(Ї,ǽ,ΒЎ);if(Vector3.Distance(Ζ.Ε,Р.GetPosition())<5)
return ΕЉ(Ј,ǽ,ΒЎ);return false;}bool ΕЉ(ż ȫ,bool ǽ,bool ΒЎ){if(ǽ) ȫ.Ιι();ȫ.ΙБ();bool ї=false;String Ǖ="";switch(ȫ.Т){case ρ.ΓЂ:{Ǖ="Waiting for command";ї=false;break;}case ρ.Ζγ:{Ǖ="Waiting for cargo";ї=Ƶ(true);break;}case ρ.Ζβ:{Ǖ="Unloading";ї=ΖΨ();break;}case ρ.ΓЇ:{ї=true;break;}
case ρ.ΓЁ:{Ǖ="Charging batteries";ї=ȩ>=100f;break;}case ρ.ΒΑ:{Ǖ="Discharging batteries";ї=ȩ<=25f;break;}case ρ.ΑБ:{Ǖ="Discharging batteries";ї=ȩ<=0f;break;}case ρ.ΓЄ:{Ǖ="Filling up hydrogen";ї=ė>=100f;break;}case ρ.ΓЅ:{Ǖ="Unloading hydrogen";ї=ė<=25f;break;}
case ρ.ΓЃ:{Ǖ="Unloading hydrogen";ї=ė<=0f;break;}case ρ.ΓІ:{bool Ην=ΙΩ();if(!Ην) ȫ.ΕΣ=true;ї=ȫ.ΕΣ&&Ην;Ǖ="Waiting for passengers";break;}}if(!ї) ȫ.ΓΪ(true);if(ї&&ȫ.Γξ()){ї=ȫ.ΓΪ(false);Ǖ="Undocking in: "+ΑЄ((int) Math.Max(0,ȫ.ǳ-ȫ.Ĵ));}if(ΒЎ) Θκ=Ǖ;return ї;}void Θπ()
{if(!ƌ) return;if(ω.Count<=1) return;float Θβ=0,Θα=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ț=ω[i];if(ƴ(Ț,true)) continue;Θβ+=(float) Ț.GetInventory(0).MaxVolume;Θα+=(float) Ț.GetInventory(0).CurrentVolume;}float ΖЂ=(float) Math.Round(К(Θα,Θβ),5);for(int Š=0;Š<Math.Max(1,Math.Floor(ω.Count/10f));Š++)
{float ΔЁ=0,ΔБ=0;float Ηζ=0,ΗЀ=0;IMyTerminalBlock ē=null,ĵ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ț=ω[i];if(ƴ(Ț,true)) continue;float ΕΗ=(float) Ț.GetInventory(0).MaxVolume;float Βμ=К((float) Ț.GetInventory(0).CurrentVolume,ΕΗ);if(ē==null||Βμ>ΔЁ){ē=Ț;ΔЁ=Βμ;Ηζ=ΕΗ;}
if(ĵ==null||Βμ<ΔБ){ĵ=Ț;ΔБ=Βμ;ΗЀ=ΕΗ;}}if(ē==null||ĵ==null||ē==ĵ) return;if(checkConveyorSystem&&!ē.GetInventory(0).IsConnectedTo(ĵ.GetInventory(0))){if(ΔΩ>20) Λ="Inventory balancing failed";else ΔΩ++;return;}ΔΩ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
ē.GetInventory(0).GetItems(Ћ);float Ας=0;if(Ћ.Count==0) continue;MyInventoryItem Βν=Ћ[0];String Ηρ=Βν.Type.TypeId+Βν.Type.SubtypeId;if(!Θι.TryGetValue(Ηρ,out Ας)){if(Ιί(ē.GetInventory(0),0,ĵ.GetInventory(0),out Ας))
Θι.Add(Ηρ,Ας);else return;}float Λα=((ΔЁ-ΖЂ)*Ηζ/Ας);float Λβ=((ΖЂ-ΔБ)*ΗЀ/Ας);int Ες=(int) Math.Min(Λβ,Λα);if(Ες<=0) return;if((float) Βν.Amount<Ες)
ē.GetInventory(0).TransferItemTo(ĵ.GetInventory(0),0,null,null,Βν.Amount);else
ē.GetInventory(0).TransferItemTo(ĵ.GetInventory(0),0,null,null,Ες);}}bool Ιί(IMyInventory Αζ,int Β,IMyInventory Κή,out float Θθ){Θθ=0;float ΚΞ=(float) Αζ.CurrentVolume;List<MyInventoryItem>ΔΫ=new List<MyInventoryItem>();Αζ.GetItems(ΔΫ);float ΖІ=0;for(int i=0;i<ΔΫ.Count;i++) ΖІ+=(float) ΔΫ[i].Amount;
Αζ.TransferItemTo(Κή,Β,null,null,1);float Θζ=ΚΞ-(float) Αζ.CurrentVolume;List<MyInventoryItem>ΔΟ=new List<MyInventoryItem>();Αζ.GetItems(ΔΟ);float Ζϋ=0;for(int i=0;i<ΔΟ.Count;i++) Ζϋ+=(float) ΔΟ[i].Amount;if(Θζ==0f||!Γς(0.9999,ΖІ-Ζϋ,1.0001))
return false;Θθ=Θζ;return true;}void ΙЈ(){Vector3 ΑΚ=Ѓ+Ŀ;Vector3 ș=ΑΚ-δ;if(ș.Length()==0) ș=new Vector3(0,0,-1);Vector3 Βύ=х(Ϋ,ș);Vector3 ΘΫ=Vector3.Normalize(Βύ);Vector3 Ǝ=х(Ϋ,Ϋ.GetNaturalGravity());float ΛΟ=ΕΠ>0?Math.Max(0,1-ů(ș,Ηά)/5):0;float Ш=(float) Math.Min((ΕΔ>0?ΕΔ:1000f),Math.Max(Αϊ(-Βύ,Ǝ,null),ΕΠ*ΛΟ));
if(!Θΰ) Ш=0;if(ΘΚ) Ш=Math.Max(0,1-Βή/ΒΡ)*Ш;if(generalSpeedLimit>0) Ш=Math.Min(generalSpeedLimit,Ш);if(Ɨ) Ш=0;if(ǖ>=0) Ш=Math.Min(Ш,ǖ);if(ΘΛ) Ш*=(float) Math.Min(1,К(ș.Length(),wpReachedDist)/2);Vector3 Ηύ=х(Ϋ,Ϋ.GetShipVelocities().LinearVelocity);float ΚΙ=(float)(Math.Max(0,15-ů(-ΘΫ,-Ηύ))/15)*0.85f+0.15f;
Εθ+=Math.Sign(ΚΙ-Εθ)/10f;Vector3 İ=ΘΫ*Ш*Εθ-(Ηύ);Vector3 ΔΧ=Γλ(İ,null);if(ΓΕ&&в>0.1f){İ.X*=ΔΓ(İ.X,ref ΓΖ.X,1f,ΔΧ.X,20);İ.Y*=ΔΓ(İ.Y,ref ΓΖ.Y,1f,ΔΧ.Y,20);İ.Z*=ΔΓ(İ.Z,ref ΓΖ.Z,1f,ΔΧ.Z,20);}else ΓΖ=new Vector3(1,1,1);Αμ=Ŧ*İ-Ǝ*Ŧ;ΕϏ(Αμ,ΓΕ);в=Vector3.Distance(δ,ΑΚ);
}float ΔΓ(float value,ref float І,float ǧ,float ě,float ΕΕ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(І)) І+=Math.Sign(І)*ǧ;else І=value;if(value==0) І=1;float ί=Math.Abs(І);if(ί<ΕΕ||ě==0) return ί;І=Math.Min(ΕΕ,Math.Max(-ΕΕ,І));ί=Math.Abs(ě);return ί;
}void Ζω(){float ǔ=90,ǫ=90,ǥ=90;float Ǿ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȏ,ǡ,ΑΩ,ΔЎ;if(ΔЏ){ȏ=Vector3.Normalize(Ѓ-δ);ΑΩ=х(Ϋ,ȏ);ǡ=х(Ϋ,Εζ);ǔ=ů(ΑΩ,new Vector3(0,-1,0))-90;ǫ=ń(ǡ,new Vector3(-1,0,0),ǡ.Y);
ǥ=ń(ΑΩ,new Vector3(-1,0,0),ΑΩ.Z);}else{ȏ=Εη;ǡ=х(Ϋ,Εζ);ΑΩ=х(Ϋ,Εη);ΔЎ=х(Ϋ,Θά);ǔ=ń(ǡ,new Vector3(0,0,1),ǡ.Y);ǫ=ń(ǡ,new Vector3(-1,0,0),ǡ.Y);ǥ=ń(ΔЎ,new Vector3(0,0,1),ΔЎ.X);}if(Ζύ&&ΑЊ()){Vector3 Δω=Ϋ.GetNaturalGravity();ǡ=х(Ϋ,Δω);ǔ=ń(ǡ,new Vector3(0,0,1),ǡ.Y);
ǫ=ń(ǡ,new Vector3(-1,0,0),ǡ.Y);}if(!Γς(-45,ǫ,45)){ǔ=0;ǥ=0;}if(!Γς(-45,ǥ,45)) ǔ=0;ΒΘ(Δϋ,1,(-ǔ)*Ǿ,(-ǥ)*Ǿ,(-ǫ)*Ǿ);Βή=Math.Max(Math.Abs(ǔ),Math.Max(Math.Abs(ǫ),Math.Abs(ǥ)));Βτ=Βή<=ΒΡ;}void Θω(IMyTerminalBlock ΒЈ){if(ΒЈ==null) return;Ȃ=new float[3,2];Ǩ=new Dictionary<string,float[,]>();
for(int i=0;i<Ą.Count;i++){IMyThrust щ=Ą[i];if(!щ.IsFunctional) continue;Vector3 ς=х(ΒЈ,щ.WorldMatrix.Backward);float ĳ=щ.MaxEffectiveThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) Ȃ[0,0]+=ĳ;else Ȃ[0,1]-=ĳ;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) Ȃ[1,0]+=ĳ;else Ȃ[1,1]-=ĳ;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) Ȃ[2,0]+=ĳ;else Ȃ[2,1]-=ĳ;String Ǘ=ΖΝ(щ);float[,]ǩ=null;if(Ǩ.ContainsKey(Ǘ)) ǩ=Ǩ[Ǘ];else{ǩ=new float[3,2];Ǩ.Add(Ǘ,ǩ);}float ě=щ.MaxThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) ǩ[0,0]+=ě;else ǩ[0,1]-=ě;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) ǩ[1,0]+=ě;else ǩ[1,1]-=ě;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) ǩ[2,0]+=ě;else ǩ[2,1]-=ě;}}static String ΖΝ(IMyThrust щ){return щ.BlockDefinition.SubtypeId;}Vector3 Γω(Vector3 ǝ,float[,]Ɛ){return new Vector3(
ǝ.X>=0?Ɛ[0,0]:Ɛ[0,1],
ǝ.Y>=0?Ɛ[1,0]:Ɛ[1,1],
ǝ.Z>=0?Ɛ[2,0]:Ɛ[2,1]);}bool ΖΜ(ά έ,String Ǘ,out float Δσ){Δσ=0;int Β=ă.IndexOf(Ǘ);if(Β==-1||έ.Ȁ==null||Β>=έ.Ȁ.Length)
return false;Δσ=έ.Ȁ[Β];if(Δσ==-1) return false;return true;}Vector3 Γλ(Vector3 ǝ,ά έ){if(έ!=null){Vector3 ί=new Vector3();for(int i=0;i<Ǩ.Keys.Count;i++){String Ǘ=Ǩ.Keys.ElementAt(i);float ΑΗ=0;if(!ΖΜ(έ,Ǘ,out ΑΗ))
return Γω(ǝ,Ȃ);ί+=Γω(ǝ,Ǩ.Values.ElementAt(i))*ΑΗ;}return ί;}return Γω(ǝ,Ȃ);}float Ʋ(Vector3 ǝ,ά έ){return Ʋ(ǝ,new Vector3(),έ);}float Ʋ(Vector3 ǝ,Vector3 φ,ά έ){Vector3 ΛΩ=Γλ(ǝ,έ);Vector3 ĳ=ΛΩ+φ*Ŧ;float ţ=(ĳ/ǝ).AbsMin();return(float)(ǝ*ţ).Length();}float Αϊ(Vector3 Βϋ,Vector3 Ǝ,ά έ)
{if(Βϋ.Length()==0) return 0;float Ζό=1;if(Ǝ.Length()>0) Ζό=Math.Min(1,ů(-Ǝ,Βϋ)/90)*0.4f+0.6f;float Αμ=Ʋ(Βϋ,Ǝ,έ);if(Αμ==0) return 0.1f;float ΒΠ=К(Αμ,Ŧ);float Εκ=(float) Math.Sqrt(К(Βϋ.Length(),ΒΠ*0.5f));return ΒΠ*Εκ*Ζό*Ȇ;}float ΑЇ(Vector3 ȏ,Vector3 Б,Vector3 φ,ά έ)
{if(φ.Length()==0f) return 0;Vector3 Βό=ŋ(ȏ,Б,Vector3.Normalize(φ));float ǵ=Ʋ(-Βό,έ);return ǵ/φ.Length();}void ΕЄ(bool Κς){float Ǫ=0;float Δφ=0.9f;if(Κς){е=-1;Ū=0;ΒБ=null;if(ε!=Ρ.ю&&Ζ.φ.Length()!=0){Ǫ=Δφ*ΑЇ(Ζ.ι,Ζ.θ*-1,Ζ.φ,null);if(Ǫ<е||е==-1) е=Ǫ;}if(Ο.λ&&Ο.φ.Length()!=0)
{Ǫ=Δφ*ΑЇ(Ο.ι,Ο.θ*-1,Ο.φ,null);if(Ǫ<е||е==-1) е=Ǫ;}return;}if(Ū==-1) return;if(Ū>=0){int ΘΓ=0;while(Ū<Ψ.Count){if(ΘΓ>100) return;ΘΓ++;ά έ=Ψ[Ū];if(έ.φ.Length()!=0f){Ǫ=Δφ*Math.Min(
ΑЇ(έ.ι,έ.θ*-1,έ.φ,έ),
ΑЇ(έ.ι*-1,έ.θ*-1,έ.φ,έ));if(Ǫ<е||е==-1) е=Ǫ;}else ΒБ=έ;Ū++;}Ū=-1;}bool Δώ=true;float Ļ=0;if(ΒБ!=null){for(int i=0;i<Ǩ.Count;i++){String Ǘ=Ǩ.Keys.ElementAt(i);float[,]ǩ=Ǩ.Values.ElementAt(i);float ΑΗ=0;if(!ΖΜ(ΒБ,Ǘ,out ΑΗ)){Δώ=false;break;}for(int a=0;a<ǩ.GetLength(0);a++)
for(int b=0;b<ǩ.GetLength(1);b++){float Ŏ=Math.Abs(ǩ[a,b]*ΑΗ);if(Ŏ==0) continue;Δώ=true;if(Ļ==0||Ŏ<Ļ) Ļ=Ŏ;}}}if(!Δώ){for(int a=0;a<Ȃ.GetLength(0);a++)
for(int b=0;b<Ȃ.GetLength(1);b++){float Ŏ=Math.Abs(Ȃ[a,b]);if(Ŏ==0) continue;if(Ļ==0||Ŏ<Ļ) Ļ=Ŏ;}}if(Ļ>0){float Κώ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǫ=К(Ļ,Κώ);if(Ǫ>0)
if(Ǫ<е||е==-1) е=Ǫ;}}void ΕϏ(Vector3 ǵ,bool О){if(!О){for(int i=0;i<Ą.Count;i++)
Ą[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔΦ=Γλ(ǵ,null);float Λς=Math.Min(1,Math.Abs(К(ǵ.X,ΔΦ.X)));float Λσ=Math.Min(1,Math.Abs(К(ǵ.Y,ΔΦ.Y)));float Λτ=Math.Min(1,Math.Abs(К(ǵ.Z,ΔΦ.Z)));for(int i=0;i<Ą.Count;i++){IMyThrust щ=Ą[i];Vector3 ς=Ιλ(х(Ϋ,щ.WorldMatrix.Backward),1);
if(ς.X!=0&&Math.Sign(ς.X)==Math.Sign(ǵ.X))
щ.SetValueFloat("Override",щ.MaxThrust*Λς);else if(ς.Y!=0&&Math.Sign(ς.Y)==Math.Sign(ǵ.Y))
щ.SetValueFloat("Override",щ.MaxThrust*Λσ);else if(ς.Z!=0&&Math.Sign(ς.Z)==Math.Sign(ǵ.Z))
щ.SetValueFloat("Override",щ.MaxThrust*Λτ);else
щ.SetValueFloat("Override",0.0f);}}void ΒΘ(bool О,float ΚЍ,float ǔ,float ǥ,float ǫ){for(int i=0;i<Βο.Count;i++){IMyGyro Л=Βο[i];Л.GyroOverride=О;if(!О) Л.GyroPower=100;else Л.GyroPower=ΚЍ;if(!О) continue;Vector3 Ь=Ϋ.WorldMatrix.Forward;Vector3 с=Ϋ.WorldMatrix.Right;Vector3 Б=Ϋ.WorldMatrix.Up;
Vector3 Ƈ=Л.WorldMatrix.Forward;Vector3 Ɖ=Л.WorldMatrix.Up;Vector3 ƈ=Л.WorldMatrix.Left*-1;if(Ƈ==Ь) Л.SetValueFloat("Roll",ǫ);else if(Ƈ==(Ь*-1)) Л.SetValueFloat("Roll",ǫ*-1);else if(Ɖ==(Ь*-1)) Л.SetValueFloat("Yaw",ǫ);else if(Ɖ==Ь) Л.SetValueFloat("Yaw",ǫ*-1);else if(ƈ==Ь) Л.SetValueFloat("Pitch",ǫ);
else if(ƈ==(Ь*-1)) Л.SetValueFloat("Pitch",ǫ*-1);if(ƈ==(с*-1)) Л.SetValueFloat("Pitch",ǔ);else if(ƈ==с) Л.SetValueFloat("Pitch",ǔ*-1);else if(Ɖ==с) Л.SetValueFloat("Yaw",ǔ);else if(Ɖ==(с*-1)) Л.SetValueFloat("Yaw",ǔ*-1);else if(Ƈ==(с*-1)) Л.SetValueFloat("Roll",ǔ);
else if(Ƈ==с) Л.SetValueFloat("Roll",ǔ*-1);if(Ɖ==(Б*-1)) Л.SetValueFloat("Yaw",ǥ);else if(Ɖ==Б) Л.SetValueFloat("Yaw",ǥ*-1);else if(ƈ==Б) Л.SetValueFloat("Pitch",ǥ);else if(ƈ==(Б*-1)) Л.SetValueFloat("Pitch",ǥ*-1);else if(Ƈ==Б) Л.SetValueFloat("Roll",ǥ);else if(Ƈ==(Б*-1)) Л.SetValueFloat("Roll",ǥ*-1);
}}void Ζσ(){this.Δϋ=false;}void л(Vector3 Ĳ,Vector3 Ь,Vector3 ΔЍ,float Ģ,bool Αρ){ΓΡ(Ĳ,Ģ,Αρ);ΒΡ=Ģ;ΔЏ=false;this.Εη=Ь;this.Θά=ΔЍ;}void л(Vector3 Ĳ,Vector3 Ь,Vector3 ΔЍ,bool Αρ){л(Ĳ,Ь,ΔЍ,2f,Αρ);}void ΓΡ(Vector3 Ĳ,float Ģ,bool Αρ){ΒΡ=Ģ;this.Δϋ=true;this.Ζύ=Αρ;ΔЏ=true;
Βτ=false;this.Εζ=Ĳ;}void ΔΘ(){Ȅ(false,false,false,Ѓ,0);ΓΕ=false;}void и(Vector3 η,float Ǿ){Ȅ(true,false,false,η,Ǿ);}void Ȅ(bool О,bool Εα,bool Ευ,Vector3 η,float Ǿ){Ȅ(О,Εα,Ευ,η,η-δ,0.0f,Ǿ);}void Ȅ(bool О,bool Εα,bool Ευ,Vector3 η,Vector3 ΛΙ,float ΛΚ,float Ǿ){ΓΕ=true;
this.Θΰ=О;Ѓ=η;this.ΕΔ=Ǿ;this.ΕΠ=ΛΚ;this.ΘΛ=Εα;this.ΘΚ=Ευ;this.Ηά=ΛΙ;в=Vector3.Distance(η,δ);}void Ιν(){χ.GetBlocksOfType(ǁ,Ǚ);}void Ιξ(){if(ΒЊ){А=2;return;}List<IMyRemoteControl>Αα=new List<IMyRemoteControl>();List<IMySensorBlock>ΘΘ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ακ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αα,Ǚ);χ.GetBlocksOfType(Ч,Ǚ);χ.GetBlocksOfType(ΘΘ,Ǚ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ακ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=Ǭ(Ч,pamTag,true);Ζλ();Ζμ(Ακ);Ŋ(Ч,setLCDFontAndSize,1.4f,false);Ŋ(Ș,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘΧ=Ǭ(ΘΘ,pamTag,true);if(ΘΧ.Count>0) ψ=ΘΧ[0];else ψ=null;if(Μ==Κ.Ƹ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Μ==Κ.ǭ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Μ==Κ.ή){χ.GetBlocksOfType(Ελ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘΦ=Ǭ(Αα,pamTag,true);if(ΘΦ.Count>0) Αα=ΘΦ;if(Αα.Count>0) Ϋ=Αα[0];else{Ϋ=null;А=2;Λ="Remote is missing";return;}μ=(float) Ϋ.CubeGrid.WorldVolume.Radius*2;ǣ=null;if(Μ!=Κ.ή){ΕЅ();if(ω.Count>0&&ǣ!=null){if(ψ!=null&&(ǣ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ϋ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ϋ.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(ǣ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Forward&&
ǣ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void Ιο(){χ.GetBlocksOfType(ȳ,Ǚ);χ.GetBlocksOfType(ў,Ǚ);χ.GetBlocksOfType(Ą,Ǚ);χ.GetBlocksOfType(Βο,Ǚ);χ.GetBlocksOfType(Į,Ǚ);χ.GetBlocksOfType(ļ,Ǚ);χ.GetBlocksOfType(ǟ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΒА,Ǚ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ў=Ǭ(ў,"ConnectorMedium",false);else
ў=Ǭ(ў,"Connector",false);List<IMyShipConnector>ΘΤ=Ǭ(ў,pamTag,true);if(ΘΤ.Count>0) ў=ΘΤ;if(А<=1){if(ў.Count==0){А=1;Λ="Connector is missing";}if(Βο.Count==0){А=1;Λ="Gyros are missing";}if(Ą.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΘΨ=Ǭ(ΒА,pamTag,true);
if(ΘΨ.Count>0) ΒА=ΘΨ;List<IMyLandingGear>ΘΥ=Ǭ(ȳ,pamTag,true);if(ΘΥ.Count>0) ȳ=ΘΥ;for(int i=0;i<ȳ.Count;i++) ȳ[i].AutoLock=false;List<IMyBatteryBlock>ΘΣ=Ǭ(Į,pamTag,true);if(ΘΣ.Count>0) Į=ΘΣ;List<IMyGasTank>ΘΩ=Ǭ(ǟ,pamTag,true);if(ΘΩ.Count>0) ǟ=ΘΩ;}void Ιπ(){χ.GetBlocksOfType(ŏ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ƅ.Clear();for(int i=ŏ.Count-1;i>=0;i--){if(ΙΧ(ŏ[i])){Ƅ.Add(ŏ[i]);ŏ.RemoveAt(i);}}}bool ΙΧ(IMyTerminalBlock Ω){if(Ω is IMyCargoContainer) return true;if(Ω is IMyShipDrill) return true;if(Ω is IMyShipGrinder) return true;if(Ω is IMyShipConnector){if(((IMyShipConnector) Ω).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Γο(Ω,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕЅ(){if(Ϋ==null) return;ǣ=null;float ΒЃ=0,Βώ=0,ΒЄ=0,ΒϏ=0,ΒЅ=0,ΒЀ=0;List<IMyTerminalBlock>Εε=Ǭ(ω,pamTag,true);bool ΚЄ=Εε.Count==0;if(Εε.Count>0) ǣ=Εε[0];else if(ω.Count>0) ǣ=ω[0];int ȭ=0;float ƚ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǣ.WorldMatrix.Forward){if(ΚЄ){А=2;Λ="Mining direction is unclear!";return;}continue;}ȭ++;ƚ=Math.Max(ƚ,ΙЁ(ω[i]));Vector3 П=ΑІ(Ϋ,ω[i].GetPosition());if(i==0){ΒЃ=П.X;Βώ=П.X;ΒЄ=П.Y;ΒϏ=П.Y;ΒЅ=П.Z;ΒЀ=П.Z;}Βώ=Math.Max(П.X,Βώ);ΒЃ=Math.Min(П.X,ΒЃ);
ΒϏ=Math.Max(П.Y,ΒϏ);ΒЄ=Math.Min(П.Y,ΒЄ);ΒЀ=Math.Max(П.Z,ΒЀ);ΒЅ=Math.Min(П.Z,ΒЅ);}if(ƚ==0) ƚ=drillRadius;bool Ĳ=ǣ!=null&&ǣ.WorldMatrix.Forward==Ϋ.WorldMatrix.Down;ȁ=(Βώ-ΒЃ)*(1-ū)+ƚ*2;ț=(Ĳ?ΒЀ-ΒЅ:ΒϏ-ΒЄ)*(1-Ŗ)+ƚ*2;IMyCubeGrid g=Ϋ.CubeGrid;float Εω=0,Εϊ=0,Εϋ=0,Εό=0;
for(int c=0;c<8;c++){Vector3 p=ΑІ(Ϋ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ĳ?p.Z:p.Y;if(c==0||p.X<Εω) Εω=p.X;if(c==0||p.X>Εϊ) Εϊ=p.X;if(c==0||y<Εϋ) Εϋ=y;if(c==0||y>Εό) Εό=y;}ΒЌ=Εϊ-Εω+g.GridSize;Εά=Εό-Εϋ+g.GridSize;}float ΙЁ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǭ<T>(List<T>Ѝ,String ǿ,bool Βπ){List<T>ί=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(Γο(Ѝ[i],ǿ,Βπ)) ί.Add(Ѝ[i]);return ί;}bool Γο<T>(T Ω,String ǿ,bool Βπ){IMyTerminalBlock b=(IMyTerminalBlock) Ω;if(Βπ&&b.CustomName.ToUpper().Contains(ǿ.ToUpper())) return true;if(!Βπ&&b.BlockDefinition.ToString().ToUpper().Contains(ǿ.ToUpper())) return true;return false;}void Ǜ<T>(List<T>Ѝ,bool О)
{for(int i=0;i<Ѝ.Count;i++)
Ζι((IMyTerminalBlock) Ѝ[i],О);}void Ζι(IMyTerminalBlock Ω,bool О){if(Ω==null) return;String ΚΗ=О?"OnOff_On":"OnOff_Off";var a=Ω.GetActionWithName(ΚΗ);a.Apply(Ω);}void ȅ(bool О){Εβ=О;if(!ŧ) return;Ǜ(ΒА,О);}void ΓА(ChargeMode Џ){for(int i=0;i<Į.Count;i++) Į[i].ChargeMode=Џ;}void ΔΑ(bool ΛΡ)
{for(int i=0;i<ǟ.Count;i++) ǟ[i].Stockpile=ΛΡ;}void ƻ(List<IMyLandingGear>Δχ,bool Ηώ){for(int i=0;i<Δχ.Count;i++){if(Ηώ) Δχ[i].Lock();if(!Ηώ) Δχ[i].Unlock();}}void Ζκ(bool О){if(ΗΛ==О) return;List<IMyShipController>ΒΫ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΫ,Ǚ);if(ΒΫ.Count==0) return;
for(int i=0;i<ΒΫ.Count;i++)
ΒΫ[i].DampenersOverride=О;ΗΛ=О;}IMyShipConnector з(MyShipConnectorStatus Ǖ){for(int i=0;i<ў.Count;i++){if(!ȡ(ў[i],true)) continue;if(ў[i].Status==Ǖ) return ў[i];}return null;}void Ō(bool Εέ,bool Ββ,float Κι,float ΚΝ){if(ψ==null||ω.Count==0) return;Vector3 ΒΧ=new Vector3();
int ȭ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǣ.WorldMatrix.Forward) continue;ȭ++;ΒΧ+=ω[i].GetPosition();}ΒΧ=ΒΧ/ȭ;Vector3 Ɣ=ΑІ(ψ,ΒΧ);ψ.Enabled=true;ψ.ShowOnHUD=Εέ;ψ.LeftExtend=(Ββ?1:ё)/2f*ȁ-Ɣ.X;ψ.RightExtend=(Ββ?1:ё)/2f*ȁ+Ɣ.X;ψ.TopExtend=(Ββ?1:ѐ)/2f*ț+Ɣ.Y;
ψ.BottomExtend=(Ββ?1:ѐ)/2f*ț-Ɣ.Y;ψ.FrontExtend=(Εέ?о:Κι)-Ɣ.Z;ψ.BackExtend=Εέ?0:ΚΝ+μ*0.75f+Ɣ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void Ŋ<T>(List<T>Ѝ,bool ΛΜ,float Κη,bool Κσ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Αι=null;if(Ѝ[i]is IMyTextSurface) Αι=(IMyTextSurface) Ѝ[i];if(Αι!=null){Αι.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΛΜ) continue;Αι.Font="Debug";if(Κσ) continue;Αι.FontSize=Κη;}}}bool ΑЊ(){Vector3D γ;return this.Ϋ.TryGetPlanetPosition(out γ);}int ΙΗ(Vector3 ΛΤ,int ΘΜ){if(ΘΜ==-1) return 0;double Ы=-1;int ď=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϋ=Vector3.Distance(Ψ[i].Ε,ΛΤ);if(Ы==-1||ϋ<Ы){ď=i;Ы=ϋ;}
}return Math.Sign(ď-ΘΜ);}bool ΙΨ(Vector3 γ){List<Vector3>Я=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) Я.Add(Ψ[i].Ε);if(Ο.λ&&Ψ.Count>=1){Vector3 ſ=new Vector3();Ƞ(Ο,dockDist*μ,false,out ſ);if(Vector3.Distance(Ο.Ε,Ψ.First().Ε)<Vector3.Distance(Ο.Ε,Ψ.Last().Ε)){Я.Insert(0,ſ);Я.Insert(0,Ο.Ε);}
else{Я.Add(ſ);Я.Add(Ο.Ε);}}if(Μ==Κ.ή){if(Ζ.λ&&Ψ.Count>=1){Vector3 ΔΡ=new Vector3();Ƞ(Ζ,dockDist*μ,false,out ΔΡ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){Я.Insert(0,ΔΡ);Я.Insert(0,Ζ.Ε);}else{Я.Add(ΔΡ);Я.Add(Ζ.Ε);}}}else{if(ε!=Ρ.ю)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
Я.Insert(0,Ζ.Ε);else Я.Add(Ζ.Ε);}int ď=-1;double Ȋ=-1;for(int i=0;i<Я.Count;i++){double ϋ=Vector3.Distance(Я[i],γ);if(ϋ<Ȋ||Ȋ==-1){Ȋ=ϋ;ď=i;}}if(Я.Count==0) return false;double Δν=Vector3.Distance(Я[ď],γ);double ΚЎ=Vector3.Distance(Я[Math.Max(0,ď-1)],Я[ď])*1.5f;
double ΚЃ=Vector3.Distance(Я[Math.Min(Я.Count-1,ď+1)],Я[ď])*1.5f;return Δν<ΚЎ||Δν<ΚЃ||Δν<laneOffset*μ*1.5f;}static float К(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ιλ(Vector3 v,int Δκ){return new Vector3(Math.Round(v.X,Δκ),Math.Round(v.Y,Δκ),Math.Round(v.Z,Δκ));
}float ů(Vector3 a,Vector3 b){if(a==b) return 0;float Κγ=(a*b).Sum;float Ηψ=a.Length();float Ηω=b.Length();if(Ηψ==0||Ηω==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(Κγ/(Ηψ*Ηω)));if(float.IsNaN(ί)) return 0;return ί;}float ń(Vector3 v,Vector3 ΒЈ,float ΛΝ){float Ž=ů(v,ΒЈ);
if(ΛΝ>0f) Ž*=-1;if(Ž>-90f) return Ž-90f;else return 180f-(-Ž-90f);}double ΕЌ(float Κά){return(Math.PI/180)*Κά;}bool Γς(double Κύ,double value,double Ƒ){return(value>=Κύ&&value<=Ƒ);}Vector3 ΙЂ(IMyTerminalBlock Ω,Vector3 П){return Vector3D.Transform(П,Ω.WorldMatrix);
}Vector3 ΑІ(IMyTerminalBlock Ω,Vector3 Λο){return х(Ω,Λο-Ω.GetPosition());}Vector3 х(IMyTerminalBlock Ω,Vector3 Εψ){return Vector3D.TransformNormal(Εψ,MatrixD.Transpose(Ω.WorldMatrix));}Vector3 ŋ(Vector3 ȏ,Vector3 Б,Vector3 Εψ){MatrixD ΕΓ=MatrixD.CreateFromDir(ȏ,Б);return Vector3D.TransformNormal(Εψ,MatrixD.Transpose(ΕΓ));
}Vector3 ΔΛ(Vector3 ȏ,Vector3 Б,Vector3 П){MatrixD ΕΓ=MatrixD.CreateFromDir(ȏ,Б);return Vector3D.Transform(П,ΕΓ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 π(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Π(τ,0)),float.Parse(Π(τ,1)),float.Parse(Π(τ,2)));
}String ΚΕ(ά έ){String С=":";String ί=σ(έ.Ε)+С+σ(έ.ι)+С+
σ(έ.θ)+С+σ(έ.Ђ)+С+σ(έ.φ);for(int i=0;i<έ.Ȁ.Length;i++)
ί+=С+Math.Round(έ.Ȁ[i],3);return ί;}ά Ιϋ(String s){String[]τ=s.Split(':');ά έ=new ά(
π(Π(τ,0)),π(Π(τ,1)),
π(Π(τ,2)),π(Π(τ,3)),
π(Π(τ,4)));int Β=5;List<float>ΗΩ=new List<float>();while(Β<τ.Length){String ΓΛ=Π(τ,Β);float f=0;if(!float.TryParse(ΓΛ,out f)) break;ΗΩ.Add(f);Β++;}έ.Ȁ=ΗΩ.ToArray();return έ;}System.Text.StringBuilder Ɲ=new System.Text.StringBuilder();void Χ<T>(T value,bool ΚЂ){if(ΚЂ) Ɲ.Append('\n');Ɲ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΑϏ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΝ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Π(String[]ΚΛ,int Β){String ΓΛ=ΚΛ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΛ)) return "";
return ΓΛ;}void Save(){if(ΒЊ||Μ==Κ.ћ){Storage="";return;}Storage=ΓΨ();}String ΓΨ(){Ɲ.Clear();Ɲ.Append(DATAREV+";");Χ(σ(ΒЉ),false);Χ(σ(Ο.ι));Χ(σ(Ο.Ђ));Χ(σ(Ο.θ));Χ(σ(Ο.φ));Χ(σ(Ο.Ε));Χ(σ(Ο.п));Χ(Ο.λ);Χ(σ(Ζ.Ε));Χ(σ(Ζ.φ));Χ(σ(Ζ.ι));Χ(σ(Ζ.θ));Χ(σ(Ζ.Ђ));Χ(σ(Ζ.п));Χ(Ζ.λ);Χ(σ(ǒ));Χ(σ(Ĝ));Χ(";");
Χ((int) Μ,false);Χ((int) ε);Χ((int) Ǐ);Χ(Ȓ);Χ(ǐ);Χ(р);Χ(Ǒ);Χ(Ȇ);Χ(Ĥ);Χ(ģ);if(Μ==Κ.ή){Χ((int) Ї.Т);Χ(Ї.ǳ);Χ(Ї.Ĵ);Χ(ΑϏ(Ї.Ǵ));Χ(ΑϏ(Ї.ǹ));Χ((int) Ј.Т);Χ(Ј.ǳ);Χ(Ј.Ĵ);Χ(ΑϏ(Ј.Ǵ));Χ(ΑϏ(Ј.ǹ));}else{Χ((int) Ȗ);Χ((int) ѕ);Χ((int) д);Χ((int) џ);Χ((int) ř);Χ(ŀ);Χ(ŧ);Χ(ƌ);Χ(Ɔ);Χ(ё);Χ(ѐ);Χ(о);Χ(Ʈ);Χ(ƭ);
Χ(ū);Χ(Ŗ);Χ(Ќ);Χ(Э);Χ(Ё);Χ(ώ);Χ(Ě);Χ(б);Χ(Ť);Χ(Ŝ);}Χ(";");for(int i=0;i<ă.Count;i++)
Χ((i>0?"|":"")+ă[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΚΕ(Ψ[i]),i>0);Χ(";",false);Χ(г+"|"+Г,false);Χ(";",false);Χ(ΘЉ(),false);String Θ=Ɲ.ToString();Ɲ.Clear();return Θ;}ќ Ικ(){return ΒΓ(Storage);}ќ ΒΓ(String Θ){if(Θ=="") return ќ.Ια;String[]ť=Θ.Split(';');if(Π(ť,0)!=DATAREV) return ќ.ΓϏ;int Β=0;try{String[]Ξ=Π(ť,1).Split('\n');
ΒЉ=π(Π(Ξ,Β++));Ο.ι=π(Π(Ξ,Β++));Ο.Ђ=π(Π(Ξ,Β++));Ο.θ=π(Π(Ξ,Β++));Ο.φ=π(Π(Ξ,Β++));Ο.Ε=π(Π(Ξ,Β++));Ο.п=π(Π(Ξ,Β++));Ο.λ=bool.Parse(Π(Ξ,Β++));Ζ.Ε=π(Π(Ξ,Β++));Ζ.φ=π(Π(Ξ,Β++));Ζ.ι=π(Π(Ξ,Β++));Ζ.θ=π(Π(Ξ,Β++));Ζ.Ђ=π(Π(Ξ,Β++));Ζ.п=π(Π(Ξ,Β++));Ζ.λ=bool.Parse(Π(Ξ,Β++));
ǒ=π(Π(Ξ,Β++));Ĝ=π(Π(Ξ,Β++));Ξ=Π(ť,2).Split('\n');Β=0;Μ=(Κ) int.Parse(Π(Ξ,Β++));ε=(Ρ) int.Parse(Π(Ξ,Β++));Ǐ=(Ρ) int.Parse(Π(Ξ,Β++));Ȓ=int.Parse(Π(Ξ,Β++));ǐ=int.Parse(Π(Ξ,Β++));р=int.Parse(Π(Ξ,Β++));Ǒ=int.Parse(Π(Ξ,Β++));Ȇ=float.Parse(Π(Ξ,Β++));Ĥ=bool.Parse(Π(Ξ,Β++));
ģ=bool.Parse(Π(Ξ,Β++));if(Μ==Κ.ή){Ї.Т=(ρ) int.Parse(Π(Ξ,Β++));Ї.ǳ=float.Parse(Π(Ξ,Β++));Ї.Ĵ=float.Parse(Π(Ξ,Β++));Ї.Ǵ=ΒΝ(Π(Ξ,Β++));Ї.ǹ=ΒΝ(Π(Ξ,Β++));Ј.Т=(ρ) int.Parse(Π(Ξ,Β++));Ј.ǳ=float.Parse(Π(Ξ,Β++));Ј.Ĵ=float.Parse(Π(Ξ,Β++));Ј.Ǵ=ΒΝ(Π(Ξ,Β++));Ј.ǹ=ΒΝ(Π(Ξ,Β++));}else
{Ȗ=(я) int.Parse(Π(Ξ,Β++));ѕ=(ы) int.Parse(Π(Ξ,Β++));д=(В) int.Parse(Π(Ξ,Β++));џ=(ǘ) int.Parse(Π(Ξ,Β++));ř=(я) int.Parse(Π(Ξ,Β++));ŀ=bool.Parse(Π(Ξ,Β++));ŧ=bool.Parse(Π(Ξ,Β++));ƌ=bool.Parse(Π(Ξ,Β++));Ɔ=bool.Parse(Π(Ξ,Β++));ё=int.Parse(Π(Ξ,Β++));ѐ=int.Parse(Π(Ξ,Β++));о=int.Parse(Π(Ξ,Β++));
Ʈ=float.Parse(Π(Ξ,Β++));ƭ=float.Parse(Π(Ξ,Β++));ū=float.Parse(Π(Ξ,Β++));Ŗ=float.Parse(Π(Ξ,Β++));Ќ=int.Parse(Π(Ξ,Β++));Э=int.Parse(Π(Ξ,Β++));Ё=int.Parse(Π(Ξ,Β++));ώ=int.Parse(Π(Ξ,Β++));Ě=int.Parse(Π(Ξ,Β++));б=int.Parse(Π(Ξ,Β++));Ť=int.Parse(Π(Ξ,Β++));Ŝ=float.Parse(Π(Ξ,Β++));
}Ξ=Π(ť,3).Replace("\n","").Split('|');ă=Ξ.ToList();Ξ=Π(ť,4).Split('\n');Ψ.Clear();if(Ξ.Count()>=1&&Ξ[0]!="")
for(int i=0;i<Ξ.Length;i++)
Ψ.Add(Ιϋ(Π(Ξ,i)));Ξ=Π(ť,5).Split('|');г=Π(Ξ,0);if(!int.TryParse(Π(Ξ,1),out Г)) Г=-1;if(г.Length!=Ќ*Э){г="";Г=-1;}ΘЈ(Π(ť,6));}catch{return ќ.ΕЏ;}ƞ=Ǐ;Є();return ќ.ΒΚ;}const int ΕЃ=64000;IMyTerminalBlock ΕА(){List<IMyTerminalBlock>Ƃ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ƃ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ƃ.Count>0?Ƃ[0]:null;}void ΘЍ(){IMyTerminalBlock Ω=ΕА();if(Ω==null){Λ="Export: name a block "+dataTag;return;}String Θ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΓΨ();if(Θ.Length>ΕЃ){Λ="Export too large: "+Θ.Length+" chars, max "+ΕЃ;return;}Ω.CustomData=Θ;Λ="Exported "+Ψ.Count+" waypoints";}void ΙΤ(bool ΔІ){IMyTerminalBlock Ω=ΕА();if(Ω==null){Λ="Import: name a block "+dataTag;return;}
ΙΥ(Ω.CustomData,ΔІ);}void ΙΥ(String Θ,bool ΔІ){Θ=Θ.Replace("\r","");if(Θ.StartsWith("//")){int ΗЌ=Θ.IndexOf('\n');Θ=ΗЌ<0?"":Θ.Substring(ΗЌ+1);}Θ=Θ.Trim();if(Θ==""){Λ="Import: no PAM data in "+dataTag;return;}Є();String ΖЄ=ΓΨ();ќ ί=ΒΓ(Θ);if(ί==ќ.ΒΚ&&ΔІ){ά Φ=new ά(Ζ);
Vector3 ΚЀ=ǒ,ΚϏ=Ĝ;int w=ё,h=ѐ,d=о;я Αη=Ȗ;ǘ Δλ=џ;float ΚЇ=ū,ΚІ=Ŗ;ΒΓ(ΖЄ);Ζ=Φ;ǒ=ΚЀ;Ĝ=ΚϏ;ё=w;ѐ=h;о=d;Ȗ=Αη;џ=Δλ;ū=ΚЇ;Ŗ=ΚІ;ε=Ρ.Ÿ;ƿ(true,true);ε=Ρ.Ÿ;Ǐ=Ρ.Ÿ;Λ="Imported job, run CONT";return;}if(ί==ќ.ΒΚ){Αέ=false;Λ="Imported "+Ψ.Count+" waypoints";
return;}ΒΓ(ΖЄ);Λ=ί==ќ.ΓϏ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζλ(){Œ.Clear();for(int i=Ч.Count-1;i>=0;i--){String Θ=Ч[i].CustomData.ToUpper();bool ΔЄ=false;if(Θ=="INSTRUCTIONS"){ΔЄ=true;Βα=true;}if(Θ=="DEBUG") ΔЄ=true;if(ΔЄ){Œ.Add(Ч[i]);Ч.RemoveAt(i);}
}Ŋ(Œ,false,1,false);}void Ζμ(List<IMyTerminalBlock>Ƃ){Ș.Clear();for(int i=0;i<Ƃ.Count;i++){IMyTerminalBlock Ω=Ƃ[i];try{String ΘΒ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ω.CustomName.IndexOf(ΘΒ);int ƣ=-1;if(Β<0||!int.TryParse(Ω.CustomName.Substring(Β+ΘΒ.Length,1),out ƣ)) continue;
if(ƣ==-1) continue;ƣ--;IMyTextSurfaceProvider ΘΔ=(IMyTextSurfaceProvider) Ω;if(ƣ<ΘΔ.SurfaceCount&&ƣ>=0)
Ș.Add(ΘΔ.GetSurface(ƣ));}catch{}}}String ΒЁ="";void Ζζ(){String ΕΒ="";String ΑΖ="";ΒЁ=ΕЂ(false);ΕΒ+=ΒЁ;ΑΖ+=ΒЁ;ΑΖ+=ΙΔ();for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(ΕΒ);for(int i=0;i<Ș.Count;i++) Ș[i].WriteText(ΕΒ);Echo(ΑΖ);for(int i=0;i<Œ.Count;i++){IMyTextPanel ш=Œ[i];
String Θ=ш.CustomData.ToUpper();if(Θ=="DEBUG") ш.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ш.WriteText(ΖΗ());}}string ΖΗ(){String Γ="";try{float ΗЁ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ɓ*ΗЁ)+" / "+(int)(ƒ*ΗЁ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ɓ*100f,1)+"% / "+Math.Round(ƒ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ǆ,2)+"ms / "+Math.Round(ΕΖ,2)+"ms \n";}catch{}for(int i=0;i<ĝ.Count;i++)
Γ+=""+ĝ.Keys.ElementAt(i)+" = "+Math.Round(ĝ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ĝ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙΞ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ŋ(Ч,setLCDFontAndSize,0.9f,false);Ŋ(Ș,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(Γ+ę.ToString());for(int i=0;i<Ș.Count;i++) Ș[i].WriteText(Γ+ę.ToString());}void Ȧ(){if(!Βα) return;try{ΕΤ=Runtime.CurrentInstructionCount;}catch{}
}void Ȟ(String ΰ){if(!Βα) return;if(ΕΤ==0) return;try{float ΓΙ=(Runtime.CurrentInstructionCount-ΕΤ)/Runtime.MaxInstructionCount*100;if(!ĝ.ContainsKey(ΰ))
ĝ.Add(ΰ,new float[]{ΓΙ,ΓΙ});else{ĝ[ΰ][0]=ΓΙ;ĝ[ΰ][1]=Math.Max(ΓΙ,ĝ[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Θρ(String Ȏ){if(!ƃ) return;String С=""+BROADCAST_SEP;String β=VERSION+С;β+=Ư(ơ)+С;
β+=(int) Μ+С;β+=Ų(δ.X)+""+С;β+=Ų(δ.Y)+С;β+=Ų(δ.Z)+С;if(Μ!=Κ.ή)
β+=Γθ(Ǐ)+(Ǐ==Ρ.И?" "+Math.Round(ΑΤ,1)+"%":"")+С;else β+=Γθ(Ǐ)+С;β+=Ư(Λ)+С;β+=Ў+""+С;β+=Ư(ΒЁ)+С;β+=Ȍ+С;β+=ȓ+С;for(int i=0;i<Ƿ.Count;i++){if(Ƿ[i].Ə!=Щ.Ŵ) continue;β+=Ư(Ƿ[i].ΰ)+"/"+Ư(Ƿ[i].υ)+"/"+Ƿ[i].Њ+С;}Ιτ(β,Ȏ);}String Ư(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ιτ(String β,String Ȏ){ΒΖ(β,BROADCAST_SELF_ID,Ȏ);}void ΒΖ(String β,String η,String Ȏ){try{if(!ƃ) return;β=BROADCAST_TAG+BROADCAST_SEP+ΖΓ()+BROADCAST_SEP+η+BROADCAST_SEP+Δή+BROADCAST_SEP+Ȏ+BROADCAST_SEP+β;this.IGC.SendBroadcastMessage(BROADCAST_TAG,β);}catch(Exception e){ę=e;}
}String ΖΓ(){if(Μ!=Κ.ћ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γχ(String β){return β.StartsWith(BROADCAST_TAG);}bool ж(ref String Θ,out string ΑΙ,Char С){int Β=Θ.IndexOf(С);ΑΙ="";if(Β<0) return false;ΑΙ=Θ.Substring(0,Β);Θ=Θ.Remove(0,Β+1);return true;}bool Ζε(ref String β,out String ȕ,out String Ȏ)
{ȕ="";Ȏ="";if(!ƃ) return false;String ǿ="";if(!ж(ref β,out ǿ,BROADCAST_SEP)||!Γχ(ǿ)) return false;if(!ж(ref β,out ȕ,BROADCAST_SEP)) return false;if(!ж(ref β,out ǿ,BROADCAST_SEP)||(ǿ!=ΖΓ()&&(ǿ!="*"&&Μ!=Κ.ћ))) return false;if(!ж(ref β,out ǿ,BROADCAST_SEP)||(ǿ!=Δή)) return false;
if(!ж(ref β,out Ȏ,BROADCAST_SEP)) return false;return true;}void ΖΥ(){bool ǋ=true;if(ΗΫ){ΗΫ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ΑΟ=(Μ!=Κ.ћ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΟ)
ΖΦ(ΑΟ);String[]Ξ=Me.CustomData.Split('\n');ƃ=bool.Parse(ΒΒ(ΑЁ(Ξ,"Enable_Broadcast",ref ǋ)));bool Εφ=false;bool ΗΟ=true;if(ƃ){if(Μ!=Κ.ћ)
ơ=ΒΒ(ΑЁ(Ξ,"Ship_Name",ref ǋ)).Replace(BROADCAST_SELF_ID,"");Δή=ΒΒ(ΑЁ(Ξ,"Broadcast_Channel",ref ǋ)).ToLower();ΗΟ=false;if(Ǳ==null){Ǳ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǳ.SetMessageCallback("");}List<IMyRadioAntenna>ž=new List<IMyRadioAntenna>();χ.GetBlocksOfType(ž);bool ΔΠ=false;
for(int i=0;i<ž.Count;i++)
if(ž[i].EnableBroadcasting&&ž[i].Enabled){ΔΠ=true;break;}if(ž.Count==0) Λ="No Antenna found";else if(!ΔΠ) Λ="Antenna not ready";Εφ=ž.Count==0||!ΔΠ;if(Ζώ&&!Εφ&&Μ!=Κ.ћ) Λ="Antenna ok";}else if(Μ==Κ.ћ)
Λ="Offline - Enable in PB custom data";Ζώ=Εφ;if(ΗΟ){if(Ǳ!=null) this.IGC.DisableBroadcastListener(Ǳ);Ǳ=null;}}catch{ǋ=false;}if(!ǋ){Λ="Reset custom data";ΖΦ(ΑΟ);}}void ΖΦ(String ΑΟ){String Θ=ΑΟ+"\n\n"+"Enable_Broadcast="+(Μ==Κ.ћ?"true":"false")+" \n";Θ+=Μ!=Κ.ћ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Θ+"Broadcast_Channel=#default";}String ΒΒ(String ś){int Β=ś.IndexOf("//");if(Β!=-1) ś=ś.Substring(0,Β);String[]τ=ś.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String ΑЁ(String[]Ξ,String ђ,ref bool Ā){foreach(String ś in Ξ)
if(ś.StartsWith(ђ)) return ś;Ā=false;return "";}void ΘЅ(string Ф){if(Ē) Н=new List<ϊ>();String ȕ="";String Ȏ="";if(!Γχ(Ф)) Ιθ(Ф);bool ΘΙ=false;int ΕΙ=50;while(Ǳ!=null&&Ǳ.HasPendingMessage&&ΕΙ-->0){MyIGCMessage β=Ǳ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;
if(Ζε(ref Θ,out ȕ,out Ȏ)&&ȕ!=""&&ȕ!=BROADCAST_SELF_ID){ϊ Δ=Γζ(ȕ,"");if(Δ==null){Δ=new ϊ(ȕ);Н.Add(Δ);}Δ.Ī(Ȏ,false,false,true);ΙЏ(Δ,Θ);ΘΙ=true;}}if(ΘΙ)
Н.Sort(delegate(ϊ a,ϊ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(Ę||Ē){if(Αγ<=0||Ē){Λ="";Ē=false;Ις();ΖΥ();ΔЌ=DateTime.Now;}}if(ĺ) ΙЇ();if(ĺ) ΙЋ();if(ĺ) ΚΓ();if(ĺ) ΙА();ΙЅ();}void ΙЏ(ϊ Δ,String Θ)
{Δ.ΔЉ=DateTime.Now;String ΑΙ="";ж(ref Θ,out Δ.ΓΝ,BROADCAST_SEP);ж(ref Θ,out Δ.ΰ,BROADCAST_SEP);if(Δ.ΓΝ!=VERSION) return;String Αά="";String[]ƙ=new string[3];ж(ref Θ,out Αά,BROADCAST_SEP);ж(ref Θ,out ƙ[0],BROADCAST_SEP);ж(ref Θ,out ƙ[1],BROADCAST_SEP);ж(ref Θ,out ƙ[2],BROADCAST_SEP);
ж(ref Θ,out Δ.Εγ,BROADCAST_SEP);ж(ref Θ,out Δ.Λ,BROADCAST_SEP);ж(ref Θ,out ΑΙ,BROADCAST_SEP);ж(ref Θ,out Δ.ΒЂ,BROADCAST_SEP);String Θη="",ΗЄ="";ж(ref Θ,out Θη,BROADCAST_SEP);ж(ref Θ,out ΗЄ,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String Ης;if(!ж(ref Θ,out Ης,BROADCAST_SEP)) break;String[]Βφ=Ης.Split('/');
if(Βφ.Count()<3) continue;int Њ=0;if(!int.TryParse(Βφ[2],out Њ)) continue;Δ.Ћ.Add(new м(Βφ[0],Βφ[1],Њ,Щ.Ŵ));}int ΗЈ=0;if(!int.TryParse(ΑΙ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(Αά,out ΗЈ)) Δ.Μ=Κ.Ĭ;if(!float.TryParse(ƙ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ƙ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(ƙ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(Θη,out Δ.Ȍ)) Δ.Ȍ=0;if(!float.TryParse(ΗЄ,out Δ.ȓ)) Δ.ȓ=0;Δ.Μ=(Κ) ΗЈ;Δ.Δξ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Εο=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.Εο+=Δ.Ћ[i].Њ;}ϊ Γζ(String Υ,String ΰ){Υ=Υ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Н.Count;i++){if(Υ!=""&&Н[i].Υ.ToUpper()==Υ) return Н[i];if(ΰ!=""&&Н[i].ΰ!=null&&Н[i].ΰ.ToUpper()==ΰ) return Н[i];}return null;}String ƅ="";ϊ ΖЎ=null;DateTime ΖЏ=DateTime.MinValue;List<ϊ>Đ=new List<ϊ>();const float Θφ=20f;bool Ζξ(ϊ η,String Ѕ){return Ѕ==ƅ&&η==ΖЎ&&((DateTime.Now-ΖЏ).TotalSeconds<Θφ||
(Ѕ=="HOMEPOS"&&(ȱ.Count>0||Ƕ!=null)));}String ΘЏ(ϊ η){if(ƅ==""||!Ζξ(η,ƅ)) return "";String s="";int Ā=0,Εή=0;for(int i=0;i<Đ.Count;i++){Љ ΓΑ=Đ[i].Ī(ƅ,false,false,false);if(ΓΑ==Љ.ƹ) Ā++;bool Αΰ=ȱ.Contains(Đ[i]);if(Εή++<8) s+="  "+Đ[i].ΰ+": "+(Αΰ?"queued ("+(ȱ.IndexOf(Đ[i])+1)+")":ΓΑ==Љ.ƹ?"received":
ΓΑ==Љ.Ź?"pending...":ΓΑ==Љ.ΑА?"no answer!":"-")+"\n";}if(Εή>8) s+="  +"+(Εή-8)+" more\n";return "--------------------------------------------\n"+ƅ+": "+Ā+"/"+Đ.Count+" received\n"+s;}void Ǯ(ϊ Δ,String Ѕ){ƅ=Ѕ;ΖЎ=Δ;ΖЏ=DateTime.Now;Đ.Clear();if(Δ==null) Đ.AddRange(Н);else Đ.Add(Δ);
if(Δ==null){ȱ.Clear();Ƕ=null;}else ȱ.Remove(Δ);if(Δ==null&&Ѕ=="HOMEPOS"){Ιϊ();return;}if(Δ==null){for(int i=0;i<Н.Count;i++)
Н[i].Ī(Ѕ,true,false,false);ΒΖ(Ѕ,"*",Ѕ);}else{Δ.Ī(Ѕ,true,false,false);ΒΖ(Ѕ,Δ.Υ,Ѕ);}}void Ιθ(string Ф){if(Ф=="") return;var τ=Ф.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ȩ=τ.ElementAtOrDefault(1);String Ȳ="Invalid argument: "+Ф;if(ύ==Σ.Ƽ){if(М!=null)
{switch(Ѕ){case "UP":{if(Ў<2) break;if(М.Ў==0){Ў=1;return;}Ǯ(М,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){Ǯ(М,"MRES");break;}Ǯ(М,"DOWN");break;}case "APPLY":{if(Ў<2) break;Ǯ(М,"APPLY");return;}}}}switch(Ѕ){case "UP":this.Γό(false);return;case "DOWN":this.Γϋ(false);return;
case "APPLY":this.Ű(true);return;}switch(Ѕ){case "CLEAR":ΘϏ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ȩ)){Λ="Usage: SENDALL <cmd>";return;}Ǯ(null,Ȩ);return;case "SEND":
if(Ф.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Ιυ(Ф.Remove(0,"SEND".Length+1));return;}Λ=Ȳ;}void Ιυ(String ǃ){if(ǃ=="") return;var τ=ǃ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String ơ=τ.ElementAtOrDefault(0).Trim();
ϊ Δ=Γζ("",ơ);if(Δ!=null) Ǯ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+ơ;}void ΘϏ(){Н.Clear();М=null;ν(Σ.Main);Ў=0;}void Ις(){List<IMyTerminalBlock>Ακ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ч,Ǚ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ακ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=Ǭ(Ч,pamTag,true);Ζμ(Ακ);Ζλ();Ισ();Ŋ(Ч,setLCDFontAndSize,1.15f,false);Ŋ(Ș,setLCDFontAndSize,1.15f,true);String Κο=ΖΘ(false);String ΗЍ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ш in Ч){if(ш.CustomData==""){ш.CustomData=ΖΘ(true);continue;}
if(!ш.CustomData.Contains(Κο))
if(!ш.CustomData.Contains(ΗЍ)) ш.CustomData=ΗЍ+ш.CustomData;}}void ΙЅ(){String ΑΖ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ș.Count;i++)
Ș[i].WriteText(ΕЀ(У.Ζή,"0",false));for(int i=0;i<Ч.Count;i++){У Џ=У.Ĭ;String Ǽ="";Ιγ(Ч[i],out Џ,out Ǽ);Ч[i].WriteText(ΕЀ(Џ,Ǽ,i==0));}Echo(ΑΖ);for(int i=0;i<Œ.Count;i++){IMyTextPanel ш=Œ[i];String Θ=ш.CustomData.ToUpper();if(Θ=="DEBUG") ш.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ш.WriteText(ΖΗ());
}}String Ű(bool Ι){int Κδ=0;return Ű(Ι,0,ref Κδ,false,1);}String Ű(bool Ι,int ΚЊ,ref int ΛΘ,bool ΗΑ,int Αή){String Γ="";String Ơ="——————————————————\n";String ΛΨ="--------------------------------------------\n";if(М==null||ύ==Σ.Main||ύ==Σ.ΓЏ)
Γ+="[PAM]-Controller | "+Н.Count+" Connected ships"+"\n"+ΙΑ()+ΙΖ()+ΙΛ();else Γ+=ΖΛ(М)+"\n";Γ+=Ơ;int Β=0;int Η=Ў;ŝ=0;if(Λ!=""){Ν(ref Γ,Η,Β++,Ι,Λ);}else if(ύ==Σ.Main){if(М==null&&Н.Count>=1) М=Н[0];String Ѝ="";if(Ν(ref Ѝ,Η,Β++,Ι," Send command to all")) ν(Σ.ΓЏ);
int Ɵ=0;for(int s=0;s<Н.Count;s++){if(ΗΑ) Ѝ+="\n";ϊ Δ=Н[s];if(Η==Β||Η==Β+1) М=Δ;if(Β==Η) Ɵ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+ΖΛ(Δ))){М=Δ;ν(Σ.Ƽ);М.Ī("",false,true,false);}if(Β==Η) Ɵ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+(ΗΑ?"":"  ")+ΙΘ(Δ)))
{М=Δ;ν(Σ.Ζν);}}int ƫ=Αή-2;ƫ+=Math.Max(0,(ΚЊ-1))*Αή;Γ+=ΓЎ(ƫ,Ѝ,Ɵ,ref ΛΘ);}else if(ύ==Σ.ΒΗ||ύ==Σ.ΓЏ){Σ ΚΜ=ύ==Σ.ΒΗ?Σ.Ƽ:Σ.Main;ϊ η=null;if(ύ==Σ.ΒΗ) η=М;if(Ν(ref Γ,Η,Β++,Ι," Back")){ν(ΚΜ);}if(Ν(ref Γ,Η,Β++,Ι," [Stop] "+ų(η,"STOP"))){Ǯ(η,"STOP");}if(Ν(ref Γ,Η,Β++,Ι," [Continue] "+ų(η,"CONT"))){Ǯ(η,"CONT");}
if(Ν(ref Γ,Η,Β++,Ι," [Move home] "+ų(η,"HOMEPOS"))){Ǯ(η,"HOMEPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Move to job] "+ų(η,"JOBPOS"))){Ǯ(η,"JOBPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Full simulation] "+ų(η,"FULL"))){Ǯ(η,"FULL");}if(η!=null&&η.Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," [Undock] "+ų(η,"UNDOCK"))){Ǯ(η,"UNDOCK");}Γ+=ΘЏ(η);}else if(ύ==Σ.Ƽ){String ΔΨ="";if(Ν(ref ΔΨ,Η,Β++,Ι," Back")){ν(Σ.Main);}Γ+=ΔΨ.Substring(0,ΔΨ.Length-1).PadRight(36,' ');if(Ν(ref Γ,Η,Β++,Ι," Send cmd")){ν(Σ.ΒΗ);}if(!М.Γψ()&&!М.Γτ()) ŝ++;
if(М.Γτ()) Γ+="No answer received...";else Γ+=Ż(М,М.ΒЂ);}else if(ύ==Σ.Ζν){if(Ν(ref Γ,Η,Β++,Ι," Back")){ν(Σ.Main);}Γ+=ΛΨ;Γ+=Ż(М,ΖΚ(М));}if(!ΕΘ) Ў=Math.Min(ŝ-1,Ў);return Γ;}String ΖΛ(ϊ Δ){if(Δ.ΓΝ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ż(Δ,Δ.Εγ)+" "+Δ.Δξ+"m";
}String ΙΘ(ϊ Δ){String s=ΓЋ("",К(Δ.Ȍ,Δ.ȓ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;м Ϗ=Δ.Ћ[i];s+=Ζψ(ΓΩ(ΙΕ(Ϗ)),3)+" "+ΖΑ(Ϗ.Њ)+" ";}return s;}String ΙΕ(м Ϗ){if(Ϗ.υ.ToUpper()=="ORE"||Ϗ.υ.ToUpper()=="INGOT"){String ΖА=ΙΓ(Ϗ.ΰ.ToUpper());if(ΖА!="") return ΖА;
}return Ϗ.ΰ;}String Ż(ϊ Δ,String Γ){if(Δ.Γψ()) return "No signal...("+ΑЄ((int)(DateTime.Now-Δ.ΔЉ).TotalSeconds)+")";return Γ;}String ΖΘ(bool Λξ){String Γ="";if(Λξ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void Ιγ(IMyTerminalBlock ш,out У Џ,out String Ǽ){bool Ā=true;String Αά=ΒΒ(ΑЁ(ш.CustomData.Split('\n'),"mode",ref Ā)).ToUpper();String[]τ=Αά.Split(':');String ΑΫ=τ.Length>0?τ[0].Trim():"";Ǽ=τ.Length>1?τ[1].Trim():"";
Џ=У.Ĭ;if(ΑΫ=="MAIN") Џ=У.Ζί;else if(ΑΫ=="MAINX") Џ=У.Ζΰ;else if(ΑΫ=="MENU") Џ=У.Γϊ;else if(ΑΫ=="INVENTORY") Џ=У.Γσ;else if(ΑΫ=="DEBUG") Џ=У.Γέ;if(Џ==У.Ĭ) Ǽ=Αά;}String ΕЀ(У Џ,String Ǽ,bool Κζ){int ΑΨ=15;if(Κζ){ΗΓ=ΒΩ;ΒΩ=0;ΗΔ=ΒΪ;ΒΪ=0;}String Γ="";if(Џ==У.Ζί)
{int ч=0;Γ=Ű(false,ΗΓ,ref ΚΧ,true,ΑΨ);if(!int.TryParse(Ǽ,out ч)) return Γ;ΒΩ=Math.Max(ч,ΒΩ);ч--;return Ζδ(ч,ΑΨ,Γ);}else if(Џ==У.Ζΰ){int ч=0;Γ=Ű(false,ΗΔ,ref ΚΨ,false,ΑΨ);if(!int.TryParse(Ǽ,out ч)) return Γ;ΒΪ=Math.Max(ч,ΒΪ);ч--;return Ζδ(ч,ΑΨ,Γ);}if(Џ==У.Ζή){return Ű(false,0,ref ΚΩ,true,ΑΨ);
}else if(Џ==У.Γσ||Џ==У.Γϊ||Џ==У.Γέ){ϊ Δ=М;if(Ǽ!=""){Δ=Γζ("",Ǽ);if(Δ==null) return "Unknown ship: "+Ǽ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ơ="—————————————————————\n";String Εμ="";String Δέ="";if(Џ==У.Γϊ){Εμ="Menu";Δέ=Ż(Δ,Δ.ΒЂ);}
else if(Џ==У.Γσ){Εμ="Inventory";Δέ=Ż(Δ,ΖΚ(Δ));}else if(Џ==У.Γέ){return Ż(Δ,ΙΙ(Δ));}Γ+=Δ.ΰ+" - "+Δ.Δξ+"m | "+ΓΩ(""+Εμ)+"\n"+Ơ;Γ+=Δέ;return Γ;}return "Unknown command: "+Ǽ;}string ΙΙ(ϊ Δ){String s="";s+=Δ.ΓΝ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Εγ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.ΒЂ.Length+"\n";s+=Δ.Ȍ+"\n";s+=Δ.ȓ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΖΚ(ϊ Δ){String s="";s+=ΓЋ("All",К(Δ.Ȍ,Δ.ȓ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){м Ϗ=Δ.Ћ[i];
s+=ΓЋ(Ϗ.ΰ,Ϗ.Њ,Δ.Εο,30,8,10)+"\n";}return s;}Љ ΘА(ϊ Δ,bool а,string Ѕ){Љ ί=Љ.None;if(Δ==null){for(int i=0;i<Н.Count;i++){Љ І=Н[i].Ī(Ѕ,false,а,false);if(І==Љ.ƹ) ί=І;if(І==Љ.ΑА) return І;if(І==Љ.Ź) return І;}return ί;}else return Δ.Ī(Ѕ,false,а,false);}String ų(ϊ Δ,String Ѕ)
{if(!Ζξ(Δ,Ѕ)) return "";Љ І=ΘА(Δ,false,Ѕ);if(І==Љ.ƹ) return "received!";if(І==Љ.Ź) return "pending...";if(І==Љ.ΑА) return "no answer!";return "";}String ΓЎ(int ƫ,String Γ,int Ɵ,ref int Ğ){String[]Ξ=Γ.Split('\n');if(Ɵ>=Ğ+ƫ-1) Ğ++;Ğ=Math.Min(Ξ.Count()-1-ƫ,Ğ);if(Ɵ<Ğ+1) Ğ--;
Ğ=Math.Max(0,Ğ);String ί="";for(int i=0;i<ƫ;i++){int Ηϋ=i+Ğ;if(Ηϋ>=Ξ.Count()) break;ί+=Ξ[Ηϋ]+"\n";}return ί;}String Ζδ(int ч,int Αή,String Γ){String[]Ξ=Γ.Split('\n');int Αη=ч*Αή;int đ=(ч+1)*Αή;String ί="";for(int i=Αη;i<đ;i++){if(i>=Ξ.Count()) break;ί+=Ξ[i]+"\n";
}return ί;}String ΓΩ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζψ(String s,int ΗЂ){if(s=="") return s;if(s.Length>ΗЂ) s=s.Substring(0,ΗЂ-1)+".";return s;}string ΓЋ(String Ś,float value,float Ƒ,int ΖЅ,int Ηφ,int ΚЉ){float ţ=К(value,Ƒ)*ΖЅ;
String ΒΦ="[";for(int i=0;i<ΖЅ;i++){if(i<=ţ) ΒΦ+="|";else ΒΦ+="'";}ΒΦ+="]";return ΒΦ+" "+Ζψ(ΓΩ(Ś),Ηφ).PadRight(Ηφ)+"".PadRight(ΚЉ)+ΖΑ(value);}String ΖΑ(float Њ){if(Њ>=1000000) return Math.Round(Њ/1000000f,Њ/1000000f<100?1:0)+"M";if(Њ>=1000) return Math.Round(Њ/1000f,Њ/1000f<100?1:0)+"K";
return ""+Math.Round(Њ);}String ΑЄ(int Αδ){if(Αδ>=60*60) return Math.Round(Αδ/(60f*60f),1)+" h";if(Αδ>=60) return Math.Round(Αδ/60f,1)+" min";return ""+Αδ+" s";}string Ų(float v){return Math.Round(v,2)+" ";}string Λυ(Vector3 v){return "X"+Ų(v.X)+"Y"+Ų(v.Y)+"Z"+Ų(v.Z);}
List<IMyCameraBlock>Ȫ=new List<IMyCameraBlock>();List<long>ƕ=new List<long>();bool Ɨ=false;int Δγ=0;int Δα=0;DateTime ΖЇ=DateTime.Now;String Δά="";void Ιρ(){Ȫ.Clear();ƕ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȫ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȫ.Count;i++){Ȫ[i].EnableRaycast=true;if(!ƕ.Contains(Ȫ[i].CubeGrid.EntityId)) ƕ.Add(Ȫ[i].CubeGrid.EntityId);}if(!ƕ.Contains(Me.CubeGrid.EntityId)) ƕ.Add(Me.CubeGrid.EntityId);}bool ΖΩ(Α s){return s==Α.ȟ||s==Α.ф||s==Α.Й||
s==Α.ĩ||s==Α.ű||s==Α.Ň||s==Α.ΑЂ||
(s==Α.ȃ&&Ǣ!=null);}void Αύ(){Ɨ=false;Δγ=0;Δά="";}void ΙЍ(){if(!obstacleBrake||Ȫ.Count==0||!ΖΩ(Ϊ)){Αύ();return;}Vector3 ΑΚ=Ѓ+Ŀ;Vector3 ș=ΑΚ-δ;float ȗ=ș.Length();if(ȗ<1f){Αύ();return;}Vector3 ς=ș/ȗ;float ΗϏ=ğ*obstacleLookAhead+μ;Vector3 ΚБ=ΑΚ+ς*(μ*0.25f);
for(int n=0;n<Ȫ.Count;n++){Δα=(Δα+1)%Ȫ.Count;IMyCameraBlock ȉ=Ȫ[Δα];if(!ȉ.IsFunctional||!ȉ.IsWorking) continue;Vector3 ΖЉ=ȉ.GetPosition();Vector3 đ=ΚБ;if(Vector3.Distance(ΖЉ,đ)>ΗϏ) đ=ΖЉ+ς*ΗϏ;if(!ȉ.CanScan(đ)) continue;MyDetectedEntityInfo ȯ=ȉ.Raycast(đ);bool ΚЅ=!ȯ.IsEmpty()&&!ƕ.Contains(ȯ.EntityId)&&!ΙΫ(ȯ.EntityId)&&
ȯ.Type!=MyDetectedEntityType.FloatingObject;if(ΚЅ){if(!Ɨ) ΖЇ=DateTime.Now;Ɨ=true;Δγ=0;Δά=ȯ.Type.ToString();}else if(Ɨ&&++Δγ>=3) Αύ();break;}if(Ɨ&&(DateTime.Now-ΖЇ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+Δά;}const String ΔΚ="[PAMTFC]";const float ΙϏ=5f;const float Γε=2f;class ī{public long Υ;public Vector3 γ,љ;public Vector3 Ĕ;public float ΗΞ;public String Φ="";public int ĕ=-1;public bool Αΰ;public int Ǹ;public float ΑΜ,ΑΘ;public long α=0;public int ΑΒ=0;public DateTime ľ;
}IMyBroadcastListener ΓΘ=null;long Ǔ=0;List<ī>і=new List<ī>();Vector3 Ŀ=new Vector3();float ǖ=-1;Vector3 Επ=new Vector3();bool ΓΗ=false;DateTime Ερ=DateTime.Now;const float Ζχ=15f;const float Ιώ=10f;int Θδ=0;String г="";int Г=-1;bool Δη=false,Βά=false;Vector3 ΗΕ=new Vector3();
String Ŷ(){if(ε==Ρ.ю||Μ==Κ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Э+"|"+(int) ř+"|"+Math.Round(ȁ,1)+"|"+Math.Round(ț,1);}void ΚΑ(){if(!trafficEnabled) return;if(ΓΘ==null) ΓΘ=IGC.RegisterBroadcastListener(ΔΚ);Ǔ=Me.CubeGrid.EntityId;int Ȉ=30;while(ΓΘ.HasPendingMessage&&Ȉ-->0){String Θ=ΓΘ.AcceptMessage().Data as String;if(Θ!=null) Ιε(Θ);
}for(int i=і.Count-1;i>=0;i--)
if((DateTime.Now-і[i].ľ).TotalSeconds>ΙϏ) і.RemoveAt(i);if(ΓΔ==null) ΓΔ=IGC.RegisterBroadcastListener(Ζυ);Ȉ=5;while(ΓΔ.HasPendingMessage&&Ȉ-->0){String Θ=ΓΔ.AcceptMessage().Data as String;if(Θ!=null) try{Ιζ(Θ);}catch{}}if(Βέ==null) Βέ=IGC.RegisterBroadcastListener(ΖΫ);
Ȉ=10;while(Βέ.HasPendingMessage&&Ȉ-->0){String Θ=Βέ.AcceptMessage().Data as String;if(Θ!=null) try{Ιβ(Θ);}catch{}}if(++Θδ<3) return;Θδ=0;IGC.SendBroadcastMessage(ΔΚ,Ǔ+";"+σ(δ)+";"+
σ(Ϋ.GetShipVelocities().LinearVelocity)+";"+μ+";"+Ŷ()+";"+
(ε==Ρ.И?Г:-1)+";"+г+";"+Θώ()+","+ΕЎ()+";"+σ(Επ)+
";"+(Βζ?1:0)+";"+(Ķ&&ε==Ρ.И?1:0)+","+ΒЌ+","+Εά+","+Ιή());}void Ιε(String Θ){try{String[]f=Θ.Split(';');if(f.Length<7) return;long Υ=long.Parse(f[0]);if(Υ==Ǔ) return;ī Δ=null;for(int i=0;i<і.Count;i++) if(і[i].Υ==Υ) Δ=і[i];if(Δ==null){Δ=new ī();Δ.Υ=Υ;і.Add(Δ);}
Δ.γ=π(f[1]);Δ.љ=π(f[2]);Δ.ΗΞ=float.Parse(f[3]);Δ.Φ=f[4];Δ.ĕ=int.Parse(f[5]);Δ.ľ=DateTime.Now;if(Δ.Φ!=""&&Δ.Φ==Ŷ()) Ζα(f[6]);Δ.α=0;Δ.ΑΒ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ΑΒ=int.Parse(d[1]);}Δ.Ĕ=f.Length>=9?π(f[8]):new Vector3();
Δ.Αΰ=f.Length>=10&&f[9]=="1";Δ.Ǹ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ΑΜ=float.Parse(w[1]);Δ.ΑΘ=float.Parse(w[2]);Δ.Ǹ=int.Parse(w[3]);}}}catch{}}bool ΙΫ(long Υ){for(int i=0;i<і.Count;i++) if(і[i].Υ==Υ) return true;return false;}Vector3 ΖΙ(int Β,int ΚЋ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();ά έ=Ψ[Β];Vector3 Λε=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*ΚЋ;Vector3 Б=έ.φ.LengthSquared()>0.01f?-Vector3.Normalize(έ.φ):-έ.θ;Vector3 с=Vector3.Cross(Λε,Б);if(с.LengthSquared()<0.0001f) return new Vector3();
float ϋ=laneOffset*μ;float Κθ=Math.Min(Vector3.Distance(έ.Ε,Ψ[0].Ε),
Vector3.Distance(έ.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(с)*ϋ*Math.Min(1,Κθ/(ϋ*4));}Vector3 Θγ=new Vector3();bool ΖΡ(){Α s=Ϊ;return Ŷ()!=""&&(s==Α.ȟ||s==Α.ŷ||s==Α.Ȥ||
s==Α.ȣ||s==Α.Ň);}int Ιή(){if(!ΖΡ()) return 0;return Ϊ!=Α.ȟ&&Vector3.Dot(δ-є,ǒ)>μ*0.5f?2:1;}bool ΙΪ(Α s){return s==Α.ȃ||s==Α.ł||s==Α.ņ||s==Α.ƺ||
s==Α.ǰ||s==Α.Ȥ||s==Α.ȣ||s==Α.Ň||s==Α.ĥ;}void ΚΒ(){Ŀ=new Vector3();ǖ=-1;Επ=new Vector3();bool ΑΛ=ΖΩ(Ϊ);if(!trafficEnabled||(!ΑΛ&&!ΙΪ(Ϊ))){ΓΗ=false;return;}Θγ=Ϋ.GetShipVelocities().LinearVelocity;ΘЀ(Ϋ.GetNaturalGravity(),!ΑΛ);Θξ();}IMyBroadcastListener ΓΔ=null;
float ƥ=-1;DateTime Θί=DateTime.MinValue;void Ιζ(String Θ){if(!Θ.StartsWith("C;")) return;Θί=DateTime.Now;ƥ=-1;String ΑΪ=Ǔ+":";String[]e=Θ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΪ)) ƥ=float.Parse(e[i].Substring(ΑΪ.Length));}void Θξ(){if(ƥ<0||(DateTime.Now-Θί).TotalSeconds>1.5) return;
ǖ=ǖ<0?ƥ:Math.Min(ǖ,ƥ);if(ƥ<0.5f) Λ="Traffic: giving way (controller)";}void ΘЀ(Vector3 φ,bool ΘΑ=false){Ŀ=new Vector3();ǖ=-1;Vector3 ș=Ѓ-δ;float ȗ=ș.Length();bool ΔЀ=ȗ>=1f;Vector3 ς=ΔЀ?ș/ȗ:new Vector3();Επ=ς;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ΑΔ;
Vector3 с=Vector3.Cross(ς,Б);с=с.LengthSquared()<0.0001f?-ΑΧ:Vector3.Normalize(с);ī Βψ=null;bool Ηί=Ϊ==Α.Й&&!č(1);for(int i=0;i<і.Count;i++){ī κ=і[i];if(κ.ΑΒ==3&&κ.љ.LengthSquared()<0.25f) continue;Vector3 γ=κ.γ+κ.љ*(float)(DateTime.Now-κ.ľ).TotalSeconds;Vector3 Ǥ=γ-δ;
float а=(μ+κ.ΗΞ)*0.5f;bool Αβ=κ.Φ!=""&&κ.Φ==Ŷ();if(Αβ&&κ.Ǹ==2) continue;bool ΒЏ=Αβ&&κ.Ǹ==1&&ΖΡ();if(ΒЏ) а=Math.Min(ȁ,ț)*0.9f;float Δθ=а*(ΒЏ?1f:1.2f);float ǂ=ΔЀ?Vector3.Dot(Ǥ,ς):0;if(ǂ>0&&(Ǥ-ς*ǂ).Length()<=Δθ){float ΒІ=Vector3.Dot(κ.љ,ς);bool Ηγ=κ.Ĕ.LengthSquared()>0.25f;
bool Ηδ=ΒІ<-1f||(Ηγ&&Vector3.Dot(κ.Ĕ,ς)<-0.5f);if(!ΘΑ){if(κ.Αΰ){if(!Ηί){Ŀ=с*а*1.3f;continue;}float ΗЇ=Vector3.Distance(δ,Ѓ),Ησ=Vector3.Distance(γ,Ѓ);if(ΗЇ<Ησ||(ΗЇ==Ησ&&Ǔ<κ.Υ)) continue;}if(Ηδ){if(ǂ<(ğ-ΒІ)*6f+а*4f) Ŀ=с*а*1.3f;continue;}if(Ηγ&&Ǔ<κ.Υ)
{Vector3 ΖЃ=-Ǥ;float ΗА=Vector3.Dot(ΖЃ,κ.Ĕ);if(ΗА>0&&(ΖЃ-κ.Ĕ*ΗА).Length()<=Δθ){Ŀ=с*а*1.3f;continue;}}}float Ηή=ǂ-а-(ΒЏ?1f:followGap*μ);float ȑ=Math.Max(0,ΒІ)+Math.Max(0,Ηή)/Γε;if(Ηδ) ȑ=Math.Max(0,Ηή+Math.Min(0,ΒІ)*Γε*2)/Γε;if(ǖ<0||ȑ<ǖ){ǖ=ȑ;Βψ=κ;}
continue;}if(κ.љ.LengthSquared()<1f) continue;if(ΔЀ&&(Ǥ-ς*ǂ).Length()<=Δθ) continue;Vector3 ΕΪ=κ.љ-Θγ;float ΘΖ=ΕΪ.LengthSquared();if(ΘΖ<0.25f) continue;float t=-Vector3.Dot(Ǥ,ΕΪ)/ΘΖ;if(t<=0||t>5f||(Ǥ+ΕΪ*t).Length()>=(ΒЏ?а:а*1.1f)) continue;if(Ǔ>κ.Υ||t<2f){ǖ=0;Βψ=κ;}
}bool ΑΠ=ǖ>=0&&ǖ<0.5f;if(!ΑΠ||ΘΑ||(Βψ!=null&&Βψ.Αΰ&&Ηί)){ΓΗ=false;return;}if(!ΓΗ){ΓΗ=true;Ερ=DateTime.Now;}double Ηε=(DateTime.Now-Ερ).TotalSeconds;if(Ηε>Ζχ+Ιώ) Ερ=DateTime.Now;else if(Ηε>Ζχ){ǖ=-1;Ŀ=с*μ*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>Ȱ=new List<int>();String Ηη="";void Γδ(){String ђ=Ќ+"|"+Э+"|"+(int) ř;if(ђ==Ηη) return;Ηη=ђ;ΕЁ(Ќ,Э,ř,Ȱ);}void ΕЁ(int w,int h,я Αη,List<int>Δβ){Δβ.Clear();if(Αη==я.ƾ){for(int k=0;k<w*h;k++){int ƛ=k/w;Δβ.Add(ƛ*w+(ƛ%2==0?k-ƛ*w:w-1-(k-ƛ*w)));
}return;}int ΑΞ=(int) Math.Ceiling(w/2f),ΑΝ=(int) Math.Ceiling(h/2f);int Δύ=w/2,Δό=h/2;int x=0,y=0,Δρ=0,Δς=-1,Ƒ=Math.Max(w,h);for(int i=0;i<Ƒ*Ƒ;i++){if(-ΑΞ<x&&x<=Δύ&&-ΑΝ<y&&y<=Δό) Δβ.Add((y-1+ΑΝ)*w+(x-1+ΑΞ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Δρ;Δρ=-Δς;Δς=t;}
x+=Δρ;y+=Δς;}}bool Γγ(){int n=Math.Max(0,Ќ*Э);if(г.Length==n) return false;г=new String('0',n);Г=-1;return true;}void Ζα(String κ){Γγ();if(κ.Length!=г.Length) return;char[]h=г.ToCharArray();for(int i=0;i<h.Length;i++) if(κ[i]=='1') h[i]='1';г=new String(h);}bool ΑЉ(int Ъ){return Ъ>=0&&Ъ<г.Length&&г[Ъ]=='1';}
void ΓБ(int Ъ,bool Ѐ){if(Ъ<0||Ъ>=г.Length) return;char[]h=г.ToCharArray();h[Ъ]=Ѐ?'1':'0';г=new String(h);}bool Γπ(int Ъ,bool Κφ){if(!trafficEnabled) return false;String ђ=Ŷ();for(int i=0;i<і.Count;i++){ī t=і[i];if(t.Φ!=ђ||t.ĕ<0||(Κφ&&t.Υ>Ǔ)) continue;if(t.ĕ==Ъ) return true;
}return false;}int Θύ(int Β){int n=Ќ*Э;if(n<1) return Β;Γδ();if(Γγ()) for(int k=0;k<Β&&k<n;k++) ΓБ(Ȱ[k],true);if(Г>=0&&(Β>=n||Ȱ[Β]!=Г)){ΓБ(Г,true);Г=-1;}if(Ζά()){if(ȋ>=0&&!ΑЉ(ȋ)){Ķ=false;Г=ȋ;return Ȱ.IndexOf(ȋ);}if(ȋ==-2){Г=-1;Ķ=г.IndexOf('0')>=0;return Ķ?-2:n;}
if(ȋ==-1&&Г>=0&&!ΑЉ(Г)){Ķ=false;return Β;}Г=-1;if(!Ķ){Ķ=true;ΗΗ=DateTime.Now;}if((DateTime.Now-ΗΗ).TotalSeconds<5) return-1;}Ķ=false;if(Г>=0&&!ΑЉ(Г)&&!Γπ(Г,true)) return Β;Г=-1;for(int k=0;k<n;k++){int c=Ȱ[k];if(ΑЉ(c)||Γπ(c,false)) continue;Г=c;
return k;}return г.IndexOf('0')>=0?-2:n;}const String ΖΫ="[PAMJOB]";const String Ζυ="[PAMTCC]";IMyBroadcastListener Βέ=null;DateTime ΗΖ=DateTime.MinValue;DateTime ΗΗ=DateTime.Now;int ȋ=-1;bool Ķ=false;bool Ζά(){return trafficEnabled&&ȋ!=-3&&(DateTime.Now-ΗΖ).TotalSeconds<3;}
void Ιβ(String Θ){String[]f=Θ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ŷ()) return;ΗΖ=DateTime.Now;Ζα(f[2]);ȋ=-3;String ΑΪ=Ǔ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΪ)) ȋ=int.Parse(e[i].Substring(ΑΪ.Length));}ǯ Ιφ(int Β)
{Γδ();if(Β<0||Β>=Ȱ.Count) return ǯ.Αχ;Ё=Ȱ[Β]%Ќ;ώ=Ȱ[Β]/Ќ;return ǯ.ΑЅ;}class ǚ{public long Υ;public Vector3 γ,Ь,Б;}List<ǚ>Х=new List<ǚ>();bool ı=false;Vector3 Βθ,Βη,Βι,Βκ;long ǎ=0;long ā=0;ά Ǣ=null;bool ΑΓ=false;ά Γΰ(Vector3 γ,Vector3 Ь,Vector3 Б){MatrixD m=MatrixD.CreateWorld(γ,Ь,Б);
ά w=new ά(Ο);w.ι=Vector3D.TransformNormal(Βθ,m);w.θ=Vector3D.TransformNormal(Βη,m);w.Ђ=Vector3D.TransformNormal(Βι,m);w.Ε=γ+(Vector3) Vector3D.TransformNormal(Βκ,m);w.λ=true;return w;}int ΕЎ(){Α s=Ϊ;if(s==Α.э) return з(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ņ||s==Α.ȧ||s==Α.Ņ||s==Α.ΒΟ||
s==Α.ΑЀ||s==Α.ǰ) return 3;if(s==Α.ȃ||s==Α.ł||s==Α.ƺ) return 2;if(s==Α.Й) return 1;return 0;}long Θώ(){if(ΕЎ()==0) return 0;if(č(1)&&Ƣ!=0) return Ƣ;return Ǣ!=null?ā:ǎ;}void Ζέ(){if(Μ==Κ.ή||!Ο.λ) return;IMyShipConnector Р=з(MyShipConnectorStatus.Connected);if(Р==null||Р.OtherConnector==null) return;
IMyShipConnector κ=Р.OtherConnector;Vector3I Κπ=new Vector3I((int) Ο.п.X,(int) Ο.п.Y,(int) Ο.п.Z);if(Р.Position==Κπ){MatrixD Ƌ=MatrixD.Transpose(κ.WorldMatrix);Βθ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Forward,Ƌ);Βη=Vector3D.TransformNormal(Ϋ.WorldMatrix.Down,Ƌ);Βι=Vector3D.TransformNormal(Ϋ.WorldMatrix.Left,Ƌ);
Βκ=Vector3D.TransformNormal(Р.GetPosition()-κ.GetPosition(),Ƌ);ı=true;}if(Vector3.Distance(Р.GetPosition(),Ο.Ε)<5) ǎ=κ.EntityId;List<IMyShipConnector>ŕ=new List<IMyShipConnector>();χ.GetBlocksOfType(ŕ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ŕ.Count>0){Х.Clear();for(int i=0;i<ŕ.Count;i++){ǚ p=new ǚ();p.Υ=ŕ[i].EntityId;p.γ=ŕ[i].GetPosition();p.Ь=ŕ[i].WorldMatrix.Forward;p.Б=ŕ[i].WorldMatrix.Up;Х.Add(p);}}Ǣ=null;ā=0;if(κ.EntityId==ǎ||!ı) return;for(int i=0;i<Х.Count;i++)
if(Х[i].Υ==κ.EntityId){ā=Х[i].Υ;Ǣ=Γΰ(Х[i].γ,Х[i].Ь,Х[i].Б);}}bool Γφ(long Υ,int ΗЊ){if(Υ==0) return false;for(int i=0;i<і.Count;i++){ī t=і[i];if(t.α==Υ&&(t.ΑΒ>ΗЊ||(t.ΑΒ==ΗЊ&&t.Υ<Ǔ))) return true;}return false;}bool ΓΫ(int Ţ){if(Μ==Κ.ή||!trafficEnabled){Ǣ=null;ā=0;return true;}
long ο=Ǣ!=null?ā:ǎ;if(!Γφ(ο,Ţ)) return true;if(!Γφ(ǎ,Ţ)){Ǣ=null;ā=0;return true;}if(!ı) return false;ǚ н=null;float ǈ=0;for(int i=0;i<Х.Count;i++){ǚ p=Х[i];if(p.Υ==ǎ||Γφ(p.Υ,Ţ)) continue;float d=Vector3.Distance(p.γ,Ο.Ε);if(н==null||d<ǈ){н=p;ǈ=d;}
}if(н==null) return false;ā=н.Υ;Ǣ=Γΰ(н.γ,н.Ь,н.Б);return true;}String ΘЉ(){String s=(ı?"1":"0")+":"+σ(Βθ)+":"+σ(Βη)+":"+
σ(Βι)+":"+σ(Βκ)+":"+ǎ;for(int i=0;i<Х.Count;i++)
s+=":"+Х[i].Υ+","+σ(Х[i].γ)+","+σ(Х[i].Ь)+","+σ(Х[i].Б);return s;}void ΘЈ(String s){Х.Clear();ı=false;ǎ=0;String[]f=s.Split(':');if(f.Length<6) return;ı=f[0]=="1";Βθ=π(f[1]);Βη=π(f[2]);Βι=π(f[3]);Βκ=π(f[4]);long.TryParse(f[5],out ǎ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǚ p=new ǚ();if(!long.TryParse(e[0],out p.Υ)) continue;p.γ=π(e[1]);p.Ь=π(e[2]);p.Б=π(e[3]);Х.Add(p);}}const String Ʊ="[PAMDCK]";const float Θυ=120f;class Ĉ{public long Δ;public int Ǡ;public long Ǎ;public bool ΖЊ;public DateTime Ǟ,ΑΦ;
}IMyBroadcastListener Βε=null;List<Ĉ>Ц=new List<Ĉ>();List<IMyShipConnector>Ġ=new List<IMyShipConnector>();List<IMySensorBlock>Ć=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΥ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΓΞ=new List<MyDetectedEntityInfo>();long ǜ=0;int Δδ=0;
long ΖЌ=0;DateTime ΖЍ=DateTime.Now;String œ="";int ΗΤ=0;IMyCubeGrid ġ=null;void Ισ(){ġ=Me.CubeGrid;χ.GetBlocksOfType(Ġ,q=>q.CubeGrid.IsSameConstructAs(ġ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ć,q=>q.CubeGrid.IsSameConstructAs(ġ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΥ,q=>q.CubeGrid.IsSameConstructAs(ġ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ć.Count;i++){Ć[i].DetectSmallShips=true;Ć[i].DetectLargeShips=true;}for(int i=0;i<ΑΥ.Count;i++) ΑΥ[i].EnableRaycast=true;}IMyShipConnector Ζπ(long Υ){if(Υ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Υ) as IMyShipConnector;return c!=null&&ġ!=null&&c.CubeGrid.IsSameConstructAs(ġ)?c:null;}bool Γυ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΪ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΥ.Count;i++)
{IMyCameraBlock ȉ=ΑΥ[i];if(!ȉ.IsWorking||Vector3D.Distance(ȉ.GetPosition(),c.GetPosition())>10) continue;Vector3D đ=ȉ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ȉ.CanScan(đ)) continue;MyDetectedEntityInfo ȯ=ȉ.Raycast(đ);if(!ȯ.IsEmpty()&&ȯ.EntityId!=ġ.EntityId&&ȯ.EntityId!=Δ&&ȯ.Type!=MyDetectedEntityType.FloatingObject)
{œ="Lane blocked: "+c.CustomName;return false;}}return true;}bool Ιά(){for(int s=0;s<Ć.Count;s++){if(!Ć[s].IsWorking) continue;ΓΞ.Clear();Ć[s].DetectedEntities(ΓΞ);for(int i=0;i<ΓΞ.Count;i++){MyDetectedEntityInfo e=ΓΞ[i];bool Κλ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Κλ&&e.EntityId!=ǜ&&e.Velocity.Length()>0.5f){œ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Ιη(Ĉ r){IMyShipConnector Ǎ=Ζπ(r.Ǎ);if(Γυ(Ǎ)&&ΖΪ(Ǎ,r.Δ)) return Ǎ.EntityId;if(!r.ΖЊ||Ǎ==null) return 0;IMyShipConnector н=null;double ǈ=0;for(int i=0;i<Ġ.Count;i++)
{IMyShipConnector c=Ġ[i];if(!Γυ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),Ǎ.GetPosition());if(н==null||d<ǈ){if(ΖΪ(c,r.Δ)){н=c;ǈ=d;}}}return н==null?0:н.EntityId;}void ΙΜ(){Ĉ Ƙ=null;long Р=0;for(int Š=2;Š>=1&&Ƙ==null;Š--){Ĉ ş=null;for(int i=0;i<Ц.Count;i++)
{Ĉ r=Ц[i];if(r.Ǡ!=Š) continue;if(Š==1){long c=Ιη(r);if(c==0) continue;if(ş==null||r.Ǟ<ş.Ǟ){ş=r;Р=c;}}else if(ş==null||r.Ǟ<ş.Ǟ) ş=r;}Ƙ=ş;}if(Ƙ==null){if(Ц.Count>0&&œ=="") œ="No free dock";return;}ǜ=Ƙ.Δ;Δδ=Ƙ.Ǡ;ΖЌ=Ƙ.Ǡ==1?Р:0;ΖЍ=DateTime.Now;
}void ΙЇ(){if(ġ==null) return;if(Βε==null) Βε=IGC.RegisterBroadcastListener(Ʊ);int Ȉ=30;while(Βε.HasPendingMessage&&Ȉ-->0){String Θ=Βε.AcceptMessage().Data as String;if(Θ!=null) ΙΝ(Θ);}Ιμ();if(++ΗΤ<3) return;ΗΤ=0;IGC.SendBroadcastMessage(Ʊ,Θσ());}void ΙΝ(String Θ){try{String[]f=Θ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ц.Count-1;i>=0;i--) if(Ц[i].Δ==Δ) Ц.RemoveAt(i);if(ǜ==Δ) ǜ=0;}else if(f[0]=="R"&&f.Length>=5){long Ǎ=long.Parse(f[3]);if(Ζπ(Ǎ)==null) return;Ĉ r=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==Δ) r=Ц[i];if(r==null){r=new Ĉ();r.Δ=Δ;r.Ǟ=DateTime.Now;Ц.Add(r);}
int Ǡ=int.Parse(f[2]);if(r.Ǡ!=Ǡ) r.Ǟ=DateTime.Now;r.Ǡ=Ǡ;r.Ǎ=Ǎ;r.ΖЊ=f[4]=="1";r.ΑΦ=DateTime.Now;}}catch{}}void Ιμ(){for(int i=Ц.Count-1;i>=0;i--)
if((DateTime.Now-Ц[i].ΑΦ).TotalSeconds>3) Ц.RemoveAt(i);Ĉ ΒΨ=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==ǜ) ΒΨ=Ц[i];if(ǜ!=0&&ΒΨ==null) ǜ=0;if(ΒΨ!=null&&(DateTime.Now-ΖЍ).TotalSeconds>Θυ){ǜ=0;ΒΨ.Ǟ=DateTime.Now;}œ="";if(ǜ==0&&Ц.Count>0&&!Ιά()) ΙΜ();
}String Θσ(){String β="S;"+ġ.EntityId+";"+ǜ+";"+Δδ+";"+ΖЌ+";";for(int i=0;i<Ц.Count;i++) β+=(i>0?",":"")+Ц[i].Δ;β+=";";for(int i=0;i<Ġ.Count;i++){IMyShipConnector c=Ġ[i];β+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}β+=";";for(int i=0;i<Ć.Count;i++){IMySensorBlock z=Ć[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();β+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return β;}String ΙΑ()
{if(Ġ.Count==0&&Ć.Count==0) return "";int ΑΛ=0;for(int i=0;i<Ġ.Count;i++) if(Γυ(Ġ[i])) ΑΛ++;return "Docks: "+ΑΛ+"/"+Ġ.Count+" free"+(ǜ!=0?(Δδ==1?" | docking":" | undocking"):"")+
(Ц.Count>1?" | "+(Ц.Count-1)+" waiting":"")+(œ!=""?"\n"+œ:"")+"\n";}IMyBroadcastListener ΓΒ=null;int Ȯ=0;DateTime ΗΣ=DateTime.Now;DateTime ΘΝ=DateTime.MinValue;long ΘΞ=0;int ΘΟ=0;long Ƣ=0;bool Ασ=false;int Δπ=0;List<Vector3>Αυ=new List<Vector3>();List<float>Εύ=new List<float>();
bool Βζ=false;bool Βδ=false;Vector3 ΗΡ=new Vector3();bool ƽ(){return Ȯ!=0&&(DateTime.Now-ΘΝ).TotalSeconds<3;}bool č(int Ǡ){return ƽ()&&ΘΞ==Ǔ&&ΘΟ==Ǡ;}bool Ζρ(){return ƽ()||(DateTime.Now-ΗΣ).TotalSeconds<3;}void ΓЍ(int Ǡ){if(Ȯ==Ǡ) return;Ȯ=Ǡ;ΗΣ=DateTime.Now;Δπ=3;
}void ΓЌ(){if(Ȯ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ʊ,"D;"+Ǔ);Ȯ=0;Ασ=false;}void ΙІ(){if(!trafficEnabled||Μ==Κ.ή) return;if(ΓΒ==null) ΓΒ=IGC.RegisterBroadcastListener(Ʊ);int Ȉ=20;while(ΓΒ.HasPendingMessage&&Ȉ-->0){String Θ=ΓΒ.AcceptMessage().Data as String;if(Θ==null||!Θ.StartsWith("S;")) continue;
try{Ιδ(Θ);}catch{}}if(Ϊ!=Α.Й){Βζ=false;Βδ=false;}if(Ȯ==1&&з(MyShipConnectorStatus.Connected)!=null) ΓЌ();if(Ȯ==2&&!Ασ&&Ϊ!=Α.ǰ&&
Vector3.Distance(δ,ь().Ε)>μ*3) ΓЌ();if(Ȯ!=0&&++Δπ>=3){Δπ=0;IGC.SendBroadcastMessage(Ʊ,"R;"+Ǔ+";"+Ȯ+";"+ǎ+";"+(ı?1:0));}}void Ιδ(String Θ){String[]f=Θ.Split(';');if(f.Length<7) return;bool Ηέ=false;String[]Ζϊ=f[5].Split(',');for(int i=0;i<Ζϊ.Length;i++) if(Ζϊ[i]==Ǔ.ToString()) Ηέ=true;
if(!Ηέ) return;ΘΝ=DateTime.Now;ΘΞ=long.Parse(f[2]);ΘΟ=int.Parse(f[3]);Ƣ=long.Parse(f[4]);Αυ.Clear();Εύ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αυ.Add(π(e[0]));Εύ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗΦ=f[6].Split(':');List<ǚ>Ѝ=new List<ǚ>();for(int i=0;i<ΗΦ.Length;i++){String[]e=ΗΦ[i].Split(',');if(e.Length<4) continue;ǚ p=new ǚ();p.Υ=long.Parse(e[0]);p.γ=π(e[1]);p.Ь=π(e[2]);p.Б=π(e[3]);Ѝ.Add(p);}Х=Ѝ;}bool Θϋ(){if(!trafficEnabled||Μ==Κ.ή||ǎ==0) return true;
ΓЍ(1);if(!Ζρ()) return true;if(!č(1)){Λ="Waiting for dock clearance";return false;}ΓΣ();return true;}bool ΓΣ(){ΑΓ=false;ǚ š=null;if(Ƣ!=0&&Ƣ!=ǎ&&ı)
for(int i=0;i<Х.Count;i++) if(Х[i].Υ==Ƣ) š=Х[i];long Ƭ=š==null?0:š.Υ;if(Ƭ==ā&&(Ƭ==0)==(Ǣ==null)) return false;ā=Ƭ;Ǣ=š==null?null:Γΰ(š.γ,š.Ь,š.Б);return true;}bool ŵ(Vector3 p){for(int i=0;i<Αυ.Count;i++) if(Vector3.Distance(p,Αυ[i])<Εύ[i]+μ*0.75f) return true;
return false;}bool ΙΡ(Vector3 ǌ,Vector3 Ş){Βζ=false;if(!trafficEnabled||Μ==Κ.ή||ǎ==0) return false;if(Ȯ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;ΓЍ(1);if(!ƽ()||č(1)||Αυ.Count==0) return false;if(!ŵ(ǌ)&&!ŵ(Ş)&&!ŵ(δ)) return false;Βζ=true;return true;
}bool ΕЈ(){if(!trafficEnabled||Μ==Κ.ή||ǎ==0) return true;ΓЍ(2);if(!Ζρ()) return true;if(!č(2)){Λ="Waiting for undock clearance";return false;}return true;}const float Ιέ=600f;class Ċ{public long Υ;public int Ъ=-1;public bool Ƭ;public float ΑΜ,ΑΘ;public DateTime ΕΫ;}class Ȣ{public String ђ;
public int w,h;public float Λζ,ΛΧ;public String Ѐ;public List<int>ΕΝ=new List<int>();public List<Ċ>т=new List<Ċ>();}List<Ȣ>Ȑ=new List<Ȣ>();IMyBroadcastListener Βχ=null;int Ητ=0;void ΙЋ(){if(Βχ==null) Βχ=IGC.RegisterBroadcastListener(ΔΚ);int Ȉ=50;while(Βχ.HasPendingMessage&&Ȉ-->0)
{String Θ=Βχ.AcceptMessage().Data as String;if(Θ==null) continue;try{ΙΟ(Θ);}catch{}try{ΙΠ(Θ);}catch{}}for(int j=Ȑ.Count-1;j>=0;j--){Ȣ Φ=Ȑ[j];for(int i=Φ.т.Count-1;i>=0;i--)
if((DateTime.Now-Φ.т[i].ΕΫ).TotalSeconds>Ιέ) Φ.т.RemoveAt(i);if(Φ.т.Count==0){Ȑ.RemoveAt(j);continue;}Θο(Φ);}if(++Ητ<3) return;Ητ=0;for(int j=0;j<Ȑ.Count;j++) IGC.SendBroadcastMessage(ΖΫ,Θς(Ȑ[j]));}void Θο(Ȣ Φ){for(int i=0;i<Φ.т.Count;i++){Ċ s=Φ.т[i];if(s.Ƭ&&(s.Ъ<0||Φ.Ѐ[s.Ъ]=='1')) s.Ъ=Ιΰ(Φ,s);
}}String Θς(Ȣ Φ){String β="J;"+Φ.ђ+";"+Φ.Ѐ+";";bool Ǟ=true;for(int i=0;i<Φ.т.Count;i++){if((DateTime.Now-Φ.т[i].ΕΫ).TotalSeconds>5) continue;β+=(Ǟ?"":",")+Φ.т[i].Υ+":"+Φ.т[i].Ъ;Ǟ=false;}return β;}void ΙΟ(String Θ){String[]f=Θ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;Ȣ Φ=null;for(int i=0;i<Ȑ.Count;i++) if(Ȑ[i].ђ==f[4]) Φ=Ȑ[i];if(Φ==null){Φ=new Ȣ();Φ.ђ=f[4];Φ.w=int.Parse(k[3]);Φ.h=int.Parse(k[4]);Φ.Λζ=float.Parse(k[6]);Φ.ΛΧ=float.Parse(k[7]);if(Φ.w<1||Φ.h<1) return;ΕЁ(Φ.w,Φ.h,(я) int.Parse(k[5]),Φ.ΕΝ);
Φ.Ѐ=new String('0',Φ.w*Φ.h);Ȑ.Add(Φ);}String d=f[6];if(d.Length==Φ.Ѐ.Length){char[]c=Φ.Ѐ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Φ.Ѐ=new String(c);}long Υ=long.Parse(f[0]);Ċ s=null;for(int i=0;i<Φ.т.Count;i++) if(Φ.т[i].Υ==Υ) s=Φ.т[i];
if(s==null){s=new Ċ();s.Υ=Υ;Φ.т.Add(s);}s.ΕΫ=DateTime.Now;String[]w=f[10].Split(',');s.Ƭ=w[0]=="1";if(w.Length>=3){s.ΑΜ=float.Parse(w[1]);s.ΑΘ=float.Parse(w[2]);}int ĕ=int.Parse(f[5]);if(ĕ>=0&&ĕ<Φ.Ѐ.Length&&(s.Ъ<0||Φ.Ѐ[s.Ъ]=='1')&&Φ.Ѐ[ĕ]!='1'&&ΖΠ(Φ,ĕ,s)==null)
s.Ъ=ĕ;}Ċ ΖΠ(Ȣ Φ,int Ъ,Ċ Κε){for(int i=0;i<Φ.т.Count;i++){Ċ o=Φ.т[i];if(o!=Κε&&o.Ъ==Ъ)
return o;}return null;}int Ιΰ(Ȣ Φ,Ċ s){for(int k=0;k<Φ.ΕΝ.Count;k++){int c=Φ.ΕΝ[k];if(Φ.Ѐ[c]!='1'&&ΖΠ(Φ,c,s)==null) return c;}return-2;}String ΙΖ(){String s="";for(int j=0;j<Ȑ.Count;j++){Ȣ Φ=Ȑ[j];int Ѐ=0;for(int i=0;i<Φ.Ѐ.Length;i++) if(Φ.Ѐ[i]=='1') Ѐ++;s+="Job "+(j+1)+": "+Ѐ+"/"+Φ.Ѐ.Length+" holes, "+Φ.т.Count+" ships\n";
}return s;}const float Ιό=8f;class ѝ{public long Υ;public Vector3 γ,љ,Ĕ;public float Βγ,ΑΜ,ΑΘ,ȑ=-1;public int Ǹ;public String Φ="";public int Ţ;public DateTime ľ;}class ΒΛ{public long a,b,Θν;public DateTime ΑΦ;}List<ѝ>З=new List<ѝ>();List<ΒΛ>Ă=new List<ΒΛ>();int Θή=0;void ΙΠ(String Θ)
{String[]f=Θ.Split(';');if(f.Length<9) return;long Υ=long.Parse(f[0]);ѝ s=null;for(int i=0;i<З.Count;i++) if(З[i].Υ==Υ) s=З[i];if(s==null){s=new ѝ();s.Υ=Υ;З.Add(s);}s.γ=π(f[1]);s.љ=π(f[2]);s.Βγ=float.Parse(f[3]);s.Φ=f[4];s.Ţ=int.Parse(f[7].Split(',')[1]);s.Ĕ=π(f[8]);
s.Ǹ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΜ=float.Parse(w[1]);s.ΑΘ=float.Parse(w[2]);s.Ǹ=int.Parse(w[3]);}}s.ľ=DateTime.Now;}bool ΔΙ(ѝ s){return s.љ.LengthSquared()<0.25f&&(s.Ţ==3||s.Ĕ.LengthSquared()<0.25f);}Vector3 Ζφ(ѝ s){return s.љ.LengthSquared()>=1f?s.љ:s.Ĕ*2f;}
void ΚΓ(){ΘЁ();if(++Θή<3) return;Θή=0;IGC.SendBroadcastMessage(Ζυ,Θτ());}void ΘЁ(){for(int i=З.Count-1;i>=0;i--)
if((DateTime.Now-З[i].ľ).TotalSeconds>3) З.RemoveAt(i);for(int i=Ă.Count-1;i>=0;i--)
if((DateTime.Now-Ă[i].ΑΦ).TotalSeconds>2) Ă.RemoveAt(i);for(int i=0;i<З.Count;i++) З[i].ȑ=-1;for(int i=0;i<З.Count;i++)
for(int j=i+1;j<З.Count;j++){ѝ a=З[i],b=З[j];if(ΔΙ(a)||ΔΙ(b)) continue;Vector3 ΚЈ=a.γ+a.љ*(float)(DateTime.Now-a.ľ).TotalSeconds;Vector3 ΚЌ=b.γ+b.љ*(float)(DateTime.Now-b.ľ).TotalSeconds;Vector3 Ƨ=Ζφ(a),ƨ=Ζφ(b);Vector3 Ǥ=ΚЌ-ΚЈ;float ϋ=Ǥ.Length();float а=(a.Βγ+b.Βγ)*0.55f;
bool Αβ=a.Φ!=""&&a.Φ==b.Φ;if(Αβ&&(a.Ǹ==2||b.Ǹ==2)) continue;if(Αβ&&a.Ǹ==1&&b.Ǹ==1){String[]k=a.Φ.Split('|');if(k.Length>=8) а=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ΕΩ=ƨ-Ƨ;float ΘΕ=ΕΩ.LengthSquared();float t=ΘΕ>0.01f?-Vector3.Dot(Ǥ,ΕΩ)/ΘΕ:0;
bool Εξ=ϋ<а;if(!Εξ&&(t<=0||t>Ιό||(Ǥ+ΕΩ*t).Length()>=а)) continue;if(!Εξ&&Ƨ.LengthSquared()>0.25f&&ƨ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ƨ),Vector3.Normalize(ƨ))<-0.7f)
continue;ΒΛ p=null;for(int k=0;k<Ă.Count;k++)
if((Ă[k].a==a.Υ&&Ă[k].b==b.Υ)||(Ă[k].a==b.Υ&&Ă[k].b==a.Υ)) p=Ă[k];if(p==null){p=new ΒΛ();p.a=a.Υ;p.b=b.Υ;p.Θν=Ιύ(a,b,Ƨ,ƨ,Ǥ,ϋ);Ă.Add(p);}p.ΑΦ=DateTime.Now;ѝ y=p.Θν==a.Υ?a:b;float Ηϊ=Εξ||t<4?0:Math.Max(1,y.љ.Length()*0.5f);if(y.ȑ<0||Ηϊ<y.ȑ) y.ȑ=Ηϊ;
}}long Ιύ(ѝ a,ѝ b,Vector3 Ƨ,Vector3 ƨ,Vector3 Ǥ,float ϋ){if(a.Υ==ǜ) return b.Υ;if(b.Υ==ǜ) return a.Υ;Vector3 n=ϋ>0.01f?Ǥ/ϋ:new Vector3(1,0,0);float ΘΡ=Vector3.Dot(Ƨ,n),Θέ=Vector3.Dot(ƨ,-n);if(ΘΡ-Θέ>1) return a.Υ;if(Θέ-ΘΡ>1) return b.Υ;return a.Υ>b.Υ?a.Υ:b.Υ;
}String Θτ(){String β="C;";bool Ǟ=true;for(int i=0;i<З.Count;i++){if(З[i].ȑ<0) continue;β+=(Ǟ?"":",")+З[i].Υ+":"+Math.Round(З[i].ȑ,1);Ǟ=false;}return β;}String ΙΛ(){int ΗЉ=0,ΑΠ=0;for(int i=0;i<З.Count;i++){if(!ΔΙ(З[i])) ΗЉ++;if(З[i].ȑ>=0) ΑΠ++;}if(З.Count==0) return "";
return "Traffic: "+ΗЉ+" moving"+(ΑΠ>0?", "+ΑΠ+" giving way":"")+"\n";}List<ϊ>ȱ=new List<ϊ>();ϊ Ƕ=null;Vector3 Ηλ;DateTime Ηκ;float Ηι=40;bool Ηθ=false;ѝ ΕБ(ϊ s){ѝ н=null;for(int i=0;i<З.Count;i++)
if(Vector3.Distance(З[i].γ,s.Ε)<15&&(н==null||
Vector3.Distance(З[i].γ,s.Ε)<Vector3.Distance(н.γ,s.Ε))) н=З[i];return н;}float ΙΣ(ϊ s){ѝ t=ΕБ(s);if(t==null) return 0;float k=t.Ǹ*1000000f;String[]ђ=t.Φ.Split('|');if(ђ.Length>=3) k+=Vector3.Distance(t.γ,new Vector3(float.Parse(ђ[0]),float.Parse(ђ[1]),float.Parse(ђ[2])));return k;
}void Ιϊ(){ȱ.Clear();Ƕ=null;List<float>ΔЇ=new List<float>();for(int i=0;i<Н.Count;i++){float k=ΙΣ(Н[i]);int ǅ=0;while(ǅ<ΔЇ.Count&&ΔЇ[ǅ]<=k) ǅ++;ΔЇ.Insert(ǅ,k);ȱ.Insert(ǅ,Н[i]);}}void ΙА(){if(Ƕ!=null){bool Κκ=Ηθ||Vector3.Distance(Ƕ.Ε,Ηλ)>Ηι||
(DateTime.Now-Ηκ).TotalSeconds>45||Ƕ.Γψ();if(!Κκ) return;Ƕ=null;}if(ȱ.Count==0) return;Ƕ=ȱ[0];ȱ.RemoveAt(0);Ηλ=Ƕ.Ε;Ηκ=DateTime.Now;ѝ t=ΕБ(Ƕ);Ηι=t==null?40:Math.Max(40,t.Βγ*3);Ηθ=t!=null&&t.Ţ==3&&t.љ.LengthSquared()<0.25f;Ƕ.Ī("HOMEPOS",true,false,false);ΒΖ("HOMEPOS",Ƕ.Υ,"HOMEPOS");
}