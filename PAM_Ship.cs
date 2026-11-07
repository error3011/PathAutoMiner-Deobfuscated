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
public enum Ζ{ΑΔ,ŵ,Ǐ,с,Ϋ}public enum Α{ж,Ƕ,ĺ,Ǻ,ǹ,Ѕ,Ю,
Ȩ,ķ,Ǣ,đ,č,Ȥ,
Ŷ,ǽ,ǒ,Đ,ΑΖ,Ƹ,
ΒЈ,ΑΕ,Ē,ƹ,Ȫ}public enum Μ{з,Ļ,Є,ΒΤ,Ƴ,ǂ,ǃ,ĳ,Ĵ}public enum р{None,Đ,ж,Δν}public enum Ϊ{Main,ΔА,Ŵ,ų,ΒЄ,Ʊ,
ΒΔ,Δώ,ΔϏ,Δό,Ηυ,Ιμ,
Ικ,Ιλ,ΒϏ,ΕΓ,ǋ}public enum а{ǉ,Ŭ,Ηχ,ΔЁ,Ϋ}public enum Ǒ{ΑΑ,ΖЈ}public enum д{ǈ,ΒΪ,ΕΕ,ů}public enum Ϗ{Βν,Δκ,ΒΞ,ΒΥ,ƶ,Βζ,ƽ}public enum ѐ{ΒΠ,ΒΙ,ΒΚ}public enum З{Ű,ΗΥ,Ď}public enum ι{Βο,Βσ,Βτ,ΔЍ,ΔЌ,
Βξ,ǆ,ǅ,Βρ,
Βς,Βπ}public enum ΰ{ǳ,Ȏ,ǲ,ư,ΒΗ,ΒΘ}public enum Ľ{Δς,Δα,Ιΰ}public enum т{ǌ,Δπ,Ηα,Βμ}public enum Ǵ{None,Βψ,Ηη,Ηΰ}public enum σ{Ż,Ư,ǀ,ů,Βκ,ǁ,Βλ,Ηύ,Βυ}public enum Ιδ{ΑΔ,Ιζ,Ιε,Ιθ,Ια,Ιί,Ιη}class Ψ{public bool δ=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 υ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float ΚΠ=0;public float[]ǟ=null;public Ψ(){}public Ψ(Ψ γ){δ=γ.δ;Δ=γ.Δ;α=γ.α;β=γ.β;υ=γ.υ;ν=γ.ν;
Ч=γ.Ч;ǟ=γ.ǟ;}public Ψ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 υ,Vector3 ν){this.Δ=Δ;this.α=α;this.β=β;this.υ=υ;this.Џ=0;this.ν=ν;}public void ΖЅ(List<IMyThrust>ΑΙ,List<string>ȉ){ǟ=new float[ȉ.Count];for(int i=0;i<ǟ.Length;i++)
ǟ[i]=-1;for(int i=0;i<ΑΙ.Count;i++){string я=Δϊ(ΑΙ[i]);int Β=ȉ.IndexOf(я);if(Β!=-1)
ǟ[Β]=А(ΑΙ[i].MaxEffectiveThrust,ΑΙ[i].MaxThrust);}}}class Βχ{public Ψ Њ=null;public List<Vector3>Ť=new List<Vector3>();public float Ƈ=0;public float ΔΥ=0;public float ł=0;public float Γά=0;public Vector3 Αέ=new Vector3();}class ľ{public ι Ќ=ι.Βο;public float ǖ=0;public float ȱ=0;
public string Ǘ="";public string ǚ="";DateTime ΕБ;public bool ΓЈ=false;private bool ΓΟ=false;public bool ΒΜ(bool Ȉ){if(Ȉ){ȱ=0;ΓΟ=false;return false;}ΓΟ=true;return ȱ>ǖ;}public void Ηθ(){ΒΜ(true);ΓЈ=false;}public void ΗЉ(){if(ΓΟ)
if((DateTime.Now-ΕБ).TotalSeconds>1){ȱ++;ΕБ=DateTime.Now;}}public bool Βγ(){switch(Ќ){case ι.Βσ:return true;case ι.Βτ:return true;}return false;}}class ё{public String φ="";public String ς="";public int Ǔ=0;public З Œ=З.Ď;public ё(String φ,String ς,int Ǔ,З Œ){this.φ=φ;this.ς=ς;
this.Ǔ=Ǔ;this.Œ=Œ;}}class ΗΓ{public ΗΓ(Vector3 ζ,float ό){this.ζ=ζ;this.ό=ό;}public Vector3 ζ;public float ό;}class Ȑ{public DateTime Θθ;public DateTime Εΰ;public String λ="";public String φ="";public String ΚΣ="";public String Λ="";public String ΚΗ="";public String ΙБ="";public String ΓЇ="";
public Vector3 Δ=new Vector3();public List<ё>Ъ=new List<ё>();public Ζ Η=Ζ.ΑΔ;public Ǵ Ȭ;public float ŕ;public float Ň;public int љ;public int ΙЀ=0;public int ΚΞ=0;public bool Ιβ(){return(DateTime.Now-Θθ).TotalSeconds>10;}public bool ΗΨ(){if(Ȭ!=Ǵ.Βψ) return false;return(DateTime.Now-Εΰ).TotalSeconds>=4;
}public Ȑ(String λ){this.λ=λ;}public Ǵ Ιι(String ĝ,bool ΘЉ,bool Ǖ,bool ΘΖ){if(ĝ==""&&!Ǖ) return Ǵ.None;if(Ȭ==Ǵ.Βψ&&ΗΨ())
Ȭ=Ǵ.Ηΰ;if(ΘЉ){ΓЇ=ĝ;Ȭ=Ǵ.Βψ;Εΰ=DateTime.Now;return Ȭ;}else if(Ǖ){ΓЇ="";Ȭ=Ǵ.None;}else if(ΓЇ==ĝ){if(ΘΖ) Ȭ=Ǵ.Ηη;return Ȭ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 Αύ=new Vector3();Vector3 ή=new Vector3();Vector3 Αΰ=new Vector3();Vector3 ƕ=new Vector3();Vector3 Ɗ=new Vector3();
DateTime ΕА=new DateTime();bool ƍ=true;int ΔΜ=0;bool ą=false;bool Αθ=false;bool ƙ=true;bool ΖϏ=false;bool Ő=false;bool Αε=false;float ȟ=0;float Ĭ=0;int Ĉ=0;int ƨ=0;int ΑЀ=0;int ΓΖ=0;float Ŕ=0;float Ń=0;double Γύ=0;float ſ=0;List<int>ō=new List<int>();List<int>ş=new List<int>();
Ζ Η=Ζ.ΑΔ;int ύ=0;float Ǡ=0;float Ǳ=0;float ε=0;float ΑЁ=0,ΓА=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΞ=new List<IMyTimerBlock>();List<IMyShipConnector>у=new List<IMyShipConnector>();List<IMyThrust>Ȋ=new List<IMyThrust>();List<IMyGyro>Αα=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȇ=new List<IMyLandingGear>();List<IMyReactor>ć=new List<IMyReactor>();List<IMyConveyorSorter>ΑЅ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>ż=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ņ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ę=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȫ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>ĉ=new List<IMyTextSurface>();List<IMyTextPanel>Ɔ=new List<IMyTextPanel>();
IMyTerminalBlock ћ=null;Ψ Θ=new Ψ();Ψ Γλ=new Ψ();Ψ Γξ=new Ψ();bool ȋ=false;bool ŧ=false;bool Ũ=false;bool Αη=false;List<String>ȉ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>ƭ=new List<Ψ>();int ΓΥ=0;double ΖЀ=0;Ψ Ε=new Ψ();Vector3 ъ;Vector3 Ȝ;Μ ί=Μ.з;Ǒ ĥ=Ǒ.ΑΑ;int Н=0;
int б=0;double ƒ=0;bool ΑΣ=false;ľ ϊ=new ľ();ľ ϋ=new ľ();д м=д.ǈ;bool ċ=true;Ǒ Ƞ=Ǒ.ΑΑ;int й=3;int и=3;int Ц=30;ѐ ф=ѐ.ΒΠ;bool ȣ=true;bool ŏ=true;bool ň=true;bool ĭ=false;Ϗ М=Ϗ.Βν;bool Ȣ=true;float Ǭ=90;float ш=20;float Ы=5;float щ=20;float Ǧ=0.70f;float Ū=1.50f;
float ũ=2.50f;float Ĳ=0f;float Ģ=0f;Α Υ;Μ ч;Α Ƣ;Βχ η=null;int Ш=0;int Щ=0;int Й=0;int Ț=0;int Ī=30;int У=0;bool Ё=true;Vector3 л;double К=0;bool ΑϏ=false;int ΔΩ=0;int Ђ=0;int ΒЏ=0;int ǭ=0;int ΖΤ=0;Vector3 ƛ=new Vector3();float к=0;float Э=0;int ı=0;Ψ ΑІ=null;
float Γρ=0;float ΑΛ=0;float С=0;float Ĩ=0;bool Γμ=false;bool Į=false;bool Ʃ=false;bool Ȗ=false;bool Ȓ=false;DateTime ΘΥ=new DateTime();Ψ Ğ=null;Ψ Εε=null;float Ǿ=0;р ǧ;float ȗ=0;float į=0;String Γψ="";float Αλ=0;float Ň=0;float ŕ=0;List<ё>Ǚ=new List<ё>();float О=0;
bool Ơ=false;bool Ζυ=false;bool Ζβ=false;bool ΑЊ=false;bool Ζγ=false;float Γϋ=0;float ΓІ=0;Vector3 Εψ=new Vector3();Vector3 χ=new Vector3();Vector3 ΑЋ=new Vector3(1,1,1);float ΔΛ=1;Vector3 ƥ=new Vector3();bool Αζ=false;bool Γδ=false;bool Γυ=false;bool ΕΠ=false;float ΑΠ=0;
float ΑΘ=2;Vector3 Ζσ;Vector3 ΔΚ;Vector3 ΔΙ;float[,]ǡ=new float[3,2];Dictionary<String,float[,]>ў=new Dictionary<string,float[,]>();Dictionary<String,float>Ζύ=new Dictionary<String,float>();int ΓΑ=0;Dictionary<String,float[]>ȝ=new Dictionary<string,float[]>();float ΓЉ;bool ΑΤ=false;
int ΑΚ=0;int Ă=0;int Αγ=0;int ģ=0;String Λ="";int[]ΖΛ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΜ=false;int Αυ=0;int љ=0;Ϊ Ѝ=Ϊ.Main;int ΙΔ=0,ΙΕ=0,ΙΖ=0,ΙΗ=0;int ΘЅ=0,ΘІ=0;bool Ņ=false;bool ΕΡ=false;String ΓБ="";String ΓΕ="";IMyBroadcastListener ȓ=null;
bool Εχ=true;List<Ȑ>Ιχ=null;Ȑ ΚΓ=null;int Ιψ=0,Ιω=0;int Ιϊ=0,Ιϋ=0;int Ιό=0,Ιύ=0,Ιώ=0;String Θη="";Exception ș=null;bool Αώ=false;Μ š=Μ.з;bool Εθ=false;bool ΕЏ=true;bool ΓЎ=false;String Ζώ="";bool Ƨ=false;bool ΔΕ=true;int Αδ=0;int[]ΚΖ=null;Vector3 ΓΗ=new Vector3();
bool Εώ=false;bool ǐ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βδ(Me,controllerTag,true))
Η=Ζ.с;π(Ϊ.Main);if(Η!=Ζ.с){Λ="Welcome to [PAM]!";т ΓЍ=Ηι();if(Η==Ζ.ΑΔ){List<IMyShipDrill>Εσ=new List<IMyShipDrill>();List<IMyShipGrinder>ΕϏ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Εσ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(ΕϏ,q=>q.CubeGrid==Me.CubeGrid);if(Εσ.Count>0)
{Η=Ζ.ŵ;Λ="Miner mode enabled!";}else if(ΕϏ.Count>0){Η=Ζ.Ǐ;Λ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;ǻ(а.Ϋ);}}if(ΓЍ==т.ǌ) ƙ=false;if(ΓЍ==т.Δπ) Λ="Data restore failed!";if(ΓЍ==т.Βμ) Λ="New version, wipe data";}}void Main(string є,UpdateType ΙΟ){try{if(ș!=null){if(є.Trim().ToUpper()=="RETRY")
{ș=null;ƍ=true;Ĉ=0;if(Η!=Ζ.с) ψ();Λ="Restarted after error";}else{ΗΠ();ΔЎ();return;}}Αθ=(ΙΟ&UpdateType.Update10)!=0;if(Αθ) ΔΜ++;ą=ΔΜ>=10;if(ą) ΔΜ=0;if(Αθ){ΑΚ++;if(ΑΚ>4) ΑΚ=0;ΑЀ=(int) Math.Max(0,10-(DateTime.Now-ΕА).TotalSeconds);}if(є!="") Θη=є;if(Η!=Ζ.с)
Ηφ(є);else
ΗΑ(є);ΖΜ=false;try{int Εζ=Runtime.CurrentInstructionCount;float Ɯ=А(Εζ,Runtime.MaxInstructionCount);if(Ɯ>0.90) Λ="Max. instructions >90%";if(Ɯ>Ŕ) Ŕ=Ɯ;if(ΑΤ){ō.Add(Εζ);while(ō.Count>10) ō.RemoveAt(0);Ń=0;for(int i=0;i<ō.Count;i++) Ń+=ō[i];Ń=А(А(Ń,ō.Count),Runtime.MaxInstructionCount);
double Γς=Runtime.LastRunTimeMs;if(Αε&&Γς>Γύ) Γύ=Γς;ş.Add((int)(Γς*1000f));while(ş.Count>10) ş.RemoveAt(0);ſ=0;for(int i=0;i<ş.Count;i++) ſ+=ş[i];ſ=А(ſ,ş.Count)/1000f;}}catch{Ŕ=0;}}catch(Exception e){ș=e;}}void Ηφ(string є){bool ΑЂ=false;String Εέ="";if(ύ<=1&&!ΔЄ(є))
ΔЏ(є);int Θς=50;while(ȓ!=null&&ȓ.HasPendingMessage&&Θς-->0){MyIGCMessage ρ=ȓ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String Ɵ="";if(ύ<=1&&Ηβ(ref Χ,out Ɵ,out Εέ)&&Ɵ==BROADCAST_SELF_ID){ΔЏ(Χ);ΑЂ=true;break;}}bool ΕЊ=Αε&&Υ==Α.ж&&!Ő&&!ΑЂ&&Ĉ==0&&!Αη;
if(ą&&Υ!=Α.ж) ΑЂ=true;if((Αθ&&!ΕЊ)||(ą&&ΕЊ)){if(Ĉ==0&&(ΑЀ<=0||ƍ)){ΖϏ=ύ>0;ύ=0;Ĉ=1;Ǽ();Ημ();ΗΦ();ǵ("Scan 1");}else if(Ĉ==1){Ĉ=2;Ǽ();Ην();ǵ("Scan 2");}else if(Ĉ==2){Ĉ=0;Ǽ();Ηξ();Ηο();if(ύ<=1) ΔЈ();ǵ("Scan 3");ΕА=DateTime.Now;if(ύ<=1&&ƙ)
Αύ=ƻ(Φ,Φ.CenterOfMass);ƙ=false;if(ƍ){ƍ=false;bool ΙΤ=ί==Μ.Є;ψ();if(ΕЏ&&resumeAfterRestart&&ΙΤ&&ύ<=1) ΓЎ=true;}ΕЏ=false;if(ΖϏ&&ύ==0) Λ="Setup complete";}else{if(ί==Μ.Є&&Η!=Ζ.Ϋ){Ǽ();ΖЂ();ǵ("Inv balance");}Ǽ();switch(ƨ){case 0:ΗЁ();break;case 1:ΗЅ();break;case 2:ΗЀ();break;
case 3:ΗЌ();break;case 4:ΗЄ();break;case 5:ΖЉ();break;case 6:ΖЇ(Φ);break;}ǵ("Update: "+ƨ);ƨ++;if(ƨ>6){ƨ=0;Αε=true;if(š!=Μ.з){switch(š){case Μ.ĳ:Ȧ();break;case Μ.Ĵ:Ȧ();break;case Μ.Є:Ȧ();break;case Μ.ǂ:ƺ();break;case Μ.ǃ:ē();break;}š=Μ.з;}}}if(!ƍ){if(!Ǹ(Φ,true)){Φ=null;
ƙ=true;ύ=2;}if(ύ>=2&&Υ!=Α.ж)
ψ();if(ύ<=1){Ĭ=Φ.CalculateShipMass().PhysicalMass;ȟ=(float) Φ.GetShipSpeed();ή=ΗϏ(Φ,Αύ);Αΰ=Φ.WorldMatrix.Forward;ƕ=Φ.WorldMatrix.Left;Ɗ=Φ.WorldMatrix.Down;ΗЈ();ΗЊ();ΗЂ();if(ΓЎ){ΓЎ=false;Ȧ();if(Υ!=Α.ж) Λ="Job resumed after restart";}if(Υ!=Α.ж){Ő=false;ΕΒ(false);Δε(false);String ΘЎ=Βί(Υ)+" "+(int) Υ;
Ǽ();ΗІ();ΒЇ(false);ǵ(ΘЎ);ΗЋ();ΗЇ();Ǽ();ΗЃ();ǵ("Thruster");Ǽ();ΕΜ();ǵ("Gyroscope");}else{if(Ő){if(ƿ()){Х(Ɗ,Αΰ,ƕ,0.25f,true);ΕΜ();Λ="Aligning to planet: "+Math.Round(ΑΠ-0.25f,2)+"°";if(Αζ) Ββ(true,true);}else Ββ(true,true);}}Ƨ=false;}}}Ǽ();ΔЎ();ǵ("Print");if(ΑЂ||ΓΖ<=0){Ǽ();ΖЃ(Εέ);
ǵ("Broadcast");ΓΖ=4;}else if(ą) ΓΖ--;}void ΔЏ(string є){if(є=="") return;var Е=є.ToUpper().Split(' ');Е.DefaultIfEmpty("");var ĝ=Е.ElementAtOrDefault(0);var Ě=Е.ElementAtOrDefault(1);var ž=Е.ElementAtOrDefault(2);var ΕΥ=Е.ElementAtOrDefault(3);String ă="Invalid argument: "+є;bool ΔΤ=false;
switch(ĝ){case "UP":this.ΔЊ(false);break;case "DOWN":this.ΔЉ(false);break;case "UPLOOP":this.ΔЊ(true);break;case "DOWNLOOP":this.ΔЉ(true);break;case "APPLY":this.Δγ(true);break;case "MRES":љ=0;break;case "STOP":this.ψ();break;case "PATHHOME":{this.ψ();this.ΒЃ();}break;case "PATH":{this.ψ();this.ΒЃ();Θ.δ=true;}break;
case "START":{this.ψ();ΒЂ();}break;case "ALIGN":{Ββ(!Ő,false);}break;case "CONT":{this.ψ();this.Ȧ();}break;case "JOBPOS":{this.ψ();this.ē();}break;case "HOMEPOS":{this.ψ();this.ƺ();}break;case "FULL":{Ơ=true;}break;case "RESET":{Αώ=true;ύ=2;}break;case "EXPORT":{ΗΘ();}break;
case "IMPORT":{ΗΣ(Ě=="JOB");}break;default:ΔΤ=true;break;}if(Η!=Ζ.Ϋ){switch(ĝ){case "SHUTTLE":{ΗΖ();}break;case "CFGS":{if(!ΖА(Ě,ž,ΕΥ)) Λ=ă;}break;case "CFGB":{if(!ΖЏ(Ě,ž)) Λ=ă;}break;case "CFGL":{if(!Ĕ(ref Ǭ,true,σ.ǀ,Ě,"")||!ΖБ(ž))
Λ=ă;}break;case "CFGE":{if(!Ĕ(ref Ы,true,σ.ǁ,Ě,"IG")||
!Ĕ(ref ш,true,σ.Βκ,ž,"IG")||
!Ĕ(ref щ,true,σ.Βλ,ΕΥ,"IG"))
Λ=ă;}break;case "CFGA":{if(!Ĕ(ref Ǧ,false,σ.Ư,Ě,"")) Λ=ă;}break;case "CFGW":{if(!Ĕ(ref Ū,false,σ.Ż,Ě,"")||
!Ĕ(ref ũ,false,σ.Ż,ž,""))
Λ=ă;}break;case "NEXT":{ΒΓ(false);}break;case "PREV":{ΒΓ(true);}break;default:if(ΔΤ) Λ=ă;break;}}else{switch(ĝ){case "UNDOCK":{Ƨ=true;}break;default:if(ΔΤ) Λ=ă;break;}}}String ΗΜ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΖ(){ψ();Η=Ζ.Ϋ;ǻ(а.Ϋ);Ε.δ=false;Θ.δ=false;ξ=null;ο.Clear();ί=Μ.з;}void Ββ(bool Ј,bool х){if(!Ј) Λ="Aligning canceled";if(х) Λ="Aligning done";if(х||!Ј){Ő=false;ΕΖ();Ǌ(false,0,0,0,0);return;}if(ƿ()) Ő=true;}bool ΖА(String ΙΦ,String Θή,String ΘΟ)
{bool ΙΡ=ί==Μ.Є;int w,h,d;if(int.TryParse(ΙΦ,out w)&&int.TryParse(Θή,out h)&&int.TryParse(ΘΟ,out d)){this.ψ();й=w;и=h;Ц=d;ė(false);ź(false,false);if(ΙΡ) Ȧ();return true;}return false;}bool ΖБ(String ΕΤ){if(ΕΤ=="ON"){ȣ=true;return true;}if(ΕΤ=="OFF"){ȣ=false;return true;}
return false;}bool ΖЏ(String Ες,String ΑΡ){bool ƚ=true;if(Ες=="HOME") ċ=true;else if(Ες=="STOP") ċ=false;else ƚ=false;if(ΑΡ=="HOME") м=д.ǈ;else if(ΑΡ=="STOP") м=д.ΕΕ;else if(ΑΡ=="JOB") м=д.ΒΪ;else if(ΑΡ=="IG") м=д.ů;else ƚ=false;return ƚ;}void π(Ϊ ΓЄ){ΖΛ[(int) Ѝ]=љ;
љ=ΖΛ[(int) ΓЄ];if(ΓЄ==Ϊ.Ηυ) љ=0;Ѝ=ΓЄ;if(Η!=Ζ.с)
Ė(Ѝ==Ϊ.Ŵ,false,0,0);ΖΜ=true;}void ΔЊ(bool Γφ){if(љ>0) љ--;else if(Γφ) љ=Αυ-1;}void ΔЉ(bool Γφ){if(љ<Αυ-1) љ++;else if(Γφ) љ=0;}String ǻ(а ρ){switch(ρ){case а.ǉ:Λ="Job is running";break;case а.Ŭ:Λ="Connector not ready!";break;case а.Ηχ:Λ="Ship modified, path outdated!";break;
case а.ΔЁ:Λ="Interrupted by player!";break;case а.Ϋ:Λ="Shuttle mode enabled!";break;}return "";}String ΗΟ(Ǒ ζ){switch(ζ){case Ǒ.ΑΑ:return "Top-Left";case Ǒ.ΖЈ:return "Center";default:return "";}}String ΗΚ(ѐ Ƙ){switch(Ƙ){case ѐ.ΒΙ:return "Auto"+(Η==Ζ.ŵ?" (Ore)":"");case ѐ.ΒΚ:return "Auto (+Stone)";
case ѐ.ΒΠ:return "Default";default:return "";}}String Βέ(Μ Ф){switch(Ф){case Μ.з:return "No job";case Μ.Ļ:return "Job paused";case Μ.Є:return "Job active";case Μ.ĳ:return "Job active";case Μ.Ĵ:return "Job active";case Μ.ΒΤ:return "Job done";case Μ.Ƴ:return "Job changed";case Μ.ǂ:return "Move home";
case Μ.ǃ:return "Move to job";}return "";}String Δυ(д ΘΕ){switch(ΘΕ){case д.ǈ:return "Return home";case д.ΒΪ:return "Fly to job pos";case д.ΕΕ:return "Stop";case д.ů:return "Ignore";}return "";}String ΗΛ(Ϗ Ƙ){switch(Ƙ){case Ϗ.Βν:return "Off";case Ϗ.ΒΥ:return "Drop pos (Stone) ";case Ϗ.ƶ:return "Drop pos (Sto.+Ice)";
case Ϗ.Δκ:return "Cur. pos (Stone)";case Ϗ.ΒΞ:return "Cur. pos (Sto.+Ice)";case Ϗ.Βζ:return "In motion (Stone)";case Ϗ.ƽ:return "In motion (Sto.+Ice)";}return "";}String Δσ(р Ф){switch(Ф){case р.None:return "No batteries";case р.Đ:return "Charging";case р.Δν:return "Discharging";}return "";}String Βί(Α Ф)
{String ΙЌ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.Ƕ:return "Flying to XY position";case Α.ĺ:return Η==Ζ.Ǐ?"Grinding":"Mining";case Α.Ǻ:return "Returning";case Α.Ē:return "Flying to drop pos";case Α.ǹ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.Ȩ:return "Flying to path";case Α.ķ:return "Flying to job position";case Α.Ǣ:return "Approaching dock";case Α.đ:return "Docking";case Α.č:return "Aligning";case Α.Ȥ:return "Aligning";case Α.Ŷ:return "Retry docking";case Α.ǽ:return "Unloading";case Α.Ȫ:return Ζώ;
case Α.ǒ:return "Undocking";case Α.Đ:return "Charging batteries";case Α.ΑΖ:return "Waiting for uranium";case Α.Ƹ:return "Filling up hydrogen";case Α.ΒЈ:return "Waiting for ejection";case Α.ΑΕ:return "Waiting for ejection";case Α.ƹ:return "Flying to drop pos";}return "";}String Δϋ(ι Ќ){switch(Ќ)
{case ι.Βο:return "On \"Undock\" command";case ι.Βσ:return "On player entered cockpit";case ι.ΔЍ:return "On ship is full";case ι.ΔЌ:return "On ship is empty";case ι.Βτ:return "On time delay";case ι.ǆ:return "On batteries empty(<25%)";case ι.ǅ:return "On batteries empty(=0%)";case ι.Βξ:return "On batteries full";
case ι.Βς:return "On hydrogen empty(<25%)";case ι.Βπ:return "On hydrogen empty(=0%)";case ι.Βρ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Ζή,int ā,bool Ξ,String Ɣ){Αυ+=1;if(Ζή==ā) Ɣ=">"+Ɣ+(ΑΚ>=2?" .":"");else Ɣ=" "+Ɣ;Γ+=Ɣ+"\n";return Ζή==ā&&Ξ;}String Δγ(bool Ξ)
{int Β=0;int Ο=љ;Αυ=0;String Ζα="———————————————\n";String Ŧ="--------------------------------------------\n";String Γ="";Γ+=Βέ(ί)+" | "+(Θ.δ?"Ready to dock":"No dock")+"\n";Γ+=Ζα;double Ǯ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ϊ.Main){bool Ą=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЃ();
if(Η==Ζ.ŵ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.Ŵ);if(Η==Ζ.Ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.Ŵ);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.ΒϏ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ȧ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƺ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) ē();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ą) π(Ϊ.ǋ);else π(Ϊ.Ʊ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.Δώ);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δό);}else if(Ѝ==Ϊ.Ŵ){double ΙΥ=Math.Round(й*Ǡ,1);double Θέ=Math.Round(и*Ǳ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) ΒЂ();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){ź(false,false);π(Ϊ.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΥ+"m)")){ĵ(ref й,5,20,1);ė(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){ĵ(ref й,-5,20,-1);ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+Θέ+"m)")){ĵ(ref и,5,20,1);ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){ĵ(ref и,-5,20,-1);ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(ф==ѐ.ΒΠ?"Depth":"Min")+": "+Ц+"m)")){ĵ(ref Ц,5,50,2);ė(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){ĵ(ref Ц,-5,50,-2);ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+ΗΚ(ф))){ф=ȧ(ф);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΗΟ(Ƞ))){Ƞ=ȧ(Ƞ);}if(Η==Ζ.Ǐ&&ф==ѐ.ΒΚ) ф=ȧ(ф);Γ+=ΔБ(8,Ѓ,Ο,ref ΘЅ);}else if(Ѝ==Ϊ.ΒϏ){float[]Εκ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΓ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δϋ(ϊ.Ќ))) ϊ.Ќ=ȧ(ϊ.Ќ);if(!ϊ.Βγ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βά((int) ϊ.ǖ))) ϊ.ǖ=Δλ(ϊ.ǖ,Εκ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δϋ(ϋ.Ќ))) ϋ.Ќ=ȧ(ϋ.Ќ);
if(!ϋ.Βγ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βά((int) ϋ.ǖ))) ϋ.ǖ=Δλ(ϋ.ǖ,Εκ);}else if(Ѝ==Ϊ.ΕΓ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЂ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.ΒϏ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǘ!=""?ϊ.Ǘ:"-"))) ϊ.Ǘ=Ǆ(ref ΙΔ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǚ!=""?ϊ.ǚ:"-"))) ϊ.ǚ=Ǆ(ref ΙΖ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǘ!=""?ϋ.Ǘ:"-"))) ϋ.Ǘ=Ǆ(ref ΙΕ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǚ!=""?ϋ.ǚ:"-"))) ϋ.ǚ=Ǆ(ref ΙΗ);
}else if(Ѝ==Ϊ.ų){String ΖΔ=Ǭ+" %";if(ą) Ă++;if(Ă>1){Ă=0;Αγ++;if(Αγ>1) Αγ=0;}bool[]ΔΒ=new bool[]{ć.Count==0,ǧ==р.None,і.Count==0};int Ζϊ=0;while(true){Ζϊ++;ģ++;if(ģ>ΔΒ.Length-1) ģ=0;if(Ζϊ>=ΔΒ.Length) break;if(!ΔΒ[ģ]) break;}bool Ą=Η==Ζ.Ϋ;if(!Ą&&ȣ&&О!=-1&&Αγ==0)
ΖΔ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){ψ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ą) π(Ϊ.Ʊ);else π(Ϊ.ǋ);if(!Ą){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΓ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƨ=true;Γ+=Ŧ;if(!Ą) Γ+="Progress: "+Math.Round(ƒ,1)+" %\n";Γ+="State: "+Βί(Υ)+" "+Ǯ+"m \n";Γ+="Load: "+Αλ+" % Max: "+ΖΔ+" \n";if(ģ==0) Γ+="Uranium: "+(ć.Count==0?"No reactors":Math.Round(į,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ģ==1) Γ+="Battery: "+(ǧ==р.None?Δσ(ǧ):Ǿ+"% "+(ш==-1||Ą?"":" Min: "+ш+" %"))+"\n";if(ģ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȗ,1)+"% "+(щ==-1||Ą?"":" Min: "+щ+" %"))+"\n";}else if(Ѝ==Ϊ.Ʊ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.ų);else π(Ϊ.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ǀ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(ȣ?"On":"Off"))) ȣ=!ȣ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+ΗΛ(М))) М=ȧ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(ĭ?"On":"Off"))){ĭ=!ĭ;if(ĭ) ǥ(ΔΕ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(Ȣ?"On":"Off"))) Ȣ=!Ȣ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǁ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-5,σ.Βκ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Min "+щ+"%"))) г(ref щ,-10,σ.Βλ,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(ċ?"Return home":"Stop"))) ċ=!ċ;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δυ(м))) м=ȧ(м);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΔ);Γ+=ΔБ(8,Ѓ,Ο,ref ΘІ);}else if(Ѝ==Ϊ.ΒΔ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.ų);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǐ?" Grinder":" Drill")+" inv. balancing: "+(ŏ?"On":"Off"))) ŏ=!ŏ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǐ?" grinders":" drills")+": "+(ň?"Fwd + Bwd":"Fwd"))) ň=!ň;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ū+"m/s")) г(ref Ū,0.5f,σ.Ż,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ũ+"m/s")) г(ref ũ,0.5f,σ.Ż,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ư,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ĳ*100f+"%")) Δη(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+Ģ*100f+"%")) Δη(false,0.05f);
}else if(Ѝ==Ϊ.ǋ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.ų);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) г(ref Ǭ,Ǭ<=80?-10:-5,σ.ǀ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȣ?"On":"Off"))) Ȣ=!Ȣ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǁ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Charge up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Fill up"))) щ=(щ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δυ(м))) м=ȧ(м);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǧ*100f)+"%"+(Ǧ>0.80f?" (risky)":""))) г(ref Ǧ,0.1f,σ.Ư,false);
}else if(Ѝ==Ϊ.ΔА){double Θι=0;if(Σ.Count>0) Θι=Vector3.Distance(Σ.Last().Δ,ή);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΗ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȋ?"Use old home":(Θ.δ?"Was set! ":"none ")))) ȋ=!ȋ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȋ?"Use old connector":(Θ.δ?"Was set! ":"none ")))) ȋ=!ȋ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(ŧ?"Use old connector":(Ε.δ?"Was set! ":"none ")))) ŧ=!ŧ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ũ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ũ=!Ũ;Γ+=Ŧ;Γ+="Wp spacing: "+Math.Round(ΖЀ)+"m\n";}else if(Ѝ==Ϊ.ΒЄ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){ψ();π(Ϊ.Main);}
Γ+=Ŧ;Γ+="State: "+Βί(Υ)+" \n";Γ+="Speed: "+Math.Round(ȟ,1)+"m/s\n";Γ+="Target dist: "+Ǯ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΩ+"\n";}else if(Ѝ==Ϊ.Δώ){List<IMyTerminalBlock>ğ=Δφ();if(ą) Ă++;if(Ă>=ğ.Count) Ă=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔϏ);
Γ+=Ŧ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αλ,1)+"% "+Math.Round(Ň,1)+" / "+Math.Round(ŕ,1)+"\n";Γ+="Uranium: "+(ć.Count==0?"No reactors":Math.Round(į,1)+"Kg "+Γψ)+"\n";Γ+="Battery: "+(ǧ==р.None?"":Ǿ+"% ")+Δσ(ǧ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ȗ,1)+"% ")+"\n";
Γ+="Damage: "+(ğ.Count==0?"None":""+(Ă+1)+"/"+ğ.Count+" "+ğ[Ă].CustomName)+"\n";}else if(Ѝ==Ϊ.ΔϏ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ŧ;Γ+="Next scan: "+ΑЀ+"s\n";Γ+="Drill area: "+Math.Round(Ǡ,1)+"x"+Math.Round(Ǳ,1)+"m Ship: "+Math.Round(ΑЁ,1)+"x"+Math.Round(ΓА,1)+"m ("+Math.Round(ε,1)+"m)\n";
Γ+="Broadcast: "+(Ņ?"Online - "+ΓБ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ŕ*100f,1)+"% \n";}else if(Ѝ==Ϊ.Δό){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ŧ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+ΑЀ+"s\n";if(Αώ) Γ="Recompile script now";int Θκ=Γ.Split('\n').Length;for(int i=Θκ;i<=10;i++) Γ+="\n";Γ+=Ζα;Γ+="Last: "+Λ+"\n";
return Γ;}void Δη(bool Θδ,float ѝ){ψ();ź(true,false);if(Θδ) г(ref Ĳ,ѝ,σ.Βυ,false);else г(ref Ģ,ѝ,σ.Βυ,false);Δζ();Ė(true,true,0,0);}float Δλ(float θ,float[]ΒΑ){float έ=ΒΑ[0];for(int i=ΒΑ.Length-1;i>=0;i--)
if(θ<ΒΑ[i]) έ=ΒΑ[i];return έ;}String Ǆ(ref int ā){String έ="";if(ā>=ΔΞ.Count) ā=-1;if(ā>=0) έ=ΔΞ[ā].CustomName;ā++;return έ;}void ΑΒ(string φ){if(ί!=Μ.Є) return;if(φ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(φ);if(Τ==null||!(Τ is IMyTimerBlock)){Λ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void ĵ(ref int value,int Ɓ,int ȡ,int ΔΓ){if(Ɓ==0) return;if(value<ȡ&&ΔΓ>0||value<=ȡ&&ΔΓ<0){value+=ΔΓ;return;}int ΗЎ=Math.Abs(Ɓ);int ΒЉ=0;int ΓϏ=1;while(true){ΒЉ+=ΓϏ*ΗЎ*10;if(Ɓ<0&&value-ȡ<=ΒЉ) break;if(Ɓ>0&&value-ȡ<ΒЉ) break;
ΓϏ++;}value+=ΓϏ*Ɓ;}void ė(bool ΚΡ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ė(Ѝ==Ϊ.Ŵ,false,0,0);}bool Ĕ(ref float value,bool ΘЂ,σ ς,String Ĥ,String Θα){if(Ĥ=="") return false;float Ś=-1;bool ΕЇ=false;if(Ĥ.ToUpper()==Θα) ΕЇ=true;else if(!float.TryParse(Ĥ,out Ś)) return false;
else Ś=Math.Max(0,Ś);if(ΘЂ) Ś=(float) Math.Round(Ś);г(ref value,Ś,ς,ΕЇ,false);return true;}void г(ref float value,float ѝ,σ ς,bool Ʀ){г(ref value,value+ѝ,ς,Ʀ,true);}void г(ref float value,float Ŗ,σ ς,bool Ʀ,bool ΙΩ){float ǜ=0,Ǜ=0;if(ς==σ.Ż){ǜ=0.5f;Ǜ=10f;}if(ς==σ.Ư){ǜ=0.1f;Ǜ=1f;}
if(ς==σ.ů){ǜ=50f;Ǜ=100f;}if(ς==σ.Βκ){ǜ=5f;Ǜ=30f;}if(ς==σ.ǁ){ǜ=1f;Ǜ=25f;}if(ς==σ.ǀ){ǜ=10f;Ǜ=95f;}if(ς==σ.Βλ){ǜ=10f;Ǜ=90f;}if(ς==σ.Ηύ){ǜ=10f;Ǜ=1800;}if(ς==σ.Βυ){ǜ=0.0f;Ǜ=0.75f;}if(Ŗ==-1&&Ʀ){value=-1;return;}if(value==-1) Ʀ=false;
bool ΖΦ=Ŗ<ǜ||Ŗ>Ǜ;if(ΖΦ&&ΙΩ){if(Ŗ<value) value=Ǜ;else if(Ŗ>value) value=ǜ;}else value=Ŗ;if(ΖΦ&&Ʀ) value=-1;else value=Math.Max(ǜ,Math.Min(value,Ǜ));value=(float) Math.Round(value,2);}void ΒΓ(bool Θύ){if(Θύ) Й=Math.Max(0,Й-1);else Й++;ΒΧ();ΒΦ();for(int k=0;k<ȅ.Count&&k<Л.Length;k++) Βύ(ȅ[k],k<Й);
Ѐ=-1;ΒЇ(true);}T ȧ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΗ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΗ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЃ(){ƭ.Clear();for(int i=0;i<Σ.Count;i++) ƭ.Add(Σ[i]);Σ.Clear();
Αη=true;Γλ=new Ψ(Θ);Γξ=new Ψ(Ε);Θ.δ=false;if(Η==Ζ.Ϋ) Ε.δ=false;for(int i=0;i<ў.Count;i++)
if(!ȉ.Contains(ў.Keys.ElementAt(i)))
ȉ.Add(ў.Keys.ElementAt(i));ȋ=false;ŧ=false;Ũ=false;π(Ϊ.ΔА);}void ΕΗ(){if(ȋ) Θ=Γλ;if(ŧ) Ε=Γξ;if(Ũ){Σ.Clear();for(int i=0;i<ƭ.Count;i++) Σ.Add(ƭ[i]);}Αη=false;ψ();π(Ϊ.Main);}void ΗЈ(){if(!Αη) return;if(Υ!=Α.ж){ΕΗ();return;}if(!Γλ.δ) ȋ=false;if(!Γξ.δ) ŧ=false;if(ƭ.Count<=1) Ũ=false;
IMyShipConnector И=П(MyShipConnectorStatus.Connectable);if(И==null) И=П(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(ȟ,2)<=0.20) ΓΥ++;else ΓΥ=0;if(ΓΥ>=5){if(Η==Ζ.Ϋ&&(Θ.δ||ȋ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.β=Φ.WorldMatrix.Forward;Ε.υ=Φ.WorldMatrix.Left;
Ε.α=Φ.WorldMatrix.Down;Ε.ν=Φ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.δ=true;Ε.Ч=И.Position;}else{Θ.β=Φ.WorldMatrix.Forward;Θ.υ=Φ.WorldMatrix.Left;Θ.α=Φ.WorldMatrix.Down;Θ.ν=Φ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.δ=true;Θ.Ч=И.Position;}}}double Γπ=-1;if(Σ.Count>0) Γπ=Vector3.Distance(ή,Σ.Last().Δ);
double ΘЌ=Math.Max(1.5,Math.Pow(ȟ/100.0,2));double ΖΝ=Math.Max(ȟ*ΘЌ,2);ΖЀ=ΖΝ;if((Γπ==-1)||Γπ>=ΖΝ){Ψ Ω=new Ψ(ή,Αΰ,Ɗ,ƕ,Φ.GetNaturalGravity());Ω.ΖЅ(Ȋ,ȉ);Σ.Add(Ω);}}Ψ е(){if(Η!=Ζ.Ϋ) return њ!=null?њ:Θ;return Εε;}void ļ(Ψ ά,Μ Ф){Εε=ά;if(ί==Μ.Є) ч=Ф;}bool Ƿ(Ψ ά,float ό,bool ΙΠ,out Vector3 ζ)
{if(ΙΠ){Vector3I Γγ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(Γγ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ζ=new Vector3();return false;}Vector3 ΘΛ=Я(Φ,Τ.FatBlock.GetPosition()-ή);Vector3 ΘΚ=Я(Φ,Τ.FatBlock.WorldMatrix.Forward);ζ=ά.Δ-ΒІ(ά.β,ά.α*-1,ΘΛ)-ΒІ(ά.β,ά.α*-1,ΘΚ)*ό;
return true;}else{ζ=ά.Δ;return true;}}Vector3 ȍ(int ΘΘ,int Ş,bool ΘЀ){if(!ΘЀ&&Εώ) return ΓΗ;float ΘΙ=((Н-1f)/2f)-ΘΘ;float ΘЃ=((б-1f)/2f)-Ş;ΓΗ=Ε.Δ+Ε.υ*ΘΙ*Ǡ+Ȝ*-1*ΘЃ*Ǳ;Εώ=true;return ΓΗ;}Vector3 Βή(Vector3 ΘА,float ΓΡ){return ΘА+(ъ*ΓΡ);}ΰ ΒΡ(){float Б=-1;ΰ έ=ΰ.ǳ;
if(Η!=Ζ.Ϋ){ΔЈ();if(ί!=Μ.з){Vector3 Љ=ĕ(ъ,Ȝ*-1,ή-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(Н*Ǡ)/2f&&Math.Abs(Љ.Y)<=(float)(б*Ǳ)/2f){if(Љ.Z<=-1&&Љ.Z>=-Ī*2) return ΰ.ư;if(Љ.Z>-1&&Љ.Z<ε*2) return ΰ.ǳ;}if(ȥ(Ε.Δ,ref Б)) έ=ΰ.ǳ;}if(Θ.δ){if(ȥ(Θ.Δ,ref Б)) έ=ΰ.ǲ;for(int i=0;i<Σ.Count;i++)
if(ȥ(Σ[i].Δ,ref Б)) έ=ΰ.Ȏ;if(Vector3.Distance(ή,Θ.Δ)<dockDist*ε) έ=ΰ.ǲ;if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǲ;}}else{Vector3 ζ=new Vector3();IMyShipConnector Ȃ=П(MyShipConnectorStatus.Connected);if(Θ.δ){if(ȥ(Θ.Δ,ref Б)) έ=ΰ.ǲ;if(Ƿ(Θ,dockDist,true,out ζ))
if(ȥ(ζ,ref Б)) έ=ΰ.ǲ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΗ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*ε&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ε)
if(ȥ(Σ[i].Δ,ref Б)) έ=ΰ.Ȏ;if(Ε.δ){if(ȥ(Ε.Δ,ref Б)) έ=ΰ.ǳ;if(Ƿ(Ε,dockDist,true,out ζ))
if(ȥ(ζ,ref Б)) έ=ΰ.ǳ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΘ;}}return έ;}bool ȥ(Vector3 ζ,ref float Б){float ό=Vector3.Distance(ζ,ή);if(ό<Б||Б==-1){Б=ό;return true;}return false;}void ΒЂ(){if(ύ>0){Λ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){Ȧ();return;}Ε.Δ=ή;Ε.ν=Φ.GetNaturalGravity();Ε.β=Αΰ;Ε.α=Ɗ;Ε.υ=ƕ;
ъ=ћ.WorldMatrix.Forward;Ȝ=Ε.α;if(ъ==Φ.WorldMatrix.Down) Ȝ=Φ.WorldMatrix.Backward;ź(true,true);Ν(Α.Ƕ);ΒЁ();}void ź(bool ǘ,bool ΘЁ){if(ί==Μ.з&&!ǘ) return;bool Θσ=ǘ||ί==Μ.ΒΤ||Н!=й||б!=и||ĥ!=Ƞ;if(Θσ){if(ί!=Μ.з){ί=Μ.Ƴ;ȍ(Ш,Щ,ΘЁ);Λ="Job changed, lost progress";}ĥ=Ƞ;
Н=й;б=и;Щ=0;Ш=0;Л="";Ѐ=-1;Ĩ=0;Ț=0;С=0;Й=0;ΒЇ(true);}}void Βω(){Р(ή,0);ѓ(Ȋ,true);}void ΒЁ(){ǻ(а.ǉ);Βω();Ÿ(Ȇ,false);ǥ(ΔΕ);ί=Μ.Є;Δε(true);ч=ί;π(Ϊ.ų);Ηλ();ΑΣ=true;Αδ=0;for(int i=ż.Count-1;i>=0;i--)
if(ű(ż[i],false)) Αδ++;if(Αδ>0) Λ="Started with damage";}void ψ(){if(ί==Μ.Є){ί=Μ.Ļ;Λ="Job paused";}Ν(Α.ж);ч=ί;Ǌ(false,0,0,0,0);ΒЅ();Δβ(new Vector3(),false);ΕΖ();Βό(ChargeMode.Auto);Βώ(false);ΕΒ(true);ȏ(Α.ж);Ė(false,false,0,0);ѓ(ο,false);ѓ(у,true);ǥ(true);ΑϏ=false;Ƶ();Ɖ=false;
Βϊ();ΑΣ=false;Ơ=false;Ƨ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.Ʊ&&Ѝ!=Ϊ.ΒΔ&&Ѝ!=Ϊ.ǋ)
π(Ϊ.Main);}void Ȧ(){ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){if(!Ε.δ||!Θ.δ) return;ΒЁ();bool Γα=Vector3.Distance(ή,Θ.Δ)<Vector3.Distance(ή,Ε.Δ);if(š==Μ.ĳ) Γα=true;if(š==Μ.Ĵ) Γα=false;if(Γα){ļ(Θ,Μ.ĳ);switch(Д){case ΰ.ΒΗ:Ν(Α.Ȫ);break;case ΰ.Ȏ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.Ǣ);break;default:Ν(Α.ǒ);break;
}}else{ļ(Ε,Μ.Ĵ);switch(Д){case ΰ.ΒΘ:Ν(Α.Ȫ);break;case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.Ȏ:Ν(Α.Ю);break;default:Ν(Α.ǒ);break;}}}else{if(ί!=Μ.Ļ&&ί!=Μ.Ƴ) return;bool ΙΣ=ί==Μ.Ƴ;ΒЁ();bool Γζ=Ų(false)&&Θ.δ;switch(Д){case ΰ.ǳ:Ν(Γζ?Α.Ȩ:Α.Ƕ);break;case ΰ.Ȏ:Ν(Γζ?Α.Ѕ:Α.Ю);break;case ΰ.ǲ:Ν(Γζ?Α.Ǣ:Α.ǽ);break;
case ΰ.ư:{if(Ț!=Й||ΙΣ) Ν(Α.Ǻ);else Ν(Α.ĺ);}break;default:break;}}}void ē(){if(ί==Μ.з&&!Θ.δ) return;if(Η==Ζ.Ϋ&&(!Ε.δ||!Θ.δ)) return;Λ="Move to job";ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){ļ(Ε,Μ.Ĵ);switch(Д){case ΰ.ǳ:Ν(Α.Ǣ);break;case ΰ.Ȏ:Ν(Α.Ю);break;case ΰ.ΒΘ:return;default:Ν(Α.ǒ);break;
}ȏ(Α.Ȫ);}else{switch(Д){case ΰ.ǳ:Ν(Α.Ю);break;case ΰ.Ȏ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.ǽ);break;case ΰ.ư:Ν(Α.Ǻ);break;default:break;}if(ί==Μ.з) ȏ(Α.Ю);else ȏ(Α.Ȥ);ΑϏ=true;}Βω();π(Ϊ.ΒЄ);Ÿ(Ȇ,false);ч=Μ.ǃ;}void ƺ(){if(!Θ.δ) return;Λ="Move home";ΰ Д=ΒΡ();if(Η==Ζ.Ϋ){ļ(Θ,Μ.ĳ);switch(Д)
{case ΰ.Ȏ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ǣ);break;case ΰ.ΒΗ:return;default:Ν(Α.ǒ);break;}ȏ(Α.Ȫ);}else{if(П(MyShipConnectorStatus.Connected)!=null) return;if(П(MyShipConnectorStatus.Connectable)!=null){Ν(Α.đ);ȏ(Α.ǽ);return;}switch(Д){case ΰ.ǳ:Ν(Α.Ȩ);break;case ΰ.Ȏ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ѕ);break;
case ΰ.ư:Ν(Α.ǹ);break;default:break;}ȏ(Α.ǽ);}Βω();π(Ϊ.ΒЄ);Ÿ(Ȇ,false);ч=Μ.ǂ;}void Ν(Α ΓЀ){if(ΓЀ==Α.ж) Ƣ=Α.ж;if(Ƣ!=Α.ж&&Υ==Ƣ&&ΓЀ!=Ƣ){ψ();return;}Ё=true;Υ=ΓЀ;}void ȏ(Α Ф){this.Ƣ=Ф;}void ΗІ(){if(Υ==Α.Ƕ){if(Ё){Ђ=0;if(Ț!=Й) Ĩ=0;Ț=Й;}if(Ђ==0){int ƃ=ΖЌ(Й);if(ƃ<0)
{if(!ƴ()){Ν(Α.ǹ);return;}if(!ΓΝ||(ƃ==-2)!=ΑΞ){ΓΝ=true;ΑΞ=ƃ==-2;Εβ=!ΑΞ?ή:Ε.Δ-ъ*ε+
Ε.υ*(Н*Ǡ/2+ΑЁ/2+ε/2);}Р(Εβ,10);Λ=ΑΞ?"Waiting for a free hole":"Waiting for the controller";return;}ΓΝ=false;if(ƃ!=Й){Й=ƃ;Ĩ=0;Ț=Й;}Ľ έ=Ης(Й);if(έ==Ľ.Δα){ί=Μ.ΒΤ;Λ="Job done";if(ċ&&Θ.δ){Ν(Α.Ȩ);ȏ(Α.ǽ);ч=Μ.ǂ;}else{Ν(Α.ķ);ȏ(Α.Ȥ);ч=Μ.ǃ;}return;}
if(έ==Ľ.Δς){Ђ=1;ѓ(ο,true);л=ȍ(Ш,Щ,true);Р(л,10);Х(Ε.α,Ε.β,Ε.υ,false);}}else{if(Ѐ>=0&&(ΔЅ()?ǩ>=0&&ǩ!=Ѐ:Βε(Ѐ,true))){Ђ=0;return;}if(К<wpReachedDist){Ν(Α.ĺ);return;}}}if(Υ==Α.ĺ){if(Ё){ѓ(ο,true);ǥ(false);л=ȍ(Ш,Щ,false);Р(Βή(л,0),0);Х(Ε.α,Ε.β,Ε.υ,false);
Ђ=1;к=0;Э=0;ΑΛ=0;Γρ=-1;Ī=Ц;Γμ=true;}if(!ƴ()){Ν(Α.ǹ);return;}if(Ų(true)){ΖΤ=Ȍ("","ORE",З.Ď);if((М==Ϗ.ΒΥ||М==Ϗ.ƶ||
М==Ϗ.Βζ||М==Ϗ.ƽ)&&Η!=Ζ.Ǐ)
Ν(Α.Ē);else if((М==Ϗ.Δκ||М==Ϗ.ΒΞ)&&Η!=Ζ.Ǐ)
Ν(Α.ΒЈ);else
Ν(Α.ǹ);return;}С=Vector3.Distance(ή,л);if(С>Ĩ){Ĩ=С;Γμ=false;}if(Η==Ζ.Ǐ&&Δχ()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ђ>0&&Ђ<Э){if(С>к){if(Э>150) к=С;else к=(float) Math.Ceiling(С);Ђ=1;}else Ђ++;}else{if(Ђ>0){Λ="Ship stuck! Retrying";к=С;Ђ=0;Ė(false,true,0,ε*sensorRange);}Р(Βή(л,Math.Max(0,к-ε)),Βα(false));if(К<=wpReachedDist/2){Ђ=1;к=0;}
return;}Ė(false,true,ε*sensorRange,0);Vector3 ΗА=л+ъ*С;bool ΕЋ=false;if(Vector3.Distance(ΗА,ή)>0.3f){Vector3 ΘΜ=л+ъ*(С+0.1f);Р(ΘΜ,4);ΕЋ=true;}else{float ǝ=Βα(true);Vector3 Ει=Βή(л,Math.Max(Ц+1,С+1));Ǥ(true,false,false,Ει,Ει-л,ǝ,ǝ);}bool ΓΣ=false;if(ф==ѐ.ΒΚ||ф==ѐ.ΒΙ)
{if(!ΕЋ){float ΓЂ=0;foreach(IMyTerminalBlock ǰ in ο)
ΓЂ+=ΗΒ(ǰ,"","",ф==ѐ.ΒΙ?new string[]{"STONE"}:null);if(ΓЂ>Γρ||С<Ц||Γμ){ΒЏ=0;ΑΛ=С;Ī=(int)(Math.Max(Ī,ΑΛ)+ε/2);}else{ΓΣ=С-ΑΛ>2&&ΒЏ>=20;ΒЏ++;}Γρ=ΓЂ;}}else ΓΣ=С>=Ī;if(Ț!=Й){Ν(Α.Ǻ);С=0;return;}if(ΓΣ){Й++;Ν(Α.Ǻ);С=0;return;}}if(Υ==Α.Ǻ||Υ==Α.ǹ||Υ==Α.Ē)
{if(Ё){л=ȍ(Ш,Щ,false);Х(Ε.α,Ε.β,Ε.υ,false);ѓ(ο,ň);ǥ(false);к=Vector3.Distance(ή,л);Ė(false,true,0,ε*sensorRange);}Р(л,Βα(false));if(Vector3.Distance(ή,л)>=к+5){ѓ(ο,false);ǥ(true);Λ="Can´t return!";}if(К<wpReachedDist){if(Υ==Α.Ǻ&&ΑϏ) Ν(Α.ķ);if(Υ==Α.Ǻ) Ν(Α.Ƕ);
if(Υ==Α.Ē) Ν(Α.ƹ);if(Υ==Α.ǹ){if(Θ.δ) Ν(Α.Ȩ);else{ψ();ē();Λ="Can´t return, no dock found";}}return;}}if(Υ==Α.Ѕ||Υ==Α.Ю){if(Υ==Α.Ю&&ί==Μ.Є&&Η!=Ζ.Ϋ){if(!ƴ()||Ų(true)){Ν(Α.Ѕ);return;}}bool ΓЋ=false;bool ΑА=false;bool ΕΦ=false;float ł=0;bool ΖΣ=false;Ψ Ω=null;
if(Ё){if(Υ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȑ(1)) ΒΖ();else{њ=null;ȇ=0;Ɖ=!ΒΝ(1);}}Ψ ά=е();η=new Βχ();η.Њ=ά;η.Ƈ=followPathDock*ε;η.ΔΥ=useDockDirectionDist*ε;η.ł=10;η.Ť.Add(ά.Δ);Vector3 Ł=new Vector3();if(Ƿ(ά,dockDist*ε,true,out Ł))
η.Ť.Add(Ł);else η.Ƈ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) η.Αέ=Ε.Δ;if(ά==Ε) η.Αέ=Θ.Δ;η.Γά=dockDist*ε*1.1f;}}else if(Υ==Α.Ю){η=new Βχ();η.Њ=Ε;η.Ƈ=followPathJob*ε;η.ΔΥ=useJobDirectionDist*ε;η.ł=10;η.Αέ=Θ.Δ;η.Γά=dockDist*ε*1.1f;η.Ť.Add(Ε.Δ);if(ί==Μ.з){if(!Θ.δ||Σ.Count==0){ψ();return;}
float ΘΣ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΤ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΣ<ΘΤ) η.Њ=Σ.Last();else η.Њ=Σ.First();}}ƛ=new Vector3();ΖΣ=!ΗΩ(ή);ѓ(ο,false);ǥ(true);У=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αέ)<=η.Γά) continue;
double ό=Vector3.Distance(Σ[i].Δ,ή);if(Ǩ==-1||ό<Ǩ){У=i;Ǩ=ό;}}ǭ=ΗΞ(η.Њ.Δ,У);Ğ=null;}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&!Ź()){Ψ ΓΒ=е();Ɖ=!ΒΝ(1);if(е()!=ΓΒ){Ν(Α.Ѕ);return;}}if(Υ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&ȑ(1)&&ΒΖ()){Ν(Α.Ѕ);return;}bool ΕЉ=false;if(Υ==Α.Ѕ&&!Ё&&У>=0&&У<Σ.Count)
{Vector3 ƅ=Σ[У].Δ+Δψ(У,ǭ);int ΓЁ=У+ǭ;Vector3 ŗ=ǭ!=0&&ΓЁ>=0&&ΓЁ<Σ.Count?Σ[ΓЁ].Δ:η.Њ.Δ;if(ΗΡ(ƅ,ŗ)){if(!ΑΦ){ΑΦ=true;Εξ=ĸ(ƅ)?ή:ƅ;}Р(Εξ,0);Х(η.Њ.α,η.Њ.β,η.Њ.υ,false);Λ="Waiting for dock clearance";return;}ΑΦ=false;ΕЉ=ȑ(1)&&(ĸ(ή)||ĸ(ŗ));}ΖІ(Σ,ǭ,η.Ť,η.Ƈ,Ё,ref Ђ);
for(int i=0;i<η.Ť.Count;i++){float ό=Vector3.Distance(ή,η.Ť[i]);if(ό<=η.Ƈ) ΓЋ=true;if(ό<=η.ΔΥ) ΑА=true;}if(ΕЉ) ΑА=true;if(ΑА) ł=η.ł;float ΚΤ=Ğ!=null?Ğ.Џ:ȟ;float ΙΨ=(float) Math.Max(ȟ*0.1f*ε,wpReachedDist);if((К<ΙΨ)||Ё){if(!Ё) У+=ǭ;if(ǭ==0||У>Σ.Count-1||У<0)
ΓЋ=true;else{ΔΩ=ǭ>0?Σ.Count-1-У:У;Ω=Σ[У];Ğ=Ω;if(У>=1&&У<Σ.Count-1)
ƛ=Ω.Δ-Σ[У-ǭ].Δ;else Ğ=null;χ=Ω.Δ+Δψ(У,ǭ);ΕΦ=true;}}if(ΑА)
Х(η.Њ.α,η.Њ.β,η.Њ.υ,false);else if(ΖΣ)
ΒΕ(η.Њ.α,10,true);else if(ΕΦ&&Ω!=null){if(ǭ>0) Х(Ω.α,Ω.β,Ω.υ,90,false);else Х(Ω.α,-Ω.β,-Ω.υ,90,false);}Ǥ(true,false,true,χ,ƛ,Ğ==null?0:Ğ.Џ,ł);if(ΓЋ){ΔΩ=0;if(Υ==Α.Ѕ&&!ΖЊ()){Р(χ,0);return;}if(Υ==Α.Ѕ&&Ɖ){Р(χ,0);Λ="Waiting for a free dock";return;}if(Υ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.Ǣ);return;}
if(Υ==Α.Ю&&ΑϏ){Ν(Α.ķ);return;}if(Υ==Α.Ю){Ν(Α.Ƕ);return;}}}if(Υ==Α.Ǣ||Υ==Α.Ŷ){if(Η!=Ζ.Ϋ&&Ź()){if(!ȑ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΒ=е();if(!ΒΝ(2)||е()!=ΓΒ){Ν(Α.Ѕ);return;}}Ψ ά=е();if(Ё){if(!Ƿ(ά,dockDist*ε,true,out χ)){ǻ(а.Ŭ);ψ();return;}
Р(χ,0);ΒΕ(ά.α,90,true);}if(К<followPathDock*ε&&К!=-1){Р(χ,10);Х(ά.α,ά.β,ά.υ,false);}if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null){Ν(Α.đ);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.č);return;}}if(Υ==Α.č||Υ==Α.Ȥ){if(Ё)
{if(Υ==Α.č){Ψ ά=е();if(!Ƿ(ά,dockDist*ε,true,out χ)){ǻ(а.Ŭ);ψ();return;}Ǥ(true,true,false,χ,0);Х(ά.α,ά.β,ά.υ,10,false);}if(Υ==Α.Ȥ){Х(Ε.α,Ε.β,Ε.υ,0.5f,false);χ=Ε.Δ;Ǥ(true,true,false,χ,0);}}if(Αζ){Ǌ(false,0,0,0,0);if(Υ==Α.č) Ν(Α.đ);if(Υ==Α.Ȥ) ψ();return;}}if(Υ==Α.đ)
{if(П(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.Ȫ);else Ν(Α.ǽ);return;}Ψ ά=е();if(Ё){Э=0;ΘΥ=DateTime.Now;Ђ=0;Х(ά.α,ά.β,ά.υ,false);}Vector3I Γγ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Εα=Me.CubeGrid.GetCubeBlock(Γγ);float Επ=dockingSpeed;float Θν=dockingSpeed*5;
float ΕΣ=Math.Max(1.5f,Math.Min(5f,ε*0.15f));if(!Ƿ(ά,0,true,out χ)||!Ƿ(ά,ΕΣ,true,out ƛ)||
Εα==null||!Εα.FatBlock.IsFunctional){ǻ(а.Ŭ);ψ();return;}if(Э==1||(Vector3.Distance(ή,χ)<=ΕΣ*1.1f&&!Ё))
Э=1;else{Vector3 ΙΘ=Я(Φ,ƛ-ή);Vector3 Γβ=Я(Φ,Φ.GetNaturalGravity());float ΘЄ=Ʋ(ΙΘ,Γβ,null);Επ=Math.Min(Θν,ΘЄ);}Ǥ(true,false,false,χ,χ-ή,dockingSpeed,Επ);if(Ё) к=(float) К;IMyShipConnector ΓΜ=П(MyShipConnectorStatus.Connectable);if(ΓΜ!=null){Ǥ(false,false,false,χ,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΓΜ.Connect();if(ΓΜ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.Ȫ);else Ν(Α.ǽ);ΒЅ();Ÿ(Ȇ,true);return;}}}else{float Εν=(float) Math.Round(К,1);if(Εν<к){Ђ=-1;к=Εν;}else Ђ++;if(Ђ>20){Ν(Α.Ŷ);return;}}}if(Υ==Α.ǽ||Υ==Α.Ȫ||
Υ==Α.ΑΖ||Υ==Α.Ƹ||Υ==Α.Đ){bool ΒЍ=false,ΒЎ=false;if(Η==Ζ.Ϋ){if(е()==Θ) ΒЍ=true;else if(е()==Ε) ΒЎ=true;}if(Ё){Ơ=false;if(П(MyShipConnectorStatus.Connected)==null){Ν(Α.ǒ);return;}ΒЅ();if(ΒЍ) ΑΒ(ϊ.Ǘ);if(ΒЎ) ΑΒ(ϋ.Ǘ);Į=false;Ȓ=false;Ȗ=false;Ʃ=false;}if(П(MyShipConnectorStatus.Connected)==null){ψ();ǻ(а.ΔЁ);return;}
if(ί!=Μ.Є||ш==-1||ǧ==р.None) Ȓ=true;else if(Ǿ>=100f) Ȓ=true;else if(Ǿ<=99f) Ȓ=false;if(ί!=Μ.Є||щ==-1||і.Count==0) Ȗ=true;else if(ȗ>=100f) Ȗ=true;else if(ȗ<=99) Ȗ=false;if(ί!=Μ.Є||Ы==-1||ć.Count==0) Ʃ=true;else Ʃ=į>=Ы;ľ п=null;if(ΒЍ) п=ϊ;
if(ΒЎ) п=ϋ;if(п!=null&&(п.Ќ==ι.ǆ||п.Ќ==ι.ǅ)) Ȓ=true;if(п!=null&&(п.Ќ==ι.Βξ)) if(!Į) Ȓ=false;if(п!=null&&(п.Ќ==ι.Βς||п.Ќ==ι.Βπ)) Ȗ=true;if(п!=null&&(п.Ќ==ι.Βρ)) if(!Į) Ȗ=false;if(ą){ChargeMode Εί=Ȓ?ChargeMode.Auto:ChargeMode.Recharge;if(п!=null&&(п.Ќ==ι.ǅ||п.Ќ==ι.ǆ))
Εί=ChargeMode.Discharge;Βό(Εί);Βώ(!Ȗ);}if(!Į){if(Η==Ζ.Ϋ)
Į=ί!=Μ.Є||ΖЋ(Ё,true)||Ƨ;else
Į=ί!=Μ.Є||ΔЂ();}else{if(!Ȓ) Ν(Α.Đ);if(!Ȗ) Ν(Α.Ƹ);if(!Ʃ) Ν(Α.ΑΖ);Ё=false;}if(Į&&Ȓ&&Ȗ&&Ʃ){Βό(ChargeMode.Auto);Βώ(false);if(ί==Μ.Є){if(Η==Ζ.Ϋ){if(е()==Θ) ΑΒ(ϊ.ǚ);else if(е()==Ε) ΑΒ(ϋ.ǚ);if(е()==Θ) ļ(Ε,Μ.Ĵ);else ļ(Θ,Μ.ĳ);}}Ν(Α.ǒ);return;}}if(Υ==Α.ǒ){if(Ƭ){if(!Δθ()) return;
Ƭ=false;Ν(Α.ǒ);return;}if(Ё){if(!Δθ()){Ƭ=true;return;}IMyShipConnector Ȃ=П(MyShipConnectorStatus.Connected);if(Ȃ==null){Ν(Α.Ю);return;}IMyShipConnector γ=Ȃ.OtherConnector;ΕΑ(Ȃ,false);Ÿ(Ȇ,false);Ψ ά=null;Ψ Γκ=Η==Ζ.Ϋ?Θ:е();if(Vector3.Distance(Ȃ.GetPosition(),Γκ.Δ)<5f&&Θ.δ) ά=Γκ;
if(Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5f&&Ε.δ) ά=Ε;if(ά!=null){if(!Ƿ(ά,dockDist*ε,true,out χ)){ǻ(а.Ŭ);ψ();return;}Р(χ,5);Х(ά.α,ά.β,ά.υ,false);}else Р(ή+γ.WorldMatrix.Forward*dockDist*ε,5);if(ί==Μ.Є) ǻ(а.ǉ);}if(К<wpReachedDist){ѓ(у,true);Ν(Α.Ю);return;}}if(Υ==Α.ķ)
{if(Ё){ǥ(true);ѓ(ο,false);χ=Ε.Δ;Р(χ,20);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist/2){Ν(Α.Ȥ);return;}}if(Υ==Α.Ȩ){if(Ё){ǥ(true);ѓ(ο,false);int Б=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ή);if(Ǩ==-1||ό<Ǩ){Б=i;Ǩ=ό;}}
if(Б==-1){Ν(Α.Ѕ);return;}χ=Σ[Б].Δ;Р(χ,10);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Υ==Α.ƹ){bool х=false;if(Ё){ǥ(true);if((М==Ϗ.ΒΥ||М==Ϗ.ƶ)&&ƿ()&&
Ķ(ъ,Φ.GetNaturalGravity())<25&&Н>=2&&б>=2){Vector3 Ƌ=ή;if(Ш>0&&Щ<б-1) Ƌ=ȍ(Ш-1,Щ+1,true);else if(Ш<Н-1&&Щ<б-1) Ƌ=ȍ(Ш+1,Щ+1,true);else if(Ш<Н-1&&Щ>0) Ƌ=ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ƌ=ȍ(Ш-1,Щ-1,true);else х=true;if(!х) Р(Ƌ,10);
}else х=true;}if(К<wpReachedDist/2) х=true;if(х){Ν(Α.ΑΕ);return;}}if(Υ==Α.ΒЈ||Υ==Α.ΑΕ){if(Ё){Ǥ(true,true,false,ή,0);ѓ(ο,false);ǥ(true);Ђ=-1;Э=М==Ϗ.Βζ||М==Ϗ.ƽ?0:-1;}bool Ġ=!ƴ();int ΑЈ=Ȍ("STONE","ORE",З.Ď);if(М==Ϗ.ΒΞ||М==Ϗ.ƽ||М==Ϗ.ƶ)
ΑЈ+=Ȍ("ICE","ORE",З.Ď);bool Θά=ΑЈ>0;bool ΓΫ=false;if(Э>=0){float Ŀ=(float) Math.Sin(Δμ(Э))*Ǡ/3f;float ΘΑ=(float) Math.Cos(Δμ(Э))*Ǳ/3f;Vector3 ΘΗ=ȍ(Ш,Щ,true)+ΒІ(ъ,Ȝ*-1,new Vector3(Ŀ,ΘΑ,0));Р(ΘΗ,0.3f);if(К<Math.Min(Ǡ,Ǳ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ђ==-1||ΑЈ<Ђ){Ђ=ΑЈ;к=0;}
else{к++;if(к>50) ΓΫ=true;}if(!Θά||Ġ||ΓΫ){if(!Ġ){int ΙΝ=Ȍ("","ORE",З.Ď);if(Ų(true)) Ġ=true;else if(100-(А(ΙΝ,ΖΤ)*100)<minEjection) Ġ=true;else ǻ(а.ǉ);}if(ΓΫ&&Ġ) Λ="Ejection failed";if(Υ==Α.ΑΕ){if(Ġ){if(Θ.δ) Ν(Α.Ȩ);else{ψ();ē();Λ="Can´t return, no dock found";}}
else Ν(Α.Ƕ);}else if(Ġ) Ν(Α.ǹ);else Ν(Α.ĺ);return;}}Ё=false;}void ΖІ(List<Ψ>Č,int κ,List<Vector3>Ƥ,float Ǯ,bool Ȉ,ref int ǔ){if(Ȉ){for(int i=0;i<Č.Count;i++) Č[i].Џ=0;ǔ=-1;return;}if(κ==0) return;int ѝ=κ*-1;if(ǔ==-1) ǔ=ѝ>0?1:Č.Count-2;int ΕЍ=0;while(ǔ>=1&&ǔ<Č.Count-1)
{if(ΕЍ>50) return;ΕЍ++;try{if((ѝ<0&&ǔ>=1)||(ѝ>0&&ǔ<=Č.Count-2)){Ψ θ=Č[ǔ];bool ΖΠ=false;for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(θ.Δ,Ƥ[j])<=Ǯ){ΖΠ=true;break;}if(!ΖΠ){Ψ Αϋ=Č[ǔ-ѝ];Ψ ŗ=Č[ǔ+ѝ];Vector3 ΙΙ=θ.Δ-ŗ.Δ;Vector3 ΔΟ=Αϋ.Δ-θ.Δ;Vector3 Θώ=θ.Δ+Vector3.Normalize(ΙΙ)*ΔΟ.Length();Vector3 ΘΡ=Αϋ.Δ-Θώ;Vector3 Αμ=ĕ(κ>0?θ.β:θ.β*-1,θ.α*-1,ΘΡ);Vector3 Θμ=ĕ(κ>0?θ.β:θ.β*-1,θ.α*-1,ΔΟ);
Vector3 Αξ=ĕ(κ>0?θ.β:θ.β*-1,θ.α*-1,θ.ν);θ.Џ=(float) Math.Sqrt(Math.Pow(Αϋ.Џ,2)+Math.Pow(Ʋ(-Θμ,Αξ,θ),2));for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(Αϋ.Δ,Ƥ[j])<=Ǯ){Vector3 Αο=ĕ(κ>0?θ.β:θ.β*-1,θ.α*-1,Ƥ[j]-θ.Δ);float ΙΑ=Ʋ(-Αο,Αξ,θ);θ.Џ=Math.Min(θ.Џ,ΙΑ)/2f;}if(Αμ.Length()==0) Αμ=new Vector3(0,0,1);Vector3 Θλ=ĕ(θ.β,θ.α*-1,θ.ν);float ΙΓ=Ů(-Αμ,Θλ,θ);float ΑΗ=А(ΙΓ,Ĭ);float ΔΝ=(float) Math.Sqrt(А(Αμ.Length()*1.0f,0.5f*ΑΗ));
θ.Џ=Math.Min(θ.Џ,(ΔΟ.Length()/ΔΝ)*Ǧ);}}}catch{return;}ǔ+=ѝ;}ǔ=-1;}void ΒЇ(bool Ȉ){if(Ȉ){ƒ=0;return;}if(Η==Ζ.Ϋ) return;float х=Й*Math.Max(1,Ц);if(Ț==Й) х+=Math.Min(Ц,С);float ΙΜ=Н*б*Math.Max(1,Ц);ƒ=Math.Max(ƒ,(float) Math.Min(х/ΙΜ*100.0,100));}MyDetectedEntityType Δχ()
{try{if(Ǹ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βα(bool ǫ){if(Η==Ζ.Ǐ&&Δχ()==MyDetectedEntityType.None&&!ű(ξ,true))
return fastSpeed;else return ǫ?Ū:ũ;}void ΗЀ(){float ΖΚ=0,Εη=0,Ĥ=0,ΓЃ=0;for(int i=0;i<ȫ.Count;i++){IMyBatteryBlock ƀ=ȫ[i];if(!Ǹ(ƀ,true)) continue;ΖΚ+=ƀ.MaxStoredPower;Εη+=ƀ.CurrentStoredPower;Ĥ+=ƀ.CurrentInput;ΓЃ+=ƀ.CurrentOutput;}Ǿ=(float) Math.Round(А(Εη,ΖΚ)*100,1);
if(Ĥ>=ΓЃ) ǧ=р.Đ;else ǧ=р.Δν;if(Ĥ==0&&ΓЃ==0||Ǿ==100.0) ǧ=р.ж;if(ȫ.Count==0) ǧ=р.None;}void ΗЄ(){float Εφ=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζρ=і[i];if(!Ǹ(Ζρ,true)) continue;Εφ+=(float) Ζρ.FilledRatio;}ȗ=А(Εφ,і.Count)*100f;}void ΗЌ(){į=0;try{for(int i=0;i<ć.Count;i++)
{IMyReactor ΓЌ=ć[i];if(!Ǹ(ΓЌ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЌ.GetInventory(0).GetItems(Ъ);float Ǔ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǔ+=(float) Г.Amount;}if(Ǔ<į||i==0){į=Ǔ;Γψ=ΓЌ.CustomName;}}}catch(Exception e){ș=e;}}void ΗЁ(){ŕ=0;Ň=0;try{for(int i=0;i<ņ.Count;i++){IMyTerminalBlock Τ=ņ[i];if(!Ǹ(Τ,true)) continue;Ň+=(float) Τ.GetInventory(0).CurrentVolume;ŕ+=(float) Τ.GetInventory(0).MaxVolume;
}Αλ=(float) Math.Min(Math.Round(А(Ň,ŕ)*100,1),100.0);}catch(Exception e){ș=e;}}void ΗЅ(){try{Ǚ.Clear();for(int i=0;i<ņ.Count;i++){IMyTerminalBlock Τ=ņ[i];if(!Ǹ(Τ,true)) continue;Δΰ(Τ,З.Ű);}if(М!=Ϗ.Βν){for(int i=0;i<ę.Count;i++){IMyTerminalBlock Τ=ę[i];if(!Ǹ(Τ,true)) continue;
Δΰ(Τ,З.ΗΥ);}}Ηψ(Ǚ);}catch(Exception e){ș=e;}}void ΖЉ(){if(ΑΣ){if(Δφ().Count>Αδ){ΑΣ=false;if(м!=д.ů){ψ();if(м==д.ΒΪ) ē();if(м==д.ǈ)
if(Θ.δ) ƺ();else ē();}Λ="Damage detected";}}}bool ƴ(){if(!Αε) return true;if(ί==Μ.Є){if(ш>0&&ǧ!=р.None)
if(Ǿ<=ш){Λ="Low energy! Move home";return false;}if(Ы>0&&ć.Count>0)
if(į<=Ы){Λ="Low fuel: "+Γψ;return false;}if(щ>0&&і.Count>0)
if(ȗ<=щ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δφ(){List<IMyTerminalBlock>ğ=new List<IMyTerminalBlock>();for(int i=0;i<ż.Count;i++){IMyTerminalBlock Τ=ż[i];if(ű(Τ,false)) ğ.Add(Τ);}return ğ;}bool ű(IMyTerminalBlock Τ,bool ΔΗ){return(!Ǹ(Τ,ΔΗ)||!Τ.IsFunctional);
}bool Ǹ(IMyTerminalBlock Τ,bool ΔΗ){if(Τ==null) return false;try{IMyCubeBlock Εύ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΔΗ) return Εύ==Τ;else return Εύ.GetType()==Τ.GetType();}catch{return false;}}float ΗΒ(IMyTerminalBlock Τ,String я,String Ζθ,String[]Ǫ){float ȃ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ŏ=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Ŏ.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(Ǫ!=null&&(Ǫ.Contains(Г.Type.TypeId.ToUpper())||Ǫ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((я==""||Г.Type.TypeId.ToUpper()==я)&&(Ζθ==""||Г.Type.SubtypeId.ToUpper()==Ζθ))
ȃ+=(float) Г.Amount;}}return ȃ;}ё ΗΙ(String φ,String ς,З Д,bool ΘΝ){φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ё Г=Ǚ[i];if(Г.φ.ToUpper()==φ&&Г.ς.ToUpper()==ς&&(Г.Œ==Д||Д==З.Ď))
return Г;}ё έ=null;if(ΘΝ){έ=new ё(φ,ς,0,Д);Ǚ.Add(έ);}return έ;}int Ȍ(String φ,String ς,З Д){return Ȍ(φ,ς,Д,null);}int Ȍ(String φ,String ς,З Д,String[]Ǫ){int ȃ=0;φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ё Г=Ǚ[i];if(Ǫ!=null&&Ǫ.Contains(Г.φ.ToUpper())) continue;
if((φ==""||Г.φ.ToUpper()==φ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.Œ==Д||Д==З.Ď))
ȃ+=Г.Ǔ;}return ȃ;}void Δΰ(IMyTerminalBlock Τ,З Д){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΙ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).Ǔ+=(int) Ъ[j].Amount;}}void Ηψ(List<ё>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){ё a=Т[j];ё b=Т[j+1];if(a.Ǔ<b.Ǔ) Т.Move(j,j+1);}}bool Ų(bool ΚΕ){if(ȣ&&Η!=Ζ.Ϋ)
if(О!=-1&&Ĭ>=О){Λ="Ship too heavy";return true;}if(Αλ>=Ǭ||Ơ){Ơ=false;Λ="Ship is full";return true;}return false;}bool ΔЂ(){String[]Ǫ=null;if(!Ȣ) Ǫ=new string[]{"ICE"};if(Η==Ζ.ŵ) return Ȍ("","ORE",З.Ű,Ǫ)==0;if(Η==Ζ.Ǐ) return Ȍ("","COMPONENT",З.Ű,Ǫ)==0;else return Ȍ("","",З.Ű,Ǫ)==0;
}bool ΗΪ(){List<IMyCockpit>ΓΛ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΛ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΛ.Count;i++)
if(ΓΛ[i].IsUnderControl) return true;return false;}bool ΖЋ(bool Ȉ,bool ΑЃ){IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δι(ϊ,Ȉ,ΑЃ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δι(ϋ,Ȉ,ΑЃ);return false;}bool Δι(ľ ȁ,bool Ȉ,bool ΑЃ){if(Ȉ) ȁ.Ηθ();ȁ.ΗЉ();bool о=false;String э="";switch(ȁ.Ќ){case ι.Βο:{э="Waiting for command";о=false;break;}case ι.ΔЍ:{э="Waiting for cargo";о=Ų(true);break;}case ι.ΔЌ:{э="Unloading";о=ΔЂ();break;}case ι.Βτ:{о=true;break;}
case ι.Βξ:{э="Charging batteries";о=Ǿ>=100f;break;}case ι.ǆ:{э="Discharging batteries";о=Ǿ<=25f;break;}case ι.ǅ:{э="Discharging batteries";о=Ǿ<=0f;break;}case ι.Βρ:{э="Filling up hydrogen";о=ȗ>=100f;break;}case ι.Βς:{э="Unloading hydrogen";о=ȗ<=25f;break;}
case ι.Βπ:{э="Unloading hydrogen";о=ȗ<=0f;break;}case ι.Βσ:{bool ΕЈ=ΗΪ();if(!ΕЈ) ȁ.ΓЈ=true;о=ȁ.ΓЈ&&ΕЈ;э="Waiting for passengers";break;}}if(!о) ȁ.ΒΜ(true);if(о&&ȁ.Βγ()){о=ȁ.ΒΜ(false);э="Undocking in: "+Βά((int) Math.Max(0,ȁ.ǖ-ȁ.ȱ));}if(ΑЃ) Ζώ=э;return о;}void ΖЂ()
{if(!ŏ) return;if(ο.Count<=1) return;float Ζχ=0,Ζφ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(ű(ǰ,true)) continue;Ζχ+=(float) ǰ.GetInventory(0).MaxVolume;Ζφ+=(float) ǰ.GetInventory(0).CurrentVolume;}float ΕΧ=(float) Math.Round(А(Ζφ,Ζχ),5);for(int ΖΧ=0;ΖΧ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΧ++)
{float Γθ=0,Γχ=0;float ΕЅ=0,ΖΘ=0;IMyTerminalBlock ȕ=null,Ȳ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǰ=ο[i];if(ű(ǰ,true)) continue;float Γώ=(float) ǰ.GetInventory(0).MaxVolume;float Αή=А((float) ǰ.GetInventory(0).CurrentVolume,Γώ);if(ȕ==null||Αή>Γθ){ȕ=ǰ;Γθ=Αή;ΕЅ=Γώ;}
if(Ȳ==null||Αή<Γχ){Ȳ=ǰ;Γχ=Αή;ΖΘ=Γώ;}}if(ȕ==null||Ȳ==null||ȕ==Ȳ) return;if(checkConveyorSystem&&!ȕ.GetInventory(0).IsConnectedTo(Ȳ.GetInventory(0))){if(ΓΑ>20) Λ="Inventory balancing failed";else ΓΑ++;return;}ΓΑ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȕ.GetInventory(0).GetItems(Ъ);float ƫ=0;if(Ъ.Count==0) continue;MyInventoryItem Αί=Ъ[0];String ΕЌ=Αί.Type.TypeId+Αί.Type.SubtypeId;if(!Ζύ.TryGetValue(ΕЌ,out ƫ)){if(Ηί(ȕ.GetInventory(0),0,Ȳ.GetInventory(0),out ƫ))
Ζύ.Add(ΕЌ,ƫ);else return;}float ΙΚ=((Γθ-ΕΧ)*ΕЅ/ƫ);float ΙΛ=((ΕΧ-Γχ)*ΖΘ/ƫ);int ΔΣ=(int) Math.Min(ΙΛ,ΙΚ);if(ΔΣ<=0) return;if((float) Αί.Amount<ΔΣ)
ȕ.GetInventory(0).TransferItemTo(Ȳ.GetInventory(0),0,null,null,Αί.Amount);else
ȕ.GetInventory(0).TransferItemTo(Ȳ.GetInventory(0),0,null,null,ΔΣ);}}bool Ηί(IMyInventory ơ,int Β,IMyInventory ΘΠ,out float Ζό){Ζό=0;float ΘΔ=(float) ơ.CurrentVolume;List<MyInventoryItem>ΓΓ=new List<MyInventoryItem>();ơ.GetItems(ΓΓ);float ΕΪ=0;for(int i=0;i<ΓΓ.Count;i++) ΕΪ+=(float) ΓΓ[i].Amount;
ơ.TransferItemTo(ΘΠ,Β,null,null,1);float Ζϋ=ΘΔ-(float) ơ.CurrentVolume;List<MyInventoryItem>ΒЊ=new List<MyInventoryItem>();ơ.GetItems(ΒЊ);float ΕΞ=0;for(int i=0;i<ΒЊ.Count;i++) ΕΞ+=(float) ΒЊ[i].Amount;if(Ζϋ==0f||!Βη(0.9999,ΕΪ-ΕΞ,1.0001))
return false;Ζό=Ζϋ;return true;}void ΗЃ(){Vector3 Ǝ=χ+Ċ;Vector3 ǯ=Ǝ-ή;if(ǯ.Length()==0) ǯ=new Vector3(0,0,-1);Vector3 Αο=Я(Φ,ǯ);Vector3 Ζς=Vector3.Normalize(Αο);Vector3 ő=Я(Φ,Φ.GetNaturalGravity());float ΘЍ=ΓІ>0?Math.Max(0,1-Ķ(ǯ,Εψ)/5):0;float Џ=(float) Math.Min((Γϋ>0?Γϋ:1000f),Math.Max(Ʋ(-Αο,ő,null),ΓІ*ΘЍ));
if(!Ζυ) Џ=0;if(Ζβ) Џ=Math.Max(0,1-ΑΠ/ΑΘ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(ś) Џ=0;if(ю>=0) Џ=Math.Min(Џ,ю);if(Ζγ) Џ*=(float) Math.Min(1,А(ǯ.Length(),wpReachedDist)/2);Vector3 ΖΕ=Я(Φ,Φ.GetShipVelocities().LinearVelocity);float ΗБ=(float)(Math.Max(0,15-Ķ(-Ζς,-ΖΕ))/15)*0.85f+0.15f;
ΔΛ+=Math.Sign(ΗБ-ΔΛ)/10f;Vector3 ȭ=Ζς*Џ*ΔΛ-(ΖΕ);Vector3 ΒБ=Βΰ(ȭ,null);if(ΑЊ&&К>0.1f){ȭ.X*=ΒЀ(ȭ.X,ref ΑЋ.X,1f,ΒБ.X,20);ȭ.Y*=ΒЀ(ȭ.Y,ref ΑЋ.Y,1f,ΒБ.Y,20);ȭ.Z*=ΒЀ(ȭ.Z,ref ΑЋ.Z,1f,ΒБ.Z,20);}else ΑЋ=new Vector3(1,1,1);ƥ=Ĭ*ȭ-ő*Ĭ;Δβ(ƥ,ΑЊ);К=Vector3.Distance(ή,Ǝ);
}float ΒЀ(float value,ref float Ф,float ѝ,float ț,float Γό){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ѝ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γό||ț==0) return έ;Ф=Math.Min(Γό,Math.Max(-Γό,Ф));έ=Math.Abs(ț);return έ;
}void ΕΜ(){float ь=90,ǎ=90,ќ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǫ,ј,Ɩ,Γτ;if(Γυ){ǫ=Vector3.Normalize(χ-ή);Ɩ=Я(Φ,ǫ);ј=Я(Φ,ΔΙ);ь=Ķ(Ɩ,new Vector3(0,-1,0))-90;ǎ=ď(ј,new Vector3(-1,0,0),ј.Y);
ќ=ď(Ɩ,new Vector3(-1,0,0),Ɩ.Z);}else{ǫ=ΔΚ;ј=Я(Φ,ΔΙ);Ɩ=Я(Φ,ΔΚ);Γτ=Я(Φ,Ζσ);ь=ď(ј,new Vector3(0,0,1),ј.Y);ǎ=ď(ј,new Vector3(-1,0,0),ј.Y);ќ=ď(Γτ,new Vector3(0,0,1),Γτ.X);}if(ΕΠ&&ƿ()){Vector3 Γβ=Φ.GetNaturalGravity();ј=Я(Φ,Γβ);ь=ď(ј,new Vector3(0,0,1),ј.Y);
ǎ=ď(ј,new Vector3(-1,0,0),ј.Y);}if(!Βη(-45,ǎ,45)){ь=0;ќ=0;}if(!Βη(-45,ќ,45)) ь=0;Ǌ(Γδ,1,(-ь)*ǝ,(-ќ)*ǝ,(-ǎ)*ǝ);ΑΠ=Math.Max(Math.Abs(ь),Math.Max(Math.Abs(ǎ),Math.Abs(ќ)));Αζ=ΑΠ<=ΑΘ;}void ΖЇ(IMyTerminalBlock Αό){if(Αό==null) return;ǡ=new float[3,2];ў=new Dictionary<string,float[,]>();
for(int i=0;i<Ȋ.Count;i++){IMyThrust в=Ȋ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(Αό,в.WorldMatrix.Backward);float Ȱ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǡ[0,0]+=Ȱ;else ǡ[0,1]-=Ȱ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǡ[1,0]+=Ȱ;else ǡ[1,1]-=Ȱ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǡ[2,0]+=Ȱ;else ǡ[2,1]-=Ȱ;String я=Δϊ(в);float[,]џ=null;if(ў.ContainsKey(я)) џ=ў[я];else{џ=new float[3,2];ў.Add(я,џ);}float ț=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) џ[0,0]+=ț;else џ[0,1]-=ț;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) џ[1,0]+=ț;else џ[1,1]-=ț;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) џ[2,0]+=ț;else џ[2,1]-=ț;}}static String Δϊ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βι(Vector3 ѕ,float[,]œ){return new Vector3(
ѕ.X>=0?œ[0,0]:œ[0,1],
ѕ.Y>=0?œ[1,0]:œ[1,1],
ѕ.Z>=0?œ[2,0]:œ[2,1]);}bool Δω(Ψ Ω,String я,out float ΓΪ){ΓΪ=0;int Β=ȉ.IndexOf(я);if(Β==-1||Ω.ǟ==null||Β>=Ω.ǟ.Length)
return false;ΓΪ=Ω.ǟ[Β];if(ΓΪ==-1) return false;return true;}Vector3 Βΰ(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<ў.Keys.Count;i++){String я=ў.Keys.ElementAt(i);float ƌ=0;if(!Δω(Ω,я,out ƌ))
return Βι(ѕ,ǡ);έ+=Βι(ѕ,ў.Values.ElementAt(i))*ƌ;}return έ;}return Βι(ѕ,ǡ);}float Ů(Vector3 ѕ,Ψ Ω){return Ů(ѕ,new Vector3(),Ω);}float Ů(Vector3 ѕ,Vector3 ν,Ψ Ω){Vector3 ΙΒ=Βΰ(ѕ,Ω);Vector3 Ȱ=ΙΒ+ν*Ĭ;float Ɯ=(Ȱ/ѕ).AbsMin();return(float)(ѕ*Ɯ).Length();}float Ʋ(Vector3 Αν,Vector3 ő,Ψ Ω)
{if(Αν.Length()==0) return 0;float ΕΟ=1;if(ő.Length()>0) ΕΟ=Math.Min(1,Ķ(-ő,Αν)/90)*0.4f+0.6f;float ƥ=Ů(Αν,ő,Ω);if(ƥ==0) return 0.1f;float ΑΗ=А(ƥ,Ĭ);float ΔΝ=(float) Math.Sqrt(А(Αν.Length(),ΑΗ*0.5f));return ΑΗ*ΔΝ*ΕΟ*Ǧ;}float Ƽ(Vector3 ǫ,Vector3 ώ,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Αξ=ĕ(ǫ,ώ,Vector3.Normalize(ν));float ǘ=Ů(-Αξ,Ω);return ǘ/ν.Length();}void Δε(bool Θβ){float Ǎ=0;float Γέ=0.9f;if(Θβ){О=-1;ı=0;ΑІ=null;if(ί!=Μ.з&&Ε.ν.Length()!=0){Ǎ=Γέ*Ƽ(Ε.β,Ε.α*-1,Ε.ν,null);if(Ǎ<О||О==-1) О=Ǎ;}if(Θ.δ&&Θ.ν.Length()!=0)
{Ǎ=Γέ*Ƽ(Θ.β,Θ.α*-1,Θ.ν,null);if(Ǎ<О||О==-1) О=Ǎ;}return;}if(ı==-1) return;if(ı>=0){int ΖΪ=0;while(ı<Σ.Count){if(ΖΪ>100) return;ΖΪ++;Ψ Ω=Σ[ı];if(Ω.ν.Length()!=0f){Ǎ=Γέ*Math.Min(
Ƽ(Ω.β,Ω.α*-1,Ω.ν,Ω),
Ƽ(Ω.β*-1,Ω.α*-1,Ω.ν,Ω));if(Ǎ<О||О==-1) О=Ǎ;}else ΑІ=Ω;ı++;}ı=-1;}bool Γε=true;float Ć=0;if(ΑІ!=null){for(int i=0;i<ў.Count;i++){String я=ў.Keys.ElementAt(i);float[,]џ=ў.Values.ElementAt(i);float ƌ=0;if(!Δω(ΑІ,я,out ƌ)){Γε=false;break;}for(int a=0;a<џ.GetLength(0);a++)
for(int b=0;b<џ.GetLength(1);b++){float Ę=Math.Abs(џ[a,b]*ƌ);if(Ę==0) continue;Γε=true;if(Ć==0||Ę<Ć) Ć=Ę;}}}if(!Γε){for(int a=0;a<ǡ.GetLength(0);a++)
for(int b=0;b<ǡ.GetLength(1);b++){float Ę=Math.Abs(ǡ[a,b]);if(Ę==0) continue;if(Ć==0||Ę<Ć) Ć=Ę;}}if(Ć>0){float Θο=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǎ=А(Ć,Θο);if(Ǎ>0)
if(Ǎ<О||О==-1) О=Ǎ;}}void Δβ(Vector3 ǘ,bool Ј){if(!Ј){for(int i=0;i<Ȋ.Count;i++)
Ȋ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒА=Βΰ(ǘ,null);float ΙΪ=Math.Min(1,Math.Abs(А(ǘ.X,ΒА.X)));float ΙΫ=Math.Min(1,Math.Abs(А(ǘ.Y,ΒА.Y)));float Ιά=Math.Min(1,Math.Abs(А(ǘ.Z,ΒА.Z)));for(int i=0;i<Ȋ.Count;i++){IMyThrust в=Ȋ[i];Vector3 κ=Ηκ(Я(Φ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǘ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΪ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǘ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΫ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǘ.Z))
в.SetValueFloat("Override",в.MaxThrust*Ιά);else
в.SetValueFloat("Override",0.0f);}}void Ǌ(bool Ј,float Θϋ,float ь,float ќ,float ǎ){for(int i=0;i<Αα.Count;i++){IMyGyro І=Αα[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=Θϋ;if(!Ј) continue;Vector3 В=Φ.WorldMatrix.Forward;Vector3 Ь=Φ.WorldMatrix.Right;Vector3 ώ=Φ.WorldMatrix.Up;
Vector3 ŉ=І.WorldMatrix.Forward;Vector3 ŋ=І.WorldMatrix.Up;Vector3 Ŋ=І.WorldMatrix.Left*-1;if(ŉ==В) І.SetValueFloat("Roll",ǎ);else if(ŉ==(В*-1)) І.SetValueFloat("Roll",ǎ*-1);else if(ŋ==(В*-1)) І.SetValueFloat("Yaw",ǎ);else if(ŋ==В) І.SetValueFloat("Yaw",ǎ*-1);else if(Ŋ==В) І.SetValueFloat("Pitch",ǎ);
else if(Ŋ==(В*-1)) І.SetValueFloat("Pitch",ǎ*-1);if(Ŋ==(Ь*-1)) І.SetValueFloat("Pitch",ь);else if(Ŋ==Ь) І.SetValueFloat("Pitch",ь*-1);else if(ŋ==Ь) І.SetValueFloat("Yaw",ь);else if(ŋ==(Ь*-1)) І.SetValueFloat("Yaw",ь*-1);else if(ŉ==(Ь*-1)) І.SetValueFloat("Roll",ь);
else if(ŉ==Ь) І.SetValueFloat("Roll",ь*-1);if(ŋ==(ώ*-1)) І.SetValueFloat("Yaw",ќ);else if(ŋ==ώ) І.SetValueFloat("Yaw",ќ*-1);else if(Ŋ==ώ) І.SetValueFloat("Pitch",ќ);else if(Ŋ==(ώ*-1)) І.SetValueFloat("Pitch",ќ*-1);else if(ŉ==ώ) І.SetValueFloat("Roll",ќ);else if(ŉ==(ώ*-1)) І.SetValueFloat("Roll",ќ*-1);
}}void ΕΖ(){this.Γδ=false;}void Х(Vector3 ȯ,Vector3 В,Vector3 Γσ,float ȡ,bool ƪ){ΒΕ(ȯ,ȡ,ƪ);ΑΘ=ȡ;Γυ=false;this.ΔΚ=В;this.Ζσ=Γσ;}void Х(Vector3 ȯ,Vector3 В,Vector3 Γσ,bool ƪ){Х(ȯ,В,Γσ,2f,ƪ);}void ΒΕ(Vector3 ȯ,float ȡ,bool ƪ){ΑΘ=ȡ;this.Γδ=true;this.ΕΠ=ƪ;Γυ=true;Αζ=false;
this.ΔΙ=ȯ;}void ΒЅ(){Ǥ(false,false,false,χ,0);ΑЊ=false;}void Р(Vector3 Њ,float ǝ){Ǥ(true,false,false,Њ,ǝ);}void Ǥ(bool Ј,bool ΔΔ,bool ΔΦ,Vector3 Њ,float ǝ){Ǥ(Ј,ΔΔ,ΔΦ,Њ,Њ-ή,0.0f,ǝ);}void Ǥ(bool Ј,bool ΔΔ,bool ΔΦ,Vector3 Њ,Vector3 ΘЇ,float ΘЈ,float ǝ){ΑЊ=true;this.Ζυ=Ј;
χ=Њ;this.Γϋ=ǝ;this.ΓІ=ΘЈ;this.Ζγ=ΔΔ;this.Ζβ=ΔΦ;this.Εψ=ΘЇ;К=Vector3.Distance(Њ,ή);}void Ηλ(){ω.GetBlocksOfType(ż,ǐ);}void Ημ(){if(Αώ){ύ=2;return;}List<IMyRemoteControl>Ɲ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζΰ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζκ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ɲ,ǐ);ω.GetBlocksOfType(ї,ǐ);ω.GetBlocksOfType(Ζΰ,ǐ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζκ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=ǣ(ї,pamTag,true);Ησ();Ητ(Ζκ);ŷ(ї,setLCDFontAndSize,1.4f,false);ŷ(ĉ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζξ=ǣ(Ζΰ,pamTag,true);if(Ζξ.Count>0) ξ=Ζξ[0];else ξ=null;if(Η==Ζ.ŵ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Λ="Drills are missing";}}else if(Η==Ζ.Ǐ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Λ="Grinders are missing";}if(ξ==null){ύ=1;Λ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ω.GetBlocksOfType(ΔΞ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζν=ǣ(Ɲ,pamTag,true);if(Ζν.Count>0) Ɲ=Ζν;if(Ɲ.Count>0) Φ=Ɲ[0];else{Φ=null;ύ=2;Λ="Remote is missing";return;}ε=(float) Φ.CubeGrid.WorldVolume.Radius*2;ћ=null;if(Η!=Ζ.Ϋ){Δζ();if(ο.Count>0&&ћ!=null){if(ξ!=null&&(ћ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Λ="Wrong sensor direction";}if(ћ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ћ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ύ=2;Λ="Wrong remote direction";}}}}void Ην(){ω.GetBlocksOfType(Ȇ,ǐ);ω.GetBlocksOfType(у,ǐ);ω.GetBlocksOfType(Ȋ,ǐ);ω.GetBlocksOfType(Αα,ǐ);ω.GetBlocksOfType(ȫ,ǐ);ω.GetBlocksOfType(ć,ǐ);ω.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑЅ,ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
у=ǣ(у,"ConnectorMedium",false);else
у=ǣ(у,"Connector",false);List<IMyShipConnector>Ζλ=ǣ(у,pamTag,true);if(Ζλ.Count>0) у=Ζλ;if(ύ<=1){if(у.Count==0){ύ=1;Λ="Connector is missing";}if(Αα.Count==0){ύ=1;Λ="Gyros are missing";}if(Ȋ.Count==0){ύ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Ζο=ǣ(ΑЅ,pamTag,true);
if(Ζο.Count>0) ΑЅ=Ζο;List<IMyLandingGear>Ζμ=ǣ(Ȇ,pamTag,true);if(Ζμ.Count>0) Ȇ=Ζμ;for(int i=0;i<Ȇ.Count;i++) Ȇ[i].AutoLock=false;List<IMyBatteryBlock>Ζι=ǣ(ȫ,pamTag,true);if(Ζι.Count>0) ȫ=Ζι;List<IMyGasTank>Ζπ=ǣ(і,pamTag,true);if(Ζπ.Count>0) і=Ζπ;}void Ηξ(){ω.GetBlocksOfType(ę,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ņ.Clear();for(int i=ę.Count-1;i>=0;i--){if(ΗΧ(ę[i])){ņ.Add(ę[i]);ę.RemoveAt(i);}}}bool ΗΧ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βδ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δζ(){if(Φ==null) return;ћ=null;float Αφ=0,Αρ=0,Αχ=0,Ας=0,Αψ=0,Ασ=0;List<IMyTerminalBlock>ΔΘ=ǣ(ο,pamTag,true);bool Θφ=ΔΘ.Count==0;if(ΔΘ.Count>0) ћ=ΔΘ[0];else if(ο.Count>0) ћ=ο[0];int ȃ=0;float Ŝ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward){if(Θφ){ύ=2;Λ="Mining direction is unclear!";return;}continue;}ȃ++;Ŝ=Math.Max(Ŝ,Ηώ(ο[i]));Vector3 Љ=ƻ(Φ,ο[i].GetPosition());if(i==0){Αφ=Љ.X;Αρ=Љ.X;Αχ=Љ.Y;Ας=Љ.Y;Αψ=Љ.Z;Ασ=Љ.Z;}Αρ=Math.Max(Љ.X,Αρ);Αφ=Math.Min(Љ.X,Αφ);
Ας=Math.Max(Љ.Y,Ας);Αχ=Math.Min(Љ.Y,Αχ);Ασ=Math.Max(Љ.Z,Ασ);Αψ=Math.Min(Љ.Z,Αψ);}if(Ŝ==0) Ŝ=drillRadius;bool ȯ=ћ!=null&&ћ.WorldMatrix.Forward==Φ.WorldMatrix.Down;Ǡ=(Αρ-Αφ)*(1-Ĳ)+Ŝ*2;Ǳ=(ȯ?Ασ-Αψ:Ας-Αχ)*(1-Ģ)+Ŝ*2;IMyCubeGrid g=Φ.CubeGrid;float ΔΫ=0,Δά=0,Δέ=0,Δή=0;
for(int c=0;c<8;c++){Vector3 p=ƻ(Φ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȯ?p.Z:p.Y;if(c==0||p.X<ΔΫ) ΔΫ=p.X;if(c==0||p.X>Δά) Δά=p.X;if(c==0||y<Δέ) Δέ=y;if(c==0||y>Δή) Δή=y;}ΑЁ=Δά-ΔΫ+g.GridSize;ΓА=Δή-Δέ+g.GridSize;}float Ηώ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǣ<T>(List<T>Т,String Ǟ,bool Αβ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βδ(Т[i],Ǟ,Αβ)) έ.Add(Т[i]);return έ;}bool Βδ<T>(T Τ,String Ǟ,bool Αβ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αβ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αβ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ѓ<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΕΑ((IMyTerminalBlock) Т[i],Ј);}void ΕΑ(IMyTerminalBlock Τ,bool Ј){if(Τ==null) return;String ΗЏ=Ј?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЏ);a.Apply(Τ);}void ǥ(bool Ј){ΔΕ=Ј;if(!ĭ) return;ѓ(ΑЅ,Ј);}void Βό(ChargeMode Ƙ){for(int i=0;i<ȫ.Count;i++) ȫ[i].ChargeMode=Ƙ;}void Βώ(bool ΘЏ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΘЏ;}void Ÿ(List<IMyLandingGear>Γΰ,bool ΖΖ){for(int i=0;i<Γΰ.Count;i++){if(ΖΖ) Γΰ[i].Lock();if(!ΖΖ) Γΰ[i].Unlock();}}void ΕΒ(bool Ј){if(Εθ==Ј) return;List<IMyShipController>ΑΝ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΝ,ǐ);if(ΑΝ.Count==0) return;
for(int i=0;i<ΑΝ.Count;i++)
ΑΝ[i].DampenersOverride=Ј;Εθ=Ј;}IMyShipConnector П(MyShipConnectorStatus э){for(int i=0;i<у.Count;i++){if(!Ǹ(у[i],true)) continue;if(у[i].Status==э) return у[i];}return null;}void Ė(bool ΔΑ,bool ΑΥ,float ΘΩ,float ΘΓ){if(ξ==null||ο.Count==0) return;Vector3 ΑΜ=new Vector3();
int ȃ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward) continue;ȃ++;ΑΜ+=ο[i].GetPosition();}ΑΜ=ΑΜ/ȃ;Vector3 Ř=ƻ(ξ,ΑΜ);ξ.Enabled=true;ξ.ShowOnHUD=ΔΑ;ξ.LeftExtend=(ΑΥ?1:й)/2f*Ǡ-Ř.X;ξ.RightExtend=(ΑΥ?1:й)/2f*Ǡ+Ř.X;ξ.TopExtend=(ΑΥ?1:и)/2f*Ǳ+Ř.Y;
ξ.BottomExtend=(ΑΥ?1:и)/2f*Ǳ-Ř.Y;ξ.FrontExtend=(ΔΑ?Ц:ΘΩ)-Ř.Z;ξ.BackExtend=ΔΑ?0:ΘΓ+ε*0.75f+Ř.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ŷ<T>(List<T>Т,bool ΘЊ,float ΘΧ,bool Θγ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƣ=null;if(Т[i]is IMyTextSurface) ƣ=(IMyTextSurface) Т[i];if(ƣ!=null){ƣ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЊ) continue;ƣ.Font="Debug";if(Θγ) continue;ƣ.FontSize=ΘΧ;}}}bool ƿ(){Vector3D ζ;return this.Φ.TryGetPlanetPosition(out ζ);}int ΗΞ(Vector3 ΘБ,int Ζδ){if(Ζδ==-1) return 0;double Б=-1;int Ȕ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘБ);if(Б==-1||ό<Б){Ȕ=i;Б=ό;}
}return Math.Sign(Ȕ-Ζδ);}bool ΗΩ(Vector3 ζ){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.δ&&Σ.Count>=1){Vector3 Ł=new Vector3();Ƿ(Θ,dockDist*ε,false,out Ł);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,Ł);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(Ł);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.δ&&Σ.Count>=1){Vector3 ΒЌ=new Vector3();Ƿ(Ε,dockDist*ε,false,out ΒЌ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЌ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЌ);Ж.Add(Ε.Δ);}}}else{if(ί!=Μ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȕ=-1;double Ǩ=-1;for(int i=0;i<Ж.Count;i++){double ό=Vector3.Distance(Ж[i],ζ);if(ό<Ǩ||Ǩ==-1){Ǩ=ό;Ȕ=i;}}if(Ж.Count==0) return false;double ΓΤ=Vector3.Distance(Ж[Ȕ],ζ);double Θό=Vector3.Distance(Ж[Math.Max(0,Ȕ-1)],Ж[Ȕ])*1.5f;
double Θυ=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȕ+1)],Ж[Ȕ])*1.5f;return ΓΤ<Θό||ΓΤ<Θυ||ΓΤ<laneOffset*ε*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηκ(Vector3 v,int ΓΠ){return new Vector3(Math.Round(v.X,ΓΠ),Math.Round(v.Y,ΓΠ),Math.Round(v.Z,ΓΠ));
}float Ķ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΦ=(a*b).Sum;float ΖΑ=a.Length();float ΖΒ=b.Length();if(ΖΑ==0||ΖΒ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΦ/(ΖΑ*ΖΒ)));if(float.IsNaN(έ)) return 0;return έ;}float ď(Vector3 v,Vector3 Αό,float ΘЋ){float Ŀ=Ķ(v,Αό);
if(ΘЋ>0f) Ŀ*=-1;if(Ŀ>-90f) return Ŀ-90f;else return 180f-(-Ŀ-90f);}double Δμ(float ΘΞ){return(Math.PI/180)*ΘΞ;}bool Βη(double Θξ,double value,double Απ){return(value>=Θξ&&value<=Απ);}Vector3 ΗϏ(IMyTerminalBlock Τ,Vector3 Љ){return Vector3D.Transform(Љ,Τ.WorldMatrix);
}Vector3 ƻ(IMyTerminalBlock Τ,Vector3 ΙΧ){return Я(Τ,ΙΧ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΪ){return Vector3D.TransformNormal(ΔΪ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 ĕ(Vector3 ǫ,Vector3 ώ,Vector3 ΔΪ){MatrixD Γϊ=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.TransformNormal(ΔΪ,MatrixD.Transpose(Γϊ));
}Vector3 ΒІ(Vector3 ǫ,Vector3 ώ,Vector3 Љ){MatrixD Γϊ=MatrixD.CreateFromDir(ǫ,ώ);return Vector3D.Transform(Љ,Γϊ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Κ(Е,0)),float.Parse(Κ(Е,1)),float.Parse(Κ(Е,2)));
}String ΗЍ(Ψ Ω){String Ћ=":";String έ=τ(Ω.Δ)+Ћ+τ(Ω.β)+Ћ+
τ(Ω.α)+Ћ+τ(Ω.υ)+Ћ+τ(Ω.ν);for(int i=0;i<Ω.ǟ.Length;i++)
έ+=Ћ+Math.Round(Ω.ǟ[i],3);return έ;}Ψ Ηω(String s){String[]Е=s.Split(':');Ψ Ω=new Ψ(
μ(Κ(Е,0)),μ(Κ(Е,1)),
μ(Κ(Е,2)),μ(Κ(Е,3)),
μ(Κ(Е,4)));int Β=5;List<float>Ετ=new List<float>();while(Β<Е.Length){String ΑБ=Κ(Е,Β);float f=0;if(!float.TryParse(ΑБ,out f)) break;Ετ.Add(f);Β++;}Ω.ǟ=Ετ.ToArray();return Ω;}System.Text.StringBuilder Š=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θτ){if(Θτ) Š.Append('\n');Š.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʒ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΓ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]ΘΒ,int Β){String ΑБ=ΘΒ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑБ)) return "";
return ΑБ;}void Save(){if(Αώ||Η==Ζ.с){Storage="";return;}Storage=ΒΛ();}String ΒΛ(){Š.Clear();Š.Append(DATAREV+";");Ρ(τ(Αύ),false);Ρ(τ(Θ.β));Ρ(τ(Θ.υ));Ρ(τ(Θ.α));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.δ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.β));Ρ(τ(Ε.α));Ρ(τ(Ε.υ));Ρ(τ(Ε.Ч));Ρ(Ε.δ);Ρ(τ(ъ));Ρ(τ(Ȝ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) ч);Ρ(Ǭ);Ρ(ш);Ρ(Ы);Ρ(щ);Ρ(Ǧ);Ρ(ȣ);Ρ(Ȣ);if(Η==Ζ.Ϋ){Ρ((int) ϊ.Ќ);Ρ(ϊ.ǖ);Ρ(ϊ.ȱ);Ρ(Ʒ(ϊ.Ǘ));Ρ(Ʒ(ϊ.ǚ));Ρ((int) ϋ.Ќ);Ρ(ϋ.ǖ);Ρ(ϋ.ȱ);Ρ(Ʒ(ϋ.Ǘ));Ρ(Ʒ(ϋ.ǚ));}else{Ρ((int) Ƞ);Ρ((int) м);Ρ((int) М);Ρ((int) ф);Ρ((int) ĥ);Ρ(ċ);Ρ(ĭ);Ρ(ŏ);Ρ(ň);Ρ(й);Ρ(и);Ρ(Ц);Ρ(Ū);Ρ(ũ);Ρ(Ĳ);Ρ(Ģ);
Ρ(Н);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(Ț);Ρ(Й);Ρ(Ī);Ρ(Ĩ);}Ρ(";");for(int i=0;i<ȉ.Count;i++)
Ρ((i>0?"|":"")+ȉ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЍ(Σ[i]),i>0);Ρ(";",false);Ρ(Л+"|"+Ѐ,false);Ρ(";",false);Ρ(ΗΕ(),false);String Χ=Š.ToString();Š.Clear();return Χ;}т Ηι(){return Ǉ(Storage);}т Ǉ(String Χ){if(Χ=="") return т.Ηα;String[]ī=Χ.Split(';');if(Κ(ī,0)!=DATAREV) return т.Βμ;int Β=0;try{String[]Ι=Κ(ī,1).Split('\n');
Αύ=μ(Κ(Ι,Β++));Θ.β=μ(Κ(Ι,Β++));Θ.υ=μ(Κ(Ι,Β++));Θ.α=μ(Κ(Ι,Β++));Θ.ν=μ(Κ(Ι,Β++));Θ.Δ=μ(Κ(Ι,Β++));Θ.Ч=μ(Κ(Ι,Β++));Θ.δ=bool.Parse(Κ(Ι,Β++));Ε.Δ=μ(Κ(Ι,Β++));Ε.ν=μ(Κ(Ι,Β++));Ε.β=μ(Κ(Ι,Β++));Ε.α=μ(Κ(Ι,Β++));Ε.υ=μ(Κ(Ι,Β++));Ε.Ч=μ(Κ(Ι,Β++));Ε.δ=bool.Parse(Κ(Ι,Β++));
ъ=μ(Κ(Ι,Β++));Ȝ=μ(Κ(Ι,Β++));Ι=Κ(ī,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Κ(Ι,Β++));ί=(Μ) int.Parse(Κ(Ι,Β++));ч=(Μ) int.Parse(Κ(Ι,Β++));Ǭ=int.Parse(Κ(Ι,Β++));ш=int.Parse(Κ(Ι,Β++));Ы=int.Parse(Κ(Ι,Β++));щ=int.Parse(Κ(Ι,Β++));Ǧ=float.Parse(Κ(Ι,Β++));ȣ=bool.Parse(Κ(Ι,Β++));
Ȣ=bool.Parse(Κ(Ι,Β++));if(Η==Ζ.Ϋ){ϊ.Ќ=(ι) int.Parse(Κ(Ι,Β++));ϊ.ǖ=float.Parse(Κ(Ι,Β++));ϊ.ȱ=float.Parse(Κ(Ι,Β++));ϊ.Ǘ=ΑΓ(Κ(Ι,Β++));ϊ.ǚ=ΑΓ(Κ(Ι,Β++));ϋ.Ќ=(ι) int.Parse(Κ(Ι,Β++));ϋ.ǖ=float.Parse(Κ(Ι,Β++));ϋ.ȱ=float.Parse(Κ(Ι,Β++));ϋ.Ǘ=ΑΓ(Κ(Ι,Β++));ϋ.ǚ=ΑΓ(Κ(Ι,Β++));}else
{Ƞ=(Ǒ) int.Parse(Κ(Ι,Β++));м=(д) int.Parse(Κ(Ι,Β++));М=(Ϗ) int.Parse(Κ(Ι,Β++));ф=(ѐ) int.Parse(Κ(Ι,Β++));ĥ=(Ǒ) int.Parse(Κ(Ι,Β++));ċ=bool.Parse(Κ(Ι,Β++));ĭ=bool.Parse(Κ(Ι,Β++));ŏ=bool.Parse(Κ(Ι,Β++));ň=bool.Parse(Κ(Ι,Β++));й=int.Parse(Κ(Ι,Β++));и=int.Parse(Κ(Ι,Β++));Ц=int.Parse(Κ(Ι,Β++));
Ū=float.Parse(Κ(Ι,Β++));ũ=float.Parse(Κ(Ι,Β++));Ĳ=float.Parse(Κ(Ι,Β++));Ģ=float.Parse(Κ(Ι,Β++));Н=int.Parse(Κ(Ι,Β++));б=int.Parse(Κ(Ι,Β++));Ш=int.Parse(Κ(Ι,Β++));Щ=int.Parse(Κ(Ι,Β++));Ț=int.Parse(Κ(Ι,Β++));Й=int.Parse(Κ(Ι,Β++));Ī=int.Parse(Κ(Ι,Β++));Ĩ=float.Parse(Κ(Ι,Β++));
}Ι=Κ(ī,3).Replace("\n","").Split('|');ȉ=Ι.ToList();Ι=Κ(ī,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηω(Κ(Ι,i)));Ι=Κ(ī,5).Split('|');Л=Κ(Ι,0);if(!int.TryParse(Κ(Ι,1),out Ѐ)) Ѐ=-1;if(Л.Length!=Н*б){Л="";Ѐ=-1;}ΗΔ(Κ(ī,6));}catch{return т.Δπ;}š=ч;ψ();return т.ǌ;}const int Δδ=64000;IMyTerminalBlock Δρ(){List<IMyTerminalBlock>ń=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ń,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ń.Count>0?ń[0]:null;}void ΗΘ(){IMyTerminalBlock Τ=Δρ();if(Τ==null){Λ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΛ();if(Χ.Length>Δδ){Λ="Export too large: "+Χ.Length+" chars, max "+Δδ;return;}Τ.CustomData=Χ;Λ="Exported "+Σ.Count+" waypoints";}void ΗΣ(bool Γο){IMyTerminalBlock Τ=Δρ();if(Τ==null){Λ="Import: name a block "+dataTag;return;}
ΗΤ(Τ.CustomData,Γο);}void ΗΤ(String Χ,bool Γο){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΡ=Χ.IndexOf('\n');Χ=ΖΡ<0?"":Χ.Substring(ΖΡ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}ψ();String ΕΩ=ΒΛ();т έ=Ǉ(Χ);if(έ==т.ǌ&&Γο){Ψ Ș=new Ψ(Ε);
Vector3 Θρ=ъ,Θπ=Ȝ;int w=й,h=и,d=Ц;Ǒ ΔΖ=Ƞ;ѐ ΓΡ=ф;float Θω=Ĳ,Θψ=Ģ;Ǉ(ΕΩ);Ε=Ș;ъ=Θρ;Ȝ=Θπ;й=w;и=h;Ц=d;Ƞ=ΔΖ;ф=ΓΡ;Ĳ=Θω;Ģ=Θψ;ί=Μ.Ļ;ź(true,true);ί=Μ.Ļ;ч=Μ.Ļ;Λ="Imported job, run CONT";return;}if(έ==т.ǌ){ƙ=false;Λ="Imported "+Σ.Count+" waypoints";
return;}Ǉ(ΕΩ);Λ=έ==т.Βμ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ησ(){Ɔ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γν=false;if(Χ=="INSTRUCTIONS"){Γν=true;ΑΤ=true;}if(Χ=="DEBUG") Γν=true;if(Γν){Ɔ.Add(ї[i]);ї.RemoveAt(i);}
}ŷ(Ɔ,false,1,false);}void Ητ(List<IMyTerminalBlock>ń){ĉ.Clear();for(int i=0;i<ń.Count;i++){IMyTerminalBlock Τ=ń[i];try{String ΖΩ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΩ);int ţ=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΩ.Length,1),out ţ)) continue;
if(ţ==-1) continue;ţ--;IMyTextSurfaceProvider ΖΫ=(IMyTextSurfaceProvider) Τ;if(ţ<ΖΫ.SurfaceCount&&ţ>=0)
ĉ.Add(ΖΫ.GetSurface(ţ));}catch{}}}String Ατ="";void ΔЎ(){String Γω="";String ΓΩ="";Ατ=Δγ(false);Γω+=Ατ;ΓΩ+=Ατ;ΓΩ+=ΗΜ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γω);for(int i=0;i<ĉ.Count;i++) ĉ[i].WriteText(Γω);Echo(ΓΩ);for(int i=0;i<Ɔ.Count;i++){IMyTextPanel ΓЅ=Ɔ[i];
String Χ=ΓЅ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЅ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЅ.WriteText(ΗΝ());}}string ΗΝ(){String Γ="";try{float ΖΙ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ń*ΖΙ)+" / "+(int)(Ŕ*ΖΙ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ń*100f,1)+"% / "+Math.Round(Ŕ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ſ,2)+"ms / "+Math.Round(Γύ,2)+"ms \n";}catch{}for(int i=0;i<ȝ.Count;i++)
Γ+=""+ȝ.Keys.ElementAt(i)+" = "+Math.Round(ȝ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȝ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΠ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ŷ(ї,setLCDFontAndSize,0.9f,false);ŷ(ĉ,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+ș.ToString());for(int i=0;i<ĉ.Count;i++) ĉ[i].WriteText(Γ+ș.ToString());}void Ǽ(){if(!ΑΤ) return;try{ΓЉ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String φ){if(!ΑΤ) return;if(ΓЉ==0) return;try{float ΑЏ=(Runtime.CurrentInstructionCount-ΓЉ)/Runtime.MaxInstructionCount*100;if(!ȝ.ContainsKey(φ))
ȝ.Add(φ,new float[]{ΑЏ,ΑЏ});else{ȝ[φ][0]=ΑЏ;ȝ[φ][1]=Math.Max(ΑЏ,ȝ[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЃ(String ȳ){if(!Ņ) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=ū(ΓБ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=ΒΫ(ή.X)+""+Ћ;ρ+=ΒΫ(ή.Y)+Ћ;ρ+=ΒΫ(ή.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=Βέ(ч)+(ч==Μ.Є?" "+Math.Round(ƒ,1)+"%":"")+Ћ;else ρ+=Βέ(ч)+Ћ;ρ+=ū(Λ)+Ћ;ρ+=љ+""+Ћ;ρ+=ū(Ατ)+Ћ;ρ+=Ň+Ћ;ρ+=ŕ+Ћ;for(int i=0;i<Ǚ.Count;i++){if(Ǚ[i].Œ!=З.Ű) continue;ρ+=ū(Ǚ[i].φ)+"/"+ū(Ǚ[i].ς)+"/"+Ǚ[i].Ǔ+Ћ;}Ηπ(ρ,ȳ);}String ū(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηπ(String ρ,String ȳ){Ηρ(ρ,BROADCAST_SELF_ID,ȳ);}void Ηρ(String ρ,String Њ,String ȳ){try{if(!Ņ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δτ()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΓΕ+BROADCAST_SEP+ȳ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){ș=e;}
}String Δτ(){if(Η!=Ζ.с) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЄ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool ŭ(ref String Χ,out string Ευ,Char Ћ){int Β=Χ.IndexOf(Ћ);Ευ="";if(Β<0) return false;Ευ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηβ(ref String ρ,out String Ɵ,out String ȳ)
{Ɵ="";ȳ="";if(!Ņ) return false;String Ǟ="";if(!ŭ(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЄ(Ǟ)) return false;if(!ŭ(ref ρ,out Ɵ,BROADCAST_SEP)) return false;if(!ŭ(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δτ()&&(Ǟ!="*"&&Η!=Ζ.с))) return false;if(!ŭ(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΕ)) return false;
if(!ŭ(ref ρ,out ȳ,BROADCAST_SEP)) return false;return true;}void ΗΦ(){bool Ƅ=true;if(Εχ){Εχ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ə=(Η!=Ζ.с?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ə)
ΔЀ(Ə);String[]Ι=Me.CustomData.Split('\n');Ņ=bool.Parse(Βφ(ΒΩ(Ι,"Enable_Broadcast",ref Ƅ)));bool ΔΨ=false;bool Εμ=true;if(Ņ){if(Η!=Ζ.с)
ΓБ=Βφ(ΒΩ(Ι,"Ship_Name",ref Ƅ)).Replace(BROADCAST_SELF_ID,"");ΓΕ=Βφ(ΒΩ(Ι,"Broadcast_Channel",ref Ƅ)).ToLower();Εμ=false;if(ȓ==null){ȓ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȓ.SetMessageCallback("");}List<IMyRadioAntenna>ŀ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ŀ);bool ΒЋ=false;
for(int i=0;i<ŀ.Count;i++)
if(ŀ[i].EnableBroadcasting&&ŀ[i].Enabled){ΒЋ=true;break;}if(ŀ.Count==0) Λ="No Antenna found";else if(!ΒЋ) Λ="Antenna not ready";ΔΨ=ŀ.Count==0||!ΒЋ;if(ΕΡ&&!ΔΨ&&Η!=Ζ.с) Λ="Antenna ok";}else if(Η==Ζ.с)
Λ="Offline - Enable in PB custom data";ΕΡ=ΔΨ;if(Εμ){if(ȓ!=null) this.IGC.DisableBroadcastListener(ȓ);ȓ=null;}}catch{Ƅ=false;}if(!Ƅ){Λ="Reset custom data";ΔЀ(Ə);}}void ΔЀ(String Ə){String Χ=Ə+"\n\n"+"Enable_Broadcast="+(Η==Ζ.с?"true":"false")+" \n";Χ+=Η!=Ζ.с?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βφ(String ħ){int Β=ħ.IndexOf("//");if(Β!=-1) ħ=ħ.Substring(0,Β);String[]Е=ħ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΩ(String[]Ι,String Ħ,ref bool ƚ){foreach(String ħ in Ι)
if(ħ.StartsWith(Ħ)) return ħ;ƚ=false;return "";}String Ισ="";Ȑ Ιυ=null;DateTime Ιφ=DateTime.MinValue;List<Ȑ>Ιτ=new List<Ȑ>();const float Ιή=20f;String ΔБ(int ΔΧ,String Γ,int Ζί,ref int Ȟ){String[]Ι=Γ.Split('\n');if(Ζί>=Ȟ+ΔΧ-1) Ȟ++;Ȟ=Math.Min(Ι.Count()-1-ΔΧ,Ȟ);if(Ζί<Ȟ+1) Ȟ--;
Ȟ=Math.Max(0,Ȟ);String έ="";for(int i=0;i<ΔΧ;i++){int ΖΓ=i+Ȟ;if(ΖΓ>=Ι.Count()) break;έ+=Ι[ΖΓ]+"\n";}return έ;}String Βά(int ƞ){if(ƞ>=60*60) return Math.Round(ƞ/(60f*60f),1)+" h";if(ƞ>=60) return Math.Round(ƞ/60f,1)+" min";return ""+ƞ+" s";}string ΒΫ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǿ=new List<IMyCameraBlock>();List<long>ř=new List<long>();bool ś=false;int ΓΚ=0;int ΓΘ=0;DateTime Εά=DateTime.Now;String ΓΔ="";void Ηο(){ǿ.Clear();ř.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǿ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǿ.Count;i++){ǿ[i].EnableRaycast=true;if(!ř.Contains(ǿ[i].CubeGrid.EntityId)) ř.Add(ǿ[i].CubeGrid.EntityId);}if(!ř.Contains(Me.CubeGrid.EntityId)) ř.Add(Me.CubeGrid.EntityId);}bool ΔЃ(Α s){return s==Α.Ƕ||s==Α.Ю||s==Α.Ѕ||
s==Α.Ȩ||s==Α.ķ||s==Α.Ē||s==Α.ƹ||
(s==Α.Ǣ&&њ!=null);}void Ƶ(){ś=false;ΓΚ=0;ΓΔ="";}void ΗЇ(){if(!obstacleBrake||ǿ.Count==0||!ΔЃ(Υ)){Ƶ();return;}Vector3 Ǝ=χ+Ċ;Vector3 ǯ=Ǝ-ή;float Ǯ=ǯ.Length();if(Ǯ<1f){Ƶ();return;}Vector3 κ=ǯ/Ǯ;float ΖΗ=ȟ*obstacleLookAhead+ε;Vector3 ΘϏ=Ǝ+κ*(ε*0.25f);
for(int n=0;n<ǿ.Count;n++){ΓΘ=(ΓΘ+1)%ǿ.Count;IMyCameraBlock Ƃ=ǿ[ΓΘ];if(!Ƃ.IsFunctional||!Ƃ.IsWorking) continue;Vector3 Εή=Ƃ.GetPosition();Vector3 Αά=ΘϏ;if(Vector3.Distance(Εή,Αά)>ΖΗ) Αά=Εή+κ*ΖΗ;if(!Ƃ.CanScan(Αά)) continue;MyDetectedEntityInfo Ɛ=Ƃ.Raycast(Αά);bool Θχ=!Ɛ.IsEmpty()&&!ř.Contains(Ɛ.EntityId)&&!Ηά(Ɛ.EntityId)&&
Ɛ.Type!=MyDetectedEntityType.FloatingObject;if(Θχ){if(!ś) Εά=DateTime.Now;ś=true;ΓΚ=0;ΓΔ=Ɛ.Type.ToString();}else if(ś&&++ΓΚ>=3) Ƶ();break;}if(ś&&(DateTime.Now-Εά).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΓΔ;}const String ΕΛ="[PAMTFC]";const float Ηό=5f;const float ΒΨ=2f;class ȩ{public long λ;public Vector3 ζ,İ;public Vector3 Ō;public float Ελ;public String Ș="";public int Γι=-1;public bool ΓЊ;public int Ƒ;public float Γί,Γή;public long ά=0;public int ƈ=0;public DateTime ΑЌ;
}IMyBroadcastListener ΑЎ=null;long ы=0;List<ȩ>н=new List<ȩ>();Vector3 Ċ=new Vector3();float ю=-1;Vector3 ΔΠ=new Vector3();bool ΑЍ=false;DateTime ΔΡ=DateTime.Now;const float ΕΚ=15f;const float Ηϋ=10f;int Ζω=0;String Л="";int Ѐ=-1;bool ΓΝ=false,ΑΞ=false;Vector3 Εβ=new Vector3();
String Ĺ(){if(ί==Μ.з||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Н+"|"+б+"|"+(int) ĥ+"|"+Math.Round(Ǡ,1)+"|"+Math.Round(Ǳ,1);}void ΗЊ(){if(!trafficEnabled) return;if(ΑЎ==null) ΑЎ=IGC.RegisterBroadcastListener(ΕΛ);ы=Me.CubeGrid.EntityId;int Ĝ=30;while(ΑЎ.HasPendingMessage&&Ĝ-->0){String Χ=ΑЎ.AcceptMessage().Data as String;if(Χ!=null) Ηε(Χ);
}for(int i=н.Count-1;i>=0;i--)
if((DateTime.Now-н[i].ΑЌ).TotalSeconds>Ηό) н.RemoveAt(i);if(ΑЉ==null) ΑЉ=IGC.RegisterBroadcastListener(Ηϊ);Ĝ=5;while(ΑЉ.HasPendingMessage&&Ĝ-->0){String Χ=ΑЉ.AcceptMessage().Data as String;if(Χ!=null) try{Ηζ(Χ);}catch{}}if(ΑΟ==null) ΑΟ=IGC.RegisterBroadcastListener(Ηέ);
Ĝ=10;while(ΑΟ.HasPendingMessage&&Ĝ-->0){String Χ=ΑΟ.AcceptMessage().Data as String;if(Χ!=null) try{Ηγ(Χ);}catch{}}if(++Ζω<3) return;Ζω=0;IGC.SendBroadcastMessage(ΕΛ,ы+";"+τ(ή)+";"+
τ(Φ.GetShipVelocities().LinearVelocity)+";"+ε+";"+Ĺ()+";"+
(ί==Μ.Є?Ѐ:-1)+";"+Л+";"+ΖЍ()+","+Δξ()+";"+τ(ΔΠ)+
";"+(ΑΧ?1:0)+";"+(Ā&&ί==Μ.Є?1:0)+","+ΑЁ+","+ΓА+","+Ηή());}void Ηε(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ы) return;ȩ Ї=null;for(int i=0;i<н.Count;i++) if(н[i].λ==λ) Ї=н[i];if(Ї==null){Ї=new ȩ();Ї.λ=λ;н.Add(Ї);}
Ї.ζ=μ(f[1]);Ї.İ=μ(f[2]);Ї.Ελ=float.Parse(f[3]);Ї.Ș=f[4];Ї.Γι=int.Parse(f[5]);Ї.ΑЌ=DateTime.Now;if(Ї.Ș!=""&&Ї.Ș==Ĺ()) ΔЋ(f[6]);Ї.ά=0;Ї.ƈ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.ƈ=int.Parse(d[1]);}Ї.Ō=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЊ=f.Length>=10&&f[9]=="1";Ї.Ƒ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.Γί=float.Parse(w[1]);Ї.Γή=float.Parse(w[2]);Ї.Ƒ=int.Parse(w[3]);}}}catch{}}bool Ηά(long λ){for(int i=0;i<н.Count;i++) if(н[i].λ==λ) return true;return false;}Vector3 Δψ(int Β,int Θϊ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΞ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θϊ;Vector3 ώ=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.α;Vector3 Ь=Vector3.Cross(ΙΞ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ό=laneOffset*ε;float ΘΨ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΨ/(ό*4));}Vector3 Ζψ=new Vector3();bool Δύ(){Α s=Υ;return Ĺ()!=""&&(s==Α.Ƕ||s==Α.ĺ||s==Α.Ǻ||
s==Α.ǹ||s==Α.Ē);}int Ηή(){if(!Δύ()) return 0;return Υ!=Α.Ƕ&&Vector3.Dot(ή-л,ъ)>ε*0.5f?2:1;}bool ΗΫ(Α s){return s==Α.Ǣ||s==Α.č||s==Α.đ||s==Α.Ŷ||
s==Α.ǒ||s==Α.Ǻ||s==Α.ǹ||s==Α.Ē||s==Α.Ȥ;}void ΗЋ(){Ċ=new Vector3();ю=-1;ΔΠ=new Vector3();bool Εϊ=ΔЃ(Υ);if(!trafficEnabled||(!Εϊ&&!ΗΫ(Υ))){ΑЍ=false;return;}Ζψ=Φ.GetShipVelocities().LinearVelocity;ΖЎ(Φ.GetNaturalGravity(),!Εϊ);ΖЁ();}IMyBroadcastListener ΑЉ=null;
float ť=-1;DateTime Ζτ=DateTime.MinValue;void Ηζ(String Χ){if(!Χ.StartsWith("C;")) return;Ζτ=DateTime.Now;ť=-1;String Ɨ=ы+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) ť=float.Parse(e[i].Substring(Ɨ.Length));}void ΖЁ(){if(ť<0||(DateTime.Now-Ζτ).TotalSeconds>1.5) return;
ю=ю<0?ť:Math.Min(ю,ť);if(ť<0.5f) Λ="Traffic: giving way (controller)";}void ΖЎ(Vector3 ν,bool ΖΨ=false){Ċ=new Vector3();ю=-1;Vector3 ǯ=χ-ή;float Ǯ=ǯ.Length();bool Γη=Ǯ>=1f;Vector3 κ=Γη?ǯ/Ǯ:new Vector3();ΔΠ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɗ;
Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-ƕ:Vector3.Normalize(Ь);ȩ Ακ=null;bool Εό=Υ==Α.Ѕ&&!ȑ(1);for(int i=0;i<н.Count;i++){ȩ γ=н[i];if(γ.ƈ==3&&γ.İ.LengthSquared()<0.25f) continue;Vector3 ζ=γ.ζ+γ.İ*(float)(DateTime.Now-γ.ΑЌ).TotalSeconds;Vector3 ŝ=ζ-ή;
float Ǖ=(ε+γ.Ελ)*0.5f;bool Ζέ=γ.Ș!=""&&γ.Ș==Ĺ();if(Ζέ&&γ.Ƒ==2) continue;bool ΑЄ=Ζέ&&γ.Ƒ==1&&Δύ();if(ΑЄ) Ǖ=Math.Min(Ǡ,Ǳ)*0.9f;float ΓΞ=Ǖ*(ΑЄ?1f:1.2f);float Ž=Γη?Vector3.Dot(ŝ,κ):0;if(Ž>0&&(ŝ-κ*Ž).Length()<=ΓΞ){float Αω=Vector3.Dot(γ.İ,κ);bool ΕЂ=γ.Ō.LengthSquared()>0.25f;
bool ΕЃ=Αω<-1f||(ΕЂ&&Vector3.Dot(γ.Ō,κ)<-0.5f);if(!ΖΨ){if(γ.ΓЊ){if(!Εό){Ċ=Ь*Ǖ*1.3f;continue;}float ΖΞ=Vector3.Distance(ή,χ),ΕЎ=Vector3.Distance(ζ,χ);if(ΖΞ<ΕЎ||(ΖΞ==ΕЎ&&ы<γ.λ)) continue;}if(ΕЃ){if(Ž<(ȟ-Αω)*6f+Ǖ*4f) Ċ=Ь*Ǖ*1.3f;continue;}if(ΕЂ&&ы<γ.λ)
{Vector3 ΕΨ=-ŝ;float ΖΥ=Vector3.Dot(ΕΨ,γ.Ō);if(ΖΥ>0&&(ΕΨ-γ.Ō*ΖΥ).Length()<=ΓΞ){Ċ=Ь*Ǖ*1.3f;continue;}}}float Εϋ=Ž-Ǖ-(ΑЄ?1f:followGap*ε);float Αι=Math.Max(0,Αω)+Math.Max(0,Εϋ)/ΒΨ;if(ΕЃ) Αι=Math.Max(0,Εϋ+Math.Min(0,Αω)*ΒΨ*2)/ΒΨ;if(ю<0||Αι<ю){ю=Αι;Ακ=γ;}
continue;}if(γ.İ.LengthSquared()<1f) continue;if(Γη&&(ŝ-κ*Ž).Length()<=ΓΞ) continue;Vector3 ΓЏ=γ.İ-Ζψ;float Ζά=ΓЏ.LengthSquared();if(Ζά<0.25f) continue;float t=-Vector3.Dot(ŝ,ΓЏ)/Ζά;if(t<=0||t>5f||(ŝ+ΓЏ*t).Length()>=(ΑЄ?Ǖ:Ǖ*1.1f)) continue;if(ы>γ.λ||t<2f){ю=0;Ακ=γ;}
}bool Θί=ю>=0&&ю<0.5f;if(!Θί||ΖΨ||(Ακ!=null&&Ακ.ΓЊ&&Εό)){ΑЍ=false;return;}if(!ΑЍ){ΑЍ=true;ΔΡ=DateTime.Now;}double ΕЄ=(DateTime.Now-ΔΡ).TotalSeconds;if(ΕЄ>ΕΚ+Ηϋ) ΔΡ=DateTime.Now;else if(ΕЄ>ΕΚ){ю=-1;Ċ=Ь*ε*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>ȅ=new List<int>();String ΕІ="";void ΒΧ(){String Ħ=Н+"|"+б+"|"+(int) ĥ;if(Ħ==ΕІ) return;ΕІ=Ħ;ΖЄ(Н,б,ĥ,ȅ);}void ΖЄ(int w,int h,Ǒ ΔΖ,List<int>ΓΙ){ΓΙ.Clear();if(ΔΖ==Ǒ.ΑΑ){for(int k=0;k<w*h;k++){int Ş=k/w;ΓΙ.Add(Ş*w+(Ş%2==0?k-Ş*w:w-1-(k-Ş*w)));
}return;}int ΕЁ=(int) Math.Ceiling(w/2f),ΕЀ=(int) Math.Ceiling(h/2f);int ΘΫ=w/2,ΘΪ=h/2;int x=0,y=0,ΓΧ=0,ΓΨ=-1,Απ=Math.Max(w,h);for(int i=0;i<Απ*Απ;i++){if(-ΕЁ<x&&x<=ΘΫ&&-ΕЀ<y&&y<=ΘΪ) ΓΙ.Add((y-1+ΕЀ)*w+(x-1+ΕЁ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΧ;ΓΧ=-ΓΨ;ΓΨ=t;}
x+=ΓΧ;y+=ΓΨ;}}bool ΒΦ(){int n=Math.Max(0,Н*б);if(Л.Length==n) return false;Л=new String('0',n);Ѐ=-1;return true;}void ΔЋ(String γ){ΒΦ();if(γ.Length!=Л.Length) return;char[]h=Л.ToCharArray();for(int i=0;i<h.Length;i++) if(γ[i]=='1') h[i]='1';Л=new String(h);}bool ƾ(int Ȁ){return Ȁ>=0&&Ȁ<Л.Length&&Л[Ȁ]=='1';}
void Βύ(int Ȁ,bool х){if(Ȁ<0||Ȁ>=Л.Length) return;char[]h=Л.ToCharArray();h[Ȁ]=х?'1':'0';Л=new String(h);}bool Βε(int Ȁ,bool Θε){if(!trafficEnabled) return false;String Ħ=Ĺ();for(int i=0;i<н.Count;i++){ȩ t=н[i];if(t.Ș!=Ħ||t.Γι<0||(Θε&&t.λ>ы)) continue;if(t.Γι==Ȁ) return true;
}return false;}int ΖЌ(int Β){int n=Н*б;if(n<1) return Β;ΒΧ();if(ΒΦ()) for(int k=0;k<Β&&k<n;k++) Βύ(ȅ[k],true);if(Ѐ>=0&&(Β>=n||ȅ[Β]!=Ѐ)){Βύ(Ѐ,true);Ѐ=-1;}if(ΔЅ()){if(ǩ>=0&&!ƾ(ǩ)){Ā=false;Ѐ=ǩ;return ȅ.IndexOf(ǩ);}if(ǩ==-2){Ѐ=-1;Ā=Л.IndexOf('0')>=0;return Ā?-2:n;}
if(ǩ==-1&&Ѐ>=0&&!ƾ(Ѐ)){Ā=false;return Β;}Ѐ=-1;if(!Ā){Ā=true;Εδ=DateTime.Now;}if((DateTime.Now-Εδ).TotalSeconds<5) return-1;}Ā=false;if(Ѐ>=0&&!ƾ(Ѐ)&&!Βε(Ѐ,true)) return Β;Ѐ=-1;for(int k=0;k<n;k++){int c=ȅ[k];if(ƾ(c)||Βε(c,false)) continue;Ѐ=c;
return k;}return Л.IndexOf('0')>=0?-2:n;}const String Ηέ="[PAMJOB]";const String Ηϊ="[PAMTCC]";IMyBroadcastListener ΑΟ=null;DateTime Εγ=DateTime.MinValue;DateTime Εδ=DateTime.Now;int ǩ=-1;bool Ā=false;bool ΔЅ(){return trafficEnabled&&ǩ!=-3&&(DateTime.Now-Εγ).TotalSeconds<3;}
void Ηγ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ĺ()) return;Εγ=DateTime.Now;ΔЋ(f[2]);ǩ=-3;String Ɨ=ы+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) ǩ=int.Parse(e[i].Substring(Ɨ.Length));}Ľ Ης(int Β)
{ΒΧ();if(Β<0||Β>=ȅ.Count) return Ľ.Δα;Ш=ȅ[Β]%Н;Щ=ȅ[Β]/Н;return Ľ.Δς;}class ђ{public long λ;public Vector3 ζ,В,ώ;}List<ђ>Ў=new List<ђ>();bool Ȯ=false;Vector3 ΑΩ,ΑΨ,ΑΪ,ΑΫ;long ц=0;long ȇ=0;Ψ њ=null;bool Ɖ=false;Ψ ΒΣ(Vector3 ζ,Vector3 В,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ζ,В,ώ);
Ψ w=new Ψ(Θ);w.β=Vector3D.TransformNormal(ΑΩ,m);w.α=Vector3D.TransformNormal(ΑΨ,m);w.υ=Vector3D.TransformNormal(ΑΪ,m);w.Δ=ζ+(Vector3) Vector3D.TransformNormal(ΑΫ,m);w.δ=true;return w;}int Δξ(){Α s=Υ;if(s==Α.ж) return П(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.đ||s==Α.ǽ||s==Α.Đ||s==Α.ΑΖ||
s==Α.Ƹ||s==Α.ǒ) return 3;if(s==Α.Ǣ||s==Α.č||s==Α.Ŷ) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЍ(){if(Δξ()==0) return 0;if(ȑ(1)&&Ţ!=0) return Ţ;return њ!=null?ȇ:ц;}void ΔЈ(){if(Η==Ζ.Ϋ||!Θ.δ) return;IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector γ=И.OtherConnector;Vector3I Θΰ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==Θΰ){MatrixD Ŏ=MatrixD.Transpose(γ.WorldMatrix);ΑΩ=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,Ŏ);ΑΨ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,Ŏ);ΑΪ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,Ŏ);
ΑΫ=Vector3D.TransformNormal(И.GetPosition()-γ.GetPosition(),Ŏ);Ȯ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) ц=γ.EntityId;List<IMyShipConnector>ġ=new List<IMyShipConnector>();ω.GetBlocksOfType(ġ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ġ.Count>0){Ў.Clear();for(int i=0;i<ġ.Count;i++){ђ p=new ђ();p.λ=ġ[i].EntityId;p.ζ=ġ[i].GetPosition();p.В=ġ[i].WorldMatrix.Forward;p.ώ=ġ[i].WorldMatrix.Up;Ў.Add(p);}}њ=null;ȇ=0;if(γ.EntityId==ц||!Ȯ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==γ.EntityId){ȇ=Ў[i].λ;њ=ΒΣ(Ў[i].ζ,Ў[i].В,Ў[i].ώ);}}bool Βθ(long λ,int ΖΟ){if(λ==0) return false;for(int i=0;i<н.Count;i++){ȩ t=н[i];if(t.ά==λ&&(t.ƈ>ΖΟ||(t.ƈ==ΖΟ&&t.λ<ы))) return true;}return false;}bool ΒΝ(int Αϊ){if(Η==Ζ.Ϋ||!trafficEnabled){њ=null;ȇ=0;return true;}
long θ=њ!=null?ȇ:ц;if(!Βθ(θ,Αϊ)) return true;if(!Βθ(ц,Αϊ)){њ=null;ȇ=0;return true;}if(!Ȯ) return false;ђ ě=null;float ΕΫ=0;for(int i=0;i<Ў.Count;i++){ђ p=Ў[i];if(p.λ==ц||Βθ(p.λ,Αϊ)) continue;float d=Vector3.Distance(p.ζ,Θ.Δ);if(ě==null||d<ΕΫ){ě=p;ΕΫ=d;}
}if(ě==null) return false;ȇ=ě.λ;њ=ΒΣ(ě.ζ,ě.В,ě.ώ);return true;}String ΗΕ(){String s=(Ȯ?"1":"0")+":"+τ(ΑΩ)+":"+τ(ΑΨ)+":"+
τ(ΑΪ)+":"+τ(ΑΫ)+":"+ц;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].ζ)+","+τ(Ў[i].В)+","+τ(Ў[i].ώ);return s;}void ΗΔ(String s){Ў.Clear();Ȯ=false;ц=0;String[]f=s.Split(':');if(f.Length<6) return;Ȯ=f[0]=="1";ΑΩ=μ(f[1]);ΑΨ=μ(f[2]);ΑΪ=μ(f[3]);ΑΫ=μ(f[4]);long.TryParse(f[5],out ц);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ђ p=new ђ();if(!long.TryParse(e[0],out p.λ)) continue;p.ζ=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Ў.Add(p);}}const String ΒΟ="[PAMDCK]";const float Ιέ=120f;class Δο{public long Ї;public int Ɠ;public long Γκ;public bool Ιξ;public DateTime ΙЅ,Θζ;
}IMyBroadcastListener ΙЂ=null;List<Δο>ΙЃ=new List<Δο>();List<IMyShipConnector>ΚΘ=new List<IMyShipConnector>();List<IMySensorBlock>ΚΧ=new List<IMySensorBlock>();List<IMyCameraBlock>ΙА=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΚΦ=new List<MyDetectedEntityInfo>();long Ιρ=0;
int Ιπ=0;long Ιο=0;DateTime Ις=DateTime.Now;String ΙЁ="";int ΙЄ=0;IMyCubeGrid ΚΙ=null;IMyBroadcastListener ΑЇ=null;int Ȅ=0;DateTime Εο=DateTime.Now;DateTime Ζε=DateTime.MinValue;long Ζζ=0;int Ζη=0;long Ţ=0;bool Ƭ=false;int ΓΦ=0;List<Vector3>Ʈ=new List<Vector3>();List<float>Δί=new List<float>();
bool ΑΧ=false;bool ΑΦ=false;Vector3 Εξ=new Vector3();bool Ź(){return Ȅ!=0&&(DateTime.Now-Ζε).TotalSeconds<3;}bool ȑ(int Ɠ){return Ź()&&Ζζ==ы&&Ζη==Ɠ;}bool ΕΔ(){return Ź()||(DateTime.Now-Εο).TotalSeconds<3;}void Βϋ(int Ɠ){if(Ȅ==Ɠ) return;Ȅ=Ɠ;Εο=DateTime.Now;ΓΦ=3;
}void Βϊ(){if(Ȅ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΟ,"D;"+ы);Ȅ=0;Ƭ=false;}void ΗЂ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑЇ==null) ΑЇ=IGC.RegisterBroadcastListener(ΒΟ);int Ĝ=20;while(ΑЇ.HasPendingMessage&&Ĝ-->0){String Χ=ΑЇ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;
try{Ηδ(Χ);}catch{}}if(Υ!=Α.Ѕ){ΑΧ=false;ΑΦ=false;}if(Ȅ==1&&П(MyShipConnectorStatus.Connected)!=null) Βϊ();if(Ȅ==2&&!Ƭ&&Υ!=Α.ǒ&&
Vector3.Distance(ή,е().Δ)>ε*3) Βϊ();if(Ȅ!=0&&++ΓΦ>=3){ΓΦ=0;IGC.SendBroadcastMessage(ΒΟ,"R;"+ы+";"+Ȅ+";"+ц+";"+(Ȯ?1:0));}}void Ηδ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Εω=false;String[]ΕΝ=f[5].Split(',');for(int i=0;i<ΕΝ.Length;i++) if(ΕΝ[i]==ы.ToString()) Εω=true;
if(!Εω) return;Ζε=DateTime.Now;Ζζ=long.Parse(f[2]);Ζη=int.Parse(f[3]);Ţ=long.Parse(f[4]);Ʈ.Clear();Δί.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʈ.Add(μ(e[0]));Δί.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ερ=f[6].Split(':');List<ђ>Т=new List<ђ>();for(int i=0;i<Ερ.Length;i++){String[]e=Ερ[i].Split(',');if(e.Length<4) continue;ђ p=new ђ();p.λ=long.Parse(e[0]);p.ζ=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖЊ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;
Βϋ(1);if(!ΕΔ()) return true;if(!ȑ(1)){Λ="Waiting for dock clearance";return false;}ΒΖ();return true;}bool ΒΖ(){Ɖ=false;ђ ĩ=null;if(Ţ!=0&&Ţ!=ц&&Ȯ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==Ţ) ĩ=Ў[i];long ΒΒ=ĩ==null?0:ĩ.λ;if(ΒΒ==ȇ&&(ΒΒ==0)==(њ==null)) return false;ȇ=ΒΒ;њ=ĩ==null?null:ΒΣ(ĩ.ζ,ĩ.В,ĩ.ώ);return true;}bool ĸ(Vector3 p){for(int i=0;i<Ʈ.Count;i++) if(Vector3.Distance(p,Ʈ[i])<Δί[i]+ε*0.75f) return true;
return false;}bool ΗΡ(Vector3 ƅ,Vector3 ŗ){ΑΧ=false;if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return false;if(Ȅ!=1&&Vector3.Distance(ή,Θ.Δ)>dockRequestDistance) return false;Βϋ(1);if(!Ź()||ȑ(1)||Ʈ.Count==0) return false;if(!ĸ(ƅ)&&!ĸ(ŗ)&&!ĸ(ή)) return false;ΑΧ=true;return true;
}bool Δθ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;Βϋ(2);if(!ΕΔ()) return true;if(!ȑ(2)){Λ="Waiting for undock clearance";return false;}return true;}const float Ιγ=600f;class ΔЇ{public long λ;public int Ȁ=-1;public bool ΒΒ;public float Γί,Γή;public DateTime ΚΒ;}class ΔІ{public String Ħ;
public int w,h;public float ΚΟ,ΚΝ;public String х;public List<int>ΚΑ=new List<int>();public List<ΔЇ>ΚΔ=new List<ΔЇ>();}List<ΔІ>ΙЏ=new List<ΔІ>();IMyBroadcastListener ΙЎ=null;int ΙЍ=0;const float Ιν=8f;class ΕΙ{public long λ;public Vector3 ζ,İ,Ō;public float ΙϏ,Γί,Γή,Αι=-1;public int Ƒ;
public String Ș="";public int Αϊ;public DateTime ΑЌ;}class ΕΘ{public long a,b,ΚΥ;public DateTime Θζ;}List<ΕΙ>ΚΜ=new List<ΕΙ>();List<ΕΘ>ΚΚ=new List<ΕΘ>();int ΚΛ=0;List<Ȑ>ΙЈ=new List<Ȑ>();Ȑ ΙІ=null;Vector3 ΙЋ;DateTime ΙЊ;float ΙЉ=40;bool ΙЇ=false;void ΗΑ(string є){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}