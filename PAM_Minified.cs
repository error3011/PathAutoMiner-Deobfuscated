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
const float drillRadius = 1.4f;//[m]

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
public enum Ι{Ĕ,ƛ,ǟ,ё,ά}public enum Β{о,đ,Ɯ,Ȫ,Į,Е,н,
Đ,Ŗ,Ƕ,ī,ħ,Č,
ƞ,Ȗ,Ǣ,Ī,Αω,Αή,
Γς,Αψ,Ɣ,Αΰ,ĕ}public enum Π{ч,ś,Д,ΒЃ,ΑΪ,Αι,Ακ,ő,Œ}public enum ѐ{None,Ī,о,ΕΫ}public enum Ρ{Main,ΕЂ,ƚ,ƙ,Γξ,ΑΨ,
Βς,Εξ,Εο,Εν,Ơ,ΕЇ,
Ας,Γε,Γι,ΕЈ,Ατ}public enum р{Αρ,ƒ,Θξ,Ες,ά}public enum Ǐ{Ƣ,ΕΤ}public enum х{Απ,ΒЉ,ΕЋ,Ɩ}public enum Ѝ{ΓΤ,ΕΨ,Βώ,ΒЄ,Αά,ΓΓ,Αε}public enum ў{ΒЀ,Βχ,Βψ}public enum Х{ř,ΘΟ,Ĩ}public enum π{ΓΦ,ΓΪ,ΓΫ,Εύ,Εό,
ΓΥ,Αν,Αμ,ΓΨ,
ΓΩ,ΓΧ}public enum ΰ{Ȑ,ȩ,ȏ,ΑΧ,Βυ,Βφ}public enum ǡ{Αγ,ΑΦ,ΗЌ}public enum ђ{Αυ,Εέ,ΘΩ,ΓΡ}public enum О{None,Ɲ,Γΰ,ΓΣ}public enum ϊ{Ƥ,ΑΥ,Αη,Ɩ,ΓΟ,Αθ,ΓΠ,Θσ,Γά}public enum П{Ĕ,Εϊ,Εω,ΓΜ,ΓΕ,ΒϏ,Εϋ}class Ϊ{public bool ζ=false;public Vector3 Ε=new Vector3();
public Vector3 γ=new Vector3();public Vector3 δ=new Vector3();public Vector3 Ѐ=new Vector3();public Vector3 φ=new Vector3();public Vector3 и=new Vector3();public float Ф=0;public float Κγ=0;public float[]ǳ=null;public Ϊ(){}public Ϊ(Ϊ ι){ζ=ι.ζ;Ε=ι.Ε;γ=ι.γ;δ=ι.δ;Ѐ=ι.Ѐ;φ=ι.φ;
и=ι.и;ǳ=ι.ǳ;}public Ϊ(Vector3 Ε,Vector3 δ,Vector3 γ,Vector3 Ѐ,Vector3 φ){this.Ε=Ε;this.γ=γ;this.δ=δ;this.Ѐ=Ѐ;this.Ф=0;this.φ=φ;}public void Ηψ(List<IMyThrust>Αό,List<string>Ȣ){ǳ=new float[Ȣ.Count];for(int i=0;i<ǳ.Length;i++)
ǳ[i]=-1;for(int i=0;i<Αό.Count;i++){string ѝ=Ελ(Αό[i]);int Α=Ȣ.IndexOf(ѝ);if(Α!=-1)
ǳ[Α]=Б(Αό[i].MaxEffectiveThrust,Αό[i].MaxThrust);}}}class Γέ{public Ϊ μ=null;public List<Vector3>Ɖ=new List<Vector3>();public float ư=0;public float ΕΕ=0;public float ţ=0;public float ΔΗ=0;public Vector3 ΑБ=new Vector3();}class ş{public π Н=π.ΓΦ;public float Ǧ=0;public float ě=0;
public string ǧ="";public string Ǫ="";DateTime ΖІ;public bool Δχ=false;private bool ΓЋ=false;public bool Βϋ(bool ǰ){if(ǰ){ě=0;ΓЋ=false;return false;}ΓЋ=true;return ě>Ǧ;}public void Θί(){Βϋ(true);Δχ=false;}public void ΘЁ(){if(ΓЋ)
if((DateTime.Now-ΖІ).TotalSeconds>1){ě++;ΖІ=DateTime.Now;}}public bool ΒБ(){switch(Н){case π.ΓΪ:return true;case π.ΓΫ:return true;}return false;}}class ж{public String ή="";public String υ="";public int Ѕ=0;public Х Ŵ=Х.Ĩ;public ж(String ή,String υ,int Ѕ,Х Ŵ){this.ή=ή;this.υ=υ;
this.Ѕ=Ѕ;this.Ŵ=Ŵ;}}class ΗЇ{public ΗЇ(Vector3 ο,float І){this.ο=ο;this.І=І;}public Vector3 ο;public float І;}class К{public DateTime Δά;public DateTime ΖΨ;public String ε="";public String ή="";public String Βο="";public String Λ="";public String ΔЅ="";public String ΒΧ="";public String Δυ="";
public Vector3 Ε=new Vector3();public List<ж>Ї=new List<ж>();public Ι Κ=Ι.Ĕ;public О ė;public float ȅ;public float ǿ;public int Љ;public int ΓБ=0;public int ΔБ=0;public bool Εχ(){return(DateTime.Now-Δά).TotalSeconds>10;}public bool ΓΖ(){if(ė!=О.Ɲ) return false;return(DateTime.Now-ΖΨ).TotalSeconds>=4;
}public К(String ε){this.ε=ε;}public О Ŝ(String Ж,bool ΙЈ,bool ш,bool ΘА){if(Ж==""&&!ш) return О.None;if(ė==О.Ɲ&&ΓΖ())
ė=О.ΓΣ;if(ΙЈ){Δυ=Ж;ė=О.Ɲ;ΖΨ=DateTime.Now;return ė;}else if(ш){Δυ="";ė=О.None;}else if(Δυ==Ж){if(ΘА) ė=О.Γΰ;return ė;}return О.None;}}public String ΘΒ(String Ιγ){switch(Ιγ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 Βή=new Vector3();Vector3 β=new Vector3();Vector3 ΒΓ=new Vector3();Vector3 ǅ=new Vector3();Vector3 ƴ=new Vector3();
DateTime Δί=new DateTime();bool ȯ=true;int ΔЋ=0;bool ȳ=false;bool ǁ=false;bool Ǌ=true;bool Ης=false;bool Ű=false;bool ΒΘ=false;float Ć=0;float ŋ=0;int ģ=0;int ΑΝ=0;int ΑΒ=0;int ΓЃ=0;float ŷ=0;float Ť=0;double Δλ=0;float Ʃ=0;List<int>ŭ=new List<int>();List<int>Ɓ=new List<int>();
Ι Κ=Ι.Ĕ;int Ћ=0;float ȍ=0;float Ǵ=0;float λ=0;IMyRemoteControl Ω;IMySensorBlock ψ;List<IMyTimerBlock>ΔЎ=new List<IMyTimerBlock>();List<IMyShipConnector>ѓ=new List<IMyShipConnector>();List<IMyThrust>ȣ=new List<IMyThrust>();List<IMyGyro>ΒΔ=new List<IMyGyro>();List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ƞ=new List<IMyLandingGear>();List<IMyReactor>Ģ=new List<IMyReactor>();List<IMyConveyorSorter>Βγ=new List<IMyConveyorSorter>();List<IMyGasTank>ǒ=new List<IMyGasTank>();List<IMyTerminalBlock>ƥ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ŧ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ĵ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ė=new List<IMyBatteryBlock>();List<IMyTextPanel>У=new List<IMyTextPanel>();List<IMyTextSurface>Ȋ=new List<IMyTextSurface>();List<IMyTextPanel>ķ=new List<IMyTextPanel>();IMyTerminalBlock ǖ=null;Ϊ Ξ=new Ϊ();Ϊ ΔΦ=new Ϊ();
Ϊ ΔΩ=new Ϊ();bool Ȥ=false;bool Ƌ=false;bool ƌ=false;bool ΒΚ=false;List<String>Ȣ=new List<string>();List<Ϊ>Φ=new List<Ϊ>();List<Ϊ>ΑΣ=new List<Ϊ>();int ΔΑ=0;double Ησ=0;Ϊ Ζ=new Ϊ();Vector3 ǭ;Vector3 ă;Π α=Π.ч;Ǐ ľ=Ǐ.Ƣ;int ξ=0;int И=0;double ǃ=0;bool ΑЈ=false;ş Ѓ=new ş();
ş Є=new ş();х ь=х.Απ;bool ĥ=true;Ǐ Ȉ=Ǐ.Ƣ;int ъ=3;int щ=3;int з=30;ў є=ў.ΒЀ;bool ċ=true;bool ů=true;bool ũ=true;bool Ō=false;Ѝ Ю=Ѝ.ΓΤ;bool Ċ=true;float Ȅ=90;float ј=20;float к=5;float љ=20;float ǹ=0.70f;float Ɛ=1.50f;float Ə=2.50f;float Ő=0f;float ĝ=0f;Β Ψ;
Π ї;Β ΑΖ;Γέ κ=null;int ώ=0;int ό=0;int Ь=0;int ā=0;int ŉ=30;int д=0;bool Ў=true;Vector3 њ;double Э=0;bool Βΰ=false;int ΕΙ=0;int Џ=0;int Γω=0;int Ȇ=0;int ΗΚ=0;Vector3 ǌ=new Vector3();float ы=0;float м=0;int ŏ=0;Ϊ Βδ=null;float Δέ=0;float Αώ=0;float г=0;float Ł=0;
bool ΔΧ=false;bool ō=false;bool ΑΞ=false;bool ȱ=false;bool Ȭ=false;DateTime ΙΠ=new DateTime();Ϊ ĵ=null;Ϊ Ζή=null;float Ș=0;ѐ Ǻ;float Ȳ=0;float Ŏ=0;String Δζ="";float ΒΝ=0;float ǿ=0;float ȅ=0;List<ж>ǩ=new List<ж>();float Я=0;bool ΑΔ=false;bool Ηη=false;bool ΗΦ=false;
bool Βη=false;bool ΗΧ=false;float Δι=0;float Δτ=0;Vector3 Ζς=new Vector3();Vector3 Ђ=new Vector3();Vector3 Βθ=new Vector3(1,1,1);float ΔЊ=1;Vector3 ΑΚ=new Vector3();bool ΒΙ=false;bool ΔΝ=false;bool Δγ=false;bool ΖΔ=false;float ΑІ=0;float Αϋ=2;Vector3 Ηζ;Vector3 ΔЉ;Vector3 ΔЈ;
float[,]ǵ=new float[3,2];Dictionary<String,float[,]>ǚ=new Dictionary<string,float[,]>();Dictionary<String,float>Ηπ=new Dictionary<String,float>();int Γύ=0;Dictionary<String,float[]>Ą=new Dictionary<string,float[]>();float Δψ;bool ΑЉ=false;int Αύ=0;int ğ=0;int ΒΖ=0;int ļ=0;
String Λ="";int[]ΗΒ=new int[Enum.GetValues(typeof(Ρ)).Length];bool Δν=false;int ł=0;int Љ=0;Ρ ϋ=Ρ.Main;int ΚΕ=0,ΚΖ=0,ΚΗ=0,ΚΘ=0;int ΙЃ=0,ΙЄ=0;bool Ŧ=false;bool ΖΕ=false;String Ɔ="";String ΓЂ="";IMyBroadcastListener Ǥ=null;bool Ζρ=true;List<К>Ы=null;К Г=null;
int ΑЂ=0,ΑЃ=0;int ΖΫ=0,Ζά=0;int ΙΕ=0,ΙΖ=0,ΙΗ=0;String Ιε="";Exception Ā=null;bool Βί=false;Π ƃ=Π.ч;bool Ζα=false;bool ΖЃ=true;bool Δύ=false;String Ηρ="";bool ΑΜ=false;bool ΔЃ=true;int ΒΗ=0;int[]ǘ=null;Vector3 ΓЄ=new Vector3();bool Ζφ=false;bool џ(IMyTerminalBlock Χ)=>Χ.CubeGrid==Me.CubeGrid;
Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΓΑ(Me,controllerTag,true))
Κ=Ι.ё;θ(Ρ.Main);if(Κ!=Ι.ё){Λ="Welcome to [PAM]!";ђ Δό=Θΰ();if(Κ==Ι.Ĕ){List<IMyShipDrill>Ζξ=new List<IMyShipDrill>();List<IMyShipGrinder>Ζχ=new List<IMyShipGrinder>();χ.GetBlocksOfType(Ζξ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(Ζχ,q=>q.CubeGrid==Me.CubeGrid);if(Ζξ.Count>0)
{Κ=Ι.ƛ;Λ="Miner mode enabled!";}else if(Ζχ.Count>0){Κ=Ι.ǟ;Λ="Grinder mode enabled!";}else{Κ=Ι.ά;Ȕ(р.ά);}}if(Δό==ђ.Αυ) Ǌ=false;if(Δό==ђ.Εέ) Λ="Data restore failed!";if(Δό==ђ.ΓΡ) Λ="New version, wipe data";}}void Main(string Р,UpdateType ΚΠ){try{if(Ā!=null){if(Р.Trim().ToUpper()=="RETRY")
{Ā=null;ȯ=true;ģ=0;if(Κ!=Ι.ё) Ϗ();Λ="Restarted after error";}else{ΘΛ();ΕЀ();return;}}ǁ=(ΚΠ&UpdateType.Update10)!=0;if(ǁ) ΔЋ++;ȳ=ΔЋ>=10;if(ȳ) ΔЋ=0;if(ǁ){Αύ++;if(Αύ>4) Αύ=0;ΑΒ=(int) Math.Max(0,10-(DateTime.Now-Δί).TotalSeconds);}if(Р!="") Ιε=Р;if(Κ!=Ι.ё)
Θν(Р);else
ΗЅ(Р);Δν=false;try{int Ζί=Runtime.CurrentInstructionCount;float ň=Б(Ζί,Runtime.MaxInstructionCount);if(ň>0.90) Λ="Max. instructions >90%";if(ň>ŷ) ŷ=ň;if(ΑЉ){ŭ.Add(Ζί);while(ŭ.Count>10) ŭ.RemoveAt(0);Ť=0;for(int i=0;i<ŭ.Count;i++) Ť+=ŭ[i];Ť=Б(Б(Ť,ŭ.Count),Runtime.MaxInstructionCount);
double Δή=Runtime.LastRunTimeMs;if(ΒΘ&&Δή>Δλ) Δλ=Δή;Ɓ.Add((int)(Δή*1000f));while(Ɓ.Count>10) Ɓ.RemoveAt(0);Ʃ=0;for(int i=0;i<Ɓ.Count;i++) Ʃ+=Ɓ[i];Ʃ=Б(Ʃ,Ɓ.Count)/1000f;}}catch{ŷ=0;}}catch(Exception e){Ā=e;}}void Θν(string Р){bool Βα=false;String ΖΠ="";if(Ћ<=1&&!ΓΙ(Р))
ΕЁ(Р);int Δξ=50;while(Ǥ!=null&&Ǥ.HasPendingMessage&&Δξ-->0){MyIGCMessage η=Ǥ.AcceptMessage();String Τ=η.Data as String;if(Τ==null) continue;String ȇ="";if(Ћ<=1&&ΕϏ(ref Τ,out ȇ,out ΖΠ)&&ȇ==BROADCAST_SELF_ID){ΕЁ(Τ);Βα=true;break;}}bool ΖϏ=ΒΘ&&Ψ==Β.о&&!Ű&&!Βα&&ģ==0&&!ΒΚ;
if(ȳ&&Ψ!=Β.о) Βα=true;if((ǁ&&!ΖϏ)||(ȳ&&ΖϏ)){if(ģ==0&&(ΑΒ<=0||ȯ)){Ης=Ћ>0;Ћ=0;ģ=1;ȕ();Θδ();Επ();ȑ("Scan 1");}else if(ģ==1){ģ=2;ȕ();Θε();ȑ("Scan 2");}else if(ģ==2){ģ=0;ȕ();Θζ();Θη();if(Ћ<=1) Εψ();ȑ("Scan 3");Δί=DateTime.Now;if(Ћ<=1&&Ǌ)
Βή=ΒЋ(Ω,Ω.CenterOfMass);Ǌ=false;if(ȯ){ȯ=false;bool ΚΥ=α==Π.Д;Ϗ();if(ΖЃ&&resumeAfterRestart&&ΚΥ&&Ћ<=1) Δύ=true;}ΖЃ=false;if(Ης&&Ћ==0) Λ="Setup complete";}else{if(α==Π.Д&&Κ!=Ι.ά){ȕ();Ητ();ȑ("Inv balance");}ȕ();switch(ΑΝ){case 0:Θφ();break;case 1:Θό();break;case 2:Θυ();break;
case 3:ΘЄ();break;case 4:Θϋ();break;case 5:Ηϋ();break;case 6:Ηϊ(Ω);break;}ȑ("Update: "+ΑΝ);ΑΝ++;if(ΑΝ>6){ΑΝ=0;ΒΘ=true;if(ƃ!=Π.ч){switch(ƃ){case Π.ő:Ď();break;case Π.Œ:Ď();break;case Π.Д:Ď();break;case Π.Αι:Αα();break;case Π.Ακ:Ĭ();break;}ƃ=Π.ч;}}}if(!ȯ){if(!ȓ(Ω,true))
{Ω=null;Ǌ=true;Ћ=2;}if(Ћ>=2&&Ψ!=Β.о)
Ϗ();if(Ћ<=1){ŋ=Ω.CalculateShipMass().PhysicalMass;Ć=(float) Ω.GetShipSpeed();β=Θτ(Ω,Βή);ΒΓ=Ω.WorldMatrix.Forward;ǅ=Ω.WorldMatrix.Left;ƴ=Ω.WorldMatrix.Down;ΘϏ();ΘЂ();Θψ();if(Δύ){Δύ=false;Ď();if(Ψ!=Β.о) Λ="Job resumed after restart";}if(Ψ!=Β.о){Ű=false;ΕЄ(false);ΕΡ(false);String ΙЍ=ΒЍ(Ψ)+" "+(int) Ψ;
ȕ();Θύ();Γρ(false);ȑ(ΙЍ);ΘЃ();Θώ();ȕ();Θϊ();ȑ("Thruster");ȕ();ΕБ();ȑ("Gyroscope");}else{if(Ű){if(Αζ()){е(ƴ,ΒΓ,ǅ,0.25f,true);ΕБ();Λ="Aligning to planet: "+Math.Round(ΑІ-0.25f,2)+"°";if(ΒΙ) ΒА(true,true);}else ΒА(true,true);}}ΑΜ=false;}}}ȕ();ΕЀ();ȑ("Print");if(Βα||ΓЃ<=0){ȕ();Ηυ(ΖΠ);
ȑ("Broadcast");ΓЃ=4;}else if(ȳ) ΓЃ--;}void ΕЁ(string Р){if(Р=="") return;var τ=Р.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ж=τ.ElementAtOrDefault(0);var ȗ=τ.ElementAtOrDefault(1);var ƨ=τ.ElementAtOrDefault(2);var ΖΘ=τ.ElementAtOrDefault(3);String ȟ="Invalid argument: "+Р;bool ΕΔ=false;
switch(Ж){case "UP":this.ΓΞ(false);break;case "DOWN":this.ΓΝ(false);break;case "UPLOOP":this.ΓΞ(true);break;case "DOWNLOOP":this.ΓΝ(true);break;case "APPLY":this.ΕΟ(true);break;case "MRES":Љ=0;break;case "STOP":this.Ϗ();break;case "PATHHOME":{this.Ϗ();this.Γν();}break;case "PATH":{this.Ϗ();this.Γν();Ξ.ζ=true;}break;
case "START":{this.Ϗ();Γμ();}break;case "ALIGN":{ΒА(!Ű,false);}break;case "CONT":{this.Ϗ();this.Ď();}break;case "JOBPOS":{this.Ϗ();this.Ĭ();}break;case "HOMEPOS":{this.Ϗ();this.Αα();}break;case "FULL":{ΑΔ=true;}break;case "RESET":{Βί=true;Ћ=2;}break;case "EXPORT":{ΗЍ();}break;
case "IMPORT":{ΘΝ(ȗ=="JOB");}break;default:ΕΔ=true;break;}if(Κ!=Ι.ά){switch(Ж){case "SHUTTLE":{ΗЊ();}break;case "CFGS":{if(!ΗЃ(ȗ,ƨ,ΖΘ)) Λ=ȟ;}break;case "CFGB":{if(!ΗЂ(ȗ,ƨ)) Λ=ȟ;}break;case "CFGL":{if(!ĭ(ref Ȅ,true,ϊ.Αη,ȗ,"")||!ΗЄ(ƨ))
Λ=ȟ;}break;case "CFGE":{if(!ĭ(ref к,true,ϊ.Αθ,ȗ,"IG")||
!ĭ(ref ј,true,ϊ.ΓΟ,ƨ,"IG")||
!ĭ(ref љ,true,ϊ.ΓΠ,ΖΘ,"IG"))
Λ=ȟ;}break;case "CFGA":{if(!ĭ(ref ǹ,false,ϊ.ΑΥ,ȗ,"")) Λ=ȟ;}break;case "CFGW":{if(!ĭ(ref Ɛ,false,ϊ.Ƥ,ȗ,"")||
!ĭ(ref Ə,false,ϊ.Ƥ,ƨ,""))
Λ=ȟ;}break;case "NEXT":{Βρ(false);}break;case "PREV":{Βρ(true);}break;default:if(ΕΔ) Λ=ȟ;break;}}else{switch(Ж){case "UNDOCK":{ΑΜ=true;}break;default:if(ΕΔ) Λ=ȟ;break;}}}String ΘΓ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Κ!=Ι.ά)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗЊ(){Ϗ();Κ=Ι.ά;Ȕ(р.ά);Ζ.ζ=false;Ξ.ζ=false;ψ=null;ω.Clear();α=Π.ч;}void ΒА(bool З,bool Ǒ){if(!З) Λ="Aligning canceled";if(Ǒ) Λ="Aligning done";if(Ǒ||!З){Ű=false;ΕЌ();Ασ(false,0,0,0,0);return;}if(Αζ()) Ű=true;}bool ΗЃ(String ΚΧ,String ΙΫ,String ΙΛ)
{bool ΚΣ=α==Π.Д;int w,h,d;if(int.TryParse(ΚΧ,out w)&&int.TryParse(ΙΫ,out h)&&int.TryParse(ΙΛ,out d)){this.Ϗ();ъ=w;щ=h;з=d;Ĳ(false);ƣ(false,false);if(ΚΣ) Ď();return true;}return false;}bool ΗЄ(String Ƨ){if(Ƨ=="ON"){ċ=true;return true;}if(Ƨ=="OFF"){ċ=false;return true;}
return false;}bool ΗЂ(String Ζμ,String ΑЇ){bool ń=true;if(Ζμ=="HOME") ĥ=true;else if(Ζμ=="STOP") ĥ=false;else ń=false;if(ΑЇ=="HOME") ь=х.Απ;else if(ΑЇ=="STOP") ь=х.ΕЋ;else if(ΑЇ=="JOB") ь=х.ΒЉ;else if(ΑЇ=="IG") ь=х.Ɩ;else ń=false;return ń;}void θ(Ρ с){ΗΒ[(int) ϋ]=Љ;
Љ=ΗΒ[(int) с];if(с==Ρ.Ơ) Љ=0;ϋ=с;if(Κ!=Ι.ё)
ı(ϋ==Ρ.ƚ,false,0,0);Δν=true;}void ΓΞ(bool Δδ){if(Љ>0) Љ--;else if(Δδ) Љ=ł-1;}void ΓΝ(bool Δδ){if(Љ<ł-1) Љ++;else if(Δδ) Љ=0;}String Ȕ(р η){switch(η){case р.Αρ:Λ="Job is running";break;case р.ƒ:Λ="Connector not ready!";break;case р.Θξ:Λ="Ship modified, path outdated!";break;
case р.Ες:Λ="Interrupted by player!";break;case р.ά:Λ="Shuttle mode enabled!";break;}return "";}String ΘΘ(Ǐ ο){switch(ο){case Ǐ.Ƣ:return "Top-Left";case Ǐ.ΕΤ:return "Center";default:return "";}}String ΗА(ў Њ){switch(Њ){case ў.Βχ:return "Auto"+(Κ==Ι.ƛ?" (Ore)":"");case ў.Βψ:return "Auto (+Stone)";
case ў.ΒЀ:return "Default";default:return "";}}String ΒЊ(Π Ё){switch(Ё){case Π.ч:return "No job";case Π.ś:return "Job paused";case Π.Д:return "Job active";case Π.ő:return "Job active";case Π.Œ:return "Job active";case Π.ΒЃ:return "Job done";case Π.ΑΪ:return "Job changed";case Π.Αι:return "Move home";
case Π.Ακ:return "Move to job";}return "";}String Εβ(х ΘЏ){switch(ΘЏ){case х.Απ:return "Return home";case х.ΒЉ:return "Fly to job pos";case х.ΕЋ:return "Stop";case х.Ɩ:return "Ignore";}return "";}String ΘΑ(Ѝ Њ){switch(Њ){case Ѝ.ΓΤ:return "Off";case Ѝ.ΒЄ:return "Drop pos (Stone) ";case Ѝ.Αά:return "Drop pos (Sto.+Ice)";
case Ѝ.ΕΨ:return "Cur. pos (Stone)";case Ѝ.Βώ:return "Cur. pos (Sto.+Ice)";case Ѝ.ΓΓ:return "In motion (Stone)";case Ѝ.Αε:return "In motion (Sto.+Ice)";}return "";}String Εΰ(ѐ Ё){switch(Ё){case ѐ.None:return "No batteries";case ѐ.Ī:return "Charging";case ѐ.ΕΫ:return "Discharging";}return "";}
String ΒЍ(Β Ё){String Κα=Κ==Ι.ά?"target":"job";switch(Ё){case Β.о:return "Idle";case Β.đ:return "Flying to XY position";case Β.Ɯ:return Κ==Ι.ǟ?"Grinding":"Mining";case Β.Ȫ:return "Returning";case Β.Ɣ:return "Flying to drop pos";case Β.Į:return "Returning to dock";case Β.Е:return "Flying to dock area";
case Β.н:return "Flying to job area";case Β.Đ:return "Flying to path";case Β.Ŗ:return "Flying to job position";case Β.Ƕ:return "Approaching dock";case Β.ī:return "Docking";case Β.ħ:return "Aligning";case Β.Č:return "Aligning";case Β.ƞ:return "Retry docking";case Β.Ȗ:return "Unloading";case Β.ĕ:return Ηρ;
case Β.Ǣ:return "Undocking";case Β.Ī:return "Charging batteries";case Β.Αω:return "Waiting for uranium";case Β.Αή:return "Filling up hydrogen";case Β.Γς:return "Waiting for ejection";case Β.Αψ:return "Waiting for ejection";case Β.Αΰ:return "Flying to drop pos";}return "";}String Εμ(π Н){switch(Н)
{case π.ΓΦ:return "On \"Undock\" command";case π.ΓΪ:return "On player entered cockpit";case π.Εύ:return "On ship is full";case π.Εό:return "On ship is empty";case π.ΓΫ:return "On time delay";case π.Αν:return "On batteries empty(<25%)";case π.Αμ:return "On batteries empty(=0%)";case π.ΓΥ:return "On batteries full";
case π.ΓΩ:return "On hydrogen empty(<25%)";case π.ΓΧ:return "On hydrogen empty(=0%)";case π.ΓΨ:return "On hydrogen full";}return "";}bool Μ(ref String Γ,int ΗΣ,int Ğ,bool Θ,String Ŀ){ł+=1;if(ΗΣ==Ğ) Ŀ=">"+Ŀ+(Αύ>=2?" .":"");else Ŀ=" "+Ŀ;Γ+=Ŀ+"\n";return ΗΣ==Ğ&&Θ;}String ΕΟ(bool Θ)
{int Α=0;int Η=Љ;ł=0;String ƅ="———————————————\n";String Ɗ="--------------------------------------------\n";String Γ="";Γ+=ΒЊ(α)+" | "+(Ξ.ζ?"Ready to dock":"No dock")+"\n";Γ+=ƅ;double ȉ=Math.Max(Math.Round(this.Э),0);if(ϋ==Ρ.Main){bool Ġ=Κ==Ι.ά;if(Μ(ref Γ,Η,Α++,Θ," Record path & set home")) Γν();
if(Κ==Ι.ƛ)
if(Μ(ref Γ,Η,Α++,Θ," Setup mining job")) θ(Ρ.ƚ);if(Κ==Ι.ǟ)
if(Μ(ref Γ,Η,Α++,Θ," Setup grinding job")) θ(Ρ.ƚ);if(Κ==Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," Setup shuttle job")) θ(Ρ.Γι);if(Μ(ref Γ,Η,Α++,Θ," Continue job")) Ď();if(Μ(ref Γ,Η,Α++,Θ," Fly to home position")) Αα();if(Μ(ref Γ,Η,Α++,Θ," Fly to job position")) Ĭ();if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(Ġ) θ(Ρ.Ατ);else θ(Ρ.ΑΨ);if(Μ(ref Γ,Η,Α++,Θ," Info")) θ(Ρ.Εξ);if(Κ!=Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," Help")) θ(Ρ.Εν);}else if(ϋ==Ρ.ƚ){double ΚΦ=Math.Round(ъ*ȍ,1);double ΙΪ=Math.Round(щ*Ǵ,1);String А="";if(Μ(ref А,Η,Α++,Θ," Start new job!")) Γμ();if(Μ(ref А,Η,Α++,Θ," Change current job")){ƣ(false,false);θ(Ρ.Main);}if(Μ(ref А,Η,Α++,Θ," Width + (Width: "+ъ+" = "+ΚΦ+"m)")){œ(ref ъ,5,20,1);Ĳ(true);}
if(Μ(ref А,Η,Α++,Θ," Width -")){œ(ref ъ,-5,20,-1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Height + (Height: "+щ+" = "+ΙΪ+"m)")){œ(ref щ,5,20,1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Height -")){œ(ref щ,-5,20,-1);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Depth + ("+(є==ў.ΒЀ?"Depth":"Min")+": "+з+"m)")){œ(ref з,5,50,2);Ĳ(true);}
if(Μ(ref А,Η,Α++,Θ," Depth -")){œ(ref з,-5,50,-2);Ĳ(true);}if(Μ(ref А,Η,Α++,Θ," Depth mode: "+ΗА(є))){є=ď(є);}if(Μ(ref А,Η,Α++,Θ," Start pos: "+ΘΘ(Ȉ))){Ȉ=ď(Ȉ);}if(Κ==Ι.ǟ&&є==ў.Βψ) є=ď(є);Γ+=Γγ(8,А,Η,ref ΙЃ);}else if(ϋ==Ρ.Γι){float[]Ζδ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Μ(ref Γ,Η,Α++,Θ," Next")) θ(Ρ.ΕЈ);if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=" Leave connector 1:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+Εμ(Ѓ.Н))) Ѓ.Н=ď(Ѓ.Н);if(!Ѓ.ΒБ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αβ((int) Ѓ.Ǧ))) Ѓ.Ǧ=ΕΩ(Ѓ.Ǧ,Ζδ);Γ+=" Leave connector 2:\n";if(Μ(ref Γ,Η,Α++,Θ," - "+Εμ(Є.Н))) Є.Н=ď(Є.Н);
if(!Є.ΒБ()) Γ+="\n";else if(Μ(ref Γ,Η,Α++,Θ," - Delay: "+Αβ((int) Є.Ǧ))) Є.Ǧ=ΕΩ(Є.Ǧ,Ζδ);}else if(ϋ==Ρ.ΕЈ){if(Μ(ref Γ,Η,Α++,Θ," Start job!")) Γμ();if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Γι);Γ+=" Timer: \"Docking connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ѓ.ǧ!=""?Ѓ.ǧ:"-"))) Ѓ.ǧ=Αλ(ref ΚΕ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Ѓ.Ǫ!=""?Ѓ.Ǫ:"-"))) Ѓ.Ǫ=Αλ(ref ΚΗ);Γ+=" Timer: \"Docking connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Є.ǧ!=""?Є.ǧ:"-"))) Є.ǧ=Αλ(ref ΚΖ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Μ(ref Γ,Η,Α++,Θ," = "+(Є.Ǫ!=""?Є.Ǫ:"-"))) Є.Ǫ=Αλ(ref ΚΘ);
}else if(ϋ==Ρ.ƙ){String ΖЊ=Ȅ+" %";if(ȳ) ğ++;if(ğ>1){ğ=0;ΒΖ++;if(ΒΖ>1) ΒΖ=0;}bool[]ΔЀ=new bool[]{Ģ.Count==0,Ǻ==ѐ.None,ǒ.Count==0};int Ημ=0;while(true){Ημ++;ļ++;if(ļ>ΔЀ.Length-1) ļ=0;if(Ημ>=ΔЀ.Length) break;if(!ΔЀ[ļ]) break;}bool Ġ=Κ==Ι.ά;if(!Ġ&&ċ&&Я!=-1&&ΒΖ==0)
ΖЊ=Я<1000000?Math.Round(Я)+" Kg":Math.Round(Я/1000)+" t";if(Μ(ref Γ,Η,Α++,Θ," Stop!")){Ϗ();θ(Ρ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!Ġ) θ(Ρ.ΑΨ);else θ(Ρ.Ατ);if(!Ġ){if(Μ(ref Γ,Η,Α++,Θ," Next hole")) Βρ(false);}else if(Μ(ref Γ,Η,Α++,Θ," Undock")) ΑΜ=true;Γ+=Ɗ;if(!Ġ) Γ+="Progress: "+Math.Round(ǃ,1)+" %\n";Γ+="State: "+ΒЍ(Ψ)+" "+ȉ+"m \n";Γ+="Load: "+ΒΝ+" % Max: "+ΖЊ+" \n";if(ļ==0) Γ+="Uranium: "+(Ģ.Count==0?"No reactors":Math.Round(Ŏ,1)+"Kg "+(к==-1?"":" Min: "+к+" Kg"))+"\n";
if(ļ==1) Γ+="Battery: "+(Ǻ==ѐ.None?Εΰ(Ǻ):Ș+"% "+(ј==-1||Ġ?"":" Min: "+ј+" %"))+"\n";if(ļ==2) Γ+="Hydrogen: "+(ǒ.Count==0?"No tanks":Math.Round(Ȳ,1)+"% "+(љ==-1||Ġ?"":" Min: "+љ+" %"))+"\n";}else if(ϋ==Ρ.ΑΨ){String А="";if(Μ(ref А,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƙ);else θ(Ρ.Main);}
if(Μ(ref А,Η,Α++,Θ," Max load: "+Ȅ+"%")) ф(ref Ȅ,Ȅ<=80?-10:-5,ϊ.Αη,false);if(Μ(ref А,Η,Α++,Θ," Weight limit: "+(ċ?"On":"Off"))) ċ=!ċ;if(Μ(ref А,Η,Α++,Θ," Ejection: "+ΘΑ(Ю))) Ю=ď(Ю);if(Μ(ref А,Η,Α++,Θ," Toggle sorters: "+(Ō?"On":"Off"))){Ō=!Ō;if(Ō) Ǹ(ΔЃ);}
if(Μ(ref А,Η,Α++,Θ," Unload ice: "+(Ċ?"On":"Off"))) Ċ=!Ċ;if(Μ(ref А,Η,Α++,Θ," Uranium: "+(к==-1?"Ignore":"Min "+к+"Kg"))) ф(ref к,(к>5?-5:-1),ϊ.Αθ,true);if(Μ(ref А,Η,Α++,Θ," Battery: "+(ј==-1?"Ignore":"Min "+ј+"%"))) ф(ref ј,-5,ϊ.ΓΟ,true);if(Μ(ref А,Η,Α++,Θ," Hydrogen: "+(љ==-1?"Ignore":"Min "+љ+"%"))) ф(ref љ,-10,ϊ.ΓΠ,true);
if(Μ(ref А,Η,Α++,Θ," When done: "+(ĥ?"Return home":"Stop"))) ĥ=!ĥ;if(Μ(ref А,Η,Α++,Θ," On damage: "+Εβ(ь))) ь=ď(ь);if(Μ(ref А,Η,Α++,Θ," Advanced...")) θ(Ρ.Βς);Γ+=Γγ(8,А,Η,ref ΙЄ);}else if(ϋ==Ρ.Βς){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƙ);else θ(Ρ.Main);}
if(Μ(ref Γ,Η,Α++,Θ,(Κ==Ι.ǟ?" Grinder":" Drill")+" inv. balancing: "+(ů?"On":"Off"))) ů=!ů;if(Μ(ref Γ,Η,Α++,Θ," Enable"+(Κ==Ι.ǟ?" grinders":" drills")+": "+(ũ?"Fwd + Bwd":"Fwd"))) ũ=!ũ;if(Μ(ref Γ,Η,Α++,Θ," Work speed fwd.: "+Ɛ+"m/s")) ф(ref Ɛ,0.5f,ϊ.Ƥ,false);
if(Μ(ref Γ,Η,Α++,Θ," Work speed bwd.: "+Ə+"m/s")) ф(ref Ə,0.5f,ϊ.Ƥ,false);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǹ*100f)+"%"+(ǹ>0.80f?" (risky)":""))) ф(ref ǹ,0.1f,ϊ.ΑΥ,false);if(Μ(ref Γ,Η,Α++,Θ," Width overlap: "+Ő*100f+"%")) ΕΥ(true,0.05f);if(Μ(ref Γ,Η,Α++,Θ," Height overlap: "+ĝ*100f+"%")) ΕΥ(false,0.05f);
}else if(ϋ==Ρ.Ατ){if(Μ(ref Γ,Η,Α++,Θ," Back")){if(α==Π.Д) θ(Ρ.ƙ);else θ(Ρ.Main);}if(Μ(ref Γ,Η,Α++,Θ," Max load: "+Ȅ+"%")) ф(ref Ȅ,Ȅ<=80?-10:-5,ϊ.Αη,false);if(Μ(ref Γ,Η,Α++,Θ," Unload ice: "+(Ċ?"On":"Off"))) Ċ=!Ċ;if(Μ(ref Γ,Η,Α++,Θ," Uranium: "+(к==-1?"Ignore":"Min "+к+"Kg"))) ф(ref к,(к>5?-5:-1),ϊ.Αθ,true);
if(Μ(ref Γ,Η,Α++,Θ," Battery: "+(ј==-1?"Ignore":"Charge up"))) ј=(ј==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," Hydrogen: "+(љ==-1?"Ignore":"Fill up"))) љ=(љ==-1?1:-1);if(Μ(ref Γ,Η,Α++,Θ," On damage: "+Εβ(ь))) ь=ď(ь);if(Μ(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǹ*100f)+"%"+(ǹ>0.80f?" (risky)":""))) ф(ref ǹ,0.1f,ϊ.ΑΥ,false);
}else if(ϋ==Ρ.ΕЂ){double Ιζ=0;if(Φ.Count>0) Ιζ=Vector3.Distance(Φ.Last().Ε,β);if(Μ(ref Γ,Η,Α++,Θ," Stop path recording")) ΕЍ();if(Κ!=Ι.ά){if(Μ(ref Γ,Η,Α++,Θ," Home: "+(Ȥ?"Use old home":(Ξ.ζ?"Was set! ":"none ")))) Ȥ=!Ȥ;}else{if(Μ(ref Γ,Η,Α++,Θ," Connector 1: "+(Ȥ?"Use old connector":(Ξ.ζ?"Was set! ":"none ")))) Ȥ=!Ȥ;
if(Μ(ref Γ,Η,Α++,Θ," Connector 2: "+(Ƌ?"Use old connector":(Ζ.ζ?"Was set! ":"none ")))) Ƌ=!Ƌ;}if(Μ(ref Γ,Η,Α++,Θ," Path: "+(ƌ?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) ƌ=!ƌ;Γ+=Ɗ;Γ+="Wp spacing: "+Math.Round(Ησ)+"m\n";}else if(ϋ==Ρ.Γξ){if(Μ(ref Γ,Η,Α++,Θ," Stop")){Ϗ();θ(Ρ.Main);}
Γ+=Ɗ;Γ+="State: "+ΒЍ(Ψ)+" \n";Γ+="Speed: "+Math.Round(Ć,1)+"m/s\n";Γ+="Target dist: "+ȉ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+ΕΙ+"\n";}else if(ϋ==Ρ.Εξ){List<IMyTerminalBlock>Ķ=Εγ();if(ȳ) ğ++;if(ğ>=Ķ.Count) ğ=0;if(Μ(ref Γ,Η,Α++,Θ," Next")) θ(Ρ.Εο);
Γ+=Ɗ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΒΝ,1)+"% "+Math.Round(ǿ,1)+" / "+Math.Round(ȅ,1)+"\n";Γ+="Uranium: "+(Ģ.Count==0?"No reactors":Math.Round(Ŏ,1)+"Kg "+Δζ)+"\n";Γ+="Battery: "+(Ǻ==ѐ.None?"":Ș+"% ")+Εΰ(Ǻ)+"\n";Γ+="Hydrogen: "+(ǒ.Count==0?"No tanks":Math.Round(Ȳ,1)+"% ")+"\n";
Γ+="Damage: "+(Ķ.Count==0?"None":""+(ğ+1)+"/"+Ķ.Count+" "+Ķ[ğ].CustomName)+"\n";}else if(ϋ==Ρ.Εο){if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=Ɗ;Γ+="Next scan: "+ΑΒ+"s\n";Γ+="Ship size: "+Math.Round(ȍ,1)+"m "+Math.Round(Ǵ,1)+"m "+Math.Round(λ,1)+"m \n";
Γ+="Broadcast: "+(Ŧ?"Online - "+Ɔ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŷ*100f,1)+"% \n";}else if(ϋ==Ρ.Εν){if(Μ(ref Γ,Η,Α++,Θ," Back")) θ(Ρ.Main);Γ+=Ɗ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Ћ==2) Γ="Fatal setup error\nNext scan: "+ΑΒ+"s\n";if(Βί) Γ="Recompile script now";int Ιη=Γ.Split('\n').Length;for(int i=Ιη;i<=10;i++) Γ+="\n";Γ+=ƅ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕΥ(bool Ιβ,float Ǚ){Ϗ();ƣ(true,false);if(Ιβ) ф(ref Ő,Ǚ,ϊ.Γά,false);else ф(ref ĝ,Ǚ,ϊ.Γά,false);ΕΣ();ı(true,true,0,0);}float ΕΩ(float ν,float[]Βξ){float έ=Βξ[0];for(int i=Βξ.Length-1;i>=0;i--)
if(ν<Βξ[i]) έ=Βξ[i];return έ;}String Αλ(ref int Ğ){String έ="";if(Ğ>=ΔЎ.Count) Ğ=-1;if(Ğ>=0) έ=ΔЎ[Ğ].CustomName;Ğ++;return έ;}void Αφ(string ή){if(α!=Π.Д) return;if(ή=="") return;IMyTerminalBlock Χ=χ.GetBlockWithName(ή);if(Χ==null||!(Χ is IMyTimerBlock)){Λ="Timerblock "+ή+" not found!";
return;}((IMyTimerBlock) Χ).Trigger();}void œ(ref int value,int Ƭ,int ĉ,int ΔЁ){if(Ƭ==0) return;if(value<ĉ&&ΔЁ>0||value<=ĉ&&ΔЁ<0){value+=ΔЁ;return;}int ΘІ=Math.Abs(Ƭ);int Γσ=0;int Δο=1;while(true){Γσ+=Δο*ΘІ*10;if(Ƭ<0&&value-ĉ<=Γσ) break;if(Ƭ>0&&value-ĉ<Γσ) break;
Δο++;}value+=Δο*Ƭ;}void Ĳ(bool Κδ){ъ=Math.Max(ъ,1);щ=Math.Max(щ,1);з=Math.Max(з,0);ı(ϋ==Ρ.ƚ,false,0,0);}bool ĭ(ref float value,bool ΙЀ,ϊ υ,String Ľ,String Ιί){if(Ľ=="") return false;float Ż=-1;bool Ζό=false;if(Ľ.ToUpper()==Ιί) Ζό=true;else if(!float.TryParse(Ľ,out Ż)) return false;
else Ż=Math.Max(0,Ż);if(ΙЀ) Ż=(float) Math.Round(Ż);ф(ref value,Ż,υ,Ζό,false);return true;}void ф(ref float value,float Ǚ,ϊ υ,bool ΑΛ){ф(ref value,value+Ǚ,υ,ΑΛ,true);}void ф(ref float value,float Ÿ,ϊ υ,bool ΑΛ,bool ΚΫ){float Ǭ=0,ǫ=0;if(υ==ϊ.Ƥ){Ǭ=0.5f;ǫ=10f;}if(υ==ϊ.ΑΥ){Ǭ=0.1f;ǫ=1f;}
if(υ==ϊ.Ɩ){Ǭ=50f;ǫ=100f;}if(υ==ϊ.ΓΟ){Ǭ=5f;ǫ=30f;}if(υ==ϊ.Αθ){Ǭ=1f;ǫ=25f;}if(υ==ϊ.Αη){Ǭ=10f;ǫ=95f;}if(υ==ϊ.ΓΠ){Ǭ=10f;ǫ=90f;}if(υ==ϊ.Θσ){Ǭ=10f;ǫ=1800;}if(υ==ϊ.Γά){Ǭ=0.0f;ǫ=0.75f;}if(Ÿ==-1&&ΑΛ){value=-1;return;}if(value==-1) ΑΛ=false;
bool ΗΜ=Ÿ<Ǭ||Ÿ>ǫ;if(ΗΜ&&ΚΫ){if(Ÿ<value) value=ǫ;else if(Ÿ>value) value=Ǭ;}else value=Ÿ;if(ΗΜ&&ΑΛ) value=-1;else value=Math.Max(Ǭ,Math.Min(value,ǫ));value=(float) Math.Round(value,2);}void Βρ(bool Ιϋ){if(Ιϋ) Ь=Math.Max(0,Ь-1);else Ь++;ΒІ();ΒЅ();for(int k=0;k<ȃ.Count&&k<й.Length;k++) Γη(ȃ[k],k<Ь);
а=-1;Γρ(true);}T ď<T>(T ν){int Α=Array.IndexOf(Enum.GetValues(ν.GetType()),ν);Α++;if(Α>=ΗЋ(ν)) Α=0;return(T) Enum.GetValues(ν.GetType()).GetValue(Α);}int ΗЋ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Γν(){ΑΣ.Clear();for(int i=0;i<Φ.Count;i++) ΑΣ.Add(Φ[i]);Φ.Clear();
ΒΚ=true;ΔΦ=new Ϊ(Ξ);ΔΩ=new Ϊ(Ζ);Ξ.ζ=false;if(Κ==Ι.ά) Ζ.ζ=false;for(int i=0;i<ǚ.Count;i++)
if(!Ȣ.Contains(ǚ.Keys.ElementAt(i)))
Ȣ.Add(ǚ.Keys.ElementAt(i));Ȥ=false;Ƌ=false;ƌ=false;θ(Ρ.ΕЂ);}void ΕЍ(){if(Ȥ) Ξ=ΔΦ;if(Ƌ) Ζ=ΔΩ;if(ƌ){Φ.Clear();for(int i=0;i<ΑΣ.Count;i++) Φ.Add(ΑΣ[i]);}ΒΚ=false;Ϗ();θ(Ρ.Main);}void ΘϏ(){if(!ΒΚ) return;if(Ψ!=Β.о){ΕЍ();return;}if(!ΔΦ.ζ) Ȥ=false;if(!ΔΩ.ζ) Ƌ=false;if(ΑΣ.Count<=1) ƌ=false;
IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connectable);if(Л==null) Л=Ъ(MyShipConnectorStatus.Connected);if(Л!=null){if(Math.Round(Ć,2)<=0.20) ΔΑ++;else ΔΑ=0;if(ΔΑ>=5){if(Κ==Ι.ά&&(Ξ.ζ||Ȥ)&&Vector3.Distance(Ξ.Ε,Л.GetPosition())>5){Ζ.δ=Ω.WorldMatrix.Forward;Ζ.Ѐ=Ω.WorldMatrix.Left;
Ζ.γ=Ω.WorldMatrix.Down;Ζ.φ=Ω.GetNaturalGravity();Ζ.Ε=Л.GetPosition();Ζ.ζ=true;Ζ.и=Л.Position;}else{Ξ.δ=Ω.WorldMatrix.Forward;Ξ.Ѐ=Ω.WorldMatrix.Left;Ξ.γ=Ω.WorldMatrix.Down;Ξ.φ=Ω.GetNaturalGravity();Ξ.Ε=Л.GetPosition();Ξ.ζ=true;Ξ.и=Л.Position;}}}double ΔΫ=-1;if(Φ.Count>0) ΔΫ=Vector3.Distance(β,Φ.Last().Ε);
double ΙЋ=Math.Max(1.5,Math.Pow(Ć/100.0,2));double ΗΓ=Math.Max(Ć*ΙЋ,2);Ησ=ΗΓ;if((ΔΫ==-1)||ΔΫ>=ΗΓ){Ϊ Ϋ=new Ϊ(β,ΒΓ,ƴ,ǅ,Ω.GetNaturalGravity());Ϋ.Ηψ(ȣ,Ȣ);Φ.Add(Ϋ);}}Ϊ ц(){if(Κ!=Ι.ά) return Ǖ!=null?Ǖ:Ξ;return Ζή;}void ŝ(Ϊ ί,Π Ё){Ζή=ί;if(α==Π.Д) ї=Ё;}bool Ȓ(Ϊ ί,float І,bool ΚΡ,out Vector3 ο)
{if(ΚΡ){Vector3I ΔΜ=new Vector3I((int) ί.и.X,(int) ί.и.Y,(int) ί.и.Z);IMySlimBlock Χ=Me.CubeGrid.GetCubeBlock(ΔΜ);if(Χ==null||!(Χ.FatBlock is IMyShipConnector)){ο=new Vector3();return false;}Vector3 ΙΔ=п(Ω,Χ.FatBlock.GetPosition()-β);Vector3 ΙΓ=п(Ω,Χ.FatBlock.WorldMatrix.Forward);ο=ί.Ε-Γπ(ί.δ,ί.γ*-1,ΙΔ)-Γπ(ί.δ,ί.γ*-1,ΙΓ)*І;
return true;}else{ο=ί.Ε;return true;}}Vector3 Ȩ(int ΙΑ,int ƀ,bool Ιώ){if(!Ιώ&&Ζφ) return ΓЄ;float ΙΒ=((ξ-1f)/2f)-ΙΑ;float ΙЁ=((И-1f)/2f)-ƀ;ΓЄ=Ζ.Ε+Ζ.Ѐ*ΙΒ*ȍ+ă*-1*ΙЁ*Ǵ;Ζφ=true;return ΓЄ;}Vector3 ΒЌ(Vector3 ΙЏ,float ΓЍ){return ΙЏ+(ǭ*ΓЍ);}ΰ ΒЁ(){float Ц=-1;ΰ έ=ΰ.Ȑ;
if(Κ!=Ι.ά){Εψ();if(α!=Π.ч){Vector3 Й=İ(ǭ,ă*-1,β-Ζ.Ε);if(Math.Abs(Й.X)<=(float)(ξ*ȍ)/2f&&Math.Abs(Й.Y)<=(float)(И*Ǵ)/2f){if(Й.Z<=-1&&Й.Z>=-ŉ*2) return ΰ.ΑΧ;if(Й.Z>-1&&Й.Z<λ*2) return ΰ.Ȑ;}if(č(Ζ.Ε,ref Ц)) έ=ΰ.Ȑ;}if(Ξ.ζ){if(č(Ξ.Ε,ref Ц)) έ=ΰ.ȏ;for(int i=0;i<Φ.Count;i++)
if(č(Φ[i].Ε,ref Ц)) έ=ΰ.ȩ;if(Vector3.Distance(β,Ξ.Ε)<dockDist*λ) έ=ΰ.ȏ;if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ȏ;}}else{Vector3 ο=new Vector3();IMyShipConnector ț=Ъ(MyShipConnectorStatus.Connected);if(Ξ.ζ){if(č(Ξ.Ε,ref Ц)) έ=ΰ.ȏ;if(Ȓ(Ξ,dockDist,true,out ο))
if(č(ο,ref Ц)) έ=ΰ.ȏ;if(ț!=null&&Vector3.Distance(ț.GetPosition(),Ξ.Ε)<5)
return ΰ.Βυ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ξ.Ε)>dockDist*λ&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*λ)
if(č(Φ[i].Ε,ref Ц)) έ=ΰ.ȩ;if(Ζ.ζ){if(č(Ζ.Ε,ref Ц)) έ=ΰ.Ȑ;if(Ȓ(Ζ,dockDist,true,out ο))
if(č(ο,ref Ц)) έ=ΰ.Ȑ;if(ț!=null&&Vector3.Distance(ț.GetPosition(),Ζ.Ε)<5)
return ΰ.Βφ;}}return έ;}bool č(Vector3 ο,ref float Ц){float І=Vector3.Distance(ο,β);if(І<Ц||Ц==-1){Ц=І;return true;}return false;}void Γμ(){if(Ћ>0){Λ="Setup error! Can't start";return;}if(Κ==Ι.ά){Ď();return;}Ζ.Ε=β;Ζ.φ=Ω.GetNaturalGravity();Ζ.δ=ΒΓ;Ζ.γ=ƴ;Ζ.Ѐ=ǅ;
ǭ=ǖ.WorldMatrix.Forward;ă=Ζ.γ;if(ǭ==Ω.WorldMatrix.Down) ă=Ω.WorldMatrix.Backward;ƣ(true,true);Σ(Β.đ);Γλ();}void ƣ(bool Ǩ,bool ΙϏ){if(α==Π.ч&&!Ǩ) return;bool Ιο=Ǩ||α==Π.ΒЃ||ξ!=ъ||И!=щ||ľ!=Ȉ;if(Ιο){if(α!=Π.ч){α=Π.ΑΪ;Ȩ(ώ,ό,ΙϏ);Λ="Job changed, lost progress";}ľ=Ȉ;
ξ=ъ;И=щ;ό=0;ώ=0;й="";а=-1;Ł=0;ā=0;г=0;Ь=0;Γρ(true);}}void Γή(){в(β,0);ǎ(ȣ,true);}void Γλ(){Ȕ(р.Αρ);Γή();Ɵ(Ƞ,false);Ǹ(ΔЃ);α=Π.Д;ΕΡ(true);ї=α;θ(Ρ.ƙ);Θγ();ΑЈ=true;ΒΗ=0;for(int i=ƥ.Count-1;i>=0;i--)
if(Ɨ(ƥ[i],false)) ΒΗ++;if(ΒΗ>0) Λ="Started with damage";}void Ϗ(){if(α==Π.Д){α=Π.ś;Λ="Job paused";}Σ(Β.о);ї=α;Ασ(false,0,0,0,0);Γο();ΕΝ(new Vector3(),false);ΕЌ();Γζ(ChargeMode.Auto);Γθ(false);ΕЄ(true);ȫ(Β.о);ı(false,false,0,0);ǎ(ω,false);ǎ(ѓ,true);Ǹ(true);Βΰ=false;ΑΫ();
ΑЅ=false;Ʋ=false;Γα();ΑЈ=false;ΑΔ=false;ΑΜ=false;if(ϋ!=Ρ.Main&&ϋ!=Ρ.ΑΨ&&ϋ!=Ρ.Βς&&ϋ!=Ρ.Ατ)
θ(Ρ.Main);}void Ď(){ΰ Ш=ΒЁ();if(Κ==Ι.ά){if(!Ζ.ζ||!Ξ.ζ) return;Γλ();bool ΔΚ=Vector3.Distance(β,Ξ.Ε)<Vector3.Distance(β,Ζ.Ε);if(ƃ==Π.ő) ΔΚ=true;if(ƃ==Π.Œ) ΔΚ=false;if(ΔΚ){ŝ(Ξ,Π.ő);switch(Ш){case ΰ.Βυ:Σ(Β.ĕ);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.ȏ:Σ(Β.Ƕ);break;default:Σ(Β.Ǣ);break;
}}else{ŝ(Ζ,Π.Œ);switch(Ш){case ΰ.Βφ:Σ(Β.ĕ);break;case ΰ.Ȑ:Σ(Β.Ƕ);break;case ΰ.ȩ:Σ(Β.н);break;default:Σ(Β.Ǣ);break;}}}else{if(α!=Π.ś&&α!=Π.ΑΪ) return;bool ΚΤ=α==Π.ΑΪ;Γλ();bool ΔΡ=Ƙ(false)&&Ξ.ζ;switch(Ш){case ΰ.Ȑ:Σ(ΔΡ?Β.Đ:Β.đ);break;case ΰ.ȩ:Σ(ΔΡ?Β.Е:Β.н);break;case ΰ.ȏ:Σ(ΔΡ?Β.Ƕ:Β.Ȗ);break;
case ΰ.ΑΧ:{if(ā!=Ь||ΚΤ) Σ(Β.Ȫ);else Σ(Β.Ɯ);}break;default:break;}}}void Ĭ(){if(α==Π.ч&&!Ξ.ζ) return;if(Κ==Ι.ά&&(!Ζ.ζ||!Ξ.ζ)) return;Λ="Move to job";ΰ Ш=ΒЁ();if(Κ==Ι.ά){ŝ(Ζ,Π.Œ);switch(Ш){case ΰ.Ȑ:Σ(Β.Ƕ);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.Βφ:return;default:Σ(Β.Ǣ);break;
}ȫ(Β.ĕ);}else{switch(Ш){case ΰ.Ȑ:Σ(Β.н);break;case ΰ.ȩ:Σ(Β.н);break;case ΰ.ȏ:Σ(Β.Ȗ);break;case ΰ.ΑΧ:Σ(Β.Ȫ);break;default:break;}if(α==Π.ч) ȫ(Β.н);else ȫ(Β.Č);Βΰ=true;}Γή();θ(Ρ.Γξ);Ɵ(Ƞ,false);ї=Π.Ακ;}void Αα(){if(!Ξ.ζ) return;Λ="Move home";ΰ Ш=ΒЁ();if(Κ==Ι.ά){ŝ(Ξ,Π.ő);switch(Ш)
{case ΰ.ȩ:Σ(Β.Е);break;case ΰ.ȏ:Σ(Β.Ƕ);break;case ΰ.Βυ:return;default:Σ(Β.Ǣ);break;}ȫ(Β.ĕ);}else{if(Ъ(MyShipConnectorStatus.Connected)!=null) return;if(Ъ(MyShipConnectorStatus.Connectable)!=null){Σ(Β.ī);ȫ(Β.Ȗ);return;}switch(Ш){case ΰ.Ȑ:Σ(Β.Đ);break;case ΰ.ȩ:Σ(Β.Е);break;case ΰ.ȏ:Σ(Β.Е);break;
case ΰ.ΑΧ:Σ(Β.Į);break;default:break;}ȫ(Β.Ȗ);}Γή();θ(Ρ.Γξ);Ɵ(Ƞ,false);ї=Π.Αι;}void Σ(Β Δπ){if(Δπ==Β.о) ΑΖ=Β.о;if(ΑΖ!=Β.о&&Ψ==ΑΖ&&Δπ!=ΑΖ){Ϗ();return;}Ў=true;Ψ=Δπ;}void ȫ(Β Ё){this.ΑΖ=Ё;}ǡ Κΰ(int ǀ,bool ǰ){if(ǰ){ǘ=null;ώ=0;ό=0;}if(Ȉ==Ǐ.Ƣ){int Ń=ǀ+1;ό=(int) Math.Floor(Б(ǀ,ξ));
if(ό%2==0) ώ=ǀ-(ό*ξ);else ώ=ξ-1-(ǀ-(ό*ξ));if(ό>=И) return ǡ.ΑΦ;else return ǡ.Αγ;}else if(Ȉ==Ǐ.ΕΤ){if(ǘ==null) ǘ=new int[]{0,-1,0,0};int Ƽ=(int) Math.Ceiling(ξ/2f);int ƻ=(int) Math.Ceiling(И/2f);int ΔΟ=(int) Math.Floor(ξ/2f);int ΔΞ=(int) Math.Floor(И/2f);
int ǂ=0;while(ǘ[2]<Math.Pow(Math.Max(ξ,И),2)){if(ǂ>200) return ǡ.ΗЌ;ǂ++;ǘ[2]++;if(-Ƽ<ώ&&ώ<=ΔΟ&&-ƻ<ό&&ό<=ΔΞ){if(ǘ[3]==ǀ){this.ώ=ώ-1+Ƽ;this.ό=ό-1+ƻ;return ǡ.Αγ;}ǘ[3]++;}if(ώ==ό||(ώ<0&&ώ==-ό)||(ώ>0&&ώ==1-ό)){int ΚΑ=ǘ[0];ǘ[0]=-ǘ[1];ǘ[1]=ΚΑ;
}ώ+=ǘ[0];ό+=ǘ[1];}}return ǡ.ΑΦ;}void Θύ(){if(Ψ==Β.đ){if(Ў){Џ=0;if(ā!=Ь) Ł=0;ā=Ь;}if(Џ==0){int ΓІ=Ηώ(Ь);if(ΓІ<0){if(!ΑЅ){ΑЅ=true;Ζέ=β;}в(Ζέ,0);Λ="Waiting for a free hole";return;}ΑЅ=false;if(ΓІ!=Ь){Ь=ΓІ;Ł=0;ā=Ь;}ǡ έ=Θμ(Ь);if(έ==ǡ.ΑΦ){α=Π.ΒЃ;Λ="Job done";
if(ĥ&&Ξ.ζ){Σ(Β.Đ);ȫ(Β.Ȗ);ї=Π.Αι;}else{Σ(Β.Ŗ);ȫ(Β.Č);ї=Π.Ακ;}return;}if(έ==ǡ.Αγ){Џ=1;ǎ(ω,true);њ=Ȩ(ώ,ό,true);в(њ,10);е(Ζ.γ,Ζ.δ,Ζ.Ѐ,false);}}else{if(а>=0&&ΓΒ(а,true)){Џ=0;return;}if(Э<wpReachedDist){Σ(Β.Ɯ);return;}}}if(Ψ==Β.Ɯ){if(Ў){ǎ(ω,true);Ǹ(false);
њ=Ȩ(ώ,ό,false);в(ΒЌ(њ,0),0);е(Ζ.γ,Ζ.δ,Ζ.Ѐ,false);Џ=1;ы=0;м=0;Αώ=0;Δέ=-1;ŉ=з;ΔΧ=true;}if(!Βό()){Σ(Β.Į);return;}if(Ƙ(true)){ΗΚ=Ȧ("","ORE",Х.Ĩ);if((Ю==Ѝ.ΒЄ||Ю==Ѝ.Αά||
Ю==Ѝ.ΓΓ||Ю==Ѝ.Αε)&&Κ!=Ι.ǟ)
Σ(Β.Ɣ);else if((Ю==Ѝ.ΕΨ||Ю==Ѝ.Βώ)&&Κ!=Ι.ǟ)
Σ(Β.Γς);else
Σ(Β.Į);return;}г=Vector3.Distance(β,њ);if(г>Ł){Ł=г;ΔΧ=false;}if(Κ==Ι.ǟ&&Εδ()==MyDetectedEntityType.SmallGrid)
м+=2;else м-=2;м=Math.Max(100,Math.Min(400,м));if(Џ>0&&Џ<м){if(г>ы){if(м>150) ы=г;else ы=(float) Math.Ceiling(г);Џ=1;}else Џ++;}else{if(Џ>0){Λ="Ship stuck! Retrying";ы=г;Џ=0;ı(false,true,0,λ*sensorRange);}в(ΒЌ(њ,Math.Max(0,ы-λ)),ΒЏ(false));if(Э<=wpReachedDist/2){Џ=1;ы=0;}
return;}ı(false,true,λ*sensorRange,0);Vector3 ΘЈ=њ+ǭ*г;bool ΖЀ=false;if(Vector3.Distance(ΘЈ,β)>0.3f){Vector3 ΙΘ=њ+ǭ*(г+0.1f);в(ΙΘ,4);ΖЀ=true;}else{float Ǳ=ΒЏ(true);Vector3 Ζγ=ΒЌ(њ,Math.Max(з+1,г+1));Ƿ(true,false,false,Ζγ,Ζγ-њ,Ǳ,Ǳ);}bool ΓЎ=false;if(є==ў.Βψ||є==ў.Βχ)
{if(!ΖЀ){float Δς=0;foreach(IMyTerminalBlock Ȍ in ω)
Δς+=ΗІ(Ȍ,"","",є==ў.Βχ?new string[]{"STONE"}:null);if(Δς>Δέ||г<з||ΔΧ){Γω=0;Αώ=г;ŉ=(int)(Math.Max(ŉ,Αώ)+λ/2);}else{ΓЎ=г-Αώ>2&&Γω>=20;Γω++;}Δέ=Δς;}}else ΓЎ=г>=ŉ;if(ā!=Ь){Σ(Β.Ȫ);г=0;return;}if(ΓЎ){Ь++;Σ(Β.Ȫ);г=0;return;}}if(Ψ==Β.Ȫ||Ψ==Β.Į||Ψ==Β.Ɣ)
{if(Ў){њ=Ȩ(ώ,ό,false);е(Ζ.γ,Ζ.δ,Ζ.Ѐ,false);ǎ(ω,ũ);Ǹ(false);ы=Vector3.Distance(β,њ);ı(false,true,0,λ*sensorRange);}в(њ,ΒЏ(false));if(Vector3.Distance(β,њ)>=ы+5){ǎ(ω,false);Ǹ(true);Λ="Can´t return!";}if(Э<wpReachedDist){if(Ψ==Β.Ȫ&&Βΰ) Σ(Β.Ŗ);if(Ψ==Β.Ȫ) Σ(Β.đ);
if(Ψ==Β.Ɣ) Σ(Β.Αΰ);if(Ψ==Β.Į){if(Ξ.ζ) Σ(Β.Đ);else{Ϗ();Ĭ();Λ="Can´t return, no dock found";}}return;}}if(Ψ==Β.Е||Ψ==Β.н){if(Ψ==Β.н&&α==Π.Д&&Κ!=Ι.ά){if(!Βό()||Ƙ(true)){Σ(Β.Е);return;}}bool Δϊ=false;bool Βμ=false;bool ΖΙ=false;float ţ=0;bool ΗΙ=false;Ϊ Ϋ=null;
if(Ў){if(Ψ==Β.Е||Κ==Ι.ά){if(Κ!=Ι.ά){if(Ē(1)) Βτ();else{Ǖ=null;ȡ=0;Ʋ=!Βύ(1);}}Ϊ ί=ц();κ=new Γέ();κ.μ=ί;κ.ư=followPathDock*λ;κ.ΕΕ=useDockDirectionDist*λ;κ.ţ=10;κ.Ɖ.Add(ί.Ε);Vector3 Ţ=new Vector3();if(Ȓ(ί,dockDist*λ,true,out Ţ))
κ.Ɖ.Add(Ţ);else κ.ư*=1.5f;if(Κ==Ι.ά){if(ί==Ξ) κ.ΑБ=Ζ.Ε;if(ί==Ζ) κ.ΑБ=Ξ.Ε;κ.ΔΗ=dockDist*λ*1.1f;}}else if(Ψ==Β.н){κ=new Γέ();κ.μ=Ζ;κ.ư=followPathJob*λ;κ.ΕΕ=useJobDirectionDist*λ;κ.ţ=10;κ.ΑБ=Ξ.Ε;κ.ΔΗ=dockDist*λ*1.1f;κ.Ɖ.Add(Ζ.Ε);if(α==Π.ч){if(!Ξ.ζ||Φ.Count==0){Ϗ();return;}
float ΙΞ=Vector3.Distance(Φ.First().Ε,Ξ.Ε);float ΙΟ=Vector3.Distance(Φ.Last().Ε,Ξ.Ε);if(ΙΞ<ΙΟ) κ.μ=Φ.Last();else κ.μ=Φ.First();}}ǌ=new Vector3();ΗΙ=!ΘΡ(β);ǎ(ω,false);Ǹ(true);д=-1;double Ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,κ.ΑБ)<=κ.ΔΗ) continue;
double І=Vector3.Distance(Φ[i].Ε,β);if(Ǿ==-1||І<Ǿ){д=i;Ǿ=І;}}Ȇ=ΘΕ(κ.μ.Ε,д);ĵ=null;}if(Ψ==Β.Е&&Κ!=Ι.ά&&!Ў&&!ơ()){Ϊ Γώ=ц();Ʋ=!Βύ(1);if(ц()!=Γώ){Σ(Β.Е);return;}}if(Ψ==Β.Е&&Κ!=Ι.ά&&!Ў&&Ē(1)&&Βτ()){Σ(Β.Е);return;}bool Ζώ=false;if(Ψ==Β.Е&&!Ў&&д>=0&&д<Φ.Count)
{Vector3 Ư=Φ[д].Ε+Εη(д,Ȇ);int Δρ=д+Ȇ;Vector3 Ń=Ȇ!=0&&Δρ>=0&&Δρ<Φ.Count?Φ[Δρ].Ε:κ.μ.Ε;if(ΘΜ(Ư,Ń)){if(!ΑЋ){ΑЋ=true;Ζη=Ś(Ư)?β:Ư;}в(Ζη,0);е(κ.μ.γ,κ.μ.δ,κ.μ.Ѐ,false);Λ="Waiting for dock clearance";return;}ΑЋ=false;Ζώ=Ē(1)&&(Ś(β)||Ś(Ń));}Ηω(Φ,Ȇ,κ.Ɖ,κ.ư,Ў,ref Џ);
for(int i=0;i<κ.Ɖ.Count;i++){float І=Vector3.Distance(β,κ.Ɖ[i]);if(І<=κ.ư) Δϊ=true;if(І<=κ.ΕΕ) Βμ=true;}if(Ζώ) Βμ=true;if(Βμ) ţ=κ.ţ;float Κε=ĵ!=null?ĵ.Ф:Ć;float ΚΪ=(float) Math.Max(Ć*0.1f*λ,wpReachedDist);if((Э<ΚΪ)||Ў){if(!Ў) д+=Ȇ;if(Ȇ==0||д>Φ.Count-1||д<0)
Δϊ=true;else{ΕΙ=Ȇ>0?Φ.Count-1-д:д;Ϋ=Φ[д];ĵ=Ϋ;if(д>=1&&д<Φ.Count-1)
ǌ=Ϋ.Ε-Φ[д-Ȇ].Ε;else ĵ=null;Ђ=Ϋ.Ε+Εη(д,Ȇ);ΖΙ=true;}}if(Βμ)
е(κ.μ.γ,κ.μ.δ,κ.μ.Ѐ,false);else if(ΗΙ)
Βσ(κ.μ.γ,10,true);else if(ΖΙ&&Ϋ!=null){if(Ȇ>0) е(Ϋ.γ,Ϋ.δ,Ϋ.Ѐ,90,false);else е(Ϋ.γ,-Ϋ.δ,-Ϋ.Ѐ,90,false);}Ƿ(true,false,true,Ђ,ǌ,ĵ==null?0:ĵ.Ф,ţ);if(Δϊ){ΕΙ=0;if(Ψ==Β.Е&&!Ηό()){в(Ђ,0);return;}if(Ψ==Β.Е&&Ʋ){в(Ђ,0);Λ="Waiting for a free dock";return;}if(Ψ==Β.Е||Κ==Ι.ά){Σ(Β.Ƕ);return;}
if(Ψ==Β.н&&Βΰ){Σ(Β.Ŗ);return;}if(Ψ==Β.н){Σ(Β.đ);return;}}}if(Ψ==Β.Ƕ||Ψ==Β.ƞ){if(Κ!=Ι.ά&&ơ()){if(!Ē(1)){Σ(Β.Е);return;}}else if(Κ!=Ι.ά){Ϊ Γώ=ц();if(!Βύ(2)||ц()!=Γώ){Σ(Β.Е);return;}}Ϊ ί=ц();if(Ў){if(!Ȓ(ί,dockDist*λ,true,out Ђ)){Ȕ(р.ƒ);Ϗ();return;}
в(Ђ,0);Βσ(ί.γ,90,true);}if(Э<followPathDock*λ&&Э!=-1){в(Ђ,10);е(ί.γ,ί.δ,ί.Ѐ,false);}if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null){Σ(Β.ī);return;}if(Э<wpReachedDist/2&&Э!=-1){Σ(Β.ħ);return;}}if(Ψ==Β.ħ||Ψ==Β.Č){if(Ў)
{if(Ψ==Β.ħ){Ϊ ί=ц();if(!Ȓ(ί,dockDist*λ,true,out Ђ)){Ȕ(р.ƒ);Ϗ();return;}Ƿ(true,true,false,Ђ,0);е(ί.γ,ί.δ,ί.Ѐ,10,false);}if(Ψ==Β.Č){е(Ζ.γ,Ζ.δ,Ζ.Ѐ,0.5f,false);Ђ=Ζ.Ε;Ƿ(true,true,false,Ђ,0);}}if(ΒΙ){Ασ(false,0,0,0,0);if(Ψ==Β.ħ) Σ(Β.ī);if(Ψ==Β.Č) Ϗ();return;}}
if(Ψ==Β.ī){if(Ъ(MyShipConnectorStatus.Connected)!=null){if(Κ==Ι.ά) Σ(Β.ĕ);else Σ(Β.Ȗ);return;}Ϊ ί=ц();if(Ў){м=0;ΙΠ=DateTime.Now;Џ=0;е(ί.γ,ί.δ,ί.Ѐ,false);}Vector3I ΔΜ=new Vector3I((int) ί.и.X,(int) ί.и.Y,(int) ί.и.Z);IMySlimBlock ΖΪ=Me.CubeGrid.GetCubeBlock(ΔΜ);float Ζκ=dockingSpeed;
float Ικ=dockingSpeed*5;float ΖΗ=Math.Max(1.5f,Math.Min(5f,λ*0.15f));if(!Ȓ(ί,0,true,out Ђ)||!Ȓ(ί,ΖΗ,true,out ǌ)||
ΖΪ==null||!ΖΪ.FatBlock.IsFunctional){Ȕ(р.ƒ);Ϗ();return;}if(м==1||(Vector3.Distance(β,Ђ)<=ΖΗ*1.1f&&!Ў))
м=1;else{Vector3 ΚΙ=п(Ω,ǌ-β);Vector3 ΔΛ=п(Ω,Ω.GetNaturalGravity());float ΙЂ=ΑΩ(ΚΙ,ΔΛ,null);Ζκ=Math.Min(Ικ,ΙЂ);}Ƿ(true,false,false,Ђ,Ђ-β,dockingSpeed,Ζκ);if(Ў) ы=(float) Э;IMyShipConnector ΓЊ=Ъ(MyShipConnectorStatus.Connectable);if(ΓЊ!=null){Ƿ(false,false,false,Ђ,0);
if(Џ>0) Џ=0;Џ--;if(Џ<-5){ΓЊ.Connect();if(ΓЊ.Status==MyShipConnectorStatus.Connected){if(Κ==Ι.ά) Σ(Β.ĕ);else Σ(Β.Ȗ);Γο();Ɵ(Ƞ,true);return;}}}else{float Ζζ=(float) Math.Round(Э,1);if(Ζζ<ы){Џ=-1;ы=Ζζ;}else Џ++;if(Џ>20){Σ(Β.ƞ);return;}}}if(Ψ==Β.Ȗ||Ψ==Β.ĕ||
Ψ==Β.Αω||Ψ==Β.Αή||Ψ==Β.Ī){bool Γχ=false,Γψ=false;if(Κ==Ι.ά){if(ц()==Ξ) Γχ=true;else if(ц()==Ζ) Γψ=true;}if(Ў){ΑΔ=false;if(Ъ(MyShipConnectorStatus.Connected)==null){Σ(Β.Ǣ);return;}Γο();if(Γχ) Αφ(Ѓ.ǧ);if(Γψ) Αφ(Є.ǧ);ō=false;Ȭ=false;ȱ=false;ΑΞ=false;}if(Ъ(MyShipConnectorStatus.Connected)==null){Ϗ();Ȕ(р.Ες);return;}
if(α!=Π.Д||ј==-1||Ǻ==ѐ.None) Ȭ=true;else if(Ș>=100f) Ȭ=true;else if(Ș<=99f) Ȭ=false;if(α!=Π.Д||љ==-1||ǒ.Count==0) ȱ=true;else if(Ȳ>=100f) ȱ=true;else if(Ȳ<=99) ȱ=false;if(α!=Π.Д||к==-1||Ģ.Count==0) ΑΞ=true;else ΑΞ=Ŏ>=к;ş я=null;if(Γχ) я=Ѓ;
if(Γψ) я=Є;if(я!=null&&(я.Н==π.Αν||я.Н==π.Αμ)) Ȭ=true;if(я!=null&&(я.Н==π.ΓΥ)) if(!ō) Ȭ=false;if(я!=null&&(я.Н==π.ΓΩ||я.Н==π.ΓΧ)) ȱ=true;if(я!=null&&(я.Н==π.ΓΨ)) if(!ō) ȱ=false;if(ȳ){ChargeMode ΖΤ=Ȭ?ChargeMode.Auto:ChargeMode.Recharge;if(я!=null&&(я.Н==π.Αμ||я.Н==π.Αν))
ΖΤ=ChargeMode.Discharge;Γζ(ΖΤ);Γθ(!ȱ);}if(!ō){if(Κ==Ι.ά)
ō=α!=Π.Д||Ηύ(Ў,true)||ΑΜ;else
ō=α!=Π.Д||Εσ();}else{if(!Ȭ) Σ(Β.Ī);if(!ȱ) Σ(Β.Αή);if(!ΑΞ) Σ(Β.Αω);Ў=false;}if(ō&&Ȭ&&ȱ&&ΑΞ){Γζ(ChargeMode.Auto);Γθ(false);if(α==Π.Д){if(Κ==Ι.ά){if(ц()==Ξ) Αφ(Ѓ.Ǫ);else if(ц()==Ζ) Αφ(Є.Ǫ);if(ц()==Ξ) ŝ(Ζ,Π.Œ);else ŝ(Ξ,Π.ő);}}Σ(Β.Ǣ);return;}}if(Ψ==Β.Ǣ){if(ΑΡ){if(!ΕΦ()) return;
ΑΡ=false;Σ(Β.Ǣ);return;}if(Ў){if(!ΕΦ()){ΑΡ=true;return;}IMyShipConnector ț=Ъ(MyShipConnectorStatus.Connected);if(ț==null){Σ(Β.н);return;}IMyShipConnector ι=ț.OtherConnector;ΕЃ(ț,false);Ɵ(Ƞ,false);Ϊ ί=null;Ϊ ѕ=Κ==Ι.ά?Ξ:ц();if(Vector3.Distance(ț.GetPosition(),ѕ.Ε)<5f&&Ξ.ζ) ί=ѕ;
if(Vector3.Distance(ț.GetPosition(),Ζ.Ε)<5f&&Ζ.ζ) ί=Ζ;if(ί!=null){if(!Ȓ(ί,dockDist*λ,true,out Ђ)){Ȕ(р.ƒ);Ϗ();return;}в(Ђ,5);е(ί.γ,ί.δ,ί.Ѐ,false);}else в(β+ι.WorldMatrix.Forward*dockDist*λ,5);if(α==Π.Д) Ȕ(р.Αρ);}if(Э<wpReachedDist){ǎ(ѓ,true);Σ(Β.н);return;}}if(Ψ==Β.Ŗ)
{if(Ў){Ǹ(true);ǎ(ω,false);Ђ=Ζ.Ε;в(Ђ,20);е(Ζ.γ,Ζ.δ,Ζ.Ѐ,false);}if(Э<wpReachedDist/2){Σ(Β.Č);return;}}if(Ψ==Β.Đ){if(Ў){Ǹ(true);ǎ(ω,false);int Ц=-1;double Ǿ=-1;for(int i=Φ.Count-1;i>=0;i--){double І=Vector3.Distance(Φ[i].Ε,β);if(Ǿ==-1||І<Ǿ){Ц=i;Ǿ=І;}}
if(Ц==-1){Σ(Β.Е);return;}Ђ=Φ[Ц].Ε;в(Ђ,10);е(Ζ.γ,Ζ.δ,Ζ.Ѐ,false);}if(Э<wpReachedDist){Σ(Β.Е);return;}}if(Ψ==Β.Αΰ){bool Ǒ=false;if(Ў){Ǹ(true);if((Ю==Ѝ.ΒЄ||Ю==Ѝ.Αά)&&Αζ()&&
Ŕ(ǭ,Ω.GetNaturalGravity())<25&&ξ>=2&&И>=2){Vector3 Ƶ=β;if(ώ>0&&ό<И-1) Ƶ=Ȩ(ώ-1,ό+1,true);else if(ώ<ξ-1&&ό<И-1) Ƶ=Ȩ(ώ+1,ό+1,true);else if(ώ<ξ-1&&ό>0) Ƶ=Ȩ(ώ+1,ό-1,true);else if(ώ>0&&ό>0) Ƶ=Ȩ(ώ-1,ό-1,true);else Ǒ=true;if(!Ǒ) в(Ƶ,10);
}else Ǒ=true;}if(Э<wpReachedDist/2) Ǒ=true;if(Ǒ){Σ(Β.Αψ);return;}}if(Ψ==Β.Γς||Ψ==Β.Αψ){if(Ў){Ƿ(true,true,false,β,0);ǎ(ω,false);Ǹ(true);Џ=-1;м=Ю==Ѝ.ΓΓ||Ю==Ѝ.Αε?0:-1;}bool Ĺ=!Βό();int Βζ=Ȧ("STONE","ORE",Х.Ĩ);if(Ю==Ѝ.Βώ||Ю==Ѝ.Αε||Ю==Ѝ.Αά)
Βζ+=Ȧ("ICE","ORE",Х.Ĩ);bool ΙΩ=Βζ>0;bool ΔΖ=false;if(м>=0){float Š=(float) Math.Sin(ΕΪ(м))*ȍ/3f;float ΘЊ=(float) Math.Cos(ΕΪ(м))*Ǵ/3f;Vector3 ΘБ=Ȩ(ώ,ό,true)+Γπ(ǭ,ă*-1,new Vector3(Š,ΘЊ,0));в(ΘБ,0.3f);if(Э<Math.Min(ȍ,Ǵ)/10f) м+=5f;if(м>=360) м=0;}if(Џ==-1||Βζ<Џ){Џ=Βζ;ы=0;}
else{ы++;if(ы>50) ΔΖ=true;}if(!ΙΩ||Ĺ||ΔΖ){if(!Ĺ){int ΚΞ=Ȧ("","ORE",Х.Ĩ);if(Ƙ(true)) Ĺ=true;else if(100-(Б(ΚΞ,ΗΚ)*100)<minEjection) Ĺ=true;else Ȕ(р.Αρ);}if(ΔΖ&&Ĺ) Λ="Ejection failed";if(Ψ==Β.Αψ){if(Ĺ){if(Ξ.ζ) Σ(Β.Đ);else{Ϗ();Ĭ();Λ="Can´t return, no dock found";}}
else Σ(Β.đ);}else if(Ĺ) Σ(Β.Į);else Σ(Β.Ɯ);return;}}Ў=false;}void Ηω(List<Ϊ>Ħ,int ρ,List<Vector3>ΑΙ,float ȉ,bool ǰ,ref int ǥ){if(ǰ){for(int i=0;i<Ħ.Count;i++) Ħ[i].Ф=0;ǥ=-1;return;}if(ρ==0) return;int Ǚ=ρ*-1;if(ǥ==-1) ǥ=Ǚ>0?1:Ħ.Count-2;int ǂ=0;while(ǥ>=1&&ǥ<Ħ.Count-1)
{if(ǂ>50) return;ǂ++;try{if((Ǚ<0&&ǥ>=1)||(Ǚ>0&&ǥ<=Ħ.Count-2)){Ϊ ν=Ħ[ǥ];bool ΗΖ=false;for(int j=0;j<ΑΙ.Count;j++)
if(Vector3.Distance(ν.Ε,ΑΙ[j])<=ȉ){ΗΖ=true;break;}if(!ΗΖ){Ϊ Βά=Ħ[ǥ-Ǚ];Ϊ Ń=Ħ[ǥ+Ǚ];Vector3 ΚΚ=ν.Ε-Ń.Ε;Vector3 ΔА=Βά.Ε-ν.Ε;Vector3 Ιό=ν.Ε+Vector3.Normalize(ΚΚ)*ΔА.Length();Vector3 ΙΝ=Βά.Ε-Ιό;Vector3 ΒΞ=İ(ρ>0?ν.δ:ν.δ*-1,ν.γ*-1,ΙΝ);Vector3 Ιι=İ(ρ>0?ν.δ:ν.δ*-1,ν.γ*-1,ΔА);
Vector3 ΒΠ=İ(ρ>0?ν.δ:ν.δ*-1,ν.γ*-1,ν.φ);ν.Ф=(float) Math.Sqrt(Math.Pow(Βά.Ф,2)+Math.Pow(ΑΩ(-Ιι,ΒΠ,ν),2));for(int j=0;j<ΑΙ.Count;j++)
if(Vector3.Distance(Βά.Ε,ΑΙ[j])<=ȉ){Vector3 ΒΡ=İ(ρ>0?ν.δ:ν.δ*-1,ν.γ*-1,ΑΙ[j]-ν.Ε);float ΙБ=ΑΩ(-ΒΡ,ΒΠ,ν);ν.Ф=Math.Min(ν.Ф,ΙБ)/2f;}if(ΒΞ.Length()==0) ΒΞ=new Vector3(0,0,1);Vector3 Ιθ=İ(ν.δ,ν.γ*-1,ν.φ);float ΚΔ=ƕ(-ΒΞ,Ιθ,ν);float Αϊ=Б(ΚΔ,ŋ);float ΔЍ=(float) Math.Sqrt(Б(ΒΞ.Length()*1.0f,0.5f*Αϊ));
ν.Ф=Math.Min(ν.Ф,(ΔА.Length()/ΔЍ)*ǹ);}}}catch{return;}ǥ+=Ǚ;}ǥ=-1;}void Γρ(bool ǰ){if(ǰ){ǃ=0;return;}if(Κ==Ι.ά) return;float Ǒ=Ь*Math.Max(1,з);if(ā==Ь) Ǒ+=Math.Min(з,г);float ΚΝ=ξ*И*Math.Max(1,з);ǃ=Math.Max(ǃ,(float) Math.Min(Ǒ/ΚΝ*100.0,100));}MyDetectedEntityType Εδ()
{try{if(ȓ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒЏ(bool Ȃ){if(Κ==Ι.ǟ&&Εδ()==MyDetectedEntityType.None&&!Ɨ(ψ,true))
return fastSpeed;else return Ȃ?Ɛ:Ə;}void Θυ(){float ΖБ=0,Ζΰ=0,Ľ=0,Δσ=0;for(int i=0;i<Ė.Count;i++){IMyBatteryBlock ƪ=Ė[i];if(!ȓ(ƪ,true)) continue;ΖБ+=ƪ.MaxStoredPower;Ζΰ+=ƪ.CurrentStoredPower;Ľ+=ƪ.CurrentInput;Δσ+=ƪ.CurrentOutput;}Ș=(float) Math.Round(Б(Ζΰ,ΖБ)*100,1);
if(Ľ>=Δσ) Ǻ=ѐ.Ī;else Ǻ=ѐ.ΕΫ;if(Ľ==0&&Δσ==0||Ș==100.0) Ǻ=ѐ.о;if(Ė.Count==0) Ǻ=ѐ.None;}void Θϋ(){float Ζπ=0;for(int i=0;i<ǒ.Count;i++){IMyGasTank Ηδ=ǒ[i];if(!ȓ(Ηδ,true)) continue;Ζπ+=(float) Ηδ.FilledRatio;}Ȳ=Б(Ζπ,ǒ.Count)*100f;}void ΘЄ(){Ŏ=0;try{for(int i=0;i<Ģ.Count;i++)
{IMyReactor Δϋ=Ģ[i];if(!ȓ(Δϋ,true)) continue;List<MyInventoryItem>Ї=new List<MyInventoryItem>();Δϋ.GetInventory(0).GetItems(Ї);float Ѕ=0;for(int j=0;j<Ї.Count;j++){MyInventoryItem ύ=Ї[j];if(ύ.Type.SubtypeId.ToUpper()=="URANIUM"&&ύ.Type.TypeId.ToUpper().Contains("_INGOT"))
Ѕ+=(float) ύ.Amount;}if(Ѕ<Ŏ||i==0){Ŏ=Ѕ;Δζ=Δϋ.CustomName;}}}catch(Exception e){Ā=e;}}void Θφ(){ȅ=0;ǿ=0;try{for(int i=0;i<ŧ.Count;i++){IMyTerminalBlock Χ=ŧ[i];if(!ȓ(Χ,true)) continue;ǿ+=(float) Χ.GetInventory(0).CurrentVolume;ȅ+=(float) Χ.GetInventory(0).MaxVolume;
}ΒΝ=(float) Math.Min(Math.Round(Б(ǿ,ȅ)*100,1),100.0);}catch(Exception e){Ā=e;}}void Θό(){try{ǩ.Clear();for(int i=0;i<ŧ.Count;i++){IMyTerminalBlock Χ=ŧ[i];if(!ȓ(Χ,true)) continue;ΕΜ(Χ,Х.ř);}if(Ю!=Ѝ.ΓΤ){for(int i=0;i<Ĵ.Count;i++){IMyTerminalBlock Χ=Ĵ[i];if(!ȓ(Χ,true)) continue;
ΕΜ(Χ,Х.ΘΟ);}}Θο(ǩ);}catch(Exception e){Ā=e;}}void Ηϋ(){if(ΑЈ){if(Εγ().Count>ΒΗ){ΑЈ=false;if(ь!=х.Ɩ){Ϗ();if(ь==х.ΒЉ) Ĭ();if(ь==х.Απ)
if(Ξ.ζ) Αα();else Ĭ();}Λ="Damage detected";}}}bool Βό(){if(!ΒΘ) return true;if(α==Π.Д){if(ј>0&&Ǻ!=ѐ.None)
if(Ș<=ј){Λ="Low energy! Move home";return false;}if(к>0&&Ģ.Count>0)
if(Ŏ<=к){Λ="Low fuel: "+Δζ;return false;}if(љ>0&&ǒ.Count>0)
if(Ȳ<=љ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Εγ(){List<IMyTerminalBlock>Ķ=new List<IMyTerminalBlock>();for(int i=0;i<ƥ.Count;i++){IMyTerminalBlock Χ=ƥ[i];if(Ɨ(Χ,false)) Ķ.Add(Χ);}return Ķ;}bool Ɨ(IMyTerminalBlock Χ,bool ΔІ){return(!ȓ(Χ,ΔІ)||!Χ.IsFunctional);
}bool ȓ(IMyTerminalBlock Χ,bool ΔІ){if(Χ==null) return false;try{IMyCubeBlock Ζυ=Me.CubeGrid.GetCubeBlock(Χ.Position).FatBlock;if(ΔІ) return Ζυ==Χ;else return Ζυ.GetType()==Χ.GetType();}catch{return false;}}float ΗІ(IMyTerminalBlock Χ,String ѝ,String Ηά,String[]Ȁ){float Ȝ=0;for(int i=0;i<Χ.InventoryCount;i++)
{IMyInventory Ů=Χ.GetInventory(i);List<MyInventoryItem>Ї=new List<MyInventoryItem>();Ů.GetItems(Ї);foreach(MyInventoryItem ύ in Ї){if(Ȁ!=null&&(Ȁ.Contains(ύ.Type.TypeId.ToUpper())||Ȁ.Contains(ύ.Type.SubtypeId.ToUpper())))
continue;if((ѝ==""||ύ.Type.TypeId.ToUpper()==ѝ)&&(Ηά==""||ύ.Type.SubtypeId.ToUpper()==Ηά))
Ȝ+=(float) ύ.Amount;}}return Ȝ;}ж ΗЎ(String ή,String υ,Х Ш,bool ΙΙ){ή=ή.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǩ.Count;i++){ж ύ=ǩ[i];if(ύ.ή.ToUpper()==ή&&ύ.υ.ToUpper()==υ&&(ύ.Ŵ==Ш||Ш==Х.Ĩ))
return ύ;}ж έ=null;if(ΙΙ){έ=new ж(ή,υ,0,Ш);ǩ.Add(έ);}return έ;}int Ȧ(String ή,String υ,Х Ш){return Ȧ(ή,υ,Ш,null);}int Ȧ(String ή,String υ,Х Ш,String[]Ȁ){int Ȝ=0;ή=ή.ToUpper();υ=υ.ToUpper();for(int i=0;i<ǩ.Count;i++){ж ύ=ǩ[i];if(Ȁ!=null&&Ȁ.Contains(ύ.ή.ToUpper())) continue;
if((ή==""||ύ.ή.ToUpper()==ή)&&(υ==""||ύ.υ.ToUpper()==υ)&&(ύ.Ŵ==Ш||Ш==Х.Ĩ))
Ȝ+=ύ.Ѕ;}return Ȝ;}void ΕΜ(IMyTerminalBlock Χ,Х Ш){for(int i=0;i<Χ.InventoryCount;i++){List<MyInventoryItem>Ї=new List<MyInventoryItem>();Χ.GetInventory(i).GetItems(Ї);for(int j=0;j<Ї.Count;j++)
ΗЎ(Ї[j].Type.SubtypeId,Ї[j].Type.TypeId.Replace("MyObjectBuilder_",""),Ш,true).Ѕ+=(int) Ї[j].Amount;}}void Θο(List<ж>Ј){for(int i=Ј.Count-1;i>0;i--)
for(int j=0;j<i;j++){ж a=Ј[j];ж b=Ј[j+1];if(a.Ѕ<b.Ѕ) Ј.Move(j,j+1);}}bool Ƙ(bool Κβ){if(ċ&&Κ!=Ι.ά)
if(Я!=-1&&ŋ>=Я){Λ="Ship too heavy";return true;}if(ΒΝ>=Ȅ||ΑΔ){ΑΔ=false;Λ="Ship is full";return true;}return false;}bool Εσ(){String[]Ȁ=null;if(!Ċ) Ȁ=new string[]{"ICE"};if(Κ==Ι.ƛ) return Ȧ("","ORE",Х.ř,Ȁ)==0;if(Κ==Ι.ǟ) return Ȧ("","COMPONENT",Х.ř,Ȁ)==0;else return Ȧ("","",Х.ř,Ȁ)==0;
}bool ΘΣ(){List<IMyCockpit>ΓЉ=new List<IMyCockpit>();χ.GetBlocksOfType(ΓЉ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓЉ.Count;i++)
if(ΓЉ[i].IsUnderControl) return true;return false;}bool Ηύ(bool ǰ,bool Ββ){IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connected);if(Л==null) return false;if(Vector3.Distance(Ξ.Ε,Л.GetPosition())<5)
return ΕΧ(Ѓ,ǰ,Ββ);if(Vector3.Distance(Ζ.Ε,Л.GetPosition())<5)
return ΕΧ(Є,ǰ,Ββ);return false;}bool ΕΧ(ş Ț,bool ǰ,bool Ββ){if(ǰ) Ț.Θί();Ț.ΘЁ();bool ю=false;String ќ="";switch(Ț.Н){case π.ΓΦ:{ќ="Waiting for command";ю=false;break;}case π.Εύ:{ќ="Waiting for cargo";ю=Ƙ(true);break;}case π.Εό:{ќ="Unloading";ю=Εσ();break;}case π.ΓΫ:{ю=true;break;}
case π.ΓΥ:{ќ="Charging batteries";ю=Ș>=100f;break;}case π.Αν:{ќ="Discharging batteries";ю=Ș<=25f;break;}case π.Αμ:{ќ="Discharging batteries";ю=Ș<=0f;break;}case π.ΓΨ:{ќ="Filling up hydrogen";ю=Ȳ>=100f;break;}case π.ΓΩ:{ќ="Unloading hydrogen";ю=Ȳ<=25f;break;}
case π.ΓΧ:{ќ="Unloading hydrogen";ю=Ȳ<=0f;break;}case π.ΓΪ:{bool Ζύ=ΘΣ();if(!Ζύ) Ț.Δχ=true;ю=Ț.Δχ&&Ζύ;ќ="Waiting for passengers";break;}}if(!ю) Ț.Βϋ(true);if(ю&&Ț.ΒБ()){ю=Ț.Βϋ(false);ќ="Undocking in: "+Αβ((int) Math.Max(0,Ț.Ǧ-Ț.ě));}if(Ββ) Ηρ=ќ;return ю;}void Ητ()
{if(!ů) return;if(ω.Count<=1) return;float Ηι=0,Ηθ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȍ=ω[i];if(Ɨ(Ȍ,true)) continue;Ηι+=(float) Ȍ.GetInventory(0).MaxVolume;Ηθ+=(float) Ȍ.GetInventory(0).CurrentVolume;}float ΖΚ=(float) Math.Round(Б(Ηθ,Ηι),5);for(int ņ=0;ņ<Math.Max(1,Math.Floor(ω.Count/10f));ņ++)
{float ΔΥ=0,Δε=0;float Ζϊ=0,ΖЎ=0;IMyTerminalBlock Ȱ=null,Ĝ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ȍ=ω[i];if(Ɨ(Ȍ,true)) continue;float Δμ=(float) Ȍ.GetInventory(0).MaxVolume;float ΒΑ=Б((float) Ȍ.GetInventory(0).CurrentVolume,Δμ);if(Ȱ==null||ΒΑ>ΔΥ){Ȱ=Ȍ;ΔΥ=ΒΑ;Ζϊ=Δμ;}
if(Ĝ==null||ΒΑ<Δε){Ĝ=Ȍ;Δε=ΒΑ;ΖЎ=Δμ;}}if(Ȱ==null||Ĝ==null||Ȱ==Ĝ) return;if(checkConveyorSystem&&!Ȱ.GetInventory(0).IsConnectedTo(Ĝ.GetInventory(0))){if(Γύ>20) Λ="Inventory balancing failed";else Γύ++;return;}Γύ=0;List<MyInventoryItem>Ї=new List<MyInventoryItem>();
Ȱ.GetInventory(0).GetItems(Ї);float ΑΠ=0;if(Ї.Count==0) continue;MyInventoryItem ΒΒ=Ї[0];String ΖЁ=ΒΒ.Type.TypeId+ΒΒ.Type.SubtypeId;if(!Ηπ.TryGetValue(ΖЁ,out ΑΠ)){if(ΘΧ(Ȱ.GetInventory(0),0,Ĝ.GetInventory(0),out ΑΠ))
Ηπ.Add(ΖЁ,ΑΠ);else return;}float ΚΛ=((ΔΥ-ΖΚ)*Ζϊ/ΑΠ);float ΚΜ=((ΖΚ-Δε)*ΖЎ/ΑΠ);int ΕΓ=(int) Math.Min(ΚΜ,ΚΛ);if(ΕΓ<=0) return;if((float) ΒΒ.Amount<ΕΓ)
Ȱ.GetInventory(0).TransferItemTo(Ĝ.GetInventory(0),0,null,null,ΒΒ.Amount);else
Ȱ.GetInventory(0).TransferItemTo(Ĝ.GetInventory(0),0,null,null,ΕΓ);}}bool ΘΧ(IMyInventory ΑΕ,int Α,IMyInventory ΙΜ,out float Ηο){Ηο=0;float ΘЎ=(float) ΑΕ.CurrentVolume;List<MyInventoryItem>ΓϏ=new List<MyInventoryItem>();ΑΕ.GetItems(ΓϏ);float ΖΞ=0;for(int i=0;i<ΓϏ.Count;i++) ΖΞ+=(float) ΓϏ[i].Amount;
ΑΕ.TransferItemTo(ΙΜ,Α,null,null,1);float Ην=ΘЎ-(float) ΑΕ.CurrentVolume;List<MyInventoryItem>Γτ=new List<MyInventoryItem>();ΑΕ.GetItems(Γτ);float ΖΒ=0;for(int i=0;i<Γτ.Count;i++) ΖΒ+=(float) Γτ[i].Amount;if(Ην==0f||!ΓΔ(0.9999,ΖΞ-ΖΒ,1.0001))
return false;Ηο=Ην;return true;}void Θϊ(){Vector3 ƹ=Ђ+Ĥ;Vector3 ȋ=ƹ-β;if(ȋ.Length()==0) ȋ=new Vector3(0,0,-1);Vector3 ΒΡ=п(Ω,ȋ);Vector3 Ηε=Vector3.Normalize(ΒΡ);Vector3 ų=п(Ω,Ω.GetNaturalGravity());float ΙЌ=Δτ>0?Math.Max(0,1-Ŕ(ȋ,Ζς)/5):0;float Ф=(float) Math.Min((Δι>0?Δι:1000f),Math.Max(ΑΩ(-ΒΡ,ų,null),Δτ*ΙЌ));
if(!Ηη) Ф=0;if(ΗΦ) Ф=Math.Max(0,1-ΑІ/Αϋ)*Ф;if(generalSpeedLimit>0) Ф=Math.Min(generalSpeedLimit,Ф);if(ż) Ф=0;if(Ȏ>=0) Ф=Math.Min(Ф,Ȏ);if(ΗΧ) Ф*=(float) Math.Min(1,Б(ȋ.Length(),wpReachedDist)/2);Vector3 ΖЋ=п(Ω,Ω.GetShipVelocities().LinearVelocity);float ΘЉ=(float)(Math.Max(0,15-Ŕ(-Ηε,-ΖЋ))/15)*0.85f+0.15f;
ΔЊ+=Math.Sign(ΘЉ-ΔЊ)/10f;Vector3 Ę=Ηε*Ф*ΔЊ-(ΖЋ);Vector3 Γϋ=ΒЎ(Ę,null);if(Βη&&Э>0.1f){Ę.X*=Γκ(Ę.X,ref Βθ.X,1f,Γϋ.X,20);Ę.Y*=Γκ(Ę.Y,ref Βθ.Y,1f,Γϋ.Y,20);Ę.Z*=Γκ(Ę.Z,ref Βθ.Z,1f,Γϋ.Z,20);}else Βθ=new Vector3(1,1,1);ΑΚ=ŋ*Ę-ų*ŋ;ΕΝ(ΑΚ,Βη);Э=Vector3.Distance(β,ƹ);
}float Γκ(float value,ref float Ё,float Ǚ,float Ă,float Δκ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ё)) Ё+=Math.Sign(Ё)*Ǚ;else Ё=value;if(value==0) Ё=1;float έ=Math.Abs(Ё);if(έ<Δκ||Ă==0) return έ;Ё=Math.Min(Δκ,Math.Max(-Δκ,Ё));έ=Math.Abs(Ă);return έ;
}void ΕБ(){float ћ=90,ǝ=90,Ǘ=90;float Ǳ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȃ,ǔ,Ǉ,Δβ;if(Δγ){Ȃ=Vector3.Normalize(Ђ-β);Ǉ=п(Ω,Ȃ);ǔ=п(Ω,ΔЈ);ћ=Ŕ(Ǉ,new Vector3(0,-1,0))-90;ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);
Ǘ=ĩ(Ǉ,new Vector3(-1,0,0),Ǉ.Z);}else{Ȃ=ΔЉ;ǔ=п(Ω,ΔЈ);Ǉ=п(Ω,ΔЉ);Δβ=п(Ω,Ηζ);ћ=ĩ(ǔ,new Vector3(0,0,1),ǔ.Y);ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);Ǘ=ĩ(Δβ,new Vector3(0,0,1),Δβ.X);}if(ΖΔ&&Αζ()){Vector3 ΔΛ=Ω.GetNaturalGravity();ǔ=п(Ω,ΔΛ);ћ=ĩ(ǔ,new Vector3(0,0,1),ǔ.Y);
ǝ=ĩ(ǔ,new Vector3(-1,0,0),ǔ.Y);}if(!ΓΔ(-45,ǝ,45)){ћ=0;Ǘ=0;}if(!ΓΔ(-45,Ǘ,45)) ћ=0;Ασ(ΔΝ,1,(-ћ)*Ǳ,(-Ǘ)*Ǳ,(-ǝ)*Ǳ);ΑІ=Math.Max(Math.Abs(ћ),Math.Max(Math.Abs(ǝ),Math.Abs(Ǘ)));ΒΙ=ΑІ<=Αϋ;}void Ηϊ(IMyTerminalBlock Βέ){if(Βέ==null) return;ǵ=new float[3,2];ǚ=new Dictionary<string,float[,]>();
for(int i=0;i<ȣ.Count;i++){IMyThrust у=ȣ[i];if(!у.IsFunctional) continue;Vector3 ρ=п(Βέ,у.WorldMatrix.Backward);float Ě=у.MaxEffectiveThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) ǵ[0,0]+=Ě;else ǵ[0,1]-=Ě;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) ǵ[1,0]+=Ě;else ǵ[1,1]-=Ě;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) ǵ[2,0]+=Ě;else ǵ[2,1]-=Ě;String ѝ=Ελ(у);float[,]Ǜ=null;if(ǚ.ContainsKey(ѝ)) Ǜ=ǚ[ѝ];else{Ǜ=new float[3,2];ǚ.Add(ѝ,Ǜ);}float Ă=у.MaxThrust;if(Math.Round(ρ.X,2)!=0.0)
if(ρ.X>=0) Ǜ[0,0]+=Ă;else Ǜ[0,1]-=Ă;if(Math.Round(ρ.Y,2)!=0.0)
if(ρ.Y>=0) Ǜ[1,0]+=Ă;else Ǜ[1,1]-=Ă;if(Math.Round(ρ.Z,2)!=0.0)
if(ρ.Z>=0) Ǜ[2,0]+=Ă;else Ǜ[2,1]-=Ă;}}static String Ελ(IMyThrust у){return у.BlockDefinition.SubtypeId;}Vector3 ΓΛ(Vector3 ǐ,float[,]ŵ){return new Vector3(
ǐ.X>=0?ŵ[0,0]:ŵ[0,1],
ǐ.Y>=0?ŵ[1,0]:ŵ[1,1],
ǐ.Z>=0?ŵ[2,0]:ŵ[2,1]);}bool Εκ(Ϊ Ϋ,String ѝ,out float ΔΕ){ΔΕ=0;int Α=Ȣ.IndexOf(ѝ);if(Α==-1||Ϋ.ǳ==null||Α>=Ϋ.ǳ.Length)
return false;ΔΕ=Ϋ.ǳ[Α];if(ΔΕ==-1) return false;return true;}Vector3 ΒЎ(Vector3 ǐ,Ϊ Ϋ){if(Ϋ!=null){Vector3 έ=new Vector3();for(int i=0;i<ǚ.Keys.Count;i++){String ѝ=ǚ.Keys.ElementAt(i);float Ʒ=0;if(!Εκ(Ϋ,ѝ,out Ʒ))
return ΓΛ(ǐ,ǵ);έ+=ΓΛ(ǐ,ǚ.Values.ElementAt(i))*Ʒ;}return έ;}return ΓΛ(ǐ,ǵ);}float ƕ(Vector3 ǐ,Ϊ Ϋ){return ƕ(ǐ,new Vector3(),Ϋ);}float ƕ(Vector3 ǐ,Vector3 φ,Ϊ Ϋ){Vector3 ΚΓ=ΒЎ(ǐ,Ϋ);Vector3 Ě=ΚΓ+φ*ŋ;float ň=(Ě/ǐ).AbsMin();return(float)(ǐ*ň).Length();}float ΑΩ(Vector3 ΒΟ,Vector3 ų,Ϊ Ϋ)
{if(ΒΟ.Length()==0) return 0;float ΖΓ=1;if(ų.Length()>0) ΖΓ=Math.Min(1,Ŕ(-ų,ΒΟ)/90)*0.4f+0.6f;float ΑΚ=ƕ(ΒΟ,ų,Ϋ);if(ΑΚ==0) return 0.1f;float Αϊ=Б(ΑΚ,ŋ);float ΔЍ=(float) Math.Sqrt(Б(ΒΟ.Length(),Αϊ*0.5f));return Αϊ*ΔЍ*ΖΓ*ǹ;}float Αδ(Vector3 Ȃ,Vector3 Ќ,Vector3 φ,Ϊ Ϋ)
{if(φ.Length()==0f) return 0;Vector3 ΒΠ=İ(Ȃ,Ќ,Vector3.Normalize(φ));float Ǩ=ƕ(-ΒΠ,Ϋ);return Ǩ/φ.Length();}void ΕΡ(bool Ιΰ){float ǜ=0;float ΔΘ=0.9f;if(Ιΰ){Я=-1;ŏ=0;Βδ=null;if(α!=Π.ч&&Ζ.φ.Length()!=0){ǜ=ΔΘ*Αδ(Ζ.δ,Ζ.γ*-1,Ζ.φ,null);if(ǜ<Я||Я==-1) Я=ǜ;}if(Ξ.ζ&&Ξ.φ.Length()!=0)
{ǜ=ΔΘ*Αδ(Ξ.δ,Ξ.γ*-1,Ξ.φ,null);if(ǜ<Я||Я==-1) Я=ǜ;}return;}if(ŏ==-1) return;if(ŏ>=0){int ΗΟ=0;while(ŏ<Φ.Count){if(ΗΟ>100) return;ΗΟ++;Ϊ Ϋ=Φ[ŏ];if(Ϋ.φ.Length()!=0f){ǜ=ΔΘ*Math.Min(
Αδ(Ϋ.δ,Ϋ.γ*-1,Ϋ.φ,Ϋ),
Αδ(Ϋ.δ*-1,Ϋ.γ*-1,Ϋ.φ,Ϋ));if(ǜ<Я||Я==-1) Я=ǜ;}else Βδ=Ϋ;ŏ++;}ŏ=-1;}bool ΔΠ=true;float ġ=0;if(Βδ!=null){for(int i=0;i<ǚ.Count;i++){String ѝ=ǚ.Keys.ElementAt(i);float[,]Ǜ=ǚ.Values.ElementAt(i);float Ʒ=0;if(!Εκ(Βδ,ѝ,out Ʒ)){ΔΠ=false;break;}for(int a=0;a<Ǜ.GetLength(0);a++)
for(int b=0;b<Ǜ.GetLength(1);b++){float ĳ=Math.Abs(Ǜ[a,b]*Ʒ);if(ĳ==0) continue;ΔΠ=true;if(ġ==0||ĳ<ġ) ġ=ĳ;}}}if(!ΔΠ){for(int a=0;a<ǵ.GetLength(0);a++)
for(int b=0;b<ǵ.GetLength(1);b++){float ĳ=Math.Abs(ǵ[a,b]);if(ĳ==0) continue;if(ġ==0||ĳ<ġ) ġ=ĳ;}}if(ġ>0){float Ιμ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǜ=Б(ġ,Ιμ);if(ǜ>0)
if(ǜ<Я||Я==-1) Я=ǜ;}}void ΕΝ(Vector3 Ǩ,bool З){if(!З){for(int i=0;i<ȣ.Count;i++)
ȣ[i].SetValueFloat("Override",0.0f);return;}Vector3 Γϊ=ΒЎ(Ǩ,null);float Κά=Math.Min(1,Math.Abs(Б(Ǩ.X,Γϊ.X)));float Κέ=Math.Min(1,Math.Abs(Б(Ǩ.Y,Γϊ.Y)));float Κή=Math.Min(1,Math.Abs(Б(Ǩ.Z,Γϊ.Z)));for(int i=0;i<ȣ.Count;i++){IMyThrust у=ȣ[i];Vector3 ρ=Θα(п(Ω,у.WorldMatrix.Backward),1);
if(ρ.X!=0&&Math.Sign(ρ.X)==Math.Sign(Ǩ.X))
у.SetValueFloat("Override",у.MaxThrust*Κά);else if(ρ.Y!=0&&Math.Sign(ρ.Y)==Math.Sign(Ǩ.Y))
у.SetValueFloat("Override",у.MaxThrust*Κέ);else if(ρ.Z!=0&&Math.Sign(ρ.Z)==Math.Sign(Ǩ.Z))
у.SetValueFloat("Override",у.MaxThrust*Κή);else
у.SetValueFloat("Override",0.0f);}}void Ασ(bool З,float Ιω,float ћ,float Ǘ,float ǝ){for(int i=0;i<ΒΔ.Count;i++){IMyGyro В=ΒΔ[i];В.GyroOverride=З;if(!З) В.GyroPower=100;else В.GyroPower=Ιω;if(!З) continue;Vector3 Ч=Ω.WorldMatrix.Forward;Vector3 л=Ω.WorldMatrix.Right;Vector3 Ќ=Ω.WorldMatrix.Up;
Vector3 Ū=В.WorldMatrix.Forward;Vector3 Ŭ=В.WorldMatrix.Up;Vector3 ū=В.WorldMatrix.Left*-1;if(Ū==Ч) В.SetValueFloat("Roll",ǝ);else if(Ū==(Ч*-1)) В.SetValueFloat("Roll",ǝ*-1);else if(Ŭ==(Ч*-1)) В.SetValueFloat("Yaw",ǝ);else if(Ŭ==Ч) В.SetValueFloat("Yaw",ǝ*-1);else if(ū==Ч) В.SetValueFloat("Pitch",ǝ);
else if(ū==(Ч*-1)) В.SetValueFloat("Pitch",ǝ*-1);if(ū==(л*-1)) В.SetValueFloat("Pitch",ћ);else if(ū==л) В.SetValueFloat("Pitch",ћ*-1);else if(Ŭ==л) В.SetValueFloat("Yaw",ћ);else if(Ŭ==(л*-1)) В.SetValueFloat("Yaw",ћ*-1);else if(Ū==(л*-1)) В.SetValueFloat("Roll",ћ);
else if(Ū==л) В.SetValueFloat("Roll",ћ*-1);if(Ŭ==(Ќ*-1)) В.SetValueFloat("Yaw",Ǘ);else if(Ŭ==Ќ) В.SetValueFloat("Yaw",Ǘ*-1);else if(ū==Ќ) В.SetValueFloat("Pitch",Ǘ);else if(ū==(Ќ*-1)) В.SetValueFloat("Pitch",Ǘ*-1);else if(Ū==Ќ) В.SetValueFloat("Roll",Ǘ);else if(Ū==(Ќ*-1)) В.SetValueFloat("Roll",Ǘ*-1);
}}void ΕЌ(){this.ΔΝ=false;}void е(Vector3 Ƴ,Vector3 Ч,Vector3 Δΰ,float ĉ,bool ΑΟ){Βσ(Ƴ,ĉ,ΑΟ);Αϋ=ĉ;Δγ=false;this.ΔЉ=Ч;this.Ηζ=Δΰ;}void е(Vector3 Ƴ,Vector3 Ч,Vector3 Δΰ,bool ΑΟ){е(Ƴ,Ч,Δΰ,2f,ΑΟ);}void Βσ(Vector3 Ƴ,float ĉ,bool ΑΟ){Αϋ=ĉ;this.ΔΝ=true;this.ΖΔ=ΑΟ;Δγ=true;
ΒΙ=false;this.ΔЈ=Ƴ;}void Γο(){Ƿ(false,false,false,Ђ,0);Βη=false;}void в(Vector3 μ,float Ǳ){Ƿ(true,false,false,μ,Ǳ);}void Ƿ(bool З,bool ΔЂ,bool ΕΖ,Vector3 μ,float Ǳ){Ƿ(З,ΔЂ,ΕΖ,μ,μ-β,0.0f,Ǳ);}void Ƿ(bool З,bool ΔЂ,bool ΕΖ,Vector3 μ,Vector3 ΙІ,float ΙЇ,float Ǳ){Βη=true;
this.Ηη=З;Ђ=μ;this.Δι=Ǳ;this.Δτ=ΙЇ;this.ΗΧ=ΔЂ;this.ΗΦ=ΕΖ;this.Ζς=ΙІ;Э=Vector3.Distance(μ,β);}void Θγ(){χ.GetBlocksOfType(ƥ,џ);}void Θδ(){if(Βί){Ћ=2;return;}List<IMyRemoteControl>ΑΑ=new List<IMyRemoteControl>();List<IMySensorBlock>ΗΤ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΑΘ=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(ΑΑ,џ);χ.GetBlocksOfType(У,џ);χ.GetBlocksOfType(ΗΤ,џ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΘ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);У=Ǟ(У,pamTag,true);ΕЅ();ΕІ(ΑΘ);į(У,setLCDFontAndSize,1.4f,false);į(Ȋ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ηα=Ǟ(ΗΤ,pamTag,true);if(Ηα.Count>0) ψ=Ηα[0];else ψ=null;if(Κ==Ι.ƛ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){Ћ=1;Λ="Drills are missing";}}else if(Κ==Ι.ǟ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){Ћ=1;Λ="Grinders are missing";}if(ψ==null){Ћ=1;Λ="Sensor is missing";}}else if(Κ==Ι.ά){χ.GetBlocksOfType(ΔЎ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ηΰ=Ǟ(ΑΑ,pamTag,true);if(Ηΰ.Count>0) ΑΑ=Ηΰ;if(ΑΑ.Count>0) Ω=ΑΑ[0];else{Ω=null;Ћ=2;Λ="Remote is missing";return;}λ=(float) Ω.CubeGrid.WorldVolume.Radius*2;ǖ=null;if(Κ!=Ι.ά){ΕΣ();if(ω.Count>0&&ǖ!=null){if(ψ!=null&&(ǖ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ω.WorldMatrix.Down==ψ.WorldMatrix.Down))){Ћ=1;Λ="Wrong sensor direction";}if(ǖ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
ǖ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){Ћ=2;Λ="Wrong remote direction";}}}}void Θε(){χ.GetBlocksOfType(Ƞ,џ);χ.GetBlocksOfType(ѓ,џ);χ.GetBlocksOfType(ȣ,џ);χ.GetBlocksOfType(ΒΔ,џ);χ.GetBlocksOfType(Ė,џ);χ.GetBlocksOfType(Ģ,џ);χ.GetBlocksOfType(ǒ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(Βγ,џ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ѓ=Ǟ(ѓ,"ConnectorMedium",false);else
ѓ=Ǟ(ѓ,"Connector",false);List<IMyShipConnector>Ηή=Ǟ(ѓ,pamTag,true);if(Ηή.Count>0) ѓ=Ηή;if(Ћ<=1){if(ѓ.Count==0){Ћ=1;Λ="Connector is missing";}if(ΒΔ.Count==0){Ћ=1;Λ="Gyros are missing";}if(ȣ.Count==0){Ћ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Ηβ=Ǟ(Βγ,pamTag,true);
if(Ηβ.Count>0) Βγ=Ηβ;List<IMyLandingGear>Ηί=Ǟ(Ƞ,pamTag,true);if(Ηί.Count>0) Ƞ=Ηί;for(int i=0;i<Ƞ.Count;i++) Ƞ[i].AutoLock=false;List<IMyBatteryBlock>Ηέ=Ǟ(Ė,pamTag,true);if(Ηέ.Count>0) Ė=Ηέ;List<IMyGasTank>Ηγ=Ǟ(ǒ,pamTag,true);if(Ηγ.Count>0) ǒ=Ηγ;}void Θζ(){χ.GetBlocksOfType(Ĵ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ŧ.Clear();for(int i=Ĵ.Count-1;i>=0;i--){if(ΘΠ(Ĵ[i])){ŧ.Add(Ĵ[i]);Ĵ.RemoveAt(i);}}}bool ΘΠ(IMyTerminalBlock Χ){if(Χ is IMyCargoContainer) return true;if(Χ is IMyShipDrill) return true;if(Χ is IMyShipGrinder) return true;if(Χ is IMyShipConnector){if(((IMyShipConnector) Χ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΓΑ(Χ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕΣ(){if(Ω==null) return;ǖ=null;float ΒΨ=0,ΒΣ=0,ΒΩ=0,ΒΤ=0,ΒΪ=0,ΒΥ=0;List<IMyTerminalBlock>ΔЇ=Ǟ(ω,pamTag,true);bool Ις=ΔЇ.Count==0;if(ΔЇ.Count>0) ǖ=ΔЇ[0];else if(ω.Count>0) ǖ=ω[0];int Ȝ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward){if(Ις){Ћ=2;Λ="Mining direction is unclear!";return;}continue;}Ȝ++;Vector3 Й=ΒЋ(Ω,ω[i].GetPosition());if(i==0){ΒΨ=Й.X;ΒΣ=Й.X;ΒΩ=Й.Y;ΒΤ=Й.Y;ΒΪ=Й.Z;ΒΥ=Й.Z;}ΒΣ=Math.Max(Й.X,ΒΣ);ΒΨ=Math.Min(Й.X,ΒΨ);ΒΤ=Math.Max(Й.Y,ΒΤ);ΒΩ=Math.Min(Й.Y,ΒΩ);
ΒΥ=Math.Max(Й.Z,ΒΥ);ΒΪ=Math.Min(Й.Z,ΒΪ);}ȍ=(ΒΣ-ΒΨ)*(1-Ő)+drillRadius*2;Ǵ=(ΒΤ-ΒΩ)*(1-ĝ)+drillRadius*2;if(ǖ!=null&&ǖ.WorldMatrix.Forward==Ω.WorldMatrix.Down)
Ǵ=(ΒΥ-ΒΪ)*(1-ĝ)+drillRadius*2;}List<T>Ǟ<T>(List<T>Ј,String ǲ,bool ΒΕ){List<T>έ=new List<T>();for(int i=0;i<Ј.Count;i++)
if(ΓΑ(Ј[i],ǲ,ΒΕ)) έ.Add(Ј[i]);return έ;}bool ΓΑ<T>(T Χ,String ǲ,bool ΒΕ){IMyTerminalBlock b=(IMyTerminalBlock) Χ;if(ΒΕ&&b.CustomName.ToUpper().Contains(ǲ.ToUpper())) return true;if(!ΒΕ&&b.BlockDefinition.ToString().ToUpper().Contains(ǲ.ToUpper())) return true;return false;}void ǎ<T>(List<T>Ј,bool З)
{for(int i=0;i<Ј.Count;i++)
ΕЃ((IMyTerminalBlock) Ј[i],З);}void ΕЃ(IMyTerminalBlock Χ,bool З){if(Χ==null) return;String ΘЇ=З?"OnOff_On":"OnOff_Off";var a=Χ.GetActionWithName(ΘЇ);a.Apply(Χ);}void Ǹ(bool З){ΔЃ=З;if(!Ō) return;ǎ(Βγ,З);}void Γζ(ChargeMode Њ){for(int i=0;i<Ė.Count;i++) Ė[i].ChargeMode=Њ;}void Γθ(bool ΙЎ)
{for(int i=0;i<ǒ.Count;i++) ǒ[i].Stockpile=ΙЎ;}void Ɵ(List<IMyLandingGear>ΔΙ,bool ΖЌ){for(int i=0;i<ΔΙ.Count;i++){if(ΖЌ) ΔΙ[i].Lock();if(!ΖЌ) ΔΙ[i].Unlock();}}void ΕЄ(bool З){if(Ζα==З) return;List<IMyShipController>ΑЄ=new List<IMyShipController>();χ.GetBlocksOfType(ΑЄ,џ);if(ΑЄ.Count==0) return;
for(int i=0;i<ΑЄ.Count;i++)
ΑЄ[i].DampenersOverride=З;Ζα=З;}IMyShipConnector Ъ(MyShipConnectorStatus ќ){for(int i=0;i<ѓ.Count;i++){if(!ȓ(ѓ[i],true)) continue;if(ѓ[i].Status==ќ) return ѓ[i];}return null;}void ı(bool ΔϏ,bool ΑЊ,float ΙΧ,float ΘЍ){if(ψ==null||ω.Count==0) return;Vector3 ΑЀ=new Vector3();
int Ȝ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǖ.WorldMatrix.Forward) continue;Ȝ++;ΑЀ+=ω[i].GetPosition();}ΑЀ=ΑЀ/Ȝ;Vector3 Ź=ΒЋ(ψ,ΑЀ);ψ.Enabled=true;ψ.ShowOnHUD=ΔϏ;ψ.LeftExtend=(ΑЊ?1:ъ)/2f*ȍ-Ź.X;ψ.RightExtend=(ΑЊ?1:ъ)/2f*ȍ+Ź.X;ψ.TopExtend=(ΑЊ?1:щ)/2f*Ǵ+Ź.Y;
ψ.BottomExtend=(ΑЊ?1:щ)/2f*Ǵ-Ź.Y;ψ.FrontExtend=(ΔϏ?з:ΙΧ)-Ź.Z;ψ.BackExtend=ΔϏ?0:ΘЍ+λ*0.75f+Ź.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void į<T>(List<T>Ј,bool ΙЉ,float ΙΥ,bool Ια){for(int i=0;i<Ј.Count;i++){IMyTextSurface ΑΗ=null;if(Ј[i]is IMyTextSurface) ΑΗ=(IMyTextSurface) Ј[i];if(ΑΗ!=null){ΑΗ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΙЉ) continue;ΑΗ.Font="Debug";if(Ια) continue;ΑΗ.FontSize=ΙΥ;}}}bool Αζ(){Vector3D ο;return this.Ω.TryGetPlanetPosition(out ο);}int ΘΕ(Vector3 ΙА,int ΗΨ){if(ΗΨ==-1) return 0;double Ц=-1;int ȭ=-1;for(int i=Φ.Count-1;i>=0;i--){double І=Vector3.Distance(Φ[i].Ε,ΙА);if(Ц==-1||І<Ц){ȭ=i;Ц=І;}
}return Math.Sign(ȭ-ΗΨ);}bool ΘΡ(Vector3 ο){List<Vector3>Щ=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Щ.Add(Φ[i].Ε);if(Ξ.ζ&&Φ.Count>=1){Vector3 Ţ=new Vector3();Ȓ(Ξ,dockDist*λ,false,out Ţ);if(Vector3.Distance(Ξ.Ε,Φ.First().Ε)<Vector3.Distance(Ξ.Ε,Φ.Last().Ε)){Щ.Insert(0,Ţ);Щ.Insert(0,Ξ.Ε);}
else{Щ.Add(Ţ);Щ.Add(Ξ.Ε);}}if(Κ==Ι.ά){if(Ζ.ζ&&Φ.Count>=1){Vector3 Γφ=new Vector3();Ȓ(Ζ,dockDist*λ,false,out Γφ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Щ.Insert(0,Γφ);Щ.Insert(0,Ζ.Ε);}else{Щ.Add(Γφ);Щ.Add(Ζ.Ε);}}}else{if(α!=Π.ч)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Щ.Insert(0,Ζ.Ε);else Щ.Add(Ζ.Ε);}int ȭ=-1;double Ǿ=-1;for(int i=0;i<Щ.Count;i++){double І=Vector3.Distance(Щ[i],ο);if(І<Ǿ||Ǿ==-1){Ǿ=І;ȭ=i;}}if(Щ.Count==0) return false;double ΓА=Vector3.Distance(Щ[ȭ],ο);double Ιϊ=Vector3.Distance(Щ[Math.Max(0,ȭ-1)],Щ[ȭ])*1.5f;
double Ιρ=Vector3.Distance(Щ[Math.Min(Щ.Count-1,ȭ+1)],Щ[ȭ])*1.5f;return ΓА<Ιϊ||ΓА<Ιρ||ΓА<laneOffset*λ*1.5f;}static float Б(float a,float b){if(b==0) return 0;return a/b;}Vector3 Θα(Vector3 v,int ΓЌ){return new Vector3(Math.Round(v.X,ΓЌ),Math.Round(v.Y,ΓЌ),Math.Round(v.Z,ΓЌ));
}float Ŕ(Vector3 a,Vector3 b){if(a==b) return 0;float ΙΡ=(a*b).Sum;float ΖЇ=a.Length();float ΖЈ=b.Length();if(ΖЇ==0||ΖЈ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΙΡ/(ΖЇ*ΖЈ)));if(float.IsNaN(έ)) return 0;return έ;}float ĩ(Vector3 v,Vector3 Βέ,float ΙЊ){float Š=Ŕ(v,Βέ);
if(ΙЊ>0f) Š*=-1;if(Š>-90f) return Š-90f;else return 180f-(-Š-90f);}double ΕΪ(float ΙΚ){return(Math.PI/180)*ΙΚ;}bool ΓΔ(double Ιλ,double value,double Ŷ){return(value>=Ιλ&&value<=Ŷ);}Vector3 Θτ(IMyTerminalBlock Χ,Vector3 Й){return Vector3D.Transform(Й,Χ.WorldMatrix);
}Vector3 ΒЋ(IMyTerminalBlock Χ,Vector3 ΚΩ){return п(Χ,ΚΩ-Χ.GetPosition());}Vector3 п(IMyTerminalBlock Χ,Vector3 ΕΚ){return Vector3D.TransformNormal(ΕΚ,MatrixD.Transpose(Χ.WorldMatrix));}Vector3 İ(Vector3 Ȃ,Vector3 Ќ,Vector3 ΕΚ){MatrixD Δθ=MatrixD.CreateFromDir(Ȃ,Ќ);return Vector3D.TransformNormal(ΕΚ,MatrixD.Transpose(Δθ));
}Vector3 Γπ(Vector3 Ȃ,Vector3 Ќ,Vector3 Й){MatrixD Δθ=MatrixD.CreateFromDir(Ȃ,Ќ);return Vector3D.Transform(Й,Δθ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ο(τ,0)),float.Parse(Ο(τ,1)),float.Parse(Ο(τ,2)));
}String ΘЅ(Ϊ Ϋ){String М=":";String έ=σ(Ϋ.Ε)+М+σ(Ϋ.δ)+М+
σ(Ϋ.γ)+М+σ(Ϋ.Ѐ)+М+σ(Ϋ.φ);for(int i=0;i<Ϋ.ǳ.Length;i++)
έ+=М+Math.Round(Ϋ.ǳ[i],3);return έ;}Ϊ Θπ(String s){String[]τ=s.Split(':');Ϊ Ϋ=new Ϊ(
ς(Ο(τ,0)),ς(Ο(τ,1)),
ς(Ο(τ,2)),ς(Ο(τ,3)),
ς(Ο(τ,4)));int Α=5;List<float>Ζο=new List<float>();while(Α<τ.Length){String Βν=Ο(τ,Α);float f=0;if(!float.TryParse(Βν,out f)) break;Ζο.Add(f);Α++;}Ϋ.ǳ=Ζο.ToArray();return Ϋ;}System.Text.StringBuilder Ƃ=new System.Text.StringBuilder();void Υ<T>(T value,bool Ιπ){if(Ιπ) Ƃ.Append('\n');Ƃ.Append(value);}
void Υ<T>(T value){Υ(value,true);}String Αέ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Αχ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ο(String[]ΘЋ,int Α){String Βν=ΘЋ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Βν)) return "";
return Βν;}void Save(){if(Βί||Κ==Ι.ё){Storage="";return;}Storage=Βω();}String Βω(){Ƃ.Clear();Ƃ.Append(DATAREV+";");Υ(σ(Βή),false);Υ(σ(Ξ.δ));Υ(σ(Ξ.Ѐ));Υ(σ(Ξ.γ));Υ(σ(Ξ.φ));Υ(σ(Ξ.Ε));Υ(σ(Ξ.и));Υ(Ξ.ζ);Υ(σ(Ζ.Ε));Υ(σ(Ζ.φ));Υ(σ(Ζ.δ));Υ(σ(Ζ.γ));Υ(σ(Ζ.Ѐ));Υ(σ(Ζ.и));Υ(Ζ.ζ);Υ(σ(ǭ));Υ(σ(ă));Υ(";");
Υ((int) Κ,false);Υ((int) α);Υ((int) ї);Υ(Ȅ);Υ(ј);Υ(к);Υ(љ);Υ(ǹ);Υ(ċ);Υ(Ċ);if(Κ==Ι.ά){Υ((int) Ѓ.Н);Υ(Ѓ.Ǧ);Υ(Ѓ.ě);Υ(Αέ(Ѓ.ǧ));Υ(Αέ(Ѓ.Ǫ));Υ((int) Є.Н);Υ(Є.Ǧ);Υ(Є.ě);Υ(Αέ(Є.ǧ));Υ(Αέ(Є.Ǫ));}else{Υ((int) Ȉ);Υ((int) ь);Υ((int) Ю);Υ((int) є);Υ((int) ľ);Υ(ĥ);Υ(Ō);Υ(ů);Υ(ũ);Υ(ъ);Υ(щ);Υ(з);Υ(Ɛ);Υ(Ə);
Υ(Ő);Υ(ĝ);Υ(ξ);Υ(И);Υ(ώ);Υ(ό);Υ(ā);Υ(Ь);Υ(ŉ);Υ(Ł);}Υ(";");for(int i=0;i<Ȣ.Count;i++)
Υ((i>0?"|":"")+Ȣ[i],false);Υ(";");for(int i=0;i<Φ.Count;i++)
Υ(ΘЅ(Φ[i]),i>0);Υ(";",false);Υ(й+"|"+а,false);Υ(";",false);Υ(ΗЉ(),false);String Τ=Ƃ.ToString();Ƃ.Clear();return Τ;}ђ Θΰ(){return Αο(Storage);}ђ Αο(String Τ){if(Τ=="") return ђ.ΘΩ;String[]Ŋ=Τ.Split(';');if(Ο(Ŋ,0)!=DATAREV) return ђ.ΓΡ;int Α=0;try{String[]Ν=Ο(Ŋ,1).Split('\n');
Βή=ς(Ο(Ν,Α++));Ξ.δ=ς(Ο(Ν,Α++));Ξ.Ѐ=ς(Ο(Ν,Α++));Ξ.γ=ς(Ο(Ν,Α++));Ξ.φ=ς(Ο(Ν,Α++));Ξ.Ε=ς(Ο(Ν,Α++));Ξ.и=ς(Ο(Ν,Α++));Ξ.ζ=bool.Parse(Ο(Ν,Α++));Ζ.Ε=ς(Ο(Ν,Α++));Ζ.φ=ς(Ο(Ν,Α++));Ζ.δ=ς(Ο(Ν,Α++));Ζ.γ=ς(Ο(Ν,Α++));Ζ.Ѐ=ς(Ο(Ν,Α++));Ζ.и=ς(Ο(Ν,Α++));Ζ.ζ=bool.Parse(Ο(Ν,Α++));
ǭ=ς(Ο(Ν,Α++));ă=ς(Ο(Ν,Α++));Ν=Ο(Ŋ,2).Split('\n');Α=0;Κ=(Ι) int.Parse(Ο(Ν,Α++));α=(Π) int.Parse(Ο(Ν,Α++));ї=(Π) int.Parse(Ο(Ν,Α++));Ȅ=int.Parse(Ο(Ν,Α++));ј=int.Parse(Ο(Ν,Α++));к=int.Parse(Ο(Ν,Α++));љ=int.Parse(Ο(Ν,Α++));ǹ=float.Parse(Ο(Ν,Α++));ċ=bool.Parse(Ο(Ν,Α++));
Ċ=bool.Parse(Ο(Ν,Α++));if(Κ==Ι.ά){Ѓ.Н=(π) int.Parse(Ο(Ν,Α++));Ѓ.Ǧ=float.Parse(Ο(Ν,Α++));Ѓ.ě=float.Parse(Ο(Ν,Α++));Ѓ.ǧ=Αχ(Ο(Ν,Α++));Ѓ.Ǫ=Αχ(Ο(Ν,Α++));Є.Н=(π) int.Parse(Ο(Ν,Α++));Є.Ǧ=float.Parse(Ο(Ν,Α++));Є.ě=float.Parse(Ο(Ν,Α++));Є.ǧ=Αχ(Ο(Ν,Α++));Є.Ǫ=Αχ(Ο(Ν,Α++));}else
{Ȉ=(Ǐ) int.Parse(Ο(Ν,Α++));ь=(х) int.Parse(Ο(Ν,Α++));Ю=(Ѝ) int.Parse(Ο(Ν,Α++));є=(ў) int.Parse(Ο(Ν,Α++));ľ=(Ǐ) int.Parse(Ο(Ν,Α++));ĥ=bool.Parse(Ο(Ν,Α++));Ō=bool.Parse(Ο(Ν,Α++));ů=bool.Parse(Ο(Ν,Α++));ũ=bool.Parse(Ο(Ν,Α++));ъ=int.Parse(Ο(Ν,Α++));щ=int.Parse(Ο(Ν,Α++));з=int.Parse(Ο(Ν,Α++));
Ɛ=float.Parse(Ο(Ν,Α++));Ə=float.Parse(Ο(Ν,Α++));Ő=float.Parse(Ο(Ν,Α++));ĝ=float.Parse(Ο(Ν,Α++));ξ=int.Parse(Ο(Ν,Α++));И=int.Parse(Ο(Ν,Α++));ώ=int.Parse(Ο(Ν,Α++));ό=int.Parse(Ο(Ν,Α++));ā=int.Parse(Ο(Ν,Α++));Ь=int.Parse(Ο(Ν,Α++));ŉ=int.Parse(Ο(Ν,Α++));Ł=float.Parse(Ο(Ν,Α++));
}Ν=Ο(Ŋ,3).Replace("\n","").Split('|');Ȣ=Ν.ToList();Ν=Ο(Ŋ,4).Split('\n');Φ.Clear();if(Ν.Count()>=1&&Ν[0]!="")
for(int i=0;i<Ν.Length;i++)
Φ.Add(Θπ(Ο(Ν,i)));Ν=Ο(Ŋ,5).Split('|');й=Ο(Ν,0);if(!int.TryParse(Ο(Ν,1),out а)) а=-1;if(й.Length!=ξ*И){й="";а=-1;}ΗЈ(Ο(Ŋ,6));}catch{return ђ.Εέ;}ƃ=ї;Ϗ();return ђ.Αυ;}const int ΕΠ=64000;IMyTerminalBlock Εή(){List<IMyTerminalBlock>ť=new List<IMyTerminalBlock>();χ.GetBlocksOfType(ť,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ť.Count>0?ť[0]:null;}void ΗЍ(){IMyTerminalBlock Χ=Εή();if(Χ==null){Λ="Export: name a block "+dataTag;return;}String Τ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Βω();if(Τ.Length>ΕΠ){Λ="Export too large: "+Τ.Length+" chars, max "+ΕΠ;return;}Χ.CustomData=Τ;Λ="Exported "+Φ.Count+" waypoints";}void ΘΝ(bool ΔΪ){IMyTerminalBlock Χ=Εή();if(Χ==null){Λ="Import: name a block "+dataTag;return;}
ΘΞ(Χ.CustomData,ΔΪ);}void ΘΞ(String Τ,bool ΔΪ){Τ=Τ.Replace("\r","");if(Τ.StartsWith("//")){int ΗΗ=Τ.IndexOf('\n');Τ=ΗΗ<0?"":Τ.Substring(ΗΗ+1);}Τ=Τ.Trim();if(Τ==""){Λ="Import: no PAM data in "+dataTag;return;}Ϗ();String ΖΜ=Βω();ђ έ=Αο(Τ);if(έ==ђ.Αυ&&ΔΪ){Ϊ ű=new Ϊ(Ζ);
Vector3 Ιξ=ǭ,Ιν=ă;int w=ъ,h=щ,d=з;Ǐ ΔЄ=Ȉ;ў ΓЍ=є;float Ιυ=Ő,Ιτ=ĝ;Αο(ΖΜ);Ζ=ű;ǭ=Ιξ;ă=Ιν;ъ=w;щ=h;з=d;Ȉ=ΔЄ;є=ΓЍ;Ő=Ιυ;ĝ=Ιτ;α=Π.ś;ƣ(true,true);α=Π.ś;ї=Π.ś;Λ="Imported job, run CONT";return;}if(έ==ђ.Αυ){Ǌ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}Αο(ΖΜ);Λ=έ==ђ.ΓΡ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΕЅ(){ķ.Clear();for(int i=У.Count-1;i>=0;i--){String Τ=У[i].CustomData.ToUpper();bool ΔΨ=false;if(Τ=="INSTRUCTIONS"){ΔΨ=true;ΑЉ=true;}if(Τ=="DEBUG") ΔΨ=true;if(ΔΨ){ķ.Add(У[i]);У.RemoveAt(i);}
}į(ķ,false,1,false);}void ΕІ(List<IMyTerminalBlock>ť){Ȋ.Clear();for(int i=0;i<ť.Count;i++){IMyTerminalBlock Χ=ť[i];try{String ΗΞ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Χ.CustomName.IndexOf(ΗΞ);int ƈ=-1;if(Α<0||!int.TryParse(Χ.CustomName.Substring(Α+ΗΞ.Length,1),out ƈ)) continue;
if(ƈ==-1) continue;ƈ--;IMyTextSurfaceProvider ΗΠ=(IMyTextSurfaceProvider) Χ;if(ƈ<ΗΠ.SurfaceCount&&ƈ>=0)
Ȋ.Add(ΗΠ.GetSurface(ƈ));}catch{}}}String ΒΦ="";void ΕЀ(){String Δη="";String ƶ="";ΒΦ=ΕΟ(false);Δη+=ΒΦ;ƶ+=ΒΦ;ƶ+=ΘΓ();for(int i=0;i<У.Count;i++) У[i].WriteText(Δη);for(int i=0;i<Ȋ.Count;i++) Ȋ[i].WriteText(Δη);Echo(ƶ);for(int i=0;i<ķ.Count;i++){IMyTextPanel т=ķ[i];
String Τ=т.CustomData.ToUpper();if(Τ=="DEBUG") т.WriteText(""+"\n"+"");if(Τ=="INSTRUCTIONS") т.WriteText(Εε());}}string Εε(){String Γ="";try{float ΖЏ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ť*ΖЏ)+" / "+(int)(ŷ*ΖЏ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ť*100f,1)+"% / "+Math.Round(ŷ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ʃ,2)+"ms / "+Math.Round(Δλ,2)+"ms \n";}catch{}for(int i=0;i<Ą.Count;i++)
Γ+=""+Ą.Keys.ElementAt(i)+" = "+Math.Round(Ą.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ą.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΘΛ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";į(У,setLCDFontAndSize,0.9f,false);į(Ȋ,setLCDFontAndSize,0.9f,true);for(int i=0;i<У.Count;i++) У[i].WriteText(Γ+Ā.ToString());for(int i=0;i<Ȋ.Count;i++) Ȋ[i].WriteText(Γ+Ā.ToString());}void ȕ(){if(!ΑЉ) return;try{Δψ=Runtime.CurrentInstructionCount;}catch{}
}void ȑ(String ή){if(!ΑЉ) return;if(Δψ==0) return;try{float Βλ=(Runtime.CurrentInstructionCount-Δψ)/Runtime.MaxInstructionCount*100;if(!Ą.ContainsKey(ή))
Ą.Add(ή,new float[]{Βλ,Βλ});else{Ą[ή][0]=Βλ;Ą[ή][1]=Math.Max(Βλ,Ą[ή][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ηυ(String ȁ){if(!Ŧ) return;String М=""+BROADCAST_SEP;String η=VERSION+М;η+=Ƒ(Ɔ)+М;
η+=(int) Κ+М;η+=ŗ(β.X)+""+М;η+=ŗ(β.Y)+М;η+=ŗ(β.Z)+М;if(Κ!=Ι.ά)
η+=ΒЊ(ї)+(ї==Π.Д?" "+Math.Round(ǃ,1)+"%":"")+М;else η+=ΒЊ(ї)+М;η+=Ƒ(Λ)+М;η+=Љ+""+М;η+=Ƒ(ΒΦ)+М;η+=ǿ+М;η+=ȅ+М;for(int i=0;i<ǩ.Count;i++){if(ǩ[i].Ŵ!=Х.ř) continue;η+=Ƒ(ǩ[i].ή)+"/"+Ƒ(ǩ[i].υ)+"/"+ǩ[i].Ѕ+М;}Θκ(η,ȁ);}String Ƒ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Θκ(String η,String ȁ){Γδ(η,BROADCAST_SELF_ID,ȁ);}void Γδ(String η,String μ,String ȁ){try{if(!Ŧ) return;η=BROADCAST_TAG+BROADCAST_SEP+Εα()+BROADCAST_SEP+μ+BROADCAST_SEP+ΓЂ+BROADCAST_SEP+ȁ+BROADCAST_SEP+η;this.IGC.SendBroadcastMessage(BROADCAST_TAG,η);}catch(Exception e){Ā=e;}
}String Εα(){if(Κ!=Ι.ё) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΓΙ(String η){return η.StartsWith(BROADCAST_TAG);}bool б(ref String Τ,out string Ƹ,Char М){int Α=Τ.IndexOf(М);Ƹ="";if(Α<0) return false;Ƹ=Τ.Substring(0,Α);Τ=Τ.Remove(0,Α+1);return true;}bool ΕϏ(ref String η,out String ȇ,out String ȁ)
{ȇ="";ȁ="";if(!Ŧ) return false;String ǲ="";if(!б(ref η,out ǲ,BROADCAST_SEP)||!ΓΙ(ǲ)) return false;if(!б(ref η,out ȇ,BROADCAST_SEP)) return false;if(!б(ref η,out ǲ,BROADCAST_SEP)||(ǲ!=Εα()&&(ǲ!="*"&&Κ!=Ι.ё))) return false;if(!б(ref η,out ǲ,BROADCAST_SEP)||(ǲ!=ΓЂ)) return false;
if(!б(ref η,out ȁ,BROADCAST_SEP)) return false;return true;}void Επ(){bool Ʈ=true;if(Ζρ){Ζρ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ƽ=(Κ!=Ι.ё?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ƽ)
Ερ(ƽ);String[]Ν=Me.CustomData.Split('\n');Ŧ=bool.Parse(Αξ(Αί(Ν,"Enable_Broadcast",ref Ʈ)));bool ΕΘ=false;bool Ζε=true;if(Ŧ){if(Κ!=Ι.ё)
Ɔ=Αξ(Αί(Ν,"Ship_Name",ref Ʈ)).Replace(BROADCAST_SELF_ID,"");ΓЂ=Αξ(Αί(Ν,"Broadcast_Channel",ref Ʈ)).ToLower();Ζε=false;if(Ǥ==null){Ǥ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǥ.SetMessageCallback("");}List<IMyRadioAntenna>š=new List<IMyRadioAntenna>();χ.GetBlocksOfType(š);bool Γυ=false;
for(int i=0;i<š.Count;i++)
if(š[i].EnableBroadcasting&&š[i].Enabled){Γυ=true;break;}if(š.Count==0) Λ="No Antenna found";else if(!Γυ) Λ="Antenna not ready";ΕΘ=š.Count==0||!Γυ;if(ΖΕ&&!ΕΘ&&Κ!=Ι.ё) Λ="Antenna ok";}else if(Κ==Ι.ё)
Λ="Offline - Enable in PB custom data";ΖΕ=ΕΘ;if(Ζε){if(Ǥ!=null) this.IGC.DisableBroadcastListener(Ǥ);Ǥ=null;}}catch{Ʈ=false;}if(!Ʈ){Λ="Reset custom data";Ερ(ƽ);}}void Ερ(String ƽ){String Τ=ƽ+"\n\n"+"Enable_Broadcast="+(Κ==Ι.ё?"true":"false")+" \n";Τ+=Κ!=Ι.ё?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Τ+"Broadcast_Channel=#default";}String Αξ(String ŀ){int Α=ŀ.IndexOf("//");if(Α!=-1) ŀ=ŀ.Substring(0,Α);String[]τ=ŀ.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αί(String[]Ν,String Ų,ref bool ń){foreach(String ŀ in Ν)
if(ŀ.StartsWith(Ų)) return ŀ;ń=false;return "";}void ΗЅ(string Р){if(ȯ) Ы=new List<К>();String ȇ="";String ȁ="";if(!ΓΙ(Р)) Θή(Р);bool ΗΥ=false;int Δξ=50;while(Ǥ!=null&&Ǥ.HasPendingMessage&&Δξ-->0){MyIGCMessage η=Ǥ.AcceptMessage();String Τ=η.Data as String;if(Τ==null) continue;
if(ΕϏ(ref Τ,out ȇ,out ȁ)&&ȇ!=""&&ȇ!=BROADCAST_SELF_ID){К Δ=ΒЈ(ȇ,"");if(Δ==null){Δ=new К(ȇ);Ы.Add(Δ);}Δ.Ŝ(ȁ,false,false,true);ΘЀ(Δ,Τ);ΗΥ=true;}}if(ΗΥ)
Ы.Sort(delegate(К a,К b){if(a.ή==null&&b.ή==null) return 0;else if(a.ή==null) return-1;else if(b.ή==null) return 1;else return a.ή.CompareTo(b.ή);});if(ȳ||ȯ){if(ΑΒ<=0||ȯ){Λ="";ȯ=false;Θθ();Επ();Δί=DateTime.Now;}}if(ǁ) Θω();Θχ();}void ΘЀ(К Δ,String Τ){Δ.Δά=DateTime.Now;
String Ƹ="";б(ref Τ,out Δ.Βο,BROADCAST_SEP);б(ref Τ,out Δ.ή,BROADCAST_SEP);if(Δ.Βο!=VERSION) return;String ǉ="";String[]ž=new string[3];б(ref Τ,out ǉ,BROADCAST_SEP);б(ref Τ,out ž[0],BROADCAST_SEP);б(ref Τ,out ž[1],BROADCAST_SEP);б(ref Τ,out ž[2],BROADCAST_SEP);б(ref Τ,out Δ.ΔЅ,BROADCAST_SEP);
б(ref Τ,out Δ.Λ,BROADCAST_SEP);б(ref Τ,out Ƹ,BROADCAST_SEP);б(ref Τ,out Δ.ΒΧ,BROADCAST_SEP);String Ηξ="",ΗΑ="";б(ref Τ,out Ηξ,BROADCAST_SEP);б(ref Τ,out ΗΑ,BROADCAST_SEP);Δ.Ї.Clear();while(true){String ΖЂ;if(!б(ref Τ,out ΖЂ,BROADCAST_SEP)) break;String[]ΒΛ=ΖЂ.Split('/');if(ΒΛ.Count()<3) continue;
int Ѕ=0;if(!int.TryParse(ΒΛ[2],out Ѕ)) continue;Δ.Ї.Add(new ж(ΒΛ[0],ΒΛ[1],Ѕ,Х.ř));}int ΗΔ=0;if(!int.TryParse(Ƹ,out Δ.Љ)) Δ.Љ=0;if(!int.TryParse(ǉ,out ΗΔ)) Δ.Κ=Ι.Ĕ;if(!float.TryParse(ž[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ž[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(ž[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(Ηξ,out Δ.ǿ)) Δ.ǿ=0;if(!float.TryParse(ΗΑ,out Δ.ȅ)) Δ.ȅ=0;Δ.Κ=(Ι) ΗΔ;Δ.ΓБ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΔБ=0;for(int i=0;i<Δ.Ї.Count();i++) Δ.ΔБ+=Δ.Ї[i].Ѕ;}К ΒЈ(String ε,String ή){ε=ε.ToUpper();ή=ή.ToUpper();for(int i=0;i<Ы.Count;i++)
{if(ε!=""&&Ы[i].ε.ToUpper()==ε) return Ы[i];if(ή!=""&&Ы[i].ή!=null&&Ы[i].ή.ToUpper()==ή) return Ы[i];}return null;}void Ǡ(К Δ,String Ж){if(Δ==null){for(int i=0;i<Ы.Count;i++)
Ы[i].Ŝ(Ж,true,false,false);Γδ(Ж,"*",Ж);}else{Δ.Ŝ(Ж,true,false,false);Γδ(Ж,Δ.ε,Ж);}}void Θή(string Р){if(Р=="") return;var τ=Р.ToUpper().Split(' ');τ.DefaultIfEmpty("");var Ж=τ.ElementAtOrDefault(0);var ȗ=τ.ElementAtOrDefault(1);String ȟ="Invalid argument: "+Р;if(ϋ==Ρ.Ơ){if(Г!=null)
{switch(Ж){case "UP":{if(Љ<2) break;if(Г.Љ==0){Љ=1;return;}Ǡ(Г,"UP");return;}case "DOWN":{if(Љ<1) break;if(Љ==1){Ǡ(Г,"MRES");break;}Ǡ(Г,"DOWN");break;}case "APPLY":{if(Љ<2) break;Ǡ(Г,"APPLY");return;}}}}switch(Ж){case "UP":this.ΓΞ(false);return;case "DOWN":this.ΓΝ(false);return;
case "APPLY":this.ŕ(true);return;}switch(Ж){case "CLEAR":ΗЀ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȗ)){Λ="Usage: SENDALL <cmd>";return;}Ǡ(null,ȗ);return;case "SEND":
if(Р.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Θλ(Р.Remove(0,"SEND".Length+1));return;}Λ=ȟ;}void Θλ(String Ƨ){if(Ƨ=="") return;var τ=Ƨ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ɔ=τ.ElementAtOrDefault(0).Trim();
К Δ=ΒЈ("",Ɔ);if(Δ!=null) Ǡ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ɔ;}void ΗЀ(){Ы.Clear();Г=null;θ(Ρ.Main);Љ=0;}void Θθ(){List<IMyTerminalBlock>ΑΘ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(У,џ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑΘ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);У=Ǟ(У,pamTag,true);ΕІ(ΑΘ);ΕЅ();Θι();į(У,setLCDFontAndSize,1.15f,false);į(Ȋ,setLCDFontAndSize,1.15f,true);String Ιέ=Εζ(false);String ΗΘ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock т in У){if(т.CustomData==""){т.CustomData=Εζ(true);continue;}
if(!т.CustomData.Contains(Ιέ))
if(!т.CustomData.Contains(ΗΘ)) т.CustomData=ΗΘ+т.CustomData;}}void Θχ(){String ƶ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȋ.Count;i++)
Ȋ[i].WriteText(ΕΞ(П.Εω,"0",false));for(int i=0;i<У.Count;i++){П Њ=П.Ĕ;String ǯ="";ΘΪ(У[i],out Њ,out ǯ);У[i].WriteText(ΕΞ(Њ,ǯ,i==0));}Echo(ƶ);for(int i=0;i<ķ.Count;i++){IMyTextPanel т=ķ[i];String Τ=т.CustomData.ToUpper();if(Τ=="DEBUG") т.WriteText(""+"\n"+"");if(Τ=="INSTRUCTIONS") т.WriteText(Εε());
}}String ŕ(bool Θ){int ΙΣ=0;return ŕ(Θ,0,ref ΙΣ,false,1);}String ŕ(bool Θ,int Ιχ,ref int ΙЅ,bool ΖΩ,int ǋ){String Γ="";String ƅ="——————————————————\n";String ΚΒ="--------------------------------------------\n";if(Г==null||ϋ==Ρ.Main||ϋ==Ρ.Γε)
Γ+="[PAM]-Controller | "+Ы.Count+" Connected ships"+"\n"+ΗБ();else Γ+=Ει(Г)+"\n";Γ+=ƅ;int Α=0;int Η=Љ;ł=0;if(Λ!=""){Μ(ref Γ,Η,Α++,Θ,Λ);}else if(ϋ==Ρ.Main){if(Г==null&&Ы.Count>=1) Г=Ы[0];String Ј="";if(Μ(ref Ј,Η,Α++,Θ," Send command to all")) θ(Ρ.Γε);int Ƅ=0;
for(int s=0;s<Ы.Count;s++){if(ΖΩ) Ј+="\n";К Δ=Ы[s];if(Η==Α||Η==Α+1) Г=Δ;if(Α==Η) Ƅ=Ј.Split('\n').Length-1;if(Μ(ref Ј,Η,Α++,Θ," "+Ει(Δ))){Г=Δ;θ(Ρ.Ơ);Г.Ŝ("",false,true,false);}if(Α==Η) Ƅ=Ј.Split('\n').Length-1;if(Μ(ref Ј,Η,Α++,Θ," "+(ΖΩ?"":"  ")+ΘΖ(Δ)))
{Г=Δ;θ(Ρ.ΕЇ);}}int Ǝ=ǋ-2;Ǝ+=Math.Max(0,(Ιχ-1))*ǋ;Γ+=Γγ(Ǝ,Ј,Ƅ,ref ΙЅ);}else if(ϋ==Ρ.Ας||ϋ==Ρ.Γε){Ρ ΘЌ=ϋ==Ρ.Ας?Ρ.Ơ:Ρ.Main;К μ=null;if(ϋ==Ρ.Ας) μ=Г;if(Μ(ref Γ,Η,Α++,Θ," Back")){θ(ΘЌ);}if(Μ(ref Γ,Η,Α++,Θ," [Stop] "+Ř(μ,"STOP"))){Ǡ(μ,"STOP");}if(Μ(ref Γ,Η,Α++,Θ," [Continue] "+Ř(μ,"CONT"))){Ǡ(μ,"CONT");}
if(Μ(ref Γ,Η,Α++,Θ," [Move home] "+Ř(μ,"HOMEPOS"))){Ǡ(μ,"HOMEPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Move to job] "+Ř(μ,"JOBPOS"))){Ǡ(μ,"JOBPOS");}if(Μ(ref Γ,Η,Α++,Θ," [Full simulation] "+Ř(μ,"FULL"))){Ǡ(μ,"FULL");}if(μ!=null&&μ.Κ==Ι.ά)
if(Μ(ref Γ,Η,Α++,Θ," [Undock] "+Ř(μ,"UNDOCK"))){Ǡ(μ,"UNDOCK");}}else if(ϋ==Ρ.Ơ){String Γό="";if(Μ(ref Γό,Η,Α++,Θ," Back")){θ(Ρ.Main);}Γ+=Γό.Substring(0,Γό.Length-1).PadRight(36,' ');if(Μ(ref Γ,Η,Α++,Θ," Send cmd")){θ(Ρ.Ας);}if(!Г.Εχ()&&!Г.ΓΖ()) ł++;if(Г.ΓΖ()) Γ+="No answer received...";
else Γ+=Ş(Г,Г.ΒΧ);}else if(ϋ==Ρ.ΕЇ){if(Μ(ref Γ,Η,Α++,Θ," Back")){θ(Ρ.Main);}Γ+=ΚΒ;Γ+=Ş(Г,Εθ(Г));}if(!Δν) Љ=Math.Min(ł-1,Љ);return Γ;}String Ει(К Δ){if(Δ.Βο!=VERSION) return Δ.ή+": Different version!";return Δ.ή+": "+Ş(Δ,Δ.ΔЅ)+" "+Δ.ΓБ+"m";}String ΘΖ(К Δ){String s=Γί("",Б(Δ.ǿ,Δ.ȅ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ї.Count;i++){if(i>=5) break;ж ύ=Δ.Ї[i];s+=ΕА(Βϊ(ΘΔ(ύ)),3)+" "+Εί(ύ.Ѕ)+" ";}return s;}String ΘΔ(ж ύ){if(ύ.υ.ToUpper()=="ORE"||ύ.υ.ToUpper()=="INGOT"){String ΖΧ=ΘΒ(ύ.ή.ToUpper());if(ΖΧ!="") return ΖΧ;}return ύ.ή;}String Ş(К Δ,String Γ){if(Δ.Εχ()) return "No signal...("+Αβ((int)(DateTime.Now-Δ.Δά).TotalSeconds)+")";
return Γ;}String Εζ(bool ΚΨ){String Γ="";if(ΚΨ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΘΪ(IMyTerminalBlock т,out П Њ,out String ǯ){bool ń=true;String ǉ=Αξ(Αί(т.CustomData.Split('\n'),"mode",ref ń)).ToUpper();String[]τ=ǉ.Split(':');String ǈ=τ.Length>0?τ[0].Trim():"";ǯ=τ.Length>1?τ[1].Trim():"";
Њ=П.Ĕ;if(ǈ=="MAIN") Њ=П.Εϊ;else if(ǈ=="MAINX") Њ=П.Εϋ;else if(ǈ=="MENU") Њ=П.ΓΜ;else if(ǈ=="INVENTORY") Њ=П.ΓΕ;else if(ǈ=="DEBUG") Њ=П.ΒϏ;if(Њ==П.Ĕ) ǯ=ǉ;}String ΕΞ(П Њ,String ǯ,bool ΙΤ){int ǆ=15;if(ΙΤ){ΖΫ=ΑЂ;ΑЂ=0;Ζά=ΑЃ;ΑЃ=0;}String Γ="";if(Њ==П.Εϊ){int с=0;
Γ=ŕ(false,ΖΫ,ref ΙΕ,true,ǆ);if(!int.TryParse(ǯ,out с)) return Γ;ΑЂ=Math.Max(с,ΑЂ);с--;return Εώ(с,ǆ,Γ);}else if(Њ==П.Εϋ){int с=0;Γ=ŕ(false,Ζά,ref ΙΖ,false,ǆ);if(!int.TryParse(ǯ,out с)) return Γ;ΑЃ=Math.Max(с,ΑЃ);с--;return Εώ(с,ǆ,Γ);}if(Њ==П.Εω){return ŕ(false,0,ref ΙΗ,true,ǆ);
}else if(Њ==П.ΓΕ||Њ==П.ΓΜ||Њ==П.ΒϏ){К Δ=Г;if(ǯ!=""){Δ=ΒЈ("",ǯ);if(Δ==null) return "Unknown ship: "+ǯ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ƅ="—————————————————————\n";String ΔЏ="";String ΓЁ="";if(Њ==П.ΓΜ){ΔЏ="Menu";ΓЁ=Ş(Δ,Δ.ΒΧ);}
else if(Њ==П.ΓΕ){ΔЏ="Inventory";ΓЁ=Ş(Δ,Εθ(Δ));}else if(Њ==П.ΒϏ){return Ş(Δ,ΘΗ(Δ));}Γ+=Δ.ή+" - "+Δ.ΓБ+"m | "+Βϊ(""+ΔЏ)+"\n"+ƅ;Γ+=ΓЁ;return Γ;}return "Unknown command: "+ǯ;}string ΘΗ(К Δ){String s="";s+=Δ.Βο+"\n";s+=Δ.ή+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΔЅ+"\n";s+=Δ.Λ+"\n";s+=Δ.Љ+"\n";s+=Δ.ΒΧ.Length+"\n";s+=Δ.ǿ+"\n";s+=Δ.ȅ+"\n";s+=Δ.Ї.Count()+"\n";return s;}string Εθ(К Δ){String s="";s+=Γί("All",Б(Δ.ǿ,Δ.ȅ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ї.Count();i++){ж ύ=Δ.Ї[i];
s+=Γί(ύ.ή,ύ.Ѕ,Δ.ΔБ,30,8,10)+"\n";}return s;}О ΗЏ(К Δ,bool ш,string Ж){О έ=О.None;if(Δ==null){for(int i=0;i<Ы.Count;i++){О Ё=Ы[i].Ŝ(Ж,false,ш,false);if(Ё==О.Γΰ) έ=Ё;if(Ё==О.ΓΣ) return Ё;if(Ё==О.Ɲ) return Ё;}return έ;}else return Δ.Ŝ(Ж,false,ш,false);}String Ř(К Δ,String Ж)
{О Ё=ΗЏ(Δ,false,Ж);if(Ё==О.Γΰ) return "received!";if(Ё==О.Ɲ) return "pending...";if(Ё==О.ΓΣ) return "no answer!";return "";}String Γγ(int Ǝ,String Γ,int Ƅ,ref int ą){String[]Ν=Γ.Split('\n');if(Ƅ>=ą+Ǝ-1) ą++;ą=Math.Min(Ν.Count()-1-Ǝ,ą);if(Ƅ<ą+1) ą--;ą=Math.Max(0,ą);
String έ="";for(int i=0;i<Ǝ;i++){int ΖЉ=i+ą;if(ΖЉ>=Ν.Count()) break;έ+=Ν[ΖЉ]+"\n";}return έ;}String Εώ(int с,int ǋ,String Γ){String[]Ν=Γ.Split('\n');int ΔЄ=с*ǋ;int Ȯ=(с+1)*ǋ;String έ="";for(int i=ΔЄ;i<Ȯ;i++){if(i>=Ν.Count()) break;έ+=Ν[i]+"\n";}return έ;
}String Βϊ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΕА(String s,int ΖА){if(s=="") return s;if(s.Length>ΖА) s=s.Substring(0,ΖА-1)+".";return s;}string Γί(String Ŀ,float value,float Ŷ,int ΖΝ,int ΖЄ,int Ιφ){float ň=Б(value,Ŷ)*ΖΝ;
String ΑϏ="[";for(int i=0;i<ΖΝ;i++){if(i<=ň) ΑϏ+="|";else ΑϏ+="'";}ΑϏ+="]";return ΑϏ+" "+ΕА(Βϊ(Ŀ),ΖЄ).PadRight(ΖЄ)+"".PadRight(Ιφ)+Εί(value);}String Εί(float Ѕ){if(Ѕ>=1000000) return Math.Round(Ѕ/1000000f,Ѕ/1000000f<100?1:0)+"M";if(Ѕ>=1000) return Math.Round(Ѕ/1000f,Ѕ/1000f<100?1:0)+"K";
return ""+Math.Round(Ѕ);}String Αβ(int ΑΓ){if(ΑΓ>=60*60) return Math.Round(ΑΓ/(60f*60f),1)+" h";if(ΑΓ>=60) return Math.Round(ΑΓ/60f,1)+" min";return ""+ΑΓ+" s";}string ŗ(float v){return Math.Round(v,2)+" ";}string Κί(Vector3 v){return "X"+ŗ(v.X)+"Y"+ŗ(v.Y)+"Z"+ŗ(v.Z);}
List<IMyCameraBlock>ș=new List<IMyCameraBlock>();List<long>ź=new List<long>();bool ż=false;int ΓЇ=0;int ΓЅ=0;DateTime ΖΟ=DateTime.Now;String ΓЀ="";void Θη(){ș.Clear();ź.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ș,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ș.Count;i++){ș[i].EnableRaycast=true;if(!ź.Contains(ș[i].CubeGrid.EntityId)) ź.Add(ș[i].CubeGrid.EntityId);}if(!ź.Contains(Me.CubeGrid.EntityId)) ź.Add(Me.CubeGrid.EntityId);}bool Ετ(Β s){return s==Β.đ||s==Β.н||s==Β.Е||
s==Β.Đ||s==Β.Ŗ||s==Β.Ɣ||s==Β.Αΰ||
(s==Β.Ƕ&&Ǖ!=null);}void ΑΫ(){ż=false;ΓЇ=0;ΓЀ="";}void Θώ(){if(!obstacleBrake||ș.Count==0||!Ετ(Ψ)){ΑΫ();return;}Vector3 ƹ=Ђ+Ĥ;Vector3 ȋ=ƹ-β;float ȉ=ȋ.Length();if(ȉ<1f){ΑΫ();return;}Vector3 ρ=ȋ/ȉ;float ΖЍ=Ć*obstacleLookAhead+λ;Vector3 Ιύ=ƹ+ρ*(λ*0.25f);
for(int n=0;n<ș.Count;n++){ΓЅ=(ΓЅ+1)%ș.Count;IMyCameraBlock ǻ=ș[ΓЅ];if(!ǻ.IsFunctional||!ǻ.IsWorking) continue;Vector3 ΖΡ=ǻ.GetPosition();Vector3 Ȯ=Ιύ;if(Vector3.Distance(ΖΡ,Ȯ)>ΖЍ) Ȯ=ΖΡ+ρ*ΖЍ;if(!ǻ.CanScan(Ȯ)) continue;MyDetectedEntityInfo Ȟ=ǻ.Raycast(Ȯ);bool Ισ=!Ȟ.IsEmpty()&&!ź.Contains(Ȟ.EntityId)&&!ΘΥ(Ȟ.EntityId)&&
Ȟ.Type!=MyDetectedEntityType.FloatingObject;if(Ισ){if(!ż) ΖΟ=DateTime.Now;ż=true;ΓЇ=0;ΓЀ=Ȟ.Type.ToString();}else if(ż&&++ΓЇ>=3) ΑΫ();break;}if(ż&&(DateTime.Now-ΖΟ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΓЀ;}const String ΕЏ="[PAMTFC]";const float Θς=5f;const float ΒЇ=2f;class ē{public long ε;public Vector3 ο,ƍ;public Vector3 ƾ;public float ΓЏ;public String ű="";public int ƿ=-1;public bool Δω;public long ί=0;public int Ʊ=0;public DateTime ΔЌ;}IMyBroadcastListener Βκ=null;
long Ǯ=0;List<ē>э=new List<ē>();Vector3 Ĥ=new Vector3();float Ȏ=-1;Vector3 ΕΑ=new Vector3();bool Βι=false;DateTime ΕΒ=DateTime.Now;const float ΕЎ=15f;const float Θρ=10f;int Ηλ=0;String й="";int а=-1;bool ΑЅ=false;Vector3 Ζέ=new Vector3();String ΓΚ(){if(α==Π.ч||Κ==Ι.ά) return "";
return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+ξ+"|"+И+"|"+(int) ľ+"|"+Math.Round(ȍ,1)+"|"+Math.Round(Ǵ,1);}void ΘЂ(){if(!trafficEnabled) return;if(Βκ==null) Βκ=IGC.RegisterBroadcastListener(ΕЏ);Ǯ=Me.CubeGrid.EntityId;int ƭ=30;while(Βκ.HasPendingMessage&&ƭ-->0){String Τ=Βκ.AcceptMessage().Data as String;if(Τ!=null) Θά(Τ);
}for(int i=э.Count-1;i>=0;i--)
if((DateTime.Now-э[i].ΔЌ).TotalSeconds>Θς) э.RemoveAt(i);if(++Ηλ<3) return;if(Ψ==Β.о&&Ъ(MyShipConnectorStatus.Connected)==null) return;Ηλ=0;IGC.SendBroadcastMessage(ΕЏ,Ǯ+";"+σ(β)+";"+
σ(Ω.GetShipVelocities().LinearVelocity)+";"+λ+";"+ΓΚ()+";"+
(α==Π.Д?а:-1)+";"+й+";"+ΗϏ()+","+Εά()+";"+σ(ΕΑ)+
";"+(Ũ?1:0));}void Θά(String Τ){try{String[]f=Τ.Split(';');if(f.Length<7) return;long ε=long.Parse(f[0]);if(ε==Ǯ) return;ē Δ=null;for(int i=0;i<э.Count;i++) if(э[i].ε==ε) Δ=э[i];if(Δ==null){Δ=new ē();Δ.ε=ε;э.Add(Δ);}Δ.ο=ς(f[1]);Δ.ƍ=ς(f[2]);Δ.ΓЏ=float.Parse(f[3]);
Δ.ű=f[4];Δ.ƿ=int.Parse(f[5]);Δ.ΔЌ=DateTime.Now;if(Δ.ű!=""&&Δ.ű==ΓΚ()) ΘΨ(f[6]);Δ.ί=0;Δ.Ʊ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.ί=long.Parse(d[0]);Δ.Ʊ=int.Parse(d[1]);}Δ.ƾ=f.Length>=9?ς(f[8]):new Vector3();Δ.Δω=f.Length>=10&&f[9]=="1";}catch{}}bool ΘΥ(long ε)
{for(int i=0;i<э.Count;i++) if(э[i].ε==ε) return true;return false;}Vector3 Εη(int Α,int Ιψ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();Ϊ Ϋ=Φ[Α];Vector3 ΚΟ=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*Ιψ;Vector3 Ќ=Ϋ.φ.LengthSquared()>0.01f?-Vector3.Normalize(Ϋ.φ):-Ϋ.γ;
Vector3 л=Vector3.Cross(ΚΟ,Ќ);if(л.LengthSquared()<0.0001f) return new Vector3();float І=laneOffset*λ;float ΙΦ=Math.Min(Vector3.Distance(Ϋ.Ε,Φ[0].Ε),
Vector3.Distance(Ϋ.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(л)*І*Math.Min(1,ΙΦ/(І*4));}Vector3 Ηκ=new Vector3();bool ΘΤ(Β s){return s==Β.Ƕ||s==Β.ħ||s==Β.ī||s==Β.ƞ||
s==Β.Ǣ||s==Β.Ȫ||s==Β.Č;}void ΘЃ(){Ĥ=new Vector3();Ȏ=-1;ΕΑ=new Vector3();bool ƺ=Ετ(Ψ);if(!trafficEnabled||(!ƺ&&!ΘΤ(Ψ))){Βι=false;return;}Ηκ=Ω.GetShipVelocities().LinearVelocity;ΗЁ(Ω.GetNaturalGravity(),!ƺ);}void ΗЁ(Vector3 φ,bool ΗΝ=false){Ĥ=new Vector3();Ȏ=-1;Vector3 ȋ=Ђ-β;
float ȉ=ȋ.Length();bool ΔΣ=ȉ>=1f;Vector3 ρ=ΔΣ?ȋ/ȉ:new Vector3();ΕΑ=ρ;Vector3 Ќ=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ƴ;Vector3 л=Vector3.Cross(ρ,Ќ);л=л.LengthSquared()<0.0001f?-ǅ:Vector3.Normalize(л);ē ΒΜ=null;for(int i=0;i<э.Count;i++){ē ι=э[i];
if(ι.Ʊ==3&&ι.ƍ.LengthSquared()<0.25f) continue;Vector3 ο=ι.ο+ι.ƍ*(float)(DateTime.Now-ι.ΔЌ).TotalSeconds;Vector3 ſ=ο-β;float ш=(λ+ι.ΓЏ)*0.5f;float Ʀ=ΔΣ?Vector3.Dot(ſ,ρ):0;if(Ʀ>0&&(ſ-ρ*Ʀ).Length()<=ш*1.2f){float ΒΫ=Vector3.Dot(ι.ƍ,ρ);bool Ζψ=ι.ƾ.LengthSquared()>0.25f;
bool ΔΤ=ΒΫ<-1f||(Ζψ&&Vector3.Dot(ι.ƾ,ρ)<-0.5f);if(ΗΝ){if(ΔΤ&&Ǯ<ι.ε) continue;}else{if(ι.Δω&&!Ũ){Ĥ=л*ш*1.3f;continue;}if(ΔΤ){if(Ʀ<(Ć-ΒΫ)*6f+ш*4f) Ĥ=л*ш*1.3f;continue;}if(Ζψ&&Ǯ<ι.ε){Vector3 ΖΛ=-ſ;float ΗΛ=Vector3.Dot(ΖΛ,ι.ƾ);if(ΗΛ>0&&(ΖΛ-ι.ƾ*ΗΛ).Length()<=ш*1.2f)
{Ĥ=л*ш*1.3f;continue;}}}float Ζτ=Ʀ-ш-followGap*λ;float Δα=Math.Max(0,ΒΫ)+Math.Max(0,Ζτ)/ΒЇ;if(ΔΤ) Δα=Math.Max(0,Ζτ+Math.Min(0,ΒΫ)*ΒЇ*2)/ΒЇ;if(Ȏ<0||Δα<Ȏ){Ȏ=Δα;ΒΜ=ι;}continue;}if(ι.ƍ.LengthSquared()<1f) continue;if(ΔΣ&&(ſ-ρ*Ʀ).Length()<=ш*1.2f) continue;
Vector3 Δώ=ι.ƍ-Ηκ;float ΗΡ=Δώ.LengthSquared();if(ΗΡ<0.25f) continue;float t=-Vector3.Dot(ſ,Δώ)/ΗΡ;if(t<=0||t>5f||(ſ+Δώ*t).Length()>=ш*1.1f) continue;if(Ǯ>ι.ε||t<2f){Ȏ=0;ΒΜ=ι;}}bool Ιά=Ȏ>=0&&Ȏ<0.5f;if(!Ιά||ΗΝ||(ΒΜ!=null&&ΒΜ.Δω&&Ũ)){Βι=false;return;}
if(!Βι){Βι=true;ΕΒ=DateTime.Now;}double Ζω=(DateTime.Now-ΕΒ).TotalSeconds;if(Ζω>ΕЎ+Θρ) ΕΒ=DateTime.Now;else if(Ζω>ΕЎ){Ȏ=-1;Ĥ=л*λ*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȃ=new List<int>();String Ζϋ="";void ΒІ(){String Ų=ξ+"|"+И+"|"+(int) ľ;
if(Ų==Ζϋ) return;Ζϋ=Ų;ȃ.Clear();int n=ξ*И;if(ľ==Ǐ.Ƣ){for(int k=0;k<n;k++){int ƀ=k/ξ;ȃ.Add(ƀ*ξ+(ƀ%2==0?k-ƀ*ξ:ξ-1-(k-ƀ*ξ)));}return;}int Ƽ=(int) Math.Ceiling(ξ/2f),ƻ=(int) Math.Ceiling(И/2f);int ΔΟ=ξ/2,ΔΞ=И/2;int x=0,y=0,ΔΓ=0,ΔΔ=-1,Ŷ=Math.Max(ξ,И);
for(int i=0;i<Ŷ*Ŷ;i++){if(-Ƽ<x&&x<=ΔΟ&&-ƻ<y&&y<=ΔΞ) ȃ.Add((y-1+ƻ)*ξ+(x-1+Ƽ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΔΓ;ΔΓ=-ΔΔ;ΔΔ=t;}x+=ΔΓ;y+=ΔΔ;}}bool ΒЅ(){int n=Math.Max(0,ξ*И);if(й.Length==n) return false;й=new String('0',n);
а=-1;return true;}void ΘΨ(String ι){ΒЅ();if(ι.Length!=й.Length) return;char[]h=й.ToCharArray();for(int i=0;i<h.Length;i++) if(ι[i]=='1') h[i]='1';й=new String(h);}bool Ευ(int Ǽ){return Ǽ>=0&&Ǽ<й.Length&&й[Ǽ]=='1';}void Γη(int Ǽ,bool Ǒ){if(Ǽ<0||Ǽ>=й.Length) return;
char[]h=й.ToCharArray();h[Ǽ]=Ǒ?'1':'0';й=new String(h);}bool ΓΒ(int Ǽ,bool Ιδ){if(!trafficEnabled) return false;String Ų=ΓΚ();for(int i=0;i<э.Count;i++){ē t=э[i];if(t.ű!=Ų||t.ƿ<0||(Ιδ&&t.ε>Ǯ)) continue;float Ζβ=Math.Abs(Ǽ%ξ-t.ƿ%ξ)*ȍ;float Ζν=Math.Abs(Ǽ/ξ-t.ƿ/ξ)*Ǵ;
if(t.ƿ==Ǽ||Math.Sqrt(Ζβ*Ζβ+Ζν*Ζν)<Math.Max(λ,t.ΓЏ)*1.1f) return true;}return false;}int Ηώ(int Α){int n=ξ*И;if(n<1) return Α;ΒІ();if(ΒЅ()) for(int k=0;k<Α&&k<n;k++) Γη(ȃ[k],true);if(а>=0&&(Α>=n||ȃ[Α]!=а)){Γη(а,true);а=-1;}if(а>=0&&!Ευ(а)&&!ΓΒ(а,true)) return Α;
а=-1;bool ΖΖ=false;for(int k=0;k<n;k++){int c=ȃ[k];if(Ευ(c)) continue;ΖΖ=true;if(ΓΒ(c,false)) continue;а=c;return k;}return ΖΖ?-1:n;}ǡ Θμ(int Α){ΒІ();if(Α<0||Α>=ȃ.Count) return ǡ.ΑΦ;ώ=ȃ[Α]%ξ;ό=ȃ[Α]/ξ;return ǡ.Αγ;}class Ǎ{public long ε;public Vector3 ο,Ч,Ќ;}List<Ǎ>С=new List<Ǎ>();
bool ę=false;Vector3 ΑЎ,ΑЍ,ΑЏ,ΑА;long і=0;long ȡ=0;Ϊ Ǖ=null;bool Ʋ=false;Ϊ ΒЂ(Vector3 ο,Vector3 Ч,Vector3 Ќ){MatrixD m=MatrixD.CreateWorld(ο,Ч,Ќ);Ϊ w=new Ϊ(Ξ);w.δ=Vector3D.TransformNormal(ΑЎ,m);w.γ=Vector3D.TransformNormal(ΑЍ,m);w.Ѐ=Vector3D.TransformNormal(ΑЏ,m);w.Ε=ο+(Vector3) Vector3D.TransformNormal(ΑА,m);
w.ζ=true;return w;}int Εά(){Β s=Ψ;if(s==Β.о) return Ъ(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Β.ī||s==Β.Ȗ||s==Β.Ī||s==Β.Αω||
s==Β.Αή||s==Β.Ǣ) return 3;if(s==Β.Ƕ||s==Β.ħ||s==Β.ƞ) return 2;if(s==Β.Е) return 1;return 0;}long ΗϏ(){if(Εά()==0) return 0;if(Ē(1)&&Ƈ!=0) return Ƈ;return Ǖ!=null?ȡ:і;}void Εψ(){if(Κ==Ι.ά||!Ξ.ζ) return;IMyShipConnector Л=Ъ(MyShipConnectorStatus.Connected);if(Л==null||Л.OtherConnector==null) return;
IMyShipConnector ι=Л.OtherConnector;Vector3I Ιή=new Vector3I((int) Ξ.и.X,(int) Ξ.и.Y,(int) Ξ.и.Z);if(Л.Position==Ιή){MatrixD Ů=MatrixD.Transpose(ι.WorldMatrix);ΑЎ=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,Ů);ΑЍ=Vector3D.TransformNormal(Ω.WorldMatrix.Down,Ů);ΑЏ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,Ů);
ΑА=Vector3D.TransformNormal(Л.GetPosition()-ι.GetPosition(),Ů);ę=true;}if(Vector3.Distance(Л.GetPosition(),Ξ.Ε)<5) і=ι.EntityId;List<IMyShipConnector>Ļ=new List<IMyShipConnector>();χ.GetBlocksOfType(Ļ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ļ.Count>0){С.Clear();for(int i=0;i<Ļ.Count;i++){Ǎ p=new Ǎ();p.ε=Ļ[i].EntityId;p.ο=Ļ[i].GetPosition();p.Ч=Ļ[i].WorldMatrix.Forward;p.Ќ=Ļ[i].WorldMatrix.Up;С.Add(p);}}Ǖ=null;ȡ=0;if(ι.EntityId==і||!ę) return;for(int i=0;i<С.Count;i++)
if(С[i].ε==ι.EntityId){ȡ=С[i].ε;Ǖ=ΒЂ(С[i].ο,С[i].Ч,С[i].Ќ);}}bool ΓΘ(long ε,int ΗΕ){if(ε==0) return false;for(int i=0;i<э.Count;i++){ē t=э[i];if(t.ί==ε&&(t.Ʊ>ΗΕ||(t.Ʊ==ΗΕ&&t.ε<Ǯ))) return true;}return false;}bool Βύ(int Δφ){if(Κ==Ι.ά||!trafficEnabled){Ǖ=null;ȡ=0;return true;}
long ν=Ǖ!=null?ȡ:і;if(!ΓΘ(ν,Δφ)) return true;if(!ΓΘ(і,Δφ)){Ǖ=null;ȡ=0;return true;}if(!ę) return false;Ǎ ǣ=null;float ƫ=0;for(int i=0;i<С.Count;i++){Ǎ p=С[i];if(p.ε==і||ΓΘ(p.ε,Δφ)) continue;float d=Vector3.Distance(p.ο,Ξ.Ε);if(ǣ==null||d<ƫ){ǣ=p;ƫ=d;}
}if(ǣ==null) return false;ȡ=ǣ.ε;Ǖ=ΒЂ(ǣ.ο,ǣ.Ч,ǣ.Ќ);return true;}String ΗЉ(){String s=(ę?"1":"0")+":"+σ(ΑЎ)+":"+σ(ΑЍ)+":"+
σ(ΑЏ)+":"+σ(ΑА)+":"+і;for(int i=0;i<С.Count;i++)
s+=":"+С[i].ε+","+σ(С[i].ο)+","+σ(С[i].Ч)+","+σ(С[i].Ќ);return s;}void ΗЈ(String s){С.Clear();ę=false;і=0;String[]f=s.Split(':');if(f.Length<6) return;ę=f[0]=="1";ΑЎ=ς(f[1]);ΑЍ=ς(f[2]);ΑЏ=ς(f[3]);ΑА=ς(f[4]);long.TryParse(f[5],out і);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǎ p=new Ǎ();if(!long.TryParse(e[0],out p.ε)) continue;p.ο=ς(e[1]);p.Ч=ς(e[2]);p.Ќ=ς(e[3]);С.Add(p);}}const String Ɠ="[PAMDCK]";const float Ηχ=120f;class ȧ{public long Δ;public int Ǔ;public long ѕ;public bool ΖΣ;public DateTime ĺ,ΖЅ;
}IMyBroadcastListener ΑЌ=null;List<ȧ>Т=new List<ȧ>();List<IMyShipConnector>ć=new List<IMyShipConnector>();List<IMySensorBlock>ȥ=new List<IMySensorBlock>();List<IMyCameraBlock>Ǆ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Βπ=new List<MyDetectedEntityInfo>();long ǽ=0;int ΓЈ=0;
long ΖΥ=0;DateTime ΖΦ=DateTime.Now;String ĸ="";int Ζι=0;IMyCubeGrid Ĉ=null;void Θι(){Ĉ=Me.CubeGrid;χ.GetBlocksOfType(ć,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ȥ,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(Ǆ,q=>q.CubeGrid.IsSameConstructAs(Ĉ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ȥ.Count;i++){ȥ[i].DetectSmallShips=true;ȥ[i].DetectLargeShips=true;}for(int i=0;i<Ǆ.Count;i++) Ǆ[i].EnableRaycast=true;}IMyShipConnector ΕЉ(long ε){if(ε==0) return null;
IMyShipConnector c=χ.GetBlockWithId(ε) as IMyShipConnector;return c!=null&&Ĉ!=null&&c.CubeGrid.IsSameConstructAs(Ĉ)?c:null;}bool ΓΗ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Εφ(IMyShipConnector c,long Δ){for(int i=0;i<Ǆ.Count;i++)
{IMyCameraBlock ǻ=Ǆ[i];if(!ǻ.IsWorking||Vector3D.Distance(ǻ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ȯ=ǻ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ǻ.CanScan(Ȯ)) continue;MyDetectedEntityInfo Ȟ=ǻ.Raycast(Ȯ);if(!Ȟ.IsEmpty()&&Ȟ.EntityId!=Ĉ.EntityId&&Ȟ.EntityId!=Δ&&Ȟ.Type!=MyDetectedEntityType.FloatingObject)
{ĸ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΘΦ(){for(int s=0;s<ȥ.Count;s++){if(!ȥ[s].IsWorking) continue;Βπ.Clear();ȥ[s].DetectedEntities(Βπ);for(int i=0;i<Βπ.Count;i++){MyDetectedEntityInfo e=Βπ[i];bool ΙΨ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΙΨ&&e.EntityId!=ǽ&&e.Velocity.Length()>0.5f){ĸ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Θέ(ȧ r){IMyShipConnector ѕ=ΕЉ(r.ѕ);if(ΓΗ(ѕ)&&Εφ(ѕ,r.Δ)) return ѕ.EntityId;if(!r.ΖΣ||ѕ==null) return 0;IMyShipConnector ǣ=null;double ƫ=0;for(int i=0;i<ć.Count;i++)
{IMyShipConnector c=ć[i];if(!ΓΗ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ѕ.GetPosition());if(ǣ==null||d<ƫ){if(Εφ(c,r.Δ)){ǣ=c;ƫ=d;}}}return ǣ==null?0:ǣ.EntityId;}void ΘΙ(){ȧ Ž=null;long Л=0;for(int ņ=2;ņ>=1&&Ž==null;ņ--){ȧ Ņ=null;for(int i=0;i<Т.Count;i++)
{ȧ r=Т[i];if(r.Ǔ!=ņ) continue;if(ņ==1){long c=Θέ(r);if(c==0) continue;if(Ņ==null||r.ĺ<Ņ.ĺ){Ņ=r;Л=c;}}else if(Ņ==null||r.ĺ<Ņ.ĺ) Ņ=r;}Ž=Ņ;}if(Ž==null){if(Т.Count>0&&ĸ=="") ĸ="No free dock";return;}ǽ=Ž.Δ;ΓЈ=Ž.Ǔ;ΖΥ=Ž.Ǔ==1?Л:0;ΖΦ=DateTime.Now;
}void Θω(){if(Ĉ==null) return;if(ΑЌ==null) ΑЌ=IGC.RegisterBroadcastListener(Ɠ);int ƭ=30;while(ΑЌ.HasPendingMessage&&ƭ-->0){String Τ=ΑЌ.AcceptMessage().Data as String;if(Τ!=null) ΘΚ(Τ);}Θβ();if(++Ζι<3) return;Ζι=0;IGC.SendBroadcastMessage(Ɠ,Ηφ());}void ΘΚ(String Τ){try{String[]f=Τ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Т.Count-1;i>=0;i--) if(Т[i].Δ==Δ) Т.RemoveAt(i);if(ǽ==Δ) ǽ=0;}else if(f[0]=="R"&&f.Length>=5){long ѕ=long.Parse(f[3]);if(ΕЉ(ѕ)==null) return;ȧ r=null;for(int i=0;i<Т.Count;i++) if(Т[i].Δ==Δ) r=Т[i];if(r==null){r=new ȧ();r.Δ=Δ;r.ĺ=DateTime.Now;Т.Add(r);}
int Ǔ=int.Parse(f[2]);if(r.Ǔ!=Ǔ) r.ĺ=DateTime.Now;r.Ǔ=Ǔ;r.ѕ=ѕ;r.ΖΣ=f[4]=="1";r.ΖЅ=DateTime.Now;}}catch{}}void Θβ(){for(int i=Т.Count-1;i>=0;i--)
if((DateTime.Now-Т[i].ΖЅ).TotalSeconds>3) Т.RemoveAt(i);ȧ ΑЁ=null;for(int i=0;i<Т.Count;i++) if(Т[i].Δ==ǽ) ΑЁ=Т[i];if(ǽ!=0&&ΑЁ==null) ǽ=0;if(ΑЁ!=null&&(DateTime.Now-ΖΦ).TotalSeconds>Ηχ){ǽ=0;ΑЁ.ĺ=DateTime.Now;}ĸ="";if(ǽ==0&&Т.Count>0&&!ΘΦ()) ΘΙ();
}String Ηφ(){String η="S;"+Ĉ.EntityId+";"+ǽ+";"+ΓЈ+";"+ΖΥ+";";for(int i=0;i<Т.Count;i++) η+=(i>0?",":"")+Т[i].Δ;η+=";";for(int i=0;i<ć.Count;i++){IMyShipConnector c=ć[i];η+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}η+=";";for(int i=0;i<ȥ.Count;i++){IMySensorBlock z=ȥ[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();η+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return η;}String ΗБ()
{if(ć.Count==0&&ȥ.Count==0) return "";int ƺ=0;for(int i=0;i<ć.Count;i++) if(ΓΗ(ć[i])) ƺ++;return "Docks: "+ƺ+"/"+ć.Count+" free"+(ǽ!=0?(ΓЈ==1?" | docking":" | undocking"):"")+
(Т.Count>1?" | "+(Т.Count-1)+" waiting":"")+(ĸ!=""?"\n"+ĸ:"")+"\n";}IMyBroadcastListener Βε=null;int ȝ=0;DateTime Ζθ=DateTime.Now;DateTime ΗΩ=DateTime.MinValue;long ΗΪ=0;int ΗΫ=0;long Ƈ=0;bool ΑΡ=false;int ΔΒ=0;List<Vector3>ΑΤ=new List<Vector3>();List<float>ΕΛ=new List<float>();
bool Ũ=false;bool ΑЋ=false;Vector3 Ζη=new Vector3();bool ơ(){return ȝ!=0&&(DateTime.Now-ΗΩ).TotalSeconds<3;}bool Ē(int Ǔ){return ơ()&&ΗΪ==Ǯ&&ΗΫ==Ǔ;}bool ΕЊ(){return ơ()||(DateTime.Now-Ζθ).TotalSeconds<3;}void Γβ(int Ǔ){if(ȝ==Ǔ) return;ȝ=Ǔ;Ζθ=DateTime.Now;ΔΒ=3;
}void Γα(){if(ȝ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ɠ,"D;"+Ǯ);ȝ=0;ΑΡ=false;}void Θψ(){if(!trafficEnabled||Κ==Ι.ά) return;if(Βε==null) Βε=IGC.RegisterBroadcastListener(Ɠ);int ƭ=20;while(Βε.HasPendingMessage&&ƭ-->0){String Τ=Βε.AcceptMessage().Data as String;if(Τ==null||!Τ.StartsWith("S;")) continue;
try{ΘΫ(Τ);}catch{}}if(Ψ!=Β.Е){Ũ=false;ΑЋ=false;}if(ȝ==1&&Ъ(MyShipConnectorStatus.Connected)!=null) Γα();if(ȝ==2&&!ΑΡ&&Ψ!=Β.Ǣ&&
Vector3.Distance(β,ц().Ε)>λ*3) Γα();if(ȝ!=0&&++ΔΒ>=3){ΔΒ=0;IGC.SendBroadcastMessage(Ɠ,"R;"+Ǯ+";"+ȝ+";"+і+";"+(ę?1:0));}}void ΘΫ(String Τ){String[]f=Τ.Split(';');if(f.Length<7) return;bool Ζσ=false;String[]ΖΑ=f[5].Split(',');for(int i=0;i<ΖΑ.Length;i++) if(ΖΑ[i]==Ǯ.ToString()) Ζσ=true;
if(!Ζσ) return;ΗΩ=DateTime.Now;ΗΪ=long.Parse(f[2]);ΗΫ=int.Parse(f[3]);Ƈ=long.Parse(f[4]);ΑΤ.Clear();ΕΛ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ΑΤ.Add(ς(e[0]));ΕΛ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ζλ=f[6].Split(':');List<Ǎ>Ј=new List<Ǎ>();for(int i=0;i<Ζλ.Length;i++){String[]e=Ζλ[i].Split(',');if(e.Length<4) continue;Ǎ p=new Ǎ();p.ε=long.Parse(e[0]);p.ο=ς(e[1]);p.Ч=ς(e[2]);p.Ќ=ς(e[3]);Ј.Add(p);}С=Ј;}bool Ηό(){if(!trafficEnabled||Κ==Ι.ά||і==0) return true;
Γβ(1);if(!ΕЊ()) return true;if(!Ē(1)){Λ="Waiting for dock clearance";return false;}Βτ();return true;}bool Βτ(){Ʋ=false;Ǎ Ň=null;if(Ƈ!=0&&Ƈ!=і&&ę)
for(int i=0;i<С.Count;i++) if(С[i].ε==Ƈ) Ň=С[i];long ΕΗ=Ň==null?0:Ň.ε;if(ΕΗ==ȡ&&(ΕΗ==0)==(Ǖ==null)) return false;ȡ=ΕΗ;Ǖ=Ň==null?null:ΒЂ(Ň.ο,Ň.Ч,Ň.Ќ);return true;}bool Ś(Vector3 p){for(int i=0;i<ΑΤ.Count;i++) if(Vector3.Distance(p,ΑΤ[i])<ΕΛ[i]+λ*0.75f) return true;
return false;}bool ΘΜ(Vector3 Ư,Vector3 Ń){Ũ=false;if(!trafficEnabled||Κ==Ι.ά||і==0) return false;if(ȝ!=1&&Vector3.Distance(β,Ξ.Ε)>dockRequestDistance) return false;Γβ(1);if(!ơ()||Ē(1)||ΑΤ.Count==0) return false;if(!Ś(Ư)&&!Ś(Ń)&&!Ś(β)) return false;Ũ=true;return true;
}bool ΕΦ(){if(!trafficEnabled||Κ==Ι.ά||і==0) return true;Γβ(2);if(!ΕЊ()) return true;if(!Ē(2)){Λ="Waiting for undock clearance";return false;}return true;}