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
public enum Ζ{Ǉ,Ů,џ,р,Ϊ}public enum Α{а,ȣ,ů,ȋ,ď,І,Я,
Ȣ,į,ǟ,Č,Ĉ,Ȟ,
Ű,ǻ,ǎ,ċ,ǉ,ư,
Βψ,ǈ,Ŧ,Ʊ,Ȧ}public enum Λ{з,ı,Ѕ,ΒΑ,Ƭ,ƹ,ƺ,ī,Ĭ}public enum п{None,ċ,а,Δέ}public enum Ω{Main,Δό,ŭ,Ŭ,Βτ,ƪ,
ΑЂ,Δν,Δξ,Δμ,ΗΡ,ΙΗ,
ΙΕ,ΙΖ,Βο,ΔЀ,ǂ}public enum в{ǀ,Ť,ΗΤ,Δπ,Ϊ}public enum ǣ{Ǆ,Ζθ}public enum е{ƿ,ΒΗ,ΔЂ,Ũ}public enum Ѐ{Βά,ΔΪ,ΑЍ,ΒΒ,Ʈ,ΒΤ,Ƶ}public enum ь{ΑЏ,ΑЇ,ΑЈ}public enum И{ũ,ΖЅ,ĉ}public enum ι{Βή,Ββ,Βγ,Δω,Δψ,
Βέ,ƽ,Ƽ,Βΰ,
Βα,Βί}public enum ή{Ǵ,Ȋ,ǳ,Ʃ,ΑЅ,ΑІ}public enum ĳ{Δβ,ΔΠ,ΘЍ}public enum с{ǃ,Δΰ,ΖА,ΒΫ}public enum ǵ{None,Βη,ΗΓ,ΖЏ}public enum σ{ŵ,ƨ,Ʒ,Ũ,ΒΩ,Ƹ,ΒΪ,ΗΩ,Βδ}public enum ΘА{Ǉ,ΙΑ,ΘБ,ΙΓ,ΘЎ,ΘЌ,ΙΒ}class Χ{public bool γ=false;public Vector3 Δ=new Vector3();
public Vector3 ΰ=new Vector3();public Vector3 α=new Vector3();public Vector3 ψ=new Vector3();public Vector3 μ=new Vector3();public Vector3 Ч=new Vector3();public float Ў=0;public float Ιη=0;public float[]ǝ=null;public Χ(){}public Χ(Χ δ){γ=δ.γ;Δ=δ.Δ;ΰ=δ.ΰ;α=δ.α;ψ=δ.ψ;μ=δ.μ;
Ч=δ.Ч;ǝ=δ.ǝ;}public Χ(Vector3 Δ,Vector3 α,Vector3 ΰ,Vector3 ψ,Vector3 μ){this.Δ=Δ;this.ΰ=ΰ;this.α=α;this.ψ=ψ;this.Ў=0;this.μ=μ;}public void Ζε(List<IMyThrust>ǌ,List<string>ȅ){ǝ=new float[ȅ.Count];for(int i=0;i<ǝ.Length;i++)
ǝ[i]=-1;for(int i=0;i<ǌ.Count;i++){string ы=Δκ(ǌ[i]);int Β=ȅ.IndexOf(ы);if(Β!=-1)
ǝ[Β]=Џ(ǌ[i].MaxEffectiveThrust,ǌ[i].MaxThrust);}}}class Βζ{public Χ Љ=null;public List<Vector3>Ŝ=new List<Vector3>();public float ƀ=0;public float ΔΓ=0;public float ĸ=0;public float ΓΙ=0;public Vector3 ΑΠ=new Vector3();}class Ĵ{public ι Ћ=ι.Βή;public float ǒ=0;public float ȭ=0;
public string Ǔ="";public string ǖ="";DateTime Εφ;public bool Γφ=false;private bool ΒЎ=false;public bool ΑЊ(bool Ȅ){if(Ȅ){ȭ=0;ΒЎ=false;return false;}ΒЎ=true;return ȭ>ǒ;}public void ΗΔ(){ΑЊ(true);Γφ=false;}public void Ηε(){if(ΒЎ)
if((DateTime.Now-Εφ).TotalSeconds>1){ȭ++;Εφ=DateTime.Now;}}public bool ΒΠ(){switch(Ћ){case ι.Ββ:return true;case ι.Βγ:return true;}return false;}}class э{public String φ="";public String ς="";public int Ǐ=0;public И Ŋ=И.ĉ;public э(String φ,String ς,int Ǐ,И Ŋ){this.φ=φ;this.ς=ς;
this.Ǐ=Ǐ;this.Ŋ=Ŋ;}}class Ζτ{public Ζτ(Vector3 η,float ύ){this.η=η;this.ύ=ύ;}public Vector3 η;public float ύ;}class Βξ{public DateTime ΘΔ;public DateTime ΕΚ;public String ν="";public String φ="";public String Ιι="";public String Μ="";public String Ιγ="";public String Ιί="";public String Γτ="";
public Vector3 Δ=new Vector3();public List<э>Ы=new List<э>();public Ζ Η=Ζ.Ǉ;public ǵ Ȩ;public float ō;public float Ľ;public int і;public int ΙΦ=0;public int Ιζ=0;public bool ΘЏ(){return(DateTime.Now-ΘΔ).TotalSeconds>10;}public bool ΖЈ(){if(Ȩ!=ǵ.Βη) return false;return(DateTime.Now-ΕΚ).TotalSeconds>=4;
}public Βξ(String ν){this.ν=ν;}public ǵ ΙΔ(String ė,bool Θζ,bool Ǒ,bool Ησ){if(ė==""&&!Ǒ) return ǵ.None;if(Ȩ==ǵ.Βη&&ΖЈ())
Ȩ=ǵ.ΖЏ;if(Θζ){Γτ=ė;Ȩ=ǵ.Βη;ΕΚ=DateTime.Now;return Ȩ;}else if(Ǒ){Γτ="";Ȩ=ǵ.None;}else if(Γτ==ė){if(Ησ) Ȩ=ǵ.ΗΓ;return Ȩ;}return ǵ.None;}}IMyGridTerminalSystem ϊ;Vector3 Αο=new Vector3();Vector3 β=new Vector3();Vector3 ΑΤ=new Vector3();Vector3 Ə=new Vector3();Vector3 ƃ=new Vector3();
DateTime Ευ=new DateTime();bool Ɔ=true;int ΓЋ=0;bool Ā=false;bool Αά=false;bool ƒ=true;bool Ζα=false;bool ņ=false;bool ΑΩ=false;float ș=0;float ĥ=0;int ă=0;int ơ=0;int Ας=0;int ΒЇ=0;float Ō=0;float Ĺ=0;double Γκ=0;float Ź=0;List<int>Ń=new List<int>();List<int>ŗ=new List<int>();
Ζ Η=Ζ.Ǉ;int ώ=0;float Ǳ=0;float ǰ=0;float θ=0;float ΓϏ=0,Γώ=0;IMyRemoteControl Υ;IMySensorBlock ξ;List<IMyTimerBlock>ΓЎ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȇ=new List<IMyThrust>();List<IMyGyro>ΑΥ=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȃ=new List<IMyLandingGear>();List<IMyReactor>Ă=new List<IMyReactor>();List<IMyConveyorSorter>Αυ=new List<IMyConveyorSorter>();List<IMyGasTank>ѓ=new List<IMyGasTank>();List<IMyTerminalBlock>Ŷ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ļ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȧ=new List<IMyBatteryBlock>();List<IMyTextPanel>є=new List<IMyTextPanel>();List<IMyTextSurface>Ą=new List<IMyTextSurface>();List<IMyTextPanel>ſ=new List<IMyTextPanel>();
IMyTerminalBlock ј=null;Χ Θ=new Χ();Χ ΓΧ=new Χ();Χ ΓΪ=new Χ();bool ȇ=false;bool Ş=false;bool ş=false;bool ΑΫ=false;List<String>ȅ=new List<string>();List<Χ>Σ=new List<Χ>();List<Χ>Ʀ=new List<Χ>();int ΓΒ=0;double Ζβ=0;Χ Ε=new Χ();Vector3 Ǚ;Vector3 Ȗ;Λ ί=Λ.з;ǣ ğ=ǣ.Ǆ;int τ=0;
int Н=0;double ƌ=0;bool ΑΗ=false;Ĵ ϋ=new Ĵ();Ĵ ό=new Ĵ();е л=е.ƿ;bool Ć=true;ǣ Ț=ǣ.Ǆ;int й=3;int и=3;int Ц=30;ь у=ь.ΑЏ;bool ȝ=true;bool Ņ=true;bool Ŀ=true;bool Ħ=false;Ѐ М=Ѐ.Βά;bool Ȝ=true;float ǫ=90;float ц=20;float Ь=5;float ч=20;float Ǥ=0.70f;float Ţ=1.50f;
float š=2.50f;float Ī=0f;float Ĝ=0f;Α Φ;Λ х;Α ƛ;Βζ ε=null;int Щ=0;int Ъ=0;int К=0;int Ȕ=0;int ģ=30;int Т=0;bool Ё=true;Vector3 ш;double Л=0;bool Αρ=false;int ΔΘ=0;int Ђ=0;int ΒϏ=0;int Ǭ=0;int ΕЈ=0;Vector3 Ɣ=new Vector3();float к=0;float Ю=0;int ĩ=0;Χ Αφ=null;
float Γέ=0;float ΑΒ=0;float Р=0;float ġ=0;bool ΓΨ=false;bool ħ=false;bool Ƣ=false;bool ȑ=false;bool ȍ=false;DateTime ΗЂ=new DateTime();Χ Ę=null;Χ ΕΜ=null;float Ǽ=0;п ǥ;float Ȓ=0;float Ĩ=0;String Γε="";float Αή=0;float Ľ=0;float ō=0;List<э>Ǖ=new List<э>();float О=0;
bool ƙ=false;bool ΖΧ=false;bool ΖΔ=false;bool Αω=false;bool ΖΕ=false;float Γθ=0;float Γσ=0;Vector3 Εΰ=new Vector3();Vector3 ω=new Vector3();Vector3 Αϊ=new Vector3(1,1,1);float ΓЊ=1;Vector3 ƞ=new Vector3();bool ΑΪ=false;bool ΓΟ=false;bool Γβ=false;bool ΔЋ=false;float ΑΕ=0;
float ǋ=2;Vector3 ΖΦ;Vector3 ΓЉ;Vector3 ΓЈ;float[,]Ǟ=new float[3,2];Dictionary<String,float[,]>ћ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζί=new Dictionary<String,float>();int ΒЂ=0;Dictionary<String,float[]>ȗ=new Dictionary<string,float[]>();float Γχ;bool ΑΘ=false;
int ΑΑ=0;int ȱ=0;int ΑΧ=0;int ĝ=0;String Μ="";int[]ΕЁ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΕЂ=false;int Αθ=0;int і=0;Ω Ќ=Ω.Main;int Θσ=0,Θτ=0,Θυ=0,Θφ=0;int Θβ=0,Θγ=0;bool Ļ=false;bool ΔЌ=false;String ΓЀ="";String ΒІ="";IMyBroadcastListener Ȏ=null;
bool Εί=true;List<Βξ>ΙΝ=null;Βξ Ιΰ=null;int ΙΞ=0,ΙΟ=0;int ΙΠ=0,ΙΡ=0;int ΙΣ=0,ΙΤ=0,ΙΥ=0;String ΘΓ="";Exception ȓ=null;bool Απ=false;Λ ř=Λ.з;bool ΕΟ=false;bool Ετ=true;bool Γό=false;String Ζΰ="";bool Ơ=false;bool ΓЅ=true;int ΑΨ=0;int[]Ιβ=null;Vector3 ΒЉ=new Vector3();
bool Εε=false;bool Ǎ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΡ(Me,controllerTag,true))
Η=Ζ.р;π(Ω.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с Γϋ=ΗΕ();if(Η==Ζ.Ǉ){List<IMyShipDrill>ΕΫ=new List<IMyShipDrill>();List<IMyShipGrinder>Εζ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(ΕΫ,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(Εζ,q=>q.CubeGrid==Me.CubeGrid);if(ΕΫ.Count>0)
{Η=Ζ.Ů;Μ="Miner mode enabled!";}else if(Εζ.Count>0){Η=Ζ.џ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϊ;ǹ(в.Ϊ);}}if(Γϋ==с.ǃ) ƒ=false;if(Γϋ==с.Δΰ) Μ="Data restore failed!";if(Γϋ==с.ΒΫ) Μ="New version, wipe data";}}void Main(string ѐ,UpdateType Θώ){try{if(ȓ!=null){if(ѐ.Trim().ToUpper()=="RETRY")
{ȓ=null;Ɔ=true;ă=0;if(Η!=Ζ.р) χ();Μ="Restarted after error";}else{ΖЁ();Δϊ();return;}}Αά=(Θώ&UpdateType.Update10)!=0;if(Αά) ΓЋ++;Ā=ΓЋ>=10;if(Ā) ΓЋ=0;if(Αά){ΑΑ++;if(ΑΑ>4) ΑΑ=0;Ας=(int) Math.Max(0,10-(DateTime.Now-Ευ).TotalSeconds);}if(ѐ!="") ΘΓ=ѐ;if(Η!=Ζ.р)
ΗΣ(ѐ);else
Ζς(ѐ);ΕЂ=false;try{int ΕΝ=Runtime.CurrentInstructionCount;float ƕ=Џ(ΕΝ,Runtime.MaxInstructionCount);if(ƕ>0.90) Μ="Max. instructions >90%";if(ƕ>Ō) Ō=ƕ;if(ΑΘ){Ń.Add(ΕΝ);while(Ń.Count>10) Ń.RemoveAt(0);Ĺ=0;for(int i=0;i<Ń.Count;i++) Ĺ+=Ń[i];Ĺ=Џ(Џ(Ĺ,Ń.Count),Runtime.MaxInstructionCount);
double Γή=Runtime.LastRunTimeMs;if(ΑΩ&&Γή>Γκ) Γκ=Γή;ŗ.Add((int)(Γή*1000f));while(ŗ.Count>10) ŗ.RemoveAt(0);Ź=0;for(int i=0;i<ŗ.Count;i++) Ź+=ŗ[i];Ź=Џ(Ź,ŗ.Count)/1000f;}}catch{Ō=0;}}catch(Exception e){ȓ=e;}}void ΗΣ(string ѐ){bool Ασ=false;String ΕΖ="";if(ώ<=1&&!Δτ(ѐ))
Δϋ(ѐ);int ΘΞ=50;while(Ȏ!=null&&Ȏ.HasPendingMessage&&ΘΞ-->0){MyIGCMessage ρ=Ȏ.AcceptMessage();String ά=ρ.Data as String;if(ά==null) continue;String Ƙ="";if(ώ<=1&&ΖБ(ref ά,out Ƙ,out ΕΖ)&&Ƙ==BROADCAST_SELF_ID){Δϋ(ά);Ασ=true;break;}}bool Επ=ΑΩ&&Φ==Α.а&&!ņ&&!Ασ&&ă==0&&!ΑΫ;
if(Ā&&Φ!=Α.а) Ασ=true;if((Αά&&!Επ)||(Ā&&Επ)){if(ă==0&&(Ας<=0||Ɔ)){Ζα=ώ>0;ώ=0;ă=1;Ǻ();ΗΘ();ΖІ();Ƕ("Scan 1");}else if(ă==1){ă=2;Ǻ();ΗΙ();Ƕ("Scan 2");}else if(ă==2){ă=0;Ǻ();ΗΚ();ΗΛ();if(ώ<=1) Δυ();Ƕ("Scan 3");Ευ=DateTime.Now;if(ώ<=1&&ƒ)
Αο=Ƴ(Υ,Υ.CenterOfMass);ƒ=false;if(Ɔ){Ɔ=false;bool ΘЂ=ί==Λ.Ѕ;χ();if(Ετ&&resumeAfterRestart&&ΘЂ&&ώ<=1) Γό=true;}Ετ=false;if(Ζα&&ώ==0) Μ="Setup complete";}else{if(ί==Λ.Ѕ&&Η!=Ζ.Ϊ){Ǻ();Ζγ();Ƕ("Inv balance");}Ǻ();switch(ơ){case 0:Ηέ();break;case 1:Ηα();break;case 2:Ηά();break;
case 3:Ηθ();break;case 4:Ηΰ();break;case 5:Ζι();break;case 6:Ζη(Υ);break;}Ƕ("Update: "+ơ);ơ++;if(ơ>6){ơ=0;ΑΩ=true;if(ř!=Λ.з){switch(ř){case Λ.ī:Ƞ();break;case Λ.Ĭ:Ƞ();break;case Λ.Ѕ:Ƞ();break;case Λ.ƹ:Ʋ();break;case Λ.ƺ:č();break;}ř=Λ.з;}}}if(!Ɔ){if(!Ǹ(Υ,true)){Υ=null;
ƒ=true;ώ=2;}if(ώ>=2&&Φ!=Α.а)
χ();if(ώ<=1){ĥ=Υ.CalculateShipMass().PhysicalMass;ș=(float) Υ.GetShipSpeed();β=ΗΫ(Υ,Αο);ΑΤ=Υ.WorldMatrix.Forward;Ə=Υ.WorldMatrix.Left;ƃ=Υ.WorldMatrix.Down;Ηδ();Ηζ();Ηή();if(Γό){Γό=false;Ƞ();if(Φ!=Α.а) Μ="Job resumed after restart";}if(Φ!=Α.а){ņ=false;ΔϏ(false);ΔΥ(false);String Θμ=ΒΜ(Φ)+" "+(int) Φ;
Ǻ();Ηβ();Βχ(false);Ƕ(Θμ);Ηη();Ηγ();Ǻ();Ηί();Ƕ("Thruster");Ǻ();ΔЇ();Ƕ("Gyroscope");}else{if(ņ){if(ƶ()){Ф(ƃ,ΑΤ,Ə,0.25f,true);ΔЇ();Μ="Aligning to planet: "+Math.Round(ΑΕ-0.25f,2)+"°";if(ΑΪ) ΒΟ(true,true);}else ΒΟ(true,true);}}Ơ=false;}}}Ǻ();Δϊ();Ƕ("Print");if(Ασ||ΒЇ<=0){Ǻ();Ζδ(ΕΖ);
Ƕ("Broadcast");ΒЇ=4;}else if(Ā) ΒЇ--;}void Δϋ(string ѐ){if(ѐ=="") return;var Д=ѐ.ToUpper().Split(' ');Д.DefaultIfEmpty("");var ė=Д.ElementAtOrDefault(0);var ĕ=Д.ElementAtOrDefault(1);var Ÿ=Д.ElementAtOrDefault(2);var ΔЏ=Д.ElementAtOrDefault(3);String Ȳ="Invalid argument: "+ѐ;bool ΔΒ=false;
switch(ė){case "UP":this.Δχ(false);break;case "DOWN":this.Δφ(false);break;case "UPLOOP":this.Δχ(true);break;case "DOWNLOOP":this.Δφ(true);break;case "APPLY":this.ΔΣ(true);break;case "MRES":і=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.Βσ();}break;case "PATH":{this.χ();this.Βσ();Θ.γ=true;}break;
case "START":{this.χ();Βς();}break;case "ALIGN":{ΒΟ(!ņ,false);}break;case "CONT":{this.χ();this.Ƞ();}break;case "JOBPOS":{this.χ();this.č();}break;case "HOMEPOS":{this.χ();this.Ʋ();}break;case "FULL":{ƙ=true;}break;case "RESET":{Απ=true;ώ=2;}break;case "EXPORT":{Ζω();}break;
case "IMPORT":{ΖЃ(ĕ=="JOB");}break;default:ΔΒ=true;break;}if(Η!=Ζ.Ϊ){switch(ė){case "SHUTTLE":{Ζχ();}break;case "CFGS":{if(!Ζπ(ĕ,Ÿ,ΔЏ)) Μ=Ȳ;}break;case "CFGB":{if(!Ζο(ĕ,Ÿ)) Μ=Ȳ;}break;case "CFGL":{if(!Ď(ref ǫ,true,σ.Ʒ,ĕ,"")||!Ζρ(Ÿ))
Μ=Ȳ;}break;case "CFGE":{if(!Ď(ref Ь,true,σ.Ƹ,ĕ,"IG")||
!Ď(ref ц,true,σ.ΒΩ,Ÿ,"IG")||
!Ď(ref ч,true,σ.ΒΪ,ΔЏ,"IG"))
Μ=Ȳ;}break;case "CFGA":{if(!Ď(ref Ǥ,false,σ.ƨ,ĕ,"")) Μ=Ȳ;}break;case "CFGW":{if(!Ď(ref Ţ,false,σ.ŵ,ĕ,"")||
!Ď(ref š,false,σ.ŵ,Ÿ,""))
Μ=Ȳ;}break;case "NEXT":{ΑЁ(false);}break;case "PREV":{ΑЁ(true);}break;default:if(ΔΒ) Μ=Ȳ;break;}}else{switch(ė){case "UNDOCK":{Ơ=true;}break;default:if(ΔΒ) Μ=Ȳ;break;}}}String Ζύ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Η!=Ζ.Ϊ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ζχ(){χ();Η=Ζ.Ϊ;ǹ(в.Ϊ);Ε.γ=false;Θ.γ=false;ξ=null;ο.Clear();ί=Λ.з;}void ΒΟ(bool Ї,bool ђ){if(!Ї) Μ="Aligning canceled";if(ђ) Μ="Aligning done";if(ђ||!Ї){ņ=false;ΔЃ();ǁ(false,0,0,0,0);return;}if(ƶ()) ņ=true;}bool Ζπ(String ΘЄ,String ΗЌ,String Ηύ)
{bool ΘЀ=ί==Λ.Ѕ;int w,h,d;if(int.TryParse(ΘЄ,out w)&&int.TryParse(ΗЌ,out h)&&int.TryParse(Ηύ,out d)){this.χ();й=w;и=h;Ц=d;Ē(false);Ŵ(false,false);if(ΘЀ) Ƞ();return true;}return false;}bool Ζρ(String ΔЎ){if(ΔЎ=="ON"){ȝ=true;return true;}if(ΔЎ=="OFF"){ȝ=false;return true;}
return false;}bool Ζο(String ΕΪ,String ΑΖ){bool Ɠ=true;if(ΕΪ=="HOME") Ć=true;else if(ΕΪ=="STOP") Ć=false;else Ɠ=false;if(ΑΖ=="HOME") л=е.ƿ;else if(ΑΖ=="STOP") л=е.ΔЂ;else if(ΑΖ=="JOB") л=е.ΒΗ;else if(ΑΖ=="IG") л=е.Ũ;else Ɠ=false;return Ɠ;}void π(Ω Γρ){ΕЁ[(int) Ќ]=і;
і=ΕЁ[(int) Γρ];if(Γρ==Ω.ΗΡ) і=0;Ќ=Γρ;if(Η!=Ζ.р)
đ(Ќ==Ω.ŭ,false,0,0);ΕЂ=true;}void Δχ(bool Γγ){if(і>0) і--;else if(Γγ) і=Αθ-1;}void Δφ(bool Γγ){if(і<Αθ-1) і++;else if(Γγ) і=0;}String ǹ(в ρ){switch(ρ){case в.ǀ:Μ="Job is running";break;case в.Ť:Μ="Connector not ready!";break;case в.ΗΤ:Μ="Ship modified, path outdated!";break;
case в.Δπ:Μ="Interrupted by player!";break;case в.Ϊ:Μ="Shuttle mode enabled!";break;}return "";}String ΖЀ(ǣ η){switch(η){case ǣ.Ǆ:return "Top-Left";case ǣ.Ζθ:return "Center";default:return "";}}String Ζϋ(ь Ƒ){switch(Ƒ){case ь.ΑЇ:return "Auto"+(Η==Ζ.Ů?" (Ore)":"");case ь.ΑЈ:return "Auto (+Stone)";
case ь.ΑЏ:return "Default";default:return "";}}String ΒΚ(Λ У){switch(У){case Λ.з:return "No job";case Λ.ı:return "Job paused";case Λ.Ѕ:return "Job active";case Λ.ī:return "Job active";case Λ.Ĭ:return "Job active";case Λ.ΒΑ:return "Job done";case Λ.Ƭ:return "Job changed";case Λ.ƹ:return "Move home";
case Λ.ƺ:return "Move to job";}return "";}String Δε(е Ης){switch(Ης){case е.ƿ:return "Return home";case е.ΒΗ:return "Fly to job pos";case е.ΔЂ:return "Stop";case е.Ũ:return "Ignore";}return "";}String Ζό(Ѐ Ƒ){switch(Ƒ){case Ѐ.Βά:return "Off";case Ѐ.ΒΒ:return "Drop pos (Stone) ";case Ѐ.Ʈ:return "Drop pos (Sto.+Ice)";
case Ѐ.ΔΪ:return "Cur. pos (Stone)";case Ѐ.ΑЍ:return "Cur. pos (Sto.+Ice)";case Ѐ.ΒΤ:return "In motion (Stone)";case Ѐ.Ƶ:return "In motion (Sto.+Ice)";}return "";}String Δγ(п У){switch(У){case п.None:return "No batteries";case п.ċ:return "Charging";case п.Δέ:return "Discharging";}return "";}String ΒΜ(Α У)
{String Ιά=Η==Ζ.Ϊ?"target":"job";switch(У){case Α.а:return "Idle";case Α.ȣ:return "Flying to XY position";case Α.ů:return Η==Ζ.џ?"Grinding":"Mining";case Α.ȋ:return "Returning";case Α.Ŧ:return "Flying to drop pos";case Α.ď:return "Returning to dock";case Α.І:return "Flying to dock area";
case Α.Я:return "Flying to job area";case Α.Ȣ:return "Flying to path";case Α.į:return "Flying to job position";case Α.ǟ:return "Approaching dock";case Α.Č:return "Docking";case Α.Ĉ:return "Aligning";case Α.Ȟ:return "Aligning";case Α.Ű:return "Retry docking";case Α.ǻ:return "Unloading";case Α.Ȧ:return Ζΰ;
case Α.ǎ:return "Undocking";case Α.ċ:return "Charging batteries";case Α.ǉ:return "Waiting for uranium";case Α.ư:return "Filling up hydrogen";case Α.Βψ:return "Waiting for ejection";case Α.ǈ:return "Waiting for ejection";case Α.Ʊ:return "Flying to drop pos";}return "";}String Δλ(ι Ћ){switch(Ћ)
{case ι.Βή:return "On \"Undock\" command";case ι.Ββ:return "On player entered cockpit";case ι.Δω:return "On ship is full";case ι.Δψ:return "On ship is empty";case ι.Βγ:return "On time delay";case ι.ƽ:return "On batteries empty(<25%)";case ι.Ƽ:return "On batteries empty(=0%)";case ι.Βέ:return "On batteries full";
case ι.Βα:return "On hydrogen empty(<25%)";case ι.Βί:return "On hydrogen empty(=0%)";case ι.Βΰ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΕБ,int Ȱ,bool Ξ,String Ǝ){Αθ+=1;if(ΕБ==Ȱ) Ǝ=">"+Ǝ+(ΑΑ>=2?" .":"");else Ǝ=" "+Ǝ;Γ+=Ǝ+"\n";return ΕБ==Ȱ&&Ξ;}String ΔΣ(bool Ξ)
{int Β=0;int Ο=і;Αθ=0;String ΖΓ="———————————————\n";String ŝ="--------------------------------------------\n";String Γ="";Γ+=ΒΚ(ί)+" | "+(Θ.γ?"Ready to dock":"No dock")+"\n";Γ+=ΖΓ;double ǭ=Math.Max(Math.Round(this.Л),0);if(Ќ==Ω.Main){bool ȳ=Η==Ζ.Ϊ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βσ();
if(Η==Ζ.Ů)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ω.ŭ);if(Η==Ζ.џ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ω.ŭ);if(Η==Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ω.Βο);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ƞ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) Ʋ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) č();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(ȳ) π(Ω.ǂ);else π(Ω.ƪ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ω.Δν);if(Η!=Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ω.Δμ);}else if(Ќ==Ω.ŭ){double ΘЃ=Math.Round(й*Ǳ,1);double ΗЋ=Math.Round(и*ǰ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) Βς();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){Ŵ(false,false);π(Ω.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΘЃ+"m)")){ĭ(ref й,5,20,1);Ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){ĭ(ref й,-5,20,-1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΗЋ+"m)")){ĭ(ref и,5,20,1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){ĭ(ref и,-5,20,-1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(у==ь.ΑЏ?"Depth":"Min")+": "+Ц+"m)")){ĭ(ref Ц,5,50,2);Ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){ĭ(ref Ц,-5,50,-2);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+Ζϋ(у))){у=ȡ(у);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΖЀ(Ț))){Ț=ȡ(Ț);}if(Η==Ζ.џ&&у==ь.ΑЈ) у=ȡ(у);Γ+=Δύ(8,Ѓ,Ο,ref Θβ);}else if(Ќ==Ω.Βο){float[]ΕΡ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.ΔЀ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δλ(ϋ.Ћ))) ϋ.Ћ=ȡ(ϋ.Ћ);if(!ϋ.ΒΠ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΙ((int) ϋ.ǒ))) ϋ.ǒ=ΔΫ(ϋ.ǒ,ΕΡ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δλ(ό.Ћ))) ό.Ћ=ȡ(ό.Ћ);
if(!ό.ΒΠ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΙ((int) ό.ǒ))) ό.ǒ=ΔΫ(ό.ǒ,ΕΡ);}else if(Ќ==Ω.ΔЀ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βς();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Βο);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǔ!=""?ϋ.Ǔ:"-"))) ϋ.Ǔ=ƻ(ref Θσ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǖ!=""?ϋ.ǖ:"-"))) ϋ.ǖ=ƻ(ref Θυ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǔ!=""?ό.Ǔ:"-"))) ό.Ǔ=ƻ(ref Θτ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǖ!=""?ό.ǖ:"-"))) ό.ǖ=ƻ(ref Θφ);
}else if(Ќ==Ω.Ŭ){String Εϊ=ǫ+" %";if(Ā) ȱ++;if(ȱ>1){ȱ=0;ΑΧ++;if(ΑΧ>1) ΑΧ=0;}bool[]ΓЂ=new bool[]{Ă.Count==0,ǥ==п.None,ѓ.Count==0};int Ζά=0;while(true){Ζά++;ĝ++;if(ĝ>ΓЂ.Length-1) ĝ=0;if(Ζά>=ΓЂ.Length) break;if(!ΓЂ[ĝ]) break;}bool ȳ=Η==Ζ.Ϊ;if(!ȳ&&ȝ&&О!=-1&&ΑΧ==0)
Εϊ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!ȳ) π(Ω.ƪ);else π(Ω.ǂ);if(!ȳ){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЁ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ơ=true;Γ+=ŝ;if(!ȳ) Γ+="Progress: "+Math.Round(ƌ,1)+" %\n";Γ+="State: "+ΒΜ(Φ)+" "+ǭ+"m \n";Γ+="Load: "+Αή+" % Max: "+Εϊ+" \n";if(ĝ==0) Γ+="Uranium: "+(Ă.Count==0?"No reactors":Math.Round(Ĩ,1)+"Kg "+(Ь==-1?"":" Min: "+Ь+" Kg"))+"\n";
if(ĝ==1) Γ+="Battery: "+(ǥ==п.None?Δγ(ǥ):Ǽ+"% "+(ц==-1||ȳ?"":" Min: "+ц+" %"))+"\n";if(ĝ==2) Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(Ȓ,1)+"% "+(ч==-1||ȳ?"":" Min: "+ч+" %"))+"\n";}else if(Ќ==Ω.ƪ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ί==Λ.Ѕ) π(Ω.Ŭ);else π(Ω.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+ǫ+"%")) д(ref ǫ,ǫ<=80?-10:-5,σ.Ʒ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(ȝ?"On":"Off"))) ȝ=!ȝ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+Ζό(М))) М=ȡ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(Ħ?"On":"Off"))){Ħ=!Ħ;if(Ħ) Ǣ(ΓЅ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(Ȝ?"On":"Off"))) Ȝ=!Ȝ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ь==-1?"Ignore":"Min "+Ь+"Kg"))) д(ref Ь,(Ь>5?-5:-1),σ.Ƹ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ц==-1?"Ignore":"Min "+ц+"%"))) д(ref ц,-5,σ.ΒΩ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(ч==-1?"Ignore":"Min "+ч+"%"))) д(ref ч,-10,σ.ΒΪ,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(Ć?"Return home":"Stop"))) Ć=!Ć;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δε(л))) л=ȡ(л);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ω.ΑЂ);Γ+=Δύ(8,Ѓ,Ο,ref Θγ);}else if(Ќ==Ω.ΑЂ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.Ѕ) π(Ω.Ŭ);else π(Ω.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.џ?" Grinder":" Drill")+" inv. balancing: "+(Ņ?"On":"Off"))) Ņ=!Ņ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.џ?" grinders":" drills")+": "+(Ŀ?"Fwd + Bwd":"Fwd"))) Ŀ=!Ŀ;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ţ+"m/s")) д(ref Ţ,0.5f,σ.ŵ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+š+"m/s")) д(ref š,0.5f,σ.ŵ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) д(ref Ǥ,0.1f,σ.ƨ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ī*100f+"%")) ΔΧ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+Ĝ*100f+"%")) ΔΧ(false,0.05f);
}else if(Ќ==Ω.ǂ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.Ѕ) π(Ω.Ŭ);else π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǫ+"%")) д(ref ǫ,ǫ<=80?-10:-5,σ.Ʒ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȝ?"On":"Off"))) Ȝ=!Ȝ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ь==-1?"Ignore":"Min "+Ь+"Kg"))) д(ref Ь,(Ь>5?-5:-1),σ.Ƹ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ц==-1?"Ignore":"Charge up"))) ц=(ц==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ч==-1?"Ignore":"Fill up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δε(л))) л=ȡ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) д(ref Ǥ,0.1f,σ.ƨ,false);
}else if(Ќ==Ω.Δό){double ΘΕ=0;if(Σ.Count>0) ΘΕ=Vector3.Distance(Σ.Last().Δ,β);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΔЄ();if(Η!=Ζ.Ϊ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȇ?"Use old home":(Θ.γ?"Was set! ":"none ")))) ȇ=!ȇ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȇ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) ȇ=!ȇ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ş?"Use old connector":(Ε.γ?"Was set! ":"none ")))) Ş=!Ş;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ş?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ş=!ş;Γ+=ŝ;Γ+="Wp spacing: "+Math.Round(Ζβ)+"m\n";}else if(Ќ==Ω.Βτ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();π(Ω.Main);}
Γ+=ŝ;Γ+="State: "+ΒΜ(Φ)+" \n";Γ+="Speed: "+Math.Round(ș,1)+"m/s\n";Γ+="Target dist: "+ǭ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΘ+"\n";}else if(Ќ==Ω.Δν){List<IMyTerminalBlock>ę=Δζ();if(Ā) ȱ++;if(ȱ>=ę.Count) ȱ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.Δξ);
Γ+=ŝ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αή,1)+"% "+Math.Round(Ľ,1)+" / "+Math.Round(ō,1)+"\n";Γ+="Uranium: "+(Ă.Count==0?"No reactors":Math.Round(Ĩ,1)+"Kg "+Γε)+"\n";Γ+="Battery: "+(ǥ==п.None?"":Ǽ+"% ")+Δγ(ǥ)+"\n";Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(Ȓ,1)+"% ")+"\n";
Γ+="Damage: "+(ę.Count==0?"None":""+(ȱ+1)+"/"+ę.Count+" "+ę[ȱ].CustomName)+"\n";}else if(Ќ==Ω.Δξ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=ŝ;Γ+="Next scan: "+Ας+"s\n";Γ+="Drill area: "+Math.Round(Ǳ,1)+"x"+Math.Round(ǰ,1)+"m Ship: "+Math.Round(ΓϏ,1)+"x"+Math.Round(Γώ,1)+"m ("+Math.Round(θ,1)+"m)\n";
Γ+="Broadcast: "+(Ļ?"Online - "+ΓЀ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ō*100f,1)+"% \n";}else if(Ќ==Ω.Δμ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=ŝ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+Ας+"s\n";if(Απ) Γ="Recompile script now";int ΘΖ=Γ.Split('\n').Length;for(int i=ΘΖ;i<=10;i++) Γ+="\n";Γ+=ΖΓ;Γ+="Last: "+Μ+"\n";
return Γ;}void ΔΧ(bool ΘΑ,float њ){χ();Ŵ(true,false);if(ΘΑ) д(ref Ī,њ,σ.Βδ,false);else д(ref Ĝ,њ,σ.Βδ,false);ΔΦ();đ(true,true,0,0);}float ΔΫ(float ζ,float[]ΑЀ){float έ=ΑЀ[0];for(int i=ΑЀ.Length-1;i>=0;i--)
if(ζ<ΑЀ[i]) έ=ΑЀ[i];return έ;}String ƻ(ref int Ȱ){String έ="";if(Ȱ>=ΓЎ.Count) Ȱ=-1;if(Ȱ>=0) έ=ΓЎ[Ȱ].CustomName;Ȱ++;return έ;}void ǅ(string φ){if(ί!=Λ.Ѕ) return;if(φ=="") return;IMyTerminalBlock Τ=ϊ.GetBlockWithName(φ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void ĭ(ref int value,int Ż,int ț,int ΓЃ){if(Ż==0) return;if(value<ț&&ΓЃ>0||value<=ț&&ΓЃ<0){value+=ΓЃ;return;}int Ηκ=Math.Abs(Ż);int Βω=0;int Γμ=1;while(true){Βω+=Γμ*Ηκ*10;if(Ż<0&&value-ț<=Βω) break;if(Ż>0&&value-ț<Βω) break;
Γμ++;}value+=Γμ*Ż;}void Ē(bool Ιθ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);đ(Ќ==Ω.ŭ,false,0,0);}bool Ď(ref float value,bool Θί,σ ς,String Ğ,String ΗЏ){if(Ğ=="") return false;float Œ=-1;bool Εν=false;if(Ğ.ToUpper()==ΗЏ) Εν=true;else if(!float.TryParse(Ğ,out Œ)) return false;
else Œ=Math.Max(0,Œ);if(Θί) Œ=(float) Math.Round(Œ);д(ref value,Œ,ς,Εν,false);return true;}void д(ref float value,float њ,σ ς,bool Ɵ){д(ref value,value+њ,ς,Ɵ,true);}void д(ref float value,float Ŏ,σ ς,bool Ɵ,bool ΘЇ){float ǘ=0,Ǘ=0;if(ς==σ.ŵ){ǘ=0.5f;Ǘ=10f;}if(ς==σ.ƨ){ǘ=0.1f;Ǘ=1f;}
if(ς==σ.Ũ){ǘ=50f;Ǘ=100f;}if(ς==σ.ΒΩ){ǘ=5f;Ǘ=30f;}if(ς==σ.Ƹ){ǘ=1f;Ǘ=25f;}if(ς==σ.Ʒ){ǘ=10f;Ǘ=95f;}if(ς==σ.ΒΪ){ǘ=10f;Ǘ=90f;}if(ς==σ.ΗΩ){ǘ=10f;Ǘ=1800;}if(ς==σ.Βδ){ǘ=0.0f;Ǘ=0.75f;}if(Ŏ==-1&&Ɵ){value=-1;return;}if(value==-1) Ɵ=false;
bool ΕЊ=Ŏ<ǘ||Ŏ>Ǘ;if(ΕЊ&&ΘЇ){if(Ŏ<value) value=Ǘ;else if(Ŏ>value) value=ǘ;}else value=Ŏ;if(ΕЊ&&Ɵ) value=-1;else value=Math.Max(ǘ,Math.Min(value,Ǘ));value=(float) Math.Round(value,2);}void ΑЁ(bool ΘΪ){if(ΘΪ) К=Math.Max(0,К-1);else К++;ΒΔ();ΒΓ();for(int k=0;k<Ǫ.Count&&k<Ш.Length;k++) Βμ(Ǫ[k],k<К);
П=-1;Βχ(true);}T ȡ<T>(T ζ){int Β=Array.IndexOf(Enum.GetValues(ζ.GetType()),ζ);Β++;if(Β>=Ζψ(ζ)) Β=0;return(T) Enum.GetValues(ζ.GetType()).GetValue(Β);}int Ζψ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βσ(){Ʀ.Clear();for(int i=0;i<Σ.Count;i++) Ʀ.Add(Σ[i]);Σ.Clear();
ΑΫ=true;ΓΧ=new Χ(Θ);ΓΪ=new Χ(Ε);Θ.γ=false;if(Η==Ζ.Ϊ) Ε.γ=false;for(int i=0;i<ћ.Count;i++)
if(!ȅ.Contains(ћ.Keys.ElementAt(i)))
ȅ.Add(ћ.Keys.ElementAt(i));ȇ=false;Ş=false;ş=false;π(Ω.Δό);}void ΔЄ(){if(ȇ) Θ=ΓΧ;if(Ş) Ε=ΓΪ;if(ş){Σ.Clear();for(int i=0;i<Ʀ.Count;i++) Σ.Add(Ʀ[i]);}ΑΫ=false;χ();π(Ω.Main);}void Ηδ(){if(!ΑΫ) return;if(Φ!=Α.а){ΔЄ();return;}if(!ΓΧ.γ) ȇ=false;if(!ΓΪ.γ) Ş=false;if(Ʀ.Count<=1) ş=false;
IMyShipConnector Й=З(MyShipConnectorStatus.Connectable);if(Й==null) Й=З(MyShipConnectorStatus.Connected);if(Й!=null){if(Math.Round(ș,2)<=0.20) ΓΒ++;else ΓΒ=0;if(ΓΒ>=5){if(Η==Ζ.Ϊ&&(Θ.γ||ȇ)&&Vector3.Distance(Θ.Δ,Й.GetPosition())>5){Ε.α=Υ.WorldMatrix.Forward;Ε.ψ=Υ.WorldMatrix.Left;
Ε.ΰ=Υ.WorldMatrix.Down;Ε.μ=Υ.GetNaturalGravity();Ε.Δ=Й.GetPosition();Ε.γ=true;Ε.Ч=Й.Position;}else{Θ.α=Υ.WorldMatrix.Forward;Θ.ψ=Υ.WorldMatrix.Left;Θ.ΰ=Υ.WorldMatrix.Down;Θ.μ=Υ.GetNaturalGravity();Θ.Δ=Й.GetPosition();Θ.γ=true;Θ.Ч=Й.Position;}}}double Γά=-1;if(Σ.Count>0) Γά=Vector3.Distance(β,Σ.Last().Δ);
double Θι=Math.Max(1.5,Math.Pow(ș/100.0,2));double ΕЃ=Math.Max(ș*Θι,2);Ζβ=ΕЃ;if((Γά==-1)||Γά>=ΕЃ){Χ Ψ=new Χ(β,ΑΤ,ƃ,Ə,Υ.GetNaturalGravity());Ψ.Ζε(Ȇ,ȅ);Σ.Add(Ψ);}}Χ ж(){if(Η!=Ζ.Ϊ) return ї!=null?ї:Θ;return ΕΜ;}void Ĳ(Χ Ϋ,Λ У){ΕΜ=Ϋ;if(ί==Λ.Ѕ) х=У;}bool Ƿ(Χ Ϋ,float ύ,bool ΘϏ,out Vector3 η)
{if(ΘϏ){Vector3I ΓΞ=new Vector3I((int) Ϋ.Ч.X,(int) Ϋ.Ч.Y,(int) Ϋ.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(ΓΞ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){η=new Vector3();return false;}Vector3 Ηψ=б(Υ,Τ.FatBlock.GetPosition()-β);Vector3 Ηχ=б(Υ,Τ.FatBlock.WorldMatrix.Forward);η=Ϋ.Δ-Βφ(Ϋ.α,Ϋ.ΰ*-1,Ηψ)-Βφ(Ϋ.α,Ϋ.ΰ*-1,Ηχ)*ύ;
return true;}else{η=Ϋ.Δ;return true;}}Vector3 ȉ(int Ηυ,int Ŗ,bool Θέ){if(!Θέ&&Εε) return ΒЉ;float Ηφ=((τ-1f)/2f)-Ηυ;float Θΰ=((Н-1f)/2f)-Ŗ;ΒЉ=Ε.Δ+Ε.ψ*Ηφ*Ǳ+Ȗ*-1*Θΰ*ǰ;Εε=true;return ΒЉ;}Vector3 ΒΛ(Vector3 Θξ,float ΒА){return Θξ+(Ǚ*ΒА);}ή ΑА(){float А=-1;ή έ=ή.Ǵ;
if(Η!=Ζ.Ϊ){Δυ();if(ί!=Λ.з){Vector3 Ј=Đ(Ǚ,Ȗ*-1,β-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(τ*Ǳ)/2f&&Math.Abs(Ј.Y)<=(float)(Н*ǰ)/2f){if(Ј.Z<=-1&&Ј.Z>=-ģ*2) return ή.Ʃ;if(Ј.Z>-1&&Ј.Z<θ*2) return ή.Ǵ;}if(ȟ(Ε.Δ,ref А)) έ=ή.Ǵ;}if(Θ.γ){if(ȟ(Θ.Δ,ref А)) έ=ή.ǳ;for(int i=0;i<Σ.Count;i++)
if(ȟ(Σ[i].Δ,ref А)) έ=ή.Ȋ;if(Vector3.Distance(β,Θ.Δ)<dockDist*θ) έ=ή.ǳ;if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null)
έ=ή.ǳ;}}else{Vector3 η=new Vector3();IMyShipConnector ǿ=З(MyShipConnectorStatus.Connected);if(Θ.γ){if(ȟ(Θ.Δ,ref А)) έ=ή.ǳ;if(Ƿ(Θ,dockDist,true,out η))
if(ȟ(η,ref А)) έ=ή.ǳ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Θ.Δ)<5)
return ή.ΑЅ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*θ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*θ)
if(ȟ(Σ[i].Δ,ref А)) έ=ή.Ȋ;if(Ε.γ){if(ȟ(Ε.Δ,ref А)) έ=ή.Ǵ;if(Ƿ(Ε,dockDist,true,out η))
if(ȟ(η,ref А)) έ=ή.Ǵ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5)
return ή.ΑІ;}}return έ;}bool ȟ(Vector3 η,ref float А){float ύ=Vector3.Distance(η,β);if(ύ<А||А==-1){А=ύ;return true;}return false;}void Βς(){if(ώ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϊ){Ƞ();return;}Ε.Δ=β;Ε.μ=Υ.GetNaturalGravity();Ε.α=ΑΤ;Ε.ΰ=ƃ;Ε.ψ=Ə;
Ǚ=ј.WorldMatrix.Forward;Ȗ=Ε.ΰ;if(Ǚ==Υ.WorldMatrix.Down) Ȗ=Υ.WorldMatrix.Backward;Ŵ(true,true);Ν(Α.ȣ);Βρ();}void Ŵ(bool ǔ,bool Θή){if(ί==Λ.з&&!ǔ) return;bool ΘΟ=ǔ||ί==Λ.ΒΑ||τ!=й||Н!=и||ğ!=Ț;if(ΘΟ){if(ί!=Λ.з){ί=Λ.Ƭ;ȉ(Щ,Ъ,Θή);Μ="Job changed, lost progress";}ğ=Ț;
τ=й;Н=и;Ъ=0;Щ=0;Ш="";П=-1;ġ=0;Ȕ=0;Р=0;К=0;Βχ(true);}}void Βθ(){Х(β,0);я(Ȇ,true);}void Βρ(){ǹ(в.ǀ);Βθ();Ų(Ȃ,false);Ǣ(ΓЅ);ί=Λ.Ѕ;ΔΥ(true);х=ί;π(Ω.Ŭ);ΗΗ();ΑΗ=true;ΑΨ=0;for(int i=Ŷ.Count-1;i>=0;i--)
if(Ū(Ŷ[i],false)) ΑΨ++;if(ΑΨ>0) Μ="Started with damage";}void χ(){if(ί==Λ.Ѕ){ί=Λ.ı;Μ="Job paused";}Ν(Α.а);х=ί;ǁ(false,0,0,0,0);Βυ();ΔΡ(new Vector3(),false);ΔЃ();Βλ(ChargeMode.Auto);Βν(false);ΔϏ(true);Ȍ(Α.а);đ(false,false,0,0);я(ο,false);я(т,true);Ǣ(true);Αρ=false;ƭ();Ƃ=false;
Βι();ΑΗ=false;ƙ=false;Ơ=false;if(Ќ!=Ω.Main&&Ќ!=Ω.ƪ&&Ќ!=Ω.ΑЂ&&Ќ!=Ω.ǂ)
π(Ω.Main);}void Ƞ(){ή Г=ΑА();if(Η==Ζ.Ϊ){if(!Ε.γ||!Θ.γ) return;Βρ();bool ΓΜ=Vector3.Distance(β,Θ.Δ)<Vector3.Distance(β,Ε.Δ);if(ř==Λ.ī) ΓΜ=true;if(ř==Λ.Ĭ) ΓΜ=false;if(ΓΜ){Ĳ(Θ,Λ.ī);switch(Г){case ή.ΑЅ:Ν(Α.Ȧ);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ǳ:Ν(Α.ǟ);break;default:Ν(Α.ǎ);break;
}}else{Ĳ(Ε,Λ.Ĭ);switch(Г){case ή.ΑІ:Ν(Α.Ȧ);break;case ή.Ǵ:Ν(Α.ǟ);break;case ή.Ȋ:Ν(Α.Я);break;default:Ν(Α.ǎ);break;}}}else{if(ί!=Λ.ı&&ί!=Λ.Ƭ) return;bool ΘЁ=ί==Λ.Ƭ;Βρ();bool ΓΡ=ū(false)&&Θ.γ;switch(Г){case ή.Ǵ:Ν(ΓΡ?Α.Ȣ:Α.ȣ);break;case ή.Ȋ:Ν(ΓΡ?Α.І:Α.Я);break;case ή.ǳ:Ν(ΓΡ?Α.ǟ:Α.ǻ);break;
case ή.Ʃ:{if(Ȕ!=К||ΘЁ) Ν(Α.ȋ);else Ν(Α.ů);}break;default:break;}}}void č(){if(ί==Λ.з&&!Θ.γ) return;if(Η==Ζ.Ϊ&&(!Ε.γ||!Θ.γ)) return;Μ="Move to job";ή Г=ΑА();if(Η==Ζ.Ϊ){Ĳ(Ε,Λ.Ĭ);switch(Г){case ή.Ǵ:Ν(Α.ǟ);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ΑІ:return;default:Ν(Α.ǎ);break;
}Ȍ(Α.Ȧ);}else{switch(Г){case ή.Ǵ:Ν(Α.Я);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ǳ:Ν(Α.ǻ);break;case ή.Ʃ:Ν(Α.ȋ);break;default:break;}if(ί==Λ.з) Ȍ(Α.Я);else Ȍ(Α.Ȟ);Αρ=true;}Βθ();π(Ω.Βτ);Ų(Ȃ,false);х=Λ.ƺ;}void Ʋ(){if(!Θ.γ) return;Μ="Move home";ή Г=ΑА();if(Η==Ζ.Ϊ){Ĳ(Θ,Λ.ī);switch(Г)
{case ή.Ȋ:Ν(Α.І);break;case ή.ǳ:Ν(Α.ǟ);break;case ή.ΑЅ:return;default:Ν(Α.ǎ);break;}Ȍ(Α.Ȧ);}else{if(З(MyShipConnectorStatus.Connected)!=null) return;if(З(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Č);Ȍ(Α.ǻ);return;}switch(Г){case ή.Ǵ:Ν(Α.Ȣ);break;case ή.Ȋ:Ν(Α.І);break;case ή.ǳ:Ν(Α.І);break;
case ή.Ʃ:Ν(Α.ď);break;default:break;}Ȍ(Α.ǻ);}Βθ();π(Ω.Βτ);Ų(Ȃ,false);х=Λ.ƹ;}void Ν(Α Γν){if(Γν==Α.а) ƛ=Α.а;if(ƛ!=Α.а&&Φ==ƛ&&Γν!=ƛ){χ();return;}Ё=true;Φ=Γν;}void Ȍ(Α У){this.ƛ=У;}void Ηβ(){if(Φ==Α.ȣ){if(Ё){Ђ=0;if(Ȕ!=К) ġ=0;Ȕ=К;}if(Ђ==0){int ΕΙ=Ζμ(К);if(ΕΙ!=К){К=ΕΙ;ġ=0;Ȕ=К;}
ĳ έ=ΗΞ(К);if(έ==ĳ.ΔΠ){ί=Λ.ΒΑ;Μ="Job done";if(Ć&&Θ.γ){Ν(Α.Ȣ);Ȍ(Α.ǻ);х=Λ.ƹ;}else{Ν(Α.į);Ȍ(Α.Ȟ);х=Λ.ƺ;}return;}if(έ==ĳ.Δβ){Ђ=1;я(ο,true);ш=ȉ(Щ,Ъ,true);Х(ш,10);Ф(Ε.ΰ,Ε.α,Ε.ψ,false);}}else{if(П>=0&&ΒΣ(П,true)){Ђ=0;return;}if(Л<wpReachedDist){Ν(Α.ů);return;}
}}if(Φ==Α.ů){if(Ё){я(ο,true);Ǣ(false);ш=ȉ(Щ,Ъ,false);Х(ΒΛ(ш,0),0);Ф(Ε.ΰ,Ε.α,Ε.ψ,false);Ђ=1;к=0;Ю=0;ΑΒ=0;Γέ=-1;ģ=Ц;ΓΨ=true;}if(!ΑЋ()){Ν(Α.ď);return;}if(ū(true)){ΕЈ=Ȉ("","ORE",И.ĉ);if((М==Ѐ.ΒΒ||М==Ѐ.Ʈ||
М==Ѐ.ΒΤ||М==Ѐ.Ƶ)&&Η!=Ζ.џ)
Ν(Α.Ŧ);else if((М==Ѐ.ΔΪ||М==Ѐ.ΑЍ)&&Η!=Ζ.џ)
Ν(Α.Βψ);else
Ν(Α.ď);return;}Р=Vector3.Distance(β,ш);if(Р>ġ){ġ=Р;ΓΨ=false;}if(Η==Ζ.џ&&Δη()==MyDetectedEntityType.SmallGrid)
Ю+=2;else Ю-=2;Ю=Math.Max(100,Math.Min(400,Ю));if(Ђ>0&&Ђ<Ю){if(Р>к){if(Ю>150) к=Р;else к=(float) Math.Ceiling(Р);Ђ=1;}else Ђ++;}else{if(Ђ>0){Μ="Ship stuck! Retrying";к=Р;Ђ=0;đ(false,true,0,θ*sensorRange);}Х(ΒΛ(ш,Math.Max(0,к-θ)),ΒΞ(false));if(Л<=wpReachedDist/2){Ђ=1;к=0;}
return;}đ(false,true,θ*sensorRange,0);Vector3 Ημ=ш+Ǚ*Р;bool Ερ=false;if(Vector3.Distance(Ημ,β)>0.3f){Vector3 Ηω=ш+Ǚ*(Р+0.1f);Х(Ηω,4);Ερ=true;}else{float Ǜ=ΒΞ(true);Vector3 ΕΠ=ΒΛ(ш,Math.Max(Ц+1,Р+1));ǡ(true,false,false,ΕΠ,ΕΠ-ш,Ǜ,Ǜ);}bool ΒБ=false;if(у==ь.ΑЈ||у==ь.ΑЇ)
{if(!Ερ){float Γο=0;foreach(IMyTerminalBlock ǯ in ο)
Γο+=Ζσ(ǯ,"","",у==ь.ΑЇ?new string[]{"STONE"}:null);if(Γο>Γέ||Р<Ц||ΓΨ){ΒϏ=0;ΑΒ=Р;ģ=(int)(Math.Max(ģ,ΑΒ)+θ/2);}else{ΒБ=Р-ΑΒ>2&&ΒϏ>=20;ΒϏ++;}Γέ=Γο;}}else ΒБ=Р>=ģ;if(Ȕ!=К){Ν(Α.ȋ);Р=0;return;}if(ΒБ){К++;Ν(Α.ȋ);Р=0;return;}}if(Φ==Α.ȋ||Φ==Α.ď||Φ==Α.Ŧ)
{if(Ё){ш=ȉ(Щ,Ъ,false);Ф(Ε.ΰ,Ε.α,Ε.ψ,false);я(ο,Ŀ);Ǣ(false);к=Vector3.Distance(β,ш);đ(false,true,0,θ*sensorRange);}Х(ш,ΒΞ(false));if(Vector3.Distance(β,ш)>=к+5){я(ο,false);Ǣ(true);Μ="Can´t return!";}if(Л<wpReachedDist){if(Φ==Α.ȋ&&Αρ) Ν(Α.į);if(Φ==Α.ȋ) Ν(Α.ȣ);
if(Φ==Α.Ŧ) Ν(Α.Ʊ);if(Φ==Α.ď){if(Θ.γ) Ν(Α.Ȣ);else{χ();č();Μ="Can´t return, no dock found";}}return;}}if(Φ==Α.І||Φ==Α.Я){if(Φ==Α.Я&&ί==Λ.Ѕ&&Η!=Ζ.Ϊ){if(!ΑЋ()||ū(true)){Ν(Α.І);return;}}bool Γω=false;bool Αώ=false;bool ΔА=false;float ĸ=0;bool ΕЇ=false;Χ Ψ=null;
if(Ё){if(Φ==Α.І||Η==Ζ.Ϊ){if(Η!=Ζ.Ϊ){if(Ȥ(1)) ΑЄ();else{ї=null;ȃ=0;Ƃ=!ΑЌ(1);}}Χ Ϋ=ж();ε=new Βζ();ε.Љ=Ϋ;ε.ƀ=followPathDock*θ;ε.ΔΓ=useDockDirectionDist*θ;ε.ĸ=10;ε.Ŝ.Add(Ϋ.Δ);Vector3 ķ=new Vector3();if(Ƿ(Ϋ,dockDist*θ,true,out ķ))
ε.Ŝ.Add(ķ);else ε.ƀ*=1.5f;if(Η==Ζ.Ϊ){if(Ϋ==Θ) ε.ΑΠ=Ε.Δ;if(Ϋ==Ε) ε.ΑΠ=Θ.Δ;ε.ΓΙ=dockDist*θ*1.1f;}}else if(Φ==Α.Я){ε=new Βζ();ε.Љ=Ε;ε.ƀ=followPathJob*θ;ε.ΔΓ=useJobDirectionDist*θ;ε.ĸ=10;ε.ΑΠ=Θ.Δ;ε.ΓΙ=dockDist*θ*1.1f;ε.Ŝ.Add(Ε.Δ);if(ί==Λ.з){if(!Θ.γ||Σ.Count==0){χ();return;}
float ΗЀ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΗЁ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΗЀ<ΗЁ) ε.Љ=Σ.Last();else ε.Љ=Σ.First();}}Ɣ=new Vector3();ΕЇ=!ΖЉ(β);я(ο,false);Ǣ(true);Т=-1;double ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ε.ΑΠ)<=ε.ΓΙ) continue;
double ύ=Vector3.Distance(Σ[i].Δ,β);if(ǧ==-1||ύ<ǧ){Т=i;ǧ=ύ;}}Ǭ=ΖϏ(ε.Љ.Δ,Т);Ę=null;}if(Φ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&!ų()){Χ ΒЃ=ж();Ƃ=!ΑЌ(1);if(ж()!=ΒЃ){Ν(Α.І);return;}}if(Φ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&Ȥ(1)&&ΑЄ()){Ν(Α.І);return;}bool Εο=false;if(Φ==Α.І&&!Ё&&Т>=0&&Т<Σ.Count)
{Vector3 ž=Σ[Т].Δ+Δθ(Т,Ǭ);int Γξ=Т+Ǭ;Vector3 ŏ=Ǭ!=0&&Γξ>=0&&Γξ<Σ.Count?Σ[Γξ].Δ:ε.Љ.Δ;if(ΖЂ(ž,ŏ)){if(!ΑΚ){ΑΚ=true;ΕΦ=İ(ž)?β:ž;}Х(ΕΦ,0);Ф(ε.Љ.ΰ,ε.Љ.α,ε.Љ.ψ,false);Μ="Waiting for dock clearance";return;}ΑΚ=false;Εο=Ȥ(1)&&(İ(β)||İ(ŏ));}Ζζ(Σ,Ǭ,ε.Ŝ,ε.ƀ,Ё,ref Ђ);
for(int i=0;i<ε.Ŝ.Count;i++){float ύ=Vector3.Distance(β,ε.Ŝ[i]);if(ύ<=ε.ƀ) Γω=true;if(ύ<=ε.ΔΓ) Αώ=true;}if(Εο) Αώ=true;if(Αώ) ĸ=ε.ĸ;float Ικ=Ę!=null?Ę.Ў:ș;float ΘІ=(float) Math.Max(ș*0.1f*θ,wpReachedDist);if((Л<ΘІ)||Ё){if(!Ё) Т+=Ǭ;if(Ǭ==0||Т>Σ.Count-1||Т<0)
Γω=true;else{ΔΘ=Ǭ>0?Σ.Count-1-Т:Т;Ψ=Σ[Т];Ę=Ψ;if(Т>=1&&Т<Σ.Count-1)
Ɣ=Ψ.Δ-Σ[Т-Ǭ].Δ;else Ę=null;ω=Ψ.Δ+Δθ(Т,Ǭ);ΔА=true;}}if(Αώ)
Ф(ε.Љ.ΰ,ε.Љ.α,ε.Љ.ψ,false);else if(ΕЇ)
ΑЃ(ε.Љ.ΰ,10,true);else if(ΔА&&Ψ!=null){if(Ǭ>0) Ф(Ψ.ΰ,Ψ.α,Ψ.ψ,90,false);else Ф(Ψ.ΰ,-Ψ.α,-Ψ.ψ,90,false);}ǡ(true,false,true,ω,Ɣ,Ę==null?0:Ę.Ў,ĸ);if(Γω){ΔΘ=0;if(Φ==Α.І&&!Ζκ()){Х(ω,0);return;}if(Φ==Α.І&&Ƃ){Х(ω,0);Μ="Waiting for a free dock";return;}if(Φ==Α.І||Η==Ζ.Ϊ){Ν(Α.ǟ);return;}
if(Φ==Α.Я&&Αρ){Ν(Α.į);return;}if(Φ==Α.Я){Ν(Α.ȣ);return;}}}if(Φ==Α.ǟ||Φ==Α.Ű){if(Η!=Ζ.Ϊ&&ų()){if(!Ȥ(1)){Ν(Α.І);return;}}else if(Η!=Ζ.Ϊ){Χ ΒЃ=ж();if(!ΑЌ(2)||ж()!=ΒЃ){Ν(Α.І);return;}}Χ Ϋ=ж();if(Ё){if(!Ƿ(Ϋ,dockDist*θ,true,out ω)){ǹ(в.Ť);χ();return;}
Х(ω,0);ΑЃ(Ϋ.ΰ,90,true);}if(Л<followPathDock*θ&&Л!=-1){Х(ω,10);Ф(Ϋ.ΰ,Ϋ.α,Ϋ.ψ,false);}if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null){Ν(Α.Č);return;}if(Л<wpReachedDist/2&&Л!=-1){Ν(Α.Ĉ);return;}}if(Φ==Α.Ĉ||Φ==Α.Ȟ){if(Ё)
{if(Φ==Α.Ĉ){Χ Ϋ=ж();if(!Ƿ(Ϋ,dockDist*θ,true,out ω)){ǹ(в.Ť);χ();return;}ǡ(true,true,false,ω,0);Ф(Ϋ.ΰ,Ϋ.α,Ϋ.ψ,10,false);}if(Φ==Α.Ȟ){Ф(Ε.ΰ,Ε.α,Ε.ψ,0.5f,false);ω=Ε.Δ;ǡ(true,true,false,ω,0);}}if(ΑΪ){ǁ(false,0,0,0,0);if(Φ==Α.Ĉ) Ν(Α.Č);if(Φ==Α.Ȟ) χ();return;}}if(Φ==Α.Č)
{if(З(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϊ) Ν(Α.Ȧ);else Ν(Α.ǻ);return;}Χ Ϋ=ж();if(Ё){Ю=0;ΗЂ=DateTime.Now;Ђ=0;Ф(Ϋ.ΰ,Ϋ.α,Ϋ.ψ,false);}Vector3I ΓΞ=new Vector3I((int) Ϋ.Ч.X,(int) Ϋ.Ч.Y,(int) Ϋ.Ч.Z);IMySlimBlock ΕΛ=Me.CubeGrid.GetCubeBlock(ΓΞ);float ΕΨ=dockingSpeed;float ΘΙ=dockingSpeed*5;
float ΔЍ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ƿ(Ϋ,0,true,out ω)||!Ƿ(Ϋ,ΔЍ,true,out Ɣ)||
ΕΛ==null||!ΕΛ.FatBlock.IsFunctional){ǹ(в.Ť);χ();return;}if(Ю==1||(Vector3.Distance(β,ω)<=ΔЍ*1.1f&&!Ё))
Ю=1;else{Vector3 Θχ=б(Υ,Ɣ-β);Vector3 ΓΝ=б(Υ,Υ.GetNaturalGravity());float Θα=ƫ(Θχ,ΓΝ,null);ΕΨ=Math.Min(ΘΙ,Θα);}ǡ(true,false,false,ω,ω-β,dockingSpeed,ΕΨ);if(Ё) к=(float) Л;IMyShipConnector ΒЍ=З(MyShipConnectorStatus.Connectable);if(ΒЍ!=null){ǡ(false,false,false,ω,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΒЍ.Connect();if(ΒЍ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϊ) Ν(Α.Ȧ);else Ν(Α.ǻ);Βυ();Ų(Ȃ,true);return;}}}else{float ΕΥ=(float) Math.Round(Л,1);if(ΕΥ<к){Ђ=-1;к=ΕΥ;}else Ђ++;if(Ђ>20){Ν(Α.Ű);return;}}}if(Φ==Α.ǻ||Φ==Α.Ȧ||
Φ==Α.ǉ||Φ==Α.ư||Φ==Α.ċ){bool Βύ=false,Βώ=false;if(Η==Ζ.Ϊ){if(ж()==Θ) Βύ=true;else if(ж()==Ε) Βώ=true;}if(Ё){ƙ=false;if(З(MyShipConnectorStatus.Connected)==null){Ν(Α.ǎ);return;}Βυ();if(Βύ) ǅ(ϋ.Ǔ);if(Βώ) ǅ(ό.Ǔ);ħ=false;ȍ=false;ȑ=false;Ƣ=false;}if(З(MyShipConnectorStatus.Connected)==null){χ();ǹ(в.Δπ);return;}
if(ί!=Λ.Ѕ||ц==-1||ǥ==п.None) ȍ=true;else if(Ǽ>=100f) ȍ=true;else if(Ǽ<=99f) ȍ=false;if(ί!=Λ.Ѕ||ч==-1||ѓ.Count==0) ȑ=true;else if(Ȓ>=100f) ȑ=true;else if(Ȓ<=99) ȑ=false;if(ί!=Λ.Ѕ||Ь==-1||Ă.Count==0) Ƣ=true;else Ƣ=Ĩ>=Ь;Ĵ о=null;if(Βύ) о=ϋ;
if(Βώ) о=ό;if(о!=null&&(о.Ћ==ι.ƽ||о.Ћ==ι.Ƽ)) ȍ=true;if(о!=null&&(о.Ћ==ι.Βέ)) if(!ħ) ȍ=false;if(о!=null&&(о.Ћ==ι.Βα||о.Ћ==ι.Βί)) ȑ=true;if(о!=null&&(о.Ћ==ι.Βΰ)) if(!ħ) ȑ=false;if(Ā){ChargeMode ΕΘ=ȍ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ћ==ι.Ƽ||о.Ћ==ι.ƽ))
ΕΘ=ChargeMode.Discharge;Βλ(ΕΘ);Βν(!ȑ);}if(!ħ){if(Η==Ζ.Ϊ)
ħ=ί!=Λ.Ѕ||Ζλ(Ё,true)||Ơ;else
ħ=ί!=Λ.Ѕ||Δρ();}else{if(!ȍ) Ν(Α.ċ);if(!ȑ) Ν(Α.ư);if(!Ƣ) Ν(Α.ǉ);Ё=false;}if(ħ&&ȍ&&ȑ&&Ƣ){Βλ(ChargeMode.Auto);Βν(false);if(ί==Λ.Ѕ){if(Η==Ζ.Ϊ){if(ж()==Θ) ǅ(ϋ.ǖ);else if(ж()==Ε) ǅ(ό.ǖ);if(ж()==Θ) Ĳ(Ε,Λ.Ĭ);else Ĳ(Θ,Λ.ī);}}Ν(Α.ǎ);return;}}if(Φ==Α.ǎ){if(ƥ){if(!ΔΨ()) return;
ƥ=false;Ν(Α.ǎ);return;}if(Ё){if(!ΔΨ()){ƥ=true;return;}IMyShipConnector ǿ=З(MyShipConnectorStatus.Connected);if(ǿ==null){Ν(Α.Я);return;}IMyShipConnector δ=ǿ.OtherConnector;Δώ(ǿ,false);Ų(Ȃ,false);Χ Ϋ=null;Χ ΓΦ=Η==Ζ.Ϊ?Θ:ж();if(Vector3.Distance(ǿ.GetPosition(),ΓΦ.Δ)<5f&&Θ.γ) Ϋ=ΓΦ;
if(Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5f&&Ε.γ) Ϋ=Ε;if(Ϋ!=null){if(!Ƿ(Ϋ,dockDist*θ,true,out ω)){ǹ(в.Ť);χ();return;}Х(ω,5);Ф(Ϋ.ΰ,Ϋ.α,Ϋ.ψ,false);}else Х(β+δ.WorldMatrix.Forward*dockDist*θ,5);if(ί==Λ.Ѕ) ǹ(в.ǀ);}if(Л<wpReachedDist){я(т,true);Ν(Α.Я);return;}}if(Φ==Α.į)
{if(Ё){Ǣ(true);я(ο,false);ω=Ε.Δ;Х(ω,20);Ф(Ε.ΰ,Ε.α,Ε.ψ,false);}if(Л<wpReachedDist/2){Ν(Α.Ȟ);return;}}if(Φ==Α.Ȣ){if(Ё){Ǣ(true);я(ο,false);int А=-1;double ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){double ύ=Vector3.Distance(Σ[i].Δ,β);if(ǧ==-1||ύ<ǧ){А=i;ǧ=ύ;}}
if(А==-1){Ν(Α.І);return;}ω=Σ[А].Δ;Х(ω,10);Ф(Ε.ΰ,Ε.α,Ε.ψ,false);}if(Л<wpReachedDist){Ν(Α.І);return;}}if(Φ==Α.Ʊ){bool ђ=false;if(Ё){Ǣ(true);if((М==Ѐ.ΒΒ||М==Ѐ.Ʈ)&&ƶ()&&
Į(Ǚ,Υ.GetNaturalGravity())<25&&τ>=2&&Н>=2){Vector3 Ƅ=β;if(Щ>0&&Ъ<Н-1) Ƅ=ȉ(Щ-1,Ъ+1,true);else if(Щ<τ-1&&Ъ<Н-1) Ƅ=ȉ(Щ+1,Ъ+1,true);else if(Щ<τ-1&&Ъ>0) Ƅ=ȉ(Щ+1,Ъ-1,true);else if(Щ>0&&Ъ>0) Ƅ=ȉ(Щ-1,Ъ-1,true);else ђ=true;if(!ђ) Х(Ƅ,10);
}else ђ=true;}if(Л<wpReachedDist/2) ђ=true;if(ђ){Ν(Α.ǈ);return;}}if(Φ==Α.Βψ||Φ==Α.ǈ){if(Ё){ǡ(true,true,false,β,0);я(ο,false);Ǣ(true);Ђ=-1;Ю=М==Ѐ.ΒΤ||М==Ѐ.Ƶ?0:-1;}bool Ě=!ΑЋ();int Αψ=Ȉ("STONE","ORE",И.ĉ);if(М==Ѐ.ΑЍ||М==Ѐ.Ƶ||М==Ѐ.Ʈ)
Αψ+=Ȉ("ICE","ORE",И.ĉ);bool ΗЊ=Αψ>0;bool ΓΘ=false;if(Ю>=0){float ĵ=(float) Math.Sin(Δά(Ю))*Ǳ/3f;float Ηξ=(float) Math.Cos(Δά(Ю))*ǰ/3f;Vector3 Ητ=ȉ(Щ,Ъ,true)+Βφ(Ǚ,Ȗ*-1,new Vector3(ĵ,Ηξ,0));Х(Ητ,0.3f);if(Л<Math.Min(Ǳ,ǰ)/10f) Ю+=5f;if(Ю>=360) Ю=0;}if(Ђ==-1||Αψ<Ђ){Ђ=Αψ;к=0;}
else{к++;if(к>50) ΓΘ=true;}if(!ΗЊ||Ě||ΓΘ){if(!Ě){int Θό=Ȉ("","ORE",И.ĉ);if(ū(true)) Ě=true;else if(100-(Џ(Θό,ΕЈ)*100)<minEjection) Ě=true;else ǹ(в.ǀ);}if(ΓΘ&&Ě) Μ="Ejection failed";if(Φ==Α.ǈ){if(Ě){if(Θ.γ) Ν(Α.Ȣ);else{χ();č();Μ="Can´t return, no dock found";}}
else Ν(Α.ȣ);}else if(Ě) Ν(Α.ď);else Ν(Α.ů);return;}}Ё=false;}void Ζζ(List<Χ>ć,int κ,List<Vector3>Ɲ,float ǭ,bool Ȅ,ref int ǐ){if(Ȅ){for(int i=0;i<ć.Count;i++) ć[i].Ў=0;ǐ=-1;return;}if(κ==0) return;int њ=κ*-1;if(ǐ==-1) ǐ=њ>0?1:ć.Count-2;int Εσ=0;while(ǐ>=1&&ǐ<ć.Count-1)
{if(Εσ>50) return;Εσ++;try{if((њ<0&&ǐ>=1)||(њ>0&&ǐ<=ć.Count-2)){Χ ζ=ć[ǐ];bool ΕЅ=false;for(int j=0;j<Ɲ.Count;j++)
if(Vector3.Distance(ζ.Δ,Ɲ[j])<=ǭ){ΕЅ=true;break;}if(!ΕЅ){Χ Αν=ć[ǐ-њ];Χ ŏ=ć[ǐ+њ];Vector3 Θψ=ζ.Δ-ŏ.Δ;Vector3 ΓЏ=Αν.Δ-ζ.Δ;Vector3 ΘΫ=ζ.Δ+Vector3.Normalize(Θψ)*ΓЏ.Length();Vector3 ΗϏ=Αν.Δ-ΘΫ;Vector3 Αί=Đ(κ>0?ζ.α:ζ.α*-1,ζ.ΰ*-1,ΗϏ);Vector3 ΘΘ=Đ(κ>0?ζ.α:ζ.α*-1,ζ.ΰ*-1,ΓЏ);
Vector3 Αα=Đ(κ>0?ζ.α:ζ.α*-1,ζ.ΰ*-1,ζ.μ);ζ.Ў=(float) Math.Sqrt(Math.Pow(Αν.Ў,2)+Math.Pow(ƫ(-ΘΘ,Αα,ζ),2));for(int j=0;j<Ɲ.Count;j++)
if(Vector3.Distance(Αν.Δ,Ɲ[j])<=ǭ){Vector3 Αβ=Đ(κ>0?ζ.α:ζ.α*-1,ζ.ΰ*-1,Ɲ[j]-ζ.Δ);float Θπ=ƫ(-Αβ,Αα,ζ);ζ.Ў=Math.Min(ζ.Ў,Θπ)/2f;}if(Αί.Length()==0) Αί=new Vector3(0,0,1);Vector3 ΘΗ=Đ(ζ.α,ζ.ΰ*-1,ζ.μ);float Θς=ŧ(-Αί,ΘΗ,ζ);float Ǌ=Џ(Θς,ĥ);float ΓЍ=(float) Math.Sqrt(Џ(Αί.Length()*1.0f,0.5f*Ǌ));
ζ.Ў=Math.Min(ζ.Ў,(ΓЏ.Length()/ΓЍ)*Ǥ);}}}catch{return;}ǐ+=њ;}ǐ=-1;}void Βχ(bool Ȅ){if(Ȅ){ƌ=0;return;}if(Η==Ζ.Ϊ) return;float ђ=К*Math.Max(1,Ц);if(Ȕ==К) ђ+=Math.Min(Ц,Р);float Θϋ=τ*Н*Math.Max(1,Ц);ƌ=Math.Max(ƌ,(float) Math.Min(ђ/Θϋ*100.0,100));}MyDetectedEntityType Δη()
{try{if(Ǹ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΞ(bool ǩ){if(Η==Ζ.џ&&Δη()==MyDetectedEntityType.None&&!Ū(ξ,true))
return fastSpeed;else return ǩ?Ţ:š;}void Ηά(){float ΕЀ=0,ΕΞ=0,Ğ=0,Γπ=0;for(int i=0;i<ȧ.Count;i++){IMyBatteryBlock ź=ȧ[i];if(!Ǹ(ź,true)) continue;ΕЀ+=ź.MaxStoredPower;ΕΞ+=ź.CurrentStoredPower;Ğ+=ź.CurrentInput;Γπ+=ź.CurrentOutput;}Ǽ=(float) Math.Round(Џ(ΕΞ,ΕЀ)*100,1);
if(Ğ>=Γπ) ǥ=п.ċ;else ǥ=п.Δέ;if(Ğ==0&&Γπ==0||Ǽ==100.0) ǥ=п.а;if(ȧ.Count==0) ǥ=п.None;}void Ηΰ(){float Εή=0;for(int i=0;i<ѓ.Count;i++){IMyGasTank ΖΤ=ѓ[i];if(!Ǹ(ΖΤ,true)) continue;Εή+=(float) ΖΤ.FilledRatio;}Ȓ=Џ(Εή,ѓ.Count)*100f;}void Ηθ(){Ĩ=0;try{for(int i=0;i<Ă.Count;i++)
{IMyReactor Γϊ=Ă[i];if(!Ǹ(Γϊ,true)) continue;List<MyInventoryItem>Ы=new List<MyInventoryItem>();Γϊ.GetInventory(0).GetItems(Ы);float Ǐ=0;for(int j=0;j<Ы.Count;j++){MyInventoryItem В=Ы[j];if(В.Type.SubtypeId.ToUpper()=="URANIUM"&&В.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǐ+=(float) В.Amount;}if(Ǐ<Ĩ||i==0){Ĩ=Ǐ;Γε=Γϊ.CustomName;}}}catch(Exception e){ȓ=e;}}void Ηέ(){ō=0;Ľ=0;try{for(int i=0;i<ļ.Count;i++){IMyTerminalBlock Τ=ļ[i];if(!Ǹ(Τ,true)) continue;Ľ+=(float) Τ.GetInventory(0).CurrentVolume;ō+=(float) Τ.GetInventory(0).MaxVolume;
}Αή=(float) Math.Min(Math.Round(Џ(Ľ,ō)*100,1),100.0);}catch(Exception e){ȓ=e;}}void Ηα(){try{Ǖ.Clear();for(int i=0;i<ļ.Count;i++){IMyTerminalBlock Τ=ļ[i];if(!Ǹ(Τ,true)) continue;ΔΟ(Τ,И.ũ);}if(М!=Ѐ.Βά){for(int i=0;i<Ĕ.Count;i++){IMyTerminalBlock Τ=Ĕ[i];if(!Ǹ(Τ,true)) continue;
ΔΟ(Τ,И.ΖЅ);}}ΗΥ(Ǖ);}catch(Exception e){ȓ=e;}}void Ζι(){if(ΑΗ){if(Δζ().Count>ΑΨ){ΑΗ=false;if(л!=е.Ũ){χ();if(л==е.ΒΗ) č();if(л==е.ƿ)
if(Θ.γ) Ʋ();else č();}Μ="Damage detected";}}}bool ΑЋ(){if(!ΑΩ) return true;if(ί==Λ.Ѕ){if(ц>0&&ǥ!=п.None)
if(Ǽ<=ц){Μ="Low energy! Move home";return false;}if(Ь>0&&Ă.Count>0)
if(Ĩ<=Ь){Μ="Low fuel: "+Γε;return false;}if(ч>0&&ѓ.Count>0)
if(Ȓ<=ч){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δζ(){List<IMyTerminalBlock>ę=new List<IMyTerminalBlock>();for(int i=0;i<Ŷ.Count;i++){IMyTerminalBlock Τ=Ŷ[i];if(Ū(Τ,false)) ę.Add(Τ);}return ę;}bool Ū(IMyTerminalBlock Τ,bool ΓІ){return(!Ǹ(Τ,ΓІ)||!Τ.IsFunctional);
}bool Ǹ(IMyTerminalBlock Τ,bool ΓІ){if(Τ==null) return false;try{IMyCubeBlock Εδ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΓІ) return Εδ==Τ;else return Εδ.GetType()==Τ.GetType();}catch{return false;}}float Ζσ(IMyTerminalBlock Τ,String ы,String ΖΚ,String[]Ǩ){float Ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory ń=Τ.GetInventory(i);List<MyInventoryItem>Ы=new List<MyInventoryItem>();ń.GetItems(Ы);foreach(MyInventoryItem В in Ы){if(Ǩ!=null&&(Ǩ.Contains(В.Type.TypeId.ToUpper())||Ǩ.Contains(В.Type.SubtypeId.ToUpper())))
continue;if((ы==""||В.Type.TypeId.ToUpper()==ы)&&(ΖΚ==""||В.Type.SubtypeId.ToUpper()==ΖΚ))
Ȁ+=(float) В.Amount;}}return Ȁ;}э Ζϊ(String φ,String ς,И Г,bool Ηϊ){φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǖ.Count;i++){э В=Ǖ[i];if(В.φ.ToUpper()==φ&&В.ς.ToUpper()==ς&&(В.Ŋ==Г||Г==И.ĉ))
return В;}э έ=null;if(Ηϊ){έ=new э(φ,ς,0,Г);Ǖ.Add(έ);}return έ;}int Ȉ(String φ,String ς,И Г){return Ȉ(φ,ς,Г,null);}int Ȉ(String φ,String ς,И Г,String[]Ǩ){int Ȁ=0;φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǖ.Count;i++){э В=Ǖ[i];if(Ǩ!=null&&Ǩ.Contains(В.φ.ToUpper())) continue;
if((φ==""||В.φ.ToUpper()==φ)&&(ς==""||В.ς.ToUpper()==ς)&&(В.Ŋ==Г||Г==И.ĉ))
Ȁ+=В.Ǐ;}return Ȁ;}void ΔΟ(IMyTerminalBlock Τ,И Г){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ы=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ы);for(int j=0;j<Ы.Count;j++)
Ζϊ(Ы[j].Type.SubtypeId,Ы[j].Type.TypeId.Replace("MyObjectBuilder_",""),Г,true).Ǐ+=(int) Ы[j].Amount;}}void ΗΥ(List<э>С){for(int i=С.Count-1;i>0;i--)
for(int j=0;j<i;j++){э a=С[j];э b=С[j+1];if(a.Ǐ<b.Ǐ) С.Move(j,j+1);}}bool ū(bool Ια){if(ȝ&&Η!=Ζ.Ϊ)
if(О!=-1&&ĥ>=О){Μ="Ship too heavy";return true;}if(Αή>=ǫ||ƙ){ƙ=false;Μ="Ship is full";return true;}return false;}bool Δρ(){String[]Ǩ=null;if(!Ȝ) Ǩ=new string[]{"ICE"};if(Η==Ζ.Ů) return Ȉ("","ORE",И.ũ,Ǩ)==0;if(Η==Ζ.џ) return Ȉ("","COMPONENT",И.ũ,Ǩ)==0;else return Ȉ("","",И.ũ,Ǩ)==0;
}bool ΖЊ(){List<IMyCockpit>ΒЌ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΒЌ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΒЌ.Count;i++)
if(ΒЌ[i].IsUnderControl) return true;return false;}bool Ζλ(bool Ȅ,bool Ατ){IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null) return false;if(Vector3.Distance(Θ.Δ,Й.GetPosition())<5)
return ΔΩ(ϋ,Ȅ,Ατ);if(Vector3.Distance(Ε.Δ,Й.GetPosition())<5)
return ΔΩ(ό,Ȅ,Ατ);return false;}bool ΔΩ(Ĵ Ǿ,bool Ȅ,bool Ατ){if(Ȅ) Ǿ.ΗΔ();Ǿ.Ηε();bool н=false;String ъ="";switch(Ǿ.Ћ){case ι.Βή:{ъ="Waiting for command";н=false;break;}case ι.Δω:{ъ="Waiting for cargo";н=ū(true);break;}case ι.Δψ:{ъ="Unloading";н=Δρ();break;}case ι.Βγ:{н=true;break;}
case ι.Βέ:{ъ="Charging batteries";н=Ǽ>=100f;break;}case ι.ƽ:{ъ="Discharging batteries";н=Ǽ<=25f;break;}case ι.Ƽ:{ъ="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βΰ:{ъ="Filling up hydrogen";н=Ȓ>=100f;break;}case ι.Βα:{ъ="Unloading hydrogen";н=Ȓ<=25f;break;}
case ι.Βί:{ъ="Unloading hydrogen";н=Ȓ<=0f;break;}case ι.Ββ:{bool Εξ=ΖЊ();if(!Εξ) Ǿ.Γφ=true;н=Ǿ.Γφ&&Εξ;ъ="Waiting for passengers";break;}}if(!н) Ǿ.ΑЊ(true);if(н&&Ǿ.ΒΠ()){н=Ǿ.ΑЊ(false);ъ="Undocking in: "+ΒΙ((int) Math.Max(0,Ǿ.ǒ-Ǿ.ȭ));}if(Ατ) Ζΰ=ъ;return н;}void Ζγ()
{if(!Ņ) return;if(ο.Count<=1) return;float ΖΩ=0,ΖΨ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǯ=ο[i];if(Ū(ǯ,true)) continue;ΖΩ+=(float) ǯ.GetInventory(0).MaxVolume;ΖΨ+=(float) ǯ.GetInventory(0).CurrentVolume;}float ΔБ=(float) Math.Round(Џ(ΖΨ,ΖΩ),5);for(int ΕЋ=0;ΕЋ<Math.Max(1,Math.Floor(ο.Count/10f));ΕЋ++)
{float ΓΥ=0,Γδ=0;float Ελ=0,Εώ=0;IMyTerminalBlock Ȑ=null,Ȯ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǯ=ο[i];if(Ū(ǯ,true)) continue;float Γλ=(float) ǯ.GetInventory(0).MaxVolume;float ΑΡ=Џ((float) ǯ.GetInventory(0).CurrentVolume,Γλ);if(Ȑ==null||ΑΡ>ΓΥ){Ȑ=ǯ;ΓΥ=ΑΡ;Ελ=Γλ;}
if(Ȯ==null||ΑΡ<Γδ){Ȯ=ǯ;Γδ=ΑΡ;Εώ=Γλ;}}if(Ȑ==null||Ȯ==null||Ȑ==Ȯ) return;if(checkConveyorSystem&&!Ȑ.GetInventory(0).IsConnectedTo(Ȯ.GetInventory(0))){if(ΒЂ>20) Μ="Inventory balancing failed";else ΒЂ++;return;}ΒЂ=0;List<MyInventoryItem>Ы=new List<MyInventoryItem>();
Ȑ.GetInventory(0).GetItems(Ы);float Ƥ=0;if(Ы.Count==0) continue;MyInventoryItem ΑΣ=Ы[0];String Ες=ΑΣ.Type.TypeId+ΑΣ.Type.SubtypeId;if(!Ζί.TryGetValue(Ες,out Ƥ)){if(ΖЍ(Ȑ.GetInventory(0),0,Ȯ.GetInventory(0),out Ƥ))
Ζί.Add(Ες,Ƥ);else return;}float Θω=((ΓΥ-ΔБ)*Ελ/Ƥ);float Θϊ=((ΔБ-Γδ)*Εώ/Ƥ);int ΔΑ=(int) Math.Min(Θϊ,Θω);if(ΔΑ<=0) return;if((float) ΑΣ.Amount<ΔΑ)
Ȑ.GetInventory(0).TransferItemTo(Ȯ.GetInventory(0),0,null,null,ΑΣ.Amount);else
Ȑ.GetInventory(0).TransferItemTo(Ȯ.GetInventory(0),0,null,null,ΔΑ);}}bool ΖЍ(IMyInventory ƚ,int Β,IMyInventory Ηώ,out float Ζή){Ζή=0;float Ηρ=(float) ƚ.CurrentVolume;List<MyInventoryItem>ΒЄ=new List<MyInventoryItem>();ƚ.GetItems(ΒЄ);float ΕΓ=0;for(int i=0;i<ΒЄ.Count;i++) ΕΓ+=(float) ΒЄ[i].Amount;
ƚ.TransferItemTo(Ηώ,Β,null,null,1);float Ζέ=Ηρ-(float) ƚ.CurrentVolume;List<MyInventoryItem>Βϊ=new List<MyInventoryItem>();ƚ.GetItems(Βϊ);float ΔЉ=0;for(int i=0;i<Βϊ.Count;i++) ΔЉ+=(float) Βϊ[i].Amount;if(Ζέ==0f||!ΒΥ(0.9999,ΕΓ-ΔЉ,1.0001))
return false;Ζή=Ζέ;return true;}void Ηί(){Vector3 Ƈ=ω+ą;Vector3 Ǯ=Ƈ-β;if(Ǯ.Length()==0) Ǯ=new Vector3(0,0,-1);Vector3 Αβ=б(Υ,Ǯ);Vector3 ΖΥ=Vector3.Normalize(Αβ);Vector3 ŉ=б(Υ,Υ.GetNaturalGravity());float Θκ=Γσ>0?Math.Max(0,1-Į(Ǯ,Εΰ)/5):0;float Ў=(float) Math.Min((Γθ>0?Γθ:1000f),Math.Max(ƫ(-Αβ,ŉ,null),Γσ*Θκ));
if(!ΖΧ) Ў=0;if(ΖΔ) Ў=Math.Max(0,1-ΑΕ/ǋ)*Ў;if(generalSpeedLimit>0) Ў=Math.Min(generalSpeedLimit,Ў);if(œ) Ў=0;if(ǲ>=0) Ў=Math.Min(Ў,ǲ);if(ΖΕ) Ў*=(float) Math.Min(1,Џ(Ǯ.Length(),wpReachedDist)/2);Vector3 Εϋ=б(Υ,Υ.GetShipVelocities().LinearVelocity);float Ην=(float)(Math.Max(0,15-Į(-ΖΥ,-Εϋ))/15)*0.85f+0.15f;
ΓЊ+=Math.Sign(Ην-ΓЊ)/10f;Vector3 ȩ=ΖΥ*Ў*ΓЊ-(Εϋ);Vector3 ΒЁ=ΒΝ(ȩ,null);if(Αω&&Л>0.1f){ȩ.X*=Βπ(ȩ.X,ref Αϊ.X,1f,ΒЁ.X,20);ȩ.Y*=Βπ(ȩ.Y,ref Αϊ.Y,1f,ΒЁ.Y,20);ȩ.Z*=Βπ(ȩ.Z,ref Αϊ.Z,1f,ΒЁ.Z,20);}else Αϊ=new Vector3(1,1,1);ƞ=ĥ*ȩ-ŉ*ĥ;ΔΡ(ƞ,Αω);Л=Vector3.Distance(β,Ƈ);
}float Βπ(float value,ref float У,float њ,float ȕ,float Γι){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(У)) У+=Math.Sign(У)*њ;else У=value;if(value==0) У=1;float έ=Math.Abs(У);if(έ<Γι||ȕ==0) return έ;У=Math.Min(Γι,Math.Max(-Γι,У));έ=Math.Abs(ȕ);return έ;
}void ΔЇ(){float щ=90,ў=90,љ=90;float Ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǩ,ѕ,Ɛ,Γα;if(Γβ){ǩ=Vector3.Normalize(ω-β);Ɛ=б(Υ,ǩ);ѕ=б(Υ,ΓЈ);щ=Į(Ɛ,new Vector3(0,-1,0))-90;ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);
љ=Ċ(Ɛ,new Vector3(-1,0,0),Ɛ.Z);}else{ǩ=ΓЉ;ѕ=б(Υ,ΓЈ);Ɛ=б(Υ,ΓЉ);Γα=б(Υ,ΖΦ);щ=Ċ(ѕ,new Vector3(0,0,1),ѕ.Y);ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);љ=Ċ(Γα,new Vector3(0,0,1),Γα.X);}if(ΔЋ&&ƶ()){Vector3 ΓΝ=Υ.GetNaturalGravity();ѕ=б(Υ,ΓΝ);щ=Ċ(ѕ,new Vector3(0,0,1),ѕ.Y);
ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);}if(!ΒΥ(-45,ў,45)){щ=0;љ=0;}if(!ΒΥ(-45,љ,45)) щ=0;ǁ(ΓΟ,1,(-щ)*Ǜ,(-љ)*Ǜ,(-ў)*Ǜ);ΑΕ=Math.Max(Math.Abs(щ),Math.Max(Math.Abs(ў),Math.Abs(љ)));ΑΪ=ΑΕ<=ǋ;}void Ζη(IMyTerminalBlock Αξ){if(Αξ==null) return;Ǟ=new float[3,2];ћ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȇ.Count;i++){IMyThrust г=Ȇ[i];if(!г.IsFunctional) continue;Vector3 κ=б(Αξ,г.WorldMatrix.Backward);float Ȭ=г.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) Ǟ[0,0]+=Ȭ;else Ǟ[0,1]-=Ȭ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) Ǟ[1,0]+=Ȭ;else Ǟ[1,1]-=Ȭ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) Ǟ[2,0]+=Ȭ;else Ǟ[2,1]-=Ȭ;String ы=Δκ(г);float[,]ќ=null;if(ћ.ContainsKey(ы)) ќ=ћ[ы];else{ќ=new float[3,2];ћ.Add(ы,ќ);}float ȕ=г.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ќ[0,0]+=ȕ;else ќ[0,1]-=ȕ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ќ[1,0]+=ȕ;else ќ[1,1]-=ȕ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ќ[2,0]+=ȕ;else ќ[2,1]-=ȕ;}}static String Δκ(IMyThrust г){return г.BlockDefinition.SubtypeId;}Vector3 ΒΨ(Vector3 ё,float[,]ŋ){return new Vector3(
ё.X>=0?ŋ[0,0]:ŋ[0,1],
ё.Y>=0?ŋ[1,0]:ŋ[1,1],
ё.Z>=0?ŋ[2,0]:ŋ[2,1]);}bool Δι(Χ Ψ,String ы,out float ΓΗ){ΓΗ=0;int Β=ȅ.IndexOf(ы);if(Β==-1||Ψ.ǝ==null||Β>=Ψ.ǝ.Length)
return false;ΓΗ=Ψ.ǝ[Β];if(ΓΗ==-1) return false;return true;}Vector3 ΒΝ(Vector3 ё,Χ Ψ){if(Ψ!=null){Vector3 έ=new Vector3();for(int i=0;i<ћ.Keys.Count;i++){String ы=ћ.Keys.ElementAt(i);float ƅ=0;if(!Δι(Ψ,ы,out ƅ))
return ΒΨ(ё,Ǟ);έ+=ΒΨ(ё,ћ.Values.ElementAt(i))*ƅ;}return έ;}return ΒΨ(ё,Ǟ);}float ŧ(Vector3 ё,Χ Ψ){return ŧ(ё,new Vector3(),Ψ);}float ŧ(Vector3 ё,Vector3 μ,Χ Ψ){Vector3 Θρ=ΒΝ(ё,Ψ);Vector3 Ȭ=Θρ+μ*ĥ;float ƕ=(Ȭ/ё).AbsMin();return(float)(ё*ƕ).Length();}float ƫ(Vector3 Αΰ,Vector3 ŉ,Χ Ψ)
{if(Αΰ.Length()==0) return 0;float ΔЊ=1;if(ŉ.Length()>0) ΔЊ=Math.Min(1,Į(-ŉ,Αΰ)/90)*0.4f+0.6f;float ƞ=ŧ(Αΰ,ŉ,Ψ);if(ƞ==0) return 0.1f;float Ǌ=Џ(ƞ,ĥ);float ΓЍ=(float) Math.Sqrt(Џ(Αΰ.Length(),Ǌ*0.5f));return Ǌ*ΓЍ*ΔЊ*Ǥ;}float ƴ(Vector3 ǩ,Vector3 Ϗ,Vector3 μ,Χ Ψ){if(μ.Length()==0f) return 0;
Vector3 Αα=Đ(ǩ,Ϗ,Vector3.Normalize(μ));float ǔ=ŧ(-Αα,Ψ);return ǔ/μ.Length();}void ΔΥ(bool ΗА){float ѝ=0;float ΓΚ=0.9f;if(ΗА){О=-1;ĩ=0;Αφ=null;if(ί!=Λ.з&&Ε.μ.Length()!=0){ѝ=ΓΚ*ƴ(Ε.α,Ε.ΰ*-1,Ε.μ,null);if(ѝ<О||О==-1) О=ѝ;}if(Θ.γ&&Θ.μ.Length()!=0){ѝ=ΓΚ*ƴ(Θ.α,Θ.ΰ*-1,Θ.μ,null);
if(ѝ<О||О==-1) О=ѝ;}return;}if(ĩ==-1) return;if(ĩ>=0){int ΕЎ=0;while(ĩ<Σ.Count){if(ΕЎ>100) return;ΕЎ++;Χ Ψ=Σ[ĩ];if(Ψ.μ.Length()!=0f){ѝ=ΓΚ*Math.Min(
ƴ(Ψ.α,Ψ.ΰ*-1,Ψ.μ,Ψ),
ƴ(Ψ.α*-1,Ψ.ΰ*-1,Ψ.μ,Ψ));if(ѝ<О||О==-1) О=ѝ;}else Αφ=Ψ;ĩ++;}ĩ=-1;}bool ΓΠ=true;float ā=0;if(Αφ!=null){for(int i=0;i<ћ.Count;i++){String ы=ћ.Keys.ElementAt(i);float[,]ќ=ћ.Values.ElementAt(i);float ƅ=0;if(!Δι(Αφ,ы,out ƅ)){ΓΠ=false;break;}for(int a=0;a<ќ.GetLength(0);a++)
for(int b=0;b<ќ.GetLength(1);b++){float ē=Math.Abs(ќ[a,b]*ƅ);if(ē==0) continue;ΓΠ=true;if(ā==0||ē<ā) ā=ē;}}}if(!ΓΠ){for(int a=0;a<Ǟ.GetLength(0);a++)
for(int b=0;b<Ǟ.GetLength(1);b++){float ē=Math.Abs(Ǟ[a,b]);if(ē==0) continue;if(ā==0||ē<ā) ā=ē;}}if(ā>0){float ΘΛ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ѝ=Џ(ā,ΘΛ);if(ѝ>0)
if(ѝ<О||О==-1) О=ѝ;}}void ΔΡ(Vector3 ǔ,bool Ї){if(!Ї){for(int i=0;i<Ȇ.Count;i++)
Ȇ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЀ=ΒΝ(ǔ,null);float ΘЈ=Math.Min(1,Math.Abs(Џ(ǔ.X,ΒЀ.X)));float ΘЉ=Math.Min(1,Math.Abs(Џ(ǔ.Y,ΒЀ.Y)));float ΘЊ=Math.Min(1,Math.Abs(Џ(ǔ.Z,ΒЀ.Z)));for(int i=0;i<Ȇ.Count;i++){IMyThrust г=Ȇ[i];Vector3 κ=ΗΖ(б(Υ,г.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǔ.X))
г.SetValueFloat("Override",г.MaxThrust*ΘЈ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǔ.Y))
г.SetValueFloat("Override",г.MaxThrust*ΘЉ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǔ.Z))
г.SetValueFloat("Override",г.MaxThrust*ΘЊ);else
г.SetValueFloat("Override",0.0f);}}void ǁ(bool Ї,float ΘΨ,float щ,float љ,float ў){for(int i=0;i<ΑΥ.Count;i++){IMyGyro Є=ΑΥ[i];Є.GyroOverride=Ї;if(!Ї) Є.GyroPower=100;else Є.GyroPower=ΘΨ;if(!Ї) continue;Vector3 Б=Υ.WorldMatrix.Forward;Vector3 Э=Υ.WorldMatrix.Right;Vector3 Ϗ=Υ.WorldMatrix.Up;
Vector3 ŀ=Є.WorldMatrix.Forward;Vector3 ł=Є.WorldMatrix.Up;Vector3 Ł=Є.WorldMatrix.Left*-1;if(ŀ==Б) Є.SetValueFloat("Roll",ў);else if(ŀ==(Б*-1)) Є.SetValueFloat("Roll",ў*-1);else if(ł==(Б*-1)) Є.SetValueFloat("Yaw",ў);else if(ł==Б) Є.SetValueFloat("Yaw",ў*-1);else if(Ł==Б) Є.SetValueFloat("Pitch",ў);
else if(Ł==(Б*-1)) Є.SetValueFloat("Pitch",ў*-1);if(Ł==(Э*-1)) Є.SetValueFloat("Pitch",щ);else if(Ł==Э) Є.SetValueFloat("Pitch",щ*-1);else if(ł==Э) Є.SetValueFloat("Yaw",щ);else if(ł==(Э*-1)) Є.SetValueFloat("Yaw",щ*-1);else if(ŀ==(Э*-1)) Є.SetValueFloat("Roll",щ);
else if(ŀ==Э) Є.SetValueFloat("Roll",щ*-1);if(ł==(Ϗ*-1)) Є.SetValueFloat("Yaw",љ);else if(ł==Ϗ) Є.SetValueFloat("Yaw",љ*-1);else if(Ł==Ϗ) Є.SetValueFloat("Pitch",љ);else if(Ł==(Ϗ*-1)) Є.SetValueFloat("Pitch",љ*-1);else if(ŀ==Ϗ) Є.SetValueFloat("Roll",љ);else if(ŀ==(Ϗ*-1)) Є.SetValueFloat("Roll",љ*-1);
}}void ΔЃ(){this.ΓΟ=false;}void Ф(Vector3 ȫ,Vector3 Б,Vector3 Γί,float ț,bool ƣ){ΑЃ(ȫ,ț,ƣ);ǋ=ț;Γβ=false;this.ΓЉ=Б;this.ΖΦ=Γί;}void Ф(Vector3 ȫ,Vector3 Б,Vector3 Γί,bool ƣ){Ф(ȫ,Б,Γί,2f,ƣ);}void ΑЃ(Vector3 ȫ,float ț,bool ƣ){ǋ=ț;this.ΓΟ=true;this.ΔЋ=ƣ;Γβ=true;ΑΪ=false;
this.ΓЈ=ȫ;}void Βυ(){ǡ(false,false,false,ω,0);Αω=false;}void Х(Vector3 Љ,float Ǜ){ǡ(true,false,false,Љ,Ǜ);}void ǡ(bool Ї,bool ΓЄ,bool ΔΔ,Vector3 Љ,float Ǜ){ǡ(Ї,ΓЄ,ΔΔ,Љ,Љ-β,0.0f,Ǜ);}void ǡ(bool Ї,bool ΓЄ,bool ΔΔ,Vector3 Љ,Vector3 Θδ,float Θε,float Ǜ){Αω=true;this.ΖΧ=Ї;
ω=Љ;this.Γθ=Ǜ;this.Γσ=Θε;this.ΖΕ=ΓЄ;this.ΖΔ=ΔΔ;this.Εΰ=Θδ;Л=Vector3.Distance(Љ,β);}void ΗΗ(){ϊ.GetBlocksOfType(Ŷ,Ǎ);}void ΗΘ(){if(Απ){ώ=2;return;}List<IMyRemoteControl>Ɩ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΒ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΖΜ=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(Ɩ,Ǎ);ϊ.GetBlocksOfType(є,Ǎ);ϊ.GetBlocksOfType(ΖΒ,Ǎ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΖΜ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);є=Ǡ(є,pamTag,true);ΗΟ();ΗΠ(ΖΜ);ű(є,setLCDFontAndSize,1.4f,false);ű(Ą,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΖΠ=Ǡ(ΖΒ,pamTag,true);if(ΖΠ.Count>0) ξ=ΖΠ[0];else ξ=null;if(Η==Ζ.Ů){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ώ=1;Μ="Drills are missing";}}else if(Η==Ζ.џ){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ώ=1;Μ="Grinders are missing";}if(ξ==null){ώ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϊ){ϊ.GetBlocksOfType(ΓЎ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΖΟ=Ǡ(Ɩ,pamTag,true);if(ΖΟ.Count>0) Ɩ=ΖΟ;if(Ɩ.Count>0) Υ=Ɩ[0];else{Υ=null;ώ=2;Μ="Remote is missing";return;}θ=(float) Υ.CubeGrid.WorldVolume.Radius*2;ј=null;if(Η!=Ζ.Ϊ){ΔΦ();if(ο.Count>0&&ј!=null){if(ξ!=null&&(ј.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Υ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ώ=1;Μ="Wrong sensor direction";}if(ј.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
ј.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ώ=2;Μ="Wrong remote direction";}}}}void ΗΙ(){ϊ.GetBlocksOfType(Ȃ,Ǎ);ϊ.GetBlocksOfType(т,Ǎ);ϊ.GetBlocksOfType(Ȇ,Ǎ);ϊ.GetBlocksOfType(ΑΥ,Ǎ);ϊ.GetBlocksOfType(ȧ,Ǎ);ϊ.GetBlocksOfType(Ă,Ǎ);ϊ.GetBlocksOfType(ѓ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(Αυ,Ǎ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=Ǡ(т,"ConnectorMedium",false);else
т=Ǡ(т,"Connector",false);List<IMyShipConnector>ΖΝ=Ǡ(т,pamTag,true);if(ΖΝ.Count>0) т=ΖΝ;if(ώ<=1){if(т.Count==0){ώ=1;Μ="Connector is missing";}if(ΑΥ.Count==0){ώ=1;Μ="Gyros are missing";}if(Ȇ.Count==0){ώ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>ΖΡ=Ǡ(Αυ,pamTag,true);
if(ΖΡ.Count>0) Αυ=ΖΡ;List<IMyLandingGear>ΖΞ=Ǡ(Ȃ,pamTag,true);if(ΖΞ.Count>0) Ȃ=ΖΞ;for(int i=0;i<Ȃ.Count;i++) Ȃ[i].AutoLock=false;List<IMyBatteryBlock>ΖΛ=Ǡ(ȧ,pamTag,true);if(ΖΛ.Count>0) ȧ=ΖΛ;List<IMyGasTank>ΖΣ=Ǡ(ѓ,pamTag,true);if(ΖΣ.Count>0) ѓ=ΖΣ;}void ΗΚ(){ϊ.GetBlocksOfType(Ĕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ļ.Clear();for(int i=Ĕ.Count-1;i>=0;i--){if(ΖЇ(Ĕ[i])){ļ.Add(Ĕ[i]);Ĕ.RemoveAt(i);}}}bool ΖЇ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΒΡ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΔΦ(){if(Υ==null) return;ј=null;float Αι=0,Αδ=0,Ακ=0,Αε=0,Αλ=0,Αζ=0;List<IMyTerminalBlock>ΓЇ=Ǡ(ο,pamTag,true);bool ΘΣ=ΓЇ.Count==0;if(ΓЇ.Count>0) ј=ΓЇ[0];else if(ο.Count>0) ј=ο[0];int Ȁ=0;float Ŕ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ј.WorldMatrix.Forward){if(ΘΣ){ώ=2;Μ="Mining direction is unclear!";return;}continue;}Ȁ++;Ŕ=Math.Max(Ŕ,ΗΪ(ο[i]));Vector3 Ј=Ƴ(Υ,ο[i].GetPosition());if(i==0){Αι=Ј.X;Αδ=Ј.X;Ακ=Ј.Y;Αε=Ј.Y;Αλ=Ј.Z;Αζ=Ј.Z;}Αδ=Math.Max(Ј.X,Αδ);Αι=Math.Min(Ј.X,Αι);
Αε=Math.Max(Ј.Y,Αε);Ακ=Math.Min(Ј.Y,Ακ);Αζ=Math.Max(Ј.Z,Αζ);Αλ=Math.Min(Ј.Z,Αλ);}if(Ŕ==0) Ŕ=drillRadius;bool ȫ=ј!=null&&ј.WorldMatrix.Forward==Υ.WorldMatrix.Down;Ǳ=(Αδ-Αι)*(1-Ī)+Ŕ*2;ǰ=(ȫ?Αζ-Αλ:Αε-Ακ)*(1-Ĝ)+Ŕ*2;IMyCubeGrid g=Υ.CubeGrid;float ΔΚ=0,ΔΛ=0,ΔΜ=0,ΔΝ=0;
for(int c=0;c<8;c++){Vector3 p=Ƴ(Υ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȫ?p.Z:p.Y;if(c==0||p.X<ΔΚ) ΔΚ=p.X;if(c==0||p.X>ΔΛ) ΔΛ=p.X;if(c==0||y<ΔΜ) ΔΜ=y;if(c==0||y>ΔΝ) ΔΝ=y;}ΓϏ=ΔΛ-ΔΚ+g.GridSize;Γώ=ΔΝ-ΔΜ+g.GridSize;}float ΗΪ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǡ<T>(List<T>С,String ǜ,bool ΑΦ){List<T>έ=new List<T>();for(int i=0;i<С.Count;i++)
if(ΒΡ(С[i],ǜ,ΑΦ)) έ.Add(С[i]);return έ;}bool ΒΡ<T>(T Τ,String ǜ,bool ΑΦ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(ΑΦ&&b.CustomName.ToUpper().Contains(ǜ.ToUpper())) return true;if(!ΑΦ&&b.BlockDefinition.ToString().ToUpper().Contains(ǜ.ToUpper())) return true;return false;}void я<T>(List<T>С,bool Ї)
{for(int i=0;i<С.Count;i++)
Δώ((IMyTerminalBlock) С[i],Ї);}void Δώ(IMyTerminalBlock Τ,bool Ї){if(Τ==null) return;String Ηλ=Ї?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(Ηλ);a.Apply(Τ);}void Ǣ(bool Ї){ΓЅ=Ї;if(!Ħ) return;я(Αυ,Ї);}void Βλ(ChargeMode Ƒ){for(int i=0;i<ȧ.Count;i++) ȧ[i].ChargeMode=Ƒ;}void Βν(bool Θν)
{for(int i=0;i<ѓ.Count;i++) ѓ[i].Stockpile=Θν;}void Ų(List<IMyLandingGear>ΓΛ,bool Εό){for(int i=0;i<ΓΛ.Count;i++){if(Εό) ΓΛ[i].Lock();if(!Εό) ΓΛ[i].Unlock();}}void ΔϏ(bool Ї){if(ΕΟ==Ї) return;List<IMyShipController>ΑΔ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΔ,Ǎ);if(ΑΔ.Count==0) return;
for(int i=0;i<ΑΔ.Count;i++)
ΑΔ[i].DampenersOverride=Ї;ΕΟ=Ї;}IMyShipConnector З(MyShipConnectorStatus ъ){for(int i=0;i<т.Count;i++){if(!Ǹ(т[i],true)) continue;if(т[i].Status==ъ) return т[i];}return null;}void đ(bool ΓЁ,bool ΑΙ,float ΗЇ,float Ηπ){if(ξ==null||ο.Count==0) return;Vector3 ΑΓ=new Vector3();
int Ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ј.WorldMatrix.Forward) continue;Ȁ++;ΑΓ+=ο[i].GetPosition();}ΑΓ=ΑΓ/Ȁ;Vector3 Ő=Ƴ(ξ,ΑΓ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЁ;ξ.LeftExtend=(ΑΙ?1:й)/2f*Ǳ-Ő.X;ξ.RightExtend=(ΑΙ?1:й)/2f*Ǳ+Ő.X;ξ.TopExtend=(ΑΙ?1:и)/2f*ǰ+Ő.Y;
ξ.BottomExtend=(ΑΙ?1:и)/2f*ǰ-Ő.Y;ξ.FrontExtend=(ΓЁ?Ц:ΗЇ)-Ő.Z;ξ.BackExtend=ΓЁ?0:Ηπ+θ*0.75f+Ő.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ű<T>(List<T>С,bool Θη,float ΗЅ,bool ΗБ){for(int i=0;i<С.Count;i++){IMyTextSurface Ɯ=null;if(С[i]is IMyTextSurface) Ɯ=(IMyTextSurface) С[i];if(Ɯ!=null){Ɯ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Θη) continue;Ɯ.Font="Debug";if(ΗБ) continue;Ɯ.FontSize=ΗЅ;}}}bool ƶ(){Vector3D η;return this.Υ.TryGetPlanetPosition(out η);}int ΖϏ(Vector3 Θο,int ΖΖ){if(ΖΖ==-1) return 0;double А=-1;int ȏ=-1;for(int i=Σ.Count-1;i>=0;i--){double ύ=Vector3.Distance(Σ[i].Δ,Θο);if(А==-1||ύ<А){ȏ=i;А=ύ;}
}return Math.Sign(ȏ-ΖΖ);}bool ΖЉ(Vector3 η){List<Vector3>Е=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Е.Add(Σ[i].Δ);if(Θ.γ&&Σ.Count>=1){Vector3 ķ=new Vector3();Ƿ(Θ,dockDist*θ,false,out ķ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Е.Insert(0,ķ);Е.Insert(0,Θ.Δ);}
else{Е.Add(ķ);Е.Add(Θ.Δ);}}if(Η==Ζ.Ϊ){if(Ε.γ&&Σ.Count>=1){Vector3 Βό=new Vector3();Ƿ(Ε,dockDist*θ,false,out Βό);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Е.Insert(0,Βό);Е.Insert(0,Ε.Δ);}else{Е.Add(Βό);Е.Add(Ε.Δ);}}}else{if(ί!=Λ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Е.Insert(0,Ε.Δ);else Е.Add(Ε.Δ);}int ȏ=-1;double ǧ=-1;for(int i=0;i<Е.Count;i++){double ύ=Vector3.Distance(Е[i],η);if(ύ<ǧ||ǧ==-1){ǧ=ύ;ȏ=i;}}if(Е.Count==0) return false;double ΓΑ=Vector3.Distance(Е[ȏ],η);double ΘΩ=Vector3.Distance(Е[Math.Max(0,ȏ-1)],Е[ȏ])*1.5f;
double ΘΡ=Vector3.Distance(Е[Math.Min(Е.Count-1,ȏ+1)],Е[ȏ])*1.5f;return ΓΑ<ΘΩ||ΓΑ<ΘΡ||ΓΑ<laneOffset*θ*1.5f;}static float Џ(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΗΖ(Vector3 v,int ΒЏ){return new Vector3(Math.Round(v.X,ΒЏ),Math.Round(v.Y,ΒЏ),Math.Round(v.Z,ΒЏ));
}float Į(Vector3 a,Vector3 b){if(a==b) return 0;float ΗЃ=(a*b).Sum;float Εχ=a.Length();float Εψ=b.Length();if(Εχ==0||Εψ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΗЃ/(Εχ*Εψ)));if(float.IsNaN(έ)) return 0;return έ;}float Ċ(Vector3 v,Vector3 Αξ,float Θθ){float ĵ=Į(v,Αξ);
if(Θθ>0f) ĵ*=-1;if(ĵ>-90f) return ĵ-90f;else return 180f-(-ĵ-90f);}double Δά(float Ηό){return(Math.PI/180)*Ηό;}bool ΒΥ(double ΘΚ,double value,double Αγ){return(value>=ΘΚ&&value<=Αγ);}Vector3 ΗΫ(IMyTerminalBlock Τ,Vector3 Ј){return Vector3D.Transform(Ј,Τ.WorldMatrix);
}Vector3 Ƴ(IMyTerminalBlock Τ,Vector3 ΘЅ){return б(Τ,ΘЅ-Τ.GetPosition());}Vector3 б(IMyTerminalBlock Τ,Vector3 ΔΙ){return Vector3D.TransformNormal(ΔΙ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 Đ(Vector3 ǩ,Vector3 Ϗ,Vector3 ΔΙ){MatrixD Γη=MatrixD.CreateFromDir(ǩ,Ϗ);return Vector3D.TransformNormal(ΔΙ,MatrixD.Transpose(Γη));
}Vector3 Βφ(Vector3 ǩ,Vector3 Ϗ,Vector3 Ј){MatrixD Γη=MatrixD.CreateFromDir(ǩ,Ϗ);return Vector3D.Transform(Ј,Γη);}String υ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 λ(String s){String[]Д=s.Split('|');return new Vector3(float.Parse(Κ(Д,0)),float.Parse(Κ(Д,1)),float.Parse(Κ(Д,2)));
}String Ηι(Χ Ψ){String Њ=":";String έ=υ(Ψ.Δ)+Њ+υ(Ψ.α)+Њ+
υ(Ψ.ΰ)+Њ+υ(Ψ.ψ)+Њ+υ(Ψ.μ);for(int i=0;i<Ψ.ǝ.Length;i++)
έ+=Њ+Math.Round(Ψ.ǝ[i],3);return έ;}Χ ΗΦ(String s){String[]Д=s.Split(':');Χ Ψ=new Χ(
λ(Κ(Д,0)),λ(Κ(Д,1)),
λ(Κ(Д,2)),λ(Κ(Д,3)),
λ(Κ(Д,4)));int Β=5;List<float>Εά=new List<float>();while(Β<Д.Length){String ΑϏ=Κ(Д,Β);float f=0;if(!float.TryParse(ΑϏ,out f)) break;Εά.Add(f);Β++;}Ψ.ǝ=Εά.ToArray();return Ψ;}System.Text.StringBuilder Ř=new System.Text.StringBuilder();void Ρ<T>(T value,bool ΘΠ){if(ΘΠ) Ř.Append('\n');Ř.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ư(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ǆ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]Ηο,int Β){String ΑϏ=Ηο.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑϏ)) return "";
return ΑϏ;}void Save(){if(Απ||Η==Ζ.р){Storage="";return;}Storage=ΑЉ();}String ΑЉ(){Ř.Clear();Ř.Append(DATAREV+";");Ρ(υ(Αο),false);Ρ(υ(Θ.α));Ρ(υ(Θ.ψ));Ρ(υ(Θ.ΰ));Ρ(υ(Θ.μ));Ρ(υ(Θ.Δ));Ρ(υ(Θ.Ч));Ρ(Θ.γ);Ρ(υ(Ε.Δ));Ρ(υ(Ε.μ));Ρ(υ(Ε.α));Ρ(υ(Ε.ΰ));Ρ(υ(Ε.ψ));Ρ(υ(Ε.Ч));Ρ(Ε.γ);Ρ(υ(Ǚ));Ρ(υ(Ȗ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) х);Ρ(ǫ);Ρ(ц);Ρ(Ь);Ρ(ч);Ρ(Ǥ);Ρ(ȝ);Ρ(Ȝ);if(Η==Ζ.Ϊ){Ρ((int) ϋ.Ћ);Ρ(ϋ.ǒ);Ρ(ϋ.ȭ);Ρ(Ư(ϋ.Ǔ));Ρ(Ư(ϋ.ǖ));Ρ((int) ό.Ћ);Ρ(ό.ǒ);Ρ(ό.ȭ);Ρ(Ư(ό.Ǔ));Ρ(Ư(ό.ǖ));}else{Ρ((int) Ț);Ρ((int) л);Ρ((int) М);Ρ((int) у);Ρ((int) ğ);Ρ(Ć);Ρ(Ħ);Ρ(Ņ);Ρ(Ŀ);Ρ(й);Ρ(и);Ρ(Ц);Ρ(Ţ);Ρ(š);Ρ(Ī);Ρ(Ĝ);
Ρ(τ);Ρ(Н);Ρ(Щ);Ρ(Ъ);Ρ(Ȕ);Ρ(К);Ρ(ģ);Ρ(ġ);}Ρ(";");for(int i=0;i<ȅ.Count;i++)
Ρ((i>0?"|":"")+ȅ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(Ηι(Σ[i]),i>0);Ρ(";",false);Ρ(Ш+"|"+П,false);Ρ(";",false);Ρ(Ζφ(),false);String ά=Ř.ToString();Ř.Clear();return ά;}с ΗΕ(){return ƾ(Storage);}с ƾ(String ά){if(ά=="") return с.ΖА;String[]Ĥ=ά.Split(';');if(Κ(Ĥ,0)!=DATAREV) return с.ΒΫ;int Β=0;try{String[]Ι=Κ(Ĥ,1).Split('\n');
Αο=λ(Κ(Ι,Β++));Θ.α=λ(Κ(Ι,Β++));Θ.ψ=λ(Κ(Ι,Β++));Θ.ΰ=λ(Κ(Ι,Β++));Θ.μ=λ(Κ(Ι,Β++));Θ.Δ=λ(Κ(Ι,Β++));Θ.Ч=λ(Κ(Ι,Β++));Θ.γ=bool.Parse(Κ(Ι,Β++));Ε.Δ=λ(Κ(Ι,Β++));Ε.μ=λ(Κ(Ι,Β++));Ε.α=λ(Κ(Ι,Β++));Ε.ΰ=λ(Κ(Ι,Β++));Ε.ψ=λ(Κ(Ι,Β++));Ε.Ч=λ(Κ(Ι,Β++));Ε.γ=bool.Parse(Κ(Ι,Β++));
Ǚ=λ(Κ(Ι,Β++));Ȗ=λ(Κ(Ι,Β++));Ι=Κ(Ĥ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Κ(Ι,Β++));ί=(Λ) int.Parse(Κ(Ι,Β++));х=(Λ) int.Parse(Κ(Ι,Β++));ǫ=int.Parse(Κ(Ι,Β++));ц=int.Parse(Κ(Ι,Β++));Ь=int.Parse(Κ(Ι,Β++));ч=int.Parse(Κ(Ι,Β++));Ǥ=float.Parse(Κ(Ι,Β++));ȝ=bool.Parse(Κ(Ι,Β++));
Ȝ=bool.Parse(Κ(Ι,Β++));if(Η==Ζ.Ϊ){ϋ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϋ.ǒ=float.Parse(Κ(Ι,Β++));ϋ.ȭ=float.Parse(Κ(Ι,Β++));ϋ.Ǔ=ǆ(Κ(Ι,Β++));ϋ.ǖ=ǆ(Κ(Ι,Β++));ό.Ћ=(ι) int.Parse(Κ(Ι,Β++));ό.ǒ=float.Parse(Κ(Ι,Β++));ό.ȭ=float.Parse(Κ(Ι,Β++));ό.Ǔ=ǆ(Κ(Ι,Β++));ό.ǖ=ǆ(Κ(Ι,Β++));}else{Ț=(ǣ) int.Parse(Κ(Ι,Β++));
л=(е) int.Parse(Κ(Ι,Β++));М=(Ѐ) int.Parse(Κ(Ι,Β++));у=(ь) int.Parse(Κ(Ι,Β++));ğ=(ǣ) int.Parse(Κ(Ι,Β++));Ć=bool.Parse(Κ(Ι,Β++));Ħ=bool.Parse(Κ(Ι,Β++));Ņ=bool.Parse(Κ(Ι,Β++));Ŀ=bool.Parse(Κ(Ι,Β++));й=int.Parse(Κ(Ι,Β++));и=int.Parse(Κ(Ι,Β++));Ц=int.Parse(Κ(Ι,Β++));Ţ=float.Parse(Κ(Ι,Β++));
š=float.Parse(Κ(Ι,Β++));Ī=float.Parse(Κ(Ι,Β++));Ĝ=float.Parse(Κ(Ι,Β++));τ=int.Parse(Κ(Ι,Β++));Н=int.Parse(Κ(Ι,Β++));Щ=int.Parse(Κ(Ι,Β++));Ъ=int.Parse(Κ(Ι,Β++));Ȕ=int.Parse(Κ(Ι,Β++));К=int.Parse(Κ(Ι,Β++));ģ=int.Parse(Κ(Ι,Β++));ġ=float.Parse(Κ(Ι,Β++));}Ι=Κ(Ĥ,3).Replace("\n","").Split('|');
ȅ=Ι.ToList();Ι=Κ(Ĥ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(ΗΦ(Κ(Ι,i)));Ι=Κ(Ĥ,5).Split('|');Ш=Κ(Ι,0);if(!int.TryParse(Κ(Ι,1),out П)) П=-1;if(Ш.Length!=τ*Н){Ш="";П=-1;}Ζυ(Κ(Ĥ,6));}catch{return с.Δΰ;}ř=х;χ();return с.ǃ;}const int ΔΤ=64000;IMyTerminalBlock Δα(){List<IMyTerminalBlock>ĺ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ĺ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ĺ.Count>0?ĺ[0]:null;}void Ζω(){IMyTerminalBlock Τ=Δα();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String ά="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΑЉ();if(ά.Length>ΔΤ){Μ="Export too large: "+ά.Length+" chars, max "+ΔΤ;return;}Τ.CustomData=ά;Μ="Exported "+Σ.Count+" waypoints";}void ΖЃ(bool ΓΫ){IMyTerminalBlock Τ=Δα();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΖЄ(Τ.CustomData,ΓΫ);}void ΖЄ(String ά,bool ΓΫ){ά=ά.Replace("\r","");if(ά.StartsWith("//")){int ΕІ=ά.IndexOf('\n');ά=ΕІ<0?"":ά.Substring(ΕІ+1);}ά=ά.Trim();if(ά==""){Μ="Import: no PAM data in "+dataTag;return;}χ();String ΕΒ=ΑЉ();с έ=ƾ(ά);if(έ==с.ǃ&&ΓΫ){Χ Ň=new Χ(Ε);
Vector3 ΘΝ=Ǚ,ΘΜ=Ȗ;int w=й,h=и,d=Ц;ǣ Θλ=Ț;ь ΒА=у;float ΘΦ=Ī,ΘΥ=Ĝ;ƾ(ΕΒ);Ε=Ň;Ǚ=ΘΝ;Ȗ=ΘΜ;й=w;и=h;Ц=d;Ț=Θλ;у=ΒА;Ī=ΘΦ;Ĝ=ΘΥ;ί=Λ.ı;Ŵ(true,true);ί=Λ.ı;х=Λ.ı;Μ="Imported job, run CONT";return;}if(έ==с.ǃ){ƒ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ƾ(ΕΒ);Μ=έ==с.ΒΫ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗΟ(){ſ.Clear();for(int i=є.Count-1;i>=0;i--){String ά=є[i].CustomData.ToUpper();bool ΓΩ=false;if(ά=="INSTRUCTIONS"){ΓΩ=true;ΑΘ=true;}if(ά=="DEBUG") ΓΩ=true;if(ΓΩ){ſ.Add(є[i]);є.RemoveAt(i);}
}ű(ſ,false,1,false);}void ΗΠ(List<IMyTerminalBlock>ĺ){Ą.Clear();for(int i=0;i<ĺ.Count;i++){IMyTerminalBlock Τ=ĺ[i];try{String ΕЍ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΕЍ);int ś=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΕЍ.Length,1),out ś)) continue;
if(ś==-1) continue;ś--;IMyTextSurfaceProvider ΕЏ=(IMyTextSurfaceProvider) Τ;if(ś<ΕЏ.SurfaceCount&&ś>=0)
Ą.Add(ΕЏ.GetSurface(ś));}catch{}}}String Αη="";void Δϊ(){String Γζ="";String ΓΖ="";Αη=ΔΣ(false);Γζ+=Αη;ΓΖ+=Αη;ΓΖ+=Ζύ();for(int i=0;i<є.Count;i++) є[i].WriteText(Γζ);for(int i=0;i<Ą.Count;i++) Ą[i].WriteText(Γζ);Echo(ΓΖ);for(int i=0;i<ſ.Count;i++){IMyTextPanel Γς=ſ[i];
String ά=Γς.CustomData.ToUpper();if(ά=="DEBUG") Γς.WriteText(""+"\n"+"");if(ά=="INSTRUCTIONS") Γς.WriteText(Ζώ());}}string Ζώ(){String Γ="";try{float ΕϏ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ĺ*ΕϏ)+" / "+(int)(Ō*ΕϏ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ĺ*100f,1)+"% / "+Math.Round(Ō*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ź,2)+"ms / "+Math.Round(Γκ,2)+"ms \n";}catch{}for(int i=0;i<ȗ.Count;i++)
Γ+=""+ȗ.Keys.ElementAt(i)+" = "+Math.Round(ȗ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȗ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΖЁ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ű(є,setLCDFontAndSize,0.9f,false);ű(Ą,setLCDFontAndSize,0.9f,true);for(int i=0;i<є.Count;i++) є[i].WriteText(Γ+ȓ.ToString());for(int i=0;i<Ą.Count;i++) Ą[i].WriteText(Γ+ȓ.ToString());}void Ǻ(){if(!ΑΘ) return;try{Γχ=Runtime.CurrentInstructionCount;}catch{}
}void Ƕ(String φ){if(!ΑΘ) return;if(Γχ==0) return;try{float Αύ=(Runtime.CurrentInstructionCount-Γχ)/Runtime.MaxInstructionCount*100;if(!ȗ.ContainsKey(φ))
ȗ.Add(φ,new float[]{Αύ,Αύ});else{ȗ[φ][0]=Αύ;ȗ[φ][1]=Math.Max(Αύ,ȗ[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζδ(String ȯ){if(!Ļ) return;String Њ=""+BROADCAST_SEP;String ρ=VERSION+Њ;ρ+=ţ(ΓЀ)+Њ;
ρ+=(int) Η+Њ;ρ+=ΒΘ(β.X)+""+Њ;ρ+=ΒΘ(β.Y)+Њ;ρ+=ΒΘ(β.Z)+Њ;if(Η!=Ζ.Ϊ)
ρ+=ΒΚ(х)+(х==Λ.Ѕ?" "+Math.Round(ƌ,1)+"%":"")+Њ;else ρ+=ΒΚ(х)+Њ;ρ+=ţ(Μ)+Њ;ρ+=і+""+Њ;ρ+=ţ(Αη)+Њ;ρ+=Ľ+Њ;ρ+=ō+Њ;for(int i=0;i<Ǖ.Count;i++){if(Ǖ[i].Ŋ!=И.ũ) continue;ρ+=ţ(Ǖ[i].φ)+"/"+ţ(Ǖ[i].ς)+"/"+Ǖ[i].Ǐ+Њ;}ΗΜ(ρ,ȯ);}String ţ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΗΜ(String ρ,String ȯ){ΗΝ(ρ,BROADCAST_SELF_ID,ȯ);}void ΗΝ(String ρ,String Љ,String ȯ){try{if(!Ļ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δδ()+BROADCAST_SEP+Љ+BROADCAST_SEP+ΒІ+BROADCAST_SEP+ȯ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){ȓ=e;}
}String Δδ(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Δτ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool ť(ref String ά,out string Εέ,Char Њ){int Β=ά.IndexOf(Њ);Εέ="";if(Β<0) return false;Εέ=ά.Substring(0,Β);ά=ά.Remove(0,Β+1);return true;}bool ΖБ(ref String ρ,out String Ƙ,out String ȯ)
{Ƙ="";ȯ="";if(!Ļ) return false;String ǜ="";if(!ť(ref ρ,out ǜ,BROADCAST_SEP)||!Δτ(ǜ)) return false;if(!ť(ref ρ,out Ƙ,BROADCAST_SEP)) return false;if(!ť(ref ρ,out ǜ,BROADCAST_SEP)||(ǜ!=Δδ()&&(ǜ!="*"&&Η!=Ζ.р))) return false;if(!ť(ref ρ,out ǜ,BROADCAST_SEP)||(ǜ!=ΒІ)) return false;
if(!ť(ref ρ,out ȯ,BROADCAST_SEP)) return false;return true;}void ΖІ(){bool Ž=true;if(Εί){Εί=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String ƈ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ƈ)
Δο(ƈ);String[]Ι=Me.CustomData.Split('\n');Ļ=bool.Parse(Βε(ΒΖ(Ι,"Enable_Broadcast",ref Ž)));bool ΔΗ=false;bool ΕΤ=true;if(Ļ){if(Η!=Ζ.р)
ΓЀ=Βε(ΒΖ(Ι,"Ship_Name",ref Ž)).Replace(BROADCAST_SELF_ID,"");ΒІ=Βε(ΒΖ(Ι,"Broadcast_Channel",ref Ž)).ToLower();ΕΤ=false;if(Ȏ==null){Ȏ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ȏ.SetMessageCallback("");}List<IMyRadioAntenna>Ķ=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(Ķ);bool Βϋ=false;
for(int i=0;i<Ķ.Count;i++)
if(Ķ[i].EnableBroadcasting&&Ķ[i].Enabled){Βϋ=true;break;}if(Ķ.Count==0) Μ="No Antenna found";else if(!Βϋ) Μ="Antenna not ready";ΔΗ=Ķ.Count==0||!Βϋ;if(ΔЌ&&!ΔΗ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΔЌ=ΔΗ;if(ΕΤ){if(Ȏ!=null) this.IGC.DisableBroadcastListener(Ȏ);Ȏ=null;}}catch{Ž=false;}if(!Ž){Μ="Reset custom data";Δο(ƈ);}}void Δο(String ƈ){String ά=ƈ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";ά+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=ά+"Broadcast_Channel=#default";}String Βε(String Ġ){int Β=Ġ.IndexOf("//");if(Β!=-1) Ġ=Ġ.Substring(0,Β);String[]Д=Ġ.Split('=');if(Д.Length<=1) return "";return Д[1].Trim();}String ΒΖ(String[]Ι,String ň,ref bool Ɠ){foreach(String Ġ in Ι)
if(Ġ.StartsWith(ň)) return Ġ;Ɠ=false;return "";}String Δύ(int ΔΕ,String Γ,int ΖΑ,ref int Ș){String[]Ι=Γ.Split('\n');if(ΖΑ>=Ș+ΔΕ-1) Ș++;Ș=Math.Min(Ι.Count()-1-ΔΕ,Ș);if(ΖΑ<Ș+1) Ș--;Ș=Math.Max(0,Ș);String έ="";for(int i=0;i<ΔΕ;i++){int Εω=i+Ș;if(Εω>=Ι.Count()) break;
έ+=Ι[Εω]+"\n";}return έ;}String ΒΙ(int Ɨ){if(Ɨ>=60*60) return Math.Round(Ɨ/(60f*60f),1)+" h";if(Ɨ>=60) return Math.Round(Ɨ/60f,1)+" min";return ""+Ɨ+" s";}string ΒΘ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>ő=new List<long>();
bool œ=false;int ΒЋ=0;int ΒЊ=0;DateTime ΕΕ=DateTime.Now;String ΒЅ="";void ΗΛ(){ǽ.Clear();ő.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!ő.Contains(ǽ[i].CubeGrid.EntityId)) ő.Add(ǽ[i].CubeGrid.EntityId);
}if(!ő.Contains(Me.CubeGrid.EntityId)) ő.Add(Me.CubeGrid.EntityId);}bool Δς(Α s){return s==Α.ȣ||s==Α.Я||s==Α.І||
s==Α.Ȣ||s==Α.į||s==Α.Ŧ||s==Α.Ʊ||
(s==Α.ǟ&&ї!=null);}void ƭ(){œ=false;ΒЋ=0;ΒЅ="";}void Ηγ(){if(!obstacleBrake||ǽ.Count==0||!Δς(Φ)){ƭ();return;}Vector3 Ƈ=ω+ą;Vector3 Ǯ=Ƈ-β;float ǭ=Ǯ.Length();if(ǭ<1f){ƭ();return;}Vector3 κ=Ǯ/ǭ;float Εύ=ș*obstacleLookAhead+θ;Vector3 Θά=Ƈ+κ*(θ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΒЊ=(ΒЊ+1)%ǽ.Count;IMyCameraBlock ż=ǽ[ΒЊ];if(!ż.IsFunctional||!ż.IsWorking) continue;Vector3 ΕΗ=ż.GetPosition();Vector3 ΑΟ=Θά;if(Vector3.Distance(ΕΗ,ΑΟ)>Εύ) ΑΟ=ΕΗ+κ*Εύ;if(!ż.CanScan(ΑΟ)) continue;MyDetectedEntityInfo Ɗ=ż.Raycast(ΑΟ);bool ΘΤ=!Ɗ.IsEmpty()&&!ő.Contains(Ɗ.EntityId)&&!ΖЌ(Ɗ.EntityId)&&
Ɗ.Type!=MyDetectedEntityType.FloatingObject;if(ΘΤ){if(!œ) ΕΕ=DateTime.Now;œ=true;ΒЋ=0;ΒЅ=Ɗ.Type.ToString();}else if(œ&&++ΒЋ>=3) ƭ();break;}if(œ&&(DateTime.Now-ΕΕ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΒЅ;}const String ΔІ="[PAMTFC]";const float ΗΨ=5f;const float ΒΕ=2f;class ȥ{public long ν;public Vector3 η,Š;public Vector3 Ɖ;public float ΕΣ;public String Ň="";public int Ƌ=-1;public bool Γψ;public long Ϋ=0;public int Ɓ=0;public DateTime ΓЌ;}IMyBroadcastListener Αό=null;
long ǚ=0;List<ȥ>м=new List<ȥ>();Vector3 ą=new Vector3();float ǲ=-1;Vector3 ΓА=new Vector3();bool Αϋ=false;DateTime ΓБ=DateTime.Now;const float ΔЅ=15f;const float ΗΧ=10f;int ΖΫ=0;String Ш="";int П=-1;String ΒΧ(){if(ί==Λ.з||Η==Ζ.Ϊ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+τ+"|"+Н+"|"+(int) ğ+"|"+Math.Round(Ǳ,1)+"|"+Math.Round(ǰ,1);}void Ηζ(){if(!trafficEnabled) return;if(Αό==null) Αό=IGC.RegisterBroadcastListener(ΔІ);ǚ=Me.CubeGrid.EntityId;int ΒЈ=30;while(Αό.HasPendingMessage&&ΒЈ-->0){String ά=Αό.AcceptMessage().Data as String;if(ά!=null) ΗΒ(ά);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΓЌ).TotalSeconds>ΗΨ) м.RemoveAt(i);if(++ΖΫ<3) return;if(Φ==Α.а&&З(MyShipConnectorStatus.Connected)==null) return;ΖΫ=0;IGC.SendBroadcastMessage(ΔІ,ǚ+";"+υ(β)+";"+
υ(Υ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ΒΧ()+";"+
(ί==Λ.Ѕ?П:-1)+";"+Ш+";"+Ζν()+","+Δή()+";"+υ(ΓА)+
";"+(ľ?1:0));}void ΗΒ(String ά){try{String[]f=ά.Split(';');if(f.Length<7) return;long ν=long.Parse(f[0]);if(ν==ǚ) return;ȥ Ж=null;for(int i=0;i<м.Count;i++) if(м[i].ν==ν) Ж=м[i];if(Ж==null){Ж=new ȥ();Ж.ν=ν;м.Add(Ж);}Ж.η=λ(f[1]);Ж.Š=λ(f[2]);Ж.ΕΣ=float.Parse(f[3]);
Ж.Ň=f[4];Ж.Ƌ=int.Parse(f[5]);Ж.ΓЌ=DateTime.Now;if(Ж.Ň!=""&&Ж.Ň==ΒΧ()) ΖЎ(f[6]);Ж.Ϋ=0;Ж.Ɓ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ж.Ϋ=long.Parse(d[0]);Ж.Ɓ=int.Parse(d[1]);}Ж.Ɖ=f.Length>=9?λ(f[8]):new Vector3();Ж.Γψ=f.Length>=10&&f[9]=="1";}catch{}}bool ΖЌ(long ν)
{for(int i=0;i<м.Count;i++) if(м[i].ν==ν) return true;return false;}Vector3 Δθ(int Β,int ΘΧ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Χ Ψ=Σ[Β];Vector3 Θύ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*ΘΧ;Vector3 Ϗ=Ψ.μ.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.μ):-Ψ.ΰ;
Vector3 Э=Vector3.Cross(Θύ,Ϗ);if(Э.LengthSquared()<0.0001f) return new Vector3();float ύ=laneOffset*θ;float ΗІ=Math.Min(Vector3.Distance(Ψ.Δ,Σ[0].Δ),
Vector3.Distance(Ψ.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Э)*ύ*Math.Min(1,ΗІ/(ύ*4));}Vector3 ΖΪ=new Vector3();bool ΖЋ(Α s){return s==Α.ǟ||s==Α.Ĉ||s==Α.Č||s==Α.Ű||
s==Α.ǎ||s==Α.ȋ||s==Α.Ȟ;}void Ηη(){ą=new Vector3();ǲ=-1;ΓА=new Vector3();bool Εβ=Δς(Φ);if(!trafficEnabled||(!Εβ&&!ΖЋ(Φ))){Αϋ=false;return;}ΖΪ=Υ.GetShipVelocities().LinearVelocity;Ζξ(Υ.GetNaturalGravity(),!Εβ);}void Ζξ(Vector3 μ,bool ΕЌ=false){ą=new Vector3();ǲ=-1;
Vector3 Ǯ=ω-β;float ǭ=Ǯ.Length();bool ΓΣ=ǭ>=1f;Vector3 κ=ΓΣ?Ǯ/ǭ:new Vector3();ΓА=κ;Vector3 Ϗ=μ.LengthSquared()>0.01f?-Vector3.Normalize(μ):-ƃ;Vector3 Э=Vector3.Cross(κ,Ϗ);Э=Э.LengthSquared()<0.0001f?-Ə:Vector3.Normalize(Э);ȥ Αέ=null;for(int i=0;i<м.Count;i++)
{ȥ δ=м[i];if(δ.Ɓ==3&&δ.Š.LengthSquared()<0.25f) continue;Vector3 η=δ.η+δ.Š*(float)(DateTime.Now-δ.ΓЌ).TotalSeconds;Vector3 ŕ=η-β;float Ǒ=(θ+δ.ΕΣ)*0.5f;float ŷ=ΓΣ?Vector3.Dot(ŕ,κ):0;if(ŷ>0&&(ŕ-κ*ŷ).Length()<=Ǒ*1.2f){float Αμ=Vector3.Dot(δ.Š,κ);bool Ει=δ.Ɖ.LengthSquared()>0.25f;
bool ΓΤ=Αμ<-1f||(Ει&&Vector3.Dot(δ.Ɖ,κ)<-0.5f);if(ΕЌ){if(ΓΤ&&ǚ<δ.ν) continue;}else{if(δ.Γψ&&!ľ){ą=Э*Ǒ*1.3f;continue;}if(ΓΤ){if(ŷ<(ș-Αμ)*6f+Ǒ*4f) ą=Э*Ǒ*1.3f;continue;}if(Ει&&ǚ<δ.ν){Vector3 ΕΑ=-ŕ;float ΕЉ=Vector3.Dot(ΕΑ,δ.Ɖ);if(ΕЉ>0&&(ΕΑ-δ.Ɖ*ΕЉ).Length()<=Ǒ*1.2f)
{ą=Э*Ǒ*1.3f;continue;}}}float Εγ=ŷ-Ǒ-followGap*θ;float Γΰ=Math.Max(0,Αμ)+Math.Max(0,Εγ)/ΒΕ;if(ΓΤ) Γΰ=Math.Max(0,Εγ+Math.Min(0,Αμ)*ΒΕ*2)/ΒΕ;if(ǲ<0||Γΰ<ǲ){ǲ=Γΰ;Αέ=δ;}continue;}if(δ.Š.LengthSquared()<1f) continue;if(ΓΣ&&(ŕ-κ*ŷ).Length()<=Ǒ*1.2f) continue;
Vector3 Γύ=δ.Š-ΖΪ;float ΕА=Γύ.LengthSquared();if(ΕА<0.25f) continue;float t=-Vector3.Dot(ŕ,Γύ)/ΕА;if(t<=0||t>5f||(ŕ+Γύ*t).Length()>=Ǒ*1.1f) continue;if(ǚ>δ.ν||t<2f){ǲ=0;Αέ=δ;}}bool ΗЍ=ǲ>=0&&ǲ<0.5f;if(!ΗЍ||ΕЌ||(Αέ!=null&&Αέ.Γψ&&ľ)){Αϋ=false;return;}
if(!Αϋ){Αϋ=true;ΓБ=DateTime.Now;}double Εκ=(DateTime.Now-ΓБ).TotalSeconds;if(Εκ>ΔЅ+ΗΧ) ΓБ=DateTime.Now;else if(Εκ>ΔЅ){ǲ=-1;ą=Э*θ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>Ǫ=new List<int>();String Εμ="";void ΒΔ(){String ň=τ+"|"+Н+"|"+(int) ğ;
if(ň==Εμ) return;Εμ=ň;Ǫ.Clear();int n=τ*Н;if(ğ==ǣ.Ǆ){for(int k=0;k<n;k++){int Ŗ=k/τ;Ǫ.Add(Ŗ*τ+(Ŗ%2==0?k-Ŗ*τ:τ-1-(k-Ŗ*τ)));}return;}int Εθ=(int) Math.Ceiling(τ/2f),Εη=(int) Math.Ceiling(Н/2f);int ΗЉ=τ/2,ΗЈ=Н/2;int x=0,y=0,ΓΔ=0,ΓΕ=-1,Αγ=Math.Max(τ,Н);
for(int i=0;i<Αγ*Αγ;i++){if(-Εθ<x&&x<=ΗЉ&&-Εη<y&&y<=ΗЈ) Ǫ.Add((y-1+Εη)*τ+(x-1+Εθ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΔ;ΓΔ=-ΓΕ;ΓΕ=t;}x+=ΓΔ;y+=ΓΕ;}}bool ΒΓ(){int n=Math.Max(0,τ*Н);if(Ш.Length==n) return false;Ш=new String('0',n);
П=-1;return true;}void ΖЎ(String δ){ΒΓ();if(δ.Length!=Ш.Length) return;char[]h=Ш.ToCharArray();for(int i=0;i<h.Length;i++) if(δ[i]=='1') h[i]='1';Ш=new String(h);}bool Δσ(int Ǧ){return Ǧ>=0&&Ǧ<Ш.Length&&Ш[Ǧ]=='1';}void Βμ(int Ǧ,bool ђ){if(Ǧ<0||Ǧ>=Ш.Length) return;
char[]h=Ш.ToCharArray();h[Ǧ]=ђ?'1':'0';Ш=new String(h);}bool ΒΣ(int Ǧ,bool ΘΒ){if(!trafficEnabled) return false;String ň=ΒΧ();for(int i=0;i<м.Count;i++){ȥ t=м[i];if(t.Ň!=ň||t.Ƌ<0||(ΘΒ&&t.ν>ǚ)) continue;float Ηϋ=Math.Abs(Ǧ%τ-t.Ƌ%τ)*Ǳ;float ΗЄ=Math.Abs(Ǧ/τ-t.Ƌ/τ)*ǰ;
if(t.Ƌ==Ǧ||(Ηϋ<ΓϏ+0.5f&&ΗЄ<Γώ+0.5f)) return true;}return false;}int Ζμ(int Β){int n=τ*Н;if(n<1) return Β;ΒΔ();if(ΒΓ()) for(int k=0;k<Β&&k<n;k++) Βμ(Ǫ[k],true);if(П>=0&&(Β>=n||Ǫ[Β]!=П)){Βμ(П,true);П=-1;}if(П>=0&&!Δσ(П)&&!ΒΣ(П,true)) return Β;
П=-1;for(int k=0;k<n;k++){int c=Ǫ[k];if(Δσ(c)||ΒΣ(c,false)) continue;П=c;return k;}return n;}ĳ ΗΞ(int Β){ΒΔ();if(Β<0||Β>=Ǫ.Count) return ĳ.ΔΠ;Щ=Ǫ[Β]%τ;Ъ=Ǫ[Β]/τ;return ĳ.Δβ;}class ю{public long ν;public Vector3 η,Б,Ϗ;}List<ю>Ѝ=new List<ю>();bool Ȫ=false;Vector3 ΑΜ,ΑΛ,ΑΝ,ΑΞ;
long ф=0;long ȃ=0;Χ ї=null;bool Ƃ=false;Χ ΑБ(Vector3 η,Vector3 Б,Vector3 Ϗ){MatrixD m=MatrixD.CreateWorld(η,Б,Ϗ);Χ w=new Χ(Θ);w.α=Vector3D.TransformNormal(ΑΜ,m);w.ΰ=Vector3D.TransformNormal(ΑΛ,m);w.ψ=Vector3D.TransformNormal(ΑΝ,m);w.Δ=η+(Vector3) Vector3D.TransformNormal(ΑΞ,m);
w.γ=true;return w;}int Δή(){Α s=Φ;if(s==Α.а) return З(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Č||s==Α.ǻ||s==Α.ċ||s==Α.ǉ||
s==Α.ư||s==Α.ǎ) return 3;if(s==Α.ǟ||s==Α.Ĉ||s==Α.Ű) return 2;if(s==Α.І) return 1;return 0;}long Ζν(){if(Δή()==0) return 0;if(Ȥ(1)&&Ś!=0) return Ś;return ї!=null?ȃ:ф;}void Δυ(){if(Η==Ζ.Ϊ||!Θ.γ) return;IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null||Й.OtherConnector==null) return;
IMyShipConnector δ=Й.OtherConnector;Vector3I ΗЎ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(Й.Position==ΗЎ){MatrixD ń=MatrixD.Transpose(δ.WorldMatrix);ΑΜ=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,ń);ΑΛ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,ń);ΑΝ=Vector3D.TransformNormal(Υ.WorldMatrix.Left,ń);
ΑΞ=Vector3D.TransformNormal(Й.GetPosition()-δ.GetPosition(),ń);Ȫ=true;}if(Vector3.Distance(Й.GetPosition(),Θ.Δ)<5) ф=δ.EntityId;List<IMyShipConnector>ě=new List<IMyShipConnector>();ϊ.GetBlocksOfType(ě,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ě.Count>0){Ѝ.Clear();for(int i=0;i<ě.Count;i++){ю p=new ю();p.ν=ě[i].EntityId;p.η=ě[i].GetPosition();p.Б=ě[i].WorldMatrix.Forward;p.Ϗ=ě[i].WorldMatrix.Up;Ѝ.Add(p);}}ї=null;ȃ=0;if(δ.EntityId==ф||!Ȫ) return;for(int i=0;i<Ѝ.Count;i++)
if(Ѝ[i].ν==δ.EntityId){ȃ=Ѝ[i].ν;ї=ΑБ(Ѝ[i].η,Ѝ[i].Б,Ѝ[i].Ϗ);}}bool ΒΦ(long ν,int ΕЄ){if(ν==0) return false;for(int i=0;i<м.Count;i++){ȥ t=м[i];if(t.Ϋ==ν&&(t.Ɓ>ΕЄ||(t.Ɓ==ΕЄ&&t.ν<ǚ))) return true;}return false;}bool ΑЌ(int Γυ){if(Η==Ζ.Ϊ||!trafficEnabled){ї=null;ȃ=0;return true;}
long ζ=ї!=null?ȃ:ф;if(!ΒΦ(ζ,Γυ)) return true;if(!ΒΦ(ф,Γυ)){ї=null;ȃ=0;return true;}if(!Ȫ) return false;ю Ė=null;float ΕΔ=0;for(int i=0;i<Ѝ.Count;i++){ю p=Ѝ[i];if(p.ν==ф||ΒΦ(p.ν,Γυ)) continue;float d=Vector3.Distance(p.η,Θ.Δ);if(Ė==null||d<ΕΔ){Ė=p;ΕΔ=d;}
}if(Ė==null) return false;ȃ=Ė.ν;ї=ΑБ(Ė.η,Ė.Б,Ė.Ϗ);return true;}String Ζφ(){String s=(Ȫ?"1":"0")+":"+υ(ΑΜ)+":"+υ(ΑΛ)+":"+
υ(ΑΝ)+":"+υ(ΑΞ)+":"+ф;for(int i=0;i<Ѝ.Count;i++)
s+=":"+Ѝ[i].ν+","+υ(Ѝ[i].η)+","+υ(Ѝ[i].Б)+","+υ(Ѝ[i].Ϗ);return s;}void Ζυ(String s){Ѝ.Clear();Ȫ=false;ф=0;String[]f=s.Split(':');if(f.Length<6) return;Ȫ=f[0]=="1";ΑΜ=λ(f[1]);ΑΛ=λ(f[2]);ΑΝ=λ(f[3]);ΑΞ=λ(f[4]);long.TryParse(f[5],out ф);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ю p=new ю();if(!long.TryParse(e[0],out p.ν)) continue;p.η=λ(e[1]);p.Б=λ(e[2]);p.Ϗ=λ(e[3]);Ѝ.Add(p);}}const String ΑЎ="[PAMDCK]";const float ΘЋ=120f;class Δί{public long Ж;public int ƍ;public long ΓΦ;public bool ΙΘ;public DateTime ΙΫ,Ιή;
}IMyBroadcastListener ΙΨ=null;List<Δί>ΙΩ=new List<Δί>();List<IMyShipConnector>Ιδ=new List<IMyShipConnector>();List<IMySensorBlock>Ιμ=new List<IMySensorBlock>();List<IMyCameraBlock>Ιέ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ιλ=new List<MyDetectedEntityInfo>();long ΙΛ=0;
int ΙΚ=0;long ΙΙ=0;DateTime ΙΜ=DateTime.Now;String ΙΧ="";int ΙΪ=0;IMyCubeGrid Ιε=null;IMyBroadcastListener Αχ=null;int ȁ=0;DateTime ΕΧ=DateTime.Now;DateTime ΖΗ=DateTime.MinValue;long ΖΘ=0;int ΖΙ=0;long Ś=0;bool ƥ=false;int ΓΓ=0;List<Vector3>Ƨ=new List<Vector3>();List<float>ΔΞ=new List<float>();
bool ľ=false;bool ΑΚ=false;Vector3 ΕΦ=new Vector3();bool ų(){return ȁ!=0&&(DateTime.Now-ΖΗ).TotalSeconds<3;}bool Ȥ(int ƍ){return ų()&&ΖΘ==ǚ&&ΖΙ==ƍ;}bool ΔЁ(){return ų()||(DateTime.Now-ΕΧ).TotalSeconds<3;}void Βκ(int ƍ){if(ȁ==ƍ) return;ȁ=ƍ;ΕΧ=DateTime.Now;ΓΓ=3;
}void Βι(){if(ȁ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΑЎ,"D;"+ǚ);ȁ=0;ƥ=false;}void Ηή(){if(!trafficEnabled||Η==Ζ.Ϊ) return;if(Αχ==null) Αχ=IGC.RegisterBroadcastListener(ΑЎ);int ΒЈ=20;while(Αχ.HasPendingMessage&&ΒЈ-->0){String ά=Αχ.AcceptMessage().Data as String;if(ά==null||!ά.StartsWith("S;")) continue;
try{ΗΑ(ά);}catch{}}if(Φ!=Α.І){ľ=false;ΑΚ=false;}if(ȁ==1&&З(MyShipConnectorStatus.Connected)!=null) Βι();if(ȁ==2&&!ƥ&&Φ!=Α.ǎ&&
Vector3.Distance(β,ж().Δ)>θ*3) Βι();if(ȁ!=0&&++ΓΓ>=3){ΓΓ=0;IGC.SendBroadcastMessage(ΑЎ,"R;"+ǚ+";"+ȁ+";"+ф+";"+(Ȫ?1:0));}}void ΗΑ(String ά){String[]f=ά.Split(';');if(f.Length<7) return;bool Εα=false;String[]ΔЈ=f[5].Split(',');for(int i=0;i<ΔЈ.Length;i++) if(ΔЈ[i]==ǚ.ToString()) Εα=true;
if(!Εα) return;ΖΗ=DateTime.Now;ΖΘ=long.Parse(f[2]);ΖΙ=int.Parse(f[3]);Ś=long.Parse(f[4]);Ƨ.Clear();ΔΞ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ƨ.Add(λ(e[0]));ΔΞ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΕΩ=f[6].Split(':');List<ю>С=new List<ю>();for(int i=0;i<ΕΩ.Length;i++){String[]e=ΕΩ[i].Split(',');if(e.Length<4) continue;ю p=new ю();p.ν=long.Parse(e[0]);p.η=λ(e[1]);p.Б=λ(e[2]);p.Ϗ=λ(e[3]);С.Add(p);}Ѝ=С;}bool Ζκ(){if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return true;
Βκ(1);if(!ΔЁ()) return true;if(!Ȥ(1)){Μ="Waiting for dock clearance";return false;}ΑЄ();return true;}bool ΑЄ(){Ƃ=false;ю Ģ=null;if(Ś!=0&&Ś!=ф&&Ȫ)
for(int i=0;i<Ѝ.Count;i++) if(Ѝ[i].ν==Ś) Ģ=Ѝ[i];long ΔΖ=Ģ==null?0:Ģ.ν;if(ΔΖ==ȃ&&(ΔΖ==0)==(ї==null)) return false;ȃ=ΔΖ;ї=Ģ==null?null:ΑБ(Ģ.η,Ģ.Б,Ģ.Ϗ);return true;}bool İ(Vector3 p){for(int i=0;i<Ƨ.Count;i++) if(Vector3.Distance(p,Ƨ[i])<ΔΞ[i]+θ*0.75f) return true;
return false;}bool ΖЂ(Vector3 ž,Vector3 ŏ){ľ=false;if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return false;if(ȁ!=1&&Vector3.Distance(β,Θ.Δ)>dockRequestDistance) return false;Βκ(1);if(!ų()||Ȥ(1)||Ƨ.Count==0) return false;if(!İ(ž)&&!İ(ŏ)&&!İ(β)) return false;ľ=true;return true;
}bool ΔΨ(){if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return true;Βκ(2);if(!ΔЁ()) return true;if(!Ȥ(2)){Μ="Waiting for undock clearance";return false;}return true;}void Ζς(string ѐ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}