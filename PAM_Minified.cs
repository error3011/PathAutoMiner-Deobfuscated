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

//Speed limit near the job area (moving between holes, backing out, arriving and leaving), so ships
//working next to each other don't change direction fast or bump into each other.
const float jobAreaSpeed = 10f;//[m/s]

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ν{Ĺ,Ǉ,Ƕ,Ǐ,ί}public enum Α{є,ǵ,ŕ,ȑ,Ƿ,И,т,
Ĕ,ſ,ȏ,ő,ō,Ĳ,
ǉ,ȱ,Ǻ,Ő,Βί,ΑЎ,
Δλ,Βή,Œ,ΑА,ĺ}public enum Σ{ы,ƅ,Л,Γϊ,ΑЉ,ΒΚ,ΒΛ,ź,Ż}public enum ǎ{None,Ő,є,Ζη}public enum Φ{Main,ΖБ,ǆ,ǅ,Δζ,ΑЇ,
Γι,Ζϋ,Ζό,Ζω,ǋ,ΗΕ,
ΒΦ,Δά,Δΰ,ΗΗ,ΒΨ}public enum ь{ΒΤ,ƾ,Κέ,ΖϏ,ί}public enum ѕ{ΑΒ,Ζί}public enum ђ{ΒΣ,ΓЁ,ΗΚ,ǂ}public enum З{ΔΜ,Ζδ,Γτ,Γϋ,ΑЌ,ΓЌ,ΒΕ}public enum ǟ{Γχ,Γξ,Γο}public enum Я{Ƃ,ΙЈ,Ŏ}public enum τ{ΔΞ,ΔΣ,ΔΤ,ΖЌ,ΖЋ,
ΔΝ,ΒΟ,ΒΞ,ΔΠ,
ΔΡ,ΔΟ}public enum ι{Ȫ,Ė,ȩ,ΑІ,Γμ,Γν}public enum ǹ{ΒΒ,ΑЅ,Ιλ}public enum ǐ{ΒΩ,Ζι,ΚΒ,ΔΛ}public enum Џ{None,Ɔ,ǈ,ΒΝ}public enum Ϗ{ΑΔ,ΑЄ,ΒΘ,ǂ,ΔΗ,ΒΙ,ΔΘ,Κε,ΔΥ}public enum Ц{Ĺ,ΖЈ,ΖЇ,ΔΔ,ΓЎ,Γυ,ΖЉ}class έ{public bool ν=false;public Vector3 Ε=new Vector3();
public Vector3 λ=new Vector3();public Vector3 μ=new Vector3();public Vector3 Ѓ=new Vector3();public Vector3 ϋ=new Vector3();public Vector3 х=new Vector3();public float Т=0;public float Μΰ=0;public float[]ȋ=null;public έ(){}public έ(έ η){ν=η.ν;Ε=η.Ε;λ=η.λ;μ=η.μ;Ѓ=η.Ѓ;ϋ=η.ϋ;
х=η.х;ȋ=η.ȋ;}public έ(Vector3 Ε,Vector3 μ,Vector3 λ,Vector3 Ѓ,Vector3 ϋ){this.Ε=Ε;this.λ=λ;this.μ=μ;this.Ѓ=Ѓ;this.Т=0;this.ϋ=ϋ;}public void ΙΤ(List<IMyThrust>Βγ,List<string>ď){ȋ=new float[ď.Count];for(int i=0;i<ȋ.Length;i++)
ȋ[i]=-1;for(int i=0;i<Βγ.Count;i++){string Ǟ=Ζχ(Βγ[i]);int Β=ď.IndexOf(Ǟ);if(Β!=-1)
ȋ[Β]=М(Βγ[i].MaxEffectiveThrust,Βγ[i].MaxThrust);}}}class ΔΦ{public έ ε=null;public List<Vector3>Ʋ=new List<Vector3>();public float ΑΟ=0;public float ΖΛ=0;public float ƍ=0;public float ΕΖ=0;public Vector3 ΒϏ=new Vector3();}class Ɖ{public τ Х=τ.ΔΞ;public float ǽ=0;public float Ł=0;
public string Ǿ="";public string ȃ="";DateTime ΘΣ;public bool Εφ=false;private bool ΔЋ=false;public bool Γς(bool ȇ){if(ȇ){Ł=0;ΔЋ=false;return false;}ΔЋ=true;return Ł>ǽ;}public void ΚΜ(){Γς(true);Εφ=false;}public void Κχ(){if(ΔЋ)
if((DateTime.Now-ΘΣ).TotalSeconds>1){Ł++;ΘΣ=DateTime.Now;}}public bool ΓЈ(){switch(Х){case τ.ΔΣ:return true;case τ.ΔΤ:return true;}return false;}}class у{public String ΰ="";public String ω="";public int А=0;public Я Ɵ=Я.Ŏ;public у(String ΰ,String ω,int А,Я Ɵ){this.ΰ=ΰ;this.ω=ω;
this.А=А;this.Ɵ=Ɵ;}}class Ιζ{public Ιζ(Vector3 ά,float υ){this.ά=ά;this.υ=υ;}public Vector3 ά;public float υ;}class ϊ{public DateTime Εά;public DateTime Ηλ;public String Θ="";public String ΰ="";public String Γζ="";public String Μ="";public String ΕЈ="";public String ΓΕ="";public String Ευ="";
public Vector3 Ε=new Vector3();public List<у>Б=new List<у>();public Ν Ξ=Ν.Ĺ;public Џ Ľ;public float ȡ;public float ș;public int Г;public int ΔА=0;public int ΖΕ=0;public bool ΔΒ(){return(DateTime.Now-Εά).TotalSeconds>10;}public bool ΓЏ(){if(Ľ!=Џ.Ɔ) return false;return(DateTime.Now-Ηλ).TotalSeconds>=4;
}public ϊ(String Θ){this.Θ=Θ;}public Џ ķ(String І,bool ΛЃ,bool ж,bool ΚЇ){if(І==""&&!ж) return Џ.None;if(Ľ==Џ.Ɔ&&ΓЏ())
Ľ=Џ.ΒΝ;if(ΛЃ){Ευ=І;Ľ=Џ.Ɔ;Ηλ=DateTime.Now;return Ľ;}else if(ж){Ευ="";Ľ=Џ.None;}else if(Ευ==І){if(ΚЇ) Ľ=Џ.ǈ;return Ľ;}return Џ.None;}}public String Ιτ(String ΛΫ){switch(ΛΫ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΓΟ=new Vector3();Vector3 β=new Vector3();Vector3 ΒЂ=new Vector3();Vector3 Αε=new Vector3();Vector3 ΑΣ=new Vector3();
DateTime Εί=new DateTime();bool ğ=true;int ΕА=0;bool Ĥ=false;bool ĥ=false;bool Αλ=true;bool ΙΘ=false;bool Ɯ=false;bool ΒЇ=false;float Ď=0;float Ŵ=0;int Ŋ=0;int Αύ=0;int Αρ=0;int ΔЂ=0;float Ƣ=0;float Ǝ=0;double Ει=0;float ΑΘ=0;List<int>ƙ=new List<int>();List<int>ƪ=new List<int>();
Ν Ξ=Ν.Ĺ;int Е=0;float ǰ=0;float Ȍ=0;float θ=0;float ΓΣ=0,ΕЁ=0;IMyRemoteControl Ω;IMySensorBlock ό;List<IMyTimerBlock>ΖΑ=new List<IMyTimerBlock>();List<IMyShipConnector>Ǒ=new List<IMyShipConnector>();List<IMyThrust>Đ=new List<IMyThrust>();List<IMyGyro>ΒЃ=new List<IMyGyro>();
List<IMyTerminalBlock>ύ=new List<IMyTerminalBlock>();List<IMyLandingGear>Ĉ=new List<IMyLandingGear>();List<IMyReactor>ň=new List<IMyReactor>();List<IMyConveyorSorter>ΓΨ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǧ=new List<IMyGasTank>();List<IMyTerminalBlock>ΑΕ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ƒ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŝ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ļ=new List<IMyBatteryBlock>();List<IMyTextPanel>Э=new List<IMyTextPanel>();List<IMyTextSurface>ȥ=new List<IMyTextSurface>();List<IMyTextPanel>Š=new List<IMyTextPanel>();
IMyTerminalBlock Ǫ=null;έ Ρ=new έ();έ ΕΤ=new έ();έ ΕΨ=new έ();bool đ=false;bool Ʒ=false;bool Ƹ=false;bool ΒЉ=false;List<String>ď=new List<string>();List<έ>Ψ=new List<έ>();List<έ>ΑЂ=new List<έ>();int ΔБ=0;double ΙΙ=0;έ Ζ=new έ();Vector3 м;Vector3 ĉ;Σ ζ=Σ.ы;ѕ Ŧ=ѕ.ΑΒ;int Ћ=0;
int Ь=0;double Αδ=0;bool Βσ=false;Ɖ Ѝ=new Ɖ();Ɖ Ў=new Ɖ();ђ ќ=ђ.ΒΣ;bool ŋ=true;ѕ Ȥ=ѕ.ΑΒ;int ј=3;int ї=3;int ф=30;ǟ ǒ=ǟ.Γχ;bool ı=true;bool ƛ=true;bool ƕ=true;bool ŵ=false;З к=З.ΔΜ;bool İ=true;float Ƞ=90;float ǘ=20;float ц=5;float Ǚ=20;float Ȕ=0.70f;float Ƽ=1.50f;
float ƻ=2.50f;float Ź=0f;float ţ=0f;Α Ϋ;Σ Ǘ;Α Αφ;ΔΦ ρ=null;int Є=0;int Ё=0;int з=0;int Ĩ=0;int Ų=30;int р=0;bool Ж=true;Vector3 Ю;double и=0;bool ΓΡ=false;int ΖΞ=0;int Ќ=0;int Δχ=0;int Ȣ=0;int Θλ=0;Vector3 Αν=new Vector3();float љ=0;float щ=0;int Ÿ=0;έ ΓΩ=null;
float Εέ=0;float Βε=0;float п=0;float Ū=0;bool ΕΦ=false;bool Ŷ=false;bool Αώ=false;bool Ģ=false;bool Ě=false;DateTime ΛΗ=new DateTime();έ Ş=null;έ Ησ=null;float ȳ=0;ǎ ȕ;float ģ=0;float ŷ=0;String Εδ="";float ΒЍ=0;float ș=0;float ȡ=0;List<у>ȁ=new List<у>();float л=0;
bool Ασ=false;bool ΘЎ=false;bool Θϊ=false;bool Γή=false;bool Θϋ=false;float Εη=0;float Ετ=0;Vector3 ΗЇ=new Vector3();Vector3 ώ=new Vector3();Vector3 Γί=new Vector3(1,1,1);float ΕЏ=1;Vector3 Αϊ=new Vector3();bool ΒЈ=false;bool ΕΜ=false;bool Εα=false;bool ΗΧ=false;float Βρ=0;
float Βα=2;Vector3 ΘЋ;Vector3 ΕЌ;Vector3 ΕЋ;float[,]ȍ=new float[3,2];Dictionary<String,float[,]>ǯ=new Dictionary<string,float[,]>();Dictionary<String,float>ΙΖ=new Dictionary<String,float>();int Δϋ=0;Dictionary<String,float[]>Ī=new Dictionary<string,float[]>();float Εχ;bool Βυ=false;
int Βδ=0;int Ņ=0;int ΒЅ=0;int Ť=0;String Μ="";int[]Θΰ=new int[Enum.GetValues(typeof(Φ)).Length];bool Ελ=false;int ū=0;int Г=0;Φ Ѐ=Φ.Main;int ΜΑ=0,ΜΒ=0,ΜΓ=0,ΜΔ=0;int Λώ=0,ΛϏ=0;bool Ɛ=false;bool ΗΨ=false;String Ư="";String ΔЁ="";IMyBroadcastListener ǻ=null;
bool ΗІ=true;List<ϊ>Ї=null;ϊ О=null;int Βλ=0,Βμ=0;int Ηξ=0,Ηο=0;int ΚЌ=0,ΚЍ=0,ΚЎ=0;String Λέ="";Exception ħ=null;bool ΓΠ=false;Σ Ƭ=Σ.ы;bool Ηφ=false;bool ΘΠ=true;bool Εύ=false;String ΙΗ="";bool Αό=false;bool ΕЇ=true;int ΒІ=0;int[]ǭ=null;Vector3 Βη=new Vector3();
bool ΗЌ=false;bool Ǡ(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓЉ(Me,controllerTag,true))
Ξ=Ν.Ǐ;ξ(Φ.Main);if(Ξ!=Ν.Ǐ){Μ="Welcome to [PAM]!";ǐ Εό=ΚΝ();if(Ξ==Ν.Ĺ){List<IMyShipDrill>ΗЃ=new List<IMyShipDrill>();List<IMyShipGrinder>ΗЍ=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗЃ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(ΗЍ,q=>q.CubeGrid==Me.CubeGrid);if(ΗЃ.Count>0)
{Ξ=Ν.Ǉ;Μ="Miner mode enabled!";}else if(ΗЍ.Count>0){Ξ=Ν.Ƕ;Μ="Grinder mode enabled!";}else{Ξ=Ν.ί;ȯ(ь.ί);}}if(Εό==ǐ.ΒΩ) Αλ=false;if(Εό==ǐ.Ζι) Μ="Data restore failed!";if(Εό==ǐ.ΔΛ) Μ="New version, wipe data";}}void Main(string Ш,UpdateType ΜΜ){try{if(ħ!=null){if(Ш.Trim().ToUpper()=="RETRY")
{ħ=null;ğ=true;Ŋ=0;if(Ξ!=Ν.Ǐ) Ѕ();Μ="Restarted after error";}else{ΙЀ();ΖЏ();return;}}ĥ=(ΜΜ&UpdateType.Update10)!=0;if(ĥ) ΕА++;Ĥ=ΕА>=10;if(Ĥ) ΕА=0;if(ĥ){Βδ++;if(Βδ>4) Βδ=0;Αρ=(int) Math.Max(0,10-(DateTime.Now-Εί).TotalSeconds);}if(Ш!="") Λέ=Ш;if(Ξ!=Ν.Ǐ)
Κά(Ш);else
Ιγ(Ш);Ελ=false;try{int Ητ=Runtime.CurrentInstructionCount;float ű=М(Ητ,Runtime.MaxInstructionCount);if(ű>0.90) Μ="Max. instructions >90%";if(ű>Ƣ) Ƣ=ű;if(Βυ){ƙ.Add(Ητ);while(ƙ.Count>10) ƙ.RemoveAt(0);Ǝ=0;for(int i=0;i<ƙ.Count;i++) Ǝ+=ƙ[i];Ǝ=М(М(Ǝ,ƙ.Count),Runtime.MaxInstructionCount);
double Εή=Runtime.LastRunTimeMs;if(ΒЇ&&Εή>Ει) Ει=Εή;ƪ.Add((int)(Εή*1000f));while(ƪ.Count>10) ƪ.RemoveAt(0);ΑΘ=0;for(int i=0;i<ƪ.Count;i++) ΑΘ+=ƪ[i];ΑΘ=М(ΑΘ,ƪ.Count)/1000f;}}catch{Ƣ=0;}}catch(Exception e){ħ=e;}}void Κά(string Ш){bool ΓΤ=false;String Ηγ="";if(Е<=1&&!ΔΑ(Ш))
ΖА(Ш);int Εμ=50;while(ǻ!=null&&ǻ.HasPendingMessage&&Εμ-->0){MyIGCMessage δ=ǻ.AcceptMessage();String Η=δ.Data as String;if(Η==null) continue;String ȣ="";if(Е<=1&&ΖЎ(ref Η,out ȣ,out Ηγ)&&ȣ==BROADCAST_SELF_ID){ΖА(Η);ΓΤ=true;break;}}bool ΘΚ=ΒЇ&&Ϋ==Α.є&&!Ɯ&&!ΓΤ&&Ŋ==0&&!ΒЉ;
if(Ĥ&&Ϋ!=Α.є) ΓΤ=true;if((ĥ&&!ΘΚ)||(Ĥ&&ΘΚ)){if(Ŋ==0&&(Αρ<=0||ğ)){ΙΘ=Е>0;Е=0;Ŋ=1;Ȱ();ΚΣ();Ζύ();ȫ("Scan 1");}else if(Ŋ==1){Ŋ=2;Ȱ();ΚΤ();ȫ("Scan 2");}else if(Ŋ==2){Ŋ=0;Ȱ();ΚΥ();ΚΦ();if(Е<=1) ΖІ();ȫ("Scan 3");Εί=DateTime.Now;if(Е<=1&&Αλ)
ΓΟ=ΒΓ(Ω,Ω.CenterOfMass);Αλ=false;if(ğ){ğ=false;bool ΜΡ=ζ==Σ.Л;Ѕ();if(ΘΠ&&resumeAfterRestart&&ΜΡ&&Е<=1) Εύ=true;}ΘΠ=false;if(ΙΘ&&Е==0) Μ="Setup complete";}else{if(ζ==Σ.Л&&Ξ!=Ν.ί){Ȱ();ΙΜ();ȫ("Inv balance");}Ȱ();switch(Αύ){case 0:Κι();break;case 1:Κπ();break;case 2:Κθ();break;
case 3:Κϋ();break;case 4:Κο();break;case 5:ΙΨ();break;case 6:ΙΦ(Ω);break;}ȫ("Update: "+Αύ);Αύ++;if(Αύ>6){Αύ=0;ΒЇ=true;if(Ƭ!=Σ.ы){switch(Ƭ){case Σ.ź:Ĵ();break;case Σ.Ż:Ĵ();break;case Σ.Л:Ĵ();break;case Σ.ΒΚ:ΑБ();break;case Σ.ΒΛ:œ();break;}Ƭ=Σ.ы;}}}if(!ğ){if(!ȭ(Ω,true))
{Ω=null;Αλ=true;Е=2;}if(Е>=2&&Ϋ!=Α.є)
Ѕ();if(Е<=1){Ŵ=Ω.CalculateShipMass().PhysicalMass;Ď=(float) Ω.GetShipSpeed();β=Κη(Ω,ΓΟ);ΒЂ=Ω.WorldMatrix.Forward;Αε=Ω.WorldMatrix.Left;ΑΣ=Ω.WorldMatrix.Down;Κτ();Κψ();Κμ();if(Εύ){Εύ=false;Ĵ();if(Ϋ!=Α.є) Μ="Job resumed after restart";}if(Ϋ!=Α.є){Ɯ=false;ΗΒ(false);Ζέ(false);
String ΛЈ=ΓЄ(Ϋ)+" "+(int) Ϋ;Ȱ();Κς();Δκ(false);ȫ(ΛЈ);Κω();Κσ();Ȱ();Κξ();ȫ("Thruster");Ȱ();ΗΣ();ȫ("Gyroscope");}else{if(Ɯ){if(ΒΗ()){с(ΑΣ,ΒЂ,Αε,0.25f,true);ΗΣ();Μ="Aligning to planet: "+Math.Round(Βρ-0.25f,2)+"°";if(ΒЈ) ΓЇ(true,true);}else ΓЇ(true,true);}}Αό=false;}}}Ȱ();ΖЏ();ȫ("Print");
if(ΓΤ||ΔЂ<=0){Ȱ();ΙΝ(Ηγ);ȫ("Broadcast");ΔЂ=4;}else if(Ĥ) ΔЂ--;}void ΖА(string Ш){if(Ш=="") return;var ψ=Ш.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var І=ψ.ElementAtOrDefault(0);var Ȳ=ψ.ElementAtOrDefault(1);var ΑΗ=ψ.ElementAtOrDefault(2);var ΗΪ=ψ.ElementAtOrDefault(3);String ć="Invalid argument: "+Ш;
bool ΖΚ=false;switch(І){case "UP":this.ΔΖ(false);break;case "DOWN":this.ΔΕ(false);break;case "UPLOOP":this.ΔΖ(true);break;case "DOWNLOOP":this.ΔΕ(true);break;case "APPLY":this.ΖΫ(true);break;case "MRES":Г=0;break;case "STOP":this.Ѕ();break;case "PATHHOME":{this.Ѕ();this.Δδ();}break;
case "PATH":{this.Ѕ();this.Δδ();Ρ.ν=true;}break;case "START":{this.Ѕ();Δγ();}break;case "ALIGN":{ΓЇ(!Ɯ,false);}break;case "CONT":{this.Ѕ();this.Ĵ();}break;case "JOBPOS":{this.Ѕ();this.œ();}break;case "HOMEPOS":{this.Ѕ();this.ΑБ();}break;case "FULL":{Ασ=true;}break;
case "RESET":{ΓΠ=true;Е=2;}break;case "EXPORT":{Ιμ();}break;case "IMPORT":{ΙІ(Ȳ=="JOB");}break;default:ΖΚ=true;break;}if(Ξ!=Ν.ί){switch(І){case "SHUTTLE":{Ιι();}break;case "CFGS":{if(!Ια(Ȳ,ΑΗ,ΗΪ)) Μ=ć;}break;case "CFGB":{if(!Ιΰ(Ȳ,ΑΗ)) Μ=ć;}break;case "CFGL":
{if(!Ŗ(ref Ƞ,true,Ϗ.ΒΘ,Ȳ,"")||!Ιβ(ΑΗ))
Μ=ć;}break;case "CFGE":{if(!Ŗ(ref ц,true,Ϗ.ΒΙ,Ȳ,"IG")||
!Ŗ(ref ǘ,true,Ϗ.ΔΗ,ΑΗ,"IG")||
!Ŗ(ref Ǚ,true,Ϗ.ΔΘ,ΗΪ,"IG"))
Μ=ć;}break;case "CFGA":{if(!Ŗ(ref Ȕ,false,Ϗ.ΑЄ,Ȳ,"")) Μ=ć;}break;case "CFGW":{if(!Ŗ(ref Ƽ,false,Ϗ.ΑΔ,Ȳ,"")||
!Ŗ(ref ƻ,false,Ϗ.ΑΔ,ΑΗ,""))
Μ=ć;}break;case "NEXT":{Γθ(false);}break;case "PREV":{Γθ(true);}break;default:if(ΖΚ) Μ=ć;break;}}else{switch(І){case "UNDOCK":{Αό=true;}break;default:if(ΖΚ) Μ=ć;break;}}}String Ιυ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ιι(){Ѕ();Ξ=Ν.ί;ȯ(ь.ί);Ζ.ν=false;Ρ.ν=false;ό=null;ύ.Clear();ζ=Σ.ы;}void ΓЇ(bool Р,bool φ){if(!Р) Μ="Aligning canceled";if(φ) Μ="Aligning done";if(φ||!Р){Ɯ=false;ΗΛ();ΒΧ(false,0,0,0,0);return;}if(ΒΗ()) Ɯ=true;}bool Ια(String ΜΤ,String ΛΤ,String ΛΑ)
{bool ΜΟ=ζ==Σ.Л;int w,h,d;if(int.TryParse(ΜΤ,out w)&&int.TryParse(ΛΤ,out h)&&int.TryParse(ΛΑ,out d)){this.Ѕ();ј=w;ї=h;ф=d;Ś(false);ΑΓ(false,false);if(ΜΟ) Ĵ();return true;}return false;}bool Ιβ(String ΑΖ){if(ΑΖ=="ON"){ı=true;return true;}if(ΑΖ=="OFF"){ı=false;return true;}
return false;}bool Ιΰ(String ΗЁ,String Βς){bool Ċ=true;if(ΗЁ=="HOME") ŋ=true;else if(ΗЁ=="STOP") ŋ=false;else Ċ=false;if(Βς=="HOME") ќ=ђ.ΒΣ;else if(Βς=="STOP") ќ=ђ.ΗΚ;else if(Βς=="JOB") ќ=ђ.ΓЁ;else if(Βς=="IG") ќ=ђ.ǂ;else Ċ=false;return Ċ;}void ξ(Φ ю){Θΰ[(int) Ѐ]=Г;
Г=Θΰ[(int) ю];if(ю==Φ.ǋ) Г=0;Ѐ=ю;if(Ξ!=Ν.Ǐ)
ř(Ѐ==Φ.ǆ,false,0,0);Ελ=true;}void ΔΖ(bool Εβ){if(Г>0) Г--;else if(Εβ) Г=ū-1;}void ΔΕ(bool Εβ){if(Г<ū-1) Г++;else if(Εβ) Г=0;}String ȯ(ь δ){switch(δ){case ь.ΒΤ:Μ="Job is running";break;case ь.ƾ:Μ="Connector not ready!";break;case ь.Κέ:Μ="Ship modified, path outdated!";break;
case ь.ΖϏ:Μ="Interrupted by player!";break;case ь.ί:Μ="Shuttle mode enabled!";break;}return "";}String Ιϋ(ѕ ά){switch(ά){case ѕ.ΑΒ:return "Top-Left";case ѕ.Ζί:return "Center";default:return "";}}String Ιρ(ǟ Д){switch(Д){case ǟ.Γξ:return "Auto"+(Ξ==Ν.Ǉ?" (Ore)":"");case ǟ.Γο:return "Auto (+Stone)";
case ǟ.Γχ:return "Default";default:return "";}}String ΓЂ(Σ Ј){switch(Ј){case Σ.ы:return "No job";case Σ.ƅ:return "Job paused";case Σ.Л:return "Job active";case Σ.ź:return "Job active";case Σ.Ż:return "Job active";case Σ.Γϊ:return "Job done";case Σ.ΑЉ:return "Job changed";case Σ.ΒΚ:return "Move home";
case Σ.ΒΛ:return "Move to job";}return "";}String Ζξ(ђ ΚІ){switch(ΚІ){case ђ.ΒΣ:return "Return home";case ђ.ΓЁ:return "Fly to job pos";case ђ.ΗΚ:return "Stop";case ђ.ǂ:return "Ignore";}return "";}String Ισ(З Д){switch(Д){case З.ΔΜ:return "Off";case З.Γϋ:return "Drop pos (Stone) ";case З.ΑЌ:return "Drop pos (Sto.+Ice)";
case З.Ζδ:return "Cur. pos (Stone)";case З.Γτ:return "Cur. pos (Sto.+Ice)";case З.ΓЌ:return "In motion (Stone)";case З.ΒΕ:return "In motion (Sto.+Ice)";}return "";}String Ζμ(ǎ Ј){switch(Ј){case ǎ.None:return "No batteries";case ǎ.Ő:return "Charging";case ǎ.Ζη:return "Discharging";}return "";}
String ΓЄ(Α Ј){String Μή=Ξ==Ν.ί?"target":"job";switch(Ј){case Α.є:return "Idle";case Α.ǵ:return "Flying to XY position";case Α.ŕ:return Ξ==Ν.Ƕ?"Grinding":"Mining";case Α.ȑ:return "Returning";case Α.Œ:return "Flying to drop pos";case Α.Ƿ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.т:return "Flying to job area";case Α.Ĕ:return "Flying to path";case Α.ſ:return "Flying to job position";case Α.ȏ:return "Approaching dock";case Α.ő:return "Docking";case Α.ō:return "Aligning";case Α.Ĳ:return "Aligning";case Α.ǉ:return "Retry docking";case Α.ȱ:return "Unloading";case Α.ĺ:return ΙΗ;
case Α.Ǻ:return "Undocking";case Α.Ő:return "Charging batteries";case Α.Βί:return "Waiting for uranium";case Α.ΑЎ:return "Filling up hydrogen";case Α.Δλ:return "Waiting for ejection";case Α.Βή:return "Waiting for ejection";case Α.ΑА:return "Flying to drop pos";}return "";}String Ζψ(τ Х){switch(Х)
{case τ.ΔΞ:return "On \"Undock\" command";case τ.ΔΣ:return "On player entered cockpit";case τ.ΖЌ:return "On ship is full";case τ.ΖЋ:return "On ship is empty";case τ.ΔΤ:return "On time delay";case τ.ΒΟ:return "On batteries empty(<25%)";case τ.ΒΞ:return "On batteries empty(=0%)";case τ.ΔΝ:return "On batteries full";
case τ.ΔΡ:return "On hydrogen empty(<25%)";case τ.ΔΟ:return "On hydrogen empty(=0%)";case τ.ΔΠ:return "On hydrogen full";}return "";}bool Ο(ref String Γ,int Θφ,int ń,bool Λ,String ŧ){ū+=1;if(Θφ==ń) ŧ=">"+ŧ+(Βδ>=2?" .":"");else ŧ=" "+ŧ;Γ+=ŧ+"\n";return Θφ==ń&&Λ;}String ΖΫ(bool Λ)
{int Β=0;int Κ=Г;ū=0;String Ʈ="———————————————\n";String ƴ="--------------------------------------------\n";String Γ="";Γ+=ΓЂ(ζ)+" | "+(Ρ.ν?"Ready to dock":"No dock")+"\n";Γ+=Ʈ;double Ȋ=Math.Max(Math.Round(this.и),0);if(Ѐ==Φ.Main){bool ņ=Ξ==Ν.ί;if(Ο(ref Γ,Κ,Β++,Λ," Record path & set home")) Δδ();
if(Ξ==Ν.Ǉ)
if(Ο(ref Γ,Κ,Β++,Λ," Setup mining job")) ξ(Φ.ǆ);if(Ξ==Ν.Ƕ)
if(Ο(ref Γ,Κ,Β++,Λ," Setup grinding job")) ξ(Φ.ǆ);if(Ξ==Ν.ί)
if(Ο(ref Γ,Κ,Β++,Λ," Setup shuttle job")) ξ(Φ.Δΰ);if(Ο(ref Γ,Κ,Β++,Λ," Continue job")) Ĵ();if(Ο(ref Γ,Κ,Β++,Λ," Fly to home position")) ΑБ();if(Ο(ref Γ,Κ,Β++,Λ," Fly to job position")) œ();if(Ο(ref Γ,Κ,Β++,Λ," Behavior settings"))
if(ņ) ξ(Φ.ΒΨ);else ξ(Φ.ΑЇ);if(Ο(ref Γ,Κ,Β++,Λ," Info")) ξ(Φ.Ζϋ);if(Ξ!=Ν.ί)
if(Ο(ref Γ,Κ,Β++,Λ," Help")) ξ(Φ.Ζω);}else if(Ѐ==Φ.ǆ){double ΜΣ=Math.Round(ј*ǰ,1);double ΛΣ=Math.Round(ї*Ȍ,1);String К="";if(Ο(ref К,Κ,Β++,Λ," Start new job!")) Δγ();if(Ο(ref К,Κ,Β++,Λ," Change current job")){ΑΓ(false,false);ξ(Φ.Main);}if(Ο(ref К,Κ,Β++,Λ," Width + (Width: "+ј+" = "+ΜΣ+"m)")){ż(ref ј,5,20,1);Ś(true);}
if(Ο(ref К,Κ,Β++,Λ," Width -")){ż(ref ј,-5,20,-1);Ś(true);}if(Ο(ref К,Κ,Β++,Λ," Height + (Height: "+ї+" = "+ΛΣ+"m)")){ż(ref ї,5,20,1);Ś(true);}if(Ο(ref К,Κ,Β++,Λ," Height -")){ż(ref ї,-5,20,-1);Ś(true);}if(Ο(ref К,Κ,Β++,Λ," Depth + ("+(ǒ==ǟ.Γχ?"Depth":"Min")+": "+ф+"m)")){ż(ref ф,5,50,2);Ś(true);}
if(Ο(ref К,Κ,Β++,Λ," Depth -")){ż(ref ф,-5,50,-2);Ś(true);}if(Ο(ref К,Κ,Β++,Λ," Depth mode: "+Ιρ(ǒ))){ǒ=ĵ(ǒ);}if(Ο(ref К,Κ,Β++,Λ," Start pos: "+Ιϋ(Ȥ))){Ȥ=ĵ(Ȥ);}if(Ξ==Ν.Ƕ&&ǒ==ǟ.Γο) ǒ=ĵ(ǒ);Γ+=ΔΫ(8,К,Κ,ref Λώ);}else if(Ѐ==Φ.Δΰ){float[]Ηψ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ο(ref Γ,Κ,Β++,Λ," Next")) ξ(Φ.ΗΗ);if(Ο(ref Γ,Κ,Β++,Λ," Back")) ξ(Φ.Main);Γ+=" Leave connector 1:\n";if(Ο(ref Γ,Κ,Β++,Λ," - "+Ζψ(Ѝ.Х))) Ѝ.Х=ĵ(Ѝ.Х);if(!Ѝ.ΓЈ()) Γ+="\n";else if(Ο(ref Γ,Κ,Β++,Λ," - Delay: "+ΒΑ((int) Ѝ.ǽ))) Ѝ.ǽ=Ζε(Ѝ.ǽ,Ηψ);Γ+=" Leave connector 2:\n";if(Ο(ref Γ,Κ,Β++,Λ," - "+Ζψ(Ў.Х))) Ў.Х=ĵ(Ў.Х);
if(!Ў.ΓЈ()) Γ+="\n";else if(Ο(ref Γ,Κ,Β++,Λ," - Delay: "+ΒΑ((int) Ў.ǽ))) Ў.ǽ=Ζε(Ў.ǽ,Ηψ);}else if(Ѐ==Φ.ΗΗ){if(Ο(ref Γ,Κ,Β++,Λ," Start job!")) Δγ();if(Ο(ref Γ,Κ,Β++,Λ," Back")) ξ(Φ.Δΰ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ο(ref Γ,Κ,Β++,Λ," = "+(Ѝ.Ǿ!=""?Ѝ.Ǿ:"-"))) Ѝ.Ǿ=ΒΜ(ref ΜΑ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ο(ref Γ,Κ,Β++,Λ," = "+(Ѝ.ȃ!=""?Ѝ.ȃ:"-"))) Ѝ.ȃ=ΒΜ(ref ΜΓ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ο(ref Γ,Κ,Β++,Λ," = "+(Ў.Ǿ!=""?Ў.Ǿ:"-"))) Ў.Ǿ=ΒΜ(ref ΜΒ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ο(ref Γ,Κ,Β++,Λ," = "+(Ў.ȃ!=""?Ў.ȃ:"-"))) Ў.ȃ=ΒΜ(ref ΜΔ);
}else if(Ѐ==Φ.ǅ){String ΘΨ=Ƞ+" %";if(Ĥ) Ņ++;if(Ņ>1){Ņ=0;ΒЅ++;if(ΒЅ>1) ΒЅ=0;}bool[]ΕЄ=new bool[]{ň.Count==0,ȕ==ǎ.None,Ǧ.Count==0};int ΙΒ=0;while(true){ΙΒ++;Ť++;if(Ť>ΕЄ.Length-1) Ť=0;if(ΙΒ>=ΕЄ.Length) break;if(!ΕЄ[Ť]) break;}bool ņ=Ξ==Ν.ί;if(!ņ&&ı&&л!=-1&&ΒЅ==0)
ΘΨ=л<1000000?Math.Round(л)+" Kg":Math.Round(л/1000)+" t";if(Ο(ref Γ,Κ,Β++,Λ," Stop!")){Ѕ();ξ(Φ.Main);}if(Ο(ref Γ,Κ,Β++,Λ," Behavior settings"))
if(!ņ) ξ(Φ.ΑЇ);else ξ(Φ.ΒΨ);if(!ņ){if(Ο(ref Γ,Κ,Β++,Λ," Next hole")) Γθ(false);}else if(Ο(ref Γ,Κ,Β++,Λ," Undock")) Αό=true;Γ+=ƴ;if(!ņ) Γ+="Progress: "+Math.Round(Αδ,1)+" %\n";Γ+="State: "+ΓЄ(Ϋ)+" "+Ȋ+"m \n";Γ+="Load: "+ΒЍ+" % Max: "+ΘΨ+" \n";if(Ť==0) Γ+="Uranium: "+(ň.Count==0?"No reactors":Math.Round(ŷ,1)+"Kg "+(ц==-1?"":" Min: "+ц+" Kg"))+"\n";
if(Ť==1) Γ+="Battery: "+(ȕ==ǎ.None?Ζμ(ȕ):ȳ+"% "+(ǘ==-1||ņ?"":" Min: "+ǘ+" %"))+"\n";if(Ť==2) Γ+="Hydrogen: "+(Ǧ.Count==0?"No tanks":Math.Round(ģ,1)+"% "+(Ǚ==-1||ņ?"":" Min: "+Ǚ+" %"))+"\n";}else if(Ѐ==Φ.ΑЇ){String К="";if(Ο(ref К,Κ,Β++,Λ," Back")){if(ζ==Σ.Л) ξ(Φ.ǅ);else ξ(Φ.Main);}
if(Ο(ref К,Κ,Β++,Λ," Max load: "+Ƞ+"%")) ё(ref Ƞ,Ƞ<=80?-10:-5,Ϗ.ΒΘ,false);if(Ο(ref К,Κ,Β++,Λ," Weight limit: "+(ı?"On":"Off"))) ı=!ı;if(Ο(ref К,Κ,Β++,Λ," Ejection: "+Ισ(к))) к=ĵ(к);if(Ο(ref К,Κ,Β++,Λ," Toggle sorters: "+(ŵ?"On":"Off"))){ŵ=!ŵ;if(ŵ) ȓ(ΕЇ);}
if(Ο(ref К,Κ,Β++,Λ," Unload ice: "+(İ?"On":"Off"))) İ=!İ;if(Ο(ref К,Κ,Β++,Λ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ё(ref ц,(ц>5?-5:-1),Ϗ.ΒΙ,true);if(Ο(ref К,Κ,Β++,Λ," Battery: "+(ǘ==-1?"Ignore":"Min "+ǘ+"%"))) ё(ref ǘ,-5,Ϗ.ΔΗ,true);if(Ο(ref К,Κ,Β++,Λ," Hydrogen: "+(Ǚ==-1?"Ignore":"Min "+Ǚ+"%"))) ё(ref Ǚ,-10,Ϗ.ΔΘ,true);
if(Ο(ref К,Κ,Β++,Λ," When done: "+(ŋ?"Return home":"Stop"))) ŋ=!ŋ;if(Ο(ref К,Κ,Β++,Λ," On damage: "+Ζξ(ќ))) ќ=ĵ(ќ);if(Ο(ref К,Κ,Β++,Λ," Advanced...")) ξ(Φ.Γι);Γ+=ΔΫ(8,К,Κ,ref ΛϏ);}else if(Ѐ==Φ.Γι){if(Ο(ref Γ,Κ,Β++,Λ," Back")){if(ζ==Σ.Л) ξ(Φ.ǅ);else ξ(Φ.Main);}
if(Ο(ref Γ,Κ,Β++,Λ,(Ξ==Ν.Ƕ?" Grinder":" Drill")+" inv. balancing: "+(ƛ?"On":"Off"))) ƛ=!ƛ;if(Ο(ref Γ,Κ,Β++,Λ," Enable"+(Ξ==Ν.Ƕ?" grinders":" drills")+": "+(ƕ?"Fwd + Bwd":"Fwd"))) ƕ=!ƕ;if(Ο(ref Γ,Κ,Β++,Λ," Work speed fwd.: "+Ƽ+"m/s")) ё(ref Ƽ,0.5f,Ϗ.ΑΔ,false);
if(Ο(ref Γ,Κ,Β++,Λ," Work speed bwd.: "+ƻ+"m/s")) ё(ref ƻ,0.5f,Ϗ.ΑΔ,false);if(Ο(ref Γ,Κ,Β++,Λ," Acceleration: "+Math.Round(Ȕ*100f)+"%"+(Ȕ>0.80f?" (risky)":""))) ё(ref Ȕ,0.1f,Ϗ.ΑЄ,false);if(Ο(ref Γ,Κ,Β++,Λ," Width overlap: "+Ź*100f+"%")) Ζΰ(true,0.05f);if(Ο(ref Γ,Κ,Β++,Λ," Height overlap: "+ţ*100f+"%")) Ζΰ(false,0.05f);
}else if(Ѐ==Φ.ΒΨ){if(Ο(ref Γ,Κ,Β++,Λ," Back")){if(ζ==Σ.Л) ξ(Φ.ǅ);else ξ(Φ.Main);}if(Ο(ref Γ,Κ,Β++,Λ," Max load: "+Ƞ+"%")) ё(ref Ƞ,Ƞ<=80?-10:-5,Ϗ.ΒΘ,false);if(Ο(ref Γ,Κ,Β++,Λ," Unload ice: "+(İ?"On":"Off"))) İ=!İ;if(Ο(ref Γ,Κ,Β++,Λ," Uranium: "+(ц==-1?"Ignore":"Min "+ц+"Kg"))) ё(ref ц,(ц>5?-5:-1),Ϗ.ΒΙ,true);
if(Ο(ref Γ,Κ,Β++,Λ," Battery: "+(ǘ==-1?"Ignore":"Charge up"))) ǘ=(ǘ==-1?1:-1);if(Ο(ref Γ,Κ,Β++,Λ," Hydrogen: "+(Ǚ==-1?"Ignore":"Fill up"))) Ǚ=(Ǚ==-1?1:-1);if(Ο(ref Γ,Κ,Β++,Λ," On damage: "+Ζξ(ќ))) ќ=ĵ(ќ);if(Ο(ref Γ,Κ,Β++,Λ," Acceleration: "+Math.Round(Ȕ*100f)+"%"+(Ȕ>0.80f?" (risky)":""))) ё(ref Ȕ,0.1f,Ϗ.ΑЄ,false);
}else if(Ѐ==Φ.ΖБ){double Λή=0;if(Ψ.Count>0) Λή=Vector3.Distance(Ψ.Last().Ε,β);if(Ο(ref Γ,Κ,Β++,Λ," Stop path recording")) ΗΜ();if(Ξ!=Ν.ί){if(Ο(ref Γ,Κ,Β++,Λ," Home: "+(đ?"Use old home":(Ρ.ν?"Was set! ":"none ")))) đ=!đ;}else{if(Ο(ref Γ,Κ,Β++,Λ," Connector 1: "+(đ?"Use old connector":(Ρ.ν?"Was set! ":"none ")))) đ=!đ;
if(Ο(ref Γ,Κ,Β++,Λ," Connector 2: "+(Ʒ?"Use old connector":(Ζ.ν?"Was set! ":"none ")))) Ʒ=!Ʒ;}if(Ο(ref Γ,Κ,Β++,Λ," Path: "+(Ƹ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) Ƹ=!Ƹ;Γ+=ƴ;Γ+="Wp spacing: "+Math.Round(ΙΙ)+"m\n";}else if(Ѐ==Φ.Δζ){if(Ο(ref Γ,Κ,Β++,Λ," Stop")){Ѕ();ξ(Φ.Main);}
Γ+=ƴ;Γ+="State: "+ΓЄ(Ϋ)+" \n";Γ+="Speed: "+Math.Round(Ď,1)+"m/s\n";Γ+="Target dist: "+Ȋ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+ΖΞ+"\n";}else if(Ѐ==Φ.Ζϋ){List<IMyTerminalBlock>ş=Ζο();if(Ĥ) Ņ++;if(Ņ>=ş.Count) Ņ=0;if(Ο(ref Γ,Κ,Β++,Λ," Next")) ξ(Φ.Ζό);
Γ+=ƴ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΒЍ,1)+"% "+Math.Round(ș,1)+" / "+Math.Round(ȡ,1)+"\n";Γ+="Uranium: "+(ň.Count==0?"No reactors":Math.Round(ŷ,1)+"Kg "+Εδ)+"\n";Γ+="Battery: "+(ȕ==ǎ.None?"":ȳ+"% ")+Ζμ(ȕ)+"\n";Γ+="Hydrogen: "+(Ǧ.Count==0?"No tanks":Math.Round(ģ,1)+"% ")+"\n";
Γ+="Damage: "+(ş.Count==0?"None":""+(Ņ+1)+"/"+ş.Count+" "+ş[Ņ].CustomName)+"\n";}else if(Ѐ==Φ.Ζό){if(Ο(ref Γ,Κ,Β++,Λ," Back")) ξ(Φ.Main);Γ+=ƴ;Γ+="Next scan: "+Αρ+"s\n";Γ+="Drill area: "+Math.Round(ǰ,1)+"x"+Math.Round(Ȍ,1)+"m Ship: "+Math.Round(ΓΣ,1)+"x"+Math.Round(ΕЁ,1)+"m ("+Math.Round(θ,1)+"m)\n"+(Ȗ.Count>0?"Cameras: "+ΔЃ+"\n":"");
Γ+="Broadcast: "+(Ɛ?"Online - "+Ư:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ƣ*100f,1)+"% \n";}else if(Ѐ==Φ.Ζω){if(Ο(ref Γ,Κ,Β++,Λ," Back")) ξ(Φ.Main);Γ+=ƴ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Е==2) Γ="Fatal setup error\nNext scan: "+Αρ+"s\n";if(ΓΠ) Γ="Recompile script now";int Λί=Γ.Split('\n').Length;for(int i=Λί;i<=10;i++) Γ+="\n";Γ+=Ʈ;Γ+="Last: "+Μ+"\n";
return Γ;}void Ζΰ(bool ΛΪ,float Ǯ){Ѕ();ΑΓ(true,false);if(ΛΪ) ё(ref Ź,Ǯ,Ϗ.ΔΥ,false);else ё(ref ţ,Ǯ,Ϗ.ΔΥ,false);Ζή();ř(true,true,0,0);}float Ζε(float σ,float[]Γε){float α=Γε[0];for(int i=Γε.Length-1;i>=0;i--)
if(σ<Γε[i]) α=Γε[i];return α;}String ΒΜ(ref int ń){String α="";if(ń>=ΖΑ.Count) ń=-1;if(ń>=0) α=ΖΑ[ń].CustomName;ń++;return α;}void Βά(string ΰ){if(ζ!=Σ.Л) return;if(ΰ=="") return;IMyTerminalBlock Ϊ=χ.GetBlockWithName(ΰ);if(Ϊ==null||!(Ϊ is IMyTimerBlock)){Μ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ϊ).Trigger();}void ż(ref int value,int ΑΛ,int Į,int ΕЅ){if(ΑΛ==0) return;if(value<Į&&ΕЅ>0||value<=Į&&ΕЅ<0){value+=ΕЅ;return;}int Κύ=Math.Abs(ΑΛ);int Δρ=0;int Εν=1;while(true){Δρ+=Εν*Κύ*10;if(ΑΛ<0&&value-Į<=Δρ) break;if(ΑΛ>0&&value-Į<Δρ) break;
Εν++;}value+=Εν*ΑΛ;}void Ś(bool Μα){ј=Math.Max(ј,1);ї=Math.Max(ї,1);ф=Math.Max(ф,0);ř(Ѐ==Φ.ǆ,false,0,0);}bool Ŗ(ref float value,bool Λϊ,Ϗ ω,String ť,String ΛΧ){if(ť=="") return false;float Ʀ=-1;bool ΘΗ=false;if(ť.ToUpper()==ΛΧ) ΘΗ=true;else if(!float.TryParse(ť,out Ʀ)) return false;
else Ʀ=Math.Max(0,Ʀ);if(Λϊ) Ʀ=(float) Math.Round(Ʀ);ё(ref value,Ʀ,ω,ΘΗ,false);return true;}void ё(ref float value,float Ǯ,Ϗ ω,bool Αϋ){ё(ref value,value+Ǯ,ω,Αϋ,true);}void ё(ref float value,float ƣ,Ϗ ω,bool Αϋ,bool ΜΨ){float ȅ=0,Ȅ=0;if(ω==Ϗ.ΑΔ){ȅ=0.5f;Ȅ=10f;}if(ω==Ϗ.ΑЄ){ȅ=0.1f;Ȅ=1f;}
if(ω==Ϗ.ǂ){ȅ=50f;Ȅ=100f;}if(ω==Ϗ.ΔΗ){ȅ=5f;Ȅ=30f;}if(ω==Ϗ.ΒΙ){ȅ=1f;Ȅ=25f;}if(ω==Ϗ.ΒΘ){ȅ=10f;Ȅ=95f;}if(ω==Ϗ.ΔΘ){ȅ=10f;Ȅ=90f;}if(ω==Ϗ.Κε){ȅ=10f;Ȅ=1800;}if(ω==Ϗ.ΔΥ){ȅ=0.0f;Ȅ=0.75f;}if(ƣ==-1&&Αϋ){value=-1;return;}if(value==-1) Αϋ=false;
bool Θν=ƣ<ȅ||ƣ>Ȅ;if(Θν&&ΜΨ){if(ƣ<value) value=Ȅ;else if(ƣ>value) value=ȅ;}else value=ƣ;if(Θν&&Αϋ) value=-1;else value=Math.Max(ȅ,Math.Min(value,Ȅ));value=(float) Math.Round(value,2);}void Γθ(bool Λυ){if(Λυ) з=Math.Max(0,з-1);else з++;Γύ();Γό();for(int k=0;k<ą.Count&&k<й.Length;k++) Δή(ą[k],k<з);
Й=-1;Δκ(true);}T ĵ<T>(T σ){int Β=Array.IndexOf(Enum.GetValues(σ.GetType()),σ);Β++;if(Β>=Ικ(σ)) Β=0;return(T) Enum.GetValues(σ.GetType()).GetValue(Β);}int Ικ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δδ(){ΑЂ.Clear();for(int i=0;i<Ψ.Count;i++) ΑЂ.Add(Ψ[i]);Ψ.Clear();
ΒЉ=true;ΕΤ=new έ(Ρ);ΕΨ=new έ(Ζ);Ρ.ν=false;if(Ξ==Ν.ί) Ζ.ν=false;for(int i=0;i<ǯ.Count;i++)
if(!ď.Contains(ǯ.Keys.ElementAt(i)))
ď.Add(ǯ.Keys.ElementAt(i));đ=false;Ʒ=false;Ƹ=false;ξ(Φ.ΖБ);}void ΗΜ(){if(đ) Ρ=ΕΤ;if(Ʒ) Ζ=ΕΨ;if(Ƹ){Ψ.Clear();for(int i=0;i<ΑЂ.Count;i++) Ψ.Add(ΑЂ[i]);}ΒЉ=false;Ѕ();ξ(Φ.Main);}void Κτ(){if(!ΒЉ) return;if(Ϋ!=Α.є){ΗΜ();return;}if(!ΕΤ.ν) đ=false;if(!ΕΨ.ν) Ʒ=false;if(ΑЂ.Count<=1) Ƹ=false;
IMyShipConnector У=о(MyShipConnectorStatus.Connectable);if(У==null) У=о(MyShipConnectorStatus.Connected);if(У!=null){if(Math.Round(Ď,2)<=0.20) ΔБ++;else ΔБ=0;if(ΔБ>=5){if(Ξ==Ν.ί&&(Ρ.ν||đ)&&Vector3.Distance(Ρ.Ε,У.GetPosition())>5){Ζ.μ=Ω.WorldMatrix.Forward;Ζ.Ѓ=Ω.WorldMatrix.Left;
Ζ.λ=Ω.WorldMatrix.Down;Ζ.ϋ=Ω.GetNaturalGravity();Ζ.Ε=У.GetPosition();Ζ.ν=true;Ζ.х=У.Position;}else{Ρ.μ=Ω.WorldMatrix.Forward;Ρ.Ѓ=Ω.WorldMatrix.Left;Ρ.λ=Ω.WorldMatrix.Down;Ρ.ϋ=Ω.GetNaturalGravity();Ρ.Ε=У.GetPosition();Ρ.ν=true;Ρ.х=У.Position;}}}double ΕΫ=-1;if(Ψ.Count>0) ΕΫ=Vector3.Distance(β,Ψ.Last().Ε);
double ΛІ=Math.Max(1.5,Math.Pow(Ď/100.0,2));double Θα=Math.Max(Ď*ΛІ,2);ΙΙ=Θα;if((ΕΫ==-1)||ΕΫ>=Θα){έ ή=new έ(β,ΒЂ,ΑΣ,Αε,Ω.GetNaturalGravity());ή.ΙΤ(Đ,ď);Ψ.Add(ή);}}έ ѓ(){if(Ξ!=Ν.ί) return ǩ!=null?ǩ:Ρ;return Ησ;}void Ƈ(έ γ,Σ Ј){Ησ=γ;if(ζ==Σ.Л) Ǘ=Ј;}bool Ȭ(έ γ,float υ,bool ΜΝ,out Vector3 ά)
{if(ΜΝ){Vector3I ΕΛ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(ΕΛ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){ά=new Vector3();return false;}Vector3 ΚЋ=ъ(Ω,Ϊ.FatBlock.GetPosition()-β);Vector3 ΚЊ=ъ(Ω,Ϊ.FatBlock.WorldMatrix.Forward);ά=γ.Ε-Δι(γ.μ,γ.λ*-1,ΚЋ)-Δι(γ.μ,γ.λ*-1,ΚЊ)*υ;
return true;}else{ά=γ.Ε;return true;}}Vector3 ĕ(int ΔЈ,int ŉ,bool Λψ){if(!Λψ&&ΗЌ) return Βη;float ΚЉ=((Ћ-1f)/2f)-ΔЈ;float Λϋ=((Ь-1f)/2f)-ŉ;Βη=Ζ.Ε+Ζ.Ѓ*ΚЉ*ǰ+ĉ*-1*Λϋ*Ȍ;ΗЌ=true;return Βη;}Vector3 ΓЃ(Vector3 ΛЊ,float ΔЍ){return ΛЊ+(м*ΔЍ);}ι Γψ(){float Щ=-1;ι α=ι.Ȫ;
if(Ξ!=Ν.ί){ΖІ();if(ζ!=Σ.ы){Vector3 С=Ř(м,ĉ*-1,β-Ζ.Ε);if(Math.Abs(С.X)<=(float)(Ћ*ǰ)/2f&&Math.Abs(С.Y)<=(float)(Ь*Ȍ)/2f){if(С.Z<=-1&&С.Z>=-Ų*2) return ι.ΑІ;if(С.Z>-1&&С.Z<θ*2) return ι.Ȫ;}if(ĳ(Ζ.Ε,ref Щ)) α=ι.Ȫ;}if(Ρ.ν){if(ĳ(Ρ.Ε,ref Щ)) α=ι.ȩ;for(int i=0;i<Ψ.Count;i++)
if(ĳ(Ψ[i].Ε,ref Щ)) α=ι.Ė;if(Vector3.Distance(β,Ρ.Ε)<dockDist*θ) α=ι.ȩ;if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null)
α=ι.ȩ;}}else{Vector3 ά=new Vector3();IMyShipConnector ā=о(MyShipConnectorStatus.Connected);if(Ρ.ν){if(ĳ(Ρ.Ε,ref Щ)) α=ι.ȩ;if(Ȭ(Ρ,dockDist,true,out ά))
if(ĳ(ά,ref Щ)) α=ι.ȩ;if(ā!=null&&Vector3.Distance(ā.GetPosition(),Ρ.Ε)<5)
return ι.Γμ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ρ.Ε)>dockDist*θ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*θ)
if(ĳ(Ψ[i].Ε,ref Щ)) α=ι.Ė;if(Ζ.ν){if(ĳ(Ζ.Ε,ref Щ)) α=ι.Ȫ;if(Ȭ(Ζ,dockDist,true,out ά))
if(ĳ(ά,ref Щ)) α=ι.Ȫ;if(ā!=null&&Vector3.Distance(ā.GetPosition(),Ζ.Ε)<5)
return ι.Γν;}}return α;}bool ĳ(Vector3 ά,ref float Щ){float υ=Vector3.Distance(ά,β);if(υ<Щ||Щ==-1){Щ=υ;return true;}return false;}void Δγ(){if(Е>0){Μ="Setup error! Can't start";return;}if(Ξ==Ν.ί){Ĵ();return;}Ζ.Ε=β;Ζ.ϋ=Ω.GetNaturalGravity();Ζ.μ=ΒЂ;Ζ.λ=ΑΣ;Ζ.Ѓ=Αε;
м=Ǫ.WorldMatrix.Forward;ĉ=Ζ.λ;if(м==Ω.WorldMatrix.Down) ĉ=Ω.WorldMatrix.Backward;ΑΓ(true,true);Υ(Α.ǵ);Δβ();}void ΑΓ(bool ǿ,bool Λω){if(ζ==Σ.ы&&!ǿ) return;bool Λθ=ǿ||ζ==Σ.Γϊ||Ћ!=ј||Ь!=ї||Ŧ!=Ȥ;if(Λθ){if(ζ!=Σ.ы){ζ=Σ.ΑЉ;ĕ(Є,Ё,Λω);Μ="Job changed, lost progress";
}Ŧ=Ȥ;Ћ=ј;Ь=ї;Ё=0;Є=0;й="";Й=-1;Ū=0;Ĩ=0;п=0;з=0;Δκ(true);}}void ΔΧ(){е(β,0);Ǣ(Đ,true);}void Δβ(){ȯ(ь.ΒΤ);ΔΧ();Ǌ(Ĉ,false);ȓ(ΕЇ);ζ=Σ.Л;Ζέ(true);Ǘ=ζ;ξ(Φ.ǅ);ΚΡ();Βσ=true;ΒІ=0;for(int i=ΑΕ.Count-1;i>=0;i--)
if(ǃ(ΑΕ[i],false)) ΒІ++;if(ΒІ>0) Μ="Started with damage";}void Ѕ(){if(ζ==Σ.Л){ζ=Σ.ƅ;Μ="Job paused";}Υ(Α.є);Ǘ=ζ;ΒΧ(false,0,0,0,0);Δη();ΖΨ(new Vector3(),false);ΗΛ();Δέ(ChargeMode.Auto);Δί(false);ΗΒ(true);ė(Α.є);ř(false,false,0,0);Ǣ(ύ,false);Ǣ(Ǒ,true);ȓ(true);ΓΡ=false;ΑЋ();
ΑΡ=false;ΔΩ();Βσ=false;Ασ=false;Αό=false;if(Ѐ!=Φ.Main&&Ѐ!=Φ.ΑЇ&&Ѐ!=Φ.Γι&&Ѐ!=Φ.ΒΨ)
ξ(Φ.Main);}void Ĵ(){ι в=Γψ();if(Ξ==Ν.ί){if(!Ζ.ν||!Ρ.ν) return;Δβ();bool ΕΙ=Vector3.Distance(β,Ρ.Ε)<Vector3.Distance(β,Ζ.Ε);if(Ƭ==Σ.ź) ΕΙ=true;if(Ƭ==Σ.Ż) ΕΙ=false;if(ΕΙ){Ƈ(Ρ,Σ.ź);switch(в){case ι.Γμ:Υ(Α.ĺ);break;case ι.Ė:Υ(Α.т);break;case ι.ȩ:Υ(Α.ȏ);break;default:Υ(Α.Ǻ);break;
}}else{Ƈ(Ζ,Σ.Ż);switch(в){case ι.Γν:Υ(Α.ĺ);break;case ι.Ȫ:Υ(Α.ȏ);break;case ι.Ė:Υ(Α.т);break;default:Υ(Α.Ǻ);break;}}}else{if(ζ!=Σ.ƅ&&ζ!=Σ.ΑЉ) return;bool ΜΠ=ζ==Σ.ΑЉ;Δβ();bool ΕΠ=Ǆ(false)&&Ρ.ν;switch(в){case ι.Ȫ:Υ(ΕΠ?Α.Ĕ:Α.ǵ);break;case ι.Ė:Υ(ΕΠ?Α.И:Α.т);break;case ι.ȩ:Υ(ΕΠ?Α.ȏ:Α.ȱ);break;
case ι.ΑІ:{if(Ĩ!=з||ΜΠ) Υ(Α.ȑ);else Υ(Α.ŕ);}break;default:break;}}}void œ(){if(ζ==Σ.ы&&!Ρ.ν) return;if(Ξ==Ν.ί&&(!Ζ.ν||!Ρ.ν)) return;Μ="Move to job";ι в=Γψ();if(Ξ==Ν.ί){Ƈ(Ζ,Σ.Ż);switch(в){case ι.Ȫ:Υ(Α.ȏ);break;case ι.Ė:Υ(Α.т);break;case ι.Γν:return;default:Υ(Α.Ǻ);break;
}ė(Α.ĺ);}else{switch(в){case ι.Ȫ:Υ(Α.т);break;case ι.Ė:Υ(Α.т);break;case ι.ȩ:Υ(Α.ȱ);break;case ι.ΑІ:Υ(Α.ȑ);break;default:break;}if(ζ==Σ.ы) ė(Α.т);else ė(Α.Ĳ);ΓΡ=true;}ΔΧ();ξ(Φ.Δζ);Ǌ(Ĉ,false);Ǘ=Σ.ΒΛ;}void ΑБ(){if(!Ρ.ν) return;Μ="Move home";ι в=Γψ();if(Ξ==Ν.ί){Ƈ(Ρ,Σ.ź);switch(в)
{case ι.Ė:Υ(Α.И);break;case ι.ȩ:Υ(Α.ȏ);break;case ι.Γμ:return;default:Υ(Α.Ǻ);break;}ė(Α.ĺ);}else{if(о(MyShipConnectorStatus.Connected)!=null) return;if(о(MyShipConnectorStatus.Connectable)!=null){Υ(Α.ő);ė(Α.ȱ);return;}switch(в){case ι.Ȫ:Υ(Α.Ĕ);break;case ι.Ė:Υ(Α.И);break;case ι.ȩ:Υ(Α.И);break;
case ι.ΑІ:Υ(Α.Ƿ);break;default:break;}ė(Α.ȱ);}ΔΧ();ξ(Φ.Δζ);Ǌ(Ĉ,false);Ǘ=Σ.ΒΚ;}void Υ(Α Εο){if(Εο==Α.є) Αφ=Α.є;if(Αφ!=Α.є&&Ϋ==Αφ&&Εο!=Αφ){Ѕ();return;}Ж=true;Ϋ=Εο;}void ė(Α Ј){this.Αφ=Ј;}ǹ Μέ(int Αα,bool ȇ){if(ȇ){ǭ=null;Є=0;Ё=0;}if(Ȥ==ѕ.ΑΒ){int ŭ=Αα+1;
Ё=(int) Math.Floor(М(Αα,Ћ));if(Ё%2==0) Є=Αα-(Ё*Ћ);else Є=Ћ-1-(Αα-(Ё*Ћ));if(Ё>=Ь) return ǹ.ΑЅ;else return ǹ.ΒΒ;}else if(Ȥ==ѕ.Ζί){if(ǭ==null) ǭ=new int[]{0,-1,0,0};int Αή=(int) Math.Ceiling(Ћ/2f);int Αέ=(int) Math.Ceiling(Ь/2f);int ΕΞ=(int) Math.Floor(Ћ/2f);
int ΕΝ=(int) Math.Floor(Ь/2f);int Αβ=0;while(ǭ[2]<Math.Pow(Math.Max(Ћ,Ь),2)){if(Αβ>200) return ǹ.Ιλ;Αβ++;ǭ[2]++;if(-Αή<Є&&Є<=ΕΞ&&-Αέ<Ё&&Ё<=ΕΝ){if(ǭ[3]==Αα){this.Є=Є-1+Αή;this.Ё=Ё-1+Αέ;return ǹ.ΒΒ;}ǭ[3]++;}if(Є==Ё||(Є<0&&Є==-Ё)||(Є>0&&Є==1-Ё))
{int ΛЍ=ǭ[0];ǭ[0]=-ǭ[1];ǭ[1]=ΛЍ;}Є+=ǭ[0];Ё+=ǭ[1];}}return ǹ.ΑЅ;}void Κς(){if(Ϋ==Α.ǵ){if(Ж){Ќ=0;if(Ĩ!=з) Ū=0;Ĩ=з;}if(Ќ==0){int ΑΜ=ΙΫ(з);if(ΑΜ<0){if(!ΑЊ()){Υ(Α.Ƿ);return;}if(!Βο||(ΑΜ==-2)!=Βξ){Βο=true;Βξ=ΑΜ==-2;Ηπ=!Βξ?β:Ζ.Ε-м*θ+
Ζ.Ѓ*(Ћ*ǰ/2+ΓΣ/2+θ/2);}е(Ηπ,10);Μ=Βξ?"Waiting for a free hole":"Waiting for the controller";return;}if(Βο) Μ="";Βο=false;if(ΑΜ!=з){з=ΑΜ;Ū=0;Ĩ=з;}ǹ α=ΚΫ(з);if(α==ǹ.ΑЅ){ζ=Σ.Γϊ;Μ="Job done";if(ŋ&&Ρ.ν){Υ(Α.Ĕ);ė(Α.ȱ);Ǘ=Σ.ΒΚ;}else{Υ(Α.ſ);ė(Α.Ĳ);Ǘ=Σ.ΒΛ;}
return;}if(α==ǹ.ΒΒ){Ќ=1;Ǣ(ύ,true);Ю=ĕ(Є,Ё,true);float h=Vector3.Dot(β-Ю,м);е(h<-1?Ю+м*h:Ю,10);с(Ζ.λ,Ζ.μ,Ζ.Ѓ,false);}}else{if(Й>=0&&(ΖЄ()?Ș>=0&&Ș!=Й:ΓЊ(Й,true))){Ќ=0;return;}if(и<wpReachedDist){if(Vector3.Distance(ώ,Ю)>1){е(Ю,10);return;}Υ(Α.ŕ);
return;}Vector3 ΓΙ=Ю-β;float Ļ=Vector3.Dot(ΓΙ,м);if(Ļ>0&&(ΓΙ-м*Ļ).Length()<wpReachedDist&&Ď<0.3f){if(++Ќ>30){Βη=β;Υ(Α.ŕ);}return;}Ќ=1;}}if(Ϋ==Α.ŕ){if(Ж){Ǣ(ύ,true);ȓ(false);Ю=ĕ(Є,Ё,false);е(ΓЃ(Ю,0),0);с(Ζ.λ,Ζ.μ,Ζ.Ѓ,false);Ќ=1;љ=0;щ=0;Βε=0;Εέ=-1;
Ų=ф;ΕΦ=true;}if(!ΑЊ()){Υ(Α.Ƿ);return;}if(Ǆ(true)){Θλ=Ē("","ORE",Я.Ŏ);if((к==З.Γϋ||к==З.ΑЌ||
к==З.ΓЌ||к==З.ΒΕ)&&Ξ!=Ν.Ƕ)
Υ(Α.Œ);else if((к==З.Ζδ||к==З.Γτ)&&Ξ!=Ν.Ƕ)
Υ(Α.Δλ);else
Υ(Α.Ƿ);return;}п=Vector3.Distance(β,Ю);if(п>Ū){Ū=п;ΕΦ=false;}if(Ξ==Ν.Ƕ&&Ζπ()==MyDetectedEntityType.SmallGrid)
щ+=2;else щ-=2;щ=Math.Max(100,Math.Min(400,щ));if(Ќ>0&&Ќ<щ){if(п>љ){if(щ>150) љ=п;else љ=(float) Math.Ceiling(п);Ќ=1;}else Ќ++;}else{if(Ќ>0){Μ="Ship stuck! Retrying";љ=п;Ќ=0;ř(false,true,0,θ*sensorRange);}е(ΓЃ(Ю,Math.Max(0,љ-θ)),ΓІ(false));if(и<=wpReachedDist/2){Ќ=1;љ=0;}
return;}ř(false,true,θ*sensorRange,0);Vector3 ΚϏ=Ю+м*п;bool ΘΛ=false;if(Vector3.Distance(ΚϏ,β)>0.3f){Vector3 ΚЏ=Ю+м*(п+0.1f);е(ΚЏ,4);ΘΛ=true;}else{float Ȉ=ΓІ(true);Vector3 Ηχ=ΓЃ(Ю,Math.Max(ф+1,п+1));Ȓ(true,false,false,Ηχ,Ηχ-Ю,Ȉ,Ȉ);}bool ΔЎ=false;if(ǒ==ǟ.Γο||ǒ==ǟ.Γξ)
{if(!ΘΛ){float Ερ=0;foreach(IMyTerminalBlock Ȩ in ύ)
Ερ+=Ιδ(Ȩ,"","",ǒ==ǟ.Γξ?new string[]{"STONE"}:null);if(Ερ>Εέ||п<ф||ΕΦ){Δχ=0;Βε=п;Ų=(int)(Math.Max(Ų,Βε)+θ/2);}else{ΔЎ=п-Βε>2&&Δχ>=20;Δχ++;}Εέ=Ερ;}}else ΔЎ=п>=Ų;if(Ĩ!=з){Υ(Α.ȑ);п=0;return;}if(ΔЎ){з++;Υ(Α.ȑ);п=0;return;}}if(Ϋ==Α.ȑ||Ϋ==Α.Ƿ||Ϋ==Α.Œ)
{if(Ж){Ю=ĕ(Є,Ё,false);с(Ζ.λ,Ζ.μ,Ζ.Ѓ,false);Ǣ(ύ,ƕ);ȓ(false);љ=Vector3.Distance(β,Ю);ř(false,true,0,θ*sensorRange);}if(Ж) ΑΧ=false;е(ΑΧ?Ю-м*θ:Ю,ΓІ(false));if(!ΑΧ&&Vector3.Distance(β,Ю)>=љ+5){Ǣ(ύ,false);ȓ(true);Μ="Can´t return!";}if(и<wpReachedDist){if(Ϋ==Α.Ƿ&&!ΑΧ){ΑΧ=true;return;}
if(Ϋ==Α.ȑ&&ΓΡ) Υ(Α.ſ);if(Ϋ==Α.ȑ) Υ(Α.ǵ);if(Ϋ==Α.Œ) Υ(Α.ΑА);if(Ϋ==Α.Ƿ){if(Ρ.ν) Υ(Α.Ĕ);else{Ѕ();œ();Μ="Can´t return, no dock found";}}return;}}if(Ϋ==Α.И||Ϋ==Α.т){if(Ϋ==Α.т&&ζ==Σ.Л&&Ξ!=Ν.ί){if(!ΑЊ()||Ǆ(true)){Υ(Α.И);return;}}bool Εϊ=false;bool Γγ=false;
bool ΗΫ=false;float ƍ=0;bool Θκ=false;έ ή=null;if(Ж){if(Ϋ==Α.И||Ξ==Ν.ί){if(Ξ!=Ν.ί){if(Ę(1)) Γλ();else{ǩ=null;Č=0;ΑΡ=!Γσ(1);}}έ γ=ѓ();ρ=new ΔΦ();ρ.ε=γ;ρ.ΑΟ=followPathDock*θ;ρ.ΖΛ=useDockDirectionDist*θ;ρ.ƍ=10;ρ.Ʋ.Add(γ.Ε);Vector3 ƌ=new Vector3();if(Ȭ(γ,dockDist*θ,true,out ƌ))
ρ.Ʋ.Add(ƌ);else ρ.ΑΟ*=1.5f;if(Ξ==Ν.ί){if(γ==Ρ) ρ.ΒϏ=Ζ.Ε;if(γ==Ζ) ρ.ΒϏ=Ρ.Ε;ρ.ΕΖ=dockDist*θ*1.1f;}}else if(Ϋ==Α.т){ρ=new ΔΦ();ρ.ε=Ζ;ρ.ΑΟ=followPathJob*θ;ρ.ΖΛ=useJobDirectionDist*θ;ρ.ƍ=10;ρ.ΒϏ=Ρ.Ε;ρ.ΕΖ=dockDist*θ*1.1f;ρ.Ʋ.Add(Ζ.Ε);if(ζ==Σ.ы){if(!Ρ.ν||Ψ.Count==0){Ѕ();return;}
float ΛΕ=Vector3.Distance(Ψ.First().Ε,Ρ.Ε);float ΛΖ=Vector3.Distance(Ψ.Last().Ε,Ρ.Ε);if(ΛΕ<ΛΖ) ρ.ε=Ψ.Last();else ρ.ε=Ψ.First();}}Αν=new Vector3();Θκ=!ΙЊ(β);Ǣ(ύ,false);ȓ(true);р=-1;double ȗ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ρ.ΒϏ)<=ρ.ΕΖ) continue;
double υ=Vector3.Distance(Ψ[i].Ε,β);if(ȗ==-1||υ<ȗ){р=i;ȗ=υ;}}Ȣ=Ιψ(ρ.ε.Ε,р);Ş=null;}if(Ϋ==Α.И&&Ξ!=Ν.ί&&!Ж&&!ǌ()){έ Δό=ѓ();ΑΡ=!Γσ(1);if(ѓ()!=Δό){Υ(Α.И);return;}}if(Ϋ==Α.И&&Ξ!=Ν.ί&&!Ж&&Ę(1)&&Γλ()){Υ(Α.И);return;}bool ΘΙ=false;if(Ϋ==Α.И&&!Ж&&р>=0&&р<Ψ.Count)
{Vector3 ΑΞ=Ψ[р].Ε+Ζσ(р,Ȣ);int Επ=р+Ȣ;Vector3 ŭ=Ȣ!=0&&Επ>=0&&Επ<Ψ.Count?Ψ[Επ].Ε:ρ.ε.Ε;if(ΙЃ(ΑΞ,ŭ)){if(!Βψ){Βψ=true;Ηό=ƃ(ΑΞ)?β:ΑΞ;}е(Ηό,0);с(ρ.ε.λ,ρ.ε.μ,ρ.ε.Ѓ,false);Μ="Waiting for dock clearance";return;}Βψ=false;ΘΙ=Ę(1)&&(ƃ(β)||ƃ(ŭ));}ΙΥ(Ψ,Ȣ,ρ.Ʋ,ρ.ΑΟ,Ж,ref Ќ);
for(int i=0;i<ρ.Ʋ.Count;i++){float υ=Vector3.Distance(β,ρ.Ʋ[i]);if(υ<=ρ.ΑΟ) Εϊ=true;if(υ<=ρ.ΖΛ) Γγ=true;}if(ΘΙ) Γγ=true;if(Γγ) ƍ=ρ.ƍ;float Μβ=Ş!=null?Ş.Т:Ď;float ΜΧ=(float) Math.Max(Ď*0.1f*θ,wpReachedDist);if((и<ΜΧ)||Ж){if(!Ж) р+=Ȣ;if(Ȣ==0||р>Ψ.Count-1||р<0)
Εϊ=true;else{ΖΞ=Ȣ>0?Ψ.Count-1-р:р;ή=Ψ[р];Ş=ή;if(р>=1&&р<Ψ.Count-1)
Αν=ή.Ε-Ψ[р-Ȣ].Ε;else Ş=null;ώ=ή.Ε+Ζσ(р,Ȣ);ΗΫ=true;}}if(Γγ)
с(ρ.ε.λ,ρ.ε.μ,ρ.ε.Ѓ,false);else if(Θκ)
Γκ(ρ.ε.λ,10,true);else if(ΗΫ&&ή!=null){if(Ȣ>0) с(ή.λ,ή.μ,ή.Ѓ,90,false);else с(ή.λ,-ή.μ,-ή.Ѓ,90,false);}Ȓ(true,false,true,ώ,Αν,Ş==null?0:Ş.Т,ƍ);if(Εϊ){ΖΞ=0;if(Ϋ==Α.И&&!ΙΩ()){е(ώ,0);return;}if(Ϋ==Α.И&&ΑΡ){е(ώ,0);Μ="Waiting for a free dock";return;}
if(Ϋ==Α.И||Ξ==Ν.ί){Υ(Α.ȏ);return;}if(Ϋ==Α.т&&ΓΡ){Υ(Α.ſ);return;}if(Ϋ==Α.т){Υ(Α.ǵ);return;}}}if(Ϋ==Α.ȏ||Ϋ==Α.ǉ){if(Ξ!=Ν.ί&&ǌ()){if(!Ę(1)){Υ(Α.И);return;}}else if(Ξ!=Ν.ί){έ Δό=ѓ();if(!Γσ(2)||ѓ()!=Δό){Υ(Α.И);return;}}έ γ=ѓ();if(Ж){if(!Ȭ(γ,dockDist*θ,true,out ώ))
{ȯ(ь.ƾ);Ѕ();return;}е(ώ,0);Γκ(γ.λ,90,true);}if(и<followPathDock*θ&&и!=-1){е(ώ,10);с(γ.λ,γ.μ,γ.Ѓ,false);}if(о(MyShipConnectorStatus.Connectable)!=null||о(MyShipConnectorStatus.Connected)!=null){Υ(Α.ő);return;}if(и<wpReachedDist/2&&и!=-1){Υ(Α.ō);return;}}if(Ϋ==Α.ō||Ϋ==Α.Ĳ)
{if(Ж){if(Ϋ==Α.ō){έ γ=ѓ();if(!Ȭ(γ,dockDist*θ,true,out ώ)){ȯ(ь.ƾ);Ѕ();return;}Ȓ(true,true,false,ώ,0);с(γ.λ,γ.μ,γ.Ѓ,10,false);}if(Ϋ==Α.Ĳ){с(Ζ.λ,Ζ.μ,Ζ.Ѓ,0.5f,false);ώ=Ζ.Ε;Ȓ(true,true,false,ώ,0);}}if(ΒЈ){ΒΧ(false,0,0,0,0);if(Ϋ==Α.ō) Υ(Α.ő);if(Ϋ==Α.Ĳ) Ѕ();return;
}}if(Ϋ==Α.ő){if(о(MyShipConnectorStatus.Connected)!=null){if(Ξ==Ν.ί) Υ(Α.ĺ);else Υ(Α.ȱ);return;}έ γ=ѓ();if(Ж){щ=0;ΛΗ=DateTime.Now;Ќ=0;с(γ.λ,γ.μ,γ.Ѓ,false);}Vector3I ΕΛ=new Vector3I((int) γ.х.X,(int) γ.х.Y,(int) γ.х.Z);IMySlimBlock Ην=Me.CubeGrid.GetCubeBlock(ΕΛ);float ΗϏ=dockingSpeed;
float Λβ=dockingSpeed*5;float ΗΩ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ȭ(γ,0,true,out ώ)||!Ȭ(γ,ΗΩ,true,out Αν)||
Ην==null||!Ην.FatBlock.IsFunctional){ȯ(ь.ƾ);Ѕ();return;}if(щ==1||(Vector3.Distance(β,ώ)<=ΗΩ*1.1f&&!Ж))
щ=1;else{Vector3 ΜΕ=ъ(Ω,Αν-β);Vector3 ΕΚ=ъ(Ω,Ω.GetNaturalGravity());float Λό=ΑЈ(ΜΕ,ΕΚ,null);ΗϏ=Math.Min(Λβ,Λό);}Ȓ(true,false,false,ώ,ώ-β,dockingSpeed,ΗϏ);if(Ж) љ=(float) и;IMyShipConnector ΔЉ=о(MyShipConnectorStatus.Connectable);if(ΔЉ!=null){Ȓ(false,false,false,ώ,0);
if(Ќ>0) Ќ=0;Ќ--;if(Ќ<-5){ΔЉ.Connect();if(ΔЉ.Status==MyShipConnectorStatus.Connected){if(Ξ==Ν.ί) Υ(Α.ĺ);else Υ(Α.ȱ);Δη();Ǌ(Ĉ,true);return;}}}else{float Ηϋ=(float) Math.Round(и,1);if(Ηϋ<љ){Ќ=-1;љ=Ηϋ;}else Ќ++;if(Ќ>20){Υ(Α.ǉ);return;}}}if(Ϋ==Α.ȱ||Ϋ==Α.ĺ||
Ϋ==Α.Βί||Ϋ==Α.ΑЎ||Ϋ==Α.Ő){bool Δυ=false,Δφ=false;if(Ξ==Ν.ί){if(ѓ()==Ρ) Δυ=true;else if(ѓ()==Ζ) Δφ=true;}if(Ж){Ασ=false;if(о(MyShipConnectorStatus.Connected)==null){Υ(Α.Ǻ);return;}Δη();if(Δυ) Βά(Ѝ.Ǿ);if(Δφ) Βά(Ў.Ǿ);Ŷ=false;Ě=false;Ģ=false;Αώ=false;}if(о(MyShipConnectorStatus.Connected)==null){Ѕ();ȯ(ь.ΖϏ);return;}
if(ζ!=Σ.Л||ǘ==-1||ȕ==ǎ.None) Ě=true;else if(ȳ>=100f) Ě=true;else if(ȳ<=99f) Ě=false;if(ζ!=Σ.Л||Ǚ==-1||Ǧ.Count==0) Ģ=true;else if(ģ>=100f) Ģ=true;else if(ģ<=99) Ģ=false;if(ζ!=Σ.Л||ц==-1||ň.Count==0) Αώ=true;else Αώ=ŷ>=ц;Ɖ Ǎ=null;if(Δυ) Ǎ=Ѝ;
if(Δφ) Ǎ=Ў;if(Ǎ!=null&&(Ǎ.Х==τ.ΒΟ||Ǎ.Х==τ.ΒΞ)) Ě=true;if(Ǎ!=null&&(Ǎ.Х==τ.ΔΝ)) if(!Ŷ) Ě=false;if(Ǎ!=null&&(Ǎ.Х==τ.ΔΡ||Ǎ.Х==τ.ΔΟ)) Ģ=true;if(Ǎ!=null&&(Ǎ.Х==τ.ΔΠ)) if(!Ŷ) Ģ=false;if(Ĥ){ChargeMode Ηζ=Ě?ChargeMode.Auto:ChargeMode.Recharge;if(Ǎ!=null&&(Ǎ.Х==τ.ΒΞ||Ǎ.Х==τ.ΒΟ))
Ηζ=ChargeMode.Discharge;Δέ(Ηζ);Δί(!Ģ);}if(!Ŷ){if(Ξ==Ν.ί)
Ŷ=ζ!=Σ.Л||ΙΪ(Ж,true)||Αό;else
Ŷ=ζ!=Σ.Л||ΖЀ();}else{if(!Ě) Υ(Α.Ő);if(!Ģ) Υ(Α.ΑЎ);if(!Αώ) Υ(Α.Βί);Ж=false;}if(Ŷ&&Ě&&Ģ&&Αώ){Δέ(ChargeMode.Auto);Δί(false);if(ζ==Σ.Л){if(Ξ==Ν.ί){if(ѓ()==Ρ) Βά(Ѝ.ȃ);else if(ѓ()==Ζ) Βά(Ў.ȃ);if(ѓ()==Ρ) Ƈ(Ζ,Σ.Ż);else Ƈ(Ρ,Σ.ź);}}Υ(Α.Ǻ);return;}}if(Ϋ==Α.Ǻ){if(ΑЁ){if(!Ζα()) return;
ΑЁ=false;Υ(Α.Ǻ);return;}if(Ж){if(!Ζα()){ΑЁ=true;return;}IMyShipConnector ā=о(MyShipConnectorStatus.Connected);if(ā==null){Υ(Α.т);return;}IMyShipConnector η=ā.OtherConnector;ΗΑ(ā,false);Ǌ(Ĉ,false);έ γ=null;έ ǔ=Ξ==Ν.ί?Ρ:ѓ();if(Vector3.Distance(ā.GetPosition(),ǔ.Ε)<5f&&Ρ.ν) γ=ǔ;
if(Vector3.Distance(ā.GetPosition(),Ζ.Ε)<5f&&Ζ.ν) γ=Ζ;if(γ!=null){if(!Ȭ(γ,dockDist*θ,true,out ώ)){ȯ(ь.ƾ);Ѕ();return;}е(ώ,5);с(γ.λ,γ.μ,γ.Ѓ,false);}else е(β+η.WorldMatrix.Forward*dockDist*θ,5);if(ζ==Σ.Л) ȯ(ь.ΒΤ);}if(и<wpReachedDist){Ǣ(Ǒ,true);Υ(Α.т);return;}}if(Ϋ==Α.ſ)
{if(Ж){ȓ(true);Ǣ(ύ,false);ώ=Ζ.Ε;е(ώ,20);с(Ζ.λ,Ζ.μ,Ζ.Ѓ,false);}if(и<wpReachedDist/2){Υ(Α.Ĳ);return;}}if(Ϋ==Α.Ĕ){if(Ж){ȓ(true);Ǣ(ύ,false);int Щ=-1;double ȗ=-1;bool Θω=ΔΚ();for(int ċ=0;ċ<2&&Щ==-1;ċ++,Θω=false)
for(int i=Ψ.Count-1;i>=0;i--){if(Θω&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)<ΖЅ()+θ) continue;double υ=Vector3.Distance(Ψ[i].Ε,β);if(ȗ==-1||υ<ȗ){Щ=i;ȗ=υ;}}if(Щ==-1){Υ(Α.И);return;}ώ=Ψ[Щ].Ε;е(ώ,10);с(Ζ.λ,Ζ.μ,Ζ.Ѓ,false);}if(и<wpReachedDist){Υ(Α.И);return;}}if(Ϋ==Α.ΑА)
{bool φ=false;if(Ж){ȓ(true);if((к==З.Γϋ||к==З.ΑЌ)&&ΒΗ()&&
Ž(м,Ω.GetNaturalGravity())<25&&Ћ>=2&&Ь>=2){Vector3 ΑΤ=β;if(Є>0&&Ё<Ь-1) ΑΤ=ĕ(Є-1,Ё+1,true);else if(Є<Ћ-1&&Ё<Ь-1) ΑΤ=ĕ(Є+1,Ё+1,true);else if(Є<Ћ-1&&Ё>0) ΑΤ=ĕ(Є+1,Ё-1,true);else if(Є>0&&Ё>0) ΑΤ=ĕ(Є-1,Ё-1,true);else φ=true;
if(!φ) е(ΑΤ,10);}else φ=true;}if(и<wpReachedDist/2) φ=true;if(φ){Υ(Α.Βή);return;}}if(Ϋ==Α.Δλ||Ϋ==Α.Βή){if(Ж){Ȓ(true,true,false,β,0);Ǣ(ύ,false);ȓ(true);Ќ=-1;щ=к==З.ΓЌ||к==З.ΒΕ?0:-1;}bool š=!ΑЊ();int Γά=Ē("STONE","ORE",Я.Ŏ);if(к==З.Γτ||к==З.ΒΕ||к==З.ΑЌ)
Γά+=Ē("ICE","ORE",Я.Ŏ);bool ΛΡ=Γά>0;bool ΕΕ=false;if(щ>=0){float Ɗ=(float) Math.Sin(Ζζ(щ))*ǰ/3f;float ΚЁ=(float) Math.Cos(Ζζ(щ))*Ȍ/3f;Vector3 ΚЈ=ĕ(Є,Ё,true)+Δι(м,ĉ*-1,new Vector3(Ɗ,ΚЁ,0));е(ΚЈ,0.3f);if(и<Math.Min(ǰ,Ȍ)/10f) щ+=5f;if(щ>=360) щ=0;}if(Ќ==-1||Γά<Ќ){Ќ=Γά;љ=0;}
else{љ++;if(љ>50) ΕΕ=true;}if(!ΛΡ||š||ΕΕ){if(!š){int ΜΚ=Ē("","ORE",Я.Ŏ);if(Ǆ(true)) š=true;else if(100-(М(ΜΚ,Θλ)*100)<minEjection) š=true;else ȯ(ь.ΒΤ);}if(ΕΕ&&š) Μ="Ejection failed";if(Ϋ==Α.Βή){if(š){if(Ρ.ν) Υ(Α.Ĕ);else{Ѕ();œ();Μ="Can´t return, no dock found";}}
else Υ(Α.ǵ);}else if(š) Υ(Α.Ƿ);else Υ(Α.ŕ);return;}}Ж=false;}void ΙΥ(List<έ>Ō,int π,List<Vector3>Αω,float Ȋ,bool ȇ,ref int Ǽ){if(ȇ){for(int i=0;i<Ō.Count;i++) Ō[i].Т=0;Ǽ=-1;return;}if(π==0) return;int Ǯ=π*-1;if(Ǽ==-1) Ǽ=Ǯ>0?1:Ō.Count-2;int Αβ=0;while(Ǽ>=1&&Ǽ<Ō.Count-1)
{if(Αβ>50) return;Αβ++;try{if((Ǯ<0&&Ǽ>=1)||(Ǯ>0&&Ǽ<=Ō.Count-2)){έ σ=Ō[Ǽ];bool Θζ=false;for(int j=0;j<Αω.Count;j++)
if(Vector3.Distance(σ.Ε,Αω[j])<=Ȋ){Θζ=true;break;}if(!Θζ){έ ΓΜ=Ō[Ǽ-Ǯ];έ ŭ=Ō[Ǽ+Ǯ];Vector3 ΜΖ=σ.Ε-ŭ.Ε;Vector3 ΖΓ=ΓΜ.Ε-σ.Ε;Vector3 Λχ=σ.Ε+Vector3.Normalize(ΜΖ)*ΖΓ.Length();Vector3 ΛΓ=ΓΜ.Ε-Λχ;Vector3 ΒЎ=Ř(π>0?σ.μ:σ.μ*-1,σ.λ*-1,ΛΓ);Vector3 Λα=Ř(π>0?σ.μ:σ.μ*-1,σ.λ*-1,ΖΓ);
Vector3 ΒА=Ř(π>0?σ.μ:σ.μ*-1,σ.λ*-1,σ.ϋ);σ.Т=(float) Math.Sqrt(Math.Pow(ΓΜ.Т,2)+Math.Pow(ΑЈ(-Λα,ΒА,σ),2));for(int j=0;j<Αω.Count;j++)
if(Vector3.Distance(ΓΜ.Ε,Αω[j])<=Ȋ){Vector3 ΒБ=Ř(π>0?σ.μ:σ.μ*-1,σ.λ*-1,Αω[j]-σ.Ε);float ΛЌ=ΑЈ(-ΒБ,ΒА,σ);σ.Т=Math.Min(σ.Т,ΛЌ)/2f;}if(ΒЎ.Length()==0) ΒЎ=new Vector3(0,0,1);Vector3 Λΰ=Ř(σ.μ,σ.λ*-1,σ.ϋ);float ΛБ=ǁ(-ΒЎ,Λΰ,σ);float Βΰ=М(ΛБ,Ŵ);float ΕБ=(float) Math.Sqrt(М(ΒЎ.Length()*1.0f,0.5f*Βΰ));
σ.Т=Math.Min(σ.Т,(ΖΓ.Length()/ΕБ)*Ȕ);}}}catch{return;}Ǽ+=Ǯ;}Ǽ=-1;}void Δκ(bool ȇ){if(ȇ){Αδ=0;return;}if(Ξ==Ν.ί) return;float φ=з*Math.Max(1,ф);if(Ĩ==з) φ+=Math.Min(ф,п);float ΜΙ=Ћ*Ь*Math.Max(1,ф);Αδ=Math.Max(Αδ,(float) Math.Min(φ/ΜΙ*100.0,100));}MyDetectedEntityType Ζπ()
{try{if(ȭ(ό,true)&&!ό.LastDetectedEntity.IsEmpty()) return ό.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΓІ(bool Ȟ){if(Ξ==Ν.Ƕ&&Ζπ()==MyDetectedEntityType.None&&!ǃ(ό,true))
return fastSpeed;else return Ȟ?Ƽ:ƻ;}void Κθ(){float Θή=0,Ηυ=0,ť=0,Ες=0;for(int i=0;i<ļ.Count;i++){IMyBatteryBlock ΑΙ=ļ[i];if(!ȭ(ΑΙ,true)) continue;Θή+=ΑΙ.MaxStoredPower;Ηυ+=ΑΙ.CurrentStoredPower;ť+=ΑΙ.CurrentInput;Ες+=ΑΙ.CurrentOutput;}ȳ=(float) Math.Round(М(Ηυ,Θή)*100,1);
if(ť>=Ες) ȕ=ǎ.Ő;else ȕ=ǎ.Ζη;if(ť==0&&Ες==0||ȳ==100.0) ȕ=ǎ.є;if(ļ.Count==0) ȕ=ǎ.None;}void Κο(){float ΗЅ=0;for(int i=0;i<Ǧ.Count;i++){IMyGasTank ΘЉ=Ǧ[i];if(!ȭ(ΘЉ,true)) continue;ΗЅ+=(float) ΘЉ.FilledRatio;}ģ=М(ΗЅ,Ǧ.Count)*100f;}void Κϋ(){ŷ=0;try{for(int i=0;i<ň.Count;i++)
{IMyReactor Εϋ=ň[i];if(!ȭ(Εϋ,true)) continue;List<MyInventoryItem>Б=new List<MyInventoryItem>();Εϋ.GetInventory(0).GetItems(Б);float А=0;for(int j=0;j<Б.Count;j++){MyInventoryItem Ђ=Б[j];if(Ђ.Type.SubtypeId.ToUpper()=="URANIUM"&&Ђ.Type.TypeId.ToUpper().Contains("_INGOT"))
А+=(float) Ђ.Amount;}if(А<ŷ||i==0){ŷ=А;Εδ=Εϋ.CustomName;}}}catch(Exception e){ħ=e;}}void Κι(){ȡ=0;ș=0;try{for(int i=0;i<Ƒ.Count;i++){IMyTerminalBlock Ϊ=Ƒ[i];if(!ȭ(Ϊ,true)) continue;ș+=(float) Ϊ.GetInventory(0).CurrentVolume;ȡ+=(float) Ϊ.GetInventory(0).MaxVolume;
}ΒЍ=(float) Math.Min(Math.Round(М(ș,ȡ)*100,1),100.0);}catch(Exception e){ħ=e;}}void Κπ(){try{ȁ.Clear();for(int i=0;i<Ƒ.Count;i++){IMyTerminalBlock Ϊ=Ƒ[i];if(!ȭ(Ϊ,true)) continue;ΖΧ(Ϊ,Я.Ƃ);}if(к!=З.ΔΜ){for(int i=0;i<Ŝ.Count;i++){IMyTerminalBlock Ϊ=Ŝ[i];if(!ȭ(Ϊ,true)) continue;
ΖΧ(Ϊ,Я.ΙЈ);}}Κή(ȁ);}catch(Exception e){ħ=e;}}void ΙΨ(){if(Βσ){if(Ζο().Count>ΒІ){Βσ=false;if(ќ!=ђ.ǂ){Ѕ();if(ќ==ђ.ΓЁ) œ();if(ќ==ђ.ΒΣ)
if(Ρ.ν) ΑБ();else œ();}Μ="Damage detected";}}}bool ΑЊ(){if(!ΒЇ) return true;if(ζ==Σ.Л){if(ǘ>0&&ȕ!=ǎ.None)
if(ȳ<=ǘ){Μ="Low energy! Move home";return false;}if(ц>0&&ň.Count>0)
if(ŷ<=ц){Μ="Low fuel: "+Εδ;return false;}if(Ǚ>0&&Ǧ.Count>0)
if(ģ<=Ǚ){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Ζο(){List<IMyTerminalBlock>ş=new List<IMyTerminalBlock>();for(int i=0;i<ΑΕ.Count;i++){IMyTerminalBlock Ϊ=ΑΕ[i];if(ǃ(Ϊ,false)) ş.Add(Ϊ);}return ş;}bool ǃ(IMyTerminalBlock Ϊ,bool ΕЉ){return(!ȭ(Ϊ,ΕЉ)||!Ϊ.IsFunctional);
}bool ȭ(IMyTerminalBlock Ϊ,bool ΕЉ){if(Ϊ==null) return false;try{IMyCubeBlock ΗЋ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(ΕЉ) return ΗЋ==Ϊ;else return ΗЋ.GetType()==Ϊ.GetType();}catch{return false;}}float Ιδ(IMyTerminalBlock Ϊ,String Ǟ,String ΘЀ,String[]Ȝ){float Ă=0;for(int i=0;i<Ϊ.InventoryCount;i++)
{IMyInventory ƚ=Ϊ.GetInventory(i);List<MyInventoryItem>Б=new List<MyInventoryItem>();ƚ.GetItems(Б);foreach(MyInventoryItem Ђ in Б){if(Ȝ!=null&&(Ȝ.Contains(Ђ.Type.TypeId.ToUpper())||Ȝ.Contains(Ђ.Type.SubtypeId.ToUpper())))
continue;if((Ǟ==""||Ђ.Type.TypeId.ToUpper()==Ǟ)&&(ΘЀ==""||Ђ.Type.SubtypeId.ToUpper()==ΘЀ))
Ă+=(float) Ђ.Amount;}}return Ă;}у Ιν(String ΰ,String ω,Я в,bool ΚА){ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<ȁ.Count;i++){у Ђ=ȁ[i];if(Ђ.ΰ.ToUpper()==ΰ&&Ђ.ω.ToUpper()==ω&&(Ђ.Ɵ==в||в==Я.Ŏ))
return Ђ;}у α=null;if(ΚА){α=new у(ΰ,ω,0,в);ȁ.Add(α);}return α;}int Ē(String ΰ,String ω,Я в){return Ē(ΰ,ω,в,null);}int Ē(String ΰ,String ω,Я в,String[]Ȝ){int Ă=0;ΰ=ΰ.ToUpper();ω=ω.ToUpper();for(int i=0;i<ȁ.Count;i++){у Ђ=ȁ[i];if(Ȝ!=null&&Ȝ.Contains(Ђ.ΰ.ToUpper())) continue;
if((ΰ==""||Ђ.ΰ.ToUpper()==ΰ)&&(ω==""||Ђ.ω.ToUpper()==ω)&&(Ђ.Ɵ==в||в==Я.Ŏ))
Ă+=Ђ.А;}return Ă;}void ΖΧ(IMyTerminalBlock Ϊ,Я в){for(int i=0;i<Ϊ.InventoryCount;i++){List<MyInventoryItem>Б=new List<MyInventoryItem>();Ϊ.GetInventory(i).GetItems(Б);for(int j=0;j<Б.Count;j++)
Ιν(Б[j].Type.SubtypeId,Б[j].Type.TypeId.Replace("MyObjectBuilder_",""),в,true).А+=(int) Б[j].Amount;}}void Κή(List<у>В){for(int i=В.Count-1;i>0;i--)
for(int j=0;j<i;j++){у a=В[j];у b=В[j+1];if(a.А<b.А) В.Move(j,j+1);}}bool Ǆ(bool Μί){if(ı&&Ξ!=Ν.ί)
if(л!=-1&&Ŵ>=л){Μ="Ship too heavy";return true;}if(ΒЍ>=Ƞ||Ασ){Ασ=false;Μ="Ship is full";return true;}return false;}bool ΖЀ(){String[]Ȝ=null;if(!İ) Ȝ=new string[]{"ICE"};if(Ξ==Ν.Ǉ) return Ē("","ORE",Я.Ƃ,Ȝ)==0;if(Ξ==Ν.Ƕ) return Ē("","COMPONENT",Я.Ƃ,Ȝ)==0;else return Ē("","",Я.Ƃ,Ȝ)==0;
}bool ΙЋ(){List<IMyCockpit>ΔЇ=new List<IMyCockpit>();χ.GetBlocksOfType(ΔЇ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΔЇ.Count;i++)
if(ΔЇ[i].IsUnderControl) return true;return false;}bool ΙΪ(bool ȇ,bool ΓΥ){IMyShipConnector У=о(MyShipConnectorStatus.Connected);if(У==null) return false;if(Vector3.Distance(Ρ.Ε,У.GetPosition())<5)
return Ζβ(Ѝ,ȇ,ΓΥ);if(Vector3.Distance(Ζ.Ε,У.GetPosition())<5)
return Ζβ(Ў,ȇ,ΓΥ);return false;}bool Ζβ(Ɖ Ā,bool ȇ,bool ΓΥ){if(ȇ) Ā.ΚΜ();Ā.Κχ();bool џ=false;String ǜ="";switch(Ā.Х){case τ.ΔΞ:{ǜ="Waiting for command";џ=false;break;}case τ.ΖЌ:{ǜ="Waiting for cargo";џ=Ǆ(true);break;}case τ.ΖЋ:{ǜ="Unloading";џ=ΖЀ();break;}case τ.ΔΤ:{џ=true;break;}
case τ.ΔΝ:{ǜ="Charging batteries";џ=ȳ>=100f;break;}case τ.ΒΟ:{ǜ="Discharging batteries";џ=ȳ<=25f;break;}case τ.ΒΞ:{ǜ="Discharging batteries";џ=ȳ<=0f;break;}case τ.ΔΠ:{ǜ="Filling up hydrogen";џ=ģ>=100f;break;}case τ.ΔΡ:{ǜ="Unloading hydrogen";џ=ģ<=25f;break;}
case τ.ΔΟ:{ǜ="Unloading hydrogen";џ=ģ<=0f;break;}case τ.ΔΣ:{bool ΘΘ=ΙЋ();if(!ΘΘ) Ā.Εφ=true;џ=Ā.Εφ&&ΘΘ;ǜ="Waiting for passengers";break;}}if(!џ) Ā.Γς(true);if(џ&&Ā.ΓЈ()){џ=Ā.Γς(false);ǜ="Undocking in: "+ΒΑ((int) Math.Max(0,Ā.ǽ-Ā.Ł));}if(ΓΥ) ΙΗ=ǜ;return џ;}void ΙΜ()
{if(!ƛ) return;if(ύ.Count<=1) return;float ΘА=0,ΘЏ=0;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock Ȩ=ύ[i];if(ǃ(Ȩ,true)) continue;ΘА+=(float) Ȩ.GetInventory(0).MaxVolume;ΘЏ+=(float) Ȩ.GetInventory(0).CurrentVolume;}float Ηά=(float) Math.Round(М(ΘЏ,ΘА),5);for(int ċ=0;ċ<Math.Max(1,Math.Floor(ύ.Count/10f));ċ++)
{float ΕΣ=0,Εγ=0;float ΘΑ=0,ΘΫ=0;IMyTerminalBlock Ġ=null,ł=null;for(int i=0;i<ύ.Count;i++){IMyTerminalBlock Ȩ=ύ[i];if(ǃ(Ȩ,true)) continue;float Εκ=(float) Ȩ.GetInventory(0).MaxVolume;float ΒЀ=М((float) Ȩ.GetInventory(0).CurrentVolume,Εκ);if(Ġ==null||ΒЀ>ΕΣ){Ġ=Ȩ;ΕΣ=ΒЀ;ΘΑ=Εκ;}
if(ł==null||ΒЀ<Εγ){ł=Ȩ;Εγ=ΒЀ;ΘΫ=Εκ;}}if(Ġ==null||ł==null||Ġ==ł) return;if(checkConveyorSystem&&!Ġ.GetInventory(0).IsConnectedTo(ł.GetInventory(0))){if(Δϋ>20) Μ="Inventory balancing failed";else Δϋ++;return;}Δϋ=0;List<MyInventoryItem>Б=new List<MyInventoryItem>();
Ġ.GetInventory(0).GetItems(Б);float ΑЀ=0;if(Б.Count==0) continue;MyInventoryItem ΒЁ=Б[0];String ΘΜ=ΒЁ.Type.TypeId+ΒЁ.Type.SubtypeId;if(!ΙΖ.TryGetValue(ΘΜ,out ΑЀ)){if(ΙБ(Ġ.GetInventory(0),0,ł.GetInventory(0),out ΑЀ))
ΙΖ.Add(ΘΜ,ΑЀ);else return;}float ΜΗ=((ΕΣ-Ηά)*ΘΑ/ΑЀ);float ΜΘ=((Ηά-Εγ)*ΘΫ/ΑЀ);int ΖΘ=(int) Math.Min(ΜΘ,ΜΗ);if(ΖΘ<=0) return;if((float) ΒЁ.Amount<ΖΘ)
Ġ.GetInventory(0).TransferItemTo(ł.GetInventory(0),0,null,null,ΒЁ.Amount);else
Ġ.GetInventory(0).TransferItemTo(ł.GetInventory(0),0,null,null,ΖΘ);}}bool ΙБ(IMyInventory Ατ,int Β,IMyInventory ΛΒ,out float ΙΕ){ΙΕ=0;float ΚЅ=(float) Ατ.CurrentVolume;List<MyInventoryItem>Δύ=new List<MyInventoryItem>();Ατ.GetItems(Δύ);float Ηΰ=0;for(int i=0;i<Δύ.Count;i++) Ηΰ+=(float) Δύ[i].Amount;
Ατ.TransferItemTo(ΛΒ,Β,null,null,1);float ΙΓ=ΚЅ-(float) Ατ.CurrentVolume;List<MyInventoryItem>Δς=new List<MyInventoryItem>();Ατ.GetItems(Δς);float ΗΥ=0;for(int i=0;i<Δς.Count;i++) ΗΥ+=(float) Δς[i].Amount;if(ΙΓ==0f||!ΓЍ(0.9999,Ηΰ-ΗΥ,1.0001))
return false;ΙΕ=ΙΓ;return true;}void Κξ(){Vector3 ΑΪ=ώ+į;Vector3 ȧ=ΑΪ-β;if(ȧ.Length()==0) ȧ=new Vector3(0,0,-1);Vector3 ΒБ=ъ(Ω,ȧ);Vector3 ΘЊ=Vector3.Normalize(ΒБ);Vector3 ƞ=ъ(Ω,Ω.GetNaturalGravity());float ΛЇ=Ετ>0?Math.Max(0,1-Ž(ȧ,ΗЇ)/5):0;float Т=(float) Math.Min((Εη>0?Εη:1000f),Math.Max(ΑЈ(-ΒБ,ƞ,null),Ετ*ΛЇ));
if(!ΘЎ) Т=0;if(Θϊ) Т=Math.Max(0,1-Βρ/Βα)*Т;if(generalSpeedLimit>0) Т=Math.Min(generalSpeedLimit,Т);if(Ƨ) Т=0;if(ǝ>=0) Т=Math.Min(Т,ǝ);if(ΔΚ()) Т=Math.Min(Т,jobAreaSpeed);if(Θϋ) Т*=(float) Math.Min(1,М(ȧ.Length(),wpReachedDist)/2);Vector3 ΘΩ=ъ(Ω,Ω.GetShipVelocities().LinearVelocity);
float ΚЀ=(float)(Math.Max(0,15-Ž(-ΘЊ,-ΘΩ))/15)*0.85f+0.15f;ΕЏ+=Math.Sign(ΚЀ-ΕЏ)/10f;Vector3 ľ=ΘЊ*Т*ΕЏ-(ΘΩ);Vector3 Δω=ΓЅ(ľ,null);if(Γή&&и>0.1f){ľ.X*=Δα(ľ.X,ref Γί.X,1f,Δω.X,20);ľ.Y*=Δα(ľ.Y,ref Γί.Y,1f,Δω.Y,20);ľ.Z*=Δα(ľ.Z,ref Γί.Z,1f,Δω.Z,20);}else Γί=new Vector3(1,1,1);
Αϊ=Ŵ*ľ-ƞ*Ŵ;ΖΨ(Αϊ,Γή);и=Vector3.Distance(β,ΑΪ);}float Δα(float value,ref float Ј,float Ǯ,float ĩ,float Εθ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ј)) Ј+=Math.Sign(Ј)*Ǯ;else Ј=value;if(value==0) Ј=1;float α=Math.Abs(Ј);if(α<Εθ||ĩ==0) return α;
Ј=Math.Min(Εθ,Math.Max(-Εθ,Ј));α=Math.Abs(ĩ);return α;}void ΗΣ(){float ǚ=90,ǳ=90,Ǭ=90;float Ȉ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȟ,Ǩ,Αη,Εΰ;if(Εα){Ȟ=Vector3.Normalize(ώ-β);Αη=ъ(Ω,Ȟ);Ǩ=ъ(Ω,ΕЋ);ǚ=Ž(Αη,new Vector3(0,-1,0))-90;
ǳ=ŏ(Ǩ,new Vector3(-1,0,0),Ǩ.Y);Ǭ=ŏ(Αη,new Vector3(-1,0,0),Αη.Z);}else{Ȟ=ΕЌ;Ǩ=ъ(Ω,ΕЋ);Αη=ъ(Ω,ΕЌ);Εΰ=ъ(Ω,ΘЋ);ǚ=ŏ(Ǩ,new Vector3(0,0,1),Ǩ.Y);ǳ=ŏ(Ǩ,new Vector3(-1,0,0),Ǩ.Y);Ǭ=ŏ(Εΰ,new Vector3(0,0,1),Εΰ.X);}if(ΗΧ&&ΒΗ()){Vector3 ΕΚ=Ω.GetNaturalGravity();Ǩ=ъ(Ω,ΕΚ);
ǚ=ŏ(Ǩ,new Vector3(0,0,1),Ǩ.Y);ǳ=ŏ(Ǩ,new Vector3(-1,0,0),Ǩ.Y);}if(!ΓЍ(-45,ǳ,45)){ǚ=0;Ǭ=0;}if(!ΓЍ(-45,Ǭ,45)) ǚ=0;ΒΧ(ΕΜ,1,(-ǚ)*Ȉ,(-Ǭ)*Ȉ,(-ǳ)*Ȉ);Βρ=Math.Max(Math.Abs(ǚ),Math.Max(Math.Abs(ǳ),Math.Abs(Ǭ)));ΒЈ=Βρ<=Βα;}void ΙΦ(IMyTerminalBlock ΓΞ){if(ΓΞ==null) return;
ȍ=new float[3,2];ǯ=new Dictionary<string,float[,]>();for(int i=0;i<Đ.Count;i++){IMyThrust ѐ=Đ[i];if(!ѐ.IsFunctional) continue;Vector3 π=ъ(ΓΞ,ѐ.WorldMatrix.Backward);float ŀ=ѐ.MaxEffectiveThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) ȍ[0,0]+=ŀ;else ȍ[0,1]-=ŀ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) ȍ[1,0]+=ŀ;else ȍ[1,1]-=ŀ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) ȍ[2,0]+=ŀ;else ȍ[2,1]-=ŀ;String Ǟ=Ζχ(ѐ);float[,]Ǳ=null;if(ǯ.ContainsKey(Ǟ)) Ǳ=ǯ[Ǟ];else{Ǳ=new float[3,2];ǯ.Add(Ǟ,Ǳ);}float ĩ=ѐ.MaxThrust;if(Math.Round(π.X,2)!=0.0)
if(π.X>=0) Ǳ[0,0]+=ĩ;else Ǳ[0,1]-=ĩ;if(Math.Round(π.Y,2)!=0.0)
if(π.Y>=0) Ǳ[1,0]+=ĩ;else Ǳ[1,1]-=ĩ;if(Math.Round(π.Z,2)!=0.0)
if(π.Z>=0) Ǳ[2,0]+=ĩ;else Ǳ[2,1]-=ĩ;}}static String Ζχ(IMyThrust ѐ){return ѐ.BlockDefinition.SubtypeId;}Vector3 ΔΓ(Vector3 Ǥ,float[,]Ơ){return new Vector3(
Ǥ.X>=0?Ơ[0,0]:Ơ[0,1],
Ǥ.Y>=0?Ơ[1,0]:Ơ[1,1],
Ǥ.Z>=0?Ơ[2,0]:Ơ[2,1]);}bool Ζφ(έ ή,String Ǟ,out float ΕΔ){ΕΔ=0;int Β=ď.IndexOf(Ǟ);if(Β==-1||ή.ȋ==null||Β>=ή.ȋ.Length)
return false;ΕΔ=ή.ȋ[Β];if(ΕΔ==-1) return false;return true;}Vector3 ΓЅ(Vector3 Ǥ,έ ή){if(ή!=null){Vector3 α=new Vector3();for(int i=0;i<ǯ.Keys.Count;i++){String Ǟ=ǯ.Keys.ElementAt(i);float ΑΦ=0;if(!Ζφ(ή,Ǟ,out ΑΦ))
return ΔΓ(Ǥ,ȍ);α+=ΔΓ(Ǥ,ǯ.Values.ElementAt(i))*ΑΦ;}return α;}return ΔΓ(Ǥ,ȍ);}float ǁ(Vector3 Ǥ,έ ή){return ǁ(Ǥ,new Vector3(),ή);}float ǁ(Vector3 Ǥ,Vector3 ϋ,έ ή){Vector3 ΛА=ΓЅ(Ǥ,ή);Vector3 ŀ=ΛА+ϋ*Ŵ;float ű=(ŀ/Ǥ).AbsMin();return(float)(Ǥ*ű).Length();}float ΑЈ(Vector3 ΒЏ,Vector3 ƞ,έ ή)
{if(ΒЏ.Length()==0) return 0;float ΗΦ=1;if(ƞ.Length()>0) ΗΦ=Math.Min(1,Ž(-ƞ,ΒЏ)/90)*0.4f+0.6f;float Αϊ=ǁ(ΒЏ,ƞ,ή);if(Αϊ==0) return 0.1f;float Βΰ=М(Αϊ,Ŵ);float ΕБ=(float) Math.Sqrt(М(ΒЏ.Length(),Βΰ*0.5f));return Βΰ*ΕБ*ΗΦ*Ȕ;}float ΒΔ(Vector3 Ȟ,Vector3 Љ,Vector3 ϋ,έ ή)
{if(ϋ.Length()==0f) return 0;Vector3 ΒА=Ř(Ȟ,Љ,Vector3.Normalize(ϋ));float ǿ=ǁ(-ΒА,ή);return ǿ/ϋ.Length();}void Ζέ(bool ΛΨ){float ǲ=0;float ΕΗ=0.9f;if(ΛΨ){л=-1;Ÿ=0;ΓΩ=null;if(ζ!=Σ.ы&&Ζ.ϋ.Length()!=0){ǲ=ΕΗ*ΒΔ(Ζ.μ,Ζ.λ*-1,Ζ.ϋ,null);if(ǲ<л||л==-1) л=ǲ;}if(Ρ.ν&&Ρ.ϋ.Length()!=0)
{ǲ=ΕΗ*ΒΔ(Ρ.μ,Ρ.λ*-1,Ρ.ϋ,null);if(ǲ<л||л==-1) л=ǲ;}return;}if(Ÿ==-1) return;if(Ÿ>=0){int Θπ=0;while(Ÿ<Ψ.Count){if(Θπ>100) return;Θπ++;έ ή=Ψ[Ÿ];if(ή.ϋ.Length()!=0f){ǲ=ΕΗ*Math.Min(
ΒΔ(ή.μ,ή.λ*-1,ή.ϋ,ή),
ΒΔ(ή.μ*-1,ή.λ*-1,ή.ϋ,ή));if(ǲ<л||л==-1) л=ǲ;}else ΓΩ=ή;Ÿ++;}Ÿ=-1;}bool ΕΟ=true;float Ň=0;if(ΓΩ!=null){for(int i=0;i<ǯ.Count;i++){String Ǟ=ǯ.Keys.ElementAt(i);float[,]Ǳ=ǯ.Values.ElementAt(i);float ΑΦ=0;if(!Ζφ(ΓΩ,Ǟ,out ΑΦ)){ΕΟ=false;break;}for(int a=0;a<Ǳ.GetLength(0);a++)
for(int b=0;b<Ǳ.GetLength(1);b++){float ś=Math.Abs(Ǳ[a,b]*ΑΦ);if(ś==0) continue;ΕΟ=true;if(Ň==0||ś<Ň) Ň=ś;}}}if(!ΕΟ){for(int a=0;a<ȍ.GetLength(0);a++)
for(int b=0;b<ȍ.GetLength(1);b++){float ś=Math.Abs(ȍ[a,b]);if(ś==0) continue;if(Ň==0||ś<Ň) Ň=ś;}}if(Ň>0){float Λδ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǲ=М(Ň,Λδ);if(ǲ>0)
if(ǲ<л||л==-1) л=ǲ;}}void ΖΨ(Vector3 ǿ,bool Р){if(!Р){for(int i=0;i<Đ.Count;i++)
Đ[i].SetValueFloat("Override",0.0f);return;}Vector3 Δψ=ΓЅ(ǿ,null);float ΜΩ=Math.Min(1,Math.Abs(М(ǿ.X,Δψ.X)));float ΜΪ=Math.Min(1,Math.Abs(М(ǿ.Y,Δψ.Y)));float ΜΫ=Math.Min(1,Math.Abs(М(ǿ.Z,Δψ.Z)));for(int i=0;i<Đ.Count;i++){IMyThrust ѐ=Đ[i];Vector3 π=ΚΞ(ъ(Ω,ѐ.WorldMatrix.Backward),1);
if(π.X!=0&&Math.Sign(π.X)==Math.Sign(ǿ.X))
ѐ.SetValueFloat("Override",ѐ.MaxThrust*ΜΩ);else if(π.Y!=0&&Math.Sign(π.Y)==Math.Sign(ǿ.Y))
ѐ.SetValueFloat("Override",ѐ.MaxThrust*ΜΪ);else if(π.Z!=0&&Math.Sign(π.Z)==Math.Sign(ǿ.Z))
ѐ.SetValueFloat("Override",ѐ.MaxThrust*ΜΫ);else
ѐ.SetValueFloat("Override",0.0f);}}void ΒΧ(bool Р,float Λσ,float ǚ,float Ǭ,float ǳ){for(int i=0;i<ΒЃ.Count;i++){IMyGyro Н=ΒЃ[i];Н.GyroOverride=Р;if(!Р) Н.GyroPower=100;else Н.GyroPower=Λσ;if(!Р) continue;Vector3 б=Ω.WorldMatrix.Forward;Vector3 ч=Ω.WorldMatrix.Right;Vector3 Љ=Ω.WorldMatrix.Up;
Vector3 Ɩ=Н.WorldMatrix.Forward;Vector3 Ƙ=Н.WorldMatrix.Up;Vector3 Ɨ=Н.WorldMatrix.Left*-1;if(Ɩ==б) Н.SetValueFloat("Roll",ǳ);else if(Ɩ==(б*-1)) Н.SetValueFloat("Roll",ǳ*-1);else if(Ƙ==(б*-1)) Н.SetValueFloat("Yaw",ǳ);else if(Ƙ==б) Н.SetValueFloat("Yaw",ǳ*-1);else if(Ɨ==б) Н.SetValueFloat("Pitch",ǳ);
else if(Ɨ==(б*-1)) Н.SetValueFloat("Pitch",ǳ*-1);if(Ɨ==(ч*-1)) Н.SetValueFloat("Pitch",ǚ);else if(Ɨ==ч) Н.SetValueFloat("Pitch",ǚ*-1);else if(Ƙ==ч) Н.SetValueFloat("Yaw",ǚ);else if(Ƙ==(ч*-1)) Н.SetValueFloat("Yaw",ǚ*-1);else if(Ɩ==(ч*-1)) Н.SetValueFloat("Roll",ǚ);
else if(Ɩ==ч) Н.SetValueFloat("Roll",ǚ*-1);if(Ƙ==(Љ*-1)) Н.SetValueFloat("Yaw",Ǭ);else if(Ƙ==Љ) Н.SetValueFloat("Yaw",Ǭ*-1);else if(Ɨ==Љ) Н.SetValueFloat("Pitch",Ǭ);else if(Ɨ==(Љ*-1)) Н.SetValueFloat("Pitch",Ǭ*-1);else if(Ɩ==Љ) Н.SetValueFloat("Roll",Ǭ);else if(Ɩ==(Љ*-1)) Н.SetValueFloat("Roll",Ǭ*-1);
}}void ΗΛ(){this.ΕΜ=false;}void с(Vector3 Ț,Vector3 б,Vector3 Ũ,float Į,bool ΑϏ){Γκ(Ț,Į,ΑϏ);Βα=Į;Εα=false;this.ΕЌ=б;this.ΘЋ=Ũ;}void с(Vector3 Ț,Vector3 б,Vector3 Ũ,bool ΑϏ){с(Ț,б,Ũ,2f,ΑϏ);}void Γκ(Vector3 Ț,float Į,bool ΑϏ){Βα=Į;this.ΕΜ=true;this.ΗΧ=ΑϏ;Εα=true;ΒЈ=false;
this.ΕЋ=Ț;}void Δη(){Ȓ(false,false,false,ώ,0);Γή=false;}void е(Vector3 ε,float Ȉ){Ȓ(true,false,false,ε,Ȉ);}void Ȓ(bool Р,bool ΕІ,bool ΖΜ,Vector3 ε,float Ȉ){Ȓ(Р,ΕІ,ΖΜ,ε,ε-β,0.0f,Ȉ);}void Ȓ(bool Р,bool ΕІ,bool ΖΜ,Vector3 ε,Vector3 ΛЁ,float ΛЂ,float Ȉ){Γή=true;this.ΘЎ=Р;
ώ=ε;this.Εη=Ȉ;this.Ετ=ΛЂ;this.Θϋ=ΕІ;this.Θϊ=ΖΜ;this.ΗЇ=ΛЁ;и=Vector3.Distance(ε,β);}void ΚΡ(){χ.GetBlocksOfType(ΑΕ,Ǡ);}void ΚΣ(){if(ΓΠ){Е=2;return;}List<IMyRemoteControl>Αο=new List<IMyRemoteControl>();List<IMySensorBlock>Θχ=new List<IMySensorBlock>();List<IMyTerminalBlock>Αψ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αο,Ǡ);χ.GetBlocksOfType(Э,Ǡ);χ.GetBlocksOfType(Θχ,Ǡ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αψ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Э=Ǵ(Э,pamTag,true);ΗΓ();ΗΔ(Αψ);ŗ(Э,setLCDFontAndSize,1.4f,false);ŗ(ȥ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘІ=Ǵ(Θχ,pamTag,true);if(ΘІ.Count>0) ό=ΘІ[0];else ό=null;if(Ξ==Ν.Ǉ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ύ.Count==0){Е=1;Μ="Drills are missing";}}else if(Ξ==Ν.Ƕ){χ.GetBlocksOfType(ύ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ύ.Count==0){Е=1;Μ="Grinders are missing";}if(ό==null){Е=1;Μ="Sensor is missing";}}else if(Ξ==Ν.ί){χ.GetBlocksOfType(ΖΑ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘЅ=Ǵ(Αο,pamTag,true);if(ΘЅ.Count>0) Αο=ΘЅ;if(Αο.Count>0) Ω=Αο[0];else{Ω=null;Е=2;Μ="Remote is missing";return;}θ=(float) Ω.CubeGrid.WorldVolume.Radius*2;Ǫ=null;if(Ξ!=Ν.ί){Ζή();if(ύ.Count>0&&Ǫ!=null){if(ό!=null&&(Ǫ.WorldMatrix.Forward!=ό.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ό.WorldMatrix.Up||Ω.WorldMatrix.Down==ό.WorldMatrix.Down))){Е=1;Μ="Wrong sensor direction";}if(Ǫ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
Ǫ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){Е=2;Μ="Wrong remote direction";}}}}void ΚΤ(){χ.GetBlocksOfType(Ĉ,Ǡ);χ.GetBlocksOfType(Ǒ,Ǡ);χ.GetBlocksOfType(Đ,Ǡ);χ.GetBlocksOfType(ΒЃ,Ǡ);χ.GetBlocksOfType(ļ,Ǡ);χ.GetBlocksOfType(ň,Ǡ);χ.GetBlocksOfType(Ǧ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΓΨ,Ǡ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
Ǒ=Ǵ(Ǒ,"ConnectorMedium",false);else
Ǒ=Ǵ(Ǒ,"Connector",false);List<IMyShipConnector>ΘЃ=Ǵ(Ǒ,pamTag,true);if(ΘЃ.Count>0) Ǒ=ΘЃ;if(Е<=1){if(Ǒ.Count==0){Е=1;Μ="Connector is missing";}if(ΒЃ.Count==0){Е=1;Μ="Gyros are missing";}if(Đ.Count==0){Е=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>ΘЇ=Ǵ(ΓΨ,pamTag,true);
if(ΘЇ.Count>0) ΓΨ=ΘЇ;List<IMyLandingGear>ΘЄ=Ǵ(Ĉ,pamTag,true);if(ΘЄ.Count>0) Ĉ=ΘЄ;for(int i=0;i<Ĉ.Count;i++) Ĉ[i].AutoLock=false;List<IMyBatteryBlock>ΘЂ=Ǵ(ļ,pamTag,true);if(ΘЂ.Count>0) ļ=ΘЂ;List<IMyGasTank>ΘЈ=Ǵ(Ǧ,pamTag,true);if(ΘЈ.Count>0) Ǧ=ΘЈ;}void ΚΥ(){χ.GetBlocksOfType(Ŝ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ƒ.Clear();for(int i=Ŝ.Count-1;i>=0;i--){if(ΙЉ(Ŝ[i])){Ƒ.Add(Ŝ[i]);Ŝ.RemoveAt(i);}}}bool ΙЉ(IMyTerminalBlock Ϊ){if(Ϊ is IMyCargoContainer) return true;if(Ϊ is IMyShipDrill) return true;if(Ϊ is IMyShipGrinder) return true;if(Ϊ is IMyShipConnector){if(((IMyShipConnector) Ϊ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓЉ(Ϊ,"ConnectorSmall",false))
return false;else return true;}return false;}void Ζή(){if(Ω==null) return;Ǫ=null;float ΓΖ=0,ΓΑ=0,ΓΗ=0,ΓΒ=0,ΓΘ=0,ΓΓ=0;List<IMyTerminalBlock>ΕЊ=Ǵ(ύ,pamTag,true);bool Λλ=ΕЊ.Count==0;if(ΕЊ.Count>0) Ǫ=ΕЊ[0];else if(ύ.Count>0) Ǫ=ύ[0];int Ă=0;float č=0;for(int i=0;i<ύ.Count;i++)
{if(ύ[i].WorldMatrix.Forward!=Ǫ.WorldMatrix.Forward){if(Λλ){Е=2;Μ="Mining direction is unclear!";return;}continue;}Ă++;č=Math.Max(č,Κζ(ύ[i]));Vector3 С=ΒΓ(Ω,ύ[i].GetPosition());if(i==0){ΓΖ=С.X;ΓΑ=С.X;ΓΗ=С.Y;ΓΒ=С.Y;ΓΘ=С.Z;ΓΓ=С.Z;}ΓΑ=Math.Max(С.X,ΓΑ);ΓΖ=Math.Min(С.X,ΓΖ);
ΓΒ=Math.Max(С.Y,ΓΒ);ΓΗ=Math.Min(С.Y,ΓΗ);ΓΓ=Math.Max(С.Z,ΓΓ);ΓΘ=Math.Min(С.Z,ΓΘ);}if(č==0) č=drillRadius;bool Ț=Ǫ!=null&&Ǫ.WorldMatrix.Forward==Ω.WorldMatrix.Down;ǰ=(ΓΑ-ΓΖ)*(1-Ź)+č*2;Ȍ=(Ț?ΓΓ-ΓΘ:ΓΒ-ΓΗ)*(1-ţ)+č*2;IMyCubeGrid g=Ω.CubeGrid;float ΖΠ=0,ΖΡ=0,ΖΣ=0,ΖΤ=0;
for(int c=0;c<8;c++){Vector3 p=ΒΓ(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ț?p.Z:p.Y;if(c==0||p.X<ΖΠ) ΖΠ=p.X;if(c==0||p.X>ΖΡ) ΖΡ=p.X;if(c==0||y<ΖΣ) ΖΣ=y;if(c==0||y>ΖΤ) ΖΤ=y;}ΓΣ=ΖΡ-ΖΠ+g.GridSize;ΕЁ=ΖΤ-ΖΣ+g.GridSize;}float Κζ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǵ<T>(List<T>В,String ȉ,bool ΒЄ){List<T>α=new List<T>();for(int i=0;i<В.Count;i++)
if(ΓЉ(В[i],ȉ,ΒЄ)) α.Add(В[i]);return α;}bool ΓЉ<T>(T Ϊ,String ȉ,bool ΒЄ){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(ΒЄ&&b.CustomName.ToUpper().Contains(ȉ.ToUpper())) return true;if(!ΒЄ&&b.BlockDefinition.ToString().ToUpper().Contains(ȉ.ToUpper())) return true;return false;}void Ǣ<T>(List<T>В,bool Р)
{for(int i=0;i<В.Count;i++)
ΗΑ((IMyTerminalBlock) В[i],Р);}void ΗΑ(IMyTerminalBlock Ϊ,bool Р){if(Ϊ==null) return;String Κώ=Р?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(Κώ);a.Apply(Ϊ);}void ȓ(bool Р){ΕЇ=Р;if(!ŵ) return;Ǣ(ΓΨ,Р);}void Δέ(ChargeMode Д){for(int i=0;i<ļ.Count;i++) ļ[i].ChargeMode=Д;}void Δί(bool ΛЉ)
{for(int i=0;i<Ǧ.Count;i++) Ǧ[i].Stockpile=ΛЉ;}void Ǌ(List<IMyLandingGear>ΕΘ,bool ΘΪ){for(int i=0;i<ΕΘ.Count;i++){if(ΘΪ) ΕΘ[i].Lock();if(!ΘΪ) ΕΘ[i].Unlock();}}void ΗΒ(bool Р){if(Ηφ==Р) return;List<IMyShipController>Βν=new List<IMyShipController>();χ.GetBlocksOfType(Βν,Ǡ);if(Βν.Count==0) return;
for(int i=0;i<Βν.Count;i++)
Βν[i].DampenersOverride=Р;Ηφ=Р;}IMyShipConnector о(MyShipConnectorStatus ǜ){for(int i=0;i<Ǒ.Count;i++){if(!ȭ(Ǒ[i],true)) continue;if(Ǒ[i].Status==ǜ) return Ǒ[i];}return null;}void ř(bool ΕЂ,bool Βχ,float ΛΞ,float ΚЄ){if(ό==null||ύ.Count==0) return;Vector3 ƒ=new Vector3();int Ă=0;
for(int i=0;i<ύ.Count;i++){if(ύ[i].WorldMatrix.Forward!=Ǫ.WorldMatrix.Forward) continue;Ă++;ƒ+=ύ[i].GetPosition();}ƒ=ƒ/Ă;Vector3 Ƥ=ΒΓ(ό,ƒ);ό.Enabled=true;ό.ShowOnHUD=ΕЂ;ό.LeftExtend=(Βχ?1:ј)/2f*ǰ-Ƥ.X;ό.RightExtend=(Βχ?1:ј)/2f*ǰ+Ƥ.X;ό.TopExtend=(Βχ?1:ї)/2f*Ȍ+Ƥ.Y;
ό.BottomExtend=(Βχ?1:ї)/2f*Ȍ-Ƥ.Y;ό.FrontExtend=(ΕЂ?ф:ΛΞ)-Ƥ.Z;ό.BackExtend=ΕЂ?0:ΚЄ+θ*0.75f+Ƥ.Z;ό.DetectFloatingObjects=true;ό.DetectAsteroids=false;ό.DetectLargeShips=true;ό.DetectSmallShips=true;ό.DetectStations=true;ό.DetectOwner=true;ό.DetectSubgrids=false;
ό.DetectPlayers=false;ό.DetectEnemy=true;ό.DetectFriendly=true;ό.DetectNeutral=true;}void ŗ<T>(List<T>В,bool ΛЄ,float ΛΜ,bool ΛΩ){for(int i=0;i<В.Count;i++){IMyTextSurface Αχ=null;if(В[i]is IMyTextSurface) Αχ=(IMyTextSurface) В[i];if(Αχ!=null){Αχ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΛЄ) continue;Αχ.Font="Debug";if(ΛΩ) continue;Αχ.FontSize=ΛΜ;}}}bool ΒΗ(){Vector3D ά;return this.Ω.TryGetPlanetPosition(out ά);}int Ιψ(Vector3 ΛЋ,int Θό){if(Θό==-1) return 0;double Щ=-1;int Ĝ=-1;for(int i=Ψ.Count-1;i>=0;i--){double υ=Vector3.Distance(Ψ[i].Ε,ΛЋ);if(Щ==-1||υ<Щ){Ĝ=i;Щ=υ;}
}return Math.Sign(Ĝ-Θό);}bool ΙЊ(Vector3 ά){List<Vector3>г=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) г.Add(Ψ[i].Ε);if(Ρ.ν&&Ψ.Count>=1){Vector3 ƌ=new Vector3();Ȭ(Ρ,dockDist*θ,false,out ƌ);if(Vector3.Distance(Ρ.Ε,Ψ.First().Ε)<Vector3.Distance(Ρ.Ε,Ψ.Last().Ε)){г.Insert(0,ƌ);г.Insert(0,Ρ.Ε);}
else{г.Add(ƌ);г.Add(Ρ.Ε);}}if(Ξ==Ν.ί){if(Ζ.ν&&Ψ.Count>=1){Vector3 Δτ=new Vector3();Ȭ(Ζ,dockDist*θ,false,out Δτ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){г.Insert(0,Δτ);г.Insert(0,Ζ.Ε);}else{г.Add(Δτ);г.Add(Ζ.Ε);}}}else{if(ζ!=Σ.ы)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
г.Insert(0,Ζ.Ε);else г.Add(Ζ.Ε);}int Ĝ=-1;double ȗ=-1;for(int i=0;i<г.Count;i++){double υ=Vector3.Distance(г[i],ά);if(υ<ȗ||ȗ==-1){ȗ=υ;Ĝ=i;}}if(г.Count==0) return false;double ΔЏ=Vector3.Distance(г[Ĝ],ά);double Λτ=Vector3.Distance(г[Math.Max(0,Ĝ-1)],г[Ĝ])*1.5f;
double Λκ=Vector3.Distance(г[Math.Min(г.Count-1,Ĝ+1)],г[Ĝ])*1.5f;return ΔЏ<Λτ||ΔЏ<Λκ||ΔЏ<laneOffset*θ*1.5f;}static float М(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΚΞ(Vector3 v,int ΔЌ){return new Vector3(Math.Round(v.X,ΔЌ),Math.Round(v.Y,ΔЌ),Math.Round(v.Z,ΔЌ));
}float Ž(Vector3 a,Vector3 b){if(a==b) return 0;float ΛΘ=(a*b).Sum;float ΘΤ=a.Length();float ΘΥ=b.Length();if(ΘΤ==0||ΘΥ==0) return 0;float α=(float)((180.0f/Math.PI)*Math.Acos(ΛΘ/(ΘΤ*ΘΥ)));if(float.IsNaN(α)) return 0;return α;}float ŏ(Vector3 v,Vector3 ΓΞ,float ΛЅ){float Ɗ=Ž(v,ΓΞ);
if(ΛЅ>0f) Ɗ*=-1;if(Ɗ>-90f) return Ɗ-90f;else return 180f-(-Ɗ-90f);}double Ζζ(float ΚБ){return(Math.PI/180)*ΚБ;}bool ΓЍ(double Λγ,double value,double ơ){return(value>=Λγ&&value<=ơ);}Vector3 Κη(IMyTerminalBlock Ϊ,Vector3 С){return Vector3D.Transform(С,Ϊ.WorldMatrix);
}Vector3 ΒΓ(IMyTerminalBlock Ϊ,Vector3 ΜΦ){return ъ(Ϊ,ΜΦ-Ϊ.GetPosition());}Vector3 ъ(IMyTerminalBlock Ϊ,Vector3 ΖΟ){return Vector3D.TransformNormal(ΖΟ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 Ř(Vector3 Ȟ,Vector3 Љ,Vector3 ΖΟ){MatrixD Εζ=MatrixD.CreateFromDir(Ȟ,Љ);return Vector3D.TransformNormal(ΖΟ,MatrixD.Transpose(Εζ));
}Vector3 Δι(Vector3 Ȟ,Vector3 Љ,Vector3 С){MatrixD Εζ=MatrixD.CreateFromDir(Ȟ,Љ);return Vector3D.Transform(С,Εζ);}String ο(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 κ(String s){String[]ψ=s.Split('|');return new Vector3(float.Parse(Τ(ψ,0)),float.Parse(Τ(ψ,1)),float.Parse(Τ(ψ,2)));
}String Κό(έ ή){String Ф=":";String α=ο(ή.Ε)+Ф+ο(ή.μ)+Ф+
ο(ή.λ)+Ф+ο(ή.Ѓ)+Ф+ο(ή.ϋ);for(int i=0;i<ή.ȋ.Length;i++)
α+=Ф+Math.Round(ή.ȋ[i],3);return α;}έ Κΰ(String s){String[]ψ=s.Split(':');έ ή=new έ(
κ(Τ(ψ,0)),κ(Τ(ψ,1)),
κ(Τ(ψ,2)),κ(Τ(ψ,3)),
κ(Τ(ψ,4)));int Β=5;List<float>ΗЄ=new List<float>();while(Β<ψ.Length){String Γδ=Τ(ψ,Β);float f=0;if(!float.TryParse(Γδ,out f)) break;ΗЄ.Add(f);Β++;}ή.ȋ=ΗЄ.ToArray();return ή;}System.Text.StringBuilder ƫ=new System.Text.StringBuilder();void Χ<T>(T value,bool Λι){if(Λι) ƫ.Append('\n');ƫ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΑЍ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Βέ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Τ(String[]ΚЂ,int Β){String Γδ=ΚЂ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Γδ)) return "";
return Γδ;}void Save(){if(ΓΠ||Ξ==Ν.Ǐ){Storage="";return;}Storage=Γπ();}String Γπ(){ƫ.Clear();ƫ.Append(DATAREV+";");Χ(ο(ΓΟ),false);Χ(ο(Ρ.μ));Χ(ο(Ρ.Ѓ));Χ(ο(Ρ.λ));Χ(ο(Ρ.ϋ));Χ(ο(Ρ.Ε));Χ(ο(Ρ.х));Χ(Ρ.ν);Χ(ο(Ζ.Ε));Χ(ο(Ζ.ϋ));Χ(ο(Ζ.μ));Χ(ο(Ζ.λ));Χ(ο(Ζ.Ѓ));Χ(ο(Ζ.х));Χ(Ζ.ν);Χ(ο(м));Χ(ο(ĉ));Χ(";");
Χ((int) Ξ,false);Χ((int) ζ);Χ((int) Ǘ);Χ(Ƞ);Χ(ǘ);Χ(ц);Χ(Ǚ);Χ(Ȕ);Χ(ı);Χ(İ);if(Ξ==Ν.ί){Χ((int) Ѝ.Х);Χ(Ѝ.ǽ);Χ(Ѝ.Ł);Χ(ΑЍ(Ѝ.Ǿ));Χ(ΑЍ(Ѝ.ȃ));Χ((int) Ў.Х);Χ(Ў.ǽ);Χ(Ў.Ł);Χ(ΑЍ(Ў.Ǿ));Χ(ΑЍ(Ў.ȃ));}else{Χ((int) Ȥ);Χ((int) ќ);Χ((int) к);Χ((int) ǒ);Χ((int) Ŧ);Χ(ŋ);Χ(ŵ);Χ(ƛ);Χ(ƕ);Χ(ј);Χ(ї);Χ(ф);Χ(Ƽ);Χ(ƻ);
Χ(Ź);Χ(ţ);Χ(Ћ);Χ(Ь);Χ(Є);Χ(Ё);Χ(Ĩ);Χ(з);Χ(Ų);Χ(Ū);}Χ(";");for(int i=0;i<ď.Count;i++)
Χ((i>0?"|":"")+ď[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(Κό(Ψ[i]),i>0);Χ(";",false);Χ(й+"|"+Й,false);Χ(";",false);Χ(Ιθ(),false);String Η=ƫ.ToString();ƫ.Clear();return Η;}ǐ ΚΝ(){return ΒΡ(Storage);}ǐ ΒΡ(String Η){if(Η=="") return ǐ.ΚΒ;String[]ų=Η.Split(';');if(Τ(ų,0)!=DATAREV) return ǐ.ΔΛ;int Β=0;try{String[]Π=Τ(ų,1).Split('\n');
ΓΟ=κ(Τ(Π,Β++));Ρ.μ=κ(Τ(Π,Β++));Ρ.Ѓ=κ(Τ(Π,Β++));Ρ.λ=κ(Τ(Π,Β++));Ρ.ϋ=κ(Τ(Π,Β++));Ρ.Ε=κ(Τ(Π,Β++));Ρ.х=κ(Τ(Π,Β++));Ρ.ν=bool.Parse(Τ(Π,Β++));Ζ.Ε=κ(Τ(Π,Β++));Ζ.ϋ=κ(Τ(Π,Β++));Ζ.μ=κ(Τ(Π,Β++));Ζ.λ=κ(Τ(Π,Β++));Ζ.Ѓ=κ(Τ(Π,Β++));Ζ.х=κ(Τ(Π,Β++));Ζ.ν=bool.Parse(Τ(Π,Β++));
м=κ(Τ(Π,Β++));ĉ=κ(Τ(Π,Β++));Π=Τ(ų,2).Split('\n');Β=0;Ξ=(Ν) int.Parse(Τ(Π,Β++));ζ=(Σ) int.Parse(Τ(Π,Β++));Ǘ=(Σ) int.Parse(Τ(Π,Β++));Ƞ=int.Parse(Τ(Π,Β++));ǘ=int.Parse(Τ(Π,Β++));ц=int.Parse(Τ(Π,Β++));Ǚ=int.Parse(Τ(Π,Β++));Ȕ=float.Parse(Τ(Π,Β++));ı=bool.Parse(Τ(Π,Β++));
İ=bool.Parse(Τ(Π,Β++));if(Ξ==Ν.ί){Ѝ.Х=(τ) int.Parse(Τ(Π,Β++));Ѝ.ǽ=float.Parse(Τ(Π,Β++));Ѝ.Ł=float.Parse(Τ(Π,Β++));Ѝ.Ǿ=Βέ(Τ(Π,Β++));Ѝ.ȃ=Βέ(Τ(Π,Β++));Ў.Х=(τ) int.Parse(Τ(Π,Β++));Ў.ǽ=float.Parse(Τ(Π,Β++));Ў.Ł=float.Parse(Τ(Π,Β++));Ў.Ǿ=Βέ(Τ(Π,Β++));Ў.ȃ=Βέ(Τ(Π,Β++));}else
{Ȥ=(ѕ) int.Parse(Τ(Π,Β++));ќ=(ђ) int.Parse(Τ(Π,Β++));к=(З) int.Parse(Τ(Π,Β++));ǒ=(ǟ) int.Parse(Τ(Π,Β++));Ŧ=(ѕ) int.Parse(Τ(Π,Β++));ŋ=bool.Parse(Τ(Π,Β++));ŵ=bool.Parse(Τ(Π,Β++));ƛ=bool.Parse(Τ(Π,Β++));ƕ=bool.Parse(Τ(Π,Β++));ј=int.Parse(Τ(Π,Β++));ї=int.Parse(Τ(Π,Β++));ф=int.Parse(Τ(Π,Β++));
Ƽ=float.Parse(Τ(Π,Β++));ƻ=float.Parse(Τ(Π,Β++));Ź=float.Parse(Τ(Π,Β++));ţ=float.Parse(Τ(Π,Β++));Ћ=int.Parse(Τ(Π,Β++));Ь=int.Parse(Τ(Π,Β++));Є=int.Parse(Τ(Π,Β++));Ё=int.Parse(Τ(Π,Β++));Ĩ=int.Parse(Τ(Π,Β++));з=int.Parse(Τ(Π,Β++));Ų=int.Parse(Τ(Π,Β++));Ū=float.Parse(Τ(Π,Β++));
}Π=Τ(ų,3).Replace("\n","").Split('|');ď=Π.ToList();Π=Τ(ų,4).Split('\n');Ψ.Clear();if(Π.Count()>=1&&Π[0]!="")
for(int i=0;i<Π.Length;i++)
Ψ.Add(Κΰ(Τ(Π,i)));Π=Τ(ų,5).Split('|');й=Τ(Π,0);if(!int.TryParse(Τ(Π,1),out Й)) Й=-1;if(й.Length!=Ћ*Ь){й="";Й=-1;}Ιη(Τ(ų,6));}catch{return ǐ.Ζι;}Ƭ=Ǘ;Ѕ();return ǐ.ΒΩ;}const int Ζά=64000;IMyTerminalBlock Ζκ(){List<IMyTerminalBlock>Ə=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ə,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ə.Count>0?Ə[0]:null;}void Ιμ(){IMyTerminalBlock Ϊ=Ζκ();if(Ϊ==null){Μ="Export: name a block "+dataTag;return;}String Η="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Γπ();if(Η.Length>Ζά){Μ="Export too large: "+Η.Length+" chars, max "+Ζά;return;}Ϊ.CustomData=Η;Μ="Exported "+Ψ.Count+" waypoints";}void ΙІ(bool ΕΩ){IMyTerminalBlock Ϊ=Ζκ();if(Ϊ==null){Μ="Import: name a block "+dataTag;return;}
ΙЇ(Ϊ.CustomData,ΕΩ);}void ΙЇ(String Η,bool ΕΩ){Η=Η.Replace("\r","");if(Η.StartsWith("//")){int Θη=Η.IndexOf('\n');Η=Θη<0?"":Η.Substring(Θη+1);}Η=Η.Trim();if(Η==""){Μ="Import: no PAM data in "+dataTag;return;}Ѕ();String Ηή=Γπ();ǐ α=ΒΡ(Η);if(α==ǐ.ΒΩ&&ΕΩ){έ Ι=new έ(Ζ);
Vector3 Λζ=м,Λε=ĉ;int w=ј,h=ї,d=ф;ѕ Αυ=Ȥ;ǟ ΔЍ=ǒ;float Λν=Ź,Λμ=ţ;ΒΡ(Ηή);Ζ=Ι;м=Λζ;ĉ=Λε;ј=w;ї=h;ф=d;Ȥ=Αυ;ǒ=ΔЍ;Ź=Λν;ţ=Λμ;ζ=Σ.ƅ;ΑΓ(true,true);ζ=Σ.ƅ;Ǘ=Σ.ƅ;Μ="Imported job, run CONT";return;}if(α==ǐ.ΒΩ){Αλ=false;Μ="Imported "+Ψ.Count+" waypoints";
return;}ΒΡ(Ηή);Μ=α==ǐ.ΔΛ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗΓ(){Š.Clear();for(int i=Э.Count-1;i>=0;i--){String Η=Э[i].CustomData.ToUpper();bool ΕΧ=false;if(Η=="INSTRUCTIONS"){ΕΧ=true;Βυ=true;}if(Η=="DEBUG") ΕΧ=true;if(ΕΧ){Š.Add(Э[i]);Э.RemoveAt(i);}
}ŗ(Š,false,1,false);}void ΗΔ(List<IMyTerminalBlock>Ə){ȥ.Clear();for(int i=0;i<Ə.Count;i++){IMyTerminalBlock Ϊ=Ə[i];try{String Θο=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϊ.CustomName.IndexOf(Θο);int Ʊ=-1;if(Β<0||!int.TryParse(Ϊ.CustomName.Substring(Β+Θο.Length,1),out Ʊ)) continue;
if(Ʊ==-1) continue;Ʊ--;IMyTextSurfaceProvider Θρ=(IMyTextSurfaceProvider) Ϊ;if(Ʊ<Θρ.SurfaceCount&&Ʊ>=0)
ȥ.Add(Θρ.GetSurface(Ʊ));}catch{}}}String ΓΔ="";void ΖЏ(){String Εε="";String ΑΥ="";ΓΔ=ΖΫ(false);Εε+=ΓΔ;ΑΥ+=ΓΔ;ΑΥ+=Ιυ();for(int i=0;i<Э.Count;i++) Э[i].WriteText(Εε);for(int i=0;i<ȥ.Count;i++) ȥ[i].WriteText(Εε);Echo(ΑΥ);for(int i=0;i<Š.Count;i++){IMyTextPanel я=Š[i];
String Η=я.CustomData.ToUpper();if(Η=="DEBUG") я.WriteText(""+"\n"+"");if(Η=="INSTRUCTIONS") я.WriteText(Ζρ());}}string Ζρ(){String Γ="";try{float Θά=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ǝ*Θά)+" / "+(int)(Ƣ*Θά)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ǝ*100f,1)+"% / "+Math.Round(Ƣ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ΑΘ,2)+"ms / "+Math.Round(Ει,2)+"ms \n";}catch{}for(int i=0;i<Ī.Count;i++)
Γ+=""+Ī.Keys.ElementAt(i)+" = "+Math.Round(Ī.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ī.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙЀ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ŗ(Э,setLCDFontAndSize,0.9f,false);ŗ(ȥ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Э.Count;i++) Э[i].WriteText(Γ+ħ.ToString());for(int i=0;i<ȥ.Count;i++) ȥ[i].WriteText(Γ+ħ.ToString());}void Ȱ(){if(!Βυ) return;try{Εχ=Runtime.CurrentInstructionCount;}catch{}
}void ȫ(String ΰ){if(!Βυ) return;if(Εχ==0) return;try{float Γβ=(Runtime.CurrentInstructionCount-Εχ)/Runtime.MaxInstructionCount*100;if(!Ī.ContainsKey(ΰ))
Ī.Add(ΰ,new float[]{Γβ,Γβ});else{Ī[ΰ][0]=Γβ;Ī[ΰ][1]=Math.Max(Γβ,Ī[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΙΝ(String ȝ){if(!Ɛ) return;String Ф=""+BROADCAST_SEP;String δ=VERSION+Ф;δ+=ƽ(Ư)+Ф;
δ+=(int) Ξ+Ф;δ+=ƀ(β.X)+""+Ф;δ+=ƀ(β.Y)+Ф;δ+=ƀ(β.Z)+Ф;if(Ξ!=Ν.ί)
δ+=ΓЂ(Ǘ)+(Ǘ==Σ.Л?" "+Math.Round(Αδ,1)+"%":"")+Ф;else δ+=ΓЂ(Ǘ)+Ф;δ+=ƽ(Μ)+Ф;δ+=Г+""+Ф;δ+=ƽ(ΓΔ)+Ф;δ+=ș+Ф;δ+=ȡ+Ф;for(int i=0;i<ȁ.Count;i++){if(ȁ[i].Ɵ!=Я.Ƃ) continue;δ+=ƽ(ȁ[i].ΰ)+"/"+ƽ(ȁ[i].ω)+"/"+ȁ[i].А+Ф;}ΚΩ(δ,ȝ);}String ƽ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΚΩ(String δ,String ȝ){ΒΥ(δ,BROADCAST_SELF_ID,ȝ);}void ΒΥ(String δ,String ε,String ȝ){try{if(!Ɛ) return;δ=BROADCAST_TAG+BROADCAST_SEP+Ζν()+BROADCAST_SEP+ε+BROADCAST_SEP+ΔЁ+BROADCAST_SEP+ȝ+BROADCAST_SEP+δ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,δ);}catch(Exception e){ħ=e;}
}String Ζν(){if(Ξ!=Ν.Ǐ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔΑ(String δ){return δ.StartsWith(BROADCAST_TAG);}bool н(ref String Η,out string ΑΩ,Char Ф){int Β=Η.IndexOf(Ф);ΑΩ="";if(Β<0) return false;ΑΩ=Η.Substring(0,Β);Η=Η.Remove(0,Β+1);return true;}bool ΖЎ(ref String δ,out String ȣ,out String ȝ)
{ȣ="";ȝ="";if(!Ɛ) return false;String ȉ="";if(!н(ref δ,out ȉ,BROADCAST_SEP)||!ΔΑ(ȉ)) return false;if(!н(ref δ,out ȣ,BROADCAST_SEP)) return false;if(!н(ref δ,out ȉ,BROADCAST_SEP)||(ȉ!=Ζν()&&(ȉ!="*"&&Ξ!=Ν.Ǐ))) return false;if(!н(ref δ,out ȉ,BROADCAST_SEP)||(ȉ!=ΔЁ)) return false;
if(!н(ref δ,out ȝ,BROADCAST_SEP)) return false;return true;}void Ζύ(){bool ΑΝ=true;if(ΗІ){ΗІ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Αί=(Ξ!=Ν.Ǐ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Αί)
Ζώ(Αί);String[]Π=Me.CustomData.Split('\n');Ɛ=bool.Parse(ΒΠ(ΑЏ(Π,"Enable_Broadcast",ref ΑΝ)));bool ΖΝ=false;bool Ηϊ=true;if(Ɛ){if(Ξ!=Ν.Ǐ)
Ư=ΒΠ(ΑЏ(Π,"Ship_Name",ref ΑΝ)).Replace(BROADCAST_SELF_ID,"");ΔЁ=ΒΠ(ΑЏ(Π,"Broadcast_Channel",ref ΑΝ)).ToLower();Ηϊ=false;if(ǻ==null){ǻ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ǻ.SetMessageCallback("");}List<IMyRadioAntenna>Ƌ=new List<IMyRadioAntenna>();χ.GetBlocksOfType(Ƌ);
bool Δσ=false;for(int i=0;i<Ƌ.Count;i++)
if(Ƌ[i].EnableBroadcasting&&Ƌ[i].Enabled){Δσ=true;break;}if(Ƌ.Count==0) Μ="No Antenna found";else if(!Δσ) Μ="Antenna not ready";ΖΝ=Ƌ.Count==0||!Δσ;if(ΗΨ&&!ΖΝ&&Ξ!=Ν.Ǐ) Μ="Antenna ok";}else if(Ξ==Ν.Ǐ)
Μ="Offline - Enable in PB custom data";ΗΨ=ΖΝ;if(Ηϊ){if(ǻ!=null) this.IGC.DisableBroadcastListener(ǻ);ǻ=null;}}catch{ΑΝ=false;}if(!ΑΝ){Μ="Reset custom data";Ζώ(Αί);}}void Ζώ(String Αί){String Η=Αί+"\n\n"+"Enable_Broadcast="+(Ξ==Ν.Ǐ?"true":"false")+" \n";Η+=Ξ!=Ν.Ǐ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Η+"Broadcast_Channel=#default";}String ΒΠ(String ũ){int Β=ũ.IndexOf("//");if(Β!=-1) ũ=ũ.Substring(0,Β);String[]ψ=ũ.Split('=');if(ψ.Length<=1) return "";return ψ[1].Trim();}String ΑЏ(String[]Π,String э,ref bool Ċ){foreach(String ũ in Π)
if(ũ.StartsWith(э)) return ũ;Ċ=false;return "";}void Ιγ(string Ш){if(ğ) Ї=new List<ϊ>();String ȣ="";String ȝ="";if(!ΔΑ(Ш)) ΚΚ(Ш);bool Θψ=false;int Εμ=50;while(ǻ!=null&&ǻ.HasPendingMessage&&Εμ-->0){MyIGCMessage δ=ǻ.AcceptMessage();String Η=δ.Data as String;if(Η==null) continue;
if(ΖЎ(ref Η,out ȣ,out ȝ)&&ȣ!=""&&ȣ!=BROADCAST_SELF_ID){ϊ Δ=ΓϏ(ȣ,"");if(Δ==null){Δ=new ϊ(ȣ);Ї.Add(Δ);}Δ.ķ(ȝ,false,false,true);Κυ(Δ,Η);Θψ=true;}}if(Θψ)
Ї.Sort(delegate(ϊ a,ϊ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(Ĥ||ğ){if(Αρ<=0||ğ){Μ="";ğ=false;ΚΧ();Ζύ();Εί=DateTime.Now;}}if(ĥ) Κν();if(ĥ) Κρ();if(ĥ) Κϊ();if(ĥ) Κφ();if(ĥ&&ΕЍ==0) Κλ();
Κκ();}void Κυ(ϊ Δ,String Η){Δ.Εά=DateTime.Now;String ΑΩ="";н(ref Η,out Δ.Γζ,BROADCAST_SEP);н(ref Η,out Δ.ΰ,BROADCAST_SEP);if(Δ.Γζ!=VERSION) return;String Ακ="";String[]Ʃ=new string[3];н(ref Η,out Ακ,BROADCAST_SEP);н(ref Η,out Ʃ[0],BROADCAST_SEP);н(ref Η,out Ʃ[1],BROADCAST_SEP);
н(ref Η,out Ʃ[2],BROADCAST_SEP);н(ref Η,out Δ.ΕЈ,BROADCAST_SEP);н(ref Η,out Δ.Μ,BROADCAST_SEP);н(ref Η,out ΑΩ,BROADCAST_SEP);н(ref Η,out Δ.ΓΕ,BROADCAST_SEP);String ΙΔ="",Θί="";н(ref Η,out ΙΔ,BROADCAST_SEP);н(ref Η,out Θί,BROADCAST_SEP);Δ.Б.Clear();while(true){String ΘΝ;if(!н(ref Η,out ΘΝ,BROADCAST_SEP)) break;
String[]ΒЊ=ΘΝ.Split('/');if(ΒЊ.Count()<3) continue;int А=0;if(!int.TryParse(ΒЊ[2],out А)) continue;Δ.Б.Add(new у(ΒЊ[0],ΒЊ[1],А,Я.Ƃ));}int Θγ=0;if(!int.TryParse(ΑΩ,out Δ.Г)) Δ.Г=0;if(!int.TryParse(Ακ,out Θγ)) Δ.Ξ=Ν.Ĺ;if(!float.TryParse(Ʃ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ʃ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ʃ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(ΙΔ,out Δ.ș)) Δ.ș=0;if(!float.TryParse(Θί,out Δ.ȡ)) Δ.ȡ=0;Δ.Ξ=(Ν) Θγ;Δ.ΔА=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΖΕ=0;for(int i=0;i<Δ.Б.Count();i++) Δ.ΖΕ+=Δ.Б[i].А;}ϊ ΓϏ(String Θ,String ΰ){Θ=Θ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Ї.Count;i++){if(Θ!=""&&Ї[i].Θ.ToUpper()==Θ) return Ї[i];if(ΰ!=""&&Ї[i].ΰ!=null&&Ї[i].ΰ.ToUpper()==ΰ) return Ї[i];}return null;}String Ɠ="";ϊ Ηθ=null;DateTime Ηι=DateTime.MinValue;List<ϊ>ĝ=new List<ϊ>();const float ΙΣ=20f;bool ΗΖ(ϊ ε,String І){return І==Ɠ&&ε==Ηθ&&((DateTime.Now-Ηι).TotalSeconds<ΙΣ||
(І=="HOMEPOS"&&(Ȁ.Count>0||Ć!=null)));}String Ιξ(ϊ ε){if(Ɠ==""||!ΗΖ(ε,Ɠ)) return "";String s="";int Ċ=0,ΕЃ=0;for(int i=0;i<ĝ.Count;i++){Џ ΓΪ=ĝ[i].ķ(Ɠ,false,false,false);if(ΓΪ==Џ.ǈ) Ċ++;bool Αξ=Ȁ.Contains(ĝ[i]);if(ΕЃ++<8) s+="  "+ĝ[i].ΰ+": "+(Αξ?"waiting to leave ("+(Ȁ.IndexOf(ĝ[i])+1)+")":ΓΪ==Џ.ǈ?"received":
ΓΪ==Џ.Ɔ?"pending...":ΓΪ==Џ.ΒΝ?"no answer!":"-")+"\n";}if(ΕЃ>8) s+="  +"+(ΕЃ-8)+" more\n";return "--------------------------------------------\n"+Ɠ+": "+Ċ+"/"+ĝ.Count+" received\n"+s;}void Ǹ(ϊ Δ,String І){Ɠ=І;Ηθ=Δ;Ηι=DateTime.Now;ĝ.Clear();if(Δ==null) ĝ.AddRange(Ї);else ĝ.Add(Δ);
if(Δ==null){Ȁ.Clear();Ć=null;}else Ȁ.Remove(Δ);if(Δ==null&&І=="HOMEPOS"){Κί();return;}if(Δ==null){for(int i=0;i<Ї.Count;i++)
Ї[i].ķ(І,true,false,false);ΒΥ(І,"*",І);}else{Δ.ķ(І,true,false,false);ΒΥ(І,Δ.Θ,І);}}void ΚΚ(string Ш){if(Ш=="") return;var ψ=Ш.ToUpper().Split(' ');ψ.DefaultIfEmpty("");var І=ψ.ElementAtOrDefault(0);var Ȳ=ψ.ElementAtOrDefault(1);String ć="Invalid argument: "+Ш;if(Ѐ==Φ.ǋ){if(О!=null)
{switch(І){case "UP":{if(Г<2) break;if(О.Г==0){Г=1;return;}Ǹ(О,"UP");return;}case "DOWN":{if(Г<1) break;if(Г==1){Ǹ(О,"MRES");break;}Ǹ(О,"DOWN");break;}case "APPLY":{if(Г<2) break;Ǹ(О,"APPLY");return;}}}}switch(І){case "UP":this.ΔΖ(false);return;case "DOWN":this.ΔΕ(false);return;
case "APPLY":this.ž(true);return;}switch(І){case "CLEAR":Ιέ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ȳ)){Μ="Usage: SENDALL <cmd>";return;}Ǹ(null,Ȳ);return;case "SEND":
if(Ш.Length<="SEND".Length+1){Μ="Usage: SEND <ship>:<cmd>";return;}ΚΪ(Ш.Remove(0,"SEND".Length+1));return;}Μ=ć;}void ΚΪ(String ΑΖ){if(ΑΖ=="") return;var ψ=ΑΖ.Split(':');if(ψ.Length!=2){Μ="Missing separator \":\"";return;}ψ.DefaultIfEmpty("");String Ư=ψ.ElementAtOrDefault(0).Trim();
ϊ Δ=ΓϏ("",Ư);if(Δ!=null) Ǹ(Δ,ψ.ElementAtOrDefault(1).Trim());else Μ="Unknown ship: "+Ư;}void Ιέ(){Ї.Clear();О=null;ξ(Φ.Main);Г=0;}void ΚΧ(){List<IMyTerminalBlock>Αψ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Э,Ǡ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αψ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Э=Ǵ(Э,pamTag,true);ΗΔ(Αψ);ΗΓ();ΚΨ();ŗ(Э,setLCDFontAndSize,1.15f,false);ŗ(ȥ,setLCDFontAndSize,1.15f,true);String ΛΥ=Ζς(false);String Θθ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock я in Э){if(я.CustomData==""){я.CustomData=Ζς(true);continue;}
if(!я.CustomData.Contains(ΛΥ))
if(!я.CustomData.Contains(Θθ)) я.CustomData=Θθ+я.CustomData;}}void Κκ(){String ΑΥ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ȥ.Count;i++)
ȥ[i].WriteText(ΖΩ(Ц.ΖЇ,"0",false));for(int i=0;i<Э.Count;i++){Ц Д=Ц.Ĺ;String Ȇ="";ΚΔ(Э[i],out Д,out Ȇ);Э[i].WriteText(ΖΩ(Д,Ȇ,i==0));}Echo(ΑΥ+Ιπ());for(int i=0;i<Š.Count;i++){IMyTextPanel я=Š[i];String Η=я.CustomData.ToUpper();if(Η=="DEBUG") я.WriteText(""+"\n"+"");
if(Η=="INSTRUCTIONS") я.WriteText(Ζρ());}}String ž(bool Λ){int ΛΙ=0;return ž(Λ,0,ref ΛΙ,false,1);}String ž(bool Λ,int Λπ,ref int ΛЀ,bool Ημ,int Αμ){String Γ="";String Ʈ="——————————————————\n";String ΛЏ="--------------------------------------------\n";if(О==null||Ѐ==Φ.Main||Ѐ==Φ.Δά)
Γ+="[PAM]-Controller | "+Ї.Count+" Connected ships"+"\n"+Ις()+Ιχ()+Ιό();else Γ+=Ζυ(О)+"\n";Γ+=Ʈ;int Β=0;int Κ=Г;ū=0;if(Μ!=""){Ο(ref Γ,Κ,Β++,Λ,Μ);}else if(Ѐ==Φ.Main){if(О==null&&Ї.Count>=1) О=Ї[0];String В="";if(Ο(ref В,Κ,Β++,Λ," Send command to all")) ξ(Φ.Δά);
int ƭ=0;for(int s=0;s<Ї.Count;s++){if(Ημ) В+="\n";ϊ Δ=Ї[s];if(Κ==Β||Κ==Β+1) О=Δ;if(Β==Κ) ƭ=В.Split('\n').Length-1;if(Ο(ref В,Κ,Β++,Λ," "+Ζυ(Δ))){О=Δ;ξ(Φ.ǋ);О.ķ("",false,true,false);}if(Β==Κ) ƭ=В.Split('\n').Length-1;if(Ο(ref В,Κ,Β++,Λ," "+(Ημ?"":"  ")+Ιω(Δ)))
{О=Δ;ξ(Φ.ΗΕ);}}int ƹ=Αμ-2;ƹ+=Math.Max(0,(Λπ-1))*Αμ;Γ+=ΔΫ(ƹ,В,ƭ,ref ΛЀ);}else if(Ѐ==Φ.ΒΦ||Ѐ==Φ.Δά){Φ ΚЃ=Ѐ==Φ.ΒΦ?Φ.ǋ:Φ.Main;ϊ ε=null;if(Ѐ==Φ.ΒΦ) ε=О;if(Ο(ref Γ,Κ,Β++,Λ," Back")){ξ(ΚЃ);}if(Ο(ref Γ,Κ,Β++,Λ," [Stop] "+Ɓ(ε,"STOP"))){Ǹ(ε,"STOP");}if(Ο(ref Γ,Κ,Β++,Λ," [Continue] "+Ɓ(ε,"CONT"))){Ǹ(ε,"CONT");}
if(Ο(ref Γ,Κ,Β++,Λ," [Move home] "+Ɓ(ε,"HOMEPOS"))){Ǹ(ε,"HOMEPOS");}if(Ο(ref Γ,Κ,Β++,Λ," [Move to job] "+Ɓ(ε,"JOBPOS"))){Ǹ(ε,"JOBPOS");}if(Ο(ref Γ,Κ,Β++,Λ," [Full simulation] "+Ɓ(ε,"FULL"))){Ǹ(ε,"FULL");}if(ε!=null&&ε.Ξ==Ν.ί)
if(Ο(ref Γ,Κ,Β++,Λ," [Undock] "+Ɓ(ε,"UNDOCK"))){Ǹ(ε,"UNDOCK");}Γ+=Ιξ(ε);}else if(Ѐ==Φ.ǋ){String Δϊ="";if(Ο(ref Δϊ,Κ,Β++,Λ," Back")){ξ(Φ.Main);}Γ+=Δϊ.Substring(0,Δϊ.Length-1).PadRight(36,' ');if(Ο(ref Γ,Κ,Β++,Λ," Send cmd")){ξ(Φ.ΒΦ);}if(!О.ΔΒ()&&!О.ΓЏ()) ū++;
if(О.ΓЏ()) Γ+="No answer received...";else Γ+=ƈ(О,О.ΓΕ);}else if(Ѐ==Φ.ΗΕ){if(Ο(ref Γ,Κ,Β++,Λ," Back")){ξ(Φ.Main);}Γ+=ΛЏ;Γ+=ƈ(О,Ζτ(О));}if(!Ελ) Г=Math.Min(ū-1,Г);return Γ;}String Ζυ(ϊ Δ){if(Δ.Γζ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+ƈ(Δ,Δ.ΕЈ)+" "+Δ.ΔА+"m";
}String Ιω(ϊ Δ){String s=ΔΨ("",М(Δ.ș,Δ.ȡ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Б.Count;i++){if(i>=5) break;у Ђ=Δ.Б[i];s+=ΗΡ(Γρ(Ιφ(Ђ)),3)+" "+Ζλ(Ђ.А)+" ";}return s;}String Ιφ(у Ђ){if(Ђ.ω.ToUpper()=="ORE"||Ђ.ω.ToUpper()=="INGOT"){String Ηκ=Ιτ(Ђ.ΰ.ToUpper());if(Ηκ!="") return Ηκ;
}return Ђ.ΰ;}String ƈ(ϊ Δ,String Γ){if(Δ.ΔΒ()) return "No signal...("+ΒΑ((int)(DateTime.Now-Δ.Εά).TotalSeconds)+")";return Γ;}String Ζς(bool ΜΥ){String Γ="";if(ΜΥ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΚΔ(IMyTerminalBlock я,out Ц Д,out String Ȇ){bool Ċ=true;String Ακ=ΒΠ(ΑЏ(я.CustomData.Split('\n'),"mode",ref Ċ)).ToUpper();String[]ψ=Ακ.Split(':');String Αι=ψ.Length>0?ψ[0].Trim():"";Ȇ=ψ.Length>1?ψ[1].Trim():"";
Д=Ц.Ĺ;if(Αι=="MAIN") Д=Ц.ΖЈ;else if(Αι=="MAINX") Д=Ц.ΖЉ;else if(Αι=="MENU") Д=Ц.ΔΔ;else if(Αι=="INVENTORY") Д=Ц.ΓЎ;else if(Αι=="DEBUG") Д=Ц.Γυ;if(Д==Ц.Ĺ) Ȇ=Ακ;}String ΖΩ(Ц Д,String Ȇ,bool ΛΛ){int Αζ=15;if(ΛΛ){Ηξ=Βλ;Βλ=0;Ηο=Βμ;Βμ=0;}String Γ="";if(Д==Ц.ΖЈ)
{int ю=0;Γ=ž(false,Ηξ,ref ΚЌ,true,Αζ);if(!int.TryParse(Ȇ,out ю)) return Γ;Βλ=Math.Max(ю,Βλ);ю--;return ΖЍ(ю,Αζ,Γ);}else if(Д==Ц.ΖЉ){int ю=0;Γ=ž(false,Ηο,ref ΚЍ,false,Αζ);if(!int.TryParse(Ȇ,out ю)) return Γ;Βμ=Math.Max(ю,Βμ);ю--;return ΖЍ(ю,Αζ,Γ);}if(Д==Ц.ΖЇ){return ž(false,0,ref ΚЎ,true,Αζ);
}else if(Д==Ц.ΓЎ||Д==Ц.ΔΔ||Д==Ц.Γυ){ϊ Δ=О;if(Ȇ!=""){Δ=ΓϏ("",Ȇ);if(Δ==null) return "Unknown ship: "+Ȇ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ʈ="—————————————————————\n";String ΖΒ="";String ΔЀ="";if(Д==Ц.ΔΔ){ΖΒ="Menu";ΔЀ=ƈ(Δ,Δ.ΓΕ);}
else if(Д==Ц.ΓЎ){ΖΒ="Inventory";ΔЀ=ƈ(Δ,Ζτ(Δ));}else if(Д==Ц.Γυ){return ƈ(Δ,Ιϊ(Δ));}Γ+=Δ.ΰ+" - "+Δ.ΔА+"m | "+Γρ(""+ΖΒ)+"\n"+Ʈ;Γ+=ΔЀ;return Γ;}return "Unknown command: "+Ȇ;}string Ιϊ(ϊ Δ){String s="";s+=Δ.Γζ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΕЈ+"\n";s+=Δ.Μ+"\n";s+=Δ.Г+"\n";s+=Δ.ΓΕ.Length+"\n";s+=Δ.ș+"\n";s+=Δ.ȡ+"\n";s+=Δ.Б.Count()+"\n";return s;}string Ζτ(ϊ Δ){String s="";s+=ΔΨ("All",М(Δ.ș,Δ.ȡ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Б.Count();i++){у Ђ=Δ.Б[i];
s+=ΔΨ(Ђ.ΰ,Ђ.А,Δ.ΖΕ,30,8,10)+"\n";}return s;}Џ Ιο(ϊ Δ,bool ж,string І){Џ α=Џ.None;if(Δ==null){for(int i=0;i<Ї.Count;i++){Џ Ј=Ї[i].ķ(І,false,ж,false);if(Ј==Џ.ǈ) α=Ј;if(Ј==Џ.ΒΝ) return Ј;if(Ј==Џ.Ɔ) return Ј;}return α;}else return Δ.ķ(І,false,ж,false);}String Ɓ(ϊ Δ,String І)
{if(!ΗΖ(Δ,І)) return "";Џ Ј=Ιο(Δ,false,І);if(Ј==Џ.ǈ) return "received!";if(Ј==Џ.Ɔ) return "pending...";if(Ј==Џ.ΒΝ) return "no answer!";return "";}String ΔΫ(int ƹ,String Γ,int ƭ,ref int ī){String[]Π=Γ.Split('\n');if(ƭ>=ī+ƹ-1) ī++;ī=Math.Min(Π.Count()-1-ƹ,ī);if(ƭ<ī+1) ī--;
ī=Math.Max(0,ī);String α="";for(int i=0;i<ƹ;i++){int ΘΧ=i+ī;if(ΘΧ>=Π.Count()) break;α+=Π[ΘΧ]+"\n";}return α;}String ΖЍ(int ю,int Αμ,String Γ){String[]Π=Γ.Split('\n');int Αυ=ю*Αμ;int ț=(ю+1)*Αμ;String α="";for(int i=Αυ;i<ț;i++){if(i>=Π.Count()) break;α+=Π[i]+"\n";
}return α;}String Γρ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΗΡ(String s,int Θέ){if(s=="") return s;if(s.Length>Θέ) s=s.Substring(0,Θέ-1)+".";return s;}string ΔΨ(String ŧ,float value,float ơ,int Ηί,int ΘΡ,int Λο){float ű=М(value,ơ)*Ηί;
String Βζ="[";for(int i=0;i<Ηί;i++){if(i<=ű) Βζ+="|";else Βζ+="'";}Βζ+="]";return Βζ+" "+ΗΡ(Γρ(ŧ),ΘΡ).PadRight(ΘΡ)+"".PadRight(Λο)+Ζλ(value);}String Ζλ(float А){if(А>=1000000) return Math.Round(А/1000000f,А/1000000f<100?1:0)+"M";if(А>=1000) return Math.Round(А/1000f,А/1000f<100?1:0)+"K";
return ""+Math.Round(А);}String ΒΑ(int Ας){if(Ας>=60*60) return Math.Round(Ας/(60f*60f),1)+" h";if(Ας>=60) return Math.Round(Ας/60f,1)+" min";return ""+Ας+" s";}string ƀ(float v){return Math.Round(v,2)+" ";}string Μά(Vector3 v){return "X"+ƀ(v.X)+"Y"+ƀ(v.Y)+"Z"+ƀ(v.Z);}
List<IMyCameraBlock>Ȗ=new List<IMyCameraBlock>();List<long>ƥ=new List<long>();bool Ƨ=false;int ΔІ=0,Λύ=0,Δώ=0;const int ΚΠ=16;int ΔЄ=0;DateTime Ηβ=DateTime.Now;String ΔϏ="";String ΔЃ="";void ΚΦ(){Ȗ.Clear();ƥ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(Ȗ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȗ.Count;i++) Ȗ[i].EnableRaycast=true;List<IMyTerminalBlock>Ββ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ββ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<Ββ.Count;i++) if(!ƥ.Contains(Ββ[i].CubeGrid.EntityId)) ƥ.Add(Ββ[i].CubeGrid.EntityId);if(!ƥ.Contains(Me.CubeGrid.EntityId)) ƥ.Add(Me.CubeGrid.EntityId);
ΔЃ="";if(Ω==null) return;MatrixD m=Ω.WorldMatrix;Vector3[]ΛΔ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Λη={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ȗ.Count;i++)
if(Vector3.Dot(Ȗ[i].WorldMatrix.Forward,ΛΔ[d])>0.7f){ΔЃ+=Λη[d]+" ";break;}}bool ΖЁ(Α s){return s==Α.ǵ||s==Α.т||s==Α.И||
s==Α.Ĕ||s==Α.ſ||s==Α.ΑА||
(s==Α.ȏ&&ǩ!=null);}void ΑЋ(){Ƨ=false;ΔІ=0;ΔϏ="";}void Κσ(){if(!obstacleBrake||Ȗ.Count==0||!ΖЁ(Ϋ)){ΑЋ();return;}Vector3 ΑΪ=ώ+į;Vector3 ȧ=ΑΪ-β;float Ȋ=ȧ.Length();if(Ȋ<1f){ΑЋ();return;}Vector3 π=ȧ/Ȋ;Vector3 д=Ω!=null?(Vector3) Ω.GetShipVelocities().LinearVelocity:new Vector3();
if(д.Length()>2) π=Vector3.Normalize(д);Vector3 ΓΦ=Vector3.Cross(π,Math.Abs(π.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΓΦ.Normalize();Vector3 Љ=Vector3.Cross(ΓΦ,π);float υ=Math.Min(Ȋ+θ*0.25f,Ď*obstacleLookAhead+θ);int Ηε=0;for(int k=0;k<2;k++){int p=k==0?0:Λύ++%8,e=p%4;
Vector3 ΓΙ=k==0?new Vector3():(e==0?ΓΦ:e==1?-ΓΦ:e==2?Љ:-Љ)*θ*0.5f;Vector3 ț=β+π*(p>=4?υ*0.5f:υ)+ΓΙ;IMyCameraBlock ě=ΙΧ(ț);if(ě==null){Ζγ();continue;}Ηε++;MyDetectedEntityInfo Ǔ=ě.Raycast(ț);bool ΜΞ=Ǔ.Type==MyDetectedEntityType.Planet||Ǔ.Type==MyDetectedEntityType.Asteroid;
bool Θι=!Ǔ.IsEmpty()&&!ƥ.Contains(Ǔ.EntityId)&&!ΙЍ(Ǔ.EntityId)&&
Ǔ.Type!=MyDetectedEntityType.FloatingObject&&!(ΜΞ&&(k>0||Ϋ==Α.ǵ||
(Ǔ.HitPosition.HasValue&&Vector3D.Distance(Ǔ.HitPosition.Value,ΑΪ)<θ)));if(Θι){if(!Ƨ) Ηβ=DateTime.Now;Ƨ=true;ΔІ=0;ΔϏ=Ǔ.Type.ToString();}else Ζγ();if(debugDraw&&Βφ.Count<16) Βφ.Add(ο(ě.GetPosition())+","+ο(ț)+","+(Θι?1:0));}Δώ=Ηε>0?0:Δώ+1;if(Δώ>12) Μ="Cameras can't see ahead";
if(Ƨ&&(DateTime.Now-Ηβ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΔϏ;}void Ζγ(){if(Ƨ&&++ΔІ>=ΚΠ) ΑЋ();}IMyCameraBlock ΙΧ(Vector3 ț){IMyCameraBlock Њ=null;float Ηα=-2;for(int n=0;n<Ȗ.Count;n++){ΔЄ=(ΔЄ+1)%Ȗ.Count;IMyCameraBlock c=Ȗ[ΔЄ];if(!c.IsFunctional||!c.IsWorking) continue;float Θυ=Vector3.Dot(Vector3.Normalize(ț-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Θυ>Ηα+0.05f&&c.CanScan(ț)){Њ=c;Ηα=Θυ;}}return Њ;}const String Δθ="[PAMTFC]";const float Κδ=5f;const float Γώ=2f;class ĸ{public long Θ;public Vector3 ά,д;public Vector3 ȟ;public float Ηω;public String Ι="";public int ġ=-1;public bool Αξ;public int Ȃ;public int Ǜ;public float Αά,ΑΨ;
public long γ=0;public int ΑΠ=0;public DateTime Ȧ;}IMyBroadcastListener Γα=null;long ћ=0;List<ĸ>ў=new List<ĸ>();Vector3 į=new Vector3();float ǝ=-1;Vector3 ΖΖ=new Vector3();bool Γΰ=false;DateTime ΖΗ=DateTime.Now;const float ΗΟ=15f;const float Κγ=10f;int ΙΑ=0;String й="";int Й=-1;
bool Βο=false,Βξ=false,ΑΧ=false;Vector3 Ηπ=new Vector3();String Ŕ(){if(ζ==Σ.ы||Ξ==Ν.ί) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ћ+"|"+Ь+"|"+(int) Ŧ+"|"+Math.Round(ǰ,1)+"|"+Math.Round(Ȍ,1);}void Κψ(){if(!trafficEnabled) return;if(Γα==null) Γα=IGC.RegisterBroadcastListener(Δθ);ћ=Me.CubeGrid.EntityId;int ǣ=30;while(Γα.HasPendingMessage&&ǣ-->0){String Η=Γα.AcceptMessage().Data as String;if(Η!=null) ΚΖ(Η);
}for(int i=ў.Count-1;i>=0;i--)
if((DateTime.Now-ў[i].Ȧ).TotalSeconds>Κδ) ў.RemoveAt(i);if(Γέ==null) Γέ=IGC.RegisterBroadcastListener(ΗΝ);ǣ=5;while(Γέ.HasPendingMessage&&ǣ-->0){String Η=Γέ.AcceptMessage().Data as String;if(Η!=null) try{ΚΗ(Η);}catch{}}if(Βπ==null) Βπ=IGC.RegisterBroadcastListener(ΖЃ);
ǣ=10;while(Βπ.HasPendingMessage&&ǣ-->0){String Η=Βπ.AcceptMessage().Data as String;if(Η!=null) try{ΚΓ(Η);}catch{}}if(++ΙΑ<3) return;ΙΑ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",ћ+";"+ο(Ζ.Ε)+";"+
ο(ζ==Σ.ы?new Vector3():Ζ.Ѓ)+";"+ο(ĉ)+";"+
ο(ώ+į)+";"+String.Join(":",Βφ));Βφ.Clear();}IGC.SendBroadcastMessage(Δθ,ћ+";"+ο(β)+";"+
ο(Ω.GetShipVelocities().LinearVelocity)+";"+θ+";"+Ŕ()+";"+
(ζ==Σ.Л?Й:-1)+";"+й+";"+Ιά()+","+Ζθ()+";"+ο(ΖΖ)+
";"+(Βϊ?1:0)+";"+(Ń&&ζ==Σ.Л?1:0)+","+ΓΣ+","+ΕЁ+","+ΙА()+","+ΗΠ());}void ΚΖ(String Η){try{String[]f=Η.Split(';');if(f.Length<7) return;long Θ=long.Parse(f[0]);if(Θ==ћ) return;ĸ Δ=null;for(int i=0;i<ў.Count;i++) if(ў[i].Θ==Θ) Δ=ў[i];if(Δ==null){Δ=new ĸ();Δ.Θ=Θ;ў.Add(Δ);}
Δ.ά=κ(f[1]);Δ.д=κ(f[2]);Δ.Ηω=float.Parse(f[3]);Δ.Ι=f[4];Δ.ġ=int.Parse(f[5]);Δ.Ȧ=DateTime.Now;if(Δ.Ι!=""&&Δ.Ι==Ŕ()) ΖЊ(f[6]);Δ.γ=0;Δ.ΑΠ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.γ=long.Parse(d[0]);Δ.ΑΠ=int.Parse(d[1]);}Δ.ȟ=f.Length>=9?κ(f[8]):new Vector3();
Δ.Αξ=f.Length>=10&&f[9]=="1";Δ.Ȃ=0;Δ.Ǜ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.Αά=float.Parse(w[1]);Δ.ΑΨ=float.Parse(w[2]);Δ.Ȃ=int.Parse(w[3]);if(w.Length>=5) Δ.Ǜ=int.Parse(w[4]);}}}catch{}}bool ΙЍ(long Θ){for(int i=0;i<ў.Count;i++) if(ў[i].Θ==Θ) return true;
return false;}Vector3 Ζσ(int Β,int Λρ){if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();έ ή=Ψ[Β];Vector3 ΜΛ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Λρ;Vector3 Љ=ή.ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ή.ϋ):-ή.λ;Vector3 ч=Vector3.Cross(ΜΛ,Љ);
if(ч.LengthSquared()<0.0001f) return new Vector3();float υ=laneOffset*θ;float ΛΝ=Math.Min(Vector3.Distance(ή.Ε,Ψ[0].Ε),
Vector3.Distance(ή.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(ч)*υ*Math.Min(1,ΛΝ/(υ*4));}Vector3 ΘБ=new Vector3();List<String>Βφ=new List<String>();bool Ζϊ(){Α s=Ϋ;return Ŕ()!=""&&(s==Α.ǵ||s==Α.ŕ||s==Α.ȑ||
s==Α.Ƿ||s==Α.Œ);}float ΖЅ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Ћ*ǰ,2)+Math.Pow(Ь*Ȍ,2));}bool ΔΚ(){return Ŕ()!=""&&Vector3.Distance(β,Ζ.Ε)<ΖЅ()+θ*2;}int ΗΠ(){Α s=Ϋ;if(s==Α.Ƿ||s==Α.Ĕ||(s==Α.И&&ΔΚ())) return 2;return s==Α.ǵ||s==Α.ȑ||s==Α.Œ||s==Α.т?1:0;
}int ΙА(){if(!Ζϊ()) return 0;return Ϋ!=Α.ǵ&&Vector3.Dot(β-Ю,м)>θ*0.5f?2:1;}bool ΙЌ(Α s){return s==Α.ȏ||s==Α.ō||s==Α.ő||s==Α.ǉ||
s==Α.Ǻ||s==Α.ȑ||s==Α.Ƿ||s==Α.Œ||s==Α.Ĳ;}void Κω(){į=new Vector3();ǝ=-1;ΖΖ=new Vector3();bool ΑΫ=ΖЁ(Ϋ);if(!trafficEnabled||(!ΑΫ&&!ΙЌ(Ϋ))){Γΰ=false;return;}ΘБ=Ω.GetShipVelocities().LinearVelocity;Ιή(Ω.GetNaturalGravity(),!ΑΫ);ΙΚ();}IMyBroadcastListener Γέ=null;
float Ƴ=-1;DateTime ΘЍ=DateTime.MinValue;void ΚΗ(String Η){if(!Η.StartsWith("C;")) return;ΘЍ=DateTime.Now;Ƴ=-1;String Αθ=ћ+":";String[]e=Η.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αθ)) Ƴ=float.Parse(e[i].Substring(Αθ.Length));}void ΙΚ(){if(Ƴ<0||(DateTime.Now-ΘЍ).TotalSeconds>1.5) return;
ǝ=ǝ<0?Ƴ:Math.Min(ǝ,Ƴ);if(Ƴ<0.5f) Μ="Traffic: giving way (controller)";}void Ιή(Vector3 ϋ,bool Θξ=false){į=new Vector3();ǝ=-1;Vector3 ȧ=ώ-β;float Ȋ=ȧ.Length();bool ΕΡ=Ȋ>=1f;Vector3 π=ΕΡ?ȧ/Ȋ:new Vector3();ΖΖ=π;Vector3 Љ=ϋ.LengthSquared()>0.01f?-Vector3.Normalize(ϋ):-ΑΣ;
Vector3 ч=Vector3.Cross(π,Љ);ч=ч.LengthSquared()<0.0001f?-Αε:Vector3.Normalize(ч);ĸ ΒЌ=null;bool ΗЊ=Ϋ==Α.И&&!Ę(1);int Εξ=ΗΠ();for(int i=0;i<ў.Count;i++){ĸ η=ў[i];if(η.ΑΠ==3&&η.д.LengthSquared()<0.25f) continue;Vector3 ά=η.ά+η.д*(float)(DateTime.Now-η.Ȧ).TotalSeconds;
Vector3 ǫ=ά-β;float ж=(θ+η.Ηω)*0.5f;bool Απ=η.Ι!=""&&η.Ι==Ŕ();if(Απ&&η.Ȃ==2) continue;bool ΓΧ=Απ&&η.Ȃ==1&&Ζϊ();if(ΓΧ) ж=Math.Min(ǰ,Ȍ)*0.9f;float ΔЊ=ж*(ΓΧ?1f:1.2f);float Ļ=ΕΡ?Vector3.Dot(ǫ,π):0;if(Ļ>0&&(ǫ-π*Ļ).Length()<=ΔЊ){float ΓΛ=Vector3.Dot(η.д,π);
bool ΗЎ=η.ȟ.LengthSquared()>0.25f;bool ΗА=ΓΛ<-1f||(ΗЎ&&Vector3.Dot(η.ȟ,π)<-0.5f);if(!Θξ&&η.Ǜ<=Εξ){if(η.Αξ){if(!ΗЊ){į=ч*ж*1.3f;continue;}float Θβ=Vector3.Distance(β,ώ),ΘΞ=Vector3.Distance(ά,ώ);if(Θβ<ΘΞ||(Θβ==ΘΞ&&ћ<η.Θ)) continue;}if(ΗА){if(Ļ<(Ď-ΓΛ)*6f+ж*4f) į=ч*ж*1.3f;
continue;}if(ΗЎ&&ћ<η.Θ){Vector3 Ηέ=-ǫ;float Θμ=Vector3.Dot(Ηέ,η.ȟ);if(Θμ>0&&(Ηέ-η.ȟ*Θμ).Length()<=ΔЊ){į=ч*ж*1.3f;continue;}}}float ΗЉ=Ļ-ж-(ΓΧ?1f:followGap*θ);float њ=Math.Max(0,ΓΛ)+Math.Max(0,ΗЉ)/Γώ;if(ΗА) њ=Math.Max(0,ΗЉ+Math.Min(0,ΓΛ)*Γώ*2)/Γώ;
if(ǝ<0||њ<ǝ){ǝ=њ;ΒЌ=η;}continue;}if(η.д.LengthSquared()<1f) continue;if(ΕΡ&&(ǫ-π*Ļ).Length()<=ΔЊ) continue;Vector3 ΕϏ=η.д-ΘБ;float Θτ=ΕϏ.LengthSquared();if(Θτ<0.25f) continue;float t=-Vector3.Dot(ǫ,ΕϏ)/Θτ;if(t<=0||t>5f||(ǫ+ΕϏ*t).Length()>=(ΓΧ?ж:ж*1.1f)) continue;
if(η.Ǜ>Εξ||(η.Ǜ==Εξ&&ћ>η.Θ)||t<2f){ǝ=0;ΒЌ=η;}}bool Αΰ=ǝ>=0&&ǝ<0.5f;if(!Αΰ||Θξ||(ΒЌ!=null&&ΒЌ.Αξ&&ΗЊ)){Γΰ=false;return;}if(!Γΰ){Γΰ=true;ΖΗ=DateTime.Now;}double ΗБ=(DateTime.Now-ΖΗ).TotalSeconds;if(ΗБ>ΗΟ+Κγ) ΖΗ=DateTime.Now;else if(ΗБ>ΗΟ){ǝ=-1;į=ч*θ*1.3f;Μ="Traffic: passing a stuck ship";}
}List<int>ą=new List<int>();String ΘΒ="";void Γύ(){String э=Ћ+"|"+Ь+"|"+(int) Ŧ;if(э==ΘΒ) return;ΘΒ=э;ΖΪ(Ћ,Ь,Ŧ,ą);}void ΖΪ(int w,int h,ѕ Αυ,List<int>ΔЅ){ΔЅ.Clear();if(Αυ==ѕ.ΑΒ){for(int k=0;k<w*h;k++){int ŉ=k/w;ΔЅ.Add(ŉ*w+(ŉ%2==0?k-ŉ*w:w-1-(k-ŉ*w)));
}return;}int Αή=(int) Math.Ceiling(w/2f),Αέ=(int) Math.Ceiling(h/2f);int ΕΞ=w/2,ΕΝ=h/2;int x=0,y=0,ΕΒ=0,ΕΓ=-1,ơ=Math.Max(w,h);for(int i=0;i<ơ*ơ;i++){if(-Αή<x&&x<=ΕΞ&&-Αέ<y&&y<=ΕΝ) ΔЅ.Add((y-1+Αέ)*w+(x-1+Αή));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΕΒ;ΕΒ=-ΕΓ;ΕΓ=t;}
x+=ΕΒ;y+=ΕΓ;}}bool Γό(){int n=Math.Max(0,Ћ*Ь);if(й.Length==n) return false;й=new String('0',n);Й=-1;return true;}void ΖЊ(String η){Γό();if(η.Length!=й.Length) return;char[]h=й.ToCharArray();for(int i=0;i<h.Length;i++) if(η[i]=='1') h[i]='1';й=new String(h);}bool ΒΖ(int а){return а>=0&&а<й.Length&&й[а]=='1';}
void Δή(int а,bool φ){if(а<0||а>=й.Length) return;char[]h=й.ToCharArray();h[а]=φ?'1':'0';й=new String(h);}bool ΓЊ(int а,bool Λά){if(!trafficEnabled) return false;String э=Ŕ();for(int i=0;i<ў.Count;i++){ĸ t=ў[i];if(t.Ι!=э||t.ġ<0||(Λά&&t.Θ>ћ)) continue;if(t.ġ==а) return true;
}return false;}int ΙΫ(int Β){int n=Ћ*Ь;if(n<1) return Β;Γύ();if(Γό()) for(int k=0;k<Β&&k<n;k++) Δή(ą[k],true);if(Й>=0&&(Β>=n||ą[Β]!=Й)){Δή(Й,true);Й=-1;}if(ΖЄ()){if(Ș>=0&&!ΒΖ(Ș)){Ń=false;Й=Ș;return ą.IndexOf(Ș);}if(Ș==-2){Й=-1;Ń=й.IndexOf('0')>=0;return Ń?-2:n;}
if(Ș==-1&&Й>=0&&!ΒΖ(Й)){Ń=false;return Β;}Й=-1;if(!Ń){Ń=true;Ης=DateTime.Now;}if((DateTime.Now-Ης).TotalSeconds<5) return-1;}Ń=false;if(Й>=0&&!ΒΖ(Й)&&!ΓЊ(Й,true)) return Β;Й=-1;for(int k=0;k<n;k++){int c=ą[k];if(ΒΖ(c)||ΓЊ(c,false)) continue;Й=c;
return k;}return й.IndexOf('0')>=0?-2:n;}const String ΖЃ="[PAMJOB]";const String ΗΝ="[PAMTCC]";IMyBroadcastListener Βπ=null;DateTime Ηρ=DateTime.MinValue;DateTime Ης=DateTime.Now;int Ș=-1;bool Ń=false;bool ΖЄ(){return trafficEnabled&&Ș!=-3&&(DateTime.Now-Ηρ).TotalSeconds<3;}
void ΚΓ(String Η){String[]f=Η.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ŕ()) return;Ηρ=DateTime.Now;ΖЊ(f[2]);Ș=-3;String Αθ=ћ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Αθ)) Ș=int.Parse(e[i].Substring(Αθ.Length));}ǹ ΚΫ(int Β)
{Γύ();if(Β<0||Β>=ą.Count) return ǹ.ΑЅ;Є=ą[Β]%Ћ;Ё=ą[Β]/Ћ;return ǹ.ΒΒ;}class ǡ{public long Θ;public Vector3 ά,б,Љ;}List<ǡ>Ъ=new List<ǡ>();bool Ŀ=false;Vector3 Βό,Βϋ,Βύ,Βώ;long Ǖ=0;long Č=0;έ ǩ=null;bool ΑΡ=false;έ Γω(Vector3 ά,Vector3 б,Vector3 Љ){MatrixD m=MatrixD.CreateWorld(ά,б,Љ);
έ w=new έ(Ρ);w.μ=Vector3D.TransformNormal(Βό,m);w.λ=Vector3D.TransformNormal(Βϋ,m);w.Ѓ=Vector3D.TransformNormal(Βύ,m);w.Ε=ά+(Vector3) Vector3D.TransformNormal(Βώ,m);w.ν=true;return w;}int Ζθ(){Α s=Ϋ;if(s==Α.є) return о(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ő||s==Α.ȱ||s==Α.Ő||s==Α.Βί||
s==Α.ΑЎ||s==Α.Ǻ) return 3;if(s==Α.ȏ||s==Α.ō||s==Α.ǉ) return 2;if(s==Α.И) return 1;return 0;}long Ιά(){if(Ζθ()==0) return 0;if(Ę(1)&&ư!=0) return ư;return ǩ!=null?Č:Ǖ;}void ΖІ(){if(Ξ==Ν.ί||!Ρ.ν) return;IMyShipConnector У=о(MyShipConnectorStatus.Connected);if(У==null||У.OtherConnector==null) return;
IMyShipConnector η=У.OtherConnector;Vector3I ΛΦ=new Vector3I((int) Ρ.х.X,(int) Ρ.х.Y,(int) Ρ.х.Z);if(У.Position==ΛΦ){MatrixD ƚ=MatrixD.Transpose(η.WorldMatrix);Βό=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,ƚ);Βϋ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,ƚ);Βύ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,ƚ);
Βώ=Vector3D.TransformNormal(У.GetPosition()-η.GetPosition(),ƚ);Ŀ=true;}if(Vector3.Distance(У.GetPosition(),Ρ.Ε)<5) Ǖ=η.EntityId;List<IMyShipConnector>Ţ=new List<IMyShipConnector>();χ.GetBlocksOfType(Ţ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ţ.Count>0){Ъ.Clear();for(int i=0;i<Ţ.Count;i++){ǡ p=new ǡ();p.Θ=Ţ[i].EntityId;p.ά=Ţ[i].GetPosition();p.б=Ţ[i].WorldMatrix.Forward;p.Љ=Ţ[i].WorldMatrix.Up;Ъ.Add(p);}}ǩ=null;Č=0;if(η.EntityId==Ǖ||!Ŀ) return;for(int i=0;i<Ъ.Count;i++)
if(Ъ[i].Θ==η.EntityId){Č=Ъ[i].Θ;ǩ=Γω(Ъ[i].ά,Ъ[i].б,Ъ[i].Љ);}}bool ΓБ(long Θ,int Θε){if(Θ==0) return false;for(int i=0;i<ў.Count;i++){ĸ t=ў[i];if(t.γ==Θ&&(t.ΑΠ>Θε||(t.ΑΠ==Θε&&t.Θ<ћ))) return true;}return false;}bool Γσ(int Ű){if(Ξ==Ν.ί||!trafficEnabled){ǩ=null;Č=0;return true;}
long σ=ǩ!=null?Č:Ǖ;if(!ΓБ(σ,Ű)) return true;if(!ΓБ(Ǖ,Ű)){ǩ=null;Č=0;return true;}if(!Ŀ) return false;ǡ Њ=null;float ΑΚ=0;for(int i=0;i<Ъ.Count;i++){ǡ p=Ъ[i];if(p.Θ==Ǖ||ΓБ(p.Θ,Ű)) continue;float d=Vector3.Distance(p.ά,Ρ.Ε);if(Њ==null||d<ΑΚ){Њ=p;ΑΚ=d;}
}if(Њ==null) return false;Č=Њ.Θ;ǩ=Γω(Њ.ά,Њ.б,Њ.Љ);return true;}String Ιθ(){String s=(Ŀ?"1":"0")+":"+ο(Βό)+":"+ο(Βϋ)+":"+
ο(Βύ)+":"+ο(Βώ)+":"+Ǖ;for(int i=0;i<Ъ.Count;i++)
s+=":"+Ъ[i].Θ+","+ο(Ъ[i].ά)+","+ο(Ъ[i].б)+","+ο(Ъ[i].Љ);return s;}void Ιη(String s){Ъ.Clear();Ŀ=false;Ǖ=0;String[]f=s.Split(':');if(f.Length<6) return;Ŀ=f[0]=="1";Βό=κ(f[1]);Βϋ=κ(f[2]);Βύ=κ(f[3]);Βώ=κ(f[4]);long.TryParse(f[5],out Ǖ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ǡ p=new ǡ();if(!long.TryParse(e[0],out p.Θ)) continue;p.ά=κ(e[1]);p.б=κ(e[2]);p.Љ=κ(e[3]);Ъ.Add(p);}}const String ƿ="[PAMDCK]";const float ΙΡ=120f;class ē{public long Δ;public int ǧ;public long ǔ;public bool Ηδ;public DateTime ǥ,Ɲ;
}IMyBroadcastListener Βω=null;List<ē>Ы=new List<ē>();List<IMyShipConnector>Ĭ=new List<IMyShipConnector>();List<IMySensorBlock>Ȏ=new List<IMySensorBlock>();List<IMyCameraBlock>Ħ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Γη=new List<MyDetectedEntityInfo>();long і=0;int Βκ=0;
long Βι=0;DateTime Ηη=DateTime.Now;String Ğ="";int Ηώ=0;IMyCubeGrid ĭ=null;void ΚΨ(){ĭ=Me.CubeGrid;χ.GetBlocksOfType(Ĭ,q=>q.CubeGrid.IsSameConstructAs(ĭ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(Ȏ,q=>q.CubeGrid.IsSameConstructAs(ĭ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(Ħ,q=>q.CubeGrid.IsSameConstructAs(ĭ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ȏ.Count;i++){Ȏ[i].DetectSmallShips=true;Ȏ[i].DetectLargeShips=true;}for(int i=0;i<Ħ.Count;i++) Ħ[i].EnableRaycast=true;}IMyShipConnector Δε(long Θ){if(Θ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Θ) as IMyShipConnector;return c!=null&&ĭ!=null&&c.CubeGrid.IsSameConstructAs(ĭ)?c:null;}bool ΓА(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖЂ(IMyShipConnector c,long Δ){for(int i=0;i<Ħ.Count;i++)
{IMyCameraBlock ě=Ħ[i];if(!ě.IsWorking||Vector3D.Distance(ě.GetPosition(),c.GetPosition())>10) continue;Vector3D ț=ě.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ě.CanScan(ț)) continue;MyDetectedEntityInfo Ǔ=ě.Raycast(ț);if(!Ǔ.IsEmpty()&&Ǔ.EntityId!=ĭ.EntityId&&Ǔ.EntityId!=Δ&&Ǔ.Type!=MyDetectedEntityType.FloatingObject)
{Ğ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙЎ(){for(int s=0;s<Ȏ.Count;s++){if(!Ȏ[s].IsWorking) continue;Γη.Clear();Ȏ[s].DetectedEntities(Γη);for(int i=0;i<Γη.Count;i++){MyDetectedEntityInfo e=Γη[i];bool ΛΠ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΛΠ&&e.EntityId!=і&&e.Velocity.Length()>0.5f){Ğ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΚΘ(ē r){IMyShipConnector ǔ=Δε(r.ǔ);if(ΓА(ǔ)&&ΖЂ(ǔ,r.Δ)) return ǔ.EntityId;if(!r.Ηδ||ǔ==null) return 0;IMyShipConnector Њ=null;double ΑΚ=0;for(int i=0;i<Ĭ.Count;i++)
{IMyShipConnector c=Ĭ[i];if(!ΓА(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ǔ.GetPosition());if(Њ==null||d<ΑΚ){if(ΖЂ(c,r.Δ)){Њ=c;ΑΚ=d;}}}return Њ==null?0:Њ.EntityId;}void Ιύ(){ē ƨ=null;long У=0;for(int ċ=2;ċ>=1&&ƨ==null;ċ--){ē Ů=null;for(int i=0;i<Ы.Count;i++)
{ē r=Ы[i];if(r.ǧ!=ċ) continue;if(ċ==1){long c=ΚΘ(r);if(c==0) continue;if(Ů==null||r.ǥ<Ů.ǥ){Ů=r;У=c;}}else if(Ů==null||r.ǥ<Ů.ǥ) Ů=r;}ƨ=Ů;}if(ƨ==null){if(Ы.Count>0&&Ğ=="") Ğ="No free dock";return;}і=ƨ.Δ;Βκ=ƨ.ǧ;Βι=ƨ.ǧ==1?У:0;Ηη=DateTime.Now;
}void Κν(){if(ĭ==null) return;if(Βω==null) Βω=IGC.RegisterBroadcastListener(ƿ);int ǣ=30;while(Βω.HasPendingMessage&&ǣ-->0){String Η=Βω.AcceptMessage().Data as String;if(Η!=null) ΙϏ(Η);}ΚΟ();if(++Ηώ<3) return;Ηώ=0;IGC.SendBroadcastMessage(ƿ,ΙΟ());}void ΙϏ(String Η){try{String[]f=Η.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ы.Count-1;i>=0;i--) if(Ы[i].Δ==Δ) Ы.RemoveAt(i);if(і==Δ) і=0;}else if(f[0]=="R"&&f.Length>=5){long ǔ=long.Parse(f[3]);if(Δε(ǔ)==null) return;ē r=null;for(int i=0;i<Ы.Count;i++) if(Ы[i].Δ==Δ) r=Ы[i];if(r==null){r=new ē();r.Δ=Δ;r.ǥ=DateTime.Now;Ы.Add(r);}
int ǧ=int.Parse(f[2]);if(r.ǧ!=ǧ) r.ǥ=DateTime.Now;r.ǧ=ǧ;r.ǔ=ǔ;r.Ηδ=f[4]=="1";r.Ɲ=DateTime.Now;}}catch{}}void ΚΟ(){for(int i=Ы.Count-1;i>=0;i--)
if((DateTime.Now-Ы[i].Ɲ).TotalSeconds>3) Ы.RemoveAt(i);ē Βθ=null;for(int i=0;i<Ы.Count;i++) if(Ы[i].Δ==і) Βθ=Ы[i];if(і!=0&&Βθ==null) і=0;if(Βθ!=null&&(DateTime.Now-Ηη).TotalSeconds>ΙΡ){і=0;Βθ.ǥ=DateTime.Now;}Ğ="";if(і==0&&Ы.Count>0&&!ΙЎ()) Ιύ();}
String ΙΟ(){String δ="S;"+ĭ.EntityId+";"+і+";"+Βκ+";"+Βι+";";for(int i=0;i<Ы.Count;i++) δ+=(i>0?",":"")+Ы[i].Δ;δ+=";";for(int i=0;i<Ĭ.Count;i++){IMyShipConnector c=Ĭ[i];δ+=(i>0?":":"")+c.EntityId+","+ο(c.GetPosition())+","+ο(c.WorldMatrix.Forward)+","+ο(c.WorldMatrix.Up);
}δ+=";";for(int i=0;i<Ȏ.Count;i++){IMySensorBlock z=Ȏ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();δ+=(i>0?":":"")+ο(z.GetPosition())+","+Math.Round(r,1);}return δ;}String Ις()
{if(Ĭ.Count==0&&Ȏ.Count==0) return "";int ΑΫ=0;for(int i=0;i<Ĭ.Count;i++) if(ΓА(Ĭ[i])) ΑΫ++;return "Docks: "+ΑΫ+"/"+Ĭ.Count+" free"+(і!=0?(Βκ==1?" | docking":" | undocking"):"")+
(Ы.Count>1?" | "+(Ы.Count-1)+" waiting":"")+(Ğ!=""?"\n"+Ğ:"")+"\n";}IMyBroadcastListener ΓΫ=null;int Ą=0;DateTime Ηύ=DateTime.Now;DateTime Θύ=DateTime.MinValue;long Θώ=0;int ΘϏ=0;long ư=0;bool ΑЁ=false;int ΕΑ=0;List<Vector3>ΑЃ=new List<Vector3>();List<float>ΖΦ=new List<float>();
bool Βϊ=false;bool Βψ=false;Vector3 Ηό=new Vector3();bool ǌ(){return Ą!=0&&(DateTime.Now-Θύ).TotalSeconds<3;}bool Ę(int ǧ){return ǌ()&&Θώ==ћ&&ΘϏ==ǧ;}bool ΗΙ(){return ǌ()||(DateTime.Now-Ηύ).TotalSeconds<3;}void ΔΪ(int ǧ){if(Ą==ǧ) return;Ą=ǧ;Ηύ=DateTime.Now;ΕΑ=3;
}void ΔΩ(){if(Ą!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƿ,"D;"+ћ);Ą=0;ΑЁ=false;}void Κμ(){if(!trafficEnabled||Ξ==Ν.ί) return;if(ΓΫ==null) ΓΫ=IGC.RegisterBroadcastListener(ƿ);int ǣ=20;while(ΓΫ.HasPendingMessage&&ǣ-->0){String Η=ΓΫ.AcceptMessage().Data as String;if(Η==null||!Η.StartsWith("S;")) continue;
try{ΚΕ(Η);}catch{}}if(Ϋ!=Α.И){Βϊ=false;Βψ=false;}if(Ą==1&&о(MyShipConnectorStatus.Connected)!=null) ΔΩ();if(Ą==2&&!ΑЁ&&Ϋ!=Α.Ǻ&&
Vector3.Distance(β,ѓ().Ε)>θ*3) ΔΩ();if(Ą!=0&&++ΕΑ>=3){ΕΑ=0;IGC.SendBroadcastMessage(ƿ,"R;"+ћ+";"+Ą+";"+Ǖ+";"+(Ŀ?1:0));}}void ΚΕ(String Η){String[]f=Η.Split(';');if(f.Length<7) return;bool ΗЈ=false;String[]ΗΤ=f[5].Split(',');for(int i=0;i<ΗΤ.Length;i++) if(ΗΤ[i]==ћ.ToString()) ΗЈ=true;
if(!ΗЈ) return;Θύ=DateTime.Now;Θώ=long.Parse(f[2]);ΘϏ=int.Parse(f[3]);ư=long.Parse(f[4]);ΑЃ.Clear();ΖΦ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑЃ.Add(κ(e[0]));ΖΦ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗЀ=f[6].Split(':');List<ǡ>В=new List<ǡ>();for(int i=0;i<ΗЀ.Length;i++){String[]e=ΗЀ[i].Split(',');if(e.Length<4) continue;ǡ p=new ǡ();p.Θ=long.Parse(e[0]);p.ά=κ(e[1]);p.б=κ(e[2]);p.Љ=κ(e[3]);В.Add(p);}Ъ=В;}bool ΙΩ(){if(!trafficEnabled||Ξ==Ν.ί||Ǖ==0) return true;
ΔΪ(1);if(!ΗΙ()) return true;if(!Ę(1)){Μ="Waiting for dock clearance";return false;}Γλ();return true;}bool Γλ(){ΑΡ=false;ǡ ů=null;if(ư!=0&&ư!=Ǖ&&Ŀ)
for(int i=0;i<Ъ.Count;i++) if(Ъ[i].Θ==ư) ů=Ъ[i];long ƺ=ů==null?0:ů.Θ;if(ƺ==Č&&(ƺ==0)==(ǩ==null)) return false;Č=ƺ;ǩ=ů==null?null:Γω(ů.ά,ů.б,ů.Љ);return true;}bool ƃ(Vector3 p){for(int i=0;i<ΑЃ.Count;i++) if(Vector3.Distance(p,ΑЃ[i])<ΖΦ[i]+θ*0.75f) return true;
return false;}bool ΙЃ(Vector3 ΑΞ,Vector3 ŭ){Βϊ=false;if(!trafficEnabled||Ξ==Ν.ί||Ǖ==0) return false;if(Ą!=1&&Vector3.Distance(β,Ρ.Ε)>dockRequestDistance) return false;ΔΪ(1);if(!ǌ()||Ę(1)||ΑЃ.Count==0) return false;if(!ƃ(ΑΞ)&&!ƃ(ŭ)&&!ƃ(β)) return false;Βϊ=true;return true;
}bool Ζα(){if(!trafficEnabled||Ξ==Ν.ί||Ǖ==0) return true;ΔΪ(2);if(!ΗΙ()) return true;if(!Ę(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΙЏ=600f;class Ȯ{public long Θ;public int а=-1;public bool ƺ;public float Αά,ΑΨ;public DateTime ΕЀ;}class Ȑ{public String э;
public int w,h;public float ΖΙ,ΕЎ;public String φ;public List<int>ΓΚ=new List<int>();public List<Ȯ>ш=new List<Ȯ>();}List<Ȑ>ǖ=new List<Ȑ>();IMyBroadcastListener ΒЋ=null;int ΘΟ=0;void Κρ(){if(ΒЋ==null) ΒЋ=IGC.RegisterBroadcastListener(Δθ);int ǣ=50;while(ΒЋ.HasPendingMessage&&ǣ-->0)
{String Η=ΒЋ.AcceptMessage().Data as String;if(Η==null) continue;try{ΙЁ(Η);}catch{}try{ΙЂ(Η);}catch{}}for(int j=ǖ.Count-1;j>=0;j--){Ȑ Ι=ǖ[j];for(int i=Ι.ш.Count-1;i>=0;i--)
if((DateTime.Now-Ι.ш[i].ΕЀ).TotalSeconds>ΙЏ) Ι.ш.RemoveAt(i);if(Ι.ш.Count==0){ǖ.RemoveAt(j);continue;}ΙΛ(Ι);}if(++ΘΟ<3) return;ΘΟ=0;for(int j=0;j<ǖ.Count;j++) IGC.SendBroadcastMessage(ΖЃ,ΙΞ(ǖ[j]));}void ΙΛ(Ȑ Ι){for(int i=0;i<Ι.ш.Count;i++){Ȯ s=Ι.ш[i];if(s.ƺ&&(s.а<0||Ι.φ[s.а]=='1')) s.а=ΚΑ(Ι,s);
}}String ΙΞ(Ȑ Ι){String δ="J;"+Ι.э+";"+Ι.φ+";";bool ǥ=true;for(int i=0;i<Ι.ш.Count;i++){if((DateTime.Now-Ι.ш[i].ΕЀ).TotalSeconds>5) continue;δ+=(ǥ?"":",")+Ι.ш[i].Θ+":"+Ι.ш[i].а;ǥ=false;}return δ;}void ΙЁ(String Η){String[]f=Η.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;Ȑ Ι=null;for(int i=0;i<ǖ.Count;i++) if(ǖ[i].э==f[4]) Ι=ǖ[i];if(Ι==null){Ι=new Ȑ();Ι.э=f[4];Ι.w=int.Parse(k[3]);Ι.h=int.Parse(k[4]);Ι.ΖΙ=float.Parse(k[6]);Ι.ΕЎ=float.Parse(k[7]);if(Ι.w<1||Ι.h<1) return;ΖΪ(Ι.w,Ι.h,(ѕ) int.Parse(k[5]),Ι.ΓΚ);
Ι.φ=new String('0',Ι.w*Ι.h);ǖ.Add(Ι);}String d=f[6];if(d.Length==Ι.φ.Length){char[]c=Ι.φ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Ι.φ=new String(c);}long Θ=long.Parse(f[0]);Ȯ s=null;for(int i=0;i<Ι.ш.Count;i++) if(Ι.ш[i].Θ==Θ) s=Ι.ш[i];
if(s==null){s=new Ȯ();s.Θ=Θ;Ι.ш.Add(s);}s.ΕЀ=DateTime.Now;String[]w=f[10].Split(',');s.ƺ=w[0]=="1";if(w.Length>=3){s.Αά=float.Parse(w[1]);s.ΑΨ=float.Parse(w[2]);}int ġ=int.Parse(f[5]);if(ġ>=0&&ġ<Ι.φ.Length&&(s.а<0||Ι.φ[s.а]=='1')&&Ι.φ[ġ]!='1'&&ΓЋ(Ι,ġ,s)==null)
s.а=ġ;}Ȯ ΓЋ(Ȑ Ι,int а,Ȯ ΛΚ){for(int i=0;i<Ι.ш.Count;i++){Ȯ o=Ι.ш[i];if(o!=ΛΚ&&o.а==а)
return o;}return null;}int ΚΑ(Ȑ Ι,Ȯ s){for(int k=0;k<Ι.ΓΚ.Count;k++){int c=Ι.ΓΚ[k];if(Ι.φ[c]!='1'&&ΓЋ(Ι,c,s)==null) return c;}return-2;}String Ιχ(){String s="";for(int j=0;j<ǖ.Count;j++){Ȑ Ι=ǖ[j];int φ=0;for(int i=0;i<Ι.φ.Length;i++) if(Ι.φ[i]=='1') φ++;s+="Job "+(j+1)+": "+φ+"/"+Ι.φ.Length+" holes, "+Ι.ш.Count+" ships\n";
}return s;}const float Κα=8f;class Ч{public long Θ;public Vector3 ά,д,ȟ;public float Ɣ,Αά,ΑΨ,њ=-1;public int Ȃ;public int Ǜ;public String Ι="";public int Ű;public DateTime Ȧ;}class ΒΫ{public long a,b,ΖΥ;public DateTime Ɲ;}List<Ч>ς=new List<Ч>();List<ΒΫ>ѝ=new List<ΒΫ>();int ΕЍ=0;void ΙЂ(String Η)
{String[]f=Η.Split(';');if(f.Length<9) return;long Θ=long.Parse(f[0]);Ч s=null;for(int i=0;i<ς.Count;i++) if(ς[i].Θ==Θ) s=ς[i];if(s==null){s=new Ч();s.Θ=Θ;ς.Add(s);}s.ά=κ(f[1]);s.д=κ(f[2]);s.Ɣ=float.Parse(f[3]);s.Ι=f[4];s.Ű=int.Parse(f[7].Split(',')[1]);s.ȟ=κ(f[8]);
s.Ȃ=0;s.Ǜ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.Αά=float.Parse(w[1]);s.ΑΨ=float.Parse(w[2]);s.Ȃ=int.Parse(w[3]);if(w.Length>=5) s.Ǜ=int.Parse(w[4]);}}s.Ȧ=DateTime.Now;}bool ΑΑ(Ч s){return s.д.LengthSquared()<0.25f&&(s.Ű==3||s.ȟ.LengthSquared()<0.25f);}
Vector3 ΗΞ(Ч s){return s.д.LengthSquared()>=1f?s.д:s.ȟ*2f;}void Κϊ(){Ιί();if(++ΕЍ<3) return;ΕЍ=0;IGC.SendBroadcastMessage(ΗΝ,ΙΠ());}void Ιί(){for(int i=ς.Count-1;i>=0;i--)
if((DateTime.Now-ς[i].Ȧ).TotalSeconds>3) ς.RemoveAt(i);for(int i=ѝ.Count-1;i>=0;i--)
if((DateTime.Now-ѝ[i].Ɲ).TotalSeconds>2) ѝ.RemoveAt(i);for(int i=0;i<ς.Count;i++) ς[i].њ=-1;for(int i=0;i<ς.Count;i++)
for(int j=i+1;j<ς.Count;j++){Ч a=ς[i],b=ς[j];if(ΑΑ(a)||ΑΑ(b)) continue;Vector3 Λξ=a.ά+a.д*(float)(DateTime.Now-a.Ȧ).TotalSeconds;Vector3 Λς=b.ά+b.д*(float)(DateTime.Now-b.Ȧ).TotalSeconds;Vector3 Ƶ=ΗΞ(a),ƶ=ΗΞ(b);Vector3 ǫ=Λς-Λξ;float υ=ǫ.Length();float ж=(a.Ɣ+b.Ɣ)*0.55f;
bool Απ=a.Ι!=""&&a.Ι==b.Ι;if(Απ&&(a.Ȃ==2||b.Ȃ==2)) continue;if(Απ&&a.Ȃ==1&&b.Ȃ==1){String[]k=a.Ι.Split('|');if(k.Length>=8) ж=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Εώ=ƶ-Ƶ;float Θσ=Εώ.LengthSquared();float t=Θσ>0.01f?-Vector3.Dot(ǫ,Εώ)/Θσ:0;
bool ΖΔ=υ<ж;if(!ΖΔ&&(t<=0||t>Κα||(ǫ+Εώ*t).Length()>=ж)) continue;if(!ΖΔ&&a.Ǜ==b.Ǜ&&Ƶ.LengthSquared()>0.25f&&ƶ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ƶ),Vector3.Normalize(ƶ))<-0.7f)
continue;ΒΫ p=null;for(int k=0;k<ѝ.Count;k++)
if((ѝ[k].a==a.Θ&&ѝ[k].b==b.Θ)||(ѝ[k].a==b.Θ&&ѝ[k].b==a.Θ)) p=ѝ[k];if(p==null){p=new ΒΫ();p.a=a.Θ;p.b=b.Θ;p.ΖΥ=Κβ(a,b,Ƶ,ƶ,ǫ,υ);ѝ.Add(p);}p.Ɲ=DateTime.Now;Ч y=p.ΖΥ==a.Θ?a:b;float ΘΦ=ΖΔ||t<4?0:Math.Max(1,y.д.Length()*0.5f);if(y.њ<0||ΘΦ<y.њ) y.њ=ΘΦ;
}ΙЄ();}long Κβ(Ч a,Ч b,Vector3 Ƶ,Vector3 ƶ,Vector3 ǫ,float υ){if(a.Θ==і) return b.Θ;if(b.Θ==і) return a.Θ;if(a.Ǜ!=b.Ǜ) return a.Ǜ<b.Ǜ?a.Θ:b.Θ;Vector3 n=υ>0.01f?ǫ/υ:new Vector3(1,0,0);float ΘЁ=Vector3.Dot(Ƶ,n),ΘЌ=Vector3.Dot(ƶ,-n);if(ΘЁ-ΘЌ>1) return a.Θ;if(ΘЌ-ΘЁ>1) return b.Θ;
return a.Θ>b.Θ?a.Θ:b.Θ;}String ΙΠ(){String δ="C;";bool ǥ=true;for(int i=0;i<ς.Count;i++){if(ς[i].њ<0) continue;δ+=(ǥ?"":",")+ς[i].Θ+":"+Math.Round(ς[i].њ,1);ǥ=false;}return δ;}String Ιό(){int Θδ=0,Αΰ=0;for(int i=0;i<ς.Count;i++){if(!ΑΑ(ς[i])) Θδ++;if(ς[i].њ>=0) Αΰ++;
}if(ς.Count==0) return "";return "Traffic: "+Θδ+" moving"+(Αΰ>0?", "+Αΰ+" giving way":"")+"\n";}List<ϊ>Ȁ=new List<ϊ>();ϊ Ć=null;Vector3 ΘΖ;DateTime ΘΕ;float ΘΔ=40;bool ΘΓ=false;Ч ΓЀ(ϊ s){Ч Њ=null;for(int i=0;i<ς.Count;i++)
if(Vector3.Distance(ς[i].ά,s.Ε)<15&&(Њ==null||
Vector3.Distance(ς[i].ά,s.Ε)<Vector3.Distance(Њ.ά,s.Ε))) Њ=ς[i];return Њ;}float ΙЅ(ϊ s){Ч t=ΓЀ(s);if(t==null) return 0;float k=t.Ȃ*1000000f;String[]э=t.Ι.Split('|');if(э.Length>=3) k+=Vector3.Distance(t.ά,new Vector3(float.Parse(э[0]),float.Parse(э[1]),float.Parse(э[2])));return k;
}void Κί(){Ȁ.Clear();Ć=null;ΕΥ.Clear();List<float>ΕΪ=new List<float>();for(int i=0;i<Ї.Count;i++){float k=ΙЅ(Ї[i]);int ŝ=0;while(ŝ<ΕΪ.Count&&ΕΪ[ŝ]<=k) ŝ++;ΕΪ.Insert(ŝ,k);Ȁ.Insert(ŝ,Ї[i]);Ї[i].ķ("HOMEPOS",true,false,false);ΒΥ("HOMEPOS",Ї[i].Θ,"HOMEPOS");}}List<Ч>ΕΥ=new List<Ч>();
void ΙЄ(){ΕΥ.Clear();for(int i=0;i<Ȁ.Count;i++){Ч t=ΓЀ(Ȁ[i]);if(t==null) continue;t.њ=0;ΕΥ.Add(t);}}void Κφ(){if(Ć!=null){bool ΛΟ=ΘΓ||Vector3.Distance(Ć.Ε,ΘΖ)>ΘΔ||
(DateTime.Now-ΘΕ).TotalSeconds>45||Ć.ΔΒ();if(!ΛΟ) return;Ć=null;}if(Ȁ.Count==0) return;Ć=Ȁ[0];Ȁ.RemoveAt(0);ΘΖ=Ć.Ε;ΘΕ=DateTime.Now;Ч t=ΓЀ(Ć);ΘΔ=t==null?40:Math.Max(40,t.Ɣ*3);ΘΓ=t!=null&&t.Ű==3&&t.д.LengthSquared()<0.25f;}const String Ιε="[PAMDBG]";class Γφ
{public readonly bool ΔΙ;IMyProgrammableBlock ę;Action<IMyProgrammableBlock>Δο;Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>Δξ;Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>Δν;Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>Δπ;
Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>Δμ;public Γφ(MyGridProgram Λφ){ę=Λφ.Me;var Ŭ=ę.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string,Delegate>>()?.GetValue(ę);if(Ŭ==null) return;Δο=(Action<IMyProgrammableBlock>) Ŭ["RemoveDraw"];Δξ=(Func<IMyProgrammableBlock,Vector3D,Color,float,float,bool,int>) Ŭ["Point"];
Δν=(Func<IMyProgrammableBlock,Vector3D,Vector3D,Color,float,float,bool,int>) Ŭ["Line"];Δπ=(Func<IMyProgrammableBlock,BoundingSphereD,Color,int,float,int,float,bool,int>) Ŭ["Sphere"];Δμ=(Func<IMyProgrammableBlock,string,Vector3D,Color?,float,int>) Ŭ["GPS"];((Action<IMyProgrammableBlock>) Ŭ["RemoveAll"])(ę);
ΔΙ=true;}public void ΚΛ(){if(Δο!=null) Δο(ę);}public void Ķ(Vector3D a,Vector3D b,Color c,float ΛЎ){if(Δν!=null) Δν(ę,a,b,c,ΛЎ,-1,false);}public void ΚΙ(Vector3D a,Color c,float č){if(Δξ!=null) Δξ(ę,a,c,č,-1,false);}public void Ƅ(String Γ,Vector3D ŝ,Color c){if(Δμ!=null) Δμ(ę,Γ,ŝ,c,-1);}
public void ΗΘ(Vector3D ƒ,double č,Color c){if(Δπ!=null) Δπ(ę,new BoundingSphereD(ƒ,č),c,1,0.05f,30,-1,false);}}class ǀ{public long Θ;public Vector3 Αγ,Ũ,Ț,ε;public bool ΗЏ;public List<Vector3>ΓΝ=new List<Vector3>(),Εω=new List<Vector3>();public List<bool>Εψ=new List<bool>();
public DateTime Ȧ;}Γφ П=null;IMyBroadcastListener Βτ=null;List<ǀ>ă=new List<ǀ>();void Ιώ(String Η){String[]f=Η.Split(';');if(f.Length<6) return;long Θ=long.Parse(f[0]);ǀ s=null;for(int i=0;i<ă.Count;i++) if(ă[i].Θ==Θ) s=ă[i];if(s==null){s=new ǀ();s.Θ=Θ;ă.Add(s);}
s.Αγ=κ(f[1]);s.Ũ=κ(f[2]);s.Ț=κ(f[3]);s.ΗЏ=s.Ũ.LengthSquared()>0.5f;s.ε=κ(f[4]);s.Ȧ=DateTime.Now;if(f[5]=="") return;s.ΓΝ.Clear();s.Εω.Clear();s.Εψ.Clear();String[]Θς=f[5].Split(':');for(int i=0;i<Θς.Length;i++){String[]r=Θς[i].Split(',');if(r.Length<3) continue;s.ΓΝ.Add(κ(r[0]));s.Εω.Add(κ(r[1]));s.Εψ.Add(r[2]=="1");
}}void Κλ(){if(!debugDraw) return;if(П==null) П=new Γφ(this);if(!П.ΔΙ) return;if(Βτ==null) Βτ=IGC.RegisterBroadcastListener(Ιε);int ǣ=20;while(Βτ.HasPendingMessage&&ǣ-->0){String Η=Βτ.AcceptMessage().Data as String;if(Η!=null) try{Ιώ(Η);}catch{}}for(int i=ă.Count-1;i>=0;i--) if((DateTime.Now-ă[i].Ȧ).TotalSeconds>5) ă.RemoveAt(i);
П.ΚΛ();for(int i=0;i<ς.Count;i++){Ч s=ς[i];Color c=ΑΑ(s)?Color.Gray:s.њ>=0?Color.Yellow:Color.Lime;П.ΗΘ(s.ά,s.Ɣ/2,c);П.Ƅ(ΒΪ(s)+": "+(ΑΑ(s)?"parked":s.њ==0?"holding (giving way)":s.њ>0?"slowing (giving way)":"moving"),s.ά,c);if(s.д.LengthSquared()>0.25f) П.Ķ(s.ά,s.ά+s.д*3,Color.White,0.1f);
if(s.ȟ.LengthSquared()>0.25f) П.Ķ(s.ά,s.ά+s.ȟ*s.Ɣ,Color.Cyan,0.1f);}for(int i=0;i<ѝ.Count;i++){if((DateTime.Now-ѝ[i].Ɲ).TotalSeconds>0.5) continue;Ч a=null,b=null;for(int k=0;k<ς.Count;k++){if(ς[k].Θ==ѝ[i].a) a=ς[k];if(ς[k].Θ==ѝ[i].b) b=ς[k];}if(a==null||b==null) continue;
П.Ķ(a.ά,b.ά,Color.Red,0.2f);П.Ƅ("Conflict: "+ΒΪ(ѝ[i].ΖΥ==a.Θ?a:b)+" gives way",(a.ά+b.ά)/2,Color.Red);}for(int i=0;i<ă.Count;i++){ǀ d=ă[i];Ч t=null;for(int k=0;k<ς.Count;k++) if(ς[k].Θ==d.Θ) t=ς[k];if(t!=null) П.Ķ(t.ά,d.ε,Color.Blue,0.08f);for(int r=0;r<d.ΓΝ.Count;r++) П.Ķ(d.ΓΝ[r],d.Εω[r],d.Εψ[r]?Color.Red:Color.Green,0.04f);
if(t==null||!d.ΗЏ) continue;Ȑ Ι=null;for(int j=0;j<ǖ.Count;j++) if(ǖ[j].э==t.Ι) Ι=ǖ[j];if(Ι==null) continue;int ΗЂ=0;for(int c=0;c<Ι.φ.Length;c++) if(Ι.φ[c]=='1') ΗЂ++;if(i==0||ă[i-1].Αγ!=d.Αγ) П.Ƅ("Job: "+ΗЂ+"/"+Ι.φ.Length+" holes done",d.Αγ,Color.White);
for(int c=0;c<Ι.φ.Length;c++){int ΔЈ=c%Ι.w,ŉ=c/Ι.w;Vector3 p=d.Αγ+d.Ũ*((Ι.w-1f)/2f-ΔЈ)*Ι.ΖΙ-d.Ț*((Ι.h-1f)/2f-ŉ)*Ι.ΕЎ;Ȯ Εσ=ΓЋ(Ι,c,null);П.ΚΙ(p,Ι.φ[c]=='1'?Color.Gray:Εσ!=null?Color.Orange:Color.White,Math.Min(Ι.ΖΙ,Ι.ΕЎ)*0.2f);if(Εσ==null||Εσ.Θ!=d.Θ) continue;
П.Ķ(t.ά,p,Color.Orange,0.05f);П.Ƅ("Hole "+(Ι.ΓΚ.IndexOf(c)+1)+": "+ΒΪ(t),p,Color.Orange);}}for(int i=0;i<Ȏ.Count;i++){IMySensorBlock z=Ȏ[i];П.ΗΘ(z.GetPosition(),new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length(),Color.Cyan);
П.Ƅ("Docking area"+(Ğ!=""?": "+Ğ:""),z.GetPosition(),Color.Cyan);}for(int i=0;i<Ħ.Count;i++)
П.Ķ(Ħ[i].GetPosition(),Ħ[i].GetPosition()+Ħ[i].WorldMatrix.Forward*dockLaneLength,Color.Purple,0.05f);if(і!=0&&Βι!=0){IMyShipConnector c=Δε(Βι);for(int k=0;k<ς.Count;k++)
if(c!=null&&ς[k].Θ==і){П.Ķ(ς[k].ά,c.GetPosition(),Color.Lime,0.15f);П.Ƅ("Cleared to "+(Βκ==2?"undock: ":"dock: ")+ΒΪ(ς[k]),c.GetPosition(),Color.Lime);}}}String ΒΪ(Ч t){ϊ Њ=null;for(int i=0;i<Ї.Count;i++)
if(Vector3.Distance(Ї[i].Ε,t.ά)<30&&(Њ==null||
Vector3.Distance(Ї[i].Ε,t.ά)<Vector3.Distance(Њ.Ε,t.ά))) Њ=Ї[i];if(Њ!=null&&Њ.ΰ!="") return Њ.ΰ;String Θ=t.Θ.ToString();return "Ship "+Θ.Substring(Math.Max(0,Θ.Length-4));}String Ιπ(){if(!debugDraw) return "";if(П==null||!П.ΔΙ) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
return "\nDebug draw: on\n"+
"Ship sphere: green moving, yellow giving way, grey parked\n"+
"White: where it is in 3 s, cyan: where it wants to go\n"+
"Red line: collision course (label: who gives way)\n"+
"Blue: flight target; thin green/red: camera rays (clear/hit)\n"+
"Points: grey hole done, orange claimed, white free\n"+
"Cyan sphere: docking area, purple: lane cameras\n"+
"Thick green: ship cleared to its connector\n";}