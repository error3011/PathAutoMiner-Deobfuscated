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
public enum Ζ{ΑΓ,ŵ,ǐ,р,Ϋ}public enum Α{ж,ȋ,ķ,Ǹ,Ȧ,Ѕ,Ю,
ȥ,ĵ,Ǣ,Đ,Č,ȡ,
Ŷ,ǻ,Ǔ,ď,ΑΕ,Ʒ,
ΒЇ,ΑΔ,Ŭ,Ƹ,Ȩ}public enum Κ{з,ĸ,Ϗ,ΒΣ,Ʋ,ǁ,ǂ,ı,Ĳ}public enum п{None,ď,ж,Δλ}public enum Ϊ{Main,ΔЎ,Ŵ,ų,ΒЃ,ư,
ΒΓ,Δό,Δύ,Δϊ,Ης,Ιι,
Ιη,Ιθ,Βώ,ΕΑ,Ǌ}public enum а{ǈ,Ū,Ητ,ΔϏ,Ϋ}public enum ǒ{ǌ,ΖІ}public enum д{Ǉ,ΒΩ,ΕΓ,Ů}public enum Ѐ{Βμ,Δθ,ΒΝ,ΒΤ,Ƶ,Βε,Ƽ}public enum я{ΒΟ,ΒΘ,ΒΙ}public enum З{ů,ΗΣ,č}public enum ι{Βξ,Βς,Βσ,ΔЋ,ΔЊ,
Βν,ǅ,Ǆ,Βπ,
Βρ,Βο}public enum ΰ{ǳ,ȍ,ǲ,Ư,ΒΖ,ΒΗ}public enum Ļ{Δπ,Δί,Ιέ}public enum с{ǋ,Δξ,Ηή,Βλ}public enum Ǵ{None,Βχ,Ηδ,Ηέ}public enum σ{Ż,Ʈ,ƿ,Ů,Βι,ǀ,Βκ,Ηϊ,Βτ}public enum Ια{ΑΓ,Ιγ,Ιβ,Ιε,Ιή,Ιά,Ιδ}class Ψ{public bool γ=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 υ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float ΚΗ=0;public float[]ǟ=null;public Ψ(){}public Ψ(Ψ δ){γ=δ.γ;Δ=δ.Δ;α=δ.α;β=δ.β;υ=δ.υ;ν=δ.ν;
Ч=δ.Ч;ǟ=δ.ǟ;}public Ψ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 υ,Vector3 ν){this.Δ=Δ;this.α=α;this.β=β;this.υ=υ;this.Џ=0;this.ν=ν;}public void ΖЃ(List<IMyThrust>ΑΘ,List<string>ȇ){ǟ=new float[ȇ.Count];for(int i=0;i<ǟ.Length;i++)
ǟ[i]=-1;for(int i=0;i<ΑΘ.Count;i++){string ю=Δψ(ΑΘ[i]);int Β=ȇ.IndexOf(ю);if(Β!=-1)
ǟ[Β]=А(ΑΘ[i].MaxEffectiveThrust,ΑΘ[i].MaxThrust);}}}class Βφ{public Ψ Њ=null;public List<Vector3>Ţ=new List<Vector3>();public float Ƈ=0;public float ΔΣ=0;public float ŀ=0;public float ΓΪ=0;public Vector3 Αά=new Vector3();}class ļ{public ι Ќ=ι.Βξ;public float ǖ=0;public float ȯ=0;
public string Ǘ="";public string ǚ="";DateTime ΕЏ;public bool ΓІ=false;private bool ΓΝ=false;public bool ΒΛ(bool Ȇ){if(Ȇ){ȯ=0;ΓΝ=false;return false;}ΓΝ=true;return ȯ>ǖ;}public void Ηε(){ΒΛ(true);ΓІ=false;}public void ΗІ(){if(ΓΝ)
if((DateTime.Now-ΕЏ).TotalSeconds>1){ȯ++;ΕЏ=DateTime.Now;}}public bool Ββ(){switch(Ќ){case ι.Βς:return true;case ι.Βσ:return true;}return false;}}class ѐ{public String φ="";public String ς="";public int ǔ=0;public З Ő=З.č;public ѐ(String φ,String ς,int ǔ,З Ő){this.φ=φ;this.ς=ς;
this.ǔ=ǔ;this.Ő=Ő;}}class ΗΑ{public ΗΑ(Vector3 ε,float ό){this.ε=ε;this.ό=ό;}public Vector3 ε;public float ό;}class ĺ{public DateTime Θε;public DateTime Εή;public String λ="";public String φ="";public String ΚΙ="";public String Μ="";public String ΙЏ="";public String ΙЈ="";public String ΓЅ="";
public Vector3 Δ=new Vector3();public List<ѐ>Ъ=new List<ѐ>();public Ζ Η=Ζ.ΑΓ;public Ǵ Ȫ;public float œ;public float Ņ;public int љ;public int Ιύ=0;public int ΚΕ=0;public bool Ιί(){return(DateTime.Now-Θε).TotalSeconds>10;}public bool ΗΦ(){if(Ȫ!=Ǵ.Βχ) return false;return(DateTime.Now-Εή).TotalSeconds>=4;
}public ĺ(String λ){this.λ=λ;}public Ǵ Ιζ(String ě,bool ΘІ,bool є,bool ΘΓ){if(ě==""&&!є) return Ǵ.None;if(Ȫ==Ǵ.Βχ&&ΗΦ())
Ȫ=Ǵ.Ηέ;if(ΘІ){ΓЅ=ě;Ȫ=Ǵ.Βχ;Εή=DateTime.Now;return Ȫ;}else if(є){ΓЅ="";Ȫ=Ǵ.None;}else if(ΓЅ==ě){if(ΘΓ) Ȫ=Ǵ.Ηδ;return Ȫ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 Αύ=new Vector3();Vector3 ί=new Vector3();Vector3 Αί=new Vector3();Vector3 Ɣ=new Vector3();Vector3 Ɗ=new Vector3();
DateTime ΕЎ=new DateTime();bool ƍ=true;int ΔΚ=0;bool ă=false;bool Αθ=false;bool Ƙ=true;bool Ζύ=false;bool Ŏ=false;bool Αε=false;float Ȝ=0;float Ī=0;int ć=0;int Ƨ=0;int ΑЀ=0;int ΓΕ=0;float Œ=0;float Ł=0;double Γϋ=0;float ſ=0;List<int>ŋ=new List<int>();List<int>ŝ=new List<int>();
Ζ Η=Ζ.ΑΓ;int ύ=0;float Ǡ=0;float Ǳ=0;float ζ=0;float ΑЁ=0,ΓЎ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΜ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȉ=new List<IMyThrust>();List<IMyGyro>Αΰ=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>Ć=new List<IMyReactor>();List<IMyConveyorSorter>ΑЄ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>ż=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ń=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ė=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>Ĉ=new List<IMyTextSurface>();List<IMyTextPanel>Ɔ=new List<IMyTextPanel>();
IMyTerminalBlock ќ=null;Ψ Θ=new Ψ();Ψ Γι=new Ψ();Ψ Γμ=new Ψ();bool ȉ=false;bool ť=false;bool Ŧ=false;bool Αη=false;List<String>ȇ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>Ƭ=new List<Ψ>();int ΓΣ=0;double Ζώ=0;Ψ Ε=new Ψ();Vector3 њ;Vector3 ș;Κ ή=Κ.з;ǒ ģ=ǒ.ǌ;int Н=0;
int б=0;double Ƒ=0;bool ΑΡ=false;ļ ϊ=new ļ();ļ ϋ=new ļ();д л=д.Ǉ;bool Ċ=true;ǒ ȝ=ǒ.ǌ;int й=3;int и=3;int Ц=30;я у=я.ΒΟ;bool Ƞ=true;bool ō=true;bool ņ=true;bool ī=false;Ѐ М=Ѐ.Βμ;bool ȟ=true;float Ǭ=90;float ч=20;float Ы=5;float ш=20;float Ǧ=0.70f;float Ũ=1.50f;
float ŧ=2.50f;float İ=0f;float Ġ=0f;Α Υ;Κ ц;Α ơ;Βφ η=null;int Ш=0;int Щ=0;int Й=0;int ȗ=0;int Ĩ=30;int У=0;bool Ђ=true;Vector3 щ;double К=0;bool ΑϏ=false;int ΔΧ=0;int Ѓ=0;int ΒЎ=0;int ǭ=0;int ΖΡ=0;Vector3 ƚ=new Vector3();float к=0;float Э=0;int į=0;Ψ ΑЅ=null;
float Γο=0;float ΑΚ=0;float С=0;float Ħ=0;bool Γκ=false;bool Ĭ=false;bool ƨ=false;bool Ȕ=false;bool Ȑ=false;DateTime ΘΡ=new DateTime();Ψ Ĝ=null;Ψ Εγ=null;float Ǽ=0;п ǧ;float ȕ=0;float ĭ=0;String Γφ="";float Αλ=0;float Ņ=0;float œ=0;List<ѐ>Ǚ=new List<ѐ>();float О=0;
bool Ɵ=false;bool Ζσ=false;bool Ζΰ=false;bool ΑЉ=false;bool Ζα=false;float Γω=0;float ΓЄ=0;Vector3 Εφ=new Vector3();Vector3 χ=new Vector3();Vector3 ΑЊ=new Vector3(1,1,1);float ΔΙ=1;Vector3 Ƥ=new Vector3();bool Αζ=false;bool Γβ=false;bool Γσ=false;bool ΕΞ=false;float ΑΟ=0;
float ΑΗ=2;Vector3 Ζρ;Vector3 ΔΘ;Vector3 ΔΗ;float[,]ǡ=new float[3,2];Dictionary<String,float[,]>џ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζϋ=new Dictionary<String,float>();int ΒБ=0;Dictionary<String,float[]>Ț=new Dictionary<string,float[]>();float ΓЇ;bool ΑΣ=false;
int ΑΙ=0;int Ā=0;int Αγ=0;int ġ=0;String Μ="";int[]ΖΙ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΚ=false;int Αυ=0;int љ=0;Ϊ Ѝ=Ϊ.Main;int ΙΑ=0,ΙΒ=0,ΙΓ=0,ΙΔ=0;int ΘЂ=0,ΘЃ=0;bool Ń=false;bool ΕΟ=false;String ΓЏ="";String ΓΔ="";IMyBroadcastListener ȑ=null;
bool Ευ=true;List<ĺ>Ιτ=null;ĺ ΙЋ=null;int Ιυ=0,Ιφ=0;int Ιχ=0,Ιψ=0;int Ιω=0,Ιϊ=0,Ιϋ=0;String Θδ="";Exception Ȗ=null;bool Αώ=false;Κ ş=Κ.з;bool Εζ=false;bool ΕЍ=true;bool ΓЌ=false;String Ζό="";bool Ʀ=false;bool ΔΓ=true;int Αδ=0;int[]ΙЎ=null;Vector3 ΓΖ=new Vector3();
bool Εό=false;bool Ǒ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βγ(Me,controllerTag,true))
Η=Ζ.р;π(Ϊ.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЋ=Ηζ();if(Η==Ζ.ΑΓ){List<IMyShipDrill>Ερ=new List<IMyShipDrill>();List<IMyShipGrinder>Εύ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Ερ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Εύ,q=>q.CubeGrid==Me.CubeGrid);if(Ερ.Count>0)
{Η=Ζ.ŵ;Μ="Miner mode enabled!";}else if(Εύ.Count>0){Η=Ζ.ǐ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;ǹ(а.Ϋ);}}if(ΓЋ==с.ǋ) Ƙ=false;if(ΓЋ==с.Δξ) Μ="Data restore failed!";if(ΓЋ==с.Βλ) Μ="New version, wipe data";}}void Main(string ѓ,UpdateType ΙΜ){try{if(Ȗ!=null){if(ѓ.Trim().ToUpper()=="RETRY")
{Ȗ=null;ƍ=true;ć=0;if(Η!=Ζ.р) ψ();Μ="Restarted after error";}else{ΗΞ();ΔЌ();return;}}Αθ=(ΙΜ&UpdateType.Update10)!=0;if(Αθ) ΔΚ++;ă=ΔΚ>=10;if(ă) ΔΚ=0;if(Αθ){ΑΙ++;if(ΑΙ>4) ΑΙ=0;ΑЀ=(int) Math.Max(0,10-(DateTime.Now-ΕЎ).TotalSeconds);}if(ѓ!="") Θδ=ѓ;if(Η!=Ζ.р)
Ησ(ѓ);else
ΖА(ѓ);ΖΚ=false;try{int Εδ=Runtime.CurrentInstructionCount;float ƛ=А(Εδ,Runtime.MaxInstructionCount);if(ƛ>0.90) Μ="Max. instructions >90%";if(ƛ>Œ) Œ=ƛ;if(ΑΣ){ŋ.Add(Εδ);while(ŋ.Count>10) ŋ.RemoveAt(0);Ł=0;for(int i=0;i<ŋ.Count;i++) Ł+=ŋ[i];Ł=А(А(Ł,ŋ.Count),Runtime.MaxInstructionCount);
double Γπ=Runtime.LastRunTimeMs;if(Αε&&Γπ>Γϋ) Γϋ=Γπ;ŝ.Add((int)(Γπ*1000f));while(ŝ.Count>10) ŝ.RemoveAt(0);ſ=0;for(int i=0;i<ŝ.Count;i++) ſ+=ŝ[i];ſ=А(ſ,ŝ.Count)/1000f;}}catch{Œ=0;}}catch(Exception e){Ȗ=e;}}void Ησ(string ѓ){bool ΑЂ=false;String ΕΫ="";if(ύ<=1&&!ΔЂ(ѓ))
ΔЍ(ѓ);int Θο=50;while(ȑ!=null&&ȑ.HasPendingMessage&&Θο-->0){MyIGCMessage ρ=ȑ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String ƞ="";if(ύ<=1&&Ηί(ref Χ,out ƞ,out ΕΫ)&&ƞ==BROADCAST_SELF_ID){ΔЍ(Χ);ΑЂ=true;break;}}bool ΕЈ=Αε&&Υ==Α.ж&&!Ŏ&&!ΑЂ&&ć==0&&!Αη;
if(ă&&Υ!=Α.ж) ΑЂ=true;if((Αθ&&!ΕЈ)||(ă&&ΕЈ)){if(ć==0&&(ΑЀ<=0||ƍ)){Ζύ=ύ>0;ύ=0;ć=1;Ǻ();Ηι();ΗΤ();ǵ("Scan 1");}else if(ć==1){ć=2;Ǻ();Ηκ();ǵ("Scan 2");}else if(ć==2){ć=0;Ǻ();Ηλ();Ημ();if(ύ<=1) ΔІ();ǵ("Scan 3");ΕЎ=DateTime.Now;if(ύ<=1&&Ƙ)
Αύ=ƺ(Φ,Φ.CenterOfMass);Ƙ=false;if(ƍ){ƍ=false;bool ΙΠ=ή==Κ.Ϗ;ψ();if(ΕЍ&&resumeAfterRestart&&ΙΠ&&ύ<=1) ΓЌ=true;}ΕЍ=false;if(Ζύ&&ύ==0) Μ="Setup complete";}else{if(ή==Κ.Ϗ&&Η!=Ζ.Ϋ){Ǻ();ΖЀ();ǵ("Inv balance");}Ǻ();switch(Ƨ){case 0:Ηώ();break;case 1:ΗЂ();break;case 2:Ηύ();break;
case 3:ΗЉ();break;case 4:ΗЁ();break;case 5:ΖЇ();break;case 6:ΖЅ(Φ);break;}ǵ("Update: "+Ƨ);Ƨ++;if(Ƨ>6){Ƨ=0;Αε=true;if(ş!=Κ.з){switch(ş){case Κ.ı:ȣ();break;case Κ.Ĳ:ȣ();break;case Κ.Ϗ:ȣ();break;case Κ.ǁ:ƹ();break;case Κ.ǂ:đ();break;}ş=Κ.з;}}}if(!ƍ){if(!Ƿ(Φ,true)){Φ=null;
Ƙ=true;ύ=2;}if(ύ>=2&&Υ!=Α.ж)
ψ();if(ύ<=1){Ī=Φ.CalculateShipMass().PhysicalMass;Ȝ=(float) Φ.GetShipSpeed();ί=Ηό(Φ,Αύ);Αί=Φ.WorldMatrix.Forward;Ɣ=Φ.WorldMatrix.Left;Ɗ=Φ.WorldMatrix.Down;ΗЅ();ΗЇ();ΗϏ();if(ΓЌ){ΓЌ=false;ȣ();if(Υ!=Α.ж) Μ="Job resumed after restart";}if(Υ!=Α.ж){Ŏ=false;ΔБ(false);Δγ(false);String ΘЋ=Βή(Υ)+" "+(int) Υ;
Ǻ();ΗЃ();ΒІ(false);ǵ(ΘЋ);ΗЈ();ΗЄ();Ǻ();ΗЀ();ǵ("Thruster");Ǻ();ΕΚ();ǵ("Gyroscope");}else{if(Ŏ){if(ƾ()){Х(Ɗ,Αί,Ɣ,0.25f,true);ΕΚ();Μ="Aligning to planet: "+Math.Round(ΑΟ-0.25f,2)+"°";if(Αζ) Βα(true,true);}else Βα(true,true);}}Ʀ=false;}}}Ǻ();ΔЌ();ǵ("Print");if(ΑЂ||ΓΕ<=0){Ǻ();ΖЁ(ΕΫ);
ǵ("Broadcast");ΓΕ=4;}else if(ă) ΓΕ--;}void ΔЍ(string ѓ){if(ѓ=="") return;var Е=ѓ.ToUpper().Split(' ');Е.DefaultIfEmpty("");var ě=Е.ElementAtOrDefault(0);var Ę=Е.ElementAtOrDefault(1);var ž=Е.ElementAtOrDefault(2);var ΕΣ=Е.ElementAtOrDefault(3);String ā="Invalid argument: "+ѓ;bool ΔΡ=false;
switch(ě){case "UP":this.ΔЈ(false);break;case "DOWN":this.ΔЇ(false);break;case "UPLOOP":this.ΔЈ(true);break;case "DOWNLOOP":this.ΔЇ(true);break;case "APPLY":this.Δα(true);break;case "MRES":љ=0;break;case "STOP":this.ψ();break;case "PATHHOME":{this.ψ();this.ΒЂ();}break;case "PATH":{this.ψ();this.ΒЂ();Θ.γ=true;}break;
case "START":{this.ψ();ΒЁ();}break;case "ALIGN":{Βα(!Ŏ,false);}break;case "CONT":{this.ψ();this.ȣ();}break;case "JOBPOS":{this.ψ();this.đ();}break;case "HOMEPOS":{this.ψ();this.ƹ();}break;case "FULL":{Ɵ=true;}break;case "RESET":{Αώ=true;ύ=2;}break;case "EXPORT":{ΗΖ();}break;
case "IMPORT":{ΗΠ(Ę=="JOB");}break;default:ΔΡ=true;break;}if(Η!=Ζ.Ϋ){switch(ě){case "SHUTTLE":{ΗΔ();}break;case "CFGS":{if(!ΖЎ(Ę,ž,ΕΣ)) Μ=ā;}break;case "CFGB":{if(!ΖЍ(Ę,ž)) Μ=ā;}break;case "CFGL":{if(!Ē(ref Ǭ,true,σ.ƿ,Ę,"")||!ΖЏ(ž))
Μ=ā;}break;case "CFGE":{if(!Ē(ref Ы,true,σ.ǀ,Ę,"IG")||
!Ē(ref ч,true,σ.Βι,ž,"IG")||
!Ē(ref ш,true,σ.Βκ,ΕΣ,"IG"))
Μ=ā;}break;case "CFGA":{if(!Ē(ref Ǧ,false,σ.Ʈ,Ę,"")) Μ=ā;}break;case "CFGW":{if(!Ē(ref Ũ,false,σ.Ż,Ę,"")||
!Ē(ref ŧ,false,σ.Ż,ž,""))
Μ=ā;}break;case "NEXT":{ΒΒ(false);}break;case "PREV":{ΒΒ(true);}break;default:if(ΔΡ) Μ=ā;break;}}else{switch(ě){case "UNDOCK":{Ʀ=true;}break;default:if(ΔΡ) Μ=ā;break;}}}String ΗΚ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΔ(){ψ();Η=Ζ.Ϋ;ǹ(а.Ϋ);Ε.γ=false;Θ.γ=false;ξ=null;ο.Clear();ή=Κ.з;}void Βα(bool Ј,bool ф){if(!Ј) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ј){Ŏ=false;ΕΔ();ǉ(false,0,0,0,0);return;}if(ƾ()) Ŏ=true;}bool ΖЎ(String ΙΣ,String ΘΫ,String ΘΜ)
{bool ΙΞ=ή==Κ.Ϗ;int w,h,d;if(int.TryParse(ΙΣ,out w)&&int.TryParse(ΘΫ,out h)&&int.TryParse(ΘΜ,out d)){this.ψ();й=w;и=h;Ц=d;ĕ(false);ź(false,false);if(ΙΞ) ȣ();return true;}return false;}bool ΖЏ(String ΕΡ){if(ΕΡ=="ON"){Ƞ=true;return true;}if(ΕΡ=="OFF"){Ƞ=false;return true;}
return false;}bool ΖЍ(String Επ,String ΑΠ){bool ƙ=true;if(Επ=="HOME") Ċ=true;else if(Επ=="STOP") Ċ=false;else ƙ=false;if(ΑΠ=="HOME") л=д.Ǉ;else if(ΑΠ=="STOP") л=д.ΕΓ;else if(ΑΠ=="JOB") л=д.ΒΩ;else if(ΑΠ=="IG") л=д.Ů;else ƙ=false;return ƙ;}void π(Ϊ ΓЂ){ΖΙ[(int) Ѝ]=љ;
љ=ΖΙ[(int) ΓЂ];if(ΓЂ==Ϊ.Ης) љ=0;Ѝ=ΓЂ;if(Η!=Ζ.р)
Ĕ(Ѝ==Ϊ.Ŵ,false,0,0);ΖΚ=true;}void ΔЈ(bool Γτ){if(љ>0) љ--;else if(Γτ) љ=Αυ-1;}void ΔЇ(bool Γτ){if(љ<Αυ-1) љ++;else if(Γτ) љ=0;}String ǹ(а ρ){switch(ρ){case а.ǈ:Μ="Job is running";break;case а.Ū:Μ="Connector not ready!";break;case а.Ητ:Μ="Ship modified, path outdated!";break;
case а.ΔϏ:Μ="Interrupted by player!";break;case а.Ϋ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΝ(ǒ ε){switch(ε){case ǒ.ǌ:return "Top-Left";case ǒ.ΖІ:return "Center";default:return "";}}String ΗΘ(я Ɨ){switch(Ɨ){case я.ΒΘ:return "Auto"+(Η==Ζ.ŵ?" (Ore)":"");case я.ΒΙ:return "Auto (+Stone)";
case я.ΒΟ:return "Default";default:return "";}}String Βά(Κ Ф){switch(Ф){case Κ.з:return "No job";case Κ.ĸ:return "Job paused";case Κ.Ϗ:return "Job active";case Κ.ı:return "Job active";case Κ.Ĳ:return "Job active";case Κ.ΒΣ:return "Job done";case Κ.Ʋ:return "Job changed";case Κ.ǁ:return "Move home";
case Κ.ǂ:return "Move to job";}return "";}String Δσ(д ΘΒ){switch(ΘΒ){case д.Ǉ:return "Return home";case д.ΒΩ:return "Fly to job pos";case д.ΕΓ:return "Stop";case д.Ů:return "Ignore";}return "";}String ΗΙ(Ѐ Ɨ){switch(Ɨ){case Ѐ.Βμ:return "Off";case Ѐ.ΒΤ:return "Drop pos (Stone) ";case Ѐ.Ƶ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δθ:return "Cur. pos (Stone)";case Ѐ.ΒΝ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βε:return "In motion (Stone)";case Ѐ.Ƽ:return "In motion (Sto.+Ice)";}return "";}String Δρ(п Ф){switch(Ф){case п.None:return "No batteries";case п.ď:return "Charging";case п.Δλ:return "Discharging";}return "";}String Βή(Α Ф)
{String ΙЃ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ȋ:return "Flying to XY position";case Α.ķ:return Η==Ζ.ǐ?"Grinding":"Mining";case Α.Ǹ:return "Returning";case Α.Ŭ:return "Flying to drop pos";case Α.Ȧ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȥ:return "Flying to path";case Α.ĵ:return "Flying to job position";case Α.Ǣ:return "Approaching dock";case Α.Đ:return "Docking";case Α.Č:return "Aligning";case Α.ȡ:return "Aligning";case Α.Ŷ:return "Retry docking";case Α.ǻ:return "Unloading";case Α.Ȩ:return Ζό;
case Α.Ǔ:return "Undocking";case Α.ď:return "Charging batteries";case Α.ΑΕ:return "Waiting for uranium";case Α.Ʒ:return "Filling up hydrogen";case Α.ΒЇ:return "Waiting for ejection";case Α.ΑΔ:return "Waiting for ejection";case Α.Ƹ:return "Flying to drop pos";}return "";}String Δω(ι Ќ){switch(Ќ)
{case ι.Βξ:return "On \"Undock\" command";case ι.Βς:return "On player entered cockpit";case ι.ΔЋ:return "On ship is full";case ι.ΔЊ:return "On ship is empty";case ι.Βσ:return "On time delay";case ι.ǅ:return "On batteries empty(<25%)";case ι.Ǆ:return "On batteries empty(=0%)";case ι.Βν:return "On batteries full";
case ι.Βρ:return "On hydrogen empty(<25%)";case ι.Βο:return "On hydrogen empty(=0%)";case ι.Βπ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΫ,int ȳ,bool Ξ,String Ɠ){Αυ+=1;if(ΖΫ==ȳ) Ɠ=">"+Ɠ+(ΑΙ>=2?" .":"");else Ɠ=" "+Ɠ;Γ+=Ɠ+"\n";return ΖΫ==ȳ&&Ξ;}String Δα(bool Ξ)
{int Β=0;int Ο=љ;Αυ=0;String Ζή="———————————————\n";String Ť="--------------------------------------------\n";String Γ="";Γ+=Βά(ή)+" | "+(Θ.γ?"Ready to dock":"No dock")+"\n";Γ+=Ζή;double Ǯ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ϊ.Main){bool Ă=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЂ();
if(Η==Ζ.ŵ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.Ŵ);if(Η==Ζ.ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.Ŵ);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.Βώ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȣ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƹ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) đ();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ă) π(Ϊ.Ǌ);else π(Ϊ.ư);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.Δό);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δϊ);}else if(Ѝ==Ϊ.Ŵ){double ΙΡ=Math.Round(й*Ǡ,1);double ΘΪ=Math.Round(и*Ǳ,1);String Є="";if(Π(ref Є,Ο,Β++,Ξ," Start new job!")) ΒЁ();if(Π(ref Є,Ο,Β++,Ξ," Change current job")){ź(false,false);π(Ϊ.Main);}if(Π(ref Є,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΡ+"m)")){ĳ(ref й,5,20,1);ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Width -")){ĳ(ref й,-5,20,-1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΘΪ+"m)")){ĳ(ref и,5,20,1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Height -")){ĳ(ref и,-5,20,-1);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth + ("+(у==я.ΒΟ?"Depth":"Min")+": "+Ц+"m)")){ĳ(ref Ц,5,50,2);ĕ(true);}
if(Π(ref Є,Ο,Β++,Ξ," Depth -")){ĳ(ref Ц,-5,50,-2);ĕ(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth mode: "+ΗΘ(у))){у=Ȥ(у);}if(Π(ref Є,Ο,Β++,Ξ," Start pos: "+ΗΝ(ȝ))){ȝ=Ȥ(ȝ);}if(Η==Ζ.ǐ&&у==я.ΒΙ) у=Ȥ(у);Γ+=ΔЏ(8,Є,Ο,ref ΘЂ);}else if(Ѝ==Ϊ.Βώ){float[]Εθ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΑ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δω(ϊ.Ќ))) ϊ.Ќ=Ȥ(ϊ.Ќ);if(!ϊ.Ββ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΫ((int) ϊ.ǖ))) ϊ.ǖ=Δι(ϊ.ǖ,Εθ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δω(ϋ.Ќ))) ϋ.Ќ=Ȥ(ϋ.Ќ);
if(!ϋ.Ββ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΫ((int) ϋ.ǖ))) ϋ.ǖ=Δι(ϋ.ǖ,Εθ);}else if(Ѝ==Ϊ.ΕΑ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЁ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Βώ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǘ!=""?ϊ.Ǘ:"-"))) ϊ.Ǘ=ǃ(ref ΙΑ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǚ!=""?ϊ.ǚ:"-"))) ϊ.ǚ=ǃ(ref ΙΓ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǘ!=""?ϋ.Ǘ:"-"))) ϋ.Ǘ=ǃ(ref ΙΒ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǚ!=""?ϋ.ǚ:"-"))) ϋ.ǚ=ǃ(ref ΙΔ);
}else if(Ѝ==Ϊ.ų){String ΖΒ=Ǭ+" %";if(ă) Ā++;if(Ā>1){Ā=0;Αγ++;if(Αγ>1) Αγ=0;}bool[]ΓБ=new bool[]{Ć.Count==0,ǧ==п.None,і.Count==0};int Ζψ=0;while(true){Ζψ++;ġ++;if(ġ>ΓБ.Length-1) ġ=0;if(Ζψ>=ΓБ.Length) break;if(!ΓБ[ġ]) break;}bool Ă=Η==Ζ.Ϋ;if(!Ă&&Ƞ&&О!=-1&&Αγ==0)
ΖΒ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){ψ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ă) π(Ϊ.ư);else π(Ϊ.Ǌ);if(!Ă){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΒ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ʀ=true;Γ+=Ť;if(!Ă) Γ+="Progress: "+Math.Round(Ƒ,1)+" %\n";Γ+="State: "+Βή(Υ)+" "+Ǯ+"m \n";Γ+="Load: "+Αλ+" % Max: "+ΖΒ+" \n";if(ġ==0) Γ+="Uranium: "+(Ć.Count==0?"No reactors":Math.Round(ĭ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ġ==1) Γ+="Battery: "+(ǧ==п.None?Δρ(ǧ):Ǽ+"% "+(ч==-1||Ă?"":" Min: "+ч+" %"))+"\n";if(ġ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% "+(ш==-1||Ă?"":" Min: "+ш+" %"))+"\n";}else if(Ѝ==Ϊ.ư){String Є="";if(Π(ref Є,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.ų);else π(Ϊ.Main);}
if(Π(ref Є,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ƿ,false);if(Π(ref Є,Ο,Β++,Ξ," Weight limit: "+(Ƞ?"On":"Off"))) Ƞ=!Ƞ;if(Π(ref Є,Ο,Β++,Ξ," Ejection: "+ΗΙ(М))) М=Ȥ(М);if(Π(ref Є,Ο,Β++,Ξ," Toggle sorters: "+(ī?"On":"Off"))){ī=!ī;if(ī) ǥ(ΔΓ);}
if(Π(ref Є,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Є,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǀ,true);if(Π(ref Є,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) г(ref ч,-5,σ.Βι,true);if(Π(ref Є,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-10,σ.Βκ,true);
if(Π(ref Є,Ο,Β++,Ξ," When done: "+(Ċ?"Return home":"Stop"))) Ċ=!Ċ;if(Π(ref Є,Ο,Β++,Ξ," On damage: "+Δσ(л))) л=Ȥ(л);if(Π(ref Є,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΓ);Γ+=ΔЏ(8,Є,Ο,ref ΘЃ);}else if(Ѝ==Ϊ.ΒΓ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.ų);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.ǐ?" Grinder":" Drill")+" inv. balancing: "+(ō?"On":"Off"))) ō=!ō;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.ǐ?" grinders":" drills")+": "+(ņ?"Fwd + Bwd":"Fwd"))) ņ=!ņ;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ũ+"m/s")) г(ref Ũ,0.5f,σ.Ż,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ŧ+"m/s")) г(ref ŧ,0.5f,σ.Ż,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ʈ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+İ*100f+"%")) Δε(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+Ġ*100f+"%")) Δε(false,0.05f);
}else if(Ѝ==Ϊ.Ǌ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ϊ.ų);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ƿ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǀ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δσ(л))) л=Ȥ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ʈ,false);
}else if(Ѝ==Ϊ.ΔЎ){double Θζ=0;if(Σ.Count>0) Θζ=Vector3.Distance(Σ.Last().Δ,ί);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΕ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȉ?"Use old home":(Θ.γ?"Was set! ":"none ")))) ȉ=!ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȉ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) ȉ=!ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(ť?"Use old connector":(Ε.γ?"Was set! ":"none ")))) ť=!ť;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ŧ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ŧ=!Ŧ;Γ+=Ť;Γ+="Wp spacing: "+Math.Round(Ζώ)+"m\n";}else if(Ѝ==Ϊ.ΒЃ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){ψ();π(Ϊ.Main);}
Γ+=Ť;Γ+="State: "+Βή(Υ)+" \n";Γ+="Speed: "+Math.Round(Ȝ,1)+"m/s\n";Γ+="Target dist: "+Ǯ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΧ+"\n";}else if(Ѝ==Ϊ.Δό){List<IMyTerminalBlock>ĝ=Δτ();if(ă) Ā++;if(Ā>=ĝ.Count) Ā=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.Δύ);
Γ+=Ť;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αλ,1)+"% "+Math.Round(Ņ,1)+" / "+Math.Round(œ,1)+"\n";Γ+="Uranium: "+(Ć.Count==0?"No reactors":Math.Round(ĭ,1)+"Kg "+Γφ)+"\n";Γ+="Battery: "+(ǧ==п.None?"":Ǽ+"% ")+Δρ(ǧ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȕ,1)+"% ")+"\n";
Γ+="Damage: "+(ĝ.Count==0?"None":""+(Ā+1)+"/"+ĝ.Count+" "+ĝ[Ā].CustomName)+"\n";}else if(Ѝ==Ϊ.Δύ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ť;Γ+="Next scan: "+ΑЀ+"s\n";Γ+="Drill area: "+Math.Round(Ǡ,1)+"x"+Math.Round(Ǳ,1)+"m Ship: "+Math.Round(ΑЁ,1)+"x"+Math.Round(ΓЎ,1)+"m ("+Math.Round(ζ,1)+"m)\n";
Γ+="Broadcast: "+(Ń?"Online - "+ΓЏ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Œ*100f,1)+"% \n";}else if(Ѝ==Ϊ.Δϊ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ť;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+ΑЀ+"s\n";if(Αώ) Γ="Recompile script now";int Θη=Γ.Split('\n').Length;for(int i=Θη;i<=10;i++) Γ+="\n";Γ+=Ζή;Γ+="Last: "+Μ+"\n";
return Γ;}void Δε(bool Θα,float ў){ψ();ź(true,false);if(Θα) г(ref İ,ў,σ.Βτ,false);else г(ref Ġ,ў,σ.Βτ,false);Δδ();Ĕ(true,true,0,0);}float Δι(float θ,float[]ΑБ){float έ=ΑБ[0];for(int i=ΑБ.Length-1;i>=0;i--)
if(θ<ΑБ[i]) έ=ΑБ[i];return έ;}String ǃ(ref int ȳ){String έ="";if(ȳ>=ΔΜ.Count) ȳ=-1;if(ȳ>=0) έ=ΔΜ[ȳ].CustomName;ȳ++;return έ;}void ΑΑ(string φ){if(ή!=Κ.Ϗ) return;if(φ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(φ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void ĳ(ref int value,int Ɓ,int Ȟ,int ΔΑ){if(Ɓ==0) return;if(value<Ȟ&&ΔΑ>0||value<=Ȟ&&ΔΑ<0){value+=ΔΑ;return;}int ΗЋ=Math.Abs(Ɓ);int ΒЈ=0;int Γύ=1;while(true){ΒЈ+=Γύ*ΗЋ*10;if(Ɓ<0&&value-Ȟ<=ΒЈ) break;if(Ɓ>0&&value-Ȟ<ΒЈ) break;
Γύ++;}value+=Γύ*Ɓ;}void ĕ(bool ΚΘ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ĕ(Ѝ==Ϊ.Ŵ,false,0,0);}bool Ē(ref float value,bool ΘϏ,σ ς,String Ģ,String Θή){if(Ģ=="") return false;float Ř=-1;bool ΕЅ=false;if(Ģ.ToUpper()==Θή) ΕЅ=true;else if(!float.TryParse(Ģ,out Ř)) return false;
else Ř=Math.Max(0,Ř);if(ΘϏ) Ř=(float) Math.Round(Ř);г(ref value,Ř,ς,ΕЅ,false);return true;}void г(ref float value,float ў,σ ς,bool ƥ){г(ref value,value+ў,ς,ƥ,true);}void г(ref float value,float Ŕ,σ ς,bool ƥ,bool ΙΦ){float ǜ=0,Ǜ=0;if(ς==σ.Ż){ǜ=0.5f;Ǜ=10f;}if(ς==σ.Ʈ){ǜ=0.1f;Ǜ=1f;}
if(ς==σ.Ů){ǜ=50f;Ǜ=100f;}if(ς==σ.Βι){ǜ=5f;Ǜ=30f;}if(ς==σ.ǀ){ǜ=1f;Ǜ=25f;}if(ς==σ.ƿ){ǜ=10f;Ǜ=95f;}if(ς==σ.Βκ){ǜ=10f;Ǜ=90f;}if(ς==σ.Ηϊ){ǜ=10f;Ǜ=1800;}if(ς==σ.Βτ){ǜ=0.0f;Ǜ=0.75f;}if(Ŕ==-1&&ƥ){value=-1;return;}if(value==-1) ƥ=false;
bool ΖΤ=Ŕ<ǜ||Ŕ>Ǜ;if(ΖΤ&&ΙΦ){if(Ŕ<value) value=Ǜ;else if(Ŕ>value) value=ǜ;}else value=Ŕ;if(ΖΤ&&ƥ) value=-1;else value=Math.Max(ǜ,Math.Min(value,Ǜ));value=(float) Math.Round(value,2);}void ΒΒ(bool Θϊ){if(Θϊ) Й=Math.Max(0,Й-1);else Й++;ΒΦ();ΒΥ();for(int k=0;k<ȃ.Count&&k<Л.Length;k++) Βό(ȃ[k],k<Й);
Ё=-1;ΒІ(true);}T Ȥ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΕ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΕ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЂ(){Ƭ.Clear();for(int i=0;i<Σ.Count;i++) Ƭ.Add(Σ[i]);Σ.Clear();
Αη=true;Γι=new Ψ(Θ);Γμ=new Ψ(Ε);Θ.γ=false;if(Η==Ζ.Ϋ) Ε.γ=false;for(int i=0;i<џ.Count;i++)
if(!ȇ.Contains(џ.Keys.ElementAt(i)))
ȇ.Add(џ.Keys.ElementAt(i));ȉ=false;ť=false;Ŧ=false;π(Ϊ.ΔЎ);}void ΕΕ(){if(ȉ) Θ=Γι;if(ť) Ε=Γμ;if(Ŧ){Σ.Clear();for(int i=0;i<Ƭ.Count;i++) Σ.Add(Ƭ[i]);}Αη=false;ψ();π(Ϊ.Main);}void ΗЅ(){if(!Αη) return;if(Υ!=Α.ж){ΕΕ();return;}if(!Γι.γ) ȉ=false;if(!Γμ.γ) ť=false;if(Ƭ.Count<=1) Ŧ=false;
IMyShipConnector И=П(MyShipConnectorStatus.Connectable);if(И==null) И=П(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(Ȝ,2)<=0.20) ΓΣ++;else ΓΣ=0;if(ΓΣ>=5){if(Η==Ζ.Ϋ&&(Θ.γ||ȉ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.β=Φ.WorldMatrix.Forward;Ε.υ=Φ.WorldMatrix.Left;
Ε.α=Φ.WorldMatrix.Down;Ε.ν=Φ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.γ=true;Ε.Ч=И.Position;}else{Θ.β=Φ.WorldMatrix.Forward;Θ.υ=Φ.WorldMatrix.Left;Θ.α=Φ.WorldMatrix.Down;Θ.ν=Φ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.γ=true;Θ.Ч=И.Position;}}}double Γξ=-1;if(Σ.Count>0) Γξ=Vector3.Distance(ί,Σ.Last().Δ);
double ΘЉ=Math.Max(1.5,Math.Pow(Ȝ/100.0,2));double ΖΛ=Math.Max(Ȝ*ΘЉ,2);Ζώ=ΖΛ;if((Γξ==-1)||Γξ>=ΖΛ){Ψ Ω=new Ψ(ί,Αί,Ɗ,Ɣ,Φ.GetNaturalGravity());Ω.ΖЃ(Ȉ,ȇ);Σ.Add(Ω);}}Ψ е(){if(Η!=Ζ.Ϋ) return ћ!=null?ћ:Θ;return Εγ;}void Ĺ(Ψ ά,Κ Ф){Εγ=ά;if(ή==Κ.Ϗ) ц=Ф;}bool Ƕ(Ψ ά,float ό,bool ΙΝ,out Vector3 ε)
{if(ΙΝ){Vector3I Γα=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(Γα);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ε=new Vector3();return false;}Vector3 ΘΘ=Я(Φ,Τ.FatBlock.GetPosition()-ί);Vector3 ΘΗ=Я(Φ,Τ.FatBlock.WorldMatrix.Forward);ε=ά.Δ-ΒЅ(ά.β,ά.α*-1,ΘΘ)-ΒЅ(ά.β,ά.α*-1,ΘΗ)*ό;
return true;}else{ε=ά.Δ;return true;}}Vector3 Ȍ(int ΘΕ,int Ŝ,bool Θύ){if(!Θύ&&Εό) return ΓΖ;float ΘΖ=((Н-1f)/2f)-ΘΕ;float ΘЀ=((б-1f)/2f)-Ŝ;ΓΖ=Ε.Δ+Ε.υ*ΘΖ*Ǡ+ș*-1*ΘЀ*Ǳ;Εό=true;return ΓΖ;}Vector3 Βέ(Vector3 ΘЍ,float ΓΟ){return ΘЍ+(њ*ΓΟ);}ΰ ΒΠ(){float Б=-1;ΰ έ=ΰ.ǳ;
if(Η!=Ζ.Ϋ){ΔІ();if(ή!=Κ.з){Vector3 Љ=ē(њ,ș*-1,ί-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(Н*Ǡ)/2f&&Math.Abs(Љ.Y)<=(float)(б*Ǳ)/2f){if(Љ.Z<=-1&&Љ.Z>=-Ĩ*2) return ΰ.Ư;if(Љ.Z>-1&&Љ.Z<ζ*2) return ΰ.ǳ;}if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;}if(Θ.γ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;for(int i=0;i<Σ.Count;i++)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Vector3.Distance(ί,Θ.Δ)<dockDist*ζ) έ=ΰ.ǲ;if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǲ;}}else{Vector3 ε=new Vector3();IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Θ.γ){if(Ȣ(Θ.Δ,ref Б)) έ=ΰ.ǲ;if(Ƕ(Θ,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǲ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΖ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*ζ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ζ)
if(Ȣ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Ε.γ){if(Ȣ(Ε.Δ,ref Б)) έ=ΰ.ǳ;if(Ƕ(Ε,dockDist,true,out ε))
if(Ȣ(ε,ref Б)) έ=ΰ.ǳ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΗ;}}return έ;}bool Ȣ(Vector3 ε,ref float Б){float ό=Vector3.Distance(ε,ί);if(ό<Б||Б==-1){Б=ό;return true;}return false;}void ΒЁ(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){ȣ();return;}Ε.Δ=ί;Ε.ν=Φ.GetNaturalGravity();Ε.β=Αί;Ε.α=Ɗ;Ε.υ=Ɣ;
њ=ќ.WorldMatrix.Forward;ș=Ε.α;if(њ==Φ.WorldMatrix.Down) ș=Φ.WorldMatrix.Backward;ź(true,true);Ν(Α.ȋ);ΒЀ();}void ź(bool ǘ,bool Θώ){if(ή==Κ.з&&!ǘ) return;bool Θπ=ǘ||ή==Κ.ΒΣ||Н!=й||б!=и||ģ!=ȝ;if(Θπ){if(ή!=Κ.з){ή=Κ.Ʋ;Ȍ(Ш,Щ,Θώ);Μ="Job changed, lost progress";}ģ=ȝ;
Н=й;б=и;Щ=0;Ш=0;Л="";Ё=-1;Ħ=0;ȗ=0;С=0;Й=0;ΒІ(true);}}void Βψ(){Р(ί,0);ђ(Ȉ,true);}void ΒЀ(){ǹ(а.ǈ);Βψ();Ÿ(Ȅ,false);ǥ(ΔΓ);ή=Κ.Ϗ;Δγ(true);ц=ή;π(Ϊ.ų);Ηθ();ΑΡ=true;Αδ=0;for(int i=ż.Count-1;i>=0;i--)
if(Ű(ż[i],false)) Αδ++;if(Αδ>0) Μ="Started with damage";}void ψ(){if(ή==Κ.Ϗ){ή=Κ.ĸ;Μ="Job paused";}Ν(Α.ж);ц=ή;ǉ(false,0,0,0,0);ΒЄ();Δΰ(new Vector3(),false);ΕΔ();Βϋ(ChargeMode.Auto);Βύ(false);ΔБ(true);Ȏ(Α.ж);Ĕ(false,false,0,0);ђ(ο,false);ђ(т,true);ǥ(true);ΑϏ=false;ƴ();Ɖ=false;
Βω();ΑΡ=false;Ɵ=false;Ʀ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.ư&&Ѝ!=Ϊ.ΒΓ&&Ѝ!=Ϊ.Ǌ)
π(Ϊ.Main);}void ȣ(){ΰ Д=ΒΠ();if(Η==Ζ.Ϋ){if(!Ε.γ||!Θ.γ) return;ΒЀ();bool Γί=Vector3.Distance(ί,Θ.Δ)<Vector3.Distance(ί,Ε.Δ);if(ş==Κ.ı) Γί=true;if(ş==Κ.Ĳ) Γί=false;if(Γί){Ĺ(Θ,Κ.ı);switch(Д){case ΰ.ΒΖ:Ν(Α.Ȩ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.Ǣ);break;default:Ν(Α.Ǔ);break;
}}else{Ĺ(Ε,Κ.Ĳ);switch(Д){case ΰ.ΒΗ:Ν(Α.Ȩ);break;case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.ȍ:Ν(Α.Ю);break;default:Ν(Α.Ǔ);break;}}}else{if(ή!=Κ.ĸ&&ή!=Κ.Ʋ) return;bool ΙΟ=ή==Κ.Ʋ;ΒЀ();bool Γδ=ű(false)&&Θ.γ;switch(Д){case ΰ.ǳ:Ν(Γδ?Α.ȥ:Α.ȋ);break;case ΰ.ȍ:Ν(Γδ?Α.Ѕ:Α.Ю);break;case ΰ.ǲ:Ν(Γδ?Α.Ǣ:Α.ǻ);break;
case ΰ.Ư:{if(ȗ!=Й||ΙΟ) Ν(Α.Ǹ);else Ν(Α.ķ);}break;default:break;}}}void đ(){if(ή==Κ.з&&!Θ.γ) return;if(Η==Ζ.Ϋ&&(!Ε.γ||!Θ.γ)) return;Μ="Move to job";ΰ Д=ΒΠ();if(Η==Ζ.Ϋ){Ĺ(Ε,Κ.Ĳ);switch(Д){case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ΒΗ:return;default:Ν(Α.Ǔ);break;
}Ȏ(Α.Ȩ);}else{switch(Д){case ΰ.ǳ:Ν(Α.Ю);break;case ΰ.ȍ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.ǻ);break;case ΰ.Ư:Ν(Α.Ǹ);break;default:break;}if(ή==Κ.з) Ȏ(Α.Ю);else Ȏ(Α.ȡ);ΑϏ=true;}Βψ();π(Ϊ.ΒЃ);Ÿ(Ȅ,false);ц=Κ.ǂ;}void ƹ(){if(!Θ.γ) return;Μ="Move home";ΰ Д=ΒΠ();if(Η==Ζ.Ϋ){Ĺ(Θ,Κ.ı);switch(Д)
{case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ǣ);break;case ΰ.ΒΖ:return;default:Ν(Α.Ǔ);break;}Ȏ(Α.Ȩ);}else{if(П(MyShipConnectorStatus.Connected)!=null) return;if(П(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Đ);Ȏ(Α.ǻ);return;}switch(Д){case ΰ.ǳ:Ν(Α.ȥ);break;case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ѕ);break;
case ΰ.Ư:Ν(Α.Ȧ);break;default:break;}Ȏ(Α.ǻ);}Βψ();π(Ϊ.ΒЃ);Ÿ(Ȅ,false);ц=Κ.ǁ;}void Ν(Α Γώ){if(Γώ==Α.ж) ơ=Α.ж;if(ơ!=Α.ж&&Υ==ơ&&Γώ!=ơ){ψ();return;}Ђ=true;Υ=Γώ;}void Ȏ(Α Ф){this.ơ=Ф;}void ΗЃ(){if(Υ==Α.ȋ){if(Ђ){Ѓ=0;if(ȗ!=Й) Ħ=0;ȗ=Й;}if(Ѓ==0){int ƃ=ΖЊ(Й);if(ƃ<0)
{if(!Ƴ()){Ν(Α.Ȧ);return;}if(!ΓΜ||(ƃ==-2)!=ΑΝ){ΓΜ=true;ΑΝ=ƃ==-2;Εΰ=!ΑΝ?ί:Ε.Δ-њ*ζ+
Ε.υ*(Н*Ǡ/2+ΑЁ/2+ζ/2);}Р(Εΰ,10);Μ=ΑΝ?"Waiting for a free hole":"Waiting for the controller";return;}ΓΜ=false;if(ƃ!=Й){Й=ƃ;Ħ=0;ȗ=Й;}Ļ έ=Ηο(Й);if(έ==Ļ.Δί){ή=Κ.ΒΣ;Μ="Job done";if(Ċ&&Θ.γ){Ν(Α.ȥ);Ȏ(Α.ǻ);ц=Κ.ǁ;}else{Ν(Α.ĵ);Ȏ(Α.ȡ);ц=Κ.ǂ;}return;}
if(έ==Ļ.Δπ){Ѓ=1;ђ(ο,true);щ=Ȍ(Ш,Щ,true);Р(щ,10);Х(Ε.α,Ε.β,Ε.υ,false);}}else{if(Ё>=0&&(ΔЃ()?ǩ>=0&&ǩ!=Ё:Βδ(Ё,true))){Ѓ=0;return;}if(К<wpReachedDist){Ν(Α.ķ);return;}}}if(Υ==Α.ķ){if(Ђ){ђ(ο,true);ǥ(false);щ=Ȍ(Ш,Щ,false);Р(Βέ(щ,0),0);Х(Ε.α,Ε.β,Ε.υ,false);
Ѓ=1;к=0;Э=0;ΑΚ=0;Γο=-1;Ĩ=Ц;Γκ=true;}if(!Ƴ()){Ν(Α.Ȧ);return;}if(ű(true)){ΖΡ=Ȋ("","ORE",З.č);if((М==Ѐ.ΒΤ||М==Ѐ.Ƶ||
М==Ѐ.Βε||М==Ѐ.Ƽ)&&Η!=Ζ.ǐ)
Ν(Α.Ŭ);else if((М==Ѐ.Δθ||М==Ѐ.ΒΝ)&&Η!=Ζ.ǐ)
Ν(Α.ΒЇ);else
Ν(Α.Ȧ);return;}С=Vector3.Distance(ί,щ);if(С>Ħ){Ħ=С;Γκ=false;}if(Η==Ζ.ǐ&&Δυ()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ѓ>0&&Ѓ<Э){if(С>к){if(Э>150) к=С;else к=(float) Math.Ceiling(С);Ѓ=1;}else Ѓ++;}else{if(Ѓ>0){Μ="Ship stuck! Retrying";к=С;Ѓ=0;Ĕ(false,true,0,ζ*sensorRange);}Р(Βέ(щ,Math.Max(0,к-ζ)),Βΰ(false));if(К<=wpReachedDist/2){Ѓ=1;к=0;}
return;}Ĕ(false,true,ζ*sensorRange,0);Vector3 ΗЍ=щ+њ*С;bool ΕЉ=false;if(Vector3.Distance(ΗЍ,ί)>0.3f){Vector3 ΘΙ=щ+њ*(С+0.1f);Р(ΘΙ,4);ΕЉ=true;}else{float ǝ=Βΰ(true);Vector3 Εη=Βέ(щ,Math.Max(Ц+1,С+1));Ǥ(true,false,false,Εη,Εη-щ,ǝ,ǝ);}bool ΓΠ=false;if(у==я.ΒΙ||у==я.ΒΘ)
{if(!ΕЉ){float ΓЀ=0;foreach(IMyTerminalBlock ǰ in ο)
ΓЀ+=ΖБ(ǰ,"","",у==я.ΒΘ?new string[]{"STONE"}:null);if(ΓЀ>Γο||С<Ц||Γκ){ΒЎ=0;ΑΚ=С;Ĩ=(int)(Math.Max(Ĩ,ΑΚ)+ζ/2);}else{ΓΠ=С-ΑΚ>2&&ΒЎ>=20;ΒЎ++;}Γο=ΓЀ;}}else ΓΠ=С>=Ĩ;if(ȗ!=Й){Ν(Α.Ǹ);С=0;return;}if(ΓΠ){Й++;Ν(Α.Ǹ);С=0;return;}}if(Υ==Α.Ǹ||Υ==Α.Ȧ||Υ==Α.Ŭ)
{if(Ђ){щ=Ȍ(Ш,Щ,false);Х(Ε.α,Ε.β,Ε.υ,false);ђ(ο,ņ);ǥ(false);к=Vector3.Distance(ί,щ);Ĕ(false,true,0,ζ*sensorRange);}Р(щ,Βΰ(false));if(Vector3.Distance(ί,щ)>=к+5){ђ(ο,false);ǥ(true);Μ="Can´t return!";}if(К<wpReachedDist){if(Υ==Α.Ǹ&&ΑϏ) Ν(Α.ĵ);if(Υ==Α.Ǹ) Ν(Α.ȋ);
if(Υ==Α.Ŭ) Ν(Α.Ƹ);if(Υ==Α.Ȧ){if(Θ.γ) Ν(Α.ȥ);else{ψ();đ();Μ="Can´t return, no dock found";}}return;}}if(Υ==Α.Ѕ||Υ==Α.Ю){if(Υ==Α.Ю&&ή==Κ.Ϗ&&Η!=Ζ.Ϋ){if(!Ƴ()||ű(true)){Ν(Α.Ѕ);return;}}bool ΓЉ=false;bool ΑЏ=false;bool ΕΤ=false;float ŀ=0;bool ΖΠ=false;Ψ Ω=null;
if(Ђ){if(Υ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȏ(1)) ΒΕ();else{ћ=null;ȅ=0;Ɖ=!ΒΜ(1);}}Ψ ά=е();η=new Βφ();η.Њ=ά;η.Ƈ=followPathDock*ζ;η.ΔΣ=useDockDirectionDist*ζ;η.ŀ=10;η.Ţ.Add(ά.Δ);Vector3 Ŀ=new Vector3();if(Ƕ(ά,dockDist*ζ,true,out Ŀ))
η.Ţ.Add(Ŀ);else η.Ƈ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) η.Αά=Ε.Δ;if(ά==Ε) η.Αά=Θ.Δ;η.ΓΪ=dockDist*ζ*1.1f;}}else if(Υ==Α.Ю){η=new Βφ();η.Њ=Ε;η.Ƈ=followPathJob*ζ;η.ΔΣ=useJobDirectionDist*ζ;η.ŀ=10;η.Αά=Θ.Δ;η.ΓΪ=dockDist*ζ*1.1f;η.Ţ.Add(Ε.Δ);if(ή==Κ.з){if(!Θ.γ||Σ.Count==0){ψ();return;}
float ΘΟ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΠ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΟ<ΘΠ) η.Њ=Σ.Last();else η.Њ=Σ.First();}}ƚ=new Vector3();ΖΠ=!ΗΧ(ί);ђ(ο,false);ǥ(true);У=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αά)<=η.ΓΪ) continue;
double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǩ==-1||ό<Ǩ){У=i;Ǩ=ό;}}ǭ=ΗΜ(η.Њ.Δ,У);Ĝ=null;}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&!Ź()){Ψ ΓΑ=е();Ɖ=!ΒΜ(1);if(е()!=ΓΑ){Ν(Α.Ѕ);return;}}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ђ&&ȏ(1)&&ΒΕ()){Ν(Α.Ѕ);return;}bool ΕЇ=false;if(Υ==Α.Ѕ&&!Ђ&&У>=0&&У<Σ.Count)
{Vector3 ƅ=Σ[У].Δ+Δφ(У,ǭ);int ΓϏ=У+ǭ;Vector3 ŕ=ǭ!=0&&ΓϏ>=0&&ΓϏ<Σ.Count?Σ[ΓϏ].Δ:η.Њ.Δ;if(ΗΟ(ƅ,ŕ)){if(!ΑΥ){ΑΥ=true;Εμ=Ķ(ƅ)?ί:ƅ;}Р(Εμ,0);Х(η.Њ.α,η.Њ.β,η.Њ.υ,false);Μ="Waiting for dock clearance";return;}ΑΥ=false;ΕЇ=ȏ(1)&&(Ķ(ί)||Ķ(ŕ));}ΖЄ(Σ,ǭ,η.Ţ,η.Ƈ,Ђ,ref Ѓ);
for(int i=0;i<η.Ţ.Count;i++){float ό=Vector3.Distance(ί,η.Ţ[i]);if(ό<=η.Ƈ) ΓЉ=true;if(ό<=η.ΔΣ) ΑЏ=true;}if(ΕЇ) ΑЏ=true;if(ΑЏ) ŀ=η.ŀ;float ΚΚ=Ĝ!=null?Ĝ.Џ:Ȝ;float ΙΥ=(float) Math.Max(Ȝ*0.1f*ζ,wpReachedDist);if((К<ΙΥ)||Ђ){if(!Ђ) У+=ǭ;if(ǭ==0||У>Σ.Count-1||У<0)
ΓЉ=true;else{ΔΧ=ǭ>0?Σ.Count-1-У:У;Ω=Σ[У];Ĝ=Ω;if(У>=1&&У<Σ.Count-1)
ƚ=Ω.Δ-Σ[У-ǭ].Δ;else Ĝ=null;χ=Ω.Δ+Δφ(У,ǭ);ΕΤ=true;}}if(ΑЏ)
Х(η.Њ.α,η.Њ.β,η.Њ.υ,false);else if(ΖΠ)
ΒΔ(η.Њ.α,10,true);else if(ΕΤ&&Ω!=null){if(ǭ>0) Х(Ω.α,Ω.β,Ω.υ,90,false);else Х(Ω.α,-Ω.β,-Ω.υ,90,false);}Ǥ(true,false,true,χ,ƚ,Ĝ==null?0:Ĝ.Џ,ŀ);if(ΓЉ){ΔΧ=0;if(Υ==Α.Ѕ&&!ΖЈ()){Р(χ,0);return;}if(Υ==Α.Ѕ&&Ɖ){Р(χ,0);Μ="Waiting for a free dock";return;}if(Υ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.Ǣ);return;}
if(Υ==Α.Ю&&ΑϏ){Ν(Α.ĵ);return;}if(Υ==Α.Ю){Ν(Α.ȋ);return;}}}if(Υ==Α.Ǣ||Υ==Α.Ŷ){if(Η!=Ζ.Ϋ&&Ź()){if(!ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΑ=е();if(!ΒΜ(2)||е()!=ΓΑ){Ν(Α.Ѕ);return;}}Ψ ά=е();if(Ђ){if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.Ū);ψ();return;}
Р(χ,0);ΒΔ(ά.α,90,true);}if(К<followPathDock*ζ&&К!=-1){Р(χ,10);Х(ά.α,ά.β,ά.υ,false);}if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null){Ν(Α.Đ);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.Č);return;}}if(Υ==Α.Č||Υ==Α.ȡ){if(Ђ)
{if(Υ==Α.Č){Ψ ά=е();if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.Ū);ψ();return;}Ǥ(true,true,false,χ,0);Х(ά.α,ά.β,ά.υ,10,false);}if(Υ==Α.ȡ){Х(Ε.α,Ε.β,Ε.υ,0.5f,false);χ=Ε.Δ;Ǥ(true,true,false,χ,0);}}if(Αζ){ǉ(false,0,0,0,0);if(Υ==Α.Č) Ν(Α.Đ);if(Υ==Α.ȡ) ψ();return;}}if(Υ==Α.Đ)
{if(П(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.Ȩ);else Ν(Α.ǻ);return;}Ψ ά=е();if(Ђ){Э=0;ΘΡ=DateTime.Now;Ѓ=0;Х(ά.α,ά.β,ά.υ,false);}Vector3I Γα=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Εί=Me.CubeGrid.GetCubeBlock(Γα);float Εξ=dockingSpeed;float Θκ=dockingSpeed*5;
float ΕΠ=Math.Max(1.5f,Math.Min(5f,ζ*0.15f));if(!Ƕ(ά,0,true,out χ)||!Ƕ(ά,ΕΠ,true,out ƚ)||
Εί==null||!Εί.FatBlock.IsFunctional){ǹ(а.Ū);ψ();return;}if(Э==1||(Vector3.Distance(ί,χ)<=ΕΠ*1.1f&&!Ђ))
Э=1;else{Vector3 ΙΕ=Я(Φ,ƚ-ί);Vector3 Γΰ=Я(Φ,Φ.GetNaturalGravity());float ΘЁ=Ʊ(ΙΕ,Γΰ,null);Εξ=Math.Min(Θκ,ΘЁ);}Ǥ(true,false,false,χ,χ-ί,dockingSpeed,Εξ);if(Ђ) к=(float) К;IMyShipConnector ΓΛ=П(MyShipConnectorStatus.Connectable);if(ΓΛ!=null){Ǥ(false,false,false,χ,0);
if(Ѓ>0) Ѓ=0;Ѓ--;if(Ѓ<-5){ΓΛ.Connect();if(ΓΛ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.Ȩ);else Ν(Α.ǻ);ΒЄ();Ÿ(Ȅ,true);return;}}}else{float Ελ=(float) Math.Round(К,1);if(Ελ<к){Ѓ=-1;к=Ελ;}else Ѓ++;if(Ѓ>20){Ν(Α.Ŷ);return;}}}if(Υ==Α.ǻ||Υ==Α.Ȩ||
Υ==Α.ΑΕ||Υ==Α.Ʒ||Υ==Α.ď){bool ΒЌ=false,ΒЍ=false;if(Η==Ζ.Ϋ){if(е()==Θ) ΒЌ=true;else if(е()==Ε) ΒЍ=true;}if(Ђ){Ɵ=false;if(П(MyShipConnectorStatus.Connected)==null){Ν(Α.Ǔ);return;}ΒЄ();if(ΒЌ) ΑΑ(ϊ.Ǘ);if(ΒЍ) ΑΑ(ϋ.Ǘ);Ĭ=false;Ȑ=false;Ȕ=false;ƨ=false;}if(П(MyShipConnectorStatus.Connected)==null){ψ();ǹ(а.ΔϏ);return;}
if(ή!=Κ.Ϗ||ч==-1||ǧ==п.None) Ȑ=true;else if(Ǽ>=100f) Ȑ=true;else if(Ǽ<=99f) Ȑ=false;if(ή!=Κ.Ϗ||ш==-1||і.Count==0) Ȕ=true;else if(ȕ>=100f) Ȕ=true;else if(ȕ<=99) Ȕ=false;if(ή!=Κ.Ϗ||Ы==-1||Ć.Count==0) ƨ=true;else ƨ=ĭ>=Ы;ļ о=null;if(ΒЌ) о=ϊ;
if(ΒЍ) о=ϋ;if(о!=null&&(о.Ќ==ι.ǅ||о.Ќ==ι.Ǆ)) Ȑ=true;if(о!=null&&(о.Ќ==ι.Βν)) if(!Ĭ) Ȑ=false;if(о!=null&&(о.Ќ==ι.Βρ||о.Ќ==ι.Βο)) Ȕ=true;if(о!=null&&(о.Ќ==ι.Βπ)) if(!Ĭ) Ȕ=false;if(ă){ChargeMode Εέ=Ȑ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ќ==ι.Ǆ||о.Ќ==ι.ǅ))
Εέ=ChargeMode.Discharge;Βϋ(Εέ);Βύ(!Ȕ);}if(!Ĭ){if(Η==Ζ.Ϋ)
Ĭ=ή!=Κ.Ϗ||ΖЉ(Ђ,true)||Ʀ;else
Ĭ=ή!=Κ.Ϗ||ΔЀ();}else{if(!Ȑ) Ν(Α.ď);if(!Ȕ) Ν(Α.Ʒ);if(!ƨ) Ν(Α.ΑΕ);Ђ=false;}if(Ĭ&&Ȑ&&Ȕ&&ƨ){Βϋ(ChargeMode.Auto);Βύ(false);if(ή==Κ.Ϗ){if(Η==Ζ.Ϋ){if(е()==Θ) ΑΑ(ϊ.ǚ);else if(е()==Ε) ΑΑ(ϋ.ǚ);if(е()==Θ) Ĺ(Ε,Κ.Ĳ);else Ĺ(Θ,Κ.ı);}}Ν(Α.Ǔ);return;}}if(Υ==Α.Ǔ){if(ƫ){if(!Δζ()) return;
ƫ=false;Ν(Α.Ǔ);return;}if(Ђ){if(!Δζ()){ƫ=true;return;}IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Ю);return;}IMyShipConnector δ=Ȁ.OtherConnector;ΔА(Ȁ,false);Ÿ(Ȅ,false);Ψ ά=null;Ψ Γθ=Η==Ζ.Ϋ?Θ:е();if(Vector3.Distance(Ȁ.GetPosition(),Γθ.Δ)<5f&&Θ.γ) ά=Γθ;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.γ) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*ζ,true,out χ)){ǹ(а.Ū);ψ();return;}Р(χ,5);Х(ά.α,ά.β,ά.υ,false);}else Р(ί+δ.WorldMatrix.Forward*dockDist*ζ,5);if(ή==Κ.Ϗ) ǹ(а.ǈ);}if(К<wpReachedDist){ђ(т,true);Ν(Α.Ю);return;}}if(Υ==Α.ĵ)
{if(Ђ){ǥ(true);ђ(ο,false);χ=Ε.Δ;Р(χ,20);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist/2){Ν(Α.ȡ);return;}}if(Υ==Α.ȥ){if(Ђ){ǥ(true);ђ(ο,false);int Б=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǩ==-1||ό<Ǩ){Б=i;Ǩ=ό;}}
if(Б==-1){Ν(Α.Ѕ);return;}χ=Σ[Б].Δ;Р(χ,10);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Υ==Α.Ƹ){bool ф=false;if(Ђ){ǥ(true);if((М==Ѐ.ΒΤ||М==Ѐ.Ƶ)&&ƾ()&&
Ĵ(њ,Φ.GetNaturalGravity())<25&&Н>=2&&б>=2){Vector3 Ƌ=ί;if(Ш>0&&Щ<б-1) Ƌ=Ȍ(Ш-1,Щ+1,true);else if(Ш<Н-1&&Щ<б-1) Ƌ=Ȍ(Ш+1,Щ+1,true);else if(Ш<Н-1&&Щ>0) Ƌ=Ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ƌ=Ȍ(Ш-1,Щ-1,true);else ф=true;if(!ф) Р(Ƌ,10);
}else ф=true;}if(К<wpReachedDist/2) ф=true;if(ф){Ν(Α.ΑΔ);return;}}if(Υ==Α.ΒЇ||Υ==Α.ΑΔ){if(Ђ){Ǥ(true,true,false,ί,0);ђ(ο,false);ǥ(true);Ѓ=-1;Э=М==Ѐ.Βε||М==Ѐ.Ƽ?0:-1;}bool Ğ=!Ƴ();int ΑЇ=Ȋ("STONE","ORE",З.č);if(М==Ѐ.ΒΝ||М==Ѐ.Ƽ||М==Ѐ.Ƶ)
ΑЇ+=Ȋ("ICE","ORE",З.č);bool ΘΩ=ΑЇ>0;bool ΓΩ=false;if(Э>=0){float Ľ=(float) Math.Sin(Δκ(Э))*Ǡ/3f;float ΗЏ=(float) Math.Cos(Δκ(Э))*Ǳ/3f;Vector3 ΘΔ=Ȍ(Ш,Щ,true)+ΒЅ(њ,ș*-1,new Vector3(Ľ,ΗЏ,0));Р(ΘΔ,0.3f);if(К<Math.Min(Ǡ,Ǳ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ѓ==-1||ΑЇ<Ѓ){Ѓ=ΑЇ;к=0;}
else{к++;if(к>50) ΓΩ=true;}if(!ΘΩ||Ğ||ΓΩ){if(!Ğ){int ΙΚ=Ȋ("","ORE",З.č);if(ű(true)) Ğ=true;else if(100-(А(ΙΚ,ΖΡ)*100)<minEjection) Ğ=true;else ǹ(а.ǈ);}if(ΓΩ&&Ğ) Μ="Ejection failed";if(Υ==Α.ΑΔ){if(Ğ){if(Θ.γ) Ν(Α.ȥ);else{ψ();đ();Μ="Can´t return, no dock found";}}
else Ν(Α.ȋ);}else if(Ğ) Ν(Α.Ȧ);else Ν(Α.ķ);return;}}Ђ=false;}void ΖЄ(List<Ψ>ċ,int κ,List<Vector3>ƣ,float Ǯ,bool Ȇ,ref int Ǖ){if(Ȇ){for(int i=0;i<ċ.Count;i++) ċ[i].Џ=0;Ǖ=-1;return;}if(κ==0) return;int ў=κ*-1;if(Ǖ==-1) Ǖ=ў>0?1:ċ.Count-2;int ΕЋ=0;while(Ǖ>=1&&Ǖ<ċ.Count-1)
{if(ΕЋ>50) return;ΕЋ++;try{if((ў<0&&Ǖ>=1)||(ў>0&&Ǖ<=ċ.Count-2)){Ψ θ=ċ[Ǖ];bool ΖΞ=false;for(int j=0;j<ƣ.Count;j++)
if(Vector3.Distance(θ.Δ,ƣ[j])<=Ǯ){ΖΞ=true;break;}if(!ΖΞ){Ψ Αϋ=ċ[Ǖ-ў];Ψ ŕ=ċ[Ǖ+ў];Vector3 ΙΖ=θ.Δ-ŕ.Δ;Vector3 ΔΝ=Αϋ.Δ-θ.Δ;Vector3 Θϋ=θ.Δ+Vector3.Normalize(ΙΖ)*ΔΝ.Length();Vector3 ΘΞ=Αϋ.Δ-Θϋ;Vector3 Αμ=ē(κ>0?θ.β:θ.β*-1,θ.α*-1,ΘΞ);Vector3 Θι=ē(κ>0?θ.β:θ.β*-1,θ.α*-1,ΔΝ);
Vector3 Αξ=ē(κ>0?θ.β:θ.β*-1,θ.α*-1,θ.ν);θ.Џ=(float) Math.Sqrt(Math.Pow(Αϋ.Џ,2)+Math.Pow(Ʊ(-Θι,Αξ,θ),2));for(int j=0;j<ƣ.Count;j++)
if(Vector3.Distance(Αϋ.Δ,ƣ[j])<=Ǯ){Vector3 Αο=ē(κ>0?θ.β:θ.β*-1,θ.α*-1,ƣ[j]-θ.Δ);float ΘЏ=Ʊ(-Αο,Αξ,θ);θ.Џ=Math.Min(θ.Џ,ΘЏ)/2f;}if(Αμ.Length()==0) Αμ=new Vector3(0,0,1);Vector3 Θθ=ē(θ.β,θ.α*-1,θ.ν);float ΘБ=ŭ(-Αμ,Θθ,θ);float ΑΖ=А(ΘБ,Ī);float ΔΛ=(float) Math.Sqrt(А(Αμ.Length()*1.0f,0.5f*ΑΖ));
θ.Џ=Math.Min(θ.Џ,(ΔΝ.Length()/ΔΛ)*Ǧ);}}}catch{return;}Ǖ+=ў;}Ǖ=-1;}void ΒІ(bool Ȇ){if(Ȇ){Ƒ=0;return;}if(Η==Ζ.Ϋ) return;float ф=Й*Math.Max(1,Ц);if(ȗ==Й) ф+=Math.Min(Ц,С);float ΙΙ=Н*б*Math.Max(1,Ц);Ƒ=Math.Max(Ƒ,(float) Math.Min(ф/ΙΙ*100.0,100));}MyDetectedEntityType Δυ()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βΰ(bool ǫ){if(Η==Ζ.ǐ&&Δυ()==MyDetectedEntityType.None&&!Ű(ξ,true))
return fastSpeed;else return ǫ?Ũ:ŧ;}void Ηύ(){float ΖΘ=0,Εε=0,Ģ=0,ΓЁ=0;for(int i=0;i<ȩ.Count;i++){IMyBatteryBlock ƀ=ȩ[i];if(!Ƿ(ƀ,true)) continue;ΖΘ+=ƀ.MaxStoredPower;Εε+=ƀ.CurrentStoredPower;Ģ+=ƀ.CurrentInput;ΓЁ+=ƀ.CurrentOutput;}Ǽ=(float) Math.Round(А(Εε,ΖΘ)*100,1);
if(Ģ>=ΓЁ) ǧ=п.ď;else ǧ=п.Δλ;if(Ģ==0&&ΓЁ==0||Ǽ==100.0) ǧ=п.ж;if(ȩ.Count==0) ǧ=п.None;}void ΗЁ(){float Ετ=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζο=і[i];if(!Ƿ(Ζο,true)) continue;Ετ+=(float) Ζο.FilledRatio;}ȕ=А(Ετ,і.Count)*100f;}void ΗЉ(){ĭ=0;try{for(int i=0;i<Ć.Count;i++)
{IMyReactor ΓЊ=Ć[i];if(!Ƿ(ΓЊ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЊ.GetInventory(0).GetItems(Ъ);float ǔ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
ǔ+=(float) Г.Amount;}if(ǔ<ĭ||i==0){ĭ=ǔ;Γφ=ΓЊ.CustomName;}}}catch(Exception e){Ȗ=e;}}void Ηώ(){œ=0;Ņ=0;try{for(int i=0;i<ń.Count;i++){IMyTerminalBlock Τ=ń[i];if(!Ƿ(Τ,true)) continue;Ņ+=(float) Τ.GetInventory(0).CurrentVolume;œ+=(float) Τ.GetInventory(0).MaxVolume;
}Αλ=(float) Math.Min(Math.Round(А(Ņ,œ)*100,1),100.0);}catch(Exception e){Ȗ=e;}}void ΗЂ(){try{Ǚ.Clear();for(int i=0;i<ń.Count;i++){IMyTerminalBlock Τ=ń[i];if(!Ƿ(Τ,true)) continue;Δή(Τ,З.ů);}if(М!=Ѐ.Βμ){for(int i=0;i<ė.Count;i++){IMyTerminalBlock Τ=ė[i];if(!Ƿ(Τ,true)) continue;
Δή(Τ,З.ΗΣ);}}Ηυ(Ǚ);}catch(Exception e){Ȗ=e;}}void ΖЇ(){if(ΑΡ){if(Δτ().Count>Αδ){ΑΡ=false;if(л!=д.Ů){ψ();if(л==д.ΒΩ) đ();if(л==д.Ǉ)
if(Θ.γ) ƹ();else đ();}Μ="Damage detected";}}}bool Ƴ(){if(!Αε) return true;if(ή==Κ.Ϗ){if(ч>0&&ǧ!=п.None)
if(Ǽ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&Ć.Count>0)
if(ĭ<=Ы){Μ="Low fuel: "+Γφ;return false;}if(ш>0&&і.Count>0)
if(ȕ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δτ(){List<IMyTerminalBlock>ĝ=new List<IMyTerminalBlock>();for(int i=0;i<ż.Count;i++){IMyTerminalBlock Τ=ż[i];if(Ű(Τ,false)) ĝ.Add(Τ);}return ĝ;}bool Ű(IMyTerminalBlock Τ,bool ΔΕ){return(!Ƿ(Τ,ΔΕ)||!Τ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Τ,bool ΔΕ){if(Τ==null) return false;try{IMyCubeBlock Εϋ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΔΕ) return Εϋ==Τ;else return Εϋ.GetType()==Τ.GetType();}catch{return false;}}float ΖБ(IMyTerminalBlock Τ,String ю,String Ζζ,String[]Ǫ){float ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ō=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Ō.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(Ǫ!=null&&(Ǫ.Contains(Г.Type.TypeId.ToUpper())||Ǫ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Г.Type.TypeId.ToUpper()==ю)&&(Ζζ==""||Г.Type.SubtypeId.ToUpper()==Ζζ))
ȁ+=(float) Г.Amount;}}return ȁ;}ѐ ΗΗ(String φ,String ς,З Д,bool ΘΚ){φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ѐ Г=Ǚ[i];if(Г.φ.ToUpper()==φ&&Г.ς.ToUpper()==ς&&(Г.Ő==Д||Д==З.č))
return Г;}ѐ έ=null;if(ΘΚ){έ=new ѐ(φ,ς,0,Д);Ǚ.Add(έ);}return έ;}int Ȋ(String φ,String ς,З Д){return Ȋ(φ,ς,Д,null);}int Ȋ(String φ,String ς,З Д,String[]Ǫ){int ȁ=0;φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ѐ Г=Ǚ[i];if(Ǫ!=null&&Ǫ.Contains(Г.φ.ToUpper())) continue;
if((φ==""||Г.φ.ToUpper()==φ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.Ő==Д||Д==З.č))
ȁ+=Г.ǔ;}return ȁ;}void Δή(IMyTerminalBlock Τ,З Д){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΗ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).ǔ+=(int) Ъ[j].Amount;}}void Ηυ(List<ѐ>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){ѐ a=Т[j];ѐ b=Т[j+1];if(a.ǔ<b.ǔ) Т.Move(j,j+1);}}bool ű(bool ΙЍ){if(Ƞ&&Η!=Ζ.Ϋ)
if(О!=-1&&Ī>=О){Μ="Ship too heavy";return true;}if(Αλ>=Ǭ||Ɵ){Ɵ=false;Μ="Ship is full";return true;}return false;}bool ΔЀ(){String[]Ǫ=null;if(!ȟ) Ǫ=new string[]{"ICE"};if(Η==Ζ.ŵ) return Ȋ("","ORE",З.ů,Ǫ)==0;if(Η==Ζ.ǐ) return Ȋ("","COMPONENT",З.ů,Ǫ)==0;else return Ȋ("","",З.ů,Ǫ)==0;
}bool ΗΨ(){List<IMyCockpit>ΓΚ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΚ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΚ.Count;i++)
if(ΓΚ[i].IsUnderControl) return true;return false;}bool ΖЉ(bool Ȇ,bool ΑЃ){IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δη(ϊ,Ȇ,ΑЃ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δη(ϋ,Ȇ,ΑЃ);return false;}bool Δη(ļ ǿ,bool Ȇ,bool ΑЃ){if(Ȇ) ǿ.Ηε();ǿ.ΗІ();bool н=false;String ь="";switch(ǿ.Ќ){case ι.Βξ:{ь="Waiting for command";н=false;break;}case ι.ΔЋ:{ь="Waiting for cargo";н=ű(true);break;}case ι.ΔЊ:{ь="Unloading";н=ΔЀ();break;}case ι.Βσ:{н=true;break;}
case ι.Βν:{ь="Charging batteries";н=Ǽ>=100f;break;}case ι.ǅ:{ь="Discharging batteries";н=Ǽ<=25f;break;}case ι.Ǆ:{ь="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βπ:{ь="Filling up hydrogen";н=ȕ>=100f;break;}case ι.Βρ:{ь="Unloading hydrogen";н=ȕ<=25f;break;}
case ι.Βο:{ь="Unloading hydrogen";н=ȕ<=0f;break;}case ι.Βς:{bool ΕІ=ΗΨ();if(!ΕІ) ǿ.ΓІ=true;н=ǿ.ΓІ&&ΕІ;ь="Waiting for passengers";break;}}if(!н) ǿ.ΒΛ(true);if(н&&ǿ.Ββ()){н=ǿ.ΒΛ(false);ь="Undocking in: "+ΒΫ((int) Math.Max(0,ǿ.ǖ-ǿ.ȯ));}if(ΑЃ) Ζό=ь;return н;}void ΖЀ()
{if(!ō) return;if(ο.Count<=1) return;float Ζυ=0,Ζτ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(Ű(ǰ,true)) continue;Ζυ+=(float) ǰ.GetInventory(0).MaxVolume;Ζτ+=(float) ǰ.GetInventory(0).CurrentVolume;}float ΕΥ=(float) Math.Round(А(Ζτ,Ζυ),5);for(int ΖΥ=0;ΖΥ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΥ++)
{float Γζ=0,Γυ=0;float ΕЃ=0,ΖΖ=0;IMyTerminalBlock ȓ=null,Ȱ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(Ű(ǰ,true)) continue;float Γό=(float) ǰ.GetInventory(0).MaxVolume;float Αέ=А((float) ǰ.GetInventory(0).CurrentVolume,Γό);if(ȓ==null||Αέ>Γζ){ȓ=ǰ;Γζ=Αέ;ΕЃ=Γό;}
if(Ȱ==null||Αέ<Γυ){Ȱ=ǰ;Γυ=Αέ;ΖΖ=Γό;}}if(ȓ==null||Ȱ==null||ȓ==Ȱ) return;if(checkConveyorSystem&&!ȓ.GetInventory(0).IsConnectedTo(Ȱ.GetInventory(0))){if(ΒБ>20) Μ="Inventory balancing failed";else ΒБ++;return;}ΒБ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȓ.GetInventory(0).GetItems(Ъ);float ƪ=0;if(Ъ.Count==0) continue;MyInventoryItem Αή=Ъ[0];String ΕЊ=Αή.Type.TypeId+Αή.Type.SubtypeId;if(!Ζϋ.TryGetValue(ΕЊ,out ƪ)){if(Ηά(ȓ.GetInventory(0),0,Ȱ.GetInventory(0),out ƪ))
Ζϋ.Add(ΕЊ,ƪ);else return;}float ΙΗ=((Γζ-ΕΥ)*ΕЃ/ƪ);float ΙΘ=((ΕΥ-Γυ)*ΖΖ/ƪ);int ΔΠ=(int) Math.Min(ΙΘ,ΙΗ);if(ΔΠ<=0) return;if((float) Αή.Amount<ΔΠ)
ȓ.GetInventory(0).TransferItemTo(Ȱ.GetInventory(0),0,null,null,Αή.Amount);else
ȓ.GetInventory(0).TransferItemTo(Ȱ.GetInventory(0),0,null,null,ΔΠ);}}bool Ηά(IMyInventory Ơ,int Β,IMyInventory ΘΝ,out float Ζϊ){Ζϊ=0;float ΘΑ=(float) Ơ.CurrentVolume;List<MyInventoryItem>ΓΒ=new List<MyInventoryItem>();Ơ.GetItems(ΓΒ);float ΕΨ=0;for(int i=0;i<ΓΒ.Count;i++) ΕΨ+=(float) ΓΒ[i].Amount;
Ơ.TransferItemTo(ΘΝ,Β,null,null,1);float Ζω=ΘΑ-(float) Ơ.CurrentVolume;List<MyInventoryItem>ΒЉ=new List<MyInventoryItem>();Ơ.GetItems(ΒЉ);float ΕΜ=0;for(int i=0;i<ΒЉ.Count;i++) ΕΜ+=(float) ΒЉ[i].Amount;if(Ζω==0f||!Βζ(0.9999,ΕΨ-ΕΜ,1.0001))
return false;Ζϊ=Ζω;return true;}void ΗЀ(){Vector3 Ǝ=χ+ĉ;Vector3 ǯ=Ǝ-ί;if(ǯ.Length()==0) ǯ=new Vector3(0,0,-1);Vector3 Αο=Я(Φ,ǯ);Vector3 Ζπ=Vector3.Normalize(Αο);Vector3 ŏ=Я(Φ,Φ.GetNaturalGravity());float ΘЊ=ΓЄ>0?Math.Max(0,1-Ĵ(ǯ,Εφ)/5):0;float Џ=(float) Math.Min((Γω>0?Γω:1000f),Math.Max(Ʊ(-Αο,ŏ,null),ΓЄ*ΘЊ));
if(!Ζσ) Џ=0;if(Ζΰ) Џ=Math.Max(0,1-ΑΟ/ΑΗ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(ř) Џ=0;if(э>=0) Џ=Math.Min(Џ,э);if(Ζα) Џ*=(float) Math.Min(1,А(ǯ.Length(),wpReachedDist)/2);Vector3 ΖΓ=Я(Φ,Φ.GetShipVelocities().LinearVelocity);float ΗЎ=(float)(Math.Max(0,15-Ĵ(-Ζπ,-ΖΓ))/15)*0.85f+0.15f;
ΔΙ+=Math.Sign(ΗЎ-ΔΙ)/10f;Vector3 ȫ=Ζπ*Џ*ΔΙ-(ΖΓ);Vector3 ΒА=Βί(ȫ,null);if(ΑЉ&&К>0.1f){ȫ.X*=ΒϏ(ȫ.X,ref ΑЊ.X,1f,ΒА.X,20);ȫ.Y*=ΒϏ(ȫ.Y,ref ΑЊ.Y,1f,ΒА.Y,20);ȫ.Z*=ΒϏ(ȫ.Z,ref ΑЊ.Z,1f,ΒА.Z,20);}else ΑЊ=new Vector3(1,1,1);Ƥ=Ī*ȫ-ŏ*Ī;Δΰ(Ƥ,ΑЉ);К=Vector3.Distance(ί,Ǝ);
}float ΒϏ(float value,ref float Ф,float ў,float Ș,float Γϊ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ў;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γϊ||Ș==0) return έ;Ф=Math.Min(Γϊ,Math.Max(-Γϊ,Ф));έ=Math.Abs(Ș);return έ;
}void ΕΚ(){float ы=90,Ǐ=90,ѝ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǫ,ј,ƕ,Γς;if(Γσ){ǫ=Vector3.Normalize(χ-ί);ƕ=Я(Φ,ǫ);ј=Я(Φ,ΔΗ);ы=Ĵ(ƕ,new Vector3(0,-1,0))-90;Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);
ѝ=Ď(ƕ,new Vector3(-1,0,0),ƕ.Z);}else{ǫ=ΔΘ;ј=Я(Φ,ΔΗ);ƕ=Я(Φ,ΔΘ);Γς=Я(Φ,Ζρ);ы=Ď(ј,new Vector3(0,0,1),ј.Y);Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);ѝ=Ď(Γς,new Vector3(0,0,1),Γς.X);}if(ΕΞ&&ƾ()){Vector3 Γΰ=Φ.GetNaturalGravity();ј=Я(Φ,Γΰ);ы=Ď(ј,new Vector3(0,0,1),ј.Y);
Ǐ=Ď(ј,new Vector3(-1,0,0),ј.Y);}if(!Βζ(-45,Ǐ,45)){ы=0;ѝ=0;}if(!Βζ(-45,ѝ,45)) ы=0;ǉ(Γβ,1,(-ы)*ǝ,(-ѝ)*ǝ,(-Ǐ)*ǝ);ΑΟ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(Ǐ),Math.Abs(ѝ)));Αζ=ΑΟ<=ΑΗ;}void ΖЅ(IMyTerminalBlock Αό){if(Αό==null) return;ǡ=new float[3,2];џ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(Αό,в.WorldMatrix.Backward);float Ȯ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǡ[0,0]+=Ȯ;else ǡ[0,1]-=Ȯ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǡ[1,0]+=Ȯ;else ǡ[1,1]-=Ȯ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǡ[2,0]+=Ȯ;else ǡ[2,1]-=Ȯ;String ю=Δψ(в);float[,]Ǎ=null;if(џ.ContainsKey(ю)) Ǎ=џ[ю];else{Ǎ=new float[3,2];џ.Add(ю,Ǎ);}float Ș=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) Ǎ[0,0]+=Ș;else Ǎ[0,1]-=Ș;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) Ǎ[1,0]+=Ș;else Ǎ[1,1]-=Ș;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) Ǎ[2,0]+=Ș;else Ǎ[2,1]-=Ș;}}static String Δψ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βθ(Vector3 ѕ,float[,]ő){return new Vector3(
ѕ.X>=0?ő[0,0]:ő[0,1],
ѕ.Y>=0?ő[1,0]:ő[1,1],
ѕ.Z>=0?ő[2,0]:ő[2,1]);}bool Δχ(Ψ Ω,String ю,out float ΓΨ){ΓΨ=0;int Β=ȇ.IndexOf(ю);if(Β==-1||Ω.ǟ==null||Β>=Ω.ǟ.Length)
return false;ΓΨ=Ω.ǟ[Β];if(ΓΨ==-1) return false;return true;}Vector3 Βί(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<џ.Keys.Count;i++){String ю=џ.Keys.ElementAt(i);float ƌ=0;if(!Δχ(Ω,ю,out ƌ))
return Βθ(ѕ,ǡ);έ+=Βθ(ѕ,џ.Values.ElementAt(i))*ƌ;}return έ;}return Βθ(ѕ,ǡ);}float ŭ(Vector3 ѕ,Ψ Ω){return ŭ(ѕ,new Vector3(),Ω);}float ŭ(Vector3 ѕ,Vector3 ν,Ψ Ω){Vector3 ΘА=Βί(ѕ,Ω);Vector3 Ȯ=ΘА+ν*Ī;float ƛ=(Ȯ/ѕ).AbsMin();return(float)(ѕ*ƛ).Length();}float Ʊ(Vector3 Αν,Vector3 ŏ,Ψ Ω)
{if(Αν.Length()==0) return 0;float ΕΝ=1;if(ŏ.Length()>0) ΕΝ=Math.Min(1,Ĵ(-ŏ,Αν)/90)*0.4f+0.6f;float Ƥ=ŭ(Αν,ŏ,Ω);if(Ƥ==0) return 0.1f;float ΑΖ=А(Ƥ,Ī);float ΔΛ=(float) Math.Sqrt(А(Αν.Length(),ΑΖ*0.5f));return ΑΖ*ΔΛ*ΕΝ*Ǧ;}float ƻ(Vector3 ǫ,Vector3 ώ,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Αξ=ē(ǫ,ώ,Vector3.Normalize(ν));float ǘ=ŭ(-Αξ,Ω);return ǘ/ν.Length();}void Δγ(bool Θί){float ǎ=0;float ΓΫ=0.9f;if(Θί){О=-1;į=0;ΑЅ=null;if(ή!=Κ.з&&Ε.ν.Length()!=0){ǎ=ΓΫ*ƻ(Ε.β,Ε.α*-1,Ε.ν,null);if(ǎ<О||О==-1) О=ǎ;}if(Θ.γ&&Θ.ν.Length()!=0)
{ǎ=ΓΫ*ƻ(Θ.β,Θ.α*-1,Θ.ν,null);if(ǎ<О||О==-1) О=ǎ;}return;}if(į==-1) return;if(į>=0){int ΖΨ=0;while(į<Σ.Count){if(ΖΨ>100) return;ΖΨ++;Ψ Ω=Σ[į];if(Ω.ν.Length()!=0f){ǎ=ΓΫ*Math.Min(
ƻ(Ω.β,Ω.α*-1,Ω.ν,Ω),
ƻ(Ω.β*-1,Ω.α*-1,Ω.ν,Ω));if(ǎ<О||О==-1) О=ǎ;}else ΑЅ=Ω;į++;}į=-1;}bool Γγ=true;float ą=0;if(ΑЅ!=null){for(int i=0;i<џ.Count;i++){String ю=џ.Keys.ElementAt(i);float[,]Ǎ=џ.Values.ElementAt(i);float ƌ=0;if(!Δχ(ΑЅ,ю,out ƌ)){Γγ=false;break;}for(int a=0;a<Ǎ.GetLength(0);a++)
for(int b=0;b<Ǎ.GetLength(1);b++){float Ė=Math.Abs(Ǎ[a,b]*ƌ);if(Ė==0) continue;Γγ=true;if(ą==0||Ė<ą) ą=Ė;}}}if(!Γγ){for(int a=0;a<ǡ.GetLength(0);a++)
for(int b=0;b<ǡ.GetLength(1);b++){float Ė=Math.Abs(ǡ[a,b]);if(Ė==0) continue;if(ą==0||Ė<ą) ą=Ė;}}if(ą>0){float Θμ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǎ=А(ą,Θμ);if(ǎ>0)
if(ǎ<О||О==-1) О=ǎ;}}void Δΰ(Vector3 ǘ,bool Ј){if(!Ј){for(int i=0;i<Ȉ.Count;i++)
Ȉ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЏ=Βί(ǘ,null);float ΙΧ=Math.Min(1,Math.Abs(А(ǘ.X,ΒЏ.X)));float ΙΨ=Math.Min(1,Math.Abs(А(ǘ.Y,ΒЏ.Y)));float ΙΩ=Math.Min(1,Math.Abs(А(ǘ.Z,ΒЏ.Z)));for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];Vector3 κ=Ηη(Я(Φ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǘ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΧ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǘ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΨ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǘ.Z))
в.SetValueFloat("Override",в.MaxThrust*ΙΩ);else
в.SetValueFloat("Override",0.0f);}}void ǉ(bool Ј,float Θψ,float ы,float ѝ,float Ǐ){for(int i=0;i<Αΰ.Count;i++){IMyGyro І=Αΰ[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=Θψ;if(!Ј) continue;Vector3 В=Φ.WorldMatrix.Forward;Vector3 Ь=Φ.WorldMatrix.Right;Vector3 ώ=Φ.WorldMatrix.Up;
Vector3 Ň=І.WorldMatrix.Forward;Vector3 ŉ=І.WorldMatrix.Up;Vector3 ň=І.WorldMatrix.Left*-1;if(Ň==В) І.SetValueFloat("Roll",Ǐ);else if(Ň==(В*-1)) І.SetValueFloat("Roll",Ǐ*-1);else if(ŉ==(В*-1)) І.SetValueFloat("Yaw",Ǐ);else if(ŉ==В) І.SetValueFloat("Yaw",Ǐ*-1);else if(ň==В) І.SetValueFloat("Pitch",Ǐ);
else if(ň==(В*-1)) І.SetValueFloat("Pitch",Ǐ*-1);if(ň==(Ь*-1)) І.SetValueFloat("Pitch",ы);else if(ň==Ь) І.SetValueFloat("Pitch",ы*-1);else if(ŉ==Ь) І.SetValueFloat("Yaw",ы);else if(ŉ==(Ь*-1)) І.SetValueFloat("Yaw",ы*-1);else if(Ň==(Ь*-1)) І.SetValueFloat("Roll",ы);
else if(Ň==Ь) І.SetValueFloat("Roll",ы*-1);if(ŉ==(ώ*-1)) І.SetValueFloat("Yaw",ѝ);else if(ŉ==ώ) І.SetValueFloat("Yaw",ѝ*-1);else if(ň==ώ) І.SetValueFloat("Pitch",ѝ);else if(ň==(ώ*-1)) І.SetValueFloat("Pitch",ѝ*-1);else if(Ň==ώ) І.SetValueFloat("Roll",ѝ);else if(Ň==(ώ*-1)) І.SetValueFloat("Roll",ѝ*-1);
}}void ΕΔ(){this.Γβ=false;}void Х(Vector3 ȭ,Vector3 В,Vector3 Γρ,float Ȟ,bool Ʃ){ΒΔ(ȭ,Ȟ,Ʃ);ΑΗ=Ȟ;Γσ=false;this.ΔΘ=В;this.Ζρ=Γρ;}void Х(Vector3 ȭ,Vector3 В,Vector3 Γρ,bool Ʃ){Х(ȭ,В,Γρ,2f,Ʃ);}void ΒΔ(Vector3 ȭ,float Ȟ,bool Ʃ){ΑΗ=Ȟ;this.Γβ=true;this.ΕΞ=Ʃ;Γσ=true;Αζ=false;
this.ΔΗ=ȭ;}void ΒЄ(){Ǥ(false,false,false,χ,0);ΑЉ=false;}void Р(Vector3 Њ,float ǝ){Ǥ(true,false,false,Њ,ǝ);}void Ǥ(bool Ј,bool ΔΒ,bool ΔΤ,Vector3 Њ,float ǝ){Ǥ(Ј,ΔΒ,ΔΤ,Њ,Њ-ί,0.0f,ǝ);}void Ǥ(bool Ј,bool ΔΒ,bool ΔΤ,Vector3 Њ,Vector3 ΘЄ,float ΘЅ,float ǝ){ΑЉ=true;this.Ζσ=Ј;
χ=Њ;this.Γω=ǝ;this.ΓЄ=ΘЅ;this.Ζα=ΔΒ;this.Ζΰ=ΔΤ;this.Εφ=ΘЄ;К=Vector3.Distance(Њ,ί);}void Ηθ(){ω.GetBlocksOfType(ż,Ǒ);}void Ηι(){if(Αώ){ύ=2;return;}List<IMyRemoteControl>Ɯ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζέ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζθ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ɯ,Ǒ);ω.GetBlocksOfType(ї,Ǒ);ω.GetBlocksOfType(Ζέ,Ǒ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζθ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=ǣ(ї,pamTag,true);Ηπ();Ηρ(Ζθ);ŷ(ї,setLCDFontAndSize,1.4f,false);ŷ(Ĉ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζμ=ǣ(Ζέ,pamTag,true);if(Ζμ.Count>0) ξ=Ζμ[0];else ξ=null;if(Η==Ζ.ŵ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.ǐ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ω.GetBlocksOfType(ΔΜ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζλ=ǣ(Ɯ,pamTag,true);if(Ζλ.Count>0) Ɯ=Ζλ;if(Ɯ.Count>0) Φ=Ɯ[0];else{Φ=null;ύ=2;Μ="Remote is missing";return;}ζ=(float) Φ.CubeGrid.WorldVolume.Radius*2;ќ=null;if(Η!=Ζ.Ϋ){Δδ();if(ο.Count>0&&ќ!=null){if(ξ!=null&&(ќ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(ќ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ќ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ηκ(){ω.GetBlocksOfType(Ȅ,Ǒ);ω.GetBlocksOfType(т,Ǒ);ω.GetBlocksOfType(Ȉ,Ǒ);ω.GetBlocksOfType(Αΰ,Ǒ);ω.GetBlocksOfType(ȩ,Ǒ);ω.GetBlocksOfType(Ć,Ǒ);ω.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑЄ,Ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǣ(т,"ConnectorMedium",false);else
т=ǣ(т,"Connector",false);List<IMyShipConnector>Ζι=ǣ(т,pamTag,true);if(Ζι.Count>0) т=Ζι;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(Αΰ.Count==0){ύ=1;Μ="Gyros are missing";}if(Ȉ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζν=ǣ(ΑЄ,pamTag,true);
if(Ζν.Count>0) ΑЄ=Ζν;List<IMyLandingGear>Ζκ=ǣ(Ȅ,pamTag,true);if(Ζκ.Count>0) Ȅ=Ζκ;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζη=ǣ(ȩ,pamTag,true);if(Ζη.Count>0) ȩ=Ζη;List<IMyGasTank>Ζξ=ǣ(і,pamTag,true);if(Ζξ.Count>0) і=Ζξ;}void Ηλ(){ω.GetBlocksOfType(ė,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ń.Clear();for(int i=ė.Count-1;i>=0;i--){if(ΗΥ(ė[i])){ń.Add(ė[i]);ė.RemoveAt(i);}}}bool ΗΥ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βγ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δδ(){if(Φ==null) return;ќ=null;float Αφ=0,Αρ=0,Αχ=0,Ας=0,Αψ=0,Ασ=0;List<IMyTerminalBlock>ΔΖ=ǣ(ο,pamTag,true);bool Θσ=ΔΖ.Count==0;if(ΔΖ.Count>0) ќ=ΔΖ[0];else if(ο.Count>0) ќ=ο[0];int ȁ=0;float Ś=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ќ.WorldMatrix.Forward){if(Θσ){ύ=2;Μ="Mining direction is unclear!";return;}continue;}ȁ++;Ś=Math.Max(Ś,Ηϋ(ο[i]));Vector3 Љ=ƺ(Φ,ο[i].GetPosition());if(i==0){Αφ=Љ.X;Αρ=Љ.X;Αχ=Љ.Y;Ας=Љ.Y;Αψ=Љ.Z;Ασ=Љ.Z;}Αρ=Math.Max(Љ.X,Αρ);Αφ=Math.Min(Љ.X,Αφ);
Ας=Math.Max(Љ.Y,Ας);Αχ=Math.Min(Љ.Y,Αχ);Ασ=Math.Max(Љ.Z,Ασ);Αψ=Math.Min(Љ.Z,Αψ);}if(Ś==0) Ś=drillRadius;bool ȭ=ќ!=null&&ќ.WorldMatrix.Forward==Φ.WorldMatrix.Down;Ǡ=(Αρ-Αφ)*(1-İ)+Ś*2;Ǳ=(ȭ?Ασ-Αψ:Ας-Αχ)*(1-Ġ)+Ś*2;IMyCubeGrid g=Φ.CubeGrid;float ΔΩ=0,ΔΪ=0,ΔΫ=0,Δά=0;
for(int c=0;c<8;c++){Vector3 p=ƺ(Φ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȭ?p.Z:p.Y;if(c==0||p.X<ΔΩ) ΔΩ=p.X;if(c==0||p.X>ΔΪ) ΔΪ=p.X;if(c==0||y<ΔΫ) ΔΫ=y;if(c==0||y>Δά) Δά=y;}ΑЁ=ΔΪ-ΔΩ+g.GridSize;ΓЎ=Δά-ΔΫ+g.GridSize;}float Ηϋ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǣ<T>(List<T>Т,String Ǟ,bool Αβ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βγ(Т[i],Ǟ,Αβ)) έ.Add(Т[i]);return έ;}bool Βγ<T>(T Τ,String Ǟ,bool Αβ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αβ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αβ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ђ<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΔА((IMyTerminalBlock) Т[i],Ј);}void ΔА(IMyTerminalBlock Τ,bool Ј){if(Τ==null) return;String ΗЌ=Ј?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЌ);a.Apply(Τ);}void ǥ(bool Ј){ΔΓ=Ј;if(!ī) return;ђ(ΑЄ,Ј);}void Βϋ(ChargeMode Ɨ){for(int i=0;i<ȩ.Count;i++) ȩ[i].ChargeMode=Ɨ;}void Βύ(bool ΘЌ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΘЌ;}void Ÿ(List<IMyLandingGear>Γή,bool ΖΔ){for(int i=0;i<Γή.Count;i++){if(ΖΔ) Γή[i].Lock();if(!ΖΔ) Γή[i].Unlock();}}void ΔБ(bool Ј){if(Εζ==Ј) return;List<IMyShipController>ΑΜ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΜ,Ǒ);if(ΑΜ.Count==0) return;
for(int i=0;i<ΑΜ.Count;i++)
ΑΜ[i].DampenersOverride=Ј;Εζ=Ј;}IMyShipConnector П(MyShipConnectorStatus ь){for(int i=0;i<т.Count;i++){if(!Ƿ(т[i],true)) continue;if(т[i].Status==ь) return т[i];}return null;}void Ĕ(bool ΓА,bool ΑΤ,float ΘΦ,float ΗБ){if(ξ==null||ο.Count==0) return;Vector3 ΑΛ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ќ.WorldMatrix.Forward) continue;ȁ++;ΑΛ+=ο[i].GetPosition();}ΑΛ=ΑΛ/ȁ;Vector3 Ŗ=ƺ(ξ,ΑΛ);ξ.Enabled=true;ξ.ShowOnHUD=ΓА;ξ.LeftExtend=(ΑΤ?1:й)/2f*Ǡ-Ŗ.X;ξ.RightExtend=(ΑΤ?1:й)/2f*Ǡ+Ŗ.X;ξ.TopExtend=(ΑΤ?1:и)/2f*Ǳ+Ŗ.Y;
ξ.BottomExtend=(ΑΤ?1:и)/2f*Ǳ-Ŗ.Y;ξ.FrontExtend=(ΓА?Ц:ΘΦ)-Ŗ.Z;ξ.BackExtend=ΓА?0:ΗБ+ζ*0.75f+Ŗ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ŷ<T>(List<T>Т,bool ΘЇ,float ΘΤ,bool Θΰ){for(int i=0;i<Т.Count;i++){IMyTextSurface Ƣ=null;if(Т[i]is IMyTextSurface) Ƣ=(IMyTextSurface) Т[i];if(Ƣ!=null){Ƣ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЇ) continue;Ƣ.Font="Debug";if(Θΰ) continue;Ƣ.FontSize=ΘΤ;}}}bool ƾ(){Vector3D ε;return this.Φ.TryGetPlanetPosition(out ε);}int ΗΜ(Vector3 ΘЎ,int Ζβ){if(Ζβ==-1) return 0;double Б=-1;int Ȓ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘЎ);if(Б==-1||ό<Б){Ȓ=i;Б=ό;}
}return Math.Sign(Ȓ-Ζβ);}bool ΗΧ(Vector3 ε){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.γ&&Σ.Count>=1){Vector3 Ŀ=new Vector3();Ƕ(Θ,dockDist*ζ,false,out Ŀ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,Ŀ);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(Ŀ);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.γ&&Σ.Count>=1){Vector3 ΒЋ=new Vector3();Ƕ(Ε,dockDist*ζ,false,out ΒЋ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЋ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЋ);Ж.Add(Ε.Δ);}}}else{if(ή!=Κ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȓ=-1;double Ǩ=-1;for(int i=0;i<Ж.Count;i++){double ό=Vector3.Distance(Ж[i],ε);if(ό<Ǩ||Ǩ==-1){Ǩ=ό;Ȓ=i;}}if(Ж.Count==0) return false;double ΓΡ=Vector3.Distance(Ж[Ȓ],ε);double Θω=Vector3.Distance(Ж[Math.Max(0,Ȓ-1)],Ж[Ȓ])*1.5f;
double Θς=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȓ+1)],Ж[Ȓ])*1.5f;return ΓΡ<Θω||ΓΡ<Θς||ΓΡ<laneOffset*ζ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηη(Vector3 v,int ΓΞ){return new Vector3(Math.Round(v.X,ΓΞ),Math.Round(v.Y,ΓΞ),Math.Round(v.Z,ΓΞ));
}float Ĵ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΣ=(a*b).Sum;float ΕА=a.Length();float ΕБ=b.Length();if(ΕА==0||ΕБ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΣ/(ΕА*ΕБ)));if(float.IsNaN(έ)) return 0;return έ;}float Ď(Vector3 v,Vector3 Αό,float ΘЈ){float Ľ=Ĵ(v,Αό);
if(ΘЈ>0f) Ľ*=-1;if(Ľ>-90f) return Ľ-90f;else return 180f-(-Ľ-90f);}double Δκ(float ΘΛ){return(Math.PI/180)*ΘΛ;}bool Βζ(double Θλ,double value,double Απ){return(value>=Θλ&&value<=Απ);}Vector3 Ηό(IMyTerminalBlock Τ,Vector3 Љ){return Vector3D.Transform(Љ,Τ.WorldMatrix);
}Vector3 ƺ(IMyTerminalBlock Τ,Vector3 ΙΤ){return Я(Τ,ΙΤ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΨ){return Vector3D.TransformNormal(ΔΨ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 ē(Vector3 ǫ,Vector3 ώ,Vector3 ΔΨ){MatrixD Γψ=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.TransformNormal(ΔΨ,MatrixD.Transpose(Γψ));
}Vector3 ΒЅ(Vector3 ǫ,Vector3 ώ,Vector3 Љ){MatrixD Γψ=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.Transform(Љ,Γψ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Λ(Е,0)),float.Parse(Λ(Е,1)),float.Parse(Λ(Е,2)));
}String ΗЊ(Ψ Ω){String Ћ=":";String έ=τ(Ω.Δ)+Ћ+τ(Ω.β)+Ћ+
τ(Ω.α)+Ћ+τ(Ω.υ)+Ћ+τ(Ω.ν);for(int i=0;i<Ω.ǟ.Length;i++)
έ+=Ћ+Math.Round(Ω.ǟ[i],3);return έ;}Ψ Ηφ(String s){String[]Е=s.Split(':');Ψ Ω=new Ψ(
μ(Λ(Е,0)),μ(Λ(Е,1)),
μ(Λ(Е,2)),μ(Λ(Е,3)),
μ(Λ(Е,4)));int Β=5;List<float>Ες=new List<float>();while(Β<Е.Length){String ΑА=Λ(Е,Β);float f=0;if(!float.TryParse(ΑА,out f)) break;Ες.Add(f);Β++;}Ω.ǟ=Ες.ToArray();return Ω;}System.Text.StringBuilder Ş=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θρ){if(Θρ) Ş.Append('\n');Ş.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƶ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΒ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Λ(String[]ΗА,int Β){String ΑА=ΗА.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑА)) return "";
return ΑА;}void Save(){if(Αώ||Η==Ζ.р){Storage="";return;}Storage=ΒΚ();}String ΒΚ(){Ş.Clear();Ş.Append(DATAREV+";");Ρ(τ(Αύ),false);Ρ(τ(Θ.β));Ρ(τ(Θ.υ));Ρ(τ(Θ.α));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.γ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.β));Ρ(τ(Ε.α));Ρ(τ(Ε.υ));Ρ(τ(Ε.Ч));Ρ(Ε.γ);Ρ(τ(њ));Ρ(τ(ș));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(Ǭ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(Ǧ);Ρ(Ƞ);Ρ(ȟ);if(Η==Ζ.Ϋ){Ρ((int) ϊ.Ќ);Ρ(ϊ.ǖ);Ρ(ϊ.ȯ);Ρ(ƶ(ϊ.Ǘ));Ρ(ƶ(ϊ.ǚ));Ρ((int) ϋ.Ќ);Ρ(ϋ.ǖ);Ρ(ϋ.ȯ);Ρ(ƶ(ϋ.Ǘ));Ρ(ƶ(ϋ.ǚ));}else{Ρ((int) ȝ);Ρ((int) л);Ρ((int) М);Ρ((int) у);Ρ((int) ģ);Ρ(Ċ);Ρ(ī);Ρ(ō);Ρ(ņ);Ρ(й);Ρ(и);Ρ(Ц);Ρ(Ũ);Ρ(ŧ);Ρ(İ);Ρ(Ġ);
Ρ(Н);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(ȗ);Ρ(Й);Ρ(Ĩ);Ρ(Ħ);}Ρ(";");for(int i=0;i<ȇ.Count;i++)
Ρ((i>0?"|":"")+ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЊ(Σ[i]),i>0);Ρ(";",false);Ρ(Л+"|"+Ё,false);Ρ(";",false);Ρ(ΗΓ(),false);String Χ=Ş.ToString();Ş.Clear();return Χ;}с Ηζ(){return ǆ(Storage);}с ǆ(String Χ){if(Χ=="") return с.Ηή;String[]ĩ=Χ.Split(';');if(Λ(ĩ,0)!=DATAREV) return с.Βλ;int Β=0;try{String[]Ι=Λ(ĩ,1).Split('\n');
Αύ=μ(Λ(Ι,Β++));Θ.β=μ(Λ(Ι,Β++));Θ.υ=μ(Λ(Ι,Β++));Θ.α=μ(Λ(Ι,Β++));Θ.ν=μ(Λ(Ι,Β++));Θ.Δ=μ(Λ(Ι,Β++));Θ.Ч=μ(Λ(Ι,Β++));Θ.γ=bool.Parse(Λ(Ι,Β++));Ε.Δ=μ(Λ(Ι,Β++));Ε.ν=μ(Λ(Ι,Β++));Ε.β=μ(Λ(Ι,Β++));Ε.α=μ(Λ(Ι,Β++));Ε.υ=μ(Λ(Ι,Β++));Ε.Ч=μ(Λ(Ι,Β++));Ε.γ=bool.Parse(Λ(Ι,Β++));
њ=μ(Λ(Ι,Β++));ș=μ(Λ(Ι,Β++));Ι=Λ(ĩ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Λ(Ι,Β++));ή=(Κ) int.Parse(Λ(Ι,Β++));ц=(Κ) int.Parse(Λ(Ι,Β++));Ǭ=int.Parse(Λ(Ι,Β++));ч=int.Parse(Λ(Ι,Β++));Ы=int.Parse(Λ(Ι,Β++));ш=int.Parse(Λ(Ι,Β++));Ǧ=float.Parse(Λ(Ι,Β++));Ƞ=bool.Parse(Λ(Ι,Β++));
ȟ=bool.Parse(Λ(Ι,Β++));if(Η==Ζ.Ϋ){ϊ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϊ.ǖ=float.Parse(Λ(Ι,Β++));ϊ.ȯ=float.Parse(Λ(Ι,Β++));ϊ.Ǘ=ΑΒ(Λ(Ι,Β++));ϊ.ǚ=ΑΒ(Λ(Ι,Β++));ϋ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϋ.ǖ=float.Parse(Λ(Ι,Β++));ϋ.ȯ=float.Parse(Λ(Ι,Β++));ϋ.Ǘ=ΑΒ(Λ(Ι,Β++));ϋ.ǚ=ΑΒ(Λ(Ι,Β++));}else
{ȝ=(ǒ) int.Parse(Λ(Ι,Β++));л=(д) int.Parse(Λ(Ι,Β++));М=(Ѐ) int.Parse(Λ(Ι,Β++));у=(я) int.Parse(Λ(Ι,Β++));ģ=(ǒ) int.Parse(Λ(Ι,Β++));Ċ=bool.Parse(Λ(Ι,Β++));ī=bool.Parse(Λ(Ι,Β++));ō=bool.Parse(Λ(Ι,Β++));ņ=bool.Parse(Λ(Ι,Β++));й=int.Parse(Λ(Ι,Β++));и=int.Parse(Λ(Ι,Β++));Ц=int.Parse(Λ(Ι,Β++));
Ũ=float.Parse(Λ(Ι,Β++));ŧ=float.Parse(Λ(Ι,Β++));İ=float.Parse(Λ(Ι,Β++));Ġ=float.Parse(Λ(Ι,Β++));Н=int.Parse(Λ(Ι,Β++));б=int.Parse(Λ(Ι,Β++));Ш=int.Parse(Λ(Ι,Β++));Щ=int.Parse(Λ(Ι,Β++));ȗ=int.Parse(Λ(Ι,Β++));Й=int.Parse(Λ(Ι,Β++));Ĩ=int.Parse(Λ(Ι,Β++));Ħ=float.Parse(Λ(Ι,Β++));
}Ι=Λ(ĩ,3).Replace("\n","").Split('|');ȇ=Ι.ToList();Ι=Λ(ĩ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηφ(Λ(Ι,i)));Ι=Λ(ĩ,5).Split('|');Л=Λ(Ι,0);if(!int.TryParse(Λ(Ι,1),out Ё)) Ё=-1;if(Л.Length!=Н*б){Л="";Ё=-1;}ΗΒ(Λ(ĩ,6));}catch{return с.Δξ;}ş=ц;ψ();return с.ǋ;}const int Δβ=64000;IMyTerminalBlock Δο(){List<IMyTerminalBlock>ł=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ł,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ł.Count>0?ł[0]:null;}void ΗΖ(){IMyTerminalBlock Τ=Δο();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΚ();if(Χ.Length>Δβ){Μ="Export too large: "+Χ.Length+" chars, max "+Δβ;return;}Τ.CustomData=Χ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΠ(bool Γν){IMyTerminalBlock Τ=Δο();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΡ(Τ.CustomData,Γν);}void ΗΡ(String Χ,bool Γν){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΟ=Χ.IndexOf('\n');Χ=ΖΟ<0?"":Χ.Substring(ΖΟ+1);}Χ=Χ.Trim();if(Χ==""){Μ="Import: no PAM data in "+dataTag;return;}ψ();String ΕΧ=ΒΚ();с έ=ǆ(Χ);if(έ==с.ǋ&&Γν){Ψ Ą=new Ψ(Ε);
Vector3 Θξ=њ,Θν=ș;int w=й,h=и,d=Ц;ǒ ΔΔ=ȝ;я ΓΟ=у;float Θφ=İ,Θυ=Ġ;ǆ(ΕΧ);Ε=Ą;њ=Θξ;ș=Θν;й=w;и=h;Ц=d;ȝ=ΔΔ;у=ΓΟ;İ=Θφ;Ġ=Θυ;ή=Κ.ĸ;ź(true,true);ή=Κ.ĸ;ц=Κ.ĸ;Μ="Imported job, run CONT";return;}if(έ==с.ǋ){Ƙ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ǆ(ΕΧ);Μ=έ==с.Βλ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ηπ(){Ɔ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γλ=false;if(Χ=="INSTRUCTIONS"){Γλ=true;ΑΣ=true;}if(Χ=="DEBUG") Γλ=true;if(Γλ){Ɔ.Add(ї[i]);ї.RemoveAt(i);}
}ŷ(Ɔ,false,1,false);}void Ηρ(List<IMyTerminalBlock>ł){Ĉ.Clear();for(int i=0;i<ł.Count;i++){IMyTerminalBlock Τ=ł[i];try{String ΖΧ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΧ);int š=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΧ.Length,1),out š)) continue;
if(š==-1) continue;š--;IMyTextSurfaceProvider ΖΩ=(IMyTextSurfaceProvider) Τ;if(š<ΖΩ.SurfaceCount&&š>=0)
Ĉ.Add(ΖΩ.GetSurface(š));}catch{}}}String Ατ="";void ΔЌ(){String Γχ="";String ΓΧ="";Ατ=Δα(false);Γχ+=Ατ;ΓΧ+=Ατ;ΓΧ+=ΗΚ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γχ);for(int i=0;i<Ĉ.Count;i++) Ĉ[i].WriteText(Γχ);Echo(ΓΧ);for(int i=0;i<Ɔ.Count;i++){IMyTextPanel ΓЃ=Ɔ[i];
String Χ=ΓЃ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЃ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЃ.WriteText(ΗΛ());}}string ΗΛ(){String Γ="";try{float ΖΗ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ł*ΖΗ)+" / "+(int)(Œ*ΖΗ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ł*100f,1)+"% / "+Math.Round(Œ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ſ,2)+"ms / "+Math.Round(Γϋ,2)+"ms \n";}catch{}for(int i=0;i<Ț.Count;i++)
Γ+=""+Ț.Keys.ElementAt(i)+" = "+Math.Round(Ț.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ț.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΞ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ŷ(ї,setLCDFontAndSize,0.9f,false);ŷ(Ĉ,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+Ȗ.ToString());for(int i=0;i<Ĉ.Count;i++) Ĉ[i].WriteText(Γ+Ȗ.ToString());}void Ǻ(){if(!ΑΣ) return;try{ΓЇ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String φ){if(!ΑΣ) return;if(ΓЇ==0) return;try{float ΑЎ=(Runtime.CurrentInstructionCount-ΓЇ)/Runtime.MaxInstructionCount*100;if(!Ț.ContainsKey(φ))
Ț.Add(φ,new float[]{ΑЎ,ΑЎ});else{Ț[φ][0]=ΑЎ;Ț[φ][1]=Math.Max(ΑЎ,Ț[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЁ(String ȱ){if(!Ń) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=ũ(ΓЏ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=ΒΪ(ί.X)+""+Ћ;ρ+=ΒΪ(ί.Y)+Ћ;ρ+=ΒΪ(ί.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=Βά(ц)+(ц==Κ.Ϗ?" "+Math.Round(Ƒ,1)+"%":"")+Ћ;else ρ+=Βά(ц)+Ћ;ρ+=ũ(Μ)+Ћ;ρ+=љ+""+Ћ;ρ+=ũ(Ατ)+Ћ;ρ+=Ņ+Ћ;ρ+=œ+Ћ;for(int i=0;i<Ǚ.Count;i++){if(Ǚ[i].Ő!=З.ů) continue;ρ+=ũ(Ǚ[i].φ)+"/"+ũ(Ǚ[i].ς)+"/"+Ǚ[i].ǔ+Ћ;}Ην(ρ,ȱ);}String ũ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ην(String ρ,String ȱ){Ηξ(ρ,BROADCAST_SELF_ID,ȱ);}void Ηξ(String ρ,String Њ,String ȱ){try{if(!Ń) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δς()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΓΔ+BROADCAST_SEP+ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȗ=e;}
}String Δς(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЂ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool ū(ref String Χ,out string Εσ,Char Ћ){int Β=Χ.IndexOf(Ћ);Εσ="";if(Β<0) return false;Εσ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηί(ref String ρ,out String ƞ,out String ȱ)
{ƞ="";ȱ="";if(!Ń) return false;String Ǟ="";if(!ū(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЂ(Ǟ)) return false;if(!ū(ref ρ,out ƞ,BROADCAST_SEP)) return false;if(!ū(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δς()&&(Ǟ!="*"&&Η!=Ζ.р))) return false;if(!ū(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΔ)) return false;
if(!ū(ref ρ,out ȱ,BROADCAST_SEP)) return false;return true;}void ΗΤ(){bool Ƅ=true;if(Ευ){Ευ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Ə=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ə)
Δώ(Ə);String[]Ι=Me.CustomData.Split('\n');Ń=bool.Parse(Βυ(ΒΨ(Ι,"Enable_Broadcast",ref Ƅ)));bool ΔΦ=false;bool Εκ=true;if(Ń){if(Η!=Ζ.р)
ΓЏ=Βυ(ΒΨ(Ι,"Ship_Name",ref Ƅ)).Replace(BROADCAST_SELF_ID,"");ΓΔ=Βυ(ΒΨ(Ι,"Broadcast_Channel",ref Ƅ)).ToLower();Εκ=false;if(ȑ==null){ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȑ.SetMessageCallback("");}List<IMyRadioAntenna>ľ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ľ);bool ΒЊ=false;
for(int i=0;i<ľ.Count;i++)
if(ľ[i].EnableBroadcasting&&ľ[i].Enabled){ΒЊ=true;break;}if(ľ.Count==0) Μ="No Antenna found";else if(!ΒЊ) Μ="Antenna not ready";ΔΦ=ľ.Count==0||!ΒЊ;if(ΕΟ&&!ΔΦ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΟ=ΔΦ;if(Εκ){if(ȑ!=null) this.IGC.DisableBroadcastListener(ȑ);ȑ=null;}}catch{Ƅ=false;}if(!Ƅ){Μ="Reset custom data";Δώ(Ə);}}void Δώ(String Ə){String Χ=Ə+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Χ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βυ(String ĥ){int Β=ĥ.IndexOf("//");if(Β!=-1) ĥ=ĥ.Substring(0,Β);String[]Е=ĥ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΨ(String[]Ι,String Ĥ,ref bool ƙ){foreach(String ĥ in Ι)
if(ĥ.StartsWith(Ĥ)) return ĥ;ƙ=false;return "";}String Ιπ="";ĺ Ις=null;DateTime Ισ=DateTime.MinValue;List<ĺ>Ιρ=new List<ĺ>();const float ΙΫ=20f;String ΔЏ(int ΔΥ,String Γ,int Ζά,ref int ț){String[]Ι=Γ.Split('\n');if(Ζά>=ț+ΔΥ-1) ț++;ț=Math.Min(Ι.Count()-1-ΔΥ,ț);if(Ζά<ț+1) ț--;
ț=Math.Max(0,ț);String έ="";for(int i=0;i<ΔΥ;i++){int ΖΑ=i+ț;if(ΖΑ>=Ι.Count()) break;έ+=Ι[ΖΑ]+"\n";}return έ;}String ΒΫ(int Ɲ){if(Ɲ>=60*60) return Math.Round(Ɲ/(60f*60f),1)+" h";if(Ɲ>=60) return Math.Round(Ɲ/60f,1)+" min";return ""+Ɲ+" s";}string ΒΪ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>ŗ=new List<long>();bool ř=false;int ΓΙ=0;int ΓΗ=0;DateTime ΕΪ=DateTime.Now;String ΓΓ="";void Ημ(){ǽ.Clear();ŗ.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!ŗ.Contains(ǽ[i].CubeGrid.EntityId)) ŗ.Add(ǽ[i].CubeGrid.EntityId);}if(!ŗ.Contains(Me.CubeGrid.EntityId)) ŗ.Add(Me.CubeGrid.EntityId);}bool ΔЁ(Α s){return s==Α.ȋ||s==Α.Ю||s==Α.Ѕ||
s==Α.ȥ||s==Α.ĵ||s==Α.Ŭ||s==Α.Ƹ||
(s==Α.Ǣ&&ћ!=null);}void ƴ(){ř=false;ΓΙ=0;ΓΓ="";}void ΗЄ(){if(!obstacleBrake||ǽ.Count==0||!ΔЁ(Υ)){ƴ();return;}Vector3 Ǝ=χ+ĉ;Vector3 ǯ=Ǝ-ί;float Ǯ=ǯ.Length();if(Ǯ<1f){ƴ();return;}Vector3 κ=ǯ/Ǯ;float ΖΕ=Ȝ*obstacleLookAhead+ζ;Vector3 Θό=Ǝ+κ*(ζ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΓΗ=(ΓΗ+1)%ǽ.Count;IMyCameraBlock Ƃ=ǽ[ΓΗ];if(!Ƃ.IsFunctional||!Ƃ.IsWorking) continue;Vector3 Εά=Ƃ.GetPosition();Vector3 ΑΫ=Θό;if(Vector3.Distance(Εά,ΑΫ)>ΖΕ) ΑΫ=Εά+κ*ΖΕ;if(!Ƃ.CanScan(ΑΫ)) continue;MyDetectedEntityInfo Ɛ=Ƃ.Raycast(ΑΫ);bool Θτ=!Ɛ.IsEmpty()&&!ŗ.Contains(Ɛ.EntityId)&&!ΗΪ(Ɛ.EntityId)&&
Ɛ.Type!=MyDetectedEntityType.FloatingObject;if(Θτ){if(!ř) ΕΪ=DateTime.Now;ř=true;ΓΙ=0;ΓΓ=Ɛ.Type.ToString();}else if(ř&&++ΓΙ>=3) ƴ();break;}if(ř&&(DateTime.Now-ΕΪ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΓΓ;}const String ΕΙ="[PAMTFC]";const float Ηω=5f;const float ΒΧ=2f;class ȧ{public long λ;public Vector3 ε,Į;public Vector3 Ŋ;public float Ει;public String Ą="";public int Γη=-1;public bool ΓЈ;public bool Αα;public float Γέ,Γά;public long ά=0;public int ƈ=0;public DateTime ΑЋ;
}IMyBroadcastListener ΑЍ=null;long ъ=0;List<ȧ>м=new List<ȧ>();Vector3 ĉ=new Vector3();float э=-1;Vector3 ΔΞ=new Vector3();bool ΑЌ=false;DateTime ΔΟ=DateTime.Now;const float ΕΘ=15f;const float Ηψ=10f;int Ζχ=0;String Л="";int Ё=-1;bool ΓΜ=false,ΑΝ=false;Vector3 Εΰ=new Vector3();
String Ų(){if(ή==Κ.з||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Н+"|"+б+"|"+(int) ģ+"|"+Math.Round(Ǡ,1)+"|"+Math.Round(Ǳ,1);}void ΗЇ(){if(!trafficEnabled) return;if(ΑЍ==null) ΑЍ=IGC.RegisterBroadcastListener(ΕΙ);ъ=Me.CubeGrid.EntityId;int Ě=30;while(ΑЍ.HasPendingMessage&&Ě-->0){String Χ=ΑЍ.AcceptMessage().Data as String;if(Χ!=null) Ηβ(Χ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΑЋ).TotalSeconds>Ηω) м.RemoveAt(i);if(ΑЈ==null) ΑЈ=IGC.RegisterBroadcastListener(Ηχ);Ě=5;while(ΑЈ.HasPendingMessage&&Ě-->0){String Χ=ΑЈ.AcceptMessage().Data as String;if(Χ!=null) try{Ηγ(Χ);}catch{}}if(ΑΞ==null) ΑΞ=IGC.RegisterBroadcastListener(ΗΫ);
Ě=10;while(ΑΞ.HasPendingMessage&&Ě-->0){String Χ=ΑΞ.AcceptMessage().Data as String;if(Χ!=null) try{Ηΰ(Χ);}catch{}}if(++Ζχ<3) return;Ζχ=0;IGC.SendBroadcastMessage(ΕΙ,ъ+";"+τ(ί)+";"+
τ(Φ.GetShipVelocities().LinearVelocity)+";"+ζ+";"+Ų()+";"+
(ή==Κ.Ϗ?Ё:-1)+";"+Л+";"+ΖЋ()+","+Δμ()+";"+τ(ΔΞ)+
";"+(ΑΦ?1:0)+";"+(Ȳ&&ή==Κ.Ϗ?1:0)+","+ΑЁ+","+ΓЎ+","+(Δϋ()?1:0));}void Ηβ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ъ) return;ȧ Ї=null;for(int i=0;i<м.Count;i++) if(м[i].λ==λ) Ї=м[i];if(Ї==null){Ї=new ȧ();Ї.λ=λ;м.Add(Ї);}
Ї.ε=μ(f[1]);Ї.Į=μ(f[2]);Ї.Ει=float.Parse(f[3]);Ї.Ą=f[4];Ї.Γη=int.Parse(f[5]);Ї.ΑЋ=DateTime.Now;if(Ї.Ą!=""&&Ї.Ą==Ų()) ΔЉ(f[6]);Ї.ά=0;Ї.ƈ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.ƈ=int.Parse(d[1]);}Ї.Ŋ=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЈ=f.Length>=10&&f[9]=="1";Ї.Αα=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.Γέ=float.Parse(w[1]);Ї.Γά=float.Parse(w[2]);Ї.Αα=w[3]=="1";}}}catch{}}bool ΗΪ(long λ){for(int i=0;i<м.Count;i++) if(м[i].λ==λ) return true;return false;}Vector3 Δφ(int Β,int Θχ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΛ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θχ;Vector3 ώ=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.α;Vector3 Ь=Vector3.Cross(ΙΛ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ό=laneOffset*ζ;float ΘΥ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΥ/(ό*4));}Vector3 Ζφ=new Vector3();bool Δϋ(){return ή==Κ.Ϗ&&(Υ==Α.ȋ||Υ==Α.ķ||Υ==Α.Ǹ);}bool ΗΩ(Α s){return s==Α.Ǣ||s==Α.Č||s==Α.Đ||s==Α.Ŷ||
s==Α.Ǔ||s==Α.Ǹ||s==Α.ȡ;}void ΗЈ(){ĉ=new Vector3();э=-1;ΔΞ=new Vector3();bool Εψ=ΔЁ(Υ);if(!trafficEnabled||(!Εψ&&!ΗΩ(Υ))){ΑЌ=false;return;}Ζφ=Φ.GetShipVelocities().LinearVelocity;ΖЌ(Φ.GetNaturalGravity(),!Εψ);ΖϏ();}IMyBroadcastListener ΑЈ=null;float ţ=-1;DateTime Ζς=DateTime.MinValue;
void Ηγ(String Χ){if(!Χ.StartsWith("C;")) return;Ζς=DateTime.Now;ţ=-1;String Ɩ=ъ+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɩ)) ţ=float.Parse(e[i].Substring(Ɩ.Length));}void ΖϏ(){if(ţ<0||(DateTime.Now-Ζς).TotalSeconds>1.5) return;
э=э<0?ţ:Math.Min(э,ţ);if(ţ<0.5f) Μ="Traffic: giving way (controller)";}void ΖЌ(Vector3 ν,bool ΖΦ=false){ĉ=new Vector3();э=-1;Vector3 ǯ=χ-ί;float Ǯ=ǯ.Length();bool Γε=Ǯ>=1f;Vector3 κ=Γε?ǯ/Ǯ:new Vector3();ΔΞ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɗ;
Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-Ɣ:Vector3.Normalize(Ь);ȧ Ακ=null;bool Εϊ=Υ==Α.Ѕ&&!ȏ(1);for(int i=0;i<м.Count;i++){ȧ δ=м[i];if(δ.ƈ==3&&δ.Į.LengthSquared()<0.25f) continue;Vector3 ε=δ.ε+δ.Į*(float)(DateTime.Now-δ.ΑЋ).TotalSeconds;Vector3 ś=ε-ί;
float є=(ζ+δ.Ει)*0.5f;bool Ζί=δ.Αα&&Δϋ()&&δ.Ą==Ų();if(Ζί) є=Math.Min(Ǡ,Ǳ)*0.9f;float Ž=Γε?Vector3.Dot(ś,κ):0;if(Ž>0&&(ś-κ*Ž).Length()<=є*1.2f){float Αω=Vector3.Dot(δ.Į,κ);bool ΕЀ=δ.Ŋ.LengthSquared()>0.25f;bool ΕЁ=Αω<-1f||(ΕЀ&&Vector3.Dot(δ.Ŋ,κ)<-0.5f);
if(!ΖΦ){if(δ.ΓЈ){if(!Εϊ){ĉ=Ь*є*1.3f;continue;}float ΖΜ=Vector3.Distance(ί,χ),ΕЌ=Vector3.Distance(ε,χ);if(ΖΜ<ΕЌ||(ΖΜ==ΕЌ&&ъ<δ.λ)) continue;}if(ΕЁ){if(Ž<(Ȝ-Αω)*6f+є*4f) ĉ=Ь*є*1.3f;continue;}if(ΕЀ&&ъ<δ.λ){Vector3 ΕΦ=-ś;float ΖΣ=Vector3.Dot(ΕΦ,δ.Ŋ);
if(ΖΣ>0&&(ΕΦ-δ.Ŋ*ΖΣ).Length()<=є*1.2f){ĉ=Ь*є*1.3f;continue;}}}float Εω=Ž-є-(Ζί?1f:followGap*ζ);float Αι=Math.Max(0,Αω)+Math.Max(0,Εω)/ΒΧ;if(ΕЁ) Αι=Math.Max(0,Εω+Math.Min(0,Αω)*ΒΧ*2)/ΒΧ;if(э<0||Αι<э){э=Αι;Ακ=δ;}continue;}if(δ.Į.LengthSquared()<1f) continue;
if(Γε&&(ś-κ*Ž).Length()<=є*1.2f) continue;Vector3 ΓЍ=δ.Į-Ζφ;float ΖΪ=ΓЍ.LengthSquared();if(ΖΪ<0.25f) continue;float t=-Vector3.Dot(ś,ΓЍ)/ΖΪ;if(t<=0||t>5f||(ś+ΓЍ*t).Length()>=є*1.1f) continue;if(ъ>δ.λ||t<2f){э=0;Ακ=δ;}}bool Θά=э>=0&&э<0.5f;
if(!Θά||ΖΦ||(Ακ!=null&&Ακ.ΓЈ&&Εϊ)){ΑЌ=false;return;}if(!ΑЌ){ΑЌ=true;ΔΟ=DateTime.Now;}double ΕЂ=(DateTime.Now-ΔΟ).TotalSeconds;if(ΕЂ>ΕΘ+Ηψ) ΔΟ=DateTime.Now;else if(ΕЂ>ΕΘ){э=-1;ĉ=Ь*ζ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>ȃ=new List<int>();
String ΕЄ="";void ΒΦ(){String Ĥ=Н+"|"+б+"|"+(int) ģ;if(Ĥ==ΕЄ) return;ΕЄ=Ĥ;ΖЂ(Н,б,ģ,ȃ);}void ΖЂ(int w,int h,ǒ ΔΔ,List<int>ΓΘ){ΓΘ.Clear();if(ΔΔ==ǒ.ǌ){for(int k=0;k<w*h;k++){int Ŝ=k/w;ΓΘ.Add(Ŝ*w+(Ŝ%2==0?k-Ŝ*w:w-1-(k-Ŝ*w)));}return;}int ΕϏ=(int) Math.Ceiling(w/2f),Εώ=(int) Math.Ceiling(h/2f);
int ΘΨ=w/2,ΘΧ=h/2;int x=0,y=0,ΓΥ=0,ΓΦ=-1,Απ=Math.Max(w,h);for(int i=0;i<Απ*Απ;i++){if(-ΕϏ<x&&x<=ΘΨ&&-Εώ<y&&y<=ΘΧ) ΓΘ.Add((y-1+Εώ)*w+(x-1+ΕϏ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΥ;ΓΥ=-ΓΦ;ΓΦ=t;}x+=ΓΥ;y+=ΓΦ;
}}bool ΒΥ(){int n=Math.Max(0,Н*б);if(Л.Length==n) return false;Л=new String('0',n);Ё=-1;return true;}void ΔЉ(String δ){ΒΥ();if(δ.Length!=Л.Length) return;char[]h=Л.ToCharArray();for(int i=0;i<h.Length;i++) if(δ[i]=='1') h[i]='1';Л=new String(h);}bool ƽ(int Ǿ){return Ǿ>=0&&Ǿ<Л.Length&&Л[Ǿ]=='1';}
void Βό(int Ǿ,bool ф){if(Ǿ<0||Ǿ>=Л.Length) return;char[]h=Л.ToCharArray();h[Ǿ]=ф?'1':'0';Л=new String(h);}bool Βδ(int Ǿ,bool Θβ){if(!trafficEnabled) return false;String Ĥ=Ų();for(int i=0;i<м.Count;i++){ȧ t=м[i];if(t.Ą!=Ĥ||t.Γη<0||(Θβ&&t.λ>ъ)) continue;if(t.Γη==Ǿ) return true;
}return false;}int ΖЊ(int Β){int n=Н*б;if(n<1) return Β;ΒΦ();if(ΒΥ()) for(int k=0;k<Β&&k<n;k++) Βό(ȃ[k],true);if(Ё>=0&&(Β>=n||ȃ[Β]!=Ё)){Βό(Ё,true);Ё=-1;}if(ΔЃ()){if(ǩ>=0&&!ƽ(ǩ)){Ȳ=false;Ё=ǩ;return ȃ.IndexOf(ǩ);}if(ǩ==-2){Ё=-1;Ȳ=Л.IndexOf('0')>=0;return Ȳ?-2:n;}
if(ǩ==-1&&Ё>=0&&!ƽ(Ё)){Ȳ=false;return Β;}Ё=-1;if(!Ȳ){Ȳ=true;Εβ=DateTime.Now;}if((DateTime.Now-Εβ).TotalSeconds<5) return-1;}Ȳ=false;if(Ё>=0&&!ƽ(Ё)&&!Βδ(Ё,true)) return Β;Ё=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(ƽ(c)||Βδ(c,false)) continue;Ё=c;
return k;}return Л.IndexOf('0')>=0?-2:n;}const String ΗΫ="[PAMJOB]";const String Ηχ="[PAMTCC]";IMyBroadcastListener ΑΞ=null;DateTime Εα=DateTime.MinValue;DateTime Εβ=DateTime.Now;int ǩ=-1;bool Ȳ=false;bool ΔЃ(){return trafficEnabled&&ǩ!=-3&&(DateTime.Now-Εα).TotalSeconds<3;}
void Ηΰ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ų()) return;Εα=DateTime.Now;ΔЉ(f[2]);ǩ=-3;String Ɩ=ъ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɩ)) ǩ=int.Parse(e[i].Substring(Ɩ.Length));}Ļ Ηο(int Β)
{ΒΦ();if(Β<0||Β>=ȃ.Count) return Ļ.Δί;Ш=ȃ[Β]%Н;Щ=ȃ[Β]/Н;return Ļ.Δπ;}class ё{public long λ;public Vector3 ε,В,ώ;}List<ё>Ў=new List<ё>();bool Ȭ=false;Vector3 ΑΨ,ΑΧ,ΑΩ,ΑΪ;long х=0;long ȅ=0;Ψ ћ=null;bool Ɖ=false;Ψ ΒΡ(Vector3 ε,Vector3 В,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ε,В,ώ);
Ψ w=new Ψ(Θ);w.β=Vector3D.TransformNormal(ΑΨ,m);w.α=Vector3D.TransformNormal(ΑΧ,m);w.υ=Vector3D.TransformNormal(ΑΩ,m);w.Δ=ε+(Vector3) Vector3D.TransformNormal(ΑΪ,m);w.γ=true;return w;}int Δμ(){Α s=Υ;if(s==Α.ж) return П(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Đ||s==Α.ǻ||s==Α.ď||s==Α.ΑΕ||
s==Α.Ʒ||s==Α.Ǔ) return 3;if(s==Α.Ǣ||s==Α.Č||s==Α.Ŷ) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЋ(){if(Δμ()==0) return 0;if(ȏ(1)&&Š!=0) return Š;return ћ!=null?ȅ:х;}void ΔІ(){if(Η==Ζ.Ϋ||!Θ.γ) return;IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector δ=И.OtherConnector;Vector3I Θέ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==Θέ){MatrixD Ō=MatrixD.Transpose(δ.WorldMatrix);ΑΨ=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,Ō);ΑΧ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,Ō);ΑΩ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,Ō);
ΑΪ=Vector3D.TransformNormal(И.GetPosition()-δ.GetPosition(),Ō);Ȭ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) х=δ.EntityId;List<IMyShipConnector>ğ=new List<IMyShipConnector>();ω.GetBlocksOfType(ğ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ğ.Count>0){Ў.Clear();for(int i=0;i<ğ.Count;i++){ё p=new ё();p.λ=ğ[i].EntityId;p.ε=ğ[i].GetPosition();p.В=ğ[i].WorldMatrix.Forward;p.ώ=ğ[i].WorldMatrix.Up;Ў.Add(p);}}ћ=null;ȅ=0;if(δ.EntityId==х||!Ȭ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==δ.EntityId){ȅ=Ў[i].λ;ћ=ΒΡ(Ў[i].ε,Ў[i].В,Ў[i].ώ);}}bool Βη(long λ,int ΖΝ){if(λ==0) return false;for(int i=0;i<м.Count;i++){ȧ t=м[i];if(t.ά==λ&&(t.ƈ>ΖΝ||(t.ƈ==ΖΝ&&t.λ<ъ))) return true;}return false;}bool ΒΜ(int Αϊ){if(Η==Ζ.Ϋ||!trafficEnabled){ћ=null;ȅ=0;return true;}
long θ=ћ!=null?ȅ:х;if(!Βη(θ,Αϊ)) return true;if(!Βη(х,Αϊ)){ћ=null;ȅ=0;return true;}if(!Ȭ) return false;ё ę=null;float ΕΩ=0;for(int i=0;i<Ў.Count;i++){ё p=Ў[i];if(p.λ==х||Βη(p.λ,Αϊ)) continue;float d=Vector3.Distance(p.ε,Θ.Δ);if(ę==null||d<ΕΩ){ę=p;ΕΩ=d;}
}if(ę==null) return false;ȅ=ę.λ;ћ=ΒΡ(ę.ε,ę.В,ę.ώ);return true;}String ΗΓ(){String s=(Ȭ?"1":"0")+":"+τ(ΑΨ)+":"+τ(ΑΧ)+":"+
τ(ΑΩ)+":"+τ(ΑΪ)+":"+х;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].ε)+","+τ(Ў[i].В)+","+τ(Ў[i].ώ);return s;}void ΗΒ(String s){Ў.Clear();Ȭ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;Ȭ=f[0]=="1";ΑΨ=μ(f[1]);ΑΧ=μ(f[2]);ΑΩ=μ(f[3]);ΑΪ=μ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ё p=new ё();if(!long.TryParse(e[0],out p.λ)) continue;p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Ў.Add(p);}}const String ΒΞ="[PAMDCK]";const float ΙΪ=120f;class Δν{public long Ї;public int ƒ;public long Γθ;public bool Ιλ;public DateTime ΙЂ,Θγ;
}IMyBroadcastListener ΙϏ=null;List<Δν>ΙЀ=new List<Δν>();List<IMyShipConnector>ΙА=new List<IMyShipConnector>();List<IMySensorBlock>ΚΝ=new List<IMySensorBlock>();List<IMyCameraBlock>ΙЇ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΚΜ=new List<MyDetectedEntityInfo>();long Ιξ=0;
int Ιν=0;long Ιμ=0;DateTime Ιο=DateTime.Now;String Ιώ="";int ΙЁ=0;IMyCubeGrid ΙБ=null;IMyBroadcastListener ΑІ=null;int Ȃ=0;DateTime Εν=DateTime.Now;DateTime Ζγ=DateTime.MinValue;long Ζδ=0;int Ζε=0;long Š=0;bool ƫ=false;int ΓΤ=0;List<Vector3>ƭ=new List<Vector3>();List<float>Δέ=new List<float>();
bool ΑΦ=false;bool ΑΥ=false;Vector3 Εμ=new Vector3();bool Ź(){return Ȃ!=0&&(DateTime.Now-Ζγ).TotalSeconds<3;}bool ȏ(int ƒ){return Ź()&&Ζδ==ъ&&Ζε==ƒ;}bool ΕΒ(){return Ź()||(DateTime.Now-Εν).TotalSeconds<3;}void Βϊ(int ƒ){if(Ȃ==ƒ) return;Ȃ=ƒ;Εν=DateTime.Now;ΓΤ=3;
}void Βω(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΞ,"D;"+ъ);Ȃ=0;ƫ=false;}void ΗϏ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑІ==null) ΑІ=IGC.RegisterBroadcastListener(ΒΞ);int Ě=20;while(ΑІ.HasPendingMessage&&Ě-->0){String Χ=ΑІ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;
try{Ηα(Χ);}catch{}}if(Υ!=Α.Ѕ){ΑΦ=false;ΑΥ=false;}if(Ȃ==1&&П(MyShipConnectorStatus.Connected)!=null) Βω();if(Ȃ==2&&!ƫ&&Υ!=Α.Ǔ&&
Vector3.Distance(ί,е().Δ)>ζ*3) Βω();if(Ȃ!=0&&++ΓΤ>=3){ΓΤ=0;IGC.SendBroadcastMessage(ΒΞ,"R;"+ъ+";"+Ȃ+";"+х+";"+(Ȭ?1:0));}}void Ηα(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Εχ=false;String[]ΕΛ=f[5].Split(',');for(int i=0;i<ΕΛ.Length;i++) if(ΕΛ[i]==ъ.ToString()) Εχ=true;
if(!Εχ) return;Ζγ=DateTime.Now;Ζδ=long.Parse(f[2]);Ζε=int.Parse(f[3]);Š=long.Parse(f[4]);ƭ.Clear();Δέ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ƭ.Add(μ(e[0]));Δέ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εο=f[6].Split(':');List<ё>Т=new List<ё>();for(int i=0;i<Εο.Length;i++){String[]e=Εο[i].Split(',');if(e.Length<4) continue;ё p=new ё();p.λ=long.Parse(e[0]);p.ε=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖЈ(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;
Βϊ(1);if(!ΕΒ()) return true;if(!ȏ(1)){Μ="Waiting for dock clearance";return false;}ΒΕ();return true;}bool ΒΕ(){Ɖ=false;ё ħ=null;if(Š!=0&&Š!=х&&Ȭ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==Š) ħ=Ў[i];long ΒΑ=ħ==null?0:ħ.λ;if(ΒΑ==ȅ&&(ΒΑ==0)==(ћ==null)) return false;ȅ=ΒΑ;ћ=ħ==null?null:ΒΡ(ħ.ε,ħ.В,ħ.ώ);return true;}bool Ķ(Vector3 p){for(int i=0;i<ƭ.Count;i++) if(Vector3.Distance(p,ƭ[i])<Δέ[i]+ζ*0.75f) return true;
return false;}bool ΗΟ(Vector3 ƅ,Vector3 ŕ){ΑΦ=false;if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return false;if(Ȃ!=1&&Vector3.Distance(ί,Θ.Δ)>dockRequestDistance) return false;Βϊ(1);if(!Ź()||ȏ(1)||ƭ.Count==0) return false;if(!Ķ(ƅ)&&!Ķ(ŕ)&&!Ķ(ί)) return false;ΑΦ=true;return true;
}bool Δζ(){if(!trafficEnabled||Η==Ζ.Ϋ||х==0) return true;Βϊ(2);if(!ΕΒ()) return true;if(!ȏ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float Ιΰ=600f;class ΔЅ{public long λ;public int Ǿ=-1;public bool ΒΑ;public float Γέ,Γά;public DateTime ΙЊ;}class ΔЄ{public String Ĥ;
public int w,h;public float ΚΖ,ΚΔ;public String ф;public List<int>ΙЉ=new List<int>();public List<ΔЅ>ΙЌ=new List<ΔЅ>();}List<ΔЄ>ΙІ=new List<ΔЄ>();IMyBroadcastListener ΙЅ=null;int ΙЄ=0;const float Ικ=8f;class ΕΗ{public long λ;public Vector3 ε,Į,Ŋ;public float Ιό,Γέ,Γά,Αι=-1;public bool Αα;
public String Ą="";public int Αϊ;public DateTime ΑЋ;}class ΕΖ{public long a,b,ΚΛ;public DateTime Θγ;}List<ΕΗ>ΚΓ=new List<ΕΗ>();List<ΕΖ>ΚΑ=new List<ΕΖ>();int ΚΒ=0;void ΖА(string ѓ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}