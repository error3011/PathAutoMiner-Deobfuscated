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
public enum Ζ{ǌ,ű,Ǎ,р,Ϊ}public enum Α{Я,Ȥ,Ų,Ȍ,Đ,І,Ю,
ȣ,Ĳ,Ǡ,č,ĉ,ȟ,
ų,ǹ,ǐ,Č,ΑΒ,Ƴ,
ΒϏ,ΑΑ,ũ,ƴ,ȧ}public enum Λ{з,Ĵ,Є,ΒΚ,Ư,ƾ,ƿ,Į,į}public enum п{None,Č,Я,Δε}public enum Ω{Main,ΔЇ,Ű,ů,Βϋ,ƭ,
ΑЋ,Δυ,Δφ,Δτ,Ηη,Ιά,
ΙΪ,ΙΫ,Βφ,ΔЋ,Ǉ}public enum б{ǅ,ŧ,Ηι,Δψ,Ϊ}public enum Ǐ{ǉ,Ζϋ}public enum е{Ǆ,ΒΠ,ΔЍ,ū}public enum Ϗ{Βδ,Δβ,ΒΕ,ΒΛ,Ʊ,Βέ,Ƹ}public enum э{ΒΗ,ΑА,ΑБ}public enum И{Ŭ,ΗΘ,Ċ}public enum ι{Βζ,Βκ,Βλ,ΔЄ,ΔЃ,
Βε,ǂ,ǁ,Βθ,
Βι,Βη}public enum ί{ǲ,ȋ,Ǳ,Ƭ,ΑЎ,ΑЏ}public enum ķ{Δκ,ΔΩ,ΙΟ}public enum с{ǈ,Δθ,ΗΤ,Βγ}public enum ǳ{None,Βο,ΗΩ,ΗΣ}public enum σ{Ÿ,ƫ,Ƽ,ū,Βα,ƽ,Ββ,Ηξ,Βμ}public enum ΙΤ{ǌ,ΙΦ,ΙΥ,ΙΨ,ΙΠ,ΙΞ,ΙΧ}class Χ{public bool γ=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 χ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ц=new Vector3();public float Ў=0;public float ΙЅ=0;public float[]Ǟ=null;public Χ(){}public Χ(Χ δ){γ=δ.γ;Δ=δ.Δ;α=δ.α;β=δ.β;χ=δ.χ;ν=δ.ν;
Ц=δ.Ц;Ǟ=δ.Ǟ;}public Χ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 χ,Vector3 ν){this.Δ=Δ;this.α=α;this.β=β;this.χ=χ;this.Ў=0;this.ν=ν;}public void Ζψ(List<IMyThrust>ΑΕ,List<string>Ȇ){Ǟ=new float[Ȇ.Count];for(int i=0;i<Ǟ.Length;i++)
Ǟ[i]=-1;for(int i=0;i<ΑΕ.Count;i++){string ь=Δς(ΑΕ[i]);int Β=Ȇ.IndexOf(ь);if(Β!=-1)
Ǟ[Β]=Џ(ΑΕ[i].MaxEffectiveThrust,ΑΕ[i].MaxThrust);}}}class Βξ{public Χ Љ=null;public List<Vector3>ş=new List<Vector3>();public float Ƅ=0;public float ΔΜ=0;public float ļ=0;public float ΓΣ=0;public Vector3 ΑΧ=new Vector3();}class ĸ{public ι Ћ=ι.Βζ;public float ǔ=0;public float Ȯ=0;
public string Ǖ="";public string ǘ="";DateTime ΕЅ;public bool ΓЀ=false;private bool ΓΖ=false;public bool ΒΒ(bool ȅ){if(ȅ){Ȯ=0;ΓΖ=false;return false;}ΓΖ=true;return Ȯ>ǔ;}public void ΗΪ(){ΒΒ(true);ΓЀ=false;}public void Ηϊ(){if(ΓΖ)
if((DateTime.Now-ΕЅ).TotalSeconds>1){Ȯ++;ΕЅ=DateTime.Now;}}public bool ΒΪ(){switch(Ћ){case ι.Βκ:return true;case ι.Βλ:return true;}return false;}}class ю{public String υ="";public String ς="";public int Ǒ=0;public И ō=И.Ċ;public ю(String υ,String ς,int Ǒ,И ō){this.υ=υ;this.ς=ς;
this.Ǒ=Ǒ;this.ō=ō;}}class ΖЇ{public ΖЇ(Vector3 η,float ό){this.η=η;this.ό=ό;}public Vector3 η;public float ό;}class Ķ{public DateTime ΘΨ;public DateTime ΕΥ;public String μ="";public String υ="";public String ΙЇ="";public String Μ="";public String ΙЁ="";public String Ιϊ="";public String Γώ="";
public Vector3 Δ=new Vector3();public List<ю>Ъ=new List<ю>();public Ζ Η=Ζ.ǌ;public ǳ ȩ;public float Ő;public float Ł;public int і;public int Ιξ=0;public int ΙЄ=0;public bool ΙΡ(){return(DateTime.Now-ΘΨ).TotalSeconds>10;}public bool ΗΛ(){if(ȩ!=ǳ.Βο) return false;return(DateTime.Now-ΕΥ).TotalSeconds>=4;
}public Ķ(String μ){this.μ=μ;}public ǳ ΙΩ(String Ę,bool Θω,bool Ǔ,bool ΗЈ){if(Ę==""&&!Ǔ) return ǳ.None;if(ȩ==ǳ.Βο&&ΗΛ())
ȩ=ǳ.ΗΣ;if(Θω){Γώ=Ę;ȩ=ǳ.Βο;ΕΥ=DateTime.Now;return ȩ;}else if(Ǔ){Γώ="";ȩ=ǳ.None;}else if(Γώ==Ę){if(ΗЈ) ȩ=ǳ.ΗΩ;return ȩ;}return ǳ.None;}}IMyGridTerminalSystem ω;Vector3 Αυ=new Vector3();Vector3 ΰ=new Vector3();Vector3 ΑΪ=new Vector3();Vector3 ƒ=new Vector3();Vector3 Ƈ=new Vector3();
DateTime ΕЄ=new DateTime();bool Ɗ=true;int ΔΓ=0;bool ā=false;bool Αβ=false;bool ƕ=true;bool Ζσ=false;bool Ŋ=false;bool Αί=false;float Ț=0;float Ĩ=0;int Ą=0;int Ƥ=0;int Αψ=0;int ΒЎ=0;float ŏ=0;float Ľ=0;double Γτ=0;float ż=0;List<int>Ň=new List<int>();List<int>Ś=new List<int>();
Ζ Η=Ζ.ǌ;int ύ=0;float ǯ=0;float Ǯ=0;float θ=0;float Αϊ=0,Αω=0;IMyRemoteControl Υ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΖ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>ȇ=new List<IMyThrust>();List<IMyGyro>ΑΫ=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>ȃ=new List<IMyLandingGear>();List<IMyReactor>ă=new List<IMyReactor>();List<IMyConveyorSorter>Αύ=new List<IMyConveyorSorter>();List<IMyGasTank>ѓ=new List<IMyGasTank>();List<IMyTerminalBlock>Ź=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ŀ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>є=new List<IMyTextPanel>();List<IMyTextSurface>ą=new List<IMyTextSurface>();List<IMyTextPanel>ƃ=new List<IMyTextPanel>();
IMyTerminalBlock љ=null;Χ Θ=new Χ();Χ Γα=new Χ();Χ Γδ=new Χ();bool Ȉ=false;bool š=false;bool Ţ=false;bool Αα=false;List<String>Ȇ=new List<string>();List<Χ>Σ=new List<Χ>();List<Χ>Ʃ=new List<Χ>();int ΓΛ=0;double Ζτ=0;Χ Ε=new Χ();Vector3 Ǜ;Vector3 ȗ;Λ ή=Λ.з;Ǐ ġ=Ǐ.ǉ;int Н=0;
int в=0;double Ə=0;bool ΑΝ=false;ĸ ϊ=new ĸ();ĸ ϋ=new ĸ();е л=е.Ǆ;bool ć=true;Ǐ ț=Ǐ.ǉ;int й=3;int и=3;int Х=30;э у=э.ΒΗ;bool Ȟ=true;bool ŉ=true;bool Ń=true;bool ĩ=false;Ϗ М=Ϗ.Βδ;bool ȝ=true;float ǩ=90;float ч=20;float Ы=5;float ш=20;float Ǥ=0.70f;float ť=1.50f;
float Ť=2.50f;float ĭ=0f;float ĝ=0f;Α Φ;Λ ц;Α ƞ;Βξ ε=null;int Ш=0;int Щ=0;int К=0;int ȕ=0;int Ħ=30;int Т=0;bool Ё=true;Vector3 щ;double Л=0;bool Αχ=false;int ΔΠ=0;int Ђ=0;int ΒІ=0;int Ǫ=0;int ΖΗ=0;Vector3 Ɨ=new Vector3();float к=0;float Э=0;int Ĭ=0;Χ Αώ=null;
float Γη=0;float ΑΗ=0;float Р=0;float Ĥ=0;bool Γβ=false;bool Ī=false;bool ƥ=false;bool Ȓ=false;bool Ȏ=false;DateTime ΘΕ=new DateTime();Χ ę=null;Χ ΕΪ=null;float Ǻ=0;п ǥ;float ȓ=0;float ī=0;String Γο="";float Αδ=0;float Ł=0;float Ő=0;List<ю>Ǘ=new List<ю>();float О=0;
bool Ɯ=false;bool Ζθ=false;bool ΖΥ=false;bool ΑЁ=false;bool ΖΦ=false;float Γς=0;float Γύ=0;Vector3 Εξ=new Vector3();Vector3 ψ=new Vector3();Vector3 ΑЂ=new Vector3(1,1,1);float ΔΒ=1;Vector3 ơ=new Vector3();bool Αΰ=false;bool ΓΩ=false;bool Γμ=false;bool ΕΕ=false;float ΑΛ=0;
float ΑΔ=2;Vector3 Ζζ;Vector3 ΔΑ;Vector3 ΓБ;float[,]ǟ=new float[3,2];Dictionary<String,float[,]>ќ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζρ=new Dictionary<String,float>();int ΒЉ=0;Dictionary<String,float[]>Ș=new Dictionary<string,float[]>();float ΓЁ;bool ΑΞ=false;
int ΑΖ=0;int Ȳ=0;int Αέ=0;int ğ=0;String Μ="";int[]ΕБ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΖΑ=false;int Αξ=0;int і=0;Ω Ќ=Ω.Main;int ΘЅ=0,ΘІ=0,ΘЇ=0,ΘЈ=0;int Θυ=0,Θφ=0;bool Ŀ=false;bool ΕΖ=false;String ΓЈ="";String ΒЍ="";IMyBroadcastListener ȏ=null;
bool Εν=true;List<Ķ>Ιζ=null;Ķ Ιύ=null;int Ιη=0,Ιθ=0;int Ιι=0,Ικ=0;int Ιλ=0,Ιμ=0,Ιν=0;String ΘΧ="";Exception Ȕ=null;bool Αφ=false;Λ Ŝ=Λ.з;bool Εέ=false;bool ΕЃ=true;bool ΓІ=false;String Ζς="";bool ƣ=false;bool ΓЍ=true;int Αή=0;int[]ΙЀ=null;Vector3 ΒЏ=new Vector3();
bool Ετ=false;bool ǎ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΫ(Me,controllerTag,true))
Η=Ζ.р;π(Ω.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЅ=ΗΫ();if(Η==Ζ.ǌ){List<IMyShipDrill>Εθ=new List<IMyShipDrill>();List<IMyShipGrinder>Ευ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Εθ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Ευ,q=>q.CubeGrid==Me.CubeGrid);if(Εθ.Count>0)
{Η=Ζ.ű;Μ="Miner mode enabled!";}else if(Ευ.Count>0){Η=Ζ.Ǎ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϊ;Ƿ(б.Ϊ);}}if(ΓЅ==с.ǈ) ƕ=false;if(ΓЅ==с.Δθ) Μ="Data restore failed!";if(ΓЅ==с.Βγ) Μ="New version, wipe data";}}void Main(string ё,UpdateType ΘА){try{if(Ȕ!=null){if(ё.Trim().ToUpper()=="RETRY")
{Ȕ=null;Ɗ=true;Ą=0;if(Η!=Ζ.р) φ();Μ="Restarted after error";}else{ΗΓ();ΔЅ();return;}}Αβ=(ΘА&UpdateType.Update10)!=0;if(Αβ) ΔΓ++;ā=ΔΓ>=10;if(ā) ΔΓ=0;if(Αβ){ΑΖ++;if(ΑΖ>4) ΑΖ=0;Αψ=(int) Math.Max(0,10-(DateTime.Now-ΕЄ).TotalSeconds);}if(ё!="") ΘΧ=ё;if(Η!=Ζ.р)
Ηθ(ё);else
ΖЅ(ё);ΖΑ=false;try{int ΕΫ=Runtime.CurrentInstructionCount;float Ƙ=Џ(ΕΫ,Runtime.MaxInstructionCount);if(Ƙ>0.90) Μ="Max. instructions >90%";if(Ƙ>ŏ) ŏ=Ƙ;if(ΑΞ){Ň.Add(ΕΫ);while(Ň.Count>10) Ň.RemoveAt(0);Ľ=0;for(int i=0;i<Ň.Count;i++) Ľ+=Ň[i];Ľ=Џ(Џ(Ľ,Ň.Count),Runtime.MaxInstructionCount);
double Γθ=Runtime.LastRunTimeMs;if(Αί&&Γθ>Γτ) Γτ=Γθ;Ś.Add((int)(Γθ*1000f));while(Ś.Count>10) Ś.RemoveAt(0);ż=0;for(int i=0;i<Ś.Count;i++) ż+=Ś[i];ż=Џ(ż,Ś.Count)/1000f;}}catch{ŏ=0;}}catch(Exception e){Ȕ=e;}}void Ηθ(string ё){bool Αϋ=false;String ΕΡ="";if(ύ<=1&&!Δϋ(ё))
ΔІ(ё);int Θβ=50;while(ȏ!=null&&ȏ.HasPendingMessage&&Θβ-->0){MyIGCMessage ρ=ȏ.AcceptMessage();String Ϋ=ρ.Data as String;if(Ϋ==null) continue;String ƛ="";if(ύ<=1&&ΗΥ(ref Ϋ,out ƛ,out ΕΡ)&&ƛ==BROADCAST_SELF_ID){ΔІ(Ϋ);Αϋ=true;break;}}bool ΕϏ=Αί&&Φ==Α.Я&&!Ŋ&&!Αϋ&&Ą==0&&!Αα;
if(ā&&Φ!=Α.Я) Αϋ=true;if((Αβ&&!ΕϏ)||(ā&&ΕϏ)){if(Ą==0&&(Αψ<=0||Ɗ)){Ζσ=ύ>0;ύ=0;Ą=1;Ǹ();Ηή();ΗΙ();Ǵ("Scan 1");}else if(Ą==1){Ą=2;Ǹ();Ηί();Ǵ("Scan 2");}else if(Ą==2){Ą=0;Ǹ();Ηΰ();Ηα();if(ύ<=1) ΔϏ();Ǵ("Scan 3");ΕЄ=DateTime.Now;if(ύ<=1&&ƕ)
Αυ=ƶ(Υ,Υ.CenterOfMass);ƕ=false;if(Ɗ){Ɗ=false;bool ΙΓ=ή==Λ.Є;φ();if(ΕЃ&&resumeAfterRestart&&ΙΓ&&ύ<=1) ΓІ=true;}ΕЃ=false;if(Ζσ&&ύ==0) Μ="Setup complete";}else{if(ή==Λ.Є&&Η!=Ζ.Ϊ){Ǹ();Ζυ();Ǵ("Inv balance");}Ǹ();switch(Ƥ){case 0:Ης();break;case 1:Ηφ();break;case 2:Ηρ();break;
case 3:Ηύ();break;case 4:Ηυ();break;case 5:Ζό();break;case 6:Ζϊ(Υ);break;}Ǵ("Update: "+Ƥ);Ƥ++;if(Ƥ>6){Ƥ=0;Αί=true;if(Ŝ!=Λ.з){switch(Ŝ){case Λ.Į:ȡ();break;case Λ.į:ȡ();break;case Λ.Є:ȡ();break;case Λ.ƾ:Ƶ();break;case Λ.ƿ:Ď();break;}Ŝ=Λ.з;}}}if(!Ɗ){if(!Ƕ(Υ,true)){Υ=null;
ƕ=true;ύ=2;}if(ύ>=2&&Φ!=Α.Я)
φ();if(ύ<=1){Ĩ=Υ.CalculateShipMass().PhysicalMass;Ț=(float) Υ.GetShipSpeed();ΰ=Ηπ(Υ,Αυ);ΑΪ=Υ.WorldMatrix.Forward;ƒ=Υ.WorldMatrix.Left;Ƈ=Υ.WorldMatrix.Down;Ηω();Ηϋ();Ησ();if(ΓІ){ΓІ=false;ȡ();if(Φ!=Α.Я) Μ="Job resumed after restart";}if(Φ!=Α.Я){Ŋ=false;ΔЊ(false);Δέ(false);String Θώ=ΒΦ(Φ)+" "+(int) Φ;
Ǹ();Ηχ();Βώ(false);Ǵ(Θώ);Ηό();Ηψ();Ǹ();Ητ();Ǵ("Thruster");Ǹ();ΕΑ();Ǵ("Gyroscope");}else{if(Ŋ){if(ƺ()){Ф(Ƈ,ΑΪ,ƒ,0.25f,true);ΕΑ();Μ="Aligning to planet: "+Math.Round(ΑΛ-0.25f,2)+"°";if(Αΰ) ΒΩ(true,true);}else ΒΩ(true,true);}}ƣ=false;}}}Ǹ();ΔЅ();Ǵ("Print");if(Αϋ||ΒЎ<=0){Ǹ();Ζφ(ΕΡ);
Ǵ("Broadcast");ΒЎ=4;}else if(ā) ΒЎ--;}void ΔІ(string ё){if(ё=="") return;var Д=ё.ToUpper().Split(' ');Д.DefaultIfEmpty("");var Ę=Д.ElementAtOrDefault(0);var Ė=Д.ElementAtOrDefault(1);var Ż=Д.ElementAtOrDefault(2);var ΕΙ=Д.ElementAtOrDefault(3);String ȳ="Invalid argument: "+ё;bool ΔΛ=false;
switch(Ę){case "UP":this.ΔЁ(false);break;case "DOWN":this.ΔЀ(false);break;case "UPLOOP":this.ΔЁ(true);break;case "DOWNLOOP":this.ΔЀ(true);break;case "APPLY":this.ΔΫ(true);break;case "MRES":і=0;break;case "STOP":this.φ();break;case "PATHHOME":{this.φ();this.Βϊ();}break;case "PATH":{this.φ();this.Βϊ();Θ.γ=true;}break;
case "START":{this.φ();Βω();}break;case "ALIGN":{ΒΩ(!Ŋ,false);}break;case "CONT":{this.φ();this.ȡ();}break;case "JOBPOS":{this.φ();this.Ď();}break;case "HOMEPOS":{this.φ();this.Ƶ();}break;case "FULL":{Ɯ=true;}break;case "RESET":{Αφ=true;ύ=2;}break;case "EXPORT":{ΖЌ();}break;
case "IMPORT":{ΗΖ(Ė=="JOB");}break;default:ΔΛ=true;break;}if(Η!=Ζ.Ϊ){switch(Ę){case "SHUTTLE":{ΖЊ();}break;case "CFGS":{if(!ΖЃ(Ė,Ż,ΕΙ)) Μ=ȳ;}break;case "CFGB":{if(!ΖЂ(Ė,Ż)) Μ=ȳ;}break;case "CFGL":{if(!ď(ref ǩ,true,σ.Ƽ,Ė,"")||!ΖЄ(Ż))
Μ=ȳ;}break;case "CFGE":{if(!ď(ref Ы,true,σ.ƽ,Ė,"IG")||
!ď(ref ч,true,σ.Βα,Ż,"IG")||
!ď(ref ш,true,σ.Ββ,ΕΙ,"IG"))
Μ=ȳ;}break;case "CFGA":{if(!ď(ref Ǥ,false,σ.ƫ,Ė,"")) Μ=ȳ;}break;case "CFGW":{if(!ď(ref ť,false,σ.Ÿ,Ė,"")||
!ď(ref Ť,false,σ.Ÿ,Ż,""))
Μ=ȳ;}break;case "NEXT":{ΑЊ(false);}break;case "PREV":{ΑЊ(true);}break;default:if(ΔΛ) Μ=ȳ;break;}}else{switch(Ę){case "UNDOCK":{ƣ=true;}break;default:if(ΔΛ) Μ=ȳ;break;}}}String ΖА(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΖЊ(){φ();Η=Ζ.Ϊ;Ƿ(б.Ϊ);Ε.γ=false;Θ.γ=false;ξ=null;ο.Clear();ή=Λ.з;}void ΒΩ(bool Ї,bool ф){if(!Ї) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ї){Ŋ=false;ΔЎ();ǆ(false,0,0,0,0);return;}if(ƺ()) Ŋ=true;}bool ΖЃ(String ΙΕ,String ΘΞ,String ΗБ)
{bool ΙΑ=ή==Λ.Є;int w,h,d;if(int.TryParse(ΙΕ,out w)&&int.TryParse(ΘΞ,out h)&&int.TryParse(ΗБ,out d)){this.φ();й=w;и=h;Х=d;ē(false);ŷ(false,false);if(ΙΑ) ȡ();return true;}return false;}bool ΖЄ(String ΕΘ){if(ΕΘ=="ON"){Ȟ=true;return true;}if(ΕΘ=="OFF"){Ȟ=false;return true;}
return false;}bool ΖЂ(String Εη,String ΑΜ){bool Ɩ=true;if(Εη=="HOME") ć=true;else if(Εη=="STOP") ć=false;else Ɩ=false;if(ΑΜ=="HOME") л=е.Ǆ;else if(ΑΜ=="STOP") л=е.ΔЍ;else if(ΑΜ=="JOB") л=е.ΒΠ;else if(ΑΜ=="IG") л=е.ū;else Ɩ=false;return Ɩ;}void π(Ω Γϋ){ΕБ[(int) Ќ]=і;
і=ΕБ[(int) Γϋ];if(Γϋ==Ω.Ηη) і=0;Ќ=Γϋ;if(Η!=Ζ.р)
Ē(Ќ==Ω.Ű,false,0,0);ΖΑ=true;}void ΔЁ(bool Γν){if(і>0) і--;else if(Γν) і=Αξ-1;}void ΔЀ(bool Γν){if(і<Αξ-1) і++;else if(Γν) і=0;}String Ƿ(б ρ){switch(ρ){case б.ǅ:Μ="Job is running";break;case б.ŧ:Μ="Connector not ready!";break;case б.Ηι:Μ="Ship modified, path outdated!";break;
case б.Δψ:Μ="Interrupted by player!";break;case б.Ϊ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΒ(Ǐ η){switch(η){case Ǐ.ǉ:return "Top-Left";case Ǐ.Ζϋ:return "Center";default:return "";}}String ΖЎ(э Ɣ){switch(Ɣ){case э.ΑА:return "Auto"+(Η==Ζ.ű?" (Ore)":"");case э.ΑБ:return "Auto (+Stone)";
case э.ΒΗ:return "Default";default:return "";}}String ΒΤ(Λ У){switch(У){case Λ.з:return "No job";case Λ.Ĵ:return "Job paused";case Λ.Є:return "Job active";case Λ.Į:return "Job active";case Λ.į:return "Job active";case Λ.ΒΚ:return "Job done";case Λ.Ư:return "Job changed";case Λ.ƾ:return "Move home";
case Λ.ƿ:return "Move to job";}return "";}String Δν(е ΗЇ){switch(ΗЇ){case е.Ǆ:return "Return home";case е.ΒΠ:return "Fly to job pos";case е.ΔЍ:return "Stop";case е.ū:return "Ignore";}return "";}String ΖЏ(Ϗ Ɣ){switch(Ɣ){case Ϗ.Βδ:return "Off";case Ϗ.ΒΛ:return "Drop pos (Stone) ";case Ϗ.Ʊ:return "Drop pos (Sto.+Ice)";
case Ϗ.Δβ:return "Cur. pos (Stone)";case Ϗ.ΒΕ:return "Cur. pos (Sto.+Ice)";case Ϗ.Βέ:return "In motion (Stone)";case Ϗ.Ƹ:return "In motion (Sto.+Ice)";}return "";}String Δλ(п У){switch(У){case п.None:return "No batteries";case п.Č:return "Charging";case п.Δε:return "Discharging";}return "";}String ΒΦ(Α У)
{String Ιτ=Η==Ζ.Ϊ?"target":"job";switch(У){case Α.Я:return "Idle";case Α.Ȥ:return "Flying to XY position";case Α.Ų:return Η==Ζ.Ǎ?"Grinding":"Mining";case Α.Ȍ:return "Returning";case Α.ũ:return "Flying to drop pos";case Α.Đ:return "Returning to dock";case Α.І:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȣ:return "Flying to path";case Α.Ĳ:return "Flying to job position";case Α.Ǡ:return "Approaching dock";case Α.č:return "Docking";case Α.ĉ:return "Aligning";case Α.ȟ:return "Aligning";case Α.ų:return "Retry docking";case Α.ǹ:return "Unloading";case Α.ȧ:return Ζς;
case Α.ǐ:return "Undocking";case Α.Č:return "Charging batteries";case Α.ΑΒ:return "Waiting for uranium";case Α.Ƴ:return "Filling up hydrogen";case Α.ΒϏ:return "Waiting for ejection";case Α.ΑΑ:return "Waiting for ejection";case Α.ƴ:return "Flying to drop pos";}return "";}String Δσ(ι Ћ){switch(Ћ)
{case ι.Βζ:return "On \"Undock\" command";case ι.Βκ:return "On player entered cockpit";case ι.ΔЄ:return "On ship is full";case ι.ΔЃ:return "On ship is empty";case ι.Βλ:return "On time delay";case ι.ǂ:return "On batteries empty(<25%)";case ι.ǁ:return "On batteries empty(=0%)";case ι.Βε:return "On batteries full";
case ι.Βι:return "On hydrogen empty(<25%)";case ι.Βη:return "On hydrogen empty(=0%)";case ι.Βθ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΠ,int ȱ,bool Ξ,String Ƒ){Αξ+=1;if(ΖΠ==ȱ) Ƒ=">"+Ƒ+(ΑΖ>=2?" .":"");else Ƒ=" "+Ƒ;Γ+=Ƒ+"\n";return ΖΠ==ȱ&&Ξ;}String ΔΫ(bool Ξ)
{int Β=0;int Ο=і;Αξ=0;String ΖΤ="———————————————\n";String Š="--------------------------------------------\n";String Γ="";Γ+=ΒΤ(ή)+" | "+(Θ.γ?"Ready to dock":"No dock")+"\n";Γ+=ΖΤ;double ǫ=Math.Max(Math.Round(this.Л),0);if(Ќ==Ω.Main){bool Ā=Η==Ζ.Ϊ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βϊ();
if(Η==Ζ.ű)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ω.Ű);if(Η==Ζ.Ǎ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ω.Ű);if(Η==Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ω.Βφ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȡ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) Ƶ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ď();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ā) π(Ω.Ǉ);else π(Ω.ƭ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ω.Δυ);if(Η!=Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ω.Δτ);}else if(Ќ==Ω.Ű){double ΙΔ=Math.Round(й*ǯ,1);double ΘΝ=Math.Round(и*Ǯ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) Βω();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){ŷ(false,false);π(Ω.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΔ+"m)")){İ(ref й,5,20,1);ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){İ(ref й,-5,20,-1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΘΝ+"m)")){İ(ref и,5,20,1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){İ(ref и,-5,20,-1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(у==э.ΒΗ?"Depth":"Min")+": "+Х+"m)")){İ(ref Х,5,50,2);ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){İ(ref Х,-5,50,-2);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+ΖЎ(у))){у=Ȣ(у);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΗΒ(ț))){ț=Ȣ(ț);}if(Η==Ζ.Ǎ&&у==э.ΑБ) у=Ȣ(у);Γ+=ΔЈ(8,Ѓ,Ο,ref Θυ);}else if(Ќ==Ω.Βφ){float[]Εί=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.ΔЋ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δσ(ϊ.Ћ))) ϊ.Ћ=Ȣ(ϊ.Ћ);if(!ϊ.ΒΪ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΣ((int) ϊ.ǔ))) ϊ.ǔ=Δγ(ϊ.ǔ,Εί);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δσ(ϋ.Ћ))) ϋ.Ћ=Ȣ(ϋ.Ћ);
if(!ϋ.ΒΪ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΣ((int) ϋ.ǔ))) ϋ.ǔ=Δγ(ϋ.ǔ,Εί);}else if(Ќ==Ω.ΔЋ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βω();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Βφ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǖ!=""?ϊ.Ǖ:"-"))) ϊ.Ǖ=ǀ(ref ΘЅ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǘ!=""?ϊ.ǘ:"-"))) ϊ.ǘ=ǀ(ref ΘЇ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǖ!=""?ϋ.Ǖ:"-"))) ϋ.Ǖ=ǀ(ref ΘІ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǘ!=""?ϋ.ǘ:"-"))) ϋ.ǘ=ǀ(ref ΘЈ);
}else if(Ќ==Ω.ů){String ΕЉ=ǩ+" %";if(ā) Ȳ++;if(Ȳ>1){Ȳ=0;Αέ++;if(Αέ>1) Αέ=0;}bool[]ΓЊ=new bool[]{ă.Count==0,ǥ==п.None,ѓ.Count==0};int Ζν=0;while(true){Ζν++;ğ++;if(ğ>ΓЊ.Length-1) ğ=0;if(Ζν>=ΓЊ.Length) break;if(!ΓЊ[ğ]) break;}bool Ā=Η==Ζ.Ϊ;if(!Ā&&Ȟ&&О!=-1&&Αέ==0)
ΕЉ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){φ();π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ā) π(Ω.ƭ);else π(Ω.Ǉ);if(!Ā){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЊ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) ƣ=true;Γ+=Š;if(!Ā) Γ+="Progress: "+Math.Round(Ə,1)+" %\n";Γ+="State: "+ΒΦ(Φ)+" "+ǫ+"m \n";Γ+="Load: "+Αδ+" % Max: "+ΕЉ+" \n";if(ğ==0) Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ğ==1) Γ+="Battery: "+(ǥ==п.None?Δλ(ǥ):Ǻ+"% "+(ч==-1||Ā?"":" Min: "+ч+" %"))+"\n";if(ğ==2) Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(ȓ,1)+"% "+(ш==-1||Ā?"":" Min: "+ш+" %"))+"\n";}else if(Ќ==Ω.ƭ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.ů);else π(Ω.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+ǩ+"%")) д(ref ǩ,ǩ<=80?-10:-5,σ.Ƽ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(Ȟ?"On":"Off"))) Ȟ=!Ȟ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+ΖЏ(М))) М=Ȣ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(ĩ?"On":"Off"))){ĩ=!ĩ;if(ĩ) ǣ(ΓЍ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(ȝ?"On":"Off"))) ȝ=!ȝ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) д(ref Ы,(Ы>5?-5:-1),σ.ƽ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) д(ref ч,-5,σ.Βα,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) д(ref ш,-10,σ.Ββ,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(ć?"Return home":"Stop"))) ć=!ć;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δν(л))) л=Ȣ(л);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ω.ΑЋ);Γ+=ΔЈ(8,Ѓ,Ο,ref Θφ);}else if(Ќ==Ω.ΑЋ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.ů);else π(Ω.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǎ?" Grinder":" Drill")+" inv. balancing: "+(ŉ?"On":"Off"))) ŉ=!ŉ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǎ?" grinders":" drills")+": "+(Ń?"Fwd + Bwd":"Fwd"))) Ń=!Ń;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ť+"m/s")) д(ref ť,0.5f,σ.Ÿ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ť+"m/s")) д(ref Ť,0.5f,σ.Ÿ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) д(ref Ǥ,0.1f,σ.ƫ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ĭ*100f+"%")) Δί(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ĝ*100f+"%")) Δί(false,0.05f);
}else if(Ќ==Ω.Ǉ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.ů);else π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǩ+"%")) д(ref ǩ,ǩ<=80?-10:-5,σ.Ƽ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȝ?"On":"Off"))) ȝ=!ȝ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) д(ref Ы,(Ы>5?-5:-1),σ.ƽ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δν(л))) л=Ȣ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) д(ref Ǥ,0.1f,σ.ƫ,false);
}else if(Ќ==Ω.ΔЇ){double ΘΩ=0;if(Σ.Count>0) ΘΩ=Vector3.Distance(Σ.Last().Δ,ΰ);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΔЏ();if(Η!=Ζ.Ϊ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȉ?"Use old home":(Θ.γ?"Was set! ":"none ")))) Ȉ=!Ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȉ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) Ȉ=!Ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(š?"Use old connector":(Ε.γ?"Was set! ":"none ")))) š=!š;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ţ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ţ=!Ţ;Γ+=Š;Γ+="Wp spacing: "+Math.Round(Ζτ)+"m\n";}else if(Ќ==Ω.Βϋ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){φ();π(Ω.Main);}
Γ+=Š;Γ+="State: "+ΒΦ(Φ)+" \n";Γ+="Speed: "+Math.Round(Ț,1)+"m/s\n";Γ+="Target dist: "+ǫ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΠ+"\n";}else if(Ќ==Ω.Δυ){List<IMyTerminalBlock>Ě=Δξ();if(ā) Ȳ++;if(Ȳ>=Ě.Count) Ȳ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.Δφ);
Γ+=Š;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αδ,1)+"% "+Math.Round(Ł,1)+" / "+Math.Round(Ő,1)+"\n";Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+Γο)+"\n";Γ+="Battery: "+(ǥ==п.None?"":Ǻ+"% ")+Δλ(ǥ)+"\n";Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(ȓ,1)+"% ")+"\n";
Γ+="Damage: "+(Ě.Count==0?"None":""+(Ȳ+1)+"/"+Ě.Count+" "+Ě[Ȳ].CustomName)+"\n";}else if(Ќ==Ω.Δφ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=Š;Γ+="Next scan: "+Αψ+"s\n";Γ+="Drill area: "+Math.Round(ǯ,1)+"x"+Math.Round(Ǯ,1)+"m Ship: "+Math.Round(Αϊ,1)+"x"+Math.Round(Αω,1)+"m ("+Math.Round(θ,1)+"m)\n";
Γ+="Broadcast: "+(Ŀ?"Online - "+ΓЈ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŏ*100f,1)+"% \n";}else if(Ќ==Ω.Δτ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=Š;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+Αψ+"s\n";if(Αφ) Γ="Recompile script now";int ΘΪ=Γ.Split('\n').Length;for(int i=ΘΪ;i<=10;i++) Γ+="\n";Γ+=ΖΤ;Γ+="Last: "+Μ+"\n";
return Γ;}void Δί(bool ΘΥ,float ћ){φ();ŷ(true,false);if(ΘΥ) д(ref ĭ,ћ,σ.Βμ,false);else д(ref ĝ,ћ,σ.Βμ,false);Δή();Ē(true,true,0,0);}float Δγ(float ζ,float[]ΑЈ){float έ=ΑЈ[0];for(int i=ΑЈ.Length-1;i>=0;i--)
if(ζ<ΑЈ[i]) έ=ΑЈ[i];return έ;}String ǀ(ref int ȱ){String έ="";if(ȱ>=ΔΖ.Count) ȱ=-1;if(ȱ>=0) έ=ΔΖ[ȱ].CustomName;ȱ++;return έ;}void Ǌ(string υ){if(ή!=Λ.Є) return;if(υ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(υ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+υ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void İ(ref int value,int ž,int Ȝ,int ΓЋ){if(ž==0) return;if(value<Ȝ&&ΓЋ>0||value<=Ȝ&&ΓЋ<0){value+=ΓЋ;return;}int ΗϏ=Math.Abs(ž);int ΒЀ=0;int Γφ=1;while(true){ΒЀ+=Γφ*ΗϏ*10;if(ž<0&&value-Ȝ<=ΒЀ) break;if(ž>0&&value-Ȝ<ΒЀ) break;
Γφ++;}value+=Γφ*ž;}void ē(bool ΙІ){й=Math.Max(й,1);и=Math.Max(и,1);Х=Math.Max(Х,0);Ē(Ќ==Ω.Ű,false,0,0);}bool ď(ref float value,bool Θς,σ ς,String Ġ,String ΘΡ){if(Ġ=="") return false;float ŕ=-1;bool Εό=false;if(Ġ.ToUpper()==ΘΡ) Εό=true;else if(!float.TryParse(Ġ,out ŕ)) return false;
else ŕ=Math.Max(0,ŕ);if(Θς) ŕ=(float) Math.Round(ŕ);д(ref value,ŕ,ς,Εό,false);return true;}void д(ref float value,float ћ,σ ς,bool Ƣ){д(ref value,value+ћ,ς,Ƣ,true);}void д(ref float value,float ő,σ ς,bool Ƣ,bool ΙΘ){float ǚ=0,Ǚ=0;if(ς==σ.Ÿ){ǚ=0.5f;Ǚ=10f;}if(ς==σ.ƫ){ǚ=0.1f;Ǚ=1f;}
if(ς==σ.ū){ǚ=50f;Ǚ=100f;}if(ς==σ.Βα){ǚ=5f;Ǚ=30f;}if(ς==σ.ƽ){ǚ=1f;Ǚ=25f;}if(ς==σ.Ƽ){ǚ=10f;Ǚ=95f;}if(ς==σ.Ββ){ǚ=10f;Ǚ=90f;}if(ς==σ.Ηξ){ǚ=10f;Ǚ=1800;}if(ς==σ.Βμ){ǚ=0.0f;Ǚ=0.75f;}if(ő==-1&&Ƣ){value=-1;return;}if(value==-1) Ƣ=false;
bool ΖΙ=ő<ǚ||ő>Ǚ;if(ΖΙ&&ΙΘ){if(ő<value) value=Ǚ;else if(ő>value) value=ǚ;}else value=ő;if(ΖΙ&&Ƣ) value=-1;else value=Math.Max(ǚ,Math.Min(value,Ǚ));value=(float) Math.Round(value,2);}void ΑЊ(bool Θν){if(Θν) К=Math.Max(0,К-1);else К++;ΒΝ();ΒΜ();for(int k=0;k<Ȃ.Count&&k<Ч.Length;k++) Βτ(Ȃ[k],k<К);
Ѐ=-1;Βώ(true);}T Ȣ<T>(T ζ){int Β=Array.IndexOf(Enum.GetValues(ζ.GetType()),ζ);Β++;if(Β>=ΖЋ(ζ)) Β=0;return(T) Enum.GetValues(ζ.GetType()).GetValue(Β);}int ΖЋ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βϊ(){Ʃ.Clear();for(int i=0;i<Σ.Count;i++) Ʃ.Add(Σ[i]);Σ.Clear();
Αα=true;Γα=new Χ(Θ);Γδ=new Χ(Ε);Θ.γ=false;if(Η==Ζ.Ϊ) Ε.γ=false;for(int i=0;i<ќ.Count;i++)
if(!Ȇ.Contains(ќ.Keys.ElementAt(i)))
Ȇ.Add(ќ.Keys.ElementAt(i));Ȉ=false;š=false;Ţ=false;π(Ω.ΔЇ);}void ΔЏ(){if(Ȉ) Θ=Γα;if(š) Ε=Γδ;if(Ţ){Σ.Clear();for(int i=0;i<Ʃ.Count;i++) Σ.Add(Ʃ[i]);}Αα=false;φ();π(Ω.Main);}void Ηω(){if(!Αα) return;if(Φ!=Α.Я){ΔЏ();return;}if(!Γα.γ) Ȉ=false;if(!Γδ.γ) š=false;if(Ʃ.Count<=1) Ţ=false;
IMyShipConnector Й=З(MyShipConnectorStatus.Connectable);if(Й==null) Й=З(MyShipConnectorStatus.Connected);if(Й!=null){if(Math.Round(Ț,2)<=0.20) ΓΛ++;else ΓΛ=0;if(ΓΛ>=5){if(Η==Ζ.Ϊ&&(Θ.γ||Ȉ)&&Vector3.Distance(Θ.Δ,Й.GetPosition())>5){Ε.β=Υ.WorldMatrix.Forward;Ε.χ=Υ.WorldMatrix.Left;
Ε.α=Υ.WorldMatrix.Down;Ε.ν=Υ.GetNaturalGravity();Ε.Δ=Й.GetPosition();Ε.γ=true;Ε.Ц=Й.Position;}else{Θ.β=Υ.WorldMatrix.Forward;Θ.χ=Υ.WorldMatrix.Left;Θ.α=Υ.WorldMatrix.Down;Θ.ν=Υ.GetNaturalGravity();Θ.Δ=Й.GetPosition();Θ.γ=true;Θ.Ц=Й.Position;}}}double Γζ=-1;if(Σ.Count>0) Γζ=Vector3.Distance(ΰ,Σ.Last().Δ);
double Θό=Math.Max(1.5,Math.Pow(Ț/100.0,2));double ΖΒ=Math.Max(Ț*Θό,2);Ζτ=ΖΒ;if((Γζ==-1)||Γζ>=ΖΒ){Χ Ψ=new Χ(ΰ,ΑΪ,Ƈ,ƒ,Υ.GetNaturalGravity());Ψ.Ζψ(ȇ,Ȇ);Σ.Add(Ψ);}}Χ ж(){if(Η!=Ζ.Ϊ) return ј!=null?ј:Θ;return ΕΪ;}void ĵ(Χ ά,Λ У){ΕΪ=ά;if(ή==Λ.Є) ц=У;}bool ǵ(Χ ά,float ό,bool ΘБ,out Vector3 η)
{if(ΘБ){Vector3I ΓΨ=new Vector3I((int) ά.Ц.X,(int) ά.Ц.Y,(int) ά.Ц.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(ΓΨ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){η=new Vector3();return false;}Vector3 ΗЍ=а(Υ,Τ.FatBlock.GetPosition()-ΰ);Vector3 ΗЌ=а(Υ,Τ.FatBlock.WorldMatrix.Forward);η=ά.Δ-Βύ(ά.β,ά.α*-1,ΗЍ)-Βύ(ά.β,ά.α*-1,ΗЌ)*ό;
return true;}else{η=ά.Δ;return true;}}Vector3 Ȋ(int ΗЊ,int ř,bool Θπ){if(!Θπ&&Ετ) return ΒЏ;float ΗЋ=((Н-1f)/2f)-ΗЊ;float Θσ=((в-1f)/2f)-ř;ΒЏ=Ε.Δ+Ε.χ*ΗЋ*ǯ+ȗ*-1*Θσ*Ǯ;Ετ=true;return ΒЏ;}Vector3 ΒΥ(Vector3 ΘЀ,float ΓΘ){return ΘЀ+(Ǜ*ΓΘ);}ί ΒΘ(){float А=-1;ί έ=ί.ǲ;
if(Η!=Ζ.Ϊ){ΔϏ();if(ή!=Λ.з){Vector3 Ј=đ(Ǜ,ȗ*-1,ΰ-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(Н*ǯ)/2f&&Math.Abs(Ј.Y)<=(float)(в*Ǯ)/2f){if(Ј.Z<=-1&&Ј.Z>=-Ħ*2) return ί.Ƭ;if(Ј.Z>-1&&Ј.Z<θ*2) return ί.ǲ;}if(Ƞ(Ε.Δ,ref А)) έ=ί.ǲ;}if(Θ.γ){if(Ƞ(Θ.Δ,ref А)) έ=ί.Ǳ;for(int i=0;i<Σ.Count;i++)
if(Ƞ(Σ[i].Δ,ref А)) έ=ί.ȋ;if(Vector3.Distance(ΰ,Θ.Δ)<dockDist*θ) έ=ί.Ǳ;if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null)
έ=ί.Ǳ;}}else{Vector3 η=new Vector3();IMyShipConnector Ǿ=З(MyShipConnectorStatus.Connected);if(Θ.γ){if(Ƞ(Θ.Δ,ref А)) έ=ί.Ǳ;if(ǵ(Θ,dockDist,true,out η))
if(Ƞ(η,ref А)) έ=ί.Ǳ;if(Ǿ!=null&&Vector3.Distance(Ǿ.GetPosition(),Θ.Δ)<5)
return ί.ΑЎ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*θ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*θ)
if(Ƞ(Σ[i].Δ,ref А)) έ=ί.ȋ;if(Ε.γ){if(Ƞ(Ε.Δ,ref А)) έ=ί.ǲ;if(ǵ(Ε,dockDist,true,out η))
if(Ƞ(η,ref А)) έ=ί.ǲ;if(Ǿ!=null&&Vector3.Distance(Ǿ.GetPosition(),Ε.Δ)<5)
return ί.ΑЏ;}}return έ;}bool Ƞ(Vector3 η,ref float А){float ό=Vector3.Distance(η,ΰ);if(ό<А||А==-1){А=ό;return true;}return false;}void Βω(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϊ){ȡ();return;}Ε.Δ=ΰ;Ε.ν=Υ.GetNaturalGravity();Ε.β=ΑΪ;Ε.α=Ƈ;Ε.χ=ƒ;
Ǜ=љ.WorldMatrix.Forward;ȗ=Ε.α;if(Ǜ==Υ.WorldMatrix.Down) ȗ=Υ.WorldMatrix.Backward;ŷ(true,true);Ν(Α.Ȥ);Βψ();}void ŷ(bool ǖ,bool Θρ){if(ή==Λ.з&&!ǖ) return;bool Θγ=ǖ||ή==Λ.ΒΚ||Н!=й||в!=и||ġ!=ț;if(Θγ){if(ή!=Λ.з){ή=Λ.Ư;Ȋ(Ш,Щ,Θρ);Μ="Job changed, lost progress";}ġ=ț;
Н=й;в=и;Щ=0;Ш=0;Ч="";Ѐ=-1;Ĥ=0;ȕ=0;Р=0;К=0;Βώ(true);}}void Βπ(){П(ΰ,0);ѐ(ȇ,true);}void Βψ(){Ƿ(б.ǅ);Βπ();ŵ(ȃ,false);ǣ(ΓЍ);ή=Λ.Є;Δέ(true);ц=ή;π(Ω.ů);Ηέ();ΑΝ=true;Αή=0;for(int i=Ź.Count-1;i>=0;i--)
if(ŭ(Ź[i],false)) Αή++;if(Αή>0) Μ="Started with damage";}void φ(){if(ή==Λ.Є){ή=Λ.Ĵ;Μ="Job paused";}Ν(Α.Я);ц=ή;ǆ(false,0,0,0,0);Βό();ΔΪ(new Vector3(),false);ΔЎ();Βσ(ChargeMode.Auto);Βυ(false);ΔЊ(true);ȍ(Α.Я);Ē(false,false,0,0);ѐ(ο,false);ѐ(т,true);ǣ(true);Αχ=false;ư();Ɔ=false;
Βρ();ΑΝ=false;Ɯ=false;ƣ=false;if(Ќ!=Ω.Main&&Ќ!=Ω.ƭ&&Ќ!=Ω.ΑЋ&&Ќ!=Ω.Ǉ)
π(Ω.Main);}void ȡ(){ί Г=ΒΘ();if(Η==Ζ.Ϊ){if(!Ε.γ||!Θ.γ) return;Βψ();bool ΓΦ=Vector3.Distance(ΰ,Θ.Δ)<Vector3.Distance(ΰ,Ε.Δ);if(Ŝ==Λ.Į) ΓΦ=true;if(Ŝ==Λ.į) ΓΦ=false;if(ΓΦ){ĵ(Θ,Λ.Į);switch(Г){case ί.ΑЎ:Ν(Α.ȧ);break;case ί.ȋ:Ν(Α.Ю);break;case ί.Ǳ:Ν(Α.Ǡ);break;default:Ν(Α.ǐ);break;
}}else{ĵ(Ε,Λ.į);switch(Г){case ί.ΑЏ:Ν(Α.ȧ);break;case ί.ǲ:Ν(Α.Ǡ);break;case ί.ȋ:Ν(Α.Ю);break;default:Ν(Α.ǐ);break;}}}else{if(ή!=Λ.Ĵ&&ή!=Λ.Ư) return;bool ΙΒ=ή==Λ.Ư;Βψ();bool ΓΫ=Ů(false)&&Θ.γ;switch(Г){case ί.ǲ:Ν(ΓΫ?Α.ȣ:Α.Ȥ);break;case ί.ȋ:Ν(ΓΫ?Α.І:Α.Ю);break;case ί.Ǳ:Ν(ΓΫ?Α.Ǡ:Α.ǹ);break;
case ί.Ƭ:{if(ȕ!=К||ΙΒ) Ν(Α.Ȍ);else Ν(Α.Ų);}break;default:break;}}}void Ď(){if(ή==Λ.з&&!Θ.γ) return;if(Η==Ζ.Ϊ&&(!Ε.γ||!Θ.γ)) return;Μ="Move to job";ί Г=ΒΘ();if(Η==Ζ.Ϊ){ĵ(Ε,Λ.į);switch(Г){case ί.ǲ:Ν(Α.Ǡ);break;case ί.ȋ:Ν(Α.Ю);break;case ί.ΑЏ:return;default:Ν(Α.ǐ);break;
}ȍ(Α.ȧ);}else{switch(Г){case ί.ǲ:Ν(Α.Ю);break;case ί.ȋ:Ν(Α.Ю);break;case ί.Ǳ:Ν(Α.ǹ);break;case ί.Ƭ:Ν(Α.Ȍ);break;default:break;}if(ή==Λ.з) ȍ(Α.Ю);else ȍ(Α.ȟ);Αχ=true;}Βπ();π(Ω.Βϋ);ŵ(ȃ,false);ц=Λ.ƿ;}void Ƶ(){if(!Θ.γ) return;Μ="Move home";ί Г=ΒΘ();if(Η==Ζ.Ϊ){ĵ(Θ,Λ.Į);switch(Г)
{case ί.ȋ:Ν(Α.І);break;case ί.Ǳ:Ν(Α.Ǡ);break;case ί.ΑЎ:return;default:Ν(Α.ǐ);break;}ȍ(Α.ȧ);}else{if(З(MyShipConnectorStatus.Connected)!=null) return;if(З(MyShipConnectorStatus.Connectable)!=null){Ν(Α.č);ȍ(Α.ǹ);return;}switch(Г){case ί.ǲ:Ν(Α.ȣ);break;case ί.ȋ:Ν(Α.І);break;case ί.Ǳ:Ν(Α.І);break;
case ί.Ƭ:Ν(Α.Đ);break;default:break;}ȍ(Α.ǹ);}Βπ();π(Ω.Βϋ);ŵ(ȃ,false);ц=Λ.ƾ;}void Ν(Α Γχ){if(Γχ==Α.Я) ƞ=Α.Я;if(ƞ!=Α.Я&&Φ==ƞ&&Γχ!=ƞ){φ();return;}Ё=true;Φ=Γχ;}void ȍ(Α У){this.ƞ=У;}void Ηχ(){if(Φ==Α.Ȥ){if(Ё){Ђ=0;if(ȕ!=К) Ĥ=0;ȕ=К;}if(Ђ==0){int ΓΑ=ΖϏ(К);if(ΓΑ<0)
{if(!ΓΕ){ΓΕ=true;ΕΧ=ΰ;}П(ΕΧ,0);Μ="Waiting for the controller";return;}ΓΕ=false;if(ΓΑ!=К){К=ΓΑ;Ĥ=0;ȕ=К;}ķ έ=Ηδ(К);if(έ==ķ.ΔΩ){ή=Λ.ΒΚ;Μ="Job done";if(ć&&Θ.γ){Ν(Α.ȣ);ȍ(Α.ǹ);ц=Λ.ƾ;}else{Ν(Α.Ĳ);ȍ(Α.ȟ);ц=Λ.ƿ;}return;}if(έ==ķ.Δκ){Ђ=1;ѐ(ο,true);щ=Ȋ(Ш,Щ,true);
П(щ,10);Ф(Ε.α,Ε.β,Ε.χ,false);}}else{if(Ѐ>=0&&(Δό()?ǿ>=0&&ǿ!=Ѐ:Βά(Ѐ,true))){Ђ=0;return;}if(Л<wpReachedDist){Ν(Α.Ų);return;}}}if(Φ==Α.Ų){if(Ё){ѐ(ο,true);ǣ(false);щ=Ȋ(Ш,Щ,false);П(ΒΥ(щ,0),0);Ф(Ε.α,Ε.β,Ε.χ,false);Ђ=1;к=0;Э=0;ΑΗ=0;Γη=-1;Ħ=Х;Γβ=true;
}if(!ΒΓ()){Ν(Α.Đ);return;}if(Ů(true)){ΖΗ=ȉ("","ORE",И.Ċ);if((М==Ϗ.ΒΛ||М==Ϗ.Ʊ||
М==Ϗ.Βέ||М==Ϗ.Ƹ)&&Η!=Ζ.Ǎ)
Ν(Α.ũ);else if((М==Ϗ.Δβ||М==Ϗ.ΒΕ)&&Η!=Ζ.Ǎ)
Ν(Α.ΒϏ);else
Ν(Α.Đ);return;}Р=Vector3.Distance(ΰ,щ);if(Р>Ĥ){Ĥ=Р;Γβ=false;}if(Η==Ζ.Ǎ&&Δο()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ђ>0&&Ђ<Э){if(Р>к){if(Э>150) к=Р;else к=(float) Math.Ceiling(Р);Ђ=1;}else Ђ++;}else{if(Ђ>0){Μ="Ship stuck! Retrying";к=Р;Ђ=0;Ē(false,true,0,θ*sensorRange);}П(ΒΥ(щ,Math.Max(0,к-θ)),ΒΨ(false));if(Л<=wpReachedDist/2){Ђ=1;к=0;}
return;}Ē(false,true,θ*sensorRange,0);Vector3 ΗЁ=щ+Ǜ*Р;bool ΕЀ=false;if(Vector3.Distance(ΗЁ,ΰ)>0.3f){Vector3 ΗЎ=щ+Ǜ*(Р+0.1f);П(ΗЎ,4);ΕЀ=true;}else{float ǜ=ΒΨ(true);Vector3 Εή=ΒΥ(щ,Math.Max(Х+1,Р+1));Ǣ(true,false,false,Εή,Εή-щ,ǜ,ǜ);}bool ΓΙ=false;if(у==э.ΑБ||у==э.ΑА)
{if(!ΕЀ){float Γω=0;foreach(IMyTerminalBlock ǭ in ο)
Γω+=ΖІ(ǭ,"","",у==э.ΑА?new string[]{"STONE"}:null);if(Γω>Γη||Р<Х||Γβ){ΒІ=0;ΑΗ=Р;Ħ=(int)(Math.Max(Ħ,ΑΗ)+θ/2);}else{ΓΙ=Р-ΑΗ>2&&ΒІ>=20;ΒІ++;}Γη=Γω;}}else ΓΙ=Р>=Ħ;if(ȕ!=К){Ν(Α.Ȍ);Р=0;return;}if(ΓΙ){К++;Ν(Α.Ȍ);Р=0;return;}}if(Φ==Α.Ȍ||Φ==Α.Đ||Φ==Α.ũ)
{if(Ё){щ=Ȋ(Ш,Щ,false);Ф(Ε.α,Ε.β,Ε.χ,false);ѐ(ο,Ń);ǣ(false);к=Vector3.Distance(ΰ,щ);Ē(false,true,0,θ*sensorRange);}П(щ,ΒΨ(false));if(Vector3.Distance(ΰ,щ)>=к+5){ѐ(ο,false);ǣ(true);Μ="Can´t return!";}if(Л<wpReachedDist){if(Φ==Α.Ȍ&&Αχ) Ν(Α.Ĳ);if(Φ==Α.Ȍ) Ν(Α.Ȥ);
if(Φ==Α.ũ) Ν(Α.ƴ);if(Φ==Α.Đ){if(Θ.γ) Ν(Α.ȣ);else{φ();Ď();Μ="Can´t return, no dock found";}}return;}}if(Φ==Α.І||Φ==Α.Ю){if(Φ==Α.Ю&&ή==Λ.Є&&Η!=Ζ.Ϊ){if(!ΒΓ()||Ů(true)){Ν(Α.І);return;}}bool ΓЃ=false;bool ΑІ=false;bool ΕΚ=false;float ļ=0;bool ΖΖ=false;Χ Ψ=null;
if(Ё){if(Φ==Α.І||Η==Ζ.Ϊ){if(Η!=Ζ.Ϊ){if(ȥ(1)) ΑЍ();else{ј=null;Ȅ=0;Ɔ=!ΒΔ(1);}}Χ ά=ж();ε=new Βξ();ε.Љ=ά;ε.Ƅ=followPathDock*θ;ε.ΔΜ=useDockDirectionDist*θ;ε.ļ=10;ε.ş.Add(ά.Δ);Vector3 Ļ=new Vector3();if(ǵ(ά,dockDist*θ,true,out Ļ))
ε.ş.Add(Ļ);else ε.Ƅ*=1.5f;if(Η==Ζ.Ϊ){if(ά==Θ) ε.ΑΧ=Ε.Δ;if(ά==Ε) ε.ΑΧ=Θ.Δ;ε.ΓΣ=dockDist*θ*1.1f;}}else if(Φ==Α.Ю){ε=new Βξ();ε.Љ=Ε;ε.Ƅ=followPathJob*θ;ε.ΔΜ=useJobDirectionDist*θ;ε.ļ=10;ε.ΑΧ=Θ.Δ;ε.ΓΣ=dockDist*θ*1.1f;ε.ş.Add(Ε.Δ);if(ή==Λ.з){if(!Θ.γ||Σ.Count==0){φ();return;}
float ΘΓ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΔ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΓ<ΘΔ) ε.Љ=Σ.Last();else ε.Љ=Σ.First();}}Ɨ=new Vector3();ΖΖ=!ΗΜ(ΰ);ѐ(ο,false);ǣ(true);Т=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ε.ΑΧ)<=ε.ΓΣ) continue;
double ό=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǧ==-1||ό<Ǧ){Т=i;Ǧ=ό;}}Ǫ=ΗΑ(ε.Љ.Δ,Т);ę=null;}if(Φ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&!Ŷ()){Χ ΒЊ=ж();Ɔ=!ΒΔ(1);if(ж()!=ΒЊ){Ν(Α.І);return;}}if(Φ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&ȥ(1)&&ΑЍ()){Ν(Α.І);return;}bool Εώ=false;if(Φ==Α.І&&!Ё&&Т>=0&&Т<Σ.Count)
{Vector3 Ƃ=Σ[Т].Δ+Δπ(Т,Ǫ);int Γψ=Т+Ǫ;Vector3 Œ=Ǫ!=0&&Γψ>=0&&Γψ<Σ.Count?Σ[Γψ].Δ:ε.Љ.Δ;if(ΗΔ(Ƃ,Œ)){if(!ΑΠ){ΑΠ=true;Εγ=ĳ(Ƃ)?ΰ:Ƃ;}П(Εγ,0);Ф(ε.Љ.α,ε.Љ.β,ε.Љ.χ,false);Μ="Waiting for dock clearance";return;}ΑΠ=false;Εώ=ȥ(1)&&(ĳ(ΰ)||ĳ(Œ));}Ζω(Σ,Ǫ,ε.ş,ε.Ƅ,Ё,ref Ђ);
for(int i=0;i<ε.ş.Count;i++){float ό=Vector3.Distance(ΰ,ε.ş[i]);if(ό<=ε.Ƅ) ΓЃ=true;if(ό<=ε.ΔΜ) ΑІ=true;}if(Εώ) ΑІ=true;if(ΑІ) ļ=ε.ļ;float ΙЈ=ę!=null?ę.Ў:Ț;float ΙΗ=(float) Math.Max(Ț*0.1f*θ,wpReachedDist);if((Л<ΙΗ)||Ё){if(!Ё) Т+=Ǫ;if(Ǫ==0||Т>Σ.Count-1||Т<0)
ΓЃ=true;else{ΔΠ=Ǫ>0?Σ.Count-1-Т:Т;Ψ=Σ[Т];ę=Ψ;if(Т>=1&&Т<Σ.Count-1)
Ɨ=Ψ.Δ-Σ[Т-Ǫ].Δ;else ę=null;ψ=Ψ.Δ+Δπ(Т,Ǫ);ΕΚ=true;}}if(ΑІ)
Ф(ε.Љ.α,ε.Љ.β,ε.Љ.χ,false);else if(ΖΖ)
ΑЌ(ε.Љ.α,10,true);else if(ΕΚ&&Ψ!=null){if(Ǫ>0) Ф(Ψ.α,Ψ.β,Ψ.χ,90,false);else Ф(Ψ.α,-Ψ.β,-Ψ.χ,90,false);}Ǣ(true,false,true,ψ,Ɨ,ę==null?0:ę.Ў,ļ);if(ΓЃ){ΔΠ=0;if(Φ==Α.І&&!Ζύ()){П(ψ,0);return;}if(Φ==Α.І&&Ɔ){П(ψ,0);Μ="Waiting for a free dock";return;}if(Φ==Α.І||Η==Ζ.Ϊ){Ν(Α.Ǡ);return;}
if(Φ==Α.Ю&&Αχ){Ν(Α.Ĳ);return;}if(Φ==Α.Ю){Ν(Α.Ȥ);return;}}}if(Φ==Α.Ǡ||Φ==Α.ų){if(Η!=Ζ.Ϊ&&Ŷ()){if(!ȥ(1)){Ν(Α.І);return;}}else if(Η!=Ζ.Ϊ){Χ ΒЊ=ж();if(!ΒΔ(2)||ж()!=ΒЊ){Ν(Α.І);return;}}Χ ά=ж();if(Ё){if(!ǵ(ά,dockDist*θ,true,out ψ)){Ƿ(б.ŧ);φ();return;}
П(ψ,0);ΑЌ(ά.α,90,true);}if(Л<followPathDock*θ&&Л!=-1){П(ψ,10);Ф(ά.α,ά.β,ά.χ,false);}if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null){Ν(Α.č);return;}if(Л<wpReachedDist/2&&Л!=-1){Ν(Α.ĉ);return;}}if(Φ==Α.ĉ||Φ==Α.ȟ){if(Ё)
{if(Φ==Α.ĉ){Χ ά=ж();if(!ǵ(ά,dockDist*θ,true,out ψ)){Ƿ(б.ŧ);φ();return;}Ǣ(true,true,false,ψ,0);Ф(ά.α,ά.β,ά.χ,10,false);}if(Φ==Α.ȟ){Ф(Ε.α,Ε.β,Ε.χ,0.5f,false);ψ=Ε.Δ;Ǣ(true,true,false,ψ,0);}}if(Αΰ){ǆ(false,0,0,0,0);if(Φ==Α.ĉ) Ν(Α.č);if(Φ==Α.ȟ) φ();return;}}if(Φ==Α.č)
{if(З(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.ǹ);return;}Χ ά=ж();if(Ё){Э=0;ΘΕ=DateTime.Now;Ђ=0;Ф(ά.α,ά.β,ά.χ,false);}Vector3I ΓΨ=new Vector3I((int) ά.Ц.X,(int) ά.Ц.Y,(int) ά.Ц.Z);IMySlimBlock ΕΦ=Me.CubeGrid.GetCubeBlock(ΓΨ);float Εε=dockingSpeed;float Θέ=dockingSpeed*5;
float ΕΗ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!ǵ(ά,0,true,out ψ)||!ǵ(ά,ΕΗ,true,out Ɨ)||
ΕΦ==null||!ΕΦ.FatBlock.IsFunctional){Ƿ(б.ŧ);φ();return;}if(Э==1||(Vector3.Distance(ΰ,ψ)<=ΕΗ*1.1f&&!Ё))
Э=1;else{Vector3 ΘЉ=а(Υ,Ɨ-ΰ);Vector3 ΓΧ=а(Υ,Υ.GetNaturalGravity());float Θτ=Ʈ(ΘЉ,ΓΧ,null);Εε=Math.Min(Θέ,Θτ);}Ǣ(true,false,false,ψ,ψ-ΰ,dockingSpeed,Εε);if(Ё) к=(float) Л;IMyShipConnector ΓΔ=З(MyShipConnectorStatus.Connectable);if(ΓΔ!=null){Ǣ(false,false,false,ψ,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΓΔ.Connect();if(ΓΔ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.ǹ);Βό();ŵ(ȃ,true);return;}}}else{float Εβ=(float) Math.Round(Л,1);if(Εβ<к){Ђ=-1;к=Εβ;}else Ђ++;if(Ђ>20){Ν(Α.ų);return;}}}if(Φ==Α.ǹ||Φ==Α.ȧ||
Φ==Α.ΑΒ||Φ==Α.Ƴ||Φ==Α.Č){bool ΒЄ=false,ΒЅ=false;if(Η==Ζ.Ϊ){if(ж()==Θ) ΒЄ=true;else if(ж()==Ε) ΒЅ=true;}if(Ё){Ɯ=false;if(З(MyShipConnectorStatus.Connected)==null){Ν(Α.ǐ);return;}Βό();if(ΒЄ) Ǌ(ϊ.Ǖ);if(ΒЅ) Ǌ(ϋ.Ǖ);Ī=false;Ȏ=false;Ȓ=false;ƥ=false;}if(З(MyShipConnectorStatus.Connected)==null){φ();Ƿ(б.Δψ);return;}
if(ή!=Λ.Є||ч==-1||ǥ==п.None) Ȏ=true;else if(Ǻ>=100f) Ȏ=true;else if(Ǻ<=99f) Ȏ=false;if(ή!=Λ.Є||ш==-1||ѓ.Count==0) Ȓ=true;else if(ȓ>=100f) Ȓ=true;else if(ȓ<=99) Ȓ=false;if(ή!=Λ.Є||Ы==-1||ă.Count==0) ƥ=true;else ƥ=ī>=Ы;ĸ о=null;if(ΒЄ) о=ϊ;
if(ΒЅ) о=ϋ;if(о!=null&&(о.Ћ==ι.ǂ||о.Ћ==ι.ǁ)) Ȏ=true;if(о!=null&&(о.Ћ==ι.Βε)) if(!Ī) Ȏ=false;if(о!=null&&(о.Ћ==ι.Βι||о.Ћ==ι.Βη)) Ȓ=true;if(о!=null&&(о.Ћ==ι.Βθ)) if(!Ī) Ȓ=false;if(ā){ChargeMode ΕΤ=Ȏ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ћ==ι.ǁ||о.Ћ==ι.ǂ))
ΕΤ=ChargeMode.Discharge;Βσ(ΕΤ);Βυ(!Ȓ);}if(!Ī){if(Η==Ζ.Ϊ)
Ī=ή!=Λ.Є||Ζώ(Ё,true)||ƣ;else
Ī=ή!=Λ.Є||Δω();}else{if(!Ȏ) Ν(Α.Č);if(!Ȓ) Ν(Α.Ƴ);if(!ƥ) Ν(Α.ΑΒ);Ё=false;}if(Ī&&Ȏ&&Ȓ&&ƥ){Βσ(ChargeMode.Auto);Βυ(false);if(ή==Λ.Є){if(Η==Ζ.Ϊ){if(ж()==Θ) Ǌ(ϊ.ǘ);else if(ж()==Ε) Ǌ(ϋ.ǘ);if(ж()==Θ) ĵ(Ε,Λ.į);else ĵ(Θ,Λ.Į);}}Ν(Α.ǐ);return;}}if(Φ==Α.ǐ){if(ƨ){if(!Δΰ()) return;
ƨ=false;Ν(Α.ǐ);return;}if(Ё){if(!Δΰ()){ƨ=true;return;}IMyShipConnector Ǿ=З(MyShipConnectorStatus.Connected);if(Ǿ==null){Ν(Α.Ю);return;}IMyShipConnector δ=Ǿ.OtherConnector;ΔЉ(Ǿ,false);ŵ(ȃ,false);Χ ά=null;Χ Γΰ=Η==Ζ.Ϊ?Θ:ж();if(Vector3.Distance(Ǿ.GetPosition(),Γΰ.Δ)<5f&&Θ.γ) ά=Γΰ;
if(Vector3.Distance(Ǿ.GetPosition(),Ε.Δ)<5f&&Ε.γ) ά=Ε;if(ά!=null){if(!ǵ(ά,dockDist*θ,true,out ψ)){Ƿ(б.ŧ);φ();return;}П(ψ,5);Ф(ά.α,ά.β,ά.χ,false);}else П(ΰ+δ.WorldMatrix.Forward*dockDist*θ,5);if(ή==Λ.Є) Ƿ(б.ǅ);}if(Л<wpReachedDist){ѐ(т,true);Ν(Α.Ю);return;}}if(Φ==Α.Ĳ)
{if(Ё){ǣ(true);ѐ(ο,false);ψ=Ε.Δ;П(ψ,20);Ф(Ε.α,Ε.β,Ε.χ,false);}if(Л<wpReachedDist/2){Ν(Α.ȟ);return;}}if(Φ==Α.ȣ){if(Ё){ǣ(true);ѐ(ο,false);int А=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǧ==-1||ό<Ǧ){А=i;Ǧ=ό;}}
if(А==-1){Ν(Α.І);return;}ψ=Σ[А].Δ;П(ψ,10);Ф(Ε.α,Ε.β,Ε.χ,false);}if(Л<wpReachedDist){Ν(Α.І);return;}}if(Φ==Α.ƴ){bool ф=false;if(Ё){ǣ(true);if((М==Ϗ.ΒΛ||М==Ϗ.Ʊ)&&ƺ()&&
ı(Ǜ,Υ.GetNaturalGravity())<25&&Н>=2&&в>=2){Vector3 ƈ=ΰ;if(Ш>0&&Щ<в-1) ƈ=Ȋ(Ш-1,Щ+1,true);else if(Ш<Н-1&&Щ<в-1) ƈ=Ȋ(Ш+1,Щ+1,true);else if(Ш<Н-1&&Щ>0) ƈ=Ȋ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) ƈ=Ȋ(Ш-1,Щ-1,true);else ф=true;if(!ф) П(ƈ,10);
}else ф=true;}if(Л<wpReachedDist/2) ф=true;if(ф){Ν(Α.ΑΑ);return;}}if(Φ==Α.ΒϏ||Φ==Α.ΑΑ){if(Ё){Ǣ(true,true,false,ΰ,0);ѐ(ο,false);ǣ(true);Ђ=-1;Э=М==Ϗ.Βέ||М==Ϗ.Ƹ?0:-1;}bool ě=!ΒΓ();int ΑЀ=ȉ("STONE","ORE",И.Ċ);if(М==Ϗ.ΒΕ||М==Ϗ.Ƹ||М==Ϗ.Ʊ)
ΑЀ+=ȉ("ICE","ORE",И.Ċ);bool ΘΜ=ΑЀ>0;bool ΓΡ=false;if(Э>=0){float Ĺ=(float) Math.Sin(Δδ(Э))*ǯ/3f;float ΗЃ=(float) Math.Cos(Δδ(Э))*Ǯ/3f;Vector3 ΗЉ=Ȋ(Ш,Щ,true)+Βύ(Ǜ,ȗ*-1,new Vector3(Ĺ,ΗЃ,0));П(ΗЉ,0.3f);if(Л<Math.Min(ǯ,Ǯ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ђ==-1||ΑЀ<Ђ){Ђ=ΑЀ;к=0;}
else{к++;if(к>50) ΓΡ=true;}if(!ΘΜ||ě||ΓΡ){if(!ě){int ΘЎ=ȉ("","ORE",И.Ċ);if(Ů(true)) ě=true;else if(100-(Џ(ΘЎ,ΖΗ)*100)<minEjection) ě=true;else Ƿ(б.ǅ);}if(ΓΡ&&ě) Μ="Ejection failed";if(Φ==Α.ΑΑ){if(ě){if(Θ.γ) Ν(Α.ȣ);else{φ();Ď();Μ="Can´t return, no dock found";}}
else Ν(Α.Ȥ);}else if(ě) Ν(Α.Đ);else Ν(Α.Ų);return;}}Ё=false;}void Ζω(List<Χ>Ĉ,int κ,List<Vector3>Ơ,float ǫ,bool ȅ,ref int ǒ){if(ȅ){for(int i=0;i<Ĉ.Count;i++) Ĉ[i].Ў=0;ǒ=-1;return;}if(κ==0) return;int ћ=κ*-1;if(ǒ==-1) ǒ=ћ>0?1:Ĉ.Count-2;int ΕЂ=0;while(ǒ>=1&&ǒ<Ĉ.Count-1)
{if(ΕЂ>50) return;ΕЂ++;try{if((ћ<0&&ǒ>=1)||(ћ>0&&ǒ<=Ĉ.Count-2)){Χ ζ=Ĉ[ǒ];bool ΖΔ=false;for(int j=0;j<Ơ.Count;j++)
if(Vector3.Distance(ζ.Δ,Ơ[j])<=ǫ){ΖΔ=true;break;}if(!ΖΔ){Χ Ασ=Ĉ[ǒ-ћ];Χ Œ=Ĉ[ǒ+ћ];Vector3 ΘЊ=ζ.Δ-Œ.Δ;Vector3 ΔΗ=Ασ.Δ-ζ.Δ;Vector3 Θξ=ζ.Δ+Vector3.Normalize(ΘЊ)*ΔΗ.Length();Vector3 ΘΒ=Ασ.Δ-Θξ;Vector3 Αε=đ(κ>0?ζ.β:ζ.β*-1,ζ.α*-1,ΘΒ);Vector3 Θά=đ(κ>0?ζ.β:ζ.β*-1,ζ.α*-1,ΔΗ);
Vector3 Αη=đ(κ>0?ζ.β:ζ.β*-1,ζ.α*-1,ζ.ν);ζ.Ў=(float) Math.Sqrt(Math.Pow(Ασ.Ў,2)+Math.Pow(Ʈ(-Θά,Αη,ζ),2));for(int j=0;j<Ơ.Count;j++)
if(Vector3.Distance(Ασ.Δ,Ơ[j])<=ǫ){Vector3 Αθ=đ(κ>0?ζ.β:ζ.β*-1,ζ.α*-1,Ơ[j]-ζ.Δ);float ΘЂ=Ʈ(-Αθ,Αη,ζ);ζ.Ў=Math.Min(ζ.Ў,ΘЂ)/2f;}if(Αε.Length()==0) Αε=new Vector3(0,0,1);Vector3 ΘΫ=đ(ζ.β,ζ.α*-1,ζ.ν);float ΘЄ=Ū(-Αε,ΘΫ,ζ);float ΑΓ=Џ(ΘЄ,Ĩ);float ΔΕ=(float) Math.Sqrt(Џ(Αε.Length()*1.0f,0.5f*ΑΓ));
ζ.Ў=Math.Min(ζ.Ў,(ΔΗ.Length()/ΔΕ)*Ǥ);}}}catch{return;}ǒ+=ћ;}ǒ=-1;}void Βώ(bool ȅ){if(ȅ){Ə=0;return;}if(Η==Ζ.Ϊ) return;float ф=К*Math.Max(1,Х);if(ȕ==К) ф+=Math.Min(Х,Р);float ΘЍ=Н*в*Math.Max(1,Х);Ə=Math.Max(Ə,(float) Math.Min(ф/ΘЍ*100.0,100));}MyDetectedEntityType Δο()
{try{if(Ƕ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΨ(bool Ǩ){if(Η==Ζ.Ǎ&&Δο()==MyDetectedEntityType.None&&!ŭ(ξ,true))
return fastSpeed;else return Ǩ?ť:Ť;}void Ηρ(){float ΕЏ=0,Εά=0,Ġ=0,Γϊ=0;for(int i=0;i<Ȩ.Count;i++){IMyBatteryBlock Ž=Ȩ[i];if(!Ƕ(Ž,true)) continue;ΕЏ+=Ž.MaxStoredPower;Εά+=Ž.CurrentStoredPower;Ġ+=Ž.CurrentInput;Γϊ+=Ž.CurrentOutput;}Ǻ=(float) Math.Round(Џ(Εά,ΕЏ)*100,1);
if(Ġ>=Γϊ) ǥ=п.Č;else ǥ=п.Δε;if(Ġ==0&&Γϊ==0||Ǻ==100.0) ǥ=п.Я;if(Ȩ.Count==0) ǥ=п.None;}void Ηυ(){float Εμ=0;for(int i=0;i<ѓ.Count;i++){IMyGasTank Ζδ=ѓ[i];if(!Ƕ(Ζδ,true)) continue;Εμ+=(float) Ζδ.FilledRatio;}ȓ=Џ(Εμ,ѓ.Count)*100f;}void Ηύ(){ī=0;try{for(int i=0;i<ă.Count;i++)
{IMyReactor ΓЄ=ă[i];if(!Ƕ(ΓЄ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЄ.GetInventory(0).GetItems(Ъ);float Ǒ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem В=Ъ[j];if(В.Type.SubtypeId.ToUpper()=="URANIUM"&&В.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǒ+=(float) В.Amount;}if(Ǒ<ī||i==0){ī=Ǒ;Γο=ΓЄ.CustomName;}}}catch(Exception e){Ȕ=e;}}void Ης(){Ő=0;Ł=0;try{for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Τ=ŀ[i];if(!Ƕ(Τ,true)) continue;Ł+=(float) Τ.GetInventory(0).CurrentVolume;Ő+=(float) Τ.GetInventory(0).MaxVolume;
}Αδ=(float) Math.Min(Math.Round(Џ(Ł,Ő)*100,1),100.0);}catch(Exception e){Ȕ=e;}}void Ηφ(){try{Ǘ.Clear();for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Τ=ŀ[i];if(!Ƕ(Τ,true)) continue;ΔΨ(Τ,И.Ŭ);}if(М!=Ϗ.Βδ){for(int i=0;i<ĕ.Count;i++){IMyTerminalBlock Τ=ĕ[i];if(!Ƕ(Τ,true)) continue;
ΔΨ(Τ,И.ΗΘ);}}Ηκ(Ǘ);}catch(Exception e){Ȕ=e;}}void Ζό(){if(ΑΝ){if(Δξ().Count>Αή){ΑΝ=false;if(л!=е.ū){φ();if(л==е.ΒΠ) Ď();if(л==е.Ǆ)
if(Θ.γ) Ƶ();else Ď();}Μ="Damage detected";}}}bool ΒΓ(){if(!Αί) return true;if(ή==Λ.Є){if(ч>0&&ǥ!=п.None)
if(Ǻ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&ă.Count>0)
if(ī<=Ы){Μ="Low fuel: "+Γο;return false;}if(ш>0&&ѓ.Count>0)
if(ȓ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δξ(){List<IMyTerminalBlock>Ě=new List<IMyTerminalBlock>();for(int i=0;i<Ź.Count;i++){IMyTerminalBlock Τ=Ź[i];if(ŭ(Τ,false)) Ě.Add(Τ);}return Ě;}bool ŭ(IMyTerminalBlock Τ,bool ΓЏ){return(!Ƕ(Τ,ΓЏ)||!Τ.IsFunctional);
}bool Ƕ(IMyTerminalBlock Τ,bool ΓЏ){if(Τ==null) return false;try{IMyCubeBlock Εσ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΓЏ) return Εσ==Τ;else return Εσ.GetType()==Τ.GetType();}catch{return false;}}float ΖІ(IMyTerminalBlock Τ,String ь,String ΖΫ,String[]ǧ){float Ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory ň=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ň.GetItems(Ъ);foreach(MyInventoryItem В in Ъ){if(ǧ!=null&&(ǧ.Contains(В.Type.TypeId.ToUpper())||ǧ.Contains(В.Type.SubtypeId.ToUpper())))
continue;if((ь==""||В.Type.TypeId.ToUpper()==ь)&&(ΖΫ==""||В.Type.SubtypeId.ToUpper()==ΖΫ))
Ȁ+=(float) В.Amount;}}return Ȁ;}ю ΖЍ(String υ,String ς,И Г,bool ΗЏ){υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){ю В=Ǘ[i];if(В.υ.ToUpper()==υ&&В.ς.ToUpper()==ς&&(В.ō==Г||Г==И.Ċ))
return В;}ю έ=null;if(ΗЏ){έ=new ю(υ,ς,0,Г);Ǘ.Add(έ);}return έ;}int ȉ(String υ,String ς,И Г){return ȉ(υ,ς,Г,null);}int ȉ(String υ,String ς,И Г,String[]ǧ){int Ȁ=0;υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){ю В=Ǘ[i];if(ǧ!=null&&ǧ.Contains(В.υ.ToUpper())) continue;
if((υ==""||В.υ.ToUpper()==υ)&&(ς==""||В.ς.ToUpper()==ς)&&(В.ō==Г||Г==И.Ċ))
Ȁ+=В.Ǒ;}return Ȁ;}void ΔΨ(IMyTerminalBlock Τ,И Г){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΖЍ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Г,true).Ǒ+=(int) Ъ[j].Amount;}}void Ηκ(List<ю>С){for(int i=С.Count-1;i>0;i--)
for(int j=0;j<i;j++){ю a=С[j];ю b=С[j+1];if(a.Ǒ<b.Ǒ) С.Move(j,j+1);}}bool Ů(bool ΙϏ){if(Ȟ&&Η!=Ζ.Ϊ)
if(О!=-1&&Ĩ>=О){Μ="Ship too heavy";return true;}if(Αδ>=ǩ||Ɯ){Ɯ=false;Μ="Ship is full";return true;}return false;}bool Δω(){String[]ǧ=null;if(!ȝ) ǧ=new string[]{"ICE"};if(Η==Ζ.ű) return ȉ("","ORE",И.Ŭ,ǧ)==0;if(Η==Ζ.Ǎ) return ȉ("","COMPONENT",И.Ŭ,ǧ)==0;else return ȉ("","",И.Ŭ,ǧ)==0;
}bool ΗΝ(){List<IMyCockpit>ΓΓ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΓ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΓ.Count;i++)
if(ΓΓ[i].IsUnderControl) return true;return false;}bool Ζώ(bool ȅ,bool Αό){IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null) return false;if(Vector3.Distance(Θ.Δ,Й.GetPosition())<5)
return Δα(ϊ,ȅ,Αό);if(Vector3.Distance(Ε.Δ,Й.GetPosition())<5)
return Δα(ϋ,ȅ,Αό);return false;}bool Δα(ĸ ǽ,bool ȅ,bool Αό){if(ȅ) ǽ.ΗΪ();ǽ.Ηϊ();bool н=false;String ы="";switch(ǽ.Ћ){case ι.Βζ:{ы="Waiting for command";н=false;break;}case ι.ΔЄ:{ы="Waiting for cargo";н=Ů(true);break;}case ι.ΔЃ:{ы="Unloading";н=Δω();break;}case ι.Βλ:{н=true;break;}
case ι.Βε:{ы="Charging batteries";н=Ǻ>=100f;break;}case ι.ǂ:{ы="Discharging batteries";н=Ǻ<=25f;break;}case ι.ǁ:{ы="Discharging batteries";н=Ǻ<=0f;break;}case ι.Βθ:{ы="Filling up hydrogen";н=ȓ>=100f;break;}case ι.Βι:{ы="Unloading hydrogen";н=ȓ<=25f;break;}
case ι.Βη:{ы="Unloading hydrogen";н=ȓ<=0f;break;}case ι.Βκ:{bool Εύ=ΗΝ();if(!Εύ) ǽ.ΓЀ=true;н=ǽ.ΓЀ&&Εύ;ы="Waiting for passengers";break;}}if(!н) ǽ.ΒΒ(true);if(н&&ǽ.ΒΪ()){н=ǽ.ΒΒ(false);ы="Undocking in: "+ΒΣ((int) Math.Max(0,ǽ.ǔ-ǽ.Ȯ));}if(Αό) Ζς=ы;return н;}void Ζυ()
{if(!ŉ) return;if(ο.Count<=1) return;float Ζκ=0,Ζι=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǭ=ο[i];if(ŭ(ǭ,true)) continue;Ζκ+=(float) ǭ.GetInventory(0).MaxVolume;Ζι+=(float) ǭ.GetInventory(0).CurrentVolume;}float ΕΛ=(float) Math.Round(Џ(Ζι,Ζκ),5);for(int ΖΚ=0;ΖΚ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΚ++)
{float Γή=0,Γξ=0;float Εϊ=0,ΕЍ=0;IMyTerminalBlock ȑ=null,ȯ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǭ=ο[i];if(ŭ(ǭ,true)) continue;float Γυ=(float) ǭ.GetInventory(0).MaxVolume;float ΑΨ=Џ((float) ǭ.GetInventory(0).CurrentVolume,Γυ);if(ȑ==null||ΑΨ>Γή){ȑ=ǭ;Γή=ΑΨ;Εϊ=Γυ;}
if(ȯ==null||ΑΨ<Γξ){ȯ=ǭ;Γξ=ΑΨ;ΕЍ=Γυ;}}if(ȑ==null||ȯ==null||ȑ==ȯ) return;if(checkConveyorSystem&&!ȑ.GetInventory(0).IsConnectedTo(ȯ.GetInventory(0))){if(ΒЉ>20) Μ="Inventory balancing failed";else ΒЉ++;return;}ΒЉ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȑ.GetInventory(0).GetItems(Ъ);float Ƨ=0;if(Ъ.Count==0) continue;MyInventoryItem ΑΩ=Ъ[0];String ΕЁ=ΑΩ.Type.TypeId+ΑΩ.Type.SubtypeId;if(!Ζρ.TryGetValue(ΕЁ,out Ƨ)){if(ΗΡ(ȑ.GetInventory(0),0,ȯ.GetInventory(0),out Ƨ))
Ζρ.Add(ΕЁ,Ƨ);else return;}float ΘЋ=((Γή-ΕΛ)*Εϊ/Ƨ);float ΘЌ=((ΕΛ-Γξ)*ΕЍ/Ƨ);int ΔΚ=(int) Math.Min(ΘЌ,ΘЋ);if(ΔΚ<=0) return;if((float) ΑΩ.Amount<ΔΚ)
ȑ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΑΩ.Amount);else
ȑ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΔΚ);}}bool ΗΡ(IMyInventory Ɲ,int Β,IMyInventory ΘΑ,out float Ζπ){Ζπ=0;float ΗІ=(float) Ɲ.CurrentVolume;List<MyInventoryItem>ΒЋ=new List<MyInventoryItem>();Ɲ.GetItems(ΒЋ);float ΕΞ=0;for(int i=0;i<ΒЋ.Count;i++) ΕΞ+=(float) ΒЋ[i].Amount;
Ɲ.TransferItemTo(ΘΑ,Β,null,null,1);float Ζο=ΗІ-(float) Ɲ.CurrentVolume;List<MyInventoryItem>ΒЁ=new List<MyInventoryItem>();Ɲ.GetItems(ΒЁ);float ΕΓ=0;for(int i=0;i<ΒЁ.Count;i++) ΕΓ+=(float) ΒЁ[i].Amount;if(Ζο==0f||!Βή(0.9999,ΕΞ-ΕΓ,1.0001))
return false;Ζπ=Ζο;return true;}void Ητ(){Vector3 Ƌ=ψ+Ć;Vector3 Ǭ=Ƌ-ΰ;if(Ǭ.Length()==0) Ǭ=new Vector3(0,0,-1);Vector3 Αθ=а(Υ,Ǭ);Vector3 Ζε=Vector3.Normalize(Αθ);Vector3 Ō=а(Υ,Υ.GetNaturalGravity());float Θύ=Γύ>0?Math.Max(0,1-ı(Ǭ,Εξ)/5):0;float Ў=(float) Math.Min((Γς>0?Γς:1000f),Math.Max(Ʈ(-Αθ,Ō,null),Γύ*Θύ));
if(!Ζθ) Ў=0;if(ΖΥ) Ў=Math.Max(0,1-ΑΛ/ΑΔ)*Ў;if(generalSpeedLimit>0) Ў=Math.Min(generalSpeedLimit,Ў);if(Ŗ) Ў=0;if(ǰ>=0) Ў=Math.Min(Ў,ǰ);if(ΖΦ) Ў*=(float) Math.Min(1,Џ(Ǭ.Length(),wpReachedDist)/2);Vector3 ΕЊ=а(Υ,Υ.GetShipVelocities().LinearVelocity);float ΗЂ=(float)(Math.Max(0,15-ı(-Ζε,-ΕЊ))/15)*0.85f+0.15f;
ΔΒ+=Math.Sign(ΗЂ-ΔΒ)/10f;Vector3 Ȫ=Ζε*Ў*ΔΒ-(ΕЊ);Vector3 ΒЈ=ΒΧ(Ȫ,null);if(ΑЁ&&Л>0.1f){Ȫ.X*=Βχ(Ȫ.X,ref ΑЂ.X,1f,ΒЈ.X,20);Ȫ.Y*=Βχ(Ȫ.Y,ref ΑЂ.Y,1f,ΒЈ.Y,20);Ȫ.Z*=Βχ(Ȫ.Z,ref ΑЂ.Z,1f,ΒЈ.Z,20);}else ΑЂ=new Vector3(1,1,1);ơ=Ĩ*Ȫ-Ō*Ĩ;ΔΪ(ơ,ΑЁ);Л=Vector3.Distance(ΰ,Ƌ);
}float Βχ(float value,ref float У,float ћ,float Ȗ,float Γσ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(У)) У+=Math.Sign(У)*ћ;else У=value;if(value==0) У=1;float έ=Math.Abs(У);if(έ<Γσ||Ȗ==0) return έ;У=Math.Min(Γσ,Math.Max(-Γσ,У));έ=Math.Abs(Ȗ);return έ;
}void ΕΑ(){float ъ=90,џ=90,њ=90;float ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǩ,ѕ,Ɠ,Γλ;if(Γμ){Ǩ=Vector3.Normalize(ψ-ΰ);Ɠ=а(Υ,Ǩ);ѕ=а(Υ,ΓБ);ъ=ı(Ɠ,new Vector3(0,-1,0))-90;џ=ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);
њ=ċ(Ɠ,new Vector3(-1,0,0),Ɠ.Z);}else{Ǩ=ΔΑ;ѕ=а(Υ,ΓБ);Ɠ=а(Υ,ΔΑ);Γλ=а(Υ,Ζζ);ъ=ċ(ѕ,new Vector3(0,0,1),ѕ.Y);џ=ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);њ=ċ(Γλ,new Vector3(0,0,1),Γλ.X);}if(ΕΕ&&ƺ()){Vector3 ΓΧ=Υ.GetNaturalGravity();ѕ=а(Υ,ΓΧ);ъ=ċ(ѕ,new Vector3(0,0,1),ѕ.Y);
џ=ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);}if(!Βή(-45,џ,45)){ъ=0;њ=0;}if(!Βή(-45,њ,45)) ъ=0;ǆ(ΓΩ,1,(-ъ)*ǜ,(-њ)*ǜ,(-џ)*ǜ);ΑΛ=Math.Max(Math.Abs(ъ),Math.Max(Math.Abs(џ),Math.Abs(њ)));Αΰ=ΑΛ<=ΑΔ;}void Ζϊ(IMyTerminalBlock Ατ){if(Ατ==null) return;ǟ=new float[3,2];ќ=new Dictionary<string,float[,]>();
for(int i=0;i<ȇ.Count;i++){IMyThrust г=ȇ[i];if(!г.IsFunctional) continue;Vector3 κ=а(Ατ,г.WorldMatrix.Backward);float ȭ=г.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǟ[0,0]+=ȭ;else ǟ[0,1]-=ȭ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǟ[1,0]+=ȭ;else ǟ[1,1]-=ȭ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǟ[2,0]+=ȭ;else ǟ[2,1]-=ȭ;String ь=Δς(г);float[,]ѝ=null;if(ќ.ContainsKey(ь)) ѝ=ќ[ь];else{ѝ=new float[3,2];ќ.Add(ь,ѝ);}float Ȗ=г.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ѝ[0,0]+=Ȗ;else ѝ[0,1]-=Ȗ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ѝ[1,0]+=Ȗ;else ѝ[1,1]-=Ȗ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ѝ[2,0]+=Ȗ;else ѝ[2,1]-=Ȗ;}}static String Δς(IMyThrust г){return г.BlockDefinition.SubtypeId;}Vector3 Βΰ(Vector3 ђ,float[,]Ŏ){return new Vector3(
ђ.X>=0?Ŏ[0,0]:Ŏ[0,1],
ђ.Y>=0?Ŏ[1,0]:Ŏ[1,1],
ђ.Z>=0?Ŏ[2,0]:Ŏ[2,1]);}bool Δρ(Χ Ψ,String ь,out float ΓΠ){ΓΠ=0;int Β=Ȇ.IndexOf(ь);if(Β==-1||Ψ.Ǟ==null||Β>=Ψ.Ǟ.Length)
return false;ΓΠ=Ψ.Ǟ[Β];if(ΓΠ==-1) return false;return true;}Vector3 ΒΧ(Vector3 ђ,Χ Ψ){if(Ψ!=null){Vector3 έ=new Vector3();for(int i=0;i<ќ.Keys.Count;i++){String ь=ќ.Keys.ElementAt(i);float Ɖ=0;if(!Δρ(Ψ,ь,out Ɖ))
return Βΰ(ђ,ǟ);έ+=Βΰ(ђ,ќ.Values.ElementAt(i))*Ɖ;}return έ;}return Βΰ(ђ,ǟ);}float Ū(Vector3 ђ,Χ Ψ){return Ū(ђ,new Vector3(),Ψ);}float Ū(Vector3 ђ,Vector3 ν,Χ Ψ){Vector3 ΘЃ=ΒΧ(ђ,Ψ);Vector3 ȭ=ΘЃ+ν*Ĩ;float Ƙ=(ȭ/ђ).AbsMin();return(float)(ђ*Ƙ).Length();}float Ʈ(Vector3 Αζ,Vector3 Ō,Χ Ψ)
{if(Αζ.Length()==0) return 0;float ΕΔ=1;if(Ō.Length()>0) ΕΔ=Math.Min(1,ı(-Ō,Αζ)/90)*0.4f+0.6f;float ơ=Ū(Αζ,Ō,Ψ);if(ơ==0) return 0.1f;float ΑΓ=Џ(ơ,Ĩ);float ΔΕ=(float) Math.Sqrt(Џ(Αζ.Length(),ΑΓ*0.5f));return ΑΓ*ΔΕ*ΕΔ*Ǥ;}float Ʒ(Vector3 Ǩ,Vector3 ώ,Vector3 ν,Χ Ψ)
{if(ν.Length()==0f) return 0;Vector3 Αη=đ(Ǩ,ώ,Vector3.Normalize(ν));float ǖ=Ū(-Αη,Ψ);return ǖ/ν.Length();}void Δέ(bool ΘΣ){float ў=0;float ΓΤ=0.9f;if(ΘΣ){О=-1;Ĭ=0;Αώ=null;if(ή!=Λ.з&&Ε.ν.Length()!=0){ў=ΓΤ*Ʒ(Ε.β,Ε.α*-1,Ε.ν,null);if(ў<О||О==-1) О=ў;}if(Θ.γ&&Θ.ν.Length()!=0)
{ў=ΓΤ*Ʒ(Θ.β,Θ.α*-1,Θ.ν,null);if(ў<О||О==-1) О=ў;}return;}if(Ĭ==-1) return;if(Ĭ>=0){int ΖΝ=0;while(Ĭ<Σ.Count){if(ΖΝ>100) return;ΖΝ++;Χ Ψ=Σ[Ĭ];if(Ψ.ν.Length()!=0f){ў=ΓΤ*Math.Min(
Ʒ(Ψ.β,Ψ.α*-1,Ψ.ν,Ψ),
Ʒ(Ψ.β*-1,Ψ.α*-1,Ψ.ν,Ψ));if(ў<О||О==-1) О=ў;}else Αώ=Ψ;Ĭ++;}Ĭ=-1;}bool ΓΪ=true;float Ă=0;if(Αώ!=null){for(int i=0;i<ќ.Count;i++){String ь=ќ.Keys.ElementAt(i);float[,]ѝ=ќ.Values.ElementAt(i);float Ɖ=0;if(!Δρ(Αώ,ь,out Ɖ)){ΓΪ=false;break;}for(int a=0;a<ѝ.GetLength(0);a++)
for(int b=0;b<ѝ.GetLength(1);b++){float Ĕ=Math.Abs(ѝ[a,b]*Ɖ);if(Ĕ==0) continue;ΓΪ=true;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}}if(!ΓΪ){for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float Ĕ=Math.Abs(ǟ[a,b]);if(Ĕ==0) continue;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}if(Ă>0){float Θί=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ў=Џ(Ă,Θί);if(ў>0)
if(ў<О||О==-1) О=ў;}}void ΔΪ(Vector3 ǖ,bool Ї){if(!Ї){for(int i=0;i<ȇ.Count;i++)
ȇ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЇ=ΒΧ(ǖ,null);float ΙΙ=Math.Min(1,Math.Abs(Џ(ǖ.X,ΒЇ.X)));float ΙΚ=Math.Min(1,Math.Abs(Џ(ǖ.Y,ΒЇ.Y)));float ΙΛ=Math.Min(1,Math.Abs(Џ(ǖ.Z,ΒЇ.Z)));for(int i=0;i<ȇ.Count;i++){IMyThrust г=ȇ[i];Vector3 κ=Ηά(а(Υ,г.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǖ.X))
г.SetValueFloat("Override",г.MaxThrust*ΙΙ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǖ.Y))
г.SetValueFloat("Override",г.MaxThrust*ΙΚ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǖ.Z))
г.SetValueFloat("Override",г.MaxThrust*ΙΛ);else
г.SetValueFloat("Override",0.0f);}}void ǆ(bool Ї,float Θλ,float ъ,float њ,float џ){for(int i=0;i<ΑΫ.Count;i++){IMyGyro Ѕ=ΑΫ[i];Ѕ.GyroOverride=Ї;if(!Ї) Ѕ.GyroPower=100;else Ѕ.GyroPower=Θλ;if(!Ї) continue;Vector3 Б=Υ.WorldMatrix.Forward;Vector3 Ь=Υ.WorldMatrix.Right;Vector3 ώ=Υ.WorldMatrix.Up;
Vector3 ń=Ѕ.WorldMatrix.Forward;Vector3 ņ=Ѕ.WorldMatrix.Up;Vector3 Ņ=Ѕ.WorldMatrix.Left*-1;if(ń==Б) Ѕ.SetValueFloat("Roll",џ);else if(ń==(Б*-1)) Ѕ.SetValueFloat("Roll",џ*-1);else if(ņ==(Б*-1)) Ѕ.SetValueFloat("Yaw",џ);else if(ņ==Б) Ѕ.SetValueFloat("Yaw",џ*-1);else if(Ņ==Б) Ѕ.SetValueFloat("Pitch",џ);
else if(Ņ==(Б*-1)) Ѕ.SetValueFloat("Pitch",џ*-1);if(Ņ==(Ь*-1)) Ѕ.SetValueFloat("Pitch",ъ);else if(Ņ==Ь) Ѕ.SetValueFloat("Pitch",ъ*-1);else if(ņ==Ь) Ѕ.SetValueFloat("Yaw",ъ);else if(ņ==(Ь*-1)) Ѕ.SetValueFloat("Yaw",ъ*-1);else if(ń==(Ь*-1)) Ѕ.SetValueFloat("Roll",ъ);
else if(ń==Ь) Ѕ.SetValueFloat("Roll",ъ*-1);if(ņ==(ώ*-1)) Ѕ.SetValueFloat("Yaw",њ);else if(ņ==ώ) Ѕ.SetValueFloat("Yaw",њ*-1);else if(Ņ==ώ) Ѕ.SetValueFloat("Pitch",њ);else if(Ņ==(ώ*-1)) Ѕ.SetValueFloat("Pitch",њ*-1);else if(ń==ώ) Ѕ.SetValueFloat("Roll",њ);else if(ń==(ώ*-1)) Ѕ.SetValueFloat("Roll",њ*-1);
}}void ΔЎ(){this.ΓΩ=false;}void Ф(Vector3 Ȭ,Vector3 Б,Vector3 Γι,float Ȝ,bool Ʀ){ΑЌ(Ȭ,Ȝ,Ʀ);ΑΔ=Ȝ;Γμ=false;this.ΔΑ=Б;this.Ζζ=Γι;}void Ф(Vector3 Ȭ,Vector3 Б,Vector3 Γι,bool Ʀ){Ф(Ȭ,Б,Γι,2f,Ʀ);}void ΑЌ(Vector3 Ȭ,float Ȝ,bool Ʀ){ΑΔ=Ȝ;this.ΓΩ=true;this.ΕΕ=Ʀ;Γμ=true;Αΰ=false;
this.ΓБ=Ȭ;}void Βό(){Ǣ(false,false,false,ψ,0);ΑЁ=false;}void П(Vector3 Љ,float ǜ){Ǣ(true,false,false,Љ,ǜ);}void Ǣ(bool Ї,bool ΓЌ,bool ΔΝ,Vector3 Љ,float ǜ){Ǣ(Ї,ΓЌ,ΔΝ,Љ,Љ-ΰ,0.0f,ǜ);}void Ǣ(bool Ї,bool ΓЌ,bool ΔΝ,Vector3 Љ,Vector3 Θχ,float Θψ,float ǜ){ΑЁ=true;this.Ζθ=Ї;
ψ=Љ;this.Γς=ǜ;this.Γύ=Θψ;this.ΖΦ=ΓЌ;this.ΖΥ=ΔΝ;this.Εξ=Θχ;Л=Vector3.Distance(Љ,ΰ);}void Ηέ(){ω.GetBlocksOfType(Ź,ǎ);}void Ηή(){if(Αφ){ύ=2;return;}List<IMyRemoteControl>ƙ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΣ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζέ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(ƙ,ǎ);ω.GetBlocksOfType(є,ǎ);ω.GetBlocksOfType(ΖΣ,ǎ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζέ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);є=ǡ(є,pamTag,true);Ηε();Ηζ(Ζέ);Ŵ(є,setLCDFontAndSize,1.4f,false);Ŵ(ą,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζα=ǡ(ΖΣ,pamTag,true);if(Ζα.Count>0) ξ=Ζα[0];else ξ=null;if(Η==Ζ.ű){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.Ǎ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϊ){ω.GetBlocksOfType(ΔΖ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζΰ=ǡ(ƙ,pamTag,true);if(Ζΰ.Count>0) ƙ=Ζΰ;if(ƙ.Count>0) Υ=ƙ[0];else{Υ=null;ύ=2;Μ="Remote is missing";return;}θ=(float) Υ.CubeGrid.WorldVolume.Radius*2;љ=null;if(Η!=Ζ.Ϊ){Δή();if(ο.Count>0&&љ!=null){if(ξ!=null&&(љ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Υ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(љ.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
љ.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ηί(){ω.GetBlocksOfType(ȃ,ǎ);ω.GetBlocksOfType(т,ǎ);ω.GetBlocksOfType(ȇ,ǎ);ω.GetBlocksOfType(ΑΫ,ǎ);ω.GetBlocksOfType(Ȩ,ǎ);ω.GetBlocksOfType(ă,ǎ);ω.GetBlocksOfType(ѓ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(Αύ,ǎ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǡ(т,"ConnectorMedium",false);else
т=ǡ(т,"Connector",false);List<IMyShipConnector>Ζή=ǡ(т,pamTag,true);if(Ζή.Count>0) т=Ζή;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(ΑΫ.Count==0){ύ=1;Μ="Gyros are missing";}if(ȇ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζβ=ǡ(Αύ,pamTag,true);
if(Ζβ.Count>0) Αύ=Ζβ;List<IMyLandingGear>Ζί=ǡ(ȃ,pamTag,true);if(Ζί.Count>0) ȃ=Ζί;for(int i=0;i<ȃ.Count;i++) ȃ[i].AutoLock=false;List<IMyBatteryBlock>Ζά=ǡ(Ȩ,pamTag,true);if(Ζά.Count>0) Ȩ=Ζά;List<IMyGasTank>Ζγ=ǡ(ѓ,pamTag,true);if(Ζγ.Count>0) ѓ=Ζγ;}void Ηΰ(){ω.GetBlocksOfType(ĕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ŀ.Clear();for(int i=ĕ.Count-1;i>=0;i--){if(ΗΚ(ĕ[i])){ŀ.Add(ĕ[i]);ĕ.RemoveAt(i);}}}bool ΗΚ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΒΫ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δή(){if(Υ==null) return;љ=null;float Αο=0,Ακ=0,Απ=0,Αλ=0,Αρ=0,Αμ=0;List<IMyTerminalBlock>ΓА=ǡ(ο,pamTag,true);bool Θζ=ΓА.Count==0;if(ΓА.Count>0) љ=ΓА[0];else if(ο.Count>0) љ=ο[0];int Ȁ=0;float ŗ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward){if(Θζ){ύ=2;Μ="Mining direction is unclear!";return;}continue;}Ȁ++;ŗ=Math.Max(ŗ,Ηο(ο[i]));Vector3 Ј=ƶ(Υ,ο[i].GetPosition());if(i==0){Αο=Ј.X;Ακ=Ј.X;Απ=Ј.Y;Αλ=Ј.Y;Αρ=Ј.Z;Αμ=Ј.Z;}Ακ=Math.Max(Ј.X,Ακ);Αο=Math.Min(Ј.X,Αο);
Αλ=Math.Max(Ј.Y,Αλ);Απ=Math.Min(Ј.Y,Απ);Αμ=Math.Max(Ј.Z,Αμ);Αρ=Math.Min(Ј.Z,Αρ);}if(ŗ==0) ŗ=drillRadius;bool Ȭ=љ!=null&&љ.WorldMatrix.Forward==Υ.WorldMatrix.Down;ǯ=(Ακ-Αο)*(1-ĭ)+ŗ*2;Ǯ=(Ȭ?Αμ-Αρ:Αλ-Απ)*(1-ĝ)+ŗ*2;IMyCubeGrid g=Υ.CubeGrid;float ΔΣ=0,ΔΤ=0,ΔΥ=0,ΔΦ=0;
for(int c=0;c<8;c++){Vector3 p=ƶ(Υ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȭ?p.Z:p.Y;if(c==0||p.X<ΔΣ) ΔΣ=p.X;if(c==0||p.X>ΔΤ) ΔΤ=p.X;if(c==0||y<ΔΥ) ΔΥ=y;if(c==0||y>ΔΦ) ΔΦ=y;}Αϊ=ΔΤ-ΔΣ+g.GridSize;Αω=ΔΦ-ΔΥ+g.GridSize;}float Ηο(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǡ<T>(List<T>С,String ǝ,bool Αά){List<T>έ=new List<T>();for(int i=0;i<С.Count;i++)
if(ΒΫ(С[i],ǝ,Αά)) έ.Add(С[i]);return έ;}bool ΒΫ<T>(T Τ,String ǝ,bool Αά){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αά&&b.CustomName.ToUpper().Contains(ǝ.ToUpper())) return true;if(!Αά&&b.BlockDefinition.ToString().ToUpper().Contains(ǝ.ToUpper())) return true;return false;}void ѐ<T>(List<T>С,bool Ї)
{for(int i=0;i<С.Count;i++)
ΔЉ((IMyTerminalBlock) С[i],Ї);}void ΔЉ(IMyTerminalBlock Τ,bool Ї){if(Τ==null) return;String ΗЀ=Ї?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЀ);a.Apply(Τ);}void ǣ(bool Ї){ΓЍ=Ї;if(!ĩ) return;ѐ(Αύ,Ї);}void Βσ(ChargeMode Ɣ){for(int i=0;i<Ȩ.Count;i++) Ȩ[i].ChargeMode=Ɣ;}void Βυ(bool ΘϏ)
{for(int i=0;i<ѓ.Count;i++) ѓ[i].Stockpile=ΘϏ;}void ŵ(List<IMyLandingGear>ΓΥ,bool ΕЋ){for(int i=0;i<ΓΥ.Count;i++){if(ΕЋ) ΓΥ[i].Lock();if(!ΕЋ) ΓΥ[i].Unlock();}}void ΔЊ(bool Ї){if(Εέ==Ї) return;List<IMyShipController>ΑΙ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΙ,ǎ);if(ΑΙ.Count==0) return;
for(int i=0;i<ΑΙ.Count;i++)
ΑΙ[i].DampenersOverride=Ї;Εέ=Ї;}IMyShipConnector З(MyShipConnectorStatus ы){for(int i=0;i<т.Count;i++){if(!Ƕ(т[i],true)) continue;if(т[i].Status==ы) return т[i];}return null;}void Ē(bool ΓЉ,bool ΑΟ,float ΘΙ,float ΗЅ){if(ξ==null||ο.Count==0) return;Vector3 ΑΘ=new Vector3();
int Ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward) continue;Ȁ++;ΑΘ+=ο[i].GetPosition();}ΑΘ=ΑΘ/Ȁ;Vector3 œ=ƶ(ξ,ΑΘ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЉ;ξ.LeftExtend=(ΑΟ?1:й)/2f*ǯ-œ.X;ξ.RightExtend=(ΑΟ?1:й)/2f*ǯ+œ.X;ξ.TopExtend=(ΑΟ?1:и)/2f*Ǯ+œ.Y;
ξ.BottomExtend=(ΑΟ?1:и)/2f*Ǯ-œ.Y;ξ.FrontExtend=(ΓЉ?Х:ΘΙ)-œ.Z;ξ.BackExtend=ΓЉ?0:ΗЅ+θ*0.75f+œ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ŵ<T>(List<T>С,bool Θϊ,float ΘΗ,bool ΘΤ){for(int i=0;i<С.Count;i++){IMyTextSurface Ɵ=null;if(С[i]is IMyTextSurface) Ɵ=(IMyTextSurface) С[i];if(Ɵ!=null){Ɵ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Θϊ) continue;Ɵ.Font="Debug";if(ΘΤ) continue;Ɵ.FontSize=ΘΗ;}}}bool ƺ(){Vector3D η;return this.Υ.TryGetPlanetPosition(out η);}int ΗΑ(Vector3 ΘЁ,int ΖΧ){if(ΖΧ==-1) return 0;double А=-1;int Ȑ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘЁ);if(А==-1||ό<А){Ȑ=i;А=ό;}
}return Math.Sign(Ȑ-ΖΧ);}bool ΗΜ(Vector3 η){List<Vector3>Е=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Е.Add(Σ[i].Δ);if(Θ.γ&&Σ.Count>=1){Vector3 Ļ=new Vector3();ǵ(Θ,dockDist*θ,false,out Ļ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Е.Insert(0,Ļ);Е.Insert(0,Θ.Δ);}
else{Е.Add(Ļ);Е.Add(Θ.Δ);}}if(Η==Ζ.Ϊ){if(Ε.γ&&Σ.Count>=1){Vector3 ΒЃ=new Vector3();ǵ(Ε,dockDist*θ,false,out ΒЃ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Е.Insert(0,ΒЃ);Е.Insert(0,Ε.Δ);}else{Е.Add(ΒЃ);Е.Add(Ε.Δ);}}}else{if(ή!=Λ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Е.Insert(0,Ε.Δ);else Е.Add(Ε.Δ);}int Ȑ=-1;double Ǧ=-1;for(int i=0;i<Е.Count;i++){double ό=Vector3.Distance(Е[i],η);if(ό<Ǧ||Ǧ==-1){Ǧ=ό;Ȑ=i;}}if(Е.Count==0) return false;double ΓΚ=Vector3.Distance(Е[Ȑ],η);double Θμ=Vector3.Distance(Е[Math.Max(0,Ȑ-1)],Е[Ȑ])*1.5f;
double Θε=Vector3.Distance(Е[Math.Min(Е.Count-1,Ȑ+1)],Е[Ȑ])*1.5f;return ΓΚ<Θμ||ΓΚ<Θε||ΓΚ<laneOffset*θ*1.5f;}static float Џ(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηά(Vector3 v,int ΓΗ){return new Vector3(Math.Round(v.X,ΓΗ),Math.Round(v.Y,ΓΗ),Math.Round(v.Z,ΓΗ));
}float ı(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΖ=(a*b).Sum;float ΕІ=a.Length();float ΕЇ=b.Length();if(ΕІ==0||ΕЇ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΖ/(ΕІ*ΕЇ)));if(float.IsNaN(έ)) return 0;return έ;}float ċ(Vector3 v,Vector3 Ατ,float Θϋ){float Ĺ=ı(v,Ατ);
if(Θϋ>0f) Ĺ*=-1;if(Ĺ>-90f) return Ĺ-90f;else return 180f-(-Ĺ-90f);}double Δδ(float ΗА){return(Math.PI/180)*ΗА;}bool Βή(double Θή,double value,double Αι){return(value>=Θή&&value<=Αι);}Vector3 Ηπ(IMyTerminalBlock Τ,Vector3 Ј){return Vector3D.Transform(Ј,Τ.WorldMatrix);
}Vector3 ƶ(IMyTerminalBlock Τ,Vector3 ΙΖ){return а(Τ,ΙΖ-Τ.GetPosition());}Vector3 а(IMyTerminalBlock Τ,Vector3 ΔΡ){return Vector3D.TransformNormal(ΔΡ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 đ(Vector3 Ǩ,Vector3 ώ,Vector3 ΔΡ){MatrixD Γρ=MatrixD.CreateFromDir(Ǩ,ώ);return Vector3D.TransformNormal(ΔΡ,MatrixD.Transpose(Γρ));
}Vector3 Βύ(Vector3 Ǩ,Vector3 ώ,Vector3 Ј){MatrixD Γρ=MatrixD.CreateFromDir(Ǩ,ώ);return Vector3D.Transform(Ј,Γρ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 λ(String s){String[]Д=s.Split('|');return new Vector3(float.Parse(Κ(Д,0)),float.Parse(Κ(Д,1)),float.Parse(Κ(Д,2)));
}String Ηώ(Χ Ψ){String Њ=":";String έ=τ(Ψ.Δ)+Њ+τ(Ψ.β)+Њ+
τ(Ψ.α)+Њ+τ(Ψ.χ)+Њ+τ(Ψ.ν);for(int i=0;i<Ψ.Ǟ.Length;i++)
έ+=Њ+Math.Round(Ψ.Ǟ[i],3);return έ;}Χ Ηλ(String s){String[]Д=s.Split(':');Χ Ψ=new Χ(
λ(Κ(Д,0)),λ(Κ(Д,1)),
λ(Κ(Д,2)),λ(Κ(Д,3)),
λ(Κ(Д,4)));int Β=5;List<float>Ει=new List<float>();while(Β<Д.Length){String ΑЇ=Κ(Д,Β);float f=0;if(!float.TryParse(ΑЇ,out f)) break;Ει.Add(f);Β++;}Ψ.Ǟ=Ει.ToArray();return Ψ;}System.Text.StringBuilder ś=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θδ){if(Θδ) ś.Append('\n');ś.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʋ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ǋ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]ΗЄ,int Β){String ΑЇ=ΗЄ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑЇ)) return "";
return ΑЇ;}void Save(){if(Αφ||Η==Ζ.р){Storage="";return;}Storage=ΒΑ();}String ΒΑ(){ś.Clear();ś.Append(DATAREV+";");Ρ(τ(Αυ),false);Ρ(τ(Θ.β));Ρ(τ(Θ.χ));Ρ(τ(Θ.α));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ц));Ρ(Θ.γ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.β));Ρ(τ(Ε.α));Ρ(τ(Ε.χ));Ρ(τ(Ε.Ц));Ρ(Ε.γ);Ρ(τ(Ǜ));Ρ(τ(ȗ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(ǩ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(Ǥ);Ρ(Ȟ);Ρ(ȝ);if(Η==Ζ.Ϊ){Ρ((int) ϊ.Ћ);Ρ(ϊ.ǔ);Ρ(ϊ.Ȯ);Ρ(Ʋ(ϊ.Ǖ));Ρ(Ʋ(ϊ.ǘ));Ρ((int) ϋ.Ћ);Ρ(ϋ.ǔ);Ρ(ϋ.Ȯ);Ρ(Ʋ(ϋ.Ǖ));Ρ(Ʋ(ϋ.ǘ));}else{Ρ((int) ț);Ρ((int) л);Ρ((int) М);Ρ((int) у);Ρ((int) ġ);Ρ(ć);Ρ(ĩ);Ρ(ŉ);Ρ(Ń);Ρ(й);Ρ(и);Ρ(Х);Ρ(ť);Ρ(Ť);Ρ(ĭ);Ρ(ĝ);
Ρ(Н);Ρ(в);Ρ(Ш);Ρ(Щ);Ρ(ȕ);Ρ(К);Ρ(Ħ);Ρ(Ĥ);}Ρ(";");for(int i=0;i<Ȇ.Count;i++)
Ρ((i>0?"|":"")+Ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(Ηώ(Σ[i]),i>0);Ρ(";",false);Ρ(Ч+"|"+Ѐ,false);Ρ(";",false);Ρ(ΖЉ(),false);String Ϋ=ś.ToString();ś.Clear();return Ϋ;}с ΗΫ(){return ǃ(Storage);}с ǃ(String Ϋ){if(Ϋ=="") return с.ΗΤ;String[]ħ=Ϋ.Split(';');if(Κ(ħ,0)!=DATAREV) return с.Βγ;int Β=0;try{String[]Ι=Κ(ħ,1).Split('\n');
Αυ=λ(Κ(Ι,Β++));Θ.β=λ(Κ(Ι,Β++));Θ.χ=λ(Κ(Ι,Β++));Θ.α=λ(Κ(Ι,Β++));Θ.ν=λ(Κ(Ι,Β++));Θ.Δ=λ(Κ(Ι,Β++));Θ.Ц=λ(Κ(Ι,Β++));Θ.γ=bool.Parse(Κ(Ι,Β++));Ε.Δ=λ(Κ(Ι,Β++));Ε.ν=λ(Κ(Ι,Β++));Ε.β=λ(Κ(Ι,Β++));Ε.α=λ(Κ(Ι,Β++));Ε.χ=λ(Κ(Ι,Β++));Ε.Ц=λ(Κ(Ι,Β++));Ε.γ=bool.Parse(Κ(Ι,Β++));
Ǜ=λ(Κ(Ι,Β++));ȗ=λ(Κ(Ι,Β++));Ι=Κ(ħ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Κ(Ι,Β++));ή=(Λ) int.Parse(Κ(Ι,Β++));ц=(Λ) int.Parse(Κ(Ι,Β++));ǩ=int.Parse(Κ(Ι,Β++));ч=int.Parse(Κ(Ι,Β++));Ы=int.Parse(Κ(Ι,Β++));ш=int.Parse(Κ(Ι,Β++));Ǥ=float.Parse(Κ(Ι,Β++));Ȟ=bool.Parse(Κ(Ι,Β++));
ȝ=bool.Parse(Κ(Ι,Β++));if(Η==Ζ.Ϊ){ϊ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϊ.ǔ=float.Parse(Κ(Ι,Β++));ϊ.Ȯ=float.Parse(Κ(Ι,Β++));ϊ.Ǖ=ǋ(Κ(Ι,Β++));ϊ.ǘ=ǋ(Κ(Ι,Β++));ϋ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϋ.ǔ=float.Parse(Κ(Ι,Β++));ϋ.Ȯ=float.Parse(Κ(Ι,Β++));ϋ.Ǖ=ǋ(Κ(Ι,Β++));ϋ.ǘ=ǋ(Κ(Ι,Β++));}else{ț=(Ǐ) int.Parse(Κ(Ι,Β++));
л=(е) int.Parse(Κ(Ι,Β++));М=(Ϗ) int.Parse(Κ(Ι,Β++));у=(э) int.Parse(Κ(Ι,Β++));ġ=(Ǐ) int.Parse(Κ(Ι,Β++));ć=bool.Parse(Κ(Ι,Β++));ĩ=bool.Parse(Κ(Ι,Β++));ŉ=bool.Parse(Κ(Ι,Β++));Ń=bool.Parse(Κ(Ι,Β++));й=int.Parse(Κ(Ι,Β++));и=int.Parse(Κ(Ι,Β++));Х=int.Parse(Κ(Ι,Β++));ť=float.Parse(Κ(Ι,Β++));
Ť=float.Parse(Κ(Ι,Β++));ĭ=float.Parse(Κ(Ι,Β++));ĝ=float.Parse(Κ(Ι,Β++));Н=int.Parse(Κ(Ι,Β++));в=int.Parse(Κ(Ι,Β++));Ш=int.Parse(Κ(Ι,Β++));Щ=int.Parse(Κ(Ι,Β++));ȕ=int.Parse(Κ(Ι,Β++));К=int.Parse(Κ(Ι,Β++));Ħ=int.Parse(Κ(Ι,Β++));Ĥ=float.Parse(Κ(Ι,Β++));}Ι=Κ(ħ,3).Replace("\n","").Split('|');
Ȇ=Ι.ToList();Ι=Κ(ħ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηλ(Κ(Ι,i)));Ι=Κ(ħ,5).Split('|');Ч=Κ(Ι,0);if(!int.TryParse(Κ(Ι,1),out Ѐ)) Ѐ=-1;if(Ч.Length!=Н*в){Ч="";Ѐ=-1;}ΖЈ(Κ(ħ,6));}catch{return с.Δθ;}Ŝ=ц;φ();return с.ǈ;}const int Δά=64000;IMyTerminalBlock Δι(){List<IMyTerminalBlock>ľ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ľ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ľ.Count>0?ľ[0]:null;}void ΖЌ(){IMyTerminalBlock Τ=Δι();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Ϋ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΑ();if(Ϋ.Length>Δά){Μ="Export too large: "+Ϋ.Length+" chars, max "+Δά;return;}Τ.CustomData=Ϋ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΖ(bool Γε){IMyTerminalBlock Τ=Δι();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΗ(Τ.CustomData,Γε);}void ΗΗ(String Ϋ,bool Γε){Ϋ=Ϋ.Replace("\r","");if(Ϋ.StartsWith("//")){int ΖΕ=Ϋ.IndexOf('\n');Ϋ=ΖΕ<0?"":Ϋ.Substring(ΖΕ+1);}Ϋ=Ϋ.Trim();if(Ϋ==""){Μ="Import: no PAM data in "+dataTag;return;}φ();String ΕΝ=ΒΑ();с έ=ǃ(Ϋ);if(έ==с.ǈ&&Γε){Χ ŋ=new Χ(Ε);
Vector3 Θα=Ǜ,Θΰ=ȗ;int w=й,h=и,d=Х;Ǐ ΓЎ=ț;э ΓΘ=у;float Θι=ĭ,Θθ=ĝ;ǃ(ΕΝ);Ε=ŋ;Ǜ=Θα;ȗ=Θΰ;й=w;и=h;Х=d;ț=ΓЎ;у=ΓΘ;ĭ=Θι;ĝ=Θθ;ή=Λ.Ĵ;ŷ(true,true);ή=Λ.Ĵ;ц=Λ.Ĵ;Μ="Imported job, run CONT";return;}if(έ==с.ǈ){ƕ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ǃ(ΕΝ);Μ=έ==с.Βγ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ηε(){ƃ.Clear();for(int i=є.Count-1;i>=0;i--){String Ϋ=є[i].CustomData.ToUpper();bool Γγ=false;if(Ϋ=="INSTRUCTIONS"){Γγ=true;ΑΞ=true;}if(Ϋ=="DEBUG") Γγ=true;if(Γγ){ƃ.Add(є[i]);є.RemoveAt(i);}
}Ŵ(ƃ,false,1,false);}void Ηζ(List<IMyTerminalBlock>ľ){ą.Clear();for(int i=0;i<ľ.Count;i++){IMyTerminalBlock Τ=ľ[i];try{String ΖΜ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΜ);int Ş=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΜ.Length,1),out Ş)) continue;
if(Ş==-1) continue;Ş--;IMyTextSurfaceProvider ΖΞ=(IMyTextSurfaceProvider) Τ;if(Ş<ΖΞ.SurfaceCount&&Ş>=0)
ą.Add(ΖΞ.GetSurface(Ş));}catch{}}}String Αν="";void ΔЅ(){String Γπ="";String ΓΟ="";Αν=ΔΫ(false);Γπ+=Αν;ΓΟ+=Αν;ΓΟ+=ΖА();for(int i=0;i<є.Count;i++) є[i].WriteText(Γπ);for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γπ);Echo(ΓΟ);for(int i=0;i<ƃ.Count;i++){IMyTextPanel Γό=ƃ[i];
String Ϋ=Γό.CustomData.ToUpper();if(Ϋ=="DEBUG") Γό.WriteText(""+"\n"+"");if(Ϋ=="INSTRUCTIONS") Γό.WriteText(ΖБ());}}string ΖБ(){String Γ="";try{float ΕЎ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ľ*ΕЎ)+" / "+(int)(ŏ*ΕЎ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ľ*100f,1)+"% / "+Math.Round(ŏ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ż,2)+"ms / "+Math.Round(Γτ,2)+"ms \n";}catch{}for(int i=0;i<Ș.Count;i++)
Γ+=""+Ș.Keys.ElementAt(i)+" = "+Math.Round(Ș.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ș.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΓ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ŵ(є,setLCDFontAndSize,0.9f,false);Ŵ(ą,setLCDFontAndSize,0.9f,true);for(int i=0;i<є.Count;i++) є[i].WriteText(Γ+Ȕ.ToString());for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γ+Ȕ.ToString());}void Ǹ(){if(!ΑΞ) return;try{ΓЁ=Runtime.CurrentInstructionCount;}catch{}
}void Ǵ(String υ){if(!ΑΞ) return;if(ΓЁ==0) return;try{float ΑЅ=(Runtime.CurrentInstructionCount-ΓЁ)/Runtime.MaxInstructionCount*100;if(!Ș.ContainsKey(υ))
Ș.Add(υ,new float[]{ΑЅ,ΑЅ});else{Ș[υ][0]=ΑЅ;Ș[υ][1]=Math.Max(ΑЅ,Ș[υ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζφ(String Ȱ){if(!Ŀ) return;String Њ=""+BROADCAST_SEP;String ρ=VERSION+Њ;ρ+=Ŧ(ΓЈ)+Њ;
ρ+=(int) Η+Њ;ρ+=ΒΡ(ΰ.X)+""+Њ;ρ+=ΒΡ(ΰ.Y)+Њ;ρ+=ΒΡ(ΰ.Z)+Њ;if(Η!=Ζ.Ϊ)
ρ+=ΒΤ(ц)+(ц==Λ.Є?" "+Math.Round(Ə,1)+"%":"")+Њ;else ρ+=ΒΤ(ц)+Њ;ρ+=Ŧ(Μ)+Њ;ρ+=і+""+Њ;ρ+=Ŧ(Αν)+Њ;ρ+=Ł+Њ;ρ+=Ő+Њ;for(int i=0;i<Ǘ.Count;i++){if(Ǘ[i].ō!=И.Ŭ) continue;ρ+=Ŧ(Ǘ[i].υ)+"/"+Ŧ(Ǘ[i].ς)+"/"+Ǘ[i].Ǒ+Њ;}Ηβ(ρ,Ȱ);}String Ŧ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηβ(String ρ,String Ȱ){Ηγ(ρ,BROADCAST_SELF_ID,Ȱ);}void Ηγ(String ρ,String Љ,String Ȱ){try{if(!Ŀ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δμ()+BROADCAST_SEP+Љ+BROADCAST_SEP+ΒЍ+BROADCAST_SEP+Ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȕ=e;}
}String Δμ(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Δϋ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ũ(ref String Ϋ,out string Ελ,Char Њ){int Β=Ϋ.IndexOf(Њ);Ελ="";if(Β<0) return false;Ελ=Ϋ.Substring(0,Β);Ϋ=Ϋ.Remove(0,Β+1);return true;}bool ΗΥ(ref String ρ,out String ƛ,out String Ȱ)
{ƛ="";Ȱ="";if(!Ŀ) return false;String ǝ="";if(!Ũ(ref ρ,out ǝ,BROADCAST_SEP)||!Δϋ(ǝ)) return false;if(!Ũ(ref ρ,out ƛ,BROADCAST_SEP)) return false;if(!Ũ(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=Δμ()&&(ǝ!="*"&&Η!=Ζ.р))) return false;if(!Ũ(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=ΒЍ)) return false;
if(!Ũ(ref ρ,out Ȱ,BROADCAST_SEP)) return false;return true;}void ΗΙ(){bool Ɓ=true;if(Εν){Εν=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String ƌ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ƌ)
Δχ(ƌ);String[]Ι=Me.CustomData.Split('\n');Ŀ=bool.Parse(Βν(ΒΟ(Ι,"Enable_Broadcast",ref Ɓ)));bool ΔΟ=false;bool Εα=true;if(Ŀ){if(Η!=Ζ.р)
ΓЈ=Βν(ΒΟ(Ι,"Ship_Name",ref Ɓ)).Replace(BROADCAST_SELF_ID,"");ΒЍ=Βν(ΒΟ(Ι,"Broadcast_Channel",ref Ɓ)).ToLower();Εα=false;if(ȏ==null){ȏ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȏ.SetMessageCallback("");}List<IMyRadioAntenna>ĺ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ĺ);bool ΒЂ=false;
for(int i=0;i<ĺ.Count;i++)
if(ĺ[i].EnableBroadcasting&&ĺ[i].Enabled){ΒЂ=true;break;}if(ĺ.Count==0) Μ="No Antenna found";else if(!ΒЂ) Μ="Antenna not ready";ΔΟ=ĺ.Count==0||!ΒЂ;if(ΕΖ&&!ΔΟ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΖ=ΔΟ;if(Εα){if(ȏ!=null) this.IGC.DisableBroadcastListener(ȏ);ȏ=null;}}catch{Ɓ=false;}if(!Ɓ){Μ="Reset custom data";Δχ(ƌ);}}void Δχ(String ƌ){String Ϋ=ƌ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Ϋ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ϋ+"Broadcast_Channel=#default";}String Βν(String ģ){int Β=ģ.IndexOf("//");if(Β!=-1) ģ=ģ.Substring(0,Β);String[]Д=ģ.Split('=');if(Д.Length<=1) return "";return Д[1].Trim();}String ΒΟ(String[]Ι,String Ģ,ref bool Ɩ){foreach(String ģ in Ι)
if(ģ.StartsWith(Ģ)) return ģ;Ɩ=false;return "";}String Ιβ="";Ķ Ιδ=null;DateTime Ιε=DateTime.MinValue;List<Ķ>Ιγ=new List<Ķ>();const float ΙΝ=20f;String ΔЈ(int ΔΞ,String Γ,int ΖΡ,ref int ș){String[]Ι=Γ.Split('\n');if(ΖΡ>=ș+ΔΞ-1) ș++;ș=Math.Min(Ι.Count()-1-ΔΞ,ș);if(ΖΡ<ș+1) ș--;
ș=Math.Max(0,ș);String έ="";for(int i=0;i<ΔΞ;i++){int ΕЈ=i+ș;if(ΕЈ>=Ι.Count()) break;έ+=Ι[ΕЈ]+"\n";}return έ;}String ΒΣ(int ƚ){if(ƚ>=60*60) return Math.Round(ƚ/(60f*60f),1)+" h";if(ƚ>=60) return Math.Round(ƚ/60f,1)+" min";return ""+ƚ+" s";}string ΒΡ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǻ=new List<IMyCameraBlock>();List<long>Ŕ=new List<long>();bool Ŗ=false;int ΓΒ=0;int ΒА=0;DateTime ΕΠ=DateTime.Now;String ΒЌ="";void Ηα(){ǻ.Clear();Ŕ.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǻ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǻ.Count;i++){ǻ[i].EnableRaycast=true;if(!Ŕ.Contains(ǻ[i].CubeGrid.EntityId)) Ŕ.Add(ǻ[i].CubeGrid.EntityId);}if(!Ŕ.Contains(Me.CubeGrid.EntityId)) Ŕ.Add(Me.CubeGrid.EntityId);}bool Δϊ(Α s){return s==Α.Ȥ||s==Α.Ю||s==Α.І||
s==Α.ȣ||s==Α.Ĳ||s==Α.ũ||s==Α.ƴ||
(s==Α.Ǡ&&ј!=null);}void ư(){Ŗ=false;ΓΒ=0;ΒЌ="";}void Ηψ(){if(!obstacleBrake||ǻ.Count==0||!Δϊ(Φ)){ư();return;}Vector3 Ƌ=ψ+Ć;Vector3 Ǭ=Ƌ-ΰ;float ǫ=Ǭ.Length();if(ǫ<1f){ư();return;}Vector3 κ=Ǭ/ǫ;float ΕЌ=Ț*obstacleLookAhead+θ;Vector3 Θο=Ƌ+κ*(θ*0.25f);
for(int n=0;n<ǻ.Count;n++){ΒА=(ΒА+1)%ǻ.Count;IMyCameraBlock ƀ=ǻ[ΒА];if(!ƀ.IsFunctional||!ƀ.IsWorking) continue;Vector3 ΕΣ=ƀ.GetPosition();Vector3 ΑΦ=Θο;if(Vector3.Distance(ΕΣ,ΑΦ)>ΕЌ) ΑΦ=ΕΣ+κ*ΕЌ;if(!ƀ.CanScan(ΑΦ)) continue;MyDetectedEntityInfo Ǝ=ƀ.Raycast(ΑΦ);bool Θη=!Ǝ.IsEmpty()&&!Ŕ.Contains(Ǝ.EntityId)&&!ΗΟ(Ǝ.EntityId)&&
Ǝ.Type!=MyDetectedEntityType.FloatingObject;if(Θη){if(!Ŗ) ΕΠ=DateTime.Now;Ŗ=true;ΓΒ=0;ΒЌ=Ǝ.Type.ToString();}else if(Ŗ&&++ΓΒ>=3) ư();break;}if(Ŗ&&(DateTime.Now-ΕΠ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΒЌ;}const String ΔБ="[PAMTFC]";const float Ην=5f;const float ΒΞ=2f;class Ȧ{public long μ;public Vector3 η,ţ;public Vector3 ƍ;public float Εΰ;public String ŋ="";public int Γί=-1;public bool ΓЂ;public long ά=0;public int ƅ=0;public DateTime ΔΔ;}IMyBroadcastListener ΑЄ=null;
long ї=0;List<Ȧ>м=new List<Ȧ>();Vector3 Ć=new Vector3();float ǰ=-1;Vector3 ΔΘ=new Vector3();bool ΑЃ=false;DateTime ΔΙ=DateTime.Now;const float ΔА=15f;const float Ημ=10f;int Ζμ=0;String Ч="";int Ѐ=-1;bool ΓΕ=false;Vector3 ΕΧ=new Vector3();String ƻ(){if(ή==Λ.з||Η==Ζ.Ϊ) return "";
return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Н+"|"+в+"|"+(int) ġ+"|"+Math.Round(ǯ,1)+"|"+Math.Round(Ǯ,1);}void Ηϋ(){if(!trafficEnabled) return;if(ΑЄ==null) ΑЄ=IGC.RegisterBroadcastListener(ΔБ);ї=Me.CubeGrid.EntityId;int ſ=30;while(ΑЄ.HasPendingMessage&&ſ-->0){String Ϋ=ΑЄ.AcceptMessage().Data as String;if(Ϋ!=null) ΗΨ(Ϋ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΔΔ).TotalSeconds>Ην) м.RemoveAt(i);if(ΑΚ==null) ΑΚ=IGC.RegisterBroadcastListener(ΗΠ);ſ=10;while(ΑΚ.HasPendingMessage&&ſ-->0){String Ϋ=ΑΚ.AcceptMessage().Data as String;if(Ϋ!=null) try{ΗΦ(Ϋ);}catch{}}if(++Ζμ<3) return;if(Φ==Α.Я&&З(MyShipConnectorStatus.Connected)==null) return;
Ζμ=0;IGC.SendBroadcastMessage(ΔБ,ї+";"+τ(ΰ)+";"+
τ(Υ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ƻ()+";"+
(ή==Λ.Є?Ѐ:-1)+";"+Ч+";"+ΖЀ()+","+Δζ()+";"+τ(ΔΘ)+
";"+(ł?1:0)+";"+(Ğ&&ή==Λ.Є?1:0)+","+Αϊ+","+Αω);}void ΗΨ(String Ϋ){try{String[]f=Ϋ.Split(';');if(f.Length<7) return;long μ=long.Parse(f[0]);if(μ==ї) return;Ȧ Ж=null;for(int i=0;i<м.Count;i++) if(м[i].μ==μ) Ж=м[i];if(Ж==null){Ж=new Ȧ();Ж.μ=μ;м.Add(Ж);}
Ж.η=λ(f[1]);Ж.ţ=λ(f[2]);Ж.Εΰ=float.Parse(f[3]);Ж.ŋ=f[4];Ж.Γί=int.Parse(f[5]);Ж.ΔΔ=DateTime.Now;if(Ж.ŋ!=""&&Ж.ŋ==ƻ()) ΔЂ(f[6]);Ж.ά=0;Ж.ƅ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ж.ά=long.Parse(d[0]);Ж.ƅ=int.Parse(d[1]);}Ж.ƍ=f.Length>=9?λ(f[8]):new Vector3();
Ж.ΓЂ=f.Length>=10&&f[9]=="1";}catch{}}bool ΗΟ(long μ){for(int i=0;i<м.Count;i++) if(м[i].μ==μ) return true;return false;}Vector3 Δπ(int Β,int Θκ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Χ Ψ=Σ[Β];Vector3 ΘЏ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θκ;
Vector3 ώ=Ψ.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.ν):-Ψ.α;Vector3 Ь=Vector3.Cross(ΘЏ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();float ό=laneOffset*θ;float ΘΘ=Math.Min(Vector3.Distance(Ψ.Δ,Σ[0].Δ),
Vector3.Distance(Ψ.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΘ/(ό*4));}Vector3 Ζλ=new Vector3();bool ΗΞ(Α s){return s==Α.Ǡ||s==Α.ĉ||s==Α.č||s==Α.ų||
s==Α.ǐ||s==Α.Ȍ||s==Α.ȟ;}void Ηό(){Ć=new Vector3();ǰ=-1;ΔΘ=new Vector3();bool Επ=Δϊ(Φ);if(!trafficEnabled||(!Επ&&!ΗΞ(Φ))){ΑЃ=false;return;}Ζλ=Υ.GetShipVelocities().LinearVelocity;ΖЁ(Υ.GetNaturalGravity(),!Επ);}void ΖЁ(Vector3 ν,bool ΖΛ=false){Ć=new Vector3();ǰ=-1;
Vector3 Ǭ=ψ-ΰ;float ǫ=Ǭ.Length();bool Γά=ǫ>=1f;Vector3 κ=Γά?Ǭ/ǫ:new Vector3();ΔΘ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ƈ;Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-ƒ:Vector3.Normalize(Ь);Ȧ Αγ=null;for(int i=0;i<м.Count;i++)
{Ȧ δ=м[i];if(δ.ƅ==3&&δ.ţ.LengthSquared()<0.25f) continue;Vector3 η=δ.η+δ.ţ*(float)(DateTime.Now-δ.ΔΔ).TotalSeconds;Vector3 Ř=η-ΰ;float Ǔ=(θ+δ.Εΰ)*0.5f;float ź=Γά?Vector3.Dot(Ř,κ):0;if(ź>0&&(Ř-κ*ź).Length()<=Ǔ*1.2f){float Ας=Vector3.Dot(δ.ţ,κ);bool Εψ=δ.ƍ.LengthSquared()>0.25f;
bool Γέ=Ας<-1f||(Εψ&&Vector3.Dot(δ.ƍ,κ)<-0.5f);if(ΖΛ){if(Γέ&&ї<δ.μ) continue;}else{if(δ.ΓЂ&&!ł){Ć=Ь*Ǔ*1.3f;continue;}if(Γέ){if(ź<(Ț-Ας)*6f+Ǔ*4f) Ć=Ь*Ǔ*1.3f;continue;}if(Εψ&&ї<δ.μ){Vector3 ΕΜ=-Ř;float ΖΘ=Vector3.Dot(ΕΜ,δ.ƍ);if(ΖΘ>0&&(ΕΜ-δ.ƍ*ΖΘ).Length()<=Ǔ*1.2f)
{Ć=Ь*Ǔ*1.3f;continue;}}}float Ες=ź-Ǔ-followGap*θ;float Γκ=Math.Max(0,Ας)+Math.Max(0,Ες)/ΒΞ;if(Γέ) Γκ=Math.Max(0,Ες+Math.Min(0,Ας)*ΒΞ*2)/ΒΞ;if(ǰ<0||Γκ<ǰ){ǰ=Γκ;Αγ=δ;}continue;}if(δ.ţ.LengthSquared()<1f) continue;if(Γά&&(Ř-κ*ź).Length()<=Ǔ*1.2f) continue;
Vector3 ΓЇ=δ.ţ-Ζλ;float ΖΟ=ΓЇ.LengthSquared();if(ΖΟ<0.25f) continue;float t=-Vector3.Dot(Ř,ΓЇ)/ΖΟ;if(t<=0||t>5f||(Ř+ΓЇ*t).Length()>=Ǔ*1.1f) continue;if(ї>δ.μ||t<2f){ǰ=0;Αγ=δ;}}bool ΘΟ=ǰ>=0&&ǰ<0.5f;if(!ΘΟ||ΖΛ||(Αγ!=null&&Αγ.ΓЂ&&ł)){ΑЃ=false;return;}
if(!ΑЃ){ΑЃ=true;ΔΙ=DateTime.Now;}double Εω=(DateTime.Now-ΔΙ).TotalSeconds;if(Εω>ΔА+Ημ) ΔΙ=DateTime.Now;else if(Εω>ΔА){ǰ=-1;Ć=Ь*θ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>Ȃ=new List<int>();String Εϋ="";void ΒΝ(){String Ģ=Н+"|"+в+"|"+(int) ġ;
if(Ģ==Εϋ) return;Εϋ=Ģ;Ζχ(Н,в,ġ,Ȃ);}void Ζχ(int w,int h,Ǐ ΓЎ,List<int>ΒБ){ΒБ.Clear();if(ΓЎ==Ǐ.ǉ){for(int k=0;k<w*h;k++){int ř=k/w;ΒБ.Add(ř*w+(ř%2==0?k-ř*w:w-1-(k-ř*w)));}return;}int Εχ=(int) Math.Ceiling(w/2f),Εφ=(int) Math.Ceiling(h/2f);int ΘΛ=w/2,ΘΚ=h/2;
int x=0,y=0,ΓΝ=0,ΓΞ=-1,Αι=Math.Max(w,h);for(int i=0;i<Αι*Αι;i++){if(-Εχ<x&&x<=ΘΛ&&-Εφ<y&&y<=ΘΚ) ΒБ.Add((y-1+Εφ)*w+(x-1+Εχ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΝ;ΓΝ=-ΓΞ;ΓΞ=t;}x+=ΓΝ;y+=ΓΞ;}}bool ΗΕ(int a,int b,int w,float Ζξ,float Ζη,float Ερ,float Εκ)
{return a==b||(Math.Abs(a%w-b%w)*Ζξ<Ερ+0.5f&&Math.Abs(a/w-b/w)*Ζη<Εκ+0.5f);}bool ΒΜ(){int n=Math.Max(0,Н*в);if(Ч.Length==n) return false;Ч=new String('0',n);Ѐ=-1;return true;}void ΔЂ(String δ){ΒΜ();if(δ.Length!=Ч.Length) return;char[]h=Ч.ToCharArray();
for(int i=0;i<h.Length;i++) if(δ[i]=='1') h[i]='1';Ч=new String(h);}bool ƹ(int Ǽ){return Ǽ>=0&&Ǽ<Ч.Length&&Ч[Ǽ]=='1';}void Βτ(int Ǽ,bool ф){if(Ǽ<0||Ǽ>=Ч.Length) return;char[]h=Ч.ToCharArray();h[Ǽ]=ф?'1':'0';Ч=new String(h);}bool Βά(int Ǽ,bool ΘΦ){if(!trafficEnabled) return false;
String Ģ=ƻ();for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ŋ!=Ģ||t.Γί<0||(ΘΦ&&t.μ>ї)) continue;if(ΗΕ(Ǽ,t.Γί,Н,ǯ,Ǯ,Αϊ,Αω)) return true;}return false;}int ΖϏ(int Β){int n=Н*в;if(n<1) return Β;ΒΝ();if(ΒΜ()) for(int k=0;k<Β&&k<n;k++) Βτ(Ȃ[k],true);if(Ѐ>=0&&(Β>=n||Ȃ[Β]!=Ѐ)){Βτ(Ѐ,true);Ѐ=-1;}
if(Δό()){if(ǿ>=0&&!ƹ(ǿ)){Ğ=false;Ѐ=ǿ;return Ȃ.IndexOf(ǿ);}if(ǿ==-2){Ğ=false;Ѐ=-1;return n;}if(ǿ==-1&&Ѐ>=0&&!ƹ(Ѐ)){Ğ=false;return Β;}Ѐ=-1;if(!Ğ){Ğ=true;ΕΩ=DateTime.Now;}if((DateTime.Now-ΕΩ).TotalSeconds<5) return-1;}Ğ=false;if(Ѐ>=0&&!ƹ(Ѐ)&&!Βά(Ѐ,true)) return Β;
Ѐ=-1;for(int k=0;k<n;k++){int c=Ȃ[k];if(ƹ(c)||Βά(c,false)) continue;Ѐ=c;return k;}return n;}const String ΗΠ="[PAMJOB]";IMyBroadcastListener ΑΚ=null;DateTime ΕΨ=DateTime.MinValue;DateTime ΕΩ=DateTime.Now;int ǿ=-1;bool Ğ=false;bool Δό(){return trafficEnabled&&(DateTime.Now-ΕΨ).TotalSeconds<3;}
void ΗΦ(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƻ()) return;ΕΨ=DateTime.Now;ΔЂ(f[2]);ǿ=-1;String ΕА=ї+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΕА)) ǿ=int.Parse(e[i].Substring(ΕА.Length));}ķ Ηδ(int Β)
{ΒΝ();if(Β<0||Β>=Ȃ.Count) return ķ.ΔΩ;Ш=Ȃ[Β]%Н;Щ=Ȃ[Β]/Н;return ķ.Δκ;}class я{public long μ;public Vector3 η,Б,ώ;}List<я>Ѝ=new List<я>();bool ȫ=false;Vector3 ΑΣ,ΑΡ,ΑΤ,ΑΥ;long х=0;long Ȅ=0;Χ ј=null;bool Ɔ=false;Χ ΒΙ(Vector3 η,Vector3 Б,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(η,Б,ώ);
Χ w=new Χ(Θ);w.β=Vector3D.TransformNormal(ΑΣ,m);w.α=Vector3D.TransformNormal(ΑΡ,m);w.χ=Vector3D.TransformNormal(ΑΤ,m);w.Δ=η+(Vector3) Vector3D.TransformNormal(ΑΥ,m);w.γ=true;return w;}int Δζ(){Α s=Φ;if(s==Α.Я) return З(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.č||s==Α.ǹ||s==Α.Č||s==Α.ΑΒ||
s==Α.Ƴ||s==Α.ǐ) return 3;if(s==Α.Ǡ||s==Α.ĉ||s==Α.ų) return 2;if(s==Α.І) return 1;return 0;}long ΖЀ(){if(Δζ()==0) return 0;if(ȥ(1)&&ŝ!=0) return ŝ;return ј!=null?Ȅ:х;}void ΔϏ(){if(Η==Ζ.Ϊ||!Θ.γ) return;IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null||Й.OtherConnector==null) return;
IMyShipConnector δ=Й.OtherConnector;Vector3I ΘΠ=new Vector3I((int) Θ.Ц.X,(int) Θ.Ц.Y,(int) Θ.Ц.Z);if(Й.Position==ΘΠ){MatrixD ň=MatrixD.Transpose(δ.WorldMatrix);ΑΣ=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,ň);ΑΡ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,ň);ΑΤ=Vector3D.TransformNormal(Υ.WorldMatrix.Left,ň);
ΑΥ=Vector3D.TransformNormal(Й.GetPosition()-δ.GetPosition(),ň);ȫ=true;}if(Vector3.Distance(Й.GetPosition(),Θ.Δ)<5) х=δ.EntityId;List<IMyShipConnector>Ĝ=new List<IMyShipConnector>();ω.GetBlocksOfType(Ĝ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ĝ.Count>0){Ѝ.Clear();for(int i=0;i<Ĝ.Count;i++){я p=new я();p.μ=Ĝ[i].EntityId;p.η=Ĝ[i].GetPosition();p.Б=Ĝ[i].WorldMatrix.Forward;p.ώ=Ĝ[i].WorldMatrix.Up;Ѝ.Add(p);}}ј=null;Ȅ=0;if(δ.EntityId==х||!ȫ) return;for(int i=0;i<Ѝ.Count;i++)
if(Ѝ[i].μ==δ.EntityId){Ȅ=Ѝ[i].μ;ј=ΒΙ(Ѝ[i].η,Ѝ[i].Б,Ѝ[i].ώ);}}bool Βί(long μ,int ΖΓ){if(μ==0) return false;for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ά==μ&&(t.ƅ>ΖΓ||(t.ƅ==ΖΓ&&t.μ<ї))) return true;}return false;}bool ΒΔ(int ΓϏ){if(Η==Ζ.Ϊ||!trafficEnabled){ј=null;Ȅ=0;return true;}
long ζ=ј!=null?Ȅ:х;if(!Βί(ζ,ΓϏ)) return true;if(!Βί(х,ΓϏ)){ј=null;Ȅ=0;return true;}if(!ȫ) return false;я ė=null;float ΕΟ=0;for(int i=0;i<Ѝ.Count;i++){я p=Ѝ[i];if(p.μ==х||Βί(p.μ,ΓϏ)) continue;float d=Vector3.Distance(p.η,Θ.Δ);if(ė==null||d<ΕΟ){ė=p;ΕΟ=d;}
}if(ė==null) return false;Ȅ=ė.μ;ј=ΒΙ(ė.η,ė.Б,ė.ώ);return true;}String ΖЉ(){String s=(ȫ?"1":"0")+":"+τ(ΑΣ)+":"+τ(ΑΡ)+":"+
τ(ΑΤ)+":"+τ(ΑΥ)+":"+х;for(int i=0;i<Ѝ.Count;i++)
s+=":"+Ѝ[i].μ+","+τ(Ѝ[i].η)+","+τ(Ѝ[i].Б)+","+τ(Ѝ[i].ώ);return s;}void ΖЈ(String s){Ѝ.Clear();ȫ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;ȫ=f[0]=="1";ΑΣ=λ(f[1]);ΑΡ=λ(f[2]);ΑΤ=λ(f[3]);ΑΥ=λ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;я p=new я();if(!long.TryParse(e[0],out p.μ)) continue;p.η=λ(e[1]);p.Б=λ(e[2]);p.ώ=λ(e[3]);Ѝ.Add(p);}}const String ΒΖ="[PAMDCK]";const float ΙΜ=120f;class Δη{public long Ж;public int Ɛ;public long Γΰ;public bool Ιέ;public DateTime Ισ,Ιω;
}IMyBroadcastListener Ιπ=null;List<Δη>Ιρ=new List<Δη>();List<IMyShipConnector>ΙЂ=new List<IMyShipConnector>();List<IMySensorBlock>ΙЊ=new List<IMySensorBlock>();List<IMyCameraBlock>Ιψ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΙЉ=new List<MyDetectedEntityInfo>();long Ιΰ=0;
int Ιί=0;long Ιή=0;DateTime Ια=DateTime.Now;String Ιο="";int Ις=0;IMyCubeGrid ΙЃ=null;IMyBroadcastListener ΑϏ=null;int ȁ=0;DateTime Εδ=DateTime.Now;DateTime ΖΨ=DateTime.MinValue;long ΖΩ=0;int ΖΪ=0;long ŝ=0;bool ƨ=false;int ΓΜ=0;List<Vector3>ƪ=new List<Vector3>();List<float>ΔΧ=new List<float>();
bool ł=false;bool ΑΠ=false;Vector3 Εγ=new Vector3();bool Ŷ(){return ȁ!=0&&(DateTime.Now-ΖΨ).TotalSeconds<3;}bool ȥ(int Ɛ){return Ŷ()&&ΖΩ==ї&&ΖΪ==Ɛ;}bool ΔЌ(){return Ŷ()||(DateTime.Now-Εδ).TotalSeconds<3;}void Βς(int Ɛ){if(ȁ==Ɛ) return;ȁ=Ɛ;Εδ=DateTime.Now;ΓΜ=3;
}void Βρ(){if(ȁ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΖ,"D;"+ї);ȁ=0;ƨ=false;}void Ησ(){if(!trafficEnabled||Η==Ζ.Ϊ) return;if(ΑϏ==null) ΑϏ=IGC.RegisterBroadcastListener(ΒΖ);int ſ=20;while(ΑϏ.HasPendingMessage&&ſ-->0){String Ϋ=ΑϏ.AcceptMessage().Data as String;if(Ϋ==null||!Ϋ.StartsWith("S;")) continue;
try{ΗΧ(Ϋ);}catch{}}if(Φ!=Α.І){ł=false;ΑΠ=false;}if(ȁ==1&&З(MyShipConnectorStatus.Connected)!=null) Βρ();if(ȁ==2&&!ƨ&&Φ!=Α.ǐ&&
Vector3.Distance(ΰ,ж().Δ)>θ*3) Βρ();if(ȁ!=0&&++ΓΜ>=3){ΓΜ=0;IGC.SendBroadcastMessage(ΒΖ,"R;"+ї+";"+ȁ+";"+х+";"+(ȫ?1:0));}}void ΗΧ(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<7) return;bool Εο=false;String[]ΕΒ=f[5].Split(',');for(int i=0;i<ΕΒ.Length;i++) if(ΕΒ[i]==ї.ToString()) Εο=true;
if(!Εο) return;ΖΨ=DateTime.Now;ΖΩ=long.Parse(f[2]);ΖΪ=int.Parse(f[3]);ŝ=long.Parse(f[4]);ƪ.Clear();ΔΧ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ƪ.Add(λ(e[0]));ΔΧ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εζ=f[6].Split(':');List<я>С=new List<я>();for(int i=0;i<Εζ.Length;i++){String[]e=Εζ[i].Split(',');if(e.Length<4) continue;я p=new я();p.μ=long.Parse(e[0]);p.η=λ(e[1]);p.Б=λ(e[2]);p.ώ=λ(e[3]);С.Add(p);}Ѝ=С;}bool Ζύ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;
Βς(1);if(!ΔЌ()) return true;if(!ȥ(1)){Μ="Waiting for dock clearance";return false;}ΑЍ();return true;}bool ΑЍ(){Ɔ=false;я ĥ=null;if(ŝ!=0&&ŝ!=х&&ȫ)
for(int i=0;i<Ѝ.Count;i++) if(Ѝ[i].μ==ŝ) ĥ=Ѝ[i];long ΑЉ=ĥ==null?0:ĥ.μ;if(ΑЉ==Ȅ&&(ΑЉ==0)==(ј==null)) return false;Ȅ=ΑЉ;ј=ĥ==null?null:ΒΙ(ĥ.η,ĥ.Б,ĥ.ώ);return true;}bool ĳ(Vector3 p){for(int i=0;i<ƪ.Count;i++) if(Vector3.Distance(p,ƪ[i])<ΔΧ[i]+θ*0.75f) return true;
return false;}bool ΗΔ(Vector3 Ƃ,Vector3 Œ){ł=false;if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return false;if(ȁ!=1&&Vector3.Distance(ΰ,Θ.Δ)>dockRequestDistance) return false;Βς(1);if(!Ŷ()||ȥ(1)||ƪ.Count==0) return false;if(!ĳ(Ƃ)&&!ĳ(Œ)&&!ĳ(ΰ)) return false;ł=true;return true;
}bool Δΰ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;Βς(2);if(!ΔЌ()) return true;if(!ȥ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΙΣ=600f;class Δώ{public long μ;public int Ǽ=-1;public bool ΑЉ;public float Ερ,Εκ;public DateTime Ιό;}class Δύ{public String Ģ;
public int w,h;public float Ζξ,Ζη;public String ф;public List<int>Ιϋ=new List<int>();public List<Δώ>Ιώ=new List<Δώ>();}List<Δύ>Ιχ=new List<Δύ>();IMyBroadcastListener Ιφ=null;int Ιυ=0;void ΖЅ(string ё){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}