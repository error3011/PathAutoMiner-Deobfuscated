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
public enum Ζ{ΑΗ,Ÿ,ǐ,р,Ϋ}public enum Α{ж,ȋ,Ĺ,Ǹ,Ȧ,Ѕ,Ю,
ȥ,ķ,Ǣ,Đ,Č,ȡ,
Ź,ǻ,Ǔ,ď,ΑΙ,ƻ,
ΒЊ,ΑΘ,ů,Ƽ,Ȩ}public enum Κ{з,ĺ,Ϗ,ΒΦ,ƶ,ǅ,ǆ,ĳ,Ĵ}public enum п{None,ď,ж,Δλ}public enum Ϊ{Main,ΔЎ,ŷ,Ŷ,ΒІ,ƴ,
ΒΖ,Δό,Δύ,Δϊ,Ηυ,Ιμ,
Ικ,Ιλ,ΒЁ,ΕΑ,ΑΒ}public enum а{ǌ,ŭ,Ηχ,ΔϏ,Ϋ}public enum ǒ{ΑΔ,ΖЈ}public enum д{ǋ,Βά,ΕΓ,ű}public enum Ѐ{Βο,Δθ,ΒΠ,ΒΧ,ƹ,Βθ,ǀ}public enum я{ΒΣ,ΒΛ,ΒΜ}public enum И{Ų,ΗΦ,č}public enum ι{Βρ,Βυ,Βφ,ΔЋ,ΔЊ,
Βπ,ǉ,ǈ,Βσ,
Βτ,Βς}public enum ΰ{ǳ,ȍ,ǲ,Ƴ,ΒΙ,ΒΚ}public enum Ľ{Δπ,Δί,Ιΰ}public enum с{ΑΓ,Δξ,Ηα,Βξ}public enum Ǵ{None,Βϊ,Ηη,Ηΰ}public enum σ{ž,Ʋ,ǃ,ű,Βμ,Ǆ,Βν,Ηύ,Βχ}public enum Ιδ{ΑΗ,Ιζ,Ιε,Ιθ,Ια,Ιί,Ιη}class Ψ{public bool δ=false;public Vector3 Δ=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 υ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float ΚΘ=0;public float[]ǟ=null;public Ψ(){}public Ψ(Ψ α){δ=α.δ;Δ=α.Δ;β=α.β;γ=α.γ;υ=α.υ;ν=α.ν;
Ч=α.Ч;ǟ=α.ǟ;}public Ψ(Vector3 Δ,Vector3 γ,Vector3 β,Vector3 υ,Vector3 ν){this.Δ=Δ;this.β=β;this.γ=γ;this.υ=υ;this.Џ=0;this.ν=ν;}public void ΖЅ(List<IMyThrust>ΑΜ,List<string>ȇ){ǟ=new float[ȇ.Count];for(int i=0;i<ǟ.Length;i++)
ǟ[i]=-1;for(int i=0;i<ΑΜ.Count;i++){string ю=Δψ(ΑΜ[i]);int Β=ȇ.IndexOf(ю);if(Β!=-1)
ǟ[Β]=А(ΑΜ[i].MaxEffectiveThrust,ΑΜ[i].MaxThrust);}}}class Βω{public Ψ Њ=null;public List<Vector3>ť=new List<Vector3>();public float Ɗ=0;public float ΔΣ=0;public float ł=0;public float Γέ=0;public Vector3 Αΰ=new Vector3();}class ľ{public ι Ќ=ι.Βρ;public float ǖ=0;public float ȯ=0;
public string Ǘ="";public string ǚ="";DateTime ΕЏ;public bool ΓЇ=false;private bool ΓΠ=false;public bool ΒΞ(bool Ȇ){if(Ȇ){ȯ=0;ΓΠ=false;return false;}ΓΠ=true;return ȯ>ǖ;}public void Ηθ(){ΒΞ(true);ΓЇ=false;}public void ΗЉ(){if(ΓΠ)
if((DateTime.Now-ΕЏ).TotalSeconds>1){ȯ++;ΕЏ=DateTime.Now;}}public bool Βε(){switch(Ќ){case ι.Βυ:return true;case ι.Βφ:return true;}return false;}}class ѐ{public String φ="";public String ς="";public int ǔ=0;public И œ=И.č;public ѐ(String φ,String ς,int ǔ,И œ){this.φ=φ;this.ς=ς;
this.ǔ=ǔ;this.œ=œ;}}class ΗΓ{public ΗΓ(Vector3 ε,float ό){this.ε=ε;this.ό=ό;}public Vector3 ε;public float ό;}class ļ{public DateTime Θθ;public DateTime Εή;public String λ="";public String φ="";public String ΚΚ="";public String Μ="";public String ΚΑ="";public String ΙЋ="";public String ΓІ="";
public Vector3 Δ=new Vector3();public List<ѐ>Ъ=new List<ѐ>();public Ζ Η=Ζ.ΑΗ;public Ǵ Ȫ;public float Ŗ;public float Ň;public int љ;public int ΙЀ=0;public int ΚΗ=0;public bool Ιβ(){return(DateTime.Now-Θθ).TotalSeconds>10;}public bool ΗΩ(){if(Ȫ!=Ǵ.Βϊ) return false;return(DateTime.Now-Εή).TotalSeconds>=4;
}public ļ(String λ){this.λ=λ;}public Ǵ Ιι(String ě,bool ΘЉ,bool є,bool ΘΖ){if(ě==""&&!є) return Ǵ.None;if(Ȫ==Ǵ.Βϊ&&ΗΩ())
Ȫ=Ǵ.Ηΰ;if(ΘЉ){ΓІ=ě;Ȫ=Ǵ.Βϊ;Εή=DateTime.Now;return Ȫ;}else if(є){ΓІ="";Ȫ=Ǵ.None;}else if(ΓІ==ě){if(ΘΖ) Ȫ=Ǵ.Ηη;return Ȫ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 ΑЁ=new Vector3();Vector3 ί=new Vector3();Vector3 Αγ=new Vector3();Vector3 Ɨ=new Vector3();Vector3 ƍ=new Vector3();
DateTime ΕЎ=new DateTime();bool Ɛ=true;int ΔΚ=0;bool ă=false;bool Αμ=false;bool ƛ=true;bool ΖϏ=false;bool ő=false;bool Αι=false;float Ȝ=0;float Ĭ=0;int ć=0;int ƫ=0;int ΑЄ=0;int ΓΘ=0;float ŕ=0;float Ń=0;double Γό=0;float Ƃ=0;List<int>Ŏ=new List<int>();List<int>Š=new List<int>();
Ζ Η=Ζ.ΑΗ;int ύ=0;float Ǡ=0;float Ǳ=0;float ζ=0;float ī=0,Ƣ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΜ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȉ=new List<IMyThrust>();List<IMyGyro>Αδ=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>Ć=new List<IMyReactor>();List<IMyConveyorSorter>ΑЇ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>ſ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ņ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ė=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>Ĉ=new List<IMyTextSurface>();List<IMyTextPanel>Ɖ=new List<IMyTextPanel>();IMyTerminalBlock ќ=null;Ψ Θ=new Ψ();Ψ Γκ=new Ψ();
Ψ Γν=new Ψ();bool ȉ=false;bool Ũ=false;bool ũ=false;bool Αλ=false;List<String>ȇ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>ư=new List<Ψ>();int ΓΦ=0;double ΖЀ=0;Ψ Ε=new Ψ();Vector3 њ;Vector3 ș;Κ ή=Κ.з;ǒ Ĥ=ǒ.ΑΔ;int Д=0;int б=0;double Ɣ=0;bool ΑΦ=false;ľ ϊ=new ľ();
ľ ϋ=new ľ();д л=д.ǋ;bool Ċ=true;ǒ ȝ=ǒ.ΑΔ;int й=3;int и=3;int Ц=30;я у=я.ΒΣ;bool Ƞ=true;bool Ő=true;bool ň=true;bool ĭ=false;Ѐ Н=Ѐ.Βο;bool ȟ=true;float Ǭ=90;float ч=20;float Ы=5;float ш=20;float Ǧ=0.70f;float ū=1.50f;float Ū=2.50f;float Ĳ=0f;float ġ=0f;Α Υ;
Κ ц;Α ƥ;Βω η=null;int Ш=0;int Щ=0;int К=0;int ȗ=0;int ĩ=30;int У=0;bool Ђ=true;Vector3 щ;double Л=0;bool ΑЃ=false;int ΔΧ=0;int Ѓ=0;int ΒБ=0;int ǭ=0;int ΖΡ=0;Vector3 Ɲ=new Vector3();float к=0;float Э=0;int ı=0;Ψ ΑЈ=null;float Γπ=0;float ΑΞ=0;float С=0;float ħ=0;
bool Γλ=false;bool Į=false;bool Ƭ=false;bool Ȕ=false;bool Ȑ=false;DateTime ΘΥ=new DateTime();Ψ Ĝ=null;Ψ Εγ=null;float Ǽ=0;п ǧ;float ȕ=0;float į=0;String Γχ="";float Αο=0;float Ň=0;float Ŗ=0;List<ѐ>Ǚ=new List<ѐ>();float О=0;bool ƣ=false;bool Ζτ=false;bool Ζΰ=false;
bool ΑЌ=false;bool Ζα=false;float Γϊ=0;float ΓЅ=0;Vector3 Εφ=new Vector3();Vector3 χ=new Vector3();Vector3 ΑЍ=new Vector3(1,1,1);float ΔΙ=1;Vector3 ƨ=new Vector3();bool Ακ=false;bool Γγ=false;bool Γτ=false;bool ΕΞ=false;float ΑΤ=0;float ΑΛ=2;Vector3 Ζρ;Vector3 ΔΘ;Vector3 ΔΗ;
float[,]ǡ=new float[3,2];Dictionary<String,float[,]>џ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζύ=new Dictionary<String,float>();int ΓΓ=0;Dictionary<String,float[]>Ț=new Dictionary<string,float[]>();float ΓЈ;bool ΑΧ=false;int ΑΝ=0;int Ā=0;int Αη=0;int Ģ=0;
String Μ="";int[]ΖΙ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΚ=false;int Αω=0;int љ=0;Ϊ Ѝ=Ϊ.Main;int ΙΔ=0,ΙΕ=0,ΙΖ=0,ΙΗ=0;int ΘЅ=0,ΘІ=0;bool Ņ=false;bool ΕΟ=false;String ΓЏ="";String ΓΗ="";IMyBroadcastListener ȑ=null;bool Ευ=true;List<ļ>Ιχ=null;ļ ΙЎ=null;
int Ιψ=0,Ιω=0;int Ιϊ=0,Ιϋ=0;int Ιό=0,Ιύ=0,Ιώ=0;String Θη="";Exception Ȗ=null;bool ΑЂ=false;Κ Ţ=Κ.з;bool Εζ=false;bool ΕЍ=true;bool ΓЍ=false;String Ζώ="";bool ƪ=false;bool ΔΓ=true;int Αθ=0;int[]ΙБ=null;Vector3 ΓΙ=new Vector3();bool Εό=false;bool Ǒ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;
Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βζ(Me,controllerTag,true))
Η=Ζ.р;π(Ϊ.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЌ=Ηι();if(Η==Ζ.ΑΗ){List<IMyShipDrill>Ερ=new List<IMyShipDrill>();List<IMyShipGrinder>Εύ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Ερ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Εύ,q=>q.CubeGrid==Me.CubeGrid);if(Ερ.Count>0)
{Η=Ζ.Ÿ;Μ="Miner mode enabled!";}else if(Εύ.Count>0){Η=Ζ.ǐ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;ǹ(а.Ϋ);}}if(ΓЌ==с.ΑΓ) ƛ=false;if(ΓЌ==с.Δξ) Μ="Data restore failed!";if(ΓЌ==с.Βξ) Μ="New version, wipe data";}}void Main(string ѓ,UpdateType ΙΟ){try{if(Ȗ!=null){if(ѓ.Trim().ToUpper()=="RETRY")
{Ȗ=null;Ɛ=true;ć=0;if(Η!=Ζ.р) ψ();Μ="Restarted after error";}else{ΗΠ();ΔЌ();return;}}Αμ=(ΙΟ&UpdateType.Update10)!=0;if(Αμ) ΔΚ++;ă=ΔΚ>=10;if(ă) ΔΚ=0;if(Αμ){ΑΝ++;if(ΑΝ>4) ΑΝ=0;ΑЄ=(int) Math.Max(0,10-(DateTime.Now-ΕЎ).TotalSeconds);}if(ѓ!="") Θη=ѓ;if(Η!=Ζ.р)
Ηφ(ѓ);else
ΗΑ(ѓ);ΖΚ=false;try{int Εδ=Runtime.CurrentInstructionCount;float ƞ=А(Εδ,Runtime.MaxInstructionCount);if(ƞ>0.90) Μ="Max. instructions >90%";if(ƞ>ŕ) ŕ=ƞ;if(ΑΧ){Ŏ.Add(Εδ);while(Ŏ.Count>10) Ŏ.RemoveAt(0);Ń=0;for(int i=0;i<Ŏ.Count;i++) Ń+=Ŏ[i];Ń=А(А(Ń,Ŏ.Count),Runtime.MaxInstructionCount);
double Γρ=Runtime.LastRunTimeMs;if(Αι&&Γρ>Γό) Γό=Γρ;Š.Add((int)(Γρ*1000f));while(Š.Count>10) Š.RemoveAt(0);Ƃ=0;for(int i=0;i<Š.Count;i++) Ƃ+=Š[i];Ƃ=А(Ƃ,Š.Count)/1000f;}}catch{ŕ=0;}}catch(Exception e){Ȗ=e;}}void Ηφ(string ѓ){bool ΑЅ=false;String ΕΫ="";if(ύ<=1&&!ΔЂ(ѓ))
ΔЍ(ѓ);int Θς=50;while(ȑ!=null&&ȑ.HasPendingMessage&&Θς-->0){MyIGCMessage ρ=ȑ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String ơ="";if(ύ<=1&&Ηβ(ref Χ,out ơ,out ΕΫ)&&ơ==BROADCAST_SELF_ID){ΔЍ(Χ);ΑЅ=true;break;}}bool ΕЈ=Αι&&Υ==Α.ж&&!ő&&!ΑЅ&&ć==0&&!Αλ;
if(ă&&Υ!=Α.ж) ΑЅ=true;if((Αμ&&!ΕЈ)||(ă&&ΕЈ)){if(ć==0&&(ΑЄ<=0||Ɛ)){ΖϏ=ύ>0;ύ=0;ć=1;Ǻ();Ημ();ΗΧ();ǵ("Scan 1");}else if(ć==1){ć=2;Ǻ();Ην();ǵ("Scan 2");}else if(ć==2){ć=0;Ǻ();Ηξ();Ηο();if(ύ<=1) ΔІ();ǵ("Scan 3");ΕЎ=DateTime.Now;if(ύ<=1&&ƛ)
ΑЁ=ƾ(Φ,Φ.CenterOfMass);ƛ=false;if(Ɛ){Ɛ=false;bool ΙΤ=ή==Κ.Ϗ;ψ();if(ΕЍ&&resumeAfterRestart&&ΙΤ&&ύ<=1) ΓЍ=true;}ΕЍ=false;if(ΖϏ&&ύ==0) Μ="Setup complete";}else{if(ή==Κ.Ϗ&&Η!=Ζ.Ϋ){Ǻ();ΖЂ();ǵ("Inv balance");}Ǻ();switch(ƫ){case 0:ΗЁ();break;case 1:ΗЅ();break;case 2:ΗЀ();break;
case 3:ΗЌ();break;case 4:ΗЄ();break;case 5:ΖЉ();break;case 6:ΖЇ(Φ);break;}ǵ("Update: "+ƫ);ƫ++;if(ƫ>6){ƫ=0;Αι=true;if(Ţ!=Κ.з){switch(Ţ){case Κ.ĳ:ȣ();break;case Κ.Ĵ:ȣ();break;case Κ.Ϗ:ȣ();break;case Κ.ǅ:ƽ();break;case Κ.ǆ:đ();break;}Ţ=Κ.з;}}}if(!Ɛ){if(!Ƿ(Φ,true)){Φ=null;
ƛ=true;ύ=2;}if(ύ>=2&&Υ!=Α.ж)
ψ();if(ύ<=1){Ĭ=Φ.CalculateShipMass().PhysicalMass;Ȝ=(float) Φ.GetShipSpeed();ί=ΗϏ(Φ,ΑЁ);Αγ=Φ.WorldMatrix.Forward;Ɨ=Φ.WorldMatrix.Left;ƍ=Φ.WorldMatrix.Down;ΗЈ();ΗЊ();ΗЂ();if(ΓЍ){ΓЍ=false;ȣ();if(Υ!=Α.ж) Μ="Job resumed after restart";}if(Υ!=Α.ж){ő=false;ΔБ(false);Δγ(false);String ΘЎ=Βα(Υ)+" "+(int) Υ;
Ǻ();ΗІ();ΒЉ(false);ǵ(ΘЎ);ΗЋ();ΗЇ();Ǻ();ΗЃ();ǵ("Thruster");Ǻ();ΕΚ();ǵ("Gyroscope");}else{if(ő){if(ǂ()){Х(ƍ,Αγ,Ɨ,0.25f,true);ΕΚ();Μ="Aligning to planet: "+Math.Round(ΑΤ-0.25f,2)+"°";if(Ακ) Βδ(true,true);}else Βδ(true,true);}}ƪ=false;}}}Ǻ();ΔЌ();ǵ("Print");if(ΑЅ||ΓΘ<=0){Ǻ();ΖЃ(ΕΫ);
ǵ("Broadcast");ΓΘ=4;}else if(ă) ΓΘ--;}void ΔЍ(string ѓ){if(ѓ=="") return;var Ж=ѓ.ToUpper().Split(' ');Ж.DefaultIfEmpty("");var ě=Ж.ElementAtOrDefault(0);var Ę=Ж.ElementAtOrDefault(1);var Ɓ=Ж.ElementAtOrDefault(2);var ΕΣ=Ж.ElementAtOrDefault(3);String ā="Invalid argument: "+ѓ;bool ΔΡ=false;
switch(ě){case "UP":this.ΔЈ(false);break;case "DOWN":this.ΔЇ(false);break;case "UPLOOP":this.ΔЈ(true);break;case "DOWNLOOP":this.ΔЇ(true);break;case "APPLY":this.Δα(true);break;case "MRES":љ=0;break;case "STOP":this.ψ();break;case "PATHHOME":{this.ψ();this.ΒЅ();}break;case "PATH":{this.ψ();this.ΒЅ();Θ.δ=true;}break;
case "START":{this.ψ();ΒЄ();}break;case "ALIGN":{Βδ(!ő,false);}break;case "CONT":{this.ψ();this.ȣ();}break;case "JOBPOS":{this.ψ();this.đ();}break;case "HOMEPOS":{this.ψ();this.ƽ();}break;case "FULL":{ƣ=true;}break;case "RESET":{ΑЂ=true;ύ=2;}break;case "EXPORT":{ΗΘ();}break;
case "IMPORT":{ΗΤ(Ę=="JOB");}break;default:ΔΡ=true;break;}if(Η!=Ζ.Ϋ){switch(ě){case "SHUTTLE":{ΗΖ();}break;case "CFGS":{if(!ΖА(Ę,Ɓ,ΕΣ)) Μ=ā;}break;case "CFGB":{if(!ΖЏ(Ę,Ɓ)) Μ=ā;}break;case "CFGL":{if(!Ē(ref Ǭ,true,σ.ǃ,Ę,"")||!ΖБ(Ɓ))
Μ=ā;}break;case "CFGE":{if(!Ē(ref Ы,true,σ.Ǆ,Ę,"IG")||
!Ē(ref ч,true,σ.Βμ,Ɓ,"IG")||
!Ē(ref ш,true,σ.Βν,ΕΣ,"IG"))
Μ=ā;}break;case "CFGA":{if(!Ē(ref Ǧ,false,σ.Ʋ,Ę,"")) Μ=ā;}break;case "CFGW":{if(!Ē(ref ū,false,σ.ž,Ę,"")||
!Ē(ref Ū,false,σ.ž,Ɓ,""))
Μ=ā;}break;case "NEXT":{ΒΕ(false);}break;case "PREV":{ΒΕ(true);}break;default:if(ΔΡ) Μ=ā;break;}}else{switch(ě){case "UNDOCK":{ƪ=true;}break;default:if(ΔΡ) Μ=ā;break;}}}String ΗΜ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Η!=Ζ.Ϋ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΖ(){ψ();Η=Ζ.Ϋ;ǹ(а.Ϋ);Ε.δ=false;Θ.δ=false;ξ=null;ο.Clear();ή=Κ.з;}void Βδ(bool Ј,bool ф){if(!Ј) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ј){ő=false;ΕΔ();ΑΑ(false,0,0,0,0);return;}if(ǂ()) ő=true;}bool ΖА(String ΙΦ,String Θή,String ΘΟ)
{bool ΙΡ=ή==Κ.Ϗ;int w,h,d;if(int.TryParse(ΙΦ,out w)&&int.TryParse(Θή,out h)&&int.TryParse(ΘΟ,out d)){this.ψ();й=w;и=h;Ц=d;ĕ(false);Ž(false,false);if(ΙΡ) ȣ();return true;}return false;}bool ΖБ(String ΕΡ){if(ΕΡ=="ON"){Ƞ=true;return true;}if(ΕΡ=="OFF"){Ƞ=false;return true;}
return false;}bool ΖЏ(String Επ,String ΑΥ){bool Ɯ=true;if(Επ=="HOME") Ċ=true;else if(Επ=="STOP") Ċ=false;else Ɯ=false;if(ΑΥ=="HOME") л=д.ǋ;else if(ΑΥ=="STOP") л=д.ΕΓ;else if(ΑΥ=="JOB") л=д.Βά;else if(ΑΥ=="IG") л=д.ű;else Ɯ=false;return Ɯ;}void π(Ϊ ΓЃ){ΖΙ[(int) Ѝ]=љ;
љ=ΖΙ[(int) ΓЃ];if(ΓЃ==Ϊ.Ηυ) љ=0;Ѝ=ΓЃ;if(Η!=Ζ.р)
Ĕ(Ѝ==Ϊ.ŷ,false,0,0);ΖΚ=true;}void ΔЈ(bool Γυ){if(љ>0) љ--;else if(Γυ) љ=Αω-1;}void ΔЇ(bool Γυ){if(љ<Αω-1) љ++;else if(Γυ) љ=0;}String ǹ(а ρ){switch(ρ){case а.ǌ:Μ="Job is running";break;case а.ŭ:Μ="Connector not ready!";break;case а.Ηχ:Μ="Ship modified, path outdated!";break;
case а.ΔϏ:Μ="Interrupted by player!";break;case а.Ϋ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΟ(ǒ ε){switch(ε){case ǒ.ΑΔ:return "Top-Left";case ǒ.ΖЈ:return "Center";default:return "";}}String ΗΚ(я ƚ){switch(ƚ){case я.ΒΛ:return "Auto"+(Η==Ζ.Ÿ?" (Ore)":"");case я.ΒΜ:return "Auto (+Stone)";
case я.ΒΣ:return "Default";default:return "";}}String Βί(Κ Ф){switch(Ф){case Κ.з:return "No job";case Κ.ĺ:return "Job paused";case Κ.Ϗ:return "Job active";case Κ.ĳ:return "Job active";case Κ.Ĵ:return "Job active";case Κ.ΒΦ:return "Job done";case Κ.ƶ:return "Job changed";case Κ.ǅ:return "Move home";
case Κ.ǆ:return "Move to job";}return "";}String Δσ(д ΘΕ){switch(ΘΕ){case д.ǋ:return "Return home";case д.Βά:return "Fly to job pos";case д.ΕΓ:return "Stop";case д.ű:return "Ignore";}return "";}String ΗΛ(Ѐ ƚ){switch(ƚ){case Ѐ.Βο:return "Off";case Ѐ.ΒΧ:return "Drop pos (Stone) ";case Ѐ.ƹ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δθ:return "Cur. pos (Stone)";case Ѐ.ΒΠ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βθ:return "In motion (Stone)";case Ѐ.ǀ:return "In motion (Sto.+Ice)";}return "";}String Δρ(п Ф){switch(Ф){case п.None:return "No batteries";case п.ď:return "Charging";case п.Δλ:return "Discharging";}return "";}String Βα(Α Ф)
{String ΙІ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ȋ:return "Flying to XY position";case Α.Ĺ:return Η==Ζ.ǐ?"Grinding":"Mining";case Α.Ǹ:return "Returning";case Α.ů:return "Flying to drop pos";case Α.Ȧ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȥ:return "Flying to path";case Α.ķ:return "Flying to job position";case Α.Ǣ:return "Approaching dock";case Α.Đ:return "Docking";case Α.Č:return "Aligning";case Α.ȡ:return "Aligning";case Α.Ź:return "Retry docking";case Α.ǻ:return "Unloading";case Α.Ȩ:return Ζώ;
case Α.Ǔ:return "Undocking";case Α.ď:return "Charging batteries";case Α.ΑΙ:return "Waiting for uranium";case Α.ƻ:return "Filling up hydrogen";case Α.ΒЊ:return "Waiting for ejection";case Α.ΑΘ:return "Waiting for ejection";case Α.Ƽ:return "Flying to drop pos";}return "";}String Δω(ι Ќ){switch(Ќ)
{case ι.Βρ:return "On \"Undock\" command";case ι.Βυ:return "On player entered cockpit";case ι.ΔЋ:return "On ship is full";case ι.ΔЊ:return "On ship is empty";case ι.Βφ:return "On time delay";case ι.ǉ:return "On batteries empty(<25%)";case ι.ǈ:return "On batteries empty(=0%)";case ι.Βπ:return "On batteries full";
case ι.Βτ:return "On hydrogen empty(<25%)";case ι.Βς:return "On hydrogen empty(=0%)";case ι.Βσ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΫ,int ȳ,bool Ξ,String Ɩ){Αω+=1;if(ΖΫ==ȳ) Ɩ=">"+Ɩ+(ΑΝ>=2?" .":"");else Ɩ=" "+Ɩ;Γ+=Ɩ+"\n";return ΖΫ==ȳ&&Ξ;}String Δα(bool Ξ)
{int Β=0;int Ο=љ;Αω=0;String Ζή="———————————————\n";String ŧ="--------------------------------------------\n";String Γ="";Γ+=Βί(ή)+" | "+(Θ.δ?"Ready to dock":"No dock")+"\n";Γ+=Ζή;double Ǯ=Math.Max(Math.Round(this.Л),0);if(Ѝ==Ϊ.Main){bool Ă=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЅ();
if(Η==Ζ.Ÿ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.ŷ);if(Η==Ζ.ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.ŷ);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.ΒЁ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȣ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƽ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) đ();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ă) π(Ϊ.ΑΒ);else π(Ϊ.ƴ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.Δό);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δϊ);}else if(Ѝ==Ϊ.ŷ){double ΙΥ=Math.Round(й*Ǡ,1);double Θέ=Math.Round(и*Ǳ,1);String Є="";if(Π(ref Є,Ο,Β++,Ξ," Start new job!")) ΒЄ();if(Π(ref Є,Ο,Β++,Ξ," Change current job")){Ž(false,false);π(Ϊ.Main);}if(Π(ref Є,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΥ+"m)")){ĵ(ref й,5,20,1);ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Width -")){ĵ(ref й,-5,20,-1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height + (Height: "+и+" = "+Θέ+"m)")){ĵ(ref и,5,20,1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height -")){ĵ(ref и,-5,20,-1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth + ("+(у==я.ΒΣ?"Depth":"Min")+": "+Ц+"m)")){ĵ(ref Ц,5,50,2);ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Depth -")){ĵ(ref Ц,-5,50,-2);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth mode: "+ΗΚ(у))){у=Ȥ(у);}if(Π(ref Є,Ο,Β++,Ξ," Start pos: "+ΗΟ(ȝ))){ȝ=Ȥ(ȝ);}if(Η==Ζ.ǐ&&у==я.ΒΜ) у=Ȥ(у);Γ+=ΔЏ(8,Є,Ο,ref ΘЅ);}else if(Ѝ==Ϊ.ΒЁ){float[]Εθ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΑ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δω(ϊ.Ќ))) ϊ.Ќ=Ȥ(ϊ.Ќ);if(!ϊ.Βε()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βή((int) ϊ.ǖ))) ϊ.ǖ=Δι(ϊ.ǖ,Εθ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δω(ϋ.Ќ))) ϋ.Ќ=Ȥ(ϋ.Ќ);
if(!ϋ.Βε()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βή((int) ϋ.ǖ))) ϋ.ǖ=Δι(ϋ.ǖ,Εθ);}else if(Ѝ==Ϊ.ΕΑ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЄ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.ΒЁ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǘ!=""?ϊ.Ǘ:"-"))) ϊ.Ǘ=Ǉ(ref ΙΔ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǚ!=""?ϊ.ǚ:"-"))) ϊ.ǚ=Ǉ(ref ΙΖ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǘ!=""?ϋ.Ǘ:"-"))) ϋ.Ǘ=Ǉ(ref ΙΕ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǚ!=""?ϋ.ǚ:"-"))) ϋ.ǚ=Ǉ(ref ΙΗ);
}else if(Ѝ==Ϊ.Ŷ){String ΖΒ=Ǭ+" %";if(ă) Ā++;if(Ā>1){Ā=0;Αη++;if(Αη>1) Αη=0;}bool[]ΓБ=new bool[]{Ć.Count==0,ǧ==п.None,і.Count==0};int Ζω=0;while(true){Ζω++;Ģ++;if(Ģ>ΓБ.Length-1) Ģ=0;if(Ζω>=ΓБ.Length) break;if(!ΓБ[Ģ]) break;}bool Ă=Η==Ζ.Ϋ;if(!Ă&&Ƞ&&О!=-1&&Αη==0)
ΖΒ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){ψ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ă) π(Ϊ.ƴ);else π(Ϊ.ΑΒ);if(!Ă){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΕ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) ƪ=true;Γ+=ŧ;if(!Ă) Γ+="Progress: "+Math.Round(Ɣ,1)+" %\n";Γ+="State: "+Βα(Υ)+" "+Ǯ+"m \n";Γ+="Load: "+Αο+" % Max: "+ΖΒ+" \n";if(Ģ==0) Γ+="Uranium: "+(Ć.Count==0?"No reactors":Math.Round(į,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ģ==1) Γ+="Battery: "+(ǧ==п.None?Δρ(ǧ):Ǽ+"% "+(ч==-1||Ă?"":" Min: "+ч+" %"))+"\n";if(Ģ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% "+(ш==-1||Ă?"":" Min: "+ш+" %"))+"\n";}else if(Ѝ==Ϊ.ƴ){String Є="";if(Π(ref Є,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}
if(Π(ref Є,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ǃ,false);if(Π(ref Є,Ο,Β++,Ξ," Weight limit: "+(Ƞ?"On":"Off"))) Ƞ=!Ƞ;if(Π(ref Є,Ο,Β++,Ξ," Ejection: "+ΗΛ(Н))) Н=Ȥ(Н);if(Π(ref Є,Ο,Β++,Ξ," Toggle sorters: "+(ĭ?"On":"Off"))){ĭ=!ĭ;if(ĭ) ǥ(ΔΓ);}
if(Π(ref Є,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Є,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.Ǆ,true);if(Π(ref Є,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) г(ref ч,-5,σ.Βμ,true);if(Π(ref Є,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-10,σ.Βν,true);
if(Π(ref Є,Ο,Β++,Ξ," When done: "+(Ċ?"Return home":"Stop"))) Ċ=!Ċ;if(Π(ref Є,Ο,Β++,Ξ," On damage: "+Δσ(л))) л=Ȥ(л);if(Π(ref Є,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΖ);Γ+=ΔЏ(8,Є,Ο,ref ΘІ);}else if(Ѝ==Ϊ.ΒΖ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.ǐ?" Grinder":" Drill")+" inv. balancing: "+(Ő?"On":"Off"))) Ő=!Ő;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.ǐ?" grinders":" drills")+": "+(ň?"Fwd + Bwd":"Fwd"))) ň=!ň;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ū+"m/s")) г(ref ū,0.5f,σ.ž,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ū+"m/s")) г(ref Ū,0.5f,σ.ž,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ʋ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ĳ*100f+"%")) Δε(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ġ*100f+"%")) Δε(false,0.05f);
}else if(Ѝ==Ϊ.ΑΒ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ǃ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.Ǆ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δσ(л))) л=Ȥ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ʋ,false);
}else if(Ѝ==Ϊ.ΔЎ){double Θι=0;if(Σ.Count>0) Θι=Vector3.Distance(Σ.Last().Δ,ί);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΕ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȉ?"Use old home":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȉ?"Use old connector":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ũ?"Use old connector":(Ε.δ?"Was set! ":"none ")))) Ũ=!Ũ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ũ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ũ=!ũ;Γ+=ŧ;Γ+="Wp spacing: "+Math.Round(ΖЀ)+"m\n";}else if(Ѝ==Ϊ.ΒІ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){ψ();π(Ϊ.Main);}
Γ+=ŧ;Γ+="State: "+Βα(Υ)+" \n";Γ+="Speed: "+Math.Round(Ȝ,1)+"m/s\n";Γ+="Target dist: "+Ǯ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΧ+"\n";}else if(Ѝ==Ϊ.Δό){List<IMyTerminalBlock>ĝ=Δτ();if(ă) Ā++;if(Ā>=ĝ.Count) Ā=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.Δύ);
Γ+=ŧ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αο,1)+"% "+Math.Round(Ň,1)+" / "+Math.Round(Ŗ,1)+"\n";Γ+="Uranium: "+(Ć.Count==0?"No reactors":Math.Round(į,1)+"Kg "+Γχ)+"\n";Γ+="Battery: "+(ǧ==п.None?"":Ǽ+"% ")+Δρ(ǧ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% ")+"\n";
Γ+="Damage: "+(ĝ.Count==0?"None":""+(Ā+1)+"/"+ĝ.Count+" "+ĝ[Ā].CustomName)+"\n";}else if(Ѝ==Ϊ.Δύ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="Next scan: "+ΑЄ+"s\n";Γ+="Drill area: "+Math.Round(Ǡ,1)+"x"+Math.Round(Ǳ,1)+"m Ship: "+Math.Round(ī,1)+"x"+Math.Round(Ƣ,1)+"m ("+Math.Round(ζ,1)+"m)\n";
Γ+="Broadcast: "+(Ņ?"Online - "+ΓЏ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŕ*100f,1)+"% \n";}else if(Ѝ==Ϊ.Δϊ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+ΑЄ+"s\n";if(ΑЂ) Γ="Recompile script now";int Θκ=Γ.Split('\n').Length;for(int i=Θκ;i<=10;i++) Γ+="\n";Γ+=Ζή;Γ+="Last: "+Μ+"\n";
return Γ;}void Δε(bool Θδ,float ў){ψ();Ž(true,false);if(Θδ) г(ref Ĳ,ў,σ.Βχ,false);else г(ref ġ,ў,σ.Βχ,false);Δδ();Ĕ(true,true,0,0);}float Δι(float θ,float[]ΒΓ){float έ=ΒΓ[0];for(int i=ΒΓ.Length-1;i>=0;i--)
if(θ<ΒΓ[i]) έ=ΒΓ[i];return έ;}String Ǉ(ref int ȳ){String έ="";if(ȳ>=ΔΜ.Count) ȳ=-1;if(ȳ>=0) έ=ΔΜ[ȳ].CustomName;ȳ++;return έ;}void ΑΕ(string φ){if(ή!=Κ.Ϗ) return;if(φ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(φ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void ĵ(ref int value,int Ƅ,int Ȟ,int ΔΑ){if(Ƅ==0) return;if(value<Ȟ&&ΔΑ>0||value<=Ȟ&&ΔΑ<0){value+=ΔΑ;return;}int ΗЎ=Math.Abs(Ƅ);int ΒЋ=0;int Γώ=1;while(true){ΒЋ+=Γώ*ΗЎ*10;if(Ƅ<0&&value-Ȟ<=ΒЋ) break;if(Ƅ>0&&value-Ȟ<ΒЋ) break;
Γώ++;}value+=Γώ*Ƅ;}void ĕ(bool ΚΙ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ĕ(Ѝ==Ϊ.ŷ,false,0,0);}bool Ē(ref float value,bool ΘЂ,σ ς,String ģ,String Θα){if(ģ=="") return false;float ś=-1;bool ΕЅ=false;if(ģ.ToUpper()==Θα) ΕЅ=true;else if(!float.TryParse(ģ,out ś)) return false;
else ś=Math.Max(0,ś);if(ΘЂ) ś=(float) Math.Round(ś);г(ref value,ś,ς,ΕЅ,false);return true;}void г(ref float value,float ў,σ ς,bool Ʃ){г(ref value,value+ў,ς,Ʃ,true);}void г(ref float value,float ŗ,σ ς,bool Ʃ,bool ΙΩ){float ǜ=0,Ǜ=0;if(ς==σ.ž){ǜ=0.5f;Ǜ=10f;}if(ς==σ.Ʋ){ǜ=0.1f;Ǜ=1f;}
if(ς==σ.ű){ǜ=50f;Ǜ=100f;}if(ς==σ.Βμ){ǜ=5f;Ǜ=30f;}if(ς==σ.Ǆ){ǜ=1f;Ǜ=25f;}if(ς==σ.ǃ){ǜ=10f;Ǜ=95f;}if(ς==σ.Βν){ǜ=10f;Ǜ=90f;}if(ς==σ.Ηύ){ǜ=10f;Ǜ=1800;}if(ς==σ.Βχ){ǜ=0.0f;Ǜ=0.75f;}if(ŗ==-1&&Ʃ){value=-1;return;}if(value==-1) Ʃ=false;
bool ΖΤ=ŗ<ǜ||ŗ>Ǜ;if(ΖΤ&&ΙΩ){if(ŗ<value) value=Ǜ;else if(ŗ>value) value=ǜ;}else value=ŗ;if(ΖΤ&&Ʃ) value=-1;else value=Math.Max(ǜ,Math.Min(value,Ǜ));value=(float) Math.Round(value,2);}void ΒΕ(bool Θύ){if(Θύ) К=Math.Max(0,К-1);else К++;ΒΩ();ΒΨ();for(int k=0;k<ȃ.Count&&k<М.Length;k++) ΒϏ(ȃ[k],k<К);
Ё=-1;ΒЉ(true);}T Ȥ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΗ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΗ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЅ(){ư.Clear();for(int i=0;i<Σ.Count;i++) ư.Add(Σ[i]);Σ.Clear();
Αλ=true;Γκ=new Ψ(Θ);Γν=new Ψ(Ε);Θ.δ=false;if(Η==Ζ.Ϋ) Ε.δ=false;for(int i=0;i<џ.Count;i++)
if(!ȇ.Contains(џ.Keys.ElementAt(i)))
ȇ.Add(џ.Keys.ElementAt(i));ȉ=false;Ũ=false;ũ=false;π(Ϊ.ΔЎ);}void ΕΕ(){if(ȉ) Θ=Γκ;if(Ũ) Ε=Γν;if(ũ){Σ.Clear();for(int i=0;i<ư.Count;i++) Σ.Add(ư[i]);}Αλ=false;ψ();π(Ϊ.Main);}void ΗЈ(){if(!Αλ) return;if(Υ!=Α.ж){ΕΕ();return;}if(!Γκ.δ) ȉ=false;if(!Γν.δ) Ũ=false;if(ư.Count<=1) ũ=false;
IMyShipConnector Й=П(MyShipConnectorStatus.Connectable);if(Й==null) Й=П(MyShipConnectorStatus.Connected);if(Й!=null){if(Math.Round(Ȝ,2)<=0.20) ΓΦ++;else ΓΦ=0;if(ΓΦ>=5){if(Η==Ζ.Ϋ&&(Θ.δ||ȉ)&&Vector3.Distance(Θ.Δ,Й.GetPosition())>5){Ε.γ=Φ.WorldMatrix.Forward;Ε.υ=Φ.WorldMatrix.Left;
Ε.β=Φ.WorldMatrix.Down;Ε.ν=Φ.GetNaturalGravity();Ε.Δ=Й.GetPosition();Ε.δ=true;Ε.Ч=Й.Position;}else{Θ.γ=Φ.WorldMatrix.Forward;Θ.υ=Φ.WorldMatrix.Left;Θ.β=Φ.WorldMatrix.Down;Θ.ν=Φ.GetNaturalGravity();Θ.Δ=Й.GetPosition();Θ.δ=true;Θ.Ч=Й.Position;}}}double Γο=-1;if(Σ.Count>0) Γο=Vector3.Distance(ί,Σ.Last().Δ);
double ΘЌ=Math.Max(1.5,Math.Pow(Ȝ/100.0,2));double ΖΛ=Math.Max(Ȝ*ΘЌ,2);ΖЀ=ΖΛ;if((Γο==-1)||Γο>=ΖΛ){Ψ Ω=new Ψ(ί,Αγ,ƍ,Ɨ,Φ.GetNaturalGravity());Ω.ΖЅ(Ȉ,ȇ);Σ.Add(Ω);}}Ψ е(){if(Η!=Ζ.Ϋ) return ћ!=null?ћ:Θ;return Εγ;}void Ļ(Ψ ά,Κ Ф){Εγ=ά;if(ή==Κ.Ϗ) ц=Ф;}bool Ƕ(Ψ ά,float ό,bool ΙΠ,out Vector3 ε)
{if(ΙΠ){Vector3I Γβ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(Γβ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ε=new Vector3();return false;}Vector3 ΘΛ=Я(Φ,Τ.FatBlock.GetPosition()-ί);Vector3 ΘΚ=Я(Φ,Τ.FatBlock.WorldMatrix.Forward);ε=ά.Δ-ΒЈ(ά.γ,ά.β*-1,ΘΛ)-ΒЈ(ά.γ,ά.β*-1,ΘΚ)*ό;
return true;}else{ε=ά.Δ;return true;}}Vector3 Ȍ(int ΘΘ,int ş,bool ΘЀ){if(!ΘЀ&&Εό) return ΓΙ;float ΘΙ=((Д-1f)/2f)-ΘΘ;float ΘЃ=((б-1f)/2f)-ş;ΓΙ=Ε.Δ+Ε.υ*ΘΙ*Ǡ+ș*-1*ΘЃ*Ǳ;Εό=true;return ΓΙ;}Vector3 Βΰ(Vector3 ΘА,float ΓΣ){return ΘА+(њ*ΓΣ);}ΰ ΒΤ(){float Б=-1;ΰ έ=ΰ.ǳ;
if(Η!=Ζ.Ϋ){ΔІ();if(ή!=Κ.з){Vector3 Љ=ē(њ,ș*-1,ί-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(Д*Ǡ)/2f&&Math.Abs(Љ.Y)<=(float)(б*Ǳ)/2f){if(Љ.Z<=-1&&Љ.Z>=-ĩ*2) return ΰ.Ƴ;if(Љ.Z>-1&&Љ.Z<ζ*2) return ΰ.ǳ;}if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;}if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;for(int i=0;i<Σ.Count;i++)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Vector3.Distance(ί,Θ.Δ)<dockDist*ζ) έ=ΰ.ǲ;if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǲ;}}else{Vector3 ε=new Vector3();IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;if(Ƕ(Θ,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǲ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΙ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*ζ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ζ)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Ε.δ){if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;if(Ƕ(Ε,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǳ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΚ;}}return έ;}bool Ȣ(Vector3 ε,ref float Б){float ό=Vector3.Distance(ε,ί);if(ό<Б||Б==-1){Б=ό;return true;}return false;}void ΒЄ(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){ȣ();return;}Ε.Δ=ί;Ε.ν=Φ.GetNaturalGravity();Ε.γ=Αγ;Ε.β=ƍ;Ε.υ=Ɨ;
њ=ќ.WorldMatrix.Forward;ș=Ε.β;if(њ==Φ.WorldMatrix.Down) ș=Φ.WorldMatrix.Backward;Ž(true,true);Ν(Α.ȋ);ΒЃ();}void Ž(bool ǘ,bool ΘЁ){if(ή==Κ.з&&!ǘ) return;bool Θσ=ǘ||ή==Κ.ΒΦ||Д!=й||б!=и||Ĥ!=ȝ;if(Θσ){if(ή!=Κ.з){ή=Κ.ƶ;Ȍ(Ш,Щ,ΘЁ);Μ="Job changed, lost progress";}Ĥ=ȝ;
Д=й;б=и;Щ=0;Ш=0;М="";Ё=-1;ħ=0;ȗ=0;С=0;К=0;ΒЉ(true);}}void Βϋ(){Р(ί,0);ђ(Ȉ,true);}void ΒЃ(){ǹ(а.ǌ);Βϋ();Ż(Ȅ,false);ǥ(ΔΓ);ή=Κ.Ϗ;Δγ(true);ц=ή;π(Ϊ.Ŷ);Ηλ();ΑΦ=true;Αθ=0;for(int i=ſ.Count-1;i>=0;i--)
if(ų(ſ[i],false)) Αθ++;if(Αθ>0) Μ="Started with damage";}void ψ(){if(ή==Κ.Ϗ){ή=Κ.ĺ;Μ="Job paused";}Ν(Α.ж);ц=ή;ΑΑ(false,0,0,0,0);ΒЇ();Δΰ(new Vector3(),false);ΕΔ();Βώ(ChargeMode.Auto);ΒЀ(false);ΔБ(true);Ȏ(Α.ж);Ĕ(false,false,0,0);ђ(ο,false);ђ(т,true);ǥ(true);ΑЃ=false;Ƹ();
ƌ=false;Βό();ΑΦ=false;ƣ=false;ƪ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.ƴ&&Ѝ!=Ϊ.ΒΖ&&Ѝ!=Ϊ.ΑΒ)
π(Ϊ.Main);}void ȣ(){ΰ Е=ΒΤ();if(Η==Ζ.Ϋ){if(!Ε.δ||!Θ.δ) return;ΒЃ();bool Γΰ=Vector3.Distance(ί,Θ.Δ)<Vector3.Distance(ί,Ε.Δ);if(Ţ==Κ.ĳ) Γΰ=true;if(Ţ==Κ.Ĵ) Γΰ=false;if(Γΰ){Ļ(Θ,Κ.ĳ);switch(Е){case ΰ.ΒΙ:Ν(Α.Ȩ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.Ǣ);break;default:Ν(Α.Ǔ);break;
}}else{Ļ(Ε,Κ.Ĵ);switch(Е){case ΰ.ΒΚ:Ν(Α.Ȩ);break;case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.ȍ:Ν(Α.Ю);break;default:Ν(Α.Ǔ);break;}}}else{if(ή!=Κ.ĺ&&ή!=Κ.ƶ) return;bool ΙΣ=ή==Κ.ƶ;ΒЃ();bool Γε=Ŵ(false)&&Θ.δ;switch(Е){case ΰ.ǳ:Ν(Γε?Α.ȥ:Α.ȋ);break;case ΰ.ȍ:Ν(Γε?Α.Ѕ:Α.Ю);break;case ΰ.ǲ:Ν(Γε?Α.Ǣ:Α.ǻ);break;
case ΰ.Ƴ:{if(ȗ!=К||ΙΣ) Ν(Α.Ǹ);else Ν(Α.Ĺ);}break;default:break;}}}void đ(){if(ή==Κ.з&&!Θ.δ) return;if(Η==Ζ.Ϋ&&(!Ε.δ||!Θ.δ)) return;Μ="Move to job";ΰ Е=ΒΤ();if(Η==Ζ.Ϋ){Ļ(Ε,Κ.Ĵ);switch(Е){case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ΒΚ:return;default:Ν(Α.Ǔ);break;
}Ȏ(Α.Ȩ);}else{switch(Е){case ΰ.ǳ:Ν(Α.Ю);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.ǻ);break;case ΰ.Ƴ:Ν(Α.Ǹ);break;default:break;}if(ή==Κ.з) Ȏ(Α.Ю);else Ȏ(Α.ȡ);ΑЃ=true;}Βϋ();π(Ϊ.ΒІ);Ż(Ȅ,false);ц=Κ.ǆ;}void ƽ(){if(!Θ.δ) return;Μ="Move home";ΰ Е=ΒΤ();if(Η==Ζ.Ϋ){Ļ(Θ,Κ.ĳ);switch(Е)
{case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ǣ);break;case ΰ.ΒΙ:return;default:Ν(Α.Ǔ);break;}Ȏ(Α.Ȩ);}else{if(П(MyShipConnectorStatus.Connected)!=null) return;if(П(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Đ);Ȏ(Α.ǻ);return;}switch(Е){case ΰ.ǳ:Ν(Α.ȥ);break;case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ѕ);break;
case ΰ.Ƴ:Ν(Α.Ȧ);break;default:break;}Ȏ(Α.ǻ);}Βϋ();π(Ϊ.ΒІ);Ż(Ȅ,false);ц=Κ.ǅ;}void Ν(Α ΓϏ){if(ΓϏ==Α.ж) ƥ=Α.ж;if(ƥ!=Α.ж&&Υ==ƥ&&ΓϏ!=ƥ){ψ();return;}Ђ=true;Υ=ΓϏ;}void Ȏ(Α Ф){this.ƥ=Ф;}void ΗІ(){if(Υ==Α.ȋ){if(Ђ){Ѓ=0;if(ȗ!=К) ħ=0;ȗ=К;}if(Ѓ==0){int Ɔ=ΖЌ(К);if(Ɔ<0)
{if(!Ʒ()){Ν(Α.Ȧ);return;}if(!ΓΟ||(Ɔ==-2)!=ΑΡ){ΓΟ=true;ΑΡ=Ɔ==-2;Εΰ=!ΑΡ?ί:Ε.Δ-њ*ζ+
Ε.υ*(Д*Ǡ/2+ī/2+ζ/2);}Р(Εΰ,10);Μ=ΑΡ?"Waiting for a free hole":"Waiting for the controller";return;}ΓΟ=false;if(Ɔ!=К){К=Ɔ;ħ=0;ȗ=К;}Ľ έ=Ης(К);if(έ==Ľ.Δί){ή=Κ.ΒΦ;Μ="Job done";if(Ċ&&Θ.δ){Ν(Α.ȥ);Ȏ(Α.ǻ);ц=Κ.ǅ;}else{Ν(Α.ķ);Ȏ(Α.ȡ);ц=Κ.ǆ;}return;}if(έ==Ľ.Δπ)
{Ѓ=1;ђ(ο,true);щ=Ȍ(Ш,Щ,true);Р(щ,10);Х(Ε.β,Ε.γ,Ε.υ,false);}}else{if(Ё>=0&&(ΔЃ()?ǩ>=0&&ǩ!=Ё:Βη(Ё,true))){Ѓ=0;return;}if(Л<wpReachedDist){Ν(Α.Ĺ);return;}}}if(Υ==Α.Ĺ){if(Ђ){ђ(ο,true);ǥ(false);щ=Ȍ(Ш,Щ,false);Р(Βΰ(щ,0),0);Х(Ε.β,Ε.γ,Ε.υ,false);Ѓ=1;к=0;
Э=0;ΑΞ=0;Γπ=-1;ĩ=Ц;Γλ=true;}if(!Ʒ()){Ν(Α.Ȧ);return;}if(Ŵ(true)){ΖΡ=Ȋ("","ORE",И.č);if((Н==Ѐ.ΒΧ||Н==Ѐ.ƹ||
Н==Ѐ.Βθ||Н==Ѐ.ǀ)&&Η!=Ζ.ǐ)
Ν(Α.ů);else if((Н==Ѐ.Δθ||Н==Ѐ.ΒΠ)&&Η!=Ζ.ǐ)
Ν(Α.ΒЊ);else
Ν(Α.Ȧ);return;}С=Vector3.Distance(ί,щ);if(С>ħ){ħ=С;Γλ=false;}if(Η==Ζ.ǐ&&Δυ()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ѓ>0&&Ѓ<Э){if(С>к){if(Э>150) к=С;else к=(float) Math.Ceiling(С);Ѓ=1;}else Ѓ++;}else{if(Ѓ>0){Μ="Ship stuck! Retrying";к=С;Ѓ=0;Ĕ(false,true,0,ζ*sensorRange);}Р(Βΰ(щ,Math.Max(0,к-ζ)),Βγ(false));if(Л<=wpReachedDist/2){Ѓ=1;к=0;}
return;}Ĕ(false,true,ζ*sensorRange,0);Vector3 ΗА=щ+њ*С;bool ΕЉ=false;if(Vector3.Distance(ΗА,ί)>0.3f){Vector3 ΘΜ=щ+њ*(С+0.1f);Р(ΘΜ,4);ΕЉ=true;}else{float ǝ=Βγ(true);Vector3 Εη=Βΰ(щ,Math.Max(Ц+1,С+1));Ǥ(true,false,false,Εη,Εη-щ,ǝ,ǝ);}bool ΓΤ=false;if(у==я.ΒΜ||у==я.ΒΛ)
{if(!ΕЉ){float ΓЁ=0;foreach(IMyTerminalBlock ǰ in ο)
ΓЁ+=ΗΒ(ǰ,"","",у==я.ΒΛ?new string[]{"STONE"}:null);if(ΓЁ>Γπ||С<Ц||Γλ){ΒБ=0;ΑΞ=С;ĩ=(int)(Math.Max(ĩ,ΑΞ)+ζ/2);}else{ΓΤ=С-ΑΞ>2&&ΒБ>=20;ΒБ++;}Γπ=ΓЁ;}}else ΓΤ=С>=ĩ;if(ȗ!=К){Ν(Α.Ǹ);С=0;return;}if(ΓΤ){К++;Ν(Α.Ǹ);С=0;return;}}if(Υ==Α.Ǹ||Υ==Α.Ȧ||Υ==Α.ů)
{if(Ђ){щ=Ȍ(Ш,Щ,false);Х(Ε.β,Ε.γ,Ε.υ,false);ђ(ο,ň);ǥ(false);к=Vector3.Distance(ί,щ);Ĕ(false,true,0,ζ*sensorRange);}Р(щ,Βγ(false));if(Vector3.Distance(ί,щ)>=к+5){ђ(ο,false);ǥ(true);Μ="Can´t return!";}if(Л<wpReachedDist){if(Υ==Α.Ǹ&&ΑЃ) Ν(Α.ķ);if(Υ==Α.Ǹ) Ν(Α.ȋ);
if(Υ==Α.ů) Ν(Α.Ƽ);if(Υ==Α.Ȧ){if(Θ.δ) Ν(Α.ȥ);else{ψ();đ();Μ="Can´t return, no dock found";}}return;}}if(Υ==Α.Ѕ||Υ==Α.Ю){if(Υ==Α.Ю&&ή==Κ.Ϗ&&Η!=Ζ.Ϋ){if(!Ʒ()||Ŵ(true)){Ν(Α.Ѕ);return;}}bool ΓЊ=false;bool ΒΑ=false;bool ΕΤ=false;float ł=0;bool ΖΠ=false;Ψ Ω=null;
if(Ђ){if(Υ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȏ(1)) ΒΘ();else{ћ=null;ȅ=0;ƌ=!ΒΟ(1);}}Ψ ά=е();η=new Βω();η.Њ=ά;η.Ɗ=followPathDock*ζ;η.ΔΣ=useDockDirectionDist*ζ;η.ł=10;η.ť.Add(ά.Δ);Vector3 Ł=new Vector3();if(Ƕ(ά,dockDist*ζ,true,out Ł))
η.ť.Add(Ł);else η.Ɗ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) η.Αΰ=Ε.Δ;if(ά==Ε) η.Αΰ=Θ.Δ;η.Γέ=dockDist*ζ*1.1f;}}else if(Υ==Α.Ю){η=new Βω();η.Њ=Ε;η.Ɗ=followPathJob*ζ;η.ΔΣ=useJobDirectionDist*ζ;η.ł=10;η.Αΰ=Θ.Δ;η.Γέ=dockDist*ζ*1.1f;η.ť.Add(Ε.Δ);if(ή==Κ.з){if(!Θ.δ||Σ.Count==0){ψ();return;}
float ΘΣ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΤ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΣ<ΘΤ) η.Њ=Σ.Last();else η.Њ=Σ.First();}}Ɲ=new Vector3();ΖΠ=!ΗΪ(ί);ђ(ο,false);ǥ(true);У=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αΰ)<=η.Γέ) continue;
double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǩ==-1||ό<Ǩ){У=i;Ǩ=ό;}}ǭ=ΗΞ(η.Њ.Δ,У);Ĝ=null;}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&!ż()){Ψ ΓΔ=е();ƌ=!ΒΟ(1);if(е()!=ΓΔ){Ν(Α.Ѕ);return;}}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&ȏ(1)&&ΒΘ()){Ν(Α.Ѕ);return;}bool ΕЇ=false;if(Υ==Α.Ѕ&&!Ђ&&У>=0&&У<Σ.Count)
{Vector3 ƈ=Σ[У].Δ+Δφ(У,ǭ);int ΓЀ=У+ǭ;Vector3 Ř=ǭ!=0&&ΓЀ>=0&&ΓЀ<Σ.Count?Σ[ΓЀ].Δ:η.Њ.Δ;if(ΗΡ(ƈ,Ř)){if(!ΑΩ){ΑΩ=true;Εμ=ĸ(ƈ)?ί:ƈ;}Р(Εμ,0);Х(η.Њ.β,η.Њ.γ,η.Њ.υ,false);Μ="Waiting for dock clearance";return;}ΑΩ=false;ΕЇ=ȏ(1)&&(ĸ(ί)||ĸ(Ř));}ΖІ(Σ,ǭ,η.ť,η.Ɗ,Ђ,ref Ѓ);
for(int i=0;i<η.ť.Count;i++){float ό=Vector3.Distance(ί,η.ť[i]);if(ό<=η.Ɗ) ΓЊ=true;if(ό<=η.ΔΣ) ΒΑ=true;}if(ΕЇ) ΒΑ=true;if(ΒΑ) ł=η.ł;float ΚΛ=Ĝ!=null?Ĝ.Џ:Ȝ;float ΙΨ=(float) Math.Max(Ȝ*0.1f*ζ,wpReachedDist);if((Л<ΙΨ)||Ђ){if(!Ђ) У+=ǭ;if(ǭ==0||У>Σ.Count-1||У<0)
ΓЊ=true;else{ΔΧ=ǭ>0?Σ.Count-1-У:У;Ω=Σ[У];Ĝ=Ω;if(У>=1&&У<Σ.Count-1)
Ɲ=Ω.Δ-Σ[У-ǭ].Δ;else Ĝ=null;χ=Ω.Δ+Δφ(У,ǭ);ΕΤ=true;}}if(ΒΑ)
Х(η.Њ.β,η.Њ.γ,η.Њ.υ,false);else if(ΖΠ)
ΒΗ(η.Њ.β,10,true);else if(ΕΤ&&Ω!=null){if(ǭ>0) Х(Ω.β,Ω.γ,Ω.υ,90,false);else Х(Ω.β,-Ω.γ,-Ω.υ,90,false);}Ǥ(true,false,true,χ,Ɲ,Ĝ==null?0:Ĝ.Џ,ł);if(ΓЊ){ΔΧ=0;if(Υ==Α.Ѕ&&!ΖЊ()){Р(χ,0);return;}if(Υ==Α.Ѕ&&ƌ){Р(χ,0);Μ="Waiting for a free dock";return;}if(Υ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.Ǣ);return;}
if(Υ==Α.Ю&&ΑЃ){Ν(Α.ķ);return;}if(Υ==Α.Ю){Ν(Α.ȋ);return;}}}if(Υ==Α.Ǣ||Υ==Α.Ź){if(Η!=Ζ.Ϋ&&ż()){if(!ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΔ=е();if(!ΒΟ(2)||е()!=ΓΔ){Ν(Α.Ѕ);return;}}Ψ ά=е();if(Ђ){if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.ŭ);ψ();return;}
Р(χ,0);ΒΗ(ά.β,90,true);}if(Л<followPathDock*ζ&&Л!=-1){Р(χ,10);Х(ά.β,ά.γ,ά.υ,false);}if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null){Ν(Α.Đ);return;}if(Л<wpReachedDist/2&&Л!=-1){Ν(Α.Č);return;}}if(Υ==Α.Č||Υ==Α.ȡ){if(Ђ)
{if(Υ==Α.Č){Ψ ά=е();if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.ŭ);ψ();return;}Ǥ(true,true,false,χ,0);Х(ά.β,ά.γ,ά.υ,10,false);}if(Υ==Α.ȡ){Х(Ε.β,Ε.γ,Ε.υ,0.5f,false);χ=Ε.Δ;Ǥ(true,true,false,χ,0);}}if(Ακ){ΑΑ(false,0,0,0,0);if(Υ==Α.Č) Ν(Α.Đ);if(Υ==Α.ȡ) ψ();return;}}
if(Υ==Α.Đ){if(П(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.Ȩ);else Ν(Α.ǻ);return;}Ψ ά=е();if(Ђ){Э=0;ΘΥ=DateTime.Now;Ѓ=0;Х(ά.β,ά.γ,ά.υ,false);}Vector3I Γβ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Εί=Me.CubeGrid.GetCubeBlock(Γβ);float Εξ=dockingSpeed;
float Θν=dockingSpeed*5;float ΕΠ=Math.Max(1.5f,Math.Min(5f,ζ*0.15f));if(!Ƕ(ά,0,true,out χ)||!Ƕ(ά,ΕΠ,true,out Ɲ)||
Εί==null||!Εί.FatBlock.IsFunctional){ǹ(а.ŭ);ψ();return;}if(Э==1||(Vector3.Distance(ί,χ)<=ΕΠ*1.1f&&!Ђ))
Э=1;else{Vector3 ΙΘ=Я(Φ,Ɲ-ί);Vector3 Γα=Я(Φ,Φ.GetNaturalGravity());float ΘЄ=Ƶ(ΙΘ,Γα,null);Εξ=Math.Min(Θν,ΘЄ);}Ǥ(true,false,false,χ,χ-ί,dockingSpeed,Εξ);if(Ђ) к=(float) Л;IMyShipConnector ΓΞ=П(MyShipConnectorStatus.Connectable);if(ΓΞ!=null){Ǥ(false,false,false,χ,0);
if(Ѓ>0) Ѓ=0;Ѓ--;if(Ѓ<-5){ΓΞ.Connect();if(ΓΞ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.Ȩ);else Ν(Α.ǻ);ΒЇ();Ż(Ȅ,true);return;}}}else{float Ελ=(float) Math.Round(Л,1);if(Ελ<к){Ѓ=-1;к=Ελ;}else Ѓ++;if(Ѓ>20){Ν(Α.Ź);return;}}}if(Υ==Α.ǻ||Υ==Α.Ȩ||
Υ==Α.ΑΙ||Υ==Α.ƻ||Υ==Α.ď){bool ΒЏ=false,ΒА=false;if(Η==Ζ.Ϋ){if(е()==Θ) ΒЏ=true;else if(е()==Ε) ΒА=true;}if(Ђ){ƣ=false;if(П(MyShipConnectorStatus.Connected)==null){Ν(Α.Ǔ);return;}ΒЇ();if(ΒЏ) ΑΕ(ϊ.Ǘ);if(ΒА) ΑΕ(ϋ.Ǘ);Į=false;Ȑ=false;Ȕ=false;Ƭ=false;}if(П(MyShipConnectorStatus.Connected)==null){ψ();ǹ(а.ΔϏ);return;}
if(ή!=Κ.Ϗ||ч==-1||ǧ==п.None) Ȑ=true;else if(Ǽ>=100f) Ȑ=true;else if(Ǽ<=99f) Ȑ=false;if(ή!=Κ.Ϗ||ш==-1||і.Count==0) Ȕ=true;else if(ȕ>=100f) Ȕ=true;else if(ȕ<=99) Ȕ=false;if(ή!=Κ.Ϗ||Ы==-1||Ć.Count==0) Ƭ=true;else Ƭ=į>=Ы;ľ о=null;if(ΒЏ) о=ϊ;
if(ΒА) о=ϋ;if(о!=null&&(о.Ќ==ι.ǉ||о.Ќ==ι.ǈ)) Ȑ=true;if(о!=null&&(о.Ќ==ι.Βπ)) if(!Į) Ȑ=false;if(о!=null&&(о.Ќ==ι.Βτ||о.Ќ==ι.Βς)) Ȕ=true;if(о!=null&&(о.Ќ==ι.Βσ)) if(!Į) Ȕ=false;if(ă){ChargeMode Εέ=Ȑ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ќ==ι.ǈ||о.Ќ==ι.ǉ))
Εέ=ChargeMode.Discharge;Βώ(Εέ);ΒЀ(!Ȕ);}if(!Į){if(Η==Ζ.Ϋ)
Į=ή!=Κ.Ϗ||ΖЋ(Ђ,true)||ƪ;else
Į=ή!=Κ.Ϗ||ΔЀ();}else{if(!Ȑ) Ν(Α.ď);if(!Ȕ) Ν(Α.ƻ);if(!Ƭ) Ν(Α.ΑΙ);Ђ=false;}if(Į&&Ȑ&&Ȕ&&Ƭ){Βώ(ChargeMode.Auto);ΒЀ(false);if(ή==Κ.Ϗ){if(Η==Ζ.Ϋ){if(е()==Θ) ΑΕ(ϊ.ǚ);else if(е()==Ε) ΑΕ(ϋ.ǚ);if(е()==Θ) Ļ(Ε,Κ.Ĵ);else Ļ(Θ,Κ.ĳ);}}Ν(Α.Ǔ);return;}}if(Υ==Α.Ǔ){if(Ư){if(!Δζ()) return;
Ư=false;Ν(Α.Ǔ);return;}if(Ђ){if(!Δζ()){Ư=true;return;}IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Ю);return;}IMyShipConnector α=Ȁ.OtherConnector;ΔА(Ȁ,false);Ż(Ȅ,false);Ψ ά=null;Ψ Γι=Η==Ζ.Ϋ?Θ:е();if(Vector3.Distance(Ȁ.GetPosition(),Γι.Δ)<5f&&Θ.δ) ά=Γι;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.δ) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.ŭ);ψ();return;}Р(χ,5);Х(ά.β,ά.γ,ά.υ,false);}else Р(ί+α.WorldMatrix.Forward*dockDist*ζ,5);if(ή==Κ.Ϗ) ǹ(а.ǌ);}if(Л<wpReachedDist){ђ(т,true);Ν(Α.Ю);return;}}if(Υ==Α.ķ)
{if(Ђ){ǥ(true);ђ(ο,false);χ=Ε.Δ;Р(χ,20);Х(Ε.β,Ε.γ,Ε.υ,false);}if(Л<wpReachedDist/2){Ν(Α.ȡ);return;}}if(Υ==Α.ȥ){if(Ђ){ǥ(true);ђ(ο,false);int Б=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǩ==-1||ό<Ǩ){Б=i;Ǩ=ό;}}
if(Б==-1){Ν(Α.Ѕ);return;}χ=Σ[Б].Δ;Р(χ,10);Х(Ε.β,Ε.γ,Ε.υ,false);}if(Л<wpReachedDist){Ν(Α.Ѕ);return;}}if(Υ==Α.Ƽ){bool ф=false;if(Ђ){ǥ(true);if((Н==Ѐ.ΒΧ||Н==Ѐ.ƹ)&&ǂ()&&
Ķ(њ,Φ.GetNaturalGravity())<25&&Д>=2&&б>=2){Vector3 Ǝ=ί;if(Ш>0&&Щ<б-1) Ǝ=Ȍ(Ш-1,Щ+1,true);else if(Ш<Д-1&&Щ<б-1) Ǝ=Ȍ(Ш+1,Щ+1,true);else if(Ш<Д-1&&Щ>0) Ǝ=Ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ǝ=Ȍ(Ш-1,Щ-1,true);else ф=true;if(!ф) Р(Ǝ,10);
}else ф=true;}if(Л<wpReachedDist/2) ф=true;if(ф){Ν(Α.ΑΘ);return;}}if(Υ==Α.ΒЊ||Υ==Α.ΑΘ){if(Ђ){Ǥ(true,true,false,ί,0);ђ(ο,false);ǥ(true);Ѓ=-1;Э=Н==Ѐ.Βθ||Н==Ѐ.ǀ?0:-1;}bool Ğ=!Ʒ();int ΑЊ=Ȋ("STONE","ORE",И.č);if(Н==Ѐ.ΒΠ||Н==Ѐ.ǀ||Н==Ѐ.ƹ)
ΑЊ+=Ȋ("ICE","ORE",И.č);bool Θά=ΑЊ>0;bool Γά=false;if(Э>=0){float Ŀ=(float) Math.Sin(Δκ(Э))*Ǡ/3f;float ΘΑ=(float) Math.Cos(Δκ(Э))*Ǳ/3f;Vector3 ΘΗ=Ȍ(Ш,Щ,true)+ΒЈ(њ,ș*-1,new Vector3(Ŀ,ΘΑ,0));Р(ΘΗ,0.3f);if(Л<Math.Min(Ǡ,Ǳ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ѓ==-1||ΑЊ<Ѓ){Ѓ=ΑЊ;к=0;}
else{к++;if(к>50) Γά=true;}if(!Θά||Ğ||Γά){if(!Ğ){int ΙΝ=Ȋ("","ORE",И.č);if(Ŵ(true)) Ğ=true;else if(100-(А(ΙΝ,ΖΡ)*100)<minEjection) Ğ=true;else ǹ(а.ǌ);}if(Γά&&Ğ) Μ="Ejection failed";if(Υ==Α.ΑΘ){if(Ğ){if(Θ.δ) Ν(Α.ȥ);else{ψ();đ();Μ="Can´t return, no dock found";}}
else Ν(Α.ȋ);}else if(Ğ) Ν(Α.Ȧ);else Ν(Α.Ĺ);return;}}Ђ=false;}void ΖІ(List<Ψ>ċ,int κ,List<Vector3>Ƨ,float Ǯ,bool Ȇ,ref int Ǖ){if(Ȇ){for(int i=0;i<ċ.Count;i++) ċ[i].Џ=0;Ǖ=-1;return;}if(κ==0) return;int ў=κ*-1;if(Ǖ==-1) Ǖ=ў>0?1:ċ.Count-2;int ΕЋ=0;while(Ǖ>=1&&Ǖ<ċ.Count-1)
{if(ΕЋ>50) return;ΕЋ++;try{if((ў<0&&Ǖ>=1)||(ў>0&&Ǖ<=ċ.Count-2)){Ψ θ=ċ[Ǖ];bool ΖΞ=false;for(int j=0;j<Ƨ.Count;j++)
if(Vector3.Distance(θ.Δ,Ƨ[j])<=Ǯ){ΖΞ=true;break;}if(!ΖΞ){Ψ ΑϏ=ċ[Ǖ-ў];Ψ Ř=ċ[Ǖ+ў];Vector3 ΙΙ=θ.Δ-Ř.Δ;Vector3 ΔΝ=ΑϏ.Δ-θ.Δ;Vector3 Θώ=θ.Δ+Vector3.Normalize(ΙΙ)*ΔΝ.Length();Vector3 ΘΡ=ΑϏ.Δ-Θώ;Vector3 Απ=ē(κ>0?θ.γ:θ.γ*-1,θ.β*-1,ΘΡ);Vector3 Θμ=ē(κ>0?θ.γ:θ.γ*-1,θ.β*-1,ΔΝ);
Vector3 Ας=ē(κ>0?θ.γ:θ.γ*-1,θ.β*-1,θ.ν);θ.Џ=(float) Math.Sqrt(Math.Pow(ΑϏ.Џ,2)+Math.Pow(Ƶ(-Θμ,Ας,θ),2));for(int j=0;j<Ƨ.Count;j++)
if(Vector3.Distance(ΑϏ.Δ,Ƨ[j])<=Ǯ){Vector3 Ασ=ē(κ>0?θ.γ:θ.γ*-1,θ.β*-1,Ƨ[j]-θ.Δ);float ΙΑ=Ƶ(-Ασ,Ας,θ);θ.Џ=Math.Min(θ.Џ,ΙΑ)/2f;}if(Απ.Length()==0) Απ=new Vector3(0,0,1);Vector3 Θλ=ē(θ.γ,θ.β*-1,θ.ν);float ΙΓ=Ű(-Απ,Θλ,θ);float ΑΚ=А(ΙΓ,Ĭ);float ΔΛ=(float) Math.Sqrt(А(Απ.Length()*1.0f,0.5f*ΑΚ));
θ.Џ=Math.Min(θ.Џ,(ΔΝ.Length()/ΔΛ)*Ǧ);}}}catch{return;}Ǖ+=ў;}Ǖ=-1;}void ΒЉ(bool Ȇ){if(Ȇ){Ɣ=0;return;}if(Η==Ζ.Ϋ) return;float ф=К*Math.Max(1,Ц);if(ȗ==К) ф+=Math.Min(Ц,С);float ΙΜ=Д*б*Math.Max(1,Ц);Ɣ=Math.Max(Ɣ,(float) Math.Min(ф/ΙΜ*100.0,100));}MyDetectedEntityType Δυ()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βγ(bool ǫ){if(Η==Ζ.ǐ&&Δυ()==MyDetectedEntityType.None&&!ų(ξ,true))
return fastSpeed;else return ǫ?ū:Ū;}void ΗЀ(){float ΖΘ=0,Εε=0,ģ=0,ΓЂ=0;for(int i=0;i<ȩ.Count;i++){IMyBatteryBlock ƃ=ȩ[i];if(!Ƿ(ƃ,true)) continue;ΖΘ+=ƃ.MaxStoredPower;Εε+=ƃ.CurrentStoredPower;ģ+=ƃ.CurrentInput;ΓЂ+=ƃ.CurrentOutput;}Ǽ=(float) Math.Round(А(Εε,ΖΘ)*100,1);
if(ģ>=ΓЂ) ǧ=п.ď;else ǧ=п.Δλ;if(ģ==0&&ΓЂ==0||Ǽ==100.0) ǧ=п.ж;if(ȩ.Count==0) ǧ=п.None;}void ΗЄ(){float Ετ=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζο=і[i];if(!Ƿ(Ζο,true)) continue;Ετ+=(float) Ζο.FilledRatio;}ȕ=А(Ετ,і.Count)*100f;}void ΗЌ(){į=0;try{for(int i=0;i<Ć.Count;i++)
{IMyReactor ΓЋ=Ć[i];if(!Ƿ(ΓЋ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЋ.GetInventory(0).GetItems(Ъ);float ǔ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
ǔ+=(float) Г.Amount;}if(ǔ<į||i==0){į=ǔ;Γχ=ΓЋ.CustomName;}}}catch(Exception e){Ȗ=e;}}void ΗЁ(){Ŗ=0;Ň=0;try{for(int i=0;i<ņ.Count;i++){IMyTerminalBlock Τ=ņ[i];if(!Ƿ(Τ,true)) continue;Ň+=(float) Τ.GetInventory(0).CurrentVolume;Ŗ+=(float) Τ.GetInventory(0).MaxVolume;
}Αο=(float) Math.Min(Math.Round(А(Ň,Ŗ)*100,1),100.0);}catch(Exception e){Ȗ=e;}}void ΗЅ(){try{Ǚ.Clear();for(int i=0;i<ņ.Count;i++){IMyTerminalBlock Τ=ņ[i];if(!Ƿ(Τ,true)) continue;Δή(Τ,И.Ų);}if(Н!=Ѐ.Βο){for(int i=0;i<ė.Count;i++){IMyTerminalBlock Τ=ė[i];if(!Ƿ(Τ,true)) continue;
Δή(Τ,И.ΗΦ);}}Ηψ(Ǚ);}catch(Exception e){Ȗ=e;}}void ΖЉ(){if(ΑΦ){if(Δτ().Count>Αθ){ΑΦ=false;if(л!=д.ű){ψ();if(л==д.Βά) đ();if(л==д.ǋ)
if(Θ.δ) ƽ();else đ();}Μ="Damage detected";}}}bool Ʒ(){if(!Αι) return true;if(ή==Κ.Ϗ){if(ч>0&&ǧ!=п.None)
if(Ǽ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&Ć.Count>0)
if(į<=Ы){Μ="Low fuel: "+Γχ;return false;}if(ш>0&&і.Count>0)
if(ȕ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δτ(){List<IMyTerminalBlock>ĝ=new List<IMyTerminalBlock>();for(int i=0;i<ſ.Count;i++){IMyTerminalBlock Τ=ſ[i];if(ų(Τ,false)) ĝ.Add(Τ);}return ĝ;}bool ų(IMyTerminalBlock Τ,bool ΔΕ){return(!Ƿ(Τ,ΔΕ)||!Τ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Τ,bool ΔΕ){if(Τ==null) return false;try{IMyCubeBlock Εϋ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΔΕ) return Εϋ==Τ;else return Εϋ.GetType()==Τ.GetType();}catch{return false;}}float ΗΒ(IMyTerminalBlock Τ,String ю,String Ζζ,String[]Ǫ){float ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory ŏ=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ŏ.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(Ǫ!=null&&(Ǫ.Contains(Г.Type.TypeId.ToUpper())||Ǫ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Г.Type.TypeId.ToUpper()==ю)&&(Ζζ==""||Г.Type.SubtypeId.ToUpper()==Ζζ))
ȁ+=(float) Г.Amount;}}return ȁ;}ѐ ΗΙ(String φ,String ς,И Е,bool ΘΝ){φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ѐ Г=Ǚ[i];if(Г.φ.ToUpper()==φ&&Г.ς.ToUpper()==ς&&(Г.œ==Е||Е==И.č))
return Г;}ѐ έ=null;if(ΘΝ){έ=new ѐ(φ,ς,0,Е);Ǚ.Add(έ);}return έ;}int Ȋ(String φ,String ς,И Е){return Ȋ(φ,ς,Е,null);}int Ȋ(String φ,String ς,И Е,String[]Ǫ){int ȁ=0;φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ѐ Г=Ǚ[i];if(Ǫ!=null&&Ǫ.Contains(Г.φ.ToUpper())) continue;
if((φ==""||Г.φ.ToUpper()==φ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.œ==Е||Е==И.č))
ȁ+=Г.ǔ;}return ȁ;}void Δή(IMyTerminalBlock Τ,И Е){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΙ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Е,true).ǔ+=(int) Ъ[j].Amount;}}void Ηψ(List<ѐ>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){ѐ a=Т[j];ѐ b=Т[j+1];if(a.ǔ<b.ǔ) Т.Move(j,j+1);}}bool Ŵ(bool ΙА){if(Ƞ&&Η!=Ζ.Ϋ)
if(О!=-1&&Ĭ>=О){Μ="Ship too heavy";return true;}if(Αο>=Ǭ||ƣ){ƣ=false;Μ="Ship is full";return true;}return false;}bool ΔЀ(){String[]Ǫ=null;if(!ȟ) Ǫ=new string[]{"ICE"};if(Η==Ζ.Ÿ) return Ȋ("","ORE",И.Ų,Ǫ)==0;if(Η==Ζ.ǐ) return Ȋ("","COMPONENT",И.Ų,Ǫ)==0;else return Ȋ("","",И.Ų,Ǫ)==0;
}bool ΗΫ(){List<IMyCockpit>ΓΝ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΝ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΝ.Count;i++)
if(ΓΝ[i].IsUnderControl) return true;return false;}bool ΖЋ(bool Ȇ,bool ΑІ){IMyShipConnector Й=П(MyShipConnectorStatus.Connected);if(Й==null) return false;if(Vector3.Distance(Θ.Δ,Й.GetPosition())<5)
return Δη(ϊ,Ȇ,ΑІ);if(Vector3.Distance(Ε.Δ,Й.GetPosition())<5)
return Δη(ϋ,Ȇ,ΑІ);return false;}bool Δη(ľ ǿ,bool Ȇ,bool ΑІ){if(Ȇ) ǿ.Ηθ();ǿ.ΗЉ();bool н=false;String ь="";switch(ǿ.Ќ){case ι.Βρ:{ь="Waiting for command";н=false;break;}case ι.ΔЋ:{ь="Waiting for cargo";н=Ŵ(true);break;}case ι.ΔЊ:{ь="Unloading";н=ΔЀ();break;}case ι.Βφ:{н=true;break;}
case ι.Βπ:{ь="Charging batteries";н=Ǽ>=100f;break;}case ι.ǉ:{ь="Discharging batteries";н=Ǽ<=25f;break;}case ι.ǈ:{ь="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βσ:{ь="Filling up hydrogen";н=ȕ>=100f;break;}case ι.Βτ:{ь="Unloading hydrogen";н=ȕ<=25f;break;}
case ι.Βς:{ь="Unloading hydrogen";н=ȕ<=0f;break;}case ι.Βυ:{bool ΕІ=ΗΫ();if(!ΕІ) ǿ.ΓЇ=true;н=ǿ.ΓЇ&&ΕІ;ь="Waiting for passengers";break;}}if(!н) ǿ.ΒΞ(true);if(н&&ǿ.Βε()){н=ǿ.ΒΞ(false);ь="Undocking in: "+Βή((int) Math.Max(0,ǿ.ǖ-ǿ.ȯ));}if(ΑІ) Ζώ=ь;return н;}void ΖЂ()
{if(!Ő) return;if(ο.Count<=1) return;float Ζφ=0,Ζυ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(ų(ǰ,true)) continue;Ζφ+=(float) ǰ.GetInventory(0).MaxVolume;Ζυ+=(float) ǰ.GetInventory(0).CurrentVolume;}float ΕΥ=(float) Math.Round(А(Ζυ,Ζφ),5);for(int ΖΥ=0;ΖΥ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΥ++)
{float Γη=0,Γφ=0;float ΕЃ=0,ΖΖ=0;IMyTerminalBlock ȓ=null,Ȱ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(ų(ǰ,true)) continue;float Γύ=(float) ǰ.GetInventory(0).MaxVolume;float Αα=А((float) ǰ.GetInventory(0).CurrentVolume,Γύ);if(ȓ==null||Αα>Γη){ȓ=ǰ;Γη=Αα;ΕЃ=Γύ;}
if(Ȱ==null||Αα<Γφ){Ȱ=ǰ;Γφ=Αα;ΖΖ=Γύ;}}if(ȓ==null||Ȱ==null||ȓ==Ȱ) return;if(checkConveyorSystem&&!ȓ.GetInventory(0).IsConnectedTo(Ȱ.GetInventory(0))){if(ΓΓ>20) Μ="Inventory balancing failed";else ΓΓ++;return;}ΓΓ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȓ.GetInventory(0).GetItems(Ъ);float Ʈ=0;if(Ъ.Count==0) continue;MyInventoryItem Αβ=Ъ[0];String ΕЊ=Αβ.Type.TypeId+Αβ.Type.SubtypeId;if(!Ζύ.TryGetValue(ΕЊ,out Ʈ)){if(Ηί(ȓ.GetInventory(0),0,Ȱ.GetInventory(0),out Ʈ))
Ζύ.Add(ΕЊ,Ʈ);else return;}float ΙΚ=((Γη-ΕΥ)*ΕЃ/Ʈ);float ΙΛ=((ΕΥ-Γφ)*ΖΖ/Ʈ);int ΔΠ=(int) Math.Min(ΙΛ,ΙΚ);if(ΔΠ<=0) return;if((float) Αβ.Amount<ΔΠ)
ȓ.GetInventory(0).TransferItemTo(Ȱ.GetInventory(0),0,null,null,Αβ.Amount);else
ȓ.GetInventory(0).TransferItemTo(Ȱ.GetInventory(0),0,null,null,ΔΠ);}}bool Ηί(IMyInventory Ƥ,int Β,IMyInventory ΘΠ,out float Ζό){Ζό=0;float ΘΔ=(float) Ƥ.CurrentVolume;List<MyInventoryItem>ΓΕ=new List<MyInventoryItem>();Ƥ.GetItems(ΓΕ);float ΕΨ=0;for(int i=0;i<ΓΕ.Count;i++) ΕΨ+=(float) ΓΕ[i].Amount;
Ƥ.TransferItemTo(ΘΠ,Β,null,null,1);float Ζϋ=ΘΔ-(float) Ƥ.CurrentVolume;List<MyInventoryItem>ΒЌ=new List<MyInventoryItem>();Ƥ.GetItems(ΒЌ);float ΕΜ=0;for(int i=0;i<ΒЌ.Count;i++) ΕΜ+=(float) ΒЌ[i].Amount;if(Ζϋ==0f||!Βι(0.9999,ΕΨ-ΕΜ,1.0001))
return false;Ζό=Ζϋ;return true;}void ΗЃ(){Vector3 Ƒ=χ+ĉ;Vector3 ǯ=Ƒ-ί;if(ǯ.Length()==0) ǯ=new Vector3(0,0,-1);Vector3 Ασ=Я(Φ,ǯ);Vector3 Ζπ=Vector3.Normalize(Ασ);Vector3 Œ=Я(Φ,Φ.GetNaturalGravity());float ΘЍ=ΓЅ>0?Math.Max(0,1-Ķ(ǯ,Εφ)/5):0;float Џ=(float) Math.Min((Γϊ>0?Γϊ:1000f),Math.Max(Ƶ(-Ασ,Œ,null),ΓЅ*ΘЍ));
if(!Ζτ) Џ=0;if(Ζΰ) Џ=Math.Max(0,1-ΑΤ/ΑΛ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(Ŝ) Џ=0;if(э>=0) Џ=Math.Min(Џ,э);if(Ζα) Џ*=(float) Math.Min(1,А(ǯ.Length(),wpReachedDist)/2);Vector3 ΖΓ=Я(Φ,Φ.GetShipVelocities().LinearVelocity);float ΗБ=(float)(Math.Max(0,15-Ķ(-Ζπ,-ΖΓ))/15)*0.85f+0.15f;
ΔΙ+=Math.Sign(ΗБ-ΔΙ)/10f;Vector3 ȫ=Ζπ*Џ*ΔΙ-(ΖΓ);Vector3 ΓΒ=Ββ(ȫ,null);if(ΑЌ&&Л>0.1f){ȫ.X*=ΒЂ(ȫ.X,ref ΑЍ.X,1f,ΓΒ.X,20);ȫ.Y*=ΒЂ(ȫ.Y,ref ΑЍ.Y,1f,ΓΒ.Y,20);ȫ.Z*=ΒЂ(ȫ.Z,ref ΑЍ.Z,1f,ΓΒ.Z,20);}else ΑЍ=new Vector3(1,1,1);ƨ=Ĭ*ȫ-Œ*Ĭ;Δΰ(ƨ,ΑЌ);Л=Vector3.Distance(ί,Ƒ);
}float ΒЂ(float value,ref float Ф,float ў,float Ș,float Γϋ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ў;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γϋ||Ș==0) return έ;Ф=Math.Min(Γϋ,Math.Max(-Γϋ,Ф));έ=Math.Abs(Ș);return έ;
}void ΕΚ(){float ы=90,Ǐ=90,ѝ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǫ,ј,Ƙ,Γσ;if(Γτ){ǫ=Vector3.Normalize(χ-ί);Ƙ=Я(Φ,ǫ);ј=Я(Φ,ΔΗ);ы=Ķ(Ƙ,new Vector3(0,-1,0))-90;Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);
ѝ=Ď(Ƙ,new Vector3(-1,0,0),Ƙ.Z);}else{ǫ=ΔΘ;ј=Я(Φ,ΔΗ);Ƙ=Я(Φ,ΔΘ);Γσ=Я(Φ,Ζρ);ы=Ď(ј,new Vector3(0,0,1),ј.Y);Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);ѝ=Ď(Γσ,new Vector3(0,0,1),Γσ.X);}if(ΕΞ&&ǂ()){Vector3 Γα=Φ.GetNaturalGravity();ј=Я(Φ,Γα);ы=Ď(ј,new Vector3(0,0,1),ј.Y);
Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);}if(!Βι(-45,Ǐ,45)){ы=0;ѝ=0;}if(!Βι(-45,ѝ,45)) ы=0;ΑΑ(Γγ,1,(-ы)*ǝ,(-ѝ)*ǝ,(-Ǐ)*ǝ);ΑΤ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(Ǐ),Math.Abs(ѝ)));Ακ=ΑΤ<=ΑΛ;}void ΖЇ(IMyTerminalBlock ΑЀ){if(ΑЀ==null) return;ǡ=new float[3,2];џ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(ΑЀ,в.WorldMatrix.Backward);float Ȯ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǡ[0,0]+=Ȯ;else ǡ[0,1]-=Ȯ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǡ[1,0]+=Ȯ;else ǡ[1,1]-=Ȯ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǡ[2,0]+=Ȯ;else ǡ[2,1]-=Ȯ;String ю=Δψ(в);float[,]Ǎ=null;if(џ.ContainsKey(ю)) Ǎ=џ[ю];else{Ǎ=new float[3,2];џ.Add(ю,Ǎ);}float Ș=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) Ǎ[0,0]+=Ș;else Ǎ[0,1]-=Ș;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) Ǎ[1,0]+=Ș;else Ǎ[1,1]-=Ș;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) Ǎ[2,0]+=Ș;else Ǎ[2,1]-=Ș;}}static String Δψ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βλ(Vector3 ѕ,float[,]Ŕ){return new Vector3(
ѕ.X>=0?Ŕ[0,0]:Ŕ[0,1],
ѕ.Y>=0?Ŕ[1,0]:Ŕ[1,1],
ѕ.Z>=0?Ŕ[2,0]:Ŕ[2,1]);}bool Δχ(Ψ Ω,String ю,out float ΓΫ){ΓΫ=0;int Β=ȇ.IndexOf(ю);if(Β==-1||Ω.ǟ==null||Β>=Ω.ǟ.Length)
return false;ΓΫ=Ω.ǟ[Β];if(ΓΫ==-1) return false;return true;}Vector3 Ββ(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<џ.Keys.Count;i++){String ю=џ.Keys.ElementAt(i);float Ə=0;if(!Δχ(Ω,ю,out Ə))
return Βλ(ѕ,ǡ);έ+=Βλ(ѕ,џ.Values.ElementAt(i))*Ə;}return έ;}return Βλ(ѕ,ǡ);}float Ű(Vector3 ѕ,Ψ Ω){return Ű(ѕ,new Vector3(),Ω);}float Ű(Vector3 ѕ,Vector3 ν,Ψ Ω){Vector3 ΙΒ=Ββ(ѕ,Ω);Vector3 Ȯ=ΙΒ+ν*Ĭ;float ƞ=(Ȯ/ѕ).AbsMin();return(float)(ѕ*ƞ).Length();}float Ƶ(Vector3 Αρ,Vector3 Œ,Ψ Ω)
{if(Αρ.Length()==0) return 0;float ΕΝ=1;if(Œ.Length()>0) ΕΝ=Math.Min(1,Ķ(-Œ,Αρ)/90)*0.4f+0.6f;float ƨ=Ű(Αρ,Œ,Ω);if(ƨ==0) return 0.1f;float ΑΚ=А(ƨ,Ĭ);float ΔΛ=(float) Math.Sqrt(А(Αρ.Length(),ΑΚ*0.5f));return ΑΚ*ΔΛ*ΕΝ*Ǧ;}float ƿ(Vector3 ǫ,Vector3 ώ,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Ας=ē(ǫ,ώ,Vector3.Normalize(ν));float ǘ=Ű(-Ας,Ω);return ǘ/ν.Length();}void Δγ(bool Θβ){float ǎ=0;float Γή=0.9f;if(Θβ){О=-1;ı=0;ΑЈ=null;if(ή!=Κ.з&&Ε.ν.Length()!=0){ǎ=Γή*ƿ(Ε.γ,Ε.β*-1,Ε.ν,null);if(ǎ<О||О==-1) О=ǎ;}if(Θ.δ&&Θ.ν.Length()!=0)
{ǎ=Γή*ƿ(Θ.γ,Θ.β*-1,Θ.ν,null);if(ǎ<О||О==-1) О=ǎ;}return;}if(ı==-1) return;if(ı>=0){int ΖΨ=0;while(ı<Σ.Count){if(ΖΨ>100) return;ΖΨ++;Ψ Ω=Σ[ı];if(Ω.ν.Length()!=0f){ǎ=Γή*Math.Min(
ƿ(Ω.γ,Ω.β*-1,Ω.ν,Ω),
ƿ(Ω.γ*-1,Ω.β*-1,Ω.ν,Ω));if(ǎ<О||О==-1) О=ǎ;}else ΑЈ=Ω;ı++;}ı=-1;}bool Γδ=true;float ą=0;if(ΑЈ!=null){for(int i=0;i<џ.Count;i++){String ю=џ.Keys.ElementAt(i);float[,]Ǎ=џ.Values.ElementAt(i);float Ə=0;if(!Δχ(ΑЈ,ю,out Ə)){Γδ=false;break;}for(int a=0;a<Ǎ.GetLength(0);a++)
for(int b=0;b<Ǎ.GetLength(1);b++){float Ė=Math.Abs(Ǎ[a,b]*Ə);if(Ė==0) continue;Γδ=true;if(ą==0||Ė<ą) ą=Ė;}}}if(!Γδ){for(int a=0;a<ǡ.GetLength(0);a++)
for(int b=0;b<ǡ.GetLength(1);b++){float Ė=Math.Abs(ǡ[a,b]);if(Ė==0) continue;if(ą==0||Ė<ą) ą=Ė;}}if(ą>0){float Θο=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǎ=А(ą,Θο);if(ǎ>0)
if(ǎ<О||О==-1) О=ǎ;}}void Δΰ(Vector3 ǘ,bool Ј){if(!Ј){for(int i=0;i<Ȉ.Count;i++)
Ȉ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓΑ=Ββ(ǘ,null);float ΙΪ=Math.Min(1,Math.Abs(А(ǘ.X,ΓΑ.X)));float ΙΫ=Math.Min(1,Math.Abs(А(ǘ.Y,ΓΑ.Y)));float Ιά=Math.Min(1,Math.Abs(А(ǘ.Z,ΓΑ.Z)));for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];Vector3 κ=Ηκ(Я(Φ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǘ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΪ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǘ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΫ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǘ.Z))
в.SetValueFloat("Override",в.MaxThrust*Ιά);else
в.SetValueFloat("Override",0.0f);}}void ΑΑ(bool Ј,float Θϋ,float ы,float ѝ,float Ǐ){for(int i=0;i<Αδ.Count;i++){IMyGyro І=Αδ[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=Θϋ;if(!Ј) continue;Vector3 В=Φ.WorldMatrix.Forward;Vector3 Ь=Φ.WorldMatrix.Right;Vector3 ώ=Φ.WorldMatrix.Up;
Vector3 Ŋ=І.WorldMatrix.Forward;Vector3 Ō=І.WorldMatrix.Up;Vector3 ŋ=І.WorldMatrix.Left*-1;if(Ŋ==В) І.SetValueFloat("Roll",Ǐ);else if(Ŋ==(В*-1)) І.SetValueFloat("Roll",Ǐ*-1);else if(Ō==(В*-1)) І.SetValueFloat("Yaw",Ǐ);else if(Ō==В) І.SetValueFloat("Yaw",Ǐ*-1);else if(ŋ==В) І.SetValueFloat("Pitch",Ǐ);
else if(ŋ==(В*-1)) І.SetValueFloat("Pitch",Ǐ*-1);if(ŋ==(Ь*-1)) І.SetValueFloat("Pitch",ы);else if(ŋ==Ь) І.SetValueFloat("Pitch",ы*-1);else if(Ō==Ь) І.SetValueFloat("Yaw",ы);else if(Ō==(Ь*-1)) І.SetValueFloat("Yaw",ы*-1);else if(Ŋ==(Ь*-1)) І.SetValueFloat("Roll",ы);
else if(Ŋ==Ь) І.SetValueFloat("Roll",ы*-1);if(Ō==(ώ*-1)) І.SetValueFloat("Yaw",ѝ);else if(Ō==ώ) І.SetValueFloat("Yaw",ѝ*-1);else if(ŋ==ώ) І.SetValueFloat("Pitch",ѝ);else if(ŋ==(ώ*-1)) І.SetValueFloat("Pitch",ѝ*-1);else if(Ŋ==ώ) І.SetValueFloat("Roll",ѝ);else if(Ŋ==(ώ*-1)) І.SetValueFloat("Roll",ѝ*-1);
}}void ΕΔ(){this.Γγ=false;}void Х(Vector3 ȭ,Vector3 В,Vector3 Γς,float Ȟ,bool ƭ){ΒΗ(ȭ,Ȟ,ƭ);ΑΛ=Ȟ;Γτ=false;this.ΔΘ=В;this.Ζρ=Γς;}void Х(Vector3 ȭ,Vector3 В,Vector3 Γς,bool ƭ){Х(ȭ,В,Γς,2f,ƭ);}void ΒΗ(Vector3 ȭ,float Ȟ,bool ƭ){ΑΛ=Ȟ;this.Γγ=true;this.ΕΞ=ƭ;Γτ=true;Ακ=false;
this.ΔΗ=ȭ;}void ΒЇ(){Ǥ(false,false,false,χ,0);ΑЌ=false;}void Р(Vector3 Њ,float ǝ){Ǥ(true,false,false,Њ,ǝ);}void Ǥ(bool Ј,bool ΔΒ,bool ΔΤ,Vector3 Њ,float ǝ){Ǥ(Ј,ΔΒ,ΔΤ,Њ,Њ-ί,0.0f,ǝ);}void Ǥ(bool Ј,bool ΔΒ,bool ΔΤ,Vector3 Њ,Vector3 ΘЇ,float ΘЈ,float ǝ){ΑЌ=true;this.Ζτ=Ј;
χ=Њ;this.Γϊ=ǝ;this.ΓЅ=ΘЈ;this.Ζα=ΔΒ;this.Ζΰ=ΔΤ;this.Εφ=ΘЇ;Л=Vector3.Distance(Њ,ί);}void Ηλ(){ω.GetBlocksOfType(ſ,Ǒ);}void Ημ(){if(ΑЂ){ύ=2;return;}List<IMyRemoteControl>Ɵ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζέ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζθ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ɵ,Ǒ);ω.GetBlocksOfType(ї,Ǒ);ω.GetBlocksOfType(Ζέ,Ǒ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζθ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=ǣ(ї,pamTag,true);Ησ();Ητ(Ζθ);ź(ї,setLCDFontAndSize,1.4f,false);ź(Ĉ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζμ=ǣ(Ζέ,pamTag,true);if(Ζμ.Count>0) ξ=Ζμ[0];else ξ=null;if(Η==Ζ.Ÿ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.ǐ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ω.GetBlocksOfType(ΔΜ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζλ=ǣ(Ɵ,pamTag,true);if(Ζλ.Count>0) Ɵ=Ζλ;if(Ɵ.Count>0) Φ=Ɵ[0];else{Φ=null;ύ=2;Μ="Remote is missing";return;}ζ=(float) Φ.CubeGrid.WorldVolume.Radius*2;ќ=null;if(Η!=Ζ.Ϋ){Δδ();if(ο.Count>0&&ќ!=null){if(ξ!=null&&(ќ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(ќ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ќ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ην(){ω.GetBlocksOfType(Ȅ,Ǒ);ω.GetBlocksOfType(т,Ǒ);ω.GetBlocksOfType(Ȉ,Ǒ);ω.GetBlocksOfType(Αδ,Ǒ);ω.GetBlocksOfType(ȩ,Ǒ);ω.GetBlocksOfType(Ć,Ǒ);ω.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑЇ,Ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǣ(т,"ConnectorMedium",false);else
т=ǣ(т,"Connector",false);List<IMyShipConnector>Ζι=ǣ(т,pamTag,true);if(Ζι.Count>0) т=Ζι;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(Αδ.Count==0){ύ=1;Μ="Gyros are missing";}if(Ȉ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζν=ǣ(ΑЇ,pamTag,true);
if(Ζν.Count>0) ΑЇ=Ζν;List<IMyLandingGear>Ζκ=ǣ(Ȅ,pamTag,true);if(Ζκ.Count>0) Ȅ=Ζκ;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζη=ǣ(ȩ,pamTag,true);if(Ζη.Count>0) ȩ=Ζη;List<IMyGasTank>Ζξ=ǣ(і,pamTag,true);if(Ζξ.Count>0) і=Ζξ;}void Ηξ(){ω.GetBlocksOfType(ė,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ņ.Clear();for(int i=ė.Count-1;i>=0;i--){if(ΗΨ(ė[i])){ņ.Add(ė[i]);ė.RemoveAt(i);}}}bool ΗΨ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βζ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δδ(){if(Φ==null) return;ќ=null;float Αϊ=0,Αυ=0,Αϋ=0,Αφ=0,Αό=0,Αχ=0;List<IMyTerminalBlock>ΔΖ=ǣ(ο,pamTag,true);bool Θφ=ΔΖ.Count==0;if(ΔΖ.Count>0) ќ=ΔΖ[0];else if(ο.Count>0) ќ=ο[0];int ȁ=0;float ŝ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ќ.WorldMatrix.Forward){if(Θφ){ύ=2;Μ="Mining direction is unclear!";return;}continue;}ȁ++;ŝ=Math.Max(ŝ,Ηώ(ο[i]));Vector3 Љ=ƾ(Φ,ο[i].GetPosition());if(i==0){Αϊ=Љ.X;Αυ=Љ.X;Αϋ=Љ.Y;Αφ=Љ.Y;Αό=Љ.Z;Αχ=Љ.Z;}Αυ=Math.Max(Љ.X,Αυ);Αϊ=Math.Min(Љ.X,Αϊ);
Αφ=Math.Max(Љ.Y,Αφ);Αϋ=Math.Min(Љ.Y,Αϋ);Αχ=Math.Max(Љ.Z,Αχ);Αό=Math.Min(Љ.Z,Αό);}if(ŝ==0) ŝ=drillRadius;bool ȭ=ќ!=null&&ќ.WorldMatrix.Forward==Φ.WorldMatrix.Down;Ǡ=(Αυ-Αϊ)*(1-Ĳ)+ŝ*2;Ǳ=(ȭ?Αχ-Αό:Αφ-Αϋ)*(1-ġ)+ŝ*2;IMyCubeGrid g=Φ.CubeGrid;float ΔΩ=0,ΔΪ=0,ΔΫ=0,Δά=0;
for(int c=0;c<8;c++){Vector3 p=ƾ(Φ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȭ?p.Z:p.Y;if(c==0||p.X<ΔΩ) ΔΩ=p.X;if(c==0||p.X>ΔΪ) ΔΪ=p.X;if(c==0||y<ΔΫ) ΔΫ=y;if(c==0||y>Δά) Δά=y;}ī=ΔΪ-ΔΩ+g.GridSize;Ƣ=Δά-ΔΫ+g.GridSize;}float Ηώ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǣ<T>(List<T>Т,String Ǟ,bool Αζ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βζ(Т[i],Ǟ,Αζ)) έ.Add(Т[i]);return έ;}bool Βζ<T>(T Τ,String Ǟ,bool Αζ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αζ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αζ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ђ<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΔА((IMyTerminalBlock) Т[i],Ј);}void ΔА(IMyTerminalBlock Τ,bool Ј){if(Τ==null) return;String ΗЏ=Ј?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЏ);a.Apply(Τ);}void ǥ(bool Ј){ΔΓ=Ј;if(!ĭ) return;ђ(ΑЇ,Ј);}void Βώ(ChargeMode ƚ){for(int i=0;i<ȩ.Count;i++) ȩ[i].ChargeMode=ƚ;}void ΒЀ(bool ΘЏ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΘЏ;}void Ż(List<IMyLandingGear>Γί,bool ΖΔ){for(int i=0;i<Γί.Count;i++){if(ΖΔ) Γί[i].Lock();if(!ΖΔ) Γί[i].Unlock();}}void ΔБ(bool Ј){if(Εζ==Ј) return;List<IMyShipController>ΑΠ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΠ,Ǒ);if(ΑΠ.Count==0) return;
for(int i=0;i<ΑΠ.Count;i++)
ΑΠ[i].DampenersOverride=Ј;Εζ=Ј;}IMyShipConnector П(MyShipConnectorStatus ь){for(int i=0;i<т.Count;i++){if(!Ƿ(т[i],true)) continue;if(т[i].Status==ь) return т[i];}return null;}void Ĕ(bool ΓА,bool ΑΨ,float ΘΩ,float ΘΓ){if(ξ==null||ο.Count==0) return;Vector3 ΑΟ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ќ.WorldMatrix.Forward) continue;ȁ++;ΑΟ+=ο[i].GetPosition();}ΑΟ=ΑΟ/ȁ;Vector3 ř=ƾ(ξ,ΑΟ);ξ.Enabled=true;ξ.ShowOnHUD=ΓА;ξ.LeftExtend=(ΑΨ?1:й)/2f*Ǡ-ř.X;ξ.RightExtend=(ΑΨ?1:й)/2f*Ǡ+ř.X;ξ.TopExtend=(ΑΨ?1:и)/2f*Ǳ+ř.Y;
ξ.BottomExtend=(ΑΨ?1:и)/2f*Ǳ-ř.Y;ξ.FrontExtend=(ΓА?Ц:ΘΩ)-ř.Z;ξ.BackExtend=ΓА?0:ΘΓ+ζ*0.75f+ř.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ź<T>(List<T>Т,bool ΘЊ,float ΘΧ,bool Θγ){for(int i=0;i<Т.Count;i++){IMyTextSurface Ʀ=null;if(Т[i]is IMyTextSurface) Ʀ=(IMyTextSurface) Т[i];if(Ʀ!=null){Ʀ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЊ) continue;Ʀ.Font="Debug";if(Θγ) continue;Ʀ.FontSize=ΘΧ;}}}bool ǂ(){Vector3D ε;return this.Φ.TryGetPlanetPosition(out ε);}int ΗΞ(Vector3 ΘБ,int Ζβ){if(Ζβ==-1) return 0;double Б=-1;int Ȓ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘБ);if(Б==-1||ό<Б){Ȓ=i;Б=ό;}
}return Math.Sign(Ȓ-Ζβ);}bool ΗΪ(Vector3 ε){List<Vector3>З=new List<Vector3>();for(int i=0;i<Σ.Count;i++) З.Add(Σ[i].Δ);if(Θ.δ&&Σ.Count>=1){Vector3 Ł=new Vector3();Ƕ(Θ,dockDist*ζ,false,out Ł);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){З.Insert(0,Ł);З.Insert(0,Θ.Δ);}
else{З.Add(Ł);З.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.δ&&Σ.Count>=1){Vector3 ΒЎ=new Vector3();Ƕ(Ε,dockDist*ζ,false,out ΒЎ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){З.Insert(0,ΒЎ);З.Insert(0,Ε.Δ);}else{З.Add(ΒЎ);З.Add(Ε.Δ);}}}else{if(ή!=Κ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
З.Insert(0,Ε.Δ);else З.Add(Ε.Δ);}int Ȓ=-1;double Ǩ=-1;for(int i=0;i<З.Count;i++){double ό=Vector3.Distance(З[i],ε);if(ό<Ǩ||Ǩ==-1){Ǩ=ό;Ȓ=i;}}if(З.Count==0) return false;double ΓΥ=Vector3.Distance(З[Ȓ],ε);double Θό=Vector3.Distance(З[Math.Max(0,Ȓ-1)],З[Ȓ])*1.5f;
double Θυ=Vector3.Distance(З[Math.Min(З.Count-1,Ȓ+1)],З[Ȓ])*1.5f;return ΓΥ<Θό||ΓΥ<Θυ||ΓΥ<laneOffset*ζ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηκ(Vector3 v,int ΓΡ){return new Vector3(Math.Round(v.X,ΓΡ),Math.Round(v.Y,ΓΡ),Math.Round(v.Z,ΓΡ));
}float Ķ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΦ=(a*b).Sum;float ΕА=a.Length();float ΕБ=b.Length();if(ΕА==0||ΕБ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΦ/(ΕА*ΕБ)));if(float.IsNaN(έ)) return 0;return έ;}float Ď(Vector3 v,Vector3 ΑЀ,float ΘЋ){float Ŀ=Ķ(v,ΑЀ);
if(ΘЋ>0f) Ŀ*=-1;if(Ŀ>-90f) return Ŀ-90f;else return 180f-(-Ŀ-90f);}double Δκ(float ΘΞ){return(Math.PI/180)*ΘΞ;}bool Βι(double Θξ,double value,double Ατ){return(value>=Θξ&&value<=Ατ);}Vector3 ΗϏ(IMyTerminalBlock Τ,Vector3 Љ){return Vector3D.Transform(Љ,Τ.WorldMatrix);
}Vector3 ƾ(IMyTerminalBlock Τ,Vector3 ΙΧ){return Я(Τ,ΙΧ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΨ){return Vector3D.TransformNormal(ΔΨ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 ē(Vector3 ǫ,Vector3 ώ,Vector3 ΔΨ){MatrixD Γω=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.TransformNormal(ΔΨ,MatrixD.Transpose(Γω));
}Vector3 ΒЈ(Vector3 ǫ,Vector3 ώ,Vector3 Љ){MatrixD Γω=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.Transform(Љ,Γω);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Ж=s.Split('|');return new Vector3(float.Parse(Λ(Ж,0)),float.Parse(Λ(Ж,1)),float.Parse(Λ(Ж,2)));
}String ΗЍ(Ψ Ω){String Ћ=":";String έ=τ(Ω.Δ)+Ћ+τ(Ω.γ)+Ћ+
τ(Ω.β)+Ћ+τ(Ω.υ)+Ћ+τ(Ω.ν);for(int i=0;i<Ω.ǟ.Length;i++)
έ+=Ћ+Math.Round(Ω.ǟ[i],3);return έ;}Ψ Ηω(String s){String[]Ж=s.Split(':');Ψ Ω=new Ψ(
μ(Λ(Ж,0)),μ(Λ(Ж,1)),
μ(Λ(Ж,2)),μ(Λ(Ж,3)),
μ(Λ(Ж,4)));int Β=5;List<float>Ες=new List<float>();while(Β<Ж.Length){String ΒΒ=Λ(Ж,Β);float f=0;if(!float.TryParse(ΒΒ,out f)) break;Ες.Add(f);Β++;}Ω.ǟ=Ες.ToArray();return Ω;}System.Text.StringBuilder š=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θτ){if(Θτ) š.Append('\n');š.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƺ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΖ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Λ(String[]ΘΒ,int Β){String ΒΒ=ΘΒ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΒΒ)) return "";
return ΒΒ;}void Save(){if(ΑЂ||Η==Ζ.р){Storage="";return;}Storage=ΒΝ();}String ΒΝ(){š.Clear();š.Append(DATAREV+";");Ρ(τ(ΑЁ),false);Ρ(τ(Θ.γ));Ρ(τ(Θ.υ));Ρ(τ(Θ.β));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.δ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.γ));Ρ(τ(Ε.β));Ρ(τ(Ε.υ));Ρ(τ(Ε.Ч));Ρ(Ε.δ);Ρ(τ(њ));Ρ(τ(ș));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(Ǭ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(Ǧ);Ρ(Ƞ);Ρ(ȟ);if(Η==Ζ.Ϋ){Ρ((int) ϊ.Ќ);Ρ(ϊ.ǖ);Ρ(ϊ.ȯ);Ρ(ƺ(ϊ.Ǘ));Ρ(ƺ(ϊ.ǚ));Ρ((int) ϋ.Ќ);Ρ(ϋ.ǖ);Ρ(ϋ.ȯ);Ρ(ƺ(ϋ.Ǘ));Ρ(ƺ(ϋ.ǚ));}else{Ρ((int) ȝ);Ρ((int) л);Ρ((int) Н);Ρ((int) у);Ρ((int) Ĥ);Ρ(Ċ);Ρ(ĭ);Ρ(Ő);Ρ(ň);Ρ(й);Ρ(и);Ρ(Ц);Ρ(ū);Ρ(Ū);Ρ(Ĳ);Ρ(ġ);
Ρ(Д);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(ȗ);Ρ(К);Ρ(ĩ);Ρ(ħ);}Ρ(";");for(int i=0;i<ȇ.Count;i++)
Ρ((i>0?"|":"")+ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЍ(Σ[i]),i>0);Ρ(";",false);Ρ(М+"|"+Ё,false);Ρ(";",false);Ρ(ΗΕ(),false);String Χ=š.ToString();š.Clear();return Χ;}с Ηι(){return Ǌ(Storage);}с Ǌ(String Χ){if(Χ=="") return с.Ηα;String[]Ī=Χ.Split(';');if(Λ(Ī,0)!=DATAREV) return с.Βξ;int Β=0;try{String[]Ι=Λ(Ī,1).Split('\n');
ΑЁ=μ(Λ(Ι,Β++));Θ.γ=μ(Λ(Ι,Β++));Θ.υ=μ(Λ(Ι,Β++));Θ.β=μ(Λ(Ι,Β++));Θ.ν=μ(Λ(Ι,Β++));Θ.Δ=μ(Λ(Ι,Β++));Θ.Ч=μ(Λ(Ι,Β++));Θ.δ=bool.Parse(Λ(Ι,Β++));Ε.Δ=μ(Λ(Ι,Β++));Ε.ν=μ(Λ(Ι,Β++));Ε.γ=μ(Λ(Ι,Β++));Ε.β=μ(Λ(Ι,Β++));Ε.υ=μ(Λ(Ι,Β++));Ε.Ч=μ(Λ(Ι,Β++));Ε.δ=bool.Parse(Λ(Ι,Β++));
њ=μ(Λ(Ι,Β++));ș=μ(Λ(Ι,Β++));Ι=Λ(Ī,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Λ(Ι,Β++));ή=(Κ) int.Parse(Λ(Ι,Β++));ц=(Κ) int.Parse(Λ(Ι,Β++));Ǭ=int.Parse(Λ(Ι,Β++));ч=int.Parse(Λ(Ι,Β++));Ы=int.Parse(Λ(Ι,Β++));ш=int.Parse(Λ(Ι,Β++));Ǧ=float.Parse(Λ(Ι,Β++));Ƞ=bool.Parse(Λ(Ι,Β++));
ȟ=bool.Parse(Λ(Ι,Β++));if(Η==Ζ.Ϋ){ϊ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϊ.ǖ=float.Parse(Λ(Ι,Β++));ϊ.ȯ=float.Parse(Λ(Ι,Β++));ϊ.Ǘ=ΑΖ(Λ(Ι,Β++));ϊ.ǚ=ΑΖ(Λ(Ι,Β++));ϋ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϋ.ǖ=float.Parse(Λ(Ι,Β++));ϋ.ȯ=float.Parse(Λ(Ι,Β++));ϋ.Ǘ=ΑΖ(Λ(Ι,Β++));ϋ.ǚ=ΑΖ(Λ(Ι,Β++));}else
{ȝ=(ǒ) int.Parse(Λ(Ι,Β++));л=(д) int.Parse(Λ(Ι,Β++));Н=(Ѐ) int.Parse(Λ(Ι,Β++));у=(я) int.Parse(Λ(Ι,Β++));Ĥ=(ǒ) int.Parse(Λ(Ι,Β++));Ċ=bool.Parse(Λ(Ι,Β++));ĭ=bool.Parse(Λ(Ι,Β++));Ő=bool.Parse(Λ(Ι,Β++));ň=bool.Parse(Λ(Ι,Β++));й=int.Parse(Λ(Ι,Β++));и=int.Parse(Λ(Ι,Β++));Ц=int.Parse(Λ(Ι,Β++));
ū=float.Parse(Λ(Ι,Β++));Ū=float.Parse(Λ(Ι,Β++));Ĳ=float.Parse(Λ(Ι,Β++));ġ=float.Parse(Λ(Ι,Β++));Д=int.Parse(Λ(Ι,Β++));б=int.Parse(Λ(Ι,Β++));Ш=int.Parse(Λ(Ι,Β++));Щ=int.Parse(Λ(Ι,Β++));ȗ=int.Parse(Λ(Ι,Β++));К=int.Parse(Λ(Ι,Β++));ĩ=int.Parse(Λ(Ι,Β++));ħ=float.Parse(Λ(Ι,Β++));
}Ι=Λ(Ī,3).Replace("\n","").Split('|');ȇ=Ι.ToList();Ι=Λ(Ī,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηω(Λ(Ι,i)));Ι=Λ(Ī,5).Split('|');М=Λ(Ι,0);if(!int.TryParse(Λ(Ι,1),out Ё)) Ё=-1;if(М.Length!=Д*б){М="";Ё=-1;}ΗΔ(Λ(Ī,6));}catch{return с.Δξ;}Ţ=ц;ψ();return с.ΑΓ;}const int Δβ=64000;IMyTerminalBlock Δο(){List<IMyTerminalBlock>ń=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ń,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ń.Count>0?ń[0]:null;}void ΗΘ(){IMyTerminalBlock Τ=Δο();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΝ();if(Χ.Length>Δβ){Μ="Export too large: "+Χ.Length+" chars, max "+Δβ;return;}Τ.CustomData=Χ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΤ(bool Γξ){IMyTerminalBlock Τ=Δο();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΥ(Τ.CustomData,Γξ);}void ΗΥ(String Χ,bool Γξ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΟ=Χ.IndexOf('\n');Χ=ΖΟ<0?"":Χ.Substring(ΖΟ+1);}Χ=Χ.Trim();if(Χ==""){Μ="Import: no PAM data in "+dataTag;return;}ψ();String ΕΧ=ΒΝ();с έ=Ǌ(Χ);if(έ==с.ΑΓ&&Γξ){Ψ Ą=new Ψ(Ε);
Vector3 Θρ=њ,Θπ=ș;int w=й,h=и,d=Ц;ǒ ΔΔ=ȝ;я ΓΣ=у;float Θω=Ĳ,Θψ=ġ;Ǌ(ΕΧ);Ε=Ą;њ=Θρ;ș=Θπ;й=w;и=h;Ц=d;ȝ=ΔΔ;у=ΓΣ;Ĳ=Θω;ġ=Θψ;ή=Κ.ĺ;Ž(true,true);ή=Κ.ĺ;ц=Κ.ĺ;Μ="Imported job, run CONT";return;}if(έ==с.ΑΓ){ƛ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}Ǌ(ΕΧ);Μ=έ==с.Βξ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ησ(){Ɖ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γμ=false;if(Χ=="INSTRUCTIONS"){Γμ=true;ΑΧ=true;}if(Χ=="DEBUG") Γμ=true;if(Γμ){Ɖ.Add(ї[i]);ї.RemoveAt(i);}
}ź(Ɖ,false,1,false);}void Ητ(List<IMyTerminalBlock>ń){Ĉ.Clear();for(int i=0;i<ń.Count;i++){IMyTerminalBlock Τ=ń[i];try{String ΖΧ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΧ);int Ť=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΧ.Length,1),out Ť)) continue;
if(Ť==-1) continue;Ť--;IMyTextSurfaceProvider ΖΩ=(IMyTextSurfaceProvider) Τ;if(Ť<ΖΩ.SurfaceCount&&Ť>=0)
Ĉ.Add(ΖΩ.GetSurface(Ť));}catch{}}}String Αψ="";void ΔЌ(){String Γψ="";String ΓΪ="";Αψ=Δα(false);Γψ+=Αψ;ΓΪ+=Αψ;ΓΪ+=ΗΜ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γψ);for(int i=0;i<Ĉ.Count;i++) Ĉ[i].WriteText(Γψ);Echo(ΓΪ);for(int i=0;i<Ɖ.Count;i++){IMyTextPanel ΓЄ=Ɖ[i];
String Χ=ΓЄ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЄ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЄ.WriteText(ΗΝ());}}string ΗΝ(){String Γ="";try{float ΖΗ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ń*ΖΗ)+" / "+(int)(ŕ*ΖΗ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ń*100f,1)+"% / "+Math.Round(ŕ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ƃ,2)+"ms / "+Math.Round(Γό,2)+"ms \n";}catch{}for(int i=0;i<Ț.Count;i++)
Γ+=""+Ț.Keys.ElementAt(i)+" = "+Math.Round(Ț.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ț.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΠ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ź(ї,setLCDFontAndSize,0.9f,false);ź(Ĉ,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+Ȗ.ToString());for(int i=0;i<Ĉ.Count;i++) Ĉ[i].WriteText(Γ+Ȗ.ToString());}void Ǻ(){if(!ΑΧ) return;try{ΓЈ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String φ){if(!ΑΧ) return;if(ΓЈ==0) return;try{float ΑБ=(Runtime.CurrentInstructionCount-ΓЈ)/Runtime.MaxInstructionCount*100;if(!Ț.ContainsKey(φ))
Ț.Add(φ,new float[]{ΑБ,ΑБ});else{Ț[φ][0]=ΑБ;Ț[φ][1]=Math.Max(ΑБ,Ț[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЃ(String ȱ){if(!Ņ) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=Ŭ(ΓЏ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=Βέ(ί.X)+""+Ћ;ρ+=Βέ(ί.Y)+Ћ;ρ+=Βέ(ί.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=Βί(ц)+(ц==Κ.Ϗ?" "+Math.Round(Ɣ,1)+"%":"")+Ћ;else ρ+=Βί(ц)+Ћ;ρ+=Ŭ(Μ)+Ћ;ρ+=љ+""+Ћ;ρ+=Ŭ(Αψ)+Ћ;ρ+=Ň+Ћ;ρ+=Ŗ+Ћ;for(int i=0;i<Ǚ.Count;i++){if(Ǚ[i].œ!=И.Ų) continue;ρ+=Ŭ(Ǚ[i].φ)+"/"+Ŭ(Ǚ[i].ς)+"/"+Ǚ[i].ǔ+Ћ;}Ηπ(ρ,ȱ);}String Ŭ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηπ(String ρ,String ȱ){Ηρ(ρ,BROADCAST_SELF_ID,ȱ);}void Ηρ(String ρ,String Њ,String ȱ){try{if(!Ņ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δς()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΓΗ+BROADCAST_SEP+ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȗ=e;}
}String Δς(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЂ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ů(ref String Χ,out string Εσ,Char Ћ){int Β=Χ.IndexOf(Ћ);Εσ="";if(Β<0) return false;Εσ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηβ(ref String ρ,out String ơ,out String ȱ)
{ơ="";ȱ="";if(!Ņ) return false;String Ǟ="";if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЂ(Ǟ)) return false;if(!Ů(ref ρ,out ơ,BROADCAST_SEP)) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δς()&&(Ǟ!="*"&&Η!=Ζ.р))) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΗ)) return false;
if(!Ů(ref ρ,out ȱ,BROADCAST_SEP)) return false;return true;}void ΗΧ(){bool Ƈ=true;if(Ευ){Ευ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String ƒ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ƒ)
Δώ(ƒ);String[]Ι=Me.CustomData.Split('\n');Ņ=bool.Parse(Βψ(ΒΫ(Ι,"Enable_Broadcast",ref Ƈ)));bool ΔΦ=false;bool Εκ=true;if(Ņ){if(Η!=Ζ.р)
ΓЏ=Βψ(ΒΫ(Ι,"Ship_Name",ref Ƈ)).Replace(BROADCAST_SELF_ID,"");ΓΗ=Βψ(ΒΫ(Ι,"Broadcast_Channel",ref Ƈ)).ToLower();Εκ=false;if(ȑ==null){ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȑ.SetMessageCallback("");}List<IMyRadioAntenna>ŀ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ŀ);bool ΒЍ=false;
for(int i=0;i<ŀ.Count;i++)
if(ŀ[i].EnableBroadcasting&&ŀ[i].Enabled){ΒЍ=true;break;}if(ŀ.Count==0) Μ="No Antenna found";else if(!ΒЍ) Μ="Antenna not ready";ΔΦ=ŀ.Count==0||!ΒЍ;if(ΕΟ&&!ΔΦ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΟ=ΔΦ;if(Εκ){if(ȑ!=null) this.IGC.DisableBroadcastListener(ȑ);ȑ=null;}}catch{Ƈ=false;}if(!Ƈ){Μ="Reset custom data";Δώ(ƒ);}}void Δώ(String ƒ){String Χ=ƒ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Χ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βψ(String Ħ){int Β=Ħ.IndexOf("//");if(Β!=-1) Ħ=Ħ.Substring(0,Β);String[]Ж=Ħ.Split('=');if(Ж.Length<=1) return "";return Ж[1].Trim();}String ΒΫ(String[]Ι,String ĥ,ref bool Ɯ){foreach(String Ħ in Ι)
if(Ħ.StartsWith(ĥ)) return Ħ;Ɯ=false;return "";}String Ισ="";ļ Ιυ=null;DateTime Ιφ=DateTime.MinValue;List<ļ>Ιτ=new List<ļ>();const float Ιή=20f;String ΔЏ(int ΔΥ,String Γ,int Ζά,ref int ț){String[]Ι=Γ.Split('\n');if(Ζά>=ț+ΔΥ-1) ț++;ț=Math.Min(Ι.Count()-1-ΔΥ,ț);if(Ζά<ț+1) ț--;
ț=Math.Max(0,ț);String έ="";for(int i=0;i<ΔΥ;i++){int ΖΑ=i+ț;if(ΖΑ>=Ι.Count()) break;έ+=Ι[ΖΑ]+"\n";}return έ;}String Βή(int Ơ){if(Ơ>=60*60) return Math.Round(Ơ/(60f*60f),1)+" h";if(Ơ>=60) return Math.Round(Ơ/60f,1)+" min";return ""+Ơ+" s";}string Βέ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>Ś=new List<long>();bool Ŝ=false;int ΓΜ=0;int ΓΚ=0;DateTime ΕΪ=DateTime.Now;String ΓΖ="";void Ηο(){ǽ.Clear();Ś.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!Ś.Contains(ǽ[i].CubeGrid.EntityId)) Ś.Add(ǽ[i].CubeGrid.EntityId);}if(!Ś.Contains(Me.CubeGrid.EntityId)) Ś.Add(Me.CubeGrid.EntityId);}bool ΔЁ(Α s){return s==Α.ȋ||s==Α.Ю||s==Α.Ѕ||
s==Α.ȥ||s==Α.ķ||s==Α.ů||s==Α.Ƽ||
(s==Α.Ǣ&&ћ!=null);}void Ƹ(){Ŝ=false;ΓΜ=0;ΓΖ="";}void ΗЇ(){if(!obstacleBrake||ǽ.Count==0||!ΔЁ(Υ)){Ƹ();return;}Vector3 Ƒ=χ+ĉ;Vector3 ǯ=Ƒ-ί;float Ǯ=ǯ.Length();if(Ǯ<1f){Ƹ();return;}Vector3 κ=ǯ/Ǯ;float ΖΕ=Ȝ*obstacleLookAhead+ζ;Vector3 ΘϏ=Ƒ+κ*(ζ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΓΚ=(ΓΚ+1)%ǽ.Count;IMyCameraBlock ƅ=ǽ[ΓΚ];if(!ƅ.IsFunctional||!ƅ.IsWorking) continue;Vector3 Εά=ƅ.GetPosition();Vector3 Αί=ΘϏ;if(Vector3.Distance(Εά,Αί)>ΖΕ) Αί=Εά+κ*ΖΕ;if(!ƅ.CanScan(Αί)) continue;MyDetectedEntityInfo Ɠ=ƅ.Raycast(Αί);bool Θχ=!Ɠ.IsEmpty()&&!Ś.Contains(Ɠ.EntityId)&&!Ηέ(Ɠ.EntityId)&&
Ɠ.Type!=MyDetectedEntityType.FloatingObject;if(Θχ){if(!Ŝ) ΕΪ=DateTime.Now;Ŝ=true;ΓΜ=0;ΓΖ=Ɠ.Type.ToString();}else if(Ŝ&&++ΓΜ>=3) Ƹ();break;}if(Ŝ&&(DateTime.Now-ΕΪ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΓΖ;}const String ΕΙ="[PAMTFC]";const float Ηό=5f;const float ΒΪ=2f;class ȧ{public long λ;public Vector3 ε,İ;public Vector3 ō;public float Ει;public String Ą="";public int Γθ=-1;public bool ΓЉ;public bool Αε;public float Ġ,ŉ;public long ά=0;public int Ƌ=0;public DateTime ΑЎ;
}IMyBroadcastListener ΑА=null;long ъ=0;List<ȧ>м=new List<ȧ>();Vector3 ĉ=new Vector3();float э=-1;Vector3 ΔΞ=new Vector3();bool ΑЏ=false;DateTime ΔΟ=DateTime.Now;const float ΕΘ=15f;const float Ηϋ=10f;int Ζψ=0;String М="";int Ё=-1;bool ΓΟ=false,ΑΡ=false;Vector3 Εΰ=new Vector3();
String ŵ(){if(ή==Κ.з||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Д+"|"+б+"|"+(int) Ĥ+"|"+Math.Round(Ǡ,1)+"|"+Math.Round(Ǳ,1);}void ΗЊ(){if(!trafficEnabled) return;if(ΑА==null) ΑА=IGC.RegisterBroadcastListener(ΕΙ);ъ=Me.CubeGrid.EntityId;int Ě=30;while(ΑА.HasPendingMessage&&Ě-->0){String Χ=ΑА.AcceptMessage().Data as String;if(Χ!=null) Ηε(Χ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΑЎ).TotalSeconds>Ηό) м.RemoveAt(i);if(ΑЋ==null) ΑЋ=IGC.RegisterBroadcastListener(Ηϊ);Ě=5;while(ΑЋ.HasPendingMessage&&Ě-->0){String Χ=ΑЋ.AcceptMessage().Data as String;if(Χ!=null) try{Ηζ(Χ);}catch{}}if(ΑΣ==null) ΑΣ=IGC.RegisterBroadcastListener(Ηή);
Ě=10;while(ΑΣ.HasPendingMessage&&Ě-->0){String Χ=ΑΣ.AcceptMessage().Data as String;if(Χ!=null) try{Ηγ(Χ);}catch{}}if(++Ζψ<3) return;Ζψ=0;IGC.SendBroadcastMessage(ΕΙ,ъ+";"+τ(ί)+";"+
τ(Φ.GetShipVelocities().LinearVelocity)+";"+ζ+";"+ŵ()+";"+
(ή==Κ.Ϗ?Ё:-1)+";"+М+";"+ΖЍ()+","+Δμ()+";"+τ(ΔΞ)+
";"+(ΑΪ?1:0)+";"+(Ȳ&&ή==Κ.Ϗ?1:0)+","+ī+","+Ƣ+","+(Δϋ()?1:0));}void Ηε(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ъ) return;ȧ Ї=null;for(int i=0;i<м.Count;i++) if(м[i].λ==λ) Ї=м[i];if(Ї==null){Ї=new ȧ();Ї.λ=λ;м.Add(Ї);}
Ї.ε=μ(f[1]);Ї.İ=μ(f[2]);Ї.Ει=float.Parse(f[3]);Ї.Ą=f[4];Ї.Γθ=int.Parse(f[5]);Ї.ΑЎ=DateTime.Now;if(Ї.Ą!=""&&Ї.Ą==ŵ()) ΔЉ(f[6]);Ї.ά=0;Ї.Ƌ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.Ƌ=int.Parse(d[1]);}Ї.ō=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЉ=f.Length>=10&&f[9]=="1";Ї.Αε=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.Ġ=float.Parse(w[1]);Ї.ŉ=float.Parse(w[2]);Ї.Αε=w[3]=="1";}}}catch{}}bool Ηέ(long λ){for(int i=0;i<м.Count;i++) if(м[i].λ==λ) return true;return false;}Vector3 Δφ(int Β,int Θϊ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΞ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θϊ;Vector3 ώ=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.β;Vector3 Ь=Vector3.Cross(ΙΞ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ό=laneOffset*ζ;float ΘΨ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΨ/(ό*4));}Vector3 Ζχ=new Vector3();bool Δϋ(){return ή==Κ.Ϗ&&(Υ==Α.ȋ||Υ==Α.Ĺ||Υ==Α.Ǹ);}bool Ηά(Α s){return s==Α.Ǣ||s==Α.Č||s==Α.Đ||s==Α.Ź||
s==Α.Ǔ||s==Α.Ǹ||s==Α.ȡ;}void ΗЋ(){ĉ=new Vector3();э=-1;ΔΞ=new Vector3();bool Εψ=ΔЁ(Υ);if(!trafficEnabled||(!Εψ&&!Ηά(Υ))){ΑЏ=false;return;}Ζχ=Φ.GetShipVelocities().LinearVelocity;ΖЎ(Φ.GetNaturalGravity(),!Εψ);ΖЁ();}IMyBroadcastListener ΑЋ=null;float Ŧ=-1;DateTime Ζς=DateTime.MinValue;
void Ηζ(String Χ){if(!Χ.StartsWith("C;")) return;Ζς=DateTime.Now;Ŧ=-1;String ƙ=ъ+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƙ)) Ŧ=float.Parse(e[i].Substring(ƙ.Length));}void ΖЁ(){if(Ŧ<0||(DateTime.Now-Ζς).TotalSeconds>1.5) return;
э=э<0?Ŧ:Math.Min(э,Ŧ);if(Ŧ<0.5f) Μ="Traffic: giving way (controller)";}void ΖЎ(Vector3 ν,bool ΖΦ=false){ĉ=new Vector3();э=-1;Vector3 ǯ=χ-ί;float Ǯ=ǯ.Length();bool Γζ=Ǯ>=1f;Vector3 κ=Γζ?ǯ/Ǯ:new Vector3();ΔΞ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-ƍ;
Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-Ɨ:Vector3.Normalize(Ь);ȧ Αξ=null;bool Εϊ=Υ==Α.Ѕ&&!ȏ(1);for(int i=0;i<м.Count;i++){ȧ α=м[i];if(α.Ƌ==3&&α.İ.LengthSquared()<0.25f) continue;Vector3 ε=α.ε+α.İ*(float)(DateTime.Now-α.ΑЎ).TotalSeconds;Vector3 Ş=ε-ί;
float є=(ζ+α.Ει)*0.5f;bool Ζί=α.Αε&&Δϋ()&&α.Ġ>0&&ī>0&&α.Ą==ŵ();if(Ζί) є=Math.Max(ī+α.Ġ,Ƣ+α.ŉ)*0.5f;float ƀ=Γζ?Vector3.Dot(Ş,κ):0;if(ƀ>0&&(Ş-κ*ƀ).Length()<=є*1.2f){float Αύ=Vector3.Dot(α.İ,κ);bool ΕЀ=α.ō.LengthSquared()>0.25f;bool ΕЁ=Αύ<-1f||(ΕЀ&&Vector3.Dot(α.ō,κ)<-0.5f);
if(!ΖΦ){if(α.ΓЉ){if(!Εϊ){ĉ=Ь*є*1.3f;continue;}float ΖΜ=Vector3.Distance(ί,χ),ΕЌ=Vector3.Distance(ε,χ);if(ΖΜ<ΕЌ||(ΖΜ==ΕЌ&&ъ<α.λ)) continue;}if(ΕЁ){if(ƀ<(Ȝ-Αύ)*6f+є*4f) ĉ=Ь*є*1.3f;continue;}if(ΕЀ&&ъ<α.λ){Vector3 ΕΦ=-Ş;float ΖΣ=Vector3.Dot(ΕΦ,α.ō);
if(ΖΣ>0&&(ΕΦ-α.ō*ΖΣ).Length()<=є*1.2f){ĉ=Ь*є*1.3f;continue;}}}float Εω=ƀ-є-(Ζί?1f:followGap*ζ);float Αν=Math.Max(0,Αύ)+Math.Max(0,Εω)/ΒΪ;if(ΕЁ) Αν=Math.Max(0,Εω+Math.Min(0,Αύ)*ΒΪ*2)/ΒΪ;if(э<0||Αν<э){э=Αν;Αξ=α;}continue;}if(α.İ.LengthSquared()<1f) continue;
if(Γζ&&(Ş-κ*ƀ).Length()<=є*1.2f) continue;Vector3 ΓЎ=α.İ-Ζχ;float ΖΪ=ΓЎ.LengthSquared();if(ΖΪ<0.25f) continue;float t=-Vector3.Dot(Ş,ΓЎ)/ΖΪ;if(t<=0||t>5f||(Ş+ΓЎ*t).Length()>=є*1.1f) continue;if(ъ>α.λ||t<2f){э=0;Αξ=α;}}bool Θί=э>=0&&э<0.5f;
if(!Θί||ΖΦ||(Αξ!=null&&Αξ.ΓЉ&&Εϊ)){ΑЏ=false;return;}if(!ΑЏ){ΑЏ=true;ΔΟ=DateTime.Now;}double ΕЂ=(DateTime.Now-ΔΟ).TotalSeconds;if(ΕЂ>ΕΘ+Ηϋ) ΔΟ=DateTime.Now;else if(ΕЂ>ΕΘ){э=-1;ĉ=Ь*ζ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>ȃ=new List<int>();
String ΕЄ="";void ΒΩ(){String ĥ=Д+"|"+б+"|"+(int) Ĥ;if(ĥ==ΕЄ) return;ΕЄ=ĥ;ΖЄ(Д,б,Ĥ,ȃ);}void ΖЄ(int w,int h,ǒ ΔΔ,List<int>ΓΛ){ΓΛ.Clear();if(ΔΔ==ǒ.ΑΔ){for(int k=0;k<w*h;k++){int ş=k/w;ΓΛ.Add(ş*w+(ş%2==0?k-ş*w:w-1-(k-ş*w)));}return;}int ΕϏ=(int) Math.Ceiling(w/2f),Εώ=(int) Math.Ceiling(h/2f);
int ΘΫ=w/2,ΘΪ=h/2;int x=0,y=0,ΓΨ=0,ΓΩ=-1,Ατ=Math.Max(w,h);for(int i=0;i<Ατ*Ατ;i++){if(-ΕϏ<x&&x<=ΘΫ&&-Εώ<y&&y<=ΘΪ) ΓΛ.Add((y-1+Εώ)*w+(x-1+ΕϏ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΨ;ΓΨ=-ΓΩ;ΓΩ=t;}x+=ΓΨ;y+=ΓΩ;
}}bool ΗΣ(int a,int b,int w,float Ζϊ,float Ζσ,float Ġ,float ŉ){return a==b||(Math.Abs(a%w-b%w)*Ζϊ<Ġ+0.5f&&Math.Abs(a/w-b/w)*Ζσ<ŉ+0.5f);}bool ΒΨ(){int n=Math.Max(0,Д*б);if(М.Length==n) return false;М=new String('0',n);Ё=-1;return true;}void ΔЉ(String α)
{ΒΨ();if(α.Length!=М.Length) return;char[]h=М.ToCharArray();for(int i=0;i<h.Length;i++) if(α[i]=='1') h[i]='1';М=new String(h);}bool ǁ(int Ǿ){return Ǿ>=0&&Ǿ<М.Length&&М[Ǿ]=='1';}void ΒϏ(int Ǿ,bool ф){if(Ǿ<0||Ǿ>=М.Length) return;char[]h=М.ToCharArray();h[Ǿ]=ф?'1':'0';
М=new String(h);}bool Βη(int Ǿ,bool Θε){if(!trafficEnabled) return false;String ĥ=ŵ();for(int i=0;i<м.Count;i++){ȧ t=м[i];if(t.Ą!=ĥ||t.Γθ<0||(Θε&&t.λ>ъ)) continue;if(ΗΣ(Ǿ,t.Γθ,Д,Ǡ,Ǳ,ī,Ƣ)) return true;}return false;}int ΖЌ(int Β){int n=Д*б;if(n<1) return Β;ΒΩ();
if(ΒΨ()) for(int k=0;k<Β&&k<n;k++) ΒϏ(ȃ[k],true);if(Ё>=0&&(Β>=n||ȃ[Β]!=Ё)){ΒϏ(Ё,true);Ё=-1;}if(ΔЃ()){if(ǩ>=0&&!ǁ(ǩ)){Ȳ=false;Ё=ǩ;return ȃ.IndexOf(ǩ);}if(ǩ==-2){Ё=-1;Ȳ=М.IndexOf('0')>=0;return Ȳ?-2:n;}if(ǩ==-1&&Ё>=0&&!ǁ(Ё)){Ȳ=false;return Β;}
Ё=-1;if(!Ȳ){Ȳ=true;Εβ=DateTime.Now;}if((DateTime.Now-Εβ).TotalSeconds<5) return-1;}Ȳ=false;if(Ё>=0&&!ǁ(Ё)&&!Βη(Ё,true)) return Β;Ё=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(ǁ(c)||Βη(c,false)) continue;Ё=c;return k;}return М.IndexOf('0')>=0?-2:n;}const String Ηή="[PAMJOB]";
const String Ηϊ="[PAMTCC]";IMyBroadcastListener ΑΣ=null;DateTime Εα=DateTime.MinValue;DateTime Εβ=DateTime.Now;int ǩ=-1;bool Ȳ=false;bool ΔЃ(){return trafficEnabled&&ǩ!=-3&&(DateTime.Now-Εα).TotalSeconds<3;}void Ηγ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ŵ()) return;
Εα=DateTime.Now;ΔЉ(f[2]);ǩ=-3;String ƙ=ъ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƙ)) ǩ=int.Parse(e[i].Substring(ƙ.Length));}Ľ Ης(int Β){ΒΩ();if(Β<0||Β>=ȃ.Count) return Ľ.Δί;Ш=ȃ[Β]%Д;Щ=ȃ[Β]/Д;return Ľ.Δπ;}class ё{public long λ;public Vector3 ε,В,ώ;
}List<ё>Ў=new List<ё>();bool Ȭ=false;Vector3 Αά,ΑΫ,Αέ,Αή;long х=0;long ȅ=0;Ψ ћ=null;bool ƌ=false;Ψ ΒΥ(Vector3 ε,Vector3 В,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ε,В,ώ);Ψ w=new Ψ(Θ);w.γ=Vector3D.TransformNormal(Αά,m);w.β=Vector3D.TransformNormal(ΑΫ,m);w.υ=Vector3D.TransformNormal(Αέ,m);
w.Δ=ε+(Vector3) Vector3D.TransformNormal(Αή,m);w.δ=true;return w;}int Δμ(){Α s=Υ;if(s==Α.ж) return П(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Đ||s==Α.ǻ||s==Α.ď||s==Α.ΑΙ||
s==Α.ƻ||s==Α.Ǔ) return 3;if(s==Α.Ǣ||s==Α.Č||s==Α.Ź) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЍ(){if(Δμ()==0) return 0;if(ȏ(1)&&ţ!=0) return ţ;return ћ!=null?ȅ:х;}void ΔІ(){if(Η==Ζ.Ϋ||!Θ.δ) return;IMyShipConnector Й=П(MyShipConnectorStatus.Connected);if(Й==null||Й.OtherConnector==null) return;
IMyShipConnector α=Й.OtherConnector;Vector3I Θΰ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(Й.Position==Θΰ){MatrixD ŏ=MatrixD.Transpose(α.WorldMatrix);Αά=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,ŏ);ΑΫ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,ŏ);Αέ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,ŏ);
Αή=Vector3D.TransformNormal(Й.GetPosition()-α.GetPosition(),ŏ);Ȭ=true;}if(Vector3.Distance(Й.GetPosition(),Θ.Δ)<5) х=α.EntityId;List<IMyShipConnector>ğ=new List<IMyShipConnector>();ω.GetBlocksOfType(ğ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ğ.Count>0){Ў.Clear();for(int i=0;i<ğ.Count;i++){ё p=new ё();p.λ=ğ[i].EntityId;p.ε=ğ[i].GetPosition();p.В=ğ[i].WorldMatrix.Forward;p.ώ=ğ[i].WorldMatrix.Up;Ў.Add(p);}}ћ=null;ȅ=0;if(α.EntityId==х||!Ȭ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==α.EntityId){ȅ=Ў[i].λ;ћ=ΒΥ(Ў[i].ε,Ў[i].В,Ў[i].ώ);}}bool Βκ(long λ,int ΖΝ){if(λ==0) return false;for(int i=0;i<м.Count;i++){ȧ t=м[i];if(t.ά==λ&&(t.Ƌ>ΖΝ||(t.Ƌ==ΖΝ&&t.λ<ъ))) return true;}return false;}bool ΒΟ(int Αώ){if(Η==Ζ.Ϋ||!trafficEnabled){ћ=null;ȅ=0;return true;}
long θ=ћ!=null?ȅ:х;if(!Βκ(θ,Αώ)) return true;if(!Βκ(х,Αώ)){ћ=null;ȅ=0;return true;}if(!Ȭ) return false;ё ę=null;float ΕΩ=0;for(int i=0;i<Ў.Count;i++){ё p=Ў[i];if(p.λ==х||Βκ(p.λ,Αώ)) continue;float d=Vector3.Distance(p.ε,Θ.Δ);if(ę==null||d<ΕΩ){ę=p;ΕΩ=d;}
}if(ę==null) return false;ȅ=ę.λ;ћ=ΒΥ(ę.ε,ę.В,ę.ώ);return true;}String ΗΕ(){String s=(Ȭ?"1":"0")+":"+τ(Αά)+":"+τ(ΑΫ)+":"+
τ(Αέ)+":"+τ(Αή)+":"+х;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].ε)+","+τ(Ў[i].В)+","+τ(Ў[i].ώ);return s;}void ΗΔ(String s){Ў.Clear();Ȭ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;Ȭ=f[0]=="1";Αά=μ(f[1]);ΑΫ=μ(f[2]);Αέ=μ(f[3]);Αή=μ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ё p=new ё();if(!long.TryParse(e[0],out p.λ)) continue;p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Ў.Add(p);}}const String ΒΡ="[PAMDCK]";const float Ιέ=120f;class Δν{public long Ї;public int ƕ;public long Γι;public bool Ιξ;public DateTime ΙЅ,Θζ;
}IMyBroadcastListener ΙЂ=null;List<Δν>ΙЃ=new List<Δν>();List<IMyShipConnector>ΚΒ=new List<IMyShipConnector>();List<IMySensorBlock>ΚΞ=new List<IMySensorBlock>();List<IMyCameraBlock>ΙЊ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΚΝ=new List<MyDetectedEntityInfo>();long Ιρ=0;
int Ιπ=0;long Ιο=0;DateTime Ις=DateTime.Now;String ΙЁ="";int ΙЄ=0;IMyCubeGrid ΚΓ=null;IMyBroadcastListener ΑЉ=null;int Ȃ=0;DateTime Εν=DateTime.Now;DateTime Ζγ=DateTime.MinValue;long Ζδ=0;int Ζε=0;long ţ=0;bool Ư=false;int ΓΧ=0;List<Vector3>Ʊ=new List<Vector3>();List<float>Δέ=new List<float>();
bool ΑΪ=false;bool ΑΩ=false;Vector3 Εμ=new Vector3();bool ż(){return Ȃ!=0&&(DateTime.Now-Ζγ).TotalSeconds<3;}bool ȏ(int ƕ){return ż()&&Ζδ==ъ&&Ζε==ƕ;}bool ΕΒ(){return ż()||(DateTime.Now-Εν).TotalSeconds<3;}void Βύ(int ƕ){if(Ȃ==ƕ) return;Ȃ=ƕ;Εν=DateTime.Now;ΓΧ=3;
}void Βό(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΡ,"D;"+ъ);Ȃ=0;Ư=false;}void ΗЂ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑЉ==null) ΑЉ=IGC.RegisterBroadcastListener(ΒΡ);int Ě=20;while(ΑЉ.HasPendingMessage&&Ě-->0){String Χ=ΑЉ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;
try{Ηδ(Χ);}catch{}}if(Υ!=Α.Ѕ){ΑΪ=false;ΑΩ=false;}if(Ȃ==1&&П(MyShipConnectorStatus.Connected)!=null) Βό();if(Ȃ==2&&!Ư&&Υ!=Α.Ǔ&&
Vector3.Distance(ί,е().Δ)>ζ*3) Βό();if(Ȃ!=0&&++ΓΧ>=3){ΓΧ=0;IGC.SendBroadcastMessage(ΒΡ,"R;"+ъ+";"+Ȃ+";"+х+";"+(Ȭ?1:0));}}void Ηδ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Εχ=false;String[]ΕΛ=f[5].Split(',');for(int i=0;i<ΕΛ.Length;i++) if(ΕΛ[i]==ъ.ToString()) Εχ=true;
if(!Εχ) return;Ζγ=DateTime.Now;Ζδ=long.Parse(f[2]);Ζε=int.Parse(f[3]);ţ=long.Parse(f[4]);Ʊ.Clear();Δέ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʊ.Add(μ(e[0]));Δέ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εο=f[6].Split(':');List<ё>Т=new List<ё>();for(int i=0;i<Εο.Length;i++){String[]e=Εο[i].Split(',');if(e.Length<4) continue;ё p=new ё();p.λ=long.Parse(e[0]);p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖЊ(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;
Βύ(1);if(!ΕΒ()) return true;if(!ȏ(1)){Μ="Waiting for dock clearance";return false;}ΒΘ();return true;}bool ΒΘ(){ƌ=false;ё Ĩ=null;if(ţ!=0&&ţ!=х&&Ȭ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==ţ) Ĩ=Ў[i];long ΒΔ=Ĩ==null?0:Ĩ.λ;if(ΒΔ==ȅ&&(ΒΔ==0)==(ћ==null)) return false;ȅ=ΒΔ;ћ=Ĩ==null?null:ΒΥ(Ĩ.ε,Ĩ.В,Ĩ.ώ);return true;}bool ĸ(Vector3 p){for(int i=0;i<Ʊ.Count;i++) if(Vector3.Distance(p,Ʊ[i])<Δέ[i]+ζ*0.75f) return true;
return false;}bool ΗΡ(Vector3 ƈ,Vector3 Ř){ΑΪ=false;if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return false;if(Ȃ!=1&&Vector3.Distance(ί,Θ.Δ)>dockRequestDistance) return false;Βύ(1);if(!ż()||ȏ(1)||Ʊ.Count==0) return false;if(!ĸ(ƈ)&&!ĸ(Ř)&&!ĸ(ί)) return false;ΑΪ=true;return true;
}bool Δζ(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;Βύ(2);if(!ΕΒ()) return true;if(!ȏ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float Ιγ=600f;class ΔЅ{public long λ;public int Ǿ=-1;public bool ΒΔ;public float Ġ,ŉ;public DateTime ΙЍ;}class ΔЄ{public String ĥ;
public int w,h;public float Ζϊ,Ζσ;public String ф;public List<int>ΙЌ=new List<int>();public List<ΔЅ>ΙЏ=new List<ΔЅ>();}List<ΔЄ>ΙЉ=new List<ΔЄ>();IMyBroadcastListener ΙЈ=null;int ΙЇ=0;const float Ιν=8f;class ΕΗ{public long λ;public Vector3 ε,İ,ō;public float ΙϏ,Ġ,ŉ,Αν=-1;public bool Αε;
public String Ą="";public int Αώ;public DateTime ΑЎ;}class ΕΖ{public long a,b,ΚΜ;public DateTime Θζ;}List<ΕΗ>ΚΖ=new List<ΕΗ>();List<ΕΖ>ΚΔ=new List<ΕΖ>();int ΚΕ=0;void ΗΑ(string ѓ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}