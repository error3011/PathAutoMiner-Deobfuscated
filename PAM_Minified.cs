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
public enum Ν{ȯ,ų,ǒ,ц,ΰ}public enum Β{м,Ȑ,Ķ,Ȕ,Ē,Ǒ,б,
ȭ,Ĳ,ȩ,ū,Ũ,č,
Βη,ȗ,Ȯ,ı,Βχ,ΑЏ,
Βφ,Ǌ,Ŭ,Ʋ,Ȱ}public enum Ξ{н,ķ,В,ΑЍ,ƭ,ƺ,ƻ,Ĭ,ĭ}public enum х{None,ı,м,ΔΡ}public enum Ρ{Main,Δφ,Ų,ű,Βς,ƫ,
Αώ,Δδ,Δε,Δγ,Ŷ,Δό,
ǃ,Βκ,Βν,Δύ,ǅ}public enum г{ǂ,Ū,ΖБ,Δθ,ΰ}public enum ǔ{Ǉ,ΔΛ}public enum к{ǁ,ΑБ,Δώ,Ů}public enum Ѕ{ΒΪ,ΔΞ,ΑЉ,ΑЎ,Ư,ΒΛ,ƶ}public enum є{ΑЋ,ΑЃ,ΑЄ}public enum О{ĵ,Ζω,Ď}public enum η{Βά,Βΰ,Βα,Δρ,Δπ,
ΒΫ,ƾ,ƽ,Βή,
Βί,Βέ}public enum ί{ǻ,ȓ,Ǻ,ƪ,ΑЁ,ΑЂ}public enum Ȗ{ΒΑ,ΑЀ,Ζζ}public enum ч{ǆ,ΔΤ,ΖЁ,ΒΨ}public enum И{None,Ŵ,Βζ,ΒΩ}public enum σ{Ź,Ʃ,Ƹ,Ů,ΒΦ,ƹ,ΒΧ,ΗΔ,Ββ}public enum Й{ȯ,Δξ,Δν,ΒΣ,ΒΝ,ΑЊ,Δο}class ά{public bool ε=false;public Vector3 Ε=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 ϋ=new Vector3();public Vector3 π=new Vector3();public Vector3 ш=new Vector3();public float Н=0;public float ΘЄ=0;public float[]ǥ=null;public ά(){}public ά(ά Ч){ε=Ч.ε;Ε=Ч.Ε;β=Ч.β;γ=Ч.γ;ϋ=Ч.ϋ;π=Ч.π;
ш=Ч.ш;ǥ=Ч.ǥ;}public ά(Vector3 Ε,Vector3 γ,Vector3 β,Vector3 ϋ,Vector3 π){this.Ε=Ε;this.β=β;this.γ=γ;this.ϋ=ϋ;this.Н=0;this.π=π;}public void ΖΦ(List<IMyThrust>ΑΑ,List<string>ȋ){ǥ=new float[ȋ.Count];for(int i=0;i<ǥ.Length;i++)
ǥ[i]=-1;for(int i=0;i<ΑΑ.Count;i++){string ѓ=Δα(ΑΑ[i]);int Α=ȋ.IndexOf(ѓ);if(Α!=-1)
ǥ[Α]=Ј(ΑΑ[i].MaxEffectiveThrust,ΑΑ[i].MaxThrust);}}}class Βγ{public ά λ=null;public List<Vector3>Š=new List<Vector3>();public float Ƃ=0;public float ΓЏ=0;public float Ŀ=0;public float ΓΖ=0;public Vector3 ΑΠ=new Vector3();}class Ļ{public η З=η.Βά;public float ǘ=0;public float ā=0;
public string Ǚ="";public string ǜ="";DateTime Εκ;public bool Γσ=false;private bool ΒЎ=false;public bool ΑЇ(bool Ǣ){if(Ǣ){ā=0;ΒЎ=false;return false;}ΒЎ=true;return ā>ǘ;}public void ΖЅ(){ΑЇ(true);Γσ=false;}public void ΗΠ(){if(ΒЎ)
if((DateTime.Now-Εκ).TotalSeconds>1){ā++;Εκ=DateTime.Now;}}public bool ΒΙ(){switch(З){case η.Βΰ:return true;case η.Βα:return true;}return false;}}class Ь{public String έ="";public String ο="";public int ώ=0;public О Ŏ=О.Ď;public Ь(String έ,String ο,int ώ,О Ŏ){this.έ=έ;this.ο=ο;
this.ώ=ώ;this.Ŏ=Ŏ;}}class Ζγ{public Ζγ(Vector3 Є,float Ϗ){this.Є=Є;this.Ϗ=Ϗ;}public Vector3 Є;public float Ϗ;}class Г{public DateTime ΓΨ;public DateTime ΕΔ;public String Р="";public String έ="";public String Αό="";public String Λ="";public String ΓϏ="";public String Αη="";public String Γς="";
public Vector3 Ε=new Vector3();public List<Ь>Ѐ=new List<Ь>();public Ν Π=Ν.ȯ;public И Ȳ;public float ǲ;public float Ǭ;public int Ё;public int ΓΒ=0;public int ΓЋ=0;public bool Δμ(){return(DateTime.Now-ΓΨ).TotalSeconds>10;}public bool ΒΞ(){if(Ȳ!=И.Ŵ) return false;return(DateTime.Now-ΕΔ).TotalSeconds>=4;
}public Г(String Р){this.Р=Р;}public И ĸ(String Ѝ,bool ΘΪ,bool ȅ,bool Ηα){if(Ѝ==""&&!ȅ) return И.None;if(Ȳ==И.Ŵ&&ΒΞ())
Ȳ=И.ΒΩ;if(ΘΪ){Γς=Ѝ;Ȳ=И.Ŵ;ΕΔ=DateTime.Now;return Ȳ;}else if(ȅ){Γς="";Ȳ=И.None;}else if(Γς==Ѝ){if(Ηα) Ȳ=И.Βζ;return Ȳ;}return И.None;}}public String Ζν(String ΗІ){switch(ΗІ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem ϊ;Vector3 Αν=new Vector3();Vector3 ζ=new Vector3();Vector3 ΑΤ=new Vector3();Vector3 Ɛ=new Vector3();Vector3 Ƅ=new Vector3();
DateTime ΓΫ=new DateTime();bool Ț=true;int ΓЅ=0;bool Ȟ=false;bool Αά=false;bool ƕ=true;bool ΖΡ=false;bool ŋ=false;bool ΑΩ=false;float ȥ=0;float Ħ=0;int Ċ=0;int Ƥ=0;int ƙ=0;int ΒЇ=0;float Ő=0;float ŀ=0;double Γη=0;float Ž=0;List<int>ŉ=new List<int>();List<int>Ř=new List<int>();
Ν Π=Ν.ȯ;int І=0;float ȍ=0;float ǹ=0;float κ=0;IMyRemoteControl Ψ;IMySensorBlock ρ;List<IMyTimerBlock>ΓЈ=new List<IMyTimerBlock>();List<IMyShipConnector>щ=new List<IMyShipConnector>();List<IMyThrust>Ȍ=new List<IMyThrust>();List<IMyGyro>ΑΥ=new List<IMyGyro>();List<IMyTerminalBlock>ς=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȋ=new List<IMyLandingGear>();List<IMyReactor>ĉ=new List<IMyReactor>();List<IMyConveyorSorter>Ας=new List<IMyConveyorSorter>();List<IMyGasTank>ј=new List<IMyGasTank>();List<IMyTerminalBlock>ź=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ń=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ę=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȱ=new List<IMyBatteryBlock>();List<IMyTextPanel>М=new List<IMyTextPanel>();List<IMyTextSurface>Ƕ=new List<IMyTextSurface>();List<IMyTextPanel>ě=new List<IMyTextPanel>();IMyTerminalBlock ћ=null;ά Ο=new ά();ά ΓΠ=new ά();
ά ΓΤ=new ά();bool Ȏ=false;bool ţ=false;bool Ť=false;bool ΑΫ=false;List<String>ȋ=new List<string>();List<ά>Φ=new List<ά>();List<ά>ƨ=new List<ά>();int ΓΓ=0;double ΖΣ=0;ά Ζ=new ά();Vector3 ǟ;Vector3 Ȣ;Ξ α=Ξ.н;ǔ Ə=ǔ.Ǉ;int Њ=0;int Л=0;double Ǝ=0;bool ΑΝ=false;Ļ ό=new Ļ();
Ļ ύ=new Ļ();к т=к.ǁ;bool ċ=true;ǔ Ǵ=ǔ.Ǉ;int р=3;int п=3;int Э=30;є ъ=є.ΑЋ;bool Ȩ=true;bool Ŋ=true;bool ń=true;bool ħ=false;Ѕ Х=Ѕ.ΒΪ;bool ȧ=true;float Ǳ=90;float э=20;float Ю=5;float ю=20;float ǩ=0.70f;float ŧ=1.50f;float Ŧ=2.50f;float ī=0f;float ă=0f;Β Ϊ;
Ξ ь;Β Ɲ;Βγ ι=null;int ψ=0;int φ=0;int Ъ=0;int Ƞ=0;int ĥ=30;int њ=0;bool Б=true;Vector3 я;double Ф=0;bool Αο=false;int ΔΒ=0;int Ќ=0;int Βώ=0;int Ĉ=0;int ΕЀ=0;Vector3 Ɨ=new Vector3();float с=0;float Я=0;int Ī=0;ά Ασ=null;float ΓΩ=0;float ΑΔ=0;float Ы=0;float ġ=0;
bool ΓΡ=false;bool Ĩ=false;bool ƥ=false;bool Ȝ=false;bool Ș=false;DateTime Ης=new DateTime();ά ę=null;ά ΕΚ=null;float Ȃ=0;х Ǫ;float ȝ=0;float ĩ=0;String Γα="";float Αή=0;float Ǭ=0;float ǲ=0;List<Ь>Ǜ=new List<Ь>();float Ц=0;bool ƛ=false;bool ΖΗ=false;bool ΕЊ=false;
bool Αυ=false;bool ΕЋ=false;float Γε=0;float Γρ=0;Vector3 ΕΪ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 Αφ=new Vector3(1,1,1);float ΓЄ=1;Vector3 ơ=new Vector3();bool ΑΪ=false;bool ΓΜ=false;bool Γή=false;bool ΔІ=false;float ΑΛ=0;float ǌ=2;Vector3 ΖΖ;Vector3 ΓЃ;Vector3 ΓЂ;
float[,]Ǧ=new float[3,2];Dictionary<String,float[,]>џ=new Dictionary<string,float[,]>();Dictionary<String,float>ΖΟ=new Dictionary<String,float>();int ΒЂ=0;Dictionary<String,float[]>ȣ=new Dictionary<string,float[]>();float Γτ;bool ΑΞ=false;int ΑΓ=0;int ą=0;int ΑΧ=0;int ĝ=0;
String Λ="";int[]Εψ=new int[Enum.GetValues(typeof(Ρ)).Length];bool Γι=false;int Ģ=0;int Ё=0;Ρ τ=Ρ.Main;int Θθ=0,Θι=0,Θκ=0,Θλ=0;int ΘΥ=0,ΘΦ=0;bool ł=false;bool ΔЇ=false;String Ş="";String ΒІ="";IMyBroadcastListener Ǖ=null;bool ΕΩ=true;List<Г>У=null;Г Ћ=null;
int ΑΗ=0,ΑΘ=0;int ΕΗ=0,ΕΘ=0;int Ηη=0,Ηθ=0,Ηι=0;String ΗЇ="";Exception ȟ=null;bool Αξ=false;Ξ Ś=Ξ.н;bool ΕΝ=false;bool Εθ=true;bool Γψ=false;String ΖΠ="";bool ƣ=false;bool Γύ=true;int ΑΨ=0;int[]ѝ=null;Vector3 ΒЈ=new Vector3();bool Εά=false;bool ѕ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;
Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΚ(Me,controllerTag,true))
Π=Ν.ц;δ(Ρ.Main);if(Π!=Ν.ц){Λ="Welcome to [PAM]!";ч Γχ=ΖІ();if(Π==Ν.ȯ){List<IMyShipDrill>ΕΦ=new List<IMyShipDrill>();List<IMyShipGrinder>Εέ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(ΕΦ,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(Εέ,q=>q.CubeGrid==Me.CubeGrid);if(ΕΦ.Count>0)
{Π=Ν.ų;Λ="Miner mode enabled!";}else if(Εέ.Count>0){Π=Ν.ǒ;Λ="Grinder mode enabled!";}else{Π=Ν.ΰ;ǿ(г.ΰ);}}if(Γχ==ч.ǆ) ƕ=false;if(Γχ==ч.ΔΤ) Λ="Data restore failed!";if(Γχ==ч.ΒΨ) Λ="New version, wipe data";}}void Main(string К,UpdateType Θσ){try{if(ȟ!=null){if(К.Trim().ToUpper()=="RETRY")
{ȟ=null;Ț=true;Ċ=0;if(Π!=Ν.ц) χ();Λ="Restarted after error";}else{Ζφ();Δτ();return;}}Αά=(Θσ&UpdateType.Update10)!=0;if(Αά) ΓЅ++;Ȟ=ΓЅ>=10;if(Ȟ) ΓЅ=0;if(Αά){ΑΓ++;if(ΑΓ>4) ΑΓ=0;ƙ=(int) Math.Max(0,10-(DateTime.Now-ΓΫ).TotalSeconds);}if(К!="") ΗЇ=К;if(Π!=Ν.ц)
ΖА(К);else
Ζα(К);Γι=false;try{int ΕΛ=Runtime.CurrentInstructionCount;float Ĥ=Ј(ΕΛ,Runtime.MaxInstructionCount);if(Ĥ>0.90) Λ="Max. instructions >90%";if(Ĥ>Ő) Ő=Ĥ;if(ΑΞ){ŉ.Add(ΕΛ);while(ŉ.Count>10) ŉ.RemoveAt(0);ŀ=0;for(int i=0;i<ŉ.Count;i++) ŀ+=ŉ[i];ŀ=Ј(Ј(ŀ,ŉ.Count),Runtime.MaxInstructionCount);
double ΓΪ=Runtime.LastRunTimeMs;if(ΑΩ&&ΓΪ>Γη) Γη=ΓΪ;Ř.Add((int)(ΓΪ*1000f));while(Ř.Count>10) Ř.RemoveAt(0);Ž=0;for(int i=0;i<Ř.Count;i++) Ž+=Ř[i];Ž=Ј(Ž,Ř.Count)/1000f;}}catch{Ő=0;}}catch(Exception e){ȟ=e;}}void ΖА(string К){bool Απ=false;String ΔБ="";if(І<=1&&!ΒΟ(К))
Δυ(К);int Γκ=50;while(Ǖ!=null&&Ǖ.HasPendingMessage&&Γκ-->0){MyIGCMessage ν=Ǖ.AcceptMessage();String Χ=ν.Data as String;if(Χ==null) continue;String ǳ="";if(І<=1&&Δσ(ref Χ,out ǳ,out ΔБ)&&ǳ==BROADCAST_SELF_ID){Δυ(Χ);Απ=true;break;}}bool Εγ=ΑΩ&&Ϊ==Β.м&&!ŋ&&!Απ&&Ċ==0&&!ΑΫ;
if(Ȟ&&Ϊ!=Β.м) Απ=true;if((Αά&&!Εγ)||(Ȟ&&Εγ)){if(Ċ==0&&(ƙ<=0||Ț)){ΖΡ=І>0;І=0;Ċ=1;Ȁ();ΖЉ();Δζ();Ǽ("Scan 1");}else if(Ċ==1){Ċ=2;Ȁ();ΖЊ();Ǽ("Scan 2");}else if(Ċ==2){Ċ=0;Ȁ();ΖЋ();ΖЌ();Ǽ("Scan 3");ΓΫ=DateTime.Now;if(І<=1&&ƕ)
Αν=ΒΓ(Ψ,Ψ.CenterOfMass);ƕ=false;if(Ț){Ț=false;bool Θχ=α==Ξ.В;χ();if(Εθ&&resumeAfterRestart&&Θχ&&І<=1) Γψ=true;}Εθ=false;if(ΖΡ&&І==0) Λ="Setup complete";}else{if(α==Ξ.В&&Π!=Ν.ΰ){Ȁ();ΖΤ();Ǽ("Inv balance");}Ȁ();switch(Ƥ){case 0:ΗΗ();break;case 1:ΗΛ();break;case 2:ΗΖ();break;
case 3:ΗΤ();break;case 4:ΗΚ();break;case 5:ΖΩ();break;case 6:ΖΨ(Ψ);break;}Ǽ("Update: "+Ƥ);Ƥ++;if(Ƥ>6){Ƥ=0;ΑΩ=true;if(Ś!=Ξ.н){switch(Ś){case Ξ.Ĭ:ȫ();break;case Ξ.ĭ:ȫ();break;case Ξ.В:ȫ();break;case Ξ.ƺ:Ƴ();break;case Ξ.ƻ:Đ();break;}Ś=Ξ.н;}}}if(!Ț){if(!Ǿ(Ψ,true)){Ψ=null;
ƕ=true;І=2;}if(І>=2&&Ϊ!=Β.м)
χ();if(І<=1){Ħ=Ψ.CalculateShipMass().PhysicalMass;ȥ=(float) Ψ.GetShipSpeed();ζ=ΗΕ(Ψ,Αν);ΑΤ=Ψ.WorldMatrix.Forward;Ɛ=Ψ.WorldMatrix.Left;Ƅ=Ψ.WorldMatrix.Down;ΗΞ();ΗΡ();if(Γψ){Γψ=false;ȫ();if(Ϊ!=Β.м) Λ="Job resumed after restart";}if(Ϊ!=Β.м){ŋ=false;Δψ(false);ΔΙ(false);String Θί=ΒΕ(Ϊ)+" "+(int) Ϊ;
Ȁ();ΗΜ();Βυ(false);Ǽ(Θί);ΗΣ();ΗΝ();Ȁ();ΗΙ();Ǽ("Thruster");Ȁ();ΔЃ();Ǽ("Gyroscope");}else{if(ŋ){if(Ʒ()){а(Ƅ,ΑΤ,Ɛ,0.25f,true);ΔЃ();Λ="Aligning to planet: "+Math.Round(ΑΛ-0.25f,2)+"°";if(ΑΪ) ΒΘ(true,true);}else ΒΘ(true,true);}}ƣ=false;}}}Ȁ();Δτ();Ǽ("Print");if(Απ||ΒЇ<=0){Ȁ();ΖΥ(ΔБ);
Ǽ("Broadcast");ΒЇ=4;}else if(Ȟ) ΒЇ--;}void Δυ(string К){if(К=="") return;var ξ=К.ToUpper().Split(' ');ξ.DefaultIfEmpty("");var Ѝ=ξ.ElementAtOrDefault(0);var ȁ=ξ.ElementAtOrDefault(1);var ż=ξ.ElementAtOrDefault(2);var ΔЊ=ξ.ElementAtOrDefault(3);String ȉ="Invalid argument: "+К;bool ΓЍ=false;
switch(Ѝ){case "UP":this.ΒΥ(false);break;case "DOWN":this.ΒΤ(false);break;case "UPLOOP":this.ΒΥ(true);break;case "DOWNLOOP":this.ΒΤ(true);break;case "APPLY":this.ΔΗ(true);break;case "MRES":Ё=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.Βρ();}break;case "PATH":{this.χ();this.Βρ();Ο.ε=true;}break;
case "START":{this.χ();Βπ();}break;case "ALIGN":{ΒΘ(!ŋ,false);}break;case "CONT":{this.χ();this.ȫ();}break;case "JOBPOS":{this.χ();this.Đ();}break;case "HOMEPOS":{this.χ();this.Ƴ();}break;case "FULL":{ƛ=true;}break;case "RESET":{Αξ=true;І=2;}break;case "EXPORT":{Ζη();}break;
case "IMPORT":{Ζχ(ȁ=="JOB");}break;default:ΓЍ=true;break;}if(Π!=Ν.ΰ){switch(Ѝ){case "SHUTTLE":{Ζδ();}break;case "CFGS":{if(!Ζί(ȁ,ż,ΔЊ)) Λ=ȉ;}break;case "CFGB":{if(!Ζή(ȁ,ż)) Λ=ȉ;}break;case "CFGL":{if(!đ(ref Ǳ,true,σ.Ƹ,ȁ,"")||!Ζΰ(ż))
Λ=ȉ;}break;case "CFGE":{if(!đ(ref Ю,true,σ.ƹ,ȁ,"IG")||
!đ(ref э,true,σ.ΒΦ,ż,"IG")||
!đ(ref ю,true,σ.ΒΧ,ΔЊ,"IG"))
Λ=ȉ;}break;case "CFGA":{if(!đ(ref ǩ,false,σ.Ʃ,ȁ,"")) Λ=ȉ;}break;case "CFGW":{if(!đ(ref ŧ,false,σ.Ź,ȁ,"")||
!đ(ref Ŧ,false,σ.Ź,ż,""))
Λ=ȉ;}break;case "NEXT":{Αύ(false);}break;case "PREV":{Αύ(true);}break;default:if(ΓЍ) Λ=ȉ;break;}}else{switch(Ѝ){case "UNDOCK":{ƣ=true;}break;default:if(ΓЍ) Λ=ȉ;break;}}}String Ζξ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Π!=Ν.ΰ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ζδ(){χ();Π=Ν.ΰ;ǿ(г.ΰ);Ζ.ε=false;Ο.ε=false;ρ=null;ς.Clear();α=Ξ.н;}void ΒΘ(bool Ў,bool ǭ){if(!Ў) Λ="Aligning canceled";if(ǭ) Λ="Aligning done";if(ǭ||!Ў){ŋ=false;ΔϏ();Ǆ(false,0,0,0,0);return;}if(Ʒ()) ŋ=true;}bool Ζί(String Θω,String Ηώ,String Ην)
{bool Θυ=α==Ξ.В;int w,h,d;if(int.TryParse(Θω,out w)&&int.TryParse(Ηώ,out h)&&int.TryParse(Ην,out d)){this.χ();р=w;п=h;Э=d;Ė(false);Ÿ(false,false);if(Θυ) ȫ();return true;}return false;}bool Ζΰ(String Ż){if(Ż=="ON"){Ȩ=true;return true;}if(Ż=="OFF"){Ȩ=false;return true;}
return false;}bool Ζή(String ΕΥ,String ΑΜ){bool ģ=true;if(ΕΥ=="HOME") ċ=true;else if(ΕΥ=="STOP") ċ=false;else ģ=false;if(ΑΜ=="HOME") т=к.ǁ;else if(ΑΜ=="STOP") т=к.Δώ;else if(ΑΜ=="JOB") т=к.ΑБ;else if(ΑΜ=="IG") т=к.Ů;else ģ=false;return ģ;}void δ(Ρ д){Εψ[(int) τ]=Ё;
Ё=Εψ[(int) д];if(д==Ρ.Ŷ) Ё=0;τ=д;if(Π!=Ν.ц)
ĕ(τ==Ρ.Ų,false,0,0);Γι=true;}void ΒΥ(bool Γί){if(Ё>0) Ё--;else if(Γί) Ё=Ģ-1;}void ΒΤ(bool Γί){if(Ё<Ģ-1) Ё++;else if(Γί) Ё=0;}String ǿ(г ν){switch(ν){case г.ǂ:Λ="Job is running";break;case г.Ū:Λ="Connector not ready!";break;case г.ΖБ:Λ="Ship modified, path outdated!";break;
case г.Δθ:Λ="Interrupted by player!";break;case г.ΰ:Λ="Shuttle mode enabled!";break;}return "";}String Ζυ(ǔ Є){switch(Є){case ǔ.Ǉ:return "Top-Left";case ǔ.ΔΛ:return "Center";default:return "";}}String Ζλ(є Ђ){switch(Ђ){case є.ΑЃ:return "Auto"+(Π==Ν.ų?" (Ore)":"");case є.ΑЄ:return "Auto (+Stone)";
case є.ΑЋ:return "Default";default:return "";}}String ΒΒ(Ξ ω){switch(ω){case Ξ.н:return "No job";case Ξ.ķ:return "Job paused";case Ξ.В:return "Job active";case Ξ.Ĭ:return "Job active";case Ξ.ĭ:return "Job active";case Ξ.ΑЍ:return "Job done";case Ξ.ƭ:return "Job changed";case Ξ.ƺ:return "Move home";
case Ξ.ƻ:return "Move to job";}return "";}String ΔΩ(к Ηί){switch(Ηί){case к.ǁ:return "Return home";case к.ΑБ:return "Fly to job pos";case к.Δώ:return "Stop";case к.Ů:return "Ignore";}return "";}String Ζμ(Ѕ Ђ){switch(Ђ){case Ѕ.ΒΪ:return "Off";case Ѕ.ΑЎ:return "Drop pos (Stone) ";case Ѕ.Ư:return "Drop pos (Sto.+Ice)";
case Ѕ.ΔΞ:return "Cur. pos (Stone)";case Ѕ.ΑЉ:return "Cur. pos (Sto.+Ice)";case Ѕ.ΒΛ:return "In motion (Stone)";case Ѕ.ƶ:return "In motion (Sto.+Ice)";}return "";}String ΔΧ(х ω){switch(ω){case х.None:return "No batteries";case х.ı:return "Charging";case х.ΔΡ:return "Discharging";}return "";}String ΒΕ(Β ω)
{String ΘЂ=Π==Ν.ΰ?"target":"job";switch(ω){case Β.м:return "Idle";case Β.Ȑ:return "Flying to XY position";case Β.Ķ:return Π==Ν.ǒ?"Grinding":"Mining";case Β.Ȕ:return "Returning";case Β.Ŭ:return "Flying to drop pos";case Β.Ē:return "Returning to dock";case Β.Ǒ:return "Flying to dock area";
case Β.б:return "Flying to job area";case Β.ȭ:return "Flying to path";case Β.Ĳ:return "Flying to job position";case Β.ȩ:return "Approaching dock";case Β.ū:return "Docking";case Β.Ũ:return "Aligning";case Β.č:return "Aligning";case Β.Βη:return "Retry docking";case Β.ȗ:return "Unloading";case Β.Ȱ:return ΖΠ;
case Β.Ȯ:return "Undocking";case Β.ı:return "Charging batteries";case Β.Βχ:return "Waiting for uranium";case Β.ΑЏ:return "Filling up hydrogen";case Β.Βφ:return "Waiting for ejection";case Β.Ǌ:return "Waiting for ejection";case Β.Ʋ:return "Flying to drop pos";}return "";}String Δβ(η З){switch(З)
{case η.Βά:return "On \"Undock\" command";case η.Βΰ:return "On player entered cockpit";case η.Δρ:return "On ship is full";case η.Δπ:return "On ship is empty";case η.Βα:return "On time delay";case η.ƾ:return "On batteries empty(<25%)";case η.ƽ:return "On batteries empty(=0%)";case η.ΒΫ:return "On batteries full";
case η.Βί:return "On hydrogen empty(<25%)";case η.Βέ:return "On hydrogen empty(=0%)";case η.Βή:return "On hydrogen full";}return "";}bool Ι(ref String Γ,int ΕЇ,int Ą,bool Θ,String ğ){Ģ+=1;if(ΕЇ==Ą) ğ=">"+ğ+(ΑΓ>=2?" .":"");else ğ=" "+ğ;Γ+=ğ+"\n";return ΕЇ==Ą&&Θ;}String ΔΗ(bool Θ)
{int Α=0;int Η=Ё;Ģ=0;String ŝ="———————————————\n";String š="--------------------------------------------\n";String Γ="";Γ+=ΒΒ(α)+" | "+(Ο.ε?"Ready to dock":"No dock")+"\n";Γ+=ŝ;double ǵ=Math.Max(Math.Round(this.Ф),0);if(τ==Ρ.Main){bool Ć=Π==Ν.ΰ;if(Ι(ref Γ,Η,Α++,Θ," Record path & set home")) Βρ();
if(Π==Ν.ų)
if(Ι(ref Γ,Η,Α++,Θ," Setup mining job")) δ(Ρ.Ų);if(Π==Ν.ǒ)
if(Ι(ref Γ,Η,Α++,Θ," Setup grinding job")) δ(Ρ.Ų);if(Π==Ν.ΰ)
if(Ι(ref Γ,Η,Α++,Θ," Setup shuttle job")) δ(Ρ.Βν);if(Ι(ref Γ,Η,Α++,Θ," Continue job")) ȫ();if(Ι(ref Γ,Η,Α++,Θ," Fly to home position")) Ƴ();if(Ι(ref Γ,Η,Α++,Θ," Fly to job position")) Đ();if(Ι(ref Γ,Η,Α++,Θ," Behavior settings"))
if(Ć) δ(Ρ.ǅ);else δ(Ρ.ƫ);if(Ι(ref Γ,Η,Α++,Θ," Info")) δ(Ρ.Δδ);if(Π!=Ν.ΰ)
if(Ι(ref Γ,Η,Α++,Θ," Help")) δ(Ρ.Δγ);}else if(τ==Ρ.Ų){double Θψ=Math.Round(р*ȍ,1);double Ηύ=Math.Round(п*ǹ,1);String Ї="";if(Ι(ref Ї,Η,Α++,Θ," Start new job!")) Βπ();if(Ι(ref Ї,Η,Α++,Θ," Change current job")){Ÿ(false,false);δ(Ρ.Main);}if(Ι(ref Ї,Η,Α++,Θ," Width + (Width: "+р+" = "+Θψ+"m)")){Į(ref р,5,20,1);Ė(true);}
if(Ι(ref Ї,Η,Α++,Θ," Width -")){Į(ref р,-5,20,-1);Ė(true);}if(Ι(ref Ї,Η,Α++,Θ," Height + (Height: "+п+" = "+Ηύ+"m)")){Į(ref п,5,20,1);Ė(true);}if(Ι(ref Ї,Η,Α++,Θ," Height -")){Į(ref п,-5,20,-1);Ė(true);}if(Ι(ref Ї,Η,Α++,Θ," Depth + ("+(ъ==є.ΑЋ?"Depth":"Min")+": "+Э+"m)")){Į(ref Э,5,50,2);Ė(true);}
if(Ι(ref Ї,Η,Α++,Θ," Depth -")){Į(ref Э,-5,50,-2);Ė(true);}if(Ι(ref Ї,Η,Α++,Θ," Depth mode: "+Ζλ(ъ))){ъ=Ȭ(ъ);}if(Ι(ref Ї,Η,Α++,Θ," Start pos: "+Ζυ(Ǵ))){Ǵ=Ȭ(Ǵ);}if(Π==Ν.ǒ&&ъ==є.ΑЄ) ъ=Ȭ(ъ);Γ+=Βθ(8,Ї,Η,ref ΘΥ);}else if(τ==Ρ.Βν){float[]ΕΟ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ι(ref Γ,Η,Α++,Θ," Next")) δ(Ρ.Δύ);if(Ι(ref Γ,Η,Α++,Θ," Back")) δ(Ρ.Main);Γ+=" Leave connector 1:\n";if(Ι(ref Γ,Η,Α++,Θ," - "+Δβ(ό.З))) ό.З=Ȭ(ό.З);if(!ό.ΒΙ()) Γ+="\n";else if(Ι(ref Γ,Η,Α++,Θ," - Delay: "+ƴ((int) ό.ǘ))) ό.ǘ=ΔΟ(ό.ǘ,ΕΟ);Γ+=" Leave connector 2:\n";if(Ι(ref Γ,Η,Α++,Θ," - "+Δβ(ύ.З))) ύ.З=Ȭ(ύ.З);
if(!ύ.ΒΙ()) Γ+="\n";else if(Ι(ref Γ,Η,Α++,Θ," - Delay: "+ƴ((int) ύ.ǘ))) ύ.ǘ=ΔΟ(ύ.ǘ,ΕΟ);}else if(τ==Ρ.Δύ){if(Ι(ref Γ,Η,Α++,Θ," Start job!")) Βπ();if(Ι(ref Γ,Η,Α++,Θ," Back")) δ(Ρ.Βν);Γ+=" Timer: \"Docking connector 1\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(ό.Ǚ!=""?ό.Ǚ:"-"))) ό.Ǚ=Ƽ(ref Θθ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(ό.ǜ!=""?ό.ǜ:"-"))) ό.ǜ=Ƽ(ref Θκ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(ύ.Ǚ!=""?ύ.Ǚ:"-"))) ύ.Ǚ=Ƽ(ref Θι);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(ύ.ǜ!=""?ύ.ǜ:"-"))) ύ.ǜ=Ƽ(ref Θλ);
}else if(τ==Ρ.ű){String Εο=Ǳ+" %";if(Ȟ) ą++;if(ą>1){ą=0;ΑΧ++;if(ΑΧ>1) ΑΧ=0;}bool[]Γϊ=new bool[]{ĉ.Count==0,Ǫ==х.None,ј.Count==0};int ΖΛ=0;while(true){ΖΛ++;ĝ++;if(ĝ>Γϊ.Length-1) ĝ=0;if(ΖΛ>=Γϊ.Length) break;if(!Γϊ[ĝ]) break;}bool Ć=Π==Ν.ΰ;if(!Ć&&Ȩ&&Ц!=-1&&ΑΧ==0)
Εο=Ц<1000000?Math.Round(Ц)+" Kg":Math.Round(Ц/1000)+" t";if(Ι(ref Γ,Η,Α++,Θ," Stop!")){χ();δ(Ρ.Main);}if(Ι(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!Ć) δ(Ρ.ƫ);else δ(Ρ.ǅ);if(!Ć){if(Ι(ref Γ,Η,Α++,Θ," Next hole")) Αύ(false);}else if(Ι(ref Γ,Η,Α++,Θ," Undock")) ƣ=true;Γ+=š;if(!Ć) Γ+="Progress: "+Math.Round(Ǝ,1)+" %\n";Γ+="State: "+ΒΕ(Ϊ)+" "+ǵ+"m \n";Γ+="Load: "+Αή+" % Max: "+Εο+" \n";if(ĝ==0) Γ+="Uranium: "+(ĉ.Count==0?"No reactors":Math.Round(ĩ,1)+"Kg "+(Ю==-1?"":" Min: "+Ю+" Kg"))+"\n";
if(ĝ==1) Γ+="Battery: "+(Ǫ==х.None?ΔΧ(Ǫ):Ȃ+"% "+(э==-1||Ć?"":" Min: "+э+" %"))+"\n";if(ĝ==2) Γ+="Hydrogen: "+(ј.Count==0?"No tanks":Math.Round(ȝ,1)+"% "+(ю==-1||Ć?"":" Min: "+ю+" %"))+"\n";}else if(τ==Ρ.ƫ){String Ї="";if(Ι(ref Ї,Η,Α++,Θ," Back")){if(α==Ξ.В) δ(Ρ.ű);else δ(Ρ.Main);}
if(Ι(ref Ї,Η,Α++,Θ," Max load: "+Ǳ+"%")) й(ref Ǳ,Ǳ<=80?-10:-5,σ.Ƹ,false);if(Ι(ref Ї,Η,Α++,Θ," Weight limit: "+(Ȩ?"On":"Off"))) Ȩ=!Ȩ;if(Ι(ref Ї,Η,Α++,Θ," Ejection: "+Ζμ(Х))) Х=Ȭ(Х);if(Ι(ref Ї,Η,Α++,Θ," Toggle sorters: "+(ħ?"On":"Off"))){ħ=!ħ;if(ħ) Ǩ(Γύ);}
if(Ι(ref Ї,Η,Α++,Θ," Unload ice: "+(ȧ?"On":"Off"))) ȧ=!ȧ;if(Ι(ref Ї,Η,Α++,Θ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) й(ref Ю,(Ю>5?-5:-1),σ.ƹ,true);if(Ι(ref Ї,Η,Α++,Θ," Battery: "+(э==-1?"Ignore":"Min "+э+"%"))) й(ref э,-5,σ.ΒΦ,true);if(Ι(ref Ї,Η,Α++,Θ," Hydrogen: "+(ю==-1?"Ignore":"Min "+ю+"%"))) й(ref ю,-10,σ.ΒΧ,true);
if(Ι(ref Ї,Η,Α++,Θ," When done: "+(ċ?"Return home":"Stop"))) ċ=!ċ;if(Ι(ref Ї,Η,Α++,Θ," On damage: "+ΔΩ(т))) т=Ȭ(т);if(Ι(ref Ї,Η,Α++,Θ," Advanced...")) δ(Ρ.Αώ);Γ+=Βθ(8,Ї,Η,ref ΘΦ);}else if(τ==Ρ.Αώ){if(Ι(ref Γ,Η,Α++,Θ," Back")){if(α==Ξ.В) δ(Ρ.ű);else δ(Ρ.Main);}
if(Ι(ref Γ,Η,Α++,Θ,(Π==Ν.ǒ?" Grinder":" Drill")+" inv. balancing: "+(Ŋ?"On":"Off"))) Ŋ=!Ŋ;if(Ι(ref Γ,Η,Α++,Θ," Enable"+(Π==Ν.ǒ?" grinders":" drills")+": "+(ń?"Fwd + Bwd":"Fwd"))) ń=!ń;if(Ι(ref Γ,Η,Α++,Θ," Work speed fwd.: "+ŧ+"m/s")) й(ref ŧ,0.5f,σ.Ź,false);
if(Ι(ref Γ,Η,Α++,Θ," Work speed bwd.: "+Ŧ+"m/s")) й(ref Ŧ,0.5f,σ.Ź,false);if(Ι(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǩ*100f)+"%"+(ǩ>0.80f?" (risky)":""))) й(ref ǩ,0.1f,σ.Ʃ,false);if(Ι(ref Γ,Η,Α++,Θ," Width overlap: "+ī*100f+"%")) ΔΜ(true,0.05f);if(Ι(ref Γ,Η,Α++,Θ," Height overlap: "+ă*100f+"%")) ΔΜ(false,0.05f);
}else if(τ==Ρ.ǅ){if(Ι(ref Γ,Η,Α++,Θ," Back")){if(α==Ξ.В) δ(Ρ.ű);else δ(Ρ.Main);}if(Ι(ref Γ,Η,Α++,Θ," Max load: "+Ǳ+"%")) й(ref Ǳ,Ǳ<=80?-10:-5,σ.Ƹ,false);if(Ι(ref Γ,Η,Α++,Θ," Unload ice: "+(ȧ?"On":"Off"))) ȧ=!ȧ;if(Ι(ref Γ,Η,Α++,Θ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) й(ref Ю,(Ю>5?-5:-1),σ.ƹ,true);
if(Ι(ref Γ,Η,Α++,Θ," Battery: "+(э==-1?"Ignore":"Charge up"))) э=(э==-1?1:-1);if(Ι(ref Γ,Η,Α++,Θ," Hydrogen: "+(ю==-1?"Ignore":"Fill up"))) ю=(ю==-1?1:-1);if(Ι(ref Γ,Η,Α++,Θ," On damage: "+ΔΩ(т))) т=Ȭ(т);if(Ι(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(ǩ*100f)+"%"+(ǩ>0.80f?" (risky)":""))) й(ref ǩ,0.1f,σ.Ʃ,false);
}else if(τ==Ρ.Δφ){double ΗЈ=0;if(Φ.Count>0) ΗЈ=Vector3.Distance(Φ.Last().Ε,ζ);if(Ι(ref Γ,Η,Α++,Θ," Stop path recording")) ΔЀ();if(Π!=Ν.ΰ){if(Ι(ref Γ,Η,Α++,Θ," Home: "+(Ȏ?"Use old home":(Ο.ε?"Was set! ":"none ")))) Ȏ=!Ȏ;}else{if(Ι(ref Γ,Η,Α++,Θ," Connector 1: "+(Ȏ?"Use old connector":(Ο.ε?"Was set! ":"none ")))) Ȏ=!Ȏ;
if(Ι(ref Γ,Η,Α++,Θ," Connector 2: "+(ţ?"Use old connector":(Ζ.ε?"Was set! ":"none ")))) ţ=!ţ;}if(Ι(ref Γ,Η,Α++,Θ," Path: "+(Ť?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) Ť=!Ť;Γ+=š;Γ+="Wp spacing: "+Math.Round(ΖΣ)+"m\n";}else if(τ==Ρ.Βς){if(Ι(ref Γ,Η,Α++,Θ," Stop")){χ();δ(Ρ.Main);}
Γ+=š;Γ+="State: "+ΒΕ(Ϊ)+" \n";Γ+="Speed: "+Math.Round(ȥ,1)+"m/s\n";Γ+="Target dist: "+ǵ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+ΔΒ+"\n";}else if(τ==Ρ.Δδ){List<IMyTerminalBlock>Ě=ΔΪ();if(Ȟ) ą++;if(ą>=Ě.Count) ą=0;if(Ι(ref Γ,Η,Α++,Θ," Next")) δ(Ρ.Δε);
Γ+=š;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αή,1)+"% "+Math.Round(Ǭ,1)+" / "+Math.Round(ǲ,1)+"\n";Γ+="Uranium: "+(ĉ.Count==0?"No reactors":Math.Round(ĩ,1)+"Kg "+Γα)+"\n";Γ+="Battery: "+(Ǫ==х.None?"":Ȃ+"% ")+ΔΧ(Ǫ)+"\n";Γ+="Hydrogen: "+(ј.Count==0?"No tanks":Math.Round(ȝ,1)+"% ")+"\n";
Γ+="Damage: "+(Ě.Count==0?"None":""+(ą+1)+"/"+Ě.Count+" "+Ě[ą].CustomName)+"\n";}else if(τ==Ρ.Δε){if(Ι(ref Γ,Η,Α++,Θ," Back")) δ(Ρ.Main);Γ+=š;Γ+="Next scan: "+ƙ+"s\n";Γ+="Ship size: "+Math.Round(ȍ,1)+"m "+Math.Round(ǹ,1)+"m "+Math.Round(κ,1)+"m \n";
Γ+="Broadcast: "+(ł?"Online - "+Ş:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ő*100f,1)+"% \n";}else if(τ==Ρ.Δγ){if(Ι(ref Γ,Η,Α++,Θ," Back")) δ(Ρ.Main);Γ+=š;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(І==2) Γ="Fatal setup error\nNext scan: "+ƙ+"s\n";if(Αξ) Γ="Recompile script now";int ΗЉ=Γ.Split('\n').Length;for(int i=ΗЉ;i<=10;i++) Γ+="\n";Γ+=ŝ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΔΜ(bool ΗЅ,float ў){χ();Ÿ(true,false);if(ΗЅ) й(ref ī,ў,σ.Ββ,false);else й(ref ă,ў,σ.Ββ,false);ΔΚ();ĕ(true,true,0,0);}float ΔΟ(float θ,float[]Αϋ){float Ϋ=Αϋ[0];for(int i=Αϋ.Length-1;i>=0;i--)
if(θ<Αϋ[i]) Ϋ=Αϋ[i];return Ϋ;}String Ƽ(ref int Ą){String Ϋ="";if(Ą>=ΓЈ.Count) Ą=-1;if(Ą>=0) Ϋ=ΓЈ[Ą].CustomName;Ą++;return Ϋ;}void ǈ(string έ){if(α!=Ξ.В) return;if(έ=="") return;IMyTerminalBlock Υ=ϊ.GetBlockWithName(έ);if(Υ==null||!(Υ is IMyTimerBlock)){Λ="Timerblock "+έ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void Į(ref int value,int ſ,int Ȧ,int Γϋ){if(ſ==0) return;if(value<Ȧ&&Γϋ>0||value<=Ȧ&&Γϋ<0){value+=Γϋ;return;}int ΗΦ=Math.Abs(ſ);int Βψ=0;int Γλ=1;while(true){Βψ+=Γλ*ΗΦ*10;if(ſ<0&&value-Ȧ<=Βψ) break;if(ſ>0&&value-Ȧ<Βψ) break;
Γλ++;}value+=Γλ*ſ;}void Ė(bool ΘЅ){р=Math.Max(р,1);п=Math.Max(п,1);Э=Math.Max(Э,0);ĕ(τ==Ρ.Ų,false,0,0);}bool đ(ref float value,bool ΘΡ,σ ο,String Ğ,String ΗЀ){if(Ğ=="") return false;float Ŕ=-1;bool Εα=false;if(Ğ.ToUpper()==ΗЀ) Εα=true;else if(!float.TryParse(Ğ,out Ŕ)) return false;
else Ŕ=Math.Max(0,Ŕ);if(ΘΡ) Ŕ=(float) Math.Round(Ŕ);й(ref value,Ŕ,ο,Εα,false);return true;}void й(ref float value,float ў,σ ο,bool Ƣ){й(ref value,value+ў,ο,Ƣ,true);}void й(ref float value,float ő,σ ο,bool Ƣ,bool Θύ){float Ǟ=0,ǝ=0;if(ο==σ.Ź){Ǟ=0.5f;ǝ=10f;}if(ο==σ.Ʃ){Ǟ=0.1f;ǝ=1f;}
if(ο==σ.Ů){Ǟ=50f;ǝ=100f;}if(ο==σ.ΒΦ){Ǟ=5f;ǝ=30f;}if(ο==σ.ƹ){Ǟ=1f;ǝ=25f;}if(ο==σ.Ƹ){Ǟ=10f;ǝ=95f;}if(ο==σ.ΒΧ){Ǟ=10f;ǝ=90f;}if(ο==σ.ΗΔ){Ǟ=10f;ǝ=1800;}if(ο==σ.Ββ){Ǟ=0.0f;ǝ=0.75f;}if(ő==-1&&Ƣ){value=-1;return;}if(value==-1) Ƣ=false;
bool ΕЁ=ő<Ǟ||ő>ǝ;if(ΕЁ&&Θύ){if(ő<value) value=ǝ;else if(ő>value) value=Ǟ;}else value=ő;if(ΕЁ&&Ƣ) value=-1;else value=Math.Max(Ǟ,Math.Min(value,ǝ));value=(float) Math.Round(value,2);}void Αύ(bool ΘΜ){if(ΘΜ) Ъ=Math.Max(0,Ъ-1);else Ъ++;Βυ(true);}T Ȭ<T>(T θ){int Α=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);
Α++;if(Α>=Ζε(θ)) Α=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Α);}int Ζε<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βρ(){ƨ.Clear();for(int i=0;i<Φ.Count;i++) ƨ.Add(Φ[i]);Φ.Clear();ΑΫ=true;ΓΠ=new ά(Ο);ΓΤ=new ά(Ζ);Ο.ε=false;if(Π==Ν.ΰ) Ζ.ε=false;for(int i=0;i<џ.Count;i++)
if(!ȋ.Contains(џ.Keys.ElementAt(i)))
ȋ.Add(џ.Keys.ElementAt(i));Ȏ=false;ţ=false;Ť=false;δ(Ρ.Δφ);}void ΔЀ(){if(Ȏ) Ο=ΓΠ;if(ţ) Ζ=ΓΤ;if(Ť){Φ.Clear();for(int i=0;i<ƨ.Count;i++) Φ.Add(ƨ[i]);}ΑΫ=false;χ();δ(Ρ.Main);}void ΗΞ(){if(!ΑΫ) return;if(Ϊ!=Β.м){ΔЀ();return;}if(!ΓΠ.ε) Ȏ=false;if(!ΓΤ.ε) ţ=false;if(ƨ.Count<=1) Ť=false;
IMyShipConnector Ǘ=л(MyShipConnectorStatus.Connectable);if(Ǘ==null) Ǘ=л(MyShipConnectorStatus.Connected);if(Ǘ!=null){if(Math.Round(ȥ,2)<=0.20) ΓΓ++;else ΓΓ=0;if(ΓΓ>=5){if(Π==Ν.ΰ&&(Ο.ε||Ȏ)&&Vector3.Distance(Ο.Ε,Ǘ.GetPosition())>5){Ζ.γ=Ψ.WorldMatrix.Forward;Ζ.ϋ=Ψ.WorldMatrix.Left;
Ζ.β=Ψ.WorldMatrix.Down;Ζ.π=Ψ.GetNaturalGravity();Ζ.Ε=Ǘ.GetPosition();Ζ.ε=true;Ζ.ш=Ǘ.Position;}else{Ο.γ=Ψ.WorldMatrix.Forward;Ο.ϋ=Ψ.WorldMatrix.Left;Ο.β=Ψ.WorldMatrix.Down;Ο.π=Ψ.GetNaturalGravity();Ο.Ε=Ǘ.GetPosition();Ο.ε=true;Ο.ш=Ǘ.Position;}}}double ΓΧ=-1;if(Φ.Count>0) ΓΧ=Vector3.Distance(ζ,Φ.Last().Ε);
double Θέ=Math.Max(1.5,Math.Pow(ȥ/100.0,2));double Εω=Math.Max(ȥ*Θέ,2);ΖΣ=Εω;if((ΓΧ==-1)||ΓΧ>=Εω){ά Ω=new ά(ζ,ΑΤ,Ƅ,Ɛ,Ψ.GetNaturalGravity());Ω.ΖΦ(Ȍ,ȋ);Φ.Add(Ω);}}ά ȑ(){if(Π!=Ν.ΰ) return Ο;return ΕΚ;}void Ĺ(ά ή,Ξ ω){ΕΚ=ή;if(α==Ξ.В) ь=ω;}bool ǽ(ά ή,float Ϗ,bool Θτ,out Vector3 Є)
{if(Θτ){Vector3I ΓΛ=new Vector3I((int) ή.ш.X,(int) ή.ш.Y,(int) ή.ш.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(ΓΛ);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){Є=new Vector3();return false;}Vector3 Ηζ=в(Ψ,Υ.FatBlock.GetPosition()-ζ);Vector3 Ηε=в(Ψ,Υ.FatBlock.WorldMatrix.Forward);Є=ή.Ε-Βτ(ή.γ,ή.β*-1,Ηζ)-Βτ(ή.γ,ή.β*-1,Ηε)*Ϗ;
return true;}else{Є=ή.Ε;return true;}}Vector3 Ȓ(int Ηγ,int Ш,bool ΘΟ){if(!ΘΟ&&Εά) return ΒЈ;float Ηδ=((Њ-1f)/2f)-Ηγ;float ΘΣ=((Л-1f)/2f)-Ш;ΒЈ=Ζ.Ε+Ζ.ϋ*Ηδ*ȍ+Ȣ*-1*ΘΣ*ǹ;Εά=true;return ΒЈ;}Vector3 ΒΔ(Vector3 Θα,float ΒА){return Θα+(ǟ*ΒА);}ί ΑЌ(){float П=-1;ί Ϋ=ί.ǻ;
if(Π!=Ν.ΰ){if(α!=Ξ.н){Vector3 А=Ĕ(ǟ,Ȣ*-1,ζ-Ζ.Ε);if(Math.Abs(А.X)<=(float)(Њ*ȍ)/2f&&Math.Abs(А.Y)<=(float)(Л*ǹ)/2f){if(А.Z<=-1&&А.Z>=-ĥ*2) return ί.ƪ;if(А.Z>-1&&А.Z<κ*2) return ί.ǻ;}if(Ȫ(Ζ.Ε,ref П)) Ϋ=ί.ǻ;}if(Ο.ε){if(Ȫ(Ο.Ε,ref П)) Ϋ=ί.Ǻ;for(int i=0;i<Φ.Count;i++)
if(Ȫ(Φ[i].Ε,ref П)) Ϋ=ί.ȓ;if(Vector3.Distance(ζ,Ο.Ε)<dockDist*κ) Ϋ=ί.Ǻ;if(л(MyShipConnectorStatus.Connectable)!=null||л(MyShipConnectorStatus.Connected)!=null)
Ϋ=ί.Ǻ;}}else{Vector3 Є=new Vector3();IMyShipConnector Ȇ=л(MyShipConnectorStatus.Connected);if(Ο.ε){if(Ȫ(Ο.Ε,ref П)) Ϋ=ί.Ǻ;if(ǽ(Ο,dockDist,true,out Є))
if(Ȫ(Є,ref П)) Ϋ=ί.Ǻ;if(Ȇ!=null&&Vector3.Distance(Ȇ.GetPosition(),Ο.Ε)<5)
return ί.ΑЁ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ο.Ε)>dockDist*κ&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*κ)
if(Ȫ(Φ[i].Ε,ref П)) Ϋ=ί.ȓ;if(Ζ.ε){if(Ȫ(Ζ.Ε,ref П)) Ϋ=ί.ǻ;if(ǽ(Ζ,dockDist,true,out Є))
if(Ȫ(Є,ref П)) Ϋ=ί.ǻ;if(Ȇ!=null&&Vector3.Distance(Ȇ.GetPosition(),Ζ.Ε)<5)
return ί.ΑЂ;}}return Ϋ;}bool Ȫ(Vector3 Є,ref float П){float Ϗ=Vector3.Distance(Є,ζ);if(Ϗ<П||П==-1){П=Ϗ;return true;}return false;}void Βπ(){if(І>0){Λ="Setup error! Can't start";return;}if(Π==Ν.ΰ){ȫ();return;}Ζ.Ε=ζ;Ζ.π=Ψ.GetNaturalGravity();Ζ.γ=ΑΤ;Ζ.β=Ƅ;Ζ.ϋ=Ɛ;
ǟ=ћ.WorldMatrix.Forward;Ȣ=Ζ.β;if(ǟ==Ψ.WorldMatrix.Down) Ȣ=Ψ.WorldMatrix.Backward;Ÿ(true,true);Τ(Β.Ȑ);Βο();}void Ÿ(bool ǚ,bool ΘΠ){if(α==Ξ.н&&!ǚ) return;bool ΗБ=ǚ||α==Ξ.ΑЍ||Њ!=р||Л!=п||Ə!=Ǵ;if(ΗБ){if(α!=Ξ.н){α=Ξ.ƭ;Ȓ(ψ,φ,ΘΠ);Λ="Job changed, lost progress";}Ə=Ǵ;
Њ=р;Л=п;φ=0;ψ=0;ы="";Ǡ=-1;ġ=0;Ƞ=0;Ы=0;Ъ=0;Βυ(true);}}void Βδ(){о(ζ,0);і(Ȍ,true);}void Βο(){ǿ(г.ǂ);Βδ();ŵ(Ȋ,false);Ǩ(Γύ);α=Ξ.В;ΔΙ(true);ь=α;δ(Ρ.ű);ΖЈ();ΑΝ=true;ΑΨ=0;for(int i=ź.Count-1;i>=0;i--)
if(ů(ź[i],false)) ΑΨ++;if(ΑΨ>0) Λ="Started with damage";}void χ(){if(α==Ξ.В){α=Ξ.ķ;Λ="Job paused";}Τ(Β.м);ь=α;Ǆ(false,0,0,0,0);Βσ();ΔΕ(new Vector3(),false);ΔϏ();Βλ(ChargeMode.Auto);Βμ(false);Δψ(true);ȕ(Β.м);ĕ(false,false,0,0);і(ς,false);і(щ,true);Ǩ(true);Αο=false;Ʈ();ΑΚ=false;
ΑΝ=false;ƛ=false;ƣ=false;if(τ!=Ρ.Main&&τ!=Ρ.ƫ&&τ!=Ρ.Αώ&&τ!=Ρ.ǅ)
δ(Ρ.Main);}void ȫ(){ί С=ΑЌ();if(Π==Ν.ΰ){if(!Ζ.ε||!Ο.ε) return;Βο();bool ΓΙ=Vector3.Distance(ζ,Ο.Ε)<Vector3.Distance(ζ,Ζ.Ε);if(Ś==Ξ.Ĭ) ΓΙ=true;if(Ś==Ξ.ĭ) ΓΙ=false;if(ΓΙ){Ĺ(Ο,Ξ.Ĭ);switch(С){case ί.ΑЁ:Τ(Β.Ȱ);break;case ί.ȓ:Τ(Β.б);break;case ί.Ǻ:Τ(Β.ȩ);break;default:Τ(Β.Ȯ);break;
}}else{Ĺ(Ζ,Ξ.ĭ);switch(С){case ί.ΑЂ:Τ(Β.Ȱ);break;case ί.ǻ:Τ(Β.ȩ);break;case ί.ȓ:Τ(Β.б);break;default:Τ(Β.Ȯ);break;}}}else{if(α!=Ξ.ķ&&α!=Ξ.ƭ) return;bool Θφ=α==Ξ.ƭ;Βο();bool ΓΞ=Ű(false)&&Ο.ε;switch(С){case ί.ǻ:Τ(ΓΞ?Β.ȭ:Β.Ȑ);break;case ί.ȓ:Τ(ΓΞ?Β.Ǒ:Β.б);break;case ί.Ǻ:Τ(ΓΞ?Β.ȩ:Β.ȗ);break;
case ί.ƪ:{if(Ƞ!=Ъ||Θφ) Τ(Β.Ȕ);else Τ(Β.Ķ);}break;default:break;}}}void Đ(){if(α==Ξ.н&&!Ο.ε) return;if(Π==Ν.ΰ&&(!Ζ.ε||!Ο.ε)) return;Λ="Move to job";ί С=ΑЌ();if(Π==Ν.ΰ){Ĺ(Ζ,Ξ.ĭ);switch(С){case ί.ǻ:Τ(Β.ȩ);break;case ί.ȓ:Τ(Β.б);break;case ί.ΑЂ:return;default:Τ(Β.Ȯ);break;
}ȕ(Β.Ȱ);}else{switch(С){case ί.ǻ:Τ(Β.б);break;case ί.ȓ:Τ(Β.б);break;case ί.Ǻ:Τ(Β.ȗ);break;case ί.ƪ:Τ(Β.Ȕ);break;default:break;}if(α==Ξ.н) ȕ(Β.б);else ȕ(Β.č);Αο=true;}Βδ();δ(Ρ.Βς);ŵ(Ȋ,false);ь=Ξ.ƻ;}void Ƴ(){if(!Ο.ε) return;Λ="Move home";ί С=ΑЌ();if(Π==Ν.ΰ){Ĺ(Ο,Ξ.Ĭ);switch(С)
{case ί.ȓ:Τ(Β.Ǒ);break;case ί.Ǻ:Τ(Β.ȩ);break;case ί.ΑЁ:return;default:Τ(Β.Ȯ);break;}ȕ(Β.Ȱ);}else{if(л(MyShipConnectorStatus.Connected)!=null) return;if(л(MyShipConnectorStatus.Connectable)!=null){Τ(Β.ū);ȕ(Β.ȗ);return;}switch(С){case ί.ǻ:Τ(Β.ȭ);break;case ί.ȓ:Τ(Β.Ǒ);break;case ί.Ǻ:Τ(Β.Ǒ);break;
case ί.ƪ:Τ(Β.Ē);break;default:break;}ȕ(Β.ȗ);}Βδ();δ(Ρ.Βς);ŵ(Ȋ,false);ь=Ξ.ƺ;}void Τ(Β Γν){if(Γν==Β.м) Ɲ=Β.м;if(Ɲ!=Β.м&&Ϊ==Ɲ&&Γν!=Ɲ){χ();return;}Б=true;Ϊ=Γν;}void ȕ(Β ω){this.Ɲ=ω;}Ȗ Ζρ(int ƌ,bool Ǣ){if(Ǣ){ѝ=null;ψ=0;φ=0;}if(Ǵ==ǔ.Ǉ){int Εό=ƌ+1;φ=(int) Math.Floor(Ј(ƌ,Њ));
if(φ%2==0) ψ=ƌ-(φ*Њ);else ψ=Њ-1-(ƌ-(φ*Њ));if(φ>=Л) return Ȗ.ΑЀ;else return Ȗ.ΒΑ;}else if(Ǵ==ǔ.ΔΛ){if(ѝ==null) ѝ=new int[]{0,-1,0,0};int Εί=(int) Math.Ceiling(Њ/2f);int Εή=(int) Math.Ceiling(Л/2f);int Ηϋ=(int) Math.Floor(Њ/2f);int Ηϊ=(int) Math.Floor(Л/2f);
int ƍ=0;while(ѝ[2]<Math.Pow(Math.Max(Њ,Л),2)){if(ƍ>200) return Ȗ.Ζζ;ƍ++;ѝ[2]++;if(-Εί<ψ&&ψ<=Ηϋ&&-Εή<φ&&φ<=Ηϊ){if(ѝ[3]==ƌ){this.ψ=ψ-1+Εί;this.φ=φ-1+Εή;return Ȗ.ΒΑ;}ѝ[3]++;}if(ψ==φ||(ψ<0&&ψ==-φ)||(ψ>0&&ψ==1-φ)){int Θδ=ѝ[0];ѝ[0]=-ѝ[1];ѝ[1]=Θδ;
}ψ+=ѝ[0];φ+=ѝ[1];}}return Ȗ.ΑЀ;}void ΗΜ(){if(Ϊ==Β.Ȑ){if(Б){Ќ=0;if(Ƞ!=Ъ) ġ=0;Ƞ=Ъ;}if(Ќ==0){int ΒЊ=ΖΫ(Ъ);if(ΒЊ<0){if(!ΑΚ){ΑΚ=true;ΕΙ=ζ;}о(ΕΙ,0);Λ="Waiting for a free row";return;}ΑΚ=false;bool Εη=ΒЊ!=Ъ;if(Εη){Ъ=ΒЊ;ġ=0;Ƞ=Ъ;}Ȗ Ϋ=Ζρ(Ъ,Б||Εη);if(Ϋ==Ȗ.ΑЀ)
{α=Ξ.ΑЍ;Λ="Job done";if(ċ&&Ο.ε){Τ(Β.ȭ);ȕ(Β.ȗ);ь=Ξ.ƺ;}else{Τ(Β.Ĳ);ȕ(Β.č);ь=Ξ.ƻ;}return;}if(Ϋ==Ȗ.ΒΑ){Ќ=1;і(ς,true);я=Ȓ(ψ,φ,true);о(я,10);а(Ζ.β,Ζ.γ,Ζ.ϋ,false);}}else{if(Ф<wpReachedDist){Τ(Β.Ķ);return;}}}if(Ϊ==Β.Ķ){if(Б){і(ς,true);Ǩ(false);я=Ȓ(ψ,φ,false);
о(ΒΔ(я,0),0);а(Ζ.β,Ζ.γ,Ζ.ϋ,false);Ќ=1;с=0;Я=0;ΑΔ=0;ΓΩ=-1;ĥ=Э;ΓΡ=true;}if(!ΑЈ()){Τ(Β.Ē);return;}if(Ű(true)){ΕЀ=ȏ("","ORE",О.Ď);if((Х==Ѕ.ΑЎ||Х==Ѕ.Ư||
Х==Ѕ.ΒΛ||Х==Ѕ.ƶ)&&Π!=Ν.ǒ)
Τ(Β.Ŭ);else if((Х==Ѕ.ΔΞ||Х==Ѕ.ΑЉ)&&Π!=Ν.ǒ)
Τ(Β.Βφ);else
Τ(Β.Ē);return;}Ы=Vector3.Distance(ζ,я);if(Ы>ġ){ġ=Ы;ΓΡ=false;}if(Π==Ν.ǒ&&ΔΫ()==MyDetectedEntityType.SmallGrid)
Я+=2;else Я-=2;Я=Math.Max(100,Math.Min(400,Я));if(Ќ>0&&Ќ<Я){if(Ы>с){if(Я>150) с=Ы;else с=(float) Math.Ceiling(Ы);Ќ=1;}else Ќ++;}else{if(Ќ>0){Λ="Ship stuck! Retrying";с=Ы;Ќ=0;ĕ(false,true,0,κ*sensorRange);}о(ΒΔ(я,Math.Max(0,с-κ)),ΒΗ(false));if(Ф<=wpReachedDist/2){Ќ=1;с=0;}
return;}ĕ(false,true,κ*sensorRange,0);Vector3 ΗΨ=я+ǟ*Ы;bool Εδ=false;if(Vector3.Distance(ΗΨ,ζ)>0.3f){Vector3 Ηκ=я+ǟ*(Ы+0.1f);о(Ηκ,4);Εδ=true;}else{float ǣ=ΒΗ(true);Vector3 ΕΞ=ΒΔ(я,Math.Max(Э+1,Ы+1));ǧ(true,false,false,ΕΞ,ΕΞ-я,ǣ,ǣ);}bool ΒБ=false;if(ъ==є.ΑЄ||ъ==є.ΑЃ)
{if(!Εδ){float Γξ=0;foreach(IMyTerminalBlock Ǹ in ς)
Γξ+=Ζβ(Ǹ,"","",ъ==є.ΑЃ?new string[]{"STONE"}:null);if(Γξ>ΓΩ||Ы<Э||ΓΡ){Βώ=0;ΑΔ=Ы;ĥ=(int)(Math.Max(ĥ,ΑΔ)+κ/2);}else{ΒБ=Ы-ΑΔ>2&&Βώ>=20;Βώ++;}ΓΩ=Γξ;}}else ΒБ=Ы>=ĥ;if(Ƞ!=Ъ){Τ(Β.Ȕ);Ы=0;return;}if(ΒБ){Ъ++;Τ(Β.Ȕ);Ы=0;return;}}if(Ϊ==Β.Ȕ||Ϊ==Β.Ē||Ϊ==Β.Ŭ)
{if(Б){я=Ȓ(ψ,φ,false);а(Ζ.β,Ζ.γ,Ζ.ϋ,false);і(ς,ń);Ǩ(false);с=Vector3.Distance(ζ,я);ĕ(false,true,0,κ*sensorRange);}о(я,ΒΗ(false));if(Vector3.Distance(ζ,я)>=с+5){і(ς,false);Ǩ(true);Λ="Can´t return!";}if(Ф<wpReachedDist){if(Ϊ==Β.Ȕ&&Αο) Τ(Β.Ĳ);if(Ϊ==Β.Ȕ) Τ(Β.Ȑ);
if(Ϊ==Β.Ŭ) Τ(Β.Ʋ);if(Ϊ==Β.Ē){if(Ο.ε) Τ(Β.ȭ);else{χ();Đ();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Β.Ǒ||Ϊ==Β.б){if(Ϊ==Β.б&&α==Ξ.В&&Π!=Ν.ΰ){if(!ΑЈ()||Ű(true)){Τ(Β.Ǒ);return;}}bool Γυ=false;bool ΓЎ=false;bool ΔЋ=false;float Ŀ=0;bool ΕϏ=false;ά Ω=null;
if(Б){if(Ϊ==Β.Ǒ||Π==Ν.ΰ){ά ή=ȑ();ι=new Βγ();ι.λ=ή;ι.Ƃ=followPathDock*κ;ι.ΓЏ=useDockDirectionDist*κ;ι.Ŀ=10;ι.Š.Add(ή.Ε);Vector3 ľ=new Vector3();if(ǽ(ή,dockDist*κ,true,out ľ))
ι.Š.Add(ľ);else ι.Ƃ*=1.5f;if(Π==Ν.ΰ){if(ή==Ο) ι.ΑΠ=Ζ.Ε;if(ή==Ζ) ι.ΑΠ=Ο.Ε;ι.ΓΖ=dockDist*κ*1.1f;}}else if(Ϊ==Β.б){ι=new Βγ();ι.λ=Ζ;ι.Ƃ=followPathJob*κ;ι.ΓЏ=useJobDirectionDist*κ;ι.Ŀ=10;ι.ΑΠ=Ο.Ε;ι.ΓΖ=dockDist*κ*1.1f;ι.Š.Add(Ζ.Ε);if(α==Ξ.н){if(!Ο.ε||Φ.Count==0){χ();return;}
float Ηπ=Vector3.Distance(Φ.First().Ε,Ο.Ε);float Ηρ=Vector3.Distance(Φ.Last().Ε,Ο.Ε);if(Ηπ<Ηρ) ι.λ=Φ.Last();else ι.λ=Φ.First();}}Ɨ=new Vector3();ΕϏ=!Ζϋ(ζ);і(ς,false);Ǩ(true);њ=-1;double ǫ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,ι.ΑΠ)<=ι.ΓΖ) continue;
double Ϗ=Vector3.Distance(Φ[i].Ε,ζ);if(ǫ==-1||Ϗ<ǫ){њ=i;ǫ=Ϗ;}}Ĉ=Ζς(ι.λ.Ε,њ);ę=null;}ΖΧ(Φ,Ĉ,ι.Š,ι.Ƃ,Б,ref Ќ);for(int i=0;i<ι.Š.Count;i++){float Ϗ=Vector3.Distance(ζ,ι.Š[i]);if(Ϗ<=ι.Ƃ) Γυ=true;if(Ϗ<=ι.ΓЏ) ΓЎ=true;}if(ΓЎ) Ŀ=ι.Ŀ;float ΘІ=ę!=null?ę.Н:ȥ;
float Θό=(float) Math.Max(ȥ*0.1f*κ,wpReachedDist);if((Ф<Θό)||Б){if(!Б) њ+=Ĉ;if(Ĉ==0||њ>Φ.Count-1||њ<0)
Γυ=true;else{ΔΒ=Ĉ>0?Φ.Count-1-њ:њ;Ω=Φ[њ];ę=Ω;if(њ>=1&&њ<Φ.Count-1)
Ɨ=Ω.Ε-Φ[њ-Ĉ].Ε;else ę=null;Ѓ=Ω.Ε+Ζπ(њ,Ĉ);ΔЋ=true;}}if(ΓЎ)
а(ι.λ.β,ι.λ.γ,ι.λ.ϋ,false);else if(ΕϏ)
ΑϏ(ι.λ.β,10,true);else if(ΔЋ&&Ω!=null){if(Ĉ>0) а(Ω.β,Ω.γ,Ω.ϋ,90,false);else а(Ω.β,-Ω.γ,-Ω.ϋ,90,false);}ǧ(true,false,true,Ѓ,Ɨ,ę==null?0:ę.Н,Ŀ);if(Γυ){ΔΒ=0;if(Ϊ==Β.Ǒ||Π==Ν.ΰ){Τ(Β.ȩ);return;}if(Ϊ==Β.б&&Αο){Τ(Β.Ĳ);return;}if(Ϊ==Β.б){Τ(Β.Ȑ);return;}
}}if(Ϊ==Β.ȩ||Ϊ==Β.Βη){ά ή=ȑ();if(Б){if(!ǽ(ή,dockDist*κ,true,out Ѓ)){ǿ(г.Ū);χ();return;}о(Ѓ,0);ΑϏ(ή.β,90,true);}if(Ф<followPathDock*κ&&Ф!=-1){о(Ѓ,10);а(ή.β,ή.γ,ή.ϋ,false);}if(л(MyShipConnectorStatus.Connectable)!=null||л(MyShipConnectorStatus.Connected)!=null)
{Τ(Β.ū);return;}if(Ф<wpReachedDist/2&&Ф!=-1){Τ(Β.Ũ);return;}}if(Ϊ==Β.Ũ||Ϊ==Β.č){if(Б){if(Ϊ==Β.Ũ){ά ή=ȑ();if(!ǽ(ή,dockDist*κ,true,out Ѓ)){ǿ(г.Ū);χ();return;}ǧ(true,true,false,Ѓ,0);а(ή.β,ή.γ,ή.ϋ,10,false);}if(Ϊ==Β.č){а(Ζ.β,Ζ.γ,Ζ.ϋ,0.5f,false);Ѓ=Ζ.Ε;
ǧ(true,true,false,Ѓ,0);}}if(ΑΪ){Ǆ(false,0,0,0,0);if(Ϊ==Β.Ũ) Τ(Β.ū);if(Ϊ==Β.č) χ();return;}}if(Ϊ==Β.ū){if(л(MyShipConnectorStatus.Connected)!=null){if(Π==Ν.ΰ) Τ(Β.Ȱ);else Τ(Β.ȗ);return;}ά ή=ȑ();if(Б){Я=0;Ης=DateTime.Now;Ќ=0;а(ή.β,ή.γ,ή.ϋ,false);}Vector3I ΓΛ=new Vector3I((int) ή.ш.X,(int) ή.ш.Y,(int) ή.ш.Z);
IMySlimBlock ΕΖ=Me.CubeGrid.GetCubeBlock(ΓΛ);float ΕΤ=dockingSpeed;float ΗЌ=dockingSpeed*5;float ΔЉ=Math.Max(1.5f,Math.Min(5f,κ*0.15f));if(!ǽ(ή,0,true,out Ѓ)||!ǽ(ή,ΔЉ,true,out Ɨ)||
ΕΖ==null||!ΕΖ.FatBlock.IsFunctional){ǿ(г.Ū);χ();return;}if(Я==1||(Vector3.Distance(ζ,Ѓ)<=ΔЉ*1.1f&&!Б))
Я=1;else{Vector3 Θμ=в(Ψ,Ɨ-ζ);Vector3 ΓΚ=в(Ψ,Ψ.GetNaturalGravity());float ΘΤ=Ƭ(Θμ,ΓΚ,null);ΕΤ=Math.Min(ΗЌ,ΘΤ);}ǧ(true,false,false,Ѓ,Ѓ-ζ,dockingSpeed,ΕΤ);if(Б) с=(float) Ф;IMyShipConnector ΒЍ=л(MyShipConnectorStatus.Connectable);if(ΒЍ!=null){ǧ(false,false,false,Ѓ,0);
if(Ќ>0) Ќ=0;Ќ--;if(Ќ<-5){ΒЍ.Connect();if(ΒЍ.Status==MyShipConnectorStatus.Connected){if(Π==Ν.ΰ) Τ(Β.Ȱ);else Τ(Β.ȗ);Βσ();ŵ(Ȋ,true);return;}}}else{float ΕΣ=(float) Math.Round(Ф,1);if(ΕΣ<с){Ќ=-1;с=ΕΣ;}else Ќ++;if(Ќ>20){Τ(Β.Βη);return;}}}if(Ϊ==Β.ȗ||Ϊ==Β.Ȱ||
Ϊ==Β.Βχ||Ϊ==Β.ΑЏ||Ϊ==Β.ı){bool Βό=false,Βύ=false;if(Π==Ν.ΰ){if(ȑ()==Ο) Βό=true;else if(ȑ()==Ζ) Βύ=true;}if(Б){ƛ=false;if(л(MyShipConnectorStatus.Connected)==null){Τ(Β.Ȯ);return;}Βσ();if(Βό) ǈ(ό.Ǚ);if(Βύ) ǈ(ύ.Ǚ);Ĩ=false;Ș=false;Ȝ=false;ƥ=false;}if(л(MyShipConnectorStatus.Connected)==null){χ();ǿ(г.Δθ);return;}
if(α!=Ξ.В||э==-1||Ǫ==х.None) Ș=true;else if(Ȃ>=100f) Ș=true;else if(Ȃ<=99f) Ș=false;if(α!=Ξ.В||ю==-1||ј.Count==0) Ȝ=true;else if(ȝ>=100f) Ȝ=true;else if(ȝ<=99) Ȝ=false;if(α!=Ξ.В||Ю==-1||ĉ.Count==0) ƥ=true;else ƥ=ĩ>=Ю;Ļ ф=null;if(Βό) ф=ό;
if(Βύ) ф=ύ;if(ф!=null&&(ф.З==η.ƾ||ф.З==η.ƽ)) Ș=true;if(ф!=null&&(ф.З==η.ΒΫ)) if(!Ĩ) Ș=false;if(ф!=null&&(ф.З==η.Βί||ф.З==η.Βέ)) Ȝ=true;if(ф!=null&&(ф.З==η.Βή)) if(!Ĩ) Ȝ=false;if(Ȟ){ChargeMode ΕΒ=Ș?ChargeMode.Auto:ChargeMode.Recharge;if(ф!=null&&(ф.З==η.ƽ||ф.З==η.ƾ))
ΕΒ=ChargeMode.Discharge;Βλ(ΕΒ);Βμ(!Ȝ);}if(!Ĩ){if(Π==Ν.ΰ)
Ĩ=α!=Ξ.В||ΖΪ(Б,true)||ƣ;else
Ĩ=α!=Ξ.В||Δι();}else{if(!Ș) Τ(Β.ı);if(!Ȝ) Τ(Β.ΑЏ);if(!ƥ) Τ(Β.Βχ);Б=false;}if(Ĩ&&Ș&&Ȝ&&ƥ){Βλ(ChargeMode.Auto);Βμ(false);if(α==Ξ.В){if(Π==Ν.ΰ){if(ȑ()==Ο) ǈ(ό.ǜ);else if(ȑ()==Ζ) ǈ(ύ.ǜ);if(ȑ()==Ο) Ĺ(Ζ,Ξ.ĭ);else Ĺ(Ο,Ξ.Ĭ);}}Τ(Β.Ȯ);return;}}if(Ϊ==Β.Ȯ){if(Б){IMyShipConnector Ȇ=л(MyShipConnectorStatus.Connected);
if(Ȇ==null){Τ(Β.б);return;}IMyShipConnector Ч=Ȇ.OtherConnector;Δχ(Ȇ,false);ŵ(Ȋ,false);ά ή=null;if(Vector3.Distance(Ȇ.GetPosition(),Ο.Ε)<5f&&Ο.ε) ή=Ο;if(Vector3.Distance(Ȇ.GetPosition(),Ζ.Ε)<5f&&Ζ.ε) ή=Ζ;if(ή!=null){if(!ǽ(ή,dockDist*κ,true,out Ѓ)){ǿ(г.Ū);χ();return;}
о(Ѓ,5);а(ή.β,ή.γ,ή.ϋ,false);}else о(ζ+Ч.WorldMatrix.Forward*dockDist*κ,5);if(α==Ξ.В) ǿ(г.ǂ);}if(Ф<wpReachedDist){і(щ,true);Τ(Β.б);return;}}if(Ϊ==Β.Ĳ){if(Б){Ǩ(true);і(ς,false);Ѓ=Ζ.Ε;о(Ѓ,20);а(Ζ.β,Ζ.γ,Ζ.ϋ,false);}if(Ф<wpReachedDist/2){Τ(Β.č);return;}}if(Ϊ==Β.ȭ){
if(Б){Ǩ(true);і(ς,false);int П=-1;double ǫ=-1;for(int i=Φ.Count-1;i>=0;i--){double Ϗ=Vector3.Distance(Φ[i].Ε,ζ);if(ǫ==-1||Ϗ<ǫ){П=i;ǫ=Ϗ;}}if(П==-1){Τ(Β.Ǒ);return;}Ѓ=Φ[П].Ε;о(Ѓ,10);а(Ζ.β,Ζ.γ,Ζ.ϋ,false);}if(Ф<wpReachedDist){Τ(Β.Ǒ);return;}}if(Ϊ==Β.Ʋ)
{bool ǭ=false;if(Б){Ǩ(true);if((Х==Ѕ.ΑЎ||Х==Ѕ.Ư)&&Ʒ()&&
į(ǟ,Ψ.GetNaturalGravity())<25&&Њ>=2&&Л>=2){Vector3 ƅ=ζ;if(ψ>0&&φ<Л-1) ƅ=Ȓ(ψ-1,φ+1,true);else if(ψ<Њ-1&&φ<Л-1) ƅ=Ȓ(ψ+1,φ+1,true);else if(ψ<Њ-1&&φ>0) ƅ=Ȓ(ψ+1,φ-1,true);else if(ψ>0&&φ>0) ƅ=Ȓ(ψ-1,φ-1,true);else ǭ=true;if(!ǭ) о(ƅ,10);
}else ǭ=true;}if(Ф<wpReachedDist/2) ǭ=true;if(ǭ){Τ(Β.Ǌ);return;}}if(Ϊ==Β.Βφ||Ϊ==Β.Ǌ){if(Б){ǧ(true,true,false,ζ,0);і(ς,false);Ǩ(true);Ќ=-1;Я=Х==Ѕ.ΒΛ||Х==Ѕ.ƶ?0:-1;}bool Ĝ=!ΑЈ();int Ατ=ȏ("STONE","ORE",О.Ď);if(Х==Ѕ.ΑЉ||Х==Ѕ.ƶ||Х==Ѕ.Ư)
Ατ+=ȏ("ICE","ORE",О.Ď);bool Ηό=Ατ>0;bool ΓΕ=false;if(Я>=0){float ļ=(float) Math.Sin(ΔΠ(Я))*ȍ/3f;float ΗΪ=(float) Math.Cos(ΔΠ(Я))*ǹ/3f;Vector3 Ηβ=Ȓ(ψ,φ,true)+Βτ(ǟ,Ȣ*-1,new Vector3(ļ,ΗΪ,0));о(Ηβ,0.3f);if(Ф<Math.Min(ȍ,ǹ)/10f) Я+=5f;if(Я>=360) Я=0;}if(Ќ==-1||Ατ<Ќ){Ќ=Ατ;с=0;}
else{с++;if(с>50) ΓΕ=true;}if(!Ηό||Ĝ||ΓΕ){if(!Ĝ){int Θρ=ȏ("","ORE",О.Ď);if(Ű(true)) Ĝ=true;else if(100-(Ј(Θρ,ΕЀ)*100)<minEjection) Ĝ=true;else ǿ(г.ǂ);}if(ΓΕ&&Ĝ) Λ="Ejection failed";if(Ϊ==Β.Ǌ){if(Ĝ){if(Ο.ε) Τ(Β.ȭ);else{χ();Đ();Λ="Can´t return, no dock found";}}
else Τ(Β.Ȑ);}else if(Ĝ) Τ(Β.Ē);else Τ(Β.Ķ);return;}}Б=false;}void ΖΧ(List<ά>Č,int μ,List<Vector3>Ơ,float ǵ,bool Ǣ,ref int ǖ){if(Ǣ){for(int i=0;i<Č.Count;i++) Č[i].Н=0;ǖ=-1;return;}if(μ==0) return;int ў=μ*-1;if(ǖ==-1) ǖ=ў>0?1:Č.Count-2;int ƍ=0;while(ǖ>=1&&ǖ<Č.Count-1)
{if(ƍ>50) return;ƍ++;try{if((ў<0&&ǖ>=1)||(ў>0&&ǖ<=Č.Count-2)){ά θ=Č[ǖ];bool Εϋ=false;for(int j=0;j<Ơ.Count;j++)
if(Vector3.Distance(θ.Ε,Ơ[j])<=ǵ){Εϋ=true;break;}if(!Εϋ){ά Αλ=Č[ǖ-ў];ά Εό=Č[ǖ+ў];Vector3 Θν=θ.Ε-Εό.Ε;Vector3 ΓЊ=Αλ.Ε-θ.Ε;Vector3 ΘΝ=θ.Ε+Vector3.Normalize(Θν)*ΓЊ.Length();Vector3 Ηο=Αλ.Ε-ΘΝ;Vector3 Αί=Ĕ(μ>0?θ.γ:θ.γ*-1,θ.β*-1,Ηο);Vector3 ΗЋ=Ĕ(μ>0?θ.γ:θ.γ*-1,θ.β*-1,ΓЊ);
Vector3 Αα=Ĕ(μ>0?θ.γ:θ.γ*-1,θ.β*-1,θ.π);θ.Н=(float) Math.Sqrt(Math.Pow(Αλ.Н,2)+Math.Pow(Ƭ(-ΗЋ,Αα,θ),2));for(int j=0;j<Ơ.Count;j++)
if(Vector3.Distance(Αλ.Ε,Ơ[j])<=ǵ){Vector3 Αβ=Ĕ(μ>0?θ.γ:θ.γ*-1,θ.β*-1,Ơ[j]-θ.Ε);float Θγ=Ƭ(-Αβ,Αα,θ);θ.Н=Math.Min(θ.Н,Θγ)/2f;}if(Αί.Length()==0) Αί=new Vector3(0,0,1);Vector3 ΗЊ=Ĕ(θ.γ,θ.β*-1,θ.π);float Θη=ŭ(-Αί,ΗЊ,θ);float ǋ=Ј(Θη,Ħ);float ΓЇ=(float) Math.Sqrt(Ј(Αί.Length()*1.0f,0.5f*ǋ));
θ.Н=Math.Min(θ.Н,(ΓЊ.Length()/ΓЇ)*ǩ);}}}catch{return;}ǖ+=ў;}ǖ=-1;}void Βυ(bool Ǣ){if(Ǣ){Ǝ=0;return;}if(Π==Ν.ΰ) return;float ǭ=Ъ*Math.Max(1,Э);if(Ƞ==Ъ) ǭ+=Math.Min(Э,Ы);float Θπ=Њ*Л*Math.Max(1,Э);Ǝ=Math.Max(Ǝ,(float) Math.Min(ǭ/Θπ*100.0,100));}MyDetectedEntityType ΔΫ()
{try{if(Ǿ(ρ,true)&&!ρ.LastDetectedEntity.IsEmpty()) return ρ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΗ(bool ǰ){if(Π==Ν.ǒ&&ΔΫ()==MyDetectedEntityType.None&&!ů(ρ,true))
return fastSpeed;else return ǰ?ŧ:Ŧ;}void ΗΖ(){float Εφ=0,ΕΜ=0,Ğ=0,Γπ=0;for(int i=0;i<ȱ.Count;i++){IMyBatteryBlock ž=ȱ[i];if(!Ǿ(ž,true)) continue;Εφ+=ž.MaxStoredPower;ΕΜ+=ž.CurrentStoredPower;Ğ+=ž.CurrentInput;Γπ+=ž.CurrentOutput;}Ȃ=(float) Math.Round(Ј(ΕΜ,Εφ)*100,1);
if(Ğ>=Γπ) Ǫ=х.ı;else Ǫ=х.ΔΡ;if(Ğ==0&&Γπ==0||Ȃ==100.0) Ǫ=х.м;if(ȱ.Count==0) Ǫ=х.None;}void ΗΚ(){float ΕΨ=0;for(int i=0;i<ј.Count;i++){IMyGasTank ΖΔ=ј[i];if(!Ǿ(ΖΔ,true)) continue;ΕΨ+=(float) ΖΔ.FilledRatio;}ȝ=Ј(ΕΨ,ј.Count)*100f;}void ΗΤ(){ĩ=0;try{for(int i=0;i<ĉ.Count;i++)
{IMyReactor Γφ=ĉ[i];if(!Ǿ(Γφ,true)) continue;List<MyInventoryItem>Ѐ=new List<MyInventoryItem>();Γφ.GetInventory(0).GetItems(Ѐ);float ώ=0;for(int j=0;j<Ѐ.Count;j++){MyInventoryItem υ=Ѐ[j];if(υ.Type.SubtypeId.ToUpper()=="URANIUM"&&υ.Type.TypeId.ToUpper().Contains("_INGOT"))
ώ+=(float) υ.Amount;}if(ώ<ĩ||i==0){ĩ=ώ;Γα=Γφ.CustomName;}}}catch(Exception e){ȟ=e;}}void ΗΗ(){ǲ=0;Ǭ=0;try{for(int i=0;i<Ń.Count;i++){IMyTerminalBlock Υ=Ń[i];if(!Ǿ(Υ,true)) continue;Ǭ+=(float) Υ.GetInventory(0).CurrentVolume;ǲ+=(float) Υ.GetInventory(0).MaxVolume;
}Αή=(float) Math.Min(Math.Round(Ј(Ǭ,ǲ)*100,1),100.0);}catch(Exception e){ȟ=e;}}void ΗΛ(){try{Ǜ.Clear();for(int i=0;i<Ń.Count;i++){IMyTerminalBlock Υ=Ń[i];if(!Ǿ(Υ,true)) continue;ΔΔ(Υ,О.ĵ);}if(Х!=Ѕ.ΒΪ){for(int i=0;i<Ę.Count;i++){IMyTerminalBlock Υ=Ę[i];if(!Ǿ(Υ,true)) continue;
ΔΔ(Υ,О.Ζω);}}ΗΑ(Ǜ);}catch(Exception e){ȟ=e;}}void ΖΩ(){if(ΑΝ){if(ΔΪ().Count>ΑΨ){ΑΝ=false;if(т!=к.Ů){χ();if(т==к.ΑБ) Đ();if(т==к.ǁ)
if(Ο.ε) Ƴ();else Đ();}Λ="Damage detected";}}}bool ΑЈ(){if(!ΑΩ) return true;if(α==Ξ.В){if(э>0&&Ǫ!=х.None)
if(Ȃ<=э){Λ="Low energy! Move home";return false;}if(Ю>0&&ĉ.Count>0)
if(ĩ<=Ю){Λ="Low fuel: "+Γα;return false;}if(ю>0&&ј.Count>0)
if(ȝ<=ю){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΔΪ(){List<IMyTerminalBlock>Ě=new List<IMyTerminalBlock>();for(int i=0;i<ź.Count;i++){IMyTerminalBlock Υ=ź[i];if(ů(Υ,false)) Ě.Add(Υ);}return Ě;}bool ů(IMyTerminalBlock Υ,bool ΓЀ){return(!Ǿ(Υ,ΓЀ)||!Υ.IsFunctional);
}bool Ǿ(IMyTerminalBlock Υ,bool ΓЀ){if(Υ==null) return false;try{IMyCubeBlock ΕΫ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΓЀ) return ΕΫ==Υ;else return ΕΫ.GetType()==Υ.GetType();}catch{return false;}}float Ζβ(IMyTerminalBlock Υ,String ѓ,String ΕЍ,String[]Ǯ){float ȇ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory ΗЃ=Υ.GetInventory(i);List<MyInventoryItem>Ѐ=new List<MyInventoryItem>();ΗЃ.GetItems(Ѐ);foreach(MyInventoryItem υ in Ѐ){if(Ǯ!=null&&(Ǯ.Contains(υ.Type.TypeId.ToUpper())||Ǯ.Contains(υ.Type.SubtypeId.ToUpper())))
continue;if((ѓ==""||υ.Type.TypeId.ToUpper()==ѓ)&&(ΕЍ==""||υ.Type.SubtypeId.ToUpper()==ΕЍ))
ȇ+=(float) υ.Amount;}}return ȇ;}Ь Ζι(String έ,String ο,О С,bool Ηλ){έ=έ.ToUpper();ο=ο.ToUpper();for(int i=0;i<Ǜ.Count;i++){Ь υ=Ǜ[i];if(υ.έ.ToUpper()==έ&&υ.ο.ToUpper()==ο&&(υ.Ŏ==С||С==О.Ď))
return υ;}Ь Ϋ=null;if(Ηλ){Ϋ=new Ь(έ,ο,0,С);Ǜ.Add(Ϋ);}return Ϋ;}int ȏ(String έ,String ο,О С){return ȏ(έ,ο,С,null);}int ȏ(String έ,String ο,О С,String[]Ǯ){int ȇ=0;έ=έ.ToUpper();ο=ο.ToUpper();for(int i=0;i<Ǜ.Count;i++){Ь υ=Ǜ[i];if(Ǯ!=null&&Ǯ.Contains(υ.έ.ToUpper())) continue;
if((έ==""||υ.έ.ToUpper()==έ)&&(ο==""||υ.ο.ToUpper()==ο)&&(υ.Ŏ==С||С==О.Ď))
ȇ+=υ.ώ;}return ȇ;}void ΔΔ(IMyTerminalBlock Υ,О С){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>Ѐ=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(Ѐ);for(int j=0;j<Ѐ.Count;j++)
Ζι(Ѐ[j].Type.SubtypeId,Ѐ[j].Type.TypeId.Replace("MyObjectBuilder_",""),С,true).ώ+=(int) Ѐ[j].Amount;}}void ΗΑ(List<Ь>Џ){for(int i=Џ.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ь a=Џ[j];Ь b=Џ[j+1];if(a.ώ<b.ώ) Џ.Move(j,j+1);}}bool Ű(bool ΘЃ){if(Ȩ&&Π!=Ν.ΰ)
if(Ц!=-1&&Ħ>=Ц){Λ="Ship too heavy";return true;}if(Αή>=Ǳ||ƛ){ƛ=false;Λ="Ship is full";return true;}return false;}bool Δι(){String[]Ǯ=null;if(!ȧ) Ǯ=new string[]{"ICE"};if(Π==Ν.ų) return ȏ("","ORE",О.ĵ,Ǯ)==0;if(Π==Ν.ǒ) return ȏ("","COMPONENT",О.ĵ,Ǯ)==0;else return ȏ("","",О.ĵ,Ǯ)==0;
}bool Ζό(){List<IMyCockpit>ΒЌ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΒЌ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΒЌ.Count;i++)
if(ΒЌ[i].IsUnderControl) return true;return false;}bool ΖΪ(bool Ǣ,bool Αρ){IMyShipConnector Ǘ=л(MyShipConnectorStatus.Connected);if(Ǘ==null) return false;if(Vector3.Distance(Ο.Ε,Ǘ.GetPosition())<5)
return ΔΝ(ό,Ǣ,Αρ);if(Vector3.Distance(Ζ.Ε,Ǘ.GetPosition())<5)
return ΔΝ(ύ,Ǣ,Αρ);return false;}bool ΔΝ(Ļ Ȅ,bool Ǣ,bool Αρ){if(Ǣ) Ȅ.ΖЅ();Ȅ.ΗΠ();bool у=false;String ђ="";switch(Ȅ.З){case η.Βά:{ђ="Waiting for command";у=false;break;}case η.Δρ:{ђ="Waiting for cargo";у=Ű(true);break;}case η.Δπ:{ђ="Unloading";у=Δι();break;}case η.Βα:{у=true;break;}
case η.ΒΫ:{ђ="Charging batteries";у=Ȃ>=100f;break;}case η.ƾ:{ђ="Discharging batteries";у=Ȃ<=25f;break;}case η.ƽ:{ђ="Discharging batteries";у=Ȃ<=0f;break;}case η.Βή:{ђ="Filling up hydrogen";у=ȝ>=100f;break;}case η.Βί:{ђ="Unloading hydrogen";у=ȝ<=25f;break;}
case η.Βέ:{ђ="Unloading hydrogen";у=ȝ<=0f;break;}case η.Βΰ:{bool Εβ=Ζό();if(!Εβ) Ȅ.Γσ=true;у=Ȅ.Γσ&&Εβ;ђ="Waiting for passengers";break;}}if(!у) Ȅ.ΑЇ(true);if(у&&Ȅ.ΒΙ()){у=Ȅ.ΑЇ(false);ђ="Undocking in: "+ƴ((int) Math.Max(0,Ȅ.ǘ-Ȅ.ā));}if(Αρ) ΖΠ=ђ;return у;}void ΖΤ(){
if(!Ŋ) return;if(ς.Count<=1) return;float ΖΙ=0,ΖΘ=0;for(int i=0;i<ς.Count;i++){IMyTerminalBlock Ǹ=ς[i];if(ů(Ǹ,true)) continue;ΖΙ+=(float) Ǹ.GetInventory(0).MaxVolume;ΖΘ+=(float) Ǹ.GetInventory(0).CurrentVolume;}float ΔЌ=(float) Math.Round(Ј(ΖΘ,ΖΙ),5);for(int ΕЂ=0;ΕЂ<Math.Max(1,Math.Floor(ς.Count/10f));ΕЂ++)
{float ΓΟ=0,Γΰ=0;float Εΰ=0,Εσ=0;IMyTerminalBlock ț=null,Ă=null;for(int i=0;i<ς.Count;i++){IMyTerminalBlock Ǹ=ς[i];if(ů(Ǹ,true)) continue;float Γθ=(float) Ǹ.GetInventory(0).MaxVolume;float ΑΡ=Ј((float) Ǹ.GetInventory(0).CurrentVolume,Γθ);if(ț==null||ΑΡ>ΓΟ){ț=Ǹ;ΓΟ=ΑΡ;Εΰ=Γθ;}
if(Ă==null||ΑΡ<Γΰ){Ă=Ǹ;Γΰ=ΑΡ;Εσ=Γθ;}}if(ț==null||Ă==null||ț==Ă) return;if(checkConveyorSystem&&!ț.GetInventory(0).IsConnectedTo(Ă.GetInventory(0))){if(ΒЂ>20) Λ="Inventory balancing failed";else ΒЂ++;return;}ΒЂ=0;List<MyInventoryItem>Ѐ=new List<MyInventoryItem>();
ț.GetInventory(0).GetItems(Ѐ);float Ƨ=0;if(Ѐ.Count==0) continue;MyInventoryItem ΑΣ=Ѐ[0];String Εε=ΑΣ.Type.TypeId+ΑΣ.Type.SubtypeId;if(!ΖΟ.TryGetValue(Εε,out Ƨ)){if(ΖϏ(ț.GetInventory(0),0,Ă.GetInventory(0),out Ƨ))
ΖΟ.Add(Εε,Ƨ);else return;}float Θξ=((ΓΟ-ΔЌ)*Εΰ/Ƨ);float Θο=((ΔЌ-Γΰ)*Εσ/Ƨ);int ΓЌ=(int) Math.Min(Θο,Θξ);if(ΓЌ<=0) return;if((float) ΑΣ.Amount<ΓЌ)
ț.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,ΑΣ.Amount);else
ț.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,ΓЌ);}}bool ΖϏ(IMyInventory Ɯ,int Α,IMyInventory Ηξ,out float ΖΞ){ΖΞ=0;float Ηή=(float) Ɯ.CurrentVolume;List<MyInventoryItem>ΒЃ=new List<MyInventoryItem>();Ɯ.GetItems(ΒЃ);float ΔЏ=0;for(int i=0;i<ΒЃ.Count;i++) ΔЏ+=(float) ΒЃ[i].Amount;
Ɯ.TransferItemTo(Ηξ,Α,null,null,1);float ΖΜ=Ηή-(float) Ɯ.CurrentVolume;List<MyInventoryItem>Βω=new List<MyInventoryItem>();Ɯ.GetItems(Βω);float ΔЄ=0;for(int i=0;i<Βω.Count;i++) ΔЄ+=(float) Βω[i].Amount;if(ΖΜ==0f||!ΒΜ(0.9999,ΔЏ-ΔЄ,1.0001))
return false;ΖΞ=ΖΜ;return true;}void ΗΙ(){Vector3 Ɖ=Ѓ+Αψ;Vector3 Ƿ=Ɖ-ζ;if(Ƿ.Length()==0) Ƿ=new Vector3(0,0,-1);Vector3 Αβ=в(Ψ,Ƿ);Vector3 ΖΕ=Vector3.Normalize(Αβ);Vector3 ō=в(Ψ,Ψ.GetNaturalGravity());float Θή=Γρ>0?Math.Max(0,1-į(Ƿ,ΕΪ)/5):0;float Н=(float) Math.Min((Γε>0?Γε:1000f),Math.Max(Ƭ(-Αβ,ō,null),Γρ*Θή));
if(!ΖΗ) Н=0;if(ΕЊ) Н=Math.Max(0,1-ΑΛ/ǌ)*Н;if(generalSpeedLimit>0) Н=Math.Min(generalSpeedLimit,Н);if(ŕ) Н=0;if(Ţ>=0) Н=Math.Min(Н,Ţ);if(ΕЋ) Н*=(float) Math.Min(1,Ј(Ƿ.Length(),wpReachedDist)/2);Vector3 Επ=в(Ψ,Ψ.GetShipVelocities().LinearVelocity);float ΗΩ=(float)(Math.Max(0,15-į(-ΖΕ,-Επ))/15)*0.85f+0.15f;
ΓЄ+=Math.Sign(ΗΩ-ΓЄ)/10f;Vector3 ȳ=ΖΕ*Н*ΓЄ-(Επ);Vector3 ΒЀ=ΒΖ(ȳ,null);if(Αυ&&Ф>0.1f){ȳ.X*=Βξ(ȳ.X,ref Αφ.X,1f,ΒЀ.X,20);ȳ.Y*=Βξ(ȳ.Y,ref Αφ.Y,1f,ΒЀ.Y,20);ȳ.Z*=Βξ(ȳ.Z,ref Αφ.Z,1f,ΒЀ.Z,20);}else Αφ=new Vector3(1,1,1);ơ=Ħ*ȳ-ō*Ħ;ΔΕ(ơ,Αυ);Ф=Vector3.Distance(ζ,Ɖ);
}float Βξ(float value,ref float ω,float ў,float ȡ,float Γζ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(ω)) ω+=Math.Sign(ω)*ў;else ω=value;if(value==0) ω=1;float Ϋ=Math.Abs(ω);if(Ϋ<Γζ||ȡ==0) return Ϋ;ω=Math.Min(Γζ,Math.Max(-Γζ,ω));Ϋ=Math.Abs(ȡ);return Ϋ;
}void ΔЃ(){float ѐ=90,Ǐ=90,ќ=90;float ǣ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǰ,љ,ƒ,Γέ;if(Γή){ǰ=Vector3.Normalize(Ѓ-ζ);ƒ=в(Ψ,ǰ);љ=в(Ψ,ΓЂ);ѐ=į(ƒ,new Vector3(0,-1,0))-90;Ǐ=ď(љ,new Vector3(-1,0,0),љ.Y);
ќ=ď(ƒ,new Vector3(-1,0,0),ƒ.Z);}else{ǰ=ΓЃ;љ=в(Ψ,ΓЂ);ƒ=в(Ψ,ΓЃ);Γέ=в(Ψ,ΖΖ);ѐ=ď(љ,new Vector3(0,0,1),љ.Y);Ǐ=ď(љ,new Vector3(-1,0,0),љ.Y);ќ=ď(Γέ,new Vector3(0,0,1),Γέ.X);}if(ΔІ&&Ʒ()){Vector3 ΓΚ=Ψ.GetNaturalGravity();љ=в(Ψ,ΓΚ);ѐ=ď(љ,new Vector3(0,0,1),љ.Y);
Ǐ=ď(љ,new Vector3(-1,0,0),љ.Y);}if(!ΒΜ(-45,Ǐ,45)){ѐ=0;ќ=0;}if(!ΒΜ(-45,ќ,45)) ѐ=0;Ǆ(ΓΜ,1,(-ѐ)*ǣ,(-ќ)*ǣ,(-Ǐ)*ǣ);ΑΛ=Math.Max(Math.Abs(ѐ),Math.Max(Math.Abs(Ǐ),Math.Abs(ќ)));ΑΪ=ΑΛ<=ǌ;}void ΖΨ(IMyTerminalBlock Αμ){if(Αμ==null) return;Ǧ=new float[3,2];џ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȍ.Count;i++){IMyThrust ж=Ȍ[i];if(!ж.IsFunctional) continue;Vector3 μ=в(Αμ,ж.WorldMatrix.Backward);float Ā=ж.MaxEffectiveThrust;if(Math.Round(μ.X,2)!=0.0)
if(μ.X>=0) Ǧ[0,0]+=Ā;else Ǧ[0,1]-=Ā;if(Math.Round(μ.Y,2)!=0.0)
if(μ.Y>=0) Ǧ[1,0]+=Ā;else Ǧ[1,1]-=Ā;if(Math.Round(μ.Z,2)!=0.0)
if(μ.Z>=0) Ǧ[2,0]+=Ā;else Ǧ[2,1]-=Ā;String ѓ=Δα(ж);float[,]Ǎ=null;if(џ.ContainsKey(ѓ)) Ǎ=џ[ѓ];else{Ǎ=new float[3,2];џ.Add(ѓ,Ǎ);}float ȡ=ж.MaxThrust;if(Math.Round(μ.X,2)!=0.0)
if(μ.X>=0) Ǎ[0,0]+=ȡ;else Ǎ[0,1]-=ȡ;if(Math.Round(μ.Y,2)!=0.0)
if(μ.Y>=0) Ǎ[1,0]+=ȡ;else Ǎ[1,1]-=ȡ;if(Math.Round(μ.Z,2)!=0.0)
if(μ.Z>=0) Ǎ[2,0]+=ȡ;else Ǎ[2,1]-=ȡ;}}static String Δα(IMyThrust ж){return ж.BlockDefinition.SubtypeId;}Vector3 ΒΡ(Vector3 ї,float[,]ŏ){return new Vector3(
ї.X>=0?ŏ[0,0]:ŏ[0,1],
ї.Y>=0?ŏ[1,0]:ŏ[1,1],
ї.Z>=0?ŏ[2,0]:ŏ[2,1]);}bool Δΰ(ά Ω,String ѓ,out float ΓΔ){ΓΔ=0;int Α=ȋ.IndexOf(ѓ);if(Α==-1||Ω.ǥ==null||Α>=Ω.ǥ.Length)
return false;ΓΔ=Ω.ǥ[Α];if(ΓΔ==-1) return false;return true;}Vector3 ΒΖ(Vector3 ї,ά Ω){if(Ω!=null){Vector3 Ϋ=new Vector3();for(int i=0;i<џ.Keys.Count;i++){String ѓ=џ.Keys.ElementAt(i);float Ƈ=0;if(!Δΰ(Ω,ѓ,out Ƈ))
return ΒΡ(ї,Ǧ);Ϋ+=ΒΡ(ї,џ.Values.ElementAt(i))*Ƈ;}return Ϋ;}return ΒΡ(ї,Ǧ);}float ŭ(Vector3 ї,ά Ω){return ŭ(ї,new Vector3(),Ω);}float ŭ(Vector3 ї,Vector3 π,ά Ω){Vector3 Θζ=ΒΖ(ї,Ω);Vector3 Ā=Θζ+π*Ħ;float Ĥ=(Ā/ї).AbsMin();return(float)(ї*Ĥ).Length();}float Ƭ(Vector3 Αΰ,Vector3 ō,ά Ω)
{if(Αΰ.Length()==0) return 0;float ΔЅ=1;if(ō.Length()>0) ΔЅ=Math.Min(1,į(-ō,Αΰ)/90)*0.4f+0.6f;float ơ=ŭ(Αΰ,ō,Ω);if(ơ==0) return 0.1f;float ǋ=Ј(ơ,Ħ);float ΓЇ=(float) Math.Sqrt(Ј(Αΰ.Length(),ǋ*0.5f));return ǋ*ΓЇ*ΔЅ*ǩ;}float Ƶ(Vector3 ǰ,Vector3 и,Vector3 π,ά Ω){if(π.Length()==0f) return 0;
Vector3 Αα=Ĕ(ǰ,и,Vector3.Normalize(π));float ǚ=ŭ(-Αα,Ω);return ǚ/π.Length();}void ΔΙ(bool ΗЂ){float ǎ=0;float ΓΗ=0.9f;if(ΗЂ){Ц=-1;Ī=0;Ασ=null;if(α!=Ξ.н&&Ζ.π.Length()!=0){ǎ=ΓΗ*Ƶ(Ζ.γ,Ζ.β*-1,Ζ.π,null);if(ǎ<Ц||Ц==-1) Ц=ǎ;}if(Ο.ε&&Ο.π.Length()!=0){ǎ=ΓΗ*Ƶ(Ο.γ,Ο.β*-1,Ο.π,null);
if(ǎ<Ц||Ц==-1) Ц=ǎ;}return;}if(Ī==-1) return;if(Ī>=0){int ΕЄ=0;while(Ī<Φ.Count){if(ΕЄ>100) return;ΕЄ++;ά Ω=Φ[Ī];if(Ω.π.Length()!=0f){ǎ=ΓΗ*Math.Min(
Ƶ(Ω.γ,Ω.β*-1,Ω.π,Ω),
Ƶ(Ω.γ*-1,Ω.β*-1,Ω.π,Ω));if(ǎ<Ц||Ц==-1) Ц=ǎ;}else Ασ=Ω;Ī++;}Ī=-1;}bool ΓΝ=true;float ć=0;if(Ασ!=null){for(int i=0;i<џ.Count;i++){String ѓ=џ.Keys.ElementAt(i);float[,]Ǎ=џ.Values.ElementAt(i);float Ƈ=0;if(!Δΰ(Ασ,ѓ,out Ƈ)){ΓΝ=false;break;}for(int a=0;a<Ǎ.GetLength(0);a++)
for(int b=0;b<Ǎ.GetLength(1);b++){float ė=Math.Abs(Ǎ[a,b]*Ƈ);if(ė==0) continue;ΓΝ=true;if(ć==0||ė<ć) ć=ė;}}}if(!ΓΝ){for(int a=0;a<Ǧ.GetLength(0);a++)
for(int b=0;b<Ǧ.GetLength(1);b++){float ė=Math.Abs(Ǧ[a,b]);if(ė==0) continue;if(ć==0||ė<ć) ć=ė;}}if(ć>0){float ΗЎ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ǎ=Ј(ć,ΗЎ);if(ǎ>0)
if(ǎ<Ц||Ц==-1) Ц=ǎ;}}void ΔΕ(Vector3 ǚ,bool Ў){if(!Ў){for(int i=0;i<Ȍ.Count;i++)
Ȍ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒϏ=ΒΖ(ǚ,null);float Θώ=Math.Min(1,Math.Abs(Ј(ǚ.X,ΒϏ.X)));float ΘϏ=Math.Min(1,Math.Abs(Ј(ǚ.Y,ΒϏ.Y)));float ΘЀ=Math.Min(1,Math.Abs(Ј(ǚ.Z,ΒϏ.Z)));for(int i=0;i<Ȍ.Count;i++){IMyThrust ж=Ȍ[i];Vector3 μ=ΖЇ(в(Ψ,ж.WorldMatrix.Backward),1);
if(μ.X!=0&&Math.Sign(μ.X)==Math.Sign(ǚ.X))
ж.SetValueFloat("Override",ж.MaxThrust*Θώ);else if(μ.Y!=0&&Math.Sign(μ.Y)==Math.Sign(ǚ.Y))
ж.SetValueFloat("Override",ж.MaxThrust*ΘϏ);else if(μ.Z!=0&&Math.Sign(μ.Z)==Math.Sign(ǚ.Z))
ж.SetValueFloat("Override",ж.MaxThrust*ΘЀ);else
ж.SetValueFloat("Override",0.0f);}}void Ǆ(bool Ў,float ΘΚ,float ѐ,float ќ,float Ǐ){for(int i=0;i<ΑΥ.Count;i++){IMyGyro Љ=ΑΥ[i];Љ.GyroOverride=Ў;if(!Ў) Љ.GyroPower=100;else Љ.GyroPower=ΘΚ;if(!Ў) continue;Vector3 Ȉ=Ψ.WorldMatrix.Forward;Vector3 ё=Ψ.WorldMatrix.Right;Vector3 и=Ψ.WorldMatrix.Up;
Vector3 ņ=Љ.WorldMatrix.Forward;Vector3 ň=Љ.WorldMatrix.Up;Vector3 Ň=Љ.WorldMatrix.Left*-1;if(ņ==Ȉ) Љ.SetValueFloat("Roll",Ǐ);else if(ņ==(Ȉ*-1)) Љ.SetValueFloat("Roll",Ǐ*-1);else if(ň==(Ȉ*-1)) Љ.SetValueFloat("Yaw",Ǐ);else if(ň==Ȉ) Љ.SetValueFloat("Yaw",Ǐ*-1);else if(Ň==Ȉ) Љ.SetValueFloat("Pitch",Ǐ);
else if(Ň==(Ȉ*-1)) Љ.SetValueFloat("Pitch",Ǐ*-1);if(Ň==(ё*-1)) Љ.SetValueFloat("Pitch",ѐ);else if(Ň==ё) Љ.SetValueFloat("Pitch",ѐ*-1);else if(ň==ё) Љ.SetValueFloat("Yaw",ѐ);else if(ň==(ё*-1)) Љ.SetValueFloat("Yaw",ѐ*-1);else if(ņ==(ё*-1)) Љ.SetValueFloat("Roll",ѐ);
else if(ņ==ё) Љ.SetValueFloat("Roll",ѐ*-1);if(ň==(и*-1)) Љ.SetValueFloat("Yaw",ќ);else if(ň==и) Љ.SetValueFloat("Yaw",ќ*-1);else if(Ň==и) Љ.SetValueFloat("Pitch",ќ);else if(Ň==(и*-1)) Љ.SetValueFloat("Pitch",ќ*-1);else if(ņ==и) Љ.SetValueFloat("Roll",ќ);else if(ņ==(и*-1)) Љ.SetValueFloat("Roll",ќ*-1);
}}void ΔϏ(){this.ΓΜ=false;}void а(Vector3 ƃ,Vector3 Ȉ,Vector3 Γά,float Ȧ,bool Ʀ){ΑϏ(ƃ,Ȧ,Ʀ);ǌ=Ȧ;Γή=false;this.ΓЃ=Ȉ;this.ΖΖ=Γά;}void а(Vector3 ƃ,Vector3 Ȉ,Vector3 Γά,bool Ʀ){а(ƃ,Ȉ,Γά,2f,Ʀ);}void ΑϏ(Vector3 ƃ,float Ȧ,bool Ʀ){ǌ=Ȧ;this.ΓΜ=true;this.ΔІ=Ʀ;Γή=true;ΑΪ=false;
this.ΓЂ=ƃ;}void Βσ(){ǧ(false,false,false,Ѓ,0);Αυ=false;}void о(Vector3 λ,float ǣ){ǧ(true,false,false,λ,ǣ);}void ǧ(bool Ў,bool Γό,bool ΓА,Vector3 λ,float ǣ){ǧ(Ў,Γό,ΓА,λ,λ-ζ,0.0f,ǣ);}void ǧ(bool Ў,bool Γό,bool ΓА,Vector3 λ,Vector3 ΘΨ,float ΘΩ,float ǣ){Αυ=true;this.ΖΗ=Ў;
Ѓ=λ;this.Γε=ǣ;this.Γρ=ΘΩ;this.ΕЋ=Γό;this.ΕЊ=ΓА;this.ΕΪ=ΘΨ;Ф=Vector3.Distance(λ,ζ);}void ΖЈ(){ϊ.GetBlocksOfType(ź,ѕ);}void ΖЉ(){if(Αξ){І=2;return;}List<IMyRemoteControl>Ƙ=new List<IMyRemoteControl>();List<IMySensorBlock>ΕЈ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ɵ=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(Ƙ,ѕ);ϊ.GetBlocksOfType(М,ѕ);ϊ.GetBlocksOfType(ΕЈ,ѕ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ɵ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);М=ǐ(М,pamTag,true);Δϊ();Δϋ(Ɵ);ē(М,setLCDFontAndSize,1.4f,false);ē(Ƕ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΖΑ=ǐ(ΕЈ,pamTag,true);if(ΖΑ.Count>0) ρ=ΖΑ[0];else ρ=null;if(Π==Ν.ų){ϊ.GetBlocksOfType(ς,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ς.Count==0){І=1;Λ="Drills are missing";}}else if(Π==Ν.ǒ){ϊ.GetBlocksOfType(ς,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ς.Count==0){І=1;Λ="Grinders are missing";}if(ρ==null){І=1;Λ="Sensor is missing";}}else if(Π==Ν.ΰ){ϊ.GetBlocksOfType(ΓЈ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΕБ=ǐ(Ƙ,pamTag,true);if(ΕБ.Count>0) Ƙ=ΕБ;if(Ƙ.Count>0) Ψ=Ƙ[0];else{Ψ=null;І=2;Λ="Remote is missing";return;}κ=(float) Ψ.CubeGrid.WorldVolume.Radius*2;ћ=null;if(Π!=Ν.ΰ){ΔΚ();if(ς.Count>0&&ћ!=null){if(ρ!=null&&(ћ.WorldMatrix.Forward!=ρ.WorldMatrix.Forward||
!(Ψ.WorldMatrix.Forward==ρ.WorldMatrix.Up||Ψ.WorldMatrix.Down==ρ.WorldMatrix.Down))){І=1;Λ="Wrong sensor direction";}if(ћ.WorldMatrix.Forward!=Ψ.WorldMatrix.Forward&&
ћ.WorldMatrix.Forward!=Ψ.WorldMatrix.Down){І=2;Λ="Wrong remote direction";}}}}void ΖЊ(){ϊ.GetBlocksOfType(Ȋ,ѕ);ϊ.GetBlocksOfType(щ,ѕ);ϊ.GetBlocksOfType(Ȍ,ѕ);ϊ.GetBlocksOfType(ΑΥ,ѕ);ϊ.GetBlocksOfType(ȱ,ѕ);ϊ.GetBlocksOfType(ĉ,ѕ);ϊ.GetBlocksOfType(ј,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(Ας,ѕ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
щ=ǐ(щ,"ConnectorMedium",false);else
щ=ǐ(щ,"Connector",false);List<IMyShipConnector>ΕЏ=ǐ(щ,pamTag,true);if(ΕЏ.Count>0) щ=ΕЏ;if(І<=1){if(щ.Count==0){І=1;Λ="Connector is missing";}if(ΑΥ.Count==0){І=1;Λ="Gyros are missing";}if(Ȍ.Count==0){І=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΖΒ=ǐ(Ας,pamTag,true);
if(ΖΒ.Count>0) Ας=ΖΒ;List<IMyLandingGear>ΕА=ǐ(Ȋ,pamTag,true);if(ΕА.Count>0) Ȋ=ΕА;for(int i=0;i<Ȋ.Count;i++) Ȋ[i].AutoLock=false;List<IMyBatteryBlock>ΕЎ=ǐ(ȱ,pamTag,true);if(ΕЎ.Count>0) ȱ=ΕЎ;List<IMyGasTank>ΖΓ=ǐ(ј,pamTag,true);if(ΖΓ.Count>0) ј=ΖΓ;}void ΖЋ(){ϊ.GetBlocksOfType(Ę,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ń.Clear();for(int i=Ę.Count-1;i>=0;i--){if(Ζϊ(Ę[i])){Ń.Add(Ę[i]);Ę.RemoveAt(i);}}}bool Ζϊ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΒΚ(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΔΚ(){if(Ψ==null) return;ћ=null;float Αθ=0,Αγ=0,Αι=0,Αδ=0,Ακ=0,Αε=0;List<IMyTerminalBlock>ΓЁ=ǐ(ς,pamTag,true);bool ΘΓ=ΓЁ.Count==0;if(ΓЁ.Count>0) ћ=ΓЁ[0];else if(ς.Count>0) ћ=ς[0];int ȇ=0;for(int i=0;i<ς.Count;i++)
{if(ς[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward){if(ΘΓ){І=2;Λ="Mining direction is unclear!";return;}continue;}ȇ++;Vector3 А=ΒΓ(Ψ,ς[i].GetPosition());if(i==0){Αθ=А.X;Αγ=А.X;Αι=А.Y;Αδ=А.Y;Ακ=А.Z;Αε=А.Z;}Αγ=Math.Max(А.X,Αγ);Αθ=Math.Min(А.X,Αθ);Αδ=Math.Max(А.Y,Αδ);Αι=Math.Min(А.Y,Αι);
Αε=Math.Max(А.Z,Αε);Ακ=Math.Min(А.Z,Ακ);}ȍ=(Αγ-Αθ)*(1-ī)+drillRadius*2;ǹ=(Αδ-Αι)*(1-ă)+drillRadius*2;if(ћ!=null&&ћ.WorldMatrix.Forward==Ψ.WorldMatrix.Down)
ǹ=(Αε-Ακ)*(1-ă)+drillRadius*2;}List<T>ǐ<T>(List<T>Џ,String Ǥ,bool ΑΦ){List<T>Ϋ=new List<T>();for(int i=0;i<Џ.Count;i++)
if(ΒΚ(Џ[i],Ǥ,ΑΦ)) Ϋ.Add(Џ[i]);return Ϋ;}bool ΒΚ<T>(T Υ,String Ǥ,bool ΑΦ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(ΑΦ&&b.CustomName.ToUpper().Contains(Ǥ.ToUpper())) return true;if(!ΑΦ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǥ.ToUpper())) return true;return false;}void і<T>(List<T>Џ,bool Ў)
{for(int i=0;i<Џ.Count;i++)
Δχ((IMyTerminalBlock) Џ[i],Ў);}void Δχ(IMyTerminalBlock Υ,bool Ў){if(Υ==null) return;String ΗΧ=Ў?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(ΗΧ);a.Apply(Υ);}void Ǩ(bool Ў){Γύ=Ў;if(!ħ) return;і(Ας,Ў);}void Βλ(ChargeMode Ђ){for(int i=0;i<ȱ.Count;i++) ȱ[i].ChargeMode=Ђ;}void Βμ(bool Θΰ)
{for(int i=0;i<ј.Count;i++) ј[i].Stockpile=Θΰ;}void ŵ(List<IMyLandingGear>ΓΘ,bool Ερ){for(int i=0;i<ΓΘ.Count;i++){if(Ερ) ΓΘ[i].Lock();if(!Ερ) ΓΘ[i].Unlock();}}void Δψ(bool Ў){if(ΕΝ==Ў) return;List<IMyShipController>ΑΙ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΙ,ѕ);if(ΑΙ.Count==0) return;
for(int i=0;i<ΑΙ.Count;i++)
ΑΙ[i].DampenersOverride=Ў;ΕΝ=Ў;}IMyShipConnector л(MyShipConnectorStatus ђ){for(int i=0;i<щ.Count;i++){if(!Ǿ(щ[i],true)) continue;if(щ[i].Status==ђ) return щ[i];}return null;}void ĕ(bool Γω,bool ΑΟ,float Ηψ,float Ηέ){if(ρ==null||ς.Count==0) return;Vector3 ΑΖ=new Vector3();
int ȇ=0;for(int i=0;i<ς.Count;i++){if(ς[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward) continue;ȇ++;ΑΖ+=ς[i].GetPosition();}ΑΖ=ΑΖ/ȇ;Vector3 Œ=ΒΓ(ρ,ΑΖ);ρ.Enabled=true;ρ.ShowOnHUD=Γω;ρ.LeftExtend=(ΑΟ?1:р)/2f*ȍ-Œ.X;ρ.RightExtend=(ΑΟ?1:р)/2f*ȍ+Œ.X;ρ.TopExtend=(ΑΟ?1:п)/2f*ǹ+Œ.Y;
ρ.BottomExtend=(ΑΟ?1:п)/2f*ǹ-Œ.Y;ρ.FrontExtend=(Γω?Э:Ηψ)-Œ.Z;ρ.BackExtend=Γω?0:Ηέ+κ*0.75f+Œ.Z;ρ.DetectFloatingObjects=true;ρ.DetectAsteroids=false;ρ.DetectLargeShips=true;ρ.DetectSmallShips=true;ρ.DetectStations=true;ρ.DetectOwner=true;ρ.DetectSubgrids=false;
ρ.DetectPlayers=false;ρ.DetectEnemy=true;ρ.DetectFriendly=true;ρ.DetectNeutral=true;}void ē<T>(List<T>Џ,bool ΘΫ,float Ηφ,bool ΗЄ){for(int i=0;i<Џ.Count;i++){IMyTextSurface ƞ=null;if(Џ[i]is IMyTextSurface) ƞ=(IMyTextSurface) Џ[i];if(ƞ!=null){ƞ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘΫ) continue;ƞ.Font="Debug";if(ΗЄ) continue;ƞ.FontSize=Ηφ;}}}bool Ʒ(){Vector3D Є;return this.Ψ.TryGetPlanetPosition(out Є);}int Ζς(Vector3 Θβ,int ΕЌ){if(ΕЌ==-1) return 0;double П=-1;int ș=-1;for(int i=Φ.Count-1;i>=0;i--){double Ϗ=Vector3.Distance(Φ[i].Ε,Θβ);if(П==-1||Ϗ<П){ș=i;П=Ϗ;}
}return Math.Sign(ș-ΕЌ);}bool Ζϋ(Vector3 Є){List<Vector3>Т=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Т.Add(Φ[i].Ε);if(Ο.ε&&Φ.Count>=1){Vector3 ľ=new Vector3();ǽ(Ο,dockDist*κ,false,out ľ);if(Vector3.Distance(Ο.Ε,Φ.First().Ε)<Vector3.Distance(Ο.Ε,Φ.Last().Ε)){Т.Insert(0,ľ);Т.Insert(0,Ο.Ε);}
else{Т.Add(ľ);Т.Add(Ο.Ε);}}if(Π==Ν.ΰ){if(Ζ.ε&&Φ.Count>=1){Vector3 Βϋ=new Vector3();ǽ(Ζ,dockDist*κ,false,out Βϋ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Т.Insert(0,Βϋ);Т.Insert(0,Ζ.Ε);}else{Т.Add(Βϋ);Т.Add(Ζ.Ε);}}}else{if(α!=Ξ.н)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Т.Insert(0,Ζ.Ε);else Т.Add(Ζ.Ε);}int ș=-1;double ǫ=-1;for(int i=0;i<Т.Count;i++){double Ϗ=Vector3.Distance(Т[i],Є);if(Ϗ<ǫ||ǫ==-1){ǫ=Ϗ;ș=i;}}if(Т.Count==0) return false;double ΓΑ=Vector3.Distance(Т[ș],Є);double ΘΛ=Vector3.Distance(Т[Math.Max(0,ș-1)],Т[ș])*1.5f;
double ΘΒ=Vector3.Distance(Т[Math.Min(Т.Count-1,ș+1)],Т[ș])*1.5f;return ΓΑ<ΘΛ||ΓΑ<ΘΒ||ΓΑ<laneOffset*κ*1.5f;}static float Ј(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΖЇ(Vector3 v,int ΒЏ){return new Vector3(Math.Round(v.X,ΒЏ),Math.Round(v.Y,ΒЏ),Math.Round(v.Z,ΒЏ));
}float į(Vector3 a,Vector3 b){if(a==b) return 0;float Ησ=(a*b).Sum;float Ελ=a.Length();float Εμ=b.Length();if(Ελ==0||Εμ==0) return 0;float Ϋ=(float)((180.0f/Math.PI)*Math.Acos(Ησ/(Ελ*Εμ)));if(float.IsNaN(Ϋ)) return 0;return Ϋ;}float ď(Vector3 v,Vector3 Αμ,float Θά){float ļ=į(v,Αμ);
if(Θά>0f) ļ*=-1;if(ļ>-90f) return ļ-90f;else return 180f-(-ļ-90f);}double ΔΠ(float Ημ){return(Math.PI/180)*Ημ;}bool ΒΜ(double ΗЍ,double value,double Γδ){return(value>=ΗЍ&&value<=Γδ);}Vector3 ΗΕ(IMyTerminalBlock Υ,Vector3 А){return Vector3D.Transform(А,Υ.WorldMatrix);
}Vector3 ΒΓ(IMyTerminalBlock Υ,Vector3 Θϋ){return в(Υ,Θϋ-Υ.GetPosition());}Vector3 в(IMyTerminalBlock Υ,Vector3 ΔΓ){return Vector3D.TransformNormal(ΔΓ,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 Ĕ(Vector3 ǰ,Vector3 и,Vector3 ΔΓ){MatrixD Γγ=MatrixD.CreateFromDir(ǰ,и);return Vector3D.TransformNormal(ΔΓ,MatrixD.Transpose(Γγ));
}Vector3 Βτ(Vector3 ǰ,Vector3 и,Vector3 А){MatrixD Γγ=MatrixD.CreateFromDir(ǰ,и);return Vector3D.Transform(А,Γγ);}String Е(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Д(String s){String[]ξ=s.Split('|');return new Vector3(float.Parse(Μ(ξ,0)),float.Parse(Μ(ξ,1)),float.Parse(Μ(ξ,2)));
}String ΗΥ(ά Ω){String Ж=":";String Ϋ=Е(Ω.Ε)+Ж+Е(Ω.γ)+Ж+
Е(Ω.β)+Ж+Е(Ω.ϋ)+Ж+Е(Ω.π);for(int i=0;i<Ω.ǥ.Length;i++)
Ϋ+=Ж+Math.Round(Ω.ǥ[i],3);return Ϋ;}ά ΗΒ(String s){String[]ξ=s.Split(':');ά Ω=new ά(
Д(Μ(ξ,0)),Д(Μ(ξ,1)),
Д(Μ(ξ,2)),Д(Μ(ξ,3)),
Д(Μ(ξ,4)));int Α=5;List<float>ΕΧ=new List<float>();while(Α<ξ.Length){String Αϊ=Μ(ξ,Α);float f=0;if(!float.TryParse(Αϊ,out f)) break;ΕΧ.Add(f);Α++;}Ω.ǥ=ΕΧ.ToArray();return Ω;}System.Text.StringBuilder ř=new System.Text.StringBuilder();void Σ<T>(T value,bool ΘΑ){if(ΘΑ) ř.Append('\n');ř.Append(value);}
void Σ<T>(T value){Σ(value,true);}String ư(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ǉ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Μ(String[]ΗΫ,int Α){String Αϊ=ΗΫ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Αϊ)) return "";
return Αϊ;}void Save(){if(Αξ||Π==Ν.ц){Storage="";return;}Storage=ΑЅ();}String ΑЅ(){ř.Clear();ř.Append(DATAREV+";");Σ(Е(Αν),false);Σ(Е(Ο.γ));Σ(Е(Ο.ϋ));Σ(Е(Ο.β));Σ(Е(Ο.π));Σ(Е(Ο.Ε));Σ(Е(Ο.ш));Σ(Ο.ε);Σ(Е(Ζ.Ε));Σ(Е(Ζ.π));Σ(Е(Ζ.γ));Σ(Е(Ζ.β));Σ(Е(Ζ.ϋ));Σ(Е(Ζ.ш));Σ(Ζ.ε);Σ(Е(ǟ));Σ(Е(Ȣ));Σ(";");
Σ((int) Π,false);Σ((int) α);Σ((int) ь);Σ(Ǳ);Σ(э);Σ(Ю);Σ(ю);Σ(ǩ);Σ(Ȩ);Σ(ȧ);if(Π==Ν.ΰ){Σ((int) ό.З);Σ(ό.ǘ);Σ(ό.ā);Σ(ư(ό.Ǚ));Σ(ư(ό.ǜ));Σ((int) ύ.З);Σ(ύ.ǘ);Σ(ύ.ā);Σ(ư(ύ.Ǚ));Σ(ư(ύ.ǜ));}else{Σ((int) Ǵ);Σ((int) т);Σ((int) Х);Σ((int) ъ);Σ((int) Ə);Σ(ċ);Σ(ħ);Σ(Ŋ);Σ(ń);Σ(р);Σ(п);Σ(Э);Σ(ŧ);Σ(Ŧ);Σ(ī);Σ(ă);
Σ(Њ);Σ(Л);Σ(ψ);Σ(φ);Σ(Ƞ);Σ(Ъ);Σ(ĥ);Σ(ġ);}Σ(";");for(int i=0;i<ȋ.Count;i++)
Σ((i>0?"|":"")+ȋ[i],false);Σ(";");for(int i=0;i<Φ.Count;i++)
Σ(ΗΥ(Φ[i]),i>0);Σ(";",false);Σ(ы+"|"+Ǡ,false);String Χ=ř.ToString();ř.Clear();return Χ;}ч ΖІ(){return ǀ(Storage);}ч ǀ(String Χ){if(Χ=="") return ч.ΖЁ;String[]ś=Χ.Split(';');if(Μ(ś,0)!=DATAREV) return ч.ΒΨ;int Α=0;try{String[]Κ=Μ(ś,1).Split('\n');Αν=Д(Μ(Κ,Α++));Ο.γ=Д(Μ(Κ,Α++));
Ο.ϋ=Д(Μ(Κ,Α++));Ο.β=Д(Μ(Κ,Α++));Ο.π=Д(Μ(Κ,Α++));Ο.Ε=Д(Μ(Κ,Α++));Ο.ш=Д(Μ(Κ,Α++));Ο.ε=bool.Parse(Μ(Κ,Α++));Ζ.Ε=Д(Μ(Κ,Α++));Ζ.π=Д(Μ(Κ,Α++));Ζ.γ=Д(Μ(Κ,Α++));Ζ.β=Д(Μ(Κ,Α++));Ζ.ϋ=Д(Μ(Κ,Α++));Ζ.ш=Д(Μ(Κ,Α++));Ζ.ε=bool.Parse(Μ(Κ,Α++));ǟ=Д(Μ(Κ,Α++));Ȣ=Д(Μ(Κ,Α++));Κ=Μ(ś,2).Split('\n');Α=0;
Π=(Ν) int.Parse(Μ(Κ,Α++));α=(Ξ) int.Parse(Μ(Κ,Α++));ь=(Ξ) int.Parse(Μ(Κ,Α++));Ǳ=int.Parse(Μ(Κ,Α++));э=int.Parse(Μ(Κ,Α++));Ю=int.Parse(Μ(Κ,Α++));ю=int.Parse(Μ(Κ,Α++));ǩ=float.Parse(Μ(Κ,Α++));Ȩ=bool.Parse(Μ(Κ,Α++));ȧ=bool.Parse(Μ(Κ,Α++));if(Π==Ν.ΰ){ό.З=(η) int.Parse(Μ(Κ,Α++));
ό.ǘ=float.Parse(Μ(Κ,Α++));ό.ā=float.Parse(Μ(Κ,Α++));ό.Ǚ=ǉ(Μ(Κ,Α++));ό.ǜ=ǉ(Μ(Κ,Α++));ύ.З=(η) int.Parse(Μ(Κ,Α++));ύ.ǘ=float.Parse(Μ(Κ,Α++));ύ.ā=float.Parse(Μ(Κ,Α++));ύ.Ǚ=ǉ(Μ(Κ,Α++));ύ.ǜ=ǉ(Μ(Κ,Α++));}else{Ǵ=(ǔ) int.Parse(Μ(Κ,Α++));т=(к) int.Parse(Μ(Κ,Α++));Х=(Ѕ) int.Parse(Μ(Κ,Α++));
ъ=(є) int.Parse(Μ(Κ,Α++));Ə=(ǔ) int.Parse(Μ(Κ,Α++));ċ=bool.Parse(Μ(Κ,Α++));ħ=bool.Parse(Μ(Κ,Α++));Ŋ=bool.Parse(Μ(Κ,Α++));ń=bool.Parse(Μ(Κ,Α++));р=int.Parse(Μ(Κ,Α++));п=int.Parse(Μ(Κ,Α++));Э=int.Parse(Μ(Κ,Α++));ŧ=float.Parse(Μ(Κ,Α++));Ŧ=float.Parse(Μ(Κ,Α++));ī=float.Parse(Μ(Κ,Α++));
ă=float.Parse(Μ(Κ,Α++));Њ=int.Parse(Μ(Κ,Α++));Л=int.Parse(Μ(Κ,Α++));ψ=int.Parse(Μ(Κ,Α++));φ=int.Parse(Μ(Κ,Α++));Ƞ=int.Parse(Μ(Κ,Α++));Ъ=int.Parse(Μ(Κ,Α++));ĥ=int.Parse(Μ(Κ,Α++));ġ=float.Parse(Μ(Κ,Α++));}Κ=Μ(ś,3).Replace("\n","").Split('|');ȋ=Κ.ToList();Κ=Μ(ś,4).Split('\n');
Φ.Clear();if(Κ.Count()>=1&&Κ[0]!="")
for(int i=0;i<Κ.Length;i++)
Φ.Add(ΗΒ(Μ(Κ,i)));Κ=Μ(ś,5).Split('|');ы=Μ(Κ,0);if(!int.TryParse(Μ(Κ,1),out Ǡ)) Ǡ=-1;}catch{return ч.ΔΤ;}Ś=ь;χ();return ч.ǆ;}const int ΔΘ=64000;IMyTerminalBlock ΔΥ(){List<IMyTerminalBlock>Ł=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(Ł,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ł.Count>0?Ł[0]:null;}void Ζη(){IMyTerminalBlock Υ=ΔΥ();if(Υ==null){Λ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΑЅ();if(Χ.Length>ΔΘ){Λ="Export too large: "+Χ.Length+" chars, max "+ΔΘ;return;}Υ.CustomData=Χ;Λ="Exported "+Φ.Count+" waypoints";}void Ζχ(bool ΓΥ){IMyTerminalBlock Υ=ΔΥ();if(Υ==null){Λ="Import: name a block "+dataTag;return;}
Ζψ(Υ.CustomData,ΓΥ);}void Ζψ(String Χ,bool ΓΥ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int Εύ=Χ.IndexOf('\n');Χ=Εύ<0?"":Χ.Substring(Εύ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}χ();String ΔЍ=ΑЅ();ч Ϋ=ǀ(Χ);if(Ϋ==ч.ǆ&&ΓΥ){ά Ō=new ά(Ζ);
Vector3 ΗА=ǟ,ΗЏ=Ȣ;int w=р,h=п,d=Э;ǔ Γώ=Ǵ;є ΒА=ъ;float ΘΖ=ī,ΘΕ=ă;ǀ(ΔЍ);Ζ=Ō;ǟ=ΗА;Ȣ=ΗЏ;р=w;п=h;Э=d;Ǵ=Γώ;ъ=ΒА;ī=ΘΖ;ă=ΘΕ;α=Ξ.ķ;Ÿ(true,true);α=Ξ.ķ;ь=Ξ.ķ;Λ="Imported job, run CONT";return;}if(Ϋ==ч.ǆ){ƕ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}ǀ(ΔЍ);Λ=Ϋ==ч.ΒΨ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Δϊ(){ě.Clear();for(int i=М.Count-1;i>=0;i--){String Χ=М[i].CustomData.ToUpper();bool ΓΣ=false;if(Χ=="INSTRUCTIONS"){ΓΣ=true;ΑΞ=true;}if(Χ=="DEBUG") ΓΣ=true;if(ΓΣ){ě.Add(М[i]);М.RemoveAt(i);}
}ē(ě,false,1,false);}void Δϋ(List<IMyTerminalBlock>Ł){Ƕ.Clear();for(int i=0;i<Ł.Count;i++){IMyTerminalBlock Υ=Ł[i];try{String ΕЃ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Υ.CustomName.IndexOf(ΕЃ);int ş=-1;if(Α<0||!int.TryParse(Υ.CustomName.Substring(Α+ΕЃ.Length,1),out ş)) continue;
if(ş==-1) continue;ş--;IMyTextSurfaceProvider ΕЅ=(IMyTextSurfaceProvider) Υ;if(ş<ΕЅ.SurfaceCount&&ş>=0)
Ƕ.Add(ΕЅ.GetSurface(ş));}catch{}}}String Αζ="";void Δτ(){String Γβ="";String Ɔ="";Αζ=ΔΗ(false);Γβ+=Αζ;Ɔ+=Αζ;Ɔ+=Ζξ();for(int i=0;i<М.Count;i++) М[i].WriteText(Γβ);for(int i=0;i<Ƕ.Count;i++) Ƕ[i].WriteText(Γβ);Echo(Ɔ);for(int i=0;i<ě.Count;i++){IMyTextPanel е=ě[i];
String Χ=е.CustomData.ToUpper();if(Χ=="DEBUG") е.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") е.WriteText(Δά());}}string Δά(){String Γ="";try{float Ετ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ŀ*Ετ)+" / "+(int)(Ő*Ετ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ŀ*100f,1)+"% / "+Math.Round(Ő*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ž,2)+"ms / "+Math.Round(Γη,2)+"ms \n";}catch{}for(int i=0;i<ȣ.Count;i++)
Γ+=""+ȣ.Keys.ElementAt(i)+" = "+Math.Round(ȣ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȣ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Ζφ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ē(М,setLCDFontAndSize,0.9f,false);ē(Ƕ,setLCDFontAndSize,0.9f,true);for(int i=0;i<М.Count;i++) М[i].WriteText(Γ+ȟ.ToString());for(int i=0;i<Ƕ.Count;i++) Ƕ[i].WriteText(Γ+ȟ.ToString());}void Ȁ(){if(!ΑΞ) return;try{Γτ=Runtime.CurrentInstructionCount;}catch{}
}void Ǽ(String έ){if(!ΑΞ) return;if(Γτ==0) return;try{float Αω=(Runtime.CurrentInstructionCount-Γτ)/Runtime.MaxInstructionCount*100;if(!ȣ.ContainsKey(έ))
ȣ.Add(έ,new float[]{Αω,Αω});else{ȣ[έ][0]=Αω;ȣ[έ][1]=Math.Max(Αω,ȣ[έ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖΥ(String ǯ){if(!ł) return;String Ж=""+BROADCAST_SEP;String ν=VERSION+Ж;ν+=ũ(Ş)+Ж;
ν+=(int) Π+Ж;ν+=ĳ(ζ.X)+""+Ж;ν+=ĳ(ζ.Y)+Ж;ν+=ĳ(ζ.Z)+Ж;if(Π!=Ν.ΰ)
ν+=ΒΒ(ь)+(ь==Ξ.В?" "+Math.Round(Ǝ,1)+"%":"")+Ж;else ν+=ΒΒ(ь)+Ж;ν+=ũ(Λ)+Ж;ν+=Ё+""+Ж;ν+=ũ(Αζ)+Ж;ν+=Ǭ+Ж;ν+=ǲ+Ж;for(int i=0;i<Ǜ.Count;i++){if(Ǜ[i].Ŏ!=О.ĵ) continue;ν+=ũ(Ǜ[i].έ)+"/"+ũ(Ǜ[i].ο)+"/"+Ǜ[i].ώ+Ж;}ΖЎ(ν,ǯ);}String ũ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΖЎ(String ν,String ǯ){Βι(ν,BROADCAST_SELF_ID,ǯ);}void Βι(String ν,String λ,String ǯ){try{if(!ł) return;ν=BROADCAST_TAG+BROADCAST_SEP+ΔΨ()+BROADCAST_SEP+λ+BROADCAST_SEP+ΒІ+BROADCAST_SEP+ǯ+BROADCAST_SEP+ν;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ν);}catch(Exception e){ȟ=e;}
}String ΔΨ(){if(Π!=Ν.ц) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΒΟ(String ν){return ν.StartsWith(BROADCAST_TAG);}bool Щ(ref String Χ,out string ƈ,Char Ж){int Α=Χ.IndexOf(Ж);ƈ="";if(Α<0) return false;ƈ=Χ.Substring(0,Α);Χ=Χ.Remove(0,Α+1);return true;}bool Δσ(ref String ν,out String ǳ,out String ǯ)
{ǳ="";ǯ="";if(!ł) return false;String Ǥ="";if(!Щ(ref ν,out Ǥ,BROADCAST_SEP)||!ΒΟ(Ǥ)) return false;if(!Щ(ref ν,out ǳ,BROADCAST_SEP)) return false;if(!Щ(ref ν,out Ǥ,BROADCAST_SEP)||(Ǥ!=ΔΨ()&&(Ǥ!="*"&&Π!=Ν.ц))) return false;if(!Щ(ref ν,out Ǥ,BROADCAST_SEP)||(Ǥ!=ΒІ)) return false;
if(!Щ(ref ν,out ǯ,BROADCAST_SEP)) return false;return true;}void Δζ(){bool Ɓ=true;if(ΕΩ){ΕΩ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ɗ=(Π!=Ν.ц?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɗ)
Δη(Ɗ);String[]Κ=Me.CustomData.Split('\n');ł=bool.Parse(ƿ(Ʊ(Κ,"Enable_Broadcast",ref Ɓ)));bool ΔΑ=false;bool ΕΡ=true;if(ł){if(Π!=Ν.ц)
Ş=ƿ(Ʊ(Κ,"Ship_Name",ref Ɓ)).Replace(BROADCAST_SELF_ID,"");ΒІ=ƿ(Ʊ(Κ,"Broadcast_Channel",ref Ɓ)).ToLower();ΕΡ=false;if(Ǖ==null){Ǖ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǖ.SetMessageCallback("");}List<IMyRadioAntenna>Ľ=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(Ľ);bool Βϊ=false;
for(int i=0;i<Ľ.Count;i++)
if(Ľ[i].EnableBroadcasting&&Ľ[i].Enabled){Βϊ=true;break;}if(Ľ.Count==0) Λ="No Antenna found";else if(!Βϊ) Λ="Antenna not ready";ΔΑ=Ľ.Count==0||!Βϊ;if(ΔЇ&&!ΔΑ&&Π!=Ν.ц) Λ="Antenna ok";}else if(Π==Ν.ц)
Λ="Offline - Enable in PB custom data";ΔЇ=ΔΑ;if(ΕΡ){if(Ǖ!=null) this.IGC.DisableBroadcastListener(Ǖ);Ǖ=null;}}catch{Ɓ=false;}if(!Ɓ){Λ="Reset custom data";Δη(Ɗ);}}void Δη(String Ɗ){String Χ=Ɗ+"\n\n"+"Enable_Broadcast="+(Π==Ν.ц?"true":"false")+" \n";Χ+=Π!=Ν.ц?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String ƿ(String Ġ){int Α=Ġ.IndexOf("//");if(Α!=-1) Ġ=Ġ.Substring(0,Α);String[]ξ=Ġ.Split('=');if(ξ.Length<=1) return "";return ξ[1].Trim();}String Ʊ(String[]Κ,String ΓΦ,ref bool ģ){foreach(String Ġ in Κ)
if(Ġ.StartsWith(ΓΦ)) return Ġ;ģ=false;return "";}void Ζα(string К){if(Ț) У=new List<Г>();String ǳ="";String ǯ="";if(!ΒΟ(К)) ΖЄ(К);bool ΕЉ=false;int Γκ=50;while(Ǖ!=null&&Ǖ.HasPendingMessage&&Γκ-->0){MyIGCMessage ν=Ǖ.AcceptMessage();String Χ=ν.Data as String;if(Χ==null) continue;
if(Δσ(ref Χ,out ǳ,out ǯ)&&ǳ!=""&&ǳ!=BROADCAST_SELF_ID){Г Δ=ΑА(ǳ,"");if(Δ==null){Δ=new Г(ǳ);У.Add(Δ);}Δ.ĸ(ǯ,false,false,true);ΗΟ(Δ,Χ);ΕЉ=true;}}if(ΕЉ)
У.Sort(delegate(Г a,Г b){if(a.έ==null&&b.έ==null) return 0;else if(a.έ==null) return-1;else if(b.έ==null) return 1;else return a.έ.CompareTo(b.έ);});if(Ȟ||Ț){if(ƙ<=0||Ț){Λ="";Ț=false;ΖЍ();Δζ();ΓΫ=DateTime.Now;}}ΗΘ();}void ΗΟ(Г Δ,String Χ){Δ.ΓΨ=DateTime.Now;String ƈ="";
Щ(ref Χ,out Δ.Αό,BROADCAST_SEP);Щ(ref Χ,out Δ.έ,BROADCAST_SEP);if(Δ.Αό!=VERSION) return;String Ɣ="";String[]Ŗ=new string[3];Щ(ref Χ,out Ɣ,BROADCAST_SEP);Щ(ref Χ,out Ŗ[0],BROADCAST_SEP);Щ(ref Χ,out Ŗ[1],BROADCAST_SEP);Щ(ref Χ,out Ŗ[2],BROADCAST_SEP);Щ(ref Χ,out Δ.ΓϏ,BROADCAST_SEP);
Щ(ref Χ,out Δ.Λ,BROADCAST_SEP);Щ(ref Χ,out ƈ,BROADCAST_SEP);Щ(ref Χ,out Δ.Αη,BROADCAST_SEP);String ΖΝ="",Εχ="";Щ(ref Χ,out ΖΝ,BROADCAST_SEP);Щ(ref Χ,out Εχ,BROADCAST_SEP);Δ.Ѐ.Clear();while(true){String Εζ;if(!Щ(ref Χ,out Εζ,BROADCAST_SEP)) break;String[]Αέ=Εζ.Split('/');if(Αέ.Count()<3) continue;
int ώ=0;if(!int.TryParse(Αέ[2],out ώ)) continue;Δ.Ѐ.Add(new Ь(Αέ[0],Αέ[1],ώ,О.ĵ));}int Εϊ=0;if(!int.TryParse(ƈ,out Δ.Ё)) Δ.Ё=0;if(!int.TryParse(Ɣ,out Εϊ)) Δ.Π=Ν.ȯ;if(!float.TryParse(Ŗ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ŗ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(Ŗ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(ΖΝ,out Δ.Ǭ)) Δ.Ǭ=0;if(!float.TryParse(Εχ,out Δ.ǲ)) Δ.ǲ=0;Δ.Π=(Ν) Εϊ;Δ.ΓΒ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΓЋ=0;for(int i=0;i<Δ.Ѐ.Count();i++) Δ.ΓЋ+=Δ.Ѐ[i].ώ;}Г ΑА(String Р,String έ){Р=Р.ToUpper();έ=έ.ToUpper();for(int i=0;i<У.Count;i++)
{if(Р!=""&&У[i].Р.ToUpper()==Р) return У[i];if(έ!=""&&У[i].έ!=null&&У[i].έ.ToUpper()==έ) return У[i];}return null;}void Ǔ(Г Δ,String Ѝ){if(Δ==null){for(int i=0;i<У.Count;i++)
У[i].ĸ(Ѝ,true,false,false);Βι(Ѝ,"*",Ѝ);}else{Δ.ĸ(Ѝ,true,false,false);Βι(Ѝ,Δ.Р,Ѝ);}}void ΖЄ(string К){if(К=="") return;var ξ=К.ToUpper().Split(' ');ξ.DefaultIfEmpty("");var Ѝ=ξ.ElementAtOrDefault(0);var ȁ=ξ.ElementAtOrDefault(1);String ȉ="Invalid argument: "+К;if(τ==Ρ.Ŷ){if(Ћ!=null)
{switch(Ѝ){case "UP":{if(Ё<2) break;if(Ћ.Ё==0){Ё=1;return;}Ǔ(Ћ,"UP");return;}case "DOWN":{if(Ё<1) break;if(Ё==1){Ǔ(Ћ,"MRES");break;}Ǔ(Ћ,"DOWN");break;}case "APPLY":{if(Ё<2) break;Ǔ(Ћ,"APPLY");return;}}}}switch(Ѝ){case "UP":this.ΒΥ(false);return;case "DOWN":this.ΒΤ(false);return;
case "APPLY":this.İ(true);return;}switch(Ѝ){case "CLEAR":Ζά();return;case "SENDALL":
if(String.IsNullOrEmpty(ȁ)){Λ="Usage: SENDALL <cmd>";return;}Ǔ(null,ȁ);return;case "SEND":
if(К.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΖЏ(К.Remove(0,"SEND".Length+1));return;}Λ=ȉ;}void ΖЏ(String Ż){if(Ż=="") return;var ξ=Ż.Split(':');if(ξ.Length!=2){Λ="Missing separator \":\"";return;}ξ.DefaultIfEmpty("");String Ş=ξ.ElementAtOrDefault(0).Trim();
Г Δ=ΑА("",Ş);if(Δ!=null) Ǔ(Δ,ξ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ş;}void Ζά(){У.Clear();Ћ=null;δ(Ρ.Main);Ё=0;}void ΖЍ(){List<IMyTerminalBlock>Ɵ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(М,ѕ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ɵ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);М=ǐ(М,pamTag,true);Δϋ(Ɵ);Δϊ();ē(М,setLCDFontAndSize,1.15f,false);ē(Ƕ,setLCDFontAndSize,1.15f,true);String ΗϏ=Δέ(false);String Εώ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock е in М){if(е.CustomData==""){е.CustomData=Δέ(true);continue;}
if(!е.CustomData.Contains(ΗϏ))
if(!е.CustomData.Contains(Εώ)) е.CustomData=Εώ+е.CustomData;}}void ΗΘ(){String Ɔ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ƕ.Count;i++)
Ƕ[i].WriteText(ΔΖ(Й.Δν,"0",false));for(int i=0;i<М.Count;i++){Й Ђ=Й.ȯ;String ǡ="";ΖЂ(М[i],out Ђ,out ǡ);М[i].WriteText(ΔΖ(Ђ,ǡ,i==0));}Echo(Ɔ);for(int i=0;i<ě.Count;i++){IMyTextPanel е=ě[i];String Χ=е.CustomData.ToUpper();if(Χ=="DEBUG") е.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") е.WriteText(Δά());
}}String İ(bool Θ){int Ητ=0;return İ(Θ,0,ref Ητ,false,1);}String İ(bool Θ,int ΘΘ,ref int ΘΧ,bool ΕΕ,int Ɩ){String Γ="";String ŝ="——————————————————\n";String Θε="--------------------------------------------\n";if(Ћ==null||τ==Ρ.Main||τ==Ρ.Βκ)
Γ+="[PAM]-Controller | "+У.Count+" Connected ships"+"\n";else Γ+=Δί(Ћ)+"\n";Γ+=ŝ;int Α=0;int Η=Ё;Ģ=0;if(Λ!=""){Ι(ref Γ,Η,Α++,Θ,Λ);}else if(τ==Ρ.Main){if(Ћ==null&&У.Count>=1) Ћ=У[0];String Џ="";if(Ι(ref Џ,Η,Α++,Θ," Send command to all")) δ(Ρ.Βκ);int Ŝ=0;
for(int s=0;s<У.Count;s++){if(ΕΕ) Џ+="\n";Г Δ=У[s];if(Η==Α||Η==Α+1) Ћ=Δ;if(Α==Η) Ŝ=Џ.Split('\n').Length-1;if(Ι(ref Џ,Η,Α++,Θ," "+Δί(Δ))){Ћ=Δ;δ(Ρ.Ŷ);Ћ.ĸ("",false,true,false);}if(Α==Η) Ŝ=Џ.Split('\n').Length-1;if(Ι(ref Џ,Η,Α++,Θ," "+(ΕΕ?"":"  ")+Ζσ(Δ)))
{Ћ=Δ;δ(Ρ.Δό);}}int ť=Ɩ-2;ť+=Math.Max(0,(ΘΘ-1))*Ɩ;Γ+=Βθ(ť,Џ,Ŝ,ref ΘΧ);}else if(τ==Ρ.ǃ||τ==Ρ.Βκ){Ρ Ηά=τ==Ρ.ǃ?Ρ.Ŷ:Ρ.Main;Г λ=null;if(τ==Ρ.ǃ) λ=Ћ;if(Ι(ref Γ,Η,Α++,Θ," Back")){δ(Ηά);}if(Ι(ref Γ,Η,Α++,Θ," [Stop] "+Ĵ(λ,"STOP"))){Ǔ(λ,"STOP");}if(Ι(ref Γ,Η,Α++,Θ," [Continue] "+Ĵ(λ,"CONT"))){Ǔ(λ,"CONT");}
if(Ι(ref Γ,Η,Α++,Θ," [Move home] "+Ĵ(λ,"HOMEPOS"))){Ǔ(λ,"HOMEPOS");}if(Ι(ref Γ,Η,Α++,Θ," [Move to job] "+Ĵ(λ,"JOBPOS"))){Ǔ(λ,"JOBPOS");}if(Ι(ref Γ,Η,Α++,Θ," [Full simulation] "+Ĵ(λ,"FULL"))){Ǔ(λ,"FULL");}if(λ!=null&&λ.Π==Ν.ΰ)
if(Ι(ref Γ,Η,Α++,Θ," [Undock] "+Ĵ(λ,"UNDOCK"))){Ǔ(λ,"UNDOCK");}}else if(τ==Ρ.Ŷ){String ΒЁ="";if(Ι(ref ΒЁ,Η,Α++,Θ," Back")){δ(Ρ.Main);}Γ+=ΒЁ.Substring(0,ΒЁ.Length-1).PadRight(36,' ');if(Ι(ref Γ,Η,Α++,Θ," Send cmd")){δ(Ρ.ǃ);}if(!Ћ.Δμ()&&!Ћ.ΒΞ()) Ģ++;if(Ћ.ΒΞ()) Γ+="No answer received...";
else Γ+=ĺ(Ћ,Ћ.Αη);}else if(τ==Ρ.Δό){if(Ι(ref Γ,Η,Α++,Θ," Back")){δ(Ρ.Main);}Γ+=Θε;Γ+=ĺ(Ћ,Δή(Ћ));}if(!Γι) Ё=Math.Min(Ģ-1,Ё);return Γ;}String Δί(Г Δ){if(Δ.Αό!=VERSION) return Δ.έ+": Different version!";return Δ.έ+": "+ĺ(Δ,Δ.ΓϏ)+" "+Δ.ΓΒ+"m";}String Ζσ(Г Δ){String s=Βε("",Ј(Δ.Ǭ,Δ.ǲ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ѐ.Count;i++){if(i>=5) break;Ь υ=Δ.Ѐ[i];s+=ΔЂ(ΑІ(Ζο(υ)),3)+" "+ΔΦ(υ.ώ)+" ";}return s;}String Ζο(Ь υ){if(υ.ο.ToUpper()=="ORE"||υ.ο.ToUpper()=="INGOT"){String ΕΓ=Ζν(υ.έ.ToUpper());if(ΕΓ!="") return ΕΓ;}return υ.έ;}String ĺ(Г Δ,String Γ){if(Δ.Δμ()) return "No signal...("+ƴ((int)(DateTime.Now-Δ.ΓΨ).TotalSeconds)+")";
return Γ;}String Δέ(bool Θϊ){String Γ="";if(Θϊ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΖЂ(IMyTerminalBlock е,out Й Ђ,out String ǡ){bool ģ=true;String Ɣ=ƿ(Ʊ(е.CustomData.Split('\n'),"mode",ref ģ)).ToUpper();String[]ξ=Ɣ.Split(':');String Ɠ=ξ.Length>0?ξ[0].Trim():"";ǡ=ξ.Length>1?ξ[1].Trim():"";
Ђ=Й.ȯ;if(Ɠ=="MAIN") Ђ=Й.Δξ;else if(Ɠ=="MAINX") Ђ=Й.Δο;else if(Ɠ=="MENU") Ђ=Й.ΒΣ;else if(Ɠ=="INVENTORY") Ђ=Й.ΒΝ;else if(Ɠ=="DEBUG") Ђ=Й.ΑЊ;if(Ђ==Й.ȯ) ǡ=Ɣ;}String ΔΖ(Й Ђ,String ǡ,bool Ηυ){int Ƒ=15;if(Ηυ){ΕΗ=ΑΗ;ΑΗ=0;ΕΘ=ΑΘ;ΑΘ=0;}String Γ="";if(Ђ==Й.Δξ){int д=0;
Γ=İ(false,ΕΗ,ref Ηη,true,Ƒ);if(!int.TryParse(ǡ,out д)) return Γ;ΑΗ=Math.Max(д,ΑΗ);д--;return Δς(д,Ƒ,Γ);}else if(Ђ==Й.Δο){int д=0;Γ=İ(false,ΕΘ,ref Ηθ,false,Ƒ);if(!int.TryParse(ǡ,out д)) return Γ;ΑΘ=Math.Max(д,ΑΘ);д--;return Δς(д,Ƒ,Γ);}if(Ђ==Й.Δν){return İ(false,0,ref Ηι,true,Ƒ);
}else if(Ђ==Й.ΒΝ||Ђ==Й.ΒΣ||Ђ==Й.ΑЊ){Г Δ=Ћ;if(ǡ!=""){Δ=ΑА("",ǡ);if(Δ==null) return "Unknown ship: "+ǡ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ŝ="—————————————————————\n";String ΓЉ="";String ΒЅ="";if(Ђ==Й.ΒΣ){ΓЉ="Menu";ΒЅ=ĺ(Δ,Δ.Αη);}
else if(Ђ==Й.ΒΝ){ΓЉ="Inventory";ΒЅ=ĺ(Δ,Δή(Δ));}else if(Ђ==Й.ΑЊ){return ĺ(Δ,Ζτ(Δ));}Γ+=Δ.έ+" - "+Δ.ΓΒ+"m | "+ΑІ(""+ΓЉ)+"\n"+ŝ;Γ+=ΒЅ;return Γ;}return "Unknown command: "+ǡ;}string Ζτ(Г Δ){String s="";s+=Δ.Αό+"\n";s+=Δ.έ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΓϏ+"\n";s+=Δ.Λ+"\n";s+=Δ.Ё+"\n";s+=Δ.Αη.Length+"\n";s+=Δ.Ǭ+"\n";s+=Δ.ǲ+"\n";s+=Δ.Ѐ.Count()+"\n";return s;}string Δή(Г Δ){String s="";s+=Βε("All",Ј(Δ.Ǭ,Δ.ǲ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ѐ.Count();i++){Ь υ=Δ.Ѐ[i];
s+=Βε(υ.έ,υ.ώ,Δ.ΓЋ,30,8,10)+"\n";}return s;}И Ζκ(Г Δ,bool ȅ,string Ѝ){И Ϋ=И.None;if(Δ==null){for(int i=0;i<У.Count;i++){И ω=У[i].ĸ(Ѝ,false,ȅ,false);if(ω==И.Βζ) Ϋ=ω;if(ω==И.ΒΩ) return ω;if(ω==И.Ŵ) return ω;}return Ϋ;}else return Δ.ĸ(Ѝ,false,ȅ,false);}String Ĵ(Г Δ,String Ѝ)
{И ω=Ζκ(Δ,false,Ѝ);if(ω==И.Βζ) return "received!";if(ω==И.Ŵ) return "pending...";if(ω==И.ΒΩ) return "no answer!";return "";}String Βθ(int ť,String Γ,int Ŝ,ref int Ȥ){String[]Κ=Γ.Split('\n');if(Ŝ>=Ȥ+ť-1) Ȥ++;Ȥ=Math.Min(Κ.Count()-1-ť,Ȥ);if(Ŝ<Ȥ+1) Ȥ--;Ȥ=Math.Max(0,Ȥ);
String Ϋ="";for(int i=0;i<ť;i++){int Εξ=i+Ȥ;if(Εξ>=Κ.Count()) break;Ϋ+=Κ[Εξ]+"\n";}return Ϋ;}String Δς(int д,int Ɩ,String Γ){String[]Κ=Γ.Split('\n');int Γώ=д*Ɩ;int Ņ=(д+1)*Ɩ;String Ϋ="";for(int i=Γώ;i<Ņ;i++){if(i>=Κ.Count()) break;Ϋ+=Κ[i]+"\n";}return Ϋ;
}String ΑІ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΔЂ(String s,int Ευ){if(s=="") return s;if(s.Length>Ευ) s=s.Substring(0,Ευ-1)+".";return s;}string Βε(String ğ,float value,float Γδ,int ΔЎ,int Ει,int ΘΗ){float Ĥ=Ј(value,Γδ)*ΔЎ;
String ΑΕ="[";for(int i=0;i<ΔЎ;i++){if(i<=Ĥ) ΑΕ+="|";else ΑΕ+="'";}ΑΕ+="]";return ΑΕ+" "+ΔЂ(ΑІ(ğ),Ει).PadRight(Ει)+"".PadRight(ΘΗ)+ΔΦ(value);}String ΔΦ(float ώ){if(ώ>=1000000) return Math.Round(ώ/1000000f,ώ/1000000f<100?1:0)+"M";if(ώ>=1000) return Math.Round(ώ/1000f,ώ/1000f<100?1:0)+"K";
return ""+Math.Round(ώ);}String ƴ(int ƚ){if(ƚ>=60*60) return Math.Round(ƚ/(60f*60f),1)+" h";if(ƚ>=60) return Math.Round(ƚ/60f,1)+" min";return ""+ƚ+" s";}string ĳ(float v){return Math.Round(v,2)+" ";}string ΘЁ(Vector3 v){return "X"+ĳ(v.X)+"Y"+ĳ(v.Y)+"Z"+ĳ(v.Z);}
List<IMyCameraBlock>ȃ=new List<IMyCameraBlock>();List<long>œ=new List<long>();bool ŕ=false;int ΒЋ=0;int ΒЉ=0;DateTime ΔА=DateTime.Now;String ΒЄ="";void ΖЌ(){ȃ.Clear();œ.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(ȃ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȃ.Count;i++){ȃ[i].EnableRaycast=true;if(!œ.Contains(ȃ[i].CubeGrid.EntityId)) œ.Add(ȃ[i].CubeGrid.EntityId);}if(!œ.Contains(Me.CubeGrid.EntityId)) œ.Add(Me.CubeGrid.EntityId);}bool Δκ(Β s){return s==Β.Ȑ||s==Β.б||s==Β.Ǒ||
s==Β.ȭ||s==Β.Ĳ||s==Β.Ŭ||s==Β.Ʋ;}void Ʈ(){ŕ=false;ΒЋ=0;ΒЄ="";}void ΗΝ(){if(!obstacleBrake||ȃ.Count==0||!Δκ(Ϊ)){Ʈ();return;}Vector3 Ɖ=Ѓ+Αψ;Vector3 Ƿ=Ɖ-ζ;float ǵ=Ƿ.Length();if(ǵ<1f){Ʈ();return;}Vector3 μ=Ƿ/ǵ;float Ες=ȥ*obstacleLookAhead+κ;Vector3 ΘΞ=Ɖ+μ*(κ*0.25f);
for(int n=0;n<ȃ.Count;n++){ΒЉ=(ΒЉ+1)%ȃ.Count;IMyCameraBlock ƀ=ȃ[ΒЉ];if(!ƀ.IsFunctional||!ƀ.IsWorking) continue;Vector3 ΕΑ=ƀ.GetPosition();Vector3 Ņ=ΘΞ;if(Vector3.Distance(ΕΑ,Ņ)>Ες) Ņ=ΕΑ+μ*Ες;if(!ƀ.CanScan(Ņ)) continue;MyDetectedEntityInfo Ƌ=ƀ.Raycast(Ņ);bool ΘΔ=!Ƌ.IsEmpty()&&!œ.Contains(Ƌ.EntityId)&&!Ζώ(Ƌ.EntityId)&&
Ƌ.Type!=MyDetectedEntityType.FloatingObject;if(ΘΔ){if(!ŕ) ΔА=DateTime.Now;ŕ=true;ΒЋ=0;ΒЄ=Ƌ.Type.ToString();}else if(ŕ&&++ΒЋ>=3) Ʈ();break;}if(ŕ&&(DateTime.Now-ΔА).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΒЄ;}const String ΔЁ="[PAMTFC]";const float ΗΓ=5f;const float Ζθ=2f;class ŷ{public long Р;public Vector3 Є,ΓБ;public float ΕΠ;public String Ō="";public int Ш=-1;public DateTime ΓІ;}IMyBroadcastListener Αχ=null;long Γμ=0;List<ŷ>з=new List<ŷ>();Vector3 Αψ=new Vector3();
float Ţ=-1;int ΖΚ=0;String ы="";int Ǡ=-1;bool ΑΚ=false;Vector3 ΕΙ=new Vector3();String ΒΠ(){if(α==Ξ.н||Π==Ν.ΰ) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Њ+"|"+Л+"|"+(int) Ə+"|"+Math.Round(ȍ,1)+"|"+Math.Round(ǹ,1);}void ΗΡ(){if(!trafficEnabled) return;if(Αχ==null) Αχ=IGC.RegisterBroadcastListener(ΔЁ);Γμ=Me.CubeGrid.EntityId;int Ηΰ=30;while(Αχ.HasPendingMessage&&Ηΰ-->0){String Χ=Αχ.AcceptMessage().Data as String;
if(Χ!=null) ΖЃ(Χ);}for(int i=з.Count-1;i>=0;i--)
if((DateTime.Now-з[i].ΓІ).TotalSeconds>ΗΓ) з.RemoveAt(i);if(Ϊ==Β.м||++ΖΚ<3) return;ΖΚ=0;bool ΗЁ=Ϊ==Β.Ȑ||Ϊ==Β.Ķ||Ϊ==Β.Ȕ;IGC.SendBroadcastMessage(ΔЁ,Γμ+";"+Е(ζ)+";"+
Е(Ψ.GetShipVelocities().LinearVelocity)+";"+κ+";"+ΒΠ()+";"+
(ΗЁ?Ǡ:-1)+";"+ы);}void ΖЃ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long Р=long.Parse(f[0]);if(Р==Γμ) return;ŷ Δ=null;for(int i=0;i<з.Count;i++) if(з[i].Р==Р) Δ=з[i];if(Δ==null){Δ=new ŷ();Δ.Р=Р;з.Add(Δ);}Δ.Є=Д(f[1]);Δ.ΓБ=Д(f[2]);Δ.ΕΠ=float.Parse(f[3]);
Δ.Ō=f[4];Δ.Ш=int.Parse(f[5]);Δ.ΓІ=DateTime.Now;if(Δ.Ō!=""&&Δ.Ō==ΒΠ()) ΖЀ(f[6]);}catch{}}bool Ζώ(long Р){for(int i=0;i<з.Count;i++) if(з[i].Р==Р) return true;return false;}Vector3 Ζπ(int Α,int ΘΙ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();
ά Ω=Φ[Α];Vector3 Θς=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*ΘΙ;Vector3 и=Ω.π.LengthSquared()>0.01f?-Vector3.Normalize(Ω.π):-Ω.β;Vector3 ё=Vector3.Cross(Θς,и);if(ё.LengthSquared()<0.0001f) return new Vector3();float Ϗ=laneOffset*κ;float Ηχ=Math.Min(Vector3.Distance(Ω.Ε,Φ[0].Ε),
Vector3.Distance(Ω.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(ё)*Ϗ*Math.Min(1,Ηχ/(Ϗ*4));}void ΗΣ(){Αψ=new Vector3();Ţ=-1;if(!trafficEnabled||з.Count==0||!Δκ(Ϊ)) return;Ζέ(Ψ.GetNaturalGravity());}void Ζέ(Vector3 π){Vector3 Ƿ=Ѓ-ζ;float ǵ=Ƿ.Length();if(ǵ<1f) return;Vector3 μ=Ƿ/ǵ;
Vector3 и=π.LengthSquared()>0.01f?-Vector3.Normalize(π):-Ƅ;Vector3 ё=Vector3.Cross(μ,и);ё=ё.LengthSquared()<0.0001f?-Ɛ:Vector3.Normalize(ё);for(int i=0;i<з.Count;i++){ŷ Ч=з[i];Vector3 Є=Ч.Є+Ч.ΓБ*(float)(DateTime.Now-Ч.ΓІ).TotalSeconds;Vector3 ΕІ=Є-ζ;float ΑΒ=Vector3.Dot(ΕІ,μ);
if(ΑΒ<=0) continue;float ȅ=(κ+Ч.ΕΠ)*0.5f;if((ΕІ-μ*ΑΒ).Length()>ȅ*1.2f) continue;float Γο=Vector3.Dot(Ч.ΓБ,μ);if(Γο<-1f){if(ΑΒ<(ȥ-Γο)*6f+ȅ*4f) Αψ=ё*ȅ*1.3f;}else{float Ηω=ΑΒ-ȅ-followGap*κ;float Εν=Math.Max(0,Γο)+Math.Max(0,Ηω)/Ζθ;if(Ţ<0||Εν<Ţ) Ţ=Εν;
}}}bool ΔΣ(){if(ы.Length==Math.Max(0,Л)) return false;ы=new String('0',Math.Max(0,Л));Ǡ=-1;return true;}void ΖЀ(String Ч){ΔΣ();if(Ч.Length!=ы.Length) return;char[]ŗ=ы.ToCharArray();for(int i=0;i<ŗ.Length;i++) if(Ч[i]=='1') ŗ[i]='1';ы=new String(ŗ);}bool Δλ(int Ш){return Ш>=0&&Ш<ы.Length&&ы[Ш]=='1';}
void Δω(int Ш){if(Ш<0||Ш>=ы.Length) return;char[]ŗ=ы.ToCharArray();ŗ[Ш]='1';ы=new String(ŗ);}bool Ζύ(int Ш){String ΓΦ=ΒΠ();for(int i=0;i<з.Count;i++)
if(з[i].Ō==ΓΦ&&з[i].Ш>=0&&Math.Abs(з[i].Ш-Ш)<=1) return true;return false;}int ΖΫ(int Α){if(Њ<1||Л<1) return Α;if(ΔΣ()) for(int r=0;r<Α/Њ&&r<Л;r++) Δω(r);if(Ǡ>=0&&Α>=(Ǡ+1)*Њ){Δω(Ǡ);Ǡ=-1;}int Ш=Α/Њ;if(Ш==Ǡ&&!Δλ(Ш)) return Α;bool ΔЈ=false;for(int r=0;r<Л;r++)
{if(Δλ(r)) continue;ΔЈ=true;if(trafficEnabled&&Ζύ(r)) continue;Ǡ=r;return r==Ш?Α:r*Њ;}return ΔЈ?-1:Њ*Л;}