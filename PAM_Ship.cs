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
public enum Ζ{ΑΕ,Ÿ,Ǐ,р,Ϋ}public enum Α{ж,ȋ,ĸ,Ǹ,đ,Ѕ,Ю,
ȥ,Ķ,ǡ,Ď,Ċ,ȡ,
Ź,ǻ,ǒ,č,ΑΗ,ƹ,
ΒЈ,ΑΖ,ů,ƺ,ȧ}public enum Κ{з,Ĺ,Ϗ,ΒΤ,Ƶ,ǃ,Ǆ,Ĳ,ĳ}public enum п{None,č,ж,Δκ}public enum Ϊ{Main,ΔЍ,ŷ,Ŷ,ΒЄ,Ƴ,
ΒΓ,Δϋ,Δό,Δω,Ηυ,Ιμ,
Ικ,Ιλ,ΒϏ,ΔБ,ǌ}public enum а{Ǌ,ŭ,Ηχ,Δώ,Ϋ}public enum Ǒ{ΑΒ,ΖЈ}public enum д{ǉ,ΒΪ,ΕΒ,ű}public enum Ѐ{Βν,Δη,ΒΞ,ΒΥ,Ʒ,Βζ,ƾ}public enum я{ΒΠ,ΒΘ,ΒΙ}public enum З{Ų,ΗΦ,ċ}public enum ι{Βο,Βσ,Βτ,ΔЊ,ΔЉ,
Βξ,Ǉ,ǆ,Βρ,
Βς,Βπ}public enum ΰ{ǳ,ȍ,ǲ,Ʋ,ΒΖ,ΒΗ}public enum ļ{Δο,Δή,Ιΰ}public enum с{ΑΑ,Δν,Ηα,Βμ}public enum Ǵ{None,Βψ,Ηη,Ηΰ}public enum σ{ž,Ʊ,ǁ,ű,Βκ,ǂ,Βλ,Ηύ,Βυ}public enum Ιδ{ΑΕ,Ιζ,Ιε,Ιθ,Ια,Ιί,Ιη}class Ψ{public bool δ=false;public Vector3 Δ=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 ψ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float ΚΘ=0;public float[]ǟ=null;public Ψ(){}public Ψ(Ψ α){δ=α.δ;Δ=α.Δ;β=α.β;γ=α.γ;ψ=α.ψ;ν=α.ν;
Ч=α.Ч;ǟ=α.ǟ;}public Ψ(Vector3 Δ,Vector3 γ,Vector3 β,Vector3 ψ,Vector3 ν){this.Δ=Δ;this.β=β;this.γ=γ;this.ψ=ψ;this.Џ=0;this.ν=ν;}public void ΖЅ(List<IMyThrust>ΑΚ,List<string>ȇ){ǟ=new float[ȇ.Count];for(int i=0;i<ǟ.Length;i++)
ǟ[i]=-1;for(int i=0;i<ΑΚ.Count;i++){string ю=Δχ(ΑΚ[i]);int Β=ȇ.IndexOf(ю);if(Β!=-1)
ǟ[Β]=А(ΑΚ[i].MaxEffectiveThrust,ΑΚ[i].MaxThrust);}}}class Βχ{public Ψ Њ=null;public List<Vector3>ť=new List<Vector3>();public float Ɖ=0;public float ΔΡ=0;public float Ł=0;public float Γά=0;public Vector3 Αέ=new Vector3();}class Ľ{public ι Ќ=ι.Βο;public float Ǖ=0;public float Ȯ=0;
public string ǖ="";public string Ǚ="";DateTime ΕЎ;public bool ΓІ=false;private bool ΓΟ=false;public bool ΒΛ(bool Ȇ){if(Ȇ){Ȯ=0;ΓΟ=false;return false;}ΓΟ=true;return Ȯ>Ǖ;}public void Ηθ(){ΒΛ(true);ΓІ=false;}public void ΗЉ(){if(ΓΟ)
if((DateTime.Now-ΕЎ).TotalSeconds>1){Ȯ++;ΕЎ=DateTime.Now;}}public bool Βγ(){switch(Ќ){case ι.Βσ:return true;case ι.Βτ:return true;}return false;}}class ѐ{public String υ="";public String ς="";public int Ǔ=0;public З Œ=З.ċ;public ѐ(String υ,String ς,int Ǔ,З Œ){this.υ=υ;this.ς=ς;
this.Ǔ=Ǔ;this.Œ=Œ;}}class ΗΓ{public ΗΓ(Vector3 ε,float ό){this.ε=ε;this.ό=ό;}public Vector3 ε;public float ό;}class Ļ{public DateTime Θθ;public DateTime Εέ;public String λ="";public String υ="";public String ΚΚ="";public String Μ="";public String ΚΑ="";public String ΙЋ="";public String ΓЅ="";
public Vector3 Δ=new Vector3();public List<ѐ>Ъ=new List<ѐ>();public Ζ Η=Ζ.ΑΕ;public Ǵ ȩ;public float ŕ;public float ņ;public int љ;public int ΙЀ=0;public int ΚΗ=0;public bool Ιβ(){return(DateTime.Now-Θθ).TotalSeconds>10;}public bool ΗΩ(){if(ȩ!=Ǵ.Βψ) return false;return(DateTime.Now-Εέ).TotalSeconds>=4;
}public Ļ(String λ){this.λ=λ;}public Ǵ Ιι(String Ě,bool ΘЉ,bool є,bool ΘΖ){if(Ě==""&&!є) return Ǵ.None;if(ȩ==Ǵ.Βψ&&ΗΩ())
ȩ=Ǵ.Ηΰ;if(ΘЉ){ΓЅ=Ě;ȩ=Ǵ.Βψ;Εέ=DateTime.Now;return ȩ;}else if(є){ΓЅ="";ȩ=Ǵ.None;}else if(ΓЅ==Ě){if(ΘΖ) ȩ=Ǵ.Ηη;return ȩ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 Αώ=new Vector3();Vector3 ί=new Vector3();Vector3 Αΰ=new Vector3();Vector3 Ɩ=new Vector3();Vector3 ƌ=new Vector3();
DateTime ΕЍ=new DateTime();bool Ə=true;int ΔΙ=0;bool ā=false;bool Αι=false;bool ƚ=true;bool ΖϏ=false;bool Ő=false;bool Αζ=false;float Ȝ=0;float ī=0;int ą=0;int ƪ=0;int ΑЁ=0;int ΓΖ=0;float Ŕ=0;float ł=0;double Γϋ=0;float Ƃ=0;List<int>ō=new List<int>();List<int>ş=new List<int>();
Ζ Η=Ζ.ΑΕ;int ύ=0;float Ǳ=0;float ǰ=0;float θ=0;float Ţ=0,ơ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΛ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȉ=new List<IMyThrust>();List<IMyGyro>Αα=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>Ą=new List<IMyReactor>();List<IMyConveyorSorter>ΑЄ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>ſ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ņ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ė=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>Ć=new List<IMyTextSurface>();List<IMyTextPanel>ƈ=new List<IMyTextPanel>();IMyTerminalBlock ћ=null;Ψ Θ=new Ψ();Ψ Γι=new Ψ();
Ψ Γμ=new Ψ();bool ȉ=false;bool Ũ=false;bool ũ=false;bool Αθ=false;List<String>ȇ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>Ư=new List<Ψ>();int ΓΥ=0;double ΖЀ=0;Ψ Ε=new Ψ();Vector3 ǜ;Vector3 ș;Κ ή=Κ.з;Ǒ Ĥ=Ǒ.ΑΒ;int М=0;int б=0;double Ɠ=0;bool ΑΣ=false;Ľ ϊ=new Ľ();
Ľ ϋ=new Ľ();д л=д.ǉ;bool Ĉ=true;Ǒ ȝ=Ǒ.ΑΒ;int й=3;int и=3;int Ц=30;я у=я.ΒΠ;bool Ƞ=true;bool ŏ=true;bool Ň=true;bool Ĭ=false;Ѐ Л=Ѐ.Βν;bool ȟ=true;float ǫ=90;float ч=20;float Ы=5;float ш=20;float ǥ=0.70f;float ū=1.50f;float Ū=2.50f;float ı=0f;float Ġ=0f;Α Υ;
Κ ц;Α Ƥ;Βχ ζ=null;int Ш=0;int Щ=0;int Й=0;int ȗ=0;int ĩ=30;int У=0;bool Ђ=true;Vector3 щ;double К=0;bool ΑЀ=false;int ΔΦ=0;int Ѓ=0;int ΒЏ=0;int Ǭ=0;int ΖΠ=0;Vector3 Ɯ=new Vector3();float к=0;float Э=0;int İ=0;Ψ ΑЅ=null;float Γο=0;float ΑΜ=0;float Р=0;float ħ=0;
bool Γκ=false;bool ĭ=false;bool ƫ=false;bool Ȕ=false;bool Ȑ=false;DateTime ΘΥ=new DateTime();Ψ ě=null;Ψ Εβ=null;float Ǽ=0;п Ǧ;float ȕ=0;float Į=0;String Γφ="";float Αμ=0;float ņ=0;float ŕ=0;List<ѐ>ǘ=new List<ѐ>();float Н=0;bool Ƣ=false;bool Ζτ=false;bool Ζΰ=false;
bool ΑЉ=false;bool Ζα=false;float Γω=0;float ΓЄ=0;Vector3 Ευ=new Vector3();Vector3 φ=new Vector3();Vector3 ΑЊ=new Vector3(1,1,1);float ΔΘ=1;Vector3 Ƨ=new Vector3();bool Αη=false;bool Γβ=false;bool Γσ=false;bool ΕΝ=false;float ΑΠ=0;float ΑΙ=2;Vector3 Ζρ;Vector3 ΔΗ;Vector3 ΔΖ;
float[,]Ǡ=new float[3,2];Dictionary<String,float[,]>ў=new Dictionary<string,float[,]>();Dictionary<String,float>Ζύ=new Dictionary<String,float>();int ΓΑ=0;Dictionary<String,float[]>Ț=new Dictionary<string,float[]>();float ΓЇ;bool ΑΤ=false;int ΑΛ=0;int Ȳ=0;int Αδ=0;int Ģ=0;
String Μ="";int[]ΖΘ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΙ=false;int Αφ=0;int љ=0;Ϊ Ѝ=Ϊ.Main;int ΙΔ=0,ΙΕ=0,ΙΖ=0,ΙΗ=0;int ΘЅ=0,ΘІ=0;bool ń=false;bool ΕΞ=false;String ΓЎ="";String ΓΕ="";IMyBroadcastListener ȑ=null;bool Ετ=true;List<Ļ>Ιχ=null;Ļ ΙЎ=null;
int Ιψ=0,Ιω=0;int Ιϊ=0,Ιϋ=0;int Ιό=0,Ιύ=0,Ιώ=0;String Θη="";Exception Ȗ=null;bool ΑϏ=false;Κ š=Κ.з;bool Εε=false;bool ΕЌ=true;bool ΓЌ=false;String Ζώ="";bool Ʃ=false;bool ΔΒ=true;int Αε=0;int[]ΙБ=null;Vector3 ΓΗ=new Vector3();bool Εϋ=false;bool ǐ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;
Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βδ(Me,controllerTag,true))
Η=Ζ.р;π(Ϊ.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЋ=Ηι();if(Η==Ζ.ΑΕ){List<IMyShipDrill>Επ=new List<IMyShipDrill>();List<IMyShipGrinder>Εό=new List<IMyShipGrinder>();ω.GetBlocksOfType(Επ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Εό,q=>q.CubeGrid==Me.CubeGrid);if(Επ.Count>0)
{Η=Ζ.Ÿ;Μ="Miner mode enabled!";}else if(Εό.Count>0){Η=Ζ.Ǐ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;ǹ(а.Ϋ);}}if(ΓЋ==с.ΑΑ) ƚ=false;if(ΓЋ==с.Δν) Μ="Data restore failed!";if(ΓЋ==с.Βμ) Μ="New version, wipe data";}}void Main(string ѓ,UpdateType ΙΟ){try{if(Ȗ!=null){if(ѓ.Trim().ToUpper()=="RETRY")
{Ȗ=null;Ə=true;ą=0;if(Η!=Ζ.р) χ();Μ="Restarted after error";}else{ΗΠ();ΔЋ();return;}}Αι=(ΙΟ&UpdateType.Update10)!=0;if(Αι) ΔΙ++;ā=ΔΙ>=10;if(ā) ΔΙ=0;if(Αι){ΑΛ++;if(ΑΛ>4) ΑΛ=0;ΑЁ=(int) Math.Max(0,10-(DateTime.Now-ΕЍ).TotalSeconds);}if(ѓ!="") Θη=ѓ;if(Η!=Ζ.р)
Ηφ(ѓ);else
ΗΑ(ѓ);ΖΙ=false;try{int Εγ=Runtime.CurrentInstructionCount;float Ɲ=А(Εγ,Runtime.MaxInstructionCount);if(Ɲ>0.90) Μ="Max. instructions >90%";if(Ɲ>Ŕ) Ŕ=Ɲ;if(ΑΤ){ō.Add(Εγ);while(ō.Count>10) ō.RemoveAt(0);ł=0;for(int i=0;i<ō.Count;i++) ł+=ō[i];ł=А(А(ł,ō.Count),Runtime.MaxInstructionCount);
double Γπ=Runtime.LastRunTimeMs;if(Αζ&&Γπ>Γϋ) Γϋ=Γπ;ş.Add((int)(Γπ*1000f));while(ş.Count>10) ş.RemoveAt(0);Ƃ=0;for(int i=0;i<ş.Count;i++) Ƃ+=ş[i];Ƃ=А(Ƃ,ş.Count)/1000f;}}catch{Ŕ=0;}}catch(Exception e){Ȗ=e;}}void Ηφ(string ѓ){bool ΑЂ=false;String ΕΪ="";if(ύ<=1&&!ΔЁ(ѓ))
ΔЌ(ѓ);int Θς=50;while(ȑ!=null&&ȑ.HasPendingMessage&&Θς-->0){MyIGCMessage ρ=ȑ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String Ơ="";if(ύ<=1&&Ηβ(ref Χ,out Ơ,out ΕΪ)&&Ơ==BROADCAST_SELF_ID){ΔЌ(Χ);ΑЂ=true;break;}}bool ΕЇ=Αζ&&Υ==Α.ж&&!Ő&&!ΑЂ&&ą==0&&!Αθ;
if(ā&&Υ!=Α.ж) ΑЂ=true;if((Αι&&!ΕЇ)||(ā&&ΕЇ)){if(ą==0&&(ΑЁ<=0||Ə)){ΖϏ=ύ>0;ύ=0;ą=1;Ǻ();Ημ();ΗΧ();ǵ("Scan 1");}else if(ą==1){ą=2;Ǻ();Ην();ǵ("Scan 2");}else if(ą==2){ą=0;Ǻ();Ηξ();Ηο();if(ύ<=1) ΔЅ();ǵ("Scan 3");ΕЍ=DateTime.Now;if(ύ<=1&&ƚ)
Αώ=Ƽ(Φ,Φ.CenterOfMass);ƚ=false;if(Ə){Ə=false;bool ΙΤ=ή==Κ.Ϗ;χ();if(ΕЌ&&resumeAfterRestart&&ΙΤ&&ύ<=1) ΓЌ=true;}ΕЌ=false;if(ΖϏ&&ύ==0) Μ="Setup complete";}else{if(ή==Κ.Ϗ&&Η!=Ζ.Ϋ){Ǻ();ΖЂ();ǵ("Inv balance");}Ǻ();switch(ƪ){case 0:ΗЁ();break;case 1:ΗЅ();break;case 2:ΗЀ();break;
case 3:ΗЌ();break;case 4:ΗЄ();break;case 5:ΖЉ();break;case 6:ΖЇ(Φ);break;}ǵ("Update: "+ƪ);ƪ++;if(ƪ>6){ƪ=0;Αζ=true;if(š!=Κ.з){switch(š){case Κ.Ĳ:ȣ();break;case Κ.ĳ:ȣ();break;case Κ.Ϗ:ȣ();break;case Κ.ǃ:ƻ();break;case Κ.Ǆ:ď();break;}š=Κ.з;}}}if(!Ə){if(!Ƿ(Φ,true)){Φ=null;
ƚ=true;ύ=2;}if(ύ>=2&&Υ!=Α.ж)
χ();if(ύ<=1){ī=Φ.CalculateShipMass().PhysicalMass;Ȝ=(float) Φ.GetShipSpeed();ί=ΗϏ(Φ,Αώ);Αΰ=Φ.WorldMatrix.Forward;Ɩ=Φ.WorldMatrix.Left;ƌ=Φ.WorldMatrix.Down;ΗЈ();ΗЊ();ΗЂ();if(ΓЌ){ΓЌ=false;ȣ();if(Υ!=Α.ж) Μ="Job resumed after restart";}if(Υ!=Α.ж){Ő=false;ΔА(false);Δβ(false);String ΘЎ=Βί(Υ)+" "+(int) Υ;
Ǻ();ΗІ();ΒЇ(false);ǵ(ΘЎ);ΗЋ();ΗЇ();Ǻ();ΗЃ();ǵ("Thruster");Ǻ();ΕΙ();ǵ("Gyroscope");}else{if(Ő){if(ǀ()){Х(ƌ,Αΰ,Ɩ,0.25f,true);ΕΙ();Μ="Aligning to planet: "+Math.Round(ΑΠ-0.25f,2)+"°";if(Αη) Ββ(true,true);}else Ββ(true,true);}}Ʃ=false;}}}Ǻ();ΔЋ();ǵ("Print");if(ΑЂ||ΓΖ<=0){Ǻ();ΖЃ(ΕΪ);
ǵ("Broadcast");ΓΖ=4;}else if(ā) ΓΖ--;}void ΔЌ(string ѓ){if(ѓ=="") return;var Е=ѓ.ToUpper().Split(' ');Е.DefaultIfEmpty("");var Ě=Е.ElementAtOrDefault(0);var ė=Е.ElementAtOrDefault(1);var Ɓ=Е.ElementAtOrDefault(2);var ΕΡ=Е.ElementAtOrDefault(3);String ȳ="Invalid argument: "+ѓ;bool ΔΠ=false;
switch(Ě){case "UP":this.ΔЇ(false);break;case "DOWN":this.ΔІ(false);break;case "UPLOOP":this.ΔЇ(true);break;case "DOWNLOOP":this.ΔІ(true);break;case "APPLY":this.Δΰ(true);break;case "MRES":љ=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.ΒЃ();}break;case "PATH":{this.χ();this.ΒЃ();Θ.δ=true;}break;
case "START":{this.χ();ΒЂ();}break;case "ALIGN":{Ββ(!Ő,false);}break;case "CONT":{this.χ();this.ȣ();}break;case "JOBPOS":{this.χ();this.ď();}break;case "HOMEPOS":{this.χ();this.ƻ();}break;case "FULL":{Ƣ=true;}break;case "RESET":{ΑϏ=true;ύ=2;}break;case "EXPORT":{ΗΘ();}break;
case "IMPORT":{ΗΤ(ė=="JOB");}break;default:ΔΠ=true;break;}if(Η!=Ζ.Ϋ){switch(Ě){case "SHUTTLE":{ΗΖ();}break;case "CFGS":{if(!ΖА(ė,Ɓ,ΕΡ)) Μ=ȳ;}break;case "CFGB":{if(!ΖЏ(ė,Ɓ)) Μ=ȳ;}break;case "CFGL":{if(!Đ(ref ǫ,true,σ.ǁ,ė,"")||!ΖБ(Ɓ))
Μ=ȳ;}break;case "CFGE":{if(!Đ(ref Ы,true,σ.ǂ,ė,"IG")||
!Đ(ref ч,true,σ.Βκ,Ɓ,"IG")||
!Đ(ref ш,true,σ.Βλ,ΕΡ,"IG"))
Μ=ȳ;}break;case "CFGA":{if(!Đ(ref ǥ,false,σ.Ʊ,ė,"")) Μ=ȳ;}break;case "CFGW":{if(!Đ(ref ū,false,σ.ž,ė,"")||
!Đ(ref Ū,false,σ.ž,Ɓ,""))
Μ=ȳ;}break;case "NEXT":{ΒΒ(false);}break;case "PREV":{ΒΒ(true);}break;default:if(ΔΠ) Μ=ȳ;break;}}else{switch(Ě){case "UNDOCK":{Ʃ=true;}break;default:if(ΔΠ) Μ=ȳ;break;}}}String ΗΜ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΖ(){χ();Η=Ζ.Ϋ;ǹ(а.Ϋ);Ε.δ=false;Θ.δ=false;ξ=null;ο.Clear();ή=Κ.з;}void Ββ(bool Ј,bool ф){if(!Ј) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ј){Ő=false;ΕΓ();ǋ(false,0,0,0,0);return;}if(ǀ()) Ő=true;}bool ΖА(String ΙΦ,String Θή,String ΘΟ)
{bool ΙΡ=ή==Κ.Ϗ;int w,h,d;if(int.TryParse(ΙΦ,out w)&&int.TryParse(Θή,out h)&&int.TryParse(ΘΟ,out d)){this.χ();й=w;и=h;Ц=d;Ĕ(false);Ž(false,false);if(ΙΡ) ȣ();return true;}return false;}bool ΖБ(String ΕΠ){if(ΕΠ=="ON"){Ƞ=true;return true;}if(ΕΠ=="OFF"){Ƞ=false;return true;}
return false;}bool ΖЏ(String Εο,String ΑΡ){bool ƛ=true;if(Εο=="HOME") Ĉ=true;else if(Εο=="STOP") Ĉ=false;else ƛ=false;if(ΑΡ=="HOME") л=д.ǉ;else if(ΑΡ=="STOP") л=д.ΕΒ;else if(ΑΡ=="JOB") л=д.ΒΪ;else if(ΑΡ=="IG") л=д.ű;else ƛ=false;return ƛ;}void π(Ϊ ΓЂ){ΖΘ[(int) Ѝ]=љ;
љ=ΖΘ[(int) ΓЂ];if(ΓЂ==Ϊ.Ηυ) љ=0;Ѝ=ΓЂ;if(Η!=Ζ.р)
ē(Ѝ==Ϊ.ŷ,false,0,0);ΖΙ=true;}void ΔЇ(bool Γτ){if(љ>0) љ--;else if(Γτ) љ=Αφ-1;}void ΔІ(bool Γτ){if(љ<Αφ-1) љ++;else if(Γτ) љ=0;}String ǹ(а ρ){switch(ρ){case а.Ǌ:Μ="Job is running";break;case а.ŭ:Μ="Connector not ready!";break;case а.Ηχ:Μ="Ship modified, path outdated!";break;
case а.Δώ:Μ="Interrupted by player!";break;case а.Ϋ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΟ(Ǒ ε){switch(ε){case Ǒ.ΑΒ:return "Top-Left";case Ǒ.ΖЈ:return "Center";default:return "";}}String ΗΚ(я ƙ){switch(ƙ){case я.ΒΘ:return "Auto"+(Η==Ζ.Ÿ?" (Ore)":"");case я.ΒΙ:return "Auto (+Stone)";
case я.ΒΠ:return "Default";default:return "";}}String Βέ(Κ Ф){switch(Ф){case Κ.з:return "No job";case Κ.Ĺ:return "Job paused";case Κ.Ϗ:return "Job active";case Κ.Ĳ:return "Job active";case Κ.ĳ:return "Job active";case Κ.ΒΤ:return "Job done";case Κ.Ƶ:return "Job changed";case Κ.ǃ:return "Move home";
case Κ.Ǆ:return "Move to job";}return "";}String Δς(д ΘΕ){switch(ΘΕ){case д.ǉ:return "Return home";case д.ΒΪ:return "Fly to job pos";case д.ΕΒ:return "Stop";case д.ű:return "Ignore";}return "";}String ΗΛ(Ѐ ƙ){switch(ƙ){case Ѐ.Βν:return "Off";case Ѐ.ΒΥ:return "Drop pos (Stone) ";case Ѐ.Ʒ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δη:return "Cur. pos (Stone)";case Ѐ.ΒΞ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βζ:return "In motion (Stone)";case Ѐ.ƾ:return "In motion (Sto.+Ice)";}return "";}String Δπ(п Ф){switch(Ф){case п.None:return "No batteries";case п.č:return "Charging";case п.Δκ:return "Discharging";}return "";}String Βί(Α Ф)
{String ΙІ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ȋ:return "Flying to XY position";case Α.ĸ:return Η==Ζ.Ǐ?"Grinding":"Mining";case Α.Ǹ:return "Returning";case Α.ů:return "Flying to drop pos";case Α.đ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȥ:return "Flying to path";case Α.Ķ:return "Flying to job position";case Α.ǡ:return "Approaching dock";case Α.Ď:return "Docking";case Α.Ċ:return "Aligning";case Α.ȡ:return "Aligning";case Α.Ź:return "Retry docking";case Α.ǻ:return "Unloading";case Α.ȧ:return Ζώ;
case Α.ǒ:return "Undocking";case Α.č:return "Charging batteries";case Α.ΑΗ:return "Waiting for uranium";case Α.ƹ:return "Filling up hydrogen";case Α.ΒЈ:return "Waiting for ejection";case Α.ΑΖ:return "Waiting for ejection";case Α.ƺ:return "Flying to drop pos";}return "";}String Δψ(ι Ќ){switch(Ќ)
{case ι.Βο:return "On \"Undock\" command";case ι.Βσ:return "On player entered cockpit";case ι.ΔЊ:return "On ship is full";case ι.ΔЉ:return "On ship is empty";case ι.Βτ:return "On time delay";case ι.Ǉ:return "On batteries empty(<25%)";case ι.ǆ:return "On batteries empty(=0%)";case ι.Βξ:return "On batteries full";
case ι.Βς:return "On hydrogen empty(<25%)";case ι.Βπ:return "On hydrogen empty(=0%)";case ι.Βρ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΫ,int ȱ,bool Ξ,String ƕ){Αφ+=1;if(ΖΫ==ȱ) ƕ=">"+ƕ+(ΑΛ>=2?" .":"");else ƕ=" "+ƕ;Γ+=ƕ+"\n";return ΖΫ==ȱ&&Ξ;}String Δΰ(bool Ξ)
{int Β=0;int Ο=љ;Αφ=0;String Ζή="———————————————\n";String ŧ="--------------------------------------------\n";String Γ="";Γ+=Βέ(ή)+" | "+(Θ.δ?"Ready to dock":"No dock")+"\n";Γ+=Ζή;double ǭ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ϊ.Main){bool Ā=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЃ();
if(Η==Ζ.Ÿ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.ŷ);if(Η==Ζ.Ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.ŷ);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.ΒϏ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȣ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƻ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) ď();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ā) π(Ϊ.ǌ);else π(Ϊ.Ƴ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.Δϋ);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δω);}else if(Ѝ==Ϊ.ŷ){double ΙΥ=Math.Round(й*Ǳ,1);double Θέ=Math.Round(и*ǰ,1);String Є="";if(Π(ref Є,Ο,Β++,Ξ," Start new job!")) ΒЂ();if(Π(ref Є,Ο,Β++,Ξ," Change current job")){Ž(false,false);π(Ϊ.Main);}if(Π(ref Є,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΥ+"m)")){Ĵ(ref й,5,20,1);Ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Width -")){Ĵ(ref й,-5,20,-1);Ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height + (Height: "+и+" = "+Θέ+"m)")){Ĵ(ref и,5,20,1);Ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height -")){Ĵ(ref и,-5,20,-1);Ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth + ("+(у==я.ΒΠ?"Depth":"Min")+": "+Ц+"m)")){Ĵ(ref Ц,5,50,2);Ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Depth -")){Ĵ(ref Ц,-5,50,-2);Ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth mode: "+ΗΚ(у))){у=Ȥ(у);}if(Π(ref Є,Ο,Β++,Ξ," Start pos: "+ΗΟ(ȝ))){ȝ=Ȥ(ȝ);}if(Η==Ζ.Ǐ&&у==я.ΒΙ) у=Ȥ(у);Γ+=ΔЎ(8,Є,Ο,ref ΘЅ);}else if(Ѝ==Ϊ.ΒϏ){float[]Εη=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔБ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δψ(ϊ.Ќ))) ϊ.Ќ=Ȥ(ϊ.Ќ);if(!ϊ.Βγ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βά((int) ϊ.Ǖ))) ϊ.Ǖ=Δθ(ϊ.Ǖ,Εη);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δψ(ϋ.Ќ))) ϋ.Ќ=Ȥ(ϋ.Ќ);
if(!ϋ.Βγ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βά((int) ϋ.Ǖ))) ϋ.Ǖ=Δθ(ϋ.Ǖ,Εη);}else if(Ѝ==Ϊ.ΔБ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЂ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.ΒϏ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǖ!=""?ϊ.ǖ:"-"))) ϊ.ǖ=ǅ(ref ΙΔ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǚ!=""?ϊ.Ǚ:"-"))) ϊ.Ǚ=ǅ(ref ΙΖ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǖ!=""?ϋ.ǖ:"-"))) ϋ.ǖ=ǅ(ref ΙΕ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǚ!=""?ϋ.Ǚ:"-"))) ϋ.Ǚ=ǅ(ref ΙΗ);
}else if(Ѝ==Ϊ.Ŷ){String ΖΑ=ǫ+" %";if(ā) Ȳ++;if(Ȳ>1){Ȳ=0;Αδ++;if(Αδ>1) Αδ=0;}bool[]ΓА=new bool[]{Ą.Count==0,Ǧ==п.None,і.Count==0};int Ζω=0;while(true){Ζω++;Ģ++;if(Ģ>ΓА.Length-1) Ģ=0;if(Ζω>=ΓА.Length) break;if(!ΓА[Ģ]) break;}bool Ā=Η==Ζ.Ϋ;if(!Ā&&Ƞ&&Н!=-1&&Αδ==0)
ΖΑ=Н<1000000?Math.Round(Н)+" Kg":Math.Round(Н/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ā) π(Ϊ.Ƴ);else π(Ϊ.ǌ);if(!Ā){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΒ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ʃ=true;Γ+=ŧ;if(!Ā) Γ+="Progress: "+Math.Round(Ɠ,1)+" %\n";Γ+="State: "+Βί(Υ)+" "+ǭ+"m \n";Γ+="Load: "+Αμ+" % Max: "+ΖΑ+" \n";if(Ģ==0) Γ+="Uranium: "+(Ą.Count==0?"No reactors":Math.Round(Į,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ģ==1) Γ+="Battery: "+(Ǧ==п.None?Δπ(Ǧ):Ǽ+"% "+(ч==-1||Ā?"":" Min: "+ч+" %"))+"\n";if(Ģ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% "+(ш==-1||Ā?"":" Min: "+ш+" %"))+"\n";}else if(Ѝ==Ϊ.Ƴ){String Є="";if(Π(ref Є,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}
if(Π(ref Є,Ο,Β++,Ξ," Max load: "+ǫ+"%")) г(ref ǫ,ǫ<=80?-10:-5,σ.ǁ,false);if(Π(ref Є,Ο,Β++,Ξ," Weight limit: "+(Ƞ?"On":"Off"))) Ƞ=!Ƞ;if(Π(ref Є,Ο,Β++,Ξ," Ejection: "+ΗΛ(Л))) Л=Ȥ(Л);if(Π(ref Є,Ο,Β++,Ξ," Toggle sorters: "+(Ĭ?"On":"Off"))){Ĭ=!Ĭ;if(Ĭ) Ǥ(ΔΒ);}
if(Π(ref Є,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Є,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǂ,true);if(Π(ref Є,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) г(ref ч,-5,σ.Βκ,true);if(Π(ref Є,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-10,σ.Βλ,true);
if(Π(ref Є,Ο,Β++,Ξ," When done: "+(Ĉ?"Return home":"Stop"))) Ĉ=!Ĉ;if(Π(ref Є,Ο,Β++,Ξ," On damage: "+Δς(л))) л=Ȥ(л);if(Π(ref Є,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΓ);Γ+=ΔЎ(8,Є,Ο,ref ΘІ);}else if(Ѝ==Ϊ.ΒΓ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǐ?" Grinder":" Drill")+" inv. balancing: "+(ŏ?"On":"Off"))) ŏ=!ŏ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǐ?" grinders":" drills")+": "+(Ň?"Fwd + Bwd":"Fwd"))) Ň=!Ň;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ū+"m/s")) г(ref ū,0.5f,σ.ž,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ū+"m/s")) г(ref Ū,0.5f,σ.ž,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǥ*100f)+"%"+(ǥ>0.80f?" (risky)":""))) г(ref ǥ,0.1f,σ.Ʊ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ı*100f+"%")) Δδ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+Ġ*100f+"%")) Δδ(false,0.05f);
}else if(Ѝ==Ϊ.ǌ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.Ŷ);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǫ+"%")) г(ref ǫ,ǫ<=80?-10:-5,σ.ǁ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǂ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δς(л))) л=Ȥ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǥ*100f)+"%"+(ǥ>0.80f?" (risky)":""))) г(ref ǥ,0.1f,σ.Ʊ,false);
}else if(Ѝ==Ϊ.ΔЍ){double Θι=0;if(Σ.Count>0) Θι=Vector3.Distance(Σ.Last().Δ,ί);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΔ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȉ?"Use old home":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȉ?"Use old connector":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ũ?"Use old connector":(Ε.δ?"Was set! ":"none ")))) Ũ=!Ũ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ũ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ũ=!ũ;Γ+=ŧ;Γ+="Wp spacing: "+Math.Round(ΖЀ)+"m\n";}else if(Ѝ==Ϊ.ΒЄ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();π(Ϊ.Main);}
Γ+=ŧ;Γ+="State: "+Βί(Υ)+" \n";Γ+="Speed: "+Math.Round(Ȝ,1)+"m/s\n";Γ+="Target dist: "+ǭ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΦ+"\n";}else if(Ѝ==Ϊ.Δϋ){List<IMyTerminalBlock>Ĝ=Δσ();if(ā) Ȳ++;if(Ȳ>=Ĝ.Count) Ȳ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.Δό);
Γ+=ŧ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αμ,1)+"% "+Math.Round(ņ,1)+" / "+Math.Round(ŕ,1)+"\n";Γ+="Uranium: "+(Ą.Count==0?"No reactors":Math.Round(Į,1)+"Kg "+Γφ)+"\n";Γ+="Battery: "+(Ǧ==п.None?"":Ǽ+"% ")+Δπ(Ǧ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% ")+"\n";
Γ+="Damage: "+(Ĝ.Count==0?"None":""+(Ȳ+1)+"/"+Ĝ.Count+" "+Ĝ[Ȳ].CustomName)+"\n";}else if(Ѝ==Ϊ.Δό){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="Next scan: "+ΑЁ+"s\n";Γ+="Drill area: "+Math.Round(Ǳ,1)+"x"+Math.Round(ǰ,1)+"m Ship: "+Math.Round(Ţ,1)+"x"+Math.Round(ơ,1)+"m ("+Math.Round(θ,1)+"m)\n";
Γ+="Broadcast: "+(ń?"Online - "+ΓЎ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ŕ*100f,1)+"% \n";}else if(Ѝ==Ϊ.Δω){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+ΑЁ+"s\n";if(ΑϏ) Γ="Recompile script now";int Θκ=Γ.Split('\n').Length;for(int i=Θκ;i<=10;i++) Γ+="\n";Γ+=Ζή;Γ+="Last: "+Μ+"\n";
return Γ;}void Δδ(bool Θδ,float ѝ){χ();Ž(true,false);if(Θδ) г(ref ı,ѝ,σ.Βυ,false);else г(ref Ġ,ѝ,σ.Βυ,false);Δγ();ē(true,true,0,0);}float Δθ(float η,float[]ΑБ){float έ=ΑБ[0];for(int i=ΑБ.Length-1;i>=0;i--)
if(η<ΑБ[i]) έ=ΑБ[i];return έ;}String ǅ(ref int ȱ){String έ="";if(ȱ>=ΔΛ.Count) ȱ=-1;if(ȱ>=0) έ=ΔΛ[ȱ].CustomName;ȱ++;return έ;}void ΑΓ(string υ){if(ή!=Κ.Ϗ) return;if(υ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(υ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+υ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void Ĵ(ref int value,int Ƅ,int Ȟ,int ΓБ){if(Ƅ==0) return;if(value<Ȟ&&ΓБ>0||value<=Ȟ&&ΓБ<0){value+=ΓБ;return;}int ΗЎ=Math.Abs(Ƅ);int ΒЉ=0;int Γύ=1;while(true){ΒЉ+=Γύ*ΗЎ*10;if(Ƅ<0&&value-Ȟ<=ΒЉ) break;if(Ƅ>0&&value-Ȟ<ΒЉ) break;
Γύ++;}value+=Γύ*Ƅ;}void Ĕ(bool ΚΙ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);ē(Ѝ==Ϊ.ŷ,false,0,0);}bool Đ(ref float value,bool ΘЂ,σ ς,String ģ,String Θα){if(ģ=="") return false;float Ś=-1;bool ΕЄ=false;if(ģ.ToUpper()==Θα) ΕЄ=true;else if(!float.TryParse(ģ,out Ś)) return false;
else Ś=Math.Max(0,Ś);if(ΘЂ) Ś=(float) Math.Round(Ś);г(ref value,Ś,ς,ΕЄ,false);return true;}void г(ref float value,float ѝ,σ ς,bool ƨ){г(ref value,value+ѝ,ς,ƨ,true);}void г(ref float value,float Ŗ,σ ς,bool ƨ,bool ΙΩ){float Ǜ=0,ǚ=0;if(ς==σ.ž){Ǜ=0.5f;ǚ=10f;}if(ς==σ.Ʊ){Ǜ=0.1f;ǚ=1f;}
if(ς==σ.ű){Ǜ=50f;ǚ=100f;}if(ς==σ.Βκ){Ǜ=5f;ǚ=30f;}if(ς==σ.ǂ){Ǜ=1f;ǚ=25f;}if(ς==σ.ǁ){Ǜ=10f;ǚ=95f;}if(ς==σ.Βλ){Ǜ=10f;ǚ=90f;}if(ς==σ.Ηύ){Ǜ=10f;ǚ=1800;}if(ς==σ.Βυ){Ǜ=0.0f;ǚ=0.75f;}if(Ŗ==-1&&ƨ){value=-1;return;}if(value==-1) ƨ=false;
bool ΖΤ=Ŗ<Ǜ||Ŗ>ǚ;if(ΖΤ&&ΙΩ){if(Ŗ<value) value=ǚ;else if(Ŗ>value) value=Ǜ;}else value=Ŗ;if(ΖΤ&&ƨ) value=-1;else value=Math.Max(Ǜ,Math.Min(value,ǚ));value=(float) Math.Round(value,2);}void ΒΒ(bool Θύ){if(Θύ) Й=Math.Max(0,Й-1);else Й++;ΒΧ();ΒΦ();for(int k=0;k<ȃ.Count&&k<С.Length;k++) Βύ(ȃ[k],k<Й);
Ё=-1;ΒЇ(true);}T Ȥ<T>(T η){int Β=Array.IndexOf(Enum.GetValues(η.GetType()),η);Β++;if(Β>=ΗΗ(η)) Β=0;return(T) Enum.GetValues(η.GetType()).GetValue(Β);}int ΗΗ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЃ(){Ư.Clear();for(int i=0;i<Σ.Count;i++) Ư.Add(Σ[i]);Σ.Clear();
Αθ=true;Γι=new Ψ(Θ);Γμ=new Ψ(Ε);Θ.δ=false;if(Η==Ζ.Ϋ) Ε.δ=false;for(int i=0;i<ў.Count;i++)
if(!ȇ.Contains(ў.Keys.ElementAt(i)))
ȇ.Add(ў.Keys.ElementAt(i));ȉ=false;Ũ=false;ũ=false;π(Ϊ.ΔЍ);}void ΕΔ(){if(ȉ) Θ=Γι;if(Ũ) Ε=Γμ;if(ũ){Σ.Clear();for(int i=0;i<Ư.Count;i++) Σ.Add(Ư[i]);}Αθ=false;χ();π(Ϊ.Main);}void ΗЈ(){if(!Αθ) return;if(Υ!=Α.ж){ΕΔ();return;}if(!Γι.δ) ȉ=false;if(!Γμ.δ) Ũ=false;if(Ư.Count<=1) ũ=false;
IMyShipConnector И=О(MyShipConnectorStatus.Connectable);if(И==null) И=О(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(Ȝ,2)<=0.20) ΓΥ++;else ΓΥ=0;if(ΓΥ>=5){if(Η==Ζ.Ϋ&&(Θ.δ||ȉ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.γ=Φ.WorldMatrix.Forward;Ε.ψ=Φ.WorldMatrix.Left;
Ε.β=Φ.WorldMatrix.Down;Ε.ν=Φ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.δ=true;Ε.Ч=И.Position;}else{Θ.γ=Φ.WorldMatrix.Forward;Θ.ψ=Φ.WorldMatrix.Left;Θ.β=Φ.WorldMatrix.Down;Θ.ν=Φ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.δ=true;Θ.Ч=И.Position;}}}double Γξ=-1;if(Σ.Count>0) Γξ=Vector3.Distance(ί,Σ.Last().Δ);
double ΘЌ=Math.Max(1.5,Math.Pow(Ȝ/100.0,2));double ΖΚ=Math.Max(Ȝ*ΘЌ,2);ΖЀ=ΖΚ;if((Γξ==-1)||Γξ>=ΖΚ){Ψ Ω=new Ψ(ί,Αΰ,ƌ,Ɩ,Φ.GetNaturalGravity());Ω.ΖЅ(Ȉ,ȇ);Σ.Add(Ω);}}Ψ е(){if(Η!=Ζ.Ϋ) return њ!=null?њ:Θ;return Εβ;}void ĺ(Ψ ά,Κ Ф){Εβ=ά;if(ή==Κ.Ϗ) ц=Ф;}bool Ƕ(Ψ ά,float ό,bool ΙΠ,out Vector3 ε)
{if(ΙΠ){Vector3I Γα=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(Γα);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ε=new Vector3();return false;}Vector3 ΘΛ=Я(Φ,Τ.FatBlock.GetPosition()-ί);Vector3 ΘΚ=Я(Φ,Τ.FatBlock.WorldMatrix.Forward);ε=ά.Δ-ΒІ(ά.γ,ά.β*-1,ΘΛ)-ΒІ(ά.γ,ά.β*-1,ΘΚ)*ό;
return true;}else{ε=ά.Δ;return true;}}Vector3 Ȍ(int ΘΘ,int Ş,bool ΘЀ){if(!ΘЀ&&Εϋ) return ΓΗ;float ΘΙ=((М-1f)/2f)-ΘΘ;float ΘЃ=((б-1f)/2f)-Ş;ΓΗ=Ε.Δ+Ε.ψ*ΘΙ*Ǳ+ș*-1*ΘЃ*ǰ;Εϋ=true;return ΓΗ;}Vector3 Βή(Vector3 ΘА,float ΓΡ){return ΘА+(ǜ*ΓΡ);}ΰ ΒΡ(){float Б=-1;ΰ έ=ΰ.ǳ;
if(Η!=Ζ.Ϋ){ΔЅ();if(ή!=Κ.з){Vector3 Љ=Ē(ǜ,ș*-1,ί-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(М*Ǳ)/2f&&Math.Abs(Љ.Y)<=(float)(б*ǰ)/2f){if(Љ.Z<=-1&&Љ.Z>=-ĩ*2) return ΰ.Ʋ;if(Љ.Z>-1&&Љ.Z<θ*2) return ΰ.ǳ;}if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;}if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;for(int i=0;i<Σ.Count;i++)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Vector3.Distance(ί,Θ.Δ)<dockDist*θ) έ=ΰ.ǲ;if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǲ;}}else{Vector3 ε=new Vector3();IMyShipConnector Ȁ=О(MyShipConnectorStatus.Connected);if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;if(Ƕ(Θ,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǲ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΖ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*θ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*θ)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Ε.δ){if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;if(Ƕ(Ε,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǳ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΗ;}}return έ;}bool Ȣ(Vector3 ε,ref float Б){float ό=Vector3.Distance(ε,ί);if(ό<Б||Б==-1){Б=ό;return true;}return false;}void ΒЂ(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){ȣ();return;}Ε.Δ=ί;Ε.ν=Φ.GetNaturalGravity();Ε.γ=Αΰ;Ε.β=ƌ;Ε.ψ=Ɩ;
ǜ=ћ.WorldMatrix.Forward;ș=Ε.β;if(ǜ==Φ.WorldMatrix.Down) ș=Φ.WorldMatrix.Backward;Ž(true,true);Ν(Α.ȋ);ΒЁ();}void Ž(bool Ǘ,bool ΘЁ){if(ή==Κ.з&&!Ǘ) return;bool Θσ=Ǘ||ή==Κ.ΒΤ||М!=й||б!=и||Ĥ!=ȝ;if(Θσ){if(ή!=Κ.з){ή=Κ.Ƶ;Ȍ(Ш,Щ,ΘЁ);Μ="Job changed, lost progress";}Ĥ=ȝ;
М=й;б=и;Щ=0;Ш=0;С="";Ё=-1;ħ=0;ȗ=0;Р=0;Й=0;ΒЇ(true);}}void Βω(){П(ί,0);ђ(Ȉ,true);}void ΒЁ(){ǹ(а.Ǌ);Βω();Ż(Ȅ,false);Ǥ(ΔΒ);ή=Κ.Ϗ;Δβ(true);ц=ή;π(Ϊ.Ŷ);Ηλ();ΑΣ=true;Αε=0;for(int i=ſ.Count-1;i>=0;i--)
if(ų(ſ[i],false)) Αε++;if(Αε>0) Μ="Started with damage";}void χ(){if(ή==Κ.Ϗ){ή=Κ.Ĺ;Μ="Job paused";}Ν(Α.ж);ц=ή;ǋ(false,0,0,0,0);ΒЅ();Δί(new Vector3(),false);ΕΓ();Βό(ChargeMode.Auto);Βώ(false);ΔА(true);Ȏ(Α.ж);ē(false,false,0,0);ђ(ο,false);ђ(т,true);Ǥ(true);ΑЀ=false;ƶ();Ƌ=false;
Βϊ();ΑΣ=false;Ƣ=false;Ʃ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.Ƴ&&Ѝ!=Ϊ.ΒΓ&&Ѝ!=Ϊ.ǌ)
π(Ϊ.Main);}void ȣ(){ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){if(!Ε.δ||!Θ.δ) return;ΒЁ();bool Γί=Vector3.Distance(ί,Θ.Δ)<Vector3.Distance(ί,Ε.Δ);if(š==Κ.Ĳ) Γί=true;if(š==Κ.ĳ) Γί=false;if(Γί){ĺ(Θ,Κ.Ĳ);switch(Д){case ΰ.ΒΖ:Ν(Α.ȧ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.ǡ);break;default:Ν(Α.ǒ);break;
}}else{ĺ(Ε,Κ.ĳ);switch(Д){case ΰ.ΒΗ:Ν(Α.ȧ);break;case ΰ.ǳ:Ν(Α.ǡ);break;case ΰ.ȍ:Ν(Α.Ю);break;default:Ν(Α.ǒ);break;}}}else{if(ή!=Κ.Ĺ&&ή!=Κ.Ƶ) return;bool ΙΣ=ή==Κ.Ƶ;ΒЁ();bool Γδ=Ŵ(false)&&Θ.δ;switch(Д){case ΰ.ǳ:Ν(Γδ?Α.ȥ:Α.ȋ);break;case ΰ.ȍ:Ν(Γδ?Α.Ѕ:Α.Ю);break;case ΰ.ǲ:Ν(Γδ?Α.ǡ:Α.ǻ);break;
case ΰ.Ʋ:{if(ȗ!=Й||ΙΣ) Ν(Α.Ǹ);else Ν(Α.ĸ);}break;default:break;}}}void ď(){if(ή==Κ.з&&!Θ.δ) return;if(Η==Ζ.Ϋ&&(!Ε.δ||!Θ.δ)) return;Μ="Move to job";ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){ĺ(Ε,Κ.ĳ);switch(Д){case ΰ.ǳ:Ν(Α.ǡ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ΒΗ:return;default:Ν(Α.ǒ);break;
}Ȏ(Α.ȧ);}else{switch(Д){case ΰ.ǳ:Ν(Α.Ю);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.ǻ);break;case ΰ.Ʋ:Ν(Α.Ǹ);break;default:break;}if(ή==Κ.з) Ȏ(Α.Ю);else Ȏ(Α.ȡ);ΑЀ=true;}Βω();π(Ϊ.ΒЄ);Ż(Ȅ,false);ц=Κ.Ǆ;}void ƻ(){if(!Θ.δ) return;Μ="Move home";ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){ĺ(Θ,Κ.Ĳ);switch(Д)
{case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.ǡ);break;case ΰ.ΒΖ:return;default:Ν(Α.ǒ);break;}Ȏ(Α.ȧ);}else{if(О(MyShipConnectorStatus.Connected)!=null) return;if(О(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Ď);Ȏ(Α.ǻ);return;}switch(Д){case ΰ.ǳ:Ν(Α.ȥ);break;case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ѕ);break;
case ΰ.Ʋ:Ν(Α.đ);break;default:break;}Ȏ(Α.ǻ);}Βω();π(Ϊ.ΒЄ);Ż(Ȅ,false);ц=Κ.ǃ;}void Ν(Α Γώ){if(Γώ==Α.ж) Ƥ=Α.ж;if(Ƥ!=Α.ж&&Υ==Ƥ&&Γώ!=Ƥ){χ();return;}Ђ=true;Υ=Γώ;}void Ȏ(Α Ф){this.Ƥ=Ф;}void ΗІ(){if(Υ==Α.ȋ){if(Ђ){Ѓ=0;if(ȗ!=Й) ħ=0;ȗ=Й;}if(Ѓ==0){int ΓΚ=ΖЌ(Й);if(ΓΚ<0)
{if(!ΓΞ){ΓΞ=true;Εί=ί;}П(Εί,0);Μ="Waiting for the controller";return;}ΓΞ=false;if(ΓΚ!=Й){Й=ΓΚ;ħ=0;ȗ=Й;}ļ έ=Ης(Й);if(έ==ļ.Δή){ή=Κ.ΒΤ;bool ΖΣ=С.IndexOf('0')>=0;Μ=ΖΣ?"No free hole left for this ship":"Job done";if((Ĉ||ΖΣ)&&Θ.δ){Ν(Α.ȥ);Ȏ(Α.ǻ);ц=Κ.ǃ;}
else{Ν(Α.Ķ);Ȏ(Α.ȡ);ц=Κ.Ǆ;}return;}if(έ==ļ.Δο){Ѓ=1;ђ(ο,true);щ=Ȍ(Ш,Щ,true);П(щ,10);Х(Ε.β,Ε.γ,Ε.ψ,false);}}else{if(Ё>=0&&(ΔЂ()?Ǩ>=0&&Ǩ!=Ё:Βε(Ё,true))){Ѓ=0;return;}if(К<wpReachedDist){Ν(Α.ĸ);return;}}}if(Υ==Α.ĸ){if(Ђ){ђ(ο,true);Ǥ(false);щ=Ȍ(Ш,Щ,false);
П(Βή(щ,0),0);Х(Ε.β,Ε.γ,Ε.ψ,false);Ѓ=1;к=0;Э=0;ΑΜ=0;Γο=-1;ĩ=Ц;Γκ=true;}if(!ΒΜ()){Ν(Α.đ);return;}if(Ŵ(true)){ΖΠ=Ȋ("","ORE",З.ċ);if((Л==Ѐ.ΒΥ||Л==Ѐ.Ʒ||
Л==Ѐ.Βζ||Л==Ѐ.ƾ)&&Η!=Ζ.Ǐ)
Ν(Α.ů);else if((Л==Ѐ.Δη||Л==Ѐ.ΒΞ)&&Η!=Ζ.Ǐ)
Ν(Α.ΒЈ);else
Ν(Α.đ);return;}Р=Vector3.Distance(ί,щ);if(Р>ħ){ħ=Р;Γκ=false;}if(Η==Ζ.Ǐ&&Δτ()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ѓ>0&&Ѓ<Э){if(Р>к){if(Э>150) к=Р;else к=(float) Math.Ceiling(Р);Ѓ=1;}else Ѓ++;}else{if(Ѓ>0){Μ="Ship stuck! Retrying";к=Р;Ѓ=0;ē(false,true,0,θ*sensorRange);}П(Βή(щ,Math.Max(0,к-θ)),Βα(false));if(К<=wpReachedDist/2){Ѓ=1;к=0;}
return;}ē(false,true,θ*sensorRange,0);Vector3 ΗА=щ+ǜ*Р;bool ΕЈ=false;if(Vector3.Distance(ΗА,ί)>0.3f){Vector3 ΘΜ=щ+ǜ*(Р+0.1f);П(ΘΜ,4);ΕЈ=true;}else{float ǝ=Βα(true);Vector3 Εζ=Βή(щ,Math.Max(Ц+1,Р+1));ǣ(true,false,false,Εζ,Εζ-щ,ǝ,ǝ);}bool ΓΣ=false;if(у==я.ΒΙ||у==я.ΒΘ)
{if(!ΕЈ){float ΓЀ=0;foreach(IMyTerminalBlock ǯ in ο)
ΓЀ+=ΗΒ(ǯ,"","",у==я.ΒΘ?new string[]{"STONE"}:null);if(ΓЀ>Γο||Р<Ц||Γκ){ΒЏ=0;ΑΜ=Р;ĩ=(int)(Math.Max(ĩ,ΑΜ)+θ/2);}else{ΓΣ=Р-ΑΜ>2&&ΒЏ>=20;ΒЏ++;}Γο=ΓЀ;}}else ΓΣ=Р>=ĩ;if(ȗ!=Й){Ν(Α.Ǹ);Р=0;return;}if(ΓΣ){Й++;Ν(Α.Ǹ);Р=0;return;}}if(Υ==Α.Ǹ||Υ==Α.đ||Υ==Α.ů)
{if(Ђ){щ=Ȍ(Ш,Щ,false);Х(Ε.β,Ε.γ,Ε.ψ,false);ђ(ο,Ň);Ǥ(false);к=Vector3.Distance(ί,щ);ē(false,true,0,θ*sensorRange);}П(щ,Βα(false));if(Vector3.Distance(ί,щ)>=к+5){ђ(ο,false);Ǥ(true);Μ="Can´t return!";}if(К<wpReachedDist){if(Υ==Α.Ǹ&&ΑЀ) Ν(Α.Ķ);if(Υ==Α.Ǹ) Ν(Α.ȋ);
if(Υ==Α.ů) Ν(Α.ƺ);if(Υ==Α.đ){if(Θ.δ) Ν(Α.ȥ);else{χ();ď();Μ="Can´t return, no dock found";}}return;}}if(Υ==Α.Ѕ||Υ==Α.Ю){if(Υ==Α.Ю&&ή==Κ.Ϗ&&Η!=Ζ.Ϋ){if(!ΒΜ()||Ŵ(true)){Ν(Α.Ѕ);return;}}bool ΓЉ=false;bool ΑЏ=false;bool ΕΣ=false;float Ł=0;bool ΖΟ=false;Ψ Ω=null;
if(Ђ){if(Υ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȏ(1)) ΒΕ();else{њ=null;ȅ=0;Ƌ=!ΒΝ(1);}}Ψ ά=е();ζ=new Βχ();ζ.Њ=ά;ζ.Ɖ=followPathDock*θ;ζ.ΔΡ=useDockDirectionDist*θ;ζ.Ł=10;ζ.ť.Add(ά.Δ);Vector3 ŀ=new Vector3();if(Ƕ(ά,dockDist*θ,true,out ŀ))
ζ.ť.Add(ŀ);else ζ.Ɖ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) ζ.Αέ=Ε.Δ;if(ά==Ε) ζ.Αέ=Θ.Δ;ζ.Γά=dockDist*θ*1.1f;}}else if(Υ==Α.Ю){ζ=new Βχ();ζ.Њ=Ε;ζ.Ɖ=followPathJob*θ;ζ.ΔΡ=useJobDirectionDist*θ;ζ.Ł=10;ζ.Αέ=Θ.Δ;ζ.Γά=dockDist*θ*1.1f;ζ.ť.Add(Ε.Δ);if(ή==Κ.з){if(!Θ.δ||Σ.Count==0){χ();return;}
float ΘΣ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΤ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΣ<ΘΤ) ζ.Њ=Σ.Last();else ζ.Њ=Σ.First();}}Ɯ=new Vector3();ΖΟ=!ΗΪ(ί);ђ(ο,false);Ǥ(true);У=-1;double ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ζ.Αέ)<=ζ.Γά) continue;
double ό=Vector3.Distance(Σ[i].Δ,ί);if(ǧ==-1||ό<ǧ){У=i;ǧ=ό;}}Ǭ=ΗΞ(ζ.Њ.Δ,У);ě=null;}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&!ż()){Ψ ΓΒ=е();Ƌ=!ΒΝ(1);if(е()!=ΓΒ){Ν(Α.Ѕ);return;}}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&ȏ(1)&&ΒΕ()){Ν(Α.Ѕ);return;}bool ΕІ=false;if(Υ==Α.Ѕ&&!Ђ&&У>=0&&У<Σ.Count)
{Vector3 Ƈ=Σ[У].Δ+Δυ(У,Ǭ);int ΓϏ=У+Ǭ;Vector3 ŗ=Ǭ!=0&&ΓϏ>=0&&ΓϏ<Σ.Count?Σ[ΓϏ].Δ:ζ.Њ.Δ;if(ΗΡ(Ƈ,ŗ)){if(!ΑΦ){ΑΦ=true;Ελ=ķ(Ƈ)?ί:Ƈ;}П(Ελ,0);Х(ζ.Њ.β,ζ.Њ.γ,ζ.Њ.ψ,false);Μ="Waiting for dock clearance";return;}ΑΦ=false;ΕІ=ȏ(1)&&(ķ(ί)||ķ(ŗ));}ΖІ(Σ,Ǭ,ζ.ť,ζ.Ɖ,Ђ,ref Ѓ);
for(int i=0;i<ζ.ť.Count;i++){float ό=Vector3.Distance(ί,ζ.ť[i]);if(ό<=ζ.Ɖ) ΓЉ=true;if(ό<=ζ.ΔΡ) ΑЏ=true;}if(ΕІ) ΑЏ=true;if(ΑЏ) Ł=ζ.Ł;float ΚΛ=ě!=null?ě.Џ:Ȝ;float ΙΨ=(float) Math.Max(Ȝ*0.1f*θ,wpReachedDist);if((К<ΙΨ)||Ђ){if(!Ђ) У+=Ǭ;if(Ǭ==0||У>Σ.Count-1||У<0)
ΓЉ=true;else{ΔΦ=Ǭ>0?Σ.Count-1-У:У;Ω=Σ[У];ě=Ω;if(У>=1&&У<Σ.Count-1)
Ɯ=Ω.Δ-Σ[У-Ǭ].Δ;else ě=null;φ=Ω.Δ+Δυ(У,Ǭ);ΕΣ=true;}}if(ΑЏ)
Х(ζ.Њ.β,ζ.Њ.γ,ζ.Њ.ψ,false);else if(ΖΟ)
ΒΔ(ζ.Њ.β,10,true);else if(ΕΣ&&Ω!=null){if(Ǭ>0) Х(Ω.β,Ω.γ,Ω.ψ,90,false);else Х(Ω.β,-Ω.γ,-Ω.ψ,90,false);}ǣ(true,false,true,φ,Ɯ,ě==null?0:ě.Џ,Ł);if(ΓЉ){ΔΦ=0;if(Υ==Α.Ѕ&&!ΖЊ()){П(φ,0);return;}if(Υ==Α.Ѕ&&Ƌ){П(φ,0);Μ="Waiting for a free dock";return;}if(Υ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.ǡ);return;}
if(Υ==Α.Ю&&ΑЀ){Ν(Α.Ķ);return;}if(Υ==Α.Ю){Ν(Α.ȋ);return;}}}if(Υ==Α.ǡ||Υ==Α.Ź){if(Η!=Ζ.Ϋ&&ż()){if(!ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΒ=е();if(!ΒΝ(2)||е()!=ΓΒ){Ν(Α.Ѕ);return;}}Ψ ά=е();if(Ђ){if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ŭ);χ();return;}
П(φ,0);ΒΔ(ά.β,90,true);}if(К<followPathDock*θ&&К!=-1){П(φ,10);Х(ά.β,ά.γ,ά.ψ,false);}if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null){Ν(Α.Ď);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.Ċ);return;}}if(Υ==Α.Ċ||Υ==Α.ȡ){if(Ђ)
{if(Υ==Α.Ċ){Ψ ά=е();if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ŭ);χ();return;}ǣ(true,true,false,φ,0);Х(ά.β,ά.γ,ά.ψ,10,false);}if(Υ==Α.ȡ){Х(Ε.β,Ε.γ,Ε.ψ,0.5f,false);φ=Ε.Δ;ǣ(true,true,false,φ,0);}}if(Αη){ǋ(false,0,0,0,0);if(Υ==Α.Ċ) Ν(Α.Ď);if(Υ==Α.ȡ) χ();return;}}if(Υ==Α.Ď)
{if(О(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.ȧ);else Ν(Α.ǻ);return;}Ψ ά=е();if(Ђ){Э=0;ΘΥ=DateTime.Now;Ѓ=0;Х(ά.β,ά.γ,ά.ψ,false);}Vector3I Γα=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Εή=Me.CubeGrid.GetCubeBlock(Γα);float Εν=dockingSpeed;float Θν=dockingSpeed*5;
float ΕΟ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ƕ(ά,0,true,out φ)||!Ƕ(ά,ΕΟ,true,out Ɯ)||
Εή==null||!Εή.FatBlock.IsFunctional){ǹ(а.ŭ);χ();return;}if(Э==1||(Vector3.Distance(ί,φ)<=ΕΟ*1.1f&&!Ђ))
Э=1;else{Vector3 ΙΘ=Я(Φ,Ɯ-ί);Vector3 Γΰ=Я(Φ,Φ.GetNaturalGravity());float ΘЄ=ƴ(ΙΘ,Γΰ,null);Εν=Math.Min(Θν,ΘЄ);}ǣ(true,false,false,φ,φ-ί,dockingSpeed,Εν);if(Ђ) к=(float) К;IMyShipConnector ΓΝ=О(MyShipConnectorStatus.Connectable);if(ΓΝ!=null){ǣ(false,false,false,φ,0);
if(Ѓ>0) Ѓ=0;Ѓ--;if(Ѓ<-5){ΓΝ.Connect();if(ΓΝ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.ȧ);else Ν(Α.ǻ);ΒЅ();Ż(Ȅ,true);return;}}}else{float Εκ=(float) Math.Round(К,1);if(Εκ<к){Ѓ=-1;к=Εκ;}else Ѓ++;if(Ѓ>20){Ν(Α.Ź);return;}}}if(Υ==Α.ǻ||Υ==Α.ȧ||
Υ==Α.ΑΗ||Υ==Α.ƹ||Υ==Α.č){bool ΒЍ=false,ΒЎ=false;if(Η==Ζ.Ϋ){if(е()==Θ) ΒЍ=true;else if(е()==Ε) ΒЎ=true;}if(Ђ){Ƣ=false;if(О(MyShipConnectorStatus.Connected)==null){Ν(Α.ǒ);return;}ΒЅ();if(ΒЍ) ΑΓ(ϊ.ǖ);if(ΒЎ) ΑΓ(ϋ.ǖ);ĭ=false;Ȑ=false;Ȕ=false;ƫ=false;}if(О(MyShipConnectorStatus.Connected)==null){χ();ǹ(а.Δώ);return;}
if(ή!=Κ.Ϗ||ч==-1||Ǧ==п.None) Ȑ=true;else if(Ǽ>=100f) Ȑ=true;else if(Ǽ<=99f) Ȑ=false;if(ή!=Κ.Ϗ||ш==-1||і.Count==0) Ȕ=true;else if(ȕ>=100f) Ȕ=true;else if(ȕ<=99) Ȕ=false;if(ή!=Κ.Ϗ||Ы==-1||Ą.Count==0) ƫ=true;else ƫ=Į>=Ы;Ľ о=null;if(ΒЍ) о=ϊ;
if(ΒЎ) о=ϋ;if(о!=null&&(о.Ќ==ι.Ǉ||о.Ќ==ι.ǆ)) Ȑ=true;if(о!=null&&(о.Ќ==ι.Βξ)) if(!ĭ) Ȑ=false;if(о!=null&&(о.Ќ==ι.Βς||о.Ќ==ι.Βπ)) Ȕ=true;if(о!=null&&(о.Ќ==ι.Βρ)) if(!ĭ) Ȕ=false;if(ā){ChargeMode Εά=Ȑ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ќ==ι.ǆ||о.Ќ==ι.Ǉ))
Εά=ChargeMode.Discharge;Βό(Εά);Βώ(!Ȕ);}if(!ĭ){if(Η==Ζ.Ϋ)
ĭ=ή!=Κ.Ϗ||ΖЋ(Ђ,true)||Ʃ;else
ĭ=ή!=Κ.Ϗ||ΔϏ();}else{if(!Ȑ) Ν(Α.č);if(!Ȕ) Ν(Α.ƹ);if(!ƫ) Ν(Α.ΑΗ);Ђ=false;}if(ĭ&&Ȑ&&Ȕ&&ƫ){Βό(ChargeMode.Auto);Βώ(false);if(ή==Κ.Ϗ){if(Η==Ζ.Ϋ){if(е()==Θ) ΑΓ(ϊ.Ǚ);else if(е()==Ε) ΑΓ(ϋ.Ǚ);if(е()==Θ) ĺ(Ε,Κ.ĳ);else ĺ(Θ,Κ.Ĳ);}}Ν(Α.ǒ);return;}}if(Υ==Α.ǒ){if(Ʈ){if(!Δε()) return;
Ʈ=false;Ν(Α.ǒ);return;}if(Ђ){if(!Δε()){Ʈ=true;return;}IMyShipConnector Ȁ=О(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Ю);return;}IMyShipConnector α=Ȁ.OtherConnector;ΔЏ(Ȁ,false);Ż(Ȅ,false);Ψ ά=null;Ψ Γθ=Η==Ζ.Ϋ?Θ:е();if(Vector3.Distance(Ȁ.GetPosition(),Γθ.Δ)<5f&&Θ.δ) ά=Γθ;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.δ) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ŭ);χ();return;}П(φ,5);Х(ά.β,ά.γ,ά.ψ,false);}else П(ί+α.WorldMatrix.Forward*dockDist*θ,5);if(ή==Κ.Ϗ) ǹ(а.Ǌ);}if(К<wpReachedDist){ђ(т,true);Ν(Α.Ю);return;}}if(Υ==Α.Ķ)
{if(Ђ){Ǥ(true);ђ(ο,false);φ=Ε.Δ;П(φ,20);Х(Ε.β,Ε.γ,Ε.ψ,false);}if(К<wpReachedDist/2){Ν(Α.ȡ);return;}}if(Υ==Α.ȥ){if(Ђ){Ǥ(true);ђ(ο,false);int Б=-1;double ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ί);if(ǧ==-1||ό<ǧ){Б=i;ǧ=ό;}}
if(Б==-1){Ν(Α.Ѕ);return;}φ=Σ[Б].Δ;П(φ,10);Х(Ε.β,Ε.γ,Ε.ψ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Υ==Α.ƺ){bool ф=false;if(Ђ){Ǥ(true);if((Л==Ѐ.ΒΥ||Л==Ѐ.Ʒ)&&ǀ()&&
ĵ(ǜ,Φ.GetNaturalGravity())<25&&М>=2&&б>=2){Vector3 ƍ=ί;if(Ш>0&&Щ<б-1) ƍ=Ȍ(Ш-1,Щ+1,true);else if(Ш<М-1&&Щ<б-1) ƍ=Ȍ(Ш+1,Щ+1,true);else if(Ш<М-1&&Щ>0) ƍ=Ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) ƍ=Ȍ(Ш-1,Щ-1,true);else ф=true;if(!ф) П(ƍ,10);
}else ф=true;}if(К<wpReachedDist/2) ф=true;if(ф){Ν(Α.ΑΖ);return;}}if(Υ==Α.ΒЈ||Υ==Α.ΑΖ){if(Ђ){ǣ(true,true,false,ί,0);ђ(ο,false);Ǥ(true);Ѓ=-1;Э=Л==Ѐ.Βζ||Л==Ѐ.ƾ?0:-1;}bool ĝ=!ΒΜ();int ΑЇ=Ȋ("STONE","ORE",З.ċ);if(Л==Ѐ.ΒΞ||Л==Ѐ.ƾ||Л==Ѐ.Ʒ)
ΑЇ+=Ȋ("ICE","ORE",З.ċ);bool Θά=ΑЇ>0;bool ΓΫ=false;if(Э>=0){float ľ=(float) Math.Sin(Δι(Э))*Ǳ/3f;float ΘΑ=(float) Math.Cos(Δι(Э))*ǰ/3f;Vector3 ΘΗ=Ȍ(Ш,Щ,true)+ΒІ(ǜ,ș*-1,new Vector3(ľ,ΘΑ,0));П(ΘΗ,0.3f);if(К<Math.Min(Ǳ,ǰ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ѓ==-1||ΑЇ<Ѓ){Ѓ=ΑЇ;к=0;}
else{к++;if(к>50) ΓΫ=true;}if(!Θά||ĝ||ΓΫ){if(!ĝ){int ΙΝ=Ȋ("","ORE",З.ċ);if(Ŵ(true)) ĝ=true;else if(100-(А(ΙΝ,ΖΠ)*100)<minEjection) ĝ=true;else ǹ(а.Ǌ);}if(ΓΫ&&ĝ) Μ="Ejection failed";if(Υ==Α.ΑΖ){if(ĝ){if(Θ.δ) Ν(Α.ȥ);else{χ();ď();Μ="Can´t return, no dock found";}}
else Ν(Α.ȋ);}else if(ĝ) Ν(Α.đ);else Ν(Α.ĸ);return;}}Ђ=false;}void ΖІ(List<Ψ>ĉ,int κ,List<Vector3>Ʀ,float ǭ,bool Ȇ,ref int ǔ){if(Ȇ){for(int i=0;i<ĉ.Count;i++) ĉ[i].Џ=0;ǔ=-1;return;}if(κ==0) return;int ѝ=κ*-1;if(ǔ==-1) ǔ=ѝ>0?1:ĉ.Count-2;int ΕЊ=0;while(ǔ>=1&&ǔ<ĉ.Count-1)
{if(ΕЊ>50) return;ΕЊ++;try{if((ѝ<0&&ǔ>=1)||(ѝ>0&&ǔ<=ĉ.Count-2)){Ψ η=ĉ[ǔ];bool ΖΝ=false;for(int j=0;j<Ʀ.Count;j++)
if(Vector3.Distance(η.Δ,Ʀ[j])<=ǭ){ΖΝ=true;break;}if(!ΖΝ){Ψ Αό=ĉ[ǔ-ѝ];Ψ ŗ=ĉ[ǔ+ѝ];Vector3 ΙΙ=η.Δ-ŗ.Δ;Vector3 ΔΜ=Αό.Δ-η.Δ;Vector3 Θώ=η.Δ+Vector3.Normalize(ΙΙ)*ΔΜ.Length();Vector3 ΘΡ=Αό.Δ-Θώ;Vector3 Αν=Ē(κ>0?η.γ:η.γ*-1,η.β*-1,ΘΡ);Vector3 Θμ=Ē(κ>0?η.γ:η.γ*-1,η.β*-1,ΔΜ);
Vector3 Αο=Ē(κ>0?η.γ:η.γ*-1,η.β*-1,η.ν);η.Џ=(float) Math.Sqrt(Math.Pow(Αό.Џ,2)+Math.Pow(ƴ(-Θμ,Αο,η),2));for(int j=0;j<Ʀ.Count;j++)
if(Vector3.Distance(Αό.Δ,Ʀ[j])<=ǭ){Vector3 Απ=Ē(κ>0?η.γ:η.γ*-1,η.β*-1,Ʀ[j]-η.Δ);float ΙΑ=ƴ(-Απ,Αο,η);η.Џ=Math.Min(η.Џ,ΙΑ)/2f;}if(Αν.Length()==0) Αν=new Vector3(0,0,1);Vector3 Θλ=Ē(η.γ,η.β*-1,η.ν);float ΙΓ=Ű(-Αν,Θλ,η);float ΑΘ=А(ΙΓ,ī);float ΔΚ=(float) Math.Sqrt(А(Αν.Length()*1.0f,0.5f*ΑΘ));
η.Џ=Math.Min(η.Џ,(ΔΜ.Length()/ΔΚ)*ǥ);}}}catch{return;}ǔ+=ѝ;}ǔ=-1;}void ΒЇ(bool Ȇ){if(Ȇ){Ɠ=0;return;}if(Η==Ζ.Ϋ) return;float ф=Й*Math.Max(1,Ц);if(ȗ==Й) ф+=Math.Min(Ц,Р);float ΙΜ=М*б*Math.Max(1,Ц);Ɠ=Math.Max(Ɠ,(float) Math.Min(ф/ΙΜ*100.0,100));}MyDetectedEntityType Δτ()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βα(bool Ǫ){if(Η==Ζ.Ǐ&&Δτ()==MyDetectedEntityType.None&&!ų(ξ,true))
return fastSpeed;else return Ǫ?ū:Ū;}void ΗЀ(){float ΖΗ=0,Εδ=0,ģ=0,ΓЁ=0;for(int i=0;i<Ȩ.Count;i++){IMyBatteryBlock ƃ=Ȩ[i];if(!Ƿ(ƃ,true)) continue;ΖΗ+=ƃ.MaxStoredPower;Εδ+=ƃ.CurrentStoredPower;ģ+=ƃ.CurrentInput;ΓЁ+=ƃ.CurrentOutput;}Ǽ=(float) Math.Round(А(Εδ,ΖΗ)*100,1);
if(ģ>=ΓЁ) Ǧ=п.č;else Ǧ=п.Δκ;if(ģ==0&&ΓЁ==0||Ǽ==100.0) Ǧ=п.ж;if(Ȩ.Count==0) Ǧ=п.None;}void ΗЄ(){float Εσ=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζο=і[i];if(!Ƿ(Ζο,true)) continue;Εσ+=(float) Ζο.FilledRatio;}ȕ=А(Εσ,і.Count)*100f;}void ΗЌ(){Į=0;try{for(int i=0;i<Ą.Count;i++)
{IMyReactor ΓЊ=Ą[i];if(!Ƿ(ΓЊ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЊ.GetInventory(0).GetItems(Ъ);float Ǔ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǔ+=(float) Г.Amount;}if(Ǔ<Į||i==0){Į=Ǔ;Γφ=ΓЊ.CustomName;}}}catch(Exception e){Ȗ=e;}}void ΗЁ(){ŕ=0;ņ=0;try{for(int i=0;i<Ņ.Count;i++){IMyTerminalBlock Τ=Ņ[i];if(!Ƿ(Τ,true)) continue;ņ+=(float) Τ.GetInventory(0).CurrentVolume;ŕ+=(float) Τ.GetInventory(0).MaxVolume;
}Αμ=(float) Math.Min(Math.Round(А(ņ,ŕ)*100,1),100.0);}catch(Exception e){Ȗ=e;}}void ΗЅ(){try{ǘ.Clear();for(int i=0;i<Ņ.Count;i++){IMyTerminalBlock Τ=Ņ[i];if(!Ƿ(Τ,true)) continue;Δέ(Τ,З.Ų);}if(Л!=Ѐ.Βν){for(int i=0;i<Ė.Count;i++){IMyTerminalBlock Τ=Ė[i];if(!Ƿ(Τ,true)) continue;
Δέ(Τ,З.ΗΦ);}}Ηψ(ǘ);}catch(Exception e){Ȗ=e;}}void ΖЉ(){if(ΑΣ){if(Δσ().Count>Αε){ΑΣ=false;if(л!=д.ű){χ();if(л==д.ΒΪ) ď();if(л==д.ǉ)
if(Θ.δ) ƻ();else ď();}Μ="Damage detected";}}}bool ΒΜ(){if(!Αζ) return true;if(ή==Κ.Ϗ){if(ч>0&&Ǧ!=п.None)
if(Ǽ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&Ą.Count>0)
if(Į<=Ы){Μ="Low fuel: "+Γφ;return false;}if(ш>0&&і.Count>0)
if(ȕ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δσ(){List<IMyTerminalBlock>Ĝ=new List<IMyTerminalBlock>();for(int i=0;i<ſ.Count;i++){IMyTerminalBlock Τ=ſ[i];if(ų(Τ,false)) Ĝ.Add(Τ);}return Ĝ;}bool ų(IMyTerminalBlock Τ,bool ΔΔ){return(!Ƿ(Τ,ΔΔ)||!Τ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Τ,bool ΔΔ){if(Τ==null) return false;try{IMyCubeBlock Εϊ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΔΔ) return Εϊ==Τ;else return Εϊ.GetType()==Τ.GetType();}catch{return false;}}float ΗΒ(IMyTerminalBlock Τ,String ю,String Ζζ,String[]ǩ){float ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ŏ=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Ŏ.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(ǩ!=null&&(ǩ.Contains(Г.Type.TypeId.ToUpper())||ǩ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Г.Type.TypeId.ToUpper()==ю)&&(Ζζ==""||Г.Type.SubtypeId.ToUpper()==Ζζ))
ȁ+=(float) Г.Amount;}}return ȁ;}ѐ ΗΙ(String υ,String ς,З Д,bool ΘΝ){υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<ǘ.Count;i++){ѐ Г=ǘ[i];if(Г.υ.ToUpper()==υ&&Г.ς.ToUpper()==ς&&(Г.Œ==Д||Д==З.ċ))
return Г;}ѐ έ=null;if(ΘΝ){έ=new ѐ(υ,ς,0,Д);ǘ.Add(έ);}return έ;}int Ȋ(String υ,String ς,З Д){return Ȋ(υ,ς,Д,null);}int Ȋ(String υ,String ς,З Д,String[]ǩ){int ȁ=0;υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<ǘ.Count;i++){ѐ Г=ǘ[i];if(ǩ!=null&&ǩ.Contains(Г.υ.ToUpper())) continue;
if((υ==""||Г.υ.ToUpper()==υ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.Œ==Д||Д==З.ċ))
ȁ+=Г.Ǔ;}return ȁ;}void Δέ(IMyTerminalBlock Τ,З Д){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΙ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).Ǔ+=(int) Ъ[j].Amount;}}void Ηψ(List<ѐ>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){ѐ a=Т[j];ѐ b=Т[j+1];if(a.Ǔ<b.Ǔ) Т.Move(j,j+1);}}bool Ŵ(bool ΙА){if(Ƞ&&Η!=Ζ.Ϋ)
if(Н!=-1&&ī>=Н){Μ="Ship too heavy";return true;}if(Αμ>=ǫ||Ƣ){Ƣ=false;Μ="Ship is full";return true;}return false;}bool ΔϏ(){String[]ǩ=null;if(!ȟ) ǩ=new string[]{"ICE"};if(Η==Ζ.Ÿ) return Ȋ("","ORE",З.Ų,ǩ)==0;if(Η==Ζ.Ǐ) return Ȋ("","COMPONENT",З.Ų,ǩ)==0;else return Ȋ("","",З.Ų,ǩ)==0;
}bool ΗΫ(){List<IMyCockpit>ΓΜ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΜ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΜ.Count;i++)
if(ΓΜ[i].IsUnderControl) return true;return false;}bool ΖЋ(bool Ȇ,bool ΑЃ){IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δζ(ϊ,Ȇ,ΑЃ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δζ(ϋ,Ȇ,ΑЃ);return false;}bool Δζ(Ľ ǿ,bool Ȇ,bool ΑЃ){if(Ȇ) ǿ.Ηθ();ǿ.ΗЉ();bool н=false;String ь="";switch(ǿ.Ќ){case ι.Βο:{ь="Waiting for command";н=false;break;}case ι.ΔЊ:{ь="Waiting for cargo";н=Ŵ(true);break;}case ι.ΔЉ:{ь="Unloading";н=ΔϏ();break;}case ι.Βτ:{н=true;break;}
case ι.Βξ:{ь="Charging batteries";н=Ǽ>=100f;break;}case ι.Ǉ:{ь="Discharging batteries";н=Ǽ<=25f;break;}case ι.ǆ:{ь="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βρ:{ь="Filling up hydrogen";н=ȕ>=100f;break;}case ι.Βς:{ь="Unloading hydrogen";н=ȕ<=25f;break;}
case ι.Βπ:{ь="Unloading hydrogen";н=ȕ<=0f;break;}case ι.Βσ:{bool ΕЅ=ΗΫ();if(!ΕЅ) ǿ.ΓІ=true;н=ǿ.ΓІ&&ΕЅ;ь="Waiting for passengers";break;}}if(!н) ǿ.ΒΛ(true);if(н&&ǿ.Βγ()){н=ǿ.ΒΛ(false);ь="Undocking in: "+Βά((int) Math.Max(0,ǿ.Ǖ-ǿ.Ȯ));}if(ΑЃ) Ζώ=ь;return н;}void ΖЂ()
{if(!ŏ) return;if(ο.Count<=1) return;float Ζφ=0,Ζυ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǯ=ο[i];if(ų(ǯ,true)) continue;Ζφ+=(float) ǯ.GetInventory(0).MaxVolume;Ζυ+=(float) ǯ.GetInventory(0).CurrentVolume;}float ΕΤ=(float) Math.Round(А(Ζυ,Ζφ),5);for(int ΖΥ=0;ΖΥ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΥ++)
{float Γζ=0,Γυ=0;float ΕЂ=0,ΖΕ=0;IMyTerminalBlock ȓ=null,ȯ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǯ=ο[i];if(ų(ǯ,true)) continue;float Γό=(float) ǯ.GetInventory(0).MaxVolume;float Αή=А((float) ǯ.GetInventory(0).CurrentVolume,Γό);if(ȓ==null||Αή>Γζ){ȓ=ǯ;Γζ=Αή;ΕЂ=Γό;}
if(ȯ==null||Αή<Γυ){ȯ=ǯ;Γυ=Αή;ΖΕ=Γό;}}if(ȓ==null||ȯ==null||ȓ==ȯ) return;if(checkConveyorSystem&&!ȓ.GetInventory(0).IsConnectedTo(ȯ.GetInventory(0))){if(ΓΑ>20) Μ="Inventory balancing failed";else ΓΑ++;return;}ΓΑ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȓ.GetInventory(0).GetItems(Ъ);float ƭ=0;if(Ъ.Count==0) continue;MyInventoryItem Αί=Ъ[0];String ΕЉ=Αί.Type.TypeId+Αί.Type.SubtypeId;if(!Ζύ.TryGetValue(ΕЉ,out ƭ)){if(Ηί(ȓ.GetInventory(0),0,ȯ.GetInventory(0),out ƭ))
Ζύ.Add(ΕЉ,ƭ);else return;}float ΙΚ=((Γζ-ΕΤ)*ΕЂ/ƭ);float ΙΛ=((ΕΤ-Γυ)*ΖΕ/ƭ);int ΔΟ=(int) Math.Min(ΙΛ,ΙΚ);if(ΔΟ<=0) return;if((float) Αί.Amount<ΔΟ)
ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,Αί.Amount);else
ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΔΟ);}}bool Ηί(IMyInventory ƣ,int Β,IMyInventory ΘΠ,out float Ζό){Ζό=0;float ΘΔ=(float) ƣ.CurrentVolume;List<MyInventoryItem>ΓΓ=new List<MyInventoryItem>();ƣ.GetItems(ΓΓ);float ΕΧ=0;for(int i=0;i<ΓΓ.Count;i++) ΕΧ+=(float) ΓΓ[i].Amount;
ƣ.TransferItemTo(ΘΠ,Β,null,null,1);float Ζϋ=ΘΔ-(float) ƣ.CurrentVolume;List<MyInventoryItem>ΒЊ=new List<MyInventoryItem>();ƣ.GetItems(ΒЊ);float ΕΛ=0;for(int i=0;i<ΒЊ.Count;i++) ΕΛ+=(float) ΒЊ[i].Amount;if(Ζϋ==0f||!Βη(0.9999,ΕΧ-ΕΛ,1.0001))
return false;Ζό=Ζϋ;return true;}void ΗЃ(){Vector3 Ɛ=φ+ć;Vector3 Ǯ=Ɛ-ί;if(Ǯ.Length()==0) Ǯ=new Vector3(0,0,-1);Vector3 Απ=Я(Φ,Ǯ);Vector3 Ζπ=Vector3.Normalize(Απ);Vector3 ő=Я(Φ,Φ.GetNaturalGravity());float ΘЍ=ΓЄ>0?Math.Max(0,1-ĵ(Ǯ,Ευ)/5):0;float Џ=(float) Math.Min((Γω>0?Γω:1000f),Math.Max(ƴ(-Απ,ő,null),ΓЄ*ΘЍ));
if(!Ζτ) Џ=0;if(Ζΰ) Џ=Math.Max(0,1-ΑΠ/ΑΙ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(ś) Џ=0;if(э>=0) Џ=Math.Min(Џ,э);if(Ζα) Џ*=(float) Math.Min(1,А(Ǯ.Length(),wpReachedDist)/2);Vector3 ΖΒ=Я(Φ,Φ.GetShipVelocities().LinearVelocity);float ΗБ=(float)(Math.Max(0,15-ĵ(-Ζπ,-ΖΒ))/15)*0.85f+0.15f;
ΔΘ+=Math.Sign(ΗБ-ΔΘ)/10f;Vector3 Ȫ=Ζπ*Џ*ΔΘ-(ΖΒ);Vector3 ΒБ=Βΰ(Ȫ,null);if(ΑЉ&&К>0.1f){Ȫ.X*=ΒЀ(Ȫ.X,ref ΑЊ.X,1f,ΒБ.X,20);Ȫ.Y*=ΒЀ(Ȫ.Y,ref ΑЊ.Y,1f,ΒБ.Y,20);Ȫ.Z*=ΒЀ(Ȫ.Z,ref ΑЊ.Z,1f,ΒБ.Z,20);}else ΑЊ=new Vector3(1,1,1);Ƨ=ī*Ȫ-ő*ī;Δί(Ƨ,ΑЉ);К=Vector3.Distance(ί,Ɛ);
}float ΒЀ(float value,ref float Ф,float ѝ,float Ș,float Γϊ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ѝ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γϊ||Ș==0) return έ;Ф=Math.Min(Γϊ,Math.Max(-Γϊ,Ф));έ=Math.Abs(Ș);return έ;
}void ΕΙ(){float ы=90,ǎ=90,ќ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǫ,ј,Ɨ,Γς;if(Γσ){Ǫ=Vector3.Normalize(φ-ί);Ɨ=Я(Φ,Ǫ);ј=Я(Φ,ΔΖ);ы=ĵ(Ɨ,new Vector3(0,-1,0))-90;ǎ=Č(ј,new Vector3(-1,0,0),ј.Y);
ќ=Č(Ɨ,new Vector3(-1,0,0),Ɨ.Z);}else{Ǫ=ΔΗ;ј=Я(Φ,ΔΖ);Ɨ=Я(Φ,ΔΗ);Γς=Я(Φ,Ζρ);ы=Č(ј,new Vector3(0,0,1),ј.Y);ǎ=Č(ј,new Vector3(-1,0,0),ј.Y);ќ=Č(Γς,new Vector3(0,0,1),Γς.X);}if(ΕΝ&&ǀ()){Vector3 Γΰ=Φ.GetNaturalGravity();ј=Я(Φ,Γΰ);ы=Č(ј,new Vector3(0,0,1),ј.Y);
ǎ=Č(ј,new Vector3(-1,0,0),ј.Y);}if(!Βη(-45,ǎ,45)){ы=0;ќ=0;}if(!Βη(-45,ќ,45)) ы=0;ǋ(Γβ,1,(-ы)*ǝ,(-ќ)*ǝ,(-ǎ)*ǝ);ΑΠ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(ǎ),Math.Abs(ќ)));Αη=ΑΠ<=ΑΙ;}void ΖЇ(IMyTerminalBlock Αύ){if(Αύ==null) return;Ǡ=new float[3,2];ў=new Dictionary<string,float[,]>();
for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(Αύ,в.WorldMatrix.Backward);float ȭ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) Ǡ[0,0]+=ȭ;else Ǡ[0,1]-=ȭ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) Ǡ[1,0]+=ȭ;else Ǡ[1,1]-=ȭ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) Ǡ[2,0]+=ȭ;else Ǡ[2,1]-=ȭ;String ю=Δχ(в);float[,]џ=null;if(ў.ContainsKey(ю)) џ=ў[ю];else{џ=new float[3,2];ў.Add(ю,џ);}float Ș=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) џ[0,0]+=Ș;else џ[0,1]-=Ș;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) џ[1,0]+=Ș;else џ[1,1]-=Ș;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) џ[2,0]+=Ș;else џ[2,1]-=Ș;}}static String Δχ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βι(Vector3 ѕ,float[,]œ){return new Vector3(
ѕ.X>=0?œ[0,0]:œ[0,1],
ѕ.Y>=0?œ[1,0]:œ[1,1],
ѕ.Z>=0?œ[2,0]:œ[2,1]);}bool Δφ(Ψ Ω,String ю,out float ΓΪ){ΓΪ=0;int Β=ȇ.IndexOf(ю);if(Β==-1||Ω.ǟ==null||Β>=Ω.ǟ.Length)
return false;ΓΪ=Ω.ǟ[Β];if(ΓΪ==-1) return false;return true;}Vector3 Βΰ(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<ў.Keys.Count;i++){String ю=ў.Keys.ElementAt(i);float Ǝ=0;if(!Δφ(Ω,ю,out Ǝ))
return Βι(ѕ,Ǡ);έ+=Βι(ѕ,ў.Values.ElementAt(i))*Ǝ;}return έ;}return Βι(ѕ,Ǡ);}float Ű(Vector3 ѕ,Ψ Ω){return Ű(ѕ,new Vector3(),Ω);}float Ű(Vector3 ѕ,Vector3 ν,Ψ Ω){Vector3 ΙΒ=Βΰ(ѕ,Ω);Vector3 ȭ=ΙΒ+ν*ī;float Ɲ=(ȭ/ѕ).AbsMin();return(float)(ѕ*Ɲ).Length();}float ƴ(Vector3 Αξ,Vector3 ő,Ψ Ω)
{if(Αξ.Length()==0) return 0;float ΕΜ=1;if(ő.Length()>0) ΕΜ=Math.Min(1,ĵ(-ő,Αξ)/90)*0.4f+0.6f;float Ƨ=Ű(Αξ,ő,Ω);if(Ƨ==0) return 0.1f;float ΑΘ=А(Ƨ,ī);float ΔΚ=(float) Math.Sqrt(А(Αξ.Length(),ΑΘ*0.5f));return ΑΘ*ΔΚ*ΕΜ*ǥ;}float ƽ(Vector3 Ǫ,Vector3 ώ,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Αο=Ē(Ǫ,ώ,Vector3.Normalize(ν));float Ǘ=Ű(-Αο,Ω);return Ǘ/ν.Length();}void Δβ(bool Θβ){float Ǎ=0;float Γέ=0.9f;if(Θβ){Н=-1;İ=0;ΑЅ=null;if(ή!=Κ.з&&Ε.ν.Length()!=0){Ǎ=Γέ*ƽ(Ε.γ,Ε.β*-1,Ε.ν,null);if(Ǎ<Н||Н==-1) Н=Ǎ;}if(Θ.δ&&Θ.ν.Length()!=0)
{Ǎ=Γέ*ƽ(Θ.γ,Θ.β*-1,Θ.ν,null);if(Ǎ<Н||Н==-1) Н=Ǎ;}return;}if(İ==-1) return;if(İ>=0){int ΖΨ=0;while(İ<Σ.Count){if(ΖΨ>100) return;ΖΨ++;Ψ Ω=Σ[İ];if(Ω.ν.Length()!=0f){Ǎ=Γέ*Math.Min(
ƽ(Ω.γ,Ω.β*-1,Ω.ν,Ω),
ƽ(Ω.γ*-1,Ω.β*-1,Ω.ν,Ω));if(Ǎ<Н||Н==-1) Н=Ǎ;}else ΑЅ=Ω;İ++;}İ=-1;}bool Γγ=true;float ă=0;if(ΑЅ!=null){for(int i=0;i<ў.Count;i++){String ю=ў.Keys.ElementAt(i);float[,]џ=ў.Values.ElementAt(i);float Ǝ=0;if(!Δφ(ΑЅ,ю,out Ǝ)){Γγ=false;break;}for(int a=0;a<џ.GetLength(0);a++)
for(int b=0;b<џ.GetLength(1);b++){float ĕ=Math.Abs(џ[a,b]*Ǝ);if(ĕ==0) continue;Γγ=true;if(ă==0||ĕ<ă) ă=ĕ;}}}if(!Γγ){for(int a=0;a<Ǡ.GetLength(0);a++)
for(int b=0;b<Ǡ.GetLength(1);b++){float ĕ=Math.Abs(Ǡ[a,b]);if(ĕ==0) continue;if(ă==0||ĕ<ă) ă=ĕ;}}if(ă>0){float Θο=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǎ=А(ă,Θο);if(Ǎ>0)
if(Ǎ<Н||Н==-1) Н=Ǎ;}}void Δί(Vector3 Ǘ,bool Ј){if(!Ј){for(int i=0;i<Ȉ.Count;i++)
Ȉ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒА=Βΰ(Ǘ,null);float ΙΪ=Math.Min(1,Math.Abs(А(Ǘ.X,ΒА.X)));float ΙΫ=Math.Min(1,Math.Abs(А(Ǘ.Y,ΒА.Y)));float Ιά=Math.Min(1,Math.Abs(А(Ǘ.Z,ΒА.Z)));for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];Vector3 κ=Ηκ(Я(Φ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(Ǘ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΪ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(Ǘ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΫ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(Ǘ.Z))
в.SetValueFloat("Override",в.MaxThrust*Ιά);else
в.SetValueFloat("Override",0.0f);}}void ǋ(bool Ј,float Θϋ,float ы,float ќ,float ǎ){for(int i=0;i<Αα.Count;i++){IMyGyro І=Αα[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=Θϋ;if(!Ј) continue;Vector3 В=Φ.WorldMatrix.Forward;Vector3 Ь=Φ.WorldMatrix.Right;Vector3 ώ=Φ.WorldMatrix.Up;
Vector3 ŉ=І.WorldMatrix.Forward;Vector3 ŋ=І.WorldMatrix.Up;Vector3 Ŋ=І.WorldMatrix.Left*-1;if(ŉ==В) І.SetValueFloat("Roll",ǎ);else if(ŉ==(В*-1)) І.SetValueFloat("Roll",ǎ*-1);else if(ŋ==(В*-1)) І.SetValueFloat("Yaw",ǎ);else if(ŋ==В) І.SetValueFloat("Yaw",ǎ*-1);else if(Ŋ==В) І.SetValueFloat("Pitch",ǎ);
else if(Ŋ==(В*-1)) І.SetValueFloat("Pitch",ǎ*-1);if(Ŋ==(Ь*-1)) І.SetValueFloat("Pitch",ы);else if(Ŋ==Ь) І.SetValueFloat("Pitch",ы*-1);else if(ŋ==Ь) І.SetValueFloat("Yaw",ы);else if(ŋ==(Ь*-1)) І.SetValueFloat("Yaw",ы*-1);else if(ŉ==(Ь*-1)) І.SetValueFloat("Roll",ы);
else if(ŉ==Ь) І.SetValueFloat("Roll",ы*-1);if(ŋ==(ώ*-1)) І.SetValueFloat("Yaw",ќ);else if(ŋ==ώ) І.SetValueFloat("Yaw",ќ*-1);else if(Ŋ==ώ) І.SetValueFloat("Pitch",ќ);else if(Ŋ==(ώ*-1)) І.SetValueFloat("Pitch",ќ*-1);else if(ŉ==ώ) І.SetValueFloat("Roll",ќ);else if(ŉ==(ώ*-1)) І.SetValueFloat("Roll",ќ*-1);
}}void ΕΓ(){this.Γβ=false;}void Х(Vector3 Ȭ,Vector3 В,Vector3 Γρ,float Ȟ,bool Ƭ){ΒΔ(Ȭ,Ȟ,Ƭ);ΑΙ=Ȟ;Γσ=false;this.ΔΗ=В;this.Ζρ=Γρ;}void Х(Vector3 Ȭ,Vector3 В,Vector3 Γρ,bool Ƭ){Х(Ȭ,В,Γρ,2f,Ƭ);}void ΒΔ(Vector3 Ȭ,float Ȟ,bool Ƭ){ΑΙ=Ȟ;this.Γβ=true;this.ΕΝ=Ƭ;Γσ=true;Αη=false;
this.ΔΖ=Ȭ;}void ΒЅ(){ǣ(false,false,false,φ,0);ΑЉ=false;}void П(Vector3 Њ,float ǝ){ǣ(true,false,false,Њ,ǝ);}void ǣ(bool Ј,bool ΔΑ,bool ΔΣ,Vector3 Њ,float ǝ){ǣ(Ј,ΔΑ,ΔΣ,Њ,Њ-ί,0.0f,ǝ);}void ǣ(bool Ј,bool ΔΑ,bool ΔΣ,Vector3 Њ,Vector3 ΘЇ,float ΘЈ,float ǝ){ΑЉ=true;this.Ζτ=Ј;
φ=Њ;this.Γω=ǝ;this.ΓЄ=ΘЈ;this.Ζα=ΔΑ;this.Ζΰ=ΔΣ;this.Ευ=ΘЇ;К=Vector3.Distance(Њ,ί);}void Ηλ(){ω.GetBlocksOfType(ſ,ǐ);}void Ημ(){if(ΑϏ){ύ=2;return;}List<IMyRemoteControl>ƞ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζέ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζθ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(ƞ,ǐ);ω.GetBlocksOfType(ї,ǐ);ω.GetBlocksOfType(Ζέ,ǐ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζθ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=Ǣ(ї,pamTag,true);Ησ();Ητ(Ζθ);ź(ї,setLCDFontAndSize,1.4f,false);ź(Ć,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζμ=Ǣ(Ζέ,pamTag,true);if(Ζμ.Count>0) ξ=Ζμ[0];else ξ=null;if(Η==Ζ.Ÿ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.Ǐ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ω.GetBlocksOfType(ΔΛ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζλ=Ǣ(ƞ,pamTag,true);if(Ζλ.Count>0) ƞ=Ζλ;if(ƞ.Count>0) Φ=ƞ[0];else{Φ=null;ύ=2;Μ="Remote is missing";return;}θ=(float) Φ.CubeGrid.WorldVolume.Radius*2;ћ=null;if(Η!=Ζ.Ϋ){Δγ();if(ο.Count>0&&ћ!=null){if(ξ!=null&&(ћ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(ћ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ћ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ην(){ω.GetBlocksOfType(Ȅ,ǐ);ω.GetBlocksOfType(т,ǐ);ω.GetBlocksOfType(Ȉ,ǐ);ω.GetBlocksOfType(Αα,ǐ);ω.GetBlocksOfType(Ȩ,ǐ);ω.GetBlocksOfType(Ą,ǐ);ω.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑЄ,ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=Ǣ(т,"ConnectorMedium",false);else
т=Ǣ(т,"Connector",false);List<IMyShipConnector>Ζι=Ǣ(т,pamTag,true);if(Ζι.Count>0) т=Ζι;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(Αα.Count==0){ύ=1;Μ="Gyros are missing";}if(Ȉ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζν=Ǣ(ΑЄ,pamTag,true);
if(Ζν.Count>0) ΑЄ=Ζν;List<IMyLandingGear>Ζκ=Ǣ(Ȅ,pamTag,true);if(Ζκ.Count>0) Ȅ=Ζκ;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζη=Ǣ(Ȩ,pamTag,true);if(Ζη.Count>0) Ȩ=Ζη;List<IMyGasTank>Ζξ=Ǣ(і,pamTag,true);if(Ζξ.Count>0) і=Ζξ;}void Ηξ(){ω.GetBlocksOfType(Ė,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ņ.Clear();for(int i=Ė.Count-1;i>=0;i--){if(ΗΨ(Ė[i])){Ņ.Add(Ė[i]);Ė.RemoveAt(i);}}}bool ΗΨ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βδ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δγ(){if(Φ==null) return;ћ=null;float Αχ=0,Ας=0,Αψ=0,Ασ=0,Αω=0,Ατ=0;List<IMyTerminalBlock>ΔΕ=Ǣ(ο,pamTag,true);bool Θφ=ΔΕ.Count==0;if(ΔΕ.Count>0) ћ=ΔΕ[0];else if(ο.Count>0) ћ=ο[0];int ȁ=0;float Ŝ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward){if(Θφ){ύ=2;Μ="Mining direction is unclear!";return;}continue;}ȁ++;Ŝ=Math.Max(Ŝ,Ηώ(ο[i]));Vector3 Љ=Ƽ(Φ,ο[i].GetPosition());if(i==0){Αχ=Љ.X;Ας=Љ.X;Αψ=Љ.Y;Ασ=Љ.Y;Αω=Љ.Z;Ατ=Љ.Z;}Ας=Math.Max(Љ.X,Ας);Αχ=Math.Min(Љ.X,Αχ);
Ασ=Math.Max(Љ.Y,Ασ);Αψ=Math.Min(Љ.Y,Αψ);Ατ=Math.Max(Љ.Z,Ατ);Αω=Math.Min(Љ.Z,Αω);}if(Ŝ==0) Ŝ=drillRadius;bool Ȭ=ћ!=null&&ћ.WorldMatrix.Forward==Φ.WorldMatrix.Down;Ǳ=(Ας-Αχ)*(1-ı)+Ŝ*2;ǰ=(Ȭ?Ατ-Αω:Ασ-Αψ)*(1-Ġ)+Ŝ*2;IMyCubeGrid g=Φ.CubeGrid;float ΔΨ=0,ΔΩ=0,ΔΪ=0,ΔΫ=0;
for(int c=0;c<8;c++){Vector3 p=Ƽ(Φ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȭ?p.Z:p.Y;if(c==0||p.X<ΔΨ) ΔΨ=p.X;if(c==0||p.X>ΔΩ) ΔΩ=p.X;if(c==0||y<ΔΪ) ΔΪ=y;if(c==0||y>ΔΫ) ΔΫ=y;}Ţ=ΔΩ-ΔΨ+g.GridSize;ơ=ΔΫ-ΔΪ+g.GridSize;}float Ηώ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǣ<T>(List<T>Т,String Ǟ,bool Αγ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βδ(Т[i],Ǟ,Αγ)) έ.Add(Т[i]);return έ;}bool Βδ<T>(T Τ,String Ǟ,bool Αγ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αγ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αγ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ђ<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΔЏ((IMyTerminalBlock) Т[i],Ј);}void ΔЏ(IMyTerminalBlock Τ,bool Ј){if(Τ==null) return;String ΗЏ=Ј?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЏ);a.Apply(Τ);}void Ǥ(bool Ј){ΔΒ=Ј;if(!Ĭ) return;ђ(ΑЄ,Ј);}void Βό(ChargeMode ƙ){for(int i=0;i<Ȩ.Count;i++) Ȩ[i].ChargeMode=ƙ;}void Βώ(bool ΘЏ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΘЏ;}void Ż(List<IMyLandingGear>Γή,bool ΖΓ){for(int i=0;i<Γή.Count;i++){if(ΖΓ) Γή[i].Lock();if(!ΖΓ) Γή[i].Unlock();}}void ΔА(bool Ј){if(Εε==Ј) return;List<IMyShipController>ΑΞ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΞ,ǐ);if(ΑΞ.Count==0) return;
for(int i=0;i<ΑΞ.Count;i++)
ΑΞ[i].DampenersOverride=Ј;Εε=Ј;}IMyShipConnector О(MyShipConnectorStatus ь){for(int i=0;i<т.Count;i++){if(!Ƿ(т[i],true)) continue;if(т[i].Status==ь) return т[i];}return null;}void ē(bool ΓЏ,bool ΑΥ,float ΘΩ,float ΘΓ){if(ξ==null||ο.Count==0) return;Vector3 ΑΝ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward) continue;ȁ++;ΑΝ+=ο[i].GetPosition();}ΑΝ=ΑΝ/ȁ;Vector3 Ř=Ƽ(ξ,ΑΝ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЏ;ξ.LeftExtend=(ΑΥ?1:й)/2f*Ǳ-Ř.X;ξ.RightExtend=(ΑΥ?1:й)/2f*Ǳ+Ř.X;ξ.TopExtend=(ΑΥ?1:и)/2f*ǰ+Ř.Y;
ξ.BottomExtend=(ΑΥ?1:и)/2f*ǰ-Ř.Y;ξ.FrontExtend=(ΓЏ?Ц:ΘΩ)-Ř.Z;ξ.BackExtend=ΓЏ?0:ΘΓ+θ*0.75f+Ř.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ź<T>(List<T>Т,bool ΘЊ,float ΘΧ,bool Θγ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƥ=null;if(Т[i]is IMyTextSurface) ƥ=(IMyTextSurface) Т[i];if(ƥ!=null){ƥ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЊ) continue;ƥ.Font="Debug";if(Θγ) continue;ƥ.FontSize=ΘΧ;}}}bool ǀ(){Vector3D ε;return this.Φ.TryGetPlanetPosition(out ε);}int ΗΞ(Vector3 ΘБ,int Ζβ){if(Ζβ==-1) return 0;double Б=-1;int Ȓ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘБ);if(Б==-1||ό<Б){Ȓ=i;Б=ό;}
}return Math.Sign(Ȓ-Ζβ);}bool ΗΪ(Vector3 ε){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.δ&&Σ.Count>=1){Vector3 ŀ=new Vector3();Ƕ(Θ,dockDist*θ,false,out ŀ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,ŀ);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(ŀ);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.δ&&Σ.Count>=1){Vector3 ΒЌ=new Vector3();Ƕ(Ε,dockDist*θ,false,out ΒЌ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЌ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЌ);Ж.Add(Ε.Δ);}}}else{if(ή!=Κ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȓ=-1;double ǧ=-1;for(int i=0;i<Ж.Count;i++){double ό=Vector3.Distance(Ж[i],ε);if(ό<ǧ||ǧ==-1){ǧ=ό;Ȓ=i;}}if(Ж.Count==0) return false;double ΓΤ=Vector3.Distance(Ж[Ȓ],ε);double Θό=Vector3.Distance(Ж[Math.Max(0,Ȓ-1)],Ж[Ȓ])*1.5f;
double Θυ=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȓ+1)],Ж[Ȓ])*1.5f;return ΓΤ<Θό||ΓΤ<Θυ||ΓΤ<laneOffset*θ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηκ(Vector3 v,int ΓΠ){return new Vector3(Math.Round(v.X,ΓΠ),Math.Round(v.Y,ΓΠ),Math.Round(v.Z,ΓΠ));
}float ĵ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΦ=(a*b).Sum;float ΕЏ=a.Length();float ΕА=b.Length();if(ΕЏ==0||ΕА==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΦ/(ΕЏ*ΕА)));if(float.IsNaN(έ)) return 0;return έ;}float Č(Vector3 v,Vector3 Αύ,float ΘЋ){float ľ=ĵ(v,Αύ);
if(ΘЋ>0f) ľ*=-1;if(ľ>-90f) return ľ-90f;else return 180f-(-ľ-90f);}double Δι(float ΘΞ){return(Math.PI/180)*ΘΞ;}bool Βη(double Θξ,double value,double Αρ){return(value>=Θξ&&value<=Αρ);}Vector3 ΗϏ(IMyTerminalBlock Τ,Vector3 Љ){return Vector3D.Transform(Љ,Τ.WorldMatrix);
}Vector3 Ƽ(IMyTerminalBlock Τ,Vector3 ΙΧ){return Я(Τ,ΙΧ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΧ){return Vector3D.TransformNormal(ΔΧ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 Ē(Vector3 Ǫ,Vector3 ώ,Vector3 ΔΧ){MatrixD Γψ=MatrixD.CreateFromDir(Ǫ,ώ);return Vector3D.TransformNormal(ΔΧ,MatrixD.Transpose(Γψ));
}Vector3 ΒІ(Vector3 Ǫ,Vector3 ώ,Vector3 Љ){MatrixD Γψ=MatrixD.CreateFromDir(Ǫ,ώ);return Vector3D.Transform(Љ,Γψ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Λ(Е,0)),float.Parse(Λ(Е,1)),float.Parse(Λ(Е,2)));
}String ΗЍ(Ψ Ω){String Ћ=":";String έ=τ(Ω.Δ)+Ћ+τ(Ω.γ)+Ћ+
τ(Ω.β)+Ћ+τ(Ω.ψ)+Ћ+τ(Ω.ν);for(int i=0;i<Ω.ǟ.Length;i++)
έ+=Ћ+Math.Round(Ω.ǟ[i],3);return έ;}Ψ Ηω(String s){String[]Е=s.Split(':');Ψ Ω=new Ψ(
μ(Λ(Е,0)),μ(Λ(Е,1)),
μ(Λ(Е,2)),μ(Λ(Е,3)),
μ(Λ(Е,4)));int Β=5;List<float>Ερ=new List<float>();while(Β<Е.Length){String ΑА=Λ(Е,Β);float f=0;if(!float.TryParse(ΑА,out f)) break;Ερ.Add(f);Β++;}Ω.ǟ=Ερ.ToArray();return Ω;}System.Text.StringBuilder Š=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θτ){if(Θτ) Š.Append('\n');Š.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ƹ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΔ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Λ(String[]ΘΒ,int Β){String ΑА=ΘΒ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑА)) return "";
return ΑА;}void Save(){if(ΑϏ||Η==Ζ.р){Storage="";return;}Storage=ΒΚ();}String ΒΚ(){Š.Clear();Š.Append(DATAREV+";");Ρ(τ(Αώ),false);Ρ(τ(Θ.γ));Ρ(τ(Θ.ψ));Ρ(τ(Θ.β));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.δ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.γ));Ρ(τ(Ε.β));Ρ(τ(Ε.ψ));Ρ(τ(Ε.Ч));Ρ(Ε.δ);Ρ(τ(ǜ));Ρ(τ(ș));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(ǫ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(ǥ);Ρ(Ƞ);Ρ(ȟ);if(Η==Ζ.Ϋ){Ρ((int) ϊ.Ќ);Ρ(ϊ.Ǖ);Ρ(ϊ.Ȯ);Ρ(Ƹ(ϊ.ǖ));Ρ(Ƹ(ϊ.Ǚ));Ρ((int) ϋ.Ќ);Ρ(ϋ.Ǖ);Ρ(ϋ.Ȯ);Ρ(Ƹ(ϋ.ǖ));Ρ(Ƹ(ϋ.Ǚ));}else{Ρ((int) ȝ);Ρ((int) л);Ρ((int) Л);Ρ((int) у);Ρ((int) Ĥ);Ρ(Ĉ);Ρ(Ĭ);Ρ(ŏ);Ρ(Ň);Ρ(й);Ρ(и);Ρ(Ц);Ρ(ū);Ρ(Ū);Ρ(ı);Ρ(Ġ);
Ρ(М);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(ȗ);Ρ(Й);Ρ(ĩ);Ρ(ħ);}Ρ(";");for(int i=0;i<ȇ.Count;i++)
Ρ((i>0?"|":"")+ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЍ(Σ[i]),i>0);Ρ(";",false);Ρ(С+"|"+Ё,false);Ρ(";",false);Ρ(ΗΕ(),false);String Χ=Š.ToString();Š.Clear();return Χ;}с Ηι(){return ǈ(Storage);}с ǈ(String Χ){if(Χ=="") return с.Ηα;String[]Ī=Χ.Split(';');if(Λ(Ī,0)!=DATAREV) return с.Βμ;int Β=0;try{String[]Ι=Λ(Ī,1).Split('\n');
Αώ=μ(Λ(Ι,Β++));Θ.γ=μ(Λ(Ι,Β++));Θ.ψ=μ(Λ(Ι,Β++));Θ.β=μ(Λ(Ι,Β++));Θ.ν=μ(Λ(Ι,Β++));Θ.Δ=μ(Λ(Ι,Β++));Θ.Ч=μ(Λ(Ι,Β++));Θ.δ=bool.Parse(Λ(Ι,Β++));Ε.Δ=μ(Λ(Ι,Β++));Ε.ν=μ(Λ(Ι,Β++));Ε.γ=μ(Λ(Ι,Β++));Ε.β=μ(Λ(Ι,Β++));Ε.ψ=μ(Λ(Ι,Β++));Ε.Ч=μ(Λ(Ι,Β++));Ε.δ=bool.Parse(Λ(Ι,Β++));
ǜ=μ(Λ(Ι,Β++));ș=μ(Λ(Ι,Β++));Ι=Λ(Ī,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Λ(Ι,Β++));ή=(Κ) int.Parse(Λ(Ι,Β++));ц=(Κ) int.Parse(Λ(Ι,Β++));ǫ=int.Parse(Λ(Ι,Β++));ч=int.Parse(Λ(Ι,Β++));Ы=int.Parse(Λ(Ι,Β++));ш=int.Parse(Λ(Ι,Β++));ǥ=float.Parse(Λ(Ι,Β++));Ƞ=bool.Parse(Λ(Ι,Β++));
ȟ=bool.Parse(Λ(Ι,Β++));if(Η==Ζ.Ϋ){ϊ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϊ.Ǖ=float.Parse(Λ(Ι,Β++));ϊ.Ȯ=float.Parse(Λ(Ι,Β++));ϊ.ǖ=ΑΔ(Λ(Ι,Β++));ϊ.Ǚ=ΑΔ(Λ(Ι,Β++));ϋ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϋ.Ǖ=float.Parse(Λ(Ι,Β++));ϋ.Ȯ=float.Parse(Λ(Ι,Β++));ϋ.ǖ=ΑΔ(Λ(Ι,Β++));ϋ.Ǚ=ΑΔ(Λ(Ι,Β++));}else
{ȝ=(Ǒ) int.Parse(Λ(Ι,Β++));л=(д) int.Parse(Λ(Ι,Β++));Л=(Ѐ) int.Parse(Λ(Ι,Β++));у=(я) int.Parse(Λ(Ι,Β++));Ĥ=(Ǒ) int.Parse(Λ(Ι,Β++));Ĉ=bool.Parse(Λ(Ι,Β++));Ĭ=bool.Parse(Λ(Ι,Β++));ŏ=bool.Parse(Λ(Ι,Β++));Ň=bool.Parse(Λ(Ι,Β++));й=int.Parse(Λ(Ι,Β++));и=int.Parse(Λ(Ι,Β++));Ц=int.Parse(Λ(Ι,Β++));
ū=float.Parse(Λ(Ι,Β++));Ū=float.Parse(Λ(Ι,Β++));ı=float.Parse(Λ(Ι,Β++));Ġ=float.Parse(Λ(Ι,Β++));М=int.Parse(Λ(Ι,Β++));б=int.Parse(Λ(Ι,Β++));Ш=int.Parse(Λ(Ι,Β++));Щ=int.Parse(Λ(Ι,Β++));ȗ=int.Parse(Λ(Ι,Β++));Й=int.Parse(Λ(Ι,Β++));ĩ=int.Parse(Λ(Ι,Β++));ħ=float.Parse(Λ(Ι,Β++));
}Ι=Λ(Ī,3).Replace("\n","").Split('|');ȇ=Ι.ToList();Ι=Λ(Ī,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηω(Λ(Ι,i)));Ι=Λ(Ī,5).Split('|');С=Λ(Ι,0);if(!int.TryParse(Λ(Ι,1),out Ё)) Ё=-1;if(С.Length!=М*б){С="";Ё=-1;}ΗΔ(Λ(Ī,6));}catch{return с.Δν;}š=ц;χ();return с.ΑΑ;}const int Δα=64000;IMyTerminalBlock Δξ(){List<IMyTerminalBlock>Ń=new List<IMyTerminalBlock>();ω.GetBlocksOfType(Ń,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ń.Count>0?Ń[0]:null;}void ΗΘ(){IMyTerminalBlock Τ=Δξ();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΚ();if(Χ.Length>Δα){Μ="Export too large: "+Χ.Length+" chars, max "+Δα;return;}Τ.CustomData=Χ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΤ(bool Γν){IMyTerminalBlock Τ=Δξ();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΥ(Τ.CustomData,Γν);}void ΗΥ(String Χ,bool Γν){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΞ=Χ.IndexOf('\n');Χ=ΖΞ<0?"":Χ.Substring(ΖΞ+1);}Χ=Χ.Trim();if(Χ==""){Μ="Import: no PAM data in "+dataTag;return;}χ();String ΕΦ=ΒΚ();с έ=ǈ(Χ);if(έ==с.ΑΑ&&Γν){Ψ Ă=new Ψ(Ε);
Vector3 Θρ=ǜ,Θπ=ș;int w=й,h=и,d=Ц;Ǒ ΔΓ=ȝ;я ΓΡ=у;float Θω=ı,Θψ=Ġ;ǈ(ΕΦ);Ε=Ă;ǜ=Θρ;ș=Θπ;й=w;и=h;Ц=d;ȝ=ΔΓ;у=ΓΡ;ı=Θω;Ġ=Θψ;ή=Κ.Ĺ;Ž(true,true);ή=Κ.Ĺ;ц=Κ.Ĺ;Μ="Imported job, run CONT";return;}if(έ==с.ΑΑ){ƚ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ǈ(ΕΦ);Μ=έ==с.Βμ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ησ(){ƈ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γλ=false;if(Χ=="INSTRUCTIONS"){Γλ=true;ΑΤ=true;}if(Χ=="DEBUG") Γλ=true;if(Γλ){ƈ.Add(ї[i]);ї.RemoveAt(i);}
}ź(ƈ,false,1,false);}void Ητ(List<IMyTerminalBlock>Ń){Ć.Clear();for(int i=0;i<Ń.Count;i++){IMyTerminalBlock Τ=Ń[i];try{String ΖΧ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΧ);int Ť=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΧ.Length,1),out Ť)) continue;
if(Ť==-1) continue;Ť--;IMyTextSurfaceProvider ΖΩ=(IMyTextSurfaceProvider) Τ;if(Ť<ΖΩ.SurfaceCount&&Ť>=0)
Ć.Add(ΖΩ.GetSurface(Ť));}catch{}}}String Αυ="";void ΔЋ(){String Γχ="";String ΓΩ="";Αυ=Δΰ(false);Γχ+=Αυ;ΓΩ+=Αυ;ΓΩ+=ΗΜ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γχ);for(int i=0;i<Ć.Count;i++) Ć[i].WriteText(Γχ);Echo(ΓΩ);for(int i=0;i<ƈ.Count;i++){IMyTextPanel ΓЃ=ƈ[i];
String Χ=ΓЃ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЃ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЃ.WriteText(ΗΝ());}}string ΗΝ(){String Γ="";try{float ΖΖ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(ł*ΖΖ)+" / "+(int)(Ŕ*ΖΖ)+"\n";Γ+="Inst. avg/max: "+Math.Round(ł*100f,1)+"% / "+Math.Round(Ŕ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ƃ,2)+"ms / "+Math.Round(Γϋ,2)+"ms \n";}catch{}for(int i=0;i<Ț.Count;i++)
Γ+=""+Ț.Keys.ElementAt(i)+" = "+Math.Round(Ț.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ț.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΠ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ź(ї,setLCDFontAndSize,0.9f,false);ź(Ć,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+Ȗ.ToString());for(int i=0;i<Ć.Count;i++) Ć[i].WriteText(Γ+Ȗ.ToString());}void Ǻ(){if(!ΑΤ) return;try{ΓЇ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String υ){if(!ΑΤ) return;if(ΓЇ==0) return;try{float ΑЎ=(Runtime.CurrentInstructionCount-ΓЇ)/Runtime.MaxInstructionCount*100;if(!Ț.ContainsKey(υ))
Ț.Add(υ,new float[]{ΑЎ,ΑЎ});else{Ț[υ][0]=ΑЎ;Ț[υ][1]=Math.Max(ΑЎ,Ț[υ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЃ(String Ȱ){if(!ń) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=Ŭ(ΓЎ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=ΒΫ(ί.X)+""+Ћ;ρ+=ΒΫ(ί.Y)+Ћ;ρ+=ΒΫ(ί.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=Βέ(ц)+(ц==Κ.Ϗ?" "+Math.Round(Ɠ,1)+"%":"")+Ћ;else ρ+=Βέ(ц)+Ћ;ρ+=Ŭ(Μ)+Ћ;ρ+=љ+""+Ћ;ρ+=Ŭ(Αυ)+Ћ;ρ+=ņ+Ћ;ρ+=ŕ+Ћ;for(int i=0;i<ǘ.Count;i++){if(ǘ[i].Œ!=З.Ų) continue;ρ+=Ŭ(ǘ[i].υ)+"/"+Ŭ(ǘ[i].ς)+"/"+ǘ[i].Ǔ+Ћ;}Ηπ(ρ,Ȱ);}String Ŭ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηπ(String ρ,String Ȱ){Ηρ(ρ,BROADCAST_SELF_ID,Ȱ);}void Ηρ(String ρ,String Њ,String Ȱ){try{if(!ń) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δρ()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΓΕ+BROADCAST_SEP+Ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȗ=e;}
}String Δρ(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЁ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ů(ref String Χ,out string Ες,Char Ћ){int Β=Χ.IndexOf(Ћ);Ες="";if(Β<0) return false;Ες=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηβ(ref String ρ,out String Ơ,out String Ȱ)
{Ơ="";Ȱ="";if(!ń) return false;String Ǟ="";if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЁ(Ǟ)) return false;if(!Ů(ref ρ,out Ơ,BROADCAST_SEP)) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δρ()&&(Ǟ!="*"&&Η!=Ζ.р))) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΕ)) return false;
if(!Ů(ref ρ,out Ȱ,BROADCAST_SEP)) return false;return true;}void ΗΧ(){bool Ɔ=true;if(Ετ){Ετ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Ƒ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ƒ)
Δύ(Ƒ);String[]Ι=Me.CustomData.Split('\n');ń=bool.Parse(Βφ(ΒΩ(Ι,"Enable_Broadcast",ref Ɔ)));bool ΔΥ=false;bool Ει=true;if(ń){if(Η!=Ζ.р)
ΓЎ=Βφ(ΒΩ(Ι,"Ship_Name",ref Ɔ)).Replace(BROADCAST_SELF_ID,"");ΓΕ=Βφ(ΒΩ(Ι,"Broadcast_Channel",ref Ɔ)).ToLower();Ει=false;if(ȑ==null){ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȑ.SetMessageCallback("");}List<IMyRadioAntenna>Ŀ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(Ŀ);bool ΒЋ=false;
for(int i=0;i<Ŀ.Count;i++)
if(Ŀ[i].EnableBroadcasting&&Ŀ[i].Enabled){ΒЋ=true;break;}if(Ŀ.Count==0) Μ="No Antenna found";else if(!ΒЋ) Μ="Antenna not ready";ΔΥ=Ŀ.Count==0||!ΒЋ;if(ΕΞ&&!ΔΥ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΞ=ΔΥ;if(Ει){if(ȑ!=null) this.IGC.DisableBroadcastListener(ȑ);ȑ=null;}}catch{Ɔ=false;}if(!Ɔ){Μ="Reset custom data";Δύ(Ƒ);}}void Δύ(String Ƒ){String Χ=Ƒ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Χ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βφ(String Ħ){int Β=Ħ.IndexOf("//");if(Β!=-1) Ħ=Ħ.Substring(0,Β);String[]Е=Ħ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΩ(String[]Ι,String ĥ,ref bool ƛ){foreach(String Ħ in Ι)
if(Ħ.StartsWith(ĥ)) return Ħ;ƛ=false;return "";}String Ισ="";Ļ Ιυ=null;DateTime Ιφ=DateTime.MinValue;List<Ļ>Ιτ=new List<Ļ>();const float Ιή=20f;String ΔЎ(int ΔΤ,String Γ,int Ζά,ref int ț){String[]Ι=Γ.Split('\n');if(Ζά>=ț+ΔΤ-1) ț++;ț=Math.Min(Ι.Count()-1-ΔΤ,ț);if(Ζά<ț+1) ț--;
ț=Math.Max(0,ț);String έ="";for(int i=0;i<ΔΤ;i++){int ΕБ=i+ț;if(ΕБ>=Ι.Count()) break;έ+=Ι[ΕБ]+"\n";}return έ;}String Βά(int Ɵ){if(Ɵ>=60*60) return Math.Round(Ɵ/(60f*60f),1)+" h";if(Ɵ>=60) return Math.Round(Ɵ/60f,1)+" min";return ""+Ɵ+" s";}string ΒΫ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>ř=new List<long>();bool ś=false;int ΓΛ=0;int ΓΘ=0;DateTime ΕΩ=DateTime.Now;String ΓΔ="";void Ηο(){ǽ.Clear();ř.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!ř.Contains(ǽ[i].CubeGrid.EntityId)) ř.Add(ǽ[i].CubeGrid.EntityId);}if(!ř.Contains(Me.CubeGrid.EntityId)) ř.Add(Me.CubeGrid.EntityId);}bool ΔЀ(Α s){return s==Α.ȋ||s==Α.Ю||s==Α.Ѕ||
s==Α.ȥ||s==Α.Ķ||s==Α.ů||s==Α.ƺ||
(s==Α.ǡ&&њ!=null);}void ƶ(){ś=false;ΓΛ=0;ΓΔ="";}void ΗЇ(){if(!obstacleBrake||ǽ.Count==0||!ΔЀ(Υ)){ƶ();return;}Vector3 Ɛ=φ+ć;Vector3 Ǯ=Ɛ-ί;float ǭ=Ǯ.Length();if(ǭ<1f){ƶ();return;}Vector3 κ=Ǯ/ǭ;float ΖΔ=Ȝ*obstacleLookAhead+θ;Vector3 ΘϏ=Ɛ+κ*(θ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΓΘ=(ΓΘ+1)%ǽ.Count;IMyCameraBlock ƅ=ǽ[ΓΘ];if(!ƅ.IsFunctional||!ƅ.IsWorking) continue;Vector3 ΕΫ=ƅ.GetPosition();Vector3 Αά=ΘϏ;if(Vector3.Distance(ΕΫ,Αά)>ΖΔ) Αά=ΕΫ+κ*ΖΔ;if(!ƅ.CanScan(Αά)) continue;MyDetectedEntityInfo ƒ=ƅ.Raycast(Αά);bool Θχ=!ƒ.IsEmpty()&&!ř.Contains(ƒ.EntityId)&&!Ηέ(ƒ.EntityId)&&
ƒ.Type!=MyDetectedEntityType.FloatingObject;if(Θχ){if(!ś) ΕΩ=DateTime.Now;ś=true;ΓΛ=0;ΓΔ=ƒ.Type.ToString();}else if(ś&&++ΓΛ>=3) ƶ();break;}if(ś&&(DateTime.Now-ΕΩ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΓΔ;}const String ΕΘ="[PAMTFC]";const float Ηό=5f;const float ΒΨ=2f;class Ȧ{public long λ;public Vector3 ε,į;public Vector3 Ō;public float Εθ;public String Ă="";public int Γη=-1;public bool ΓЈ;public bool Αβ;public float ğ,ň;public long ά=0;public int Ɗ=0;public DateTime ΑЋ;
}IMyBroadcastListener ΑЍ=null;long ъ=0;List<Ȧ>м=new List<Ȧ>();Vector3 ć=new Vector3();float э=-1;Vector3 ΔΝ=new Vector3();bool ΑЌ=false;DateTime ΔΞ=DateTime.Now;const float ΕΗ=15f;const float Ηϋ=10f;int Ζψ=0;String С="";int Ё=-1;bool ΓΞ=false;Vector3 Εί=new Vector3();String ŵ()
{if(ή==Κ.з||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+М+"|"+б+"|"+(int) Ĥ+"|"+Math.Round(Ǳ,1)+"|"+Math.Round(ǰ,1);}void ΗЊ(){if(!trafficEnabled) return;if(ΑЍ==null) ΑЍ=IGC.RegisterBroadcastListener(ΕΘ);ъ=Me.CubeGrid.EntityId;int ę=30;while(ΑЍ.HasPendingMessage&&ę-->0){String Χ=ΑЍ.AcceptMessage().Data as String;if(Χ!=null) Ηε(Χ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΑЋ).TotalSeconds>Ηό) м.RemoveAt(i);if(ΑЈ==null) ΑЈ=IGC.RegisterBroadcastListener(Ηϊ);ę=5;while(ΑЈ.HasPendingMessage&&ę-->0){String Χ=ΑЈ.AcceptMessage().Data as String;if(Χ!=null) try{Ηζ(Χ);}catch{}}if(ΑΟ==null) ΑΟ=IGC.RegisterBroadcastListener(Ηή);
ę=10;while(ΑΟ.HasPendingMessage&&ę-->0){String Χ=ΑΟ.AcceptMessage().Data as String;if(Χ!=null) try{Ηγ(Χ);}catch{}}if(++Ζψ<3) return;Ζψ=0;IGC.SendBroadcastMessage(ΕΘ,ъ+";"+τ(ί)+";"+
τ(Φ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ŵ()+";"+
(ή==Κ.Ϗ?Ё:-1)+";"+С+";"+ΖЍ()+","+Δλ()+";"+τ(ΔΝ)+
";"+(ΑΧ?1:0)+";"+(ġ&&ή==Κ.Ϗ?1:0)+","+Ţ+","+ơ+","+(Δϊ()?1:0));}void Ηε(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ъ) return;Ȧ Ї=null;for(int i=0;i<м.Count;i++) if(м[i].λ==λ) Ї=м[i];if(Ї==null){Ї=new Ȧ();Ї.λ=λ;м.Add(Ї);}
Ї.ε=μ(f[1]);Ї.į=μ(f[2]);Ї.Εθ=float.Parse(f[3]);Ї.Ă=f[4];Ї.Γη=int.Parse(f[5]);Ї.ΑЋ=DateTime.Now;if(Ї.Ă!=""&&Ї.Ă==ŵ()) ΔЈ(f[6]);Ї.ά=0;Ї.Ɗ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.Ɗ=int.Parse(d[1]);}Ї.Ō=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЈ=f.Length>=10&&f[9]=="1";Ї.Αβ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.ğ=float.Parse(w[1]);Ї.ň=float.Parse(w[2]);Ї.Αβ=w[3]=="1";}}}catch{}}bool Ηέ(long λ){for(int i=0;i<м.Count;i++) if(м[i].λ==λ) return true;return false;}Vector3 Δυ(int Β,int Θϊ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΞ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θϊ;Vector3 ώ=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.β;Vector3 Ь=Vector3.Cross(ΙΞ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ό=laneOffset*θ;float ΘΨ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΨ/(ό*4));}Vector3 Ζχ=new Vector3();bool Δϊ(){return ή==Κ.Ϗ&&(Υ==Α.ȋ||Υ==Α.ĸ||Υ==Α.Ǹ);}bool Ηά(Α s){return s==Α.ǡ||s==Α.Ċ||s==Α.Ď||s==Α.Ź||
s==Α.ǒ||s==Α.Ǹ||s==Α.ȡ;}void ΗЋ(){ć=new Vector3();э=-1;ΔΝ=new Vector3();bool Εχ=ΔЀ(Υ);if(!trafficEnabled||(!Εχ&&!Ηά(Υ))){ΑЌ=false;return;}Ζχ=Φ.GetShipVelocities().LinearVelocity;ΖЎ(Φ.GetNaturalGravity(),!Εχ);ΖЁ();}IMyBroadcastListener ΑЈ=null;float Ŧ=-1;DateTime Ζς=DateTime.MinValue;
void Ηζ(String Χ){if(!Χ.StartsWith("C;")) return;Ζς=DateTime.Now;Ŧ=-1;String Ƙ=ъ+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ƙ)) Ŧ=float.Parse(e[i].Substring(Ƙ.Length));}void ΖЁ(){if(Ŧ<0||(DateTime.Now-Ζς).TotalSeconds>1.5) return;
э=э<0?Ŧ:Math.Min(э,Ŧ);if(Ŧ<0.5f) Μ="Traffic: giving way (controller)";}void ΖЎ(Vector3 ν,bool ΖΦ=false){ć=new Vector3();э=-1;Vector3 Ǯ=φ-ί;float ǭ=Ǯ.Length();bool Γε=ǭ>=1f;Vector3 κ=Γε?Ǯ/ǭ:new Vector3();ΔΝ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-ƌ;
Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-Ɩ:Vector3.Normalize(Ь);Ȧ Αλ=null;bool Εω=Υ==Α.Ѕ&&!ȏ(1);for(int i=0;i<м.Count;i++){Ȧ α=м[i];if(α.Ɗ==3&&α.į.LengthSquared()<0.25f) continue;Vector3 ε=α.ε+α.į*(float)(DateTime.Now-α.ΑЋ).TotalSeconds;Vector3 ŝ=ε-ί;
float є=(θ+α.Εθ)*0.5f;bool Ζί=α.Αβ&&Δϊ()&&α.ğ>0&&Ţ>0&&α.Ă==ŵ();if(Ζί) є=Math.Max(Ţ+α.ğ,ơ+α.ň)*0.5f;float ƀ=Γε?Vector3.Dot(ŝ,κ):0;if(ƀ>0&&(ŝ-κ*ƀ).Length()<=є*1.2f){float Αϊ=Vector3.Dot(α.į,κ);bool ΕϏ=α.Ō.LengthSquared()>0.25f;bool ΕЀ=Αϊ<-1f||(ΕϏ&&Vector3.Dot(α.Ō,κ)<-0.5f);
if(!ΖΦ){if(α.ΓЈ){if(!Εω){ć=Ь*є*1.3f;continue;}float ΖΛ=Vector3.Distance(ί,φ),ΕЋ=Vector3.Distance(ε,φ);if(ΖΛ<ΕЋ||(ΖΛ==ΕЋ&&ъ<α.λ)) continue;}if(ΕЀ){if(ƀ<(Ȝ-Αϊ)*6f+є*4f) ć=Ь*є*1.3f;continue;}if(ΕϏ&&ъ<α.λ){Vector3 ΕΥ=-ŝ;float ΖΡ=Vector3.Dot(ΕΥ,α.Ō);
if(ΖΡ>0&&(ΕΥ-α.Ō*ΖΡ).Length()<=є*1.2f){ć=Ь*є*1.3f;continue;}}}float Εψ=ƀ-є-(Ζί?1f:followGap*θ);float Ακ=Math.Max(0,Αϊ)+Math.Max(0,Εψ)/ΒΨ;if(ΕЀ) Ακ=Math.Max(0,Εψ+Math.Min(0,Αϊ)*ΒΨ*2)/ΒΨ;if(э<0||Ακ<э){э=Ακ;Αλ=α;}continue;}if(α.į.LengthSquared()<1f) continue;
if(Γε&&(ŝ-κ*ƀ).Length()<=є*1.2f) continue;Vector3 ΓЍ=α.į-Ζχ;float ΖΪ=ΓЍ.LengthSquared();if(ΖΪ<0.25f) continue;float t=-Vector3.Dot(ŝ,ΓЍ)/ΖΪ;if(t<=0||t>5f||(ŝ+ΓЍ*t).Length()>=є*1.1f) continue;if(ъ>α.λ||t<2f){э=0;Αλ=α;}}bool Θί=э>=0&&э<0.5f;
if(!Θί||ΖΦ||(Αλ!=null&&Αλ.ΓЈ&&Εω)){ΑЌ=false;return;}if(!ΑЌ){ΑЌ=true;ΔΞ=DateTime.Now;}double ΕЁ=(DateTime.Now-ΔΞ).TotalSeconds;if(ΕЁ>ΕΗ+Ηϋ) ΔΞ=DateTime.Now;else if(ΕЁ>ΕΗ){э=-1;ć=Ь*θ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>ȃ=new List<int>();
String ΕЃ="";void ΒΧ(){String ĥ=М+"|"+б+"|"+(int) Ĥ;if(ĥ==ΕЃ) return;ΕЃ=ĥ;ΖЄ(М,б,Ĥ,ȃ);}void ΖЄ(int w,int h,Ǒ ΔΓ,List<int>ΓΙ){ΓΙ.Clear();if(ΔΓ==Ǒ.ΑΒ){for(int k=0;k<w*h;k++){int Ş=k/w;ΓΙ.Add(Ş*w+(Ş%2==0?k-Ş*w:w-1-(k-Ş*w)));}return;}int Εώ=(int) Math.Ceiling(w/2f),Εύ=(int) Math.Ceiling(h/2f);
int ΘΫ=w/2,ΘΪ=h/2;int x=0,y=0,ΓΧ=0,ΓΨ=-1,Αρ=Math.Max(w,h);for(int i=0;i<Αρ*Αρ;i++){if(-Εώ<x&&x<=ΘΫ&&-Εύ<y&&y<=ΘΪ) ΓΙ.Add((y-1+Εύ)*w+(x-1+Εώ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΧ;ΓΧ=-ΓΨ;ΓΨ=t;}x+=ΓΧ;y+=ΓΨ;
}}bool ΗΣ(int a,int b,int w,float Ζϊ,float Ζσ,float ğ,float ň){return a==b||(Math.Abs(a%w-b%w)*Ζϊ<ğ+0.5f&&Math.Abs(a/w-b/w)*Ζσ<ň+0.5f);}bool ΒΦ(){int n=Math.Max(0,М*б);if(С.Length==n) return false;С=new String('0',n);Ё=-1;return true;}void ΔЈ(String α)
{ΒΦ();if(α.Length!=С.Length) return;char[]h=С.ToCharArray();for(int i=0;i<h.Length;i++) if(α[i]=='1') h[i]='1';С=new String(h);}bool ƿ(int Ǿ){return Ǿ>=0&&Ǿ<С.Length&&С[Ǿ]=='1';}void Βύ(int Ǿ,bool ф){if(Ǿ<0||Ǿ>=С.Length) return;char[]h=С.ToCharArray();h[Ǿ]=ф?'1':'0';
С=new String(h);}bool Βε(int Ǿ,bool Θε){if(!trafficEnabled) return false;String ĥ=ŵ();for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.Ă!=ĥ||t.Γη<0||(Θε&&t.λ>ъ)) continue;if(ΗΣ(Ǿ,t.Γη,М,Ǳ,ǰ,Ţ,ơ)) return true;}return false;}int ΖЌ(int Β){int n=М*б;if(n<1) return Β;ΒΧ();
if(ΒΦ()) for(int k=0;k<Β&&k<n;k++) Βύ(ȃ[k],true);if(Ё>=0&&(Β>=n||ȃ[Β]!=Ё)){Βύ(Ё,true);Ё=-1;}if(ΔЂ()){if(Ǩ>=0&&!ƿ(Ǩ)){ġ=false;Ё=Ǩ;return ȃ.IndexOf(Ǩ);}if(Ǩ==-2){ġ=false;Ё=-1;return n;}if(Ǩ==-1&&Ё>=0&&!ƿ(Ё)){ġ=false;return Β;}Ё=-1;
if(!ġ){ġ=true;Εα=DateTime.Now;}if((DateTime.Now-Εα).TotalSeconds<5) return-1;}ġ=false;if(Ё>=0&&!ƿ(Ё)&&!Βε(Ё,true)) return Β;Ё=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(ƿ(c)||Βε(c,false)) continue;Ё=c;return k;}return n;}const String Ηή="[PAMJOB]";const String Ηϊ="[PAMTCC]";
IMyBroadcastListener ΑΟ=null;DateTime Εΰ=DateTime.MinValue;DateTime Εα=DateTime.Now;int Ǩ=-1;bool ġ=false;bool ΔЂ(){return trafficEnabled&&Ǩ!=-3&&(DateTime.Now-Εΰ).TotalSeconds<3;}void Ηγ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ŵ()) return;
Εΰ=DateTime.Now;ΔЈ(f[2]);Ǩ=-3;String Ƙ=ъ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ƙ)) Ǩ=int.Parse(e[i].Substring(Ƙ.Length));}ļ Ης(int Β){ΒΧ();if(Β<0||Β>=ȃ.Count) return ļ.Δή;Ш=ȃ[Β]%М;Щ=ȃ[Β]/М;return ļ.Δο;}class ё{public long λ;public Vector3 ε,В,ώ;
}List<ё>Ў=new List<ё>();bool ȫ=false;Vector3 ΑΩ,ΑΨ,ΑΪ,ΑΫ;long х=0;long ȅ=0;Ψ њ=null;bool Ƌ=false;Ψ ΒΣ(Vector3 ε,Vector3 В,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ε,В,ώ);Ψ w=new Ψ(Θ);w.γ=Vector3D.TransformNormal(ΑΩ,m);w.β=Vector3D.TransformNormal(ΑΨ,m);w.ψ=Vector3D.TransformNormal(ΑΪ,m);
w.Δ=ε+(Vector3) Vector3D.TransformNormal(ΑΫ,m);w.δ=true;return w;}int Δλ(){Α s=Υ;if(s==Α.ж) return О(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ď||s==Α.ǻ||s==Α.č||s==Α.ΑΗ||
s==Α.ƹ||s==Α.ǒ) return 3;if(s==Α.ǡ||s==Α.Ċ||s==Α.Ź) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЍ(){if(Δλ()==0) return 0;if(ȏ(1)&&ţ!=0) return ţ;return њ!=null?ȅ:х;}void ΔЅ(){if(Η==Ζ.Ϋ||!Θ.δ) return;IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector α=И.OtherConnector;Vector3I Θΰ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==Θΰ){MatrixD Ŏ=MatrixD.Transpose(α.WorldMatrix);ΑΩ=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,Ŏ);ΑΨ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,Ŏ);ΑΪ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,Ŏ);
ΑΫ=Vector3D.TransformNormal(И.GetPosition()-α.GetPosition(),Ŏ);ȫ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) х=α.EntityId;List<IMyShipConnector>Ğ=new List<IMyShipConnector>();ω.GetBlocksOfType(Ğ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ğ.Count>0){Ў.Clear();for(int i=0;i<Ğ.Count;i++){ё p=new ё();p.λ=Ğ[i].EntityId;p.ε=Ğ[i].GetPosition();p.В=Ğ[i].WorldMatrix.Forward;p.ώ=Ğ[i].WorldMatrix.Up;Ў.Add(p);}}њ=null;ȅ=0;if(α.EntityId==х||!ȫ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==α.EntityId){ȅ=Ў[i].λ;њ=ΒΣ(Ў[i].ε,Ў[i].В,Ў[i].ώ);}}bool Βθ(long λ,int ΖΜ){if(λ==0) return false;for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ά==λ&&(t.Ɗ>ΖΜ||(t.Ɗ==ΖΜ&&t.λ<ъ))) return true;}return false;}bool ΒΝ(int Αϋ){if(Η==Ζ.Ϋ||!trafficEnabled){њ=null;ȅ=0;return true;}
long η=њ!=null?ȅ:х;if(!Βθ(η,Αϋ)) return true;if(!Βθ(х,Αϋ)){њ=null;ȅ=0;return true;}if(!ȫ) return false;ё Ę=null;float ΕΨ=0;for(int i=0;i<Ў.Count;i++){ё p=Ў[i];if(p.λ==х||Βθ(p.λ,Αϋ)) continue;float d=Vector3.Distance(p.ε,Θ.Δ);if(Ę==null||d<ΕΨ){Ę=p;ΕΨ=d;}
}if(Ę==null) return false;ȅ=Ę.λ;њ=ΒΣ(Ę.ε,Ę.В,Ę.ώ);return true;}String ΗΕ(){String s=(ȫ?"1":"0")+":"+τ(ΑΩ)+":"+τ(ΑΨ)+":"+
τ(ΑΪ)+":"+τ(ΑΫ)+":"+х;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].ε)+","+τ(Ў[i].В)+","+τ(Ў[i].ώ);return s;}void ΗΔ(String s){Ў.Clear();ȫ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;ȫ=f[0]=="1";ΑΩ=μ(f[1]);ΑΨ=μ(f[2]);ΑΪ=μ(f[3]);ΑΫ=μ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ё p=new ё();if(!long.TryParse(e[0],out p.λ)) continue;p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Ў.Add(p);}}const String ΒΟ="[PAMDCK]";const float Ιέ=120f;class Δμ{public long Ї;public int Ɣ;public long Γθ;public bool Ιξ;public DateTime ΙЅ,Θζ;
}IMyBroadcastListener ΙЂ=null;List<Δμ>ΙЃ=new List<Δμ>();List<IMyShipConnector>ΚΒ=new List<IMyShipConnector>();List<IMySensorBlock>ΚΞ=new List<IMySensorBlock>();List<IMyCameraBlock>ΙЊ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΚΝ=new List<MyDetectedEntityInfo>();long Ιρ=0;
int Ιπ=0;long Ιο=0;DateTime Ις=DateTime.Now;String ΙЁ="";int ΙЄ=0;IMyCubeGrid ΚΓ=null;IMyBroadcastListener ΑІ=null;int Ȃ=0;DateTime Εμ=DateTime.Now;DateTime Ζγ=DateTime.MinValue;long Ζδ=0;int Ζε=0;long ţ=0;bool Ʈ=false;int ΓΦ=0;List<Vector3>ư=new List<Vector3>();List<float>Δά=new List<float>();
bool ΑΧ=false;bool ΑΦ=false;Vector3 Ελ=new Vector3();bool ż(){return Ȃ!=0&&(DateTime.Now-Ζγ).TotalSeconds<3;}bool ȏ(int Ɣ){return ż()&&Ζδ==ъ&&Ζε==Ɣ;}bool ΕΑ(){return ż()||(DateTime.Now-Εμ).TotalSeconds<3;}void Βϋ(int Ɣ){if(Ȃ==Ɣ) return;Ȃ=Ɣ;Εμ=DateTime.Now;ΓΦ=3;
}void Βϊ(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΟ,"D;"+ъ);Ȃ=0;Ʈ=false;}void ΗЂ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑІ==null) ΑІ=IGC.RegisterBroadcastListener(ΒΟ);int ę=20;while(ΑІ.HasPendingMessage&&ę-->0){String Χ=ΑІ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;
try{Ηδ(Χ);}catch{}}if(Υ!=Α.Ѕ){ΑΧ=false;ΑΦ=false;}if(Ȃ==1&&О(MyShipConnectorStatus.Connected)!=null) Βϊ();if(Ȃ==2&&!Ʈ&&Υ!=Α.ǒ&&
Vector3.Distance(ί,е().Δ)>θ*3) Βϊ();if(Ȃ!=0&&++ΓΦ>=3){ΓΦ=0;IGC.SendBroadcastMessage(ΒΟ,"R;"+ъ+";"+Ȃ+";"+х+";"+(ȫ?1:0));}}void Ηδ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Εφ=false;String[]ΕΚ=f[5].Split(',');for(int i=0;i<ΕΚ.Length;i++) if(ΕΚ[i]==ъ.ToString()) Εφ=true;
if(!Εφ) return;Ζγ=DateTime.Now;Ζδ=long.Parse(f[2]);Ζε=int.Parse(f[3]);ţ=long.Parse(f[4]);ư.Clear();Δά.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ư.Add(μ(e[0]));Δά.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εξ=f[6].Split(':');List<ё>Т=new List<ё>();for(int i=0;i<Εξ.Length;i++){String[]e=Εξ[i].Split(',');if(e.Length<4) continue;ё p=new ё();p.λ=long.Parse(e[0]);p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖЊ(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;
Βϋ(1);if(!ΕΑ()) return true;if(!ȏ(1)){Μ="Waiting for dock clearance";return false;}ΒΕ();return true;}bool ΒΕ(){Ƌ=false;ё Ĩ=null;if(ţ!=0&&ţ!=х&&ȫ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==ţ) Ĩ=Ў[i];long ΒΑ=Ĩ==null?0:Ĩ.λ;if(ΒΑ==ȅ&&(ΒΑ==0)==(њ==null)) return false;ȅ=ΒΑ;њ=Ĩ==null?null:ΒΣ(Ĩ.ε,Ĩ.В,Ĩ.ώ);return true;}bool ķ(Vector3 p){for(int i=0;i<ư.Count;i++) if(Vector3.Distance(p,ư[i])<Δά[i]+θ*0.75f) return true;
return false;}bool ΗΡ(Vector3 Ƈ,Vector3 ŗ){ΑΧ=false;if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return false;if(Ȃ!=1&&Vector3.Distance(ί,Θ.Δ)>dockRequestDistance) return false;Βϋ(1);if(!ż()||ȏ(1)||ư.Count==0) return false;if(!ķ(Ƈ)&&!ķ(ŗ)&&!ķ(ί)) return false;ΑΧ=true;return true;
}bool Δε(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;Βϋ(2);if(!ΕΑ()) return true;if(!ȏ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float Ιγ=600f;class ΔЄ{public long λ;public int Ǿ=-1;public bool ΒΑ;public float ğ,ň;public DateTime ΙЍ;}class ΔЃ{public String ĥ;
public int w,h;public float Ζϊ,Ζσ;public String ф;public List<int>ΙЌ=new List<int>();public List<ΔЄ>ΙЏ=new List<ΔЄ>();}List<ΔЃ>ΙЉ=new List<ΔЃ>();IMyBroadcastListener ΙЈ=null;int ΙЇ=0;const float Ιν=8f;class ΕΖ{public long λ;public Vector3 ε,į,Ō;public float ΙϏ,ğ,ň,Ακ=-1;public bool Αβ;
public String Ă="";public int Αϋ;public DateTime ΑЋ;}class ΕΕ{public long a,b,ΚΜ;public DateTime Θζ;}List<ΕΖ>ΚΖ=new List<ΕΖ>();List<ΕΕ>ΚΔ=new List<ΕΕ>();int ΚΕ=0;void ΗΑ(string ѓ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}