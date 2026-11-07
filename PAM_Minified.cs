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
public enum Ι{ğ,Ʃ,ǣ,і,έ}public enum Β{ъ,ĝ,ƪ,ȳ,ĺ,И,с,
Ĝ,ţ,Ǹ,Ķ,Ĳ,Ę,
Ƭ,Ț,Ǧ,ĵ,ΑЊ,Αλ,
ΓЇ,ΑЉ,Ƣ,Αν,Ġ}public enum Ρ{ы,Ũ,З,ΓΚ,Αη,Αω,Αϊ,Ş,ş}public enum ѕ{None,ĵ,ъ,Εψ}public enum Σ{Main,ΖΣ,ƨ,Ƨ,ΓЂ,Αε,
ΒЊ,ΕЍ,ΕЎ,ΕЊ,Ʈ,ΖΨ,
ΑЃ,Γω,Γύ,ΖΪ,ΑЅ}public enum у{ΑЂ,Ơ,ΙΞ,ΕБ,έ}public enum ь{ư,Ερ}public enum ш{ΑЁ,ΓΠ,Ζέ,Ƥ}public enum В{Γι,Ευ,ΓΕ,ΓΛ,Αι,ΓΫ,Ασ}public enum Ǒ{ΓΗ,ΒЏ,ΒА}public enum Ш{Ŧ,ΘϏ,ĳ}public enum π{Γλ,Γο,Γπ,ΖΝ,ΖΜ,
Γκ,Αώ,Αύ,Γν,
Γξ,Γμ}public enum δ{ȓ,Ȳ,Ȓ,Αδ,ΒЍ,ΒЎ}public enum ǥ{Απ,Αγ,Θθ}public enum ї{ΑІ,Εϊ,ΘЉ,Γθ}public enum Ј{None,ũ,ƫ,Αό}public enum ϊ{Ʋ,Αβ,Αχ,Ƥ,Γζ,Αψ,Γη,ΙΤ,Γρ}public enum С{ğ,ΖΙ,ΖΘ,Γγ,Γέ,ΓΖ,ΖΚ}class Ϋ{public bool ι=false;public Vector3 Ε=new Vector3();
public Vector3 η=new Vector3();public Vector3 θ=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 φ=new Vector3();public Vector3 м=new Vector3();public float Ч=0;public float ΛΕ=0;public float[]Ƕ=null;public Ϋ(){}public Ϋ(Ϋ λ){ι=λ.ι;Ε=λ.Ε;η=λ.η;θ=λ.θ;Ђ=λ.Ђ;φ=λ.φ;
м=λ.м;Ƕ=λ.Ƕ;}public Ϋ(Vector3 Ε,Vector3 θ,Vector3 η,Vector3 Ђ,Vector3 φ){this.Ε=Ε;this.η=η;this.θ=θ;this.Ђ=Ђ;this.Ч=0;this.φ=φ;}public void ΘΤ(List<IMyThrust>ΑЍ,List<string>Ȫ){Ƕ=new float[Ȫ.Count];for(int i=0;i<Ƕ.Length;i++)
Ƕ[i]=-1;for(int i=0;i<ΑЍ.Count;i++){string ǐ=ΕЈ(ΑЍ[i]);int Α=Ȫ.IndexOf(ǐ);if(Α!=-1)
Ƕ[Α]=Й(ΑЍ[i].MaxEffectiveThrust,ΑЍ[i].MaxThrust);}}}class Γς{public Ϋ ζ=null;public List<Vector3>Ɩ=new List<Vector3>();public float ƽ=0;public float Εί=0;public float Ű=0;public float Δί=0;public Vector3 ΒΣ=new Vector3();}class Ŭ{public π Р=π.Γλ;public float Ǫ=0;public float ħ=0;
public string ǫ="";public string Ǯ="";DateTime Ηά;public bool ΔЏ=false;private bool ΔΡ=false;public bool ΓΒ(bool ǳ){if(ǳ){ħ=0;ΔΡ=false;return false;}ΔΡ=true;return ħ>Ǫ;}public void ΘА(){ΓΒ(true);ΔЏ=false;}public void Ιδ(){if(ΔΡ)
if((DateTime.Now-Ηά).TotalSeconds>1){ħ++;Ηά=DateTime.Now;}}public bool ΓΨ(){switch(Р){case π.Γο:return true;case π.Γπ:return true;}return false;}}class к{public String ί="";public String υ="";public int Љ=0;public Ш ƀ=Ш.ĳ;public к(String ί,String υ,int Љ,Ш ƀ){this.ί=ί;this.υ=υ;
this.Љ=Љ;this.ƀ=ƀ;}}class Θγ{public Θγ(Vector3 ν,float Њ){this.ν=ν;this.Њ=Њ;}public Vector3 ν;public float Њ;}class Є{public DateTime Δσ;public DateTime Ζϊ;public String ΰ="";public String ί="";public String ΒЇ="";public String Λ="";public String ΕΝ="";public String Βκ="";public String ΔЍ="";
public Vector3 Ε=new Vector3();public List<к>Ћ=new List<к>();public Ι Κ=Ι.ğ;public Ј Ģ;public float ȇ;public float ȁ;public int Ў;public int ΔΧ=0;public int ΕΪ=0;public bool ΖΔ(){return(DateTime.Now-Δσ).TotalSeconds>10;}public bool Γή(){if(Ģ!=Ј.ũ) return false;return(DateTime.Now-Ζϊ).TotalSeconds>=4;
}public Є(String ΰ){this.ΰ=ΰ;}public Ј Ĺ(String Ѕ,bool Κλ,bool э,bool Ισ){if(Ѕ==""&&!э) return Ј.None;if(Ģ==Ј.ũ&&Γή())
Ģ=Ј.Αό;if(Κλ){ΔЍ=Ѕ;Ģ=Ј.ũ;Ζϊ=DateTime.Now;return Ģ;}else if(э){ΔЍ="";Ģ=Ј.None;}else if(ΔЍ==Ѕ){if(Ισ) Ģ=Ј.ƫ;return Ģ;}return Ј.None;}}public String Θπ(String ΚΕ){switch(ΚΕ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Βρ=new Vector3();Vector3 γ=new Vector3();Vector3 ΒΦ=new Vector3();Vector3 ΑΕ=new Vector3();Vector3 ǀ=new Vector3();
DateTime Δφ=new DateTime();bool Ć=true;int ΕΤ=0;bool ċ=false;bool ž=false;bool ΑΚ=true;bool ΘΚ=false;bool Ž=false;bool ΒΫ=false;float Ē=0;float Ř=0;int Į=0;int ΑΫ=0;int ΑΞ=0;int ΔΗ=0;float ƃ=0;float ű=0;double ΔЂ=0;float Ʒ=0;List<int>ź=new List<int>();List<int>Ǝ=new List<int>();
Ι Κ=Ι.ğ;int А=0;float Ȑ=0;float ȏ=0;float ο=0;float Βυ=0,Βτ=0;IMyRemoteControl Ω;IMySensorBlock ψ;List<IMyTimerBlock>ΕΧ=new List<IMyTimerBlock>();List<IMyShipConnector>ј=new List<IMyShipConnector>();List<IMyThrust>ȫ=new List<IMyThrust>();List<IMyGyro>ΒΧ=new List<IMyGyro>();
List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();List<IMyLandingGear>ȧ=new List<IMyLandingGear>();List<IMyReactor>ĭ=new List<IMyReactor>();List<IMyConveyorSorter>Βψ=new List<IMyConveyorSorter>();List<IMyGasTank>ǖ=new List<IMyGasTank>();List<IMyTerminalBlock>Ƴ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ŵ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ŀ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ġ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ц=new List<IMyTextPanel>();List<IMyTextSurface>Ȍ=new List<IMyTextSurface>();List<IMyTextPanel>ń=new List<IMyTextPanel>();
IMyTerminalBlock ǚ=null;Ϋ Ο=new Ϋ();Ϋ Δν=new Ϋ();Ϋ Δπ=new Ϋ();bool Ȭ=false;bool Ƙ=false;bool ƙ=false;bool Βέ=false;List<String>Ȫ=new List<string>();List<Ϋ>Φ=new List<Ϋ>();List<Ϋ>Αΰ=new List<Ϋ>();int ΔΨ=0;double ΘΛ=0;Ϋ Ζ=new Ϋ();Vector3 Ǳ;Vector3 ď;Ρ β=Ρ.ы;ь Ō=ь.ư;int Ќ=0;
int Ь=0;double ΑΓ=0;bool ΒΘ=false;Ŭ І=new Ŭ();Ŭ Ї=new Ŭ();ш ё=ш.ΑЁ;bool İ=true;ь Ȋ=ь.ư;int я=3;int ю=3;int л=30;Ǒ љ=Ǒ.ΓΗ;bool ė=true;bool ż=true;bool Ŷ=true;bool ř=false;В б=В.Γι;bool Ė=true;float Ȇ=90;float ѝ=20;float н=5;float ў=20;float ǻ=0.70f;float ƞ=1.50f;
float Ɲ=2.50f;float ŝ=0f;float ň=0f;Β Ϊ;Ρ ќ;Β ΑΤ;Γς μ=null;int Ϗ=0;int ό=0;int Я=0;int č=0;int Ŗ=30;int и=0;bool Д=true;Vector3 џ;double а=0;bool Βσ=false;int Εβ=0;int Е=0;int ΓЎ=0;int Ȉ=0;int Ησ=0;Vector3 ΑΜ=new Vector3();float ѐ=0;float р=0;int Ŝ=0;Ϋ Βω=null;
float Δτ=0;float ΑЏ=0;float ж=0;float ŏ=0;bool Δξ=false;bool Ś=false;bool Αά=false;bool ĉ=false;bool Ă=false;DateTime ΙЄ=new DateTime();Ϋ ł=null;Ϋ ΖЂ=null;float Ȝ=0;ѕ Ǽ;float Ċ=0;float ś=0;String Δύ="";float Βα=0;float ȁ=0;float ȇ=0;List<к>ǭ=new List<к>();float в=0;
bool ΑΠ=false;bool ΗА=false;bool ΗϏ=false;bool Βώ=false;bool ΗЀ=false;float ΔЀ=0;float ΔЌ=0;Vector3 ΗΔ=new Vector3();Vector3 Ѐ=new Vector3();Vector3 ΒϏ=new Vector3(1,1,1);float ΕΣ=1;Vector3 ΑΨ=new Vector3();bool Βά=false;bool Δε=false;bool Δϊ=false;bool Ζζ=false;float ΒΖ=0;
float ΑЌ=2;Vector3 ΗЏ;Vector3 ΕΡ;Vector3 ΕΠ;float[,]Ƿ=new float[3,2];Dictionary<String,float[,]>Ǟ=new Dictionary<string,float[,]>();Dictionary<String,float>ΘΘ=new Dictionary<String,float>();int ΔΑ=0;Dictionary<String,float[]>Đ=new Dictionary<string,float[]>();float ΔА;bool ΒΙ=false;
int ΑЎ=0;int Ī=0;int ΒΩ=0;int Ŋ=0;String Λ="";int[]Ηκ=new int[Enum.GetValues(typeof(Σ)).Length];bool ΔЄ=false;int Ő=0;int Ў=0;Σ ϋ=Σ.Main;int Κω=0,Κϊ=0,Κϋ=0,Κό=0;int Κζ=0,Κη=0;bool ų=false;bool Ζη=false;String Ɠ="";String ΔΖ="";IMyBroadcastListener Ǩ=null;
bool ΗΓ=true;List<Є>Ъ=null;Є Л=null;int ΒΒ=0,ΒΓ=0;int Ζύ=0,Ζώ=0;int Ιω=0,Ιϊ=0,Ιϋ=0;String ΚΗ="";Exception Č=null;bool Βς=false;Ρ Ɛ=Ρ.ы;bool ΖЅ=false;bool ΗΩ=true;bool ΕΔ=false;String ΘΙ="";bool ΑΪ=false;bool ΕΜ=true;int ΒΪ=0;int[]ǜ=null;Vector3 ΔΘ=new Vector3();
bool ΗΙ=false;bool ǒ(IMyTerminalBlock Ψ)=>Ψ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓΩ(Me,controllerTag,true))
Κ=Ι.і;κ(Σ.Main);if(Κ!=Ι.і){Λ="Welcome to [PAM]!";ї ΕΓ=ΘБ();if(Κ==Ι.ğ){List<IMyShipDrill>ΖБ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗΚ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΖБ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗΚ,q=>q.CubeGrid==Me.CubeGrid);if(ΖБ.Count>0)
{Κ=Ι.Ʃ;Λ="Miner mode enabled!";}else if(ΗΚ.Count>0){Κ=Ι.ǣ;Λ="Grinder mode enabled!";}else{Κ=Ι.έ;Ș(у.έ);}}if(ΕΓ==ї.ΑІ) ΑΚ=false;if(ΕΓ==ї.Εϊ) Λ="Data restore failed!";if(ΕΓ==ї.Γθ) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΚЄ){try{if(Č!=null){if(Т.Trim().ToUpper()=="RETRY")
{Č=null;Ć=true;Į=0;if(Κ!=Ι.і) Ё();Λ="Restarted after error";}else{Θϊ();ΖΠ();return;}}ž=(ΚЄ&UpdateType.Update10)!=0;if(ž) ΕΤ++;ċ=ΕΤ>=10;if(ċ) ΕΤ=0;if(ž){ΑЎ++;if(ΑЎ>4) ΑЎ=0;ΑΞ=(int) Math.Max(0,10-(DateTime.Now-Δφ).TotalSeconds);}if(Т!="") ΚΗ=Т;if(Κ!=Ι.і)
ΙΝ(Т);else
Θα(Т);ΔЄ=false;try{int ΖЃ=Runtime.CurrentInstructionCount;float ŕ=Й(ΖЃ,Runtime.MaxInstructionCount);if(ŕ>0.90) Λ="Max. instructions >90%";if(ŕ>ƃ) ƃ=ŕ;if(ΒΙ){ź.Add(ΖЃ);while(ź.Count>10) ź.RemoveAt(0);ű=0;for(int i=0;i<ź.Count;i++) ű+=ź[i];ű=Й(Й(ű,ź.Count),Runtime.MaxInstructionCount);
double Δυ=Runtime.LastRunTimeMs;if(ΒΫ&&Δυ>ΔЂ) ΔЂ=Δυ;Ǝ.Add((int)(Δυ*1000f));while(Ǝ.Count>10) Ǝ.RemoveAt(0);Ʒ=0;for(int i=0;i<Ǝ.Count;i++) Ʒ+=Ǝ[i];Ʒ=Й(Ʒ,Ǝ.Count)/1000f;}}catch{ƃ=0;}}catch(Exception e){Č=e;}}void ΙΝ(string Т){bool Βφ=false;String Ζρ="";if(А<=1&&!Γα(Т))
ΖΡ(Т);int ΔЅ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔЅ-->0){MyIGCMessage ε=Ǩ.AcceptMessage();String Ξ=ε.Data as String;if(Ξ==null) continue;String ȉ="";if(А<=1&&ΖΟ(ref Ξ,out ȉ,out Ζρ)&&ȉ==BROADCAST_SELF_ID){ΖΡ(Ξ);Βφ=true;break;}}bool ΗΣ=ΒΫ&&Ϊ==Β.ъ&&!Ž&&!Βφ&&Į==0&&!Βέ;
if(ċ&&Ϊ!=Β.ъ) Βφ=true;if((ž&&!ΗΣ)||(ċ&&ΗΣ)){if(Į==0&&(ΑΞ<=0||Ć)){ΘΚ=А>0;А=0;Į=1;ș();ΙΔ();ΕЏ();Ȕ("Scan 1");}else if(Į==1){Į=2;ș();ΙΕ();Ȕ("Scan 2");}else if(Į==2){Į=0;ș();ΙΖ();ΙΗ();if(А<=1) ΖΗ();Ȕ("Scan 3");Δφ=DateTime.Now;if(А<=1&&ΑΚ)
Βρ=Αρ(Ω,Ω.CenterOfMass);ΑΚ=false;if(Ć){Ć=false;bool ΚЈ=β==Ρ.З;Ё();if(ΗΩ&&resumeAfterRestart&&ΚЈ&&А<=1) ΕΔ=true;}ΗΩ=false;if(ΘΚ&&А==0) Λ="Setup complete";}else{if(β==Ρ.З&&Κ!=Ι.έ){ș();ΘΝ();Ȕ("Inv balance");}ș();switch(ΑΫ){case 0:ΙΨ();break;case 1:Ιή();break;case 2:ΙΧ();break;
case 3:Ιη();break;case 4:Ιέ();break;case 5:ΘΧ();break;case 6:ΘΦ(Ω);break;}Ȕ("Update: "+ΑΫ);ΑΫ++;if(ΑΫ>6){ΑΫ=0;ΒΫ=true;if(Ɛ!=Ρ.ы){switch(Ɛ){case Ρ.Ş:Ě();break;case Ρ.ş:Ě();break;case Ρ.З:Ě();break;case Ρ.Αω:Αξ();break;case Ρ.Αϊ:ķ();break;}Ɛ=Ρ.ы;}}}if(!Ć){if(!Ȗ(Ω,true))
{Ω=null;ΑΚ=true;А=2;}if(А>=2&&Ϊ!=Β.ъ)
Ё();if(А<=1){Ř=Ω.CalculateShipMass().PhysicalMass;Ē=(float) Ω.GetShipSpeed();γ=ΙΦ(Ω,Βρ);ΒΦ=Ω.WorldMatrix.Forward;ΑΕ=Ω.WorldMatrix.Left;ǀ=Ω.WorldMatrix.Down;Ιβ();Ιε();ΙΪ();if(ΕΔ){ΕΔ=false;Ě();if(Ϊ!=Β.ъ) Λ="Job resumed after restart";}if(Ϊ!=Β.ъ){Ž=false;ΖΥ(false);Εο(false);String Κπ=ΓΤ(Ϊ)+" "+(int) Ϊ;
ș();Ιΰ();ΓІ(false);Ȕ(Κπ);Ιζ();Ια();ș();Ιά();Ȕ("Thruster");ș();Ζβ();Ȕ("Gyroscope");}else{if(Ž){if(Αυ()){й(ǀ,ΒΦ,ΑΕ,0.25f,true);Ζβ();Λ="Aligning to planet: "+Math.Round(ΒΖ-0.25f,2)+"°";if(Βά) ΓΧ(true,true);}else ΓΧ(true,true);}}ΑΪ=false;}}}ș();ΖΠ();Ȕ("Print");if(Βφ||ΔΗ<=0){ș();ΘΞ(Ζρ);
Ȕ("Broadcast");ΔΗ=4;}else if(ċ) ΔΗ--;}void ΖΡ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var ț=τ.ElementAtOrDefault(1);var ƶ=τ.ElementAtOrDefault(2);var Ζι=τ.ElementAtOrDefault(3);String ȥ="Invalid argument: "+Т;bool Εή=false;
switch(Ѕ){case "UP":this.Γε(false);break;case "DOWN":this.Γδ(false);break;case "UPLOOP":this.Γε(true);break;case "DOWNLOOP":this.Γδ(true);break;case "APPLY":this.Εν(true);break;case "MRES":Ў=0;break;case "STOP":this.Ё();break;case "PATHHOME":{this.Ё();this.ΓЁ();}break;case "PATH":{this.Ё();this.ΓЁ();Ο.ι=true;}break;
case "START":{this.Ё();ΓЀ();}break;case "ALIGN":{ΓΧ(!Ž,false);}break;case "CONT":{this.Ё();this.Ě();}break;case "JOBPOS":{this.Ё();this.ķ();}break;case "HOMEPOS":{this.Ё();this.Αξ();}break;case "FULL":{ΑΠ=true;}break;case "RESET":{Βς=true;А=2;}break;case "EXPORT":{Θι();}break;
case "IMPORT":{Θύ(ț=="JOB");}break;default:Εή=true;break;}if(Κ!=Ι.έ){switch(Ѕ){case "SHUTTLE":{Θζ();}break;case "CFGS":{if(!Θί(ț,ƶ,Ζι)) Λ=ȥ;}break;case "CFGB":{if(!Θή(ț,ƶ)) Λ=ȥ;}break;case "CFGL":{if(!ĸ(ref Ȇ,true,ϊ.Αχ,ț,"")||!Θΰ(ƶ))
Λ=ȥ;}break;case "CFGE":{if(!ĸ(ref н,true,ϊ.Αψ,ț,"IG")||
!ĸ(ref ѝ,true,ϊ.Γζ,ƶ,"IG")||
!ĸ(ref ў,true,ϊ.Γη,Ζι,"IG"))
Λ=ȥ;}break;case "CFGA":{if(!ĸ(ref ǻ,false,ϊ.Αβ,ț,"")) Λ=ȥ;}break;case "CFGW":{if(!ĸ(ref ƞ,false,ϊ.Ʋ,ț,"")||
!ĸ(ref Ɲ,false,ϊ.Ʋ,ƶ,""))
Λ=ȥ;}break;case "NEXT":{ΒЉ(false);}break;case "PREV":{ΒЉ(true);}break;default:if(Εή) Λ=ȥ;break;}}else{switch(Ѕ){case "UNDOCK":{ΑΪ=true;}break;default:if(Εή) Λ=ȥ;break;}}}String Θρ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Θζ(){Ё();Κ=Ι.έ;Ș(у.έ);Ζ.ι=false;Ο.ι=false;ψ=null;ω.Clear();β=Ρ.ы;}void ΓΧ(bool М,bool ώ){if(!М) Λ="Aligning canceled";if(ώ) Λ="Aligning done";if(ώ||!М){Ž=false;Ζή();ΑЄ(false,0,0,0,0);return;}if(Αυ()) Ž=true;}bool Θί(String ΚЊ,String ΙЎ,String ΙϏ)
{bool ΚІ=β==Ρ.З;int w,h,d;if(int.TryParse(ΚЊ,out w)&&int.TryParse(ΙЎ,out h)&&int.TryParse(ΙϏ,out d)){this.Ё();я=w;ю=h;л=d;ľ(false);Ʊ(false,false);if(ΚІ) Ě();return true;}return false;}bool Θΰ(String Ƶ){if(Ƶ=="ON"){ė=true;return true;}if(Ƶ=="OFF"){ė=false;return true;}
return false;}bool Θή(String ΖА,String ΒΗ){bool Ȩ=true;if(ΖА=="HOME") İ=true;else if(ΖА=="STOP") İ=false;else Ȩ=false;if(ΒΗ=="HOME") ё=ш.ΑЁ;else if(ΒΗ=="STOP") ё=ш.Ζέ;else if(ΒΗ=="JOB") ё=ш.ΓΠ;else if(ΒΗ=="IG") ё=ш.Ƥ;else Ȩ=false;return Ȩ;}void κ(Σ ф){Ηκ[(int) ϋ]=Ў;
Ў=Ηκ[(int) ф];if(ф==Σ.Ʈ) Ў=0;ϋ=ф;if(Κ!=Ι.і)
Ľ(ϋ==Σ.ƨ,false,0,0);ΔЄ=true;}void Γε(bool Δϋ){if(Ў>0) Ў--;else if(Δϋ) Ў=Ő-1;}void Γδ(bool Δϋ){if(Ў<Ő-1) Ў++;else if(Δϋ) Ў=0;}String Ș(у ε){switch(ε){case у.ΑЂ:Λ="Job is running";break;case у.Ơ:Λ="Connector not ready!";break;case у.ΙΞ:Λ="Ship modified, path outdated!";break;
case у.ΕБ:Λ="Interrupted by player!";break;case у.έ:Λ="Shuttle mode enabled!";break;}return "";}String Θχ(ь ν){switch(ν){case ь.ư:return "Top-Left";case ь.Ερ:return "Center";default:return "";}}String Θν(Ǒ Џ){switch(Џ){case Ǒ.ΒЏ:return "Auto"+(Κ==Ι.Ʃ?" (Ore)":"");case Ǒ.ΒА:return "Auto (+Stone)";
case Ǒ.ΓΗ:return "Default";default:return "";}}String ΓΡ(Ρ Ѓ){switch(Ѓ){case Ρ.ы:return "No job";case Ρ.Ũ:return "Job paused";case Ρ.З:return "Job active";case Ρ.Ş:return "Job active";case Ρ.ş:return "Job active";case Ρ.ΓΚ:return "Job done";case Ρ.Αη:return "Job changed";case Ρ.Αω:return "Move home";
case Ρ.Αϊ:return "Move to job";}return "";}String ΕϏ(ш Ις){switch(Ις){case ш.ΑЁ:return "Return home";case ш.ΓΠ:return "Fly to job pos";case ш.Ζέ:return "Stop";case ш.Ƥ:return "Ignore";}return "";}String Θο(В Џ){switch(Џ){case В.Γι:return "Off";case В.ΓΛ:return "Drop pos (Stone) ";case В.Αι:return "Drop pos (Sto.+Ice)";
case В.Ευ:return "Cur. pos (Stone)";case В.ΓΕ:return "Cur. pos (Sto.+Ice)";case В.ΓΫ:return "In motion (Stone)";case В.Ασ:return "In motion (Sto.+Ice)";}return "";}String Εύ(ѕ Ѓ){switch(Ѓ){case ѕ.None:return "No batteries";case ѕ.ĵ:return "Charging";case ѕ.Εψ:return "Discharging";}return "";}
String ΓΤ(Β Ѓ){String ΛΓ=Κ==Ι.έ?"target":"job";switch(Ѓ){case Β.ъ:return "Idle";case Β.ĝ:return "Flying to XY position";case Β.ƪ:return Κ==Ι.ǣ?"Grinding":"Mining";case Β.ȳ:return "Returning";case Β.Ƣ:return "Flying to drop pos";case Β.ĺ:return "Returning to dock";case Β.И:return "Flying to dock area";
case Β.с:return "Flying to job area";case Β.Ĝ:return "Flying to path";case Β.ţ:return "Flying to job position";case Β.Ǹ:return "Approaching dock";case Β.Ķ:return "Docking";case Β.Ĳ:return "Aligning";case Β.Ę:return "Aligning";case Β.Ƭ:return "Retry docking";case Β.Ț:return "Unloading";case Β.Ġ:return ΘΙ;
case Β.Ǧ:return "Undocking";case Β.ĵ:return "Charging batteries";case Β.ΑЊ:return "Waiting for uranium";case Β.Αλ:return "Filling up hydrogen";case Β.ΓЇ:return "Waiting for ejection";case Β.ΑЉ:return "Waiting for ejection";case Β.Αν:return "Flying to drop pos";}return "";}String ΕЉ(π Р){switch(Р)
{case π.Γλ:return "On \"Undock\" command";case π.Γο:return "On player entered cockpit";case π.ΖΝ:return "On ship is full";case π.ΖΜ:return "On ship is empty";case π.Γπ:return "On time delay";case π.Αώ:return "On batteries empty(<25%)";case π.Αύ:return "On batteries empty(=0%)";case π.Γκ:return "On batteries full";
case π.Γξ:return "On hydrogen empty(<25%)";case π.Γμ:return "On hydrogen empty(=0%)";case π.Γν:return "On hydrogen full";}return "";}bool Μ(ref String Γ,int Ηό,int ĩ,bool Θ,String ō){Ő+=1;if(Ηό==ĩ) ō=">"+ō+(ΑЎ>=2?" .":"");else ō=" "+ō;Γ+=ō+"\n";return Ηό==ĩ&&Θ;}String Εν(bool Θ)
{int Α=0;int Η=Ў;Ő=0;String ƒ="———————————————\n";String Ɨ="--------------------------------------------\n";String Γ="";Γ+=ΓΡ(β)+" | "+(Ο.ι?"Ready to dock":"No dock")+"\n";Γ+=ƒ;double ȋ=Math.Max(Math.Round(this.а),0);if(ϋ==Σ.Main){bool ī=Κ==Ι.έ;if(Μ(ref Γ,Η,Α++,Θ," Record path & set home")) ΓЁ();
if(Κ==Ι.Ʃ)
if(Μ(ref Γ,Η,Α++,Θ," Setup mining job")) κ(Σ.ƨ);if(Κ==Ι.ǣ)
if(Μ(ref Γ,Η,Α++,Θ," Setup grinding job")) κ(Σ.ƨ);if(Κ==Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," Setup shuttle job")) κ(Σ.Γύ);if(Μ(ref Γ,Η,Α++,Θ," Continue job")) Ě();if(Μ(ref Γ,Η,Α++,Θ," Fly to home position")) Αξ();if(Μ(ref Γ,Η,Α++,Θ," Fly to job position")) ķ();if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(ī) κ(Σ.ΑЅ);else κ(Σ.Αε);if(Μ(ref Γ,Η,Α++,Θ," Info")) κ(Σ.ΕЍ);if(Κ!=Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," Help")) κ(Σ.ΕЊ);}else if(ϋ==Σ.ƨ){double ΚЉ=Math.Round(я*Ȑ,1);double ΙЍ=Math.Round(ю*ȏ,1);String Ж="";if(Μ(ref Ж,Η,Α++,Θ," Start new job!")) ΓЀ();if(Μ(ref Ж,Η,Α++,Θ," Change current job")){Ʊ(false,false);κ(Σ.Main);}if(Μ(ref Ж,Η,Α++,Θ," Width + (Width: "+я+" = "+ΚЉ+"m)")){Š(ref я,5,20,1);ľ(true);}
if(Μ(ref Ж,Η,Α++,Θ," Width -")){Š(ref я,-5,20,-1);ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Height + (Height: "+ю+" = "+ΙЍ+"m)")){Š(ref ю,5,20,1);ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Height -")){Š(ref ю,-5,20,-1);ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Depth + ("+(љ==Ǒ.ΓΗ?"Depth":"Min")+": "+л+"m)")){Š(ref л,5,50,2);ľ(true);}
if(Μ(ref Ж,Η,Α++,Θ," Depth -")){Š(ref л,-5,50,-2);ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Depth mode: "+Θν(љ))){љ=ě(љ);}if(Μ(ref Ж,Η,Α++,Θ," Start pos: "+Θχ(Ȋ))){Ȋ=ě(Ȋ);}if(Κ==Ι.ǣ&&љ==Ǒ.ΒА) љ=ě(љ);Γ+=Γχ(8,Ж,Η,ref Κζ);}else if(ϋ==Σ.Γύ){float[]ΖЇ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Μ(ref Γ,Η,Α++,Θ," Next")) κ(Σ.ΖΪ);if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=" Leave connector 1:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+ΕЉ(І.Р))) І.Р=ě(І.Р);if(!І.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αο((int) І.Ǫ))) І.Ǫ=Εφ(І.Ǫ,ΖЇ);Γ+=" Leave connector 2:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+ΕЉ(Ї.Р))) Ї.Р=ě(Ї.Р);
if(!Ї.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αο((int) Ї.Ǫ))) Ї.Ǫ=Εφ(Ї.Ǫ,ΖЇ);}else if(ϋ==Σ.ΖΪ){if(Μ(ref Γ,Η,Α++,Θ," Start job!")) ΓЀ();if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Γύ);Γ+=" Timer: \"Docking connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(І.ǫ!=""?І.ǫ:"-"))) І.ǫ=Αϋ(ref Κω);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(І.Ǯ!=""?І.Ǯ:"-"))) І.Ǯ=Αϋ(ref Κϋ);Γ+=" Timer: \"Docking connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ї.ǫ!=""?Ї.ǫ:"-"))) Ї.ǫ=Αϋ(ref Κϊ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ї.Ǯ!=""?Ї.Ǯ:"-"))) Ї.Ǯ=Αϋ(ref Κό);
}else if(ϋ==Σ.Ƨ){String Ηΰ=Ȇ+" %";if(ċ) Ī++;if(Ī>1){Ī=0;ΒΩ++;if(ΒΩ>1) ΒΩ=0;}bool[]ΕΙ=new bool[]{ĭ.Count==0,Ǽ==ѕ.None,ǖ.Count==0};int ΘΔ=0;while(true){ΘΔ++;Ŋ++;if(Ŋ>ΕΙ.Length-1) Ŋ=0;if(ΘΔ>=ΕΙ.Length) break;if(!ΕΙ[Ŋ]) break;}bool ī=Κ==Ι.έ;if(!ī&&ė&&в!=-1&&ΒΩ==0)
Ηΰ=в<1000000?Math.Round(в)+" Kg":Math.Round(в/1000)+" t";if(Μ(ref Γ,Η,Α++,Θ," Stop!")){Ё();κ(Σ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!ī) κ(Σ.Αε);else κ(Σ.ΑЅ);if(!ī){if(Μ(ref Γ,Η,Α++,Θ," Next hole")) ΒЉ(false);}else if(Μ(ref Γ,Η,Α++,Θ," Undock")) ΑΪ=true;Γ+=Ɨ;if(!ī) Γ+="Progress: "+Math.Round(ΑΓ,1)+" %\n";Γ+="State: "+ΓΤ(Ϊ)+" "+ȋ+"m \n";Γ+="Load: "+Βα+" % Max: "+Ηΰ+" \n";if(Ŋ==0) Γ+="Uranium: "+(ĭ.Count==0?"No reactors":Math.Round(ś,1)+"Kg "+(н==-1?"":" Min: "+н+" Kg"))+"\n";
if(Ŋ==1) Γ+="Battery: "+(Ǽ==ѕ.None?Εύ(Ǽ):Ȝ+"% "+(ѝ==-1||ī?"":" Min: "+ѝ+" %"))+"\n";if(Ŋ==2) Γ+="Hydrogen: "+(ǖ.Count==0?"No tanks":Math.Round(Ċ,1)+"% "+(ў==-1||ī?"":" Min: "+ў+" %"))+"\n";}else if(ϋ==Σ.Αε){String Ж="";if(Μ(ref Ж,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.Ƨ);else κ(Σ.Main);}
if(Μ(ref Ж,Η,Α++,Θ," Max load: "+Ȇ+"%")) ч(ref Ȇ,Ȇ<=80?-10:-5,ϊ.Αχ,false);if(Μ(ref Ж,Η,Α++,Θ," Weight limit: "+(ė?"On":"Off"))) ė=!ė;if(Μ(ref Ж,Η,Α++,Θ," Ejection: "+Θο(б))) б=ě(б);if(Μ(ref Ж,Η,Α++,Θ," Toggle sorters: "+(ř?"On":"Off"))){ř=!ř;if(ř) Ǻ(ΕΜ);}
if(Μ(ref Ж,Η,Α++,Θ," Unload ice: "+(Ė?"On":"Off"))) Ė=!Ė;if(Μ(ref Ж,Η,Α++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ч(ref н,(н>5?-5:-1),ϊ.Αψ,true);if(Μ(ref Ж,Η,Α++,Θ," Battery: "+(ѝ==-1?"Ignore":"Min "+ѝ+"%"))) ч(ref ѝ,-5,ϊ.Γζ,true);if(Μ(ref Ж,Η,Α++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Min "+ў+"%"))) ч(ref ў,-10,ϊ.Γη,true);
if(Μ(ref Ж,Η,Α++,Θ," When done: "+(İ?"Return home":"Stop"))) İ=!İ;if(Μ(ref Ж,Η,Α++,Θ," On damage: "+ΕϏ(ё))) ё=ě(ё);if(Μ(ref Ж,Η,Α++,Θ," Advanced...")) κ(Σ.ΒЊ);Γ+=Γχ(8,Ж,Η,ref Κη);}else if(ϋ==Σ.ΒЊ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.Ƨ);else κ(Σ.Main);}
if(Μ(ref Γ,Η,Α++,Θ,(Κ==Ι.ǣ?" Grinder":" Drill")+" inv. balancing: "+(ż?"On":"Off"))) ż=!ż;if(Μ(ref Γ,Η,Α++,Θ," Enable"+(Κ==Ι.ǣ?" grinders":" drills")+": "+(Ŷ?"Fwd + Bwd":"Fwd"))) Ŷ=!Ŷ;if(Μ(ref Γ,Η,Α++,Θ," Work speed fwd.: "+ƞ+"m/s")) ч(ref ƞ,0.5f,ϊ.Ʋ,false);
if(Μ(ref Γ,Η,Α++,Θ," Work speed bwd.: "+Ɲ+"m/s")) ч(ref Ɲ,0.5f,ϊ.Ʋ,false);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ч(ref ǻ,0.1f,ϊ.Αβ,false);if(Μ(ref Γ,Η,Α++,Θ," Width overlap: "+ŝ*100f+"%")) Ες(true,0.05f);if(Μ(ref Γ,Η,Α++,Θ," Height overlap: "+ň*100f+"%")) Ες(false,0.05f);
}else if(ϋ==Σ.ΑЅ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.Ƨ);else κ(Σ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Max load: "+Ȇ+"%")) ч(ref Ȇ,Ȇ<=80?-10:-5,ϊ.Αχ,false);if(Μ(ref Γ,Η,Α++,Θ," Unload ice: "+(Ė?"On":"Off"))) Ė=!Ė;if(Μ(ref Γ,Η,Α++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ч(ref н,(н>5?-5:-1),ϊ.Αψ,true);
if(Μ(ref Γ,Η,Α++,Θ," Battery: "+(ѝ==-1?"Ignore":"Charge up"))) ѝ=(ѝ==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Fill up"))) ў=(ў==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," On damage: "+ΕϏ(ё))) ё=ě(ё);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ч(ref ǻ,0.1f,ϊ.Αβ,false);
}else if(ϋ==Σ.ΖΣ){double ΚΘ=0;if(Φ.Count>0) ΚΘ=Vector3.Distance(Φ.Last().Ε,γ);if(Μ(ref Γ,Η,Α++,Θ," Stop path recording")) Ζί();if(Κ!=Ι.έ){if(Μ(ref Γ,Η,Α++,Θ," Home: "+(Ȭ?"Use old home":(Ο.ι?"Was set! ":"none ")))) Ȭ=!Ȭ;}else{if(Μ(ref Γ,Η,Α++,Θ," Connector 1: "+(Ȭ?"Use old connector":(Ο.ι?"Was set! ":"none ")))) Ȭ=!Ȭ;
if(Μ(ref Γ,Η,Α++,Θ," Connector 2: "+(Ƙ?"Use old connector":(Ζ.ι?"Was set! ":"none ")))) Ƙ=!Ƙ;}if(Μ(ref Γ,Η,Α++,Θ," Path: "+(ƙ?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) ƙ=!ƙ;Γ+=Ɨ;Γ+="Wp spacing: "+Math.Round(ΘΛ)+"m\n";}else if(ϋ==Σ.ΓЂ){if(Μ(ref Γ,Η,Α++,Θ," Stop")){Ё();κ(Σ.Main);}
Γ+=Ɨ;Γ+="State: "+ΓΤ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(Ē,1)+"m/s\n";Γ+="Target dist: "+ȋ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+Εβ+"\n";}else if(ϋ==Σ.ΕЍ){List<IMyTerminalBlock>Ń=ΕЀ();if(ċ) Ī++;if(Ī>=Ń.Count) Ī=0;if(Μ(ref Γ,Η,Α++,Θ," Next")) κ(Σ.ΕЎ);
Γ+=Ɨ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βα,1)+"% "+Math.Round(ȁ,1)+" / "+Math.Round(ȇ,1)+"\n";Γ+="Uranium: "+(ĭ.Count==0?"No reactors":Math.Round(ś,1)+"Kg "+Δύ)+"\n";Γ+="Battery: "+(Ǽ==ѕ.None?"":Ȝ+"% ")+Εύ(Ǽ)+"\n";Γ+="Hydrogen: "+(ǖ.Count==0?"No tanks":Math.Round(Ċ,1)+"% ")+"\n";
Γ+="Damage: "+(Ń.Count==0?"None":""+(Ī+1)+"/"+Ń.Count+" "+Ń[Ī].CustomName)+"\n";}else if(ϋ==Σ.ΕЎ){if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=Ɨ;Γ+="Next scan: "+ΑΞ+"s\n";Γ+="Drill area: "+Math.Round(Ȑ,1)+"x"+Math.Round(ȏ,1)+"m Ship: "+Math.Round(Βυ,1)+"x"+Math.Round(Βτ,1)+"m ("+Math.Round(ο,1)+"m)\n";
Γ+="Broadcast: "+(ų?"Online - "+Ɠ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ƃ*100f,1)+"% \n";}else if(ϋ==Σ.ΕЊ){if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=Ɨ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+ΑΞ+"s\n";if(Βς) Γ="Recompile script now";int ΚΙ=Γ.Split('\n').Length;for(int i=ΚΙ;i<=10;i++) Γ+="\n";Γ+=ƒ;Γ+="Last: "+Λ+"\n";
return Γ;}void Ες(bool ΚΔ,float ǝ){Ё();Ʊ(true,false);if(ΚΔ) ч(ref ŝ,ǝ,ϊ.Γρ,false);else ч(ref ň,ǝ,ϊ.Γρ,false);Επ();Ľ(true,true,0,0);}float Εφ(float ξ,float[]ΒІ){float ή=ΒІ[0];for(int i=ΒІ.Length-1;i>=0;i--)
if(ξ<ΒІ[i]) ή=ΒІ[i];return ή;}String Αϋ(ref int ĩ){String ή="";if(ĩ>=ΕΧ.Count) ĩ=-1;if(ĩ>=0) ή=ΕΧ[ĩ].CustomName;ĩ++;return ή;}void ΑЇ(string ί){if(β!=Ρ.З) return;if(ί=="") return;IMyTerminalBlock Ψ=χ.GetBlockWithName(ί);if(Ψ==null||!(Ψ is IMyTimerBlock)){Λ="Timerblock "+ί+" not found!";
return;}((IMyTimerBlock) Ψ).Trigger();}void Š(ref int value,int ƺ,int ĕ,int ΕΚ){if(ƺ==0) return;if(value<ĕ&&ΕΚ>0||value<=ĕ&&ΕΚ<0){value+=ΕΚ;return;}int Ιι=Math.Abs(ƺ);int ΓЈ=0;int ΔІ=1;while(true){ΓЈ+=ΔІ*Ιι*10;if(ƺ<0&&value-ĕ<=ΓЈ) break;if(ƺ>0&&value-ĕ<ΓЈ) break;
ΔІ++;}value+=ΔІ*ƺ;}void ľ(bool ΛΖ){я=Math.Max(я,1);ю=Math.Max(ю,1);л=Math.Max(л,0);Ľ(ϋ==Σ.ƨ,false,0,0);}bool ĸ(ref float value,bool Κγ,ϊ υ,String ŋ,String ΚΑ){if(ŋ=="") return false;float Ƈ=-1;bool ΗΟ=false;if(ŋ.ToUpper()==ΚΑ) ΗΟ=true;else if(!float.TryParse(ŋ,out Ƈ)) return false;
else Ƈ=Math.Max(0,Ƈ);if(Κγ) Ƈ=(float) Math.Round(Ƈ);ч(ref value,Ƈ,υ,ΗΟ,false);return true;}void ч(ref float value,float ǝ,ϊ υ,bool ΑΩ){ч(ref value,value+ǝ,υ,ΑΩ,true);}void ч(ref float value,float Ƅ,ϊ υ,bool ΑΩ,bool ΚЎ){float ǰ=0,ǯ=0;if(υ==ϊ.Ʋ){ǰ=0.5f;ǯ=10f;}if(υ==ϊ.Αβ){ǰ=0.1f;ǯ=1f;}
if(υ==ϊ.Ƥ){ǰ=50f;ǯ=100f;}if(υ==ϊ.Γζ){ǰ=5f;ǯ=30f;}if(υ==ϊ.Αψ){ǰ=1f;ǯ=25f;}if(υ==ϊ.Αχ){ǰ=10f;ǯ=95f;}if(υ==ϊ.Γη){ǰ=10f;ǯ=90f;}if(υ==ϊ.ΙΤ){ǰ=10f;ǯ=1800;}if(υ==ϊ.Γρ){ǰ=0.0f;ǯ=0.75f;}if(Ƅ==-1&&ΑΩ){value=-1;return;}if(value==-1) ΑΩ=false;
bool Ηφ=Ƅ<ǰ||Ƅ>ǯ;if(Ηφ&&ΚЎ){if(Ƅ<value) value=ǯ;else if(Ƅ>value) value=ǰ;}else value=Ƅ;if(Ηφ&&ΑΩ) value=-1;else value=Math.Max(ǰ,Math.Min(value,ǯ));value=(float) Math.Round(value,2);}void ΒЉ(bool Κή){if(Κή) Я=Math.Max(0,Я-1);else Я++;ΓΝ();ΓΜ();for(int k=0;k<Ȥ.Count&&k<з.Length;k++) Γϋ(Ȥ[k],k<Я);
Г=-1;ΓІ(true);}T ě<T>(T ξ){int Α=Array.IndexOf(Enum.GetValues(ξ.GetType()),ξ);Α++;if(Α>=Θη(ξ)) Α=0;return(T) Enum.GetValues(ξ.GetType()).GetValue(Α);}int Θη<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЁ(){Αΰ.Clear();for(int i=0;i<Φ.Count;i++) Αΰ.Add(Φ[i]);Φ.Clear();
Βέ=true;Δν=new Ϋ(Ο);Δπ=new Ϋ(Ζ);Ο.ι=false;if(Κ==Ι.έ) Ζ.ι=false;for(int i=0;i<Ǟ.Count;i++)
if(!Ȫ.Contains(Ǟ.Keys.ElementAt(i)))
Ȫ.Add(Ǟ.Keys.ElementAt(i));Ȭ=false;Ƙ=false;ƙ=false;κ(Σ.ΖΣ);}void Ζί(){if(Ȭ) Ο=Δν;if(Ƙ) Ζ=Δπ;if(ƙ){Φ.Clear();for(int i=0;i<Αΰ.Count;i++) Φ.Add(Αΰ[i]);}Βέ=false;Ё();κ(Σ.Main);}void Ιβ(){if(!Βέ) return;if(Ϊ!=Β.ъ){Ζί();return;}if(!Δν.ι) Ȭ=false;if(!Δπ.ι) Ƙ=false;if(Αΰ.Count<=1) ƙ=false;
IMyShipConnector О=д(MyShipConnectorStatus.Connectable);if(О==null) О=д(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Ē,2)<=0.20) ΔΨ++;else ΔΨ=0;if(ΔΨ>=5){if(Κ==Ι.έ&&(Ο.ι||Ȭ)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.θ=Ω.WorldMatrix.Forward;Ζ.Ђ=Ω.WorldMatrix.Left;
Ζ.η=Ω.WorldMatrix.Down;Ζ.φ=Ω.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.ι=true;Ζ.м=О.Position;}else{Ο.θ=Ω.WorldMatrix.Forward;Ο.Ђ=Ω.WorldMatrix.Left;Ο.η=Ω.WorldMatrix.Down;Ο.φ=Ω.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.ι=true;Ο.м=О.Position;}}}double Δς=-1;if(Φ.Count>0) Δς=Vector3.Distance(γ,Φ.Last().Ε);
double Κξ=Math.Max(1.5,Math.Pow(Ē/100.0,2));double Ηλ=Math.Max(Ē*Κξ,2);ΘΛ=Ηλ;if((Δς==-1)||Δς>=Ηλ){Ϋ ά=new Ϋ(γ,ΒΦ,ǀ,ΑΕ,Ω.GetNaturalGravity());ά.ΘΤ(ȫ,Ȫ);Φ.Add(ά);}}Ϋ щ(){if(Κ!=Ι.έ) return Ǚ!=null?Ǚ:Ο;return ΖЂ;}void Ū(Ϋ α,Ρ Ѓ){ΖЂ=α;if(β==Ρ.З) ќ=Ѓ;}bool ȕ(Ϋ α,float Њ,bool ΚЅ,out Vector3 ν)
{if(ΚЅ){Vector3I Δδ=new Vector3I((int) α.м.X,(int) α.м.Y,(int) α.м.Z);IMySlimBlock Ψ=Me.CubeGrid.GetCubeBlock(Δδ);if(Ψ==null||!(Ψ.FatBlock is IMyShipConnector)){ν=new Vector3();return false;}Vector3 Ιψ=т(Ω,Ψ.FatBlock.GetPosition()-γ);Vector3 Ιχ=т(Ω,Ψ.FatBlock.WorldMatrix.Forward);ν=α.Ε-ΓЅ(α.θ,α.η*-1,Ιψ)-ΓЅ(α.θ,α.η*-1,Ιχ)*Њ;
return true;}else{ν=α.Ε;return true;}}Vector3 Ȱ(int Ιυ,int ƍ,bool Κα){if(!Κα&&ΗΙ) return ΔΘ;float Ιφ=((Ќ-1f)/2f)-Ιυ;float Κδ=((Ь-1f)/2f)-ƍ;ΔΘ=Ζ.Ε+Ζ.Ђ*Ιφ*Ȑ+ď*-1*Κδ*ȏ;ΗΙ=true;return ΔΘ;}Vector3 ΓΣ(Vector3 Κς,float ΔΤ){return Κς+(Ǳ*ΔΤ);}δ ΓΘ(){float Щ=-1;δ ή=δ.ȓ;
if(Κ!=Ι.έ){ΖΗ();if(β!=Ρ.ы){Vector3 Н=ļ(Ǳ,ď*-1,γ-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ќ*Ȑ)/2f&&Math.Abs(Н.Y)<=(float)(Ь*ȏ)/2f){if(Н.Z<=-1&&Н.Z>=-Ŗ*2) return δ.Αδ;if(Н.Z>-1&&Н.Z<ο*2) return δ.ȓ;}if(ę(Ζ.Ε,ref Щ)) ή=δ.ȓ;}if(Ο.ι){if(ę(Ο.Ε,ref Щ)) ή=δ.Ȓ;for(int i=0;i<Φ.Count;i++)
if(ę(Φ[i].Ε,ref Щ)) ή=δ.Ȳ;if(Vector3.Distance(γ,Ο.Ε)<dockDist*ο) ή=δ.Ȓ;if(д(MyShipConnectorStatus.Connectable)!=null||д(MyShipConnectorStatus.Connected)!=null)
ή=δ.Ȓ;}}else{Vector3 ν=new Vector3();IMyShipConnector ȟ=д(MyShipConnectorStatus.Connected);if(Ο.ι){if(ę(Ο.Ε,ref Щ)) ή=δ.Ȓ;if(ȕ(Ο,dockDist,true,out ν))
if(ę(ν,ref Щ)) ή=δ.Ȓ;if(ȟ!=null&&Vector3.Distance(ȟ.GetPosition(),Ο.Ε)<5)
return δ.ΒЍ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ο.Ε)>dockDist*ο&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*ο)
if(ę(Φ[i].Ε,ref Щ)) ή=δ.Ȳ;if(Ζ.ι){if(ę(Ζ.Ε,ref Щ)) ή=δ.ȓ;if(ȕ(Ζ,dockDist,true,out ν))
if(ę(ν,ref Щ)) ή=δ.ȓ;if(ȟ!=null&&Vector3.Distance(ȟ.GetPosition(),Ζ.Ε)<5)
return δ.ΒЎ;}}return ή;}bool ę(Vector3 ν,ref float Щ){float Њ=Vector3.Distance(ν,γ);if(Њ<Щ||Щ==-1){Щ=Њ;return true;}return false;}void ΓЀ(){if(А>0){Λ="Setup error! Can't start";return;}if(Κ==Ι.έ){Ě();return;}Ζ.Ε=γ;Ζ.φ=Ω.GetNaturalGravity();Ζ.θ=ΒΦ;Ζ.η=ǀ;Ζ.Ђ=ΑΕ;
Ǳ=ǚ.WorldMatrix.Forward;ď=Ζ.η;if(Ǳ==Ω.WorldMatrix.Down) ď=Ω.WorldMatrix.Backward;Ʊ(true,true);Τ(Β.ĝ);ΓϏ();}void Ʊ(bool Ǭ,bool Κβ){if(β==Ρ.ы&&!Ǭ) return;bool ΚΡ=Ǭ||β==Ρ.ΓΚ||Ќ!=я||Ь!=ю||Ō!=Ȋ;if(ΚΡ){if(β!=Ρ.ы){β=Ρ.Αη;Ȱ(Ϗ,ό,Κβ);Λ="Job changed, lost progress";}Ō=Ȋ;
Ќ=я;Ь=ю;ό=0;Ϗ=0;з="";Г=-1;ŏ=0;č=0;ж=0;Я=0;ΓІ(true);}}void Γσ(){е(γ,0);ǔ(ȫ,true);}void ΓϏ(){Ș(у.ΑЂ);Γσ();ƭ(ȧ,false);Ǻ(ΕΜ);β=Ρ.З;Εο(true);ќ=β;κ(Σ.Ƨ);ΙΓ();ΒΘ=true;ΒΪ=0;for(int i=Ƴ.Count-1;i>=0;i--)
if(ƥ(Ƴ[i],false)) ΒΪ++;if(ΒΪ>0) Λ="Started with damage";}void Ё(){if(β==Ρ.З){β=Ρ.Ũ;Λ="Job paused";}Τ(Β.ъ);ќ=β;ΑЄ(false,0,0,0,0);ΓЃ();Εκ(new Vector3(),false);Ζή();Γϊ(ChargeMode.Auto);Γό(false);ΖΥ(true);Ā(Β.ъ);Ľ(false,false,0,0);ǔ(ω,false);ǔ(ј,true);Ǻ(true);Βσ=false;Αθ();
ƿ=false;Γυ();ΒΘ=false;ΑΠ=false;ΑΪ=false;if(ϋ!=Σ.Main&&ϋ!=Σ.Αε&&ϋ!=Σ.ΒЊ&&ϋ!=Σ.ΑЅ)
κ(Σ.Main);}void Ě(){δ Э=ΓΘ();if(Κ==Ι.έ){if(!Ζ.ι||!Ο.ι) return;ΓϏ();bool Δβ=Vector3.Distance(γ,Ο.Ε)<Vector3.Distance(γ,Ζ.Ε);if(Ɛ==Ρ.Ş) Δβ=true;if(Ɛ==Ρ.ş) Δβ=false;if(Δβ){Ū(Ο,Ρ.Ş);switch(Э){case δ.ΒЍ:Τ(Β.Ġ);break;case δ.Ȳ:Τ(Β.с);break;case δ.Ȓ:Τ(Β.Ǹ);break;default:Τ(Β.Ǧ);break;
}}else{Ū(Ζ,Ρ.ş);switch(Э){case δ.ΒЎ:Τ(Β.Ġ);break;case δ.ȓ:Τ(Β.Ǹ);break;case δ.Ȳ:Τ(Β.с);break;default:Τ(Β.Ǧ);break;}}}else{if(β!=Ρ.Ũ&&β!=Ρ.Αη) return;bool ΚЇ=β==Ρ.Αη;ΓϏ();bool Δι=Ʀ(false)&&Ο.ι;switch(Э){case δ.ȓ:Τ(Δι?Β.Ĝ:Β.ĝ);break;case δ.Ȳ:Τ(Δι?Β.И:Β.с);break;case δ.Ȓ:Τ(Δι?Β.Ǹ:Β.Ț);break;
case δ.Αδ:{if(č!=Я||ΚЇ) Τ(Β.ȳ);else Τ(Β.ƪ);}break;default:break;}}}void ķ(){if(β==Ρ.ы&&!Ο.ι) return;if(Κ==Ι.έ&&(!Ζ.ι||!Ο.ι)) return;Λ="Move to job";δ Э=ΓΘ();if(Κ==Ι.έ){Ū(Ζ,Ρ.ş);switch(Э){case δ.ȓ:Τ(Β.Ǹ);break;case δ.Ȳ:Τ(Β.с);break;case δ.ΒЎ:return;default:Τ(Β.Ǧ);break;
}Ā(Β.Ġ);}else{switch(Э){case δ.ȓ:Τ(Β.с);break;case δ.Ȳ:Τ(Β.с);break;case δ.Ȓ:Τ(Β.Ț);break;case δ.Αδ:Τ(Β.ȳ);break;default:break;}if(β==Ρ.ы) Ā(Β.с);else Ā(Β.Ę);Βσ=true;}Γσ();κ(Σ.ΓЂ);ƭ(ȧ,false);ќ=Ρ.Αϊ;}void Αξ(){if(!Ο.ι) return;Λ="Move home";δ Э=ΓΘ();if(Κ==Ι.έ){Ū(Ο,Ρ.Ş);switch(Э)
{case δ.Ȳ:Τ(Β.И);break;case δ.Ȓ:Τ(Β.Ǹ);break;case δ.ΒЍ:return;default:Τ(Β.Ǧ);break;}Ā(Β.Ġ);}else{if(д(MyShipConnectorStatus.Connected)!=null) return;if(д(MyShipConnectorStatus.Connectable)!=null){Τ(Β.Ķ);Ā(Β.Ț);return;}switch(Э){case δ.ȓ:Τ(Β.Ĝ);break;case δ.Ȳ:Τ(Β.И);break;case δ.Ȓ:Τ(Β.И);break;
case δ.Αδ:Τ(Β.ĺ);break;default:break;}Ā(Β.Ț);}Γσ();κ(Σ.ΓЂ);ƭ(ȧ,false);ќ=Ρ.Αω;}void Τ(Β ΔЇ){if(ΔЇ==Β.ъ) ΑΤ=Β.ъ;if(ΑΤ!=Β.ъ&&Ϊ==ΑΤ&&ΔЇ!=ΑΤ){Ё();return;}Д=true;Ϊ=ΔЇ;}void Ā(Β Ѓ){this.ΑΤ=Ѓ;}ǥ ΛΒ(int ΑΑ,bool ǳ){if(ǳ){ǜ=null;Ϗ=0;ό=0;}if(Ȋ==ь.ư){int ő=ΑΑ+1;
ό=(int) Math.Floor(Й(ΑΑ,Ќ));if(ό%2==0) Ϗ=ΑΑ-(ό*Ќ);else Ϗ=Ќ-1-(ΑΑ-(ό*Ќ));if(ό>=Ь) return ǥ.Αγ;else return ǥ.Απ;}else if(Ȋ==ь.Ερ){if(ǜ==null) ǜ=new int[]{0,-1,0,0};int Ǌ=(int) Math.Ceiling(Ќ/2f);int ǉ=(int) Math.Ceiling(Ь/2f);int Δη=(int) Math.Floor(Ќ/2f);
int Δζ=(int) Math.Floor(Ь/2f);int ΑΒ=0;while(ǜ[2]<Math.Pow(Math.Max(Ќ,Ь),2)){if(ΑΒ>200) return ǥ.Θθ;ΑΒ++;ǜ[2]++;if(-Ǌ<Ϗ&&Ϗ<=Δη&&-ǉ<ό&&ό<=Δζ){if(ǜ[3]==ΑΑ){this.Ϗ=Ϗ-1+Ǌ;this.ό=ό-1+ǉ;return ǥ.Απ;}ǜ[3]++;}if(Ϗ==ό||(Ϗ<0&&Ϗ==-ό)||(Ϗ>0&&Ϗ==1-ό))
{int Κυ=ǜ[0];ǜ[0]=-ǜ[1];ǜ[1]=Κυ;}Ϗ+=ǜ[0];ό+=ǜ[1];}}return ǥ.Αγ;}void Ιΰ(){if(Ϊ==Β.ĝ){if(Д){Е=0;if(č!=Я) ŏ=0;č=Я;}if(Е==0){int ΔΛ=ΘΪ(Я);if(ΔΛ<0){if(!ΔΠ){ΔΠ=true;ΖϏ=γ;}е(ΖϏ,0);Λ="Waiting for the controller";return;}ΔΠ=false;if(ΔΛ!=Я){Я=ΔΛ;ŏ=0;č=Я;}
ǥ ή=ΙΜ(Я);if(ή==ǥ.Αγ){β=Ρ.ΓΚ;bool Ηυ=з.IndexOf('0')>=0;Λ=Ηυ?"No free hole left for this ship":"Job done";if((İ||Ηυ)&&Ο.ι){Τ(Β.Ĝ);Ā(Β.Ț);ќ=Ρ.Αω;}else{Τ(Β.ţ);Ā(Β.Ę);ќ=Ρ.Αϊ;}return;}if(ή==ǥ.Απ){Е=1;ǔ(ω,true);џ=Ȱ(Ϗ,ό,true);е(џ,10);й(Ζ.η,Ζ.θ,Ζ.Ђ,false);}}else
{if(Г>=0&&(ΖΖ()?Ȁ>=0&&Ȁ!=Г:ΓΪ(Г,true))){Е=0;return;}if(а<wpReachedDist){Τ(Β.ƪ);return;}}}if(Ϊ==Β.ƪ){if(Д){ǔ(ω,true);Ǻ(false);џ=Ȱ(Ϗ,ό,false);е(ΓΣ(џ,0),0);й(Ζ.η,Ζ.θ,Ζ.Ђ,false);Е=1;ѐ=0;р=0;ΑЏ=0;Δτ=-1;Ŗ=л;Δξ=true;}if(!ΓΓ()){Τ(Β.ĺ);return;}if(Ʀ(true))
{Ησ=Ȯ("","ORE",Ш.ĳ);if((б==В.ΓΛ||б==В.Αι||
б==В.ΓΫ||б==В.Ασ)&&Κ!=Ι.ǣ)
Τ(Β.Ƣ);else if((б==В.Ευ||б==В.ΓΕ)&&Κ!=Ι.ǣ)
Τ(Β.ΓЇ);else
Τ(Β.ĺ);return;}ж=Vector3.Distance(γ,џ);if(ж>ŏ){ŏ=ж;Δξ=false;}if(Κ==Ι.ǣ&&ΕЁ()==MyDetectedEntityType.SmallGrid)
р+=2;else р-=2;р=Math.Max(100,Math.Min(400,р));if(Е>0&&Е<р){if(ж>ѐ){if(р>150) ѐ=ж;else ѐ=(float) Math.Ceiling(ж);Е=1;}else Е++;}else{if(Е>0){Λ="Ship stuck! Retrying";ѐ=ж;Е=0;Ľ(false,true,0,ο*sensorRange);}е(ΓΣ(џ,Math.Max(0,ѐ-ο)),ΓΦ(false));if(а<=wpReachedDist/2){Е=1;ѐ=0;}
return;}Ľ(false,true,ο*sensorRange,0);Vector3 Ιλ=џ+Ǳ*ж;bool ΗΤ=false;if(Vector3.Distance(Ιλ,γ)>0.3f){Vector3 Ιό=џ+Ǳ*(ж+0.1f);е(Ιό,4);ΗΤ=true;}else{float Ǵ=ΓΦ(true);Vector3 ΖІ=ΓΣ(џ,Math.Max(л+1,ж+1));ǹ(true,false,false,ΖІ,ΖІ-џ,Ǵ,Ǵ);}bool ΔΥ=false;if(љ==Ǒ.ΒА||љ==Ǒ.ΒЏ)
{if(!ΗΤ){float ΔЊ=0;foreach(IMyTerminalBlock Ȏ in ω)
ΔЊ+=Θβ(Ȏ,"","",љ==Ǒ.ΒЏ?new string[]{"STONE"}:null);if(ΔЊ>Δτ||ж<л||Δξ){ΓЎ=0;ΑЏ=ж;Ŗ=(int)(Math.Max(Ŗ,ΑЏ)+ο/2);}else{ΔΥ=ж-ΑЏ>2&&ΓЎ>=20;ΓЎ++;}Δτ=ΔЊ;}}else ΔΥ=ж>=Ŗ;if(č!=Я){Τ(Β.ȳ);ж=0;return;}if(ΔΥ){Я++;Τ(Β.ȳ);ж=0;return;}}if(Ϊ==Β.ȳ||Ϊ==Β.ĺ||Ϊ==Β.Ƣ)
{if(Д){џ=Ȱ(Ϗ,ό,false);й(Ζ.η,Ζ.θ,Ζ.Ђ,false);ǔ(ω,Ŷ);Ǻ(false);ѐ=Vector3.Distance(γ,џ);Ľ(false,true,0,ο*sensorRange);}е(џ,ΓΦ(false));if(Vector3.Distance(γ,џ)>=ѐ+5){ǔ(ω,false);Ǻ(true);Λ="Can´t return!";}if(а<wpReachedDist){if(Ϊ==Β.ȳ&&Βσ) Τ(Β.ţ);if(Ϊ==Β.ȳ) Τ(Β.ĝ);
if(Ϊ==Β.Ƣ) Τ(Β.Αν);if(Ϊ==Β.ĺ){if(Ο.ι) Τ(Β.Ĝ);else{Ё();ķ();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Β.И||Ϊ==Β.с){if(Ϊ==Β.с&&β==Ρ.З&&Κ!=Ι.έ){if(!ΓΓ()||Ʀ(true)){Τ(Β.И);return;}}bool ΕΑ=false;bool ΒЄ=false;bool Ζκ=false;float Ű=0;bool Ης=false;Ϋ ά=null;
if(Д){if(Ϊ==Β.И||Κ==Ι.έ){if(Κ!=Ι.έ){if(ā(1)) ΒЌ();else{Ǚ=null;ȩ=0;ƿ=!ΓΔ(1);}}Ϋ α=щ();μ=new Γς();μ.ζ=α;μ.ƽ=followPathDock*ο;μ.Εί=useDockDirectionDist*ο;μ.Ű=10;μ.Ɩ.Add(α.Ε);Vector3 ů=new Vector3();if(ȕ(α,dockDist*ο,true,out ů))
μ.Ɩ.Add(ů);else μ.ƽ*=1.5f;if(Κ==Ι.έ){if(α==Ο) μ.ΒΣ=Ζ.Ε;if(α==Ζ) μ.ΒΣ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;}}else if(Ϊ==Β.с){μ=new Γς();μ.ζ=Ζ;μ.ƽ=followPathJob*ο;μ.Εί=useJobDirectionDist*ο;μ.Ű=10;μ.ΒΣ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;μ.Ɩ.Add(Ζ.Ε);if(β==Ρ.ы){if(!Ο.ι||Φ.Count==0){Ё();return;}
float ΙЂ=Vector3.Distance(Φ.First().Ε,Ο.Ε);float ΙЃ=Vector3.Distance(Φ.Last().Ε,Ο.Ε);if(ΙЂ<ΙЃ) μ.ζ=Φ.Last();else μ.ζ=Φ.First();}}ΑΜ=new Vector3();Ης=!ΘЁ(γ);ǔ(ω,false);Ǻ(true);и=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,μ.ΒΣ)<=μ.Δί) continue;
double Њ=Vector3.Distance(Φ[i].Ε,γ);if(ǿ==-1||Њ<ǿ){и=i;ǿ=Њ;}}Ȉ=Θτ(μ.ζ.Ε,и);ł=null;}if(Ϊ==Β.И&&Κ!=Ι.έ&&!Д&&!Ư()){Ϋ ΔΒ=щ();ƿ=!ΓΔ(1);if(щ()!=ΔΒ){Τ(Β.И);return;}}if(Ϊ==Β.И&&Κ!=Ι.έ&&!Д&&ā(1)&&ΒЌ()){Τ(Β.И);return;}bool ΗΡ=false;if(Ϊ==Β.И&&!Д&&и>=0&&и<Φ.Count)
{Vector3 Ƽ=Φ[и].Ε+ΕЄ(и,Ȉ);int ΔЈ=и+Ȉ;Vector3 ő=Ȉ!=0&&ΔЈ>=0&&ΔЈ<Φ.Count?Φ[ΔЈ].Ε:μ.ζ.Ε;if(Θό(Ƽ,ő)){if(!ΒΛ){ΒΛ=true;ΖЋ=ŧ(Ƽ)?γ:Ƽ;}е(ΖЋ,0);й(μ.ζ.η,μ.ζ.θ,μ.ζ.Ђ,false);Λ="Waiting for dock clearance";return;}ΒΛ=false;ΗΡ=ā(1)&&(ŧ(γ)||ŧ(ő));}ΘΥ(Φ,Ȉ,μ.Ɩ,μ.ƽ,Д,ref Е);
for(int i=0;i<μ.Ɩ.Count;i++){float Њ=Vector3.Distance(γ,μ.Ɩ[i]);if(Њ<=μ.ƽ) ΕΑ=true;if(Њ<=μ.Εί) ΒЄ=true;}if(ΗΡ) ΒЄ=true;if(ΒЄ) Ű=μ.Ű;float ΛΗ=ł!=null?ł.Ч:Ē;float ΚЍ=(float) Math.Max(Ē*0.1f*ο,wpReachedDist);if((а<ΚЍ)||Д){if(!Д) и+=Ȉ;if(Ȉ==0||и>Φ.Count-1||и<0)
ΕΑ=true;else{Εβ=Ȉ>0?Φ.Count-1-и:и;ά=Φ[и];ł=ά;if(и>=1&&и<Φ.Count-1)
ΑΜ=ά.Ε-Φ[и-Ȉ].Ε;else ł=null;Ѐ=ά.Ε+ΕЄ(и,Ȉ);Ζκ=true;}}if(ΒЄ)
й(μ.ζ.η,μ.ζ.θ,μ.ζ.Ђ,false);else if(Ης)
ΒЋ(μ.ζ.η,10,true);else if(Ζκ&&ά!=null){if(Ȉ>0) й(ά.η,ά.θ,ά.Ђ,90,false);else й(ά.η,-ά.θ,-ά.Ђ,90,false);}ǹ(true,false,true,Ѐ,ΑΜ,ł==null?0:ł.Ч,Ű);if(ΕΑ){Εβ=0;if(Ϊ==Β.И&&!ΘΨ()){е(Ѐ,0);return;}if(Ϊ==Β.И&&ƿ){е(Ѐ,0);Λ="Waiting for a free dock";return;}if(Ϊ==Β.И||Κ==Ι.έ){Τ(Β.Ǹ);return;}
if(Ϊ==Β.с&&Βσ){Τ(Β.ţ);return;}if(Ϊ==Β.с){Τ(Β.ĝ);return;}}}if(Ϊ==Β.Ǹ||Ϊ==Β.Ƭ){if(Κ!=Ι.έ&&Ư()){if(!ā(1)){Τ(Β.И);return;}}else if(Κ!=Ι.έ){Ϋ ΔΒ=щ();if(!ΓΔ(2)||щ()!=ΔΒ){Τ(Β.И);return;}}Ϋ α=щ();if(Д){if(!ȕ(α,dockDist*ο,true,out Ѐ)){Ș(у.Ơ);Ё();return;}
е(Ѐ,0);ΒЋ(α.η,90,true);}if(а<followPathDock*ο&&а!=-1){е(Ѐ,10);й(α.η,α.θ,α.Ђ,false);}if(д(MyShipConnectorStatus.Connectable)!=null||д(MyShipConnectorStatus.Connected)!=null){Τ(Β.Ķ);return;}if(а<wpReachedDist/2&&а!=-1){Τ(Β.Ĳ);return;}}if(Ϊ==Β.Ĳ||Ϊ==Β.Ę){if(Д)
{if(Ϊ==Β.Ĳ){Ϋ α=щ();if(!ȕ(α,dockDist*ο,true,out Ѐ)){Ș(у.Ơ);Ё();return;}ǹ(true,true,false,Ѐ,0);й(α.η,α.θ,α.Ђ,10,false);}if(Ϊ==Β.Ę){й(Ζ.η,Ζ.θ,Ζ.Ђ,0.5f,false);Ѐ=Ζ.Ε;ǹ(true,true,false,Ѐ,0);}}if(Βά){ΑЄ(false,0,0,0,0);if(Ϊ==Β.Ĳ) Τ(Β.Ķ);if(Ϊ==Β.Ę) Ё();return;}}
if(Ϊ==Β.Ķ){if(д(MyShipConnectorStatus.Connected)!=null){if(Κ==Ι.έ) Τ(Β.Ġ);else Τ(Β.Ț);return;}Ϋ α=щ();if(Д){р=0;ΙЄ=DateTime.Now;Е=0;й(α.η,α.θ,α.Ђ,false);}Vector3I Δδ=new Vector3I((int) α.м.X,(int) α.м.Y,(int) α.м.Z);IMySlimBlock Ζό=Me.CubeGrid.GetCubeBlock(Δδ);float ΖЎ=dockingSpeed;
float ΚΜ=dockingSpeed*5;float Ζθ=Math.Max(1.5f,Math.Min(5f,ο*0.15f));if(!ȕ(α,0,true,out Ѐ)||!ȕ(α,Ζθ,true,out ΑΜ)||
Ζό==null||!Ζό.FatBlock.IsFunctional){Ș(у.Ơ);Ё();return;}if(р==1||(Vector3.Distance(γ,Ѐ)<=Ζθ*1.1f&&!Д))
р=1;else{Vector3 Κύ=т(Ω,ΑΜ-γ);Vector3 Δγ=т(Ω,Ω.GetNaturalGravity());float Κε=Αζ(Κύ,Δγ,null);ΖЎ=Math.Min(ΚΜ,Κε);}ǹ(true,false,false,Ѐ,Ѐ-γ,dockingSpeed,ΖЎ);if(Д) ѐ=(float) а;IMyShipConnector ΔΟ=д(MyShipConnectorStatus.Connectable);if(ΔΟ!=null){ǹ(false,false,false,Ѐ,0);
if(Е>0) Е=0;Е--;if(Е<-5){ΔΟ.Connect();if(ΔΟ.Status==MyShipConnectorStatus.Connected){if(Κ==Ι.έ) Τ(Β.Ġ);else Τ(Β.Ț);ΓЃ();ƭ(ȧ,true);return;}}}else{float ΖЊ=(float) Math.Round(а,1);if(ΖЊ<ѐ){Е=-1;ѐ=ΖЊ;}else Е++;if(Е>20){Τ(Β.Ƭ);return;}}}if(Ϊ==Β.Ț||Ϊ==Β.Ġ||
Ϊ==Β.ΑЊ||Ϊ==Β.Αλ||Ϊ==Β.ĵ){bool ΓЌ=false,ΓЍ=false;if(Κ==Ι.έ){if(щ()==Ο) ΓЌ=true;else if(щ()==Ζ) ΓЍ=true;}if(Д){ΑΠ=false;if(д(MyShipConnectorStatus.Connected)==null){Τ(Β.Ǧ);return;}ΓЃ();if(ΓЌ) ΑЇ(І.ǫ);if(ΓЍ) ΑЇ(Ї.ǫ);Ś=false;Ă=false;ĉ=false;Αά=false;}if(д(MyShipConnectorStatus.Connected)==null){Ё();Ș(у.ΕБ);return;}
if(β!=Ρ.З||ѝ==-1||Ǽ==ѕ.None) Ă=true;else if(Ȝ>=100f) Ă=true;else if(Ȝ<=99f) Ă=false;if(β!=Ρ.З||ў==-1||ǖ.Count==0) ĉ=true;else if(Ċ>=100f) ĉ=true;else if(Ċ<=99) ĉ=false;if(β!=Ρ.З||н==-1||ĭ.Count==0) Αά=true;else Αά=ś>=н;Ŭ є=null;if(ΓЌ) є=І;
if(ΓЍ) є=Ї;if(є!=null&&(є.Р==π.Αώ||є.Р==π.Αύ)) Ă=true;if(є!=null&&(є.Р==π.Γκ)) if(!Ś) Ă=false;if(є!=null&&(є.Р==π.Γξ||є.Р==π.Γμ)) ĉ=true;if(є!=null&&(є.Р==π.Γν)) if(!Ś) ĉ=false;if(ċ){ChargeMode Ζτ=Ă?ChargeMode.Auto:ChargeMode.Recharge;if(є!=null&&(є.Р==π.Αύ||є.Р==π.Αώ))
Ζτ=ChargeMode.Discharge;Γϊ(Ζτ);Γό(!ĉ);}if(!Ś){if(Κ==Ι.έ)
Ś=β!=Ρ.З||ΘΩ(Д,true)||ΑΪ;else
Ś=β!=Ρ.З||ΖΑ();}else{if(!Ă) Τ(Β.ĵ);if(!ĉ) Τ(Β.Αλ);if(!Αά) Τ(Β.ΑЊ);Д=false;}if(Ś&&Ă&&ĉ&&Αά){Γϊ(ChargeMode.Auto);Γό(false);if(β==Ρ.З){if(Κ==Ι.έ){if(щ()==Ο) ΑЇ(І.Ǯ);else if(щ()==Ζ) ΑЇ(Ї.Ǯ);if(щ()==Ο) Ū(Ζ,Ρ.ş);else Ū(Ο,Ρ.Ş);}}Τ(Β.Ǧ);return;}}if(Ϊ==Β.Ǧ){if(Αί){if(!Εσ()) return;
Αί=false;Τ(Β.Ǧ);return;}if(Д){if(!Εσ()){Αί=true;return;}IMyShipConnector ȟ=д(MyShipConnectorStatus.Connected);if(ȟ==null){Τ(Β.с);return;}IMyShipConnector λ=ȟ.OtherConnector;ΖΤ(ȟ,false);ƭ(ȧ,false);Ϋ α=null;Ϋ њ=Κ==Ι.έ?Ο:щ();if(Vector3.Distance(ȟ.GetPosition(),њ.Ε)<5f&&Ο.ι) α=њ;
if(Vector3.Distance(ȟ.GetPosition(),Ζ.Ε)<5f&&Ζ.ι) α=Ζ;if(α!=null){if(!ȕ(α,dockDist*ο,true,out Ѐ)){Ș(у.Ơ);Ё();return;}е(Ѐ,5);й(α.η,α.θ,α.Ђ,false);}else е(γ+λ.WorldMatrix.Forward*dockDist*ο,5);if(β==Ρ.З) Ș(у.ΑЂ);}if(а<wpReachedDist){ǔ(ј,true);Τ(Β.с);return;}}if(Ϊ==Β.ţ)
{if(Д){Ǻ(true);ǔ(ω,false);Ѐ=Ζ.Ε;е(Ѐ,20);й(Ζ.η,Ζ.θ,Ζ.Ђ,false);}if(а<wpReachedDist/2){Τ(Β.Ę);return;}}if(Ϊ==Β.Ĝ){if(Д){Ǻ(true);ǔ(ω,false);int Щ=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,γ);if(ǿ==-1||Њ<ǿ){Щ=i;ǿ=Њ;}}
if(Щ==-1){Τ(Β.И);return;}Ѐ=Φ[Щ].Ε;е(Ѐ,10);й(Ζ.η,Ζ.θ,Ζ.Ђ,false);}if(а<wpReachedDist){Τ(Β.И);return;}}if(Ϊ==Β.Αν){bool ώ=false;if(Д){Ǻ(true);if((б==В.ΓΛ||б==В.Αι)&&Αυ()&&
š(Ǳ,Ω.GetNaturalGravity())<25&&Ќ>=2&&Ь>=2){Vector3 ǁ=γ;if(Ϗ>0&&ό<Ь-1) ǁ=Ȱ(Ϗ-1,ό+1,true);else if(Ϗ<Ќ-1&&ό<Ь-1) ǁ=Ȱ(Ϗ+1,ό+1,true);else if(Ϗ<Ќ-1&&ό>0) ǁ=Ȱ(Ϗ+1,ό-1,true);else if(Ϗ>0&&ό>0) ǁ=Ȱ(Ϗ-1,ό-1,true);else ώ=true;if(!ώ) е(ǁ,10);
}else ώ=true;}if(а<wpReachedDist/2) ώ=true;if(ώ){Τ(Β.ΑЉ);return;}}if(Ϊ==Β.ΓЇ||Ϊ==Β.ΑЉ){if(Д){ǹ(true,true,false,γ,0);ǔ(ω,false);Ǻ(true);Е=-1;р=б==В.ΓΫ||б==В.Ασ?0:-1;}bool ņ=!ΓΓ();int Βό=Ȯ("STONE","ORE",Ш.ĳ);if(б==В.ΓΕ||б==В.Ασ||б==В.Αι)
Βό+=Ȯ("ICE","ORE",Ш.ĳ);bool ΙЌ=Βό>0;bool Δέ=false;if(р>=0){float ŭ=(float) Math.Sin(Εχ(р))*Ȑ/3f;float Ιν=(float) Math.Cos(Εχ(р))*ȏ/3f;Vector3 Ιτ=Ȱ(Ϗ,ό,true)+ΓЅ(Ǳ,ď*-1,new Vector3(ŭ,Ιν,0));е(Ιτ,0.3f);if(а<Math.Min(Ȑ,ȏ)/10f) р+=5f;if(р>=360) р=0;}if(Е==-1||Βό<Е){Е=Βό;ѐ=0;}
else{ѐ++;if(ѐ>50) Δέ=true;}if(!ΙЌ||ņ||Δέ){if(!ņ){int ΚЂ=Ȯ("","ORE",Ш.ĳ);if(Ʀ(true)) ņ=true;else if(100-(Й(ΚЂ,Ησ)*100)<minEjection) ņ=true;else Ș(у.ΑЂ);}if(Δέ&&ņ) Λ="Ejection failed";if(Ϊ==Β.ΑЉ){if(ņ){if(Ο.ι) Τ(Β.Ĝ);else{Ё();ķ();Λ="Can´t return, no dock found";}}
else Τ(Β.ĝ);}else if(ņ) Τ(Β.ĺ);else Τ(Β.ƪ);return;}}Д=false;}void ΘΥ(List<Ϋ>ı,int ρ,List<Vector3>ΑΧ,float ȋ,bool ǳ,ref int ǩ){if(ǳ){for(int i=0;i<ı.Count;i++) ı[i].Ч=0;ǩ=-1;return;}if(ρ==0) return;int ǝ=ρ*-1;if(ǩ==-1) ǩ=ǝ>0?1:ı.Count-2;int ΑΒ=0;while(ǩ>=1&&ǩ<ı.Count-1)
{if(ΑΒ>50) return;ΑΒ++;try{if((ǝ<0&&ǩ>=1)||(ǝ>0&&ǩ<=ı.Count-2)){Ϋ ξ=ı[ǩ];bool Ηο=false;for(int j=0;j<ΑΧ.Count;j++)
if(Vector3.Distance(ξ.Ε,ΑΧ[j])<=ȋ){Ηο=true;break;}if(!Ηο){Ϋ Βο=ı[ǩ-ǝ];Ϋ ő=ı[ǩ+ǝ];Vector3 Κώ=ξ.Ε-ő.Ε;Vector3 ΕΩ=Βο.Ε-ξ.Ε;Vector3 Κί=ξ.Ε+Vector3.Normalize(Κώ)*ΕΩ.Length();Vector3 ΙЁ=Βο.Ε-Κί;Vector3 Ββ=ļ(ρ>0?ξ.θ:ξ.θ*-1,ξ.η*-1,ΙЁ);Vector3 ΚΛ=ļ(ρ>0?ξ.θ:ξ.θ*-1,ξ.η*-1,ΕΩ);
Vector3 Βδ=ļ(ρ>0?ξ.θ:ξ.θ*-1,ξ.η*-1,ξ.φ);ξ.Ч=(float) Math.Sqrt(Math.Pow(Βο.Ч,2)+Math.Pow(Αζ(-ΚΛ,Βδ,ξ),2));for(int j=0;j<ΑΧ.Count;j++)
if(Vector3.Distance(Βο.Ε,ΑΧ[j])<=ȋ){Vector3 Βε=ļ(ρ>0?ξ.θ:ξ.θ*-1,ξ.η*-1,ΑΧ[j]-ξ.Ε);float Κτ=Αζ(-Βε,Βδ,ξ);ξ.Ч=Math.Min(ξ.Ч,Κτ)/2f;}if(Ββ.Length()==0) Ββ=new Vector3(0,0,1);Vector3 ΚΚ=ļ(ξ.θ,ξ.η*-1,ξ.φ);float Κψ=ƣ(-Ββ,ΚΚ,ξ);float ΑЋ=Й(Κψ,Ř);float ΕΦ=(float) Math.Sqrt(Й(Ββ.Length()*1.0f,0.5f*ΑЋ));
ξ.Ч=Math.Min(ξ.Ч,(ΕΩ.Length()/ΕΦ)*ǻ);}}}catch{return;}ǩ+=ǝ;}ǩ=-1;}void ΓІ(bool ǳ){if(ǳ){ΑΓ=0;return;}if(Κ==Ι.έ) return;float ώ=Я*Math.Max(1,л);if(č==Я) ώ+=Math.Min(л,ж);float ΚЁ=Ќ*Ь*Math.Max(1,л);ΑΓ=Math.Max(ΑΓ,(float) Math.Min(ώ/ΚЁ*100.0,100));}MyDetectedEntityType ΕЁ()
{try{if(Ȗ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓΦ(bool Ȅ){if(Κ==Ι.ǣ&&ΕЁ()==MyDetectedEntityType.None&&!ƥ(ψ,true))
return fastSpeed;else return Ȅ?ƞ:Ɲ;}void ΙΧ(){float Ηη=0,ΖЄ=0,ŋ=0,ΔЋ=0;for(int i=0;i<ġ.Count;i++){IMyBatteryBlock Ƹ=ġ[i];if(!Ȗ(Ƹ,true)) continue;Ηη+=Ƹ.MaxStoredPower;ΖЄ+=Ƹ.CurrentStoredPower;ŋ+=Ƹ.CurrentInput;ΔЋ+=Ƹ.CurrentOutput;}Ȝ=(float) Math.Round(Й(ΖЄ,Ηη)*100,1);
if(ŋ>=ΔЋ) Ǽ=ѕ.ĵ;else Ǽ=ѕ.Εψ;if(ŋ==0&&ΔЋ==0||Ȝ==100.0) Ǽ=ѕ.ъ;if(ġ.Count==0) Ǽ=ѕ.None;}void Ιέ(){float ΗΒ=0;for(int i=0;i<ǖ.Count;i++){IMyGasTank ΗЍ=ǖ[i];if(!Ȗ(ΗЍ,true)) continue;ΗΒ+=(float) ΗЍ.FilledRatio;}Ċ=Й(ΗΒ,ǖ.Count)*100f;}void Ιη(){ś=0;try{for(int i=0;i<ĭ.Count;i++)
{IMyReactor ΕΒ=ĭ[i];if(!Ȗ(ΕΒ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΒ.GetInventory(0).GetItems(Ћ);float Љ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem ύ=Ћ[j];if(ύ.Type.SubtypeId.ToUpper()=="URANIUM"&&ύ.Type.TypeId.ToUpper().Contains("_INGOT"))
Љ+=(float) ύ.Amount;}if(Љ<ś||i==0){ś=Љ;Δύ=ΕΒ.CustomName;}}}catch(Exception e){Č=e;}}void ΙΨ(){ȇ=0;ȁ=0;try{for(int i=0;i<Ŵ.Count;i++){IMyTerminalBlock Ψ=Ŵ[i];if(!Ȗ(Ψ,true)) continue;ȁ+=(float) Ψ.GetInventory(0).CurrentVolume;ȇ+=(float) Ψ.GetInventory(0).MaxVolume;
}Βα=(float) Math.Min(Math.Round(Й(ȁ,ȇ)*100,1),100.0);}catch(Exception e){Č=e;}}void Ιή(){try{ǭ.Clear();for(int i=0;i<Ŵ.Count;i++){IMyTerminalBlock Ψ=Ŵ[i];if(!Ȗ(Ψ,true)) continue;Ει(Ψ,Ш.Ŧ);}if(б!=В.Γι){for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Ψ=ŀ[i];if(!Ȗ(Ψ,true)) continue;
Ει(Ψ,Ш.ΘϏ);}}ΙΟ(ǭ);}catch(Exception e){Č=e;}}void ΘΧ(){if(ΒΘ){if(ΕЀ().Count>ΒΪ){ΒΘ=false;if(ё!=ш.Ƥ){Ё();if(ё==ш.ΓΠ) ķ();if(ё==ш.ΑЁ)
if(Ο.ι) Αξ();else ķ();}Λ="Damage detected";}}}bool ΓΓ(){if(!ΒΫ) return true;if(β==Ρ.З){if(ѝ>0&&Ǽ!=ѕ.None)
if(Ȝ<=ѝ){Λ="Low energy! Move home";return false;}if(н>0&&ĭ.Count>0)
if(ś<=н){Λ="Low fuel: "+Δύ;return false;}if(ў>0&&ǖ.Count>0)
if(Ċ<=ў){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕЀ(){List<IMyTerminalBlock>Ń=new List<IMyTerminalBlock>();for(int i=0;i<Ƴ.Count;i++){IMyTerminalBlock Ψ=Ƴ[i];if(ƥ(Ψ,false)) Ń.Add(Ψ);}return Ń;}bool ƥ(IMyTerminalBlock Ψ,bool ΕΞ){return(!Ȗ(Ψ,ΕΞ)||!Ψ.IsFunctional);
}bool Ȗ(IMyTerminalBlock Ψ,bool ΕΞ){if(Ψ==null) return false;try{IMyCubeBlock ΗΘ=Me.CubeGrid.GetCubeBlock(Ψ.Position).FatBlock;if(ΕΞ) return ΗΘ==Ψ;else return ΗΘ.GetType()==Ψ.GetType();}catch{return false;}}float Θβ(IMyTerminalBlock Ψ,String ǐ,String ΗЅ,String[]Ȃ){float Ƞ=0;for(int i=0;i<Ψ.InventoryCount;i++)
{IMyInventory Ż=Ψ.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ż.GetItems(Ћ);foreach(MyInventoryItem ύ in Ћ){if(Ȃ!=null&&(Ȃ.Contains(ύ.Type.TypeId.ToUpper())||Ȃ.Contains(ύ.Type.SubtypeId.ToUpper())))
continue;if((ǐ==""||ύ.Type.TypeId.ToUpper()==ǐ)&&(ΗЅ==""||ύ.Type.SubtypeId.ToUpper()==ΗЅ))
Ƞ+=(float) ύ.Amount;}}return Ƞ;}к Θκ(String ί,String υ,Ш Э,bool Ιύ){ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){к ύ=ǭ[i];if(ύ.ί.ToUpper()==ί&&ύ.υ.ToUpper()==υ&&(ύ.ƀ==Э||Э==Ш.ĳ))
return ύ;}к ή=null;if(Ιύ){ή=new к(ί,υ,0,Э);ǭ.Add(ή);}return ή;}int Ȯ(String ί,String υ,Ш Э){return Ȯ(ί,υ,Э,null);}int Ȯ(String ί,String υ,Ш Э,String[]Ȃ){int Ƞ=0;ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){к ύ=ǭ[i];if(Ȃ!=null&&Ȃ.Contains(ύ.ί.ToUpper())) continue;
if((ί==""||ύ.ί.ToUpper()==ί)&&(υ==""||ύ.υ.ToUpper()==υ)&&(ύ.ƀ==Э||Э==Ш.ĳ))
Ƞ+=ύ.Љ;}return Ƞ;}void Ει(IMyTerminalBlock Ψ,Ш Э){for(int i=0;i<Ψ.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ψ.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
Θκ(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Э,true).Љ+=(int) Ћ[j].Amount;}}void ΙΟ(List<к>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){к a=Ѝ[j];к b=Ѝ[j+1];if(a.Љ<b.Љ) Ѝ.Move(j,j+1);}}bool Ʀ(bool ΛΔ){if(ė&&Κ!=Ι.έ)
if(в!=-1&&Ř>=в){Λ="Ship too heavy";return true;}if(Βα>=Ȇ||ΑΠ){ΑΠ=false;Λ="Ship is full";return true;}return false;}bool ΖΑ(){String[]Ȃ=null;if(!Ė) Ȃ=new string[]{"ICE"};if(Κ==Ι.Ʃ) return Ȯ("","ORE",Ш.Ŧ,Ȃ)==0;if(Κ==Ι.ǣ) return Ȯ("","COMPONENT",Ш.Ŧ,Ȃ)==0;else return Ȯ("","",Ш.Ŧ,Ȃ)==0;
}bool ΘЂ(){List<IMyCockpit>ΔΞ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔΞ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔΞ.Count;i++)
if(ΔΞ[i].IsUnderControl) return true;return false;}bool ΘΩ(bool ǳ,bool Βχ){IMyShipConnector О=д(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return Ετ(І,ǳ,Βχ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return Ετ(Ї,ǳ,Βχ);return false;}bool Ετ(Ŭ Ȟ,bool ǳ,bool Βχ){if(ǳ) Ȟ.ΘА();Ȟ.Ιδ();bool ѓ=false;String Ǐ="";switch(Ȟ.Р){case π.Γλ:{Ǐ="Waiting for command";ѓ=false;break;}case π.ΖΝ:{Ǐ="Waiting for cargo";ѓ=Ʀ(true);break;}case π.ΖΜ:{Ǐ="Unloading";ѓ=ΖΑ();break;}case π.Γπ:{ѓ=true;break;}
case π.Γκ:{Ǐ="Charging batteries";ѓ=Ȝ>=100f;break;}case π.Αώ:{Ǐ="Discharging batteries";ѓ=Ȝ<=25f;break;}case π.Αύ:{Ǐ="Discharging batteries";ѓ=Ȝ<=0f;break;}case π.Γν:{Ǐ="Filling up hydrogen";ѓ=Ċ>=100f;break;}case π.Γξ:{Ǐ="Unloading hydrogen";ѓ=Ċ<=25f;break;}
case π.Γμ:{Ǐ="Unloading hydrogen";ѓ=Ċ<=0f;break;}case π.Γο:{bool ΗΠ=ΘЂ();if(!ΗΠ) Ȟ.ΔЏ=true;ѓ=Ȟ.ΔЏ&&ΗΠ;Ǐ="Waiting for passengers";break;}}if(!ѓ) Ȟ.ΓΒ(true);if(ѓ&&Ȟ.ΓΨ()){ѓ=Ȟ.ΓΒ(false);Ǐ="Undocking in: "+Αο((int) Math.Max(0,Ȟ.Ǫ-Ȟ.ħ));}if(Βχ) ΘΙ=Ǐ;return ѓ;}void ΘΝ()
{if(!ż) return;if(ω.Count<=1) return;float ΘΑ=0,ΗБ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȏ=ω[i];if(ƥ(Ȏ,true)) continue;ΘΑ+=(float) Ȏ.GetInventory(0).MaxVolume;ΗБ+=(float) Ȏ.GetInventory(0).CurrentVolume;}float Ζλ=(float) Math.Round(Й(ΗБ,ΘΑ),5);for(int œ=0;œ<Math.Max(1,Math.Floor(ω.Count/10f));œ++)
{float Δμ=0,Δό=0;float ΗΝ=0,Ηδ=0;IMyTerminalBlock ć=null,Ĩ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȏ=ω[i];if(ƥ(Ȏ,true)) continue;float ΔЃ=(float) Ȏ.GetInventory(0).MaxVolume;float ΒΤ=Й((float) Ȏ.GetInventory(0).CurrentVolume,ΔЃ);if(ć==null||ΒΤ>Δμ){ć=Ȏ;Δμ=ΒΤ;ΗΝ=ΔЃ;}
if(Ĩ==null||ΒΤ<Δό){Ĩ=Ȏ;Δό=ΒΤ;Ηδ=ΔЃ;}}if(ć==null||Ĩ==null||ć==Ĩ) return;if(checkConveyorSystem&&!ć.GetInventory(0).IsConnectedTo(Ĩ.GetInventory(0))){if(ΔΑ>20) Λ="Inventory balancing failed";else ΔΑ++;return;}ΔΑ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
ć.GetInventory(0).GetItems(Ћ);float Αή=0;if(Ћ.Count==0) continue;MyInventoryItem ΒΥ=Ћ[0];String ΗΥ=ΒΥ.Type.TypeId+ΒΥ.Type.SubtypeId;if(!ΘΘ.TryGetValue(ΗΥ,out Αή)){if(ΘЇ(ć.GetInventory(0),0,Ĩ.GetInventory(0),out Αή))
ΘΘ.Add(ΗΥ,Αή);else return;}float ΚϏ=((Δμ-Ζλ)*ΗΝ/Αή);float ΚЀ=((Ζλ-Δό)*Ηδ/Αή);int Εέ=(int) Math.Min(ΚЀ,ΚϏ);if(Εέ<=0) return;if((float) ΒΥ.Amount<Εέ)
ć.GetInventory(0).TransferItemTo(Ĩ.GetInventory(0),0,null,null,ΒΥ.Amount);else
ć.GetInventory(0).TransferItemTo(Ĩ.GetInventory(0),0,null,null,Εέ);}}bool ΘЇ(IMyInventory ΑΡ,int Α,IMyInventory ΙЀ,out float ΘΗ){ΘΗ=0;float Ιρ=(float) ΑΡ.CurrentVolume;List<MyInventoryItem>ΔΓ=new List<MyInventoryItem>();ΑΡ.GetItems(ΔΓ);float Ζο=0;for(int i=0;i<ΔΓ.Count;i++) Ζο+=(float) ΔΓ[i].Amount;
ΑΡ.TransferItemTo(ΙЀ,Α,null,null,1);float ΘΕ=Ιρ-(float) ΑΡ.CurrentVolume;List<MyInventoryItem>ΓЉ=new List<MyInventoryItem>();ΑΡ.GetItems(ΓЉ);float Ζδ=0;for(int i=0;i<ΓЉ.Count;i++) Ζδ+=(float) ΓЉ[i].Amount;if(ΘΕ==0f||!Γά(0.9999,Ζο-Ζδ,1.0001))
return false;ΘΗ=ΘΕ;return true;}void Ιά(){Vector3 ǆ=Ѐ+į;Vector3 ȍ=ǆ-γ;if(ȍ.Length()==0) ȍ=new Vector3(0,0,-1);Vector3 Βε=т(Ω,ȍ);Vector3 ΗЎ=Vector3.Normalize(Βε);Vector3 ſ=т(Ω,Ω.GetNaturalGravity());float Κο=ΔЌ>0?Math.Max(0,1-š(ȍ,ΗΔ)/5):0;float Ч=(float) Math.Min((ΔЀ>0?ΔЀ:1000f),Math.Max(Αζ(-Βε,ſ,null),ΔЌ*Κο));
if(!ΗА) Ч=0;if(ΗϏ) Ч=Math.Max(0,1-ΒΖ/ΑЌ)*Ч;if(generalSpeedLimit>0) Ч=Math.Min(generalSpeedLimit,Ч);if(ƈ) Ч=0;if(ȑ>=0) Ч=Math.Min(Ч,ȑ);if(ΗЀ) Ч*=(float) Math.Min(1,Й(ȍ.Length(),wpReachedDist)/2);Vector3 Ηα=т(Ω,Ω.GetShipVelocities().LinearVelocity);float Ιμ=(float)(Math.Max(0,15-š(-ΗЎ,-Ηα))/15)*0.85f+0.15f;
ΕΣ+=Math.Sign(Ιμ-ΕΣ)/10f;Vector3 ģ=ΗЎ*Ч*ΕΣ-(Ηα);Vector3 ΓА=ΓΥ(ģ,null);if(Βώ&&а>0.1f){ģ.X*=Γώ(ģ.X,ref ΒϏ.X,1f,ΓА.X,20);ģ.Y*=Γώ(ģ.Y,ref ΒϏ.Y,1f,ΓА.Y,20);ģ.Z*=Γώ(ģ.Z,ref ΒϏ.Z,1f,ΓА.Z,20);}else ΒϏ=new Vector3(1,1,1);ΑΨ=Ř*ģ-ſ*Ř;Εκ(ΑΨ,Βώ);а=Vector3.Distance(γ,ǆ);
}float Γώ(float value,ref float Ѓ,float ǝ,float Ď,float ΔЁ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ѓ)) Ѓ+=Math.Sign(Ѓ)*ǝ;else Ѓ=value;if(value==0) Ѓ=1;float ή=Math.Abs(Ѓ);if(ή<ΔЁ||Ď==0) return ή;Ѓ=Math.Min(ΔЁ,Math.Max(-ΔЁ,Ѓ));ή=Math.Abs(Ď);return ή;
}void Ζβ(){float ǎ=90,ǡ=90,Ǜ=90;float Ǵ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȅ,ǘ,ΑΗ,Δω;if(Δϊ){Ȅ=Vector3.Normalize(Ѐ-γ);ΑΗ=т(Ω,Ȅ);ǘ=т(Ω,ΕΠ);ǎ=š(ΑΗ,new Vector3(0,-1,0))-90;ǡ=Ĵ(ǘ,new Vector3(-1,0,0),ǘ.Y);
Ǜ=Ĵ(ΑΗ,new Vector3(-1,0,0),ΑΗ.Z);}else{Ȅ=ΕΡ;ǘ=т(Ω,ΕΠ);ΑΗ=т(Ω,ΕΡ);Δω=т(Ω,ΗЏ);ǎ=Ĵ(ǘ,new Vector3(0,0,1),ǘ.Y);ǡ=Ĵ(ǘ,new Vector3(-1,0,0),ǘ.Y);Ǜ=Ĵ(Δω,new Vector3(0,0,1),Δω.X);}if(Ζζ&&Αυ()){Vector3 Δγ=Ω.GetNaturalGravity();ǘ=т(Ω,Δγ);ǎ=Ĵ(ǘ,new Vector3(0,0,1),ǘ.Y);
ǡ=Ĵ(ǘ,new Vector3(-1,0,0),ǘ.Y);}if(!Γά(-45,ǡ,45)){ǎ=0;Ǜ=0;}if(!Γά(-45,Ǜ,45)) ǎ=0;ΑЄ(Δε,1,(-ǎ)*Ǵ,(-Ǜ)*Ǵ,(-ǡ)*Ǵ);ΒΖ=Math.Max(Math.Abs(ǎ),Math.Max(Math.Abs(ǡ),Math.Abs(Ǜ)));Βά=ΒΖ<=ΑЌ;}void ΘΦ(IMyTerminalBlock Βπ){if(Βπ==null) return;Ƿ=new float[3,2];Ǟ=new Dictionary<string,float[,]>();
for(int i=0;i<ȫ.Count;i++){IMyThrust ц=ȫ[i];if(!ц.IsFunctional) continue;Vector3 ρ=т(Βπ,ц.WorldMatrix.Backward);float Ħ=ц.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ƿ[0,0]+=Ħ;else Ƿ[0,1]-=Ħ;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ƿ[1,0]+=Ħ;else Ƿ[1,1]-=Ħ;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ƿ[2,0]+=Ħ;else Ƿ[2,1]-=Ħ;String ǐ=ΕЈ(ц);float[,]ǟ=null;if(Ǟ.ContainsKey(ǐ)) ǟ=Ǟ[ǐ];else{ǟ=new float[3,2];Ǟ.Add(ǐ,ǟ);}float Ď=ц.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ǟ[0,0]+=Ď;else ǟ[0,1]-=Ď;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ǟ[1,0]+=Ď;else ǟ[1,1]-=Ď;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ǟ[2,0]+=Ď;else ǟ[2,1]-=Ď;}}static String ΕЈ(IMyThrust ц){return ц.BlockDefinition.SubtypeId;}Vector3 Γβ(Vector3 Ǖ,float[,]Ɓ){return new Vector3(
Ǖ.X>=0?Ɓ[0,0]:Ɓ[0,1],
Ǖ.Y>=0?Ɓ[1,0]:Ɓ[1,1],
Ǖ.Z>=0?Ɓ[2,0]:Ɓ[2,1]);}bool ΕЇ(Ϋ ά,String ǐ,out float Δά){Δά=0;int Α=Ȫ.IndexOf(ǐ);if(Α==-1||ά.Ƕ==null||Α>=ά.Ƕ.Length)
return false;Δά=ά.Ƕ[Α];if(Δά==-1) return false;return true;}Vector3 ΓΥ(Vector3 Ǖ,Ϋ ά){if(ά!=null){Vector3 ή=new Vector3();for(int i=0;i<Ǟ.Keys.Count;i++){String ǐ=Ǟ.Keys.ElementAt(i);float ǃ=0;if(!ΕЇ(ά,ǐ,out ǃ))
return Γβ(Ǖ,Ƿ);ή+=Γβ(Ǖ,Ǟ.Values.ElementAt(i))*ǃ;}return ή;}return Γβ(Ǖ,Ƿ);}float ƣ(Vector3 Ǖ,Ϋ ά){return ƣ(Ǖ,new Vector3(),ά);}float ƣ(Vector3 Ǖ,Vector3 φ,Ϋ ά){Vector3 Κχ=ΓΥ(Ǖ,ά);Vector3 Ħ=Κχ+φ*Ř;float ŕ=(Ħ/Ǖ).AbsMin();return(float)(Ǖ*ŕ).Length();}float Αζ(Vector3 Βγ,Vector3 ſ,Ϋ ά)
{if(Βγ.Length()==0) return 0;float Ζε=1;if(ſ.Length()>0) Ζε=Math.Min(1,š(-ſ,Βγ)/90)*0.4f+0.6f;float ΑΨ=ƣ(Βγ,ſ,ά);if(ΑΨ==0) return 0.1f;float ΑЋ=Й(ΑΨ,Ř);float ΕΦ=(float) Math.Sqrt(Й(Βγ.Length(),ΑЋ*0.5f));return ΑЋ*ΕΦ*Ζε*ǻ;}float Ας(Vector3 Ȅ,Vector3 Б,Vector3 φ,Ϋ ά)
{if(φ.Length()==0f) return 0;Vector3 Βδ=ļ(Ȅ,Б,Vector3.Normalize(φ));float Ǭ=ƣ(-Βδ,ά);return Ǭ/φ.Length();}void Εο(bool ΚΒ){float Ǡ=0;float Δΰ=0.9f;if(ΚΒ){в=-1;Ŝ=0;Βω=null;if(β!=Ρ.ы&&Ζ.φ.Length()!=0){Ǡ=Δΰ*Ας(Ζ.θ,Ζ.η*-1,Ζ.φ,null);if(Ǡ<в||в==-1) в=Ǡ;}if(Ο.ι&&Ο.φ.Length()!=0)
{Ǡ=Δΰ*Ας(Ο.θ,Ο.η*-1,Ο.φ,null);if(Ǡ<в||в==-1) в=Ǡ;}return;}if(Ŝ==-1) return;if(Ŝ>=0){int Ηω=0;while(Ŝ<Φ.Count){if(Ηω>100) return;Ηω++;Ϋ ά=Φ[Ŝ];if(ά.φ.Length()!=0f){Ǡ=Δΰ*Math.Min(
Ας(ά.θ,ά.η*-1,ά.φ,ά),
Ας(ά.θ*-1,ά.η*-1,ά.φ,ά));if(Ǡ<в||в==-1) в=Ǡ;}else Βω=ά;Ŝ++;}Ŝ=-1;}bool Δθ=true;float Ĭ=0;if(Βω!=null){for(int i=0;i<Ǟ.Count;i++){String ǐ=Ǟ.Keys.ElementAt(i);float[,]ǟ=Ǟ.Values.ElementAt(i);float ǃ=0;if(!ΕЇ(Βω,ǐ,out ǃ)){Δθ=false;break;}for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float Ŀ=Math.Abs(ǟ[a,b]*ǃ);if(Ŀ==0) continue;Δθ=true;if(Ĭ==0||Ŀ<Ĭ) Ĭ=Ŀ;}}}if(!Δθ){for(int a=0;a<Ƿ.GetLength(0);a++)
for(int b=0;b<Ƿ.GetLength(1);b++){float Ŀ=Math.Abs(Ƿ[a,b]);if(Ŀ==0) continue;if(Ĭ==0||Ŀ<Ĭ) Ĭ=Ŀ;}}if(Ĭ>0){float ΚΞ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǡ=Й(Ĭ,ΚΞ);if(Ǡ>0)
if(Ǡ<в||в==-1) в=Ǡ;}}void Εκ(Vector3 Ǭ,bool М){if(!М){for(int i=0;i<ȫ.Count;i++)
ȫ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓЏ=ΓΥ(Ǭ,null);float ΚЏ=Math.Min(1,Math.Abs(Й(Ǭ.X,ΓЏ.X)));float ΚА=Math.Min(1,Math.Abs(Й(Ǭ.Y,ΓЏ.Y)));float ΚБ=Math.Min(1,Math.Abs(Й(Ǭ.Z,ΓЏ.Z)));for(int i=0;i<ȫ.Count;i++){IMyThrust ц=ȫ[i];Vector3 ρ=ΙΑ(т(Ω,ц.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǭ.X))
ц.SetValueFloat("Override",ц.MaxThrust*ΚЏ);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǭ.Y))
ц.SetValueFloat("Override",ц.MaxThrust*ΚА);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǭ.Z))
ц.SetValueFloat("Override",ц.MaxThrust*ΚБ);else
ц.SetValueFloat("Override",0.0f);}}void ΑЄ(bool М,float Κά,float ǎ,float Ǜ,float ǡ){for(int i=0;i<ΒΧ.Count;i++){IMyGyro К=ΒΧ[i];К.GyroOverride=М;if(!М) К.GyroPower=100;else К.GyroPower=Κά;if(!М) continue;Vector3 Ы=Ω.WorldMatrix.Forward;Vector3 о=Ω.WorldMatrix.Right;Vector3 Б=Ω.WorldMatrix.Up;
Vector3 ŷ=К.WorldMatrix.Forward;Vector3 Ź=К.WorldMatrix.Up;Vector3 Ÿ=К.WorldMatrix.Left*-1;if(ŷ==Ы) К.SetValueFloat("Roll",ǡ);else if(ŷ==(Ы*-1)) К.SetValueFloat("Roll",ǡ*-1);else if(Ź==(Ы*-1)) К.SetValueFloat("Yaw",ǡ);else if(Ź==Ы) К.SetValueFloat("Yaw",ǡ*-1);else if(Ÿ==Ы) К.SetValueFloat("Pitch",ǡ);
else if(Ÿ==(Ы*-1)) К.SetValueFloat("Pitch",ǡ*-1);if(Ÿ==(о*-1)) К.SetValueFloat("Pitch",ǎ);else if(Ÿ==о) К.SetValueFloat("Pitch",ǎ*-1);else if(Ź==о) К.SetValueFloat("Yaw",ǎ);else if(Ź==(о*-1)) К.SetValueFloat("Yaw",ǎ*-1);else if(ŷ==(о*-1)) К.SetValueFloat("Roll",ǎ);
else if(ŷ==о) К.SetValueFloat("Roll",ǎ*-1);if(Ź==(Б*-1)) К.SetValueFloat("Yaw",Ǜ);else if(Ź==Б) К.SetValueFloat("Yaw",Ǜ*-1);else if(Ÿ==Б) К.SetValueFloat("Pitch",Ǜ);else if(Ÿ==(Б*-1)) К.SetValueFloat("Pitch",Ǜ*-1);else if(ŷ==Б) К.SetValueFloat("Roll",Ǜ);else if(ŷ==(Б*-1)) К.SetValueFloat("Roll",Ǜ*-1);
}}void Ζή(){this.Δε=false;}void й(Vector3 ĥ,Vector3 Ы,Vector3 Δχ,float ĕ,bool Αέ){ΒЋ(ĥ,ĕ,Αέ);ΑЌ=ĕ;Δϊ=false;this.ΕΡ=Ы;this.ΗЏ=Δχ;}void й(Vector3 ĥ,Vector3 Ы,Vector3 Δχ,bool Αέ){й(ĥ,Ы,Δχ,2f,Αέ);}void ΒЋ(Vector3 ĥ,float ĕ,bool Αέ){ΑЌ=ĕ;this.Δε=true;this.Ζζ=Αέ;Δϊ=true;
Βά=false;this.ΕΠ=ĥ;}void ΓЃ(){ǹ(false,false,false,Ѐ,0);Βώ=false;}void е(Vector3 ζ,float Ǵ){ǹ(true,false,false,ζ,Ǵ);}void ǹ(bool М,bool ΕΛ,bool Εΰ,Vector3 ζ,float Ǵ){ǹ(М,ΕΛ,Εΰ,ζ,ζ-γ,0.0f,Ǵ);}void ǹ(bool М,bool ΕΛ,bool Εΰ,Vector3 ζ,Vector3 Κι,float Κκ,float Ǵ){Βώ=true;
this.ΗА=М;Ѐ=ζ;this.ΔЀ=Ǵ;this.ΔЌ=Κκ;this.ΗЀ=ΕΛ;this.ΗϏ=Εΰ;this.ΗΔ=Κι;а=Vector3.Distance(ζ,γ);}void ΙΓ(){χ.GetBlocksOfType(Ƴ,ǒ);}void ΙΔ(){if(Βς){А=2;return;}List<IMyRemoteControl>ΑΝ=new List<IMyRemoteControl>();List<IMySensorBlock>Ηύ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑΦ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(ΑΝ,ǒ);χ.GetBlocksOfType(Ц,ǒ);χ.GetBlocksOfType(Ηύ,ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΦ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΦ();ΖΧ(ΑΦ);Ļ(Ц,setLCDFontAndSize,1.4f,false);Ļ(Ȍ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΗЊ=Ǣ(Ηύ,pamTag,true);if(ΗЊ.Count>0) ψ=ΗЊ[0];else ψ=null;if(Κ==Ι.Ʃ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Κ==Ι.ǣ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Κ==Ι.έ){χ.GetBlocksOfType(ΕΧ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΗЉ=Ǣ(ΑΝ,pamTag,true);if(ΗЉ.Count>0) ΑΝ=ΗЉ;if(ΑΝ.Count>0) Ω=ΑΝ[0];else{Ω=null;А=2;Λ="Remote is missing";return;}ο=(float) Ω.CubeGrid.WorldVolume.Radius*2;ǚ=null;if(Κ!=Ι.έ){Επ();if(ω.Count>0&&ǚ!=null){if(ψ!=null&&(ǚ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ω.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(ǚ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
ǚ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void ΙΕ(){χ.GetBlocksOfType(ȧ,ǒ);χ.GetBlocksOfType(ј,ǒ);χ.GetBlocksOfType(ȫ,ǒ);χ.GetBlocksOfType(ΒΧ,ǒ);χ.GetBlocksOfType(ġ,ǒ);χ.GetBlocksOfType(ĭ,ǒ);χ.GetBlocksOfType(ǖ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Βψ,ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ј=Ǣ(ј,"ConnectorMedium",false);else
ј=Ǣ(ј,"Connector",false);List<IMyShipConnector>ΗЇ=Ǣ(ј,pamTag,true);if(ΗЇ.Count>0) ј=ΗЇ;if(А<=1){if(ј.Count==0){А=1;Λ="Connector is missing";}if(ΒΧ.Count==0){А=1;Λ="Gyros are missing";}if(ȫ.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΗЋ=Ǣ(Βψ,pamTag,true);
if(ΗЋ.Count>0) Βψ=ΗЋ;List<IMyLandingGear>ΗЈ=Ǣ(ȧ,pamTag,true);if(ΗЈ.Count>0) ȧ=ΗЈ;for(int i=0;i<ȧ.Count;i++) ȧ[i].AutoLock=false;List<IMyBatteryBlock>ΗІ=Ǣ(ġ,pamTag,true);if(ΗІ.Count>0) ġ=ΗІ;List<IMyGasTank>ΗЌ=Ǣ(ǖ,pamTag,true);if(ΗЌ.Count>0) ǖ=ΗЌ;}void ΙΖ(){χ.GetBlocksOfType(ŀ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ŵ.Clear();for(int i=ŀ.Count-1;i>=0;i--){if(ΘЀ(ŀ[i])){Ŵ.Add(ŀ[i]);ŀ.RemoveAt(i);}}}bool ΘЀ(IMyTerminalBlock Ψ){if(Ψ is IMyCargoContainer) return true;if(Ψ is IMyShipDrill) return true;if(Ψ is IMyShipGrinder) return true;if(Ψ is IMyShipConnector){if(((IMyShipConnector) Ψ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓΩ(Ψ,"ConnectorSmall",false))
return false;else return true;}return false;}void Επ(){if(Ω==null) return;ǚ=null;float Βλ=0,Βζ=0,Βμ=0,Βη=0,Βν=0,Βθ=0;List<IMyTerminalBlock>ΕΟ=Ǣ(ω,pamTag,true);bool ΚΥ=ΕΟ.Count==0;if(ΕΟ.Count>0) ǚ=ΕΟ[0];else if(ω.Count>0) ǚ=ω[0];int Ƞ=0;float Ƌ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward){if(ΚΥ){А=2;Λ="Mining direction is unclear!";return;}continue;}Ƞ++;Ƌ=Math.Max(Ƌ,ΙΥ(ω[i]));Vector3 Н=Αρ(Ω,ω[i].GetPosition());if(i==0){Βλ=Н.X;Βζ=Н.X;Βμ=Н.Y;Βη=Н.Y;Βν=Н.Z;Βθ=Н.Z;}Βζ=Math.Max(Н.X,Βζ);Βλ=Math.Min(Н.X,Βλ);
Βη=Math.Max(Н.Y,Βη);Βμ=Math.Min(Н.Y,Βμ);Βθ=Math.Max(Н.Z,Βθ);Βν=Math.Min(Н.Z,Βν);}if(Ƌ==0) Ƌ=drillRadius;bool ĥ=ǚ!=null&&ǚ.WorldMatrix.Forward==Ω.WorldMatrix.Down;Ȑ=(Βζ-Βλ)*(1-ŝ)+Ƌ*2;ȏ=(ĥ?Βθ-Βν:Βη-Βμ)*(1-ň)+Ƌ*2;IMyCubeGrid g=Ω.CubeGrid;float Εδ=0,Εε=0,Εζ=0,Εη=0;
for(int c=0;c<8;c++){Vector3 p=Αρ(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ĥ?p.Z:p.Y;if(c==0||p.X<Εδ) Εδ=p.X;if(c==0||p.X>Εε) Εε=p.X;if(c==0||y<Εζ) Εζ=y;if(c==0||y>Εη) Εη=y;}Βυ=Εε-Εδ+g.GridSize;Βτ=Εη-Εζ+g.GridSize;}float ΙΥ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǣ<T>(List<T>Ѝ,String ǵ,bool ΒΨ){List<T>ή=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(ΓΩ(Ѝ[i],ǵ,ΒΨ)) ή.Add(Ѝ[i]);return ή;}bool ΓΩ<T>(T Ψ,String ǵ,bool ΒΨ){IMyTerminalBlock b=(IMyTerminalBlock) Ψ;if(ΒΨ&&b.CustomName.ToUpper().Contains(ǵ.ToUpper())) return true;if(!ΒΨ&&b.BlockDefinition.ToString().ToUpper().Contains(ǵ.ToUpper())) return true;return false;}void ǔ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
ΖΤ((IMyTerminalBlock) Ѝ[i],М);}void ΖΤ(IMyTerminalBlock Ψ,bool М){if(Ψ==null) return;String Ικ=М?"OnOff_On":"OnOff_Off";var a=Ψ.GetActionWithName(Ικ);a.Apply(Ψ);}void Ǻ(bool М){ΕΜ=М;if(!ř) return;ǔ(Βψ,М);}void Γϊ(ChargeMode Џ){for(int i=0;i<ġ.Count;i++) ġ[i].ChargeMode=Џ;}void Γό(bool Κρ)
{for(int i=0;i<ǖ.Count;i++) ǖ[i].Stockpile=Κρ;}void ƭ(List<IMyLandingGear>Δα,bool Ηβ){for(int i=0;i<Δα.Count;i++){if(Ηβ) Δα[i].Lock();if(!Ηβ) Δα[i].Unlock();}}void ΖΥ(bool М){if(ΖЅ==М) return;List<IMyShipController>ΒΔ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΔ,ǒ);if(ΒΔ.Count==0) return;
for(int i=0;i<ΒΔ.Count;i++)
ΒΔ[i].DampenersOverride=М;ΖЅ=М;}IMyShipConnector д(MyShipConnectorStatus Ǐ){for(int i=0;i<ј.Count;i++){if(!Ȗ(ј[i],true)) continue;if(ј[i].Status==Ǐ) return ј[i];}return null;}void Ľ(bool ΕΗ,bool ΒΚ,float ΙЊ,float Ιπ){if(ψ==null||ω.Count==0) return;Vector3 ΑБ=new Vector3();
int Ƞ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward) continue;Ƞ++;ΑБ+=ω[i].GetPosition();}ΑБ=ΑБ/Ƞ;Vector3 ƅ=Αρ(ψ,ΑБ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΗ;ψ.LeftExtend=(ΒΚ?1:я)/2f*Ȑ-ƅ.X;ψ.RightExtend=(ΒΚ?1:я)/2f*Ȑ+ƅ.X;ψ.TopExtend=(ΒΚ?1:ю)/2f*ȏ+ƅ.Y;
ψ.BottomExtend=(ΒΚ?1:ю)/2f*ȏ-ƅ.Y;ψ.FrontExtend=(ΕΗ?л:ΙЊ)-ƅ.Z;ψ.BackExtend=ΕΗ?0:Ιπ+ο*0.75f+ƅ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void Ļ<T>(List<T>Ѝ,bool Κμ,float ΙЈ,bool ΚΓ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface ΑΥ=null;if(Ѝ[i]is IMyTextSurface) ΑΥ=(IMyTextSurface) Ѝ[i];if(ΑΥ!=null){ΑΥ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Κμ) continue;ΑΥ.Font="Debug";if(ΚΓ) continue;ΑΥ.FontSize=ΙЈ;}}}bool Αυ(){Vector3D ν;return this.Ω.TryGetPlanetPosition(out ν);}int Θτ(Vector3 Κσ,int ΗЁ){if(ΗЁ==-1) return 0;double Щ=-1;int Ą=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,Κσ);if(Щ==-1||Њ<Щ){Ą=i;Щ=Њ;}
}return Math.Sign(Ą-ΗЁ);}bool ΘЁ(Vector3 ν){List<Vector3>Ю=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Ю.Add(Φ[i].Ε);if(Ο.ι&&Φ.Count>=1){Vector3 ů=new Vector3();ȕ(Ο,dockDist*ο,false,out ů);if(Vector3.Distance(Ο.Ε,Φ.First().Ε)<Vector3.Distance(Ο.Ε,Φ.Last().Ε)){Ю.Insert(0,ů);Ю.Insert(0,Ο.Ε);}
else{Ю.Add(ů);Ю.Add(Ο.Ε);}}if(Κ==Ι.έ){if(Ζ.ι&&Φ.Count>=1){Vector3 ΓЋ=new Vector3();ȕ(Ζ,dockDist*ο,false,out ΓЋ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Ю.Insert(0,ΓЋ);Ю.Insert(0,Ζ.Ε);}else{Ю.Add(ΓЋ);Ю.Add(Ζ.Ε);}}}else{if(β!=Ρ.ы)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Ю.Insert(0,Ζ.Ε);else Ю.Add(Ζ.Ε);}int Ą=-1;double ǿ=-1;for(int i=0;i<Ю.Count;i++){double Њ=Vector3.Distance(Ю[i],ν);if(Њ<ǿ||ǿ==-1){ǿ=Њ;Ą=i;}}if(Ю.Count==0) return false;double ΔΦ=Vector3.Distance(Ю[Ą],ν);double Κέ=Vector3.Distance(Ю[Math.Max(0,Ą-1)],Ю[Ą])*1.5f;
double ΚΤ=Vector3.Distance(Ю[Math.Min(Ю.Count-1,Ą+1)],Ю[Ą])*1.5f;return ΔΦ<Κέ||ΔΦ<ΚΤ||ΔΦ<laneOffset*ο*1.5f;}static float Й(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΙΑ(Vector3 v,int ΔΣ){return new Vector3(Math.Round(v.X,ΔΣ),Math.Round(v.Y,ΔΣ),Math.Round(v.Z,ΔΣ));
}float š(Vector3 a,Vector3 b){if(a==b) return 0;float ΙЅ=(a*b).Sum;float Ηέ=a.Length();float Ηή=b.Length();if(Ηέ==0||Ηή==0) return 0;float ή=(float)((180.0f/Math.PI)*Math.Acos(ΙЅ/(Ηέ*Ηή)));if(float.IsNaN(ή)) return 0;return ή;}float Ĵ(Vector3 v,Vector3 Βπ,float Κν){float ŭ=š(v,Βπ);
if(Κν>0f) ŭ*=-1;if(ŭ>-90f) return ŭ-90f;else return 180f-(-ŭ-90f);}double Εχ(float Ιώ){return(Math.PI/180)*Ιώ;}bool Γά(double ΚΝ,double value,double Ƃ){return(value>=ΚΝ&&value<=Ƃ);}Vector3 ΙΦ(IMyTerminalBlock Ψ,Vector3 Н){return Vector3D.Transform(Н,Ψ.WorldMatrix);
}Vector3 Αρ(IMyTerminalBlock Ψ,Vector3 ΚЌ){return т(Ψ,ΚЌ-Ψ.GetPosition());}Vector3 т(IMyTerminalBlock Ψ,Vector3 Εγ){return Vector3D.TransformNormal(Εγ,MatrixD.Transpose(Ψ.WorldMatrix));}Vector3 ļ(Vector3 Ȅ,Vector3 Б,Vector3 Εγ){MatrixD ΔϏ=MatrixD.CreateFromDir(Ȅ,Б);return Vector3D.TransformNormal(Εγ,MatrixD.Transpose(ΔϏ));
}Vector3 ΓЅ(Vector3 Ȅ,Vector3 Б,Vector3 Н){MatrixD ΔϏ=MatrixD.CreateFromDir(Ȅ,Б);return Vector3D.Transform(Н,ΔϏ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Π(τ,0)),float.Parse(Π(τ,1)),float.Parse(Π(τ,2)));
}String Ιθ(Ϋ ά){String П=":";String ή=σ(ά.Ε)+П+σ(ά.θ)+П+
σ(ά.η)+П+σ(ά.Ђ)+П+σ(ά.φ);for(int i=0;i<ά.Ƕ.Length;i++)
ή+=П+Math.Round(ά.Ƕ[i],3);return ή;}Ϋ ΙΠ(String s){String[]τ=s.Split(':');Ϋ ά=new Ϋ(
ς(Π(τ,0)),ς(Π(τ,1)),
ς(Π(τ,2)),ς(Π(τ,3)),
ς(Π(τ,4)));int Α=5;List<float>ΗΑ=new List<float>();while(Α<τ.Length){String ΒЅ=Π(τ,Α);float f=0;if(!float.TryParse(ΒЅ,out f)) break;ΗΑ.Add(f);Α++;}ά.Ƕ=ΗΑ.ToArray();return ά;}System.Text.StringBuilder Ə=new System.Text.StringBuilder();void Υ<T>(T value,bool ΚΣ){if(ΚΣ) Ə.Append('\n');Ə.Append(value);}
void Υ<T>(T value){Υ(value,true);}String Ακ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑЈ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Π(String[]Ιξ,int Α){String ΒЅ=Ιξ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ΒЅ)) return "";
return ΒЅ;}void Save(){if(Βς||Κ==Ι.і){Storage="";return;}Storage=ΒБ();}String ΒБ(){Ə.Clear();Ə.Append(DATAREV+";");Υ(σ(Βρ),false);Υ(σ(Ο.θ));Υ(σ(Ο.Ђ));Υ(σ(Ο.η));Υ(σ(Ο.φ));Υ(σ(Ο.Ε));Υ(σ(Ο.м));Υ(Ο.ι);Υ(σ(Ζ.Ε));Υ(σ(Ζ.φ));Υ(σ(Ζ.θ));Υ(σ(Ζ.η));Υ(σ(Ζ.Ђ));Υ(σ(Ζ.м));Υ(Ζ.ι);Υ(σ(Ǳ));Υ(σ(ď));Υ(";");
Υ((int) Κ,false);Υ((int) β);Υ((int) ќ);Υ(Ȇ);Υ(ѝ);Υ(н);Υ(ў);Υ(ǻ);Υ(ė);Υ(Ė);if(Κ==Ι.έ){Υ((int) І.Р);Υ(І.Ǫ);Υ(І.ħ);Υ(Ακ(І.ǫ));Υ(Ακ(І.Ǯ));Υ((int) Ї.Р);Υ(Ї.Ǫ);Υ(Ї.ħ);Υ(Ακ(Ї.ǫ));Υ(Ακ(Ї.Ǯ));}else{Υ((int) Ȋ);Υ((int) ё);Υ((int) б);Υ((int) љ);Υ((int) Ō);Υ(İ);Υ(ř);Υ(ż);Υ(Ŷ);Υ(я);Υ(ю);Υ(л);Υ(ƞ);Υ(Ɲ);
Υ(ŝ);Υ(ň);Υ(Ќ);Υ(Ь);Υ(Ϗ);Υ(ό);Υ(č);Υ(Я);Υ(Ŗ);Υ(ŏ);}Υ(";");for(int i=0;i<Ȫ.Count;i++)
Υ((i>0?"|":"")+Ȫ[i],false);Υ(";");for(int i=0;i<Φ.Count;i++)
Υ(Ιθ(Φ[i]),i>0);Υ(";",false);Υ(з+"|"+Г,false);Υ(";",false);Υ(Θε(),false);String Ξ=Ə.ToString();Ə.Clear();return Ξ;}ї ΘБ(){return ΑЀ(Storage);}ї ΑЀ(String Ξ){if(Ξ=="") return ї.ΘЉ;String[]ŗ=Ξ.Split(';');if(Π(ŗ,0)!=DATAREV) return ї.Γθ;int Α=0;try{String[]Ν=Π(ŗ,1).Split('\n');
Βρ=ς(Π(Ν,Α++));Ο.θ=ς(Π(Ν,Α++));Ο.Ђ=ς(Π(Ν,Α++));Ο.η=ς(Π(Ν,Α++));Ο.φ=ς(Π(Ν,Α++));Ο.Ε=ς(Π(Ν,Α++));Ο.м=ς(Π(Ν,Α++));Ο.ι=bool.Parse(Π(Ν,Α++));Ζ.Ε=ς(Π(Ν,Α++));Ζ.φ=ς(Π(Ν,Α++));Ζ.θ=ς(Π(Ν,Α++));Ζ.η=ς(Π(Ν,Α++));Ζ.Ђ=ς(Π(Ν,Α++));Ζ.м=ς(Π(Ν,Α++));Ζ.ι=bool.Parse(Π(Ν,Α++));
Ǳ=ς(Π(Ν,Α++));ď=ς(Π(Ν,Α++));Ν=Π(ŗ,2).Split('\n');Α=0;Κ=(Ι) int.Parse(Π(Ν,Α++));β=(Ρ) int.Parse(Π(Ν,Α++));ќ=(Ρ) int.Parse(Π(Ν,Α++));Ȇ=int.Parse(Π(Ν,Α++));ѝ=int.Parse(Π(Ν,Α++));н=int.Parse(Π(Ν,Α++));ў=int.Parse(Π(Ν,Α++));ǻ=float.Parse(Π(Ν,Α++));ė=bool.Parse(Π(Ν,Α++));
Ė=bool.Parse(Π(Ν,Α++));if(Κ==Ι.έ){І.Р=(π) int.Parse(Π(Ν,Α++));І.Ǫ=float.Parse(Π(Ν,Α++));І.ħ=float.Parse(Π(Ν,Α++));І.ǫ=ΑЈ(Π(Ν,Α++));І.Ǯ=ΑЈ(Π(Ν,Α++));Ї.Р=(π) int.Parse(Π(Ν,Α++));Ї.Ǫ=float.Parse(Π(Ν,Α++));Ї.ħ=float.Parse(Π(Ν,Α++));Ї.ǫ=ΑЈ(Π(Ν,Α++));Ї.Ǯ=ΑЈ(Π(Ν,Α++));}else
{Ȋ=(ь) int.Parse(Π(Ν,Α++));ё=(ш) int.Parse(Π(Ν,Α++));б=(В) int.Parse(Π(Ν,Α++));љ=(Ǒ) int.Parse(Π(Ν,Α++));Ō=(ь) int.Parse(Π(Ν,Α++));İ=bool.Parse(Π(Ν,Α++));ř=bool.Parse(Π(Ν,Α++));ż=bool.Parse(Π(Ν,Α++));Ŷ=bool.Parse(Π(Ν,Α++));я=int.Parse(Π(Ν,Α++));ю=int.Parse(Π(Ν,Α++));л=int.Parse(Π(Ν,Α++));
ƞ=float.Parse(Π(Ν,Α++));Ɲ=float.Parse(Π(Ν,Α++));ŝ=float.Parse(Π(Ν,Α++));ň=float.Parse(Π(Ν,Α++));Ќ=int.Parse(Π(Ν,Α++));Ь=int.Parse(Π(Ν,Α++));Ϗ=int.Parse(Π(Ν,Α++));ό=int.Parse(Π(Ν,Α++));č=int.Parse(Π(Ν,Α++));Я=int.Parse(Π(Ν,Α++));Ŗ=int.Parse(Π(Ν,Α++));ŏ=float.Parse(Π(Ν,Α++));
}Ν=Π(ŗ,3).Replace("\n","").Split('|');Ȫ=Ν.ToList();Ν=Π(ŗ,4).Split('\n');Φ.Clear();if(Ν.Count()>=1&&Ν[0]!="")
for(int i=0;i<Ν.Length;i++)
Φ.Add(ΙΠ(Π(Ν,i)));Ν=Π(ŗ,5).Split('|');з=Π(Ν,0);if(!int.TryParse(Π(Ν,1),out Г)) Г=-1;if(з.Length!=Ќ*Ь){з="";Г=-1;}Θδ(Π(ŗ,6));}catch{return ї.Εϊ;}Ɛ=ќ;Ё();return ї.ΑІ;}const int Εξ=64000;IMyTerminalBlock Εϋ(){List<IMyTerminalBlock>Ų=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ų,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ų.Count>0?Ų[0]:null;}void Θι(){IMyTerminalBlock Ψ=Εϋ();if(Ψ==null){Λ="Export: name a block "+dataTag;return;}String Ξ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒБ();if(Ξ.Length>Εξ){Λ="Export too large: "+Ξ.Length+" chars, max "+Εξ;return;}Ψ.CustomData=Ξ;Λ="Exported "+Φ.Count+" waypoints";}void Θύ(bool Δρ){IMyTerminalBlock Ψ=Εϋ();if(Ψ==null){Λ="Import: name a block "+dataTag;return;}
Θώ(Ψ.CustomData,Δρ);}void Θώ(String Ξ,bool Δρ){Ξ=Ξ.Replace("\r","");if(Ξ.StartsWith("//")){int Ηπ=Ξ.IndexOf('\n');Ξ=Ηπ<0?"":Ξ.Substring(Ηπ+1);}Ξ=Ξ.Trim();if(Ξ==""){Λ="Import: no PAM data in "+dataTag;return;}Ё();String Ζν=ΒБ();ї ή=ΑЀ(Ξ);if(ή==ї.ΑІ&&Δρ){Ϋ Χ=new Ϋ(Ζ);
Vector3 ΚΠ=Ǳ,ΚΟ=ď;int w=я,h=ю,d=л;ь ΑΣ=Ȋ;Ǒ ΔΤ=љ;float ΚΨ=ŝ,ΚΧ=ň;ΑЀ(Ζν);Ζ=Χ;Ǳ=ΚΠ;ď=ΚΟ;я=w;ю=h;л=d;Ȋ=ΑΣ;љ=ΔΤ;ŝ=ΚΨ;ň=ΚΧ;β=Ρ.Ũ;Ʊ(true,true);β=Ρ.Ũ;ќ=Ρ.Ũ;Λ="Imported job, run CONT";return;}if(ή==ї.ΑІ){ΑΚ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}ΑЀ(Ζν);Λ=ή==ї.Γθ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖΦ(){ń.Clear();for(int i=Ц.Count-1;i>=0;i--){String Ξ=Ц[i].CustomData.ToUpper();bool Δο=false;if(Ξ=="INSTRUCTIONS"){Δο=true;ΒΙ=true;}if(Ξ=="DEBUG") Δο=true;if(Δο){ń.Add(Ц[i]);Ц.RemoveAt(i);}
}Ļ(ń,false,1,false);}void ΖΧ(List<IMyTerminalBlock>Ų){Ȍ.Clear();for(int i=0;i<Ų.Count;i++){IMyTerminalBlock Ψ=Ų[i];try{String Ηψ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Ψ.CustomName.IndexOf(Ηψ);int ƕ=-1;if(Α<0||!int.TryParse(Ψ.CustomName.Substring(Α+Ηψ.Length,1),out ƕ)) continue;
if(ƕ==-1) continue;ƕ--;IMyTextSurfaceProvider Ηϊ=(IMyTextSurfaceProvider) Ψ;if(ƕ<Ηϊ.SurfaceCount&&ƕ>=0)
Ȍ.Add(Ηϊ.GetSurface(ƕ));}catch{}}}String Βι="";void ΖΠ(){String Δώ="";String ǂ="";Βι=Εν(false);Δώ+=Βι;ǂ+=Βι;ǂ+=Θρ();for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Δώ);for(int i=0;i<Ȍ.Count;i++) Ȍ[i].WriteText(Δώ);Echo(ǂ);for(int i=0;i<ń.Count;i++){IMyTextPanel х=ń[i];
String Ξ=х.CustomData.ToUpper();if(Ξ=="DEBUG") х.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") х.WriteText(ΕЂ());}}string ΕЂ(){String Γ="";try{float Ηε=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ű*Ηε)+" / "+(int)(ƃ*Ηε)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ű*100f,1)+"% / "+Math.Round(ƃ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ʒ,2)+"ms / "+Math.Round(ΔЂ,2)+"ms \n";}catch{}for(int i=0;i<Đ.Count;i++)
Γ+=""+Đ.Keys.ElementAt(i)+" = "+Math.Round(Đ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Đ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Θϊ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ļ(Ц,setLCDFontAndSize,0.9f,false);Ļ(Ȍ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Γ+Č.ToString());for(int i=0;i<Ȍ.Count;i++) Ȍ[i].WriteText(Γ+Č.ToString());}void ș(){if(!ΒΙ) return;try{ΔА=Runtime.CurrentInstructionCount;}catch{}
}void Ȕ(String ί){if(!ΒΙ) return;if(ΔА==0) return;try{float ΒЃ=(Runtime.CurrentInstructionCount-ΔА)/Runtime.MaxInstructionCount*100;if(!Đ.ContainsKey(ί))
Đ.Add(ί,new float[]{ΒЃ,ΒЃ});else{Đ[ί][0]=ΒЃ;Đ[ί][1]=Math.Max(ΒЃ,Đ[ί][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΘΞ(String ȃ){if(!ų) return;String П=""+BROADCAST_SEP;String ε=VERSION+П;ε+=Ɵ(Ɠ)+П;
ε+=(int) Κ+П;ε+=Ť(γ.X)+""+П;ε+=Ť(γ.Y)+П;ε+=Ť(γ.Z)+П;if(Κ!=Ι.έ)
ε+=ΓΡ(ќ)+(ќ==Ρ.З?" "+Math.Round(ΑΓ,1)+"%":"")+П;else ε+=ΓΡ(ќ)+П;ε+=Ɵ(Λ)+П;ε+=Ў+""+П;ε+=Ɵ(Βι)+П;ε+=ȁ+П;ε+=ȇ+П;for(int i=0;i<ǭ.Count;i++){if(ǭ[i].ƀ!=Ш.Ŧ) continue;ε+=Ɵ(ǭ[i].ί)+"/"+Ɵ(ǭ[i].υ)+"/"+ǭ[i].Љ+П;}ΙΚ(ε,ȃ);}String Ɵ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΙΚ(String ε,String ȃ){Γψ(ε,BROADCAST_SELF_ID,ȃ);}void Γψ(String ε,String ζ,String ȃ){try{if(!ų) return;ε=BROADCAST_TAG+BROADCAST_SEP+Εώ()+BROADCAST_SEP+ζ+BROADCAST_SEP+ΔΖ+BROADCAST_SEP+ȃ+BROADCAST_SEP+ε;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ε);}catch(Exception e){Č=e;}
}String Εώ(){if(Κ!=Ι.і) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γα(String ε){return ε.StartsWith(BROADCAST_TAG);}bool г(ref String Ξ,out string ǅ,Char П){int Α=Ξ.IndexOf(П);ǅ="";if(Α<0) return false;ǅ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΖΟ(ref String ε,out String ȉ,out String ȃ)
{ȉ="";ȃ="";if(!ų) return false;String ǵ="";if(!г(ref ε,out ǵ,BROADCAST_SEP)||!Γα(ǵ)) return false;if(!г(ref ε,out ȉ,BROADCAST_SEP)) return false;if(!г(ref ε,out ǵ,BROADCAST_SEP)||(ǵ!=Εώ()&&(ǵ!="*"&&Κ!=Ι.і))) return false;if(!г(ref ε,out ǵ,BROADCAST_SEP)||(ǵ!=ΔΖ)) return false;
if(!г(ref ε,out ȃ,BROADCAST_SEP)) return false;return true;}void ΕЏ(){bool ƻ=true;if(ΗΓ){ΗΓ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ǋ=(Κ!=Ι.і?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ǋ)
ΕА(ǋ);String[]Ν=Me.CustomData.Split('\n');ų=bool.Parse(ΑϏ(Αμ(Ν,"Enable_Broadcast",ref ƻ)));bool Εα=false;bool ΖЉ=true;if(ų){if(Κ!=Ι.і)
Ɠ=ΑϏ(Αμ(Ν,"Ship_Name",ref ƻ)).Replace(BROADCAST_SELF_ID,"");ΔΖ=ΑϏ(Αμ(Ν,"Broadcast_Channel",ref ƻ)).ToLower();ΖЉ=false;if(Ǩ==null){Ǩ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǩ.SetMessageCallback("");}List<IMyRadioAntenna>Ů=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ů);bool ΓЊ=false;
for(int i=0;i<Ů.Count;i++)
if(Ů[i].EnableBroadcasting&&Ů[i].Enabled){ΓЊ=true;break;}if(Ů.Count==0) Λ="No Antenna found";else if(!ΓЊ) Λ="Antenna not ready";Εα=Ů.Count==0||!ΓЊ;if(Ζη&&!Εα&&Κ!=Ι.і) Λ="Antenna ok";}else if(Κ==Ι.і)
Λ="Offline - Enable in PB custom data";Ζη=Εα;if(ΖЉ){if(Ǩ!=null) this.IGC.DisableBroadcastListener(Ǩ);Ǩ=null;}}catch{ƻ=false;}if(!ƻ){Λ="Reset custom data";ΕА(ǋ);}}void ΕА(String ǋ){String Ξ=ǋ+"\n\n"+"Enable_Broadcast="+(Κ==Ι.і?"true":"false")+" \n";Ξ+=Κ!=Ι.і?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ΑϏ(String Ŏ){int Α=Ŏ.IndexOf("//");if(Α!=-1) Ŏ=Ŏ.Substring(0,Α);String[]τ=Ŏ.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αμ(String[]Ν,String Ȧ,ref bool Ȩ){foreach(String Ŏ in Ν)
if(Ŏ.StartsWith(Ȧ)) return Ŏ;Ȩ=false;return "";}void Θα(string Т){if(Ć) Ъ=new List<Є>();String ȉ="";String ȃ="";if(!Γα(Т)) ΘЏ(Т);bool Ηώ=false;int ΔЅ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔЅ-->0){MyIGCMessage ε=Ǩ.AcceptMessage();String Ξ=ε.Data as String;if(Ξ==null) continue;
if(ΖΟ(ref Ξ,out ȉ,out ȃ)&&ȉ!=""&&ȉ!=BROADCAST_SELF_ID){Є Δ=ΓΟ(ȉ,"");if(Δ==null){Δ=new Є(ȉ);Ъ.Add(Δ);}Δ.Ĺ(ȃ,false,false,true);Ιγ(Δ,Ξ);Ηώ=true;}}if(Ηώ)
Ъ.Sort(delegate(Є a,Є b){if(a.ί==null&&b.ί==null) return 0;else if(a.ί==null) return-1;else if(b.ί==null) return 1;else return a.ί.CompareTo(b.ί);});if(ċ||Ć){if(ΑΞ<=0||Ć){Λ="";Ć=false;ΙΘ();ΕЏ();Δφ=DateTime.Now;}}if(ž) ΙΫ();if(ž) Ιί();ΙΩ();}void Ιγ(Є Δ,String Ξ){Δ.Δσ=DateTime.Now;
String ǅ="";г(ref Ξ,out Δ.ΒЇ,BROADCAST_SEP);г(ref Ξ,out Δ.ί,BROADCAST_SEP);if(Δ.ΒЇ!=VERSION) return;String ΑΙ="";String[]Ɗ=new string[3];г(ref Ξ,out ΑΙ,BROADCAST_SEP);г(ref Ξ,out Ɗ[0],BROADCAST_SEP);г(ref Ξ,out Ɗ[1],BROADCAST_SEP);г(ref Ξ,out Ɗ[2],BROADCAST_SEP);г(ref Ξ,out Δ.ΕΝ,BROADCAST_SEP);
г(ref Ξ,out Δ.Λ,BROADCAST_SEP);г(ref Ξ,out ǅ,BROADCAST_SEP);г(ref Ξ,out Δ.Βκ,BROADCAST_SEP);String ΘΖ="",Ηθ="";г(ref Ξ,out ΘΖ,BROADCAST_SEP);г(ref Ξ,out Ηθ,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String ΗΦ;if(!г(ref Ξ,out ΗΦ,BROADCAST_SEP)) break;String[]Βή=ΗΦ.Split('/');if(Βή.Count()<3) continue;
int Љ=0;if(!int.TryParse(Βή[2],out Љ)) continue;Δ.Ћ.Add(new к(Βή[0],Βή[1],Љ,Ш.Ŧ));}int Ην=0;if(!int.TryParse(ǅ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΙ,out Ην)) Δ.Κ=Ι.ğ;if(!float.TryParse(Ɗ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ɗ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(Ɗ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(ΘΖ,out Δ.ȁ)) Δ.ȁ=0;if(!float.TryParse(Ηθ,out Δ.ȇ)) Δ.ȇ=0;Δ.Κ=(Ι) Ην;Δ.ΔΧ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΕΪ=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.ΕΪ+=Δ.Ћ[i].Љ;}Є ΓΟ(String ΰ,String ί){ΰ=ΰ.ToUpper();ί=ί.ToUpper();for(int i=0;i<Ъ.Count;i++)
{if(ΰ!=""&&Ъ[i].ΰ.ToUpper()==ΰ) return Ъ[i];if(ί!=""&&Ъ[i].ί!=null&&Ъ[i].ί.ToUpper()==ί) return Ъ[i];}return null;}String ŵ="";Є Ζχ=null;DateTime Ζψ=DateTime.MinValue;List<Є>Ł=new List<Є>();const float ΘΣ=20f;bool ΖΩ(Є ζ,String Ѕ){return Ѕ==ŵ&&ζ==Ζχ&&(DateTime.Now-Ζψ).TotalSeconds<ΘΣ;
}String Θλ(Є ζ){if(ŵ==""||!ΖΩ(ζ,ŵ)) return "";String s="";int Ȩ=0,ΕΘ=0;for(int i=0;i<Ł.Count;i++){Ј Βϊ=Ł[i].Ĺ(ŵ,false,false,false);if(Βϊ==Ј.ƫ) Ȩ++;if(ΕΘ++<8) s+="  "+Ł[i].ί+": "+(Βϊ==Ј.ƫ?"received":Βϊ==Ј.ũ?"pending...":Βϊ==Ј.Αό?"no answer!":"-")+"\n";
}if(ΕΘ>8) s+="  +"+(ΕΘ-8)+" more\n";return "--------------------------------------------\n"+ŵ+": "+Ȩ+"/"+Ł.Count+" received\n"+s;}void Ǥ(Є Δ,String Ѕ){ŵ=Ѕ;Ζχ=Δ;Ζψ=DateTime.Now;Ł.Clear();if(Δ==null) Ł.AddRange(Ъ);else Ł.Add(Δ);if(Δ==null){for(int i=0;i<Ъ.Count;i++)
Ъ[i].Ĺ(Ѕ,true,false,false);Γψ(Ѕ,"*",Ѕ);}else{Δ.Ĺ(Ѕ,true,false,false);Γψ(Ѕ,Δ.ΰ,Ѕ);}}void ΘЏ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var ț=τ.ElementAtOrDefault(1);String ȥ="Invalid argument: "+Т;if(ϋ==Σ.Ʈ){if(Л!=null)
{switch(Ѕ){case "UP":{if(Ў<2) break;if(Л.Ў==0){Ў=1;return;}Ǥ(Л,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){Ǥ(Л,"MRES");break;}Ǥ(Л,"DOWN");break;}case "APPLY":{if(Ў<2) break;Ǥ(Л,"APPLY");return;}}}}switch(Ѕ){case "UP":this.Γε(false);return;case "DOWN":this.Γδ(false);return;
case "APPLY":this.Ţ(true);return;}switch(Ѕ){case "CLEAR":Θά();return;case "SENDALL":
if(String.IsNullOrEmpty(ț)){Λ="Usage: SENDALL <cmd>";return;}Ǥ(null,ț);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΙΛ(Т.Remove(0,"SEND".Length+1));return;}Λ=ȥ;}void ΙΛ(String Ƶ){if(Ƶ=="") return;var τ=Ƶ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ɠ=τ.ElementAtOrDefault(0).Trim();
Є Δ=ΓΟ("",Ɠ);if(Δ!=null) Ǥ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ɠ;}void Θά(){Ъ.Clear();Л=null;κ(Σ.Main);Ў=0;}void ΙΘ(){List<IMyTerminalBlock>ΑΦ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ц,ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΦ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΧ(ΑΦ);ΖΦ();ΙΙ();Ļ(Ц,setLCDFontAndSize,1.15f,false);Ļ(Ȍ,setLCDFontAndSize,1.15f,true);String ΙА=ΕЃ(false);String Ηρ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock х in Ц){if(х.CustomData==""){х.CustomData=ΕЃ(true);continue;}
if(!х.CustomData.Contains(ΙА))
if(!х.CustomData.Contains(Ηρ)) х.CustomData=Ηρ+х.CustomData;}}void ΙΩ(){String ǂ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȍ.Count;i++)
Ȍ[i].WriteText(Ελ(С.ΖΘ,"0",false));for(int i=0;i<Ц.Count;i++){С Џ=С.ğ;String ǲ="";ΘЋ(Ц[i],out Џ,out ǲ);Ц[i].WriteText(Ελ(Џ,ǲ,i==0));}Echo(ǂ);for(int i=0;i<ń.Count;i++){IMyTextPanel х=ń[i];String Ξ=х.CustomData.ToUpper();if(Ξ=="DEBUG") х.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") х.WriteText(ΕЂ());
}}String Ţ(bool Θ){int ΙІ=0;return Ţ(Θ,0,ref ΙІ,false,1);}String Ţ(bool Θ,int ΚΪ,ref int Κθ,bool Ζϋ,int ΑΛ){String Γ="";String ƒ="——————————————————\n";String Κφ="--------------------------------------------\n";if(Л==null||ϋ==Σ.Main||ϋ==Σ.Γω)
Γ+="[PAM]-Controller | "+Ъ.Count+" Connected ships"+"\n"+Θξ()+Θσ();else Γ+=ΕІ(Л)+"\n";Γ+=ƒ;int Α=0;int Η=Ў;Ő=0;if(Λ!=""){Μ(ref Γ,Η,Α++,Θ,Λ);}else if(ϋ==Σ.Main){if(Л==null&&Ъ.Count>=1) Л=Ъ[0];String Ѝ="";if(Μ(ref Ѝ,Η,Α++,Θ," Send command to all")) κ(Σ.Γω);
int Ƒ=0;for(int s=0;s<Ъ.Count;s++){if(Ζϋ) Ѝ+="\n";Є Δ=Ъ[s];if(Η==Α||Η==Α+1) Л=Δ;if(Α==Η) Ƒ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Α++,Θ," "+ΕІ(Δ))){Л=Δ;κ(Σ.Ʈ);Л.Ĺ("",false,true,false);}if(Α==Η) Ƒ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Α++,Θ," "+(Ζϋ?"":"  ")+Θυ(Δ)))
{Л=Δ;κ(Σ.ΖΨ);}}int ƛ=ΑΛ-2;ƛ+=Math.Max(0,(ΚΪ-1))*ΑΛ;Γ+=Γχ(ƛ,Ѝ,Ƒ,ref Κθ);}else if(ϋ==Σ.ΑЃ||ϋ==Σ.Γω){Σ Ιο=ϋ==Σ.ΑЃ?Σ.Ʈ:Σ.Main;Є ζ=null;if(ϋ==Σ.ΑЃ) ζ=Л;if(Μ(ref Γ,Η,Α++,Θ," Back")){κ(Ιο);}if(Μ(ref Γ,Η,Α++,Θ," [Stop] "+ť(ζ,"STOP"))){Ǥ(ζ,"STOP");}if(Μ(ref Γ,Η,Α++,Θ," [Continue] "+ť(ζ,"CONT"))){Ǥ(ζ,"CONT");}
if(Μ(ref Γ,Η,Α++,Θ," [Move home] "+ť(ζ,"HOMEPOS"))){Ǥ(ζ,"HOMEPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Move to job] "+ť(ζ,"JOBPOS"))){Ǥ(ζ,"JOBPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Full simulation] "+ť(ζ,"FULL"))){Ǥ(ζ,"FULL");}if(ζ!=null&&ζ.Κ==Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," [Undock] "+ť(ζ,"UNDOCK"))){Ǥ(ζ,"UNDOCK");}Γ+=Θλ(ζ);}else if(ϋ==Σ.Ʈ){String ΓБ="";if(Μ(ref ΓБ,Η,Α++,Θ," Back")){κ(Σ.Main);}Γ+=ΓБ.Substring(0,ΓБ.Length-1).PadRight(36,' ');if(Μ(ref Γ,Η,Α++,Θ," Send cmd")){κ(Σ.ΑЃ);}if(!Л.ΖΔ()&&!Л.Γή()) Ő++;
if(Л.Γή()) Γ+="No answer received...";else Γ+=ū(Л,Л.Βκ);}else if(ϋ==Σ.ΖΨ){if(Μ(ref Γ,Η,Α++,Θ," Back")){κ(Σ.Main);}Γ+=Κφ;Γ+=ū(Л,ΕЅ(Л));}if(!ΔЄ) Ў=Math.Min(Ő-1,Ў);return Γ;}String ΕІ(Є Δ){if(Δ.ΒЇ!=VERSION) return Δ.ί+": Different version!";return Δ.ί+": "+ū(Δ,Δ.ΕΝ)+" "+Δ.ΔΧ+"m";
}String Θυ(Є Δ){String s=Γτ("",Й(Δ.ȁ,Δ.ȇ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;к ύ=Δ.Ћ[i];s+=Ζα(ΓΑ(Θς(ύ)),3)+" "+Εό(ύ.Љ)+" ";}return s;}String Θς(к ύ){if(ύ.υ.ToUpper()=="ORE"||ύ.υ.ToUpper()=="INGOT"){String Ζω=Θπ(ύ.ί.ToUpper());if(Ζω!="") return Ζω;
}return ύ.ί;}String ū(Є Δ,String Γ){if(Δ.ΖΔ()) return "No signal...("+Αο((int)(DateTime.Now-Δ.Δσ).TotalSeconds)+")";return Γ;}String ΕЃ(bool ΚЋ){String Γ="";if(ΚЋ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΘЋ(IMyTerminalBlock х,out С Џ,out String ǲ){bool Ȩ=true;String ΑΙ=ΑϏ(Αμ(х.CustomData.Split('\n'),"mode",ref Ȩ)).ToUpper();String[]τ=ΑΙ.Split(':');String ΑΘ=τ.Length>0?τ[0].Trim():"";ǲ=τ.Length>1?τ[1].Trim():"";
Џ=С.ğ;if(ΑΘ=="MAIN") Џ=С.ΖΙ;else if(ΑΘ=="MAINX") Џ=С.ΖΚ;else if(ΑΘ=="MENU") Џ=С.Γγ;else if(ΑΘ=="INVENTORY") Џ=С.Γέ;else if(ΑΘ=="DEBUG") Џ=С.ΓΖ;if(Џ==С.ğ) ǲ=ΑΙ;}String Ελ(С Џ,String ǲ,bool ΙЇ){int ΑΖ=15;if(ΙЇ){Ζύ=ΒΒ;ΒΒ=0;Ζώ=ΒΓ;ΒΓ=0;}String Γ="";if(Џ==С.ΖΙ)
{int ф=0;Γ=Ţ(false,Ζύ,ref Ιω,true,ΑΖ);if(!int.TryParse(ǲ,out ф)) return Γ;ΒΒ=Math.Max(ф,ΒΒ);ф--;return ΖΞ(ф,ΑΖ,Γ);}else if(Џ==С.ΖΚ){int ф=0;Γ=Ţ(false,Ζώ,ref Ιϊ,false,ΑΖ);if(!int.TryParse(ǲ,out ф)) return Γ;ΒΓ=Math.Max(ф,ΒΓ);ф--;return ΖΞ(ф,ΑΖ,Γ);}if(Џ==С.ΖΘ){return Ţ(false,0,ref Ιϋ,true,ΑΖ);
}else if(Џ==С.Γέ||Џ==С.Γγ||Џ==С.ΓΖ){Є Δ=Л;if(ǲ!=""){Δ=ΓΟ("",ǲ);if(Δ==null) return "Unknown ship: "+ǲ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƒ="—————————————————————\n";String ΕΨ="";String ΔΕ="";if(Џ==С.Γγ){ΕΨ="Menu";ΔΕ=ū(Δ,Δ.Βκ);}
else if(Џ==С.Γέ){ΕΨ="Inventory";ΔΕ=ū(Δ,ΕЅ(Δ));}else if(Џ==С.ΓΖ){return ū(Δ,Θφ(Δ));}Γ+=Δ.ί+" - "+Δ.ΔΧ+"m | "+ΓΑ(""+ΕΨ)+"\n"+ƒ;Γ+=ΔΕ;return Γ;}return "Unknown command: "+ǲ;}string Θφ(Є Δ){String s="";s+=Δ.ΒЇ+"\n";s+=Δ.ί+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕΝ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βκ.Length+"\n";s+=Δ.ȁ+"\n";s+=Δ.ȇ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΕЅ(Є Δ){String s="";s+=Γτ("All",Й(Δ.ȁ,Δ.ȇ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){к ύ=Δ.Ћ[i];
s+=Γτ(ύ.ί,ύ.Љ,Δ.ΕΪ,30,8,10)+"\n";}return s;}Ј Θμ(Є Δ,bool э,string Ѕ){Ј ή=Ј.None;if(Δ==null){for(int i=0;i<Ъ.Count;i++){Ј Ѓ=Ъ[i].Ĺ(Ѕ,false,э,false);if(Ѓ==Ј.ƫ) ή=Ѓ;if(Ѓ==Ј.Αό) return Ѓ;if(Ѓ==Ј.ũ) return Ѓ;}return ή;}else return Δ.Ĺ(Ѕ,false,э,false);}String ť(Є Δ,String Ѕ)
{if(!ΖΩ(Δ,Ѕ)) return "";Ј Ѓ=Θμ(Δ,false,Ѕ);if(Ѓ==Ј.ƫ) return "received!";if(Ѓ==Ј.ũ) return "pending...";if(Ѓ==Ј.Αό) return "no answer!";return "";}String Γχ(int ƛ,String Γ,int Ƒ,ref int đ){String[]Ν=Γ.Split('\n');if(Ƒ>=đ+ƛ-1) đ++;đ=Math.Min(Ν.Count()-1-ƛ,đ);if(Ƒ<đ+1) đ--;
đ=Math.Max(0,đ);String ή="";for(int i=0;i<ƛ;i++){int Ηί=i+đ;if(Ηί>=Ν.Count()) break;ή+=Ν[Ηί]+"\n";}return ή;}String ΖΞ(int ф,int ΑΛ,String Γ){String[]Ν=Γ.Split('\n');int ΑΣ=ф*ΑΛ;int ą=(ф+1)*ΑΛ;String ή="";for(int i=ΑΣ;i<ą;i++){if(i>=Ν.Count()) break;ή+=Ν[i]+"\n";
}return ή;}String ΓΑ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζα(String s,int Ηζ){if(s=="") return s;if(s.Length>Ηζ) s=s.Substring(0,Ηζ-1)+".";return s;}string Γτ(String ō,float value,float Ƃ,int Ζξ,int ΗΪ,int ΚΩ){float ŕ=Й(value,Ƃ)*Ζξ;
String ΑА="[";for(int i=0;i<Ζξ;i++){if(i<=ŕ) ΑА+="|";else ΑА+="'";}ΑА+="]";return ΑА+" "+Ζα(ΓΑ(ō),ΗΪ).PadRight(ΗΪ)+"".PadRight(ΚΩ)+Εό(value);}String Εό(float Љ){if(Љ>=1000000) return Math.Round(Љ/1000000f,Љ/1000000f<100?1:0)+"M";if(Љ>=1000) return Math.Round(Љ/1000f,Љ/1000f<100?1:0)+"K";
return ""+Math.Round(Љ);}String Αο(int ΑΟ){if(ΑΟ>=60*60) return Math.Round(ΑΟ/(60f*60f),1)+" h";if(ΑΟ>=60) return Math.Round(ΑΟ/60f,1)+" min";return ""+ΑΟ+" s";}string Ť(float v){return Math.Round(v,2)+" ";}string ΛΑ(Vector3 v){return "X"+Ť(v.X)+"Y"+Ť(v.Y)+"Z"+Ť(v.Z);}
List<IMyCameraBlock>ȝ=new List<IMyCameraBlock>();List<long>Ɔ=new List<long>();bool ƈ=false;int ΔΜ=0;int ΔΙ=0;DateTime Ζπ=DateTime.Now;String ΔΔ="";void ΙΗ(){ȝ.Clear();Ɔ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ȝ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȝ.Count;i++){ȝ[i].EnableRaycast=true;if(!Ɔ.Contains(ȝ[i].CubeGrid.EntityId)) Ɔ.Add(ȝ[i].CubeGrid.EntityId);}if(!Ɔ.Contains(Me.CubeGrid.EntityId)) Ɔ.Add(Me.CubeGrid.EntityId);}bool ΖΒ(Β s){return s==Β.ĝ||s==Β.с||s==Β.И||
s==Β.Ĝ||s==Β.ţ||s==Β.Ƣ||s==Β.Αν||
(s==Β.Ǹ&&Ǚ!=null);}void Αθ(){ƈ=false;ΔΜ=0;ΔΔ="";}void Ια(){if(!obstacleBrake||ȝ.Count==0||!ΖΒ(Ϊ)){Αθ();return;}Vector3 ǆ=Ѐ+į;Vector3 ȍ=ǆ-γ;float ȋ=ȍ.Length();if(ȋ<1f){Αθ();return;}Vector3 ρ=ȍ/ȋ;float Ηγ=Ē*obstacleLookAhead+ο;Vector3 Κΰ=ǆ+ρ*(ο*0.25f);
for(int n=0;n<ȝ.Count;n++){ΔΙ=(ΔΙ+1)%ȝ.Count;IMyCameraBlock ǽ=ȝ[ΔΙ];if(!ǽ.IsFunctional||!ǽ.IsWorking) continue;Vector3 Ζς=ǽ.GetPosition();Vector3 ą=Κΰ;if(Vector3.Distance(Ζς,ą)>Ηγ) ą=Ζς+ρ*Ηγ;if(!ǽ.CanScan(ą)) continue;MyDetectedEntityInfo ȣ=ǽ.Raycast(ą);bool ΚΦ=!ȣ.IsEmpty()&&!Ɔ.Contains(ȣ.EntityId)&&!ΘЄ(ȣ.EntityId)&&
ȣ.Type!=MyDetectedEntityType.FloatingObject;if(ΚΦ){if(!ƈ) Ζπ=DateTime.Now;ƈ=true;ΔΜ=0;ΔΔ=ȣ.Type.ToString();}else if(ƈ&&++ΔΜ>=3) Αθ();break;}if(ƈ&&(DateTime.Now-Ζπ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΔ;}const String ΓЄ="[PAMTFC]";const float ΙΣ=5f;const float ΓΞ=2f;class Ğ{public long ΰ;public Vector3 ν,ƚ;public Vector3 ǌ;public float ΖЈ;public String Χ="";public int Ĉ=-1;public bool ΔБ;public long α=0;public int ƾ=0;public DateTime ΕΥ;}IMyBroadcastListener ΒЁ=null;
long Ǎ=0;List<Ğ>ђ=new List<Ğ>();Vector3 į=new Vector3();float ȑ=-1;Vector3 ΕΫ=new Vector3();bool ΒЀ=false;DateTime Εά=DateTime.Now;const float Ζΰ=15f;const float ΙΡ=10f;int ΘΓ=0;String з="";int Г=-1;bool ΔΠ=false;Vector3 ΖϏ=new Vector3();String Αφ(){if(β==Ρ.ы||Κ==Ι.έ) return "";
return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Ь+"|"+(int) Ō+"|"+Math.Round(Ȑ,1)+"|"+Math.Round(ȏ,1);}void Ιε(){if(!trafficEnabled) return;if(ΒЁ==null) ΒЁ=IGC.RegisterBroadcastListener(ΓЄ);Ǎ=Me.CubeGrid.EntityId;int ă=30;while(ΒЁ.HasPendingMessage&&ă-->0){String Ξ=ΒЁ.AcceptMessage().Data as String;if(Ξ!=null) ΘЍ(Ξ);
}for(int i=ђ.Count-1;i>=0;i--)
if((DateTime.Now-ђ[i].ΕΥ).TotalSeconds>ΙΣ) ђ.RemoveAt(i);if(ΒΕ==null) ΒΕ=IGC.RegisterBroadcastListener(ΖΕ);ă=10;while(ΒΕ.HasPendingMessage&&ă-->0){String Ξ=ΒΕ.AcceptMessage().Data as String;if(Ξ!=null) try{ΘЊ(Ξ);}catch{}}if(++ΘΓ<3) return;ΘΓ=0;IGC.SendBroadcastMessage(ΓЄ,Ǎ+";"+σ(γ)+";"+
σ(Ω.GetShipVelocities().LinearVelocity)+";"+ο+";"+Αφ()+";"+
(β==Ρ.З?Г:-1)+";"+з+";"+ΘΫ()+","+Εω()+";"+σ(ΕΫ)+
";"+(ΒΝ?1:0)+";"+(ŉ&&β==Ρ.З?1:0)+","+Βυ+","+Βτ);}void ΘЍ(String Ξ){try{String[]f=Ξ.Split(';');if(f.Length<7) return;long ΰ=long.Parse(f[0]);if(ΰ==Ǎ) return;Ğ Δ=null;for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) Δ=ђ[i];if(Δ==null){Δ=new Ğ();Δ.ΰ=ΰ;ђ.Add(Δ);}
Δ.ν=ς(f[1]);Δ.ƚ=ς(f[2]);Δ.ΖЈ=float.Parse(f[3]);Δ.Χ=f[4];Δ.Ĉ=int.Parse(f[5]);Δ.ΕΥ=DateTime.Now;if(Δ.Χ!=""&&Δ.Χ==Αφ()) ΖΛ(f[6]);Δ.α=0;Δ.ƾ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ƾ=int.Parse(d[1]);}Δ.ǌ=f.Length>=9?ς(f[8]):new Vector3();
Δ.ΔБ=f.Length>=10&&f[9]=="1";}catch{}}bool ΘЄ(long ΰ){for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) return true;return false;}Vector3 ΕЄ(int Α,int ΚΫ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();Ϋ ά=Φ[Α];Vector3 ΚЃ=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*ΚΫ;
Vector3 Б=ά.φ.LengthSquared()>0.01f?-Vector3.Normalize(ά.φ):-ά.η;Vector3 о=Vector3.Cross(ΚЃ,Б);if(о.LengthSquared()<0.0001f) return new Vector3();float Њ=laneOffset*ο;float ΙЉ=Math.Min(Vector3.Distance(ά.Ε,Φ[0].Ε),
Vector3.Distance(ά.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(о)*Њ*Math.Min(1,ΙЉ/(Њ*4));}Vector3 ΘΒ=new Vector3();bool ΘЃ(Β s){return s==Β.Ǹ||s==Β.Ĳ||s==Β.Ķ||s==Β.Ƭ||
s==Β.Ǧ||s==Β.ȳ||s==Β.Ę;}void Ιζ(){į=new Vector3();ȑ=-1;ΕΫ=new Vector3();bool Ǉ=ΖΒ(Ϊ);if(!trafficEnabled||(!Ǉ&&!ΘЃ(Ϊ))){ΒЀ=false;return;}ΘΒ=Ω.GetShipVelocities().LinearVelocity;Θέ(Ω.GetNaturalGravity(),!Ǉ);}void Θέ(Vector3 φ,bool Ηχ=false){į=new Vector3();ȑ=-1;Vector3 ȍ=Ѐ-γ;
float ȋ=ȍ.Length();bool Δκ=ȋ>=1f;Vector3 ρ=Δκ?ȍ/ȋ:new Vector3();ΕΫ=ρ;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ǀ;Vector3 о=Vector3.Cross(ρ,Б);о=о.LengthSquared()<0.0001f?-ΑΕ:Vector3.Normalize(о);Ğ Βΰ=null;bool ΗΗ=Ϊ==Β.И&&!ā(1);for(int i=0;i<ђ.Count;i++)
{Ğ λ=ђ[i];if(λ.ƾ==3&&λ.ƚ.LengthSquared()<0.25f) continue;Vector3 ν=λ.ν+λ.ƚ*(float)(DateTime.Now-λ.ΕΥ).TotalSeconds;Vector3 ƌ=ν-γ;float э=(ο+λ.ΖЈ)*0.5f;float ƴ=Δκ?Vector3.Dot(ƌ,ρ):0;if(ƴ>0&&(ƌ-ρ*ƴ).Length()<=э*1.2f){float Βξ=Vector3.Dot(λ.ƚ,ρ);bool ΗΛ=λ.ǌ.LengthSquared()>0.25f;
bool Δλ=Βξ<-1f||(ΗΛ&&Vector3.Dot(λ.ǌ,ρ)<-0.5f);if(Ηχ){if(Δλ&&Ǎ<λ.ΰ) continue;}else{if(λ.ΔБ){if(!ΗΗ){į=о*э*1.3f;continue;}float Ημ=Vector3.Distance(γ,Ѐ),ΗΧ=Vector3.Distance(ν,Ѐ);if(Ημ<ΗΧ||(Ημ==ΗΧ&&Ǎ<λ.ΰ)) continue;}if(Δλ){if(ƴ<(Ē-Βξ)*6f+э*4f) į=о*э*1.3f;
continue;}if(ΗΛ&&Ǎ<λ.ΰ){Vector3 Ζμ=-ƌ;float Ητ=Vector3.Dot(Ζμ,λ.ǌ);if(Ητ>0&&(Ζμ-λ.ǌ*Ητ).Length()<=э*1.2f){į=о*э*1.3f;continue;}}}float ΗΖ=ƴ-э-followGap*ο;float Δψ=Math.Max(0,Βξ)+Math.Max(0,ΗΖ)/ΓΞ;if(Δλ) Δψ=Math.Max(0,ΗΖ+Math.Min(0,Βξ)*ΓΞ*2)/ΓΞ;
if(ȑ<0||Δψ<ȑ){ȑ=Δψ;Βΰ=λ;}continue;}if(λ.ƚ.LengthSquared()<1f) continue;if(Δκ&&(ƌ-ρ*ƴ).Length()<=э*1.2f) continue;Vector3 ΕΕ=λ.ƚ-ΘΒ;float Ηϋ=ΕΕ.LengthSquared();if(Ηϋ<0.25f) continue;float t=-Vector3.Dot(ƌ,ΕΕ)/Ηϋ;if(t<=0||t>5f||(ƌ+ΕΕ*t).Length()>=э*1.1f) continue;
if(Ǎ>λ.ΰ||t<2f){ȑ=0;Βΰ=λ;}}bool ΙЏ=ȑ>=0&&ȑ<0.5f;if(!ΙЏ||Ηχ||(Βΰ!=null&&Βΰ.ΔБ&&ΗΗ)){ΒЀ=false;return;}if(!ΒЀ){ΒЀ=true;Εά=DateTime.Now;}double ΗΜ=(DateTime.Now-Εά).TotalSeconds;if(ΗΜ>Ζΰ+ΙΡ) Εά=DateTime.Now;else if(ΗΜ>Ζΰ){ȑ=-1;į=о*ο*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>Ȥ=new List<int>();String ΗΞ="";void ΓΝ(){String Ȧ=Ќ+"|"+Ь+"|"+(int) Ō;if(Ȧ==ΗΞ) return;ΗΞ=Ȧ;Εμ(Ќ,Ь,Ō,Ȥ);}void Εμ(int w,int h,ь ΑΣ,List<int>ΔΚ){ΔΚ.Clear();if(ΑΣ==ь.ư){for(int k=0;k<w*h;k++){int ƍ=k/w;ΔΚ.Add(ƍ*w+(ƍ%2==0?k-ƍ*w:w-1-(k-ƍ*w)));
}return;}int Ǌ=(int) Math.Ceiling(w/2f),ǉ=(int) Math.Ceiling(h/2f);int Δη=w/2,Δζ=h/2;int x=0,y=0,ΔΪ=0,ΔΫ=-1,Ƃ=Math.Max(w,h);for(int i=0;i<Ƃ*Ƃ;i++){if(-Ǌ<x&&x<=Δη&&-ǉ<y&&y<=Δζ) ΔΚ.Add((y-1+ǉ)*w+(x-1+Ǌ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔΪ;ΔΪ=-ΔΫ;ΔΫ=t;}
x+=ΔΪ;y+=ΔΫ;}}bool ΕЌ(int a,int b,int w,float ΒЂ,float Βύ,float ǈ,float Ǆ){return a==b||(Math.Abs(a%w-b%w)*ΒЂ<ǈ+0.5f&&Math.Abs(a/w-b/w)*Βύ<Ǆ+0.5f);}bool ΓΜ(){int n=Math.Max(0,Ќ*Ь);if(з.Length==n) return false;з=new String('0',n);Г=-1;return true;}
void ΖΛ(String λ){ΓΜ();if(λ.Length!=з.Length) return;char[]h=з.ToCharArray();for(int i=0;i<h.Length;i++) if(λ[i]=='1') h[i]='1';з=new String(h);}bool Ατ(int У){return У>=0&&У<з.Length&&з[У]=='1';}void Γϋ(int У,bool ώ){if(У<0||У>=з.Length) return;char[]h=з.ToCharArray();
h[У]=ώ?'1':'0';з=new String(h);}bool ΓΪ(int У,bool ΚΖ){if(!trafficEnabled) return false;String Ȧ=Αφ();for(int i=0;i<ђ.Count;i++){Ğ t=ђ[i];if(t.Χ!=Ȧ||t.Ĉ<0||(ΚΖ&&t.ΰ>Ǎ)) continue;if(ΕЌ(У,t.Ĉ,Ќ,Ȑ,ȏ,Βυ,Βτ)) return true;}return false;}int ΘΪ(int Α){int n=Ќ*Ь;if(n<1) return Α;
ΓΝ();if(ΓΜ()) for(int k=0;k<Α&&k<n;k++) Γϋ(Ȥ[k],true);if(Г>=0&&(Α>=n||Ȥ[Α]!=Г)){Γϋ(Г,true);Г=-1;}if(ΖΖ()){if(Ȁ>=0&&!Ατ(Ȁ)){ŉ=false;Г=Ȁ;return Ȥ.IndexOf(Ȁ);}if(Ȁ==-2){ŉ=false;Г=-1;return n;}if(Ȁ==-1&&Г>=0&&!Ατ(Г)){ŉ=false;return Α;}
Г=-1;if(!ŉ){ŉ=true;ΖЁ=DateTime.Now;}if((DateTime.Now-ΖЁ).TotalSeconds<5) return-1;}ŉ=false;if(Г>=0&&!Ατ(Г)&&!ΓΪ(Г,true)) return Α;Г=-1;for(int k=0;k<n;k++){int c=Ȥ[k];if(Ατ(c)||ΓΪ(c,false)) continue;Г=c;return k;}return n;}const String ΖΕ="[PAMJOB]";IMyBroadcastListener ΒΕ=null;
DateTime ΖЀ=DateTime.MinValue;DateTime ΖЁ=DateTime.Now;int Ȁ=-1;bool ŉ=false;bool ΖΖ(){return trafficEnabled&&Ȁ!=-3&&(DateTime.Now-ΖЀ).TotalSeconds<3;}void ΘЊ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Αφ()) return;ΖЀ=DateTime.Now;
ΖΛ(f[2]);Ȁ=-3;String Ηι=Ǎ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ηι)) Ȁ=int.Parse(e[i].Substring(Ηι.Length));}ǥ ΙΜ(int Α){ΓΝ();if(Α<0||Α>=Ȥ.Count) return ǥ.Αγ;Ϗ=Ȥ[Α]%Ќ;ό=Ȥ[Α]/Ќ;return ǥ.Απ;}class Ǔ{public long ΰ;public Vector3 ν,Ы,Б;
}List<Ǔ>Ф=new List<Ǔ>();bool Ĥ=false;Vector3 ΒΟ,ΒΞ,ΒΠ,ΒΡ;long ћ=0;long ȩ=0;Ϋ Ǚ=null;bool ƿ=false;Ϋ ΓΙ(Vector3 ν,Vector3 Ы,Vector3 Б){MatrixD m=MatrixD.CreateWorld(ν,Ы,Б);Ϋ w=new Ϋ(Ο);w.θ=Vector3D.TransformNormal(ΒΟ,m);w.η=Vector3D.TransformNormal(ΒΞ,m);w.Ђ=Vector3D.TransformNormal(ΒΠ,m);
w.Ε=ν+(Vector3) Vector3D.TransformNormal(ΒΡ,m);w.ι=true;return w;}int Εω(){Β s=Ϊ;if(s==Β.ъ) return д(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Β.Ķ||s==Β.Ț||s==Β.ĵ||s==Β.ΑЊ||
s==Β.Αλ||s==Β.Ǧ) return 3;if(s==Β.Ǹ||s==Β.Ĳ||s==Β.Ƭ) return 2;if(s==Β.И) return 1;return 0;}long ΘΫ(){if(Εω()==0) return 0;if(ā(1)&&Ɣ!=0) return Ɣ;return Ǚ!=null?ȩ:ћ;}void ΖΗ(){if(Κ==Ι.έ||!Ο.ι) return;IMyShipConnector О=д(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector λ=О.OtherConnector;Vector3I ΙБ=new Vector3I((int) Ο.м.X,(int) Ο.м.Y,(int) Ο.м.Z);if(О.Position==ΙБ){MatrixD Ż=MatrixD.Transpose(λ.WorldMatrix);ΒΟ=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,Ż);ΒΞ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,Ż);ΒΠ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,Ż);
ΒΡ=Vector3D.TransformNormal(О.GetPosition()-λ.GetPosition(),Ż);Ĥ=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ћ=λ.EntityId;List<IMyShipConnector>Ň=new List<IMyShipConnector>();χ.GetBlocksOfType(Ň,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ň.Count>0){Ф.Clear();for(int i=0;i<Ň.Count;i++){Ǔ p=new Ǔ();p.ΰ=Ň[i].EntityId;p.ν=Ň[i].GetPosition();p.Ы=Ň[i].WorldMatrix.Forward;p.Б=Ň[i].WorldMatrix.Up;Ф.Add(p);}}Ǚ=null;ȩ=0;if(λ.EntityId==ћ||!Ĥ) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].ΰ==λ.EntityId){ȩ=Ф[i].ΰ;Ǚ=ΓΙ(Ф[i].ν,Ф[i].Ы,Ф[i].Б);}}bool Γΰ(long ΰ,int Ηξ){if(ΰ==0) return false;for(int i=0;i<ђ.Count;i++){Ğ t=ђ[i];if(t.α==ΰ&&(t.ƾ>Ηξ||(t.ƾ==Ηξ&&t.ΰ<Ǎ))) return true;}return false;}bool ΓΔ(int ΔЎ){if(Κ==Ι.έ||!trafficEnabled){Ǚ=null;ȩ=0;return true;}
long ξ=Ǚ!=null?ȩ:ћ;if(!Γΰ(ξ,ΔЎ)) return true;if(!Γΰ(ћ,ΔЎ)){Ǚ=null;ȩ=0;return true;}if(!Ĥ) return false;Ǔ ǧ=null;float ƹ=0;for(int i=0;i<Ф.Count;i++){Ǔ p=Ф[i];if(p.ΰ==ћ||Γΰ(p.ΰ,ΔЎ)) continue;float d=Vector3.Distance(p.ν,Ο.Ε);if(ǧ==null||d<ƹ){ǧ=p;ƹ=d;}
}if(ǧ==null) return false;ȩ=ǧ.ΰ;Ǚ=ΓΙ(ǧ.ν,ǧ.Ы,ǧ.Б);return true;}String Θε(){String s=(Ĥ?"1":"0")+":"+σ(ΒΟ)+":"+σ(ΒΞ)+":"+
σ(ΒΠ)+":"+σ(ΒΡ)+":"+ћ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].ΰ+","+σ(Ф[i].ν)+","+σ(Ф[i].Ы)+","+σ(Ф[i].Б);return s;}void Θδ(String s){Ф.Clear();Ĥ=false;ћ=0;String[]f=s.Split(':');if(f.Length<6) return;Ĥ=f[0]=="1";ΒΟ=ς(f[1]);ΒΞ=ς(f[2]);ΒΠ=ς(f[3]);ΒΡ=ς(f[4]);long.TryParse(f[5],out ћ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǔ p=new Ǔ();if(!long.TryParse(e[0],out p.ΰ)) continue;p.ν=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ф.Add(p);}}const String ơ="[PAMDCK]";const float ΘΡ=120f;class ȯ{public long Δ;public int Ǘ;public long њ;public bool Ζσ;public DateTime Ȣ,ΗΫ;
}IMyBroadcastListener ΒΜ=null;List<ȯ>Х=new List<ȯ>();List<IMyShipConnector>ē=new List<IMyShipConnector>();List<IMySensorBlock>ȭ=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΔ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΒЈ=new List<MyDetectedEntityInfo>();long Ǿ=0;int ΔΝ=0;
long Ζυ=0;DateTime Ζφ=DateTime.Now;String Ņ="";int ΖЍ=0;IMyCubeGrid Ĕ=null;void ΙΙ(){Ĕ=Me.CubeGrid;χ.GetBlocksOfType(ē,q=>q.CubeGrid.IsSameConstructAs(Ĕ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ȭ,q=>q.CubeGrid.IsSameConstructAs(Ĕ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΔ,q=>q.CubeGrid.IsSameConstructAs(Ĕ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ȭ.Count;i++){ȭ[i].DetectSmallShips=true;ȭ[i].DetectLargeShips=true;}for(int i=0;i<ΑΔ.Count;i++) ΑΔ[i].EnableRaycast=true;}IMyShipConnector ΖΫ(long ΰ){if(ΰ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(ΰ) as IMyShipConnector;return c!=null&&Ĕ!=null&&c.CubeGrid.IsSameConstructAs(Ĕ)?c:null;}bool Γί(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΓ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΔ.Count;i++)
{IMyCameraBlock ǽ=ΑΔ[i];if(!ǽ.IsWorking||Vector3D.Distance(ǽ.GetPosition(),c.GetPosition())>10) continue;Vector3D ą=ǽ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ǽ.CanScan(ą)) continue;MyDetectedEntityInfo ȣ=ǽ.Raycast(ą);if(!ȣ.IsEmpty()&&ȣ.EntityId!=Ĕ.EntityId&&ȣ.EntityId!=Δ&&ȣ.Type!=MyDetectedEntityType.FloatingObject)
{Ņ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΘЅ(){for(int s=0;s<ȭ.Count;s++){if(!ȭ[s].IsWorking) continue;ΒЈ.Clear();ȭ[s].DetectedEntities(ΒЈ);for(int i=0;i<ΒЈ.Count;i++){MyDetectedEntityInfo e=ΒЈ[i];bool ΙЋ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΙЋ&&e.EntityId!=Ǿ&&e.Velocity.Length()>0.5f){Ņ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΘЎ(ȯ r){IMyShipConnector њ=ΖΫ(r.њ);if(Γί(њ)&&ΖΓ(њ,r.Δ)) return њ.EntityId;if(!r.Ζσ||њ==null) return 0;IMyShipConnector ǧ=null;double ƹ=0;for(int i=0;i<ē.Count;i++)
{IMyShipConnector c=ē[i];if(!Γί(c)) continue;double d=Vector3D.Distance(c.GetPosition(),њ.GetPosition());if(ǧ==null||d<ƹ){if(ΖΓ(c,r.Δ)){ǧ=c;ƹ=d;}}}return ǧ==null?0:ǧ.EntityId;}void Θψ(){ȯ Ɖ=null;long О=0;for(int œ=2;œ>=1&&Ɖ==null;œ--){ȯ Œ=null;for(int i=0;i<Х.Count;i++)
{ȯ r=Х[i];if(r.Ǘ!=œ) continue;if(œ==1){long c=ΘЎ(r);if(c==0) continue;if(Œ==null||r.Ȣ<Œ.Ȣ){Œ=r;О=c;}}else if(Œ==null||r.Ȣ<Œ.Ȣ) Œ=r;}Ɖ=Œ;}if(Ɖ==null){if(Х.Count>0&&Ņ=="") Ņ="No free dock";return;}Ǿ=Ɖ.Δ;ΔΝ=Ɖ.Ǘ;Ζυ=Ɖ.Ǘ==1?О:0;Ζφ=DateTime.Now;
}void ΙΫ(){if(Ĕ==null) return;if(ΒΜ==null) ΒΜ=IGC.RegisterBroadcastListener(ơ);int ă=30;while(ΒΜ.HasPendingMessage&&ă-->0){String Ξ=ΒΜ.AcceptMessage().Data as String;if(Ξ!=null) Θω(Ξ);}ΙΒ();if(++ΖЍ<3) return;ΖЍ=0;IGC.SendBroadcastMessage(ơ,ΘΠ());}void Θω(String Ξ){try{String[]f=Ξ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Х.Count-1;i>=0;i--) if(Х[i].Δ==Δ) Х.RemoveAt(i);if(Ǿ==Δ) Ǿ=0;}else if(f[0]=="R"&&f.Length>=5){long њ=long.Parse(f[3]);if(ΖΫ(њ)==null) return;ȯ r=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Δ) r=Х[i];if(r==null){r=new ȯ();r.Δ=Δ;r.Ȣ=DateTime.Now;Х.Add(r);}
int Ǘ=int.Parse(f[2]);if(r.Ǘ!=Ǘ) r.Ȣ=DateTime.Now;r.Ǘ=Ǘ;r.њ=њ;r.Ζσ=f[4]=="1";r.ΗΫ=DateTime.Now;}}catch{}}void ΙΒ(){for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].ΗΫ).TotalSeconds>3) Х.RemoveAt(i);ȯ ΒΑ=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Ǿ) ΒΑ=Х[i];if(Ǿ!=0&&ΒΑ==null) Ǿ=0;if(ΒΑ!=null&&(DateTime.Now-Ζφ).TotalSeconds>ΘΡ){Ǿ=0;ΒΑ.Ȣ=DateTime.Now;}Ņ="";if(Ǿ==0&&Х.Count>0&&!ΘЅ()) Θψ();
}String ΘΠ(){String ε="S;"+Ĕ.EntityId+";"+Ǿ+";"+ΔΝ+";"+Ζυ+";";for(int i=0;i<Х.Count;i++) ε+=(i>0?",":"")+Х[i].Δ;ε+=";";for(int i=0;i<ē.Count;i++){IMyShipConnector c=ē[i];ε+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}ε+=";";for(int i=0;i<ȭ.Count;i++){IMySensorBlock z=ȭ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();ε+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return ε;}String Θξ()
{if(ē.Count==0&&ȭ.Count==0) return "";int Ǉ=0;for(int i=0;i<ē.Count;i++) if(Γί(ē[i])) Ǉ++;return "Docks: "+Ǉ+"/"+ē.Count+" free"+(Ǿ!=0?(ΔΝ==1?" | docking":" | undocking"):"")+
(Х.Count>1?" | "+(Х.Count-1)+" waiting":"")+(Ņ!=""?"\n"+Ņ:"")+"\n";}IMyBroadcastListener Βϋ=null;int ȡ=0;DateTime ΖЌ=DateTime.Now;DateTime ΗЂ=DateTime.MinValue;long ΗЃ=0;int ΗЄ=0;long Ɣ=0;bool Αί=false;int ΔΩ=0;List<Vector3>Αα=new List<Vector3>();List<float>Εθ=new List<float>();
bool ΒΝ=false;bool ΒΛ=false;Vector3 ΖЋ=new Vector3();bool Ư(){return ȡ!=0&&(DateTime.Now-ΗЂ).TotalSeconds<3;}bool ā(int Ǘ){return Ư()&&ΗЃ==Ǎ&&ΗЄ==Ǘ;}bool Ζά(){return Ư()||(DateTime.Now-ΖЌ).TotalSeconds<3;}void Γφ(int Ǘ){if(ȡ==Ǘ) return;ȡ=Ǘ;ΖЌ=DateTime.Now;ΔΩ=3;
}void Γυ(){if(ȡ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ơ,"D;"+Ǎ);ȡ=0;Αί=false;}void ΙΪ(){if(!trafficEnabled||Κ==Ι.έ) return;if(Βϋ==null) Βϋ=IGC.RegisterBroadcastListener(ơ);int ă=20;while(Βϋ.HasPendingMessage&&ă-->0){String Ξ=Βϋ.AcceptMessage().Data as String;if(Ξ==null||!Ξ.StartsWith("S;")) continue;
try{ΘЌ(Ξ);}catch{}}if(Ϊ!=Β.И){ΒΝ=false;ΒΛ=false;}if(ȡ==1&&д(MyShipConnectorStatus.Connected)!=null) Γυ();if(ȡ==2&&!Αί&&Ϊ!=Β.Ǧ&&
Vector3.Distance(γ,щ().Ε)>ο*3) Γυ();if(ȡ!=0&&++ΔΩ>=3){ΔΩ=0;IGC.SendBroadcastMessage(ơ,"R;"+Ǎ+";"+ȡ+";"+ћ+";"+(Ĥ?1:0));}}void ΘЌ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<7) return;bool ΗΕ=false;String[]Ζγ=f[5].Split(',');for(int i=0;i<Ζγ.Length;i++) if(Ζγ[i]==Ǎ.ToString()) ΗΕ=true;
if(!ΗΕ) return;ΗЂ=DateTime.Now;ΗЃ=long.Parse(f[2]);ΗЄ=int.Parse(f[3]);Ɣ=long.Parse(f[4]);Αα.Clear();Εθ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αα.Add(ς(e[0]));Εθ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΖЏ=f[6].Split(':');List<Ǔ>Ѝ=new List<Ǔ>();for(int i=0;i<ΖЏ.Length;i++){String[]e=ΖЏ[i].Split(',');if(e.Length<4) continue;Ǔ p=new Ǔ();p.ΰ=long.Parse(e[0]);p.ν=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ѝ.Add(p);}Ф=Ѝ;}bool ΘΨ(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;
Γφ(1);if(!Ζά()) return true;if(!ā(1)){Λ="Waiting for dock clearance";return false;}ΒЌ();return true;}bool ΒЌ(){ƿ=false;Ǔ Ŕ=null;if(Ɣ!=0&&Ɣ!=ћ&&Ĥ)
for(int i=0;i<Ф.Count;i++) if(Ф[i].ΰ==Ɣ) Ŕ=Ф[i];long Ɯ=Ŕ==null?0:Ŕ.ΰ;if(Ɯ==ȩ&&(Ɯ==0)==(Ǚ==null)) return false;ȩ=Ɯ;Ǚ=Ŕ==null?null:ΓΙ(Ŕ.ν,Ŕ.Ы,Ŕ.Б);return true;}bool ŧ(Vector3 p){for(int i=0;i<Αα.Count;i++) if(Vector3.Distance(p,Αα[i])<Εθ[i]+ο*0.75f) return true;
return false;}bool Θό(Vector3 Ƽ,Vector3 ő){ΒΝ=false;if(!trafficEnabled||Κ==Ι.έ||ћ==0) return false;if(ȡ!=1&&Vector3.Distance(γ,Ο.Ε)>dockRequestDistance) return false;Γφ(1);if(!Ư()||ā(1)||Αα.Count==0) return false;if(!ŧ(Ƽ)&&!ŧ(ő)&&!ŧ(γ)) return false;ΒΝ=true;return true;
}bool Εσ(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;Γφ(2);if(!Ζά()) return true;if(!ā(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΘІ=600f;class ȱ{public long ΰ;public int У=-1;public bool Ɯ;public float ǈ,Ǆ;public DateTime ΕΖ;}class ȗ{public String Ȧ;
public int w,h;public float ΒЂ,Βύ;public String ώ;public List<int>ΔЉ=new List<int>();public List<ȱ>п=new List<ȱ>();}List<ȗ>ȅ=new List<ȗ>();IMyBroadcastListener Βί=null;int ΗΨ=0;void Ιί(){if(Βί==null) Βί=IGC.RegisterBroadcastListener(ΓЄ);int ă=50;while(Βί.HasPendingMessage&&ă-->0)
{String Ξ=Βί.AcceptMessage().Data as String;if(Ξ!=null) try{Θϋ(Ξ);}catch{}}for(int j=ȅ.Count-1;j>=0;j--){ȗ Χ=ȅ[j];for(int i=Χ.п.Count-1;i>=0;i--)
if((DateTime.Now-Χ.п[i].ΕΖ).TotalSeconds>ΘІ) Χ.п.RemoveAt(i);if(Χ.п.Count==0){ȅ.RemoveAt(j);continue;}ΘΜ(Χ);}if(++ΗΨ<3) return;ΗΨ=0;for(int j=0;j<ȅ.Count;j++) IGC.SendBroadcastMessage(ΖΕ,ΘΟ(ȅ[j]));}void ΘΜ(ȗ Χ){for(int i=0;i<Χ.п.Count;i++){ȱ s=Χ.п[i];if(s.Ɯ&&(s.У<0||Χ.ώ[s.У]=='1')) s.У=ΘЈ(Χ,s);
}}String ΘΟ(ȗ Χ){String ε="J;"+Χ.Ȧ+";"+Χ.ώ+";";bool Ȣ=true;for(int i=0;i<Χ.п.Count;i++){if((DateTime.Now-Χ.п[i].ΕΖ).TotalSeconds>5) continue;ε+=(Ȣ?"":",")+Χ.п[i].ΰ+":"+Χ.п[i].У;Ȣ=false;}return ε;}void Θϋ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȗ Χ=null;for(int i=0;i<ȅ.Count;i++) if(ȅ[i].Ȧ==f[4]) Χ=ȅ[i];if(Χ==null){Χ=new ȗ();Χ.Ȧ=f[4];Χ.w=int.Parse(k[3]);Χ.h=int.Parse(k[4]);Χ.ΒЂ=float.Parse(k[6]);Χ.Βύ=float.Parse(k[7]);if(Χ.w<1||Χ.h<1) return;Εμ(Χ.w,Χ.h,(ь) int.Parse(k[5]),Χ.ΔЉ);
Χ.ώ=new String('0',Χ.w*Χ.h);ȅ.Add(Χ);}String d=f[6];if(d.Length==Χ.ώ.Length){char[]c=Χ.ώ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Χ.ώ=new String(c);}long ΰ=long.Parse(f[0]);ȱ s=null;for(int i=0;i<Χ.п.Count;i++) if(Χ.п[i].ΰ==ΰ) s=Χ.п[i];
if(s==null){s=new ȱ();s.ΰ=ΰ;Χ.п.Add(s);}s.ΕΖ=DateTime.Now;String[]w=f[10].Split(',');s.Ɯ=w[0]=="1";if(w.Length>=3){s.ǈ=float.Parse(w[1]);s.Ǆ=float.Parse(w[2]);}int Ĉ=int.Parse(f[5]);if(Ĉ>=0&&Ĉ<Χ.ώ.Length&&(s.У<0||Χ.ώ[s.У]=='1')&&Χ.ώ[Ĉ]!='1'&&ΕЋ(Χ,Ĉ,s)==null)
s.У=Ĉ;}ȱ ΕЋ(ȗ Χ,int У,ȱ Δή){for(int i=0;i<Χ.п.Count;i++){ȱ o=Χ.п[i];if(o!=Δή&&o.У>=0&&ΕЌ(У,o.У,Χ.w,Χ.ΒЂ,Χ.Βύ,Math.Max(o.ǈ,Δή.ǈ),Math.Max(o.Ǆ,Δή.Ǆ)))
return o;}return null;}int ΘЈ(ȗ Χ,ȱ s){for(int k=0;k<Χ.ΔЉ.Count;k++){int c=Χ.ΔЉ[k];if(Χ.ώ[c]!='1'&&ΕЋ(Χ,c,s)==null) return c;}return-2;}String Θσ(){String s="";for(int j=0;j<ȅ.Count;j++){ȗ Χ=ȅ[j];int ώ=0;for(int i=0;i<Χ.ώ.Length;i++) if(Χ.ώ[i]=='1') ώ++;s+="Job "+(j+1)+": "+ώ+"/"+Χ.ώ.Length+" holes, "+Χ.п.Count+" ships\n";
}return s;}