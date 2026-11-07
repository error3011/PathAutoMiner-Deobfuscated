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
public enum Ζ{ǋ,Ű,Ǎ,р,Ϊ}public enum Α{ж,ȥ,ű,Ȍ,Đ,Ѕ,Ю,
Ȥ,Ĳ,Ǡ,č,ĉ,Ƞ,
Ų,Ǻ,ǐ,Č,ΑΑ,Ʋ,
ΒϏ,ǌ,Ũ,Ƴ,ȧ}public enum Λ{з,Ĵ,Є,ΒΚ,Ʈ,ƽ,ƾ,Į,į}public enum п{None,Č,ж,Δε}public enum Ω{Main,ΔЇ,ů,Ů,Βϋ,Ƭ,
ΑЋ,Δυ,Δφ,Δτ,Ηλ,Ιΰ,
Ιή,Ιί,Βφ,ΔЋ,ǆ}public enum а{Ǆ,Ŧ,Ην,Δψ,Ϊ}public enum Ǐ{ǈ,ΖϏ}public enum д{ǃ,ΒΠ,ΔЍ,Ū}public enum Ϗ{Βδ,Δβ,ΒΕ,ΒΛ,ư,Βέ,Ʒ}public enum ю{ΒΗ,ΑА,ΑБ}public enum З{ū,ΗΜ,Ċ}public enum ι{Βζ,Βκ,Βλ,ΔЄ,ΔЃ,
Βε,ǁ,ǀ,Βθ,
Βι,Βη}public enum ΰ{ǳ,ȋ,ǲ,ƫ,ΑЎ,ΑЏ}public enum ķ{Δκ,ΔΩ,ΙΤ}public enum с{Ǉ,Δθ,ΗΨ,Βγ}public enum Ǵ{None,Βο,Ηέ,ΗΧ}public enum σ{ŷ,ƪ,ƻ,Ū,Βα,Ƽ,Ββ,Ης,Βμ}public enum ΙΨ{ǋ,ΙΪ,ΙΩ,Ιά,ΙΥ,ΙΣ,ΙΫ}class Χ{public bool γ=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 ψ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Ў=0;public float ΙЉ=0;public float[]Ǟ=null;public Χ(){}public Χ(Χ δ){γ=δ.γ;Δ=δ.Δ;α=δ.α;β=δ.β;ψ=δ.ψ;ν=δ.ν;
Ч=δ.Ч;Ǟ=δ.Ǟ;}public Χ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 ψ,Vector3 ν){this.Δ=Δ;this.α=α;this.β=β;this.ψ=ψ;this.Ў=0;this.ν=ν;}public void Ζό(List<IMyThrust>ΑΔ,List<string>Ȇ){Ǟ=new float[Ȇ.Count];for(int i=0;i<Ǟ.Length;i++)
Ǟ[i]=-1;for(int i=0;i<ΑΔ.Count;i++){string э=Δς(ΑΔ[i]);int Β=Ȇ.IndexOf(э);if(Β!=-1)
Ǟ[Β]=Џ(ΑΔ[i].MaxEffectiveThrust,ΑΔ[i].MaxThrust);}}}class Βξ{public Χ Љ=null;public List<Vector3>Ş=new List<Vector3>();public float ƃ=0;public float ΔΜ=0;public float ļ=0;public float ΓΣ=0;public Vector3 ΑΧ=new Vector3();}class ĸ{public ι Ћ=ι.Βζ;public float ǔ=0;public float Ȯ=0;
public string Ǖ="";public string ǘ="";DateTime ΕЇ;public bool ΓЀ=false;private bool ΓΖ=false;public bool ΒΒ(bool ȅ){if(ȅ){Ȯ=0;ΓΖ=false;return false;}ΓΖ=true;return Ȯ>ǔ;}public void Ηή(){ΒΒ(true);ΓЀ=false;}public void Ηώ(){if(ΓΖ)
if((DateTime.Now-ΕЇ).TotalSeconds>1){Ȯ++;ΕЇ=DateTime.Now;}}public bool ΒΪ(){switch(Ћ){case ι.Βκ:return true;case ι.Βλ:return true;}return false;}}class я{public String υ="";public String ς="";public int Ǒ=0;public З Ō=З.Ċ;public я(String υ,String ς,int Ǒ,З Ō){this.υ=υ;this.ς=ς;
this.Ǒ=Ǒ;this.Ō=Ō;}}class ΖЋ{public ΖЋ(Vector3 ζ,float ό){this.ζ=ζ;this.ό=ό;}public Vector3 ζ;public float ό;}class Ķ{public DateTime Θά;public DateTime ΕΥ;public String λ="";public String υ="";public String ΙЋ="";public String Μ="";public String ΙЅ="";public String Ιώ="";public String Γώ="";
public Vector3 Δ=new Vector3();public List<я>Ъ=new List<я>();public Ζ Η=Ζ.ǋ;public Ǵ ȩ;public float ŏ;public float Ł;public int ї;public int Ις=0;public int ΙЈ=0;public bool ΙΦ(){return(DateTime.Now-Θά).TotalSeconds>10;}public bool ΗΟ(){if(ȩ!=Ǵ.Βο) return false;return(DateTime.Now-ΕΥ).TotalSeconds>=4;
}public Ķ(String λ){this.λ=λ;}public Ǵ Ιέ(String Ę,bool Θύ,bool Ǔ,bool ΗЌ){if(Ę==""&&!Ǔ) return Ǵ.None;if(ȩ==Ǵ.Βο&&ΗΟ())
ȩ=Ǵ.ΗΧ;if(Θύ){Γώ=Ę;ȩ=Ǵ.Βο;ΕΥ=DateTime.Now;return ȩ;}else if(Ǔ){Γώ="";ȩ=Ǵ.None;}else if(Γώ==Ę){if(ΗЌ) ȩ=Ǵ.Ηέ;return ȩ;}return Ǵ.None;}}IMyGridTerminalSystem ω;Vector3 Αυ=new Vector3();Vector3 ί=new Vector3();Vector3 ΑΪ=new Vector3();Vector3 Ƒ=new Vector3();Vector3 Ɔ=new Vector3();
DateTime ΕІ=new DateTime();bool Ɖ=true;int ΔΓ=0;bool ā=false;bool Αβ=false;bool Ɣ=true;bool Ζχ=false;bool ŉ=false;bool Αί=false;float ț=0;float Ĩ=0;int Ą=0;int ƣ=0;int Αψ=0;int ΒЎ=0;float Ŏ=0;float Ľ=0;double Γτ=0;float Ż=0;List<int>ņ=new List<int>();List<int>ř=new List<int>();
Ζ Η=Ζ.ǋ;int ύ=0;float ǰ=0;float ǯ=0;float θ=0;float Αϊ=0,Αω=0;IMyRemoteControl Υ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΖ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>ȇ=new List<IMyThrust>();List<IMyGyro>ΑΫ=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>ȃ=new List<IMyLandingGear>();List<IMyReactor>ă=new List<IMyReactor>();List<IMyConveyorSorter>Αύ=new List<IMyConveyorSorter>();List<IMyGasTank>є=new List<IMyGasTank>();List<IMyTerminalBlock>Ÿ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ŀ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȩ=new List<IMyBatteryBlock>();List<IMyTextPanel>ѕ=new List<IMyTextPanel>();List<IMyTextSurface>ą=new List<IMyTextSurface>();List<IMyTextPanel>Ƃ=new List<IMyTextPanel>();
IMyTerminalBlock љ=null;Χ Θ=new Χ();Χ Γα=new Χ();Χ Γδ=new Χ();bool Ȉ=false;bool Š=false;bool š=false;bool Αα=false;List<String>Ȇ=new List<string>();List<Χ>Σ=new List<Χ>();List<Χ>ƨ=new List<Χ>();int ΓΛ=0;double Ζψ=0;Χ Ε=new Χ();Vector3 Ǜ;Vector3 Ș;Λ ή=Λ.з;Ǐ ġ=Ǐ.ǈ;int М=0;
int б=0;double Ǝ=0;bool ΑΜ=false;ĸ ϊ=new ĸ();ĸ ϋ=new ĸ();д л=д.ǃ;bool ć=true;Ǐ Ȝ=Ǐ.ǈ;int й=3;int и=3;int Ц=30;ю у=ю.ΒΗ;bool ȟ=true;bool ň=true;bool ł=true;bool ĩ=false;Ϗ Л=Ϗ.Βδ;bool Ȟ=true;float Ǫ=90;float ч=20;float Ы=5;float ш=20;float Ǥ=0.70f;float Ť=1.50f;
float ţ=2.50f;float ĭ=0f;float ĝ=0f;Α Φ;Λ ц;Α Ɲ;Βξ ε=null;int Ш=0;int Щ=0;int Й=0;int Ȗ=0;int Ħ=30;int У=0;bool Ё=true;Vector3 щ;double К=0;bool Αχ=false;int ΔΠ=0;int Ђ=0;int ΒІ=0;int ǫ=0;int ΖΚ=0;Vector3 Ɩ=new Vector3();float к=0;float Э=0;int Ĭ=0;Χ Αώ=null;
float Γη=0;float ΑΖ=0;float Р=0;float Ĥ=0;bool Γβ=false;bool Ī=false;bool Ƥ=false;bool ȓ=false;bool ȏ=false;DateTime ΘΙ=new DateTime();Χ ę=null;Χ ΕΪ=null;float ǻ=0;п ǥ;float Ȕ=0;float ī=0;String Γο="";float Αδ=0;float Ł=0;float ŏ=0;List<я>Ǘ=new List<я>();float Н=0;
bool ƛ=false;bool Ζμ=false;bool ΖΩ=false;bool ΑЁ=false;bool ΖΪ=false;float Γς=0;float Γύ=0;Vector3 Εξ=new Vector3();Vector3 φ=new Vector3();Vector3 ΑЂ=new Vector3(1,1,1);float ΔΒ=1;Vector3 Ơ=new Vector3();bool Αΰ=false;bool ΓΩ=false;bool Γμ=false;bool ΕΕ=false;float ΑΚ=0;
float ΑΓ=2;Vector3 Ζκ;Vector3 ΔΑ;Vector3 ΓБ;float[,]ǟ=new float[3,2];Dictionary<String,float[,]>ќ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζυ=new Dictionary<String,float>();int ΒЉ=0;Dictionary<String,float[]>ș=new Dictionary<string,float[]>();float ΓЁ;bool ΑΝ=false;
int ΑΕ=0;int Ȳ=0;int Αέ=0;int ğ=0;String Μ="";int[]ΖΒ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΖΓ=false;int Αξ=0;int ї=0;Ω Ќ=Ω.Main;int ΘЉ=0,ΘЊ=0,ΘЋ=0,ΘЌ=0;int Θω=0,Θϊ=0;bool Ŀ=false;bool ΕΖ=false;String ΓЈ="";String ΒЍ="";IMyBroadcastListener Ȑ=null;
bool Εν=true;List<Ķ>Ικ=null;Ķ ΙЁ=null;int Ιλ=0,Ιμ=0;int Ιν=0,Ιξ=0;int Ιο=0,Ιπ=0,Ιρ=0;String ΘΫ="";Exception ȕ=null;bool Αφ=false;Λ ś=Λ.з;bool Εέ=false;bool ΕЅ=true;bool ΓІ=false;String Ζφ="";bool Ƣ=false;bool ΓЍ=true;int Αή=0;int[]ΙЄ=null;Vector3 ΒЏ=new Vector3();
bool Ευ=false;bool ǎ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΫ(Me,controllerTag,true))
Η=Ζ.р;π(Ω.Main);if(Η!=Ζ.р){Μ="Welcome to [PAM]!";с ΓЅ=Ηί();if(Η==Ζ.ǋ){List<IMyShipDrill>Εθ=new List<IMyShipDrill>();List<IMyShipGrinder>Εφ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Εθ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(Εφ,q=>q.CubeGrid==Me.CubeGrid);if(Εθ.Count>0)
{Η=Ζ.Ű;Μ="Miner mode enabled!";}else if(Εφ.Count>0){Η=Ζ.Ǎ;Μ="Grinder mode enabled!";}else{Η=Ζ.Ϊ;Ǹ(а.Ϊ);}}if(ΓЅ==с.Ǉ) Ɣ=false;if(ΓЅ==с.Δθ) Μ="Data restore failed!";if(ΓЅ==с.Βγ) Μ="New version, wipe data";}}void Main(string ђ,UpdateType ΙΓ){try{if(ȕ!=null){if(ђ.Trim().ToUpper()=="RETRY")
{ȕ=null;Ɖ=true;Ą=0;if(Η!=Ζ.р) χ();Μ="Restarted after error";}else{ΗΗ();ΔЅ();return;}}Αβ=(ΙΓ&UpdateType.Update10)!=0;if(Αβ) ΔΓ++;ā=ΔΓ>=10;if(ā) ΔΓ=0;if(Αβ){ΑΕ++;if(ΑΕ>4) ΑΕ=0;Αψ=(int) Math.Max(0,10-(DateTime.Now-ΕІ).TotalSeconds);}if(ђ!="") ΘΫ=ђ;if(Η!=Ζ.р)
Ημ(ђ);else
ΖЉ(ђ);ΖΓ=false;try{int ΕΫ=Runtime.CurrentInstructionCount;float Ɨ=Џ(ΕΫ,Runtime.MaxInstructionCount);if(Ɨ>0.90) Μ="Max. instructions >90%";if(Ɨ>Ŏ) Ŏ=Ɨ;if(ΑΝ){ņ.Add(ΕΫ);while(ņ.Count>10) ņ.RemoveAt(0);Ľ=0;for(int i=0;i<ņ.Count;i++) Ľ+=ņ[i];Ľ=Џ(Џ(Ľ,ņ.Count),Runtime.MaxInstructionCount);
double Γθ=Runtime.LastRunTimeMs;if(Αί&&Γθ>Γτ) Γτ=Γθ;ř.Add((int)(Γθ*1000f));while(ř.Count>10) ř.RemoveAt(0);Ż=0;for(int i=0;i<ř.Count;i++) Ż+=ř[i];Ż=Џ(Ż,ř.Count)/1000f;}}catch{Ŏ=0;}}catch(Exception e){ȕ=e;}}void Ημ(string ђ){bool Αϋ=false;String ΕΡ="";if(ύ<=1&&!Δϋ(ђ))
ΔІ(ђ);int Θζ=50;while(Ȑ!=null&&Ȑ.HasPendingMessage&&Θζ-->0){MyIGCMessage ρ=Ȑ.AcceptMessage();String Ϋ=ρ.Data as String;if(Ϋ==null) continue;String ƚ="";if(ύ<=1&&ΗΩ(ref Ϋ,out ƚ,out ΕΡ)&&ƚ==BROADCAST_SELF_ID){ΔІ(Ϋ);Αϋ=true;break;}}bool ΕЀ=Αί&&Φ==Α.ж&&!ŉ&&!Αϋ&&Ą==0&&!Αα;
if(ā&&Φ!=Α.ж) Αϋ=true;if((Αβ&&!ΕЀ)||(ā&&ΕЀ)){if(Ą==0&&(Αψ<=0||Ɖ)){Ζχ=ύ>0;ύ=0;Ą=1;ǹ();Ηβ();ΗΝ();ǵ("Scan 1");}else if(Ą==1){Ą=2;ǹ();Ηγ();ǵ("Scan 2");}else if(Ą==2){Ą=0;ǹ();Ηδ();Ηε();if(ύ<=1) ΔϏ();ǵ("Scan 3");ΕІ=DateTime.Now;if(ύ<=1&&Ɣ)
Αυ=Ƶ(Υ,Υ.CenterOfMass);Ɣ=false;if(Ɖ){Ɖ=false;bool ΙΗ=ή==Λ.Є;χ();if(ΕЅ&&resumeAfterRestart&&ΙΗ&&ύ<=1) ΓІ=true;}ΕЅ=false;if(Ζχ&&ύ==0) Μ="Setup complete";}else{if(ή==Λ.Є&&Η!=Ζ.Ϊ){ǹ();Ζω();ǵ("Inv balance");}ǹ();switch(ƣ){case 0:Ηφ();break;case 1:Ηϊ();break;case 2:Ηυ();break;
case 3:ΗЁ();break;case 4:Ηω();break;case 5:ΖЀ();break;case 6:Ζώ(Υ);break;}ǵ("Update: "+ƣ);ƣ++;if(ƣ>6){ƣ=0;Αί=true;if(ś!=Λ.з){switch(ś){case Λ.Į:Ȣ();break;case Λ.į:Ȣ();break;case Λ.Є:Ȣ();break;case Λ.ƽ:ƴ();break;case Λ.ƾ:Ď();break;}ś=Λ.з;}}}if(!Ɖ){if(!Ƿ(Υ,true)){Υ=null;
Ɣ=true;ύ=2;}if(ύ>=2&&Φ!=Α.ж)
χ();if(ύ<=1){Ĩ=Υ.CalculateShipMass().PhysicalMass;ț=(float) Υ.GetShipSpeed();ί=Ητ(Υ,Αυ);ΑΪ=Υ.WorldMatrix.Forward;Ƒ=Υ.WorldMatrix.Left;Ɔ=Υ.WorldMatrix.Down;Ηύ();ΗϏ();Ηχ();if(ΓІ){ΓІ=false;Ȣ();if(Φ!=Α.ж) Μ="Job resumed after restart";}if(Φ!=Α.ж){ŉ=false;ΔЊ(false);Δέ(false);String ΘЂ=ΒΦ(Φ)+" "+(int) Φ;
ǹ();Ηϋ();Βώ(false);ǵ(ΘЂ);ΗЀ();Ηό();ǹ();Ηψ();ǵ("Thruster");ǹ();ΕΑ();ǵ("Gyroscope");}else{if(ŉ){if(ƹ()){Х(Ɔ,ΑΪ,Ƒ,0.25f,true);ΕΑ();Μ="Aligning to planet: "+Math.Round(ΑΚ-0.25f,2)+"°";if(Αΰ) ΒΩ(true,true);}else ΒΩ(true,true);}}Ƣ=false;}}}ǹ();ΔЅ();ǵ("Print");if(Αϋ||ΒЎ<=0){ǹ();Ζϊ(ΕΡ);
ǵ("Broadcast");ΒЎ=4;}else if(ā) ΒЎ--;}void ΔІ(string ђ){if(ђ=="") return;var Д=ђ.ToUpper().Split(' ');Д.DefaultIfEmpty("");var Ę=Д.ElementAtOrDefault(0);var Ė=Д.ElementAtOrDefault(1);var ź=Д.ElementAtOrDefault(2);var ΕΙ=Д.ElementAtOrDefault(3);String ȳ="Invalid argument: "+ђ;bool ΔΛ=false;
switch(Ę){case "UP":this.ΔЁ(false);break;case "DOWN":this.ΔЀ(false);break;case "UPLOOP":this.ΔЁ(true);break;case "DOWNLOOP":this.ΔЀ(true);break;case "APPLY":this.ΔΫ(true);break;case "MRES":ї=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.Βϊ();}break;case "PATH":{this.χ();this.Βϊ();Θ.γ=true;}break;
case "START":{this.χ();Βω();}break;case "ALIGN":{ΒΩ(!ŉ,false);}break;case "CONT":{this.χ();this.Ȣ();}break;case "JOBPOS":{this.χ();this.Ď();}break;case "HOMEPOS":{this.χ();this.ƴ();}break;case "FULL":{ƛ=true;}break;case "RESET":{Αφ=true;ύ=2;}break;case "EXPORT":{ΖА();}break;
case "IMPORT":{ΗΚ(Ė=="JOB");}break;default:ΔΛ=true;break;}if(Η!=Ζ.Ϊ){switch(Ę){case "SHUTTLE":{ΖЎ();}break;case "CFGS":{if(!ΖЇ(Ė,ź,ΕΙ)) Μ=ȳ;}break;case "CFGB":{if(!ΖІ(Ė,ź)) Μ=ȳ;}break;case "CFGL":{if(!ď(ref Ǫ,true,σ.ƻ,Ė,"")||!ΖЈ(ź))
Μ=ȳ;}break;case "CFGE":{if(!ď(ref Ы,true,σ.Ƽ,Ė,"IG")||
!ď(ref ч,true,σ.Βα,ź,"IG")||
!ď(ref ш,true,σ.Ββ,ΕΙ,"IG"))
Μ=ȳ;}break;case "CFGA":{if(!ď(ref Ǥ,false,σ.ƪ,Ė,"")) Μ=ȳ;}break;case "CFGW":{if(!ď(ref Ť,false,σ.ŷ,Ė,"")||
!ď(ref ţ,false,σ.ŷ,ź,""))
Μ=ȳ;}break;case "NEXT":{ΑЊ(false);}break;case "PREV":{ΑЊ(true);}break;default:if(ΔΛ) Μ=ȳ;break;}}else{switch(Ę){case "UNDOCK":{Ƣ=true;}break;default:if(ΔΛ) Μ=ȳ;break;}}}String ΗΓ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΖЎ(){χ();Η=Ζ.Ϊ;Ǹ(а.Ϊ);Ε.γ=false;Θ.γ=false;ξ=null;ο.Clear();ή=Λ.з;}void ΒΩ(bool Ї,bool ф){if(!Ї) Μ="Aligning canceled";if(ф) Μ="Aligning done";if(ф||!Ї){ŉ=false;ΔЎ();ǅ(false,0,0,0,0);return;}if(ƹ()) ŉ=true;}bool ΖЇ(String ΙΙ,String ΘΣ,String ΘΔ)
{bool ΙΕ=ή==Λ.Є;int w,h,d;if(int.TryParse(ΙΙ,out w)&&int.TryParse(ΘΣ,out h)&&int.TryParse(ΘΔ,out d)){this.χ();й=w;и=h;Ц=d;ē(false);Ŷ(false,false);if(ΙΕ) Ȣ();return true;}return false;}bool ΖЈ(String ΕΘ){if(ΕΘ=="ON"){ȟ=true;return true;}if(ΕΘ=="OFF"){ȟ=false;return true;}
return false;}bool ΖІ(String Εη,String ΑΛ){bool ƕ=true;if(Εη=="HOME") ć=true;else if(Εη=="STOP") ć=false;else ƕ=false;if(ΑΛ=="HOME") л=д.ǃ;else if(ΑΛ=="STOP") л=д.ΔЍ;else if(ΑΛ=="JOB") л=д.ΒΠ;else if(ΑΛ=="IG") л=д.Ū;else ƕ=false;return ƕ;}void π(Ω Γϋ){ΖΒ[(int) Ќ]=ї;
ї=ΖΒ[(int) Γϋ];if(Γϋ==Ω.Ηλ) ї=0;Ќ=Γϋ;if(Η!=Ζ.р)
Ē(Ќ==Ω.ů,false,0,0);ΖΓ=true;}void ΔЁ(bool Γν){if(ї>0) ї--;else if(Γν) ї=Αξ-1;}void ΔЀ(bool Γν){if(ї<Αξ-1) ї++;else if(Γν) ї=0;}String Ǹ(а ρ){switch(ρ){case а.Ǆ:Μ="Job is running";break;case а.Ŧ:Μ="Connector not ready!";break;case а.Ην:Μ="Ship modified, path outdated!";break;
case а.Δψ:Μ="Interrupted by player!";break;case а.Ϊ:Μ="Shuttle mode enabled!";break;}return "";}String ΗΖ(Ǐ ζ){switch(ζ){case Ǐ.ǈ:return "Top-Left";case Ǐ.ΖϏ:return "Center";default:return "";}}String ΗΑ(ю Ɠ){switch(Ɠ){case ю.ΑА:return "Auto"+(Η==Ζ.Ű?" (Ore)":"");case ю.ΑБ:return "Auto (+Stone)";
case ю.ΒΗ:return "Default";default:return "";}}String ΒΤ(Λ Ф){switch(Ф){case Λ.з:return "No job";case Λ.Ĵ:return "Job paused";case Λ.Є:return "Job active";case Λ.Į:return "Job active";case Λ.į:return "Job active";case Λ.ΒΚ:return "Job done";case Λ.Ʈ:return "Job changed";case Λ.ƽ:return "Move home";
case Λ.ƾ:return "Move to job";}return "";}String Δν(д ΗЋ){switch(ΗЋ){case д.ǃ:return "Return home";case д.ΒΠ:return "Fly to job pos";case д.ΔЍ:return "Stop";case д.Ū:return "Ignore";}return "";}String ΗΒ(Ϗ Ɠ){switch(Ɠ){case Ϗ.Βδ:return "Off";case Ϗ.ΒΛ:return "Drop pos (Stone) ";case Ϗ.ư:return "Drop pos (Sto.+Ice)";
case Ϗ.Δβ:return "Cur. pos (Stone)";case Ϗ.ΒΕ:return "Cur. pos (Sto.+Ice)";case Ϗ.Βέ:return "In motion (Stone)";case Ϗ.Ʒ:return "In motion (Sto.+Ice)";}return "";}String Δλ(п Ф){switch(Ф){case п.None:return "No batteries";case п.Č:return "Charging";case п.Δε:return "Discharging";}return "";}String ΒΦ(Α Ф)
{String Ιψ=Η==Ζ.Ϊ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ȥ:return "Flying to XY position";case Α.ű:return Η==Ζ.Ǎ?"Grinding":"Mining";case Α.Ȍ:return "Returning";case Α.Ũ:return "Flying to drop pos";case Α.Đ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.Ȥ:return "Flying to path";case Α.Ĳ:return "Flying to job position";case Α.Ǡ:return "Approaching dock";case Α.č:return "Docking";case Α.ĉ:return "Aligning";case Α.Ƞ:return "Aligning";case Α.Ų:return "Retry docking";case Α.Ǻ:return "Unloading";case Α.ȧ:return Ζφ;
case Α.ǐ:return "Undocking";case Α.Č:return "Charging batteries";case Α.ΑΑ:return "Waiting for uranium";case Α.Ʋ:return "Filling up hydrogen";case Α.ΒϏ:return "Waiting for ejection";case Α.ǌ:return "Waiting for ejection";case Α.Ƴ:return "Flying to drop pos";}return "";}String Δσ(ι Ћ){switch(Ћ)
{case ι.Βζ:return "On \"Undock\" command";case ι.Βκ:return "On player entered cockpit";case ι.ΔЄ:return "On ship is full";case ι.ΔЃ:return "On ship is empty";case ι.Βλ:return "On time delay";case ι.ǁ:return "On batteries empty(<25%)";case ι.ǀ:return "On batteries empty(=0%)";case ι.Βε:return "On batteries full";
case ι.Βι:return "On hydrogen empty(<25%)";case ι.Βη:return "On hydrogen empty(=0%)";case ι.Βθ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΥ,int ȱ,bool Ξ,String Ɛ){Αξ+=1;if(ΖΥ==ȱ) Ɛ=">"+Ɛ+(ΑΕ>=2?" .":"");else Ɛ=" "+Ɛ;Γ+=Ɛ+"\n";return ΖΥ==ȱ&&Ξ;}String ΔΫ(bool Ξ)
{int Β=0;int Ο=ї;Αξ=0;String ΖΨ="———————————————\n";String ş="--------------------------------------------\n";String Γ="";Γ+=ΒΤ(ή)+" | "+(Θ.γ?"Ready to dock":"No dock")+"\n";Γ+=ΖΨ;double Ǭ=Math.Max(Math.Round(this.К),0);if(Ќ==Ω.Main){bool Ā=Η==Ζ.Ϊ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βϊ();
if(Η==Ζ.Ű)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ω.ů);if(Η==Ζ.Ǎ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ω.ů);if(Η==Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ω.Βφ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ȣ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƴ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ď();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ā) π(Ω.ǆ);else π(Ω.Ƭ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ω.Δυ);if(Η!=Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ω.Δτ);}else if(Ќ==Ω.ů){double ΙΘ=Math.Round(й*ǰ,1);double ΘΡ=Math.Round(и*ǯ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) Βω();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){Ŷ(false,false);π(Ω.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΘ+"m)")){İ(ref й,5,20,1);ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){İ(ref й,-5,20,-1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΘΡ+"m)")){İ(ref и,5,20,1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){İ(ref и,-5,20,-1);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(у==ю.ΒΗ?"Depth":"Min")+": "+Ц+"m)")){İ(ref Ц,5,50,2);ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){İ(ref Ц,-5,50,-2);ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+ΗΑ(у))){у=ȣ(у);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΗΖ(Ȝ))){Ȝ=ȣ(Ȝ);}if(Η==Ζ.Ǎ&&у==ю.ΑБ) у=ȣ(у);Γ+=ΔЈ(8,Ѓ,Ο,ref Θω);}else if(Ќ==Ω.Βφ){float[]Εί=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.ΔЋ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δσ(ϊ.Ћ))) ϊ.Ћ=ȣ(ϊ.Ћ);if(!ϊ.ΒΪ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΣ((int) ϊ.ǔ))) ϊ.ǔ=Δγ(ϊ.ǔ,Εί);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δσ(ϋ.Ћ))) ϋ.Ћ=ȣ(ϋ.Ћ);
if(!ϋ.ΒΪ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΣ((int) ϋ.ǔ))) ϋ.ǔ=Δγ(ϋ.ǔ,Εί);}else if(Ќ==Ω.ΔЋ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βω();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Βφ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.Ǖ!=""?ϊ.Ǖ:"-"))) ϊ.Ǖ=ƿ(ref ΘЉ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϊ.ǘ!=""?ϊ.ǘ:"-"))) ϊ.ǘ=ƿ(ref ΘЋ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǖ!=""?ϋ.Ǖ:"-"))) ϋ.Ǖ=ƿ(ref ΘЊ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǘ!=""?ϋ.ǘ:"-"))) ϋ.ǘ=ƿ(ref ΘЌ);
}else if(Ќ==Ω.Ů){String ΕЋ=Ǫ+" %";if(ā) Ȳ++;if(Ȳ>1){Ȳ=0;Αέ++;if(Αέ>1) Αέ=0;}bool[]ΓЊ=new bool[]{ă.Count==0,ǥ==п.None,є.Count==0};int Ζρ=0;while(true){Ζρ++;ğ++;if(ğ>ΓЊ.Length-1) ğ=0;if(Ζρ>=ΓЊ.Length) break;if(!ΓЊ[ğ]) break;}bool Ā=Η==Ζ.Ϊ;if(!Ā&&ȟ&&Н!=-1&&Αέ==0)
ΕЋ=Н<1000000?Math.Round(Н)+" Kg":Math.Round(Н/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ā) π(Ω.Ƭ);else π(Ω.ǆ);if(!Ā){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЊ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƣ=true;Γ+=ş;if(!Ā) Γ+="Progress: "+Math.Round(Ǝ,1)+" %\n";Γ+="State: "+ΒΦ(Φ)+" "+Ǭ+"m \n";Γ+="Load: "+Αδ+" % Max: "+ΕЋ+" \n";if(ğ==0) Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(ğ==1) Γ+="Battery: "+(ǥ==п.None?Δλ(ǥ):ǻ+"% "+(ч==-1||Ā?"":" Min: "+ч+" %"))+"\n";if(ğ==2) Γ+="Hydrogen: "+(є.Count==0?"No tanks":Math.Round(Ȕ,1)+"% "+(ш==-1||Ā?"":" Min: "+ш+" %"))+"\n";}else if(Ќ==Ω.Ƭ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.Ů);else π(Ω.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+Ǫ+"%")) г(ref Ǫ,Ǫ<=80?-10:-5,σ.ƻ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(ȟ?"On":"Off"))) ȟ=!ȟ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+ΗΒ(Л))) Л=ȣ(Л);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(ĩ?"On":"Off"))){ĩ=!ĩ;if(ĩ) ǣ(ΓЍ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(Ȟ?"On":"Off"))) Ȟ=!Ȟ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.Ƽ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Min "+ч+"%"))) г(ref ч,-5,σ.Βα,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-10,σ.Ββ,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(ć?"Return home":"Stop"))) ć=!ć;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δν(л))) л=ȣ(л);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ω.ΑЋ);Γ+=ΔЈ(8,Ѓ,Ο,ref Θϊ);}else if(Ќ==Ω.ΑЋ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.Ů);else π(Ω.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǎ?" Grinder":" Drill")+" inv. balancing: "+(ň?"On":"Off"))) ň=!ň;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǎ?" grinders":" drills")+": "+(ł?"Fwd + Bwd":"Fwd"))) ł=!ł;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ť+"m/s")) г(ref Ť,0.5f,σ.ŷ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ţ+"m/s")) г(ref ţ,0.5f,σ.ŷ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) г(ref Ǥ,0.1f,σ.ƪ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ĭ*100f+"%")) Δί(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ĝ*100f+"%")) Δί(false,0.05f);
}else if(Ќ==Ω.ǆ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ή==Λ.Є) π(Ω.Ů);else π(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǫ+"%")) г(ref Ǫ,Ǫ<=80?-10:-5,σ.ƻ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȟ?"On":"Off"))) Ȟ=!Ȟ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.Ƽ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ч==-1?"Ignore":"Charge up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ш==-1?"Ignore":"Fill up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δν(л))) л=ȣ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǥ*100f)+"%"+(Ǥ>0.80f?" (risky)":""))) г(ref Ǥ,0.1f,σ.ƪ,false);
}else if(Ќ==Ω.ΔЇ){double Θέ=0;if(Σ.Count>0) Θέ=Vector3.Distance(Σ.Last().Δ,ί);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΔЏ();if(Η!=Ζ.Ϊ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȉ?"Use old home":(Θ.γ?"Was set! ":"none ")))) Ȉ=!Ȉ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȉ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) Ȉ=!Ȉ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Š?"Use old connector":(Ε.γ?"Was set! ":"none ")))) Š=!Š;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(š?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) š=!š;Γ+=ş;Γ+="Wp spacing: "+Math.Round(Ζψ)+"m\n";}else if(Ќ==Ω.Βϋ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();π(Ω.Main);}
Γ+=ş;Γ+="State: "+ΒΦ(Φ)+" \n";Γ+="Speed: "+Math.Round(ț,1)+"m/s\n";Γ+="Target dist: "+Ǭ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΠ+"\n";}else if(Ќ==Ω.Δυ){List<IMyTerminalBlock>Ě=Δξ();if(ā) Ȳ++;if(Ȳ>=Ě.Count) Ȳ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ω.Δφ);
Γ+=ş;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αδ,1)+"% "+Math.Round(Ł,1)+" / "+Math.Round(ŏ,1)+"\n";Γ+="Uranium: "+(ă.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+Γο)+"\n";Γ+="Battery: "+(ǥ==п.None?"":ǻ+"% ")+Δλ(ǥ)+"\n";Γ+="Hydrogen: "+(є.Count==0?"No tanks":Math.Round(Ȕ,1)+"% ")+"\n";
Γ+="Damage: "+(Ě.Count==0?"None":""+(Ȳ+1)+"/"+Ě.Count+" "+Ě[Ȳ].CustomName)+"\n";}else if(Ќ==Ω.Δφ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=ş;Γ+="Next scan: "+Αψ+"s\n";Γ+="Drill area: "+Math.Round(ǰ,1)+"x"+Math.Round(ǯ,1)+"m Ship: "+Math.Round(Αϊ,1)+"x"+Math.Round(Αω,1)+"m ("+Math.Round(θ,1)+"m)\n";
Γ+="Broadcast: "+(Ŀ?"Online - "+ΓЈ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ŏ*100f,1)+"% \n";}else if(Ќ==Ω.Δτ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ω.Main);Γ+=ş;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+Αψ+"s\n";if(Αφ) Γ="Recompile script now";int Θή=Γ.Split('\n').Length;for(int i=Θή;i<=10;i++) Γ+="\n";Γ+=ΖΨ;Γ+="Last: "+Μ+"\n";
return Γ;}void Δί(bool ΘΩ,float ћ){χ();Ŷ(true,false);if(ΘΩ) г(ref ĭ,ћ,σ.Βμ,false);else г(ref ĝ,ћ,σ.Βμ,false);Δή();Ē(true,true,0,0);}float Δγ(float η,float[]ΑЈ){float έ=ΑЈ[0];for(int i=ΑЈ.Length-1;i>=0;i--)
if(η<ΑЈ[i]) έ=ΑЈ[i];return έ;}String ƿ(ref int ȱ){String έ="";if(ȱ>=ΔΖ.Count) ȱ=-1;if(ȱ>=0) έ=ΔΖ[ȱ].CustomName;ȱ++;return έ;}void ǉ(string υ){if(ή!=Λ.Є) return;if(υ=="") return;IMyTerminalBlock Τ=ω.GetBlockWithName(υ);if(Τ==null||!(Τ is IMyTimerBlock)){Μ="Timerblock "+υ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void İ(ref int value,int Ž,int ȝ,int ΓЋ){if(Ž==0) return;if(value<ȝ&&ΓЋ>0||value<=ȝ&&ΓЋ<0){value+=ΓЋ;return;}int ΗЃ=Math.Abs(Ž);int ΒЀ=0;int Γφ=1;while(true){ΒЀ+=Γφ*ΗЃ*10;if(Ž<0&&value-ȝ<=ΒЀ) break;if(Ž>0&&value-ȝ<ΒЀ) break;
Γφ++;}value+=Γφ*Ž;}void ē(bool ΙЊ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ē(Ќ==Ω.ů,false,0,0);}bool ď(ref float value,bool Θφ,σ ς,String Ġ,String ΘΦ){if(Ġ=="") return false;float Ŕ=-1;bool Εύ=false;if(Ġ.ToUpper()==ΘΦ) Εύ=true;else if(!float.TryParse(Ġ,out Ŕ)) return false;
else Ŕ=Math.Max(0,Ŕ);if(Θφ) Ŕ=(float) Math.Round(Ŕ);г(ref value,Ŕ,ς,Εύ,false);return true;}void г(ref float value,float ћ,σ ς,bool ơ){г(ref value,value+ћ,ς,ơ,true);}void г(ref float value,float Ő,σ ς,bool ơ,bool ΙΜ){float ǚ=0,Ǚ=0;if(ς==σ.ŷ){ǚ=0.5f;Ǚ=10f;}if(ς==σ.ƪ){ǚ=0.1f;Ǚ=1f;}
if(ς==σ.Ū){ǚ=50f;Ǚ=100f;}if(ς==σ.Βα){ǚ=5f;Ǚ=30f;}if(ς==σ.Ƽ){ǚ=1f;Ǚ=25f;}if(ς==σ.ƻ){ǚ=10f;Ǚ=95f;}if(ς==σ.Ββ){ǚ=10f;Ǚ=90f;}if(ς==σ.Ης){ǚ=10f;Ǚ=1800;}if(ς==σ.Βμ){ǚ=0.0f;Ǚ=0.75f;}if(Ő==-1&&ơ){value=-1;return;}if(value==-1) ơ=false;
bool ΖΝ=Ő<ǚ||Ő>Ǚ;if(ΖΝ&&ΙΜ){if(Ő<value) value=Ǚ;else if(Ő>value) value=ǚ;}else value=Ő;if(ΖΝ&&ơ) value=-1;else value=Math.Max(ǚ,Math.Min(value,Ǚ));value=(float) Math.Round(value,2);}void ΑЊ(bool Θρ){if(Θρ) Й=Math.Max(0,Й-1);else Й++;ΒΝ();ΒΜ();for(int k=0;k<Ȃ.Count&&k<С.Length;k++) Βτ(Ȃ[k],k<Й);
Ѐ=-1;Βώ(true);}T ȣ<T>(T η){int Β=Array.IndexOf(Enum.GetValues(η.GetType()),η);Β++;if(Β>=ΖЏ(η)) Β=0;return(T) Enum.GetValues(η.GetType()).GetValue(Β);}int ΖЏ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βϊ(){ƨ.Clear();for(int i=0;i<Σ.Count;i++) ƨ.Add(Σ[i]);Σ.Clear();
Αα=true;Γα=new Χ(Θ);Γδ=new Χ(Ε);Θ.γ=false;if(Η==Ζ.Ϊ) Ε.γ=false;for(int i=0;i<ќ.Count;i++)
if(!Ȇ.Contains(ќ.Keys.ElementAt(i)))
Ȇ.Add(ќ.Keys.ElementAt(i));Ȉ=false;Š=false;š=false;π(Ω.ΔЇ);}void ΔЏ(){if(Ȉ) Θ=Γα;if(Š) Ε=Γδ;if(š){Σ.Clear();for(int i=0;i<ƨ.Count;i++) Σ.Add(ƨ[i]);}Αα=false;χ();π(Ω.Main);}void Ηύ(){if(!Αα) return;if(Φ!=Α.ж){ΔЏ();return;}if(!Γα.γ) Ȉ=false;if(!Γδ.γ) Š=false;if(ƨ.Count<=1) š=false;
IMyShipConnector И=О(MyShipConnectorStatus.Connectable);if(И==null) И=О(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(ț,2)<=0.20) ΓΛ++;else ΓΛ=0;if(ΓΛ>=5){if(Η==Ζ.Ϊ&&(Θ.γ||Ȉ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.β=Υ.WorldMatrix.Forward;Ε.ψ=Υ.WorldMatrix.Left;
Ε.α=Υ.WorldMatrix.Down;Ε.ν=Υ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.γ=true;Ε.Ч=И.Position;}else{Θ.β=Υ.WorldMatrix.Forward;Θ.ψ=Υ.WorldMatrix.Left;Θ.α=Υ.WorldMatrix.Down;Θ.ν=Υ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.γ=true;Θ.Ч=И.Position;}}}double Γζ=-1;if(Σ.Count>0) Γζ=Vector3.Distance(ί,Σ.Last().Δ);
double ΘЀ=Math.Max(1.5,Math.Pow(ț/100.0,2));double ΖΔ=Math.Max(ț*ΘЀ,2);Ζψ=ΖΔ;if((Γζ==-1)||Γζ>=ΖΔ){Χ Ψ=new Χ(ί,ΑΪ,Ɔ,Ƒ,Υ.GetNaturalGravity());Ψ.Ζό(ȇ,Ȇ);Σ.Add(Ψ);}}Χ е(){if(Η!=Ζ.Ϊ) return ј!=null?ј:Θ;return ΕΪ;}void ĵ(Χ ά,Λ Ф){ΕΪ=ά;if(ή==Λ.Є) ц=Ф;}bool Ƕ(Χ ά,float ό,bool ΙΔ,out Vector3 ζ)
{if(ΙΔ){Vector3I ΓΨ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(ΓΨ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){ζ=new Vector3();return false;}Vector3 ΗБ=Я(Υ,Τ.FatBlock.GetPosition()-ί);Vector3 ΗА=Я(Υ,Τ.FatBlock.WorldMatrix.Forward);ζ=ά.Δ-Βύ(ά.β,ά.α*-1,ΗБ)-Βύ(ά.β,ά.α*-1,ΗА)*ό;
return true;}else{ζ=ά.Δ;return true;}}Vector3 Ȋ(int ΗЎ,int Ř,bool Θτ){if(!Θτ&&Ευ) return ΒЏ;float ΗЏ=((М-1f)/2f)-ΗЎ;float Θχ=((б-1f)/2f)-Ř;ΒЏ=Ε.Δ+Ε.ψ*ΗЏ*ǰ+Ș*-1*Θχ*ǯ;Ευ=true;return ΒЏ;}Vector3 ΒΥ(Vector3 ΘЄ,float ΓΘ){return ΘЄ+(Ǜ*ΓΘ);}ΰ ΒΘ(){float А=-1;ΰ έ=ΰ.ǳ;
if(Η!=Ζ.Ϊ){ΔϏ();if(ή!=Λ.з){Vector3 Ј=đ(Ǜ,Ș*-1,ί-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(М*ǰ)/2f&&Math.Abs(Ј.Y)<=(float)(б*ǯ)/2f){if(Ј.Z<=-1&&Ј.Z>=-Ħ*2) return ΰ.ƫ;if(Ј.Z>-1&&Ј.Z<θ*2) return ΰ.ǳ;}if(ȡ(Ε.Δ,ref А)) έ=ΰ.ǳ;}if(Θ.γ){if(ȡ(Θ.Δ,ref А)) έ=ΰ.ǲ;for(int i=0;i<Σ.Count;i++)
if(ȡ(Σ[i].Δ,ref А)) έ=ΰ.ȋ;if(Vector3.Distance(ί,Θ.Δ)<dockDist*θ) έ=ΰ.ǲ;if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǲ;}}else{Vector3 ζ=new Vector3();IMyShipConnector ǿ=О(MyShipConnectorStatus.Connected);if(Θ.γ){if(ȡ(Θ.Δ,ref А)) έ=ΰ.ǲ;if(Ƕ(Θ,dockDist,true,out ζ))
if(ȡ(ζ,ref А)) έ=ΰ.ǲ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Θ.Δ)<5)
return ΰ.ΑЎ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*θ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*θ)
if(ȡ(Σ[i].Δ,ref А)) έ=ΰ.ȋ;if(Ε.γ){if(ȡ(Ε.Δ,ref А)) έ=ΰ.ǳ;if(Ƕ(Ε,dockDist,true,out ζ))
if(ȡ(ζ,ref А)) έ=ΰ.ǳ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5)
return ΰ.ΑЏ;}}return έ;}bool ȡ(Vector3 ζ,ref float А){float ό=Vector3.Distance(ζ,ί);if(ό<А||А==-1){А=ό;return true;}return false;}void Βω(){if(ύ>0){Μ="Setup error! Can't start";return;}if(Η==Ζ.Ϊ){Ȣ();return;}Ε.Δ=ί;Ε.ν=Υ.GetNaturalGravity();Ε.β=ΑΪ;Ε.α=Ɔ;Ε.ψ=Ƒ;
Ǜ=љ.WorldMatrix.Forward;Ș=Ε.α;if(Ǜ==Υ.WorldMatrix.Down) Ș=Υ.WorldMatrix.Backward;Ŷ(true,true);Ν(Α.ȥ);Βψ();}void Ŷ(bool ǖ,bool Θυ){if(ή==Λ.з&&!ǖ) return;bool Θη=ǖ||ή==Λ.ΒΚ||М!=й||б!=и||ġ!=Ȝ;if(Θη){if(ή!=Λ.з){ή=Λ.Ʈ;Ȋ(Ш,Щ,Θυ);Μ="Job changed, lost progress";}ġ=Ȝ;
М=й;б=и;Щ=0;Ш=0;С="";Ѐ=-1;Ĥ=0;Ȗ=0;Р=0;Й=0;Βώ(true);}}void Βπ(){П(ί,0);ё(ȇ,true);}void Βψ(){Ǹ(а.Ǆ);Βπ();Ŵ(ȃ,false);ǣ(ΓЍ);ή=Λ.Є;Δέ(true);ц=ή;π(Ω.Ů);Ηα();ΑΜ=true;Αή=0;for(int i=Ÿ.Count-1;i>=0;i--)
if(Ŭ(Ÿ[i],false)) Αή++;if(Αή>0) Μ="Started with damage";}void χ(){if(ή==Λ.Є){ή=Λ.Ĵ;Μ="Job paused";}Ν(Α.ж);ц=ή;ǅ(false,0,0,0,0);Βό();ΔΪ(new Vector3(),false);ΔЎ();Βσ(ChargeMode.Auto);Βυ(false);ΔЊ(true);ȍ(Α.ж);Ē(false,false,0,0);ё(ο,false);ё(т,true);ǣ(true);Αχ=false;Ư();ƅ=false;
Βρ();ΑΜ=false;ƛ=false;Ƣ=false;if(Ќ!=Ω.Main&&Ќ!=Ω.Ƭ&&Ќ!=Ω.ΑЋ&&Ќ!=Ω.ǆ)
π(Ω.Main);}void Ȣ(){ΰ Г=ΒΘ();if(Η==Ζ.Ϊ){if(!Ε.γ||!Θ.γ) return;Βψ();bool ΓΦ=Vector3.Distance(ί,Θ.Δ)<Vector3.Distance(ί,Ε.Δ);if(ś==Λ.Į) ΓΦ=true;if(ś==Λ.į) ΓΦ=false;if(ΓΦ){ĵ(Θ,Λ.Į);switch(Г){case ΰ.ΑЎ:Ν(Α.ȧ);break;case ΰ.ȋ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.Ǡ);break;default:Ν(Α.ǐ);break;
}}else{ĵ(Ε,Λ.į);switch(Г){case ΰ.ΑЏ:Ν(Α.ȧ);break;case ΰ.ǳ:Ν(Α.Ǡ);break;case ΰ.ȋ:Ν(Α.Ю);break;default:Ν(Α.ǐ);break;}}}else{if(ή!=Λ.Ĵ&&ή!=Λ.Ʈ) return;bool ΙΖ=ή==Λ.Ʈ;Βψ();bool ΓΫ=ŭ(false)&&Θ.γ;switch(Г){case ΰ.ǳ:Ν(ΓΫ?Α.Ȥ:Α.ȥ);break;case ΰ.ȋ:Ν(ΓΫ?Α.Ѕ:Α.Ю);break;case ΰ.ǲ:Ν(ΓΫ?Α.Ǡ:Α.Ǻ);break;
case ΰ.ƫ:{if(Ȗ!=Й||ΙΖ) Ν(Α.Ȍ);else Ν(Α.ű);}break;default:break;}}}void Ď(){if(ή==Λ.з&&!Θ.γ) return;if(Η==Ζ.Ϊ&&(!Ε.γ||!Θ.γ)) return;Μ="Move to job";ΰ Г=ΒΘ();if(Η==Ζ.Ϊ){ĵ(Ε,Λ.į);switch(Г){case ΰ.ǳ:Ν(Α.Ǡ);break;case ΰ.ȋ:Ν(Α.Ю);break;case ΰ.ΑЏ:return;default:Ν(Α.ǐ);break;
}ȍ(Α.ȧ);}else{switch(Г){case ΰ.ǳ:Ν(Α.Ю);break;case ΰ.ȋ:Ν(Α.Ю);break;case ΰ.ǲ:Ν(Α.Ǻ);break;case ΰ.ƫ:Ν(Α.Ȍ);break;default:break;}if(ή==Λ.з) ȍ(Α.Ю);else ȍ(Α.Ƞ);Αχ=true;}Βπ();π(Ω.Βϋ);Ŵ(ȃ,false);ц=Λ.ƾ;}void ƴ(){if(!Θ.γ) return;Μ="Move home";ΰ Г=ΒΘ();if(Η==Ζ.Ϊ){ĵ(Θ,Λ.Į);switch(Г)
{case ΰ.ȋ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ǡ);break;case ΰ.ΑЎ:return;default:Ν(Α.ǐ);break;}ȍ(Α.ȧ);}else{if(О(MyShipConnectorStatus.Connected)!=null) return;if(О(MyShipConnectorStatus.Connectable)!=null){Ν(Α.č);ȍ(Α.Ǻ);return;}switch(Г){case ΰ.ǳ:Ν(Α.Ȥ);break;case ΰ.ȋ:Ν(Α.Ѕ);break;case ΰ.ǲ:Ν(Α.Ѕ);break;
case ΰ.ƫ:Ν(Α.Đ);break;default:break;}ȍ(Α.Ǻ);}Βπ();π(Ω.Βϋ);Ŵ(ȃ,false);ц=Λ.ƽ;}void Ν(Α Γχ){if(Γχ==Α.ж) Ɲ=Α.ж;if(Ɲ!=Α.ж&&Φ==Ɲ&&Γχ!=Ɲ){χ();return;}Ё=true;Φ=Γχ;}void ȍ(Α Ф){this.Ɲ=Ф;}void Ηϋ(){if(Φ==Α.ȥ){if(Ё){Ђ=0;if(Ȗ!=Й) Ĥ=0;Ȗ=Й;}if(Ђ==0){int ΓΑ=ΖЃ(Й);if(ΓΑ<0)
{if(!ΓΕ){ΓΕ=true;ΕΧ=ί;}П(ΕΧ,0);Μ="Waiting for the controller";return;}ΓΕ=false;if(ΓΑ!=Й){Й=ΓΑ;Ĥ=0;Ȗ=Й;}ķ έ=Ηθ(Й);if(έ==ķ.ΔΩ){ή=Λ.ΒΚ;bool ΖΜ=С.IndexOf('0')>=0;Μ=ΖΜ?"No free hole left for this ship":"Job done";if((ć||ΖΜ)&&Θ.γ){Ν(Α.Ȥ);ȍ(Α.Ǻ);ц=Λ.ƽ;}
else{Ν(Α.Ĳ);ȍ(Α.Ƞ);ц=Λ.ƾ;}return;}if(έ==ķ.Δκ){Ђ=1;ё(ο,true);щ=Ȋ(Ш,Щ,true);П(щ,10);Х(Ε.α,Ε.β,Ε.ψ,false);}}else{if(Ѐ>=0&&(Δό()?ǧ>=0&&ǧ!=Ѐ:Βά(Ѐ,true))){Ђ=0;return;}if(К<wpReachedDist){Ν(Α.ű);return;}}}if(Φ==Α.ű){if(Ё){ё(ο,true);ǣ(false);щ=Ȋ(Ш,Щ,false);
П(ΒΥ(щ,0),0);Х(Ε.α,Ε.β,Ε.ψ,false);Ђ=1;к=0;Э=0;ΑΖ=0;Γη=-1;Ħ=Ц;Γβ=true;}if(!ΒΓ()){Ν(Α.Đ);return;}if(ŭ(true)){ΖΚ=ȉ("","ORE",З.Ċ);if((Л==Ϗ.ΒΛ||Л==Ϗ.ư||
Л==Ϗ.Βέ||Л==Ϗ.Ʒ)&&Η!=Ζ.Ǎ)
Ν(Α.Ũ);else if((Л==Ϗ.Δβ||Л==Ϗ.ΒΕ)&&Η!=Ζ.Ǎ)
Ν(Α.ΒϏ);else
Ν(Α.Đ);return;}Р=Vector3.Distance(ί,щ);if(Р>Ĥ){Ĥ=Р;Γβ=false;}if(Η==Ζ.Ǎ&&Δο()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ђ>0&&Ђ<Э){if(Р>к){if(Э>150) к=Р;else к=(float) Math.Ceiling(Р);Ђ=1;}else Ђ++;}else{if(Ђ>0){Μ="Ship stuck! Retrying";к=Р;Ђ=0;Ē(false,true,0,θ*sensorRange);}П(ΒΥ(щ,Math.Max(0,к-θ)),ΒΨ(false));if(К<=wpReachedDist/2){Ђ=1;к=0;}
return;}Ē(false,true,θ*sensorRange,0);Vector3 ΗЅ=щ+Ǜ*Р;bool ΕЁ=false;if(Vector3.Distance(ΗЅ,ί)>0.3f){Vector3 ΘΑ=щ+Ǜ*(Р+0.1f);П(ΘΑ,4);ΕЁ=true;}else{float ǜ=ΒΨ(true);Vector3 Εή=ΒΥ(щ,Math.Max(Ц+1,Р+1));Ǣ(true,false,false,Εή,Εή-щ,ǜ,ǜ);}bool ΓΙ=false;if(у==ю.ΑБ||у==ю.ΑА)
{if(!ΕЁ){float Γω=0;foreach(IMyTerminalBlock Ǯ in ο)
Γω+=ΖЊ(Ǯ,"","",у==ю.ΑА?new string[]{"STONE"}:null);if(Γω>Γη||Р<Ц||Γβ){ΒІ=0;ΑΖ=Р;Ħ=(int)(Math.Max(Ħ,ΑΖ)+θ/2);}else{ΓΙ=Р-ΑΖ>2&&ΒІ>=20;ΒІ++;}Γη=Γω;}}else ΓΙ=Р>=Ħ;if(Ȗ!=Й){Ν(Α.Ȍ);Р=0;return;}if(ΓΙ){Й++;Ν(Α.Ȍ);Р=0;return;}}if(Φ==Α.Ȍ||Φ==Α.Đ||Φ==Α.Ũ)
{if(Ё){щ=Ȋ(Ш,Щ,false);Х(Ε.α,Ε.β,Ε.ψ,false);ё(ο,ł);ǣ(false);к=Vector3.Distance(ί,щ);Ē(false,true,0,θ*sensorRange);}П(щ,ΒΨ(false));if(Vector3.Distance(ί,щ)>=к+5){ё(ο,false);ǣ(true);Μ="Can´t return!";}if(К<wpReachedDist){if(Φ==Α.Ȍ&&Αχ) Ν(Α.Ĳ);if(Φ==Α.Ȍ) Ν(Α.ȥ);
if(Φ==Α.Ũ) Ν(Α.Ƴ);if(Φ==Α.Đ){if(Θ.γ) Ν(Α.Ȥ);else{χ();Ď();Μ="Can´t return, no dock found";}}return;}}if(Φ==Α.Ѕ||Φ==Α.Ю){if(Φ==Α.Ю&&ή==Λ.Є&&Η!=Ζ.Ϊ){if(!ΒΓ()||ŭ(true)){Ν(Α.Ѕ);return;}}bool ΓЃ=false;bool ΑІ=false;bool ΕΚ=false;float ļ=0;bool ΖΙ=false;Χ Ψ=null;
if(Ё){if(Φ==Α.Ѕ||Η==Ζ.Ϊ){if(Η!=Ζ.Ϊ){if(Ȏ(1)) ΑЍ();else{ј=null;Ȅ=0;ƅ=!ΒΔ(1);}}Χ ά=е();ε=new Βξ();ε.Љ=ά;ε.ƃ=followPathDock*θ;ε.ΔΜ=useDockDirectionDist*θ;ε.ļ=10;ε.Ş.Add(ά.Δ);Vector3 Ļ=new Vector3();if(Ƕ(ά,dockDist*θ,true,out Ļ))
ε.Ş.Add(Ļ);else ε.ƃ*=1.5f;if(Η==Ζ.Ϊ){if(ά==Θ) ε.ΑΧ=Ε.Δ;if(ά==Ε) ε.ΑΧ=Θ.Δ;ε.ΓΣ=dockDist*θ*1.1f;}}else if(Φ==Α.Ю){ε=new Βξ();ε.Љ=Ε;ε.ƃ=followPathJob*θ;ε.ΔΜ=useJobDirectionDist*θ;ε.ļ=10;ε.ΑΧ=Θ.Δ;ε.ΓΣ=dockDist*θ*1.1f;ε.Ş.Add(Ε.Δ);if(ή==Λ.з){if(!Θ.γ||Σ.Count==0){χ();return;}
float ΘΗ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΘ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΗ<ΘΘ) ε.Љ=Σ.Last();else ε.Љ=Σ.First();}}Ɩ=new Vector3();ΖΙ=!ΗΠ(ί);ё(ο,false);ǣ(true);У=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ε.ΑΧ)<=ε.ΓΣ) continue;
double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǧ==-1||ό<Ǧ){У=i;Ǧ=ό;}}ǫ=ΗΕ(ε.Љ.Δ,У);ę=null;}if(Φ==Α.Ѕ&&Η!=Ζ.Ϊ&&!Ё&&!ŵ()){Χ ΒЊ=е();ƅ=!ΒΔ(1);if(е()!=ΒЊ){Ν(Α.Ѕ);return;}}if(Φ==Α.Ѕ&&Η!=Ζ.Ϊ&&!Ё&&Ȏ(1)&&ΑЍ()){Ν(Α.Ѕ);return;}bool ΕϏ=false;if(Φ==Α.Ѕ&&!Ё&&У>=0&&У<Σ.Count)
{Vector3 Ɓ=Σ[У].Δ+Δπ(У,ǫ);int Γψ=У+ǫ;Vector3 ő=ǫ!=0&&Γψ>=0&&Γψ<Σ.Count?Σ[Γψ].Δ:ε.Љ.Δ;if(ΗΘ(Ɓ,ő)){if(!ΑΟ){ΑΟ=true;Εγ=ĳ(Ɓ)?ί:Ɓ;}П(Εγ,0);Х(ε.Љ.α,ε.Љ.β,ε.Љ.ψ,false);Μ="Waiting for dock clearance";return;}ΑΟ=false;ΕϏ=Ȏ(1)&&(ĳ(ί)||ĳ(ő));}Ζύ(Σ,ǫ,ε.Ş,ε.ƃ,Ё,ref Ђ);
for(int i=0;i<ε.Ş.Count;i++){float ό=Vector3.Distance(ί,ε.Ş[i]);if(ό<=ε.ƃ) ΓЃ=true;if(ό<=ε.ΔΜ) ΑІ=true;}if(ΕϏ) ΑІ=true;if(ΑІ) ļ=ε.ļ;float ΙЌ=ę!=null?ę.Ў:ț;float ΙΛ=(float) Math.Max(ț*0.1f*θ,wpReachedDist);if((К<ΙΛ)||Ё){if(!Ё) У+=ǫ;if(ǫ==0||У>Σ.Count-1||У<0)
ΓЃ=true;else{ΔΠ=ǫ>0?Σ.Count-1-У:У;Ψ=Σ[У];ę=Ψ;if(У>=1&&У<Σ.Count-1)
Ɩ=Ψ.Δ-Σ[У-ǫ].Δ;else ę=null;φ=Ψ.Δ+Δπ(У,ǫ);ΕΚ=true;}}if(ΑІ)
Х(ε.Љ.α,ε.Љ.β,ε.Љ.ψ,false);else if(ΖΙ)
ΑЌ(ε.Љ.α,10,true);else if(ΕΚ&&Ψ!=null){if(ǫ>0) Х(Ψ.α,Ψ.β,Ψ.ψ,90,false);else Х(Ψ.α,-Ψ.β,-Ψ.ψ,90,false);}Ǣ(true,false,true,φ,Ɩ,ę==null?0:ę.Ў,ļ);if(ΓЃ){ΔΠ=0;if(Φ==Α.Ѕ&&!ΖЁ()){П(φ,0);return;}if(Φ==Α.Ѕ&&ƅ){П(φ,0);Μ="Waiting for a free dock";return;}if(Φ==Α.Ѕ||Η==Ζ.Ϊ){Ν(Α.Ǡ);return;}
if(Φ==Α.Ю&&Αχ){Ν(Α.Ĳ);return;}if(Φ==Α.Ю){Ν(Α.ȥ);return;}}}if(Φ==Α.Ǡ||Φ==Α.Ų){if(Η!=Ζ.Ϊ&&ŵ()){if(!Ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϊ){Χ ΒЊ=е();if(!ΒΔ(2)||е()!=ΒЊ){Ν(Α.Ѕ);return;}}Χ ά=е();if(Ё){if(!Ƕ(ά,dockDist*θ,true,out φ)){Ǹ(а.Ŧ);χ();return;}
П(φ,0);ΑЌ(ά.α,90,true);}if(К<followPathDock*θ&&К!=-1){П(φ,10);Х(ά.α,ά.β,ά.ψ,false);}if(О(MyShipConnectorStatus.Connectable)!=null||О(MyShipConnectorStatus.Connected)!=null){Ν(Α.č);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.ĉ);return;}}if(Φ==Α.ĉ||Φ==Α.Ƞ){if(Ё)
{if(Φ==Α.ĉ){Χ ά=е();if(!Ƕ(ά,dockDist*θ,true,out φ)){Ǹ(а.Ŧ);χ();return;}Ǣ(true,true,false,φ,0);Х(ά.α,ά.β,ά.ψ,10,false);}if(Φ==Α.Ƞ){Х(Ε.α,Ε.β,Ε.ψ,0.5f,false);φ=Ε.Δ;Ǣ(true,true,false,φ,0);}}if(Αΰ){ǅ(false,0,0,0,0);if(Φ==Α.ĉ) Ν(Α.č);if(Φ==Α.Ƞ) χ();return;}}if(Φ==Α.č)
{if(О(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.Ǻ);return;}Χ ά=е();if(Ё){Э=0;ΘΙ=DateTime.Now;Ђ=0;Х(ά.α,ά.β,ά.ψ,false);}Vector3I ΓΨ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock ΕΦ=Me.CubeGrid.GetCubeBlock(ΓΨ);float Εε=dockingSpeed;float Θα=dockingSpeed*5;
float ΕΗ=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ƕ(ά,0,true,out φ)||!Ƕ(ά,ΕΗ,true,out Ɩ)||
ΕΦ==null||!ΕΦ.FatBlock.IsFunctional){Ǹ(а.Ŧ);χ();return;}if(Э==1||(Vector3.Distance(ί,φ)<=ΕΗ*1.1f&&!Ё))
Э=1;else{Vector3 ΘЍ=Я(Υ,Ɩ-ί);Vector3 ΓΧ=Я(Υ,Υ.GetNaturalGravity());float Θψ=ƭ(ΘЍ,ΓΧ,null);Εε=Math.Min(Θα,Θψ);}Ǣ(true,false,false,φ,φ-ί,dockingSpeed,Εε);if(Ё) к=(float) К;IMyShipConnector ΓΔ=О(MyShipConnectorStatus.Connectable);if(ΓΔ!=null){Ǣ(false,false,false,φ,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΓΔ.Connect();if(ΓΔ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϊ) Ν(Α.ȧ);else Ν(Α.Ǻ);Βό();Ŵ(ȃ,true);return;}}}else{float Εβ=(float) Math.Round(К,1);if(Εβ<к){Ђ=-1;к=Εβ;}else Ђ++;if(Ђ>20){Ν(Α.Ų);return;}}}if(Φ==Α.Ǻ||Φ==Α.ȧ||
Φ==Α.ΑΑ||Φ==Α.Ʋ||Φ==Α.Č){bool ΒЄ=false,ΒЅ=false;if(Η==Ζ.Ϊ){if(е()==Θ) ΒЄ=true;else if(е()==Ε) ΒЅ=true;}if(Ё){ƛ=false;if(О(MyShipConnectorStatus.Connected)==null){Ν(Α.ǐ);return;}Βό();if(ΒЄ) ǉ(ϊ.Ǖ);if(ΒЅ) ǉ(ϋ.Ǖ);Ī=false;ȏ=false;ȓ=false;Ƥ=false;}if(О(MyShipConnectorStatus.Connected)==null){χ();Ǹ(а.Δψ);return;}
if(ή!=Λ.Є||ч==-1||ǥ==п.None) ȏ=true;else if(ǻ>=100f) ȏ=true;else if(ǻ<=99f) ȏ=false;if(ή!=Λ.Є||ш==-1||є.Count==0) ȓ=true;else if(Ȕ>=100f) ȓ=true;else if(Ȕ<=99) ȓ=false;if(ή!=Λ.Є||Ы==-1||ă.Count==0) Ƥ=true;else Ƥ=ī>=Ы;ĸ о=null;if(ΒЄ) о=ϊ;
if(ΒЅ) о=ϋ;if(о!=null&&(о.Ћ==ι.ǁ||о.Ћ==ι.ǀ)) ȏ=true;if(о!=null&&(о.Ћ==ι.Βε)) if(!Ī) ȏ=false;if(о!=null&&(о.Ћ==ι.Βι||о.Ћ==ι.Βη)) ȓ=true;if(о!=null&&(о.Ћ==ι.Βθ)) if(!Ī) ȓ=false;if(ā){ChargeMode ΕΤ=ȏ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ћ==ι.ǀ||о.Ћ==ι.ǁ))
ΕΤ=ChargeMode.Discharge;Βσ(ΕΤ);Βυ(!ȓ);}if(!Ī){if(Η==Ζ.Ϊ)
Ī=ή!=Λ.Є||ΖЂ(Ё,true)||Ƣ;else
Ī=ή!=Λ.Є||Δω();}else{if(!ȏ) Ν(Α.Č);if(!ȓ) Ν(Α.Ʋ);if(!Ƥ) Ν(Α.ΑΑ);Ё=false;}if(Ī&&ȏ&&ȓ&&Ƥ){Βσ(ChargeMode.Auto);Βυ(false);if(ή==Λ.Є){if(Η==Ζ.Ϊ){if(е()==Θ) ǉ(ϊ.ǘ);else if(е()==Ε) ǉ(ϋ.ǘ);if(е()==Θ) ĵ(Ε,Λ.į);else ĵ(Θ,Λ.Į);}}Ν(Α.ǐ);return;}}if(Φ==Α.ǐ){if(Ƨ){if(!Δΰ()) return;
Ƨ=false;Ν(Α.ǐ);return;}if(Ё){if(!Δΰ()){Ƨ=true;return;}IMyShipConnector ǿ=О(MyShipConnectorStatus.Connected);if(ǿ==null){Ν(Α.Ю);return;}IMyShipConnector δ=ǿ.OtherConnector;ΔЉ(ǿ,false);Ŵ(ȃ,false);Χ ά=null;Χ Γΰ=Η==Ζ.Ϊ?Θ:е();if(Vector3.Distance(ǿ.GetPosition(),Γΰ.Δ)<5f&&Θ.γ) ά=Γΰ;
if(Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5f&&Ε.γ) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*θ,true,out φ)){Ǹ(а.Ŧ);χ();return;}П(φ,5);Х(ά.α,ά.β,ά.ψ,false);}else П(ί+δ.WorldMatrix.Forward*dockDist*θ,5);if(ή==Λ.Є) Ǹ(а.Ǆ);}if(К<wpReachedDist){ё(т,true);Ν(Α.Ю);return;}}if(Φ==Α.Ĳ)
{if(Ё){ǣ(true);ё(ο,false);φ=Ε.Δ;П(φ,20);Х(Ε.α,Ε.β,Ε.ψ,false);}if(К<wpReachedDist/2){Ν(Α.Ƞ);return;}}if(Φ==Α.Ȥ){if(Ё){ǣ(true);ё(ο,false);int А=-1;double Ǧ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ί);if(Ǧ==-1||ό<Ǧ){А=i;Ǧ=ό;}}
if(А==-1){Ν(Α.Ѕ);return;}φ=Σ[А].Δ;П(φ,10);Х(Ε.α,Ε.β,Ε.ψ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Φ==Α.Ƴ){bool ф=false;if(Ё){ǣ(true);if((Л==Ϗ.ΒΛ||Л==Ϗ.ư)&&ƹ()&&
ı(Ǜ,Υ.GetNaturalGravity())<25&&М>=2&&б>=2){Vector3 Ƈ=ί;if(Ш>0&&Щ<б-1) Ƈ=Ȋ(Ш-1,Щ+1,true);else if(Ш<М-1&&Щ<б-1) Ƈ=Ȋ(Ш+1,Щ+1,true);else if(Ш<М-1&&Щ>0) Ƈ=Ȋ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ƈ=Ȋ(Ш-1,Щ-1,true);else ф=true;if(!ф) П(Ƈ,10);
}else ф=true;}if(К<wpReachedDist/2) ф=true;if(ф){Ν(Α.ǌ);return;}}if(Φ==Α.ΒϏ||Φ==Α.ǌ){if(Ё){Ǣ(true,true,false,ί,0);ё(ο,false);ǣ(true);Ђ=-1;Э=Л==Ϗ.Βέ||Л==Ϗ.Ʒ?0:-1;}bool ě=!ΒΓ();int ΑЀ=ȉ("STONE","ORE",З.Ċ);if(Л==Ϗ.ΒΕ||Л==Ϗ.Ʒ||Л==Ϗ.ư)
ΑЀ+=ȉ("ICE","ORE",З.Ċ);bool ΘΠ=ΑЀ>0;bool ΓΡ=false;if(Э>=0){float Ĺ=(float) Math.Sin(Δδ(Э))*ǰ/3f;float ΗЇ=(float) Math.Cos(Δδ(Э))*ǯ/3f;Vector3 ΗЍ=Ȋ(Ш,Щ,true)+Βύ(Ǜ,Ș*-1,new Vector3(Ĺ,ΗЇ,0));П(ΗЍ,0.3f);if(К<Math.Min(ǰ,ǯ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ђ==-1||ΑЀ<Ђ){Ђ=ΑЀ;к=0;}
else{к++;if(к>50) ΓΡ=true;}if(!ΘΠ||ě||ΓΡ){if(!ě){int ΙΑ=ȉ("","ORE",З.Ċ);if(ŭ(true)) ě=true;else if(100-(Џ(ΙΑ,ΖΚ)*100)<minEjection) ě=true;else Ǹ(а.Ǆ);}if(ΓΡ&&ě) Μ="Ejection failed";if(Φ==Α.ǌ){if(ě){if(Θ.γ) Ν(Α.Ȥ);else{χ();Ď();Μ="Can´t return, no dock found";}}
else Ν(Α.ȥ);}else if(ě) Ν(Α.Đ);else Ν(Α.ű);return;}}Ё=false;}void Ζύ(List<Χ>Ĉ,int κ,List<Vector3>Ɵ,float Ǭ,bool ȅ,ref int ǒ){if(ȅ){for(int i=0;i<Ĉ.Count;i++) Ĉ[i].Ў=0;ǒ=-1;return;}if(κ==0) return;int ћ=κ*-1;if(ǒ==-1) ǒ=ћ>0?1:Ĉ.Count-2;int ΕЃ=0;while(ǒ>=1&&ǒ<Ĉ.Count-1)
{if(ΕЃ>50) return;ΕЃ++;try{if((ћ<0&&ǒ>=1)||(ћ>0&&ǒ<=Ĉ.Count-2)){Χ η=Ĉ[ǒ];bool ΖΗ=false;for(int j=0;j<Ɵ.Count;j++)
if(Vector3.Distance(η.Δ,Ɵ[j])<=Ǭ){ΖΗ=true;break;}if(!ΖΗ){Χ Ασ=Ĉ[ǒ-ћ];Χ ő=Ĉ[ǒ+ћ];Vector3 ΘЎ=η.Δ-ő.Δ;Vector3 ΔΗ=Ασ.Δ-η.Δ;Vector3 Θς=η.Δ+Vector3.Normalize(ΘЎ)*ΔΗ.Length();Vector3 ΘΖ=Ασ.Δ-Θς;Vector3 Αε=đ(κ>0?η.β:η.β*-1,η.α*-1,ΘΖ);Vector3 Θΰ=đ(κ>0?η.β:η.β*-1,η.α*-1,ΔΗ);
Vector3 Αη=đ(κ>0?η.β:η.β*-1,η.α*-1,η.ν);η.Ў=(float) Math.Sqrt(Math.Pow(Ασ.Ў,2)+Math.Pow(ƭ(-Θΰ,Αη,η),2));for(int j=0;j<Ɵ.Count;j++)
if(Vector3.Distance(Ασ.Δ,Ɵ[j])<=Ǭ){Vector3 Αθ=đ(κ>0?η.β:η.β*-1,η.α*-1,Ɵ[j]-η.Δ);float ΘІ=ƭ(-Αθ,Αη,η);η.Ў=Math.Min(η.Ў,ΘІ)/2f;}if(Αε.Length()==0) Αε=new Vector3(0,0,1);Vector3 Θί=đ(η.β,η.α*-1,η.ν);float ΘЈ=ũ(-Αε,Θί,η);float ΑΒ=Џ(ΘЈ,Ĩ);float ΔΕ=(float) Math.Sqrt(Џ(Αε.Length()*1.0f,0.5f*ΑΒ));
η.Ў=Math.Min(η.Ў,(ΔΗ.Length()/ΔΕ)*Ǥ);}}}catch{return;}ǒ+=ћ;}ǒ=-1;}void Βώ(bool ȅ){if(ȅ){Ǝ=0;return;}if(Η==Ζ.Ϊ) return;float ф=Й*Math.Max(1,Ц);if(Ȗ==Й) ф+=Math.Min(Ц,Р);float ΘБ=М*б*Math.Max(1,Ц);Ǝ=Math.Max(Ǝ,(float) Math.Min(ф/ΘБ*100.0,100));}MyDetectedEntityType Δο()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΨ(bool ǩ){if(Η==Ζ.Ǎ&&Δο()==MyDetectedEntityType.None&&!Ŭ(ξ,true))
return fastSpeed;else return ǩ?Ť:ţ;}void Ηυ(){float ΕБ=0,Εά=0,Ġ=0,Γϊ=0;for(int i=0;i<Ȩ.Count;i++){IMyBatteryBlock ż=Ȩ[i];if(!Ƿ(ż,true)) continue;ΕБ+=ż.MaxStoredPower;Εά+=ż.CurrentStoredPower;Ġ+=ż.CurrentInput;Γϊ+=ż.CurrentOutput;}ǻ=(float) Math.Round(Џ(Εά,ΕБ)*100,1);
if(Ġ>=Γϊ) ǥ=п.Č;else ǥ=п.Δε;if(Ġ==0&&Γϊ==0||ǻ==100.0) ǥ=п.ж;if(Ȩ.Count==0) ǥ=п.None;}void Ηω(){float Εμ=0;for(int i=0;i<є.Count;i++){IMyGasTank Ζθ=є[i];if(!Ƿ(Ζθ,true)) continue;Εμ+=(float) Ζθ.FilledRatio;}Ȕ=Џ(Εμ,є.Count)*100f;}void ΗЁ(){ī=0;try{for(int i=0;i<ă.Count;i++)
{IMyReactor ΓЄ=ă[i];if(!Ƿ(ΓЄ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЄ.GetInventory(0).GetItems(Ъ);float Ǒ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem В=Ъ[j];if(В.Type.SubtypeId.ToUpper()=="URANIUM"&&В.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǒ+=(float) В.Amount;}if(Ǒ<ī||i==0){ī=Ǒ;Γο=ΓЄ.CustomName;}}}catch(Exception e){ȕ=e;}}void Ηφ(){ŏ=0;Ł=0;try{for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Τ=ŀ[i];if(!Ƿ(Τ,true)) continue;Ł+=(float) Τ.GetInventory(0).CurrentVolume;ŏ+=(float) Τ.GetInventory(0).MaxVolume;
}Αδ=(float) Math.Min(Math.Round(Џ(Ł,ŏ)*100,1),100.0);}catch(Exception e){ȕ=e;}}void Ηϊ(){try{Ǘ.Clear();for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Τ=ŀ[i];if(!Ƿ(Τ,true)) continue;ΔΨ(Τ,З.ū);}if(Л!=Ϗ.Βδ){for(int i=0;i<ĕ.Count;i++){IMyTerminalBlock Τ=ĕ[i];if(!Ƿ(Τ,true)) continue;
ΔΨ(Τ,З.ΗΜ);}}Ηξ(Ǘ);}catch(Exception e){ȕ=e;}}void ΖЀ(){if(ΑΜ){if(Δξ().Count>Αή){ΑΜ=false;if(л!=д.Ū){χ();if(л==д.ΒΠ) Ď();if(л==д.ǃ)
if(Θ.γ) ƴ();else Ď();}Μ="Damage detected";}}}bool ΒΓ(){if(!Αί) return true;if(ή==Λ.Є){if(ч>0&&ǥ!=п.None)
if(ǻ<=ч){Μ="Low energy! Move home";return false;}if(Ы>0&&ă.Count>0)
if(ī<=Ы){Μ="Low fuel: "+Γο;return false;}if(ш>0&&є.Count>0)
if(Ȕ<=ш){Μ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δξ(){List<IMyTerminalBlock>Ě=new List<IMyTerminalBlock>();for(int i=0;i<Ÿ.Count;i++){IMyTerminalBlock Τ=Ÿ[i];if(Ŭ(Τ,false)) Ě.Add(Τ);}return Ě;}bool Ŭ(IMyTerminalBlock Τ,bool ΓЏ){return(!Ƿ(Τ,ΓЏ)||!Τ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Τ,bool ΓЏ){if(Τ==null) return false;try{IMyCubeBlock Ετ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΓЏ) return Ετ==Τ;else return Ετ.GetType()==Τ.GetType();}catch{return false;}}float ΖЊ(IMyTerminalBlock Τ,String э,String Ζί,String[]Ǩ){float Ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ň=Τ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Ň.GetItems(Ъ);foreach(MyInventoryItem В in Ъ){if(Ǩ!=null&&(Ǩ.Contains(В.Type.TypeId.ToUpper())||Ǩ.Contains(В.Type.SubtypeId.ToUpper())))
continue;if((э==""||В.Type.TypeId.ToUpper()==э)&&(Ζί==""||В.Type.SubtypeId.ToUpper()==Ζί))
Ȁ+=(float) В.Amount;}}return Ȁ;}я ΖБ(String υ,String ς,З Г,bool ΘΒ){υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){я В=Ǘ[i];if(В.υ.ToUpper()==υ&&В.ς.ToUpper()==ς&&(В.Ō==Г||Г==З.Ċ))
return В;}я έ=null;if(ΘΒ){έ=new я(υ,ς,0,Г);Ǘ.Add(έ);}return έ;}int ȉ(String υ,String ς,З Г){return ȉ(υ,ς,Г,null);}int ȉ(String υ,String ς,З Г,String[]Ǩ){int Ȁ=0;υ=υ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǘ.Count;i++){я В=Ǘ[i];if(Ǩ!=null&&Ǩ.Contains(В.υ.ToUpper())) continue;
if((υ==""||В.υ.ToUpper()==υ)&&(ς==""||В.ς.ToUpper()==ς)&&(В.Ō==Г||Г==З.Ċ))
Ȁ+=В.Ǒ;}return Ȁ;}void ΔΨ(IMyTerminalBlock Τ,З Г){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΖБ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Г,true).Ǒ+=(int) Ъ[j].Amount;}}void Ηξ(List<я>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){я a=Т[j];я b=Т[j+1];if(a.Ǒ<b.Ǒ) Т.Move(j,j+1);}}bool ŭ(bool ΙЃ){if(ȟ&&Η!=Ζ.Ϊ)
if(Н!=-1&&Ĩ>=Н){Μ="Ship too heavy";return true;}if(Αδ>=Ǫ||ƛ){ƛ=false;Μ="Ship is full";return true;}return false;}bool Δω(){String[]Ǩ=null;if(!Ȟ) Ǩ=new string[]{"ICE"};if(Η==Ζ.Ű) return ȉ("","ORE",З.ū,Ǩ)==0;if(Η==Ζ.Ǎ) return ȉ("","COMPONENT",З.ū,Ǩ)==0;else return ȉ("","",З.ū,Ǩ)==0;
}bool ΗΡ(){List<IMyCockpit>ΓΓ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΓ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΓ.Count;i++)
if(ΓΓ[i].IsUnderControl) return true;return false;}bool ΖЂ(bool ȅ,bool Αό){IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δα(ϊ,ȅ,Αό);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δα(ϋ,ȅ,Αό);return false;}bool Δα(ĸ Ǿ,bool ȅ,bool Αό){if(ȅ) Ǿ.Ηή();Ǿ.Ηώ();bool н=false;String ь="";switch(Ǿ.Ћ){case ι.Βζ:{ь="Waiting for command";н=false;break;}case ι.ΔЄ:{ь="Waiting for cargo";н=ŭ(true);break;}case ι.ΔЃ:{ь="Unloading";н=Δω();break;}case ι.Βλ:{н=true;break;}
case ι.Βε:{ь="Charging batteries";н=ǻ>=100f;break;}case ι.ǁ:{ь="Discharging batteries";н=ǻ<=25f;break;}case ι.ǀ:{ь="Discharging batteries";н=ǻ<=0f;break;}case ι.Βθ:{ь="Filling up hydrogen";н=Ȕ>=100f;break;}case ι.Βι:{ь="Unloading hydrogen";н=Ȕ<=25f;break;}
case ι.Βη:{ь="Unloading hydrogen";н=Ȕ<=0f;break;}case ι.Βκ:{bool Εώ=ΗΡ();if(!Εώ) Ǿ.ΓЀ=true;н=Ǿ.ΓЀ&&Εώ;ь="Waiting for passengers";break;}}if(!н) Ǿ.ΒΒ(true);if(н&&Ǿ.ΒΪ()){н=Ǿ.ΒΒ(false);ь="Undocking in: "+ΒΣ((int) Math.Max(0,Ǿ.ǔ-Ǿ.Ȯ));}if(Αό) Ζφ=ь;return н;}void Ζω()
{if(!ň) return;if(ο.Count<=1) return;float Ζξ=0,Ζν=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǯ=ο[i];if(Ŭ(Ǯ,true)) continue;Ζξ+=(float) Ǯ.GetInventory(0).MaxVolume;Ζν+=(float) Ǯ.GetInventory(0).CurrentVolume;}float ΕΛ=(float) Math.Round(Џ(Ζν,Ζξ),5);for(int ΖΞ=0;ΖΞ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΞ++)
{float Γή=0,Γξ=0;float Εϋ=0,ΕЏ=0;IMyTerminalBlock Ȓ=null,ȯ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǯ=ο[i];if(Ŭ(Ǯ,true)) continue;float Γυ=(float) Ǯ.GetInventory(0).MaxVolume;float ΑΨ=Џ((float) Ǯ.GetInventory(0).CurrentVolume,Γυ);if(Ȓ==null||ΑΨ>Γή){Ȓ=Ǯ;Γή=ΑΨ;Εϋ=Γυ;}
if(ȯ==null||ΑΨ<Γξ){ȯ=Ǯ;Γξ=ΑΨ;ΕЏ=Γυ;}}if(Ȓ==null||ȯ==null||Ȓ==ȯ) return;if(checkConveyorSystem&&!Ȓ.GetInventory(0).IsConnectedTo(ȯ.GetInventory(0))){if(ΒЉ>20) Μ="Inventory balancing failed";else ΒЉ++;return;}ΒЉ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
Ȓ.GetInventory(0).GetItems(Ъ);float Ʀ=0;if(Ъ.Count==0) continue;MyInventoryItem ΑΩ=Ъ[0];String ΕЂ=ΑΩ.Type.TypeId+ΑΩ.Type.SubtypeId;if(!Ζυ.TryGetValue(ΕЂ,out Ʀ)){if(ΗΦ(Ȓ.GetInventory(0),0,ȯ.GetInventory(0),out Ʀ))
Ζυ.Add(ΕЂ,Ʀ);else return;}float ΘЏ=((Γή-ΕΛ)*Εϋ/Ʀ);float ΘА=((ΕΛ-Γξ)*ΕЏ/Ʀ);int ΔΚ=(int) Math.Min(ΘА,ΘЏ);if(ΔΚ<=0) return;if((float) ΑΩ.Amount<ΔΚ)
Ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΑΩ.Amount);else
Ȓ.GetInventory(0).TransferItemTo(ȯ.GetInventory(0),0,null,null,ΔΚ);}}bool ΗΦ(IMyInventory Ɯ,int Β,IMyInventory ΘΕ,out float Ζτ){Ζτ=0;float ΗЊ=(float) Ɯ.CurrentVolume;List<MyInventoryItem>ΒЋ=new List<MyInventoryItem>();Ɯ.GetItems(ΒЋ);float ΕΞ=0;for(int i=0;i<ΒЋ.Count;i++) ΕΞ+=(float) ΒЋ[i].Amount;
Ɯ.TransferItemTo(ΘΕ,Β,null,null,1);float Ζσ=ΗЊ-(float) Ɯ.CurrentVolume;List<MyInventoryItem>ΒЁ=new List<MyInventoryItem>();Ɯ.GetItems(ΒЁ);float ΕΓ=0;for(int i=0;i<ΒЁ.Count;i++) ΕΓ+=(float) ΒЁ[i].Amount;if(Ζσ==0f||!Βή(0.9999,ΕΞ-ΕΓ,1.0001))
return false;Ζτ=Ζσ;return true;}void Ηψ(){Vector3 Ɗ=φ+Ć;Vector3 ǭ=Ɗ-ί;if(ǭ.Length()==0) ǭ=new Vector3(0,0,-1);Vector3 Αθ=Я(Υ,ǭ);Vector3 Ζι=Vector3.Normalize(Αθ);Vector3 ŋ=Я(Υ,Υ.GetNaturalGravity());float ΘЁ=Γύ>0?Math.Max(0,1-ı(ǭ,Εξ)/5):0;float Ў=(float) Math.Min((Γς>0?Γς:1000f),Math.Max(ƭ(-Αθ,ŋ,null),Γύ*ΘЁ));
if(!Ζμ) Ў=0;if(ΖΩ) Ў=Math.Max(0,1-ΑΚ/ΑΓ)*Ў;if(generalSpeedLimit>0) Ў=Math.Min(generalSpeedLimit,Ў);if(ŕ) Ў=0;if(Ǳ>=0) Ў=Math.Min(Ў,Ǳ);if(ΖΪ) Ў*=(float) Math.Min(1,Џ(ǭ.Length(),wpReachedDist)/2);Vector3 ΕЌ=Я(Υ,Υ.GetShipVelocities().LinearVelocity);float ΗІ=(float)(Math.Max(0,15-ı(-Ζι,-ΕЌ))/15)*0.85f+0.15f;
ΔΒ+=Math.Sign(ΗІ-ΔΒ)/10f;Vector3 Ȫ=Ζι*Ў*ΔΒ-(ΕЌ);Vector3 ΒЈ=ΒΧ(Ȫ,null);if(ΑЁ&&К>0.1f){Ȫ.X*=Βχ(Ȫ.X,ref ΑЂ.X,1f,ΒЈ.X,20);Ȫ.Y*=Βχ(Ȫ.Y,ref ΑЂ.Y,1f,ΒЈ.Y,20);Ȫ.Z*=Βχ(Ȫ.Z,ref ΑЂ.Z,1f,ΒЈ.Z,20);}else ΑЂ=new Vector3(1,1,1);Ơ=Ĩ*Ȫ-ŋ*Ĩ;ΔΪ(Ơ,ΑЁ);К=Vector3.Distance(ί,Ɗ);
}float Βχ(float value,ref float Ф,float ћ,float ȗ,float Γσ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ћ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γσ||ȗ==0) return έ;Ф=Math.Min(Γσ,Math.Max(-Γσ,Ф));έ=Math.Abs(ȗ);return έ;
}void ΕΑ(){float ы=90,џ=90,њ=90;float ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǩ,і,ƒ,Γλ;if(Γμ){ǩ=Vector3.Normalize(φ-ί);ƒ=Я(Υ,ǩ);і=Я(Υ,ΓБ);ы=ı(ƒ,new Vector3(0,-1,0))-90;џ=ċ(і,new Vector3(-1,0,0),і.Y);
њ=ċ(ƒ,new Vector3(-1,0,0),ƒ.Z);}else{ǩ=ΔΑ;і=Я(Υ,ΓБ);ƒ=Я(Υ,ΔΑ);Γλ=Я(Υ,Ζκ);ы=ċ(і,new Vector3(0,0,1),і.Y);џ=ċ(і,new Vector3(-1,0,0),і.Y);њ=ċ(Γλ,new Vector3(0,0,1),Γλ.X);}if(ΕΕ&&ƹ()){Vector3 ΓΧ=Υ.GetNaturalGravity();і=Я(Υ,ΓΧ);ы=ċ(і,new Vector3(0,0,1),і.Y);
џ=ċ(і,new Vector3(-1,0,0),і.Y);}if(!Βή(-45,џ,45)){ы=0;њ=0;}if(!Βή(-45,њ,45)) ы=0;ǅ(ΓΩ,1,(-ы)*ǜ,(-њ)*ǜ,(-џ)*ǜ);ΑΚ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(џ),Math.Abs(њ)));Αΰ=ΑΚ<=ΑΓ;}void Ζώ(IMyTerminalBlock Ατ){if(Ατ==null) return;ǟ=new float[3,2];ќ=new Dictionary<string,float[,]>();
for(int i=0;i<ȇ.Count;i++){IMyThrust в=ȇ[i];if(!в.IsFunctional) continue;Vector3 κ=Я(Ατ,в.WorldMatrix.Backward);float ȭ=в.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǟ[0,0]+=ȭ;else ǟ[0,1]-=ȭ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǟ[1,0]+=ȭ;else ǟ[1,1]-=ȭ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǟ[2,0]+=ȭ;else ǟ[2,1]-=ȭ;String э=Δς(в);float[,]ѝ=null;if(ќ.ContainsKey(э)) ѝ=ќ[э];else{ѝ=new float[3,2];ќ.Add(э,ѝ);}float ȗ=в.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ѝ[0,0]+=ȗ;else ѝ[0,1]-=ȗ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ѝ[1,0]+=ȗ;else ѝ[1,1]-=ȗ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ѝ[2,0]+=ȗ;else ѝ[2,1]-=ȗ;}}static String Δς(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βΰ(Vector3 ѓ,float[,]ō){return new Vector3(
ѓ.X>=0?ō[0,0]:ō[0,1],
ѓ.Y>=0?ō[1,0]:ō[1,1],
ѓ.Z>=0?ō[2,0]:ō[2,1]);}bool Δρ(Χ Ψ,String э,out float ΓΠ){ΓΠ=0;int Β=Ȇ.IndexOf(э);if(Β==-1||Ψ.Ǟ==null||Β>=Ψ.Ǟ.Length)
return false;ΓΠ=Ψ.Ǟ[Β];if(ΓΠ==-1) return false;return true;}Vector3 ΒΧ(Vector3 ѓ,Χ Ψ){if(Ψ!=null){Vector3 έ=new Vector3();for(int i=0;i<ќ.Keys.Count;i++){String э=ќ.Keys.ElementAt(i);float ƈ=0;if(!Δρ(Ψ,э,out ƈ))
return Βΰ(ѓ,ǟ);έ+=Βΰ(ѓ,ќ.Values.ElementAt(i))*ƈ;}return έ;}return Βΰ(ѓ,ǟ);}float ũ(Vector3 ѓ,Χ Ψ){return ũ(ѓ,new Vector3(),Ψ);}float ũ(Vector3 ѓ,Vector3 ν,Χ Ψ){Vector3 ΘЇ=ΒΧ(ѓ,Ψ);Vector3 ȭ=ΘЇ+ν*Ĩ;float Ɨ=(ȭ/ѓ).AbsMin();return(float)(ѓ*Ɨ).Length();}float ƭ(Vector3 Αζ,Vector3 ŋ,Χ Ψ)
{if(Αζ.Length()==0) return 0;float ΕΔ=1;if(ŋ.Length()>0) ΕΔ=Math.Min(1,ı(-ŋ,Αζ)/90)*0.4f+0.6f;float Ơ=ũ(Αζ,ŋ,Ψ);if(Ơ==0) return 0.1f;float ΑΒ=Џ(Ơ,Ĩ);float ΔΕ=(float) Math.Sqrt(Џ(Αζ.Length(),ΑΒ*0.5f));return ΑΒ*ΔΕ*ΕΔ*Ǥ;}float ƶ(Vector3 ǩ,Vector3 ώ,Vector3 ν,Χ Ψ)
{if(ν.Length()==0f) return 0;Vector3 Αη=đ(ǩ,ώ,Vector3.Normalize(ν));float ǖ=ũ(-Αη,Ψ);return ǖ/ν.Length();}void Δέ(bool ΘΧ){float ў=0;float ΓΤ=0.9f;if(ΘΧ){Н=-1;Ĭ=0;Αώ=null;if(ή!=Λ.з&&Ε.ν.Length()!=0){ў=ΓΤ*ƶ(Ε.β,Ε.α*-1,Ε.ν,null);if(ў<Н||Н==-1) Н=ў;}if(Θ.γ&&Θ.ν.Length()!=0)
{ў=ΓΤ*ƶ(Θ.β,Θ.α*-1,Θ.ν,null);if(ў<Н||Н==-1) Н=ў;}return;}if(Ĭ==-1) return;if(Ĭ>=0){int ΖΡ=0;while(Ĭ<Σ.Count){if(ΖΡ>100) return;ΖΡ++;Χ Ψ=Σ[Ĭ];if(Ψ.ν.Length()!=0f){ў=ΓΤ*Math.Min(
ƶ(Ψ.β,Ψ.α*-1,Ψ.ν,Ψ),
ƶ(Ψ.β*-1,Ψ.α*-1,Ψ.ν,Ψ));if(ў<Н||Н==-1) Н=ў;}else Αώ=Ψ;Ĭ++;}Ĭ=-1;}bool ΓΪ=true;float Ă=0;if(Αώ!=null){for(int i=0;i<ќ.Count;i++){String э=ќ.Keys.ElementAt(i);float[,]ѝ=ќ.Values.ElementAt(i);float ƈ=0;if(!Δρ(Αώ,э,out ƈ)){ΓΪ=false;break;}for(int a=0;a<ѝ.GetLength(0);a++)
for(int b=0;b<ѝ.GetLength(1);b++){float Ĕ=Math.Abs(ѝ[a,b]*ƈ);if(Ĕ==0) continue;ΓΪ=true;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}}if(!ΓΪ){for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float Ĕ=Math.Abs(ǟ[a,b]);if(Ĕ==0) continue;if(Ă==0||Ĕ<Ă) Ă=Ĕ;}}if(Ă>0){float Θγ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ў=Џ(Ă,Θγ);if(ў>0)
if(ў<Н||Н==-1) Н=ў;}}void ΔΪ(Vector3 ǖ,bool Ї){if(!Ї){for(int i=0;i<ȇ.Count;i++)
ȇ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЇ=ΒΧ(ǖ,null);float ΙΝ=Math.Min(1,Math.Abs(Џ(ǖ.X,ΒЇ.X)));float ΙΞ=Math.Min(1,Math.Abs(Џ(ǖ.Y,ΒЇ.Y)));float ΙΟ=Math.Min(1,Math.Abs(Џ(ǖ.Z,ΒЇ.Z)));for(int i=0;i<ȇ.Count;i++){IMyThrust в=ȇ[i];Vector3 κ=Ηΰ(Я(Υ,в.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǖ.X))
в.SetValueFloat("Override",в.MaxThrust*ΙΝ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǖ.Y))
в.SetValueFloat("Override",в.MaxThrust*ΙΞ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǖ.Z))
в.SetValueFloat("Override",в.MaxThrust*ΙΟ);else
в.SetValueFloat("Override",0.0f);}}void ǅ(bool Ї,float Θο,float ы,float њ,float џ){for(int i=0;i<ΑΫ.Count;i++){IMyGyro І=ΑΫ[i];І.GyroOverride=Ї;if(!Ї) І.GyroPower=100;else І.GyroPower=Θο;if(!Ї) continue;Vector3 Б=Υ.WorldMatrix.Forward;Vector3 Ь=Υ.WorldMatrix.Right;Vector3 ώ=Υ.WorldMatrix.Up;
Vector3 Ń=І.WorldMatrix.Forward;Vector3 Ņ=І.WorldMatrix.Up;Vector3 ń=І.WorldMatrix.Left*-1;if(Ń==Б) І.SetValueFloat("Roll",џ);else if(Ń==(Б*-1)) І.SetValueFloat("Roll",џ*-1);else if(Ņ==(Б*-1)) І.SetValueFloat("Yaw",џ);else if(Ņ==Б) І.SetValueFloat("Yaw",џ*-1);else if(ń==Б) І.SetValueFloat("Pitch",џ);
else if(ń==(Б*-1)) І.SetValueFloat("Pitch",џ*-1);if(ń==(Ь*-1)) І.SetValueFloat("Pitch",ы);else if(ń==Ь) І.SetValueFloat("Pitch",ы*-1);else if(Ņ==Ь) І.SetValueFloat("Yaw",ы);else if(Ņ==(Ь*-1)) І.SetValueFloat("Yaw",ы*-1);else if(Ń==(Ь*-1)) І.SetValueFloat("Roll",ы);
else if(Ń==Ь) І.SetValueFloat("Roll",ы*-1);if(Ņ==(ώ*-1)) І.SetValueFloat("Yaw",њ);else if(Ņ==ώ) І.SetValueFloat("Yaw",њ*-1);else if(ń==ώ) І.SetValueFloat("Pitch",њ);else if(ń==(ώ*-1)) І.SetValueFloat("Pitch",њ*-1);else if(Ń==ώ) І.SetValueFloat("Roll",њ);else if(Ń==(ώ*-1)) І.SetValueFloat("Roll",њ*-1);
}}void ΔЎ(){this.ΓΩ=false;}void Х(Vector3 Ȭ,Vector3 Б,Vector3 Γι,float ȝ,bool ƥ){ΑЌ(Ȭ,ȝ,ƥ);ΑΓ=ȝ;Γμ=false;this.ΔΑ=Б;this.Ζκ=Γι;}void Х(Vector3 Ȭ,Vector3 Б,Vector3 Γι,bool ƥ){Х(Ȭ,Б,Γι,2f,ƥ);}void ΑЌ(Vector3 Ȭ,float ȝ,bool ƥ){ΑΓ=ȝ;this.ΓΩ=true;this.ΕΕ=ƥ;Γμ=true;Αΰ=false;
this.ΓБ=Ȭ;}void Βό(){Ǣ(false,false,false,φ,0);ΑЁ=false;}void П(Vector3 Љ,float ǜ){Ǣ(true,false,false,Љ,ǜ);}void Ǣ(bool Ї,bool ΓЌ,bool ΔΝ,Vector3 Љ,float ǜ){Ǣ(Ї,ΓЌ,ΔΝ,Љ,Љ-ί,0.0f,ǜ);}void Ǣ(bool Ї,bool ΓЌ,bool ΔΝ,Vector3 Љ,Vector3 Θϋ,float Θό,float ǜ){ΑЁ=true;this.Ζμ=Ї;
φ=Љ;this.Γς=ǜ;this.Γύ=Θό;this.ΖΪ=ΓЌ;this.ΖΩ=ΔΝ;this.Εξ=Θϋ;К=Vector3.Distance(Љ,ί);}void Ηα(){ω.GetBlocksOfType(Ÿ,ǎ);}void Ηβ(){if(Αφ){ύ=2;return;}List<IMyRemoteControl>Ƙ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΧ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζα=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ƙ,ǎ);ω.GetBlocksOfType(ѕ,ǎ);ω.GetBlocksOfType(ΖΧ,ǎ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζα,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ѕ=ǡ(ѕ,pamTag,true);Ηι();Ηκ(Ζα);ų(ѕ,setLCDFontAndSize,1.4f,false);ų(ą,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζε=ǡ(ΖΧ,pamTag,true);if(Ζε.Count>0) ξ=Ζε[0];else ξ=null;if(Η==Ζ.Ű){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Μ="Drills are missing";}}else if(Η==Ζ.Ǎ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Μ="Grinders are missing";}if(ξ==null){ύ=1;Μ="Sensor is missing";}}else if(Η==Ζ.Ϊ){ω.GetBlocksOfType(ΔΖ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζδ=ǡ(Ƙ,pamTag,true);if(Ζδ.Count>0) Ƙ=Ζδ;if(Ƙ.Count>0) Υ=Ƙ[0];else{Υ=null;ύ=2;Μ="Remote is missing";return;}θ=(float) Υ.CubeGrid.WorldVolume.Radius*2;љ=null;if(Η!=Ζ.Ϊ){Δή();if(ο.Count>0&&љ!=null){if(ξ!=null&&(љ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Υ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Μ="Wrong sensor direction";}if(љ.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
љ.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ύ=2;Μ="Wrong remote direction";}}}}void Ηγ(){ω.GetBlocksOfType(ȃ,ǎ);ω.GetBlocksOfType(т,ǎ);ω.GetBlocksOfType(ȇ,ǎ);ω.GetBlocksOfType(ΑΫ,ǎ);ω.GetBlocksOfType(Ȩ,ǎ);ω.GetBlocksOfType(ă,ǎ);ω.GetBlocksOfType(є,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(Αύ,ǎ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǡ(т,"ConnectorMedium",false);else
т=ǡ(т,"Connector",false);List<IMyShipConnector>Ζβ=ǡ(т,pamTag,true);if(Ζβ.Count>0) т=Ζβ;if(ύ<=1){if(т.Count==0){ύ=1;Μ="Connector is missing";}if(ΑΫ.Count==0){ύ=1;Μ="Gyros are missing";}if(ȇ.Count==0){ύ=1;Μ="Thrusters are missing";}}List<IMyConveyorSorter>Ζζ=ǡ(Αύ,pamTag,true);
if(Ζζ.Count>0) Αύ=Ζζ;List<IMyLandingGear>Ζγ=ǡ(ȃ,pamTag,true);if(Ζγ.Count>0) ȃ=Ζγ;for(int i=0;i<ȃ.Count;i++) ȃ[i].AutoLock=false;List<IMyBatteryBlock>Ζΰ=ǡ(Ȩ,pamTag,true);if(Ζΰ.Count>0) Ȩ=Ζΰ;List<IMyGasTank>Ζη=ǡ(є,pamTag,true);if(Ζη.Count>0) є=Ζη;}void Ηδ(){ω.GetBlocksOfType(ĕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ŀ.Clear();for(int i=ĕ.Count-1;i>=0;i--){if(ΗΞ(ĕ[i])){ŀ.Add(ĕ[i]);ĕ.RemoveAt(i);}}}bool ΗΞ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΒΫ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δή(){if(Υ==null) return;љ=null;float Αο=0,Ακ=0,Απ=0,Αλ=0,Αρ=0,Αμ=0;List<IMyTerminalBlock>ΓА=ǡ(ο,pamTag,true);bool Θκ=ΓА.Count==0;if(ΓА.Count>0) љ=ΓА[0];else if(ο.Count>0) љ=ο[0];int Ȁ=0;float Ŗ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward){if(Θκ){ύ=2;Μ="Mining direction is unclear!";return;}continue;}Ȁ++;Ŗ=Math.Max(Ŗ,Ησ(ο[i]));Vector3 Ј=Ƶ(Υ,ο[i].GetPosition());if(i==0){Αο=Ј.X;Ακ=Ј.X;Απ=Ј.Y;Αλ=Ј.Y;Αρ=Ј.Z;Αμ=Ј.Z;}Ακ=Math.Max(Ј.X,Ακ);Αο=Math.Min(Ј.X,Αο);
Αλ=Math.Max(Ј.Y,Αλ);Απ=Math.Min(Ј.Y,Απ);Αμ=Math.Max(Ј.Z,Αμ);Αρ=Math.Min(Ј.Z,Αρ);}if(Ŗ==0) Ŗ=drillRadius;bool Ȭ=љ!=null&&љ.WorldMatrix.Forward==Υ.WorldMatrix.Down;ǰ=(Ακ-Αο)*(1-ĭ)+Ŗ*2;ǯ=(Ȭ?Αμ-Αρ:Αλ-Απ)*(1-ĝ)+Ŗ*2;IMyCubeGrid g=Υ.CubeGrid;float ΔΣ=0,ΔΤ=0,ΔΥ=0,ΔΦ=0;
for(int c=0;c<8;c++){Vector3 p=Ƶ(Υ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȭ?p.Z:p.Y;if(c==0||p.X<ΔΣ) ΔΣ=p.X;if(c==0||p.X>ΔΤ) ΔΤ=p.X;if(c==0||y<ΔΥ) ΔΥ=y;if(c==0||y>ΔΦ) ΔΦ=y;}Αϊ=ΔΤ-ΔΣ+g.GridSize;Αω=ΔΦ-ΔΥ+g.GridSize;}float Ησ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǡ<T>(List<T>Т,String ǝ,bool Αά){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(ΒΫ(Т[i],ǝ,Αά)) έ.Add(Т[i]);return έ;}bool ΒΫ<T>(T Τ,String ǝ,bool Αά){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(Αά&&b.CustomName.ToUpper().Contains(ǝ.ToUpper())) return true;if(!Αά&&b.BlockDefinition.ToString().ToUpper().Contains(ǝ.ToUpper())) return true;return false;}void ё<T>(List<T>Т,bool Ї)
{for(int i=0;i<Т.Count;i++)
ΔЉ((IMyTerminalBlock) Т[i],Ї);}void ΔЉ(IMyTerminalBlock Τ,bool Ї){if(Τ==null) return;String ΗЄ=Ї?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(ΗЄ);a.Apply(Τ);}void ǣ(bool Ї){ΓЍ=Ї;if(!ĩ) return;ё(Αύ,Ї);}void Βσ(ChargeMode Ɠ){for(int i=0;i<Ȩ.Count;i++) Ȩ[i].ChargeMode=Ɠ;}void Βυ(bool ΘЃ)
{for(int i=0;i<є.Count;i++) є[i].Stockpile=ΘЃ;}void Ŵ(List<IMyLandingGear>ΓΥ,bool ΕЍ){for(int i=0;i<ΓΥ.Count;i++){if(ΕЍ) ΓΥ[i].Lock();if(!ΕЍ) ΓΥ[i].Unlock();}}void ΔЊ(bool Ї){if(Εέ==Ї) return;List<IMyShipController>ΑΘ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΘ,ǎ);if(ΑΘ.Count==0) return;
for(int i=0;i<ΑΘ.Count;i++)
ΑΘ[i].DampenersOverride=Ї;Εέ=Ї;}IMyShipConnector О(MyShipConnectorStatus ь){for(int i=0;i<т.Count;i++){if(!Ƿ(т[i],true)) continue;if(т[i].Status==ь) return т[i];}return null;}void Ē(bool ΓЉ,bool ΑΞ,float ΘΝ,float ΗЉ){if(ξ==null||ο.Count==0) return;Vector3 ΑΗ=new Vector3();
int Ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward) continue;Ȁ++;ΑΗ+=ο[i].GetPosition();}ΑΗ=ΑΗ/Ȁ;Vector3 Œ=Ƶ(ξ,ΑΗ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЉ;ξ.LeftExtend=(ΑΞ?1:й)/2f*ǰ-Œ.X;ξ.RightExtend=(ΑΞ?1:й)/2f*ǰ+Œ.X;ξ.TopExtend=(ΑΞ?1:и)/2f*ǯ+Œ.Y;
ξ.BottomExtend=(ΑΞ?1:и)/2f*ǯ-Œ.Y;ξ.FrontExtend=(ΓЉ?Ц:ΘΝ)-Œ.Z;ξ.BackExtend=ΓЉ?0:ΗЉ+θ*0.75f+Œ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ų<T>(List<T>Т,bool Θώ,float ΘΛ,bool ΘΨ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƞ=null;if(Т[i]is IMyTextSurface) ƞ=(IMyTextSurface) Т[i];if(ƞ!=null){ƞ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Θώ) continue;ƞ.Font="Debug";if(ΘΨ) continue;ƞ.FontSize=ΘΛ;}}}bool ƹ(){Vector3D ζ;return this.Υ.TryGetPlanetPosition(out ζ);}int ΗΕ(Vector3 ΘЅ,int ΖΫ){if(ΖΫ==-1) return 0;double А=-1;int ȑ=-1;for(int i=Σ.Count-1;i>=0;i--){double ό=Vector3.Distance(Σ[i].Δ,ΘЅ);if(А==-1||ό<А){ȑ=i;А=ό;}
}return Math.Sign(ȑ-ΖΫ);}bool ΗΠ(Vector3 ζ){List<Vector3>Е=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Е.Add(Σ[i].Δ);if(Θ.γ&&Σ.Count>=1){Vector3 Ļ=new Vector3();Ƕ(Θ,dockDist*θ,false,out Ļ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Е.Insert(0,Ļ);Е.Insert(0,Θ.Δ);}
else{Е.Add(Ļ);Е.Add(Θ.Δ);}}if(Η==Ζ.Ϊ){if(Ε.γ&&Σ.Count>=1){Vector3 ΒЃ=new Vector3();Ƕ(Ε,dockDist*θ,false,out ΒЃ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Е.Insert(0,ΒЃ);Е.Insert(0,Ε.Δ);}else{Е.Add(ΒЃ);Е.Add(Ε.Δ);}}}else{if(ή!=Λ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Е.Insert(0,Ε.Δ);else Е.Add(Ε.Δ);}int ȑ=-1;double Ǧ=-1;for(int i=0;i<Е.Count;i++){double ό=Vector3.Distance(Е[i],ζ);if(ό<Ǧ||Ǧ==-1){Ǧ=ό;ȑ=i;}}if(Е.Count==0) return false;double ΓΚ=Vector3.Distance(Е[ȑ],ζ);double Θπ=Vector3.Distance(Е[Math.Max(0,ȑ-1)],Е[ȑ])*1.5f;
double Θι=Vector3.Distance(Е[Math.Min(Е.Count-1,ȑ+1)],Е[ȑ])*1.5f;return ΓΚ<Θπ||ΓΚ<Θι||ΓΚ<laneOffset*θ*1.5f;}static float Џ(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηΰ(Vector3 v,int ΓΗ){return new Vector3(Math.Round(v.X,ΓΗ),Math.Round(v.Y,ΓΗ),Math.Round(v.Z,ΓΗ));
}float ı(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΚ=(a*b).Sum;float ΕЈ=a.Length();float ΕЉ=b.Length();if(ΕЈ==0||ΕЉ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΚ/(ΕЈ*ΕЉ)));if(float.IsNaN(έ)) return 0;return έ;}float ċ(Vector3 v,Vector3 Ατ,float ΘϏ){float Ĺ=ı(v,Ατ);
if(ΘϏ>0f) Ĺ*=-1;if(Ĺ>-90f) return Ĺ-90f;else return 180f-(-Ĺ-90f);}double Δδ(float ΘΓ){return(Math.PI/180)*ΘΓ;}bool Βή(double Θβ,double value,double Αι){return(value>=Θβ&&value<=Αι);}Vector3 Ητ(IMyTerminalBlock Τ,Vector3 Ј){return Vector3D.Transform(Ј,Τ.WorldMatrix);
}Vector3 Ƶ(IMyTerminalBlock Τ,Vector3 ΙΚ){return Я(Τ,ΙΚ-Τ.GetPosition());}Vector3 Я(IMyTerminalBlock Τ,Vector3 ΔΡ){return Vector3D.TransformNormal(ΔΡ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 đ(Vector3 ǩ,Vector3 ώ,Vector3 ΔΡ){MatrixD Γρ=MatrixD.CreateFromDir(ǩ,ώ);return Vector3D.TransformNormal(ΔΡ,MatrixD.Transpose(Γρ));
}Vector3 Βύ(Vector3 ǩ,Vector3 ώ,Vector3 Ј){MatrixD Γρ=MatrixD.CreateFromDir(ǩ,ώ);return Vector3D.Transform(Ј,Γρ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Д=s.Split('|');return new Vector3(float.Parse(Κ(Д,0)),float.Parse(Κ(Д,1)),float.Parse(Κ(Д,2)));
}String ΗЂ(Χ Ψ){String Њ=":";String έ=τ(Ψ.Δ)+Њ+τ(Ψ.β)+Њ+
τ(Ψ.α)+Њ+τ(Ψ.ψ)+Њ+τ(Ψ.ν);for(int i=0;i<Ψ.Ǟ.Length;i++)
έ+=Њ+Math.Round(Ψ.Ǟ[i],3);return έ;}Χ Ηο(String s){String[]Д=s.Split(':');Χ Ψ=new Χ(
μ(Κ(Д,0)),μ(Κ(Д,1)),
μ(Κ(Д,2)),μ(Κ(Д,3)),
μ(Κ(Д,4)));int Β=5;List<float>Ει=new List<float>();while(Β<Д.Length){String ΑЇ=Κ(Д,Β);float f=0;if(!float.TryParse(ΑЇ,out f)) break;Ει.Add(f);Β++;}Ψ.Ǟ=Ει.ToArray();return Ψ;}System.Text.StringBuilder Ś=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θθ){if(Θθ) Ś.Append('\n');Ś.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʊ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ǌ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]ΗЈ,int Β){String ΑЇ=ΗЈ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑЇ)) return "";
return ΑЇ;}void Save(){if(Αφ||Η==Ζ.р){Storage="";return;}Storage=ΒΑ();}String ΒΑ(){Ś.Clear();Ś.Append(DATAREV+";");Ρ(τ(Αυ),false);Ρ(τ(Θ.β));Ρ(τ(Θ.ψ));Ρ(τ(Θ.α));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.γ);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.β));Ρ(τ(Ε.α));Ρ(τ(Ε.ψ));Ρ(τ(Ε.Ч));Ρ(Ε.γ);Ρ(τ(Ǜ));Ρ(τ(Ș));Ρ(";");
Ρ((int) Η,false);Ρ((int) ή);Ρ((int) ц);Ρ(Ǫ);Ρ(ч);Ρ(Ы);Ρ(ш);Ρ(Ǥ);Ρ(ȟ);Ρ(Ȟ);if(Η==Ζ.Ϊ){Ρ((int) ϊ.Ћ);Ρ(ϊ.ǔ);Ρ(ϊ.Ȯ);Ρ(Ʊ(ϊ.Ǖ));Ρ(Ʊ(ϊ.ǘ));Ρ((int) ϋ.Ћ);Ρ(ϋ.ǔ);Ρ(ϋ.Ȯ);Ρ(Ʊ(ϋ.Ǖ));Ρ(Ʊ(ϋ.ǘ));}else{Ρ((int) Ȝ);Ρ((int) л);Ρ((int) Л);Ρ((int) у);Ρ((int) ġ);Ρ(ć);Ρ(ĩ);Ρ(ň);Ρ(ł);Ρ(й);Ρ(и);Ρ(Ц);Ρ(Ť);Ρ(ţ);Ρ(ĭ);Ρ(ĝ);
Ρ(М);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(Ȗ);Ρ(Й);Ρ(Ħ);Ρ(Ĥ);}Ρ(";");for(int i=0;i<Ȇ.Count;i++)
Ρ((i>0?"|":"")+Ȇ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЂ(Σ[i]),i>0);Ρ(";",false);Ρ(С+"|"+Ѐ,false);Ρ(";",false);Ρ(ΖЍ(),false);String Ϋ=Ś.ToString();Ś.Clear();return Ϋ;}с Ηί(){return ǂ(Storage);}с ǂ(String Ϋ){if(Ϋ=="") return с.ΗΨ;String[]ħ=Ϋ.Split(';');if(Κ(ħ,0)!=DATAREV) return с.Βγ;int Β=0;try{String[]Ι=Κ(ħ,1).Split('\n');
Αυ=μ(Κ(Ι,Β++));Θ.β=μ(Κ(Ι,Β++));Θ.ψ=μ(Κ(Ι,Β++));Θ.α=μ(Κ(Ι,Β++));Θ.ν=μ(Κ(Ι,Β++));Θ.Δ=μ(Κ(Ι,Β++));Θ.Ч=μ(Κ(Ι,Β++));Θ.γ=bool.Parse(Κ(Ι,Β++));Ε.Δ=μ(Κ(Ι,Β++));Ε.ν=μ(Κ(Ι,Β++));Ε.β=μ(Κ(Ι,Β++));Ε.α=μ(Κ(Ι,Β++));Ε.ψ=μ(Κ(Ι,Β++));Ε.Ч=μ(Κ(Ι,Β++));Ε.γ=bool.Parse(Κ(Ι,Β++));
Ǜ=μ(Κ(Ι,Β++));Ș=μ(Κ(Ι,Β++));Ι=Κ(ħ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Κ(Ι,Β++));ή=(Λ) int.Parse(Κ(Ι,Β++));ц=(Λ) int.Parse(Κ(Ι,Β++));Ǫ=int.Parse(Κ(Ι,Β++));ч=int.Parse(Κ(Ι,Β++));Ы=int.Parse(Κ(Ι,Β++));ш=int.Parse(Κ(Ι,Β++));Ǥ=float.Parse(Κ(Ι,Β++));ȟ=bool.Parse(Κ(Ι,Β++));
Ȟ=bool.Parse(Κ(Ι,Β++));if(Η==Ζ.Ϊ){ϊ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϊ.ǔ=float.Parse(Κ(Ι,Β++));ϊ.Ȯ=float.Parse(Κ(Ι,Β++));ϊ.Ǖ=Ǌ(Κ(Ι,Β++));ϊ.ǘ=Ǌ(Κ(Ι,Β++));ϋ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϋ.ǔ=float.Parse(Κ(Ι,Β++));ϋ.Ȯ=float.Parse(Κ(Ι,Β++));ϋ.Ǖ=Ǌ(Κ(Ι,Β++));ϋ.ǘ=Ǌ(Κ(Ι,Β++));}else{Ȝ=(Ǐ) int.Parse(Κ(Ι,Β++));
л=(д) int.Parse(Κ(Ι,Β++));Л=(Ϗ) int.Parse(Κ(Ι,Β++));у=(ю) int.Parse(Κ(Ι,Β++));ġ=(Ǐ) int.Parse(Κ(Ι,Β++));ć=bool.Parse(Κ(Ι,Β++));ĩ=bool.Parse(Κ(Ι,Β++));ň=bool.Parse(Κ(Ι,Β++));ł=bool.Parse(Κ(Ι,Β++));й=int.Parse(Κ(Ι,Β++));и=int.Parse(Κ(Ι,Β++));Ц=int.Parse(Κ(Ι,Β++));Ť=float.Parse(Κ(Ι,Β++));
ţ=float.Parse(Κ(Ι,Β++));ĭ=float.Parse(Κ(Ι,Β++));ĝ=float.Parse(Κ(Ι,Β++));М=int.Parse(Κ(Ι,Β++));б=int.Parse(Κ(Ι,Β++));Ш=int.Parse(Κ(Ι,Β++));Щ=int.Parse(Κ(Ι,Β++));Ȗ=int.Parse(Κ(Ι,Β++));Й=int.Parse(Κ(Ι,Β++));Ħ=int.Parse(Κ(Ι,Β++));Ĥ=float.Parse(Κ(Ι,Β++));}Ι=Κ(ħ,3).Replace("\n","").Split('|');
Ȇ=Ι.ToList();Ι=Κ(ħ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηο(Κ(Ι,i)));Ι=Κ(ħ,5).Split('|');С=Κ(Ι,0);if(!int.TryParse(Κ(Ι,1),out Ѐ)) Ѐ=-1;if(С.Length!=М*б){С="";Ѐ=-1;}ΖЌ(Κ(ħ,6));}catch{return с.Δθ;}ś=ц;χ();return с.Ǉ;}const int Δά=64000;IMyTerminalBlock Δι(){List<IMyTerminalBlock>ľ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ľ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ľ.Count>0?ľ[0]:null;}void ΖА(){IMyTerminalBlock Τ=Δι();if(Τ==null){Μ="Export: name a block "+dataTag;return;}String Ϋ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΑ();if(Ϋ.Length>Δά){Μ="Export too large: "+Ϋ.Length+" chars, max "+Δά;return;}Τ.CustomData=Ϋ;Μ="Exported "+Σ.Count+" waypoints";}void ΗΚ(bool Γε){IMyTerminalBlock Τ=Δι();if(Τ==null){Μ="Import: name a block "+dataTag;return;}
ΗΛ(Τ.CustomData,Γε);}void ΗΛ(String Ϋ,bool Γε){Ϋ=Ϋ.Replace("\r","");if(Ϋ.StartsWith("//")){int ΖΘ=Ϋ.IndexOf('\n');Ϋ=ΖΘ<0?"":Ϋ.Substring(ΖΘ+1);}Ϋ=Ϋ.Trim();if(Ϋ==""){Μ="Import: no PAM data in "+dataTag;return;}χ();String ΕΝ=ΒΑ();с έ=ǂ(Ϋ);if(έ==с.Ǉ&&Γε){Χ Ŋ=new Χ(Ε);
Vector3 Θε=Ǜ,Θδ=Ș;int w=й,h=и,d=Ц;Ǐ ΓЎ=Ȝ;ю ΓΘ=у;float Θν=ĭ,Θμ=ĝ;ǂ(ΕΝ);Ε=Ŋ;Ǜ=Θε;Ș=Θδ;й=w;и=h;Ц=d;Ȝ=ΓЎ;у=ΓΘ;ĭ=Θν;ĝ=Θμ;ή=Λ.Ĵ;Ŷ(true,true);ή=Λ.Ĵ;ц=Λ.Ĵ;Μ="Imported job, run CONT";return;}if(έ==с.Ǉ){Ɣ=false;Μ="Imported "+Σ.Count+" waypoints";
return;}ǂ(ΕΝ);Μ=έ==с.Βγ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ηι(){Ƃ.Clear();for(int i=ѕ.Count-1;i>=0;i--){String Ϋ=ѕ[i].CustomData.ToUpper();bool Γγ=false;if(Ϋ=="INSTRUCTIONS"){Γγ=true;ΑΝ=true;}if(Ϋ=="DEBUG") Γγ=true;if(Γγ){Ƃ.Add(ѕ[i]);ѕ.RemoveAt(i);}
}ų(Ƃ,false,1,false);}void Ηκ(List<IMyTerminalBlock>ľ){ą.Clear();for(int i=0;i<ľ.Count;i++){IMyTerminalBlock Τ=ľ[i];try{String ΖΠ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΖΠ);int ŝ=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΖΠ.Length,1),out ŝ)) continue;
if(ŝ==-1) continue;ŝ--;IMyTextSurfaceProvider ΖΣ=(IMyTextSurfaceProvider) Τ;if(ŝ<ΖΣ.SurfaceCount&&ŝ>=0)
ą.Add(ΖΣ.GetSurface(ŝ));}catch{}}}String Αν="";void ΔЅ(){String Γπ="";String ΓΟ="";Αν=ΔΫ(false);Γπ+=Αν;ΓΟ+=Αν;ΓΟ+=ΗΓ();for(int i=0;i<ѕ.Count;i++) ѕ[i].WriteText(Γπ);for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γπ);Echo(ΓΟ);for(int i=0;i<Ƃ.Count;i++){IMyTextPanel Γό=Ƃ[i];
String Ϋ=Γό.CustomData.ToUpper();if(Ϋ=="DEBUG") Γό.WriteText(""+"\n"+"");if(Ϋ=="INSTRUCTIONS") Γό.WriteText(ΗΔ());}}string ΗΔ(){String Γ="";try{float ΕА=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ľ*ΕА)+" / "+(int)(Ŏ*ΕА)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ľ*100f,1)+"% / "+Math.Round(Ŏ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ż,2)+"ms / "+Math.Round(Γτ,2)+"ms \n";}catch{}for(int i=0;i<ș.Count;i++)
Γ+=""+ș.Keys.ElementAt(i)+" = "+Math.Round(ș.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ș.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΗ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ų(ѕ,setLCDFontAndSize,0.9f,false);ų(ą,setLCDFontAndSize,0.9f,true);for(int i=0;i<ѕ.Count;i++) ѕ[i].WriteText(Γ+ȕ.ToString());for(int i=0;i<ą.Count;i++) ą[i].WriteText(Γ+ȕ.ToString());}void ǹ(){if(!ΑΝ) return;try{ΓЁ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String υ){if(!ΑΝ) return;if(ΓЁ==0) return;try{float ΑЅ=(Runtime.CurrentInstructionCount-ΓЁ)/Runtime.MaxInstructionCount*100;if(!ș.ContainsKey(υ))
ș.Add(υ,new float[]{ΑЅ,ΑЅ});else{ș[υ][0]=ΑЅ;ș[υ][1]=Math.Max(ΑЅ,ș[υ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζϊ(String Ȱ){if(!Ŀ) return;String Њ=""+BROADCAST_SEP;String ρ=VERSION+Њ;ρ+=ť(ΓЈ)+Њ;
ρ+=(int) Η+Њ;ρ+=ΒΡ(ί.X)+""+Њ;ρ+=ΒΡ(ί.Y)+Њ;ρ+=ΒΡ(ί.Z)+Њ;if(Η!=Ζ.Ϊ)
ρ+=ΒΤ(ц)+(ц==Λ.Є?" "+Math.Round(Ǝ,1)+"%":"")+Њ;else ρ+=ΒΤ(ц)+Њ;ρ+=ť(Μ)+Њ;ρ+=ї+""+Њ;ρ+=ť(Αν)+Њ;ρ+=Ł+Њ;ρ+=ŏ+Њ;for(int i=0;i<Ǘ.Count;i++){if(Ǘ[i].Ō!=З.ū) continue;ρ+=ť(Ǘ[i].υ)+"/"+ť(Ǘ[i].ς)+"/"+Ǘ[i].Ǒ+Њ;}Ηζ(ρ,Ȱ);}String ť(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηζ(String ρ,String Ȱ){Ηη(ρ,BROADCAST_SELF_ID,Ȱ);}void Ηη(String ρ,String Љ,String Ȱ){try{if(!Ŀ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δμ()+BROADCAST_SEP+Љ+BROADCAST_SEP+ΒЍ+BROADCAST_SEP+Ȱ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){ȕ=e;}
}String Δμ(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Δϋ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool ŧ(ref String Ϋ,out string Ελ,Char Њ){int Β=Ϋ.IndexOf(Њ);Ελ="";if(Β<0) return false;Ελ=Ϋ.Substring(0,Β);Ϋ=Ϋ.Remove(0,Β+1);return true;}bool ΗΩ(ref String ρ,out String ƚ,out String Ȱ)
{ƚ="";Ȱ="";if(!Ŀ) return false;String ǝ="";if(!ŧ(ref ρ,out ǝ,BROADCAST_SEP)||!Δϋ(ǝ)) return false;if(!ŧ(ref ρ,out ƚ,BROADCAST_SEP)) return false;if(!ŧ(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=Δμ()&&(ǝ!="*"&&Η!=Ζ.р))) return false;if(!ŧ(ref ρ,out ǝ,BROADCAST_SEP)||(ǝ!=ΒЍ)) return false;
if(!ŧ(ref ρ,out Ȱ,BROADCAST_SEP)) return false;return true;}void ΗΝ(){bool ƀ=true;if(Εν){Εν=false;if(Me.CustomData.Contains("Antenna_Name")){Μ="Update custom data";Me.CustomData="";}}String Ƌ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ƌ)
Δχ(Ƌ);String[]Ι=Me.CustomData.Split('\n');Ŀ=bool.Parse(Βν(ΒΟ(Ι,"Enable_Broadcast",ref ƀ)));bool ΔΟ=false;bool Εα=true;if(Ŀ){if(Η!=Ζ.р)
ΓЈ=Βν(ΒΟ(Ι,"Ship_Name",ref ƀ)).Replace(BROADCAST_SELF_ID,"");ΒЍ=Βν(ΒΟ(Ι,"Broadcast_Channel",ref ƀ)).ToLower();Εα=false;if(Ȑ==null){Ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ȑ.SetMessageCallback("");}List<IMyRadioAntenna>ĺ=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ĺ);bool ΒЂ=false;
for(int i=0;i<ĺ.Count;i++)
if(ĺ[i].EnableBroadcasting&&ĺ[i].Enabled){ΒЂ=true;break;}if(ĺ.Count==0) Μ="No Antenna found";else if(!ΒЂ) Μ="Antenna not ready";ΔΟ=ĺ.Count==0||!ΒЂ;if(ΕΖ&&!ΔΟ&&Η!=Ζ.р) Μ="Antenna ok";}else if(Η==Ζ.р)
Μ="Offline - Enable in PB custom data";ΕΖ=ΔΟ;if(Εα){if(Ȑ!=null) this.IGC.DisableBroadcastListener(Ȑ);Ȑ=null;}}catch{ƀ=false;}if(!ƀ){Μ="Reset custom data";Δχ(Ƌ);}}void Δχ(String Ƌ){String Ϋ=Ƌ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";Ϋ+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ϋ+"Broadcast_Channel=#default";}String Βν(String ģ){int Β=ģ.IndexOf("//");if(Β!=-1) ģ=ģ.Substring(0,Β);String[]Д=ģ.Split('=');if(Д.Length<=1) return "";return Д[1].Trim();}String ΒΟ(String[]Ι,String Ģ,ref bool ƕ){foreach(String ģ in Ι)
if(ģ.StartsWith(Ģ)) return ģ;ƕ=false;return "";}String Ιζ="";Ķ Ιθ=null;DateTime Ιι=DateTime.MinValue;List<Ķ>Ιη=new List<Ķ>();const float ΙΡ=20f;String ΔЈ(int ΔΞ,String Γ,int ΖΦ,ref int Ț){String[]Ι=Γ.Split('\n');if(ΖΦ>=Ț+ΔΞ-1) Ț++;Ț=Math.Min(Ι.Count()-1-ΔΞ,Ț);if(ΖΦ<Ț+1) Ț--;
Ț=Math.Max(0,Ț);String έ="";for(int i=0;i<ΔΞ;i++){int ΕЊ=i+Ț;if(ΕЊ>=Ι.Count()) break;έ+=Ι[ΕЊ]+"\n";}return έ;}String ΒΣ(int ƙ){if(ƙ>=60*60) return Math.Round(ƙ/(60f*60f),1)+" h";if(ƙ>=60) return Math.Round(ƙ/60f,1)+" min";return ""+ƙ+" s";}string ΒΡ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>Ǽ=new List<IMyCameraBlock>();List<long>œ=new List<long>();bool ŕ=false;int ΓΒ=0;int ΒА=0;DateTime ΕΠ=DateTime.Now;String ΒЌ="";void Ηε(){Ǽ.Clear();œ.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(Ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ǽ.Count;i++){Ǽ[i].EnableRaycast=true;if(!œ.Contains(Ǽ[i].CubeGrid.EntityId)) œ.Add(Ǽ[i].CubeGrid.EntityId);}if(!œ.Contains(Me.CubeGrid.EntityId)) œ.Add(Me.CubeGrid.EntityId);}bool Δϊ(Α s){return s==Α.ȥ||s==Α.Ю||s==Α.Ѕ||
s==Α.Ȥ||s==Α.Ĳ||s==Α.Ũ||s==Α.Ƴ||
(s==Α.Ǡ&&ј!=null);}void Ư(){ŕ=false;ΓΒ=0;ΒЌ="";}void Ηό(){if(!obstacleBrake||Ǽ.Count==0||!Δϊ(Φ)){Ư();return;}Vector3 Ɗ=φ+Ć;Vector3 ǭ=Ɗ-ί;float Ǭ=ǭ.Length();if(Ǭ<1f){Ư();return;}Vector3 κ=ǭ/Ǭ;float ΕЎ=ț*obstacleLookAhead+θ;Vector3 Θσ=Ɗ+κ*(θ*0.25f);
for(int n=0;n<Ǽ.Count;n++){ΒА=(ΒА+1)%Ǽ.Count;IMyCameraBlock ſ=Ǽ[ΒА];if(!ſ.IsFunctional||!ſ.IsWorking) continue;Vector3 ΕΣ=ſ.GetPosition();Vector3 ΑΦ=Θσ;if(Vector3.Distance(ΕΣ,ΑΦ)>ΕЎ) ΑΦ=ΕΣ+κ*ΕЎ;if(!ſ.CanScan(ΑΦ)) continue;MyDetectedEntityInfo ƍ=ſ.Raycast(ΑΦ);bool Θλ=!ƍ.IsEmpty()&&!œ.Contains(ƍ.EntityId)&&!ΗΤ(ƍ.EntityId)&&
ƍ.Type!=MyDetectedEntityType.FloatingObject;if(Θλ){if(!ŕ) ΕΠ=DateTime.Now;ŕ=true;ΓΒ=0;ΒЌ=ƍ.Type.ToString();}else if(ŕ&&++ΓΒ>=3) Ư();break;}if(ŕ&&(DateTime.Now-ΕΠ).TotalSeconds>obstacleTimeout)
Μ="Path blocked: "+ΒЌ;}const String ΔБ="[PAMTFC]";const float Ηρ=5f;const float ΒΞ=2f;class Ȧ{public long λ;public Vector3 ζ,Ţ;public Vector3 ƌ;public float Εΰ;public String Ŋ="";public int Γί=-1;public bool ΓЂ;public long ά=0;public int Ƅ=0;public DateTime ΔΔ;}IMyBroadcastListener ΑЄ=null;
long ъ=0;List<Ȧ>м=new List<Ȧ>();Vector3 Ć=new Vector3();float Ǳ=-1;Vector3 ΔΘ=new Vector3();bool ΑЃ=false;DateTime ΔΙ=DateTime.Now;const float ΔА=15f;const float Ηπ=10f;int Ζπ=0;String С="";int Ѐ=-1;bool ΓΕ=false;Vector3 ΕΧ=new Vector3();String ƺ(){if(ή==Λ.з||Η==Ζ.Ϊ) return "";
return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+М+"|"+б+"|"+(int) ġ+"|"+Math.Round(ǰ,1)+"|"+Math.Round(ǯ,1);}void ΗϏ(){if(!trafficEnabled) return;if(ΑЄ==null) ΑЄ=IGC.RegisterBroadcastListener(ΔБ);ъ=Me.CubeGrid.EntityId;int ž=30;while(ΑЄ.HasPendingMessage&&ž-->0){String Ϋ=ΑЄ.AcceptMessage().Data as String;if(Ϋ!=null) Ηά(Ϋ);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΔΔ).TotalSeconds>Ηρ) м.RemoveAt(i);if(ΑΙ==null) ΑΙ=IGC.RegisterBroadcastListener(ΗΥ);ž=10;while(ΑΙ.HasPendingMessage&&ž-->0){String Ϋ=ΑΙ.AcceptMessage().Data as String;if(Ϋ!=null) try{ΗΪ(Ϋ);}catch{}}if(++Ζπ<3) return;Ζπ=0;IGC.SendBroadcastMessage(ΔБ,ъ+";"+τ(ί)+";"+
τ(Υ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ƺ()+";"+
(ή==Λ.Є?Ѐ:-1)+";"+С+";"+ΖЄ()+","+Δζ()+";"+τ(ΔΘ)+
";"+(ΑΠ?1:0)+";"+(Ğ&&ή==Λ.Є?1:0)+","+Αϊ+","+Αω);}void Ηά(String Ϋ){try{String[]f=Ϋ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ъ) return;Ȧ Ж=null;for(int i=0;i<м.Count;i++) if(м[i].λ==λ) Ж=м[i];if(Ж==null){Ж=new Ȧ();Ж.λ=λ;м.Add(Ж);}
Ж.ζ=μ(f[1]);Ж.Ţ=μ(f[2]);Ж.Εΰ=float.Parse(f[3]);Ж.Ŋ=f[4];Ж.Γί=int.Parse(f[5]);Ж.ΔΔ=DateTime.Now;if(Ж.Ŋ!=""&&Ж.Ŋ==ƺ()) ΔЂ(f[6]);Ж.ά=0;Ж.Ƅ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ж.ά=long.Parse(d[0]);Ж.Ƅ=int.Parse(d[1]);}Ж.ƌ=f.Length>=9?μ(f[8]):new Vector3();
Ж.ΓЂ=f.Length>=10&&f[9]=="1";}catch{}}bool ΗΤ(long λ){for(int i=0;i<м.Count;i++) if(м[i].λ==λ) return true;return false;}Vector3 Δπ(int Β,int Θξ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Χ Ψ=Σ[Β];Vector3 ΙΒ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θξ;
Vector3 ώ=Ψ.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.ν):-Ψ.α;Vector3 Ь=Vector3.Cross(ΙΒ,ώ);if(Ь.LengthSquared()<0.0001f) return new Vector3();float ό=laneOffset*θ;float ΘΜ=Math.Min(Vector3.Distance(Ψ.Δ,Σ[0].Δ),
Vector3.Distance(Ψ.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ό*Math.Min(1,ΘΜ/(ό*4));}Vector3 Ζο=new Vector3();bool ΗΣ(Α s){return s==Α.Ǡ||s==Α.ĉ||s==Α.č||s==Α.Ų||
s==Α.ǐ||s==Α.Ȍ||s==Α.Ƞ;}void ΗЀ(){Ć=new Vector3();Ǳ=-1;ΔΘ=new Vector3();bool Επ=Δϊ(Φ);if(!trafficEnabled||(!Επ&&!ΗΣ(Φ))){ΑЃ=false;return;}Ζο=Υ.GetShipVelocities().LinearVelocity;ΖЅ(Υ.GetNaturalGravity(),!Επ);}void ΖЅ(Vector3 ν,bool ΖΟ=false){Ć=new Vector3();Ǳ=-1;
Vector3 ǭ=φ-ί;float Ǭ=ǭ.Length();bool Γά=Ǭ>=1f;Vector3 κ=Γά?ǭ/Ǭ:new Vector3();ΔΘ=κ;Vector3 ώ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɔ;Vector3 Ь=Vector3.Cross(κ,ώ);Ь=Ь.LengthSquared()<0.0001f?-Ƒ:Vector3.Normalize(Ь);Ȧ Αγ=null;bool Εσ=Φ==Α.Ѕ&&!Ȏ(1);
for(int i=0;i<м.Count;i++){Ȧ δ=м[i];if(δ.Ƅ==3&&δ.Ţ.LengthSquared()<0.25f) continue;Vector3 ζ=δ.ζ+δ.Ţ*(float)(DateTime.Now-δ.ΔΔ).TotalSeconds;Vector3 ŗ=ζ-ί;float Ǔ=(θ+δ.Εΰ)*0.5f;float Ź=Γά?Vector3.Dot(ŗ,κ):0;if(Ź>0&&(ŗ-κ*Ź).Length()<=Ǔ*1.2f){float Ας=Vector3.Dot(δ.Ţ,κ);
bool Εω=δ.ƌ.LengthSquared()>0.25f;bool Γέ=Ας<-1f||(Εω&&Vector3.Dot(δ.ƌ,κ)<-0.5f);if(ΖΟ){if(Γέ&&ъ<δ.λ) continue;}else{if(δ.ΓЂ){if(!Εσ){Ć=Ь*Ǔ*1.3f;continue;}float ΖΕ=Vector3.Distance(ί,φ),ΕЄ=Vector3.Distance(ζ,φ);if(ΖΕ<ΕЄ||(ΖΕ==ΕЄ&&ъ<δ.λ)) continue;}if(Γέ)
{if(Ź<(ț-Ας)*6f+Ǔ*4f) Ć=Ь*Ǔ*1.3f;continue;}if(Εω&&ъ<δ.λ){Vector3 ΕΜ=-ŗ;float ΖΛ=Vector3.Dot(ΕΜ,δ.ƌ);if(ΖΛ>0&&(ΕΜ-δ.ƌ*ΖΛ).Length()<=Ǔ*1.2f){Ć=Ь*Ǔ*1.3f;continue;}}}float Ες=Ź-Ǔ-followGap*θ;float Γκ=Math.Max(0,Ας)+Math.Max(0,Ες)/ΒΞ;if(Γέ) Γκ=Math.Max(0,Ες+Math.Min(0,Ας)*ΒΞ*2)/ΒΞ;
if(Ǳ<0||Γκ<Ǳ){Ǳ=Γκ;Αγ=δ;}continue;}if(δ.Ţ.LengthSquared()<1f) continue;if(Γά&&(ŗ-κ*Ź).Length()<=Ǔ*1.2f) continue;Vector3 ΓЇ=δ.Ţ-Ζο;float ΖΤ=ΓЇ.LengthSquared();if(ΖΤ<0.25f) continue;float t=-Vector3.Dot(ŗ,ΓЇ)/ΖΤ;if(t<=0||t>5f||(ŗ+ΓЇ*t).Length()>=Ǔ*1.1f) continue;
if(ъ>δ.λ||t<2f){Ǳ=0;Αγ=δ;}}bool ΘΤ=Ǳ>=0&&Ǳ<0.5f;if(!ΘΤ||ΖΟ||(Αγ!=null&&Αγ.ΓЂ&&Εσ)){ΑЃ=false;return;}if(!ΑЃ){ΑЃ=true;ΔΙ=DateTime.Now;}double Εϊ=(DateTime.Now-ΔΙ).TotalSeconds;if(Εϊ>ΔА+Ηπ) ΔΙ=DateTime.Now;else if(Εϊ>ΔА){Ǳ=-1;Ć=Ь*θ*1.3f;Μ="Traffic: passing a stuck ship";}
}List<int>Ȃ=new List<int>();String Εό="";void ΒΝ(){String Ģ=М+"|"+б+"|"+(int) ġ;if(Ģ==Εό) return;Εό=Ģ;Ζϋ(М,б,ġ,Ȃ);}void Ζϋ(int w,int h,Ǐ ΓЎ,List<int>ΒБ){ΒБ.Clear();if(ΓЎ==Ǐ.ǈ){for(int k=0;k<w*h;k++){int Ř=k/w;ΒБ.Add(Ř*w+(Ř%2==0?k-Ř*w:w-1-(k-Ř*w)));
}return;}int Εψ=(int) Math.Ceiling(w/2f),Εχ=(int) Math.Ceiling(h/2f);int ΘΟ=w/2,ΘΞ=h/2;int x=0,y=0,ΓΝ=0,ΓΞ=-1,Αι=Math.Max(w,h);for(int i=0;i<Αι*Αι;i++){if(-Εψ<x&&x<=ΘΟ&&-Εχ<y&&y<=ΘΞ) ΒБ.Add((y-1+Εχ)*w+(x-1+Εψ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΝ;ΓΝ=-ΓΞ;ΓΞ=t;}
x+=ΓΝ;y+=ΓΞ;}}bool ΗΙ(int a,int b,int w,float Ζς,float Ζλ,float Ερ,float Εκ){return a==b||(Math.Abs(a%w-b%w)*Ζς<Ερ+0.5f&&Math.Abs(a/w-b/w)*Ζλ<Εκ+0.5f);}bool ΒΜ(){int n=Math.Max(0,М*б);if(С.Length==n) return false;С=new String('0',n);Ѐ=-1;return true;
}void ΔЂ(String δ){ΒΜ();if(δ.Length!=С.Length) return;char[]h=С.ToCharArray();for(int i=0;i<h.Length;i++) if(δ[i]=='1') h[i]='1';С=new String(h);}bool Ƹ(int ǽ){return ǽ>=0&&ǽ<С.Length&&С[ǽ]=='1';}void Βτ(int ǽ,bool ф){if(ǽ<0||ǽ>=С.Length) return;char[]h=С.ToCharArray();
h[ǽ]=ф?'1':'0';С=new String(h);}bool Βά(int ǽ,bool ΘΪ){if(!trafficEnabled) return false;String Ģ=ƺ();for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.Ŋ!=Ģ||t.Γί<0||(ΘΪ&&t.λ>ъ)) continue;if(ΗΙ(ǽ,t.Γί,М,ǰ,ǯ,Αϊ,Αω)) return true;}return false;}int ΖЃ(int Β){int n=М*б;
if(n<1) return Β;ΒΝ();if(ΒΜ()) for(int k=0;k<Β&&k<n;k++) Βτ(Ȃ[k],true);if(Ѐ>=0&&(Β>=n||Ȃ[Β]!=Ѐ)){Βτ(Ѐ,true);Ѐ=-1;}if(Δό()){if(ǧ>=0&&!Ƹ(ǧ)){Ğ=false;Ѐ=ǧ;return Ȃ.IndexOf(ǧ);}if(ǧ==-2){Ğ=false;Ѐ=-1;return n;}if(ǧ==-1&&Ѐ>=0&&!Ƹ(Ѐ)){Ğ=false;return Β;}
Ѐ=-1;if(!Ğ){Ğ=true;ΕΩ=DateTime.Now;}if((DateTime.Now-ΕΩ).TotalSeconds<5) return-1;}Ğ=false;if(Ѐ>=0&&!Ƹ(Ѐ)&&!Βά(Ѐ,true)) return Β;Ѐ=-1;for(int k=0;k<n;k++){int c=Ȃ[k];if(Ƹ(c)||Βά(c,false)) continue;Ѐ=c;return k;}return n;}const String ΗΥ="[PAMJOB]";IMyBroadcastListener ΑΙ=null;
DateTime ΕΨ=DateTime.MinValue;DateTime ΕΩ=DateTime.Now;int ǧ=-1;bool Ğ=false;bool Δό(){return trafficEnabled&&ǧ!=-3&&(DateTime.Now-ΕΨ).TotalSeconds<3;}void ΗΪ(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƺ()) return;ΕΨ=DateTime.Now;
ΔЂ(f[2]);ǧ=-3;String ΖΑ=ъ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΖΑ)) ǧ=int.Parse(e[i].Substring(ΖΑ.Length));}ķ Ηθ(int Β){ΒΝ();if(Β<0||Β>=Ȃ.Count) return ķ.ΔΩ;Ш=Ȃ[Β]%М;Щ=Ȃ[Β]/М;return ķ.Δκ;}class ѐ{public long λ;public Vector3 ζ,Б,ώ;
}List<ѐ>Ѝ=new List<ѐ>();bool ȫ=false;Vector3 ΑΣ,ΑΡ,ΑΤ,ΑΥ;long х=0;long Ȅ=0;Χ ј=null;bool ƅ=false;Χ ΒΙ(Vector3 ζ,Vector3 Б,Vector3 ώ){MatrixD m=MatrixD.CreateWorld(ζ,Б,ώ);Χ w=new Χ(Θ);w.β=Vector3D.TransformNormal(ΑΣ,m);w.α=Vector3D.TransformNormal(ΑΡ,m);w.ψ=Vector3D.TransformNormal(ΑΤ,m);
w.Δ=ζ+(Vector3) Vector3D.TransformNormal(ΑΥ,m);w.γ=true;return w;}int Δζ(){Α s=Φ;if(s==Α.ж) return О(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.č||s==Α.Ǻ||s==Α.Č||s==Α.ΑΑ||
s==Α.Ʋ||s==Α.ǐ) return 3;if(s==Α.Ǡ||s==Α.ĉ||s==Α.Ų) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЄ(){if(Δζ()==0) return 0;if(Ȏ(1)&&Ŝ!=0) return Ŝ;return ј!=null?Ȅ:х;}void ΔϏ(){if(Η==Ζ.Ϊ||!Θ.γ) return;IMyShipConnector И=О(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector δ=И.OtherConnector;Vector3I ΘΥ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==ΘΥ){MatrixD Ň=MatrixD.Transpose(δ.WorldMatrix);ΑΣ=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,Ň);ΑΡ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,Ň);ΑΤ=Vector3D.TransformNormal(Υ.WorldMatrix.Left,Ň);
ΑΥ=Vector3D.TransformNormal(И.GetPosition()-δ.GetPosition(),Ň);ȫ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) х=δ.EntityId;List<IMyShipConnector>Ĝ=new List<IMyShipConnector>();ω.GetBlocksOfType(Ĝ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ĝ.Count>0){Ѝ.Clear();for(int i=0;i<Ĝ.Count;i++){ѐ p=new ѐ();p.λ=Ĝ[i].EntityId;p.ζ=Ĝ[i].GetPosition();p.Б=Ĝ[i].WorldMatrix.Forward;p.ώ=Ĝ[i].WorldMatrix.Up;Ѝ.Add(p);}}ј=null;Ȅ=0;if(δ.EntityId==х||!ȫ) return;for(int i=0;i<Ѝ.Count;i++)
if(Ѝ[i].λ==δ.EntityId){Ȅ=Ѝ[i].λ;ј=ΒΙ(Ѝ[i].ζ,Ѝ[i].Б,Ѝ[i].ώ);}}bool Βί(long λ,int ΖΖ){if(λ==0) return false;for(int i=0;i<м.Count;i++){Ȧ t=м[i];if(t.ά==λ&&(t.Ƅ>ΖΖ||(t.Ƅ==ΖΖ&&t.λ<ъ))) return true;}return false;}bool ΒΔ(int ΓϏ){if(Η==Ζ.Ϊ||!trafficEnabled){ј=null;Ȅ=0;return true;}
long η=ј!=null?Ȅ:х;if(!Βί(η,ΓϏ)) return true;if(!Βί(х,ΓϏ)){ј=null;Ȅ=0;return true;}if(!ȫ) return false;ѐ ė=null;float ΕΟ=0;for(int i=0;i<Ѝ.Count;i++){ѐ p=Ѝ[i];if(p.λ==х||Βί(p.λ,ΓϏ)) continue;float d=Vector3.Distance(p.ζ,Θ.Δ);if(ė==null||d<ΕΟ){ė=p;ΕΟ=d;}
}if(ė==null) return false;Ȅ=ė.λ;ј=ΒΙ(ė.ζ,ė.Б,ė.ώ);return true;}String ΖЍ(){String s=(ȫ?"1":"0")+":"+τ(ΑΣ)+":"+τ(ΑΡ)+":"+
τ(ΑΤ)+":"+τ(ΑΥ)+":"+х;for(int i=0;i<Ѝ.Count;i++)
s+=":"+Ѝ[i].λ+","+τ(Ѝ[i].ζ)+","+τ(Ѝ[i].Б)+","+τ(Ѝ[i].ώ);return s;}void ΖЌ(String s){Ѝ.Clear();ȫ=false;х=0;String[]f=s.Split(':');if(f.Length<6) return;ȫ=f[0]=="1";ΑΣ=μ(f[1]);ΑΡ=μ(f[2]);ΑΤ=μ(f[3]);ΑΥ=μ(f[4]);long.TryParse(f[5],out х);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();if(!long.TryParse(e[0],out p.λ)) continue;p.ζ=μ(e[1]);p.Б=μ(e[2]);p.ώ=μ(e[3]);Ѝ.Add(p);}}const String ΒΖ="[PAMDCK]";const float ΙΠ=120f;class Δη{public long Ж;public int Ə;public long Γΰ;public bool Ια;public DateTime Ιχ,Ιύ;
}IMyBroadcastListener Ιτ=null;List<Δη>Ιυ=new List<Δη>();List<IMyShipConnector>ΙІ=new List<IMyShipConnector>();List<IMySensorBlock>ΙЎ=new List<IMySensorBlock>();List<IMyCameraBlock>Ιό=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΙЍ=new List<MyDetectedEntityInfo>();long Ιδ=0;
int Ιγ=0;long Ιβ=0;DateTime Ιε=DateTime.Now;String Ισ="";int Ιφ=0;IMyCubeGrid ΙЇ=null;IMyBroadcastListener ΑϏ=null;int ȁ=0;DateTime Εδ=DateTime.Now;DateTime Ζά=DateTime.MinValue;long Ζέ=0;int Ζή=0;long Ŝ=0;bool Ƨ=false;int ΓΜ=0;List<Vector3>Ʃ=new List<Vector3>();List<float>ΔΧ=new List<float>();
bool ΑΠ=false;bool ΑΟ=false;Vector3 Εγ=new Vector3();bool ŵ(){return ȁ!=0&&(DateTime.Now-Ζά).TotalSeconds<3;}bool Ȏ(int Ə){return ŵ()&&Ζέ==ъ&&Ζή==Ə;}bool ΔЌ(){return ŵ()||(DateTime.Now-Εδ).TotalSeconds<3;}void Βς(int Ə){if(ȁ==Ə) return;ȁ=Ə;Εδ=DateTime.Now;ΓΜ=3;
}void Βρ(){if(ȁ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΖ,"D;"+ъ);ȁ=0;Ƨ=false;}void Ηχ(){if(!trafficEnabled||Η==Ζ.Ϊ) return;if(ΑϏ==null) ΑϏ=IGC.RegisterBroadcastListener(ΒΖ);int ž=20;while(ΑϏ.HasPendingMessage&&ž-->0){String Ϋ=ΑϏ.AcceptMessage().Data as String;if(Ϋ==null||!Ϋ.StartsWith("S;")) continue;
try{ΗΫ(Ϋ);}catch{}}if(Φ!=Α.Ѕ){ΑΠ=false;ΑΟ=false;}if(ȁ==1&&О(MyShipConnectorStatus.Connected)!=null) Βρ();if(ȁ==2&&!Ƨ&&Φ!=Α.ǐ&&
Vector3.Distance(ί,е().Δ)>θ*3) Βρ();if(ȁ!=0&&++ΓΜ>=3){ΓΜ=0;IGC.SendBroadcastMessage(ΒΖ,"R;"+ъ+";"+ȁ+";"+х+";"+(ȫ?1:0));}}void ΗΫ(String Ϋ){String[]f=Ϋ.Split(';');if(f.Length<7) return;bool Εο=false;String[]ΕΒ=f[5].Split(',');for(int i=0;i<ΕΒ.Length;i++) if(ΕΒ[i]==ъ.ToString()) Εο=true;
if(!Εο) return;Ζά=DateTime.Now;Ζέ=long.Parse(f[2]);Ζή=int.Parse(f[3]);Ŝ=long.Parse(f[4]);Ʃ.Clear();ΔΧ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʃ.Add(μ(e[0]));ΔΧ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εζ=f[6].Split(':');List<ѐ>Т=new List<ѐ>();for(int i=0;i<Εζ.Length;i++){String[]e=Εζ[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();p.λ=long.Parse(e[0]);p.ζ=μ(e[1]);p.Б=μ(e[2]);p.ώ=μ(e[3]);Т.Add(p);}Ѝ=Т;}bool ΖЁ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;
Βς(1);if(!ΔЌ()) return true;if(!Ȏ(1)){Μ="Waiting for dock clearance";return false;}ΑЍ();return true;}bool ΑЍ(){ƅ=false;ѐ ĥ=null;if(Ŝ!=0&&Ŝ!=х&&ȫ)
for(int i=0;i<Ѝ.Count;i++) if(Ѝ[i].λ==Ŝ) ĥ=Ѝ[i];long ΑЉ=ĥ==null?0:ĥ.λ;if(ΑЉ==Ȅ&&(ΑЉ==0)==(ј==null)) return false;Ȅ=ΑЉ;ј=ĥ==null?null:ΒΙ(ĥ.ζ,ĥ.Б,ĥ.ώ);return true;}bool ĳ(Vector3 p){for(int i=0;i<Ʃ.Count;i++) if(Vector3.Distance(p,Ʃ[i])<ΔΧ[i]+θ*0.75f) return true;
return false;}bool ΗΘ(Vector3 Ɓ,Vector3 ő){ΑΠ=false;if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return false;if(ȁ!=1&&Vector3.Distance(ί,Θ.Δ)>dockRequestDistance) return false;Βς(1);if(!ŵ()||Ȏ(1)||Ʃ.Count==0) return false;if(!ĳ(Ɓ)&&!ĳ(ő)&&!ĳ(ί)) return false;ΑΠ=true;return true;
}bool Δΰ(){if(!trafficEnabled||Η==Ζ.Ϊ||х==0) return true;Βς(2);if(!ΔЌ()) return true;if(!Ȏ(2)){Μ="Waiting for undock clearance";return false;}return true;}const float ΙΧ=600f;class Δώ{public long λ;public int ǽ=-1;public bool ΑЉ;public float Ερ,Εκ;public DateTime ΙЀ;}class Δύ{public String Ģ;
public int w,h;public float Ζς,Ζλ;public String ф;public List<int>ΙϏ=new List<int>();public List<Δώ>ΙЂ=new List<Δώ>();}List<Δύ>Ιϋ=new List<Δύ>();IMyBroadcastListener Ιϊ=null;int Ιω=0;void ΖЉ(string ђ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}