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
public enum Κ{ħ,ƶ,Ǫ,ї,ή}public enum Α{ь,Ą,Ų,Ȟ,ĥ,И,у,
Ĥ,ŭ,ǿ,Ł,Ľ,Ġ,
Ƹ,Ȣ,ǭ,ŀ,ΒΙ,Αϋ,
ΔΕ,ΒΘ,Ʈ,Αύ,Ĩ}public enum Π{э,ų,В,ΓΩ,Αφ,ΑЈ,ΑЉ,Ũ,ũ}public enum і{None,ŀ,ь,ΕЅ}public enum Σ{Main,Ζΰ,Ƶ,ƴ,ΓА,Ατ,
ΓΘ,ΖΙ,ΖΚ,ΖΖ,ƺ,Ζε,
ΒΑ,ΓЇ,ΓЋ,Ζη,ΒΓ}public enum х{ΑБ,Ƭ,Ιλ,ΖΝ,ή}public enum ю{Ƽ,Εώ}public enum ъ{ΑА,Γί,Ζκ,ư}public enum Г{Γχ,ΕЂ,ΓΤ,ΓΪ,Αω,Γι,ΑЃ}public enum ǔ{ΓΦ,ΓΝ,ΓΞ}public enum Ш{Ű,ΙΙ,ľ}public enum ρ{Γω,Γύ,Γώ,ΖΫ,ΖΪ,
Γψ,ΑЍ,ΑЌ,Γϋ,
Γό,Γϊ}public enum ε{ș,ć,Ș,Ασ,ΓΛ,ΓΜ}public enum Ǭ{ΑЀ,Ας,ΘЁ}public enum ј{ΒΔ,ΕЇ,ΙΤ,Γφ}public enum Љ{None,Ŵ,Ʒ,ΑЋ}public enum ϋ{ƾ,Αρ,ΑІ,ư,Γτ,ΑЇ,Γυ,Ις,ΓϏ}public enum С{ħ,ΖΧ,ΖΦ,Γρ,Γλ,ΓΥ,ΖΨ}class ά{public bool κ=false;public Vector3 Ε=new Vector3();
public Vector3 η=new Vector3();public Vector3 θ=new Vector3();public Vector3 Ё=new Vector3();public Vector3 φ=new Vector3();public Vector3 о=new Vector3();public float Ц=0;public float Λι=0;public float[]Ǽ=null;public ά(){}public ά(ά λ){κ=λ.κ;Ε=λ.Ε;η=λ.η;θ=λ.θ;Ё=λ.Ё;φ=λ.φ;
о=λ.о;Ǽ=λ.Ǽ;}public ά(Vector3 Ε,Vector3 θ,Vector3 η,Vector3 Ё,Vector3 φ){this.Ε=Ε;this.η=η;this.θ=θ;this.Ё=Ё;this.Ц=0;this.φ=φ;}public void Θμ(List<IMyThrust>ΒΜ,List<string>Ȳ){Ǽ=new float[Ȳ.Count];for(int i=0;i<Ǽ.Length;i++)
Ǽ[i]=-1;for(int i=0;i<ΒΜ.Count;i++){string ǒ=ΖΔ(ΒΜ[i]);int Β=Ȳ.IndexOf(ǒ);if(Β!=-1)
Ǽ[Β]=Й(ΒΜ[i].MaxEffectiveThrust,ΒΜ[i].MaxThrust);}}}class ΓЀ{public ά ζ=null;public List<Vector3>Ơ=new List<Vector3>();public float Ǌ=0;public float Εμ=0;public float Ż=0;public float Δν=0;public Vector3 Βδ=new Vector3();}class ŷ{public ρ Р=ρ.Γω;public float Ǳ=0;public float į=0;
public string ǲ="";public string ǵ="";DateTime Ηλ;public bool ΕΙ=false;private bool Δΰ=false;public bool ΓΡ(bool ǹ){if(ǹ){į=0;Δΰ=false;return false;}Δΰ=true;return į>Ǳ;}public void Ιά(){ΓΡ(true);ΕΙ=false;}public void ΙЂ(){if(Δΰ)
if((DateTime.Now-Ηλ).TotalSeconds>1){į++;Ηλ=DateTime.Now;}}public bool Γζ(){switch(Р){case ρ.Γύ:return true;case ρ.Γώ:return true;}return false;}}class м{public String ΰ="";public String υ="";public int Њ=0;public Ш Ɗ=Ш.ľ;public м(String ΰ,String υ,int Њ,Ш Ɗ){this.ΰ=ΰ;this.υ=υ;
this.Њ=Њ;this.Ɗ=Ɗ;}}class Θό{public Θό(Vector3 ι,float ϊ){this.ι=ι;this.ϊ=ϊ;}public Vector3 ι;public float ϊ;}class Ѕ{public DateTime ΔЀ;public DateTime ΖЉ;public String Υ="";public String ΰ="";public String ΓΕ="";public String Λ="";public String ΕΫ="";public String Βϋ="";public String ΕΘ="";
public Vector3 Ε=new Vector3();public List<м>Ћ=new List<м>();public Κ Μ=Κ.ħ;public Љ Ī;public float ȏ;public float Ȉ;public int Ў;public int Δζ=0;public int Εη=0;public bool ΖΡ(){return(DateTime.Now-ΔЀ).TotalSeconds>10;}public bool Γμ(){if(Ī!=Љ.Ŵ) return false;return(DateTime.Now-ΖЉ).TotalSeconds>=4;
}public Ѕ(String Υ){this.Υ=Υ;}public Љ ń(String І,bool ΚЌ,bool Ъ,bool ΚΑ){if(І==""&&!Ъ) return Љ.None;if(Ī==Љ.Ŵ&&Γμ())
Ī=Љ.ΑЋ;if(ΚЌ){ΕΘ=І;Ī=Љ.Ŵ;ΖЉ=DateTime.Now;return Ī;}else if(Ъ){ΕΘ="";Ī=Љ.None;}else if(ΕΘ==І){if(ΚΑ) Ī=Љ.Ʒ;return Ī;}return Љ.None;}}public String ΘЉ(String Κε){switch(Κε){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΒЂ=new Vector3();Vector3 δ=new Vector3();Vector3 Βη=new Vector3();Vector3 ΑΤ=new Vector3();Vector3 ΑΑ=new Vector3();
DateTime ΔЃ=new DateTime();bool č=true;int Εα=0;bool ē=false;bool ŕ=false;bool ΑΪ=true;bool Θΰ=false;bool ƈ=false;bool Βμ=false;float Ě=0;float Ţ=0;int ĸ=0;int Ακ=0;int Αή=0;int ΔΧ=0;float ƍ=0;float ż=0;double ΔЎ=0;float ǃ=0;List<int>ƅ=new List<int>();List<int>Ƙ=new List<int>();
Κ Μ=Κ.ħ;int А=0;float ǽ=0;float ȗ=0;float ν=0;float ΒЅ=0,ΕΤ=0;IMyRemoteControl Ϋ;IMySensorBlock ψ;List<IMyTimerBlock>Εγ=new List<IMyTimerBlock>();List<IMyShipConnector>љ=new List<IMyShipConnector>();List<IMyThrust>ȳ=new List<IMyThrust>();List<IMyGyro>Βθ=new List<IMyGyro>();
List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȯ=new List<IMyLandingGear>();List<IMyReactor>ķ=new List<IMyReactor>();List<IMyConveyorSorter>ΒЈ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǜ=new List<IMyGasTank>();List<IMyTerminalBlock>ƿ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ſ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŋ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ĩ=new List<IMyBatteryBlock>();List<IMyTextPanel>Х=new List<IMyTextPanel>();List<IMyTextSurface>Ȕ=new List<IMyTextSurface>();List<IMyTextPanel>Ŏ=new List<IMyTextPanel>();
IMyTerminalBlock Ǡ=null;ά Ο=new ά();ά Δϊ=new ά();ά Δύ=new ά();bool Ā=false;bool ƥ=false;bool Ʀ=false;bool Βξ=false;List<String>Ȳ=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Αο=new List<ά>();int Δη=0;double Θα=0;ά Ζ=new ά();Vector3 Ǟ;Vector3 ė;Π γ=Π.э;ю Ŗ=ю.Ƽ;int Ќ=0;
int Ю=0;double ΑΠ=0;bool ΒΪ=false;ŷ Ї=new ŷ();ŷ Ј=new ŷ();ъ ђ=ъ.ΑА;bool Ļ=true;ю Ȓ=ю.Ƽ;int ѐ=3;int я=3;int н=30;ǔ њ=ǔ.ΓΦ;bool ğ=true;bool Ƈ=true;bool Ɓ=true;bool ţ=false;Г д=Г.Γχ;bool Ğ=true;float Ȏ=90;float ў=20;float п=5;float џ=20;float Ȃ=0.70f;float ƪ=1.50f;
float Ʃ=2.50f;float ŧ=0f;float Œ=0f;Α Ϊ;Π ѝ;Α Αγ;ΓЀ ξ=null;int Ѐ=0;int ύ=0;int б=0;int ĕ=0;int Š=30;int к=0;bool Е=true;Vector3 Ǎ;double в=0;bool ΒЄ=false;int Εο=0;int Ж=0;int ΔΜ=0;int Ȑ=0;int ΗЃ=0;Vector3 Αά=new Vector3();float ё=0;float т=0;int Ŧ=0;ά ΒЉ=null;
float ΔЁ=0;float ΒΞ=0;float й=0;float ř=0;bool Δϋ=false;bool Ť=false;bool Αλ=false;bool đ=false;bool Ċ=false;DateTime ΚΣ=new DateTime();ά Ō=null;ά ΖБ=null;float Ȥ=0;і ȃ;float Ē=0;float ť=0;String ΔЉ="";float Βς=0;float Ȉ=0;float ȏ=0;List<м>Ǵ=new List<м>();float е=0;
bool Αΰ=false;bool ΘΥ=false;bool ΗА=false;bool ΒЎ=false;bool ΗБ=false;float ΔЌ=0;float ΕΗ=0;Vector3 ΗΤ=new Vector3();Vector3 Ђ=new Vector3();Vector3 ΒЏ=new Vector3(1,1,1);float Εΰ=1;Vector3 Αη=new Vector3();bool Βν=false;bool Δσ=false;bool ΔІ=false;bool Ζυ=false;float ΒΨ=0;
float ΒΛ=2;Vector3 ΘΠ;Vector3 Εί;Vector3 Εή;float[,]Ǿ=new float[3,2];Dictionary<String,float[,]>ǥ=new Dictionary<string,float[,]>();Dictionary<String,float>Θή=new Dictionary<String,float>();int ΔΠ=0;Dictionary<String,float[]>Ę=new Dictionary<string,float[]>();float ΕΚ;bool ΒΫ=false;
int ΒΝ=0;int Ĵ=0;int Βκ=0;int œ=0;String Λ="";int[]Ηω=new int[Enum.GetValues(typeof(Σ)).Length];bool ΔА=false;int Ś=0;int Ў=0;Σ ό=Σ.Main;int ΛΚ=0,ΛΛ=0,ΛΜ=0,ΛΝ=0;int ΚЇ=0,ΚЈ=0;bool ž=false;bool Ζφ=false;String Ɲ="";String ΔΦ="";IMyBroadcastListener ǯ=null;
bool ΗΣ=true;List<Ѕ>Ь=null;Ѕ Л=null;int ΒΣ=0,ΒΤ=0;int ΖЌ=0,ΖЍ=0;int ΚΗ=0,ΚΘ=0,ΚΙ=0;String Κη="";Exception Ĕ=null;bool ΒЃ=false;Π ƚ=Π.э;bool ΗΓ=false;bool Ηι=true;bool ΕΟ=false;String Θί="";bool Αι=false;bool ΕΪ=true;int Βλ=0;int[]ǣ=null;Vector3 ΔΨ=new Vector3();
bool ΗΩ=false;bool Ǖ(IMyTerminalBlock Ω)=>Ω.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Γη(Me,controllerTag,true))
Μ=Κ.ї;μ(Σ.Main);if(Μ!=Κ.ї){Λ="Welcome to [PAM]!";ј ΕΞ=Ιέ();if(Μ==Κ.ħ){List<IMyShipDrill>ΗΟ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗΪ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗΟ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗΪ,q=>q.CubeGrid==Me.CubeGrid);if(ΗΟ.Count>0)
{Μ=Κ.ƶ;Λ="Miner mode enabled!";}else if(ΗΪ.Count>0){Μ=Κ.Ǫ;Λ="Grinder mode enabled!";}else{Μ=Κ.ή;ȟ(х.ή);}}if(ΕΞ==ј.ΒΔ) ΑΪ=false;if(ΕΞ==ј.ΕЇ) Λ="Data restore failed!";if(ΕΞ==ј.Γφ) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΛΧ){try{if(Ĕ!=null){if(Т.Trim().ToUpper()=="RETRY")
{Ĕ=null;č=true;ĸ=0;if(Μ!=Κ.ї) Ѓ();Λ="Restarted after error";}else{ΙΓ();Ζή();return;}}ŕ=(ΛΧ&UpdateType.Update10)!=0;if(ŕ) Εα++;ē=Εα>=10;if(ē) Εα=0;if(ŕ){ΒΝ++;if(ΒΝ>4) ΒΝ=0;Αή=(int) Math.Max(0,10-(DateTime.Now-ΔЃ).TotalSeconds);}if(Т!="") Κη=Т;if(Μ!=Κ.ї)
Ικ(Т);else
Θϊ(Т);ΔА=false;try{int ΗΑ=Runtime.CurrentInstructionCount;float ş=Й(ΗΑ,Runtime.MaxInstructionCount);if(ş>0.90) Λ="Max. instructions >90%";if(ş>ƍ) ƍ=ş;if(ΒΫ){ƅ.Add(ΗΑ);while(ƅ.Count>10) ƅ.RemoveAt(0);ż=0;for(int i=0;i<ƅ.Count;i++) ż+=ƅ[i];ż=Й(Й(ż,ƅ.Count),Runtime.MaxInstructionCount);
double ΔЂ=Runtime.LastRunTimeMs;if(Βμ&&ΔЂ>ΔЎ) ΔЎ=ΔЂ;Ƙ.Add((int)(ΔЂ*1000f));while(Ƙ.Count>10) Ƙ.RemoveAt(0);ǃ=0;for(int i=0;i<Ƙ.Count;i++) ǃ+=Ƙ[i];ǃ=Й(ǃ,Ƙ.Count)/1000f;}}catch{ƍ=0;}}catch(Exception e){Ĕ=e;}}void Ικ(string Т){bool ΒІ=false;String ΖЀ="";if(А<=1&&!Γο(Т))
Ζί(Т);int ΔБ=50;while(ǯ!=null&&ǯ.HasPendingMessage&&ΔБ-->0){MyIGCMessage β=ǯ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;String ȑ="";if(А<=1&&Ζέ(ref Θ,out ȑ,out ΖЀ)&&ȑ==BROADCAST_SELF_ID){Ζί(Θ);ΒІ=true;break;}}bool Ηγ=Βμ&&Ϊ==Α.ь&&!ƈ&&!ΒІ&&ĸ==0&&!Βξ;
if(ē&&Ϊ!=Α.ь) ΒІ=true;if((ŕ&&!Ηγ)||(ē&&Ηγ)){if(ĸ==0&&(Αή<=0||č)){Θΰ=А>0;А=0;ĸ=1;Ƞ();Ια();ΖΛ();Ț("Scan 1");}else if(ĸ==1){ĸ=2;Ƞ();Ιβ();Ț("Scan 2");}else if(ĸ==2){ĸ=0;Ƞ();Ιγ();Ιδ();if(А<=1) ΖΥ();Ț("Scan 3");ΔЃ=DateTime.Now;if(А<=1&&ΑΪ)
ΒЂ=ΑЁ(Ϋ,Ϋ.CenterOfMass);ΑΪ=false;if(č){č=false;bool ΛΫ=γ==Π.В;Ѓ();if(Ηι&&resumeAfterRestart&&ΛΫ&&А<=1) ΕΟ=true;}Ηι=false;if(Θΰ&&А==0) Λ="Setup complete";}else{if(γ==Π.В&&Μ!=Κ.ή){Ƞ();Θε();Ț("Inv balance");}Ƞ();switch(Ακ){case 0:Ιφ();break;case 1:Ιό();break;case 2:Ιυ();break;
case 3:ΙІ();break;case 4:Ιϋ();break;case 5:Θο();break;case 6:Θξ(Ϋ);break;}Ț("Update: "+Ακ);Ακ++;if(Ακ>6){Ακ=0;Βμ=true;if(ƚ!=Π.э){switch(ƚ){case Π.Ũ:Ģ();break;case Π.ũ:Ģ();break;case Π.В:Ģ();break;case Π.ΑЈ:Αώ();break;case Π.ΑЉ:ł();break;}ƚ=Π.э;}}}if(!č){if(!Ȝ(Ϋ,true))
{Ϋ=null;ΑΪ=true;А=2;}if(А>=2&&Ϊ!=Α.ь)
Ѓ();if(А<=1){Ţ=Ϋ.CalculateShipMass().PhysicalMass;Ě=(float) Ϋ.GetShipSpeed();δ=Ιτ(Ϋ,ΒЂ);Βη=Ϋ.WorldMatrix.Forward;ΑΤ=Ϋ.WorldMatrix.Left;ΑΑ=Ϋ.WorldMatrix.Down;ΙЀ();ΙЃ();Ιψ();if(ΕΟ){ΕΟ=false;Ģ();if(Ϊ!=Α.ь) Λ="Job resumed after restart";}if(Ϊ!=Α.ь){ƈ=false;Ζβ(false);Εό(false);
String ΚБ=Γβ(Ϊ)+" "+(int) Ϊ;Ƞ();Ιώ();ΔΔ(false);Ț(ΚБ);ΙЄ();ΙϏ();Ƞ();Ιϊ();Ț("Thruster");Ƞ();Ζρ();Ț("Gyroscope");}else{if(ƈ){if(ΑЅ()){л(ΑΑ,Βη,ΑΤ,0.25f,true);Ζρ();Λ="Aligning to planet: "+Math.Round(ΒΨ-0.25f,2)+"°";if(Βν) Γε(true,true);}else Γε(true,true);}}Αι=false;}}}Ƞ();Ζή();Ț("Print");
if(ΒІ||ΔΧ<=0){Ƞ();Θζ(ΖЀ);Ț("Broadcast");ΔΧ=4;}else if(ē) ΔΧ--;}void Ζί(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȣ=τ.ElementAtOrDefault(1);var ǂ=τ.ElementAtOrDefault(2);var Ζψ=τ.ElementAtOrDefault(3);String Ȭ="Invalid argument: "+Т;
bool Ελ=false;switch(І){case "UP":this.Γσ(false);break;case "DOWN":this.Γς(false);break;case "UPLOOP":this.Γσ(true);break;case "DOWNLOOP":this.Γς(true);break;case "APPLY":this.Εϊ(true);break;case "MRES":Ў=0;break;case "STOP":this.Ѓ();break;case "PATHHOME":{this.Ѓ();this.ΓЏ();}break;
case "PATH":{this.Ѓ();this.ΓЏ();Ο.κ=true;}break;case "START":{this.Ѓ();ΓЎ();}break;case "ALIGN":{Γε(!ƈ,false);}break;case "CONT":{this.Ѓ();this.Ģ();}break;case "JOBPOS":{this.Ѓ();this.ł();}break;case "HOMEPOS":{this.Ѓ();this.Αώ();}break;case "FULL":{Αΰ=true;}break;
case "RESET":{ΒЃ=true;А=2;}break;case "EXPORT":{ΘЂ();}break;case "IMPORT":{ΙΗ(ȣ=="JOB");}break;default:Ελ=true;break;}if(Μ!=Κ.ή){switch(І){case "SHUTTLE":{ΘϏ();}break;case "CFGS":{if(!Θψ(ȣ,ǂ,Ζψ)) Λ=Ȭ;}break;case "CFGB":{if(!Θχ(ȣ,ǂ)) Λ=Ȭ;}break;case "CFGL":
{if(!Ń(ref Ȏ,true,ϋ.ΑІ,ȣ,"")||!Θω(ǂ))
Λ=Ȭ;}break;case "CFGE":{if(!Ń(ref п,true,ϋ.ΑЇ,ȣ,"IG")||
!Ń(ref ў,true,ϋ.Γτ,ǂ,"IG")||
!Ń(ref џ,true,ϋ.Γυ,Ζψ,"IG"))
Λ=Ȭ;}break;case "CFGA":{if(!Ń(ref Ȃ,false,ϋ.Αρ,ȣ,"")) Λ=Ȭ;}break;case "CFGW":{if(!Ń(ref ƪ,false,ϋ.ƾ,ȣ,"")||
!Ń(ref Ʃ,false,ϋ.ƾ,ǂ,""))
Λ=Ȭ;}break;case "NEXT":{ΓΗ(false);}break;case "PREV":{ΓΗ(true);}break;default:if(Ελ) Λ=Ȭ;break;}}else{switch(І){case "UNDOCK":{Αι=true;}break;default:if(Ελ) Λ=Ȭ;break;}}}String ΘЊ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΘϏ(){Ѓ();Μ=Κ.ή;ȟ(х.ή);Ζ.κ=false;Ο.κ=false;ψ=null;ω.Clear();γ=Π.э;}void Γε(bool М,bool Ϗ){if(!М) Λ="Aligning canceled";if(Ϗ) Λ="Aligning done";if(Ϗ||!М){ƈ=false;Ζλ();ΒΒ(false,0,0,0,0);return;}if(ΑЅ()) ƈ=true;}bool Θψ(String Λέ,String Κή,String ΚΝ)
{bool ΛΩ=γ==Π.В;int w,h,d;if(int.TryParse(Λέ,out w)&&int.TryParse(Κή,out h)&&int.TryParse(ΚΝ,out d)){this.Ѓ();ѐ=w;я=h;н=d;ň(false);ƽ(false,false);if(ΛΩ) Ģ();return true;}return false;}bool Θω(String ǁ){if(ǁ=="ON"){ğ=true;return true;}if(ǁ=="OFF"){ğ=false;return true;}
return false;}bool Θχ(String ΗΞ,String ΒΩ){bool ȯ=true;if(ΗΞ=="HOME") Ļ=true;else if(ΗΞ=="STOP") Ļ=false;else ȯ=false;if(ΒΩ=="HOME") ђ=ъ.ΑА;else if(ΒΩ=="STOP") ђ=ъ.Ζκ;else if(ΒΩ=="JOB") ђ=ъ.Γί;else if(ΒΩ=="IG") ђ=ъ.ư;else ȯ=false;return ȯ;}void μ(Σ ц){Ηω[(int) ό]=Ў;
Ў=Ηω[(int) ц];if(ц==Σ.ƺ) Ў=0;ό=ц;if(Μ!=Κ.ї)
Ň(ό==Σ.Ƶ,false,0,0);ΔА=true;}void Γσ(bool ΔЇ){if(Ў>0) Ў--;else if(ΔЇ) Ў=Ś-1;}void Γς(bool ΔЇ){if(Ў<Ś-1) Ў++;else if(ΔЇ) Ў=0;}String ȟ(х β){switch(β){case х.ΑБ:Λ="Job is running";break;case х.Ƭ:Λ="Connector not ready!";break;case х.Ιλ:Λ="Ship modified, path outdated!";break;
case х.ΖΝ:Λ="Interrupted by player!";break;case х.ή:Λ="Shuttle mode enabled!";break;}return "";}String ΘА(ю ι){switch(ι){case ю.Ƽ:return "Top-Left";case ю.Εώ:return "Center";default:return "";}}String ΘІ(ǔ Џ){switch(Џ){case ǔ.ΓΝ:return "Auto"+(Μ==Κ.ƶ?" (Ore)":"");case ǔ.ΓΞ:return "Auto (+Stone)";
case ǔ.ΓΦ:return "Default";default:return "";}}String Γΰ(Π Є){switch(Є){case Π.э:return "No job";case Π.ų:return "Job paused";case Π.В:return "Job active";case Π.Ũ:return "Job active";case Π.ũ:return "Job active";case Π.ΓΩ:return "Job done";case Π.Αφ:return "Job changed";case Π.ΑЈ:return "Move home";
case Π.ΑЉ:return "Move to job";}return "";}String ΕЌ(ъ ΙБ){switch(ΙБ){case ъ.ΑА:return "Return home";case ъ.Γί:return "Fly to job pos";case ъ.Ζκ:return "Stop";case ъ.ư:return "Ignore";}return "";}String ΘЈ(Г Џ){switch(Џ){case Г.Γχ:return "Off";case Г.ΓΪ:return "Drop pos (Stone) ";case Г.Αω:return "Drop pos (Sto.+Ice)";
case Г.ΕЂ:return "Cur. pos (Stone)";case Г.ΓΤ:return "Cur. pos (Sto.+Ice)";case Г.Γι:return "In motion (Stone)";case Г.ΑЃ:return "In motion (Sto.+Ice)";}return "";}String ΕЊ(і Є){switch(Є){case і.None:return "No batteries";case і.ŀ:return "Charging";case і.ΕЅ:return "Discharging";}return "";}
String Γβ(Α Є){String Λη=Μ==Κ.ή?"target":"job";switch(Є){case Α.ь:return "Idle";case Α.Ą:return "Flying to XY position";case Α.Ų:return Μ==Κ.Ǫ?"Grinding":"Mining";case Α.Ȟ:return "Returning";case Α.Ʈ:return "Flying to drop pos";case Α.ĥ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.у:return "Flying to job area";case Α.Ĥ:return "Flying to path";case Α.ŭ:return "Flying to job position";case Α.ǿ:return "Approaching dock";case Α.Ł:return "Docking";case Α.Ľ:return "Aligning";case Α.Ġ:return "Aligning";case Α.Ƹ:return "Retry docking";case Α.Ȣ:return "Unloading";case Α.Ĩ:return Θί;
case Α.ǭ:return "Undocking";case Α.ŀ:return "Charging batteries";case Α.ΒΙ:return "Waiting for uranium";case Α.Αϋ:return "Filling up hydrogen";case Α.ΔΕ:return "Waiting for ejection";case Α.ΒΘ:return "Waiting for ejection";case Α.Αύ:return "Flying to drop pos";}return "";}String ΖΕ(ρ Р){switch(Р)
{case ρ.Γω:return "On \"Undock\" command";case ρ.Γύ:return "On player entered cockpit";case ρ.ΖΫ:return "On ship is full";case ρ.ΖΪ:return "On ship is empty";case ρ.Γώ:return "On time delay";case ρ.ΑЍ:return "On batteries empty(<25%)";case ρ.ΑЌ:return "On batteries empty(=0%)";case ρ.Γψ:return "On batteries full";
case ρ.Γό:return "On hydrogen empty(<25%)";case ρ.Γϊ:return "On hydrogen empty(=0%)";case ρ.Γϋ:return "On hydrogen full";}return "";}bool Ν(ref String Γ,int ΗЌ,int ĳ,bool Ι,String ŗ){Ś+=1;if(ΗЌ==ĳ) ŗ=">"+ŗ+(ΒΝ>=2?" .":"");else ŗ=" "+ŗ;Γ+=ŗ+"\n";return ΗЌ==ĳ&&Ι;}String Εϊ(bool Ι)
{int Β=0;int Η=Ў;Ś=0;String Ɯ="———————————————\n";String Ƣ="--------------------------------------------\n";String Γ="";Γ+=Γΰ(γ)+" | "+(Ο.κ?"Ready to dock":"No dock")+"\n";Γ+=Ɯ;double ȓ=Math.Max(Math.Round(this.в),0);if(ό==Σ.Main){bool ĵ=Μ==Κ.ή;if(Ν(ref Γ,Η,Β++,Ι," Record path & set home")) ΓЏ();
if(Μ==Κ.ƶ)
if(Ν(ref Γ,Η,Β++,Ι," Setup mining job")) μ(Σ.Ƶ);if(Μ==Κ.Ǫ)
if(Ν(ref Γ,Η,Β++,Ι," Setup grinding job")) μ(Σ.Ƶ);if(Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Setup shuttle job")) μ(Σ.ΓЋ);if(Ν(ref Γ,Η,Β++,Ι," Continue job")) Ģ();if(Ν(ref Γ,Η,Β++,Ι," Fly to home position")) Αώ();if(Ν(ref Γ,Η,Β++,Ι," Fly to job position")) ł();if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(ĵ) μ(Σ.ΒΓ);else μ(Σ.Ατ);if(Ν(ref Γ,Η,Β++,Ι," Info")) μ(Σ.ΖΙ);if(Μ!=Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Help")) μ(Σ.ΖΖ);}else if(ό==Σ.Ƶ){double Λά=Math.Round(ѐ*ǽ,1);double Κέ=Math.Round(я*ȗ,1);String З="";if(Ν(ref З,Η,Β++,Ι," Start new job!")) ΓЎ();if(Ν(ref З,Η,Β++,Ι," Change current job")){ƽ(false,false);μ(Σ.Main);}if(Ν(ref З,Η,Β++,Ι," Width + (Width: "+ѐ+" = "+Λά+"m)")){Ū(ref ѐ,5,20,1);ň(true);}
if(Ν(ref З,Η,Β++,Ι," Width -")){Ū(ref ѐ,-5,20,-1);ň(true);}if(Ν(ref З,Η,Β++,Ι," Height + (Height: "+я+" = "+Κέ+"m)")){Ū(ref я,5,20,1);ň(true);}if(Ν(ref З,Η,Β++,Ι," Height -")){Ū(ref я,-5,20,-1);ň(true);}if(Ν(ref З,Η,Β++,Ι," Depth + ("+(њ==ǔ.ΓΦ?"Depth":"Min")+": "+н+"m)")){Ū(ref н,5,50,2);ň(true);}
if(Ν(ref З,Η,Β++,Ι," Depth -")){Ū(ref н,-5,50,-2);ň(true);}if(Ν(ref З,Η,Β++,Ι," Depth mode: "+ΘІ(њ))){њ=ģ(њ);}if(Ν(ref З,Η,Β++,Ι," Start pos: "+ΘА(Ȓ))){Ȓ=ģ(Ȓ);}if(Μ==Κ.Ǫ&&њ==ǔ.ΓΞ) њ=ģ(њ);Γ+=ΓЅ(8,З,Η,ref ΚЇ);}else if(ό==Σ.ΓЋ){float[]ΗΕ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.Ζη);if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=" Leave connector 1:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΕ(Ї.Р))) Ї.Р=ģ(Ї.Р);if(!Ї.Γζ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑϏ((int) Ї.Ǳ))) Ї.Ǳ=ΕЃ(Ї.Ǳ,ΗΕ);Γ+=" Leave connector 2:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΕ(Ј.Р))) Ј.Р=ģ(Ј.Р);
if(!Ј.Γζ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑϏ((int) Ј.Ǳ))) Ј.Ǳ=ΕЃ(Ј.Ǳ,ΗΕ);}else if(ό==Σ.Ζη){if(Ν(ref Γ,Η,Β++,Ι," Start job!")) ΓЎ();if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.ΓЋ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.ǲ!=""?Ї.ǲ:"-"))) Ї.ǲ=ΑЊ(ref ΛΚ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ї.ǵ!=""?Ї.ǵ:"-"))) Ї.ǵ=ΑЊ(ref ΛΜ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǲ!=""?Ј.ǲ:"-"))) Ј.ǲ=ΑЊ(ref ΛΛ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǵ!=""?Ј.ǵ:"-"))) Ј.ǵ=ΑЊ(ref ΛΝ);
}else if(ό==Σ.ƴ){String Ηπ=Ȏ+" %";if(ē) Ĵ++;if(Ĵ>1){Ĵ=0;Βκ++;if(Βκ>1) Βκ=0;}bool[]ΕΧ=new bool[]{ķ.Count==0,ȃ==і.None,Ǜ.Count==0};int ΘΪ=0;while(true){ΘΪ++;œ++;if(œ>ΕΧ.Length-1) œ=0;if(ΘΪ>=ΕΧ.Length) break;if(!ΕΧ[œ]) break;}bool ĵ=Μ==Κ.ή;if(!ĵ&&ğ&&е!=-1&&Βκ==0)
Ηπ=е<1000000?Math.Round(е)+" Kg":Math.Round(е/1000)+" t";if(Ν(ref Γ,Η,Β++,Ι," Stop!")){Ѓ();μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(!ĵ) μ(Σ.Ατ);else μ(Σ.ΒΓ);if(!ĵ){if(Ν(ref Γ,Η,Β++,Ι," Next hole")) ΓΗ(false);}else if(Ν(ref Γ,Η,Β++,Ι," Undock")) Αι=true;Γ+=Ƣ;if(!ĵ) Γ+="Progress: "+Math.Round(ΑΠ,1)+" %\n";Γ+="State: "+Γβ(Ϊ)+" "+ȓ+"m \n";Γ+="Load: "+Βς+" % Max: "+Ηπ+" \n";if(œ==0) Γ+="Uranium: "+(ķ.Count==0?"No reactors":Math.Round(ť,1)+"Kg "+(п==-1?"":" Min: "+п+" Kg"))+"\n";
if(œ==1) Γ+="Battery: "+(ȃ==і.None?ΕЊ(ȃ):Ȥ+"% "+(ў==-1||ĵ?"":" Min: "+ў+" %"))+"\n";if(œ==2) Γ+="Hydrogen: "+(Ǜ.Count==0?"No tanks":Math.Round(Ē,1)+"% "+(џ==-1||ĵ?"":" Min: "+џ+" %"))+"\n";}else if(ό==Σ.Ατ){String З="";if(Ν(ref З,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.ƴ);else μ(Σ.Main);}
if(Ν(ref З,Η,Β++,Ι," Max load: "+Ȏ+"%")) щ(ref Ȏ,Ȏ<=80?-10:-5,ϋ.ΑІ,false);if(Ν(ref З,Η,Β++,Ι," Weight limit: "+(ğ?"On":"Off"))) ğ=!ğ;if(Ν(ref З,Η,Β++,Ι," Ejection: "+ΘЈ(д))) д=ģ(д);if(Ν(ref З,Η,Β++,Ι," Toggle sorters: "+(ţ?"On":"Off"))){ţ=!ţ;if(ţ) ȁ(ΕΪ);}
if(Ν(ref З,Η,Β++,Ι," Unload ice: "+(Ğ?"On":"Off"))) Ğ=!Ğ;if(Ν(ref З,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЇ,true);if(Ν(ref З,Η,Β++,Ι," Battery: "+(ў==-1?"Ignore":"Min "+ў+"%"))) щ(ref ў,-5,ϋ.Γτ,true);if(Ν(ref З,Η,Β++,Ι," Hydrogen: "+(џ==-1?"Ignore":"Min "+џ+"%"))) щ(ref џ,-10,ϋ.Γυ,true);
if(Ν(ref З,Η,Β++,Ι," When done: "+(Ļ?"Return home":"Stop"))) Ļ=!Ļ;if(Ν(ref З,Η,Β++,Ι," On damage: "+ΕЌ(ђ))) ђ=ģ(ђ);if(Ν(ref З,Η,Β++,Ι," Advanced...")) μ(Σ.ΓΘ);Γ+=ΓЅ(8,З,Η,ref ΚЈ);}else if(ό==Σ.ΓΘ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.ƴ);else μ(Σ.Main);}
if(Ν(ref Γ,Η,Β++,Ι,(Μ==Κ.Ǫ?" Grinder":" Drill")+" inv. balancing: "+(Ƈ?"On":"Off"))) Ƈ=!Ƈ;if(Ν(ref Γ,Η,Β++,Ι," Enable"+(Μ==Κ.Ǫ?" grinders":" drills")+": "+(Ɓ?"Fwd + Bwd":"Fwd"))) Ɓ=!Ɓ;if(Ν(ref Γ,Η,Β++,Ι," Work speed fwd.: "+ƪ+"m/s")) щ(ref ƪ,0.5f,ϋ.ƾ,false);
if(Ν(ref Γ,Η,Β++,Ι," Work speed bwd.: "+Ʃ+"m/s")) щ(ref Ʃ,0.5f,ϋ.ƾ,false);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȃ*100f)+"%"+(Ȃ>0.80f?" (risky)":""))) щ(ref Ȃ,0.1f,ϋ.Αρ,false);if(Ν(ref Γ,Η,Β++,Ι," Width overlap: "+ŧ*100f+"%")) ΕϏ(true,0.05f);if(Ν(ref Γ,Η,Β++,Ι," Height overlap: "+Œ*100f+"%")) ΕϏ(false,0.05f);
}else if(ό==Σ.ΒΓ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.ƴ);else μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Max load: "+Ȏ+"%")) щ(ref Ȏ,Ȏ<=80?-10:-5,ϋ.ΑІ,false);if(Ν(ref Γ,Η,Β++,Ι," Unload ice: "+(Ğ?"On":"Off"))) Ğ=!Ğ;if(Ν(ref Γ,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЇ,true);
if(Ν(ref Γ,Η,Β++,Ι," Battery: "+(ў==-1?"Ignore":"Charge up"))) ў=(ў==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," Hydrogen: "+(џ==-1?"Ignore":"Fill up"))) џ=(џ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," On damage: "+ΕЌ(ђ))) ђ=ģ(ђ);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȃ*100f)+"%"+(Ȃ>0.80f?" (risky)":""))) щ(ref Ȃ,0.1f,ϋ.Αρ,false);
}else if(ό==Σ.Ζΰ){double Κθ=0;if(Ψ.Count>0) Κθ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ν(ref Γ,Η,Β++,Ι," Stop path recording")) Ζμ();if(Μ!=Κ.ή){if(Ν(ref Γ,Η,Β++,Ι," Home: "+(Ā?"Use old home":(Ο.κ?"Was set! ":"none ")))) Ā=!Ā;}else{if(Ν(ref Γ,Η,Β++,Ι," Connector 1: "+(Ā?"Use old connector":(Ο.κ?"Was set! ":"none ")))) Ā=!Ā;
if(Ν(ref Γ,Η,Β++,Ι," Connector 2: "+(ƥ?"Use old connector":(Ζ.κ?"Was set! ":"none ")))) ƥ=!ƥ;}if(Ν(ref Γ,Η,Β++,Ι," Path: "+(Ʀ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) Ʀ=!Ʀ;Γ+=Ƣ;Γ+="Wp spacing: "+Math.Round(Θα)+"m\n";}else if(ό==Σ.ΓА){if(Ν(ref Γ,Η,Β++,Ι," Stop")){Ѓ();μ(Σ.Main);}
Γ+=Ƣ;Γ+="State: "+Γβ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(Ě,1)+"m/s\n";Γ+="Target dist: "+ȓ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+Εο+"\n";}else if(ό==Σ.ΖΙ){List<IMyTerminalBlock>ō=ΕЍ();if(ē) Ĵ++;if(Ĵ>=ō.Count) Ĵ=0;if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.ΖΚ);
Γ+=Ƣ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βς,1)+"% "+Math.Round(Ȉ,1)+" / "+Math.Round(ȏ,1)+"\n";Γ+="Uranium: "+(ķ.Count==0?"No reactors":Math.Round(ť,1)+"Kg "+ΔЉ)+"\n";Γ+="Battery: "+(ȃ==і.None?"":Ȥ+"% ")+ΕЊ(ȃ)+"\n";Γ+="Hydrogen: "+(Ǜ.Count==0?"No tanks":Math.Round(Ē,1)+"% ")+"\n";
Γ+="Damage: "+(ō.Count==0?"None":""+(Ĵ+1)+"/"+ō.Count+" "+ō[Ĵ].CustomName)+"\n";}else if(ό==Σ.ΖΚ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=Ƣ;Γ+="Next scan: "+Αή+"s\n";Γ+="Drill area: "+Math.Round(ǽ,1)+"x"+Math.Round(ȗ,1)+"m Ship: "+Math.Round(ΒЅ,1)+"x"+Math.Round(ΕΤ,1)+"m ("+Math.Round(ν,1)+"m)\n";
Γ+="Broadcast: "+(ž?"Online - "+Ɲ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ƍ*100f,1)+"% \n";}else if(ό==Σ.ΖΖ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=Ƣ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+Αή+"s\n";if(ΒЃ) Γ="Recompile script now";int Κι=Γ.Split('\n').Length;for(int i=Κι;i<=10;i++) Γ+="\n";Γ+=Ɯ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕϏ(bool Κδ,float Ǥ){Ѓ();ƽ(true,false);if(Κδ) щ(ref ŧ,Ǥ,ϋ.ΓϏ,false);else щ(ref Œ,Ǥ,ϋ.ΓϏ,false);Εύ();Ň(true,true,0,0);}float ΕЃ(float ο,float[]ΓΔ){float ί=ΓΔ[0];for(int i=ΓΔ.Length-1;i>=0;i--)
if(ο<ΓΔ[i]) ί=ΓΔ[i];return ί;}String ΑЊ(ref int ĳ){String ί="";if(ĳ>=Εγ.Count) ĳ=-1;if(ĳ>=0) ί=Εγ[ĳ].CustomName;ĳ++;return ί;}void ΒΖ(string ΰ){if(γ!=Π.В) return;if(ΰ=="") return;IMyTerminalBlock Ω=χ.GetBlockWithName(ΰ);if(Ω==null||!(Ω is IMyTimerBlock)){Λ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ω).Trigger();}void Ū(ref int value,int ǆ,int ĝ,int ΕΨ){if(ǆ==0) return;if(value<ĝ&&ΕΨ>0||value<=ĝ&&ΕΨ<0){value+=ΕΨ;return;}int ΙЈ=Math.Abs(ǆ);int ΔΖ=0;int ΕΑ=1;while(true){ΔΖ+=ΕΑ*ΙЈ*10;if(ǆ<0&&value-ĝ<=ΔΖ) break;if(ǆ>0&&value-ĝ<ΔΖ) break;
ΕΑ++;}value+=ΕΑ*ǆ;}void ň(bool Λκ){ѐ=Math.Max(ѐ,1);я=Math.Max(я,1);н=Math.Max(н,0);Ň(ό==Σ.Ƶ,false,0,0);}bool Ń(ref float value,bool ΚЄ,ϋ υ,String Ŕ,String Κα){if(Ŕ=="") return false;float Ƒ=-1;bool Ηΰ=false;if(Ŕ.ToUpper()==Κα) Ηΰ=true;else if(!float.TryParse(Ŕ,out Ƒ)) return false;
else Ƒ=Math.Max(0,Ƒ);if(ΚЄ) Ƒ=(float) Math.Round(Ƒ);щ(ref value,Ƒ,υ,Ηΰ,false);return true;}void щ(ref float value,float Ǥ,ϋ υ,bool Αθ){щ(ref value,value+Ǥ,υ,Αθ,true);}void щ(ref float value,float Ǝ,ϋ υ,bool Αθ,bool Λα){float Ƿ=0,Ƕ=0;if(υ==ϋ.ƾ){Ƿ=0.5f;Ƕ=10f;}if(υ==ϋ.Αρ){Ƿ=0.1f;Ƕ=1f;}
if(υ==ϋ.ư){Ƿ=50f;Ƕ=100f;}if(υ==ϋ.Γτ){Ƿ=5f;Ƕ=30f;}if(υ==ϋ.ΑЇ){Ƿ=1f;Ƕ=25f;}if(υ==ϋ.ΑІ){Ƿ=10f;Ƕ=95f;}if(υ==ϋ.Γυ){Ƿ=10f;Ƕ=90f;}if(υ==ϋ.Ις){Ƿ=10f;Ƕ=1800;}if(υ==ϋ.ΓϏ){Ƿ=0.0f;Ƕ=0.75f;}if(Ǝ==-1&&Αθ){value=-1;return;}if(value==-1) Αθ=false;
bool ΗЅ=Ǝ<Ƿ||Ǝ>Ƕ;if(ΗЅ&&Λα){if(Ǝ<value) value=Ƕ;else if(Ǝ>value) value=Ƿ;}else value=Ǝ;if(ΗЅ&&Αθ) value=-1;else value=Math.Max(Ƿ,Math.Min(value,Ƕ));value=(float) Math.Round(value,2);}void ΓΗ(bool ΚϏ){if(ΚϏ) б=Math.Max(0,б-1);else б++;Γά();ΓΫ();for(int k=0;k<ȫ.Count&&k<г.Length;k++) ΓЉ(ȫ[k],k<б);
Д=-1;ΔΔ(true);}T ģ<T>(T ο){int Β=Array.IndexOf(Enum.GetValues(ο.GetType()),ο);Β++;if(Β>=ΘЀ(ο)) Β=0;return(T) Enum.GetValues(ο.GetType()).GetValue(Β);}int ΘЀ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΓЏ(){Αο.Clear();for(int i=0;i<Ψ.Count;i++) Αο.Add(Ψ[i]);Ψ.Clear();
Βξ=true;Δϊ=new ά(Ο);Δύ=new ά(Ζ);Ο.κ=false;if(Μ==Κ.ή) Ζ.κ=false;for(int i=0;i<ǥ.Count;i++)
if(!Ȳ.Contains(ǥ.Keys.ElementAt(i)))
Ȳ.Add(ǥ.Keys.ElementAt(i));Ā=false;ƥ=false;Ʀ=false;μ(Σ.Ζΰ);}void Ζμ(){if(Ā) Ο=Δϊ;if(ƥ) Ζ=Δύ;if(Ʀ){Ψ.Clear();for(int i=0;i<Αο.Count;i++) Ψ.Add(Αο[i]);}Βξ=false;Ѓ();μ(Σ.Main);}void ΙЀ(){if(!Βξ) return;if(Ϊ!=Α.ь){Ζμ();return;}if(!Δϊ.κ) Ā=false;if(!Δύ.κ) ƥ=false;if(Αο.Count<=1) Ʀ=false;
IMyShipConnector О=з(MyShipConnectorStatus.Connectable);if(О==null) О=з(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Ě,2)<=0.20) Δη++;else Δη=0;if(Δη>=5){if(Μ==Κ.ή&&(Ο.κ||Ā)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.θ=Ϋ.WorldMatrix.Forward;Ζ.Ё=Ϋ.WorldMatrix.Left;
Ζ.η=Ϋ.WorldMatrix.Down;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.κ=true;Ζ.о=О.Position;}else{Ο.θ=Ϋ.WorldMatrix.Forward;Ο.Ё=Ϋ.WorldMatrix.Left;Ο.η=Ϋ.WorldMatrix.Down;Ο.φ=Ϋ.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.κ=true;Ο.о=О.Position;}}}double ΔϏ=-1;if(Ψ.Count>0) ΔϏ=Vector3.Distance(δ,Ψ.Last().Ε);
double ΚЏ=Math.Max(1.5,Math.Pow(Ě/100.0,2));double Ηϊ=Math.Max(Ě*ΚЏ,2);Θα=Ηϊ;if((ΔϏ==-1)||ΔϏ>=Ηϊ){ά έ=new ά(δ,Βη,ΑΑ,ΑΤ,Ϋ.GetNaturalGravity());έ.Θμ(ȳ,Ȳ);Ψ.Add(έ);}}ά ы(){if(Μ!=Κ.ή) return ǟ!=null?ǟ:Ο;return ΖБ;}void ŵ(ά α,Π Є){ΖБ=α;if(γ==Π.В) ѝ=Є;}bool ț(ά α,float ϊ,bool ΛΨ,out Vector3 ι)
{if(ΛΨ){Vector3I Δς=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock Ω=Me.CubeGrid.GetCubeBlock(Δς);if(Ω==null||!(Ω.FatBlock is IMyShipConnector)){ι=new Vector3();return false;}Vector3 ΚΖ=ф(Ϋ,Ω.FatBlock.GetPosition()-δ);Vector3 ΚΕ=ф(Ϋ,Ω.FatBlock.WorldMatrix.Forward);ι=α.Ε-ΔΓ(α.θ,α.η*-1,ΚΖ)-ΔΓ(α.θ,α.η*-1,ΚΕ)*ϊ;
return true;}else{ι=α.Ε;return true;}}Vector3 ą(int ΚΓ,int Ɨ,bool ΚЂ){if(!ΚЂ&&ΗΩ) return ΔΨ;float ΚΔ=((Ќ-1f)/2f)-ΚΓ;float ΚЅ=((Ю-1f)/2f)-Ɨ;ΔΨ=Ζ.Ε+Ζ.Ё*ΚΔ*ǽ+ė*-1*ΚЅ*ȗ;ΗΩ=true;return ΔΨ;}Vector3 Γα(Vector3 ΛΒ,float Δβ){return ΛΒ+(Ǟ*Δβ);}ε ΓΧ(){float Ы=-1;ε ί=ε.ș;
if(Μ!=Κ.ή){ΖΥ();if(γ!=Π.э){Vector3 Н=ņ(Ǟ,ė*-1,δ-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ќ*ǽ)/2f&&Math.Abs(Н.Y)<=(float)(Ю*ȗ)/2f){if(Н.Z<=-1&&Н.Z>=-Š*2) return ε.Ασ;if(Н.Z>-1&&Н.Z<ν*2) return ε.ș;}if(ġ(Ζ.Ε,ref Ы)) ί=ε.ș;}if(Ο.κ){if(ġ(Ο.Ε,ref Ы)) ί=ε.Ș;for(int i=0;i<Ψ.Count;i++)
if(ġ(Ψ[i].Ε,ref Ы)) ί=ε.ć;if(Vector3.Distance(δ,Ο.Ε)<dockDist*ν) ί=ε.Ș;if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null)
ί=ε.Ș;}}else{Vector3 ι=new Vector3();IMyShipConnector ȧ=з(MyShipConnectorStatus.Connected);if(Ο.κ){if(ġ(Ο.Ε,ref Ы)) ί=ε.Ș;if(ț(Ο,dockDist,true,out ι))
if(ġ(ι,ref Ы)) ί=ε.Ș;if(ȧ!=null&&Vector3.Distance(ȧ.GetPosition(),Ο.Ε)<5)
return ε.ΓΛ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ο.Ε)>dockDist*ν&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*ν)
if(ġ(Ψ[i].Ε,ref Ы)) ί=ε.ć;if(Ζ.κ){if(ġ(Ζ.Ε,ref Ы)) ί=ε.ș;if(ț(Ζ,dockDist,true,out ι))
if(ġ(ι,ref Ы)) ί=ε.ș;if(ȧ!=null&&Vector3.Distance(ȧ.GetPosition(),Ζ.Ε)<5)
return ε.ΓΜ;}}return ί;}bool ġ(Vector3 ι,ref float Ы){float ϊ=Vector3.Distance(ι,δ);if(ϊ<Ы||Ы==-1){Ы=ϊ;return true;}return false;}void ΓЎ(){if(А>0){Λ="Setup error! Can't start";return;}if(Μ==Κ.ή){Ģ();return;}Ζ.Ε=δ;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.θ=Βη;Ζ.η=ΑΑ;Ζ.Ё=ΑΤ;
Ǟ=Ǡ.WorldMatrix.Forward;ė=Ζ.η;if(Ǟ==Ϋ.WorldMatrix.Down) ė=Ϋ.WorldMatrix.Backward;ƽ(true,true);Τ(Α.Ą);ΓЍ();}void ƽ(bool ǳ,bool ΚЃ){if(γ==Π.э&&!ǳ) return;bool Κρ=ǳ||γ==Π.ΓΩ||Ќ!=ѐ||Ю!=я||Ŗ!=Ȓ;if(Κρ){if(γ!=Π.э){γ=Π.Αφ;ą(Ѐ,ύ,ΚЃ);Λ="Job changed, lost progress";}Ŗ=Ȓ;
Ќ=ѐ;Ю=я;ύ=0;Ѐ=0;г="";Д=-1;ř=0;ĕ=0;й=0;б=0;ΔΔ(true);}}void ΓЁ(){и(δ,0);Ǘ(ȳ,true);}void ΓЍ(){ȟ(х.ΑБ);ΓЁ();ƹ(Ȯ,false);ȁ(ΕΪ);γ=Π.В;Εό(true);ѝ=γ;μ(Σ.ƴ);Ιΰ();ΒΪ=true;Βλ=0;for(int i=ƿ.Count-1;i>=0;i--)
if(Ʊ(ƿ[i],false)) Βλ++;if(Βλ>0) Λ="Started with damage";}void Ѓ(){if(γ==Π.В){γ=Π.ų;Λ="Job paused";}Τ(Α.ь);ѝ=γ;ΒΒ(false,0,0,0,0);ΓБ();Εχ(new Vector3(),false);Ζλ();ΓЈ(ChargeMode.Auto);ΓЊ(false);Ζβ(true);Ĉ(Α.ь);Ň(false,false,0,0);Ǘ(ω,false);Ǘ(љ,true);ȁ(true);ΒЄ=false;Αψ();
ǌ=false;ΓЃ();ΒΪ=false;Αΰ=false;Αι=false;if(ό!=Σ.Main&&ό!=Σ.Ατ&&ό!=Σ.ΓΘ&&ό!=Σ.ΒΓ)
μ(Σ.Main);}void Ģ(){ε Я=ΓΧ();if(Μ==Κ.ή){if(!Ζ.κ||!Ο.κ) return;ΓЍ();bool Δπ=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(ƚ==Π.Ũ) Δπ=true;if(ƚ==Π.ũ) Δπ=false;if(Δπ){ŵ(Ο,Π.Ũ);switch(Я){case ε.ΓΛ:Τ(Α.Ĩ);break;case ε.ć:Τ(Α.у);break;case ε.Ș:Τ(Α.ǿ);break;default:Τ(Α.ǭ);break;
}}else{ŵ(Ζ,Π.ũ);switch(Я){case ε.ΓΜ:Τ(Α.Ĩ);break;case ε.ș:Τ(Α.ǿ);break;case ε.ć:Τ(Α.у);break;default:Τ(Α.ǭ);break;}}}else{if(γ!=Π.ų&&γ!=Π.Αφ) return;bool ΛΪ=γ==Π.Αφ;ΓЍ();bool Δχ=Ʋ(false)&&Ο.κ;switch(Я){case ε.ș:Τ(Δχ?Α.Ĥ:Α.Ą);break;case ε.ć:Τ(Δχ?Α.И:Α.у);break;case ε.Ș:Τ(Δχ?Α.ǿ:Α.Ȣ);break;
case ε.Ασ:{if(ĕ!=б||ΛΪ) Τ(Α.Ȟ);else Τ(Α.Ų);}break;default:break;}}}void ł(){if(γ==Π.э&&!Ο.κ) return;if(Μ==Κ.ή&&(!Ζ.κ||!Ο.κ)) return;Λ="Move to job";ε Я=ΓΧ();if(Μ==Κ.ή){ŵ(Ζ,Π.ũ);switch(Я){case ε.ș:Τ(Α.ǿ);break;case ε.ć:Τ(Α.у);break;case ε.ΓΜ:return;default:Τ(Α.ǭ);break;
}Ĉ(Α.Ĩ);}else{switch(Я){case ε.ș:Τ(Α.у);break;case ε.ć:Τ(Α.у);break;case ε.Ș:Τ(Α.Ȣ);break;case ε.Ασ:Τ(Α.Ȟ);break;default:break;}if(γ==Π.э) Ĉ(Α.у);else Ĉ(Α.Ġ);ΒЄ=true;}ΓЁ();μ(Σ.ΓА);ƹ(Ȯ,false);ѝ=Π.ΑЉ;}void Αώ(){if(!Ο.κ) return;Λ="Move home";ε Я=ΓΧ();if(Μ==Κ.ή){ŵ(Ο,Π.Ũ);switch(Я)
{case ε.ć:Τ(Α.И);break;case ε.Ș:Τ(Α.ǿ);break;case ε.ΓΛ:return;default:Τ(Α.ǭ);break;}Ĉ(Α.Ĩ);}else{if(з(MyShipConnectorStatus.Connected)!=null) return;if(з(MyShipConnectorStatus.Connectable)!=null){Τ(Α.Ł);Ĉ(Α.Ȣ);return;}switch(Я){case ε.ș:Τ(Α.Ĥ);break;case ε.ć:Τ(Α.И);break;case ε.Ș:Τ(Α.И);break;
case ε.Ασ:Τ(Α.ĥ);break;default:break;}Ĉ(Α.Ȣ);}ΓЁ();μ(Σ.ΓА);ƹ(Ȯ,false);ѝ=Π.ΑЈ;}void Τ(Α ΕΒ){if(ΕΒ==Α.ь) Αγ=Α.ь;if(Αγ!=Α.ь&&Ϊ==Αγ&&ΕΒ!=Αγ){Ѓ();return;}Е=true;Ϊ=ΕΒ;}void Ĉ(Α Є){this.Αγ=Є;}Ǭ Λζ(int ΑΞ,bool ǹ){if(ǹ){ǣ=null;Ѐ=0;ύ=0;}if(Ȓ==ю.Ƽ){int ś=ΑΞ+1;
ύ=(int) Math.Floor(Й(ΑΞ,Ќ));if(ύ%2==0) Ѐ=ΑΞ-(ύ*Ќ);else Ѐ=Ќ-1-(ΑΞ-(ύ*Ќ));if(ύ>=Ю) return Ǭ.Ας;else return Ǭ.ΑЀ;}else if(Ȓ==ю.Εώ){if(ǣ==null) ǣ=new int[]{0,-1,0,0};int ΑΛ=(int) Math.Ceiling(Ќ/2f);int ΑΚ=(int) Math.Ceiling(Ю/2f);int Δυ=(int) Math.Floor(Ќ/2f);
int Δτ=(int) Math.Floor(Ю/2f);int ΑΟ=0;while(ǣ[2]<Math.Pow(Math.Max(Ќ,Ю),2)){if(ΑΟ>200) return Ǭ.ΘЁ;ΑΟ++;ǣ[2]++;if(-ΑΛ<Ѐ&&Ѐ<=Δυ&&-ΑΚ<ύ&&ύ<=Δτ){if(ǣ[3]==ΑΞ){this.Ѐ=Ѐ-1+ΑΛ;this.ύ=ύ-1+ΑΚ;return Ǭ.ΑЀ;}ǣ[3]++;}if(Ѐ==ύ||(Ѐ<0&&Ѐ==-ύ)||(Ѐ>0&&Ѐ==1-ύ))
{int ΛΕ=ǣ[0];ǣ[0]=-ǣ[1];ǣ[1]=ΛΕ;}Ѐ+=ǣ[0];ύ+=ǣ[1];}}return Ǭ.Ας;}void Ιώ(){if(Ϊ==Α.Ą){if(Е){Ж=0;if(ĕ!=б) ř=0;ĕ=б;}if(Ж==0){int Ǉ=Θς(б);if(Ǉ<0){if(!Αχ()){Τ(Α.ĥ);return;}if(!Δί||(Ǉ==-2)!=ΒΦ){Δί=true;ΒΦ=Ǉ==-2;ΖЎ=!ΒΦ?δ:Ζ.Ε-Ǟ*ν+
Ζ.Ё*(Ќ*ǽ/2+ΒЅ/2+ν/2);}и(ΖЎ,10);Λ=ΒΦ?"Waiting for a free hole":"Waiting for the controller";return;}Δί=false;if(Ǉ!=б){б=Ǉ;ř=0;ĕ=б;}Ǭ ί=Ιι(б);if(ί==Ǭ.Ας){γ=Π.ΓΩ;Λ="Job done";if(Ļ&&Ο.κ){Τ(Α.Ĥ);Ĉ(Α.Ȣ);ѝ=Π.ΑЈ;}else{Τ(Α.ŭ);Ĉ(Α.Ġ);ѝ=Π.ΑЉ;}return;
}if(ί==Ǭ.ΑЀ){Ж=1;Ǘ(ω,true);Ǎ=ą(Ѐ,ύ,true);и(Ǎ,10);л(Ζ.η,Ζ.θ,Ζ.Ё,false);}}else{if(Д>=0&&(ΖΤ()?ȇ>=0&&ȇ!=Д:Γθ(Д,true))){Ж=0;return;}if(в<wpReachedDist){Τ(Α.Ų);return;}}}if(Ϊ==Α.Ų){if(Е){Ǘ(ω,true);ȁ(false);Ǎ=ą(Ѐ,ύ,false);и(Γα(Ǎ,0),0);л(Ζ.η,Ζ.θ,Ζ.Ё,false);
Ж=1;ё=0;т=0;ΒΞ=0;ΔЁ=-1;Š=н;Δϋ=true;}if(!Αχ()){Τ(Α.ĥ);return;}if(Ʋ(true)){ΗЃ=Ă("","ORE",Ш.ľ);if((д==Г.ΓΪ||д==Г.Αω||
д==Г.Γι||д==Г.ΑЃ)&&Μ!=Κ.Ǫ)
Τ(Α.Ʈ);else if((д==Г.ΕЂ||д==Г.ΓΤ)&&Μ!=Κ.Ǫ)
Τ(Α.ΔΕ);else
Τ(Α.ĥ);return;}й=Vector3.Distance(δ,Ǎ);if(й>ř){ř=й;Δϋ=false;}if(Μ==Κ.Ǫ&&ΕЎ()==MyDetectedEntityType.SmallGrid)
т+=2;else т-=2;т=Math.Max(100,Math.Min(400,т));if(Ж>0&&Ж<т){if(й>ё){if(т>150) ё=й;else ё=(float) Math.Ceiling(й);Ж=1;}else Ж++;}else{if(Ж>0){Λ="Ship stuck! Retrying";ё=й;Ж=0;Ň(false,true,0,ν*sensorRange);}и(Γα(Ǎ,Math.Max(0,ё-ν)),Γδ(false));if(в<=wpReachedDist/2){Ж=1;ё=0;}
return;}Ň(false,true,ν*sensorRange,0);Vector3 ΙЊ=Ǎ+Ǟ*й;bool Ηδ=false;if(Vector3.Distance(ΙЊ,δ)>0.3f){Vector3 ΚΚ=Ǎ+Ǟ*(й+0.1f);и(ΚΚ,4);Ηδ=true;}else{float Ǻ=Γδ(true);Vector3 ΗΔ=Γα(Ǎ,Math.Max(н+1,й+1));Ȁ(true,false,false,ΗΔ,ΗΔ-Ǎ,Ǻ,Ǻ);}bool Δγ=false;if(њ==ǔ.ΓΞ||њ==ǔ.ΓΝ)
{if(!Ηδ){float ΕΕ=0;foreach(IMyTerminalBlock Ȗ in ω)
ΕΕ+=Θϋ(Ȗ,"","",њ==ǔ.ΓΝ?new string[]{"STONE"}:null);if(ΕΕ>ΔЁ||й<н||Δϋ){ΔΜ=0;ΒΞ=й;Š=(int)(Math.Max(Š,ΒΞ)+ν/2);}else{Δγ=й-ΒΞ>2&&ΔΜ>=20;ΔΜ++;}ΔЁ=ΕΕ;}}else Δγ=й>=Š;if(ĕ!=б){Τ(Α.Ȟ);й=0;return;}if(Δγ){б++;Τ(Α.Ȟ);й=0;return;}}if(Ϊ==Α.Ȟ||Ϊ==Α.ĥ||Ϊ==Α.Ʈ)
{if(Е){Ǎ=ą(Ѐ,ύ,false);л(Ζ.η,Ζ.θ,Ζ.Ё,false);Ǘ(ω,Ɓ);ȁ(false);ё=Vector3.Distance(δ,Ǎ);Ň(false,true,0,ν*sensorRange);}и(Ǎ,Γδ(false));if(Vector3.Distance(δ,Ǎ)>=ё+5){Ǘ(ω,false);ȁ(true);Λ="Can´t return!";}if(в<wpReachedDist){if(Ϊ==Α.Ȟ&&ΒЄ) Τ(Α.ŭ);if(Ϊ==Α.Ȟ) Τ(Α.Ą);
if(Ϊ==Α.Ʈ) Τ(Α.Αύ);if(Ϊ==Α.ĥ){if(Ο.κ) Τ(Α.Ĥ);else{Ѓ();ł();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Α.И||Ϊ==Α.у){if(Ϊ==Α.у&&γ==Π.В&&Μ!=Κ.ή){if(!Αχ()||Ʋ(true)){Τ(Α.И);return;}}bool ΕΜ=false;bool ΓΒ=false;bool Ζω=false;float Ż=0;bool ΗЂ=false;ά έ=null;
if(Е){if(Ϊ==Α.И||Μ==Κ.ή){if(Μ!=Κ.ή){if(ĉ(1)) ΓΚ();else{ǟ=null;Ȱ=0;ǌ=!ΓΣ(1);}}ά α=ы();ξ=new ΓЀ();ξ.ζ=α;ξ.Ǌ=followPathDock*ν;ξ.Εμ=useDockDirectionDist*ν;ξ.Ż=10;ξ.Ơ.Add(α.Ε);Vector3 ź=new Vector3();if(ț(α,dockDist*ν,true,out ź))
ξ.Ơ.Add(ź);else ξ.Ǌ*=1.5f;if(Μ==Κ.ή){if(α==Ο) ξ.Βδ=Ζ.Ε;if(α==Ζ) ξ.Βδ=Ο.Ε;ξ.Δν=dockDist*ν*1.1f;}}else if(Ϊ==Α.у){ξ=new ΓЀ();ξ.ζ=Ζ;ξ.Ǌ=followPathJob*ν;ξ.Εμ=useJobDirectionDist*ν;ξ.Ż=10;ξ.Βδ=Ο.Ε;ξ.Δν=dockDist*ν*1.1f;ξ.Ơ.Add(Ζ.Ε);if(γ==Π.э){if(!Ο.κ||Ψ.Count==0){Ѓ();return;}
float ΚΠ=Vector3.Distance(Ψ.First().Ε,Ο.Ε);float ΚΡ=Vector3.Distance(Ψ.Last().Ε,Ο.Ε);if(ΚΠ<ΚΡ) ξ.ζ=Ψ.Last();else ξ.ζ=Ψ.First();}}Αά=new Vector3();ΗЂ=!ΙΛ(δ);Ǘ(ω,false);ȁ(true);к=-1;double Ȇ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ξ.Βδ)<=ξ.Δν) continue;
double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȇ==-1||ϊ<Ȇ){к=i;Ȇ=ϊ;}}Ȑ=ΘЍ(ξ.ζ.Ε,к);Ō=null;}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&!ƻ()){ά ΔΡ=ы();ǌ=!ΓΣ(1);if(ы()!=ΔΡ){Τ(Α.И);return;}}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&ĉ(1)&&ΓΚ()){Τ(Α.И);return;}bool Ηβ=false;if(Ϊ==Α.И&&!Е&&к>=0&&к<Ψ.Count)
{Vector3 ǉ=Ψ[к].Ε+ΕБ(к,Ȑ);int ΕΓ=к+Ȑ;Vector3 ś=Ȑ!=0&&ΕΓ>=0&&ΕΓ<Ψ.Count?Ψ[ΕΓ].Ε:ξ.ζ.Ε;if(ΙΖ(ǉ,ś)){if(!Βέ){Βέ=true;ΗΙ=ű(ǉ)?δ:ǉ;}и(ΗΙ,0);л(ξ.ζ.η,ξ.ζ.θ,ξ.ζ.Ё,false);Λ="Waiting for dock clearance";return;}Βέ=false;Ηβ=ĉ(1)&&(ű(δ)||ű(ś));}Θν(Ψ,Ȑ,ξ.Ơ,ξ.Ǌ,Е,ref Ж);
for(int i=0;i<ξ.Ơ.Count;i++){float ϊ=Vector3.Distance(δ,ξ.Ơ[i]);if(ϊ<=ξ.Ǌ) ΕΜ=true;if(ϊ<=ξ.Εμ) ΓΒ=true;}if(Ηβ) ΓΒ=true;if(ΓΒ) Ż=ξ.Ż;float Λλ=Ō!=null?Ō.Ц:Ě;float Λΰ=(float) Math.Max(Ě*0.1f*ν,wpReachedDist);if((в<Λΰ)||Е){if(!Е) к+=Ȑ;if(Ȑ==0||к>Ψ.Count-1||к<0)
ΕΜ=true;else{Εο=Ȑ>0?Ψ.Count-1-к:к;έ=Ψ[к];Ō=έ;if(к>=1&&к<Ψ.Count-1)
Αά=έ.Ε-Ψ[к-Ȑ].Ε;else Ō=null;Ђ=έ.Ε+ΕБ(к,Ȑ);Ζω=true;}}if(ΓΒ)
л(ξ.ζ.η,ξ.ζ.θ,ξ.ζ.Ё,false);else if(ΗЂ)
ΓΙ(ξ.ζ.η,10,true);else if(Ζω&&έ!=null){if(Ȑ>0) л(έ.η,έ.θ,έ.Ё,90,false);else л(έ.η,-έ.θ,-έ.Ё,90,false);}Ȁ(true,false,true,Ђ,Αά,Ō==null?0:Ō.Ц,Ż);if(ΕΜ){Εο=0;if(Ϊ==Α.И&&!Θπ()){и(Ђ,0);return;}if(Ϊ==Α.И&&ǌ){и(Ђ,0);Λ="Waiting for a free dock";return;}if(Ϊ==Α.И||Μ==Κ.ή){Τ(Α.ǿ);return;}
if(Ϊ==Α.у&&ΒЄ){Τ(Α.ŭ);return;}if(Ϊ==Α.у){Τ(Α.Ą);return;}}}if(Ϊ==Α.ǿ||Ϊ==Α.Ƹ){if(Μ!=Κ.ή&&ƻ()){if(!ĉ(1)){Τ(Α.И);return;}}else if(Μ!=Κ.ή){ά ΔΡ=ы();if(!ΓΣ(2)||ы()!=ΔΡ){Τ(Α.И);return;}}ά α=ы();if(Е){if(!ț(α,dockDist*ν,true,out Ђ)){ȟ(х.Ƭ);Ѓ();return;}
и(Ђ,0);ΓΙ(α.η,90,true);}if(в<followPathDock*ν&&в!=-1){и(Ђ,10);л(α.η,α.θ,α.Ё,false);}if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null){Τ(Α.Ł);return;}if(в<wpReachedDist/2&&в!=-1){Τ(Α.Ľ);return;}}if(Ϊ==Α.Ľ||Ϊ==Α.Ġ){if(Е)
{if(Ϊ==Α.Ľ){ά α=ы();if(!ț(α,dockDist*ν,true,out Ђ)){ȟ(х.Ƭ);Ѓ();return;}Ȁ(true,true,false,Ђ,0);л(α.η,α.θ,α.Ё,10,false);}if(Ϊ==Α.Ġ){л(Ζ.η,Ζ.θ,Ζ.Ё,0.5f,false);Ђ=Ζ.Ε;Ȁ(true,true,false,Ђ,0);}}if(Βν){ΒΒ(false,0,0,0,0);if(Ϊ==Α.Ľ) Τ(Α.Ł);if(Ϊ==Α.Ġ) Ѓ();return;}}
if(Ϊ==Α.Ł){if(з(MyShipConnectorStatus.Connected)!=null){if(Μ==Κ.ή) Τ(Α.Ĩ);else Τ(Α.Ȣ);return;}ά α=ы();if(Е){т=0;ΚΣ=DateTime.Now;Ж=0;л(α.η,α.θ,α.Ё,false);}Vector3I Δς=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock ΖЋ=Me.CubeGrid.GetCubeBlock(Δς);float ΗΜ=dockingSpeed;
float Κμ=dockingSpeed*5;float Ζχ=Math.Max(1.5f,Math.Min(5f,ν*0.15f));if(!ț(α,0,true,out Ђ)||!ț(α,Ζχ,true,out Αά)||
ΖЋ==null||!ΖЋ.FatBlock.IsFunctional){ȟ(х.Ƭ);Ѓ();return;}if(т==1||(Vector3.Distance(δ,Ђ)<=Ζχ*1.1f&&!Е))
т=1;else{Vector3 ΛΞ=ф(Ϋ,Αά-δ);Vector3 Δρ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΚІ=Αυ(ΛΞ,Δρ,null);ΗΜ=Math.Min(Κμ,ΚІ);}Ȁ(true,false,false,Ђ,Ђ-δ,dockingSpeed,ΗΜ);if(Е) ё=(float) в;IMyShipConnector Δή=з(MyShipConnectorStatus.Connectable);if(Δή!=null){Ȁ(false,false,false,Ђ,0);
if(Ж>0) Ж=0;Ж--;if(Ж<-5){Δή.Connect();if(Δή.Status==MyShipConnectorStatus.Connected){if(Μ==Κ.ή) Τ(Α.Ĩ);else Τ(Α.Ȣ);ΓБ();ƹ(Ȯ,true);return;}}}else{float ΗΘ=(float) Math.Round(в,1);if(ΗΘ<ё){Ж=-1;ё=ΗΘ;}else Ж++;if(Ж>20){Τ(Α.Ƹ);return;}}}if(Ϊ==Α.Ȣ||Ϊ==Α.Ĩ||
Ϊ==Α.ΒΙ||Ϊ==Α.Αϋ||Ϊ==Α.ŀ){bool ΔΚ=false,ΔΛ=false;if(Μ==Κ.ή){if(ы()==Ο) ΔΚ=true;else if(ы()==Ζ) ΔΛ=true;}if(Е){Αΰ=false;if(з(MyShipConnectorStatus.Connected)==null){Τ(Α.ǭ);return;}ΓБ();if(ΔΚ) ΒΖ(Ї.ǲ);if(ΔΛ) ΒΖ(Ј.ǲ);Ť=false;Ċ=false;đ=false;Αλ=false;}if(з(MyShipConnectorStatus.Connected)==null){Ѓ();ȟ(х.ΖΝ);return;}
if(γ!=Π.В||ў==-1||ȃ==і.None) Ċ=true;else if(Ȥ>=100f) Ċ=true;else if(Ȥ<=99f) Ċ=false;if(γ!=Π.В||џ==-1||Ǜ.Count==0) đ=true;else if(Ē>=100f) đ=true;else if(Ē<=99) đ=false;if(γ!=Π.В||п==-1||ķ.Count==0) Αλ=true;else Αλ=ť>=п;ŷ ѕ=null;if(ΔΚ) ѕ=Ї;
if(ΔΛ) ѕ=Ј;if(ѕ!=null&&(ѕ.Р==ρ.ΑЍ||ѕ.Р==ρ.ΑЌ)) Ċ=true;if(ѕ!=null&&(ѕ.Р==ρ.Γψ)) if(!Ť) Ċ=false;if(ѕ!=null&&(ѕ.Р==ρ.Γό||ѕ.Р==ρ.Γϊ)) đ=true;if(ѕ!=null&&(ѕ.Р==ρ.Γϋ)) if(!Ť) đ=false;if(ē){ChargeMode ΖЃ=Ċ?ChargeMode.Auto:ChargeMode.Recharge;if(ѕ!=null&&(ѕ.Р==ρ.ΑЌ||ѕ.Р==ρ.ΑЍ))
ΖЃ=ChargeMode.Discharge;ΓЈ(ΖЃ);ΓЊ(!đ);}if(!Ť){if(Μ==Κ.ή)
Ť=γ!=Π.В||Θρ(Е,true)||Αι;else
Ť=γ!=Π.В||ΖΞ();}else{if(!Ċ) Τ(Α.ŀ);if(!đ) Τ(Α.Αϋ);if(!Αλ) Τ(Α.ΒΙ);Е=false;}if(Ť&&Ċ&&đ&&Αλ){ΓЈ(ChargeMode.Auto);ΓЊ(false);if(γ==Π.В){if(Μ==Κ.ή){if(ы()==Ο) ΒΖ(Ї.ǵ);else if(ы()==Ζ) ΒΖ(Ј.ǵ);if(ы()==Ο) ŵ(Ζ,Π.ũ);else ŵ(Ο,Π.Ũ);}}Τ(Α.ǭ);return;}}if(Ϊ==Α.ǭ){if(Αξ){if(!ΕЀ()) return;
Αξ=false;Τ(Α.ǭ);return;}if(Е){if(!ΕЀ()){Αξ=true;return;}IMyShipConnector ȧ=з(MyShipConnectorStatus.Connected);if(ȧ==null){Τ(Α.у);return;}IMyShipConnector λ=ȧ.OtherConnector;Ζα(ȧ,false);ƹ(Ȯ,false);ά α=null;ά ћ=Μ==Κ.ή?Ο:ы();if(Vector3.Distance(ȧ.GetPosition(),ћ.Ε)<5f&&Ο.κ) α=ћ;
if(Vector3.Distance(ȧ.GetPosition(),Ζ.Ε)<5f&&Ζ.κ) α=Ζ;if(α!=null){if(!ț(α,dockDist*ν,true,out Ђ)){ȟ(х.Ƭ);Ѓ();return;}и(Ђ,5);л(α.η,α.θ,α.Ё,false);}else и(δ+λ.WorldMatrix.Forward*dockDist*ν,5);if(γ==Π.В) ȟ(х.ΑБ);}if(в<wpReachedDist){Ǘ(љ,true);Τ(Α.у);return;}}if(Ϊ==Α.ŭ)
{if(Е){ȁ(true);Ǘ(ω,false);Ђ=Ζ.Ε;и(Ђ,20);л(Ζ.η,Ζ.θ,Ζ.Ё,false);}if(в<wpReachedDist/2){Τ(Α.Ġ);return;}}if(Ϊ==Α.Ĥ){if(Е){ȁ(true);Ǘ(ω,false);int Ы=-1;double Ȇ=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȇ==-1||ϊ<Ȇ){Ы=i;Ȇ=ϊ;}}
if(Ы==-1){Τ(Α.И);return;}Ђ=Ψ[Ы].Ε;и(Ђ,10);л(Ζ.η,Ζ.θ,Ζ.Ё,false);}if(в<wpReachedDist){Τ(Α.И);return;}}if(Ϊ==Α.Αύ){bool Ϗ=false;if(Е){ȁ(true);if((д==Г.ΓΪ||д==Г.Αω)&&ΑЅ()&&
ū(Ǟ,Ϋ.GetNaturalGravity())<25&&Ќ>=2&&Ю>=2){Vector3 ΑΒ=δ;if(Ѐ>0&&ύ<Ю-1) ΑΒ=ą(Ѐ-1,ύ+1,true);else if(Ѐ<Ќ-1&&ύ<Ю-1) ΑΒ=ą(Ѐ+1,ύ+1,true);else if(Ѐ<Ќ-1&&ύ>0) ΑΒ=ą(Ѐ+1,ύ-1,true);else if(Ѐ>0&&ύ>0) ΑΒ=ą(Ѐ-1,ύ-1,true);else Ϗ=true;
if(!Ϗ) и(ΑΒ,10);}else Ϗ=true;}if(в<wpReachedDist/2) Ϗ=true;if(Ϗ){Τ(Α.ΒΘ);return;}}if(Ϊ==Α.ΔΕ||Ϊ==Α.ΒΘ){if(Е){Ȁ(true,true,false,δ,0);Ǘ(ω,false);ȁ(true);Ж=-1;т=д==Г.Γι||д==Г.ΑЃ?0:-1;}bool Ő=!Αχ();int ΒЌ=Ă("STONE","ORE",Ш.ľ);if(д==Г.ΓΤ||д==Г.ΑЃ||д==Г.Αω)
ΒЌ+=Ă("ICE","ORE",Ш.ľ);bool Κά=ΒЌ>0;bool Δμ=false;if(т>=0){float Ÿ=(float) Math.Sin(ΕЄ(т))*ǽ/3f;float ΙЌ=(float) Math.Cos(ΕЄ(т))*ȗ/3f;Vector3 ΚΒ=ą(Ѐ,ύ,true)+ΔΓ(Ǟ,ė*-1,new Vector3(Ÿ,ΙЌ,0));и(ΚΒ,0.3f);if(в<Math.Min(ǽ,ȗ)/10f) т+=5f;if(т>=360) т=0;}if(Ж==-1||ΒЌ<Ж){Ж=ΒЌ;ё=0;}
else{ё++;if(ё>50) Δμ=true;}if(!Κά||Ő||Δμ){if(!Ő){int ΛΤ=Ă("","ORE",Ш.ľ);if(Ʋ(true)) Ő=true;else if(100-(Й(ΛΤ,ΗЃ)*100)<minEjection) Ő=true;else ȟ(х.ΑБ);}if(Δμ&&Ő) Λ="Ejection failed";if(Ϊ==Α.ΒΘ){if(Ő){if(Ο.κ) Τ(Α.Ĥ);else{Ѓ();ł();Λ="Can´t return, no dock found";}}
else Τ(Α.Ą);}else if(Ő) Τ(Α.ĥ);else Τ(Α.Ų);return;}}Е=false;}void Θν(List<ά>ļ,int ς,List<Vector3>Αζ,float ȓ,bool ǹ,ref int ǰ){if(ǹ){for(int i=0;i<ļ.Count;i++) ļ[i].Ц=0;ǰ=-1;return;}if(ς==0) return;int Ǥ=ς*-1;if(ǰ==-1) ǰ=Ǥ>0?1:ļ.Count-2;int ΑΟ=0;while(ǰ>=1&&ǰ<ļ.Count-1)
{if(ΑΟ>50) return;ΑΟ++;try{if((Ǥ<0&&ǰ>=1)||(Ǥ>0&&ǰ<=ļ.Count-2)){ά ο=ļ[ǰ];bool ΗϏ=false;for(int j=0;j<Αζ.Count;j++)
if(Vector3.Distance(ο.Ε,Αζ[j])<=ȓ){ΗϏ=true;break;}if(!ΗϏ){ά ΒЀ=ļ[ǰ-Ǥ];ά ś=ļ[ǰ+Ǥ];Vector3 ΛΟ=ο.Ε-ś.Ε;Vector3 Εε=ΒЀ.Ε-ο.Ε;Vector3 ΚЀ=ο.Ε+Vector3.Normalize(ΛΟ)*Εε.Length();Vector3 ΚΟ=ΒЀ.Ε-ΚЀ;Vector3 Βσ=ņ(ς>0?ο.θ:ο.θ*-1,ο.η*-1,ΚΟ);Vector3 Κλ=ņ(ς>0?ο.θ:ο.θ*-1,ο.η*-1,Εε);
Vector3 Βυ=ņ(ς>0?ο.θ:ο.θ*-1,ο.η*-1,ο.φ);ο.Ц=(float) Math.Sqrt(Math.Pow(ΒЀ.Ц,2)+Math.Pow(Αυ(-Κλ,Βυ,ο),2));for(int j=0;j<Αζ.Count;j++)
if(Vector3.Distance(ΒЀ.Ε,Αζ[j])<=ȓ){Vector3 Βφ=ņ(ς>0?ο.θ:ο.θ*-1,ο.η*-1,Αζ[j]-ο.Ε);float ΛΔ=Αυ(-Βφ,Βυ,ο);ο.Ц=Math.Min(ο.Ц,ΛΔ)/2f;}if(Βσ.Length()==0) Βσ=new Vector3(0,0,1);Vector3 Κκ=ņ(ο.θ,ο.η*-1,ο.φ);float ΛΙ=Ư(-Βσ,Κκ,ο);float ΒΚ=Й(ΛΙ,Ţ);float Εβ=(float) Math.Sqrt(Й(Βσ.Length()*1.0f,0.5f*ΒΚ));
ο.Ц=Math.Min(ο.Ц,(Εε.Length()/Εβ)*Ȃ);}}}catch{return;}ǰ+=Ǥ;}ǰ=-1;}void ΔΔ(bool ǹ){if(ǹ){ΑΠ=0;return;}if(Μ==Κ.ή) return;float Ϗ=б*Math.Max(1,н);if(ĕ==б) Ϗ+=Math.Min(н,й);float ΛΣ=Ќ*Ю*Math.Max(1,н);ΑΠ=Math.Max(ΑΠ,(float) Math.Min(Ϗ/ΛΣ*100.0,100));}MyDetectedEntityType ΕЎ()
{try{if(Ȝ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γδ(bool ȋ){if(Μ==Κ.Ǫ&&ΕЎ()==MyDetectedEntityType.None&&!Ʊ(ψ,true))
return fastSpeed;else return ȋ?ƪ:Ʃ;}void Ιυ(){float Ηχ=0,ΗΒ=0,Ŕ=0,ΕΖ=0;for(int i=0;i<ĩ.Count;i++){IMyBatteryBlock Ǆ=ĩ[i];if(!Ȝ(Ǆ,true)) continue;Ηχ+=Ǆ.MaxStoredPower;ΗΒ+=Ǆ.CurrentStoredPower;Ŕ+=Ǆ.CurrentInput;ΕΖ+=Ǆ.CurrentOutput;}Ȥ=(float) Math.Round(Й(ΗΒ,Ηχ)*100,1);
if(Ŕ>=ΕΖ) ȃ=і.ŀ;else ȃ=і.ΕЅ;if(Ŕ==0&&ΕΖ==0||Ȥ==100.0) ȃ=і.ь;if(ĩ.Count==0) ȃ=і.None;}void Ιϋ(){float ΗΡ=0;for(int i=0;i<Ǜ.Count;i++){IMyGasTank ΘΞ=Ǜ[i];if(!Ȝ(ΘΞ,true)) continue;ΗΡ+=(float) ΘΞ.FilledRatio;}Ē=Й(ΗΡ,Ǜ.Count)*100f;}void ΙІ(){ť=0;try{for(int i=0;i<ķ.Count;i++)
{IMyReactor ΕΝ=ķ[i];if(!Ȝ(ΕΝ,true)) continue;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();ΕΝ.GetInventory(0).GetItems(Ћ);float Њ=0;for(int j=0;j<Ћ.Count;j++){MyInventoryItem ώ=Ћ[j];if(ώ.Type.SubtypeId.ToUpper()=="URANIUM"&&ώ.Type.TypeId.ToUpper().Contains("_INGOT"))
Њ+=(float) ώ.Amount;}if(Њ<ť||i==0){ť=Њ;ΔЉ=ΕΝ.CustomName;}}}catch(Exception e){Ĕ=e;}}void Ιφ(){ȏ=0;Ȉ=0;try{for(int i=0;i<ſ.Count;i++){IMyTerminalBlock Ω=ſ[i];if(!Ȝ(Ω,true)) continue;Ȉ+=(float) Ω.GetInventory(0).CurrentVolume;ȏ+=(float) Ω.GetInventory(0).MaxVolume;
}Βς=(float) Math.Min(Math.Round(Й(Ȉ,ȏ)*100,1),100.0);}catch(Exception e){Ĕ=e;}}void Ιό(){try{Ǵ.Clear();for(int i=0;i<ſ.Count;i++){IMyTerminalBlock Ω=ſ[i];if(!Ȝ(Ω,true)) continue;Εφ(Ω,Ш.Ű);}if(д!=Г.Γχ){for(int i=0;i<Ŋ.Count;i++){IMyTerminalBlock Ω=Ŋ[i];if(!Ȝ(Ω,true)) continue;
Εφ(Ω,Ш.ΙΙ);}}Ιμ(Ǵ);}catch(Exception e){Ĕ=e;}}void Θο(){if(ΒΪ){if(ΕЍ().Count>Βλ){ΒΪ=false;if(ђ!=ъ.ư){Ѓ();if(ђ==ъ.Γί) ł();if(ђ==ъ.ΑА)
if(Ο.κ) Αώ();else ł();}Λ="Damage detected";}}}bool Αχ(){if(!Βμ) return true;if(γ==Π.В){if(ў>0&&ȃ!=і.None)
if(Ȥ<=ў){Λ="Low energy! Move home";return false;}if(п>0&&ķ.Count>0)
if(ť<=п){Λ="Low fuel: "+ΔЉ;return false;}if(џ>0&&Ǜ.Count>0)
if(Ē<=џ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕЍ(){List<IMyTerminalBlock>ō=new List<IMyTerminalBlock>();for(int i=0;i<ƿ.Count;i++){IMyTerminalBlock Ω=ƿ[i];if(Ʊ(Ω,false)) ō.Add(Ω);}return ō;}bool Ʊ(IMyTerminalBlock Ω,bool Εά){return(!Ȝ(Ω,Εά)||!Ω.IsFunctional);
}bool Ȝ(IMyTerminalBlock Ω,bool Εά){if(Ω==null) return false;try{IMyCubeBlock ΗΨ=Me.CubeGrid.GetCubeBlock(Ω.Position).FatBlock;if(Εά) return ΗΨ==Ω;else return ΗΨ.GetType()==Ω.GetType();}catch{return false;}}float Θϋ(IMyTerminalBlock Ω,String ǒ,String ΘΕ,String[]ȉ){float Ȩ=0;for(int i=0;i<Ω.InventoryCount;i++)
{IMyInventory Ɔ=Ω.GetInventory(i);List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ɔ.GetItems(Ћ);foreach(MyInventoryItem ώ in Ћ){if(ȉ!=null&&(ȉ.Contains(ώ.Type.TypeId.ToUpper())||ȉ.Contains(ώ.Type.SubtypeId.ToUpper())))
continue;if((ǒ==""||ώ.Type.TypeId.ToUpper()==ǒ)&&(ΘΕ==""||ώ.Type.SubtypeId.ToUpper()==ΘΕ))
Ȩ+=(float) ώ.Amount;}}return Ȩ;}м ΘЃ(String ΰ,String υ,Ш Я,bool ΚΛ){ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ǵ.Count;i++){м ώ=Ǵ[i];if(ώ.ΰ.ToUpper()==ΰ&&ώ.υ.ToUpper()==υ&&(ώ.Ɗ==Я||Я==Ш.ľ))
return ώ;}м ί=null;if(ΚΛ){ί=new м(ΰ,υ,0,Я);Ǵ.Add(ί);}return ί;}int Ă(String ΰ,String υ,Ш Я){return Ă(ΰ,υ,Я,null);}int Ă(String ΰ,String υ,Ш Я,String[]ȉ){int Ȩ=0;ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ǵ.Count;i++){м ώ=Ǵ[i];if(ȉ!=null&&ȉ.Contains(ώ.ΰ.ToUpper())) continue;
if((ΰ==""||ώ.ΰ.ToUpper()==ΰ)&&(υ==""||ώ.υ.ToUpper()==υ)&&(ώ.Ɗ==Я||Я==Ш.ľ))
Ȩ+=ώ.Њ;}return Ȩ;}void Εφ(IMyTerminalBlock Ω,Ш Я){for(int i=0;i<Ω.InventoryCount;i++){List<MyInventoryItem>Ћ=new List<MyInventoryItem>();Ω.GetInventory(i).GetItems(Ћ);for(int j=0;j<Ћ.Count;j++)
ΘЃ(Ћ[j].Type.SubtypeId,Ћ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Я,true).Њ+=(int) Ћ[j].Amount;}}void Ιμ(List<м>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){м a=Ѝ[j];м b=Ѝ[j+1];if(a.Њ<b.Њ) Ѝ.Move(j,j+1);}}bool Ʋ(bool Λθ){if(ğ&&Μ!=Κ.ή)
if(е!=-1&&Ţ>=е){Λ="Ship too heavy";return true;}if(Βς>=Ȏ||Αΰ){Αΰ=false;Λ="Ship is full";return true;}return false;}bool ΖΞ(){String[]ȉ=null;if(!Ğ) ȉ=new string[]{"ICE"};if(Μ==Κ.ƶ) return Ă("","ORE",Ш.Ű,ȉ)==0;if(Μ==Κ.Ǫ) return Ă("","COMPONENT",Ш.Ű,ȉ)==0;else return Ă("","",Ш.Ű,ȉ)==0;
}bool ΙΜ(){List<IMyCockpit>Δέ=new List<IMyCockpit>();χ.GetBlocksOfType(Δέ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δέ.Count;i++)
if(Δέ[i].IsUnderControl) return true;return false;}bool Θρ(bool ǹ,bool ΒЇ){IMyShipConnector О=з(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return ΕЁ(Ї,ǹ,ΒЇ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return ΕЁ(Ј,ǹ,ΒЇ);return false;}bool ΕЁ(ŷ Ȧ,bool ǹ,bool ΒЇ){if(ǹ) Ȧ.Ιά();Ȧ.ΙЂ();bool є=false;String ǐ="";switch(Ȧ.Р){case ρ.Γω:{ǐ="Waiting for command";є=false;break;}case ρ.ΖΫ:{ǐ="Waiting for cargo";є=Ʋ(true);break;}case ρ.ΖΪ:{ǐ="Unloading";є=ΖΞ();break;}case ρ.Γώ:{є=true;break;}
case ρ.Γψ:{ǐ="Charging batteries";є=Ȥ>=100f;break;}case ρ.ΑЍ:{ǐ="Discharging batteries";є=Ȥ<=25f;break;}case ρ.ΑЌ:{ǐ="Discharging batteries";є=Ȥ<=0f;break;}case ρ.Γϋ:{ǐ="Filling up hydrogen";є=Ē>=100f;break;}case ρ.Γό:{ǐ="Unloading hydrogen";є=Ē<=25f;break;}
case ρ.Γϊ:{ǐ="Unloading hydrogen";є=Ē<=0f;break;}case ρ.Γύ:{bool Ηα=ΙΜ();if(!Ηα) Ȧ.ΕΙ=true;є=Ȧ.ΕΙ&&Ηα;ǐ="Waiting for passengers";break;}}if(!є) Ȧ.ΓΡ(true);if(є&&Ȧ.Γζ()){є=Ȧ.ΓΡ(false);ǐ="Undocking in: "+ΑϏ((int) Math.Max(0,Ȧ.Ǳ-Ȧ.į));}if(ΒЇ) Θί=ǐ;return є;}void Θε()
{if(!Ƈ) return;if(ω.Count<=1) return;float ΘΧ=0,ΘΦ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȗ=ω[i];if(Ʊ(Ȗ,true)) continue;ΘΧ+=(float) Ȗ.GetInventory(0).MaxVolume;ΘΦ+=(float) Ȗ.GetInventory(0).CurrentVolume;}float Ζϊ=(float) Math.Round(Й(ΘΦ,ΘΧ),5);for(int ŝ=0;ŝ<Math.Max(1,Math.Floor(ω.Count/10f));ŝ++)
{float Δω=0,ΔЈ=0;float Ηή=0,Ητ=0;IMyTerminalBlock Ď=null,İ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȗ=ω[i];if(Ʊ(Ȗ,true)) continue;float ΔЏ=(float) Ȗ.GetInventory(0).MaxVolume;float Βε=Й((float) Ȗ.GetInventory(0).CurrentVolume,ΔЏ);if(Ď==null||Βε>Δω){Ď=Ȗ;Δω=Βε;Ηή=ΔЏ;}
if(İ==null||Βε<ΔЈ){İ=Ȗ;ΔЈ=Βε;Ητ=ΔЏ;}}if(Ď==null||İ==null||Ď==İ) return;if(checkConveyorSystem&&!Ď.GetInventory(0).IsConnectedTo(İ.GetInventory(0))){if(ΔΠ>20) Λ="Inventory balancing failed";else ΔΠ++;return;}ΔΠ=0;List<MyInventoryItem>Ћ=new List<MyInventoryItem>();
Ď.GetInventory(0).GetItems(Ћ);float Αν=0;if(Ћ.Count==0) continue;MyInventoryItem Βζ=Ћ[0];String Ηε=Βζ.Type.TypeId+Βζ.Type.SubtypeId;if(!Θή.TryGetValue(Ηε,out Αν)){if(ΙΡ(Ď.GetInventory(0),0,İ.GetInventory(0),out Αν))
Θή.Add(Ηε,Αν);else return;}float ΛΠ=((Δω-Ζϊ)*Ηή/Αν);float ΛΡ=((Ζϊ-ΔЈ)*Ητ/Αν);int Εκ=(int) Math.Min(ΛΡ,ΛΠ);if(Εκ<=0) return;if((float) Βζ.Amount<Εκ)
Ď.GetInventory(0).TransferItemTo(İ.GetInventory(0),0,null,null,Βζ.Amount);else
Ď.GetInventory(0).TransferItemTo(İ.GetInventory(0),0,null,null,Εκ);}}bool ΙΡ(IMyInventory Αα,int Β,IMyInventory ΚΞ,out float Θέ){Θέ=0;float ΙА=(float) Αα.CurrentVolume;List<MyInventoryItem>ΔΣ=new List<MyInventoryItem>();Αα.GetItems(ΔΣ);float Ζώ=0;for(int i=0;i<ΔΣ.Count;i++) Ζώ+=(float) ΔΣ[i].Amount;
Αα.TransferItemTo(ΚΞ,Β,null,null,1);float ΘΫ=ΙА-(float) Αα.CurrentVolume;List<MyInventoryItem>ΔΗ=new List<MyInventoryItem>();Αα.GetItems(ΔΗ);float Ζσ=0;for(int i=0;i<ΔΗ.Count;i++) Ζσ+=(float) ΔΗ[i].Amount;if(ΘΫ==0f||!Γκ(0.9999,Ζώ-Ζσ,1.0001))
return false;Θέ=ΘΫ;return true;}void Ιϊ(){Vector3 ΑΗ=Ђ+ĺ;Vector3 ȕ=ΑΗ-δ;if(ȕ.Length()==0) ȕ=new Vector3(0,0,-1);Vector3 Βφ=ф(Ϋ,ȕ);Vector3 ΘΟ=Vector3.Normalize(Βφ);Vector3 Ɖ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΚА=ΕΗ>0?Math.Max(0,1-ū(ȕ,ΗΤ)/5):0;float Ц=(float) Math.Min((ΔЌ>0?ΔЌ:1000f),Math.Max(Αυ(-Βφ,Ɖ,null),ΕΗ*ΚА));
if(!ΘΥ) Ц=0;if(ΗА) Ц=Math.Max(0,1-ΒΨ/ΒΛ)*Ц;if(generalSpeedLimit>0) Ц=Math.Min(generalSpeedLimit,Ц);if(ƒ) Ц=0;if(Ǒ>=0) Ц=Math.Min(Ц,Ǒ);if(ΗБ) Ц*=(float) Math.Min(1,Й(ȕ.Length(),wpReachedDist)/2);Vector3 Ηρ=ф(Ϋ,Ϋ.GetShipVelocities().LinearVelocity);float ΙЋ=(float)(Math.Max(0,15-ū(-ΘΟ,-Ηρ))/15)*0.85f+0.15f;
Εΰ+=Math.Sign(ΙЋ-Εΰ)/10f;Vector3 ī=ΘΟ*Ц*Εΰ-(Ηρ);Vector3 ΔΞ=Γγ(ī,null);if(ΒЎ&&в>0.1f){ī.X*=ΓЌ(ī.X,ref ΒЏ.X,1f,ΔΞ.X,20);ī.Y*=ΓЌ(ī.Y,ref ΒЏ.Y,1f,ΔΞ.Y,20);ī.Z*=ΓЌ(ī.Z,ref ΒЏ.Z,1f,ΔΞ.Z,20);}else ΒЏ=new Vector3(1,1,1);Αη=Ţ*ī-Ɖ*Ţ;Εχ(Αη,ΒЎ);в=Vector3.Distance(δ,ΑΗ);
}float ΓЌ(float value,ref float Є,float Ǥ,float Ė,float ΔЍ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Є)) Є+=Math.Sign(Є)*Ǥ;else Є=value;if(value==0) Є=1;float ί=Math.Abs(Є);if(ί<ΔЍ||Ė==0) return ί;Є=Math.Min(ΔЍ,Math.Max(-ΔЍ,Є));ί=Math.Abs(Ė);return ί;
}void Ζρ(){float Ǐ=90,Ǩ=90,Ǣ=90;float Ǻ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȋ,ǝ,ΑΦ,ΔЅ;if(ΔІ){ȋ=Vector3.Normalize(Ђ-δ);ΑΦ=ф(Ϋ,ȋ);ǝ=ф(Ϋ,Εή);Ǐ=ū(ΑΦ,new Vector3(0,-1,0))-90;Ǩ=Ŀ(ǝ,new Vector3(-1,0,0),ǝ.Y);
Ǣ=Ŀ(ΑΦ,new Vector3(-1,0,0),ΑΦ.Z);}else{ȋ=Εί;ǝ=ф(Ϋ,Εή);ΑΦ=ф(Ϋ,Εί);ΔЅ=ф(Ϋ,ΘΠ);Ǐ=Ŀ(ǝ,new Vector3(0,0,1),ǝ.Y);Ǩ=Ŀ(ǝ,new Vector3(-1,0,0),ǝ.Y);Ǣ=Ŀ(ΔЅ,new Vector3(0,0,1),ΔЅ.X);}if(Ζυ&&ΑЅ()){Vector3 Δρ=Ϋ.GetNaturalGravity();ǝ=ф(Ϋ,Δρ);Ǐ=Ŀ(ǝ,new Vector3(0,0,1),ǝ.Y);
Ǩ=Ŀ(ǝ,new Vector3(-1,0,0),ǝ.Y);}if(!Γκ(-45,Ǩ,45)){Ǐ=0;Ǣ=0;}if(!Γκ(-45,Ǣ,45)) Ǐ=0;ΒΒ(Δσ,1,(-Ǐ)*Ǻ,(-Ǣ)*Ǻ,(-Ǩ)*Ǻ);ΒΨ=Math.Max(Math.Abs(Ǐ),Math.Max(Math.Abs(Ǩ),Math.Abs(Ǣ)));Βν=ΒΨ<=ΒΛ;}void Θξ(IMyTerminalBlock ΒЁ){if(ΒЁ==null) return;Ǿ=new float[3,2];ǥ=new Dictionary<string,float[,]>();
for(int i=0;i<ȳ.Count;i++){IMyThrust ш=ȳ[i];if(!ш.IsFunctional) continue;Vector3 ς=ф(ΒЁ,ш.WorldMatrix.Backward);float Į=ш.MaxEffectiveThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) Ǿ[0,0]+=Į;else Ǿ[0,1]-=Į;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) Ǿ[1,0]+=Į;else Ǿ[1,1]-=Į;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) Ǿ[2,0]+=Į;else Ǿ[2,1]-=Į;String ǒ=ΖΔ(ш);float[,]Ǧ=null;if(ǥ.ContainsKey(ǒ)) Ǧ=ǥ[ǒ];else{Ǧ=new float[3,2];ǥ.Add(ǒ,Ǧ);}float Ė=ш.MaxThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) Ǧ[0,0]+=Ė;else Ǧ[0,1]-=Ė;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) Ǧ[1,0]+=Ė;else Ǧ[1,1]-=Ė;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) Ǧ[2,0]+=Ė;else Ǧ[2,1]-=Ė;}}static String ΖΔ(IMyThrust ш){return ш.BlockDefinition.SubtypeId;}Vector3 Γπ(Vector3 Ǚ,float[,]Ƌ){return new Vector3(
Ǚ.X>=0?Ƌ[0,0]:Ƌ[0,1],
Ǚ.Y>=0?Ƌ[1,0]:Ƌ[1,1],
Ǚ.Z>=0?Ƌ[2,0]:Ƌ[2,1]);}bool ΖΓ(ά έ,String ǒ,out float Δλ){Δλ=0;int Β=Ȳ.IndexOf(ǒ);if(Β==-1||έ.Ǽ==null||Β>=έ.Ǽ.Length)
return false;Δλ=έ.Ǽ[Β];if(Δλ==-1) return false;return true;}Vector3 Γγ(Vector3 Ǚ,ά έ){if(έ!=null){Vector3 ί=new Vector3();for(int i=0;i<ǥ.Keys.Count;i++){String ǒ=ǥ.Keys.ElementAt(i);float ΑΔ=0;if(!ΖΓ(έ,ǒ,out ΑΔ))
return Γπ(Ǚ,Ǿ);ί+=Γπ(Ǚ,ǥ.Values.ElementAt(i))*ΑΔ;}return ί;}return Γπ(Ǚ,Ǿ);}float Ư(Vector3 Ǚ,ά έ){return Ư(Ǚ,new Vector3(),έ);}float Ư(Vector3 Ǚ,Vector3 φ,ά έ){Vector3 ΛΘ=Γγ(Ǚ,έ);Vector3 Į=ΛΘ+φ*Ţ;float ş=(Į/Ǚ).AbsMin();return(float)(Ǚ*ş).Length();}float Αυ(Vector3 Βτ,Vector3 Ɖ,ά έ)
{if(Βτ.Length()==0) return 0;float Ζτ=1;if(Ɖ.Length()>0) Ζτ=Math.Min(1,ū(-Ɖ,Βτ)/90)*0.4f+0.6f;float Αη=Ư(Βτ,Ɖ,έ);if(Αη==0) return 0.1f;float ΒΚ=Й(Αη,Ţ);float Εβ=(float) Math.Sqrt(Й(Βτ.Length(),ΒΚ*0.5f));return ΒΚ*Εβ*Ζτ*Ȃ;}float ΑЂ(Vector3 ȋ,Vector3 Б,Vector3 φ,ά έ)
{if(φ.Length()==0f) return 0;Vector3 Βυ=ņ(ȋ,Б,Vector3.Normalize(φ));float ǳ=Ư(-Βυ,έ);return ǳ/φ.Length();}void Εό(bool Κβ){float ǧ=0;float Δξ=0.9f;if(Κβ){е=-1;Ŧ=0;ΒЉ=null;if(γ!=Π.э&&Ζ.φ.Length()!=0){ǧ=Δξ*ΑЂ(Ζ.θ,Ζ.η*-1,Ζ.φ,null);if(ǧ<е||е==-1) е=ǧ;}if(Ο.κ&&Ο.φ.Length()!=0)
{ǧ=Δξ*ΑЂ(Ο.θ,Ο.η*-1,Ο.φ,null);if(ǧ<е||е==-1) е=ǧ;}return;}if(Ŧ==-1) return;if(Ŧ>=0){int ΗЈ=0;while(Ŧ<Ψ.Count){if(ΗЈ>100) return;ΗЈ++;ά έ=Ψ[Ŧ];if(έ.φ.Length()!=0f){ǧ=Δξ*Math.Min(
ΑЂ(έ.θ,έ.η*-1,έ.φ,έ),
ΑЂ(έ.θ*-1,έ.η*-1,έ.φ,έ));if(ǧ<е||е==-1) е=ǧ;}else ΒЉ=έ;Ŧ++;}Ŧ=-1;}bool Δφ=true;float Ķ=0;if(ΒЉ!=null){for(int i=0;i<ǥ.Count;i++){String ǒ=ǥ.Keys.ElementAt(i);float[,]Ǧ=ǥ.Values.ElementAt(i);float ΑΔ=0;if(!ΖΓ(ΒЉ,ǒ,out ΑΔ)){Δφ=false;break;}for(int a=0;a<Ǧ.GetLength(0);a++)
for(int b=0;b<Ǧ.GetLength(1);b++){float ŉ=Math.Abs(Ǧ[a,b]*ΑΔ);if(ŉ==0) continue;Δφ=true;if(Ķ==0||ŉ<Ķ) Ķ=ŉ;}}}if(!Δφ){for(int a=0;a<Ǿ.GetLength(0);a++)
for(int b=0;b<Ǿ.GetLength(1);b++){float ŉ=Math.Abs(Ǿ[a,b]);if(ŉ==0) continue;if(Ķ==0||ŉ<Ķ) Ķ=ŉ;}}if(Ķ>0){float Κξ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǧ=Й(Ķ,Κξ);if(ǧ>0)
if(ǧ<е||е==-1) е=ǧ;}}void Εχ(Vector3 ǳ,bool М){if(!М){for(int i=0;i<ȳ.Count;i++)
ȳ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔΝ=Γγ(ǳ,null);float Λβ=Math.Min(1,Math.Abs(Й(ǳ.X,ΔΝ.X)));float Λγ=Math.Min(1,Math.Abs(Й(ǳ.Y,ΔΝ.Y)));float Λδ=Math.Min(1,Math.Abs(Й(ǳ.Z,ΔΝ.Z)));for(int i=0;i<ȳ.Count;i++){IMyThrust ш=ȳ[i];Vector3 ς=Ιή(ф(Ϋ,ш.WorldMatrix.Backward),1);
if(ς.X!=0&&Math.Sign(ς.X)==Math.Sign(ǳ.X))
ш.SetValueFloat("Override",ш.MaxThrust*Λβ);else if(ς.Y!=0&&Math.Sign(ς.Y)==Math.Sign(ǳ.Y))
ш.SetValueFloat("Override",ш.MaxThrust*Λγ);else if(ς.Z!=0&&Math.Sign(ς.Z)==Math.Sign(ǳ.Z))
ш.SetValueFloat("Override",ш.MaxThrust*Λδ);else
ш.SetValueFloat("Override",0.0f);}}void ΒΒ(bool М,float Κύ,float Ǐ,float Ǣ,float Ǩ){for(int i=0;i<Βθ.Count;i++){IMyGyro К=Βθ[i];К.GyroOverride=М;if(!М) К.GyroPower=100;else К.GyroPower=Κύ;if(!М) continue;Vector3 Э=Ϋ.WorldMatrix.Forward;Vector3 р=Ϋ.WorldMatrix.Right;Vector3 Б=Ϋ.WorldMatrix.Up;
Vector3 Ƃ=К.WorldMatrix.Forward;Vector3 Ƅ=К.WorldMatrix.Up;Vector3 ƃ=К.WorldMatrix.Left*-1;if(Ƃ==Э) К.SetValueFloat("Roll",Ǩ);else if(Ƃ==(Э*-1)) К.SetValueFloat("Roll",Ǩ*-1);else if(Ƅ==(Э*-1)) К.SetValueFloat("Yaw",Ǩ);else if(Ƅ==Э) К.SetValueFloat("Yaw",Ǩ*-1);else if(ƃ==Э) К.SetValueFloat("Pitch",Ǩ);
else if(ƃ==(Э*-1)) К.SetValueFloat("Pitch",Ǩ*-1);if(ƃ==(р*-1)) К.SetValueFloat("Pitch",Ǐ);else if(ƃ==р) К.SetValueFloat("Pitch",Ǐ*-1);else if(Ƅ==р) К.SetValueFloat("Yaw",Ǐ);else if(Ƅ==(р*-1)) К.SetValueFloat("Yaw",Ǐ*-1);else if(Ƃ==(р*-1)) К.SetValueFloat("Roll",Ǐ);
else if(Ƃ==р) К.SetValueFloat("Roll",Ǐ*-1);if(Ƅ==(Б*-1)) К.SetValueFloat("Yaw",Ǣ);else if(Ƅ==Б) К.SetValueFloat("Yaw",Ǣ*-1);else if(ƃ==Б) К.SetValueFloat("Pitch",Ǣ);else if(ƃ==(Б*-1)) К.SetValueFloat("Pitch",Ǣ*-1);else if(Ƃ==Б) К.SetValueFloat("Roll",Ǣ);else if(Ƃ==(Б*-1)) К.SetValueFloat("Roll",Ǣ*-1);
}}void Ζλ(){this.Δσ=false;}void л(Vector3 ĭ,Vector3 Э,Vector3 ΔЄ,float ĝ,bool Αμ){ΓΙ(ĭ,ĝ,Αμ);ΒΛ=ĝ;ΔІ=false;this.Εί=Э;this.ΘΠ=ΔЄ;}void л(Vector3 ĭ,Vector3 Э,Vector3 ΔЄ,bool Αμ){л(ĭ,Э,ΔЄ,2f,Αμ);}void ΓΙ(Vector3 ĭ,float ĝ,bool Αμ){ΒΛ=ĝ;this.Δσ=true;this.Ζυ=Αμ;ΔІ=true;
Βν=false;this.Εή=ĭ;}void ΓБ(){Ȁ(false,false,false,Ђ,0);ΒЎ=false;}void и(Vector3 ζ,float Ǻ){Ȁ(true,false,false,ζ,Ǻ);}void Ȁ(bool М,bool ΕΩ,bool Εν,Vector3 ζ,float Ǻ){Ȁ(М,ΕΩ,Εν,ζ,ζ-δ,0.0f,Ǻ);}void Ȁ(bool М,bool ΕΩ,bool Εν,Vector3 ζ,Vector3 ΚЊ,float ΚЋ,float Ǻ){ΒЎ=true;
this.ΘΥ=М;Ђ=ζ;this.ΔЌ=Ǻ;this.ΕΗ=ΚЋ;this.ΗБ=ΕΩ;this.ΗА=Εν;this.ΗΤ=ΚЊ;в=Vector3.Distance(ζ,δ);}void Ιΰ(){χ.GetBlocksOfType(ƿ,Ǖ);}void Ια(){if(ΒЃ){А=2;return;}List<IMyRemoteControl>Αέ=new List<IMyRemoteControl>();List<IMySensorBlock>ΗЍ=new List<IMySensorBlock>();List<IMyTerminalBlock>Αε=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αέ,Ǖ);χ.GetBlocksOfType(Х,Ǖ);χ.GetBlocksOfType(ΗЍ,Ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αε,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Х=ǩ(Х,pamTag,true);Ζγ();Ζδ(Αε);Ņ(Х,setLCDFontAndSize,1.4f,false);Ņ(Ȕ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘΛ=ǩ(ΗЍ,pamTag,true);if(ΘΛ.Count>0) ψ=ΘΛ[0];else ψ=null;if(Μ==Κ.ƶ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Μ==Κ.Ǫ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Μ==Κ.ή){χ.GetBlocksOfType(Εγ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘΚ=ǩ(Αέ,pamTag,true);if(ΘΚ.Count>0) Αέ=ΘΚ;if(Αέ.Count>0) Ϋ=Αέ[0];else{Ϋ=null;А=2;Λ="Remote is missing";return;}ν=(float) Ϋ.CubeGrid.WorldVolume.Radius*2;Ǡ=null;if(Μ!=Κ.ή){Εύ();if(ω.Count>0&&Ǡ!=null){if(ψ!=null&&(Ǡ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ϋ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ϋ.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(Ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Forward&&
Ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void Ιβ(){χ.GetBlocksOfType(Ȯ,Ǖ);χ.GetBlocksOfType(љ,Ǖ);χ.GetBlocksOfType(ȳ,Ǖ);χ.GetBlocksOfType(Βθ,Ǖ);χ.GetBlocksOfType(ĩ,Ǖ);χ.GetBlocksOfType(ķ,Ǖ);χ.GetBlocksOfType(Ǜ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΒЈ,Ǖ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
љ=ǩ(љ,"ConnectorMedium",false);else
љ=ǩ(љ,"Connector",false);List<IMyShipConnector>ΘΘ=ǩ(љ,pamTag,true);if(ΘΘ.Count>0) љ=ΘΘ;if(А<=1){if(љ.Count==0){А=1;Λ="Connector is missing";}if(Βθ.Count==0){А=1;Λ="Gyros are missing";}if(ȳ.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΘΜ=ǩ(ΒЈ,pamTag,true);
if(ΘΜ.Count>0) ΒЈ=ΘΜ;List<IMyLandingGear>ΘΙ=ǩ(Ȯ,pamTag,true);if(ΘΙ.Count>0) Ȯ=ΘΙ;for(int i=0;i<Ȯ.Count;i++) Ȯ[i].AutoLock=false;List<IMyBatteryBlock>ΘΗ=ǩ(ĩ,pamTag,true);if(ΘΗ.Count>0) ĩ=ΘΗ;List<IMyGasTank>ΘΝ=ǩ(Ǜ,pamTag,true);if(ΘΝ.Count>0) Ǜ=ΘΝ;}void Ιγ(){χ.GetBlocksOfType(Ŋ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ſ.Clear();for(int i=Ŋ.Count-1;i>=0;i--){if(ΙΚ(Ŋ[i])){ſ.Add(Ŋ[i]);Ŋ.RemoveAt(i);}}}bool ΙΚ(IMyTerminalBlock Ω){if(Ω is IMyCargoContainer) return true;if(Ω is IMyShipDrill) return true;if(Ω is IMyShipGrinder) return true;if(Ω is IMyShipConnector){if(((IMyShipConnector) Ω).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Γη(Ω,"ConnectorSmall",false))
return false;else return true;}return false;}void Εύ(){if(Ϋ==null) return;Ǡ=null;float Βό=0,Βχ=0,Βύ=0,Βψ=0,Βώ=0,Βω=0;List<IMyTerminalBlock>Εέ=ǩ(ω,pamTag,true);bool Κτ=Εέ.Count==0;if(Εέ.Count>0) Ǡ=Εέ[0];else if(ω.Count>0) Ǡ=ω[0];int Ȩ=0;float Ɩ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=Ǡ.WorldMatrix.Forward){if(Κτ){А=2;Λ="Mining direction is unclear!";return;}continue;}Ȩ++;Ɩ=Math.Max(Ɩ,Ισ(ω[i]));Vector3 Н=ΑЁ(Ϋ,ω[i].GetPosition());if(i==0){Βό=Н.X;Βχ=Н.X;Βύ=Н.Y;Βψ=Н.Y;Βώ=Н.Z;Βω=Н.Z;}Βχ=Math.Max(Н.X,Βχ);Βό=Math.Min(Н.X,Βό);
Βψ=Math.Max(Н.Y,Βψ);Βύ=Math.Min(Н.Y,Βύ);Βω=Math.Max(Н.Z,Βω);Βώ=Math.Min(Н.Z,Βώ);}if(Ɩ==0) Ɩ=drillRadius;bool ĭ=Ǡ!=null&&Ǡ.WorldMatrix.Forward==Ϋ.WorldMatrix.Down;ǽ=(Βχ-Βό)*(1-ŧ)+Ɩ*2;ȗ=(ĭ?Βω-Βώ:Βψ-Βύ)*(1-Œ)+Ɩ*2;IMyCubeGrid g=Ϋ.CubeGrid;float Ερ=0,Ες=0,Εσ=0,Ετ=0;
for(int c=0;c<8;c++){Vector3 p=ΑЁ(Ϋ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ĭ?p.Z:p.Y;if(c==0||p.X<Ερ) Ερ=p.X;if(c==0||p.X>Ες) Ες=p.X;if(c==0||y<Εσ) Εσ=y;if(c==0||y>Ετ) Ετ=y;}ΒЅ=Ες-Ερ+g.GridSize;ΕΤ=Ετ-Εσ+g.GridSize;}float Ισ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǩ<T>(List<T>Ѝ,String ǻ,bool Βι){List<T>ί=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(Γη(Ѝ[i],ǻ,Βι)) ί.Add(Ѝ[i]);return ί;}bool Γη<T>(T Ω,String ǻ,bool Βι){IMyTerminalBlock b=(IMyTerminalBlock) Ω;if(Βι&&b.CustomName.ToUpper().Contains(ǻ.ToUpper())) return true;if(!Βι&&b.BlockDefinition.ToString().ToUpper().Contains(ǻ.ToUpper())) return true;return false;}void Ǘ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
Ζα((IMyTerminalBlock) Ѝ[i],М);}void Ζα(IMyTerminalBlock Ω,bool М){if(Ω==null) return;String ΙЉ=М?"OnOff_On":"OnOff_Off";var a=Ω.GetActionWithName(ΙЉ);a.Apply(Ω);}void ȁ(bool М){ΕΪ=М;if(!ţ) return;Ǘ(ΒЈ,М);}void ΓЈ(ChargeMode Џ){for(int i=0;i<ĩ.Count;i++) ĩ[i].ChargeMode=Џ;}void ΓЊ(bool ΛΑ)
{for(int i=0;i<Ǜ.Count;i++) Ǜ[i].Stockpile=ΛΑ;}void ƹ(List<IMyLandingGear>Δο,bool Ης){for(int i=0;i<Δο.Count;i++){if(Ης) Δο[i].Lock();if(!Ης) Δο[i].Unlock();}}void Ζβ(bool М){if(ΗΓ==М) return;List<IMyShipController>ΒΥ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΥ,Ǖ);if(ΒΥ.Count==0) return;
for(int i=0;i<ΒΥ.Count;i++)
ΒΥ[i].DampenersOverride=М;ΗΓ=М;}IMyShipConnector з(MyShipConnectorStatus ǐ){for(int i=0;i<љ.Count;i++){if(!Ȝ(љ[i],true)) continue;if(љ[i].Status==ǐ) return љ[i];}return null;}void Ň(bool ΕΥ,bool Βά,float ΚΪ,float ΙЏ){if(ψ==null||ω.Count==0) return;Vector3 ΒΠ=new Vector3();
int Ȩ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=Ǡ.WorldMatrix.Forward) continue;Ȩ++;ΒΠ+=ω[i].GetPosition();}ΒΠ=ΒΠ/Ȩ;Vector3 Ə=ΑЁ(ψ,ΒΠ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΥ;ψ.LeftExtend=(Βά?1:ѐ)/2f*ǽ-Ə.X;ψ.RightExtend=(Βά?1:ѐ)/2f*ǽ+Ə.X;ψ.TopExtend=(Βά?1:я)/2f*ȗ+Ə.Y;
ψ.BottomExtend=(Βά?1:я)/2f*ȗ-Ə.Y;ψ.FrontExtend=(ΕΥ?н:ΚΪ)-Ə.Z;ψ.BackExtend=ΕΥ?0:ΙЏ+ν*0.75f+Ə.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void Ņ<T>(List<T>Ѝ,bool ΚЍ,float ΚΨ,bool Κγ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Αδ=null;if(Ѝ[i]is IMyTextSurface) Αδ=(IMyTextSurface) Ѝ[i];if(Αδ!=null){Αδ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΚЍ) continue;Αδ.Font="Debug";if(Κγ) continue;Αδ.FontSize=ΚΨ;}}}bool ΑЅ(){Vector3D ι;return this.Ϋ.TryGetPlanetPosition(out ι);}int ΘЍ(Vector3 ΛΓ,int ΘΑ){if(ΘΑ==-1) return 0;double Ы=-1;int ċ=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,ΛΓ);if(Ы==-1||ϊ<Ы){ċ=i;Ы=ϊ;}
}return Math.Sign(ċ-ΘΑ);}bool ΙΛ(Vector3 ι){List<Vector3>а=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) а.Add(Ψ[i].Ε);if(Ο.κ&&Ψ.Count>=1){Vector3 ź=new Vector3();ț(Ο,dockDist*ν,false,out ź);if(Vector3.Distance(Ο.Ε,Ψ.First().Ε)<Vector3.Distance(Ο.Ε,Ψ.Last().Ε)){а.Insert(0,ź);а.Insert(0,Ο.Ε);}
else{а.Add(ź);а.Add(Ο.Ε);}}if(Μ==Κ.ή){if(Ζ.κ&&Ψ.Count>=1){Vector3 ΔΙ=new Vector3();ț(Ζ,dockDist*ν,false,out ΔΙ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){а.Insert(0,ΔΙ);а.Insert(0,Ζ.Ε);}else{а.Add(ΔΙ);а.Add(Ζ.Ε);}}}else{if(γ!=Π.э)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
а.Insert(0,Ζ.Ε);else а.Add(Ζ.Ε);}int ċ=-1;double Ȇ=-1;for(int i=0;i<а.Count;i++){double ϊ=Vector3.Distance(а[i],ι);if(ϊ<Ȇ||Ȇ==-1){Ȇ=ϊ;ċ=i;}}if(а.Count==0) return false;double Δε=Vector3.Distance(а[ċ],ι);double Κώ=Vector3.Distance(а[Math.Max(0,ċ-1)],а[ċ])*1.5f;
double Κσ=Vector3.Distance(а[Math.Min(а.Count-1,ċ+1)],а[ċ])*1.5f;return Δε<Κώ||Δε<Κσ||Δε<laneOffset*ν*1.5f;}static float Й(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ιή(Vector3 v,int Δα){return new Vector3(Math.Round(v.X,Δα),Math.Round(v.Y,Δα),Math.Round(v.Z,Δα));
}float ū(Vector3 a,Vector3 b){if(a==b) return 0;float ΚΤ=(a*b).Sum;float Ημ=a.Length();float Ην=b.Length();if(Ημ==0||Ην==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(ΚΤ/(Ημ*Ην)));if(float.IsNaN(ί)) return 0;return ί;}float Ŀ(Vector3 v,Vector3 ΒЁ,float ΚЎ){float Ÿ=ū(v,ΒЁ);
if(ΚЎ>0f) Ÿ*=-1;if(Ÿ>-90f) return Ÿ-90f;else return 180f-(-Ÿ-90f);}double ΕЄ(float ΚΜ){return(Math.PI/180)*ΚΜ;}bool Γκ(double Κν,double value,double ƌ){return(value>=Κν&&value<=ƌ);}Vector3 Ιτ(IMyTerminalBlock Ω,Vector3 Н){return Vector3D.Transform(Н,Ω.WorldMatrix);
}Vector3 ΑЁ(IMyTerminalBlock Ω,Vector3 Λί){return ф(Ω,Λί-Ω.GetPosition());}Vector3 ф(IMyTerminalBlock Ω,Vector3 Επ){return Vector3D.TransformNormal(Επ,MatrixD.Transpose(Ω.WorldMatrix));}Vector3 ņ(Vector3 ȋ,Vector3 Б,Vector3 Επ){MatrixD ΔЋ=MatrixD.CreateFromDir(ȋ,Б);return Vector3D.TransformNormal(Επ,MatrixD.Transpose(ΔЋ));
}Vector3 ΔΓ(Vector3 ȋ,Vector3 Б,Vector3 Н){MatrixD ΔЋ=MatrixD.CreateFromDir(ȋ,Б);return Vector3D.Transform(Н,ΔЋ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 π(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ρ(τ,0)),float.Parse(Ρ(τ,1)),float.Parse(Ρ(τ,2)));
}String ΙЇ(ά έ){String П=":";String ί=σ(έ.Ε)+П+σ(έ.θ)+П+
σ(έ.η)+П+σ(έ.Ё)+П+σ(έ.φ);for(int i=0;i<έ.Ǽ.Length;i++)
ί+=П+Math.Round(έ.Ǽ[i],3);return ί;}ά Ιν(String s){String[]τ=s.Split(':');ά έ=new ά(
π(Ρ(τ,0)),π(Ρ(τ,1)),
π(Ρ(τ,2)),π(Ρ(τ,3)),
π(Ρ(τ,4)));int Β=5;List<float>ΗΠ=new List<float>();while(Β<τ.Length){String ΓΓ=Ρ(τ,Β);float f=0;if(!float.TryParse(ΓΓ,out f)) break;ΗΠ.Add(f);Β++;}έ.Ǽ=ΗΠ.ToArray();return έ;}System.Text.StringBuilder ƙ=new System.Text.StringBuilder();void Χ<T>(T value,bool Κς){if(Κς) ƙ.Append('\n');ƙ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String Αϊ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΗ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ρ(String[]ΙЍ,int Β){String ΓΓ=ΙЍ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΓ)) return "";
return ΓΓ;}void Save(){if(ΒЃ||Μ==Κ.ї){Storage="";return;}Storage=ΓΟ();}String ΓΟ(){ƙ.Clear();ƙ.Append(DATAREV+";");Χ(σ(ΒЂ),false);Χ(σ(Ο.θ));Χ(σ(Ο.Ё));Χ(σ(Ο.η));Χ(σ(Ο.φ));Χ(σ(Ο.Ε));Χ(σ(Ο.о));Χ(Ο.κ);Χ(σ(Ζ.Ε));Χ(σ(Ζ.φ));Χ(σ(Ζ.θ));Χ(σ(Ζ.η));Χ(σ(Ζ.Ё));Χ(σ(Ζ.о));Χ(Ζ.κ);Χ(σ(Ǟ));Χ(σ(ė));Χ(";");
Χ((int) Μ,false);Χ((int) γ);Χ((int) ѝ);Χ(Ȏ);Χ(ў);Χ(п);Χ(џ);Χ(Ȃ);Χ(ğ);Χ(Ğ);if(Μ==Κ.ή){Χ((int) Ї.Р);Χ(Ї.Ǳ);Χ(Ї.į);Χ(Αϊ(Ї.ǲ));Χ(Αϊ(Ї.ǵ));Χ((int) Ј.Р);Χ(Ј.Ǳ);Χ(Ј.į);Χ(Αϊ(Ј.ǲ));Χ(Αϊ(Ј.ǵ));}else{Χ((int) Ȓ);Χ((int) ђ);Χ((int) д);Χ((int) њ);Χ((int) Ŗ);Χ(Ļ);Χ(ţ);Χ(Ƈ);Χ(Ɓ);Χ(ѐ);Χ(я);Χ(н);Χ(ƪ);Χ(Ʃ);
Χ(ŧ);Χ(Œ);Χ(Ќ);Χ(Ю);Χ(Ѐ);Χ(ύ);Χ(ĕ);Χ(б);Χ(Š);Χ(ř);}Χ(";");for(int i=0;i<Ȳ.Count;i++)
Χ((i>0?"|":"")+Ȳ[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΙЇ(Ψ[i]),i>0);Χ(";",false);Χ(г+"|"+Д,false);Χ(";",false);Χ(Θώ(),false);String Θ=ƙ.ToString();ƙ.Clear();return Θ;}ј Ιέ(){return ΑЏ(Storage);}ј ΑЏ(String Θ){if(Θ=="") return ј.ΙΤ;String[]š=Θ.Split(';');if(Ρ(š,0)!=DATAREV) return ј.Γφ;int Β=0;try{String[]Ξ=Ρ(š,1).Split('\n');
ΒЂ=π(Ρ(Ξ,Β++));Ο.θ=π(Ρ(Ξ,Β++));Ο.Ё=π(Ρ(Ξ,Β++));Ο.η=π(Ρ(Ξ,Β++));Ο.φ=π(Ρ(Ξ,Β++));Ο.Ε=π(Ρ(Ξ,Β++));Ο.о=π(Ρ(Ξ,Β++));Ο.κ=bool.Parse(Ρ(Ξ,Β++));Ζ.Ε=π(Ρ(Ξ,Β++));Ζ.φ=π(Ρ(Ξ,Β++));Ζ.θ=π(Ρ(Ξ,Β++));Ζ.η=π(Ρ(Ξ,Β++));Ζ.Ё=π(Ρ(Ξ,Β++));Ζ.о=π(Ρ(Ξ,Β++));Ζ.κ=bool.Parse(Ρ(Ξ,Β++));
Ǟ=π(Ρ(Ξ,Β++));ė=π(Ρ(Ξ,Β++));Ξ=Ρ(š,2).Split('\n');Β=0;Μ=(Κ) int.Parse(Ρ(Ξ,Β++));γ=(Π) int.Parse(Ρ(Ξ,Β++));ѝ=(Π) int.Parse(Ρ(Ξ,Β++));Ȏ=int.Parse(Ρ(Ξ,Β++));ў=int.Parse(Ρ(Ξ,Β++));п=int.Parse(Ρ(Ξ,Β++));џ=int.Parse(Ρ(Ξ,Β++));Ȃ=float.Parse(Ρ(Ξ,Β++));ğ=bool.Parse(Ρ(Ξ,Β++));
Ğ=bool.Parse(Ρ(Ξ,Β++));if(Μ==Κ.ή){Ї.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Ї.Ǳ=float.Parse(Ρ(Ξ,Β++));Ї.į=float.Parse(Ρ(Ξ,Β++));Ї.ǲ=ΒΗ(Ρ(Ξ,Β++));Ї.ǵ=ΒΗ(Ρ(Ξ,Β++));Ј.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Ј.Ǳ=float.Parse(Ρ(Ξ,Β++));Ј.į=float.Parse(Ρ(Ξ,Β++));Ј.ǲ=ΒΗ(Ρ(Ξ,Β++));Ј.ǵ=ΒΗ(Ρ(Ξ,Β++));}else
{Ȓ=(ю) int.Parse(Ρ(Ξ,Β++));ђ=(ъ) int.Parse(Ρ(Ξ,Β++));д=(Г) int.Parse(Ρ(Ξ,Β++));њ=(ǔ) int.Parse(Ρ(Ξ,Β++));Ŗ=(ю) int.Parse(Ρ(Ξ,Β++));Ļ=bool.Parse(Ρ(Ξ,Β++));ţ=bool.Parse(Ρ(Ξ,Β++));Ƈ=bool.Parse(Ρ(Ξ,Β++));Ɓ=bool.Parse(Ρ(Ξ,Β++));ѐ=int.Parse(Ρ(Ξ,Β++));я=int.Parse(Ρ(Ξ,Β++));н=int.Parse(Ρ(Ξ,Β++));
ƪ=float.Parse(Ρ(Ξ,Β++));Ʃ=float.Parse(Ρ(Ξ,Β++));ŧ=float.Parse(Ρ(Ξ,Β++));Œ=float.Parse(Ρ(Ξ,Β++));Ќ=int.Parse(Ρ(Ξ,Β++));Ю=int.Parse(Ρ(Ξ,Β++));Ѐ=int.Parse(Ρ(Ξ,Β++));ύ=int.Parse(Ρ(Ξ,Β++));ĕ=int.Parse(Ρ(Ξ,Β++));б=int.Parse(Ρ(Ξ,Β++));Š=int.Parse(Ρ(Ξ,Β++));ř=float.Parse(Ρ(Ξ,Β++));
}Ξ=Ρ(š,3).Replace("\n","").Split('|');Ȳ=Ξ.ToList();Ξ=Ρ(š,4).Split('\n');Ψ.Clear();if(Ξ.Count()>=1&&Ξ[0]!="")
for(int i=0;i<Ξ.Length;i++)
Ψ.Add(Ιν(Ρ(Ξ,i)));Ξ=Ρ(š,5).Split('|');г=Ρ(Ξ,0);if(!int.TryParse(Ρ(Ξ,1),out Д)) Д=-1;if(г.Length!=Ќ*Ю){г="";Д=-1;}Θύ(Ρ(š,6));}catch{return ј.ΕЇ;}ƚ=ѝ;Ѓ();return ј.ΒΔ;}const int Εϋ=64000;IMyTerminalBlock ΕЈ(){List<IMyTerminalBlock>Ž=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ž,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ž.Count>0?Ž[0]:null;}void ΘЂ(){IMyTerminalBlock Ω=ΕЈ();if(Ω==null){Λ="Export: name a block "+dataTag;return;}String Θ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΓΟ();if(Θ.Length>Εϋ){Λ="Export too large: "+Θ.Length+" chars, max "+Εϋ;return;}Ω.CustomData=Θ;Λ="Exported "+Ψ.Count+" waypoints";}void ΙΗ(bool Δώ){IMyTerminalBlock Ω=ΕЈ();if(Ω==null){Λ="Import: name a block "+dataTag;return;}
ΙΘ(Ω.CustomData,Δώ);}void ΙΘ(String Θ,bool Δώ){Θ=Θ.Replace("\r","");if(Θ.StartsWith("//")){int ΗЀ=Θ.IndexOf('\n');Θ=ΗЀ<0?"":Θ.Substring(ΗЀ+1);}Θ=Θ.Trim();if(Θ==""){Λ="Import: no PAM data in "+dataTag;return;}Ѓ();String Ζό=ΓΟ();ј ί=ΑЏ(Θ);if(ί==ј.ΒΔ&&Δώ){ά Φ=new ά(Ζ);
Vector3 Κπ=Ǟ,Κο=ė;int w=ѐ,h=я,d=н;ю Αβ=Ȓ;ǔ Δβ=њ;float Κχ=ŧ,Κφ=Œ;ΑЏ(Ζό);Ζ=Φ;Ǟ=Κπ;ė=Κο;ѐ=w;я=h;н=d;Ȓ=Αβ;њ=Δβ;ŧ=Κχ;Œ=Κφ;γ=Π.ų;ƽ(true,true);γ=Π.ų;ѝ=Π.ų;Λ="Imported job, run CONT";return;}if(ί==ј.ΒΔ){ΑΪ=false;Λ="Imported "+Ψ.Count+" waypoints";
return;}ΑЏ(Ζό);Λ=ί==ј.Γφ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζγ(){Ŏ.Clear();for(int i=Х.Count-1;i>=0;i--){String Θ=Х[i].CustomData.ToUpper();bool Δό=false;if(Θ=="INSTRUCTIONS"){Δό=true;ΒΫ=true;}if(Θ=="DEBUG") Δό=true;if(Δό){Ŏ.Add(Х[i]);Х.RemoveAt(i);}
}Ņ(Ŏ,false,1,false);}void Ζδ(List<IMyTerminalBlock>Ž){Ȕ.Clear();for(int i=0;i<Ž.Count;i++){IMyTerminalBlock Ω=Ž[i];try{String ΗЇ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ω.CustomName.IndexOf(ΗЇ);int Ɵ=-1;if(Β<0||!int.TryParse(Ω.CustomName.Substring(Β+ΗЇ.Length,1),out Ɵ)) continue;
if(Ɵ==-1) continue;Ɵ--;IMyTextSurfaceProvider ΗЉ=(IMyTextSurfaceProvider) Ω;if(Ɵ<ΗЉ.SurfaceCount&&Ɵ>=0)
Ȕ.Add(ΗЉ.GetSurface(Ɵ));}catch{}}}String Βϊ="";void Ζή(){String ΔЊ="";String ΑΓ="";Βϊ=Εϊ(false);ΔЊ+=Βϊ;ΑΓ+=Βϊ;ΑΓ+=ΘЊ();for(int i=0;i<Х.Count;i++) Х[i].WriteText(ΔЊ);for(int i=0;i<Ȕ.Count;i++) Ȕ[i].WriteText(ΔЊ);Echo(ΑΓ);for(int i=0;i<Ŏ.Count;i++){IMyTextPanel ч=Ŏ[i];
String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΕЏ());}}string ΕЏ(){String Γ="";try{float Ηυ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ż*Ηυ)+" / "+(int)(ƍ*Ηυ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ż*100f,1)+"% / "+Math.Round(ƍ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ǃ,2)+"ms / "+Math.Round(ΔЎ,2)+"ms \n";}catch{}for(int i=0;i<Ę.Count;i++)
Γ+=""+Ę.Keys.ElementAt(i)+" = "+Math.Round(Ę.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ę.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙΓ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ņ(Х,setLCDFontAndSize,0.9f,false);Ņ(Ȕ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Х.Count;i++) Х[i].WriteText(Γ+Ĕ.ToString());for(int i=0;i<Ȕ.Count;i++) Ȕ[i].WriteText(Γ+Ĕ.ToString());}void Ƞ(){if(!ΒΫ) return;try{ΕΚ=Runtime.CurrentInstructionCount;}catch{}
}void Ț(String ΰ){if(!ΒΫ) return;if(ΕΚ==0) return;try{float ΓΑ=(Runtime.CurrentInstructionCount-ΕΚ)/Runtime.MaxInstructionCount*100;if(!Ę.ContainsKey(ΰ))
Ę.Add(ΰ,new float[]{ΓΑ,ΓΑ});else{Ę[ΰ][0]=ΓΑ;Ę[ΰ][1]=Math.Max(ΓΑ,Ę[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Θζ(String Ȋ){if(!ž) return;String П=""+BROADCAST_SEP;String β=VERSION+П;β+=ƫ(Ɲ)+П;
β+=(int) Μ+П;β+=Ů(δ.X)+""+П;β+=Ů(δ.Y)+П;β+=Ů(δ.Z)+П;if(Μ!=Κ.ή)
β+=Γΰ(ѝ)+(ѝ==Π.В?" "+Math.Round(ΑΠ,1)+"%":"")+П;else β+=Γΰ(ѝ)+П;β+=ƫ(Λ)+П;β+=Ў+""+П;β+=ƫ(Βϊ)+П;β+=Ȉ+П;β+=ȏ+П;for(int i=0;i<Ǵ.Count;i++){if(Ǵ[i].Ɗ!=Ш.Ű) continue;β+=ƫ(Ǵ[i].ΰ)+"/"+ƫ(Ǵ[i].υ)+"/"+Ǵ[i].Њ+П;}Ιη(β,Ȋ);}String ƫ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ιη(String β,String Ȋ){ΓІ(β,BROADCAST_SELF_ID,Ȋ);}void ΓІ(String β,String ζ,String Ȋ){try{if(!ž) return;β=BROADCAST_TAG+BROADCAST_SEP+ΕЋ()+BROADCAST_SEP+ζ+BROADCAST_SEP+ΔΦ+BROADCAST_SEP+Ȋ+BROADCAST_SEP+β;this.IGC.SendBroadcastMessage(BROADCAST_TAG,β);}catch(Exception e){Ĕ=e;}
}String ΕЋ(){if(Μ!=Κ.ї) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γο(String β){return β.StartsWith(BROADCAST_TAG);}bool ж(ref String Θ,out string ΑΖ,Char П){int Β=Θ.IndexOf(П);ΑΖ="";if(Β<0) return false;ΑΖ=Θ.Substring(0,Β);Θ=Θ.Remove(0,Β+1);return true;}bool Ζέ(ref String β,out String ȑ,out String Ȋ)
{ȑ="";Ȋ="";if(!ž) return false;String ǻ="";if(!ж(ref β,out ǻ,BROADCAST_SEP)||!Γο(ǻ)) return false;if(!ж(ref β,out ȑ,BROADCAST_SEP)) return false;if(!ж(ref β,out ǻ,BROADCAST_SEP)||(ǻ!=ΕЋ()&&(ǻ!="*"&&Μ!=Κ.ї))) return false;if(!ж(ref β,out ǻ,BROADCAST_SEP)||(ǻ!=ΔΦ)) return false;
if(!ж(ref β,out Ȋ,BROADCAST_SEP)) return false;return true;}void ΖΛ(){bool ǈ=true;if(ΗΣ){ΗΣ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ΑΜ=(Μ!=Κ.ї?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΜ)
ΖΜ(ΑΜ);String[]Ξ=Me.CustomData.Split('\n');ž=bool.Parse(ΑЎ(Αό(Ξ,"Enable_Broadcast",ref ǈ)));bool Εξ=false;bool ΗΗ=true;if(ž){if(Μ!=Κ.ї)
Ɲ=ΑЎ(Αό(Ξ,"Ship_Name",ref ǈ)).Replace(BROADCAST_SELF_ID,"");ΔΦ=ΑЎ(Αό(Ξ,"Broadcast_Channel",ref ǈ)).ToLower();ΗΗ=false;if(ǯ==null){ǯ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ǯ.SetMessageCallback("");}List<IMyRadioAntenna>Ź=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ź);bool ΔΘ=false;
for(int i=0;i<Ź.Count;i++)
if(Ź[i].EnableBroadcasting&&Ź[i].Enabled){ΔΘ=true;break;}if(Ź.Count==0) Λ="No Antenna found";else if(!ΔΘ) Λ="Antenna not ready";Εξ=Ź.Count==0||!ΔΘ;if(Ζφ&&!Εξ&&Μ!=Κ.ї) Λ="Antenna ok";}else if(Μ==Κ.ї)
Λ="Offline - Enable in PB custom data";Ζφ=Εξ;if(ΗΗ){if(ǯ!=null) this.IGC.DisableBroadcastListener(ǯ);ǯ=null;}}catch{ǈ=false;}if(!ǈ){Λ="Reset custom data";ΖΜ(ΑΜ);}}void ΖΜ(String ΑΜ){String Θ=ΑΜ+"\n\n"+"Enable_Broadcast="+(Μ==Κ.ї?"true":"false")+" \n";Θ+=Μ!=Κ.ї?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Θ+"Broadcast_Channel=#default";}String ΑЎ(String Ř){int Β=Ř.IndexOf("//");if(Β!=-1) Ř=Ř.Substring(0,Β);String[]τ=Ř.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αό(String[]Ξ,String ȭ,ref bool ȯ){foreach(String Ř in Ξ)
if(Ř.StartsWith(ȭ)) return Ř;ȯ=false;return "";}void Θϊ(string Т){if(č) Ь=new List<Ѕ>();String ȑ="";String Ȋ="";if(!Γο(Т)) ΙΫ(Т);bool ΗЎ=false;int ΔБ=50;while(ǯ!=null&&ǯ.HasPendingMessage&&ΔБ-->0){MyIGCMessage β=ǯ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;
if(Ζέ(ref Θ,out ȑ,out Ȋ)&&ȑ!=""&&ȑ!=BROADCAST_SELF_ID){Ѕ Δ=Γή(ȑ,"");if(Δ==null){Δ=new Ѕ(ȑ);Ь.Add(Δ);}Δ.ń(Ȋ,false,false,true);ΙЁ(Δ,Θ);ΗЎ=true;}}if(ΗЎ)
Ь.Sort(delegate(Ѕ a,Ѕ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ē||č){if(Αή<=0||č){Λ="";č=false;Ιε();ΖΛ();ΔЃ=DateTime.Now;}}if(ŕ) Ιω();if(ŕ) Ιύ();if(ŕ) ΙЅ();Ιχ();}void ΙЁ(Ѕ Δ,String Θ)
{Δ.ΔЀ=DateTime.Now;String ΑΖ="";ж(ref Θ,out Δ.ΓΕ,BROADCAST_SEP);ж(ref Θ,out Δ.ΰ,BROADCAST_SEP);if(Δ.ΓΕ!=VERSION) return;String ΑΩ="";String[]ƕ=new string[3];ж(ref Θ,out ΑΩ,BROADCAST_SEP);ж(ref Θ,out ƕ[0],BROADCAST_SEP);ж(ref Θ,out ƕ[1],BROADCAST_SEP);ж(ref Θ,out ƕ[2],BROADCAST_SEP);
ж(ref Θ,out Δ.ΕΫ,BROADCAST_SEP);ж(ref Θ,out Δ.Λ,BROADCAST_SEP);ж(ref Θ,out ΑΖ,BROADCAST_SEP);ж(ref Θ,out Δ.Βϋ,BROADCAST_SEP);String Θά="",Ηψ="";ж(ref Θ,out Θά,BROADCAST_SEP);ж(ref Θ,out Ηψ,BROADCAST_SEP);Δ.Ћ.Clear();while(true){String Ηζ;if(!ж(ref Θ,out Ηζ,BROADCAST_SEP)) break;String[]Βο=Ηζ.Split('/');
if(Βο.Count()<3) continue;int Њ=0;if(!int.TryParse(Βο[2],out Њ)) continue;Δ.Ћ.Add(new м(Βο[0],Βο[1],Њ,Ш.Ű));}int Ηό=0;if(!int.TryParse(ΑΖ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΩ,out Ηό)) Δ.Μ=Κ.ħ;if(!float.TryParse(ƕ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ƕ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(ƕ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(Θά,out Δ.Ȉ)) Δ.Ȉ=0;if(!float.TryParse(Ηψ,out Δ.ȏ)) Δ.ȏ=0;Δ.Μ=(Κ) Ηό;Δ.Δζ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Εη=0;for(int i=0;i<Δ.Ћ.Count();i++) Δ.Εη+=Δ.Ћ[i].Њ;}Ѕ Γή(String Υ,String ΰ){Υ=Υ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Ь.Count;i++){if(Υ!=""&&Ь[i].Υ.ToUpper()==Υ) return Ь[i];if(ΰ!=""&&Ь[i].ΰ!=null&&Ь[i].ΰ.ToUpper()==ΰ) return Ь[i];}return null;}String ƀ="";Ѕ ΖІ=null;DateTime ΖЇ=DateTime.MinValue;List<Ѕ>ŋ=new List<Ѕ>();const float Θλ=20f;bool Ζζ(Ѕ ζ,String І){return І==ƀ&&ζ==ΖІ&&(DateTime.Now-ΖЇ).TotalSeconds<Θλ;
}String ΘЄ(Ѕ ζ){if(ƀ==""||!Ζζ(ζ,ƀ)) return "";String s="";int ȯ=0,ΕΦ=0;for(int i=0;i<ŋ.Count;i++){Љ ΒЊ=ŋ[i].ń(ƀ,false,false,false);if(ΒЊ==Љ.Ʒ) ȯ++;if(ΕΦ++<8) s+="  "+ŋ[i].ΰ+": "+(ΒЊ==Љ.Ʒ?"received":ΒЊ==Љ.Ŵ?"pending...":ΒЊ==Љ.ΑЋ?"no answer!":"-")+"\n";
}if(ΕΦ>8) s+="  +"+(ΕΦ-8)+" more\n";return "--------------------------------------------\n"+ƀ+": "+ȯ+"/"+ŋ.Count+" received\n"+s;}void ǫ(Ѕ Δ,String І){ƀ=І;ΖІ=Δ;ΖЇ=DateTime.Now;ŋ.Clear();if(Δ==null) ŋ.AddRange(Ь);else ŋ.Add(Δ);if(Δ==null){for(int i=0;i<Ь.Count;i++)
Ь[i].ń(І,true,false,false);ΓІ(І,"*",І);}else{Δ.ń(І,true,false,false);ΓІ(І,Δ.Υ,І);}}void ΙΫ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȣ=τ.ElementAtOrDefault(1);String Ȭ="Invalid argument: "+Т;if(ό==Σ.ƺ){if(Л!=null)
{switch(І){case "UP":{if(Ў<2) break;if(Л.Ў==0){Ў=1;return;}ǫ(Л,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){ǫ(Л,"MRES");break;}ǫ(Л,"DOWN");break;}case "APPLY":{if(Ў<2) break;ǫ(Л,"APPLY");return;}}}}switch(І){case "UP":this.Γσ(false);return;case "DOWN":this.Γς(false);return;
case "APPLY":this.Ŭ(true);return;}switch(І){case "CLEAR":Θτ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȣ)){Λ="Usage: SENDALL <cmd>";return;}ǫ(null,ȣ);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Ιθ(Т.Remove(0,"SEND".Length+1));return;}Λ=Ȭ;}void Ιθ(String ǁ){if(ǁ=="") return;var τ=ǁ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ɲ=τ.ElementAtOrDefault(0).Trim();
Ѕ Δ=Γή("",Ɲ);if(Δ!=null) ǫ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ɲ;}void Θτ(){Ь.Clear();Л=null;μ(Σ.Main);Ў=0;}void Ιε(){List<IMyTerminalBlock>Αε=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Х,Ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αε,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Х=ǩ(Х,pamTag,true);Ζδ(Αε);Ζγ();Ιζ();Ņ(Х,setLCDFontAndSize,1.15f,false);Ņ(Ȕ,setLCDFontAndSize,1.15f,true);String Κί=ΕА(false);String ΗЁ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ч in Х){if(ч.CustomData==""){ч.CustomData=ΕА(true);continue;}
if(!ч.CustomData.Contains(Κί))
if(!ч.CustomData.Contains(ΗЁ)) ч.CustomData=ΗЁ+ч.CustomData;}}void Ιχ(){String ΑΓ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȕ.Count;i++)
Ȕ[i].WriteText(Εψ(С.ΖΦ,"0",false));for(int i=0;i<Х.Count;i++){С Џ=С.ħ;String Ǹ="";ΙΦ(Х[i],out Џ,out Ǹ);Х[i].WriteText(Εψ(Џ,Ǹ,i==0));}Echo(ΑΓ);for(int i=0;i<Ŏ.Count;i++){IMyTextPanel ч=Ŏ[i];String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΕЏ());
}}String Ŭ(bool Ι){int ΚΥ=0;return Ŭ(Ι,0,ref ΚΥ,false,1);}String Ŭ(bool Ι,int Κϊ,ref int ΚЉ,bool ΖЊ,int ΑΫ){String Γ="";String Ɯ="——————————————————\n";String ΛΗ="--------------------------------------------\n";if(Л==null||ό==Σ.Main||ό==Σ.ΓЇ)
Γ+="[PAM]-Controller | "+Ь.Count+" Connected ships"+"\n"+ΘЇ()+ΘЌ()+ΘБ();else Γ+=ΖΒ(Л)+"\n";Γ+=Ɯ;int Β=0;int Η=Ў;Ś=0;if(Λ!=""){Ν(ref Γ,Η,Β++,Ι,Λ);}else if(ό==Σ.Main){if(Л==null&&Ь.Count>=1) Л=Ь[0];String Ѝ="";if(Ν(ref Ѝ,Η,Β++,Ι," Send command to all")) μ(Σ.ΓЇ);
int ƛ=0;for(int s=0;s<Ь.Count;s++){if(ΖЊ) Ѝ+="\n";Ѕ Δ=Ь[s];if(Η==Β||Η==Β+1) Л=Δ;if(Β==Η) ƛ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+ΖΒ(Δ))){Л=Δ;μ(Σ.ƺ);Л.ń("",false,true,false);}if(Β==Η) ƛ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+(ΖЊ?"":"  ")+ΘЎ(Δ)))
{Л=Δ;μ(Σ.Ζε);}}int Ƨ=ΑΫ-2;Ƨ+=Math.Max(0,(Κϊ-1))*ΑΫ;Γ+=ΓЅ(Ƨ,Ѝ,ƛ,ref ΚЉ);}else if(ό==Σ.ΒΑ||ό==Σ.ΓЇ){Σ ΙЎ=ό==Σ.ΒΑ?Σ.ƺ:Σ.Main;Ѕ ζ=null;if(ό==Σ.ΒΑ) ζ=Л;if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(ΙЎ);}if(Ν(ref Γ,Η,Β++,Ι," [Stop] "+ů(ζ,"STOP"))){ǫ(ζ,"STOP");}if(Ν(ref Γ,Η,Β++,Ι," [Continue] "+ů(ζ,"CONT"))){ǫ(ζ,"CONT");}
if(Ν(ref Γ,Η,Β++,Ι," [Move home] "+ů(ζ,"HOMEPOS"))){ǫ(ζ,"HOMEPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Move to job] "+ů(ζ,"JOBPOS"))){ǫ(ζ,"JOBPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Full simulation] "+ů(ζ,"FULL"))){ǫ(ζ,"FULL");}if(ζ!=null&&ζ.Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," [Undock] "+ů(ζ,"UNDOCK"))){ǫ(ζ,"UNDOCK");}Γ+=ΘЄ(ζ);}else if(ό==Σ.ƺ){String ΔΟ="";if(Ν(ref ΔΟ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΔΟ.Substring(0,ΔΟ.Length-1).PadRight(36,' ');if(Ν(ref Γ,Η,Β++,Ι," Send cmd")){μ(Σ.ΒΑ);}if(!Л.ΖΡ()&&!Л.Γμ()) Ś++;
if(Л.Γμ()) Γ+="No answer received...";else Γ+=Ŷ(Л,Л.Βϋ);}else if(ό==Σ.Ζε){if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΛΗ;Γ+=Ŷ(Л,ΖΑ(Л));}if(!ΔА) Ў=Math.Min(Ś-1,Ў);return Γ;}String ΖΒ(Ѕ Δ){if(Δ.ΓΕ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ŷ(Δ,Δ.ΕΫ)+" "+Δ.Δζ+"m";
}String ΘЎ(Ѕ Δ){String s=ΓЂ("",Й(Δ.Ȉ,Δ.ȏ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ћ.Count;i++){if(i>=5) break;м ώ=Δ.Ћ[i];s+=Ζπ(ΓΠ(ΘЋ(ώ)),3)+" "+ΕЉ(ώ.Њ)+" ";}return s;}String ΘЋ(м ώ){if(ώ.υ.ToUpper()=="ORE"||ώ.υ.ToUpper()=="INGOT"){String ΖЈ=ΘЉ(ώ.ΰ.ToUpper());if(ΖЈ!="") return ΖЈ;
}return ώ.ΰ;}String Ŷ(Ѕ Δ,String Γ){if(Δ.ΖΡ()) return "No signal...("+ΑϏ((int)(DateTime.Now-Δ.ΔЀ).TotalSeconds)+")";return Γ;}String ΕА(bool Λή){String Γ="";if(Λή) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΙΦ(IMyTerminalBlock ч,out С Џ,out String Ǹ){bool ȯ=true;String ΑΩ=ΑЎ(Αό(ч.CustomData.Split('\n'),"mode",ref ȯ)).ToUpper();String[]τ=ΑΩ.Split(':');String ΑΨ=τ.Length>0?τ[0].Trim():"";Ǹ=τ.Length>1?τ[1].Trim():"";
Џ=С.ħ;if(ΑΨ=="MAIN") Џ=С.ΖΧ;else if(ΑΨ=="MAINX") Џ=С.ΖΨ;else if(ΑΨ=="MENU") Џ=С.Γρ;else if(ΑΨ=="INVENTORY") Џ=С.Γλ;else if(ΑΨ=="DEBUG") Џ=С.ΓΥ;if(Џ==С.ħ) Ǹ=ΑΩ;}String Εψ(С Џ,String Ǹ,bool ΚΧ){int ΑΥ=15;if(ΚΧ){ΖЌ=ΒΣ;ΒΣ=0;ΖЍ=ΒΤ;ΒΤ=0;}String Γ="";if(Џ==С.ΖΧ)
{int ц=0;Γ=Ŭ(false,ΖЌ,ref ΚΗ,true,ΑΥ);if(!int.TryParse(Ǹ,out ц)) return Γ;ΒΣ=Math.Max(ц,ΒΣ);ц--;return Ζά(ц,ΑΥ,Γ);}else if(Џ==С.ΖΨ){int ц=0;Γ=Ŭ(false,ΖЍ,ref ΚΘ,false,ΑΥ);if(!int.TryParse(Ǹ,out ц)) return Γ;ΒΤ=Math.Max(ц,ΒΤ);ц--;return Ζά(ц,ΑΥ,Γ);}if(Џ==С.ΖΦ){return Ŭ(false,0,ref ΚΙ,true,ΑΥ);
}else if(Џ==С.Γλ||Џ==С.Γρ||Џ==С.ΓΥ){Ѕ Δ=Л;if(Ǹ!=""){Δ=Γή("",Ǹ);if(Δ==null) return "Unknown ship: "+Ǹ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ɯ="—————————————————————\n";String Εδ="";String ΔΥ="";if(Џ==С.Γρ){Εδ="Menu";ΔΥ=Ŷ(Δ,Δ.Βϋ);}
else if(Џ==С.Γλ){Εδ="Inventory";ΔΥ=Ŷ(Δ,ΖΑ(Δ));}else if(Џ==С.ΓΥ){return Ŷ(Δ,ΘЏ(Δ));}Γ+=Δ.ΰ+" - "+Δ.Δζ+"m | "+ΓΠ(""+Εδ)+"\n"+Ɯ;Γ+=ΔΥ;return Γ;}return "Unknown command: "+Ǹ;}string ΘЏ(Ѕ Δ){String s="";s+=Δ.ΓΕ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕΫ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βϋ.Length+"\n";s+=Δ.Ȉ+"\n";s+=Δ.ȏ+"\n";s+=Δ.Ћ.Count()+"\n";return s;}string ΖΑ(Ѕ Δ){String s="";s+=ΓЂ("All",Й(Δ.Ȉ,Δ.ȏ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ћ.Count();i++){м ώ=Δ.Ћ[i];
s+=ΓЂ(ώ.ΰ,ώ.Њ,Δ.Εη,30,8,10)+"\n";}return s;}Љ ΘЅ(Ѕ Δ,bool Ъ,string І){Љ ί=Љ.None;if(Δ==null){for(int i=0;i<Ь.Count;i++){Љ Є=Ь[i].ń(І,false,Ъ,false);if(Є==Љ.Ʒ) ί=Є;if(Є==Љ.ΑЋ) return Є;if(Є==Љ.Ŵ) return Є;}return ί;}else return Δ.ń(І,false,Ъ,false);}String ů(Ѕ Δ,String І)
{if(!Ζζ(Δ,І)) return "";Љ Є=ΘЅ(Δ,false,І);if(Є==Љ.Ʒ) return "received!";if(Є==Љ.Ŵ) return "pending...";if(Є==Љ.ΑЋ) return "no answer!";return "";}String ΓЅ(int Ƨ,String Γ,int ƛ,ref int ę){String[]Ξ=Γ.Split('\n');if(ƛ>=ę+Ƨ-1) ę++;ę=Math.Min(Ξ.Count()-1-Ƨ,ę);if(ƛ<ę+1) ę--;
ę=Math.Max(0,ę);String ί="";for(int i=0;i<Ƨ;i++){int Ηο=i+ę;if(Ηο>=Ξ.Count()) break;ί+=Ξ[Ηο]+"\n";}return ί;}String Ζά(int ц,int ΑΫ,String Γ){String[]Ξ=Γ.Split('\n');int Αβ=ц*ΑΫ;int Č=(ц+1)*ΑΫ;String ί="";for(int i=Αβ;i<Č;i++){if(i>=Ξ.Count()) break;ί+=Ξ[i]+"\n";
}return ί;}String ΓΠ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζπ(String s,int Ηφ){if(s=="") return s;if(s.Length>Ηφ) s=s.Substring(0,Ηφ-1)+".";return s;}string ΓЂ(String ŗ,float value,float ƌ,int Ζύ,int Ηκ,int Κω){float ş=Й(value,ƌ)*Ζύ;
String ΒΟ="[";for(int i=0;i<Ζύ;i++){if(i<=ş) ΒΟ+="|";else ΒΟ+="'";}ΒΟ+="]";return ΒΟ+" "+Ζπ(ΓΠ(ŗ),Ηκ).PadRight(Ηκ)+"".PadRight(Κω)+ΕЉ(value);}String ΕЉ(float Њ){if(Њ>=1000000) return Math.Round(Њ/1000000f,Њ/1000000f<100?1:0)+"M";if(Њ>=1000) return Math.Round(Њ/1000f,Њ/1000f<100?1:0)+"K";
return ""+Math.Round(Њ);}String ΑϏ(int Αί){if(Αί>=60*60) return Math.Round(Αί/(60f*60f),1)+" h";if(Αί>=60) return Math.Round(Αί/60f,1)+" min";return ""+Αί+" s";}string Ů(float v){return Math.Round(v,2)+" ";}string Λε(Vector3 v){return "X"+Ů(v.X)+"Y"+Ů(v.Y)+"Z"+Ů(v.Z);}
List<IMyCameraBlock>ȥ=new List<IMyCameraBlock>();List<long>Ɛ=new List<long>();bool ƒ=false;int ΔΫ=0;int ΔΩ=0;DateTime ΖϏ=DateTime.Now;String ΔΤ="";void Ιδ(){ȥ.Clear();Ɛ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ȥ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȥ.Count;i++){ȥ[i].EnableRaycast=true;if(!Ɛ.Contains(ȥ[i].CubeGrid.EntityId)) Ɛ.Add(ȥ[i].CubeGrid.EntityId);}if(!Ɛ.Contains(Me.CubeGrid.EntityId)) Ɛ.Add(Me.CubeGrid.EntityId);}bool ΖΟ(Α s){return s==Α.Ą||s==Α.у||s==Α.И||
s==Α.Ĥ||s==Α.ŭ||s==Α.Ʈ||s==Α.Αύ||
(s==Α.ǿ&&ǟ!=null);}void Αψ(){ƒ=false;ΔΫ=0;ΔΤ="";}void ΙϏ(){if(!obstacleBrake||ȥ.Count==0||!ΖΟ(Ϊ)){Αψ();return;}Vector3 ΑΗ=Ђ+ĺ;Vector3 ȕ=ΑΗ-δ;float ȓ=ȕ.Length();if(ȓ<1f){Αψ();return;}Vector3 ς=ȕ/ȓ;float Ησ=Ě*obstacleLookAhead+ν;Vector3 ΚЁ=ΑΗ+ς*(ν*0.25f);
for(int n=0;n<ȥ.Count;n++){ΔΩ=(ΔΩ+1)%ȥ.Count;IMyCameraBlock ȅ=ȥ[ΔΩ];if(!ȅ.IsFunctional||!ȅ.IsWorking) continue;Vector3 ΖЁ=ȅ.GetPosition();Vector3 Č=ΚЁ;if(Vector3.Distance(ΖЁ,Č)>Ησ) Č=ΖЁ+ς*Ησ;if(!ȅ.CanScan(Č)) continue;MyDetectedEntityInfo Ȫ=ȅ.Raycast(Č);bool Κυ=!Ȫ.IsEmpty()&&!Ɛ.Contains(Ȫ.EntityId)&&!ΙΞ(Ȫ.EntityId)&&
Ȫ.Type!=MyDetectedEntityType.FloatingObject;if(Κυ){if(!ƒ) ΖϏ=DateTime.Now;ƒ=true;ΔΫ=0;ΔΤ=Ȫ.Type.ToString();}else if(ƒ&&++ΔΫ>=3) Αψ();break;}if(ƒ&&(DateTime.Now-ΖϏ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΤ;}const String ΔΒ="[PAMTFC]";const float Ιρ=5f;const float Γέ=2f;class Ħ{public long Υ;public Vector3 ι,Ǔ;public Vector3 ď;public float ΗΖ;public String Φ="";public int Đ=-1;public bool ΕΛ;public bool Ĳ;public float ΑΙ,ΑΕ;public long α=0;public int ǋ=0;public DateTime Ĺ;
}IMyBroadcastListener ΒБ=null;long ǎ=0;List<Ħ>ѓ=new List<Ħ>();Vector3 ĺ=new Vector3();float Ǒ=-1;Vector3 Εθ=new Vector3();bool ΒА=false;DateTime Ει=DateTime.Now;const float Ζο=15f;const float Ιπ=10f;int ΘΩ=0;String г="";int Д=-1;bool Δί=false,ΒΦ=false;Vector3 ΖЎ=new Vector3();
String Ƴ(){if(γ==Π.э||Μ==Κ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ќ+"|"+Ю+"|"+(int) Ŗ+"|"+Math.Round(ǽ,1)+"|"+Math.Round(ȗ,1);}void ΙЃ(){if(!trafficEnabled) return;if(ΒБ==null) ΒБ=IGC.RegisterBroadcastListener(ΔΒ);ǎ=Me.CubeGrid.EntityId;int Ȅ=30;while(ΒБ.HasPendingMessage&&Ȅ-->0){String Θ=ΒБ.AcceptMessage().Data as String;if(Θ!=null) ΙΨ(Θ);
}for(int i=ѓ.Count-1;i>=0;i--)
if((DateTime.Now-ѓ[i].Ĺ).TotalSeconds>Ιρ) ѓ.RemoveAt(i);if(ΒЍ==null) ΒЍ=IGC.RegisterBroadcastListener(Ζν);Ȅ=5;while(ΒЍ.HasPendingMessage&&Ȅ-->0){String Θ=ΒЍ.AcceptMessage().Data as String;if(Θ!=null) try{ΙΩ(Θ);}catch{}}if(ΒΧ==null) ΒΧ=IGC.RegisterBroadcastListener(ΖΣ);
Ȅ=10;while(ΒΧ.HasPendingMessage&&Ȅ-->0){String Θ=ΒΧ.AcceptMessage().Data as String;if(Θ!=null) try{ΙΥ(Θ);}catch{}}if(++ΘΩ<3) return;ΘΩ=0;IGC.SendBroadcastMessage(ΔΒ,ǎ+";"+σ(δ)+";"+
σ(Ϋ.GetShipVelocities().LinearVelocity)+";"+ν+";"+Ƴ()+";"+
(γ==Π.В?Д:-1)+";"+г+";"+Θσ()+","+ΕІ()+";"+σ(Εθ)+
";"+(Βί?1:0)+";"+(ı&&γ==Π.В?1:0)+","+ΒЅ+","+ΕΤ+","+(ΖΘ()?1:0));}void ΙΨ(String Θ){try{String[]f=Θ.Split(';');if(f.Length<7) return;long Υ=long.Parse(f[0]);if(Υ==ǎ) return;Ħ Δ=null;for(int i=0;i<ѓ.Count;i++) if(ѓ[i].Υ==Υ) Δ=ѓ[i];if(Δ==null){Δ=new Ħ();Δ.Υ=Υ;ѓ.Add(Δ);}
Δ.ι=π(f[1]);Δ.Ǔ=π(f[2]);Δ.ΗΖ=float.Parse(f[3]);Δ.Φ=f[4];Δ.Đ=int.Parse(f[5]);Δ.Ĺ=DateTime.Now;if(Δ.Φ!=""&&Δ.Φ==Ƴ()) ΖΩ(f[6]);Δ.α=0;Δ.ǋ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ǋ=int.Parse(d[1]);}Δ.ď=f.Length>=9?π(f[8]):new Vector3();Δ.ΕΛ=f.Length>=10&&f[9]=="1";
Δ.Ĳ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ΑΙ=float.Parse(w[1]);Δ.ΑΕ=float.Parse(w[2]);Δ.Ĳ=w[3]=="1";}}}catch{}}bool ΙΞ(long Υ){for(int i=0;i<ѓ.Count;i++) if(ѓ[i].Υ==Υ) return true;return false;}Vector3 ΕБ(int Β,int Κϋ){if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();
ά έ=Ψ[Β];Vector3 ΛΥ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Κϋ;Vector3 Б=έ.φ.LengthSquared()>0.01f?-Vector3.Normalize(έ.φ):-έ.η;Vector3 р=Vector3.Cross(ΛΥ,Б);if(р.LengthSquared()<0.0001f) return new Vector3();float ϊ=laneOffset*ν;float ΚΩ=Math.Min(Vector3.Distance(έ.Ε,Ψ[0].Ε),
Vector3.Distance(έ.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(р)*ϊ*Math.Min(1,ΚΩ/(ϊ*4));}Vector3 ΘΨ=new Vector3();bool ΖΘ(){return γ==Π.В&&(Ϊ==Α.Ą||Ϊ==Α.Ų||Ϊ==Α.Ȟ);}bool ΙΝ(Α s){return s==Α.ǿ||s==Α.Ľ||s==Α.Ł||s==Α.Ƹ||
s==Α.ǭ||s==Α.Ȟ||s==Α.Ġ;}void ΙЄ(){ĺ=new Vector3();Ǒ=-1;Εθ=new Vector3();bool ΑΘ=ΖΟ(Ϊ);if(!trafficEnabled||(!ΑΘ&&!ΙΝ(Ϊ))){ΒА=false;return;}ΘΨ=Ϋ.GetShipVelocities().LinearVelocity;Θυ(Ϋ.GetNaturalGravity(),!ΑΘ);Θγ();}IMyBroadcastListener ΒЍ=null;float ơ=-1;DateTime ΘΤ=DateTime.MinValue;
void ΙΩ(String Θ){if(!Θ.StartsWith("C;")) return;ΘΤ=DateTime.Now;ơ=-1;String ΑΧ=ǎ+":";String[]e=Θ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΧ)) ơ=float.Parse(e[i].Substring(ΑΧ.Length));}void Θγ(){if(ơ<0||(DateTime.Now-ΘΤ).TotalSeconds>1.5) return;
Ǒ=Ǒ<0?ơ:Math.Min(Ǒ,ơ);if(ơ<0.5f) Λ="Traffic: giving way (controller)";}void Θυ(Vector3 φ,bool ΗІ=false){ĺ=new Vector3();Ǒ=-1;Vector3 ȕ=Ђ-δ;float ȓ=ȕ.Length();bool Δψ=ȓ>=1f;Vector3 ς=Δψ?ȕ/ȓ:new Vector3();Εθ=ς;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ΑΑ;
Vector3 р=Vector3.Cross(ς,Б);р=р.LengthSquared()<0.0001f?-ΑΤ:Vector3.Normalize(р);Ħ Βρ=null;bool ΗΧ=Ϊ==Α.И&&!ĉ(1);for(int i=0;i<ѓ.Count;i++){Ħ λ=ѓ[i];if(λ.ǋ==3&&λ.Ǔ.LengthSquared()<0.25f) continue;Vector3 ι=λ.ι+λ.Ǔ*(float)(DateTime.Now-λ.Ĺ).TotalSeconds;Vector3 ǡ=ι-δ;
float Ъ=(ν+λ.ΗΖ)*0.5f;bool ΗЏ=λ.Ĳ&&ΖΘ()&&λ.Φ==Ƴ();if(ΗЏ) Ъ=Math.Min(ǽ,ȗ)*0.9f;float ǀ=Δψ?Vector3.Dot(ǡ,ς):0;if(ǀ>0&&(ǡ-ς*ǀ).Length()<=Ъ*1.2f){float ΒϏ=Vector3.Dot(λ.Ǔ,ς);bool ΗΫ=λ.ď.LengthSquared()>0.25f;bool Ηά=ΒϏ<-1f||(ΗΫ&&Vector3.Dot(λ.ď,ς)<-0.5f);
if(!ΗІ){if(λ.ΕΛ){if(!ΗΧ){ĺ=р*Ъ*1.3f;continue;}float Ηϋ=Vector3.Distance(δ,Ђ),Ηη=Vector3.Distance(ι,Ђ);if(Ηϋ<Ηη||(Ηϋ==Ηη&&ǎ<λ.Υ)) continue;}if(Ηά){if(ǀ<(Ě-ΒϏ)*6f+Ъ*4f) ĺ=р*Ъ*1.3f;continue;}if(ΗΫ&&ǎ<λ.Υ){Vector3 Ζϋ=-ǡ;float ΗЄ=Vector3.Dot(Ζϋ,λ.ď);
if(ΗЄ>0&&(Ζϋ-λ.ď*ΗЄ).Length()<=Ъ*1.2f){ĺ=р*Ъ*1.3f;continue;}}}float ΗΦ=ǀ-Ъ-(ΗЏ?1f:followGap*ν);float ȍ=Math.Max(0,ΒϏ)+Math.Max(0,ΗΦ)/Γέ;if(Ηά) ȍ=Math.Max(0,ΗΦ+Math.Min(0,ΒϏ)*Γέ*2)/Γέ;if(Ǒ<0||ȍ<Ǒ){Ǒ=ȍ;Βρ=λ;}continue;}if(λ.Ǔ.LengthSquared()<1f) continue;
if(Δψ&&(ǡ-ς*ǀ).Length()<=Ъ*1.2f) continue;Vector3 ΕΡ=λ.Ǔ-ΘΨ;float ΗЋ=ΕΡ.LengthSquared();if(ΗЋ<0.25f) continue;float t=-Vector3.Dot(ǡ,ΕΡ)/ΗЋ;if(t<=0||t>5f||(ǡ+ΕΡ*t).Length()>=Ъ*1.1f) continue;if(ǎ>λ.Υ||t<2f){Ǒ=0;Βρ=λ;}}bool ΑΝ=Ǒ>=0&&Ǒ<0.5f;
if(!ΑΝ||ΗІ||(Βρ!=null&&Βρ.ΕΛ&&ΗΧ)){ΒА=false;return;}if(!ΒА){ΒА=true;Ει=DateTime.Now;}double Ηέ=(DateTime.Now-Ει).TotalSeconds;if(Ηέ>Ζο+Ιπ) Ει=DateTime.Now;else if(Ηέ>Ζο){Ǒ=-1;ĺ=р*ν*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȫ=new List<int>();
String Ηί="";void Γά(){String ȭ=Ќ+"|"+Ю+"|"+(int) Ŗ;if(ȭ==Ηί) return;Ηί=ȭ;Εω(Ќ,Ю,Ŗ,ȫ);}void Εω(int w,int h,ю Αβ,List<int>ΔΪ){ΔΪ.Clear();if(Αβ==ю.Ƽ){for(int k=0;k<w*h;k++){int Ɨ=k/w;ΔΪ.Add(Ɨ*w+(Ɨ%2==0?k-Ɨ*w:w-1-(k-Ɨ*w)));}return;}int ΑΛ=(int) Math.Ceiling(w/2f),ΑΚ=(int) Math.Ceiling(h/2f);
int Δυ=w/2,Δτ=h/2;int x=0,y=0,Δι=0,Δκ=-1,ƌ=Math.Max(w,h);for(int i=0;i<ƌ*ƌ;i++){if(-ΑΛ<x&&x<=Δυ&&-ΑΚ<y&&y<=Δτ) ΔΪ.Add((y-1+ΑΚ)*w+(x-1+ΑΛ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Δι;Δι=-Δκ;Δκ=t;}x+=Δι;y+=Δκ;
}}bool ΓΫ(){int n=Math.Max(0,Ќ*Ю);if(г.Length==n) return false;г=new String('0',n);Д=-1;return true;}void ΖΩ(String λ){ΓΫ();if(λ.Length!=г.Length) return;char[]h=г.ToCharArray();for(int i=0;i<h.Length;i++) if(λ[i]=='1') h[i]='1';г=new String(h);}bool ΑЄ(int Щ){return Щ>=0&&Щ<г.Length&&г[Щ]=='1';}
void ΓЉ(int Щ,bool Ϗ){if(Щ<0||Щ>=г.Length) return;char[]h=г.ToCharArray();h[Щ]=Ϗ?'1':'0';г=new String(h);}bool Γθ(int Щ,bool Κζ){if(!trafficEnabled) return false;String ȭ=Ƴ();for(int i=0;i<ѓ.Count;i++){Ħ t=ѓ[i];if(t.Φ!=ȭ||t.Đ<0||(Κζ&&t.Υ>ǎ)) continue;if(t.Đ==Щ) return true;
}return false;}int Θς(int Β){int n=Ќ*Ю;if(n<1) return Β;Γά();if(ΓΫ()) for(int k=0;k<Β&&k<n;k++) ΓЉ(ȫ[k],true);if(Д>=0&&(Β>=n||ȫ[Β]!=Д)){ΓЉ(Д,true);Д=-1;}if(ΖΤ()){if(ȇ>=0&&!ΑЄ(ȇ)){ı=false;Д=ȇ;return ȫ.IndexOf(ȇ);}if(ȇ==-2){Д=-1;ı=г.IndexOf('0')>=0;return ı?-2:n;}
if(ȇ==-1&&Д>=0&&!ΑЄ(Д)){ı=false;return Β;}Д=-1;if(!ı){ı=true;ΖА=DateTime.Now;}if((DateTime.Now-ΖА).TotalSeconds<5) return-1;}ı=false;if(Д>=0&&!ΑЄ(Д)&&!Γθ(Д,true)) return Β;Д=-1;for(int k=0;k<n;k++){int c=ȫ[k];if(ΑЄ(c)||Γθ(c,false)) continue;Д=c;
return k;}return г.IndexOf('0')>=0?-2:n;}const String ΖΣ="[PAMJOB]";const String Ζν="[PAMTCC]";IMyBroadcastListener ΒΧ=null;DateTime ΖЏ=DateTime.MinValue;DateTime ΖА=DateTime.Now;int ȇ=-1;bool ı=false;bool ΖΤ(){return trafficEnabled&&ȇ!=-3&&(DateTime.Now-ΖЏ).TotalSeconds<3;}
void ΙΥ(String Θ){String[]f=Θ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ƴ()) return;ΖЏ=DateTime.Now;ΖΩ(f[2]);ȇ=-3;String ΑΧ=ǎ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΧ)) ȇ=int.Parse(e[i].Substring(ΑΧ.Length));}Ǭ Ιι(int Β)
{Γά();if(Β<0||Β>=ȫ.Count) return Ǭ.Ας;Ѐ=ȫ[Β]%Ќ;ύ=ȫ[Β]/Ќ;return Ǭ.ΑЀ;}class ǖ{public long Υ;public Vector3 ι,Э,Б;}List<ǖ>У=new List<ǖ>();bool Ĭ=false;Vector3 Βα,Βΰ,Ββ,Βγ;long ќ=0;long Ȱ=0;ά ǟ=null;bool ǌ=false;ά ΓΨ(Vector3 ι,Vector3 Э,Vector3 Б){MatrixD m=MatrixD.CreateWorld(ι,Э,Б);
ά w=new ά(Ο);w.θ=Vector3D.TransformNormal(Βα,m);w.η=Vector3D.TransformNormal(Βΰ,m);w.Ё=Vector3D.TransformNormal(Ββ,m);w.Ε=ι+(Vector3) Vector3D.TransformNormal(Βγ,m);w.κ=true;return w;}int ΕІ(){Α s=Ϊ;if(s==Α.ь) return з(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ł||s==Α.Ȣ||s==Α.ŀ||s==Α.ΒΙ||
s==Α.Αϋ||s==Α.ǭ) return 3;if(s==Α.ǿ||s==Α.Ľ||s==Α.Ƹ) return 2;if(s==Α.И) return 1;return 0;}long Θσ(){if(ΕІ()==0) return 0;if(ĉ(1)&&ƞ!=0) return ƞ;return ǟ!=null?Ȱ:ќ;}void ΖΥ(){if(Μ==Κ.ή||!Ο.κ) return;IMyShipConnector О=з(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector λ=О.OtherConnector;Vector3I Κΰ=new Vector3I((int) Ο.о.X,(int) Ο.о.Y,(int) Ο.о.Z);if(О.Position==Κΰ){MatrixD Ɔ=MatrixD.Transpose(λ.WorldMatrix);Βα=Vector3D.TransformNormal(Ϋ.WorldMatrix.Forward,Ɔ);Βΰ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Down,Ɔ);Ββ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Left,Ɔ);
Βγ=Vector3D.TransformNormal(О.GetPosition()-λ.GetPosition(),Ɔ);Ĭ=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ќ=λ.EntityId;List<IMyShipConnector>ő=new List<IMyShipConnector>();χ.GetBlocksOfType(ő,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ő.Count>0){У.Clear();for(int i=0;i<ő.Count;i++){ǖ p=new ǖ();p.Υ=ő[i].EntityId;p.ι=ő[i].GetPosition();p.Э=ő[i].WorldMatrix.Forward;p.Б=ő[i].WorldMatrix.Up;У.Add(p);}}ǟ=null;Ȱ=0;if(λ.EntityId==ќ||!Ĭ) return;for(int i=0;i<У.Count;i++)
if(У[i].Υ==λ.EntityId){Ȱ=У[i].Υ;ǟ=ΓΨ(У[i].ι,У[i].Э,У[i].Б);}}bool Γξ(long Υ,int Ηώ){if(Υ==0) return false;for(int i=0;i<ѓ.Count;i++){Ħ t=ѓ[i];if(t.α==Υ&&(t.ǋ>Ηώ||(t.ǋ==Ηώ&&t.Υ<ǎ))) return true;}return false;}bool ΓΣ(int Ɠ){if(Μ==Κ.ή||!trafficEnabled){ǟ=null;Ȱ=0;return true;}
long ο=ǟ!=null?Ȱ:ќ;if(!Γξ(ο,Ɠ)) return true;if(!Γξ(ќ,Ɠ)){ǟ=null;Ȱ=0;return true;}if(!Ĭ) return false;ǖ Ǯ=null;float ǅ=0;for(int i=0;i<У.Count;i++){ǖ p=У[i];if(p.Υ==ќ||Γξ(p.Υ,Ɠ)) continue;float d=Vector3.Distance(p.ι,Ο.Ε);if(Ǯ==null||d<ǅ){Ǯ=p;ǅ=d;}
}if(Ǯ==null) return false;Ȱ=Ǯ.Υ;ǟ=ΓΨ(Ǯ.ι,Ǯ.Э,Ǯ.Б);return true;}String Θώ(){String s=(Ĭ?"1":"0")+":"+σ(Βα)+":"+σ(Βΰ)+":"+
σ(Ββ)+":"+σ(Βγ)+":"+ќ;for(int i=0;i<У.Count;i++)
s+=":"+У[i].Υ+","+σ(У[i].ι)+","+σ(У[i].Э)+","+σ(У[i].Б);return s;}void Θύ(String s){У.Clear();Ĭ=false;ќ=0;String[]f=s.Split(':');if(f.Length<6) return;Ĭ=f[0]=="1";Βα=π(f[1]);Βΰ=π(f[2]);Ββ=π(f[3]);Βγ=π(f[4]);long.TryParse(f[5],out ќ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǖ p=new ǖ();if(!long.TryParse(e[0],out p.Υ)) continue;p.ι=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);У.Add(p);}}const String ƭ="[PAMDCK]";const float Θκ=120f;class ă{public long Δ;public int ǜ;public long ћ;public bool ΖЂ;public DateTime ǚ,ΑΣ;
}IMyBroadcastListener Βή=null;List<ă>Ф=new List<ă>();List<IMyShipConnector>ě=new List<IMyShipConnector>();List<IMySensorBlock>ā=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΡ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΓΖ=new List<MyDetectedEntityInfo>();long ǘ=0;int Δά=0;
long ΖЄ=0;DateTime ΖЅ=DateTime.Now;String ŏ="";int ΗΛ=0;IMyCubeGrid Ĝ=null;void Ιζ(){Ĝ=Me.CubeGrid;χ.GetBlocksOfType(ě,q=>q.CubeGrid.IsSameConstructAs(Ĝ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ā,q=>q.CubeGrid.IsSameConstructAs(Ĝ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΡ,q=>q.CubeGrid.IsSameConstructAs(Ĝ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ā.Count;i++){ā[i].DetectSmallShips=true;ā[i].DetectLargeShips=true;}for(int i=0;i<ΑΡ.Count;i++) ΑΡ[i].EnableRaycast=true;}IMyShipConnector Ζθ(long Υ){if(Υ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Υ) as IMyShipConnector;return c!=null&&Ĝ!=null&&c.CubeGrid.IsSameConstructAs(Ĝ)?c:null;}bool Γν(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΠ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΡ.Count;i++)
{IMyCameraBlock ȅ=ΑΡ[i];if(!ȅ.IsWorking||Vector3D.Distance(ȅ.GetPosition(),c.GetPosition())>10) continue;Vector3D Č=ȅ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ȅ.CanScan(Č)) continue;MyDetectedEntityInfo Ȫ=ȅ.Raycast(Č);if(!Ȫ.IsEmpty()&&Ȫ.EntityId!=Ĝ.EntityId&&Ȫ.EntityId!=Δ&&Ȫ.Type!=MyDetectedEntityType.FloatingObject)
{ŏ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙΟ(){for(int s=0;s<ā.Count;s++){if(!ā[s].IsWorking) continue;ΓΖ.Clear();ā[s].DetectedEntities(ΓΖ);for(int i=0;i<ΓΖ.Count;i++){MyDetectedEntityInfo e=ΓΖ[i];bool ΚΫ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΚΫ&&e.EntityId!=ǘ&&e.Velocity.Length()>0.5f){ŏ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΙΪ(ă r){IMyShipConnector ћ=Ζθ(r.ћ);if(Γν(ћ)&&ΖΠ(ћ,r.Δ)) return ћ.EntityId;if(!r.ΖЂ||ћ==null) return 0;IMyShipConnector Ǯ=null;double ǅ=0;for(int i=0;i<ě.Count;i++)
{IMyShipConnector c=ě[i];if(!Γν(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ћ.GetPosition());if(Ǯ==null||d<ǅ){if(ΖΠ(c,r.Δ)){Ǯ=c;ǅ=d;}}}return Ǯ==null?0:Ǯ.EntityId;}void ΙΑ(){ă Ɣ=null;long О=0;for(int ŝ=2;ŝ>=1&&Ɣ==null;ŝ--){ă Ŝ=null;for(int i=0;i<Ф.Count;i++)
{ă r=Ф[i];if(r.ǜ!=ŝ) continue;if(ŝ==1){long c=ΙΪ(r);if(c==0) continue;if(Ŝ==null||r.ǚ<Ŝ.ǚ){Ŝ=r;О=c;}}else if(Ŝ==null||r.ǚ<Ŝ.ǚ) Ŝ=r;}Ɣ=Ŝ;}if(Ɣ==null){if(Ф.Count>0&&ŏ=="") ŏ="No free dock";return;}ǘ=Ɣ.Δ;Δά=Ɣ.ǜ;ΖЄ=Ɣ.ǜ==1?О:0;ΖЅ=DateTime.Now;
}void Ιω(){if(Ĝ==null) return;if(Βή==null) Βή=IGC.RegisterBroadcastListener(ƭ);int Ȅ=30;while(Βή.HasPendingMessage&&Ȅ-->0){String Θ=Βή.AcceptMessage().Data as String;if(Θ!=null) ΙΒ(Θ);}Ιί();if(++ΗΛ<3) return;ΗΛ=0;IGC.SendBroadcastMessage(ƭ,Θθ());}void ΙΒ(String Θ){try{String[]f=Θ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ф.Count-1;i>=0;i--) if(Ф[i].Δ==Δ) Ф.RemoveAt(i);if(ǘ==Δ) ǘ=0;}else if(f[0]=="R"&&f.Length>=5){long ћ=long.Parse(f[3]);if(Ζθ(ћ)==null) return;ă r=null;for(int i=0;i<Ф.Count;i++) if(Ф[i].Δ==Δ) r=Ф[i];if(r==null){r=new ă();r.Δ=Δ;r.ǚ=DateTime.Now;Ф.Add(r);}
int ǜ=int.Parse(f[2]);if(r.ǜ!=ǜ) r.ǚ=DateTime.Now;r.ǜ=ǜ;r.ћ=ћ;r.ΖЂ=f[4]=="1";r.ΑΣ=DateTime.Now;}}catch{}}void Ιί(){for(int i=Ф.Count-1;i>=0;i--)
if((DateTime.Now-Ф[i].ΑΣ).TotalSeconds>3) Ф.RemoveAt(i);ă ΒΡ=null;for(int i=0;i<Ф.Count;i++) if(Ф[i].Δ==ǘ) ΒΡ=Ф[i];if(ǘ!=0&&ΒΡ==null) ǘ=0;if(ΒΡ!=null&&(DateTime.Now-ΖЅ).TotalSeconds>Θκ){ǘ=0;ΒΡ.ǚ=DateTime.Now;}ŏ="";if(ǘ==0&&Ф.Count>0&&!ΙΟ()) ΙΑ();
}String Θθ(){String β="S;"+Ĝ.EntityId+";"+ǘ+";"+Δά+";"+ΖЄ+";";for(int i=0;i<Ф.Count;i++) β+=(i>0?",":"")+Ф[i].Δ;β+=";";for(int i=0;i<ě.Count;i++){IMyShipConnector c=ě[i];β+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}β+=";";for(int i=0;i<ā.Count;i++){IMySensorBlock z=ā[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();β+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return β;}String ΘЇ()
{if(ě.Count==0&&ā.Count==0) return "";int ΑΘ=0;for(int i=0;i<ě.Count;i++) if(Γν(ě[i])) ΑΘ++;return "Docks: "+ΑΘ+"/"+ě.Count+" free"+(ǘ!=0?(Δά==1?" | docking":" | undocking"):"")+
(Ф.Count>1?" | "+(Ф.Count-1)+" waiting":"")+(ŏ!=""?"\n"+ŏ:"")+"\n";}IMyBroadcastListener ΒЋ=null;int ȩ=0;DateTime ΗΚ=DateTime.Now;DateTime ΘΒ=DateTime.MinValue;long ΘΓ=0;int ΘΔ=0;long ƞ=0;bool Αξ=false;int Δθ=0;List<Vector3>Απ=new List<Vector3>();List<float>Ευ=new List<float>();
bool Βί=false;bool Βέ=false;Vector3 ΗΙ=new Vector3();bool ƻ(){return ȩ!=0&&(DateTime.Now-ΘΒ).TotalSeconds<3;}bool ĉ(int ǜ){return ƻ()&&ΘΓ==ǎ&&ΘΔ==ǜ;}bool Ζι(){return ƻ()||(DateTime.Now-ΗΚ).TotalSeconds<3;}void ΓЄ(int ǜ){if(ȩ==ǜ) return;ȩ=ǜ;ΗΚ=DateTime.Now;Δθ=3;
}void ΓЃ(){if(ȩ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƭ,"D;"+ǎ);ȩ=0;Αξ=false;}void Ιψ(){if(!trafficEnabled||Μ==Κ.ή) return;if(ΒЋ==null) ΒЋ=IGC.RegisterBroadcastListener(ƭ);int Ȅ=20;while(ΒЋ.HasPendingMessage&&Ȅ-->0){String Θ=ΒЋ.AcceptMessage().Data as String;if(Θ==null||!Θ.StartsWith("S;")) continue;
try{ΙΧ(Θ);}catch{}}if(Ϊ!=Α.И){Βί=false;Βέ=false;}if(ȩ==1&&з(MyShipConnectorStatus.Connected)!=null) ΓЃ();if(ȩ==2&&!Αξ&&Ϊ!=Α.ǭ&&
Vector3.Distance(δ,ы().Ε)>ν*3) ΓЃ();if(ȩ!=0&&++Δθ>=3){Δθ=0;IGC.SendBroadcastMessage(ƭ,"R;"+ǎ+";"+ȩ+";"+ќ+";"+(Ĭ?1:0));}}void ΙΧ(String Θ){String[]f=Θ.Split(';');if(f.Length<7) return;bool ΗΥ=false;String[]Ζς=f[5].Split(',');for(int i=0;i<Ζς.Length;i++) if(Ζς[i]==ǎ.ToString()) ΗΥ=true;
if(!ΗΥ) return;ΘΒ=DateTime.Now;ΘΓ=long.Parse(f[2]);ΘΔ=int.Parse(f[3]);ƞ=long.Parse(f[4]);Απ.Clear();Ευ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Απ.Add(π(e[0]));Ευ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗΝ=f[6].Split(':');List<ǖ>Ѝ=new List<ǖ>();for(int i=0;i<ΗΝ.Length;i++){String[]e=ΗΝ[i].Split(',');if(e.Length<4) continue;ǖ p=new ǖ();p.Υ=long.Parse(e[0]);p.ι=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);Ѝ.Add(p);}У=Ѝ;}bool Θπ(){if(!trafficEnabled||Μ==Κ.ή||ќ==0) return true;
ΓЄ(1);if(!Ζι()) return true;if(!ĉ(1)){Λ="Waiting for dock clearance";return false;}ΓΚ();return true;}bool ΓΚ(){ǌ=false;ǖ Ş=null;if(ƞ!=0&&ƞ!=ќ&&Ĭ)
for(int i=0;i<У.Count;i++) if(У[i].Υ==ƞ) Ş=У[i];long ƨ=Ş==null?0:Ş.Υ;if(ƨ==Ȱ&&(ƨ==0)==(ǟ==null)) return false;Ȱ=ƨ;ǟ=Ş==null?null:ΓΨ(Ş.ι,Ş.Э,Ş.Б);return true;}bool ű(Vector3 p){for(int i=0;i<Απ.Count;i++) if(Vector3.Distance(p,Απ[i])<Ευ[i]+ν*0.75f) return true;
return false;}bool ΙΖ(Vector3 ǉ,Vector3 ś){Βί=false;if(!trafficEnabled||Μ==Κ.ή||ќ==0) return false;if(ȩ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;ΓЄ(1);if(!ƻ()||ĉ(1)||Απ.Count==0) return false;if(!ű(ǉ)&&!ű(ś)&&!ű(δ)) return false;Βί=true;return true;
}bool ΕЀ(){if(!trafficEnabled||Μ==Κ.ή||ќ==0) return true;ΓЄ(2);if(!Ζι()) return true;if(!ĉ(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΙΠ=600f;class Ć{public long Υ;public int Щ=-1;public bool ƨ;public float ΑΙ,ΑΕ;public DateTime ΕΣ;}class ȝ{public String ȭ;
public int w,h;public float ΛΦ,ΛΖ;public String Ϗ;public List<int>ΕΔ=new List<int>();public List<Ć>с=new List<Ć>();}List<ȝ>Ȍ=new List<ȝ>();IMyBroadcastListener Βπ=null;int Ηθ=0;void Ιύ(){if(Βπ==null) Βπ=IGC.RegisterBroadcastListener(ΔΒ);int Ȅ=50;while(Βπ.HasPendingMessage&&Ȅ-->0)
{String Θ=Βπ.AcceptMessage().Data as String;if(Θ==null) continue;try{ΙΔ(Θ);}catch{}try{ΙΕ(Θ);}catch{}}for(int j=Ȍ.Count-1;j>=0;j--){ȝ Φ=Ȍ[j];for(int i=Φ.с.Count-1;i>=0;i--)
if((DateTime.Now-Φ.с[i].ΕΣ).TotalSeconds>ΙΠ) Φ.с.RemoveAt(i);if(Φ.с.Count==0){Ȍ.RemoveAt(j);continue;}Θδ(Φ);}if(++Ηθ<3) return;Ηθ=0;for(int j=0;j<Ȍ.Count;j++) IGC.SendBroadcastMessage(ΖΣ,Θη(Ȍ[j]));}void Θδ(ȝ Φ){for(int i=0;i<Φ.с.Count;i++){Ć s=Φ.с[i];if(s.ƨ&&(s.Щ<0||Φ.Ϗ[s.Щ]=='1')) s.Щ=ΙΣ(Φ,s);
}}String Θη(ȝ Φ){String β="J;"+Φ.ȭ+";"+Φ.Ϗ+";";bool ǚ=true;for(int i=0;i<Φ.с.Count;i++){if((DateTime.Now-Φ.с[i].ΕΣ).TotalSeconds>5) continue;β+=(ǚ?"":",")+Φ.с[i].Υ+":"+Φ.с[i].Щ;ǚ=false;}return β;}void ΙΔ(String Θ){String[]f=Θ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȝ Φ=null;for(int i=0;i<Ȍ.Count;i++) if(Ȍ[i].ȭ==f[4]) Φ=Ȍ[i];if(Φ==null){Φ=new ȝ();Φ.ȭ=f[4];Φ.w=int.Parse(k[3]);Φ.h=int.Parse(k[4]);Φ.ΛΦ=float.Parse(k[6]);Φ.ΛΖ=float.Parse(k[7]);if(Φ.w<1||Φ.h<1) return;Εω(Φ.w,Φ.h,(ю) int.Parse(k[5]),Φ.ΕΔ);
Φ.Ϗ=new String('0',Φ.w*Φ.h);Ȍ.Add(Φ);}String d=f[6];if(d.Length==Φ.Ϗ.Length){char[]c=Φ.Ϗ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Φ.Ϗ=new String(c);}long Υ=long.Parse(f[0]);Ć s=null;for(int i=0;i<Φ.с.Count;i++) if(Φ.с[i].Υ==Υ) s=Φ.с[i];
if(s==null){s=new Ć();s.Υ=Υ;Φ.с.Add(s);}s.ΕΣ=DateTime.Now;String[]w=f[10].Split(',');s.ƨ=w[0]=="1";if(w.Length>=3){s.ΑΙ=float.Parse(w[1]);s.ΑΕ=float.Parse(w[2]);}int Đ=int.Parse(f[5]);if(Đ>=0&&Đ<Φ.Ϗ.Length&&(s.Щ<0||Φ.Ϗ[s.Щ]=='1')&&Φ.Ϗ[Đ]!='1'&&ΖΗ(Φ,Đ,s)==null)
s.Щ=Đ;}Ć ΖΗ(ȝ Φ,int Щ,Ć ΚΦ){for(int i=0;i<Φ.с.Count;i++){Ć o=Φ.с[i];if(o!=ΚΦ&&o.Щ==Щ)
return o;}return null;}int ΙΣ(ȝ Φ,Ć s){for(int k=0;k<Φ.ΕΔ.Count;k++){int c=Φ.ΕΔ[k];if(Φ.Ϗ[c]!='1'&&ΖΗ(Φ,c,s)==null) return c;}return-2;}String ΘЌ(){String s="";for(int j=0;j<Ȍ.Count;j++){ȝ Φ=Ȍ[j];int Ϗ=0;for(int i=0;i<Φ.Ϗ.Length;i++) if(Φ.Ϗ[i]=='1') Ϗ++;s+="Job "+(j+1)+": "+Ϗ+"/"+Φ.Ϗ.Length+" holes, "+Φ.с.Count+" ships\n";
}return s;}const float Ιξ=8f;class ȡ{public long Υ;public Vector3 ι,Ǔ,ď;public float Δδ,ΑΙ,ΑΕ,ȍ=-1;public bool Ĳ;public String Φ="";public int Ɠ;public DateTime Ĺ;}class ΒΕ{public long a,b,Θβ;public DateTime ΑΣ;}List<ȡ>Ч=new List<ȡ>();List<ΒΕ>ȱ=new List<ΒΕ>();int ΘΣ=0;void ΙΕ(String Θ)
{String[]f=Θ.Split(';');if(f.Length<9) return;long Υ=long.Parse(f[0]);ȡ s=null;for(int i=0;i<Ч.Count;i++) if(Ч[i].Υ==Υ) s=Ч[i];if(s==null){s=new ȡ();s.Υ=Υ;Ч.Add(s);}s.ι=π(f[1]);s.Ǔ=π(f[2]);s.Δδ=float.Parse(f[3]);s.Φ=f[4];s.Ɠ=int.Parse(f[7].Split(',')[1]);s.ď=π(f[8]);
s.Ĳ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΙ=float.Parse(w[1]);s.ΑΕ=float.Parse(w[2]);s.Ĳ=w[3]=="1";}}s.Ĺ=DateTime.Now;}bool ΔΑ(ȡ s){return s.Ǔ.LengthSquared()<0.25f&&(s.Ɠ==3||s.ď.LengthSquared()<0.25f);}Vector3 Ζξ(ȡ s){return s.Ǔ.LengthSquared()>=1f?s.Ǔ:s.ď*2f;}
void ΙЅ(){Θφ();if(++ΘΣ<3) return;ΘΣ=0;IGC.SendBroadcastMessage(Ζν,Θι());}void Θφ(){for(int i=Ч.Count-1;i>=0;i--)
if((DateTime.Now-Ч[i].Ĺ).TotalSeconds>3) Ч.RemoveAt(i);for(int i=ȱ.Count-1;i>=0;i--)
if((DateTime.Now-ȱ[i].ΑΣ).TotalSeconds>2) ȱ.RemoveAt(i);for(int i=0;i<Ч.Count;i++) Ч[i].ȍ=-1;for(int i=0;i<Ч.Count;i++)
for(int j=i+1;j<Ч.Count;j++){ȡ a=Ч[i],b=Ч[j];if(ΔΑ(a)||ΔΑ(b)) continue;Vector3 Κψ=a.ι+a.Ǔ*(float)(DateTime.Now-a.Ĺ).TotalSeconds;Vector3 Κό=b.ι+b.Ǔ*(float)(DateTime.Now-b.Ĺ).TotalSeconds;Vector3 ƣ=Ζξ(a),Ƥ=Ζξ(b);Vector3 ǡ=Κό-Κψ;float ϊ=ǡ.Length();float Ъ=(a.Δδ+b.Δδ)*0.55f;
if(a.Ĳ&&b.Ĳ&&a.Φ!=""&&a.Φ==b.Φ){String[]k=a.Φ.Split('|');if(k.Length>=8) Ъ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ΕΠ=Ƥ-ƣ;float ΗЊ=ΕΠ.LengthSquared();float t=ΗЊ>0.01f?-Vector3.Dot(ǡ,ΕΠ)/ΗЊ:0;bool Εζ=ϊ<Ъ;if(!Εζ&&(t<=0||t>Ιξ||(ǡ+ΕΠ*t).Length()>=Ъ)) continue;
if(!Εζ&&ƣ.LengthSquared()>0.25f&&Ƥ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ƣ),Vector3.Normalize(Ƥ))<-0.7f)
continue;ΒΕ p=null;for(int k=0;k<ȱ.Count;k++)
if((ȱ[k].a==a.Υ&&ȱ[k].b==b.Υ)||(ȱ[k].a==b.Υ&&ȱ[k].b==a.Υ)) p=ȱ[k];if(p==null){p=new ΒΕ();p.a=a.Υ;p.b=b.Υ;p.Θβ=Ιο(a,b,ƣ,Ƥ,ǡ,ϊ);ȱ.Add(p);}p.ΑΣ=DateTime.Now;ȡ y=p.Θβ==a.Υ?a:b;float Ηξ=Εζ||t<4?0:Math.Max(1,y.Ǔ.Length()*0.5f);if(y.ȍ<0||Ηξ<y.ȍ) y.ȍ=Ηξ;
}}long Ιο(ȡ a,ȡ b,Vector3 ƣ,Vector3 Ƥ,Vector3 ǡ,float ϊ){if(a.Υ==ǘ) return b.Υ;if(b.Υ==ǘ) return a.Υ;Vector3 n=ϊ>0.01f?ǡ/ϊ:new Vector3(1,0,0);float ΘΖ=Vector3.Dot(ƣ,n),ΘΡ=Vector3.Dot(Ƥ,-n);if(ΘΖ-ΘΡ>1) return a.Υ;if(ΘΡ-ΘΖ>1) return b.Υ;return a.Υ>b.Υ?a.Υ:b.Υ;
}String Θι(){String β="C;";bool ǚ=true;for(int i=0;i<Ч.Count;i++){if(Ч[i].ȍ<0) continue;β+=(ǚ?"":",")+Ч[i].Υ+":"+Math.Round(Ч[i].ȍ,1);ǚ=false;}return β;}String ΘБ(){int Ηύ=0,ΑΝ=0;for(int i=0;i<Ч.Count;i++){if(!ΔΑ(Ч[i])) Ηύ++;if(Ч[i].ȍ>=0) ΑΝ++;}if(Ч.Count==0) return "";
return "Traffic: "+Ηύ+" moving"+(ΑΝ>0?", "+ΑΝ+" giving way":"")+"\n";}