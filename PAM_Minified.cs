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
public enum Ι{Ğ,ƪ,ǣ,і,έ}public enum Β{с,ě,ƫ,Ȳ,Ĺ,Л,р,
Ě,ţ,Ǹ,ĵ,ı,Ė,
ƭ,ș,Ǧ,Ĵ,ΑЋ,Αμ,
ΓЇ,ΑЊ,ƣ,Αξ,ğ}public enum Ρ{ы,Ũ,З,ΓΚ,Αθ,Αϊ,Αϋ,Ş,ş}public enum ѕ{None,Ĵ,с,Εχ}public enum Σ{Main,ΖΡ,Ʃ,ƨ,ΓЂ,Αζ,
ΒЊ,ΕЌ,ΕЍ,ΕЉ,Ư,ΖΧ,
ΑЄ,Γω,Γύ,ΖΩ,ΑІ}public enum у{ΑЃ,ơ,ΙΚ,ΕА,έ}public enum ь{Ʊ,Επ}public enum щ{ΑЂ,ΓΠ,Ζά,ƥ}public enum В{Γι,Ετ,ΓΕ,ΓΛ,Ακ,ΓΫ,Ατ}public enum ǐ{ΓΗ,ΒЏ,ΒА}public enum Ш{Ŧ,Θϋ,Ĳ}public enum π{Γλ,Γο,Γπ,ΖΜ,ΖΛ,
Γκ,ΑϏ,Αώ,Γν,
Γξ,Γμ}public enum γ{Ȓ,ȱ,ȑ,Αε,ΒЍ,ΒЎ}public enum ǥ{Αρ,Αδ,Θδ}public enum ї{ΑЇ,Εω,ΘЅ,Γθ}public enum Ј{None,ũ,Ƭ,Αύ}public enum ϊ{Ƴ,Αγ,Αψ,ƥ,Γζ,Αω,Γη,ΙΟ,Γρ}public enum С{Ğ,ΖΘ,ΖΗ,Γγ,Γέ,ΓΖ,ΖΙ}class Ϋ{public bool ι=false;public Vector3 Ε=new Vector3();
public Vector3 η=new Vector3();public Vector3 θ=new Vector3();public Vector3 Ё=new Vector3();public Vector3 φ=new Vector3();public Vector3 л=new Vector3();public float Ч=0;public float ΛΑ=0;public float[]Ƕ=null;public Ϋ(){}public Ϋ(Ϋ λ){ι=λ.ι;Ε=λ.Ε;η=λ.η;θ=λ.θ;Ё=λ.Ё;φ=λ.φ;
л=λ.л;Ƕ=λ.Ƕ;}public Ϋ(Vector3 Ε,Vector3 θ,Vector3 η,Vector3 Ё,Vector3 φ){this.Ε=Ε;this.η=η;this.θ=θ;this.Ё=Ё;this.Ч=0;this.φ=φ;}public void ΘΟ(List<IMyThrust>ΑЎ,List<string>ȩ){Ƕ=new float[ȩ.Count];for(int i=0;i<Ƕ.Length;i++)
Ƕ[i]=-1;for(int i=0;i<ΑЎ.Count;i++){string Ǐ=ΕЇ(ΑЎ[i]);int Α=ȩ.IndexOf(Ǐ);if(Α!=-1)
Ƕ[Α]=И(ΑЎ[i].MaxEffectiveThrust,ΑЎ[i].MaxThrust);}}}class Γς{public Ϋ ζ=null;public List<Vector3>Ɨ=new List<Vector3>();public float ƾ=0;public float Εή=0;public float Ű=0;public float Δί=0;public Vector3 ΒΣ=new Vector3();}class Ŭ{public π Р=π.Γλ;public float Ǫ=0;public float Ħ=0;
public string ǫ="";public string Ǯ="";DateTime ΗΩ;public bool ΔЏ=false;private bool ΔΡ=false;public bool ΓΒ(bool ǳ){if(ǳ){Ħ=0;ΔΡ=false;return false;}ΔΡ=true;return Ħ>Ǫ;}public void ΘЌ(){ΓΒ(true);ΔЏ=false;}public void Ιΰ(){if(ΔΡ)
if((DateTime.Now-ΗΩ).TotalSeconds>1){Ħ++;ΗΩ=DateTime.Now;}}public bool ΓΨ(){switch(Р){case π.Γο:return true;case π.Γπ:return true;}return false;}}class й{public String ί="";public String υ="";public int Љ=0;public Ш Ɓ=Ш.Ĳ;public й(String ί,String υ,int Љ,Ш Ɓ){this.ί=ί;this.υ=υ;
this.Љ=Љ;this.Ɓ=Ɓ;}}class Θί{public Θί(Vector3 ξ,float Њ){this.ξ=ξ;this.Њ=Њ;}public Vector3 ξ;public float Њ;}class Є{public DateTime Δσ;public DateTime Ζω;public String ΰ="";public String ί="";public String ΒЇ="";public String Λ="";public String ΕΜ="";public String Βκ="";public String ΔЍ="";
public Vector3 Ε=new Vector3();public List<й>Ћ=new List<й>();public Ι Κ=Ι.Ğ;public Ј ġ;public float Ȇ;public float Ȁ;public int Ў;public int ΔΧ=0;public int ΕΩ=0;public bool ΖΓ(){return(DateTime.Now-Δσ).TotalSeconds>10;}public bool Γή(){if(ġ!=Ј.ũ) return false;return(DateTime.Now-Ζω).TotalSeconds>=4;
}public Є(String ΰ){this.ΰ=ΰ;}public Ј ĸ(String Ѕ,bool Κη,bool э,bool Ιο){if(Ѕ==""&&!э) return Ј.None;if(ġ==Ј.ũ&&Γή())
ġ=Ј.Αύ;if(Κη){ΔЍ=Ѕ;ġ=Ј.ũ;Ζω=DateTime.Now;return ġ;}else if(э){ΔЍ="";ġ=Ј.None;}else if(ΔЍ==Ѕ){if(Ιο) ġ=Ј.Ƭ;return ġ;}return Ј.None;}}public String Θμ(String ΚΑ){switch(ΚΑ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Βρ=new Vector3();Vector3 ε=new Vector3();Vector3 ΒΦ=new Vector3();Vector3 ΑΖ=new Vector3();Vector3 ǁ=new Vector3();
DateTime Δφ=new DateTime();bool Ą=true;int ΕΣ=0;bool ĉ=false;bool ſ=false;bool ΑΛ=true;bool ΘΖ=false;bool ž=false;bool ΒΫ=false;float Đ=0;float Ř=0;int ĭ=0;int Αά=0;int ΑΟ=0;int ΔΗ=0;float Ƅ=0;float ű=0;double ΔЂ=0;float Ƹ=0;List<int>Ż=new List<int>();List<int>Ə=new List<int>();
Ι Κ=Ι.Ğ;int А=0;float ȏ=0;float Ȏ=0;float ο=0;float Βυ=0,Βτ=0;IMyRemoteControl Ω;IMySensorBlock ψ;List<IMyTimerBlock>ΕΦ=new List<IMyTimerBlock>();List<IMyShipConnector>ј=new List<IMyShipConnector>();List<IMyThrust>Ȫ=new List<IMyThrust>();List<IMyGyro>ΒΧ=new List<IMyGyro>();
List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȧ=new List<IMyLandingGear>();List<IMyReactor>Ĭ=new List<IMyReactor>();List<IMyConveyorSorter>Βψ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǖ=new List<IMyGasTank>();List<IMyTerminalBlock>ƴ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ŵ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŀ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ġ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ц=new List<IMyTextPanel>();List<IMyTextSurface>ȋ=new List<IMyTextSurface>();List<IMyTextPanel>Ń=new List<IMyTextPanel>();
IMyTerminalBlock ǚ=null;Ϋ Ο=new Ϋ();Ϋ Δν=new Ϋ();Ϋ Δπ=new Ϋ();bool ȫ=false;bool ƙ=false;bool ƚ=false;bool Βέ=false;List<String>ȩ=new List<string>();List<Ϋ>Φ=new List<Ϋ>();List<Ϋ>Αα=new List<Ϋ>();int ΔΨ=0;double ΘΗ=0;Ϋ Ζ=new Ϋ();Vector3 Ǳ;Vector3 č;Ρ β=Ρ.ы;ь Ō=ь.Ʊ;int Ќ=0;
int Ь=0;double ΑΔ=0;bool ΒΙ=false;Ŭ І=new Ŭ();Ŭ Ї=new Ŭ();щ ё=щ.ΑЂ;bool į=true;ь ȉ=ь.Ʊ;int я=3;int ю=3;int к=30;ǐ љ=ǐ.ΓΗ;bool ĕ=true;bool Ž=true;bool ŷ=true;bool ř=false;В в=В.Γι;bool Ĕ=true;float ȅ=90;float ѝ=20;float н=5;float ў=20;float ǻ=0.70f;float Ɵ=1.50f;
float ƞ=2.50f;float ŝ=0f;float ň=0f;Β Ϊ;Ρ ќ;Β ΑΥ;Γς μ=null;int Ϗ=0;int ό=0;int а=0;int ċ=0;int Ŗ=30;int з=0;bool Д=true;Vector3 џ;double б=0;bool Βσ=false;int Εα=0;int Е=0;int ΓЎ=0;int ȇ=0;int Ηο=0;Vector3 ΑΝ=new Vector3();float ѐ=0;float п=0;int Ŝ=0;Ϋ Βω=null;
float Δτ=0;float ΑА=0;float ж=0;float ŏ=0;bool Δξ=false;bool Ś=false;bool Αέ=false;bool ć=false;bool Ā=false;DateTime ΙЀ=new DateTime();Ϋ Ł=null;Ϋ ΖЁ=null;float ț=0;ѕ Ǽ;float Ĉ=0;float ś=0;String Δύ="";float Βα=0;float Ȁ=0;float Ȇ=0;List<й>ǭ=new List<й>();float г=0;
bool ΑΡ=false;bool ΗЌ=false;bool Ηϋ=false;bool Βώ=false;bool Ηό=false;float ΔЀ=0;float ΔЌ=0;Vector3 ΗΓ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 ΒϏ=new Vector3(1,1,1);float ΕΡ=1;Vector3 ΑΩ=new Vector3();bool Βά=false;bool Δε=false;bool Δϊ=false;bool Ζε=false;float ΒΗ=0;
float ΑЍ=2;Vector3 ΗЋ;Vector3 ΕΠ;Vector3 ΕΟ;float[,]Ƿ=new float[3,2];Dictionary<String,float[,]>Ǟ=new Dictionary<string,float[,]>();Dictionary<String,float>ΘΔ=new Dictionary<String,float>();int ΔΑ=0;Dictionary<String,float[]>Ď=new Dictionary<string,float[]>();float ΔА;bool ΒΚ=false;
int ΑЏ=0;int ĩ=0;int ΒΩ=0;int Ŋ=0;String Λ="";int[]Ηη=new int[Enum.GetValues(typeof(Σ)).Length];bool ΔЄ=false;int Ő=0;int Ў=0;Σ ϋ=Σ.Main;int Κυ=0,Κφ=0,Κχ=0,Κψ=0;int Κβ=0,Κγ=0;bool ų=false;bool Ζζ=false;String Ɣ="";String ΔΖ="";IMyBroadcastListener Ǩ=null;
bool ΗΒ=true;List<Є>Ъ=null;Є К=null;int ΒΓ=0,ΒΔ=0;int Ζό=0,Ζύ=0;int Ιυ=0,Ιφ=0,Ιχ=0;String ΚΓ="";Exception Ċ=null;bool Βς=false;Ρ Ƒ=Ρ.ы;bool ΖЄ=false;bool ΗΦ=true;bool ΕΔ=false;String ΘΕ="";bool ΑΫ=false;bool ΕΛ=true;int ΒΪ=0;int[]ǜ=null;Vector3 ΔΘ=new Vector3();
bool ΗΗ=false;bool Ǒ(IMyTerminalBlock Ψ)=>Ψ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓΩ(Me,controllerTag,true))
Κ=Ι.і;κ(Σ.Main);if(Κ!=Ι.і){Λ="Welcome to [PAM]!";ї ΕΓ=ΘЍ();if(Κ==Ι.Ğ){List<IMyShipDrill>ΖА=new List<IMyShipDrill>();List<IMyShipGrinder>ΗΘ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΖА,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗΘ,q=>q.CubeGrid==Me.CubeGrid);if(ΖА.Count>0)
{Κ=Ι.ƪ;Λ="Miner mode enabled!";}else if(ΗΘ.Count>0){Κ=Ι.ǣ;Λ="Grinder mode enabled!";}else{Κ=Ι.έ;ȗ(у.έ);}}if(ΕΓ==ї.ΑЇ) ΑΛ=false;if(ΕΓ==ї.Εω) Λ="Data restore failed!";if(ΕΓ==ї.Γθ) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΚЀ){try{if(Ċ!=null){if(Т.Trim().ToUpper()=="RETRY")
{Ċ=null;Ą=true;ĭ=0;if(Κ!=Ι.і) Ѐ();Λ="Restarted after error";}else{Θφ();ΖΟ();return;}}ſ=(ΚЀ&UpdateType.Update10)!=0;if(ſ) ΕΣ++;ĉ=ΕΣ>=10;if(ĉ) ΕΣ=0;if(ſ){ΑЏ++;if(ΑЏ>4) ΑЏ=0;ΑΟ=(int) Math.Max(0,10-(DateTime.Now-Δφ).TotalSeconds);}if(Т!="") ΚΓ=Т;if(Κ!=Ι.і)
ΙΙ(Т);else
Θέ(Т);ΔЄ=false;try{int ΖЂ=Runtime.CurrentInstructionCount;float ŕ=И(ΖЂ,Runtime.MaxInstructionCount);if(ŕ>0.90) Λ="Max. instructions >90%";if(ŕ>Ƅ) Ƅ=ŕ;if(ΒΚ){Ż.Add(ΖЂ);while(Ż.Count>10) Ż.RemoveAt(0);ű=0;for(int i=0;i<Ż.Count;i++) ű+=Ż[i];ű=И(И(ű,Ż.Count),Runtime.MaxInstructionCount);
double Δυ=Runtime.LastRunTimeMs;if(ΒΫ&&Δυ>ΔЂ) ΔЂ=Δυ;Ə.Add((int)(Δυ*1000f));while(Ə.Count>10) Ə.RemoveAt(0);Ƹ=0;for(int i=0;i<Ə.Count;i++) Ƹ+=Ə[i];Ƹ=И(Ƹ,Ə.Count)/1000f;}}catch{Ƅ=0;}}catch(Exception e){Ċ=e;}}void ΙΙ(string Т){bool Βφ=false;String Ζπ="";if(А<=1&&!Γα(Т))
ΖΠ(Т);int ΔЅ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔЅ-->0){MyIGCMessage δ=Ǩ.AcceptMessage();String Ξ=δ.Data as String;if(Ξ==null) continue;String Ȉ="";if(А<=1&&ΖΞ(ref Ξ,out Ȉ,out Ζπ)&&Ȉ==BROADCAST_SELF_ID){ΖΠ(Ξ);Βφ=true;break;}}bool ΗΠ=ΒΫ&&Ϊ==Β.с&&!ž&&!Βφ&&ĭ==0&&!Βέ;
if(ĉ&&Ϊ!=Β.с) Βφ=true;if((ſ&&!ΗΠ)||(ĉ&&ΗΠ)){if(ĭ==0&&(ΑΟ<=0||Ą)){ΘΖ=А>0;А=0;ĭ=1;Ș();ΘБ();ΕЎ();ȓ("Scan 1");}else if(ĭ==1){ĭ=2;Ș();ΙΑ();ȓ("Scan 2");}else if(ĭ==2){ĭ=0;Ș();ΙΒ();ΙΓ();if(А<=1) ΖΖ();ȓ("Scan 3");Δφ=DateTime.Now;if(А<=1&&ΑΛ)
Βρ=Ας(Ω,Ω.CenterOfMass);ΑΛ=false;if(Ą){Ą=false;bool ΚЄ=β==Ρ.З;Ѐ();if(ΗΦ&&resumeAfterRestart&&ΚЄ&&А<=1) ΕΔ=true;}ΗΦ=false;if(ΘΖ&&А==0) Λ="Setup complete";}else{if(β==Ρ.З&&Κ!=Ι.έ){Ș();ΘΙ();ȓ("Inv balance");}Ș();switch(Αά){case 0:ΙΤ();break;case 1:ΙΪ();break;case 2:ΙΣ();break;
case 3:Ιγ();break;case 4:ΙΩ();break;case 5:ΘΣ();break;case 6:ΘΡ(Ω);break;}ȓ("Update: "+Αά);Αά++;if(Αά>6){Αά=0;ΒΫ=true;if(Ƒ!=Ρ.ы){switch(Ƒ){case Ρ.Ş:Ę();break;case Ρ.ş:Ę();break;case Ρ.З:Ę();break;case Ρ.Αϊ:Αο();break;case Ρ.Αϋ:Ķ();break;}Ƒ=Ρ.ы;}}}if(!Ą){if(!ȕ(Ω,true))
{Ω=null;ΑΛ=true;А=2;}if(А>=2&&Ϊ!=Β.с)
Ѐ();if(А<=1){Ř=Ω.CalculateShipMass().PhysicalMass;Đ=(float) Ω.GetShipSpeed();ε=ΙΡ(Ω,Βρ);ΒΦ=Ω.WorldMatrix.Forward;ΑΖ=Ω.WorldMatrix.Left;ǁ=Ω.WorldMatrix.Down;Ιή();Ια();ΙΦ();if(ΕΔ){ΕΔ=false;Ę();if(Ϊ!=Β.с) Λ="Job resumed after restart";}if(Ϊ!=Β.с){ž=false;ΖΤ(false);Εξ(false);String Κμ=ΓΤ(Ϊ)+" "+(int) Ϊ;
Ș();Ιά();ΓІ(false);ȓ(Κμ);Ιβ();Ιέ();Ș();ΙΨ();ȓ("Thruster");Ș();Ζα();ȓ("Gyroscope");}else{if(ž){if(Αφ()){и(ǁ,ΒΦ,ΑΖ,0.25f,true);Ζα();Λ="Aligning to planet: "+Math.Round(ΒΗ-0.25f,2)+"°";if(Βά) ΓΧ(true,true);}else ΓΧ(true,true);}}ΑΫ=false;}}}Ș();ΖΟ();ȓ("Print");if(Βφ||ΔΗ<=0){Ș();ΘΚ(Ζπ);
ȓ("Broadcast");ΔΗ=4;}else if(ĉ) ΔΗ--;}void ΖΠ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ț=τ.ElementAtOrDefault(1);var Ʒ=τ.ElementAtOrDefault(2);var Ζθ=τ.ElementAtOrDefault(3);String Ȥ="Invalid argument: "+Т;bool Εέ=false;
switch(Ѕ){case "UP":this.Γε(false);break;case "DOWN":this.Γδ(false);break;case "UPLOOP":this.Γε(true);break;case "DOWNLOOP":this.Γδ(true);break;case "APPLY":this.Εμ(true);break;case "MRES":Ў=0;break;case "STOP":this.Ѐ();break;case "PATHHOME":{this.Ѐ();this.ΓЁ();}break;case "PATH":{this.Ѐ();this.ΓЁ();Ο.ι=true;}break;
case "START":{this.Ѐ();ΓЀ();}break;case "ALIGN":{ΓΧ(!ž,false);}break;case "CONT":{this.Ѐ();this.Ę();}break;case "JOBPOS":{this.Ѐ();this.Ķ();}break;case "HOMEPOS":{this.Ѐ();this.Αο();}break;case "FULL":{ΑΡ=true;}break;case "RESET":{Βς=true;А=2;}break;case "EXPORT":{Θε();}break;
case "IMPORT":{Θω(Ț=="JOB");}break;default:Εέ=true;break;}if(Κ!=Ι.έ){switch(Ѕ){case "SHUTTLE":{Θβ();}break;case "CFGS":{if(!ΘΫ(Ț,Ʒ,Ζθ)) Λ=Ȥ;}break;case "CFGB":{if(!ΘΪ(Ț,Ʒ)) Λ=Ȥ;}break;case "CFGL":{if(!ķ(ref ȅ,true,ϊ.Αψ,Ț,"")||!Θά(Ʒ))
Λ=Ȥ;}break;case "CFGE":{if(!ķ(ref н,true,ϊ.Αω,Ț,"IG")||
!ķ(ref ѝ,true,ϊ.Γζ,Ʒ,"IG")||
!ķ(ref ў,true,ϊ.Γη,Ζθ,"IG"))
Λ=Ȥ;}break;case "CFGA":{if(!ķ(ref ǻ,false,ϊ.Αγ,Ț,"")) Λ=Ȥ;}break;case "CFGW":{if(!ķ(ref Ɵ,false,ϊ.Ƴ,Ț,"")||
!ķ(ref ƞ,false,ϊ.Ƴ,Ʒ,""))
Λ=Ȥ;}break;case "NEXT":{ΒЉ(false);}break;case "PREV":{ΒЉ(true);}break;default:if(Εέ) Λ=Ȥ;break;}}else{switch(Ѕ){case "UNDOCK":{ΑΫ=true;}break;default:if(Εέ) Λ=Ȥ;break;}}}String Θν(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Θβ(){Ѐ();Κ=Ι.έ;ȗ(у.έ);Ζ.ι=false;Ο.ι=false;ψ=null;ω.Clear();β=Ρ.ы;}void ΓΧ(bool М,bool ώ){if(!М) Λ="Aligning canceled";if(ώ) Λ="Aligning done";if(ώ||!М){ž=false;Ζέ();ΑЅ(false,0,0,0,0);return;}if(Αφ()) ž=true;}bool ΘΫ(String ΚІ,String ΙЊ,String Ιϋ)
{bool ΚЂ=β==Ρ.З;int w,h,d;if(int.TryParse(ΚІ,out w)&&int.TryParse(ΙЊ,out h)&&int.TryParse(Ιϋ,out d)){this.Ѐ();я=w;ю=h;к=d;Ľ(false);Ʋ(false,false);if(ΚЂ) Ę();return true;}return false;}bool Θά(String ƶ){if(ƶ=="ON"){ĕ=true;return true;}if(ƶ=="OFF"){ĕ=false;return true;}
return false;}bool ΘΪ(String ΖЏ,String ΒΘ){bool ȧ=true;if(ΖЏ=="HOME") į=true;else if(ΖЏ=="STOP") į=false;else ȧ=false;if(ΒΘ=="HOME") ё=щ.ΑЂ;else if(ΒΘ=="STOP") ё=щ.Ζά;else if(ΒΘ=="JOB") ё=щ.ΓΠ;else if(ΒΘ=="IG") ё=щ.ƥ;else ȧ=false;return ȧ;}void κ(Σ ф){Ηη[(int) ϋ]=Ў;
Ў=Ηη[(int) ф];if(ф==Σ.Ư) Ў=0;ϋ=ф;if(Κ!=Ι.і)
ļ(ϋ==Σ.Ʃ,false,0,0);ΔЄ=true;}void Γε(bool Δϋ){if(Ў>0) Ў--;else if(Δϋ) Ў=Ő-1;}void Γδ(bool Δϋ){if(Ў<Ő-1) Ў++;else if(Δϋ) Ў=0;}String ȗ(у δ){switch(δ){case у.ΑЃ:Λ="Job is running";break;case у.ơ:Λ="Connector not ready!";break;case у.ΙΚ:Λ="Ship modified, path outdated!";break;
case у.ΕА:Λ="Interrupted by player!";break;case у.έ:Λ="Shuttle mode enabled!";break;}return "";}String Θσ(ь ξ){switch(ξ){case ь.Ʊ:return "Top-Left";case ь.Επ:return "Center";default:return "";}}String Θι(ǐ Џ){switch(Џ){case ǐ.ΒЏ:return "Auto"+(Κ==Ι.ƪ?" (Ore)":"");case ǐ.ΒА:return "Auto (+Stone)";
case ǐ.ΓΗ:return "Default";default:return "";}}String ΓΡ(Ρ Ђ){switch(Ђ){case Ρ.ы:return "No job";case Ρ.Ũ:return "Job paused";case Ρ.З:return "Job active";case Ρ.Ş:return "Job active";case Ρ.ş:return "Job active";case Ρ.ΓΚ:return "Job done";case Ρ.Αθ:return "Job changed";case Ρ.Αϊ:return "Move home";
case Ρ.Αϋ:return "Move to job";}return "";}String Εώ(щ Ιξ){switch(Ιξ){case щ.ΑЂ:return "Return home";case щ.ΓΠ:return "Fly to job pos";case щ.Ζά:return "Stop";case щ.ƥ:return "Ignore";}return "";}String Θλ(В Џ){switch(Џ){case В.Γι:return "Off";case В.ΓΛ:return "Drop pos (Stone) ";case В.Ακ:return "Drop pos (Sto.+Ice)";
case В.Ετ:return "Cur. pos (Stone)";case В.ΓΕ:return "Cur. pos (Sto.+Ice)";case В.ΓΫ:return "In motion (Stone)";case В.Ατ:return "In motion (Sto.+Ice)";}return "";}String Εό(ѕ Ђ){switch(Ђ){case ѕ.None:return "No batteries";case ѕ.Ĵ:return "Charging";case ѕ.Εχ:return "Discharging";}return "";}
String ΓΤ(Β Ђ){String ΚА=Κ==Ι.έ?"target":"job";switch(Ђ){case Β.с:return "Idle";case Β.ě:return "Flying to XY position";case Β.ƫ:return Κ==Ι.ǣ?"Grinding":"Mining";case Β.Ȳ:return "Returning";case Β.ƣ:return "Flying to drop pos";case Β.Ĺ:return "Returning to dock";case Β.Л:return "Flying to dock area";
case Β.р:return "Flying to job area";case Β.Ě:return "Flying to path";case Β.ţ:return "Flying to job position";case Β.Ǹ:return "Approaching dock";case Β.ĵ:return "Docking";case Β.ı:return "Aligning";case Β.Ė:return "Aligning";case Β.ƭ:return "Retry docking";case Β.ș:return "Unloading";case Β.ğ:return ΘΕ;
case Β.Ǧ:return "Undocking";case Β.Ĵ:return "Charging batteries";case Β.ΑЋ:return "Waiting for uranium";case Β.Αμ:return "Filling up hydrogen";case Β.ΓЇ:return "Waiting for ejection";case Β.ΑЊ:return "Waiting for ejection";case Β.Αξ:return "Flying to drop pos";}return "";}String ΕЈ(π Р){switch(Р)
{case π.Γλ:return "On \"Undock\" command";case π.Γο:return "On player entered cockpit";case π.ΖΜ:return "On ship is full";case π.ΖΛ:return "On ship is empty";case π.Γπ:return "On time delay";case π.ΑϏ:return "On batteries empty(<25%)";case π.Αώ:return "On batteries empty(=0%)";case π.Γκ:return "On batteries full";
case π.Γξ:return "On hydrogen empty(<25%)";case π.Γμ:return "On hydrogen empty(=0%)";case π.Γν:return "On hydrogen full";}return "";}bool Μ(ref String Γ,int Ηψ,int Ĩ,bool Θ,String ō){Ő+=1;if(Ηψ==Ĩ) ō=">"+ō+(ΑЏ>=2?" .":"");else ō=" "+ō;Γ+=ō+"\n";return Ηψ==Ĩ&&Θ;}String Εμ(bool Θ)
{int Α=0;int Η=Ў;Ő=0;String Ɠ="———————————————\n";String Ƙ="--------------------------------------------\n";String Γ="";Γ+=ΓΡ(β)+" | "+(Ο.ι?"Ready to dock":"No dock")+"\n";Γ+=Ɠ;double Ȋ=Math.Max(Math.Round(this.б),0);if(ϋ==Σ.Main){bool Ī=Κ==Ι.έ;if(Μ(ref Γ,Η,Α++,Θ," Record path & set home")) ΓЁ();
if(Κ==Ι.ƪ)
if(Μ(ref Γ,Η,Α++,Θ," Setup mining job")) κ(Σ.Ʃ);if(Κ==Ι.ǣ)
if(Μ(ref Γ,Η,Α++,Θ," Setup grinding job")) κ(Σ.Ʃ);if(Κ==Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," Setup shuttle job")) κ(Σ.Γύ);if(Μ(ref Γ,Η,Α++,Θ," Continue job")) Ę();if(Μ(ref Γ,Η,Α++,Θ," Fly to home position")) Αο();if(Μ(ref Γ,Η,Α++,Θ," Fly to job position")) Ķ();if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(Ī) κ(Σ.ΑІ);else κ(Σ.Αζ);if(Μ(ref Γ,Η,Α++,Θ," Info")) κ(Σ.ΕЌ);if(Κ!=Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," Help")) κ(Σ.ΕЉ);}else if(ϋ==Σ.Ʃ){double ΚЅ=Math.Round(я*ȏ,1);double ΙЉ=Math.Round(ю*Ȏ,1);String Ж="";if(Μ(ref Ж,Η,Α++,Θ," Start new job!")) ΓЀ();if(Μ(ref Ж,Η,Α++,Θ," Change current job")){Ʋ(false,false);κ(Σ.Main);}if(Μ(ref Ж,Η,Α++,Θ," Width + (Width: "+я+" = "+ΚЅ+"m)")){Š(ref я,5,20,1);Ľ(true);}
if(Μ(ref Ж,Η,Α++,Θ," Width -")){Š(ref я,-5,20,-1);Ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Height + (Height: "+ю+" = "+ΙЉ+"m)")){Š(ref ю,5,20,1);Ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Height -")){Š(ref ю,-5,20,-1);Ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Depth + ("+(љ==ǐ.ΓΗ?"Depth":"Min")+": "+к+"m)")){Š(ref к,5,50,2);Ľ(true);}
if(Μ(ref Ж,Η,Α++,Θ," Depth -")){Š(ref к,-5,50,-2);Ľ(true);}if(Μ(ref Ж,Η,Α++,Θ," Depth mode: "+Θι(љ))){љ=ę(љ);}if(Μ(ref Ж,Η,Α++,Θ," Start pos: "+Θσ(ȉ))){ȉ=ę(ȉ);}if(Κ==Ι.ǣ&&љ==ǐ.ΒА) љ=ę(љ);Γ+=Γχ(8,Ж,Η,ref Κβ);}else if(ϋ==Σ.Γύ){float[]ΖІ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Μ(ref Γ,Η,Α++,Θ," Next")) κ(Σ.ΖΩ);if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=" Leave connector 1:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+ΕЈ(І.Р))) І.Р=ę(І.Р);if(!І.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Απ((int) І.Ǫ))) І.Ǫ=Ευ(І.Ǫ,ΖІ);Γ+=" Leave connector 2:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+ΕЈ(Ї.Р))) Ї.Р=ę(Ї.Р);
if(!Ї.ΓΨ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Απ((int) Ї.Ǫ))) Ї.Ǫ=Ευ(Ї.Ǫ,ΖІ);}else if(ϋ==Σ.ΖΩ){if(Μ(ref Γ,Η,Α++,Θ," Start job!")) ΓЀ();if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Γύ);Γ+=" Timer: \"Docking connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(І.ǫ!=""?І.ǫ:"-"))) І.ǫ=Αό(ref Κυ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(І.Ǯ!=""?І.Ǯ:"-"))) І.Ǯ=Αό(ref Κχ);Γ+=" Timer: \"Docking connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ї.ǫ!=""?Ї.ǫ:"-"))) Ї.ǫ=Αό(ref Κφ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ї.Ǯ!=""?Ї.Ǯ:"-"))) Ї.Ǯ=Αό(ref Κψ);
}else if(ϋ==Σ.ƨ){String Ηέ=ȅ+" %";if(ĉ) ĩ++;if(ĩ>1){ĩ=0;ΒΩ++;if(ΒΩ>1) ΒΩ=0;}bool[]ΕΘ=new bool[]{Ĭ.Count==0,Ǽ==ѕ.None,Ǖ.Count==0};int ΗБ=0;while(true){ΗБ++;Ŋ++;if(Ŋ>ΕΘ.Length-1) Ŋ=0;if(ΗБ>=ΕΘ.Length) break;if(!ΕΘ[Ŋ]) break;}bool Ī=Κ==Ι.έ;if(!Ī&&ĕ&&г!=-1&&ΒΩ==0)
Ηέ=г<1000000?Math.Round(г)+" Kg":Math.Round(г/1000)+" t";if(Μ(ref Γ,Η,Α++,Θ," Stop!")){Ѐ();κ(Σ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!Ī) κ(Σ.Αζ);else κ(Σ.ΑІ);if(!Ī){if(Μ(ref Γ,Η,Α++,Θ," Next hole")) ΒЉ(false);}else if(Μ(ref Γ,Η,Α++,Θ," Undock")) ΑΫ=true;Γ+=Ƙ;if(!Ī) Γ+="Progress: "+Math.Round(ΑΔ,1)+" %\n";Γ+="State: "+ΓΤ(Ϊ)+" "+Ȋ+"m \n";Γ+="Load: "+Βα+" % Max: "+Ηέ+" \n";if(Ŋ==0) Γ+="Uranium: "+(Ĭ.Count==0?"No reactors":Math.Round(ś,1)+"Kg "+(н==-1?"":" Min: "+н+" Kg"))+"\n";
if(Ŋ==1) Γ+="Battery: "+(Ǽ==ѕ.None?Εό(Ǽ):ț+"% "+(ѝ==-1||Ī?"":" Min: "+ѝ+" %"))+"\n";if(Ŋ==2) Γ+="Hydrogen: "+(Ǖ.Count==0?"No tanks":Math.Round(Ĉ,1)+"% "+(ў==-1||Ī?"":" Min: "+ў+" %"))+"\n";}else if(ϋ==Σ.Αζ){String Ж="";if(Μ(ref Ж,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.ƨ);else κ(Σ.Main);}
if(Μ(ref Ж,Η,Α++,Θ," Max load: "+ȅ+"%")) ш(ref ȅ,ȅ<=80?-10:-5,ϊ.Αψ,false);if(Μ(ref Ж,Η,Α++,Θ," Weight limit: "+(ĕ?"On":"Off"))) ĕ=!ĕ;if(Μ(ref Ж,Η,Α++,Θ," Ejection: "+Θλ(в))) в=ę(в);if(Μ(ref Ж,Η,Α++,Θ," Toggle sorters: "+(ř?"On":"Off"))){ř=!ř;if(ř) Ǻ(ΕΛ);}
if(Μ(ref Ж,Η,Α++,Θ," Unload ice: "+(Ĕ?"On":"Off"))) Ĕ=!Ĕ;if(Μ(ref Ж,Η,Α++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ш(ref н,(н>5?-5:-1),ϊ.Αω,true);if(Μ(ref Ж,Η,Α++,Θ," Battery: "+(ѝ==-1?"Ignore":"Min "+ѝ+"%"))) ш(ref ѝ,-5,ϊ.Γζ,true);if(Μ(ref Ж,Η,Α++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Min "+ў+"%"))) ш(ref ў,-10,ϊ.Γη,true);
if(Μ(ref Ж,Η,Α++,Θ," When done: "+(į?"Return home":"Stop"))) į=!į;if(Μ(ref Ж,Η,Α++,Θ," On damage: "+Εώ(ё))) ё=ę(ё);if(Μ(ref Ж,Η,Α++,Θ," Advanced...")) κ(Σ.ΒЊ);Γ+=Γχ(8,Ж,Η,ref Κγ);}else if(ϋ==Σ.ΒЊ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.ƨ);else κ(Σ.Main);}
if(Μ(ref Γ,Η,Α++,Θ,(Κ==Ι.ǣ?" Grinder":" Drill")+" inv. balancing: "+(Ž?"On":"Off"))) Ž=!Ž;if(Μ(ref Γ,Η,Α++,Θ," Enable"+(Κ==Ι.ǣ?" grinders":" drills")+": "+(ŷ?"Fwd + Bwd":"Fwd"))) ŷ=!ŷ;if(Μ(ref Γ,Η,Α++,Θ," Work speed fwd.: "+Ɵ+"m/s")) ш(ref Ɵ,0.5f,ϊ.Ƴ,false);
if(Μ(ref Γ,Η,Α++,Θ," Work speed bwd.: "+ƞ+"m/s")) ш(ref ƞ,0.5f,ϊ.Ƴ,false);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ш(ref ǻ,0.1f,ϊ.Αγ,false);if(Μ(ref Γ,Η,Α++,Θ," Width overlap: "+ŝ*100f+"%")) Ερ(true,0.05f);if(Μ(ref Γ,Η,Α++,Θ," Height overlap: "+ň*100f+"%")) Ερ(false,0.05f);
}else if(ϋ==Σ.ΑІ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(β==Ρ.З) κ(Σ.ƨ);else κ(Σ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Max load: "+ȅ+"%")) ш(ref ȅ,ȅ<=80?-10:-5,ϊ.Αψ,false);if(Μ(ref Γ,Η,Α++,Θ," Unload ice: "+(Ĕ?"On":"Off"))) Ĕ=!Ĕ;if(Μ(ref Γ,Η,Α++,Θ," Uranium: "+(н==-1?"Ignore":"Min "+н+"Kg"))) ш(ref н,(н>5?-5:-1),ϊ.Αω,true);
if(Μ(ref Γ,Η,Α++,Θ," Battery: "+(ѝ==-1?"Ignore":"Charge up"))) ѝ=(ѝ==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," Hydrogen: "+(ў==-1?"Ignore":"Fill up"))) ў=(ў==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," On damage: "+Εώ(ё))) ё=ę(ё);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǻ*100f)+"%"+(ǻ>0.80f?" (risky)":""))) ш(ref ǻ,0.1f,ϊ.Αγ,false);
}else if(ϋ==Σ.ΖΡ){double ΚΔ=0;if(Φ.Count>0) ΚΔ=Vector3.Distance(Φ.Last().Ε,ε);if(Μ(ref Γ,Η,Α++,Θ," Stop path recording")) Ζή();if(Κ!=Ι.έ){if(Μ(ref Γ,Η,Α++,Θ," Home: "+(ȫ?"Use old home":(Ο.ι?"Was set! ":"none ")))) ȫ=!ȫ;}else{if(Μ(ref Γ,Η,Α++,Θ," Connector 1: "+(ȫ?"Use old connector":(Ο.ι?"Was set! ":"none ")))) ȫ=!ȫ;
if(Μ(ref Γ,Η,Α++,Θ," Connector 2: "+(ƙ?"Use old connector":(Ζ.ι?"Was set! ":"none ")))) ƙ=!ƙ;}if(Μ(ref Γ,Η,Α++,Θ," Path: "+(ƚ?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) ƚ=!ƚ;Γ+=Ƙ;Γ+="Wp spacing: "+Math.Round(ΘΗ)+"m\n";}else if(ϋ==Σ.ΓЂ){if(Μ(ref Γ,Η,Α++,Θ," Stop")){Ѐ();κ(Σ.Main);}
Γ+=Ƙ;Γ+="State: "+ΓΤ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(Đ,1)+"m/s\n";Γ+="Target dist: "+Ȋ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+Εα+"\n";}else if(ϋ==Σ.ΕЌ){List<IMyTerminalBlock>ł=ΕϏ();if(ĉ) ĩ++;if(ĩ>=ł.Count) ĩ=0;if(Μ(ref Γ,Η,Α++,Θ," Next")) κ(Σ.ΕЍ);
Γ+=Ƙ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βα,1)+"% "+Math.Round(Ȁ,1)+" / "+Math.Round(Ȇ,1)+"\n";Γ+="Uranium: "+(Ĭ.Count==0?"No reactors":Math.Round(ś,1)+"Kg "+Δύ)+"\n";Γ+="Battery: "+(Ǽ==ѕ.None?"":ț+"% ")+Εό(Ǽ)+"\n";Γ+="Hydrogen: "+(Ǖ.Count==0?"No tanks":Math.Round(Ĉ,1)+"% ")+"\n";
Γ+="Damage: "+(ł.Count==0?"None":""+(ĩ+1)+"/"+ł.Count+" "+ł[ĩ].CustomName)+"\n";}else if(ϋ==Σ.ΕЍ){if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=Ƙ;Γ+="Next scan: "+ΑΟ+"s\n";Γ+="Drill area: "+Math.Round(ȏ,1)+"x"+Math.Round(Ȏ,1)+"m Ship: "+Math.Round(Βυ,1)+"x"+Math.Round(Βτ,1)+"m ("+Math.Round(ο,1)+"m)\n";
Γ+="Broadcast: "+(ų?"Online - "+Ɣ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ƅ*100f,1)+"% \n";}else if(ϋ==Σ.ΕЉ){if(Μ(ref Γ,Η,Α++,Θ," Back")) κ(Σ.Main);Γ+=Ƙ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+ΑΟ+"s\n";if(Βς) Γ="Recompile script now";int ΚΕ=Γ.Split('\n').Length;for(int i=ΚΕ;i<=10;i++) Γ+="\n";Γ+=Ɠ;Γ+="Last: "+Λ+"\n";
return Γ;}void Ερ(bool ΙБ,float ǝ){Ѐ();Ʋ(true,false);if(ΙБ) ш(ref ŝ,ǝ,ϊ.Γρ,false);else ш(ref ň,ǝ,ϊ.Γρ,false);Εο();ļ(true,true,0,0);}float Ευ(float ν,float[]ΒІ){float ή=ΒІ[0];for(int i=ΒІ.Length-1;i>=0;i--)
if(ν<ΒІ[i]) ή=ΒІ[i];return ή;}String Αό(ref int Ĩ){String ή="";if(Ĩ>=ΕΦ.Count) Ĩ=-1;if(Ĩ>=0) ή=ΕΦ[Ĩ].CustomName;Ĩ++;return ή;}void ΑЈ(string ί){if(β!=Ρ.З) return;if(ί=="") return;IMyTerminalBlock Ψ=χ.GetBlockWithName(ί);if(Ψ==null||!(Ψ is IMyTimerBlock)){Λ="Timerblock "+ί+" not found!";
return;}((IMyTimerBlock) Ψ).Trigger();}void Š(ref int value,int ƻ,int ē,int ΕΙ){if(ƻ==0) return;if(value<ē&&ΕΙ>0||value<=ē&&ΕΙ<0){value+=ΕΙ;return;}int Ιε=Math.Abs(ƻ);int ΓЈ=0;int ΔІ=1;while(true){ΓЈ+=ΔІ*Ιε*10;if(ƻ<0&&value-ē<=ΓЈ) break;if(ƻ>0&&value-ē<ΓЈ) break;
ΔІ++;}value+=ΔІ*ƻ;}void Ľ(bool ΛΒ){я=Math.Max(я,1);ю=Math.Max(ю,1);к=Math.Max(к,0);ļ(ϋ==Σ.Ʃ,false,0,0);}bool ķ(ref float value,bool Κί,ϊ υ,String ŋ,String ΙЎ){if(ŋ=="") return false;float ƈ=-1;bool ΗΝ=false;if(ŋ.ToUpper()==ΙЎ) ΗΝ=true;else if(!float.TryParse(ŋ,out ƈ)) return false;
else ƈ=Math.Max(0,ƈ);if(Κί) ƈ=(float) Math.Round(ƈ);ш(ref value,ƈ,υ,ΗΝ,false);return true;}void ш(ref float value,float ǝ,ϊ υ,bool ΑΪ){ш(ref value,value+ǝ,υ,ΑΪ,true);}void ш(ref float value,float ƅ,ϊ υ,bool ΑΪ,bool ΚЊ){float ǰ=0,ǯ=0;if(υ==ϊ.Ƴ){ǰ=0.5f;ǯ=10f;}if(υ==ϊ.Αγ){ǰ=0.1f;ǯ=1f;}
if(υ==ϊ.ƥ){ǰ=50f;ǯ=100f;}if(υ==ϊ.Γζ){ǰ=5f;ǯ=30f;}if(υ==ϊ.Αω){ǰ=1f;ǯ=25f;}if(υ==ϊ.Αψ){ǰ=10f;ǯ=95f;}if(υ==ϊ.Γη){ǰ=10f;ǯ=90f;}if(υ==ϊ.ΙΟ){ǰ=10f;ǯ=1800;}if(υ==ϊ.Γρ){ǰ=0.0f;ǯ=0.75f;}if(ƅ==-1&&ΑΪ){value=-1;return;}if(value==-1) ΑΪ=false;
bool Ηρ=ƅ<ǰ||ƅ>ǯ;if(Ηρ&&ΚЊ){if(ƅ<value) value=ǯ;else if(ƅ>value) value=ǰ;}else value=ƅ;if(Ηρ&&ΑΪ) value=-1;else value=Math.Max(ǰ,Math.Min(value,ǯ));value=(float) Math.Round(value,2);}void ΒЉ(bool ΚΪ){if(ΚΪ) а=Math.Max(0,а-1);else а++;ΓΝ();ΓΜ();for(int k=0;k<ȣ.Count&&k<м.Length;k++) Γϋ(ȣ[k],k<а);
Г=-1;ΓІ(true);}T ę<T>(T ν){int Α=Array.IndexOf(Enum.GetValues(ν.GetType()),ν);Α++;if(Α>=Θγ(ν)) Α=0;return(T) Enum.GetValues(ν.GetType()).GetValue(Α);}int Θγ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЁ(){Αα.Clear();for(int i=0;i<Φ.Count;i++) Αα.Add(Φ[i]);Φ.Clear();
Βέ=true;Δν=new Ϋ(Ο);Δπ=new Ϋ(Ζ);Ο.ι=false;if(Κ==Ι.έ) Ζ.ι=false;for(int i=0;i<Ǟ.Count;i++)
if(!ȩ.Contains(Ǟ.Keys.ElementAt(i)))
ȩ.Add(Ǟ.Keys.ElementAt(i));ȫ=false;ƙ=false;ƚ=false;κ(Σ.ΖΡ);}void Ζή(){if(ȫ) Ο=Δν;if(ƙ) Ζ=Δπ;if(ƚ){Φ.Clear();for(int i=0;i<Αα.Count;i++) Φ.Add(Αα[i]);}Βέ=false;Ѐ();κ(Σ.Main);}void Ιή(){if(!Βέ) return;if(Ϊ!=Β.с){Ζή();return;}if(!Δν.ι) ȫ=false;if(!Δπ.ι) ƙ=false;if(Αα.Count<=1) ƚ=false;
IMyShipConnector О=Я(MyShipConnectorStatus.Connectable);if(О==null) О=Я(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Đ,2)<=0.20) ΔΨ++;else ΔΨ=0;if(ΔΨ>=5){if(Κ==Ι.έ&&(Ο.ι||ȫ)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.θ=Ω.WorldMatrix.Forward;Ζ.Ё=Ω.WorldMatrix.Left;
Ζ.η=Ω.WorldMatrix.Down;Ζ.φ=Ω.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.ι=true;Ζ.л=О.Position;}else{Ο.θ=Ω.WorldMatrix.Forward;Ο.Ё=Ω.WorldMatrix.Left;Ο.η=Ω.WorldMatrix.Down;Ο.φ=Ω.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.ι=true;Ο.л=О.Position;}}}double Δς=-1;if(Φ.Count>0) Δς=Vector3.Distance(ε,Φ.Last().Ε);
double Κκ=Math.Max(1.5,Math.Pow(Đ/100.0,2));double Ηθ=Math.Max(Đ*Κκ,2);ΘΗ=Ηθ;if((Δς==-1)||Δς>=Ηθ){Ϋ ά=new Ϋ(ε,ΒΦ,ǁ,ΑΖ,Ω.GetNaturalGravity());ά.ΘΟ(Ȫ,ȩ);Φ.Add(ά);}}Ϋ ъ(){if(Κ!=Ι.έ) return Ǚ!=null?Ǚ:Ο;return ΖЁ;}void Ū(Ϋ α,Ρ Ђ){ΖЁ=α;if(β==Ρ.З) ќ=Ђ;}bool Ȕ(Ϋ α,float Њ,bool ΚЁ,out Vector3 ξ)
{if(ΚЁ){Vector3I Δδ=new Vector3I((int) α.л.X,(int) α.л.Y,(int) α.л.Z);IMySlimBlock Ψ=Me.CubeGrid.GetCubeBlock(Δδ);if(Ψ==null||!(Ψ.FatBlock is IMyShipConnector)){ξ=new Vector3();return false;}Vector3 Ιτ=т(Ω,Ψ.FatBlock.GetPosition()-ε);Vector3 Ισ=т(Ω,Ψ.FatBlock.WorldMatrix.Forward);ξ=α.Ε-ΓЅ(α.θ,α.η*-1,Ιτ)-ΓЅ(α.θ,α.η*-1,Ισ)*Њ;
return true;}else{ξ=α.Ε;return true;}}Vector3 ȯ(int Ιρ,int Ǝ,bool Κέ){if(!Κέ&&ΗΗ) return ΔΘ;float Ις=((Ќ-1f)/2f)-Ιρ;float Κΰ=((Ь-1f)/2f)-Ǝ;ΔΘ=Ζ.Ε+Ζ.Ё*Ις*ȏ+č*-1*Κΰ*Ȏ;ΗΗ=true;return ΔΘ;}Vector3 ΓΣ(Vector3 Κξ,float ΔΤ){return Κξ+(Ǳ*ΔΤ);}γ ΓΘ(){float Щ=-1;γ ή=γ.Ȓ;
if(Κ!=Ι.έ){ΖΖ();if(β!=Ρ.ы){Vector3 Н=Ļ(Ǳ,č*-1,ε-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ќ*ȏ)/2f&&Math.Abs(Н.Y)<=(float)(Ь*Ȏ)/2f){if(Н.Z<=-1&&Н.Z>=-Ŗ*2) return γ.Αε;if(Н.Z>-1&&Н.Z<ο*2) return γ.Ȓ;}if(ė(Ζ.Ε,ref Щ)) ή=γ.Ȓ;}if(Ο.ι){if(ė(Ο.Ε,ref Щ)) ή=γ.ȑ;for(int i=0;i<Φ.Count;i++)
if(ė(Φ[i].Ε,ref Щ)) ή=γ.ȱ;if(Vector3.Distance(ε,Ο.Ε)<dockDist*ο) ή=γ.ȑ;if(Я(MyShipConnectorStatus.Connectable)!=null||Я(MyShipConnectorStatus.Connected)!=null)
ή=γ.ȑ;}}else{Vector3 ξ=new Vector3();IMyShipConnector Ȟ=Я(MyShipConnectorStatus.Connected);if(Ο.ι){if(ė(Ο.Ε,ref Щ)) ή=γ.ȑ;if(Ȕ(Ο,dockDist,true,out ξ))
if(ė(ξ,ref Щ)) ή=γ.ȑ;if(Ȟ!=null&&Vector3.Distance(Ȟ.GetPosition(),Ο.Ε)<5)
return γ.ΒЍ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ο.Ε)>dockDist*ο&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*ο)
if(ė(Φ[i].Ε,ref Щ)) ή=γ.ȱ;if(Ζ.ι){if(ė(Ζ.Ε,ref Щ)) ή=γ.Ȓ;if(Ȕ(Ζ,dockDist,true,out ξ))
if(ė(ξ,ref Щ)) ή=γ.Ȓ;if(Ȟ!=null&&Vector3.Distance(Ȟ.GetPosition(),Ζ.Ε)<5)
return γ.ΒЎ;}}return ή;}bool ė(Vector3 ξ,ref float Щ){float Њ=Vector3.Distance(ξ,ε);if(Њ<Щ||Щ==-1){Щ=Њ;return true;}return false;}void ΓЀ(){if(А>0){Λ="Setup error! Can't start";return;}if(Κ==Ι.έ){Ę();return;}Ζ.Ε=ε;Ζ.φ=Ω.GetNaturalGravity();Ζ.θ=ΒΦ;Ζ.η=ǁ;Ζ.Ё=ΑΖ;
Ǳ=ǚ.WorldMatrix.Forward;č=Ζ.η;if(Ǳ==Ω.WorldMatrix.Down) č=Ω.WorldMatrix.Backward;Ʋ(true,true);Τ(Β.ě);ΓϏ();}void Ʋ(bool Ǭ,bool Κή){if(β==Ρ.ы&&!Ǭ) return;bool ΚΝ=Ǭ||β==Ρ.ΓΚ||Ќ!=я||Ь!=ю||Ō!=ȉ;if(ΚΝ){if(β!=Ρ.ы){β=Ρ.Αθ;ȯ(Ϗ,ό,Κή);Λ="Job changed, lost progress";}Ō=ȉ;
Ќ=я;Ь=ю;ό=0;Ϗ=0;м="";Г=-1;ŏ=0;ċ=0;ж=0;а=0;ΓІ(true);}}void Γσ(){е(ε,0);Ǔ(Ȫ,true);}void ΓϏ(){ȗ(у.ΑЃ);Γσ();Ʈ(Ȧ,false);Ǻ(ΕΛ);β=Ρ.З;Εξ(true);ќ=β;κ(Σ.ƨ);ΘА();ΒΙ=true;ΒΪ=0;for(int i=ƴ.Count-1;i>=0;i--)
if(Ʀ(ƴ[i],false)) ΒΪ++;if(ΒΪ>0) Λ="Started with damage";}void Ѐ(){if(β==Ρ.З){β=Ρ.Ũ;Λ="Job paused";}Τ(Β.с);ќ=β;ΑЅ(false,0,0,0,0);ΓЃ();Ει(new Vector3(),false);Ζέ();Γϊ(ChargeMode.Auto);Γό(false);ΖΤ(true);ȳ(Β.с);ļ(false,false,0,0);Ǔ(ω,false);Ǔ(ј,true);Ǻ(true);Βσ=false;Αι();
ǀ=false;Γυ();ΒΙ=false;ΑΡ=false;ΑΫ=false;if(ϋ!=Σ.Main&&ϋ!=Σ.Αζ&&ϋ!=Σ.ΒЊ&&ϋ!=Σ.ΑІ)
κ(Σ.Main);}void Ę(){γ Э=ΓΘ();if(Κ==Ι.έ){if(!Ζ.ι||!Ο.ι) return;ΓϏ();bool Δβ=Vector3.Distance(ε,Ο.Ε)<Vector3.Distance(ε,Ζ.Ε);if(Ƒ==Ρ.Ş) Δβ=true;if(Ƒ==Ρ.ş) Δβ=false;if(Δβ){Ū(Ο,Ρ.Ş);switch(Э){case γ.ΒЍ:Τ(Β.ğ);break;case γ.ȱ:Τ(Β.р);break;case γ.ȑ:Τ(Β.Ǹ);break;default:Τ(Β.Ǧ);break;
}}else{Ū(Ζ,Ρ.ş);switch(Э){case γ.ΒЎ:Τ(Β.ğ);break;case γ.Ȓ:Τ(Β.Ǹ);break;case γ.ȱ:Τ(Β.р);break;default:Τ(Β.Ǧ);break;}}}else{if(β!=Ρ.Ũ&&β!=Ρ.Αθ) return;bool ΚЃ=β==Ρ.Αθ;ΓϏ();bool Δι=Ƨ(false)&&Ο.ι;switch(Э){case γ.Ȓ:Τ(Δι?Β.Ě:Β.ě);break;case γ.ȱ:Τ(Δι?Β.Л:Β.р);break;case γ.ȑ:Τ(Δι?Β.Ǹ:Β.ș);break;
case γ.Αε:{if(ċ!=а||ΚЃ) Τ(Β.Ȳ);else Τ(Β.ƫ);}break;default:break;}}}void Ķ(){if(β==Ρ.ы&&!Ο.ι) return;if(Κ==Ι.έ&&(!Ζ.ι||!Ο.ι)) return;Λ="Move to job";γ Э=ΓΘ();if(Κ==Ι.έ){Ū(Ζ,Ρ.ş);switch(Э){case γ.Ȓ:Τ(Β.Ǹ);break;case γ.ȱ:Τ(Β.р);break;case γ.ΒЎ:return;default:Τ(Β.Ǧ);break;
}ȳ(Β.ğ);}else{switch(Э){case γ.Ȓ:Τ(Β.р);break;case γ.ȱ:Τ(Β.р);break;case γ.ȑ:Τ(Β.ș);break;case γ.Αε:Τ(Β.Ȳ);break;default:break;}if(β==Ρ.ы) ȳ(Β.р);else ȳ(Β.Ė);Βσ=true;}Γσ();κ(Σ.ΓЂ);Ʈ(Ȧ,false);ќ=Ρ.Αϋ;}void Αο(){if(!Ο.ι) return;Λ="Move home";γ Э=ΓΘ();if(Κ==Ι.έ){Ū(Ο,Ρ.Ş);switch(Э)
{case γ.ȱ:Τ(Β.Л);break;case γ.ȑ:Τ(Β.Ǹ);break;case γ.ΒЍ:return;default:Τ(Β.Ǧ);break;}ȳ(Β.ğ);}else{if(Я(MyShipConnectorStatus.Connected)!=null) return;if(Я(MyShipConnectorStatus.Connectable)!=null){Τ(Β.ĵ);ȳ(Β.ș);return;}switch(Э){case γ.Ȓ:Τ(Β.Ě);break;case γ.ȱ:Τ(Β.Л);break;case γ.ȑ:Τ(Β.Л);break;
case γ.Αε:Τ(Β.Ĺ);break;default:break;}ȳ(Β.ș);}Γσ();κ(Σ.ΓЂ);Ʈ(Ȧ,false);ќ=Ρ.Αϊ;}void Τ(Β ΔЇ){if(ΔЇ==Β.с) ΑΥ=Β.с;if(ΑΥ!=Β.с&&Ϊ==ΑΥ&&ΔЇ!=ΑΥ){Ѐ();return;}Д=true;Ϊ=ΔЇ;}void ȳ(Β Ђ){this.ΑΥ=Ђ;}ǥ ΚЏ(int ΑΒ,bool ǳ){if(ǳ){ǜ=null;Ϗ=0;ό=0;}if(ȉ==ь.Ʊ){int ő=ΑΒ+1;
ό=(int) Math.Floor(И(ΑΒ,Ќ));if(ό%2==0) Ϗ=ΑΒ-(ό*Ќ);else Ϗ=Ќ-1-(ΑΒ-(ό*Ќ));if(ό>=Ь) return ǥ.Αδ;else return ǥ.Αρ;}else if(ȉ==ь.Επ){if(ǜ==null) ǜ=new int[]{0,-1,0,0};int ǋ=(int) Math.Ceiling(Ќ/2f);int Ǌ=(int) Math.Ceiling(Ь/2f);int Δη=(int) Math.Floor(Ќ/2f);
int Δζ=(int) Math.Floor(Ь/2f);int ΑΓ=0;while(ǜ[2]<Math.Pow(Math.Max(Ќ,Ь),2)){if(ΑΓ>200) return ǥ.Θδ;ΑΓ++;ǜ[2]++;if(-ǋ<Ϗ&&Ϗ<=Δη&&-Ǌ<ό&&ό<=Δζ){if(ǜ[3]==ΑΒ){this.Ϗ=Ϗ-1+ǋ;this.ό=ό-1+Ǌ;return ǥ.Αρ;}ǜ[3]++;}if(Ϗ==ό||(Ϗ<0&&Ϗ==-ό)||(Ϗ>0&&Ϗ==1-ό))
{int Κρ=ǜ[0];ǜ[0]=-ǜ[1];ǜ[1]=Κρ;}Ϗ+=ǜ[0];ό+=ǜ[1];}}return ǥ.Αδ;}void Ιά(){if(Ϊ==Β.ě){if(Д){Е=0;if(ċ!=а) ŏ=0;ċ=а;}if(Е==0){int ΔΛ=ΘΦ(а);if(ΔΛ<0){if(!ΔΠ){ΔΠ=true;Ζώ=ε;}е(Ζώ,0);Λ="Waiting for the controller";return;}ΔΠ=false;if(ΔΛ!=а){а=ΔΛ;ŏ=0;ċ=а;}
ǥ ή=ΙΘ(а);if(ή==ǥ.Αδ){β=Ρ.ΓΚ;Λ="Job done";if(į&&Ο.ι){Τ(Β.Ě);ȳ(Β.ș);ќ=Ρ.Αϊ;}else{Τ(Β.ţ);ȳ(Β.Ė);ќ=Ρ.Αϋ;}return;}if(ή==ǥ.Αρ){Е=1;Ǔ(ω,true);џ=ȯ(Ϗ,ό,true);е(џ,10);и(Ζ.η,Ζ.θ,Ζ.Ё,false);}}else{if(Г>=0&&(ΖΕ()?ȟ>=0&&ȟ!=Г:ΓΪ(Г,true))){Е=0;return;}if(б<wpReachedDist){Τ(Β.ƫ);return;}
}}if(Ϊ==Β.ƫ){if(Д){Ǔ(ω,true);Ǻ(false);џ=ȯ(Ϗ,ό,false);е(ΓΣ(џ,0),0);и(Ζ.η,Ζ.θ,Ζ.Ё,false);Е=1;ѐ=0;п=0;ΑА=0;Δτ=-1;Ŗ=к;Δξ=true;}if(!ΓΓ()){Τ(Β.Ĺ);return;}if(Ƨ(true)){Ηο=ȭ("","ORE",Ш.Ĳ);if((в==В.ΓΛ||в==В.Ακ||
в==В.ΓΫ||в==В.Ατ)&&Κ!=Ι.ǣ)
Τ(Β.ƣ);else if((в==В.Ετ||в==В.ΓΕ)&&Κ!=Ι.ǣ)
Τ(Β.ΓЇ);else
Τ(Β.Ĺ);return;}ж=Vector3.Distance(ε,џ);if(ж>ŏ){ŏ=ж;Δξ=false;}if(Κ==Ι.ǣ&&ΕЀ()==MyDetectedEntityType.SmallGrid)
п+=2;else п-=2;п=Math.Max(100,Math.Min(400,п));if(Е>0&&Е<п){if(ж>ѐ){if(п>150) ѐ=ж;else ѐ=(float) Math.Ceiling(ж);Е=1;}else Е++;}else{if(Е>0){Λ="Ship stuck! Retrying";ѐ=ж;Е=0;ļ(false,true,0,ο*sensorRange);}е(ΓΣ(џ,Math.Max(0,ѐ-ο)),ΓΦ(false));if(б<=wpReachedDist/2){Е=1;ѐ=0;}
return;}ļ(false,true,ο*sensorRange,0);Vector3 Ιη=џ+Ǳ*ж;bool ΗΡ=false;if(Vector3.Distance(Ιη,ε)>0.3f){Vector3 Ιψ=џ+Ǳ*(ж+0.1f);е(Ιψ,4);ΗΡ=true;}else{float Ǵ=ΓΦ(true);Vector3 ΖЅ=ΓΣ(џ,Math.Max(к+1,ж+1));ǹ(true,false,false,ΖЅ,ΖЅ-џ,Ǵ,Ǵ);}bool ΔΥ=false;if(љ==ǐ.ΒА||љ==ǐ.ΒЏ)
{if(!ΗΡ){float ΔЊ=0;foreach(IMyTerminalBlock ȍ in ω)
ΔЊ+=Θή(ȍ,"","",љ==ǐ.ΒЏ?new string[]{"STONE"}:null);if(ΔЊ>Δτ||ж<к||Δξ){ΓЎ=0;ΑА=ж;Ŗ=(int)(Math.Max(Ŗ,ΑА)+ο/2);}else{ΔΥ=ж-ΑА>2&&ΓЎ>=20;ΓЎ++;}Δτ=ΔЊ;}}else ΔΥ=ж>=Ŗ;if(ċ!=а){Τ(Β.Ȳ);ж=0;return;}if(ΔΥ){а++;Τ(Β.Ȳ);ж=0;return;}}if(Ϊ==Β.Ȳ||Ϊ==Β.Ĺ||Ϊ==Β.ƣ)
{if(Д){џ=ȯ(Ϗ,ό,false);и(Ζ.η,Ζ.θ,Ζ.Ё,false);Ǔ(ω,ŷ);Ǻ(false);ѐ=Vector3.Distance(ε,џ);ļ(false,true,0,ο*sensorRange);}е(џ,ΓΦ(false));if(Vector3.Distance(ε,џ)>=ѐ+5){Ǔ(ω,false);Ǻ(true);Λ="Can´t return!";}if(б<wpReachedDist){if(Ϊ==Β.Ȳ&&Βσ) Τ(Β.ţ);if(Ϊ==Β.Ȳ) Τ(Β.ě);
if(Ϊ==Β.ƣ) Τ(Β.Αξ);if(Ϊ==Β.Ĺ){if(Ο.ι) Τ(Β.Ě);else{Ѐ();Ķ();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Β.Л||Ϊ==Β.р){if(Ϊ==Β.р&&β==Ρ.З&&Κ!=Ι.έ){if(!ΓΓ()||Ƨ(true)){Τ(Β.Л);return;}}bool ΕΑ=false;bool ΒЄ=false;bool Ζι=false;float Ű=0;bool Ηξ=false;Ϋ ά=null;
if(Д){if(Ϊ==Β.Л||Κ==Ι.έ){if(Κ!=Ι.έ){if(Ĝ(1)) ΒЌ();else{Ǚ=null;Ȩ=0;ǀ=!ΓΔ(1);}}Ϋ α=ъ();μ=new Γς();μ.ζ=α;μ.ƾ=followPathDock*ο;μ.Εή=useDockDirectionDist*ο;μ.Ű=10;μ.Ɨ.Add(α.Ε);Vector3 ů=new Vector3();if(Ȕ(α,dockDist*ο,true,out ů))
μ.Ɨ.Add(ů);else μ.ƾ*=1.5f;if(Κ==Ι.έ){if(α==Ο) μ.ΒΣ=Ζ.Ε;if(α==Ζ) μ.ΒΣ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;}}else if(Ϊ==Β.р){μ=new Γς();μ.ζ=Ζ;μ.ƾ=followPathJob*ο;μ.Εή=useJobDirectionDist*ο;μ.Ű=10;μ.ΒΣ=Ο.Ε;μ.Δί=dockDist*ο*1.1f;μ.Ɨ.Add(Ζ.Ε);if(β==Ρ.ы){if(!Ο.ι||Φ.Count==0){Ѐ();return;}
float Ιώ=Vector3.Distance(Φ.First().Ε,Ο.Ε);float ΙϏ=Vector3.Distance(Φ.Last().Ε,Ο.Ε);if(Ιώ<ΙϏ) μ.ζ=Φ.Last();else μ.ζ=Φ.First();}}ΑΝ=new Vector3();Ηξ=!Θύ(ε);Ǔ(ω,false);Ǻ(true);з=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,μ.ΒΣ)<=μ.Δί) continue;
double Њ=Vector3.Distance(Φ[i].Ε,ε);if(ǿ==-1||Њ<ǿ){з=i;ǿ=Њ;}}ȇ=Θπ(μ.ζ.Ε,з);Ł=null;}if(Ϊ==Β.Л&&Κ!=Ι.έ&&!Д&&!ư()){Ϋ ΔΒ=ъ();ǀ=!ΓΔ(1);if(ъ()!=ΔΒ){Τ(Β.Л);return;}}if(Ϊ==Β.Л&&Κ!=Ι.έ&&!Д&&Ĝ(1)&&ΒЌ()){Τ(Β.Л);return;}bool ΗΟ=false;if(Ϊ==Β.Л&&!Д&&з>=0&&з<Φ.Count)
{Vector3 ƽ=Φ[з].Ε+ΕЃ(з,ȇ);int ΔЈ=з+ȇ;Vector3 ő=ȇ!=0&&ΔЈ>=0&&ΔЈ<Φ.Count?Φ[ΔЈ].Ε:μ.ζ.Ε;if(Θψ(ƽ,ő)){if(!ΒΜ){ΒΜ=true;ΖЊ=ŧ(ƽ)?ε:ƽ;}е(ΖЊ,0);и(μ.ζ.η,μ.ζ.θ,μ.ζ.Ё,false);Λ="Waiting for dock clearance";return;}ΒΜ=false;ΗΟ=Ĝ(1)&&(ŧ(ε)||ŧ(ő));}ΘΠ(Φ,ȇ,μ.Ɨ,μ.ƾ,Д,ref Е);
for(int i=0;i<μ.Ɨ.Count;i++){float Њ=Vector3.Distance(ε,μ.Ɨ[i]);if(Њ<=μ.ƾ) ΕΑ=true;if(Њ<=μ.Εή) ΒЄ=true;}if(ΗΟ) ΒЄ=true;if(ΒЄ) Ű=μ.Ű;float ΛΓ=Ł!=null?Ł.Ч:Đ;float ΚЉ=(float) Math.Max(Đ*0.1f*ο,wpReachedDist);if((б<ΚЉ)||Д){if(!Д) з+=ȇ;if(ȇ==0||з>Φ.Count-1||з<0)
ΕΑ=true;else{Εα=ȇ>0?Φ.Count-1-з:з;ά=Φ[з];Ł=ά;if(з>=1&&з<Φ.Count-1)
ΑΝ=ά.Ε-Φ[з-ȇ].Ε;else Ł=null;Ѓ=ά.Ε+ΕЃ(з,ȇ);Ζι=true;}}if(ΒЄ)
и(μ.ζ.η,μ.ζ.θ,μ.ζ.Ё,false);else if(Ηξ)
ΒЋ(μ.ζ.η,10,true);else if(Ζι&&ά!=null){if(ȇ>0) и(ά.η,ά.θ,ά.Ё,90,false);else и(ά.η,-ά.θ,-ά.Ё,90,false);}ǹ(true,false,true,Ѓ,ΑΝ,Ł==null?0:Ł.Ч,Ű);if(ΕΑ){Εα=0;if(Ϊ==Β.Л&&!ΘΤ()){е(Ѓ,0);return;}if(Ϊ==Β.Л&&ǀ){е(Ѓ,0);Λ="Waiting for a free dock";return;}if(Ϊ==Β.Л||Κ==Ι.έ){Τ(Β.Ǹ);return;}
if(Ϊ==Β.р&&Βσ){Τ(Β.ţ);return;}if(Ϊ==Β.р){Τ(Β.ě);return;}}}if(Ϊ==Β.Ǹ||Ϊ==Β.ƭ){if(Κ!=Ι.έ&&ư()){if(!Ĝ(1)){Τ(Β.Л);return;}}else if(Κ!=Ι.έ){Ϋ ΔΒ=ъ();if(!ΓΔ(2)||ъ()!=ΔΒ){Τ(Β.Л);return;}}Ϋ α=ъ();if(Д){if(!Ȕ(α,dockDist*ο,true,out Ѓ)){ȗ(у.ơ);Ѐ();return;}
е(Ѓ,0);ΒЋ(α.η,90,true);}if(б<followPathDock*ο&&б!=-1){е(Ѓ,10);и(α.η,α.θ,α.Ё,false);}if(Я(MyShipConnectorStatus.Connectable)!=null||Я(MyShipConnectorStatus.Connected)!=null){Τ(Β.ĵ);return;}if(б<wpReachedDist/2&&б!=-1){Τ(Β.ı);return;}}if(Ϊ==Β.ı||Ϊ==Β.Ė){if(Д)
{if(Ϊ==Β.ı){Ϋ α=ъ();if(!Ȕ(α,dockDist*ο,true,out Ѓ)){ȗ(у.ơ);Ѐ();return;}ǹ(true,true,false,Ѓ,0);и(α.η,α.θ,α.Ё,10,false);}if(Ϊ==Β.Ė){и(Ζ.η,Ζ.θ,Ζ.Ё,0.5f,false);Ѓ=Ζ.Ε;ǹ(true,true,false,Ѓ,0);}}if(Βά){ΑЅ(false,0,0,0,0);if(Ϊ==Β.ı) Τ(Β.ĵ);if(Ϊ==Β.Ė) Ѐ();return;}}
if(Ϊ==Β.ĵ){if(Я(MyShipConnectorStatus.Connected)!=null){if(Κ==Ι.έ) Τ(Β.ğ);else Τ(Β.ș);return;}Ϋ α=ъ();if(Д){п=0;ΙЀ=DateTime.Now;Е=0;и(α.η,α.θ,α.Ё,false);}Vector3I Δδ=new Vector3I((int) α.л.X,(int) α.л.Y,(int) α.л.Z);IMySlimBlock Ζϋ=Me.CubeGrid.GetCubeBlock(Δδ);float ΖЍ=dockingSpeed;
float ΚΘ=dockingSpeed*5;float Ζη=Math.Max(1.5f,Math.Min(5f,ο*0.15f));if(!Ȕ(α,0,true,out Ѓ)||!Ȕ(α,Ζη,true,out ΑΝ)||
Ζϋ==null||!Ζϋ.FatBlock.IsFunctional){ȗ(у.ơ);Ѐ();return;}if(п==1||(Vector3.Distance(ε,Ѓ)<=Ζη*1.1f&&!Д))
п=1;else{Vector3 Κω=т(Ω,ΑΝ-ε);Vector3 Δγ=т(Ω,Ω.GetNaturalGravity());float Κα=Αη(Κω,Δγ,null);ΖЍ=Math.Min(ΚΘ,Κα);}ǹ(true,false,false,Ѓ,Ѓ-ε,dockingSpeed,ΖЍ);if(Д) ѐ=(float) б;IMyShipConnector ΔΟ=Я(MyShipConnectorStatus.Connectable);if(ΔΟ!=null){ǹ(false,false,false,Ѓ,0);
if(Е>0) Е=0;Е--;if(Е<-5){ΔΟ.Connect();if(ΔΟ.Status==MyShipConnectorStatus.Connected){if(Κ==Ι.έ) Τ(Β.ğ);else Τ(Β.ș);ΓЃ();Ʈ(Ȧ,true);return;}}}else{float ΖЉ=(float) Math.Round(б,1);if(ΖЉ<ѐ){Е=-1;ѐ=ΖЉ;}else Е++;if(Е>20){Τ(Β.ƭ);return;}}}if(Ϊ==Β.ș||Ϊ==Β.ğ||
Ϊ==Β.ΑЋ||Ϊ==Β.Αμ||Ϊ==Β.Ĵ){bool ΓЌ=false,ΓЍ=false;if(Κ==Ι.έ){if(ъ()==Ο) ΓЌ=true;else if(ъ()==Ζ) ΓЍ=true;}if(Д){ΑΡ=false;if(Я(MyShipConnectorStatus.Connected)==null){Τ(Β.Ǧ);return;}ΓЃ();if(ΓЌ) ΑЈ(І.ǫ);if(ΓЍ) ΑЈ(Ї.ǫ);Ś=false;Ā=false;ć=false;Αέ=false;}if(Я(MyShipConnectorStatus.Connected)==null){Ѐ();ȗ(у.ΕА);return;}
if(β!=Ρ.З||ѝ==-1||Ǽ==ѕ.None) Ā=true;else if(ț>=100f) Ā=true;else if(ț<=99f) Ā=false;if(β!=Ρ.З||ў==-1||Ǖ.Count==0) ć=true;else if(Ĉ>=100f) ć=true;else if(Ĉ<=99) ć=false;if(β!=Ρ.З||н==-1||Ĭ.Count==0) Αέ=true;else Αέ=ś>=н;Ŭ є=null;if(ΓЌ) є=І;
if(ΓЍ) є=Ї;if(є!=null&&(є.Р==π.ΑϏ||є.Р==π.Αώ)) Ā=true;if(є!=null&&(є.Р==π.Γκ)) if(!Ś) Ā=false;if(є!=null&&(є.Р==π.Γξ||є.Р==π.Γμ)) ć=true;if(є!=null&&(є.Р==π.Γν)) if(!Ś) ć=false;if(ĉ){ChargeMode Ζσ=Ā?ChargeMode.Auto:ChargeMode.Recharge;if(є!=null&&(є.Р==π.Αώ||є.Р==π.ΑϏ))
Ζσ=ChargeMode.Discharge;Γϊ(Ζσ);Γό(!ć);}if(!Ś){if(Κ==Ι.έ)
Ś=β!=Ρ.З||ΘΥ(Д,true)||ΑΫ;else
Ś=β!=Ρ.З||ΕБ();}else{if(!Ā) Τ(Β.Ĵ);if(!ć) Τ(Β.Αμ);if(!Αέ) Τ(Β.ΑЋ);Д=false;}if(Ś&&Ā&&ć&&Αέ){Γϊ(ChargeMode.Auto);Γό(false);if(β==Ρ.З){if(Κ==Ι.έ){if(ъ()==Ο) ΑЈ(І.Ǯ);else if(ъ()==Ζ) ΑЈ(Ї.Ǯ);if(ъ()==Ο) Ū(Ζ,Ρ.ş);else Ū(Ο,Ρ.Ş);}}Τ(Β.Ǧ);return;}}if(Ϊ==Β.Ǧ){if(Αΰ){if(!Ες()) return;
Αΰ=false;Τ(Β.Ǧ);return;}if(Д){if(!Ες()){Αΰ=true;return;}IMyShipConnector Ȟ=Я(MyShipConnectorStatus.Connected);if(Ȟ==null){Τ(Β.р);return;}IMyShipConnector λ=Ȟ.OtherConnector;ΖΣ(Ȟ,false);Ʈ(Ȧ,false);Ϋ α=null;Ϋ њ=Κ==Ι.έ?Ο:ъ();if(Vector3.Distance(Ȟ.GetPosition(),њ.Ε)<5f&&Ο.ι) α=њ;
if(Vector3.Distance(Ȟ.GetPosition(),Ζ.Ε)<5f&&Ζ.ι) α=Ζ;if(α!=null){if(!Ȕ(α,dockDist*ο,true,out Ѓ)){ȗ(у.ơ);Ѐ();return;}е(Ѓ,5);и(α.η,α.θ,α.Ё,false);}else е(ε+λ.WorldMatrix.Forward*dockDist*ο,5);if(β==Ρ.З) ȗ(у.ΑЃ);}if(б<wpReachedDist){Ǔ(ј,true);Τ(Β.р);return;}}if(Ϊ==Β.ţ)
{if(Д){Ǻ(true);Ǔ(ω,false);Ѓ=Ζ.Ε;е(Ѓ,20);и(Ζ.η,Ζ.θ,Ζ.Ё,false);}if(б<wpReachedDist/2){Τ(Β.Ė);return;}}if(Ϊ==Β.Ě){if(Д){Ǻ(true);Ǔ(ω,false);int Щ=-1;double ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,ε);if(ǿ==-1||Њ<ǿ){Щ=i;ǿ=Њ;}}
if(Щ==-1){Τ(Β.Л);return;}Ѓ=Φ[Щ].Ε;е(Ѓ,10);и(Ζ.η,Ζ.θ,Ζ.Ё,false);}if(б<wpReachedDist){Τ(Β.Л);return;}}if(Ϊ==Β.Αξ){bool ώ=false;if(Д){Ǻ(true);if((в==В.ΓΛ||в==В.Ακ)&&Αφ()&&
š(Ǳ,Ω.GetNaturalGravity())<25&&Ќ>=2&&Ь>=2){Vector3 ǂ=ε;if(Ϗ>0&&ό<Ь-1) ǂ=ȯ(Ϗ-1,ό+1,true);else if(Ϗ<Ќ-1&&ό<Ь-1) ǂ=ȯ(Ϗ+1,ό+1,true);else if(Ϗ<Ќ-1&&ό>0) ǂ=ȯ(Ϗ+1,ό-1,true);else if(Ϗ>0&&ό>0) ǂ=ȯ(Ϗ-1,ό-1,true);else ώ=true;if(!ώ) е(ǂ,10);
}else ώ=true;}if(б<wpReachedDist/2) ώ=true;if(ώ){Τ(Β.ΑЊ);return;}}if(Ϊ==Β.ΓЇ||Ϊ==Β.ΑЊ){if(Д){ǹ(true,true,false,ε,0);Ǔ(ω,false);Ǻ(true);Е=-1;п=в==В.ΓΫ||в==В.Ατ?0:-1;}bool Ņ=!ΓΓ();int Βό=ȭ("STONE","ORE",Ш.Ĳ);if(в==В.ΓΕ||в==В.Ατ||в==В.Ακ)
Βό+=ȭ("ICE","ORE",Ш.Ĳ);bool ΙЈ=Βό>0;bool Δέ=false;if(п>=0){float ŭ=(float) Math.Sin(Εφ(п))*ȏ/3f;float Ιι=(float) Math.Cos(Εφ(п))*Ȏ/3f;Vector3 Ιπ=ȯ(Ϗ,ό,true)+ΓЅ(Ǳ,č*-1,new Vector3(ŭ,Ιι,0));е(Ιπ,0.3f);if(б<Math.Min(ȏ,Ȏ)/10f) п+=5f;if(п>=360) п=0;}if(Е==-1||Βό<Е){Е=Βό;ѐ=0;}
else{ѐ++;if(ѐ>50) Δέ=true;}if(!ΙЈ||Ņ||Δέ){if(!Ņ){int Κώ=ȭ("","ORE",Ш.Ĳ);if(Ƨ(true)) Ņ=true;else if(100-(И(Κώ,Ηο)*100)<minEjection) Ņ=true;else ȗ(у.ΑЃ);}if(Δέ&&Ņ) Λ="Ejection failed";if(Ϊ==Β.ΑЊ){if(Ņ){if(Ο.ι) Τ(Β.Ě);else{Ѐ();Ķ();Λ="Can´t return, no dock found";}}
else Τ(Β.ě);}else if(Ņ) Τ(Β.Ĺ);else Τ(Β.ƫ);return;}}Д=false;}void ΘΠ(List<Ϋ>İ,int ρ,List<Vector3>ΑΨ,float Ȋ,bool ǳ,ref int ǩ){if(ǳ){for(int i=0;i<İ.Count;i++) İ[i].Ч=0;ǩ=-1;return;}if(ρ==0) return;int ǝ=ρ*-1;if(ǩ==-1) ǩ=ǝ>0?1:İ.Count-2;int ΑΓ=0;while(ǩ>=1&&ǩ<İ.Count-1)
{if(ΑΓ>50) return;ΑΓ++;try{if((ǝ<0&&ǩ>=1)||(ǝ>0&&ǩ<=İ.Count-2)){Ϋ ν=İ[ǩ];bool Ηλ=false;for(int j=0;j<ΑΨ.Count;j++)
if(Vector3.Distance(ν.Ε,ΑΨ[j])<=Ȋ){Ηλ=true;break;}if(!Ηλ){Ϋ Βο=İ[ǩ-ǝ];Ϋ ő=İ[ǩ+ǝ];Vector3 Κϊ=ν.Ε-ő.Ε;Vector3 ΕΨ=Βο.Ε-ν.Ε;Vector3 ΚΫ=ν.Ε+Vector3.Normalize(Κϊ)*ΕΨ.Length();Vector3 Ιύ=Βο.Ε-ΚΫ;Vector3 Ββ=Ļ(ρ>0?ν.θ:ν.θ*-1,ν.η*-1,Ιύ);Vector3 ΚΗ=Ļ(ρ>0?ν.θ:ν.θ*-1,ν.η*-1,ΕΨ);
Vector3 Βδ=Ļ(ρ>0?ν.θ:ν.θ*-1,ν.η*-1,ν.φ);ν.Ч=(float) Math.Sqrt(Math.Pow(Βο.Ч,2)+Math.Pow(Αη(-ΚΗ,Βδ,ν),2));for(int j=0;j<ΑΨ.Count;j++)
if(Vector3.Distance(Βο.Ε,ΑΨ[j])<=Ȋ){Vector3 Βε=Ļ(ρ>0?ν.θ:ν.θ*-1,ν.η*-1,ΑΨ[j]-ν.Ε);float Κπ=Αη(-Βε,Βδ,ν);ν.Ч=Math.Min(ν.Ч,Κπ)/2f;}if(Ββ.Length()==0) Ββ=new Vector3(0,0,1);Vector3 ΚΖ=Ļ(ν.θ,ν.η*-1,ν.φ);float Κτ=Ƥ(-Ββ,ΚΖ,ν);float ΑЌ=И(Κτ,Ř);float ΕΥ=(float) Math.Sqrt(И(Ββ.Length()*1.0f,0.5f*ΑЌ));
ν.Ч=Math.Min(ν.Ч,(ΕΨ.Length()/ΕΥ)*ǻ);}}}catch{return;}ǩ+=ǝ;}ǩ=-1;}void ΓІ(bool ǳ){if(ǳ){ΑΔ=0;return;}if(Κ==Ι.έ) return;float ώ=а*Math.Max(1,к);if(ċ==а) ώ+=Math.Min(к,ж);float Κύ=Ќ*Ь*Math.Max(1,к);ΑΔ=Math.Max(ΑΔ,(float) Math.Min(ώ/Κύ*100.0,100));}MyDetectedEntityType ΕЀ()
{try{if(ȕ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓΦ(bool ȃ){if(Κ==Ι.ǣ&&ΕЀ()==MyDetectedEntityType.None&&!Ʀ(ψ,true))
return fastSpeed;else return ȃ?Ɵ:ƞ;}void ΙΣ(){float Ηδ=0,ΖЃ=0,ŋ=0,ΔЋ=0;for(int i=0;i<Ġ.Count;i++){IMyBatteryBlock ƹ=Ġ[i];if(!ȕ(ƹ,true)) continue;Ηδ+=ƹ.MaxStoredPower;ΖЃ+=ƹ.CurrentStoredPower;ŋ+=ƹ.CurrentInput;ΔЋ+=ƹ.CurrentOutput;}ț=(float) Math.Round(И(ΖЃ,Ηδ)*100,1);
if(ŋ>=ΔЋ) Ǽ=ѕ.Ĵ;else Ǽ=ѕ.Εχ;if(ŋ==0&&ΔЋ==0||ț==100.0) Ǽ=ѕ.с;if(Ġ.Count==0) Ǽ=ѕ.None;}void ΙΩ(){float ΗΑ=0;for(int i=0;i<Ǖ.Count;i++){IMyGasTank ΗЉ=Ǖ[i];if(!ȕ(ΗЉ,true)) continue;ΗΑ+=(float) ΗЉ.FilledRatio;}Ĉ=И(ΗΑ,Ǖ.Count)*100f;}void Ιγ(){ś=0;try{for(int i=0;i<Ĭ.Count;i++)
{IMyReactor ΕΒ=Ĭ[i];if(!ȕ(ΕΒ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΒ.GetInventory(0).GetItems(Ћ);float Љ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem ύ=Ћ[j];if(ύ.Type.SubtypeId.ToUpper()=="URANIUM"&&ύ.Type.TypeId.ToUpper().Contains("_INGOT"))
Љ+=(float) ύ.Amount;}if(Љ<ś||i==0){ś=Љ;Δύ=ΕΒ.CustomName;}}}catch(Exception e){Ċ=e;}}void ΙΤ(){Ȇ=0;Ȁ=0;try{for(int i=0;i<Ŵ.Count;i++){IMyTerminalBlock Ψ=Ŵ[i];if(!ȕ(Ψ,true)) continue;Ȁ+=(float) Ψ.GetInventory(0).CurrentVolume;Ȇ+=(float) Ψ.GetInventory(0).MaxVolume;
}Βα=(float) Math.Min(Math.Round(И(Ȁ,Ȇ)*100,1),100.0);}catch(Exception e){Ċ=e;}}void ΙΪ(){try{ǭ.Clear();for(int i=0;i<Ŵ.Count;i++){IMyTerminalBlock Ψ=Ŵ[i];if(!ȕ(Ψ,true)) continue;Εθ(Ψ,Ш.Ŧ);}if(в!=В.Γι){for(int i=0;i<Ŀ.Count;i++){IMyTerminalBlock Ψ=Ŀ[i];if(!ȕ(Ψ,true)) continue;
Εθ(Ψ,Ш.Θϋ);}}ΙΛ(ǭ);}catch(Exception e){Ċ=e;}}void ΘΣ(){if(ΒΙ){if(ΕϏ().Count>ΒΪ){ΒΙ=false;if(ё!=щ.ƥ){Ѐ();if(ё==щ.ΓΠ) Ķ();if(ё==щ.ΑЂ)
if(Ο.ι) Αο();else Ķ();}Λ="Damage detected";}}}bool ΓΓ(){if(!ΒΫ) return true;if(β==Ρ.З){if(ѝ>0&&Ǽ!=ѕ.None)
if(ț<=ѝ){Λ="Low energy! Move home";return false;}if(н>0&&Ĭ.Count>0)
if(ś<=н){Λ="Low fuel: "+Δύ;return false;}if(ў>0&&Ǖ.Count>0)
if(Ĉ<=ў){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕϏ(){List<IMyTerminalBlock>ł=new List<IMyTerminalBlock>();for(int i=0;i<ƴ.Count;i++){IMyTerminalBlock Ψ=ƴ[i];if(Ʀ(Ψ,false)) ł.Add(Ψ);}return ł;}bool Ʀ(IMyTerminalBlock Ψ,bool ΕΝ){return(!ȕ(Ψ,ΕΝ)||!Ψ.IsFunctional);
}bool ȕ(IMyTerminalBlock Ψ,bool ΕΝ){if(Ψ==null) return false;try{IMyCubeBlock ΗΖ=Me.CubeGrid.GetCubeBlock(Ψ.Position).FatBlock;if(ΕΝ) return ΗΖ==Ψ;else return ΗΖ.GetType()==Ψ.GetType();}catch{return false;}}float Θή(IMyTerminalBlock Ψ,String Ǐ,String ΗЁ,String[]ȁ){float Ƞ=0;for(int i=0;i<Ψ.InventoryCount;i++)
{IMyInventory ż=Ψ.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ż.GetItems(Ћ);foreach(MyInventoryItem ύ in Ћ){if(ȁ!=null&&(ȁ.Contains(ύ.Type.TypeId.ToUpper())||ȁ.Contains(ύ.Type.SubtypeId.ToUpper())))
continue;if((Ǐ==""||ύ.Type.TypeId.ToUpper()==Ǐ)&&(ΗЁ==""||ύ.Type.SubtypeId.ToUpper()==ΗЁ))
Ƞ+=(float) ύ.Amount;}}return Ƞ;}й Θζ(String ί,String υ,Ш Э,bool Ιω){ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){й ύ=ǭ[i];if(ύ.ί.ToUpper()==ί&&ύ.υ.ToUpper()==υ&&(ύ.Ɓ==Э||Э==Ш.Ĳ))
return ύ;}й ή=null;if(Ιω){ή=new й(ί,υ,0,Э);ǭ.Add(ή);}return ή;}int ȭ(String ί,String υ,Ш Э){return ȭ(ί,υ,Э,null);}int ȭ(String ί,String υ,Ш Э,String[]ȁ){int Ƞ=0;ί=ί.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǭ.Count;i++){й ύ=ǭ[i];if(ȁ!=null&&ȁ.Contains(ύ.ί.ToUpper())) continue;
if((ί==""||ύ.ί.ToUpper()==ί)&&(υ==""||ύ.υ.ToUpper()==υ)&&(ύ.Ɓ==Э||Э==Ш.Ĳ))
Ƞ+=ύ.Љ;}return Ƞ;}void Εθ(IMyTerminalBlock Ψ,Ш Э){for(int i=0;i<Ψ.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ψ.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
Θζ(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Э,true).Љ+=(int) Ћ[j].Amount;}}void ΙΛ(List<й>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){й a=Ѝ[j];й b=Ѝ[j+1];if(a.Љ<b.Љ) Ѝ.Move(j,j+1);}}bool Ƨ(bool ΚБ){if(ĕ&&Κ!=Ι.έ)
if(г!=-1&&Ř>=г){Λ="Ship too heavy";return true;}if(Βα>=ȅ||ΑΡ){ΑΡ=false;Λ="Ship is full";return true;}return false;}bool ΕБ(){String[]ȁ=null;if(!Ĕ) ȁ=new string[]{"ICE"};if(Κ==Ι.ƪ) return ȭ("","ORE",Ш.Ŧ,ȁ)==0;if(Κ==Ι.ǣ) return ȭ("","COMPONENT",Ш.Ŧ,ȁ)==0;else return ȭ("","",Ш.Ŧ,ȁ)==0;
}bool Θώ(){List<IMyCockpit>ΔΞ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔΞ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔΞ.Count;i++)
if(ΔΞ[i].IsUnderControl) return true;return false;}bool ΘΥ(bool ǳ,bool Βχ){IMyShipConnector О=Я(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return Εσ(І,ǳ,Βχ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return Εσ(Ї,ǳ,Βχ);return false;}bool Εσ(Ŭ ȝ,bool ǳ,bool Βχ){if(ǳ) ȝ.ΘЌ();ȝ.Ιΰ();bool ѓ=false;String ǎ="";switch(ȝ.Р){case π.Γλ:{ǎ="Waiting for command";ѓ=false;break;}case π.ΖΜ:{ǎ="Waiting for cargo";ѓ=Ƨ(true);break;}case π.ΖΛ:{ǎ="Unloading";ѓ=ΕБ();break;}case π.Γπ:{ѓ=true;break;}
case π.Γκ:{ǎ="Charging batteries";ѓ=ț>=100f;break;}case π.ΑϏ:{ǎ="Discharging batteries";ѓ=ț<=25f;break;}case π.Αώ:{ǎ="Discharging batteries";ѓ=ț<=0f;break;}case π.Γν:{ǎ="Filling up hydrogen";ѓ=Ĉ>=100f;break;}case π.Γξ:{ǎ="Unloading hydrogen";ѓ=Ĉ<=25f;break;}
case π.Γμ:{ǎ="Unloading hydrogen";ѓ=Ĉ<=0f;break;}case π.Γο:{bool ΗΞ=Θώ();if(!ΗΞ) ȝ.ΔЏ=true;ѓ=ȝ.ΔЏ&&ΗΞ;ǎ="Waiting for passengers";break;}}if(!ѓ) ȝ.ΓΒ(true);if(ѓ&&ȝ.ΓΨ()){ѓ=ȝ.ΓΒ(false);ǎ="Undocking in: "+Απ((int) Math.Max(0,ȝ.Ǫ-ȝ.Ħ));}if(Βχ) ΘΕ=ǎ;return ѓ;}void ΘΙ()
{if(!Ž) return;if(ω.Count<=1) return;float ΗЎ=0,ΗЍ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȍ=ω[i];if(Ʀ(ȍ,true)) continue;ΗЎ+=(float) ȍ.GetInventory(0).MaxVolume;ΗЍ+=(float) ȍ.GetInventory(0).CurrentVolume;}float Ζκ=(float) Math.Round(И(ΗЍ,ΗЎ),5);for(int œ=0;œ<Math.Max(1,Math.Floor(ω.Count/10f));œ++)
{float Δμ=0,Δό=0;float ΗΛ=0,Ηα=0;IMyTerminalBlock ą=null,ħ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock ȍ=ω[i];if(Ʀ(ȍ,true)) continue;float ΔЃ=(float) ȍ.GetInventory(0).MaxVolume;float ΒΤ=И((float) ȍ.GetInventory(0).CurrentVolume,ΔЃ);if(ą==null||ΒΤ>Δμ){ą=ȍ;Δμ=ΒΤ;ΗΛ=ΔЃ;}
if(ħ==null||ΒΤ<Δό){ħ=ȍ;Δό=ΒΤ;Ηα=ΔЃ;}}if(ą==null||ħ==null||ą==ħ) return;if(checkConveyorSystem&&!ą.GetInventory(0).IsConnectedTo(ħ.GetInventory(0))){if(ΔΑ>20) Λ="Inventory balancing failed";else ΔΑ++;return;}ΔΑ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
ą.GetInventory(0).GetItems(Ћ);float Αί=0;if(Ћ.Count==0) continue;MyInventoryItem ΒΥ=Ћ[0];String ΗΣ=ΒΥ.Type.TypeId+ΒΥ.Type.SubtypeId;if(!ΘΔ.TryGetValue(ΗΣ,out Αί)){if(ΘЃ(ą.GetInventory(0),0,ħ.GetInventory(0),out Αί))
ΘΔ.Add(ΗΣ,Αί);else return;}float Κϋ=((Δμ-Ζκ)*ΗΛ/Αί);float Κό=((Ζκ-Δό)*Ηα/Αί);int Εά=(int) Math.Min(Κό,Κϋ);if(Εά<=0) return;if((float) ΒΥ.Amount<Εά)
ą.GetInventory(0).TransferItemTo(ħ.GetInventory(0),0,null,null,ΒΥ.Amount);else
ą.GetInventory(0).TransferItemTo(ħ.GetInventory(0),0,null,null,Εά);}}bool ΘЃ(IMyInventory ΑΣ,int Α,IMyInventory Ιό,out float ΘΓ){ΘΓ=0;float Ιν=(float) ΑΣ.CurrentVolume;List<MyInventoryItem>ΔΓ=new List<MyInventoryItem>();ΑΣ.GetItems(ΔΓ);float Ζξ=0;for(int i=0;i<ΔΓ.Count;i++) Ζξ+=(float) ΔΓ[i].Amount;
ΑΣ.TransferItemTo(Ιό,Α,null,null,1);float ΘΑ=Ιν-(float) ΑΣ.CurrentVolume;List<MyInventoryItem>ΓЉ=new List<MyInventoryItem>();ΑΣ.GetItems(ΓЉ);float Ζγ=0;for(int i=0;i<ΓЉ.Count;i++) Ζγ+=(float) ΓЉ[i].Amount;if(ΘΑ==0f||!Γά(0.9999,Ζξ-Ζγ,1.0001))
return false;ΘΓ=ΘΑ;return true;}void ΙΨ(){Vector3 Ǉ=Ѓ+Į;Vector3 Ȍ=Ǉ-ε;if(Ȍ.Length()==0) Ȍ=new Vector3(0,0,-1);Vector3 Βε=т(Ω,Ȍ);Vector3 ΗЊ=Vector3.Normalize(Βε);Vector3 ƀ=т(Ω,Ω.GetNaturalGravity());float Κλ=ΔЌ>0?Math.Max(0,1-š(Ȍ,ΗΓ)/5):0;float Ч=(float) Math.Min((ΔЀ>0?ΔЀ:1000f),Math.Max(Αη(-Βε,ƀ,null),ΔЌ*Κλ));
if(!ΗЌ) Ч=0;if(Ηϋ) Ч=Math.Max(0,1-ΒΗ/ΑЍ)*Ч;if(generalSpeedLimit>0) Ч=Math.Min(generalSpeedLimit,Ч);if(Ɖ) Ч=0;if(Ȑ>=0) Ч=Math.Min(Ч,Ȑ);if(Ηό) Ч*=(float) Math.Min(1,И(Ȍ.Length(),wpReachedDist)/2);Vector3 Ηή=т(Ω,Ω.GetShipVelocities().LinearVelocity);float Ιθ=(float)(Math.Max(0,15-š(-ΗЊ,-Ηή))/15)*0.85f+0.15f;
ΕΡ+=Math.Sign(Ιθ-ΕΡ)/10f;Vector3 Ģ=ΗЊ*Ч*ΕΡ-(Ηή);Vector3 ΓА=ΓΥ(Ģ,null);if(Βώ&&б>0.1f){Ģ.X*=Γώ(Ģ.X,ref ΒϏ.X,1f,ΓА.X,20);Ģ.Y*=Γώ(Ģ.Y,ref ΒϏ.Y,1f,ΓА.Y,20);Ģ.Z*=Γώ(Ģ.Z,ref ΒϏ.Z,1f,ΓА.Z,20);}else ΒϏ=new Vector3(1,1,1);ΑΩ=Ř*Ģ-ƀ*Ř;Ει(ΑΩ,Βώ);б=Vector3.Distance(ε,Ǉ);
}float Γώ(float value,ref float Ђ,float ǝ,float Č,float ΔЁ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ђ)) Ђ+=Math.Sign(Ђ)*ǝ;else Ђ=value;if(value==0) Ђ=1;float ή=Math.Abs(Ђ);if(ή<ΔЁ||Č==0) return ή;Ђ=Math.Min(ΔЁ,Math.Max(-ΔЁ,Ђ));ή=Math.Abs(Č);return ή;
}void Ζα(){float Ǎ=90,ǡ=90,Ǜ=90;float Ǵ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȃ,Ǘ,ΑΘ,Δω;if(Δϊ){ȃ=Vector3.Normalize(Ѓ-ε);ΑΘ=т(Ω,ȃ);Ǘ=т(Ω,ΕΟ);Ǎ=š(ΑΘ,new Vector3(0,-1,0))-90;ǡ=ĳ(Ǘ,new Vector3(-1,0,0),Ǘ.Y);
Ǜ=ĳ(ΑΘ,new Vector3(-1,0,0),ΑΘ.Z);}else{ȃ=ΕΠ;Ǘ=т(Ω,ΕΟ);ΑΘ=т(Ω,ΕΠ);Δω=т(Ω,ΗЋ);Ǎ=ĳ(Ǘ,new Vector3(0,0,1),Ǘ.Y);ǡ=ĳ(Ǘ,new Vector3(-1,0,0),Ǘ.Y);Ǜ=ĳ(Δω,new Vector3(0,0,1),Δω.X);}if(Ζε&&Αφ()){Vector3 Δγ=Ω.GetNaturalGravity();Ǘ=т(Ω,Δγ);Ǎ=ĳ(Ǘ,new Vector3(0,0,1),Ǘ.Y);
ǡ=ĳ(Ǘ,new Vector3(-1,0,0),Ǘ.Y);}if(!Γά(-45,ǡ,45)){Ǎ=0;Ǜ=0;}if(!Γά(-45,Ǜ,45)) Ǎ=0;ΑЅ(Δε,1,(-Ǎ)*Ǵ,(-Ǜ)*Ǵ,(-ǡ)*Ǵ);ΒΗ=Math.Max(Math.Abs(Ǎ),Math.Max(Math.Abs(ǡ),Math.Abs(Ǜ)));Βά=ΒΗ<=ΑЍ;}void ΘΡ(IMyTerminalBlock Βπ){if(Βπ==null) return;Ƿ=new float[3,2];Ǟ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȫ.Count;i++){IMyThrust ч=Ȫ[i];if(!ч.IsFunctional) continue;Vector3 ρ=т(Βπ,ч.WorldMatrix.Backward);float ĥ=ч.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ƿ[0,0]+=ĥ;else Ƿ[0,1]-=ĥ;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ƿ[1,0]+=ĥ;else Ƿ[1,1]-=ĥ;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ƿ[2,0]+=ĥ;else Ƿ[2,1]-=ĥ;String Ǐ=ΕЇ(ч);float[,]ǟ=null;if(Ǟ.ContainsKey(Ǐ)) ǟ=Ǟ[Ǐ];else{ǟ=new float[3,2];Ǟ.Add(Ǐ,ǟ);}float Č=ч.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ǟ[0,0]+=Č;else ǟ[0,1]-=Č;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ǟ[1,0]+=Č;else ǟ[1,1]-=Č;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ǟ[2,0]+=Č;else ǟ[2,1]-=Č;}}static String ΕЇ(IMyThrust ч){return ч.BlockDefinition.SubtypeId;}Vector3 Γβ(Vector3 ǔ,float[,]Ƃ){return new Vector3(
ǔ.X>=0?Ƃ[0,0]:Ƃ[0,1],
ǔ.Y>=0?Ƃ[1,0]:Ƃ[1,1],
ǔ.Z>=0?Ƃ[2,0]:Ƃ[2,1]);}bool ΕІ(Ϋ ά,String Ǐ,out float Δά){Δά=0;int Α=ȩ.IndexOf(Ǐ);if(Α==-1||ά.Ƕ==null||Α>=ά.Ƕ.Length)
return false;Δά=ά.Ƕ[Α];if(Δά==-1) return false;return true;}Vector3 ΓΥ(Vector3 ǔ,Ϋ ά){if(ά!=null){Vector3 ή=new Vector3();for(int i=0;i<Ǟ.Keys.Count;i++){String Ǐ=Ǟ.Keys.ElementAt(i);float Ǆ=0;if(!ΕІ(ά,Ǐ,out Ǆ))
return Γβ(ǔ,Ƿ);ή+=Γβ(ǔ,Ǟ.Values.ElementAt(i))*Ǆ;}return ή;}return Γβ(ǔ,Ƿ);}float Ƥ(Vector3 ǔ,Ϋ ά){return Ƥ(ǔ,new Vector3(),ά);}float Ƥ(Vector3 ǔ,Vector3 φ,Ϋ ά){Vector3 Κσ=ΓΥ(ǔ,ά);Vector3 ĥ=Κσ+φ*Ř;float ŕ=(ĥ/ǔ).AbsMin();return(float)(ǔ*ŕ).Length();}float Αη(Vector3 Βγ,Vector3 ƀ,Ϋ ά)
{if(Βγ.Length()==0) return 0;float Ζδ=1;if(ƀ.Length()>0) Ζδ=Math.Min(1,š(-ƀ,Βγ)/90)*0.4f+0.6f;float ΑΩ=Ƥ(Βγ,ƀ,ά);if(ΑΩ==0) return 0.1f;float ΑЌ=И(ΑΩ,Ř);float ΕΥ=(float) Math.Sqrt(И(Βγ.Length(),ΑЌ*0.5f));return ΑЌ*ΕΥ*Ζδ*ǻ;}float Ασ(Vector3 ȃ,Vector3 Б,Vector3 φ,Ϋ ά)
{if(φ.Length()==0f) return 0;Vector3 Βδ=Ļ(ȃ,Б,Vector3.Normalize(φ));float Ǭ=Ƥ(-Βδ,ά);return Ǭ/φ.Length();}void Εξ(bool ΙЏ){float Ǡ=0;float Δΰ=0.9f;if(ΙЏ){г=-1;Ŝ=0;Βω=null;if(β!=Ρ.ы&&Ζ.φ.Length()!=0){Ǡ=Δΰ*Ασ(Ζ.θ,Ζ.η*-1,Ζ.φ,null);if(Ǡ<г||г==-1) г=Ǡ;}if(Ο.ι&&Ο.φ.Length()!=0)
{Ǡ=Δΰ*Ασ(Ο.θ,Ο.η*-1,Ο.φ,null);if(Ǡ<г||г==-1) г=Ǡ;}return;}if(Ŝ==-1) return;if(Ŝ>=0){int Ητ=0;while(Ŝ<Φ.Count){if(Ητ>100) return;Ητ++;Ϋ ά=Φ[Ŝ];if(ά.φ.Length()!=0f){Ǡ=Δΰ*Math.Min(
Ασ(ά.θ,ά.η*-1,ά.φ,ά),
Ασ(ά.θ*-1,ά.η*-1,ά.φ,ά));if(Ǡ<г||г==-1) г=Ǡ;}else Βω=ά;Ŝ++;}Ŝ=-1;}bool Δθ=true;float ī=0;if(Βω!=null){for(int i=0;i<Ǟ.Count;i++){String Ǐ=Ǟ.Keys.ElementAt(i);float[,]ǟ=Ǟ.Values.ElementAt(i);float Ǆ=0;if(!ΕІ(Βω,Ǐ,out Ǆ)){Δθ=false;break;}for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float ľ=Math.Abs(ǟ[a,b]*Ǆ);if(ľ==0) continue;Δθ=true;if(ī==0||ľ<ī) ī=ľ;}}}if(!Δθ){for(int a=0;a<Ƿ.GetLength(0);a++)
for(int b=0;b<Ƿ.GetLength(1);b++){float ľ=Math.Abs(Ƿ[a,b]);if(ľ==0) continue;if(ī==0||ľ<ī) ī=ľ;}}if(ī>0){float ΚΚ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǡ=И(ī,ΚΚ);if(Ǡ>0)
if(Ǡ<г||г==-1) г=Ǡ;}}void Ει(Vector3 Ǭ,bool М){if(!М){for(int i=0;i<Ȫ.Count;i++)
Ȫ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓЏ=ΓΥ(Ǭ,null);float ΚЋ=Math.Min(1,Math.Abs(И(Ǭ.X,ΓЏ.X)));float ΚЌ=Math.Min(1,Math.Abs(И(Ǭ.Y,ΓЏ.Y)));float ΚЍ=Math.Min(1,Math.Abs(И(Ǭ.Z,ΓЏ.Z)));for(int i=0;i<Ȫ.Count;i++){IMyThrust ч=Ȫ[i];Vector3 ρ=ΘЎ(т(Ω,ч.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǭ.X))
ч.SetValueFloat("Override",ч.MaxThrust*ΚЋ);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǭ.Y))
ч.SetValueFloat("Override",ч.MaxThrust*ΚЌ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǭ.Z))
ч.SetValueFloat("Override",ч.MaxThrust*ΚЍ);else
ч.SetValueFloat("Override",0.0f);}}void ΑЅ(bool М,float ΚΨ,float Ǎ,float Ǜ,float ǡ){for(int i=0;i<ΒΧ.Count;i++){IMyGyro Й=ΒΧ[i];Й.GyroOverride=М;if(!М) Й.GyroPower=100;else Й.GyroPower=ΚΨ;if(!М) continue;Vector3 Ы=Ω.WorldMatrix.Forward;Vector3 о=Ω.WorldMatrix.Right;Vector3 Б=Ω.WorldMatrix.Up;
Vector3 Ÿ=Й.WorldMatrix.Forward;Vector3 ź=Й.WorldMatrix.Up;Vector3 Ź=Й.WorldMatrix.Left*-1;if(Ÿ==Ы) Й.SetValueFloat("Roll",ǡ);else if(Ÿ==(Ы*-1)) Й.SetValueFloat("Roll",ǡ*-1);else if(ź==(Ы*-1)) Й.SetValueFloat("Yaw",ǡ);else if(ź==Ы) Й.SetValueFloat("Yaw",ǡ*-1);else if(Ź==Ы) Й.SetValueFloat("Pitch",ǡ);
else if(Ź==(Ы*-1)) Й.SetValueFloat("Pitch",ǡ*-1);if(Ź==(о*-1)) Й.SetValueFloat("Pitch",Ǎ);else if(Ź==о) Й.SetValueFloat("Pitch",Ǎ*-1);else if(ź==о) Й.SetValueFloat("Yaw",Ǎ);else if(ź==(о*-1)) Й.SetValueFloat("Yaw",Ǎ*-1);else if(Ÿ==(о*-1)) Й.SetValueFloat("Roll",Ǎ);
else if(Ÿ==о) Й.SetValueFloat("Roll",Ǎ*-1);if(ź==(Б*-1)) Й.SetValueFloat("Yaw",Ǜ);else if(ź==Б) Й.SetValueFloat("Yaw",Ǜ*-1);else if(Ź==Б) Й.SetValueFloat("Pitch",Ǜ);else if(Ź==(Б*-1)) Й.SetValueFloat("Pitch",Ǜ*-1);else if(Ÿ==Б) Й.SetValueFloat("Roll",Ǜ);else if(Ÿ==(Б*-1)) Й.SetValueFloat("Roll",Ǜ*-1);
}}void Ζέ(){this.Δε=false;}void и(Vector3 Ĥ,Vector3 Ы,Vector3 Δχ,float ē,bool Αή){ΒЋ(Ĥ,ē,Αή);ΑЍ=ē;Δϊ=false;this.ΕΠ=Ы;this.ΗЋ=Δχ;}void и(Vector3 Ĥ,Vector3 Ы,Vector3 Δχ,bool Αή){и(Ĥ,Ы,Δχ,2f,Αή);}void ΒЋ(Vector3 Ĥ,float ē,bool Αή){ΑЍ=ē;this.Δε=true;this.Ζε=Αή;Δϊ=true;
Βά=false;this.ΕΟ=Ĥ;}void ΓЃ(){ǹ(false,false,false,Ѓ,0);Βώ=false;}void е(Vector3 ζ,float Ǵ){ǹ(true,false,false,ζ,Ǵ);}void ǹ(bool М,bool ΕΚ,bool Εί,Vector3 ζ,float Ǵ){ǹ(М,ΕΚ,Εί,ζ,ζ-ε,0.0f,Ǵ);}void ǹ(bool М,bool ΕΚ,bool Εί,Vector3 ζ,Vector3 Κε,float Κζ,float Ǵ){Βώ=true;
this.ΗЌ=М;Ѓ=ζ;this.ΔЀ=Ǵ;this.ΔЌ=Κζ;this.Ηό=ΕΚ;this.Ηϋ=Εί;this.ΗΓ=Κε;б=Vector3.Distance(ζ,ε);}void ΘА(){χ.GetBlocksOfType(ƴ,Ǒ);}void ΘБ(){if(Βς){А=2;return;}List<IMyRemoteControl>ΑΞ=new List<IMyRemoteControl>();List<IMySensorBlock>Ηω=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑΧ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(ΑΞ,Ǒ);χ.GetBlocksOfType(Ц,Ǒ);χ.GetBlocksOfType(Ηω,Ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΧ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΥ();ΖΦ(ΑΧ);ĺ(Ц,setLCDFontAndSize,1.4f,false);ĺ(ȋ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΗІ=Ǣ(Ηω,pamTag,true);if(ΗІ.Count>0) ψ=ΗІ[0];else ψ=null;if(Κ==Ι.ƪ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Κ==Ι.ǣ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Κ==Ι.έ){χ.GetBlocksOfType(ΕΦ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΗЅ=Ǣ(ΑΞ,pamTag,true);if(ΗЅ.Count>0) ΑΞ=ΗЅ;if(ΑΞ.Count>0) Ω=ΑΞ[0];else{Ω=null;А=2;Λ="Remote is missing";return;}ο=(float) Ω.CubeGrid.WorldVolume.Radius*2;ǚ=null;if(Κ!=Ι.έ){Εο();if(ω.Count>0&&ǚ!=null){if(ψ!=null&&(ǚ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ω.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(ǚ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
ǚ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void ΙΑ(){χ.GetBlocksOfType(Ȧ,Ǒ);χ.GetBlocksOfType(ј,Ǒ);χ.GetBlocksOfType(Ȫ,Ǒ);χ.GetBlocksOfType(ΒΧ,Ǒ);χ.GetBlocksOfType(Ġ,Ǒ);χ.GetBlocksOfType(Ĭ,Ǒ);χ.GetBlocksOfType(Ǖ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Βψ,Ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ј=Ǣ(ј,"ConnectorMedium",false);else
ј=Ǣ(ј,"Connector",false);List<IMyShipConnector>ΗЃ=Ǣ(ј,pamTag,true);if(ΗЃ.Count>0) ј=ΗЃ;if(А<=1){if(ј.Count==0){А=1;Λ="Connector is missing";}if(ΒΧ.Count==0){А=1;Λ="Gyros are missing";}if(Ȫ.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΗЇ=Ǣ(Βψ,pamTag,true);
if(ΗЇ.Count>0) Βψ=ΗЇ;List<IMyLandingGear>ΗЄ=Ǣ(Ȧ,pamTag,true);if(ΗЄ.Count>0) Ȧ=ΗЄ;for(int i=0;i<Ȧ.Count;i++) Ȧ[i].AutoLock=false;List<IMyBatteryBlock>ΗЂ=Ǣ(Ġ,pamTag,true);if(ΗЂ.Count>0) Ġ=ΗЂ;List<IMyGasTank>ΗЈ=Ǣ(Ǖ,pamTag,true);if(ΗЈ.Count>0) Ǖ=ΗЈ;}void ΙΒ(){χ.GetBlocksOfType(Ŀ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ŵ.Clear();for(int i=Ŀ.Count-1;i>=0;i--){if(Θό(Ŀ[i])){Ŵ.Add(Ŀ[i]);Ŀ.RemoveAt(i);}}}bool Θό(IMyTerminalBlock Ψ){if(Ψ is IMyCargoContainer) return true;if(Ψ is IMyShipDrill) return true;if(Ψ is IMyShipGrinder) return true;if(Ψ is IMyShipConnector){if(((IMyShipConnector) Ψ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓΩ(Ψ,"ConnectorSmall",false))
return false;else return true;}return false;}void Εο(){if(Ω==null) return;ǚ=null;float Βλ=0,Βζ=0,Βμ=0,Βη=0,Βν=0,Βθ=0;List<IMyTerminalBlock>ΕΞ=Ǣ(ω,pamTag,true);bool ΚΠ=ΕΞ.Count==0;if(ΕΞ.Count>0) ǚ=ΕΞ[0];else if(ω.Count>0) ǚ=ω[0];int Ƞ=0;float ƌ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward){if(ΚΠ){А=2;Λ="Mining direction is unclear!";return;}continue;}Ƞ++;ƌ=Math.Max(ƌ,ΙΠ(ω[i]));Vector3 Н=Ας(Ω,ω[i].GetPosition());if(i==0){Βλ=Н.X;Βζ=Н.X;Βμ=Н.Y;Βη=Н.Y;Βν=Н.Z;Βθ=Н.Z;}Βζ=Math.Max(Н.X,Βζ);Βλ=Math.Min(Н.X,Βλ);
Βη=Math.Max(Н.Y,Βη);Βμ=Math.Min(Н.Y,Βμ);Βθ=Math.Max(Н.Z,Βθ);Βν=Math.Min(Н.Z,Βν);}if(ƌ==0) ƌ=drillRadius;bool Ĥ=ǚ!=null&&ǚ.WorldMatrix.Forward==Ω.WorldMatrix.Down;ȏ=(Βζ-Βλ)*(1-ŝ)+ƌ*2;Ȏ=(Ĥ?Βθ-Βν:Βη-Βμ)*(1-ň)+ƌ*2;IMyCubeGrid g=Ω.CubeGrid;float Εγ=0,Εδ=0,Εε=0,Εζ=0;
for(int c=0;c<8;c++){Vector3 p=Ας(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ĥ?p.Z:p.Y;if(c==0||p.X<Εγ) Εγ=p.X;if(c==0||p.X>Εδ) Εδ=p.X;if(c==0||y<Εε) Εε=y;if(c==0||y>Εζ) Εζ=y;}Βυ=Εδ-Εγ+g.GridSize;Βτ=Εζ-Εε+g.GridSize;}float ΙΠ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǣ<T>(List<T>Ѝ,String ǵ,bool ΒΨ){List<T>ή=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(ΓΩ(Ѝ[i],ǵ,ΒΨ)) ή.Add(Ѝ[i]);return ή;}bool ΓΩ<T>(T Ψ,String ǵ,bool ΒΨ){IMyTerminalBlock b=(IMyTerminalBlock) Ψ;if(ΒΨ&&b.CustomName.ToUpper().Contains(ǵ.ToUpper())) return true;if(!ΒΨ&&b.BlockDefinition.ToString().ToUpper().Contains(ǵ.ToUpper())) return true;return false;}void Ǔ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
ΖΣ((IMyTerminalBlock) Ѝ[i],М);}void ΖΣ(IMyTerminalBlock Ψ,bool М){if(Ψ==null) return;String Ιζ=М?"OnOff_On":"OnOff_Off";var a=Ψ.GetActionWithName(Ιζ);a.Apply(Ψ);}void Ǻ(bool М){ΕΛ=М;if(!ř) return;Ǔ(Βψ,М);}void Γϊ(ChargeMode Џ){for(int i=0;i<Ġ.Count;i++) Ġ[i].ChargeMode=Џ;}void Γό(bool Κν)
{for(int i=0;i<Ǖ.Count;i++) Ǖ[i].Stockpile=Κν;}void Ʈ(List<IMyLandingGear>Δα,bool Ηί){for(int i=0;i<Δα.Count;i++){if(Ηί) Δα[i].Lock();if(!Ηί) Δα[i].Unlock();}}void ΖΤ(bool М){if(ΖЄ==М) return;List<IMyShipController>ΒΕ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΕ,Ǒ);if(ΒΕ.Count==0) return;
for(int i=0;i<ΒΕ.Count;i++)
ΒΕ[i].DampenersOverride=М;ΖЄ=М;}IMyShipConnector Я(MyShipConnectorStatus ǎ){for(int i=0;i<ј.Count;i++){if(!ȕ(ј[i],true)) continue;if(ј[i].Status==ǎ) return ј[i];}return null;}void ļ(bool ΕΖ,bool ΒΛ,float ΙІ,float Ιμ){if(ψ==null||ω.Count==0) return;Vector3 ΒΑ=new Vector3();
int Ƞ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǚ.WorldMatrix.Forward) continue;Ƞ++;ΒΑ+=ω[i].GetPosition();}ΒΑ=ΒΑ/Ƞ;Vector3 Ɔ=Ας(ψ,ΒΑ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΖ;ψ.LeftExtend=(ΒΛ?1:я)/2f*ȏ-Ɔ.X;ψ.RightExtend=(ΒΛ?1:я)/2f*ȏ+Ɔ.X;ψ.TopExtend=(ΒΛ?1:ю)/2f*Ȏ+Ɔ.Y;
ψ.BottomExtend=(ΒΛ?1:ю)/2f*Ȏ-Ɔ.Y;ψ.FrontExtend=(ΕΖ?к:ΙІ)-Ɔ.Z;ψ.BackExtend=ΕΖ?0:Ιμ+ο*0.75f+Ɔ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void ĺ<T>(List<T>Ѝ,bool Κθ,float ΙЄ,bool ΙА){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface ΑΦ=null;if(Ѝ[i]is IMyTextSurface) ΑΦ=(IMyTextSurface) Ѝ[i];if(ΑΦ!=null){ΑΦ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Κθ) continue;ΑΦ.Font="Debug";if(ΙА) continue;ΑΦ.FontSize=ΙЄ;}}}bool Αφ(){Vector3D ξ;return this.Ω.TryGetPlanetPosition(out ξ);}int Θπ(Vector3 Κο,int Ηύ){if(Ηύ==-1) return 0;double Щ=-1;int Ă=-1;for(int i=Φ.Count-1;i>=0;i--){double Њ=Vector3.Distance(Φ[i].Ε,Κο);if(Щ==-1||Њ<Щ){Ă=i;Щ=Њ;}
}return Math.Sign(Ă-Ηύ);}bool Θύ(Vector3 ξ){List<Vector3>Ю=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Ю.Add(Φ[i].Ε);if(Ο.ι&&Φ.Count>=1){Vector3 ů=new Vector3();Ȕ(Ο,dockDist*ο,false,out ů);if(Vector3.Distance(Ο.Ε,Φ.First().Ε)<Vector3.Distance(Ο.Ε,Φ.Last().Ε)){Ю.Insert(0,ů);Ю.Insert(0,Ο.Ε);}
else{Ю.Add(ů);Ю.Add(Ο.Ε);}}if(Κ==Ι.έ){if(Ζ.ι&&Φ.Count>=1){Vector3 ΓЋ=new Vector3();Ȕ(Ζ,dockDist*ο,false,out ΓЋ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Ю.Insert(0,ΓЋ);Ю.Insert(0,Ζ.Ε);}else{Ю.Add(ΓЋ);Ю.Add(Ζ.Ε);}}}else{if(β!=Ρ.ы)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Ю.Insert(0,Ζ.Ε);else Ю.Add(Ζ.Ε);}int Ă=-1;double ǿ=-1;for(int i=0;i<Ю.Count;i++){double Њ=Vector3.Distance(Ю[i],ξ);if(Њ<ǿ||ǿ==-1){ǿ=Њ;Ă=i;}}if(Ю.Count==0) return false;double ΔΦ=Vector3.Distance(Ю[Ă],ξ);double ΚΩ=Vector3.Distance(Ю[Math.Max(0,Ă-1)],Ю[Ă])*1.5f;
double ΚΟ=Vector3.Distance(Ю[Math.Min(Ю.Count-1,Ă+1)],Ю[Ă])*1.5f;return ΔΦ<ΚΩ||ΔΦ<ΚΟ||ΔΦ<laneOffset*ο*1.5f;}static float И(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΘЎ(Vector3 v,int ΔΣ){return new Vector3(Math.Round(v.X,ΔΣ),Math.Round(v.Y,ΔΣ),Math.Round(v.Z,ΔΣ));
}float š(Vector3 a,Vector3 b){if(a==b) return 0;float ΙЁ=(a*b).Sum;float ΗΪ=a.Length();float ΗΫ=b.Length();if(ΗΪ==0||ΗΫ==0) return 0;float ή=(float)((180.0f/Math.PI)*Math.Acos(ΙЁ/(ΗΪ*ΗΫ)));if(float.IsNaN(ή)) return 0;return ή;}float ĳ(Vector3 v,Vector3 Βπ,float Κι){float ŭ=š(v,Βπ);
if(Κι>0f) ŭ*=-1;if(ŭ>-90f) return ŭ-90f;else return 180f-(-ŭ-90f);}double Εφ(float Ιϊ){return(Math.PI/180)*Ιϊ;}bool Γά(double ΚΙ,double value,double ƃ){return(value>=ΚΙ&&value<=ƃ);}Vector3 ΙΡ(IMyTerminalBlock Ψ,Vector3 Н){return Vector3D.Transform(Н,Ψ.WorldMatrix);
}Vector3 Ας(IMyTerminalBlock Ψ,Vector3 ΚЈ){return т(Ψ,ΚЈ-Ψ.GetPosition());}Vector3 т(IMyTerminalBlock Ψ,Vector3 Εβ){return Vector3D.TransformNormal(Εβ,MatrixD.Transpose(Ψ.WorldMatrix));}Vector3 Ļ(Vector3 ȃ,Vector3 Б,Vector3 Εβ){MatrixD ΔϏ=MatrixD.CreateFromDir(ȃ,Б);return Vector3D.TransformNormal(Εβ,MatrixD.Transpose(ΔϏ));
}Vector3 ΓЅ(Vector3 ȃ,Vector3 Б,Vector3 Н){MatrixD ΔϏ=MatrixD.CreateFromDir(ȃ,Б);return Vector3D.Transform(Н,ΔϏ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Π(τ,0)),float.Parse(Π(τ,1)),float.Parse(Π(τ,2)));
}String Ιδ(Ϋ ά){String П=":";String ή=σ(ά.Ε)+П+σ(ά.θ)+П+
σ(ά.η)+П+σ(ά.Ё)+П+σ(ά.φ);for(int i=0;i<ά.Ƕ.Length;i++)
ή+=П+Math.Round(ά.Ƕ[i],3);return ή;}Ϋ ΙΜ(String s){String[]τ=s.Split(':');Ϋ ά=new Ϋ(
ς(Π(τ,0)),ς(Π(τ,1)),
ς(Π(τ,2)),ς(Π(τ,3)),
ς(Π(τ,4)));int Α=5;List<float>ΖБ=new List<float>();while(Α<τ.Length){String ΒЅ=Π(τ,Α);float f=0;if(!float.TryParse(ΒЅ,out f)) break;ΖБ.Add(f);Α++;}ά.Ƕ=ΖБ.ToArray();return ά;}System.Text.StringBuilder Ɛ=new System.Text.StringBuilder();void Υ<T>(T value,bool ΚΞ){if(ΚΞ) Ɛ.Append('\n');Ɛ.Append(value);}
void Υ<T>(T value){Υ(value,true);}String Αλ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑЉ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Π(String[]Ικ,int Α){String ΒЅ=Ικ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ΒЅ)) return "";
return ΒЅ;}void Save(){if(Βς||Κ==Ι.і){Storage="";return;}Storage=ΒБ();}String ΒБ(){Ɛ.Clear();Ɛ.Append(DATAREV+";");Υ(σ(Βρ),false);Υ(σ(Ο.θ));Υ(σ(Ο.Ё));Υ(σ(Ο.η));Υ(σ(Ο.φ));Υ(σ(Ο.Ε));Υ(σ(Ο.л));Υ(Ο.ι);Υ(σ(Ζ.Ε));Υ(σ(Ζ.φ));Υ(σ(Ζ.θ));Υ(σ(Ζ.η));Υ(σ(Ζ.Ё));Υ(σ(Ζ.л));Υ(Ζ.ι);Υ(σ(Ǳ));Υ(σ(č));Υ(";");
Υ((int) Κ,false);Υ((int) β);Υ((int) ќ);Υ(ȅ);Υ(ѝ);Υ(н);Υ(ў);Υ(ǻ);Υ(ĕ);Υ(Ĕ);if(Κ==Ι.έ){Υ((int) І.Р);Υ(І.Ǫ);Υ(І.Ħ);Υ(Αλ(І.ǫ));Υ(Αλ(І.Ǯ));Υ((int) Ї.Р);Υ(Ї.Ǫ);Υ(Ї.Ħ);Υ(Αλ(Ї.ǫ));Υ(Αλ(Ї.Ǯ));}else{Υ((int) ȉ);Υ((int) ё);Υ((int) в);Υ((int) љ);Υ((int) Ō);Υ(į);Υ(ř);Υ(Ž);Υ(ŷ);Υ(я);Υ(ю);Υ(к);Υ(Ɵ);Υ(ƞ);
Υ(ŝ);Υ(ň);Υ(Ќ);Υ(Ь);Υ(Ϗ);Υ(ό);Υ(ċ);Υ(а);Υ(Ŗ);Υ(ŏ);}Υ(";");for(int i=0;i<ȩ.Count;i++)
Υ((i>0?"|":"")+ȩ[i],false);Υ(";");for(int i=0;i<Φ.Count;i++)
Υ(Ιδ(Φ[i]),i>0);Υ(";",false);Υ(м+"|"+Г,false);Υ(";",false);Υ(Θα(),false);String Ξ=Ɛ.ToString();Ɛ.Clear();return Ξ;}ї ΘЍ(){return ΑЁ(Storage);}ї ΑЁ(String Ξ){if(Ξ=="") return ї.ΘЅ;String[]ŗ=Ξ.Split(';');if(Π(ŗ,0)!=DATAREV) return ї.Γθ;int Α=0;try{String[]Ν=Π(ŗ,1).Split('\n');
Βρ=ς(Π(Ν,Α++));Ο.θ=ς(Π(Ν,Α++));Ο.Ё=ς(Π(Ν,Α++));Ο.η=ς(Π(Ν,Α++));Ο.φ=ς(Π(Ν,Α++));Ο.Ε=ς(Π(Ν,Α++));Ο.л=ς(Π(Ν,Α++));Ο.ι=bool.Parse(Π(Ν,Α++));Ζ.Ε=ς(Π(Ν,Α++));Ζ.φ=ς(Π(Ν,Α++));Ζ.θ=ς(Π(Ν,Α++));Ζ.η=ς(Π(Ν,Α++));Ζ.Ё=ς(Π(Ν,Α++));Ζ.л=ς(Π(Ν,Α++));Ζ.ι=bool.Parse(Π(Ν,Α++));
Ǳ=ς(Π(Ν,Α++));č=ς(Π(Ν,Α++));Ν=Π(ŗ,2).Split('\n');Α=0;Κ=(Ι) int.Parse(Π(Ν,Α++));β=(Ρ) int.Parse(Π(Ν,Α++));ќ=(Ρ) int.Parse(Π(Ν,Α++));ȅ=int.Parse(Π(Ν,Α++));ѝ=int.Parse(Π(Ν,Α++));н=int.Parse(Π(Ν,Α++));ў=int.Parse(Π(Ν,Α++));ǻ=float.Parse(Π(Ν,Α++));ĕ=bool.Parse(Π(Ν,Α++));
Ĕ=bool.Parse(Π(Ν,Α++));if(Κ==Ι.έ){І.Р=(π) int.Parse(Π(Ν,Α++));І.Ǫ=float.Parse(Π(Ν,Α++));І.Ħ=float.Parse(Π(Ν,Α++));І.ǫ=ΑЉ(Π(Ν,Α++));І.Ǯ=ΑЉ(Π(Ν,Α++));Ї.Р=(π) int.Parse(Π(Ν,Α++));Ї.Ǫ=float.Parse(Π(Ν,Α++));Ї.Ħ=float.Parse(Π(Ν,Α++));Ї.ǫ=ΑЉ(Π(Ν,Α++));Ї.Ǯ=ΑЉ(Π(Ν,Α++));}else
{ȉ=(ь) int.Parse(Π(Ν,Α++));ё=(щ) int.Parse(Π(Ν,Α++));в=(В) int.Parse(Π(Ν,Α++));љ=(ǐ) int.Parse(Π(Ν,Α++));Ō=(ь) int.Parse(Π(Ν,Α++));į=bool.Parse(Π(Ν,Α++));ř=bool.Parse(Π(Ν,Α++));Ž=bool.Parse(Π(Ν,Α++));ŷ=bool.Parse(Π(Ν,Α++));я=int.Parse(Π(Ν,Α++));ю=int.Parse(Π(Ν,Α++));к=int.Parse(Π(Ν,Α++));
Ɵ=float.Parse(Π(Ν,Α++));ƞ=float.Parse(Π(Ν,Α++));ŝ=float.Parse(Π(Ν,Α++));ň=float.Parse(Π(Ν,Α++));Ќ=int.Parse(Π(Ν,Α++));Ь=int.Parse(Π(Ν,Α++));Ϗ=int.Parse(Π(Ν,Α++));ό=int.Parse(Π(Ν,Α++));ċ=int.Parse(Π(Ν,Α++));а=int.Parse(Π(Ν,Α++));Ŗ=int.Parse(Π(Ν,Α++));ŏ=float.Parse(Π(Ν,Α++));
}Ν=Π(ŗ,3).Replace("\n","").Split('|');ȩ=Ν.ToList();Ν=Π(ŗ,4).Split('\n');Φ.Clear();if(Ν.Count()>=1&&Ν[0]!="")
for(int i=0;i<Ν.Length;i++)
Φ.Add(ΙΜ(Π(Ν,i)));Ν=Π(ŗ,5).Split('|');м=Π(Ν,0);if(!int.TryParse(Π(Ν,1),out Г)) Г=-1;if(м.Length!=Ќ*Ь){м="";Г=-1;}Θΰ(Π(ŗ,6));}catch{return ї.Εω;}Ƒ=ќ;Ѐ();return ї.ΑЇ;}const int Εν=64000;IMyTerminalBlock Εϊ(){List<IMyTerminalBlock>Ų=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ų,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ų.Count>0?Ų[0]:null;}void Θε(){IMyTerminalBlock Ψ=Εϊ();if(Ψ==null){Λ="Export: name a block "+dataTag;return;}String Ξ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒБ();if(Ξ.Length>Εν){Λ="Export too large: "+Ξ.Length+" chars, max "+Εν;return;}Ψ.CustomData=Ξ;Λ="Exported "+Φ.Count+" waypoints";}void Θω(bool Δρ){IMyTerminalBlock Ψ=Εϊ();if(Ψ==null){Λ="Import: name a block "+dataTag;return;}
Θϊ(Ψ.CustomData,Δρ);}void Θϊ(String Ξ,bool Δρ){Ξ=Ξ.Replace("\r","");if(Ξ.StartsWith("//")){int Ημ=Ξ.IndexOf('\n');Ξ=Ημ<0?"":Ξ.Substring(Ημ+1);}Ξ=Ξ.Trim();if(Ξ==""){Λ="Import: no PAM data in "+dataTag;return;}Ѐ();String Ζμ=ΒБ();ї ή=ΑЁ(Ξ);if(ή==ї.ΑЇ&&Δρ){Ϋ Χ=new Ϋ(Ζ);
Vector3 ΚΜ=Ǳ,ΚΛ=č;int w=я,h=ю,d=к;ь ΑΤ=ȉ;ǐ ΔΤ=љ;float ΚΤ=ŝ,ΚΣ=ň;ΑЁ(Ζμ);Ζ=Χ;Ǳ=ΚΜ;č=ΚΛ;я=w;ю=h;к=d;ȉ=ΑΤ;љ=ΔΤ;ŝ=ΚΤ;ň=ΚΣ;β=Ρ.Ũ;Ʋ(true,true);β=Ρ.Ũ;ќ=Ρ.Ũ;Λ="Imported job, run CONT";return;}if(ή==ї.ΑЇ){ΑΛ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}ΑЁ(Ζμ);Λ=ή==ї.Γθ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖΥ(){Ń.Clear();for(int i=Ц.Count-1;i>=0;i--){String Ξ=Ц[i].CustomData.ToUpper();bool Δο=false;if(Ξ=="INSTRUCTIONS"){Δο=true;ΒΚ=true;}if(Ξ=="DEBUG") Δο=true;if(Δο){Ń.Add(Ц[i]);Ц.RemoveAt(i);}
}ĺ(Ń,false,1,false);}void ΖΦ(List<IMyTerminalBlock>Ų){ȋ.Clear();for(int i=0;i<Ų.Count;i++){IMyTerminalBlock Ψ=Ų[i];try{String Ησ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Ψ.CustomName.IndexOf(Ησ);int Ɩ=-1;if(Α<0||!int.TryParse(Ψ.CustomName.Substring(Α+Ησ.Length,1),out Ɩ)) continue;
if(Ɩ==-1) continue;Ɩ--;IMyTextSurfaceProvider Ηυ=(IMyTextSurfaceProvider) Ψ;if(Ɩ<Ηυ.SurfaceCount&&Ɩ>=0)
ȋ.Add(Ηυ.GetSurface(Ɩ));}catch{}}}String Βι="";void ΖΟ(){String Δώ="";String ǃ="";Βι=Εμ(false);Δώ+=Βι;ǃ+=Βι;ǃ+=Θν();for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Δώ);for(int i=0;i<ȋ.Count;i++) ȋ[i].WriteText(Δώ);Echo(ǃ);for(int i=0;i<Ń.Count;i++){IMyTextPanel х=Ń[i];
String Ξ=х.CustomData.ToUpper();if(Ξ=="DEBUG") х.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") х.WriteText(ΕЁ());}}string ΕЁ(){String Γ="";try{float Ηβ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ű*Ηβ)+" / "+(int)(Ƅ*Ηβ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ű*100f,1)+"% / "+Math.Round(Ƅ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ƹ,2)+"ms / "+Math.Round(ΔЂ,2)+"ms \n";}catch{}for(int i=0;i<Ď.Count;i++)
Γ+=""+Ď.Keys.ElementAt(i)+" = "+Math.Round(Ď.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ď.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Θφ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ĺ(Ц,setLCDFontAndSize,0.9f,false);ĺ(ȋ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Γ+Ċ.ToString());for(int i=0;i<ȋ.Count;i++) ȋ[i].WriteText(Γ+Ċ.ToString());}void Ș(){if(!ΒΚ) return;try{ΔА=Runtime.CurrentInstructionCount;}catch{}
}void ȓ(String ί){if(!ΒΚ) return;if(ΔА==0) return;try{float ΒЃ=(Runtime.CurrentInstructionCount-ΔА)/Runtime.MaxInstructionCount*100;if(!Ď.ContainsKey(ί))
Ď.Add(ί,new float[]{ΒЃ,ΒЃ});else{Ď[ί][0]=ΒЃ;Ď[ί][1]=Math.Max(ΒЃ,Ď[ί][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΘΚ(String Ȃ){if(!ų) return;String П=""+BROADCAST_SEP;String δ=VERSION+П;δ+=Ơ(Ɣ)+П;
δ+=(int) Κ+П;δ+=Ť(ε.X)+""+П;δ+=Ť(ε.Y)+П;δ+=Ť(ε.Z)+П;if(Κ!=Ι.έ)
δ+=ΓΡ(ќ)+(ќ==Ρ.З?" "+Math.Round(ΑΔ,1)+"%":"")+П;else δ+=ΓΡ(ќ)+П;δ+=Ơ(Λ)+П;δ+=Ў+""+П;δ+=Ơ(Βι)+П;δ+=Ȁ+П;δ+=Ȇ+П;for(int i=0;i<ǭ.Count;i++){if(ǭ[i].Ɓ!=Ш.Ŧ) continue;δ+=Ơ(ǭ[i].ί)+"/"+Ơ(ǭ[i].υ)+"/"+ǭ[i].Љ+П;}ΙΖ(δ,Ȃ);}String Ơ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΙΖ(String δ,String Ȃ){Γψ(δ,BROADCAST_SELF_ID,Ȃ);}void Γψ(String δ,String ζ,String Ȃ){try{if(!ų) return;δ=BROADCAST_TAG+BROADCAST_SEP+Εύ()+BROADCAST_SEP+ζ+BROADCAST_SEP+ΔΖ+BROADCAST_SEP+Ȃ+BROADCAST_SEP+δ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,δ);}catch(Exception e){Ċ=e;}
}String Εύ(){if(Κ!=Ι.і) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γα(String δ){return δ.StartsWith(BROADCAST_TAG);}bool д(ref String Ξ,out string ǆ,Char П){int Α=Ξ.IndexOf(П);ǆ="";if(Α<0) return false;ǆ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool ΖΞ(ref String δ,out String Ȉ,out String Ȃ)
{Ȉ="";Ȃ="";if(!ų) return false;String ǵ="";if(!д(ref δ,out ǵ,BROADCAST_SEP)||!Γα(ǵ)) return false;if(!д(ref δ,out Ȉ,BROADCAST_SEP)) return false;if(!д(ref δ,out ǵ,BROADCAST_SEP)||(ǵ!=Εύ()&&(ǵ!="*"&&Κ!=Ι.і))) return false;if(!д(ref δ,out ǵ,BROADCAST_SEP)||(ǵ!=ΔΖ)) return false;
if(!д(ref δ,out Ȃ,BROADCAST_SEP)) return false;return true;}void ΕЎ(){bool Ƽ=true;if(ΗΒ){ΗΒ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ǌ=(Κ!=Ι.і?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ǌ)
ΕЏ(ǌ);String[]Ν=Me.CustomData.Split('\n');ų=bool.Parse(ΑЀ(Αν(Ν,"Enable_Broadcast",ref Ƽ)));bool Εΰ=false;bool ΖЈ=true;if(ų){if(Κ!=Ι.і)
Ɣ=ΑЀ(Αν(Ν,"Ship_Name",ref Ƽ)).Replace(BROADCAST_SELF_ID,"");ΔΖ=ΑЀ(Αν(Ν,"Broadcast_Channel",ref Ƽ)).ToLower();ΖЈ=false;if(Ǩ==null){Ǩ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǩ.SetMessageCallback("");}List<IMyRadioAntenna>Ů=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ů);bool ΓЊ=false;
for(int i=0;i<Ů.Count;i++)
if(Ů[i].EnableBroadcasting&&Ů[i].Enabled){ΓЊ=true;break;}if(Ů.Count==0) Λ="No Antenna found";else if(!ΓЊ) Λ="Antenna not ready";Εΰ=Ů.Count==0||!ΓЊ;if(Ζζ&&!Εΰ&&Κ!=Ι.і) Λ="Antenna ok";}else if(Κ==Ι.і)
Λ="Offline - Enable in PB custom data";Ζζ=Εΰ;if(ΖЈ){if(Ǩ!=null) this.IGC.DisableBroadcastListener(Ǩ);Ǩ=null;}}catch{Ƽ=false;}if(!Ƽ){Λ="Reset custom data";ΕЏ(ǌ);}}void ΕЏ(String ǌ){String Ξ=ǌ+"\n\n"+"Enable_Broadcast="+(Κ==Ι.і?"true":"false")+" \n";Ξ+=Κ!=Ι.і?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ΑЀ(String Ŏ){int Α=Ŏ.IndexOf("//");if(Α!=-1) Ŏ=Ŏ.Substring(0,Α);String[]τ=Ŏ.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αν(String[]Ν,String ȥ,ref bool ȧ){foreach(String Ŏ in Ν)
if(Ŏ.StartsWith(ȥ)) return Ŏ;ȧ=false;return "";}void Θέ(string Т){if(Ą) Ъ=new List<Є>();String Ȉ="";String Ȃ="";if(!Γα(Т)) ΘЋ(Т);bool Ηϊ=false;int ΔЅ=50;while(Ǩ!=null&&Ǩ.HasPendingMessage&&ΔЅ-->0){MyIGCMessage δ=Ǩ.AcceptMessage();String Ξ=δ.Data as String;if(Ξ==null) continue;
if(ΖΞ(ref Ξ,out Ȉ,out Ȃ)&&Ȉ!=""&&Ȉ!=BROADCAST_SELF_ID){Є Δ=ΓΟ(Ȉ,"");if(Δ==null){Δ=new Є(Ȉ);Ъ.Add(Δ);}Δ.ĸ(Ȃ,false,false,true);Ιί(Δ,Ξ);Ηϊ=true;}}if(Ηϊ)
Ъ.Sort(delegate(Є a,Є b){if(a.ί==null&&b.ί==null) return 0;else if(a.ί==null) return-1;else if(b.ί==null) return 1;else return a.ί.CompareTo(b.ί);});if(ĉ||Ą){if(ΑΟ<=0||Ą){Λ="";Ą=false;ΙΔ();ΕЎ();Δφ=DateTime.Now;}}if(ſ) ΙΧ();if(ſ) ΙΫ();ΙΥ();}void Ιί(Є Δ,String Ξ){Δ.Δσ=DateTime.Now;
String ǆ="";д(ref Ξ,out Δ.ΒЇ,BROADCAST_SEP);д(ref Ξ,out Δ.ί,BROADCAST_SEP);if(Δ.ΒЇ!=VERSION) return;String ΑΚ="";String[]Ƌ=new string[3];д(ref Ξ,out ΑΚ,BROADCAST_SEP);д(ref Ξ,out Ƌ[0],BROADCAST_SEP);д(ref Ξ,out Ƌ[1],BROADCAST_SEP);д(ref Ξ,out Ƌ[2],BROADCAST_SEP);д(ref Ξ,out Δ.ΕΜ,BROADCAST_SEP);
д(ref Ξ,out Δ.Λ,BROADCAST_SEP);д(ref Ξ,out ǆ,BROADCAST_SEP);д(ref Ξ,out Δ.Βκ,BROADCAST_SEP);String ΘΒ="",Ηε="";д(ref Ξ,out ΘΒ,BROADCAST_SEP);д(ref Ξ,out Ηε,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String ΗΤ;if(!д(ref Ξ,out ΗΤ,BROADCAST_SEP)) break;String[]Βή=ΗΤ.Split('/');if(Βή.Count()<3) continue;
int Љ=0;if(!int.TryParse(Βή[2],out Љ)) continue;Δ.Ћ.Add(new й(Βή[0],Βή[1],Љ,Ш.Ŧ));}int Ηι=0;if(!int.TryParse(ǆ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΚ,out Ηι)) Δ.Κ=Ι.Ğ;if(!float.TryParse(Ƌ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ƌ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(Ƌ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(ΘΒ,out Δ.Ȁ)) Δ.Ȁ=0;if(!float.TryParse(Ηε,out Δ.Ȇ)) Δ.Ȇ=0;Δ.Κ=(Ι) Ηι;Δ.ΔΧ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΕΩ=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.ΕΩ+=Δ.Ћ[i].Љ;}Є ΓΟ(String ΰ,String ί){ΰ=ΰ.ToUpper();ί=ί.ToUpper();for(int i=0;i<Ъ.Count;i++)
{if(ΰ!=""&&Ъ[i].ΰ.ToUpper()==ΰ) return Ъ[i];if(ί!=""&&Ъ[i].ί!=null&&Ъ[i].ί.ToUpper()==ί) return Ъ[i];}return null;}String ŵ="";Є Ζφ=null;DateTime Ζχ=DateTime.MinValue;List<Є>ŀ=new List<Є>();const float ΘΞ=20f;bool ΖΨ(Є ζ,String Ѕ){return Ѕ==ŵ&&ζ==Ζφ&&(DateTime.Now-Ζχ).TotalSeconds<ΘΞ;
}String Θη(Є ζ){if(ŵ==""||!ΖΨ(ζ,ŵ)) return "";String s="";int ȧ=0,ΕΗ=0;for(int i=0;i<ŀ.Count;i++){Ј Βϊ=ŀ[i].ĸ(ŵ,false,false,false);if(Βϊ==Ј.Ƭ) ȧ++;if(ΕΗ++<8) s+="  "+ŀ[i].ί+": "+(Βϊ==Ј.Ƭ?"received":Βϊ==Ј.ũ?"pending...":Βϊ==Ј.Αύ?"no answer!":"-")+"\n";
}if(ΕΗ>8) s+="  +"+(ΕΗ-8)+" more\n";return "--------------------------------------------\n"+ŵ+": "+ȧ+"/"+ŀ.Count+" received\n"+s;}void Ǥ(Є Δ,String Ѕ){ŵ=Ѕ;Ζφ=Δ;Ζχ=DateTime.Now;ŀ.Clear();if(Δ==null) ŀ.AddRange(Ъ);else ŀ.Add(Δ);if(Δ==null){for(int i=0;i<Ъ.Count;i++)
Ъ[i].ĸ(Ѕ,true,false,false);Γψ(Ѕ,"*",Ѕ);}else{Δ.ĸ(Ѕ,true,false,false);Γψ(Ѕ,Δ.ΰ,Ѕ);}}void ΘЋ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ѕ=τ.ElementAtOrDefault(0);var Ț=τ.ElementAtOrDefault(1);String Ȥ="Invalid argument: "+Т;if(ϋ==Σ.Ư){if(К!=null)
{switch(Ѕ){case "UP":{if(Ў<2) break;if(К.Ў==0){Ў=1;return;}Ǥ(К,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){Ǥ(К,"MRES");break;}Ǥ(К,"DOWN");break;}case "APPLY":{if(Ў<2) break;Ǥ(К,"APPLY");return;}}}}switch(Ѕ){case "UP":this.Γε(false);return;case "DOWN":this.Γδ(false);return;
case "APPLY":this.Ţ(true);return;}switch(Ѕ){case "CLEAR":ΘΨ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ț)){Λ="Usage: SENDALL <cmd>";return;}Ǥ(null,Ț);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΙΗ(Т.Remove(0,"SEND".Length+1));return;}Λ=Ȥ;}void ΙΗ(String ƶ){if(ƶ=="") return;var τ=ƶ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ɣ=τ.ElementAtOrDefault(0).Trim();
Є Δ=ΓΟ("",Ɣ);if(Δ!=null) Ǥ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ɣ;}void ΘΨ(){Ъ.Clear();К=null;κ(Σ.Main);Ў=0;}void ΙΔ(){List<IMyTerminalBlock>ΑΧ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ц,Ǒ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΧ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǣ(Ц,pamTag,true);ΖΦ(ΑΧ);ΖΥ();ΙΕ();ĺ(Ц,setLCDFontAndSize,1.15f,false);ĺ(ȋ,setLCDFontAndSize,1.15f,true);String ΙЌ=ΕЂ(false);String Ην="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock х in Ц){if(х.CustomData==""){х.CustomData=ΕЂ(true);continue;}
if(!х.CustomData.Contains(ΙЌ))
if(!х.CustomData.Contains(Ην)) х.CustomData=Ην+х.CustomData;}}void ΙΥ(){String ǃ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȋ.Count;i++)
ȋ[i].WriteText(Εκ(С.ΖΗ,"0",false));for(int i=0;i<Ц.Count;i++){С Џ=С.Ğ;String ǲ="";ΘЇ(Ц[i],out Џ,out ǲ);Ц[i].WriteText(Εκ(Џ,ǲ,i==0));}Echo(ǃ);for(int i=0;i<Ń.Count;i++){IMyTextPanel х=Ń[i];String Ξ=х.CustomData.ToUpper();if(Ξ=="DEBUG") х.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") х.WriteText(ΕЁ());
}}String Ţ(bool Θ){int ΙЂ=0;return Ţ(Θ,0,ref ΙЂ,false,1);}String Ţ(bool Θ,int ΚΦ,ref int Κδ,bool Ζϊ,int ΑΜ){String Γ="";String Ɠ="——————————————————\n";String Κς="--------------------------------------------\n";if(К==null||ϋ==Σ.Main||ϋ==Σ.Γω)
Γ+="[PAM]-Controller | "+Ъ.Count+" Connected ships"+"\n"+Θκ()+Θο();else Γ+=ΕЅ(К)+"\n";Γ+=Ɠ;int Α=0;int Η=Ў;Ő=0;if(Λ!=""){Μ(ref Γ,Η,Α++,Θ,Λ);}else if(ϋ==Σ.Main){if(К==null&&Ъ.Count>=1) К=Ъ[0];String Ѝ="";if(Μ(ref Ѝ,Η,Α++,Θ," Send command to all")) κ(Σ.Γω);
int ƒ=0;for(int s=0;s<Ъ.Count;s++){if(Ζϊ) Ѝ+="\n";Є Δ=Ъ[s];if(Η==Α||Η==Α+1) К=Δ;if(Α==Η) ƒ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Α++,Θ," "+ΕЅ(Δ))){К=Δ;κ(Σ.Ư);К.ĸ("",false,true,false);}if(Α==Η) ƒ=Ѝ.Split('\n').Length-1;if(Μ(ref Ѝ,Η,Α++,Θ," "+(Ζϊ?"":"  ")+Θρ(Δ)))
{К=Δ;κ(Σ.ΖΧ);}}int Ɯ=ΑΜ-2;Ɯ+=Math.Max(0,(ΚΦ-1))*ΑΜ;Γ+=Γχ(Ɯ,Ѝ,ƒ,ref Κδ);}else if(ϋ==Σ.ΑЄ||ϋ==Σ.Γω){Σ Ιλ=ϋ==Σ.ΑЄ?Σ.Ư:Σ.Main;Є ζ=null;if(ϋ==Σ.ΑЄ) ζ=К;if(Μ(ref Γ,Η,Α++,Θ," Back")){κ(Ιλ);}if(Μ(ref Γ,Η,Α++,Θ," [Stop] "+ť(ζ,"STOP"))){Ǥ(ζ,"STOP");}if(Μ(ref Γ,Η,Α++,Θ," [Continue] "+ť(ζ,"CONT"))){Ǥ(ζ,"CONT");}
if(Μ(ref Γ,Η,Α++,Θ," [Move home] "+ť(ζ,"HOMEPOS"))){Ǥ(ζ,"HOMEPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Move to job] "+ť(ζ,"JOBPOS"))){Ǥ(ζ,"JOBPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Full simulation] "+ť(ζ,"FULL"))){Ǥ(ζ,"FULL");}if(ζ!=null&&ζ.Κ==Ι.έ)
if(Μ(ref Γ,Η,Α++,Θ," [Undock] "+ť(ζ,"UNDOCK"))){Ǥ(ζ,"UNDOCK");}Γ+=Θη(ζ);}else if(ϋ==Σ.Ư){String ΓБ="";if(Μ(ref ΓБ,Η,Α++,Θ," Back")){κ(Σ.Main);}Γ+=ΓБ.Substring(0,ΓБ.Length-1).PadRight(36,' ');if(Μ(ref Γ,Η,Α++,Θ," Send cmd")){κ(Σ.ΑЄ);}if(!К.ΖΓ()&&!К.Γή()) Ő++;
if(К.Γή()) Γ+="No answer received...";else Γ+=ū(К,К.Βκ);}else if(ϋ==Σ.ΖΧ){if(Μ(ref Γ,Η,Α++,Θ," Back")){κ(Σ.Main);}Γ+=Κς;Γ+=ū(К,ΕЄ(К));}if(!ΔЄ) Ў=Math.Min(Ő-1,Ў);return Γ;}String ΕЅ(Є Δ){if(Δ.ΒЇ!=VERSION) return Δ.ί+": Different version!";return Δ.ί+": "+ū(Δ,Δ.ΕΜ)+" "+Δ.ΔΧ+"m";
}String Θρ(Є Δ){String s=Γτ("",И(Δ.Ȁ,Δ.Ȇ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;й ύ=Δ.Ћ[i];s+=Ζΰ(ΓΑ(Θξ(ύ)),3)+" "+Εϋ(ύ.Љ)+" ";}return s;}String Θξ(й ύ){if(ύ.υ.ToUpper()=="ORE"||ύ.υ.ToUpper()=="INGOT"){String Ζψ=Θμ(ύ.ί.ToUpper());if(Ζψ!="") return Ζψ;
}return ύ.ί;}String ū(Є Δ,String Γ){if(Δ.ΖΓ()) return "No signal...("+Απ((int)(DateTime.Now-Δ.Δσ).TotalSeconds)+")";return Γ;}String ΕЂ(bool ΚЇ){String Γ="";if(ΚЇ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΘЇ(IMyTerminalBlock х,out С Џ,out String ǲ){bool ȧ=true;String ΑΚ=ΑЀ(Αν(х.CustomData.Split('\n'),"mode",ref ȧ)).ToUpper();String[]τ=ΑΚ.Split(':');String ΑΙ=τ.Length>0?τ[0].Trim():"";ǲ=τ.Length>1?τ[1].Trim():"";
Џ=С.Ğ;if(ΑΙ=="MAIN") Џ=С.ΖΘ;else if(ΑΙ=="MAINX") Џ=С.ΖΙ;else if(ΑΙ=="MENU") Џ=С.Γγ;else if(ΑΙ=="INVENTORY") Џ=С.Γέ;else if(ΑΙ=="DEBUG") Џ=С.ΓΖ;if(Џ==С.Ğ) ǲ=ΑΚ;}String Εκ(С Џ,String ǲ,bool ΙЃ){int ΑΗ=15;if(ΙЃ){Ζό=ΒΓ;ΒΓ=0;Ζύ=ΒΔ;ΒΔ=0;}String Γ="";if(Џ==С.ΖΘ)
{int ф=0;Γ=Ţ(false,Ζό,ref Ιυ,true,ΑΗ);if(!int.TryParse(ǲ,out ф)) return Γ;ΒΓ=Math.Max(ф,ΒΓ);ф--;return ΖΝ(ф,ΑΗ,Γ);}else if(Џ==С.ΖΙ){int ф=0;Γ=Ţ(false,Ζύ,ref Ιφ,false,ΑΗ);if(!int.TryParse(ǲ,out ф)) return Γ;ΒΔ=Math.Max(ф,ΒΔ);ф--;return ΖΝ(ф,ΑΗ,Γ);}if(Џ==С.ΖΗ){return Ţ(false,0,ref Ιχ,true,ΑΗ);
}else if(Џ==С.Γέ||Џ==С.Γγ||Џ==С.ΓΖ){Є Δ=К;if(ǲ!=""){Δ=ΓΟ("",ǲ);if(Δ==null) return "Unknown ship: "+ǲ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ɠ="—————————————————————\n";String ΕΧ="";String ΔΕ="";if(Џ==С.Γγ){ΕΧ="Menu";ΔΕ=ū(Δ,Δ.Βκ);}
else if(Џ==С.Γέ){ΕΧ="Inventory";ΔΕ=ū(Δ,ΕЄ(Δ));}else if(Џ==С.ΓΖ){return ū(Δ,Θς(Δ));}Γ+=Δ.ί+" - "+Δ.ΔΧ+"m | "+ΓΑ(""+ΕΧ)+"\n"+Ɠ;Γ+=ΔΕ;return Γ;}return "Unknown command: "+ǲ;}string Θς(Є Δ){String s="";s+=Δ.ΒЇ+"\n";s+=Δ.ί+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕΜ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βκ.Length+"\n";s+=Δ.Ȁ+"\n";s+=Δ.Ȇ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΕЄ(Є Δ){String s="";s+=Γτ("All",И(Δ.Ȁ,Δ.Ȇ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){й ύ=Δ.Ћ[i];
s+=Γτ(ύ.ί,ύ.Љ,Δ.ΕΩ,30,8,10)+"\n";}return s;}Ј Θθ(Є Δ,bool э,string Ѕ){Ј ή=Ј.None;if(Δ==null){for(int i=0;i<Ъ.Count;i++){Ј Ђ=Ъ[i].ĸ(Ѕ,false,э,false);if(Ђ==Ј.Ƭ) ή=Ђ;if(Ђ==Ј.Αύ) return Ђ;if(Ђ==Ј.ũ) return Ђ;}return ή;}else return Δ.ĸ(Ѕ,false,э,false);}String ť(Є Δ,String Ѕ)
{if(!ΖΨ(Δ,Ѕ)) return "";Ј Ђ=Θθ(Δ,false,Ѕ);if(Ђ==Ј.Ƭ) return "received!";if(Ђ==Ј.ũ) return "pending...";if(Ђ==Ј.Αύ) return "no answer!";return "";}String Γχ(int Ɯ,String Γ,int ƒ,ref int ď){String[]Ν=Γ.Split('\n');if(ƒ>=ď+Ɯ-1) ď++;ď=Math.Min(Ν.Count()-1-Ɯ,ď);if(ƒ<ď+1) ď--;
ď=Math.Max(0,ď);String ή="";for(int i=0;i<Ɯ;i++){int Ηά=i+ď;if(Ηά>=Ν.Count()) break;ή+=Ν[Ηά]+"\n";}return ή;}String ΖΝ(int ф,int ΑΜ,String Γ){String[]Ν=Γ.Split('\n');int ΑΤ=ф*ΑΜ;int ă=(ф+1)*ΑΜ;String ή="";for(int i=ΑΤ;i<ă;i++){if(i>=Ν.Count()) break;ή+=Ν[i]+"\n";
}return ή;}String ΓΑ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζΰ(String s,int Ηγ){if(s=="") return s;if(s.Length>Ηγ) s=s.Substring(0,Ηγ-1)+".";return s;}string Γτ(String ō,float value,float ƃ,int Ζν,int ΗΧ,int ΚΥ){float ŕ=И(value,ƃ)*Ζν;
String ΑБ="[";for(int i=0;i<Ζν;i++){if(i<=ŕ) ΑБ+="|";else ΑБ+="'";}ΑБ+="]";return ΑБ+" "+Ζΰ(ΓΑ(ō),ΗΧ).PadRight(ΗΧ)+"".PadRight(ΚΥ)+Εϋ(value);}String Εϋ(float Љ){if(Љ>=1000000) return Math.Round(Љ/1000000f,Љ/1000000f<100?1:0)+"M";if(Љ>=1000) return Math.Round(Љ/1000f,Љ/1000f<100?1:0)+"K";
return ""+Math.Round(Љ);}String Απ(int ΑΠ){if(ΑΠ>=60*60) return Math.Round(ΑΠ/(60f*60f),1)+" h";if(ΑΠ>=60) return Math.Round(ΑΠ/60f,1)+" min";return ""+ΑΠ+" s";}string Ť(float v){return Math.Round(v,2)+" ";}string ΚЎ(Vector3 v){return "X"+Ť(v.X)+"Y"+Ť(v.Y)+"Z"+Ť(v.Z);}
List<IMyCameraBlock>Ȝ=new List<IMyCameraBlock>();List<long>Ƈ=new List<long>();bool Ɖ=false;int ΔΜ=0;int ΔΙ=0;DateTime Ζο=DateTime.Now;String ΔΔ="";void ΙΓ(){Ȝ.Clear();Ƈ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȝ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȝ.Count;i++){Ȝ[i].EnableRaycast=true;if(!Ƈ.Contains(Ȝ[i].CubeGrid.EntityId)) Ƈ.Add(Ȝ[i].CubeGrid.EntityId);}if(!Ƈ.Contains(Me.CubeGrid.EntityId)) Ƈ.Add(Me.CubeGrid.EntityId);}bool ΖΑ(Β s){return s==Β.ě||s==Β.р||s==Β.Л||
s==Β.Ě||s==Β.ţ||s==Β.ƣ||s==Β.Αξ||
(s==Β.Ǹ&&Ǚ!=null);}void Αι(){Ɖ=false;ΔΜ=0;ΔΔ="";}void Ιέ(){if(!obstacleBrake||Ȝ.Count==0||!ΖΑ(Ϊ)){Αι();return;}Vector3 Ǉ=Ѓ+Į;Vector3 Ȍ=Ǉ-ε;float Ȋ=Ȍ.Length();if(Ȋ<1f){Αι();return;}Vector3 ρ=Ȍ/Ȋ;float Ηΰ=Đ*obstacleLookAhead+ο;Vector3 Κά=Ǉ+ρ*(ο*0.25f);
for(int n=0;n<Ȝ.Count;n++){ΔΙ=(ΔΙ+1)%Ȝ.Count;IMyCameraBlock ǽ=Ȝ[ΔΙ];if(!ǽ.IsFunctional||!ǽ.IsWorking) continue;Vector3 Ζρ=ǽ.GetPosition();Vector3 ă=Κά;if(Vector3.Distance(Ζρ,ă)>Ηΰ) ă=Ζρ+ρ*Ηΰ;if(!ǽ.CanScan(ă)) continue;MyDetectedEntityInfo Ȣ=ǽ.Raycast(ă);bool ΚΡ=!Ȣ.IsEmpty()&&!Ƈ.Contains(Ȣ.EntityId)&&!ΘЀ(Ȣ.EntityId)&&
Ȣ.Type!=MyDetectedEntityType.FloatingObject;if(ΚΡ){if(!Ɖ) Ζο=DateTime.Now;Ɖ=true;ΔΜ=0;ΔΔ=Ȣ.Type.ToString();}else if(Ɖ&&++ΔΜ>=3) Αι();break;}if(Ɖ&&(DateTime.Now-Ζο).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΔ;}const String ΓЄ="[PAMTFC]";const float ΙΞ=5f;const float ΓΞ=2f;class ĝ{public long ΰ;public Vector3 ξ,ƛ;public Vector3 ΑΑ;public float ΖЇ;public String Χ="";public int Ć=-1;public bool ΔБ;public long α=0;public int ƿ=0;public DateTime ΕΤ;}IMyBroadcastListener ΒЁ=null;
long ǘ=0;List<ĝ>ђ=new List<ĝ>();Vector3 Į=new Vector3();float Ȑ=-1;Vector3 ΕΪ=new Vector3();bool ΒЀ=false;DateTime ΕΫ=DateTime.Now;const float Ζί=15f;const float ΙΝ=10f;int ΗА=0;String м="";int Г=-1;bool ΔΠ=false;Vector3 Ζώ=new Vector3();String Αχ(){if(β==Ρ.ы||Κ==Ι.έ) return "";
return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Ь+"|"+(int) Ō+"|"+Math.Round(ȏ,1)+"|"+Math.Round(Ȏ,1);}void Ια(){if(!trafficEnabled) return;if(ΒЁ==null) ΒЁ=IGC.RegisterBroadcastListener(ΓЄ);ǘ=Me.CubeGrid.EntityId;int ā=30;while(ΒЁ.HasPendingMessage&&ā-->0){String Ξ=ΒЁ.AcceptMessage().Data as String;if(Ξ!=null) ΘЉ(Ξ);
}for(int i=ђ.Count-1;i>=0;i--)
if((DateTime.Now-ђ[i].ΕΤ).TotalSeconds>ΙΞ) ђ.RemoveAt(i);if(ΒΖ==null) ΒΖ=IGC.RegisterBroadcastListener(ΖΔ);ā=10;while(ΒΖ.HasPendingMessage&&ā-->0){String Ξ=ΒΖ.AcceptMessage().Data as String;if(Ξ!=null) try{ΘІ(Ξ);}catch{}}if(++ΗА<3) return;if(Ϊ==Β.с&&Я(MyShipConnectorStatus.Connected)==null) return;
ΗА=0;IGC.SendBroadcastMessage(ΓЄ,ǘ+";"+σ(ε)+";"+
σ(Ω.GetShipVelocities().LinearVelocity)+";"+ο+";"+Αχ()+";"+
(β==Ρ.З?Г:-1)+";"+м+";"+ΘΧ()+","+Εψ()+";"+σ(ΕΪ)+
";"+(Ŷ?1:0)+";"+(ŉ&&β==Ρ.З?1:0)+","+Βυ+","+Βτ);}void ΘЉ(String Ξ){try{String[]f=Ξ.Split(';');if(f.Length<7) return;long ΰ=long.Parse(f[0]);if(ΰ==ǘ) return;ĝ Δ=null;for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) Δ=ђ[i];if(Δ==null){Δ=new ĝ();Δ.ΰ=ΰ;ђ.Add(Δ);}
Δ.ξ=ς(f[1]);Δ.ƛ=ς(f[2]);Δ.ΖЇ=float.Parse(f[3]);Δ.Χ=f[4];Δ.Ć=int.Parse(f[5]);Δ.ΕΤ=DateTime.Now;if(Δ.Χ!=""&&Δ.Χ==Αχ()) ΖΚ(f[6]);Δ.α=0;Δ.ƿ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ƿ=int.Parse(d[1]);}Δ.ΑΑ=f.Length>=9?ς(f[8]):new Vector3();
Δ.ΔБ=f.Length>=10&&f[9]=="1";}catch{}}bool ΘЀ(long ΰ){for(int i=0;i<ђ.Count;i++) if(ђ[i].ΰ==ΰ) return true;return false;}Vector3 ΕЃ(int Α,int ΚΧ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();Ϋ ά=Φ[Α];Vector3 ΚϏ=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*ΚΧ;
Vector3 Б=ά.φ.LengthSquared()>0.01f?-Vector3.Normalize(ά.φ):-ά.η;Vector3 о=Vector3.Cross(ΚϏ,Б);if(о.LengthSquared()<0.0001f) return new Vector3();float Њ=laneOffset*ο;float ΙЅ=Math.Min(Vector3.Distance(ά.Ε,Φ[0].Ε),
Vector3.Distance(ά.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(о)*Њ*Math.Min(1,ΙЅ/(Њ*4));}Vector3 ΗЏ=new Vector3();bool ΘϏ(Β s){return s==Β.Ǹ||s==Β.ı||s==Β.ĵ||s==Β.ƭ||
s==Β.Ǧ||s==Β.Ȳ||s==Β.Ė;}void Ιβ(){Į=new Vector3();Ȑ=-1;ΕΪ=new Vector3();bool ǈ=ΖΑ(Ϊ);if(!trafficEnabled||(!ǈ&&!ΘϏ(Ϊ))){ΒЀ=false;return;}ΗЏ=Ω.GetShipVelocities().LinearVelocity;ΘΩ(Ω.GetNaturalGravity(),!ǈ);}void ΘΩ(Vector3 φ,bool Ης=false){Į=new Vector3();Ȑ=-1;Vector3 Ȍ=Ѓ-ε;
float Ȋ=Ȍ.Length();bool Δκ=Ȋ>=1f;Vector3 ρ=Δκ?Ȍ/Ȋ:new Vector3();ΕΪ=ρ;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ǁ;Vector3 о=Vector3.Cross(ρ,Б);о=о.LengthSquared()<0.0001f?-ΑΖ:Vector3.Normalize(о);ĝ Βΰ=null;for(int i=0;i<ђ.Count;i++){ĝ λ=ђ[i];
if(λ.ƿ==3&&λ.ƛ.LengthSquared()<0.25f) continue;Vector3 ξ=λ.ξ+λ.ƛ*(float)(DateTime.Now-λ.ΕΤ).TotalSeconds;Vector3 ƍ=ξ-ε;float э=(ο+λ.ΖЇ)*0.5f;float Ƶ=Δκ?Vector3.Dot(ƍ,ρ):0;if(Ƶ>0&&(ƍ-ρ*Ƶ).Length()<=э*1.2f){float Βξ=Vector3.Dot(λ.ƛ,ρ);bool ΗΙ=λ.ΑΑ.LengthSquared()>0.25f;
bool Δλ=Βξ<-1f||(ΗΙ&&Vector3.Dot(λ.ΑΑ,ρ)<-0.5f);if(Ης){if(Δλ&&ǘ<λ.ΰ) continue;}else{if(λ.ΔБ&&!Ŷ){Į=о*э*1.3f;continue;}if(Δλ){if(Ƶ<(Đ-Βξ)*6f+э*4f) Į=о*э*1.3f;continue;}if(ΗΙ&&ǘ<λ.ΰ){Vector3 Ζλ=-ƍ;float Ηπ=Vector3.Dot(Ζλ,λ.ΑΑ);if(Ηπ>0&&(Ζλ-λ.ΑΑ*Ηπ).Length()<=э*1.2f)
{Į=о*э*1.3f;continue;}}}float ΗΕ=Ƶ-э-followGap*ο;float Δψ=Math.Max(0,Βξ)+Math.Max(0,ΗΕ)/ΓΞ;if(Δλ) Δψ=Math.Max(0,ΗΕ+Math.Min(0,Βξ)*ΓΞ*2)/ΓΞ;if(Ȑ<0||Δψ<Ȑ){Ȑ=Δψ;Βΰ=λ;}continue;}if(λ.ƛ.LengthSquared()<1f) continue;if(Δκ&&(ƍ-ρ*Ƶ).Length()<=э*1.2f) continue;
Vector3 ΕΕ=λ.ƛ-ΗЏ;float Ηφ=ΕΕ.LengthSquared();if(Ηφ<0.25f) continue;float t=-Vector3.Dot(ƍ,ΕΕ)/Ηφ;if(t<=0||t>5f||(ƍ+ΕΕ*t).Length()>=э*1.1f) continue;if(ǘ>λ.ΰ||t<2f){Ȑ=0;Βΰ=λ;}}bool ΙЋ=Ȑ>=0&&Ȑ<0.5f;if(!ΙЋ||Ης||(Βΰ!=null&&Βΰ.ΔБ&&Ŷ)){ΒЀ=false;return;}
if(!ΒЀ){ΒЀ=true;ΕΫ=DateTime.Now;}double ΗΚ=(DateTime.Now-ΕΫ).TotalSeconds;if(ΗΚ>Ζί+ΙΝ) ΕΫ=DateTime.Now;else if(ΗΚ>Ζί){Ȑ=-1;Į=о*ο*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȣ=new List<int>();String ΗΜ="";void ΓΝ(){String ȥ=Ќ+"|"+Ь+"|"+(int) Ō;
if(ȥ==ΗΜ) return;ΗΜ=ȥ;Ελ(Ќ,Ь,Ō,ȣ);}void Ελ(int w,int h,ь ΑΤ,List<int>ΔΚ){ΔΚ.Clear();if(ΑΤ==ь.Ʊ){for(int k=0;k<w*h;k++){int Ǝ=k/w;ΔΚ.Add(Ǝ*w+(Ǝ%2==0?k-Ǝ*w:w-1-(k-Ǝ*w)));}return;}int ǋ=(int) Math.Ceiling(w/2f),Ǌ=(int) Math.Ceiling(h/2f);int Δη=w/2,Δζ=h/2;
int x=0,y=0,ΔΪ=0,ΔΫ=-1,ƃ=Math.Max(w,h);for(int i=0;i<ƃ*ƃ;i++){if(-ǋ<x&&x<=Δη&&-Ǌ<y&&y<=Δζ) ΔΚ.Add((y-1+Ǌ)*w+(x-1+ǋ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔΪ;ΔΪ=-ΔΫ;ΔΫ=t;}x+=ΔΪ;y+=ΔΫ;}}bool ΕЋ(int a,int b,int w,float ΒЂ,float Βύ,float ǉ,float ǅ)
{return a==b||(Math.Abs(a%w-b%w)*ΒЂ<ǉ+0.5f&&Math.Abs(a/w-b/w)*Βύ<ǅ+0.5f);}bool ΓΜ(){int n=Math.Max(0,Ќ*Ь);if(м.Length==n) return false;м=new String('0',n);Г=-1;return true;}void ΖΚ(String λ){ΓΜ();if(λ.Length!=м.Length) return;char[]h=м.ToCharArray();for(int i=0;i<h.Length;i++) if(λ[i]=='1') h[i]='1';
м=new String(h);}bool Αυ(int У){return У>=0&&У<м.Length&&м[У]=='1';}void Γϋ(int У,bool ώ){if(У<0||У>=м.Length) return;char[]h=м.ToCharArray();h[У]=ώ?'1':'0';м=new String(h);}bool ΓΪ(int У,bool ΚΒ){if(!trafficEnabled) return false;String ȥ=Αχ();for(int i=0;i<ђ.Count;i++)
{ĝ t=ђ[i];if(t.Χ!=ȥ||t.Ć<0||(ΚΒ&&t.ΰ>ǘ)) continue;if(ΕЋ(У,t.Ć,Ќ,ȏ,Ȏ,Βυ,Βτ)) return true;}return false;}int ΘΦ(int Α){int n=Ќ*Ь;if(n<1) return Α;ΓΝ();if(ΓΜ()) for(int k=0;k<Α&&k<n;k++) Γϋ(ȣ[k],true);if(Г>=0&&(Α>=n||ȣ[Α]!=Г)){Γϋ(Г,true);Г=-1;}
if(ΖΕ()){if(ȟ>=0&&!Αυ(ȟ)){ŉ=false;Г=ȟ;return ȣ.IndexOf(ȟ);}if(ȟ==-2){ŉ=false;Г=-1;return n;}if(ȟ==-1&&Г>=0&&!Αυ(Г)){ŉ=false;return Α;}Г=-1;if(!ŉ){ŉ=true;ΖЀ=DateTime.Now;}if((DateTime.Now-ΖЀ).TotalSeconds<5) return-1;}ŉ=false;if(Г>=0&&!Αυ(Г)&&!ΓΪ(Г,true)) return Α;
Г=-1;for(int k=0;k<n;k++){int c=ȣ[k];if(Αυ(c)||ΓΪ(c,false)) continue;Г=c;return k;}return n;}const String ΖΔ="[PAMJOB]";IMyBroadcastListener ΒΖ=null;DateTime ΖϏ=DateTime.MinValue;DateTime ΖЀ=DateTime.Now;int ȟ=-1;bool ŉ=false;bool ΖΕ(){return trafficEnabled&&(DateTime.Now-ΖϏ).TotalSeconds<3;}
void ΘІ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Αχ()) return;ΖϏ=DateTime.Now;ΖΚ(f[2]);ȟ=-1;String Ηζ=ǘ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ηζ)) ȟ=int.Parse(e[i].Substring(Ηζ.Length));}ǥ ΙΘ(int Α)
{ΓΝ();if(Α<0||Α>=ȣ.Count) return ǥ.Αδ;Ϗ=ȣ[Α]%Ќ;ό=ȣ[Α]/Ќ;return ǥ.Αρ;}class ǒ{public long ΰ;public Vector3 ξ,Ы,Б;}List<ǒ>Ф=new List<ǒ>();bool ģ=false;Vector3 ΒΟ,ΒΞ,ΒΠ,ΒΡ;long ћ=0;long Ȩ=0;Ϋ Ǚ=null;bool ǀ=false;Ϋ ΓΙ(Vector3 ξ,Vector3 Ы,Vector3 Б){MatrixD m=MatrixD.CreateWorld(ξ,Ы,Б);
Ϋ w=new Ϋ(Ο);w.θ=Vector3D.TransformNormal(ΒΟ,m);w.η=Vector3D.TransformNormal(ΒΞ,m);w.Ё=Vector3D.TransformNormal(ΒΠ,m);w.Ε=ξ+(Vector3) Vector3D.TransformNormal(ΒΡ,m);w.ι=true;return w;}int Εψ(){Β s=Ϊ;if(s==Β.с) return Я(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Β.ĵ||s==Β.ș||s==Β.Ĵ||s==Β.ΑЋ||
s==Β.Αμ||s==Β.Ǧ) return 3;if(s==Β.Ǹ||s==Β.ı||s==Β.ƭ) return 2;if(s==Β.Л) return 1;return 0;}long ΘΧ(){if(Εψ()==0) return 0;if(Ĝ(1)&&ƕ!=0) return ƕ;return Ǚ!=null?Ȩ:ћ;}void ΖΖ(){if(Κ==Ι.έ||!Ο.ι) return;IMyShipConnector О=Я(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector λ=О.OtherConnector;Vector3I ΙЍ=new Vector3I((int) Ο.л.X,(int) Ο.л.Y,(int) Ο.л.Z);if(О.Position==ΙЍ){MatrixD ż=MatrixD.Transpose(λ.WorldMatrix);ΒΟ=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,ż);ΒΞ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,ż);ΒΠ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,ż);
ΒΡ=Vector3D.TransformNormal(О.GetPosition()-λ.GetPosition(),ż);ģ=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ћ=λ.EntityId;List<IMyShipConnector>Ň=new List<IMyShipConnector>();χ.GetBlocksOfType(Ň,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ň.Count>0){Ф.Clear();for(int i=0;i<Ň.Count;i++){ǒ p=new ǒ();p.ΰ=Ň[i].EntityId;p.ξ=Ň[i].GetPosition();p.Ы=Ň[i].WorldMatrix.Forward;p.Б=Ň[i].WorldMatrix.Up;Ф.Add(p);}}Ǚ=null;Ȩ=0;if(λ.EntityId==ћ||!ģ) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].ΰ==λ.EntityId){Ȩ=Ф[i].ΰ;Ǚ=ΓΙ(Ф[i].ξ,Ф[i].Ы,Ф[i].Б);}}bool Γΰ(long ΰ,int Ηκ){if(ΰ==0) return false;for(int i=0;i<ђ.Count;i++){ĝ t=ђ[i];if(t.α==ΰ&&(t.ƿ>Ηκ||(t.ƿ==Ηκ&&t.ΰ<ǘ))) return true;}return false;}bool ΓΔ(int ΔЎ){if(Κ==Ι.έ||!trafficEnabled){Ǚ=null;Ȩ=0;return true;}
long ν=Ǚ!=null?Ȩ:ћ;if(!Γΰ(ν,ΔЎ)) return true;if(!Γΰ(ћ,ΔЎ)){Ǚ=null;Ȩ=0;return true;}if(!ģ) return false;ǒ ǧ=null;float ƺ=0;for(int i=0;i<Ф.Count;i++){ǒ p=Ф[i];if(p.ΰ==ћ||Γΰ(p.ΰ,ΔЎ)) continue;float d=Vector3.Distance(p.ξ,Ο.Ε);if(ǧ==null||d<ƺ){ǧ=p;ƺ=d;}
}if(ǧ==null) return false;Ȩ=ǧ.ΰ;Ǚ=ΓΙ(ǧ.ξ,ǧ.Ы,ǧ.Б);return true;}String Θα(){String s=(ģ?"1":"0")+":"+σ(ΒΟ)+":"+σ(ΒΞ)+":"+
σ(ΒΠ)+":"+σ(ΒΡ)+":"+ћ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].ΰ+","+σ(Ф[i].ξ)+","+σ(Ф[i].Ы)+","+σ(Ф[i].Б);return s;}void Θΰ(String s){Ф.Clear();ģ=false;ћ=0;String[]f=s.Split(':');if(f.Length<6) return;ģ=f[0]=="1";ΒΟ=ς(f[1]);ΒΞ=ς(f[2]);ΒΠ=ς(f[3]);ΒΡ=ς(f[4]);long.TryParse(f[5],out ћ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǒ p=new ǒ();if(!long.TryParse(e[0],out p.ΰ)) continue;p.ξ=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ф.Add(p);}}const String Ƣ="[PAMDCK]";const float ΘΝ=120f;class Ȯ{public long Δ;public int ǖ;public long њ;public bool Ζς;public DateTime ņ,ΗΨ;
}IMyBroadcastListener ΒΝ=null;List<Ȯ>Х=new List<Ȯ>();List<IMyShipConnector>đ=new List<IMyShipConnector>();List<IMySensorBlock>Ȭ=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΕ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΒЈ=new List<MyDetectedEntityInfo>();long Ǿ=0;int ΔΝ=0;
long Ζτ=0;DateTime Ζυ=DateTime.Now;String ń="";int ΖЌ=0;IMyCubeGrid Ē=null;void ΙΕ(){Ē=Me.CubeGrid;χ.GetBlocksOfType(đ,q=>q.CubeGrid.IsSameConstructAs(Ē)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȭ,q=>q.CubeGrid.IsSameConstructAs(Ē)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΕ,q=>q.CubeGrid.IsSameConstructAs(Ē)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȭ.Count;i++){Ȭ[i].DetectSmallShips=true;Ȭ[i].DetectLargeShips=true;}for(int i=0;i<ΑΕ.Count;i++) ΑΕ[i].EnableRaycast=true;}IMyShipConnector ΖΪ(long ΰ){if(ΰ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(ΰ) as IMyShipConnector;return c!=null&&Ē!=null&&c.CubeGrid.IsSameConstructAs(Ē)?c:null;}bool Γί(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΒ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΕ.Count;i++)
{IMyCameraBlock ǽ=ΑΕ[i];if(!ǽ.IsWorking||Vector3D.Distance(ǽ.GetPosition(),c.GetPosition())>10) continue;Vector3D ă=ǽ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ǽ.CanScan(ă)) continue;MyDetectedEntityInfo Ȣ=ǽ.Raycast(ă);if(!Ȣ.IsEmpty()&&Ȣ.EntityId!=Ē.EntityId&&Ȣ.EntityId!=Δ&&Ȣ.Type!=MyDetectedEntityType.FloatingObject)
{ń="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΘЁ(){for(int s=0;s<Ȭ.Count;s++){if(!Ȭ[s].IsWorking) continue;ΒЈ.Clear();Ȭ[s].DetectedEntities(ΒЈ);for(int i=0;i<ΒЈ.Count;i++){MyDetectedEntityInfo e=ΒЈ[i];bool ΙЇ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΙЇ&&e.EntityId!=Ǿ&&e.Velocity.Length()>0.5f){ń="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΘЊ(Ȯ r){IMyShipConnector њ=ΖΪ(r.њ);if(Γί(њ)&&ΖΒ(њ,r.Δ)) return њ.EntityId;if(!r.Ζς||њ==null) return 0;IMyShipConnector ǧ=null;double ƺ=0;for(int i=0;i<đ.Count;i++)
{IMyShipConnector c=đ[i];if(!Γί(c)) continue;double d=Vector3D.Distance(c.GetPosition(),њ.GetPosition());if(ǧ==null||d<ƺ){if(ΖΒ(c,r.Δ)){ǧ=c;ƺ=d;}}}return ǧ==null?0:ǧ.EntityId;}void Θτ(){Ȯ Ɗ=null;long О=0;for(int œ=2;œ>=1&&Ɗ==null;œ--){Ȯ Œ=null;for(int i=0;i<Х.Count;i++)
{Ȯ r=Х[i];if(r.ǖ!=œ) continue;if(œ==1){long c=ΘЊ(r);if(c==0) continue;if(Œ==null||r.ņ<Œ.ņ){Œ=r;О=c;}}else if(Œ==null||r.ņ<Œ.ņ) Œ=r;}Ɗ=Œ;}if(Ɗ==null){if(Х.Count>0&&ń=="") ń="No free dock";return;}Ǿ=Ɗ.Δ;ΔΝ=Ɗ.ǖ;Ζτ=Ɗ.ǖ==1?О:0;Ζυ=DateTime.Now;
}void ΙΧ(){if(Ē==null) return;if(ΒΝ==null) ΒΝ=IGC.RegisterBroadcastListener(Ƣ);int ā=30;while(ΒΝ.HasPendingMessage&&ā-->0){String Ξ=ΒΝ.AcceptMessage().Data as String;if(Ξ!=null) Θυ(Ξ);}ΘЏ();if(++ΖЌ<3) return;ΖЌ=0;IGC.SendBroadcastMessage(Ƣ,ΘΜ());}void Θυ(String Ξ){try{String[]f=Ξ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Х.Count-1;i>=0;i--) if(Х[i].Δ==Δ) Х.RemoveAt(i);if(Ǿ==Δ) Ǿ=0;}else if(f[0]=="R"&&f.Length>=5){long њ=long.Parse(f[3]);if(ΖΪ(њ)==null) return;Ȯ r=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Δ) r=Х[i];if(r==null){r=new Ȯ();r.Δ=Δ;r.ņ=DateTime.Now;Х.Add(r);}
int ǖ=int.Parse(f[2]);if(r.ǖ!=ǖ) r.ņ=DateTime.Now;r.ǖ=ǖ;r.њ=њ;r.Ζς=f[4]=="1";r.ΗΨ=DateTime.Now;}}catch{}}void ΘЏ(){for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].ΗΨ).TotalSeconds>3) Х.RemoveAt(i);Ȯ ΒΒ=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Ǿ) ΒΒ=Х[i];if(Ǿ!=0&&ΒΒ==null) Ǿ=0;if(ΒΒ!=null&&(DateTime.Now-Ζυ).TotalSeconds>ΘΝ){Ǿ=0;ΒΒ.ņ=DateTime.Now;}ń="";if(Ǿ==0&&Х.Count>0&&!ΘЁ()) Θτ();
}String ΘΜ(){String δ="S;"+Ē.EntityId+";"+Ǿ+";"+ΔΝ+";"+Ζτ+";";for(int i=0;i<Х.Count;i++) δ+=(i>0?",":"")+Х[i].Δ;δ+=";";for(int i=0;i<đ.Count;i++){IMyShipConnector c=đ[i];δ+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}δ+=";";for(int i=0;i<Ȭ.Count;i++){IMySensorBlock z=Ȭ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();δ+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return δ;}String Θκ()
{if(đ.Count==0&&Ȭ.Count==0) return "";int ǈ=0;for(int i=0;i<đ.Count;i++) if(Γί(đ[i])) ǈ++;return "Docks: "+ǈ+"/"+đ.Count+" free"+(Ǿ!=0?(ΔΝ==1?" | docking":" | undocking"):"")+
(Х.Count>1?" | "+(Х.Count-1)+" waiting":"")+(ń!=""?"\n"+ń:"")+"\n";}IMyBroadcastListener Βϋ=null;int ȡ=0;DateTime ΖЋ=DateTime.Now;DateTime Ηώ=DateTime.MinValue;long ΗϏ=0;int ΗЀ=0;long ƕ=0;bool Αΰ=false;int ΔΩ=0;List<Vector3>Αβ=new List<Vector3>();List<float>Εη=new List<float>();
bool Ŷ=false;bool ΒΜ=false;Vector3 ΖЊ=new Vector3();bool ư(){return ȡ!=0&&(DateTime.Now-Ηώ).TotalSeconds<3;}bool Ĝ(int ǖ){return ư()&&ΗϏ==ǘ&&ΗЀ==ǖ;}bool ΖΫ(){return ư()||(DateTime.Now-ΖЋ).TotalSeconds<3;}void Γφ(int ǖ){if(ȡ==ǖ) return;ȡ=ǖ;ΖЋ=DateTime.Now;ΔΩ=3;
}void Γυ(){if(ȡ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ƣ,"D;"+ǘ);ȡ=0;Αΰ=false;}void ΙΦ(){if(!trafficEnabled||Κ==Ι.έ) return;if(Βϋ==null) Βϋ=IGC.RegisterBroadcastListener(Ƣ);int ā=20;while(Βϋ.HasPendingMessage&&ā-->0){String Ξ=Βϋ.AcceptMessage().Data as String;if(Ξ==null||!Ξ.StartsWith("S;")) continue;
try{ΘЈ(Ξ);}catch{}}if(Ϊ!=Β.Л){Ŷ=false;ΒΜ=false;}if(ȡ==1&&Я(MyShipConnectorStatus.Connected)!=null) Γυ();if(ȡ==2&&!Αΰ&&Ϊ!=Β.Ǧ&&
Vector3.Distance(ε,ъ().Ε)>ο*3) Γυ();if(ȡ!=0&&++ΔΩ>=3){ΔΩ=0;IGC.SendBroadcastMessage(Ƣ,"R;"+ǘ+";"+ȡ+";"+ћ+";"+(ģ?1:0));}}void ΘЈ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<7) return;bool ΗΔ=false;String[]Ζβ=f[5].Split(',');for(int i=0;i<Ζβ.Length;i++) if(Ζβ[i]==ǘ.ToString()) ΗΔ=true;
if(!ΗΔ) return;Ηώ=DateTime.Now;ΗϏ=long.Parse(f[2]);ΗЀ=int.Parse(f[3]);ƕ=long.Parse(f[4]);Αβ.Clear();Εη.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αβ.Add(ς(e[0]));Εη.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΖЎ=f[6].Split(':');List<ǒ>Ѝ=new List<ǒ>();for(int i=0;i<ΖЎ.Length;i++){String[]e=ΖЎ[i].Split(',');if(e.Length<4) continue;ǒ p=new ǒ();p.ΰ=long.Parse(e[0]);p.ξ=ς(e[1]);p.Ы=ς(e[2]);p.Б=ς(e[3]);Ѝ.Add(p);}Ф=Ѝ;}bool ΘΤ(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;
Γφ(1);if(!ΖΫ()) return true;if(!Ĝ(1)){Λ="Waiting for dock clearance";return false;}ΒЌ();return true;}bool ΒЌ(){ǀ=false;ǒ Ŕ=null;if(ƕ!=0&&ƕ!=ћ&&ģ)
for(int i=0;i<Ф.Count;i++) if(Ф[i].ΰ==ƕ) Ŕ=Ф[i];long Ɲ=Ŕ==null?0:Ŕ.ΰ;if(Ɲ==Ȩ&&(Ɲ==0)==(Ǚ==null)) return false;Ȩ=Ɲ;Ǚ=Ŕ==null?null:ΓΙ(Ŕ.ξ,Ŕ.Ы,Ŕ.Б);return true;}bool ŧ(Vector3 p){for(int i=0;i<Αβ.Count;i++) if(Vector3.Distance(p,Αβ[i])<Εη[i]+ο*0.75f) return true;
return false;}bool Θψ(Vector3 ƽ,Vector3 ő){Ŷ=false;if(!trafficEnabled||Κ==Ι.έ||ћ==0) return false;if(ȡ!=1&&Vector3.Distance(ε,Ο.Ε)>dockRequestDistance) return false;Γφ(1);if(!ư()||Ĝ(1)||Αβ.Count==0) return false;if(!ŧ(ƽ)&&!ŧ(ő)&&!ŧ(ε)) return false;Ŷ=true;return true;
}bool Ες(){if(!trafficEnabled||Κ==Ι.έ||ћ==0) return true;Γφ(2);if(!ΖΫ()) return true;if(!Ĝ(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΘЂ=600f;class Ȱ{public long ΰ;public int У=-1;public bool Ɲ;public float ǉ,ǅ;public DateTime Ηχ;}class Ȗ{public String ȥ;
public int w,h;public float ΒЂ,Βύ;public String ώ;public List<int>ΔЉ=new List<int>();public List<Ȱ>ц=new List<Ȱ>();}List<Ȗ>Ȅ=new List<Ȗ>();IMyBroadcastListener Βί=null;int ΗΥ=0;void ΙΫ(){if(Βί==null) Βί=IGC.RegisterBroadcastListener(ΓЄ);int ā=50;while(Βί.HasPendingMessage&&ā-->0)
{String Ξ=Βί.AcceptMessage().Data as String;if(Ξ!=null) try{Θχ(Ξ);}catch{}}for(int j=Ȅ.Count-1;j>=0;j--){Ȗ Χ=Ȅ[j];for(int i=Χ.ц.Count-1;i>=0;i--)
if((DateTime.Now-Χ.ц[i].Ηχ).TotalSeconds>ΘЂ) Χ.ц.RemoveAt(i);if(Χ.ц.Count==0){Ȅ.RemoveAt(j);continue;}ΘΘ(Χ);}if(++ΗΥ<3) return;ΗΥ=0;for(int j=0;j<Ȅ.Count;j++) IGC.SendBroadcastMessage(ΖΔ,ΘΛ(Ȅ[j]));}void ΘΘ(Ȗ Χ){for(int i=0;i<Χ.ц.Count;i++){Ȱ s=Χ.ц[i];if(s.Ɲ&&(s.У<0||Χ.ώ[s.У]=='1')) s.У=ΘЄ(Χ,s);
}}String ΘΛ(Ȗ Χ){String δ="J;"+Χ.ȥ+";"+Χ.ώ+";";for(int i=0;i<Χ.ц.Count;i++) δ+=(i>0?",":"")+Χ.ц[i].ΰ+":"+Χ.ц[i].У;return δ;}void Θχ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;String[]k=f[4].Split('|');if(k.Length<8) return;Ȗ Χ=null;
for(int i=0;i<Ȅ.Count;i++) if(Ȅ[i].ȥ==f[4]) Χ=Ȅ[i];if(Χ==null){Χ=new Ȗ();Χ.ȥ=f[4];Χ.w=int.Parse(k[3]);Χ.h=int.Parse(k[4]);Χ.ΒЂ=float.Parse(k[6]);Χ.Βύ=float.Parse(k[7]);if(Χ.w<1||Χ.h<1) return;Ελ(Χ.w,Χ.h,(ь) int.Parse(k[5]),Χ.ΔЉ);Χ.ώ=new String('0',Χ.w*Χ.h);Ȅ.Add(Χ);
}String d=f[6];if(d.Length==Χ.ώ.Length){char[]c=Χ.ώ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Χ.ώ=new String(c);}long ΰ=long.Parse(f[0]);Ȱ s=null;for(int i=0;i<Χ.ц.Count;i++) if(Χ.ц[i].ΰ==ΰ) s=Χ.ц[i];if(s==null){s=new Ȱ();s.ΰ=ΰ;Χ.ц.Add(s);}
s.Ηχ=DateTime.Now;String[]w=f[10].Split(',');s.Ɲ=w[0]=="1";if(w.Length>=3){s.ǉ=float.Parse(w[1]);s.ǅ=float.Parse(w[2]);}int Ć=int.Parse(f[5]);if(Ć>=0&&Ć<Χ.ώ.Length&&(s.У<0||Χ.ώ[s.У]=='1')&&Χ.ώ[Ć]!='1'&&ΕЊ(Χ,Ć,s)==null)
s.У=Ć;}Ȱ ΕЊ(Ȗ Χ,int У,Ȱ Δή){for(int i=0;i<Χ.ц.Count;i++){Ȱ o=Χ.ц[i];if(o!=Δή&&o.У>=0&&ΕЋ(У,o.У,Χ.w,Χ.ΒЂ,Χ.Βύ,Math.Max(o.ǉ,Δή.ǉ),Math.Max(o.ǅ,Δή.ǅ)))
return o;}return null;}int ΘЄ(Ȗ Χ,Ȱ s){for(int k=0;k<Χ.ΔЉ.Count;k++){int c=Χ.ΔЉ[k];if(Χ.ώ[c]!='1'&&ΕЊ(Χ,c,s)==null) return c;}return-2;}String Θο(){String s="";for(int j=0;j<Ȅ.Count;j++){Ȗ Χ=Ȅ[j];int ώ=0;for(int i=0;i<Χ.ώ.Length;i++) if(Χ.ώ[i]=='1') ώ++;s+="Job "+(j+1)+": "+ώ+"/"+Χ.ώ.Length+" holes, "+Χ.ц.Count+" ships\n";
}return s;}