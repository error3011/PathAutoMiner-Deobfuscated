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
public enum Θ{Ʊ,Š,ќ,м,Ϋ}public enum Α{в,ȁ,ħ,Ȅ,Ć,С,Ы,
Ȝ,Ħ,Ǭ,ĥ,ģ,Ā,
ƨ,ǵ,ȇ,ă,Ƴ,ƚ,
ΒΪ,Ʋ,Ř,ƛ,Ȟ}public enum Λ{г,Ĩ,Љ,Αω,Ɩ,Ƣ,ƣ,Ġ,ġ}public enum л{None,ă,в,Γώ}public enum Ω{Main,ΔΛ,ş,Ş,ΒΦ,Ɣ,
Αλ,ΓЍ,ΓЎ,ΓЌ,Ζβ,ΘΤ,
ΘΡ,ΘΣ,ΒΠ,ΔΠ,Ƭ}public enum Ю{ƪ,Ŗ,Ζδ,ΓА,Ϋ}public enum ў{Ʈ,Γψ}public enum б{Ʃ,Αό,ΔΡ,Ś}public enum Ϗ{ΑБ,Γϋ,Αφ,Αϊ,Ƙ,ΑЉ,ƞ}public enum ъ{Αχ,Απ,Αρ}public enum Е{ś,ΖΖ,ā}public enum ε{ΒΒ,ΒΖ,ΒΗ,ΔΘ,ΔΗ,
ΒΑ,Ʀ,ƥ,ΒΔ,
ΒΕ,ΒΓ}public enum έ{Ǯ,ȃ,ǭ,Ɠ,Αξ,Αο}public enum Ȇ{ΑϏ,Αν,ΕЈ}public enum н{ƭ,ΓЂ,ΖΡ,ΑА}public enum ǯ{None,ΒΛ,ΖΥ,ΖΠ}public enum π{Ť,ƒ,Ơ,Ś,ΑЎ,ơ,ΑЏ,Ζθ,ΒΘ}public enum ΘΛ{Ʊ,ΘΝ,ΘΜ,ΘΟ,ΘΙ,ΘΘ,ΘΞ}class Χ{public bool β=false;public Vector3 Δ=new Vector3();
public Vector3 ΰ=new Vector3();public Vector3 α=new Vector3();public Vector3 χ=new Vector3();public Vector3 κ=new Vector3();public Vector3 У=new Vector3();public float Џ=0;public float Θδ=0;public float[]ǚ=null;public Χ(){}public Χ(Χ Ѐ){β=Ѐ.β;Δ=Ѐ.Δ;ΰ=Ѐ.ΰ;α=Ѐ.α;χ=Ѐ.χ;κ=Ѐ.κ;
У=Ѐ.У;ǚ=Ѐ.ǚ;}public Χ(Vector3 Δ,Vector3 α,Vector3 ΰ,Vector3 χ,Vector3 κ){this.Δ=Δ;this.ΰ=ΰ;this.α=α;this.χ=χ;this.Џ=0;this.κ=κ;}public void Εφ(List<IMyThrust>ƶ,List<string>Ǽ){ǚ=new float[Ǽ.Count];for(int i=0;i<ǚ.Length;i++)
ǚ[i]=-1;for(int i=0;i<ƶ.Count;i++){string щ=ΓЊ(ƶ[i]);int Β=Ǽ.IndexOf(щ);if(Β!=-1)
ǚ[Β]=Ќ(ƶ[i].MaxEffectiveThrust,ƶ[i].MaxThrust);}}}class ΒΚ{public Χ Л=null;public List<Vector3>Ŏ=new List<Vector3>();public float ŭ=0;public float Γλ=0;public float į=0;public float Βω=0;public Vector3 ǉ=new Vector3();}class ī{public ε Ћ=ε.ΒΒ;public float ǎ=0;public float ȣ=0;
public string Ǐ="";public string ǒ="";DateTime ΔЋ;public bool ΓΡ=false;private bool Βπ=false;public bool Ασ(bool Ǘ){if(Ǘ){ȣ=0;Βπ=false;return false;}Βπ=true;return ȣ>ǎ;}public void ΖΦ(){Ασ(true);ΓΡ=false;}public void Ζς(){if(Βπ)
if((DateTime.Now-ΔЋ).TotalSeconds>1){ȣ++;ΔЋ=DateTime.Now;}}public bool ΑЇ(){switch(Ћ){case ε.ΒΖ:return true;case ε.ΒΗ:return true;}return false;}}class ы{public String ς="";public String ξ="";public int џ=0;public Е ŀ=Е.ā;public ы(String ς,String ξ,int џ,Е ŀ){this.ς=ς;this.ξ=ξ;
this.џ=џ;this.ŀ=ŀ;}}class ΕЃ{public ΕЃ(Vector3 ζ,float ό){this.ζ=ζ;this.ό=ό;}public Vector3 ζ;public float ό;}class ΒΟ{public DateTime ΗΠ;public DateTime Δθ;public String ύ="";public String ς="";public String Θζ="";public String Ξ="";public String Θβ="";public String Θί="";public String ΓΟ="";
public Vector3 Δ=new Vector3();public List<ы>Х=new List<ы>();public Θ Κ=Θ.Ʊ;public ǯ Ƞ;public float Ń;public float Ĵ;public int ђ;public int Θέ=0;public int Θγ=0;public bool ΘΚ(){return(DateTime.Now-ΗΠ).TotalSeconds>10;}public bool ΖΙ(){if(Ƞ!=ǯ.ΒΛ) return false;return(DateTime.Now-Δθ).TotalSeconds>=4;
}public ΒΟ(String ύ){this.ύ=ύ;}public ǯ ΘΠ(String ď,bool Ησ,bool Ď,bool ΖЁ){if(ď==""&&!Ď) return ǯ.None;if(Ƞ==ǯ.ΒΛ&&ΖΙ())
Ƞ=ǯ.ΖΠ;if(Ησ){ΓΟ=ď;Ƞ=ǯ.ΒΛ;Δθ=DateTime.Now;return Ƞ;}else if(Ď){ΓΟ="";Ƞ=ǯ.None;}else if(ΓΟ==ď){if(ΖЁ) Ƞ=ǯ.ΖΥ;return Ƞ;}return ǯ.None;}}IMyGridTerminalSystem ψ;Vector3 ΑΪ=new Vector3();Vector3 γ=new Vector3();Vector3 ǌ=new Vector3();Vector3 Ż=new Vector3();Vector3 ů=new Vector3();
DateTime ΔЊ=new DateTime();bool Ų=true;int Γγ=0;bool ȫ=false;bool ΑΘ=false;bool ž=true;bool Ες=false;bool Ľ=false;bool ΑΕ=false;float ȕ=0;float Ě=0;int Ȱ=0;int ƍ=0;int Αέ=0;int Βι=0;float ł=0;float İ=0;double ΓΕ=0;float ŧ=0;List<int>ĺ=new List<int>();List<int>Ŋ=new List<int>();
Θ Κ=Θ.Ʊ;int ώ=0;float Ǿ=0;float ǫ=0;float θ=0;IMyRemoteControl Υ;IMySensorBlock λ;List<IMyTimerBlock>Γζ=new List<IMyTimerBlock>();List<IMyShipConnector>п=new List<IMyShipConnector>();List<IMyThrust>ǽ=new List<IMyThrust>();List<IMyGyro>ΑΑ=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>ǻ=new List<IMyLandingGear>();List<IMyReactor>ȯ=new List<IMyReactor>();List<IMyConveyorSorter>Αΰ=new List<IMyConveyorSorter>();List<IMyGasTank>я=new List<IMyGasTank>();List<IMyTerminalBlock>ť=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ĳ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ċ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȟ=new List<IMyBatteryBlock>();List<IMyTextPanel>ѐ=new List<IMyTextPanel>();List<IMyTextSurface>ȱ=new List<IMyTextSurface>();List<IMyTextPanel>Ŭ=new List<IMyTextPanel>();IMyTerminalBlock є=null;Χ Ζ=new Χ();Χ ΒЃ=new Χ();
Χ ΒІ=new Χ();bool ǿ=false;bool ő=false;bool Œ=false;bool ΑΗ=false;List<String>Ǽ=new List<string>();List<Χ>Τ=new List<Χ>();List<Χ>Ƒ=new List<Χ>();int Βυ=0;double Εσ=0;Χ Ε=new Χ();Vector3 Ǖ;Vector3 Ȓ;Λ ή=Λ.г;ў Ź=ў.Ʈ;int Ѓ=0;int Ў=0;double Ÿ=0;bool ƿ=false;ī ϊ=new ī();
ī ϋ=new ī();б и=б.Ʃ;bool Ȳ=true;ў ǧ=ў.Ʈ;int е=3;int д=3;int Т=30;ъ р=ъ.Αχ;bool Ș=true;bool ļ=true;bool Ķ=true;bool ě=false;Ϗ И=Ϗ.ΑБ;bool ȗ=true;float ǥ=90;float у=20;float Ц=5;float ф=20;float ǟ=0.70f;float Ŕ=1.50f;float œ=2.50f;float ğ=0f;float Ȧ=0f;Α Φ;
Λ т;Α Ƈ;ΒΚ η=null;int φ=0;int ρ=0;int М=0;int Ȑ=0;int Ę=30;int ѓ=0;bool Є=true;Vector3 х;double З=0;bool Αά=false;int Γπ=0;int Ѕ=0;int Βα=0;int ȭ=0;int ΕΞ=0;Vector3 ƀ=new Vector3();float ж=0;float Ш=0;int Ğ=0;Χ Αα=null;float ΒЊ=0;float ƹ=0;float Н=0;float ė=0;
bool ΒЄ=false;bool Ĝ=false;bool Ǝ=false;bool ȍ=false;bool Ȉ=false;DateTime ΖЏ=new DateTime();Χ Đ=null;Χ Δλ=null;float Ƕ=0;л Ǡ;float Ȏ=0;float ĝ=0;String ΒБ="";float ΑΙ=0;float Ĵ=0;float Ń=0;List<ы>Ǒ=new List<ы>();float Й=0;bool ƅ=false;bool Ει=false;bool ΕΪ=false;
bool Αγ=false;bool ΕΫ=false;float ΓΓ=0;float ΓΞ=0;Vector3 Δϋ=new Vector3();Vector3 ω=new Vector3();Vector3 Αδ=new Vector3(1,1,1);float Γβ=1;Vector3 Ɗ=new Vector3();bool ΑΖ=false;bool ΒϏ=false;bool ΒЎ=false;bool ΔΩ=false;float ƽ=0;float Ƶ=2;Vector3 Εθ;Vector3 Γα;Vector3 Γΰ;
float[,]Ǜ=new float[3,2];Dictionary<String,float[,]>ј=new Dictionary<string,float[,]>();Dictionary<String,float>Επ=new Dictionary<String,float>();int Βδ=0;Dictionary<String,float[]>ȓ=new Dictionary<string,float[]>();float ΓΣ;bool ǀ=false;int Ƹ=0;int Ȩ=0;int ΑΓ=0;int Ĕ=0;
String Ξ="";int[]ΕΖ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΕΗ=false;int ΑΣ=0;int ђ=0;Ω Ѝ=Ω.Main;int ΗЁ=0,ΗЂ=0,ΗЃ=0,ΗЄ=0;int Ηο=0,Ηπ=0;bool Ĳ=false;bool ΔΪ=false;String ΓΨ="";String Βθ="";IMyBroadcastListener ȉ=null;bool Δϊ=true;List<ΒΟ>ΘΥ=null;ΒΟ Θΰ=null;
int ΘΦ=0,ΘΧ=0;int ΘΨ=0,ΘΩ=0;int ΘΪ=0,ΘΫ=0,Θά=0;String ΗΟ="";Exception ȏ=null;bool ΑΫ=false;Λ Ō=Λ.г;bool Δξ=false;bool ΔЉ=true;bool ΓΧ=false;String Ερ="";bool ƌ=false;bool Γέ=true;int ΑΔ=0;int[]і=null;Vector3 Βκ=new Vector3();bool Δύ=false;bool ѝ(IMyTerminalBlock Σ)=>Σ.CubeGrid==Me.CubeGrid;
Program(){ψ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΑЈ(Me,controllerTag,true))
Κ=Θ.м;μ(Ω.Main);if(Κ!=Θ.м){Ξ="Welcome to [PAM]!";н ΓΦ=ΖΧ();if(Κ==Θ.Ʊ){List<IMyShipDrill>Δφ=new List<IMyShipDrill>();List<IMyShipGrinder>Δώ=new List<IMyShipGrinder>();ψ.GetBlocksOfType(Δφ,q=>q.CubeGrid==Me.CubeGrid);ψ.GetBlocksOfType(Δώ,q=>q.CubeGrid==Me.CubeGrid);if(Δφ.Count>0)
{Κ=Θ.Š;Ξ="Miner mode enabled!";}else if(Δώ.Count>0){Κ=Θ.ќ;Ξ="Grinder mode enabled!";}else{Κ=Θ.Ϋ;ǳ(Ю.Ϋ);}}if(ΓΦ==н.ƭ) ž=false;if(ΓΦ==н.ΓЂ) Ξ="Data restore failed!";if(ΓΦ==н.ΑА) Ξ="New version, wipe data";}}void Main(string э,UpdateType ΗЌ){try{if(ȏ!=null){if(э.Trim().ToUpper()=="RETRY")
{ȏ=null;Ų=true;Ȱ=0;if(Κ!=Θ.м) σ();Ξ="Restarted after error";}else{ΖΓ();ΔΙ();return;}}ΑΘ=(ΗЌ&UpdateType.Update10)!=0;if(ΑΘ) Γγ++;ȫ=Γγ>=10;if(ȫ) Γγ=0;if(ΑΘ){Ƹ++;if(Ƹ>4) Ƹ=0;Αέ=(int) Math.Max(0,10-(DateTime.Now-ΔЊ).TotalSeconds);}if(э!="") ΗΟ=э;if(Κ!=Θ.м)
Ζγ(э);else
ΕЁ(э);ΕΗ=false;try{int Δμ=Runtime.CurrentInstructionCount;float Ɓ=Ќ(Δμ,Runtime.MaxInstructionCount);if(Ɓ>0.90) Ξ="Max. instructions >90%";if(Ɓ>ł) ł=Ɓ;if(ǀ){ĺ.Add(Δμ);while(ĺ.Count>10) ĺ.RemoveAt(0);İ=0;for(int i=0;i<ĺ.Count;i++) İ+=ĺ[i];İ=Ќ(Ќ(İ,ĺ.Count),Runtime.MaxInstructionCount);
double ΒЋ=Runtime.LastRunTimeMs;if(ΑΕ&&ΒЋ>ΓΕ) ΓΕ=ΒЋ;Ŋ.Add((int)(ΒЋ*1000f));while(Ŋ.Count>10) Ŋ.RemoveAt(0);ŧ=0;for(int i=0;i<Ŋ.Count;i++) ŧ+=Ŋ[i];ŧ=Ќ(ŧ,Ŋ.Count)/1000f;}}catch{ł=0;}}catch(Exception e){ȏ=e;}}void Ζγ(string э){bool Αή=false;String Δε="";if(ώ<=1&&!ΔΒ(э))
ΔΚ(э);int Ηά=50;while(ȉ!=null&&ȉ.HasPendingMessage&&Ηά-->0){MyIGCMessage ν=ȉ.AcceptMessage();String ί=ν.Data as String;if(ί==null) continue;String Ƅ="";if(ώ<=1&&ΖΣ(ref ί,out Ƅ,out Δε)&&Ƅ==BROADCAST_SELF_ID){ΔΚ(ί);Αή=true;break;}}bool ΔЅ=ΑΕ&&Φ==Α.в&&!Ľ&&!Αή&&Ȱ==0&&!ΑΗ;
if(ȫ&&Φ!=Α.в) Αή=true;if((ΑΘ&&!ΔЅ)||(ȫ&&ΔЅ)){if(Ȱ==0&&(Αέ<=0||Ų)){Ες=ώ>0;ώ=0;Ȱ=1;Ǵ();ΖΪ();ΖΗ();ǰ("Scan 1");}else if(Ȱ==1){Ȱ=2;Ǵ();ΖΫ();ǰ("Scan 2");}else if(Ȱ==2){Ȱ=0;Ǵ();Ζά();Ζέ();if(ώ<=1) ΔΔ();ǰ("Scan 3");ΔЊ=DateTime.Now;if(ώ<=1&&ž)
ΑΪ=ΑЁ(Υ,Υ.CenterOfMass);ž=false;if(Ų){Ų=false;bool ΗА=ή==Λ.Љ;σ();if(ΔЉ&&resumeAfterRestart&&ΗА&&ώ<=1) ΓΧ=true;}ΔЉ=false;if(Ες&&ώ==0) Ξ="Setup complete";}else{if(ή==Λ.Љ&&Κ!=Θ.Ϋ){Ǵ();Ετ();ǰ("Inv balance");}Ǵ();switch(ƍ){case 0:Ζλ();break;case 1:Ζξ();break;case 2:Ζκ();break;
case 3:Ζυ();break;case 4:Ζν();break;case 5:Εω();break;case 6:Εψ(Υ);break;}ǰ("Update: "+ƍ);ƍ++;if(ƍ>6){ƍ=0;ΑΕ=true;if(Ō!=Λ.г){switch(Ō){case Λ.Ġ:Ț();break;case Λ.ġ:Ț();break;case Λ.Љ:Ț();break;case Λ.Ƣ:Ɯ();break;case Λ.ƣ:Ą();break;}Ō=Λ.г;}}}if(!Ų){if(!ǲ(Υ,true)){Υ=null;
ž=true;ώ=2;}if(ώ>=2&&Φ!=Α.в)
σ();if(ώ<=1){Ě=Υ.CalculateShipMass().PhysicalMass;ȕ=(float) Υ.GetShipSpeed();γ=Ζι(Υ,ΑΪ);ǌ=Υ.WorldMatrix.Forward;Ż=Υ.WorldMatrix.Left;ů=Υ.WorldMatrix.Down;Ζρ();Ζσ();if(ΓΧ){ΓΧ=false;Ț();if(Φ!=Α.в) Ξ="Job resumed after restart";}if(Φ!=Α.в){Ľ=false;ΔΞ(false);Γφ(false);String Ηω=ΑЃ(Φ)+" "+(int) Φ;
Ǵ();Ζο();ΒΩ(false);ǰ(Ηω);Ζτ();Ζπ();Ǵ();Ζμ();ǰ("Thruster");Ǵ();ΔΦ();ǰ("Gyroscope");}else{if(Ľ){if(Ɵ()){Щ(ů,ǌ,Ż,0.25f,true);ΔΦ();Ξ="Aligning to planet: "+Math.Round(ƽ-0.25f,2)+"°";if(ΑΖ) ΑІ(true,true);}else ΑІ(true,true);}}ƌ=false;}}}Ǵ();ΔΙ();ǰ("Print");if(Αή||Βι<=0){Ǵ();Ευ(Δε);
ǰ("Broadcast");Βι=4;}else if(ȫ) Βι--;}void ΔΚ(string э){if(э=="") return;var Г=э.ToUpper().Split(' ');Г.DefaultIfEmpty("");var ď=Г.ElementAtOrDefault(0);var Č=Г.ElementAtOrDefault(1);var Ŧ=Г.ElementAtOrDefault(2);var Δή=Г.ElementAtOrDefault(3);String ȩ="Invalid argument: "+э;bool Γι=false;
switch(ď){case "UP":this.ΔΖ(false);break;case "DOWN":this.ΔΕ(false);break;case "UPLOOP":this.ΔΖ(true);break;case "DOWNLOOP":this.ΔΕ(true);break;case "APPLY":this.Γτ(true);break;case "MRES":ђ=0;break;case "STOP":this.σ();break;case "PATHHOME":{this.σ();this.ΒΥ();}break;case "PATH":{this.σ();this.ΒΥ();Ζ.β=true;}break;
case "START":{this.σ();ΒΤ();}break;case "ALIGN":{ΑІ(!Ľ,false);}break;case "CONT":{this.σ();this.Ț();}break;case "JOBPOS":{this.σ();this.Ą();}break;case "HOMEPOS":{this.σ();this.Ɯ();}break;case "FULL":{ƅ=true;}break;case "RESET":{ΑΫ=true;ώ=2;}break;case "EXPORT":{ΕЉ();}break;
case "IMPORT":{ΖΔ(Č=="JOB");}break;default:Γι=true;break;}if(Κ!=Θ.Ϋ){switch(ď){case "SHUTTLE":{ΕІ();}break;case "CFGS":{if(!ΕϏ(Č,Ŧ,Δή)) Ξ=ȩ;}break;case "CFGB":{if(!Εώ(Č,Ŧ)) Ξ=ȩ;}break;case "CFGL":{if(!ą(ref ǥ,true,π.Ơ,Č,"")||!ΕЀ(Ŧ))
Ξ=ȩ;}break;case "CFGE":{if(!ą(ref Ц,true,π.ơ,Č,"IG")||
!ą(ref у,true,π.ΑЎ,Ŧ,"IG")||
!ą(ref ф,true,π.ΑЏ,Δή,"IG"))
Ξ=ȩ;}break;case "CFGA":{if(!ą(ref ǟ,false,π.ƒ,Č,"")) Ξ=ȩ;}break;case "CFGW":{if(!ą(ref Ŕ,false,π.Ť,Č,"")||
!ą(ref œ,false,π.Ť,Ŧ,""))
Ξ=ȩ;}break;case "NEXT":{Ακ(false);}break;case "PREV":{Ακ(true);}break;default:if(Γι) Ξ=ȩ;break;}}else{switch(ď){case "UNDOCK":{ƌ=true;}break;default:if(Γι) Ξ=ȩ;break;}}}String ΕЎ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Κ!=Θ.Ϋ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΕІ(){σ();Κ=Θ.Ϋ;ǳ(Ю.Ϋ);Ε.β=false;Ζ.β=false;λ=null;ο.Clear();ή=Λ.г;}void ΑІ(bool Ї,bool Ǣ){if(!Ї) Ξ="Aligning canceled";if(Ǣ) Ξ="Aligning done";if(Ǣ||!Ї){Ľ=false;ΔΣ();ƫ(false,0,0,0,0);return;}if(Ɵ()) Ľ=true;}bool ΕϏ(String ΘΑ,String ΗΘ,String ΖЊ)
{bool ΗЎ=ή==Λ.Љ;int w,h,d;if(int.TryParse(ΘΑ,out w)&&int.TryParse(ΗΘ,out h)&&int.TryParse(ΖЊ,out d)){this.σ();е=w;д=h;Т=d;ĉ(false);ţ(false,false);if(ΗЎ) Ț();return true;}return false;}bool ΕЀ(String Δέ){if(Δέ=="ON"){Ș=true;return true;}if(Δέ=="OFF"){Ș=false;return true;}
return false;}bool Εώ(String Δυ,String ƾ){bool ſ=true;if(Δυ=="HOME") Ȳ=true;else if(Δυ=="STOP") Ȳ=false;else ſ=false;if(ƾ=="HOME") и=б.Ʃ;else if(ƾ=="STOP") и=б.ΔΡ;else if(ƾ=="JOB") и=б.Αό;else if(ƾ=="IG") и=б.Ś;else ſ=false;return ſ;}void μ(Ω ΓΜ){ΕΖ[(int) Ѝ]=ђ;ђ=ΕΖ[(int) ΓΜ];
if(ΓΜ==Ω.Ζβ) ђ=0;Ѝ=ΓΜ;if(Κ!=Θ.м)
Ĉ(Ѝ==Ω.ş,false,0,0);ΕΗ=true;}void ΔΖ(bool ΒЏ){if(ђ>0) ђ--;else if(ΒЏ) ђ=ΑΣ-1;}void ΔΕ(bool ΒЏ){if(ђ<ΑΣ-1) ђ++;else if(ΒЏ) ђ=0;}String ǳ(Ю ν){switch(ν){case Ю.ƪ:Ξ="Job is running";break;case Ю.Ŗ:Ξ="Connector not ready!";break;case Ю.Ζδ:Ξ="Ship modified, path outdated!";break;
case Ю.ΓА:Ξ="Interrupted by player!";break;case Ю.Ϋ:Ξ="Shuttle mode enabled!";break;}return "";}String ΖΒ(ў ζ){switch(ζ){case ў.Ʈ:return "Top-Left";case ў.Γψ:return "Center";default:return "";}}String ΕЌ(ъ Ž){switch(Ž){case ъ.Απ:return "Auto"+(Κ==Θ.Š?" (Ore)":"");case ъ.Αρ:return "Auto (+Stone)";
case ъ.Αχ:return "Default";default:return "";}}String ΑЀ(Λ П){switch(П){case Λ.г:return "No job";case Λ.Ĩ:return "Job paused";case Λ.Љ:return "Job active";case Λ.Ġ:return "Job active";case Λ.ġ:return "Job active";case Λ.Αω:return "Job done";case Λ.Ɩ:return "Job changed";case Λ.Ƣ:return "Move home";
case Λ.ƣ:return "Move to job";}return "";}String ΓІ(б ΖϏ){switch(ΖϏ){case б.Ʃ:return "Return home";case б.Αό:return "Fly to job pos";case б.ΔΡ:return "Stop";case б.Ś:return "Ignore";}return "";}String ΕЍ(Ϗ Ž){switch(Ž){case Ϗ.ΑБ:return "Off";case Ϗ.Αϊ:return "Drop pos (Stone) ";case Ϗ.Ƙ:return "Drop pos (Sto.+Ice)";
case Ϗ.Γϋ:return "Cur. pos (Stone)";case Ϗ.Αφ:return "Cur. pos (Sto.+Ice)";case Ϗ.ΑЉ:return "In motion (Stone)";case Ϗ.ƞ:return "In motion (Sto.+Ice)";}return "";}String ΓЄ(л П){switch(П){case л.None:return "No batteries";case л.ă:return "Charging";case л.Γώ:return "Discharging";}return "";}String ΑЃ(Α П)
{String Θή=Κ==Θ.Ϋ?"target":"job";switch(П){case Α.в:return "Idle";case Α.ȁ:return "Flying to XY position";case Α.ħ:return Κ==Θ.ќ?"Grinding":"Mining";case Α.Ȅ:return "Returning";case Α.Ř:return "Flying to drop pos";case Α.Ć:return "Returning to dock";case Α.С:return "Flying to dock area";
case Α.Ы:return "Flying to job area";case Α.Ȝ:return "Flying to path";case Α.Ħ:return "Flying to job position";case Α.Ǭ:return "Approaching dock";case Α.ĥ:return "Docking";case Α.ģ:return "Aligning";case Α.Ā:return "Aligning";case Α.ƨ:return "Retry docking";case Α.ǵ:return "Unloading";case Α.Ȟ:return Ερ;
case Α.ȇ:return "Undocking";case Α.ă:return "Charging batteries";case Α.Ƴ:return "Waiting for uranium";case Α.ƚ:return "Filling up hydrogen";case Α.ΒΪ:return "Waiting for ejection";case Α.Ʋ:return "Waiting for ejection";case Α.ƛ:return "Flying to drop pos";}return "";}String ΓЋ(ε Ћ){switch(Ћ)
{case ε.ΒΒ:return "On \"Undock\" command";case ε.ΒΖ:return "On player entered cockpit";case ε.ΔΘ:return "On ship is full";case ε.ΔΗ:return "On ship is empty";case ε.ΒΗ:return "On time delay";case ε.Ʀ:return "On batteries empty(<25%)";case ε.ƥ:return "On batteries empty(=0%)";case ε.ΒΑ:return "On batteries full";
case ε.ΒΕ:return "On hydrogen empty(<25%)";case ε.ΒΓ:return "On hydrogen empty(=0%)";case ε.ΒΔ:return "On hydrogen full";}return "";}bool Ο(ref String Γ,int ΕΦ,int ȧ,bool Μ,String ź){ΑΣ+=1;if(ΕΦ==ȧ) ź=">"+ź+(Ƹ>=2?" .":"");else ź=" "+ź;Γ+=ź+"\n";return ΕΦ==ȧ&&Μ;}String Γτ(bool Μ)
{int Β=0;int Ν=ђ;ΑΣ=0;String ΕΩ="———————————————\n";String ŏ="--------------------------------------------\n";String Γ="";Γ+=ΑЀ(ή)+" | "+(Ζ.β?"Ready to dock":"No dock")+"\n";Γ+=ΕΩ;double Ǩ=Math.Max(Math.Round(this.З),0);if(Ѝ==Ω.Main){bool Ȫ=Κ==Θ.Ϋ;if(Ο(ref Γ,Ν,Β++,Μ," Record path & set home")) ΒΥ();
if(Κ==Θ.Š)
if(Ο(ref Γ,Ν,Β++,Μ," Setup mining job")) μ(Ω.ş);if(Κ==Θ.ќ)
if(Ο(ref Γ,Ν,Β++,Μ," Setup grinding job")) μ(Ω.ş);if(Κ==Θ.Ϋ)
if(Ο(ref Γ,Ν,Β++,Μ," Setup shuttle job")) μ(Ω.ΒΠ);if(Ο(ref Γ,Ν,Β++,Μ," Continue job")) Ț();if(Ο(ref Γ,Ν,Β++,Μ," Fly to home position")) Ɯ();if(Ο(ref Γ,Ν,Β++,Μ," Fly to job position")) Ą();if(Ο(ref Γ,Ν,Β++,Μ," Behavior settings"))
if(Ȫ) μ(Ω.Ƭ);else μ(Ω.Ɣ);if(Ο(ref Γ,Ν,Β++,Μ," Info")) μ(Ω.ΓЍ);if(Κ!=Θ.Ϋ)
if(Ο(ref Γ,Ν,Β++,Μ," Help")) μ(Ω.ΓЌ);}else if(Ѝ==Ω.ş){double ΗБ=Math.Round(е*Ǿ,1);double ΗΗ=Math.Round(д*ǫ,1);String Ё="";if(Ο(ref Ё,Ν,Β++,Μ," Start new job!")) ΒΤ();if(Ο(ref Ё,Ν,Β++,Μ," Change current job")){ţ(false,false);μ(Ω.Main);}if(Ο(ref Ё,Ν,Β++,Μ," Width + (Width: "+е+" = "+ΗБ+"m)")){Ģ(ref е,5,20,1);ĉ(true);}
if(Ο(ref Ё,Ν,Β++,Μ," Width -")){Ģ(ref е,-5,20,-1);ĉ(true);}if(Ο(ref Ё,Ν,Β++,Μ," Height + (Height: "+д+" = "+ΗΗ+"m)")){Ģ(ref д,5,20,1);ĉ(true);}if(Ο(ref Ё,Ν,Β++,Μ," Height -")){Ģ(ref д,-5,20,-1);ĉ(true);}if(Ο(ref Ё,Ν,Β++,Μ," Depth + ("+(р==ъ.Αχ?"Depth":"Min")+": "+Т+"m)")){Ģ(ref Т,5,50,2);ĉ(true);}
if(Ο(ref Ё,Ν,Β++,Μ," Depth -")){Ģ(ref Т,-5,50,-2);ĉ(true);}if(Ο(ref Ё,Ν,Β++,Μ," Depth mode: "+ΕЌ(р))){р=ț(р);}if(Ο(ref Ё,Ν,Β++,Μ," Start pos: "+ΖΒ(ǧ))){ǧ=ț(ǧ);}if(Κ==Θ.ќ&&р==ъ.Αρ) р=ț(р);Γ+=ΔΜ(8,Ё,Ν,ref Ηο);}else if(Ѝ==Ω.ΒΠ){float[]Δπ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ο(ref Γ,Ν,Β++,Μ," Next")) μ(Ω.ΔΠ);if(Ο(ref Γ,Ν,Β++,Μ," Back")) μ(Ω.Main);Γ+=" Leave connector 1:\n";if(Ο(ref Γ,Ν,Β++,Μ," - "+ΓЋ(ϊ.Ћ))) ϊ.Ћ=ț(ϊ.Ћ);if(!ϊ.ΑЇ()) Γ+="\n";else if(Ο(ref Γ,Ν,Β++,Μ," - Delay: "+Αώ((int) ϊ.ǎ))) ϊ.ǎ=Γό(ϊ.ǎ,Δπ);Γ+=" Leave connector 2:\n";if(Ο(ref Γ,Ν,Β++,Μ," - "+ΓЋ(ϋ.Ћ))) ϋ.Ћ=ț(ϋ.Ћ);
if(!ϋ.ΑЇ()) Γ+="\n";else if(Ο(ref Γ,Ν,Β++,Μ," - Delay: "+Αώ((int) ϋ.ǎ))) ϋ.ǎ=Γό(ϋ.ǎ,Δπ);}else if(Ѝ==Ω.ΔΠ){if(Ο(ref Γ,Ν,Β++,Μ," Start job!")) ΒΤ();if(Ο(ref Γ,Ν,Β++,Μ," Back")) μ(Ω.ΒΠ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ο(ref Γ,Ν,Β++,Μ," = "+(ϊ.Ǐ!=""?ϊ.Ǐ:"-"))) ϊ.Ǐ=Ƥ(ref ΗЁ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ο(ref Γ,Ν,Β++,Μ," = "+(ϊ.ǒ!=""?ϊ.ǒ:"-"))) ϊ.ǒ=Ƥ(ref ΗЃ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ο(ref Γ,Ν,Β++,Μ," = "+(ϋ.Ǐ!=""?ϋ.Ǐ:"-"))) ϋ.Ǐ=Ƥ(ref ΗЂ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ο(ref Γ,Ν,Β++,Μ," = "+(ϋ.ǒ!=""?ϋ.ǒ:"-"))) ϋ.ǒ=Ƥ(ref ΗЄ);
}else if(Ѝ==Ω.Ş){String ΔА=ǥ+" %";if(ȫ) Ȩ++;if(Ȩ>1){Ȩ=0;ΑΓ++;if(ΑΓ>1) ΑΓ=0;}bool[]ΓΪ=new bool[]{ȯ.Count==0,Ǡ==л.None,я.Count==0};int Εν=0;while(true){Εν++;Ĕ++;if(Ĕ>ΓΪ.Length-1) Ĕ=0;if(Εν>=ΓΪ.Length) break;if(!ΓΪ[Ĕ]) break;}bool Ȫ=Κ==Θ.Ϋ;if(!Ȫ&&Ș&&Й!=-1&&ΑΓ==0)
ΔА=Й<1000000?Math.Round(Й)+" Kg":Math.Round(Й/1000)+" t";if(Ο(ref Γ,Ν,Β++,Μ," Stop!")){σ();μ(Ω.Main);}if(Ο(ref Γ,Ν,Β++,Μ," Behavior settings"))
if(!Ȫ) μ(Ω.Ɣ);else μ(Ω.Ƭ);if(!Ȫ){if(Ο(ref Γ,Ν,Β++,Μ," Next hole")) Ακ(false);}else if(Ο(ref Γ,Ν,Β++,Μ," Undock")) ƌ=true;Γ+=ŏ;if(!Ȫ) Γ+="Progress: "+Math.Round(Ÿ,1)+" %\n";Γ+="State: "+ΑЃ(Φ)+" "+Ǩ+"m \n";Γ+="Load: "+ΑΙ+" % Max: "+ΔА+" \n";if(Ĕ==0) Γ+="Uranium: "+(ȯ.Count==0?"No reactors":Math.Round(ĝ,1)+"Kg "+(Ц==-1?"":" Min: "+Ц+" Kg"))+"\n";
if(Ĕ==1) Γ+="Battery: "+(Ǡ==л.None?ΓЄ(Ǡ):Ƕ+"% "+(у==-1||Ȫ?"":" Min: "+у+" %"))+"\n";if(Ĕ==2) Γ+="Hydrogen: "+(я.Count==0?"No tanks":Math.Round(Ȏ,1)+"% "+(ф==-1||Ȫ?"":" Min: "+ф+" %"))+"\n";}else if(Ѝ==Ω.Ɣ){String Ё="";if(Ο(ref Ё,Ν,Β++,Μ," Back")){if(ή==Λ.Љ) μ(Ω.Ş);else μ(Ω.Main);}
if(Ο(ref Ё,Ν,Β++,Μ," Max load: "+ǥ+"%")) а(ref ǥ,ǥ<=80?-10:-5,π.Ơ,false);if(Ο(ref Ё,Ν,Β++,Μ," Weight limit: "+(Ș?"On":"Off"))) Ș=!Ș;if(Ο(ref Ё,Ν,Β++,Μ," Ejection: "+ΕЍ(И))) И=ț(И);if(Ο(ref Ё,Ν,Β++,Μ," Toggle sorters: "+(ě?"On":"Off"))){ě=!ě;if(ě) Ǟ(Γέ);}
if(Ο(ref Ё,Ν,Β++,Μ," Unload ice: "+(ȗ?"On":"Off"))) ȗ=!ȗ;if(Ο(ref Ё,Ν,Β++,Μ," Uranium: "+(Ц==-1?"Ignore":"Min "+Ц+"Kg"))) а(ref Ц,(Ц>5?-5:-1),π.ơ,true);if(Ο(ref Ё,Ν,Β++,Μ," Battery: "+(у==-1?"Ignore":"Min "+у+"%"))) а(ref у,-5,π.ΑЎ,true);if(Ο(ref Ё,Ν,Β++,Μ," Hydrogen: "+(ф==-1?"Ignore":"Min "+ф+"%"))) а(ref ф,-10,π.ΑЏ,true);
if(Ο(ref Ё,Ν,Β++,Μ," When done: "+(Ȳ?"Return home":"Stop"))) Ȳ=!Ȳ;if(Ο(ref Ё,Ν,Β++,Μ," On damage: "+ΓІ(и))) и=ț(и);if(Ο(ref Ё,Ν,Β++,Μ," Advanced...")) μ(Ω.Αλ);Γ+=ΔΜ(8,Ё,Ν,ref Ηπ);}else if(Ѝ==Ω.Αλ){if(Ο(ref Γ,Ν,Β++,Μ," Back")){if(ή==Λ.Љ) μ(Ω.Ş);else μ(Ω.Main);}
if(Ο(ref Γ,Ν,Β++,Μ,(Κ==Θ.ќ?" Grinder":" Drill")+" inv. balancing: "+(ļ?"On":"Off"))) ļ=!ļ;if(Ο(ref Γ,Ν,Β++,Μ," Enable"+(Κ==Θ.ќ?" grinders":" drills")+": "+(Ķ?"Fwd + Bwd":"Fwd"))) Ķ=!Ķ;if(Ο(ref Γ,Ν,Β++,Μ," Work speed fwd.: "+Ŕ+"m/s")) а(ref Ŕ,0.5f,π.Ť,false);
if(Ο(ref Γ,Ν,Β++,Μ," Work speed bwd.: "+œ+"m/s")) а(ref œ,0.5f,π.Ť,false);if(Ο(ref Γ,Ν,Β++,Μ," Acceleration: "+Math.Round(ǟ*100f)+"%"+(ǟ>0.80f?" (risky)":""))) а(ref ǟ,0.1f,π.ƒ,false);if(Ο(ref Γ,Ν,Β++,Μ," Width overlap: "+ğ*100f+"%")) Γω(true,0.05f);if(Ο(ref Γ,Ν,Β++,Μ," Height overlap: "+Ȧ*100f+"%")) Γω(false,0.05f);
}else if(Ѝ==Ω.Ƭ){if(Ο(ref Γ,Ν,Β++,Μ," Back")){if(ή==Λ.Љ) μ(Ω.Ş);else μ(Ω.Main);}if(Ο(ref Γ,Ν,Β++,Μ," Max load: "+ǥ+"%")) а(ref ǥ,ǥ<=80?-10:-5,π.Ơ,false);if(Ο(ref Γ,Ν,Β++,Μ," Unload ice: "+(ȗ?"On":"Off"))) ȗ=!ȗ;if(Ο(ref Γ,Ν,Β++,Μ," Uranium: "+(Ц==-1?"Ignore":"Min "+Ц+"Kg"))) а(ref Ц,(Ц>5?-5:-1),π.ơ,true);
if(Ο(ref Γ,Ν,Β++,Μ," Battery: "+(у==-1?"Ignore":"Charge up"))) у=(у==-1?1:-1);if(Ο(ref Γ,Ν,Β++,Μ," Hydrogen: "+(ф==-1?"Ignore":"Fill up"))) ф=(ф==-1?1:-1);if(Ο(ref Γ,Ν,Β++,Μ," On damage: "+ΓІ(и))) и=ț(и);if(Ο(ref Γ,Ν,Β++,Μ," Acceleration: "+Math.Round(ǟ*100f)+"%"+(ǟ>0.80f?" (risky)":""))) а(ref ǟ,0.1f,π.ƒ,false);
}else if(Ѝ==Ω.ΔΛ){double ΗΡ=0;if(Τ.Count>0) ΗΡ=Vector3.Distance(Τ.Last().Δ,γ);if(Ο(ref Γ,Ν,Β++,Μ," Stop path recording")) ΔΤ();if(Κ!=Θ.Ϋ){if(Ο(ref Γ,Ν,Β++,Μ," Home: "+(ǿ?"Use old home":(Ζ.β?"Was set! ":"none ")))) ǿ=!ǿ;}else{if(Ο(ref Γ,Ν,Β++,Μ," Connector 1: "+(ǿ?"Use old connector":(Ζ.β?"Was set! ":"none ")))) ǿ=!ǿ;
if(Ο(ref Γ,Ν,Β++,Μ," Connector 2: "+(ő?"Use old connector":(Ε.β?"Was set! ":"none ")))) ő=!ő;}if(Ο(ref Γ,Ν,Β++,Μ," Path: "+(Œ?"Use old path":(Τ.Count>1?"Count: "+Τ.Count:"none ")))) Œ=!Œ;Γ+=ŏ;Γ+="Wp spacing: "+Math.Round(Εσ)+"m\n";}else if(Ѝ==Ω.ΒΦ){if(Ο(ref Γ,Ν,Β++,Μ," Stop")){σ();μ(Ω.Main);}
Γ+=ŏ;Γ+="State: "+ΑЃ(Φ)+" \n";Γ+="Speed: "+Math.Round(ȕ,1)+"m/s\n";Γ+="Target dist: "+Ǩ+"m\n";Γ+="Wp count: "+Τ.Count+"\n";Γ+="Wp left: "+Γπ+"\n";}else if(Ѝ==Ω.ΓЍ){List<IMyTerminalBlock>đ=ΓЇ();if(ȫ) Ȩ++;if(Ȩ>=đ.Count) Ȩ=0;if(Ο(ref Γ,Ν,Β++,Μ," Next")) μ(Ω.ΓЎ);
Γ+=ŏ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ΑΙ,1)+"% "+Math.Round(Ĵ,1)+" / "+Math.Round(Ń,1)+"\n";Γ+="Uranium: "+(ȯ.Count==0?"No reactors":Math.Round(ĝ,1)+"Kg "+ΒБ)+"\n";Γ+="Battery: "+(Ǡ==л.None?"":Ƕ+"% ")+ΓЄ(Ǡ)+"\n";Γ+="Hydrogen: "+(я.Count==0?"No tanks":Math.Round(Ȏ,1)+"% ")+"\n";
Γ+="Damage: "+(đ.Count==0?"None":""+(Ȩ+1)+"/"+đ.Count+" "+đ[Ȩ].CustomName)+"\n";}else if(Ѝ==Ω.ΓЎ){if(Ο(ref Γ,Ν,Β++,Μ," Back")) μ(Ω.Main);Γ+=ŏ;Γ+="Next scan: "+Αέ+"s\n";Γ+="Ship size: "+Math.Round(Ǿ,1)+"m "+Math.Round(ǫ,1)+"m "+Math.Round(θ,1)+"m \n";
Γ+="Broadcast: "+(Ĳ?"Online - "+ΓΨ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ł*100f,1)+"% \n";}else if(Ѝ==Ω.ΓЌ){if(Ο(ref Γ,Ν,Β++,Μ," Back")) μ(Ω.Main);Γ+=ŏ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+Αέ+"s\n";if(ΑΫ) Γ="Recompile script now";int ΗΣ=Γ.Split('\n').Length;for(int i=ΗΣ;i<=10;i++) Γ+="\n";Γ+=ΕΩ;Γ+="Last: "+Ξ+"\n";
return Γ;}void Γω(bool ΗΞ,float ї){σ();ţ(true,false);if(ΗΞ) а(ref ğ,ї,π.ΒΘ,false);else а(ref Ȧ,ї,π.ΒΘ,false);Γχ();Ĉ(true,true,0,0);}float Γό(float δ,float[]Αι){float ά=Αι[0];for(int i=Αι.Length-1;i>=0;i--)
if(δ<Αι[i]) ά=Αι[i];return ά;}String Ƥ(ref int ȧ){String ά="";if(ȧ>=Γζ.Count) ȧ=-1;if(ȧ>=0) ά=Γζ[ȧ].CustomName;ȧ++;return ά;}void Ư(string ς){if(ή!=Λ.Љ) return;if(ς=="") return;IMyTerminalBlock Σ=ψ.GetBlockWithName(ς);if(Σ==null||!(Σ is IMyTimerBlock)){Ξ="Timerblock "+ς+" not found!";
return;}((IMyTimerBlock) Σ).Trigger();}void Ģ(ref int value,int ũ,int Ȗ,int ΓΫ){if(ũ==0) return;if(value<Ȗ&&ΓΫ>0||value<=Ȗ&&ΓΫ<0){value+=ΓΫ;return;}int Ζχ=Math.Abs(ũ);int ΒΫ=0;int ΓΗ=1;while(true){ΒΫ+=ΓΗ*Ζχ*10;if(ũ<0&&value-Ȗ<=ΒΫ) break;if(ũ>0&&value-Ȗ<ΒΫ) break;
ΓΗ++;}value+=ΓΗ*ũ;}void ĉ(bool Θε){е=Math.Max(е,1);д=Math.Max(д,1);Т=Math.Max(Т,0);Ĉ(Ѝ==Ω.ş,false,0,0);}bool ą(ref float value,bool Ημ,π ξ,String ĕ,String ΗΚ){if(ĕ=="") return false;float Ň=-1;bool ΔЃ=false;if(ĕ.ToUpper()==ΗΚ) ΔЃ=true;else if(!float.TryParse(ĕ,out Ň)) return false;
else Ň=Math.Max(0,Ň);if(Ημ) Ň=(float) Math.Round(Ň);а(ref value,Ň,ξ,ΔЃ,false);return true;}void а(ref float value,float ї,π ξ,bool Ƌ){а(ref value,value+ї,ξ,Ƌ,true);}void а(ref float value,float ń,π ξ,bool Ƌ,bool ΘΔ){float ǔ=0,Ǔ=0;if(ξ==π.Ť){ǔ=0.5f;Ǔ=10f;}if(ξ==π.ƒ){ǔ=0.1f;Ǔ=1f;}
if(ξ==π.Ś){ǔ=50f;Ǔ=100f;}if(ξ==π.ΑЎ){ǔ=5f;Ǔ=30f;}if(ξ==π.ơ){ǔ=1f;Ǔ=25f;}if(ξ==π.Ơ){ǔ=10f;Ǔ=95f;}if(ξ==π.ΑЏ){ǔ=10f;Ǔ=90f;}if(ξ==π.Ζθ){ǔ=10f;Ǔ=1800;}if(ξ==π.ΒΘ){ǔ=0.0f;Ǔ=0.75f;}if(ń==-1&&Ƌ){value=-1;return;}if(value==-1) Ƌ=false;
bool ΕΟ=ń<ǔ||ń>Ǔ;if(ΕΟ&&ΘΔ){if(ń<value) value=Ǔ;else if(ń>value) value=ǔ;}else value=ń;if(ΕΟ&&Ƌ) value=-1;else value=Math.Max(ǔ,Math.Min(value,Ǔ));value=(float) Math.Round(value,2);}void Ακ(bool Ηη){if(Ηη) М=Math.Max(0,М-1);else М++;ΒΩ(true);}T ț<T>(T δ){int Β=Array.IndexOf(Enum.GetValues(δ.GetType()),δ);
Β++;if(Β>=ΕЇ(δ)) Β=0;return(T) Enum.GetValues(δ.GetType()).GetValue(Β);}int ΕЇ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒΥ(){Ƒ.Clear();for(int i=0;i<Τ.Count;i++) Ƒ.Add(Τ[i]);Τ.Clear();ΑΗ=true;ΒЃ=new Χ(Ζ);ΒІ=new Χ(Ε);Ζ.β=false;if(Κ==Θ.Ϋ) Ε.β=false;for(int i=0;i<ј.Count;i++)
if(!Ǽ.Contains(ј.Keys.ElementAt(i)))
Ǽ.Add(ј.Keys.ElementAt(i));ǿ=false;ő=false;Œ=false;μ(Ω.ΔΛ);}void ΔΤ(){if(ǿ) Ζ=ΒЃ;if(ő) Ε=ΒІ;if(Œ){Τ.Clear();for(int i=0;i<Ƒ.Count;i++) Τ.Add(Ƒ[i]);}ΑΗ=false;σ();μ(Ω.Main);}void Ζρ(){if(!ΑΗ) return;if(Φ!=Α.в){ΔΤ();return;}if(!ΒЃ.β) ǿ=false;if(!ΒІ.β) ő=false;if(Ƒ.Count<=1) Œ=false;
IMyShipConnector Ж=Ъ(MyShipConnectorStatus.Connectable);if(Ж==null) Ж=Ъ(MyShipConnectorStatus.Connected);if(Ж!=null){if(Math.Round(ȕ,2)<=0.20) Βυ++;else Βυ=0;if(Βυ>=5){if(Κ==Θ.Ϋ&&(Ζ.β||ǿ)&&Vector3.Distance(Ζ.Δ,Ж.GetPosition())>5){Ε.α=Υ.WorldMatrix.Forward;Ε.χ=Υ.WorldMatrix.Left;
Ε.ΰ=Υ.WorldMatrix.Down;Ε.κ=Υ.GetNaturalGravity();Ε.Δ=Ж.GetPosition();Ε.β=true;Ε.У=Ж.Position;}else{Ζ.α=Υ.WorldMatrix.Forward;Ζ.χ=Υ.WorldMatrix.Left;Ζ.ΰ=Υ.WorldMatrix.Down;Ζ.κ=Υ.GetNaturalGravity();Ζ.Δ=Ж.GetPosition();Ζ.β=true;Ζ.У=Ж.Position;}}}double ΒЉ=-1;if(Τ.Count>0) ΒЉ=Vector3.Distance(γ,Τ.Last().Δ);
double Ηφ=Math.Max(1.5,Math.Pow(ȕ/100.0,2));double ΕΘ=Math.Max(ȕ*Ηφ,2);Εσ=ΕΘ;if((ΒЉ==-1)||ΒЉ>=ΕΘ){Χ Ψ=new Χ(γ,ǌ,ů,Ż,Υ.GetNaturalGravity());Ψ.Εφ(ǽ,Ǽ);Τ.Add(Ψ);}}Χ о(){if(Κ!=Θ.Ϋ) return Ǧ!=null?Ǧ:Ζ;return Δλ;}void ĩ(Χ Ϊ,Λ П){Δλ=Ϊ;if(ή==Λ.Љ) т=П;}bool Ǳ(Χ Ϊ,float ό,bool ΗЍ,out Vector3 ζ)
{if(ΗЍ){Vector3I Βώ=new Vector3I((int) Ϊ.У.X,(int) Ϊ.У.Y,(int) Ϊ.У.Z);IMySlimBlock Σ=Me.CubeGrid.GetCubeBlock(Βώ);if(Σ==null||!(Σ.FatBlock is IMyShipConnector)){ζ=new Vector3();return false;}Vector3 ΖІ=Ь(Υ,Σ.FatBlock.GetPosition()-γ);Vector3 ΖЅ=Ь(Υ,Σ.FatBlock.WorldMatrix.Forward);ζ=Ϊ.Δ-ΒΨ(Ϊ.α,Ϊ.ΰ*-1,ΖІ)-ΒΨ(Ϊ.α,Ϊ.ΰ*-1,ΖЅ)*ό;
return true;}else{ζ=Ϊ.Δ;return true;}}Vector3 Ȃ(int ΖЃ,int К,bool Ηκ){if(!Ηκ&&Δύ) return Βκ;float ΖЄ=((Ѓ-1f)/2f)-ΖЃ;float Ην=((Ў-1f)/2f)-К;Βκ=Ε.Δ+Ε.χ*ΖЄ*Ǿ+Ȓ*-1*Ην*ǫ;Δύ=true;return Βκ;}Vector3 ΑЂ(Vector3 Ηϋ,float Βς){return Ηϋ+(Ǖ*Βς);}έ Αψ(){float А=-1;έ ά=έ.Ǯ;
if(Κ!=Θ.Ϋ){ΔΔ();if(ή!=Λ.г){Vector3 Ј=ć(Ǖ,Ȓ*-1,γ-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(Ѓ*Ǿ)/2f&&Math.Abs(Ј.Y)<=(float)(Ў*ǫ)/2f){if(Ј.Z<=-1&&Ј.Z>=-Ę*2) return έ.Ɠ;if(Ј.Z>-1&&Ј.Z<θ*2) return έ.Ǯ;}if(ș(Ε.Δ,ref А)) ά=έ.Ǯ;}if(Ζ.β){if(ș(Ζ.Δ,ref А)) ά=έ.ǭ;for(int i=0;i<Τ.Count;i++)
if(ș(Τ[i].Δ,ref А)) ά=έ.ȃ;if(Vector3.Distance(γ,Ζ.Δ)<dockDist*θ) ά=έ.ǭ;if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null)
ά=έ.ǭ;}}else{Vector3 ζ=new Vector3();IMyShipConnector ǹ=Ъ(MyShipConnectorStatus.Connected);if(Ζ.β){if(ș(Ζ.Δ,ref А)) ά=έ.ǭ;if(Ǳ(Ζ,dockDist,true,out ζ))
if(ș(ζ,ref А)) ά=έ.ǭ;if(ǹ!=null&&Vector3.Distance(ǹ.GetPosition(),Ζ.Δ)<5)
return έ.Αξ;}for(int i=0;i<Τ.Count;i++)
if(Vector3.Distance(Τ[i].Δ,Ζ.Δ)>dockDist*θ&&Vector3.Distance(Τ[i].Δ,Ε.Δ)>dockDist*θ)
if(ș(Τ[i].Δ,ref А)) ά=έ.ȃ;if(Ε.β){if(ș(Ε.Δ,ref А)) ά=έ.Ǯ;if(Ǳ(Ε,dockDist,true,out ζ))
if(ș(ζ,ref А)) ά=έ.Ǯ;if(ǹ!=null&&Vector3.Distance(ǹ.GetPosition(),Ε.Δ)<5)
return έ.Αο;}}return ά;}bool ș(Vector3 ζ,ref float А){float ό=Vector3.Distance(ζ,γ);if(ό<А||А==-1){А=ό;return true;}return false;}void ΒΤ(){if(ώ>0){Ξ="Setup error! Can't start";return;}if(Κ==Θ.Ϋ){Ț();return;}Ε.Δ=γ;Ε.κ=Υ.GetNaturalGravity();Ε.α=ǌ;Ε.ΰ=ů;Ε.χ=Ż;
Ǖ=є.WorldMatrix.Forward;Ȓ=Ε.ΰ;if(Ǖ==Υ.WorldMatrix.Down) Ȓ=Υ.WorldMatrix.Backward;ţ(true,true);Ρ(Α.ȁ);ΒΣ();}void ţ(bool ǐ,bool Ηλ){if(ή==Λ.г&&!ǐ) return;bool Ηέ=ǐ||ή==Λ.Αω||Ѓ!=е||Ў!=д||Ź!=ǧ;if(Ηέ){if(ή!=Λ.г){ή=Λ.Ɩ;Ȃ(φ,ρ,Ηλ);Ξ="Job changed, lost progress";}Ź=ǧ;
Ѓ=е;Ў=д;ρ=0;φ=0;с="";ǖ=-1;ė=0;Ȑ=0;Н=0;М=0;ΒΩ(true);}}void ΒΜ(){Э(γ,0);ь(ǽ,true);}void ΒΣ(){ǳ(Ю.ƪ);ΒΜ();Ţ(ǻ,false);Ǟ(Γέ);ή=Λ.Љ;Γφ(true);т=ή;μ(Ω.Ş);ΖΩ();ƿ=true;ΑΔ=0;for(int i=ť.Count-1;i>=0;i--)
if(Ŝ(ť[i],false)) ΑΔ++;if(ΑΔ>0) Ξ="Started with damage";}void σ(){if(ή==Λ.Љ){ή=Λ.Ĩ;Ξ="Job paused";}Ρ(Α.в);т=ή;ƫ(false,0,0,0,0);ΒΧ();Γσ(new Vector3(),false);ΔΣ();ΒΝ(ChargeMode.Auto);ΒΞ(false);ΔΞ(true);ȅ(Α.в);Ĉ(false,false,0,0);ь(ο,false);ь(п,true);Ǟ(true);Αά=false;Ɨ();Ƽ=false;
Ǉ=false;ƿ=false;ƅ=false;ƌ=false;if(Ѝ!=Ω.Main&&Ѝ!=Ω.Ɣ&&Ѝ!=Ω.Αλ&&Ѝ!=Ω.Ƭ)
μ(Ω.Main);}void Ț(){έ В=Αψ();if(Κ==Θ.Ϋ){if(!Ε.β||!Ζ.β) return;ΒΣ();bool Βό=Vector3.Distance(γ,Ζ.Δ)<Vector3.Distance(γ,Ε.Δ);if(Ō==Λ.Ġ) Βό=true;if(Ō==Λ.ġ) Βό=false;if(Βό){ĩ(Ζ,Λ.Ġ);switch(В){case έ.Αξ:Ρ(Α.Ȟ);break;case έ.ȃ:Ρ(Α.Ы);break;case έ.ǭ:Ρ(Α.Ǭ);break;default:Ρ(Α.ȇ);break;
}}else{ĩ(Ε,Λ.ġ);switch(В){case έ.Αο:Ρ(Α.Ȟ);break;case έ.Ǯ:Ρ(Α.Ǭ);break;case έ.ȃ:Ρ(Α.Ы);break;default:Ρ(Α.ȇ);break;}}}else{if(ή!=Λ.Ĩ&&ή!=Λ.Ɩ) return;bool ΗЏ=ή==Λ.Ɩ;ΒΣ();bool ΒЁ=ŝ(false)&&Ζ.β;switch(В){case έ.Ǯ:Ρ(ΒЁ?Α.Ȝ:Α.ȁ);break;case έ.ȃ:Ρ(ΒЁ?Α.С:Α.Ы);break;case έ.ǭ:Ρ(ΒЁ?Α.Ǭ:Α.ǵ);break;
case έ.Ɠ:{if(Ȑ!=М||ΗЏ) Ρ(Α.Ȅ);else Ρ(Α.ħ);}break;default:break;}}}void Ą(){if(ή==Λ.г&&!Ζ.β) return;if(Κ==Θ.Ϋ&&(!Ε.β||!Ζ.β)) return;Ξ="Move to job";έ В=Αψ();if(Κ==Θ.Ϋ){ĩ(Ε,Λ.ġ);switch(В){case έ.Ǯ:Ρ(Α.Ǭ);break;case έ.ȃ:Ρ(Α.Ы);break;case έ.Αο:return;default:Ρ(Α.ȇ);break;
}ȅ(Α.Ȟ);}else{switch(В){case έ.Ǯ:Ρ(Α.Ы);break;case έ.ȃ:Ρ(Α.Ы);break;case έ.ǭ:Ρ(Α.ǵ);break;case έ.Ɠ:Ρ(Α.Ȅ);break;default:break;}if(ή==Λ.г) ȅ(Α.Ы);else ȅ(Α.Ā);Αά=true;}ΒΜ();μ(Ω.ΒΦ);Ţ(ǻ,false);т=Λ.ƣ;}void Ɯ(){if(!Ζ.β) return;Ξ="Move home";έ В=Αψ();if(Κ==Θ.Ϋ){ĩ(Ζ,Λ.Ġ);switch(В)
{case έ.ȃ:Ρ(Α.С);break;case έ.ǭ:Ρ(Α.Ǭ);break;case έ.Αξ:return;default:Ρ(Α.ȇ);break;}ȅ(Α.Ȟ);}else{if(Ъ(MyShipConnectorStatus.Connected)!=null) return;if(Ъ(MyShipConnectorStatus.Connectable)!=null){Ρ(Α.ĥ);ȅ(Α.ǵ);return;}switch(В){case έ.Ǯ:Ρ(Α.Ȝ);break;case έ.ȃ:Ρ(Α.С);break;case έ.ǭ:Ρ(Α.С);break;
case έ.Ɠ:Ρ(Α.Ć);break;default:break;}ȅ(Α.ǵ);}ΒΜ();μ(Ω.ΒΦ);Ţ(ǻ,false);т=Λ.Ƣ;}void Ρ(Α ΓΘ){if(ΓΘ==Α.в) Ƈ=Α.в;if(Ƈ!=Α.в&&Φ==Ƈ&&ΓΘ!=Ƈ){σ();return;}Є=true;Φ=ΓΘ;}void ȅ(Α П){this.Ƈ=П;}Ȇ ΕБ(int Ŷ,bool Ǘ){if(Ǘ){і=null;φ=0;ρ=0;}if(ǧ==ў.Ʈ){int ΕΛ=Ŷ+1;ρ=(int) Math.Floor(Ќ(Ŷ,Ѓ));
if(ρ%2==0) φ=Ŷ-(ρ*Ѓ);else φ=Ѓ-1-(Ŷ-(ρ*Ѓ));if(ρ>=Ў) return Ȇ.Αν;else return Ȇ.ΑϏ;}else if(ǧ==ў.Γψ){if(і==null) і=new int[]{0,-1,0,0};int ΔЀ=(int) Math.Ceiling(Ѓ/2f);int ΔϏ=(int) Math.Ceiling(Ў/2f);int ΗΕ=(int) Math.Floor(Ѓ/2f);int ΗΔ=(int) Math.Floor(Ў/2f);
int ŷ=0;while(і[2]<Math.Pow(Math.Max(Ѓ,Ў),2)){if(ŷ>200) return Ȇ.ΕЈ;ŷ++;і[2]++;if(-ΔЀ<φ&&φ<=ΗΕ&&-ΔϏ<ρ&&ρ<=ΗΔ){if(і[3]==Ŷ){this.φ=φ-1+ΔЀ;this.ρ=ρ-1+ΔϏ;return Ȇ.ΑϏ;}і[3]++;}if(φ==ρ||(φ<0&&φ==-ρ)||(φ>0&&φ==1-ρ)){int Ηώ=і[0];і[0]=-і[1];і[1]=Ηώ;
}φ+=і[0];ρ+=і[1];}}return Ȇ.Αν;}void Ζο(){if(Φ==Α.ȁ){if(Є){Ѕ=0;if(Ȑ!=М) ė=0;Ȑ=М;}if(Ѕ==0){int Βμ=Εϋ(М);if(Βμ<0){if(!Ƽ){Ƽ=true;Δκ=γ;}Э(Δκ,0);Ξ="Waiting for a free row";return;}Ƽ=false;bool ΔЈ=Βμ!=М;if(ΔЈ){М=Βμ;ė=0;Ȑ=М;}Ȇ ά=ΕБ(М,Є||ΔЈ);if(ά==Ȇ.Αν)
{ή=Λ.Αω;Ξ="Job done";if(Ȳ&&Ζ.β){Ρ(Α.Ȝ);ȅ(Α.ǵ);т=Λ.Ƣ;}else{Ρ(Α.Ħ);ȅ(Α.Ā);т=Λ.ƣ;}return;}if(ά==Ȇ.ΑϏ){Ѕ=1;ь(ο,true);х=Ȃ(φ,ρ,true);Э(х,10);Щ(Ε.ΰ,Ε.α,Ε.χ,false);}}else{if(З<wpReachedDist){Ρ(Α.ħ);return;}}}if(Φ==Α.ħ){if(Є){ь(ο,true);Ǟ(false);х=Ȃ(φ,ρ,false);
Э(ΑЂ(х,0),0);Щ(Ε.ΰ,Ε.α,Ε.χ,false);Ѕ=1;ж=0;Ш=0;ƹ=0;ΒЊ=-1;Ę=Т;ΒЄ=true;}if(!Ατ()){Ρ(Α.Ć);return;}if(ŝ(true)){ΕΞ=Ȁ("","ORE",Е.ā);if((И==Ϗ.Αϊ||И==Ϗ.Ƙ||
И==Ϗ.ΑЉ||И==Ϗ.ƞ)&&Κ!=Θ.ќ)
Ρ(Α.Ř);else if((И==Ϗ.Γϋ||И==Ϗ.Αφ)&&Κ!=Θ.ќ)
Ρ(Α.ΒΪ);else
Ρ(Α.Ć);return;}Н=Vector3.Distance(γ,х);if(Н>ė){ė=Н;ΒЄ=false;}if(Κ==Θ.ќ&&ΓЈ()==MyDetectedEntityType.SmallGrid)
Ш+=2;else Ш-=2;Ш=Math.Max(100,Math.Min(400,Ш));if(Ѕ>0&&Ѕ<Ш){if(Н>ж){if(Ш>150) ж=Н;else ж=(float) Math.Ceiling(Н);Ѕ=1;}else Ѕ++;}else{if(Ѕ>0){Ξ="Ship stuck! Retrying";ж=Н;Ѕ=0;Ĉ(false,true,0,θ*sensorRange);}Э(ΑЂ(х,Math.Max(0,ж-θ)),ΑЅ(false));if(З<=wpReachedDist/2){Ѕ=1;ж=0;}
return;}Ĉ(false,true,θ*sensorRange,0);Vector3 Ζω=х+Ǖ*Н;bool ΔІ=false;if(Vector3.Distance(Ζω,γ)>0.3f){Vector3 ΖЇ=х+Ǖ*(Н+0.1f);Э(ΖЇ,4);ΔІ=true;}else{float ǘ=ΑЅ(true);Vector3 Δο=ΑЂ(х,Math.Max(Т+1,Н+1));ǝ(true,false,false,Δο,Δο-х,ǘ,ǘ);}bool Βσ=false;if(р==ъ.Αρ||р==ъ.Απ)
{if(!ΔІ){float ΓΙ=0;foreach(IMyTerminalBlock Ǫ in ο)
ΓΙ+=ΕЂ(Ǫ,"","",р==ъ.Απ?new string[]{"STONE"}:null);if(ΓΙ>ΒЊ||Н<Т||ΒЄ){Βα=0;ƹ=Н;Ę=(int)(Math.Max(Ę,ƹ)+θ/2);}else{Βσ=Н-ƹ>2&&Βα>=20;Βα++;}ΒЊ=ΓΙ;}}else Βσ=Н>=Ę;if(Ȑ!=М){Ρ(Α.Ȅ);Н=0;return;}if(Βσ){М++;Ρ(Α.Ȅ);Н=0;return;}}if(Φ==Α.Ȅ||Φ==Α.Ć||Φ==Α.Ř)
{if(Є){х=Ȃ(φ,ρ,false);Щ(Ε.ΰ,Ε.α,Ε.χ,false);ь(ο,Ķ);Ǟ(false);ж=Vector3.Distance(γ,х);Ĉ(false,true,0,θ*sensorRange);}Э(х,ΑЅ(false));if(Vector3.Distance(γ,х)>=ж+5){ь(ο,false);Ǟ(true);Ξ="Can´t return!";}if(З<wpReachedDist){if(Φ==Α.Ȅ&&Αά) Ρ(Α.Ħ);if(Φ==Α.Ȅ) Ρ(Α.ȁ);
if(Φ==Α.Ř) Ρ(Α.ƛ);if(Φ==Α.Ć){if(Ζ.β) Ρ(Α.Ȝ);else{σ();Ą();Ξ="Can´t return, no dock found";}}return;}}if(Φ==Α.С||Φ==Α.Ы){if(Φ==Α.Ы&&ή==Λ.Љ&&Κ!=Θ.Ϋ){if(!Ατ()||ŝ(true)){Ρ(Α.С);return;}}bool ΓΤ=false;bool Γκ=false;bool Δί=false;float į=0;bool ΕΝ=false;Χ Ψ=null;
if(Є){if(Φ==Α.С||Κ==Θ.Ϋ){if(Κ!=Θ.Ϋ){Ǧ=null;Ȯ=0;Ǉ=!Αυ(1);}Χ Ϊ=о();η=new ΒΚ();η.Л=Ϊ;η.ŭ=followPathDock*θ;η.Γλ=useDockDirectionDist*θ;η.į=10;η.Ŏ.Add(Ϊ.Δ);Vector3 Į=new Vector3();if(Ǳ(Ϊ,dockDist*θ,true,out Į))
η.Ŏ.Add(Į);else η.ŭ*=1.5f;if(Κ==Θ.Ϋ){if(Ϊ==Ζ) η.ǉ=Ε.Δ;if(Ϊ==Ε) η.ǉ=Ζ.Δ;η.Βω=dockDist*θ*1.1f;}}else if(Φ==Α.Ы){η=new ΒΚ();η.Л=Ε;η.ŭ=followPathJob*θ;η.Γλ=useJobDirectionDist*θ;η.į=10;η.ǉ=Ζ.Δ;η.Βω=dockDist*θ*1.1f;η.Ŏ.Add(Ε.Δ);if(ή==Λ.г){if(!Ζ.β||Τ.Count==0){σ();return;}
float ΖЍ=Vector3.Distance(Τ.First().Δ,Ζ.Δ);float ΖЎ=Vector3.Distance(Τ.Last().Δ,Ζ.Δ);if(ΖЍ<ΖЎ) η.Л=Τ.Last();else η.Л=Τ.First();}}ƀ=new Vector3();ΕΝ=!ΖΚ(γ);ь(ο,false);Ǟ(true);ѓ=-1;double ǡ=-1;for(int i=Τ.Count-1;i>=0;i--){if(Vector3.Distance(Τ[i].Δ,η.ǉ)<=η.Βω) continue;
double ό=Vector3.Distance(Τ[i].Δ,γ);if(ǡ==-1||ό<ǡ){ѓ=i;ǡ=ό;}}ȭ=ΖΑ(η.Л.Δ,ѓ);Đ=null;}if(Φ==Α.С&&Κ!=Θ.Ϋ&&!Є){Χ Βε=о();Ǉ=!Αυ(1);if(о()!=Βε){Ρ(Α.С);return;}}Εχ(Τ,ȭ,η.Ŏ,η.ŭ,Є,ref Ѕ);for(int i=0;i<η.Ŏ.Count;i++){float ό=Vector3.Distance(γ,η.Ŏ[i]);if(ό<=η.ŭ) ΓΤ=true;
if(ό<=η.Γλ) Γκ=true;}if(Γκ) į=η.į;float Θη=Đ!=null?Đ.Џ:ȕ;float ΘΓ=(float) Math.Max(ȕ*0.1f*θ,wpReachedDist);if((З<ΘΓ)||Є){if(!Є) ѓ+=ȭ;if(ȭ==0||ѓ>Τ.Count-1||ѓ<0)
ΓΤ=true;else{Γπ=ȭ>0?Τ.Count-1-ѓ:ѓ;Ψ=Τ[ѓ];Đ=Ψ;if(ѓ>=1&&ѓ<Τ.Count-1)
ƀ=Ψ.Δ-Τ[ѓ-ȭ].Δ;else Đ=null;ω=Ψ.Δ+ΕА(ѓ,ȭ);Δί=true;}}if(Γκ)
Щ(η.Л.ΰ,η.Л.α,η.Л.χ,false);else if(ΕΝ)
Αμ(η.Л.ΰ,10,true);else if(Δί&&Ψ!=null){if(ȭ>0) Щ(Ψ.ΰ,Ψ.α,Ψ.χ,90,false);else Щ(Ψ.ΰ,-Ψ.α,-Ψ.χ,90,false);}ǝ(true,false,true,ω,ƀ,Đ==null?0:Đ.Џ,į);if(ΓΤ){Γπ=0;if(Φ==Α.С&&Ǉ){Э(ω,0);Ξ="Waiting for a free dock";return;}if(Φ==Α.С||Κ==Θ.Ϋ){Ρ(Α.Ǭ);return;}
if(Φ==Α.Ы&&Αά){Ρ(Α.Ħ);return;}if(Φ==Α.Ы){Ρ(Α.ȁ);return;}}}if(Φ==Α.Ǭ||Φ==Α.ƨ){if(Κ!=Θ.Ϋ){Χ Βε=о();if(!Αυ(2)||о()!=Βε){Ρ(Α.С);return;}}Χ Ϊ=о();if(Є){if(!Ǳ(Ϊ,dockDist*θ,true,out ω)){ǳ(Ю.Ŗ);σ();return;}Э(ω,0);Αμ(Ϊ.ΰ,90,true);}if(З<followPathDock*θ&&З!=-1)
{Э(ω,10);Щ(Ϊ.ΰ,Ϊ.α,Ϊ.χ,false);}if(Ъ(MyShipConnectorStatus.Connectable)!=null||Ъ(MyShipConnectorStatus.Connected)!=null){Ρ(Α.ĥ);return;}if(З<wpReachedDist/2&&З!=-1){Ρ(Α.ģ);return;}}if(Φ==Α.ģ||Φ==Α.Ā){if(Є){if(Φ==Α.ģ){Χ Ϊ=о();if(!Ǳ(Ϊ,dockDist*θ,true,out ω)){ǳ(Ю.Ŗ);σ();return;}
ǝ(true,true,false,ω,0);Щ(Ϊ.ΰ,Ϊ.α,Ϊ.χ,10,false);}if(Φ==Α.Ā){Щ(Ε.ΰ,Ε.α,Ε.χ,0.5f,false);ω=Ε.Δ;ǝ(true,true,false,ω,0);}}if(ΑΖ){ƫ(false,0,0,0,0);if(Φ==Α.ģ) Ρ(Α.ĥ);if(Φ==Α.Ā) σ();return;}}if(Φ==Α.ĥ){if(Ъ(MyShipConnectorStatus.Connected)!=null){if(Κ==Θ.Ϋ) Ρ(Α.Ȟ);else Ρ(Α.ǵ);
return;}Χ Ϊ=о();if(Є){Ш=0;ΖЏ=DateTime.Now;Ѕ=0;Щ(Ϊ.ΰ,Ϊ.α,Ϊ.χ,false);}Vector3I Βώ=new Vector3I((int) Ϊ.У.X,(int) Ϊ.У.Y,(int) Ϊ.У.Z);IMySlimBlock Δι=Me.CubeGrid.GetCubeBlock(Βώ);float Δτ=dockingSpeed;float ΗΧ=dockingSpeed*5;float Δά=Math.Max(1.5f,Math.Min(5f,θ*0.15f));if(!Ǳ(Ϊ,0,true,out ω)||!Ǳ(Ϊ,Δά,true,out ƀ)||
Δι==null||!Δι.FatBlock.IsFunctional){ǳ(Ю.Ŗ);σ();return;}if(Ш==1||(Vector3.Distance(γ,ω)<=Δά*1.1f&&!Є))
Ш=1;else{Vector3 ΗЅ=Ь(Υ,ƀ-γ);Vector3 Βύ=Ь(Υ,Υ.GetNaturalGravity());float Ηξ=ƕ(ΗЅ,Βύ,null);Δτ=Math.Min(ΗΧ,Ηξ);}ǝ(true,false,false,ω,ω-γ,dockingSpeed,Δτ);if(Є) ж=(float) З;IMyShipConnector Βο=Ъ(MyShipConnectorStatus.Connectable);if(Βο!=null){ǝ(false,false,false,ω,0);
if(Ѕ>0) Ѕ=0;Ѕ--;if(Ѕ<-5){Βο.Connect();if(Βο.Status==MyShipConnectorStatus.Connected){if(Κ==Θ.Ϋ) Ρ(Α.Ȟ);else Ρ(Α.ǵ);ΒΧ();Ţ(ǻ,true);return;}}}else{float Δσ=(float) Math.Round(З,1);if(Δσ<ж){Ѕ=-1;ж=Δσ;}else Ѕ++;if(Ѕ>20){Ρ(Α.ƨ);return;}}}if(Φ==Α.ǵ||Φ==Α.Ȟ||
Φ==Α.Ƴ||Φ==Α.ƚ||Φ==Α.ă){bool Βί=false,Βΰ=false;if(Κ==Θ.Ϋ){if(о()==Ζ) Βί=true;else if(о()==Ε) Βΰ=true;}if(Є){ƅ=false;if(Ъ(MyShipConnectorStatus.Connected)==null){Ρ(Α.ȇ);return;}ΒΧ();if(Βί) Ư(ϊ.Ǐ);if(Βΰ) Ư(ϋ.Ǐ);Ĝ=false;Ȉ=false;ȍ=false;Ǝ=false;}if(Ъ(MyShipConnectorStatus.Connected)==null){σ();ǳ(Ю.ΓА);return;}
if(ή!=Λ.Љ||у==-1||Ǡ==л.None) Ȉ=true;else if(Ƕ>=100f) Ȉ=true;else if(Ƕ<=99f) Ȉ=false;if(ή!=Λ.Љ||ф==-1||я.Count==0) ȍ=true;else if(Ȏ>=100f) ȍ=true;else if(Ȏ<=99) ȍ=false;if(ή!=Λ.Љ||Ц==-1||ȯ.Count==0) Ǝ=true;else Ǝ=ĝ>=Ц;ī к=null;if(Βί) к=ϊ;
if(Βΰ) к=ϋ;if(к!=null&&(к.Ћ==ε.Ʀ||к.Ћ==ε.ƥ)) Ȉ=true;if(к!=null&&(к.Ћ==ε.ΒΑ)) if(!Ĝ) Ȉ=false;if(к!=null&&(к.Ћ==ε.ΒΕ||к.Ћ==ε.ΒΓ)) ȍ=true;if(к!=null&&(к.Ћ==ε.ΒΔ)) if(!Ĝ) ȍ=false;if(ȫ){ChargeMode Δη=Ȉ?ChargeMode.Auto:ChargeMode.Recharge;if(к!=null&&(к.Ћ==ε.ƥ||к.Ћ==ε.Ʀ))
Δη=ChargeMode.Discharge;ΒΝ(Δη);ΒΞ(!ȍ);}if(!Ĝ){if(Κ==Θ.Ϋ)
Ĝ=ή!=Λ.Љ||Εϊ(Є,true)||ƌ;else
Ĝ=ή!=Λ.Љ||ΓБ();}else{if(!Ȉ) Ρ(Α.ă);if(!ȍ) Ρ(Α.ƚ);if(!Ǝ) Ρ(Α.Ƴ);Є=false;}if(Ĝ&&Ȉ&&ȍ&&Ǝ){ΒΝ(ChargeMode.Auto);ΒΞ(false);if(ή==Λ.Љ){if(Κ==Θ.Ϋ){if(о()==Ζ) Ư(ϊ.ǒ);else if(о()==Ε) Ư(ϋ.ǒ);if(о()==Ζ) ĩ(Ε,Λ.ġ);else ĩ(Ζ,Λ.Ġ);}}Ρ(Α.ȇ);return;}}if(Φ==Α.ȇ){if(Є){IMyShipConnector ǹ=Ъ(MyShipConnectorStatus.Connected);
if(ǹ==null){Ρ(Α.Ы);return;}IMyShipConnector Ѐ=ǹ.OtherConnector;ΔΝ(ǹ,false);Ţ(ǻ,false);Χ Ϊ=null;Χ ΔЂ=Κ==Θ.Ϋ?Ζ:о();if(Vector3.Distance(ǹ.GetPosition(),ΔЂ.Δ)<5f&&Ζ.β) Ϊ=ΔЂ;if(Vector3.Distance(ǹ.GetPosition(),Ε.Δ)<5f&&Ε.β) Ϊ=Ε;if(Ϊ!=null){if(!Ǳ(Ϊ,dockDist*θ,true,out ω))
{ǳ(Ю.Ŗ);σ();return;}Э(ω,5);Щ(Ϊ.ΰ,Ϊ.α,Ϊ.χ,false);}else Э(γ+Ѐ.WorldMatrix.Forward*dockDist*θ,5);if(ή==Λ.Љ) ǳ(Ю.ƪ);}if(З<wpReachedDist){ь(п,true);Ρ(Α.Ы);return;}}if(Φ==Α.Ħ){if(Є){Ǟ(true);ь(ο,false);ω=Ε.Δ;Э(ω,20);Щ(Ε.ΰ,Ε.α,Ε.χ,false);}if(З<wpReachedDist/2){Ρ(Α.Ā);return;}
}if(Φ==Α.Ȝ){if(Є){Ǟ(true);ь(ο,false);int А=-1;double ǡ=-1;for(int i=Τ.Count-1;i>=0;i--){double ό=Vector3.Distance(Τ[i].Δ,γ);if(ǡ==-1||ό<ǡ){А=i;ǡ=ό;}}if(А==-1){Ρ(Α.С);return;}ω=Τ[А].Δ;Э(ω,10);Щ(Ε.ΰ,Ε.α,Ε.χ,false);}if(З<wpReachedDist){Ρ(Α.С);return;}
}if(Φ==Α.ƛ){bool Ǣ=false;if(Є){Ǟ(true);if((И==Ϗ.Αϊ||И==Ϗ.Ƙ)&&Ɵ()&&
Ĥ(Ǖ,Υ.GetNaturalGravity())<25&&Ѓ>=2&&Ў>=2){Vector3 Ű=γ;if(φ>0&&ρ<Ў-1) Ű=Ȃ(φ-1,ρ+1,true);else if(φ<Ѓ-1&&ρ<Ў-1) Ű=Ȃ(φ+1,ρ+1,true);else if(φ<Ѓ-1&&ρ>0) Ű=Ȃ(φ+1,ρ-1,true);else if(φ>0&&ρ>0) Ű=Ȃ(φ-1,ρ-1,true);else Ǣ=true;if(!Ǣ) Э(Ű,10);
}else Ǣ=true;}if(З<wpReachedDist/2) Ǣ=true;if(Ǣ){Ρ(Α.Ʋ);return;}}if(Φ==Α.ΒΪ||Φ==Α.Ʋ){if(Є){ǝ(true,true,false,γ,0);ь(ο,false);Ǟ(true);Ѕ=-1;Ш=И==Ϗ.ΑЉ||И==Ϗ.ƞ?0:-1;}bool Ē=!Ατ();int Αβ=Ȁ("STONE","ORE",Е.ā);if(И==Ϗ.Αφ||И==Ϗ.ƞ||И==Ϗ.Ƙ)
Αβ+=Ȁ("ICE","ORE",Е.ā);bool ΗΖ=Αβ>0;bool Βψ=false;if(Ш>=0){float Ĭ=(float) Math.Sin(Γύ(Ш))*Ǿ/3f;float Ζϋ=(float) Math.Cos(Γύ(Ш))*ǫ/3f;Vector3 ΖЂ=Ȃ(φ,ρ,true)+ΒΨ(Ǖ,Ȓ*-1,new Vector3(Ĭ,Ζϋ,0));Э(ΖЂ,0.3f);if(З<Math.Min(Ǿ,ǫ)/10f) Ш+=5f;if(Ш>=360) Ш=0;}if(Ѕ==-1||Αβ<Ѕ){Ѕ=Αβ;ж=0;}
else{ж++;if(ж>50) Βψ=true;}if(!ΗΖ||Ē||Βψ){if(!Ē){int ΗЊ=Ȁ("","ORE",Е.ā);if(ŝ(true)) Ē=true;else if(100-(Ќ(ΗЊ,ΕΞ)*100)<minEjection) Ē=true;else ǳ(Ю.ƪ);}if(Βψ&&Ē) Ξ="Ejection failed";if(Φ==Α.Ʋ){if(Ē){if(Ζ.β) Ρ(Α.Ȝ);else{σ();Ą();Ξ="Can´t return, no dock found";}}
else Ρ(Α.ȁ);}else if(Ē) Ρ(Α.Ć);else Ρ(Α.ħ);return;}}Є=false;}void Εχ(List<Χ>ȳ,int ι,List<Vector3>Ɖ,float Ǩ,bool Ǘ,ref int Ǎ){if(Ǘ){for(int i=0;i<ȳ.Count;i++) ȳ[i].Џ=0;Ǎ=-1;return;}if(ι==0) return;int ї=ι*-1;if(Ǎ==-1) Ǎ=ї>0?1:ȳ.Count-2;int ŷ=0;while(Ǎ>=1&&Ǎ<ȳ.Count-1)
{if(ŷ>50) return;ŷ++;try{if((ї<0&&Ǎ>=1)||(ї>0&&Ǎ<=ȳ.Count-2)){Χ δ=ȳ[Ǎ];bool ΕΚ=false;for(int j=0;j<Ɖ.Count;j++)
if(Vector3.Distance(δ.Δ,Ɖ[j])<=Ǩ){ΕΚ=true;break;}if(!ΕΚ){Χ ΑΨ=ȳ[Ǎ-ї];Χ ΕΛ=ȳ[Ǎ+ї];Vector3 ΗІ=δ.Δ-ΕΛ.Δ;Vector3 Γη=ΑΨ.Δ-δ.Δ;Vector3 Ηθ=δ.Δ+Vector3.Normalize(ΗІ)*Γη.Length();Vector3 ΖЌ=ΑΨ.Δ-Ηθ;Vector3 ΑΚ=ć(ι>0?δ.α:δ.α*-1,δ.ΰ*-1,ΖЌ);Vector3 ΗΥ=ć(ι>0?δ.α:δ.α*-1,δ.ΰ*-1,Γη);
Vector3 ΑΜ=ć(ι>0?δ.α:δ.α*-1,δ.ΰ*-1,δ.κ);δ.Џ=(float) Math.Sqrt(Math.Pow(ΑΨ.Џ,2)+Math.Pow(ƕ(-ΗΥ,ΑΜ,δ),2));for(int j=0;j<Ɖ.Count;j++)
if(Vector3.Distance(ΑΨ.Δ,Ɖ[j])<=Ǩ){Vector3 ΑΝ=ć(ι>0?δ.α:δ.α*-1,δ.ΰ*-1,Ɖ[j]-δ.Δ);float Ηύ=ƕ(-ΑΝ,ΑΜ,δ);δ.Џ=Math.Min(δ.Џ,Ηύ)/2f;}if(ΑΚ.Length()==0) ΑΚ=new Vector3(0,0,1);Vector3 ΗΤ=ć(δ.α,δ.ΰ*-1,δ.κ);float ΗЀ=ř(-ΑΚ,ΗΤ,δ);float ƴ=Ќ(ΗЀ,Ě);float Γε=(float) Math.Sqrt(Ќ(ΑΚ.Length()*1.0f,0.5f*ƴ));
δ.Џ=Math.Min(δ.Џ,(Γη.Length()/Γε)*ǟ);}}}catch{return;}Ǎ+=ї;}Ǎ=-1;}void ΒΩ(bool Ǘ){if(Ǘ){Ÿ=0;return;}if(Κ==Θ.Ϋ) return;float Ǣ=М*Math.Max(1,Т);if(Ȑ==М) Ǣ+=Math.Min(Т,Н);float ΗЉ=Ѓ*Ў*Math.Max(1,Т);Ÿ=Math.Max(Ÿ,(float) Math.Min(Ǣ/ΗЉ*100.0,100));}MyDetectedEntityType ΓЈ()
{try{if(ǲ(λ,true)&&!λ.LastDetectedEntity.IsEmpty()) return λ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΑЅ(bool Ǥ){if(Κ==Θ.ќ&&ΓЈ()==MyDetectedEntityType.None&&!Ŝ(λ,true))
return fastSpeed;else return Ǥ?Ŕ:œ;}void Ζκ(){float ΕΕ=0,Δν=0,ĕ=0,ΓΛ=0;for(int i=0;i<ȟ.Count;i++){IMyBatteryBlock Ũ=ȟ[i];if(!ǲ(Ũ,true)) continue;ΕΕ+=Ũ.MaxStoredPower;Δν+=Ũ.CurrentStoredPower;ĕ+=Ũ.CurrentInput;ΓΛ+=Ũ.CurrentOutput;}Ƕ=(float) Math.Round(Ќ(Δν,ΕΕ)*100,1);
if(ĕ>=ΓΛ) Ǡ=л.ă;else Ǡ=л.Γώ;if(ĕ==0&&ΓΛ==0||Ƕ==100.0) Ǡ=л.в;if(ȟ.Count==0) Ǡ=л.None;}void Ζν(){float Δω=0;for(int i=0;i<я.Count;i++){IMyGasTank Εζ=я[i];if(!ǲ(Εζ,true)) continue;Δω+=(float) Εζ.FilledRatio;}Ȏ=Ќ(Δω,я.Count)*100f;}void Ζυ(){ĝ=0;try{for(int i=0;i<ȯ.Count;i++)
{IMyReactor ΓΥ=ȯ[i];if(!ǲ(ΓΥ,true)) continue;List<MyInventoryItem>Х=new List<MyInventoryItem>();ΓΥ.GetInventory(0).GetItems(Х);float џ=0;for(int j=0;j<Х.Count;j++){MyInventoryItem Б=Х[j];if(Б.Type.SubtypeId.ToUpper()=="URANIUM"&&Б.Type.TypeId.ToUpper().Contains("_INGOT"))
џ+=(float) Б.Amount;}if(џ<ĝ||i==0){ĝ=џ;ΒБ=ΓΥ.CustomName;}}}catch(Exception e){ȏ=e;}}void Ζλ(){Ń=0;Ĵ=0;try{for(int i=0;i<ĳ.Count;i++){IMyTerminalBlock Σ=ĳ[i];if(!ǲ(Σ,true)) continue;Ĵ+=(float) Σ.GetInventory(0).CurrentVolume;Ń+=(float) Σ.GetInventory(0).MaxVolume;
}ΑΙ=(float) Math.Min(Math.Round(Ќ(Ĵ,Ń)*100,1),100.0);}catch(Exception e){ȏ=e;}}void Ζξ(){try{Ǒ.Clear();for(int i=0;i<ĳ.Count;i++){IMyTerminalBlock Σ=ĳ[i];if(!ǲ(Σ,true)) continue;Γς(Σ,Е.ś);}if(И!=Ϗ.ΑБ){for(int i=0;i<ċ.Count;i++){IMyTerminalBlock Σ=ċ[i];if(!ǲ(Σ,true)) continue;
Γς(Σ,Е.ΖΖ);}}Ζε(Ǒ);}catch(Exception e){ȏ=e;}}void Εω(){if(ƿ){if(ΓЇ().Count>ΑΔ){ƿ=false;if(и!=б.Ś){σ();if(и==б.Αό) Ą();if(и==б.Ʃ)
if(Ζ.β) Ɯ();else Ą();}Ξ="Damage detected";}}}bool Ατ(){if(!ΑΕ) return true;if(ή==Λ.Љ){if(у>0&&Ǡ!=л.None)
if(Ƕ<=у){Ξ="Low energy! Move home";return false;}if(Ц>0&&ȯ.Count>0)
if(ĝ<=Ц){Ξ="Low fuel: "+ΒБ;return false;}if(ф>0&&я.Count>0)
if(Ȏ<=ф){Ξ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΓЇ(){List<IMyTerminalBlock>đ=new List<IMyTerminalBlock>();for(int i=0;i<ť.Count;i++){IMyTerminalBlock Σ=ť[i];if(Ŝ(Σ,false)) đ.Add(Σ);}return đ;}bool Ŝ(IMyTerminalBlock Σ,bool Γή){return(!ǲ(Σ,Γή)||!Σ.IsFunctional);
}bool ǲ(IMyTerminalBlock Σ,bool Γή){if(Σ==null) return false;try{IMyCubeBlock Δό=Me.CubeGrid.GetCubeBlock(Σ.Position).FatBlock;if(Γή) return Δό==Σ;else return Δό.GetType()==Σ.GetType();}catch{return false;}}float ΕЂ(IMyTerminalBlock Σ,String щ,String Εέ,String[]ǣ){float Ǻ=0;for(int i=0;i<Σ.InventoryCount;i++)
{IMyInventory Ļ=Σ.GetInventory(i);List<MyInventoryItem>Х=new List<MyInventoryItem>();Ļ.GetItems(Х);foreach(MyInventoryItem Б in Х){if(ǣ!=null&&(ǣ.Contains(Б.Type.TypeId.ToUpper())||ǣ.Contains(Б.Type.SubtypeId.ToUpper())))
continue;if((щ==""||Б.Type.TypeId.ToUpper()==щ)&&(Εέ==""||Б.Type.SubtypeId.ToUpper()==Εέ))
Ǻ+=(float) Б.Amount;}}return Ǻ;}ы ΕЋ(String ς,String ξ,Е В,bool ΖЈ){ς=ς.ToUpper();ξ=ξ.ToUpper();for(int i=0;i<Ǒ.Count;i++){ы Б=Ǒ[i];if(Б.ς.ToUpper()==ς&&Б.ξ.ToUpper()==ξ&&(Б.ŀ==В||В==Е.ā))
return Б;}ы ά=null;if(ΖЈ){ά=new ы(ς,ξ,0,В);Ǒ.Add(ά);}return ά;}int Ȁ(String ς,String ξ,Е В){return Ȁ(ς,ξ,В,null);}int Ȁ(String ς,String ξ,Е В,String[]ǣ){int Ǻ=0;ς=ς.ToUpper();ξ=ξ.ToUpper();for(int i=0;i<Ǒ.Count;i++){ы Б=Ǒ[i];if(ǣ!=null&&ǣ.Contains(Б.ς.ToUpper())) continue;
if((ς==""||Б.ς.ToUpper()==ς)&&(ξ==""||Б.ξ.ToUpper()==ξ)&&(Б.ŀ==В||В==Е.ā))
Ǻ+=Б.џ;}return Ǻ;}void Γς(IMyTerminalBlock Σ,Е В){for(int i=0;i<Σ.InventoryCount;i++){List<MyInventoryItem>Х=new List<MyInventoryItem>();Σ.GetInventory(i).GetItems(Х);for(int j=0;j<Х.Count;j++)
ΕЋ(Х[j].Type.SubtypeId,Х[j].Type.TypeId.Replace("MyObjectBuilder_",""),В,true).џ+=(int) Х[j].Amount;}}void Ζε(List<ы>з){for(int i=з.Count-1;i>0;i--)
for(int j=0;j<i;j++){ы a=з[j];ы b=з[j+1];if(a.џ<b.џ) з.Move(j,j+1);}}bool ŝ(bool Θα){if(Ș&&Κ!=Θ.Ϋ)
if(Й!=-1&&Ě>=Й){Ξ="Ship too heavy";return true;}if(ΑΙ>=ǥ||ƅ){ƅ=false;Ξ="Ship is full";return true;}return false;}bool ΓБ(){String[]ǣ=null;if(!ȗ) ǣ=new string[]{"ICE"};if(Κ==Θ.Š) return Ȁ("","ORE",Е.ś,ǣ)==0;if(Κ==Θ.ќ) return Ȁ("","COMPONENT",Е.ś,ǣ)==0;else return Ȁ("","",Е.ś,ǣ)==0;
}bool ΖΛ(){List<IMyCockpit>Βξ=new List<IMyCockpit>();ψ.GetBlocksOfType(Βξ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Βξ.Count;i++)
if(Βξ[i].IsUnderControl) return true;return false;}bool Εϊ(bool Ǘ,bool Αί){IMyShipConnector Ж=Ъ(MyShipConnectorStatus.Connected);if(Ж==null) return false;if(Vector3.Distance(Ζ.Δ,Ж.GetPosition())<5)
return Γϊ(ϊ,Ǘ,Αί);if(Vector3.Distance(Ε.Δ,Ж.GetPosition())<5)
return Γϊ(ϋ,Ǘ,Αί);return false;}bool Γϊ(ī Ǹ,bool Ǘ,bool Αί){if(Ǘ) Ǹ.ΖΦ();Ǹ.Ζς();bool й=false;String ш="";switch(Ǹ.Ћ){case ε.ΒΒ:{ш="Waiting for command";й=false;break;}case ε.ΔΘ:{ш="Waiting for cargo";й=ŝ(true);break;}case ε.ΔΗ:{ш="Unloading";й=ΓБ();break;}case ε.ΒΗ:{й=true;break;}
case ε.ΒΑ:{ш="Charging batteries";й=Ƕ>=100f;break;}case ε.Ʀ:{ш="Discharging batteries";й=Ƕ<=25f;break;}case ε.ƥ:{ш="Discharging batteries";й=Ƕ<=0f;break;}case ε.ΒΔ:{ш="Filling up hydrogen";й=Ȏ>=100f;break;}case ε.ΒΕ:{ш="Unloading hydrogen";й=Ȏ<=25f;break;}
case ε.ΒΓ:{ш="Unloading hydrogen";й=Ȏ<=0f;break;}case ε.ΒΖ:{bool ΔЄ=ΖΛ();if(!ΔЄ) Ǹ.ΓΡ=true;й=Ǹ.ΓΡ&&ΔЄ;ш="Waiting for passengers";break;}}if(!й) Ǹ.Ασ(true);if(й&&Ǹ.ΑЇ()){й=Ǹ.Ασ(false);ш="Undocking in: "+Αώ((int) Math.Max(0,Ǹ.ǎ-Ǹ.ȣ));}if(Αί) Ερ=ш;return й;}void Ετ()
{if(!ļ) return;if(ο.Count<=1) return;float Ελ=0,Εκ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǫ=ο[i];if(Ŝ(Ǫ,true)) continue;Ελ+=(float) Ǫ.GetInventory(0).MaxVolume;Εκ+=(float) Ǫ.GetInventory(0).CurrentVolume;}float Δΰ=(float) Math.Round(Ќ(Εκ,Ελ),5);for(int ΕΠ=0;ΕΠ<Math.Max(1,Math.Floor(ο.Count/10f));ΕΠ++)
{float ΒЂ=0,ΒА=0;float ΔЁ=0,ΕΓ=0;IMyTerminalBlock ȋ=null,Ȥ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǫ=ο[i];if(Ŝ(Ǫ,true)) continue;float ΓΖ=(float) Ǫ.GetInventory(0).MaxVolume;float Ǌ=Ќ((float) Ǫ.GetInventory(0).CurrentVolume,ΓΖ);if(ȋ==null||Ǌ>ΒЂ){ȋ=Ǫ;ΒЂ=Ǌ;ΔЁ=ΓΖ;}
if(Ȥ==null||Ǌ<ΒА){Ȥ=Ǫ;ΒА=Ǌ;ΕΓ=ΓΖ;}}if(ȋ==null||Ȥ==null||ȋ==Ȥ) return;if(checkConveyorSystem&&!ȋ.GetInventory(0).IsConnectedTo(Ȥ.GetInventory(0))){if(Βδ>20) Ξ="Inventory balancing failed";else Βδ++;return;}Βδ=0;List<MyInventoryItem>Х=new List<MyInventoryItem>();
ȋ.GetInventory(0).GetItems(Х);float Ɛ=0;if(Х.Count==0) continue;MyInventoryItem ǋ=Х[0];String ΔЇ=ǋ.Type.TypeId+ǋ.Type.SubtypeId;if(!Επ.TryGetValue(ΔЇ,out Ɛ)){if(ΖΞ(ȋ.GetInventory(0),0,Ȥ.GetInventory(0),out Ɛ))
Επ.Add(ΔЇ,Ɛ);else return;}float ΗЇ=((ΒЂ-Δΰ)*ΔЁ/Ɛ);float ΗЈ=((Δΰ-ΒА)*ΕΓ/Ɛ);int Γθ=(int) Math.Min(ΗЈ,ΗЇ);if(Γθ<=0) return;if((float) ǋ.Amount<Γθ)
ȋ.GetInventory(0).TransferItemTo(Ȥ.GetInventory(0),0,null,null,ǋ.Amount);else
ȋ.GetInventory(0).TransferItemTo(Ȥ.GetInventory(0),0,null,null,Γθ);}}bool ΖΞ(IMyInventory Ɔ,int Β,IMyInventory ΖЋ,out float Εο){Εο=0;float Ζώ=(float) Ɔ.CurrentVolume;List<MyInventoryItem>Βζ=new List<MyInventoryItem>();Ɔ.GetItems(Βζ);float Δβ=0;for(int i=0;i<Βζ.Count;i++) Δβ+=(float) Βζ[i].Amount;
Ɔ.TransferItemTo(ΖЋ,Β,null,null,1);float Εξ=Ζώ-(float) Ɔ.CurrentVolume;List<MyInventoryItem>Βά=new List<MyInventoryItem>();Ɔ.GetItems(Βά);float ΔΧ=0;for(int i=0;i<Βά.Count;i++) ΔΧ+=(float) Βά[i].Amount;if(Εξ==0f||!ΑЊ(0.9999,Δβ-ΔΧ,1.0001))
return false;Εο=Εξ;return true;}void Ζμ(){Vector3 ų=ω+Αζ;Vector3 ǩ=ų-γ;if(ǩ.Length()==0) ǩ=new Vector3(0,0,-1);Vector3 ΑΝ=Ь(Υ,ǩ);Vector3 Εη=Vector3.Normalize(ΑΝ);Vector3 Ŀ=Ь(Υ,Υ.GetNaturalGravity());float Ηχ=ΓΞ>0?Math.Max(0,1-Ĥ(ǩ,Δϋ)/5):0;float Џ=(float) Math.Min((ΓΓ>0?ΓΓ:1000f),Math.Max(ƕ(-ΑΝ,Ŀ,null),ΓΞ*Ηχ));
if(!Ει) Џ=0;if(ΕΪ) Џ=Math.Max(0,1-ƽ/Ƶ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(ň) Џ=0;if(Ő>=0) Џ=Math.Min(Џ,Ő);if(ΕΫ) Џ*=(float) Math.Min(1,Ќ(ǩ.Length(),wpReachedDist)/2);Vector3 ΔБ=Ь(Υ,Υ.GetShipVelocities().LinearVelocity);float Ζϊ=(float)(Math.Max(0,15-Ĥ(-Εη,-ΔБ))/15)*0.85f+0.15f;
Γβ+=Math.Sign(Ζϊ-Γβ)/10f;Vector3 ȡ=Εη*Џ*Γβ-(ΔБ);Vector3 Βγ=ΑЄ(ȡ,null);if(Αγ&&З>0.1f){ȡ.X*=ΒΡ(ȡ.X,ref Αδ.X,1f,Βγ.X,20);ȡ.Y*=ΒΡ(ȡ.Y,ref Αδ.Y,1f,Βγ.Y,20);ȡ.Z*=ΒΡ(ȡ.Z,ref Αδ.Z,1f,Βγ.Z,20);}else Αδ=new Vector3(1,1,1);Ɗ=Ě*ȡ-Ŀ*Ě;Γσ(Ɗ,Αγ);З=Vector3.Distance(γ,ų);
}float ΒΡ(float value,ref float П,float ї,float ȑ,float ΓΔ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(П)) П+=Math.Sign(П)*ї;else П=value;if(value==0) П=1;float ά=Math.Abs(П);if(ά<ΓΔ||ȑ==0) return ά;П=Math.Min(ΓΔ,Math.Max(-ΓΔ,П));ά=Math.Abs(ȑ);return ά;
}void ΔΦ(){float ц=90,ћ=90,ѕ=90;float ǘ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǥ,ё,ż,ΒЍ;if(ΒЎ){Ǥ=Vector3.Normalize(ω-γ);ż=Ь(Υ,Ǥ);ё=Ь(Υ,Γΰ);ц=Ĥ(ż,new Vector3(0,-1,0))-90;ћ=Ă(ё,new Vector3(-1,0,0),ё.Y);
ѕ=Ă(ż,new Vector3(-1,0,0),ż.Z);}else{Ǥ=Γα;ё=Ь(Υ,Γΰ);ż=Ь(Υ,Γα);ΒЍ=Ь(Υ,Εθ);ц=Ă(ё,new Vector3(0,0,1),ё.Y);ћ=Ă(ё,new Vector3(-1,0,0),ё.Y);ѕ=Ă(ΒЍ,new Vector3(0,0,1),ΒЍ.X);}if(ΔΩ&&Ɵ()){Vector3 Βύ=Υ.GetNaturalGravity();ё=Ь(Υ,Βύ);ц=Ă(ё,new Vector3(0,0,1),ё.Y);
ћ=Ă(ё,new Vector3(-1,0,0),ё.Y);}if(!ΑЊ(-45,ћ,45)){ц=0;ѕ=0;}if(!ΑЊ(-45,ѕ,45)) ц=0;ƫ(ΒϏ,1,(-ц)*ǘ,(-ѕ)*ǘ,(-ћ)*ǘ);ƽ=Math.Max(Math.Abs(ц),Math.Max(Math.Abs(ћ),Math.Abs(ѕ)));ΑΖ=ƽ<=Ƶ;}void Εψ(IMyTerminalBlock ΑΩ){if(ΑΩ==null) return;Ǜ=new float[3,2];ј=new Dictionary<string,float[,]>();
for(int i=0;i<ǽ.Count;i++){IMyThrust Я=ǽ[i];if(!Я.IsFunctional) continue;Vector3 ι=Ь(ΑΩ,Я.WorldMatrix.Backward);float Ȣ=Я.MaxEffectiveThrust;if(Math.Round(ι.X,2)!=0.0)
if(ι.X>=0) Ǜ[0,0]+=Ȣ;else Ǜ[0,1]-=Ȣ;if(Math.Round(ι.Y,2)!=0.0)
if(ι.Y>=0) Ǜ[1,0]+=Ȣ;else Ǜ[1,1]-=Ȣ;if(Math.Round(ι.Z,2)!=0.0)
if(ι.Z>=0) Ǜ[2,0]+=Ȣ;else Ǜ[2,1]-=Ȣ;String щ=ΓЊ(Я);float[,]љ=null;if(ј.ContainsKey(щ)) љ=ј[щ];else{љ=new float[3,2];ј.Add(щ,љ);}float ȑ=Я.MaxThrust;if(Math.Round(ι.X,2)!=0.0)
if(ι.X>=0) љ[0,0]+=ȑ;else љ[0,1]-=ȑ;if(Math.Round(ι.Y,2)!=0.0)
if(ι.Y>=0) љ[1,0]+=ȑ;else љ[1,1]-=ȑ;if(Math.Round(ι.Z,2)!=0.0)
if(ι.Z>=0) љ[2,0]+=ȑ;else љ[2,1]-=ȑ;}}static String ΓЊ(IMyThrust Я){return Я.BlockDefinition.SubtypeId;}Vector3 ΑЍ(Vector3 ю,float[,]Ł){return new Vector3(
ю.X>=0?Ł[0,0]:Ł[0,1],
ю.Y>=0?Ł[1,0]:Ł[1,1],
ю.Z>=0?Ł[2,0]:Ł[2,1]);}bool ΓЉ(Χ Ψ,String щ,out float Βχ){Βχ=0;int Β=Ǽ.IndexOf(щ);if(Β==-1||Ψ.ǚ==null||Β>=Ψ.ǚ.Length)
return false;Βχ=Ψ.ǚ[Β];if(Βχ==-1) return false;return true;}Vector3 ΑЄ(Vector3 ю,Χ Ψ){if(Ψ!=null){Vector3 ά=new Vector3();for(int i=0;i<ј.Keys.Count;i++){String щ=ј.Keys.ElementAt(i);float ű=0;if(!ΓЉ(Ψ,щ,out ű))
return ΑЍ(ю,Ǜ);ά+=ΑЍ(ю,ј.Values.ElementAt(i))*ű;}return ά;}return ΑЍ(ю,Ǜ);}float ř(Vector3 ю,Χ Ψ){return ř(ю,new Vector3(),Ψ);}float ř(Vector3 ю,Vector3 κ,Χ Ψ){Vector3 ΗϏ=ΑЄ(ю,Ψ);Vector3 Ȣ=ΗϏ+κ*Ě;float Ɓ=(Ȣ/ю).AbsMin();return(float)(ю*Ɓ).Length();}float ƕ(Vector3 ΑΛ,Vector3 Ŀ,Χ Ψ)
{if(ΑΛ.Length()==0) return 0;float ΔΨ=1;if(Ŀ.Length()>0) ΔΨ=Math.Min(1,Ĥ(-Ŀ,ΑΛ)/90)*0.4f+0.6f;float Ɗ=ř(ΑΛ,Ŀ,Ψ);if(Ɗ==0) return 0.1f;float ƴ=Ќ(Ɗ,Ě);float Γε=(float) Math.Sqrt(Ќ(ΑΛ.Length(),ƴ*0.5f));return ƴ*Γε*ΔΨ*ǟ;}float Ɲ(Vector3 Ǥ,Vector3 І,Vector3 κ,Χ Ψ){if(κ.Length()==0f) return 0;
Vector3 ΑΜ=ć(Ǥ,І,Vector3.Normalize(κ));float ǐ=ř(-ΑΜ,Ψ);return ǐ/κ.Length();}void Γφ(bool ΗΜ){float њ=0;float Βϊ=0.9f;if(ΗΜ){Й=-1;Ğ=0;Αα=null;if(ή!=Λ.г&&Ε.κ.Length()!=0){њ=Βϊ*Ɲ(Ε.α,Ε.ΰ*-1,Ε.κ,null);if(њ<Й||Й==-1) Й=њ;}if(Ζ.β&&Ζ.κ.Length()!=0){њ=Βϊ*Ɲ(Ζ.α,Ζ.ΰ*-1,Ζ.κ,null);
if(њ<Й||Й==-1) Й=њ;}return;}if(Ğ==-1) return;if(Ğ>=0){int ΕΣ=0;while(Ğ<Τ.Count){if(ΕΣ>100) return;ΕΣ++;Χ Ψ=Τ[Ğ];if(Ψ.κ.Length()!=0f){њ=Βϊ*Math.Min(
Ɲ(Ψ.α,Ψ.ΰ*-1,Ψ.κ,Ψ),
Ɲ(Ψ.α*-1,Ψ.ΰ*-1,Ψ.κ,Ψ));if(њ<Й||Й==-1) Й=њ;}else Αα=Ψ;Ğ++;}Ğ=-1;}bool ΒЀ=true;float Ȭ=0;if(Αα!=null){for(int i=0;i<ј.Count;i++){String щ=ј.Keys.ElementAt(i);float[,]љ=ј.Values.ElementAt(i);float ű=0;if(!ΓЉ(Αα,щ,out ű)){ΒЀ=false;break;}for(int a=0;a<љ.GetLength(0);a++)
for(int b=0;b<љ.GetLength(1);b++){float Ċ=Math.Abs(љ[a,b]*ű);if(Ċ==0) continue;ΒЀ=true;if(Ȭ==0||Ċ<Ȭ) Ȭ=Ċ;}}}if(!ΒЀ){for(int a=0;a<Ǜ.GetLength(0);a++)
for(int b=0;b<Ǜ.GetLength(1);b++){float Ċ=Math.Abs(Ǜ[a,b]);if(Ċ==0) continue;if(Ȭ==0||Ċ<Ȭ) Ȭ=Ċ;}}if(Ȭ>0){float ΗΩ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;њ=Ќ(Ȭ,ΗΩ);if(њ>0)
if(њ<Й||Й==-1) Й=њ;}}void Γσ(Vector3 ǐ,bool Ї){if(!Ї){for(int i=0;i<ǽ.Count;i++)
ǽ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ββ=ΑЄ(ǐ,null);float ΘΕ=Math.Min(1,Math.Abs(Ќ(ǐ.X,Ββ.X)));float ΘΖ=Math.Min(1,Math.Abs(Ќ(ǐ.Y,Ββ.Y)));float ΘΗ=Math.Min(1,Math.Abs(Ќ(ǐ.Z,Ββ.Z)));for(int i=0;i<ǽ.Count;i++){IMyThrust Я=ǽ[i];Vector3 ι=ΖΨ(Ь(Υ,Я.WorldMatrix.Backward),1);
if(ι.X!=0&&Math.Sign(ι.X)==Math.Sign(ǐ.X))
Я.SetValueFloat("Override",Я.MaxThrust*ΘΕ);else if(ι.Y!=0&&Math.Sign(ι.Y)==Math.Sign(ǐ.Y))
Я.SetValueFloat("Override",Я.MaxThrust*ΘΖ);else if(ι.Z!=0&&Math.Sign(ι.Z)==Math.Sign(ǐ.Z))
Я.SetValueFloat("Override",Я.MaxThrust*ΘΗ);else
Я.SetValueFloat("Override",0.0f);}}void ƫ(bool Ї,float Ηε,float ц,float ѕ,float ћ){for(int i=0;i<ΑΑ.Count;i++){IMyGyro Ђ=ΑΑ[i];Ђ.GyroOverride=Ї;if(!Ї) Ђ.GyroPower=100;else Ђ.GyroPower=Ηε;if(!Ї) continue;Vector3 О=Υ.WorldMatrix.Forward;Vector3 ч=Υ.WorldMatrix.Right;Vector3 І=Υ.WorldMatrix.Up;
Vector3 ķ=Ђ.WorldMatrix.Forward;Vector3 Ĺ=Ђ.WorldMatrix.Up;Vector3 ĸ=Ђ.WorldMatrix.Left*-1;if(ķ==О) Ђ.SetValueFloat("Roll",ћ);else if(ķ==(О*-1)) Ђ.SetValueFloat("Roll",ћ*-1);else if(Ĺ==(О*-1)) Ђ.SetValueFloat("Yaw",ћ);else if(Ĺ==О) Ђ.SetValueFloat("Yaw",ћ*-1);else if(ĸ==О) Ђ.SetValueFloat("Pitch",ћ);
else if(ĸ==(О*-1)) Ђ.SetValueFloat("Pitch",ћ*-1);if(ĸ==(ч*-1)) Ђ.SetValueFloat("Pitch",ц);else if(ĸ==ч) Ђ.SetValueFloat("Pitch",ц*-1);else if(Ĺ==ч) Ђ.SetValueFloat("Yaw",ц);else if(Ĺ==(ч*-1)) Ђ.SetValueFloat("Yaw",ц*-1);else if(ķ==(ч*-1)) Ђ.SetValueFloat("Roll",ц);
else if(ķ==ч) Ђ.SetValueFloat("Roll",ц*-1);if(Ĺ==(І*-1)) Ђ.SetValueFloat("Yaw",ѕ);else if(Ĺ==І) Ђ.SetValueFloat("Yaw",ѕ*-1);else if(ĸ==І) Ђ.SetValueFloat("Pitch",ѕ);else if(ĸ==(І*-1)) Ђ.SetValueFloat("Pitch",ѕ*-1);else if(ķ==І) Ђ.SetValueFloat("Roll",ѕ);else if(ķ==(І*-1)) Ђ.SetValueFloat("Roll",ѕ*-1);
}}void ΔΣ(){this.ΒϏ=false;}void Щ(Vector3 Ů,Vector3 О,Vector3 ΒЌ,float Ȗ,bool Ə){Αμ(Ů,Ȗ,Ə);Ƶ=Ȗ;ΒЎ=false;this.Γα=О;this.Εθ=ΒЌ;}void Щ(Vector3 Ů,Vector3 О,Vector3 ΒЌ,bool Ə){Щ(Ů,О,ΒЌ,2f,Ə);}void Αμ(Vector3 Ů,float Ȗ,bool Ə){Ƶ=Ȗ;this.ΒϏ=true;this.ΔΩ=Ə;ΒЎ=true;ΑΖ=false;
this.Γΰ=Ů;}void ΒΧ(){ǝ(false,false,false,ω,0);Αγ=false;}void Э(Vector3 Л,float ǘ){ǝ(true,false,false,Л,ǘ);}void ǝ(bool Ї,bool Γά,bool Γμ,Vector3 Л,float ǘ){ǝ(Ї,Γά,Γμ,Л,Л-γ,0.0f,ǘ);}void ǝ(bool Ї,bool Γά,bool Γμ,Vector3 Л,Vector3 Ηρ,float Ης,float ǘ){Αγ=true;this.Ει=Ї;
ω=Л;this.ΓΓ=ǘ;this.ΓΞ=Ης;this.ΕΫ=Γά;this.ΕΪ=Γμ;this.Δϋ=Ηρ;З=Vector3.Distance(Л,γ);}void ΖΩ(){ψ.GetBlocksOfType(ť,ѝ);}void ΖΪ(){if(ΑΫ){ώ=2;return;}List<IMyRemoteControl>Ƃ=new List<IMyRemoteControl>();List<IMySensorBlock>ΕΨ=new List<IMySensorBlock>();List<IMyTerminalBlock>Εί=new List<IMyTerminalBlock>();
ψ.GetBlocksOfType(Ƃ,ѝ);ψ.GetBlocksOfType(ѐ,ѝ);ψ.GetBlocksOfType(ΕΨ,ѝ);ψ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Εί,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ѐ=ǜ(ѐ,pamTag,true);Ζΰ();Ζα(Εί);š(ѐ,setLCDFontAndSize,1.4f,false);š(ȱ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Εγ=ǜ(ΕΨ,pamTag,true);if(Εγ.Count>0) λ=Εγ[0];else λ=null;if(Κ==Θ.Š){ψ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ώ=1;Ξ="Drills are missing";}}else if(Κ==Θ.ќ){ψ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ώ=1;Ξ="Grinders are missing";}if(λ==null){ώ=1;Ξ="Sensor is missing";}}else if(Κ==Θ.Ϋ){ψ.GetBlocksOfType(Γζ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Εβ=ǜ(Ƃ,pamTag,true);if(Εβ.Count>0) Ƃ=Εβ;if(Ƃ.Count>0) Υ=Ƃ[0];else{Υ=null;ώ=2;Ξ="Remote is missing";return;}θ=(float) Υ.CubeGrid.WorldVolume.Radius*2;є=null;if(Κ!=Θ.Ϋ){Γχ();if(ο.Count>0&&є!=null){if(λ!=null&&(є.WorldMatrix.Forward!=λ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==λ.WorldMatrix.Up||Υ.WorldMatrix.Down==λ.WorldMatrix.Down))){ώ=1;Ξ="Wrong sensor direction";}if(є.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
є.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ώ=2;Ξ="Wrong remote direction";}}}}void ΖΫ(){ψ.GetBlocksOfType(ǻ,ѝ);ψ.GetBlocksOfType(п,ѝ);ψ.GetBlocksOfType(ǽ,ѝ);ψ.GetBlocksOfType(ΑΑ,ѝ);ψ.GetBlocksOfType(ȟ,ѝ);ψ.GetBlocksOfType(ȯ,ѝ);ψ.GetBlocksOfType(я,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ψ.GetBlocksOfType(Αΰ,ѝ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
п=ǜ(п,"ConnectorMedium",false);else
п=ǜ(п,"Connector",false);List<IMyShipConnector>Εΰ=ǜ(п,pamTag,true);if(Εΰ.Count>0) п=Εΰ;if(ώ<=1){if(п.Count==0){ώ=1;Ξ="Connector is missing";}if(ΑΑ.Count==0){ώ=1;Ξ="Gyros are missing";}if(ǽ.Count==0){ώ=1;Ξ="Thrusters are missing";}}List<IMyConveyorSorter>Εδ=ǜ(Αΰ,pamTag,true);
if(Εδ.Count>0) Αΰ=Εδ;List<IMyLandingGear>Εα=ǜ(ǻ,pamTag,true);if(Εα.Count>0) ǻ=Εα;for(int i=0;i<ǻ.Count;i++) ǻ[i].AutoLock=false;List<IMyBatteryBlock>Εή=ǜ(ȟ,pamTag,true);if(Εή.Count>0) ȟ=Εή;List<IMyGasTank>Εε=ǜ(я,pamTag,true);if(Εε.Count>0) я=Εε;}void Ζά(){ψ.GetBlocksOfType(ċ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ĳ.Clear();for(int i=ċ.Count-1;i>=0;i--){if(ΖΘ(ċ[i])){ĳ.Add(ċ[i]);ċ.RemoveAt(i);}}}bool ΖΘ(IMyTerminalBlock Σ){if(Σ is IMyCargoContainer) return true;if(Σ is IMyShipDrill) return true;if(Σ is IMyShipGrinder) return true;if(Σ is IMyShipConnector){if(((IMyShipConnector) Σ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΑЈ(Σ,"ConnectorSmall",false))
return false;else return true;}return false;}void Γχ(){if(Υ==null) return;є=null;float ΑΤ=0,ΑΞ=0,ΑΥ=0,ΑΟ=0,ΑΦ=0,ΑΠ=0;List<IMyTerminalBlock>Γί=ǜ(ο,pamTag,true);bool Ηΰ=Γί.Count==0;if(Γί.Count>0) є=Γί[0];else if(ο.Count>0) є=ο[0];int Ǻ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=є.WorldMatrix.Forward){if(Ηΰ){ώ=2;Ξ="Mining direction is unclear!";return;}continue;}Ǻ++;Vector3 Ј=ΑЁ(Υ,ο[i].GetPosition());if(i==0){ΑΤ=Ј.X;ΑΞ=Ј.X;ΑΥ=Ј.Y;ΑΟ=Ј.Y;ΑΦ=Ј.Z;ΑΠ=Ј.Z;}ΑΞ=Math.Max(Ј.X,ΑΞ);ΑΤ=Math.Min(Ј.X,ΑΤ);ΑΟ=Math.Max(Ј.Y,ΑΟ);ΑΥ=Math.Min(Ј.Y,ΑΥ);
ΑΠ=Math.Max(Ј.Z,ΑΠ);ΑΦ=Math.Min(Ј.Z,ΑΦ);}Ǿ=(ΑΞ-ΑΤ)*(1-ğ)+drillRadius*2;ǫ=(ΑΟ-ΑΥ)*(1-Ȧ)+drillRadius*2;if(є!=null&&є.WorldMatrix.Forward==Υ.WorldMatrix.Down)
ǫ=(ΑΠ-ΑΦ)*(1-Ȧ)+drillRadius*2;}List<T>ǜ<T>(List<T>з,String Ǚ,bool ΑΒ){List<T>ά=new List<T>();for(int i=0;i<з.Count;i++)
if(ΑЈ(з[i],Ǚ,ΑΒ)) ά.Add(з[i]);return ά;}bool ΑЈ<T>(T Σ,String Ǚ,bool ΑΒ){IMyTerminalBlock b=(IMyTerminalBlock) Σ;if(ΑΒ&&b.CustomName.ToUpper().Contains(Ǚ.ToUpper())) return true;if(!ΑΒ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǚ.ToUpper())) return true;return false;}void ь<T>(List<T>з,bool Ї)
{for(int i=0;i<з.Count;i++)
ΔΝ((IMyTerminalBlock) з[i],Ї);}void ΔΝ(IMyTerminalBlock Σ,bool Ї){if(Σ==null) return;String Ζψ=Ї?"OnOff_On":"OnOff_Off";var a=Σ.GetActionWithName(Ζψ);a.Apply(Σ);}void Ǟ(bool Ї){Γέ=Ї;if(!ě) return;ь(Αΰ,Ї);}void ΒΝ(ChargeMode Ž){for(int i=0;i<ȟ.Count;i++) ȟ[i].ChargeMode=Ž;}void ΒΞ(bool Ηϊ)
{for(int i=0;i<я.Count;i++) я[i].Stockpile=Ηϊ;}void Ţ(List<IMyLandingGear>Βϋ,bool ΕΑ){for(int i=0;i<Βϋ.Count;i++){if(ΕΑ) Βϋ[i].Lock();if(!ΕΑ) Βϋ[i].Unlock();}}void ΔΞ(bool Ї){if(Δξ==Ї) return;List<IMyShipController>ƻ=new List<IMyShipController>();ψ.GetBlocksOfType(ƻ,ѝ);if(ƻ.Count==0) return;
for(int i=0;i<ƻ.Count;i++)
ƻ[i].DampenersOverride=Ї;Δξ=Ї;}IMyShipConnector Ъ(MyShipConnectorStatus ш){for(int i=0;i<п.Count;i++){if(!ǲ(п[i],true)) continue;if(п[i].Status==ш) return п[i];}return null;}void Ĉ(bool ΓΩ,bool ǁ,float ΗΒ,float Ζύ){if(λ==null||ο.Count==0) return;Vector3 ƺ=new Vector3();int Ǻ=0;
for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=є.WorldMatrix.Forward) continue;Ǻ++;ƺ+=ο[i].GetPosition();}ƺ=ƺ/Ǻ;Vector3 Ņ=ΑЁ(λ,ƺ);λ.Enabled=true;λ.ShowOnHUD=ΓΩ;λ.LeftExtend=(ǁ?1:е)/2f*Ǿ-Ņ.X;λ.RightExtend=(ǁ?1:е)/2f*Ǿ+Ņ.X;λ.TopExtend=(ǁ?1:д)/2f*ǫ+Ņ.Y;
λ.BottomExtend=(ǁ?1:д)/2f*ǫ-Ņ.Y;λ.FrontExtend=(ΓΩ?Т:ΗΒ)-Ņ.Z;λ.BackExtend=ΓΩ?0:Ζύ+θ*0.75f+Ņ.Z;λ.DetectFloatingObjects=true;λ.DetectAsteroids=false;λ.DetectLargeShips=true;λ.DetectSmallShips=true;λ.DetectStations=true;λ.DetectOwner=true;λ.DetectSubgrids=false;
λ.DetectPlayers=false;λ.DetectEnemy=true;λ.DetectFriendly=true;λ.DetectNeutral=true;}void š<T>(List<T>з,bool Ητ,float ΖБ,bool ΗΝ){for(int i=0;i<з.Count;i++){IMyTextSurface ƈ=null;if(з[i]is IMyTextSurface) ƈ=(IMyTextSurface) з[i];if(ƈ!=null){ƈ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ητ) continue;ƈ.Font="Debug";if(ΗΝ) continue;ƈ.FontSize=ΖБ;}}}bool Ɵ(){Vector3D ζ;return this.Υ.TryGetPlanetPosition(out ζ);}int ΖΑ(Vector3 Ηό,int Εά){if(Εά==-1) return 0;double А=-1;int Ȋ=-1;for(int i=Τ.Count-1;i>=0;i--){double ό=Vector3.Distance(Τ[i].Δ,Ηό);if(А==-1||ό<А){Ȋ=i;А=ό;}
}return Math.Sign(Ȋ-Εά);}bool ΖΚ(Vector3 ζ){List<Vector3>Д=new List<Vector3>();for(int i=0;i<Τ.Count;i++) Д.Add(Τ[i].Δ);if(Ζ.β&&Τ.Count>=1){Vector3 Į=new Vector3();Ǳ(Ζ,dockDist*θ,false,out Į);if(Vector3.Distance(Ζ.Δ,Τ.First().Δ)<Vector3.Distance(Ζ.Δ,Τ.Last().Δ)){Д.Insert(0,Į);Д.Insert(0,Ζ.Δ);}
else{Д.Add(Į);Д.Add(Ζ.Δ);}}if(Κ==Θ.Ϋ){if(Ε.β&&Τ.Count>=1){Vector3 Βή=new Vector3();Ǳ(Ε,dockDist*θ,false,out Βή);if(Vector3.Distance(Ε.Δ,Τ.First().Δ)<Vector3.Distance(Ε.Δ,Τ.Last().Δ)){Д.Insert(0,Βή);Д.Insert(0,Ε.Δ);}else{Д.Add(Βή);Д.Add(Ε.Δ);}}}else{if(ή!=Λ.г)
if(Τ.Count>0&&Vector3.Distance(Ε.Δ,Τ.First().Δ)<Vector3.Distance(Ε.Δ,Τ.Last().Δ))
Д.Insert(0,Ε.Δ);else Д.Add(Ε.Δ);}int Ȋ=-1;double ǡ=-1;for(int i=0;i<Д.Count;i++){double ό=Vector3.Distance(Д[i],ζ);if(ό<ǡ||ǡ==-1){ǡ=ό;Ȋ=i;}}if(Д.Count==0) return false;double Βτ=Vector3.Distance(Д[Ȋ],ζ);double Ηζ=Vector3.Distance(Д[Math.Max(0,Ȋ-1)],Д[Ȋ])*1.5f;
double Ηί=Vector3.Distance(Д[Math.Min(Д.Count-1,Ȋ+1)],Д[Ȋ])*1.5f;return Βτ<Ηζ||Βτ<Ηί||Βτ<laneOffset*θ*1.5f;}static float Ќ(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΖΨ(Vector3 v,int Βρ){return new Vector3(Math.Round(v.X,Βρ),Math.Round(v.Y,Βρ),Math.Round(v.Z,Βρ));
}float Ĥ(Vector3 a,Vector3 b){if(a==b) return 0;float ΖА=(a*b).Sum;float ΔЌ=a.Length();float ΔЍ=b.Length();if(ΔЌ==0||ΔЍ==0) return 0;float ά=(float)((180.0f/Math.PI)*Math.Acos(ΖА/(ΔЌ*ΔЍ)));if(float.IsNaN(ά)) return 0;return ά;}float Ă(Vector3 v,Vector3 ΑΩ,float Ηυ){float Ĭ=Ĥ(v,ΑΩ);
if(Ηυ>0f) Ĭ*=-1;if(Ĭ>-90f) return Ĭ-90f;else return 180f-(-Ĭ-90f);}double Γύ(float ΖЉ){return(Math.PI/180)*ΖЉ;}bool ΑЊ(double ΗΨ,double value,double ΗΦ){return(value>=ΗΨ&&value<=ΗΦ);}Vector3 Ζι(IMyTerminalBlock Σ,Vector3 Ј){return Vector3D.Transform(Ј,Σ.WorldMatrix);
}Vector3 ΑЁ(IMyTerminalBlock Σ,Vector3 ΘΒ){return Ь(Σ,ΘΒ-Σ.GetPosition());}Vector3 Ь(IMyTerminalBlock Σ,Vector3 Γρ){return Vector3D.TransformNormal(Γρ,MatrixD.Transpose(Σ.WorldMatrix));}Vector3 ć(Vector3 Ǥ,Vector3 І,Vector3 Γρ){MatrixD ΓΒ=MatrixD.CreateFromDir(Ǥ,І);return Vector3D.TransformNormal(Γρ,MatrixD.Transpose(ΓΒ));
}Vector3 ΒΨ(Vector3 Ǥ,Vector3 І,Vector3 Ј){MatrixD ΓΒ=MatrixD.CreateFromDir(Ǥ,І);return Vector3D.Transform(Ј,ΓΒ);}String υ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 τ(String s){String[]Г=s.Split('|');return new Vector3(float.Parse(Ι(Г,0)),float.Parse(Ι(Г,1)),float.Parse(Ι(Г,2)));
}String Ζφ(Χ Ψ){String Њ=":";String ά=υ(Ψ.Δ)+Њ+υ(Ψ.α)+Њ+
υ(Ψ.ΰ)+Њ+υ(Ψ.χ)+Њ+υ(Ψ.κ);for(int i=0;i<Ψ.ǚ.Length;i++)
ά+=Њ+Math.Round(Ψ.ǚ[i],3);return ά;}Χ Ζζ(String s){String[]Г=s.Split(':');Χ Ψ=new Χ(
τ(Ι(Г,0)),τ(Ι(Г,1)),
τ(Ι(Г,2)),τ(Ι(Г,3)),
τ(Ι(Г,4)));int Β=5;List<float>Δχ=new List<float>();while(Β<Г.Length){String Αθ=Ι(Г,Β);float f=0;if(!float.TryParse(Αθ,out f)) break;Δχ.Add(f);Β++;}Ψ.ǚ=Δχ.ToArray();return Ψ;}System.Text.StringBuilder ŋ=new System.Text.StringBuilder();void Π<T>(T value,bool Ηή){if(Ηή) ŋ.Append('\n');ŋ.Append(value);}
void Π<T>(T value){Π(value,true);}String ƙ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ư(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ι(String[]Ζό,int Β){String Αθ=Ζό.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Αθ)) return "";
return Αθ;}void Save(){if(ΑΫ||Κ==Θ.м){Storage="";return;}Storage=Ας();}String Ας(){ŋ.Clear();ŋ.Append(DATAREV+";");Π(υ(ΑΪ),false);Π(υ(Ζ.α));Π(υ(Ζ.χ));Π(υ(Ζ.ΰ));Π(υ(Ζ.κ));Π(υ(Ζ.Δ));Π(υ(Ζ.У));Π(Ζ.β);Π(υ(Ε.Δ));Π(υ(Ε.κ));Π(υ(Ε.α));Π(υ(Ε.ΰ));Π(υ(Ε.χ));Π(υ(Ε.У));Π(Ε.β);Π(υ(Ǖ));Π(υ(Ȓ));Π(";");
Π((int) Κ,false);Π((int) ή);Π((int) т);Π(ǥ);Π(у);Π(Ц);Π(ф);Π(ǟ);Π(Ș);Π(ȗ);if(Κ==Θ.Ϋ){Π((int) ϊ.Ћ);Π(ϊ.ǎ);Π(ϊ.ȣ);Π(ƙ(ϊ.Ǐ));Π(ƙ(ϊ.ǒ));Π((int) ϋ.Ћ);Π(ϋ.ǎ);Π(ϋ.ȣ);Π(ƙ(ϋ.Ǐ));Π(ƙ(ϋ.ǒ));}else{Π((int) ǧ);Π((int) и);Π((int) И);Π((int) р);Π((int) Ź);Π(Ȳ);Π(ě);Π(ļ);Π(Ķ);Π(е);Π(д);Π(Т);Π(Ŕ);Π(œ);Π(ğ);Π(Ȧ);
Π(Ѓ);Π(Ў);Π(φ);Π(ρ);Π(Ȑ);Π(М);Π(Ę);Π(ė);}Π(";");for(int i=0;i<Ǽ.Count;i++)
Π((i>0?"|":"")+Ǽ[i],false);Π(";");for(int i=0;i<Τ.Count;i++)
Π(Ζφ(Τ[i]),i>0);Π(";",false);Π(с+"|"+ǖ,false);Π(";",false);Π(ΕЅ(),false);String ί=ŋ.ToString();ŋ.Clear();return ί;}н ΖΧ(){return Ƨ(Storage);}н Ƨ(String ί){if(ί=="") return н.ΖΡ;String[]ę=ί.Split(';');if(Ι(ę,0)!=DATAREV) return н.ΑА;int Β=0;try{String[]Η=Ι(ę,1).Split('\n');
ΑΪ=τ(Ι(Η,Β++));Ζ.α=τ(Ι(Η,Β++));Ζ.χ=τ(Ι(Η,Β++));Ζ.ΰ=τ(Ι(Η,Β++));Ζ.κ=τ(Ι(Η,Β++));Ζ.Δ=τ(Ι(Η,Β++));Ζ.У=τ(Ι(Η,Β++));Ζ.β=bool.Parse(Ι(Η,Β++));Ε.Δ=τ(Ι(Η,Β++));Ε.κ=τ(Ι(Η,Β++));Ε.α=τ(Ι(Η,Β++));Ε.ΰ=τ(Ι(Η,Β++));Ε.χ=τ(Ι(Η,Β++));Ε.У=τ(Ι(Η,Β++));Ε.β=bool.Parse(Ι(Η,Β++));
Ǖ=τ(Ι(Η,Β++));Ȓ=τ(Ι(Η,Β++));Η=Ι(ę,2).Split('\n');Β=0;Κ=(Θ) int.Parse(Ι(Η,Β++));ή=(Λ) int.Parse(Ι(Η,Β++));т=(Λ) int.Parse(Ι(Η,Β++));ǥ=int.Parse(Ι(Η,Β++));у=int.Parse(Ι(Η,Β++));Ц=int.Parse(Ι(Η,Β++));ф=int.Parse(Ι(Η,Β++));ǟ=float.Parse(Ι(Η,Β++));Ș=bool.Parse(Ι(Η,Β++));
ȗ=bool.Parse(Ι(Η,Β++));if(Κ==Θ.Ϋ){ϊ.Ћ=(ε) int.Parse(Ι(Η,Β++));ϊ.ǎ=float.Parse(Ι(Η,Β++));ϊ.ȣ=float.Parse(Ι(Η,Β++));ϊ.Ǐ=ư(Ι(Η,Β++));ϊ.ǒ=ư(Ι(Η,Β++));ϋ.Ћ=(ε) int.Parse(Ι(Η,Β++));ϋ.ǎ=float.Parse(Ι(Η,Β++));ϋ.ȣ=float.Parse(Ι(Η,Β++));ϋ.Ǐ=ư(Ι(Η,Β++));ϋ.ǒ=ư(Ι(Η,Β++));}else{ǧ=(ў) int.Parse(Ι(Η,Β++));
и=(б) int.Parse(Ι(Η,Β++));И=(Ϗ) int.Parse(Ι(Η,Β++));р=(ъ) int.Parse(Ι(Η,Β++));Ź=(ў) int.Parse(Ι(Η,Β++));Ȳ=bool.Parse(Ι(Η,Β++));ě=bool.Parse(Ι(Η,Β++));ļ=bool.Parse(Ι(Η,Β++));Ķ=bool.Parse(Ι(Η,Β++));е=int.Parse(Ι(Η,Β++));д=int.Parse(Ι(Η,Β++));Т=int.Parse(Ι(Η,Β++));Ŕ=float.Parse(Ι(Η,Β++));
œ=float.Parse(Ι(Η,Β++));ğ=float.Parse(Ι(Η,Β++));Ȧ=float.Parse(Ι(Η,Β++));Ѓ=int.Parse(Ι(Η,Β++));Ў=int.Parse(Ι(Η,Β++));φ=int.Parse(Ι(Η,Β++));ρ=int.Parse(Ι(Η,Β++));Ȑ=int.Parse(Ι(Η,Β++));М=int.Parse(Ι(Η,Β++));Ę=int.Parse(Ι(Η,Β++));ė=float.Parse(Ι(Η,Β++));}Η=Ι(ę,3).Replace("\n","").Split('|');
Ǽ=Η.ToList();Η=Ι(ę,4).Split('\n');Τ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
Τ.Add(Ζζ(Ι(Η,i)));Η=Ι(ę,5).Split('|');с=Ι(Η,0);if(!int.TryParse(Ι(Η,1),out ǖ)) ǖ=-1;ΕЄ(Ι(ę,6));}catch{return н.ΓЂ;}Ō=т;σ();return н.ƭ;}const int Γυ=64000;IMyTerminalBlock ΓЃ(){List<IMyTerminalBlock>ı=new List<IMyTerminalBlock>();ψ.GetBlocksOfType(ı,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ı.Count>0?ı[0]:null;}void ΕЉ(){IMyTerminalBlock Σ=ΓЃ();if(Σ==null){Ξ="Export: name a block "+dataTag;return;}String ί="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Τ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Ας();if(ί.Length>Γυ){Ξ="Export too large: "+ί.Length+" chars, max "+Γυ;return;}Σ.CustomData=ί;Ξ="Exported "+Τ.Count+" waypoints";}void ΖΔ(bool ΒЇ){IMyTerminalBlock Σ=ΓЃ();if(Σ==null){Ξ="Import: name a block "+dataTag;return;}
ΖΕ(Σ.CustomData,ΒЇ);}void ΖΕ(String ί,bool ΒЇ){ί=ί.Replace("\r","");if(ί.StartsWith("//")){int ΕΜ=ί.IndexOf('\n');ί=ΕΜ<0?"":ί.Substring(ΕΜ+1);}ί=ί.Trim();if(ί==""){Ξ="Import: no PAM data in "+dataTag;return;}σ();String Δα=Ας();н ά=Ƨ(ί);if(ά==н.ƭ&&ΒЇ){Χ ľ=new Χ(Ε);
Vector3 ΗΫ=Ǖ,ΗΪ=Ȓ;int w=е,h=д,d=Т;ў Ηψ=ǧ;ъ Βς=р;float Ηγ=ğ,Ηβ=Ȧ;Ƨ(Δα);Ε=ľ;Ǖ=ΗΫ;Ȓ=ΗΪ;е=w;д=h;Т=d;ǧ=Ηψ;р=Βς;ğ=Ηγ;Ȧ=Ηβ;ή=Λ.Ĩ;ţ(true,true);ή=Λ.Ĩ;т=Λ.Ĩ;Ξ="Imported job, run CONT";return;}if(ά==н.ƭ){ž=false;Ξ="Imported "+Τ.Count+" waypoints";
return;}Ƨ(Δα);Ξ=ά==н.ΑА?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζΰ(){Ŭ.Clear();for(int i=ѐ.Count-1;i>=0;i--){String ί=ѐ[i].CustomData.ToUpper();bool ΒЅ=false;if(ί=="INSTRUCTIONS"){ΒЅ=true;ǀ=true;}if(ί=="DEBUG") ΒЅ=true;if(ΒЅ){Ŭ.Add(ѐ[i]);ѐ.RemoveAt(i);}
}š(Ŭ,false,1,false);}void Ζα(List<IMyTerminalBlock>ı){ȱ.Clear();for(int i=0;i<ı.Count;i++){IMyTerminalBlock Σ=ı[i];try{String ΕΡ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Σ.CustomName.IndexOf(ΕΡ);int ō=-1;if(Β<0||!int.TryParse(Σ.CustomName.Substring(Β+ΕΡ.Length,1),out ō)) continue;
if(ō==-1) continue;ō--;IMyTextSurfaceProvider ΕΤ=(IMyTextSurfaceProvider) Σ;if(ō<ΕΤ.SurfaceCount&&ō>=0)
ȱ.Add(ΕΤ.GetSurface(ō));}catch{}}}String ΑΡ="";void ΔΙ(){String ΓΑ="";String Βφ="";ΑΡ=Γτ(false);ΓΑ+=ΑΡ;Βφ+=ΑΡ;Βφ+=ΕЎ();for(int i=0;i<ѐ.Count;i++) ѐ[i].WriteText(ΓΑ);for(int i=0;i<ȱ.Count;i++) ȱ[i].WriteText(ΓΑ);Echo(Βφ);for(int i=0;i<Ŭ.Count;i++){IMyTextPanel ΓΝ=Ŭ[i];
String ί=ΓΝ.CustomData.ToUpper();if(ί=="DEBUG") ΓΝ.WriteText(""+"\n"+"");if(ί=="INSTRUCTIONS") ΓΝ.WriteText(ΕЏ());}}string ΕЏ(){String Γ="";try{float ΕΔ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(İ*ΕΔ)+" / "+(int)(ł*ΕΔ)+"\n";Γ+="Inst. avg/max: "+Math.Round(İ*100f,1)+"% / "+Math.Round(ł*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ŧ,2)+"ms / "+Math.Round(ΓΕ,2)+"ms \n";}catch{}for(int i=0;i<ȓ.Count;i++)
Γ+=""+ȓ.Keys.ElementAt(i)+" = "+Math.Round(ȓ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȓ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΖΓ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";š(ѐ,setLCDFontAndSize,0.9f,false);š(ȱ,setLCDFontAndSize,0.9f,true);for(int i=0;i<ѐ.Count;i++) ѐ[i].WriteText(Γ+ȏ.ToString());for(int i=0;i<ȱ.Count;i++) ȱ[i].WriteText(Γ+ȏ.ToString());}void Ǵ(){if(!ǀ) return;try{ΓΣ=Runtime.CurrentInstructionCount;}catch{}
}void ǰ(String ς){if(!ǀ) return;if(ΓΣ==0) return;try{float Αη=(Runtime.CurrentInstructionCount-ΓΣ)/Runtime.MaxInstructionCount*100;if(!ȓ.ContainsKey(ς))
ȓ.Add(ς,new float[]{Αη,Αη});else{ȓ[ς][0]=Αη;ȓ[ς][1]=Math.Max(Αη,ȓ[ς][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ευ(String ȥ){if(!Ĳ) return;String Њ=""+BROADCAST_SEP;String ν=VERSION+Њ;ν+=ŕ(ΓΨ)+Њ;
ν+=(int) Κ+Њ;ν+=Αύ(γ.X)+""+Њ;ν+=Αύ(γ.Y)+Њ;ν+=Αύ(γ.Z)+Њ;if(Κ!=Θ.Ϋ)
ν+=ΑЀ(т)+(т==Λ.Љ?" "+Math.Round(Ÿ,1)+"%":"")+Њ;else ν+=ΑЀ(т)+Њ;ν+=ŕ(Ξ)+Њ;ν+=ђ+""+Њ;ν+=ŕ(ΑΡ)+Њ;ν+=Ĵ+Њ;ν+=Ń+Њ;for(int i=0;i<Ǒ.Count;i++){if(Ǒ[i].ŀ!=Е.ś) continue;ν+=ŕ(Ǒ[i].ς)+"/"+ŕ(Ǒ[i].ξ)+"/"+Ǒ[i].џ+Њ;}Ζή(ν,ȥ);}String ŕ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ζή(String ν,String ȥ){Ζί(ν,BROADCAST_SELF_ID,ȥ);}void Ζί(String ν,String Л,String ȥ){try{if(!Ĳ) return;ν=BROADCAST_TAG+BROADCAST_SEP+ΓЅ()+BROADCAST_SEP+Л+BROADCAST_SEP+Βθ+BROADCAST_SEP+ȥ+BROADCAST_SEP+ν;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ν);}catch(Exception e){ȏ=e;}
}String ΓЅ(){if(Κ!=Θ.м) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔΒ(String ν){return ν.StartsWith(BROADCAST_TAG);}bool ŗ(ref String ί,out string Δψ,Char Њ){int Β=ί.IndexOf(Њ);Δψ="";if(Β<0) return false;Δψ=ί.Substring(0,Β);ί=ί.Remove(0,Β+1);return true;}bool ΖΣ(ref String ν,out String Ƅ,out String ȥ)
{Ƅ="";ȥ="";if(!Ĳ) return false;String Ǚ="";if(!ŗ(ref ν,out Ǚ,BROADCAST_SEP)||!ΔΒ(Ǚ)) return false;if(!ŗ(ref ν,out Ƅ,BROADCAST_SEP)) return false;if(!ŗ(ref ν,out Ǚ,BROADCAST_SEP)||(Ǚ!=ΓЅ()&&(Ǚ!="*"&&Κ!=Θ.м))) return false;if(!ŗ(ref ν,out Ǚ,BROADCAST_SEP)||(Ǚ!=Βθ)) return false;
if(!ŗ(ref ν,out ȥ,BROADCAST_SEP)) return false;return true;}void ΖΗ(){bool ū=true;if(Δϊ){Δϊ=false;if(Me.CustomData.Contains("Antenna_Name")){Ξ="Update custom data";Me.CustomData="";}}String Ŵ=(Κ!=Θ.м?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ŵ)
ΓЏ(Ŵ);String[]Η=Me.CustomData.Split('\n');Ĳ=bool.Parse(ΒΙ(Αϋ(Η,"Enable_Broadcast",ref ū)));bool Γο=false;bool Δς=true;if(Ĳ){if(Κ!=Θ.м)
ΓΨ=ΒΙ(Αϋ(Η,"Ship_Name",ref ū)).Replace(BROADCAST_SELF_ID,"");Βθ=ΒΙ(Αϋ(Η,"Broadcast_Channel",ref ū)).ToLower();Δς=false;if(ȉ==null){ȉ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȉ.SetMessageCallback("");}List<IMyRadioAntenna>ĭ=new List<IMyRadioAntenna>();ψ.GetBlocksOfType(ĭ);bool Βέ=false;
for(int i=0;i<ĭ.Count;i++)
if(ĭ[i].EnableBroadcasting&&ĭ[i].Enabled){Βέ=true;break;}if(ĭ.Count==0) Ξ="No Antenna found";else if(!Βέ) Ξ="Antenna not ready";Γο=ĭ.Count==0||!Βέ;if(ΔΪ&&!Γο&&Κ!=Θ.м) Ξ="Antenna ok";}else if(Κ==Θ.м)
Ξ="Offline - Enable in PB custom data";ΔΪ=Γο;if(Δς){if(ȉ!=null) this.IGC.DisableBroadcastListener(ȉ);ȉ=null;}}catch{ū=false;}if(!ū){Ξ="Reset custom data";ΓЏ(Ŵ);}}void ΓЏ(String Ŵ){String ί=Ŵ+"\n\n"+"Enable_Broadcast="+(Κ==Θ.м?"true":"false")+" \n";ί+=Κ!=Θ.м?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=ί+"Broadcast_Channel=#default";}String ΒΙ(String Ė){int Β=Ė.IndexOf("//");if(Β!=-1) Ė=Ė.Substring(0,Β);String[]Г=Ė.Split('=');if(Г.Length<=1) return "";return Г[1].Trim();}String Αϋ(String[]Η,String ΒЈ,ref bool ſ){foreach(String Ė in Η)
if(Ė.StartsWith(ΒЈ)) return Ė;ſ=false;return "";}String ΔΜ(int Γξ,String Γ,int ΕΧ,ref int Ȕ){String[]Η=Γ.Split('\n');if(ΕΧ>=Ȕ+Γξ-1) Ȕ++;Ȕ=Math.Min(Η.Count()-1-Γξ,Ȕ);if(ΕΧ<Ȕ+1) Ȕ--;Ȕ=Math.Max(0,Ȕ);String ά="";for(int i=0;i<Γξ;i++){int ΔЏ=i+Ȕ;if(ΔЏ>=Η.Count()) break;
ά+=Η[ΔЏ]+"\n";}return ά;}String Αώ(int ƃ){if(ƃ>=60*60) return Math.Round(ƃ/(60f*60f),1)+" h";if(ƃ>=60) return Math.Round(ƃ/60f,1)+" min";return ""+ƃ+" s";}string Αύ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>Ƿ=new List<IMyCameraBlock>();List<long>ņ=new List<long>();
bool ň=false;int Βν=0;int Βλ=0;DateTime Δδ=DateTime.Now;String Βη="";void Ζέ(){Ƿ.Clear();ņ.Clear();if(!obstacleBrake) return;ψ.GetBlocksOfType(Ƿ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<Ƿ.Count;i++){Ƿ[i].EnableRaycast=true;if(!ņ.Contains(Ƿ[i].CubeGrid.EntityId)) ņ.Add(Ƿ[i].CubeGrid.EntityId);
}if(!ņ.Contains(Me.CubeGrid.EntityId)) ņ.Add(Me.CubeGrid.EntityId);}bool ΔΑ(Α s){return s==Α.ȁ||s==Α.Ы||s==Α.С||
s==Α.Ȝ||s==Α.Ħ||s==Α.Ř||s==Α.ƛ||
(s==Α.Ǭ&&Ǧ!=null);}void Ɨ(){ň=false;Βν=0;Βη="";}void Ζπ(){if(!obstacleBrake||Ƿ.Count==0||!ΔΑ(Φ)){Ɨ();return;}Vector3 ų=ω+Αζ;Vector3 ǩ=ų-γ;float Ǩ=ǩ.Length();if(Ǩ<1f){Ɨ();return;}Vector3 ι=ǩ/Ǩ;float ΕΒ=ȕ*obstacleLookAhead+θ;Vector3 Ηι=ų+ι*(θ*0.25f);
for(int n=0;n<Ƿ.Count;n++){Βλ=(Βλ+1)%Ƿ.Count;IMyCameraBlock Ū=Ƿ[Βλ];if(!Ū.IsFunctional||!Ū.IsWorking) continue;Vector3 Δζ=Ū.GetPosition();Vector3 ǈ=Ηι;if(Vector3.Distance(Δζ,ǈ)>ΕΒ) ǈ=Δζ+ι*ΕΒ;if(!Ū.CanScan(ǈ)) continue;MyDetectedEntityInfo ŵ=Ū.Raycast(ǈ);bool Ηα=!ŵ.IsEmpty()&&!ņ.Contains(ŵ.EntityId)&&!ΖΝ(ŵ.EntityId)&&
ŵ.Type!=MyDetectedEntityType.FloatingObject;if(Ηα){if(!ň) Δδ=DateTime.Now;ň=true;Βν=0;Βη=ŵ.Type.ToString();}else if(ň&&++Βν>=3) Ɨ();break;}if(ň&&(DateTime.Now-Δδ).TotalSeconds>obstacleTimeout)
Ξ="Path blocked: "+Βη;}const String ΔΥ="[PAMTFC]";const float Ζη=5f;const float ΕЊ=2f;class Ī{public long ύ;public Vector3 ζ,Γν;public float Δρ;public String ľ="";public int К=-1;public long Ϊ=0;public int ǂ=0;public DateTime Γδ;}IMyBroadcastListener Αε=null;long ΑΧ=0;List<Ī>Р=new List<Ī>();
Vector3 Αζ=new Vector3();float Ő=-1;int Εμ=0;String с="";int ǖ=-1;bool Ƽ=false;Vector3 Δκ=new Vector3();String ΑЌ(){if(ή==Λ.г||Κ==Θ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Ѓ+"|"+Ў+"|"+(int) Ź+"|"+Math.Round(Ǿ,1)+"|"+Math.Round(ǫ,1);}void Ζσ(){if(!trafficEnabled) return;if(Αε==null) Αε=IGC.RegisterBroadcastListener(ΔΥ);ΑΧ=Me.CubeGrid.EntityId;int ΖЀ=30;while(Αε.HasPendingMessage&&ΖЀ-->0){String ί=Αε.AcceptMessage().Data as String;
if(ί!=null) ΖΤ(ί);}for(int i=Р.Count-1;i>=0;i--)
if((DateTime.Now-Р[i].Γδ).TotalSeconds>Ζη) Р.RemoveAt(i);if(Φ==Α.в||++Εμ<3) return;Εμ=0;bool ΗΛ=Φ==Α.ȁ||Φ==Α.ħ||Φ==Α.Ȅ;IGC.SendBroadcastMessage(ΔΥ,ΑΧ+";"+υ(γ)+";"+
υ(Υ.GetShipVelocities().LinearVelocity)+";"+θ+";"+ΑЌ()+";"+
(ΗΛ?ǖ:-1)+";"+с+";"+Εό()+","+ΓϏ());}void ΖΤ(String ί){try{String[]f=ί.Split(';');if(f.Length<7) return;long ύ=long.Parse(f[0]);if(ύ==ΑΧ) return;Ī Ч=null;for(int i=0;i<Р.Count;i++) if(Р[i].ύ==ύ) Ч=Р[i];if(Ч==null){Ч=new Ī();Ч.ύ=ύ;Р.Add(Ч);}Ч.ζ=τ(f[1]);
Ч.Γν=τ(f[2]);Ч.Δρ=float.Parse(f[3]);Ч.ľ=f[4];Ч.К=int.Parse(f[5]);Ч.Γδ=DateTime.Now;if(Ч.ľ!=""&&Ч.ľ==ΑЌ()) ΖΟ(f[6]);Ч.Ϊ=0;Ч.ǂ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ч.Ϊ=long.Parse(d[0]);Ч.ǂ=int.Parse(d[1]);}}catch{}}bool ΖΝ(long ύ){for(int i=0;i<Р.Count;i++) if(Р[i].ύ==ύ) return true;
return false;}Vector3 ΕА(int Β,int Ηδ){if(laneOffset<=0||Τ.Count<2||Β<0||Β>=Τ.Count) return new Vector3();Χ Ψ=Τ[Β];Vector3 ΗЋ=(Τ[Math.Min(Τ.Count-1,Β+1)].Δ-Τ[Math.Max(0,Β-1)].Δ)*Ηδ;Vector3 І=Ψ.κ.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.κ):-Ψ.ΰ;Vector3 ч=Vector3.Cross(ΗЋ,І);
if(ч.LengthSquared()<0.0001f) return new Vector3();float ό=laneOffset*θ;float ΗΑ=Math.Min(Vector3.Distance(Ψ.Δ,Τ[0].Δ),
Vector3.Distance(Ψ.Δ,Τ[Τ.Count-1].Δ));return Vector3.Normalize(ч)*ό*Math.Min(1,ΗΑ/(ό*4));}void Ζτ(){Αζ=new Vector3();Ő=-1;if(!trafficEnabled||Р.Count==0||!ΔΑ(Φ)) return;Εύ(Υ.GetNaturalGravity());}void Εύ(Vector3 κ){Vector3 ǩ=ω-γ;float Ǩ=ǩ.Length();if(Ǩ<1f) return;Vector3 ι=ǩ/Ǩ;
Vector3 І=κ.LengthSquared()>0.01f?-Vector3.Normalize(κ):-ů;Vector3 ч=Vector3.Cross(ι,І);ч=ч.LengthSquared()<0.0001f?-Ż:Vector3.Normalize(ч);for(int i=0;i<Р.Count;i++){Ī Ѐ=Р[i];Vector3 ζ=Ѐ.ζ+Ѐ.Γν*(float)(DateTime.Now-Ѐ.Γδ).TotalSeconds;Vector3 ΕΥ=ζ-γ;float Ʒ=Vector3.Dot(ΕΥ,ι);
if(Ʒ<=0) continue;float Ď=(θ+Ѐ.Δρ)*0.5f;if((ΕΥ-ι*Ʒ).Length()>Ď*1.2f) continue;float ΓΚ=Vector3.Dot(Ѐ.Γν,ι);if(ΓΚ<-1f){if(Ʒ<(ȕ-ΓΚ)*6f+Ď*4f) Αζ=ч*Ď*1.3f;}else{float ΗΓ=Ʒ-Ď-followGap*θ;float ΔЎ=Math.Max(0,ΓΚ)+Math.Max(0,ΗΓ)/ΕЊ;if(Ő<0||ΔЎ<Ő) Ő=ΔЎ;
}}}bool ΓЁ(){if(с.Length==Math.Max(0,Ў)) return false;с=new String('0',Math.Max(0,Ў));ǖ=-1;return true;}void ΖΟ(String Ѐ){ΓЁ();if(Ѐ.Length!=с.Length) return;char[]ŉ=с.ToCharArray();for(int i=0;i<ŉ.Length;i++) if(Ѐ[i]=='1') ŉ[i]='1';с=new String(ŉ);}bool ΔΓ(int К){return К>=0&&К<с.Length&&с[К]=='1';}
void ΔΟ(int К){if(К<0||К>=с.Length) return;char[]ŉ=с.ToCharArray();ŉ[К]='1';с=new String(ŉ);}bool ΖΜ(int К){String ΒЈ=ΑЌ();for(int i=0;i<Р.Count;i++)
if(Р[i].ľ==ΒЈ&&Р[i].К>=0&&Math.Abs(Р[i].К-К)<=1) return true;return false;}int Εϋ(int Β){if(Ѓ<1||Ў<1) return Β;if(ΓЁ()) for(int r=0;r<Β/Ѓ&&r<Ў;r++) ΔΟ(r);if(ǖ>=0&&Β>=(ǖ+1)*Ѓ){ΔΟ(ǖ);ǖ=-1;}int К=Β/Ѓ;if(К==ǖ&&!ΔΓ(К)) return Β;bool ΔΫ=false;for(int r=0;r<Ў;r++)
{if(ΔΓ(r)) continue;ΔΫ=true;if(trafficEnabled&&ΖΜ(r)) continue;ǖ=r;return r==К?Β:r*Ѓ;}return ΔΫ?-1:Ѓ*Ў;}class ȝ{public long ύ;public Vector3 ζ,О,І;}List<ȝ>Ф=new List<ȝ>();bool ĵ=false;Vector3 Ǆ,ǃ,ǅ,ǆ;long Ȍ=0;long Ȯ=0;Χ Ǧ=null;bool Ǉ=false;Χ ΓЀ(Vector3 ζ,Vector3 О,Vector3 І)
{MatrixD m=MatrixD.CreateWorld(ζ,О,І);Χ w=new Χ(Ζ);w.α=Vector3D.TransformNormal(Ǆ,m);w.ΰ=Vector3D.TransformNormal(ǃ,m);w.χ=Vector3D.TransformNormal(ǅ,m);w.Δ=ζ+(Vector3) Vector3D.TransformNormal(ǆ,m);w.β=true;return w;}int ΓϏ(){Α s=Φ;if(s==Α.ĥ||s==Α.ǵ||s==Α.ă||s==Α.Ƴ||
s==Α.ƚ||s==Α.ȇ) return 3;if(s==Α.Ǭ||s==Α.ģ||s==Α.ƨ) return 2;if(s==Α.С) return 1;return 0;}long Εό(){return ΓϏ()==0?0:(Ǧ!=null?Ȯ:Ȍ);}void ΔΔ(){if(Κ==Θ.Ϋ||!Ζ.β) return;IMyShipConnector Ж=Ъ(MyShipConnectorStatus.Connected);if(Ж==null||Ж.OtherConnector==null) return;
IMyShipConnector Ѐ=Ж.OtherConnector;Vector3I ΗΙ=new Vector3I((int) Ζ.У.X,(int) Ζ.У.Y,(int) Ζ.У.Z);if(Ж.Position==ΗΙ){MatrixD Ļ=MatrixD.Transpose(Ѐ.WorldMatrix);Ǆ=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,Ļ);ǃ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,Ļ);ǅ=Vector3D.TransformNormal(Υ.WorldMatrix.Left,Ļ);
ǆ=Vector3D.TransformNormal(Ж.GetPosition()-Ѐ.GetPosition(),Ļ);ĵ=true;}if(Vector3.Distance(Ж.GetPosition(),Ζ.Δ)<5) Ȍ=Ѐ.EntityId;List<IMyShipConnector>ē=new List<IMyShipConnector>();ψ.GetBlocksOfType(ē,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ē.Count>0){Ф.Clear();for(int i=0;i<ē.Count;i++){ȝ p=new ȝ();p.ύ=ē[i].EntityId;p.ζ=ē[i].GetPosition();p.О=ē[i].WorldMatrix.Forward;p.І=ē[i].WorldMatrix.Up;Ф.Add(p);}}Ǧ=null;Ȯ=0;if(Ѐ.EntityId==Ȍ||!ĵ) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].ύ==Ѐ.EntityId){Ȯ=Ф[i].ύ;Ǧ=ΓЀ(Ф[i].ζ,Ф[i].О,Ф[i].І);}}bool ΑЋ(long ύ,int ΕΙ){if(ύ==0) return false;for(int i=0;i<Р.Count;i++){Ī t=Р[i];if(t.Ϊ==ύ&&(t.ǂ>ΕΙ||(t.ǂ==ΕΙ&&t.ύ<ΑΧ))) return true;}return false;}bool Αυ(int ΓΠ){if(Κ==Θ.Ϋ||!trafficEnabled){Ǧ=null;Ȯ=0;return true;}
long δ=Ǧ!=null?Ȯ:Ȍ;if(!ΑЋ(δ,ΓΠ)) return true;if(!ΑЋ(Ȍ,ΓΠ)){Ǧ=null;Ȯ=0;return true;}if(!ĵ) return false;ȝ č=null;float Δγ=0;for(int i=0;i<Ф.Count;i++){ȝ p=Ф[i];if(p.ύ==Ȍ||ΑЋ(p.ύ,ΓΠ)) continue;float d=Vector3.Distance(p.ζ,Ζ.Δ);if(č==null||d<Δγ){č=p;Δγ=d;}
}if(č==null) return false;Ȯ=č.ύ;Ǧ=ΓЀ(č.ζ,č.О,č.І);return true;}String ΕЅ(){String s=(ĵ?"1":"0")+":"+υ(Ǆ)+":"+υ(ǃ)+":"+
υ(ǅ)+":"+υ(ǆ)+":"+Ȍ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].ύ+","+υ(Ф[i].ζ)+","+υ(Ф[i].О)+","+υ(Ф[i].І);return s;}void ΕЄ(String s){Ф.Clear();ĵ=false;Ȍ=0;String[]f=s.Split(':');if(f.Length<6) return;ĵ=f[0]=="1";Ǆ=τ(f[1]);ǃ=τ(f[2]);ǅ=τ(f[3]);ǆ=τ(f[4]);long.TryParse(f[5],out Ȍ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ȝ p=new ȝ();if(!long.TryParse(e[0],out p.ύ)) continue;p.ζ=τ(e[1]);p.О=τ(e[2]);p.І=τ(e[3]);Ф.Add(p);}}void ΕЁ(string э){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}