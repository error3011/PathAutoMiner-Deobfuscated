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
// (each script only lists the settings it uses; details in the README)

const String pamTag = "[PAM]"; //LCDs of cockpits and other blocks: [PAM:<lcdIndex>] e.g. [PAM:1]
const String controllerTag = "[PAM-Controller]";
const String dataTag = "[PAM-Data]"; //EXPORT/IMPORT use the Custom Data of the block with this tag
const String dockPoolTag = "[PAM-Dock]"; //base connectors ships may use when their own home dock is taken
const float dockRequestDistance = 300f;//[m] ships ask for clearance this close to home, then wait outside the zone

const int gyroSpeedSmall = 15; //[RPM] small ship
const int gyroSpeedLarge = 5; //[RPM] large ship
const int generalSpeedLimit = 100; //[m/s] 0 = no limit (if set to 0 ignore unreachable code warning)
const float dockingSpeed = 0.5f; //[m/s]
const float jobAreaSpeed = 10f;//[m/s] near the job area (between holes, backing out, arriving, leaving)

//multiplied with ship size
const float dockDist = 0.6f; //position in front of the home connector
const float followPathDock = 2f; //stop following path, fly direct to target
const float followPathJob = 1f; //same as dock
const float useDockDirectionDist = 1f; //Override waypoint direction, use docking dir
const float useJobDirectionDist = 0f; //same as dock

//other distances
const float wpReachedDist = 2f;//[m]
const float drillRadius = 1.4f;//[m] grinders and modded drills (vanilla drills use their real cut radius)

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

//Obstacle brake: the ship's cameras scan where it flies and stop it when something is in the way.
//A camera scans within 45° of where it faces: add cameras facing every direction the ship travels.
const bool obstacleBrake = true;
const float obstacleLookAhead = 3f;//[s] scan distance = speed * this + ship size
const float obstacleTimeout = 10f;//[s] show "Path blocked" after holding this long

//Traffic between PAM ships (needs an antenna): lanes, spacing, right of way, co-op holes (IMPORT JOB)
const bool trafficEnabled = true;
const float laneOffset = 0.75f;//x ship size: keep right of the path; 0 = fly on the path
const float followGap = 1f;//x ship size: free space behind a PAM ship ahead

const bool resumeAfterRestart = true;//continue a running job after a server restart / world load

//Check if blocks are connected with conveyors
const bool checkConveyorSystem = false;//temporarily disabled because of a SE bug

