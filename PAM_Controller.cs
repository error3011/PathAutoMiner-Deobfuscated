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
public enum Ν{ǐ,Ɣ,ȃ,А,ν}public enum Λ{Ǡ,Ɔ,ΑЍ,ƙ,ΒΗ,ȭ,Ǎ,
ƅ,ΓЍ,ǿ,Αλ,Γη,Αδ,
Δυ,Ā,ȳ,ſ,ΕΛ,ΓЈ,
ΕΙ,ΕΚ,ΓЉ,ΓЊ,ā}public enum Τ{ь,Ɩ,ǟ,Αμ,Ĳ,ΑЎ,ΑЏ,Į,į}public enum ќ{None,ſ,Ǡ,Γώ}public enum Ι{Main,ΒΕ,ȯ,Ȯ,ơ,ı,
Ż,ΑЁ,ΑЂ,ΑϏ,Ȳ,ΒΙ,
Ļ,Ɯ,Ɵ,ΒΜ,Ŀ}public enum Ǐ{ΒΘ,Γϊ,ΔІ,ΔΪ,ν}public enum Р{Ł,Γυ}public enum џ{ΒΖ,ΓЌ,ΔЉ,Ɗ}public enum Я{ΑБ,Γύ,Γό,ΓЄ,ΓЃ,ΔΨ,ΔΧ}public enum ш{Ƃ,Γλ,Αε}public enum Ķ{ΔΦ,Ζς,Γθ}public enum Ѓ{ΒΑ,ΒΒ,ΒΓ,Δλ,Δκ,
Δε,Δζ,Δδ,Δθ,
Δι,Δη}public enum Υ{й,э,и,İ,ż,Ž}public enum Ζυ{Ζρ,Ζμ,Ζο}public enum к{ΒΟ,Αν,Δγ,ΑА}public enum ζ{None,Ȅ,Ȱ,Ĺ}public enum ς{Ƥ,ź,Ƒ,Ɗ,ΑЋ,Ɠ,ΑЌ,ΕΑ,ƕ}public enum ψ{ǐ,ΑЉ,ΑЈ,ƒ,Ƌ,Ɓ,ΑЊ}class Ω{public bool κ=false;public Vector3 Ρ=new Vector3();
public Vector3 Ϗ=new Vector3();public Vector3 Є=new Vector3();public Vector3 Л=new Vector3();public Vector3 ξ=new Vector3();public Vector3 Ф=new Vector3();public float ΕЁ=0;public float Ηώ=0;public float[]Э=null;public Ω(){}public Ω(Ω Д){κ=Д.κ;Ρ=Д.Ρ;Ϗ=Д.Ϗ;Є=Д.Є;Л=Д.Л;ξ=Д.ξ;
Ф=Д.Ф;Э=Д.Э;}public Ω(Vector3 Ρ,Vector3 Є,Vector3 Ϗ,Vector3 Л,Vector3 ξ){this.Ρ=Ρ;this.Ϗ=Ϗ;this.Є=Є;this.Л=Л;this.ΕЁ=0;this.ξ=ξ;}public void Ζξ(List<IMyThrust>Ņ,List<string>њ){Э=new float[њ.Count];for(int i=0;i<Э.Length;i++)
Э[i]=-1;for(int i=0;i<Ņ.Count;i++){string ǻ=ΔΝ(Ņ[i]);int Α=њ.IndexOf(ǻ);if(Α!=-1)
Э[Α]=О(Ņ[i].MaxEffectiveThrust,Ņ[i].MaxThrust);}}}class Δξ{public Ω α=null;public List<Vector3>Ην=new List<Vector3>();public float ΖБ=0;public float ΗЂ=0;public float Ζύ=0;public float ΗΙ=0;public Vector3 ΗΚ=new Vector3();}class ł{public Ѓ М=Ѓ.ΒΑ;public float б=0;public float Ǩ=0;
public string н="";public string р="";DateTime ΒЋ;public bool ΕЏ=false;private bool Ư=false;public bool Γφ(bool ΑΕ){if(ΑΕ){Ǩ=0;Ư=false;return false;}Ư=true;return Ǩ>б;}public void Ζσ(){Γφ(true);ΕЏ=false;}public void Ζω(){if(Ư)
if((DateTime.Now-ΒЋ).TotalSeconds>1){Ǩ++;ΒЋ=DateTime.Now;}}public bool Αύ(){switch(М){case Ѓ.ΒΒ:return true;case Ѓ.ΒΓ:return true;}return false;}}class ъ{public String θ="";public String Њ="";public int І=0;public Ķ ǃ=Ķ.Γθ;public ъ(String θ,String Њ,int І,Ķ ǃ){this.θ=θ;this.Њ=Њ;
this.І=І;this.ǃ=ǃ;}}class ΓϏ{public ΓϏ(Vector3 ά,float Г){this.ά=ά;this.Г=Г;}public Vector3 ά;public float Г;}class έ{public DateTime ǀ;public DateTime Βε;public String Ο="";public String θ="";public String ŵ="";public String Ψ="";public String ΑΥ="";public String Ũ="";public String ΑΒ="";
public Vector3 Ρ=new Vector3();public List<ъ>є=new List<ъ>();public Ν Π=Ν.ǐ;public ζ ǒ;public float ǰ;public float Ǥ;public int π;public int Ʊ=0;public int ΑΫ=0;public bool Ə(){return(DateTime.Now-ǀ).TotalSeconds>10;}public bool ƌ(){if(ǒ!=ζ.Ȅ) return false;return(DateTime.Now-Βε).TotalSeconds>=4;
}public έ(String Ο){this.Ο=Ο;}public ζ ǎ(String ι,bool ΖΚ,bool ѐ,bool ΕΧ){if(ι==""&&!ѐ) return ζ.None;if(ǒ==ζ.Ȅ&&ƌ())
ǒ=ζ.Ĺ;if(ΖΚ){ΑΒ=ι;ǒ=ζ.Ȅ;Βε=DateTime.Now;return ǒ;}else if(ѐ){ΑΒ="";ǒ=ζ.None;}else if(ΑΒ==ι){if(ΕΧ) ǒ=ζ.Ȱ;return ǒ;}return ζ.None;}}public String ΔΔ(String Εω){switch(Εω){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Й;Vector3 ΓΞ=new Vector3();Vector3 з=new Vector3();Vector3 Ει=new Vector3();Vector3 Εό=new Vector3();Vector3 Εβ=new Vector3();DateTime ΒЊ=new DateTime();
bool đ=true;int ΑΨ=0;bool Ę=false;bool Ǭ=false;bool ΕЅ=true;bool ΗЄ=false;bool ΗΞ=false;bool Ευ=false;float ΖΜ=0;float ΓΧ=0;int ΖΔ=0;int ΗϏ=0;int Ů=0;int ΖЄ=0;float ȕ=0;float ȉ=0;double ǈ=0;float Ą=0;List<int>ȑ=new List<int>();List<int>Ȣ=new List<int>();Ν Π=Ν.ǐ;
int ΑΛ=0;float Ǻ=0;float ǹ=0;float Ȧ=0;float ΓΦ=0,ΓΥ=0;IMyRemoteControl Ј;IMySensorBlock ό;List<IMyTimerBlock>Γί=new List<IMyTimerBlock>();List<IMyShipConnector>Ċ=new List<IMyShipConnector>();List<IMyThrust>Ħ=new List<IMyThrust>();List<IMyGyro>Βϋ=new List<IMyGyro>();List<IMyTerminalBlock>Ю=new List<IMyTerminalBlock>();
List<IMyLandingGear>ƿ=new List<IMyLandingGear>();List<IMyReactor>ΑΔ=new List<IMyReactor>();List<IMyConveyorSorter>ΖΟ=new List<IMyConveyorSorter>();List<IMyGasTank>Ė=new List<IMyGasTank>();List<IMyTerminalBlock>Ă=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ΖІ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ζό=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΒΫ=new List<IMyBatteryBlock>();List<IMyTextPanel>Ї=new List<IMyTextPanel>();List<IMyTextSurface>х=new List<IMyTextSurface>();List<IMyTextPanel>Ǧ=new List<IMyTextPanel>();IMyTerminalBlock Ǜ=null;Ω Σ=new Ω();Ω ΒЀ=new Ω();
Ω ΒЇ=new Ω();bool Ǟ=false;bool Ī=false;bool ī=false;bool ΒЅ=false;List<String>њ=new List<string>();List<Ω>χ=new List<Ω>();List<Ω>Ŷ=new List<Ω>();int ΗΑ=0;double Ζγ=0;Ω Φ=new Ω();Vector3 ĝ;Vector3 Ȗ;Τ τ=Τ.ь;Р Ȓ=Р.Ł;int е=0;int д=0;double Ţ=0;bool Βλ=false;ł υ=new ł();
ł φ=new ł();џ ǚ=џ.ΒΖ;bool Ĭ=true;Р Ƿ=Р.Ł;int У=3;int Т=3;int а=30;ш ѓ=ш.Ƃ;bool ǽ=true;bool ė=true;bool ď=true;bool Ǹ=false;Я Ď=Я.ΑБ;bool ǝ=true;float с=90;float т=20;float Щ=5;float у=20;float Ǒ=0.70f;float Ÿ=1.50f;float ŷ=2.50f;float ĭ=0f;float Ĕ=0f;Λ ŏ;
Τ ǘ;Λ Ĥ;Δξ ΗΫ=null;int Ŝ=0;int ŝ=0;int м=0;int ě=0;int ΑΘ=30;int Ηέ=0;bool ΖΠ=true;Vector3 ΗΧ;double Βπ=0;bool ΓΠ=false;int Ζδ=0;int Ηθ=0;int Ζώ=0;int ΗΪ=0;int ΗΨ=0;Vector3 Ηά=new Vector3();float ΗΤ=0;float Ηι=0;int Ǽ=0;Ω ů=null;float ΗΥ=0;float ΖϏ=0;float Βκ=0;
float Ǉ=0;bool ΗΟ=false;bool Ηύ=false;bool ΗЁ=false;bool ΗΜ=false;bool ΖЁ=false;DateTime ΗΘ=new DateTime();Ω ΖЏ=null;Ω Εΰ=null;float Βά=0;ќ ą;float ΒЄ=0;float Γβ=0;String ΕϏ="";float ǂ=0;float Ǥ=0;float ǰ=0;List<ъ>ΗΠ=new List<ъ>();float Ѐ=0;bool ΑΞ=false;bool ΖΧ=false;
bool ΖΝ=false;bool Γή=false;bool ΖΞ=false;float ΕЀ=0;float ΕЍ=0;Vector3 Εη=new Vector3();Vector3 ΓΕ=new Vector3();Vector3 Ησ=new Vector3(1,1,1);float Ηρ=1;Vector3 Ης=new Vector3();bool ΗΝ=false;bool Εξ=false;bool ΗΦ=false;bool Ζϋ=false;float ΖЎ=0;float Ζϊ=2;Vector3 Ημ;
Vector3 Ηλ;Vector3 Ηκ;float[,]ĩ=new float[3,2];Dictionary<String,float[,]>љ=new Dictionary<string,float[,]>();Dictionary<String,float>ΗЃ=new Dictionary<String,float>();int ΖЀ=0;Dictionary<String,float[]>ŭ=new Dictionary<string,float[]>();float Ηή;bool Βν=false;int ņ=0;int ǔ=0;
int ş=0;int ǫ=0;String Ψ="";int[]ΓΒ=new int[Enum.GetValues(typeof(Ι)).Length];bool Ǌ=false;int Ǳ=0;int π=0;Ι ΰ=Ι.Main;int ΖΨ=0,ΖΩ=0,ΖΪ=0,ΖΫ=0;int ΖΕ=0,ΖΖ=0;bool ć=false;bool ΒΧ=false;String Ģ="";String ƪ="";IMyBroadcastListener я=null;bool Βφ=true;List<έ>σ=null;
έ ρ=null;int ŋ=0,Ō=0;int Βη=0,Βθ=0;int Εέ=0,Εή=0,Εί=0;String Εϊ="";Exception ȓ=null;bool ΓΟ=false;Τ ΑΙ=Τ.ь;bool Βμ=false;bool ΗΣ=true;bool Ηί=false;String Ζα="";bool Γα=false;bool ΑΣ=true;int Ƽ=0;int[]Ηβ=null;Vector3 Ƭ=new Vector3();bool Βω=false;bool Ǝ(IMyTerminalBlock ή)=>ή.CubeGrid==Me.CubeGrid;
Program(){Й=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Αώ(Me,controllerTag,true))
Π=Ν.А;Χ(Ι.Main);if(Π!=Ν.А){Ψ="Welcome to [PAM]!";к ΑΖ=Δσ();if(Π==Ν.ǐ){List<IMyShipDrill>Βτ=new List<IMyShipDrill>();List<IMyShipGrinder>Βϊ=new List<IMyShipGrinder>();Й.GetBlocksOfType(Βτ,q=>q.CubeGrid==Me.CubeGrid);Й.GetBlocksOfType(Βϊ,q=>q.CubeGrid==Me.CubeGrid);if(Βτ.Count>0)
{Π=Ν.Ɣ;Ψ="Miner mode enabled!";}else if(Βϊ.Count>0){Π=Ν.ȃ;Ψ="Grinder mode enabled!";}else{Π=Ν.ν;ΒΛ(Ǐ.ν);}}if(ΑΖ==к.ΒΟ) ΕЅ=false;if(ΑΖ==к.Αν) Ψ="Data restore failed!";if(ΑΖ==к.ΑА) Ψ="New version, wipe data";}}void Main(string Ѝ,UpdateType Ζή){try{if(ȓ!=null){if(Ѝ.Trim().ToUpper()=="RETRY")
{ȓ=null;đ=true;ΖΔ=0;if(Π!=Ν.А) Ǣ();Ψ="Restarted after error";}else{ΔΡ();Δπ();return;}}Ǭ=(Ζή&UpdateType.Update10)!=0;if(Ǭ) ΑΨ++;Ę=ΑΨ>=10;if(Ę) ΑΨ=0;if(Ǭ){ņ++;if(ņ>4) ņ=0;Ů=(int) Math.Max(0,10-(DateTime.Now-ΒЊ).TotalSeconds);}if(Ѝ!="") Εϊ=Ѝ;if(Π!=Ν.А)
ΔЅ(Ѝ);else
Γϋ(Ѝ);Ǌ=false;try{int Βι=Runtime.CurrentInstructionCount;float Ǵ=О(Βι,Runtime.MaxInstructionCount);if(Ǵ>0.90) Ψ="Max. instructions >90%";if(Ǵ>ȕ) ȕ=Ǵ;if(Βν){ȑ.Add(Βι);while(ȑ.Count>10) ȑ.RemoveAt(0);ȉ=0;for(int i=0;i<ȑ.Count;i++) ȉ+=ȑ[i];ȉ=О(О(ȉ,ȑ.Count),Runtime.MaxInstructionCount);
double ǁ=Runtime.LastRunTimeMs;if(Ευ&&ǁ>ǈ) ǈ=ǁ;Ȣ.Add((int)(ǁ*1000f));while(Ȣ.Count>10) Ȣ.RemoveAt(0);Ą=0;for(int i=0;i<Ȣ.Count;i++) Ą+=Ȣ[i];Ą=О(Ą,Ȣ.Count)/1000f;}}catch{ȕ=0;}}catch(Exception e){ȓ=e;}}String ΔΕ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Π!=Ν.ν)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Χ(Ι Ћ){ΓΒ[(int) ΰ]=π;π=ΓΒ[(int) Ћ];if(Ћ==Ι.Ȳ) π=0;ΰ=Ћ;if(Π!=Ν.А)
ń(ΰ==Ι.ȯ,false,0,0);Ǌ=true;}void Δα(bool Ǆ){if(π>0) π--;else if(Ǆ) π=Ǳ-1;}void Δΰ(bool Ǆ){if(π<Ǳ-1) π++;else if(Ǆ) π=0;}String ΒΛ(Ǐ δ){switch(δ){case Ǐ.ΒΘ:Ψ="Job is running";break;case Ǐ.Γϊ:Ψ="Connector not ready!";break;case Ǐ.ΔІ:Ψ="Ship modified, path outdated!";break;
case Ǐ.ΔΪ:Ψ="Interrupted by player!";break;case Ǐ.ν:Ψ="Shuttle mode enabled!";break;}return "";}String ΔΜ(Р ά){switch(ά){case Р.Ł:return "Top-Left";case Р.Γυ:return "Center";default:return "";}}String ΔΑ(ш η){switch(η){case ш.Γλ:return "Auto"+(Π==Ν.Ɣ?" (Ore)":"");case ш.Αε:return "Auto (+Stone)";
case ш.Ƃ:return "Default";default:return "";}}String ΔΙ(Τ ώ){switch(ώ){case Τ.ь:return "No job";case Τ.Ɩ:return "Job paused";case Τ.ǟ:return "Job active";case Τ.Į:return "Job active";case Τ.į:return "Job active";case Τ.Αμ:return "Job done";case Τ.Ĳ:return "Job changed";case Τ.ΑЎ:return "Move home";
case Τ.ΑЏ:return "Move to job";}return "";}String Ασ(џ ΕΣ){switch(ΕΣ){case џ.ΒΖ:return "Return home";case џ.ΓЌ:return "Fly to job pos";case џ.ΔЉ:return "Stop";case џ.Ɗ:return "Ignore";}return "";}String ΔΓ(Я η){switch(η){case Я.ΑБ:return "Off";case Я.ΓЄ:return "Drop pos (Stone) ";case Я.ΓЃ:return "Drop pos (Sto.+Ice)";
case Я.Γύ:return "Cur. pos (Stone)";case Я.Γό:return "Cur. pos (Sto.+Ice)";case Я.ΔΨ:return "In motion (Stone)";case Я.ΔΧ:return "In motion (Sto.+Ice)";}return "";}String Αρ(ќ ώ){switch(ώ){case ќ.None:return "No batteries";case ќ.ſ:return "Charging";case ќ.Γώ:return "Discharging";}return "";}
String Αχ(Λ ώ){String ΗΡ=Π==Ν.ν?"target":"job";switch(ώ){case Λ.Ǡ:return "Idle";case Λ.Ɔ:return "Flying to XY position";case Λ.ΑЍ:return Π==Ν.ȃ?"Grinding":"Mining";case Λ.ƙ:return "Returning";case Λ.ΓЉ:return "Flying to drop pos";case Λ.ΒΗ:return "Returning to dock";case Λ.ȭ:return "Flying to dock area";
case Λ.Ǎ:return "Flying to job area";case Λ.ƅ:return "Flying to path";case Λ.ΓЍ:return "Flying to job position";case Λ.ǿ:return "Approaching dock";case Λ.Αλ:return "Docking";case Λ.Γη:return "Aligning";case Λ.Αδ:return "Aligning";case Λ.Δυ:return "Retry docking";case Λ.Ā:return "Unloading";case Λ.ā:return Ζα;
case Λ.ȳ:return "Undocking";case Λ.ſ:return "Charging batteries";case Λ.ΕΛ:return "Waiting for uranium";case Λ.ΓЈ:return "Filling up hydrogen";case Λ.ΕΙ:return "Waiting for ejection";case Λ.ΕΚ:return "Waiting for ejection";case Λ.ΓЊ:return "Flying to drop pos";}return "";}String Αό(Ѓ М){switch(М)
{case Ѓ.ΒΑ:return "On \"Undock\" command";case Ѓ.ΒΒ:return "On player entered cockpit";case Ѓ.Δλ:return "On ship is full";case Ѓ.Δκ:return "On ship is empty";case Ѓ.ΒΓ:return "On time delay";case Ѓ.Δζ:return "On batteries empty(<25%)";case Ѓ.Δδ:return "On batteries empty(=0%)";case Ѓ.Δε:return "On batteries full";
case Ѓ.Δι:return "On hydrogen empty(<25%)";case Ѓ.Δη:return "On hydrogen empty(=0%)";case Ѓ.Δθ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int ΓΤ,int Ǔ,bool Ε,String ǭ){Ǳ+=1;if(ΓΤ==Ǔ) ǭ=">"+ǭ+(ņ>=2?" .":"");else ǭ=" "+ǭ;Β+=ǭ+"\n";return ΓΤ==Ǔ&&Ε;}String Γν(bool Ε)
{int Α=0;int Δ=π;Ǳ=0;String ȥ="———————————————\n";String Ȩ="--------------------------------------------\n";String Β="";Β+=ΔΙ(τ)+" | "+(Σ.κ?"Ready to dock":"No dock")+"\n";Β+=ȥ;double ΓΫ=Math.Max(Math.Round(this.Βπ),0);if(ΰ==Ι.Main){bool Ǖ=Π==Ν.ν;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) ΔЇ();
if(Π==Ν.Ɣ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Χ(Ι.ȯ);if(Π==Ν.ȃ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Χ(Ι.ȯ);if(Π==Ν.ν)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Χ(Ι.Ɵ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) Αι();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) ΓЋ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) ΓЎ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(Ǖ) Χ(Ι.Ŀ);else Χ(Ι.ı);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Χ(Ι.ΑЁ);if(Π!=Ν.ν)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Χ(Ι.ΑϏ);}else if(ΰ==Ι.ȯ){double Ζε=Math.Round(У*Ǻ,1);double Ερ=Math.Round(Т*ǹ,1);String λ="";if(Ζ(ref λ,Δ,Α++,Ε," Start new job!")) ΒΝ();if(Ζ(ref λ,Δ,Α++,Ε," Change current job")){ƣ(false,false);Χ(Ι.Main);}if(Ζ(ref λ,Δ,Α++,Ε," Width + (Width: "+У+" = "+Ζε+"m)")){Ǿ(ref У,5,20,1);ȇ(true);}
if(Ζ(ref λ,Δ,Α++,Ε," Width -")){Ǿ(ref У,-5,20,-1);ȇ(true);}if(Ζ(ref λ,Δ,Α++,Ε," Height + (Height: "+Т+" = "+Ερ+"m)")){Ǿ(ref Т,5,20,1);ȇ(true);}if(Ζ(ref λ,Δ,Α++,Ε," Height -")){Ǿ(ref Т,-5,20,-1);ȇ(true);}if(Ζ(ref λ,Δ,Α++,Ε," Depth + ("+(ѓ==ш.Ƃ?"Depth":"Min")+": "+а+"m)")){Ǿ(ref а,5,50,2);ȇ(true);}
if(Ζ(ref λ,Δ,Α++,Ε," Depth -")){Ǿ(ref а,-5,50,-2);ȇ(true);}if(Ζ(ref λ,Δ,Α++,Ε," Depth mode: "+ΔΑ(ѓ))){ѓ=ў(ѓ);}if(Ζ(ref λ,Δ,Α++,Ε," Start pos: "+ΔΜ(Ƿ))){Ƿ=ў(Ƿ);}if(Π==Ν.ȃ&&ѓ==ш.Αε) ѓ=ў(ѓ);Β+=ƚ(8,λ,Δ,ref ΖΕ);}else if(ΰ==Ι.Ɵ){float[]Βξ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.ΒΜ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αό(υ.М))) υ.М=ў(υ.М);if(!υ.Αύ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ƈ((int) υ.б))) υ.б=Ακ(υ.б,Βξ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αό(φ.М))) φ.М=ў(φ.М);
if(!φ.Αύ()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ƈ((int) φ.б))) φ.б=Ακ(φ.б,Βξ);}else if(ΰ==Ι.ΒΜ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ΒΝ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Ɵ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(υ.н!=""?υ.н:"-"))) υ.н=ĸ(ref ΖΨ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(υ.р!=""?υ.р:"-"))) υ.р=ĸ(ref ΖΪ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(φ.н!=""?φ.н:"-"))) φ.н=ĸ(ref ΖΩ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(φ.р!=""?φ.р:"-"))) φ.р=ĸ(ref ΖΫ);
}else if(ΰ==Ι.Ȯ){String ΒЎ=с+" %";if(Ę) ǔ++;if(ǔ>1){ǔ=0;ş++;if(ş>1) ş=0;}bool[]ΑΟ=new bool[]{ΑΔ.Count==0,ą==ќ.None,Ė.Count==0};int Γΰ=0;while(true){Γΰ++;ǫ++;if(ǫ>ΑΟ.Length-1) ǫ=0;if(Γΰ>=ΑΟ.Length) break;if(!ΑΟ[ǫ]) break;}bool Ǖ=Π==Ν.ν;if(!Ǖ&&ǽ&&Ѐ!=-1&&ş==0)
ΒЎ=Ѐ<1000000?Math.Round(Ѐ)+" Kg":Math.Round(Ѐ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){Ǣ();Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!Ǖ) Χ(Ι.ı);else Χ(Ι.Ŀ);if(!Ǖ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) Γζ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) Γα=true;Β+=Ȩ;if(!Ǖ) Β+="Progress: "+Math.Round(Ţ,1)+" %\n";Β+="State: "+Αχ(ŏ)+" "+ΓΫ+"m \n";Β+="Load: "+ǂ+" % Max: "+ΒЎ+" \n";if(ǫ==0) Β+="Uranium: "+(ΑΔ.Count==0?"No reactors":Math.Round(Γβ,1)+"Kg "+(Щ==-1?"":" Min: "+Щ+" Kg"))+"\n";
if(ǫ==1) Β+="Battery: "+(ą==ќ.None?Αρ(ą):Βά+"% "+(т==-1||Ǖ?"":" Min: "+т+" %"))+"\n";if(ǫ==2) Β+="Hydrogen: "+(Ė.Count==0?"No tanks":Math.Round(ΒЄ,1)+"% "+(у==-1||Ǖ?"":" Min: "+у+" %"))+"\n";}else if(ΰ==Ι.ı){String λ="";if(Ζ(ref λ,Δ,Α++,Ε," Back")){if(τ==Τ.ǟ) Χ(Ι.Ȯ);else Χ(Ι.Main);}
if(Ζ(ref λ,Δ,Α++,Ε," Max load: "+с+"%")) Џ(ref с,с<=80?-10:-5,ς.Ƒ,false);if(Ζ(ref λ,Δ,Α++,Ε," Weight limit: "+(ǽ?"On":"Off"))) ǽ=!ǽ;if(Ζ(ref λ,Δ,Α++,Ε," Ejection: "+ΔΓ(Ď))) Ď=ў(Ď);if(Ζ(ref λ,Δ,Α++,Ε," Toggle sorters: "+(Ǹ?"On":"Off"))){Ǹ=!Ǹ;if(Ǹ) ƞ(ΑΣ);}
if(Ζ(ref λ,Δ,Α++,Ε," Unload ice: "+(ǝ?"On":"Off"))) ǝ=!ǝ;if(Ζ(ref λ,Δ,Α++,Ε," Uranium: "+(Щ==-1?"Ignore":"Min "+Щ+"Kg"))) Џ(ref Щ,(Щ>5?-5:-1),ς.Ɠ,true);if(Ζ(ref λ,Δ,Α++,Ε," Battery: "+(т==-1?"Ignore":"Min "+т+"%"))) Џ(ref т,-5,ς.ΑЋ,true);if(Ζ(ref λ,Δ,Α++,Ε," Hydrogen: "+(у==-1?"Ignore":"Min "+у+"%"))) Џ(ref у,-10,ς.ΑЌ,true);
if(Ζ(ref λ,Δ,Α++,Ε," When done: "+(Ĭ?"Return home":"Stop"))) Ĭ=!Ĭ;if(Ζ(ref λ,Δ,Α++,Ε," On damage: "+Ασ(ǚ))) ǚ=ў(ǚ);if(Ζ(ref λ,Δ,Α++,Ε," Advanced...")) Χ(Ι.Ż);Β+=ƚ(8,λ,Δ,ref ΖΖ);}else if(ΰ==Ι.Ż){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(τ==Τ.ǟ) Χ(Ι.Ȯ);else Χ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Π==Ν.ȃ?" Grinder":" Drill")+" inv. balancing: "+(ė?"On":"Off"))) ė=!ė;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Π==Ν.ȃ?" grinders":" drills")+": "+(ď?"Fwd + Bwd":"Fwd"))) ď=!ď;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+Ÿ+"m/s")) Џ(ref Ÿ,0.5f,ς.Ƥ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+ŷ+"m/s")) Џ(ref ŷ,0.5f,ς.Ƥ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(Ǒ*100f)+"%"+(Ǒ>0.80f?" (risky)":""))) Џ(ref Ǒ,0.1f,ς.ź,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+ĭ*100f+"%")) Αθ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+Ĕ*100f+"%")) Αθ(false,0.05f);
}else if(ΰ==Ι.Ŀ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(τ==Τ.ǟ) Χ(Ι.Ȯ);else Χ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+с+"%")) Џ(ref с,с<=80?-10:-5,ς.Ƒ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(ǝ?"On":"Off"))) ǝ=!ǝ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Щ==-1?"Ignore":"Min "+Щ+"Kg"))) Џ(ref Щ,(Щ>5?-5:-1),ς.Ɠ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(т==-1?"Ignore":"Charge up"))) т=(т==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(у==-1?"Ignore":"Fill up"))) у=(у==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+Ασ(ǚ))) ǚ=ў(ǚ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(Ǒ*100f)+"%"+(Ǒ>0.80f?" (risky)":""))) Џ(ref Ǒ,0.1f,ς.ź,false);
}else if(ΰ==Ι.ΒΕ){double Εϋ=0;if(χ.Count>0) Εϋ=Vector3.Distance(χ.Last().Ρ,з);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) ΔЌ();if(Π!=Ν.ν){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(Ǟ?"Use old home":(Σ.κ?"Was set! ":"none ")))) Ǟ=!Ǟ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(Ǟ?"Use old connector":(Σ.κ?"Was set! ":"none ")))) Ǟ=!Ǟ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(Ī?"Use old connector":(Φ.κ?"Was set! ":"none ")))) Ī=!Ī;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(ī?"Use old path":(χ.Count>1?"Count: "+χ.Count:"none ")))) ī=!ī;Β+=Ȩ;Β+="Wp spacing: "+Math.Round(Ζγ)+"m\n";}else if(ΰ==Ι.ơ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){Ǣ();Χ(Ι.Main);}
Β+=Ȩ;Β+="State: "+Αχ(ŏ)+" \n";Β+="Speed: "+Math.Round(ΖΜ,1)+"m/s\n";Β+="Target dist: "+ΓΫ+"m\n";Β+="Wp count: "+χ.Count+"\n";Β+="Wp left: "+Ζδ+"\n";}else if(ΰ==Ι.ΑЁ){List<IMyTerminalBlock>ǥ=ΓБ();if(Ę) ǔ++;if(ǔ>=ǥ.Count) ǔ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Χ(Ι.ΑЂ);
Β+=Ȩ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ǂ,1)+"% "+Math.Round(Ǥ,1)+" / "+Math.Round(ǰ,1)+"\n";Β+="Uranium: "+(ΑΔ.Count==0?"No reactors":Math.Round(Γβ,1)+"Kg "+ΕϏ)+"\n";Β+="Battery: "+(ą==ќ.None?"":Βά+"% ")+Αρ(ą)+"\n";Β+="Hydrogen: "+(Ė.Count==0?"No tanks":Math.Round(ΒЄ,1)+"% ")+"\n";
Β+="Damage: "+(ǥ.Count==0?"None":""+(ǔ+1)+"/"+ǥ.Count+" "+ǥ[ǔ].CustomName)+"\n";}else if(ΰ==Ι.ΑЂ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȩ;Β+="Next scan: "+Ů+"s\n";Β+="Drill area: "+Math.Round(Ǻ,1)+"x"+Math.Round(ǹ,1)+"m Ship: "+Math.Round(ΓΦ,1)+"x"+Math.Round(ΓΥ,1)+"m ("+Math.Round(Ȧ,1)+"m)\n"+(ΕΦ.Count>0?"Cameras: "+ΕΥ+"\n":"");
Β+="Broadcast: "+(ć?"Online - "+Ģ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ȕ*100f,1)+"% \n";}else if(ΰ==Ι.ΑϏ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Χ(Ι.Main);Β+=Ȩ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ΑΛ==2) Β="Fatal setup error\nNext scan: "+Ů+"s\n";if(ΓΟ) Β="Recompile script now";int Εύ=Β.Split('\n').Length;for(int i=Εύ;i<=10;i++) Β+="\n";Β+=ȥ;Β+="Last: "+Ψ+"\n";
return Β;}void Αθ(bool Εψ,float ű){Ǣ();ƣ(true,false);if(Εψ) Џ(ref ĭ,ű,ς.ƕ,false);else Џ(ref Ĕ,ű,ς.ƕ,false);Γτ();ń(true,true,0,0);}float Ακ(float ȋ,float[]Ŵ){float Ϊ=Ŵ[0];for(int i=Ŵ.Length-1;i>=0;i--)
if(ȋ<Ŵ[i]) Ϊ=Ŵ[i];return Ϊ;}String ĸ(ref int Ǔ){String Ϊ="";if(Ǔ>=Γί.Count) Ǔ=-1;if(Ǔ>=0) Ϊ=Γί[Ǔ].CustomName;Ǔ++;return Ϊ;}void Ǿ(ref int value,int Ć,int Ų,int ΑΠ){if(Ć==0) return;if(value<Ų&&ΑΠ>0||value<=Ų&&ΑΠ<0){value+=ΑΠ;return;}int ΕΝ=Math.Abs(Ć);int ƥ=0;
int ǋ=1;while(true){ƥ+=ǋ*ΕΝ*10;if(Ć<0&&value-Ų<=ƥ) break;if(Ć>0&&value-Ų<ƥ) break;ǋ++;}value+=ǋ*Ć;}void ȇ(bool ΗЀ){У=Math.Max(У,1);Т=Math.Max(Т,1);а=Math.Max(а,0);ń(ΰ==Ι.ȯ,false,0,0);}void Џ(ref float value,float ű,ς Њ,bool Ĩ){Џ(ref value,value+ű,Њ,Ĩ,true);
}void Џ(ref float value,float ȗ,ς Њ,bool Ĩ,bool Ζθ){float Ъ=0,Ш=0;if(Њ==ς.Ƥ){Ъ=0.5f;Ш=10f;}if(Њ==ς.ź){Ъ=0.1f;Ш=1f;}if(Њ==ς.Ɗ){Ъ=50f;Ш=100f;}if(Њ==ς.ΑЋ){Ъ=5f;Ш=30f;}if(Њ==ς.Ɠ){Ъ=1f;Ш=25f;}if(Њ==ς.Ƒ){Ъ=10f;Ш=95f;}if(Њ==ς.ΑЌ){Ъ=10f;Ш=90f;}
if(Њ==ς.ΕΑ){Ъ=10f;Ш=1800;}if(Њ==ς.ƕ){Ъ=0.0f;Ш=0.75f;}if(ȗ==-1&&Ĩ){value=-1;return;}if(value==-1) Ĩ=false;bool ΓΗ=ȗ<Ъ||ȗ>Ш;if(ΓΗ&&Ζθ){if(ȗ<value) value=Ш;else if(ȗ>value) value=Ъ;}else value=ȗ;if(ΓΗ&&Ĩ) value=-1;else value=Math.Max(Ъ,Math.Min(value,Ш));
value=(float) Math.Round(value,2);}void Γζ(bool ΕБ){if(ΕБ) м=Math.Max(0,м-1);else м++;ΓІ();ΓЅ();for(int k=0;k<ƻ.Count&&k<в.Length;k++) ΔЁ(ƻ[k],k<м);ǲ=-1;ΒΦ(true);}T ў<T>(T ȋ){int Α=Array.IndexOf(Enum.GetValues(ȋ.GetType()),ȋ);Α++;if(Α>=ΓЇ(ȋ)) Α=0;return(T) Enum.GetValues(ȋ.GetType()).GetValue(Α);
}int ΓЇ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔЇ(){Ŷ.Clear();for(int i=0;i<χ.Count;i++) Ŷ.Add(χ[i]);χ.Clear();ΒЅ=true;ΒЀ=new Ω(Σ);ΒЇ=new Ω(Φ);Σ.κ=false;if(Π==Ν.ν) Φ.κ=false;for(int i=0;i<љ.Count;i++)
if(!њ.Contains(љ.Keys.ElementAt(i)))
њ.Add(љ.Keys.ElementAt(i));Ǟ=false;Ī=false;ī=false;Χ(Ι.ΒΕ);}void ΔЌ(){if(Ǟ) Σ=ΒЀ;if(Ī) Φ=ΒЇ;if(ī){χ.Clear();for(int i=0;i<Ŷ.Count;i++) χ.Add(Ŷ[i]);}ΒЅ=false;Ǣ();Χ(Ι.Main);}void Ľ(Ω Х,Τ ώ){Εΰ=Х;if(τ==Τ.ǟ) ǘ=ώ;}bool Ατ(Ω Х,float Г,bool Ζί,out Vector3 ά){if(Ζί){Vector3I Εν=new Vector3I((int) Х.Ф.X,(int) Х.Ф.Y,(int) Х.Ф.Z);
IMySlimBlock ή=Me.CubeGrid.GetCubeBlock(Εν);if(ή==null||!(ή.FatBlock is IMyShipConnector)){ά=new Vector3();return false;}Vector3 Εά=ķ(Ј,ή.FatBlock.GetPosition()-з);Vector3 ΕΫ=ķ(Ј,ή.FatBlock.WorldMatrix.Forward);ά=Х.Ρ-ΒΣ(Х.Є,Х.Ϗ*-1,Εά)-ΒΣ(Х.Є,Х.Ϗ*-1,ΕΫ)*Г;return true;}
else{ά=Х.Ρ;return true;}}Vector3 ΔΗ(int ΕΩ,int ȡ,bool ΖΑ){if(!ΖΑ&&Βω) return Ƭ;float ΕΪ=((е-1f)/2f)-ΕΩ;float ΖΓ=((д-1f)/2f)-ȡ;Ƭ=Φ.Ρ+Φ.Л*ΕΪ*Ǻ+Ȗ*-1*ΖΓ*ǹ;Βω=true;return Ƭ;}Υ ƃ(){float С=-1;Υ Ϊ=Υ.й;if(Π!=Ν.ν){Δί();if(τ!=Τ.ь){Vector3 ϋ=ΒΤ(ĝ,Ȗ*-1,з-Φ.Ρ);
if(Math.Abs(ϋ.X)<=(float)(е*Ǻ)/2f&&Math.Abs(ϋ.Y)<=(float)(д*ǹ)/2f){if(ϋ.Z<=-1&&ϋ.Z>=-ΑΘ*2) return Υ.İ;if(ϋ.Z>-1&&ϋ.Z<Ȧ*2) return Υ.й;}if(ѝ(Φ.Ρ,ref С)) Ϊ=Υ.й;}if(Σ.κ){if(ѝ(Σ.Ρ,ref С)) Ϊ=Υ.и;for(int i=0;i<χ.Count;i++)
if(ѝ(χ[i].Ρ,ref С)) Ϊ=Υ.э;if(Vector3.Distance(з,Σ.Ρ)<dockDist*Ȧ) Ϊ=Υ.и;if(ȁ(MyShipConnectorStatus.Connectable)!=null||ȁ(MyShipConnectorStatus.Connected)!=null)
Ϊ=Υ.и;}}else{Vector3 ά=new Vector3();IMyShipConnector Ŋ=ȁ(MyShipConnectorStatus.Connected);if(Σ.κ){if(ѝ(Σ.Ρ,ref С)) Ϊ=Υ.и;if(Ατ(Σ,dockDist,true,out ά))
if(ѝ(ά,ref С)) Ϊ=Υ.и;if(Ŋ!=null&&Vector3.Distance(Ŋ.GetPosition(),Σ.Ρ)<5)
return Υ.ż;}for(int i=0;i<χ.Count;i++)
if(Vector3.Distance(χ[i].Ρ,Σ.Ρ)>dockDist*Ȧ&&Vector3.Distance(χ[i].Ρ,Φ.Ρ)>dockDist*Ȧ)
if(ѝ(χ[i].Ρ,ref С)) Ϊ=Υ.э;if(Φ.κ){if(ѝ(Φ.Ρ,ref С)) Ϊ=Υ.й;if(Ατ(Φ,dockDist,true,out ά))
if(ѝ(ά,ref С)) Ϊ=Υ.й;if(Ŋ!=null&&Vector3.Distance(Ŋ.GetPosition(),Φ.Ρ)<5)
return Υ.Ž;}}return Ϊ;}bool ѝ(Vector3 ά,ref float С){float Г=Vector3.Distance(ά,з);if(Г<С||С==-1){С=Г;return true;}return false;}void ΒΝ(){if(ΑΛ>0){Ψ="Setup error! Can't start";return;}if(Π==Ν.ν){Αι();return;}Φ.Ρ=з;Φ.ξ=Ј.GetNaturalGravity();Φ.Є=Ει;Φ.Ϗ=Εβ;Φ.Л=Εό;
ĝ=Ǜ.WorldMatrix.Forward;Ȗ=Φ.Ϗ;if(ĝ==Ј.WorldMatrix.Down) Ȗ=Ј.WorldMatrix.Backward;ƣ(true,true);β(Λ.Ɔ);Ơ();}void ƣ(bool Ц,bool ΖΒ){if(τ==Τ.ь&&!Ц) return;bool ΕІ=Ц||τ==Τ.Αμ||е!=У||д!=Т||Ȓ!=Ƿ;if(ΕІ){if(τ!=Τ.ь){τ=Τ.Ĳ;ΔΗ(Ŝ,ŝ,ΖΒ);Ψ="Job changed, lost progress";}Ȓ=Ƿ;
е=У;д=Т;ŝ=0;Ŝ=0;в="";ǲ=-1;Ǉ=0;ě=0;Βκ=0;м=0;ΒΦ(true);}}void Ɨ(){ΔϏ(з,0);ļ(Ħ,true);}void Ơ(){ΒΛ(Ǐ.ΒΘ);Ɨ();Ɲ(ƿ,false);ƞ(ΑΣ);τ=Τ.ǟ;Γσ(true);ǘ=τ;Χ(Ι.Ȯ);Δψ();Βλ=true;Ƽ=0;for(int i=Ă.Count-1;i>=0;i--)
if(ΑЄ(Ă[i],false)) Ƽ++;if(Ƽ>0) Ψ="Started with damage";}void Ǣ(){if(τ==Τ.ǟ){τ=Τ.Ɩ;Ψ="Job paused";}β(Λ.Ǡ);ǘ=τ;ΔЀ(false,0,0,0,0);ΔЊ();Γι(new Vector3(),false);ΔЋ();Δό(ChargeMode.Auto);ΔЂ(false);Δώ(true);ǡ(Λ.Ǡ);ń(false,false,0,0);ļ(Ю,false);ļ(Ċ,true);ƞ(true);ΓΠ=false;Γχ();
Εα=false;Δς();Βλ=false;ΑΞ=false;Γα=false;if(ΰ!=Ι.Main&&ΰ!=Ι.ı&&ΰ!=Ι.Ż&&ΰ!=Ι.Ŀ)
Χ(Ι.Main);}void Αι(){Υ ѕ=ƃ();if(Π==Ν.ν){if(!Φ.κ||!Σ.κ) return;Ơ();bool Ʒ=Vector3.Distance(з,Σ.Ρ)<Vector3.Distance(з,Φ.Ρ);if(ΑΙ==Τ.Į) Ʒ=true;if(ΑΙ==Τ.į) Ʒ=false;if(Ʒ){Ľ(Σ,Τ.Į);switch(ѕ){case Υ.ż:β(Λ.ā);break;case Υ.э:β(Λ.Ǎ);break;case Υ.и:β(Λ.ǿ);break;default:β(Λ.ȳ);break;
}}else{Ľ(Φ,Τ.į);switch(ѕ){case Υ.Ž:β(Λ.ā);break;case Υ.й:β(Λ.ǿ);break;case Υ.э:β(Λ.Ǎ);break;default:β(Λ.ȳ);break;}}}else{if(τ!=Τ.Ɩ&&τ!=Τ.Ĳ) return;bool Ζβ=τ==Τ.Ĳ;Ơ();bool ƹ=ΔΫ(false)&&Σ.κ;switch(ѕ){case Υ.й:β(ƹ?Λ.ƅ:Λ.Ɔ);break;case Υ.э:β(ƹ?Λ.ȭ:Λ.Ǎ);break;case Υ.и:β(ƹ?Λ.ǿ:Λ.Ā);break;
case Υ.İ:{if(ě!=м||Ζβ) β(Λ.ƙ);else β(Λ.ΑЍ);}break;default:break;}}}void ΓЎ(){if(τ==Τ.ь&&!Σ.κ) return;if(Π==Ν.ν&&(!Φ.κ||!Σ.κ)) return;Ψ="Move to job";Υ ѕ=ƃ();if(Π==Ν.ν){Ľ(Φ,Τ.į);switch(ѕ){case Υ.й:β(Λ.ǿ);break;case Υ.э:β(Λ.Ǎ);break;case Υ.Ž:return;default:β(Λ.ȳ);break;
}ǡ(Λ.ā);}else{switch(ѕ){case Υ.й:β(Λ.Ǎ);break;case Υ.э:β(Λ.Ǎ);break;case Υ.и:β(Λ.Ā);break;case Υ.İ:β(Λ.ƙ);break;default:break;}if(τ==Τ.ь) ǡ(Λ.Ǎ);else ǡ(Λ.Αδ);ΓΠ=true;}Ɨ();Χ(Ι.ơ);Ɲ(ƿ,false);ǘ=Τ.ΑЏ;}void ΓЋ(){if(!Σ.κ) return;Ψ="Move home";Υ ѕ=ƃ();if(Π==Ν.ν){Ľ(Σ,Τ.Į);switch(ѕ)
{case Υ.э:β(Λ.ȭ);break;case Υ.и:β(Λ.ǿ);break;case Υ.ż:return;default:β(Λ.ȳ);break;}ǡ(Λ.ā);}else{if(ȁ(MyShipConnectorStatus.Connected)!=null) return;if(ȁ(MyShipConnectorStatus.Connectable)!=null){β(Λ.Αλ);ǡ(Λ.Ā);return;}switch(ѕ){case Υ.й:β(Λ.ƅ);break;case Υ.э:β(Λ.ȭ);break;case Υ.и:β(Λ.ȭ);break;
case Υ.İ:β(Λ.ΒΗ);break;default:break;}ǡ(Λ.Ā);}Ɨ();Χ(Ι.ơ);Ɲ(ƿ,false);ǘ=Τ.ΑЎ;}void β(Λ ǌ){if(ǌ==Λ.Ǡ) Ĥ=Λ.Ǡ;if(Ĥ!=Λ.Ǡ&&ŏ==Ĥ&&ǌ!=Ĥ){Ǣ();return;}ΖΠ=true;ŏ=ǌ;}void ǡ(Λ ώ){this.Ĥ=ώ;}void ΒΦ(bool ΑΕ){if(ΑΕ){Ţ=0;return;}if(Π==Ν.ν) return;float ϊ=м*Math.Max(1,а);if(ě==м) ϊ+=Math.Min(а,Βκ);
float Ζά=е*д*Math.Max(1,а);Ţ=Math.Max(Ţ,(float) Math.Min(ϊ/Ζά*100.0,100));}List<IMyTerminalBlock>ΓБ(){List<IMyTerminalBlock>ǥ=new List<IMyTerminalBlock>();for(int i=0;i<Ă.Count;i++){IMyTerminalBlock ή=Ă[i];if(ΑЄ(ή,false)) ǥ.Add(ή);}return ǥ;}bool ΑЄ(IMyTerminalBlock ή,bool ΑΦ)
{return(!ΑЅ(ή,ΑΦ)||!ή.IsFunctional);}bool ΑЅ(IMyTerminalBlock ή,bool ΑΦ){if(ή==null) return false;try{IMyCubeBlock Βψ=Me.CubeGrid.GetCubeBlock(ή.Position).FatBlock;if(ΑΦ) return Βψ==ή;else return Βψ.GetType()==ή.GetType();}catch{return false;}}bool ΔΫ(bool Ηα){if(ǽ&&Π!=Ν.ν)
if(Ѐ!=-1&&ΓΧ>=Ѐ){Ψ="Ship too heavy";return true;}if(ǂ>=с||ΑΞ){ΑΞ=false;Ψ="Ship is full";return true;}return false;}static String ΔΝ(IMyThrust ц){return ц.BlockDefinition.SubtypeId;}Vector3 Ɛ(Vector3 Ж,float[,]Ȕ){return new Vector3(
Ж.X>=0?Ȕ[0,0]:Ȕ[0,1],
Ж.Y>=0?Ȕ[1,0]:Ȕ[1,1],
Ж.Z>=0?Ȕ[2,0]:Ȕ[2,1]);}bool Αϋ(Ω Ϋ,String ǻ,out float ƴ){ƴ=0;int Α=њ.IndexOf(ǻ);if(Α==-1||Ϋ.Э==null||Α>=Ϋ.Э.Length)
return false;ƴ=Ϋ.Э[Α];if(ƴ==-1) return false;return true;}Vector3 Αϊ(Vector3 Ж,Ω Ϋ){if(Ϋ!=null){Vector3 Ϊ=new Vector3();for(int i=0;i<љ.Keys.Count;i++){String ǻ=љ.Keys.ElementAt(i);float č=0;if(!Αϋ(Ϋ,ǻ,out č))
return Ɛ(Ж,ĩ);Ϊ+=Ɛ(Ж,љ.Values.ElementAt(i))*č;}return Ϊ;}return Ɛ(Ж,ĩ);}float Ɖ(Vector3 Ж,Ω Ϋ){return Ɖ(Ж,new Vector3(),Ϋ);}float Ɖ(Vector3 Ж,Vector3 ξ,Ω Ϋ){Vector3 ΖΦ=Αϊ(Ж,Ϋ);Vector3 Εδ=ΖΦ+ξ*ΓΧ;float Ǵ=(Εδ/Ж).AbsMin();return(float)(Ж*Ǵ).Length();}float ĵ(Vector3 Ē,Vector3 Ё,Vector3 ξ,Ω Ϋ)
{if(ξ.Length()==0f) return 0;Vector3 Εώ=ΒΤ(Ē,Ё,Vector3.Normalize(ξ));float Ц=Ɖ(-Εώ,Ϋ);return Ц/ξ.Length();}void Γσ(bool Ετ){float Н=0;float Ƶ=0.9f;if(Ετ){Ѐ=-1;Ǽ=0;ů=null;if(τ!=Τ.ь&&Φ.ξ.Length()!=0){Н=Ƶ*ĵ(Φ.Є,Φ.Ϗ*-1,Φ.ξ,null);if(Н<Ѐ||Ѐ==-1) Ѐ=Н;}if(Σ.κ&&Σ.ξ.Length()!=0)
{Н=Ƶ*ĵ(Σ.Є,Σ.Ϗ*-1,Σ.ξ,null);if(Н<Ѐ||Ѐ==-1) Ѐ=Н;}return;}if(Ǽ==-1) return;if(Ǽ>=0){int ΓΛ=0;while(Ǽ<χ.Count){if(ΓΛ>100) return;ΓΛ++;Ω Ϋ=χ[Ǽ];if(Ϋ.ξ.Length()!=0f){Н=Ƶ*Math.Min(
ĵ(Ϋ.Є,Ϋ.Ϗ*-1,Ϋ.ξ,Ϋ),
ĵ(Ϋ.Є*-1,Ϋ.Ϗ*-1,Ϋ.ξ,Ϋ));if(Н<Ѐ||Ѐ==-1) Ѐ=Н;}else ů=Ϋ;Ǽ++;}Ǽ=-1;}bool Ƹ=true;float Ǚ=0;if(ů!=null){for(int i=0;i<љ.Count;i++){String ǻ=љ.Keys.ElementAt(i);float[,]Αά=љ.Values.ElementAt(i);float č=0;if(!Αϋ(ů,ǻ,out č)){Ƹ=false;break;}for(int a=0;a<Αά.GetLength(0);a++)
for(int b=0;b<Αά.GetLength(1);b++){float ǣ=Math.Abs(Αά[a,b]*č);if(ǣ==0) continue;Ƹ=true;if(Ǚ==0||ǣ<Ǚ) Ǚ=ǣ;}}}if(!Ƹ){for(int a=0;a<ĩ.GetLength(0);a++)
for(int b=0;b<ĩ.GetLength(1);b++){float ǣ=Math.Abs(ĩ[a,b]);if(ǣ==0) continue;if(Ǚ==0||ǣ<Ǚ) Ǚ=ǣ;}}if(Ǚ>0){float ΕЂ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Н=О(Ǚ,ΕЂ);if(Н>0)
if(Н<Ѐ||Ѐ==-1) Ѐ=Н;}}void Γι(Vector3 Ц,bool ύ){if(!ύ){for(int i=0;i<Ħ.Count;i++)
Ħ[i].SetValueFloat("Override",0.0f);return;}Vector3 Ƨ=Αϊ(Ц,null);float Ζι=Math.Min(1,Math.Abs(О(Ц.X,Ƨ.X)));float Ζκ=Math.Min(1,Math.Abs(О(Ц.Y,Ƨ.Y)));float Ζλ=Math.Min(1,Math.Abs(О(Ц.Z,Ƨ.Z)));for(int i=0;i<Ħ.Count;i++){IMyThrust ц=Ħ[i];Vector3 Ȍ=Δφ(ķ(Ј,ц.WorldMatrix.Backward),1);
if(Ȍ.X!=0&&Math.Sign(Ȍ.X)==Math.Sign(Ц.X))
ц.SetValueFloat("Override",ц.MaxThrust*Ζι);else if(Ȍ.Y!=0&&Math.Sign(Ȍ.Y)==Math.Sign(Ц.Y))
ц.SetValueFloat("Override",ц.MaxThrust*Ζκ);else if(Ȍ.Z!=0&&Math.Sign(Ȍ.Z)==Math.Sign(Ц.Z))
ц.SetValueFloat("Override",ц.MaxThrust*Ζλ);else
ц.SetValueFloat("Override",0.0f);}}void ΔЀ(bool ύ,float ΕА,float ț,float Ƞ,float Ȭ){for(int i=0;i<Βϋ.Count;i++){IMyGyro ο=Βϋ[i];ο.GyroOverride=ύ;if(!ύ) ο.GyroPower=100;else ο.GyroPower=ΕА;if(!ύ) continue;Vector3 И=Ј.WorldMatrix.Forward;Vector3 ȟ=Ј.WorldMatrix.Right;Vector3 Ё=Ј.WorldMatrix.Up;
Vector3 ȍ=ο.WorldMatrix.Forward;Vector3 ȏ=ο.WorldMatrix.Up;Vector3 Ȏ=ο.WorldMatrix.Left*-1;if(ȍ==И) ο.SetValueFloat("Roll",Ȭ);else if(ȍ==(И*-1)) ο.SetValueFloat("Roll",Ȭ*-1);else if(ȏ==(И*-1)) ο.SetValueFloat("Yaw",Ȭ);else if(ȏ==И) ο.SetValueFloat("Yaw",Ȭ*-1);else if(Ȏ==И) ο.SetValueFloat("Pitch",Ȭ);
else if(Ȏ==(И*-1)) ο.SetValueFloat("Pitch",Ȭ*-1);if(Ȏ==(ȟ*-1)) ο.SetValueFloat("Pitch",ț);else if(Ȏ==ȟ) ο.SetValueFloat("Pitch",ț*-1);else if(ȏ==ȟ) ο.SetValueFloat("Yaw",ț);else if(ȏ==(ȟ*-1)) ο.SetValueFloat("Yaw",ț*-1);else if(ȍ==(ȟ*-1)) ο.SetValueFloat("Roll",ț);
else if(ȍ==ȟ) ο.SetValueFloat("Roll",ț*-1);if(ȏ==(Ё*-1)) ο.SetValueFloat("Yaw",Ƞ);else if(ȏ==Ё) ο.SetValueFloat("Yaw",Ƞ*-1);else if(Ȏ==Ё) ο.SetValueFloat("Pitch",Ƞ);else if(Ȏ==(Ё*-1)) ο.SetValueFloat("Pitch",Ƞ*-1);else if(ȍ==Ё) ο.SetValueFloat("Roll",Ƞ);else if(ȍ==(Ё*-1)) ο.SetValueFloat("Roll",Ƞ*-1);
}}void ΔЋ(){this.Εξ=false;}void ΔЊ(){ľ(false,false,false,ΓΕ,0);Γή=false;}void ΔϏ(Vector3 α,float ģ){ľ(true,false,false,α,ģ);}void ľ(bool ύ,bool ΑΡ,bool Αέ,Vector3 α,float ģ){ľ(ύ,ΑΡ,Αέ,α,α-з,0.0f,ģ);}void ľ(bool ύ,bool ΑΡ,bool Αέ,Vector3 α,Vector3 ΖΘ,float ΖΙ,float ģ)
{Γή=true;this.ΖΧ=ύ;ΓΕ=α;this.ΕЀ=ģ;this.ΕЍ=ΖΙ;this.ΖΞ=ΑΡ;this.ΖΝ=Αέ;this.Εη=ΖΘ;Βπ=Vector3.Distance(α,з);}void Δψ(){Й.GetBlocksOfType(Ă,Ǝ);}void Γτ(){if(Ј==null) return;Ǜ=null;float ũ=0,ť=0,Ū=0,Ŧ=0,ū=0,ŧ=0;List<IMyTerminalBlock>ΑΧ=Αξ(Ю,pamTag,true);bool ΕЈ=ΑΧ.Count==0;
if(ΑΧ.Count>0) Ǜ=ΑΧ[0];else if(Ю.Count>0) Ǜ=Ю[0];int Ŏ=0;float ȝ=0;for(int i=0;i<Ю.Count;i++){if(Ю[i].WorldMatrix.Forward!=Ǜ.WorldMatrix.Forward){if(ΕЈ){ΑΛ=2;Ψ="Mining direction is unclear!";return;}continue;}Ŏ++;ȝ=Math.Max(ȝ,ΕΒ(Ю[i]));Vector3 ϋ=ƈ(Ј,Ю[i].GetPosition());
if(i==0){ũ=ϋ.X;ť=ϋ.X;Ū=ϋ.Y;Ŧ=ϋ.Y;ū=ϋ.Z;ŧ=ϋ.Z;}ť=Math.Max(ϋ.X,ť);ũ=Math.Min(ϋ.X,ũ);Ŧ=Math.Max(ϋ.Y,Ŧ);Ū=Math.Min(ϋ.Y,Ū);ŧ=Math.Max(ϋ.Z,ŧ);ū=Math.Min(ϋ.Z,ū);}if(ȝ==0) ȝ=drillRadius;bool Βσ=Ǜ!=null&&Ǜ.WorldMatrix.Forward==Ј.WorldMatrix.Down;Ǻ=(ť-ũ)*(1-ĭ)+ȝ*2;
ǹ=(Βσ?ŧ-ū:Ŧ-Ū)*(1-Ĕ)+ȝ*2;IMyCubeGrid g=Ј.CubeGrid;float Αΰ=0,Αα=0,Αβ=0,Αγ=0;for(int c=0;c<8;c++){Vector3 p=ƈ(Ј,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Βσ?p.Z:p.Y;if(c==0||p.X<Αΰ) Αΰ=p.X;if(c==0||p.X>Αα) Αα=p.X;if(c==0||y<Αβ) Αβ=y;if(c==0||y>Αγ) Αγ=y;}ΓΦ=Αα-Αΰ+g.GridSize;ΓΥ=Αγ-Αβ+g.GridSize;}float ΕΒ(IMyTerminalBlock b)
{if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Αξ<T>(List<T>Ѕ,String Ь,bool Ş){List<T>Ϊ=new List<T>();
for(int i=0;i<Ѕ.Count;i++)
if(Αώ(Ѕ[i],Ь,Ş)) Ϊ.Add(Ѕ[i]);return Ϊ;}bool Αώ<T>(T ή,String Ь,bool Ş){IMyTerminalBlock b=(IMyTerminalBlock) ή;if(Ş&&b.CustomName.ToUpper().Contains(Ь.ToUpper())) return true;if(!Ş&&b.BlockDefinition.ToString().ToUpper().Contains(Ь.ToUpper())) return true;return false;}void ļ<T>(List<T>Ѕ,bool ύ)
{for(int i=0;i<Ѕ.Count;i++)
Δύ((IMyTerminalBlock) Ѕ[i],ύ);}void Δύ(IMyTerminalBlock ή,bool ύ){if(ή==null) return;String ΕΞ=ύ?"OnOff_On":"OnOff_Off";var a=ή.GetActionWithName(ΕΞ);a.Apply(ή);}void ƞ(bool ύ){ΑΣ=ύ;if(!Ǹ) return;ļ(ΖΟ,ύ);}void Δό(ChargeMode η){for(int i=0;i<ΒΫ.Count;i++) ΒΫ[i].ChargeMode=η;}void ΔЂ(bool ΖΣ)
{for(int i=0;i<Ė.Count;i++) Ė[i].Stockpile=ΖΣ;}void Ɲ(List<IMyLandingGear>ƶ,bool ΒЏ){for(int i=0;i<ƶ.Count;i++){if(ΒЏ) ƶ[i].Lock();if(!ΒЏ) ƶ[i].Unlock();}}void Δώ(bool ύ){if(Βμ==ύ) return;List<IMyShipController>ō=new List<IMyShipController>();Й.GetBlocksOfType(ō,Ǝ);if(ō.Count==0) return;
for(int i=0;i<ō.Count;i++)
ō[i].DampenersOverride=ύ;Βμ=ύ;}IMyShipConnector ȁ(MyShipConnectorStatus ΖΡ){for(int i=0;i<Ċ.Count;i++){if(!ΑЅ(Ċ[i],true)) continue;if(Ċ[i].Status==ΖΡ) return Ċ[i];}return null;}void ń(bool ΑΜ,bool Ő,float Εκ,float ΕΡ){if(ό==null||Ю.Count==0) return;Vector3 ň=new Vector3();
int Ŏ=0;for(int i=0;i<Ю.Count;i++){if(Ю[i].WorldMatrix.Forward!=Ǜ.WorldMatrix.Forward) continue;Ŏ++;ň+=Ю[i].GetPosition();}ň=ň/Ŏ;Vector3 Ș=ƈ(ό,ň);ό.Enabled=true;ό.ShowOnHUD=ΑΜ;ό.LeftExtend=(Ő?1:У)/2f*Ǻ-Ș.X;ό.RightExtend=(Ő?1:У)/2f*Ǻ+Ș.X;ό.TopExtend=(Ő?1:Т)/2f*ǹ+Ș.Y;
ό.BottomExtend=(Ő?1:Т)/2f*ǹ-Ș.Y;ό.FrontExtend=(ΑΜ?а:Εκ)-Ș.Z;ό.BackExtend=ΑΜ?0:ΕΡ+Ȧ*0.75f+Ș.Z;ό.DetectFloatingObjects=true;ό.DetectAsteroids=false;ό.DetectLargeShips=true;ό.DetectSmallShips=true;ό.DetectStations=true;ό.DetectOwner=true;ό.DetectSubgrids=false;
ό.DetectPlayers=false;ό.DetectEnemy=true;ό.DetectFriendly=true;ό.DetectNeutral=true;}void ȱ<T>(List<T>Ѕ,bool ΖΛ,float Εθ,bool Εχ){for(int i=0;i<Ѕ.Count;i++){IMyTextSurface ĥ=null;if(Ѕ[i]is IMyTextSurface) ĥ=(IMyTextSurface) Ѕ[i];if(ĥ!=null){ĥ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΖΛ) continue;ĥ.Font="Debug";if(Εχ) continue;ĥ.FontSize=Εθ;}}}static float О(float a,float b){if(b==0) return 0;return a/b;}Vector3 Δφ(Vector3 v,int ư){return new Vector3(Math.Round(v.X,ư),Math.Round(v.Y,ư),Math.Round(v.Z,ư));}Vector3 ƈ(IMyTerminalBlock ή,Vector3 Ζη){return ķ(ή,Ζη-ή.GetPosition());
}Vector3 ķ(IMyTerminalBlock ή,Vector3 Αί){return Vector3D.TransformNormal(Αί,MatrixD.Transpose(ή.WorldMatrix));}Vector3 ΒΤ(Vector3 Ē,Vector3 Ё,Vector3 Αί){MatrixD ǆ=MatrixD.CreateFromDir(Ē,Ё);return Vector3D.TransformNormal(Αί,MatrixD.Transpose(ǆ));}Vector3 ΒΣ(Vector3 Ē,Vector3 Ё,Vector3 ϋ)
{MatrixD ǆ=MatrixD.CreateFromDir(Ē,Ё);return Vector3D.Transform(ϋ,ǆ);}String ί(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 γ(String s){String[]ε=s.Split('|');return new Vector3(float.Parse(Θ(ε,0)),float.Parse(Θ(ε,1)),float.Parse(Θ(ε,2)));}String ΕΜ(Ω Ϋ){String Ƕ=":";String Ϊ=ί(Ϋ.Ρ)+Ƕ+ί(Ϋ.Є)+Ƕ+
ί(Ϋ.Ϗ)+Ƕ+ί(Ϋ.Л)+Ƕ+ί(Ϋ.ξ);for(int i=0;i<Ϋ.Э.Length;i++)
Ϊ+=Ƕ+Math.Round(Ϋ.Э[i],3);return Ϊ;}Ω ΔЍ(String s){String[]ε=s.Split(':');Ω Ϋ=new Ω(
γ(Θ(ε,0)),γ(Θ(ε,1)),
γ(Θ(ε,2)),γ(Θ(ε,3)),
γ(Θ(ε,4)));int Α=5;List<float>Βυ=new List<float>();while(Α<ε.Length){String ų=Θ(ε,Α);float f=0;if(!float.TryParse(ų,out f)) break;Βυ.Add(f);Α++;}Ϋ.Э=Βυ.ToArray();return Ϋ;}System.Text.StringBuilder ȣ=new System.Text.StringBuilder();void Κ<T>(T value,bool ΕЇ){if(ΕЇ) ȣ.Append('\n');ȣ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String ĳ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ń(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]ΕΟ,int Α){String ų=ΕΟ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ų)) return "";
return ų;}void Save(){if(ΓΟ||Π==Ν.А){Storage="";return;}Storage=Γξ();}String Γξ(){ȣ.Clear();ȣ.Append(DATAREV+";");Κ(ί(ΓΞ),false);Κ(ί(Σ.Є));Κ(ί(Σ.Л));Κ(ί(Σ.Ϗ));Κ(ί(Σ.ξ));Κ(ί(Σ.Ρ));Κ(ί(Σ.Ф));Κ(Σ.κ);Κ(ί(Φ.Ρ));Κ(ί(Φ.ξ));Κ(ί(Φ.Є));Κ(ί(Φ.Ϗ));Κ(ί(Φ.Л));Κ(ί(Φ.Ф));Κ(Φ.κ);Κ(ί(ĝ));Κ(ί(Ȗ));Κ(";");
Κ((int) Π,false);Κ((int) τ);Κ((int) ǘ);Κ(с);Κ(т);Κ(Щ);Κ(у);Κ(Ǒ);Κ(ǽ);Κ(ǝ);if(Π==Ν.ν){Κ((int) υ.М);Κ(υ.б);Κ(υ.Ǩ);Κ(ĳ(υ.н));Κ(ĳ(υ.р));Κ((int) φ.М);Κ(φ.б);Κ(φ.Ǩ);Κ(ĳ(φ.н));Κ(ĳ(φ.р));}else{Κ((int) Ƿ);Κ((int) ǚ);Κ((int) Ď);Κ((int) ѓ);Κ((int) Ȓ);Κ(Ĭ);Κ(Ǹ);Κ(ė);Κ(ď);Κ(У);Κ(Т);Κ(а);Κ(Ÿ);Κ(ŷ);Κ(ĭ);Κ(Ĕ);
Κ(е);Κ(д);Κ(Ŝ);Κ(ŝ);Κ(ě);Κ(м);Κ(ΑΘ);Κ(Ǉ);}Κ(";");for(int i=0;i<њ.Count;i++)
Κ((i>0?"|":"")+њ[i],false);Κ(";");for(int i=0;i<χ.Count;i++)
Κ(ΕΜ(χ[i]),i>0);Κ(";",false);Κ(в+"|"+ǲ,false);Κ(";",false);Κ(ΓЁ(),false);String Ξ=ȣ.ToString();ȣ.Clear();return Ξ;}к Δσ(){return Δτ(Storage);}к Δτ(String Ξ){if(Ξ=="") return к.Δγ;String[]ǵ=Ξ.Split(';');if(Θ(ǵ,0)!=DATAREV) return к.ΑА;int Α=0;try{String[]Η=Θ(ǵ,1).Split('\n');
ΓΞ=γ(Θ(Η,Α++));Σ.Є=γ(Θ(Η,Α++));Σ.Л=γ(Θ(Η,Α++));Σ.Ϗ=γ(Θ(Η,Α++));Σ.ξ=γ(Θ(Η,Α++));Σ.Ρ=γ(Θ(Η,Α++));Σ.Ф=γ(Θ(Η,Α++));Σ.κ=bool.Parse(Θ(Η,Α++));Φ.Ρ=γ(Θ(Η,Α++));Φ.ξ=γ(Θ(Η,Α++));Φ.Є=γ(Θ(Η,Α++));Φ.Ϗ=γ(Θ(Η,Α++));Φ.Л=γ(Θ(Η,Α++));Φ.Ф=γ(Θ(Η,Α++));Φ.κ=bool.Parse(Θ(Η,Α++));
ĝ=γ(Θ(Η,Α++));Ȗ=γ(Θ(Η,Α++));Η=Θ(ǵ,2).Split('\n');Α=0;Π=(Ν) int.Parse(Θ(Η,Α++));τ=(Τ) int.Parse(Θ(Η,Α++));ǘ=(Τ) int.Parse(Θ(Η,Α++));с=int.Parse(Θ(Η,Α++));т=int.Parse(Θ(Η,Α++));Щ=int.Parse(Θ(Η,Α++));у=int.Parse(Θ(Η,Α++));Ǒ=float.Parse(Θ(Η,Α++));ǽ=bool.Parse(Θ(Η,Α++));
ǝ=bool.Parse(Θ(Η,Α++));if(Π==Ν.ν){υ.М=(Ѓ) int.Parse(Θ(Η,Α++));υ.б=float.Parse(Θ(Η,Α++));υ.Ǩ=float.Parse(Θ(Η,Α++));υ.н=Ń(Θ(Η,Α++));υ.р=Ń(Θ(Η,Α++));φ.М=(Ѓ) int.Parse(Θ(Η,Α++));φ.б=float.Parse(Θ(Η,Α++));φ.Ǩ=float.Parse(Θ(Η,Α++));φ.н=Ń(Θ(Η,Α++));φ.р=Ń(Θ(Η,Α++));}else{Ƿ=(Р) int.Parse(Θ(Η,Α++));
ǚ=(џ) int.Parse(Θ(Η,Α++));Ď=(Я) int.Parse(Θ(Η,Α++));ѓ=(ш) int.Parse(Θ(Η,Α++));Ȓ=(Р) int.Parse(Θ(Η,Α++));Ĭ=bool.Parse(Θ(Η,Α++));Ǹ=bool.Parse(Θ(Η,Α++));ė=bool.Parse(Θ(Η,Α++));ď=bool.Parse(Θ(Η,Α++));У=int.Parse(Θ(Η,Α++));Т=int.Parse(Θ(Η,Α++));а=int.Parse(Θ(Η,Α++));Ÿ=float.Parse(Θ(Η,Α++));
ŷ=float.Parse(Θ(Η,Α++));ĭ=float.Parse(Θ(Η,Α++));Ĕ=float.Parse(Θ(Η,Α++));е=int.Parse(Θ(Η,Α++));д=int.Parse(Θ(Η,Α++));Ŝ=int.Parse(Θ(Η,Α++));ŝ=int.Parse(Θ(Η,Α++));ě=int.Parse(Θ(Η,Α++));м=int.Parse(Θ(Η,Α++));ΑΘ=int.Parse(Θ(Η,Α++));Ǉ=float.Parse(Θ(Η,Α++));}Η=Θ(ǵ,3).Replace("\n","").Split('|');
њ=Η.ToList();Η=Θ(ǵ,4).Split('\n');χ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
χ.Add(ΔЍ(Θ(Η,i)));Η=Θ(ǵ,5).Split('|');в=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ǲ)) ǲ=-1;if(в.Length!=е*д){в="";ǲ=-1;}ΓЀ(Θ(ǵ,6));}catch{return к.Αν;}ΑΙ=ǘ;Ǣ();return к.ΒΟ;}const int Ζν=64000;void ΔЃ(){Ǧ.Clear();for(int i=Ї.Count-1;i>=0;i--){String Ξ=Ї[i].CustomData.ToUpper();
bool ƽ=false;if(Ξ=="INSTRUCTIONS"){ƽ=true;Βν=true;}if(Ξ=="DEBUG") ƽ=true;if(ƽ){Ǧ.Add(Ї[i]);Ї.RemoveAt(i);}}ȱ(Ǧ,false,1,false);}void ΔЄ(List<IMyTerminalBlock>Βή){х.Clear();for(int i=0;i<Βή.Count;i++){IMyTerminalBlock ή=Βή[i];try{String ΓΚ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=ή.CustomName.IndexOf(ΓΚ);int ȧ=-1;if(Α<0||!int.TryParse(ή.CustomName.Substring(Α+ΓΚ.Length,1),out ȧ)) continue;if(ȧ==-1) continue;ȧ--;IMyTextSurfaceProvider ΓΜ=(IMyTextSurfaceProvider) ή;if(ȧ<ΓΜ.SurfaceCount&&ȧ>=0)
х.Add(ΓΜ.GetSurface(ȧ));}catch{}}}String ǉ="";void Δπ(){String ǅ="";String Č="";ǉ=Γν(false);ǅ+=ǉ;Č+=ǉ;Č+=ΔΕ();for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(ǅ);for(int i=0;i<х.Count;i++) х[i].WriteText(ǅ);Echo(Č);for(int i=0;i<Ǧ.Count;i++){IMyTextPanel Ќ=Ǧ[i];String Ξ=Ќ.CustomData.ToUpper();
if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Αυ());}}string Αυ(){String Β="";try{float ΒА=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ȉ*ΒА)+" / "+(int)(ȕ*ΒА)+"\n";
Β+="Inst. avg/max: "+Math.Round(ȉ*100f,1)+"% / "+Math.Round(ȕ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ą,2)+"ms / "+Math.Round(ǈ,2)+"ms \n";}catch{}for(int i=0;i<ŭ.Count;i++)
Β+=""+ŭ.Keys.ElementAt(i)+" = "+Math.Round(ŭ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ŭ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΔΡ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ȱ(Ї,setLCDFontAndSize,0.9f,false);ȱ(х,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ї.Count;i++) Ї[i].WriteText(Β+ȓ.ToString());for(int i=0;i<х.Count;i++) х[i].WriteText(Β+ȓ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ƛ(String δ,String α,String ǩ){try{if(!ć) return;δ=BROADCAST_TAG+BROADCAST_SEP+Ας()+BROADCAST_SEP+α+BROADCAST_SEP+ƪ+BROADCAST_SEP+ǩ+BROADCAST_SEP+δ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,δ);}catch(Exception e){ȓ=e;}}String Ας(){if(Π!=Ν.А) return ""+Me.GetId();
return BROADCAST_SELF_ID;}bool ΑІ(String δ){return δ.StartsWith(BROADCAST_TAG);}bool Ђ(ref String Ξ,out string Đ,Char Ƕ){int Α=Ξ.IndexOf(Ƕ);Đ="";if(Α<0) return false;Đ=Ξ.Substring(0,Α);Ξ=Ξ.Remove(0,Α+1);return true;}bool Δμ(ref String δ,out String ǜ,out String ǩ){ǜ="";ǩ="";if(!ć) return false;
String Ь="";if(!Ђ(ref δ,out Ь,BROADCAST_SEP)||!ΑІ(Ь)) return false;if(!Ђ(ref δ,out ǜ,BROADCAST_SEP)) return false;if(!Ђ(ref δ,out Ь,BROADCAST_SEP)||(Ь!=Ας()&&(Ь!="*"&&Π!=Ν.А))) return false;if(!Ђ(ref δ,out Ь,BROADCAST_SEP)||(Ь!=ƪ)) return false;if(!Ђ(ref δ,out ǩ,BROADCAST_SEP)) return false;
return true;}void ΔΩ(){bool ĉ=true;if(Βφ){Βφ=false;if(Me.CustomData.Contains("Antenna_Name")){Ψ="Update custom data";Me.CustomData="";}}String ē=(Π!=Ν.А?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ē)
ΑЃ(ē);String[]Η=Me.CustomData.Split('\n');ć=bool.Parse(ĺ(Ĵ(Η,"Enable_Broadcast",ref ĉ)));bool Αή=false;bool Βο=true;if(ć){if(Π!=Ν.А)
Ģ=ĺ(Ĵ(Η,"Ship_Name",ref ĉ)).Replace(BROADCAST_SELF_ID,"");ƪ=ĺ(Ĵ(Η,"Broadcast_Channel",ref ĉ)).ToLower();Βο=false;if(я==null){я=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);я.SetMessageCallback("");}List<IMyRadioAntenna>Ȉ=new List<IMyRadioAntenna>();Й.GetBlocksOfType(Ȉ);bool Ʀ=false;
for(int i=0;i<Ȉ.Count;i++)
if(Ȉ[i].EnableBroadcasting&&Ȉ[i].Enabled){Ʀ=true;break;}if(Ȉ.Count==0) Ψ="No Antenna found";else if(!Ʀ) Ψ="Antenna not ready";Αή=Ȉ.Count==0||!Ʀ;if(ΒΧ&&!Αή&&Π!=Ν.А) Ψ="Antenna ok";}else if(Π==Ν.А)
Ψ="Offline - Enable in PB custom data";ΒΧ=Αή;if(Βο){if(я!=null) this.IGC.DisableBroadcastListener(я);я=null;}}catch{ĉ=false;}if(!ĉ){Ψ="Reset custom data";ΑЃ(ē);}}void ΑЃ(String ē){String Ξ=ē+"\n\n"+"Enable_Broadcast="+(Π==Ν.А?"true":"false")+" \n";Ξ+=Π!=Ν.А?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ξ+"Broadcast_Channel=#default";}String ĺ(String ǯ){int Α=ǯ.IndexOf("//");if(Α!=-1) ǯ=ǯ.Substring(0,Α);String[]ε=ǯ.Split('=');if(ε.Length<=1) return "";return ε[1].Trim();}String Ĵ(String[]Η,String К,ref bool ș){foreach(String ǯ in Η)
if(ǯ.StartsWith(К)) return ǯ;ș=false;return "";}void Γϋ(string Ѝ){if(đ) σ=new List<έ>();String ǜ="";String ǩ="";if(!ΑІ(Ѝ)) Δρ(Ѝ);bool ΓΨ=false;int ΕЃ=50;while(я!=null&&я.HasPendingMessage&&ΕЃ-->0){MyIGCMessage δ=я.AcceptMessage();String Ξ=δ.Data as String;if(Ξ==null) continue;
if(Δμ(ref Ξ,out ǜ,out ǩ)&&ǜ!=""&&ǜ!=BROADCAST_SELF_ID){έ Γ=Ƅ(ǜ,"");if(Γ==null){Γ=new έ(ǜ);σ.Add(Γ);}Γ.ǎ(ǩ,false,false,true);ΕΖ(Γ,Ξ);ΓΨ=true;}}if(ΓΨ)
σ.Sort(delegate(έ a,έ b){if(a.θ==null&&b.θ==null) return 0;else if(a.θ==null) return-1;else if(b.θ==null) return 1;else return a.θ.CompareTo(b.θ);});if(Ę||đ){if(Ů<=0||đ){Ψ="";đ=false;Δω();ΔΩ();ΒЊ=DateTime.Now;}}if(Ǭ) ΕΔ();if(Ǭ) ΕΕ();if(Ǭ) ΕΘ();if(Ǭ) ΕΗ();ΕΓ();}void ΕΖ(έ Γ,String Ξ)
{Γ.ǀ=DateTime.Now;String Đ="";Ђ(ref Ξ,out Γ.ŵ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.θ,BROADCAST_SEP);if(Γ.ŵ!=VERSION) return;String ğ="";String[]Ȝ=new string[3];Ђ(ref Ξ,out ğ,BROADCAST_SEP);Ђ(ref Ξ,out Ȝ[0],BROADCAST_SEP);Ђ(ref Ξ,out Ȝ[1],BROADCAST_SEP);Ђ(ref Ξ,out Ȝ[2],BROADCAST_SEP);
Ђ(ref Ξ,out Γ.ΑΥ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ψ,BROADCAST_SEP);Ђ(ref Ξ,out Đ,BROADCAST_SEP);Ђ(ref Ξ,out Γ.Ũ,BROADCAST_SEP);String Γγ="",ΓΑ="";Ђ(ref Ξ,out Γγ,BROADCAST_SEP);Ђ(ref Ξ,out ΓΑ,BROADCAST_SEP);Γ.є.Clear();while(true){String ΒІ;if(!Ђ(ref Ξ,out ΒІ,BROADCAST_SEP)) break;String[]š=ΒІ.Split('/');
if(š.Count()<3) continue;int І=0;if(!int.TryParse(š[2],out І)) continue;Γ.є.Add(new ъ(š[0],š[1],І,Ķ.ΔΦ));}int ΓΓ=0;if(!int.TryParse(Đ,out Γ.π)) Γ.π=0;if(!int.TryParse(ğ,out ΓΓ)) Γ.Π=Ν.ǐ;if(!float.TryParse(Ȝ[0],out Γ.Ρ.X)) Γ.Ρ.X=0;if(!float.TryParse(Ȝ[1],out Γ.Ρ.Y)) Γ.Ρ.Y=0;
if(!float.TryParse(Ȝ[2],out Γ.Ρ.Z)) Γ.Ρ.Z=0;if(!float.TryParse(Γγ,out Γ.Ǥ)) Γ.Ǥ=0;if(!float.TryParse(ΓΑ,out Γ.ǰ)) Γ.ǰ=0;Γ.Π=(Ν) ΓΓ;Γ.Ʊ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Ρ));Γ.ΑΫ=0;for(int i=0;i<Γ.є.Count();i++) Γ.ΑΫ+=Γ.є[i].І;}έ Ƅ(String Ο,String θ){Ο=Ο.ToUpper();θ=θ.ToUpper();
for(int i=0;i<σ.Count;i++){if(Ο!=""&&σ[i].Ο.ToUpper()==Ο) return σ[i];if(θ!=""&&σ[i].θ!=null&&σ[i].θ.ToUpper()==θ) return σ[i];}return null;}String Ȋ="";έ Ββ=null;DateTime Βγ=DateTime.MinValue;List<έ>ё=new List<έ>();const float Γς=20f;bool ΒΚ(έ α,String ι){return ι==Ȋ&&α==Ββ&&((DateTime.Now-Βγ).TotalSeconds<Γς||
(ι=="HOMEPOS"&&(о.Count>0||Ч!=null)));}String ΓЏ(έ α){if(Ȋ==""||!ΒΚ(α,Ȋ)) return "";String s="";int ș=0,ΑΝ=0;for(int i=0;i<ё.Count;i++){ζ Ű=ё[i].ǎ(Ȋ,false,false,false);if(Ű==ζ.Ȱ) ș++;bool ΓΝ=о.Contains(ё[i]);if(ΑΝ++<8) s+="  "+ё[i].θ+": "+(ΓΝ?"queued ("+(о.IndexOf(ё[i])+1)+")":Ű==ζ.Ȱ?"received":
Ű==ζ.Ȅ?"pending...":Ű==ζ.Ĺ?"no answer!":"-")+"\n";}if(ΑΝ>8) s+="  +"+(ΑΝ-8)+" more\n";return "--------------------------------------------\n"+Ȋ+": "+ș+"/"+ё.Count+" received\n"+s;}void П(έ Γ,String ι){Ȋ=ι;Ββ=Γ;Βγ=DateTime.Now;ё.Clear();if(Γ==null) ё.AddRange(σ);else ё.Add(Γ);
if(Γ==null){о.Clear();Ч=null;}else о.Remove(Γ);if(Γ==null&&ι=="HOMEPOS"){ΔЈ();return;}if(Γ==null){for(int i=0;i<σ.Count;i++)
σ[i].ǎ(ι,true,false,false);ƛ(ι,"*",ι);}else{Γ.ǎ(ι,true,false,false);ƛ(ι,Γ.Ο,ι);}}void Δρ(string Ѝ){if(Ѝ=="") return;var ε=Ѝ.ToUpper().Split(' ');ε.DefaultIfEmpty("");var ι=ε.ElementAtOrDefault(0);var ΒΩ=ε.ElementAtOrDefault(1);String Εφ="Invalid argument: "+Ѝ;if(ΰ==Ι.Ȳ){if(ρ!=null)
{switch(ι){case "UP":{if(π<2) break;if(ρ.π==0){π=1;return;}П(ρ,"UP");return;}case "DOWN":{if(π<1) break;if(π==1){П(ρ,"MRES");break;}П(ρ,"DOWN");break;}case "APPLY":{if(π<2) break;П(ρ,"APPLY");return;}}}}switch(ι){case "UP":this.Δα(false);return;case "DOWN":this.Δΰ(false);return;
case "APPLY":this.Ȁ(true);return;}switch(ι){case "CLEAR":Γψ();return;case "SENDALL":
if(String.IsNullOrEmpty(ΒΩ)){Ψ="Usage: SENDALL <cmd>";return;}П(null,ΒΩ);return;case "SEND":
if(Ѝ.Length<="SEND".Length+1){Ψ="Usage: SEND <ship>:<cmd>";return;}Δϋ(Ѝ.Remove(0,"SEND".Length+1));return;}Ψ=Εφ;}void Δϋ(String ΒΨ){if(ΒΨ=="") return;var ε=ΒΨ.Split(':');if(ε.Length!=2){Ψ="Missing separator \":\"";return;}ε.DefaultIfEmpty("");String Ģ=ε.ElementAtOrDefault(0).Trim();
έ Γ=Ƅ("",Ģ);if(Γ!=null) П(Γ,ε.ElementAtOrDefault(1).Trim());else Ψ="Unknown ship: "+Ģ;}void Γψ(){σ.Clear();ρ=null;Χ(Ι.Main);π=0;}void Δω(){List<IMyTerminalBlock>ΓΪ=new List<IMyTerminalBlock>();Й.GetBlocksOfType(Ї,Ǝ);Й.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΓΪ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ї=Αξ(Ї,pamTag,true);ΔЄ(ΓΪ);ΔЃ();Δϊ();ȱ(Ї,setLCDFontAndSize,1.15f,false);ȱ(х,setLCDFontAndSize,1.15f,true);String Ες=Αφ(false);String ΓΖ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ќ in Ї){if(Ќ.CustomData==""){Ќ.CustomData=Αφ(true);continue;}
if(!Ќ.CustomData.Contains(Ες))
if(!Ќ.CustomData.Contains(ΓΖ)) Ќ.CustomData=ΓΖ+Ќ.CustomData;}}void ΕΓ(){String Č="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<х.Count;i++)
х[i].WriteText(Αζ(ψ.ΑЈ,"0",false));for(int i=0;i<Ї.Count;i++){ψ η=ψ.ǐ;String Ы="";Δν(Ї[i],out η,out Ы);Ї[i].WriteText(Αζ(η,Ы,i==0));}Echo(Č);for(int i=0;i<Ǧ.Count;i++){IMyTextPanel Ќ=Ǧ[i];String Ξ=Ќ.CustomData.ToUpper();if(Ξ=="DEBUG") Ќ.WriteText(""+"\n"+"");if(Ξ=="INSTRUCTIONS") Ќ.WriteText(Αυ());
}}String Ȁ(bool Ε){int Εγ=0;return Ȁ(Ε,0,ref Εγ,false,1);}String Ȁ(bool Ε,int ΕЋ,ref int ΖΗ,bool Βζ,int Ġ){String Β="";String ȥ="——————————————————\n";String ΖΥ="--------------------------------------------\n";if(ρ==null||ΰ==Ι.Main||ΰ==Ι.Ɯ)
Β+="[PAM]-Controller | "+σ.Count+" Connected ships"+"\n"+ΔΒ()+ΔΘ()+ΔΞ();else Β+=Αω(ρ)+"\n";Β+=ȥ;int Α=0;int Δ=π;Ǳ=0;if(Ψ!=""){Ζ(ref Β,Δ,Α++,Ε,Ψ);}else if(ΰ==Ι.Main){if(ρ==null&&σ.Count>=1) ρ=σ[0];String Ѕ="";if(Ζ(ref Ѕ,Δ,Α++,Ε," Send command to all")) Χ(Ι.Ɯ);
int Ȥ=0;for(int s=0;s<σ.Count;s++){if(Βζ) Ѕ+="\n";έ Γ=σ[s];if(Δ==Α||Δ==Α+1) ρ=Γ;if(Α==Δ) Ȥ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+Αω(Γ))){ρ=Γ;Χ(Ι.Ȳ);ρ.ǎ("",false,true,false);}if(Α==Δ) Ȥ=Ѕ.Split('\n').Length-1;if(Ζ(ref Ѕ,Δ,Α++,Ε," "+(Βζ?"":"  ")+ΔΚ(Γ)))
{ρ=Γ;Χ(Ι.ΒΙ);}}int ȫ=Ġ-2;ȫ+=Math.Max(0,(ΕЋ-1))*Ġ;Β+=ƚ(ȫ,Ѕ,Ȥ,ref ΖΗ);}else if(ΰ==Ι.Ļ||ΰ==Ι.Ɯ){Ι ΕΠ=ΰ==Ι.Ļ?Ι.Ȳ:Ι.Main;έ α=null;if(ΰ==Ι.Ļ) α=ρ;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(ΕΠ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+Ȃ(α,"STOP"))){П(α,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+Ȃ(α,"CONT"))){П(α,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+Ȃ(α,"HOMEPOS"))){П(α,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+Ȃ(α,"JOBPOS"))){П(α,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+Ȃ(α,"FULL"))){П(α,"FULL");}if(α!=null&&α.Π==Ν.ν)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+Ȃ(α,"UNDOCK"))){П(α,"UNDOCK");}Β+=ΓЏ(α);}else if(ΰ==Ι.Ȳ){String ƨ="";if(Ζ(ref ƨ,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=ƨ.Substring(0,ƨ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Χ(Ι.Ļ);}if(!ρ.Ə()&&!ρ.ƌ()) Ǳ++;if(ρ.ƌ()) Β+="No answer received...";
else Β+=Ȇ(ρ,ρ.Ũ);}else if(ΰ==Ι.ΒΙ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Χ(Ι.Main);}Β+=ΖΥ;Β+=Ȇ(ρ,Αψ(ρ));}if(!Ǌ) π=Math.Min(Ǳ-1,π);return Β;}String Αω(έ Γ){if(Γ.ŵ!=VERSION) return Γ.θ+": Different version!";return Γ.θ+": "+Ȇ(Γ,Γ.ΑΥ)+" "+Γ.Ʊ+"m";}String ΔΚ(έ Γ){String s=Ƙ("",О(Γ.Ǥ,Γ.ǰ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.є.Count;i++){if(i>=5) break;ъ п=Γ.є[i];s+=ΒΥ(ž(ΔΖ(п)),3)+" "+Απ(п.І)+" ";}return s;}String ΔΖ(ъ п){if(п.Њ.ToUpper()=="ORE"||п.Њ.ToUpper()=="INGOT"){String Βδ=ΔΔ(п.θ.ToUpper());if(Βδ!="") return Βδ;}return п.θ;}String Ȇ(έ Γ,String Β){if(Γ.Ə()) return "No signal...("+Ƈ((int)(DateTime.Now-Γ.ǀ).TotalSeconds)+")";
return Β;}String Αφ(bool Ζζ){String Β="";if(Ζζ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void Δν(IMyTerminalBlock Ќ,out ψ η,out String Ы){bool ș=true;String ğ=ĺ(Ĵ(Ќ.CustomData.Split('\n'),"mode",ref ș)).ToUpper();String[]ε=ğ.Split(':');String Ğ=ε.Length>0?ε[0].Trim():"";Ы=ε.Length>1?ε[1].Trim():"";
η=ψ.ǐ;if(Ğ=="MAIN") η=ψ.ΑЉ;else if(Ğ=="MAINX") η=ψ.ΑЊ;else if(Ğ=="MENU") η=ψ.ƒ;else if(Ğ=="INVENTORY") η=ψ.Ƌ;else if(Ğ=="DEBUG") η=ψ.Ɓ;if(η==ψ.ǐ) Ы=ğ;}String Αζ(ψ η,String Ы,bool Εζ){int Ĝ=15;if(Εζ){Βη=ŋ;ŋ=0;Βθ=Ō;Ō=0;}String Β="";if(η==ψ.ΑЉ){int Ћ=0;
Β=Ȁ(false,Βη,ref Εέ,true,Ĝ);if(!int.TryParse(Ы,out Ћ)) return Β;ŋ=Math.Max(Ћ,ŋ);Ћ--;return ΒΔ(Ћ,Ĝ,Β);}else if(η==ψ.ΑЊ){int Ћ=0;Β=Ȁ(false,Βθ,ref Εή,false,Ĝ);if(!int.TryParse(Ы,out Ћ)) return Β;Ō=Math.Max(Ћ,Ō);Ћ--;return ΒΔ(Ћ,Ĝ,Β);}if(η==ψ.ΑЈ){return Ȁ(false,0,ref Εί,true,Ĝ);
}else if(η==ψ.Ƌ||η==ψ.ƒ||η==ψ.Ɓ){έ Γ=ρ;if(Ы!=""){Γ=Ƅ("",Ы);if(Γ==null) return "Unknown ship: "+Ы;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ȥ="—————————————————————\n";String ΑΩ="";String Ʃ="";if(η==ψ.ƒ){ΑΩ="Menu";Ʃ=Ȇ(Γ,Γ.Ũ);}
else if(η==ψ.Ƌ){ΑΩ="Inventory";Ʃ=Ȇ(Γ,Αψ(Γ));}else if(η==ψ.Ɓ){return Ȇ(Γ,ΔΛ(Γ));}Β+=Γ.θ+" - "+Γ.Ʊ+"m | "+ž(""+ΑΩ)+"\n"+ȥ;Β+=Ʃ;return Β;}return "Unknown command: "+Ы;}string ΔΛ(έ Γ){String s="";s+=Γ.ŵ+"\n";s+=Γ.θ+"\n";s+=Γ.Ρ.X+"\n";s+=Γ.Ρ.Y+"\n";
s+=Γ.Ρ.Z+"\n";s+=Γ.ΑΥ+"\n";s+=Γ.Ψ+"\n";s+=Γ.π+"\n";s+=Γ.Ũ.Length+"\n";s+=Γ.Ǥ+"\n";s+=Γ.ǰ+"\n";s+=Γ.є.Count()+"\n";return s;}string Αψ(έ Γ){String s="";s+=Ƙ("All",О(Γ.Ǥ,Γ.ǰ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.є.Count();i++){ъ п=Γ.є[i];
s+=Ƙ(п.θ,п.І,Γ.ΑΫ,30,8,10)+"\n";}return s;}ζ ΓА(έ Γ,bool ѐ,string ι){ζ Ϊ=ζ.None;if(Γ==null){for(int i=0;i<σ.Count;i++){ζ ώ=σ[i].ǎ(ι,false,ѐ,false);if(ώ==ζ.Ȱ) Ϊ=ώ;if(ώ==ζ.Ĺ) return ώ;if(ώ==ζ.Ȅ) return ώ;}return Ϊ;}else return Γ.ǎ(ι,false,ѐ,false);}String Ȃ(έ Γ,String ι)
{if(!ΒΚ(Γ,ι)) return "";ζ ώ=ΓА(Γ,false,ι);if(ώ==ζ.Ȱ) return "received!";if(ώ==ζ.Ȅ) return "pending...";if(ώ==ζ.Ĺ) return "no answer!";return "";}String ƚ(int ȫ,String Β,int Ȥ,ref int і){String[]Η=Β.Split('\n');if(Ȥ>=і+ȫ-1) і++;і=Math.Min(Η.Count()-1-ȫ,і);if(Ȥ<і+1) і--;
і=Math.Max(0,і);String Ϊ="";for(int i=0;i<ȫ;i++){int ΒЍ=i+і;if(ΒЍ>=Η.Count()) break;Ϊ+=Η[ΒЍ]+"\n";}return Ϊ;}String ΒΔ(int Ћ,int Ġ,String Β){String[]Η=Β.Split('\n');int ΑΤ=Ћ*Ġ;int ŗ=(Ћ+1)*Ġ;String Ϊ="";for(int i=ΑΤ;i<ŗ;i++){if(i>=Η.Count()) break;Ϊ+=Η[i]+"\n";
}return Ϊ;}String ž(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΒΥ(String s,int ΒБ){if(s=="") return s;if(s.Length>ΒБ) s=s.Substring(0,ΒБ-1)+".";return s;}string Ƙ(String ǭ,float value,float Ť,int ΒΪ,int ΒЉ,int ΕЊ){float Ǵ=О(value,Ť)*ΒΪ;
String Ň="[";for(int i=0;i<ΒΪ;i++){if(i<=Ǵ) Ň+="|";else Ň+="'";}Ň+="]";return Ň+" "+ΒΥ(ž(ǭ),ΒЉ).PadRight(ΒЉ)+"".PadRight(ΕЊ)+Απ(value);}String Απ(float І){if(І>=1000000) return Math.Round(І/1000000f,І/1000000f<100?1:0)+"M";if(І>=1000) return Math.Round(І/1000f,І/1000f<100?1:0)+"K";
return ""+Math.Round(І);}String Ƈ(int ġ){if(ġ>=60*60) return Math.Round(ġ/(60f*60f),1)+" h";if(ġ>=60) return Math.Round(ġ/60f,1)+" min";return ""+ġ+" s";}List<IMyCameraBlock>ΕΦ=new List<IMyCameraBlock>();List<long>ΗΩ=new List<long>();bool ΕЌ=false;int ΕΨ=0,Ηΰ=0,ΖЂ=0;
const int Ζτ=16;int ΖЅ=0;DateTime ΖЃ=DateTime.Now;String ΕΤ="";String ΕΥ="";void Γχ(){ΕЌ=false;ΕΨ=0;ΕΤ="";}const String ΔБ="[PAMTFC]";const float Ζψ=5f;const float Ζπ=2f;class ΒΡ{public long Ο;public Vector3 ά,ћ;public Vector3 Ś;public float ΖА;public String Μ="";public int Ȑ=-1;
public bool ΓΝ;public int ǖ;public float ř,Ř;public long Х=0;public int ΗΔ=0;public DateTime ħ;}IMyBroadcastListener Ηψ=null;long ΕЄ=0;List<ΒΡ>Ητ=new List<ΒΡ>();Vector3 Ηϊ=new Vector3();float Ηϋ=-1;Vector3 Ηυ=new Vector3();bool Ηχ=false;DateTime Ηφ=DateTime.Now;const float Ζχ=15f;
const float Ζφ=10f;int Ηό=0;String в="";int ǲ=-1;bool ΖЉ=false,ΖЇ=false;Vector3 ΖЈ=new Vector3();Vector3 Ηω=new Vector3();IMyBroadcastListener Ηο=null;float Ηξ=-1;DateTime Ηπ=DateTime.MinValue;List<int>ƻ=new List<int>();String Βώ="";void ΓІ(){String К=е+"|"+д+"|"+(int) Ȓ;
if(К==Βώ) return;Βώ=К;Αη(е,д,Ȓ,ƻ);}void Αη(int w,int h,Р ΑΤ,List<int>ƭ){ƭ.Clear();if(ΑΤ==Р.Ł){for(int k=0;k<w*h;k++){int ȡ=k/w;ƭ.Add(ȡ*w+(ȡ%2==0?k-ȡ*w:w-1-(k-ȡ*w)));}return;}int Βύ=(int) Math.Ceiling(w/2f),Βό=(int) Math.Ceiling(h/2f);int Επ=w/2,Εο=h/2;
int x=0,y=0,Ʋ=0,Ƴ=-1,Ť=Math.Max(w,h);for(int i=0;i<Ť*Ť;i++){if(-Βύ<x&&x<=Επ&&-Βό<y&&y<=Εο) ƭ.Add((y-1+Βό)*w+(x-1+Βύ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ʋ;Ʋ=-Ƴ;Ƴ=t;}x+=Ʋ;y+=Ƴ;}}bool ΓЅ(){int n=Math.Max(0,е*д);
if(в.Length==n) return false;в=new String('0',n);ǲ=-1;return true;}void ΔЁ(int В,bool ϊ){if(В<0||В>=в.Length) return;char[]h=в.ToCharArray();h[В]=ϊ?'1':'0';в=new String(h);}const String Δή="[PAMJOB]";const String ΔЏ="[PAMTCC]";IMyBroadcastListener ΖЋ=null;DateTime ΖЌ=DateTime.MinValue;
DateTime ΖЍ=DateTime.Now;int ΖЊ=-1;bool ΗΛ=false;class ȅ{public long Ο;public Vector3 ά,И,Ё;}List<ȅ>Ў=new List<ȅ>();bool ċ=false;Vector3 Ŕ,œ,ŕ,Ŗ;long ĕ=0;long ΓΙ=0;Ω ΓΘ=null;bool Εα=false;Ω ΓЂ(Vector3 ά,Vector3 И,Vector3 Ё){MatrixD m=MatrixD.CreateWorld(ά,И,Ё);Ω w=new Ω(Σ);
w.Є=Vector3D.TransformNormal(Ŕ,m);w.Ϗ=Vector3D.TransformNormal(œ,m);w.Л=Vector3D.TransformNormal(ŕ,m);w.Ρ=ά+(Vector3) Vector3D.TransformNormal(Ŗ,m);w.κ=true;return w;}void Δί(){if(Π==Ν.ν||!Σ.κ) return;IMyShipConnector ђ=ȁ(MyShipConnectorStatus.Connected);if(ђ==null||ђ.OtherConnector==null) return;
IMyShipConnector Д=ђ.OtherConnector;Vector3I Εσ=new Vector3I((int) Σ.Ф.X,(int) Σ.Ф.Y,(int) Σ.Ф.Z);if(ђ.Position==Εσ){MatrixD Š=MatrixD.Transpose(Д.WorldMatrix);Ŕ=Vector3D.TransformNormal(Ј.WorldMatrix.Forward,Š);œ=Vector3D.TransformNormal(Ј.WorldMatrix.Down,Š);ŕ=Vector3D.TransformNormal(Ј.WorldMatrix.Left,Š);
Ŗ=Vector3D.TransformNormal(ђ.GetPosition()-Д.GetPosition(),Š);ċ=true;}if(Vector3.Distance(ђ.GetPosition(),Σ.Ρ)<5) ĕ=Д.EntityId;List<IMyShipConnector>Ǫ=new List<IMyShipConnector>();Й.GetBlocksOfType(Ǫ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ǫ.Count>0){Ў.Clear();for(int i=0;i<Ǫ.Count;i++){ȅ p=new ȅ();p.Ο=Ǫ[i].EntityId;p.ά=Ǫ[i].GetPosition();p.И=Ǫ[i].WorldMatrix.Forward;p.Ё=Ǫ[i].WorldMatrix.Up;Ў.Add(p);}}ΓΘ=null;ΓΙ=0;if(Д.EntityId==ĕ||!ċ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].Ο==Д.EntityId){ΓΙ=Ў[i].Ο;ΓΘ=ΓЂ(Ў[i].ά,Ў[i].И,Ў[i].Ё);}}String ΓЁ(){String s=(ċ?"1":"0")+":"+ί(Ŕ)+":"+ί(œ)+":"+
ί(ŕ)+":"+ί(Ŗ)+":"+ĕ;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].Ο+","+ί(Ў[i].ά)+","+ί(Ў[i].И)+","+ί(Ў[i].Ё);return s;}void ΓЀ(String s){Ў.Clear();ċ=false;ĕ=0;String[]f=s.Split(':');if(f.Length<6) return;ċ=f[0]=="1";Ŕ=γ(f[1]);œ=γ(f[2]);ŕ=γ(f[3]);Ŗ=γ(f[4]);long.TryParse(f[5],out ĕ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ȅ p=new ȅ();if(!long.TryParse(e[0],out p.Ο)) continue;p.ά=γ(e[1]);p.И=γ(e[2]);p.Ё=γ(e[3]);Ў.Add(p);}}const String ƀ="[PAMDCK]";const float Γρ=120f;class щ{public long Γ;public int Ǘ;public long г;public bool Βί;public DateTime З,Ě;
}IMyBroadcastListener Œ=null;List<щ>ω=new List<щ>();List<IMyShipConnector>ї=new List<IMyShipConnector>();List<IMySensorBlock>ч=new List<IMySensorBlock>();List<IMyCameraBlock>ę=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ź=new List<MyDetectedEntityInfo>();long Е=0;int Ʈ=0;
long Βΰ=0;DateTime Βα=DateTime.Now;String ǧ="";int Βς=0;IMyCubeGrid ј=null;void Δϊ(){ј=Me.CubeGrid;Й.GetBlocksOfType(ї,q=>q.CubeGrid.IsSameConstructAs(ј)&&q.CustomName.Contains(dockPoolTag));Й.GetBlocksOfType(ч,q=>q.CubeGrid.IsSameConstructAs(ј)&&q.CustomName.Contains(zoneTag));
Й.GetBlocksOfType(ę,q=>q.CubeGrid.IsSameConstructAs(ј)&&q.CustomName.Contains(laneTag));for(int i=0;i<ч.Count;i++){ч[i].DetectSmallShips=true;ч[i].DetectLargeShips=true;}for(int i=0;i<ę.Count;i++) ę[i].EnableRaycast=true;}IMyShipConnector ΒΞ(long Ο){if(Ο==0) return null;
IMyShipConnector c=Й.GetBlockWithId(Ο) as IMyShipConnector;return c!=null&&ј!=null&&c.CubeGrid.IsSameConstructAs(ј)?c:null;}bool ƍ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΑЇ(IMyShipConnector c,long Γ){for(int i=0;i<ę.Count;i++)
{IMyCameraBlock Ĉ=ę[i];if(!Ĉ.IsWorking||Vector3D.Distance(Ĉ.GetPosition(),c.GetPosition())>10) continue;Vector3D ŗ=Ĉ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!Ĉ.CanScan(ŗ)) continue;MyDetectedEntityInfo ś=Ĉ.Raycast(ŗ);if(!ś.IsEmpty()&&ś.EntityId!=ј.EntityId&&ś.EntityId!=Γ&&ś.Type!=MyDetectedEntityType.FloatingObject)
{ǧ="Lane blocked: "+c.CustomName;return false;}}return true;}bool Δά(){for(int s=0;s<ч.Count;s++){if(!ч[s].IsWorking) continue;Ź.Clear();ч[s].DetectedEntities(Ź);for(int i=0;i<Ź.Count;i++){MyDetectedEntityInfo e=Ź[i];bool Εμ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Εμ&&e.EntityId!=Е&&e.Velocity.Length()>0.5f){ǧ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Δο(щ r){IMyShipConnector г=ΒΞ(r.г);if(ƍ(г)&&ΑЇ(г,r.Γ)) return г.EntityId;if(!r.Βί||г==null) return 0;IMyShipConnector ю=null;double Βέ=0;for(int i=0;i<ї.Count;i++)
{IMyShipConnector c=ї[i];if(!ƍ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),г.GetPosition());if(ю==null||d<Βέ){if(ΑЇ(c,r.Γ)){ю=c;Βέ=d;}}}return ю==null?0:ю.EntityId;}void ΔΟ(){щ Ț=null;long ђ=0;for(int Ŭ=2;Ŭ>=1&&Ț==null;Ŭ--){щ ǳ=null;for(int i=0;i<ω.Count;i++)
{щ r=ω[i];if(r.Ǘ!=Ŭ) continue;if(Ŭ==1){long c=Δο(r);if(c==0) continue;if(ǳ==null||r.З<ǳ.З){ǳ=r;ђ=c;}}else if(ǳ==null||r.З<ǳ.З) ǳ=r;}Ț=ǳ;}if(Ț==null){if(ω.Count>0&&ǧ=="") ǧ="No free dock";return;}Е=Ț.Γ;Ʈ=Ț.Ǘ;Βΰ=Ț.Ǘ==1?ђ:0;Βα=DateTime.Now;
}void ΕΔ(){if(ј==null) return;if(Œ==null) Œ=IGC.RegisterBroadcastListener(ƀ);int ƫ=30;while(Œ.HasPendingMessage&&ƫ-->0){String Ξ=Œ.AcceptMessage().Data as String;if(Ξ!=null) ΔΠ(Ξ);}Δχ();if(++Βς<3) return;Βς=0;IGC.SendBroadcastMessage(ƀ,Γο());}void ΔΠ(String Ξ){try{String[]f=Ξ.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=ω.Count-1;i>=0;i--) if(ω[i].Γ==Γ) ω.RemoveAt(i);if(Е==Γ) Е=0;}else if(f[0]=="R"&&f.Length>=5){long г=long.Parse(f[3]);if(ΒΞ(г)==null) return;щ r=null;for(int i=0;i<ω.Count;i++) if(ω[i].Γ==Γ) r=ω[i];if(r==null){r=new щ();r.Γ=Γ;r.З=DateTime.Now;ω.Add(r);}
int Ǘ=int.Parse(f[2]);if(r.Ǘ!=Ǘ) r.З=DateTime.Now;r.Ǘ=Ǘ;r.г=г;r.Βί=f[4]=="1";r.Ě=DateTime.Now;}}catch{}}void Δχ(){for(int i=ω.Count-1;i>=0;i--)
if((DateTime.Now-ω[i].Ě).TotalSeconds>3) ω.RemoveAt(i);щ ŉ=null;for(int i=0;i<ω.Count;i++) if(ω[i].Γ==Е) ŉ=ω[i];if(Е!=0&&ŉ==null) Е=0;if(ŉ!=null&&(DateTime.Now-Βα).TotalSeconds>Γρ){Е=0;ŉ.З=DateTime.Now;}ǧ="";if(Е==0&&ω.Count>0&&!Δά()) ΔΟ();}String Γο()
{String δ="S;"+ј.EntityId+";"+Е+";"+Ʈ+";"+Βΰ+";";for(int i=0;i<ω.Count;i++) δ+=(i>0?",":"")+ω[i].Γ;δ+=";";for(int i=0;i<ї.Count;i++){IMyShipConnector c=ї[i];δ+=(i>0?":":"")+c.EntityId+","+ί(c.GetPosition())+","+ί(c.WorldMatrix.Forward)+","+ί(c.WorldMatrix.Up);
}δ+=";";for(int i=0;i<ч.Count;i++){IMySensorBlock z=ч[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();δ+=(i>0?":":"")+ί(z.GetPosition())+","+Math.Round(r,1);}return δ;}String ΔΒ()
{if(ї.Count==0&&ч.Count==0) return "";int Βχ=0;for(int i=0;i<ї.Count;i++) if(ƍ(ї[i])) Βχ++;return "Docks: "+Βχ+"/"+ї.Count+" free"+(Е!=0?(Ʈ==1?" | docking":" | undocking"):"")+
(ω.Count>1?" | "+(ω.Count-1)+" waiting":"")+(ǧ!=""?"\n"+ǧ:"")+"\n";}IMyBroadcastListener Ηη=null;int Βρ=0;DateTime ΗΖ=DateTime.Now;DateTime Ηγ=DateTime.MinValue;long Ηδ=0;int Ηε=0;long Ηζ=0;bool Ζΰ=false;int ΗΗ=0;List<Vector3>ΗЅ=new List<Vector3>();List<float>ΗІ=new List<float>();
bool ΗΕ=false;bool ΗΓ=false;Vector3 ΗΒ=new Vector3();void Δς(){if(Βρ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ƀ,"D;"+ΕЄ);Βρ=0;Ζΰ=false;}const float Δέ=600f;class ы{public long Ο;public int В=-1;public bool Γδ;public float ř,Ř;public DateTime ΑΚ;}class л{public String К;public int w,h;
public float Ζέ,ΖΤ;public String ϊ;public List<int>ΑΑ=new List<int>();public List<ы>Љ=new List<ы>();}List<л>ж=new List<л>();IMyBroadcastListener ţ=null;int ΒЈ=0;void ΕΕ(){if(ţ==null) ţ=IGC.RegisterBroadcastListener(ΔБ);int ƫ=50;while(ţ.HasPendingMessage&&ƫ-->0){String Ξ=ţ.AcceptMessage().Data as String;
if(Ξ==null) continue;try{ΔΣ(Ξ);}catch{}try{ΔΤ(Ξ);}catch{}}for(int j=ж.Count-1;j>=0;j--){л Μ=ж[j];for(int i=Μ.Љ.Count-1;i>=0;i--)
if((DateTime.Now-Μ.Љ[i].ΑΚ).TotalSeconds>Δέ) Μ.Љ.RemoveAt(i);if(Μ.Љ.Count==0){ж.RemoveAt(j);continue;}Γκ(Μ);}if(++ΒЈ<3) return;ΒЈ=0;for(int j=0;j<ж.Count;j++) IGC.SendBroadcastMessage(Δή,Γμ(ж[j]));}void Γκ(л Μ){for(int i=0;i<Μ.Љ.Count;i++){ы s=Μ.Љ[i];if(s.Γδ&&(s.В<0||Μ.ϊ[s.В]=='1')) s.В=Δβ(Μ,s);
}}String Γμ(л Μ){String δ="J;"+Μ.К+";"+Μ.ϊ+";";bool З=true;for(int i=0;i<Μ.Љ.Count;i++){if((DateTime.Now-Μ.Љ[i].ΑΚ).TotalSeconds>5) continue;δ+=(З?"":",")+Μ.Љ[i].Ο+":"+Μ.Љ[i].В;З=false;}return δ;}void ΔΣ(String Ξ){String[]f=Ξ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;л Μ=null;for(int i=0;i<ж.Count;i++) if(ж[i].К==f[4]) Μ=ж[i];if(Μ==null){Μ=new л();Μ.К=f[4];Μ.w=int.Parse(k[3]);Μ.h=int.Parse(k[4]);Μ.Ζέ=float.Parse(k[6]);Μ.ΖΤ=float.Parse(k[7]);if(Μ.w<1||Μ.h<1) return;Αη(Μ.w,Μ.h,(Р) int.Parse(k[5]),Μ.ΑΑ);
Μ.ϊ=new String('0',Μ.w*Μ.h);ж.Add(Μ);}String d=f[6];if(d.Length==Μ.ϊ.Length){char[]c=Μ.ϊ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Μ.ϊ=new String(c);}long Ο=long.Parse(f[0]);ы s=null;for(int i=0;i<Μ.Љ.Count;i++) if(Μ.Љ[i].Ο==Ο) s=Μ.Љ[i];
if(s==null){s=new ы();s.Ο=Ο;Μ.Љ.Add(s);}s.ΑΚ=DateTime.Now;String[]w=f[10].Split(',');s.Γδ=w[0]=="1";if(w.Length>=3){s.ř=float.Parse(w[1]);s.Ř=float.Parse(w[2]);}int Ȑ=int.Parse(f[5]);if(Ȑ>=0&&Ȑ<Μ.ϊ.Length&&(s.В<0||Μ.ϊ[s.В]=='1')&&Μ.ϊ[Ȑ]!='1'&&ΑЀ(Μ,Ȑ,s)==null)
s.В=Ȑ;}ы ΑЀ(л Μ,int В,ы Εε){for(int i=0;i<Μ.Љ.Count;i++){ы o=Μ.Љ[i];if(o!=Εε&&o.В==В)
return o;}return null;}int Δβ(л Μ,ы s){for(int k=0;k<Μ.ΑΑ.Count;k++){int c=Μ.ΑΑ[k];if(Μ.ϊ[c]!='1'&&ΑЀ(Μ,c,s)==null) return c;}return-2;}String ΔΘ(){String s="";for(int j=0;j<ж.Count;j++){л Μ=ж[j];int ϊ=0;for(int i=0;i<Μ.ϊ.Length;i++) if(Μ.ϊ[i]=='1') ϊ++;s+="Job "+(j+1)+": "+ϊ+"/"+Μ.ϊ.Length+" holes, "+Μ.Љ.Count+" ships\n";
}return s;}const float ΔЎ=8f;class Б{public long Ο;public Vector3 ά,ћ,Ś;public float ő,ř,Ř,Ǯ=-1;public int ǖ;public String Μ="";public int ΑΓ;public DateTime ħ;}class ŀ{public long a,b,Γε;public DateTime Ě;}List<Б>μ=new List<Б>();List<ŀ>ф=new List<ŀ>();int Γέ=0;void ΔΤ(String Ξ){
String[]f=Ξ.Split(';');if(f.Length<9) return;long Ο=long.Parse(f[0]);Б s=null;for(int i=0;i<μ.Count;i++) if(μ[i].Ο==Ο) s=μ[i];if(s==null){s=new Б();s.Ο=Ο;μ.Add(s);}s.ά=γ(f[1]);s.ћ=γ(f[2]);s.ő=float.Parse(f[3]);s.Μ=f[4];s.ΑΓ=int.Parse(f[7].Split(',')[1]);s.Ś=γ(f[8]);
s.ǖ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ř=float.Parse(w[1]);s.Ř=float.Parse(w[2]);s.ǖ=int.Parse(w[3]);}}s.ħ=DateTime.Now;}bool Ƣ(Б s){return s.ћ.LengthSquared()<0.25f&&(s.ΑΓ==3||s.Ś.LengthSquared()<0.25f);}Vector3 ΒΠ(Б s){return s.ћ.LengthSquared()>=1f?s.ћ:s.Ś*2f;}
void ΕΘ(){Γω();if(++Γέ<3) return;Γέ=0;IGC.SendBroadcastMessage(ΔЏ,Γπ());}void Γω(){for(int i=μ.Count-1;i>=0;i--)
if((DateTime.Now-μ[i].ħ).TotalSeconds>3) μ.RemoveAt(i);for(int i=ф.Count-1;i>=0;i--)
if((DateTime.Now-ф[i].Ě).TotalSeconds>2) ф.RemoveAt(i);for(int i=0;i<μ.Count;i++) μ[i].Ǯ=-1;for(int i=0;i<μ.Count;i++)
for(int j=i+1;j<μ.Count;j++){Б a=μ[i],b=μ[j];if(Ƣ(a)||Ƣ(b)) continue;Vector3 ΕЉ=a.ά+a.ћ*(float)(DateTime.Now-a.ħ).TotalSeconds;Vector3 ΕЎ=b.ά+b.ћ*(float)(DateTime.Now-b.ħ).TotalSeconds;Vector3 ȩ=ΒΠ(a),Ȫ=ΒΠ(b);Vector3 Ȟ=ΕЎ-ΕЉ;float Г=Ȟ.Length();float ѐ=(a.ő+b.ő)*0.55f;
bool ΓΣ=a.Μ!=""&&a.Μ==b.Μ;if(ΓΣ&&(a.ǖ==2||b.ǖ==2)) continue;if(ΓΣ&&a.ǖ==1&&b.ǖ==1){String[]k=a.Μ.Split('|');if(k.Length>=8) ѐ=Math.Min(float.Parse(k[6]),float.Parse(k[7]))*0.9f;}Vector3 ΑΗ=Ȫ-ȩ;float ΓΡ=ΑΗ.LengthSquared();float t=ΓΡ>0.01f?-Vector3.Dot(Ȟ,ΑΗ)/ΓΡ:0;
bool ΑΪ=Г<ѐ;if(!ΑΪ&&(t<=0||t>ΔЎ||(Ȟ+ΑΗ*t).Length()>=ѐ)) continue;if(!ΑΪ&&ȩ.LengthSquared()>0.25f&&Ȫ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(ȩ),Vector3.Normalize(Ȫ))<-0.7f)
continue;ŀ p=null;for(int k=0;k<ф.Count;k++)
if((ф[k].a==a.Ο&&ф[k].b==b.Ο)||(ф[k].a==b.Ο&&ф[k].b==a.Ο)) p=ф[k];if(p==null){p=new ŀ();p.a=a.Ο;p.b=b.Ο;p.Γε=ΔА(a,b,ȩ,Ȫ,Ȟ,Г);ф.Add(p);}p.Ě=DateTime.Now;Б y=p.Γε==a.Ο?a:b;float ΒЌ=ΑΪ||t<4?0:Math.Max(1,y.ћ.Length()*0.5f);if(y.Ǯ<0||ΒЌ<y.Ǯ) y.Ǯ=ΒЌ;
}}long ΔА(Б a,Б b,Vector3 ȩ,Vector3 Ȫ,Vector3 Ȟ,float Г){if(a.Ο==Е) return b.Ο;if(b.Ο==Е) return a.Ο;Vector3 n=Г>0.01f?Ȟ/Г:new Vector3(1,0,0);float ΓΩ=Vector3.Dot(ȩ,n),Γά=Vector3.Dot(Ȫ,-n);if(ΓΩ-Γά>1) return a.Ο;if(Γά-ΓΩ>1) return b.Ο;return a.Ο>b.Ο?a.Ο:b.Ο;
}String Γπ(){String δ="C;";bool З=true;for(int i=0;i<μ.Count;i++){if(μ[i].Ǯ<0) continue;δ+=(З?"":",")+μ[i].Ο+":"+Math.Round(μ[i].Ǯ,1);З=false;}return δ;}String ΔΞ(){int ΓΔ=0,ƺ=0;for(int i=0;i<μ.Count;i++){if(!Ƣ(μ[i])) ΓΔ++;if(μ[i].Ǯ>=0) ƺ++;}if(μ.Count==0) return "";
return "Traffic: "+ΓΔ+" moving"+(ƺ>0?", "+ƺ+" giving way":"")+"\n";}List<έ>о=new List<έ>();έ Ч=null;Vector3 ΒЃ;DateTime ΒЂ;float ΒЁ=40;bool ΒϏ=false;Б Αο(έ s){Б ю=null;for(int i=0;i<μ.Count;i++)
if(Vector3.Distance(μ[i].ά,s.Ρ)<15&&(ю==null||
Vector3.Distance(μ[i].ά,s.Ρ)<Vector3.Distance(ю.ά,s.Ρ))) ю=μ[i];return ю;}float ΔΥ(έ s){Б t=Αο(s);if(t==null) return 0;float k=t.ǖ*1000000f;String[]К=t.Μ.Split('|');if(К.Length>=3) k+=Vector3.Distance(t.ά,new Vector3(float.Parse(К[0]),float.Parse(К[1]),float.Parse(К[2])));return k;
}void ΔЈ(){о.Clear();Ч=null;List<float>ƾ=new List<float>();for(int i=0;i<σ.Count;i++){float k=ΔΥ(σ[i]);int ă=0;while(ă<ƾ.Count&&ƾ[ă]<=k) ă++;ƾ.Insert(ă,k);о.Insert(ă,σ[i]);}}void ΕΗ(){if(Ч!=null){bool Ελ=ΒϏ||Vector3.Distance(Ч.Ρ,ΒЃ)>ΒЁ||
(DateTime.Now-ΒЂ).TotalSeconds>45||Ч.Ə();if(!Ελ) return;Ч=null;}if(о.Count==0) return;Ч=о[0];о.RemoveAt(0);ΒЃ=Ч.Ρ;ΒЂ=DateTime.Now;Б t=Αο(Ч);ΒЁ=t==null?40:Math.Max(40,t.ő*3);ΒϏ=t!=null&&t.ΑΓ==3&&t.ћ.LengthSquared()<0.25f;Ч.ǎ("HOMEPOS",true,false,false);ƛ("HOMEPOS",Ч.Ο,"HOMEPOS");
}void ΔЅ(string Ѝ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}