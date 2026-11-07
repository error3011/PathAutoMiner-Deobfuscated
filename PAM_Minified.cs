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

// ===== End of settings: don't edit below this line =====
public enum Λ{ī,Ƹ,Ǯ,ћ,ή}public enum Α{ю,Ȇ,ŷ,ȥ,Ȥ,Й,х,
Ĩ,ű,ȅ,ņ,ł,Ĥ,
ƺ,Ȩ,Ǳ,Ņ,ΒΟ,ΑЀ,
ΔΠ,ΒΞ,Ű,ΑЂ,Ĭ}public enum Ρ{я,Ÿ,И,Γδ,Αϋ,ΑЍ,ΑЎ,ū,Ŭ}public enum њ{None,Ņ,ю,ΖΑ}public enum Σ{Main,Ζν,Ʒ,ƶ,ΔΚ,Αω,
ΓΤ,ΖΨ,ΖΩ,ΖΥ,Ƽ,Ζς,
ΒΗ,ΔΑ,ΔΕ,Ζτ,ΒΙ}public enum ч{ΒΕ,ư,ΙЀ,Ζά,ή}public enum ѐ{ƾ,ΕЊ}public enum ь{ΒΔ,Γκ,Ζχ,Ƴ}public enum В{ΓЃ,ΕЏ,Γί,Γε,Αώ,Γτ,ΑЈ}public enum Ǚ{Γα,ΓΩ,ΓΪ}public enum Щ{Ŵ,Ιέ,Ń}public enum ς{ΓЅ,ΓЉ,ΓЊ,Ζθ,Ζη,
ΓЄ,ΒΑ,ΑБ,ΓЇ,
ΓЈ,ΓІ}public enum ζ{ȟ,ċ,Ȟ,Αψ,ΓΧ,ΓΨ}public enum ǰ{ΑЅ,Αχ,ΙΒ}public enum ќ{ΒΚ,ΖΓ,Ιθ,ΓЂ}public enum Њ{None,Ź,ƹ,ΑА}public enum ό{ǀ,Αφ,ΑЋ,Ƴ,ΓЀ,ΑЌ,ΓЁ,ΙЈ,ΓЋ}public enum У{ī,Ζδ,Ζγ,Γύ,Γφ,Γΰ,Ζε}class ά{public bool μ=false;public Vector3 Ε=new Vector3();
public Vector3 θ=new Vector3();public Vector3 ι=new Vector3();public Vector3 Ђ=new Vector3();public Vector3 ψ=new Vector3();public Vector3 р=new Vector3();public float Ш=0;public float ΛЅ=0;public float[]Ȃ=null;public ά(){}public ά(ά κ){μ=κ.μ;Ε=κ.Ε;θ=κ.θ;ι=κ.ι;Ђ=κ.Ђ;ψ=κ.ψ;
р=κ.р;Ȃ=κ.Ȃ;}public ά(Vector3 Ε,Vector3 ι,Vector3 θ,Vector3 Ђ,Vector3 ψ){this.Ε=Ε;this.θ=θ;this.ι=ι;this.Ђ=Ђ;this.Ш=0;this.ψ=ψ;}public void Θύ(List<IMyThrust>ΒΤ,List<string>ă){Ȃ=new float[ă.Count];for(int i=0;i<Ȃ.Length;i++)
Ȃ[i]=-1;for(int i=0;i<ΒΤ.Count;i++){string ǘ=ΖΣ(ΒΤ[i]);int Β=ă.IndexOf(ǘ);if(Β!=-1)
Ȃ[Β]=К(ΒΤ[i].MaxEffectiveThrust,ΒΤ[i].MaxThrust);}}}class ΓЌ{public ά η=null;public List<Vector3>Ƥ=new List<Vector3>();public float ΑΑ=0;public float Εψ=0;public float ƀ=0;public float Δω=0;public Vector3 Βν=new Vector3();}class ż{public ς Т=ς.ΓЅ;public float Ǵ=0;public float Ĵ=0;
public string ǵ="";public string Ǻ="";DateTime Ηύ;public bool ΕΧ=false;private bool Δν=false;public bool Γέ(bool Ǿ){if(Ǿ){Ĵ=0;Δν=false;return false;}Δν=true;return Ĵ>Ǵ;}public void Ιπ(){Γέ(true);ΕΧ=false;}public void ΚΘ(){if(Δν)
if((DateTime.Now-Ηύ).TotalSeconds>1){Ĵ++;Ηύ=DateTime.Now;}}public bool Γρ(){switch(Т){case ς.ΓЉ:return true;case ς.ΓЊ:return true;}return false;}}class о{public String ΰ="";public String χ="";public int Ћ=0;public Щ Ə=Щ.Ń;public о(String ΰ,String χ,int Ћ,Щ Ə){this.ΰ=ΰ;this.χ=χ;
this.Ћ=Ћ;this.Ə=Ə;}}class ΘЎ{public ΘЎ(Vector3 γ,float σ){this.γ=γ;this.σ=σ;}public Vector3 γ;public float σ;}class ϋ{public DateTime ΔЍ;public DateTime ΗΖ;public String Υ="";public String ΰ="";public String ΓΠ="";public String Κ="";public String Εη="";public String ΒЄ="";public String ΕΦ="";
public Vector3 Ε=new Vector3();public List<о>Ќ=new List<о>();public Λ Μ=Λ.ī;public Њ į;public float Ȗ;public float ȏ;public int Џ;public int Δς=0;public int Εσ=0;public bool Γϋ(){return(DateTime.Now-ΔЍ).TotalSeconds>10;}public bool Γχ(){if(į!=Њ.Ź) return false;return(DateTime.Now-ΗΖ).TotalSeconds>=4;
}public ϋ(String Υ){this.Υ=Υ;}public Њ ĩ(String Ѕ,bool ΛΧ,bool б,bool ΚΩ){if(Ѕ==""&&!б) return Њ.None;if(į==Њ.Ź&&Γχ())
į=Њ.ΑА;if(ΛΧ){ΕΦ=Ѕ;į=Њ.Ź;ΗΖ=DateTime.Now;return į;}else if(б){ΕΦ="";į=Њ.None;}else if(ΕΦ==Ѕ){if(ΚΩ) į=Њ.ƹ;return į;}return Њ.None;}}public String ΙΚ(String Κώ){switch(Κώ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem υ;Vector3 ΒЋ=new Vector3();Vector3 δ=new Vector3();Vector3 Βπ=new Vector3();Vector3 ΑΧ=new Vector3();Vector3 ΑΔ=new Vector3();
DateTime ΔА=new DateTime();bool đ=true;int Εν=0;bool ė=false;bool ĺ=false;bool Αέ=true;bool Θρ=false;bool ƍ=false;bool Βυ=false;float Ğ=0;float ť=0;int Ľ=0;int Αο=0;int Αγ=0;int Δγ=0;float ƒ=0;float Ɓ=0;double ΕΚ=0;float ǆ=0;List<int>Ɗ=new List<int>();List<int>Ɯ=new List<int>();
Λ Μ=Λ.ī;int Б=0;float ȃ=0;float ȝ=0;float λ=0;float ΒЎ=0,Εΰ=0;IMyRemoteControl Ω;IMySensorBlock ω;List<IMyTimerBlock>Εο=new List<IMyTimerBlock>();List<IMyShipConnector>ў=new List<IMyShipConnector>();List<IMyThrust>Ą=new List<IMyThrust>();List<IMyGyro>Βρ=new List<IMyGyro>();
List<IMyTerminalBlock>ϊ=new List<IMyTerminalBlock>();List<IMyLandingGear>ȳ=new List<IMyLandingGear>();List<IMyReactor>ļ=new List<IMyReactor>();List<IMyConveyorSorter>ΓΒ=new List<IMyConveyorSorter>();List<IMyGasTank>Ǡ=new List<IMyGasTank>();List<IMyTerminalBlock>ǁ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ƅ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŏ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ĭ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ч=new List<IMyTextPanel>();List<IMyTextSurface>Ț=new List<IMyTextSurface>();List<IMyTextPanel>ő=new List<IMyTextPanel>();
IMyTerminalBlock Ǥ=null;ά Ο=new ά();ά ΔІ=new ά();ά ΔЉ=new ά();bool ą=false;bool Ʃ=false;bool ƪ=false;bool Βχ=false;List<String>ă=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Ατ=new List<ά>();int Δσ=0;double Θς=0;ά Ζ=new ά();Vector3 Ǔ;Vector3 ě;Ρ ε=Ρ.я;ѐ Ř=ѐ.ƾ;int Ѝ=0;
int Ю=0;double ΑΤ=0;bool Ββ=false;ż Ј=new ż();ż Љ=new ż();ь і=ь.ΒΔ;bool ŀ=true;ѐ ș=ѐ.ƾ;int ђ=3;int ё=3;int п=30;Ǚ џ=Ǚ.Γα;bool ģ=true;bool ƌ=true;bool Ɔ=true;bool Ŧ=false;В е=В.ΓЃ;bool Ģ=true;float ȕ=90;float Ǒ=20;float с=5;float ǒ=20;float ȉ=0.70f;float Ʈ=1.50f;
float ƭ=2.50f;float Ū=0f;float ŕ=0f;Α Ϋ;Ρ ǐ;Α Αθ;ΓЌ ο=null;int Ё=0;int ώ=0;int в=0;int ę=0;int ţ=30;int л=0;bool Д=true;Vector3 ѕ;double г=0;bool ΒЍ=false;int Εϋ=0;int Е=0;int ΔΨ=0;int ȗ=0;int ΘΓ=0;Vector3 Αί=new Vector3();float є=0;float ф=0;int ũ=0;ά ΓΓ=null;
float ΔЎ=0;float ΒΦ=0;float к=0;float ś=0;bool ΔЇ=false;bool ŧ=false;bool Απ=false;bool ĕ=false;bool Ď=false;DateTime Κλ=new DateTime();ά ŏ=null;ά ΗΞ=null;float Ȫ=0;њ Ȋ;float Ė=0;float Ũ=0;String ΕΕ="";float Βϋ=0;float ȏ=0;float Ȗ=0;List<о>Ǹ=new List<о>();float ж=0;
bool Αε=false;bool Θζ=false;bool ΘΠ=false;bool ΓΘ=false;bool ΘΡ=false;float ΕΘ=0;float ΕΥ=0;Vector3 Ηβ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 ΓΙ=new Vector3(1,1,1);float Εμ=1;Vector3 Αμ=new Vector3();bool Βφ=false;bool ΔϏ=false;bool ΕΒ=false;bool ΖЂ=false;float Βΰ=0;
float ΒΡ=2;Vector3 Θβ;Vector3 Ελ;Vector3 Εκ;float[,]Ȅ=new float[3,2];Dictionary<String,float[,]>ǩ=new Dictionary<string,float[,]>();Dictionary<String,float>Θο=new Dictionary<String,float>();int Δά=0;Dictionary<String,float[]>Ĝ=new Dictionary<string,float[]>();float ΕΨ;bool Βγ=false;
int ΒΥ=0;int ĸ=0;int Βσ=0;int Ŗ=0;String Κ="";int[]ΗЊ=new int[Enum.GetValues(typeof(Σ)).Length];bool ΕΜ=false;int Ŝ=0;int Џ=0;Σ ύ=Σ.Main;int Λζ=0,Λη=0,Λθ=0,Λι=0;int ΛΡ=0,ΛΣ=0;bool ƃ=false;bool ΖЃ=false;String ơ="";String Δβ="";IMyBroadcastListener ǲ=null;
bool Ηα=true;List<ϋ>Н=null;ϋ М=null;int ΒΪ=0,ΒΫ=0;int ΗΙ=0,ΗΚ=0;int Κί=0,Κΰ=0,Κα=0;String ΚЀ="";Exception Ę=null;bool ΒЌ=false;Ρ ƞ=Ρ.я;bool ΗΡ=false;bool Ηϋ=true;bool Εά=false;String Θπ="";bool Αξ=false;bool Εζ=true;int Βτ=0;int[]ǧ=null;Vector3 Δδ=new Vector3();
bool Ηη=false;bool ǚ(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;Program(){υ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Γς(Me,controllerTag,true))
Μ=Λ.ћ;ν(Σ.Main);if(Μ!=Λ.ћ){Κ="Welcome to [PAM]!";ќ ΕΫ=Ιρ();if(Μ==Λ.ī){List<IMyShipDrill>Ηή=new List<IMyShipDrill>();List<IMyShipGrinder>Ηθ=new List<IMyShipGrinder>();υ.GetBlocksOfType(Ηή,q=>q.CubeGrid==Me.CubeGrid);υ.GetBlocksOfType(Ηθ,q=>q.CubeGrid==Me.CubeGrid);if(Ηή.Count>0)
{Μ=Λ.Ƹ;Κ="Miner mode enabled!";}else if(Ηθ.Count>0){Μ=Λ.Ǯ;Κ="Grinder mode enabled!";}else{Μ=Λ.ή;Ȧ(ч.ή);}}if(ΕΫ==ќ.ΒΚ) Αέ=false;if(ΕΫ==ќ.ΖΓ) Κ="Data restore failed!";if(ΕΫ==ќ.ΓЂ) Κ="New version, wipe data";}}void Main(string Ф,UpdateType Λς){try{if(Ę!=null){if(Ф.Trim().ToUpper()=="RETRY")
{Ę=null;đ=true;Ľ=0;if(Μ!=Λ.ћ) Є();Κ="Restarted after error";}else{ΙΦ();Ζλ();return;}}ĺ=(Λς&UpdateType.Update10)!=0;if(ĺ) Εν++;ė=Εν>=10;if(ė) Εν=0;if(ĺ){ΒΥ++;if(ΒΥ>4) ΒΥ=0;Αγ=(int) Math.Max(0,10-(DateTime.Now-ΔА).TotalSeconds);}if(Ф!="") ΚЀ=Ф;if(Μ!=Λ.ћ)
ΙϏ(Ф);else
ΘЌ(Ф);ΕΜ=false;try{int ΗΟ=Runtime.CurrentInstructionCount;float Ţ=К(ΗΟ,Runtime.MaxInstructionCount);if(Ţ>0.90) Κ="Max. instructions >90%";if(Ţ>ƒ) ƒ=Ţ;if(Βγ){Ɗ.Add(ΗΟ);while(Ɗ.Count>10) Ɗ.RemoveAt(0);Ɓ=0;for(int i=0;i<Ɗ.Count;i++) Ɓ+=Ɗ[i];Ɓ=К(К(Ɓ,Ɗ.Count),Runtime.MaxInstructionCount);
double ΔЏ=Runtime.LastRunTimeMs;if(Βυ&&ΔЏ>ΕΚ) ΕΚ=ΔЏ;Ɯ.Add((int)(ΔЏ*1000f));while(Ɯ.Count>10) Ɯ.RemoveAt(0);ǆ=0;for(int i=0;i<Ɯ.Count;i++) ǆ+=Ɯ[i];ǆ=К(ǆ,Ɯ.Count)/1000f;}}catch{ƒ=0;}}catch(Exception e){Ę=e;}}void ΙϏ(string Ф){bool ΒЏ=false;String ΖЎ="";if(Б<=1&&!Γϊ(Ф))
Ζμ(Ф);int ΕΝ=50;while(ǲ!=null&&ǲ.HasPendingMessage&&ΕΝ-->0){MyIGCMessage β=ǲ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;String Ș="";if(Б<=1&&Ζκ(ref Θ,out Ș,out ΖЎ)&&Ș==BROADCAST_SELF_ID){Ζμ(Θ);ΒЏ=true;break;}}bool Ηυ=Βυ&&Ϋ==Α.ю&&!ƍ&&!ΒЏ&&Ľ==0&&!Βχ;
if(ė&&Ϋ!=Α.ю) ΒЏ=true;if((ĺ&&!Ηυ)||(ė&&Ηυ)){if(Ľ==0&&(Αγ<=0||đ)){Θρ=Б>0;Б=0;Ľ=1;ȧ();Ιφ();ΖΪ();Ƞ("Scan 1");}else if(Ľ==1){Ľ=2;ȧ();Ιχ();Ƞ("Scan 2");}else if(Ľ==2){Ľ=0;ȧ();Ιψ();Ιω();if(Б<=1) Ζβ();Ƞ("Scan 3");ΔА=DateTime.Now;if(Б<=1&&Αέ)
ΒЋ=ΑІ(Ω,Ω.CenterOfMass);Αέ=false;if(đ){đ=false;bool Λχ=ε==Ρ.И;Є();if(Ηϋ&&resumeAfterRestart&&Λχ&&Б<=1) Εά=true;}Ηϋ=false;if(Θρ&&Б==0) Κ="Setup complete";}else{if(ε==Ρ.И&&Μ!=Λ.ή){ȧ();Θφ();Ƞ("Inv balance");}ȧ();switch(Αο){case 0:ΙЌ();break;case 1:ΚΑ();break;case 2:ΙЋ();break;
case 3:ΚΜ();break;case 4:ΙБ();break;case 5:ΘЁ();break;case 6:ΘϏ(Ω);break;}Ƞ("Update: "+Αο);Αο++;if(Αο>6){Αο=0;Βυ=true;if(ƞ!=Ρ.я){switch(ƞ){case Ρ.ū:Ħ();break;case Ρ.Ŭ:Ħ();break;case Ρ.И:Ħ();break;case Ρ.ΑЍ:ΑЃ();break;case Ρ.ΑЎ:Ň();break;}ƞ=Ρ.я;}}}if(!đ){if(!Ȣ(Ω,true))
{Ω=null;Αέ=true;Б=2;}if(Б>=2&&Ϋ!=Α.ю)
Є();if(Б<=1){ť=Ω.CalculateShipMass().PhysicalMass;Ğ=(float) Ω.GetShipSpeed();δ=ΙЊ(Ω,ΒЋ);Βπ=Ω.WorldMatrix.Forward;ΑΧ=Ω.WorldMatrix.Left;ΑΔ=Ω.WorldMatrix.Down;ΚΕ();ΚΙ();ΙЎ();if(Εά){Εά=false;Ħ();if(Ϋ!=Α.ю) Κ="Job resumed after restart";}if(Ϋ!=Α.ю){ƍ=false;Ζο(false);ΕЈ(false);
String Λά=Γν(Ϋ)+" "+(int) Ϋ;ȧ();ΚΓ();ΔΟ(false);Ƞ(Λά);ΚΚ();ΚΔ();ȧ();ΙА();Ƞ("Thruster");ȧ();Ζώ();Ƞ("Gyroscope");}else{if(ƍ){if(ΑЊ()){н(ΑΔ,Βπ,ΑΧ,0.25f,true);Ζώ();Κ="Aligning to planet: "+Math.Round(Βΰ-0.25f,2)+"°";if(Βφ) Γπ(true,true);}else Γπ(true,true);}}Αξ=false;}}}ȧ();Ζλ();Ƞ("Print");
if(ΒЏ||Δγ<=0){ȧ();Θχ(ΖЎ);Ƞ("Broadcast");Δγ=4;}else if(ė) Δγ--;}void Ζμ(string Ф){if(Ф=="") return;var φ=Ф.ToUpper().Split(' ');φ.DefaultIfEmpty("");var Ѕ=φ.ElementAtOrDefault(0);var ȩ=φ.ElementAtOrDefault(1);var Ǆ=φ.ElementAtOrDefault(2);var ΖЅ=φ.ElementAtOrDefault(3);String Ȳ="Invalid argument: "+Ф;
bool Εχ=false;switch(Ѕ){case "UP":this.ΓϏ(false);break;case "DOWN":this.Γώ(false);break;case "UPLOOP":this.ΓϏ(true);break;case "DOWNLOOP":this.Γώ(true);break;case "APPLY":this.ΕІ(true);break;case "MRES":Џ=0;break;case "STOP":this.Є();break;case "PATHHOME":{this.Є();this.ΔΙ();}break;
case "PATH":{this.Є();this.ΔΙ();Ο.μ=true;}break;case "START":{this.Є();ΔΘ();}break;case "ALIGN":{Γπ(!ƍ,false);}break;case "CONT":{this.Є();this.Ħ();}break;case "JOBPOS":{this.Є();this.Ň();}break;case "HOMEPOS":{this.Є();this.ΑЃ();}break;case "FULL":{Αε=true;}break;
case "RESET":{ΒЌ=true;Б=2;}break;case "EXPORT":{ΙΓ();}break;case "IMPORT":{ΙΫ(ȩ=="JOB");}break;default:Εχ=true;break;}if(Μ!=Λ.ή){switch(Ѕ){case "SHUTTLE":{ΘБ();}break;case "CFGS":{if(!ΘЊ(ȩ,Ǆ,ΖЅ)) Κ=Ȳ;}break;case "CFGB":{if(!ΘЉ(ȩ,Ǆ)) Κ=Ȳ;}break;case "CFGL":
{if(!ň(ref ȕ,true,ό.ΑЋ,ȩ,"")||!ΘЋ(Ǆ))
Κ=Ȳ;}break;case "CFGE":{if(!ň(ref с,true,ό.ΑЌ,ȩ,"IG")||
!ň(ref Ǒ,true,ό.ΓЀ,Ǆ,"IG")||
!ň(ref ǒ,true,ό.ΓЁ,ΖЅ,"IG"))
Κ=Ȳ;}break;case "CFGA":{if(!ň(ref ȉ,false,ό.Αφ,ȩ,"")) Κ=Ȳ;}break;case "CFGW":{if(!ň(ref Ʈ,false,ό.ǀ,ȩ,"")||
!ň(ref ƭ,false,ό.ǀ,Ǆ,""))
Κ=Ȳ;}break;case "NEXT":{ΓΣ(false);}break;case "PREV":{ΓΣ(true);}break;default:if(Εχ) Κ=Ȳ;break;}}else{switch(Ѕ){case "UNDOCK":{Αξ=true;}break;default:if(Εχ) Κ=Ȳ;break;}}}String ΙΛ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Μ!=Λ.ή)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΘБ(){Є();Μ=Λ.ή;Ȧ(ч.ή);Ζ.μ=false;Ο.μ=false;ω=null;ϊ.Clear();ε=Ρ.я;}void Γπ(bool О,bool Ѐ){if(!О) Κ="Aligning canceled";if(Ѐ) Κ="Aligning done";if(Ѐ||!О){ƍ=false;Ζψ();ΒΘ(false,0,0,0,0);return;}if(ΑЊ()) ƍ=true;}bool ΘЊ(String Λω,String Κχ,String Κε)
{bool Λυ=ε==Ρ.И;int w,h,d;if(int.TryParse(Λω,out w)&&int.TryParse(Κχ,out h)&&int.TryParse(Κε,out d)){this.Є();ђ=w;ё=h;п=d;Ō(false);ƿ(false,false);if(Λυ) Ħ();return true;}return false;}bool ΘЋ(String ǃ){if(ǃ=="ON"){ģ=true;return true;}if(ǃ=="OFF"){ģ=false;return true;}
return false;}bool ΘЉ(String Ηέ,String Βα){bool Ā=true;if(Ηέ=="HOME") ŀ=true;else if(Ηέ=="STOP") ŀ=false;else Ā=false;if(Βα=="HOME") і=ь.ΒΔ;else if(Βα=="STOP") і=ь.Ζχ;else if(Βα=="JOB") і=ь.Γκ;else if(Βα=="IG") і=ь.Ƴ;else Ā=false;return Ā;}void ν(Σ ш){ΗЊ[(int) ύ]=Џ;
Џ=ΗЊ[(int) ш];if(ш==Σ.Ƽ) Џ=0;ύ=ш;if(Μ!=Λ.ћ)
ŋ(ύ==Σ.Ʒ,false,0,0);ΕΜ=true;}void ΓϏ(bool ΕΓ){if(Џ>0) Џ--;else if(ΕΓ) Џ=Ŝ-1;}void Γώ(bool ΕΓ){if(Џ<Ŝ-1) Џ++;else if(ΕΓ) Џ=0;}String Ȧ(ч β){switch(β){case ч.ΒΕ:Κ="Job is running";break;case ч.ư:Κ="Connector not ready!";break;case ч.ΙЀ:Κ="Ship modified, path outdated!";break;
case ч.Ζά:Κ="Interrupted by player!";break;case ч.ή:Κ="Shuttle mode enabled!";break;}return "";}String ΙΡ(ѐ γ){switch(γ){case ѐ.ƾ:return "Top-Left";case ѐ.ΕЊ:return "Center";default:return "";}}String ΙΗ(Ǚ А){switch(А){case Ǚ.ΓΩ:return "Auto"+(Μ==Λ.Ƹ?" (Ore)":"");case Ǚ.ΓΪ:return "Auto (+Stone)";
case Ǚ.Γα:return "Default";default:return "";}}String Γλ(Ρ І){switch(І){case Ρ.я:return "No job";case Ρ.Ÿ:return "Job paused";case Ρ.И:return "Job active";case Ρ.ū:return "Job active";case Ρ.Ŭ:return "Job active";case Ρ.Γδ:return "Job done";case Ρ.Αϋ:return "Job changed";case Ρ.ΑЍ:return "Move home";
case Ρ.ΑЎ:return "Move to job";}return "";}String ΖΙ(ь ΚΨ){switch(ΚΨ){case ь.ΒΔ:return "Return home";case ь.Γκ:return "Fly to job pos";case ь.Ζχ:return "Stop";case ь.Ƴ:return "Ignore";}return "";}String ΙΙ(В А){switch(А){case В.ΓЃ:return "Off";case В.Γε:return "Drop pos (Stone) ";case В.Αώ:return "Drop pos (Sto.+Ice)";
case В.ΕЏ:return "Cur. pos (Stone)";case В.Γί:return "Cur. pos (Sto.+Ice)";case В.Γτ:return "In motion (Stone)";case В.ΑЈ:return "In motion (Sto.+Ice)";}return "";}String ΖΗ(њ І){switch(І){case њ.None:return "No batteries";case њ.Ņ:return "Charging";case њ.ΖΑ:return "Discharging";}return "";}
String Γν(Α І){String ΛЃ=Μ==Λ.ή?"target":"job";switch(І){case Α.ю:return "Idle";case Α.Ȇ:return "Flying to XY position";case Α.ŷ:return Μ==Λ.Ǯ?"Grinding":"Mining";case Α.ȥ:return "Returning";case Α.Ű:return "Flying to drop pos";case Α.Ȥ:return "Returning to dock";case Α.Й:return "Flying to dock area";
case Α.х:return "Flying to job area";case Α.Ĩ:return "Flying to path";case Α.ű:return "Flying to job position";case Α.ȅ:return "Approaching dock";case Α.ņ:return "Docking";case Α.ł:return "Aligning";case Α.Ĥ:return "Aligning";case Α.ƺ:return "Retry docking";case Α.Ȩ:return "Unloading";case Α.Ĭ:return Θπ;
case Α.Ǳ:return "Undocking";case Α.Ņ:return "Charging batteries";case Α.ΒΟ:return "Waiting for uranium";case Α.ΑЀ:return "Filling up hydrogen";case Α.ΔΠ:return "Waiting for ejection";case Α.ΒΞ:return "Waiting for ejection";case Α.ΑЂ:return "Flying to drop pos";}return "";}String ΖΤ(ς Т){switch(Т)
{case ς.ΓЅ:return "On \"Undock\" command";case ς.ΓЉ:return "On player entered cockpit";case ς.Ζθ:return "On ship is full";case ς.Ζη:return "On ship is empty";case ς.ΓЊ:return "On time delay";case ς.ΒΑ:return "On batteries empty(<25%)";case ς.ΑБ:return "On batteries empty(=0%)";case ς.ΓЄ:return "On batteries full";
case ς.ΓЈ:return "On hydrogen empty(<25%)";case ς.ΓІ:return "On hydrogen empty(=0%)";case ς.ΓЇ:return "On hydrogen full";}return "";}bool Ν(ref String Γ,int ΘΝ,int ķ,bool Ι,String ř){Ŝ+=1;if(ΘΝ==ķ) ř=">"+ř+(ΒΥ>=2?" .":"");else ř=" "+ř;Γ+=ř+"\n";return ΘΝ==ķ&&Ι;}String ΕІ(bool Ι)
{int Β=0;int Η=Џ;Ŝ=0;String Ơ="———————————————\n";String Ʀ="--------------------------------------------\n";String Γ="";Γ+=Γλ(ε)+" | "+(Ο.μ?"Ready to dock":"No dock")+"\n";Γ+=Ơ;double ȁ=Math.Max(Math.Round(this.г),0);if(ύ==Σ.Main){bool Ĺ=Μ==Λ.ή;if(Ν(ref Γ,Η,Β++,Ι," Record path & set home")) ΔΙ();
if(Μ==Λ.Ƹ)
if(Ν(ref Γ,Η,Β++,Ι," Setup mining job")) ν(Σ.Ʒ);if(Μ==Λ.Ǯ)
if(Ν(ref Γ,Η,Β++,Ι," Setup grinding job")) ν(Σ.Ʒ);if(Μ==Λ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Setup shuttle job")) ν(Σ.ΔΕ);if(Ν(ref Γ,Η,Β++,Ι," Continue job")) Ħ();if(Ν(ref Γ,Η,Β++,Ι," Fly to home position")) ΑЃ();if(Ν(ref Γ,Η,Β++,Ι," Fly to job position")) Ň();if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(Ĺ) ν(Σ.ΒΙ);else ν(Σ.Αω);if(Ν(ref Γ,Η,Β++,Ι," Info")) ν(Σ.ΖΨ);if(Μ!=Λ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Help")) ν(Σ.ΖΥ);}else if(ύ==Σ.Ʒ){double Λψ=Math.Round(ђ*ȃ,1);double Κφ=Math.Round(ё*ȝ,1);String Ж="";if(Ν(ref Ж,Η,Β++,Ι," Start new job!")) ΔΘ();if(Ν(ref Ж,Η,Β++,Ι," Change current job")){ƿ(false,false);ν(Σ.Main);}if(Ν(ref Ж,Η,Β++,Ι," Width + (Width: "+ђ+" = "+Λψ+"m)")){ŭ(ref ђ,5,20,1);Ō(true);}
if(Ν(ref Ж,Η,Β++,Ι," Width -")){ŭ(ref ђ,-5,20,-1);Ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Height + (Height: "+ё+" = "+Κφ+"m)")){ŭ(ref ё,5,20,1);Ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Height -")){ŭ(ref ё,-5,20,-1);Ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Depth + ("+(џ==Ǚ.Γα?"Depth":"Min")+": "+п+"m)")){ŭ(ref п,5,50,2);Ō(true);}
if(Ν(ref Ж,Η,Β++,Ι," Depth -")){ŭ(ref п,-5,50,-2);Ō(true);}if(Ν(ref Ж,Η,Β++,Ι," Depth mode: "+ΙΗ(џ))){џ=ħ(џ);}if(Ν(ref Ж,Η,Β++,Ι," Start pos: "+ΙΡ(ș))){ș=ħ(ș);}if(Μ==Λ.Ǯ&&џ==Ǚ.ΓΪ) џ=ħ(џ);Γ+=ΓБ(8,Ж,Η,ref ΛΡ);}else if(ύ==Σ.ΔΕ){float[]ΗΤ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ν(ref Γ,Η,Β++,Ι," Next")) ν(Σ.Ζτ);if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=" Leave connector 1:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΤ(Ј.Т))) Ј.Т=ħ(Ј.Т);if(!Ј.Γρ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЄ((int) Ј.Ǵ))) Ј.Ǵ=ΕА(Ј.Ǵ,ΗΤ);Γ+=" Leave connector 2:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΤ(Љ.Т))) Љ.Т=ħ(Љ.Т);
if(!Љ.Γρ()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЄ((int) Љ.Ǵ))) Љ.Ǵ=ΕА(Љ.Ǵ,ΗΤ);}else if(ύ==Σ.Ζτ){if(Ν(ref Γ,Η,Β++,Ι," Start job!")) ΔΘ();if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.ΔΕ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǵ!=""?Ј.ǵ:"-"))) Ј.ǵ=ΑЏ(ref Λζ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.Ǻ!=""?Ј.Ǻ:"-"))) Ј.Ǻ=ΑЏ(ref Λθ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Љ.ǵ!=""?Љ.ǵ:"-"))) Љ.ǵ=ΑЏ(ref Λη);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Љ.Ǻ!=""?Љ.Ǻ:"-"))) Љ.Ǻ=ΑЏ(ref Λι);
}else if(ύ==Σ.ƶ){String ΗЂ=ȕ+" %";if(ė) ĸ++;if(ĸ>1){ĸ=0;Βσ++;if(Βσ>1) Βσ=0;}bool[]Εγ=new bool[]{ļ.Count==0,Ȋ==њ.None,Ǡ.Count==0};int Θλ=0;while(true){Θλ++;Ŗ++;if(Ŗ>Εγ.Length-1) Ŗ=0;if(Θλ>=Εγ.Length) break;if(!Εγ[Ŗ]) break;}bool Ĺ=Μ==Λ.ή;if(!Ĺ&&ģ&&ж!=-1&&Βσ==0)
ΗЂ=ж<1000000?Math.Round(ж)+" Kg":Math.Round(ж/1000)+" t";if(Ν(ref Γ,Η,Β++,Ι," Stop!")){Є();ν(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(!Ĺ) ν(Σ.Αω);else ν(Σ.ΒΙ);if(!Ĺ){if(Ν(ref Γ,Η,Β++,Ι," Next hole")) ΓΣ(false);}else if(Ν(ref Γ,Η,Β++,Ι," Undock")) Αξ=true;Γ+=Ʀ;if(!Ĺ) Γ+="Progress: "+Math.Round(ΑΤ,1)+" %\n";Γ+="State: "+Γν(Ϋ)+" "+ȁ+"m \n";Γ+="Load: "+Βϋ+" % Max: "+ΗЂ+" \n";if(Ŗ==0) Γ+="Uranium: "+(ļ.Count==0?"No reactors":Math.Round(Ũ,1)+"Kg "+(с==-1?"":" Min: "+с+" Kg"))+"\n";
if(Ŗ==1) Γ+="Battery: "+(Ȋ==њ.None?ΖΗ(Ȋ):Ȫ+"% "+(Ǒ==-1||Ĺ?"":" Min: "+Ǒ+" %"))+"\n";if(Ŗ==2) Γ+="Hydrogen: "+(Ǡ.Count==0?"No tanks":Math.Round(Ė,1)+"% "+(ǒ==-1||Ĺ?"":" Min: "+ǒ+" %"))+"\n";}else if(ύ==Σ.Αω){String Ж="";if(Ν(ref Ж,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}
if(Ν(ref Ж,Η,Β++,Ι," Max load: "+ȕ+"%")) ы(ref ȕ,ȕ<=80?-10:-5,ό.ΑЋ,false);if(Ν(ref Ж,Η,Β++,Ι," Weight limit: "+(ģ?"On":"Off"))) ģ=!ģ;if(Ν(ref Ж,Η,Β++,Ι," Ejection: "+ΙΙ(е))) е=ħ(е);if(Ν(ref Ж,Η,Β++,Ι," Toggle sorters: "+(Ŧ?"On":"Off"))){Ŧ=!Ŧ;if(Ŧ) Ȉ(Εζ);}
if(Ν(ref Ж,Η,Β++,Ι," Unload ice: "+(Ģ?"On":"Off"))) Ģ=!Ģ;if(Ν(ref Ж,Η,Β++,Ι," Uranium: "+(с==-1?"Ignore":"Min "+с+"Kg"))) ы(ref с,(с>5?-5:-1),ό.ΑЌ,true);if(Ν(ref Ж,Η,Β++,Ι," Battery: "+(Ǒ==-1?"Ignore":"Min "+Ǒ+"%"))) ы(ref Ǒ,-5,ό.ΓЀ,true);if(Ν(ref Ж,Η,Β++,Ι," Hydrogen: "+(ǒ==-1?"Ignore":"Min "+ǒ+"%"))) ы(ref ǒ,-10,ό.ΓЁ,true);
if(Ν(ref Ж,Η,Β++,Ι," When done: "+(ŀ?"Return home":"Stop"))) ŀ=!ŀ;if(Ν(ref Ж,Η,Β++,Ι," On damage: "+ΖΙ(і))) і=ħ(і);if(Ν(ref Ж,Η,Β++,Ι," Advanced...")) ν(Σ.ΓΤ);Γ+=ΓБ(8,Ж,Η,ref ΛΣ);}else if(ύ==Σ.ΓΤ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}
if(Ν(ref Γ,Η,Β++,Ι,(Μ==Λ.Ǯ?" Grinder":" Drill")+" inv. balancing: "+(ƌ?"On":"Off"))) ƌ=!ƌ;if(Ν(ref Γ,Η,Β++,Ι," Enable"+(Μ==Λ.Ǯ?" grinders":" drills")+": "+(Ɔ?"Fwd + Bwd":"Fwd"))) Ɔ=!Ɔ;if(Ν(ref Γ,Η,Β++,Ι," Work speed fwd.: "+Ʈ+"m/s")) ы(ref Ʈ,0.5f,ό.ǀ,false);
if(Ν(ref Γ,Η,Β++,Ι," Work speed bwd.: "+ƭ+"m/s")) ы(ref ƭ,0.5f,ό.ǀ,false);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(ȉ*100f)+"%"+(ȉ>0.80f?" (risky)":""))) ы(ref ȉ,0.1f,ό.Αφ,false);if(Ν(ref Γ,Η,Β++,Ι," Width overlap: "+Ū*100f+"%")) ΕЋ(true,0.05f);if(Ν(ref Γ,Η,Β++,Ι," Height overlap: "+ŕ*100f+"%")) ΕЋ(false,0.05f);
}else if(ύ==Σ.ΒΙ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(ε==Ρ.И) ν(Σ.ƶ);else ν(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Max load: "+ȕ+"%")) ы(ref ȕ,ȕ<=80?-10:-5,ό.ΑЋ,false);if(Ν(ref Γ,Η,Β++,Ι," Unload ice: "+(Ģ?"On":"Off"))) Ģ=!Ģ;if(Ν(ref Γ,Η,Β++,Ι," Uranium: "+(с==-1?"Ignore":"Min "+с+"Kg"))) ы(ref с,(с>5?-5:-1),ό.ΑЌ,true);
if(Ν(ref Γ,Η,Β++,Ι," Battery: "+(Ǒ==-1?"Ignore":"Charge up"))) Ǒ=(Ǒ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," Hydrogen: "+(ǒ==-1?"Ignore":"Fill up"))) ǒ=(ǒ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," On damage: "+ΖΙ(і))) і=ħ(і);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(ȉ*100f)+"%"+(ȉ>0.80f?" (risky)":""))) ы(ref ȉ,0.1f,ό.Αφ,false);
}else if(ύ==Σ.Ζν){double ΚЁ=0;if(Ψ.Count>0) ΚЁ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ν(ref Γ,Η,Β++,Ι," Stop path recording")) Ζω();if(Μ!=Λ.ή){if(Ν(ref Γ,Η,Β++,Ι," Home: "+(ą?"Use old home":(Ο.μ?"Was set! ":"none ")))) ą=!ą;}else{if(Ν(ref Γ,Η,Β++,Ι," Connector 1: "+(ą?"Use old connector":(Ο.μ?"Was set! ":"none ")))) ą=!ą;
if(Ν(ref Γ,Η,Β++,Ι," Connector 2: "+(Ʃ?"Use old connector":(Ζ.μ?"Was set! ":"none ")))) Ʃ=!Ʃ;}if(Ν(ref Γ,Η,Β++,Ι," Path: "+(ƪ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) ƪ=!ƪ;Γ+=Ʀ;Γ+="Wp spacing: "+Math.Round(Θς)+"m\n";}else if(ύ==Σ.ΔΚ){if(Ν(ref Γ,Η,Β++,Ι," Stop")){Є();ν(Σ.Main);}
Γ+=Ʀ;Γ+="State: "+Γν(Ϋ)+" \n";Γ+="Speed: "+Math.Round(Ğ,1)+"m/s\n";Γ+="Target dist: "+ȁ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+Εϋ+"\n";}else if(ύ==Σ.ΖΨ){List<IMyTerminalBlock>Ő=ΖΚ();if(ė) ĸ++;if(ĸ>=Ő.Count) ĸ=0;if(Ν(ref Γ,Η,Β++,Ι," Next")) ν(Σ.ΖΩ);
Γ+=Ʀ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βϋ,1)+"% "+Math.Round(ȏ,1)+" / "+Math.Round(Ȗ,1)+"\n";Γ+="Uranium: "+(ļ.Count==0?"No reactors":Math.Round(Ũ,1)+"Kg "+ΕΕ)+"\n";Γ+="Battery: "+(Ȋ==њ.None?"":Ȫ+"% ")+ΖΗ(Ȋ)+"\n";Γ+="Hydrogen: "+(Ǡ.Count==0?"No tanks":Math.Round(Ė,1)+"% ")+"\n";
Γ+="Damage: "+(Ő.Count==0?"None":""+(ĸ+1)+"/"+Ő.Count+" "+Ő[ĸ].CustomName)+"\n";}else if(ύ==Σ.ΖΩ){if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=Ʀ;Γ+="Next scan: "+Αγ+"s\n";Γ+="Drill area: "+Math.Round(ȃ,1)+"x"+Math.Round(ȝ,1)+"m Ship: "+Math.Round(ΒЎ,1)+"x"+Math.Round(Εΰ,1)+"m ("+Math.Round(λ,1)+"m)\n"+(Ȍ.Count>0?"Cameras: "+Δε+"\n":"");
Γ+="Broadcast: "+(ƃ?"Online - "+ơ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ƒ*100f,1)+"% \n";}else if(ύ==Σ.ΖΥ){if(Ν(ref Γ,Η,Β++,Ι," Back")) ν(Σ.Main);Γ+=Ʀ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Б==2) Γ="Fatal setup error\nNext scan: "+Αγ+"s\n";if(ΒЌ) Γ="Recompile script now";int ΚЂ=Γ.Split('\n').Length;for(int i=ΚЂ;i<=10;i++) Γ+="\n";Γ+=Ơ;Γ+="Last: "+Κ+"\n";
return Γ;}void ΕЋ(bool Κύ,float Ǩ){Є();ƿ(true,false);if(Κύ) ы(ref Ū,Ǩ,ό.ΓЋ,false);else ы(ref ŕ,Ǩ,ό.ΓЋ,false);ΕЉ();ŋ(true,true,0,0);}float ΕА(float π,float[]ΓΟ){float ί=ΓΟ[0];for(int i=ΓΟ.Length-1;i>=0;i--)
if(π<ΓΟ[i]) ί=ΓΟ[i];return ί;}String ΑЏ(ref int ķ){String ί="";if(ķ>=Εο.Count) ķ=-1;if(ķ>=0) ί=Εο[ķ].CustomName;ķ++;return ί;}void ΒΜ(string ΰ){if(ε!=Ρ.И) return;if(ΰ=="") return;IMyTerminalBlock Ϊ=υ.GetBlockWithName(ΰ);if(Ϊ==null||!(Ϊ is IMyTimerBlock)){Κ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ϊ).Trigger();}void ŭ(ref int value,int ǉ,int ġ,int Εδ){if(ǉ==0) return;if(value<ġ&&Εδ>0||value<=ġ&&Εδ<0){value+=Εδ;return;}int ΚΞ=Math.Abs(ǉ);int ΔΡ=0;int ΕΞ=1;while(true){ΔΡ+=ΕΞ*ΚΞ*10;if(ǉ<0&&value-ġ<=ΔΡ) break;if(ǉ>0&&value-ġ<ΔΡ) break;
ΕΞ++;}value+=ΕΞ*ǉ;}void Ō(bool ΛІ){ђ=Math.Max(ђ,1);ё=Math.Max(ё,1);п=Math.Max(п,0);ŋ(ύ==Σ.Ʒ,false,0,0);}bool ň(ref float value,bool ΛΝ,ό χ,String ŗ,String Κϊ){if(ŗ=="") return false;float Ɩ=-1;bool Ης=false;if(ŗ.ToUpper()==Κϊ) Ης=true;else if(!float.TryParse(ŗ,out Ɩ)) return false;
else Ɩ=Math.Max(0,Ɩ);if(ΛΝ) Ɩ=(float) Math.Round(Ɩ);ы(ref value,Ɩ,χ,Ης,false);return true;}void ы(ref float value,float Ǩ,ό χ,bool Αν){ы(ref value,value+Ǩ,χ,Αν,true);}void ы(ref float value,float Ɠ,ό χ,bool Αν,bool Λύ){float Ǽ=0,ǻ=0;if(χ==ό.ǀ){Ǽ=0.5f;ǻ=10f;}if(χ==ό.Αφ){Ǽ=0.1f;ǻ=1f;}
if(χ==ό.Ƴ){Ǽ=50f;ǻ=100f;}if(χ==ό.ΓЀ){Ǽ=5f;ǻ=30f;}if(χ==ό.ΑЌ){Ǽ=1f;ǻ=25f;}if(χ==ό.ΑЋ){Ǽ=10f;ǻ=95f;}if(χ==ό.ΓЁ){Ǽ=10f;ǻ=90f;}if(χ==ό.ΙЈ){Ǽ=10f;ǻ=1800;}if(χ==ό.ΓЋ){Ǽ=0.0f;ǻ=0.75f;}if(Ɠ==-1&&Αν){value=-1;return;}if(value==-1) Αν=false;
bool ΘΕ=Ɠ<Ǽ||Ɠ>ǻ;if(ΘΕ&&Λύ){if(Ɠ<value) value=ǻ;else if(Ɠ>value) value=Ǽ;}else value=Ɠ;if(ΘΕ&&Αν) value=-1;else value=Math.Max(Ǽ,Math.Min(value,ǻ));value=(float) Math.Round(value,2);}void ΓΣ(bool ΛΙ){if(ΛΙ) в=Math.Max(0,в-1);else в++;Γη();Γζ();for(int k=0;k<Ȱ.Count&&k<д.Length;k++) ΔΓ(Ȱ[k],k<в);
Г=-1;ΔΟ(true);}T ħ<T>(T π){int Β=Array.IndexOf(Enum.GetValues(π.GetType()),π);Β++;if(Β>=ΙΑ(π)) Β=0;return(T) Enum.GetValues(π.GetType()).GetValue(Β);}int ΙΑ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔΙ(){Ατ.Clear();for(int i=0;i<Ψ.Count;i++) Ατ.Add(Ψ[i]);Ψ.Clear();
Βχ=true;ΔІ=new ά(Ο);ΔЉ=new ά(Ζ);Ο.μ=false;if(Μ==Λ.ή) Ζ.μ=false;for(int i=0;i<ǩ.Count;i++)
if(!ă.Contains(ǩ.Keys.ElementAt(i)))
ă.Add(ǩ.Keys.ElementAt(i));ą=false;Ʃ=false;ƪ=false;ν(Σ.Ζν);}void Ζω(){if(ą) Ο=ΔІ;if(Ʃ) Ζ=ΔЉ;if(ƪ){Ψ.Clear();for(int i=0;i<Ατ.Count;i++) Ψ.Add(Ατ[i]);}Βχ=false;Є();ν(Σ.Main);}void ΚΕ(){if(!Βχ) return;if(Ϋ!=Α.ю){Ζω();return;}if(!ΔІ.μ) ą=false;if(!ΔЉ.μ) Ʃ=false;if(Ατ.Count<=1) ƪ=false;
IMyShipConnector Р=и(MyShipConnectorStatus.Connectable);if(Р==null) Р=и(MyShipConnectorStatus.Connected);if(Р!=null){if(Math.Round(Ğ,2)<=0.20) Δσ++;else Δσ=0;if(Δσ>=5){if(Μ==Λ.ή&&(Ο.μ||ą)&&Vector3.Distance(Ο.Ε,Р.GetPosition())>5){Ζ.ι=Ω.WorldMatrix.Forward;Ζ.Ђ=Ω.WorldMatrix.Left;
Ζ.θ=Ω.WorldMatrix.Down;Ζ.ψ=Ω.GetNaturalGravity();Ζ.Ε=Р.GetPosition();Ζ.μ=true;Ζ.р=Р.Position;}else{Ο.ι=Ω.WorldMatrix.Forward;Ο.Ђ=Ω.WorldMatrix.Left;Ο.θ=Ω.WorldMatrix.Down;Ο.ψ=Ω.GetNaturalGravity();Ο.Ε=Р.GetPosition();Ο.μ=true;Ο.р=Р.Position;}}}double ΔЌ=-1;if(Ψ.Count>0) ΔЌ=Vector3.Distance(δ,Ψ.Last().Ε);
double ΛΪ=Math.Max(1.5,Math.Pow(Ğ/100.0,2));double ΗЋ=Math.Max(Ğ*ΛΪ,2);Θς=ΗЋ;if((ΔЌ==-1)||ΔЌ>=ΗЋ){ά έ=new ά(δ,Βπ,ΑΔ,ΑΧ,Ω.GetNaturalGravity());έ.Θύ(Ą,ă);Ψ.Add(έ);}}ά э(){if(Μ!=Λ.ή) return ǣ!=null?ǣ:Ο;return ΗΞ;}void ź(ά α,Ρ І){ΗΞ=α;if(ε==Ρ.И) ǐ=І;}bool ȡ(ά α,float σ,bool Λσ,out Vector3 γ)
{if(Λσ){Vector3I Δώ=new Vector3I((int) α.р.X,(int) α.р.Y,(int) α.р.Z);IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(Δώ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){γ=new Vector3();return false;}Vector3 Κή=ц(Ω,Ϊ.FatBlock.GetPosition()-δ);Vector3 Κέ=ц(Ω,Ϊ.FatBlock.WorldMatrix.Forward);γ=α.Ε-ΔΞ(α.ι,α.θ*-1,Κή)-ΔΞ(α.ι,α.θ*-1,Κέ)*σ;
return true;}else{γ=α.Ε;return true;}}Vector3 ĉ(int ΚΫ,int ƛ,bool ΛΛ){if(!ΛΛ&&Ηη) return Δδ;float Κά=((Ѝ-1f)/2f)-ΚΫ;float ΛΞ=((Ю-1f)/2f)-ƛ;Δδ=Ζ.Ε+Ζ.Ђ*Κά*ȃ+ě*-1*ΛΞ*ȝ;Ηη=true;return Δδ;}Vector3 Γμ(Vector3 Λή,float Δο){return Λή+(Ǔ*Δο);}ζ Γβ(){float Ь=-1;ζ ί=ζ.ȟ;
if(Μ!=Λ.ή){Ζβ();if(ε!=Ρ.я){Vector3 П=Ŋ(Ǔ,ě*-1,δ-Ζ.Ε);if(Math.Abs(П.X)<=(float)(Ѝ*ȃ)/2f&&Math.Abs(П.Y)<=(float)(Ю*ȝ)/2f){if(П.Z<=-1&&П.Z>=-ţ*2) return ζ.Αψ;if(П.Z>-1&&П.Z<λ*2) return ζ.ȟ;}if(ĥ(Ζ.Ε,ref Ь)) ί=ζ.ȟ;}if(Ο.μ){if(ĥ(Ο.Ε,ref Ь)) ί=ζ.Ȟ;for(int i=0;i<Ψ.Count;i++)
if(ĥ(Ψ[i].Ε,ref Ь)) ί=ζ.ċ;if(Vector3.Distance(δ,Ο.Ε)<dockDist*λ) ί=ζ.Ȟ;if(и(MyShipConnectorStatus.Connectable)!=null||и(MyShipConnectorStatus.Connected)!=null)
ί=ζ.Ȟ;}}else{Vector3 γ=new Vector3();IMyShipConnector Ȭ=и(MyShipConnectorStatus.Connected);if(Ο.μ){if(ĥ(Ο.Ε,ref Ь)) ί=ζ.Ȟ;if(ȡ(Ο,dockDist,true,out γ))
if(ĥ(γ,ref Ь)) ί=ζ.Ȟ;if(Ȭ!=null&&Vector3.Distance(Ȭ.GetPosition(),Ο.Ε)<5)
return ζ.ΓΧ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ο.Ε)>dockDist*λ&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*λ)
if(ĥ(Ψ[i].Ε,ref Ь)) ί=ζ.ċ;if(Ζ.μ){if(ĥ(Ζ.Ε,ref Ь)) ί=ζ.ȟ;if(ȡ(Ζ,dockDist,true,out γ))
if(ĥ(γ,ref Ь)) ί=ζ.ȟ;if(Ȭ!=null&&Vector3.Distance(Ȭ.GetPosition(),Ζ.Ε)<5)
return ζ.ΓΨ;}}return ί;}bool ĥ(Vector3 γ,ref float Ь){float σ=Vector3.Distance(γ,δ);if(σ<Ь||Ь==-1){Ь=σ;return true;}return false;}void ΔΘ(){if(Б>0){Κ="Setup error! Can't start";return;}if(Μ==Λ.ή){Ħ();return;}Ζ.Ε=δ;Ζ.ψ=Ω.GetNaturalGravity();Ζ.ι=Βπ;Ζ.θ=ΑΔ;Ζ.Ђ=ΑΧ;
Ǔ=Ǥ.WorldMatrix.Forward;ě=Ζ.θ;if(Ǔ==Ω.WorldMatrix.Down) ě=Ω.WorldMatrix.Backward;ƿ(true,true);Τ(Α.Ȇ);ΔΗ();}void ƿ(bool Ƕ,bool ΛΜ){if(ε==Ρ.я&&!Ƕ) return;bool ΚЋ=Ƕ||ε==Ρ.Γδ||Ѝ!=ђ||Ю!=ё||Ř!=ș;if(ΚЋ){if(ε!=Ρ.я){ε=Ρ.Αϋ;ĉ(Ё,ώ,ΛΜ);Κ="Job changed, lost progress";}Ř=ș;
Ѝ=ђ;Ю=ё;ώ=0;Ё=0;д="";Г=-1;ś=0;ę=0;к=0;в=0;ΔΟ(true);}}void ΓЍ(){й(δ,0);ǜ(Ą,true);}void ΔΗ(){Ȧ(ч.ΒΕ);ΓЍ();ƻ(ȳ,false);Ȉ(Εζ);ε=Ρ.И;ΕЈ(true);ǐ=ε;ν(Σ.ƶ);Ιυ();Ββ=true;Βτ=0;for(int i=ǁ.Count-1;i>=0;i--)
if(ƴ(ǁ[i],false)) Βτ++;if(Βτ>0) Κ="Started with damage";}void Є(){if(ε==Ρ.И){ε=Ρ.Ÿ;Κ="Job paused";}Τ(Α.ю);ǐ=ε;ΒΘ(false,0,0,0,0);ΔΛ();ΕЃ(new Vector3(),false);Ζψ();ΔΒ(ChargeMode.Auto);ΔΔ(false);Ζο(true);Č(Α.ю);ŋ(false,false,0,0);ǜ(ϊ,false);ǜ(ў,true);Ȉ(true);ΒЍ=false;Αύ();
ΑΓ=false;ΓЏ();Ββ=false;Αε=false;Αξ=false;if(ύ!=Σ.Main&&ύ!=Σ.Αω&&ύ!=Σ.ΓΤ&&ύ!=Σ.ΒΙ)
ν(Σ.Main);}void Ħ(){ζ Я=Γβ();if(Μ==Λ.ή){if(!Ζ.μ||!Ο.μ) return;ΔΗ();bool Δό=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(ƞ==Ρ.ū) Δό=true;if(ƞ==Ρ.Ŭ) Δό=false;if(Δό){ź(Ο,Ρ.ū);switch(Я){case ζ.ΓΧ:Τ(Α.Ĭ);break;case ζ.ċ:Τ(Α.х);break;case ζ.Ȟ:Τ(Α.ȅ);break;default:Τ(Α.Ǳ);break;
}}else{ź(Ζ,Ρ.Ŭ);switch(Я){case ζ.ΓΨ:Τ(Α.Ĭ);break;case ζ.ȟ:Τ(Α.ȅ);break;case ζ.ċ:Τ(Α.х);break;default:Τ(Α.Ǳ);break;}}}else{if(ε!=Ρ.Ÿ&&ε!=Ρ.Αϋ) return;bool Λφ=ε==Ρ.Αϋ;ΔΗ();bool ΔЃ=Ƶ(false)&&Ο.μ;switch(Я){case ζ.ȟ:Τ(ΔЃ?Α.Ĩ:Α.Ȇ);break;case ζ.ċ:Τ(ΔЃ?Α.Й:Α.х);break;case ζ.Ȟ:Τ(ΔЃ?Α.ȅ:Α.Ȩ);break;
case ζ.Αψ:{if(ę!=в||Λφ) Τ(Α.ȥ);else Τ(Α.ŷ);}break;default:break;}}}void Ň(){if(ε==Ρ.я&&!Ο.μ) return;if(Μ==Λ.ή&&(!Ζ.μ||!Ο.μ)) return;Κ="Move to job";ζ Я=Γβ();if(Μ==Λ.ή){ź(Ζ,Ρ.Ŭ);switch(Я){case ζ.ȟ:Τ(Α.ȅ);break;case ζ.ċ:Τ(Α.х);break;case ζ.ΓΨ:return;default:Τ(Α.Ǳ);break;
}Č(Α.Ĭ);}else{switch(Я){case ζ.ȟ:Τ(Α.х);break;case ζ.ċ:Τ(Α.х);break;case ζ.Ȟ:Τ(Α.Ȩ);break;case ζ.Αψ:Τ(Α.ȥ);break;default:break;}if(ε==Ρ.я) Č(Α.х);else Č(Α.Ĥ);ΒЍ=true;}ΓЍ();ν(Σ.ΔΚ);ƻ(ȳ,false);ǐ=Ρ.ΑЎ;}void ΑЃ(){if(!Ο.μ) return;Κ="Move home";ζ Я=Γβ();if(Μ==Λ.ή){ź(Ο,Ρ.ū);switch(Я)
{case ζ.ċ:Τ(Α.Й);break;case ζ.Ȟ:Τ(Α.ȅ);break;case ζ.ΓΧ:return;default:Τ(Α.Ǳ);break;}Č(Α.Ĭ);}else{if(и(MyShipConnectorStatus.Connected)!=null) return;if(и(MyShipConnectorStatus.Connectable)!=null){Τ(Α.ņ);Č(Α.Ȩ);return;}switch(Я){case ζ.ȟ:Τ(Α.Ĩ);break;case ζ.ċ:Τ(Α.Й);break;case ζ.Ȟ:Τ(Α.Й);break;
case ζ.Αψ:Τ(Α.Ȥ);break;default:break;}Č(Α.Ȩ);}ΓЍ();ν(Σ.ΔΚ);ƻ(ȳ,false);ǐ=Ρ.ΑЍ;}void Τ(Α ΕΟ){if(ΕΟ==Α.ю) Αθ=Α.ю;if(Αθ!=Α.ю&&Ϋ==Αθ&&ΕΟ!=Αθ){Є();return;}Д=true;Ϋ=ΕΟ;}void Č(Α І){this.Αθ=І;}ǰ ΛЂ(int ΑΡ,bool Ǿ){if(Ǿ){ǧ=null;Ё=0;ώ=0;}if(ș==ѐ.ƾ){int ŝ=ΑΡ+1;
ώ=(int) Math.Floor(К(ΑΡ,Ѝ));if(ώ%2==0) Ё=ΑΡ-(ώ*Ѝ);else Ё=Ѝ-1-(ΑΡ-(ώ*Ѝ));if(ώ>=Ю) return ǰ.Αχ;else return ǰ.ΑЅ;}else if(ș==ѐ.ΕЊ){if(ǧ==null) ǧ=new int[]{0,-1,0,0};int ΑΞ=(int) Math.Ceiling(Ѝ/2f);int ΑΝ=(int) Math.Ceiling(Ю/2f);int ΔЁ=(int) Math.Floor(Ѝ/2f);
int ΔЀ=(int) Math.Floor(Ю/2f);int ΑΣ=0;while(ǧ[2]<Math.Pow(Math.Max(Ѝ,Ю),2)){if(ΑΣ>200) return ǰ.ΙΒ;ΑΣ++;ǧ[2]++;if(-ΑΞ<Ё&&Ё<=ΔЁ&&-ΑΝ<ώ&&ώ<=ΔЀ){if(ǧ[3]==ΑΡ){this.Ё=Ё-1+ΑΞ;this.ώ=ώ-1+ΑΝ;return ǰ.ΑЅ;}ǧ[3]++;}if(Ё==ώ||(Ё<0&&Ё==-ώ)||(Ё>0&&Ё==1-ώ))
{int Λα=ǧ[0];ǧ[0]=-ǧ[1];ǧ[1]=Λα;}Ё+=ǧ[0];ώ+=ǧ[1];}}return ǰ.Αχ;}void ΚΓ(){if(Ϋ==Α.Ȇ){if(Д){Е=0;if(ę!=в) ś=0;ę=в;}if(Е==0){int Ǌ=ΘЄ(в);if(Ǌ<0){if(!Αό()){Τ(Α.Ȥ);return;}if(!Βή||(Ǌ==-2)!=Βέ){Βή=true;Βέ=Ǌ==-2;ΗΛ=!Βέ?δ:Ζ.Ε-Ǔ*λ+
Ζ.Ђ*(Ѝ*ȃ/2+ΒЎ/2+λ/2);}й(ΗΛ,10);Κ=Βέ?"Waiting for a free hole":"Waiting for the controller";return;}if(Βή) Κ="";Βή=false;if(Ǌ!=в){в=Ǌ;ś=0;ę=в;}ǰ ί=Ιώ(в);if(ί==ǰ.Αχ){ε=Ρ.Γδ;Κ="Job done";if(ŀ&&Ο.μ){Τ(Α.Ĩ);Č(Α.Ȩ);ǐ=Ρ.ΑЍ;}else{Τ(Α.ű);Č(Α.Ĥ);ǐ=Ρ.ΑЎ;}
return;}if(ί==ǰ.ΑЅ){Е=1;ǜ(ϊ,true);ѕ=ĉ(Ё,ώ,true);й(ѕ,10);н(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}}else{if(Г>=0&&(Ζα()?Ȏ>=0&&Ȏ!=Г:Γσ(Г,true))){Е=0;return;}if(г<wpReachedDist){Τ(Α.ŷ);return;}}}if(Ϋ==Α.ŷ){if(Д){ǜ(ϊ,true);Ȉ(false);ѕ=ĉ(Ё,ώ,false);й(Γμ(ѕ,0),0);н(Ζ.θ,Ζ.ι,Ζ.Ђ,false);
Е=1;є=0;ф=0;ΒΦ=0;ΔЎ=-1;ţ=п;ΔЇ=true;}if(!Αό()){Τ(Α.Ȥ);return;}if(Ƶ(true)){ΘΓ=ć("","ORE",Щ.Ń);if((е==В.Γε||е==В.Αώ||
е==В.Γτ||е==В.ΑЈ)&&Μ!=Λ.Ǯ)
Τ(Α.Ű);else if((е==В.ΕЏ||е==В.Γί)&&Μ!=Λ.Ǯ)
Τ(Α.ΔΠ);else
Τ(Α.Ȥ);return;}к=Vector3.Distance(δ,ѕ);if(к>ś){ś=к;ΔЇ=false;}if(Μ==Λ.Ǯ&&ΖΛ()==MyDetectedEntityType.SmallGrid)
ф+=2;else ф-=2;ф=Math.Max(100,Math.Min(400,ф));if(Е>0&&Е<ф){if(к>є){if(ф>150) є=к;else є=(float) Math.Ceiling(к);Е=1;}else Е++;}else{if(Е>0){Κ="Ship stuck! Retrying";є=к;Е=0;ŋ(false,true,0,λ*sensorRange);}й(Γμ(ѕ,Math.Max(0,є-λ)),Γο(false));if(г<=wpReachedDist/2){Е=1;є=0;}
return;}ŋ(false,true,λ*sensorRange,0);Vector3 ΚΠ=ѕ+Ǔ*к;bool Ηφ=false;if(Vector3.Distance(ΚΠ,δ)>0.3f){Vector3 Κβ=ѕ+Ǔ*(к+0.1f);й(Κβ,4);Ηφ=true;}else{float ǿ=Γο(true);Vector3 ΗΣ=Γμ(ѕ,Math.Max(п+1,к+1));ȇ(true,false,false,ΗΣ,ΗΣ-ѕ,ǿ,ǿ);}bool Δπ=false;if(џ==Ǚ.ΓΪ||џ==Ǚ.ΓΩ)
{if(!Ηφ){float ΕΣ=0;foreach(IMyTerminalBlock Ȝ in ϊ)
ΕΣ+=ΘЍ(Ȝ,"","",џ==Ǚ.ΓΩ?new string[]{"STONE"}:null);if(ΕΣ>ΔЎ||к<п||ΔЇ){ΔΨ=0;ΒΦ=к;ţ=(int)(Math.Max(ţ,ΒΦ)+λ/2);}else{Δπ=к-ΒΦ>2&&ΔΨ>=20;ΔΨ++;}ΔЎ=ΕΣ;}}else Δπ=к>=ţ;if(ę!=в){Τ(Α.ȥ);к=0;return;}if(Δπ){в++;Τ(Α.ȥ);к=0;return;}}if(Ϋ==Α.ȥ||Ϋ==Α.Ȥ||Ϋ==Α.Ű)
{if(Д){ѕ=ĉ(Ё,ώ,false);н(Ζ.θ,Ζ.ι,Ζ.Ђ,false);ǜ(ϊ,Ɔ);Ȉ(false);є=Vector3.Distance(δ,ѕ);ŋ(false,true,0,λ*sensorRange);}й(ѕ,Γο(false));if(Vector3.Distance(δ,ѕ)>=є+5){ǜ(ϊ,false);Ȉ(true);Κ="Can´t return!";}if(г<wpReachedDist){if(Ϋ==Α.ȥ&&ΒЍ) Τ(Α.ű);if(Ϋ==Α.ȥ) Τ(Α.Ȇ);
if(Ϋ==Α.Ű) Τ(Α.ΑЂ);if(Ϋ==Α.Ȥ){if(Ο.μ) Τ(Α.Ĩ);else{Є();Ň();Κ="Can´t return, no dock found";}}return;}}if(Ϋ==Α.Й||Ϋ==Α.х){if(Ϋ==Α.х&&ε==Ρ.И&&Μ!=Λ.ή){if(!Αό()||Ƶ(true)){Τ(Α.Й);return;}}bool ΕΩ=false;bool ΓΝ=false;bool ΖІ=false;float ƀ=0;bool ΘΒ=false;ά έ=null;
if(Д){if(Ϋ==Α.Й||Μ==Λ.ή){if(Μ!=Λ.ή){if(č(1)) ΓΦ();else{ǣ=null;ā=0;ΑΓ=!Γή(1);}}ά α=э();ο=new ΓЌ();ο.η=α;ο.ΑΑ=followPathDock*λ;ο.Εψ=useDockDirectionDist*λ;ο.ƀ=10;ο.Ƥ.Add(α.Ε);Vector3 ſ=new Vector3();if(ȡ(α,dockDist*λ,true,out ſ))
ο.Ƥ.Add(ſ);else ο.ΑΑ*=1.5f;if(Μ==Λ.ή){if(α==Ο) ο.Βν=Ζ.Ε;if(α==Ζ) ο.Βν=Ο.Ε;ο.Δω=dockDist*λ*1.1f;}}else if(Ϋ==Α.х){ο=new ΓЌ();ο.η=Ζ;ο.ΑΑ=followPathJob*λ;ο.Εψ=useJobDirectionDist*λ;ο.ƀ=10;ο.Βν=Ο.Ε;ο.Δω=dockDist*λ*1.1f;ο.Ƥ.Add(Ζ.Ε);if(ε==Ρ.я){if(!Ο.μ||Ψ.Count==0){Є();return;}
float Κι=Vector3.Distance(Ψ.First().Ε,Ο.Ε);float Κκ=Vector3.Distance(Ψ.Last().Ε,Ο.Ε);if(Κι<Κκ) ο.η=Ψ.Last();else ο.η=Ψ.First();}}Αί=new Vector3();ΘΒ=!Ιί(δ);ǜ(ϊ,false);Ȉ(true);л=-1;double ȍ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ο.Βν)<=ο.Δω) continue;
double σ=Vector3.Distance(Ψ[i].Ε,δ);if(ȍ==-1||σ<ȍ){л=i;ȍ=σ;}}ȗ=ΙΞ(ο.η.Ε,л);ŏ=null;}if(Ϋ==Α.Й&&Μ!=Λ.ή&&!Д&&!ƽ()){ά Δέ=э();ΑΓ=!Γή(1);if(э()!=Δέ){Τ(Α.Й);return;}}if(Ϋ==Α.Й&&Μ!=Λ.ή&&!Д&&č(1)&&ΓΦ()){Τ(Α.Й);return;}bool Ητ=false;if(Ϋ==Α.Й&&!Д&&л>=0&&л<Ψ.Count)
{Vector3 ǌ=Ψ[л].Ε+ΖΞ(л,ȗ);int ΕΠ=л+ȗ;Vector3 ŝ=ȗ!=0&&ΕΠ>=0&&ΕΠ<Ψ.Count?Ψ[ΕΠ].Ε:ο.η.Ε;if(ΙΩ(ǌ,ŝ)){if(!Βζ){Βζ=true;ΗΨ=ŵ(ǌ)?δ:ǌ;}й(ΗΨ,0);н(ο.η.θ,ο.η.ι,ο.η.Ђ,false);Κ="Waiting for dock clearance";return;}Βζ=false;Ητ=č(1)&&(ŵ(δ)||ŵ(ŝ));}Θώ(Ψ,ȗ,ο.Ƥ,ο.ΑΑ,Д,ref Е);
for(int i=0;i<ο.Ƥ.Count;i++){float σ=Vector3.Distance(δ,ο.Ƥ[i]);if(σ<=ο.ΑΑ) ΕΩ=true;if(σ<=ο.Εψ) ΓΝ=true;}if(Ητ) ΓΝ=true;if(ΓΝ) ƀ=ο.ƀ;float ΛЇ=ŏ!=null?ŏ.Ш:Ğ;float Λό=(float) Math.Max(Ğ*0.1f*λ,wpReachedDist);if((г<Λό)||Д){if(!Д) л+=ȗ;if(ȗ==0||л>Ψ.Count-1||л<0)
ΕΩ=true;else{Εϋ=ȗ>0?Ψ.Count-1-л:л;έ=Ψ[л];ŏ=έ;if(л>=1&&л<Ψ.Count-1)
Αί=έ.Ε-Ψ[л-ȗ].Ε;else ŏ=null;Ѓ=έ.Ε+ΖΞ(л,ȗ);ΖІ=true;}}if(ΓΝ)
н(ο.η.θ,ο.η.ι,ο.η.Ђ,false);else if(ΘΒ)
ΓΥ(ο.η.θ,10,true);else if(ΖІ&&έ!=null){if(ȗ>0) н(έ.θ,έ.ι,έ.Ђ,90,false);else н(έ.θ,-έ.ι,-έ.Ђ,90,false);}ȇ(true,false,true,Ѓ,Αί,ŏ==null?0:ŏ.Ш,ƀ);if(ΕΩ){Εϋ=0;if(Ϋ==Α.Й&&!ΘЂ()){й(Ѓ,0);return;}if(Ϋ==Α.Й&&ΑΓ){й(Ѓ,0);Κ="Waiting for a free dock";return;}
if(Ϋ==Α.Й||Μ==Λ.ή){Τ(Α.ȅ);return;}if(Ϋ==Α.х&&ΒЍ){Τ(Α.ű);return;}if(Ϋ==Α.х){Τ(Α.Ȇ);return;}}}if(Ϋ==Α.ȅ||Ϋ==Α.ƺ){if(Μ!=Λ.ή&&ƽ()){if(!č(1)){Τ(Α.Й);return;}}else if(Μ!=Λ.ή){ά Δέ=э();if(!Γή(2)||э()!=Δέ){Τ(Α.Й);return;}}ά α=э();if(Д){if(!ȡ(α,dockDist*λ,true,out Ѓ))
{Ȧ(ч.ư);Є();return;}й(Ѓ,0);ΓΥ(α.θ,90,true);}if(г<followPathDock*λ&&г!=-1){й(Ѓ,10);н(α.θ,α.ι,α.Ђ,false);}if(и(MyShipConnectorStatus.Connectable)!=null||и(MyShipConnectorStatus.Connected)!=null){Τ(Α.ņ);return;}if(г<wpReachedDist/2&&г!=-1){Τ(Α.ł);return;}}if(Ϋ==Α.ł||Ϋ==Α.Ĥ)
{if(Д){if(Ϋ==Α.ł){ά α=э();if(!ȡ(α,dockDist*λ,true,out Ѓ)){Ȧ(ч.ư);Є();return;}ȇ(true,true,false,Ѓ,0);н(α.θ,α.ι,α.Ђ,10,false);}if(Ϋ==Α.Ĥ){н(Ζ.θ,Ζ.ι,Ζ.Ђ,0.5f,false);Ѓ=Ζ.Ε;ȇ(true,true,false,Ѓ,0);}}if(Βφ){ΒΘ(false,0,0,0,0);if(Ϋ==Α.ł) Τ(Α.ņ);if(Ϋ==Α.Ĥ) Є();return;
}}if(Ϋ==Α.ņ){if(и(MyShipConnectorStatus.Connected)!=null){if(Μ==Λ.ή) Τ(Α.Ĭ);else Τ(Α.Ȩ);return;}ά α=э();if(Д){ф=0;Κλ=DateTime.Now;Е=0;н(α.θ,α.ι,α.Ђ,false);}Vector3I Δώ=new Vector3I((int) α.р.X,(int) α.р.Y,(int) α.р.Z);IMySlimBlock ΗΘ=Me.CubeGrid.GetCubeBlock(Δώ);float ΗΫ=dockingSpeed;
float ΚЅ=dockingSpeed*5;float ΖЄ=Math.Max(1.5f,Math.Min(5f,λ*0.15f));if(!ȡ(α,0,true,out Ѓ)||!ȡ(α,ΖЄ,true,out Αί)||
ΗΘ==null||!ΗΘ.FatBlock.IsFunctional){Ȧ(ч.ư);Є();return;}if(ф==1||(Vector3.Distance(δ,Ѓ)<=ΖЄ*1.1f&&!Д))
ф=1;else{Vector3 Λκ=ц(Ω,Αί-δ);Vector3 Δύ=ц(Ω,Ω.GetNaturalGravity());float ΛΟ=Αϊ(Λκ,Δύ,null);ΗΫ=Math.Min(ΚЅ,ΛΟ);}ȇ(true,false,false,Ѓ,Ѓ-δ,dockingSpeed,ΗΫ);if(Д) є=(float) г;IMyShipConnector Δλ=и(MyShipConnectorStatus.Connectable);if(Δλ!=null){ȇ(false,false,false,Ѓ,0);
if(Е>0) Е=0;Е--;if(Е<-5){Δλ.Connect();if(Δλ.Status==MyShipConnectorStatus.Connected){if(Μ==Λ.ή) Τ(Α.Ĭ);else Τ(Α.Ȩ);ΔΛ();ƻ(ȳ,true);return;}}}else{float ΗΧ=(float) Math.Round(г,1);if(ΗΧ<є){Е=-1;є=ΗΧ;}else Е++;if(Е>20){Τ(Α.ƺ);return;}}}if(Ϋ==Α.Ȩ||Ϋ==Α.Ĭ||
Ϋ==Α.ΒΟ||Ϋ==Α.ΑЀ||Ϋ==Α.Ņ){bool ΔΦ=false,ΔΧ=false;if(Μ==Λ.ή){if(э()==Ο) ΔΦ=true;else if(э()==Ζ) ΔΧ=true;}if(Д){Αε=false;if(и(MyShipConnectorStatus.Connected)==null){Τ(Α.Ǳ);return;}ΔΛ();if(ΔΦ) ΒΜ(Ј.ǵ);if(ΔΧ) ΒΜ(Љ.ǵ);ŧ=false;Ď=false;ĕ=false;Απ=false;}if(и(MyShipConnectorStatus.Connected)==null){Є();Ȧ(ч.Ζά);return;}
if(ε!=Ρ.И||Ǒ==-1||Ȋ==њ.None) Ď=true;else if(Ȫ>=100f) Ď=true;else if(Ȫ<=99f) Ď=false;if(ε!=Ρ.И||ǒ==-1||Ǡ.Count==0) ĕ=true;else if(Ė>=100f) ĕ=true;else if(Ė<=99) ĕ=false;if(ε!=Ρ.И||с==-1||ļ.Count==0) Απ=true;else Απ=Ũ>=с;ż љ=null;if(ΔΦ) љ=Ј;
if(ΔΧ) љ=Љ;if(љ!=null&&(љ.Т==ς.ΒΑ||љ.Т==ς.ΑБ)) Ď=true;if(љ!=null&&(љ.Т==ς.ΓЄ)) if(!ŧ) Ď=false;if(љ!=null&&(љ.Т==ς.ΓЈ||љ.Т==ς.ΓІ)) ĕ=true;if(љ!=null&&(љ.Т==ς.ΓЇ)) if(!ŧ) ĕ=false;if(ė){ChargeMode ΖБ=Ď?ChargeMode.Auto:ChargeMode.Recharge;if(љ!=null&&(љ.Т==ς.ΑБ||љ.Т==ς.ΒΑ))
ΖБ=ChargeMode.Discharge;ΔΒ(ΖБ);ΔΔ(!ĕ);}if(!ŧ){if(Μ==Λ.ή)
ŧ=ε!=Ρ.И||ΘЃ(Д,true)||Αξ;else
ŧ=ε!=Ρ.И||Ζέ();}else{if(!Ď) Τ(Α.Ņ);if(!ĕ) Τ(Α.ΑЀ);if(!Απ) Τ(Α.ΒΟ);Д=false;}if(ŧ&&Ď&&ĕ&&Απ){ΔΒ(ChargeMode.Auto);ΔΔ(false);if(ε==Ρ.И){if(Μ==Λ.ή){if(э()==Ο) ΒΜ(Ј.Ǻ);else if(э()==Ζ) ΒΜ(Љ.Ǻ);if(э()==Ο) ź(Ζ,Ρ.Ŭ);else ź(Ο,Ρ.ū);}}Τ(Α.Ǳ);return;}}if(Ϋ==Α.Ǳ){if(Ασ){if(!ΕЌ()) return;
Ασ=false;Τ(Α.Ǳ);return;}if(Д){if(!ΕЌ()){Ασ=true;return;}IMyShipConnector Ȭ=и(MyShipConnectorStatus.Connected);if(Ȭ==null){Τ(Α.х);return;}IMyShipConnector κ=Ȭ.OtherConnector;Ζξ(Ȭ,false);ƻ(ȳ,false);ά α=null;ά ǎ=Μ==Λ.ή?Ο:э();if(Vector3.Distance(Ȭ.GetPosition(),ǎ.Ε)<5f&&Ο.μ) α=ǎ;
if(Vector3.Distance(Ȭ.GetPosition(),Ζ.Ε)<5f&&Ζ.μ) α=Ζ;if(α!=null){if(!ȡ(α,dockDist*λ,true,out Ѓ)){Ȧ(ч.ư);Є();return;}й(Ѓ,5);н(α.θ,α.ι,α.Ђ,false);}else й(δ+κ.WorldMatrix.Forward*dockDist*λ,5);if(ε==Ρ.И) Ȧ(ч.ΒΕ);}if(г<wpReachedDist){ǜ(ў,true);Τ(Α.х);return;}}if(Ϋ==Α.ű)
{if(Д){Ȉ(true);ǜ(ϊ,false);Ѓ=Ζ.Ε;й(Ѓ,20);н(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(г<wpReachedDist/2){Τ(Α.Ĥ);return;}}if(Ϋ==Α.Ĩ){if(Д){Ȉ(true);ǜ(ϊ,false);int Ь=-1;double ȍ=-1;for(int i=Ψ.Count-1;i>=0;i--){double σ=Vector3.Distance(Ψ[i].Ε,δ);if(ȍ==-1||σ<ȍ){Ь=i;ȍ=σ;}}
if(Ь==-1){Τ(Α.Й);return;}Ѓ=Ψ[Ь].Ε;й(Ѓ,10);н(Ζ.θ,Ζ.ι,Ζ.Ђ,false);}if(г<wpReachedDist){Τ(Α.Й);return;}}if(Ϋ==Α.ΑЂ){bool Ѐ=false;if(Д){Ȉ(true);if((е==В.Γε||е==В.Αώ)&&ΑЊ()&&
Ů(Ǔ,Ω.GetNaturalGravity())<25&&Ѝ>=2&&Ю>=2){Vector3 ΑΕ=δ;if(Ё>0&&ώ<Ю-1) ΑΕ=ĉ(Ё-1,ώ+1,true);else if(Ё<Ѝ-1&&ώ<Ю-1) ΑΕ=ĉ(Ё+1,ώ+1,true);else if(Ё<Ѝ-1&&ώ>0) ΑΕ=ĉ(Ё+1,ώ-1,true);else if(Ё>0&&ώ>0) ΑΕ=ĉ(Ё-1,ώ-1,true);else Ѐ=true;
if(!Ѐ) й(ΑΕ,10);}else Ѐ=true;}if(г<wpReachedDist/2) Ѐ=true;if(Ѐ){Τ(Α.ΒΞ);return;}}if(Ϋ==Α.ΔΠ||Ϋ==Α.ΒΞ){if(Д){ȇ(true,true,false,δ,0);ǜ(ϊ,false);Ȉ(true);Е=-1;ф=е==В.Γτ||е==В.ΑЈ?0:-1;}bool œ=!Αό();int ΓΖ=ć("STONE","ORE",Щ.Ń);if(е==В.Γί||е==В.ΑЈ||е==В.Αώ)
ΓΖ+=ć("ICE","ORE",Щ.Ń);bool Κυ=ΓΖ>0;bool Δψ=false;if(ф>=0){float Ž=(float) Math.Sin(ΕБ(ф))*ȃ/3f;float ΚΣ=(float) Math.Cos(ΕБ(ф))*ȝ/3f;Vector3 ΚΪ=ĉ(Ё,ώ,true)+ΔΞ(Ǔ,ě*-1,new Vector3(Ž,ΚΣ,0));й(ΚΪ,0.3f);if(г<Math.Min(ȃ,ȝ)/10f) ф+=5f;if(ф>=360) ф=0;}if(Е==-1||ΓΖ<Е){Е=ΓΖ;є=0;}
else{є++;if(є>50) Δψ=true;}if(!Κυ||œ||Δψ){if(!œ){int Λο=ć("","ORE",Щ.Ń);if(Ƶ(true)) œ=true;else if(100-(К(Λο,ΘΓ)*100)<minEjection) œ=true;else Ȧ(ч.ΒΕ);}if(Δψ&&œ) Κ="Ejection failed";if(Ϋ==Α.ΒΞ){if(œ){if(Ο.μ) Τ(Α.Ĩ);else{Є();Ň();Κ="Can´t return, no dock found";}}
else Τ(Α.Ȇ);}else if(œ) Τ(Α.Ȥ);else Τ(Α.ŷ);return;}}Д=false;}void Θώ(List<ά>Ł,int ξ,List<Vector3>Αλ,float ȁ,bool Ǿ,ref int ǳ){if(Ǿ){for(int i=0;i<Ł.Count;i++) Ł[i].Ш=0;ǳ=-1;return;}if(ξ==0) return;int Ǩ=ξ*-1;if(ǳ==-1) ǳ=Ǩ>0?1:Ł.Count-2;int ΑΣ=0;while(ǳ>=1&&ǳ<Ł.Count-1)
{if(ΑΣ>50) return;ΑΣ++;try{if((Ǩ<0&&ǳ>=1)||(Ǩ>0&&ǳ<=Ł.Count-2)){ά π=Ł[ǳ];bool ΗА=false;for(int j=0;j<Αλ.Count;j++)
if(Vector3.Distance(π.Ε,Αλ[j])<=ȁ){ΗА=true;break;}if(!ΗА){ά ΒЉ=Ł[ǳ-Ǩ];ά ŝ=Ł[ǳ+Ǩ];Vector3 Λλ=π.Ε-ŝ.Ε;Vector3 Ερ=ΒЉ.Ε-π.Ε;Vector3 ΛΚ=π.Ε+Vector3.Normalize(Λλ)*Ερ.Length();Vector3 Κη=ΒЉ.Ε-ΛΚ;Vector3 Βό=Ŋ(ξ>0?π.ι:π.ι*-1,π.θ*-1,Κη);Vector3 ΚЄ=Ŋ(ξ>0?π.ι:π.ι*-1,π.θ*-1,Ερ);
Vector3 Βώ=Ŋ(ξ>0?π.ι:π.ι*-1,π.θ*-1,π.ψ);π.Ш=(float) Math.Sqrt(Math.Pow(ΒЉ.Ш,2)+Math.Pow(Αϊ(-ΚЄ,Βώ,π),2));for(int j=0;j<Αλ.Count;j++)
if(Vector3.Distance(ΒЉ.Ε,Αλ[j])<=ȁ){Vector3 ΒϏ=Ŋ(ξ>0?π.ι:π.ι*-1,π.θ*-1,Αλ[j]-π.Ε);float Λΰ=Αϊ(-ΒϏ,Βώ,π);π.Ш=Math.Min(π.Ш,Λΰ)/2f;}if(Βό.Length()==0) Βό=new Vector3(0,0,1);Vector3 ΚЃ=Ŋ(π.ι,π.θ*-1,π.ψ);float Λε=Ʋ(-Βό,ΚЃ,π);float ΒΠ=К(Λε,ť);float Εξ=(float) Math.Sqrt(К(Βό.Length()*1.0f,0.5f*ΒΠ));
π.Ш=Math.Min(π.Ш,(Ερ.Length()/Εξ)*ȉ);}}}catch{return;}ǳ+=Ǩ;}ǳ=-1;}void ΔΟ(bool Ǿ){if(Ǿ){ΑΤ=0;return;}if(Μ==Λ.ή) return;float Ѐ=в*Math.Max(1,п);if(ę==в) Ѐ+=Math.Min(п,к);float Λξ=Ѝ*Ю*Math.Max(1,п);ΑΤ=Math.Max(ΑΤ,(float) Math.Min(Ѐ/Λξ*100.0,100));}MyDetectedEntityType ΖΛ()
{try{if(Ȣ(ω,true)&&!ω.LastDetectedEntity.IsEmpty()) return ω.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γο(bool Ȓ){if(Μ==Λ.Ǯ&&ΖΛ()==MyDetectedEntityType.None&&!ƴ(ω,true))
return fastSpeed;else return Ȓ?Ʈ:ƭ;}void ΙЋ(){float ΗЈ=0,ΗΠ=0,ŗ=0,ΕΤ=0;for(int i=0;i<ĭ.Count;i++){IMyBatteryBlock Ǉ=ĭ[i];if(!Ȣ(Ǉ,true)) continue;ΗЈ+=Ǉ.MaxStoredPower;ΗΠ+=Ǉ.CurrentStoredPower;ŗ+=Ǉ.CurrentInput;ΕΤ+=Ǉ.CurrentOutput;}Ȫ=(float) Math.Round(К(ΗΠ,ΗЈ)*100,1);
if(ŗ>=ΕΤ) Ȋ=њ.Ņ;else Ȋ=њ.ΖΑ;if(ŗ==0&&ΕΤ==0||Ȫ==100.0) Ȋ=њ.ю;if(ĭ.Count==0) Ȋ=њ.None;}void ΙБ(){float Ηΰ=0;for(int i=0;i<Ǡ.Count;i++){IMyGasTank Θΰ=Ǡ[i];if(!Ȣ(Θΰ,true)) continue;Ηΰ+=(float) Θΰ.FilledRatio;}Ė=К(Ηΰ,Ǡ.Count)*100f;}void ΚΜ(){Ũ=0;try{for(int i=0;i<ļ.Count;i++)
{IMyReactor ΕΪ=ļ[i];if(!Ȣ(ΕΪ,true)) continue;List<MyInventoryItem>Ќ=new List<MyInventoryItem>();ΕΪ.GetInventory(0).GetItems(Ќ);float Ћ=0;for(int j=0;j<Ќ.Count;j++){MyInventoryItem Ϗ=Ќ[j];if(Ϗ.Type.SubtypeId.ToUpper()=="URANIUM"&&Ϗ.Type.TypeId.ToUpper().Contains("_INGOT"))
Ћ+=(float) Ϗ.Amount;}if(Ћ<Ũ||i==0){Ũ=Ћ;ΕΕ=ΕΪ.CustomName;}}}catch(Exception e){Ę=e;}}void ΙЌ(){Ȗ=0;ȏ=0;try{for(int i=0;i<Ƅ.Count;i++){IMyTerminalBlock Ϊ=Ƅ[i];if(!Ȣ(Ϊ,true)) continue;ȏ+=(float) Ϊ.GetInventory(0).CurrentVolume;Ȗ+=(float) Ϊ.GetInventory(0).MaxVolume;
}Βϋ=(float) Math.Min(Math.Round(К(ȏ,Ȗ)*100,1),100.0);}catch(Exception e){Ę=e;}}void ΚΑ(){try{Ǹ.Clear();for(int i=0;i<Ƅ.Count;i++){IMyTerminalBlock Ϊ=Ƅ[i];if(!Ȣ(Ϊ,true)) continue;ΕЂ(Ϊ,Щ.Ŵ);}if(е!=В.ΓЃ){for(int i=0;i<Ŏ.Count;i++){IMyTerminalBlock Ϊ=Ŏ[i];if(!Ȣ(Ϊ,true)) continue;
ΕЂ(Ϊ,Щ.Ιέ);}}ΙЁ(Ǹ);}catch(Exception e){Ę=e;}}void ΘЁ(){if(Ββ){if(ΖΚ().Count>Βτ){Ββ=false;if(і!=ь.Ƴ){Є();if(і==ь.Γκ) Ň();if(і==ь.ΒΔ)
if(Ο.μ) ΑЃ();else Ň();}Κ="Damage detected";}}}bool Αό(){if(!Βυ) return true;if(ε==Ρ.И){if(Ǒ>0&&Ȋ!=њ.None)
if(Ȫ<=Ǒ){Κ="Low energy! Move home";return false;}if(с>0&&ļ.Count>0)
if(Ũ<=с){Κ="Low fuel: "+ΕΕ;return false;}if(ǒ>0&&Ǡ.Count>0)
if(Ė<=ǒ){Κ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΖΚ(){List<IMyTerminalBlock>Ő=new List<IMyTerminalBlock>();for(int i=0;i<ǁ.Count;i++){IMyTerminalBlock Ϊ=ǁ[i];if(ƴ(Ϊ,false)) Ő.Add(Ϊ);}return Ő;}bool ƴ(IMyTerminalBlock Ϊ,bool Εθ){return(!Ȣ(Ϊ,Εθ)||!Ϊ.IsFunctional);
}bool Ȣ(IMyTerminalBlock Ϊ,bool Εθ){if(Ϊ==null) return false;try{IMyCubeBlock Ηζ=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(Εθ) return Ηζ==Ϊ;else return Ηζ.GetType()==Ϊ.GetType();}catch{return false;}}float ΘЍ(IMyTerminalBlock Ϊ,String ǘ,String ΘΧ,String[]Ȑ){float ȭ=0;for(int i=0;i<Ϊ.InventoryCount;i++)
{IMyInventory Ƌ=Ϊ.GetInventory(i);List<MyInventoryItem>Ќ=new List<MyInventoryItem>();Ƌ.GetItems(Ќ);foreach(MyInventoryItem Ϗ in Ќ){if(Ȑ!=null&&(Ȑ.Contains(Ϗ.Type.TypeId.ToUpper())||Ȑ.Contains(Ϗ.Type.SubtypeId.ToUpper())))
continue;if((ǘ==""||Ϗ.Type.TypeId.ToUpper()==ǘ)&&(ΘΧ==""||Ϗ.Type.SubtypeId.ToUpper()==ΘΧ))
ȭ+=(float) Ϗ.Amount;}}return ȭ;}о ΙΔ(String ΰ,String χ,Щ Я,bool Κγ){ΰ=ΰ.ToUpper();χ=χ.ToUpper();for(int i=0;i<Ǹ.Count;i++){о Ϗ=Ǹ[i];if(Ϗ.ΰ.ToUpper()==ΰ&&Ϗ.χ.ToUpper()==χ&&(Ϗ.Ə==Я||Я==Щ.Ń))
return Ϗ;}о ί=null;if(Κγ){ί=new о(ΰ,χ,0,Я);Ǹ.Add(ί);}return ί;}int ć(String ΰ,String χ,Щ Я){return ć(ΰ,χ,Я,null);}int ć(String ΰ,String χ,Щ Я,String[]Ȑ){int ȭ=0;ΰ=ΰ.ToUpper();χ=χ.ToUpper();for(int i=0;i<Ǹ.Count;i++){о Ϗ=Ǹ[i];if(Ȑ!=null&&Ȑ.Contains(Ϗ.ΰ.ToUpper())) continue;
if((ΰ==""||Ϗ.ΰ.ToUpper()==ΰ)&&(χ==""||Ϗ.χ.ToUpper()==χ)&&(Ϗ.Ə==Я||Я==Щ.Ń))
ȭ+=Ϗ.Ћ;}return ȭ;}void ΕЂ(IMyTerminalBlock Ϊ,Щ Я){for(int i=0;i<Ϊ.InventoryCount;i++){List<MyInventoryItem>Ќ=new List<MyInventoryItem>();Ϊ.GetInventory(i).GetItems(Ќ);for(int j=0;j<Ќ.Count;j++)
ΙΔ(Ќ[j].Type.SubtypeId,Ќ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Я,true).Ћ+=(int) Ќ[j].Amount;}}void ΙЁ(List<о>Ў){for(int i=Ў.Count-1;i>0;i--)
for(int j=0;j<i;j++){о a=Ў[j];о b=Ў[j+1];if(a.Ћ<b.Ћ) Ў.Move(j,j+1);}}bool Ƶ(bool ΛЄ){if(ģ&&Μ!=Λ.ή)
if(ж!=-1&&ť>=ж){Κ="Ship too heavy";return true;}if(Βϋ>=ȕ||Αε){Αε=false;Κ="Ship is full";return true;}return false;}bool Ζέ(){String[]Ȑ=null;if(!Ģ) Ȑ=new string[]{"ICE"};if(Μ==Λ.Ƹ) return ć("","ORE",Щ.Ŵ,Ȑ)==0;if(Μ==Λ.Ǯ) return ć("","COMPONENT",Щ.Ŵ,Ȑ)==0;else return ć("","",Щ.Ŵ,Ȑ)==0;
}bool Ιΰ(){List<IMyCockpit>Δκ=new List<IMyCockpit>();υ.GetBlocksOfType(Δκ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δκ.Count;i++)
if(Δκ[i].IsUnderControl) return true;return false;}bool ΘЃ(bool Ǿ,bool ΒА){IMyShipConnector Р=и(MyShipConnectorStatus.Connected);if(Р==null) return false;if(Vector3.Distance(Ο.Ε,Р.GetPosition())<5)
return ΕЍ(Ј,Ǿ,ΒА);if(Vector3.Distance(Ζ.Ε,Р.GetPosition())<5)
return ΕЍ(Љ,Ǿ,ΒА);return false;}bool ΕЍ(ż ȫ,bool Ǿ,bool ΒА){if(Ǿ) ȫ.Ιπ();ȫ.ΚΘ();bool ј=false;String ǖ="";switch(ȫ.Т){case ς.ΓЅ:{ǖ="Waiting for command";ј=false;break;}case ς.Ζθ:{ǖ="Waiting for cargo";ј=Ƶ(true);break;}case ς.Ζη:{ǖ="Unloading";ј=Ζέ();break;}case ς.ΓЊ:{ј=true;break;}
case ς.ΓЄ:{ǖ="Charging batteries";ј=Ȫ>=100f;break;}case ς.ΒΑ:{ǖ="Discharging batteries";ј=Ȫ<=25f;break;}case ς.ΑБ:{ǖ="Discharging batteries";ј=Ȫ<=0f;break;}case ς.ΓЇ:{ǖ="Filling up hydrogen";ј=Ė>=100f;break;}case ς.ΓЈ:{ǖ="Unloading hydrogen";ј=Ė<=25f;break;}
case ς.ΓІ:{ǖ="Unloading hydrogen";ј=Ė<=0f;break;}case ς.ΓЉ:{bool Ησ=Ιΰ();if(!Ησ) ȫ.ΕΧ=true;ј=ȫ.ΕΧ&&Ησ;ǖ="Waiting for passengers";break;}}if(!ј) ȫ.Γέ(true);if(ј&&ȫ.Γρ()){ј=ȫ.Γέ(false);ǖ="Undocking in: "+ΑЄ((int) Math.Max(0,ȫ.Ǵ-ȫ.Ĵ));}if(ΒА) Θπ=ǖ;return ј;}void Θφ()
{if(!ƌ) return;if(ϊ.Count<=1) return;float Θθ=0,Θη=0;for(int i=0;i<ϊ.Count;i++){IMyTerminalBlock Ȝ=ϊ[i];if(ƴ(Ȝ,true)) continue;Θθ+=(float) Ȝ.GetInventory(0).MaxVolume;Θη+=(float) Ȝ.GetInventory(0).CurrentVolume;}float ΖЇ=(float) Math.Round(К(Θη,Θθ),5);for(int ş=0;ş<Math.Max(1,Math.Floor(ϊ.Count/10f));ş++)
{float ΔЅ=0,ΕΔ=0;float Ημ=0,ΗЅ=0;IMyTerminalBlock Ē=null,ĵ=null;for(int i=0;i<ϊ.Count;i++){IMyTerminalBlock Ȝ=ϊ[i];if(ƴ(Ȝ,true)) continue;float ΕΛ=(float) Ȝ.GetInventory(0).MaxVolume;float Βξ=К((float) Ȝ.GetInventory(0).CurrentVolume,ΕΛ);if(Ē==null||Βξ>ΔЅ){Ē=Ȝ;ΔЅ=Βξ;Ημ=ΕΛ;}
if(ĵ==null||Βξ<ΕΔ){ĵ=Ȝ;ΕΔ=Βξ;ΗЅ=ΕΛ;}}if(Ē==null||ĵ==null||Ē==ĵ) return;if(checkConveyorSystem&&!Ē.GetInventory(0).IsConnectedTo(ĵ.GetInventory(0))){if(Δά>20) Κ="Inventory balancing failed";else Δά++;return;}Δά=0;List<MyInventoryItem>Ќ=new List<MyInventoryItem>();
Ē.GetInventory(0).GetItems(Ќ);float Ας=0;if(Ќ.Count==0) continue;MyInventoryItem Βο=Ќ[0];String Ηχ=Βο.Type.TypeId+Βο.Type.SubtypeId;if(!Θο.TryGetValue(Ηχ,out Ας)){if(Ιζ(Ē.GetInventory(0),0,ĵ.GetInventory(0),out Ας))
Θο.Add(Ηχ,Ας);else return;}float Λμ=((ΔЅ-ΖЇ)*Ημ/Ας);float Λν=((ΖЇ-ΕΔ)*ΗЅ/Ας);int Εφ=(int) Math.Min(Λν,Λμ);if(Εφ<=0) return;if((float) Βο.Amount<Εφ)
Ē.GetInventory(0).TransferItemTo(ĵ.GetInventory(0),0,null,null,Βο.Amount);else
Ē.GetInventory(0).TransferItemTo(ĵ.GetInventory(0),0,null,null,Εφ);}}bool Ιζ(IMyInventory Αζ,int Β,IMyInventory Κζ,out float Θξ){Θξ=0;float ΚΧ=(float) Αζ.CurrentVolume;List<MyInventoryItem>Δή=new List<MyInventoryItem>();Αζ.GetItems(Δή);float ΖЋ=0;for(int i=0;i<Δή.Count;i++) ΖЋ+=(float) Δή[i].Amount;
Αζ.TransferItemTo(Κζ,Β,null,null,1);float Θμ=ΚΧ-(float) Αζ.CurrentVolume;List<MyInventoryItem>ΔΣ=new List<MyInventoryItem>();Αζ.GetItems(ΔΣ);float ΖЀ=0;for(int i=0;i<ΔΣ.Count;i++) ΖЀ+=(float) ΔΣ[i].Amount;if(Θμ==0f||!Γυ(0.9999,ΖЋ-ΖЀ,1.0001))
return false;Θξ=Θμ;return true;}void ΙА(){Vector3 ΑΚ=Ѓ+Ŀ;Vector3 ț=ΑΚ-δ;if(ț.Length()==0) ț=new Vector3(0,0,-1);Vector3 ΒϏ=ц(Ω,ț);Vector3 Θα=Vector3.Normalize(ΒϏ);Vector3 Ǝ=ц(Ω,Ω.GetNaturalGravity());float ΛΫ=ΕΥ>0?Math.Max(0,1-Ů(ț,Ηβ)/5):0;float Ш=(float) Math.Min((ΕΘ>0?ΕΘ:1000f),Math.Max(Αϊ(-ΒϏ,Ǝ,null),ΕΥ*ΛΫ));
if(!Θζ) Ш=0;if(ΘΠ) Ш=Math.Max(0,1-Βΰ/ΒΡ)*Ш;if(generalSpeedLimit>0) Ш=Math.Min(generalSpeedLimit,Ш);if(Ɨ) Ш=0;if(Ǘ>=0) Ш=Math.Min(Ш,Ǘ);if(ΘΡ) Ш*=(float) Math.Min(1,К(ț.Length(),wpReachedDist)/2);Vector3 ΗЃ=ц(Ω,Ω.GetShipVelocities().LinearVelocity);float ΚΡ=(float)(Math.Max(0,15-Ů(-Θα,-ΗЃ))/15)*0.85f+0.15f;
Εμ+=Math.Sign(ΚΡ-Εμ)/10f;Vector3 İ=Θα*Ш*Εμ-(ΗЃ);Vector3 ΔΪ=Γξ(İ,null);if(ΓΘ&&г>0.1f){İ.X*=ΔΖ(İ.X,ref ΓΙ.X,1f,ΔΪ.X,20);İ.Y*=ΔΖ(İ.Y,ref ΓΙ.Y,1f,ΔΪ.Y,20);İ.Z*=ΔΖ(İ.Z,ref ΓΙ.Z,1f,ΔΪ.Z,20);}else ΓΙ=new Vector3(1,1,1);Αμ=ť*İ-Ǝ*ť;ΕЃ(Αμ,ΓΘ);г=Vector3.Distance(δ,ΑΚ);
}float ΔΖ(float value,ref float І,float Ǩ,float Ě,float ΕΙ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(І)) І+=Math.Sign(І)*Ǩ;else І=value;if(value==0) І=1;float ί=Math.Abs(І);if(ί<ΕΙ||Ě==0) return ί;І=Math.Min(ΕΙ,Math.Max(-ΕΙ,І));ί=Math.Abs(Ě);return ί;
}void Ζώ(){float Ǖ=90,Ǭ=90,Ǧ=90;float ǿ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ȓ,Ǣ,ΑΩ,ΕΑ;if(ΕΒ){Ȓ=Vector3.Normalize(Ѓ-δ);ΑΩ=ц(Ω,Ȓ);Ǣ=ц(Ω,Εκ);Ǖ=Ů(ΑΩ,new Vector3(0,-1,0))-90;Ǭ=ń(Ǣ,new Vector3(-1,0,0),Ǣ.Y);
Ǧ=ń(ΑΩ,new Vector3(-1,0,0),ΑΩ.Z);}else{Ȓ=Ελ;Ǣ=ц(Ω,Εκ);ΑΩ=ц(Ω,Ελ);ΕΑ=ц(Ω,Θβ);Ǖ=ń(Ǣ,new Vector3(0,0,1),Ǣ.Y);Ǭ=ń(Ǣ,new Vector3(-1,0,0),Ǣ.Y);Ǧ=ń(ΕΑ,new Vector3(0,0,1),ΕΑ.X);}if(ΖЂ&&ΑЊ()){Vector3 Δύ=Ω.GetNaturalGravity();Ǣ=ц(Ω,Δύ);Ǖ=ń(Ǣ,new Vector3(0,0,1),Ǣ.Y);
Ǭ=ń(Ǣ,new Vector3(-1,0,0),Ǣ.Y);}if(!Γυ(-45,Ǭ,45)){Ǖ=0;Ǧ=0;}if(!Γυ(-45,Ǧ,45)) Ǖ=0;ΒΘ(ΔϏ,1,(-Ǖ)*ǿ,(-Ǧ)*ǿ,(-Ǭ)*ǿ);Βΰ=Math.Max(Math.Abs(Ǖ),Math.Max(Math.Abs(Ǭ),Math.Abs(Ǧ)));Βφ=Βΰ<=ΒΡ;}void ΘϏ(IMyTerminalBlock ΒЊ){if(ΒЊ==null) return;Ȅ=new float[3,2];ǩ=new Dictionary<string,float[,]>();
for(int i=0;i<Ą.Count;i++){IMyThrust ъ=Ą[i];if(!ъ.IsFunctional) continue;Vector3 ξ=ц(ΒЊ,ъ.WorldMatrix.Backward);float ĳ=ъ.MaxEffectiveThrust;if(Math.Round(ξ.X,2)!=0.0)
if(ξ.X>=0) Ȅ[0,0]+=ĳ;else Ȅ[0,1]-=ĳ;if(Math.Round(ξ.Y,2)!=0.0)
if(ξ.Y>=0) Ȅ[1,0]+=ĳ;else Ȅ[1,1]-=ĳ;if(Math.Round(ξ.Z,2)!=0.0)
if(ξ.Z>=0) Ȅ[2,0]+=ĳ;else Ȅ[2,1]-=ĳ;String ǘ=ΖΣ(ъ);float[,]Ǫ=null;if(ǩ.ContainsKey(ǘ)) Ǫ=ǩ[ǘ];else{Ǫ=new float[3,2];ǩ.Add(ǘ,Ǫ);}float Ě=ъ.MaxThrust;if(Math.Round(ξ.X,2)!=0.0)
if(ξ.X>=0) Ǫ[0,0]+=Ě;else Ǫ[0,1]-=Ě;if(Math.Round(ξ.Y,2)!=0.0)
if(ξ.Y>=0) Ǫ[1,0]+=Ě;else Ǫ[1,1]-=Ě;if(Math.Round(ξ.Z,2)!=0.0)
if(ξ.Z>=0) Ǫ[2,0]+=Ě;else Ǫ[2,1]-=Ě;}}static String ΖΣ(IMyThrust ъ){return ъ.BlockDefinition.SubtypeId;}Vector3 Γό(Vector3 Ǟ,float[,]Ɛ){return new Vector3(
Ǟ.X>=0?Ɛ[0,0]:Ɛ[0,1],
Ǟ.Y>=0?Ɛ[1,0]:Ɛ[1,1],
Ǟ.Z>=0?Ɛ[2,0]:Ɛ[2,1]);}bool ΖΡ(ά έ,String ǘ,out float Δχ){Δχ=0;int Β=ă.IndexOf(ǘ);if(Β==-1||έ.Ȃ==null||Β>=έ.Ȃ.Length)
return false;Δχ=έ.Ȃ[Β];if(Δχ==-1) return false;return true;}Vector3 Γξ(Vector3 Ǟ,ά έ){if(έ!=null){Vector3 ί=new Vector3();for(int i=0;i<ǩ.Keys.Count;i++){String ǘ=ǩ.Keys.ElementAt(i);float ΑΗ=0;if(!ΖΡ(έ,ǘ,out ΑΗ))
return Γό(Ǟ,Ȅ);ί+=Γό(Ǟ,ǩ.Values.ElementAt(i))*ΑΗ;}return ί;}return Γό(Ǟ,Ȅ);}float Ʋ(Vector3 Ǟ,ά έ){return Ʋ(Ǟ,new Vector3(),έ);}float Ʋ(Vector3 Ǟ,Vector3 ψ,ά έ){Vector3 Λδ=Γξ(Ǟ,έ);Vector3 ĳ=Λδ+ψ*ť;float Ţ=(ĳ/Ǟ).AbsMin();return(float)(Ǟ*Ţ).Length();}float Αϊ(Vector3 Βύ,Vector3 Ǝ,ά έ)
{if(Βύ.Length()==0) return 0;float ΖЁ=1;if(Ǝ.Length()>0) ΖЁ=Math.Min(1,Ů(-Ǝ,Βύ)/90)*0.4f+0.6f;float Αμ=Ʋ(Βύ,Ǝ,έ);if(Αμ==0) return 0.1f;float ΒΠ=К(Αμ,ť);float Εξ=(float) Math.Sqrt(К(Βύ.Length(),ΒΠ*0.5f));return ΒΠ*Εξ*ΖЁ*ȉ;}float ΑЇ(Vector3 Ȓ,Vector3 Ї,Vector3 ψ,ά έ)
{if(ψ.Length()==0f) return 0;Vector3 Βώ=Ŋ(Ȓ,Ї,Vector3.Normalize(ψ));float Ƕ=Ʋ(-Βώ,έ);return Ƕ/ψ.Length();}void ΕЈ(bool Κϋ){float ǫ=0;float Δϊ=0.9f;if(Κϋ){ж=-1;ũ=0;ΓΓ=null;if(ε!=Ρ.я&&Ζ.ψ.Length()!=0){ǫ=Δϊ*ΑЇ(Ζ.ι,Ζ.θ*-1,Ζ.ψ,null);if(ǫ<ж||ж==-1) ж=ǫ;}if(Ο.μ&&Ο.ψ.Length()!=0)
{ǫ=Δϊ*ΑЇ(Ο.ι,Ο.θ*-1,Ο.ψ,null);if(ǫ<ж||ж==-1) ж=ǫ;}return;}if(ũ==-1) return;if(ũ>=0){int ΘΘ=0;while(ũ<Ψ.Count){if(ΘΘ>100) return;ΘΘ++;ά έ=Ψ[ũ];if(έ.ψ.Length()!=0f){ǫ=Δϊ*Math.Min(
ΑЇ(έ.ι,έ.θ*-1,έ.ψ,έ),
ΑЇ(έ.ι*-1,έ.θ*-1,έ.ψ,έ));if(ǫ<ж||ж==-1) ж=ǫ;}else ΓΓ=έ;ũ++;}ũ=-1;}bool ΔЂ=true;float Ļ=0;if(ΓΓ!=null){for(int i=0;i<ǩ.Count;i++){String ǘ=ǩ.Keys.ElementAt(i);float[,]Ǫ=ǩ.Values.ElementAt(i);float ΑΗ=0;if(!ΖΡ(ΓΓ,ǘ,out ΑΗ)){ΔЂ=false;break;}for(int a=0;a<Ǫ.GetLength(0);a++)
for(int b=0;b<Ǫ.GetLength(1);b++){float ō=Math.Abs(Ǫ[a,b]*ΑΗ);if(ō==0) continue;ΔЂ=true;if(Ļ==0||ō<Ļ) Ļ=ō;}}}if(!ΔЂ){for(int a=0;a<Ȅ.GetLength(0);a++)
for(int b=0;b<Ȅ.GetLength(1);b++){float ō=Math.Abs(Ȅ[a,b]);if(ō==0) continue;if(Ļ==0||ō<Ļ) Ļ=ō;}}if(Ļ>0){float ΚЇ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǫ=К(Ļ,ΚЇ);if(ǫ>0)
if(ǫ<ж||ж==-1) ж=ǫ;}}void ΕЃ(Vector3 Ƕ,bool О){if(!О){for(int i=0;i<Ą.Count;i++)
Ą[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔΩ=Γξ(Ƕ,null);float Λώ=Math.Min(1,Math.Abs(К(Ƕ.X,ΔΩ.X)));float ΛϏ=Math.Min(1,Math.Abs(К(Ƕ.Y,ΔΩ.Y)));float ΛЀ=Math.Min(1,Math.Abs(К(Ƕ.Z,ΔΩ.Z)));for(int i=0;i<Ą.Count;i++){IMyThrust ъ=Ą[i];Vector3 ξ=Ις(ц(Ω,ъ.WorldMatrix.Backward),1);
if(ξ.X!=0&&Math.Sign(ξ.X)==Math.Sign(Ƕ.X))
ъ.SetValueFloat("Override",ъ.MaxThrust*Λώ);else if(ξ.Y!=0&&Math.Sign(ξ.Y)==Math.Sign(Ƕ.Y))
ъ.SetValueFloat("Override",ъ.MaxThrust*ΛϏ);else if(ξ.Z!=0&&Math.Sign(ξ.Z)==Math.Sign(Ƕ.Z))
ъ.SetValueFloat("Override",ъ.MaxThrust*ΛЀ);else
ъ.SetValueFloat("Override",0.0f);}}void ΒΘ(bool О,float ΛΗ,float Ǖ,float Ǧ,float Ǭ){for(int i=0;i<Βρ.Count;i++){IMyGyro Л=Βρ[i];Л.GyroOverride=О;if(!О) Л.GyroPower=100;else Л.GyroPower=ΛΗ;if(!О) continue;Vector3 Э=Ω.WorldMatrix.Forward;Vector3 т=Ω.WorldMatrix.Right;Vector3 Ї=Ω.WorldMatrix.Up;
Vector3 Ƈ=Л.WorldMatrix.Forward;Vector3 Ɖ=Л.WorldMatrix.Up;Vector3 ƈ=Л.WorldMatrix.Left*-1;if(Ƈ==Э) Л.SetValueFloat("Roll",Ǭ);else if(Ƈ==(Э*-1)) Л.SetValueFloat("Roll",Ǭ*-1);else if(Ɖ==(Э*-1)) Л.SetValueFloat("Yaw",Ǭ);else if(Ɖ==Э) Л.SetValueFloat("Yaw",Ǭ*-1);else if(ƈ==Э) Л.SetValueFloat("Pitch",Ǭ);
else if(ƈ==(Э*-1)) Л.SetValueFloat("Pitch",Ǭ*-1);if(ƈ==(т*-1)) Л.SetValueFloat("Pitch",Ǖ);else if(ƈ==т) Л.SetValueFloat("Pitch",Ǖ*-1);else if(Ɖ==т) Л.SetValueFloat("Yaw",Ǖ);else if(Ɖ==(т*-1)) Л.SetValueFloat("Yaw",Ǖ*-1);else if(Ƈ==(т*-1)) Л.SetValueFloat("Roll",Ǖ);
else if(Ƈ==т) Л.SetValueFloat("Roll",Ǖ*-1);if(Ɖ==(Ї*-1)) Л.SetValueFloat("Yaw",Ǧ);else if(Ɖ==Ї) Л.SetValueFloat("Yaw",Ǧ*-1);else if(ƈ==Ї) Л.SetValueFloat("Pitch",Ǧ);else if(ƈ==(Ї*-1)) Л.SetValueFloat("Pitch",Ǧ*-1);else if(Ƈ==Ї) Л.SetValueFloat("Roll",Ǧ);else if(Ƈ==(Ї*-1)) Л.SetValueFloat("Roll",Ǧ*-1);
}}void Ζψ(){this.ΔϏ=false;}void н(Vector3 Ĳ,Vector3 Э,Vector3 ΔБ,float ġ,bool Αρ){ΓΥ(Ĳ,ġ,Αρ);ΒΡ=ġ;ΕΒ=false;this.Ελ=Э;this.Θβ=ΔБ;}void н(Vector3 Ĳ,Vector3 Э,Vector3 ΔБ,bool Αρ){н(Ĳ,Э,ΔБ,2f,Αρ);}void ΓΥ(Vector3 Ĳ,float ġ,bool Αρ){ΒΡ=ġ;this.ΔϏ=true;this.ΖЂ=Αρ;ΕΒ=true;
Βφ=false;this.Εκ=Ĳ;}void ΔΛ(){ȇ(false,false,false,Ѓ,0);ΓΘ=false;}void й(Vector3 η,float ǿ){ȇ(true,false,false,η,ǿ);}void ȇ(bool О,bool Εε,bool Εω,Vector3 η,float ǿ){ȇ(О,Εε,Εω,η,η-δ,0.0f,ǿ);}void ȇ(bool О,bool Εε,bool Εω,Vector3 η,Vector3 ΛΥ,float ΛΦ,float ǿ){ΓΘ=true;
this.Θζ=О;Ѓ=η;this.ΕΘ=ǿ;this.ΕΥ=ΛΦ;this.ΘΡ=Εε;this.ΘΠ=Εω;this.Ηβ=ΛΥ;г=Vector3.Distance(η,δ);}void Ιυ(){υ.GetBlocksOfType(ǁ,ǚ);}void Ιφ(){if(ΒЌ){Б=2;return;}List<IMyRemoteControl>Αα=new List<IMyRemoteControl>();List<IMySensorBlock>ΘΞ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ακ=new List<IMyTerminalBlock>();
υ.GetBlocksOfType(Αα,ǚ);υ.GetBlocksOfType(Ч,ǚ);υ.GetBlocksOfType(ΘΞ,ǚ);υ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ακ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=ǭ(Ч,pamTag,true);Ζπ();Ζρ(Ακ);ŉ(Ч,setLCDFontAndSize,1.4f,false);ŉ(Ț,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Θέ=ǭ(ΘΞ,pamTag,true);if(Θέ.Count>0) ω=Θέ[0];else ω=null;if(Μ==Λ.Ƹ){υ.GetBlocksOfType(ϊ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ϊ.Count==0){Б=1;Κ="Drills are missing";}}else if(Μ==Λ.Ǯ){υ.GetBlocksOfType(ϊ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ϊ.Count==0){Б=1;Κ="Grinders are missing";}if(ω==null){Б=1;Κ="Sensor is missing";}}else if(Μ==Λ.ή){υ.GetBlocksOfType(Εο,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Θά=ǭ(Αα,pamTag,true);if(Θά.Count>0) Αα=Θά;if(Αα.Count>0) Ω=Αα[0];else{Ω=null;Б=2;Κ="Remote is missing";return;}λ=(float) Ω.CubeGrid.WorldVolume.Radius*2;Ǥ=null;if(Μ!=Λ.ή){ΕЉ();if(ϊ.Count>0&&Ǥ!=null){if(ω!=null&&(Ǥ.WorldMatrix.Forward!=ω.WorldMatrix.Forward||
!(Ω.WorldMatrix.Forward==ω.WorldMatrix.Up||Ω.WorldMatrix.Down==ω.WorldMatrix.Down))){Б=1;Κ="Wrong sensor direction";}if(Ǥ.WorldMatrix.Forward!=Ω.WorldMatrix.Forward&&
Ǥ.WorldMatrix.Forward!=Ω.WorldMatrix.Down){Б=2;Κ="Wrong remote direction";}}}}void Ιχ(){υ.GetBlocksOfType(ȳ,ǚ);υ.GetBlocksOfType(ў,ǚ);υ.GetBlocksOfType(Ą,ǚ);υ.GetBlocksOfType(Βρ,ǚ);υ.GetBlocksOfType(ĭ,ǚ);υ.GetBlocksOfType(ļ,ǚ);υ.GetBlocksOfType(Ǡ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));υ.GetBlocksOfType(ΓΒ,ǚ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ў=ǭ(ў,"ConnectorMedium",false);else
ў=ǭ(ў,"Connector",false);List<IMyShipConnector>ΘΪ=ǭ(ў,pamTag,true);if(ΘΪ.Count>0) ў=ΘΪ;if(Б<=1){if(ў.Count==0){Б=1;Κ="Connector is missing";}if(Βρ.Count==0){Б=1;Κ="Gyros are missing";}if(Ą.Count==0){Б=1;Κ="Thrusters are missing";}}List<IMyConveyorSorter>Θή=ǭ(ΓΒ,pamTag,true);
if(Θή.Count>0) ΓΒ=Θή;List<IMyLandingGear>ΘΫ=ǭ(ȳ,pamTag,true);if(ΘΫ.Count>0) ȳ=ΘΫ;for(int i=0;i<ȳ.Count;i++) ȳ[i].AutoLock=false;List<IMyBatteryBlock>ΘΩ=ǭ(ĭ,pamTag,true);if(ΘΩ.Count>0) ĭ=ΘΩ;List<IMyGasTank>Θί=ǭ(Ǡ,pamTag,true);if(Θί.Count>0) Ǡ=Θί;}void Ιψ(){υ.GetBlocksOfType(Ŏ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ƅ.Clear();for(int i=Ŏ.Count-1;i>=0;i--){if(Ιή(Ŏ[i])){Ƅ.Add(Ŏ[i]);Ŏ.RemoveAt(i);}}}bool Ιή(IMyTerminalBlock Ϊ){if(Ϊ is IMyCargoContainer) return true;if(Ϊ is IMyShipDrill) return true;if(Ϊ is IMyShipGrinder) return true;if(Ϊ is IMyShipConnector){if(((IMyShipConnector) Ϊ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Γς(Ϊ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕЉ(){if(Ω==null) return;Ǥ=null;float ΒЅ=0,ΒЀ=0,ΒІ=0,ΒЁ=0,ΒЇ=0,ΒЂ=0;List<IMyTerminalBlock>Ει=ǭ(ϊ,pamTag,true);bool ΚЎ=Ει.Count==0;if(Ει.Count>0) Ǥ=Ει[0];else if(ϊ.Count>0) Ǥ=ϊ[0];int ȭ=0;float ƚ=0;for(int i=0;i<ϊ.Count;i++)
{if(ϊ[i].WorldMatrix.Forward!=Ǥ.WorldMatrix.Forward){if(ΚЎ){Б=2;Κ="Mining direction is unclear!";return;}continue;}ȭ++;ƚ=Math.Max(ƚ,ΙЉ(ϊ[i]));Vector3 П=ΑІ(Ω,ϊ[i].GetPosition());if(i==0){ΒЅ=П.X;ΒЀ=П.X;ΒІ=П.Y;ΒЁ=П.Y;ΒЇ=П.Z;ΒЂ=П.Z;}ΒЀ=Math.Max(П.X,ΒЀ);ΒЅ=Math.Min(П.X,ΒЅ);
ΒЁ=Math.Max(П.Y,ΒЁ);ΒІ=Math.Min(П.Y,ΒІ);ΒЂ=Math.Max(П.Z,ΒЂ);ΒЇ=Math.Min(П.Z,ΒЇ);}if(ƚ==0) ƚ=drillRadius;bool Ĳ=Ǥ!=null&&Ǥ.WorldMatrix.Forward==Ω.WorldMatrix.Down;ȃ=(ΒЀ-ΒЅ)*(1-Ū)+ƚ*2;ȝ=(Ĳ?ΒЂ-ΒЇ:ΒЁ-ΒІ)*(1-ŕ)+ƚ*2;IMyCubeGrid g=Ω.CubeGrid;float Εύ=0,Εώ=0,ΕϏ=0,ΕЀ=0;
for(int c=0;c<8;c++){Vector3 p=ΑІ(Ω,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ĳ?p.Z:p.Y;if(c==0||p.X<Εύ) Εύ=p.X;if(c==0||p.X>Εώ) Εώ=p.X;if(c==0||y<ΕϏ) ΕϏ=y;if(c==0||y>ΕЀ) ΕЀ=y;}ΒЎ=Εώ-Εύ+g.GridSize;Εΰ=ΕЀ-ΕϏ+g.GridSize;}float ΙЉ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǭ<T>(List<T>Ў,String Ȁ,bool Βς){List<T>ί=new List<T>();for(int i=0;i<Ў.Count;i++)
if(Γς(Ў[i],Ȁ,Βς)) ί.Add(Ў[i]);return ί;}bool Γς<T>(T Ϊ,String Ȁ,bool Βς){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(Βς&&b.CustomName.ToUpper().Contains(Ȁ.ToUpper())) return true;if(!Βς&&b.BlockDefinition.ToString().ToUpper().Contains(Ȁ.ToUpper())) return true;return false;}void ǜ<T>(List<T>Ў,bool О)
{for(int i=0;i<Ў.Count;i++)
Ζξ((IMyTerminalBlock) Ў[i],О);}void Ζξ(IMyTerminalBlock Ϊ,bool О){if(Ϊ==null) return;String ΚΟ=О?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(ΚΟ);a.Apply(Ϊ);}void Ȉ(bool О){Εζ=О;if(!Ŧ) return;ǜ(ΓΒ,О);}void ΔΒ(ChargeMode А){for(int i=0;i<ĭ.Count;i++) ĭ[i].ChargeMode=А;}void ΔΔ(bool Λέ)
{for(int i=0;i<Ǡ.Count;i++) Ǡ[i].Stockpile=Λέ;}void ƻ(List<IMyLandingGear>Δϋ,bool ΗЄ){for(int i=0;i<Δϋ.Count;i++){if(ΗЄ) Δϋ[i].Lock();if(!ΗЄ) Δϋ[i].Unlock();}}void Ζο(bool О){if(ΗΡ==О) return;List<IMyShipController>Βά=new List<IMyShipController>();υ.GetBlocksOfType(Βά,ǚ);if(Βά.Count==0) return;
for(int i=0;i<Βά.Count;i++)
Βά[i].DampenersOverride=О;ΗΡ=О;}IMyShipConnector и(MyShipConnectorStatus ǖ){for(int i=0;i<ў.Count;i++){if(!Ȣ(ў[i],true)) continue;if(ў[i].Status==ǖ) return ў[i];}return null;}void ŋ(bool Εα,bool Βδ,float Κς,float ΚΦ){if(ω==null||ϊ.Count==0) return;Vector3 ΒΨ=new Vector3();
int ȭ=0;for(int i=0;i<ϊ.Count;i++){if(ϊ[i].WorldMatrix.Forward!=Ǥ.WorldMatrix.Forward) continue;ȭ++;ΒΨ+=ϊ[i].GetPosition();}ΒΨ=ΒΨ/ȭ;Vector3 Ɣ=ΑІ(ω,ΒΨ);ω.Enabled=true;ω.ShowOnHUD=Εα;ω.LeftExtend=(Βδ?1:ђ)/2f*ȃ-Ɣ.X;ω.RightExtend=(Βδ?1:ђ)/2f*ȃ+Ɣ.X;ω.TopExtend=(Βδ?1:ё)/2f*ȝ+Ɣ.Y;
ω.BottomExtend=(Βδ?1:ё)/2f*ȝ-Ɣ.Y;ω.FrontExtend=(Εα?п:Κς)-Ɣ.Z;ω.BackExtend=Εα?0:ΚΦ+λ*0.75f+Ɣ.Z;ω.DetectFloatingObjects=true;ω.DetectAsteroids=false;ω.DetectLargeShips=true;ω.DetectSmallShips=true;ω.DetectStations=true;ω.DetectOwner=true;ω.DetectSubgrids=false;
ω.DetectPlayers=false;ω.DetectEnemy=true;ω.DetectFriendly=true;ω.DetectNeutral=true;}void ŉ<T>(List<T>Ў,bool ΛΨ,float Κπ,bool Κό){for(int i=0;i<Ў.Count;i++){IMyTextSurface Αι=null;if(Ў[i]is IMyTextSurface) Αι=(IMyTextSurface) Ў[i];if(Αι!=null){Αι.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΛΨ) continue;Αι.Font="Debug";if(Κό) continue;Αι.FontSize=Κπ;}}}bool ΑЊ(){Vector3D γ;return this.Ω.TryGetPlanetPosition(out γ);}int ΙΞ(Vector3 Λί,int ΘΣ){if(ΘΣ==-1) return 0;double Ь=-1;int ď=-1;for(int i=Ψ.Count-1;i>=0;i--){double σ=Vector3.Distance(Ψ[i].Ε,Λί);if(Ь==-1||σ<Ь){ď=i;Ь=σ;}
}return Math.Sign(ď-ΘΣ);}bool Ιί(Vector3 γ){List<Vector3>а=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) а.Add(Ψ[i].Ε);if(Ο.μ&&Ψ.Count>=1){Vector3 ſ=new Vector3();ȡ(Ο,dockDist*λ,false,out ſ);if(Vector3.Distance(Ο.Ε,Ψ.First().Ε)<Vector3.Distance(Ο.Ε,Ψ.Last().Ε)){а.Insert(0,ſ);а.Insert(0,Ο.Ε);}
else{а.Add(ſ);а.Add(Ο.Ε);}}if(Μ==Λ.ή){if(Ζ.μ&&Ψ.Count>=1){Vector3 ΔΥ=new Vector3();ȡ(Ζ,dockDist*λ,false,out ΔΥ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){а.Insert(0,ΔΥ);а.Insert(0,Ζ.Ε);}else{а.Add(ΔΥ);а.Add(Ζ.Ε);}}}else{if(ε!=Ρ.я)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
а.Insert(0,Ζ.Ε);else а.Add(Ζ.Ε);}int ď=-1;double ȍ=-1;for(int i=0;i<а.Count;i++){double σ=Vector3.Distance(а[i],γ);if(σ<ȍ||ȍ==-1){ȍ=σ;ď=i;}}if(а.Count==0) return false;double Δρ=Vector3.Distance(а[ď],γ);double ΛΘ=Vector3.Distance(а[Math.Max(0,ď-1)],а[ď])*1.5f;
double ΚЍ=Vector3.Distance(а[Math.Min(а.Count-1,ď+1)],а[ď])*1.5f;return Δρ<ΛΘ||Δρ<ΚЍ||Δρ<laneOffset*λ*1.5f;}static float К(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ις(Vector3 v,int Δξ){return new Vector3(Math.Round(v.X,Δξ),Math.Round(v.Y,Δξ),Math.Round(v.Z,Δξ));
}float Ů(Vector3 a,Vector3 b){if(a==b) return 0;float Κμ=(a*b).Sum;float Ηώ=a.Length();float ΗϏ=b.Length();if(Ηώ==0||ΗϏ==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(Κμ/(Ηώ*ΗϏ)));if(float.IsNaN(ί)) return 0;return ί;}float ń(Vector3 v,Vector3 ΒЊ,float ΛΩ){float Ž=Ů(v,ΒЊ);
if(ΛΩ>0f) Ž*=-1;if(Ž>-90f) return Ž-90f;else return 180f-(-Ž-90f);}double ΕБ(float Κδ){return(Math.PI/180)*Κδ;}bool Γυ(double ΚІ,double value,double Ƒ){return(value>=ΚІ&&value<=Ƒ);}Vector3 ΙЊ(IMyTerminalBlock Ϊ,Vector3 П){return Vector3D.Transform(П,Ϊ.WorldMatrix);
}Vector3 ΑІ(IMyTerminalBlock Ϊ,Vector3 Λϋ){return ц(Ϊ,Λϋ-Ϊ.GetPosition());}Vector3 ц(IMyTerminalBlock Ϊ,Vector3 Εό){return Vector3D.TransformNormal(Εό,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 Ŋ(Vector3 Ȓ,Vector3 Ї,Vector3 Εό){MatrixD ΕΗ=MatrixD.CreateFromDir(Ȓ,Ї);return Vector3D.TransformNormal(Εό,MatrixD.Transpose(ΕΗ));
}Vector3 ΔΞ(Vector3 Ȓ,Vector3 Ї,Vector3 П){MatrixD ΕΗ=MatrixD.CreateFromDir(Ȓ,Ї);return Vector3D.Transform(П,ΕΗ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ρ(String s){String[]φ=s.Split('|');return new Vector3(float.Parse(Π(φ,0)),float.Parse(Π(φ,1)),float.Parse(Π(φ,2)));
}String ΚΝ(ά έ){String С=":";String ί=τ(έ.Ε)+С+τ(έ.ι)+С+
τ(έ.θ)+С+τ(έ.Ђ)+С+τ(έ.ψ);for(int i=0;i<έ.Ȃ.Length;i++)
ί+=С+Math.Round(έ.Ȃ[i],3);return ί;}ά ΙЃ(String s){String[]φ=s.Split(':');ά έ=new ά(
ρ(Π(φ,0)),ρ(Π(φ,1)),
ρ(Π(φ,2)),ρ(Π(φ,3)),
ρ(Π(φ,4)));int Β=5;List<float>Ηί=new List<float>();while(Β<φ.Length){String ΓΞ=Π(φ,Β);float f=0;if(!float.TryParse(ΓΞ,out f)) break;Ηί.Add(f);Β++;}έ.Ȃ=Ηί.ToArray();return έ;}System.Text.StringBuilder Ɲ=new System.Text.StringBuilder();void Χ<T>(T value,bool ΚЌ){if(ΚЌ) Ɲ.Append('\n');Ɲ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String ΑϏ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΝ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Π(String[]ΚΤ,int Β){String ΓΞ=ΚΤ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΞ)) return "";
return ΓΞ;}void Save(){if(ΒЌ||Μ==Λ.ћ){Storage="";return;}Storage=ΓΫ();}String ΓΫ(){Ɲ.Clear();Ɲ.Append(DATAREV+";");Χ(τ(ΒЋ),false);Χ(τ(Ο.ι));Χ(τ(Ο.Ђ));Χ(τ(Ο.θ));Χ(τ(Ο.ψ));Χ(τ(Ο.Ε));Χ(τ(Ο.р));Χ(Ο.μ);Χ(τ(Ζ.Ε));Χ(τ(Ζ.ψ));Χ(τ(Ζ.ι));Χ(τ(Ζ.θ));Χ(τ(Ζ.Ђ));Χ(τ(Ζ.р));Χ(Ζ.μ);Χ(τ(Ǔ));Χ(τ(ě));Χ(";");
Χ((int) Μ,false);Χ((int) ε);Χ((int) ǐ);Χ(ȕ);Χ(Ǒ);Χ(с);Χ(ǒ);Χ(ȉ);Χ(ģ);Χ(Ģ);if(Μ==Λ.ή){Χ((int) Ј.Т);Χ(Ј.Ǵ);Χ(Ј.Ĵ);Χ(ΑϏ(Ј.ǵ));Χ(ΑϏ(Ј.Ǻ));Χ((int) Љ.Т);Χ(Љ.Ǵ);Χ(Љ.Ĵ);Χ(ΑϏ(Љ.ǵ));Χ(ΑϏ(Љ.Ǻ));}else{Χ((int) ș);Χ((int) і);Χ((int) е);Χ((int) џ);Χ((int) Ř);Χ(ŀ);Χ(Ŧ);Χ(ƌ);Χ(Ɔ);Χ(ђ);Χ(ё);Χ(п);Χ(Ʈ);Χ(ƭ);
Χ(Ū);Χ(ŕ);Χ(Ѝ);Χ(Ю);Χ(Ё);Χ(ώ);Χ(ę);Χ(в);Χ(ţ);Χ(ś);}Χ(";");for(int i=0;i<ă.Count;i++)
Χ((i>0?"|":"")+ă[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΚΝ(Ψ[i]),i>0);Χ(";",false);Χ(д+"|"+Г,false);Χ(";",false);Χ(ΘА(),false);String Θ=Ɲ.ToString();Ɲ.Clear();return Θ;}ќ Ιρ(){return ΒΓ(Storage);}ќ ΒΓ(String Θ){if(Θ=="") return ќ.Ιθ;String[]Ť=Θ.Split(';');if(Π(Ť,0)!=DATAREV) return ќ.ΓЂ;int Β=0;try{String[]Ξ=Π(Ť,1).Split('\n');
ΒЋ=ρ(Π(Ξ,Β++));Ο.ι=ρ(Π(Ξ,Β++));Ο.Ђ=ρ(Π(Ξ,Β++));Ο.θ=ρ(Π(Ξ,Β++));Ο.ψ=ρ(Π(Ξ,Β++));Ο.Ε=ρ(Π(Ξ,Β++));Ο.р=ρ(Π(Ξ,Β++));Ο.μ=bool.Parse(Π(Ξ,Β++));Ζ.Ε=ρ(Π(Ξ,Β++));Ζ.ψ=ρ(Π(Ξ,Β++));Ζ.ι=ρ(Π(Ξ,Β++));Ζ.θ=ρ(Π(Ξ,Β++));Ζ.Ђ=ρ(Π(Ξ,Β++));Ζ.р=ρ(Π(Ξ,Β++));Ζ.μ=bool.Parse(Π(Ξ,Β++));
Ǔ=ρ(Π(Ξ,Β++));ě=ρ(Π(Ξ,Β++));Ξ=Π(Ť,2).Split('\n');Β=0;Μ=(Λ) int.Parse(Π(Ξ,Β++));ε=(Ρ) int.Parse(Π(Ξ,Β++));ǐ=(Ρ) int.Parse(Π(Ξ,Β++));ȕ=int.Parse(Π(Ξ,Β++));Ǒ=int.Parse(Π(Ξ,Β++));с=int.Parse(Π(Ξ,Β++));ǒ=int.Parse(Π(Ξ,Β++));ȉ=float.Parse(Π(Ξ,Β++));ģ=bool.Parse(Π(Ξ,Β++));
Ģ=bool.Parse(Π(Ξ,Β++));if(Μ==Λ.ή){Ј.Т=(ς) int.Parse(Π(Ξ,Β++));Ј.Ǵ=float.Parse(Π(Ξ,Β++));Ј.Ĵ=float.Parse(Π(Ξ,Β++));Ј.ǵ=ΒΝ(Π(Ξ,Β++));Ј.Ǻ=ΒΝ(Π(Ξ,Β++));Љ.Т=(ς) int.Parse(Π(Ξ,Β++));Љ.Ǵ=float.Parse(Π(Ξ,Β++));Љ.Ĵ=float.Parse(Π(Ξ,Β++));Љ.ǵ=ΒΝ(Π(Ξ,Β++));Љ.Ǻ=ΒΝ(Π(Ξ,Β++));}else
{ș=(ѐ) int.Parse(Π(Ξ,Β++));і=(ь) int.Parse(Π(Ξ,Β++));е=(В) int.Parse(Π(Ξ,Β++));џ=(Ǚ) int.Parse(Π(Ξ,Β++));Ř=(ѐ) int.Parse(Π(Ξ,Β++));ŀ=bool.Parse(Π(Ξ,Β++));Ŧ=bool.Parse(Π(Ξ,Β++));ƌ=bool.Parse(Π(Ξ,Β++));Ɔ=bool.Parse(Π(Ξ,Β++));ђ=int.Parse(Π(Ξ,Β++));ё=int.Parse(Π(Ξ,Β++));п=int.Parse(Π(Ξ,Β++));
Ʈ=float.Parse(Π(Ξ,Β++));ƭ=float.Parse(Π(Ξ,Β++));Ū=float.Parse(Π(Ξ,Β++));ŕ=float.Parse(Π(Ξ,Β++));Ѝ=int.Parse(Π(Ξ,Β++));Ю=int.Parse(Π(Ξ,Β++));Ё=int.Parse(Π(Ξ,Β++));ώ=int.Parse(Π(Ξ,Β++));ę=int.Parse(Π(Ξ,Β++));в=int.Parse(Π(Ξ,Β++));ţ=int.Parse(Π(Ξ,Β++));ś=float.Parse(Π(Ξ,Β++));
}Ξ=Π(Ť,3).Replace("\n","").Split('|');ă=Ξ.ToList();Ξ=Π(Ť,4).Split('\n');Ψ.Clear();if(Ξ.Count()>=1&&Ξ[0]!="")
for(int i=0;i<Ξ.Length;i++)
Ψ.Add(ΙЃ(Π(Ξ,i)));Ξ=Π(Ť,5).Split('|');д=Π(Ξ,0);if(!int.TryParse(Π(Ξ,1),out Г)) Г=-1;if(д.Length!=Ѝ*Ю){д="";Г=-1;}ΘЏ(Π(Ť,6));}catch{return ќ.ΖΓ;}ƞ=ǐ;Є();return ќ.ΒΚ;}const int ΕЇ=64000;IMyTerminalBlock ΖΔ(){List<IMyTerminalBlock>Ƃ=new List<IMyTerminalBlock>();υ.GetBlocksOfType(Ƃ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ƃ.Count>0?Ƃ[0]:null;}void ΙΓ(){IMyTerminalBlock Ϊ=ΖΔ();if(Ϊ==null){Κ="Export: name a block "+dataTag;return;}String Θ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΓΫ();if(Θ.Length>ΕЇ){Κ="Export too large: "+Θ.Length+" chars, max "+ΕЇ;return;}Ϊ.CustomData=Θ;Κ="Exported "+Ψ.Count+" waypoints";}void ΙΫ(bool ΔЊ){IMyTerminalBlock Ϊ=ΖΔ();if(Ϊ==null){Κ="Import: name a block "+dataTag;return;}
Ιά(Ϊ.CustomData,ΔЊ);}void Ιά(String Θ,bool ΔЊ){Θ=Θ.Replace("\r","");if(Θ.StartsWith("//")){int ΗБ=Θ.IndexOf('\n');Θ=ΗБ<0?"":Θ.Substring(ΗБ+1);}Θ=Θ.Trim();if(Θ==""){Κ="Import: no PAM data in "+dataTag;return;}Є();String ΖЉ=ΓΫ();ќ ί=ΒΓ(Θ);if(ί==ќ.ΒΚ&&ΔЊ){ά Φ=new ά(Ζ);
Vector3 ΚЉ=Ǔ,ΚЈ=ě;int w=ђ,h=ё,d=п;ѐ Αη=ș;Ǚ Δο=џ;float ΛΑ=Ū,ΚБ=ŕ;ΒΓ(ΖЉ);Ζ=Φ;Ǔ=ΚЉ;ě=ΚЈ;ђ=w;ё=h;п=d;ș=Αη;џ=Δο;Ū=ΛΑ;ŕ=ΚБ;ε=Ρ.Ÿ;ƿ(true,true);ε=Ρ.Ÿ;ǐ=Ρ.Ÿ;Κ="Imported job, run CONT";return;}if(ί==ќ.ΒΚ){Αέ=false;Κ="Imported "+Ψ.Count+" waypoints";
return;}ΒΓ(ΖЉ);Κ=ί==ќ.ΓЂ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζπ(){ő.Clear();for(int i=Ч.Count-1;i>=0;i--){String Θ=Ч[i].CustomData.ToUpper();bool ΔЈ=false;if(Θ=="INSTRUCTIONS"){ΔЈ=true;Βγ=true;}if(Θ=="DEBUG") ΔЈ=true;if(ΔЈ){ő.Add(Ч[i]);Ч.RemoveAt(i);}
}ŉ(ő,false,1,false);}void Ζρ(List<IMyTerminalBlock>Ƃ){Ț.Clear();for(int i=0;i<Ƃ.Count;i++){IMyTerminalBlock Ϊ=Ƃ[i];try{String ΘΗ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ϊ.CustomName.IndexOf(ΘΗ);int ƣ=-1;if(Β<0||!int.TryParse(Ϊ.CustomName.Substring(Β+ΘΗ.Length,1),out ƣ)) continue;
if(ƣ==-1) continue;ƣ--;IMyTextSurfaceProvider ΘΙ=(IMyTextSurfaceProvider) Ϊ;if(ƣ<ΘΙ.SurfaceCount&&ƣ>=0)
Ț.Add(ΘΙ.GetSurface(ƣ));}catch{}}}String ΒЃ="";void Ζλ(){String ΕΖ="";String ΑΖ="";ΒЃ=ΕІ(false);ΕΖ+=ΒЃ;ΑΖ+=ΒЃ;ΑΖ+=ΙΛ();for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(ΕΖ);for(int i=0;i<Ț.Count;i++) Ț[i].WriteText(ΕΖ);Echo(ΑΖ);for(int i=0;i<ő.Count;i++){IMyTextPanel щ=ő[i];
String Θ=щ.CustomData.ToUpper();if(Θ=="DEBUG") щ.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") щ.WriteText(ΖΜ());}}string ΖΜ(){String Γ="";try{float ΗІ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(Ɓ*ΗІ)+" / "+(int)(ƒ*ΗІ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(Ɓ*100f,1)+"% / "+Math.Round(ƒ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ǆ,2)+"ms / "+Math.Round(ΕΚ,2)+"ms \n";}catch{}for(int i=0;i<Ĝ.Count;i++)
Γ+=""+Ĝ.Keys.ElementAt(i)+" = "+Math.Round(Ĝ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ĝ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙΦ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ŉ(Ч,setLCDFontAndSize,0.9f,false);ŉ(Ț,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ч.Count;i++) Ч[i].WriteText(Γ+Ę.ToString());for(int i=0;i<Ț.Count;i++) Ț[i].WriteText(Γ+Ę.ToString());}void ȧ(){if(!Βγ) return;try{ΕΨ=Runtime.CurrentInstructionCount;}catch{}
}void Ƞ(String ΰ){if(!Βγ) return;if(ΕΨ==0) return;try{float ΓΜ=(Runtime.CurrentInstructionCount-ΕΨ)/Runtime.MaxInstructionCount*100;if(!Ĝ.ContainsKey(ΰ))
Ĝ.Add(ΰ,new float[]{ΓΜ,ΓΜ});else{Ĝ[ΰ][0]=ΓΜ;Ĝ[ΰ][1]=Math.Max(ΓΜ,Ĝ[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Θχ(String ȑ){if(!ƃ) return;String С=""+BROADCAST_SEP;String β=VERSION+С;β+=Ư(ơ)+С;
β+=(int) Μ+С;β+=Ų(δ.X)+""+С;β+=Ų(δ.Y)+С;β+=Ų(δ.Z)+С;if(Μ!=Λ.ή)
β+=Γλ(ǐ)+(ǐ==Ρ.И?" "+Math.Round(ΑΤ,1)+"%":"")+С;else β+=Γλ(ǐ)+С;β+=Ư(Κ)+С;β+=Џ+""+С;β+=Ư(ΒЃ)+С;β+=ȏ+С;β+=Ȗ+С;for(int i=0;i<Ǹ.Count;i++){if(Ǹ[i].Ə!=Щ.Ŵ) continue;β+=Ư(Ǹ[i].ΰ)+"/"+Ư(Ǹ[i].χ)+"/"+Ǹ[i].Ћ+С;}Ιό(β,ȑ);}String Ư(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ιό(String β,String ȑ){ΒΖ(β,BROADCAST_SELF_ID,ȑ);}void ΒΖ(String β,String η,String ȑ){try{if(!ƃ) return;β=BROADCAST_TAG+BROADCAST_SEP+ΖΘ()+BROADCAST_SEP+η+BROADCAST_SEP+Δβ+BROADCAST_SEP+ȑ+BROADCAST_SEP+β;this.IGC.SendBroadcastMessage(BROADCAST_TAG,β);}catch(Exception e){Ę=e;}
}String ΖΘ(){if(Μ!=Λ.ћ) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γϊ(String β){return β.StartsWith(BROADCAST_TAG);}bool з(ref String Θ,out string ΑΙ,Char С){int Β=Θ.IndexOf(С);ΑΙ="";if(Β<0) return false;ΑΙ=Θ.Substring(0,Β);Θ=Θ.Remove(0,Β+1);return true;}bool Ζκ(ref String β,out String Ș,out String ȑ)
{Ș="";ȑ="";if(!ƃ) return false;String Ȁ="";if(!з(ref β,out Ȁ,BROADCAST_SEP)||!Γϊ(Ȁ)) return false;if(!з(ref β,out Ș,BROADCAST_SEP)) return false;if(!з(ref β,out Ȁ,BROADCAST_SEP)||(Ȁ!=ΖΘ()&&(Ȁ!="*"&&Μ!=Λ.ћ))) return false;if(!з(ref β,out Ȁ,BROADCAST_SEP)||(Ȁ!=Δβ)) return false;
if(!з(ref β,out ȑ,BROADCAST_SEP)) return false;return true;}void ΖΪ(){bool ǋ=true;if(Ηα){Ηα=false;if(Me.CustomData.Contains("Antenna_Name")){Κ="Update custom data";Me.CustomData="";}}String ΑΟ=(Μ!=Λ.ћ?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΟ)
ΖΫ(ΑΟ);String[]Ξ=Me.CustomData.Split('\n');ƃ=bool.Parse(ΒΒ(ΑЁ(Ξ,"Enable_Broadcast",ref ǋ)));bool Εϊ=false;bool ΗΦ=true;if(ƃ){if(Μ!=Λ.ћ)
ơ=ΒΒ(ΑЁ(Ξ,"Ship_Name",ref ǋ)).Replace(BROADCAST_SELF_ID,"");Δβ=ΒΒ(ΑЁ(Ξ,"Broadcast_Channel",ref ǋ)).ToLower();ΗΦ=false;if(ǲ==null){ǲ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ǲ.SetMessageCallback("");}List<IMyRadioAntenna>ž=new List<IMyRadioAntenna>();υ.GetBlocksOfType(ž);bool ΔΤ=false;
for(int i=0;i<ž.Count;i++)
if(ž[i].EnableBroadcasting&&ž[i].Enabled){ΔΤ=true;break;}if(ž.Count==0) Κ="No Antenna found";else if(!ΔΤ) Κ="Antenna not ready";Εϊ=ž.Count==0||!ΔΤ;if(ΖЃ&&!Εϊ&&Μ!=Λ.ћ) Κ="Antenna ok";}else if(Μ==Λ.ћ)
Κ="Offline - Enable in PB custom data";ΖЃ=Εϊ;if(ΗΦ){if(ǲ!=null) this.IGC.DisableBroadcastListener(ǲ);ǲ=null;}}catch{ǋ=false;}if(!ǋ){Κ="Reset custom data";ΖΫ(ΑΟ);}}void ΖΫ(String ΑΟ){String Θ=ΑΟ+"\n\n"+"Enable_Broadcast="+(Μ==Λ.ћ?"true":"false")+" \n";Θ+=Μ!=Λ.ћ?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Θ+"Broadcast_Channel=#default";}String ΒΒ(String Ś){int Β=Ś.IndexOf("//");if(Β!=-1) Ś=Ś.Substring(0,Β);String[]φ=Ś.Split('=');if(φ.Length<=1) return "";return φ[1].Trim();}String ΑЁ(String[]Ξ,String ѓ,ref bool Ā){foreach(String Ś in Ξ)
if(Ś.StartsWith(ѓ)) return Ś;Ā=false;return "";}void ΘЌ(string Ф){if(đ) Н=new List<ϋ>();String Ș="";String ȑ="";if(!Γϊ(Ф)) Ιο(Ф);bool ΘΟ=false;int ΕΝ=50;while(ǲ!=null&&ǲ.HasPendingMessage&&ΕΝ-->0){MyIGCMessage β=ǲ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;
if(Ζκ(ref Θ,out Ș,out ȑ)&&Ș!=""&&Ș!=BROADCAST_SELF_ID){ϋ Δ=Γι(Ș,"");if(Δ==null){Δ=new ϋ(Ș);Н.Add(Δ);}Δ.ĩ(ȑ,false,false,true);ΚΖ(Δ,Θ);ΘΟ=true;}}if(ΘΟ)
Н.Sort(delegate(ϋ a,ϋ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ė||đ){if(Αγ<=0||đ){Κ="";đ=false;Ιϊ();ΖΪ();ΔА=DateTime.Now;}}if(ĺ) ΙЏ();if(ĺ) ΚΒ();if(ĺ) ΚΛ();if(ĺ) ΚΗ();ΙЍ();}void ΚΖ(ϋ Δ,String Θ)
{Δ.ΔЍ=DateTime.Now;String ΑΙ="";з(ref Θ,out Δ.ΓΠ,BROADCAST_SEP);з(ref Θ,out Δ.ΰ,BROADCAST_SEP);if(Δ.ΓΠ!=VERSION) return;String Αά="";String[]ƙ=new string[3];з(ref Θ,out Αά,BROADCAST_SEP);з(ref Θ,out ƙ[0],BROADCAST_SEP);з(ref Θ,out ƙ[1],BROADCAST_SEP);з(ref Θ,out ƙ[2],BROADCAST_SEP);
з(ref Θ,out Δ.Εη,BROADCAST_SEP);з(ref Θ,out Δ.Κ,BROADCAST_SEP);з(ref Θ,out ΑΙ,BROADCAST_SEP);з(ref Θ,out Δ.ΒЄ,BROADCAST_SEP);String Θν="",ΗЉ="";з(ref Θ,out Θν,BROADCAST_SEP);з(ref Θ,out ΗЉ,BROADCAST_SEP);Δ.Ќ.Clear();while(true){String Ηψ;if(!з(ref Θ,out Ηψ,BROADCAST_SEP)) break;String[]Βψ=Ηψ.Split('/');
if(Βψ.Count()<3) continue;int Ћ=0;if(!int.TryParse(Βψ[2],out Ћ)) continue;Δ.Ќ.Add(new о(Βψ[0],Βψ[1],Ћ,Щ.Ŵ));}int ΗЍ=0;if(!int.TryParse(ΑΙ,out Δ.Џ)) Δ.Џ=0;if(!int.TryParse(Αά,out ΗЍ)) Δ.Μ=Λ.ī;if(!float.TryParse(ƙ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ƙ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(ƙ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(Θν,out Δ.ȏ)) Δ.ȏ=0;if(!float.TryParse(ΗЉ,out Δ.Ȗ)) Δ.Ȗ=0;Δ.Μ=(Λ) ΗЍ;Δ.Δς=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Εσ=0;for(int i=0;i<Δ.Ќ.Count();i++) Δ.Εσ+=Δ.Ќ[i].Ћ;}ϋ Γι(String Υ,String ΰ){Υ=Υ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Н.Count;i++){if(Υ!=""&&Н[i].Υ.ToUpper()==Υ) return Н[i];if(ΰ!=""&&Н[i].ΰ!=null&&Н[i].ΰ.ToUpper()==ΰ) return Н[i];}return null;}String ƅ="";ϋ ΗΓ=null;DateTime ΗΔ=DateTime.MinValue;List<ϋ>Đ=new List<ϋ>();const float Θό=20f;bool Ζσ(ϋ η,String Ѕ){return Ѕ==ƅ&&η==ΗΓ&&((DateTime.Now-ΗΔ).TotalSeconds<Θό||
(Ѕ=="HOMEPOS"&&(ȱ.Count>0||Ƿ!=null)));}String ΙΕ(ϋ η){if(ƅ==""||!Ζσ(η,ƅ)) return "";String s="";int Ā=0,Εβ=0;for(int i=0;i<Đ.Count;i++){Њ ΓΔ=Đ[i].ĩ(ƅ,false,false,false);if(ΓΔ==Њ.ƹ) Ā++;bool Αΰ=ȱ.Contains(Đ[i]);if(Εβ++<8) s+="  "+Đ[i].ΰ+": "+(Αΰ?"queued ("+(ȱ.IndexOf(Đ[i])+1)+")":ΓΔ==Њ.ƹ?"received":
ΓΔ==Њ.Ź?"pending...":ΓΔ==Њ.ΑА?"no answer!":"-")+"\n";}if(Εβ>8) s+="  +"+(Εβ-8)+" more\n";return "--------------------------------------------\n"+ƅ+": "+Ā+"/"+Đ.Count+" received\n"+s;}void ǯ(ϋ Δ,String Ѕ){ƅ=Ѕ;ΗΓ=Δ;ΗΔ=DateTime.Now;Đ.Clear();if(Δ==null) Đ.AddRange(Н);else Đ.Add(Δ);
if(Δ==null){ȱ.Clear();Ƿ=null;}else ȱ.Remove(Δ);if(Δ==null&&Ѕ=="HOMEPOS"){ΙЂ();return;}if(Δ==null){for(int i=0;i<Н.Count;i++)
Н[i].ĩ(Ѕ,true,false,false);ΒΖ(Ѕ,"*",Ѕ);}else{Δ.ĩ(Ѕ,true,false,false);ΒΖ(Ѕ,Δ.Υ,Ѕ);}}void Ιο(string Ф){if(Ф=="") return;var φ=Ф.ToUpper().Split(' ');φ.DefaultIfEmpty("");var Ѕ=φ.ElementAtOrDefault(0);var ȩ=φ.ElementAtOrDefault(1);String Ȳ="Invalid argument: "+Ф;if(ύ==Σ.Ƽ){if(М!=null)
{switch(Ѕ){case "UP":{if(Џ<2) break;if(М.Џ==0){Џ=1;return;}ǯ(М,"UP");return;}case "DOWN":{if(Џ<1) break;if(Џ==1){ǯ(М,"MRES");break;}ǯ(М,"DOWN");break;}case "APPLY":{if(Џ<2) break;ǯ(М,"APPLY");return;}}}}switch(Ѕ){case "UP":this.ΓϏ(false);return;case "DOWN":this.Γώ(false);return;
case "APPLY":this.ů(true);return;}switch(Ѕ){case "CLEAR":ΘІ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȩ)){Κ="Usage: SENDALL <cmd>";return;}ǯ(null,ȩ);return;case "SEND":
if(Ф.Length<="SEND".Length+1){Κ="Usage: SEND <ship>:<cmd>";return;}Ιύ(Ф.Remove(0,"SEND".Length+1));return;}Κ=Ȳ;}void Ιύ(String ǃ){if(ǃ=="") return;var φ=ǃ.Split(':');if(φ.Length!=2){Κ="Missing separator \":\"";return;}φ.DefaultIfEmpty("");String ơ=φ.ElementAtOrDefault(0).Trim();
ϋ Δ=Γι("",ơ);if(Δ!=null) ǯ(Δ,φ.ElementAtOrDefault(1).Trim());else Κ="Unknown ship: "+ơ;}void ΘІ(){Н.Clear();М=null;ν(Σ.Main);Џ=0;}void Ιϊ(){List<IMyTerminalBlock>Ακ=new List<IMyTerminalBlock>();υ.GetBlocksOfType(Ч,ǚ);υ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ακ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ч=ǭ(Ч,pamTag,true);Ζρ(Ακ);Ζπ();Ιϋ();ŉ(Ч,setLCDFontAndSize,1.15f,false);ŉ(Ț,setLCDFontAndSize,1.15f,true);String Κψ=ΖΝ(false);String ΘΑ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock щ in Ч){if(щ.CustomData==""){щ.CustomData=ΖΝ(true);continue;}
if(!щ.CustomData.Contains(Κψ))
if(!щ.CustomData.Contains(ΘΑ)) щ.CustomData=ΘΑ+щ.CustomData;}}void ΙЍ(){String ΑΖ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ț.Count;i++)
Ț[i].WriteText(ΕЄ(У.Ζγ,"0",false));for(int i=0;i<Ч.Count;i++){У А=У.ī;String ǽ="";Ικ(Ч[i],out А,out ǽ);Ч[i].WriteText(ΕЄ(А,ǽ,i==0));}Echo(ΑΖ);for(int i=0;i<ő.Count;i++){IMyTextPanel щ=ő[i];String Θ=щ.CustomData.ToUpper();if(Θ=="DEBUG") щ.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") щ.WriteText(ΖΜ());
}}String ů(bool Ι){int Κν=0;return ů(Ι,0,ref Κν,false,1);}String ů(bool Ι,int ΛΔ,ref int ΛΤ,bool ΗΗ,int Αή){String Γ="";String Ơ="——————————————————\n";String Λγ="--------------------------------------------\n";if(М==null||ύ==Σ.Main||ύ==Σ.ΔΑ)
Γ+="[PAM]-Controller | "+Н.Count+" Connected ships"+"\n"+ΙΘ()+ΙΝ()+ΙΣ();else Γ+=ΖΠ(М)+"\n";Γ+=Ơ;int Β=0;int Η=Џ;Ŝ=0;if(Κ!=""){Ν(ref Γ,Η,Β++,Ι,Κ);}else if(ύ==Σ.Main){if(М==null&&Н.Count>=1) М=Н[0];String Ў="";if(Ν(ref Ў,Η,Β++,Ι," Send command to all")) ν(Σ.ΔΑ);
int Ɵ=0;for(int s=0;s<Н.Count;s++){if(ΗΗ) Ў+="\n";ϋ Δ=Н[s];if(Η==Β||Η==Β+1) М=Δ;if(Β==Η) Ɵ=Ў.Split('\n').Length-1;if(Ν(ref Ў,Η,Β++,Ι," "+ΖΠ(Δ))){М=Δ;ν(Σ.Ƽ);М.ĩ("",false,true,false);}if(Β==Η) Ɵ=Ў.Split('\n').Length-1;if(Ν(ref Ў,Η,Β++,Ι," "+(ΗΗ?"":"  ")+ΙΟ(Δ)))
{М=Δ;ν(Σ.Ζς);}}int ƫ=Αή-2;ƫ+=Math.Max(0,(ΛΔ-1))*Αή;Γ+=ΓБ(ƫ,Ў,Ɵ,ref ΛΤ);}else if(ύ==Σ.ΒΗ||ύ==Σ.ΔΑ){Σ ΚΥ=ύ==Σ.ΒΗ?Σ.Ƽ:Σ.Main;ϋ η=null;if(ύ==Σ.ΒΗ) η=М;if(Ν(ref Γ,Η,Β++,Ι," Back")){ν(ΚΥ);}if(Ν(ref Γ,Η,Β++,Ι," [Stop] "+ų(η,"STOP"))){ǯ(η,"STOP");}if(Ν(ref Γ,Η,Β++,Ι," [Continue] "+ų(η,"CONT"))){ǯ(η,"CONT");}
if(Ν(ref Γ,Η,Β++,Ι," [Move home] "+ų(η,"HOMEPOS"))){ǯ(η,"HOMEPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Move to job] "+ų(η,"JOBPOS"))){ǯ(η,"JOBPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Full simulation] "+ų(η,"FULL"))){ǯ(η,"FULL");}if(η!=null&&η.Μ==Λ.ή)
if(Ν(ref Γ,Η,Β++,Ι," [Undock] "+ų(η,"UNDOCK"))){ǯ(η,"UNDOCK");}Γ+=ΙΕ(η);}else if(ύ==Σ.Ƽ){String ΔΫ="";if(Ν(ref ΔΫ,Η,Β++,Ι," Back")){ν(Σ.Main);}Γ+=ΔΫ.Substring(0,ΔΫ.Length-1).PadRight(36,' ');if(Ν(ref Γ,Η,Β++,Ι," Send cmd")){ν(Σ.ΒΗ);}if(!М.Γϋ()&&!М.Γχ()) Ŝ++;
if(М.Γχ()) Γ+="No answer received...";else Γ+=Ż(М,М.ΒЄ);}else if(ύ==Σ.Ζς){if(Ν(ref Γ,Η,Β++,Ι," Back")){ν(Σ.Main);}Γ+=Λγ;Γ+=Ż(М,ΖΟ(М));}if(!ΕΜ) Џ=Math.Min(Ŝ-1,Џ);return Γ;}String ΖΠ(ϋ Δ){if(Δ.ΓΠ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ż(Δ,Δ.Εη)+" "+Δ.Δς+"m";
}String ΙΟ(ϋ Δ){String s=ΓЎ("",К(Δ.ȏ,Δ.Ȗ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ќ.Count;i++){if(i>=5) break;о Ϗ=Δ.Ќ[i];s+=Ζύ(Γά(ΙΜ(Ϗ)),3)+" "+ΖΖ(Ϗ.Ћ)+" ";}return s;}String ΙΜ(о Ϗ){if(Ϗ.χ.ToUpper()=="ORE"||Ϗ.χ.ToUpper()=="INGOT"){String ΗΕ=ΙΚ(Ϗ.ΰ.ToUpper());if(ΗΕ!="") return ΗΕ;
}return Ϗ.ΰ;}String Ż(ϋ Δ,String Γ){if(Δ.Γϋ()) return "No signal...("+ΑЄ((int)(DateTime.Now-Δ.ΔЍ).TotalSeconds)+")";return Γ;}String ΖΝ(bool Λϊ){String Γ="";if(Λϊ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void Ικ(IMyTerminalBlock щ,out У А,out String ǽ){bool Ā=true;String Αά=ΒΒ(ΑЁ(щ.CustomData.Split('\n'),"mode",ref Ā)).ToUpper();String[]φ=Αά.Split(':');String ΑΫ=φ.Length>0?φ[0].Trim():"";ǽ=φ.Length>1?φ[1].Trim():"";
А=У.ī;if(ΑΫ=="MAIN") А=У.Ζδ;else if(ΑΫ=="MAINX") А=У.Ζε;else if(ΑΫ=="MENU") А=У.Γύ;else if(ΑΫ=="INVENTORY") А=У.Γφ;else if(ΑΫ=="DEBUG") А=У.Γΰ;if(А==У.ī) ǽ=Αά;}String ΕЄ(У А,String ǽ,bool Κο){int ΑΨ=15;if(Κο){ΗΙ=ΒΪ;ΒΪ=0;ΗΚ=ΒΫ;ΒΫ=0;}String Γ="";if(А==У.Ζδ)
{int ш=0;Γ=ů(false,ΗΙ,ref Κί,true,ΑΨ);if(!int.TryParse(ǽ,out ш)) return Γ;ΒΪ=Math.Max(ш,ΒΪ);ш--;return Ζι(ш,ΑΨ,Γ);}else if(А==У.Ζε){int ш=0;Γ=ů(false,ΗΚ,ref Κΰ,false,ΑΨ);if(!int.TryParse(ǽ,out ш)) return Γ;ΒΫ=Math.Max(ш,ΒΫ);ш--;return Ζι(ш,ΑΨ,Γ);}if(А==У.Ζγ){return ů(false,0,ref Κα,true,ΑΨ);
}else if(А==У.Γφ||А==У.Γύ||А==У.Γΰ){ϋ Δ=М;if(ǽ!=""){Δ=Γι("",ǽ);if(Δ==null) return "Unknown ship: "+ǽ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ơ="—————————————————————\n";String Επ="";String Δα="";if(А==У.Γύ){Επ="Menu";Δα=Ż(Δ,Δ.ΒЄ);}
else if(А==У.Γφ){Επ="Inventory";Δα=Ż(Δ,ΖΟ(Δ));}else if(А==У.Γΰ){return Ż(Δ,ΙΠ(Δ));}Γ+=Δ.ΰ+" - "+Δ.Δς+"m | "+Γά(""+Επ)+"\n"+Ơ;Γ+=Δα;return Γ;}return "Unknown command: "+ǽ;}string ΙΠ(ϋ Δ){String s="";s+=Δ.ΓΠ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Εη+"\n";s+=Δ.Κ+"\n";s+=Δ.Џ+"\n";s+=Δ.ΒЄ.Length+"\n";s+=Δ.ȏ+"\n";s+=Δ.Ȗ+"\n";s+=Δ.Ќ.Count()+"\n";return s;}string ΖΟ(ϋ Δ){String s="";s+=ΓЎ("All",К(Δ.ȏ,Δ.Ȗ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ќ.Count();i++){о Ϗ=Δ.Ќ[i];
s+=ΓЎ(Ϗ.ΰ,Ϗ.Ћ,Δ.Εσ,30,8,10)+"\n";}return s;}Њ ΙΖ(ϋ Δ,bool б,string Ѕ){Њ ί=Њ.None;if(Δ==null){for(int i=0;i<Н.Count;i++){Њ І=Н[i].ĩ(Ѕ,false,б,false);if(І==Њ.ƹ) ί=І;if(І==Њ.ΑА) return І;if(І==Њ.Ź) return І;}return ί;}else return Δ.ĩ(Ѕ,false,б,false);}String ų(ϋ Δ,String Ѕ)
{if(!Ζσ(Δ,Ѕ)) return "";Њ І=ΙΖ(Δ,false,Ѕ);if(І==Њ.ƹ) return "received!";if(І==Њ.Ź) return "pending...";if(І==Њ.ΑА) return "no answer!";return "";}String ΓБ(int ƫ,String Γ,int Ɵ,ref int ĝ){String[]Ξ=Γ.Split('\n');if(Ɵ>=ĝ+ƫ-1) ĝ++;ĝ=Math.Min(Ξ.Count()-1-ƫ,ĝ);if(Ɵ<ĝ+1) ĝ--;
ĝ=Math.Max(0,ĝ);String ί="";for(int i=0;i<ƫ;i++){int ΗЁ=i+ĝ;if(ΗЁ>=Ξ.Count()) break;ί+=Ξ[ΗЁ]+"\n";}return ί;}String Ζι(int ш,int Αή,String Γ){String[]Ξ=Γ.Split('\n');int Αη=ш*Αή;int ȯ=(ш+1)*Αή;String ί="";for(int i=Αη;i<ȯ;i++){if(i>=Ξ.Count()) break;ί+=Ξ[i]+"\n";
}return ί;}String Γά(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζύ(String s,int ΗЇ){if(s=="") return s;if(s.Length>ΗЇ) s=s.Substring(0,ΗЇ-1)+".";return s;}string ΓЎ(String ř,float value,float Ƒ,int ΖЊ,int Ηό,int ΛΓ){float Ţ=К(value,Ƒ)*ΖЊ;
String ΒΧ="[";for(int i=0;i<ΖЊ;i++){if(i<=Ţ) ΒΧ+="|";else ΒΧ+="'";}ΒΧ+="]";return ΒΧ+" "+Ζύ(Γά(ř),Ηό).PadRight(Ηό)+"".PadRight(ΛΓ)+ΖΖ(value);}String ΖΖ(float Ћ){if(Ћ>=1000000) return Math.Round(Ћ/1000000f,Ћ/1000000f<100?1:0)+"M";if(Ћ>=1000) return Math.Round(Ћ/1000f,Ћ/1000f<100?1:0)+"K";
return ""+Math.Round(Ћ);}String ΑЄ(int Αδ){if(Αδ>=60*60) return Math.Round(Αδ/(60f*60f),1)+" h";if(Αδ>=60) return Math.Round(Αδ/60f,1)+" min";return ""+Αδ+" s";}string Ų(float v){return Math.Round(v,2)+" ";}string ΛЁ(Vector3 v){return "X"+Ų(v.X)+"Y"+Ų(v.Y)+"Z"+Ų(v.Z);}
List<IMyCameraBlock>Ȍ=new List<IMyCameraBlock>();List<long>ƕ=new List<long>();bool Ɨ=false;int Δθ=0,ΛΠ=0,Δί=0;const int Ιτ=16;int Δζ=0;DateTime ΖЍ=DateTime.Now;String Δΰ="";String Δε="";void Ιω(){Ȍ.Clear();ƕ.Clear();if(!obstacleBrake) return;υ.GetBlocksOfType(Ȍ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ȍ.Count;i++) Ȍ[i].EnableRaycast=true;List<IMyTerminalBlock>ΒΣ=new List<IMyTerminalBlock>();υ.GetBlocksOfType(ΒΣ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΒΣ.Count;i++) if(!ƕ.Contains(ΒΣ[i].CubeGrid.EntityId)) ƕ.Add(ΒΣ[i].CubeGrid.EntityId);if(!ƕ.Contains(Me.CubeGrid.EntityId)) ƕ.Add(Me.CubeGrid.EntityId);
Δε="";if(Ω==null) return;MatrixD m=Ω.WorldMatrix;Vector3[]Κθ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]ΚЊ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ȍ.Count;i++)
if(Vector3.Dot(Ȍ[i].WorldMatrix.Forward,Κθ[d])>0.7f){Δε+=ΚЊ[d]+" ";break;}}bool Ζή(Α s){return s==Α.Ȇ||s==Α.х||s==Α.Й||
s==Α.Ĩ||s==Α.ű||s==Α.ΑЂ||
(s==Α.ȅ&&ǣ!=null);}void Αύ(){Ɨ=false;Δθ=0;Δΰ="";}void ΚΔ(){if(!obstacleBrake||Ȍ.Count==0||!Ζή(Ϋ)){Αύ();return;}Vector3 ΑΚ=Ѓ+Ŀ;Vector3 ț=ΑΚ-δ;float ȁ=ț.Length();if(ȁ<1f){Αύ();return;}Vector3 ξ=ț/ȁ;Vector3 м=Ω!=null?(Vector3) Ω.GetShipVelocities().LinearVelocity:new Vector3();
if(м.Length()>2) ξ=Vector3.Normalize(м);Vector3 ΒБ=Vector3.Cross(ξ,Math.Abs(ξ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΒБ.Normalize();Vector3 Ї=Vector3.Cross(ΒБ,ξ);float σ=Math.Min(ȁ+λ*0.25f,Ğ*obstacleLookAhead+λ);int ΖА=0;for(int k=0;k<2;k++){int p=k==0?0:ΛΠ++%8,e=p%4;
Vector3 ΚА=k==0?new Vector3():(e==0?ΒБ:e==1?-ΒБ:e==2?Ї:-Ї)*λ*0.5f;Vector3 ȯ=δ+ξ*(p>=4?σ*0.5f:σ)+ΚА;IMyCameraBlock Į=ΘЀ(ȯ);if(Į==null){ΕЎ();continue;}ΖА++;MyDetectedEntityInfo Ǎ=Į.Raycast(ȯ);bool Λτ=Ǎ.Type==MyDetectedEntityType.Planet||Ǎ.Type==MyDetectedEntityType.Asteroid;
bool ΚЏ=!Ǎ.IsEmpty()&&!ƕ.Contains(Ǎ.EntityId)&&!Ιβ(Ǎ.EntityId)&&
Ǎ.Type!=MyDetectedEntityType.FloatingObject&&!(Λτ&&(k>0||Ϋ==Α.Ȇ||
(Ǎ.HitPosition.HasValue&&Vector3D.Distance(Ǎ.HitPosition.Value,ΑΚ)<λ)));if(ΚЏ){if(!Ɨ) ΖЍ=DateTime.Now;Ɨ=true;Δθ=0;Δΰ=Ǎ.Type.ToString();}else ΕЎ();}Δί=ΖА>0?0:Δί+1;if(Δί>12) Κ="Cameras can't see ahead";if(Ɨ&&(DateTime.Now-ΖЍ).TotalSeconds>obstacleTimeout)
Κ="Path blocked: "+Δΰ;}void ΕЎ(){if(Ɨ&&++Δθ>=Ιτ) Αύ();}IMyCameraBlock ΘЀ(Vector3 ȯ){IMyCameraBlock Ъ=null;float ΖЌ=-2;for(int n=0;n<Ȍ.Count;n++){Δζ=(Δζ+1)%Ȍ.Count;IMyCameraBlock c=Ȍ[Δζ];if(!c.IsFunctional||!c.IsWorking) continue;float ΘΜ=Vector3.Dot(Vector3.Normalize(ȯ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(ΘΜ>ΖЌ+0.05f&&c.CanScan(ȯ)){Ъ=c;ΖЌ=ΘΜ;}}return Ъ;}const String ΔΝ="[PAMTFC]";const float ΙЇ=5f;const float Γθ=2f;class Ī{public long Υ;public Vector3 γ,м;public Vector3 ē;public float ΗΥ;public String Φ="";public int Ĕ=-1;public bool Αΰ;public int ǹ;public float ΑΜ,ΑΘ;public long α=0;
public int ΑΒ=0;public DateTime ľ;}IMyBroadcastListener ΓΛ=null;long ǔ=0;List<Ī>ї=new List<Ī>();Vector3 Ŀ=new Vector3();float Ǘ=-1;Vector3 Ετ=new Vector3();bool ΓΚ=false;DateTime Ευ=DateTime.Now;const float Ζό=15f;const float ΙІ=10f;int Θκ=0;String д="";int Г=-1;bool Βή=false,Βέ=false;
Vector3 ΗΛ=new Vector3();String Ŷ(){if(ε==Ρ.я||Μ==Λ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ѝ+"|"+Ю+"|"+(int) Ř+"|"+Math.Round(ȃ,1)+"|"+Math.Round(ȝ,1);}void ΚΙ(){if(!trafficEnabled) return;if(ΓΛ==null) ΓΛ=IGC.RegisterBroadcastListener(ΔΝ);ǔ=Me.CubeGrid.EntityId;int ȋ=30;while(ΓΛ.HasPendingMessage&&ȋ-->0){String Θ=ΓΛ.AcceptMessage().Data as String;if(Θ!=null) Ιμ(Θ);
}for(int i=ї.Count-1;i>=0;i--)
if((DateTime.Now-ї[i].ľ).TotalSeconds>ΙЇ) ї.RemoveAt(i);if(ΓΗ==null) ΓΗ=IGC.RegisterBroadcastListener(Ζϊ);ȋ=5;while(ΓΗ.HasPendingMessage&&ȋ-->0){String Θ=ΓΗ.AcceptMessage().Data as String;if(Θ!=null) try{Ιν(Θ);}catch{}}if(Βί==null) Βί=IGC.RegisterBroadcastListener(Ζΰ);
ȋ=10;while(Βί.HasPendingMessage&&ȋ-->0){String Θ=Βί.AcceptMessage().Data as String;if(Θ!=null) try{Ιι(Θ);}catch{}}if(++Θκ<3) return;Θκ=0;IGC.SendBroadcastMessage(ΔΝ,ǔ+";"+τ(δ)+";"+
τ(Ω.GetShipVelocities().LinearVelocity)+";"+λ+";"+Ŷ()+";"+
(ε==Ρ.И?Г:-1)+";"+д+";"+ΘЅ()+","+ΖΒ()+";"+τ(Ετ)+
";"+(Βθ?1:0)+";"+(Ķ&&ε==Ρ.И?1:0)+","+ΒЎ+","+Εΰ+","+Ιε());}void Ιμ(String Θ){try{String[]f=Θ.Split(';');if(f.Length<7) return;long Υ=long.Parse(f[0]);if(Υ==ǔ) return;Ī Δ=null;for(int i=0;i<ї.Count;i++) if(ї[i].Υ==Υ) Δ=ї[i];if(Δ==null){Δ=new Ī();Δ.Υ=Υ;ї.Add(Δ);}
Δ.γ=ρ(f[1]);Δ.м=ρ(f[2]);Δ.ΗΥ=float.Parse(f[3]);Δ.Φ=f[4];Δ.Ĕ=int.Parse(f[5]);Δ.ľ=DateTime.Now;if(Δ.Φ!=""&&Δ.Φ==Ŷ()) Ζζ(f[6]);Δ.α=0;Δ.ΑΒ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ΑΒ=int.Parse(d[1]);}Δ.ē=f.Length>=9?ρ(f[8]):new Vector3();
Δ.Αΰ=f.Length>=10&&f[9]=="1";Δ.ǹ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ΑΜ=float.Parse(w[1]);Δ.ΑΘ=float.Parse(w[2]);Δ.ǹ=int.Parse(w[3]);}}}catch{}}bool Ιβ(long Υ){for(int i=0;i<ї.Count;i++) if(ї[i].Υ==Υ) return true;return false;}Vector3 ΖΞ(int Β,int ΛΕ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();ά έ=Ψ[Β];Vector3 Λπ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*ΛΕ;Vector3 Ї=έ.ψ.LengthSquared()>0.01f?-Vector3.Normalize(έ.ψ):-έ.θ;Vector3 т=Vector3.Cross(Λπ,Ї);if(т.LengthSquared()<0.0001f) return new Vector3();
float σ=laneOffset*λ;float Κρ=Math.Min(Vector3.Distance(έ.Ε,Ψ[0].Ε),
Vector3.Distance(έ.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(т)*σ*Math.Min(1,Κρ/(σ*4));}Vector3 Θι=new Vector3();bool ΖΧ(){Α s=Ϋ;return Ŷ()!=""&&(s==Α.Ȇ||s==Α.ŷ||s==Α.ȥ||
s==Α.Ȥ||s==Α.Ű);}int Ιε(){if(!ΖΧ()) return 0;return Ϋ!=Α.Ȇ&&Vector3.Dot(δ-ѕ,Ǔ)>λ*0.5f?2:1;}bool Ια(Α s){return s==Α.ȅ||s==Α.ł||s==Α.ņ||s==Α.ƺ||
s==Α.Ǳ||s==Α.ȥ||s==Α.Ȥ||s==Α.Ű||s==Α.Ĥ;}void ΚΚ(){Ŀ=new Vector3();Ǘ=-1;Ετ=new Vector3();bool ΑΛ=Ζή(Ϋ);if(!trafficEnabled||(!ΑΛ&&!Ια(Ϋ))){ΓΚ=false;return;}Θι=Ω.GetShipVelocities().LinearVelocity;ΘЇ(Ω.GetNaturalGravity(),!ΑΛ);Θτ();}IMyBroadcastListener ΓΗ=null;
float ƥ=-1;DateTime Θε=DateTime.MinValue;void Ιν(String Θ){if(!Θ.StartsWith("C;")) return;Θε=DateTime.Now;ƥ=-1;String ΑΪ=ǔ+":";String[]e=Θ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΪ)) ƥ=float.Parse(e[i].Substring(ΑΪ.Length));}void Θτ(){if(ƥ<0||(DateTime.Now-Θε).TotalSeconds>1.5) return;
Ǘ=Ǘ<0?ƥ:Math.Min(Ǘ,ƥ);if(ƥ<0.5f) Κ="Traffic: giving way (controller)";}void ΘЇ(Vector3 ψ,bool ΘΖ=false){Ŀ=new Vector3();Ǘ=-1;Vector3 ț=Ѓ-δ;float ȁ=ț.Length();bool ΔЄ=ȁ>=1f;Vector3 ξ=ΔЄ?ț/ȁ:new Vector3();Ετ=ξ;Vector3 Ї=ψ.LengthSquared()>0.01f?-Vector3.Normalize(ψ):-ΑΔ;
Vector3 т=Vector3.Cross(ξ,Ї);т=т.LengthSquared()<0.0001f?-ΑΧ:Vector3.Normalize(т);Ī Βϊ=null;bool Ηε=Ϋ==Α.Й&&!č(1);for(int i=0;i<ї.Count;i++){Ī κ=ї[i];if(κ.ΑΒ==3&&κ.м.LengthSquared()<0.25f) continue;Vector3 γ=κ.γ+κ.м*(float)(DateTime.Now-κ.ľ).TotalSeconds;Vector3 ǥ=γ-δ;
float б=(λ+κ.ΗΥ)*0.5f;bool Αβ=κ.Φ!=""&&κ.Φ==Ŷ();if(Αβ&&κ.ǹ==2) continue;bool ΓΑ=Αβ&&κ.ǹ==1&&ΖΧ();if(ΓΑ) б=Math.Min(ȃ,ȝ)*0.9f;float Δμ=б*(ΓΑ?1f:1.2f);float ǂ=ΔЄ?Vector3.Dot(ǥ,ξ):0;if(ǂ>0&&(ǥ-ξ*ǂ).Length()<=Δμ){float ΒЈ=Vector3.Dot(κ.м,ξ);bool Ηι=κ.ē.LengthSquared()>0.25f;
bool Ηκ=ΒЈ<-1f||(Ηι&&Vector3.Dot(κ.ē,ξ)<-0.5f);if(!ΘΖ){if(κ.Αΰ){if(!Ηε){Ŀ=т*б*1.3f;continue;}float ΗЌ=Vector3.Distance(δ,Ѓ),Ηω=Vector3.Distance(γ,Ѓ);if(ΗЌ<Ηω||(ΗЌ==Ηω&&ǔ<κ.Υ)) continue;}if(Ηκ){if(ǂ<(Ğ-ΒЈ)*6f+б*4f) Ŀ=т*б*1.3f;continue;}if(Ηι&&ǔ<κ.Υ)
{Vector3 ΖЈ=-ǥ;float ΘΔ=Vector3.Dot(ΖЈ,κ.ē);if(ΘΔ>0&&(ΖЈ-κ.ē*ΘΔ).Length()<=Δμ){Ŀ=т*б*1.3f;continue;}}}float Ηδ=ǂ-б-(ΓΑ?1f:followGap*λ);float Ȕ=Math.Max(0,ΒЈ)+Math.Max(0,Ηδ)/Γθ;if(Ηκ) Ȕ=Math.Max(0,Ηδ+Math.Min(0,ΒЈ)*Γθ*2)/Γθ;if(Ǘ<0||Ȕ<Ǘ){Ǘ=Ȕ;Βϊ=κ;}
continue;}if(κ.м.LengthSquared()<1f) continue;if(ΔЄ&&(ǥ-ξ*ǂ).Length()<=Δμ) continue;Vector3 Εή=κ.м-Θι;float ΘΛ=Εή.LengthSquared();if(ΘΛ<0.25f) continue;float t=-Vector3.Dot(ǥ,Εή)/ΘΛ;if(t<=0||t>5f||(ǥ+Εή*t).Length()>=(ΓΑ?б:б*1.1f)) continue;if(ǔ>κ.Υ||t<2f){Ǘ=0;Βϊ=κ;}
}bool ΑΠ=Ǘ>=0&&Ǘ<0.5f;if(!ΑΠ||ΘΖ||(Βϊ!=null&&Βϊ.Αΰ&&Ηε)){ΓΚ=false;return;}if(!ΓΚ){ΓΚ=true;Ευ=DateTime.Now;}double Ηλ=(DateTime.Now-Ευ).TotalSeconds;if(Ηλ>Ζό+ΙІ) Ευ=DateTime.Now;else if(Ηλ>Ζό){Ǘ=-1;Ŀ=т*λ*1.3f;Κ="Traffic: passing a stuck ship";}
}List<int>Ȱ=new List<int>();String Ην="";void Γη(){String ѓ=Ѝ+"|"+Ю+"|"+(int) Ř;if(ѓ==Ην) return;Ην=ѓ;ΕЅ(Ѝ,Ю,Ř,Ȱ);}void ΕЅ(int w,int h,ѐ Αη,List<int>Δη){Δη.Clear();if(Αη==ѐ.ƾ){for(int k=0;k<w*h;k++){int ƛ=k/w;Δη.Add(ƛ*w+(ƛ%2==0?k-ƛ*w:w-1-(k-ƛ*w)));
}return;}int ΑΞ=(int) Math.Ceiling(w/2f),ΑΝ=(int) Math.Ceiling(h/2f);int ΔЁ=w/2,ΔЀ=h/2;int x=0,y=0,Δυ=0,Δφ=-1,Ƒ=Math.Max(w,h);for(int i=0;i<Ƒ*Ƒ;i++){if(-ΑΞ<x&&x<=ΔЁ&&-ΑΝ<y&&y<=ΔЀ) Δη.Add((y-1+ΑΝ)*w+(x-1+ΑΞ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Δυ;Δυ=-Δφ;Δφ=t;}
x+=Δυ;y+=Δφ;}}bool Γζ(){int n=Math.Max(0,Ѝ*Ю);if(д.Length==n) return false;д=new String('0',n);Г=-1;return true;}void Ζζ(String κ){Γζ();if(κ.Length!=д.Length) return;char[]h=д.ToCharArray();for(int i=0;i<h.Length;i++) if(κ[i]=='1') h[i]='1';д=new String(h);}bool ΑЉ(int Ы){return Ы>=0&&Ы<д.Length&&д[Ы]=='1';}
void ΔΓ(int Ы,bool Ѐ){if(Ы<0||Ы>=д.Length) return;char[]h=д.ToCharArray();h[Ы]=Ѐ?'1':'0';д=new String(h);}bool Γσ(int Ы,bool ΚϏ){if(!trafficEnabled) return false;String ѓ=Ŷ();for(int i=0;i<ї.Count;i++){Ī t=ї[i];if(t.Φ!=ѓ||t.Ĕ<0||(ΚϏ&&t.Υ>ǔ)) continue;if(t.Ĕ==Ы) return true;
}return false;}int ΘЄ(int Β){int n=Ѝ*Ю;if(n<1) return Β;Γη();if(Γζ()) for(int k=0;k<Β&&k<n;k++) ΔΓ(Ȱ[k],true);if(Г>=0&&(Β>=n||Ȱ[Β]!=Г)){ΔΓ(Г,true);Г=-1;}if(Ζα()){if(Ȏ>=0&&!ΑЉ(Ȏ)){Ķ=false;Г=Ȏ;return Ȱ.IndexOf(Ȏ);}if(Ȏ==-2){Г=-1;Ķ=д.IndexOf('0')>=0;return Ķ?-2:n;}
if(Ȏ==-1&&Г>=0&&!ΑЉ(Г)){Ķ=false;return Β;}Г=-1;if(!Ķ){Ķ=true;ΗΝ=DateTime.Now;}if((DateTime.Now-ΗΝ).TotalSeconds<5) return-1;}Ķ=false;if(Г>=0&&!ΑЉ(Г)&&!Γσ(Г,true)) return Β;Г=-1;for(int k=0;k<n;k++){int c=Ȱ[k];if(ΑЉ(c)||Γσ(c,false)) continue;Г=c;
return k;}return д.IndexOf('0')>=0?-2:n;}const String Ζΰ="[PAMJOB]";const String Ζϊ="[PAMTCC]";IMyBroadcastListener Βί=null;DateTime ΗΜ=DateTime.MinValue;DateTime ΗΝ=DateTime.Now;int Ȏ=-1;bool Ķ=false;bool Ζα(){return trafficEnabled&&Ȏ!=-3&&(DateTime.Now-ΗΜ).TotalSeconds<3;}
void Ιι(String Θ){String[]f=Θ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ŷ()) return;ΗΜ=DateTime.Now;Ζζ(f[2]);Ȏ=-3;String ΑΪ=ǔ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΪ)) Ȏ=int.Parse(e[i].Substring(ΑΪ.Length));}ǰ Ιώ(int Β)
{Γη();if(Β<0||Β>=Ȱ.Count) return ǰ.Αχ;Ё=Ȱ[Β]%Ѝ;ώ=Ȱ[Β]/Ѝ;return ǰ.ΑЅ;}class Ǜ{public long Υ;public Vector3 γ,Э,Ї;}List<Ǜ>Х=new List<Ǜ>();bool ı=false;Vector3 Βκ,Βι,Βλ,Βμ;long Ǐ=0;long ā=0;ά ǣ=null;bool ΑΓ=false;ά Γγ(Vector3 γ,Vector3 Э,Vector3 Ї){MatrixD m=MatrixD.CreateWorld(γ,Э,Ї);
ά w=new ά(Ο);w.ι=Vector3D.TransformNormal(Βκ,m);w.θ=Vector3D.TransformNormal(Βι,m);w.Ђ=Vector3D.TransformNormal(Βλ,m);w.Ε=γ+(Vector3) Vector3D.TransformNormal(Βμ,m);w.μ=true;return w;}int ΖΒ(){Α s=Ϋ;if(s==Α.ю) return и(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ņ||s==Α.Ȩ||s==Α.Ņ||s==Α.ΒΟ||
s==Α.ΑЀ||s==Α.Ǳ) return 3;if(s==Α.ȅ||s==Α.ł||s==Α.ƺ) return 2;if(s==Α.Й) return 1;return 0;}long ΘЅ(){if(ΖΒ()==0) return 0;if(č(1)&&Ƣ!=0) return Ƣ;return ǣ!=null?ā:Ǐ;}void Ζβ(){if(Μ==Λ.ή||!Ο.μ) return;IMyShipConnector Р=и(MyShipConnectorStatus.Connected);if(Р==null||Р.OtherConnector==null) return;
IMyShipConnector κ=Р.OtherConnector;Vector3I Κω=new Vector3I((int) Ο.р.X,(int) Ο.р.Y,(int) Ο.р.Z);if(Р.Position==Κω){MatrixD Ƌ=MatrixD.Transpose(κ.WorldMatrix);Βκ=Vector3D.TransformNormal(Ω.WorldMatrix.Forward,Ƌ);Βι=Vector3D.TransformNormal(Ω.WorldMatrix.Down,Ƌ);Βλ=Vector3D.TransformNormal(Ω.WorldMatrix.Left,Ƌ);
Βμ=Vector3D.TransformNormal(Р.GetPosition()-κ.GetPosition(),Ƌ);ı=true;}if(Vector3.Distance(Р.GetPosition(),Ο.Ε)<5) Ǐ=κ.EntityId;List<IMyShipConnector>Ŕ=new List<IMyShipConnector>();υ.GetBlocksOfType(Ŕ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ŕ.Count>0){Х.Clear();for(int i=0;i<Ŕ.Count;i++){Ǜ p=new Ǜ();p.Υ=Ŕ[i].EntityId;p.γ=Ŕ[i].GetPosition();p.Э=Ŕ[i].WorldMatrix.Forward;p.Ї=Ŕ[i].WorldMatrix.Up;Х.Add(p);}}ǣ=null;ā=0;if(κ.EntityId==Ǐ||!ı) return;for(int i=0;i<Х.Count;i++)
if(Х[i].Υ==κ.EntityId){ā=Х[i].Υ;ǣ=Γγ(Х[i].γ,Х[i].Э,Х[i].Ї);}}bool Γω(long Υ,int ΗЏ){if(Υ==0) return false;for(int i=0;i<ї.Count;i++){Ī t=ї[i];if(t.α==Υ&&(t.ΑΒ>ΗЏ||(t.ΑΒ==ΗЏ&&t.Υ<ǔ))) return true;}return false;}bool Γή(int š){if(Μ==Λ.ή||!trafficEnabled){ǣ=null;ā=0;return true;}
long π=ǣ!=null?ā:Ǐ;if(!Γω(π,š)) return true;if(!Γω(Ǐ,š)){ǣ=null;ā=0;return true;}if(!ı) return false;Ǜ Ъ=null;float ǈ=0;for(int i=0;i<Х.Count;i++){Ǜ p=Х[i];if(p.Υ==Ǐ||Γω(p.Υ,š)) continue;float d=Vector3.Distance(p.γ,Ο.Ε);if(Ъ==null||d<ǈ){Ъ=p;ǈ=d;}
}if(Ъ==null) return false;ā=Ъ.Υ;ǣ=Γγ(Ъ.γ,Ъ.Э,Ъ.Ї);return true;}String ΘА(){String s=(ı?"1":"0")+":"+τ(Βκ)+":"+τ(Βι)+":"+
τ(Βλ)+":"+τ(Βμ)+":"+Ǐ;for(int i=0;i<Х.Count;i++)
s+=":"+Х[i].Υ+","+τ(Х[i].γ)+","+τ(Х[i].Э)+","+τ(Х[i].Ї);return s;}void ΘЏ(String s){Х.Clear();ı=false;Ǐ=0;String[]f=s.Split(':');if(f.Length<6) return;ı=f[0]=="1";Βκ=ρ(f[1]);Βι=ρ(f[2]);Βλ=ρ(f[3]);Βμ=ρ(f[4]);long.TryParse(f[5],out Ǐ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǜ p=new Ǜ();if(!long.TryParse(e[0],out p.Υ)) continue;p.γ=ρ(e[1]);p.Э=ρ(e[2]);p.Ї=ρ(e[3]);Х.Add(p);}}const String Ʊ="[PAMDCK]";const float Θϋ=120f;class Ĉ{public long Δ;public int ǡ;public long ǎ;public bool ΖЏ;public DateTime ǟ,ΑΦ;
}IMyBroadcastListener Βη=null;List<Ĉ>Ц=new List<Ĉ>();List<IMyShipConnector>ğ=new List<IMyShipConnector>();List<IMySensorBlock>Ć=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΥ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΓΡ=new List<MyDetectedEntityInfo>();long ǝ=0;int Δι=0;
long ΗΑ=0;DateTime ΗΒ=DateTime.Now;String Œ="";int ΗΪ=0;IMyCubeGrid Ġ=null;void Ιϋ(){Ġ=Me.CubeGrid;υ.GetBlocksOfType(ğ,q=>q.CubeGrid.IsSameConstructAs(Ġ)&&q.CustomName.Contains(dockPoolTag));υ.GetBlocksOfType(Ć,q=>q.CubeGrid.IsSameConstructAs(Ġ)&&q.CustomName.Contains(zoneTag));
υ.GetBlocksOfType(ΑΥ,q=>q.CubeGrid.IsSameConstructAs(Ġ)&&q.CustomName.Contains(laneTag));for(int i=0;i<Ć.Count;i++){Ć[i].DetectSmallShips=true;Ć[i].DetectLargeShips=true;}for(int i=0;i<ΑΥ.Count;i++) ΑΥ[i].EnableRaycast=true;}IMyShipConnector Ζυ(long Υ){if(Υ==0) return null;
IMyShipConnector c=υ.GetBlockWithId(Υ) as IMyShipConnector;return c!=null&&Ġ!=null&&c.CubeGrid.IsSameConstructAs(Ġ)?c:null;}bool Γψ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Ζί(IMyShipConnector c,long Δ){for(int i=0;i<ΑΥ.Count;i++)
{IMyCameraBlock Į=ΑΥ[i];if(!Į.IsWorking||Vector3D.Distance(Į.GetPosition(),c.GetPosition())>10) continue;Vector3D ȯ=Į.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Į.CanScan(ȯ)) continue;MyDetectedEntityInfo Ǎ=Į.Raycast(ȯ);if(!Ǎ.IsEmpty()&&Ǎ.EntityId!=Ġ.EntityId&&Ǎ.EntityId!=Δ&&Ǎ.Type!=MyDetectedEntityType.FloatingObject)
{Œ="Lane blocked: "+c.CustomName;return false;}}return true;}bool Ιγ(){for(int s=0;s<Ć.Count;s++){if(!Ć[s].IsWorking) continue;ΓΡ.Clear();Ć[s].DetectedEntities(ΓΡ);for(int i=0;i<ΓΡ.Count;i++){MyDetectedEntityInfo e=ΓΡ[i];bool Κτ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Κτ&&e.EntityId!=ǝ&&e.Velocity.Length()>0.5f){Œ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Ιξ(Ĉ r){IMyShipConnector ǎ=Ζυ(r.ǎ);if(Γψ(ǎ)&&Ζί(ǎ,r.Δ)) return ǎ.EntityId;if(!r.ΖЏ||ǎ==null) return 0;IMyShipConnector Ъ=null;double ǈ=0;for(int i=0;i<ğ.Count;i++)
{IMyShipConnector c=ğ[i];if(!Γψ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ǎ.GetPosition());if(Ъ==null||d<ǈ){if(Ζί(c,r.Δ)){Ъ=c;ǈ=d;}}}return Ъ==null?0:Ъ.EntityId;}void ΙΤ(){Ĉ Ƙ=null;long Р=0;for(int ş=2;ş>=1&&Ƙ==null;ş--){Ĉ Ş=null;for(int i=0;i<Ц.Count;i++)
{Ĉ r=Ц[i];if(r.ǡ!=ş) continue;if(ş==1){long c=Ιξ(r);if(c==0) continue;if(Ş==null||r.ǟ<Ş.ǟ){Ş=r;Р=c;}}else if(Ş==null||r.ǟ<Ş.ǟ) Ş=r;}Ƙ=Ş;}if(Ƙ==null){if(Ц.Count>0&&Œ=="") Œ="No free dock";return;}ǝ=Ƙ.Δ;Δι=Ƙ.ǡ;ΗΑ=Ƙ.ǡ==1?Р:0;ΗΒ=DateTime.Now;
}void ΙЏ(){if(Ġ==null) return;if(Βη==null) Βη=IGC.RegisterBroadcastListener(Ʊ);int ȋ=30;while(Βη.HasPendingMessage&&ȋ-->0){String Θ=Βη.AcceptMessage().Data as String;if(Θ!=null) ΙΥ(Θ);}Ισ();if(++ΗΪ<3) return;ΗΪ=0;IGC.SendBroadcastMessage(Ʊ,Θω());}void ΙΥ(String Θ){try{String[]f=Θ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Ц.Count-1;i>=0;i--) if(Ц[i].Δ==Δ) Ц.RemoveAt(i);if(ǝ==Δ) ǝ=0;}else if(f[0]=="R"&&f.Length>=5){long ǎ=long.Parse(f[3]);if(Ζυ(ǎ)==null) return;Ĉ r=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==Δ) r=Ц[i];if(r==null){r=new Ĉ();r.Δ=Δ;r.ǟ=DateTime.Now;Ц.Add(r);}
int ǡ=int.Parse(f[2]);if(r.ǡ!=ǡ) r.ǟ=DateTime.Now;r.ǡ=ǡ;r.ǎ=ǎ;r.ΖЏ=f[4]=="1";r.ΑΦ=DateTime.Now;}}catch{}}void Ισ(){for(int i=Ц.Count-1;i>=0;i--)
if((DateTime.Now-Ц[i].ΑΦ).TotalSeconds>3) Ц.RemoveAt(i);Ĉ ΒΩ=null;for(int i=0;i<Ц.Count;i++) if(Ц[i].Δ==ǝ) ΒΩ=Ц[i];if(ǝ!=0&&ΒΩ==null) ǝ=0;if(ΒΩ!=null&&(DateTime.Now-ΗΒ).TotalSeconds>Θϋ){ǝ=0;ΒΩ.ǟ=DateTime.Now;}Œ="";if(ǝ==0&&Ц.Count>0&&!Ιγ()) ΙΤ();
}String Θω(){String β="S;"+Ġ.EntityId+";"+ǝ+";"+Δι+";"+ΗΑ+";";for(int i=0;i<Ц.Count;i++) β+=(i>0?",":"")+Ц[i].Δ;β+=";";for(int i=0;i<ğ.Count;i++){IMyShipConnector c=ğ[i];β+=(i>0?":":"")+c.EntityId+","+τ(c.GetPosition())+","+τ(c.WorldMatrix.Forward)+","+τ(c.WorldMatrix.Up);
}β+=";";for(int i=0;i<Ć.Count;i++){IMySensorBlock z=Ć[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();β+=(i>0?":":"")+τ(z.GetPosition())+","+Math.Round(r,1);}return β;}String ΙΘ()
{if(ğ.Count==0&&Ć.Count==0) return "";int ΑΛ=0;for(int i=0;i<ğ.Count;i++) if(Γψ(ğ[i])) ΑΛ++;return "Docks: "+ΑΛ+"/"+ğ.Count+" free"+(ǝ!=0?(Δι==1?" | docking":" | undocking"):"")+
(Ц.Count>1?" | "+(Ц.Count-1)+" waiting":"")+(Œ!=""?"\n"+Œ:"")+"\n";}IMyBroadcastListener ΓΕ=null;int Ȯ=0;DateTime ΗΩ=DateTime.Now;DateTime ΘΤ=DateTime.MinValue;long ΘΥ=0;int ΘΦ=0;long Ƣ=0;bool Ασ=false;int Δτ=0;List<Vector3>Αυ=new List<Vector3>();List<float>ΕЁ=new List<float>();
bool Βθ=false;bool Βζ=false;Vector3 ΗΨ=new Vector3();bool ƽ(){return Ȯ!=0&&(DateTime.Now-ΘΤ).TotalSeconds<3;}bool č(int ǡ){return ƽ()&&ΘΥ==ǔ&&ΘΦ==ǡ;}bool Ζφ(){return ƽ()||(DateTime.Now-ΗΩ).TotalSeconds<3;}void ΓА(int ǡ){if(Ȯ==ǡ) return;Ȯ=ǡ;ΗΩ=DateTime.Now;Δτ=3;
}void ΓЏ(){if(Ȯ!=0&&trafficEnabled) IGC.SendBroadcastMessage(Ʊ,"D;"+ǔ);Ȯ=0;Ασ=false;}void ΙЎ(){if(!trafficEnabled||Μ==Λ.ή) return;if(ΓΕ==null) ΓΕ=IGC.RegisterBroadcastListener(Ʊ);int ȋ=20;while(ΓΕ.HasPendingMessage&&ȋ-->0){String Θ=ΓΕ.AcceptMessage().Data as String;if(Θ==null||!Θ.StartsWith("S;")) continue;
try{Ιλ(Θ);}catch{}}if(Ϋ!=Α.Й){Βθ=false;Βζ=false;}if(Ȯ==1&&и(MyShipConnectorStatus.Connected)!=null) ΓЏ();if(Ȯ==2&&!Ασ&&Ϋ!=Α.Ǳ&&
Vector3.Distance(δ,э().Ε)>λ*3) ΓЏ();if(Ȯ!=0&&++Δτ>=3){Δτ=0;IGC.SendBroadcastMessage(Ʊ,"R;"+ǔ+";"+Ȯ+";"+Ǐ+";"+(ı?1:0));}}void Ιλ(String Θ){String[]f=Θ.Split(';');if(f.Length<7) return;bool Ηγ=false;String[]ΖϏ=f[5].Split(',');for(int i=0;i<ΖϏ.Length;i++) if(ΖϏ[i]==ǔ.ToString()) Ηγ=true;
if(!Ηγ) return;ΘΤ=DateTime.Now;ΘΥ=long.Parse(f[2]);ΘΦ=int.Parse(f[3]);Ƣ=long.Parse(f[4]);Αυ.Clear();ΕЁ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Αυ.Add(ρ(e[0]));ΕЁ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ηά=f[6].Split(':');List<Ǜ>Ў=new List<Ǜ>();for(int i=0;i<Ηά.Length;i++){String[]e=Ηά[i].Split(',');if(e.Length<4) continue;Ǜ p=new Ǜ();p.Υ=long.Parse(e[0]);p.γ=ρ(e[1]);p.Э=ρ(e[2]);p.Ї=ρ(e[3]);Ў.Add(p);}Х=Ў;}bool ΘЂ(){if(!trafficEnabled||Μ==Λ.ή||Ǐ==0) return true;
ΓА(1);if(!Ζφ()) return true;if(!č(1)){Κ="Waiting for dock clearance";return false;}ΓΦ();return true;}bool ΓΦ(){ΑΓ=false;Ǜ Š=null;if(Ƣ!=0&&Ƣ!=Ǐ&&ı)
for(int i=0;i<Х.Count;i++) if(Х[i].Υ==Ƣ) Š=Х[i];long Ƭ=Š==null?0:Š.Υ;if(Ƭ==ā&&(Ƭ==0)==(ǣ==null)) return false;ā=Ƭ;ǣ=Š==null?null:Γγ(Š.γ,Š.Э,Š.Ї);return true;}bool ŵ(Vector3 p){for(int i=0;i<Αυ.Count;i++) if(Vector3.Distance(p,Αυ[i])<ΕЁ[i]+λ*0.75f) return true;
return false;}bool ΙΩ(Vector3 ǌ,Vector3 ŝ){Βθ=false;if(!trafficEnabled||Μ==Λ.ή||Ǐ==0) return false;if(Ȯ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;ΓА(1);if(!ƽ()||č(1)||Αυ.Count==0) return false;if(!ŵ(ǌ)&&!ŵ(ŝ)&&!ŵ(δ)) return false;Βθ=true;return true;
}bool ΕЌ(){if(!trafficEnabled||Μ==Λ.ή||Ǐ==0) return true;ΓА(2);if(!Ζφ()) return true;if(!č(2)){Κ="Waiting for undock clearance";return false;}return true;}const float Ιδ=600f;class Ċ{public long Υ;public int Ы=-1;public bool Ƭ;public float ΑΜ,ΑΘ;public DateTime Εί;}class ȣ{public String ѓ;
public int w,h;public float Λρ,Λβ;public String Ѐ;public List<int>ΕΡ=new List<int>();public List<Ċ>у=new List<Ċ>();}List<ȣ>ȓ=new List<ȣ>();IMyBroadcastListener Βω=null;int Ηϊ=0;void ΚΒ(){if(Βω==null) Βω=IGC.RegisterBroadcastListener(ΔΝ);int ȋ=50;while(Βω.HasPendingMessage&&ȋ-->0)
{String Θ=Βω.AcceptMessage().Data as String;if(Θ==null) continue;try{ΙΧ(Θ);}catch{}try{ΙΨ(Θ);}catch{}}for(int j=ȓ.Count-1;j>=0;j--){ȣ Φ=ȓ[j];for(int i=Φ.у.Count-1;i>=0;i--)
if((DateTime.Now-Φ.у[i].Εί).TotalSeconds>Ιδ) Φ.у.RemoveAt(i);if(Φ.у.Count==0){ȓ.RemoveAt(j);continue;}Θυ(Φ);}if(++Ηϊ<3) return;Ηϊ=0;for(int j=0;j<ȓ.Count;j++) IGC.SendBroadcastMessage(Ζΰ,Θψ(ȓ[j]));}void Θυ(ȣ Φ){for(int i=0;i<Φ.у.Count;i++){Ċ s=Φ.у[i];if(s.Ƭ&&(s.Ы<0||Φ.Ѐ[s.Ы]=='1')) s.Ы=Ιη(Φ,s);
}}String Θψ(ȣ Φ){String β="J;"+Φ.ѓ+";"+Φ.Ѐ+";";bool ǟ=true;for(int i=0;i<Φ.у.Count;i++){if((DateTime.Now-Φ.у[i].Εί).TotalSeconds>5) continue;β+=(ǟ?"":",")+Φ.у[i].Υ+":"+Φ.у[i].Ы;ǟ=false;}return β;}void ΙΧ(String Θ){String[]f=Θ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȣ Φ=null;for(int i=0;i<ȓ.Count;i++) if(ȓ[i].ѓ==f[4]) Φ=ȓ[i];if(Φ==null){Φ=new ȣ();Φ.ѓ=f[4];Φ.w=int.Parse(k[3]);Φ.h=int.Parse(k[4]);Φ.Λρ=float.Parse(k[6]);Φ.Λβ=float.Parse(k[7]);if(Φ.w<1||Φ.h<1) return;ΕЅ(Φ.w,Φ.h,(ѐ) int.Parse(k[5]),Φ.ΕΡ);
Φ.Ѐ=new String('0',Φ.w*Φ.h);ȓ.Add(Φ);}String d=f[6];if(d.Length==Φ.Ѐ.Length){char[]c=Φ.Ѐ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Φ.Ѐ=new String(c);}long Υ=long.Parse(f[0]);Ċ s=null;for(int i=0;i<Φ.у.Count;i++) if(Φ.у[i].Υ==Υ) s=Φ.у[i];
if(s==null){s=new Ċ();s.Υ=Υ;Φ.у.Add(s);}s.Εί=DateTime.Now;String[]w=f[10].Split(',');s.Ƭ=w[0]=="1";if(w.Length>=3){s.ΑΜ=float.Parse(w[1]);s.ΑΘ=float.Parse(w[2]);}int Ĕ=int.Parse(f[5]);if(Ĕ>=0&&Ĕ<Φ.Ѐ.Length&&(s.Ы<0||Φ.Ѐ[s.Ы]=='1')&&Φ.Ѐ[Ĕ]!='1'&&ΖΦ(Φ,Ĕ,s)==null)
s.Ы=Ĕ;}Ċ ΖΦ(ȣ Φ,int Ы,Ċ Κξ){for(int i=0;i<Φ.у.Count;i++){Ċ o=Φ.у[i];if(o!=Κξ&&o.Ы==Ы)
return o;}return null;}int Ιη(ȣ Φ,Ċ s){for(int k=0;k<Φ.ΕΡ.Count;k++){int c=Φ.ΕΡ[k];if(Φ.Ѐ[c]!='1'&&ΖΦ(Φ,c,s)==null) return c;}return-2;}String ΙΝ(){String s="";for(int j=0;j<ȓ.Count;j++){ȣ Φ=ȓ[j];int Ѐ=0;for(int i=0;i<Φ.Ѐ.Length;i++) if(Φ.Ѐ[i]=='1') Ѐ++;s+="Job "+(j+1)+": "+Ѐ+"/"+Φ.Ѐ.Length+" holes, "+Φ.у.Count+" ships\n";
}return s;}const float ΙЄ=8f;class ѝ{public long Υ;public Vector3 γ,м,ē;public float Βε,ΑΜ,ΑΘ,Ȕ=-1;public int ǹ;public String Φ="";public int š;public DateTime ľ;}class ΒΛ{public long a,b,Θσ;public DateTime ΑΦ;}List<ѝ>З=new List<ѝ>();List<ΒΛ>Ă=new List<ΒΛ>();int Θδ=0;void ΙΨ(String Θ)
{String[]f=Θ.Split(';');if(f.Length<9) return;long Υ=long.Parse(f[0]);ѝ s=null;for(int i=0;i<З.Count;i++) if(З[i].Υ==Υ) s=З[i];if(s==null){s=new ѝ();s.Υ=Υ;З.Add(s);}s.γ=ρ(f[1]);s.м=ρ(f[2]);s.Βε=float.Parse(f[3]);s.Φ=f[4];s.š=int.Parse(f[7].Split(',')[1]);s.ē=ρ(f[8]);
s.ǹ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ΑΜ=float.Parse(w[1]);s.ΑΘ=float.Parse(w[2]);s.ǹ=int.Parse(w[3]);}}s.ľ=DateTime.Now;}bool ΔΜ(ѝ s){return s.м.LengthSquared()<0.25f&&(s.š==3||s.ē.LengthSquared()<0.25f);}Vector3 Ζϋ(ѝ s){return s.м.LengthSquared()>=1f?s.м:s.ē*2f;}
void ΚΛ(){ΘЈ();if(++Θδ<3) return;Θδ=0;IGC.SendBroadcastMessage(Ζϊ,Θϊ());}void ΘЈ(){for(int i=З.Count-1;i>=0;i--)
if((DateTime.Now-З[i].ľ).TotalSeconds>3) З.RemoveAt(i);for(int i=Ă.Count-1;i>=0;i--)
if((DateTime.Now-Ă[i].ΑΦ).TotalSeconds>2) Ă.RemoveAt(i);for(int i=0;i<З.Count;i++) З[i].Ȕ=-1;for(int i=0;i<З.Count;i++)
for(int j=i+1;j<З.Count;j++){ѝ a=З[i],b=З[j];if(ΔΜ(a)||ΔΜ(b)) continue;Vector3 ΛΒ=a.γ+a.м*(float)(DateTime.Now-a.ľ).TotalSeconds;Vector3 ΛΖ=b.γ+b.м*(float)(DateTime.Now-b.ľ).TotalSeconds;Vector3 Ƨ=Ζϋ(a),ƨ=Ζϋ(b);Vector3 ǥ=ΛΖ-ΛΒ;float σ=ǥ.Length();float б=(a.Βε+b.Βε)*0.55f;
bool Αβ=a.Φ!=""&&a.Φ==b.Φ;if(Αβ&&(a.ǹ==2||b.ǹ==2)) continue;if(Αβ&&a.ǹ==1&&b.ǹ==1){String[]k=a.Φ.Split('|');if(k.Length>=8) б=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 Εέ=ƨ-Ƨ;float ΘΚ=Εέ.LengthSquared();float t=ΘΚ>0.01f?-Vector3.Dot(ǥ,Εέ)/ΘΚ:0;
bool Ες=σ<б;if(!Ες&&(t<=0||t>ΙЄ||(ǥ+Εέ*t).Length()>=б)) continue;if(!Ες&&Ƨ.LengthSquared()>0.25f&&ƨ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ƨ),Vector3.Normalize(ƨ))<-0.7f)
continue;ΒΛ p=null;for(int k=0;k<Ă.Count;k++)
if((Ă[k].a==a.Υ&&Ă[k].b==b.Υ)||(Ă[k].a==b.Υ&&Ă[k].b==a.Υ)) p=Ă[k];if(p==null){p=new ΒΛ();p.a=a.Υ;p.b=b.Υ;p.Θσ=ΙЅ(a,b,Ƨ,ƨ,ǥ,σ);Ă.Add(p);}p.ΑΦ=DateTime.Now;ѝ y=p.Θσ==a.Υ?a:b;float ΗЀ=Ες||t<4?0:Math.Max(1,y.м.Length()*0.5f);if(y.Ȕ<0||ΗЀ<y.Ȕ) y.Ȕ=ΗЀ;
}}long ΙЅ(ѝ a,ѝ b,Vector3 Ƨ,Vector3 ƨ,Vector3 ǥ,float σ){if(a.Υ==ǝ) return b.Υ;if(b.Υ==ǝ) return a.Υ;Vector3 n=σ>0.01f?ǥ/σ:new Vector3(1,0,0);float ΘΨ=Vector3.Dot(Ƨ,n),Θγ=Vector3.Dot(ƨ,-n);if(ΘΨ-Θγ>1) return a.Υ;if(Θγ-ΘΨ>1) return b.Υ;return a.Υ>b.Υ?a.Υ:b.Υ;
}String Θϊ(){String β="C;";bool ǟ=true;for(int i=0;i<З.Count;i++){if(З[i].Ȕ<0) continue;β+=(ǟ?"":",")+З[i].Υ+":"+Math.Round(З[i].Ȕ,1);ǟ=false;}return β;}String ΙΣ(){int ΗЎ=0,ΑΠ=0;for(int i=0;i<З.Count;i++){if(!ΔΜ(З[i])) ΗЎ++;if(З[i].Ȕ>=0) ΑΠ++;}if(З.Count==0) return "";
return "Traffic: "+ΗЎ+" moving"+(ΑΠ>0?", "+ΑΠ+" giving way":"")+"\n";}List<ϋ>ȱ=new List<ϋ>();ϋ Ƿ=null;Vector3 Ηρ;DateTime Ηπ;float Ηο=40;bool Ηξ=false;ѝ ΖΕ(ϋ s){ѝ Ъ=null;for(int i=0;i<З.Count;i++)
if(Vector3.Distance(З[i].γ,s.Ε)<15&&(Ъ==null||
Vector3.Distance(З[i].γ,s.Ε)<Vector3.Distance(Ъ.γ,s.Ε))) Ъ=З[i];return Ъ;}float ΙΪ(ϋ s){ѝ t=ΖΕ(s);if(t==null) return 0;float k=t.ǹ*1000000f;String[]ѓ=t.Φ.Split('|');if(ѓ.Length>=3) k+=Vector3.Distance(t.γ,new Vector3(float.Parse(ѓ[0]),float.Parse(ѓ[1]),float.Parse(ѓ[2])));return k;
}void ΙЂ(){ȱ.Clear();Ƿ=null;List<float>ΔЋ=new List<float>();for(int i=0;i<Н.Count;i++){float k=ΙΪ(Н[i]);int ǅ=0;while(ǅ<ΔЋ.Count&&ΔЋ[ǅ]<=k) ǅ++;ΔЋ.Insert(ǅ,k);ȱ.Insert(ǅ,Н[i]);}}void ΚΗ(){if(Ƿ!=null){bool Κσ=Ηξ||Vector3.Distance(Ƿ.Ε,Ηρ)>Ηο||
(DateTime.Now-Ηπ).TotalSeconds>45||Ƿ.Γϋ();if(!Κσ) return;Ƿ=null;}if(ȱ.Count==0) return;Ƿ=ȱ[0];ȱ.RemoveAt(0);Ηρ=Ƿ.Ε;Ηπ=DateTime.Now;ѝ t=ΖΕ(Ƿ);Ηο=t==null?40:Math.Max(40,t.Βε*3);Ηξ=t!=null&&t.š==3&&t.м.LengthSquared()<0.25f;Ƿ.ĩ("HOMEPOS",true,false,false);ΒΖ("HOMEPOS",Ƿ.Υ,"HOMEPOS");
}