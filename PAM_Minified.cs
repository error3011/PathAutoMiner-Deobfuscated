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
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size (the sweep covers the ship's width)
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

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Λ{ĵ,ǃ,ǲ,џ,ί}public enum Α{ё,Ȍ,ſ,ȭ,Ȭ,Й,ц,
ı,Ź,ȋ,ō,ŉ,ĭ,
ǅ,Ȱ,ǵ,Ō,ΒΪ,ΑЉ,
Δί,ΒΩ,Ÿ,ΑЋ,Ķ}public enum Σ{ш,ƀ,И,Γρ,ΑЄ,ΒΕ,ΒΖ,ų,Ŵ}public enum ў{None,Ō,ё,ΖΪ}public enum Υ{Main,ΖЅ,ǂ,ǁ,ΔΪ,ΑЂ,
Γΰ,Ζο,Ζπ,Ζν,Ǉ,ΖЊ,
ΒΠ,ΔΟ,ΔΤ,ΖЌ,ΒΣ}public enum щ{ΒΞ,ƺ,ΚΝ,Ζσ,ί}public enum ђ{ǉ,ΖΡ}public enum я{ΒΝ,Γχ,ΖЏ,ƾ}public enum Г{ΓБ,ΖΧ,Γλ,Γς,ΑЇ,ΓЂ,ΑБ}public enum ǝ{Γξ,Γε,Γζ}public enum Щ{ż,Ιω,Ŋ}public enum τ{ΔΒ,ΔΖ,ΔΗ,ΖЀ,ΖϏ,
ΔΑ,ΒΚ,ΒΙ,ΔΔ,
ΔΕ,ΔΓ}public enum η{ȧ,đ,Ȧ,ΑЁ,Γγ,Γδ}public enum Ǵ{ΑЎ,ΑЀ,Ιή}public enum Ǎ{ΒΤ,Ζά,ΙЄ,ΓА}public enum Ћ{None,Ɓ,Ǆ,ΒΘ}public enum ώ{ǋ,ΑϏ,ΒΓ,ƾ,ΓЎ,ΒΔ,ΓЏ,ΚΦ,ΔΘ}public enum У{ĵ,Ζϋ,Ζϊ,ΓЋ,ΓЄ,Γμ,Ζό}class ά{public bool ν=false;public Vector3 Ε=new Vector3();
public Vector3 ι=new Vector3();public Vector3 κ=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 ω=new Vector3();public Vector3 с=new Vector3();public float Ш=0;public float ΜΡ=0;public float[]ȇ=null;public ά(){}public ά(ά λ){ν=λ.ν;Ε=λ.Ε;ι=λ.ι;κ=λ.κ;Ђ=λ.Ђ;ω=λ.ω;
с=λ.с;ȇ=λ.ȇ;}public ά(Vector3 Ε,Vector3 κ,Vector3 ι,Vector3 Ђ,Vector3 ω){this.Ε=Ε;this.ι=ι;this.κ=κ;this.Ђ=Ђ;this.Ш=0;this.ω=ω;}public void ΙΖ(List<IMyThrust>Βή,List<string>ċ){ȇ=new float[ċ.Count];for(int i=0;i<ȇ.Length;i++)
ȇ[i]=-1;for(int i=0;i<Βή.Count;i++){string ǜ=Ζλ(Βή[i]);int Β=ċ.IndexOf(ǜ);if(Β!=-1)
ȇ[Β]=К(Βή[i].MaxEffectiveThrust,Βή[i].MaxThrust);}}}class ΔΙ{public ά ε=null;public List<Vector3>Ʈ=new List<Vector3>();public float ΑΜ=0;public float ΕА=0;public float ƈ=0;public float ΔЌ=0;public Vector3 Βψ=new Vector3();}class Ƅ{public τ Т=τ.ΔΒ;public float Ǹ=0;public float Ľ=0;
public string ǹ="";public string Ǿ="";DateTime ΘΕ;public bool Εκ=false;private bool ΔЀ=false;public bool Γι(bool ȃ){if(ȃ){Ľ=0;ΔЀ=false;return false;}ΔЀ=true;return Ľ>Ǹ;}public void ΙЎ(){Γι(true);Εκ=false;}public void Κθ(){if(ΔЀ)
if((DateTime.Now-ΘΕ).TotalSeconds>1){Ľ++;ΘΕ=DateTime.Now;}}public bool Γώ(){switch(Т){case τ.ΔΖ:return true;case τ.ΔΗ:return true;}return false;}}class о{public String α="";public String ψ="";public int Ќ=0;public Щ ƚ=Щ.Ŋ;public о(String α,String ψ,int Ќ,Щ ƚ){this.α=α;this.ψ=ψ;
this.Ќ=Ќ;this.ƚ=ƚ;}}class ΙΩ{public ΙΩ(Vector3 έ,float υ){this.έ=έ;this.υ=υ;}public Vector3 έ;public float υ;}class ό{public DateTime ΕΟ;public DateTime Ηή;public String Ξ="";public String α="";public String Γέ="";public String Κ="";public String Εό="";public String ΒЏ="";public String Ει="";
public Vector3 Ε=new Vector3();public List<о>Ѝ=new List<о>();public Λ Ν=Λ.ĵ;public Ћ Ĺ;public float ȝ;public float ȕ;public int А;public int ΔЅ=0;public int ΕЊ=0;public bool ΓЉ(){return(DateTime.Now-ΕΟ).TotalSeconds>10;}public bool ΓЅ(){if(Ĺ!=Ћ.Ɓ) return false;return(DateTime.Now-Ηή).TotalSeconds>=4;
}public ό(String Ξ){this.Ξ=Ξ;}public Ћ ĳ(String І,bool Λυ,bool в,bool Κψ){if(І==""&&!в) return Ћ.None;if(Ĺ==Ћ.Ɓ&&ΓЅ())
Ĺ=Ћ.ΒΘ;if(Λυ){Ει=І;Ĺ=Ћ.Ɓ;Ηή=DateTime.Now;return Ĺ;}else if(в){Ει="";Ĺ=Ћ.None;}else if(Ει==І){if(Κψ) Ĺ=Ћ.Ǆ;return Ĺ;}return Ћ.None;}}public String Ιζ(String ΛΛ){switch(ΛΛ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem φ;Vector3 ΓΖ=new Vector3();Vector3 δ=new Vector3();Vector3 Βϋ=new Vector3();Vector3 Αΰ=new Vector3();Vector3 ΑΟ=new Vector3();
DateTime ΕΣ=new DateTime();bool ę=true;int ΕЄ=0;bool Ğ=false;bool ğ=false;bool Αζ=true;bool ΘЋ=false;bool Ɨ=false;bool ΒЀ=false;float Ħ=0;float ŭ=0;int ņ=0;int Αψ=0;int Αμ=0;int Δυ=0;float Ɲ=0;float Ɖ=0;double Εέ=0;float ΑΕ=0;List<int>Ɣ=new List<int>();List<int>Ʀ=new List<int>();
Λ Ν=Λ.ĵ;int В=0;float Ȉ=0;float ȥ=0;float μ=0;float ΓΙ=0,Ευ=0;IMyRemoteControl Ω;IMySensorBlock ϊ;List<IMyTimerBlock>ΕІ=new List<IMyTimerBlock>();List<IMyShipConnector>ǎ=new List<IMyShipConnector>();List<IMyThrust>Č=new List<IMyThrust>();List<IMyGyro>Βό=new List<IMyGyro>();
List<IMyTerminalBlock>ϋ=new List<IMyTerminalBlock>();List<IMyLandingGear>Ć=new List<IMyLandingGear>();List<IMyReactor>ń=new List<IMyReactor>();List<IMyConveyorSorter>ΓΞ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǥ=new List<IMyGasTank>();List<IMyTerminalBlock>ǌ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ƌ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ŕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ĸ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ч=new List<IMyTextPanel>();List<IMyTextSurface>ȡ=new List<IMyTextSurface>();List<IMyTextPanel>Ř=new List<IMyTextPanel>();
IMyTerminalBlock Ǩ=null;ά Ρ=new ά();ά ΕΘ=new ά();ά ΕΛ=new ά();bool č=false;bool Ƴ=false;bool ƴ=false;bool ΒЂ=false;List<String>ċ=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Αύ=new List<ά>();int ΔІ=0;double ΘЌ=0;ά Ζ=new ά();Vector3 Ǘ;Vector3 ć;Σ ζ=Σ.ш;ђ ş=ђ.ǉ;int Ў=0;
int Ю=0;double Αί=0;bool Βμ=false;Ƅ Љ=new Ƅ();Ƅ Њ=new Ƅ();я њ=я.ΒΝ;bool Ň=true;ђ Ƞ=ђ.ǉ;int ѕ=3;int є=3;int р=30;ǝ Ǐ=ǝ.Γξ;bool Ĭ=true;bool Ɩ=true;bool Ɛ=true;bool Ů=false;Г ж=Г.ΓБ;bool ī=true;float Ȝ=90;float Ǖ=20;float т=5;float ǖ=20;float Ȑ=0.70f;float Ƹ=1.50f;
float Ʒ=2.50f;float Ų=0f;float Ŝ=0f;Α Ϋ;Σ ǔ;Α Αρ;ΔΙ ρ=null;int Є=0;int Ѐ=0;int г=0;int Ģ=0;int ū=30;int м=0;bool Е=true;Vector3 ј;double д=0;bool ΓΘ=false;int ΖΒ=0;int Ж=0;int Δκ=0;int Ȟ=0;int Θή=0;Vector3 Αθ=new Vector3();float ї=0;float х=0;int ű=0;ά ΓΟ=null;
float ΕΠ=0;float Βΰ=0;float л=0;float ţ=0;bool ΕΙ=false;bool ů=false;bool Αω=false;bool Ĝ=false;bool Ĕ=false;DateTime ΚЉ=new DateTime();ά Ŗ=null;ά Ηζ=null;float Ȳ=0;ў ȑ;float ĝ=0;float Ű=0;String ΕΨ="";float ΒІ=0;float ȕ=0;float ȝ=0;List<о>Ǽ=new List<о>();float з=0;
bool Αξ=false;bool ΘЀ=false;bool Θμ=false;bool ΓΥ=false;bool Θν=false;float ΕΫ=0;float Εθ=0;Vector3 Ηω=new Vector3();Vector3 Ѓ=new Vector3();Vector3 ΓΦ=new Vector3(1,1,1);float ΕЃ=1;Vector3 Αυ=new Vector3();bool ΒЁ=false;bool ΕΑ=false;bool ΕΥ=false;bool ΗΙ=false;float Βκ=0;
float Βά=2;Vector3 Θύ;Vector3 ΕЀ;Vector3 ΕϏ;float[,]ȉ=new float[3,2];Dictionary<String,float[,]>ǭ=new Dictionary<string,float[,]>();Dictionary<String,float>ΘЉ=new Dictionary<String,float>();int Δξ=0;Dictionary<String,float[]>Ĥ=new Dictionary<string,float[]>();float Ελ;bool Βξ=false;
int Βί=0;int Ł=0;int Βώ=0;int ŝ=0;String Κ="";int[]ΘΣ=new int[Enum.GetValues(typeof(Υ)).Length];bool Εί=false;int Ť=0;int А=0;Υ Ϗ=Υ.Main;int ΛЄ=0,ΛЅ=0,ΛІ=0,ΛЇ=0;int Λπ=0,Λρ=0;bool Ƌ=false;bool ΗΚ=false;String ƫ="";String Δτ="";IMyBroadcastListener Ƕ=null;
bool Ηψ=true;List<ό>Н=null;ό М=null;int Βδ=0,Βε=0;int Ηα=0,Ηβ=0;int Κύ=0,Κώ=0,ΚϏ=0;String ΛΝ="";Exception ġ=null;bool ΓΗ=false;Σ ƨ=Σ.ш;bool Ηι=false;bool ΘΓ=true;bool Ερ=false;String ΘЊ="";bool Αχ=false;bool Εϋ=true;int ΒϏ=0;int[]ǫ=null;Vector3 Δφ=new Vector3();
bool Ηώ=false;bool Ǟ(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;Program(){φ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓϏ(Me,controllerTag,true))
Ν=Λ.џ;ξ(Υ.Main);if(Ν!=Λ.џ){Κ="Welcome to [PAM]!";Ǎ Επ=ΙЏ();if(Ν==Λ.ĵ){List<IMyShipDrill>Ηυ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗϏ=new List<IMyShipGrinder>();φ.GetBlocksOfType(Ηυ,q=>q.CubeGrid==Me.CubeGrid);φ.GetBlocksOfType(ΗϏ,q=>q.CubeGrid==Me.CubeGrid);if(Ηυ.Count>0)
{Ν=Λ.ǃ;Κ="Miner mode enabled!";}else if(ΗϏ.Count>0){Ν=Λ.ǲ;Κ="Grinder mode enabled!";}else{Ν=Λ.ί;Ȯ(щ.ί);}}if(Επ==Ǎ.ΒΤ) Αζ=false;if(Επ==Ǎ.Ζά) Κ="Data restore failed!";if(Επ==Ǎ.ΓА) Κ="New version, wipe data";}}void Main(string Ф,UpdateType ΛЏ){try{if(ġ!=null){if(Ф.Trim().ToUpper()=="RETRY")
{ġ=null;ę=true;ņ=0;if(Ν!=Λ.џ) Ѕ();Κ="Restarted after error";}else{Ις();ΖЃ();return;}}ğ=(ΛЏ&UpdateType.Update10)!=0;if(ğ) ΕЄ++;Ğ=ΕЄ>=10;if(Ğ) ΕЄ=0;if(ğ){Βί++;if(Βί>4) Βί=0;Αμ=(int) Math.Max(0,10-(DateTime.Now-ΕΣ).TotalSeconds);}if(Ф!="") ΛΝ=Ф;if(Ν!=Λ.џ)
ΚΜ(Ф);else
ΙΦ(Ф);Εί=false;try{int Ηη=Runtime.CurrentInstructionCount;float Ū=К(Ηη,Runtime.MaxInstructionCount);if(Ū>0.90) Κ="Max. instructions >90%";if(Ū>Ɲ) Ɲ=Ū;if(Βξ){Ɣ.Add(Ηη);while(Ɣ.Count>10) Ɣ.RemoveAt(0);Ɖ=0;for(int i=0;i<Ɣ.Count;i++) Ɖ+=Ɣ[i];Ɖ=К(К(Ɖ,Ɣ.Count),Runtime.MaxInstructionCount);
double ΕΡ=Runtime.LastRunTimeMs;if(ΒЀ&&ΕΡ>Εέ) Εέ=ΕΡ;Ʀ.Add((int)(ΕΡ*1000f));while(Ʀ.Count>10) Ʀ.RemoveAt(0);ΑΕ=0;for(int i=0;i<Ʀ.Count;i++) ΑΕ+=Ʀ[i];ΑΕ=К(ΑΕ,Ʀ.Count)/1000f;}}catch{Ɲ=0;}}catch(Exception e){ġ=e;}}void ΚΜ(string Ф){bool ΓΚ=false;String ΗΦ="";if(В<=1&&!ΓЈ(Ф))
ΖЄ(Ф);int Εΰ=50;while(Ƕ!=null&&Ƕ.HasPendingMessage&&Εΰ-->0){MyIGCMessage γ=Ƕ.AcceptMessage();String Η=γ.Data as String;if(Η==null) continue;String ȟ="";if(В<=1&&ΖЂ(ref Η,out ȟ,out ΗΦ)&&ȟ==BROADCAST_SELF_ID){ΖЄ(Η);ΓΚ=true;break;}}bool ΗЍ=ΒЀ&&Ϋ==Α.ё&&!Ɨ&&!ΓΚ&&ņ==0&&!ΒЂ;
if(Ğ&&Ϋ!=Α.ё) ΓΚ=true;if((ğ&&!ΗЍ)||(Ğ&&ΗЍ)){if(ņ==0&&(Αμ<=0||ę)){ΘЋ=В>0;В=0;ņ=1;ȯ();ΚΓ();Ζρ();Ȩ("Scan 1");}else if(ņ==1){ņ=2;ȯ();ΚΔ();Ȩ("Scan 2");}else if(ņ==2){ņ=0;ȯ();ΚΕ();ΚΖ();if(В<=1) Ζω();Ȩ("Scan 3");ΕΣ=DateTime.Now;if(В<=1&&Αζ)
ΓΖ=ΑЏ(Ω,Ω.CenterOfMass);Αζ=false;if(ę){ę=false;bool ΜΓ=ζ==Σ.И;Ѕ();if(ΘΓ&&resumeAfterRestart&&ΜΓ&&В<=1) Ερ=true;}ΘΓ=false;if(ΘЋ&&В==0) Κ="Setup complete";}else{if(ζ==Σ.И&&Ν!=Λ.ί){ȯ();ΘА();Ȩ("Inv balance");}ȯ();switch(Αψ){case 0:ΚΪ();break;case 1:Κα();break;case 2:ΚΩ();break;
case 3:Κμ();break;case 4:Κΰ();break;case 5:ΙΚ();break;case 6:ΙΘ(Ω);break;}Ȩ("Update: "+Αψ);Αψ++;if(Αψ>6){Αψ=0;ΒЀ=true;if(ƨ!=Σ.ш){switch(ƨ){case Σ.ų:į();break;case Σ.Ŵ:į();break;case Σ.И:į();break;case Σ.ΒΕ:ΑЌ();break;case Σ.ΒΖ:Ŏ();break;}ƨ=Σ.ш;}}}if(!ę){if(!Ȫ(Ω,true))
{Ω=null;Αζ=true;В=2;}if(В>=2&&Ϋ!=Α.ё)
Ѕ();if(В<=1){ŭ=Ω.CalculateShipMass().PhysicalMass;Ħ=(float) Ω.GetShipSpeed();δ=ΚΨ(Ω,ΓΖ);Βϋ=Ω.WorldMatrix.Forward;Αΰ=Ω.WorldMatrix.Left;ΑΟ=Ω.WorldMatrix.Down;Κε();Κι();Κέ();if(Ερ){Ερ=false;į();if(Ϋ!=Α.ё) Κ="Job resumed after restart";}if(Ϋ!=Α.ё){Ɨ=false;ΖЇ(false);ΖΟ(false);
String Λϊ=Γϊ(Ϋ)+" "+(int) Ϋ;ȯ();Κγ();Δή(false);Ȩ(Λϊ);Κκ();Κδ();ȯ();Κί();Ȩ("Thruster");ȯ();ΗΕ();Ȩ("Gyroscope");}else{if(Ɨ){if(ΒΒ()){н(ΑΟ,Βϋ,Αΰ,0.25f,true);ΗΕ();Κ="Aligning to planet: "+Math.Round(Βκ-0.25f,2)+"°";if(ΒЁ) Γύ(true,true);}else Γύ(true,true);}}Αχ=false;}}}ȯ();ΖЃ();Ȩ("Print");
if(ΓΚ||Δυ<=0){ȯ();ΘБ(ΗΦ);Ȩ("Broadcast");Δυ=4;}else if(Ğ) Δυ--;}void ΖЄ(string Ф){if(Ф=="") return;var χ=Ф.ToUpper().Split(' ');χ.DefaultIfEmpty("");var І=χ.ElementAtOrDefault(0);var ȱ=χ.ElementAtOrDefault(1);var ΑΓ=χ.ElementAtOrDefault(2);var ΗΜ=χ.ElementAtOrDefault(3);String ą="Invalid argument: "+Ф;
bool ΕЏ=false;switch(І){case "UP":this.ΓЍ(false);break;case "DOWN":this.ΓЌ(false);break;case "UPLOOP":this.ΓЍ(true);break;case "DOWNLOOP":this.ΓЌ(true);break;case "APPLY":this.ΖΝ(true);break;case "MRES":А=0;break;case "STOP":this.Ѕ();break;case "PATHHOME":{this.Ѕ();this.ΔΨ();}break;
case "PATH":{this.Ѕ();this.ΔΨ();Ρ.ν=true;}break;case "START":{this.Ѕ();ΔΧ();}break;case "ALIGN":{Γύ(!Ɨ,false);}break;case "CONT":{this.Ѕ();this.į();}break;case "JOBPOS":{this.Ѕ();this.Ŏ();}break;case "HOMEPOS":{this.Ѕ();this.ΑЌ();}break;case "FULL":{Αξ=true;}break;
case "RESET":{ΓΗ=true;В=2;}break;case "EXPORT":{Ιί();}break;case "IMPORT":{Ιχ(ȱ=="JOB");}break;default:ΕЏ=true;break;}if(Ν!=Λ.ί){switch(І){case "SHUTTLE":{Ιά();}break;case "CFGS":{if(!ΙΤ(ȱ,ΑΓ,ΗΜ)) Κ=ą;}break;case "CFGB":{if(!ΙΣ(ȱ,ΑΓ)) Κ=ą;}break;case "CFGL":
{if(!ŏ(ref Ȝ,true,ώ.ΒΓ,ȱ,"")||!ΙΥ(ΑΓ))
Κ=ą;}break;case "CFGE":{if(!ŏ(ref т,true,ώ.ΒΔ,ȱ,"IG")||
!ŏ(ref Ǖ,true,ώ.ΓЎ,ΑΓ,"IG")||
!ŏ(ref ǖ,true,ώ.ΓЏ,ΗΜ,"IG"))
Κ=ą;}break;case "CFGA":{if(!ŏ(ref Ȑ,false,ώ.ΑϏ,ȱ,"")) Κ=ą;}break;case "CFGW":{if(!ŏ(ref Ƹ,false,ώ.ǋ,ȱ,"")||
!ŏ(ref Ʒ,false,ώ.ǋ,ΑΓ,""))
Κ=ą;}break;case "NEXT":{Γί(false);}break;case "PREV":{Γί(true);}break;default:if(ΕЏ) Κ=ą;break;}}else{switch(І){case "UNDOCK":{Αχ=true;}break;default:if(ΕЏ) Κ=ą;break;}}}String Ιη(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ν!=Λ.ί)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ιά(){Ѕ();Ν=Λ.ί;Ȯ(щ.ί);Ζ.ν=false;Ρ.ν=false;ϊ=null;ϋ.Clear();ζ=Σ.ш;}void Γύ(bool О,bool ύ){if(!О) Κ="Aligning canceled";if(ύ) Κ="Aligning done";if(ύ||!О){Ɨ=false;ΖА();ΒΡ(false,0,0,0,0);return;}if(ΒΒ()) Ɨ=true;}bool ΙΤ(String ΜΕ,String ΛΔ,String ΚЃ)
{bool ΜΑ=ζ==Σ.И;int w,h,d;if(int.TryParse(ΜΕ,out w)&&int.TryParse(ΛΔ,out h)&&int.TryParse(ΚЃ,out d)){this.Ѕ();ѕ=w;є=h;р=d;œ(false);Ǌ(false,false);if(ΜΑ) į();return true;}return false;}bool ΙΥ(String ΑΒ){if(ΑΒ=="ON"){Ĭ=true;return true;}if(ΑΒ=="OFF"){Ĭ=false;return true;}
return false;}bool ΙΣ(String Ητ,String Βλ){bool Ĉ=true;if(Ητ=="HOME") Ň=true;else if(Ητ=="STOP") Ň=false;else Ĉ=false;if(Βλ=="HOME") њ=я.ΒΝ;else if(Βλ=="STOP") њ=я.ΖЏ;else if(Βλ=="JOB") њ=я.Γχ;else if(Βλ=="IG") њ=я.ƾ;else Ĉ=false;return Ĉ;}void ξ(Υ ы){ΘΣ[(int) Ϗ]=А;
А=ΘΣ[(int) ы];if(ы==Υ.Ǉ) А=0;Ϗ=ы;if(Ν!=Λ.џ)
Œ(Ϗ==Υ.ǂ,false,0,0);Εί=true;}void ΓЍ(bool ΕΦ){if(А>0) А--;else if(ΕΦ) А=Ť-1;}void ΓЌ(bool ΕΦ){if(А<Ť-1) А++;else if(ΕΦ) А=0;}String Ȯ(щ γ){switch(γ){case щ.ΒΞ:Κ="Job is running";break;case щ.ƺ:Κ="Connector not ready!";break;case щ.ΚΝ:Κ="Ship modified, path outdated!";break;
case щ.Ζσ:Κ="Interrupted by player!";break;case щ.ί:Κ="Shuttle mode enabled!";break;}return "";}String Ιν(ђ έ){switch(έ){case ђ.ǉ:return "Top-Left";case ђ.ΖΡ:return "Center";default:return "";}}String Ιγ(ǝ Б){switch(Б){case ǝ.Γε:return "Auto"+(Ν==Λ.ǃ?" (Ore)":"");case ǝ.Γζ:return "Auto (+Stone)";
case ǝ.Γξ:return "Default";default:return "";}}String Γψ(Σ Ї){switch(Ї){case Σ.ш:return "No job";case Σ.ƀ:return "Job paused";case Σ.И:return "Job active";case Σ.ų:return "Job active";case Σ.Ŵ:return "Job active";case Σ.Γρ:return "Job done";case Σ.ΑЄ:return "Job changed";case Σ.ΒΕ:return "Move home";
case Σ.ΒΖ:return "Move to job";}return "";}String Ζβ(я Κχ){switch(Κχ){case я.ΒΝ:return "Return home";case я.Γχ:return "Fly to job pos";case я.ΖЏ:return "Stop";case я.ƾ:return "Ignore";}return "";}String Ιε(Г Б){switch(Б){case Г.ΓБ:return "Off";case Г.Γς:return "Drop pos (Stone) ";case Г.ΑЇ:return "Drop pos (Sto.+Ice)";
case Г.ΖΧ:return "Cur. pos (Stone)";case Г.Γλ:return "Cur. pos (Sto.+Ice)";case Г.ΓЂ:return "In motion (Stone)";case Г.ΑБ:return "In motion (Sto.+Ice)";}return "";}String Ζΰ(ў Ї){switch(Ї){case ў.None:return "No batteries";case ў.Ō:return "Charging";case ў.ΖΪ:return "Discharging";}return "";}
String Γϊ(Α Ї){String ΜΟ=Ν==Λ.ί?"target":"job";switch(Ї){case Α.ё:return "Idle";case Α.Ȍ:return "Flying to XY position";case Α.ſ:return Ν==Λ.ǲ?"Grinding":"Mining";case Α.ȭ:return "Returning";case Α.Ÿ:return "Flying to drop pos";case Α.Ȭ:return "Returning to dock";case Α.Й:return "Flying to dock area";
case Α.ц:return "Flying to job area";case Α.ı:return "Flying to path";case Α.Ź:return "Flying to job position";case Α.ȋ:return "Approaching dock";case Α.ō:return "Docking";case Α.ŉ:return "Aligning";case Α.ĭ:return "Aligning";case Α.ǅ:return "Retry docking";case Α.Ȱ:return "Unloading";case Α.Ķ:return ΘЊ;
case Α.ǵ:return "Undocking";case Α.Ō:return "Charging batteries";case Α.ΒΪ:return "Waiting for uranium";case Α.ΑЉ:return "Filling up hydrogen";case Α.Δί:return "Waiting for ejection";case Α.ΒΩ:return "Waiting for ejection";case Α.ΑЋ:return "Flying to drop pos";}return "";}String Ζμ(τ Т){switch(Т)
{case τ.ΔΒ:return "On \"Undock\" command";case τ.ΔΖ:return "On player entered cockpit";case τ.ΖЀ:return "On ship is full";case τ.ΖϏ:return "On ship is empty";case τ.ΔΗ:return "On time delay";case τ.ΒΚ:return "On batteries empty(<25%)";case τ.ΒΙ:return "On batteries empty(=0%)";case τ.ΔΑ:return "On batteries full";
case τ.ΔΕ:return "On hydrogen empty(<25%)";case τ.ΔΓ:return "On hydrogen empty(=0%)";case τ.ΔΔ:return "On hydrogen full";}return "";}bool Ο(ref String Γ,int Θι,int ŀ,bool Ι,String Š){Ť+=1;if(Θι==ŀ) Š=">"+Š+(Βί>=2?" .":"");else Š=" "+Š;Γ+=Š+"\n";return Θι==ŀ&&Ι;}String ΖΝ(bool Ι)
{int Β=0;int Θ=А;Ť=0;String ƪ="———————————————\n";String ư="--------------------------------------------\n";String Γ="";Γ+=Γψ(ζ)+" | "+(Ρ.ν?"Ready to dock":"No dock")+"\n";Γ+=ƪ;double Ȇ=Math.Max(Math.Round(this.д),0);if(Ϗ==Υ.Main){bool ł=Ν==Λ.ί;if(Ο(ref Γ,Θ,Β++,Ι," Record path & set home")) ΔΨ();
if(Ν==Λ.ǃ)
if(Ο(ref Γ,Θ,Β++,Ι," Setup mining job")) ξ(Υ.ǂ);if(Ν==Λ.ǲ)
if(Ο(ref Γ,Θ,Β++,Ι," Setup grinding job")) ξ(Υ.ǂ);if(Ν==Λ.ί)
if(Ο(ref Γ,Θ,Β++,Ι," Setup shuttle job")) ξ(Υ.ΔΤ);if(Ο(ref Γ,Θ,Β++,Ι," Continue job")) į();if(Ο(ref Γ,Θ,Β++,Ι," Fly to home position")) ΑЌ();if(Ο(ref Γ,Θ,Β++,Ι," Fly to job position")) Ŏ();if(Ο(ref Γ,Θ,Β++,Ι," Behavior settings"))
if(ł) ξ(Υ.ΒΣ);else ξ(Υ.ΑЂ);if(Ο(ref Γ,Θ,Β++,Ι," Info")) ξ(Υ.Ζο);if(Ν!=Λ.ί)
if(Ο(ref Γ,Θ,Β++,Ι," Help")) ξ(Υ.Ζν);}else if(Ϗ==Υ.ǂ){double ΜΔ=Math.Round(ѕ*Ȉ,1);double ΛΓ=Math.Round(є*ȥ,1);String З="";if(Ο(ref З,Θ,Β++,Ι," Start new job!")) ΔΧ();if(Ο(ref З,Θ,Β++,Ι," Change current job")){Ǌ(false,false);ξ(Υ.Main);}if(Ο(ref З,Θ,Β++,Ι," Width + (Width: "+ѕ+" = "+ΜΔ+"m)")){ŵ(ref ѕ,5,20,1);œ(true);}
if(Ο(ref З,Θ,Β++,Ι," Width -")){ŵ(ref ѕ,-5,20,-1);œ(true);}if(Ο(ref З,Θ,Β++,Ι," Height + (Height: "+є+" = "+ΛΓ+"m)")){ŵ(ref є,5,20,1);œ(true);}if(Ο(ref З,Θ,Β++,Ι," Height -")){ŵ(ref є,-5,20,-1);œ(true);}if(Ο(ref З,Θ,Β++,Ι," Depth + ("+(Ǐ==ǝ.Γξ?"Depth":"Min")+": "+р+"m)")){ŵ(ref р,5,50,2);œ(true);}
if(Ο(ref З,Θ,Β++,Ι," Depth -")){ŵ(ref р,-5,50,-2);œ(true);}if(Ο(ref З,Θ,Β++,Ι," Depth mode: "+Ιγ(Ǐ))){Ǐ=İ(Ǐ);}if(Ο(ref З,Θ,Β++,Ι," Start pos: "+Ιν(Ƞ))){Ƞ=İ(Ƞ);}if(Ν==Λ.ǲ&&Ǐ==ǝ.Γζ) Ǐ=İ(Ǐ);Γ+=ΔΞ(8,З,Θ,ref Λπ);}else if(Ϗ==Υ.ΔΤ){float[]Ηλ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ο(ref Γ,Θ,Β++,Ι," Next")) ξ(Υ.ΖЌ);if(Ο(ref Γ,Θ,Β++,Ι," Back")) ξ(Υ.Main);Γ+=" Leave connector 1:\n";if(Ο(ref Γ,Θ,Β++,Ι," - "+Ζμ(Љ.Т))) Љ.Т=İ(Љ.Т);if(!Љ.Γώ()) Γ+="\n";else if(Ο(ref Γ,Θ,Β++,Ι," - Delay: "+ΑЍ((int) Љ.Ǹ))) Љ.Ǹ=ΖΨ(Љ.Ǹ,Ηλ);Γ+=" Leave connector 2:\n";if(Ο(ref Γ,Θ,Β++,Ι," - "+Ζμ(Њ.Т))) Њ.Т=İ(Њ.Т);
if(!Њ.Γώ()) Γ+="\n";else if(Ο(ref Γ,Θ,Β++,Ι," - Delay: "+ΑЍ((int) Њ.Ǹ))) Њ.Ǹ=ΖΨ(Њ.Ǹ,Ηλ);}else if(Ϗ==Υ.ΖЌ){if(Ο(ref Γ,Θ,Β++,Ι," Start job!")) ΔΧ();if(Ο(ref Γ,Θ,Β++,Ι," Back")) ξ(Υ.ΔΤ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ο(ref Γ,Θ,Β++,Ι," = "+(Љ.ǹ!=""?Љ.ǹ:"-"))) Љ.ǹ=ΒΗ(ref ΛЄ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ο(ref Γ,Θ,Β++,Ι," = "+(Љ.Ǿ!=""?Љ.Ǿ:"-"))) Љ.Ǿ=ΒΗ(ref ΛІ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ο(ref Γ,Θ,Β++,Ι," = "+(Њ.ǹ!=""?Њ.ǹ:"-"))) Њ.ǹ=ΒΗ(ref ΛЅ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ο(ref Γ,Θ,Β++,Ι," = "+(Њ.Ǿ!=""?Њ.Ǿ:"-"))) Њ.Ǿ=ΒΗ(ref ΛЇ);
}else if(Ϗ==Υ.ǁ){String ΘΚ=Ȝ+" %";if(Ğ) Ł++;if(Ł>1){Ł=0;Βώ++;if(Βώ>1) Βώ=0;}bool[]Εψ=new bool[]{ń.Count==0,ȑ==ў.None,Ǥ.Count==0};int ΘЅ=0;while(true){ΘЅ++;ŝ++;if(ŝ>Εψ.Length-1) ŝ=0;if(ΘЅ>=Εψ.Length) break;if(!Εψ[ŝ]) break;}bool ł=Ν==Λ.ί;if(!ł&&Ĭ&&з!=-1&&Βώ==0)
ΘΚ=з<1000000?Math.Round(з)+" Kg":Math.Round(з/1000)+" t";if(Ο(ref Γ,Θ,Β++,Ι," Stop!")){Ѕ();ξ(Υ.Main);}if(Ο(ref Γ,Θ,Β++,Ι," Behavior settings"))
if(!ł) ξ(Υ.ΑЂ);else ξ(Υ.ΒΣ);if(!ł){if(Ο(ref Γ,Θ,Β++,Ι," Next hole")) Γί(false);}else if(Ο(ref Γ,Θ,Β++,Ι," Undock")) Αχ=true;Γ+=ư;if(!ł) Γ+="Progress: "+Math.Round(Αί,1)+" %\n";Γ+="State: "+Γϊ(Ϋ)+" "+Ȇ+"m \n";Γ+="Load: "+ΒІ+" % Max: "+ΘΚ+" \n";if(ŝ==0) Γ+="Uranium: "+(ń.Count==0?"No reactors":Math.Round(Ű,1)+"Kg "+(т==-1?"":" Min: "+т+" Kg"))+"\n";
if(ŝ==1) Γ+="Battery: "+(ȑ==ў.None?Ζΰ(ȑ):Ȳ+"% "+(Ǖ==-1||ł?"":" Min: "+Ǖ+" %"))+"\n";if(ŝ==2) Γ+="Hydrogen: "+(Ǥ.Count==0?"No tanks":Math.Round(ĝ,1)+"% "+(ǖ==-1||ł?"":" Min: "+ǖ+" %"))+"\n";}else if(Ϗ==Υ.ΑЂ){String З="";if(Ο(ref З,Θ,Β++,Ι," Back")){if(ζ==Σ.И) ξ(Υ.ǁ);else ξ(Υ.Main);}
if(Ο(ref З,Θ,Β++,Ι," Max load: "+Ȝ+"%")) ю(ref Ȝ,Ȝ<=80?-10:-5,ώ.ΒΓ,false);if(Ο(ref З,Θ,Β++,Ι," Weight limit: "+(Ĭ?"On":"Off"))) Ĭ=!Ĭ;if(Ο(ref З,Θ,Β++,Ι," Ejection: "+Ιε(ж))) ж=İ(ж);if(Ο(ref З,Θ,Β++,Ι," Toggle sorters: "+(Ů?"On":"Off"))){Ů=!Ů;if(Ů) ȏ(Εϋ);}
if(Ο(ref З,Θ,Β++,Ι," Unload ice: "+(ī?"On":"Off"))) ī=!ī;if(Ο(ref З,Θ,Β++,Ι," Uranium: "+(т==-1?"Ignore":"Min "+т+"Kg"))) ю(ref т,(т>5?-5:-1),ώ.ΒΔ,true);if(Ο(ref З,Θ,Β++,Ι," Battery: "+(Ǖ==-1?"Ignore":"Min "+Ǖ+"%"))) ю(ref Ǖ,-5,ώ.ΓЎ,true);if(Ο(ref З,Θ,Β++,Ι," Hydrogen: "+(ǖ==-1?"Ignore":"Min "+ǖ+"%"))) ю(ref ǖ,-10,ώ.ΓЏ,true);
if(Ο(ref З,Θ,Β++,Ι," When done: "+(Ň?"Return home":"Stop"))) Ň=!Ň;if(Ο(ref З,Θ,Β++,Ι," On damage: "+Ζβ(њ))) њ=İ(њ);if(Ο(ref З,Θ,Β++,Ι," Advanced...")) ξ(Υ.Γΰ);Γ+=ΔΞ(8,З,Θ,ref Λρ);}else if(Ϗ==Υ.Γΰ){if(Ο(ref Γ,Θ,Β++,Ι," Back")){if(ζ==Σ.И) ξ(Υ.ǁ);else ξ(Υ.Main);}
if(Ο(ref Γ,Θ,Β++,Ι,(Ν==Λ.ǲ?" Grinder":" Drill")+" inv. balancing: "+(Ɩ?"On":"Off"))) Ɩ=!Ɩ;if(Ο(ref Γ,Θ,Β++,Ι," Enable"+(Ν==Λ.ǲ?" grinders":" drills")+": "+(Ɛ?"Fwd + Bwd":"Fwd"))) Ɛ=!Ɛ;if(Ο(ref Γ,Θ,Β++,Ι," Work speed fwd.: "+Ƹ+"m/s")) ю(ref Ƹ,0.5f,ώ.ǋ,false);
if(Ο(ref Γ,Θ,Β++,Ι," Work speed bwd.: "+Ʒ+"m/s")) ю(ref Ʒ,0.5f,ώ.ǋ,false);if(Ο(ref Γ,Θ,Β++,Ι," Acceleration: "+Math.Round(Ȑ*100f)+"%"+(Ȑ>0.80f?" (risky)":""))) ю(ref Ȑ,0.1f,ώ.ΑϏ,false);if(Ο(ref Γ,Θ,Β++,Ι," Width overlap: "+Ų*100f+"%")) ΖΣ(true,0.05f);if(Ο(ref Γ,Θ,Β++,Ι," Height overlap: "+Ŝ*100f+"%")) ΖΣ(false,0.05f);
}else if(Ϗ==Υ.ΒΣ){if(Ο(ref Γ,Θ,Β++,Ι," Back")){if(ζ==Σ.И) ξ(Υ.ǁ);else ξ(Υ.Main);}if(Ο(ref Γ,Θ,Β++,Ι," Max load: "+Ȝ+"%")) ю(ref Ȝ,Ȝ<=80?-10:-5,ώ.ΒΓ,false);if(Ο(ref Γ,Θ,Β++,Ι," Unload ice: "+(ī?"On":"Off"))) ī=!ī;if(Ο(ref Γ,Θ,Β++,Ι," Uranium: "+(т==-1?"Ignore":"Min "+т+"Kg"))) ю(ref т,(т>5?-5:-1),ώ.ΒΔ,true);
if(Ο(ref Γ,Θ,Β++,Ι," Battery: "+(Ǖ==-1?"Ignore":"Charge up"))) Ǖ=(Ǖ==-1?1:-1);if(Ο(ref Γ,Θ,Β++,Ι," Hydrogen: "+(ǖ==-1?"Ignore":"Fill up"))) ǖ=(ǖ==-1?1:-1);if(Ο(ref Γ,Θ,Β++,Ι," On damage: "+Ζβ(њ))) њ=İ(њ);if(Ο(ref Γ,Θ,Β++,Ι," Acceleration: "+Math.Round(Ȑ*100f)+"%"+(Ȑ>0.80f?" (risky)":""))) ю(ref Ȑ,0.1f,ώ.ΑϏ,false);
}else if(Ϗ==Υ.ΖЅ){double ΛΞ=0;if(Ψ.Count>0) ΛΞ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ο(ref Γ,Θ,Β++,Ι," Stop path recording")) ΖБ();if(Ν!=Λ.ί){if(Ο(ref Γ,Θ,Β++,Ι," Home: "+(č?"Use old home":(Ρ.ν?"Was set! ":"none ")))) č=!č;}else{if(Ο(ref Γ,Θ,Β++,Ι," Connector 1: "+(č?"Use old connector":(Ρ.ν?"Was set! ":"none ")))) č=!č;
if(Ο(ref Γ,Θ,Β++,Ι," Connector 2: "+(Ƴ?"Use old connector":(Ζ.ν?"Was set! ":"none ")))) Ƴ=!Ƴ;}if(Ο(ref Γ,Θ,Β++,Ι," Path: "+(ƴ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ƴ=!ƴ;Γ+=ư;Γ+="Wp spacing: "+Math.Round(ΘЌ)+"m\n";}else if(Ϗ==Υ.ΔΪ){if(Ο(ref Γ,Θ,Β++,Ι," Stop")){Ѕ();ξ(Υ.Main);}
Γ+=ư;Γ+="State: "+Γϊ(Ϋ)+" \n";Γ+="Speed: "+Math.Round(Ħ,1)+"m/s\n";Γ+="Target dist: "+Ȇ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+ΖΒ+"\n";}else if(Ϗ==Υ.Ζο){List<IMyTerminalBlock>ŗ=Ζγ();if(Ğ) Ł++;if(Ł>=ŗ.Count) Ł=0;if(Ο(ref Γ,Θ,Β++,Ι," Next")) ξ(Υ.Ζπ);
Γ+=ư;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΒІ,1)+"% "+Math.Round(ȕ,1)+" / "+Math.Round(ȝ,1)+"\n";Γ+="Uranium: "+(ń.Count==0?"No reactors":Math.Round(Ű,1)+"Kg "+ΕΨ)+"\n";Γ+="Battery: "+(ȑ==ў.None?"":Ȳ+"% ")+Ζΰ(ȑ)+"\n";Γ+="Hydrogen: "+(Ǥ.Count==0?"No tanks":Math.Round(ĝ,1)+"% ")+"\n";
Γ+="Damage: "+(ŗ.Count==0?"None":""+(Ł+1)+"/"+ŗ.Count+" "+ŗ[Ł].CustomName)+"\n";}else if(Ϗ==Υ.Ζπ){if(Ο(ref Γ,Θ,Β++,Ι," Back")) ξ(Υ.Main);Γ+=ư;Γ+="Next scan: "+Αμ+"s\n";Γ+="Drill area: "+Math.Round(Ȉ,1)+"x"+Math.Round(ȥ,1)+"m Ship: "+Math.Round(ΓΙ,1)+"x"+Math.Round(Ευ,1)+"m ("+Math.Round(μ,1)+"m)\n"+(Ȓ.Count>0?"Cameras: "+Δχ+"\n":"");
Γ+="Broadcast: "+(Ƌ?"Online - "+ƫ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ɲ*100f,1)+"% \n";}else if(Ϗ==Υ.Ζν){if(Ο(ref Γ,Θ,Β++,Ι," Back")) ξ(Υ.Main);Γ+=ư;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(В==2) Γ="Fatal setup error\nNext scan: "+Αμ+"s\n";if(ΓΗ) Γ="Recompile script now";int ΛΟ=Γ.Split('\n').Length;for(int i=ΛΟ;i<=10;i++) Γ+="\n";Γ+=ƪ;Γ+="Last: "+Κ+"\n";
return Γ;}void ΖΣ(bool ΛΚ,float Ǭ){Ѕ();Ǌ(true,false);if(ΛΚ) ю(ref Ų,Ǭ,ώ.ΔΘ,false);else ю(ref Ŝ,Ǭ,ώ.ΔΘ,false);ΖΠ();Œ(true,true,0,0);}float ΖΨ(float ς,float[]Γά){float ΰ=Γά[0];for(int i=Γά.Length-1;i>=0;i--)
if(ς<Γά[i]) ΰ=Γά[i];return ΰ;}String ΒΗ(ref int ŀ){String ΰ="";if(ŀ>=ΕІ.Count) ŀ=-1;if(ŀ>=0) ΰ=ΕІ[ŀ].CustomName;ŀ++;return ΰ;}void ΒΧ(string α){if(ζ!=Σ.И) return;if(α=="") return;IMyTerminalBlock Ϊ=φ.GetBlockWithName(α);if(Ϊ==null||!(Ϊ is IMyTimerBlock)){Κ="Timerblock "+α+" not found!";
return;}((IMyTimerBlock) Ϊ).Trigger();}void ŵ(ref int value,int ΑΘ,int ĩ,int Εω){if(ΑΘ==0) return;if(value<ĩ&&Εω>0||value<=ĩ&&Εω<0){value+=Εω;return;}int Κξ=Math.Abs(ΑΘ);int Δδ=0;int Εα=1;while(true){Δδ+=Εα*Κξ*10;if(ΑΘ<0&&value-ĩ<=Δδ) break;if(ΑΘ>0&&value-ĩ<Δδ) break;
Εα++;}value+=Εα*ΑΘ;}void œ(bool ΜΣ){ѕ=Math.Max(ѕ,1);є=Math.Max(є,1);р=Math.Max(р,0);Œ(Ϗ==Υ.ǂ,false,0,0);}bool ŏ(ref float value,bool Λμ,ώ ψ,String Ş,String ΛΗ){if(Ş=="") return false;float Ƣ=-1;bool ΗЊ=false;if(Ş.ToUpper()==ΛΗ) ΗЊ=true;else if(!float.TryParse(Ş,out Ƣ)) return false;
else Ƣ=Math.Max(0,Ƣ);if(Λμ) Ƣ=(float) Math.Round(Ƣ);ю(ref value,Ƣ,ψ,ΗЊ,false);return true;}void ю(ref float value,float Ǭ,ώ ψ,bool Αφ){ю(ref value,value+Ǭ,ψ,Αφ,true);}void ю(ref float value,float Ɵ,ώ ψ,bool Αφ,bool ΜΙ){float ȁ=0,Ȁ=0;if(ψ==ώ.ǋ){ȁ=0.5f;Ȁ=10f;}if(ψ==ώ.ΑϏ){ȁ=0.1f;Ȁ=1f;}
if(ψ==ώ.ƾ){ȁ=50f;Ȁ=100f;}if(ψ==ώ.ΓЎ){ȁ=5f;Ȁ=30f;}if(ψ==ώ.ΒΔ){ȁ=1f;Ȁ=25f;}if(ψ==ώ.ΒΓ){ȁ=10f;Ȁ=95f;}if(ψ==ώ.ΓЏ){ȁ=10f;Ȁ=90f;}if(ψ==ώ.ΚΦ){ȁ=10f;Ȁ=1800;}if(ψ==ώ.ΔΘ){ȁ=0.0f;Ȁ=0.75f;}if(Ɵ==-1&&Αφ){value=-1;return;}if(value==-1) Αφ=false;
bool Θΰ=Ɵ<ȁ||Ɵ>Ȁ;if(Θΰ&&ΜΙ){if(Ɵ<value) value=Ȁ;else if(Ɵ>value) value=ȁ;}else value=Ɵ;if(Θΰ&&Αφ) value=-1;else value=Math.Max(ȁ,Math.Min(value,Ȁ));value=(float) Math.Round(value,2);}void Γί(bool Λη){if(Λη) г=Math.Max(0,г-1);else г++;Γτ();Γσ();for(int k=0;k<ă.Count&&k<е.Length;k++) ΔΡ(ă[k],k<г);
Д=-1;Δή(true);}T İ<T>(T ς){int Β=Array.IndexOf(Enum.GetValues(ς.GetType()),ς);Β++;if(Β>=Ιέ(ς)) Β=0;return(T) Enum.GetValues(ς.GetType()).GetValue(Β);}int Ιέ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔΨ(){Αύ.Clear();for(int i=0;i<Ψ.Count;i++) Αύ.Add(Ψ[i]);Ψ.Clear();
ΒЂ=true;ΕΘ=new ά(Ρ);ΕΛ=new ά(Ζ);Ρ.ν=false;if(Ν==Λ.ί) Ζ.ν=false;for(int i=0;i<ǭ.Count;i++)
if(!ċ.Contains(ǭ.Keys.ElementAt(i)))
ċ.Add(ǭ.Keys.ElementAt(i));č=false;Ƴ=false;ƴ=false;ξ(Υ.ΖЅ);}void ΖБ(){if(č) Ρ=ΕΘ;if(Ƴ) Ζ=ΕΛ;if(ƴ){Ψ.Clear();for(int i=0;i<Αύ.Count;i++) Ψ.Add(Αύ[i]);}ΒЂ=false;Ѕ();ξ(Υ.Main);}void Κε(){if(!ΒЂ) return;if(Ϋ!=Α.ё){ΖБ();return;}if(!ΕΘ.ν) č=false;if(!ΕΛ.ν) Ƴ=false;if(Αύ.Count<=1) ƴ=false;
IMyShipConnector Р=й(MyShipConnectorStatus.Connectable);if(Р==null) Р=й(MyShipConnectorStatus.Connected);if(Р!=null){if(Math.Round(Ħ,2)<=0.20) ΔІ++;else ΔІ=0;if(ΔІ>=5){if(Ν==Λ.ί&&(Ρ.ν||č)&&Vector3.Distance(Ρ.Ε,Р.GetPosition())>5){Ζ.κ=Ω.WorldMatrix.Forward;Ζ.Ђ=Ω.WorldMatrix.Left;
Ζ.ι=Ω.WorldMatrix.Down;Ζ.ω=Ω.GetNaturalGravity();Ζ.Ε=Р.GetPosition();Ζ.ν=true;Ζ.с=Р.Position;}else{Ρ.κ=Ω.WorldMatrix.Forward;Ρ.Ђ=Ω.WorldMatrix.Left;Ρ.ι=Ω.WorldMatrix.Down;Ρ.ω=Ω.GetNaturalGravity();Ρ.Ε=Р.GetPosition();Ρ.ν=true;Ρ.с=Р.Position;}}}double ΕΞ=-1;if(Ψ.Count>0) ΕΞ=Vector3.Distance(δ,Ψ.Last().Ε);
double Λψ=Math.Max(1.5,Math.Pow(Ħ/100.0,2));double ΘΤ=Math.Max(Ħ*Λψ,2);ΘЌ=ΘΤ;if((ΕΞ==-1)||ΕΞ>=ΘΤ){ά ή=new ά(δ,Βϋ,ΑΟ,Αΰ,Ω.GetNaturalGravity());ή.ΙΖ(Č,ċ);Ψ.Add(ή);}}ά ѐ(){if(Ν!=Λ.ί) return ǧ!=null?ǧ:Ρ;return Ηζ;}void Ƃ(ά β,Σ Ї){Ηζ=β;if(ζ==Σ.И) ǔ=Ї;}bool ȩ(ά β,float υ,bool ΛА,out Vector3 έ)
{if(ΛА){Vector3I ΔБ=new Vector3I((int) β.с.X,(int) β.с.Y,(int) β.с.Z);IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(ΔБ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){έ=new Vector3();return false;}Vector3 Κό=ч(Ω,Ϊ.FatBlock.GetPosition()-δ);Vector3 Κϋ=ч(Ω,Ϊ.FatBlock.WorldMatrix.Forward);έ=β.Ε-Δέ(β.κ,β.ι*-1,Κό)-Δέ(β.κ,β.ι*-1,Κϋ)*υ;
return true;}else{έ=β.Ε;return true;}}Vector3 Đ(int Δύ,int Ņ,bool Λκ){if(!Λκ&&Ηώ) return Δφ;float Κϊ=((Ў-1f)/2f)-Δύ;float Λν=((Ю-1f)/2f)-Ņ;Δφ=Ζ.Ε+Ζ.Ђ*Κϊ*Ȉ+ć*-1*Λν*ȥ;Ηώ=true;return Δφ;}Vector3 Γω(Vector3 Λό,float ΔЂ){return Λό+(Ǘ*ΔЂ);}η Γο(){float Ь=-1;η ΰ=η.ȧ;
if(Ν!=Λ.ί){Ζω();if(ζ!=Σ.ш){Vector3 П=ő(Ǘ,ć*-1,δ-Ζ.Ε);if(Math.Abs(П.X)<=(float)(Ў*Ȉ)/2f&&Math.Abs(П.Y)<=(float)(Ю*ȥ)/2f){if(П.Z<=-1&&П.Z>=-ū*2) return η.ΑЁ;if(П.Z>-1&&П.Z<μ*2) return η.ȧ;}if(Į(Ζ.Ε,ref Ь)) ΰ=η.ȧ;}if(Ρ.ν){if(Į(Ρ.Ε,ref Ь)) ΰ=η.Ȧ;for(int i=0;i<Ψ.Count;i++)
if(Į(Ψ[i].Ε,ref Ь)) ΰ=η.đ;if(Vector3.Distance(δ,Ρ.Ε)<dockDist*μ) ΰ=η.Ȧ;if(й(MyShipConnectorStatus.Connectable)!=null||й(MyShipConnectorStatus.Connected)!=null)
ΰ=η.Ȧ;}}else{Vector3 έ=new Vector3();IMyShipConnector Ā=й(MyShipConnectorStatus.Connected);if(Ρ.ν){if(Į(Ρ.Ε,ref Ь)) ΰ=η.Ȧ;if(ȩ(Ρ,dockDist,true,out έ))
if(Į(έ,ref Ь)) ΰ=η.Ȧ;if(Ā!=null&&Vector3.Distance(Ā.GetPosition(),Ρ.Ε)<5)
return η.Γγ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ρ.Ε)>dockDist*μ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*μ)
if(Į(Ψ[i].Ε,ref Ь)) ΰ=η.đ;if(Ζ.ν){if(Į(Ζ.Ε,ref Ь)) ΰ=η.ȧ;if(ȩ(Ζ,dockDist,true,out έ))
if(Į(έ,ref Ь)) ΰ=η.ȧ;if(Ā!=null&&Vector3.Distance(Ā.GetPosition(),Ζ.Ε)<5)
return η.Γδ;}}return ΰ;}bool Į(Vector3 έ,ref float Ь){float υ=Vector3.Distance(έ,δ);if(υ<Ь||Ь==-1){Ь=υ;return true;}return false;}void ΔΧ(){if(В>0){Κ="Setup error! Can't start";return;}if(Ν==Λ.ί){į();return;}Ζ.Ε=δ;Ζ.ω=Ω.GetNaturalGravity();Ζ.κ=Βϋ;Ζ.ι=ΑΟ;Ζ.Ђ=Αΰ;
Ǘ=Ǩ.WorldMatrix.Forward;ć=Ζ.ι;if(Ǘ==Ω.WorldMatrix.Down) ć=Ω.WorldMatrix.Backward;Ǌ(true,true);Φ(Α.Ȍ);ΔΦ();}void Ǌ(bool Ǻ,bool Λλ){if(ζ==Σ.ш&&!Ǻ) return;bool ΛΩ=Ǻ||ζ==Σ.Γρ||Ў!=ѕ||Ю!=є||ş!=Ƞ;if(ΛΩ){if(ζ!=Σ.ш){ζ=Σ.ΑЄ;Đ(Є,Ѐ,Λλ);Κ="Job changed, lost progress";}ş=Ƞ;
Ў=ѕ;Ю=є;Ѐ=0;Є=0;е="";Д=-1;ţ=0;Ģ=0;л=0;г=0;Δή(true);}}void ΔΚ(){к(δ,0);Ǡ(Č,true);}void ΔΦ(){Ȯ(щ.ΒΞ);ΔΚ();ǆ(Ć,false);ȏ(Εϋ);ζ=Σ.И;ΖΟ(true);ǔ=ζ;ξ(Υ.ǁ);ΚΒ();Βμ=true;ΒϏ=0;for(int i=ǌ.Count-1;i>=0;i--)
if(ƿ(ǌ[i],false)) ΒϏ++;if(ΒϏ>0) Κ="Started with damage";}void Ѕ(){if(ζ==Σ.И){ζ=Σ.ƀ;Κ="Job paused";}Φ(Α.ё);ǔ=ζ;ΒΡ(false,0,0,0,0);ΔΫ();ΖΚ(new Vector3(),false);ΖА();ΔΠ(ChargeMode.Auto);ΔΣ(false);ΖЇ(true);Ē(Α.ё);Œ(false,false,0,0);Ǡ(ϋ,false);Ǡ(ǎ,true);ȏ(true);ΓΘ=false;ΑІ();
ΑΞ=false;ΔΜ();Βμ=false;Αξ=false;Αχ=false;if(Ϗ!=Υ.Main&&Ϗ!=Υ.ΑЂ&&Ϗ!=Υ.Γΰ&&Ϗ!=Υ.ΒΣ)
ξ(Υ.Main);}void į(){η Я=Γο();if(Ν==Λ.ί){if(!Ζ.ν||!Ρ.ν) return;ΔΦ();bool ΔЏ=Vector3.Distance(δ,Ρ.Ε)<Vector3.Distance(δ,Ζ.Ε);if(ƨ==Σ.ų) ΔЏ=true;if(ƨ==Σ.Ŵ) ΔЏ=false;if(ΔЏ){Ƃ(Ρ,Σ.ų);switch(Я){case η.Γγ:Φ(Α.Ķ);break;case η.đ:Φ(Α.ц);break;case η.Ȧ:Φ(Α.ȋ);break;default:Φ(Α.ǵ);break;
}}else{Ƃ(Ζ,Σ.Ŵ);switch(Я){case η.Γδ:Φ(Α.Ķ);break;case η.ȧ:Φ(Α.ȋ);break;case η.đ:Φ(Α.ц);break;default:Φ(Α.ǵ);break;}}}else{if(ζ!=Σ.ƀ&&ζ!=Σ.ΑЄ) return;bool ΜΒ=ζ==Σ.ΑЄ;ΔΦ();bool ΕΕ=ǀ(false)&&Ρ.ν;switch(Я){case η.ȧ:Φ(ΕΕ?Α.ı:Α.Ȍ);break;case η.đ:Φ(ΕΕ?Α.Й:Α.ц);break;case η.Ȧ:Φ(ΕΕ?Α.ȋ:Α.Ȱ);break;
case η.ΑЁ:{if(Ģ!=г||ΜΒ) Φ(Α.ȭ);else Φ(Α.ſ);}break;default:break;}}}void Ŏ(){if(ζ==Σ.ш&&!Ρ.ν) return;if(Ν==Λ.ί&&(!Ζ.ν||!Ρ.ν)) return;Κ="Move to job";η Я=Γο();if(Ν==Λ.ί){Ƃ(Ζ,Σ.Ŵ);switch(Я){case η.ȧ:Φ(Α.ȋ);break;case η.đ:Φ(Α.ц);break;case η.Γδ:return;default:Φ(Α.ǵ);break;
}Ē(Α.Ķ);}else{switch(Я){case η.ȧ:Φ(Α.ц);break;case η.đ:Φ(Α.ц);break;case η.Ȧ:Φ(Α.Ȱ);break;case η.ΑЁ:Φ(Α.ȭ);break;default:break;}if(ζ==Σ.ш) Ē(Α.ц);else Ē(Α.ĭ);ΓΘ=true;}ΔΚ();ξ(Υ.ΔΪ);ǆ(Ć,false);ǔ=Σ.ΒΖ;}void ΑЌ(){if(!Ρ.ν) return;Κ="Move home";η Я=Γο();if(Ν==Λ.ί){Ƃ(Ρ,Σ.ų);switch(Я)
{case η.đ:Φ(Α.Й);break;case η.Ȧ:Φ(Α.ȋ);break;case η.Γγ:return;default:Φ(Α.ǵ);break;}Ē(Α.Ķ);}else{if(й(MyShipConnectorStatus.Connected)!=null) return;if(й(MyShipConnectorStatus.Connectable)!=null){Φ(Α.ō);Ē(Α.Ȱ);return;}switch(Я){case η.ȧ:Φ(Α.ı);break;case η.đ:Φ(Α.Й);break;case η.Ȧ:Φ(Α.Й);break;
case η.ΑЁ:Φ(Α.Ȭ);break;default:break;}Ē(Α.Ȱ);}ΔΚ();ξ(Υ.ΔΪ);ǆ(Ć,false);ǔ=Σ.ΒΕ;}void Φ(Α Εβ){if(Εβ==Α.ё) Αρ=Α.ё;if(Αρ!=Α.ё&&Ϋ==Αρ&&Εβ!=Αρ){Ѕ();return;}Е=true;Ϋ=Εβ;}void Ē(Α Ї){this.Αρ=Ї;}Ǵ ΜΞ(int Αέ,bool ȃ){if(ȃ){ǫ=null;Є=0;Ѐ=0;}if(Ƞ==ђ.ǉ){int ť=Αέ+1;
Ѐ=(int) Math.Floor(К(Αέ,Ў));if(Ѐ%2==0) Є=Αέ-(Ѐ*Ў);else Є=Ў-1-(Αέ-(Ѐ*Ў));if(Ѐ>=Ю) return Ǵ.ΑЀ;else return Ǵ.ΑЎ;}else if(Ƞ==ђ.ΖΡ){if(ǫ==null) ǫ=new int[]{0,-1,0,0};int ΑΪ=(int) Math.Ceiling(Ў/2f);int ΑΩ=(int) Math.Ceiling(Ю/2f);int ΕΓ=(int) Math.Floor(Ў/2f);
int ΕΒ=(int) Math.Floor(Ю/2f);int Αή=0;while(ǫ[2]<Math.Pow(Math.Max(Ў,Ю),2)){if(Αή>200) return Ǵ.Ιή;Αή++;ǫ[2]++;if(-ΑΪ<Є&&Є<=ΕΓ&&-ΑΩ<Ѐ&&Ѐ<=ΕΒ){if(ǫ[3]==Αέ){this.Є=Є-1+ΑΪ;this.Ѐ=Ѐ-1+ΑΩ;return Ǵ.ΑЎ;}ǫ[3]++;}if(Є==Ѐ||(Є<0&&Є==-Ѐ)||(Є>0&&Є==1-Ѐ))
{int ΛϏ=ǫ[0];ǫ[0]=-ǫ[1];ǫ[1]=ΛϏ;}Є+=ǫ[0];Ѐ+=ǫ[1];}}return Ǵ.ΑЀ;}void Κγ(){if(Ϋ==Α.Ȍ){if(Е){Ж=0;if(Ģ!=г) ţ=0;Ģ=г;}if(Ж==0){int ΑΙ=ΙΝ(г);if(ΑΙ<0){if(!ΑЅ()){Φ(Α.Ȭ);return;}if(!Βθ||(ΑΙ==-2)!=Βη){Βθ=true;Βη=ΑΙ==-2;Ηγ=!Βη?δ:Ζ.Ε-Ǘ*μ+
Ζ.Ђ*(Ў*Ȉ/2+ΓΙ/2+μ/2);}к(Ηγ,10);Κ=Βη?"Waiting for a free hole":"Waiting for the controller";return;}if(Βθ) Κ="";Βθ=false;if(ΑΙ!=г){г=ΑΙ;ţ=0;Ģ=г;}Ǵ ΰ=ΚΛ(г);if(ΰ==Ǵ.ΑЀ){ζ=Σ.Γρ;Κ="Job done";if(Ň&&Ρ.ν){Φ(Α.ı);Ē(Α.Ȱ);ǔ=Σ.ΒΕ;}else{Φ(Α.Ź);Ē(Α.ĭ);ǔ=Σ.ΒΖ;}
return;}if(ΰ==Ǵ.ΑЎ){Ж=1;Ǡ(ϋ,true);ј=Đ(Є,Ѐ,true);к(ј,10);н(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}}else{if(Д>=0&&(Ζψ()?Ȕ>=0&&Ȕ!=Д:ΓЀ(Д,true))){Ж=0;return;}if(д<wpReachedDist){Φ(Α.ſ);return;}}}if(Ϋ==Α.ſ){if(Е){Ǡ(ϋ,true);ȏ(false);ј=Đ(Є,Ѐ,false);к(Γω(ј,0),0);н(Ζ.ι,Ζ.κ,Ζ.Ђ,false);
Ж=1;ї=0;х=0;Βΰ=0;ΕΠ=-1;ū=р;ΕΙ=true;}if(!ΑЅ()){Φ(Α.Ȭ);return;}if(ǀ(true)){Θή=Ď("","ORE",Щ.Ŋ);if((ж==Г.Γς||ж==Г.ΑЇ||
ж==Г.ΓЂ||ж==Г.ΑБ)&&Ν!=Λ.ǲ)
Φ(Α.Ÿ);else if((ж==Г.ΖΧ||ж==Г.Γλ)&&Ν!=Λ.ǲ)
Φ(Α.Δί);else
Φ(Α.Ȭ);return;}л=Vector3.Distance(δ,ј);if(л>ţ){ţ=л;ΕΙ=false;}if(Ν==Λ.ǲ&&Ζδ()==MyDetectedEntityType.SmallGrid)
х+=2;else х-=2;х=Math.Max(100,Math.Min(400,х));if(Ж>0&&Ж<х){if(л>ї){if(х>150) ї=л;else ї=(float) Math.Ceiling(л);Ж=1;}else Ж++;}else{if(Ж>0){Κ="Ship stuck! Retrying";ї=л;Ж=0;Œ(false,true,0,μ*sensorRange);}к(Γω(ј,Math.Max(0,ї-μ)),Γό(false));if(д<=wpReachedDist/2){Ж=1;ї=0;}
return;}Œ(false,true,μ*sensorRange,0);Vector3 Κπ=ј+Ǘ*л;bool ΗЎ=false;if(Vector3.Distance(Κπ,δ)>0.3f){Vector3 ΚЀ=ј+Ǘ*(л+0.1f);к(ΚЀ,4);ΗЎ=true;}else{float Ȅ=Γό(true);Vector3 Ηκ=Γω(ј,Math.Max(р+1,л+1));Ȏ(true,false,false,Ηκ,Ηκ-ј,Ȅ,Ȅ);}bool ΔЃ=false;if(Ǐ==ǝ.Γζ||Ǐ==ǝ.Γε)
{if(!ΗЎ){float Εε=0;foreach(IMyTerminalBlock Ȥ in ϋ)
Εε+=ΙΧ(Ȥ,"","",Ǐ==ǝ.Γε?new string[]{"STONE"}:null);if(Εε>ΕΠ||л<р||ΕΙ){Δκ=0;Βΰ=л;ū=(int)(Math.Max(ū,Βΰ)+μ/2);}else{ΔЃ=л-Βΰ>2&&Δκ>=20;Δκ++;}ΕΠ=Εε;}}else ΔЃ=л>=ū;if(Ģ!=г){Φ(Α.ȭ);л=0;return;}if(ΔЃ){г++;Φ(Α.ȭ);л=0;return;}}if(Ϋ==Α.ȭ||Ϋ==Α.Ȭ||Ϋ==Α.Ÿ)
{if(Е){ј=Đ(Є,Ѐ,false);н(Ζ.ι,Ζ.κ,Ζ.Ђ,false);Ǡ(ϋ,Ɛ);ȏ(false);ї=Vector3.Distance(δ,ј);Œ(false,true,0,μ*sensorRange);}к(ј,Γό(false));if(Vector3.Distance(δ,ј)>=ї+5){Ǡ(ϋ,false);ȏ(true);Κ="Can´t return!";}if(д<wpReachedDist){if(Ϋ==Α.ȭ&&ΓΘ) Φ(Α.Ź);if(Ϋ==Α.ȭ) Φ(Α.Ȍ);
if(Ϋ==Α.Ÿ) Φ(Α.ΑЋ);if(Ϋ==Α.Ȭ){if(Ρ.ν) Φ(Α.ı);else{Ѕ();Ŏ();Κ="Can´t return, no dock found";}}return;}}if(Ϋ==Α.Й||Ϋ==Α.ц){if(Ϋ==Α.ц&&ζ==Σ.И&&Ν!=Λ.ί){if(!ΑЅ()||ǀ(true)){Φ(Α.Й);return;}}bool Εξ=false;bool ΓΪ=false;bool ΗΝ=false;float ƈ=0;bool Θέ=false;ά ή=null;
if(Е){if(Ϋ==Α.Й||Ν==Λ.ί){if(Ν!=Λ.ί){if(ē(1)) Γβ();else{ǧ=null;ĉ=0;ΑΞ=!Γκ(1);}}ά β=ѐ();ρ=new ΔΙ();ρ.ε=β;ρ.ΑΜ=followPathDock*μ;ρ.ΕА=useDockDirectionDist*μ;ρ.ƈ=10;ρ.Ʈ.Add(β.Ε);Vector3 Ƈ=new Vector3();if(ȩ(β,dockDist*μ,true,out Ƈ))
ρ.Ʈ.Add(Ƈ);else ρ.ΑΜ*=1.5f;if(Ν==Λ.ί){if(β==Ρ) ρ.Βψ=Ζ.Ε;if(β==Ζ) ρ.Βψ=Ρ.Ε;ρ.ΔЌ=dockDist*μ*1.1f;}}else if(Ϋ==Α.ц){ρ=new ΔΙ();ρ.ε=Ζ;ρ.ΑΜ=followPathJob*μ;ρ.ΕА=useJobDirectionDist*μ;ρ.ƈ=10;ρ.Βψ=Ρ.Ε;ρ.ΔЌ=dockDist*μ*1.1f;ρ.Ʈ.Add(Ζ.Ε);if(ζ==Σ.ш){if(!Ρ.ν||Ψ.Count==0){Ѕ();return;}
float ΚЇ=Vector3.Distance(Ψ.First().Ε,Ρ.Ε);float ΚЈ=Vector3.Distance(Ψ.Last().Ε,Ρ.Ε);if(ΚЇ<ΚЈ) ρ.ε=Ψ.Last();else ρ.ε=Ψ.First();}}Αθ=new Vector3();Θέ=!Ιϋ(δ);Ǡ(ϋ,false);ȏ(true);м=-1;double ȓ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ρ.Βψ)<=ρ.ΔЌ) continue;
double υ=Vector3.Distance(Ψ[i].Ε,δ);if(ȓ==-1||υ<ȓ){м=i;ȓ=υ;}}Ȟ=Ικ(ρ.ε.Ε,м);Ŗ=null;}if(Ϋ==Α.Й&&Ν!=Λ.ί&&!Е&&!ǈ()){ά Δο=ѐ();ΑΞ=!Γκ(1);if(ѐ()!=Δο){Φ(Α.Й);return;}}if(Ϋ==Α.Й&&Ν!=Λ.ί&&!Е&&ē(1)&&Γβ()){Φ(Α.Й);return;}bool ΗЌ=false;if(Ϋ==Α.Й&&!Е&&м>=0&&м<Ψ.Count)
{Vector3 ΑΛ=Ψ[м].Ε+Ζη(м,Ȟ);int Εγ=м+Ȟ;Vector3 ť=Ȟ!=0&&Εγ>=0&&Εγ<Ψ.Count?Ψ[Εγ].Ε:ρ.ε.Ε;if(Ιυ(ΑΛ,ť)){if(!Βρ){Βρ=true;Ηο=Ž(ΑΛ)?δ:ΑΛ;}к(Ηο,0);н(ρ.ε.ι,ρ.ε.κ,ρ.ε.Ђ,false);Κ="Waiting for dock clearance";return;}Βρ=false;ΗЌ=ē(1)&&(Ž(δ)||Ž(ť));}ΙΗ(Ψ,Ȟ,ρ.Ʈ,ρ.ΑΜ,Е,ref Ж);
for(int i=0;i<ρ.Ʈ.Count;i++){float υ=Vector3.Distance(δ,ρ.Ʈ[i]);if(υ<=ρ.ΑΜ) Εξ=true;if(υ<=ρ.ΕА) ΓΪ=true;}if(ΗЌ) ΓΪ=true;if(ΓΪ) ƈ=ρ.ƈ;float ΜΤ=Ŗ!=null?Ŗ.Ш:Ħ;float ΜΘ=(float) Math.Max(Ħ*0.1f*μ,wpReachedDist);if((д<ΜΘ)||Е){if(!Е) м+=Ȟ;if(Ȟ==0||м>Ψ.Count-1||м<0)
Εξ=true;else{ΖΒ=Ȟ>0?Ψ.Count-1-м:м;ή=Ψ[м];Ŗ=ή;if(м>=1&&м<Ψ.Count-1)
Αθ=ή.Ε-Ψ[м-Ȟ].Ε;else Ŗ=null;Ѓ=ή.Ε+Ζη(м,Ȟ);ΗΝ=true;}}if(ΓΪ)
н(ρ.ε.ι,ρ.ε.κ,ρ.ε.Ђ,false);else if(Θέ)
Γα(ρ.ε.ι,10,true);else if(ΗΝ&&ή!=null){if(Ȟ>0) н(ή.ι,ή.κ,ή.Ђ,90,false);else н(ή.ι,-ή.κ,-ή.Ђ,90,false);}Ȏ(true,false,true,Ѓ,Αθ,Ŗ==null?0:Ŗ.Ш,ƈ);if(Εξ){ΖΒ=0;if(Ϋ==Α.Й&&!ΙΛ()){к(Ѓ,0);return;}if(Ϋ==Α.Й&&ΑΞ){к(Ѓ,0);Κ="Waiting for a free dock";return;}
if(Ϋ==Α.Й||Ν==Λ.ί){Φ(Α.ȋ);return;}if(Ϋ==Α.ц&&ΓΘ){Φ(Α.Ź);return;}if(Ϋ==Α.ц){Φ(Α.Ȍ);return;}}}if(Ϋ==Α.ȋ||Ϋ==Α.ǅ){if(Ν!=Λ.ί&&ǈ()){if(!ē(1)){Φ(Α.Й);return;}}else if(Ν!=Λ.ί){ά Δο=ѐ();if(!Γκ(2)||ѐ()!=Δο){Φ(Α.Й);return;}}ά β=ѐ();if(Е){if(!ȩ(β,dockDist*μ,true,out Ѓ))
{Ȯ(щ.ƺ);Ѕ();return;}к(Ѓ,0);Γα(β.ι,90,true);}if(д<followPathDock*μ&&д!=-1){к(Ѓ,10);н(β.ι,β.κ,β.Ђ,false);}if(й(MyShipConnectorStatus.Connectable)!=null||й(MyShipConnectorStatus.Connected)!=null){Φ(Α.ō);return;}if(д<wpReachedDist/2&&д!=-1){Φ(Α.ŉ);return;}}if(Ϋ==Α.ŉ||Ϋ==Α.ĭ)
{if(Е){if(Ϋ==Α.ŉ){ά β=ѐ();if(!ȩ(β,dockDist*μ,true,out Ѓ)){Ȯ(щ.ƺ);Ѕ();return;}Ȏ(true,true,false,Ѓ,0);н(β.ι,β.κ,β.Ђ,10,false);}if(Ϋ==Α.ĭ){н(Ζ.ι,Ζ.κ,Ζ.Ђ,0.5f,false);Ѓ=Ζ.Ε;Ȏ(true,true,false,Ѓ,0);}}if(ΒЁ){ΒΡ(false,0,0,0,0);if(Ϋ==Α.ŉ) Φ(Α.ō);if(Ϋ==Α.ĭ) Ѕ();return;
}}if(Ϋ==Α.ō){if(й(MyShipConnectorStatus.Connected)!=null){if(Ν==Λ.ί) Φ(Α.Ķ);else Φ(Α.Ȱ);return;}ά β=ѐ();if(Е){х=0;ΚЉ=DateTime.Now;Ж=0;н(β.ι,β.κ,β.Ђ,false);}Vector3I ΔБ=new Vector3I((int) β.с.X,(int) β.с.Y,(int) β.с.Z);IMySlimBlock Ηΰ=Me.CubeGrid.GetCubeBlock(ΔБ);float Ης=dockingSpeed;
float ΛΣ=dockingSpeed*5;float ΗΛ=Math.Max(1.5f,Math.Min(5f,μ*0.15f));if(!ȩ(β,0,true,out Ѓ)||!ȩ(β,ΗΛ,true,out Αθ)||
Ηΰ==null||!Ηΰ.FatBlock.IsFunctional){Ȯ(щ.ƺ);Ѕ();return;}if(х==1||(Vector3.Distance(δ,Ѓ)<=ΗΛ*1.1f&&!Е))
х=1;else{Vector3 ΛЈ=ч(Ω,Αθ-δ);Vector3 ΔА=ч(Ω,Ω.GetNaturalGravity());float Λξ=ΑЃ(ΛЈ,ΔА,null);Ης=Math.Min(ΛΣ,Λξ);}Ȏ(true,false,false,Ѓ,Ѓ-δ,dockingSpeed,Ης);if(Е) ї=(float) д;IMyShipConnector Δώ=й(MyShipConnectorStatus.Connectable);if(Δώ!=null){Ȏ(false,false,false,Ѓ,0);
if(Ж>0) Ж=0;Ж--;if(Ж<-5){Δώ.Connect();if(Δώ.Status==MyShipConnectorStatus.Connected){if(Ν==Λ.ί) Φ(Α.Ķ);else Φ(Α.Ȱ);ΔΫ();ǆ(Ć,true);return;}}}else{float Ηξ=(float) Math.Round(д,1);if(Ηξ<ї){Ж=-1;ї=Ηξ;}else Ж++;if(Ж>20){Φ(Α.ǅ);return;}}}if(Ϋ==Α.Ȱ||Ϋ==Α.Ķ||
Ϋ==Α.ΒΪ||Ϋ==Α.ΑЉ||Ϋ==Α.Ō){bool Δθ=false,Δι=false;if(Ν==Λ.ί){if(ѐ()==Ρ) Δθ=true;else if(ѐ()==Ζ) Δι=true;}if(Е){Αξ=false;if(й(MyShipConnectorStatus.Connected)==null){Φ(Α.ǵ);return;}ΔΫ();if(Δθ) ΒΧ(Љ.ǹ);if(Δι) ΒΧ(Њ.ǹ);ů=false;Ĕ=false;Ĝ=false;Αω=false;}if(й(MyShipConnectorStatus.Connected)==null){Ѕ();Ȯ(щ.Ζσ);return;}
if(ζ!=Σ.И||Ǖ==-1||ȑ==ў.None) Ĕ=true;else if(Ȳ>=100f) Ĕ=true;else if(Ȳ<=99f) Ĕ=false;if(ζ!=Σ.И||ǖ==-1||Ǥ.Count==0) Ĝ=true;else if(ĝ>=100f) Ĝ=true;else if(ĝ<=99) Ĝ=false;if(ζ!=Σ.И||т==-1||ń.Count==0) Αω=true;else Αω=Ű>=т;Ƅ ѝ=null;if(Δθ) ѝ=Љ;
if(Δι) ѝ=Њ;if(ѝ!=null&&(ѝ.Т==τ.ΒΚ||ѝ.Т==τ.ΒΙ)) Ĕ=true;if(ѝ!=null&&(ѝ.Т==τ.ΔΑ)) if(!ů) Ĕ=false;if(ѝ!=null&&(ѝ.Т==τ.ΔΕ||ѝ.Т==τ.ΔΓ)) Ĝ=true;if(ѝ!=null&&(ѝ.Т==τ.ΔΔ)) if(!ů) Ĝ=false;if(Ğ){ChargeMode ΗΩ=Ĕ?ChargeMode.Auto:ChargeMode.Recharge;if(ѝ!=null&&(ѝ.Т==τ.ΒΙ||ѝ.Т==τ.ΒΚ))
ΗΩ=ChargeMode.Discharge;ΔΠ(ΗΩ);ΔΣ(!Ĝ);}if(!ů){if(Ν==Λ.ί)
ů=ζ!=Σ.И||ΙΜ(Е,true)||Αχ;else
ů=ζ!=Σ.И||Ζτ();}else{if(!Ĕ) Φ(Α.Ō);if(!Ĝ) Φ(Α.ΑЉ);if(!Αω) Φ(Α.ΒΪ);Е=false;}if(ů&&Ĕ&&Ĝ&&Αω){ΔΠ(ChargeMode.Auto);ΔΣ(false);if(ζ==Σ.И){if(Ν==Λ.ί){if(ѐ()==Ρ) ΒΧ(Љ.Ǿ);else if(ѐ()==Ζ) ΒΧ(Њ.Ǿ);if(ѐ()==Ρ) Ƃ(Ζ,Σ.Ŵ);else Ƃ(Ρ,Σ.ų);}}Φ(Α.ǵ);return;}}if(Ϋ==Α.ǵ){if(Αό){if(!ΖΤ()) return;
Αό=false;Φ(Α.ǵ);return;}if(Е){if(!ΖΤ()){Αό=true;return;}IMyShipConnector Ā=й(MyShipConnectorStatus.Connected);if(Ā==null){Φ(Α.ц);return;}IMyShipConnector λ=Ā.OtherConnector;ΖІ(Ā,false);ǆ(Ć,false);ά β=null;ά Ǒ=Ν==Λ.ί?Ρ:ѐ();if(Vector3.Distance(Ā.GetPosition(),Ǒ.Ε)<5f&&Ρ.ν) β=Ǒ;
if(Vector3.Distance(Ā.GetPosition(),Ζ.Ε)<5f&&Ζ.ν) β=Ζ;if(β!=null){if(!ȩ(β,dockDist*μ,true,out Ѓ)){Ȯ(щ.ƺ);Ѕ();return;}к(Ѓ,5);н(β.ι,β.κ,β.Ђ,false);}else к(δ+λ.WorldMatrix.Forward*dockDist*μ,5);if(ζ==Σ.И) Ȯ(щ.ΒΞ);}if(д<wpReachedDist){Ǡ(ǎ,true);Φ(Α.ц);return;}}if(Ϋ==Α.Ź)
{if(Е){ȏ(true);Ǡ(ϋ,false);Ѓ=Ζ.Ε;к(Ѓ,20);н(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}if(д<wpReachedDist/2){Φ(Α.ĭ);return;}}if(Ϋ==Α.ı){if(Е){ȏ(true);Ǡ(ϋ,false);int Ь=-1;double ȓ=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,δ);if(ȓ==-1||υ<ȓ){Ь=i;ȓ=υ;}}
if(Ь==-1){Φ(Α.Й);return;}Ѓ=Ψ[Ь].Ε;к(Ѓ,10);н(Ζ.ι,Ζ.κ,Ζ.Ђ,false);}if(д<wpReachedDist){Φ(Α.Й);return;}}if(Ϋ==Α.ΑЋ){bool ύ=false;if(Е){ȏ(true);if((ж==Г.Γς||ж==Г.ΑЇ)&&ΒΒ()&&
Ŷ(Ǘ,Ω.GetNaturalGravity())<25&&Ў>=2&&Ю>=2){Vector3 ΑΠ=δ;if(Є>0&&Ѐ<Ю-1) ΑΠ=Đ(Є-1,Ѐ+1,true);else if(Є<Ў-1&&Ѐ<Ю-1) ΑΠ=Đ(Є+1,Ѐ+1,true);else if(Є<Ў-1&&Ѐ>0) ΑΠ=Đ(Є+1,Ѐ-1,true);else if(Є>0&&Ѐ>0) ΑΠ=Đ(Є-1,Ѐ-1,true);else ύ=true;
if(!ύ) к(ΑΠ,10);}else ύ=true;}if(д<wpReachedDist/2) ύ=true;if(ύ){Φ(Α.ΒΩ);return;}}if(Ϋ==Α.Δί||Ϋ==Α.ΒΩ){if(Е){Ȏ(true,true,false,δ,0);Ǡ(ϋ,false);ȏ(true);Ж=-1;х=ж==Г.ΓЂ||ж==Г.ΑБ?0:-1;}bool Ś=!ΑЅ();int ΓΣ=Ď("STONE","ORE",Щ.Ŋ);if(ж==Г.Γλ||ж==Г.ΑБ||ж==Г.ΑЇ)
ΓΣ+=Ď("ICE","ORE",Щ.Ŋ);bool ΛΒ=ΓΣ>0;bool ΔЋ=false;if(х>=0){float ƅ=(float) Math.Sin(ΖΩ(х))*Ȉ/3f;float Κς=(float) Math.Cos(ΖΩ(х))*ȥ/3f;Vector3 Κω=Đ(Є,Ѐ,true)+Δέ(Ǘ,ć*-1,new Vector3(ƅ,Κς,0));к(Κω,0.3f);if(д<Math.Min(Ȉ,ȥ)/10f) х+=5f;if(х>=360) х=0;}if(Ж==-1||ΓΣ<Ж){Ж=ΓΣ;ї=0;}
else{ї++;if(ї>50) ΔЋ=true;}if(!ΛΒ||Ś||ΔЋ){if(!Ś){int ΛЍ=Ď("","ORE",Щ.Ŋ);if(ǀ(true)) Ś=true;else if(100-(К(ΛЍ,Θή)*100)<minEjection) Ś=true;else Ȯ(щ.ΒΞ);}if(ΔЋ&&Ś) Κ="Ejection failed";if(Ϋ==Α.ΒΩ){if(Ś){if(Ρ.ν) Φ(Α.ı);else{Ѕ();Ŏ();Κ="Can´t return, no dock found";}}
else Φ(Α.Ȍ);}else if(Ś) Φ(Α.Ȭ);else Φ(Α.ſ);return;}}Е=false;}void ΙΗ(List<ά>ň,int π,List<Vector3>Ατ,float Ȇ,bool ȃ,ref int Ƿ){if(ȃ){for(int i=0;i<ň.Count;i++) ň[i].Ш=0;Ƿ=-1;return;}if(π==0) return;int Ǭ=π*-1;if(Ƿ==-1) Ƿ=Ǭ>0?1:ň.Count-2;int Αή=0;while(Ƿ>=1&&Ƿ<ň.Count-1)
{if(Αή>50) return;Αή++;try{if((Ǭ<0&&Ƿ>=1)||(Ǭ>0&&Ƿ<=ň.Count-2)){ά ς=ň[Ƿ];bool ΘΩ=false;for(int j=0;j<Ατ.Count;j++)
if(Vector3.Distance(ς.Ε,Ατ[j])<=Ȇ){ΘΩ=true;break;}if(!ΘΩ){ά ΓΓ=ň[Ƿ-Ǭ];ά ť=ň[Ƿ+Ǭ];Vector3 ΛЉ=ς.Ε-ť.Ε;Vector3 ΕЈ=ΓΓ.Ε-ς.Ε;Vector3 Λι=ς.Ε+Vector3.Normalize(ΛЉ)*ΕЈ.Length();Vector3 ΚЅ=ΓΓ.Ε-Λι;Vector3 ΒЇ=ő(π>0?ς.κ:ς.κ*-1,ς.ι*-1,ΚЅ);Vector3 ΛΡ=ő(π>0?ς.κ:ς.κ*-1,ς.ι*-1,ΕЈ);
Vector3 ΒЉ=ő(π>0?ς.κ:ς.κ*-1,ς.ι*-1,ς.ω);ς.Ш=(float) Math.Sqrt(Math.Pow(ΓΓ.Ш,2)+Math.Pow(ΑЃ(-ΛΡ,ΒЉ,ς),2));for(int j=0;j<Ατ.Count;j++)
if(Vector3.Distance(ΓΓ.Ε,Ατ[j])<=Ȇ){Vector3 ΒЊ=ő(π>0?ς.κ:ς.κ*-1,ς.ι*-1,Ατ[j]-ς.Ε);float Λώ=ΑЃ(-ΒЊ,ΒЉ,ς);ς.Ш=Math.Min(ς.Ш,Λώ)/2f;}if(ΒЇ.Length()==0) ΒЇ=new Vector3(0,0,1);Vector3 ΛΠ=ő(ς.κ,ς.ι*-1,ς.ω);float ΛЃ=ƽ(-ΒЇ,ΛΠ,ς);float ΒΫ=К(ΛЃ,ŭ);float ΕЅ=(float) Math.Sqrt(К(ΒЇ.Length()*1.0f,0.5f*ΒΫ));
ς.Ш=Math.Min(ς.Ш,(ΕЈ.Length()/ΕЅ)*Ȑ);}}}catch{return;}Ƿ+=Ǭ;}Ƿ=-1;}void Δή(bool ȃ){if(ȃ){Αί=0;return;}if(Ν==Λ.ί) return;float ύ=г*Math.Max(1,р);if(Ģ==г) ύ+=Math.Min(р,л);float ΛЌ=Ў*Ю*Math.Max(1,р);Αί=Math.Max(Αί,(float) Math.Min(ύ/ΛЌ*100.0,100));}MyDetectedEntityType Ζδ()
{try{if(Ȫ(ϊ,true)&&!ϊ.LastDetectedEntity.IsEmpty()) return ϊ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γό(bool Ț){if(Ν==Λ.ǲ&&Ζδ()==MyDetectedEntityType.None&&!ƿ(ϊ,true))
return fastSpeed;else return Ț?Ƹ:Ʒ;}void ΚΩ(){float ΘΠ=0,Ηθ=0,Ş=0,Εζ=0;for(int i=0;i<ĸ.Count;i++){IMyBatteryBlock ΑΖ=ĸ[i];if(!Ȫ(ΑΖ,true)) continue;ΘΠ+=ΑΖ.MaxStoredPower;Ηθ+=ΑΖ.CurrentStoredPower;Ş+=ΑΖ.CurrentInput;Εζ+=ΑΖ.CurrentOutput;}Ȳ=(float) Math.Round(К(Ηθ,ΘΠ)*100,1);
if(Ş>=Εζ) ȑ=ў.Ō;else ȑ=ў.ΖΪ;if(Ş==0&&Εζ==0||Ȳ==100.0) ȑ=ў.ё;if(ĸ.Count==0) ȑ=ў.None;}void Κΰ(){float Ηχ=0;for(int i=0;i<Ǥ.Count;i++){IMyGasTank Θϋ=Ǥ[i];if(!Ȫ(Θϋ,true)) continue;Ηχ+=(float) Θϋ.FilledRatio;}ĝ=К(Ηχ,Ǥ.Count)*100f;}void Κμ(){Ű=0;try{for(int i=0;i<ń.Count;i++)
{IMyReactor Εο=ń[i];if(!Ȫ(Εο,true)) continue;List<MyInventoryItem>Ѝ=new List<MyInventoryItem>();Εο.GetInventory(0).GetItems(Ѝ);float Ќ=0;for(int j=0;j<Ѝ.Count;j++){MyInventoryItem Ё=Ѝ[j];if(Ё.Type.SubtypeId.ToUpper()=="URANIUM"&&Ё.Type.TypeId.ToUpper().Contains("_INGOT"))
Ќ+=(float) Ё.Amount;}if(Ќ<Ű||i==0){Ű=Ќ;ΕΨ=Εο.CustomName;}}}catch(Exception e){ġ=e;}}void ΚΪ(){ȝ=0;ȕ=0;try{for(int i=0;i<ƌ.Count;i++){IMyTerminalBlock Ϊ=ƌ[i];if(!Ȫ(Ϊ,true)) continue;ȕ+=(float) Ϊ.GetInventory(0).CurrentVolume;ȝ+=(float) Ϊ.GetInventory(0).MaxVolume;
}ΒІ=(float) Math.Min(Math.Round(К(ȕ,ȝ)*100,1),100.0);}catch(Exception e){ġ=e;}}void Κα(){try{Ǽ.Clear();for(int i=0;i<ƌ.Count;i++){IMyTerminalBlock Ϊ=ƌ[i];if(!Ȫ(Ϊ,true)) continue;ΖΙ(Ϊ,Щ.ż);}if(ж!=Г.ΓБ){for(int i=0;i<ŕ.Count;i++){IMyTerminalBlock Ϊ=ŕ[i];if(!Ȫ(Ϊ,true)) continue;
ΖΙ(Ϊ,Щ.Ιω);}}ΚΞ(Ǽ);}catch(Exception e){ġ=e;}}void ΙΚ(){if(Βμ){if(Ζγ().Count>ΒϏ){Βμ=false;if(њ!=я.ƾ){Ѕ();if(њ==я.Γχ) Ŏ();if(њ==я.ΒΝ)
if(Ρ.ν) ΑЌ();else Ŏ();}Κ="Damage detected";}}}bool ΑЅ(){if(!ΒЀ) return true;if(ζ==Σ.И){if(Ǖ>0&&ȑ!=ў.None)
if(Ȳ<=Ǖ){Κ="Low energy! Move home";return false;}if(т>0&&ń.Count>0)
if(Ű<=т){Κ="Low fuel: "+ΕΨ;return false;}if(ǖ>0&&Ǥ.Count>0)
if(ĝ<=ǖ){Κ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Ζγ(){List<IMyTerminalBlock>ŗ=new List<IMyTerminalBlock>();for(int i=0;i<ǌ.Count;i++){IMyTerminalBlock Ϊ=ǌ[i];if(ƿ(Ϊ,false)) ŗ.Add(Ϊ);}return ŗ;}bool ƿ(IMyTerminalBlock Ϊ,bool Εύ){return(!Ȫ(Ϊ,Εύ)||!Ϊ.IsFunctional);
}bool Ȫ(IMyTerminalBlock Ϊ,bool Εύ){if(Ϊ==null) return false;try{IMyCubeBlock Ηύ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(Εύ) return Ηύ==Ϊ;else return Ηύ.GetType()==Ϊ.GetType();}catch{return false;}}float ΙΧ(IMyTerminalBlock Ϊ,String ǜ,String Θς,String[]Ș){float ā=0;for(int i=0;i<Ϊ.InventoryCount;i++)
{IMyInventory ƕ=Ϊ.GetInventory(i);List<MyInventoryItem>Ѝ=new List<MyInventoryItem>();ƕ.GetItems(Ѝ);foreach(MyInventoryItem Ё in Ѝ){if(Ș!=null&&(Ș.Contains(Ё.Type.TypeId.ToUpper())||Ș.Contains(Ё.Type.SubtypeId.ToUpper())))
continue;if((ǜ==""||Ё.Type.TypeId.ToUpper()==ǜ)&&(Θς==""||Ё.Type.SubtypeId.ToUpper()==Θς))
ā+=(float) Ё.Amount;}}return ā;}о Ιΰ(String α,String ψ,Щ Я,bool ΚЁ){α=α.ToUpper();ψ=ψ.ToUpper();for(int i=0;i<Ǽ.Count;i++){о Ё=Ǽ[i];if(Ё.α.ToUpper()==α&&Ё.ψ.ToUpper()==ψ&&(Ё.ƚ==Я||Я==Щ.Ŋ))
return Ё;}о ΰ=null;if(ΚЁ){ΰ=new о(α,ψ,0,Я);Ǽ.Add(ΰ);}return ΰ;}int Ď(String α,String ψ,Щ Я){return Ď(α,ψ,Я,null);}int Ď(String α,String ψ,Щ Я,String[]Ș){int ā=0;α=α.ToUpper();ψ=ψ.ToUpper();for(int i=0;i<Ǽ.Count;i++){о Ё=Ǽ[i];if(Ș!=null&&Ș.Contains(Ё.α.ToUpper())) continue;
if((α==""||Ё.α.ToUpper()==α)&&(ψ==""||Ё.ψ.ToUpper()==ψ)&&(Ё.ƚ==Я||Я==Щ.Ŋ))
ā+=Ё.Ќ;}return ā;}void ΖΙ(IMyTerminalBlock Ϊ,Щ Я){for(int i=0;i<Ϊ.InventoryCount;i++){List<MyInventoryItem>Ѝ=new List<MyInventoryItem>();Ϊ.GetInventory(i).GetItems(Ѝ);for(int j=0;j<Ѝ.Count;j++)
Ιΰ(Ѝ[j].Type.SubtypeId,Ѝ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Я,true).Ќ+=(int) Ѝ[j].Amount;}}void ΚΞ(List<о>Џ){for(int i=Џ.Count-1;i>0;i--)
for(int j=0;j<i;j++){о a=Џ[j];о b=Џ[j+1];if(a.Ќ<b.Ќ) Џ.Move(j,j+1);}}bool ǀ(bool ΜΠ){if(Ĭ&&Ν!=Λ.ί)
if(з!=-1&&ŭ>=з){Κ="Ship too heavy";return true;}if(ΒІ>=Ȝ||Αξ){Αξ=false;Κ="Ship is full";return true;}return false;}bool Ζτ(){String[]Ș=null;if(!ī) Ș=new string[]{"ICE"};if(Ν==Λ.ǃ) return Ď("","ORE",Щ.ż,Ș)==0;if(Ν==Λ.ǲ) return Ď("","COMPONENT",Щ.ż,Ș)==0;else return Ď("","",Щ.ż,Ș)==0;
}bool Ιό(){List<IMyCockpit>Δό=new List<IMyCockpit>();φ.GetBlocksOfType(Δό,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δό.Count;i++)
if(Δό[i].IsUnderControl) return true;return false;}bool ΙΜ(bool ȃ,bool ΓΛ){IMyShipConnector Р=й(MyShipConnectorStatus.Connected);if(Р==null) return false;if(Vector3.Distance(Ρ.Ε,Р.GetPosition())<5)
return ΖΥ(Љ,ȃ,ΓΛ);if(Vector3.Distance(Ζ.Ε,Р.GetPosition())<5)
return ΖΥ(Њ,ȃ,ΓΛ);return false;}bool ΖΥ(Ƅ ȳ,bool ȃ,bool ΓΛ){if(ȃ) ȳ.ΙЎ();ȳ.Κθ();bool ќ=false;String Ǚ="";switch(ȳ.Т){case τ.ΔΒ:{Ǚ="Waiting for command";ќ=false;break;}case τ.ΖЀ:{Ǚ="Waiting for cargo";ќ=ǀ(true);break;}case τ.ΖϏ:{Ǚ="Unloading";ќ=Ζτ();break;}case τ.ΔΗ:{ќ=true;break;}
case τ.ΔΑ:{Ǚ="Charging batteries";ќ=Ȳ>=100f;break;}case τ.ΒΚ:{Ǚ="Discharging batteries";ќ=Ȳ<=25f;break;}case τ.ΒΙ:{Ǚ="Discharging batteries";ќ=Ȳ<=0f;break;}case τ.ΔΔ:{Ǚ="Filling up hydrogen";ќ=ĝ>=100f;break;}case τ.ΔΕ:{Ǚ="Unloading hydrogen";ќ=ĝ<=25f;break;}
case τ.ΔΓ:{Ǚ="Unloading hydrogen";ќ=ĝ<=0f;break;}case τ.ΔΖ:{bool ΗЋ=Ιό();if(!ΗЋ) ȳ.Εκ=true;ќ=ȳ.Εκ&&ΗЋ;Ǚ="Waiting for passengers";break;}}if(!ќ) ȳ.Γι(true);if(ќ&&ȳ.Γώ()){ќ=ȳ.Γι(false);Ǚ="Undocking in: "+ΑЍ((int) Math.Max(0,ȳ.Ǹ-ȳ.Ľ));}if(ΓΛ) ΘЊ=Ǚ;return ќ;}void ΘА()
{if(!Ɩ) return;if(ϋ.Count<=1) return;float ΘЂ=0,ΘЁ=0;for(int i=0;i<ϋ.Count;i++){IMyTerminalBlock Ȥ=ϋ[i];if(ƿ(Ȥ,true)) continue;ΘЂ+=(float) Ȥ.GetInventory(0).MaxVolume;ΘЁ+=(float) Ȥ.GetInventory(0).CurrentVolume;}float ΗΞ=(float) Math.Round(К(ΘЁ,ΘЂ),5);for(int ŧ=0;ŧ<Math.Max(1,Math.Floor(ϋ.Count/10f));ŧ++)
{float ΕΗ=0,ΕΧ=0;float ΗЄ=0,ΘΝ=0;IMyTerminalBlock Ě=null,ľ=null;for(int i=0;i<ϋ.Count;i++){IMyTerminalBlock Ȥ=ϋ[i];if(ƿ(Ȥ,true)) continue;float Εή=(float) Ȥ.GetInventory(0).MaxVolume;float Βω=К((float) Ȥ.GetInventory(0).CurrentVolume,Εή);if(Ě==null||Βω>ΕΗ){Ě=Ȥ;ΕΗ=Βω;ΗЄ=Εή;}
if(ľ==null||Βω<ΕΧ){ľ=Ȥ;ΕΧ=Βω;ΘΝ=Εή;}}if(Ě==null||ľ==null||Ě==ľ) return;if(checkConveyorSystem&&!Ě.GetInventory(0).IsConnectedTo(ľ.GetInventory(0))){if(Δξ>20) Κ="Inventory balancing failed";else Δξ++;return;}Δξ=0;List<MyInventoryItem>Ѝ=new List<MyInventoryItem>();
Ě.GetInventory(0).GetItems(Ѝ);float Αϋ=0;if(Ѝ.Count==0) continue;MyInventoryItem Βϊ=Ѝ[0];String ΗЏ=Βϊ.Type.TypeId+Βϊ.Type.SubtypeId;if(!ΘЉ.TryGetValue(ΗЏ,out Αϋ)){if(ΙЂ(Ě.GetInventory(0),0,ľ.GetInventory(0),out Αϋ))
ΘЉ.Add(ΗЏ,Αϋ);else return;}float ΛЊ=((ΕΗ-ΗΞ)*ΗЄ/Αϋ);float ΛЋ=((ΗΞ-ΕΧ)*ΘΝ/Αϋ);int ΕЍ=(int) Math.Min(ΛЋ,ΛЊ);if(ΕЍ<=0) return;if((float) Βϊ.Amount<ΕЍ)
Ě.GetInventory(0).TransferItemTo(ľ.GetInventory(0),0,null,null,Βϊ.Amount);else
Ě.GetInventory(0).TransferItemTo(ľ.GetInventory(0),0,null,null,ΕЍ);}}bool ΙЂ(IMyInventory Αο,int Β,IMyInventory ΚЄ,out float ΘЈ){ΘЈ=0;float Κφ=(float) Αο.CurrentVolume;List<MyInventoryItem>Δπ=new List<MyInventoryItem>();Αο.GetItems(Δπ);float ΗΣ=0;for(int i=0;i<Δπ.Count;i++) ΗΣ+=(float) Δπ[i].Amount;
Αο.TransferItemTo(ΚЄ,Β,null,null,1);float ΘІ=Κφ-(float) Αο.CurrentVolume;List<MyInventoryItem>Δε=new List<MyInventoryItem>();Αο.GetItems(Δε);float ΗΗ=0;for(int i=0;i<Δε.Count;i++) ΗΗ+=(float) Δε[i].Amount;if(ΘІ==0f||!ΓЃ(0.9999,ΗΣ-ΗΗ,1.0001))
return false;ΘЈ=ΘІ;return true;}void Κί(){Vector3 ΑΦ=Ѓ+Ī;Vector3 ȣ=ΑΦ-δ;if(ȣ.Length()==0) ȣ=new Vector3(0,0,-1);Vector3 ΒЊ=ч(Ω,ȣ);Vector3 Θό=Vector3.Normalize(ΒЊ);Vector3 ƙ=ч(Ω,Ω.GetNaturalGravity());float Λω=Εθ>0?Math.Max(0,1-Ŷ(ȣ,Ηω)/5):0;float Ш=(float) Math.Min((ΕΫ>0?ΕΫ:1000f),Math.Max(ΑЃ(-ΒЊ,ƙ,null),Εθ*Λω));
if(!ΘЀ) Ш=0;if(Θμ) Ш=Math.Max(0,1-Βκ/Βά)*Ш;if(generalSpeedLimit>0) Ш=Math.Min(generalSpeedLimit,Ш);if(ƣ) Ш=0;if(Ǜ>=0) Ш=Math.Min(Ш,Ǜ);if(Θν) Ш*=(float) Math.Min(1,К(ȣ.Length(),wpReachedDist)/2);Vector3 ΘΛ=ч(Ω,Ω.GetShipVelocities().LinearVelocity);float Κρ=(float)(Math.Max(0,15-Ŷ(-Θό,-ΘΛ))/15)*0.85f+0.15f;
ΕЃ+=Math.Sign(Κρ-ΕЃ)/10f;Vector3 ĺ=Θό*Ш*ΕЃ-(ΘΛ);Vector3 Δμ=Γϋ(ĺ,null);if(ΓΥ&&д>0.1f){ĺ.X*=ΔΥ(ĺ.X,ref ΓΦ.X,1f,Δμ.X,20);ĺ.Y*=ΔΥ(ĺ.Y,ref ΓΦ.Y,1f,Δμ.Y,20);ĺ.Z*=ΔΥ(ĺ.Z,ref ΓΦ.Z,1f,Δμ.Z,20);}else ΓΦ=new Vector3(1,1,1);Αυ=ŭ*ĺ-ƙ*ŭ;ΖΚ(Αυ,ΓΥ);д=Vector3.Distance(δ,ΑΦ);
}float ΔΥ(float value,ref float Ї,float Ǭ,float ģ,float Εά){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ї)) Ї+=Math.Sign(Ї)*Ǭ;else Ї=value;if(value==0) Ї=1;float ΰ=Math.Abs(Ї);if(ΰ<Εά||ģ==0) return ΰ;Ї=Math.Min(Εά,Math.Max(-Εά,Ї));ΰ=Math.Abs(ģ);return ΰ;
}void ΗΕ(){float ǘ=90,ǰ=90,Ǫ=90;float Ȅ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ț,Ǧ,Αβ,ΕΤ;if(ΕΥ){Ț=Vector3.Normalize(Ѓ-δ);Αβ=ч(Ω,Ț);Ǧ=ч(Ω,ΕϏ);ǘ=Ŷ(Αβ,new Vector3(0,-1,0))-90;ǰ=ŋ(Ǧ,new Vector3(-1,0,0),Ǧ.Y);
Ǫ=ŋ(Αβ,new Vector3(-1,0,0),Αβ.Z);}else{Ț=ΕЀ;Ǧ=ч(Ω,ΕϏ);Αβ=ч(Ω,ΕЀ);ΕΤ=ч(Ω,Θύ);ǘ=ŋ(Ǧ,new Vector3(0,0,1),Ǧ.Y);ǰ=ŋ(Ǧ,new Vector3(-1,0,0),Ǧ.Y);Ǫ=ŋ(ΕΤ,new Vector3(0,0,1),ΕΤ.X);}if(ΗΙ&&ΒΒ()){Vector3 ΔА=Ω.GetNaturalGravity();Ǧ=ч(Ω,ΔА);ǘ=ŋ(Ǧ,new Vector3(0,0,1),Ǧ.Y);
ǰ=ŋ(Ǧ,new Vector3(-1,0,0),Ǧ.Y);}if(!ΓЃ(-45,ǰ,45)){ǘ=0;Ǫ=0;}if(!ΓЃ(-45,Ǫ,45)) ǘ=0;ΒΡ(ΕΑ,1,(-ǘ)*Ȅ,(-Ǫ)*Ȅ,(-ǰ)*Ȅ);Βκ=Math.Max(Math.Abs(ǘ),Math.Max(Math.Abs(ǰ),Math.Abs(Ǫ)));ΒЁ=Βκ<=Βά;}void ΙΘ(IMyTerminalBlock ΓΕ){if(ΓΕ==null) return;ȉ=new float[3,2];ǭ=new Dictionary<string,float[,]>();
for(int i=0;i<Č.Count;i++){IMyThrust э=Č[i];if(!э.IsFunctional) continue;Vector3 π=ч(ΓΕ,э.WorldMatrix.Backward);float ļ=э.MaxEffectiveThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) ȉ[0,0]+=ļ;else ȉ[0,1]-=ļ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) ȉ[1,0]+=ļ;else ȉ[1,1]-=ļ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) ȉ[2,0]+=ļ;else ȉ[2,1]-=ļ;String ǜ=Ζλ(э);float[,]Ǯ=null;if(ǭ.ContainsKey(ǜ)) Ǯ=ǭ[ǜ];else{Ǯ=new float[3,2];ǭ.Add(ǜ,Ǯ);}float ģ=э.MaxThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) Ǯ[0,0]+=ģ;else Ǯ[0,1]-=ģ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) Ǯ[1,0]+=ģ;else Ǯ[1,1]-=ģ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) Ǯ[2,0]+=ģ;else Ǯ[2,1]-=ģ;}}static String Ζλ(IMyThrust э){return э.BlockDefinition.SubtypeId;}Vector3 ΓЊ(Vector3 Ǣ,float[,]ƛ){return new Vector3(
Ǣ.X>=0?ƛ[0,0]:ƛ[0,1],
Ǣ.Y>=0?ƛ[1,0]:ƛ[1,1],
Ǣ.Z>=0?ƛ[2,0]:ƛ[2,1]);}bool Ζκ(ά ή,String ǜ,out float ΔЊ){ΔЊ=0;int Β=ċ.IndexOf(ǜ);if(Β==-1||ή.ȇ==null||Β>=ή.ȇ.Length)
return false;ΔЊ=ή.ȇ[Β];if(ΔЊ==-1) return false;return true;}Vector3 Γϋ(Vector3 Ǣ,ά ή){if(ή!=null){Vector3 ΰ=new Vector3();for(int i=0;i<ǭ.Keys.Count;i++){String ǜ=ǭ.Keys.ElementAt(i);float ΑΣ=0;if(!Ζκ(ή,ǜ,out ΑΣ))
return ΓЊ(Ǣ,ȉ);ΰ+=ΓЊ(Ǣ,ǭ.Values.ElementAt(i))*ΑΣ;}return ΰ;}return ΓЊ(Ǣ,ȉ);}float ƽ(Vector3 Ǣ,ά ή){return ƽ(Ǣ,new Vector3(),ή);}float ƽ(Vector3 Ǣ,Vector3 ω,ά ή){Vector3 ΛЂ=Γϋ(Ǣ,ή);Vector3 ļ=ΛЂ+ω*ŭ;float Ū=(ļ/Ǣ).AbsMin();return(float)(Ǣ*Ū).Length();}float ΑЃ(Vector3 ΒЈ,Vector3 ƙ,ά ή)
{if(ΒЈ.Length()==0) return 0;float ΗΘ=1;if(ƙ.Length()>0) ΗΘ=Math.Min(1,Ŷ(-ƙ,ΒЈ)/90)*0.4f+0.6f;float Αυ=ƽ(ΒЈ,ƙ,ή);if(Αυ==0) return 0.1f;float ΒΫ=К(Αυ,ŭ);float ΕЅ=(float) Math.Sqrt(К(ΒЈ.Length(),ΒΫ*0.5f));return ΒΫ*ΕЅ*ΗΘ*Ȑ;}float ΑА(Vector3 Ț,Vector3 Ј,Vector3 ω,ά ή)
{if(ω.Length()==0f) return 0;Vector3 ΒЉ=ő(Ț,Ј,Vector3.Normalize(ω));float Ǻ=ƽ(-ΒЉ,ή);return Ǻ/ω.Length();}void ΖΟ(bool ΛΘ){float ǯ=0;float ΔЍ=0.9f;if(ΛΘ){з=-1;ű=0;ΓΟ=null;if(ζ!=Σ.ш&&Ζ.ω.Length()!=0){ǯ=ΔЍ*ΑА(Ζ.κ,Ζ.ι*-1,Ζ.ω,null);if(ǯ<з||з==-1) з=ǯ;}if(Ρ.ν&&Ρ.ω.Length()!=0)
{ǯ=ΔЍ*ΑА(Ρ.κ,Ρ.ι*-1,Ρ.ω,null);if(ǯ<з||з==-1) з=ǯ;}return;}if(ű==-1) return;if(ű>=0){int Θγ=0;while(ű<Ψ.Count){if(Θγ>100) return;Θγ++;ά ή=Ψ[ű];if(ή.ω.Length()!=0f){ǯ=ΔЍ*Math.Min(
ΑА(ή.κ,ή.ι*-1,ή.ω,ή),
ΑА(ή.κ*-1,ή.ι*-1,ή.ω,ή));if(ǯ<з||з==-1) з=ǯ;}else ΓΟ=ή;ű++;}ű=-1;}bool ΕΔ=true;float Ń=0;if(ΓΟ!=null){for(int i=0;i<ǭ.Count;i++){String ǜ=ǭ.Keys.ElementAt(i);float[,]Ǯ=ǭ.Values.ElementAt(i);float ΑΣ=0;if(!Ζκ(ΓΟ,ǜ,out ΑΣ)){ΕΔ=false;break;}for(int a=0;a<Ǯ.GetLength(0);a++)
for(int b=0;b<Ǯ.GetLength(1);b++){float Ŕ=Math.Abs(Ǯ[a,b]*ΑΣ);if(Ŕ==0) continue;ΕΔ=true;if(Ń==0||Ŕ<Ń) Ń=Ŕ;}}}if(!ΕΔ){for(int a=0;a<ȉ.GetLength(0);a++)
for(int b=0;b<ȉ.GetLength(1);b++){float Ŕ=Math.Abs(ȉ[a,b]);if(Ŕ==0) continue;if(Ń==0||Ŕ<Ń) Ń=Ŕ;}}if(Ń>0){float ΛΥ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǯ=К(Ń,ΛΥ);if(ǯ>0)
if(ǯ<з||з==-1) з=ǯ;}}void ΖΚ(Vector3 Ǻ,bool О){if(!О){for(int i=0;i<Č.Count;i++)
Č[i].SetValueFloat("Override",0.0f);return;}Vector3 Δλ=Γϋ(Ǻ,null);float ΜΚ=Math.Min(1,Math.Abs(К(Ǻ.X,Δλ.X)));float ΜΛ=Math.Min(1,Math.Abs(К(Ǻ.Y,Δλ.Y)));float ΜΜ=Math.Min(1,Math.Abs(К(Ǻ.Z,Δλ.Z)));for(int i=0;i<Č.Count;i++){IMyThrust э=Č[i];Vector3 π=ΙА(ч(Ω,э.WorldMatrix.Backward),1);
if(π.X!=0&&Math.Sign(π.X)==Math.Sign(Ǻ.X))
э.SetValueFloat("Override",э.MaxThrust*ΜΚ);else if(π.Y!=0&&Math.Sign(π.Y)==Math.Sign(Ǻ.Y))
э.SetValueFloat("Override",э.MaxThrust*ΜΛ);else if(π.Z!=0&&Math.Sign(π.Z)==Math.Sign(Ǻ.Z))
э.SetValueFloat("Override",э.MaxThrust*ΜΜ);else
э.SetValueFloat("Override",0.0f);}}void ΒΡ(bool О,float Λε,float ǘ,float Ǫ,float ǰ){for(int i=0;i<Βό.Count;i++){IMyGyro Л=Βό[i];Л.GyroOverride=О;if(!О) Л.GyroPower=100;else Л.GyroPower=Λε;if(!О) continue;Vector3 Э=Ω.WorldMatrix.Forward;Vector3 у=Ω.WorldMatrix.Right;Vector3 Ј=Ω.WorldMatrix.Up;
Vector3 Ƒ=Л.WorldMatrix.Forward;Vector3 Ɠ=Л.WorldMatrix.Up;Vector3 ƒ=Л.WorldMatrix.Left*-1;if(Ƒ==Э) Л.SetValueFloat("Roll",ǰ);else if(Ƒ==(Э*-1)) Л.SetValueFloat("Roll",ǰ*-1);else if(Ɠ==(Э*-1)) Л.SetValueFloat("Yaw",ǰ);else if(Ɠ==Э) Л.SetValueFloat("Yaw",ǰ*-1);else if(ƒ==Э) Л.SetValueFloat("Pitch",ǰ);
else if(ƒ==(Э*-1)) Л.SetValueFloat("Pitch",ǰ*-1);if(ƒ==(у*-1)) Л.SetValueFloat("Pitch",ǘ);else if(ƒ==у) Л.SetValueFloat("Pitch",ǘ*-1);else if(Ɠ==у) Л.SetValueFloat("Yaw",ǘ);else if(Ɠ==(у*-1)) Л.SetValueFloat("Yaw",ǘ*-1);else if(Ƒ==(у*-1)) Л.SetValueFloat("Roll",ǘ);
else if(Ƒ==у) Л.SetValueFloat("Roll",ǘ*-1);if(Ɠ==(Ј*-1)) Л.SetValueFloat("Yaw",Ǫ);else if(Ɠ==Ј) Л.SetValueFloat("Yaw",Ǫ*-1);else if(ƒ==Ј) Л.SetValueFloat("Pitch",Ǫ);else if(ƒ==(Ј*-1)) Л.SetValueFloat("Pitch",Ǫ*-1);else if(Ƒ==Ј) Л.SetValueFloat("Roll",Ǫ);else if(Ƒ==(Ј*-1)) Л.SetValueFloat("Roll",Ǫ*-1);
}}void ΖА(){this.ΕΑ=false;}void н(Vector3 Ȗ,Vector3 Э,Vector3 š,float ĩ,bool Αϊ){Γα(Ȗ,ĩ,Αϊ);Βά=ĩ;ΕΥ=false;this.ΕЀ=Э;this.Θύ=š;}void н(Vector3 Ȗ,Vector3 Э,Vector3 š,bool Αϊ){н(Ȗ,Э,š,2f,Αϊ);}void Γα(Vector3 Ȗ,float ĩ,bool Αϊ){Βά=ĩ;this.ΕΑ=true;this.ΗΙ=Αϊ;ΕΥ=true;ΒЁ=false;
this.ΕϏ=Ȗ;}void ΔΫ(){Ȏ(false,false,false,Ѓ,0);ΓΥ=false;}void к(Vector3 ε,float Ȅ){Ȏ(true,false,false,ε,Ȅ);}void Ȏ(bool О,bool Εϊ,bool ΕБ,Vector3 ε,float Ȅ){Ȏ(О,Εϊ,ΕБ,ε,ε-δ,0.0f,Ȅ);}void Ȏ(bool О,bool Εϊ,bool ΕБ,Vector3 ε,Vector3 Λσ,float Λτ,float Ȅ){ΓΥ=true;this.ΘЀ=О;
Ѓ=ε;this.ΕΫ=Ȅ;this.Εθ=Λτ;this.Θν=Εϊ;this.Θμ=ΕБ;this.Ηω=Λσ;д=Vector3.Distance(ε,δ);}void ΚΒ(){φ.GetBlocksOfType(ǌ,Ǟ);}void ΚΓ(){if(ΓΗ){В=2;return;}List<IMyRemoteControl>Ακ=new List<IMyRemoteControl>();List<IMySensorBlock>Θκ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ασ=new List<IMyTerminalBlock>();
φ.GetBlocksOfType(Ακ,Ǟ);φ.GetBlocksOfType(Ч,Ǟ);φ.GetBlocksOfType(Θκ,Ǟ);φ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ασ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=Ǳ(Ч,pamTag,true);ΖЈ();ΖЉ(Ασ);Ő(Ч,setLCDFontAndSize,1.4f,false);Ő(ȡ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Θψ=Ǳ(Θκ,pamTag,true);if(Θψ.Count>0) ϊ=Θψ[0];else ϊ=null;if(Ν==Λ.ǃ){φ.GetBlocksOfType(ϋ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ϋ.Count==0){В=1;Κ="Drills are missing";}}else if(Ν==Λ.ǲ){φ.GetBlocksOfType(ϋ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ϋ.Count==0){В=1;Κ="Grinders are missing";}if(ϊ==null){В=1;Κ="Sensor is missing";}}else if(Ν==Λ.ί){φ.GetBlocksOfType(ΕІ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Θχ=Ǳ(Ακ,pamTag,true);if(Θχ.Count>0) Ακ=Θχ;if(Ακ.Count>0) Ω=Ακ[0];else{Ω=null;В=2;Κ="Remote is missing";return;}μ=(float) Ω.CubeGrid.WorldVolume.Radius*2;Ǩ=null;if(Ν!=Λ.ί){ΖΠ();if(ϋ.Count>0&&Ǩ!=null){if(ϊ!=null&&(Ǩ.WorldMatrix.Forward!=ϊ.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ϊ.WorldMatrix.Up||Ω.WorldMatrix.Down==ϊ.WorldMatrix.Down))){В=1;Κ="Wrong sensor direction";}if(Ǩ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
Ǩ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){В=2;Κ="Wrong remote direction";}}}}void ΚΔ(){φ.GetBlocksOfType(Ć,Ǟ);φ.GetBlocksOfType(ǎ,Ǟ);φ.GetBlocksOfType(Č,Ǟ);φ.GetBlocksOfType(Βό,Ǟ);φ.GetBlocksOfType(ĸ,Ǟ);φ.GetBlocksOfType(ń,Ǟ);φ.GetBlocksOfType(Ǥ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));φ.GetBlocksOfType(ΓΞ,Ǟ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ǎ=Ǳ(ǎ,"ConnectorMedium",false);else
ǎ=Ǳ(ǎ,"Connector",false);List<IMyShipConnector>Θυ=Ǳ(ǎ,pamTag,true);if(Θυ.Count>0) ǎ=Θυ;if(В<=1){if(ǎ.Count==0){В=1;Κ="Connector is missing";}if(Βό.Count==0){В=1;Κ="Gyros are missing";}if(Č.Count==0){В=1;Κ="Thrusters are missing";}}List<IMyConveyorSorter>Θω=Ǳ(ΓΞ,pamTag,true);
if(Θω.Count>0) ΓΞ=Θω;List<IMyLandingGear>Θφ=Ǳ(Ć,pamTag,true);if(Θφ.Count>0) Ć=Θφ;for(int i=0;i<Ć.Count;i++) Ć[i].AutoLock=false;List<IMyBatteryBlock>Θτ=Ǳ(ĸ,pamTag,true);if(Θτ.Count>0) ĸ=Θτ;List<IMyGasTank>Θϊ=Ǳ(Ǥ,pamTag,true);if(Θϊ.Count>0) Ǥ=Θϊ;}void ΚΕ(){φ.GetBlocksOfType(ŕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ƌ.Clear();for(int i=ŕ.Count-1;i>=0;i--){if(Ιϊ(ŕ[i])){ƌ.Add(ŕ[i]);ŕ.RemoveAt(i);}}}bool Ιϊ(IMyTerminalBlock Ϊ){if(Ϊ is IMyCargoContainer) return true;if(Ϊ is IMyShipDrill) return true;if(Ϊ is IMyShipGrinder) return true;if(Ϊ is IMyShipConnector){if(((IMyShipConnector) Ϊ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓϏ(Ϊ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΖΠ(){if(Ω==null) return;Ǩ=null;float ΒА=0,ΒЋ=0,ΒБ=0,ΒЌ=0,ΓΑ=0,ΒЍ=0;List<IMyTerminalBlock>Εώ=Ǳ(ϋ,pamTag,true);bool Λά=Εώ.Count==0;if(Εώ.Count>0) Ǩ=Εώ[0];else if(ϋ.Count>0) Ǩ=ϋ[0];int ā=0;float Ċ=0;for(int i=0;i<ϋ.Count;i++)
{if(ϋ[i].WorldMatrix.Forward!=Ǩ.WorldMatrix.Forward){if(Λά){В=2;Κ="Mining direction is unclear!";return;}continue;}ā++;Ċ=Math.Max(Ċ,ΚΧ(ϋ[i]));Vector3 П=ΑЏ(Ω,ϋ[i].GetPosition());if(i==0){ΒА=П.X;ΒЋ=П.X;ΒБ=П.Y;ΒЌ=П.Y;ΓΑ=П.Z;ΒЍ=П.Z;}ΒЋ=Math.Max(П.X,ΒЋ);ΒА=Math.Min(П.X,ΒА);
ΒЌ=Math.Max(П.Y,ΒЌ);ΒБ=Math.Min(П.Y,ΒБ);ΒЍ=Math.Max(П.Z,ΒЍ);ΓΑ=Math.Min(П.Z,ΓΑ);}if(Ċ==0) Ċ=drillRadius;bool Ȗ=Ǩ!=null&&Ǩ.WorldMatrix.Forward==Ω.WorldMatrix.Down;Ȉ=(ΒЋ-ΒА)*(1-Ų)+Ċ*2;ȥ=(Ȗ?ΒЍ-ΓΑ:ΒЌ-ΒБ)*(1-Ŝ)+Ċ*2;IMyCubeGrid g=Ω.CubeGrid;float ΖΔ=0,ΖΕ=0,ΖΖ=0,ΖΗ=0;
for(int c=0;c<8;c++){Vector3 p=ΑЏ(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȗ?p.Z:p.Y;if(c==0||p.X<ΖΔ) ΖΔ=p.X;if(c==0||p.X>ΖΕ) ΖΕ=p.X;if(c==0||y<ΖΖ) ΖΖ=y;if(c==0||y>ΖΗ) ΖΗ=y;}ΓΙ=ΖΕ-ΖΔ+g.GridSize;Ευ=ΖΗ-ΖΖ+g.GridSize;}float ΚΧ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǳ<T>(List<T>Џ,String ȅ,bool Βύ){List<T>ΰ=new List<T>();for(int i=0;i<Џ.Count;i++)
if(ΓϏ(Џ[i],ȅ,Βύ)) ΰ.Add(Џ[i]);return ΰ;}bool ΓϏ<T>(T Ϊ,String ȅ,bool Βύ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(Βύ&&b.CustomName.ToUpper().Contains(ȅ.ToUpper())) return true;if(!Βύ&&b.BlockDefinition.ToString().ToUpper().Contains(ȅ.ToUpper())) return true;return false;}void Ǡ<T>(List<T>Џ,bool О)
{for(int i=0;i<Џ.Count;i++)
ΖІ((IMyTerminalBlock) Џ[i],О);}void ΖІ(IMyTerminalBlock Ϊ,bool О){if(Ϊ==null) return;String Κο=О?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(Κο);a.Apply(Ϊ);}void ȏ(bool О){Εϋ=О;if(!Ů) return;Ǡ(ΓΞ,О);}void ΔΠ(ChargeMode Б){for(int i=0;i<ĸ.Count;i++) ĸ[i].ChargeMode=Б;}void ΔΣ(bool Λϋ)
{for(int i=0;i<Ǥ.Count;i++) Ǥ[i].Stockpile=Λϋ;}void ǆ(List<IMyLandingGear>ΔЎ,bool ΘΜ){for(int i=0;i<ΔЎ.Count;i++){if(ΘΜ) ΔЎ[i].Lock();if(!ΘΜ) ΔЎ[i].Unlock();}}void ΖЇ(bool О){if(Ηι==О) return;List<IMyShipController>Βζ=new List<IMyShipController>();φ.GetBlocksOfType(Βζ,Ǟ);if(Βζ.Count==0) return;
for(int i=0;i<Βζ.Count;i++)
Βζ[i].DampenersOverride=О;Ηι=О;}IMyShipConnector й(MyShipConnectorStatus Ǚ){for(int i=0;i<ǎ.Count;i++){if(!Ȫ(ǎ[i],true)) continue;if(ǎ[i].Status==Ǚ) return ǎ[i];}return null;}void Œ(bool Εφ,bool Βπ,float ΚА,float Κυ){if(ϊ==null||ϋ.Count==0) return;Vector3 ƍ=new Vector3();int ā=0;
for(int i=0;i<ϋ.Count;i++){if(ϋ[i].WorldMatrix.Forward!=Ǩ.WorldMatrix.Forward) continue;ā++;ƍ+=ϋ[i].GetPosition();}ƍ=ƍ/ā;Vector3 Ơ=ΑЏ(ϊ,ƍ);ϊ.Enabled=true;ϊ.ShowOnHUD=Εφ;ϊ.LeftExtend=(Βπ?1:ѕ)/2f*Ȉ-Ơ.X;ϊ.RightExtend=(Βπ?1:ѕ)/2f*Ȉ+Ơ.X;ϊ.TopExtend=(Βπ?1:є)/2f*ȥ+Ơ.Y;
ϊ.BottomExtend=(Βπ?1:є)/2f*ȥ-Ơ.Y;ϊ.FrontExtend=(Εφ?р:ΚА)-Ơ.Z;ϊ.BackExtend=Εφ?0:Κυ+μ*0.75f+Ơ.Z;ϊ.DetectFloatingObjects=true;ϊ.DetectAsteroids=false;ϊ.DetectLargeShips=true;ϊ.DetectSmallShips=true;ϊ.DetectStations=true;ϊ.DetectOwner=true;ϊ.DetectSubgrids=false;
ϊ.DetectPlayers=false;ϊ.DetectEnemy=true;ϊ.DetectFriendly=true;ϊ.DetectNeutral=true;}void Ő<T>(List<T>Џ,bool Λφ,float ΚЎ,bool ΛΙ){for(int i=0;i<Џ.Count;i++){IMyTextSurface Ας=null;if(Џ[i]is IMyTextSurface) Ας=(IMyTextSurface) Џ[i];if(Ας!=null){Ας.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Λφ) continue;Ας.Font="Debug";if(ΛΙ) continue;Ας.FontSize=ΚЎ;}}}bool ΒΒ(){Vector3D έ;return this.Ω.TryGetPlanetPosition(out έ);}int Ικ(Vector3 Λύ,int Θξ){if(Θξ==-1) return 0;double Ь=-1;int Ė=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,Λύ);if(Ь==-1||υ<Ь){Ė=i;Ь=υ;}
}return Math.Sign(Ė-Θξ);}bool Ιϋ(Vector3 έ){List<Vector3>а=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) а.Add(Ψ[i].Ε);if(Ρ.ν&&Ψ.Count>=1){Vector3 Ƈ=new Vector3();ȩ(Ρ,dockDist*μ,false,out Ƈ);if(Vector3.Distance(Ρ.Ε,Ψ.First().Ε)<Vector3.Distance(Ρ.Ε,Ψ.Last().Ε)){а.Insert(0,Ƈ);а.Insert(0,Ρ.Ε);}
else{а.Add(Ƈ);а.Add(Ρ.Ε);}}if(Ν==Λ.ί){if(Ζ.ν&&Ψ.Count>=1){Vector3 Δη=new Vector3();ȩ(Ζ,dockDist*μ,false,out Δη);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){а.Insert(0,Δη);а.Insert(0,Ζ.Ε);}else{а.Add(Δη);а.Add(Ζ.Ε);}}}else{if(ζ!=Σ.ш)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
а.Insert(0,Ζ.Ε);else а.Add(Ζ.Ε);}int Ė=-1;double ȓ=-1;for(int i=0;i<а.Count;i++){double υ=Vector3.Distance(а[i],έ);if(υ<ȓ||ȓ==-1){ȓ=υ;Ė=i;}}if(а.Count==0) return false;double ΔЄ=Vector3.Distance(а[Ė],έ);double Λζ=Vector3.Distance(а[Math.Max(0,Ė-1)],а[Ė])*1.5f;
double ΛΫ=Vector3.Distance(а[Math.Min(а.Count-1,Ė+1)],а[Ė])*1.5f;return ΔЄ<Λζ||ΔЄ<ΛΫ||ΔЄ<laneOffset*μ*1.5f;}static float К(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΙА(Vector3 v,int ΔЁ){return new Vector3(Math.Round(v.X,ΔЁ),Math.Round(v.Y,ΔЁ),Math.Round(v.Z,ΔЁ));
}float Ŷ(Vector3 a,Vector3 b){if(a==b) return 0;float ΚЊ=(a*b).Sum;float ΘΖ=a.Length();float ΘΗ=b.Length();if(ΘΖ==0||ΘΗ==0) return 0;float ΰ=(float)((180.0f/Math.PI)*Math.Acos(ΚЊ/(ΘΖ*ΘΗ)));if(float.IsNaN(ΰ)) return 0;return ΰ;}float ŋ(Vector3 v,Vector3 ΓΕ,float Λχ){float ƅ=Ŷ(v,ΓΕ);
if(Λχ>0f) ƅ*=-1;if(ƅ>-90f) return ƅ-90f;else return 180f-(-ƅ-90f);}double ΖΩ(float ΚЂ){return(Math.PI/180)*ΚЂ;}bool ΓЃ(double ΛΤ,double value,double Ɯ){return(value>=ΛΤ&&value<=Ɯ);}Vector3 ΚΨ(IMyTerminalBlock Ϊ,Vector3 П){return Vector3D.Transform(П,Ϊ.WorldMatrix);
}Vector3 ΑЏ(IMyTerminalBlock Ϊ,Vector3 ΜΗ){return ч(Ϊ,ΜΗ-Ϊ.GetPosition());}Vector3 ч(IMyTerminalBlock Ϊ,Vector3 ΖΓ){return Vector3D.TransformNormal(ΖΓ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 ő(Vector3 Ț,Vector3 Ј,Vector3 ΖΓ){MatrixD ΕΪ=MatrixD.CreateFromDir(Ț,Ј);return Vector3D.TransformNormal(ΖΓ,MatrixD.Transpose(ΕΪ));
}Vector3 Δέ(Vector3 Ț,Vector3 Ј,Vector3 П){MatrixD ΕΪ=MatrixD.CreateFromDir(Ț,Ј);return Vector3D.Transform(П,ΕΪ);}String ο(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 θ(String s){String[]χ=s.Split('|');return new Vector3(float.Parse(Τ(χ,0)),float.Parse(Τ(χ,1)),float.Parse(Τ(χ,2)));
}String Κν(ά ή){String С=":";String ΰ=ο(ή.Ε)+С+ο(ή.κ)+С+
ο(ή.ι)+С+ο(ή.Ђ)+С+ο(ή.ω);for(int i=0;i<ή.ȇ.Length;i++)
ΰ+=С+Math.Round(ή.ȇ[i],3);return ΰ;}ά ΚΠ(String s){String[]χ=s.Split(':');ά ή=new ά(
θ(Τ(χ,0)),θ(Τ(χ,1)),
θ(Τ(χ,2)),θ(Τ(χ,3)),
θ(Τ(χ,4)));int Β=5;List<float>Ηφ=new List<float>();while(Β<χ.Length){String ΓΫ=Τ(χ,Β);float f=0;if(!float.TryParse(ΓΫ,out f)) break;Ηφ.Add(f);Β++;}ή.ȇ=Ηφ.ToArray();return ή;}System.Text.StringBuilder Ƨ=new System.Text.StringBuilder();void Χ<T>(T value,bool ΛΪ){if(ΛΪ) Ƨ.Append('\n');Ƨ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΑЈ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΨ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Τ(String[]Κσ,int Β){String ΓΫ=Κσ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΫ)) return "";
return ΓΫ;}void Save(){if(ΓΗ||Ν==Λ.џ){Storage="";return;}Storage=Γη();}String Γη(){Ƨ.Clear();Ƨ.Append(DATAREV+";");Χ(ο(ΓΖ),false);Χ(ο(Ρ.κ));Χ(ο(Ρ.Ђ));Χ(ο(Ρ.ι));Χ(ο(Ρ.ω));Χ(ο(Ρ.Ε));Χ(ο(Ρ.с));Χ(Ρ.ν);Χ(ο(Ζ.Ε));Χ(ο(Ζ.ω));Χ(ο(Ζ.κ));Χ(ο(Ζ.ι));Χ(ο(Ζ.Ђ));Χ(ο(Ζ.с));Χ(Ζ.ν);Χ(ο(Ǘ));Χ(ο(ć));Χ(";");
Χ((int) Ν,false);Χ((int) ζ);Χ((int) ǔ);Χ(Ȝ);Χ(Ǖ);Χ(т);Χ(ǖ);Χ(Ȑ);Χ(Ĭ);Χ(ī);if(Ν==Λ.ί){Χ((int) Љ.Т);Χ(Љ.Ǹ);Χ(Љ.Ľ);Χ(ΑЈ(Љ.ǹ));Χ(ΑЈ(Љ.Ǿ));Χ((int) Њ.Т);Χ(Њ.Ǹ);Χ(Њ.Ľ);Χ(ΑЈ(Њ.ǹ));Χ(ΑЈ(Њ.Ǿ));}else{Χ((int) Ƞ);Χ((int) њ);Χ((int) ж);Χ((int) Ǐ);Χ((int) ş);Χ(Ň);Χ(Ů);Χ(Ɩ);Χ(Ɛ);Χ(ѕ);Χ(є);Χ(р);Χ(Ƹ);Χ(Ʒ);
Χ(Ų);Χ(Ŝ);Χ(Ў);Χ(Ю);Χ(Є);Χ(Ѐ);Χ(Ģ);Χ(г);Χ(ū);Χ(ţ);}Χ(";");for(int i=0;i<ċ.Count;i++)
Χ((i>0?"|":"")+ċ[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(Κν(Ψ[i]),i>0);Χ(";",false);Χ(е+"|"+Д,false);Χ(";",false);Χ(ΙΫ(),false);String Η=Ƨ.ToString();Ƨ.Clear();return Η;}Ǎ ΙЏ(){return ΒΜ(Storage);}Ǎ ΒΜ(String Η){if(Η=="") return Ǎ.ΙЄ;String[]Ŭ=Η.Split(';');if(Τ(Ŭ,0)!=DATAREV) return Ǎ.ΓА;int Β=0;try{String[]Π=Τ(Ŭ,1).Split('\n');
ΓΖ=θ(Τ(Π,Β++));Ρ.κ=θ(Τ(Π,Β++));Ρ.Ђ=θ(Τ(Π,Β++));Ρ.ι=θ(Τ(Π,Β++));Ρ.ω=θ(Τ(Π,Β++));Ρ.Ε=θ(Τ(Π,Β++));Ρ.с=θ(Τ(Π,Β++));Ρ.ν=bool.Parse(Τ(Π,Β++));Ζ.Ε=θ(Τ(Π,Β++));Ζ.ω=θ(Τ(Π,Β++));Ζ.κ=θ(Τ(Π,Β++));Ζ.ι=θ(Τ(Π,Β++));Ζ.Ђ=θ(Τ(Π,Β++));Ζ.с=θ(Τ(Π,Β++));Ζ.ν=bool.Parse(Τ(Π,Β++));
Ǘ=θ(Τ(Π,Β++));ć=θ(Τ(Π,Β++));Π=Τ(Ŭ,2).Split('\n');Β=0;Ν=(Λ) int.Parse(Τ(Π,Β++));ζ=(Σ) int.Parse(Τ(Π,Β++));ǔ=(Σ) int.Parse(Τ(Π,Β++));Ȝ=int.Parse(Τ(Π,Β++));Ǖ=int.Parse(Τ(Π,Β++));т=int.Parse(Τ(Π,Β++));ǖ=int.Parse(Τ(Π,Β++));Ȑ=float.Parse(Τ(Π,Β++));Ĭ=bool.Parse(Τ(Π,Β++));
ī=bool.Parse(Τ(Π,Β++));if(Ν==Λ.ί){Љ.Т=(τ) int.Parse(Τ(Π,Β++));Љ.Ǹ=float.Parse(Τ(Π,Β++));Љ.Ľ=float.Parse(Τ(Π,Β++));Љ.ǹ=ΒΨ(Τ(Π,Β++));Љ.Ǿ=ΒΨ(Τ(Π,Β++));Њ.Т=(τ) int.Parse(Τ(Π,Β++));Њ.Ǹ=float.Parse(Τ(Π,Β++));Њ.Ľ=float.Parse(Τ(Π,Β++));Њ.ǹ=ΒΨ(Τ(Π,Β++));Њ.Ǿ=ΒΨ(Τ(Π,Β++));}else
{Ƞ=(ђ) int.Parse(Τ(Π,Β++));њ=(я) int.Parse(Τ(Π,Β++));ж=(Г) int.Parse(Τ(Π,Β++));Ǐ=(ǝ) int.Parse(Τ(Π,Β++));ş=(ђ) int.Parse(Τ(Π,Β++));Ň=bool.Parse(Τ(Π,Β++));Ů=bool.Parse(Τ(Π,Β++));Ɩ=bool.Parse(Τ(Π,Β++));Ɛ=bool.Parse(Τ(Π,Β++));ѕ=int.Parse(Τ(Π,Β++));є=int.Parse(Τ(Π,Β++));р=int.Parse(Τ(Π,Β++));
Ƹ=float.Parse(Τ(Π,Β++));Ʒ=float.Parse(Τ(Π,Β++));Ų=float.Parse(Τ(Π,Β++));Ŝ=float.Parse(Τ(Π,Β++));Ў=int.Parse(Τ(Π,Β++));Ю=int.Parse(Τ(Π,Β++));Є=int.Parse(Τ(Π,Β++));Ѐ=int.Parse(Τ(Π,Β++));Ģ=int.Parse(Τ(Π,Β++));г=int.Parse(Τ(Π,Β++));ū=int.Parse(Τ(Π,Β++));ţ=float.Parse(Τ(Π,Β++));
}Π=Τ(Ŭ,3).Replace("\n","").Split('|');ċ=Π.ToList();Π=Τ(Ŭ,4).Split('\n');Ψ.Clear();if(Π.Count()>=1&&Π[0]!="")
for(int i=0;i<Π.Length;i++)
Ψ.Add(ΚΠ(Τ(Π,i)));Π=Τ(Ŭ,5).Split('|');е=Τ(Π,0);if(!int.TryParse(Τ(Π,1),out Д)) Д=-1;if(е.Length!=Ў*Ю){е="";Д=-1;}ΙΪ(Τ(Ŭ,6));}catch{return Ǎ.Ζά;}ƨ=ǔ;Ѕ();return Ǎ.ΒΤ;}const int ΖΞ=64000;IMyTerminalBlock Ζέ(){List<IMyTerminalBlock>Ɗ=new List<IMyTerminalBlock>();φ.GetBlocksOfType(Ɗ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ɗ.Count>0?Ɗ[0]:null;}void Ιί(){IMyTerminalBlock Ϊ=Ζέ();if(Ϊ==null){Κ="Export: name a block "+dataTag;return;}String Η="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Γη();if(Η.Length>ΖΞ){Κ="Export too large: "+Η.Length+" chars, max "+ΖΞ;return;}Ϊ.CustomData=Η;Κ="Exported "+Ψ.Count+" waypoints";}void Ιχ(bool ΕΜ){IMyTerminalBlock Ϊ=Ζέ();if(Ϊ==null){Κ="Import: name a block "+dataTag;return;}
Ιψ(Ϊ.CustomData,ΕΜ);}void Ιψ(String Η,bool ΕΜ){Η=Η.Replace("\r","");if(Η.StartsWith("//")){int ΘΪ=Η.IndexOf('\n');Η=ΘΪ<0?"":Η.Substring(ΘΪ+1);}Η=Η.Trim();if(Η==""){Κ="Import: no PAM data in "+dataTag;return;}Ѕ();String ΗΠ=Γη();Ǎ ΰ=ΒΜ(Η);if(ΰ==Ǎ.ΒΤ&&ΕΜ){ά Μ=new ά(Ζ);
Vector3 ΛΧ=Ǘ,ΛΦ=ć;int w=ѕ,h=є,d=р;ђ Απ=Ƞ;ǝ ΔЂ=Ǐ;float Λί=Ų,Λή=Ŝ;ΒΜ(ΗΠ);Ζ=Μ;Ǘ=ΛΧ;ć=ΛΦ;ѕ=w;є=h;р=d;Ƞ=Απ;Ǐ=ΔЂ;Ų=Λί;Ŝ=Λή;ζ=Σ.ƀ;Ǌ(true,true);ζ=Σ.ƀ;ǔ=Σ.ƀ;Κ="Imported job, run CONT";return;}if(ΰ==Ǎ.ΒΤ){Αζ=false;Κ="Imported "+Ψ.Count+" waypoints";
return;}ΒΜ(ΗΠ);Κ=ΰ==Ǎ.ΓА?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖЈ(){Ř.Clear();for(int i=Ч.Count-1;i>=0;i--){String Η=Ч[i].CustomData.ToUpper();bool ΕΚ=false;if(Η=="INSTRUCTIONS"){ΕΚ=true;Βξ=true;}if(Η=="DEBUG") ΕΚ=true;if(ΕΚ){Ř.Add(Ч[i]);Ч.RemoveAt(i);}
}Ő(Ř,false,1,false);}void ΖЉ(List<IMyTerminalBlock>Ɗ){ȡ.Clear();for(int i=0;i<Ɗ.Count;i++){IMyTerminalBlock Ϊ=Ɗ[i];try{String Θβ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϊ.CustomName.IndexOf(Θβ);int ƭ=-1;if(Β<0||!int.TryParse(Ϊ.CustomName.Substring(Β+Θβ.Length,1),out ƭ)) continue;
if(ƭ==-1) continue;ƭ--;IMyTextSurfaceProvider Θδ=(IMyTextSurfaceProvider) Ϊ;if(ƭ<Θδ.SurfaceCount&&ƭ>=0)
ȡ.Add(Θδ.GetSurface(ƭ));}catch{}}}String ΒЎ="";void ΖЃ(){String ΕΩ="";String ΑΡ="";ΒЎ=ΖΝ(false);ΕΩ+=ΒЎ;ΑΡ+=ΒЎ;ΑΡ+=Ιη();for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(ΕΩ);for(int i=0;i<ȡ.Count;i++) ȡ[i].WriteText(ΕΩ);Echo(ΑΡ);for(int i=0;i<Ř.Count;i++){IMyTextPanel ь=Ř[i];
String Η=ь.CustomData.ToUpper();if(Η=="DEBUG") ь.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") ь.WriteText(Ζε());}}string Ζε(){String Γ="";try{float ΘΞ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ɖ*ΘΞ)+" / "+(int)(Ɲ*ΘΞ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ɖ*100f,1)+"% / "+Math.Round(Ɲ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ΑΕ,2)+"ms / "+Math.Round(Εέ,2)+"ms \n";}catch{}for(int i=0;i<Ĥ.Count;i++)
Γ+=""+Ĥ.Keys.ElementAt(i)+" = "+Math.Round(Ĥ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ĥ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Ις(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ő(Ч,setLCDFontAndSize,0.9f,false);Ő(ȡ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(Γ+ġ.ToString());for(int i=0;i<ȡ.Count;i++) ȡ[i].WriteText(Γ+ġ.ToString());}void ȯ(){if(!Βξ) return;try{Ελ=Runtime.CurrentInstructionCount;}catch{}
}void Ȩ(String α){if(!Βξ) return;if(Ελ==0) return;try{float ΓΩ=(Runtime.CurrentInstructionCount-Ελ)/Runtime.MaxInstructionCount*100;if(!Ĥ.ContainsKey(α))
Ĥ.Add(α,new float[]{ΓΩ,ΓΩ});else{Ĥ[α][0]=ΓΩ;Ĥ[α][1]=Math.Max(ΓΩ,Ĥ[α][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΘБ(String ș){if(!Ƌ) return;String С=""+BROADCAST_SEP;String γ=VERSION+С;γ+=ƹ(ƫ)+С;
γ+=(int) Ν+С;γ+=ź(δ.X)+""+С;γ+=ź(δ.Y)+С;γ+=ź(δ.Z)+С;if(Ν!=Λ.ί)
γ+=Γψ(ǔ)+(ǔ==Σ.И?" "+Math.Round(Αί,1)+"%":"")+С;else γ+=Γψ(ǔ)+С;γ+=ƹ(Κ)+С;γ+=А+""+С;γ+=ƹ(ΒЎ)+С;γ+=ȕ+С;γ+=ȝ+С;for(int i=0;i<Ǽ.Count;i++){if(Ǽ[i].ƚ!=Щ.ż) continue;γ+=ƹ(Ǽ[i].α)+"/"+ƹ(Ǽ[i].ψ)+"/"+Ǽ[i].Ќ+С;}ΚΙ(γ,ș);}String ƹ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΚΙ(String γ,String ș){ΒΟ(γ,BROADCAST_SELF_ID,ș);}void ΒΟ(String γ,String ε,String ș){try{if(!Ƌ) return;γ=BROADCAST_TAG+BROADCAST_SEP+Ζα()+BROADCAST_SEP+ε+BROADCAST_SEP+Δτ+BROADCAST_SEP+ș+BROADCAST_SEP+γ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,γ);}catch(Exception e){ġ=e;}
}String Ζα(){if(Ν!=Λ.џ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΓЈ(String γ){return γ.StartsWith(BROADCAST_TAG);}bool и(ref String Η,out string ΑΥ,Char С){int Β=Η.IndexOf(С);ΑΥ="";if(Β<0) return false;ΑΥ=Η.Substring(0,Β);Η=Η.Remove(0,Β+1);return true;}bool ΖЂ(ref String γ,out String ȟ,out String ș)
{ȟ="";ș="";if(!Ƌ) return false;String ȅ="";if(!и(ref γ,out ȅ,BROADCAST_SEP)||!ΓЈ(ȅ)) return false;if(!и(ref γ,out ȟ,BROADCAST_SEP)) return false;if(!и(ref γ,out ȅ,BROADCAST_SEP)||(ȅ!=Ζα()&&(ȅ!="*"&&Ν!=Λ.џ))) return false;if(!и(ref γ,out ȅ,BROADCAST_SEP)||(ȅ!=Δτ)) return false;
if(!и(ref γ,out ș,BROADCAST_SEP)) return false;return true;}void Ζρ(){bool ΑΚ=true;if(Ηψ){Ηψ=false;if(Me.CustomData.Contains("Antenna_Name")){Κ="Update custom data";Me.CustomData="";}}String ΑΫ=(Ν!=Λ.џ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΫ)
Ζς(ΑΫ);String[]Π=Me.CustomData.Split('\n');Ƌ=bool.Parse(ΒΛ(ΑЊ(Π,"Enable_Broadcast",ref ΑΚ)));bool ΖΑ=false;bool Ην=true;if(Ƌ){if(Ν!=Λ.џ)
ƫ=ΒΛ(ΑЊ(Π,"Ship_Name",ref ΑΚ)).Replace(BROADCAST_SELF_ID,"");Δτ=ΒΛ(ΑЊ(Π,"Broadcast_Channel",ref ΑΚ)).ToLower();Ην=false;if(Ƕ==null){Ƕ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ƕ.SetMessageCallback("");}List<IMyRadioAntenna>Ɔ=new List<IMyRadioAntenna>();φ.GetBlocksOfType(Ɔ);
bool Δζ=false;for(int i=0;i<Ɔ.Count;i++)
if(Ɔ[i].EnableBroadcasting&&Ɔ[i].Enabled){Δζ=true;break;}if(Ɔ.Count==0) Κ="No Antenna found";else if(!Δζ) Κ="Antenna not ready";ΖΑ=Ɔ.Count==0||!Δζ;if(ΗΚ&&!ΖΑ&&Ν!=Λ.џ) Κ="Antenna ok";}else if(Ν==Λ.џ)
Κ="Offline - Enable in PB custom data";ΗΚ=ΖΑ;if(Ην){if(Ƕ!=null) this.IGC.DisableBroadcastListener(Ƕ);Ƕ=null;}}catch{ΑΚ=false;}if(!ΑΚ){Κ="Reset custom data";Ζς(ΑΫ);}}void Ζς(String ΑΫ){String Η=ΑΫ+"\n\n"+"Enable_Broadcast="+(Ν==Λ.џ?"true":"false")+" \n";Η+=Ν!=Λ.џ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Η+"Broadcast_Channel=#default";}String ΒΛ(String Ţ){int Β=Ţ.IndexOf("//");if(Β!=-1) Ţ=Ţ.Substring(0,Β);String[]χ=Ţ.Split('=');if(χ.Length<=1) return "";return χ[1].Trim();}String ΑЊ(String[]Π,String ъ,ref bool Ĉ){foreach(String Ţ in Π)
if(Ţ.StartsWith(ъ)) return Ţ;Ĉ=false;return "";}void ΙΦ(string Ф){if(ę) Н=new List<ό>();String ȟ="";String ș="";if(!ΓЈ(Ф)) ΙЌ(Ф);bool Θλ=false;int Εΰ=50;while(Ƕ!=null&&Ƕ.HasPendingMessage&&Εΰ-->0){MyIGCMessage γ=Ƕ.AcceptMessage();String Η=γ.Data as String;if(Η==null) continue;
if(ΖЂ(ref Η,out ȟ,out ș)&&ȟ!=""&&ȟ!=BROADCAST_SELF_ID){ό Δ=Γφ(ȟ,"");if(Δ==null){Δ=new ό(ȟ);Н.Add(Δ);}Δ.ĳ(ș,false,false,true);Κζ(Δ,Η);Θλ=true;}}if(Θλ)
Н.Sort(delegate(ό a,ό b){if(a.α==null&&b.α==null) return 0;else if(a.α==null) return-1;else if(b.α==null) return 1;else return a.α.CompareTo(b.α);});if(Ğ||ę){if(Αμ<=0||ę){Κ="";ę=false;ΚΗ();Ζρ();ΕΣ=DateTime.Now;}}if(ğ) Κή();if(ğ) Κβ();if(ğ) Κλ();if(ğ) Κη();if(ğ&&ΕЁ==0) Κά();
ΚΫ();}void Κζ(ό Δ,String Η){Δ.ΕΟ=DateTime.Now;String ΑΥ="";и(ref Η,out Δ.Γέ,BROADCAST_SEP);и(ref Η,out Δ.α,BROADCAST_SEP);if(Δ.Γέ!=VERSION) return;String Αε="";String[]ƥ=new string[3];и(ref Η,out Αε,BROADCAST_SEP);и(ref Η,out ƥ[0],BROADCAST_SEP);и(ref Η,out ƥ[1],BROADCAST_SEP);
и(ref Η,out ƥ[2],BROADCAST_SEP);и(ref Η,out Δ.Εό,BROADCAST_SEP);и(ref Η,out Δ.Κ,BROADCAST_SEP);и(ref Η,out ΑΥ,BROADCAST_SEP);и(ref Η,out Δ.ΒЏ,BROADCAST_SEP);String ΘЇ="",ΘΡ="";и(ref Η,out ΘЇ,BROADCAST_SEP);и(ref Η,out ΘΡ,BROADCAST_SEP);Δ.Ѝ.Clear();while(true){String ΗА;if(!и(ref Η,out ΗА,BROADCAST_SEP)) break;
String[]ΒЃ=ΗА.Split('/');if(ΒЃ.Count()<3) continue;int Ќ=0;if(!int.TryParse(ΒЃ[2],out Ќ)) continue;Δ.Ѝ.Add(new о(ΒЃ[0],ΒЃ[1],Ќ,Щ.ż));}int ΘΦ=0;if(!int.TryParse(ΑΥ,out Δ.А)) Δ.А=0;if(!int.TryParse(Αε,out ΘΦ)) Δ.Ν=Λ.ĵ;if(!float.TryParse(ƥ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ƥ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(ƥ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(ΘЇ,out Δ.ȕ)) Δ.ȕ=0;if(!float.TryParse(ΘΡ,out Δ.ȝ)) Δ.ȝ=0;Δ.Ν=(Λ) ΘΦ;Δ.ΔЅ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΕЊ=0;for(int i=0;i<Δ.Ѝ.Count();i++) Δ.ΕЊ+=Δ.Ѝ[i].Ќ;}ό Γφ(String Ξ,String α){Ξ=Ξ.ToUpper();α=α.ToUpper();
for(int i=0;i<Н.Count;i++){if(Ξ!=""&&Н[i].Ξ.ToUpper()==Ξ) return Н[i];if(α!=""&&Н[i].α!=null&&Н[i].α.ToUpper()==α) return Н[i];}return null;}String Ǝ="";ό ΗΫ=null;DateTime Ηά=DateTime.MinValue;List<ό>ė=new List<ό>();const float ΙΕ=20f;bool ΖЋ(ό ε,String І){return І==Ǝ&&ε==ΗΫ&&((DateTime.Now-Ηά).TotalSeconds<ΙΕ||
(І=="HOMEPOS"&&(Ą.Count>0||ǻ!=null)));}String Ια(ό ε){if(Ǝ==""||!ΖЋ(ε,Ǝ)) return "";String s="";int Ĉ=0,Εχ=0;for(int i=0;i<ė.Count;i++){Ћ ΓΠ=ė[i].ĳ(Ǝ,false,false,false);if(ΓΠ==Ћ.Ǆ) Ĉ++;bool Αι=Ą.Contains(ė[i]);if(Εχ++<8) s+="  "+ė[i].α+": "+(Αι?"queued ("+(Ą.IndexOf(ė[i])+1)+")":ΓΠ==Ћ.Ǆ?"received":
ΓΠ==Ћ.Ɓ?"pending...":ΓΠ==Ћ.ΒΘ?"no answer!":"-")+"\n";}if(Εχ>8) s+="  +"+(Εχ-8)+" more\n";return "--------------------------------------------\n"+Ǝ+": "+Ĉ+"/"+ė.Count+" received\n"+s;}void ǳ(ό Δ,String І){Ǝ=І;ΗΫ=Δ;Ηά=DateTime.Now;ė.Clear();if(Δ==null) ė.AddRange(Н);else ė.Add(Δ);
if(Δ==null){Ą.Clear();ǻ=null;}else Ą.Remove(Δ);if(Δ==null&&І=="HOMEPOS"){ΚΟ();return;}if(Δ==null){for(int i=0;i<Н.Count;i++)
Н[i].ĳ(І,true,false,false);ΒΟ(І,"*",І);}else{Δ.ĳ(І,true,false,false);ΒΟ(І,Δ.Ξ,І);}}void ΙЌ(string Ф){if(Ф=="") return;var χ=Ф.ToUpper().Split(' ');χ.DefaultIfEmpty("");var І=χ.ElementAtOrDefault(0);var ȱ=χ.ElementAtOrDefault(1);String ą="Invalid argument: "+Ф;if(Ϗ==Υ.Ǉ){if(М!=null)
{switch(І){case "UP":{if(А<2) break;if(М.А==0){А=1;return;}ǳ(М,"UP");return;}case "DOWN":{if(А<1) break;if(А==1){ǳ(М,"MRES");break;}ǳ(М,"DOWN");break;}case "APPLY":{if(А<2) break;ǳ(М,"APPLY");return;}}}}switch(І){case "UP":this.ΓЍ(false);return;case "DOWN":this.ΓЌ(false);return;
case "APPLY":this.ŷ(true);return;}switch(І){case "CLEAR":ΙΟ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȱ)){Κ="Usage: SENDALL <cmd>";return;}ǳ(null,ȱ);return;case "SEND":
if(Ф.Length<="SEND".Length+1){Κ="Usage: SEND <ship>:<cmd>";return;}ΚΚ(Ф.Remove(0,"SEND".Length+1));return;}Κ=ą;}void ΚΚ(String ΑΒ){if(ΑΒ=="") return;var χ=ΑΒ.Split(':');if(χ.Length!=2){Κ="Missing separator \":\"";return;}χ.DefaultIfEmpty("");String ƫ=χ.ElementAtOrDefault(0).Trim();
ό Δ=Γφ("",ƫ);if(Δ!=null) ǳ(Δ,χ.ElementAtOrDefault(1).Trim());else Κ="Unknown ship: "+ƫ;}void ΙΟ(){Н.Clear();М=null;ξ(Υ.Main);А=0;}void ΚΗ(){List<IMyTerminalBlock>Ασ=new List<IMyTerminalBlock>();φ.GetBlocksOfType(Ч,Ǟ);φ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ασ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=Ǳ(Ч,pamTag,true);ΖЉ(Ασ);ΖЈ();ΚΘ();Ő(Ч,setLCDFontAndSize,1.15f,false);Ő(ȡ,setLCDFontAndSize,1.15f,true);String ΛΕ=Ζζ(false);String ΘΫ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ь in Ч){if(ь.CustomData==""){ь.CustomData=Ζζ(true);continue;}
if(!ь.CustomData.Contains(ΛΕ))
if(!ь.CustomData.Contains(ΘΫ)) ь.CustomData=ΘΫ+ь.CustomData;}}void ΚΫ(){String ΑΡ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȡ.Count;i++)
ȡ[i].WriteText(ΖΛ(У.Ζϊ,"0",false));for(int i=0;i<Ч.Count;i++){У Б=У.ĵ;String Ȃ="";ΙІ(Ч[i],out Б,out Ȃ);Ч[i].WriteText(ΖΛ(Б,Ȃ,i==0));}Echo(ΑΡ);for(int i=0;i<Ř.Count;i++){IMyTextPanel ь=Ř[i];String Η=ь.CustomData.ToUpper();if(Η=="DEBUG") ь.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") ь.WriteText(Ζε());
}}String ŷ(bool Ι){int ΚЋ=0;return ŷ(Ι,0,ref ΚЋ,false,1);}String ŷ(bool Ι,int Λβ,ref int Λς,bool Ηί,int Αη){String Γ="";String ƪ="——————————————————\n";String ΛЁ="--------------------------------------------\n";if(М==null||Ϗ==Υ.Main||Ϗ==Υ.ΔΟ)
Γ+="[PAM]-Controller | "+Н.Count+" Connected ships"+"\n"+Ιδ()+Ιι()+Ιξ();else Γ+=Ζι(М)+"\n";Γ+=ƪ;int Β=0;int Θ=А;Ť=0;if(Κ!=""){Ο(ref Γ,Θ,Β++,Ι,Κ);}else if(Ϗ==Υ.Main){if(М==null&&Н.Count>=1) М=Н[0];String Џ="";if(Ο(ref Џ,Θ,Β++,Ι," Send command to all")) ξ(Υ.ΔΟ);
int Ʃ=0;for(int s=0;s<Н.Count;s++){if(Ηί) Џ+="\n";ό Δ=Н[s];if(Θ==Β||Θ==Β+1) М=Δ;if(Β==Θ) Ʃ=Џ.Split('\n').Length-1;if(Ο(ref Џ,Θ,Β++,Ι," "+Ζι(Δ))){М=Δ;ξ(Υ.Ǉ);М.ĳ("",false,true,false);}if(Β==Θ) Ʃ=Џ.Split('\n').Length-1;if(Ο(ref Џ,Θ,Β++,Ι," "+(Ηί?"":"  ")+Ιλ(Δ)))
{М=Δ;ξ(Υ.ΖЊ);}}int Ƶ=Αη-2;Ƶ+=Math.Max(0,(Λβ-1))*Αη;Γ+=ΔΞ(Ƶ,Џ,Ʃ,ref Λς);}else if(Ϗ==Υ.ΒΠ||Ϗ==Υ.ΔΟ){Υ Κτ=Ϗ==Υ.ΒΠ?Υ.Ǉ:Υ.Main;ό ε=null;if(Ϗ==Υ.ΒΠ) ε=М;if(Ο(ref Γ,Θ,Β++,Ι," Back")){ξ(Κτ);}if(Ο(ref Γ,Θ,Β++,Ι," [Stop] "+Ż(ε,"STOP"))){ǳ(ε,"STOP");}if(Ο(ref Γ,Θ,Β++,Ι," [Continue] "+Ż(ε,"CONT"))){ǳ(ε,"CONT");}
if(Ο(ref Γ,Θ,Β++,Ι," [Move home] "+Ż(ε,"HOMEPOS"))){ǳ(ε,"HOMEPOS");}if(Ο(ref Γ,Θ,Β++,Ι," [Move to job] "+Ż(ε,"JOBPOS"))){ǳ(ε,"JOBPOS");}if(Ο(ref Γ,Θ,Β++,Ι," [Full simulation] "+Ż(ε,"FULL"))){ǳ(ε,"FULL");}if(ε!=null&&ε.Ν==Λ.ί)
if(Ο(ref Γ,Θ,Β++,Ι," [Undock] "+Ż(ε,"UNDOCK"))){ǳ(ε,"UNDOCK");}Γ+=Ια(ε);}else if(Ϗ==Υ.Ǉ){String Δν="";if(Ο(ref Δν,Θ,Β++,Ι," Back")){ξ(Υ.Main);}Γ+=Δν.Substring(0,Δν.Length-1).PadRight(36,' ');if(Ο(ref Γ,Θ,Β++,Ι," Send cmd")){ξ(Υ.ΒΠ);}if(!М.ΓЉ()&&!М.ΓЅ()) Ť++;
if(М.ΓЅ()) Γ+="No answer received...";else Γ+=ƃ(М,М.ΒЏ);}else if(Ϗ==Υ.ΖЊ){if(Ο(ref Γ,Θ,Β++,Ι," Back")){ξ(Υ.Main);}Γ+=ΛЁ;Γ+=ƃ(М,Ζθ(М));}if(!Εί) А=Math.Min(Ť-1,А);return Γ;}String Ζι(ό Δ){if(Δ.Γέ!=VERSION) return Δ.α+": Different version!";return Δ.α+": "+ƃ(Δ,Δ.Εό)+" "+Δ.ΔЅ+"m";
}String Ιλ(ό Δ){String s=ΔΛ("",К(Δ.ȕ,Δ.ȝ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ѝ.Count;i++){if(i>=5) break;о Ё=Δ.Ѝ[i];s+=ΗΔ(Γθ(Ιθ(Ё)),3)+" "+Ζί(Ё.Ќ)+" ";}return s;}String Ιθ(о Ё){if(Ё.ψ.ToUpper()=="ORE"||Ё.ψ.ToUpper()=="INGOT"){String Ηέ=Ιζ(Ё.α.ToUpper());if(Ηέ!="") return Ηέ;
}return Ё.α;}String ƃ(ό Δ,String Γ){if(Δ.ΓЉ()) return "No signal...("+ΑЍ((int)(DateTime.Now-Δ.ΕΟ).TotalSeconds)+")";return Γ;}String Ζζ(bool ΜΖ){String Γ="";if(ΜΖ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΙІ(IMyTerminalBlock ь,out У Б,out String Ȃ){bool Ĉ=true;String Αε=ΒΛ(ΑЊ(ь.CustomData.Split('\n'),"mode",ref Ĉ)).ToUpper();String[]χ=Αε.Split(':');String Αδ=χ.Length>0?χ[0].Trim():"";Ȃ=χ.Length>1?χ[1].Trim():"";
Б=У.ĵ;if(Αδ=="MAIN") Б=У.Ζϋ;else if(Αδ=="MAINX") Б=У.Ζό;else if(Αδ=="MENU") Б=У.ΓЋ;else if(Αδ=="INVENTORY") Б=У.ΓЄ;else if(Αδ=="DEBUG") Б=У.Γμ;if(Б==У.ĵ) Ȃ=Αε;}String ΖΛ(У Б,String Ȃ,bool ΚЍ){int Αα=15;if(ΚЍ){Ηα=Βδ;Βδ=0;Ηβ=Βε;Βε=0;}String Γ="";if(Б==У.Ζϋ)
{int ы=0;Γ=ŷ(false,Ηα,ref Κύ,true,Αα);if(!int.TryParse(Ȃ,out ы)) return Γ;Βδ=Math.Max(ы,Βδ);ы--;return ΖЁ(ы,Αα,Γ);}else if(Б==У.Ζό){int ы=0;Γ=ŷ(false,Ηβ,ref Κώ,false,Αα);if(!int.TryParse(Ȃ,out ы)) return Γ;Βε=Math.Max(ы,Βε);ы--;return ΖЁ(ы,Αα,Γ);}if(Б==У.Ζϊ){return ŷ(false,0,ref ΚϏ,true,Αα);
}else if(Б==У.ΓЄ||Б==У.ΓЋ||Б==У.Γμ){ό Δ=М;if(Ȃ!=""){Δ=Γφ("",Ȃ);if(Δ==null) return "Unknown ship: "+Ȃ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƪ="—————————————————————\n";String ΕЇ="";String Δσ="";if(Б==У.ΓЋ){ΕЇ="Menu";Δσ=ƃ(Δ,Δ.ΒЏ);}
else if(Б==У.ΓЄ){ΕЇ="Inventory";Δσ=ƃ(Δ,Ζθ(Δ));}else if(Б==У.Γμ){return ƃ(Δ,Ιμ(Δ));}Γ+=Δ.α+" - "+Δ.ΔЅ+"m | "+Γθ(""+ΕЇ)+"\n"+ƪ;Γ+=Δσ;return Γ;}return "Unknown command: "+Ȃ;}string Ιμ(ό Δ){String s="";s+=Δ.Γέ+"\n";s+=Δ.α+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Εό+"\n";s+=Δ.Κ+"\n";s+=Δ.А+"\n";s+=Δ.ΒЏ.Length+"\n";s+=Δ.ȕ+"\n";s+=Δ.ȝ+"\n";s+=Δ.Ѝ.Count()+"\n";return s;}string Ζθ(ό Δ){String s="";s+=ΔΛ("All",К(Δ.ȕ,Δ.ȝ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ѝ.Count();i++){о Ё=Δ.Ѝ[i];
s+=ΔΛ(Ё.α,Ё.Ќ,Δ.ΕЊ,30,8,10)+"\n";}return s;}Ћ Ιβ(ό Δ,bool в,string І){Ћ ΰ=Ћ.None;if(Δ==null){for(int i=0;i<Н.Count;i++){Ћ Ї=Н[i].ĳ(І,false,в,false);if(Ї==Ћ.Ǆ) ΰ=Ї;if(Ї==Ћ.ΒΘ) return Ї;if(Ї==Ћ.Ɓ) return Ї;}return ΰ;}else return Δ.ĳ(І,false,в,false);}String Ż(ό Δ,String І)
{if(!ΖЋ(Δ,І)) return "";Ћ Ї=Ιβ(Δ,false,І);if(Ї==Ћ.Ǆ) return "received!";if(Ї==Ћ.Ɓ) return "pending...";if(Ї==Ћ.ΒΘ) return "no answer!";return "";}String ΔΞ(int Ƶ,String Γ,int Ʃ,ref int ĥ){String[]Π=Γ.Split('\n');if(Ʃ>=ĥ+Ƶ-1) ĥ++;ĥ=Math.Min(Π.Count()-1-Ƶ,ĥ);if(Ʃ<ĥ+1) ĥ--;
ĥ=Math.Max(0,ĥ);String ΰ="";for(int i=0;i<Ƶ;i++){int ΘΙ=i+ĥ;if(ΘΙ>=Π.Count()) break;ΰ+=Π[ΘΙ]+"\n";}return ΰ;}String ΖЁ(int ы,int Αη,String Γ){String[]Π=Γ.Split('\n');int Απ=ы*Αη;int ȗ=(ы+1)*Αη;String ΰ="";for(int i=Απ;i<ȗ;i++){if(i>=Π.Count()) break;ΰ+=Π[i]+"\n";
}return ΰ;}String Γθ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΗΔ(String s,int ΘΟ){if(s=="") return s;if(s.Length>ΘΟ) s=s.Substring(0,ΘΟ-1)+".";return s;}string ΔΛ(String Š,float value,float Ɯ,int ΗΡ,int ΘΔ,int Λα){float Ū=К(value,Ɯ)*ΗΡ;
String Βα="[";for(int i=0;i<ΗΡ;i++){if(i<=Ū) Βα+="|";else Βα+="'";}Βα+="]";return Βα+" "+ΗΔ(Γθ(Š),ΘΔ).PadRight(ΘΔ)+"".PadRight(Λα)+Ζί(value);}String Ζί(float Ќ){if(Ќ>=1000000) return Math.Round(Ќ/1000000f,Ќ/1000000f<100?1:0)+"M";if(Ќ>=1000) return Math.Round(Ќ/1000f,Ќ/1000f<100?1:0)+"K";
return ""+Math.Round(Ќ);}String ΑЍ(int Αν){if(Αν>=60*60) return Math.Round(Αν/(60f*60f),1)+" h";if(Αν>=60) return Math.Round(Αν/60f,1)+" min";return ""+Αν+" s";}string ź(float v){return Math.Round(v,2)+" ";}string ΜΝ(Vector3 v){return "X"+ź(v.X)+"Y"+ź(v.Y)+"Z"+ź(v.Z);}
List<IMyCameraBlock>Ȓ=new List<IMyCameraBlock>();List<long>ơ=new List<long>();bool ƣ=false;int Δϊ=0,Λο=0,Δρ=0;const int ΚΑ=16;int Δψ=0;DateTime ΗΥ=DateTime.Now;String Δς="";String Δχ="";void ΚΖ(){Ȓ.Clear();ơ.Clear();if(!obstacleBrake) return;φ.GetBlocksOfType(Ȓ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȓ.Count;i++) Ȓ[i].EnableRaycast=true;List<IMyTerminalBlock>Βέ=new List<IMyTerminalBlock>();φ.GetBlocksOfType(Βέ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<Βέ.Count;i++) if(!ơ.Contains(Βέ[i].CubeGrid.EntityId)) ơ.Add(Βέ[i].CubeGrid.EntityId);if(!ơ.Contains(Me.CubeGrid.EntityId)) ơ.Add(Me.CubeGrid.EntityId);
Δχ="";if(Ω==null) return;MatrixD m=Ω.WorldMatrix;Vector3[]ΚІ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]ΛΨ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ȓ.Count;i++)
if(Vector3.Dot(Ȓ[i].WorldMatrix.Forward,ΚІ[d])>0.7f){Δχ+=ΛΨ[d]+" ";break;}}bool Ζυ(Α s){return s==Α.Ȍ||s==Α.ц||s==Α.Й||
s==Α.ı||s==Α.Ź||s==Α.ΑЋ||
(s==Α.ȋ&&ǧ!=null);}void ΑІ(){ƣ=false;Δϊ=0;Δς="";}void Κδ(){if(!obstacleBrake||Ȓ.Count==0||!Ζυ(Ϋ)){ΑІ();return;}Vector3 ΑΦ=Ѓ+Ī;Vector3 ȣ=ΑΦ-δ;float Ȇ=ȣ.Length();if(Ȇ<1f){ΑІ();return;}Vector3 π=ȣ/Ȇ;Vector3 б=Ω!=null?(Vector3) Ω.GetShipVelocities().LinearVelocity:new Vector3();
if(б.Length()>2) π=Vector3.Normalize(б);Vector3 ΓΜ=Vector3.Cross(π,Math.Abs(π.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΓΜ.Normalize();Vector3 Ј=Vector3.Cross(ΓΜ,π);float υ=Math.Min(Ȇ+μ*0.25f,Ħ*obstacleLookAhead+μ);int ΗΨ=0;for(int k=0;k<2;k++){int p=k==0?0:Λο++%8,e=p%4;
Vector3 Λέ=k==0?new Vector3():(e==0?ΓΜ:e==1?-ΓΜ:e==2?Ј:-Ј)*μ*0.5f;Vector3 ȗ=δ+π*(p>=4?υ*0.5f:υ)+Λέ;IMyCameraBlock ĕ=ΙΙ(ȗ);if(ĕ==null){ΖΦ();continue;}ΗΨ++;MyDetectedEntityInfo ǐ=ĕ.Raycast(ȗ);bool ΛБ=ǐ.Type==MyDetectedEntityType.Planet||ǐ.Type==MyDetectedEntityType.Asteroid;
bool Θά=!ǐ.IsEmpty()&&!ơ.Contains(ǐ.EntityId)&&!Ιώ(ǐ.EntityId)&&
ǐ.Type!=MyDetectedEntityType.FloatingObject&&!(ΛБ&&(k>0||Ϋ==Α.Ȍ||
(ǐ.HitPosition.HasValue&&Vector3D.Distance(ǐ.HitPosition.Value,ΑΦ)<μ)));if(Θά){if(!ƣ) ΗΥ=DateTime.Now;ƣ=true;Δϊ=0;Δς=ǐ.Type.ToString();}else ΖΦ();if(debugDraw&&Βο.Count<16) Βο.Add(ο(ĕ.GetPosition())+","+ο(ȗ)+","+(Θά?1:0));}Δρ=ΗΨ>0?0:Δρ+1;if(Δρ>12) Κ="Cameras can't see ahead";
if(ƣ&&(DateTime.Now-ΗΥ).TotalSeconds>obstacleTimeout)
Κ="Path blocked: "+Δς;}void ΖΦ(){if(ƣ&&++Δϊ>=ΚΑ) ΑІ();}IMyCameraBlock ΙΙ(Vector3 ȗ){IMyCameraBlock Ъ=null;float ΗΤ=-2;for(int n=0;n<Ȓ.Count;n++){Δψ=(Δψ+1)%Ȓ.Count;IMyCameraBlock c=Ȓ[Δψ];if(!c.IsFunctional||!c.IsWorking) continue;float Θθ=Vector3.Dot(Vector3.Normalize(ȗ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Θθ>ΗΤ+0.05f&&c.CanScan(ȗ)){Ъ=c;ΗΤ=Θθ;}}return Ъ;}const String Δά="[PAMTFC]";const float ΚΥ=5f;const float Γυ=2f;class Ĵ{public long Ξ;public Vector3 έ,б;public Vector3 ț;public float Ημ;public String Μ="";public int ě=-1;public bool Αι;public int ǽ;public float ΑΨ,ΑΤ;public long β=0;
public int ΑΝ=0;public DateTime Ȣ;}IMyBroadcastListener ΓΨ=null;long љ=0;List<Ĵ>ћ=new List<Ĵ>();Vector3 Ī=new Vector3();float Ǜ=-1;Vector3 ΕЋ=new Vector3();bool ΓΧ=false;DateTime ΕЌ=DateTime.Now;const float ΗΓ=15f;const float ΚΤ=10f;int ΘЄ=0;String е="";int Д=-1;bool Βθ=false,Βη=false;
Vector3 Ηγ=new Vector3();String ž(){if(ζ==Σ.ш||Ν==Λ.ί) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ў+"|"+Ю+"|"+(int) ş+"|"+Math.Round(Ȉ,1)+"|"+Math.Round(ȥ,1);}void Κι(){if(!trafficEnabled) return;if(ΓΨ==null) ΓΨ=IGC.RegisterBroadcastListener(Δά);љ=Me.CubeGrid.EntityId;int ǡ=30;while(ΓΨ.HasPendingMessage&&ǡ-->0){String Η=ΓΨ.AcceptMessage().Data as String;if(Η!=null) ΙЈ(Η);
}for(int i=ћ.Count-1;i>=0;i--)
if((DateTime.Now-ћ[i].Ȣ).TotalSeconds>ΚΥ) ћ.RemoveAt(i);if(ΓΤ==null) ΓΤ=IGC.RegisterBroadcastListener(ΗΑ);ǡ=5;while(ΓΤ.HasPendingMessage&&ǡ-->0){String Η=ΓΤ.AcceptMessage().Data as String;if(Η!=null) try{ΙЉ(Η);}catch{}}if(Βι==null) Βι=IGC.RegisterBroadcastListener(Ζχ);
ǡ=10;while(Βι.HasPendingMessage&&ǡ-->0){String Η=Βι.AcceptMessage().Data as String;if(Η!=null) try{ΙЅ(Η);}catch{}}if(++ΘЄ<3) return;ΘЄ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",љ+";"+ο(Ζ.Ε)+";"+
ο(ζ==Σ.ш?new Vector3():Ζ.Ђ)+";"+ο(ć)+";"+
ο(Ѓ+Ī)+";"+String.Join(":",Βο));Βο.Clear();}IGC.SendBroadcastMessage(Δά,љ+";"+ο(δ)+";"+
ο(Ω.GetShipVelocities().LinearVelocity)+";"+μ+";"+ž()+";"+
(ζ==Σ.И?Д:-1)+";"+е+";"+ΙΞ()+","+ΖΫ()+";"+ο(ΕЋ)+
";"+(Βσ?1:0)+";"+(Ŀ&&ζ==Σ.И?1:0)+","+ΓΙ+","+Ευ+","+ΙЁ());}void ΙЈ(String Η){try{String[]f=Η.Split(';');if(f.Length<7) return;long Ξ=long.Parse(f[0]);if(Ξ==љ) return;Ĵ Δ=null;for(int i=0;i<ћ.Count;i++) if(ћ[i].Ξ==Ξ) Δ=ћ[i];if(Δ==null){Δ=new Ĵ();Δ.Ξ=Ξ;ћ.Add(Δ);}
Δ.έ=θ(f[1]);Δ.б=θ(f[2]);Δ.Ημ=float.Parse(f[3]);Δ.Μ=f[4];Δ.ě=int.Parse(f[5]);Δ.Ȣ=DateTime.Now;if(Δ.Μ!=""&&Δ.Μ==ž()) Ζύ(f[6]);Δ.β=0;Δ.ΑΝ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.β=long.Parse(d[0]);Δ.ΑΝ=int.Parse(d[1]);}Δ.ț=f.Length>=9?θ(f[8]):new Vector3();
Δ.Αι=f.Length>=10&&f[9]=="1";Δ.ǽ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ΑΨ=float.Parse(w[1]);Δ.ΑΤ=float.Parse(w[2]);Δ.ǽ=int.Parse(w[3]);}}}catch{}}bool Ιώ(long Ξ){for(int i=0;i<ћ.Count;i++) if(ћ[i].Ξ==Ξ) return true;return false;}Vector3 Ζη(int Β,int Λγ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();ά ή=Ψ[Β];Vector3 ΛЎ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Λγ;Vector3 Ј=ή.ω.LengthSquared()>0.01f?-Vector3.Normalize(ή.ω):-ή.ι;Vector3 у=Vector3.Cross(ΛЎ,Ј);if(у.LengthSquared()<0.0001f) return new Vector3();
float υ=laneOffset*μ;float ΚЏ=Math.Min(Vector3.Distance(ή.Ε,Ψ[0].Ε),
Vector3.Distance(ή.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(у)*υ*Math.Min(1,ΚЏ/(υ*4));}Vector3 ΘЃ=new Vector3();List<String>Βο=new List<String>();bool Ζξ(){Α s=Ϋ;return ž()!=""&&(s==Α.Ȍ||s==Α.ſ||s==Α.ȭ||
s==Α.Ȭ||s==Α.Ÿ);}int ΙЁ(){if(!Ζξ()) return 0;return Ϋ!=Α.Ȍ&&Vector3.Dot(δ-ј,Ǘ)>μ*0.5f?2:1;}bool Ιύ(Α s){return s==Α.ȋ||s==Α.ŉ||s==Α.ō||s==Α.ǅ||
s==Α.ǵ||s==Α.ȭ||s==Α.Ȭ||s==Α.Ÿ||s==Α.ĭ;}void Κκ(){Ī=new Vector3();Ǜ=-1;ΕЋ=new Vector3();bool ΑΧ=Ζυ(Ϋ);if(!trafficEnabled||(!ΑΧ&&!Ιύ(Ϋ))){ΓΧ=false;return;}ΘЃ=Ω.GetShipVelocities().LinearVelocity;ΙΠ(Ω.GetNaturalGravity(),!ΑΧ);ΘЎ();}IMyBroadcastListener ΓΤ=null;
float Ư=-1;DateTime ΘϏ=DateTime.MinValue;void ΙЉ(String Η){if(!Η.StartsWith("C;")) return;ΘϏ=DateTime.Now;Ư=-1;String Αγ=љ+":";String[]e=Η.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αγ)) Ư=float.Parse(e[i].Substring(Αγ.Length));}void ΘЎ(){if(Ư<0||(DateTime.Now-ΘϏ).TotalSeconds>1.5) return;
Ǜ=Ǜ<0?Ư:Math.Min(Ǜ,Ư);if(Ư<0.5f) Κ="Traffic: giving way (controller)";}void ΙΠ(Vector3 ω,bool Θα=false){Ī=new Vector3();Ǜ=-1;Vector3 ȣ=Ѓ-δ;float Ȇ=ȣ.Length();bool ΕΖ=Ȇ>=1f;Vector3 π=ΕΖ?ȣ/Ȇ:new Vector3();ΕЋ=π;Vector3 Ј=ω.LengthSquared()>0.01f?-Vector3.Normalize(ω):-ΑΟ;
Vector3 у=Vector3.Cross(π,Ј);у=у.LengthSquared()<0.0001f?-Αΰ:Vector3.Normalize(у);Ĵ ΒЅ=null;bool Ηό=Ϋ==Α.Й&&!ē(1);for(int i=0;i<ћ.Count;i++){Ĵ λ=ћ[i];if(λ.ΑΝ==3&&λ.б.LengthSquared()<0.25f) continue;Vector3 έ=λ.έ+λ.б*(float)(DateTime.Now-λ.Ȣ).TotalSeconds;Vector3 ǩ=έ-δ;
float в=(μ+λ.Ημ)*0.5f;bool Αλ=λ.Μ!=""&&λ.Μ==ž();if(Αλ&&λ.ǽ==2) continue;bool ΓΝ=Αλ&&λ.ǽ==1&&Ζξ();if(ΓΝ) в=Math.Min(Ȉ,ȥ)*0.9f;float ΔϏ=в*(ΓΝ?1f:1.2f);float ΑΑ=ΕΖ?Vector3.Dot(ǩ,π):0;if(ΑΑ>0&&(ǩ-π*ΑΑ).Length()<=ΔϏ){float ΓΒ=Vector3.Dot(λ.б,π);
bool ΗЀ=λ.ț.LengthSquared()>0.25f;bool ΗЂ=ΓΒ<-1f||(ΗЀ&&Vector3.Dot(λ.ț,π)<-0.5f);if(!Θα){if(λ.Αι){if(!Ηό){Ī=у*в*1.3f;continue;}float ΘΥ=Vector3.Distance(δ,Ѓ),ΗБ=Vector3.Distance(έ,Ѓ);if(ΘΥ<ΗБ||(ΘΥ==ΗБ&&љ<λ.Ξ)) continue;}if(ΗЂ){if(ΑΑ<(Ħ-ΓΒ)*6f+в*4f) Ī=у*в*1.3f;
continue;}if(ΗЀ&&љ<λ.Ξ){Vector3 ΗΟ=-ǩ;float Θί=Vector3.Dot(ΗΟ,λ.ț);if(Θί>0&&(ΗΟ-λ.ț*Θί).Length()<=ΔϏ){Ī=у*в*1.3f;continue;}}}float Ηϋ=ΑΑ-в-(ΓΝ?1f:followGap*μ);float ǿ=Math.Max(0,ΓΒ)+Math.Max(0,Ηϋ)/Γυ;if(ΗЂ) ǿ=Math.Max(0,Ηϋ+Math.Min(0,ΓΒ)*Γυ*2)/Γυ;
if(Ǜ<0||ǿ<Ǜ){Ǜ=ǿ;ΒЅ=λ;}continue;}if(λ.б.LengthSquared()<1f) continue;if(ΕΖ&&(ǩ-π*ΑΑ).Length()<=ΔϏ) continue;Vector3 Εσ=λ.б-ΘЃ;float Θη=Εσ.LengthSquared();if(Θη<0.25f) continue;float t=-Vector3.Dot(ǩ,Εσ)/Θη;if(t<=0||t>5f||(ǩ+Εσ*t).Length()>=(ΓΝ?в:в*1.1f)) continue;
if(љ>λ.Ξ||t<2f){Ǜ=0;ΒЅ=λ;}}bool Αά=Ǜ>=0&&Ǜ<0.5f;if(!Αά||Θα||(ΒЅ!=null&&ΒЅ.Αι&&Ηό)){ΓΧ=false;return;}if(!ΓΧ){ΓΧ=true;ΕЌ=DateTime.Now;}double ΗЃ=(DateTime.Now-ΕЌ).TotalSeconds;if(ΗЃ>ΗΓ+ΚΤ) ΕЌ=DateTime.Now;else if(ΗЃ>ΗΓ){Ǜ=-1;Ī=у*μ*1.3f;Κ="Traffic: passing a stuck ship";}
}List<int>ă=new List<int>();String ΗЅ="";void Γτ(){String ъ=Ў+"|"+Ю+"|"+(int) ş;if(ъ==ΗЅ) return;ΗЅ=ъ;ΖΜ(Ў,Ю,ş,ă);}void ΖΜ(int w,int h,ђ Απ,List<int>Δω){Δω.Clear();if(Απ==ђ.ǉ){for(int k=0;k<w*h;k++){int Ņ=k/w;Δω.Add(Ņ*w+(Ņ%2==0?k-Ņ*w:w-1-(k-Ņ*w)));
}return;}int ΑΪ=(int) Math.Ceiling(w/2f),ΑΩ=(int) Math.Ceiling(h/2f);int ΕΓ=w/2,ΕΒ=h/2;int x=0,y=0,ΔЈ=0,ΔЉ=-1,Ɯ=Math.Max(w,h);for(int i=0;i<Ɯ*Ɯ;i++){if(-ΑΪ<x&&x<=ΕΓ&&-ΑΩ<y&&y<=ΕΒ) Δω.Add((y-1+ΑΩ)*w+(x-1+ΑΪ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔЈ;ΔЈ=-ΔЉ;ΔЉ=t;}
x+=ΔЈ;y+=ΔЉ;}}bool Γσ(){int n=Math.Max(0,Ў*Ю);if(е.Length==n) return false;е=new String('0',n);Д=-1;return true;}void Ζύ(String λ){Γσ();if(λ.Length!=е.Length) return;char[]h=е.ToCharArray();for(int i=0;i<h.Length;i++) if(λ[i]=='1') h[i]='1';е=new String(h);}bool ΒΑ(int Ы){return Ы>=0&&Ы<е.Length&&е[Ы]=='1';}
void ΔΡ(int Ы,bool ύ){if(Ы<0||Ы>=е.Length) return;char[]h=е.ToCharArray();h[Ы]=ύ?'1':'0';е=new String(h);}bool ΓЀ(int Ы,bool ΛΜ){if(!trafficEnabled) return false;String ъ=ž();for(int i=0;i<ћ.Count;i++){Ĵ t=ћ[i];if(t.Μ!=ъ||t.ě<0||(ΛΜ&&t.Ξ>љ)) continue;if(t.ě==Ы) return true;
}return false;}int ΙΝ(int Β){int n=Ў*Ю;if(n<1) return Β;Γτ();if(Γσ()) for(int k=0;k<Β&&k<n;k++) ΔΡ(ă[k],true);if(Д>=0&&(Β>=n||ă[Β]!=Д)){ΔΡ(Д,true);Д=-1;}if(Ζψ()){if(Ȕ>=0&&!ΒΑ(Ȕ)){Ŀ=false;Д=Ȕ;return ă.IndexOf(Ȕ);}if(Ȕ==-2){Д=-1;Ŀ=е.IndexOf('0')>=0;return Ŀ?-2:n;}
if(Ȕ==-1&&Д>=0&&!ΒΑ(Д)){Ŀ=false;return Β;}Д=-1;if(!Ŀ){Ŀ=true;Ηε=DateTime.Now;}if((DateTime.Now-Ηε).TotalSeconds<5) return-1;}Ŀ=false;if(Д>=0&&!ΒΑ(Д)&&!ΓЀ(Д,true)) return Β;Д=-1;for(int k=0;k<n;k++){int c=ă[k];if(ΒΑ(c)||ΓЀ(c,false)) continue;Д=c;
return k;}return е.IndexOf('0')>=0?-2:n;}const String Ζχ="[PAMJOB]";const String ΗΑ="[PAMTCC]";IMyBroadcastListener Βι=null;DateTime Ηδ=DateTime.MinValue;DateTime Ηε=DateTime.Now;int Ȕ=-1;bool Ŀ=false;bool Ζψ(){return trafficEnabled&&Ȕ!=-3&&(DateTime.Now-Ηδ).TotalSeconds<3;}
void ΙЅ(String Η){String[]f=Η.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ž()) return;Ηδ=DateTime.Now;Ζύ(f[2]);Ȕ=-3;String Αγ=љ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αγ)) Ȕ=int.Parse(e[i].Substring(Αγ.Length));}Ǵ ΚΛ(int Β)
{Γτ();if(Β<0||Β>=ă.Count) return Ǵ.ΑЀ;Є=ă[Β]%Ў;Ѐ=ă[Β]/Ў;return Ǵ.ΑЎ;}class ǟ{public long Ξ;public Vector3 έ,Э,Ј;}List<ǟ>Х=new List<ǟ>();bool Ļ=false;Vector3 Βυ,Βτ,Βφ,Βχ;long ǒ=0;long ĉ=0;ά ǧ=null;bool ΑΞ=false;ά Γπ(Vector3 έ,Vector3 Э,Vector3 Ј){MatrixD m=MatrixD.CreateWorld(έ,Э,Ј);
ά w=new ά(Ρ);w.κ=Vector3D.TransformNormal(Βυ,m);w.ι=Vector3D.TransformNormal(Βτ,m);w.Ђ=Vector3D.TransformNormal(Βφ,m);w.Ε=έ+(Vector3) Vector3D.TransformNormal(Βχ,m);w.ν=true;return w;}int ΖΫ(){Α s=Ϋ;if(s==Α.ё) return й(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ō||s==Α.Ȱ||s==Α.Ō||s==Α.ΒΪ||
s==Α.ΑЉ||s==Α.ǵ) return 3;if(s==Α.ȋ||s==Α.ŉ||s==Α.ǅ) return 2;if(s==Α.Й) return 1;return 0;}long ΙΞ(){if(ΖΫ()==0) return 0;if(ē(1)&&Ƭ!=0) return Ƭ;return ǧ!=null?ĉ:ǒ;}void Ζω(){if(Ν==Λ.ί||!Ρ.ν) return;IMyShipConnector Р=й(MyShipConnectorStatus.Connected);if(Р==null||Р.OtherConnector==null) return;
IMyShipConnector λ=Р.OtherConnector;Vector3I ΛΖ=new Vector3I((int) Ρ.с.X,(int) Ρ.с.Y,(int) Ρ.с.Z);if(Р.Position==ΛΖ){MatrixD ƕ=MatrixD.Transpose(λ.WorldMatrix);Βυ=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,ƕ);Βτ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,ƕ);Βφ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,ƕ);
Βχ=Vector3D.TransformNormal(Р.GetPosition()-λ.GetPosition(),ƕ);Ļ=true;}if(Vector3.Distance(Р.GetPosition(),Ρ.Ε)<5) ǒ=λ.EntityId;List<IMyShipConnector>ś=new List<IMyShipConnector>();φ.GetBlocksOfType(ś,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ś.Count>0){Х.Clear();for(int i=0;i<ś.Count;i++){ǟ p=new ǟ();p.Ξ=ś[i].EntityId;p.έ=ś[i].GetPosition();p.Э=ś[i].WorldMatrix.Forward;p.Ј=ś[i].WorldMatrix.Up;Х.Add(p);}}ǧ=null;ĉ=0;if(λ.EntityId==ǒ||!Ļ) return;for(int i=0;i<Х.Count;i++)
if(Х[i].Ξ==λ.EntityId){ĉ=Х[i].Ξ;ǧ=Γπ(Х[i].έ,Х[i].Э,Х[i].Ј);}}bool ΓЇ(long Ξ,int ΘΨ){if(Ξ==0) return false;for(int i=0;i<ћ.Count;i++){Ĵ t=ћ[i];if(t.β==Ξ&&(t.ΑΝ>ΘΨ||(t.ΑΝ==ΘΨ&&t.Ξ<љ))) return true;}return false;}bool Γκ(int ũ){if(Ν==Λ.ί||!trafficEnabled){ǧ=null;ĉ=0;return true;}
long ς=ǧ!=null?ĉ:ǒ;if(!ΓЇ(ς,ũ)) return true;if(!ΓЇ(ǒ,ũ)){ǧ=null;ĉ=0;return true;}if(!Ļ) return false;ǟ Ъ=null;float ΑΗ=0;for(int i=0;i<Х.Count;i++){ǟ p=Х[i];if(p.Ξ==ǒ||ΓЇ(p.Ξ,ũ)) continue;float d=Vector3.Distance(p.έ,Ρ.Ε);if(Ъ==null||d<ΑΗ){Ъ=p;ΑΗ=d;}
}if(Ъ==null) return false;ĉ=Ъ.Ξ;ǧ=Γπ(Ъ.έ,Ъ.Э,Ъ.Ј);return true;}String ΙΫ(){String s=(Ļ?"1":"0")+":"+ο(Βυ)+":"+ο(Βτ)+":"+
ο(Βφ)+":"+ο(Βχ)+":"+ǒ;for(int i=0;i<Х.Count;i++)
s+=":"+Х[i].Ξ+","+ο(Х[i].έ)+","+ο(Х[i].Э)+","+ο(Х[i].Ј);return s;}void ΙΪ(String s){Х.Clear();Ļ=false;ǒ=0;String[]f=s.Split(':');if(f.Length<6) return;Ļ=f[0]=="1";Βυ=θ(f[1]);Βτ=θ(f[2]);Βφ=θ(f[3]);Βχ=θ(f[4]);long.TryParse(f[5],out ǒ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǟ p=new ǟ();if(!long.TryParse(e[0],out p.Ξ)) continue;p.έ=θ(e[1]);p.Э=θ(e[2]);p.Ј=θ(e[3]);Х.Add(p);}}const String ƻ="[PAMDCK]";const float ΙΔ=120f;class ď{public long Δ;public int ǥ;public long Ǒ;public bool ΗΧ;public DateTime ǣ,Ƙ;
}IMyBroadcastListener Βς=null;List<ď>Ц=new List<ď>();List<IMyShipConnector>ħ=new List<IMyShipConnector>();List<IMySensorBlock>Ȋ=new List<IMySensorBlock>();List<IMyCameraBlock>Ġ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Γή=new List<MyDetectedEntityInfo>();long ѓ=0;int Δϋ=0;
long Βγ=0;DateTime ΗΪ=DateTime.Now;String ř="";int Ηρ=0;IMyCubeGrid Ĩ=null;void ΚΘ(){Ĩ=Me.CubeGrid;φ.GetBlocksOfType(ħ,q=>q.CubeGrid.IsSameConstructAs(Ĩ)&&q.CustomName.Contains(dockPoolTag));φ.GetBlocksOfType(Ȋ,q=>q.CubeGrid.IsSameConstructAs(Ĩ)&&q.CustomName.Contains(zoneTag));
φ.GetBlocksOfType(Ġ,q=>q.CubeGrid.IsSameConstructAs(Ĩ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȋ.Count;i++){Ȋ[i].DetectSmallShips=true;Ȋ[i].DetectLargeShips=true;}for(int i=0;i<Ġ.Count;i++) Ġ[i].EnableRaycast=true;}IMyShipConnector ΔΩ(long Ξ){if(Ξ==0) return null;
IMyShipConnector c=φ.GetBlockWithId(Ξ) as IMyShipConnector;return c!=null&&Ĩ!=null&&c.CubeGrid.IsSameConstructAs(Ĩ)?c:null;}bool ΓІ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Ζφ(IMyShipConnector c,long Δ){for(int i=0;i<Ġ.Count;i++)
{IMyCameraBlock ĕ=Ġ[i];if(!ĕ.IsWorking||Vector3D.Distance(ĕ.GetPosition(),c.GetPosition())>10) continue;Vector3D ȗ=ĕ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ĕ.CanScan(ȗ)) continue;MyDetectedEntityInfo ǐ=ĕ.Raycast(ȗ);if(!ǐ.IsEmpty()&&ǐ.EntityId!=Ĩ.EntityId&&ǐ.EntityId!=Δ&&ǐ.Type!=MyDetectedEntityType.FloatingObject)
{ř="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙϏ(){for(int s=0;s<Ȋ.Count;s++){if(!Ȋ[s].IsWorking) continue;Γή.Clear();Ȋ[s].DetectedEntities(Γή);for(int i=0;i<Γή.Count;i++){MyDetectedEntityInfo e=Γή[i];bool ΛΑ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΛΑ&&e.EntityId!=ѓ&&e.Velocity.Length()>0.5f){ř="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΙЊ(ď r){IMyShipConnector Ǒ=ΔΩ(r.Ǒ);if(ΓІ(Ǒ)&&Ζφ(Ǒ,r.Δ)) return Ǒ.EntityId;if(!r.ΗΧ||Ǒ==null) return 0;IMyShipConnector Ъ=null;double ΑΗ=0;for(int i=0;i<ħ.Count;i++)
{IMyShipConnector c=ħ[i];if(!ΓІ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),Ǒ.GetPosition());if(Ъ==null||d<ΑΗ){if(Ζφ(c,r.Δ)){Ъ=c;ΑΗ=d;}}}return Ъ==null?0:Ъ.EntityId;}void Ιο(){ď Ƥ=null;long Р=0;for(int ŧ=2;ŧ>=1&&Ƥ==null;ŧ--){ď Ŧ=null;for(int i=0;i<Ц.Count;i++)
{ď r=Ц[i];if(r.ǥ!=ŧ) continue;if(ŧ==1){long c=ΙЊ(r);if(c==0) continue;if(Ŧ==null||r.ǣ<Ŧ.ǣ){Ŧ=r;Р=c;}}else if(Ŧ==null||r.ǣ<Ŧ.ǣ) Ŧ=r;}Ƥ=Ŧ;}if(Ƥ==null){if(Ц.Count>0&&ř=="") ř="No free dock";return;}ѓ=Ƥ.Δ;Δϋ=Ƥ.ǥ;Βγ=Ƥ.ǥ==1?Р:0;ΗΪ=DateTime.Now;
}void Κή(){if(Ĩ==null) return;if(Βς==null) Βς=IGC.RegisterBroadcastListener(ƻ);int ǡ=30;while(Βς.HasPendingMessage&&ǡ-->0){String Η=Βς.AcceptMessage().Data as String;if(Η!=null) Ιρ(Η);}ΙБ();if(++Ηρ<3) return;Ηρ=0;IGC.SendBroadcastMessage(ƻ,ΙΒ());}void Ιρ(String Η){try{String[]f=Η.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ц.Count-1;i>=0;i--) if(Ц[i].Δ==Δ) Ц.RemoveAt(i);if(ѓ==Δ) ѓ=0;}else if(f[0]=="R"&&f.Length>=5){long Ǒ=long.Parse(f[3]);if(ΔΩ(Ǒ)==null) return;ď r=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==Δ) r=Ц[i];if(r==null){r=new ď();r.Δ=Δ;r.ǣ=DateTime.Now;Ц.Add(r);}
int ǥ=int.Parse(f[2]);if(r.ǥ!=ǥ) r.ǣ=DateTime.Now;r.ǥ=ǥ;r.Ǒ=Ǒ;r.ΗΧ=f[4]=="1";r.Ƙ=DateTime.Now;}}catch{}}void ΙБ(){for(int i=Ц.Count-1;i>=0;i--)
if((DateTime.Now-Ц[i].Ƙ).TotalSeconds>3) Ц.RemoveAt(i);ď Ββ=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==ѓ) Ββ=Ц[i];if(ѓ!=0&&Ββ==null) ѓ=0;if(Ββ!=null&&(DateTime.Now-ΗΪ).TotalSeconds>ΙΔ){ѓ=0;Ββ.ǣ=DateTime.Now;}ř="";if(ѓ==0&&Ц.Count>0&&!ΙϏ()) Ιο();}
String ΙΒ(){String γ="S;"+Ĩ.EntityId+";"+ѓ+";"+Δϋ+";"+Βγ+";";for(int i=0;i<Ц.Count;i++) γ+=(i>0?",":"")+Ц[i].Δ;γ+=";";for(int i=0;i<ħ.Count;i++){IMyShipConnector c=ħ[i];γ+=(i>0?":":"")+c.EntityId+","+ο(c.GetPosition())+","+ο(c.WorldMatrix.Forward)+","+ο(c.WorldMatrix.Up);
}γ+=";";for(int i=0;i<Ȋ.Count;i++){IMySensorBlock z=Ȋ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();γ+=(i>0?":":"")+ο(z.GetPosition())+","+Math.Round(r,1);}return γ;}String Ιδ()
{if(ħ.Count==0&&Ȋ.Count==0) return "";int ΑΧ=0;for(int i=0;i<ħ.Count;i++) if(ΓІ(ħ[i])) ΑΧ++;return "Docks: "+ΑΧ+"/"+ħ.Count+" free"+(ѓ!=0?(Δϋ==1?" | docking":" | undocking"):"")+
(Ц.Count>1?" | "+(Ц.Count-1)+" waiting":"")+(ř!=""?"\n"+ř:"")+"\n";}IMyBroadcastListener ΓΡ=null;int Ă=0;DateTime Ηπ=DateTime.Now;DateTime Θο=DateTime.MinValue;long Θπ=0;int Θρ=0;long Ƭ=0;bool Αό=false;int ΔЇ=0;List<Vector3>Αώ=new List<Vector3>();List<float>ΖΘ=new List<float>();
bool Βσ=false;bool Βρ=false;Vector3 Ηο=new Vector3();bool ǈ(){return Ă!=0&&(DateTime.Now-Θο).TotalSeconds<3;}bool ē(int ǥ){return ǈ()&&Θπ==љ&&Θρ==ǥ;}bool ΖЎ(){return ǈ()||(DateTime.Now-Ηπ).TotalSeconds<3;}void ΔΝ(int ǥ){if(Ă==ǥ) return;Ă=ǥ;Ηπ=DateTime.Now;ΔЇ=3;
}void ΔΜ(){if(Ă!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƻ,"D;"+љ);Ă=0;Αό=false;}void Κέ(){if(!trafficEnabled||Ν==Λ.ί) return;if(ΓΡ==null) ΓΡ=IGC.RegisterBroadcastListener(ƻ);int ǡ=20;while(ΓΡ.HasPendingMessage&&ǡ-->0){String Η=ΓΡ.AcceptMessage().Data as String;if(Η==null||!Η.StartsWith("S;")) continue;
try{ΙЇ(Η);}catch{}}if(Ϋ!=Α.Й){Βσ=false;Βρ=false;}if(Ă==1&&й(MyShipConnectorStatus.Connected)!=null) ΔΜ();if(Ă==2&&!Αό&&Ϋ!=Α.ǵ&&
Vector3.Distance(δ,ѐ().Ε)>μ*3) ΔΜ();if(Ă!=0&&++ΔЇ>=3){ΔЇ=0;IGC.SendBroadcastMessage(ƻ,"R;"+љ+";"+Ă+";"+ǒ+";"+(Ļ?1:0));}}void ΙЇ(String Η){String[]f=Η.Split(';');if(f.Length<7) return;bool Ηϊ=false;String[]ΗΖ=f[5].Split(',');for(int i=0;i<ΗΖ.Length;i++) if(ΗΖ[i]==љ.ToString()) Ηϊ=true;
if(!Ηϊ) return;Θο=DateTime.Now;Θπ=long.Parse(f[2]);Θρ=int.Parse(f[3]);Ƭ=long.Parse(f[4]);Αώ.Clear();ΖΘ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αώ.Add(θ(e[0]));ΖΘ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ησ=f[6].Split(':');List<ǟ>Џ=new List<ǟ>();for(int i=0;i<Ησ.Length;i++){String[]e=Ησ[i].Split(',');if(e.Length<4) continue;ǟ p=new ǟ();p.Ξ=long.Parse(e[0]);p.έ=θ(e[1]);p.Э=θ(e[2]);p.Ј=θ(e[3]);Џ.Add(p);}Х=Џ;}bool ΙΛ(){if(!trafficEnabled||Ν==Λ.ί||ǒ==0) return true;
ΔΝ(1);if(!ΖЎ()) return true;if(!ē(1)){Κ="Waiting for dock clearance";return false;}Γβ();return true;}bool Γβ(){ΑΞ=false;ǟ Ũ=null;if(Ƭ!=0&&Ƭ!=ǒ&&Ļ)
for(int i=0;i<Х.Count;i++) if(Х[i].Ξ==Ƭ) Ũ=Х[i];long ƶ=Ũ==null?0:Ũ.Ξ;if(ƶ==ĉ&&(ƶ==0)==(ǧ==null)) return false;ĉ=ƶ;ǧ=Ũ==null?null:Γπ(Ũ.έ,Ũ.Э,Ũ.Ј);return true;}bool Ž(Vector3 p){for(int i=0;i<Αώ.Count;i++) if(Vector3.Distance(p,Αώ[i])<ΖΘ[i]+μ*0.75f) return true;
return false;}bool Ιυ(Vector3 ΑΛ,Vector3 ť){Βσ=false;if(!trafficEnabled||Ν==Λ.ί||ǒ==0) return false;if(Ă!=1&&Vector3.Distance(δ,Ρ.Ε)>dockRequestDistance) return false;ΔΝ(1);if(!ǈ()||ē(1)||Αώ.Count==0) return false;if(!Ž(ΑΛ)&&!Ž(ť)&&!Ž(δ)) return false;Βσ=true;return true;
}bool ΖΤ(){if(!trafficEnabled||Ν==Λ.ί||ǒ==0) return true;ΔΝ(2);if(!ΖЎ()) return true;if(!ē(2)){Κ="Waiting for undock clearance";return false;}return true;}const float ΙЀ=600f;class ȫ{public long Ξ;public int Ы=-1;public bool ƶ;public float ΑΨ,ΑΤ;public DateTime Ετ;}class ȍ{public String ъ;
public int w,h;public float ΕЎ,ΕЂ;public String ύ;public List<int>Εδ=new List<int>();public List<ȫ>ф=new List<ȫ>();}List<ȍ>Ǔ=new List<ȍ>();IMyBroadcastListener ΒЄ=null;int ΘΒ=0;void Κβ(){if(ΒЄ==null) ΒЄ=IGC.RegisterBroadcastListener(Δά);int ǡ=50;while(ΒЄ.HasPendingMessage&&ǡ-->0)
{String Η=ΒЄ.AcceptMessage().Data as String;if(Η==null) continue;try{Ισ(Η);}catch{}try{Ιτ(Η);}catch{}}for(int j=Ǔ.Count-1;j>=0;j--){ȍ Μ=Ǔ[j];for(int i=Μ.ф.Count-1;i>=0;i--)
if((DateTime.Now-Μ.ф[i].Ετ).TotalSeconds>ΙЀ) Μ.ф.RemoveAt(i);if(Μ.ф.Count==0){Ǔ.RemoveAt(j);continue;}ΘЏ(Μ);}if(++ΘΒ<3) return;ΘΒ=0;for(int j=0;j<Ǔ.Count;j++) IGC.SendBroadcastMessage(Ζχ,ΙΑ(Ǔ[j]));}void ΘЏ(ȍ Μ){for(int i=0;i<Μ.ф.Count;i++){ȫ s=Μ.ф[i];if(s.ƶ&&(s.Ы<0||Μ.ύ[s.Ы]=='1')) s.Ы=ΙЃ(Μ,s);
}}String ΙΑ(ȍ Μ){String γ="J;"+Μ.ъ+";"+Μ.ύ+";";bool ǣ=true;for(int i=0;i<Μ.ф.Count;i++){if((DateTime.Now-Μ.ф[i].Ετ).TotalSeconds>5) continue;γ+=(ǣ?"":",")+Μ.ф[i].Ξ+":"+Μ.ф[i].Ы;ǣ=false;}return γ;}void Ισ(String Η){String[]f=Η.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȍ Μ=null;for(int i=0;i<Ǔ.Count;i++) if(Ǔ[i].ъ==f[4]) Μ=Ǔ[i];if(Μ==null){Μ=new ȍ();Μ.ъ=f[4];Μ.w=int.Parse(k[3]);Μ.h=int.Parse(k[4]);Μ.ΕЎ=float.Parse(k[6]);Μ.ΕЂ=float.Parse(k[7]);if(Μ.w<1||Μ.h<1) return;ΖΜ(Μ.w,Μ.h,(ђ) int.Parse(k[5]),Μ.Εδ);
Μ.ύ=new String('0',Μ.w*Μ.h);Ǔ.Add(Μ);}String d=f[6];if(d.Length==Μ.ύ.Length){char[]c=Μ.ύ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Μ.ύ=new String(c);}long Ξ=long.Parse(f[0]);ȫ s=null;for(int i=0;i<Μ.ф.Count;i++) if(Μ.ф[i].Ξ==Ξ) s=Μ.ф[i];
if(s==null){s=new ȫ();s.Ξ=Ξ;Μ.ф.Add(s);}s.Ετ=DateTime.Now;String[]w=f[10].Split(',');s.ƶ=w[0]=="1";if(w.Length>=3){s.ΑΨ=float.Parse(w[1]);s.ΑΤ=float.Parse(w[2]);}int ě=int.Parse(f[5]);if(ě>=0&&ě<Μ.ύ.Length&&(s.Ы<0||Μ.ύ[s.Ы]=='1')&&Μ.ύ[ě]!='1'&&ΓЁ(Μ,ě,s)==null)
s.Ы=ě;}ȫ ΓЁ(ȍ Μ,int Ы,ȫ ΚЌ){for(int i=0;i<Μ.ф.Count;i++){ȫ o=Μ.ф[i];if(o!=ΚЌ&&o.Ы==Ы)
return o;}return null;}int ΙЃ(ȍ Μ,ȫ s){for(int k=0;k<Μ.Εδ.Count;k++){int c=Μ.Εδ[k];if(Μ.ύ[c]!='1'&&ΓЁ(Μ,c,s)==null) return c;}return-2;}String Ιι(){String s="";for(int j=0;j<Ǔ.Count;j++){ȍ Μ=Ǔ[j];int ύ=0;for(int i=0;i<Μ.ύ.Length;i++) if(Μ.ύ[i]=='1') ύ++;s+="Job "+(j+1)+": "+ύ+"/"+Μ.ύ.Length+" holes, "+Μ.ф.Count+" ships\n";
}return s;}const float ΚΡ=8f;class п{public long Ξ;public Vector3 έ,б,ț;public float Ə,ΑΨ,ΑΤ,ǿ=-1;public int ǽ;public String Μ="";public int ũ;public DateTime Ȣ;}class ΒΥ{public long a,b,ΘЍ;public DateTime Ƙ;}List<п>σ=new List<п>();List<ΒΥ>ǚ=new List<ΒΥ>();int ΕЁ=0;void Ιτ(String Η)
{String[]f=Η.Split(';');if(f.Length<9) return;long Ξ=long.Parse(f[0]);п s=null;for(int i=0;i<σ.Count;i++) if(σ[i].Ξ==Ξ) s=σ[i];if(s==null){s=new п();s.Ξ=Ξ;σ.Add(s);}s.έ=θ(f[1]);s.б=θ(f[2]);s.Ə=float.Parse(f[3]);s.Μ=f[4];s.ũ=int.Parse(f[7].Split(',')[1]);s.ț=θ(f[8]);
s.ǽ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΨ=float.Parse(w[1]);s.ΑΤ=float.Parse(w[2]);s.ǽ=int.Parse(w[3]);}}s.Ȣ=DateTime.Now;}bool ΒΦ(п s){return s.б.LengthSquared()<0.25f&&(s.ũ==3||s.ț.LengthSquared()<0.25f);}Vector3 ΗΒ(п s){return s.б.LengthSquared()>=1f?s.б:s.ț*2f;}
void Κλ(){ΙΡ();if(++ΕЁ<3) return;ΕЁ=0;IGC.SendBroadcastMessage(ΗΑ,ΙΓ());}void ΙΡ(){for(int i=σ.Count-1;i>=0;i--)
if((DateTime.Now-σ[i].Ȣ).TotalSeconds>3) σ.RemoveAt(i);for(int i=ǚ.Count-1;i>=0;i--)
if((DateTime.Now-ǚ[i].Ƙ).TotalSeconds>2) ǚ.RemoveAt(i);for(int i=0;i<σ.Count;i++) σ[i].ǿ=-1;for(int i=0;i<σ.Count;i++)
for(int j=i+1;j<σ.Count;j++){п a=σ[i],b=σ[j];if(ΒΦ(a)||ΒΦ(b)) continue;Vector3 Λΰ=a.έ+a.б*(float)(DateTime.Now-a.Ȣ).TotalSeconds;Vector3 Λδ=b.έ+b.б*(float)(DateTime.Now-b.Ȣ).TotalSeconds;Vector3 Ʊ=ΗΒ(a),Ʋ=ΗΒ(b);Vector3 ǩ=Λδ-Λΰ;float υ=ǩ.Length();float в=(a.Ə+b.Ə)*0.55f;
bool Αλ=a.Μ!=""&&a.Μ==b.Μ;if(Αλ&&(a.ǽ==2||b.ǽ==2)) continue;if(Αλ&&a.ǽ==1&&b.ǽ==1){String[]k=a.Μ.Split('|');if(k.Length>=8) в=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Ες=Ʋ-Ʊ;float Θζ=Ες.LengthSquared();float t=Θζ>0.01f?-Vector3.Dot(ǩ,Ες)/Θζ:0;
bool ΕЉ=υ<в;if(!ΕЉ&&(t<=0||t>ΚΡ||(ǩ+Ες*t).Length()>=в)) continue;if(!ΕЉ&&Ʊ.LengthSquared()>0.25f&&Ʋ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ʊ),Vector3.Normalize(Ʋ))<-0.7f)
continue;ΒΥ p=null;for(int k=0;k<ǚ.Count;k++)
if((ǚ[k].a==a.Ξ&&ǚ[k].b==b.Ξ)||(ǚ[k].a==b.Ξ&&ǚ[k].b==a.Ξ)) p=ǚ[k];if(p==null){p=new ΒΥ();p.a=a.Ξ;p.b=b.Ξ;p.ΘЍ=ΚΣ(a,b,Ʊ,Ʋ,ǩ,υ);ǚ.Add(p);}p.Ƙ=DateTime.Now;п y=p.ΘЍ==a.Ξ?a:b;float ΘΘ=ΕЉ||t<4?0:Math.Max(1,y.б.Length()*0.5f);if(y.ǿ<0||ΘΘ<y.ǿ) y.ǿ=ΘΘ;
}}long ΚΣ(п a,п b,Vector3 Ʊ,Vector3 Ʋ,Vector3 ǩ,float υ){if(a.Ξ==ѓ) return b.Ξ;if(b.Ξ==ѓ) return a.Ξ;Vector3 n=υ>0.01f?ǩ/υ:new Vector3(1,0,0);float Θσ=Vector3.Dot(Ʊ,n),Θώ=Vector3.Dot(Ʋ,-n);if(Θσ-Θώ>1) return a.Ξ;if(Θώ-Θσ>1) return b.Ξ;return a.Ξ>b.Ξ?a.Ξ:b.Ξ;
}String ΙΓ(){String γ="C;";bool ǣ=true;for(int i=0;i<σ.Count;i++){if(σ[i].ǿ<0) continue;γ+=(ǣ?"":",")+σ[i].Ξ+":"+Math.Round(σ[i].ǿ,1);ǣ=false;}return γ;}String Ιξ(){int ΘΧ=0,Αά=0;for(int i=0;i<σ.Count;i++){if(!ΒΦ(σ[i])) ΘΧ++;if(σ[i].ǿ>=0) Αά++;}if(σ.Count==0) return "";
return "Traffic: "+ΘΧ+" moving"+(Αά>0?", "+Αά+" giving way":"")+"\n";}List<ό>Ą=new List<ό>();ό ǻ=null;Vector3 ΗЉ;DateTime ΗЈ;float ΗЇ=40;bool ΗІ=false;п Ζή(ό s){п Ъ=null;for(int i=0;i<σ.Count;i++)
if(Vector3.Distance(σ[i].έ,s.Ε)<15&&(Ъ==null||
Vector3.Distance(σ[i].έ,s.Ε)<Vector3.Distance(Ъ.έ,s.Ε))) Ъ=σ[i];return Ъ;}float Ιφ(ό s){п t=Ζή(s);if(t==null) return 0;float k=t.ǽ*1000000f;String[]ъ=t.Μ.Split('|');if(ъ.Length>=3) k+=Vector3.Distance(t.έ,new Vector3(float.Parse(ъ[0]),float.Parse(ъ[1]),float.Parse(ъ[2])));return k;
}void ΚΟ(){Ą.Clear();ǻ=null;List<float>ΕΝ=new List<float>();for(int i=0;i<Н.Count;i++){float k=Ιφ(Н[i]);int ΑΔ=0;while(ΑΔ<ΕΝ.Count&&ΕΝ[ΑΔ]<=k) ΑΔ++;ΕΝ.Insert(ΑΔ,k);Ą.Insert(ΑΔ,Н[i]);}}void Κη(){if(ǻ!=null){bool ΚБ=ΗІ||Vector3.Distance(ǻ.Ε,ΗЉ)>ΗЇ||
(DateTime.Now-ΗЈ).TotalSeconds>45||ǻ.ΓЉ();if(!ΚБ) return;ǻ=null;}if(Ą.Count==0) return;ǻ=Ą[0];Ą.RemoveAt(0);ΗЉ=ǻ.Ε;ΗЈ=DateTime.Now;п t=Ζή(ǻ);ΗЇ=t==null?40:Math.Max(40,t.Ə*3);ΗІ=t!=null&&t.ũ==3&&t.б.LengthSquared()<0.25f;ǻ.ĳ("HOMEPOS",true,false,false);ΒΟ("HOMEPOS",ǻ.Ξ,"HOMEPOS");
}const String ΙΨ="[PAMDBG]";class Γν{public readonly bool Ζώ;IMyProgrammableBlock ķ;Action<IMyProgrammableBlock>Δβ;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Δα;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Δΰ;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>Δγ;
public Γν(MyGridProgram Λθ){ķ=Λθ.Me;var ƞ=ķ.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ķ);if(ƞ==null) return;Δβ=(Action<IMyProgrammableBlock>) ƞ["RemoveDraw"];Δα=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) ƞ["Point"];Δΰ=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) ƞ["Line"];
Δγ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) ƞ["Sphere"];((Action<IMyProgrammableBlock>) ƞ["RemoveAll"])(ķ);Ζώ=true;}public void ΙЍ(){if(Δβ!=null) Δβ(ķ);}public void Ĳ(Vector3D a,Vector3D b,Color c,float ΛЀ){if(Δΰ!=null) Δΰ(ķ,a,b,c,ΛЀ,-1,false);}
public void ΙЋ(Vector3D a,Color c,float Ċ){if(Δα!=null) Δα(ķ,a,c,Ċ,-1,false);}public void ΖЍ(Vector3D ƍ,double Ċ,Color c){if(Δγ!=null) Δγ(ķ,new BoundingSphereD(ƍ,Ċ),c,1,0.05f,30,-1,false);}}class Ƽ{public long Ξ;public Vector3 ΘΑ,š,Ȗ,ε;public bool ΗЁ;public List<Vector3>ΓΔ=new List<Vector3>(),Εν=new List<Vector3>();
public List<bool>Εμ=new List<bool>();public DateTime Ȣ;}Γν і=null;IMyBroadcastListener Βν=null;List<Ƽ>Ę=new List<Ƽ>();void Ιπ(String Η){String[]f=Η.Split(';');if(f.Length<6) return;long Ξ=long.Parse(f[0]);Ƽ s=null;for(int i=0;i<Ę.Count;i++) if(Ę[i].Ξ==Ξ) s=Ę[i];if(s==null){s=new Ƽ();s.Ξ=Ξ;Ę.Add(s);}
s.ΘΑ=θ(f[1]);s.š=θ(f[2]);s.Ȗ=θ(f[3]);s.ΗЁ=s.š.LengthSquared()>0.5f;s.ε=θ(f[4]);s.Ȣ=DateTime.Now;if(f[5]=="") return;s.ΓΔ.Clear();s.Εν.Clear();s.Εμ.Clear();String[]Θε=f[5].Split(':');for(int i=0;i<Θε.Length;i++){String[]r=Θε[i].Split(',');if(r.Length<3) continue;s.ΓΔ.Add(θ(r[0]));s.Εν.Add(θ(r[1]));s.Εμ.Add(r[2]=="1");
}}void Κά(){if(!debugDraw) return;if(і==null) і=new Γν(this);if(!і.Ζώ) return;if(Βν==null) Βν=IGC.RegisterBroadcastListener(ΙΨ);int ǡ=20;while(Βν.HasPendingMessage&&ǡ-->0){String Η=Βν.AcceptMessage().Data as String;if(Η!=null) try{Ιπ(Η);}catch{}}for(int i=Ę.Count-1;i>=0;i--) if((DateTime.Now-Ę[i].Ȣ).TotalSeconds>5) Ę.RemoveAt(i);
і.ΙЍ();for(int i=0;i<σ.Count;i++){п s=σ[i];Color c=ΒΦ(s)?Color.Gray:s.ǿ>=0?Color.Yellow:Color.Lime;і.ΖЍ(s.έ,s.Ə/2,c);if(s.б.LengthSquared()>0.25f) і.Ĳ(s.έ,s.έ+s.б*3,Color.White,0.1f);if(s.ț.LengthSquared()>0.25f) і.Ĳ(s.έ,s.έ+s.ț*s.Ə,Color.Cyan,0.1f);}for(int i=0;i<ǚ.Count;i++)
{if((DateTime.Now-ǚ[i].Ƙ).TotalSeconds>0.5) continue;п a=null,b=null;for(int k=0;k<σ.Count;k++){if(σ[k].Ξ==ǚ[i].a) a=σ[k];if(σ[k].Ξ==ǚ[i].b) b=σ[k];}if(a!=null&&b!=null) і.Ĳ(a.έ,b.έ,Color.Red,0.2f);}for(int i=0;i<Ę.Count;i++){Ƽ d=Ę[i];п t=null;for(int k=0;k<σ.Count;k++) if(σ[k].Ξ==d.Ξ) t=σ[k];
if(t!=null) і.Ĳ(t.έ,d.ε,Color.Blue,0.08f);for(int r=0;r<d.ΓΔ.Count;r++) і.Ĳ(d.ΓΔ[r],d.Εν[r],d.Εμ[r]?Color.Red:Color.Green,0.04f);if(t==null||!d.ΗЁ) continue;ȍ Μ=null;for(int j=0;j<Ǔ.Count;j++) if(Ǔ[j].ъ==t.Μ) Μ=Ǔ[j];if(Μ==null) continue;for(int c=0;c<Μ.ύ.Length;c++)
{int Δύ=c%Μ.w,Ņ=c/Μ.w;Vector3 p=d.ΘΑ+d.š*((Μ.w-1f)/2f-Δύ)*Μ.ΕЎ-d.Ȗ*((Μ.h-1f)/2f-Ņ)*Μ.ΕЂ;ȫ Εη=ΓЁ(Μ,c,null);і.ΙЋ(p,Μ.ύ[c]=='1'?Color.Gray:Εη!=null?Color.Orange:Color.White,Math.Min(Μ.ΕЎ,Μ.ΕЂ)*0.2f);if(Εη!=null&&Εη.Ξ==d.Ξ) і.Ĳ(t.έ,p,Color.Orange,0.05f);
}}for(int i=0;i<Ȋ.Count;i++){IMySensorBlock z=Ȋ[i];і.ΖЍ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);}for(int i=0;i<Ġ.Count;i++)
і.Ĳ(Ġ[i].GetPosition(),Ġ[i].GetPosition()+Ġ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(ѓ!=0&&Βγ!=0){IMyShipConnector c=ΔΩ(Βγ);for(int k=0;k<σ.Count;k++)
if(c!=null&&σ[k].Ξ==ѓ) і.Ĳ(σ[k].έ,c.GetPosition(),Color.Lime,0.15f);}}