//3D debug drawing (offline world + Digi's "(DevTool) Programmable Block DebugAPI" mod), see README
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ζ{ΕΡ,Ŷ,Ǐ,ф,Ϋ}public enum Α{к,ǎ,ę,ǥ,ё,Ё,Ш,
ǹ,Ļ,Ǣ,ĕ,đ,Ȫ,
ŷ,Ǿ,ǒ,Ĕ,ΑΗ,ƺ,
ΒЋ,ΑΖ,Ė,ƻ,ȯ}public enum Μ{в,Ŀ,І,ΒΧ,Ƶ,Ǆ,ǅ,ķ,ĸ}public enum у{None,Ĕ,к,Δυ}public enum Ϊ{Main,ΕΕ,ŵ,Ŵ,ΒЇ,Ƴ,
ΒΗ,ΔЅ,ΔІ,ΔЃ,Ηϋ,Ιι,
Ιη,Ιθ,ΒЂ,ΕΙ,ΑΑ}public enum г{ǋ,ŭ,Ηύ,ΔЈ,Ϋ}public enum Ǒ{ΑΓ,ΖБ}public enum и{Ǌ,Βέ,ΕΛ,Ű}public enum Ѐ{Βρ,Δς,ΒΡ,ΒΨ,Ƹ,Βι,ƿ}public enum я{ΒΤ,ΒΜ,ΒΝ}public enum К{ű,Ηέ,Ē}public enum ι{Βσ,Βχ,Βψ,ΕΒ,ΕΑ,
Βς,ǈ,Ǉ,Βυ,
Βφ,Βτ}public enum β{Ƿ,ȏ,Ƕ,Ʋ,ΒΚ,ΒΛ}public enum Ł{Δω,Δθ,Ιζ}public enum х{ΑΒ,Δχ,Ηη,Βπ}public enum τ{ż,Ʊ,ǂ,Ű,Βν,ǃ,Βξ,ΗЃ,Βω}class Ψ{public bool ε=false;public Vector3 Δ=new Vector3();public Vector3 γ=new Vector3();public Vector3 δ=new Vector3();public Vector3 υ=new Vector3();
public Vector3 ν=new Vector3();public Vector3 Ъ=new Vector3();public float Ћ=0;public float Ιμ=0;public float[]Ǡ=null;public Ψ(){}public Ψ(Ψ α){ε=α.ε;Δ=α.Δ;γ=α.γ;δ=α.δ;υ=α.υ;ν=α.ν;Ъ=α.Ъ;Ǡ=α.Ǡ;}public Ψ(Vector3 Δ,Vector3 δ,Vector3 γ,Vector3 υ,Vector3 ν){this.Δ=Δ;this.γ=γ;
this.δ=δ;this.υ=υ;this.Ћ=0;this.ν=ν;}public void ΖЍ(List<IMyThrust>ΑΛ,List<string>Ȋ){Ǡ=new float[Ȋ.Count];for(int i=0;i<Ǡ.Length;i++)
Ǡ[i]=-1;for(int i=0;i<ΑΛ.Count;i++){string ю=ΔЁ(ΑΛ[i]);int Β=Ȋ.IndexOf(ю);if(Β!=-1)
Ǡ[Β]=Г(ΑΛ[i].MaxEffectiveThrust,ΑΛ[i].MaxThrust);}}}class Βϋ{public Ψ Ѝ=null;public List<Vector3>ť=new List<Vector3>();public float Ƈ=0;public float ΔΫ=0;public float ņ=0;public float Γα=0;public Vector3 Αα=new Vector3();}class ł{public ι Џ=ι.Βσ;public float Ǖ=0;public float ā=0;
public string ǖ="";public string Ǚ="";DateTime ΖΗ;public bool ΓЌ=false;private bool ΓΥ=false;public bool ΒΟ(bool Ȉ){if(Ȉ){ā=0;ΓΥ=false;return false;}ΓΥ=true;return ā>Ǖ;}public void Ην(){ΒΟ(true);ΓЌ=false;}public void ΗЏ(){if(ΓΥ)
if((DateTime.Now-ΖΗ).TotalSeconds>1){ā++;ΖΗ=DateTime.Now;}}public bool Βζ(){switch(Џ){case ι.Βχ:return true;case ι.Βψ:return true;}return false;}}class Ǥ{public String ψ="";public String σ="";public int Ǔ=0;public К Ŗ=К.Ē;public Ǥ(String ψ,String σ,int Ǔ,К Ŗ){this.ψ=ψ;this.σ=σ;
this.Ǔ=Ǔ;this.Ŗ=Ŗ;}}IMyGridTerminalSystem ϊ;Vector3 ΑЂ=new Vector3();Vector3 ά=new Vector3();Vector3 Αδ=new Vector3();Vector3 Ɣ=new Vector3();Vector3 Ɗ=new Vector3();DateTime ΖΖ=new DateTime();bool Ǝ=true;int ΔΡ=0;bool ĉ=false;bool Αμ=false;bool ƙ=true;bool ΖЇ=false;bool œ=false;
bool Αι=false;float ȉ=0;float ı=0;int č=0;int ƪ=0;int ΑЅ=0;int ΓΚ=0;float Ř=0;float Ň=0;double ΓЀ=0;float ſ=0;List<int>Ő=new List<int>();List<int>Š=new List<int>();Ζ Ι=Ζ.ΕΡ;int ώ=0;float ǵ=0;float Ǵ=0;int ĩ=0;float ȝ=0,Ȝ=0;float ί=0;float ΔΔ=0,ΔΓ=0;IMyRemoteControl Υ;
IMySensorBlock ξ;List<IMyTimerBlock>ΔΥ=new List<IMyTimerBlock>();List<IMyShipConnector>ц=new List<IMyShipConnector>();List<IMyThrust>ȋ=new List<IMyThrust>();List<IMyGyro>Αε=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȇ=new List<IMyLandingGear>();
List<IMyReactor>Č=new List<IMyReactor>();List<IMyConveyorSorter>ΑЊ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>Ž=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŋ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ğ=new List<IMyTerminalBlock>();
List<IMyBatteryBlock>Ȱ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>Ď=new List<IMyTextSurface>();List<IMyTextPanel>Ɔ=new List<IMyTextPanel>();IMyTerminalBlock њ=null;Ψ Η=new Ψ();Ψ Γν=new Ψ();Ψ Γπ=new Ψ();bool Ȍ=false;bool Ũ=false;
bool ũ=false;bool Αλ=false;List<String>Ȋ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>Ư=new List<Ψ>();int ΓΪ=0;double ΖЈ=0;Ψ Ε=new Ψ();Vector3 Ќ;Vector3 ǰ;Μ ΰ=Μ.в;Ǒ Ī=Ǒ.ΑΓ;int Р=0;int Э=0;double ƒ=0;bool ΑΧ=false;ł ό=new ł();ł ύ=new ł();и о=и.Ǌ;bool ď=true;Ǒ Ȥ=Ǒ.ΑΓ;
int м=3;int л=3;int Щ=30;я ч=я.ΒΤ;bool ȩ=true;bool Œ=true;bool ŋ=true;bool Ĳ=false;Ѐ П=Ѐ.Βρ;bool ȧ=true;float ǯ=90;float ъ=20;float Ю=5;float ы=20;float Ǩ=0.70f;float ū=1.50f;float Ū=2.50f;float Ķ=0f;float Ħ=0f;Α Τ;Μ щ;Α Ƥ;Βϋ η=null;int Ы=0;int Ь=0;int М=0;
int ȟ=0;int į=30;int Х=0;bool Ϗ=true;Vector3 Ј;double Н=0;bool ΑЄ=false;int Δΰ=0;int ϋ=0;int ΓΑ=0;int Ǳ=0;int ΖΫ=0;Vector3 Ɯ=new Vector3();float н=0;float а=0;int ĵ=0;Ψ ΑЋ=null;float Γσ=0;float ΑΝ=0;float У=0;float Ĭ=0;bool Γξ=false;bool ĳ=false;bool ƫ=false;
bool Ț=false;bool ȓ=false;DateTime ΘΫ=new DateTime();Ψ Ģ=null;Ψ Ελ=null;float ǿ=0;у ǩ;float ț=0;float Ĵ=0;String Γϋ="";float Απ=0;float ƅ=0;float Ɩ=0;List<Ǥ>ǘ=new List<Ǥ>();float С=0;bool Ƣ=false;bool Ζύ=false;bool Ζκ=false;bool ΑЏ=false;bool Ζλ=false;float Γώ=0;
float ΓЊ=0;Vector3 Εώ=new Vector3();Vector3 ς=new Vector3();Vector3 ΑА=new Vector3(1,1,1);float ΔΠ=1;Vector3 Ƨ=new Vector3();bool Ακ=false;bool Γη=false;bool Γψ=false;bool ΕΧ=false;float ΑΥ=0;float ΑΙ=2;Vector3 Ζϋ;Vector3 ΔΟ;Vector3 ΔΞ;float[,]ǡ=new float[3,2];Dictionary<String,float[,]>ѝ=new Dictionary<string,float[,]>();
Dictionary<String,float>ΖЅ=new Dictionary<String,float>();int ΓΔ=0;Dictionary<String,float[]>ȡ=new Dictionary<string,float[]>();float ΓЍ;bool ΑΨ=false;int ΑΜ=0;int Ć=0;int Αη=0;int ħ=0;String Λ="";int[]ΖΡ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΣ=false;int Αϊ=0;
int Ǜ=0;Ϊ Б=Ϊ.Main;int ΙΜ=0,ΙΝ=0,ΙΞ=0,ΙΟ=0;int ΘЍ=0,ΘЎ=0;bool ŉ=false;bool ΕΨ=false;String ΔΕ="";String ΓΙ="";IMyBroadcastListener Ȕ=null;bool Εύ=true;String Θπ="";Exception Ȟ=null;bool ΑЃ=false;Μ Ţ=Μ.в;bool Εξ=false;bool ΖΕ=true;bool ΔΑ=false;String ΖІ="";
bool Ʃ=false;bool ΔΚ=true;int Αθ=0;Vector3 ΑΞ=new Vector3();bool ΕЄ=false;bool ǐ(IMyTerminalBlock Φ)=>Φ.CubeGrid==Me.CubeGrid;Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βη(Me,controllerTag,true))
Ι=Ζ.ф;π(Ϊ.Main);if(Ι!=Ζ.ф){Λ="Welcome to [PAM]!";х ΓБ=Ηξ();if(Ι==Ζ.ΕΡ){List<IMyShipDrill>Εω=new List<IMyShipDrill>();List<IMyShipGrinder>ΕЅ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(Εω,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(ΕЅ,q=>q.CubeGrid==Me.CubeGrid);if(Εω.Count>0)
{Ι=Ζ.Ŷ;Λ="Miner mode enabled!";}else if(ΕЅ.Count>0){Ι=Ζ.Ǐ;Λ="Grinder mode enabled!";}else{Ι=Ζ.Ϋ;Ǽ(г.Ϋ);}}if(ΓБ==х.ΑΒ) ƙ=false;if(ΓБ==х.Δχ) Λ="Data restore failed!";if(ΓБ==х.Βπ) Λ="New version, wipe data";}}void Main(string ѓ,UpdateType ΙΨ){try{if(Ȟ!=null){if(ѓ.Trim().ToUpper()=="RETRY")
{Ȟ=null;Ǝ=true;č=0;if(Ι!=Ζ.ф) φ();Λ="Restarted after error";}else{ΗΩ();ΕΓ();return;}}Αμ=(ΙΨ&UpdateType.Update10)!=0;if(Αμ) ΔΡ++;ĉ=ΔΡ>=10;if(ĉ) ΔΡ=0;if(Αμ){ΑΜ++;if(ΑΜ>4) ΑΜ=0;ΑЅ=(int) Math.Max(0,10-(DateTime.Now-ΖΖ).TotalSeconds);}if(ѓ!="") Θπ=ѓ;if(Ι!=Ζ.ф)
Ηό(ѓ);else
ΗΚ(ѓ);ΖΣ=false;try{int Εμ=Runtime.CurrentInstructionCount;float ƞ=Г(Εμ,Runtime.MaxInstructionCount);if(ƞ>0.90) Λ="Max. instructions >90%";if(ƞ>Ř) Ř=ƞ;if(ΑΨ){Ő.Add(Εμ);while(Ő.Count>10) Ő.RemoveAt(0);Ň=0;for(int i=0;i<Ő.Count;i++) Ň+=Ő[i];Ň=Г(Г(Ň,Ő.Count),Runtime.MaxInstructionCount);
double Γτ=Runtime.LastRunTimeMs;if(Αι&&Γτ>ΓЀ) ΓЀ=Γτ;Š.Add((int)(Γτ*1000f));while(Š.Count>10) Š.RemoveAt(0);ſ=0;for(int i=0;i<Š.Count;i++) ſ+=Š[i];ſ=Г(ſ,Š.Count)/1000f;}}catch{Ř=0;}}catch(Exception e){Ȟ=e;}}void Ηό(string ѓ){bool ΑІ=false;String Εδ="";if(ώ<=1&&!ΔЋ(ѓ))
ΕΔ(ѓ);int Θϊ=50;while(Ȕ!=null&&Ȕ.HasPendingMessage&&Θϊ-->0){MyIGCMessage ρ=Ȕ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String ơ="";if(ώ<=1&&Ηθ(ref Χ,out ơ,out Εδ)&&ơ==BROADCAST_SELF_ID){ΕΔ(Χ);ΑІ=true;break;}}bool ΕБ=Αι&&Τ==Α.к&&!œ&&!ΑІ&&č==0&&!Αλ;
if(ĉ&&Τ!=Α.к) ΑІ=true;if((Αμ&&!ΕБ)||(ĉ&&ΕБ)){if(č==0&&(ΑЅ<=0||Ǝ)){ΖЇ=ώ>0;ώ=0;č=1;ǽ();Ης();Ηή();Ǹ("Scan 1");}else if(č==1){č=2;ǽ();Ησ();Ǹ("Scan 2");}else if(č==2){č=0;ǽ();Ητ();Ηυ();if(ώ<=1) ΔЎ();Ǹ("Scan 3");ΖΖ=DateTime.Now;if(ώ<=1&&ƙ)
ΑЂ=ƽ(Υ,Υ.CenterOfMass);ƙ=false;if(Ǝ){Ǝ=false;bool Ιέ=ΰ==Μ.І;φ();if(ΖΕ&&resumeAfterRestart&&Ιέ&&ώ<=1) ΔΑ=true;}ΖΕ=false;if(ΖЇ&&ώ==0) Λ="Setup complete";}else{if(ΰ==Μ.І&&Ι!=Ζ.Ϋ){ǽ();ΖЊ();Ǹ("Inv balance");}ǽ();switch(ƪ){case 0:ΗЇ();break;case 1:ΗЋ();break;case 2:ΗІ();break;
case 3:ΘΑ();break;case 4:ΗЊ();break;case 5:ΗΑ();break;case 6:ΖЏ(Υ);break;}Ǹ("Update: "+ƪ);ƪ++;if(ƪ>6){ƪ=0;Αι=true;if(Ţ!=Μ.в){switch(Ţ){case Μ.ķ:Ȭ();break;case Μ.ĸ:Ȭ();break;case Μ.І:Ȭ();break;case Μ.Ǆ:Ƽ();break;case Μ.ǅ:ė();break;}Ţ=Μ.в;}}}if(!Ǝ){if(!ǻ(Υ,true)){Υ=null;
ƙ=true;ώ=2;}if(ώ>=2&&Τ!=Α.к)
φ();if(ώ<=1){ı=Υ.CalculateShipMass().PhysicalMass;ȉ=(float) Υ.GetShipSpeed();ά=ΗЅ(Υ,ΑЂ);Αδ=Υ.WorldMatrix.Forward;Ɣ=Υ.WorldMatrix.Left;Ɗ=Υ.WorldMatrix.Down;ΗЎ();ΗА();ΗЈ();if(ΔΑ){ΔΑ=false;Ȭ();if(Τ!=Α.к) Λ="Job resumed after restart";}if(Τ!=Α.к){œ=false;ΕΘ(false);Δμ(false);String ΙΕ=Ββ(Τ)+" "+(int) Τ;
ǽ();ΗЌ();ΒЊ(false);Ǹ(ΙΕ);ΗБ();ΗЍ();ǽ();ΗЉ();Ǹ("Thruster");ǽ();ΕΣ();Ǹ("Gyroscope");}else{if(œ){if(ǁ()){Ч(Ɗ,Αδ,Ɣ,0.25f,true);ΕΣ();Λ="Aligning to planet: "+Math.Round(ΑΥ-0.25f,2)+"°";if(Ακ) Βε(true,true);}else Βε(true,true);}}Ʃ=false;}}}ǽ();ΕΓ();Ǹ("Print");if(ΑІ||ΓΚ<=0){ǽ();ΖЋ(Εδ);
Ǹ("Broadcast");ΓΚ=4;}else if(ĉ) ΓΚ--;}void ΕΔ(string ѓ){if(ѓ=="") return;var И=ѓ.ToUpper().Split(' ');И.DefaultIfEmpty("");var ΓΠ=И.ElementAtOrDefault(0);var Ġ=И.ElementAtOrDefault(1);var ž=И.ElementAtOrDefault(2);var ΕΫ=И.ElementAtOrDefault(3);String ć="Invalid argument: "+ѓ;bool ΔΪ=false;
switch(ΓΠ){case "UP":this.ΔА(false);break;case "DOWN":this.ΔЏ(false);break;case "UPLOOP":this.ΔА(true);break;case "DOWNLOOP":this.ΔЏ(true);break;case "APPLY":this.Δκ(true);break;case "MRES":Ǜ=0;break;case "STOP":this.φ();break;case "PATHHOME":{this.φ();this.ΒІ();}break;case "PATH":{this.φ();this.ΒІ();Η.ε=true;}break;
case "START":{this.φ();ΒЅ();}break;case "ALIGN":{Βε(!œ,false);}break;case "CONT":{this.φ();this.Ȭ();}break;case "JOBPOS":{this.φ();this.ė();}break;case "HOMEPOS":{this.φ();this.Ƽ();}break;case "FULL":{Ƣ=true;}break;case "RESET":{ΑЃ=true;ώ=2;}break;case "EXPORT":{ΗΠ();}break;
case "IMPORT":{ΗΫ(Ġ=="JOB");}break;default:ΔΪ=true;break;}if(Ι!=Ζ.Ϋ){switch(ΓΠ){case "SHUTTLE":{ΗΞ();}break;case "CFGS":{if(!ΗΘ(Ġ,ž,ΕΫ)) Λ=ć;}break;case "CFGB":{if(!ΗΗ(Ġ,ž)) Λ=ć;}break;case "CFGL":{if(!Ě(ref ǯ,true,τ.ǂ,Ġ,"")||!ΗΙ(ž))
Λ=ć;}break;case "CFGE":{if(!Ě(ref Ю,true,τ.ǃ,Ġ,"IG")||
!Ě(ref ъ,true,τ.Βν,ž,"IG")||
!Ě(ref ы,true,τ.Βξ,ΕΫ,"IG"))
Λ=ć;}break;case "CFGA":{if(!Ě(ref Ǩ,false,τ.Ʊ,Ġ,"")) Λ=ć;}break;case "CFGW":{if(!Ě(ref ū,false,τ.ż,Ġ,"")||
!Ě(ref Ū,false,τ.ż,ž,""))
Λ=ć;}break;case "NEXT":{ΒΖ(false);}break;case "PREV":{ΒΖ(true);}break;default:if(ΔΪ) Λ=ć;break;}}else{switch(ΓΠ){case "UNDOCK":{Ʃ=true;}break;default:if(ΔΪ) Λ=ć;break;}}}String ΗΥ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ι!=Ζ.Ϋ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΞ(){φ();Ι=Ζ.Ϋ;Ǽ(г.Ϋ);Ε.ε=false;Η.ε=false;ξ=null;ο.Clear();ΰ=Μ.в;}void Βε(bool Љ,bool ѕ){if(!Љ) Λ="Aligning canceled";if(ѕ) Λ="Aligning done";if(ѕ||!Љ){œ=false;ΕΜ();ǌ(false,0,0,0,0);return;}if(ǁ()) œ=true;}bool ΗΘ(String Ιί,String Θζ,String ΘΥ)
{bool ΙΫ=ΰ==Μ.І;int w,h,d;if(int.TryParse(Ιί,out w)&&int.TryParse(Θζ,out h)&&int.TryParse(ΘΥ,out d)){this.φ();м=w;л=h;Щ=d;ĝ(false);Ż(false,false);if(ΙΫ) Ȭ();return true;}return false;}bool ΗΙ(String ΕΪ){if(ΕΪ=="ON"){ȩ=true;return true;}if(ΕΪ=="OFF"){ȩ=false;return true;}
return false;}bool ΗΗ(String Εψ,String ΑΦ){bool ƚ=true;if(Εψ=="HOME") ď=true;else if(Εψ=="STOP") ď=false;else ƚ=false;if(ΑΦ=="HOME") о=и.Ǌ;else if(ΑΦ=="STOP") о=и.ΕΛ;else if(ΑΦ=="JOB") о=и.Βέ;else if(ΑΦ=="IG") о=и.Ű;else ƚ=false;return ƚ;}void π(Ϊ ΓЈ){ΖΡ[(int) Б]=Ǜ;
Ǜ=ΖΡ[(int) ΓЈ];if(ΓЈ==Ϊ.Ηϋ) Ǜ=0;Б=ΓЈ;if(Ι!=Ζ.ф)
Ĝ(Б==Ϊ.ŵ,false,0,0);ΖΣ=true;}void ΔА(bool Γω){if(Ǜ>0) Ǜ--;else if(Γω) Ǜ=Αϊ-1;}void ΔЏ(bool Γω){if(Ǜ<Αϊ-1) Ǜ++;else if(Γω) Ǜ=0;}String Ǽ(г ρ){switch(ρ){case г.ǋ:Λ="Job is running";break;case г.ŭ:Λ="Connector not ready!";break;case г.Ηύ:Λ="Ship modified, path outdated!";break;
case г.ΔЈ:Λ="Interrupted by player!";break;case г.Ϋ:Λ="Shuttle mode enabled!";break;}return "";}String ΗΨ(Ǒ λ){switch(λ){case Ǒ.ΑΓ:return "Top-Left";case Ǒ.ΖБ:return "Center";default:return "";}}String ΗΣ(я Ƙ){switch(Ƙ){case я.ΒΜ:return "Auto"+(Ι==Ζ.Ŷ?" (Ore)":"");case я.ΒΝ:return "Auto (+Stone)";
case я.ΒΤ:return "Default";default:return "";}}String Βΰ(Μ Ц){switch(Ц){case Μ.в:return "No job";case Μ.Ŀ:return "Job paused";case Μ.І:return "Job active";case Μ.ķ:return "Job active";case Μ.ĸ:return "Job active";case Μ.ΒΧ:return "Job done";case Μ.Ƶ:return "Job changed";case Μ.Ǆ:return "Move home";
case Μ.ǅ:return "Move to job";}return "";}String Δό(и ΘΛ){switch(ΘΛ){case и.Ǌ:return "Return home";case и.Βέ:return "Fly to job pos";case и.ΕΛ:return "Stop";case и.Ű:return "Ignore";}return "";}String ΗΤ(Ѐ Ƙ){switch(Ƙ){case Ѐ.Βρ:return "Off";case Ѐ.ΒΨ:return "Drop pos (Stone) ";case Ѐ.Ƹ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δς:return "Cur. pos (Stone)";case Ѐ.ΒΡ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βι:return "In motion (Stone)";case Ѐ.ƿ:return "In motion (Sto.+Ice)";}return "";}String Δϊ(у Ц){switch(Ц){case у.None:return "No batteries";case у.Ĕ:return "Charging";case у.Δυ:return "Discharging";}return "";}String Ββ(Α Ц)
{String Ικ=Ι==Ζ.Ϋ?"target":"job";switch(Ц){case Α.к:return "Idle";case Α.ǎ:return "Flying to XY position";case Α.ę:return Ι==Ζ.Ǐ?"Grinding":"Mining";case Α.ǥ:return "Returning";case Α.Ė:return "Flying to drop pos";case Α.ё:return "Returning to dock";case Α.Ё:return "Flying to dock area";
case Α.Ш:return "Flying to job area";case Α.ǹ:return "Flying to path";case Α.Ļ:return "Flying to job position";case Α.Ǣ:return "Approaching dock";case Α.ĕ:return "Docking";case Α.đ:return "Aligning";case Α.Ȫ:return "Aligning";case Α.ŷ:return "Retry docking";case Α.Ǿ:return "Unloading";case Α.ȯ:return ΖІ;
case Α.ǒ:return "Undocking";case Α.Ĕ:return "Charging batteries";case Α.ΑΗ:return "Waiting for uranium";case Α.ƺ:return "Filling up hydrogen";case Α.ΒЋ:return "Waiting for ejection";case Α.ΑΖ:return "Waiting for ejection";case Α.ƻ:return "Flying to drop pos";}return "";}String ΔЂ(ι Џ){switch(Џ)
{case ι.Βσ:return "On \"Undock\" command";case ι.Βχ:return "On player entered cockpit";case ι.ΕΒ:return "On ship is full";case ι.ΕΑ:return "On ship is empty";case ι.Βψ:return "On time delay";case ι.ǈ:return "On batteries empty(<25%)";case ι.Ǉ:return "On batteries empty(=0%)";case ι.Βς:return "On batteries full";
case ι.Βφ:return "On hydrogen empty(<25%)";case ι.Βτ:return "On hydrogen empty(=0%)";case ι.Βυ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Ζε,int ą,bool Ξ,String Ɠ){Αϊ+=1;if(Ζε==ą) Ɠ=">"+Ɠ+(ΑΜ>=2?" .":"");else Ɠ=" "+Ɠ;Γ+=Ɠ+"\n";return Ζε==ą&&Ξ;}String Δκ(bool Ξ)
{int Β=0;int Ο=Ǜ;Αϊ=0;String Ζθ="———————————————\n";String ŧ="--------------------------------------------\n";String Γ="";Γ+=Βΰ(ΰ)+" | "+(Η.ε?"Ready to dock":"No dock")+"\n";Γ+=Ζθ;double ǟ=Math.Max(Math.Round(this.Н),0);if(Б==Ϊ.Main){bool Ĉ=Ι==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒІ();
if(Ι==Ζ.Ŷ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.ŵ);if(Ι==Ζ.Ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.ŵ);if(Ι==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.ΒЂ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ȭ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) Ƽ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) ė();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ĉ) π(Ϊ.ΑΑ);else π(Ϊ.Ƴ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.ΔЅ);if(Ι!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.ΔЃ);}else if(Б==Ϊ.ŵ){double Ιή=Math.Round(м*ǵ,1);double Θε=Math.Round(л*Ǵ,1);String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Start new job!")) ΒЅ();if(Π(ref Ѕ,Ο,Β++,Ξ," Change current job")){Ż(false,false);π(Ϊ.Main);}if(Π(ref Ѕ,Ο,Β++,Ξ," Width + (Width: "+м+" = "+Ιή+"m)")){Ĺ(ref м,5,20,1);ĝ(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Width -")){Ĺ(ref м,-5,20,-1);ĝ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height + (Height: "+л+" = "+Θε+"m)")){Ĺ(ref л,5,20,1);ĝ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height -")){Ĺ(ref л,-5,20,-1);ĝ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth + ("+(ч==я.ΒΤ?"Depth":"Min")+": "+Щ+"m)")){Ĺ(ref Щ,5,50,2);ĝ(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Depth -")){Ĺ(ref Щ,-5,50,-2);ĝ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth mode: "+ΗΣ(ч))){ч=ȭ(ч);}if(Π(ref Ѕ,Ο,Β++,Ξ," Start pos: "+ΗΨ(Ȥ))){Ȥ=ȭ(Ȥ);}if(Ι==Ζ.Ǐ&&ч==я.ΒΝ) ч=ȭ(ч);Γ+=ΕΖ(8,Ѕ,Ο,ref ΘЍ);}else if(Б==Ϊ.ΒЂ){float[]Επ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΙ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔЂ(ό.Џ))) ό.Џ=ȭ(ό.Џ);if(!ό.Βζ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βί((int) ό.Ǖ))) ό.Ǖ=Δσ(ό.Ǖ,Επ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔЂ(ύ.Џ))) ύ.Џ=ȭ(ύ.Џ);
if(!ύ.Βζ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βί((int) ύ.Ǖ))) ύ.Ǖ=Δσ(ύ.Ǖ,Επ);}else if(Б==Ϊ.ΕΙ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЅ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.ΒЂ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǖ!=""?ό.ǖ:"-"))) ό.ǖ=ǆ(ref ΙΜ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǚ!=""?ό.Ǚ:"-"))) ό.Ǚ=ǆ(ref ΙΞ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.ǖ!=""?ύ.ǖ:"-"))) ύ.ǖ=ǆ(ref ΙΝ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.Ǚ!=""?ύ.Ǚ:"-"))) ύ.Ǚ=ǆ(ref ΙΟ);
}else if(Б==Ϊ.Ŵ){String ΖΛ=ǯ+" %";if(ĉ) Ć++;if(Ć>1){Ć=0;Αη++;if(Αη>1) Αη=0;}bool[]ΔΗ=new bool[]{Č.Count==0,ǩ==у.None,і.Count==0};int ΖЂ=0;while(true){ΖЂ++;ħ++;if(ħ>ΔΗ.Length-1) ħ=0;if(ΖЂ>=ΔΗ.Length) break;if(!ΔΗ[ħ]) break;}bool Ĉ=Ι==Ζ.Ϋ;if(!Ĉ&&ȩ&&С!=-1&&Αη==0)
ΖΛ=С<1000000?Math.Round(С)+" Kg":Math.Round(С/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){φ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ĉ) π(Ϊ.Ƴ);else π(Ϊ.ΑΑ);if(!Ĉ){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΖ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ʃ=true;Γ+=ŧ;if(!Ĉ) Γ+="Progress: "+Math.Round(ƒ,1)+" %\n";Γ+="State: "+Ββ(Τ)+" "+ǟ+"m \n";Γ+="Load: "+Απ+" % Max: "+ΖΛ+" \n";if(ħ==0) Γ+="Uranium: "+(Č.Count==0?"No reactors":Math.Round(Ĵ,1)+"Kg "+(Ю==-1?"":" Min: "+Ю+" Kg"))+"\n";
if(ħ==1) Γ+="Battery: "+(ǩ==у.None?Δϊ(ǩ):ǿ+"% "+(ъ==-1||Ĉ?"":" Min: "+ъ+" %"))+"\n";if(ħ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ț,1)+"% "+(ы==-1||Ĉ?"":" Min: "+ы+" %"))+"\n";}else if(Б==Ϊ.Ƴ){String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) π(Ϊ.Ŵ);else π(Ϊ.Main);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) з(ref ǯ,ǯ<=80?-10:-5,τ.ǂ,false);if(Π(ref Ѕ,Ο,Β++,Ξ," Weight limit: "+(ȩ?"On":"Off"))) ȩ=!ȩ;if(Π(ref Ѕ,Ο,Β++,Ξ," Ejection: "+ΗΤ(П))) П=ȭ(П);if(Π(ref Ѕ,Ο,Β++,Ξ," Toggle sorters: "+(Ĳ?"On":"Off"))){Ĳ=!Ĳ;if(Ĳ) ǧ(ΔΚ);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Unload ice: "+(ȧ?"On":"Off"))) ȧ=!ȧ;if(Π(ref Ѕ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) з(ref Ю,(Ю>5?-5:-1),τ.ǃ,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Battery: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) з(ref ъ,-5,τ.Βν,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Hydrogen: "+(ы==-1?"Ignore":"Min "+ы+"%"))) з(ref ы,-10,τ.Βξ,true);
if(Π(ref Ѕ,Ο,Β++,Ξ," When done: "+(ď?"Return home":"Stop"))) ď=!ď;if(Π(ref Ѕ,Ο,Β++,Ξ," On damage: "+Δό(о))) о=ȭ(о);if(Π(ref Ѕ,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΗ);Γ+=ΕΖ(8,Ѕ,Ο,ref ΘЎ);}else if(Б==Ϊ.ΒΗ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) π(Ϊ.Ŵ);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Ι==Ζ.Ǐ?" Grinder":" Drill")+" inv. balancing: "+(Œ?"On":"Off"))) Œ=!Œ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Ι==Ζ.Ǐ?" grinders":" drills")+": "+(ŋ?"Fwd + Bwd":"Fwd"))) ŋ=!ŋ;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ū+"m/s")) з(ref ū,0.5f,τ.ż,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ū+"m/s")) з(ref Ū,0.5f,τ.ż,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) з(ref Ǩ,0.1f,τ.Ʊ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ķ*100f+"%")) Δξ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+Ħ*100f+"%")) Δξ(false,0.05f);
}else if(Б==Ϊ.ΑΑ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) π(Ϊ.Ŵ);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) з(ref ǯ,ǯ<=80?-10:-5,τ.ǂ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȧ?"On":"Off"))) ȧ=!ȧ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) з(ref Ю,(Ю>5?-5:-1),τ.ǃ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ъ==-1?"Ignore":"Charge up"))) ъ=(ъ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ы==-1?"Ignore":"Fill up"))) ы=(ы==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δό(о))) о=ȭ(о);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) з(ref Ǩ,0.1f,τ.Ʊ,false);
}else if(Б==Ϊ.ΕΕ){double Θρ=0;if(Σ.Count>0) Θρ=Vector3.Distance(Σ.Last().Δ,ά);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΝ();if(Ι!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȍ?"Use old home":(Η.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȍ?"Use old connector":(Η.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ũ?"Use old connector":(Ε.ε?"Was set! ":"none ")))) Ũ=!Ũ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ũ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ũ=!ũ;Γ+=ŧ;Γ+="Wp spacing: "+Math.Round(ΖЈ)+"m\n";}else if(Б==Ϊ.ΒЇ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){φ();π(Ϊ.Main);}
Γ+=ŧ;Γ+="State: "+Ββ(Τ)+" \n";Γ+="Speed: "+Math.Round(ȉ,1)+"m/s\n";Γ+="Target dist: "+ǟ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+Δΰ+"\n";}else if(Б==Ϊ.ΔЅ){List<IMyTerminalBlock>ģ=Δύ();if(ĉ) Ć++;if(Ć>=ģ.Count) Ć=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔІ);
Γ+=ŧ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Απ,1)+"% "+Math.Round(ƅ,1)+" / "+Math.Round(Ɩ,1)+"\n";Γ+="Uranium: "+(Č.Count==0?"No reactors":Math.Round(Ĵ,1)+"Kg "+Γϋ)+"\n";Γ+="Battery: "+(ǩ==у.None?"":ǿ+"% ")+Δϊ(ǩ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(ț,1)+"% ")+"\n";
Γ+="Damage: "+(ģ.Count==0?"None":""+(Ć+1)+"/"+ģ.Count+" "+ģ[Ć].CustomName)+"\n";}else if(Б==Ϊ.ΔІ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="Next scan: "+ΑЅ+"s\n";Γ+="Drill area: "+Math.Round(ǵ,1)+"x"+Math.Round(Ǵ,1)+"m Ship: "+Math.Round(ΔΔ,1)+"x"+Math.Round(ΔΓ,1)+"m ("+Math.Round(ί,1)+"m)\n"+(Ǫ.Count>0?"Cameras: "+ΓΜ+"\n":"");
Γ+="Broadcast: "+(ŉ?"Online - "+ΔΕ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ř*100f,1)+"% \n";}else if(Б==Ϊ.ΔЃ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+ΑЅ+"s\n";if(ΑЃ) Γ="Recompile script now";int Θς=Γ.Split('\n').Length;for(int i=Θς;i<=10;i++) Γ+="\n";Γ+=Ζθ;Γ+="Last: "+Λ+"\n";
return Γ;}void Δξ(bool Θμ,float ќ){φ();Ż(true,false);if(Θμ) з(ref Ķ,ќ,τ.Βω,false);else з(ref Ħ,ќ,τ.Βω,false);Δν();ȝ=ǵ;Ȝ=Ǵ;Ĝ(true,true,0,0);}float Δσ(float θ,float[]ΒΕ){float ή=ΒΕ[0];for(int i=ΒΕ.Length-1;i>=0;i--)
if(θ<ΒΕ[i]) ή=ΒΕ[i];return ή;}String ǆ(ref int ą){String ή="";if(ą>=ΔΥ.Count) ą=-1;if(ą>=0) ή=ΔΥ[ą].CustomName;ą++;return ή;}void ΑΔ(string ψ){if(ΰ!=Μ.І) return;if(ψ=="") return;IMyTerminalBlock Φ=ϊ.GetBlockWithName(ψ);if(Φ==null||!(Φ is IMyTimerBlock)){Λ="Timerblock "+ψ+" not found!";
return;}((IMyTimerBlock) Φ).Trigger();}void Ĺ(ref int value,int Ɓ,int ȥ,int ΔΘ){if(Ɓ==0) return;if(value<ȥ&&ΔΘ>0||value<=ȥ&&ΔΘ<0){value+=ΔΘ;return;}int ΘΓ=Math.Abs(Ɓ);int ΒЌ=0;int ΓЂ=1;while(true){ΒЌ+=ΓЂ*ΘΓ*10;if(Ɓ<0&&value-ȥ<=ΒЌ) break;if(Ɓ>0&&value-ȥ<ΒЌ) break;
ΓЂ++;}value+=ΓЂ*Ɓ;}void ĝ(bool Ιν){м=Math.Max(м,1);л=Math.Max(л,1);Щ=Math.Max(Щ,0);Ĝ(Б==Ϊ.ŵ,false,0,0);}bool Ě(ref float value,bool ΘЉ,τ σ,String Ĩ,String Θι){if(Ĩ=="") return false;float ŝ=-1;bool ΕЎ=false;if(Ĩ.ToUpper()==Θι) ΕЎ=true;else if(!float.TryParse(Ĩ,out ŝ)) return false;
else ŝ=Math.Max(0,ŝ);if(ΘЉ) ŝ=(float) Math.Round(ŝ);з(ref value,ŝ,σ,ΕЎ,false);return true;}void з(ref float value,float ќ,τ σ,bool ƨ){з(ref value,value+ќ,σ,ƨ,true);}void з(ref float value,float ř,τ σ,bool ƨ,bool Ιβ){float ǜ=0,ǚ=0;if(σ==τ.ż){ǜ=0.5f;ǚ=10f;}if(σ==τ.Ʊ){ǜ=0.1f;ǚ=1f;}
if(σ==τ.Ű){ǜ=50f;ǚ=100f;}if(σ==τ.Βν){ǜ=5f;ǚ=30f;}if(σ==τ.ǃ){ǜ=1f;ǚ=25f;}if(σ==τ.ǂ){ǜ=10f;ǚ=95f;}if(σ==τ.Βξ){ǜ=10f;ǚ=90f;}if(σ==τ.ΗЃ){ǜ=10f;ǚ=1800;}if(σ==τ.Βω){ǜ=0.0f;ǚ=0.75f;}if(ř==-1&&ƨ){value=-1;return;}if(value==-1) ƨ=false;
bool Ζέ=ř<ǜ||ř>ǚ;if(Ζέ&&Ιβ){if(ř<value) value=ǚ;else if(ř>value) value=ǜ;}else value=ř;if(Ζέ&&ƨ) value=-1;else value=Math.Max(ǜ,Math.Min(value,ǚ));value=(float) Math.Round(value,2);}void ΒΖ(bool ΘЅ){if(ΘЅ) М=Math.Max(0,М-1);else М++;ΒΪ();ΒΩ();for(int k=0;k<ȅ.Count&&k<О.Length;k++) ΒЀ(ȅ[k],k<М);
Ѓ=-1;ΒЊ(true);}T ȭ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΟ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΟ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒІ(){Ư.Clear();for(int i=0;i<Σ.Count;i++) Ư.Add(Σ[i]);Σ.Clear();
Αλ=true;Γν=new Ψ(Η);Γπ=new Ψ(Ε);Η.ε=false;if(Ι==Ζ.Ϋ) Ε.ε=false;for(int i=0;i<ѝ.Count;i++)
if(!Ȋ.Contains(ѝ.Keys.ElementAt(i)))
Ȋ.Add(ѝ.Keys.ElementAt(i));Ȍ=false;Ũ=false;ũ=false;π(Ϊ.ΕΕ);}void ΕΝ(){if(Ȍ) Η=Γν;if(Ũ) Ε=Γπ;if(ũ){Σ.Clear();for(int i=0;i<Ư.Count;i++) Σ.Add(Ư[i]);}Αλ=false;φ();π(Ϊ.Main);}void ΗЎ(){if(!Αλ) return;if(Τ!=Α.к){ΕΝ();return;}if(!Γν.ε) Ȍ=false;if(!Γπ.ε) Ũ=false;if(Ư.Count<=1) ũ=false;
IMyShipConnector Л=Т(MyShipConnectorStatus.Connectable);if(Л==null) Л=Т(MyShipConnectorStatus.Connected);if(Л!=null){if(Math.Round(ȉ,2)<=0.20) ΓΪ++;else ΓΪ=0;if(ΓΪ>=5){if(Ι==Ζ.Ϋ&&(Η.ε||Ȍ)&&Vector3.Distance(Η.Δ,Л.GetPosition())>5){Ε.δ=Υ.WorldMatrix.Forward;Ε.υ=Υ.WorldMatrix.Left;
Ε.γ=Υ.WorldMatrix.Down;Ε.ν=Υ.GetNaturalGravity();Ε.Δ=Л.GetPosition();Ε.ε=true;Ε.Ъ=Л.Position;}else{Η.δ=Υ.WorldMatrix.Forward;Η.υ=Υ.WorldMatrix.Left;Η.γ=Υ.WorldMatrix.Down;Η.ν=Υ.GetNaturalGravity();Η.Δ=Л.GetPosition();Η.ε=true;Η.Ъ=Л.Position;}}}double Γς=-1;if(Σ.Count>0) Γς=Vector3.Distance(ά,Σ.Last().Δ);
double ΙΓ=Math.Max(1.5,Math.Pow(ȉ/100.0,2));double ΖΤ=Math.Max(ȉ*ΙΓ,2);ΖЈ=ΖΤ;if((Γς==-1)||Γς>=ΖΤ){Ψ Ω=new Ψ(ά,Αδ,Ɗ,Ɣ,Υ.GetNaturalGravity());Ω.ΖЍ(ȋ,Ȋ);Σ.Add(Ω);}}Ψ й(){if(Ι!=Ζ.Ϋ) return љ!=null?љ:Η;return Ελ;}void ŀ(Ψ έ,Μ Ц){Ελ=έ;if(ΰ==Μ.І) щ=Ц;}bool Ǻ(Ψ έ,float Ђ,bool ΙΩ,out Vector3 λ)
{if(ΙΩ){Vector3I Γζ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);IMySlimBlock Φ=Me.CubeGrid.GetCubeBlock(Γζ);if(Φ==null||!(Φ.FatBlock is IMyShipConnector)){λ=new Vector3();return false;}Vector3 ΘΠ=б(Υ,Φ.FatBlock.GetPosition()-ά);Vector3 ΘΟ=б(Υ,Φ.FatBlock.WorldMatrix.Forward);λ=έ.Δ-ΒЉ(έ.δ,έ.γ*-1,ΘΠ)-ΒЉ(έ.δ,έ.γ*-1,ΘΟ)*Ђ;
return true;}else{λ=έ.Δ;return true;}}Vector3 Ȏ(int ΘΝ,int ş,bool ΘЇ){if(!ΘЇ&&ΕЄ) return ΑΞ;float ΘΞ=((Р-1f)/2f)-ΘΝ;float ΘЊ=((Э-1f)/2f)-ş;ΑΞ=Ε.Δ+Ε.υ*ΘΞ*ľ()+ǰ*-1*ΘЊ*Ľ();ΕЄ=true;return ΑΞ;}Vector3 Βα(Vector3 ΙΗ,float ΓΧ){return ΙΗ+(Ќ*ΓΧ);}β ΒΥ(){float А=-1;
β ή=β.Ƿ;if(Ι!=Ζ.Ϋ){ΔЎ();if(ΰ!=Μ.в){Vector3 Њ=ě(Ќ,ǰ*-1,ά-Ε.Δ);if(Math.Abs(Њ.X)<=(float)(Р*ǵ)/2f&&Math.Abs(Њ.Y)<=(float)(Э*Ǵ)/2f){if(Њ.Z<=-1&&Њ.Z>=-į*2) return β.Ʋ;if(Њ.Z>-1&&Њ.Z<ί*2) return β.Ƿ;}if(ȫ(Ε.Δ,ref А)) ή=β.Ƿ;}if(Η.ε){if(ȫ(Η.Δ,ref А)) ή=β.Ƕ;
for(int i=0;i<Σ.Count;i++)
if(ȫ(Σ[i].Δ,ref А)) ή=β.ȏ;if(Vector3.Distance(ά,Η.Δ)<dockDist*ί) ή=β.Ƕ;if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null)
ή=β.Ƕ;}}else{Vector3 λ=new Vector3();IMyShipConnector Ȃ=Т(MyShipConnectorStatus.Connected);if(Η.ε){if(ȫ(Η.Δ,ref А)) ή=β.Ƕ;if(Ǻ(Η,dockDist,true,out λ))
if(ȫ(λ,ref А)) ή=β.Ƕ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Η.Δ)<5)
return β.ΒΚ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Η.Δ)>dockDist*ί&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ί)
if(ȫ(Σ[i].Δ,ref А)) ή=β.ȏ;if(Ε.ε){if(ȫ(Ε.Δ,ref А)) ή=β.Ƿ;if(Ǻ(Ε,dockDist,true,out λ))
if(ȫ(λ,ref А)) ή=β.Ƿ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5)
return β.ΒΛ;}}return ή;}bool ȫ(Vector3 λ,ref float А){float Ђ=Vector3.Distance(λ,ά);if(Ђ<А||А==-1){А=Ђ;return true;}return false;}void ΒЅ(){if(ώ>0){Λ="Setup error! Can't start";return;}if(Ι==Ζ.Ϋ){Ȭ();return;}Ε.Δ=ά;Ε.ν=Υ.GetNaturalGravity();Ε.δ=Αδ;Ε.γ=Ɗ;Ε.υ=Ɣ;
Ќ=њ.WorldMatrix.Forward;ǰ=Ε.γ;if(Ќ==Υ.WorldMatrix.Down) ǰ=Υ.WorldMatrix.Backward;ȝ=ǵ;Ȝ=Ǵ;Ż(true,true);Ν(Α.ǎ);ΒЄ();}void Ż(bool Ǘ,bool ΘЈ){if(ΰ==Μ.в&&!Ǘ) return;bool Θό=Ǘ||ΰ==Μ.ΒΧ||Р!=м||Э!=л||Ī!=Ȥ;if(Θό){if(ΰ!=Μ.в){ΰ=Μ.Ƶ;Ȏ(Ы,Ь,ΘЈ);Λ="Job changed, lost progress";
}Ī=Ȥ;Р=м;Э=л;Ь=0;Ы=0;О="";Ѓ=-1;ĩ=(int)(DateTime.Now.Ticks/10000000%1000000);Ĭ=0;ȟ=0;У=0;М=0;ΒЊ(true);}}void Βό(){Д(ά,0);ђ(ȋ,true);}void ΒЄ(){Ǽ(г.ǋ);Βό();Ź(Ȇ,false);ǧ(ΔΚ);ΰ=Μ.І;Δμ(true);щ=ΰ;π(Ϊ.Ŵ);Ηρ();ΑΧ=true;Αθ=0;for(int i=Ž.Count-1;i>=0;i--)
if(Ų(Ž[i],false)) Αθ++;if(Αθ>0) Λ="Started with damage";}void φ(){if(ΰ==Μ.І){ΰ=Μ.Ŀ;Λ="Job paused";}Ν(Α.к);щ=ΰ;ǌ(false,0,0,0,0);ΒЈ();Δι(new Vector3(),false);ΕΜ();ΒϏ(ChargeMode.Auto);ΒЁ(false);ΕΘ(true);Ȑ(Α.к);Ĝ(false,false,0,0);ђ(ο,false);ђ(ц,true);ǧ(true);ΑЄ=false;Ʒ();Ɖ=false;
Βύ();ΑΧ=false;Ƣ=false;Ʃ=false;if(Б!=Ϊ.Main&&Б!=Ϊ.Ƴ&&Б!=Ϊ.ΒΗ&&Б!=Ϊ.ΑΑ)
π(Ϊ.Main);}void Ȭ(){β З=ΒΥ();if(Ι==Ζ.Ϋ){if(!Ε.ε||!Η.ε) return;ΒЄ();bool Γδ=Vector3.Distance(ά,Η.Δ)<Vector3.Distance(ά,Ε.Δ);if(Ţ==Μ.ķ) Γδ=true;if(Ţ==Μ.ĸ) Γδ=false;if(Γδ){ŀ(Η,Μ.ķ);switch(З){case β.ΒΚ:Ν(Α.ȯ);break;case β.ȏ:Ν(Α.Ш);break;case β.Ƕ:Ν(Α.Ǣ);break;default:Ν(Α.ǒ);break;
}}else{ŀ(Ε,Μ.ĸ);switch(З){case β.ΒΛ:Ν(Α.ȯ);break;case β.Ƿ:Ν(Α.Ǣ);break;case β.ȏ:Ν(Α.Ш);break;default:Ν(Α.ǒ);break;}}}else{if(ΰ!=Μ.Ŀ&&ΰ!=Μ.Ƶ) return;bool Ιά=ΰ==Μ.Ƶ;ΒЄ();bool Γι=ų(false)&&Η.ε;switch(З){case β.Ƿ:Ν(Γι?Α.ǹ:Α.ǎ);break;case β.ȏ:Ν(Γι?Α.Ё:Α.Ш);break;case β.Ƕ:Ν(Γι?Α.Ǣ:Α.Ǿ);break;
case β.Ʋ:{if(ȟ!=М||Ιά) Ν(Α.ǥ);else Ν(Α.ę);}break;default:break;}}}void ė(){if(ΰ==Μ.в&&!Η.ε) return;if(Ι==Ζ.Ϋ&&(!Ε.ε||!Η.ε)) return;Λ="Move to job";β З=ΒΥ();if(Ι==Ζ.Ϋ){ŀ(Ε,Μ.ĸ);switch(З){case β.Ƿ:Ν(Α.Ǣ);break;case β.ȏ:Ν(Α.Ш);break;case β.ΒΛ:return;default:Ν(Α.ǒ);break;
}Ȑ(Α.ȯ);}else{switch(З){case β.Ƿ:Ν(Α.Ш);break;case β.ȏ:Ν(Α.Ш);break;case β.Ƕ:Ν(Α.Ǿ);break;case β.Ʋ:Ν(Α.ǥ);break;default:break;}if(ΰ==Μ.в) Ȑ(Α.Ш);else Ȑ(Α.Ȫ);ΑЄ=true;}Βό();π(Ϊ.ΒЇ);Ź(Ȇ,false);щ=Μ.ǅ;}void Ƽ(){if(!Η.ε) return;Λ="Move home";β З=ΒΥ();if(Ι==Ζ.Ϋ){ŀ(Η,Μ.ķ);switch(З)
{case β.ȏ:Ν(Α.Ё);break;case β.Ƕ:Ν(Α.Ǣ);break;case β.ΒΚ:return;default:Ν(Α.ǒ);break;}Ȑ(Α.ȯ);}else{if(Т(MyShipConnectorStatus.Connected)!=null) return;if(Т(MyShipConnectorStatus.Connectable)!=null){Ν(Α.ĕ);Ȑ(Α.Ǿ);return;}switch(З){case β.Ƿ:Ν(Α.ǹ);break;case β.ȏ:Ν(Α.Ё);break;case β.Ƕ:Ν(Α.Ё);break;
case β.Ʋ:Ν(Α.ё);break;default:break;}Ȑ(Α.Ǿ);}Βό();π(Ϊ.ΒЇ);Ź(Ȇ,false);щ=Μ.Ǆ;}void Ν(Α ΓЄ){if(ΓЄ==Α.к) Ƥ=Α.к;if(Ƥ!=Α.к&&Τ==Ƥ&&ΓЄ!=Ƥ){φ();return;}Ϗ=true;Τ=ΓЄ;}void Ȑ(Α Ц){this.Ƥ=Ц;}void ΗЌ(){if(Τ==Α.ǎ){if(Ϗ){ϋ=0;if(ȟ!=М) Ĭ=0;ȟ=М;}if(ϋ==0){int Ƃ=ΗΔ(М);if(Ƃ<0)
{if(!ƶ()){Ν(Α.ё);return;}if(!ΑΣ||(Ƃ==-2)!=ΑΡ){ΑΣ=true;ΑΡ=Ƃ==-2;double a=(е%6)*Math.PI/3;Εθ=!ΑΡ?ά:Ε.Δ-Ќ*ί*2.5f+
(Ε.υ*(float) Math.Cos(a)+ǰ*(float) Math.Sin(a))*ί*1.2f;}Д(Εθ,10);Λ=ΑΡ?"Waiting for a free hole":"Waiting for the controller";return;}if(ΑΣ) Λ="";ΑΣ=false;if(Ƃ!=М){М=Ƃ;Ĭ=0;ȟ=М;}Ł ή=Ηψ(М);if(ή==Ł.Δθ){ΰ=Μ.ΒΧ;Λ="Job done";if(ď&&Η.ε){Ν(Α.ǹ);Ȑ(Α.Ǿ);щ=Μ.Ǆ;}
else{Ν(Α.Ļ);Ȑ(Α.Ȫ);щ=Μ.ǅ;}return;}if(ή==Ł.Δω){ϋ=1;ђ(ο,true);Ј=Ȏ(Ы,Ь,true);float h=Vector3.Dot(ά-Ј,Ќ);if((ά-Ј-Ќ*h).Length()>Math.Max(ľ(),Ľ())*1.5f) h=Math.Min(h,-ί);Д(h<-1?Ј+Ќ*h:Ј,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}}else{if(Ѓ>=0&&(ΔЌ()?Ǭ>=0&&Ǭ!=Ѓ:Βθ(Ѓ,true))){ϋ=0;return;}
if(Н<wpReachedDist){if(Vector3.Distance(ς,Ј)>1){Д(Ј,10);return;}Ν(Α.ę);return;}if(ȉ<0.3f&&р<0&&!ĭ){if(++ϋ<30) return;ϋ=1;Vector3 Αώ=Ј-ά;float Ȓ=Vector3.Dot(Αώ,Ќ);if(Ȓ>0&&(Αώ-Ќ*Ȓ).Length()<wpReachedDist){ΑΞ=ά;Ν(Α.ę);}else Д(Ј+Ќ*(Math.Min(0,-Ȓ)-ί),10);
return;}ϋ=1;}}if(Τ==Α.ę){if(Ϗ){ђ(ο,true);ǧ(false);Ј=Ȏ(Ы,Ь,false);Д(Βα(Ј,0),0);Ч(Ε.γ,Ε.δ,Ε.υ,false);ϋ=1;н=0;а=0;ΑΝ=0;Γσ=-1;į=Щ;Γξ=true;}if(!ƶ()){Ν(Α.ё);return;}if(ų(true)){ΖΫ=ȍ("","ORE",К.Ē);if((П==Ѐ.ΒΨ||П==Ѐ.Ƹ||
П==Ѐ.Βι||П==Ѐ.ƿ)&&Ι!=Ζ.Ǐ)
Ν(Α.Ė);else if((П==Ѐ.Δς||П==Ѐ.ΒΡ)&&Ι!=Ζ.Ǐ)
Ν(Α.ΒЋ);else
Ν(Α.ё);return;}У=Vector3.Distance(ά,Ј);if(У>Ĭ){Ĭ=У;Γξ=false;}if(Ι==Ζ.Ǐ&&Δώ()==MyDetectedEntityType.SmallGrid)
а+=2;else а-=2;а=Math.Max(100,Math.Min(400,а));if(ϋ>0&&ϋ<а){if(У>н){if(а>150) н=У;else н=(float) Math.Ceiling(У);ϋ=1;}else ϋ++;}else{if(ϋ>0){Λ="Ship stuck! Retrying";н=У;ϋ=0;Ĝ(false,true,0,ί*sensorRange);}Д(Βα(Ј,Math.Max(0,н-ί)),Βδ(false));if(Н<=wpReachedDist/2){ϋ=1;н=0;}
return;}Ĝ(false,true,ί*sensorRange,0);Vector3 ΘΕ=Ј+Ќ*У;bool ΖΑ=false;if(Vector3.Distance(ΘΕ,ά)>0.3f){Vector3 ΘΡ=Ј+Ќ*(У+0.1f);Д(ΘΡ,4);ΖΑ=true;}else{float ǝ=Βδ(true);Vector3 Εο=Βα(Ј,Math.Max(Щ+1,У+1));Ǧ(true,false,false,Εο,Εο-Ј,ǝ,ǝ);}bool ΓΨ=false;if(ч==я.ΒΝ||ч==я.ΒΜ)
{if(!ΖΑ){float ΓІ=0;foreach(IMyTerminalBlock ǳ in ο)
ΓІ+=ΗΛ(ǳ,"","",ч==я.ΒΜ?new string[]{"STONE"}:null);if(ΓІ>Γσ||У<Щ||Γξ){ΓΑ=0;ΑΝ=У;į=(int)(Math.Max(į,ΑΝ)+ί/2);}else{ΓΨ=У-ΑΝ>2&&ΓΑ>=20;ΓΑ++;}Γσ=ΓІ;}}else ΓΨ=У>=į;if(ȟ!=М){Ν(Α.ǥ);У=0;return;}if(ΓΨ){М++;Ν(Α.ǥ);У=0;return;}}if(Τ==Α.ǥ||Τ==Α.ё||Τ==Α.Ė)
{if(Ϗ){Ј=Ȏ(Ы,Ь,false);Vector3 Ȣ=ά-Ј;if(Τ==Α.ё&&Η.ε&&
(Ȣ-Ќ*Vector3.Dot(Ȣ,Ќ)).Length()>Math.Max(ľ(),Ľ())){Ν(Α.ǹ);return;}Ч(Ε.γ,Ε.δ,Ε.υ,false);ђ(ο,ŋ);ǧ(false);н=Vector3.Distance(ά,Ј);Ĝ(false,true,0,ί*sensorRange);}if(Ϗ) ƍ=false;Д(ƍ?Ј-Ќ*ί:Ј,Βδ(false));if(!ƍ&&Vector3.Distance(ά,Ј)>=н+5){ђ(ο,false);ǧ(true);Λ="Can´t return!";}
if(Н<wpReachedDist){if(Τ==Α.ё&&!ƍ){ƍ=true;return;}if(Τ==Α.ǥ&&ΑЄ) Ν(Α.Ļ);if(Τ==Α.ǥ) Ν(Α.ǎ);if(Τ==Α.Ė) Ν(Α.ƻ);if(Τ==Α.ё){if(Η.ε) Ν(Α.ǹ);else{φ();ė();Λ="Can´t return, no dock found";}}return;}}if(Τ==Α.Ё||Τ==Α.Ш){if(Τ==Α.Ш&&ΰ==Μ.І&&Ι!=Ζ.Ϋ){if(!ƶ()||ų(true)){Ν(Α.Ё);return;}
}bool ΓЏ=false;bool ΒΓ=false;bool Εά=false;float ņ=0;bool ΖΪ=false;Ψ Ω=null;if(Ϗ){if(Τ==Α.Ё||Ι==Ζ.Ϋ){if(Ι!=Ζ.Ϋ){if(ȑ(1)) ΒΙ();else{љ=null;ȇ=0;Ɖ=!ΒΠ(1);}}Ψ έ=й();η=new Βϋ();η.Ѝ=έ;η.Ƈ=followPathDock*ί;η.ΔΫ=useDockDirectionDist*ί;η.ņ=10;η.ť.Add(έ.Δ);Vector3 Ņ=new Vector3();
if(Ǻ(έ,dockDist*ί,true,out Ņ))
η.ť.Add(Ņ);else η.Ƈ*=1.5f;if(Ι==Ζ.Ϋ){if(έ==Η) η.Αα=Ε.Δ;if(έ==Ε) η.Αα=Η.Δ;η.Γα=dockDist*ί*1.1f;}}else if(Τ==Α.Ш){η=new Βϋ();η.Ѝ=Ε;η.Ƈ=followPathJob*ί;η.ΔΫ=useJobDirectionDist*ί;η.ņ=10;η.Αα=Η.Δ;η.Γα=dockDist*ί*1.1f;η.ť.Add(Ε.Δ);if(ΰ==Μ.в){if(!Η.ε||Σ.Count==0){φ();return;}
float ΘΩ=Vector3.Distance(Σ.First().Δ,Η.Δ);float ΘΪ=Vector3.Distance(Σ.Last().Δ,Η.Δ);if(ΘΩ<ΘΪ) η.Ѝ=Σ.Last();else η.Ѝ=Σ.First();}}Ɯ=new Vector3();ΖΪ=!Ηΰ(ά);ђ(ο,false);ǧ(true);Х=-1;double ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αα)<=η.Γα) continue;
double Ђ=Vector3.Distance(Σ[i].Δ,ά);if(ǫ==-1||Ђ<ǫ){Х=i;ǫ=Ђ;}}Ǳ=ΗΧ(η.Ѝ.Δ,Х);Ģ=null;}if(Τ==Α.Ё&&Ι!=Ζ.Ϋ&&!Ϗ&&!ź()){Ψ ΓΕ=й();Ɖ=!ΒΠ(1);if(й()!=ΓΕ){Ν(Α.Ё);return;}}if(Τ==Α.Ё&&Ι!=Ζ.Ϋ&&!Ϗ&&ȑ(1)&&ΒΙ()){Ν(Α.Ё);return;}bool ΕА=false;if(Τ==Α.Ё&&!Ϗ&&Х>=0&&Х<Σ.Count)
{Vector3 Ƅ=Σ[Х].Δ+ΔϏ(Х,Ǳ);int ΓЅ=Х+Ǳ;Vector3 Ś=Ǳ!=0&&ΓЅ>=0&&ΓЅ<Σ.Count?Σ[ΓЅ].Δ:η.Ѝ.Δ;if(ΗΪ(Ƅ,Ś)){if(!ΑΫ){ΑΫ=true;Ετ=ļ(Ƅ)?ά:Ƅ;}Д(Ετ,0);Ч(η.Ѝ.γ,η.Ѝ.δ,η.Ѝ.υ,false);Λ="Waiting for dock clearance";return;}ΑΫ=false;ΕА=ȑ(1)&&(ļ(ά)||ļ(Ś));}ΖЎ(Σ,Ǳ,η.ť,η.Ƈ,Ϗ,ref ϋ);
for(int i=0;i<η.ť.Count;i++){float Ђ=Vector3.Distance(ά,η.ť[i]);if(Ђ<=η.Ƈ) ΓЏ=true;if(Ђ<=η.ΔΫ) ΒΓ=true;}if(ΕА) ΒΓ=true;if(ΒΓ) ņ=η.ņ;float Ιξ=Ģ!=null?Ģ.Ћ:ȉ;float Ια=(float) Math.Max(ȉ*0.1f*ί,wpReachedDist);if((Н<Ια)||Ϗ){if(!Ϗ) Х+=Ǳ;if(Ǳ==0||Х>Σ.Count-1||Х<0)
ΓЏ=true;else{Δΰ=Ǳ>0?Σ.Count-1-Х:Х;Ω=Σ[Х];Ģ=Ω;if(Х>=1&&Х<Σ.Count-1)
Ɯ=Ω.Δ-Σ[Х-Ǳ].Δ;else Ģ=null;ς=Ω.Δ+ΔϏ(Х,Ǳ);Εά=true;}}if(ΒΓ)
Ч(η.Ѝ.γ,η.Ѝ.δ,η.Ѝ.υ,false);else if(ΖΪ)
ΒΘ(η.Ѝ.γ,10,true);else if(Εά&&Ω!=null){if(Ǳ>0) Ч(Ω.γ,Ω.δ,Ω.υ,90,false);else Ч(Ω.γ,-Ω.δ,-Ω.υ,90,false);}Ǧ(true,false,true,ς,Ɯ,Ģ==null?0:Ģ.Ћ,ņ);if(ΓЏ){Δΰ=0;if(Τ==Α.Ё&&!ΗΒ()){Д(ς,0);return;}if(Τ==Α.Ё&&Ɖ){Д(ς,0);Λ="Waiting for a free dock";return;}if(Τ==Α.Ё||Ι==Ζ.Ϋ){Ν(Α.Ǣ);return;}
if(Τ==Α.Ш&&ΑЄ){Ν(Α.Ļ);return;}if(Τ==Α.Ш){Ν(Α.ǎ);return;}}}if(Τ==Α.Ǣ||Τ==Α.ŷ){if(Ι!=Ζ.Ϋ&&ź()){if(!ȑ(1)){Ν(Α.Ё);return;}}else if(Ι!=Ζ.Ϋ){Ψ ΓΕ=й();if(!ΒΠ(2)||й()!=ΓΕ){Ν(Α.Ё);return;}}Ψ έ=й();if(Ϗ){if(!Ǻ(έ,dockDist*ί,true,out ς)){Ǽ(г.ŭ);φ();return;}
Д(ς,0);ΒΘ(έ.γ,90,true);}if(Н<followPathDock*ί&&Н!=-1){Д(ς,10);Ч(έ.γ,έ.δ,έ.υ,false);}if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null){Ν(Α.ĕ);return;}if(Н<wpReachedDist/2&&Н!=-1){Ν(Α.đ);return;}}if(Τ==Α.đ||Τ==Α.Ȫ){if(Ϗ)
{if(Τ==Α.đ){Ψ έ=й();if(!Ǻ(έ,dockDist*ί,true,out ς)){Ǽ(г.ŭ);φ();return;}Ǧ(true,true,false,ς,0);Ч(έ.γ,έ.δ,έ.υ,10,false);}if(Τ==Α.Ȫ){Ч(Ε.γ,Ε.δ,Ε.υ,0.5f,false);ς=Ε.Δ;Ǧ(true,true,false,ς,0);}}if(Ακ){ǌ(false,0,0,0,0);if(Τ==Α.đ) Ν(Α.ĕ);if(Τ==Α.Ȫ) φ();return;}}if(Τ==Α.ĕ)
{if(Т(MyShipConnectorStatus.Connected)!=null){if(Ι==Ζ.Ϋ) Ν(Α.ȯ);else Ν(Α.Ǿ);return;}Ψ έ=й();if(Ϗ){а=0;ΘΫ=DateTime.Now;ϋ=0;Ч(έ.γ,έ.δ,έ.υ,false);}Vector3I Γζ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);IMySlimBlock Εη=Me.CubeGrid.GetCubeBlock(Γζ);float Εφ=dockingSpeed;float Θυ=dockingSpeed*5;
float ΕΩ=Math.Max(1.5f,Math.Min(5f,ί*0.15f));if(!Ǻ(έ,0,true,out ς)||!Ǻ(έ,ΕΩ,true,out Ɯ)||
Εη==null||!Εη.FatBlock.IsFunctional){Ǽ(г.ŭ);φ();return;}if(а==1||(Vector3.Distance(ά,ς)<=ΕΩ*1.1f&&!Ϗ))
а=1;else{Vector3 ΙΠ=б(Υ,Ɯ-ά);Vector3 Γε=б(Υ,Υ.GetNaturalGravity());float ΘЋ=ƴ(ΙΠ,Γε,null);Εφ=Math.Min(Θυ,ΘЋ);}Ǧ(true,false,false,ς,ς-ά,dockingSpeed,Εφ);if(Ϗ) н=(float) Н;IMyShipConnector ΓΣ=Т(MyShipConnectorStatus.Connectable);if(ΓΣ!=null){Ǧ(false,false,false,ς,0);
if(ϋ>0) ϋ=0;ϋ--;if(ϋ<-5){ΓΣ.Connect();if(ΓΣ.Status==MyShipConnectorStatus.Connected){if(Ι==Ζ.Ϋ) Ν(Α.ȯ);else Ν(Α.Ǿ);ΒЈ();Ź(Ȇ,true);return;}}}else{float Εσ=(float) Math.Round(Н,1);if(Εσ<н){ϋ=-1;н=Εσ;}else ϋ++;if(ϋ>20){Ν(Α.ŷ);return;}}}if(Τ==Α.Ǿ||Τ==Α.ȯ||
Τ==Α.ΑΗ||Τ==Α.ƺ||Τ==Α.Ĕ){bool ΒА=false,ΒБ=false;if(Ι==Ζ.Ϋ){if(й()==Η) ΒА=true;else if(й()==Ε) ΒБ=true;}if(Ϗ){Ƣ=false;if(Т(MyShipConnectorStatus.Connected)==null){Ν(Α.ǒ);return;}ΒЈ();if(ΒА) ΑΔ(ό.ǖ);if(ΒБ) ΑΔ(ύ.ǖ);ĳ=false;ȓ=false;Ț=false;ƫ=false;}if(Т(MyShipConnectorStatus.Connected)==null){φ();Ǽ(г.ΔЈ);return;}
if(ΰ!=Μ.І||ъ==-1||ǩ==у.None) ȓ=true;else if(ǿ>=100f) ȓ=true;else if(ǿ<=99f) ȓ=false;if(ΰ!=Μ.І||ы==-1||і.Count==0) Ț=true;else if(ț>=100f) Ț=true;else if(ț<=99) Ț=false;if(ΰ!=Μ.І||Ю==-1||Č.Count==0) ƫ=true;else ƫ=Ĵ>=Ю;ł т=null;if(ΒА) т=ό;
if(ΒБ) т=ύ;if(т!=null&&(т.Џ==ι.ǈ||т.Џ==ι.Ǉ)) ȓ=true;if(т!=null&&(т.Џ==ι.Βς)) if(!ĳ) ȓ=false;if(т!=null&&(т.Џ==ι.Βφ||т.Џ==ι.Βτ)) Ț=true;if(т!=null&&(т.Џ==ι.Βυ)) if(!ĳ) Ț=false;if(ĉ){ChargeMode Εζ=ȓ?ChargeMode.Auto:ChargeMode.Recharge;if(т!=null&&(т.Џ==ι.Ǉ||т.Џ==ι.ǈ))
Εζ=ChargeMode.Discharge;ΒϏ(Εζ);ΒЁ(!Ț);}if(!ĳ){if(Ι==Ζ.Ϋ)
ĳ=ΰ!=Μ.І||ΗΓ(Ϗ,true)||Ʃ;else
ĳ=ΰ!=Μ.І||ΔЉ();}else{if(!ȓ) Ν(Α.Ĕ);if(!Ț) Ν(Α.ƺ);if(!ƫ) Ν(Α.ΑΗ);Ϗ=false;}if(ĳ&&ȓ&&Ț&&ƫ){ΒϏ(ChargeMode.Auto);ΒЁ(false);if(ΰ==Μ.І){if(Ι==Ζ.Ϋ){if(й()==Η) ΑΔ(ό.Ǚ);else if(й()==Ε) ΑΔ(ύ.Ǚ);if(й()==Η) ŀ(Ε,Μ.ĸ);else ŀ(Η,Μ.ķ);}}Ν(Α.ǒ);return;}}if(Τ==Α.ǒ){if(Ʈ){if(!Δο()) return;
Ʈ=false;Ν(Α.ǒ);return;}if(Ϗ){if(!Δο()){Ʈ=true;return;}IMyShipConnector Ȃ=Т(MyShipConnectorStatus.Connected);if(Ȃ==null){Ν(Α.Ш);return;}IMyShipConnector α=Ȃ.OtherConnector;ΕΗ(Ȃ,false);Ź(Ȇ,false);Ψ έ=null;Ψ ΕЍ=Ι==Ζ.Ϋ?Η:й();if(Vector3.Distance(Ȃ.GetPosition(),ΕЍ.Δ)<5f&&Η.ε) έ=ΕЍ;
if(Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5f&&Ε.ε) έ=Ε;if(έ!=null){if(!Ǻ(έ,dockDist*ί,true,out ς)){Ǽ(г.ŭ);φ();return;}Д(ς,5);Ч(έ.γ,έ.δ,έ.υ,false);}else Д(ά+α.WorldMatrix.Forward*dockDist*ί,5);if(ΰ==Μ.І) Ǽ(г.ǋ);}if(Н<wpReachedDist){ђ(ц,true);Ν(Α.Ш);return;}}if(Τ==Α.Ļ)
{if(Ϗ){ǧ(true);ђ(ο,false);ς=Ε.Δ;Д(ς,20);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(Н<wpReachedDist/2){Ν(Α.Ȫ);return;}}if(Τ==Α.ǹ){if(Ϗ){ǧ(true);ђ(ο,false);int А=-1;double ǫ=-1;bool Ζι=Βο();for(int ƛ=0;ƛ<2&&А==-1;ƛ++,Ζι=false)
for(int i=Σ.Count-1;i>=0;i--){if(Ζι&&Vector3.Distance(Σ[i].Δ,Ε.Δ)<ΔЍ()+ί) continue;double Ђ=Vector3.Distance(Σ[i].Δ,ά);if(ǫ==-1||Ђ<ǫ){А=i;ǫ=Ђ;}}if(А==-1){Ν(Α.Ё);return;}ς=Σ[А].Δ;Д(ς,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(Н<wpReachedDist){Ν(Α.Ё);return;}}if(Τ==Α.ƻ)
{bool ѕ=false;if(Ϗ){ǧ(true);if((П==Ѐ.ΒΨ||П==Ѐ.Ƹ)&&ǁ()&&
ĺ(Ќ,Υ.GetNaturalGravity())<25&&Р>=2&&Э>=2){Vector3 Ƌ=ά;if(Ы>0&&Ь<Э-1) Ƌ=Ȏ(Ы-1,Ь+1,true);else if(Ы<Р-1&&Ь<Э-1) Ƌ=Ȏ(Ы+1,Ь+1,true);else if(Ы<Р-1&&Ь>0) Ƌ=Ȏ(Ы+1,Ь-1,true);else if(Ы>0&&Ь>0) Ƌ=Ȏ(Ы-1,Ь-1,true);else ѕ=true;if(!ѕ) Д(Ƌ,10);
}else ѕ=true;}if(Н<wpReachedDist/2) ѕ=true;if(ѕ){Ν(Α.ΑΖ);return;}}if(Τ==Α.ΒЋ||Τ==Α.ΑΖ){if(Ϗ){Ǧ(true,true,false,ά,0);ђ(ο,false);ǧ(true);ϋ=-1;а=П==Ѐ.Βι||П==Ѐ.ƿ?0:-1;}bool Ĥ=!ƶ();int ΑЍ=ȍ("STONE","ORE",К.Ē);if(П==Ѐ.ΒΡ||П==Ѐ.ƿ||П==Ѐ.Ƹ)
ΑЍ+=ȍ("ICE","ORE",К.Ē);bool Θδ=ΑЍ>0;bool Γΰ=false;if(а>=0){float Ń=(float) Math.Sin(Δτ(а))*ǵ/3f;float ΘΗ=(float) Math.Cos(Δτ(а))*Ǵ/3f;Vector3 ΘΜ=Ȏ(Ы,Ь,true)+ΒЉ(Ќ,ǰ*-1,new Vector3(Ń,ΘΗ,0));Д(ΘΜ,0.3f);if(Н<Math.Min(ǵ,Ǵ)/10f) а+=5f;if(а>=360) а=0;}if(ϋ==-1||ΑЍ<ϋ){ϋ=ΑЍ;н=0;}
else{н++;if(н>50) Γΰ=true;}if(!Θδ||Ĥ||Γΰ){if(!Ĥ){int ΙΦ=ȍ("","ORE",К.Ē);if(ų(true)) Ĥ=true;else if(100-(Г(ΙΦ,ΖΫ)*100)<minEjection) Ĥ=true;else Ǽ(г.ǋ);}if(Γΰ&&Ĥ) Λ="Ejection failed";if(Τ==Α.ΑΖ){if(Ĥ){if(Η.ε) Ν(Α.ǹ);else{φ();ė();Λ="Can´t return, no dock found";}}
else Ν(Α.ǎ);}else if(Ĥ) Ν(Α.ё);else Ν(Α.ę);return;}}Ϗ=false;}void ΖЎ(List<Ψ>Đ,int ζ,List<Vector3>Ʀ,float ǟ,bool Ȉ,ref int ǔ){if(Ȉ){for(int i=0;i<Đ.Count;i++) Đ[i].Ћ=0;ǔ=-1;return;}if(ζ==0) return;int ќ=ζ*-1;if(ǔ==-1) ǔ=ќ>0?1:Đ.Count-2;int ΖΓ=0;while(ǔ>=1&&ǔ<Đ.Count-1)
{if(ΖΓ>50) return;ΖΓ++;try{if((ќ<0&&ǔ>=1)||(ќ>0&&ǔ<=Đ.Count-2)){Ψ θ=Đ[ǔ];bool ΖΧ=false;for(int j=0;j<Ʀ.Count;j++)
if(Vector3.Distance(θ.Δ,Ʀ[j])<=ǟ){ΖΧ=true;break;}if(!ΖΧ){Ψ ΑЀ=Đ[ǔ-ќ];Ψ Ś=Đ[ǔ+ќ];Vector3 ΙΡ=θ.Δ-Ś.Δ;Vector3 ΔΦ=ΑЀ.Δ-θ.Δ;Vector3 ΘІ=θ.Δ+Vector3.Normalize(ΙΡ)*ΔΦ.Length();Vector3 ΘΧ=ΑЀ.Δ-ΘІ;Vector3 Αρ=ě(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ΘΧ);Vector3 Θτ=ě(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ΔΦ);
Vector3 Ασ=ě(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,θ.ν);θ.Ћ=(float) Math.Sqrt(Math.Pow(ΑЀ.Ћ,2)+Math.Pow(ƴ(-Θτ,Ασ,θ),2));for(int j=0;j<Ʀ.Count;j++)
if(Vector3.Distance(ΑЀ.Δ,Ʀ[j])<=ǟ){Vector3 Ατ=ě(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,Ʀ[j]-θ.Δ);float ΙΙ=ƴ(-Ατ,Ασ,θ);θ.Ћ=Math.Min(θ.Ћ,ΙΙ)/2f;}if(Αρ.Length()==0) Αρ=new Vector3(0,0,1);Vector3 Θσ=ě(θ.δ,θ.γ*-1,θ.ν);float ΙΛ=ů(-Αρ,Θσ,θ);float ΑΘ=Г(ΙΛ,ı);float ΔΤ=(float) Math.Sqrt(Г(Αρ.Length()*1.0f,0.5f*ΑΘ));
θ.Ћ=Math.Min(θ.Ћ,(ΔΦ.Length()/ΔΤ)*Ǩ);}}}catch{return;}ǔ+=ќ;}ǔ=-1;}void ΒЊ(bool Ȉ){if(Ȉ){ƒ=0;return;}if(Ι==Ζ.Ϋ) return;float ѕ=М*Math.Max(1,Щ);if(ȟ==М) ѕ+=Math.Min(Щ,У);float ΙΥ=Р*Э*Math.Max(1,Щ);ƒ=Math.Max(ƒ,(float) Math.Min(ѕ/ΙΥ*100.0,100));}MyDetectedEntityType Δώ()
{try{if(ǻ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βδ(bool Ǯ){if(Ι==Ζ.Ǐ&&Δώ()==MyDetectedEntityType.None&&!Ų(ξ,true))
return fastSpeed;else return Ǯ?ū:Ū;}void ΗІ(){float ΖΠ=0,Εν=0,Ĩ=0,ΓЇ=0;for(int i=0;i<Ȱ.Count;i++){IMyBatteryBlock ƀ=Ȱ[i];if(!ǻ(ƀ,true)) continue;ΖΠ+=ƀ.MaxStoredPower;Εν+=ƀ.CurrentStoredPower;Ĩ+=ƀ.CurrentInput;ΓЇ+=ƀ.CurrentOutput;}ǿ=(float) Math.Round(Г(Εν,ΖΠ)*100,1);
if(Ĩ>=ΓЇ) ǩ=у.Ĕ;else ǩ=у.Δυ;if(Ĩ==0&&ΓЇ==0||ǿ==100.0) ǩ=у.к;if(Ȱ.Count==0) ǩ=у.None;}void ΗЊ(){float Εό=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζω=і[i];if(!ǻ(Ζω,true)) continue;Εό+=(float) Ζω.FilledRatio;}ț=Г(Εό,і.Count)*100f;}void ΘΑ(){Ĵ=0;try{for(int i=0;i<Č.Count;i++)
{IMyReactor ΓА=Č[i];if(!ǻ(ΓА,true)) continue;List<MyInventoryItem>д=new List<MyInventoryItem>();ΓА.GetInventory(0).GetItems(д);float Ǔ=0;for(int j=0;j<д.Count;j++){MyInventoryItem Ж=д[j];if(Ж.Type.SubtypeId.ToUpper()=="URANIUM"&&Ж.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǔ+=(float) Ж.Amount;}if(Ǔ<Ĵ||i==0){Ĵ=Ǔ;Γϋ=ΓА.CustomName;}}}catch(Exception e){Ȟ=e;}}void ΗЇ(){Ɩ=0;ƅ=0;try{for(int i=0;i<Ŋ.Count;i++){IMyTerminalBlock Φ=Ŋ[i];if(!ǻ(Φ,true)) continue;ƅ+=(float) Φ.GetInventory(0).CurrentVolume;Ɩ+=(float) Φ.GetInventory(0).MaxVolume;
}Απ=(float) Math.Min(Math.Round(Г(ƅ,Ɩ)*100,1),100.0);}catch(Exception e){Ȟ=e;}}void ΗЋ(){try{ǘ.Clear();for(int i=0;i<Ŋ.Count;i++){IMyTerminalBlock Φ=Ŋ[i];if(!ǻ(Φ,true)) continue;Δη(Φ,К.ű);}if(П!=Ѐ.Βρ){for(int i=0;i<ğ.Count;i++){IMyTerminalBlock Φ=ğ[i];if(!ǻ(Φ,true)) continue;
Δη(Φ,К.Ηέ);}}Ηώ(ǘ);}catch(Exception e){Ȟ=e;}}void ΗΑ(){if(ΑΧ){if(Δύ().Count>Αθ){ΑΧ=false;if(о!=и.Ű){φ();if(о==и.Βέ) ė();if(о==и.Ǌ)
if(Η.ε) Ƽ();else ė();}Λ="Damage detected";}}}bool ƶ(){if(!Αι) return true;if(ΰ==Μ.І){if(ъ>0&&ǩ!=у.None)
if(ǿ<=ъ){Λ="Low energy! Move home";return false;}if(Ю>0&&Č.Count>0)
if(Ĵ<=Ю){Λ="Low fuel: "+Γϋ;return false;}if(ы>0&&і.Count>0)
if(ț<=ы){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δύ(){List<IMyTerminalBlock>ģ=new List<IMyTerminalBlock>();for(int i=0;i<Ž.Count;i++){IMyTerminalBlock Φ=Ž[i];if(Ų(Φ,false)) ģ.Add(Φ);}return ģ;}bool Ų(IMyTerminalBlock Φ,bool ΔΜ){return(!ǻ(Φ,ΔΜ)||!Φ.IsFunctional);
}bool ǻ(IMyTerminalBlock Φ,bool ΔΜ){if(Φ==null) return false;try{IMyCubeBlock ΕЃ=Me.CubeGrid.GetCubeBlock(Φ.Position).FatBlock;if(ΔΜ) return ΕЃ==Φ;else return ΕЃ.GetType()==Φ.GetType();}catch{return false;}}float ΗΛ(IMyTerminalBlock Φ,String ю,String Ζπ,String[]ǭ){float ȃ=0;for(int i=0;i<Φ.InventoryCount;i++)
{IMyInventory ő=Φ.GetInventory(i);List<MyInventoryItem>д=new List<MyInventoryItem>();ő.GetItems(д);foreach(MyInventoryItem Ж in д){if(ǭ!=null&&(ǭ.Contains(Ж.Type.TypeId.ToUpper())||ǭ.Contains(Ж.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Ж.Type.TypeId.ToUpper()==ю)&&(Ζπ==""||Ж.Type.SubtypeId.ToUpper()==Ζπ))
ȃ+=(float) Ж.Amount;}}return ȃ;}Ǥ ΗΡ(String ψ,String σ,К З,bool ΘΣ){ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<ǘ.Count;i++){Ǥ Ж=ǘ[i];if(Ж.ψ.ToUpper()==ψ&&Ж.σ.ToUpper()==σ&&(Ж.Ŗ==З||З==К.Ē))
return Ж;}Ǥ ή=null;if(ΘΣ){ή=new Ǥ(ψ,σ,0,З);ǘ.Add(ή);}return ή;}int ȍ(String ψ,String σ,К З){return ȍ(ψ,σ,З,null);}int ȍ(String ψ,String σ,К З,String[]ǭ){int ȃ=0;ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<ǘ.Count;i++){Ǥ Ж=ǘ[i];if(ǭ!=null&&ǭ.Contains(Ж.ψ.ToUpper())) continue;
if((ψ==""||Ж.ψ.ToUpper()==ψ)&&(σ==""||Ж.σ.ToUpper()==σ)&&(Ж.Ŗ==З||З==К.Ē))
ȃ+=Ж.Ǔ;}return ȃ;}void Δη(IMyTerminalBlock Φ,К З){for(int i=0;i<Φ.InventoryCount;i++){List<MyInventoryItem>д=new List<MyInventoryItem>();Φ.GetInventory(i).GetItems(д);for(int j=0;j<д.Count;j++)
ΗΡ(д[j].Type.SubtypeId,д[j].Type.TypeId.Replace("MyObjectBuilder_",""),З,true).Ǔ+=(int) д[j].Amount;}}void Ηώ(List<Ǥ>Ф){for(int i=Ф.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ǥ a=Ф[j];Ǥ b=Ф[j+1];if(a.Ǔ<b.Ǔ) Ф.Move(j,j+1);}}bool ų(bool Ιλ){if(ȩ&&Ι!=Ζ.Ϋ)
if(С!=-1&&ı>=С){Λ="Ship too heavy";return true;}if(Απ>=ǯ||Ƣ){Ƣ=false;Λ="Ship is full";return true;}return false;}bool ΔЉ(){String[]ǭ=null;if(!ȧ) ǭ=new string[]{"ICE"};if(Ι==Ζ.Ŷ) return ȍ("","ORE",К.ű,ǭ)==0;if(Ι==Ζ.Ǐ) return ȍ("","COMPONENT",К.ű,ǭ)==0;else return ȍ("","",К.ű,ǭ)==0;
}bool Ηα(){List<IMyCockpit>ΓΡ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΓΡ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΡ.Count;i++)
if(ΓΡ[i].IsUnderControl) return true;return false;}bool ΗΓ(bool Ȉ,bool ΑЇ){IMyShipConnector Л=Т(MyShipConnectorStatus.Connected);if(Л==null) return false;if(Vector3.Distance(Η.Δ,Л.GetPosition())<5)
return Δπ(ό,Ȉ,ΑЇ);if(Vector3.Distance(Ε.Δ,Л.GetPosition())<5)
return Δπ(ύ,Ȉ,ΑЇ);return false;}bool Δπ(ł ȁ,bool Ȉ,bool ΑЇ){if(Ȉ) ȁ.Ην();ȁ.ΗЏ();bool с=false;String э="";switch(ȁ.Џ){case ι.Βσ:{э="Waiting for command";с=false;break;}case ι.ΕΒ:{э="Waiting for cargo";с=ų(true);break;}case ι.ΕΑ:{э="Unloading";с=ΔЉ();break;}case ι.Βψ:{с=true;break;}
case ι.Βς:{э="Charging batteries";с=ǿ>=100f;break;}case ι.ǈ:{э="Discharging batteries";с=ǿ<=25f;break;}case ι.Ǉ:{э="Discharging batteries";с=ǿ<=0f;break;}case ι.Βυ:{э="Filling up hydrogen";с=ț>=100f;break;}case ι.Βφ:{э="Unloading hydrogen";с=ț<=25f;break;}
case ι.Βτ:{э="Unloading hydrogen";с=ț<=0f;break;}case ι.Βχ:{bool ΕЏ=Ηα();if(!ΕЏ) ȁ.ΓЌ=true;с=ȁ.ΓЌ&&ΕЏ;э="Waiting for passengers";break;}}if(!с) ȁ.ΒΟ(true);if(с&&ȁ.Βζ()){с=ȁ.ΒΟ(false);э="Undocking in: "+Βί((int) Math.Max(0,ȁ.Ǖ-ȁ.ā));}if(ΑЇ) ΖІ=э;return с;}void ΖЊ()
{if(!Œ) return;if(ο.Count<=1) return;float ΖϏ=0,Ζώ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǳ=ο[i];if(Ų(ǳ,true)) continue;ΖϏ+=(float) ǳ.GetInventory(0).MaxVolume;Ζώ+=(float) ǳ.GetInventory(0).CurrentVolume;}float Εέ=(float) Math.Round(Г(Ζώ,ΖϏ),5);for(int ƛ=0;ƛ<Math.Max(1,Math.Floor(ο.Count/10f));ƛ++)
{float Γλ=0,Γϊ=0;float ΕЋ=0,ΖΞ=0;IMyTerminalBlock Ș=null,Ă=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǳ=ο[i];if(Ų(ǳ,true)) continue;float ΓЁ=(float) ǳ.GetInventory(0).MaxVolume;float Αβ=Г((float) ǳ.GetInventory(0).CurrentVolume,ΓЁ);if(Ș==null||Αβ>Γλ){Ș=ǳ;Γλ=Αβ;ΕЋ=ΓЁ;}
if(Ă==null||Αβ<Γϊ){Ă=ǳ;Γϊ=Αβ;ΖΞ=ΓЁ;}}if(Ș==null||Ă==null||Ș==Ă) return;if(checkConveyorSystem&&!Ș.GetInventory(0).IsConnectedTo(Ă.GetInventory(0))){if(ΓΔ>20) Λ="Inventory balancing failed";else ΓΔ++;return;}ΓΔ=0;List<MyInventoryItem>д=new List<MyInventoryItem>();
Ș.GetInventory(0).GetItems(д);float ƭ=0;if(д.Count==0) continue;MyInventoryItem Αγ=д[0];String ΖΒ=Αγ.Type.TypeId+Αγ.Type.SubtypeId;if(!ΖЅ.TryGetValue(ΖΒ,out ƭ)){if(Ηζ(Ș.GetInventory(0),0,Ă.GetInventory(0),out ƭ))
ΖЅ.Add(ΖΒ,ƭ);else return;}float ΙΣ=((Γλ-Εέ)*ΕЋ/ƭ);float ΙΤ=((Εέ-Γϊ)*ΖΞ/ƭ);int ΔΩ=(int) Math.Min(ΙΤ,ΙΣ);if(ΔΩ<=0) return;if((float) Αγ.Amount<ΔΩ)
Ș.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,Αγ.Amount);else
Ș.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,ΔΩ);}}bool Ηζ(IMyInventory ƣ,int Β,IMyInventory ΘΦ,out float ΖЄ){ΖЄ=0;float ΘΚ=(float) ƣ.CurrentVolume;List<MyInventoryItem>ΓΖ=new List<MyInventoryItem>();ƣ.GetItems(ΓΖ);float Εΰ=0;for(int i=0;i<ΓΖ.Count;i++) Εΰ+=(float) ΓΖ[i].Amount;
ƣ.TransferItemTo(ΘΦ,Β,null,null,1);float ΖЃ=ΘΚ-(float) ƣ.CurrentVolume;List<MyInventoryItem>ΒЍ=new List<MyInventoryItem>();ƣ.GetItems(ΒЍ);float ΕΥ=0;for(int i=0;i<ΒЍ.Count;i++) ΕΥ+=(float) ΒЍ[i].Amount;if(ΖЃ==0f||!Βκ(0.9999,Εΰ-ΕΥ,1.0001))
return false;ΖЄ=ΖЃ;return true;}void ΗЉ(){Vector3 Ə=ς+Ȧ;Vector3 ǲ=Ə-ά;if(ǲ.Length()==0) ǲ=new Vector3(0,0,-1);Vector3 Ατ=б(Υ,ǲ);Vector3 Ζϊ=Vector3.Normalize(Ατ);Vector3 ŕ=б(Υ,Υ.GetNaturalGravity());float ΙΔ=ΓЊ>0?Math.Max(0,1-ĺ(ǲ,Εώ)/5):0;float Ћ=(float) Math.Min((Γώ>0?Γώ:1000f),Math.Max(ƴ(-Ατ,ŕ,null),ΓЊ*ΙΔ));
if(!Ζύ) Ћ=0;if(Ζκ) Ћ=Math.Max(0,1-ΑΥ/ΑΙ)*Ћ;if(generalSpeedLimit>0) Ћ=Math.Min(generalSpeedLimit,Ћ);if(ĭ) Ћ=0;if(р>=0) Ћ=Math.Min(Ћ,р);if(Βο()) Ћ=Math.Min(Ћ,jobAreaSpeed);if(Ζλ) Ћ*=(float) Math.Min(1,Г(ǲ.Length(),wpReachedDist)/2);Vector3 ΖΜ=б(Υ,Υ.GetShipVelocities().LinearVelocity);
float ΘΖ=(float)(Math.Max(0,15-ĺ(-Ζϊ,-ΖΜ))/15)*0.85f+0.15f;ΔΠ+=Math.Sign(ΘΖ-ΔΠ)/10f;Vector3 ȱ=Ζϊ*Ћ*ΔΠ-(ΖΜ);Vector3 ΓΓ=Βγ(ȱ,null);if(ΑЏ&&Н>0.1f){ȱ.X*=ΒЃ(ȱ.X,ref ΑА.X,1f,ΓΓ.X,20);ȱ.Y*=ΒЃ(ȱ.Y,ref ΑА.Y,1f,ΓΓ.Y,20);ȱ.Z*=ΒЃ(ȱ.Z,ref ΑА.Z,1f,ΓΓ.Z,20);}else ΑА=new Vector3(1,1,1);
Ƨ=ı*ȱ-ŕ*ı;Δι(Ƨ,ΑЏ);Н=Vector3.Distance(ά,Ə);}float ΒЃ(float value,ref float Ц,float ќ,float Ƞ,float ΓϏ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ц)) Ц+=Math.Sign(Ц)*ќ;else Ц=value;if(value==0) Ц=1;float ή=Math.Abs(Ц);if(ή<ΓϏ||Ƞ==0) return ή;Ц=Math.Min(ΓϏ,Math.Max(-ΓϏ,Ц));
ή=Math.Abs(Ƞ);return ή;}void ΕΣ(){float ь=90,Ǎ=90,ћ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǯ,ј,ƕ,Γχ;if(Γψ){Ǯ=Vector3.Normalize(ς-ά);ƕ=б(Υ,Ǯ);ј=б(Υ,ΔΞ);ь=ĺ(ƕ,new Vector3(0,-1,0))-90;Ǎ=ē(ј,new Vector3(-1,0,0),ј.Y);
ћ=ē(ƕ,new Vector3(-1,0,0),ƕ.Z);}else{Ǯ=ΔΟ;ј=б(Υ,ΔΞ);ƕ=б(Υ,ΔΟ);Γχ=б(Υ,Ζϋ);ь=ē(ј,new Vector3(0,0,1),ј.Y);Ǎ=ē(ј,new Vector3(-1,0,0),ј.Y);ћ=ē(Γχ,new Vector3(0,0,1),Γχ.X);}if(ΕΧ&&ǁ()){Vector3 Γε=Υ.GetNaturalGravity();ј=б(Υ,Γε);ь=ē(ј,new Vector3(0,0,1),ј.Y);
Ǎ=ē(ј,new Vector3(-1,0,0),ј.Y);}if(!Βκ(-45,Ǎ,45)){ь=0;ћ=0;}if(!Βκ(-45,ћ,45)) ь=0;ǌ(Γη,1,(-ь)*ǝ,(-ћ)*ǝ,(-Ǎ)*ǝ);ΑΥ=Math.Max(Math.Abs(ь),Math.Max(Math.Abs(Ǎ),Math.Abs(ћ)));Ακ=ΑΥ<=ΑΙ;}void ΖЏ(IMyTerminalBlock ΑЁ){if(ΑЁ==null) return;ǡ=new float[3,2];ѝ=new Dictionary<string,float[,]>();
for(int i=0;i<ȋ.Count;i++){IMyThrust ж=ȋ[i];if(!ж.IsFunctional) continue;Vector3 ζ=б(ΑЁ,ж.WorldMatrix.Backward);float Ā=ж.MaxEffectiveThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) ǡ[0,0]+=Ā;else ǡ[0,1]-=Ā;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) ǡ[1,0]+=Ā;else ǡ[1,1]-=Ā;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) ǡ[2,0]+=Ā;else ǡ[2,1]-=Ā;String ю=ΔЁ(ж);float[,]ў=null;if(ѝ.ContainsKey(ю)) ў=ѝ[ю];else{ў=new float[3,2];ѝ.Add(ю,ў);}float Ƞ=ж.MaxThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) ў[0,0]+=Ƞ;else ў[0,1]-=Ƞ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) ў[1,0]+=Ƞ;else ў[1,1]-=Ƞ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) ў[2,0]+=Ƞ;else ў[2,1]-=Ƞ;}}static String ΔЁ(IMyThrust ж){return ж.BlockDefinition.SubtypeId;}Vector3 Βμ(Vector3 є,float[,]ŗ){return new Vector3(
є.X>=0?ŗ[0,0]:ŗ[0,1],
є.Y>=0?ŗ[1,0]:ŗ[1,1],
є.Z>=0?ŗ[2,0]:ŗ[2,1]);}bool ΔЀ(Ψ Ω,String ю,out float Γί){Γί=0;int Β=Ȋ.IndexOf(ю);if(Β==-1||Ω.Ǡ==null||Β>=Ω.Ǡ.Length)
return false;Γί=Ω.Ǡ[Β];if(Γί==-1) return false;return true;}Vector3 Βγ(Vector3 є,Ψ Ω){if(Ω!=null){Vector3 ή=new Vector3();for(int i=0;i<ѝ.Keys.Count;i++){String ю=ѝ.Keys.ElementAt(i);float ƌ=0;if(!ΔЀ(Ω,ю,out ƌ))
return Βμ(є,ǡ);ή+=Βμ(є,ѝ.Values.ElementAt(i))*ƌ;}return ή;}return Βμ(є,ǡ);}float ů(Vector3 є,Ψ Ω){return ů(є,new Vector3(),Ω);}float ů(Vector3 є,Vector3 ν,Ψ Ω){Vector3 ΙΚ=Βγ(є,Ω);Vector3 Ā=ΙΚ+ν*ı;float ƞ=(Ā/є).AbsMin();return(float)(є*ƞ).Length();}float ƴ(Vector3 Ας,Vector3 ŕ,Ψ Ω)
{if(Ας.Length()==0) return 0;float ΕΦ=1;if(ŕ.Length()>0) ΕΦ=Math.Min(1,ĺ(-ŕ,Ας)/90)*0.4f+0.6f;float Ƨ=ů(Ας,ŕ,Ω);if(Ƨ==0) return 0.1f;float ΑΘ=Г(Ƨ,ı);float ΔΤ=(float) Math.Sqrt(Г(Ας.Length(),ΑΘ*0.5f));return ΑΘ*ΔΤ*ΕΦ*Ǩ;}float ƾ(Vector3 Ǯ,Vector3 ω,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Ασ=ě(Ǯ,ω,Vector3.Normalize(ν));float Ǘ=ů(-Ασ,Ω);return Ǘ/ν.Length();}void Δμ(bool Θκ){float џ=0;float Γβ=0.9f;if(Θκ){С=-1;ĵ=0;ΑЋ=null;if(ΰ!=Μ.в&&Ε.ν.Length()!=0){џ=Γβ*ƾ(Ε.δ,Ε.γ*-1,Ε.ν,null);if(џ<С||С==-1) С=џ;}if(Η.ε&&Η.ν.Length()!=0)
{џ=Γβ*ƾ(Η.δ,Η.γ*-1,Η.ν,null);if(џ<С||С==-1) С=џ;}return;}if(ĵ==-1) return;if(ĵ>=0){int Ζΰ=0;while(ĵ<Σ.Count){if(Ζΰ>100) return;Ζΰ++;Ψ Ω=Σ[ĵ];if(Ω.ν.Length()!=0f){џ=Γβ*Math.Min(
ƾ(Ω.δ,Ω.γ*-1,Ω.ν,Ω),
ƾ(Ω.δ*-1,Ω.γ*-1,Ω.ν,Ω));if(џ<С||С==-1) С=џ;}else ΑЋ=Ω;ĵ++;}ĵ=-1;}bool Γθ=true;float ċ=0;if(ΑЋ!=null){for(int i=0;i<ѝ.Count;i++){String ю=ѝ.Keys.ElementAt(i);float[,]ў=ѝ.Values.ElementAt(i);float ƌ=0;if(!ΔЀ(ΑЋ,ю,out ƌ)){Γθ=false;break;}for(int a=0;a<ў.GetLength(0);a++)
for(int b=0;b<ў.GetLength(1);b++){float Ğ=Math.Abs(ў[a,b]*ƌ);if(Ğ==0) continue;Γθ=true;if(ċ==0||Ğ<ċ) ċ=Ğ;}}}if(!Γθ){for(int a=0;a<ǡ.GetLength(0);a++)
for(int b=0;b<ǡ.GetLength(1);b++){float Ğ=Math.Abs(ǡ[a,b]);if(Ğ==0) continue;if(ċ==0||Ğ<ċ) ċ=Ğ;}}if(ċ>0){float Θχ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;џ=Г(ċ,Θχ);if(џ>0)
if(џ<С||С==-1) С=џ;}}void Δι(Vector3 Ǘ,bool Љ){if(!Љ){for(int i=0;i<ȋ.Count;i++)
ȋ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓΒ=Βγ(Ǘ,null);float Ιγ=Math.Min(1,Math.Abs(Г(Ǘ.X,ΓΒ.X)));float Ιδ=Math.Min(1,Math.Abs(Г(Ǘ.Y,ΓΒ.Y)));float Ιε=Math.Min(1,Math.Abs(Г(Ǘ.Z,ΓΒ.Z)));for(int i=0;i<ȋ.Count;i++){IMyThrust ж=ȋ[i];Vector3 ζ=Ηο(б(Υ,ж.WorldMatrix.Backward),1);
if(ζ.X!=0&&Math.Sign(ζ.X)==Math.Sign(Ǘ.X))
ж.SetValueFloat("Override",ж.MaxThrust*Ιγ);else if(ζ.Y!=0&&Math.Sign(ζ.Y)==Math.Sign(Ǘ.Y))
ж.SetValueFloat("Override",ж.MaxThrust*Ιδ);else if(ζ.Z!=0&&Math.Sign(ζ.Z)==Math.Sign(Ǘ.Z))
ж.SetValueFloat("Override",ж.MaxThrust*Ιε);else
ж.SetValueFloat("Override",0.0f);}}void ǌ(bool Љ,float ΘЃ,float ь,float ћ,float Ǎ){for(int i=0;i<Αε.Count;i++){IMyGyro Ї=Αε[i];Ї.GyroOverride=Љ;if(!Љ) Ї.GyroPower=100;else Ї.GyroPower=ΘЃ;if(!Љ) continue;Vector3 Е=Υ.WorldMatrix.Forward;Vector3 Я=Υ.WorldMatrix.Right;Vector3 ω=Υ.WorldMatrix.Up;
Vector3 ō=Ї.WorldMatrix.Forward;Vector3 ŏ=Ї.WorldMatrix.Up;Vector3 Ŏ=Ї.WorldMatrix.Left*-1;if(ō==Е) Ї.SetValueFloat("Roll",Ǎ);else if(ō==(Е*-1)) Ї.SetValueFloat("Roll",Ǎ*-1);else if(ŏ==(Е*-1)) Ї.SetValueFloat("Yaw",Ǎ);else if(ŏ==Е) Ї.SetValueFloat("Yaw",Ǎ*-1);else if(Ŏ==Е) Ї.SetValueFloat("Pitch",Ǎ);
else if(Ŏ==(Е*-1)) Ї.SetValueFloat("Pitch",Ǎ*-1);if(Ŏ==(Я*-1)) Ї.SetValueFloat("Pitch",ь);else if(Ŏ==Я) Ї.SetValueFloat("Pitch",ь*-1);else if(ŏ==Я) Ї.SetValueFloat("Yaw",ь);else if(ŏ==(Я*-1)) Ї.SetValueFloat("Yaw",ь*-1);else if(ō==(Я*-1)) Ї.SetValueFloat("Roll",ь);
else if(ō==Я) Ї.SetValueFloat("Roll",ь*-1);if(ŏ==(ω*-1)) Ї.SetValueFloat("Yaw",ћ);else if(ŏ==ω) Ї.SetValueFloat("Yaw",ћ*-1);else if(Ŏ==ω) Ї.SetValueFloat("Pitch",ћ);else if(Ŏ==(ω*-1)) Ї.SetValueFloat("Pitch",ћ*-1);else if(ō==ω) Ї.SetValueFloat("Roll",ћ);else if(ō==(ω*-1)) Ї.SetValueFloat("Roll",ћ*-1);
}}void ΕΜ(){this.Γη=false;}void Ч(Vector3 ȳ,Vector3 Е,Vector3 Γυ,float ȥ,bool Ƭ){ΒΘ(ȳ,ȥ,Ƭ);ΑΙ=ȥ;Γψ=false;this.ΔΟ=Е;this.Ζϋ=Γυ;}void Ч(Vector3 ȳ,Vector3 Е,Vector3 Γυ,bool Ƭ){Ч(ȳ,Е,Γυ,2f,Ƭ);}void ΒΘ(Vector3 ȳ,float ȥ,bool Ƭ){ΑΙ=ȥ;this.Γη=true;this.ΕΧ=Ƭ;Γψ=true;Ακ=false;
this.ΔΞ=ȳ;}void ΒЈ(){Ǧ(false,false,false,ς,0);ΑЏ=false;}void Д(Vector3 Ѝ,float ǝ){Ǧ(true,false,false,Ѝ,ǝ);}void Ǧ(bool Љ,bool ΔΙ,bool Δά,Vector3 Ѝ,float ǝ){Ǧ(Љ,ΔΙ,Δά,Ѝ,Ѝ-ά,0.0f,ǝ);}void Ǧ(bool Љ,bool ΔΙ,bool Δά,Vector3 Ѝ,Vector3 ΘЏ,float ΘА,float ǝ){ΑЏ=true;this.Ζύ=Љ;
ς=Ѝ;this.Γώ=ǝ;this.ΓЊ=ΘА;this.Ζλ=ΔΙ;this.Ζκ=Δά;this.Εώ=ΘЏ;Н=Vector3.Distance(Ѝ,ά);}void Ηρ(){ϊ.GetBlocksOfType(Ž,ǐ);}void Ης(){if(ΑЃ){ώ=2;return;}List<IMyRemoteControl>Ɵ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζη=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζς=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(Ɵ,ǐ);ϊ.GetBlocksOfType(ї,ǐ);ϊ.GetBlocksOfType(Ζη,ǐ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζς,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=ǣ(ї,pamTag,true);Ηω();Ηϊ(Ζς);Ÿ(ї,setLCDFontAndSize,1.4f,false);Ÿ(Ď,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζφ=ǣ(Ζη,pamTag,true);if(Ζφ.Count>0) ξ=Ζφ[0];else ξ=null;if(Ι==Ζ.Ŷ){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ώ=1;Λ="Drills are missing";}}else if(Ι==Ζ.Ǐ){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ώ=1;Λ="Grinders are missing";}if(ξ==null){ώ=1;Λ="Sensor is missing";}}else if(Ι==Ζ.Ϋ){ϊ.GetBlocksOfType(ΔΥ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζυ=ǣ(Ɵ,pamTag,true);if(Ζυ.Count>0) Ɵ=Ζυ;if(Ɵ.Count>0) Υ=Ɵ[0];else{Υ=null;ώ=2;Λ="Remote is missing";return;}ί=(float) Υ.CubeGrid.WorldVolume.Radius*2;њ=null;if(Ι!=Ζ.Ϋ){Δν();if(ο.Count>0&&њ!=null){if(ξ!=null&&(њ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Υ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ώ=1;Λ="Wrong sensor direction";}if(њ.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
њ.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ώ=2;Λ="Wrong remote direction";}}}}void Ησ(){ϊ.GetBlocksOfType(Ȇ,ǐ);ϊ.GetBlocksOfType(ц,ǐ);ϊ.GetBlocksOfType(ȋ,ǐ);ϊ.GetBlocksOfType(Αε,ǐ);ϊ.GetBlocksOfType(Ȱ,ǐ);ϊ.GetBlocksOfType(Č,ǐ);ϊ.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(ΑЊ,ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ц=ǣ(ц,"ConnectorMedium",false);else
ц=ǣ(ц,"Connector",false);List<IMyShipConnector>Ζσ=ǣ(ц,pamTag,true);if(Ζσ.Count>0) ц=Ζσ;if(ώ<=1){if(ц.Count==0){ώ=1;Λ="Connector is missing";}if(Αε.Count==0){ώ=1;Λ="Gyros are missing";}if(ȋ.Count==0){ώ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Ζχ=ǣ(ΑЊ,pamTag,true);
if(Ζχ.Count>0) ΑЊ=Ζχ;List<IMyLandingGear>Ζτ=ǣ(Ȇ,pamTag,true);if(Ζτ.Count>0) Ȇ=Ζτ;for(int i=0;i<Ȇ.Count;i++) Ȇ[i].AutoLock=false;List<IMyBatteryBlock>Ζρ=ǣ(Ȱ,pamTag,true);if(Ζρ.Count>0) Ȱ=Ζρ;List<IMyGasTank>Ζψ=ǣ(і,pamTag,true);if(Ζψ.Count>0) і=Ζψ;}void Ητ(){ϊ.GetBlocksOfType(ğ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ŋ.Clear();for(int i=ğ.Count-1;i>=0;i--){if(Ηί(ğ[i])){Ŋ.Add(ğ[i]);ğ.RemoveAt(i);}}}bool Ηί(IMyTerminalBlock Φ){if(Φ is IMyCargoContainer) return true;if(Φ is IMyShipDrill) return true;if(Φ is IMyShipGrinder) return true;if(Φ is IMyShipConnector){if(((IMyShipConnector) Φ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βη(Φ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δν(){if(Υ==null) return;њ=null;float Αϋ=0,Αφ=0,Αό=0,Αχ=0,Αύ=0,Αψ=0;List<IMyTerminalBlock>ΔΝ=ǣ(ο,pamTag,true);bool ΘϏ=ΔΝ.Count==0;if(ΔΝ.Count>0) њ=ΔΝ[0];else if(ο.Count>0) њ=ο[0];int ȃ=0;float Ş=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward){if(ΘϏ){ώ=2;Λ="Mining direction is unclear!";return;}continue;}ȃ++;Ş=Math.Max(Ş,ΗЄ(ο[i]));Vector3 Њ=ƽ(Υ,ο[i].GetPosition());if(i==0){Αϋ=Њ.X;Αφ=Њ.X;Αό=Њ.Y;Αχ=Њ.Y;Αύ=Њ.Z;Αψ=Њ.Z;}Αφ=Math.Max(Њ.X,Αφ);Αϋ=Math.Min(Њ.X,Αϋ);
Αχ=Math.Max(Њ.Y,Αχ);Αό=Math.Min(Њ.Y,Αό);Αψ=Math.Max(Њ.Z,Αψ);Αύ=Math.Min(Њ.Z,Αύ);}if(Ş==0) Ş=drillRadius;bool ȳ=њ!=null&&њ.WorldMatrix.Forward==Υ.WorldMatrix.Down;ǵ=(Αφ-Αϋ)*(1-Ķ)+Ş*2;Ǵ=(ȳ?Αψ-Αύ:Αχ-Αό)*(1-Ħ)+Ş*2;IMyCubeGrid g=Υ.CubeGrid;float Δβ=0,Δγ=0,Δδ=0,Δε=0;
for(int c=0;c<8;c++){Vector3 p=ƽ(Υ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȳ?p.Z:p.Y;if(c==0||p.X<Δβ) Δβ=p.X;if(c==0||p.X>Δγ) Δγ=p.X;if(c==0||y<Δδ) Δδ=y;if(c==0||y>Δε) Δε=y;}ΔΔ=Δγ-Δβ+g.GridSize;ΔΓ=Δε-Δδ+g.GridSize;}float ΗЄ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǣ<T>(List<T>Ф,String Ǟ,bool Αζ){List<T>ή=new List<T>();for(int i=0;i<Ф.Count;i++)
if(Βη(Ф[i],Ǟ,Αζ)) ή.Add(Ф[i]);return ή;}bool Βη<T>(T Φ,String Ǟ,bool Αζ){IMyTerminalBlock b=(IMyTerminalBlock) Φ;if(Αζ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αζ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ђ<T>(List<T>Ф,bool Љ)
{for(int i=0;i<Ф.Count;i++)
ΕΗ((IMyTerminalBlock) Ф[i],Љ);}void ΕΗ(IMyTerminalBlock Φ,bool Љ){if(Φ==null) return;String ΘΔ=Љ?"OnOff_On":"OnOff_Off";var a=Φ.GetActionWithName(ΘΔ);a.Apply(Φ);}void ǧ(bool Љ){ΔΚ=Љ;if(!Ĳ) return;ђ(ΑЊ,Љ);}void ΒϏ(ChargeMode Ƙ){for(int i=0;i<Ȱ.Count;i++) Ȱ[i].ChargeMode=Ƙ;}void ΒЁ(bool ΙΖ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΙΖ;}void Ź(List<IMyLandingGear>Γγ,bool ΖΝ){for(int i=0;i<Γγ.Count;i++){if(ΖΝ) Γγ[i].Lock();if(!ΖΝ) Γγ[i].Unlock();}}void ΕΘ(bool Љ){if(Εξ==Љ) return;List<IMyShipController>ΑΠ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΠ,ǐ);if(ΑΠ.Count==0) return;
for(int i=0;i<ΑΠ.Count;i++)
ΑΠ[i].DampenersOverride=Љ;Εξ=Љ;}IMyShipConnector Т(MyShipConnectorStatus э){for(int i=0;i<ц.Count;i++){if(!ǻ(ц[i],true)) continue;if(ц[i].Status==э) return ц[i];}return null;}void Ĝ(bool ΔΖ,bool ΑΪ,float Θΰ,float ΘΙ){if(ξ==null||ο.Count==0) return;Vector3 ΑΟ=new Vector3();
int ȃ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=њ.WorldMatrix.Forward) continue;ȃ++;ΑΟ+=ο[i].GetPosition();}ΑΟ=ΑΟ/ȃ;Vector3 ś=ƽ(ξ,ΑΟ);ξ.Enabled=true;ξ.ShowOnHUD=ΔΖ;ξ.LeftExtend=(ΑΪ?1:м)/2f*ǵ-ś.X;ξ.RightExtend=(ΑΪ?1:м)/2f*ǵ+ś.X;ξ.TopExtend=(ΑΪ?1:л)/2f*Ǵ+ś.Y;
ξ.BottomExtend=(ΑΪ?1:л)/2f*Ǵ-ś.Y;ξ.FrontExtend=(ΔΖ?Щ:Θΰ)-ś.Z;ξ.BackExtend=ΔΖ?0:ΘΙ+ί*0.75f+ś.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ÿ<T>(List<T>Ф,bool ΙΑ,float Θή,bool Θλ){for(int i=0;i<Ф.Count;i++){IMyTextSurface ƥ=null;if(Ф[i]is IMyTextSurface) ƥ=(IMyTextSurface) Ф[i];if(ƥ!=null){ƥ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΙΑ) continue;ƥ.Font="Debug";if(Θλ) continue;ƥ.FontSize=Θή;}}}bool ǁ(){Vector3D λ;return this.Υ.TryGetPlanetPosition(out λ);}int ΗΧ(Vector3 ΙΘ,int Ζμ){if(Ζμ==-1) return 0;double А=-1;int ȗ=-1;for(int i=Σ.Count-1;i>=0;i--){double Ђ=Vector3.Distance(Σ[i].Δ,ΙΘ);if(А==-1||Ђ<А){ȗ=i;А=Ђ;}
}return Math.Sign(ȗ-Ζμ);}bool Ηΰ(Vector3 λ){List<Vector3>Й=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Й.Add(Σ[i].Δ);if(Η.ε&&Σ.Count>=1){Vector3 Ņ=new Vector3();Ǻ(Η,dockDist*ί,false,out Ņ);if(Vector3.Distance(Η.Δ,Σ.First().Δ)<Vector3.Distance(Η.Δ,Σ.Last().Δ)){Й.Insert(0,Ņ);Й.Insert(0,Η.Δ);}
else{Й.Add(Ņ);Й.Add(Η.Δ);}}if(Ι==Ζ.Ϋ){if(Ε.ε&&Σ.Count>=1){Vector3 ΒЏ=new Vector3();Ǻ(Ε,dockDist*ί,false,out ΒЏ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Й.Insert(0,ΒЏ);Й.Insert(0,Ε.Δ);}else{Й.Add(ΒЏ);Й.Add(Ε.Δ);}}}else{if(ΰ!=Μ.в)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Й.Insert(0,Ε.Δ);else Й.Add(Ε.Δ);}int ȗ=-1;double ǫ=-1;for(int i=0;i<Й.Count;i++){double Ђ=Vector3.Distance(Й[i],λ);if(Ђ<ǫ||ǫ==-1){ǫ=Ђ;ȗ=i;}}if(Й.Count==0) return false;double ΓΩ=Vector3.Distance(Й[ȗ],λ);double ΘЄ=Vector3.Distance(Й[Math.Max(0,ȗ-1)],Й[ȗ])*1.5f;
double Θώ=Vector3.Distance(Й[Math.Min(Й.Count-1,ȗ+1)],Й[ȗ])*1.5f;return ΓΩ<ΘЄ||ΓΩ<Θώ||ΓΩ<laneOffset*ί*1.5f;}static float Г(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηο(Vector3 v,int ΓΦ){return new Vector3(Math.Round(v.X,ΓΦ),Math.Round(v.Y,ΓΦ),Math.Round(v.Z,ΓΦ));
}float ĺ(Vector3 a,Vector3 b){if(a==b) return 0;float Θά=(a*b).Sum;float ΖΘ=a.Length();float ΖΙ=b.Length();if(ΖΘ==0||ΖΙ==0) return 0;float ή=(float)((180.0f/Math.PI)*Math.Acos(Θά/(ΖΘ*ΖΙ)));if(float.IsNaN(ή)) return 0;return ή;}float ē(Vector3 v,Vector3 ΑЁ,float ΙΒ){float Ń=ĺ(v,ΑЁ);
if(ΙΒ>0f) Ń*=-1;if(Ń>-90f) return Ń-90f;else return 180f-(-Ń-90f);}double Δτ(float ΘΤ){return(Math.PI/180)*ΘΤ;}bool Βκ(double Θφ,double value,double Αυ){return(value>=Θφ&&value<=Αυ);}Vector3 ΗЅ(IMyTerminalBlock Φ,Vector3 Њ){return Vector3D.Transform(Њ,Φ.WorldMatrix);
}Vector3 ƽ(IMyTerminalBlock Φ,Vector3 Ιΰ){return б(Φ,Ιΰ-Φ.GetPosition());}Vector3 б(IMyTerminalBlock Φ,Vector3 Δα){return Vector3D.TransformNormal(Δα,MatrixD.Transpose(Φ.WorldMatrix));}Vector3 ě(Vector3 Ǯ,Vector3 ω,Vector3 Δα){MatrixD Γύ=MatrixD.CreateFromDir(Ǯ,ω);return Vector3D.TransformNormal(Δα,MatrixD.Transpose(Γύ));
}Vector3 ΒЉ(Vector3 Ǯ,Vector3 ω,Vector3 Њ){MatrixD Γύ=MatrixD.CreateFromDir(Ǯ,ω);return Vector3D.Transform(Њ,Γύ);}String κ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]И=s.Split('|');return new Vector3(float.Parse(Κ(И,0)),float.Parse(Κ(И,1)),float.Parse(Κ(И,2)));
}String ΘΒ(Ψ Ω){String Ў=":";String ή=κ(Ω.Δ)+Ў+κ(Ω.δ)+Ў+
κ(Ω.γ)+Ў+κ(Ω.υ)+Ў+κ(Ω.ν);for(int i=0;i<Ω.Ǡ.Length;i++)
ή+=Ў+Math.Round(Ω.Ǡ[i],3);return ή;}Ψ ΗϏ(String s){String[]И=s.Split(':');Ψ Ω=new Ψ(
μ(Κ(И,0)),μ(Κ(И,1)),
μ(Κ(И,2)),μ(Κ(И,3)),
μ(Κ(И,4)));int Β=5;List<float>Εϊ=new List<float>();while(Β<И.Length){String ΒΔ=Κ(И,Β);float f=0;if(!float.TryParse(ΒΔ,out f)) break;Εϊ.Add(f);Β++;}Ω.Ǡ=Εϊ.ToArray();return Ω;}System.Text.StringBuilder š=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θύ){if(Θύ) š.Append('\n');š.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƹ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΕ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]ΘΘ,int Β){String ΒΔ=ΘΘ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΒΔ)) return "";
return ΒΔ;}void Save(){if(ΑЃ||Ι==Ζ.ф){Storage="";return;}Storage=ΒΞ();}String ΒΞ(){š.Clear();š.Append(DATAREV+";");Ρ(κ(ΑЂ),false);Ρ(κ(Η.δ));Ρ(κ(Η.υ));Ρ(κ(Η.γ));Ρ(κ(Η.ν));Ρ(κ(Η.Δ));Ρ(κ(Η.Ъ));Ρ(Η.ε);Ρ(κ(Ε.Δ));Ρ(κ(Ε.ν));Ρ(κ(Ε.δ));Ρ(κ(Ε.γ));Ρ(κ(Ε.υ));Ρ(κ(Ε.Ъ));Ρ(Ε.ε);Ρ(κ(Ќ));Ρ(κ(ǰ));Ρ(";");
Ρ((int) Ι,false);Ρ((int) ΰ);Ρ((int) щ);Ρ(ǯ);Ρ(ъ);Ρ(Ю);Ρ(ы);Ρ(Ǩ);Ρ(ȩ);Ρ(ȧ);if(Ι==Ζ.Ϋ){Ρ((int) ό.Џ);Ρ(ό.Ǖ);Ρ(ό.ā);Ρ(ƹ(ό.ǖ));Ρ(ƹ(ό.Ǚ));Ρ((int) ύ.Џ);Ρ(ύ.Ǖ);Ρ(ύ.ā);Ρ(ƹ(ύ.ǖ));Ρ(ƹ(ύ.Ǚ));}else{Ρ((int) Ȥ);Ρ((int) о);Ρ((int) П);Ρ((int) ч);Ρ((int) Ī);Ρ(ď);Ρ(Ĳ);Ρ(Œ);Ρ(ŋ);Ρ(м);Ρ(л);Ρ(Щ);Ρ(ū);Ρ(Ū);Ρ(Ķ);Ρ(Ħ);
Ρ(Р);Ρ(Э);Ρ(Ы);Ρ(Ь);Ρ(ȟ);Ρ(М);Ρ(į);Ρ(Ĭ);}Ρ(";");for(int i=0;i<Ȋ.Count;i++)
Ρ((i>0?"|":"")+Ȋ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΘΒ(Σ[i]),i>0);Ρ(";",false);Ρ(О+"|"+Ѓ+"|"+ȝ+"|"+Ȝ+"|"+ĩ,false);Ρ(";",false);Ρ(ΗΝ(),false);String Χ=š.ToString();š.Clear();return Χ;}х Ηξ(){return ǉ(Storage);}х ǉ(String Χ){if(Χ=="") return х.Ηη;String[]İ=Χ.Split(';');if(Κ(İ,0)!=DATAREV) return х.Βπ;int Β=0;try
{String[]Θ=Κ(İ,1).Split('\n');ΑЂ=μ(Κ(Θ,Β++));Η.δ=μ(Κ(Θ,Β++));Η.υ=μ(Κ(Θ,Β++));Η.γ=μ(Κ(Θ,Β++));Η.ν=μ(Κ(Θ,Β++));Η.Δ=μ(Κ(Θ,Β++));Η.Ъ=μ(Κ(Θ,Β++));Η.ε=bool.Parse(Κ(Θ,Β++));Ε.Δ=μ(Κ(Θ,Β++));Ε.ν=μ(Κ(Θ,Β++));Ε.δ=μ(Κ(Θ,Β++));Ε.γ=μ(Κ(Θ,Β++));Ε.υ=μ(Κ(Θ,Β++));Ε.Ъ=μ(Κ(Θ,Β++));
Ε.ε=bool.Parse(Κ(Θ,Β++));Ќ=μ(Κ(Θ,Β++));ǰ=μ(Κ(Θ,Β++));Θ=Κ(İ,2).Split('\n');Β=0;Ι=(Ζ) int.Parse(Κ(Θ,Β++));ΰ=(Μ) int.Parse(Κ(Θ,Β++));щ=(Μ) int.Parse(Κ(Θ,Β++));ǯ=int.Parse(Κ(Θ,Β++));ъ=int.Parse(Κ(Θ,Β++));Ю=int.Parse(Κ(Θ,Β++));ы=int.Parse(Κ(Θ,Β++));Ǩ=float.Parse(Κ(Θ,Β++));
ȩ=bool.Parse(Κ(Θ,Β++));ȧ=bool.Parse(Κ(Θ,Β++));if(Ι==Ζ.Ϋ){ό.Џ=(ι) int.Parse(Κ(Θ,Β++));ό.Ǖ=float.Parse(Κ(Θ,Β++));ό.ā=float.Parse(Κ(Θ,Β++));ό.ǖ=ΑΕ(Κ(Θ,Β++));ό.Ǚ=ΑΕ(Κ(Θ,Β++));ύ.Џ=(ι) int.Parse(Κ(Θ,Β++));ύ.Ǖ=float.Parse(Κ(Θ,Β++));ύ.ā=float.Parse(Κ(Θ,Β++));ύ.ǖ=ΑΕ(Κ(Θ,Β++));
ύ.Ǚ=ΑΕ(Κ(Θ,Β++));}else{Ȥ=(Ǒ) int.Parse(Κ(Θ,Β++));о=(и) int.Parse(Κ(Θ,Β++));П=(Ѐ) int.Parse(Κ(Θ,Β++));ч=(я) int.Parse(Κ(Θ,Β++));Ī=(Ǒ) int.Parse(Κ(Θ,Β++));ď=bool.Parse(Κ(Θ,Β++));Ĳ=bool.Parse(Κ(Θ,Β++));Œ=bool.Parse(Κ(Θ,Β++));ŋ=bool.Parse(Κ(Θ,Β++));м=int.Parse(Κ(Θ,Β++));л=int.Parse(Κ(Θ,Β++));
Щ=int.Parse(Κ(Θ,Β++));ū=float.Parse(Κ(Θ,Β++));Ū=float.Parse(Κ(Θ,Β++));Ķ=float.Parse(Κ(Θ,Β++));Ħ=float.Parse(Κ(Θ,Β++));Р=int.Parse(Κ(Θ,Β++));Э=int.Parse(Κ(Θ,Β++));Ы=int.Parse(Κ(Θ,Β++));Ь=int.Parse(Κ(Θ,Β++));ȟ=int.Parse(Κ(Θ,Β++));М=int.Parse(Κ(Θ,Β++));į=int.Parse(Κ(Θ,Β++));
Ĭ=float.Parse(Κ(Θ,Β++));}Θ=Κ(İ,3).Replace("\n","").Split('|');Ȋ=Θ.ToList();Θ=Κ(İ,4).Split('\n');Σ.Clear();if(Θ.Count()>=1&&Θ[0]!="")
for(int i=0;i<Θ.Length;i++)
Σ.Add(ΗϏ(Κ(Θ,i)));Θ=Κ(İ,5).Split('|');О=Κ(Θ,0);if(!int.TryParse(Κ(Θ,1),out Ѓ)) Ѓ=-1;if(!float.TryParse(Κ(Θ,2),out ȝ)) ȝ=0;if(!float.TryParse(Κ(Θ,3),out Ȝ)) Ȝ=0;if(!int.TryParse(Κ(Θ,4),out ĩ)) ĩ=0;if(О.Length!=Р*Э){О="";Ѓ=-1;}ΗΜ(Κ(İ,6));}catch{return х.Δχ;}Ţ=щ;
φ();return х.ΑΒ;}const int Δλ=64000;IMyTerminalBlock Δψ(){List<IMyTerminalBlock>ň=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ň,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));return ň.Count>0?ň[0]:null;}void ΗΠ(){IMyTerminalBlock Φ=Δψ();if(Φ==null){Λ="Export: name a block "+dataTag;return;}
String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΞ();if(Χ.Length>Δλ){Λ="Export too large: "+Χ.Length+" chars, max "+Δλ;return;}Φ.CustomData=Χ;Λ="Exported "+Σ.Count+" waypoints";}void ΗΫ(bool Γρ){IMyTerminalBlock Φ=Δψ();if(Φ==null){Λ="Import: name a block "+dataTag;return;}
Ηά(Φ.CustomData,Γρ);}void Ηά(String Χ,bool Γρ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΨ=Χ.IndexOf('\n');Χ=ΖΨ<0?"":Χ.Substring(ΖΨ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}φ();String Εί=ΒΞ();х ή=ǉ(Χ);if(ή==х.ΑΒ&&Γρ){Ψ Ċ=new Ψ(Ε);
Vector3 Θω=Ќ,Θψ=ǰ;int w=м,h=л,d=Щ;Ǒ ΔΛ=Ȥ;я ΓΧ=ч;float ΘЁ=Ķ,ΘЀ=Ħ,Θξ=ȝ,Θν=Ȝ;int ΘБ=ĩ;ǉ(Εί);Ε=Ċ;Ќ=Θω;ǰ=Θψ;м=w;л=h;Щ=d;Ȥ=ΔΛ;ч=ΓΧ;Ķ=ΘЁ;Ħ=ΘЀ;ΰ=Μ.Ŀ;Ż(true,true);ȝ=Θξ;Ȝ=Θν;ĩ=ΘБ;ΰ=Μ.Ŀ;щ=Μ.Ŀ;Λ="Imported job, run CONT";return;}if(ή==х.ΑΒ)
{ƙ=false;Λ="Imported "+Σ.Count+" waypoints";return;}ǉ(Εί);Λ=ή==х.Βπ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ηω(){Ɔ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γο=false;if(Χ=="INSTRUCTIONS"){Γο=true;ΑΨ=true;}
if(Χ=="DEBUG") Γο=true;if(Γο){Ɔ.Add(ї[i]);ї.RemoveAt(i);}}Ÿ(Ɔ,false,1,false);}void Ηϊ(List<IMyTerminalBlock>ň){Ď.Clear();for(int i=0;i<ň.Count;i++){IMyTerminalBlock Φ=ň[i];try{String Ζί=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Φ.CustomName.IndexOf(Ζί);int Ť=-1;if(Β<0||!int.TryParse(Φ.CustomName.Substring(Β+Ζί.Length,1),out Ť)) continue;
if(Ť==-1) continue;Ť--;IMyTextSurfaceProvider Ζα=(IMyTextSurfaceProvider) Φ;if(Ť<Ζα.SurfaceCount&&Ť>=0)
Ď.Add(Ζα.GetSurface(Ť));}catch{}}}String Αω="";void ΕΓ(){String Γό="";String Γή="";Αω=Δκ(false);Γό+=Αω;Γή+=Αω;Γή+=ΗΥ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γό);for(int i=0;i<Ď.Count;i++) Ď[i].WriteText(Γό);Echo(Γή);for(int i=0;i<Ɔ.Count;i++){IMyTextPanel ΓЉ=Ɔ[i];
String Χ=ΓЉ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЉ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЉ.WriteText(ΗΦ());}}string ΗΦ(){String Γ="";try{float ΖΟ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ň*ΖΟ)+" / "+(int)(Ř*ΖΟ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ň*100f,1)+"% / "+Math.Round(Ř*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ſ,2)+"ms / "+Math.Round(ΓЀ,2)+"ms \n";}catch{}for(int i=0;i<ȡ.Count;i++)
Γ+=""+ȡ.Keys.ElementAt(i)+" = "+Math.Round(ȡ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȡ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΩ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ÿ(ї,setLCDFontAndSize,0.9f,false);Ÿ(Ď,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+Ȟ.ToString());for(int i=0;i<Ď.Count;i++) Ď[i].WriteText(Γ+Ȟ.ToString());}void ǽ(){if(!ΑΨ) return;try{ΓЍ=Runtime.CurrentInstructionCount;}catch{}
}void Ǹ(String ψ){if(!ΑΨ) return;if(ΓЍ==0) return;try{float ΒΒ=(Runtime.CurrentInstructionCount-ΓЍ)/Runtime.MaxInstructionCount*100;if(!ȡ.ContainsKey(ψ))
ȡ.Add(ψ,new float[]{ΒΒ,ΒΒ});else{ȡ[ψ][0]=ΒΒ;ȡ[ψ][1]=Math.Max(ΒΒ,ȡ[ψ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЋ(String ă){if(!ŉ) return;String Ў=""+BROADCAST_SEP;String ρ=VERSION+Ў;ρ+=Ŭ(ΔΕ)+Ў;
ρ+=(int) Ι+Ў;ρ+=Βή(ά.X)+""+Ў;ρ+=Βή(ά.Y)+Ў;ρ+=Βή(ά.Z)+Ў;if(Ι!=Ζ.Ϋ)
ρ+=Βΰ(щ)+(щ==Μ.І?" "+Math.Round(ƒ,1)+"%":"")+Ў;else ρ+=Βΰ(щ)+Ў;ρ+=Ŭ(Λ)+Ў;ρ+=Ǜ+""+Ў;ρ+=Ŭ(Αω)+Ў;ρ+=ƅ+Ў;ρ+=Ɩ+Ў;for(int i=0;i<ǘ.Count;i++){if(ǘ[i].Ŗ!=К.ű) continue;ρ+=Ŭ(ǘ[i].ψ)+"/"+Ŭ(ǘ[i].σ)+"/"+ǘ[i].Ǔ+Ў;}Ηφ(ρ,ă);}String Ŭ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηφ(String ρ,String ă){Ηχ(ρ,BROADCAST_SELF_ID,ă);}void Ηχ(String ρ,String Ѝ,String ă){try{if(!ŉ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δϋ()+BROADCAST_SEP+Ѝ+BROADCAST_SEP+ΓΙ+BROADCAST_SEP+ă+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȟ=e;}
}String Δϋ(){if(Ι!=Ζ.ф) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЋ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ů(ref String Χ,out string Εϋ,Char Ў){int Β=Χ.IndexOf(Ў);Εϋ="";if(Β<0) return false;Εϋ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηθ(ref String ρ,out String ơ,out String ă)
{ơ="";ă="";if(!ŉ) return false;String Ǟ="";if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЋ(Ǟ)) return false;if(!Ů(ref ρ,out ơ,BROADCAST_SEP)) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δϋ()&&(Ǟ!="*"&&Ι!=Ζ.ф))) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΙ)) return false;
if(!Ů(ref ρ,out ă,BROADCAST_SEP)) return false;return true;}void Ηή(){bool ƃ=true;if(Εύ){Εύ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ɛ=(Ι!=Ζ.ф?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɛ)
ΔЇ(Ɛ);String[]Θ=Me.CustomData.Split('\n');ŉ=bool.Parse(Βϊ(Βά(Θ,"Enable_Broadcast",ref ƃ)));bool Δί=false;bool Ες=true;if(ŉ){if(Ι!=Ζ.ф)
ΔΕ=Βϊ(Βά(Θ,"Ship_Name",ref ƃ)).Replace(BROADCAST_SELF_ID,"");ΓΙ=Βϊ(Βά(Θ,"Broadcast_Channel",ref ƃ)).ToLower();Ες=false;if(Ȕ==null){Ȕ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ȕ.SetMessageCallback("");}List<IMyRadioAntenna>ń=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(ń);bool ΒЎ=false;
for(int i=0;i<ń.Count;i++)
if(ń[i].EnableBroadcasting&&ń[i].Enabled){ΒЎ=true;break;}if(ń.Count==0) Λ="No Antenna found";else if(!ΒЎ) Λ="Antenna not ready";Δί=ń.Count==0||!ΒЎ;if(ΕΨ&&!Δί&&Ι!=Ζ.ф) Λ="Antenna ok";}else if(Ι==Ζ.ф)
Λ="Offline - Enable in PB custom data";ΕΨ=Δί;if(Ες){if(Ȕ!=null) this.IGC.DisableBroadcastListener(Ȕ);Ȕ=null;}}catch{ƃ=false;}if(!ƃ){Λ="Reset custom data";ΔЇ(Ɛ);}}void ΔЇ(String Ɛ){String Χ=Ɛ+"\n\n"+"Enable_Broadcast="+(Ι==Ζ.ф?"true":"false")+" \n";Χ+=Ι!=Ζ.ф?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βϊ(String ī){int Β=ī.IndexOf("//");if(Β!=-1) ī=ī.Substring(0,Β);String[]И=ī.Split('=');if(И.Length<=1) return "";return И[1].Trim();}String Βά(String[]Θ,String Ŕ,ref bool ƚ){foreach(String ī in Θ)
if(ī.StartsWith(Ŕ)) return ī;ƚ=false;return "";}String ΕΖ(int Δέ,String Γ,int Ζζ,ref int ȣ){String[]Θ=Γ.Split('\n');if(Ζζ>=ȣ+Δέ-1) ȣ++;ȣ=Math.Min(Θ.Count()-1-Δέ,ȣ);if(Ζζ<ȣ+1) ȣ--;ȣ=Math.Max(0,ȣ);String ή="";for(int i=0;i<Δέ;i++){int ΖΚ=i+ȣ;if(ΖΚ>=Θ.Count()) break;
ή+=Θ[ΖΚ]+"\n";}return ή;}String Βί(int Ơ){if(Ơ>=60*60) return Math.Round(Ơ/(60f*60f),1)+" h";if(Ơ>=60) return Math.Round(Ơ/60f,1)+" min";return ""+Ơ+" s";}string Βή(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>Ǫ=new List<IMyCameraBlock>();List<long>Ŝ=new List<long>();
bool ĭ=false;int ΓΟ=0,ΘЌ=0,ΓΗ=0;const int Ηπ=16;int ΓΝ=0;DateTime Εγ=DateTime.Now;String ΓΘ="";String ΓΜ="";void Ηυ(){Ǫ.Clear();Ŝ.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(Ǫ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<Ǫ.Count;i++) Ǫ[i].EnableRaycast=true;
List<IMyTerminalBlock>ΑΚ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ΑΚ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΑΚ.Count;i++) if(!Ŝ.Contains(ΑΚ[i].CubeGrid.EntityId)) Ŝ.Add(ΑΚ[i].CubeGrid.EntityId);if(!Ŝ.Contains(Me.CubeGrid.EntityId)) Ŝ.Add(Me.CubeGrid.EntityId);
ΓΜ="";if(Υ==null) return;MatrixD m=Υ.WorldMatrix;Vector3[]ΘΨ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Θϋ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ǫ.Count;i++)
if(Vector3.Dot(Ǫ[i].WorldMatrix.Forward,ΘΨ[d])>0.7f){ΓΜ+=Θϋ[d]+" ";break;}}bool ΔЊ(Α s){return s==Α.ǎ||s==Α.Ш||s==Α.Ё||
s==Α.ǹ||s==Α.Ļ||s==Α.ƻ||
(s==Α.Ǣ&&љ!=null);}void Ʒ(){ĭ=false;ΓΟ=0;ΓΘ="";}void ΗЍ(){if(!obstacleBrake||Ǫ.Count==0||!ΔЊ(Τ)){Ʒ();return;}Vector3 Ə=ς+Ȧ;Vector3 ǲ=Ə-ά;float ǟ=ǲ.Length();if(ǟ<1f){Ʒ();return;}Vector3 ζ=ǲ/ǟ;Vector3 Ȩ=Υ!=null?(Vector3) Υ.GetShipVelocities().LinearVelocity:new Vector3();
if(Ȩ.Length()>2) ζ=Vector3.Normalize(Ȩ);Vector3 ΑЈ=Vector3.Cross(ζ,Math.Abs(ζ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΑЈ.Normalize();Vector3 ω=Vector3.Cross(ΑЈ,ζ);float Ђ=Math.Min(ǟ+ί*0.25f,ȉ*obstacleLookAhead+ί);int Εε=0;for(int k=0;k<2;k++){int p=k==0?0:ΘЌ++%8,e=p%4;
Vector3 Αώ=k==0?new Vector3():(e==0?ΑЈ:e==1?-ΑЈ:e==2?ω:-ω)*ί*0.5f;Vector3 Ō=ά+ζ*(p>=4?Ђ*0.5f:Ђ)+Αώ;IMyCameraBlock ΓΛ=ΖА(Ō);if(ΓΛ==null){Δρ();continue;}Εε++;MyDetectedEntityInfo ș=ΓΛ.Raycast(Ō);bool ΙΪ=ș.Type==MyDetectedEntityType.Planet||ș.Type==MyDetectedEntityType.Asteroid;
bool ΖΩ=!ș.IsEmpty()&&!Ŝ.Contains(ș.EntityId)&&!Ηγ(ș.EntityId)&&
ș.Type!=MyDetectedEntityType.FloatingObject&&!(ΙΪ&&(k>0||Τ==Α.ǎ||
(ș.HitPosition.HasValue&&Vector3D.Distance(ș.HitPosition.Value,Ə)<ί)));if(ΖΩ){if(!ĭ) Εγ=DateTime.Now;ĭ=true;ΓΟ=0;ΓΘ=ș.Type.ToString();}else Δρ();if(debugDraw&&ΑΩ.Count<16) ΑΩ.Add(κ(ΓΛ.GetPosition())+","+κ(Ō)+","+(ΖΩ?1:0));}ΓΗ=Εε>0?0:ΓΗ+1;if(ΓΗ>12) Λ="Cameras can't see ahead";
if(ĭ&&(DateTime.Now-Εγ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΓΘ;}void Δρ(){if(ĭ&&++ΓΟ>=Ηπ) Ʒ();}IMyCameraBlock ΖА(Vector3 Ō){IMyCameraBlock Ȁ=null;float Εβ=-2;for(int n=0;n<Ǫ.Count;n++){ΓΝ=(ΓΝ+1)%Ǫ.Count;IMyCameraBlock c=Ǫ[ΓΝ];if(!c.IsFunctional||!c.IsWorking) continue;float Ζδ=Vector3.Dot(Vector3.Normalize(Ō-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Ζδ>Εβ+0.05f&&c.CanScan(Ō)){Ȁ=c;Εβ=Ζδ;}}return Ȁ;}const String ΕΟ="[PAMTFC]";const float ΗЂ=5f;const float ΒΫ=2f;class Ȯ{public long χ;public Vector3 λ,Ȩ;public Vector3 Ƒ;public float Ερ;public String Ċ="";public int Γμ=-1;public bool ΓЎ;public int Αν;public int Ɲ;public float Θα,Θέ;
public long έ=0;public int ƈ=0;public DateTime ΔΣ;}IMyBroadcastListener ΒΑ=null;long е=0;List<Ȯ>п=new List<Ȯ>();Vector3 Ȧ=new Vector3();float р=-1;Vector3 ΔΧ=new Vector3();bool ΑБ=false;DateTime ΔΨ=DateTime.Now;const float ΕΞ=15f;const float ΗЁ=10f;int ΖЁ=0;String О="";int Ѓ=-1;
bool ΑΣ=false,ΑΡ=false,ƍ=false;Vector3 Εθ=new Vector3();float ľ(){return ȝ>0?ȝ:ǵ;}float Ľ(){return Ȝ>0?Ȝ:Ǵ;}String Ę(){if(ΰ==Μ.в||Ι==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Р+"|"+Э+"|"+(int) Ī+"|"+Math.Round(ľ(),1)+"|"+Math.Round(Ľ(),1)+"|"+ĩ;}void ΗА(){if(!trafficEnabled) return;if(ΒΑ==null) ΒΑ=IGC.RegisterBroadcastListener(ΕΟ);е=Me.CubeGrid.EntityId;int ġ=30;while(ΒΑ.HasPendingMessage&&ġ-->0){String Χ=ΒΑ.AcceptMessage().Data as String;
if(Χ!=null) Ηλ(Χ);}for(int i=п.Count-1;i>=0;i--)
if((DateTime.Now-п[i].ΔΣ).TotalSeconds>ΗЂ) п.RemoveAt(i);if(ΑЎ==null) ΑЎ=IGC.RegisterBroadcastListener(ΗЀ);ġ=5;while(ΑЎ.HasPendingMessage&&ġ-->0){String Χ=ΑЎ.AcceptMessage().Data as String;if(Χ!=null) try{Ημ(Χ);}catch{}}if(ΑΤ==null) ΑΤ=IGC.RegisterBroadcastListener(Ηδ);
ġ=10;while(ΑΤ.HasPendingMessage&&ġ-->0){String Χ=ΑΤ.AcceptMessage().Data as String;if(Χ!=null) try{Ηι(Χ);}catch{}}if(++ΖЁ<3) return;ΖЁ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",е+";"+κ(Ε.Δ)+";"+
κ(ΰ==Μ.в?new Vector3():Ε.υ)+";"+κ(ǰ)+";"+
κ(ς+Ȧ)+";"+String.Join(":",ΑΩ));ΑΩ.Clear();}IGC.SendBroadcastMessage(ΕΟ,е+";"+κ(ά)+";"+
κ(Υ.GetShipVelocities().LinearVelocity)+";"+ί+";"+Ę()+";"+
(ΰ==Μ.І?Ѓ:-1)+";"+О+";"+ΗΕ()+","+Δφ()+";"+κ(ΔΧ)+
";"+(Αά?1:0)+";"+(Ą&&ΰ==Μ.І?1:0)+","+ΔΔ+","+ΔΓ+","+Ηε()+","+ΕΠ()+";"+Me.GetId());}void Ηλ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long χ=long.Parse(f[0]);if(χ==е) return;Ȯ Є=null;for(int i=0;i<п.Count;i++) if(п[i].χ==χ) Є=п[i];
if(Є==null){Є=new Ȯ();Є.χ=χ;п.Add(Є);}Є.λ=μ(f[1]);Є.Ȩ=μ(f[2]);Є.Ερ=float.Parse(f[3]);Є.Ċ=f[4];Є.Γμ=int.Parse(f[5]);Є.ΔΣ=DateTime.Now;if(Є.Ċ!=""&&Є.Ċ==Ę()) ΔБ(f[6]);Є.έ=0;Є.ƈ=0;if(f.Length>=8){String[]d=f[7].Split(',');Є.έ=long.Parse(d[0]);Є.ƈ=int.Parse(d[1]);
}Є.Ƒ=f.Length>=9?μ(f[8]):new Vector3();Є.ΓЎ=f.Length>=10&&f[9]=="1";Є.Αν=0;Є.Ɲ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Є.Θα=float.Parse(w[1]);Є.Θέ=float.Parse(w[2]);Є.Αν=int.Parse(w[3]);if(w.Length>=5) Є.Ɲ=int.Parse(w[4]);}}}catch{}
}bool Ηγ(long χ){for(int i=0;i<п.Count;i++) if(п[i].χ==χ) return true;return false;}Vector3 ΔϏ(int Β,int ΘЂ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΧ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*ΘЂ;Vector3 ω=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.γ;
Vector3 Я=Vector3.Cross(ΙΧ,ω);if(Я.LengthSquared()<0.0001f) return new Vector3();float Ђ=laneOffset*ί;float Θί=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Я)*Ђ*Math.Min(1,Θί/(Ђ*4));}Vector3 ΖЀ=new Vector3();List<String>ΑΩ=new List<String>();bool ΔЄ(){Α s=Τ;return Ę()!=""&&(s==Α.ǎ||s==Α.ę||s==Α.ǥ||
s==Α.ё||s==Α.Ė);}float ΔЍ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Р*ľ(),2)+Math.Pow(Э*Ľ(),2));}bool Βο(){return Ę()!=""&&Vector3.Distance(ά,Ε.Δ)<ΔЍ()+ί*2;}int ΕΠ(){Α s=Τ;if(s==Α.ё||s==Α.ǹ||(s==Α.Ё&&Βο())) return 2;return s==Α.ǎ||s==Α.ǥ||s==Α.Ė||s==Α.Ш?1:0;
}int Ηε(){if(!ΔЄ()) return 0;return Τ!=Α.ǎ&&Vector3.Dot(ά-Ј,Ќ)>ί*0.5f?2:1;}bool Ηβ(Α s){return s==Α.Ǣ||s==Α.đ||s==Α.ĕ||s==Α.ŷ||
s==Α.ǒ||s==Α.ǥ||s==Α.ё||s==Α.Ė||s==Α.Ȫ;}void ΗБ(){Ȧ=new Vector3();р=-1;ΔΧ=new Vector3();bool ΕЀ=ΔЊ(Τ);if(!trafficEnabled||(!ΕЀ&&!Ηβ(Τ))){ΑБ=false;return;}ΖЀ=Υ.GetShipVelocities().LinearVelocity;ΗΖ(Υ.GetNaturalGravity(),!ΕЀ);ΖЉ();}IMyBroadcastListener ΑЎ=null;
float Ŧ=-1;DateTime Ζό=DateTime.MinValue;void Ημ(String Χ){if(!Χ.StartsWith("C;")) return;Ζό=DateTime.Now;Ŧ=-1;String Ɨ=е+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) Ŧ=float.Parse(e[i].Substring(Ɨ.Length));}void ΖЉ(){if(Ŧ<0||(DateTime.Now-Ζό).TotalSeconds>1.5) return;
р=р<0?Ŧ:Math.Min(р,Ŧ);if(Ŧ<0.5f) Λ="Traffic: giving way (controller)";}void ΗΖ(Vector3 ν,bool Ζή=false){Ȧ=new Vector3();р=-1;Vector3 ǲ=ς-ά;float ǟ=ǲ.Length();bool Γκ=ǟ>=1f;Vector3 ζ=Γκ?ǲ/ǟ:new Vector3();ΔΧ=ζ;Vector3 ω=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɗ;
Vector3 Я=Vector3.Cross(ζ,ω);Я=Я.LengthSquared()<0.0001f?-Ɣ:Vector3.Normalize(Я);Ȯ Αο=null;bool ΕЂ=Τ==Α.Ё&&!ȑ(1);int ΓЃ=ΕΠ();for(int i=0;i<п.Count;i++){Ȯ α=п[i];if(α.ƈ==3&&α.Ȩ.LengthSquared()<0.25f) continue;Vector3 λ=α.λ+α.Ȩ*(float)(DateTime.Now-α.ΔΣ).TotalSeconds;
Vector3 Ȣ=λ-ά;float Ȗ=(ί+α.Ερ)*0.5f;bool Ζγ=α.Ċ!=""&&α.Ċ==Ę();if(Ζγ&&α.Αν==2) continue;bool ΑЉ=Ζγ&&α.Αν==1&&ΔЄ();if(ΑЉ) Ȗ=Math.Min(ľ(),Ľ())*0.9f;float ΓΤ=Ȗ*(ΑЉ?1f:1.2f);float Ȓ=Γκ?Vector3.Dot(Ȣ,ζ):0;if(Ȓ>0&&(Ȣ-ζ*Ȓ).Length()<=ΓΤ){float ΑϏ=Vector3.Dot(α.Ȩ,ζ);
bool ΕЈ=α.Ƒ.LengthSquared()>0.25f;bool ΕЉ=ΑϏ<-1f||(ΕЈ&&Vector3.Dot(α.Ƒ,ζ)<-0.5f);if(!Ζή&&α.Ɲ<=ΓЃ){if(α.ΓЎ){if(!ΕЂ){Ȧ=Я*Ȗ*1.3f;continue;}float ΖΥ=Vector3.Distance(ά,ς),ΖΔ=Vector3.Distance(λ,ς);if(ΖΥ<ΖΔ||(ΖΥ==ΖΔ&&е<α.χ)) continue;}if(ΕЉ){if(Ȓ<(ȉ-ΑϏ)*6f+Ȗ*4f) Ȧ=Я*Ȗ*1.3f;
continue;}if(ΕЈ&&е<α.χ){Vector3 Εή=-Ȣ;float Ζά=Vector3.Dot(Εή,α.Ƒ);if(Ζά>0&&(Εή-α.Ƒ*Ζά).Length()<=ΓΤ){Ȧ=Я*Ȗ*1.3f;continue;}}}float ΕЁ=Ȓ-Ȗ-(ΑЉ?1f:followGap*ί);float Γφ=Math.Max(0,ΑϏ)+Math.Max(0,ΕЁ)/ΒΫ;if(ΕЉ) Γφ=Math.Max(0,ΕЁ+Math.Min(0,ΑϏ)*ΒΫ*2)/ΒΫ;
if(р<0||Γφ<р){р=Γφ;Αο=α;}continue;}if(α.Ȩ.LengthSquared()<1f) continue;if(Γκ&&(Ȣ-ζ*Ȓ).Length()<=ΓΤ) continue;Vector3 ΔΒ=α.Ȩ-ΖЀ;float Ζβ=ΔΒ.LengthSquared();if(Ζβ<0.25f) continue;float t=-Vector3.Dot(Ȣ,ΔΒ)/Ζβ;if(t<=0||t>5f||(Ȣ+ΔΒ*t).Length()>=(ΑЉ?Ȗ:Ȗ*1.1f)) continue;
if(α.Ɲ>ΓЃ||(α.Ɲ==ΓЃ&&е>α.χ)||t<2f){р=0;Αο=α;}}bool Θη=р>=0&&р<0.5f;if(!Θη||Ζή||(Αο!=null&&Αο.ΓЎ&&ΕЂ)){ΑБ=false;return;}if(!ΑБ){ΑБ=true;ΔΨ=DateTime.Now;}double ΕЊ=(DateTime.Now-ΔΨ).TotalSeconds;if(ΕЊ>ΕΞ+ΗЁ) ΔΨ=DateTime.Now;else if(ΕЊ>ΕΞ){р=-1;Ȧ=Я*ί*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>ȅ=new List<int>();String ΕЌ="";void ΒΪ(){String Ŕ=Р+"|"+Э+"|"+(int) Ī;if(Ŕ==ΕЌ) return;ΕЌ=Ŕ;ΖЌ(Р,Э,Ī,ȅ);}void ΖЌ(int w,int h,Ǒ ΔΛ,List<int>ΓΞ){ΓΞ.Clear();if(ΔΛ==Ǒ.ΑΓ){for(int k=0;k<w*h;k++){int ş=k/w;ΓΞ.Add(ş*w+(ş%2==0?k-ş*w:w-1-(k-ş*w)));
}return;}int ΕЇ=(int) Math.Ceiling(w/2f),ΕІ=(int) Math.Ceiling(h/2f);int Θγ=w/2,Θβ=h/2;int x=0,y=0,Γά=0,Γέ=-1,Αυ=Math.Max(w,h);for(int i=0;i<Αυ*Αυ;i++){if(-ΕЇ<x&&x<=Θγ&&-ΕІ<y&&y<=Θβ) ΓΞ.Add((y-1+ΕІ)*w+(x-1+ΕЇ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Γά;Γά=-Γέ;Γέ=t;}
x+=Γά;y+=Γέ;}}bool ΒΩ(){int n=Math.Max(0,Р*Э);if(О.Length==n) return false;О=new String('0',n);Ѓ=-1;return true;}void ΔБ(String α){ΒΩ();if(α.Length!=О.Length) return;char[]h=О.ToCharArray();for(int i=0;i<h.Length;i++) if(α[i]=='1') h[i]='1';О=new String(h);}bool ǀ(int ȕ){return ȕ>=0&&ȕ<О.Length&&О[ȕ]=='1';}
void ΒЀ(int ȕ,bool ѕ){if(ȕ<0||ȕ>=О.Length) return;char[]h=О.ToCharArray();h[ȕ]=ѕ?'1':'0';О=new String(h);}bool Βθ(int ȕ,bool Θο){if(!trafficEnabled) return false;String Ŕ=Ę();for(int i=0;i<п.Count;i++){Ȯ t=п[i];if(t.Ċ!=Ŕ||t.Γμ<0||(Θο&&t.χ>е)) continue;if(t.Γμ==ȕ) return true;
}return false;}int ΗΔ(int Β){int n=Р*Э;if(n<1) return Β;ΒΪ();if(ΒΩ()) for(int k=0;k<Β&&k<n;k++) ΒЀ(ȅ[k],true);if(Ѓ>=0&&(Β>=n||ȅ[Β]!=Ѓ)){ΒЀ(Ѓ,true);Ѓ=-1;}if(ΔЌ()){if(Ǭ>=0&&!ǀ(Ǭ)){Ą=false;Ѓ=Ǭ;return ȅ.IndexOf(Ǭ);}if(Ǭ==-2){Ѓ=-1;Ą=О.IndexOf('0')>=0;return Ą?-2:n;}
if(Ǭ==-1&&Ѓ>=0&&!ǀ(Ѓ)){Ą=false;return Β;}Ѓ=-1;if(!Ą){Ą=true;Εκ=DateTime.Now;}if((DateTime.Now-Εκ).TotalSeconds<5) return-1;}Ą=false;if(Ѓ>=0&&!ǀ(Ѓ)&&!Βθ(Ѓ,true)) return Β;Ѓ=-1;for(int k=0;k<n;k++){int c=ȅ[k];if(ǀ(c)||Βθ(c,false)) continue;Ѓ=c;
return k;}return О.IndexOf('0')>=0?-2:n;}const String Ηδ="[PAMJOB]";const String ΗЀ="[PAMTCC]";IMyBroadcastListener ΑΤ=null;DateTime Ει=DateTime.MinValue;DateTime Εκ=DateTime.Now;int Ǭ=-1;bool Ą=false;bool ΔЌ(){return trafficEnabled&&Ǭ!=-3&&(DateTime.Now-Ει).TotalSeconds<3;}
void Ηι(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=Ę()) return;Ει=DateTime.Now;ΔБ(f[2]);Ǭ=-3;String Ɨ=е+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) Ǭ=int.Parse(e[i].Substring(Ɨ.Length));}Ł Ηψ(int Β)
{ΒΪ();if(Β<0||Β>=ȅ.Count) return Ł.Δθ;Ы=ȅ[Β]%Р;Ь=ȅ[Β]/Р;return Ł.Δω;}class ѐ{public long χ;public Vector3 λ,Е,ω;}List<ѐ>В=new List<ѐ>();bool Ȳ=false;Vector3 Αή,Αέ,Αί,Αΰ;long ш=0;long ȇ=0;Ψ љ=null;bool Ɖ=false;Ψ ΒΦ(Vector3 λ,Vector3 Е,Vector3 ω){MatrixD m=MatrixD.CreateWorld(λ,Е,ω);
Ψ w=new Ψ(Η);w.δ=Vector3D.TransformNormal(Αή,m);w.γ=Vector3D.TransformNormal(Αέ,m);w.υ=Vector3D.TransformNormal(Αί,m);w.Δ=λ+(Vector3) Vector3D.TransformNormal(Αΰ,m);w.ε=true;return w;}int Δφ(){Α s=Τ;if(s==Α.к) return Т(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.ĕ||s==Α.Ǿ||s==Α.Ĕ||s==Α.ΑΗ||
s==Α.ƺ||s==Α.ǒ) return 3;if(s==Α.Ǣ||s==Α.đ||s==Α.ŷ) return 2;if(s==Α.Ё) return 1;return 0;}long ΗΕ(){if(Δφ()==0) return 0;if(ȑ(1)&&ţ!=0) return ţ;return љ!=null?ȇ:ш;}void ΔЎ(){if(Ι==Ζ.Ϋ||!Η.ε) return;IMyShipConnector Л=Т(MyShipConnectorStatus.Connected);if(Л==null||Л.OtherConnector==null) return;
IMyShipConnector α=Л.OtherConnector;Vector3I Θθ=new Vector3I((int) Η.Ъ.X,(int) Η.Ъ.Y,(int) Η.Ъ.Z);if(Л.Position==Θθ){MatrixD ő=MatrixD.Transpose(α.WorldMatrix);Αή=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,ő);Αέ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,ő);Αί=Vector3D.TransformNormal(Υ.WorldMatrix.Left,ő);
Αΰ=Vector3D.TransformNormal(Л.GetPosition()-α.GetPosition(),ő);Ȳ=true;}if(Vector3.Distance(Л.GetPosition(),Η.Δ)<5) ш=α.EntityId;List<IMyShipConnector>ĥ=new List<IMyShipConnector>();ϊ.GetBlocksOfType(ĥ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ĥ.Count>0){В.Clear();for(int i=0;i<ĥ.Count;i++){ѐ p=new ѐ();p.χ=ĥ[i].EntityId;p.λ=ĥ[i].GetPosition();p.Е=ĥ[i].WorldMatrix.Forward;p.ω=ĥ[i].WorldMatrix.Up;В.Add(p);}}љ=null;ȇ=0;if(α.EntityId==ш||!Ȳ) return;for(int i=0;i<В.Count;i++)
if(В[i].χ==α.EntityId){ȇ=В[i].χ;љ=ΒΦ(В[i].λ,В[i].Е,В[i].ω);}}bool Βλ(long χ,int ΖΦ){if(χ==0) return false;for(int i=0;i<п.Count;i++){Ȯ t=п[i];if(t.έ==χ&&(t.ƈ>ΖΦ||(t.ƈ==ΖΦ&&t.χ<е))) return true;}return false;}bool ΒΠ(int ΓЋ){if(Ι==Ζ.Ϋ||!trafficEnabled){љ=null;ȇ=0;return true;}
long θ=љ!=null?ȇ:ш;if(!Βλ(θ,ΓЋ)) return true;if(!Βλ(ш,ΓЋ)){љ=null;ȇ=0;return true;}if(!Ȳ) return false;ѐ Ȁ=null;float Εα=0;for(int i=0;i<В.Count;i++){ѐ p=В[i];if(p.χ==ш||Βλ(p.χ,ΓЋ)) continue;float d=Vector3.Distance(p.λ,Η.Δ);if(Ȁ==null||d<Εα){Ȁ=p;Εα=d;}
}if(Ȁ==null) return false;ȇ=Ȁ.χ;љ=ΒΦ(Ȁ.λ,Ȁ.Е,Ȁ.ω);return true;}String ΗΝ(){String s=(Ȳ?"1":"0")+":"+κ(Αή)+":"+κ(Αέ)+":"+
κ(Αί)+":"+κ(Αΰ)+":"+ш;for(int i=0;i<В.Count;i++)
s+=":"+В[i].χ+","+κ(В[i].λ)+","+κ(В[i].Е)+","+κ(В[i].ω);return s;}void ΗΜ(String s){В.Clear();Ȳ=false;ш=0;String[]f=s.Split(':');if(f.Length<6) return;Ȳ=f[0]=="1";Αή=μ(f[1]);Αέ=μ(f[2]);Αί=μ(f[3]);Αΰ=μ(f[4]);long.TryParse(f[5],out ш);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();if(!long.TryParse(e[0],out p.χ)) continue;p.λ=μ(e[1]);p.Е=μ(e[2]);p.ω=μ(e[3]);В.Add(p);}}const String ΒΣ="[PAMDCK]";IMyBroadcastListener ΑЌ=null;int Ȅ=0;DateTime Ευ=DateTime.Now;DateTime Ζν=DateTime.MinValue;long Ζξ=0;
int Ζο=0;long ţ=0;bool Ʈ=false;int ΓΫ=0;List<Vector3>ư=new List<Vector3>();List<float>Δζ=new List<float>();bool Αά=false;bool ΑΫ=false;Vector3 Ετ=new Vector3();bool ź(){return Ȅ!=0&&(DateTime.Now-Ζν).TotalSeconds<3;}bool ȑ(int Αξ){return ź()&&Ζξ==е&&Ζο==Αξ;}bool ΕΚ(){return ź()||(DateTime.Now-Ευ).TotalSeconds<3;}
void Βώ(int Αξ){if(Ȅ==Αξ) return;Ȅ=Αξ;Ευ=DateTime.Now;ΓΫ=3;}void Βύ(){if(Ȅ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΣ,"D;"+е);Ȅ=0;Ʈ=false;}void ΗЈ(){if(!trafficEnabled||Ι==Ζ.Ϋ) return;if(ΑЌ==null) ΑЌ=IGC.RegisterBroadcastListener(ΒΣ);int ġ=20;while(ΑЌ.HasPendingMessage&&ġ-->0)
{String Χ=ΑЌ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;try{Ηκ(Χ);}catch{}}if(Τ!=Α.Ё){Αά=false;ΑΫ=false;}if(Ȅ==1&&Т(MyShipConnectorStatus.Connected)!=null) Βύ();if(Ȅ==2&&!Ʈ&&Τ!=Α.ǒ&&
Vector3.Distance(ά,й().Δ)>ί*3) Βύ();if(Ȅ!=0&&++ΓΫ>=3){ΓΫ=0;IGC.SendBroadcastMessage(ΒΣ,"R;"+е+";"+Ȅ+";"+ш+";"+(Ȳ?1:0));}}void Ηκ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool ΕϏ=false;String[]ΕΤ=f[5].Split(',');for(int i=0;i<ΕΤ.Length;i++) if(ΕΤ[i]==е.ToString()) ΕϏ=true;
if(!ΕϏ) return;Ζν=DateTime.Now;Ζξ=long.Parse(f[2]);Ζο=int.Parse(f[3]);ţ=long.Parse(f[4]);ư.Clear();Δζ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ư.Add(μ(e[0]));Δζ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εχ=f[6].Split(':');List<ѐ>Ф=new List<ѐ>();for(int i=0;i<Εχ.Length;i++){String[]e=Εχ[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();p.χ=long.Parse(e[0]);p.λ=μ(e[1]);p.Е=μ(e[2]);p.ω=μ(e[3]);Ф.Add(p);}В=Ф;}bool ΗΒ(){if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return true;
Βώ(1);if(!ΕΚ()) return true;if(!ȑ(1)){Λ="Waiting for dock clearance";return false;}ΒΙ();return true;}bool ΒΙ(){Ɖ=false;ѐ Į=null;if(ţ!=0&&ţ!=ш&&Ȳ)
for(int i=0;i<В.Count;i++) if(В[i].χ==ţ) Į=В[i];long Δή=Į==null?0:Į.χ;if(Δή==ȇ&&(Δή==0)==(љ==null)) return false;ȇ=Δή;љ=Į==null?null:ΒΦ(Į.λ,Į.Е,Į.ω);return true;}bool ļ(Vector3 p){for(int i=0;i<ư.Count;i++) if(Vector3.Distance(p,ư[i])<Δζ[i]+ί*0.75f) return true;
return false;}bool ΗΪ(Vector3 Ƅ,Vector3 Ś){Αά=false;if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return false;if(Ȅ!=1&&Vector3.Distance(ά,Η.Δ)>dockRequestDistance) return false;Βώ(1);if(!ź()||ȑ(1)||ư.Count==0) return false;if(!ļ(Ƅ)&&!ļ(Ś)&&!ļ(ά)) return false;Αά=true;return true;
}bool Δο(){if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return true;Βώ(2);if(!ΕΚ()) return true;if(!ȑ(2)){Λ="Waiting for undock clearance";return false;}return true;}void ΗΚ(string ѓ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}