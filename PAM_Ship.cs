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
public enum Ζ{ΕΨ,ź,Ǒ,ф,Ϋ}public enum Α{й,ǐ,Ě,ǥ,ђ,Ђ,Ш,
ǹ,Ļ,Ǐ,Ė,Ē,ȫ,
Ż,Ǿ,ǔ,ĕ,ΑΚ,Ƽ,
ΒА,ΑΙ,ė,ƽ,ȯ}public enum Μ{в,Ŀ,І,Βέ,Ƹ,ǆ,Ǉ,ķ,ĸ}public enum у{None,ĕ,й,Δϊ}public enum Ϊ{Main,ΕΛ,Ź,Ÿ,ΒЌ,ƶ,
ΒΝ,ΔЋ,ΔЌ,ΔЉ,ΗЃ,Ιρ,
Ιο,Ιπ,ΒЇ,ΕΟ,ΑΔ}public enum г{ΑΒ,ű,ΗЅ,ΔЎ,Ϋ}public enum Ǔ{ΑΖ,ΗΘ}public enum з{ΑΑ,Βγ,ΕΡ,Ŵ}public enum Ё{Βφ,Δχ,ΒΨ,Βή,ƺ,Βο,ǁ}public enum ѐ{ΒΪ,ΒΣ,ΒΤ}public enum Л{ŵ,Ηε,ē}public enum ι{Βψ,Βό,Βύ,ΕΘ,ΕΗ,
Βχ,ǋ,Ǌ,Βϊ,
Βϋ,Βω}public enum β{Ƿ,ȏ,Ƕ,Ƶ,ΒΠ,ΒΡ}public enum Ł{Δώ,Δν,Ιξ}public enum х{ΑΕ,Δό,Ηο,Βυ}public enum τ{ƀ,ƴ,Ǆ,Ŵ,Βσ,ǅ,Βτ,ΗЋ,Βώ}class Ψ{public bool ε=false;public Vector3 Δ=new Vector3();public Vector3 γ=new Vector3();public Vector3 δ=new Vector3();public Vector3 υ=new Vector3();
public Vector3 ξ=new Vector3();public Vector3 Ъ=new Vector3();public float Ћ=0;public float Ιτ=0;public float[]ǡ=null;public Ψ(){}public Ψ(Ψ α){ε=α.ε;Δ=α.Δ;γ=α.γ;δ=α.δ;υ=α.υ;ξ=α.ξ;Ъ=α.Ъ;ǡ=α.ǡ;}public Ψ(Vector3 Δ,Vector3 δ,Vector3 γ,Vector3 υ,Vector3 ξ){this.Δ=Δ;this.γ=γ;
this.δ=δ;this.υ=υ;this.Ћ=0;this.ξ=ξ;}public void ΗΔ(List<IMyThrust>ΑΞ,List<string>Ȋ){ǡ=new float[Ȋ.Count];for(int i=0;i<ǡ.Length;i++)
ǡ[i]=-1;for(int i=0;i<ΑΞ.Count;i++){string я=ΔІ(ΑΞ[i]);int Β=Ȋ.IndexOf(я);if(Β!=-1)
ǡ[Β]=Д(ΑΞ[i].MaxEffectiveThrust,ΑΞ[i].MaxThrust);}}}class ΒЀ{public Ψ Ѝ=null;public List<Vector3>Ũ=new List<Vector3>();public float Ƌ=0;public float Δΰ=0;public float ņ=0;public float Γζ=0;public Vector3 Αζ=new Vector3();}class ł{public ι Џ=ι.Βψ;public float Ǘ=0;public float ā=0;
public string ǘ="";public string Ǜ="";DateTime ΖΞ;public bool ΓБ=false;private bool ΓΪ=false;public bool ΒΦ(bool Ȉ){if(Ȉ){ā=0;ΓΪ=false;return false;}ΓΪ=true;return ā>Ǘ;}public void Ηυ(){ΒΦ(true);ΓБ=false;}public void ΘΖ(){if(ΓΪ)
if((DateTime.Now-ΖΞ).TotalSeconds>1){ā++;ΖΞ=DateTime.Now;}}public bool Βμ(){switch(Џ){case ι.Βό:return true;case ι.Βύ:return true;}return false;}}class Ǥ{public String ψ="";public String σ="";public int Ǖ=0;public Л Ř=Л.ē;public Ǥ(String ψ,String σ,int Ǖ,Л Ř){this.ψ=ψ;this.σ=σ;
this.Ǖ=Ǖ;this.Ř=Ř;}}IMyGridTerminalSystem ω;Vector3 ΑЉ=new Vector3();Vector3 ά=new Vector3();Vector3 Ακ=new Vector3();Vector3 Ɨ=new Vector3();Vector3 Ǝ=new Vector3();DateTime ΖΝ=new DateTime();bool ƒ=true;int ΔΧ=0;bool ĉ=false;bool Ας=false;bool Ɯ=true;bool ΖЏ=false;bool ŕ=false;
bool Αο=false;float ȉ=0;float ı=0;int Ď=0;int ƭ=0;int ΑЌ=0;int ΓΟ=0;float Ś=0;float Ň=0;double ΓЅ=0;float ƃ=0;List<int>Œ=new List<int>();List<int>Ţ=new List<int>();Ζ Ι=Ζ.ΕΨ;int ώ=0;float ǵ=0;float Ǵ=0;int Ī=0;float Ȟ=0,ȝ=0;float ή=0;float ΔΙ=0,ΔΘ=0;IMyRemoteControl Υ;
IMySensorBlock ο;List<IMyTimerBlock>ΔΪ=new List<IMyTimerBlock>();List<IMyShipConnector>ц=new List<IMyShipConnector>();List<IMyThrust>ȋ=new List<IMyThrust>();List<IMyGyro>Αλ=new List<IMyGyro>();List<IMyTerminalBlock>π=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȇ=new List<IMyLandingGear>();
List<IMyReactor>č=new List<IMyReactor>();List<IMyConveyorSorter>ΑА=new List<IMyConveyorSorter>();List<IMyGasTank>ї=new List<IMyGasTank>();List<IMyTerminalBlock>Ɓ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŋ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ġ=new List<IMyTerminalBlock>();
List<IMyBatteryBlock>Ȱ=new List<IMyBatteryBlock>();List<IMyTextPanel>ј=new List<IMyTextPanel>();List<IMyTextSurface>ď=new List<IMyTextSurface>();List<IMyTextPanel>Ɗ=new List<IMyTextPanel>();IMyTerminalBlock ћ=null;Ψ Η=new Ψ();Ψ Γς=new Ψ();Ψ Γυ=new Ψ();bool Ȍ=false;bool ū=false;
bool Ŭ=false;bool Αρ=false;List<String>Ȋ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>Ʋ=new List<Ψ>();int Γί=0;double ΖА=0;Ψ Ε=new Ψ();Vector3 Ќ;Vector3 ǰ;Μ ΰ=Μ.в;Ǔ ī=Ǔ.ΑΖ;int Р=0;int Э=0;double ƕ=0;bool Αά=false;ł ό=new ł();ł ύ=new ł();з о=з.ΑΑ;bool Đ=true;
Ǔ ȥ=Ǔ.ΑΖ;int л=3;int к=3;int Щ=30;ѐ ч=ѐ.ΒΪ;bool Ȫ=true;bool Ŕ=true;bool ō=true;bool Ĳ=false;Ё П=Ё.Βφ;bool Ȩ=true;float ǯ=90;float ъ=20;float Ю=5;float ы=20;float Ǩ=0.70f;float Ů=1.50f;float ŭ=2.50f;float Ķ=0f;float ħ=0f;Α Τ;Μ щ;Α Ƨ;ΒЀ η=null;int Ы=0;int Ь=0;
int Н=0;int Ƞ=0;int į=30;int Х=0;bool Ѐ=true;Vector3 Ј;double В=0;bool ΑЋ=false;int Δε=0;int ϋ=0;int ΓΖ=0;int Ǳ=0;int Ζγ=0;Vector3 Ɵ=new Vector3();float м=0;float а=0;int ĵ=0;Ψ ΑБ=null;float Γψ=0;float ΑΡ=0;float У=0;float ĭ=0;bool Γσ=false;bool ĳ=false;bool Ʈ=false;
bool ț=false;bool Ȕ=false;DateTime Θγ=new DateTime();Ψ ģ=null;Ψ Ες=null;float ǿ=0;у ǩ;float Ȝ=0;float Ĵ=0;String ΓЀ="";float Αφ=0;float Ɖ=0;float ƙ=0;List<Ǥ>ǚ=new List<Ǥ>();float С=0;bool ƥ=false;bool ΖЅ=false;bool Ζς=false;bool ΒΔ=false;bool Ζσ=false;float ΓЃ=0;
float ΓЏ=0;Vector3 ΕЅ=new Vector3();Vector3 ν=new Vector3();Vector3 ΒΕ=new Vector3(1,1,1);float ΔΦ=1;Vector3 ƪ=new Vector3();bool Απ=false;bool Γμ=false;bool Γύ=false;bool Εέ=false;float ΑΪ=0;float ΑΜ=2;Vector3 ΖЃ;Vector3 ΔΥ;Vector3 ΔΤ;float[,]Ǣ=new float[3,2];Dictionary<String,float[,]>ў=new Dictionary<string,float[,]>();
Dictionary<String,float>ΖЍ=new Dictionary<String,float>();int ΓΙ=0;Dictionary<String,float[]>Ȣ=new Dictionary<string,float[]>();float ΔΑ;bool Αέ=false;int ΑΟ=0;int Ć=0;int Αν=0;int Ĩ=0;String Λ="";int[]ΖΩ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΪ=false;int ΑЀ=0;
int ǝ=0;Ϊ Б=Ϊ.Main;int ΙΥ=0,ΙΦ=0,ΙΧ=0,ΙΨ=0;int ΙΔ=0,ΙΕ=0;bool ŉ=false;bool Εή=false;String ΔΚ="";String ΓΞ="";IMyBroadcastListener ȕ=null;bool ΕЄ=true;String Θψ="";Exception ȟ=null;bool ΑЊ=false;Μ Ť=Μ.в;bool Ευ=false;bool ΖΜ=true;bool ΔΖ=false;String ΖЎ="";
bool Ƭ=false;bool ΔΟ=true;int Αξ=0;Vector3 ΑΤ=new Vector3();bool ΕЋ=false;bool ǒ(IMyTerminalBlock Φ)=>Φ.CubeGrid==Me.CubeGrid;Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βν(Me,controllerTag,true))
Ι=Ζ.ф;ρ(Ϊ.Main);if(Ι!=Ζ.ф){Λ="Welcome to [PAM]!";х ΔΕ=Ηφ();if(Ι==Ζ.ΕΨ){List<IMyShipDrill>ΕЀ=new List<IMyShipDrill>();List<IMyShipGrinder>ΕЌ=new List<IMyShipGrinder>();ω.GetBlocksOfType(ΕЀ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(ΕЌ,q=>q.CubeGrid==Me.CubeGrid);if(ΕЀ.Count>0)
{Ι=Ζ.ź;Λ="Miner mode enabled!";}else if(ΕЌ.Count>0){Ι=Ζ.Ǒ;Λ="Grinder mode enabled!";}else{Ι=Ζ.Ϋ;Ǽ(г.Ϋ);}}if(ΔΕ==х.ΑΕ) Ɯ=false;if(ΔΕ==х.Δό) Λ="Data restore failed!";if(ΔΕ==х.Βυ) Λ="New version, wipe data";}}void Main(string є,UpdateType Ιΰ){try{if(ȟ!=null){if(є.Trim().ToUpper()=="RETRY")
{ȟ=null;ƒ=true;Ď=0;if(Ι!=Ζ.ф) χ();Λ="Restarted after error";}else{Ηα();ΕΙ();return;}}Ας=(Ιΰ&UpdateType.Update10)!=0;if(Ας) ΔΧ++;ĉ=ΔΧ>=10;if(ĉ) ΔΧ=0;if(Ας){ΑΟ++;if(ΑΟ>4) ΑΟ=0;ΑЌ=(int) Math.Max(0,10-(DateTime.Now-ΖΝ).TotalSeconds);}if(є!="") Θψ=є;if(Ι!=Ζ.ф)
ΗЄ(є);else
ΗΣ(є);ΖΪ=false;try{int Εσ=Runtime.CurrentInstructionCount;float ơ=Д(Εσ,Runtime.MaxInstructionCount);if(ơ>0.90) Λ="Max. instructions >90%";if(ơ>Ś) Ś=ơ;if(Αέ){Œ.Add(Εσ);while(Œ.Count>10) Œ.RemoveAt(0);Ň=0;for(int i=0;i<Œ.Count;i++) Ň+=Œ[i];Ň=Д(Д(Ň,Œ.Count),Runtime.MaxInstructionCount);
double Γω=Runtime.LastRunTimeMs;if(Αο&&Γω>ΓЅ) ΓЅ=Γω;Ţ.Add((int)(Γω*1000f));while(Ţ.Count>10) Ţ.RemoveAt(0);ƃ=0;for(int i=0;i<Ţ.Count;i++) ƃ+=Ţ[i];ƃ=Д(ƃ,Ţ.Count)/1000f;}}catch{Ś=0;}}catch(Exception e){ȟ=e;}}void ΗЄ(string є){bool ΑЍ=false;String Ελ="";if(ώ<=1&&!ΔБ(є))
ΕΚ(є);int ΘЂ=50;while(ȕ!=null&&ȕ.HasPendingMessage&&ΘЂ-->0){MyIGCMessage ς=ȕ.AcceptMessage();String Χ=ς.Data as String;if(Χ==null) continue;String Ƥ="";if(ώ<=1&&Ηπ(ref Χ,out Ƥ,out Ελ)&&Ƥ==BROADCAST_SELF_ID){ΕΚ(Χ);ΑЍ=true;break;}}bool ΖΗ=Αο&&Τ==Α.й&&!ŕ&&!ΑЍ&&Ď==0&&!Αρ;
if(ĉ&&Τ!=Α.й) ΑЍ=true;if((Ας&&!ΖΗ)||(ĉ&&ΖΗ)){if(Ď==0&&(ΑЌ<=0||ƒ)){ΖЏ=ώ>0;ώ=0;Ď=1;ǽ();Ηϊ();Ηζ();Ǹ("Scan 1");}else if(Ď==1){Ď=2;ǽ();Ηϋ();Ǹ("Scan 2");}else if(Ď==2){Ď=0;ǽ();Ηό();Ηύ();if(ώ<=1) ΕΓ();Ǹ("Scan 3");ΖΝ=DateTime.Now;if(ώ<=1&&Ɯ)
ΑЉ=ƿ(Υ,Υ.CenterOfMass);Ɯ=false;if(ƒ){ƒ=false;bool Ιε=ΰ==Μ.І;χ();if(ΖΜ&&resumeAfterRestart&&Ιε&&ώ<=1) ΔΖ=true;}ΖΜ=false;if(ΖЏ&&ώ==0) Λ="Setup complete";}else{if(ΰ==Μ.І&&Ι!=Ζ.Ϋ){ǽ();ΗΑ();Ǹ("Inv balance");}ǽ();switch(ƭ){case 0:ΗЏ();break;case 1:ΘΒ();break;case 2:ΗЎ();break;
case 3:ΘΙ();break;case 4:ΘΑ();break;case 5:ΗΙ();break;case 6:ΗΖ(Υ);break;}Ǹ("Update: "+ƭ);ƭ++;if(ƭ>6){ƭ=0;Αο=true;if(Ť!=Μ.в){switch(Ť){case Μ.ķ:ȭ();break;case Μ.ĸ:ȭ();break;case Μ.І:ȭ();break;case Μ.ǆ:ƾ();break;case Μ.Ǉ:Ę();break;}Ť=Μ.в;}}}if(!ƒ){if(!ǻ(Υ,true)){Υ=null;
Ɯ=true;ώ=2;}if(ώ>=2&&Τ!=Α.й)
χ();if(ώ<=1){ı=Υ.CalculateShipMass().PhysicalMass;ȉ=(float) Υ.GetShipSpeed();ά=ΗЍ(Υ,ΑЉ);Ακ=Υ.WorldMatrix.Forward;Ɨ=Υ.WorldMatrix.Left;Ǝ=Υ.WorldMatrix.Down;ΘΕ();ΘΗ();ΗА();if(ΔΖ){ΔΖ=false;ȭ();if(Τ!=Α.й) Λ="Job resumed after restart";}if(Τ!=Α.й){ŕ=false;ΕΞ(false);Δρ(false);String ΙΝ=Βθ(Τ)+" "+(int) Τ;
ǽ();ΘΓ();ΒЏ(false);Ǹ(ΙΝ);ΘΘ();ΘΔ();ǽ();ΗБ();Ǹ("Thruster");ǽ();ΕΩ();Ǹ("Gyroscope");}else{if(ŕ){if(ǃ()){Ч(Ǝ,Ακ,Ɨ,0.25f,true);ΕΩ();Λ="Aligning to planet: "+Math.Round(ΑΪ-0.25f,2)+"°";if(Απ) Βλ(true,true);}else Βλ(true,true);}}Ƭ=false;}}}ǽ();ΕΙ();Ǹ("Print");if(ΑЍ||ΓΟ<=0){ǽ();ΗΒ(Ελ);
Ǹ("Broadcast");ΓΟ=4;}else if(ĉ) ΓΟ--;}void ΕΚ(string є){if(є=="") return;var Й=є.ToUpper().Split(' ');Й.DefaultIfEmpty("");var ΓΦ=Й.ElementAtOrDefault(0);var ġ=Й.ElementAtOrDefault(1);var Ƃ=Й.ElementAtOrDefault(2);var Εα=Й.ElementAtOrDefault(3);String ć="Invalid argument: "+є;bool Δί=false;
switch(ΓΦ){case "UP":this.ΕΕ(false);break;case "DOWN":this.ΕΔ(false);break;case "UPLOOP":this.ΕΕ(true);break;case "DOWNLOOP":this.ΕΔ(true);break;case "APPLY":this.Δο(true);break;case "MRES":ǝ=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.ΒЋ();}break;case "PATH":{this.χ();this.ΒЋ();Η.ε=true;}break;
case "START":{this.χ();ΒЊ();}break;case "ALIGN":{Βλ(!ŕ,false);}break;case "CONT":{this.χ();this.ȭ();}break;case "JOBPOS":{this.χ();this.Ę();}break;case "HOMEPOS":{this.χ();this.ƾ();}break;case "FULL":{ƥ=true;}break;case "RESET":{ΑЊ=true;ώ=2;}break;case "EXPORT":{ΗΩ();}break;
case "IMPORT":{Ηγ(ġ=="JOB");}break;default:Δί=true;break;}if(Ι!=Ζ.Ϋ){switch(ΓΦ){case "SHUTTLE":{ΗΧ();}break;case "CFGS":{if(!ΗΠ(ġ,Ƃ,Εα)) Λ=ć;}break;case "CFGB":{if(!ΗΟ(ġ,Ƃ)) Λ=ć;}break;case "CFGL":{if(!ě(ref ǯ,true,τ.Ǆ,ġ,"")||!ΗΡ(Ƃ))
Λ=ć;}break;case "CFGE":{if(!ě(ref Ю,true,τ.ǅ,ġ,"IG")||
!ě(ref ъ,true,τ.Βσ,Ƃ,"IG")||
!ě(ref ы,true,τ.Βτ,Εα,"IG"))
Λ=ć;}break;case "CFGA":{if(!ě(ref Ǩ,false,τ.ƴ,ġ,"")) Λ=ć;}break;case "CFGW":{if(!ě(ref Ů,false,τ.ƀ,ġ,"")||
!ě(ref ŭ,false,τ.ƀ,Ƃ,""))
Λ=ć;}break;case "NEXT":{ΒΜ(false);}break;case "PREV":{ΒΜ(true);}break;default:if(Δί) Λ=ć;break;}}else{switch(ΓΦ){case "UNDOCK":{Ƭ=true;}break;default:if(Δί) Λ=ć;break;}}}String Ηέ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΧ(){χ();Ι=Ζ.Ϋ;Ǽ(г.Ϋ);Ε.ε=false;Η.ε=false;ο=null;π.Clear();ΰ=Μ.в;}void Βλ(bool Љ,bool і){if(!Љ) Λ="Aligning canceled";if(і) Λ="Aligning done";if(і||!Љ){ŕ=false;ΕΣ();ΑΓ(false,0,0,0,0);return;}if(ǃ()) ŕ=true;}bool ΗΠ(String Ιη,String Θξ,String Θέ)
{bool Ιγ=ΰ==Μ.І;int w,h,d;if(int.TryParse(Ιη,out w)&&int.TryParse(Θξ,out h)&&int.TryParse(Θέ,out d)){this.χ();л=w;к=h;Щ=d;Ğ(false);ſ(false,false);if(Ιγ) ȭ();return true;}return false;}bool ΗΡ(String Εΰ){if(Εΰ=="ON"){Ȫ=true;return true;}if(Εΰ=="OFF"){Ȫ=false;return true;}
return false;}bool ΗΟ(String ΕϏ,String ΑΫ){bool Ɲ=true;if(ΕϏ=="HOME") Đ=true;else if(ΕϏ=="STOP") Đ=false;else Ɲ=false;if(ΑΫ=="HOME") о=з.ΑΑ;else if(ΑΫ=="STOP") о=з.ΕΡ;else if(ΑΫ=="JOB") о=з.Βγ;else if(ΑΫ=="IG") о=з.Ŵ;else Ɲ=false;return Ɲ;}void ρ(Ϊ ΓЍ){ΖΩ[(int) Б]=ǝ;
ǝ=ΖΩ[(int) ΓЍ];if(ΓЍ==Ϊ.ΗЃ) ǝ=0;Б=ΓЍ;if(Ι!=Ζ.ф)
ĝ(Б==Ϊ.Ź,false,0,0);ΖΪ=true;}void ΕΕ(bool Γώ){if(ǝ>0) ǝ--;else if(Γώ) ǝ=ΑЀ-1;}void ΕΔ(bool Γώ){if(ǝ<ΑЀ-1) ǝ++;else if(Γώ) ǝ=0;}String Ǽ(г ς){switch(ς){case г.ΑΒ:Λ="Job is running";break;case г.ű:Λ="Connector not ready!";break;case г.ΗЅ:Λ="Ship modified, path outdated!";break;
case г.ΔЎ:Λ="Interrupted by player!";break;case г.Ϋ:Λ="Shuttle mode enabled!";break;}return "";}String Ηΰ(Ǔ λ){switch(λ){case Ǔ.ΑΖ:return "Top-Left";case Ǔ.ΗΘ:return "Center";default:return "";}}String ΗΫ(ѐ ƛ){switch(ƛ){case ѐ.ΒΣ:return "Auto"+(Ι==Ζ.ź?" (Ore)":"");case ѐ.ΒΤ:return "Auto (+Stone)";
case ѐ.ΒΪ:return "Default";default:return "";}}String Βζ(Μ Ц){switch(Ц){case Μ.в:return "No job";case Μ.Ŀ:return "Job paused";case Μ.І:return "Job active";case Μ.ķ:return "Job active";case Μ.ĸ:return "Job active";case Μ.Βέ:return "Job done";case Μ.Ƹ:return "Job changed";case Μ.ǆ:return "Move home";
case Μ.Ǉ:return "Move to job";}return "";}String ΔЁ(з ΘΤ){switch(ΘΤ){case з.ΑΑ:return "Return home";case з.Βγ:return "Fly to job pos";case з.ΕΡ:return "Stop";case з.Ŵ:return "Ignore";}return "";}String Ηά(Ё ƛ){switch(ƛ){case Ё.Βφ:return "Off";case Ё.Βή:return "Drop pos (Stone) ";case Ё.ƺ:return "Drop pos (Sto.+Ice)";
case Ё.Δχ:return "Cur. pos (Stone)";case Ё.ΒΨ:return "Cur. pos (Sto.+Ice)";case Ё.Βο:return "In motion (Stone)";case Ё.ǁ:return "In motion (Sto.+Ice)";}return "";}String ΔϏ(у Ц){switch(Ц){case у.None:return "No batteries";case у.ĕ:return "Charging";case у.Δϊ:return "Discharging";}return "";}String Βθ(Α Ц)
{String Ις=Ι==Ζ.Ϋ?"target":"job";switch(Ц){case Α.й:return "Idle";case Α.ǐ:return "Flying to XY position";case Α.Ě:return Ι==Ζ.Ǒ?"Grinding":"Mining";case Α.ǥ:return "Returning";case Α.ė:return "Flying to drop pos";case Α.ђ:return "Returning to dock";case Α.Ђ:return "Flying to dock area";
case Α.Ш:return "Flying to job area";case Α.ǹ:return "Flying to path";case Α.Ļ:return "Flying to job position";case Α.Ǐ:return "Approaching dock";case Α.Ė:return "Docking";case Α.Ē:return "Aligning";case Α.ȫ:return "Aligning";case Α.Ż:return "Retry docking";case Α.Ǿ:return "Unloading";case Α.ȯ:return ΖЎ;
case Α.ǔ:return "Undocking";case Α.ĕ:return "Charging batteries";case Α.ΑΚ:return "Waiting for uranium";case Α.Ƽ:return "Filling up hydrogen";case Α.ΒА:return "Waiting for ejection";case Α.ΑΙ:return "Waiting for ejection";case Α.ƽ:return "Flying to drop pos";}return "";}String ΔЇ(ι Џ){switch(Џ)
{case ι.Βψ:return "On \"Undock\" command";case ι.Βό:return "On player entered cockpit";case ι.ΕΘ:return "On ship is full";case ι.ΕΗ:return "On ship is empty";case ι.Βύ:return "On time delay";case ι.ǋ:return "On batteries empty(<25%)";case ι.Ǌ:return "On batteries empty(=0%)";case ι.Βχ:return "On batteries full";
case ι.Βϋ:return "On hydrogen empty(<25%)";case ι.Βω:return "On hydrogen empty(=0%)";case ι.Βϊ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Ζν,int ą,bool Ξ,String Ɩ){ΑЀ+=1;if(Ζν==ą) Ɩ=">"+Ɩ+(ΑΟ>=2?" .":"");else Ɩ=" "+Ɩ;Γ+=Ɩ+"\n";return Ζν==ą&&Ξ;}String Δο(bool Ξ)
{int Β=0;int Ο=ǝ;ΑЀ=0;String Ζπ="———————————————\n";String Ū="--------------------------------------------\n";String Γ="";Γ+=Βζ(ΰ)+" | "+(Η.ε?"Ready to dock":"No dock")+"\n";Γ+=Ζπ;double ю=Math.Max(Math.Round(this.В),0);if(Б==Ϊ.Main){bool Ĉ=Ι==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЋ();
if(Ι==Ζ.ź)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) ρ(Ϊ.Ź);if(Ι==Ζ.Ǒ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) ρ(Ϊ.Ź);if(Ι==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) ρ(Ϊ.ΒЇ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȭ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƾ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ę();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ĉ) ρ(Ϊ.ΑΔ);else ρ(Ϊ.ƶ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) ρ(Ϊ.ΔЋ);if(Ι!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) ρ(Ϊ.ΔЉ);}else if(Б==Ϊ.Ź){double Ιζ=Math.Round(л*ǵ,1);double Θν=Math.Round(к*Ǵ,1);String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Start new job!")) ΒЊ();if(Π(ref Ѕ,Ο,Β++,Ξ," Change current job")){ſ(false,false);ρ(Ϊ.Main);}if(Π(ref Ѕ,Ο,Β++,Ξ," Width + (Width: "+л+" = "+Ιζ+"m)")){Ĺ(ref л,5,20,1);Ğ(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Width -")){Ĺ(ref л,-5,20,-1);Ğ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height + (Height: "+к+" = "+Θν+"m)")){Ĺ(ref к,5,20,1);Ğ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height -")){Ĺ(ref к,-5,20,-1);Ğ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth + ("+(ч==ѐ.ΒΪ?"Depth":"Min")+": "+Щ+"m)")){Ĺ(ref Щ,5,50,2);Ğ(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Depth -")){Ĺ(ref Щ,-5,50,-2);Ğ(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth mode: "+ΗΫ(ч))){ч=Ȯ(ч);}if(Π(ref Ѕ,Ο,Β++,Ξ," Start pos: "+Ηΰ(ȥ))){ȥ=Ȯ(ȥ);}if(Ι==Ζ.Ǒ&&ч==ѐ.ΒΤ) ч=Ȯ(ч);Γ+=ΕΜ(8,Ѕ,Ο,ref ΙΔ);}else if(Б==Ϊ.ΒЇ){float[]Εχ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) ρ(Ϊ.ΕΟ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) ρ(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔЇ(ό.Џ))) ό.Џ=Ȯ(ό.Џ);if(!ό.Βμ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βε((int) ό.Ǘ))) ό.Ǘ=Δψ(ό.Ǘ,Εχ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔЇ(ύ.Џ))) ύ.Џ=Ȯ(ύ.Џ);
if(!ύ.Βμ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βε((int) ύ.Ǘ))) ύ.Ǘ=Δψ(ύ.Ǘ,Εχ);}else if(Б==Ϊ.ΕΟ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЊ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) ρ(Ϊ.ΒЇ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǘ!=""?ό.ǘ:"-"))) ό.ǘ=ǉ(ref ΙΥ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǜ!=""?ό.Ǜ:"-"))) ό.Ǜ=ǉ(ref ΙΧ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.ǘ!=""?ύ.ǘ:"-"))) ύ.ǘ=ǉ(ref ΙΦ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.Ǜ!=""?ύ.Ǜ:"-"))) ύ.Ǜ=ǉ(ref ΙΨ);
}else if(Б==Ϊ.Ÿ){String ΖΣ=ǯ+" %";if(ĉ) Ć++;if(Ć>1){Ć=0;Αν++;if(Αν>1) Αν=0;}bool[]ΔΜ=new bool[]{č.Count==0,ǩ==у.None,ї.Count==0};int ΖЊ=0;while(true){ΖЊ++;Ĩ++;if(Ĩ>ΔΜ.Length-1) Ĩ=0;if(ΖЊ>=ΔΜ.Length) break;if(!ΔΜ[Ĩ]) break;}bool Ĉ=Ι==Ζ.Ϋ;if(!Ĉ&&Ȫ&&С!=-1&&Αν==0)
ΖΣ=С<1000000?Math.Round(С)+" Kg":Math.Round(С/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();ρ(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ĉ) ρ(Ϊ.ƶ);else ρ(Ϊ.ΑΔ);if(!Ĉ){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΜ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƭ=true;Γ+=Ū;if(!Ĉ) Γ+="Progress: "+Math.Round(ƕ,1)+" %\n";Γ+="State: "+Βθ(Τ)+" "+ю+"m \n";Γ+="Load: "+Αφ+" % Max: "+ΖΣ+" \n";if(Ĩ==0) Γ+="Uranium: "+(č.Count==0?"No reactors":Math.Round(Ĵ,1)+"Kg "+(Ю==-1?"":" Min: "+Ю+" Kg"))+"\n";
if(Ĩ==1) Γ+="Battery: "+(ǩ==у.None?ΔϏ(ǩ):ǿ+"% "+(ъ==-1||Ĉ?"":" Min: "+ъ+" %"))+"\n";if(Ĩ==2) Γ+="Hydrogen: "+(ї.Count==0?"No tanks":Math.Round(Ȝ,1)+"% "+(ы==-1||Ĉ?"":" Min: "+ы+" %"))+"\n";}else if(Б==Ϊ.ƶ){String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) ρ(Ϊ.Ÿ);else ρ(Ϊ.Main);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) ж(ref ǯ,ǯ<=80?-10:-5,τ.Ǆ,false);if(Π(ref Ѕ,Ο,Β++,Ξ," Weight limit: "+(Ȫ?"On":"Off"))) Ȫ=!Ȫ;if(Π(ref Ѕ,Ο,Β++,Ξ," Ejection: "+Ηά(П))) П=Ȯ(П);if(Π(ref Ѕ,Ο,Β++,Ξ," Toggle sorters: "+(Ĳ?"On":"Off"))){Ĳ=!Ĳ;if(Ĳ) ǧ(ΔΟ);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Unload ice: "+(Ȩ?"On":"Off"))) Ȩ=!Ȩ;if(Π(ref Ѕ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) ж(ref Ю,(Ю>5?-5:-1),τ.ǅ,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Battery: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) ж(ref ъ,-5,τ.Βσ,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Hydrogen: "+(ы==-1?"Ignore":"Min "+ы+"%"))) ж(ref ы,-10,τ.Βτ,true);
if(Π(ref Ѕ,Ο,Β++,Ξ," When done: "+(Đ?"Return home":"Stop"))) Đ=!Đ;if(Π(ref Ѕ,Ο,Β++,Ξ," On damage: "+ΔЁ(о))) о=Ȯ(о);if(Π(ref Ѕ,Ο,Β++,Ξ," Advanced...")) ρ(Ϊ.ΒΝ);Γ+=ΕΜ(8,Ѕ,Ο,ref ΙΕ);}else if(Б==Ϊ.ΒΝ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) ρ(Ϊ.Ÿ);else ρ(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Ι==Ζ.Ǒ?" Grinder":" Drill")+" inv. balancing: "+(Ŕ?"On":"Off"))) Ŕ=!Ŕ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Ι==Ζ.Ǒ?" grinders":" drills")+": "+(ō?"Fwd + Bwd":"Fwd"))) ō=!ō;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ů+"m/s")) ж(ref Ů,0.5f,τ.ƀ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ŭ+"m/s")) ж(ref ŭ,0.5f,τ.ƀ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) ж(ref Ǩ,0.1f,τ.ƴ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ķ*100f+"%")) Δσ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ħ*100f+"%")) Δσ(false,0.05f);
}else if(Б==Ϊ.ΑΔ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ΰ==Μ.І) ρ(Ϊ.Ÿ);else ρ(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) ж(ref ǯ,ǯ<=80?-10:-5,τ.Ǆ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȩ?"On":"Off"))) Ȩ=!Ȩ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) ж(ref Ю,(Ю>5?-5:-1),τ.ǅ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ъ==-1?"Ignore":"Charge up"))) ъ=(ъ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ы==-1?"Ignore":"Fill up"))) ы=(ы==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+ΔЁ(о))) о=Ȯ(о);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) ж(ref Ǩ,0.1f,τ.ƴ,false);
}else if(Б==Ϊ.ΕΛ){double Θω=0;if(Σ.Count>0) Θω=Vector3.Distance(Σ.Last().Δ,ά);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΤ();if(Ι!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȍ?"Use old home":(Η.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȍ?"Use old connector":(Η.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(ū?"Use old connector":(Ε.ε?"Was set! ":"none ")))) ū=!ū;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ŭ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ŭ=!Ŭ;Γ+=Ū;Γ+="Wp spacing: "+Math.Round(ΖА)+"m\n";}else if(Б==Ϊ.ΒЌ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();ρ(Ϊ.Main);}
Γ+=Ū;Γ+="State: "+Βθ(Τ)+" \n";Γ+="Speed: "+Math.Round(ȉ,1)+"m/s\n";Γ+="Target dist: "+ю+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+Δε+"\n";}else if(Б==Ϊ.ΔЋ){List<IMyTerminalBlock>Ĥ=ΔЂ();if(ĉ) Ć++;if(Ć>=Ĥ.Count) Ć=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) ρ(Ϊ.ΔЌ);
Γ+=Ū;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αφ,1)+"% "+Math.Round(Ɖ,1)+" / "+Math.Round(ƙ,1)+"\n";Γ+="Uranium: "+(č.Count==0?"No reactors":Math.Round(Ĵ,1)+"Kg "+ΓЀ)+"\n";Γ+="Battery: "+(ǩ==у.None?"":ǿ+"% ")+ΔϏ(ǩ)+"\n";Γ+="Hydrogen: "+(ї.Count==0?"No tanks":Math.Round(Ȝ,1)+"% ")+"\n";
Γ+="Damage: "+(Ĥ.Count==0?"None":""+(Ć+1)+"/"+Ĥ.Count+" "+Ĥ[Ć].CustomName)+"\n";}else if(Б==Ϊ.ΔЌ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) ρ(Ϊ.Main);Γ+=Ū;Γ+="Next scan: "+ΑЌ+"s\n";Γ+="Drill area: "+Math.Round(ǵ,1)+"x"+Math.Round(Ǵ,1)+"m Ship: "+Math.Round(ΔΙ,1)+"x"+Math.Round(ΔΘ,1)+"m ("+Math.Round(ή,1)+"m)\n"+(Ǫ.Count>0?"Cameras: "+ΓΡ+"\n":"");
Γ+="Broadcast: "+(ŉ?"Online - "+ΔΚ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ś*100f,1)+"% \n";}else if(Б==Ϊ.ΔЉ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) ρ(Ϊ.Main);Γ+=Ū;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+ΑЌ+"s\n";if(ΑЊ) Γ="Recompile script now";int Θϊ=Γ.Split('\n').Length;for(int i=Θϊ;i<=10;i++) Γ+="\n";Γ+=Ζπ;Γ+="Last: "+Λ+"\n";
return Γ;}void Δσ(bool Θτ,float ѝ){χ();ſ(true,false);if(Θτ) ж(ref Ķ,ѝ,τ.Βώ,false);else ж(ref ħ,ѝ,τ.Βώ,false);Δς();Ȟ=ǵ;ȝ=Ǵ;ĝ(true,true,0,0);}float Δψ(float θ,float[]ΒΛ){float ί=ΒΛ[0];for(int i=ΒΛ.Length-1;i>=0;i--)
if(θ<ΒΛ[i]) ί=ΒΛ[i];return ί;}String ǉ(ref int ą){String ί="";if(ą>=ΔΪ.Count) ą=-1;if(ą>=0) ί=ΔΪ[ą].CustomName;ą++;return ί;}void ΑΗ(string ψ){if(ΰ!=Μ.І) return;if(ψ=="") return;IMyTerminalBlock Φ=ω.GetBlockWithName(ψ);if(Φ==null||!(Φ is IMyTimerBlock)){Λ="Timerblock "+ψ+" not found!";
return;}((IMyTimerBlock) Φ).Trigger();}void Ĺ(ref int value,int ƅ,int Ȧ,int ΔΝ){if(ƅ==0) return;if(value<Ȧ&&ΔΝ>0||value<=Ȧ&&ΔΝ<0){value+=ΔΝ;return;}int ΘΛ=Math.Abs(ƅ);int ΒБ=0;int ΓЇ=1;while(true){ΒБ+=ΓЇ*ΘΛ*10;if(ƅ<0&&value-Ȧ<=ΒБ) break;if(ƅ>0&&value-Ȧ<ΒБ) break;
ΓЇ++;}value+=ΓЇ*ƅ;}void Ğ(bool Ιυ){л=Math.Max(л,1);к=Math.Max(к,1);Щ=Math.Max(Щ,0);ĝ(Б==Ϊ.Ź,false,0,0);}bool ě(ref float value,bool ΘБ,τ σ,String ĩ,String Θρ){if(ĩ=="") return false;float ş=-1;bool ΖΔ=false;if(ĩ.ToUpper()==Θρ) ΖΔ=true;else if(!float.TryParse(ĩ,out ş)) return false;
else ş=Math.Max(0,ş);if(ΘБ) ş=(float) Math.Round(ş);ж(ref value,ş,σ,ΖΔ,false);return true;}void ж(ref float value,float ѝ,τ σ,bool ƫ){ж(ref value,value+ѝ,σ,ƫ,true);}void ж(ref float value,float ś,τ σ,bool ƫ,bool Ικ){float Ǟ=0,ǜ=0;if(σ==τ.ƀ){Ǟ=0.5f;ǜ=10f;}if(σ==τ.ƴ){Ǟ=0.1f;ǜ=1f;}
if(σ==τ.Ŵ){Ǟ=50f;ǜ=100f;}if(σ==τ.Βσ){Ǟ=5f;ǜ=30f;}if(σ==τ.ǅ){Ǟ=1f;ǜ=25f;}if(σ==τ.Ǆ){Ǟ=10f;ǜ=95f;}if(σ==τ.Βτ){Ǟ=10f;ǜ=90f;}if(σ==τ.ΗЋ){Ǟ=10f;ǜ=1800;}if(σ==τ.Βώ){Ǟ=0.0f;ǜ=0.75f;}if(ś==-1&&ƫ){value=-1;return;}if(value==-1) ƫ=false;
bool Ζε=ś<Ǟ||ś>ǜ;if(Ζε&&Ικ){if(ś<value) value=ǜ;else if(ś>value) value=Ǟ;}else value=ś;if(Ζε&&ƫ) value=-1;else value=Math.Max(Ǟ,Math.Min(value,ǜ));value=(float) Math.Round(value,2);}void ΒΜ(bool ΘЍ){if(ΘЍ) Н=Math.Max(0,Н-1);else Н++;Βΰ();Βί();for(int k=0;k<ȅ.Count&&k<О.Length;k++) ΒЅ(ȅ[k],k<Н);
Є=-1;ΒЏ(true);}T Ȯ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΨ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΨ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЋ(){Ʋ.Clear();for(int i=0;i<Σ.Count;i++) Ʋ.Add(Σ[i]);Σ.Clear();
Αρ=true;Γς=new Ψ(Η);Γυ=new Ψ(Ε);Η.ε=false;if(Ι==Ζ.Ϋ) Ε.ε=false;for(int i=0;i<ў.Count;i++)
if(!Ȋ.Contains(ў.Keys.ElementAt(i)))
Ȋ.Add(ў.Keys.ElementAt(i));Ȍ=false;ū=false;Ŭ=false;ρ(Ϊ.ΕΛ);}void ΕΤ(){if(Ȍ) Η=Γς;if(ū) Ε=Γυ;if(Ŭ){Σ.Clear();for(int i=0;i<Ʋ.Count;i++) Σ.Add(Ʋ[i]);}Αρ=false;χ();ρ(Ϊ.Main);}void ΘΕ(){if(!Αρ) return;if(Τ!=Α.й){ΕΤ();return;}if(!Γς.ε) Ȍ=false;if(!Γυ.ε) ū=false;if(Ʋ.Count<=1) Ŭ=false;
IMyShipConnector М=Т(MyShipConnectorStatus.Connectable);if(М==null) М=Т(MyShipConnectorStatus.Connected);if(М!=null){if(Math.Round(ȉ,2)<=0.20) Γί++;else Γί=0;if(Γί>=5){if(Ι==Ζ.Ϋ&&(Η.ε||Ȍ)&&Vector3.Distance(Η.Δ,М.GetPosition())>5){Ε.δ=Υ.WorldMatrix.Forward;Ε.υ=Υ.WorldMatrix.Left;
Ε.γ=Υ.WorldMatrix.Down;Ε.ξ=Υ.GetNaturalGravity();Ε.Δ=М.GetPosition();Ε.ε=true;Ε.Ъ=М.Position;}else{Η.δ=Υ.WorldMatrix.Forward;Η.υ=Υ.WorldMatrix.Left;Η.γ=Υ.WorldMatrix.Down;Η.ξ=Υ.GetNaturalGravity();Η.Δ=М.GetPosition();Η.ε=true;Η.Ъ=М.Position;}}}double Γχ=-1;if(Σ.Count>0) Γχ=Vector3.Distance(ά,Σ.Last().Δ);
double ΙΛ=Math.Max(1.5,Math.Pow(ȉ/100.0,2));double ΖΫ=Math.Max(ȉ*ΙΛ,2);ΖА=ΖΫ;if((Γχ==-1)||Γχ>=ΖΫ){Ψ Ω=new Ψ(ά,Ακ,Ǝ,Ɨ,Υ.GetNaturalGravity());Ω.ΗΔ(ȋ,Ȋ);Σ.Add(Ω);}}Ψ и(){if(Ι!=Ζ.Ϋ) return њ!=null?њ:Η;return Ες;}void ŀ(Ψ έ,Μ Ц){Ες=έ;if(ΰ==Μ.І) щ=Ц;}bool Ǻ(Ψ έ,float Ѓ,bool Ια,out Vector3 λ)
{if(Ια){Vector3I Γλ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);IMySlimBlock Φ=Me.CubeGrid.GetCubeBlock(Γλ);if(Φ==null||!(Φ.FatBlock is IMyShipConnector)){λ=new Vector3();return false;}Vector3 ΘΩ=б(Υ,Φ.FatBlock.GetPosition()-ά);Vector3 ΘΨ=б(Υ,Φ.FatBlock.WorldMatrix.Forward);λ=έ.Δ-ΒЎ(έ.δ,έ.γ*-1,ΘΩ)-ΒЎ(έ.δ,έ.γ*-1,ΘΨ)*Ѓ;
return true;}else{λ=έ.Δ;return true;}}Vector3 Ȏ(int ΘΦ,int š,bool ΘЏ){if(!ΘЏ&&ΕЋ) return ΑΤ;float ΘΧ=((Р-1f)/2f)-ΘΦ;float ΙΑ=((Э-1f)/2f)-š;ΑΤ=Ε.Δ+Ε.υ*ΘΧ*ľ()+ǰ*-1*ΙΑ*Ľ();ΕЋ=true;return ΑΤ;}Vector3 Βη(Vector3 ΙΟ,float Γά){return ΙΟ+(Ќ*Γά);}β ΒΫ(){float А=-1;
β ί=β.Ƿ;if(Ι!=Ζ.Ϋ){ΕΓ();if(ΰ!=Μ.в){Vector3 Њ=Ĝ(Ќ,ǰ*-1,ά-Ε.Δ);if(Math.Abs(Њ.X)<=(float)(Р*ǵ)/2f&&Math.Abs(Њ.Y)<=(float)(Э*Ǵ)/2f){if(Њ.Z<=-1&&Њ.Z>=-į*2) return β.Ƶ;if(Њ.Z>-1&&Њ.Z<ή*2) return β.Ƿ;}if(Ȭ(Ε.Δ,ref А)) ί=β.Ƿ;}if(Η.ε){if(Ȭ(Η.Δ,ref А)) ί=β.Ƕ;
for(int i=0;i<Σ.Count;i++)
if(Ȭ(Σ[i].Δ,ref А)) ί=β.ȏ;if(Vector3.Distance(ά,Η.Δ)<dockDist*ή) ί=β.Ƕ;if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null)
ί=β.Ƕ;}}else{Vector3 λ=new Vector3();IMyShipConnector Ȃ=Т(MyShipConnectorStatus.Connected);if(Η.ε){if(Ȭ(Η.Δ,ref А)) ί=β.Ƕ;if(Ǻ(Η,dockDist,true,out λ))
if(Ȭ(λ,ref А)) ί=β.Ƕ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Η.Δ)<5)
return β.ΒΠ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Η.Δ)>dockDist*ή&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ή)
if(Ȭ(Σ[i].Δ,ref А)) ί=β.ȏ;if(Ε.ε){if(Ȭ(Ε.Δ,ref А)) ί=β.Ƿ;if(Ǻ(Ε,dockDist,true,out λ))
if(Ȭ(λ,ref А)) ί=β.Ƿ;if(Ȃ!=null&&Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5)
return β.ΒΡ;}}return ί;}bool Ȭ(Vector3 λ,ref float А){float Ѓ=Vector3.Distance(λ,ά);if(Ѓ<А||А==-1){А=Ѓ;return true;}return false;}void ΒЊ(){if(ώ>0){Λ="Setup error! Can't start";return;}if(Ι==Ζ.Ϋ){ȭ();return;}Ε.Δ=ά;Ε.ξ=Υ.GetNaturalGravity();Ε.δ=Ακ;Ε.γ=Ǝ;Ε.υ=Ɨ;
Ќ=ћ.WorldMatrix.Forward;ǰ=Ε.γ;if(Ќ==Υ.WorldMatrix.Down) ǰ=Υ.WorldMatrix.Backward;Ȟ=ǵ;ȝ=Ǵ;ſ(true,true);Ν(Α.ǐ);ΒЉ();}void ſ(bool Ǚ,bool ΘА){if(ΰ==Μ.в&&!Ǚ) return;bool ΘЄ=Ǚ||ΰ==Μ.Βέ||Р!=л||Э!=к||ī!=ȥ;if(ΘЄ){if(ΰ!=Μ.в){ΰ=Μ.Ƹ;Ȏ(Ы,Ь,ΘА);Λ="Job changed, lost progress";
}ī=ȥ;Р=л;Э=к;Ь=0;Ы=0;О="";Є=-1;Ī=(int)(DateTime.Now.Ticks/10000000%1000000);ĭ=0;Ƞ=0;У=0;Н=0;ΒЏ(true);}}void ΒЁ(){Е(ά,0);ѓ(ȋ,true);}void ΒЉ(){Ǽ(г.ΑΒ);ΒЁ();Ž(Ȇ,false);ǧ(ΔΟ);ΰ=Μ.І;Δρ(true);щ=ΰ;ρ(Ϊ.Ÿ);Ηω();Αά=true;Αξ=0;for(int i=Ɓ.Count-1;i>=0;i--)
if(Ŷ(Ɓ[i],false)) Αξ++;if(Αξ>0) Λ="Started with damage";}void χ(){if(ΰ==Μ.І){ΰ=Μ.Ŀ;Λ="Job paused";}Ν(Α.й);щ=ΰ;ΑΓ(false,0,0,0,0);ΒЍ();Δξ(new Vector3(),false);ΕΣ();ΒЄ(ChargeMode.Auto);ΒІ(false);ΕΞ(true);Ȑ(Α.й);ĝ(false,false,0,0);ѓ(π,false);ѓ(ц,true);ǧ(true);ΑЋ=false;Ű();
ƍ=false;ΒЂ();Αά=false;ƥ=false;Ƭ=false;if(Б!=Ϊ.Main&&Б!=Ϊ.ƶ&&Б!=Ϊ.ΒΝ&&Б!=Ϊ.ΑΔ)
ρ(Ϊ.Main);}void ȭ(){β И=ΒΫ();if(Ι==Ζ.Ϋ){if(!Ε.ε||!Η.ε) return;ΒЉ();bool Γι=Vector3.Distance(ά,Η.Δ)<Vector3.Distance(ά,Ε.Δ);if(Ť==Μ.ķ) Γι=true;if(Ť==Μ.ĸ) Γι=false;if(Γι){ŀ(Η,Μ.ķ);switch(И){case β.ΒΠ:Ν(Α.ȯ);break;case β.ȏ:Ν(Α.Ш);break;case β.Ƕ:Ν(Α.Ǐ);break;default:Ν(Α.ǔ);break;
}}else{ŀ(Ε,Μ.ĸ);switch(И){case β.ΒΡ:Ν(Α.ȯ);break;case β.Ƿ:Ν(Α.Ǐ);break;case β.ȏ:Ν(Α.Ш);break;default:Ν(Α.ǔ);break;}}}else{if(ΰ!=Μ.Ŀ&&ΰ!=Μ.Ƹ) return;bool Ιδ=ΰ==Μ.Ƹ;ΒЉ();bool Γξ=ŷ(false)&&Η.ε;switch(И){case β.Ƿ:Ν(Γξ?Α.ǹ:Α.ǐ);break;case β.ȏ:Ν(Γξ?Α.Ђ:Α.Ш);break;case β.Ƕ:Ν(Γξ?Α.Ǐ:Α.Ǿ);break;
case β.Ƶ:{if(Ƞ!=Н||Ιδ) Ν(Α.ǥ);else Ν(Α.Ě);}break;default:break;}}}void Ę(){if(ΰ==Μ.в&&!Η.ε) return;if(Ι==Ζ.Ϋ&&(!Ε.ε||!Η.ε)) return;Λ="Move to job";β И=ΒΫ();if(Ι==Ζ.Ϋ){ŀ(Ε,Μ.ĸ);switch(И){case β.Ƿ:Ν(Α.Ǐ);break;case β.ȏ:Ν(Α.Ш);break;case β.ΒΡ:return;default:Ν(Α.ǔ);break;
}Ȑ(Α.ȯ);}else{switch(И){case β.Ƿ:Ν(Α.Ш);break;case β.ȏ:Ν(Α.Ш);break;case β.Ƕ:Ν(Α.Ǿ);break;case β.Ƶ:Ν(Α.ǥ);break;default:break;}if(ΰ==Μ.в) Ȑ(Α.Ш);else Ȑ(Α.ȫ);ΑЋ=true;}ΒЁ();ρ(Ϊ.ΒЌ);Ž(Ȇ,false);щ=Μ.Ǉ;}void ƾ(){if(!Η.ε) return;Λ="Move home";β И=ΒΫ();if(Ι==Ζ.Ϋ){ŀ(Η,Μ.ķ);switch(И)
{case β.ȏ:Ν(Α.Ђ);break;case β.Ƕ:Ν(Α.Ǐ);break;case β.ΒΠ:return;default:Ν(Α.ǔ);break;}Ȑ(Α.ȯ);}else{if(Т(MyShipConnectorStatus.Connected)!=null) return;if(Т(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Ė);Ȑ(Α.Ǿ);return;}switch(И){case β.Ƿ:Ν(Α.ǹ);break;case β.ȏ:Ν(Α.Ђ);break;case β.Ƕ:Ν(Α.Ђ);break;
case β.Ƶ:Ν(Α.ђ);break;default:break;}Ȑ(Α.Ǿ);}ΒЁ();ρ(Ϊ.ΒЌ);Ž(Ȇ,false);щ=Μ.ǆ;}void Ν(Α ΓЉ){if(ΓЉ==Α.й) Ƨ=Α.й;if(Ƨ!=Α.й&&Τ==Ƨ&&ΓЉ!=Ƨ){χ();return;}Ѐ=true;Τ=ΓЉ;}void Ȑ(Α Ц){this.Ƨ=Ц;}void ΘΓ(){if(Τ==Α.ǐ){if(Ѐ){ϋ=0;if(Ƞ!=Н) ĭ=0;Ƞ=Н;}if(ϋ==0){int Ɔ=ΗΜ(Н);if(Ɔ<0)
{if(!ƹ()){Ν(Α.ђ);return;}if(!ΑΨ||(Ɔ==-2)!=ΑΧ){ΑΨ=true;ΑΧ=Ɔ==-2;double a=(н%6)*Math.PI/3;Εο=!ΑΧ?ά:Ε.Δ-Ќ*ή*2.5f+
(Ε.υ*(float) Math.Cos(a)+ǰ*(float) Math.Sin(a))*ή*1.2f;}Е(Εο,10);Λ=ΑΧ?"Waiting for a free hole":"Waiting for the controller";return;}if(ΑΨ) Λ="";ΑΨ=false;if(Ɔ!=Н){Н=Ɔ;ĭ=0;Ƞ=Н;}Ł ί=ΗЀ(Н);if(ί==Ł.Δν){ΰ=Μ.Βέ;Λ="Job done";if(Đ&&Η.ε){Ν(Α.ǹ);Ȑ(Α.Ǿ);щ=Μ.ǆ;}
else{Ν(Α.Ļ);Ȑ(Α.ȫ);щ=Μ.Ǉ;}return;}if(ί==Ł.Δώ){ϋ=1;ѓ(π,true);Ј=Ȏ(Ы,Ь,true);float h=Vector3.Dot(ά-Ј,Ќ);if((ά-Ј-Ќ*h).Length()>Math.Max(ľ(),Ľ())*1.5f) h=Math.Min(h,-ή);Е(h<-1?Ј+Ќ*h:Ј,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}}else{if(Є>=0&&(ΕΑ()?Ǭ>=0&&Ǭ!=Є:Βξ(Є,true))){ϋ=0;return;}
if(В<wpReachedDist){if(Vector3.Distance(ν,Ј)>1){Е(Ј,10);return;}Ν(Α.Ě);return;}if(ȉ<0.3f&&р<0&&!Č){if(++ϋ<30) return;ϋ=1;Vector3 ΑЅ=Ј-ά;float ȓ=Vector3.Dot(ΑЅ,Ќ);if(ȓ>0&&(ΑЅ-Ќ*ȓ).Length()<wpReachedDist){ΑΤ=ά;Ν(Α.Ě);}else Е(Ј+Ќ*(Math.Min(0,-ȓ)-ή),10);
return;}ϋ=1;}}if(Τ==Α.Ě){if(Ѐ){ѓ(π,true);ǧ(false);Ј=Ȏ(Ы,Ь,false);Е(Βη(Ј,0),0);Ч(Ε.γ,Ε.δ,Ε.υ,false);ϋ=1;м=0;а=0;ΑΡ=0;Γψ=-1;į=Щ;Γσ=true;}if(!ƹ()){Ν(Α.ђ);return;}if(ŷ(true)){Ζγ=ȍ("","ORE",Л.ē);if((П==Ё.Βή||П==Ё.ƺ||
П==Ё.Βο||П==Ё.ǁ)&&Ι!=Ζ.Ǒ)
Ν(Α.ė);else if((П==Ё.Δχ||П==Ё.ΒΨ)&&Ι!=Ζ.Ǒ)
Ν(Α.ΒА);else
Ν(Α.ђ);return;}У=Vector3.Distance(ά,Ј);if(У>ĭ){ĭ=У;Γσ=false;}if(Ι==Ζ.Ǒ&&ΔЃ()==MyDetectedEntityType.SmallGrid)
а+=2;else а-=2;а=Math.Max(100,Math.Min(400,а));if(ϋ>0&&ϋ<а){if(У>м){if(а>150) м=У;else м=(float) Math.Ceiling(У);ϋ=1;}else ϋ++;}else{if(ϋ>0){Λ="Ship stuck! Retrying";м=У;ϋ=0;ĝ(false,true,0,ή*sensorRange);}Е(Βη(Ј,Math.Max(0,м-ή)),Βκ(false));if(В<=wpReachedDist/2){ϋ=1;м=0;}
return;}ĝ(false,true,ή*sensorRange,0);Vector3 ΘΝ=Ј+Ќ*У;bool ΖΘ=false;if(Vector3.Distance(ΘΝ,ά)>0.3f){Vector3 ΘΪ=Ј+Ќ*(У+0.1f);Е(ΘΪ,4);ΖΘ=true;}else{float ǟ=Βκ(true);Vector3 Εφ=Βη(Ј,Math.Max(Щ+1,У+1));Ǧ(true,false,false,Εφ,Εφ-Ј,ǟ,ǟ);}bool Γέ=false;if(ч==ѐ.ΒΤ||ч==ѐ.ΒΣ)
{if(!ΖΘ){float ΓЋ=0;foreach(IMyTerminalBlock ǳ in π)
ΓЋ+=ΗΤ(ǳ,"","",ч==ѐ.ΒΣ?new string[]{"STONE"}:null);if(ΓЋ>Γψ||У<Щ||Γσ){ΓΖ=0;ΑΡ=У;į=(int)(Math.Max(į,ΑΡ)+ή/2);}else{Γέ=У-ΑΡ>2&&ΓΖ>=20;ΓΖ++;}Γψ=ΓЋ;}}else Γέ=У>=į;if(Ƞ!=Н){Ν(Α.ǥ);У=0;return;}if(Γέ){Н++;Ν(Α.ǥ);У=0;return;}}if(Τ==Α.ǥ||Τ==Α.ђ||Τ==Α.ė)
{if(Ѐ){Ј=Ȏ(Ы,Ь,false);Vector3 ȣ=ά-Ј;if(Τ==Α.ђ&&Η.ε&&
(ȣ-Ќ*Vector3.Dot(ȣ,Ќ)).Length()>Math.Max(ľ(),Ľ())){Ν(Α.ǹ);return;}Ч(Ε.γ,Ε.δ,Ε.υ,false);ѓ(π,ō);ǧ(false);м=Vector3.Distance(ά,Ј);ĝ(false,true,0,ή*sensorRange);}if(Ѐ) Ƒ=false;Е(Ƒ?Ј-Ќ*ή:Ј,Βκ(false));if(!Ƒ&&Vector3.Distance(ά,Ј)>=м+5){ѓ(π,false);ǧ(true);Λ="Can´t return!";}
if(В<wpReachedDist){if(Τ==Α.ђ&&!Ƒ){Ƒ=true;return;}if(Τ==Α.ǥ&&ΑЋ) Ν(Α.Ļ);if(Τ==Α.ǥ) Ν(Α.ǐ);if(Τ==Α.ė) Ν(Α.ƽ);if(Τ==Α.ђ){if(Η.ε) Ν(Α.ǹ);else{χ();Ę();Λ="Can´t return, no dock found";}}return;}}if(Τ==Α.Ђ||Τ==Α.Ш){if(Τ==Α.Ш&&ΰ==Μ.І&&Ι!=Ζ.Ϋ){if(!ƹ()||ŷ(true)){Ν(Α.Ђ);return;}
}bool ΔΓ=false;bool ΒΙ=false;bool Εγ=false;float ņ=0;bool Ζβ=false;Ψ Ω=null;if(Ѐ){if(Τ==Α.Ђ||Ι==Ζ.Ϋ){if(Ι!=Ζ.Ϋ){if(ȑ(1)) ΒΟ();else{њ=null;ȇ=0;ƍ=!ΒΧ(1);}}Ψ έ=и();η=new ΒЀ();η.Ѝ=έ;η.Ƌ=followPathDock*ή;η.Δΰ=useDockDirectionDist*ή;η.ņ=10;η.Ũ.Add(έ.Δ);Vector3 Ņ=new Vector3();
if(Ǻ(έ,dockDist*ή,true,out Ņ))
η.Ũ.Add(Ņ);else η.Ƌ*=1.5f;if(Ι==Ζ.Ϋ){if(έ==Η) η.Αζ=Ε.Δ;if(έ==Ε) η.Αζ=Η.Δ;η.Γζ=dockDist*ή*1.1f;}}else if(Τ==Α.Ш){η=new ΒЀ();η.Ѝ=Ε;η.Ƌ=followPathJob*ή;η.Δΰ=useJobDirectionDist*ή;η.ņ=10;η.Αζ=Η.Δ;η.Γζ=dockDist*ή*1.1f;η.Ũ.Add(Ε.Δ);if(ΰ==Μ.в){if(!Η.ε||Σ.Count==0){χ();return;}
float Θα=Vector3.Distance(Σ.First().Δ,Η.Δ);float Θβ=Vector3.Distance(Σ.Last().Δ,Η.Δ);if(Θα<Θβ) η.Ѝ=Σ.Last();else η.Ѝ=Σ.First();}}Ɵ=new Vector3();Ζβ=!Ηθ(ά);ѓ(π,false);ǧ(true);Х=-1;double ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αζ)<=η.Γζ) continue;
double Ѓ=Vector3.Distance(Σ[i].Δ,ά);if(ǫ==-1||Ѓ<ǫ){Х=i;ǫ=Ѓ;}}Ǳ=Ηί(η.Ѝ.Δ,Х);ģ=null;}if(Τ==Α.Ђ&&Ι!=Ζ.Ϋ&&!Ѐ&&!ž()){Ψ ΓΚ=и();ƍ=!ΒΧ(1);if(и()!=ΓΚ){Ν(Α.Ђ);return;}}if(Τ==Α.Ђ&&Ι!=Ζ.Ϋ&&!Ѐ&&ȑ(1)&&ΒΟ()){Ν(Α.Ђ);return;}bool ΖΖ=false;if(Τ==Α.Ђ&&!Ѐ&&Х>=0&&Х<Σ.Count)
{Vector3 ƈ=Σ[Х].Δ+ΔЄ(Х,Ǳ);int ΓЊ=Х+Ǳ;Vector3 Ŝ=Ǳ!=0&&ΓЊ>=0&&ΓЊ<Σ.Count?Σ[ΓЊ].Δ:η.Ѝ.Δ;if(Ηβ(ƈ,Ŝ)){if(!Αΰ){Αΰ=true;Εϋ=ļ(ƈ)?ά:ƈ;}Е(Εϋ,0);Ч(η.Ѝ.γ,η.Ѝ.δ,η.Ѝ.υ,false);Λ="Waiting for dock clearance";return;}Αΰ=false;ΖΖ=ȑ(1)&&(ļ(ά)||ļ(Ŝ));}ΗΕ(Σ,Ǳ,η.Ũ,η.Ƌ,Ѐ,ref ϋ);
for(int i=0;i<η.Ũ.Count;i++){float Ѓ=Vector3.Distance(ά,η.Ũ[i]);if(Ѓ<=η.Ƌ) ΔΓ=true;if(Ѓ<=η.Δΰ) ΒΙ=true;}if(ΖΖ) ΒΙ=true;if(ΒΙ) ņ=η.ņ;float Ιφ=ģ!=null?ģ.Ћ:ȉ;float Ιι=(float) Math.Max(ȉ*0.1f*ή,wpReachedDist);if((В<Ιι)||Ѐ){if(!Ѐ) Х+=Ǳ;if(Ǳ==0||Х>Σ.Count-1||Х<0)
ΔΓ=true;else{Δε=Ǳ>0?Σ.Count-1-Х:Х;Ω=Σ[Х];ģ=Ω;if(Х>=1&&Х<Σ.Count-1)
Ɵ=Ω.Δ-Σ[Х-Ǳ].Δ;else ģ=null;ν=Ω.Δ+ΔЄ(Х,Ǳ);Εγ=true;}}if(ΒΙ)
Ч(η.Ѝ.γ,η.Ѝ.δ,η.Ѝ.υ,false);else if(Ζβ)
ΒΞ(η.Ѝ.γ,10,true);else if(Εγ&&Ω!=null){if(Ǳ>0) Ч(Ω.γ,Ω.δ,Ω.υ,90,false);else Ч(Ω.γ,-Ω.δ,-Ω.υ,90,false);}Ǧ(true,false,true,ν,Ɵ,ģ==null?0:ģ.Ћ,ņ);if(ΔΓ){Δε=0;if(Τ==Α.Ђ&&!ΗΚ()){Е(ν,0);return;}if(Τ==Α.Ђ&&ƍ){Е(ν,0);Λ="Waiting for a free dock";return;}if(Τ==Α.Ђ||Ι==Ζ.Ϋ){Ν(Α.Ǐ);return;}
if(Τ==Α.Ш&&ΑЋ){Ν(Α.Ļ);return;}if(Τ==Α.Ш){Ν(Α.ǐ);return;}}}if(Τ==Α.Ǐ||Τ==Α.Ż){if(Ι!=Ζ.Ϋ&&ž()){if(!ȑ(1)){Ν(Α.Ђ);return;}}else if(Ι!=Ζ.Ϋ){Ψ ΓΚ=и();if(!ΒΧ(2)||и()!=ΓΚ){Ν(Α.Ђ);return;}}Ψ έ=и();if(Ѐ){if(!Ǻ(έ,dockDist*ή,true,out ν)){Ǽ(г.ű);χ();return;}
Е(ν,0);ΒΞ(έ.γ,90,true);}if(В<followPathDock*ή&&В!=-1){Е(ν,10);Ч(έ.γ,έ.δ,έ.υ,false);}if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null){Ν(Α.Ė);return;}if(ŋ.LengthSquared()>0&&В<wpReachedDist&&В!=-1){ŋ=new Vector3();return;}
if(В<wpReachedDist/2&&В!=-1){Ν(Α.Ē);return;}}if(Τ==Α.Ē||Τ==Α.ȫ){if(Ѐ){if(Τ==Α.Ē){Ψ έ=и();if(!Ǻ(έ,dockDist*ή,true,out ν)){Ǽ(г.ű);χ();return;}Ǧ(true,true,false,ν,0);Ч(έ.γ,έ.δ,έ.υ,10,false);}if(Τ==Α.ȫ){Ч(Ε.γ,Ε.δ,Ε.υ,0.5f,false);ν=Ε.Δ;Ǧ(true,true,false,ν,0);
}}if(Απ){ΑΓ(false,0,0,0,0);if(Τ==Α.Ē) Ν(Α.Ė);if(Τ==Α.ȫ) χ();return;}}if(Τ==Α.Ė){if(Т(MyShipConnectorStatus.Connected)!=null){if(Ι==Ζ.Ϋ) Ν(Α.ȯ);else Ν(Α.Ǿ);return;}Ψ έ=и();if(Ѐ){а=0;Θγ=DateTime.Now;ϋ=0;Ч(έ.γ,έ.δ,έ.υ,false);}Vector3I Γλ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);
IMySlimBlock Εξ=Me.CubeGrid.GetCubeBlock(Γλ);float Εύ=dockingSpeed;float Θύ=dockingSpeed*5;float Εί=Math.Max(1.5f,Math.Min(5f,ή*0.15f));if(!Ǻ(έ,0,true,out ν)||!Ǻ(έ,Εί,true,out Ɵ)||
Εξ==null||!Εξ.FatBlock.IsFunctional){Ǽ(г.ű);χ();return;}if(а==1||(Vector3.Distance(ά,ν)<=Εί*1.1f&&!Ѐ))
а=1;else{Vector3 ΙΩ=б(Υ,Ɵ-ά);Vector3 Γκ=б(Υ,Υ.GetNaturalGravity());float ΙΒ=Ʒ(ΙΩ,Γκ,null);Εύ=Math.Min(Θύ,ΙΒ);}Ǧ(true,false,false,ν,ν-ά,dockingSpeed,Εύ);if(Ѐ) м=(float) В;IMyShipConnector ΓΨ=Т(MyShipConnectorStatus.Connectable);if(ΓΨ!=null){Ǧ(false,false,false,ν,0);
if(ϋ>0) ϋ=0;ϋ--;if(ϋ<-5){ΓΨ.Connect();if(ΓΨ.Status==MyShipConnectorStatus.Connected){if(Ι==Ζ.Ϋ) Ν(Α.ȯ);else Ν(Α.Ǿ);ΒЍ();Ž(Ȇ,true);return;}}}else{float Εϊ=(float) Math.Round(В,1);if(Εϊ<м){ϋ=-1;м=Εϊ;}else ϋ++;if(ϋ>20){Ν(Α.Ż);return;}}}if(Τ==Α.Ǿ||Τ==Α.ȯ||
Τ==Α.ΑΚ||Τ==Α.Ƽ||Τ==Α.ĕ){bool ΓΔ=false,ΓΕ=false;if(Ι==Ζ.Ϋ){if(и()==Η) ΓΔ=true;else if(и()==Ε) ΓΕ=true;}if(Ѐ){ƥ=false;if(Т(MyShipConnectorStatus.Connected)==null){Ν(Α.ǔ);return;}ΒЍ();if(ΓΔ) ΑΗ(ό.ǘ);if(ΓΕ) ΑΗ(ύ.ǘ);ĳ=false;Ȕ=false;ț=false;Ʈ=false;}if(Т(MyShipConnectorStatus.Connected)==null){χ();Ǽ(г.ΔЎ);return;}
if(ΰ!=Μ.І||ъ==-1||ǩ==у.None) Ȕ=true;else if(ǿ>=100f) Ȕ=true;else if(ǿ<=99f) Ȕ=false;if(ΰ!=Μ.І||ы==-1||ї.Count==0) ț=true;else if(Ȝ>=100f) ț=true;else if(Ȝ<=99) ț=false;if(ΰ!=Μ.І||Ю==-1||č.Count==0) Ʈ=true;else Ʈ=Ĵ>=Ю;ł т=null;if(ΓΔ) т=ό;
if(ΓΕ) т=ύ;if(т!=null&&(т.Џ==ι.ǋ||т.Џ==ι.Ǌ)) Ȕ=true;if(т!=null&&(т.Џ==ι.Βχ)) if(!ĳ) Ȕ=false;if(т!=null&&(т.Џ==ι.Βϋ||т.Џ==ι.Βω)) ț=true;if(т!=null&&(т.Џ==ι.Βϊ)) if(!ĳ) ț=false;if(ĉ){ChargeMode Εν=Ȕ?ChargeMode.Auto:ChargeMode.Recharge;if(т!=null&&(т.Џ==ι.Ǌ||т.Џ==ι.ǋ))
Εν=ChargeMode.Discharge;ΒЄ(Εν);ΒІ(!ț);}if(!ĳ){if(Ι==Ζ.Ϋ)
ĳ=ΰ!=Μ.І||ΗΛ(Ѐ,true)||Ƭ;else
ĳ=ΰ!=Μ.І||ΔЏ();}else{if(!Ȕ) Ν(Α.ĕ);if(!ț) Ν(Α.Ƽ);if(!Ʈ) Ν(Α.ΑΚ);Ѐ=false;}if(ĳ&&Ȕ&&ț&&Ʈ){ΒЄ(ChargeMode.Auto);ΒІ(false);if(ΰ==Μ.І){if(Ι==Ζ.Ϋ){if(и()==Η) ΑΗ(ό.Ǜ);else if(и()==Ε) ΑΗ(ύ.Ǜ);if(и()==Η) ŀ(Ε,Μ.ĸ);else ŀ(Η,Μ.ķ);}}Ν(Α.ǔ);return;}}if(Τ==Α.ǔ){if(Ʊ){if(!Δτ()) return;
Ʊ=false;Ν(Α.ǔ);return;}if(Ѐ){if(!Δτ()){Ʊ=true;return;}IMyShipConnector Ȃ=Т(MyShipConnectorStatus.Connected);if(Ȃ==null){Ν(Α.Ш);return;}IMyShipConnector α=Ȃ.OtherConnector;ΕΝ(Ȃ,false);Ž(Ȇ,false);Ψ έ=null;Ψ ΖΓ=Ι==Ζ.Ϋ?Η:и();if(Vector3.Distance(Ȃ.GetPosition(),ΖΓ.Δ)<5f&&Η.ε) έ=ΖΓ;
if(Vector3.Distance(Ȃ.GetPosition(),Ε.Δ)<5f&&Ε.ε) έ=Ε;if(έ!=null){if(!Ǻ(έ,dockDist*ή,true,out ν)){Ǽ(г.ű);χ();return;}Е(ν,5);Ч(έ.γ,έ.δ,έ.υ,false);}else Е(ά+α.WorldMatrix.Forward*dockDist*ή,5);if(ΰ==Μ.І) Ǽ(г.ΑΒ);}if(В<wpReachedDist){ѓ(ц,true);Ν(Α.Ш);return;}}if(Τ==Α.Ļ)
{if(Ѐ){ǧ(true);ѓ(π,false);ν=Ε.Δ;Е(ν,20);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(В<wpReachedDist/2){Ν(Α.ȫ);return;}}if(Τ==Α.ǹ){if(Ѐ){ǧ(true);ѓ(π,false);int А=-1;double ǫ=-1;bool Ζρ=ǈ();for(int ƞ=0;ƞ<2&&А==-1;ƞ++,Ζρ=false)
for(int i=Σ.Count-1;i>=0;i--){if(Ζρ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)<ΕΒ()+ή) continue;double Ѓ=Vector3.Distance(Σ[i].Δ,ά);if(ǫ==-1||Ѓ<ǫ){А=i;ǫ=Ѓ;}}if(А==-1){Ν(Α.Ђ);return;}ν=Σ[А].Δ;Е(ν,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(В<wpReachedDist){Ν(Α.Ђ);return;}}if(Τ==Α.ƽ)
{bool і=false;if(Ѐ){ǧ(true);if((П==Ё.Βή||П==Ё.ƺ)&&ǃ()&&
ĺ(Ќ,Υ.GetNaturalGravity())<25&&Р>=2&&Э>=2){Vector3 Ə=ά;if(Ы>0&&Ь<Э-1) Ə=Ȏ(Ы-1,Ь+1,true);else if(Ы<Р-1&&Ь<Э-1) Ə=Ȏ(Ы+1,Ь+1,true);else if(Ы<Р-1&&Ь>0) Ə=Ȏ(Ы+1,Ь-1,true);else if(Ы>0&&Ь>0) Ə=Ȏ(Ы-1,Ь-1,true);else і=true;if(!і) Е(Ə,10);
}else і=true;}if(В<wpReachedDist/2) і=true;if(і){Ν(Α.ΑΙ);return;}}if(Τ==Α.ΒА||Τ==Α.ΑΙ){if(Ѐ){Ǧ(true,true,false,ά,0);ѓ(π,false);ǧ(true);ϋ=-1;а=П==Ё.Βο||П==Ё.ǁ?0:-1;}bool ĥ=!ƹ();int ΒΒ=ȍ("STONE","ORE",Л.ē);if(П==Ё.ΒΨ||П==Ё.ǁ||П==Ё.ƺ)
ΒΒ+=ȍ("ICE","ORE",Л.ē);bool Θμ=ΒΒ>0;bool Γε=false;if(а>=0){float Ń=(float) Math.Sin(Δω(а))*ǵ/3f;float ΘΟ=(float) Math.Cos(Δω(а))*Ǵ/3f;Vector3 ΘΥ=Ȏ(Ы,Ь,true)+ΒЎ(Ќ,ǰ*-1,new Vector3(Ń,ΘΟ,0));Е(ΘΥ,0.3f);if(В<Math.Min(ǵ,Ǵ)/10f) а+=5f;if(а>=360) а=0;}if(ϋ==-1||ΒΒ<ϋ){ϋ=ΒΒ;м=0;}
else{м++;if(м>50) Γε=true;}if(!Θμ||ĥ||Γε){if(!ĥ){int Ιή=ȍ("","ORE",Л.ē);if(ŷ(true)) ĥ=true;else if(100-(Д(Ιή,Ζγ)*100)<minEjection) ĥ=true;else Ǽ(г.ΑΒ);}if(Γε&&ĥ) Λ="Ejection failed";if(Τ==Α.ΑΙ){if(ĥ){if(Η.ε) Ν(Α.ǹ);else{χ();Ę();Λ="Can´t return, no dock found";}}
else Ν(Α.ǐ);}else if(ĥ) Ν(Α.ђ);else Ν(Α.Ě);return;}}Ѐ=false;}void ΗΕ(List<Ψ>đ,int ζ,List<Vector3>Ʃ,float ю,bool Ȉ,ref int ǖ){if(Ȉ){for(int i=0;i<đ.Count;i++) đ[i].Ћ=0;ǖ=-1;return;}if(ζ==0) return;int ѝ=ζ*-1;if(ǖ==-1) ǖ=ѝ>0?1:đ.Count-2;int ΖΚ=0;while(ǖ>=1&&ǖ<đ.Count-1)
{if(ΖΚ>50) return;ΖΚ++;try{if((ѝ<0&&ǖ>=1)||(ѝ>0&&ǖ<=đ.Count-2)){Ψ θ=đ[ǖ];bool Ζί=false;for(int j=0;j<Ʃ.Count;j++)
if(Vector3.Distance(θ.Δ,Ʃ[j])<=ю){Ζί=true;break;}if(!Ζί){Ψ ΑЇ=đ[ǖ-ѝ];Ψ Ŝ=đ[ǖ+ѝ];Vector3 ΙΪ=θ.Δ-Ŝ.Δ;Vector3 ΔΫ=ΑЇ.Δ-θ.Δ;Vector3 ΘЎ=θ.Δ+Vector3.Normalize(ΙΪ)*ΔΫ.Length();Vector3 Θί=ΑЇ.Δ-ΘЎ;Vector3 Αχ=Ĝ(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,Θί);Vector3 Θό=Ĝ(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ΔΫ);
Vector3 Αω=Ĝ(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,θ.ξ);θ.Ћ=(float) Math.Sqrt(Math.Pow(ΑЇ.Ћ,2)+Math.Pow(Ʒ(-Θό,Αω,θ),2));for(int j=0;j<Ʃ.Count;j++)
if(Vector3.Distance(ΑЇ.Δ,Ʃ[j])<=ю){Vector3 Αϊ=Ĝ(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,Ʃ[j]-θ.Δ);float ΙΡ=Ʒ(-Αϊ,Αω,θ);θ.Ћ=Math.Min(θ.Ћ,ΙΡ)/2f;}if(Αχ.Length()==0) Αχ=new Vector3(0,0,1);Vector3 Θϋ=Ĝ(θ.δ,θ.γ*-1,θ.ξ);float ΙΤ=ų(-Αχ,Θϋ,θ);float ΑΛ=Д(ΙΤ,ı);float ΔΩ=(float) Math.Sqrt(Д(Αχ.Length()*1.0f,0.5f*ΑΛ));
θ.Ћ=Math.Min(θ.Ћ,(ΔΫ.Length()/ΔΩ)*Ǩ);}}}catch{return;}ǖ+=ѝ;}ǖ=-1;}void ΒЏ(bool Ȉ){if(Ȉ){ƕ=0;return;}if(Ι==Ζ.Ϋ) return;float і=Н*Math.Max(1,Щ);if(Ƞ==Н) і+=Math.Min(Щ,У);float Ιέ=Р*Э*Math.Max(1,Щ);ƕ=Math.Max(ƕ,(float) Math.Min(і/Ιέ*100.0,100));}MyDetectedEntityType ΔЃ()
{try{if(ǻ(ο,true)&&!ο.LastDetectedEntity.IsEmpty()) return ο.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βκ(bool Ǯ){if(Ι==Ζ.Ǒ&&ΔЃ()==MyDetectedEntityType.None&&!Ŷ(ο,true))
return fastSpeed;else return Ǯ?Ů:ŭ;}void ΗЎ(){float ΖΨ=0,Ετ=0,ĩ=0,ΓЌ=0;for(int i=0;i<Ȱ.Count;i++){IMyBatteryBlock Ƅ=Ȱ[i];if(!ǻ(Ƅ,true)) continue;ΖΨ+=Ƅ.MaxStoredPower;Ετ+=Ƅ.CurrentStoredPower;ĩ+=Ƅ.CurrentInput;ΓЌ+=Ƅ.CurrentOutput;}ǿ=(float) Math.Round(Д(Ετ,ΖΨ)*100,1);
if(ĩ>=ΓЌ) ǩ=у.ĕ;else ǩ=у.Δϊ;if(ĩ==0&&ΓЌ==0||ǿ==100.0) ǩ=у.й;if(Ȱ.Count==0) ǩ=у.None;}void ΘΑ(){float ΕЃ=0;for(int i=0;i<ї.Count;i++){IMyGasTank ΖЁ=ї[i];if(!ǻ(ΖЁ,true)) continue;ΕЃ+=(float) ΖЁ.FilledRatio;}Ȝ=Д(ΕЃ,ї.Count)*100f;}void ΘΙ(){Ĵ=0;try{for(int i=0;i<č.Count;i++)
{IMyReactor ΔΔ=č[i];if(!ǻ(ΔΔ,true)) continue;List<MyInventoryItem>д=new List<MyInventoryItem>();ΔΔ.GetInventory(0).GetItems(д);float Ǖ=0;for(int j=0;j<д.Count;j++){MyInventoryItem З=д[j];if(З.Type.SubtypeId.ToUpper()=="URANIUM"&&З.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǖ+=(float) З.Amount;}if(Ǖ<Ĵ||i==0){Ĵ=Ǖ;ΓЀ=ΔΔ.CustomName;}}}catch(Exception e){ȟ=e;}}void ΗЏ(){ƙ=0;Ɖ=0;try{for(int i=0;i<Ŋ.Count;i++){IMyTerminalBlock Φ=Ŋ[i];if(!ǻ(Φ,true)) continue;Ɖ+=(float) Φ.GetInventory(0).CurrentVolume;ƙ+=(float) Φ.GetInventory(0).MaxVolume;
}Αφ=(float) Math.Min(Math.Round(Д(Ɖ,ƙ)*100,1),100.0);}catch(Exception e){ȟ=e;}}void ΘΒ(){try{ǚ.Clear();for(int i=0;i<Ŋ.Count;i++){IMyTerminalBlock Φ=Ŋ[i];if(!ǻ(Φ,true)) continue;Δμ(Φ,Л.ŵ);}if(П!=Ё.Βφ){for(int i=0;i<Ġ.Count;i++){IMyTerminalBlock Φ=Ġ[i];if(!ǻ(Φ,true)) continue;
Δμ(Φ,Л.Ηε);}}ΗІ(ǚ);}catch(Exception e){ȟ=e;}}void ΗΙ(){if(Αά){if(ΔЂ().Count>Αξ){Αά=false;if(о!=з.Ŵ){χ();if(о==з.Βγ) Ę();if(о==з.ΑΑ)
if(Η.ε) ƾ();else Ę();}Λ="Damage detected";}}}bool ƹ(){if(!Αο) return true;if(ΰ==Μ.І){if(ъ>0&&ǩ!=у.None)
if(ǿ<=ъ){Λ="Low energy! Move home";return false;}if(Ю>0&&č.Count>0)
if(Ĵ<=Ю){Λ="Low fuel: "+ΓЀ;return false;}if(ы>0&&ї.Count>0)
if(Ȝ<=ы){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΔЂ(){List<IMyTerminalBlock>Ĥ=new List<IMyTerminalBlock>();for(int i=0;i<Ɓ.Count;i++){IMyTerminalBlock Φ=Ɓ[i];if(Ŷ(Φ,false)) Ĥ.Add(Φ);}return Ĥ;}bool Ŷ(IMyTerminalBlock Φ,bool ΔΡ){return(!ǻ(Φ,ΔΡ)||!Φ.IsFunctional);
}bool ǻ(IMyTerminalBlock Φ,bool ΔΡ){if(Φ==null) return false;try{IMyCubeBlock ΕЊ=Me.CubeGrid.GetCubeBlock(Φ.Position).FatBlock;if(ΔΡ) return ΕЊ==Φ;else return ΕЊ.GetType()==Φ.GetType();}catch{return false;}}float ΗΤ(IMyTerminalBlock Φ,String я,String Ζψ,String[]ǭ){float ȃ=0;for(int i=0;i<Φ.InventoryCount;i++)
{IMyInventory œ=Φ.GetInventory(i);List<MyInventoryItem>д=new List<MyInventoryItem>();œ.GetItems(д);foreach(MyInventoryItem З in д){if(ǭ!=null&&(ǭ.Contains(З.Type.TypeId.ToUpper())||ǭ.Contains(З.Type.SubtypeId.ToUpper())))
continue;if((я==""||З.Type.TypeId.ToUpper()==я)&&(Ζψ==""||З.Type.SubtypeId.ToUpper()==Ζψ))
ȃ+=(float) З.Amount;}}return ȃ;}Ǥ ΗΪ(String ψ,String σ,Л И,bool ΘΫ){ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<ǚ.Count;i++){Ǥ З=ǚ[i];if(З.ψ.ToUpper()==ψ&&З.σ.ToUpper()==σ&&(З.Ř==И||И==Л.ē))
return З;}Ǥ ί=null;if(ΘΫ){ί=new Ǥ(ψ,σ,0,И);ǚ.Add(ί);}return ί;}int ȍ(String ψ,String σ,Л И){return ȍ(ψ,σ,И,null);}int ȍ(String ψ,String σ,Л И,String[]ǭ){int ȃ=0;ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<ǚ.Count;i++){Ǥ З=ǚ[i];if(ǭ!=null&&ǭ.Contains(З.ψ.ToUpper())) continue;
if((ψ==""||З.ψ.ToUpper()==ψ)&&(σ==""||З.σ.ToUpper()==σ)&&(З.Ř==И||И==Л.ē))
ȃ+=З.Ǖ;}return ȃ;}void Δμ(IMyTerminalBlock Φ,Л И){for(int i=0;i<Φ.InventoryCount;i++){List<MyInventoryItem>д=new List<MyInventoryItem>();Φ.GetInventory(i).GetItems(д);for(int j=0;j<д.Count;j++)
ΗΪ(д[j].Type.SubtypeId,д[j].Type.TypeId.Replace("MyObjectBuilder_",""),И,true).Ǖ+=(int) д[j].Amount;}}void ΗІ(List<Ǥ>Ф){for(int i=Ф.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ǥ a=Ф[j];Ǥ b=Ф[j+1];if(a.Ǖ<b.Ǖ) Ф.Move(j,j+1);}}bool ŷ(bool Ισ){if(Ȫ&&Ι!=Ζ.Ϋ)
if(С!=-1&&ı>=С){Λ="Ship too heavy";return true;}if(Αφ>=ǯ||ƥ){ƥ=false;Λ="Ship is full";return true;}return false;}bool ΔЏ(){String[]ǭ=null;if(!Ȩ) ǭ=new string[]{"ICE"};if(Ι==Ζ.ź) return ȍ("","ORE",Л.ŵ,ǭ)==0;if(Ι==Ζ.Ǒ) return ȍ("","COMPONENT",Л.ŵ,ǭ)==0;else return ȍ("","",Л.ŵ,ǭ)==0;
}bool Ηι(){List<IMyCockpit>ΓΧ=new List<IMyCockpit>();ω.GetBlocksOfType(ΓΧ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΧ.Count;i++)
if(ΓΧ[i].IsUnderControl) return true;return false;}bool ΗΛ(bool Ȉ,bool ΑЎ){IMyShipConnector М=Т(MyShipConnectorStatus.Connected);if(М==null) return false;if(Vector3.Distance(Η.Δ,М.GetPosition())<5)
return Δυ(ό,Ȉ,ΑЎ);if(Vector3.Distance(Ε.Δ,М.GetPosition())<5)
return Δυ(ύ,Ȉ,ΑЎ);return false;}bool Δυ(ł ȁ,bool Ȉ,bool ΑЎ){if(Ȉ) ȁ.Ηυ();ȁ.ΘΖ();bool с=false;String э="";switch(ȁ.Џ){case ι.Βψ:{э="Waiting for command";с=false;break;}case ι.ΕΘ:{э="Waiting for cargo";с=ŷ(true);break;}case ι.ΕΗ:{э="Unloading";с=ΔЏ();break;}case ι.Βύ:{с=true;break;}
case ι.Βχ:{э="Charging batteries";с=ǿ>=100f;break;}case ι.ǋ:{э="Discharging batteries";с=ǿ<=25f;break;}case ι.Ǌ:{э="Discharging batteries";с=ǿ<=0f;break;}case ι.Βϊ:{э="Filling up hydrogen";с=Ȝ>=100f;break;}case ι.Βϋ:{э="Unloading hydrogen";с=Ȝ<=25f;break;}
case ι.Βω:{э="Unloading hydrogen";с=Ȝ<=0f;break;}case ι.Βό:{bool ΖΕ=Ηι();if(!ΖΕ) ȁ.ΓБ=true;с=ȁ.ΓБ&&ΖΕ;э="Waiting for passengers";break;}}if(!с) ȁ.ΒΦ(true);if(с&&ȁ.Βμ()){с=ȁ.ΒΦ(false);э="Undocking in: "+Βε((int) Math.Max(0,ȁ.Ǘ-ȁ.ā));}if(ΑЎ) ΖЎ=э;return с;}void ΗΑ()
{if(!Ŕ) return;if(π.Count<=1) return;float ΖЇ=0,ΖІ=0;for(int i=0;i<π.Count;i++){IMyTerminalBlock ǳ=π[i];if(Ŷ(ǳ,true)) continue;ΖЇ+=(float) ǳ.GetInventory(0).MaxVolume;ΖІ+=(float) ǳ.GetInventory(0).CurrentVolume;}float Εδ=(float) Math.Round(Д(ΖІ,ΖЇ),5);for(int ƞ=0;ƞ<Math.Max(1,Math.Floor(π.Count/10f));ƞ++)
{float Γπ=0,ΓϏ=0;float ΖΑ=0,ΖΦ=0;IMyTerminalBlock ș=null,Ă=null;for(int i=0;i<π.Count;i++){IMyTerminalBlock ǳ=π[i];if(Ŷ(ǳ,true)) continue;float ΓІ=(float) ǳ.GetInventory(0).MaxVolume;float Αη=Д((float) ǳ.GetInventory(0).CurrentVolume,ΓІ);if(ș==null||Αη>Γπ){ș=ǳ;Γπ=Αη;ΖΑ=ΓІ;}
if(Ă==null||Αη<ΓϏ){Ă=ǳ;ΓϏ=Αη;ΖΦ=ΓІ;}}if(ș==null||Ă==null||ș==Ă) return;if(checkConveyorSystem&&!ș.GetInventory(0).IsConnectedTo(Ă.GetInventory(0))){if(ΓΙ>20) Λ="Inventory balancing failed";else ΓΙ++;return;}ΓΙ=0;List<MyInventoryItem>д=new List<MyInventoryItem>();
ș.GetInventory(0).GetItems(д);float ư=0;if(д.Count==0) continue;MyInventoryItem Αθ=д[0];String ΖΙ=Αθ.Type.TypeId+Αθ.Type.SubtypeId;if(!ΖЍ.TryGetValue(ΖΙ,out ư)){if(Ηξ(ș.GetInventory(0),0,Ă.GetInventory(0),out ư))
ΖЍ.Add(ΖΙ,ư);else return;}float ΙΫ=((Γπ-Εδ)*ΖΑ/ư);float Ιά=((Εδ-ΓϏ)*ΖΦ/ư);int Δή=(int) Math.Min(Ιά,ΙΫ);if(Δή<=0) return;if((float) Αθ.Amount<Δή)
ș.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,Αθ.Amount);else
ș.GetInventory(0).TransferItemTo(Ă.GetInventory(0),0,null,null,Δή);}}bool Ηξ(IMyInventory Ʀ,int Β,IMyInventory Θή,out float ΖЌ){ΖЌ=0;float ΘΣ=(float) Ʀ.CurrentVolume;List<MyInventoryItem>ΓΛ=new List<MyInventoryItem>();Ʀ.GetItems(ΓΛ);float Εη=0;for(int i=0;i<ΓΛ.Count;i++) Εη+=(float) ΓΛ[i].Amount;
Ʀ.TransferItemTo(Θή,Β,null,null,1);float ΖЋ=ΘΣ-(float) Ʀ.CurrentVolume;List<MyInventoryItem>ΓΑ=new List<MyInventoryItem>();Ʀ.GetItems(ΓΑ);float ΕΫ=0;for(int i=0;i<ΓΑ.Count;i++) ΕΫ+=(float) ΓΑ[i].Amount;if(ΖЋ==0f||!Βπ(0.9999,Εη-ΕΫ,1.0001))
return false;ΖЌ=ΖЋ;return true;}void ΗБ(){Vector3 Αι=ν+ȧ+ŋ;Vector3 ǲ=Αι-ά;if(ǲ.Length()==0) ǲ=new Vector3(0,0,-1);Vector3 Αϊ=б(Υ,ǲ);Vector3 ΖЂ=Vector3.Normalize(Αϊ);Vector3 ŗ=б(Υ,Υ.GetNaturalGravity());float ΙΜ=ΓЏ>0?Math.Max(0,1-ĺ(ǲ,ΕЅ)/5):0;float Ћ=(float) Math.Min((ΓЃ>0?ΓЃ:1000f),Math.Max(Ʒ(-Αϊ,ŗ,null),ΓЏ*ΙΜ));
if(!ΖЅ) Ћ=0;if(Ζς) Ћ=Math.Max(0,1-ΑΪ/ΑΜ)*Ћ;if(generalSpeedLimit>0) Ћ=Math.Min(generalSpeedLimit,Ћ);if(Č) Ћ=0;if(р>=0) Ћ=Math.Min(Ћ,р);if(ǈ()) Ћ=Math.Min(Ћ,jobAreaSpeed);if(Ζσ) Ћ*=(float) Math.Min(1,Д(ǲ.Length(),wpReachedDist)/2);Vector3 ΖΤ=б(Υ,Υ.GetShipVelocities().LinearVelocity);
float ΘΞ=(float)(Math.Max(0,15-ĺ(-ΖЂ,-ΖΤ))/15)*0.85f+0.15f;ΔΦ+=Math.Sign(ΘΞ-ΔΦ)/10f;Vector3 ȱ=ΖЂ*Ћ*ΔΦ-(ΖΤ);Vector3 ΓΘ=Βι(ȱ,null);if(ΒΔ&&В>0.1f){ȱ.X*=ΒЈ(ȱ.X,ref ΒΕ.X,1f,ΓΘ.X,20);ȱ.Y*=ΒЈ(ȱ.Y,ref ΒΕ.Y,1f,ΓΘ.Y,20);ȱ.Z*=ΒЈ(ȱ.Z,ref ΒΕ.Z,1f,ΓΘ.Z,20);}else ΒΕ=new Vector3(1,1,1);
ƪ=ı*ȱ-ŗ*ı;Δξ(ƪ,ΒΔ);В=Vector3.Distance(ά,Αι);}float ΒЈ(float value,ref float Ц,float ѝ,float ȡ,float ΓЄ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ц)) Ц+=Math.Sign(Ц)*ѝ;else Ц=value;if(value==0) Ц=1;float ί=Math.Abs(Ц);if(ί<ΓЄ||ȡ==0) return ί;Ц=Math.Min(ΓЄ,Math.Max(-ΓЄ,Ц));
ί=Math.Abs(ȡ);return ί;}void ΕΩ(){float ь=90,ǎ=90,ќ=90;float ǟ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǯ,љ,Ƙ,Γό;if(Γύ){Ǯ=Vector3.Normalize(ν-ά);Ƙ=б(Υ,Ǯ);љ=б(Υ,ΔΤ);ь=ĺ(Ƙ,new Vector3(0,-1,0))-90;ǎ=Ĕ(љ,new Vector3(-1,0,0),љ.Y);
ќ=Ĕ(Ƙ,new Vector3(-1,0,0),Ƙ.Z);}else{Ǯ=ΔΥ;љ=б(Υ,ΔΤ);Ƙ=б(Υ,ΔΥ);Γό=б(Υ,ΖЃ);ь=Ĕ(љ,new Vector3(0,0,1),љ.Y);ǎ=Ĕ(љ,new Vector3(-1,0,0),љ.Y);ќ=Ĕ(Γό,new Vector3(0,0,1),Γό.X);}if(Εέ&&ǃ()){Vector3 Γκ=Υ.GetNaturalGravity();љ=б(Υ,Γκ);ь=Ĕ(љ,new Vector3(0,0,1),љ.Y);
ǎ=Ĕ(љ,new Vector3(-1,0,0),љ.Y);}if(!Βπ(-45,ǎ,45)){ь=0;ќ=0;}if(!Βπ(-45,ќ,45)) ь=0;ΑΓ(Γμ,1,(-ь)*ǟ,(-ќ)*ǟ,(-ǎ)*ǟ);ΑΪ=Math.Max(Math.Abs(ь),Math.Max(Math.Abs(ǎ),Math.Abs(ќ)));Απ=ΑΪ<=ΑΜ;}void ΗΖ(IMyTerminalBlock ΑЈ){if(ΑЈ==null) return;Ǣ=new float[3,2];ў=new Dictionary<string,float[,]>();
for(int i=0;i<ȋ.Count;i++){IMyThrust е=ȋ[i];if(!е.IsFunctional) continue;Vector3 ζ=б(ΑЈ,е.WorldMatrix.Backward);float Ā=е.MaxEffectiveThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) Ǣ[0,0]+=Ā;else Ǣ[0,1]-=Ā;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) Ǣ[1,0]+=Ā;else Ǣ[1,1]-=Ā;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) Ǣ[2,0]+=Ā;else Ǣ[2,1]-=Ā;String я=ΔІ(е);float[,]џ=null;if(ў.ContainsKey(я)) џ=ў[я];else{џ=new float[3,2];ў.Add(я,џ);}float ȡ=е.MaxThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) џ[0,0]+=ȡ;else џ[0,1]-=ȡ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) џ[1,0]+=ȡ;else џ[1,1]-=ȡ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) џ[2,0]+=ȡ;else џ[2,1]-=ȡ;}}static String ΔІ(IMyThrust е){return е.BlockDefinition.SubtypeId;}Vector3 Βς(Vector3 ѕ,float[,]ř){return new Vector3(
ѕ.X>=0?ř[0,0]:ř[0,1],
ѕ.Y>=0?ř[1,0]:ř[1,1],
ѕ.Z>=0?ř[2,0]:ř[2,1]);}bool ΔЅ(Ψ Ω,String я,out float Γδ){Γδ=0;int Β=Ȋ.IndexOf(я);if(Β==-1||Ω.ǡ==null||Β>=Ω.ǡ.Length)
return false;Γδ=Ω.ǡ[Β];if(Γδ==-1) return false;return true;}Vector3 Βι(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 ί=new Vector3();for(int i=0;i<ў.Keys.Count;i++){String я=ў.Keys.ElementAt(i);float Ɛ=0;if(!ΔЅ(Ω,я,out Ɛ))
return Βς(ѕ,Ǣ);ί+=Βς(ѕ,ў.Values.ElementAt(i))*Ɛ;}return ί;}return Βς(ѕ,Ǣ);}float ų(Vector3 ѕ,Ψ Ω){return ų(ѕ,new Vector3(),Ω);}float ų(Vector3 ѕ,Vector3 ξ,Ψ Ω){Vector3 ΙΣ=Βι(ѕ,Ω);Vector3 Ā=ΙΣ+ξ*ı;float ơ=(Ā/ѕ).AbsMin();return(float)(ѕ*ơ).Length();}float Ʒ(Vector3 Αψ,Vector3 ŗ,Ψ Ω)
{if(Αψ.Length()==0) return 0;float Εά=1;if(ŗ.Length()>0) Εά=Math.Min(1,ĺ(-ŗ,Αψ)/90)*0.4f+0.6f;float ƪ=ų(Αψ,ŗ,Ω);if(ƪ==0) return 0.1f;float ΑΛ=Д(ƪ,ı);float ΔΩ=(float) Math.Sqrt(Д(Αψ.Length(),ΑΛ*0.5f));return ΑΛ*ΔΩ*Εά*Ǩ;}float ǀ(Vector3 Ǯ,Vector3 φ,Vector3 ξ,Ψ Ω)
{if(ξ.Length()==0f) return 0;Vector3 Αω=Ĝ(Ǯ,φ,Vector3.Normalize(ξ));float Ǚ=ų(-Αω,Ω);return Ǚ/ξ.Length();}void Δρ(bool Θς){float Ǎ=0;float Γη=0.9f;if(Θς){С=-1;ĵ=0;ΑБ=null;if(ΰ!=Μ.в&&Ε.ξ.Length()!=0){Ǎ=Γη*ǀ(Ε.δ,Ε.γ*-1,Ε.ξ,null);if(Ǎ<С||С==-1) С=Ǎ;}if(Η.ε&&Η.ξ.Length()!=0)
{Ǎ=Γη*ǀ(Η.δ,Η.γ*-1,Η.ξ,null);if(Ǎ<С||С==-1) С=Ǎ;}return;}if(ĵ==-1) return;if(ĵ>=0){int Ζθ=0;while(ĵ<Σ.Count){if(Ζθ>100) return;Ζθ++;Ψ Ω=Σ[ĵ];if(Ω.ξ.Length()!=0f){Ǎ=Γη*Math.Min(
ǀ(Ω.δ,Ω.γ*-1,Ω.ξ,Ω),
ǀ(Ω.δ*-1,Ω.γ*-1,Ω.ξ,Ω));if(Ǎ<С||С==-1) С=Ǎ;}else ΑБ=Ω;ĵ++;}ĵ=-1;}bool Γν=true;float ċ=0;if(ΑБ!=null){for(int i=0;i<ў.Count;i++){String я=ў.Keys.ElementAt(i);float[,]џ=ў.Values.ElementAt(i);float Ɛ=0;if(!ΔЅ(ΑБ,я,out Ɛ)){Γν=false;break;}for(int a=0;a<џ.GetLength(0);a++)
for(int b=0;b<џ.GetLength(1);b++){float ğ=Math.Abs(џ[a,b]*Ɛ);if(ğ==0) continue;Γν=true;if(ċ==0||ğ<ċ) ċ=ğ;}}}if(!Γν){for(int a=0;a<Ǣ.GetLength(0);a++)
for(int b=0;b<Ǣ.GetLength(1);b++){float ğ=Math.Abs(Ǣ[a,b]);if(ğ==0) continue;if(ċ==0||ğ<ċ) ċ=ğ;}}if(ċ>0){float ΘϏ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǎ=Д(ċ,ΘϏ);if(Ǎ>0)
if(Ǎ<С||С==-1) С=Ǎ;}}void Δξ(Vector3 Ǚ,bool Љ){if(!Љ){for(int i=0;i<ȋ.Count;i++)
ȋ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓΗ=Βι(Ǚ,null);float Ιλ=Math.Min(1,Math.Abs(Д(Ǚ.X,ΓΗ.X)));float Ιμ=Math.Min(1,Math.Abs(Д(Ǚ.Y,ΓΗ.Y)));float Ιν=Math.Min(1,Math.Abs(Д(Ǚ.Z,ΓΗ.Z)));for(int i=0;i<ȋ.Count;i++){IMyThrust е=ȋ[i];Vector3 ζ=Ηχ(б(Υ,е.WorldMatrix.Backward),1);
if(ζ.X!=0&&Math.Sign(ζ.X)==Math.Sign(Ǚ.X))
е.SetValueFloat("Override",е.MaxThrust*Ιλ);else if(ζ.Y!=0&&Math.Sign(ζ.Y)==Math.Sign(Ǚ.Y))
е.SetValueFloat("Override",е.MaxThrust*Ιμ);else if(ζ.Z!=0&&Math.Sign(ζ.Z)==Math.Sign(Ǚ.Z))
е.SetValueFloat("Override",е.MaxThrust*Ιν);else
е.SetValueFloat("Override",0.0f);}}void ΑΓ(bool Љ,float ΘЋ,float ь,float ќ,float ǎ){for(int i=0;i<Αλ.Count;i++){IMyGyro Ї=Αλ[i];Ї.GyroOverride=Љ;if(!Љ) Ї.GyroPower=100;else Ї.GyroPower=ΘЋ;if(!Љ) continue;Vector3 Ж=Υ.WorldMatrix.Forward;Vector3 Я=Υ.WorldMatrix.Right;Vector3 φ=Υ.WorldMatrix.Up;
Vector3 ŏ=Ї.WorldMatrix.Forward;Vector3 ő=Ї.WorldMatrix.Up;Vector3 Ő=Ї.WorldMatrix.Left*-1;if(ŏ==Ж) Ї.SetValueFloat("Roll",ǎ);else if(ŏ==(Ж*-1)) Ї.SetValueFloat("Roll",ǎ*-1);else if(ő==(Ж*-1)) Ї.SetValueFloat("Yaw",ǎ);else if(ő==Ж) Ї.SetValueFloat("Yaw",ǎ*-1);else if(Ő==Ж) Ї.SetValueFloat("Pitch",ǎ);
else if(Ő==(Ж*-1)) Ї.SetValueFloat("Pitch",ǎ*-1);if(Ő==(Я*-1)) Ї.SetValueFloat("Pitch",ь);else if(Ő==Я) Ї.SetValueFloat("Pitch",ь*-1);else if(ő==Я) Ї.SetValueFloat("Yaw",ь);else if(ő==(Я*-1)) Ї.SetValueFloat("Yaw",ь*-1);else if(ŏ==(Я*-1)) Ї.SetValueFloat("Roll",ь);
else if(ŏ==Я) Ї.SetValueFloat("Roll",ь*-1);if(ő==(φ*-1)) Ї.SetValueFloat("Yaw",ќ);else if(ő==φ) Ї.SetValueFloat("Yaw",ќ*-1);else if(Ő==φ) Ї.SetValueFloat("Pitch",ќ);else if(Ő==(φ*-1)) Ї.SetValueFloat("Pitch",ќ*-1);else if(ŏ==φ) Ї.SetValueFloat("Roll",ќ);else if(ŏ==(φ*-1)) Ї.SetValueFloat("Roll",ќ*-1);
}}void ΕΣ(){this.Γμ=false;}void Ч(Vector3 ȳ,Vector3 Ж,Vector3 Γϊ,float Ȧ,bool Ư){ΒΞ(ȳ,Ȧ,Ư);ΑΜ=Ȧ;Γύ=false;this.ΔΥ=Ж;this.ΖЃ=Γϊ;}void Ч(Vector3 ȳ,Vector3 Ж,Vector3 Γϊ,bool Ư){Ч(ȳ,Ж,Γϊ,2f,Ư);}void ΒΞ(Vector3 ȳ,float Ȧ,bool Ư){ΑΜ=Ȧ;this.Γμ=true;this.Εέ=Ư;Γύ=true;Απ=false;
this.ΔΤ=ȳ;}void ΒЍ(){Ǧ(false,false,false,ν,0);ΒΔ=false;}void Е(Vector3 Ѝ,float ǟ){Ǧ(true,false,false,Ѝ,ǟ);}void Ǧ(bool Љ,bool ΔΞ,bool Δα,Vector3 Ѝ,float ǟ){Ǧ(Љ,ΔΞ,Δα,Ѝ,Ѝ-ά,0.0f,ǟ);}void Ǧ(bool Љ,bool ΔΞ,bool Δα,Vector3 Ѝ,Vector3 ΙΖ,float ΙΗ,float ǟ){ΒΔ=true;this.ΖЅ=Љ;
ν=Ѝ;this.ΓЃ=ǟ;this.ΓЏ=ΙΗ;this.Ζσ=ΔΞ;this.Ζς=Δα;this.ΕЅ=ΙΖ;В=Vector3.Distance(Ѝ,ά);}void Ηω(){ω.GetBlocksOfType(Ɓ,ǒ);}void Ηϊ(){if(ΑЊ){ώ=2;return;}List<IMyRemoteControl>Ƣ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζο=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζϊ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(Ƣ,ǒ);ω.GetBlocksOfType(ј,ǒ);ω.GetBlocksOfType(Ζο,ǒ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζϊ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ј=ǣ(ј,pamTag,true);ΗЁ();ΗЂ(Ζϊ);ż(ј,setLCDFontAndSize,1.4f,false);ż(ď,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζώ=ǣ(Ζο,pamTag,true);if(Ζώ.Count>0) ο=Ζώ[0];else ο=null;if(Ι==Ζ.ź){ω.GetBlocksOfType(π,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(π.Count==0){ώ=1;Λ="Drills are missing";}}else if(Ι==Ζ.Ǒ){ω.GetBlocksOfType(π,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(π.Count==0){ώ=1;Λ="Grinders are missing";}if(ο==null){ώ=1;Λ="Sensor is missing";}}else if(Ι==Ζ.Ϋ){ω.GetBlocksOfType(ΔΪ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζύ=ǣ(Ƣ,pamTag,true);if(Ζύ.Count>0) Ƣ=Ζύ;if(Ƣ.Count>0) Υ=Ƣ[0];else{Υ=null;ώ=2;Λ="Remote is missing";return;}ή=(float) Υ.CubeGrid.WorldVolume.Radius*2;ћ=null;if(Ι!=Ζ.Ϋ){Δς();if(π.Count>0&&ћ!=null){if(ο!=null&&(ћ.WorldMatrix.Forward!=ο.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==ο.WorldMatrix.Up||Υ.WorldMatrix.Down==ο.WorldMatrix.Down))){ώ=1;Λ="Wrong sensor direction";}if(ћ.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
ћ.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ώ=2;Λ="Wrong remote direction";}}}}void Ηϋ(){ω.GetBlocksOfType(Ȇ,ǒ);ω.GetBlocksOfType(ц,ǒ);ω.GetBlocksOfType(ȋ,ǒ);ω.GetBlocksOfType(Αλ,ǒ);ω.GetBlocksOfType(Ȱ,ǒ);ω.GetBlocksOfType(č,ǒ);ω.GetBlocksOfType(ї,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(ΑА,ǒ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ц=ǣ(ц,"ConnectorMedium",false);else
ц=ǣ(ц,"Connector",false);List<IMyShipConnector>Ζϋ=ǣ(ц,pamTag,true);if(Ζϋ.Count>0) ц=Ζϋ;if(ώ<=1){if(ц.Count==0){ώ=1;Λ="Connector is missing";}if(Αλ.Count==0){ώ=1;Λ="Gyros are missing";}if(ȋ.Count==0){ώ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΖϏ=ǣ(ΑА,pamTag,true);
if(ΖϏ.Count>0) ΑА=ΖϏ;List<IMyLandingGear>Ζό=ǣ(Ȇ,pamTag,true);if(Ζό.Count>0) Ȇ=Ζό;for(int i=0;i<Ȇ.Count;i++) Ȇ[i].AutoLock=false;List<IMyBatteryBlock>Ζω=ǣ(Ȱ,pamTag,true);if(Ζω.Count>0) Ȱ=Ζω;List<IMyGasTank>ΖЀ=ǣ(ї,pamTag,true);if(ΖЀ.Count>0) ї=ΖЀ;}void Ηό(){ω.GetBlocksOfType(Ġ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ŋ.Clear();for(int i=Ġ.Count-1;i>=0;i--){if(Ηη(Ġ[i])){Ŋ.Add(Ġ[i]);Ġ.RemoveAt(i);}}}bool Ηη(IMyTerminalBlock Φ){if(Φ is IMyCargoContainer) return true;if(Φ is IMyShipDrill) return true;if(Φ is IMyShipGrinder) return true;if(Φ is IMyShipConnector){if(((IMyShipConnector) Φ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βν(Φ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δς(){if(Υ==null) return;ћ=null;float ΑЂ=0,Αό=0,ΑЃ=0,Αύ=0,ΑЄ=0,Αώ=0;List<IMyTerminalBlock>ΔΣ=ǣ(π,pamTag,true);bool ΘЇ=ΔΣ.Count==0;if(ΔΣ.Count>0) ћ=ΔΣ[0];else if(π.Count>0) ћ=π[0];int ȃ=0;float Š=0;for(int i=0;i<π.Count;i++)
{if(π[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward){if(ΘЇ){ώ=2;Λ="Mining direction is unclear!";return;}continue;}ȃ++;Š=Math.Max(Š,ΗЌ(π[i]));Vector3 Њ=ƿ(Υ,π[i].GetPosition());if(i==0){ΑЂ=Њ.X;Αό=Њ.X;ΑЃ=Њ.Y;Αύ=Њ.Y;ΑЄ=Њ.Z;Αώ=Њ.Z;}Αό=Math.Max(Њ.X,Αό);ΑЂ=Math.Min(Њ.X,ΑЂ);
Αύ=Math.Max(Њ.Y,Αύ);ΑЃ=Math.Min(Њ.Y,ΑЃ);Αώ=Math.Max(Њ.Z,Αώ);ΑЄ=Math.Min(Њ.Z,ΑЄ);}if(Š==0) Š=drillRadius;bool ȳ=ћ!=null&&ћ.WorldMatrix.Forward==Υ.WorldMatrix.Down;ǵ=(Αό-ΑЂ)*(1-Ķ)+Š*2;Ǵ=(ȳ?Αώ-ΑЄ:Αύ-ΑЃ)*(1-ħ)+Š*2;IMyCubeGrid g=Υ.CubeGrid;float Δη=0,Δθ=0,Δι=0,Δκ=0;
for(int c=0;c<8;c++){Vector3 p=ƿ(Υ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȳ?p.Z:p.Y;if(c==0||p.X<Δη) Δη=p.X;if(c==0||p.X>Δθ) Δθ=p.X;if(c==0||y<Δι) Δι=y;if(c==0||y>Δκ) Δκ=y;}ΔΙ=Δθ-Δη+g.GridSize;ΔΘ=Δκ-Δι+g.GridSize;}float ΗЌ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǣ<T>(List<T>Ф,String Ǡ,bool Αμ){List<T>ί=new List<T>();for(int i=0;i<Ф.Count;i++)
if(Βν(Ф[i],Ǡ,Αμ)) ί.Add(Ф[i]);return ί;}bool Βν<T>(T Φ,String Ǡ,bool Αμ){IMyTerminalBlock b=(IMyTerminalBlock) Φ;if(Αμ&&b.CustomName.ToUpper().Contains(Ǡ.ToUpper())) return true;if(!Αμ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǡ.ToUpper())) return true;return false;}void ѓ<T>(List<T>Ф,bool Љ)
{for(int i=0;i<Ф.Count;i++)
ΕΝ((IMyTerminalBlock) Ф[i],Љ);}void ΕΝ(IMyTerminalBlock Φ,bool Љ){if(Φ==null) return;String ΘΜ=Љ?"OnOff_On":"OnOff_Off";var a=Φ.GetActionWithName(ΘΜ);a.Apply(Φ);}void ǧ(bool Љ){ΔΟ=Љ;if(!Ĳ) return;ѓ(ΑА,Љ);}void ΒЄ(ChargeMode ƛ){for(int i=0;i<Ȱ.Count;i++) Ȱ[i].ChargeMode=ƛ;}void ΒІ(bool ΙΞ)
{for(int i=0;i<ї.Count;i++) ї[i].Stockpile=ΙΞ;}void Ž(List<IMyLandingGear>Γθ,bool ΖΥ){for(int i=0;i<Γθ.Count;i++){if(ΖΥ) Γθ[i].Lock();if(!ΖΥ) Γθ[i].Unlock();}}void ΕΞ(bool Љ){if(Ευ==Љ) return;List<IMyShipController>ΑΦ=new List<IMyShipController>();ω.GetBlocksOfType(ΑΦ,ǒ);if(ΑΦ.Count==0) return;
for(int i=0;i<ΑΦ.Count;i++)
ΑΦ[i].DampenersOverride=Љ;Ευ=Љ;}IMyShipConnector Т(MyShipConnectorStatus э){for(int i=0;i<ц.Count;i++){if(!ǻ(ц[i],true)) continue;if(ц[i].Status==э) return ц[i];}return null;}void ĝ(bool ΔΛ,bool Αί,float Θθ,float ΘΡ){if(ο==null||π.Count==0) return;Vector3 ΑΥ=new Vector3();
int ȃ=0;for(int i=0;i<π.Count;i++){if(π[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward) continue;ȃ++;ΑΥ+=π[i].GetPosition();}ΑΥ=ΑΥ/ȃ;Vector3 ŝ=ƿ(ο,ΑΥ);ο.Enabled=true;ο.ShowOnHUD=ΔΛ;ο.LeftExtend=(Αί?1:л)/2f*ǵ-ŝ.X;ο.RightExtend=(Αί?1:л)/2f*ǵ+ŝ.X;ο.TopExtend=(Αί?1:к)/2f*Ǵ+ŝ.Y;
ο.BottomExtend=(Αί?1:к)/2f*Ǵ-ŝ.Y;ο.FrontExtend=(ΔΛ?Щ:Θθ)-ŝ.Z;ο.BackExtend=ΔΛ?0:ΘΡ+ή*0.75f+ŝ.Z;ο.DetectFloatingObjects=true;ο.DetectAsteroids=false;ο.DetectLargeShips=true;ο.DetectSmallShips=true;ο.DetectStations=true;ο.DetectOwner=true;ο.DetectSubgrids=false;
ο.DetectPlayers=false;ο.DetectEnemy=true;ο.DetectFriendly=true;ο.DetectNeutral=true;}void ż<T>(List<T>Ф,bool ΙΙ,float Θζ,bool Θσ){for(int i=0;i<Ф.Count;i++){IMyTextSurface ƨ=null;if(Ф[i]is IMyTextSurface) ƨ=(IMyTextSurface) Ф[i];if(ƨ!=null){ƨ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΙΙ) continue;ƨ.Font="Debug";if(Θσ) continue;ƨ.FontSize=Θζ;}}}bool ǃ(){Vector3D λ;return this.Υ.TryGetPlanetPosition(out λ);}int Ηί(Vector3 ΙΠ,int Ζτ){if(Ζτ==-1) return 0;double А=-1;int Ș=-1;for(int i=Σ.Count-1;i>=0;i--){double Ѓ=Vector3.Distance(Σ[i].Δ,ΙΠ);if(А==-1||Ѓ<А){Ș=i;А=Ѓ;}
}return Math.Sign(Ș-Ζτ);}bool Ηθ(Vector3 λ){List<Vector3>К=new List<Vector3>();for(int i=0;i<Σ.Count;i++) К.Add(Σ[i].Δ);if(Η.ε&&Σ.Count>=1){Vector3 Ņ=new Vector3();Ǻ(Η,dockDist*ή,false,out Ņ);if(Vector3.Distance(Η.Δ,Σ.First().Δ)<Vector3.Distance(Η.Δ,Σ.Last().Δ)){К.Insert(0,Ņ);К.Insert(0,Η.Δ);}
else{К.Add(Ņ);К.Add(Η.Δ);}}if(Ι==Ζ.Ϋ){if(Ε.ε&&Σ.Count>=1){Vector3 ΓΓ=new Vector3();Ǻ(Ε,dockDist*ή,false,out ΓΓ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){К.Insert(0,ΓΓ);К.Insert(0,Ε.Δ);}else{К.Add(ΓΓ);К.Add(Ε.Δ);}}}else{if(ΰ!=Μ.в)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
К.Insert(0,Ε.Δ);else К.Add(Ε.Δ);}int Ș=-1;double ǫ=-1;for(int i=0;i<К.Count;i++){double Ѓ=Vector3.Distance(К[i],λ);if(Ѓ<ǫ||ǫ==-1){ǫ=Ѓ;Ș=i;}}if(К.Count==0) return false;double Γή=Vector3.Distance(К[Ș],λ);double ΘЌ=Vector3.Distance(К[Math.Max(0,Ș-1)],К[Ș])*1.5f;
double ΘІ=Vector3.Distance(К[Math.Min(К.Count-1,Ș+1)],К[Ș])*1.5f;return Γή<ΘЌ||Γή<ΘІ||Γή<laneOffset*ή*1.5f;}static float Д(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηχ(Vector3 v,int ΓΫ){return new Vector3(Math.Round(v.X,ΓΫ),Math.Round(v.Y,ΓΫ),Math.Round(v.Z,ΓΫ));
}float ĺ(Vector3 a,Vector3 b){if(a==b) return 0;float Θδ=(a*b).Sum;float ΖΟ=a.Length();float ΖΠ=b.Length();if(ΖΟ==0||ΖΠ==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(Θδ/(ΖΟ*ΖΠ)));if(float.IsNaN(ί)) return 0;return ί;}float Ĕ(Vector3 v,Vector3 ΑЈ,float ΙΚ){float Ń=ĺ(v,ΑЈ);
if(ΙΚ>0f) Ń*=-1;if(Ń>-90f) return Ń-90f;else return 180f-(-Ń-90f);}double Δω(float Θά){return(Math.PI/180)*Θά;}bool Βπ(double Θώ,double value,double Αϋ){return(value>=Θώ&&value<=Αϋ);}Vector3 ΗЍ(IMyTerminalBlock Φ,Vector3 Њ){return Vector3D.Transform(Њ,Φ.WorldMatrix);
}Vector3 ƿ(IMyTerminalBlock Φ,Vector3 Ιθ){return б(Φ,Ιθ-Φ.GetPosition());}Vector3 б(IMyTerminalBlock Φ,Vector3 Δζ){return Vector3D.TransformNormal(Δζ,MatrixD.Transpose(Φ.WorldMatrix));}Vector3 Ĝ(Vector3 Ǯ,Vector3 φ,Vector3 Δζ){MatrixD ΓЂ=MatrixD.CreateFromDir(Ǯ,φ);return Vector3D.TransformNormal(Δζ,MatrixD.Transpose(ΓЂ));
}Vector3 ΒЎ(Vector3 Ǯ,Vector3 φ,Vector3 Њ){MatrixD ΓЂ=MatrixD.CreateFromDir(Ǯ,φ);return Vector3D.Transform(Њ,ΓЂ);}String κ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Й=s.Split('|');return new Vector3(float.Parse(Κ(Й,0)),float.Parse(Κ(Й,1)),float.Parse(Κ(Й,2)));
}String ΘΚ(Ψ Ω){String Ў=":";String ί=κ(Ω.Δ)+Ў+κ(Ω.δ)+Ў+
κ(Ω.γ)+Ў+κ(Ω.υ)+Ў+κ(Ω.ξ);for(int i=0;i<Ω.ǡ.Length;i++)
ί+=Ў+Math.Round(Ω.ǡ[i],3);return ί;}Ψ ΗЇ(String s){String[]Й=s.Split(':');Ψ Ω=new Ψ(
μ(Κ(Й,0)),μ(Κ(Й,1)),
μ(Κ(Й,2)),μ(Κ(Й,3)),
μ(Κ(Й,4)));int Β=5;List<float>ΕЁ=new List<float>();while(Β<Й.Length){String ΒΚ=Κ(Й,Β);float f=0;if(!float.TryParse(ΒΚ,out f)) break;ΕЁ.Add(f);Β++;}Ω.ǡ=ΕЁ.ToArray();return Ω;}System.Text.StringBuilder ţ=new System.Text.StringBuilder();void Ρ<T>(T value,bool ΘЅ){if(ΘЅ) ţ.Append('\n');ţ.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƻ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΘ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]ΘΠ,int Β){String ΒΚ=ΘΠ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΒΚ)) return "";
return ΒΚ;}void Save(){if(ΑЊ||Ι==Ζ.ф){Storage="";return;}Storage=ΒΥ();}String ΒΥ(){ţ.Clear();ţ.Append(DATAREV+";");Ρ(κ(ΑЉ),false);Ρ(κ(Η.δ));Ρ(κ(Η.υ));Ρ(κ(Η.γ));Ρ(κ(Η.ξ));Ρ(κ(Η.Δ));Ρ(κ(Η.Ъ));Ρ(Η.ε);Ρ(κ(Ε.Δ));Ρ(κ(Ε.ξ));Ρ(κ(Ε.δ));Ρ(κ(Ε.γ));Ρ(κ(Ε.υ));Ρ(κ(Ε.Ъ));Ρ(Ε.ε);Ρ(κ(Ќ));Ρ(κ(ǰ));Ρ(";");
Ρ((int) Ι,false);Ρ((int) ΰ);Ρ((int) щ);Ρ(ǯ);Ρ(ъ);Ρ(Ю);Ρ(ы);Ρ(Ǩ);Ρ(Ȫ);Ρ(Ȩ);if(Ι==Ζ.Ϋ){Ρ((int) ό.Џ);Ρ(ό.Ǘ);Ρ(ό.ā);Ρ(ƻ(ό.ǘ));Ρ(ƻ(ό.Ǜ));Ρ((int) ύ.Џ);Ρ(ύ.Ǘ);Ρ(ύ.ā);Ρ(ƻ(ύ.ǘ));Ρ(ƻ(ύ.Ǜ));}else{Ρ((int) ȥ);Ρ((int) о);Ρ((int) П);Ρ((int) ч);Ρ((int) ī);Ρ(Đ);Ρ(Ĳ);Ρ(Ŕ);Ρ(ō);Ρ(л);Ρ(к);Ρ(Щ);Ρ(Ů);Ρ(ŭ);Ρ(Ķ);Ρ(ħ);
Ρ(Р);Ρ(Э);Ρ(Ы);Ρ(Ь);Ρ(Ƞ);Ρ(Н);Ρ(į);Ρ(ĭ);}Ρ(";");for(int i=0;i<Ȋ.Count;i++)
Ρ((i>0?"|":"")+Ȋ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΘΚ(Σ[i]),i>0);Ρ(";",false);Ρ(О+"|"+Є+"|"+Ȟ+"|"+ȝ+"|"+Ī,false);Ρ(";",false);Ρ(ΗΦ(),false);String Χ=ţ.ToString();ţ.Clear();return Χ;}х Ηφ(){return ǌ(Storage);}х ǌ(String Χ){if(Χ=="") return х.Ηο;String[]İ=Χ.Split(';');if(Κ(İ,0)!=DATAREV) return х.Βυ;int Β=0;try
{String[]Θ=Κ(İ,1).Split('\n');ΑЉ=μ(Κ(Θ,Β++));Η.δ=μ(Κ(Θ,Β++));Η.υ=μ(Κ(Θ,Β++));Η.γ=μ(Κ(Θ,Β++));Η.ξ=μ(Κ(Θ,Β++));Η.Δ=μ(Κ(Θ,Β++));Η.Ъ=μ(Κ(Θ,Β++));Η.ε=bool.Parse(Κ(Θ,Β++));Ε.Δ=μ(Κ(Θ,Β++));Ε.ξ=μ(Κ(Θ,Β++));Ε.δ=μ(Κ(Θ,Β++));Ε.γ=μ(Κ(Θ,Β++));Ε.υ=μ(Κ(Θ,Β++));Ε.Ъ=μ(Κ(Θ,Β++));
Ε.ε=bool.Parse(Κ(Θ,Β++));Ќ=μ(Κ(Θ,Β++));ǰ=μ(Κ(Θ,Β++));Θ=Κ(İ,2).Split('\n');Β=0;Ι=(Ζ) int.Parse(Κ(Θ,Β++));ΰ=(Μ) int.Parse(Κ(Θ,Β++));щ=(Μ) int.Parse(Κ(Θ,Β++));ǯ=int.Parse(Κ(Θ,Β++));ъ=int.Parse(Κ(Θ,Β++));Ю=int.Parse(Κ(Θ,Β++));ы=int.Parse(Κ(Θ,Β++));Ǩ=float.Parse(Κ(Θ,Β++));
Ȫ=bool.Parse(Κ(Θ,Β++));Ȩ=bool.Parse(Κ(Θ,Β++));if(Ι==Ζ.Ϋ){ό.Џ=(ι) int.Parse(Κ(Θ,Β++));ό.Ǘ=float.Parse(Κ(Θ,Β++));ό.ā=float.Parse(Κ(Θ,Β++));ό.ǘ=ΑΘ(Κ(Θ,Β++));ό.Ǜ=ΑΘ(Κ(Θ,Β++));ύ.Џ=(ι) int.Parse(Κ(Θ,Β++));ύ.Ǘ=float.Parse(Κ(Θ,Β++));ύ.ā=float.Parse(Κ(Θ,Β++));ύ.ǘ=ΑΘ(Κ(Θ,Β++));
ύ.Ǜ=ΑΘ(Κ(Θ,Β++));}else{ȥ=(Ǔ) int.Parse(Κ(Θ,Β++));о=(з) int.Parse(Κ(Θ,Β++));П=(Ё) int.Parse(Κ(Θ,Β++));ч=(ѐ) int.Parse(Κ(Θ,Β++));ī=(Ǔ) int.Parse(Κ(Θ,Β++));Đ=bool.Parse(Κ(Θ,Β++));Ĳ=bool.Parse(Κ(Θ,Β++));Ŕ=bool.Parse(Κ(Θ,Β++));ō=bool.Parse(Κ(Θ,Β++));л=int.Parse(Κ(Θ,Β++));к=int.Parse(Κ(Θ,Β++));
Щ=int.Parse(Κ(Θ,Β++));Ů=float.Parse(Κ(Θ,Β++));ŭ=float.Parse(Κ(Θ,Β++));Ķ=float.Parse(Κ(Θ,Β++));ħ=float.Parse(Κ(Θ,Β++));Р=int.Parse(Κ(Θ,Β++));Э=int.Parse(Κ(Θ,Β++));Ы=int.Parse(Κ(Θ,Β++));Ь=int.Parse(Κ(Θ,Β++));Ƞ=int.Parse(Κ(Θ,Β++));Н=int.Parse(Κ(Θ,Β++));į=int.Parse(Κ(Θ,Β++));
ĭ=float.Parse(Κ(Θ,Β++));}Θ=Κ(İ,3).Replace("\n","").Split('|');Ȋ=Θ.ToList();Θ=Κ(İ,4).Split('\n');Σ.Clear();if(Θ.Count()>=1&&Θ[0]!="")
for(int i=0;i<Θ.Length;i++)
Σ.Add(ΗЇ(Κ(Θ,i)));Θ=Κ(İ,5).Split('|');О=Κ(Θ,0);if(!int.TryParse(Κ(Θ,1),out Є)) Є=-1;if(!float.TryParse(Κ(Θ,2),out Ȟ)) Ȟ=0;if(!float.TryParse(Κ(Θ,3),out ȝ)) ȝ=0;if(!int.TryParse(Κ(Θ,4),out Ī)) Ī=0;if(О.Length!=Р*Э){О="";Є=-1;}ΗΥ(Κ(İ,6));}catch{return х.Δό;}Ť=щ;
χ();return х.ΑΕ;}const int Δπ=64000;IMyTerminalBlock Δύ(){List<IMyTerminalBlock>ň=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ň,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));return ň.Count>0?ň[0]:null;}void ΗΩ(){IMyTerminalBlock Φ=Δύ();if(Φ==null){Λ="Export: name a block "+dataTag;return;}
String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΥ();if(Χ.Length>Δπ){Λ="Export too large: "+Χ.Length+" chars, max "+Δπ;return;}Φ.CustomData=Χ;Λ="Exported "+Σ.Count+" waypoints";}void Ηγ(bool Γφ){IMyTerminalBlock Φ=Δύ();if(Φ==null){Λ="Import: name a block "+dataTag;return;}
Ηδ(Φ.CustomData,Γφ);}void Ηδ(String Χ,bool Γφ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int Ζΰ=Χ.IndexOf('\n');Χ=Ζΰ<0?"":Χ.Substring(Ζΰ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}χ();String Εζ=ΒΥ();х ί=ǌ(Χ);if(ί==х.ΑΕ&&Γφ){Ψ Ċ=new Ψ(Ε);
Vector3 ΘЁ=Ќ,ΘЀ=ǰ;int w=л,h=к,d=Щ;Ǔ ΔΠ=ȥ;ѐ Γά=ч;float ΘЉ=Ķ,ΘЈ=ħ,Θφ=Ȟ,Θυ=ȝ;int ΙΘ=Ī;ǌ(Εζ);Ε=Ċ;Ќ=ΘЁ;ǰ=ΘЀ;л=w;к=h;Щ=d;ȥ=ΔΠ;ч=Γά;Ķ=ΘЉ;ħ=ΘЈ;ΰ=Μ.Ŀ;ſ(true,true);Ȟ=Θφ;ȝ=Θυ;Ī=ΙΘ;ΰ=Μ.Ŀ;щ=Μ.Ŀ;Λ="Imported job, run CONT";return;}if(ί==х.ΑΕ)
{Ɯ=false;Λ="Imported "+Σ.Count+" waypoints";return;}ǌ(Εζ);Λ=ί==х.Βυ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗЁ(){Ɗ.Clear();for(int i=ј.Count-1;i>=0;i--){String Χ=ј[i].CustomData.ToUpper();bool Γτ=false;if(Χ=="INSTRUCTIONS"){Γτ=true;Αέ=true;}
if(Χ=="DEBUG") Γτ=true;if(Γτ){Ɗ.Add(ј[i]);ј.RemoveAt(i);}}ż(Ɗ,false,1,false);}void ΗЂ(List<IMyTerminalBlock>ň){ď.Clear();for(int i=0;i<ň.Count;i++){IMyTerminalBlock Φ=ň[i];try{String Ζη=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Φ.CustomName.IndexOf(Ζη);int ŧ=-1;if(Β<0||!int.TryParse(Φ.CustomName.Substring(Β+Ζη.Length,1),out ŧ)) continue;
if(ŧ==-1) continue;ŧ--;IMyTextSurfaceProvider Ζι=(IMyTextSurfaceProvider) Φ;if(ŧ<Ζι.SurfaceCount&&ŧ>=0)
ď.Add(Ζι.GetSurface(ŧ));}catch{}}}String ΑϏ="";void ΕΙ(){String ΓЁ="";String Γγ="";ΑϏ=Δο(false);ΓЁ+=ΑϏ;Γγ+=ΑϏ;Γγ+=Ηέ();for(int i=0;i<ј.Count;i++) ј[i].WriteText(ΓЁ);for(int i=0;i<ď.Count;i++) ď[i].WriteText(ΓЁ);Echo(Γγ);for(int i=0;i<Ɗ.Count;i++){IMyTextPanel ΓЎ=Ɗ[i];
String Χ=ΓЎ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЎ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЎ.WriteText(Ηή());}}string Ηή(){String Γ="";try{float ΖΧ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ň*ΖΧ)+" / "+(int)(Ś*ΖΧ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ň*100f,1)+"% / "+Math.Round(Ś*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ƃ,2)+"ms / "+Math.Round(ΓЅ,2)+"ms \n";}catch{}for(int i=0;i<Ȣ.Count;i++)
Γ+=""+Ȣ.Keys.ElementAt(i)+" = "+Math.Round(Ȣ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȣ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Ηα(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ż(ј,setLCDFontAndSize,0.9f,false);ż(ď,setLCDFontAndSize,0.9f,true);for(int i=0;i<ј.Count;i++) ј[i].WriteText(Γ+ȟ.ToString());for(int i=0;i<ď.Count;i++) ď[i].WriteText(Γ+ȟ.ToString());}void ǽ(){if(!Αέ) return;try{ΔΑ=Runtime.CurrentInstructionCount;}catch{}
}void Ǹ(String ψ){if(!Αέ) return;if(ΔΑ==0) return;try{float ΒΘ=(Runtime.CurrentInstructionCount-ΔΑ)/Runtime.MaxInstructionCount*100;if(!Ȣ.ContainsKey(ψ))
Ȣ.Add(ψ,new float[]{ΒΘ,ΒΘ});else{Ȣ[ψ][0]=ΒΘ;Ȣ[ψ][1]=Math.Max(ΒΘ,Ȣ[ψ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΗΒ(String ă){if(!ŉ) return;String Ў=""+BROADCAST_SEP;String ς=VERSION+Ў;ς+=ů(ΔΚ)+Ў;
ς+=(int) Ι+Ў;ς+=Βδ(ά.X)+""+Ў;ς+=Βδ(ά.Y)+Ў;ς+=Βδ(ά.Z)+Ў;if(Ι!=Ζ.Ϋ)
ς+=Βζ(щ)+(щ==Μ.І?" "+Math.Round(ƕ,1)+"%":"")+Ў;else ς+=Βζ(щ)+Ў;ς+=ů(Λ)+Ў;ς+=ǝ+""+Ў;ς+=ů(ΑϏ)+Ў;ς+=Ɖ+Ў;ς+=ƙ+Ў;for(int i=0;i<ǚ.Count;i++){if(ǚ[i].Ř!=Л.ŵ) continue;ς+=ů(ǚ[i].ψ)+"/"+ů(ǚ[i].σ)+"/"+ǚ[i].Ǖ+Ў;}Ηώ(ς,ă);}String ů(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηώ(String ς,String ă){ΗϏ(ς,BROADCAST_SELF_ID,ă);}void ΗϏ(String ς,String Ѝ,String ă){try{if(!ŉ) return;ς=BROADCAST_TAG+BROADCAST_SEP+ΔЀ()+BROADCAST_SEP+Ѝ+BROADCAST_SEP+ΓΞ+BROADCAST_SEP+ă+BROADCAST_SEP+ς;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ς);}catch(Exception e){ȟ=e;}
}String ΔЀ(){if(Ι!=Ζ.ф) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔБ(String ς){return ς.StartsWith(BROADCAST_TAG);}bool Ų(ref String Χ,out string ΕЂ,Char Ў){int Β=Χ.IndexOf(Ў);ΕЂ="";if(Β<0) return false;ΕЂ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηπ(ref String ς,out String Ƥ,out String ă)
{Ƥ="";ă="";if(!ŉ) return false;String Ǡ="";if(!Ų(ref ς,out Ǡ,BROADCAST_SEP)||!ΔБ(Ǡ)) return false;if(!Ų(ref ς,out Ƥ,BROADCAST_SEP)) return false;if(!Ų(ref ς,out Ǡ,BROADCAST_SEP)||(Ǡ!=ΔЀ()&&(Ǡ!="*"&&Ι!=Ζ.ф))) return false;if(!Ų(ref ς,out Ǡ,BROADCAST_SEP)||(Ǡ!=ΓΞ)) return false;
if(!Ų(ref ς,out ă,BROADCAST_SEP)) return false;return true;}void Ηζ(){bool Ƈ=true;if(ΕЄ){ΕЄ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ɠ=(Ι!=Ζ.ф?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɠ)
ΔЍ(Ɠ);String[]Θ=Me.CustomData.Split('\n');ŉ=bool.Parse(ΒϏ(Ββ(Θ,"Enable_Broadcast",ref Ƈ)));bool Δδ=false;bool Εω=true;if(ŉ){if(Ι!=Ζ.ф)
ΔΚ=ΒϏ(Ββ(Θ,"Ship_Name",ref Ƈ)).Replace(BROADCAST_SELF_ID,"");ΓΞ=ΒϏ(Ββ(Θ,"Broadcast_Channel",ref Ƈ)).ToLower();Εω=false;if(ȕ==null){ȕ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȕ.SetMessageCallback("");}List<IMyRadioAntenna>ń=new List<IMyRadioAntenna>();ω.GetBlocksOfType(ń);bool ΓΒ=false;
for(int i=0;i<ń.Count;i++)
if(ń[i].EnableBroadcasting&&ń[i].Enabled){ΓΒ=true;break;}if(ń.Count==0) Λ="No Antenna found";else if(!ΓΒ) Λ="Antenna not ready";Δδ=ń.Count==0||!ΓΒ;if(Εή&&!Δδ&&Ι!=Ζ.ф) Λ="Antenna ok";}else if(Ι==Ζ.ф)
Λ="Offline - Enable in PB custom data";Εή=Δδ;if(Εω){if(ȕ!=null) this.IGC.DisableBroadcastListener(ȕ);ȕ=null;}}catch{Ƈ=false;}if(!Ƈ){Λ="Reset custom data";ΔЍ(Ɠ);}}void ΔЍ(String Ɠ){String Χ=Ɠ+"\n\n"+"Enable_Broadcast="+(Ι==Ζ.ф?"true":"false")+" \n";Χ+=Ι!=Ζ.ф?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String ΒϏ(String Ĭ){int Β=Ĭ.IndexOf("//");if(Β!=-1) Ĭ=Ĭ.Substring(0,Β);String[]Й=Ĭ.Split('=');if(Й.Length<=1) return "";return Й[1].Trim();}String Ββ(String[]Θ,String Ŗ,ref bool Ɲ){foreach(String Ĭ in Θ)
if(Ĭ.StartsWith(Ŗ)) return Ĭ;Ɲ=false;return "";}String ΕΜ(int Δβ,String Γ,int Ζξ,ref int Ȥ){String[]Θ=Γ.Split('\n');if(Ζξ>=Ȥ+Δβ-1) Ȥ++;Ȥ=Math.Min(Θ.Count()-1-Δβ,Ȥ);if(Ζξ<Ȥ+1) Ȥ--;Ȥ=Math.Max(0,Ȥ);String ί="";for(int i=0;i<Δβ;i++){int ΖΡ=i+Ȥ;if(ΖΡ>=Θ.Count()) break;
ί+=Θ[ΖΡ]+"\n";}return ί;}String Βε(int ƣ){if(ƣ>=60*60) return Math.Round(ƣ/(60f*60f),1)+" h";if(ƣ>=60) return Math.Round(ƣ/60f,1)+" min";return ""+ƣ+" s";}string Βδ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>Ǫ=new List<IMyCameraBlock>();List<long>Ş=new List<long>();
bool Č=false;int ΓΥ=0,ΙΓ=0,ΓΜ=0;const int Ηψ=16;int ΓΣ=0;DateTime ΓΝ=DateTime.Now;String ΑΣ="";String ΓΡ="";Vector3 ŋ=new Vector3();int Ō=0;void Ηύ(){Ǫ.Clear();Ş.Clear();if(!obstacleBrake) return;ω.GetBlocksOfType(Ǫ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ǫ.Count;i++) Ǫ[i].EnableRaycast=true;List<IMyTerminalBlock>ΑΝ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(ΑΝ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΑΝ.Count;i++) if(!Ş.Contains(ΑΝ[i].CubeGrid.EntityId)) Ş.Add(ΑΝ[i].CubeGrid.EntityId);if(!Ş.Contains(Me.CubeGrid.EntityId)) Ş.Add(Me.CubeGrid.EntityId);
ΓΡ="";if(Υ==null) return;MatrixD m=Υ.WorldMatrix;Vector3[]Θΰ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]ΘЃ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ǫ.Count;i++)
if(Vector3.Dot(Ǫ[i].WorldMatrix.Forward,Θΰ[d])>0.7f){ΓΡ+=ΘЃ[d]+" ";break;}}bool ΔА(Α s){return s==Α.ǐ||s==Α.Ш||s==Α.Ђ||
s==Α.ǹ||s==Α.Ļ||s==Α.ƽ||
(s==Α.Ǐ&&њ!=null);}void Ű(){Č=false;ΓΥ=0;ΑΣ="";}void ΘΔ(){bool ΑΠ=Τ==Α.Ǐ;if(!ΑΠ){ŋ=new Vector3();Ō=0;}if(!obstacleBrake||Ǫ.Count==0||!(ΔА(Τ)||ΑΠ)){Ű();return;}Vector3 Αι=ν+ȧ+ŋ;Vector3 ǲ=Αι-ά;float ю=ǲ.Length();if(ю<1f){Ű();return;}Vector3 ζ=ǲ/ю;
Vector3 ȩ=Υ!=null?(Vector3) Υ.GetShipVelocities().LinearVelocity:new Vector3();if(ȩ.Length()>2) ζ=Vector3.Normalize(ȩ);Vector3 ť=Vector3.Cross(ζ,Math.Abs(ζ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ť.Normalize();Vector3 φ=Vector3.Cross(ť,ζ);float Ѓ=Math.Min(ю+ή*0.25f,ȉ*obstacleLookAhead+ή);
int Εμ=0;for(int k=0;k<2;k++){int p=k==0?0:ΙΓ++%8,e=p%4;Vector3 ΑЅ=k==0?new Vector3():(e==0?ť:e==1?-ť:e==2?φ:-φ)*ή*0.5f;Vector3 Ŏ=ά+ζ*(p>=4?Ѓ*0.5f:Ѓ)+ΑЅ;IMyCameraBlock ΓΠ=ΗΗ(Ŏ);if(ΓΠ==null){Δφ();continue;}Εμ++;MyDetectedEntityInfo Ț=ΓΠ.Raycast(Ŏ);
bool Ιβ=Ț.Type==MyDetectedEntityType.Planet||Ț.Type==MyDetectedEntityType.Asteroid;bool Εβ=Ț.HitPosition.HasValue&&Vector3D.Distance(Ț.HitPosition.Value,ν)<ή;bool Ζα=!Ț.IsEmpty()&&!Ş.Contains(Ț.EntityId)&&!Ηλ(Ț.EntityId)&&
Ț.Type!=MyDetectedEntityType.FloatingObject&&!(ΑΠ&&Εβ)&&
!(Ιβ&&(k>0||Τ==Α.ǐ||Εβ));if(Ζα){if(!Č) ΓΝ=DateTime.Now;Č=true;ΓΥ=0;ΑΣ=Ț.Type.ToString();}else Δφ();if(debugDraw&&Αή.Count<16) Αή.Add(κ(ΓΠ.GetPosition())+","+κ(Ŏ)+","+(Ζα?1:0));}ΓΜ=Εμ>0?0:ΓΜ+1;if(ΓΜ>12) Λ="Cameras can't see ahead";double Εκ=(DateTime.Now-ΓΝ).TotalSeconds;
if(ΑΠ&&Č&&Εκ>4){if(Ō<4){Ō++;ŋ=(Ō==1?φ:Ō==2?ť:Ō==3?-ť:-φ)*ή;ΓΝ=DateTime.Now;Ű();Λ="Docking: going around an obstacle";}else Λ="Docking blocked: "+ΑΣ+", stopped";return;}if(Č&&Εκ>obstacleTimeout)
Λ="Path blocked: "+ΑΣ;}void Δφ(){if(Č&&++ΓΥ>=Ηψ) Ű();}IMyCameraBlock ΗΗ(Vector3 Ŏ){IMyCameraBlock Ȁ=null;float Ει=-2;for(int n=0;n<Ǫ.Count;n++){ΓΣ=(ΓΣ+1)%Ǫ.Count;IMyCameraBlock c=Ǫ[ΓΣ];if(!c.IsFunctional||!c.IsWorking) continue;float Ζμ=Vector3.Dot(Vector3.Normalize(Ŏ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Ζμ>Ει+0.05f&&c.CanScan(Ŏ)){Ȁ=c;Ει=Ζμ;}}return Ȁ;}const String ΕΦ="[PAMTFC]";const float ΗЊ=5f;const float Βα=2f;class Ȓ{public long ϊ;public Vector3 λ,ȩ;public Vector3 Ɣ;public float Εψ;public String Ċ="";public int Γρ=-1;public bool ΔΒ;public int Ασ;public int Ơ;public float ΑЁ=-1;
public float Θι,Θε;public long έ=0;public int ƌ=0;public DateTime ΔΨ;}IMyBroadcastListener ΒΗ=null;long н=0;List<Ȓ>п=new List<Ȓ>();Vector3 ȧ=new Vector3();float р=-1;Vector3 Δά=new Vector3();bool ΒΖ=false;DateTime Δέ=DateTime.Now;const float ΕΥ=15f;const float ΗЉ=10f;int ΖЉ=0;
String О="";int Є=-1;bool ΑΨ=false,ΑΧ=false,Ƒ=false;Vector3 Εο=new Vector3();float ľ(){return Ȟ>0?Ȟ:ǵ;}float Ľ(){return ȝ>0?ȝ:Ǵ;}String ę(){if(ΰ==Μ.в||Ι==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Р+"|"+Э+"|"+(int) ī+"|"+Math.Round(ľ(),1)+"|"+Math.Round(Ľ(),1)+"|"+Ī;}void ΘΗ(){if(!trafficEnabled) return;if(ΒΗ==null) ΒΗ=IGC.RegisterBroadcastListener(ΕΦ);н=Me.CubeGrid.EntityId;int Ģ=30;while(ΒΗ.HasPendingMessage&&Ģ-->0){String Χ=ΒΗ.AcceptMessage().Data as String;
if(Χ!=null) Ησ(Χ);}for(int i=п.Count-1;i>=0;i--)
if((DateTime.Now-п[i].ΔΨ).TotalSeconds>ΗЊ) п.RemoveAt(i);if(ΒΓ==null) ΒΓ=IGC.RegisterBroadcastListener(ΗЈ);Ģ=5;while(ΒΓ.HasPendingMessage&&Ģ-->0){String Χ=ΒΓ.AcceptMessage().Data as String;if(Χ!=null) try{Ητ(Χ);}catch{}}if(ΑΩ==null) ΑΩ=IGC.RegisterBroadcastListener(Ημ);
Ģ=10;while(ΑΩ.HasPendingMessage&&Ģ-->0){String Χ=ΑΩ.AcceptMessage().Data as String;if(Χ!=null) try{Ηρ(Χ);}catch{}}if(++ΖЉ<3) return;ΖЉ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",н+";"+κ(Ε.Δ)+";"+
κ(ΰ==Μ.в?new Vector3():Ε.υ)+";"+κ(ǰ)+";"+
κ(ν+ȧ)+";"+String.Join(":",Αή));Αή.Clear();}IGC.SendBroadcastMessage(ΕΦ,н+";"+κ(ά)+";"+
κ(Υ.GetShipVelocities().LinearVelocity)+";"+ή+";"+ę()+";"+
(ΰ==Μ.І?Є:-1)+";"+О+";"+ΗΝ()+","+Δϋ()+";"+κ(Δά)+
";"+(Αα?1:0)+";"+(Ą&&ΰ==Μ.І?1:0)+","+ΔΙ+","+ΔΘ+","+Ην()+","+ΕΧ()+","+Math.Round(Vector3.Distance(ά,ν))+";"+Me.GetId());}void Ησ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long ϊ=long.Parse(f[0]);if(ϊ==н) return;Ȓ Ϗ=null;for(int i=0;i<п.Count;i++) if(п[i].ϊ==ϊ) Ϗ=п[i];
if(Ϗ==null){Ϗ=new Ȓ();Ϗ.ϊ=ϊ;п.Add(Ϗ);}Ϗ.λ=μ(f[1]);Ϗ.ȩ=μ(f[2]);Ϗ.Εψ=float.Parse(f[3]);Ϗ.Ċ=f[4];Ϗ.Γρ=int.Parse(f[5]);Ϗ.ΔΨ=DateTime.Now;if(Ϗ.Ċ!=""&&Ϗ.Ċ==ę()) ΕΖ(f[6]);Ϗ.έ=0;Ϗ.ƌ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ϗ.έ=long.Parse(d[0]);Ϗ.ƌ=int.Parse(d[1]);
}Ϗ.Ɣ=f.Length>=9?μ(f[8]):new Vector3();Ϗ.ΔΒ=f.Length>=10&&f[9]=="1";Ϗ.Ασ=0;Ϗ.Ơ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ϗ.Θι=float.Parse(w[1]);Ϗ.Θε=float.Parse(w[2]);Ϗ.Ασ=int.Parse(w[3]);if(w.Length>=5) Ϗ.Ơ=int.Parse(w[4]);Ϗ.ΑЁ=w.Length>=6?float.Parse(w[5]):-1;}
}}catch{}}bool Ηλ(long ϊ){for(int i=0;i<п.Count;i++) if(п[i].ϊ==ϊ) return true;return false;}Vector3 ΔЄ(int Β,int ΘЊ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 Ιί=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*ΘЊ;
Vector3 φ=Ω.ξ.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ξ):-Ω.γ;Vector3 Я=Vector3.Cross(Ιί,φ);if(Я.LengthSquared()<0.0001f) return new Vector3();float Ѓ=laneOffset*ή;float Θη=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Я)*Ѓ*Math.Min(1,Θη/(Ѓ*4));}Vector3 ΖЈ=new Vector3();List<String>Αή=new List<String>();bool ΔЊ(){Α s=Τ;return ę()!=""&&(s==Α.ǐ||s==Α.Ě||s==Α.ǥ||
s==Α.ђ||s==Α.ė);}float ΕΒ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Р*ľ(),2)+Math.Pow(Э*Ľ(),2));}bool ǈ(){return ę()!=""&&Vector3.Distance(ά,Ε.Δ)<ΕΒ()+ή*2;}int ΕΧ(){Α s=Τ;if(s==Α.ђ||s==Α.ǹ||(s==Α.Ђ&&ǈ())) return 2;return s==Α.ǐ||s==Α.ǥ||s==Α.ė||(s==Α.Ш&&ǈ())?1:0;
}bool ΔЈ(float Ζέ,Ȓ o){if(o.ΑЁ<0||Math.Abs(Ζέ-o.ΑЁ)<=2) return н<o.ϊ;return Ζέ<o.ΑЁ;}int Ην(){if(!ΔЊ()) return 0;return Τ!=Α.ǐ&&Vector3.Dot(ά-Ј,Ќ)>ή*0.5f?2:1;}bool Ηκ(Α s){return s==Α.Ǐ||s==Α.Ē||s==Α.Ė||s==Α.Ż||
s==Α.ǔ||s==Α.ǥ||s==Α.ђ||s==Α.ė||s==Α.ȫ;}void ΘΘ(){ȧ=new Vector3();р=-1;Δά=new Vector3();bool ΕЇ=ΔА(Τ);if(!trafficEnabled||(!ΕЇ&&!Ηκ(Τ))){ΒΖ=false;return;}ΖЈ=Υ.GetShipVelocities().LinearVelocity;ΗΞ(Υ.GetNaturalGravity(),!ΕЇ);ΖБ();}IMyBroadcastListener ΒΓ=null;
float ũ=-1;DateTime ΖЄ=DateTime.MinValue;void Ητ(String Χ){if(!Χ.StartsWith("C;")) return;ΖЄ=DateTime.Now;ũ=-1;String ƚ=н+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƚ)) ũ=float.Parse(e[i].Substring(ƚ.Length));}void ΖБ(){if(ũ<0||(DateTime.Now-ΖЄ).TotalSeconds>1.5) return;
р=р<0?ũ:Math.Min(р,ũ);if(ũ<0.5f) Λ="Traffic: giving way (controller)";}void ΗΞ(Vector3 ξ,bool Ζζ=false){ȧ=new Vector3();р=-1;Vector3 ǲ=ν-ά;float ю=ǲ.Length();bool Γο=ю>=1f;Vector3 ζ=Γο?ǲ/ю:new Vector3();Δά=ζ;Vector3 φ=ξ.LengthSquared()>0.01f?-Vector3.Normalize(ξ):-Ǝ;
Vector3 Я=Vector3.Cross(ζ,φ);Я=Я.LengthSquared()<0.0001f?-Ɨ:Vector3.Normalize(Я);Ȓ Αυ=null;bool ΕЉ=Τ==Α.Ђ&&!ȑ(1);int ΓЈ=ΕΧ();for(int i=0;i<п.Count;i++){Ȓ α=п[i];if(α.ƌ==3&&α.ȩ.LengthSquared()<0.25f) continue;Vector3 λ=α.λ+α.ȩ*(float)(DateTime.Now-α.ΔΨ).TotalSeconds;
Vector3 ȣ=λ-ά;float ȗ=(ή+α.Εψ)*0.5f;bool Ζλ=α.Ċ!=""&&α.Ċ==ę();if(Ζλ&&α.Ασ==2) continue;bool ΑЏ=Ζλ&&α.Ασ==1&&ΔЊ();if(ΑЏ) ȗ=Math.Min(ľ(),Ľ())*0.9f;float ΓΩ=ȗ*(ΑЏ?1f:1.2f);float ȓ=Γο?Vector3.Dot(ȣ,ζ):0;if(ȓ>0&&(ȣ-ζ*ȓ).Length()<=ΓΩ){float ΑІ=Vector3.Dot(α.ȩ,ζ);
bool ΕЏ=α.Ɣ.LengthSquared()>0.25f;bool ΕА=ΑІ<-1f||(ΕЏ&&Vector3.Dot(α.Ɣ,ζ)<-0.5f);if(!Ζζ&&α.Ơ<=ΓЈ){if(α.ΔΒ){if(!ΕЉ){ȧ=Я*ȗ*1.3f;continue;}float Ζά=Vector3.Distance(ά,ν),ΖΛ=Vector3.Distance(λ,ν);if(Ζά<ΖΛ||(Ζά==ΖΛ&&н<α.ϊ)) continue;}if(ΕА){if(ȓ<(ȉ-ΑІ)*6f+ȗ*4f) ȧ=Я*ȗ*1.3f;
continue;}if(ΕЏ&&ΔЈ(ю,α)){Vector3 Εε=-ȣ;float Ζδ=Vector3.Dot(Εε,α.Ɣ);if(Ζδ>0&&(Εε-α.Ɣ*Ζδ).Length()<=ΓΩ){ȧ=Я*ȗ*1.3f;continue;}}}float ΕЈ=ȓ-ȗ-(ΑЏ?1f:followGap*ή);float Γϋ=Math.Max(0,ΑІ)+Math.Max(0,ΕЈ)/Βα;if(ΕА) Γϋ=Math.Max(0,ΕЈ+Math.Min(0,ΑІ)*Βα*2)/Βα;
if(р<0||Γϋ<р){р=Γϋ;Αυ=α;}continue;}if(α.ȩ.LengthSquared()<1f) continue;if(Γο&&(ȣ-ζ*ȓ).Length()<=ΓΩ) continue;Vector3 ΔΗ=α.ȩ-ΖЈ;float Ζκ=ΔΗ.LengthSquared();if(Ζκ<0.25f) continue;float t=-Vector3.Dot(ȣ,ΔΗ)/Ζκ;if(t<=0||t>5f||(ȣ+ΔΗ*t).Length()>=(ΑЏ?ȗ:ȗ*1.1f)) continue;
if(α.Ơ>ΓЈ||(α.Ơ==ΓЈ&&!ΔЈ(ю,α))||t<2f){р=0;Αυ=α;}}bool Θο=р>=0&&р<0.5f;if(!Θο||Ζζ||(Αυ!=null&&Αυ.ΔΒ&&ΕЉ)){ΒΖ=false;return;}if(!ΒΖ){ΒΖ=true;Δέ=DateTime.Now;}double ΕБ=(DateTime.Now-Δέ).TotalSeconds;if(ΕБ>ΕΥ+ΗЉ) Δέ=DateTime.Now;else if(ΕБ>ΕΥ){р=-1;ȧ=Я*ή*1.3f;Λ="Traffic: passing a stuck ship";}
}List<int>ȅ=new List<int>();String ΖΒ="";void Βΰ(){String Ŗ=Р+"|"+Э+"|"+(int) ī;if(Ŗ==ΖΒ) return;ΖΒ=Ŗ;ΗΓ(Р,Э,ī,ȅ);}void ΗΓ(int w,int h,Ǔ ΔΠ,List<int>ΓΤ){ΓΤ.Clear();if(ΔΠ==Ǔ.ΑΖ){for(int k=0;k<w*h;k++){int š=k/w;ΓΤ.Add(š*w+(š%2==0?k-š*w:w-1-(k-š*w)));
}return;}int ΕЎ=(int) Math.Ceiling(w/2f),ΕЍ=(int) Math.Ceiling(h/2f);int Θλ=w/2,Θκ=h/2;int x=0,y=0,Γα=0,Γβ=-1,Αϋ=Math.Max(w,h);for(int i=0;i<Αϋ*Αϋ;i++){if(-ΕЎ<x&&x<=Θλ&&-ΕЍ<y&&y<=Θκ) ΓΤ.Add((y-1+ΕЍ)*w+(x-1+ΕЎ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Γα;Γα=-Γβ;Γβ=t;}
x+=Γα;y+=Γβ;}}bool Βί(){int n=Math.Max(0,Р*Э);if(О.Length==n) return false;О=new String('0',n);Є=-1;return true;}void ΕΖ(String α){Βί();if(α.Length!=О.Length) return;char[]h=О.ToCharArray();for(int i=0;i<h.Length;i++) if(α[i]=='1') h[i]='1';О=new String(h);}bool ǂ(int Ȗ){return Ȗ>=0&&Ȗ<О.Length&&О[Ȗ]=='1';}
void ΒЅ(int Ȗ,bool і){if(Ȗ<0||Ȗ>=О.Length) return;char[]h=О.ToCharArray();h[Ȗ]=і?'1':'0';О=new String(h);}bool Βξ(int Ȗ,bool Θχ){if(!trafficEnabled) return false;String Ŗ=ę();for(int i=0;i<п.Count;i++){Ȓ t=п[i];if(t.Ċ!=Ŗ||t.Γρ<0||(Θχ&&t.ϊ>н)) continue;if(t.Γρ==Ȗ) return true;
}return false;}int ΗΜ(int Β){int n=Р*Э;if(n<1) return Β;Βΰ();if(Βί()) for(int k=0;k<Β&&k<n;k++) ΒЅ(ȅ[k],true);if(Є>=0&&(Β>=n||ȅ[Β]!=Є)){ΒЅ(Є,true);Є=-1;}if(ΕΑ()){if(Ǭ>=0&&!ǂ(Ǭ)){Ą=false;Є=Ǭ;return ȅ.IndexOf(Ǭ);}if(Ǭ==-2){Є=-1;Ą=О.IndexOf('0')>=0;return Ą?-2:n;}
if(Ǭ==-1&&Є>=0&&!ǂ(Є)){Ą=false;return Β;}Є=-1;if(!Ą){Ą=true;Ερ=DateTime.Now;}if((DateTime.Now-Ερ).TotalSeconds<5) return-1;}Ą=false;if(Є>=0&&!ǂ(Є)&&!Βξ(Є,true)) return Β;Є=-1;for(int k=0;k<n;k++){int c=ȅ[k];if(ǂ(c)||Βξ(c,false)) continue;Є=c;
return k;}return О.IndexOf('0')>=0?-2:n;}const String Ημ="[PAMJOB]";const String ΗЈ="[PAMTCC]";IMyBroadcastListener ΑΩ=null;DateTime Επ=DateTime.MinValue;DateTime Ερ=DateTime.Now;int Ǭ=-1;bool Ą=false;bool ΕΑ(){return trafficEnabled&&Ǭ!=-3&&(DateTime.Now-Επ).TotalSeconds<3;}
void Ηρ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ę()) return;Επ=DateTime.Now;ΕΖ(f[2]);Ǭ=-3;String ƚ=н+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƚ)) Ǭ=int.Parse(e[i].Substring(ƚ.Length));}Ł ΗЀ(int Β)
{Βΰ();if(Β<0||Β>=ȅ.Count) return Ł.Δν;Ы=ȅ[Β]%Р;Ь=ȅ[Β]/Р;return Ł.Δώ;}class ё{public long ϊ;public Vector3 λ,Ж,φ;}List<ё>Г=new List<ё>();bool Ȳ=false;Vector3 Αγ,Αβ,Αδ,Αε;long ш=0;long ȇ=0;Ψ њ=null;bool ƍ=false;Ψ Βά(Vector3 λ,Vector3 Ж,Vector3 φ){MatrixD m=MatrixD.CreateWorld(λ,Ж,φ);
Ψ w=new Ψ(Η);w.δ=Vector3D.TransformNormal(Αγ,m);w.γ=Vector3D.TransformNormal(Αβ,m);w.υ=Vector3D.TransformNormal(Αδ,m);w.Δ=λ+(Vector3) Vector3D.TransformNormal(Αε,m);w.ε=true;return w;}int Δϋ(){Α s=Τ;if(s==Α.й) return Т(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ė||s==Α.Ǿ||s==Α.ĕ||s==Α.ΑΚ||
s==Α.Ƽ||s==Α.ǔ) return 3;if(s==Α.Ǐ||s==Α.Ē||s==Α.Ż) return 2;if(s==Α.Ђ) return 1;return 0;}long ΗΝ(){if(Δϋ()==0) return 0;if(ȑ(1)&&Ŧ!=0) return Ŧ;return њ!=null?ȇ:ш;}void ΕΓ(){if(Ι==Ζ.Ϋ||!Η.ε) return;IMyShipConnector М=Т(MyShipConnectorStatus.Connected);if(М==null||М.OtherConnector==null) return;
IMyShipConnector α=М.OtherConnector;Vector3I Θπ=new Vector3I((int) Η.Ъ.X,(int) Η.Ъ.Y,(int) Η.Ъ.Z);if(М.Position==Θπ){MatrixD œ=MatrixD.Transpose(α.WorldMatrix);Αγ=Vector3D.TransformNormal(Υ.WorldMatrix.Forward,œ);Αβ=Vector3D.TransformNormal(Υ.WorldMatrix.Down,œ);Αδ=Vector3D.TransformNormal(Υ.WorldMatrix.Left,œ);
Αε=Vector3D.TransformNormal(М.GetPosition()-α.GetPosition(),œ);Ȳ=true;}if(Vector3.Distance(М.GetPosition(),Η.Δ)<5) ш=α.EntityId;List<IMyShipConnector>Ħ=new List<IMyShipConnector>();ω.GetBlocksOfType(Ħ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ħ.Count>0){Г.Clear();for(int i=0;i<Ħ.Count;i++){ё p=new ё();p.ϊ=Ħ[i].EntityId;p.λ=Ħ[i].GetPosition();p.Ж=Ħ[i].WorldMatrix.Forward;p.φ=Ħ[i].WorldMatrix.Up;Г.Add(p);}}њ=null;ȇ=0;if(α.EntityId==ш||!Ȳ) return;for(int i=0;i<Г.Count;i++)
if(Г[i].ϊ==α.EntityId){ȇ=Г[i].ϊ;њ=Βά(Г[i].λ,Г[i].Ж,Г[i].φ);}}bool Βρ(long ϊ,int Ζή){if(ϊ==0) return false;for(int i=0;i<п.Count;i++){Ȓ t=п[i];if(t.έ==ϊ&&(t.ƌ>Ζή||(t.ƌ==Ζή&&t.ϊ<н))) return true;}return false;}bool ΒΧ(int ΓА){if(Ι==Ζ.Ϋ||!trafficEnabled){њ=null;ȇ=0;return true;}
long θ=њ!=null?ȇ:ш;if(!Βρ(θ,ΓА)) return true;if(!Βρ(ш,ΓА)){њ=null;ȇ=0;return true;}if(!Ȳ) return false;ё Ȁ=null;float Εθ=0;for(int i=0;i<Г.Count;i++){ё p=Г[i];if(p.ϊ==ш||Βρ(p.ϊ,ΓА)) continue;float d=Vector3.Distance(p.λ,Η.Δ);if(Ȁ==null||d<Εθ){Ȁ=p;Εθ=d;}
}if(Ȁ==null) return false;ȇ=Ȁ.ϊ;њ=Βά(Ȁ.λ,Ȁ.Ж,Ȁ.φ);return true;}String ΗΦ(){String s=(Ȳ?"1":"0")+":"+κ(Αγ)+":"+κ(Αβ)+":"+
κ(Αδ)+":"+κ(Αε)+":"+ш;for(int i=0;i<Г.Count;i++)
s+=":"+Г[i].ϊ+","+κ(Г[i].λ)+","+κ(Г[i].Ж)+","+κ(Г[i].φ);return s;}void ΗΥ(String s){Г.Clear();Ȳ=false;ш=0;String[]f=s.Split(':');if(f.Length<6) return;Ȳ=f[0]=="1";Αγ=μ(f[1]);Αβ=μ(f[2]);Αδ=μ(f[3]);Αε=μ(f[4]);long.TryParse(f[5],out ш);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ё p=new ё();if(!long.TryParse(e[0],out p.ϊ)) continue;p.λ=μ(e[1]);p.Ж=μ(e[2]);p.φ=μ(e[3]);Г.Add(p);}}const String ΒΩ="[PAMDCK]";IMyBroadcastListener ΒΑ=null;int Ȅ=0;DateTime Εό=DateTime.Now;DateTime Ζυ=DateTime.MinValue;long Ζφ=0;
int Ζχ=0;long Ŧ=0;bool Ʊ=false;int Γΰ=0;List<Vector3>Ƴ=new List<Vector3>();List<float>Δλ=new List<float>();bool Αα=false;bool Αΰ=false;Vector3 Εϋ=new Vector3();bool ž(){return Ȅ!=0&&(DateTime.Now-Ζυ).TotalSeconds<3;}bool ȑ(int Ατ){return ž()&&Ζφ==н&&Ζχ==Ατ;}bool ΕΠ(){return ž()||(DateTime.Now-Εό).TotalSeconds<3;}
void ΒЃ(int Ατ){if(Ȅ==Ατ) return;Ȅ=Ατ;Εό=DateTime.Now;Γΰ=3;}void ΒЂ(){if(Ȅ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΩ,"D;"+н);Ȅ=0;Ʊ=false;}void ΗА(){if(!trafficEnabled||Ι==Ζ.Ϋ) return;if(ΒΑ==null) ΒΑ=IGC.RegisterBroadcastListener(ΒΩ);int Ģ=20;while(ΒΑ.HasPendingMessage&&Ģ-->0)
{String Χ=ΒΑ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;try{Ης(Χ);}catch{}}if(Τ!=Α.Ђ){Αα=false;Αΰ=false;}if(Ȅ==1&&Т(MyShipConnectorStatus.Connected)!=null) ΒЂ();if(Ȅ==2&&!Ʊ&&Τ!=Α.ǔ&&
Vector3.Distance(ά,и().Δ)>ή*3) ΒЂ();if(Ȅ!=0&&++Γΰ>=3){Γΰ=0;IGC.SendBroadcastMessage(ΒΩ,"R;"+н+";"+Ȅ+";"+ш+";"+(Ȳ?1:0));}}void Ης(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool ΕІ=false;String[]ΕΪ=f[5].Split(',');for(int i=0;i<ΕΪ.Length;i++) if(ΕΪ[i]==н.ToString()) ΕІ=true;
if(!ΕІ) return;Ζυ=DateTime.Now;Ζφ=long.Parse(f[2]);Ζχ=int.Parse(f[3]);Ŧ=long.Parse(f[4]);Ƴ.Clear();Δλ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ƴ.Add(μ(e[0]));Δλ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εώ=f[6].Split(':');List<ё>Ф=new List<ё>();for(int i=0;i<Εώ.Length;i++){String[]e=Εώ[i].Split(',');if(e.Length<4) continue;ё p=new ё();p.ϊ=long.Parse(e[0]);p.λ=μ(e[1]);p.Ж=μ(e[2]);p.φ=μ(e[3]);Ф.Add(p);}Г=Ф;}bool ΗΚ(){if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return true;
ΒЃ(1);if(!ΕΠ()) return true;if(!ȑ(1)){Λ="Waiting for dock clearance";return false;}ΒΟ();return true;}bool ΒΟ(){ƍ=false;ё Į=null;if(Ŧ!=0&&Ŧ!=ш&&Ȳ)
for(int i=0;i<Г.Count;i++) if(Г[i].ϊ==Ŧ) Į=Г[i];long Δγ=Į==null?0:Į.ϊ;if(Δγ==ȇ&&(Δγ==0)==(њ==null)) return false;ȇ=Δγ;њ=Į==null?null:Βά(Į.λ,Į.Ж,Į.φ);return true;}bool ļ(Vector3 p){for(int i=0;i<Ƴ.Count;i++) if(Vector3.Distance(p,Ƴ[i])<Δλ[i]+ή*0.75f) return true;
return false;}bool Ηβ(Vector3 ƈ,Vector3 Ŝ){Αα=false;if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return false;if(Ȅ!=1&&Vector3.Distance(ά,Η.Δ)>dockRequestDistance) return false;ΒЃ(1);if(!ž()||ȑ(1)||Ƴ.Count==0) return false;if(!ļ(ƈ)&&!ļ(Ŝ)&&!ļ(ά)) return false;Αα=true;return true;
}bool Δτ(){if(!trafficEnabled||Ι==Ζ.Ϋ||ш==0) return true;ΒЃ(2);if(!ΕΠ()) return true;if(!ȑ(2)){Λ="Waiting for undock clearance";return false;}return true;}void ΗΣ(string є){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}