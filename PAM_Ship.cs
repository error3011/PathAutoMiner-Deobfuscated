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
public enum Ζ{ΑΓ,Ŵ,ǎ,р,Ϊ}public enum Α{ж,ȋ,ĵ,Ǹ,Đ,Ѕ,Ю,
ȥ,ĳ,Ǡ,č,ĉ,ȡ,
ŵ,ǻ,Ǒ,Č,ΑΕ,Ʒ,
ΒЁ,ΑΔ,ū,Ƹ,ȧ}public enum Κ{з,Ķ,Ϗ,ΒΜ,Ƴ,ǁ,ǂ,į,İ}public enum п{None,Č,ж,Δθ}public enum Ω{Main,ΔЋ,ų,Ų,Βύ,Ʊ,
ΑЍ,Δω,Δϊ,Δχ,Ηξ,Ιγ,
Ια,Ιβ,Βψ,ΔЏ,Ǌ}public enum а{ǈ,ũ,Ηπ,Δό,Ϊ}public enum ǐ{ǌ,ΖЂ}public enum д{Ǉ,ΒΣ,ΔБ,ŭ}public enum Ѐ{Βζ,Δε,ΒΗ,ΒΝ,Ƶ,Βί,Ƽ}public enum ю{ΒΙ,ΒΑ,ΒΒ}public enum З{Ů,ΗΟ,Ċ}public enum ι{Βθ,Βμ,Βν,ΔЈ,ΔЇ,
Βη,ǅ,Ǆ,Βκ,
Βλ,Βι}public enum α{ǳ,ȍ,ǲ,ư,ΑА,ΑБ}public enum Ĺ{Δν,Δά,ΙΧ}public enum с{ǋ,Δλ,ΗΫ,Βε}public enum Ǵ{None,Βρ,Ηΰ,ΗΪ}public enum σ{ź,Ư,ƿ,ŭ,Βγ,ǀ,Βδ,Ηυ,Βξ}public enum ΙΫ{ΑΓ,Ιέ,Ιά,Ιί,ΙΨ,ΙΦ,Ιή}class Χ{public bool δ=false;public Vector3 Δ=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 ψ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float ΙЌ=0;public float[]Ǟ=null;public Χ(){}public Χ(Χ ί){δ=ί.δ;Δ=ί.Δ;β=ί.β;γ=ί.γ;ψ=ί.ψ;ν=ί.ν;
Ч=ί.Ч;Ǟ=ί.Ǟ;}public Χ(Vector3 Δ,Vector3 γ,Vector3 β,Vector3 ψ,Vector3 ν){this.Δ=Δ;this.β=β;this.γ=γ;this.ψ=ψ;this.Џ=0;this.ν=ν;}public void ΖϏ(List<IMyThrust>ΑΘ,List<string>ȇ){Ǟ=new float[ȇ.Count];for(int i=0;i<Ǟ.Length;i++)
Ǟ[i]=-1;for(int i=0;i<ΑΘ.Count;i++){string э=Δυ(ΑΘ[i]);int Β=ȇ.IndexOf(э);if(Β!=-1)
Ǟ[Β]=А(ΑΘ[i].MaxEffectiveThrust,ΑΘ[i].MaxThrust);}}}class Βπ{public Χ Њ=null;public List<Vector3>š=new List<Vector3>();public float Ɔ=0;public float ΔΟ=0;public float ľ=0;public float ΓΥ=0;public Vector3 ΑΫ=new Vector3();}class ĺ{public ι Ќ=ι.Βθ;public float ǔ=0;public float Ȯ=0;
public string Ǖ="";public string ǘ="";DateTime ΕЉ;public bool ΓЃ=false;private bool ΓΘ=false;public bool ΒΔ(bool Ȇ){if(Ȇ){Ȯ=0;ΓΘ=false;return false;}ΓΘ=true;return Ȯ>ǔ;}public void Ηα(){ΒΔ(true);ΓЃ=false;}public void ΗЁ(){if(ΓΘ)
if((DateTime.Now-ΕЉ).TotalSeconds>1){Ȯ++;ΕЉ=DateTime.Now;}}public bool Βά(){switch(Ќ){case ι.Βμ:return true;case ι.Βν:return true;}return false;}}class я{public String υ="";public String ς="";public int ǒ=0;public З Ŏ=З.Ċ;public я(String υ,String ς,int ǒ,З Ŏ){this.υ=υ;this.ς=ς;
this.ǒ=ǒ;this.Ŏ=Ŏ;}}class ΖЎ{public ΖЎ(Vector3 ζ,float ό){this.ζ=ζ;this.ό=ό;}public Vector3 ζ;public float ό;}class ĸ{public DateTime Θί;public DateTime ΕΩ;public String λ="";public String υ="";public String ΙЎ="";public String Μ="";public String ΙЈ="";public String ΙЁ="";public String ΓЁ="";
public Vector3 Δ=new Vector3();public List<я>Ъ=new List<я>();public Ζ Η=Ζ.ΑΓ;public Ǵ ȩ;public float ő;public float Ń;public int ј;public int Ιυ=0;public int ΙЋ=0;public bool ΙΩ(){return(DateTime.Now-Θί).TotalSeconds>10;}public bool ΗΣ(){if(ȩ!=Ǵ.Βρ) return false;return(DateTime.Now-ΕΩ).TotalSeconds>=4;
}public ĸ(String λ){this.λ=λ;}public Ǵ Ιΰ(String Ę,bool ΘЀ,bool ѓ,bool ΗЏ){if(Ę==""&&!ѓ) return Ǵ.None;if(ȩ==Ǵ.Βρ&&ΗΣ())
ȩ=Ǵ.ΗΪ;if(ΘЀ){ΓЁ=Ę;ȩ=Ǵ.Βρ;ΕΩ=DateTime.Now;return ȩ;}else if(ѓ){ΓЁ="";ȩ=Ǵ.None;}else if(ΓЁ==Ę){if(ΗЏ) ȩ=Ǵ.Ηΰ;return ȩ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 Αω=new Vector3();Vector3 ΰ=new Vector3();Vector3 Αή=new Vector3();Vector3 ƕ=new Vector3();Vector3 Ɖ=new Vector3();
DateTime ΕЈ=new DateTime();bool ƍ=true;int ΔΖ=0;bool ā=false;bool Αζ=false;bool Ƙ=true;bool Ζϊ=false;bool Ō=false;bool Αγ=false;float Ȝ=0;float ĩ=0;int Ą=0;int ƨ=0;int Αό=0;int ΒА=0;float Ő=0;float Ŀ=0;double Γχ=0;float ž=0;List<int>ŉ=new List<int>();List<int>ś=new List<int>();
Ζ Η=Ζ.ΑΓ;int ύ=0;float ǰ=0;float ǯ=0;float θ=0;float Ş=0,Ɵ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΙ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȉ=new List<IMyThrust>();List<IMyGyro>Αί=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>ă=new List<IMyReactor>();List<IMyConveyorSorter>ΑϏ=new List<IMyConveyorSorter>();List<IMyGasTank>ѕ=new List<IMyGasTank>();List<IMyTerminalBlock>Ż=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ł=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>і=new List<IMyTextPanel>();List<IMyTextSurface>ą=new List<IMyTextSurface>();List<IMyTextPanel>ƅ=new List<IMyTextPanel>();IMyTerminalBlock њ=null;Χ Θ=new Χ();Χ Γγ=new Χ();
Χ Γη=new Χ();bool ȉ=false;bool ţ=false;bool Ť=false;bool Αε=false;List<String>ȇ=new List<string>();List<Χ>Σ=new List<Χ>();List<Χ>ƭ=new List<Χ>();int ΓΝ=0;double Ζϋ=0;Χ Ε=new Χ();Vector3 Ǜ;Vector3 ș;Κ ή=Κ.з;ǐ Ģ=ǐ.ǌ;int М=0;int б=0;double ƒ=0;bool ΑΠ=false;ĺ ϊ=new ĺ();
ĺ ϋ=new ĺ();д л=д.Ǉ;bool ć=true;ǐ ȝ=ǐ.ǌ;int й=3;int и=3;int Ц=30;ю у=ю.ΒΙ;bool Ƞ=true;bool ŋ=true;bool ń=true;bool Ī=false;Ѐ Л=Ѐ.Βζ;bool ȟ=true;float Ǫ=90;float ч=20;float Ы=5;float ш=20;float Ǥ=0.70f;float ŧ=1.50f;float Ŧ=2.50f;float Į=0f;float ĝ=0f;Α Υ;
Κ ц;Α Ƣ;Βπ ε=null;int Ш=0;int Щ=0;int Й=0;int ȗ=0;int ħ=30;int У=0;bool Ђ=true;Vector3 щ;double К=0;bool Αϋ=false;int ΔΤ=0;int Ѓ=0;int ΒЈ=0;int ǫ=0;int ΖΜ=0;Vector3 ƚ=new Vector3();float к=0;float Э=0;int ĭ=0;Χ ΑЀ=null;float Γκ=0;float ΑΚ=0;float Р=0;float ĥ=0;
bool Γε=false;bool ī=false;bool Ʃ=false;bool Ȕ=false;bool Ȑ=false;DateTime ΘΜ=new DateTime();Χ ę=null;Χ Εή=null;float Ǽ=0;п ǥ;float ȕ=0;float Ĭ=0;String Γς="";float Αθ=0;float Ń=0;float ő=0;List<я>Ǘ=new List<я>();float Н=0;bool Ơ=false;bool Ζο=false;bool Ζά=false;
bool ΑЃ=false;bool Ζέ=false;float Γυ=0;float ΓЀ=0;Vector3 Ερ=new Vector3();Vector3 φ=new Vector3();Vector3 ΑЄ=new Vector3(1,1,1);float ΔΕ=1;Vector3 ƥ=new Vector3();bool Αδ=false;bool ΓΫ=false;bool Γο=false;bool ΕΙ=false;float ΑΞ=0;float ΑΗ=2;Vector3 Ζν;Vector3 ΔΔ;Vector3 ΔΓ;
float[,]ǟ=new float[3,2];Dictionary<String,float[,]>ѝ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζψ=new Dictionary<String,float>();int ΒЋ=0;Dictionary<String,float[]>Ț=new Dictionary<string,float[]>();float ΓЄ;bool ΑΡ=false;int ΑΙ=0;int Ȳ=0;int Αα=0;int ğ=0;
String Μ="";int[]ΖΔ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΖΕ=false;int Ας=0;int ј=0;Ω Ѝ=Ω.Main;int ΘЌ=0,ΘЍ=0,ΘЎ=0,ΘЏ=0;int Θό=0,Θύ=0;bool Ł=false;bool ΕΚ=false;String ΓЋ="";String ΒЏ="";IMyBroadcastListener ȑ=null;bool Επ=true;List<ĸ>Ιν=null;ĸ ΙЄ=null;
int Ιξ=0,Ιο=0;int Ιπ=0,Ιρ=0;int Ις=0,Ισ=0,Ιτ=0;String Θή="";Exception Ȗ=null;bool Αϊ=false;Κ ŝ=Κ.з;bool Εα=false;bool ΕЇ=true;bool ΓЉ=false;String Ζω="";bool Ƨ=false;bool ΓА=true;int Αβ=0;int[]ΙЇ=null;Vector3 ΒБ=new Vector3();bool Εχ=false;bool Ǐ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;
Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βέ(Me,controllerTag,true))
Η=Ζ.р;π(Ω.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЈ=Ηβ();if(Η==Ζ.ΑΓ){List<IMyShipDrill>Εμ=new List<IMyShipDrill>();List<IMyShipGrinder>Εψ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Εμ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Εψ,q=>q.CubeGrid==Me.CubeGrid);if(Εμ.Count>0)
{Η=Ζ.Ŵ;Μ="Miner mode enabled!";}else if(Εψ.Count>0){Η=Ζ.ǎ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϊ;ǹ(а.Ϊ);}}if(ΓЈ==с.ǋ) Ƙ=false;if(ΓЈ==с.Δλ) Μ="Data restore failed!";if(ΓЈ==с.Βε) Μ="New version, wipe data";}}void Main(string ђ,UpdateType ΙΖ){try{if(Ȗ!=null){if(ђ.Trim().ToUpper()=="RETRY")
{Ȗ=null;ƍ=true;Ą=0;if(Η!=Ζ.р) χ();Μ="Restarted after error";}else{ΗΚ();ΔЉ();return;}}Αζ=(ΙΖ&UpdateType.Update10)!=0;if(Αζ) ΔΖ++;ā=ΔΖ>=10;if(ā) ΔΖ=0;if(Αζ){ΑΙ++;if(ΑΙ>4) ΑΙ=0;Αό=(int) Math.Max(0,10-(DateTime.Now-ΕЈ).TotalSeconds);}if(ђ!="") Θή=ђ;if(Η!=Ζ.р)
Ηο(ђ);else
ΖЌ(ђ);ΖΕ=false;try{int Εί=Runtime.CurrentInstructionCount;float ƛ=А(Εί,Runtime.MaxInstructionCount);if(ƛ>0.90) Μ="Max. instructions >90%";if(ƛ>Ő) Ő=ƛ;if(ΑΡ){ŉ.Add(Εί);while(ŉ.Count>10) ŉ.RemoveAt(0);Ŀ=0;for(int i=0;i<ŉ.Count;i++) Ŀ+=ŉ[i];Ŀ=А(А(Ŀ,ŉ.Count),Runtime.MaxInstructionCount);
double Γλ=Runtime.LastRunTimeMs;if(Αγ&&Γλ>Γχ) Γχ=Γλ;ś.Add((int)(Γλ*1000f));while(ś.Count>10) ś.RemoveAt(0);ž=0;for(int i=0;i<ś.Count;i++) ž+=ś[i];ž=А(ž,ś.Count)/1000f;}}catch{Ő=0;}}catch(Exception e){Ȗ=e;}}void Ηο(string ђ){bool Αύ=false;String ΕΦ="";if(ύ<=1&&!ΔϏ(ђ))
ΔЊ(ђ);int Θι=50;while(ȑ!=null&&ȑ.HasPendingMessage&&Θι-->0){MyIGCMessage ρ=ȑ.AcceptMessage();String Ϋ=ρ.Data as String;if(Ϋ==null) continue;String ƞ="";if(ύ<=1&&Ηά(ref Ϋ,out ƞ,out ΕΦ)&&ƞ==BROADCAST_SELF_ID){ΔЊ(Ϋ);Αύ=true;break;}}bool ΕЂ=Αγ&&Υ==Α.ж&&!Ō&&!Αύ&&Ą==0&&!Αε;
if(ā&&Υ!=Α.ж) Αύ=true;if((Αζ&&!ΕЂ)||(ā&&ΕЂ)){if(Ą==0&&(Αό<=0||ƍ)){Ζϊ=ύ>0;ύ=0;Ą=1;Ǻ();Ηε();ΗΠ();ǵ("Scan 1");}else if(Ą==1){Ą=2;Ǻ();Ηζ();ǵ("Scan 2");}else if(Ą==2){Ą=0;Ǻ();Ηη();Ηθ();if(ύ<=1) ΔЃ();ǵ("Scan 3");ΕЈ=DateTime.Now;if(ύ<=1&&Ƙ)
Αω=ƺ(Φ,Φ.CenterOfMass);Ƙ=false;if(ƍ){ƍ=false;bool ΙΚ=ή==Κ.Ϗ;χ();if(ΕЇ&&resumeAfterRestart&&ΙΚ&&ύ<=1) ΓЉ=true;}ΕЇ=false;if(Ζϊ&&ύ==0) Μ="Setup complete";}else{if(ή==Κ.Ϗ&&Η!=Ζ.Ϊ){Ǻ();Ζό();ǵ("Inv balance");}Ǻ();switch(ƨ){case 0:Ηω();break;case 1:Ηύ();break;case 2:Ηψ();break;
case 3:ΗЄ();break;case 4:Ηό();break;case 5:ΖЃ();break;case 6:ΖЁ(Φ);break;}ǵ("Update: "+ƨ);ƨ++;if(ƨ>6){ƨ=0;Αγ=true;if(ŝ!=Κ.з){switch(ŝ){case Κ.į:ȣ();break;case Κ.İ:ȣ();break;case Κ.Ϗ:ȣ();break;case Κ.ǁ:ƹ();break;case Κ.ǂ:Ď();break;}ŝ=Κ.з;}}}if(!ƍ){if(!Ƿ(Φ,true)){Φ=null;
Ƙ=true;ύ=2;}if(ύ>=2&&Υ!=Α.ж)
χ();if(ύ<=1){ĩ=Φ.CalculateShipMass().PhysicalMass;Ȝ=(float) Φ.GetShipSpeed();ΰ=Ηχ(Φ,Αω);Αή=Φ.WorldMatrix.Forward;ƕ=Φ.WorldMatrix.Left;Ɖ=Φ.WorldMatrix.Down;ΗЀ();ΗЂ();Ηϊ();if(ΓЉ){ΓЉ=false;ȣ();if(Υ!=Α.ж) Μ="Job resumed after restart";}if(Υ!=Α.ж){Ō=false;ΔЎ(false);Δΰ(false);String ΘЅ=ΒΨ(Υ)+" "+(int) Υ;
Ǻ();Ηώ();ΒЀ(false);ǵ(ΘЅ);ΗЃ();ΗϏ();Ǻ();Ηϋ();ǵ("Thruster");Ǻ();ΕΕ();ǵ("Gyroscope");}else{if(Ō){if(ƾ()){Х(Ɖ,Αή,ƕ,0.25f,true);ΕΕ();Μ="Aligning to planet: "+Math.Round(ΑΞ-0.25f,2)+"°";if(Αδ) ΒΫ(true,true);}else ΒΫ(true,true);}}Ƨ=false;}}}Ǻ();ΔЉ();ǵ("Print");if(Αύ||ΒА<=0){Ǻ();Ζύ(ΕΦ);
ǵ("Broadcast");ΒА=4;}else if(ā) ΒА--;}void ΔЊ(string ђ){if(ђ=="") return;var Е=ђ.ToUpper().Split(' ');Е.DefaultIfEmpty("");var Ę=Е.ElementAtOrDefault(0);var Ė=Е.ElementAtOrDefault(1);var Ž=Е.ElementAtOrDefault(2);var ΕΝ=Е.ElementAtOrDefault(3);String ȳ="Invalid argument: "+ђ;bool ΔΞ=false;
switch(Ę){case "UP":this.ΔЅ(false);break;case "DOWN":this.ΔЄ(false);break;case "UPLOOP":this.ΔЅ(true);break;case "DOWNLOOP":this.ΔЄ(true);break;case "APPLY":this.Δή(true);break;case "MRES":ј=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.Βό();}break;case "PATH":{this.χ();this.Βό();Θ.δ=true;}break;
case "START":{this.χ();Βϋ();}break;case "ALIGN":{ΒΫ(!Ō,false);}break;case "CONT":{this.χ();this.ȣ();}break;case "JOBPOS":{this.χ();this.Ď();}break;case "HOMEPOS":{this.χ();this.ƹ();}break;case "FULL":{Ơ=true;}break;case "RESET":{Αϊ=true;ύ=2;}break;case "EXPORT":{ΗΒ();}break;
case "IMPORT":{ΗΝ(Ė=="JOB");}break;default:ΔΞ=true;break;}if(Η!=Ζ.Ϊ){switch(Ę){case "SHUTTLE":{ΖБ();}break;case "CFGS":{if(!ΖЊ(Ė,Ž,ΕΝ)) Μ=ȳ;}break;case "CFGB":{if(!ΖЉ(Ė,Ž)) Μ=ȳ;}break;case "CFGL":{if(!ď(ref Ǫ,true,σ.ƿ,Ė,"")||!ΖЋ(Ž))
Μ=ȳ;}break;case "CFGE":{if(!ď(ref Ы,true,σ.ǀ,Ė,"IG")||
!ď(ref ч,true,σ.Βγ,Ž,"IG")||
!ď(ref ш,true,σ.Βδ,ΕΝ,"IG"))
Μ=ȳ;}break;case "CFGA":{if(!ď(ref Ǥ,false,σ.Ư,Ė,"")) Μ=ȳ;}break;case "CFGW":{if(!ď(ref ŧ,false,σ.ź,Ė,"")||
!ď(ref Ŧ,false,σ.ź,Ž,""))
Μ=ȳ;}break;case "NEXT":{ΑЌ(false);}break;case "PREV":{ΑЌ(true);}break;default:if(ΔΞ) Μ=ȳ;break;}}else{switch(Ę){case "UNDOCK":{Ƨ=true;}break;default:if(ΔΞ) Μ=ȳ;break;}}}String ΗΖ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΖБ(){χ();Η=Ζ.Ϊ;ǹ(а.Ϊ);Ε.δ=false;Θ.δ=false;ξ=null;ο.Clear();ή=Κ.з;}void ΒΫ(bool Ј,bool ф){if(!Ј) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ј){Ō=false;ΕΑ();ǉ(false,0,0,0,0);return;}if(ƾ()) Ō=true;}bool ΖЊ(String ΙΜ,String ΘΦ,String ΘΗ)
{bool ΙΘ=ή==Κ.Ϗ;int w,h,d;if(int.TryParse(ΙΜ,out w)&&int.TryParse(ΘΦ,out h)&&int.TryParse(ΘΗ,out d)){this.χ();й=w;и=h;Ц=d;ē(false);Ź(false,false);if(ΙΘ) ȣ();return true;}return false;}bool ΖЋ(String ΕΜ){if(ΕΜ=="ON"){Ƞ=true;return true;}if(ΕΜ=="OFF"){Ƞ=false;return true;}
return false;}bool ΖЉ(String Ελ,String ΑΟ){bool ƙ=true;if(Ελ=="HOME") ć=true;else if(Ελ=="STOP") ć=false;else ƙ=false;if(ΑΟ=="HOME") л=д.Ǉ;else if(ΑΟ=="STOP") л=д.ΔБ;else if(ΑΟ=="JOB") л=д.ΒΣ;else if(ΑΟ=="IG") л=д.ŭ;else ƙ=false;return ƙ;}void π(Ω Γώ){ΖΔ[(int) Ѝ]=ј;
ј=ΖΔ[(int) Γώ];if(Γώ==Ω.Ηξ) ј=0;Ѝ=Γώ;if(Η!=Ζ.р)
Ē(Ѝ==Ω.ų,false,0,0);ΖΕ=true;}void ΔЅ(bool Γπ){if(ј>0) ј--;else if(Γπ) ј=Ας-1;}void ΔЄ(bool Γπ){if(ј<Ας-1) ј++;else if(Γπ) ј=0;}String ǹ(а ρ){switch(ρ){case а.ǈ:Μ="Job is running";break;case а.ũ:Μ="Connector not ready!";break;case а.Ηπ:Μ="Ship modified, path outdated!";break;
case а.Δό:Μ="Interrupted by player!";break;case а.Ϊ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΙ(ǐ ζ){switch(ζ){case ǐ.ǌ:return "Top-Left";case ǐ.ΖЂ:return "Center";default:return "";}}String ΗΔ(ю Ɨ){switch(Ɨ){case ю.ΒΑ:return "Auto"+(Η==Ζ.Ŵ?" (Ore)":"");case ю.ΒΒ:return "Auto (+Stone)";
case ю.ΒΙ:return "Default";default:return "";}}String ΒΦ(Κ Ф){switch(Ф){case Κ.з:return "No job";case Κ.Ķ:return "Job paused";case Κ.Ϗ:return "Job active";case Κ.į:return "Job active";case Κ.İ:return "Job active";case Κ.ΒΜ:return "Job done";case Κ.Ƴ:return "Job changed";case Κ.ǁ:return "Move home";
case Κ.ǂ:return "Move to job";}return "";}String Δπ(д ΗЎ){switch(ΗЎ){case д.Ǉ:return "Return home";case д.ΒΣ:return "Fly to job pos";case д.ΔБ:return "Stop";case д.ŭ:return "Ignore";}return "";}String ΗΕ(Ѐ Ɨ){switch(Ɨ){case Ѐ.Βζ:return "Off";case Ѐ.ΒΝ:return "Drop pos (Stone) ";case Ѐ.Ƶ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δε:return "Cur. pos (Stone)";case Ѐ.ΒΗ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βί:return "In motion (Stone)";case Ѐ.Ƽ:return "In motion (Sto.+Ice)";}return "";}String Δξ(п Ф){switch(Ф){case п.None:return "No batteries";case п.Č:return "Charging";case п.Δθ:return "Discharging";}return "";}String ΒΨ(Α Ф)
{String Ιϋ=Η==Ζ.Ϊ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ȋ:return "Flying to XY position";case Α.ĵ:return Η==Ζ.ǎ?"Grinding":"Mining";case Α.Ǹ:return "Returning";case Α.ū:return "Flying to drop pos";case Α.Đ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȥ:return "Flying to path";case Α.ĳ:return "Flying to job position";case Α.Ǡ:return "Approaching dock";case Α.č:return "Docking";case Α.ĉ:return "Aligning";case Α.ȡ:return "Aligning";case Α.ŵ:return "Retry docking";case Α.ǻ:return "Unloading";case Α.ȧ:return Ζω;
case Α.Ǒ:return "Undocking";case Α.Č:return "Charging batteries";case Α.ΑΕ:return "Waiting for uranium";case Α.Ʒ:return "Filling up hydrogen";case Α.ΒЁ:return "Waiting for ejection";case Α.ΑΔ:return "Waiting for ejection";case Α.Ƹ:return "Flying to drop pos";}return "";}String Δφ(ι Ќ){switch(Ќ)
{case ι.Βθ:return "On \"Undock\" command";case ι.Βμ:return "On player entered cockpit";case ι.ΔЈ:return "On ship is full";case ι.ΔЇ:return "On ship is empty";case ι.Βν:return "On time delay";case ι.ǅ:return "On batteries empty(<25%)";case ι.Ǆ:return "On batteries empty(=0%)";case ι.Βη:return "On batteries full";
case ι.Βλ:return "On hydrogen empty(<25%)";case ι.Βι:return "On hydrogen empty(=0%)";case ι.Βκ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΧ,int ȱ,bool Ξ,String Ɣ){Ας+=1;if(ΖΧ==ȱ) Ɣ=">"+Ɣ+(ΑΙ>=2?" .":"");else Ɣ=" "+Ɣ;Γ+=Ɣ+"\n";return ΖΧ==ȱ&&Ξ;}String Δή(bool Ξ)
{int Β=0;int Ο=ј;Ας=0;String ΖΪ="———————————————\n";String Ţ="--------------------------------------------\n";String Γ="";Γ+=ΒΦ(ή)+" | "+(Θ.δ?"Ready to dock":"No dock")+"\n";Γ+=ΖΪ;double Ǭ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ω.Main){bool Ā=Η==Ζ.Ϊ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βό();
if(Η==Ζ.Ŵ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ω.ų);if(Η==Ζ.ǎ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ω.ų);if(Η==Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ω.Βψ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȣ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƹ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ď();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ā) π(Ω.Ǌ);else π(Ω.Ʊ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ω.Δω);if(Η!=Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ω.Δχ);}else if(Ѝ==Ω.ų){double ΙΛ=Math.Round(й*ǰ,1);double ΘΥ=Math.Round(и*ǯ,1);String Є="";if(Π(ref Є,Ο,Β++,Ξ," Start new job!")) Βϋ();if(Π(ref Є,Ο,Β++,Ξ," Change current job")){Ź(false,false);π(Ω.Main);}if(Π(ref Є,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΛ+"m)")){ı(ref й,5,20,1);ē(true);}
if(Π(ref Є,Ο,Β++,Ξ," Width -")){ı(ref й,-5,20,-1);ē(true);}if(Π(ref Є,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΘΥ+"m)")){ı(ref и,5,20,1);ē(true);}if(Π(ref Є,Ο,Β++,Ξ," Height -")){ı(ref и,-5,20,-1);ē(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth + ("+(у==ю.ΒΙ?"Depth":"Min")+": "+Ц+"m)")){ı(ref Ц,5,50,2);ē(true);}
if(Π(ref Є,Ο,Β++,Ξ," Depth -")){ı(ref Ц,-5,50,-2);ē(true);}if(Π(ref Є,Ο,Β++,Ξ," Depth mode: "+ΗΔ(у))){у=Ȥ(у);}if(Π(ref Є,Ο,Β++,Ξ," Start pos: "+ΗΙ(ȝ))){ȝ=Ȥ(ȝ);}if(Η==Ζ.ǎ&&у==ю.ΒΒ) у=Ȥ(у);Γ+=ΔЌ(8,Є,Ο,ref Θό);}else if(Ѝ==Ω.Βψ){float[]Εγ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.ΔЏ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δφ(ϊ.Ќ))) ϊ.Ќ=Ȥ(ϊ.Ќ);if(!ϊ.Βά()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΥ((int) ϊ.ǔ))) ϊ.ǔ=Δζ(ϊ.ǔ,Εγ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δφ(ϋ.Ќ))) ϋ.Ќ=Ȥ(ϋ.Ќ);
if(!ϋ.Βά()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΥ((int) ϋ.ǔ))) ϋ.ǔ=Δζ(ϋ.ǔ,Εγ);}else if(Ѝ==Ω.ΔЏ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βϋ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Βψ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǖ!=""?ϊ.Ǖ:"-"))) ϊ.Ǖ=ǃ(ref ΘЌ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǘ!=""?ϊ.ǘ:"-"))) ϊ.ǘ=ǃ(ref ΘЎ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǖ!=""?ϋ.Ǖ:"-"))) ϋ.Ǖ=ǃ(ref ΘЍ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǘ!=""?ϋ.ǘ:"-"))) ϋ.ǘ=ǃ(ref ΘЏ);
}else if(Ѝ==Ω.Ų){String ΕЍ=Ǫ+" %";if(ā) Ȳ++;if(Ȳ>1){Ȳ=0;Αα++;if(Αα>1) Αα=0;}bool[]ΓЍ=new bool[]{ă.Count==0,ǥ==п.None,ѕ.Count==0};int Ζτ=0;while(true){Ζτ++;ğ++;if(ğ>ΓЍ.Length-1) ğ=0;if(Ζτ>=ΓЍ.Length) break;if(!ΓЍ[ğ]) break;}bool Ā=Η==Ζ.Ϊ;if(!Ā&&Ƞ&&Н!=-1&&Αα==0)
ΕЍ=Н<1000000?Math.Round(Н)+" Kg":Math.Round(Н/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ā) π(Ω.Ʊ);else π(Ω.Ǌ);if(!Ā){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЌ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƨ=true;Γ+=Ţ;if(!Ā) Γ+="Progress: "+Math.Round(ƒ,1)+" %\n";Γ+="State: "+ΒΨ(Υ)+" "+Ǭ+"m \n";Γ+="Load: "+Αθ+" % Max: "+ΕЍ+" \n";if(ğ==0) Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(Ĭ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ğ==1) Γ+="Battery: "+(ǥ==п.None?Δξ(ǥ):Ǽ+"% "+(ч==-1||Ā?"":" Min: "+ч+" %"))+"\n";if(ğ==2) Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(ȕ,1)+"% "+(ш==-1||Ā?"":" Min: "+ш+" %"))+"\n";}else if(Ѝ==Ω.Ʊ){String Є="";if(Π(ref Є,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ω.Ų);else π(Ω.Main);}
if(Π(ref Є,Ο,Β++,Ξ," Max load: "+Ǫ+"%")) г(ref Ǫ,Ǫ<=80?-10:-5,σ.ƿ,false);if(Π(ref Є,Ο,Β++,Ξ," Weight limit: "+(Ƞ?"On":"Off"))) Ƞ=!Ƞ;if(Π(ref Є,Ο,Β++,Ξ," Ejection: "+ΗΕ(Л))) Л=Ȥ(Л);if(Π(ref Є,Ο,Β++,Ξ," Toggle sorters: "+(Ī?"On":"Off"))){Ī=!Ī;if(Ī) ǣ(ΓА);}
if(Π(ref Є,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Є,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǀ,true);if(Π(ref Є,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) г(ref ч,-5,σ.Βγ,true);if(Π(ref Є,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-10,σ.Βδ,true);
if(Π(ref Є,Ο,Β++,Ξ," When done: "+(ć?"Return home":"Stop"))) ć=!ć;if(Π(ref Є,Ο,Β++,Ξ," On damage: "+Δπ(л))) л=Ȥ(л);if(Π(ref Є,Ο,Β++,Ξ," Advanced...")) π(Ω.ΑЍ);Γ+=ΔЌ(8,Є,Ο,ref Θύ);}else if(Ѝ==Ω.ΑЍ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ω.Ų);else π(Ω.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.ǎ?" Grinder":" Drill")+" inv. balancing: "+(ŋ?"On":"Off"))) ŋ=!ŋ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.ǎ?" grinders":" drills")+": "+(ń?"Fwd + Bwd":"Fwd"))) ń=!ń;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ŧ+"m/s")) г(ref ŧ,0.5f,σ.ź,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ŧ+"m/s")) г(ref Ŧ,0.5f,σ.ź,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) г(ref Ǥ,0.1f,σ.Ư,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Į*100f+"%")) Δβ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ĝ*100f+"%")) Δβ(false,0.05f);
}else if(Ѝ==Ω.Ǌ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Κ.Ϗ) π(Ω.Ų);else π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǫ+"%")) г(ref Ǫ,Ǫ<=80?-10:-5,σ.ƿ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǀ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δπ(л))) л=Ȥ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) г(ref Ǥ,0.1f,σ.Ư,false);
}else if(Ѝ==Ω.ΔЋ){double Θΰ=0;if(Σ.Count>0) Θΰ=Vector3.Distance(Σ.Last().Δ,ΰ);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΒ();if(Η!=Ζ.Ϊ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȉ?"Use old home":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȉ?"Use old connector":(Θ.δ?"Was set! ":"none ")))) ȉ=!ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(ţ?"Use old connector":(Ε.δ?"Was set! ":"none ")))) ţ=!ţ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ť?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ť=!Ť;Γ+=Ţ;Γ+="Wp spacing: "+Math.Round(Ζϋ)+"m\n";}else if(Ѝ==Ω.Βύ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();π(Ω.Main);}
Γ+=Ţ;Γ+="State: "+ΒΨ(Υ)+" \n";Γ+="Speed: "+Math.Round(Ȝ,1)+"m/s\n";Γ+="Target dist: "+Ǭ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΤ+"\n";}else if(Ѝ==Ω.Δω){List<IMyTerminalBlock>Ě=Δρ();if(ā) Ȳ++;if(Ȳ>=Ě.Count) Ȳ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.Δϊ);
Γ+=Ţ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αθ,1)+"% "+Math.Round(Ń,1)+" / "+Math.Round(ő,1)+"\n";Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(Ĭ,1)+"Kg "+Γς)+"\n";Γ+="Battery: "+(ǥ==п.None?"":Ǽ+"% ")+Δξ(ǥ)+"\n";Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(ȕ,1)+"% ")+"\n";
Γ+="Damage: "+(Ě.Count==0?"None":""+(Ȳ+1)+"/"+Ě.Count+" "+Ě[Ȳ].CustomName)+"\n";}else if(Ѝ==Ω.Δϊ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=Ţ;Γ+="Next scan: "+Αό+"s\n";Γ+="Drill area: "+Math.Round(ǰ,1)+"x"+Math.Round(ǯ,1)+"m Ship: "+Math.Round(Ş,1)+"x"+Math.Round(Ɵ,1)+"m ("+Math.Round(θ,1)+"m)\n";
Γ+="Broadcast: "+(Ł?"Online - "+ΓЋ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ő*100f,1)+"% \n";}else if(Ѝ==Ω.Δχ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=Ţ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+Αό+"s\n";if(Αϊ) Γ="Recompile script now";int Θα=Γ.Split('\n').Length;for(int i=Θα;i<=10;i++) Γ+="\n";Γ+=ΖΪ;Γ+="Last: "+Μ+"\n";
return Γ;}void Δβ(bool Θά,float ќ){χ();Ź(true,false);if(Θά) г(ref Į,ќ,σ.Βξ,false);else г(ref ĝ,ќ,σ.Βξ,false);Δα();Ē(true,true,0,0);}float Δζ(float η,float[]ΑЊ){float έ=ΑЊ[0];for(int i=ΑЊ.Length-1;i>=0;i--)
if(η<ΑЊ[i]) έ=ΑЊ[i];return έ;}String ǃ(ref int ȱ){String έ="";if(ȱ>=ΔΙ.Count) ȱ=-1;if(ȱ>=0) έ=ΔΙ[ȱ].CustomName;ȱ++;return έ;}void ΑΑ(string υ){if(ή!=Κ.Ϗ) return;if(υ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(υ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+υ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void ı(ref int value,int ƀ,int Ȟ,int ΓЎ){if(ƀ==0) return;if(value<Ȟ&&ΓЎ>0||value<=Ȟ&&ΓЎ<0){value+=ΓЎ;return;}int ΗІ=Math.Abs(ƀ);int ΒЂ=0;int Γω=1;while(true){ΒЂ+=Γω*ΗІ*10;if(ƀ<0&&value-Ȟ<=ΒЂ) break;if(ƀ>0&&value-Ȟ<ΒЂ) break;
Γω++;}value+=Γω*ƀ;}void ē(bool ΙЍ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ē(Ѝ==Ω.ų,false,0,0);}bool ď(ref float value,bool Θω,σ ς,String Ġ,String ΘΩ){if(Ġ=="") return false;float Ŗ=-1;bool ΕϏ=false;if(Ġ.ToUpper()==ΘΩ) ΕϏ=true;else if(!float.TryParse(Ġ,out Ŗ)) return false;
else Ŗ=Math.Max(0,Ŗ);if(Θω) Ŗ=(float) Math.Round(Ŗ);г(ref value,Ŗ,ς,ΕϏ,false);return true;}void г(ref float value,float ќ,σ ς,bool Ʀ){г(ref value,value+ќ,ς,Ʀ,true);}void г(ref float value,float Œ,σ ς,bool Ʀ,bool ΙΟ){float ǚ=0,Ǚ=0;if(ς==σ.ź){ǚ=0.5f;Ǚ=10f;}if(ς==σ.Ư){ǚ=0.1f;Ǚ=1f;}
if(ς==σ.ŭ){ǚ=50f;Ǚ=100f;}if(ς==σ.Βγ){ǚ=5f;Ǚ=30f;}if(ς==σ.ǀ){ǚ=1f;Ǚ=25f;}if(ς==σ.ƿ){ǚ=10f;Ǚ=95f;}if(ς==σ.Βδ){ǚ=10f;Ǚ=90f;}if(ς==σ.Ηυ){ǚ=10f;Ǚ=1800;}if(ς==σ.Βξ){ǚ=0.0f;Ǚ=0.75f;}if(Œ==-1&&Ʀ){value=-1;return;}if(value==-1) Ʀ=false;
bool ΖΟ=Œ<ǚ||Œ>Ǚ;if(ΖΟ&&ΙΟ){if(Œ<value) value=Ǚ;else if(Œ>value) value=ǚ;}else value=Œ;if(ΖΟ&&Ʀ) value=-1;else value=Math.Max(ǚ,Math.Min(value,Ǚ));value=(float) Math.Round(value,2);}void ΑЌ(bool Θτ){if(Θτ) Й=Math.Max(0,Й-1);else Й++;ΒΟ();ΒΞ();for(int k=0;k<ȃ.Count&&k<С.Length;k++) Βφ(ȃ[k],k<Й);
Ё=-1;ΒЀ(true);}T Ȥ<T>(T η){int Β=Array.IndexOf(Enum.GetValues(η.GetType()),η);Β++;if(Β>=ΗΑ(η)) Β=0;return(T) Enum.GetValues(η.GetType()).GetValue(Β);}int ΗΑ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βό(){ƭ.Clear();for(int i=0;i<Σ.Count;i++) ƭ.Add(Σ[i]);Σ.Clear();
Αε=true;Γγ=new Χ(Θ);Γη=new Χ(Ε);Θ.δ=false;if(Η==Ζ.Ϊ) Ε.δ=false;for(int i=0;i<ѝ.Count;i++)
if(!ȇ.Contains(ѝ.Keys.ElementAt(i)))
ȇ.Add(ѝ.Keys.ElementAt(i));ȉ=false;ţ=false;Ť=false;π(Ω.ΔЋ);}void ΕΒ(){if(ȉ) Θ=Γγ;if(ţ) Ε=Γη;if(Ť){Σ.Clear();for(int i=0;i<ƭ.Count;i++) Σ.Add(ƭ[i]);}Αε=false;χ();π(Ω.Main);}void ΗЀ(){if(!Αε) return;if(Υ!=Α.ж){ΕΒ();return;}if(!Γγ.δ) ȉ=false;if(!Γη.δ) ţ=false;if(ƭ.Count<=1) Ť=false;
IMyShipConnector И=О(MyShipConnectorStatus.Connectable);if(И==null) И=О(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(Ȝ,2)<=0.20) ΓΝ++;else ΓΝ=0;if(ΓΝ>=5){if(Η==Ζ.Ϊ&&(Θ.δ||ȉ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.γ=Φ.WorldMatrix.Forward;Ε.ψ=Φ.WorldMatrix.Left;
Ε.β=Φ.WorldMatrix.Down;Ε.ν=Φ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.δ=true;Ε.Ч=И.Position;}else{Θ.γ=Φ.WorldMatrix.Forward;Θ.ψ=Φ.WorldMatrix.Left;Θ.β=Φ.WorldMatrix.Down;Θ.ν=Φ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.δ=true;Θ.Ч=И.Position;}}}double Γι=-1;if(Σ.Count>0) Γι=Vector3.Distance(ΰ,Σ.Last().Δ);
double ΘЃ=Math.Max(1.5,Math.Pow(Ȝ/100.0,2));double ΖΖ=Math.Max(Ȝ*ΘЃ,2);Ζϋ=ΖΖ;if((Γι==-1)||Γι>=ΖΖ){Χ Ψ=new Χ(ΰ,Αή,Ɖ,ƕ,Φ.GetNaturalGravity());Ψ.ΖϏ(Ȉ,ȇ);Σ.Add(Ψ);}}Χ е(){if(Η!=Ζ.Ϊ) return љ!=null?љ:Θ;return Εή;}void ķ(Χ ά,Κ Ф){Εή=ά;if(ή==Κ.Ϗ) ц=Ф;}bool Ƕ(Χ ά,float ό,bool ΙΗ,out Vector3 ζ)
{if(ΙΗ){Vector3I ΓΪ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(ΓΪ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ζ=new Vector3();return false;}Vector3 ΘΓ=Я(Φ,Τ.FatBlock.GetPosition()-ΰ);Vector3 ΘΒ=Я(Φ,Τ.FatBlock.WorldMatrix.Forward);ζ=ά.Δ-ΒϏ(ά.γ,ά.β*-1,ΘΓ)-ΒϏ(ά.γ,ά.β*-1,ΘΒ)*ό;
return true;}else{ζ=ά.Δ;return true;}}Vector3 Ȍ(int ΗБ,int Ś,bool Θχ){if(!Θχ&&Εχ) return ΒБ;float ΘΑ=((М-1f)/2f)-ΗБ;float Θϊ=((б-1f)/2f)-Ś;ΒБ=Ε.Δ+Ε.ψ*ΘΑ*ǰ+ș*-1*Θϊ*ǯ;Εχ=true;return ΒБ;}Vector3 ΒΧ(Vector3 ΘЇ,float ΓΚ){return ΘЇ+(Ǜ*ΓΚ);}α ΒΚ(){float Б=-1;α έ=α.ǳ;
if(Η!=Ζ.Ϊ){ΔЃ();if(ή!=Κ.з){Vector3 Љ=đ(Ǜ,ș*-1,ΰ-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(М*ǰ)/2f&&Math.Abs(Љ.Y)<=(float)(б*ǯ)/2f){if(Љ.Z<=-1&&Љ.Z>=-ħ*2) return α.ư;if(Љ.Z>-1&&Љ.Z<θ*2) return α.ǳ;}if(Ȣ(Ε.Δ,ref Б)) έ=α.ǳ;}if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=α.ǲ;for(int i=0;i<Σ.Count;i++)
if(Ȣ(Σ[i].Δ,ref Б)) έ=α.ȍ;if(Vector3.Distance(ΰ,Θ.Δ)<dockDist*θ) έ=α.ǲ;if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null)
έ=α.ǲ;}}else{Vector3 ζ=new Vector3();IMyShipConnector Ȁ=О(MyShipConnectorStatus.Connected);if(Θ.δ){if(Ȣ(Θ.Δ,ref Б)) έ=α.ǲ;if(Ƕ(Θ,dockDist,true,out ζ))
if(Ȣ(ζ,ref Б)) έ=α.ǲ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return α.ΑА;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*θ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*θ)
if(Ȣ(Σ[i].Δ,ref Б)) έ=α.ȍ;if(Ε.δ){if(Ȣ(Ε.Δ,ref Б)) έ=α.ǳ;if(Ƕ(Ε,dockDist,true,out ζ))
if(Ȣ(ζ,ref Б)) έ=α.ǳ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return α.ΑБ;}}return έ;}bool Ȣ(Vector3 ζ,ref float Б){float ό=Vector3.Distance(ζ,ΰ);if(ό<Б||Б==-1){Б=ό;return true;}return false;}void Βϋ(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϊ){ȣ();return;}Ε.Δ=ΰ;Ε.ν=Φ.GetNaturalGravity();Ε.γ=Αή;Ε.β=Ɖ;Ε.ψ=ƕ;
Ǜ=њ.WorldMatrix.Forward;ș=Ε.β;if(Ǜ==Φ.WorldMatrix.Down) ș=Φ.WorldMatrix.Backward;Ź(true,true);Ν(Α.ȋ);Βϊ();}void Ź(bool ǖ,bool Θψ){if(ή==Κ.з&&!ǖ) return;bool Θκ=ǖ||ή==Κ.ΒΜ||М!=й||б!=и||Ģ!=ȝ;if(Θκ){if(ή!=Κ.з){ή=Κ.Ƴ;Ȍ(Ш,Щ,Θψ);Μ="Job changed, lost progress";}Ģ=ȝ;
М=й;б=и;Щ=0;Ш=0;С="";Ё=-1;ĥ=0;ȗ=0;Р=0;Й=0;ΒЀ(true);}}void Βς(){П(ΰ,0);ё(Ȉ,true);}void Βϊ(){ǹ(а.ǈ);Βς();ŷ(Ȅ,false);ǣ(ΓА);ή=Κ.Ϗ;Δΰ(true);ц=ή;π(Ω.Ų);Ηδ();ΑΠ=true;Αβ=0;for(int i=Ż.Count-1;i>=0;i--)
if(ů(Ż[i],false)) Αβ++;if(Αβ>0) Μ="Started with damage";}void χ(){if(ή==Κ.Ϗ){ή=Κ.Ķ;Μ="Job paused";}Ν(Α.ж);ц=ή;ǉ(false,0,0,0,0);Βώ();Δέ(new Vector3(),false);ΕΑ();Βυ(ChargeMode.Auto);Βχ(false);ΔЎ(true);Ȏ(Α.ж);Ē(false,false,0,0);ё(ο,false);ё(т,true);ǣ(true);Αϋ=false;ƴ();ƈ=false;
Βσ();ΑΠ=false;Ơ=false;Ƨ=false;if(Ѝ!=Ω.Main&&Ѝ!=Ω.Ʊ&&Ѝ!=Ω.ΑЍ&&Ѝ!=Ω.Ǌ)
π(Ω.Main);}void ȣ(){α Д=ΒΚ();if(Η==Ζ.Ϊ){if(!Ε.δ||!Θ.δ) return;Βϊ();bool ΓΨ=Vector3.Distance(ΰ,Θ.Δ)<Vector3.Distance(ΰ,Ε.Δ);if(ŝ==Κ.į) ΓΨ=true;if(ŝ==Κ.İ) ΓΨ=false;if(ΓΨ){ķ(Θ,Κ.į);switch(Д){case α.ΑА:Ν(Α.ȧ);break;case α.ȍ:Ν(Α.Ю);break;case α.ǲ:Ν(Α.Ǡ);break;default:Ν(Α.Ǒ);break;
}}else{ķ(Ε,Κ.İ);switch(Д){case α.ΑБ:Ν(Α.ȧ);break;case α.ǳ:Ν(Α.Ǡ);break;case α.ȍ:Ν(Α.Ю);break;default:Ν(Α.Ǒ);break;}}}else{if(ή!=Κ.Ķ&&ή!=Κ.Ƴ) return;bool ΙΙ=ή==Κ.Ƴ;Βϊ();bool Γέ=Ű(false)&&Θ.δ;switch(Д){case α.ǳ:Ν(Γέ?Α.ȥ:Α.ȋ);break;case α.ȍ:Ν(Γέ?Α.Ѕ:Α.Ю);break;case α.ǲ:Ν(Γέ?Α.Ǡ:Α.ǻ);break;
case α.ư:{if(ȗ!=Й||ΙΙ) Ν(Α.Ǹ);else Ν(Α.ĵ);}break;default:break;}}}void Ď(){if(ή==Κ.з&&!Θ.δ) return;if(Η==Ζ.Ϊ&&(!Ε.δ||!Θ.δ)) return;Μ="Move to job";α Д=ΒΚ();if(Η==Ζ.Ϊ){ķ(Ε,Κ.İ);switch(Д){case α.ǳ:Ν(Α.Ǡ);break;case α.ȍ:Ν(Α.Ю);break;case α.ΑБ:return;default:Ν(Α.Ǒ);break;
}Ȏ(Α.ȧ);}else{switch(Д){case α.ǳ:Ν(Α.Ю);break;case α.ȍ:Ν(Α.Ю);break;case α.ǲ:Ν(Α.ǻ);break;case α.ư:Ν(Α.Ǹ);break;default:break;}if(ή==Κ.з) Ȏ(Α.Ю);else Ȏ(Α.ȡ);Αϋ=true;}Βς();π(Ω.Βύ);ŷ(Ȅ,false);ц=Κ.ǂ;}void ƹ(){if(!Θ.δ) return;Μ="Move home";α Д=ΒΚ();if(Η==Ζ.Ϊ){ķ(Θ,Κ.į);switch(Д)
{case α.ȍ:Ν(Α.Ѕ);break;case α.ǲ:Ν(Α.Ǡ);break;case α.ΑА:return;default:Ν(Α.Ǒ);break;}Ȏ(Α.ȧ);}else{if(О(MyShipConnectorStatus.Connected)!=null) return;if(О(MyShipConnectorStatus.Connectable)!=null){Ν(Α.č);Ȏ(Α.ǻ);return;}switch(Д){case α.ǳ:Ν(Α.ȥ);break;case α.ȍ:Ν(Α.Ѕ);break;case α.ǲ:Ν(Α.Ѕ);break;
case α.ư:Ν(Α.Đ);break;default:break;}Ȏ(Α.ǻ);}Βς();π(Ω.Βύ);ŷ(Ȅ,false);ц=Κ.ǁ;}void Ν(Α Γϊ){if(Γϊ==Α.ж) Ƣ=Α.ж;if(Ƣ!=Α.ж&&Υ==Ƣ&&Γϊ!=Ƣ){χ();return;}Ђ=true;Υ=Γϊ;}void Ȏ(Α Ф){this.Ƣ=Ф;}void Ηώ(){if(Υ==Α.ȋ){if(Ђ){Ѓ=0;if(ȗ!=Й) ĥ=0;ȗ=Й;}if(Ѓ==0){int ΓΓ=ΖІ(Й);if(ΓΓ<0)
{if(!ΓΗ){ΓΗ=true;ΕΫ=ΰ;}П(ΕΫ,0);Μ="Waiting for the controller";return;}ΓΗ=false;if(ΓΓ!=Й){Й=ΓΓ;ĥ=0;ȗ=Й;}Ĺ έ=Ηλ(Й);if(έ==Ĺ.Δά){ή=Κ.ΒΜ;bool ΖΞ=С.IndexOf('0')>=0;Μ=ΖΞ?"No free hole left for this ship":"Job done";if((ć||ΖΞ)&&Θ.δ){Ν(Α.ȥ);Ȏ(Α.ǻ);ц=Κ.ǁ;}
else{Ν(Α.ĳ);Ȏ(Α.ȡ);ц=Κ.ǂ;}return;}if(έ==Ĺ.Δν){Ѓ=1;ё(ο,true);щ=Ȍ(Ш,Щ,true);П(щ,10);Х(Ε.β,Ε.γ,Ε.ψ,false);}}else{if(Ё>=0&&(ΔЀ()?ǧ>=0&&ǧ!=Ё:Βή(Ё,true))){Ѓ=0;return;}if(К<wpReachedDist){Ν(Α.ĵ);return;}}}if(Υ==Α.ĵ){if(Ђ){ё(ο,true);ǣ(false);щ=Ȍ(Ш,Щ,false);
П(ΒΧ(щ,0),0);Х(Ε.β,Ε.γ,Ε.ψ,false);Ѓ=1;к=0;Э=0;ΑΚ=0;Γκ=-1;ħ=Ц;Γε=true;}if(!ΒΕ()){Ν(Α.Đ);return;}if(Ű(true)){ΖΜ=Ȋ("","ORE",З.Ċ);if((Л==Ѐ.ΒΝ||Л==Ѐ.Ƶ||
Л==Ѐ.Βί||Л==Ѐ.Ƽ)&&Η!=Ζ.ǎ)
Ν(Α.ū);else if((Л==Ѐ.Δε||Л==Ѐ.ΒΗ)&&Η!=Ζ.ǎ)
Ν(Α.ΒЁ);else
Ν(Α.Đ);return;}Р=Vector3.Distance(ΰ,щ);if(Р>ĥ){ĥ=Р;Γε=false;}if(Η==Ζ.ǎ&&Δς()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ѓ>0&&Ѓ<Э){if(Р>к){if(Э>150) к=Р;else к=(float) Math.Ceiling(Р);Ѓ=1;}else Ѓ++;}else{if(Ѓ>0){Μ="Ship stuck! Retrying";к=Р;Ѓ=0;Ē(false,true,0,θ*sensorRange);}П(ΒΧ(щ,Math.Max(0,к-θ)),ΒΪ(false));if(К<=wpReachedDist/2){Ѓ=1;к=0;}
return;}Ē(false,true,θ*sensorRange,0);Vector3 ΗЈ=щ+Ǜ*Р;bool ΕЃ=false;if(Vector3.Distance(ΗЈ,ΰ)>0.3f){Vector3 ΘΔ=щ+Ǜ*(Р+0.1f);П(ΘΔ,4);ΕЃ=true;}else{float ǜ=ΒΪ(true);Vector3 Εβ=ΒΧ(щ,Math.Max(Ц+1,Р+1));Ǣ(true,false,false,Εβ,Εβ-щ,ǜ,ǜ);}bool ΓΛ=false;if(у==ю.ΒΒ||у==ю.ΒΑ)
{if(!ΕЃ){float Γό=0;foreach(IMyTerminalBlock Ǯ in ο)
Γό+=ΖЍ(Ǯ,"","",у==ю.ΒΑ?new string[]{"STONE"}:null);if(Γό>Γκ||Р<Ц||Γε){ΒЈ=0;ΑΚ=Р;ħ=(int)(Math.Max(ħ,ΑΚ)+θ/2);}else{ΓΛ=Р-ΑΚ>2&&ΒЈ>=20;ΒЈ++;}Γκ=Γό;}}else ΓΛ=Р>=ħ;if(ȗ!=Й){Ν(Α.Ǹ);Р=0;return;}if(ΓΛ){Й++;Ν(Α.Ǹ);Р=0;return;}}if(Υ==Α.Ǹ||Υ==Α.Đ||Υ==Α.ū)
{if(Ђ){щ=Ȍ(Ш,Щ,false);Х(Ε.β,Ε.γ,Ε.ψ,false);ё(ο,ń);ǣ(false);к=Vector3.Distance(ΰ,щ);Ē(false,true,0,θ*sensorRange);}П(щ,ΒΪ(false));if(Vector3.Distance(ΰ,щ)>=к+5){ё(ο,false);ǣ(true);Μ="Can´t return!";}if(К<wpReachedDist){if(Υ==Α.Ǹ&&Αϋ) Ν(Α.ĳ);if(Υ==Α.Ǹ) Ν(Α.ȋ);
if(Υ==Α.ū) Ν(Α.Ƹ);if(Υ==Α.Đ){if(Θ.δ) Ν(Α.ȥ);else{χ();Ď();Μ="Can´t return, no dock found";}}return;}}if(Υ==Α.Ѕ||Υ==Α.Ю){if(Υ==Α.Ю&&ή==Κ.Ϗ&&Η!=Ζ.Ϊ){if(!ΒΕ()||Ű(true)){Ν(Α.Ѕ);return;}}bool ΓІ=false;bool ΑЈ=false;bool ΕΞ=false;float ľ=0;bool ΖΛ=false;Χ Ψ=null;
if(Ђ){if(Υ==Α.Ѕ||Η==Ζ.Ϊ){if(Η!=Ζ.Ϊ){if(ȏ(1)) ΑЏ();else{љ=null;ȅ=0;ƈ=!ΒΖ(1);}}Χ ά=е();ε=new Βπ();ε.Њ=ά;ε.Ɔ=followPathDock*θ;ε.ΔΟ=useDockDirectionDist*θ;ε.ľ=10;ε.š.Add(ά.Δ);Vector3 Ľ=new Vector3();if(Ƕ(ά,dockDist*θ,true,out Ľ))
ε.š.Add(Ľ);else ε.Ɔ*=1.5f;if(Η==Ζ.Ϊ){if(ά==Θ) ε.ΑΫ=Ε.Δ;if(ά==Ε) ε.ΑΫ=Θ.Δ;ε.ΓΥ=dockDist*θ*1.1f;}}else if(Υ==Α.Ю){ε=new Βπ();ε.Њ=Ε;ε.Ɔ=followPathJob*θ;ε.ΔΟ=useJobDirectionDist*θ;ε.ľ=10;ε.ΑΫ=Θ.Δ;ε.ΓΥ=dockDist*θ*1.1f;ε.š.Add(Ε.Δ);if(ή==Κ.з){if(!Θ.δ||Σ.Count==0){χ();return;}
float ΘΚ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΛ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΚ<ΘΛ) ε.Њ=Σ.Last();else ε.Њ=Σ.First();}}ƚ=new Vector3();ΖΛ=!ΗΤ(ΰ);ё(ο,false);ǣ(true);У=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ε.ΑΫ)<=ε.ΓΥ) continue;
double ό=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǧ==-1||ό<Ǧ){У=i;Ǧ=ό;}}ǫ=ΗΘ(ε.Њ.Δ,У);ę=null;}if(Υ==Α.Ѕ&&Η!=Ζ.Ϊ&&!Ђ&&!Ÿ()){Χ ΒЌ=е();ƈ=!ΒΖ(1);if(е()!=ΒЌ){Ν(Α.Ѕ);return;}}if(Υ==Α.Ѕ&&Η!=Ζ.Ϊ&&!Ђ&&ȏ(1)&&ΑЏ()){Ν(Α.Ѕ);return;}bool ΕЁ=false;if(Υ==Α.Ѕ&&!Ђ&&У>=0&&У<Σ.Count)
{Vector3 Ƅ=Σ[У].Δ+Δσ(У,ǫ);int Γϋ=У+ǫ;Vector3 œ=ǫ!=0&&Γϋ>=0&&Γϋ<Σ.Count?Σ[Γϋ].Δ:ε.Њ.Δ;if(ΗΛ(Ƅ,œ)){if(!ΑΤ){ΑΤ=true;Εη=Ĵ(Ƅ)?ΰ:Ƅ;}П(Εη,0);Х(ε.Њ.β,ε.Њ.γ,ε.Њ.ψ,false);Μ="Waiting for dock clearance";return;}ΑΤ=false;ΕЁ=ȏ(1)&&(Ĵ(ΰ)||Ĵ(œ));}ΖЀ(Σ,ǫ,ε.š,ε.Ɔ,Ђ,ref Ѓ);
for(int i=0;i<ε.š.Count;i++){float ό=Vector3.Distance(ΰ,ε.š[i]);if(ό<=ε.Ɔ) ΓІ=true;if(ό<=ε.ΔΟ) ΑЈ=true;}if(ΕЁ) ΑЈ=true;if(ΑЈ) ľ=ε.ľ;float ΙЏ=ę!=null?ę.Џ:Ȝ;float ΙΞ=(float) Math.Max(Ȝ*0.1f*θ,wpReachedDist);if((К<ΙΞ)||Ђ){if(!Ђ) У+=ǫ;if(ǫ==0||У>Σ.Count-1||У<0)
ΓІ=true;else{ΔΤ=ǫ>0?Σ.Count-1-У:У;Ψ=Σ[У];ę=Ψ;if(У>=1&&У<Σ.Count-1)
ƚ=Ψ.Δ-Σ[У-ǫ].Δ;else ę=null;φ=Ψ.Δ+Δσ(У,ǫ);ΕΞ=true;}}if(ΑЈ)
Х(ε.Њ.β,ε.Њ.γ,ε.Њ.ψ,false);else if(ΖΛ)
ΑЎ(ε.Њ.β,10,true);else if(ΕΞ&&Ψ!=null){if(ǫ>0) Х(Ψ.β,Ψ.γ,Ψ.ψ,90,false);else Х(Ψ.β,-Ψ.γ,-Ψ.ψ,90,false);}Ǣ(true,false,true,φ,ƚ,ę==null?0:ę.Џ,ľ);if(ΓІ){ΔΤ=0;if(Υ==Α.Ѕ&&!ΖЄ()){П(φ,0);return;}if(Υ==Α.Ѕ&&ƈ){П(φ,0);Μ="Waiting for a free dock";return;}if(Υ==Α.Ѕ||Η==Ζ.Ϊ){Ν(Α.Ǡ);return;}
if(Υ==Α.Ю&&Αϋ){Ν(Α.ĳ);return;}if(Υ==Α.Ю){Ν(Α.ȋ);return;}}}if(Υ==Α.Ǡ||Υ==Α.ŵ){if(Η!=Ζ.Ϊ&&Ÿ()){if(!ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϊ){Χ ΒЌ=е();if(!ΒΖ(2)||е()!=ΒЌ){Ν(Α.Ѕ);return;}}Χ ά=е();if(Ђ){if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ũ);χ();return;}
П(φ,0);ΑЎ(ά.β,90,true);}if(К<followPathDock*θ&&К!=-1){П(φ,10);Х(ά.β,ά.γ,ά.ψ,false);}if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null){Ν(Α.č);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.ĉ);return;}}if(Υ==Α.ĉ||Υ==Α.ȡ){if(Ђ)
{if(Υ==Α.ĉ){Χ ά=е();if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ũ);χ();return;}Ǣ(true,true,false,φ,0);Х(ά.β,ά.γ,ά.ψ,10,false);}if(Υ==Α.ȡ){Х(Ε.β,Ε.γ,Ε.ψ,0.5f,false);φ=Ε.Δ;Ǣ(true,true,false,φ,0);}}if(Αδ){ǉ(false,0,0,0,0);if(Υ==Α.ĉ) Ν(Α.č);if(Υ==Α.ȡ) χ();return;}}if(Υ==Α.č)
{if(О(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.ǻ);return;}Χ ά=е();if(Ђ){Э=0;ΘΜ=DateTime.Now;Ѓ=0;Х(ά.β,ά.γ,ά.ψ,false);}Vector3I ΓΪ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock ΕΪ=Me.CubeGrid.GetCubeBlock(ΓΪ);float Ει=dockingSpeed;float Θδ=dockingSpeed*5;
float ΕΛ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ƕ(ά,0,true,out φ)||!Ƕ(ά,ΕΛ,true,out ƚ)||
ΕΪ==null||!ΕΪ.FatBlock.IsFunctional){ǹ(а.ũ);χ();return;}if(Э==1||(Vector3.Distance(ΰ,φ)<=ΕΛ*1.1f&&!Ђ))
Э=1;else{Vector3 ΘА=Я(Φ,ƚ-ΰ);Vector3 ΓΩ=Я(Φ,Φ.GetNaturalGravity());float Θϋ=Ʋ(ΘА,ΓΩ,null);Ει=Math.Min(Θδ,Θϋ);}Ǣ(true,false,false,φ,φ-ΰ,dockingSpeed,Ει);if(Ђ) к=(float) К;IMyShipConnector ΓΖ=О(MyShipConnectorStatus.Connectable);if(ΓΖ!=null){Ǣ(false,false,false,φ,0);
if(Ѓ>0) Ѓ=0;Ѓ--;if(Ѓ<-5){ΓΖ.Connect();if(ΓΖ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.ǻ);Βώ();ŷ(Ȅ,true);return;}}}else{float Εζ=(float) Math.Round(К,1);if(Εζ<к){Ѓ=-1;к=Εζ;}else Ѓ++;if(Ѓ>20){Ν(Α.ŵ);return;}}}if(Υ==Α.ǻ||Υ==Α.ȧ||
Υ==Α.ΑΕ||Υ==Α.Ʒ||Υ==Α.Č){bool ΒІ=false,ΒЇ=false;if(Η==Ζ.Ϊ){if(е()==Θ) ΒІ=true;else if(е()==Ε) ΒЇ=true;}if(Ђ){Ơ=false;if(О(MyShipConnectorStatus.Connected)==null){Ν(Α.Ǒ);return;}Βώ();if(ΒІ) ΑΑ(ϊ.Ǖ);if(ΒЇ) ΑΑ(ϋ.Ǖ);ī=false;Ȑ=false;Ȕ=false;Ʃ=false;}if(О(MyShipConnectorStatus.Connected)==null){χ();ǹ(а.Δό);return;}
if(ή!=Κ.Ϗ||ч==-1||ǥ==п.None) Ȑ=true;else if(Ǽ>=100f) Ȑ=true;else if(Ǽ<=99f) Ȑ=false;if(ή!=Κ.Ϗ||ш==-1||ѕ.Count==0) Ȕ=true;else if(ȕ>=100f) Ȕ=true;else if(ȕ<=99) Ȕ=false;if(ή!=Κ.Ϗ||Ы==-1||ă.Count==0) Ʃ=true;else Ʃ=Ĭ>=Ы;ĺ о=null;if(ΒІ) о=ϊ;
if(ΒЇ) о=ϋ;if(о!=null&&(о.Ќ==ι.ǅ||о.Ќ==ι.Ǆ)) Ȑ=true;if(о!=null&&(о.Ќ==ι.Βη)) if(!ī) Ȑ=false;if(о!=null&&(о.Ќ==ι.Βλ||о.Ќ==ι.Βι)) Ȕ=true;if(о!=null&&(о.Ќ==ι.Βκ)) if(!ī) Ȕ=false;if(ā){ChargeMode ΕΨ=Ȑ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ќ==ι.Ǆ||о.Ќ==ι.ǅ))
ΕΨ=ChargeMode.Discharge;Βυ(ΕΨ);Βχ(!Ȕ);}if(!ī){if(Η==Ζ.Ϊ)
ī=ή!=Κ.Ϗ||ΖЅ(Ђ,true)||Ƨ;else
ī=ή!=Κ.Ϗ||Δύ();}else{if(!Ȑ) Ν(Α.Č);if(!Ȕ) Ν(Α.Ʒ);if(!Ʃ) Ν(Α.ΑΕ);Ђ=false;}if(ī&&Ȑ&&Ȕ&&Ʃ){Βυ(ChargeMode.Auto);Βχ(false);if(ή==Κ.Ϗ){if(Η==Ζ.Ϊ){if(е()==Θ) ΑΑ(ϊ.ǘ);else if(е()==Ε) ΑΑ(ϋ.ǘ);if(е()==Θ) ķ(Ε,Κ.İ);else ķ(Θ,Κ.į);}}Ν(Α.Ǒ);return;}}if(Υ==Α.Ǒ){if(Ƭ){if(!Δγ()) return;
Ƭ=false;Ν(Α.Ǒ);return;}if(Ђ){if(!Δγ()){Ƭ=true;return;}IMyShipConnector Ȁ=О(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Ю);return;}IMyShipConnector ί=Ȁ.OtherConnector;ΔЍ(Ȁ,false);ŷ(Ȅ,false);Χ ά=null;Χ Γβ=Η==Ζ.Ϊ?Θ:е();if(Vector3.Distance(Ȁ.GetPosition(),Γβ.Δ)<5f&&Θ.δ) ά=Γβ;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.δ) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*θ,true,out φ)){ǹ(а.ũ);χ();return;}П(φ,5);Х(ά.β,ά.γ,ά.ψ,false);}else П(ΰ+ί.WorldMatrix.Forward*dockDist*θ,5);if(ή==Κ.Ϗ) ǹ(а.ǈ);}if(К<wpReachedDist){ё(т,true);Ν(Α.Ю);return;}}if(Υ==Α.ĳ)
{if(Ђ){ǣ(true);ё(ο,false);φ=Ε.Δ;П(φ,20);Х(Ε.β,Ε.γ,Ε.ψ,false);}if(К<wpReachedDist/2){Ν(Α.ȡ);return;}}if(Υ==Α.ȥ){if(Ђ){ǣ(true);ё(ο,false);int Б=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǧ==-1||ό<Ǧ){Б=i;Ǧ=ό;}}
if(Б==-1){Ν(Α.Ѕ);return;}φ=Σ[Б].Δ;П(φ,10);Х(Ε.β,Ε.γ,Ε.ψ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Υ==Α.Ƹ){bool ф=false;if(Ђ){ǣ(true);if((Л==Ѐ.ΒΝ||Л==Ѐ.Ƶ)&&ƾ()&&
Ĳ(Ǜ,Φ.GetNaturalGravity())<25&&М>=2&&б>=2){Vector3 Ɗ=ΰ;if(Ш>0&&Щ<б-1) Ɗ=Ȍ(Ш-1,Щ+1,true);else if(Ш<М-1&&Щ<б-1) Ɗ=Ȍ(Ш+1,Щ+1,true);else if(Ш<М-1&&Щ>0) Ɗ=Ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ɗ=Ȍ(Ш-1,Щ-1,true);else ф=true;if(!ф) П(Ɗ,10);
}else ф=true;}if(К<wpReachedDist/2) ф=true;if(ф){Ν(Α.ΑΔ);return;}}if(Υ==Α.ΒЁ||Υ==Α.ΑΔ){if(Ђ){Ǣ(true,true,false,ΰ,0);ё(ο,false);ǣ(true);Ѓ=-1;Э=Л==Ѐ.Βί||Л==Ѐ.Ƽ?0:-1;}bool ě=!ΒΕ();int ΑЂ=Ȋ("STONE","ORE",З.Ċ);if(Л==Ѐ.ΒΗ||Л==Ѐ.Ƽ||Л==Ѐ.Ƶ)
ΑЂ+=Ȋ("ICE","ORE",З.Ċ);bool ΘΤ=ΑЂ>0;bool ΓΤ=false;if(Э>=0){float Ļ=(float) Math.Sin(Δη(Э))*ǰ/3f;float ΗЊ=(float) Math.Cos(Δη(Э))*ǯ/3f;Vector3 ΗА=Ȍ(Ш,Щ,true)+ΒϏ(Ǜ,ș*-1,new Vector3(Ļ,ΗЊ,0));П(ΗА,0.3f);if(К<Math.Min(ǰ,ǯ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ѓ==-1||ΑЂ<Ѓ){Ѓ=ΑЂ;к=0;}
else{к++;if(к>50) ΓΤ=true;}if(!ΘΤ||ě||ΓΤ){if(!ě){int ΙΔ=Ȋ("","ORE",З.Ċ);if(Ű(true)) ě=true;else if(100-(А(ΙΔ,ΖΜ)*100)<minEjection) ě=true;else ǹ(а.ǈ);}if(ΓΤ&&ě) Μ="Ejection failed";if(Υ==Α.ΑΔ){if(ě){if(Θ.δ) Ν(Α.ȥ);else{χ();Ď();Μ="Can´t return, no dock found";}}
else Ν(Α.ȋ);}else if(ě) Ν(Α.Đ);else Ν(Α.ĵ);return;}}Ђ=false;}void ΖЀ(List<Χ>Ĉ,int κ,List<Vector3>Ƥ,float Ǭ,bool Ȇ,ref int Ǔ){if(Ȇ){for(int i=0;i<Ĉ.Count;i++) Ĉ[i].Џ=0;Ǔ=-1;return;}if(κ==0) return;int ќ=κ*-1;if(Ǔ==-1) Ǔ=ќ>0?1:Ĉ.Count-2;int ΕЅ=0;while(Ǔ>=1&&Ǔ<Ĉ.Count-1)
{if(ΕЅ>50) return;ΕЅ++;try{if((ќ<0&&Ǔ>=1)||(ќ>0&&Ǔ<=Ĉ.Count-2)){Χ η=Ĉ[Ǔ];bool ΖΙ=false;for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(η.Δ,Ƥ[j])<=Ǭ){ΖΙ=true;break;}if(!ΖΙ){Χ Αχ=Ĉ[Ǔ-ќ];Χ œ=Ĉ[Ǔ+ќ];Vector3 ΘБ=η.Δ-œ.Δ;Vector3 ΔΚ=Αχ.Δ-η.Δ;Vector3 Θυ=η.Δ+Vector3.Normalize(ΘБ)*ΔΚ.Length();Vector3 ΘΙ=Αχ.Δ-Θυ;Vector3 Αι=đ(κ>0?η.γ:η.γ*-1,η.β*-1,ΘΙ);Vector3 Θγ=đ(κ>0?η.γ:η.γ*-1,η.β*-1,ΔΚ);
Vector3 Αλ=đ(κ>0?η.γ:η.γ*-1,η.β*-1,η.ν);η.Џ=(float) Math.Sqrt(Math.Pow(Αχ.Џ,2)+Math.Pow(Ʋ(-Θγ,Αλ,η),2));for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(Αχ.Δ,Ƥ[j])<=Ǭ){Vector3 Αμ=đ(κ>0?η.γ:η.γ*-1,η.β*-1,Ƥ[j]-η.Δ);float ΘЉ=Ʋ(-Αμ,Αλ,η);η.Џ=Math.Min(η.Џ,ΘЉ)/2f;}if(Αι.Length()==0) Αι=new Vector3(0,0,1);Vector3 Θβ=đ(η.γ,η.β*-1,η.ν);float ΘЋ=Ŭ(-Αι,Θβ,η);float ΑΖ=А(ΘЋ,ĩ);float ΔΘ=(float) Math.Sqrt(А(Αι.Length()*1.0f,0.5f*ΑΖ));
η.Џ=Math.Min(η.Џ,(ΔΚ.Length()/ΔΘ)*Ǥ);}}}catch{return;}Ǔ+=ќ;}Ǔ=-1;}void ΒЀ(bool Ȇ){if(Ȇ){ƒ=0;return;}if(Η==Ζ.Ϊ) return;float ф=Й*Math.Max(1,Ц);if(ȗ==Й) ф+=Math.Min(Ц,Р);float ΙΓ=М*б*Math.Max(1,Ц);ƒ=Math.Max(ƒ,(float) Math.Min(ф/ΙΓ*100.0,100));}MyDetectedEntityType Δς()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΪ(bool ǩ){if(Η==Ζ.ǎ&&Δς()==MyDetectedEntityType.None&&!ů(ξ,true))
return fastSpeed;else return ǩ?ŧ:Ŧ;}void Ηψ(){float ΖΒ=0,Εΰ=0,Ġ=0,Γύ=0;for(int i=0;i<Ȩ.Count;i++){IMyBatteryBlock ſ=Ȩ[i];if(!Ƿ(ſ,true)) continue;ΖΒ+=ſ.MaxStoredPower;Εΰ+=ſ.CurrentStoredPower;Ġ+=ſ.CurrentInput;Γύ+=ſ.CurrentOutput;}Ǽ=(float) Math.Round(А(Εΰ,ΖΒ)*100,1);
if(Ġ>=Γύ) ǥ=п.Č;else ǥ=п.Δθ;if(Ġ==0&&Γύ==0||Ǽ==100.0) ǥ=п.ж;if(Ȩ.Count==0) ǥ=п.None;}void Ηό(){float Εο=0;for(int i=0;i<ѕ.Count;i++){IMyGasTank Ζλ=ѕ[i];if(!Ƿ(Ζλ,true)) continue;Εο+=(float) Ζλ.FilledRatio;}ȕ=А(Εο,ѕ.Count)*100f;}void ΗЄ(){Ĭ=0;try{for(int i=0;i<ă.Count;i++)
{IMyReactor ΓЇ=ă[i];if(!Ƿ(ΓЇ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЇ.GetInventory(0).GetItems(Ъ);float ǒ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
ǒ+=(float) Г.Amount;}if(ǒ<Ĭ||i==0){Ĭ=ǒ;Γς=ΓЇ.CustomName;}}}catch(Exception e){Ȗ=e;}}void Ηω(){ő=0;Ń=0;try{for(int i=0;i<ł.Count;i++){IMyTerminalBlock Τ=ł[i];if(!Ƿ(Τ,true)) continue;Ń+=(float) Τ.GetInventory(0).CurrentVolume;ő+=(float) Τ.GetInventory(0).MaxVolume;
}Αθ=(float) Math.Min(Math.Round(А(Ń,ő)*100,1),100.0);}catch(Exception e){Ȗ=e;}}void Ηύ(){try{Ǘ.Clear();for(int i=0;i<ł.Count;i++){IMyTerminalBlock Τ=ł[i];if(!Ƿ(Τ,true)) continue;ΔΫ(Τ,З.Ů);}if(Л!=Ѐ.Βζ){for(int i=0;i<ĕ.Count;i++){IMyTerminalBlock Τ=ĕ[i];if(!Ƿ(Τ,true)) continue;
ΔΫ(Τ,З.ΗΟ);}}Ηρ(Ǘ);}catch(Exception e){Ȗ=e;}}void ΖЃ(){if(ΑΠ){if(Δρ().Count>Αβ){ΑΠ=false;if(л!=д.ŭ){χ();if(л==д.ΒΣ) Ď();if(л==д.Ǉ)
if(Θ.δ) ƹ();else Ď();}Μ="Damage detected";}}}bool ΒΕ(){if(!Αγ) return true;if(ή==Κ.Ϗ){if(ч>0&&ǥ!=п.None)
if(Ǽ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&ă.Count>0)
if(Ĭ<=Ы){Μ="Low fuel: "+Γς;return false;}if(ш>0&&ѕ.Count>0)
if(ȕ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δρ(){List<IMyTerminalBlock>Ě=new List<IMyTerminalBlock>();for(int i=0;i<Ż.Count;i++){IMyTerminalBlock Τ=Ż[i];if(ů(Τ,false)) Ě.Add(Τ);}return Ě;}bool ů(IMyTerminalBlock Τ,bool ΔΑ){return(!Ƿ(Τ,ΔΑ)||!Τ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Τ,bool ΔΑ){if(Τ==null) return false;try{IMyCubeBlock Εφ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΔΑ) return Εφ==Τ;else return Εφ.GetType()==Τ.GetType();}catch{return false;}}float ΖЍ(IMyTerminalBlock Τ,String э,String Ζβ,String[]Ǩ){float ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ŋ=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Ŋ.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(Ǩ!=null&&(Ǩ.Contains(Г.Type.TypeId.ToUpper())||Ǩ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((э==""||Г.Type.TypeId.ToUpper()==э)&&(Ζβ==""||Г.Type.SubtypeId.ToUpper()==Ζβ))
ȁ+=(float) Г.Amount;}}return ȁ;}я ΗΓ(String υ,String ς,З Д,bool ΘΕ){υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){я Г=Ǘ[i];if(Г.υ.ToUpper()==υ&&Г.ς.ToUpper()==ς&&(Г.Ŏ==Д||Д==З.Ċ))
return Г;}я έ=null;if(ΘΕ){έ=new я(υ,ς,0,Д);Ǘ.Add(έ);}return έ;}int Ȋ(String υ,String ς,З Д){return Ȋ(υ,ς,Д,null);}int Ȋ(String υ,String ς,З Д,String[]Ǩ){int ȁ=0;υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){я Г=Ǘ[i];if(Ǩ!=null&&Ǩ.Contains(Г.υ.ToUpper())) continue;
if((υ==""||Г.υ.ToUpper()==υ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.Ŏ==Д||Д==З.Ċ))
ȁ+=Г.ǒ;}return ȁ;}void ΔΫ(IMyTerminalBlock Τ,З Д){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΓ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).ǒ+=(int) Ъ[j].Amount;}}void Ηρ(List<я>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){я a=Т[j];я b=Т[j+1];if(a.ǒ<b.ǒ) Т.Move(j,j+1);}}bool Ű(bool ΙІ){if(Ƞ&&Η!=Ζ.Ϊ)
if(Н!=-1&&ĩ>=Н){Μ="Ship too heavy";return true;}if(Αθ>=Ǫ||Ơ){Ơ=false;Μ="Ship is full";return true;}return false;}bool Δύ(){String[]Ǩ=null;if(!ȟ) Ǩ=new string[]{"ICE"};if(Η==Ζ.Ŵ) return Ȋ("","ORE",З.Ů,Ǩ)==0;if(Η==Ζ.ǎ) return Ȋ("","COMPONENT",З.Ů,Ǩ)==0;else return Ȋ("","",З.Ů,Ǩ)==0;
}bool ΗΥ(){List<IMyCockpit>ΓΕ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΕ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΕ.Count;i++)
if(ΓΕ[i].IsUnderControl) return true;return false;}bool ΖЅ(bool Ȇ,bool Αώ){IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δδ(ϊ,Ȇ,Αώ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δδ(ϋ,Ȇ,Αώ);return false;}bool Δδ(ĺ ǿ,bool Ȇ,bool Αώ){if(Ȇ) ǿ.Ηα();ǿ.ΗЁ();bool н=false;String ь="";switch(ǿ.Ќ){case ι.Βθ:{ь="Waiting for command";н=false;break;}case ι.ΔЈ:{ь="Waiting for cargo";н=Ű(true);break;}case ι.ΔЇ:{ь="Unloading";н=Δύ();break;}case ι.Βν:{н=true;break;}
case ι.Βη:{ь="Charging batteries";н=Ǽ>=100f;break;}case ι.ǅ:{ь="Discharging batteries";н=Ǽ<=25f;break;}case ι.Ǆ:{ь="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βκ:{ь="Filling up hydrogen";н=ȕ>=100f;break;}case ι.Βλ:{ь="Unloading hydrogen";н=ȕ<=25f;break;}
case ι.Βι:{ь="Unloading hydrogen";н=ȕ<=0f;break;}case ι.Βμ:{bool ΕЀ=ΗΥ();if(!ΕЀ) ǿ.ΓЃ=true;н=ǿ.ΓЃ&&ΕЀ;ь="Waiting for passengers";break;}}if(!н) ǿ.ΒΔ(true);if(н&&ǿ.Βά()){н=ǿ.ΒΔ(false);ь="Undocking in: "+ΒΥ((int) Math.Max(0,ǿ.ǔ-ǿ.Ȯ));}if(Αώ) Ζω=ь;return н;}void Ζό()
{if(!ŋ) return;if(ο.Count<=1) return;float Ζρ=0,Ζπ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǯ=ο[i];if(ů(Ǯ,true)) continue;Ζρ+=(float) Ǯ.GetInventory(0).MaxVolume;Ζπ+=(float) Ǯ.GetInventory(0).CurrentVolume;}float ΕΟ=(float) Math.Round(А(Ζπ,Ζρ),5);for(int ΖΠ=0;ΖΠ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΠ++)
{float Γΰ=0,Γρ=0;float Εύ=0,ΕБ=0;IMyTerminalBlock ȓ=null,ȯ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǯ=ο[i];if(ů(Ǯ,true)) continue;float Γψ=(float) Ǯ.GetInventory(0).MaxVolume;float Αά=А((float) Ǯ.GetInventory(0).CurrentVolume,Γψ);if(ȓ==null||Αά>Γΰ){ȓ=Ǯ;Γΰ=Αά;Εύ=Γψ;}
if(ȯ==null||Αά<Γρ){ȯ=Ǯ;Γρ=Αά;ΕБ=Γψ;}}if(ȓ==null||ȯ==null||ȓ==ȯ) return;if(checkConveyorSystem&&!ȓ.GetInventory(0).IsConnectedTo(ȯ.GetInventory(0))){if(ΒЋ>20) Μ="Inventory balancing failed";else ΒЋ++;return;}ΒЋ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȓ.GetInventory(0).GetItems(Ъ);float ƫ=0;if(Ъ.Count==0) continue;MyInventoryItem Αέ=Ъ[0];String ΕЄ=Αέ.Type.TypeId+Αέ.Type.SubtypeId;if(!Ζψ.TryGetValue(ΕЄ,out ƫ)){if(ΗΩ(ȓ.GetInventory(0),0,ȯ.GetInventory(0),out ƫ))
Ζψ.Add(ΕЄ,ƫ);else return;}float ΙΑ=((Γΰ-ΕΟ)*Εύ/ƫ);float ΙΒ=((ΕΟ-Γρ)*ΕБ/ƫ);int ΔΝ=(int) Math.Min(ΙΒ,ΙΑ);if(ΔΝ<=0) return;if((float) Αέ.Amount<ΔΝ)
ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,Αέ.Amount);else
ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΔΝ);}}bool ΗΩ(IMyInventory ơ,int Β,IMyInventory ΘΘ,out float Ζχ){Ζχ=0;float ΗЍ=(float) ơ.CurrentVolume;List<MyInventoryItem>ΒЍ=new List<MyInventoryItem>();ơ.GetItems(ΒЍ);float ΕΣ=0;for(int i=0;i<ΒЍ.Count;i++) ΕΣ+=(float) ΒЍ[i].Amount;
ơ.TransferItemTo(ΘΘ,Β,null,null,1);float Ζφ=ΗЍ-(float) ơ.CurrentVolume;List<MyInventoryItem>ΒЃ=new List<MyInventoryItem>();ơ.GetItems(ΒЃ);float ΕΗ=0;for(int i=0;i<ΒЃ.Count;i++) ΕΗ+=(float) ΒЃ[i].Amount;if(Ζφ==0f||!Βΰ(0.9999,ΕΣ-ΕΗ,1.0001))
return false;Ζχ=Ζφ;return true;}void Ηϋ(){Vector3 Ǝ=φ+Ć;Vector3 ǭ=Ǝ-ΰ;if(ǭ.Length()==0) ǭ=new Vector3(0,0,-1);Vector3 Αμ=Я(Φ,ǭ);Vector3 Ζμ=Vector3.Normalize(Αμ);Vector3 ō=Я(Φ,Φ.GetNaturalGravity());float ΘЄ=ΓЀ>0?Math.Max(0,1-Ĳ(ǭ,Ερ)/5):0;float Џ=(float) Math.Min((Γυ>0?Γυ:1000f),Math.Max(Ʋ(-Αμ,ō,null),ΓЀ*ΘЄ));
if(!Ζο) Џ=0;if(Ζά) Џ=Math.Max(0,1-ΑΞ/ΑΗ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(ŗ) Џ=0;if(Ǳ>=0) Џ=Math.Min(Џ,Ǳ);if(Ζέ) Џ*=(float) Math.Min(1,А(ǭ.Length(),wpReachedDist)/2);Vector3 ΕЎ=Я(Φ,Φ.GetShipVelocities().LinearVelocity);float ΗЉ=(float)(Math.Max(0,15-Ĳ(-Ζμ,-ΕЎ))/15)*0.85f+0.15f;
ΔΕ+=Math.Sign(ΗЉ-ΔΕ)/10f;Vector3 Ȫ=Ζμ*Џ*ΔΕ-(ΕЎ);Vector3 ΒЊ=ΒΩ(Ȫ,null);if(ΑЃ&&К>0.1f){Ȫ.X*=Βω(Ȫ.X,ref ΑЄ.X,1f,ΒЊ.X,20);Ȫ.Y*=Βω(Ȫ.Y,ref ΑЄ.Y,1f,ΒЊ.Y,20);Ȫ.Z*=Βω(Ȫ.Z,ref ΑЄ.Z,1f,ΒЊ.Z,20);}else ΑЄ=new Vector3(1,1,1);ƥ=ĩ*Ȫ-ō*ĩ;Δέ(ƥ,ΑЃ);К=Vector3.Distance(ΰ,Ǝ);
}float Βω(float value,ref float Ф,float ќ,float Ș,float Γφ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ќ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γφ||Ș==0) return έ;Ф=Math.Min(Γφ,Math.Max(-Γφ,Ф));έ=Math.Abs(Ș);return έ;
}void ΕΕ(){float ы=90,Ǎ=90,ћ=90;float ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǩ,ї,Ɩ,Γξ;if(Γο){ǩ=Vector3.Normalize(φ-ΰ);Ɩ=Я(Φ,ǩ);ї=Я(Φ,ΔΓ);ы=Ĳ(Ɩ,new Vector3(0,-1,0))-90;Ǎ=ċ(ї,new Vector3(-1,0,0),ї.Y);
ћ=ċ(Ɩ,new Vector3(-1,0,0),Ɩ.Z);}else{ǩ=ΔΔ;ї=Я(Φ,ΔΓ);Ɩ=Я(Φ,ΔΔ);Γξ=Я(Φ,Ζν);ы=ċ(ї,new Vector3(0,0,1),ї.Y);Ǎ=ċ(ї,new Vector3(-1,0,0),ї.Y);ћ=ċ(Γξ,new Vector3(0,0,1),Γξ.X);}if(ΕΙ&&ƾ()){Vector3 ΓΩ=Φ.GetNaturalGravity();ї=Я(Φ,ΓΩ);ы=ċ(ї,new Vector3(0,0,1),ї.Y);
Ǎ=ċ(ї,new Vector3(-1,0,0),ї.Y);}if(!Βΰ(-45,Ǎ,45)){ы=0;ћ=0;}if(!Βΰ(-45,ћ,45)) ы=0;ǉ(ΓΫ,1,(-ы)*ǜ,(-ћ)*ǜ,(-Ǎ)*ǜ);ΑΞ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(Ǎ),Math.Abs(ћ)));Αδ=ΑΞ<=ΑΗ;}void ΖЁ(IMyTerminalBlock Αψ){if(Αψ==null) return;ǟ=new float[3,2];ѝ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(Αψ,в.WorldMatrix.Backward);float ȭ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǟ[0,0]+=ȭ;else ǟ[0,1]-=ȭ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǟ[1,0]+=ȭ;else ǟ[1,1]-=ȭ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǟ[2,0]+=ȭ;else ǟ[2,1]-=ȭ;String э=Δυ(в);float[,]ў=null;if(ѝ.ContainsKey(э)) ў=ѝ[э];else{ў=new float[3,2];ѝ.Add(э,ў);}float Ș=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ў[0,0]+=Ș;else ў[0,1]-=Ș;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ў[1,0]+=Ș;else ў[1,1]-=Ș;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ў[2,0]+=Ș;else ў[2,1]-=Ș;}}static String Δυ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Ββ(Vector3 є,float[,]ŏ){return new Vector3(
є.X>=0?ŏ[0,0]:ŏ[0,1],
є.Y>=0?ŏ[1,0]:ŏ[1,1],
є.Z>=0?ŏ[2,0]:ŏ[2,1]);}bool Δτ(Χ Ψ,String э,out float ΓΣ){ΓΣ=0;int Β=ȇ.IndexOf(э);if(Β==-1||Ψ.Ǟ==null||Β>=Ψ.Ǟ.Length)
return false;ΓΣ=Ψ.Ǟ[Β];if(ΓΣ==-1) return false;return true;}Vector3 ΒΩ(Vector3 є,Χ Ψ){if(Ψ!=null){Vector3 έ=new Vector3();for(int i=0;i<ѝ.Keys.Count;i++){String э=ѝ.Keys.ElementAt(i);float Ƌ=0;if(!Δτ(Ψ,э,out Ƌ))
return Ββ(є,ǟ);έ+=Ββ(є,ѝ.Values.ElementAt(i))*Ƌ;}return έ;}return Ββ(є,ǟ);}float Ŭ(Vector3 є,Χ Ψ){return Ŭ(є,new Vector3(),Ψ);}float Ŭ(Vector3 є,Vector3 ν,Χ Ψ){Vector3 ΘЊ=ΒΩ(є,Ψ);Vector3 ȭ=ΘЊ+ν*ĩ;float ƛ=(ȭ/є).AbsMin();return(float)(є*ƛ).Length();}float Ʋ(Vector3 Ακ,Vector3 ō,Χ Ψ)
{if(Ακ.Length()==0) return 0;float ΕΘ=1;if(ō.Length()>0) ΕΘ=Math.Min(1,Ĳ(-ō,Ακ)/90)*0.4f+0.6f;float ƥ=Ŭ(Ακ,ō,Ψ);if(ƥ==0) return 0.1f;float ΑΖ=А(ƥ,ĩ);float ΔΘ=(float) Math.Sqrt(А(Ακ.Length(),ΑΖ*0.5f));return ΑΖ*ΔΘ*ΕΘ*Ǥ;}float ƻ(Vector3 ǩ,Vector3 ώ,Vector3 ν,Χ Ψ)
{if(ν.Length()==0f) return 0;Vector3 Αλ=đ(ǩ,ώ,Vector3.Normalize(ν));float ǖ=Ŭ(-Αλ,Ψ);return ǖ/ν.Length();}void Δΰ(bool ΘΪ){float џ=0;float ΓΦ=0.9f;if(ΘΪ){Н=-1;ĭ=0;ΑЀ=null;if(ή!=Κ.з&&Ε.ν.Length()!=0){џ=ΓΦ*ƻ(Ε.γ,Ε.β*-1,Ε.ν,null);if(џ<Н||Н==-1) Н=џ;}if(Θ.δ&&Θ.ν.Length()!=0)
{џ=ΓΦ*ƻ(Θ.γ,Θ.β*-1,Θ.ν,null);if(џ<Н||Н==-1) Н=џ;}return;}if(ĭ==-1) return;if(ĭ>=0){int ΖΤ=0;while(ĭ<Σ.Count){if(ΖΤ>100) return;ΖΤ++;Χ Ψ=Σ[ĭ];if(Ψ.ν.Length()!=0f){џ=ΓΦ*Math.Min(
ƻ(Ψ.γ,Ψ.β*-1,Ψ.ν,Ψ),
ƻ(Ψ.γ*-1,Ψ.β*-1,Ψ.ν,Ψ));if(џ<Н||Н==-1) Н=џ;}else ΑЀ=Ψ;ĭ++;}ĭ=-1;}bool Γά=true;float Ă=0;if(ΑЀ!=null){for(int i=0;i<ѝ.Count;i++){String э=ѝ.Keys.ElementAt(i);float[,]ў=ѝ.Values.ElementAt(i);float Ƌ=0;if(!Δτ(ΑЀ,э,out Ƌ)){Γά=false;break;}for(int a=0;a<ў.GetLength(0);a++)
for(int b=0;b<ў.GetLength(1);b++){float Ĕ=Math.Abs(ў[a,b]*Ƌ);if(Ĕ==0) continue;Γά=true;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}}if(!Γά){for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float Ĕ=Math.Abs(ǟ[a,b]);if(Ĕ==0) continue;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}if(Ă>0){float Θζ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;џ=А(Ă,Θζ);if(џ>0)
if(џ<Н||Н==-1) Н=џ;}}void Δέ(Vector3 ǖ,bool Ј){if(!Ј){for(int i=0;i<Ȉ.Count;i++)
Ȉ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЉ=ΒΩ(ǖ,null);float ΙΠ=Math.Min(1,Math.Abs(А(ǖ.X,ΒЉ.X)));float ΙΡ=Math.Min(1,Math.Abs(А(ǖ.Y,ΒЉ.Y)));float ΙΣ=Math.Min(1,Math.Abs(А(ǖ.Z,ΒЉ.Z)));for(int i=0;i<Ȉ.Count;i++){IMyThrust в=Ȉ[i];Vector3 κ=Ηγ(Я(Φ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǖ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΠ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǖ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΡ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǖ.Z))
в.SetValueFloat("Override",в.MaxThrust*ΙΣ);else
в.SetValueFloat("Override",0.0f);}}void ǉ(bool Ј,float Θς,float ы,float ћ,float Ǎ){for(int i=0;i<Αί.Count;i++){IMyGyro І=Αί[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=Θς;if(!Ј) continue;Vector3 В=Φ.WorldMatrix.Forward;Vector3 Ь=Φ.WorldMatrix.Right;Vector3 ώ=Φ.WorldMatrix.Up;
Vector3 ņ=І.WorldMatrix.Forward;Vector3 ň=І.WorldMatrix.Up;Vector3 Ň=І.WorldMatrix.Left*-1;if(ņ==В) І.SetValueFloat("Roll",Ǎ);else if(ņ==(В*-1)) І.SetValueFloat("Roll",Ǎ*-1);else if(ň==(В*-1)) І.SetValueFloat("Yaw",Ǎ);else if(ň==В) І.SetValueFloat("Yaw",Ǎ*-1);else if(Ň==В) І.SetValueFloat("Pitch",Ǎ);
else if(Ň==(В*-1)) І.SetValueFloat("Pitch",Ǎ*-1);if(Ň==(Ь*-1)) І.SetValueFloat("Pitch",ы);else if(Ň==Ь) І.SetValueFloat("Pitch",ы*-1);else if(ň==Ь) І.SetValueFloat("Yaw",ы);else if(ň==(Ь*-1)) І.SetValueFloat("Yaw",ы*-1);else if(ņ==(Ь*-1)) І.SetValueFloat("Roll",ы);
else if(ņ==Ь) І.SetValueFloat("Roll",ы*-1);if(ň==(ώ*-1)) І.SetValueFloat("Yaw",ћ);else if(ň==ώ) І.SetValueFloat("Yaw",ћ*-1);else if(Ň==ώ) І.SetValueFloat("Pitch",ћ);else if(Ň==(ώ*-1)) І.SetValueFloat("Pitch",ћ*-1);else if(ņ==ώ) І.SetValueFloat("Roll",ћ);else if(ņ==(ώ*-1)) І.SetValueFloat("Roll",ћ*-1);
}}void ΕΑ(){this.ΓΫ=false;}void Х(Vector3 Ȭ,Vector3 В,Vector3 Γμ,float Ȟ,bool ƪ){ΑЎ(Ȭ,Ȟ,ƪ);ΑΗ=Ȟ;Γο=false;this.ΔΔ=В;this.Ζν=Γμ;}void Х(Vector3 Ȭ,Vector3 В,Vector3 Γμ,bool ƪ){Х(Ȭ,В,Γμ,2f,ƪ);}void ΑЎ(Vector3 Ȭ,float Ȟ,bool ƪ){ΑΗ=Ȟ;this.ΓΫ=true;this.ΕΙ=ƪ;Γο=true;Αδ=false;
this.ΔΓ=Ȭ;}void Βώ(){Ǣ(false,false,false,φ,0);ΑЃ=false;}void П(Vector3 Њ,float ǜ){Ǣ(true,false,false,Њ,ǜ);}void Ǣ(bool Ј,bool ΓЏ,bool ΔΠ,Vector3 Њ,float ǜ){Ǣ(Ј,ΓЏ,ΔΠ,Њ,Њ-ΰ,0.0f,ǜ);}void Ǣ(bool Ј,bool ΓЏ,bool ΔΠ,Vector3 Њ,Vector3 Θώ,float ΘϏ,float ǜ){ΑЃ=true;this.Ζο=Ј;
φ=Њ;this.Γυ=ǜ;this.ΓЀ=ΘϏ;this.Ζέ=ΓЏ;this.Ζά=ΔΠ;this.Ερ=Θώ;К=Vector3.Distance(Њ,ΰ);}void Ηδ(){ω.GetBlocksOfType(Ż,Ǐ);}void Ηε(){if(Αϊ){ύ=2;return;}List<IMyRemoteControl>Ɯ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΩ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζδ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ɯ,Ǐ);ω.GetBlocksOfType(і,Ǐ);ω.GetBlocksOfType(ΖΩ,Ǐ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζδ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);і=ǡ(і,pamTag,true);Ημ();Ην(Ζδ);Ŷ(і,setLCDFontAndSize,1.4f,false);Ŷ(ą,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζθ=ǡ(ΖΩ,pamTag,true);if(Ζθ.Count>0) ξ=Ζθ[0];else ξ=null;if(Η==Ζ.Ŵ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.ǎ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϊ){ω.GetBlocksOfType(ΔΙ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζη=ǡ(Ɯ,pamTag,true);if(Ζη.Count>0) Ɯ=Ζη;if(Ɯ.Count>0) Φ=Ɯ[0];else{Φ=null;ύ=2;Μ="Remote is missing";return;}θ=(float) Φ.CubeGrid.WorldVolume.Radius*2;њ=null;if(Η!=Ζ.Ϊ){Δα();if(ο.Count>0&&њ!=null){if(ξ!=null&&(њ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(њ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
њ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ηζ(){ω.GetBlocksOfType(Ȅ,Ǐ);ω.GetBlocksOfType(т,Ǐ);ω.GetBlocksOfType(Ȉ,Ǐ);ω.GetBlocksOfType(Αί,Ǐ);ω.GetBlocksOfType(Ȩ,Ǐ);ω.GetBlocksOfType(ă,Ǐ);ω.GetBlocksOfType(ѕ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑϏ,Ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǡ(т,"ConnectorMedium",false);else
т=ǡ(т,"Connector",false);List<IMyShipConnector>Ζε=ǡ(т,pamTag,true);if(Ζε.Count>0) т=Ζε;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(Αί.Count==0){ύ=1;Μ="Gyros are missing";}if(Ȉ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζι=ǡ(ΑϏ,pamTag,true);
if(Ζι.Count>0) ΑϏ=Ζι;List<IMyLandingGear>Ζζ=ǡ(Ȅ,pamTag,true);if(Ζζ.Count>0) Ȅ=Ζζ;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζγ=ǡ(Ȩ,pamTag,true);if(Ζγ.Count>0) Ȩ=Ζγ;List<IMyGasTank>Ζκ=ǡ(ѕ,pamTag,true);if(Ζκ.Count>0) ѕ=Ζκ;}void Ηη(){ω.GetBlocksOfType(ĕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ł.Clear();for(int i=ĕ.Count-1;i>=0;i--){if(ΗΡ(ĕ[i])){ł.Add(ĕ[i]);ĕ.RemoveAt(i);}}}bool ΗΡ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βέ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δα(){if(Φ==null) return;њ=null;float Ασ=0,Αξ=0,Ατ=0,Αο=0,Αυ=0,Απ=0;List<IMyTerminalBlock>ΔΒ=ǡ(ο,pamTag,true);bool Θν=ΔΒ.Count==0;if(ΔΒ.Count>0) њ=ΔΒ[0];else if(ο.Count>0) њ=ο[0];int ȁ=0;float Ř=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward){if(Θν){ύ=2;Μ="Mining direction is unclear!";return;}continue;}ȁ++;Ř=Math.Max(Ř,Ηφ(ο[i]));Vector3 Љ=ƺ(Φ,ο[i].GetPosition());if(i==0){Ασ=Љ.X;Αξ=Љ.X;Ατ=Љ.Y;Αο=Љ.Y;Αυ=Љ.Z;Απ=Љ.Z;}Αξ=Math.Max(Љ.X,Αξ);Ασ=Math.Min(Љ.X,Ασ);
Αο=Math.Max(Љ.Y,Αο);Ατ=Math.Min(Љ.Y,Ατ);Απ=Math.Max(Љ.Z,Απ);Αυ=Math.Min(Љ.Z,Αυ);}if(Ř==0) Ř=drillRadius;bool Ȭ=њ!=null&&њ.WorldMatrix.Forward==Φ.WorldMatrix.Down;ǰ=(Αξ-Ασ)*(1-Į)+Ř*2;ǯ=(Ȭ?Απ-Αυ:Αο-Ατ)*(1-ĝ)+Ř*2;IMyCubeGrid g=Φ.CubeGrid;float ΔΦ=0,ΔΧ=0,ΔΨ=0,ΔΩ=0;
for(int c=0;c<8;c++){Vector3 p=ƺ(Φ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȭ?p.Z:p.Y;if(c==0||p.X<ΔΦ) ΔΦ=p.X;if(c==0||p.X>ΔΧ) ΔΧ=p.X;if(c==0||y<ΔΨ) ΔΨ=y;if(c==0||y>ΔΩ) ΔΩ=y;}Ş=ΔΧ-ΔΦ+g.GridSize;Ɵ=ΔΩ-ΔΨ+g.GridSize;}float Ηφ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǡ<T>(List<T>Т,String ǝ,bool Αΰ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βέ(Т[i],ǝ,Αΰ)) έ.Add(Т[i]);return έ;}bool Βέ<T>(T Τ,String ǝ,bool Αΰ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αΰ&&b.CustomName.ToUpper().Contains(ǝ.ToUpper())) return true;if(!Αΰ&&b.BlockDefinition.ToString().ToUpper().Contains(ǝ.ToUpper())) return true;return false;}void ё<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΔЍ((IMyTerminalBlock) Т[i],Ј);}void ΔЍ(IMyTerminalBlock Τ,bool Ј){if(Τ==null) return;String ΗЇ=Ј?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЇ);a.Apply(Τ);}void ǣ(bool Ј){ΓА=Ј;if(!Ī) return;ё(ΑϏ,Ј);}void Βυ(ChargeMode Ɨ){for(int i=0;i<Ȩ.Count;i++) Ȩ[i].ChargeMode=Ɨ;}void Βχ(bool ΘІ)
{for(int i=0;i<ѕ.Count;i++) ѕ[i].Stockpile=ΘІ;}void ŷ(List<IMyLandingGear>ΓΧ,bool ΕЏ){for(int i=0;i<ΓΧ.Count;i++){if(ΕЏ) ΓΧ[i].Lock();if(!ΕЏ) ΓΧ[i].Unlock();}}void ΔЎ(bool Ј){if(Εα==Ј) return;List<IMyShipController>ΑΜ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΜ,Ǐ);if(ΑΜ.Count==0) return;
for(int i=0;i<ΑΜ.Count;i++)
ΑΜ[i].DampenersOverride=Ј;Εα=Ј;}IMyShipConnector О(MyShipConnectorStatus ь){for(int i=0;i<т.Count;i++){if(!Ƿ(т[i],true)) continue;if(т[i].Status==ь) return т[i];}return null;}void Ē(bool ΓЌ,bool ΑΣ,float ΘΠ,float ΗЌ){if(ξ==null||ο.Count==0) return;Vector3 ΑΛ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward) continue;ȁ++;ΑΛ+=ο[i].GetPosition();}ΑΛ=ΑΛ/ȁ;Vector3 Ŕ=ƺ(ξ,ΑΛ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЌ;ξ.LeftExtend=(ΑΣ?1:й)/2f*ǰ-Ŕ.X;ξ.RightExtend=(ΑΣ?1:й)/2f*ǰ+Ŕ.X;ξ.TopExtend=(ΑΣ?1:и)/2f*ǯ+Ŕ.Y;
ξ.BottomExtend=(ΑΣ?1:и)/2f*ǯ-Ŕ.Y;ξ.FrontExtend=(ΓЌ?Ц:ΘΠ)-Ŕ.Z;ξ.BackExtend=ΓЌ?0:ΗЌ+θ*0.75f+Ŕ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ŷ<T>(List<T>Т,bool ΘЁ,float ΘΞ,bool ΘΫ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƣ=null;if(Т[i]is IMyTextSurface) ƣ=(IMyTextSurface) Т[i];if(ƣ!=null){ƣ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЁ) continue;ƣ.Font="Debug";if(ΘΫ) continue;ƣ.FontSize=ΘΞ;}}}bool ƾ(){Vector3D ζ;return this.Φ.TryGetPlanetPosition(out ζ);}int ΗΘ(Vector3 ΘЈ,int Ζή){if(Ζή==-1) return 0;double Б=-1;int Ȓ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘЈ);if(Б==-1||ό<Б){Ȓ=i;Б=ό;}
}return Math.Sign(Ȓ-Ζή);}bool ΗΤ(Vector3 ζ){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.δ&&Σ.Count>=1){Vector3 Ľ=new Vector3();Ƕ(Θ,dockDist*θ,false,out Ľ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,Ľ);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(Ľ);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϊ){if(Ε.δ&&Σ.Count>=1){Vector3 ΒЅ=new Vector3();Ƕ(Ε,dockDist*θ,false,out ΒЅ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЅ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЅ);Ж.Add(Ε.Δ);}}}else{if(ή!=Κ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȓ=-1;double Ǧ=-1;for(int i=0;i<Ж.Count;i++){double ό=Vector3.Distance(Ж[i],ζ);if(ό<Ǧ||Ǧ==-1){Ǧ=ό;Ȓ=i;}}if(Ж.Count==0) return false;double ΓΜ=Vector3.Distance(Ж[Ȓ],ζ);double Θσ=Vector3.Distance(Ж[Math.Max(0,Ȓ-1)],Ж[Ȓ])*1.5f;
double Θμ=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȓ+1)],Ж[Ȓ])*1.5f;return ΓΜ<Θσ||ΓΜ<Θμ||ΓΜ<laneOffset*θ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηγ(Vector3 v,int ΓΙ){return new Vector3(Math.Round(v.X,ΓΙ),Math.Round(v.Y,ΓΙ),Math.Round(v.Z,ΓΙ));
}float Ĳ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΝ=(a*b).Sum;float ΕЊ=a.Length();float ΕЋ=b.Length();if(ΕЊ==0||ΕЋ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΝ/(ΕЊ*ΕЋ)));if(float.IsNaN(έ)) return 0;return έ;}float ċ(Vector3 v,Vector3 Αψ,float ΘЂ){float Ļ=Ĳ(v,Αψ);
if(ΘЂ>0f) Ļ*=-1;if(Ļ>-90f) return Ļ-90f;else return 180f-(-Ļ-90f);}double Δη(float ΘΖ){return(Math.PI/180)*ΘΖ;}bool Βΰ(double Θε,double value,double Αν){return(value>=Θε&&value<=Αν);}Vector3 Ηχ(IMyTerminalBlock Τ,Vector3 Љ){return Vector3D.Transform(Љ,Τ.WorldMatrix);
}Vector3 ƺ(IMyTerminalBlock Τ,Vector3 ΙΝ){return Я(Τ,ΙΝ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΥ){return Vector3D.TransformNormal(ΔΥ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 đ(Vector3 ǩ,Vector3 ώ,Vector3 ΔΥ){MatrixD Γτ=MatrixD.CreateFromDir(ǩ,ώ);return Vector3D.TransformNormal(ΔΥ,MatrixD.Transpose(Γτ));
}Vector3 ΒϏ(Vector3 ǩ,Vector3 ώ,Vector3 Љ){MatrixD Γτ=MatrixD.CreateFromDir(ǩ,ώ);return Vector3D.Transform(Љ,Γτ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Λ(Е,0)),float.Parse(Λ(Е,1)),float.Parse(Λ(Е,2)));
}String ΗЅ(Χ Ψ){String Ћ=":";String έ=τ(Ψ.Δ)+Ћ+τ(Ψ.γ)+Ћ+
τ(Ψ.β)+Ћ+τ(Ψ.ψ)+Ћ+τ(Ψ.ν);for(int i=0;i<Ψ.Ǟ.Length;i++)
έ+=Ћ+Math.Round(Ψ.Ǟ[i],3);return έ;}Χ Ης(String s){String[]Е=s.Split(':');Χ Ψ=new Χ(
μ(Λ(Е,0)),μ(Λ(Е,1)),
μ(Λ(Е,2)),μ(Λ(Е,3)),
μ(Λ(Е,4)));int Β=5;List<float>Εν=new List<float>();while(Β<Е.Length){String ΑЉ=Λ(Е,Β);float f=0;if(!float.TryParse(ΑЉ,out f)) break;Εν.Add(f);Β++;}Ψ.Ǟ=Εν.ToArray();return Ψ;}System.Text.StringBuilder Ŝ=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θλ){if(Θλ) Ŝ.Append('\n');Ŝ.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƶ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΒ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Λ(String[]ΗЋ,int Β){String ΑЉ=ΗЋ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑЉ)) return "";
return ΑЉ;}void Save(){if(Αϊ||Η==Ζ.р){Storage="";return;}Storage=ΒΓ();}String ΒΓ(){Ŝ.Clear();Ŝ.Append(DATAREV+";");Ρ(τ(Αω),false);Ρ(τ(Θ.γ));Ρ(τ(Θ.ψ));Ρ(τ(Θ.β));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.δ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.γ));Ρ(τ(Ε.β));Ρ(τ(Ε.ψ));Ρ(τ(Ε.Ч));Ρ(Ε.δ);Ρ(τ(Ǜ));Ρ(τ(ș));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(Ǫ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(Ǥ);Ρ(Ƞ);Ρ(ȟ);if(Η==Ζ.Ϊ){Ρ((int) ϊ.Ќ);Ρ(ϊ.ǔ);Ρ(ϊ.Ȯ);Ρ(ƶ(ϊ.Ǖ));Ρ(ƶ(ϊ.ǘ));Ρ((int) ϋ.Ќ);Ρ(ϋ.ǔ);Ρ(ϋ.Ȯ);Ρ(ƶ(ϋ.Ǖ));Ρ(ƶ(ϋ.ǘ));}else{Ρ((int) ȝ);Ρ((int) л);Ρ((int) Л);Ρ((int) у);Ρ((int) Ģ);Ρ(ć);Ρ(Ī);Ρ(ŋ);Ρ(ń);Ρ(й);Ρ(и);Ρ(Ц);Ρ(ŧ);Ρ(Ŧ);Ρ(Į);Ρ(ĝ);
Ρ(М);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(ȗ);Ρ(Й);Ρ(ħ);Ρ(ĥ);}Ρ(";");for(int i=0;i<ȇ.Count;i++)
Ρ((i>0?"|":"")+ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЅ(Σ[i]),i>0);Ρ(";",false);Ρ(С+"|"+Ё,false);Ρ(";",false);Ρ(ΖА(),false);String Ϋ=Ŝ.ToString();Ŝ.Clear();return Ϋ;}с Ηβ(){return ǆ(Storage);}с ǆ(String Ϋ){if(Ϋ=="") return с.ΗΫ;String[]Ĩ=Ϋ.Split(';');if(Λ(Ĩ,0)!=DATAREV) return с.Βε;int Β=0;try{String[]Ι=Λ(Ĩ,1).Split('\n');
Αω=μ(Λ(Ι,Β++));Θ.γ=μ(Λ(Ι,Β++));Θ.ψ=μ(Λ(Ι,Β++));Θ.β=μ(Λ(Ι,Β++));Θ.ν=μ(Λ(Ι,Β++));Θ.Δ=μ(Λ(Ι,Β++));Θ.Ч=μ(Λ(Ι,Β++));Θ.δ=bool.Parse(Λ(Ι,Β++));Ε.Δ=μ(Λ(Ι,Β++));Ε.ν=μ(Λ(Ι,Β++));Ε.γ=μ(Λ(Ι,Β++));Ε.β=μ(Λ(Ι,Β++));Ε.ψ=μ(Λ(Ι,Β++));Ε.Ч=μ(Λ(Ι,Β++));Ε.δ=bool.Parse(Λ(Ι,Β++));
Ǜ=μ(Λ(Ι,Β++));ș=μ(Λ(Ι,Β++));Ι=Λ(Ĩ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Λ(Ι,Β++));ή=(Κ) int.Parse(Λ(Ι,Β++));ц=(Κ) int.Parse(Λ(Ι,Β++));Ǫ=int.Parse(Λ(Ι,Β++));ч=int.Parse(Λ(Ι,Β++));Ы=int.Parse(Λ(Ι,Β++));ш=int.Parse(Λ(Ι,Β++));Ǥ=float.Parse(Λ(Ι,Β++));Ƞ=bool.Parse(Λ(Ι,Β++));
ȟ=bool.Parse(Λ(Ι,Β++));if(Η==Ζ.Ϊ){ϊ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϊ.ǔ=float.Parse(Λ(Ι,Β++));ϊ.Ȯ=float.Parse(Λ(Ι,Β++));ϊ.Ǖ=ΑΒ(Λ(Ι,Β++));ϊ.ǘ=ΑΒ(Λ(Ι,Β++));ϋ.Ќ=(ι) int.Parse(Λ(Ι,Β++));ϋ.ǔ=float.Parse(Λ(Ι,Β++));ϋ.Ȯ=float.Parse(Λ(Ι,Β++));ϋ.Ǖ=ΑΒ(Λ(Ι,Β++));ϋ.ǘ=ΑΒ(Λ(Ι,Β++));}else
{ȝ=(ǐ) int.Parse(Λ(Ι,Β++));л=(д) int.Parse(Λ(Ι,Β++));Л=(Ѐ) int.Parse(Λ(Ι,Β++));у=(ю) int.Parse(Λ(Ι,Β++));Ģ=(ǐ) int.Parse(Λ(Ι,Β++));ć=bool.Parse(Λ(Ι,Β++));Ī=bool.Parse(Λ(Ι,Β++));ŋ=bool.Parse(Λ(Ι,Β++));ń=bool.Parse(Λ(Ι,Β++));й=int.Parse(Λ(Ι,Β++));и=int.Parse(Λ(Ι,Β++));Ц=int.Parse(Λ(Ι,Β++));
ŧ=float.Parse(Λ(Ι,Β++));Ŧ=float.Parse(Λ(Ι,Β++));Į=float.Parse(Λ(Ι,Β++));ĝ=float.Parse(Λ(Ι,Β++));М=int.Parse(Λ(Ι,Β++));б=int.Parse(Λ(Ι,Β++));Ш=int.Parse(Λ(Ι,Β++));Щ=int.Parse(Λ(Ι,Β++));ȗ=int.Parse(Λ(Ι,Β++));Й=int.Parse(Λ(Ι,Β++));ħ=int.Parse(Λ(Ι,Β++));ĥ=float.Parse(Λ(Ι,Β++));
}Ι=Λ(Ĩ,3).Replace("\n","").Split('|');ȇ=Ι.ToList();Ι=Λ(Ĩ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ης(Λ(Ι,i)));Ι=Λ(Ĩ,5).Split('|');С=Λ(Ι,0);if(!int.TryParse(Λ(Ι,1),out Ё)) Ё=-1;if(С.Length!=М*б){С="";Ё=-1;}ΖЏ(Λ(Ĩ,6));}catch{return с.Δλ;}ŝ=ц;χ();return с.ǋ;}const int Δί=64000;IMyTerminalBlock Δμ(){List<IMyTerminalBlock>ŀ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ŀ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ŀ.Count>0?ŀ[0]:null;}void ΗΒ(){IMyTerminalBlock Τ=Δμ();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Ϋ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΓ();if(Ϋ.Length>Δί){Μ="Export too large: "+Ϋ.Length+" chars, max "+Δί;return;}Τ.CustomData=Ϋ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΝ(bool Γθ){IMyTerminalBlock Τ=Δμ();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΞ(Τ.CustomData,Γθ);}void ΗΞ(String Ϋ,bool Γθ){Ϋ=Ϋ.Replace("\r","");if(Ϋ.StartsWith("//")){int ΖΚ=Ϋ.IndexOf('\n');Ϋ=ΖΚ<0?"":Ϋ.Substring(ΖΚ+1);}Ϋ=Ϋ.Trim();if(Ϋ==""){Μ="Import: no PAM data in "+dataTag;return;}χ();String ΕΡ=ΒΓ();с έ=ǆ(Ϋ);if(έ==с.ǋ&&Γθ){Χ ġ=new Χ(Ε);
Vector3 Θθ=Ǜ,Θη=ș;int w=й,h=и,d=Ц;ǐ ΓБ=ȝ;ю ΓΚ=у;float Θπ=Į,Θο=ĝ;ǆ(ΕΡ);Ε=ġ;Ǜ=Θθ;ș=Θη;й=w;и=h;Ц=d;ȝ=ΓБ;у=ΓΚ;Į=Θπ;ĝ=Θο;ή=Κ.Ķ;Ź(true,true);ή=Κ.Ķ;ц=Κ.Ķ;Μ="Imported job, run CONT";return;}if(έ==с.ǋ){Ƙ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ǆ(ΕΡ);Μ=έ==с.Βε?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ημ(){ƅ.Clear();for(int i=і.Count-1;i>=0;i--){String Ϋ=і[i].CustomData.ToUpper();bool Γζ=false;if(Ϋ=="INSTRUCTIONS"){Γζ=true;ΑΡ=true;}if(Ϋ=="DEBUG") Γζ=true;if(Γζ){ƅ.Add(і[i]);і.RemoveAt(i);}
}Ŷ(ƅ,false,1,false);}void Ην(List<IMyTerminalBlock>ŀ){ą.Clear();for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Τ=ŀ[i];try{String ΖΣ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΣ);int Š=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΣ.Length,1),out Š)) continue;
if(Š==-1) continue;Š--;IMyTextSurfaceProvider ΖΥ=(IMyTextSurfaceProvider) Τ;if(Š<ΖΥ.SurfaceCount&&Š>=0)
ą.Add(ΖΥ.GetSurface(Š));}catch{}}}String Αρ="";void ΔЉ(){String Γσ="";String ΓΡ="";Αρ=Δή(false);Γσ+=Αρ;ΓΡ+=Αρ;ΓΡ+=ΗΖ();for(int i=0;i<і.Count;i++) і[i].WriteText(Γσ);for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γσ);Echo(ΓΡ);for(int i=0;i<ƅ.Count;i++){IMyTextPanel ΓϏ=ƅ[i];
String Ϋ=ΓϏ.CustomData.ToUpper();if(Ϋ=="DEBUG") ΓϏ.WriteText(""+"\n"+"");if(Ϋ=="INSTRUCTIONS") ΓϏ.WriteText(ΗΗ());}}string ΗΗ(){String Γ="";try{float ΖΑ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ŀ*ΖΑ)+" / "+(int)(Ő*ΖΑ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ŀ*100f,1)+"% / "+Math.Round(Ő*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ž,2)+"ms / "+Math.Round(Γχ,2)+"ms \n";}catch{}for(int i=0;i<Ț.Count;i++)
Γ+=""+Ț.Keys.ElementAt(i)+" = "+Math.Round(Ț.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ț.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΚ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ŷ(і,setLCDFontAndSize,0.9f,false);Ŷ(ą,setLCDFontAndSize,0.9f,true);for(int i=0;i<і.Count;i++) і[i].WriteText(Γ+Ȗ.ToString());for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γ+Ȗ.ToString());}void Ǻ(){if(!ΑΡ) return;try{ΓЄ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String υ){if(!ΑΡ) return;if(ΓЄ==0) return;try{float ΑЇ=(Runtime.CurrentInstructionCount-ΓЄ)/Runtime.MaxInstructionCount*100;if(!Ț.ContainsKey(υ))
Ț.Add(υ,new float[]{ΑЇ,ΑЇ});else{Ț[υ][0]=ΑЇ;Ț[υ][1]=Math.Max(ΑЇ,Ț[υ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζύ(String Ȱ){if(!Ł) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=Ũ(ΓЋ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=ΒΤ(ΰ.X)+""+Ћ;ρ+=ΒΤ(ΰ.Y)+Ћ;ρ+=ΒΤ(ΰ.Z)+Ћ;if(Η!=Ζ.Ϊ)
ρ+=ΒΦ(ц)+(ц==Κ.Ϗ?" "+Math.Round(ƒ,1)+"%":"")+Ћ;else ρ+=ΒΦ(ц)+Ћ;ρ+=Ũ(Μ)+Ћ;ρ+=ј+""+Ћ;ρ+=Ũ(Αρ)+Ћ;ρ+=Ń+Ћ;ρ+=ő+Ћ;for(int i=0;i<Ǘ.Count;i++){if(Ǘ[i].Ŏ!=З.Ů) continue;ρ+=Ũ(Ǘ[i].υ)+"/"+Ũ(Ǘ[i].ς)+"/"+Ǘ[i].ǒ+Ћ;}Ηι(ρ,Ȱ);}String Ũ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηι(String ρ,String Ȱ){Ηκ(ρ,BROADCAST_SELF_ID,Ȱ);}void Ηκ(String ρ,String Њ,String Ȱ){try{if(!Ł) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δο()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΒЏ+BROADCAST_SEP+Ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȗ=e;}
}String Δο(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔϏ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ū(ref String Ϋ,out string Εξ,Char Ћ){int Β=Ϋ.IndexOf(Ћ);Εξ="";if(Β<0) return false;Εξ=Ϋ.Substring(0,Β);Ϋ=Ϋ.Remove(0,Β+1);return true;}bool Ηά(ref String ρ,out String ƞ,out String Ȱ)
{ƞ="";Ȱ="";if(!Ł) return false;String ǝ="";if(!Ū(ref ρ,out ǝ,BROADCAST_SEP)||!ΔϏ(ǝ)) return false;if(!Ū(ref ρ,out ƞ,BROADCAST_SEP)) return false;if(!Ū(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=Δο()&&(ǝ!="*"&&Η!=Ζ.р))) return false;if(!Ū(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=ΒЏ)) return false;
if(!Ū(ref ρ,out Ȱ,BROADCAST_SEP)) return false;return true;}void ΗΠ(){bool ƃ=true;if(Επ){Επ=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Ə=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ə)
Δϋ(Ə);String[]Ι=Me.CustomData.Split('\n');Ł=bool.Parse(Βο(ΒΡ(Ι,"Enable_Broadcast",ref ƃ)));bool ΔΣ=false;bool Εε=true;if(Ł){if(Η!=Ζ.р)
ΓЋ=Βο(ΒΡ(Ι,"Ship_Name",ref ƃ)).Replace(BROADCAST_SELF_ID,"");ΒЏ=Βο(ΒΡ(Ι,"Broadcast_Channel",ref ƃ)).ToLower();Εε=false;if(ȑ==null){ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȑ.SetMessageCallback("");}List<IMyRadioAntenna>ļ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ļ);bool ΒЄ=false;
for(int i=0;i<ļ.Count;i++)
if(ļ[i].EnableBroadcasting&&ļ[i].Enabled){ΒЄ=true;break;}if(ļ.Count==0) Μ="No Antenna found";else if(!ΒЄ) Μ="Antenna not ready";ΔΣ=ļ.Count==0||!ΒЄ;if(ΕΚ&&!ΔΣ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΚ=ΔΣ;if(Εε){if(ȑ!=null) this.IGC.DisableBroadcastListener(ȑ);ȑ=null;}}catch{ƃ=false;}if(!ƃ){Μ="Reset custom data";Δϋ(Ə);}}void Δϋ(String Ə){String Ϋ=Ə+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Ϋ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ϋ+"Broadcast_Channel=#default";}String Βο(String Ĥ){int Β=Ĥ.IndexOf("//");if(Β!=-1) Ĥ=Ĥ.Substring(0,Β);String[]Е=Ĥ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΡ(String[]Ι,String ģ,ref bool ƙ){foreach(String Ĥ in Ι)
if(Ĥ.StartsWith(ģ)) return Ĥ;ƙ=false;return "";}String Ιι="";ĸ Ιλ=null;DateTime Ιμ=DateTime.MinValue;List<ĸ>Ικ=new List<ĸ>();const float ΙΥ=20f;String ΔЌ(int ΔΡ,String Γ,int ΖΨ,ref int ț){String[]Ι=Γ.Split('\n');if(ΖΨ>=ț+ΔΡ-1) ț++;ț=Math.Min(Ι.Count()-1-ΔΡ,ț);if(ΖΨ<ț+1) ț--;
ț=Math.Max(0,ț);String έ="";for(int i=0;i<ΔΡ;i++){int ΕЌ=i+ț;if(ΕЌ>=Ι.Count()) break;έ+=Ι[ΕЌ]+"\n";}return έ;}String ΒΥ(int Ɲ){if(Ɲ>=60*60) return Math.Round(Ɲ/(60f*60f),1)+" h";if(Ɲ>=60) return Math.Round(Ɲ/60f,1)+" min";return ""+Ɲ+" s";}string ΒΤ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>ŕ=new List<long>();bool ŗ=false;int ΓΔ=0;int ΓΑ=0;DateTime ΕΥ=DateTime.Now;String ΒЎ="";void Ηθ(){ǽ.Clear();ŕ.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!ŕ.Contains(ǽ[i].CubeGrid.EntityId)) ŕ.Add(ǽ[i].CubeGrid.EntityId);}if(!ŕ.Contains(Me.CubeGrid.EntityId)) ŕ.Add(Me.CubeGrid.EntityId);}bool Δώ(Α s){return s==Α.ȋ||s==Α.Ю||s==Α.Ѕ||
s==Α.ȥ||s==Α.ĳ||s==Α.ū||s==Α.Ƹ||
(s==Α.Ǡ&&љ!=null);}void ƴ(){ŗ=false;ΓΔ=0;ΒЎ="";}void ΗϏ(){if(!obstacleBrake||ǽ.Count==0||!Δώ(Υ)){ƴ();return;}Vector3 Ǝ=φ+Ć;Vector3 ǭ=Ǝ-ΰ;float Ǭ=ǭ.Length();if(Ǭ<1f){ƴ();return;}Vector3 κ=ǭ/Ǭ;float ΕА=Ȝ*obstacleLookAhead+θ;Vector3 Θφ=Ǝ+κ*(θ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΓΑ=(ΓΑ+1)%ǽ.Count;IMyCameraBlock Ƃ=ǽ[ΓΑ];if(!Ƃ.IsFunctional||!Ƃ.IsWorking) continue;Vector3 ΕΧ=Ƃ.GetPosition();Vector3 ΑΪ=Θφ;if(Vector3.Distance(ΕΧ,ΑΪ)>ΕА) ΑΪ=ΕΧ+κ*ΕА;if(!Ƃ.CanScan(ΑΪ)) continue;MyDetectedEntityInfo Ƒ=Ƃ.Raycast(ΑΪ);bool Θξ=!Ƒ.IsEmpty()&&!ŕ.Contains(Ƒ.EntityId)&&!ΗΧ(Ƒ.EntityId)&&
Ƒ.Type!=MyDetectedEntityType.FloatingObject;if(Θξ){if(!ŗ) ΕΥ=DateTime.Now;ŗ=true;ΓΔ=0;ΒЎ=Ƒ.Type.ToString();}else if(ŗ&&++ΓΔ>=3) ƴ();break;}if(ŗ&&(DateTime.Now-ΕΥ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΒЎ;}const String ΕΔ="[PAMTFC]";const float Ητ=5f;const float ΒΠ=2f;class Ȧ{public long λ;public Vector3 ζ,ť;public Vector3 Ɛ;public float Εδ;public String ġ="";public int Γα=-1;public bool ΓЅ;public bool Γδ;public float Ņ,ƌ;public long ά=0;public int Ƈ=0;public DateTime ΔΗ;
}IMyBroadcastListener ΑІ=null;long ъ=0;List<Ȧ>м=new List<Ȧ>();Vector3 Ć=new Vector3();float Ǳ=-1;Vector3 ΔΛ=new Vector3();bool ΑЅ=false;DateTime ΔΜ=DateTime.Now;const float ΕΓ=15f;const float Ησ=10f;int Ζσ=0;String С="";int Ё=-1;bool ΓΗ=false;Vector3 ΕΫ=new Vector3();String ű()
{if(ή==Κ.з||Η==Ζ.Ϊ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+М+"|"+б+"|"+(int) Ģ+"|"+Math.Round(ǰ,1)+"|"+Math.Round(ǯ,1);}void ΗЂ(){if(!trafficEnabled) return;if(ΑІ==null) ΑІ=IGC.RegisterBroadcastListener(ΕΔ);ъ=Me.CubeGrid.EntityId;int Ɓ=30;while(ΑІ.HasPendingMessage&&Ɓ-->0){String Ϋ=ΑІ.AcceptMessage().Data as String;if(Ϋ!=null) Ηί(Ϋ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΔΗ).TotalSeconds>Ητ) м.RemoveAt(i);if(ΑΝ==null) ΑΝ=IGC.RegisterBroadcastListener(ΗΨ);Ɓ=10;while(ΑΝ.HasPendingMessage&&Ɓ-->0){String Ϋ=ΑΝ.AcceptMessage().Data as String;if(Ϋ!=null) try{Ηέ(Ϋ);}catch{}}if(++Ζσ<3) return;Ζσ=0;IGC.SendBroadcastMessage(ΕΔ,ъ+";"+τ(ΰ)+";"+
τ(Φ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ű()+";"+
(ή==Κ.Ϗ?Ё:-1)+";"+С+";"+ΖЇ()+","+Δι()+";"+τ(ΔΛ)+
";"+(ΑΥ?1:0)+";"+(Ğ&&ή==Κ.Ϗ?1:0)+","+Ş+","+Ɵ+","+(Δψ()?1:0));}void Ηί(String Ϋ){try{String[]f=Ϋ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ъ) return;Ȧ Ї=null;for(int i=0;i<м.Count;i++) if(м[i].λ==λ) Ї=м[i];if(Ї==null){Ї=new Ȧ();Ї.λ=λ;м.Add(Ї);}
Ї.ζ=μ(f[1]);Ї.ť=μ(f[2]);Ї.Εδ=float.Parse(f[3]);Ї.ġ=f[4];Ї.Γα=int.Parse(f[5]);Ї.ΔΗ=DateTime.Now;if(Ї.ġ!=""&&Ї.ġ==ű()) ΔІ(f[6]);Ї.ά=0;Ї.Ƈ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.Ƈ=int.Parse(d[1]);}Ї.Ɛ=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЅ=f.Length>=10&&f[9]=="1";Ї.Γδ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.Ņ=float.Parse(w[1]);Ї.ƌ=float.Parse(w[2]);Ї.Γδ=w[3]=="1";}}}catch{}}bool ΗΧ(long λ){for(int i=0;i<м.Count;i++) if(м[i].λ==λ) return true;return false;}Vector3 Δσ(int Β,int Θρ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Χ Ψ=Σ[Β];Vector3 ΙΕ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θρ;Vector3 ώ=Ψ.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.ν):-Ψ.β;Vector3 Ь=Vector3.Cross(ΙΕ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ό=laneOffset*θ;float ΘΟ=Math.Min(Vector3.Distance(Ψ.Δ,Σ[0].Δ),
Vector3.Distance(Ψ.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΟ/(ό*4));}Vector3 Ζς=new Vector3();bool Δψ(){return ή==Κ.Ϗ&&(Υ==Α.ȋ||Υ==Α.ĵ||Υ==Α.Ǹ);}bool ΗΦ(Α s){return s==Α.Ǡ||s==Α.ĉ||s==Α.č||s==Α.ŵ||
s==Α.Ǒ||s==Α.Ǹ||s==Α.ȡ;}void ΗЃ(){Ć=new Vector3();Ǳ=-1;ΔΛ=new Vector3();bool Εσ=Δώ(Υ);if(!trafficEnabled||(!Εσ&&!ΗΦ(Υ))){ΑЅ=false;return;}Ζς=Φ.GetShipVelocities().LinearVelocity;ΖЈ(Φ.GetNaturalGravity(),!Εσ);}void ΖЈ(Vector3 ν,bool ΖΡ=false){Ć=new Vector3();Ǳ=-1;
Vector3 ǭ=φ-ΰ;float Ǭ=ǭ.Length();bool Γή=Ǭ>=1f;Vector3 κ=Γή?ǭ/Ǭ:new Vector3();ΔΛ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɖ;Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-ƕ:Vector3.Normalize(Ь);Ȧ Αη=null;bool Ευ=Υ==Α.Ѕ&&!ȏ(1);
for(int i=0;i<м.Count;i++){Ȧ ί=м[i];if(ί.Ƈ==3&&ί.ť.LengthSquared()<0.25f) continue;Vector3 ζ=ί.ζ+ί.ť*(float)(DateTime.Now-ί.ΔΗ).TotalSeconds;Vector3 ř=ζ-ΰ;float ѓ=(θ+ί.Εδ)*0.5f;bool ΖΫ=ί.Γδ&&Δψ()&&ί.Ņ>0&&Ş>0&&ί.ġ==ű();if(ΖΫ) ѓ=Math.Max(Ş+ί.Ņ,Ɵ+ί.ƌ)*0.5f;
float ż=Γή?Vector3.Dot(ř,κ):0;if(ż>0&&(ř-κ*ż).Length()<=ѓ*1.2f){float Αφ=Vector3.Dot(ί.ť,κ);bool Εϋ=ί.Ɛ.LengthSquared()>0.25f;bool Γί=Αφ<-1f||(Εϋ&&Vector3.Dot(ί.Ɛ,κ)<-0.5f);if(ΖΡ){if(Γί&&ъ<ί.λ) continue;}else{if(ί.ΓЅ){if(!Ευ){Ć=Ь*ѓ*1.3f;continue;}
float ΖΗ=Vector3.Distance(ΰ,φ),ΕІ=Vector3.Distance(ζ,φ);if(ΖΗ<ΕІ||(ΖΗ==ΕІ&&ъ<ί.λ)) continue;}if(Γί){if(ż<(Ȝ-Αφ)*6f+ѓ*4f) Ć=Ь*ѓ*1.3f;continue;}if(Εϋ&&ъ<ί.λ){Vector3 ΕΠ=-ř;float ΖΝ=Vector3.Dot(ΕΠ,ί.Ɛ);if(ΖΝ>0&&(ΕΠ-ί.Ɛ*ΖΝ).Length()<=ѓ*1.2f){Ć=Ь*ѓ*1.3f;continue;}
}}float Ετ=ż-ѓ-(ΖΫ?1f:followGap*θ);float Γν=Math.Max(0,Αφ)+Math.Max(0,Ετ)/ΒΠ;if(Γί) Γν=Math.Max(0,Ετ+Math.Min(0,Αφ)*ΒΠ*2)/ΒΠ;if(Ǳ<0||Γν<Ǳ){Ǳ=Γν;Αη=ί;}continue;}if(ί.ť.LengthSquared()<1f) continue;if(Γή&&(ř-κ*ż).Length()<=ѓ*1.2f) continue;Vector3 ΓЊ=ί.ť-Ζς;
float ΖΦ=ΓЊ.LengthSquared();if(ΖΦ<0.25f) continue;float t=-Vector3.Dot(ř,ΓЊ)/ΖΦ;if(t<=0||t>5f||(ř+ΓЊ*t).Length()>=ѓ*1.1f) continue;if(ъ>ί.λ||t<2f){Ǳ=0;Αη=ί;}}bool ΘΧ=Ǳ>=0&&Ǳ<0.5f;if(!ΘΧ||ΖΡ||(Αη!=null&&Αη.ΓЅ&&Ευ)){ΑЅ=false;return;}if(!ΑЅ){ΑЅ=true;ΔΜ=DateTime.Now;}
double Εό=(DateTime.Now-ΔΜ).TotalSeconds;if(Εό>ΕΓ+Ησ) ΔΜ=DateTime.Now;else if(Εό>ΕΓ){Ǳ=-1;Ć=Ь*θ*1.3f;Μ="Traffic: passing a stuck ship";}}List<int>ȃ=new List<int>();String Εώ="";void ΒΟ(){String ģ=М+"|"+б+"|"+(int) Ģ;if(ģ==Εώ) return;Εώ=ģ;Ζώ(М,б,Ģ,ȃ);
}void Ζώ(int w,int h,ǐ ΓБ,List<int>ΓΒ){ΓΒ.Clear();if(ΓБ==ǐ.ǌ){for(int k=0;k<w*h;k++){int Ś=k/w;ΓΒ.Add(Ś*w+(Ś%2==0?k-Ś*w:w-1-(k-Ś*w)));}return;}int Εϊ=(int) Math.Ceiling(w/2f),Εω=(int) Math.Ceiling(h/2f);int ΘΣ=w/2,ΘΡ=h/2;int x=0,y=0,ΓΟ=0,ΓΠ=-1,Αν=Math.Max(w,h);
for(int i=0;i<Αν*Αν;i++){if(-Εϊ<x&&x<=ΘΣ&&-Εω<y&&y<=ΘΡ) ΓΒ.Add((y-1+Εω)*w+(x-1+Εϊ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΟ;ΓΟ=-ΓΠ;ΓΠ=t;}x+=ΓΟ;y+=ΓΠ;}}bool ΗΜ(int a,int b,int w,float Ζυ,float Ζξ,float Ņ,float ƌ){return a==b||(Math.Abs(a%w-b%w)*Ζυ<Ņ+0.5f&&Math.Abs(a/w-b/w)*Ζξ<ƌ+0.5f);
}bool ΒΞ(){int n=Math.Max(0,М*б);if(С.Length==n) return false;С=new String('0',n);Ё=-1;return true;}void ΔІ(String ί){ΒΞ();if(ί.Length!=С.Length) return;char[]h=С.ToCharArray();for(int i=0;i<h.Length;i++) if(ί[i]=='1') h[i]='1';С=new String(h);}bool ƽ(int Ǿ){return Ǿ>=0&&Ǿ<С.Length&&С[Ǿ]=='1';}
void Βφ(int Ǿ,bool ф){if(Ǿ<0||Ǿ>=С.Length) return;char[]h=С.ToCharArray();h[Ǿ]=ф?'1':'0';С=new String(h);}bool Βή(int Ǿ,bool Θέ){if(!trafficEnabled) return false;String ģ=ű();for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ġ!=ģ||t.Γα<0||(Θέ&&t.λ>ъ)) continue;if(ΗΜ(Ǿ,t.Γα,М,ǰ,ǯ,Ş,Ɵ)) return true;
}return false;}int ΖІ(int Β){int n=М*б;if(n<1) return Β;ΒΟ();if(ΒΞ()) for(int k=0;k<Β&&k<n;k++) Βφ(ȃ[k],true);if(Ё>=0&&(Β>=n||ȃ[Β]!=Ё)){Βφ(Ё,true);Ё=-1;}if(ΔЀ()){if(ǧ>=0&&!ƽ(ǧ)){Ğ=false;Ё=ǧ;return ȃ.IndexOf(ǧ);}if(ǧ==-2){Ğ=false;Ё=-1;return n;}
if(ǧ==-1&&Ё>=0&&!ƽ(Ё)){Ğ=false;return Β;}Ё=-1;if(!Ğ){Ğ=true;Εέ=DateTime.Now;}if((DateTime.Now-Εέ).TotalSeconds<5) return-1;}Ğ=false;if(Ё>=0&&!ƽ(Ё)&&!Βή(Ё,true)) return Β;Ё=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(ƽ(c)||Βή(c,false)) continue;Ё=c;
return k;}return n;}const String ΗΨ="[PAMJOB]";IMyBroadcastListener ΑΝ=null;DateTime Εά=DateTime.MinValue;DateTime Εέ=DateTime.Now;int ǧ=-1;bool Ğ=false;bool ΔЀ(){return trafficEnabled&&ǧ!=-3&&(DateTime.Now-Εά).TotalSeconds<3;}void Ηέ(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ű()) return;
Εά=DateTime.Now;ΔІ(f[2]);ǧ=-3;String ΖΓ=ъ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΖΓ)) ǧ=int.Parse(e[i].Substring(ΖΓ.Length));}Ĺ Ηλ(int Β){ΒΟ();if(Β<0||Β>=ȃ.Count) return Ĺ.Δά;Ш=ȃ[Β]%М;Щ=ȃ[Β]/М;return Ĺ.Δν;}class ѐ{public long λ;
public Vector3 ζ,В,ώ;}List<ѐ>Ў=new List<ѐ>();bool ȫ=false;Vector3 ΑΧ,ΑΦ,ΑΨ,ΑΩ;long х=0;long ȅ=0;Χ љ=null;bool ƈ=false;Χ ΒΛ(Vector3 ζ,Vector3 В,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ζ,В,ώ);Χ w=new Χ(Θ);w.γ=Vector3D.TransformNormal(ΑΧ,m);w.β=Vector3D.TransformNormal(ΑΦ,m);
w.ψ=Vector3D.TransformNormal(ΑΨ,m);w.Δ=ζ+(Vector3) Vector3D.TransformNormal(ΑΩ,m);w.δ=true;return w;}int Δι(){Α s=Υ;if(s==Α.ж) return О(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.č||s==Α.ǻ||s==Α.Č||s==Α.ΑΕ||
s==Α.Ʒ||s==Α.Ǒ) return 3;if(s==Α.Ǡ||s==Α.ĉ||s==Α.ŵ) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЇ(){if(Δι()==0) return 0;if(ȏ(1)&&ş!=0) return ş;return љ!=null?ȅ:х;}void ΔЃ(){if(Η==Ζ.Ϊ||!Θ.δ) return;IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector ί=И.OtherConnector;Vector3I ΘΨ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==ΘΨ){MatrixD Ŋ=MatrixD.Transpose(ί.WorldMatrix);ΑΧ=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,Ŋ);ΑΦ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,Ŋ);ΑΨ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,Ŋ);
ΑΩ=Vector3D.TransformNormal(И.GetPosition()-ί.GetPosition(),Ŋ);ȫ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) х=ί.EntityId;List<IMyShipConnector>Ĝ=new List<IMyShipConnector>();ω.GetBlocksOfType(Ĝ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ĝ.Count>0){Ў.Clear();for(int i=0;i<Ĝ.Count;i++){ѐ p=new ѐ();p.λ=Ĝ[i].EntityId;p.ζ=Ĝ[i].GetPosition();p.В=Ĝ[i].WorldMatrix.Forward;p.ώ=Ĝ[i].WorldMatrix.Up;Ў.Add(p);}}љ=null;ȅ=0;if(ί.EntityId==х||!ȫ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==ί.EntityId){ȅ=Ў[i].λ;љ=ΒΛ(Ў[i].ζ,Ў[i].В,Ў[i].ώ);}}bool Βα(long λ,int ΖΘ){if(λ==0) return false;for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ά==λ&&(t.Ƈ>ΖΘ||(t.Ƈ==ΖΘ&&t.λ<ъ))) return true;}return false;}bool ΒΖ(int ΓЂ){if(Η==Ζ.Ϊ||!trafficEnabled){љ=null;ȅ=0;return true;}
long η=љ!=null?ȅ:х;if(!Βα(η,ΓЂ)) return true;if(!Βα(х,ΓЂ)){љ=null;ȅ=0;return true;}if(!ȫ) return false;ѐ ė=null;float ΕΤ=0;for(int i=0;i<Ў.Count;i++){ѐ p=Ў[i];if(p.λ==х||Βα(p.λ,ΓЂ)) continue;float d=Vector3.Distance(p.ζ,Θ.Δ);if(ė==null||d<ΕΤ){ė=p;ΕΤ=d;}
}if(ė==null) return false;ȅ=ė.λ;љ=ΒΛ(ė.ζ,ė.В,ė.ώ);return true;}String ΖА(){String s=(ȫ?"1":"0")+":"+τ(ΑΧ)+":"+τ(ΑΦ)+":"+
τ(ΑΨ)+":"+τ(ΑΩ)+":"+х;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].ζ)+","+τ(Ў[i].В)+","+τ(Ў[i].ώ);return s;}void ΖЏ(String s){Ў.Clear();ȫ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;ȫ=f[0]=="1";ΑΧ=μ(f[1]);ΑΦ=μ(f[2]);ΑΨ=μ(f[3]);ΑΩ=μ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();if(!long.TryParse(e[0],out p.λ)) continue;p.ζ=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Ў.Add(p);}}const String ΒΘ="[PAMDCK]";const float ΙΤ=120f;class Δκ{public long Ї;public int Ɠ;public long Γβ;public bool Ιδ;public DateTime Ιϊ,ΙЀ;
}IMyBroadcastListener Ιχ=null;List<Δκ>Ιψ=new List<Δκ>();List<IMyShipConnector>ΙЉ=new List<IMyShipConnector>();List<IMySensorBlock>ΙБ=new List<IMySensorBlock>();List<IMyCameraBlock>ΙϏ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΙА=new List<MyDetectedEntityInfo>();long Ιη=0;
int Ιζ=0;long Ιε=0;DateTime Ιθ=DateTime.Now;String Ιφ="";int Ιω=0;IMyCubeGrid ΙЊ=null;IMyBroadcastListener ΑЁ=null;int Ȃ=0;DateTime Εθ=DateTime.Now;DateTime Ζί=DateTime.MinValue;long Ζΰ=0;int Ζα=0;long ş=0;bool Ƭ=false;int ΓΞ=0;List<Vector3>Ʈ=new List<Vector3>();List<float>ΔΪ=new List<float>();
bool ΑΥ=false;bool ΑΤ=false;Vector3 Εη=new Vector3();bool Ÿ(){return Ȃ!=0&&(DateTime.Now-Ζί).TotalSeconds<3;}bool ȏ(int Ɠ){return Ÿ()&&Ζΰ==ъ&&Ζα==Ɠ;}bool ΔА(){return Ÿ()||(DateTime.Now-Εθ).TotalSeconds<3;}void Βτ(int Ɠ){if(Ȃ==Ɠ) return;Ȃ=Ɠ;Εθ=DateTime.Now;ΓΞ=3;
}void Βσ(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΘ,"D;"+ъ);Ȃ=0;Ƭ=false;}void Ηϊ(){if(!trafficEnabled||Η==Ζ.Ϊ) return;if(ΑЁ==null) ΑЁ=IGC.RegisterBroadcastListener(ΒΘ);int Ɓ=20;while(ΑЁ.HasPendingMessage&&Ɓ-->0){String Ϋ=ΑЁ.AcceptMessage().Data as String;if(Ϋ==null||!Ϋ.StartsWith("S;")) continue;
try{Ηή(Ϋ);}catch{}}if(Υ!=Α.Ѕ){ΑΥ=false;ΑΤ=false;}if(Ȃ==1&&О(MyShipConnectorStatus.Connected)!=null) Βσ();if(Ȃ==2&&!Ƭ&&Υ!=Α.Ǒ&&
Vector3.Distance(ΰ,е().Δ)>θ*3) Βσ();if(Ȃ!=0&&++ΓΞ>=3){ΓΞ=0;IGC.SendBroadcastMessage(ΒΘ,"R;"+ъ+";"+Ȃ+";"+х+";"+(ȫ?1:0));}}void Ηή(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<7) return;bool Ες=false;String[]ΕΖ=f[5].Split(',');for(int i=0;i<ΕΖ.Length;i++) if(ΕΖ[i]==ъ.ToString()) Ες=true;
if(!Ες) return;Ζί=DateTime.Now;Ζΰ=long.Parse(f[2]);Ζα=int.Parse(f[3]);ş=long.Parse(f[4]);Ʈ.Clear();ΔΪ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʈ.Add(μ(e[0]));ΔΪ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εκ=f[6].Split(':');List<ѐ>Т=new List<ѐ>();for(int i=0;i<Εκ.Length;i++){String[]e=Εκ[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();p.λ=long.Parse(e[0]);p.ζ=μ(e[1]);p.В=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖЄ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;
Βτ(1);if(!ΔА()) return true;if(!ȏ(1)){Μ="Waiting for dock clearance";return false;}ΑЏ();return true;}bool ΑЏ(){ƈ=false;ѐ Ħ=null;if(ş!=0&&ş!=х&&ȫ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==ş) Ħ=Ў[i];long ΑЋ=Ħ==null?0:Ħ.λ;if(ΑЋ==ȅ&&(ΑЋ==0)==(љ==null)) return false;ȅ=ΑЋ;љ=Ħ==null?null:ΒΛ(Ħ.ζ,Ħ.В,Ħ.ώ);return true;}bool Ĵ(Vector3 p){for(int i=0;i<Ʈ.Count;i++) if(Vector3.Distance(p,Ʈ[i])<ΔΪ[i]+θ*0.75f) return true;
return false;}bool ΗΛ(Vector3 Ƅ,Vector3 œ){ΑΥ=false;if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return false;if(Ȃ!=1&&Vector3.Distance(ΰ,Θ.Δ)>dockRequestDistance) return false;Βτ(1);if(!Ÿ()||ȏ(1)||Ʈ.Count==0) return false;if(!Ĵ(Ƅ)&&!Ĵ(œ)&&!Ĵ(ΰ)) return false;ΑΥ=true;return true;
}bool Δγ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;Βτ(2);if(!ΔА()) return true;if(!ȏ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΙΪ=600f;class ΔЂ{public long λ;public int Ǿ=-1;public bool ΑЋ;public float Ņ,ƌ;public DateTime ΙЃ;}class ΔЁ{public String ģ;
public int w,h;public float Ζυ,Ζξ;public String ф;public List<int>ΙЂ=new List<int>();public List<ΔЂ>ΙЅ=new List<ΔЂ>();}List<ΔЁ>Ιώ=new List<ΔЁ>();IMyBroadcastListener Ιύ=null;int Ιό=0;void ΖЌ(string ђ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}