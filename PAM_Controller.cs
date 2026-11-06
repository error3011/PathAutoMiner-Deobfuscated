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
public enum Μ{р,Ň,Ǜ,Ї,η}public enum Κ{ђ,ļ,ƺ,Ŏ,ǅ,Ǿ,о,
Ļ,ΒΣ,ǘ,ƙ,ΑЉ,Ɠ,
Βϋ,ȅ,Ȅ,Ķ,ΓΔ,ΒΝ,
ΓΒ,ΓΓ,ΒΞ,ΒΟ,Ȇ}public enum Ξ{Ю,Ŋ,ё,ƚ,Ȱ,ƻ,Ƽ,Ȭ,ȭ}public enum к{None,Ķ,ђ,ΒΘ}public enum Ι{Main,ǃ,Ȁ,ǿ,ŕ,ȯ,
Ĳ,Ʈ,Ư,ƭ,ȃ,ǈ,
Ą,Ő,œ,Ǌ,Ĉ}public enum п{ǆ,ΒΔ,ΒЉ,Βη,η}public enum а{Ŗ,ΑБ}public enum н{Ǆ,ΒΡ,ΒЋ,Ŀ}public enum К{ƾ,ΒΗ,ΒΖ,ΒΛ,ΒΚ,Βε,Βδ}public enum Ь{ĸ,ΑЌ,Ɣ}public enum Ā{Βγ,Δα,ΑЊ}public enum ϊ{ƿ,ǀ,ǁ,Βσ,Βς,
Βν,Βξ,Βμ,Βπ,
Βρ,Βο}public enum Ο{Т,Я,С,Ȯ,ĳ,Ĵ}public enum Δγ{Δΰ,ΔΫ,Δή}public enum У{ǌ,ƛ,Βλ,ƽ}public enum ξ{None,ȁ,ō,ň}public enum ζ{Ř,ı,ń,Ŀ,Ƹ,ņ,ƹ,ΒА,ŉ}public enum ο{р,ƶ,Ƶ,Ņ,ŀ,ķ,Ʒ}class Ψ{public bool θ=false;public Vector3 Ρ=new Vector3();public Vector3 ϋ=new Vector3();
public Vector3 ώ=new Vector3();public Vector3 Б=new Vector3();public Vector3 β=new Vector3();public Vector3 в=new Vector3();public float Γυ=0;public float ΕΤ=0;public float[]И=null;public Ψ(){}public Ψ(Ψ ы){θ=ы.θ;Ρ=ы.Ρ;ϋ=ы.ϋ;ώ=ы.ώ;Б=ы.Б;β=ы.β;в=ы.в;И=ы.И;}public Ψ(Vector3 Ρ,Vector3 ώ,Vector3 ϋ,Vector3 Б,Vector3 β)
{this.Ρ=Ρ;this.ϋ=ϋ;this.ώ=ώ;this.Б=Б;this.Γυ=0;this.β=β;}public void Δέ(List<IMyThrust>Č,List<string>й){И=new float[й.Count];for(int i=0;i<И.Length;i++)
И[i]=-1;for(int i=0;i<Č.Count;i++){string ǔ=Βα(Č[i]);int Α=й.IndexOf(ǔ);if(Α!=-1)
И[Α]=Ћ(Č[i].MaxEffectiveThrust,Č[i].MaxThrust);}}}class Βφ{public Ψ ί=null;public List<Vector3>ΕΘ=new List<Vector3>();public float Δψ=0;public float ΕΨ=0;public float Δκ=0;public float Δϋ=0;public Vector3 Δό=new Vector3();}class ĉ{public ϊ Љ=ϊ.ƿ;public float М=0;public float љ=0;
public string Ф="";public string Ц="";DateTime Ακ;public bool ΓЀ=false;private bool Š=false;public bool ΒΑ(bool ž){if(ž){љ=0;Š=false;return false;}Š=true;return љ>М;}public void Δβ(){ΒΑ(true);ΓЀ=false;}public void Δζ(){if(Š)
if((DateTime.Now-Ακ).TotalSeconds>1){љ++;Ακ=DateTime.Now;}}public bool ƫ(){switch(Љ){case ϊ.ǀ:return true;case ϊ.ǁ:return true;}return false;}}class Э{public String ΰ="";public String Ё="";public int ύ=0;public Ā ű=Ā.ΑЊ;public Э(String ΰ,String Ё,int ύ,Ā ű){this.ΰ=ΰ;this.Ё=Ё;
this.ύ=ύ;this.ű=ű;}}class ΒΙ{public ΒΙ(Vector3 Ѐ,float у){this.Ѐ=Ѐ;this.у=у;}public Vector3 Ѐ;public float у;}class μ{public DateTime Ů;public DateTime ΑΞ;public String О="";public String ΰ="";public String ĭ="";public String Φ="";public String Ɖ="";public String ġ="";public String ż="";
public Vector3 Ρ=new Vector3();public List<Э>е=new List<Э>();public Μ Ν=Μ.р;public ξ т;public float Ǎ;public float і;public int δ;public int Ţ=0;public int Ǝ=0;public bool ƴ(){return(DateTime.Now-Ů).TotalSeconds>10;}public bool Ł(){if(т!=ξ.ȁ) return false;return(DateTime.Now-ΑΞ).TotalSeconds>=4;
}public μ(String О){this.О=О;}public ξ ǜ(String φ,bool ΓЌ,bool Ȍ,bool ΓΝ){if(φ==""&&!Ȍ) return ξ.None;if(т==ξ.ȁ&&Ł())
т=ξ.ň;if(ΓЌ){ż=φ;т=ξ.ȁ;ΑΞ=DateTime.Now;return т;}else if(Ȍ){ż="";т=ξ.None;}else if(ż==φ){if(ΓΝ) т=ξ.ō;return т;}return ξ.None;}}public String ΒΩ(String Γλ){switch(Γλ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem ф;Vector3 Αω=new Vector3();Vector3 П=new Vector3();Vector3 Γΰ=new Vector3();Vector3 Γο=new Vector3();Vector3 ΓΩ=new Vector3();DateTime Αι=new DateTime();
bool Ȕ=true;int ƌ=0;bool ș=false;bool Ŭ=false;bool Γψ=true;bool ΕΫ=false;bool ΔϏ=false;bool Γη=false;float ΓЎ=0;float Αώ=0;int ΓІ=0;int ΕΥ=0;int ħ=0;int Δπ=0;float ǫ=0;float Ǡ=0;double Ŷ=0;float Ȉ=0;List<int>Ǩ=new List<int>();List<int>ǳ=new List<int>();Μ Ν=Μ.р;
int Ƃ=0;float Ǔ=0;float ю=0;float Ǹ=0;IMyRemoteControl Ж;IMySensorBlock ρ;List<IMyTimerBlock>ΑЃ=new List<IMyTimerBlock>();List<IMyShipConnector>Ȏ=new List<IMyShipConnector>();List<IMyThrust>ȥ=new List<IMyThrust>();List<IMyGyro>Αβ=new List<IMyGyro>();List<IMyTerminalBlock>Р=new List<IMyTerminalBlock>();
List<IMyLandingGear>ŭ=new List<IMyLandingGear>();List<IMyReactor>Ž=new List<IMyReactor>();List<IMyConveyorSorter>ΓБ=new List<IMyConveyorSorter>();List<IMyGasTank>ȗ=new List<IMyGasTank>();List<IMyTerminalBlock>ȇ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Δσ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Δι=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΑΚ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ϗ=new List<IMyTextPanel>();List<IMyTextSurface>Ъ=new List<IMyTextSurface>();List<IMyTextPanel>ј=new List<IMyTextPanel>();IMyTerminalBlock ь=null;Ψ Σ=new Ψ();Ψ Αγ=new Ψ();
Ψ Αη=new Ψ();bool ѐ=false;bool Ȩ=false;bool ȩ=false;bool Αε=false;List<String>й=new List<string>();List<Ψ>ν=new List<Ψ>();List<Ψ>Į=new List<Ψ>();int Δω=0;double ΔΡ=0;Ψ Π=new Ψ();Vector3 Ȝ;Vector3 Ǭ;Ξ ι=Ξ.Ю;а ĝ=а.Ŗ;int ѝ=0;int ќ=0;double Ĝ=0;bool ΑΥ=false;ĉ κ=new ĉ();
ĉ λ=new ĉ();н ъ=н.Ǆ;bool Ȫ=true;а Ǒ=а.Ŗ;int Џ=3;int Ў=3;int Л=30;Ь д=Ь.ĸ;bool ǖ=true;bool Ș=true;bool Ȓ=true;bool ǒ=false;К ȑ=К.ƾ;bool я=true;float Ч=90;float Ш=20;float Г=5;float Щ=20;float с=0.70f;float İ=1.50f;float į=2.50f;float ȫ=0f;float ǧ=0f;Κ ĕ;Ξ ш;
Κ ȣ;Βφ ΔЍ=null;int ė=0;int Ę=0;int г=0;int Ț=0;int ƀ=30;int ΔЏ=0;bool ΔΒ=true;Vector3 ΔЈ;double ΑΪ=0;bool Αϋ=false;int ΔΣ=0;int ΕΓ=0;int Δλ=0;int ΔЌ=0;int ΔЊ=0;Vector3 ΔЎ=new Vector3();float ΔЅ=0;float ΕΔ=0;int Ǖ=0;Ψ Ĩ=null;float ΔІ=0;float Δμ=0;float ΑΤ=0;
float ŵ=0;bool ΔЀ=false;bool ΕΣ=false;bool ΕΧ=false;bool Δύ=false;bool Δξ=false;DateTime Δϊ=new DateTime();Ψ Δφ=null;Ψ ΓΨ=null;float ΑΛ=0;к ȉ;float Αδ=0;float ΑІ=0;String Γς="";float Ű=0;float і=0;float Ǎ=0;List<Э>ΔЁ=new List<Э>();float ψ=0;bool Ƅ=false;bool ΔΗ=false;
bool ΓЏ=false;bool ΑЂ=false;bool ΓА=false;float Γτ=0;float ΓϏ=0;Vector3 Γή=new Vector3();Vector3 Ασ=new Vector3();Vector3 ΕΛ=new Vector3(1,1,1);float ΕΙ=1;Vector3 ΕΚ=new Vector3();bool Δώ=false;bool Γγ=false;bool ΔЇ=false;bool Δθ=false;float Δυ=0;float Δη=2;Vector3 ΕΗ;
Vector3 ΕΖ;Vector3 ΕΕ;float[,]ȧ=new float[3,2];Dictionary<String,float[,]>и=new Dictionary<string,float[,]>();Dictionary<String,float>ΕΪ=new Dictionary<String,float>();int Δν=0;Dictionary<String,float[]>Ħ=new Dictionary<string,float[]>();float ΔА;bool ΑΧ=false;int č=0;int ц=0;
int Ě=0;int ћ=0;String Φ="";int[]Αρ=new int[Enum.GetValues(typeof(Ι)).Length];bool Ÿ=false;int ǎ=0;int δ=0;Ι Ϋ=Ι.Main;int ΔΘ=0,ΔΙ=0,ΔΚ=0,ΔΛ=0;int ΓЇ=0,ΓЈ=0;bool ȋ=false;bool ΑΖ=false;String ȡ="";String Ş="";IMyBroadcastListener б=null;bool Αή=true;List<μ>χ=null;
μ ε=null;int đ=0,Ē=0;int ΑΠ=0,ΑΡ=0;int ΓΤ=0,ΓΥ=0,ΓΦ=0;String Γν="";Exception ǩ=null;bool Αϊ=false;Ξ Ɓ=Ξ.Ю;bool ΑΦ=false;bool ΔЄ=true;bool ΔБ=false;String ΔΟ="";bool ΑЅ=false;bool ƈ=true;int Ū=0;int[]ΕΒ=null;Vector3 ş=new Vector3();bool Αΰ=false;bool ł(IMyTerminalBlock Ϊ)=>Ϊ.CubeGrid==Me.CubeGrid;
Program(){ф=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Ƭ(Me,controllerTag,true))
Ν=Μ.Ї;Τ(Ι.Main);if(Ν!=Μ.Ї){Φ="Welcome to [PAM]!";У ſ=Βω();if(Ν==Μ.р){List<IMyShipDrill>Αά=new List<IMyShipDrill>();List<IMyShipGrinder>Αα=new List<IMyShipGrinder>();ф.GetBlocksOfType(Αά,q=>q.CubeGrid==Me.CubeGrid);ф.GetBlocksOfType(Αα,q=>q.CubeGrid==Me.CubeGrid);if(Αά.Count>0)
{Ν=Μ.Ň;Φ="Miner mode enabled!";}else if(Αα.Count>0){Ν=Μ.Ǜ;Φ="Grinder mode enabled!";}else{Ν=Μ.η;ǉ(п.η);}}if(ſ==У.ǌ) Γψ=false;if(ſ==У.ƛ) Φ="Data restore failed!";if(ſ==У.ƽ) Φ="New version, wipe data";}}void Main(string Є,UpdateType ΔΝ){try{if(ǩ!=null){if(Є.Trim().ToUpper()=="RETRY")
{ǩ=null;Ȕ=true;ΓІ=0;if(Ν!=Μ.Ї) є();Φ="Restarted after error";}else{Ββ();Βχ();return;}}Ŭ=(ΔΝ&UpdateType.Update10)!=0;if(Ŭ) ƌ++;ș=ƌ>=10;if(ș) ƌ=0;if(Ŭ){č++;if(č>4) č=0;ħ=(int) Math.Max(0,10-(DateTime.Now-Αι).TotalSeconds);}if(Є!="") Γν=Є;if(Ν!=Μ.Ї)
ΒЈ(Є);else
ΒΕ(Є);Ÿ=false;try{int ΑΣ=Runtime.CurrentInstructionCount;float Ǐ=Ћ(ΑΣ,Runtime.MaxInstructionCount);if(Ǐ>0.90) Φ="Max. instructions >90%";if(Ǐ>ǫ) ǫ=Ǐ;if(ΑΧ){Ǩ.Add(ΑΣ);while(Ǩ.Count>10) Ǩ.RemoveAt(0);Ǡ=0;for(int i=0;i<Ǩ.Count;i++) Ǡ+=Ǩ[i];Ǡ=Ћ(Ћ(Ǡ,Ǩ.Count),Runtime.MaxInstructionCount);
double ů=Runtime.LastRunTimeMs;if(Γη&&ů>Ŷ) Ŷ=ů;ǳ.Add((int)(ů*1000f));while(ǳ.Count>10) ǳ.RemoveAt(0);Ȉ=0;for(int i=0;i<ǳ.Count;i++) Ȉ+=ǳ[i];Ȉ=Ћ(Ȉ,ǳ.Count)/1000f;}}catch{ǫ=0;}}catch(Exception e){ǩ=e;}}String ΒΪ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ν!=Μ.η)
Β+="[SHUTTLE] Enable shuttle mode\n"+"[NEXT] Next hole\n"+"[PREV] Previous hole\n\n"+
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Τ(Ι Ђ){Αρ[(int) Ϋ]=δ;δ=Αρ[(int) Ђ];if(Ђ==Ι.ȃ) δ=0;Ϋ=Ђ;if(Ν!=Μ.Ї)
ċ(Ϋ==Ι.Ȁ,false,0,0);Ÿ=true;}void Βκ(bool Ų){if(δ>0) δ--;else if(Ų) δ=ǎ-1;}void Βι(bool Ų){if(δ<ǎ-1) δ++;else if(Ų) δ=0;}String ǉ(п Ѕ){switch(Ѕ){case п.ǆ:Φ="Job is running";break;case п.ΒΔ:Φ="Connector not ready!";break;case п.ΒЉ:Φ="Ship modified, path outdated!";break;
case п.Βη:Φ="Interrupted by player!";break;case п.η:Φ="Shuttle mode enabled!";break;}return "";}String Βΰ(а Ѐ){switch(Ѐ){case а.Ŗ:return "Top-Left";case а.ΑБ:return "Center";default:return "";}}String ΒΧ(Ь ή){switch(ή){case Ь.ΑЌ:return "Auto"+(Ν==Μ.Ň?" (Ore)":"");case Ь.Ɣ:return "Auto (+Stone)";
case Ь.ĸ:return "Default";default:return "";}}String Βέ(Ξ υ){switch(υ){case Ξ.Ю:return "No job";case Ξ.Ŋ:return "Job paused";case Ξ.ё:return "Job active";case Ξ.Ȭ:return "Job active";case Ξ.ȭ:return "Job active";case Ξ.ƚ:return "Job done";case Ξ.Ȱ:return "Job changed";case Ξ.ƻ:return "Move home";
case Ξ.Ƽ:return "Move to job";}return "";}String Ơ(н ΓΛ){switch(ΓΛ){case н.Ǆ:return "Return home";case н.ΒΡ:return "Fly to job pos";case н.ΒЋ:return "Stop";case н.Ŀ:return "Ignore";}return "";}String ΒΨ(К ή){switch(ή){case К.ƾ:return "Off";case К.ΒΛ:return "Drop pos (Stone) ";case К.ΒΚ:return "Drop pos (Sto.+Ice)";
case К.ΒΗ:return "Cur. pos (Stone)";case К.ΒΖ:return "Cur. pos (Sto.+Ice)";case К.Βε:return "In motion (Stone)";case К.Βδ:return "In motion (Sto.+Ice)";}return "";}String ƞ(к υ){switch(υ){case к.None:return "No batteries";case к.Ķ:return "Charging";case к.ΒΘ:return "Discharging";}return "";}String ƥ(Κ υ)
{String ΔЃ=Ν==Μ.η?"target":"job";switch(υ){case Κ.ђ:return "Idle";case Κ.ļ:return "Flying to XY position";case Κ.ƺ:return Ν==Μ.Ǜ?"Grinding":"Mining";case Κ.Ŏ:return "Returning";case Κ.ΒΞ:return "Flying to drop pos";case Κ.ǅ:return "Returning to dock";case Κ.Ǿ:return "Flying to dock area";
case Κ.о:return "Flying to job area";case Κ.Ļ:return "Flying to path";case Κ.ΒΣ:return "Flying to job position";case Κ.ǘ:return "Approaching dock";case Κ.ƙ:return "Docking";case Κ.ΑЉ:return "Aligning";case Κ.Ɠ:return "Aligning";case Κ.Βϋ:return "Retry docking";case Κ.ȅ:return "Unloading";case Κ.Ȇ:return ΔΟ;
case Κ.Ȅ:return "Undocking";case Κ.Ķ:return "Charging batteries";case Κ.ΓΔ:return "Waiting for uranium";case Κ.ΒΝ:return "Filling up hydrogen";case Κ.ΓΒ:return "Waiting for ejection";case Κ.ΓΓ:return "Waiting for ejection";case Κ.ΒΟ:return "Flying to drop pos";}return "";}String ƪ(ϊ Љ){switch(Љ)
{case ϊ.ƿ:return "On \"Undock\" command";case ϊ.ǀ:return "On player entered cockpit";case ϊ.Βσ:return "On ship is full";case ϊ.Βς:return "On ship is empty";case ϊ.ǁ:return "On time delay";case ϊ.Βξ:return "On batteries empty(<25%)";case ϊ.Βμ:return "On batteries empty(=0%)";case ϊ.Βν:return "On batteries full";
case ϊ.Βρ:return "On hydrogen empty(<25%)";case ϊ.Βο:return "On hydrogen empty(=0%)";case ϊ.Βπ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int Αύ,int х,bool Ε,String ў){ǎ+=1;if(Αύ==х) ў=">"+ў+(č>=2?" .":"");else ў=" "+ў;Β+=ў+"\n";return Αύ==х&&Ε;}String ΑЍ(bool Ε)
{int Α=0;int Δ=δ;ǎ=0;String Ƿ="———————————————\n";String Ǻ="--------------------------------------------\n";String Β="";Β+=Βέ(ι)+" | "+(Σ.θ?"Ready to dock":"No dock")+"\n";Β+=Ƿ;double ΑЁ=Math.Max(Math.Round(this.ΑΪ),0);if(Ϋ==Ι.Main){bool ч=Ν==Μ.η;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΒЊ();
if(Ν==Μ.Ň)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Τ(Ι.Ȁ);if(Ν==Μ.Ǜ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Τ(Ι.Ȁ);if(Ν==Μ.η)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Τ(Ι.œ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) Ɨ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΒΠ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΒΤ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ч) Τ(Ι.Ĉ);else Τ(Ι.ȯ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Τ(Ι.Ʈ);if(Ν!=Μ.η)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Τ(Ι.ƭ);}else if(Ϋ==Ι.Ȁ){double ΔΤ=Math.Round(Џ*Ǔ,1);double Γδ=Math.Round(Ў*ю,1);String α="";if(Ζ(ref α,Δ,Α++,Ε," Start new job!")) ǋ();if(Ζ(ref α,Δ,Α++,Ε," Change current job")){ŗ(false,false);Τ(Ι.Main);}if(Ζ(ref α,Δ,Α++,Ε," Width + (Width: "+Џ+" = "+ΔΤ+"m)")){Ǘ(ref Џ,5,20,1);Ǟ(true);}
if(Ζ(ref α,Δ,Α++,Ε," Width -")){Ǘ(ref Џ,-5,20,-1);Ǟ(true);}if(Ζ(ref α,Δ,Α++,Ε," Height + (Height: "+Ў+" = "+Γδ+"m)")){Ǘ(ref Ў,5,20,1);Ǟ(true);}if(Ζ(ref α,Δ,Α++,Ε," Height -")){Ǘ(ref Ў,-5,20,-1);Ǟ(true);}if(Ζ(ref α,Δ,Α++,Ε," Depth + ("+(д==Ь.ĸ?"Depth":"Min")+": "+Л+"m)")){Ǘ(ref Л,5,50,2);Ǟ(true);}
if(Ζ(ref α,Δ,Α++,Ε," Depth -")){Ǘ(ref Л,-5,50,-2);Ǟ(true);}if(Ζ(ref α,Δ,Α++,Ε," Depth mode: "+ΒΧ(д))){д=м(д);}if(Ζ(ref α,Δ,Α++,Ε," Start pos: "+Βΰ(Ǒ))){Ǒ=м(Ǒ);}if(Ν==Μ.Ǜ&&д==Ь.Ɣ) д=м(д);Β+=ŏ(8,α,Δ,ref ΓЇ);}else if(Ϋ==Ι.œ){float[]ΑΨ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Τ(Ι.Ǌ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ƪ(κ.Љ))) κ.Љ=м(κ.Љ);if(!κ.ƫ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ľ((int) κ.М))) κ.М=Ƙ(κ.М,ΑΨ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+ƪ(λ.Љ))) λ.Љ=м(λ.Љ);
if(!λ.ƫ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ľ((int) λ.М))) λ.М=Ƙ(λ.М,ΑΨ);}else if(Ϋ==Ι.Ǌ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ǋ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.œ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(κ.Ф!=""?κ.Ф:"-"))) κ.Ф=Ă(ref ΔΘ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(κ.Ц!=""?κ.Ц:"-"))) κ.Ц=Ă(ref ΔΚ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(λ.Ф!=""?λ.Ф:"-"))) λ.Ф=Ă(ref ΔΙ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(λ.Ц!=""?λ.Ц:"-"))) λ.Ц=Ă(ref ΔΛ);
}else if(Ϋ==Ι.ǿ){String Αμ=Ч+" %";if(ș) ц++;if(ц>1){ц=0;Ě++;if(Ě>1) Ě=0;}bool[]ƅ=new bool[]{Ž.Count==0,ȉ==к.None,ȗ.Count==0};int ΑЄ=0;while(true){ΑЄ++;ћ++;if(ћ>ƅ.Length-1) ћ=0;if(ΑЄ>=ƅ.Length) break;if(!ƅ[ћ]) break;}bool ч=Ν==Μ.η;if(!ч&&ǖ&&ψ!=-1&&Ě==0)
Αμ=ψ<1000000?Math.Round(ψ)+" Kg":Math.Round(ψ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){є();Τ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ч) Τ(Ι.ȯ);else Τ(Ι.Ĉ);if(!ч){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΑЈ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΑЅ=true;Β+=Ǻ;if(!ч) Β+="Progress: "+Math.Round(Ĝ,1)+" %\n";Β+="State: "+ƥ(ĕ)+" "+ΑЁ+"m \n";Β+="Load: "+Ű+" % Max: "+Αμ+" \n";if(ћ==0) Β+="Uranium: "+(Ž.Count==0?"No reactors":Math.Round(ΑІ,1)+"Kg "+(Г==-1?"":" Min: "+Г+" Kg"))+"\n";
if(ћ==1) Β+="Battery: "+(ȉ==к.None?ƞ(ȉ):ΑΛ+"% "+(Ш==-1||ч?"":" Min: "+Ш+" %"))+"\n";if(ћ==2) Β+="Hydrogen: "+(ȗ.Count==0?"No tanks":Math.Round(Αδ,1)+"% "+(Щ==-1||ч?"":" Min: "+Щ+" %"))+"\n";}else if(Ϋ==Ι.ȯ){String α="";if(Ζ(ref α,Δ,Α++,Ε," Back")){if(ι==Ξ.ё) Τ(Ι.ǿ);else Τ(Ι.Main);}
if(Ζ(ref α,Δ,Α++,Ε," Max load: "+Ч+"%")) І(ref Ч,Ч<=80?-10:-5,ζ.ń,false);if(Ζ(ref α,Δ,Α++,Ε," Weight limit: "+(ǖ?"On":"Off"))) ǖ=!ǖ;if(Ζ(ref α,Δ,Α++,Ε," Ejection: "+ΒΨ(ȑ))) ȑ=м(ȑ);if(Ζ(ref α,Δ,Α++,Ε," Toggle sorters: "+(ǒ?"On":"Off"))){ǒ=!ǒ;if(ǒ) Œ(ƈ);}
if(Ζ(ref α,Δ,Α++,Ε," Unload ice: "+(я?"On":"Off"))) я=!я;if(Ζ(ref α,Δ,Α++,Ε," Uranium: "+(Г==-1?"Ignore":"Min "+Г+"Kg"))) І(ref Г,(Г>5?-5:-1),ζ.ņ,true);if(Ζ(ref α,Δ,Α++,Ε," Battery: "+(Ш==-1?"Ignore":"Min "+Ш+"%"))) І(ref Ш,-5,ζ.Ƹ,true);if(Ζ(ref α,Δ,Α++,Ε," Hydrogen: "+(Щ==-1?"Ignore":"Min "+Щ+"%"))) І(ref Щ,-10,ζ.ƹ,true);
if(Ζ(ref α,Δ,Α++,Ε," When done: "+(Ȫ?"Return home":"Stop"))) Ȫ=!Ȫ;if(Ζ(ref α,Δ,Α++,Ε," On damage: "+Ơ(ъ))) ъ=м(ъ);if(Ζ(ref α,Δ,Α++,Ε," Advanced...")) Τ(Ι.Ĳ);Β+=ŏ(8,α,Δ,ref ΓЈ);}else if(Ϋ==Ι.Ĳ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ι==Ξ.ё) Τ(Ι.ǿ);else Τ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ν==Μ.Ǜ?" Grinder":" Drill")+" inv. balancing: "+(Ș?"On":"Off"))) Ș=!Ș;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ν==Μ.Ǜ?" grinders":" drills")+": "+(Ȓ?"Fwd + Bwd":"Fwd"))) Ȓ=!Ȓ;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+İ+"m/s")) І(ref İ,0.5f,ζ.Ř,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+į+"m/s")) І(ref į,0.5f,ζ.Ř,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(с*100f)+"%"+(с>0.80f?" (risky)":""))) І(ref с,0.1f,ζ.ı,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ȫ*100f+"%")) Ɩ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+ǧ*100f+"%")) Ɩ(false,0.05f);
}else if(Ϋ==Ι.Ĉ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ι==Ξ.ё) Τ(Ι.ǿ);else Τ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+Ч+"%")) І(ref Ч,Ч<=80?-10:-5,ζ.ń,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(я?"On":"Off"))) я=!я;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Г==-1?"Ignore":"Min "+Г+"Kg"))) І(ref Г,(Г>5?-5:-1),ζ.ņ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(Ш==-1?"Ignore":"Charge up"))) Ш=(Ш==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(Щ==-1?"Ignore":"Fill up"))) Щ=(Щ==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+Ơ(ъ))) ъ=м(ъ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(с*100f)+"%"+(с>0.80f?" (risky)":""))) І(ref с,0.1f,ζ.ı,false);
}else if(Ϋ==Ι.ǃ){double Γξ=0;if(ν.Count>0) Γξ=Vector3.Distance(ν.Last().Ρ,П);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΒЎ();if(Ν!=Μ.η){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ѐ?"Use old home":(Σ.θ?"Was set! ":"none ")))) ѐ=!ѐ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ѐ?"Use old connector":(Σ.θ?"Was set! ":"none ")))) ѐ=!ѐ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ȩ?"Use old connector":(Π.θ?"Was set! ":"none ")))) Ȩ=!Ȩ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ȩ?"Use old path":(ν.Count>1?"Count: "+ν.Count:"none ")))) ȩ=!ȩ;Β+=Ǻ;Β+="Wp spacing: "+Math.Round(ΔΡ)+"m\n";}else if(Ϋ==Ι.ŕ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){є();Τ(Ι.Main);}
Β+=Ǻ;Β+="State: "+ƥ(ĕ)+" \n";Β+="Speed: "+Math.Round(ΓЎ,1)+"m/s\n";Β+="Target dist: "+ΑЁ+"m\n";Β+="Wp count: "+ν.Count+"\n";Β+="Wp left: "+ΔΣ+"\n";}else if(Ϋ==Ι.Ʈ){List<IMyTerminalBlock>ї=ΒΦ();if(ș) ц++;if(ц>=ї.Count) ц=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Τ(Ι.Ư);
Β+=Ǻ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(Ű,1)+"% "+Math.Round(і,1)+" / "+Math.Round(Ǎ,1)+"\n";Β+="Uranium: "+(Ž.Count==0?"No reactors":Math.Round(ΑІ,1)+"Kg "+Γς)+"\n";Β+="Battery: "+(ȉ==к.None?"":ΑΛ+"% ")+ƞ(ȉ)+"\n";Β+="Hydrogen: "+(ȗ.Count==0?"No tanks":Math.Round(Αδ,1)+"% ")+"\n";
Β+="Damage: "+(ї.Count==0?"None":""+(ц+1)+"/"+ї.Count+" "+ї[ц].CustomName)+"\n";}else if(Ϋ==Ι.Ư){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=Ǻ;Β+="Next scan: "+ħ+"s\n";Β+="Ship size: "+Math.Round(Ǔ,1)+"m "+Math.Round(ю,1)+"m "+Math.Round(Ǹ,1)+"m \n";
Β+="Broadcast: "+(ȋ?"Online - "+ȡ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ǫ*100f,1)+"% \n";}else if(Ϋ==Ι.ƭ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Τ(Ι.Main);Β+=Ǻ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Ƃ==2) Β="Fatal setup error\nNext scan: "+ħ+"s\n";if(Αϊ) Β="Recompile script now";int Γπ=Β.Split('\n').Length;for(int i=Γπ;i<=10;i++) Β+="\n";Β+=Ƿ;Β+="Last: "+Φ+"\n";
return Β;}void Ɩ(bool Γκ,float ĩ){є();ŗ(true,false);if(Γκ) І(ref ȫ,ĩ,ζ.ŉ,false);else І(ref ǧ,ĩ,ζ.ŉ,false);ΑА();ċ(true,true,0,0);}float Ƙ(float ǡ,float[]Ĭ){float Χ=Ĭ[0];for(int i=Ĭ.Length-1;i>=0;i--)
if(ǡ<Ĭ[i]) Χ=Ĭ[i];return Χ;}String Ă(ref int х){String Χ="";if(х>=ΑЃ.Count) х=-1;if(х>=0) Χ=ΑЃ[х].CustomName;х++;return Χ;}void Ǘ(ref int value,int Ȋ,int Ī,int Ɔ){if(Ȋ==0) return;if(value<Ī&&Ɔ>0||value<=Ī&&Ɔ<0){value+=Ɔ;return;}int ΓΖ=Math.Abs(Ȋ);int ř=0;int Ź=1;
while(true){ř+=Ź*ΓΖ*10;if(Ȋ<0&&value-Ī<=ř) break;if(Ȋ>0&&value-Ī<ř) break;Ź++;}value+=Ź*Ȋ;}void Ǟ(bool ΕΦ){Џ=Math.Max(Џ,1);Ў=Math.Max(Ў,1);Л=Math.Max(Л,0);ċ(Ϋ==Ι.Ȁ,false,0,0);}void І(ref float value,float ĩ,ζ Ё,bool Ȧ){І(ref value,value+ĩ,Ё,Ȧ,true);
}void І(ref float value,float ǭ,ζ Ё,bool Ȧ,bool ΔΧ){float Д=0,В=0;if(Ё==ζ.Ř){Д=0.5f;В=10f;}if(Ё==ζ.ı){Д=0.1f;В=1f;}if(Ё==ζ.Ŀ){Д=50f;В=100f;}if(Ё==ζ.Ƹ){Д=5f;В=30f;}if(Ё==ζ.ņ){Д=1f;В=25f;}if(Ё==ζ.ń){Д=10f;В=95f;}if(Ё==ζ.ƹ){Д=10f;В=90f;}
if(Ё==ζ.ΒА){Д=10f;В=1800;}if(Ё==ζ.ŉ){Д=0.0f;В=0.75f;}if(ǭ==-1&&Ȧ){value=-1;return;}if(value==-1) Ȧ=false;bool Αυ=ǭ<Д||ǭ>В;if(Αυ&&ΔΧ){if(ǭ<value) value=В;else if(ǭ>value) value=Д;}else value=ǭ;if(Αυ&&Ȧ) value=-1;else value=Math.Max(Д,Math.Min(value,В));
value=(float) Math.Round(value,2);}void ΑЈ(bool ΓЂ){if(ΓЂ) г=Math.Max(0,г-1);else г++;ΑΕ(true);}T м<T>(T ǡ){int Α=Array.IndexOf(Enum.GetValues(ǡ.GetType()),ǡ);Α++;if(Α>=ΒΜ(ǡ)) Α=0;return(T) Enum.GetValues(ǡ.GetType()).GetValue(Α);}int ΒΜ<T>(T value){return Enum.GetValues(value.GetType()).Length;
}void ΒЊ(){Į.Clear();for(int i=0;i<ν.Count;i++) Į.Add(ν[i]);ν.Clear();Αε=true;Αγ=new Ψ(Σ);Αη=new Ψ(Π);Σ.θ=false;if(Ν==Μ.η) Π.θ=false;for(int i=0;i<и.Count;i++)
if(!й.Contains(и.Keys.ElementAt(i)))
й.Add(и.Keys.ElementAt(i));ѐ=false;Ȩ=false;ȩ=false;Τ(Ι.ǃ);}void ΒЎ(){if(ѐ) Σ=Αγ;if(Ȩ) Π=Αη;if(ȩ){ν.Clear();for(int i=0;i<Į.Count;i++) ν.Add(Į[i]);}Αε=false;є();Τ(Ι.Main);}void Ć(Ψ Н,Ξ υ){ΓΨ=Н;if(ι==Ξ.ё) ш=υ;}bool ơ(Ψ Н,float у,bool ΔΞ,out Vector3 Ѐ){if(ΔΞ){Vector3I Γβ=new Vector3I((int) Н.в.X,(int) Н.в.Y,(int) Н.в.Z);
IMySlimBlock Ϊ=Me.CubeGrid.GetCubeBlock(Γβ);if(Ϊ==null||!(Ϊ.FatBlock is IMyShipConnector)){Ѐ=new Vector3();return false;}Vector3 ΓΣ=ā(Ж,Ϊ.FatBlock.GetPosition()-П);Vector3 ΓΡ=ā(Ж,Ϊ.FatBlock.WorldMatrix.Forward);Ѐ=Н.Ρ-ΑΒ(Н.ώ,Н.ϋ*-1,ΓΣ)-ΑΒ(Н.ώ,Н.ϋ*-1,ΓΡ)*у;return true;}
else{Ѐ=Н.Ρ;return true;}}Vector3 Βά(int ΓΟ,int Αό,bool ΓЃ){if(!ΓЃ&&Αΰ) return ş;float ΓΠ=((ѝ-1f)/2f)-ΓΟ;float ΓЅ=((ќ-1f)/2f)-Αό;ş=Π.Ρ+Π.Б*ΓΠ*Ǔ+Ǭ*-1*ΓЅ*ю;Αΰ=true;return ş;}Ο Ĺ(){float Ѝ=-1;Ο Χ=Ο.Т;if(Ν!=Μ.η){if(ι!=Ξ.Ю){Vector3 π=ΑΓ(Ȝ,Ǭ*-1,П-Π.Ρ);
if(Math.Abs(π.X)<=(float)(ѝ*Ǔ)/2f&&Math.Abs(π.Y)<=(float)(ќ*ю)/2f){if(π.Z<=-1&&π.Z>=-ƀ*2) return Ο.Ȯ;if(π.Z>-1&&π.Z<Ǹ*2) return Ο.Т;}if(л(Π.Ρ,ref Ѝ)) Χ=Ο.Т;}if(Σ.θ){if(л(Σ.Ρ,ref Ѝ)) Χ=Ο.С;for(int i=0;i<ν.Count;i++)
if(л(ν[i].Ρ,ref Ѝ)) Χ=Ο.Я;if(Vector3.Distance(П,Σ.Ρ)<dockDist*Ǹ) Χ=Ο.С;if(ǽ(MyShipConnectorStatus.Connectable)!=null||ǽ(MyShipConnectorStatus.Connected)!=null)
Χ=Ο.С;}}else{Vector3 Ѐ=new Vector3();IMyShipConnector Đ=ǽ(MyShipConnectorStatus.Connected);if(Σ.θ){if(л(Σ.Ρ,ref Ѝ)) Χ=Ο.С;if(ơ(Σ,dockDist,true,out Ѐ))
if(л(Ѐ,ref Ѝ)) Χ=Ο.С;if(Đ!=null&&Vector3.Distance(Đ.GetPosition(),Σ.Ρ)<5)
return Ο.ĳ;}for(int i=0;i<ν.Count;i++)
if(Vector3.Distance(ν[i].Ρ,Σ.Ρ)>dockDist*Ǹ&&Vector3.Distance(ν[i].Ρ,Π.Ρ)>dockDist*Ǹ)
if(л(ν[i].Ρ,ref Ѝ)) Χ=Ο.Я;if(Π.θ){if(л(Π.Ρ,ref Ѝ)) Χ=Ο.Т;if(ơ(Π,dockDist,true,out Ѐ))
if(л(Ѐ,ref Ѝ)) Χ=Ο.Т;if(Đ!=null&&Vector3.Distance(Đ.GetPosition(),Π.Ρ)<5)
return Ο.Ĵ;}}return Χ;}bool л(Vector3 Ѐ,ref float Ѝ){float у=Vector3.Distance(Ѐ,П);if(у<Ѝ||Ѝ==-1){Ѝ=у;return true;}return false;}void ǋ(){if(Ƃ>0){Φ="Setup error! Can't start";return;}if(Ν==Μ.η){Ɨ();return;}Π.Ρ=П;Π.β=Ж.GetNaturalGravity();Π.ώ=Γΰ;Π.ϋ=ΓΩ;Π.Б=Γο;
Ȝ=ь.WorldMatrix.Forward;Ǭ=Π.ϋ;if(Ȝ==Ж.WorldMatrix.Down) Ǭ=Ж.WorldMatrix.Backward;ŗ(true,true);ά(Κ.ļ);Ŕ();}void ŗ(bool А,bool ΓЄ){if(ι==Ξ.Ю&&!А) return;bool Γω=А||ι==Ξ.ƚ||ѝ!=Џ||ќ!=Ў||ĝ!=Ǒ;if(Γω){if(ι!=Ξ.Ю){ι=Ξ.Ȱ;Βά(ė,Ę,ΓЄ);Φ="Job changed, lost progress";}ĝ=Ǒ;
ѝ=Џ;ќ=Ў;Ę=0;ė=0;ţ="";ĥ=-1;ŵ=0;Ț=0;ΑΤ=0;г=0;ΑΕ(true);}}void ŋ(){ΒЃ(П,0);ą(ȥ,true);}void Ŕ(){ǉ(п.ǆ);ŋ();ő(ŭ,false);Œ(ƈ);ι=Ξ.ё;ΑЏ(true);ш=ι;Τ(Ι.ǿ);Βύ();ΑΥ=true;Ū=0;for(int i=ȇ.Count-1;i>=0;i--)
if(Ʊ(ȇ[i],false)) Ū++;if(Ū>0) Φ="Started with damage";}void є(){if(ι==Ξ.ё){ι=Ξ.Ŋ;Φ="Job paused";}ά(Κ.ђ);ш=ι;ΒЄ(false,0,0,0,0);ΒЌ();ΑЋ(new Vector3(),false);ΒЍ();ΒЀ(ChargeMode.Auto);ΒЅ(false);ΒЂ(true);ѓ(Κ.ђ);ċ(false,false,0,0);ą(Р,false);ą(Ȏ,true);Œ(true);Αϋ=false;ΒΒ();ΓΧ=false;
ΑΥ=false;Ƅ=false;ΑЅ=false;if(Ϋ!=Ι.Main&&Ϋ!=Ι.ȯ&&Ϋ!=Ι.Ĳ&&Ϋ!=Ι.Ĉ)
Τ(Ι.Main);}void Ɨ(){Ο ж=Ĺ();if(Ν==Μ.η){if(!Π.θ||!Σ.θ) return;Ŕ();bool ŧ=Vector3.Distance(П,Σ.Ρ)<Vector3.Distance(П,Π.Ρ);if(Ɓ==Ξ.Ȭ) ŧ=true;if(Ɓ==Ξ.ȭ) ŧ=false;if(ŧ){Ć(Σ,Ξ.Ȭ);switch(ж){case Ο.ĳ:ά(Κ.Ȇ);break;case Ο.Я:ά(Κ.о);break;case Ο.С:ά(Κ.ǘ);break;default:ά(Κ.Ȅ);break;
}}else{Ć(Π,Ξ.ȭ);switch(ж){case Ο.Ĵ:ά(Κ.Ȇ);break;case Ο.Т:ά(Κ.ǘ);break;case Ο.Я:ά(Κ.о);break;default:ά(Κ.Ȅ);break;}}}else{if(ι!=Ξ.Ŋ&&ι!=Ξ.Ȱ) return;bool ΔΠ=ι==Ξ.Ȱ;Ŕ();bool ũ=Βθ(false)&&Σ.θ;switch(ж){case Ο.Т:ά(ũ?Κ.Ļ:Κ.ļ);break;case Ο.Я:ά(ũ?Κ.Ǿ:Κ.о);break;case Ο.С:ά(ũ?Κ.ǘ:Κ.ȅ);break;
case Ο.Ȯ:{if(Ț!=г||ΔΠ) ά(Κ.Ŏ);else ά(Κ.ƺ);}break;default:break;}}}void ΒΤ(){if(ι==Ξ.Ю&&!Σ.θ) return;if(Ν==Μ.η&&(!Π.θ||!Σ.θ)) return;Φ="Move to job";Ο ж=Ĺ();if(Ν==Μ.η){Ć(Π,Ξ.ȭ);switch(ж){case Ο.Т:ά(Κ.ǘ);break;case Ο.Я:ά(Κ.о);break;case Ο.Ĵ:return;default:ά(Κ.Ȅ);break;
}ѓ(Κ.Ȇ);}else{switch(ж){case Ο.Т:ά(Κ.о);break;case Ο.Я:ά(Κ.о);break;case Ο.С:ά(Κ.ȅ);break;case Ο.Ȯ:ά(Κ.Ŏ);break;default:break;}if(ι==Ξ.Ю) ѓ(Κ.о);else ѓ(Κ.Ɠ);Αϋ=true;}ŋ();Τ(Ι.ŕ);ő(ŭ,false);ш=Ξ.Ƽ;}void ΒΠ(){if(!Σ.θ) return;Φ="Move home";Ο ж=Ĺ();if(Ν==Μ.η){Ć(Σ,Ξ.Ȭ);switch(ж){
case Ο.Я:ά(Κ.Ǿ);break;case Ο.С:ά(Κ.ǘ);break;case Ο.ĳ:return;default:ά(Κ.Ȅ);break;}ѓ(Κ.Ȇ);}else{if(ǽ(MyShipConnectorStatus.Connected)!=null) return;if(ǽ(MyShipConnectorStatus.Connectable)!=null){ά(Κ.ƙ);ѓ(Κ.ȅ);return;}switch(ж){case Ο.Т:ά(Κ.Ļ);break;case Ο.Я:ά(Κ.Ǿ);break;case Ο.С:ά(Κ.Ǿ);break;
case Ο.Ȯ:ά(Κ.ǅ);break;default:break;}ѓ(Κ.ȅ);}ŋ();Τ(Ι.ŕ);ő(ŭ,false);ш=Ξ.ƻ;}void ά(Κ ź){if(ź==Κ.ђ) ȣ=Κ.ђ;if(ȣ!=Κ.ђ&&ĕ==ȣ&&ź!=ȣ){є();return;}ΔΒ=true;ĕ=ź;}void ѓ(Κ υ){this.ȣ=υ;}void ΑΕ(bool ž){if(ž){Ĝ=0;return;}if(Ν==Μ.η) return;float ΑΫ=г*Math.Max(1,Л);if(Ț==г) ΑΫ+=Math.Min(Л,ΑΤ);
float ΔΜ=ѝ*ќ*Math.Max(1,Л);Ĝ=Math.Max(Ĝ,(float) Math.Min(ΑΫ/ΔΜ*100.0,100));}List<IMyTerminalBlock>ΒΦ(){List<IMyTerminalBlock>ї=new List<IMyTerminalBlock>();for(int i=0;i<ȇ.Count;i++){IMyTerminalBlock Ϊ=ȇ[i];if(Ʊ(Ϊ,false)) ї.Add(Ϊ);}return ї;}bool Ʊ(IMyTerminalBlock Ϊ,bool Ɗ)
{return(!Ʋ(Ϊ,Ɗ)||!Ϊ.IsFunctional);}bool Ʋ(IMyTerminalBlock Ϊ,bool Ɗ){if(Ϊ==null) return false;try{IMyCubeBlock Αί=Me.CubeGrid.GetCubeBlock(Ϊ.Position).FatBlock;if(Ɗ) return Αί==Ϊ;else return Αί.GetType()==Ϊ.GetType();}catch{return false;}}bool Βθ(bool ΕΑ){if(ǖ&&Ν!=Μ.η)
if(ψ!=-1&&Αώ>=ψ){Φ="Ship too heavy";return true;}if(Ű>=Ч||Ƅ){Ƅ=false;Φ="Ship is full";return true;}return false;}static String Βα(IMyThrust Ы){return Ы.BlockDefinition.SubtypeId;}Vector3 Ń(Vector3 Ј,float[,]Ǫ){return new Vector3(
Ј.X>=0?Ǫ[0,0]:Ǫ[0,1],
Ј.Y>=0?Ǫ[1,0]:Ǫ[1,1],
Ј.Z>=0?Ǫ[2,0]:Ǫ[2,1]);}bool Ʃ(Ψ Ω,String ǔ,out float Ť){Ť=0;int Α=й.IndexOf(ǔ);if(Α==-1||Ω.И==null||Α>=Ω.И.Length)
return false;Ť=Ω.И[Α];if(Ť==-1) return false;return true;}Vector3 ƨ(Vector3 Ј,Ψ Ω){if(Ω!=null){Vector3 Χ=new Vector3();for(int i=0;i<и.Keys.Count;i++){String ǔ=и.Keys.ElementAt(i);float Ȑ=0;if(!Ʃ(Ω,ǔ,out Ȑ))
return Ń(Ј,ȧ);Χ+=Ń(Ј,и.Values.ElementAt(i))*Ȑ;}return Χ;}return Ń(Ј,ȧ);}float ľ(Vector3 Ј,Ψ Ω){return ľ(Ј,new Vector3(),Ω);}float ľ(Vector3 Ј,Vector3 β,Ψ Ω){Vector3 ΔΖ=ƨ(Ј,Ω);Vector3 ΓΫ=ΔΖ+β*Αώ;float Ǐ=(ΓΫ/Ј).AbsMin();return(float)(Ј*Ǐ).Length();}float ȳ(Vector3 ȕ,Vector3 Й,Vector3 β,Ψ Ω)
{if(β.Length()==0f) return 0;Vector3 Γρ=ΑΓ(ȕ,Й,Vector3.Normalize(β));float А=ľ(-Γρ,Ω);return А/β.Length();}void ΑЏ(bool Γζ){float Њ=0;float ť=0.9f;if(Γζ){ψ=-1;Ǖ=0;Ĩ=null;if(ι!=Ξ.Ю&&Π.β.Length()!=0){Њ=ť*ȳ(Π.ώ,Π.ϋ*-1,Π.β,null);if(Њ<ψ||ψ==-1) ψ=Њ;}if(Σ.θ&&Σ.β.Length()!=0)
{Њ=ť*ȳ(Σ.ώ,Σ.ϋ*-1,Σ.β,null);if(Њ<ψ||ψ==-1) ψ=Њ;}return;}if(Ǖ==-1) return;if(Ǖ>=0){int Αχ=0;while(Ǖ<ν.Count){if(Αχ>100) return;Αχ++;Ψ Ω=ν[Ǖ];if(Ω.β.Length()!=0f){Њ=ť*Math.Min(
ȳ(Ω.ώ,Ω.ϋ*-1,Ω.β,Ω),
ȳ(Ω.ώ*-1,Ω.ϋ*-1,Ω.β,Ω));if(Њ<ψ||ψ==-1) ψ=Њ;}else Ĩ=Ω;Ǖ++;}Ǖ=-1;}bool Ũ=true;float щ=0;if(Ĩ!=null){for(int i=0;i<и.Count;i++){String ǔ=и.Keys.ElementAt(i);float[,]Ə=и.Values.ElementAt(i);float Ȑ=0;if(!Ʃ(Ĩ,ǔ,out Ȑ)){Ũ=false;break;}for(int a=0;a<Ə.GetLength(0);a++)
for(int b=0;b<Ə.GetLength(1);b++){float ѕ=Math.Abs(Ə[a,b]*Ȑ);if(ѕ==0) continue;Ũ=true;if(щ==0||ѕ<щ) щ=ѕ;}}}if(!Ũ){for(int a=0;a<ȧ.GetLength(0);a++)
for(int b=0;b<ȧ.GetLength(1);b++){float ѕ=Math.Abs(ȧ[a,b]);if(ѕ==0) continue;if(щ==0||ѕ<щ) щ=ѕ;}}if(щ>0){float Γφ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Њ=Ћ(щ,Γφ);if(Њ>0)
if(Њ<ψ||ψ==-1) ψ=Њ;}}void ΑЋ(Vector3 А,bool τ){if(!τ){for(int i=0;i<ȥ.Count;i++)
ȥ[i].SetValueFloat("Override",0.0f);return;}Vector3 ś=ƨ(А,null);float ΔΨ=Math.Min(1,Math.Abs(Ћ(А.X,ś.X)));float ΔΩ=Math.Min(1,Math.Abs(Ћ(А.Y,ś.Y)));float ΔΪ=Math.Min(1,Math.Abs(Ћ(А.Z,ś.Z)));for(int i=0;i<ȥ.Count;i++){IMyThrust Ы=ȥ[i];Vector3 Ǣ=Βό(ā(Ж,Ы.WorldMatrix.Backward),1);
if(Ǣ.X!=0&&Math.Sign(Ǣ.X)==Math.Sign(А.X))
Ы.SetValueFloat("Override",Ы.MaxThrust*ΔΨ);else if(Ǣ.Y!=0&&Math.Sign(Ǣ.Y)==Math.Sign(А.Y))
Ы.SetValueFloat("Override",Ы.MaxThrust*ΔΩ);else if(Ǣ.Z!=0&&Math.Sign(Ǣ.Z)==Math.Sign(А.Z))
Ы.SetValueFloat("Override",Ы.MaxThrust*ΔΪ);else
Ы.SetValueFloat("Override",0.0f);}}void ΒЄ(bool τ,float ΓЁ,float ǯ,float ǲ,float Ǽ){for(int i=0;i<Αβ.Count;i++){IMyGyro γ=Αβ[i];γ.GyroOverride=τ;if(!τ) γ.GyroPower=100;else γ.GyroPower=ΓЁ;if(!τ) continue;Vector3 ǣ=Ж.WorldMatrix.Forward;Vector3 Ǳ=Ж.WorldMatrix.Right;Vector3 Й=Ж.WorldMatrix.Up;
Vector3 Ǥ=γ.WorldMatrix.Forward;Vector3 Ǧ=γ.WorldMatrix.Up;Vector3 ǥ=γ.WorldMatrix.Left*-1;if(Ǥ==ǣ) γ.SetValueFloat("Roll",Ǽ);else if(Ǥ==(ǣ*-1)) γ.SetValueFloat("Roll",Ǽ*-1);else if(Ǧ==(ǣ*-1)) γ.SetValueFloat("Yaw",Ǽ);else if(Ǧ==ǣ) γ.SetValueFloat("Yaw",Ǽ*-1);else if(ǥ==ǣ) γ.SetValueFloat("Pitch",Ǽ);
else if(ǥ==(ǣ*-1)) γ.SetValueFloat("Pitch",Ǽ*-1);if(ǥ==(Ǳ*-1)) γ.SetValueFloat("Pitch",ǯ);else if(ǥ==Ǳ) γ.SetValueFloat("Pitch",ǯ*-1);else if(Ǧ==Ǳ) γ.SetValueFloat("Yaw",ǯ);else if(Ǧ==(Ǳ*-1)) γ.SetValueFloat("Yaw",ǯ*-1);else if(Ǥ==(Ǳ*-1)) γ.SetValueFloat("Roll",ǯ);
else if(Ǥ==Ǳ) γ.SetValueFloat("Roll",ǯ*-1);if(Ǧ==(Й*-1)) γ.SetValueFloat("Yaw",ǲ);else if(Ǧ==Й) γ.SetValueFloat("Yaw",ǲ*-1);else if(ǥ==Й) γ.SetValueFloat("Pitch",ǲ);else if(ǥ==(Й*-1)) γ.SetValueFloat("Pitch",ǲ*-1);else if(Ǥ==Й) γ.SetValueFloat("Roll",ǲ);else if(Ǥ==(Й*-1)) γ.SetValueFloat("Roll",ǲ*-1);
}}void ΒЍ(){this.Γγ=false;}void ΒЌ(){ć(false,false,false,Ασ,0);ΑЂ=false;}void ΒЃ(Vector3 ί,float Ȣ){ć(true,false,false,ί,Ȣ);}void ć(bool τ,bool Ƈ,bool Ɛ,Vector3 ί,float Ȣ){ć(τ,Ƈ,Ɛ,ί,ί-П,0.0f,Ȣ);}void ć(bool τ,bool Ƈ,bool Ɛ,Vector3 ί,Vector3 ΓЊ,float ΓЋ,float Ȣ){ΑЂ=true;
this.ΔΗ=τ;Ασ=ί;this.Γτ=Ȣ;this.ΓϏ=ΓЋ;this.ΓА=Ƈ;this.ΓЏ=Ɛ;this.Γή=ΓЊ;ΑΪ=Vector3.Distance(ί,П);}void Βύ(){ф.GetBlocksOfType(ȇ,ł);}void ΑА(){if(Ж==null) return;ь=null;float Ģ=0,Ğ=0,ģ=0,ğ=0,Ĥ=0,Ġ=0;List<IMyTerminalBlock>Ƌ=Ɯ(Р,pamTag,true);bool Γϋ=Ƌ.Count==0;
if(Ƌ.Count>0) ь=Ƌ[0];else if(Р.Count>0) ь=Р[0];int Ĕ=0;for(int i=0;i<Р.Count;i++){if(Р[i].WorldMatrix.Forward!=ь.WorldMatrix.Forward){if(Γϋ){Ƃ=2;Φ="Mining direction is unclear!";return;}continue;}Ĕ++;Vector3 π=Ƥ(Ж,Р[i].GetPosition());if(i==0){Ģ=π.X;Ğ=π.X;ģ=π.Y;ğ=π.Y;Ĥ=π.Z;Ġ=π.Z;}
Ğ=Math.Max(π.X,Ğ);Ģ=Math.Min(π.X,Ģ);ğ=Math.Max(π.Y,ğ);ģ=Math.Min(π.Y,ģ);Ġ=Math.Max(π.Z,Ġ);Ĥ=Math.Min(π.Z,Ĥ);}Ǔ=(Ğ-Ģ)*(1-ȫ)+drillRadius*2;ю=(ğ-ģ)*(1-ǧ)+drillRadius*2;if(ь!=null&&ь.WorldMatrix.Forward==Ж.WorldMatrix.Down)
ю=(Ġ-Ĥ)*(1-ǧ)+drillRadius*2;}List<T>Ɯ<T>(List<T>ό,String З,bool ę){List<T>Χ=new List<T>();for(int i=0;i<ό.Count;i++)
if(Ƭ(ό[i],З,ę)) Χ.Add(ό[i]);return Χ;}bool Ƭ<T>(T Ϊ,String З,bool ę){IMyTerminalBlock b=(IMyTerminalBlock) Ϊ;if(ę&&b.CustomName.ToUpper().Contains(З.ToUpper())) return true;if(!ę&&b.BlockDefinition.ToString().ToUpper().Contains(З.ToUpper())) return true;return false;}void ą<T>(List<T>ό,bool τ)
{for(int i=0;i<ό.Count;i++)
ΒЁ((IMyTerminalBlock) ό[i],τ);}void ΒЁ(IMyTerminalBlock Ϊ,bool τ){if(Ϊ==null) return;String ΓΗ=τ?"OnOff_On":"OnOff_Off";var a=Ϊ.GetActionWithName(ΓΗ);a.Apply(Ϊ);}void Œ(bool τ){ƈ=τ;if(!ǒ) return;ą(ΓБ,τ);}void ΒЀ(ChargeMode ή){for(int i=0;i<ΑΚ.Count;i++) ΑΚ[i].ChargeMode=ή;}void ΒЅ(bool ΔΔ)
{for(int i=0;i<ȗ.Count;i++) ȗ[i].Stockpile=ΔΔ;}void ő(List<IMyLandingGear>Ŧ,bool Αν){for(int i=0;i<Ŧ.Count;i++){if(Αν) Ŧ[i].Lock();if(!Αν) Ŧ[i].Unlock();}}void ΒЂ(bool τ){if(ΑΦ==τ) return;List<IMyShipController>ē=new List<IMyShipController>();ф.GetBlocksOfType(ē,ł);if(ē.Count==0) return;
for(int i=0;i<ē.Count;i++)
ē[i].DampenersOverride=τ;ΑΦ=τ;}IMyShipConnector ǽ(MyShipConnectorStatus ΔΓ){for(int i=0;i<Ȏ.Count;i++){if(!Ʋ(Ȏ[i],true)) continue;if(Ȏ[i].Status==ΔΓ) return Ȏ[i];}return null;}void ċ(bool ƃ,bool Ė,float Γα,float ΓΚ){if(ρ==null||Р.Count==0) return;Vector3 ď=new Vector3();int Ĕ=0;
for(int i=0;i<Р.Count;i++){if(Р[i].WorldMatrix.Forward!=ь.WorldMatrix.Forward) continue;Ĕ++;ď+=Р[i].GetPosition();}ď=ď/Ĕ;Vector3 Ǯ=Ƥ(ρ,ď);ρ.Enabled=true;ρ.ShowOnHUD=ƃ;ρ.LeftExtend=(Ė?1:Џ)/2f*Ǔ-Ǯ.X;ρ.RightExtend=(Ė?1:Џ)/2f*Ǔ+Ǯ.X;ρ.TopExtend=(Ė?1:Ў)/2f*ю+Ǯ.Y;
ρ.BottomExtend=(Ė?1:Ў)/2f*ю-Ǯ.Y;ρ.FrontExtend=(ƃ?Л:Γα)-Ǯ.Z;ρ.BackExtend=ƃ?0:ΓΚ+Ǹ*0.75f+Ǯ.Z;ρ.DetectFloatingObjects=true;ρ.DetectAsteroids=false;ρ.DetectLargeShips=true;ρ.DetectSmallShips=true;ρ.DetectStations=true;ρ.DetectOwner=true;ρ.DetectSubgrids=false;
ρ.DetectPlayers=false;ρ.DetectEnemy=true;ρ.DetectFriendly=true;ρ.DetectNeutral=true;}void Ȃ<T>(List<T>ό,bool ΓЍ,float Γί,bool Γι){for(int i=0;i<ό.Count;i++){IMyTextSurface Ȥ=null;if(ό[i]is IMyTextSurface) Ȥ=(IMyTextSurface) ό[i];if(Ȥ!=null){Ȥ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΓЍ) continue;Ȥ.Font="Debug";if(Γι) continue;Ȥ.FontSize=Γί;}}}static float Ћ(float a,float b){if(b==0) return 0;return a/b;}Vector3 Βό(Vector3 v,int š){return new Vector3(Math.Round(v.X,š),Math.Round(v.Y,š),Math.Round(v.Z,š));}Vector3 Ƥ(IMyTerminalBlock Ϊ,Vector3 ΔΦ){return ā(Ϊ,ΔΦ-Ϊ.GetPosition());
}Vector3 ā(IMyTerminalBlock Ϊ,Vector3 ƒ){return Vector3D.TransformNormal(ƒ,MatrixD.Transpose(Ϊ.WorldMatrix));}Vector3 ΑΓ(Vector3 ȕ,Vector3 Й,Vector3 ƒ){MatrixD Ŵ=MatrixD.CreateFromDir(ȕ,Й);return Vector3D.TransformNormal(ƒ,MatrixD.Transpose(Ŵ));}Vector3 ΑΒ(Vector3 ȕ,Vector3 Й,Vector3 π){MatrixD Ŵ=MatrixD.CreateFromDir(ȕ,Й);
return Vector3D.Transform(π,Ŵ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ς(String s){String[]έ=s.Split('|');return new Vector3(float.Parse(Θ(έ,0)),float.Parse(Θ(έ,1)),float.Parse(Θ(έ,2)));}String ΓΕ(Ψ Ω){String ǐ=":";String Χ=σ(Ω.Ρ)+ǐ+σ(Ω.ώ)+ǐ+
σ(Ω.ϋ)+ǐ+σ(Ω.Б)+ǐ+σ(Ω.β);for(int i=0;i<Ω.И.Length;i++)
Χ+=ǐ+Math.Round(Ω.И[i],3);return Χ;}Ψ ΒЏ(String s){String[]έ=s.Split(':');Ψ Ω=new Ψ(
ς(Θ(έ,0)),ς(Θ(έ,1)),
ς(Θ(έ,2)),ς(Θ(έ,3)),
ς(Θ(έ,4)));int Α=5;List<float>Αέ=new List<float>();while(Α<έ.Length){String ī=Θ(έ,Α);float f=0;if(!float.TryParse(ī,out f)) break;Αέ.Add(f);Α++;}Ω.И=Αέ.ToArray();return Ω;}System.Text.StringBuilder Ǵ=new System.Text.StringBuilder();void Λ<T>(T value,bool Γϊ){if(Γϊ) Ǵ.Append('\n');Ǵ.Append(value);}
void Λ<T>(T value){Λ(value,true);}String ȱ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ċ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΓΘ,int Α){String ī=ΓΘ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ī)) return "";
return ī;}void Save(){if(Αϊ||Ν==Μ.Ї){Storage="";return;}Storage=ΑЎ();}String ΑЎ(){Ǵ.Clear();Ǵ.Append(DATAREV+";");Λ(σ(Αω),false);Λ(σ(Σ.ώ));Λ(σ(Σ.Б));Λ(σ(Σ.ϋ));Λ(σ(Σ.β));Λ(σ(Σ.Ρ));Λ(σ(Σ.в));Λ(Σ.θ);Λ(σ(Π.Ρ));Λ(σ(Π.β));Λ(σ(Π.ώ));Λ(σ(Π.ϋ));Λ(σ(Π.Б));Λ(σ(Π.в));Λ(Π.θ);Λ(σ(Ȝ));Λ(σ(Ǭ));Λ(";");
Λ((int) Ν,false);Λ((int) ι);Λ((int) ш);Λ(Ч);Λ(Ш);Λ(Г);Λ(Щ);Λ(с);Λ(ǖ);Λ(я);if(Ν==Μ.η){Λ((int) κ.Љ);Λ(κ.М);Λ(κ.љ);Λ(ȱ(κ.Ф));Λ(ȱ(κ.Ц));Λ((int) λ.Љ);Λ(λ.М);Λ(λ.љ);Λ(ȱ(λ.Ф));Λ(ȱ(λ.Ц));}else{Λ((int) Ǒ);Λ((int) ъ);Λ((int) ȑ);Λ((int) д);Λ((int) ĝ);Λ(Ȫ);Λ(ǒ);Λ(Ș);Λ(Ȓ);Λ(Џ);Λ(Ў);Λ(Л);Λ(İ);Λ(į);Λ(ȫ);Λ(ǧ);
Λ(ѝ);Λ(ќ);Λ(ė);Λ(Ę);Λ(Ț);Λ(г);Λ(ƀ);Λ(ŵ);}Λ(";");for(int i=0;i<й.Count;i++)
Λ((i>0?"|":"")+й[i],false);Λ(";");for(int i=0;i<ν.Count;i++)
Λ(ΓΕ(ν[i]),i>0);Λ(";",false);Λ(ţ+"|"+ĥ,false);String Υ=Ǵ.ToString();Ǵ.Clear();return Υ;}У Βω(){return Βϊ(Storage);}У Βϊ(String Υ){if(Υ=="") return У.Βλ;String[]ǵ=Υ.Split(';');if(Θ(ǵ,0)!=DATAREV) return У.ƽ;int Α=0;try{String[]Η=Θ(ǵ,1).Split('\n');Αω=ς(Θ(Η,Α++));Σ.ώ=ς(Θ(Η,Α++));
Σ.Б=ς(Θ(Η,Α++));Σ.ϋ=ς(Θ(Η,Α++));Σ.β=ς(Θ(Η,Α++));Σ.Ρ=ς(Θ(Η,Α++));Σ.в=ς(Θ(Η,Α++));Σ.θ=bool.Parse(Θ(Η,Α++));Π.Ρ=ς(Θ(Η,Α++));Π.β=ς(Θ(Η,Α++));Π.ώ=ς(Θ(Η,Α++));Π.ϋ=ς(Θ(Η,Α++));Π.Б=ς(Θ(Η,Α++));Π.в=ς(Θ(Η,Α++));Π.θ=bool.Parse(Θ(Η,Α++));Ȝ=ς(Θ(Η,Α++));Ǭ=ς(Θ(Η,Α++));Η=Θ(ǵ,2).Split('\n');Α=0;
Ν=(Μ) int.Parse(Θ(Η,Α++));ι=(Ξ) int.Parse(Θ(Η,Α++));ш=(Ξ) int.Parse(Θ(Η,Α++));Ч=int.Parse(Θ(Η,Α++));Ш=int.Parse(Θ(Η,Α++));Г=int.Parse(Θ(Η,Α++));Щ=int.Parse(Θ(Η,Α++));с=float.Parse(Θ(Η,Α++));ǖ=bool.Parse(Θ(Η,Α++));я=bool.Parse(Θ(Η,Α++));if(Ν==Μ.η){κ.Љ=(ϊ) int.Parse(Θ(Η,Α++));
κ.М=float.Parse(Θ(Η,Α++));κ.љ=float.Parse(Θ(Η,Α++));κ.Ф=Ċ(Θ(Η,Α++));κ.Ц=Ċ(Θ(Η,Α++));λ.Љ=(ϊ) int.Parse(Θ(Η,Α++));λ.М=float.Parse(Θ(Η,Α++));λ.љ=float.Parse(Θ(Η,Α++));λ.Ф=Ċ(Θ(Η,Α++));λ.Ц=Ċ(Θ(Η,Α++));}else{Ǒ=(а) int.Parse(Θ(Η,Α++));ъ=(н) int.Parse(Θ(Η,Α++));ȑ=(К) int.Parse(Θ(Η,Α++));
д=(Ь) int.Parse(Θ(Η,Α++));ĝ=(а) int.Parse(Θ(Η,Α++));Ȫ=bool.Parse(Θ(Η,Α++));ǒ=bool.Parse(Θ(Η,Α++));Ș=bool.Parse(Θ(Η,Α++));Ȓ=bool.Parse(Θ(Η,Α++));Џ=int.Parse(Θ(Η,Α++));Ў=int.Parse(Θ(Η,Α++));Л=int.Parse(Θ(Η,Α++));İ=float.Parse(Θ(Η,Α++));į=float.Parse(Θ(Η,Α++));ȫ=float.Parse(Θ(Η,Α++));
ǧ=float.Parse(Θ(Η,Α++));ѝ=int.Parse(Θ(Η,Α++));ќ=int.Parse(Θ(Η,Α++));ė=int.Parse(Θ(Η,Α++));Ę=int.Parse(Θ(Η,Α++));Ț=int.Parse(Θ(Η,Α++));г=int.Parse(Θ(Η,Α++));ƀ=int.Parse(Θ(Η,Α++));ŵ=float.Parse(Θ(Η,Α++));}Η=Θ(ǵ,3).Replace("\n","").Split('|');й=Η.ToList();Η=Θ(ǵ,4).Split('\n');
ν.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
ν.Add(ΒЏ(Θ(Η,i)));Η=Θ(ǵ,5).Split('|');ţ=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ĥ)) ĥ=-1;}catch{return У.ƛ;}Ɓ=ш;є();return У.ǌ;}const int Δά=64000;void ΒІ(){ј.Clear();for(int i=Ϗ.Count-1;i>=0;i--){String Υ=Ϗ[i].CustomData.ToUpper();bool ū=false;if(Υ=="INSTRUCTIONS"){ū=true;ΑΧ=true;}
if(Υ=="DEBUG") ū=true;if(ū){ј.Add(Ϗ[i]);Ϗ.RemoveAt(i);}}Ȃ(ј,false,1,false);}void ΒЇ(List<IMyTerminalBlock>ΑΜ){Ъ.Clear();for(int i=0;i<ΑΜ.Count;i++){IMyTerminalBlock Ϊ=ΑΜ[i];try{String Αφ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Ϊ.CustomName.IndexOf(Αφ);int ǹ=-1;
if(Α<0||!int.TryParse(Ϊ.CustomName.Substring(Α+Αφ.Length,1),out ǹ)) continue;if(ǹ==-1) continue;ǹ--;IMyTextSurfaceProvider Αψ=(IMyTextSurfaceProvider) Ϊ;if(ǹ<Αψ.SurfaceCount&&ǹ>=0)
Ъ.Add(Αψ.GetSurface(ǹ));}catch{}}}String ŷ="";void Βχ(){String ų="";String ȏ="";ŷ=ΑЍ(false);ų+=ŷ;ȏ+=ŷ;ȏ+=ΒΪ();for(int i=0;i<Ϗ.Count;i++) Ϗ[i].WriteText(ų);for(int i=0;i<Ъ.Count;i++) Ъ[i].WriteText(ų);Echo(ȏ);for(int i=0;i<ј.Count;i++){IMyTextPanel Ѓ=ј[i];String Υ=Ѓ.CustomData.ToUpper();
if(Υ=="DEBUG") Ѓ.WriteText(""+"\n"+"");if(Υ=="INSTRUCTIONS") Ѓ.WriteText(Ƣ());}}string Ƣ(){String Β="";try{float Αξ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ǡ*Αξ)+" / "+(int)(ǫ*Αξ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ǡ*100f,1)+"% / "+Math.Round(ǫ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ȉ,2)+"ms / "+Math.Round(Ŷ,2)+"ms \n";}catch{}for(int i=0;i<Ħ.Count;i++)
Β+=""+Ħ.Keys.ElementAt(i)+" = "+Math.Round(Ħ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ħ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Ββ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ȃ(Ϗ,setLCDFontAndSize,0.9f,false);Ȃ(Ъ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ϗ.Count;i++) Ϗ[i].WriteText(Β+ǩ.ToString());for(int i=0;i<Ъ.Count;i++) Ъ[i].WriteText(Β+ǩ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void Ǉ(String Ѕ,String ί,String њ){try{if(!ȋ) return;Ѕ=BROADCAST_TAG+BROADCAST_SEP+Ɵ()+BROADCAST_SEP+ί+BROADCAST_SEP+Ş+BROADCAST_SEP+њ+BROADCAST_SEP+Ѕ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,Ѕ);}catch(Exception e){ǩ=e;}}String Ɵ(){if(Ν!=Μ.Ї) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool Ƴ(String Ѕ){return Ѕ.StartsWith(BROADCAST_TAG);}bool ω(ref String Υ,out string ȓ,Char ǐ){int Α=Υ.IndexOf(ǐ);ȓ="";if(Α<0) return false;ȓ=Υ.Substring(0,Α);Υ=Υ.Remove(0,Α+1);return true;}bool Βτ(ref String Ѕ,out String э,out String њ){э="";њ="";if(!ȋ) return false;
String З="";if(!ω(ref Ѕ,out З,BROADCAST_SEP)||!Ƴ(З)) return false;if(!ω(ref Ѕ,out э,BROADCAST_SEP)) return false;if(!ω(ref Ѕ,out З,BROADCAST_SEP)||(З!=Ɵ()&&(З!="*"&&Ν!=Μ.Ї))) return false;if(!ω(ref Ѕ,out З,BROADCAST_SEP)||(З!=Ş)) return false;if(!ω(ref Ѕ,out њ,BROADCAST_SEP)) return false;
return true;}void Βζ(){bool ȍ=true;if(Αή){Αή=false;if(Me.CustomData.Contains("Antenna_Name")){Φ="Update custom data";Me.CustomData="";}}String Ȗ=(Ν!=Μ.Ї?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ȗ)
ư(Ȗ);String[]Η=Me.CustomData.Split('\n');ȋ=bool.Parse(ă(Ȳ(Η,"Enable_Broadcast",ref ȍ)));bool Ƒ=false;bool ΑΩ=true;if(ȋ){if(Ν!=Μ.Ї)
ȡ=ă(Ȳ(Η,"Ship_Name",ref ȍ)).Replace(BROADCAST_SELF_ID,"");Ş=ă(Ȳ(Η,"Broadcast_Channel",ref ȍ)).ToLower();ΑΩ=false;if(б==null){б=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);б.SetMessageCallback("");}List<IMyRadioAntenna>ǟ=new List<IMyRadioAntenna>();ф.GetBlocksOfType(ǟ);bool Ś=false;
for(int i=0;i<ǟ.Count;i++)
if(ǟ[i].EnableBroadcasting&&ǟ[i].Enabled){Ś=true;break;}if(ǟ.Count==0) Φ="No Antenna found";else if(!Ś) Φ="Antenna not ready";Ƒ=ǟ.Count==0||!Ś;if(ΑΖ&&!Ƒ&&Ν!=Μ.Ї) Φ="Antenna ok";}else if(Ν==Μ.Ї)
Φ="Offline - Enable in PB custom data";ΑΖ=Ƒ;if(ΑΩ){if(б!=null) this.IGC.DisableBroadcastListener(б);б=null;}}catch{ȍ=false;}if(!ȍ){Φ="Reset custom data";ư(Ȗ);}}void ư(String Ȗ){String Υ=Ȗ+"\n\n"+"Enable_Broadcast="+(Ν==Μ.Ї?"true":"false")+" \n";Υ+=Ν!=Μ.Ї?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Υ+"Broadcast_Channel=#default";}String ă(String џ){int Α=џ.IndexOf("//");if(Α!=-1) џ=џ.Substring(0,Α);String[]έ=џ.Split('=');if(έ.Length<=1) return "";return έ[1].Trim();}String Ȳ(String[]Η,String Γμ,ref bool Ż){foreach(String џ in Η)
if(џ.StartsWith(Γμ)) return џ;Ż=false;return "";}void ΒΕ(string Є){if(Ȕ) χ=new List<μ>();String э="";String њ="";if(!Ƴ(Є)) Βψ(Є);bool ΑϏ=false;int Γχ=50;while(б!=null&&б.HasPendingMessage&&Γχ-->0){MyIGCMessage Ѕ=б.AcceptMessage();String Υ=Ѕ.Data as String;if(Υ==null) continue;
if(Βτ(ref Υ,out э,out њ)&&э!=""&&э!=BROADCAST_SELF_ID){μ Γ=ĺ(э,"");if(Γ==null){Γ=new μ(э);χ.Add(Γ);}Γ.ǜ(њ,false,false,true);ΓΑ(Γ,Υ);ΑϏ=true;}}if(ΑϏ)
χ.Sort(delegate(μ a,μ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ș||Ȕ){if(ħ<=0||Ȕ){Φ="";Ȕ=false;Βώ();Βζ();Αι=DateTime.Now;}}ΒБ();}void ΓΑ(μ Γ,String Υ){Γ.Ů=DateTime.Now;String ȓ="";
ω(ref Υ,out Γ.ĭ,BROADCAST_SEP);ω(ref Υ,out Γ.ΰ,BROADCAST_SEP);if(Γ.ĭ!=VERSION) return;String Ȟ="";String[]ǰ=new string[3];ω(ref Υ,out Ȟ,BROADCAST_SEP);ω(ref Υ,out ǰ[0],BROADCAST_SEP);ω(ref Υ,out ǰ[1],BROADCAST_SEP);ω(ref Υ,out ǰ[2],BROADCAST_SEP);ω(ref Υ,out Γ.Ɖ,BROADCAST_SEP);ω(ref Υ,out Γ.Φ,BROADCAST_SEP);
ω(ref Υ,out ȓ,BROADCAST_SEP);ω(ref Υ,out Γ.ġ,BROADCAST_SEP);String ΑЇ="",Απ="";ω(ref Υ,out ΑЇ,BROADCAST_SEP);ω(ref Υ,out Απ,BROADCAST_SEP);Γ.е.Clear();while(true){String Αζ;if(!ω(ref Υ,out Αζ,BROADCAST_SEP)) break;String[]ě=Αζ.Split('/');if(ě.Count()<3) continue;int ύ=0;if(!int.TryParse(ě[2],out ύ)) continue;
Γ.е.Add(new Э(ě[0],ě[1],ύ,Ā.Βγ));}int Ας=0;if(!int.TryParse(ȓ,out Γ.δ)) Γ.δ=0;if(!int.TryParse(Ȟ,out Ας)) Γ.Ν=Μ.р;if(!float.TryParse(ǰ[0],out Γ.Ρ.X)) Γ.Ρ.X=0;if(!float.TryParse(ǰ[1],out Γ.Ρ.Y)) Γ.Ρ.Y=0;if(!float.TryParse(ǰ[2],out Γ.Ρ.Z)) Γ.Ρ.Z=0;if(!float.TryParse(ΑЇ,out Γ.і)) Γ.і=0;
if(!float.TryParse(Απ,out Γ.Ǎ)) Γ.Ǎ=0;Γ.Ν=(Μ) Ας;Γ.Ţ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Ρ));Γ.Ǝ=0;for(int i=0;i<Γ.е.Count();i++) Γ.Ǝ+=Γ.е[i].ύ;}μ ĺ(String О,String ΰ){О=О.ToUpper();ΰ=ΰ.ToUpper();for(int i=0;i<χ.Count;i++){if(О!=""&&χ[i].О.ToUpper()==О) return χ[i];
if(ΰ!=""&&χ[i].ΰ!=null&&χ[i].ΰ.ToUpper()==ΰ) return χ[i];}return null;}void Ќ(μ Γ,String φ){if(Γ==null){for(int i=0;i<χ.Count;i++)
χ[i].ǜ(φ,true,false,false);Ǉ(φ,"*",φ);}else{Γ.ǜ(φ,true,false,false);Ǉ(φ,Γ.О,φ);}}void Βψ(string Є){if(Є=="") return;var έ=Є.ToUpper().Split(' ');έ.DefaultIfEmpty("");var φ=έ.ElementAtOrDefault(0);var ΑΘ=έ.ElementAtOrDefault(1);String Γθ="Invalid argument: "+Є;if(Ϋ==Ι.ȃ){if(ε!=null)
{switch(φ){case "UP":{if(δ<2) break;if(ε.δ==0){δ=1;return;}Ќ(ε,"UP");return;}case "DOWN":{if(δ<1) break;if(δ==1){Ќ(ε,"MRES");break;}Ќ(ε,"DOWN");break;}case "APPLY":{if(δ<2) break;Ќ(ε,"APPLY");return;}}}}switch(φ){case "UP":this.Βκ(false);return;case "DOWN":this.Βι(false);return;
case "APPLY":this.Ǚ(true);return;}switch(φ){case "CLEAR":ΒΓ();return;case "SENDALL":
if(String.IsNullOrEmpty(ΑΘ)){Φ="Usage: SENDALL <cmd>";return;}Ќ(null,ΑΘ);return;case "SEND":
if(Є.Length<="SEND".Length+1){Φ="Usage: SEND <ship>:<cmd>";return;}ΒϏ(Є.Remove(0,"SEND".Length+1));return;}Φ=Γθ;}void ΒϏ(String ΑΗ){if(ΑΗ=="") return;var έ=ΑΗ.Split(':');if(έ.Length!=2){Φ="Missing separator \":\"";return;}έ.DefaultIfEmpty("");String ȡ=έ.ElementAtOrDefault(0).Trim();
μ Γ=ĺ("",ȡ);if(Γ!=null) Ќ(Γ,έ.ElementAtOrDefault(1).Trim());else Φ="Unknown ship: "+ȡ;}void ΒΓ(){χ.Clear();ε=null;Τ(Ι.Main);δ=0;}void Βώ(){List<IMyTerminalBlock>ΑЀ=new List<IMyTerminalBlock>();ф.GetBlocksOfType(Ϗ,ł);ф.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΑЀ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ϗ=Ɯ(Ϗ,pamTag,true);ΒЇ(ΑЀ);ΒІ();Ȃ(Ϗ,setLCDFontAndSize,1.15f,false);Ȃ(Ъ,setLCDFontAndSize,1.15f,true);String Γε=ƣ(false);String Ατ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ѓ in Ϗ){if(Ѓ.CustomData==""){Ѓ.CustomData=ƣ(true);continue;}
if(!Ѓ.CustomData.Contains(Γε))
if(!Ѓ.CustomData.Contains(Ατ)) Ѓ.CustomData=Ατ+Ѓ.CustomData;}}void ΒБ(){String ȏ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ъ.Count;i++)
Ъ[i].WriteText(ƕ(ο.Ƶ,"0",false));for(int i=0;i<Ϗ.Count;i++){ο ή=ο.р;String Е="";Βυ(Ϗ[i],out ή,out Е);Ϗ[i].WriteText(ƕ(ή,Е,i==0));}Echo(ȏ);for(int i=0;i<ј.Count;i++){IMyTextPanel Ѓ=ј[i];String Υ=Ѓ.CustomData.ToUpper();if(Υ=="DEBUG") Ѓ.WriteText(""+"\n"+"");if(Υ=="INSTRUCTIONS") Ѓ.WriteText(Ƣ());
}}String Ǚ(bool Ε){int ΓΪ=0;return Ǚ(Ε,0,ref ΓΪ,false,1);}String Ǚ(bool Ε,int Γύ,ref int ΓЉ,bool ΑΟ,int ȟ){String Β="";String Ƿ="——————————————————\n";String ΔΕ="--------------------------------------------\n";if(ε==null||Ϋ==Ι.Main||Ϋ==Ι.Ő)
Β+="[PAM]-Controller | "+χ.Count+" Connected ships"+"\n";else Β+=Ƨ(ε)+"\n";Β+=Ƿ;int Α=0;int Δ=δ;ǎ=0;if(Φ!=""){Ζ(ref Β,Δ,Α++,Ε,Φ);}else if(Ϋ==Ι.Main){if(ε==null&&χ.Count>=1) ε=χ[0];String ό="";if(Ζ(ref ό,Δ,Α++,Ε," Send command to all")) Τ(Ι.Ő);int Ƕ=0;for(int s=0;s<χ.Count;s++)
{if(ΑΟ) ό+="\n";μ Γ=χ[s];if(Δ==Α||Δ==Α+1) ε=Γ;if(Α==Δ) Ƕ=ό.Split('\n').Length-1;if(Ζ(ref ό,Δ,Α++,Ε," "+Ƨ(Γ))){ε=Γ;Τ(Ι.ȃ);ε.ǜ("",false,true,false);}if(Α==Δ) Ƕ=ό.Split('\n').Length-1;if(Ζ(ref ό,Δ,Α++,Ε," "+(ΑΟ?"":"  ")+Βή(Γ))){ε=Γ;Τ(Ι.ǈ);}}int ǻ=ȟ-2;
ǻ+=Math.Max(0,(Γύ-1))*ȟ;Β+=ŏ(ǻ,ό,Ƕ,ref ΓЉ);}else if(Ϋ==Ι.Ą||Ϋ==Ι.Ő){Ι ΓΙ=Ϋ==Ι.Ą?Ι.ȃ:Ι.Main;μ ί=null;if(Ϋ==Ι.Ą) ί=ε;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Τ(ΓΙ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+ǚ(ί,"STOP"))){Ќ(ί,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+ǚ(ί,"CONT"))){Ќ(ί,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+ǚ(ί,"HOMEPOS"))){Ќ(ί,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+ǚ(ί,"JOBPOS"))){Ќ(ί,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+ǚ(ί,"FULL"))){Ќ(ί,"FULL");}if(ί!=null&&ί.Ν==Μ.η)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+ǚ(ί,"UNDOCK"))){Ќ(ί,"UNDOCK");}}else if(Ϋ==Ι.ȃ){String Ŝ="";if(Ζ(ref Ŝ,Δ,Α++,Ε," Back")){Τ(Ι.Main);}Β+=Ŝ.Substring(0,Ŝ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Τ(Ι.Ą);}if(!ε.ƴ()&&!ε.Ł()) ǎ++;if(ε.Ł()) Β+="No answer received...";
else Β+=ǝ(ε,ε.ġ);}else if(Ϋ==Ι.ǈ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Τ(Ι.Main);}Β+=ΔΕ;Β+=ǝ(ε,Ʀ(ε));}if(!Ÿ) δ=Math.Min(ǎ-1,δ);return Β;}String Ƨ(μ Γ){if(Γ.ĭ!=VERSION) return Γ.ΰ+": Different version!";return Γ.ΰ+": "+ǝ(Γ,Γ.Ɖ)+" "+Γ.Ţ+"m";}String Βή(μ Γ){String s=Ō("",Ћ(Γ.і,Γ.Ǎ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.е.Count;i++){if(i>=5) break;Э Х=Γ.е[i];s+=ΑΔ(ĵ(ΒΫ(Х)),3)+" "+Ɲ(Х.ύ)+" ";}return s;}String ΒΫ(Э Х){if(Х.Ё.ToUpper()=="ORE"||Х.Ё.ToUpper()=="INGOT"){String ΑΝ=ΒΩ(Х.ΰ.ToUpper());if(ΑΝ!="") return ΑΝ;}return Х.ΰ;}String ǝ(μ Γ,String Β){if(Γ.ƴ()) return "No signal...("+Ľ((int)(DateTime.Now-Γ.Ů).TotalSeconds)+")";
return Β;}String ƣ(bool ΔΥ){String Β="";if(ΔΥ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Βυ(IMyTerminalBlock Ѓ,out ο ή,out String Е){bool Ż=true;String Ȟ=ă(Ȳ(Ѓ.CustomData.Split('\n'),"mode",ref Ż)).ToUpper();String[]έ=Ȟ.Split(':');String ȝ=έ.Length>0?έ[0].Trim():"";Е=έ.Length>1?έ[1].Trim():"";
ή=ο.р;if(ȝ=="MAIN") ή=ο.ƶ;else if(ȝ=="MAINX") ή=ο.Ʒ;else if(ȝ=="MENU") ή=ο.Ņ;else if(ȝ=="INVENTORY") ή=ο.ŀ;else if(ȝ=="DEBUG") ή=ο.ķ;if(ή==ο.р) Е=Ȟ;}String ƕ(ο ή,String Е,bool Γέ){int ț=15;if(Γέ){ΑΠ=đ;đ=0;ΑΡ=Ē;Ē=0;}String Β="";if(ή==ο.ƶ){int Ђ=0;Β=Ǚ(false,ΑΠ,ref ΓΤ,true,ț);
if(!int.TryParse(Е,out Ђ)) return Β;đ=Math.Max(Ђ,đ);Ђ--;return ǂ(Ђ,ț,Β);}else if(ή==ο.Ʒ){int Ђ=0;Β=Ǚ(false,ΑΡ,ref ΓΥ,false,ț);if(!int.TryParse(Е,out Ђ)) return Β;Ē=Math.Max(Ђ,Ē);Ђ--;return ǂ(Ђ,ț,Β);}if(ή==ο.Ƶ){return Ǚ(false,0,ref ΓΦ,true,ț);}else if(ή==ο.ŀ||ή==ο.Ņ||ή==ο.ķ)
{μ Γ=ε;if(Е!=""){Γ=ĺ("",Е);if(Γ==null) return "Unknown ship: "+Е;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ƿ="—————————————————————\n";String ƍ="";String ŝ="";if(ή==ο.Ņ){ƍ="Menu";ŝ=ǝ(Γ,Γ.ġ);}else if(ή==ο.ŀ){ƍ="Inventory";ŝ=ǝ(Γ,Ʀ(Γ));}
else if(ή==ο.ķ){return ǝ(Γ,Βί(Γ));}Β+=Γ.ΰ+" - "+Γ.Ţ+"m | "+ĵ(""+ƍ)+"\n"+Ƿ;Β+=ŝ;return Β;}return "Unknown command: "+Е;}string Βί(μ Γ){String s="";s+=Γ.ĭ+"\n";s+=Γ.ΰ+"\n";s+=Γ.Ρ.X+"\n";s+=Γ.Ρ.Y+"\n";s+=Γ.Ρ.Z+"\n";s+=Γ.Ɖ+"\n";s+=Γ.Φ+"\n";s+=Γ.δ+"\n";
s+=Γ.ġ.Length+"\n";s+=Γ.і+"\n";s+=Γ.Ǎ+"\n";s+=Γ.е.Count()+"\n";return s;}string Ʀ(μ Γ){String s="";s+=Ō("All",Ћ(Γ.і,Γ.Ǎ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.е.Count();i++){Э Х=Γ.е[i];s+=Ō(Х.ΰ,Х.ύ,Γ.Ǝ,30,8,10)+"\n";}return s;}ξ ΒΥ(μ Γ,bool Ȍ,string φ)
{ξ Χ=ξ.None;if(Γ==null){for(int i=0;i<χ.Count;i++){ξ υ=χ[i].ǜ(φ,false,Ȍ,false);if(υ==ξ.ō) Χ=υ;if(υ==ξ.ň) return υ;if(υ==ξ.ȁ) return υ;}return Χ;}else return Γ.ǜ(φ,false,Ȍ,false);}String ǚ(μ Γ,String φ){ξ υ=ΒΥ(Γ,false,φ);if(υ==ξ.ō) return "received!";if(υ==ξ.ȁ) return "pending...";
if(υ==ξ.ň) return "no answer!";return "";}String ŏ(int ǻ,String Β,int Ƕ,ref int з){String[]Η=Β.Split('\n');if(Ƕ>=з+ǻ-1) з++;з=Math.Min(Η.Count()-1-ǻ,з);if(Ƕ<з+1) з--;з=Math.Max(0,з);String Χ="";for(int i=0;i<ǻ;i++){int Αλ=i+з;if(Αλ>=Η.Count()) break;Χ+=Η[Αλ]+"\n";
}return Χ;}String ǂ(int Ђ,int ȟ,String Β){String[]Η=Β.Split('\n');int ΔΑ=Ђ*ȟ;int Γά=(Ђ+1)*ȟ;String Χ="";for(int i=ΔΑ;i<Γά;i++){if(i>=Η.Count()) break;Χ+=Η[i]+"\n";}return Χ;}String ĵ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();
}String ΑΔ(String s,int Αο){if(s=="") return s;if(s.Length>Αο) s=s.Substring(0,Αο-1)+".";return s;}string Ō(String ў,float value,float Γσ,int ΑΙ,int Αθ,int Γό){float Ǐ=Ћ(value,Γσ)*ΑΙ;String Ď="[";for(int i=0;i<ΑΙ;i++){if(i<=Ǐ) Ď+="|";else Ď+="'";}Ď+="]";return Ď+" "+ΑΔ(ĵ(ў),Αθ).PadRight(Αθ)+"".PadRight(Γό)+Ɲ(value);
}String Ɲ(float ύ){if(ύ>=1000000) return Math.Round(ύ/1000000f,ύ/1000000f<100?1:0)+"M";if(ύ>=1000) return Math.Round(ύ/1000f,ύ/1000f<100?1:0)+"K";return ""+Math.Round(ύ);}String Ľ(int Ƞ){if(Ƞ>=60*60) return Math.Round(Ƞ/(60f*60f),1)+" h";if(Ƞ>=60) return Math.Round(Ƞ/60f,1)+" min";
return ""+Ƞ+" s";}List<IMyCameraBlock>Δς=new List<IMyCameraBlock>();List<long>ΔЋ=new List<long>();bool Γώ=false;int ΓΞ=0;int Δρ=0;DateTime Δο=DateTime.Now;String ΓΜ="";void ΒΒ(){Γώ=false;ΓΞ=0;ΓΜ="";}const String Δδ="[PAMTFC]";const float Δε=5f;const float Δί=2f;class ΑΑ
{public long О;public Vector3 Ѐ,ΕΩ;public float Δχ;public String ΔЂ="";public int Αό=-1;public DateTime ΕΜ;}IMyBroadcastListener ΕΞ=null;long ΔЉ=0;List<ΑΑ>ΕΝ=new List<ΑΑ>();Vector3 ΕΟ=new Vector3();float ΕΠ=-1;int ΕΡ=0;String ţ="";int ĥ=-1;bool ΓΧ=false;Vector3 Δτ=new Vector3();
void ΒЈ(string Є){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}