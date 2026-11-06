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
public enum Ν{і,Ƅ,Ǹ,А,λ}public enum Λ{ǔ,ŷ,Αφ,Ɖ,ΑЁ,ȟ,є,
Ŷ,Γι,Ǵ,ΑΤ,ΓΔ,ΑΜ,
ΔΞ,Ȧ,ȥ,Ű,Δπ,Γδ,
Δξ,Δο,Γε,Γζ,ȧ}public enum Ρ{ф,Ɔ,Ǔ,ΑΥ,ĥ,Αχ,Αψ,ġ,Ģ}public enum ѐ{None,Ű,ǔ,ΓΪ}public enum Ι{Main,ΑϏ,ȡ,Ƞ,Ɛ,Ĥ,
Ŭ,Αι,Ακ,Αη,Ȥ,ΑЄ,
Į,Ƌ,Ǝ,ΑЇ,Ĳ}public enum ѕ{ΑЂ,ΓΦ,Δΰ,ΓЄ,λ}public enum К{ĳ,ΓΡ}public enum ѓ{ΑЀ,Γθ,Δβ,Ż}public enum Ш{Αϊ,ΓΩ,ΓΨ,Γΰ,Γί,ΓЂ,ΓЁ}public enum р{ų,ΓΘ,ΑΝ}public enum ĩ{ΓЀ,ΕЎ,ΓΕ}public enum Ђ{Αϋ,Αό,Αύ,ΔΔ,ΔΓ,
ΓЏ,ΓА,ΓЎ,ΔΑ,
ΔΒ,ΓБ}public enum Σ{г,х,в,ģ,ŭ,Ů}public enum ΕА{ΕЍ,ΕЈ,ΕЋ}public enum д{ΑЊ,ΑΦ,ΓЍ,Αω}public enum δ{None,ǹ,Ȣ,Ĭ}public enum π{ƒ,ū,Ɓ,Ż,Ατ,ƃ,Αυ,Δθ,ƅ}public enum φ{і,Ας,Αρ,Ƃ,ż,Ų,Ασ}class Ψ{public bool η=false;public Vector3 Τ=new Vector3();
public Vector3 ώ=new Vector3();public Vector3 Ѓ=new Vector3();public Vector3 Е=new Vector3();public Vector3 μ=new Vector3();public Vector3 О=new Vector3();public float ΕΠ=0;public float ΗΖ=0;public float[]Ц=null;public Ψ(){}public Ψ(Ψ Б){η=Б.η;Τ=Б.Τ;ώ=Б.ώ;Ѓ=Б.Ѓ;Е=Б.Е;μ=Б.μ;
О=Б.О;Ц=Б.Ц;}public Ψ(Vector3 Τ,Vector3 Ѓ,Vector3 ώ,Vector3 Е,Vector3 μ){this.Τ=Τ;this.ώ=ώ;this.Ѓ=Ѓ;this.Е=Е;this.ΕΠ=0;this.μ=μ;}public void ΕЊ(List<IMyThrust>ķ,List<string>я){Ц=new float[я.Count];for(int i=0;i<Ц.Length;i++)
Ц[i]=-1;for(int i=0;i<ķ.Count;i++){string ǰ=Γϊ(ķ[i]);int Α=я.IndexOf(ǰ);if(Α!=-1)
Ц[Α]=И(ķ[i].MaxEffectiveThrust,ķ[i].MaxThrust);}}}class ΔΗ{public Ψ ή=null;public List<Vector3>ΖЈ=new List<Vector3>();public float ΖΫ=0;public float ΗΚ=0;public float ΖΗ=0;public float Ζδ=0;public Vector3 Ζε=new Vector3();}class Ĵ{public Ђ Ж=Ђ.Αϋ;public float Ы=0;public float Ǟ=0;
public string з="";public string й="";DateTime Βρ;public bool Εέ=false;private bool Ɲ=false;public bool ΓΣ(bool ƽ){if(ƽ){Ǟ=0;Ɲ=false;return false;}Ɲ=true;return Ǟ>Ы;}public void ΕЏ(){ΓΣ(true);Εέ=false;}public void ΖΓ(){if(Ɲ)
if((DateTime.Now-Βρ).TotalSeconds>1){Ǟ++;Βρ=DateTime.Now;}}public bool Αε(){switch(Ж){case Ђ.Αό:return true;case Ђ.Αύ:return true;}return false;}}class т{public String ζ="";public String Ј="";public int Ѕ=0;public ĩ ư=ĩ.ΓΕ;public т(String ζ,String Ј,int Ѕ,ĩ ư){this.ζ=ζ;this.Ј=Ј;
this.Ѕ=Ѕ;this.ư=ư;}}class ΓΫ{public ΓΫ(Vector3 ι,float љ){this.ι=ι;this.љ=љ;}public Vector3 ι;public float љ;}class α{public DateTime ƭ;public DateTime ΒΝ;public String γ="";public String ζ="";public String Ŧ="";public String Χ="";public String Ǌ="";public String ŗ="";public String ƻ="";
public Vector3 Τ=new Vector3();public List<т>щ=new List<т>();public Ν Ξ=Ν.і;public δ ј;public float ǥ;public float ǚ;public int ξ;public int Ɵ=0;public int ΑΓ=0;public bool Απ(){return(DateTime.Now-ƭ).TotalSeconds>10;}public bool Ž(){if(ј!=δ.ǹ) return false;return(DateTime.Now-ΒΝ).TotalSeconds>=4;
}public α(String γ){this.γ=γ;}public δ Ǖ(String ρ,bool Ει,bool Ȯ,bool Δω){if(ρ==""&&!Ȯ) return δ.None;if(ј==δ.ǹ&&Ž())
ј=δ.Ĭ;if(Ει){ƻ=ρ;ј=δ.ǹ;ΒΝ=DateTime.Now;return ј;}else if(Ȯ){ƻ="";ј=δ.None;}else if(ƻ==ρ){if(Δω) ј=δ.Ȣ;return ј;}return δ.None;}}public String Γρ(String ΕΘ){switch(ΕΘ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";case "SILICON":return "Si";
case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem Д;Vector3 ΒЂ=new Vector3();Vector3 б=new Vector3();Vector3 ΔЊ=new Vector3();Vector3 ΕΛ=new Vector3();Vector3 ΔЄ=new Vector3();DateTime Βπ=new DateTime();
bool Ą=true;int ΑΑ=0;bool Č=false;bool č=false;bool ΕΥ=true;bool ΗΝ=false;bool Ζκ=false;bool ΕΔ=false;float Ελ=0;float ΒЉ=0;int Εγ=0;int ΗΗ=0;int ŝ=0;int ΖΝ=0;float Ȋ=0;float Ǿ=0;double Ƶ=0;float ȩ=0;List<int>Ȇ=new List<int>();List<int>Ȗ=new List<int>();Ν Ξ=Ν.і;
int ǁ=0;float ǯ=0;float Ǯ=0;float Ț=0;float ΒЈ=0,ΒЇ=0;IMyRemoteControl Ї;IMySensorBlock ϊ;List<IMyTimerBlock>ΒЎ=new List<IMyTimerBlock>();List<IMyShipConnector>Ȱ=new List<IMyShipConnector>();List<IMyThrust>Ě=new List<IMyThrust>();List<IMyGyro>Βδ=new List<IMyGyro>();List<IMyTerminalBlock>Ч=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ƭ=new List<IMyLandingGear>();List<IMyReactor>Ƽ=new List<IMyReactor>();List<IMyConveyorSorter>Εξ=new List<IMyConveyorSorter>();List<IMyGasTank>Ċ=new List<IMyGasTank>();List<IMyTerminalBlock>Ȩ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ΖΠ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ΖΖ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ΒΓ=new List<IMyBatteryBlock>();List<IMyTextPanel>І=new List<IMyTextPanel>();List<IMyTextSurface>н=new List<IMyTextSurface>();List<IMyTextPanel>ǜ=new List<IMyTextPanel>();IMyTerminalBlock Ǐ=null;Ψ Π=new Ψ();Ψ Βθ=new Ψ();
Ψ Βμ=new Ψ();bool ǒ=false;bool ĝ=false;bool Ğ=false;bool Βκ=false;List<String>я=new List<string>();List<Ψ>υ=new List<Ψ>();List<Ψ>ŧ=new List<Ψ>();int Ζά=0;double ΕϏ=0;Ψ Υ=new Ψ();Vector3 đ;Vector3 ȋ;Ρ ς=Ρ.ф;К ȇ=К.ĳ;int Я=0;int Ю=0;double ő=0;bool ΒΤ=false;Ĵ σ=new Ĵ();
Ĵ τ=new Ĵ();ѓ ǎ=ѓ.ΑЀ;bool ğ=true;К Ǭ=К.ĳ;int Н=3;int М=3;int Ъ=30;р ш=р.ų;bool ǲ=true;bool ċ=true;bool ā=true;bool ǭ=false;Ш Ā=Ш.Αϊ;bool Ǒ=true;float к=90;float л=20;float Т=5;float м=20;float ї=0.70f;float ũ=1.50f;float Ũ=2.50f;float Ġ=0f;float Ĉ=0f;Λ ł;
Ρ џ;Λ Ę;ΔΗ Ζφ=null;int ŋ=0;int Ō=0;int ж=0;int ď=0;int ƿ=30;int Ζψ=0;bool Εο=true;Vector3 Ζς;double ΒΩ=0;bool ΒЄ=false;int ΕЀ=0;int ΖЃ=0;int ΖΘ=0;int Ζυ=0;int Ζσ=0;Vector3 Ζχ=new Vector3();float Ζο=0;float ΖЄ=0;int Ǳ=0;Ψ Ş=null;float Ζπ=0;float ΖΙ=0;float ΒΣ=0;
float ƴ=0;bool Ζλ=false;bool ΗΕ=false;bool ΗΙ=false;bool Ζθ=false;bool ΖΛ=false;DateTime Ζγ=new DateTime();Ψ ΖΩ=null;Ψ ΔЂ=null;float ΒΔ=0;ѐ Ȫ;float Βι=0;float ΒБ=0;String ΕΞ="";float Ư=0;float ǚ=0;float ǥ=0;List<т>Ζμ=new List<т>();float Ϗ=0;bool Ǆ=false;bool Ετ=false;
bool Εμ=false;bool ΒЍ=false;bool Εν=false;float ΕΟ=0;float Εά=0;Vector3 ΔЈ=new Vector3();Vector3 Βϊ=new Vector3();Vector3 ΖЋ=new Vector3(1,1,1);float ΖЉ=1;Vector3 ΖЊ=new Vector3();bool Ζι=false;bool ΔЎ=false;bool Ζρ=false;bool ΖΕ=false;float ΖΨ=0;float ΖΔ=2;Vector3 ΖЇ;
Vector3 ΖІ;Vector3 ΖЅ;float[,]Ĝ=new float[3,2];Dictionary<String,float[,]>ю=new Dictionary<string,float[,]>();Dictionary<String,float>ΗΜ=new Dictionary<String,float>();int ΖΚ=0;Dictionary<String,float[]>Ŝ=new Dictionary<string,float[]>();float Ζω;bool ΒΦ=false;int ĸ=0;int ћ=0;
int Ŏ=0;int Ǣ=0;String Χ="";int[]Βψ=new int[Enum.GetValues(typeof(Ι)).Length];bool Ʒ=false;int Ǧ=0;int ξ=0;Ι έ=Ι.Main;int Ευ=0,Εφ=0,Εχ=0,Εψ=0;int Εδ=0,Εε=0;bool Ȭ=false;bool ΑА=false;String Ė="";String Ƙ="";IMyBroadcastListener ц=null;bool Βί=true;List<α>ϋ=null;
α ο=null;int ľ=0,Ŀ=0;int ΒΟ=0,ΒΠ=0;int ΔϏ=0,ΔЀ=0,ΔЁ=0;String ΕΙ="";Exception Ȉ=null;bool ΒЃ=false;Ρ ǀ=Ρ.ф;bool ΒΥ=false;bool Ζξ=true;bool Ζϋ=false;String Εύ="";bool ΒА=false;bool ǈ=true;int ƪ=0;int[]Ζύ=null;Vector3 ƚ=new Vector3();bool Ββ=false;bool ſ(IMyTerminalBlock Ϋ)=>Ϋ.CubeGrid==Me.CubeGrid;
Program(){Д=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Αζ(Me,controllerTag,true))
Ξ=Ν.А;Φ(Ι.Main);if(Ξ!=Ν.А){Χ="Welcome to [PAM]!";д ƾ=ΔΜ();if(Ξ==Ν.і){List<IMyShipDrill>Βέ=new List<IMyShipDrill>();List<IMyShipGrinder>Βγ=new List<IMyShipGrinder>();Д.GetBlocksOfType(Βέ,q=>q.CubeGrid==Me.CubeGrid);Д.GetBlocksOfType(Βγ,q=>q.CubeGrid==Me.CubeGrid);if(Βέ.Count>0)
{Ξ=Ν.Ƅ;Χ="Miner mode enabled!";}else if(Βγ.Count>0){Ξ=Ν.Ǹ;Χ="Grinder mode enabled!";}else{Ξ=Ν.λ;ΑІ(ѕ.λ);}}if(ƾ==д.ΑЊ) ΕΥ=false;if(ƾ==д.ΑΦ) Χ="Data restore failed!";if(ƾ==д.Αω) Χ="New version, wipe data";}}void Main(string Ќ,UpdateType Εϊ){try{if(Ȉ!=null){if(Ќ.Trim().ToUpper()=="RETRY")
{Ȉ=null;Ą=true;Εγ=0;if(Ξ!=Ν.А) Ǘ();Χ="Restarted after error";}else{Γύ();ΔΙ();return;}}č=(Εϊ&UpdateType.Update10)!=0;if(č) ΑΑ++;Č=ΑΑ>=10;if(Č) ΑΑ=0;if(č){ĸ++;if(ĸ>4) ĸ=0;ŝ=(int) Math.Max(0,10-(DateTime.Now-Βπ).TotalSeconds);}if(Ќ!="") ΕΙ=Ќ;if(Ξ!=Ν.А)
Δί(Ќ);else
ΓΧ(Ќ);Ʒ=false;try{int ΒΡ=Runtime.CurrentInstructionCount;float ǩ=И(ΒΡ,Runtime.MaxInstructionCount);if(ǩ>0.90) Χ="Max. instructions >90%";if(ǩ>Ȋ) Ȋ=ǩ;if(ΒΦ){Ȇ.Add(ΒΡ);while(Ȇ.Count>10) Ȇ.RemoveAt(0);Ǿ=0;for(int i=0;i<Ȇ.Count;i++) Ǿ+=Ȇ[i];Ǿ=И(И(Ǿ,Ȇ.Count),Runtime.MaxInstructionCount);
double Ʈ=Runtime.LastRunTimeMs;if(ΕΔ&&Ʈ>Ƶ) Ƶ=Ʈ;Ȗ.Add((int)(Ʈ*1000f));while(Ȗ.Count>10) Ȗ.RemoveAt(0);ȩ=0;for(int i=0;i<Ȗ.Count;i++) ȩ+=Ȗ[i];ȩ=И(ȩ,Ȗ.Count)/1000f;}}catch{Ȋ=0;}}catch(Exception e){Ȉ=e;}}String Γς(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ξ!=Ν.λ)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Φ(Ι Љ){Βψ[(int) έ]=ξ;ξ=Βψ[(int) Љ];if(Љ==Ι.Ȥ) ξ=0;έ=Љ;if(Ξ!=Ν.А)
Ķ(έ==Ι.ȡ,false,0,0);Ʒ=true;}void ΓЋ(bool Ʊ){if(ξ>0) ξ--;else if(Ʊ) ξ=Ǧ-1;}void ΓЊ(bool Ʊ){if(ξ<Ǧ-1) ξ++;else if(Ʊ) ξ=0;}String ΑІ(ѕ θ){switch(θ){case ѕ.ΑЂ:Χ="Job is running";break;case ѕ.ΓΦ:Χ="Connector not ready!";break;case ѕ.Δΰ:Χ="Ship modified, path outdated!";break;
case ѕ.ΓЄ:Χ="Interrupted by player!";break;case ѕ.λ:Χ="Shuttle mode enabled!";break;}return "";}String Γω(К ι){switch(ι){case К.ĳ:return "Top-Left";case К.ΓΡ:return "Center";default:return "";}}String Γξ(р ε){switch(ε){case р.ΓΘ:return "Auto"+(Ξ==Ν.Ƅ?" (Ore)":"");case р.ΑΝ:return "Auto (+Stone)";
case р.ų:return "Default";default:return "";}}String Γφ(Ρ ύ){switch(ύ){case Ρ.ф:return "No job";case Ρ.Ɔ:return "Job paused";case Ρ.Ǔ:return "Job active";case Ρ.ġ:return "Job active";case Ρ.Ģ:return "Job active";case Ρ.ΑΥ:return "Job done";case Ρ.ĥ:return "Job changed";case Ρ.Αχ:return "Move home";
case Ρ.Αψ:return "Move to job";}return "";}String ΑΫ(ѓ Δχ){switch(Δχ){case ѓ.ΑЀ:return "Return home";case ѓ.Γθ:return "Fly to job pos";case ѓ.Δβ:return "Stop";case ѓ.Ż:return "Ignore";}return "";}String Γπ(Ш ε){switch(ε){case Ш.Αϊ:return "Off";case Ш.Γΰ:return "Drop pos (Stone) ";case Ш.Γί:return "Drop pos (Sto.+Ice)";
case Ш.ΓΩ:return "Cur. pos (Stone)";case Ш.ΓΨ:return "Cur. pos (Sto.+Ice)";case Ш.ΓЂ:return "In motion (Stone)";case Ш.ΓЁ:return "In motion (Sto.+Ice)";}return "";}String ΑΩ(ѐ ύ){switch(ύ){case ѐ.None:return "No batteries";case ѐ.Ű:return "Charging";case ѐ.ΓΪ:return "Discharging";}return "";}
String Αί(Λ ύ){String Ζν=Ξ==Ν.λ?"target":"job";switch(ύ){case Λ.ǔ:return "Idle";case Λ.ŷ:return "Flying to XY position";case Λ.Αφ:return Ξ==Ν.Ǹ?"Grinding":"Mining";case Λ.Ɖ:return "Returning";case Λ.Γε:return "Flying to drop pos";case Λ.ΑЁ:return "Returning to dock";case Λ.ȟ:return "Flying to dock area";
case Λ.є:return "Flying to job area";case Λ.Ŷ:return "Flying to path";case Λ.Γι:return "Flying to job position";case Λ.Ǵ:return "Approaching dock";case Λ.ΑΤ:return "Docking";case Λ.ΓΔ:return "Aligning";case Λ.ΑΜ:return "Aligning";case Λ.ΔΞ:return "Retry docking";case Λ.Ȧ:return "Unloading";case Λ.ȧ:return Εύ;
case Λ.ȥ:return "Undocking";case Λ.Ű:return "Charging batteries";case Λ.Δπ:return "Waiting for uranium";case Λ.Γδ:return "Filling up hydrogen";case Λ.Δξ:return "Waiting for ejection";case Λ.Δο:return "Waiting for ejection";case Λ.Γζ:return "Flying to drop pos";}return "";}String Αδ(Ђ Ж){switch(Ж)
{case Ђ.Αϋ:return "On \"Undock\" command";case Ђ.Αό:return "On player entered cockpit";case Ђ.ΔΔ:return "On ship is full";case Ђ.ΔΓ:return "On ship is empty";case Ђ.Αύ:return "On time delay";case Ђ.ΓА:return "On batteries empty(<25%)";case Ђ.ΓЎ:return "On batteries empty(=0%)";case Ђ.ΓЏ:return "On batteries full";
case Ђ.ΔΒ:return "On hydrogen empty(<25%)";case Ђ.ΓБ:return "On hydrogen empty(=0%)";case Ђ.ΔΑ:return "On hydrogen full";}return "";}bool Ζ(ref String Β,int ΒІ,int њ,bool Ε,String ǣ){Ǧ+=1;if(ΒІ==њ) ǣ=">"+ǣ+(ĸ>=2?" .":"");else ǣ=" "+ǣ;Β+=ǣ+"\n";return ΒІ==њ&&Ε;}String ΓΚ(bool Ε)
{int Α=0;int Δ=ξ;Ǧ=0;String ș="———————————————\n";String Ȝ="--------------------------------------------\n";String Β="";Β+=Γφ(ς)+" | "+(Π.η?"Ready to dock":"No dock")+"\n";Β+=ș;double ΒЌ=Math.Max(Math.Round(this.ΒΩ),0);if(έ==Ι.Main){bool ќ=Ξ==Ν.λ;if(Ζ(ref Β,Δ,Α++,Ε," Record path & set home")) Δα();
if(Ξ==Ν.Ƅ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup mining job")) Φ(Ι.ȡ);if(Ξ==Ν.Ǹ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup grinding job")) Φ(Ι.ȡ);if(Ξ==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Setup shuttle job")) Φ(Ι.Ǝ);if(Ζ(ref Β,Δ,Α++,Ε," Continue job")) ΑΡ();if(Ζ(ref Β,Δ,Α++,Ε," Fly to home position")) Γη();if(Ζ(ref Β,Δ,Α++,Ε," Fly to job position")) Γκ();if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(ќ) Φ(Ι.Ĳ);else Φ(Ι.Ĥ);if(Ζ(ref Β,Δ,Α++,Ε," Info")) Φ(Ι.Αι);if(Ξ!=Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," Help")) Φ(Ι.Αη);}else if(έ==Ι.ȡ){double ΕЁ=Math.Round(Н*ǯ,1);double ΔБ=Math.Round(М*Ǯ,1);String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Start new job!")) ΑЈ();if(Ζ(ref κ,Δ,Α++,Ε," Change current job")){Ƒ(false,false);Φ(Ι.Main);}if(Ζ(ref κ,Δ,Α++,Ε," Width + (Width: "+Н+" = "+ΕЁ+"m)")){ǳ(ref Н,5,20,1);Ǽ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Width -")){ǳ(ref Н,-5,20,-1);Ǽ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height + (Height: "+М+" = "+ΔБ+"m)")){ǳ(ref М,5,20,1);Ǽ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Height -")){ǳ(ref М,-5,20,-1);Ǽ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth + ("+(ш==р.ų?"Depth":"Min")+": "+Ъ+"m)")){ǳ(ref Ъ,5,50,2);Ǽ(true);}
if(Ζ(ref κ,Δ,Α++,Ε," Depth -")){ǳ(ref Ъ,-5,50,-2);Ǽ(true);}if(Ζ(ref κ,Δ,Α++,Ε," Depth mode: "+Γξ(ш))){ш=ђ(ш);}if(Ζ(ref κ,Δ,Α++,Ε," Start pos: "+Γω(Ǭ))){Ǭ=ђ(Ǭ);}if(Ξ==Ν.Ǹ&&ш==р.ΑΝ) ш=ђ(ш);Β+=Ɗ(8,κ,Δ,ref Εδ);}else if(έ==Ι.Ǝ){float[]ΒΧ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ζ(ref Β,Δ,Α++,Ε," Next")) Φ(Ι.ΑЇ);if(Ζ(ref Β,Δ,Α++,Ε," Back")) Φ(Ι.Main);Β+=" Leave connector 1:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αδ(σ.Ж))) σ.Ж=ђ(σ.Ж);if(!σ.Αε()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ÿ((int) σ.Ы))) σ.Ы=ΑΣ(σ.Ы,ΒΧ);Β+=" Leave connector 2:\n";if(Ζ(ref Β,Δ,Α++,Ε," - "+Αδ(τ.Ж))) τ.Ж=ђ(τ.Ж);
if(!τ.Αε()) Β+="\n";else if(Ζ(ref Β,Δ,Α++,Ε," - Delay: "+Ÿ((int) τ.Ы))) τ.Ы=ΑΣ(τ.Ы,ΒΧ);}else if(έ==Ι.ΑЇ){if(Ζ(ref Β,Δ,Α++,Ε," Start job!")) ΑЈ();if(Ζ(ref Β,Δ,Α++,Ε," Back")) Φ(Ι.Ǝ);Β+=" Timer: \"Docking connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.з!=""?σ.з:"-"))) σ.з=ī(ref Ευ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(σ.й!=""?σ.й:"-"))) σ.й=ī(ref Εχ);Β+=" Timer: \"Docking connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.з!=""?τ.з:"-"))) τ.з=ī(ref Εφ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ζ(ref Β,Δ,Α++,Ε," = "+(τ.й!=""?τ.й:"-"))) τ.й=ī(ref Εψ);
}else if(έ==Ι.Ƞ){String Βσ=к+" %";if(Č) ћ++;if(ћ>1){ћ=0;Ŏ++;if(Ŏ>1) Ŏ=0;}bool[]ǅ=new bool[]{Ƽ.Count==0,Ȫ==ѐ.None,Ċ.Count==0};int ΒЏ=0;while(true){ΒЏ++;Ǣ++;if(Ǣ>ǅ.Length-1) Ǣ=0;if(ΒЏ>=ǅ.Length) break;if(!ǅ[Ǣ]) break;}bool ќ=Ξ==Ν.λ;if(!ќ&&ǲ&&Ϗ!=-1&&Ŏ==0)
Βσ=Ϗ<1000000?Math.Round(Ϗ)+" Kg":Math.Round(Ϗ/1000)+" t";if(Ζ(ref Β,Δ,Α++,Ε," Stop!")){Ǘ();Φ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Behavior settings"))
if(!ќ) Φ(Ι.Ĥ);else Φ(Ι.Ĳ);if(!ќ){if(Ζ(ref Β,Δ,Α++,Ε," Next hole")) ΓΓ(false);}else if(Ζ(ref Β,Δ,Α++,Ε," Undock")) ΒА=true;Β+=Ȝ;if(!ќ) Β+="Progress: "+Math.Round(ő,1)+" %\n";Β+="State: "+Αί(ł)+" "+ΒЌ+"m \n";Β+="Load: "+Ư+" % Max: "+Βσ+" \n";if(Ǣ==0) Β+="Uranium: "+(Ƽ.Count==0?"No reactors":Math.Round(ΒБ,1)+"Kg "+(Т==-1?"":" Min: "+Т+" Kg"))+"\n";
if(Ǣ==1) Β+="Battery: "+(Ȫ==ѐ.None?ΑΩ(Ȫ):ΒΔ+"% "+(л==-1||ќ?"":" Min: "+л+" %"))+"\n";if(Ǣ==2) Β+="Hydrogen: "+(Ċ.Count==0?"No tanks":Math.Round(Βι,1)+"% "+(м==-1||ќ?"":" Min: "+м+" %"))+"\n";}else if(έ==Ι.Ĥ){String κ="";if(Ζ(ref κ,Δ,Α++,Ε," Back")){if(ς==Ρ.Ǔ) Φ(Ι.Ƞ);else Φ(Ι.Main);}
if(Ζ(ref κ,Δ,Α++,Ε," Max load: "+к+"%")) Џ(ref к,к<=80?-10:-5,π.Ɓ,false);if(Ζ(ref κ,Δ,Α++,Ε," Weight limit: "+(ǲ?"On":"Off"))) ǲ=!ǲ;if(Ζ(ref κ,Δ,Α++,Ε," Ejection: "+Γπ(Ā))) Ā=ђ(Ā);if(Ζ(ref κ,Δ,Α++,Ε," Toggle sorters: "+(ǭ?"On":"Off"))){ǭ=!ǭ;if(ǭ) ƍ(ǈ);}
if(Ζ(ref κ,Δ,Α++,Ε," Unload ice: "+(Ǒ?"On":"Off"))) Ǒ=!Ǒ;if(Ζ(ref κ,Δ,Α++,Ε," Uranium: "+(Т==-1?"Ignore":"Min "+Т+"Kg"))) Џ(ref Т,(Т>5?-5:-1),π.ƃ,true);if(Ζ(ref κ,Δ,Α++,Ε," Battery: "+(л==-1?"Ignore":"Min "+л+"%"))) Џ(ref л,-5,π.Ατ,true);if(Ζ(ref κ,Δ,Α++,Ε," Hydrogen: "+(м==-1?"Ignore":"Min "+м+"%"))) Џ(ref м,-10,π.Αυ,true);
if(Ζ(ref κ,Δ,Α++,Ε," When done: "+(ğ?"Return home":"Stop"))) ğ=!ğ;if(Ζ(ref κ,Δ,Α++,Ε," On damage: "+ΑΫ(ǎ))) ǎ=ђ(ǎ);if(Ζ(ref κ,Δ,Α++,Ε," Advanced...")) Φ(Ι.Ŭ);Β+=Ɗ(8,κ,Δ,ref Εε);}else if(έ==Ι.Ŭ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Ρ.Ǔ) Φ(Ι.Ƞ);else Φ(Ι.Main);}
if(Ζ(ref Β,Δ,Α++,Ε,(Ξ==Ν.Ǹ?" Grinder":" Drill")+" inv. balancing: "+(ċ?"On":"Off"))) ċ=!ċ;if(Ζ(ref Β,Δ,Α++,Ε," Enable"+(Ξ==Ν.Ǹ?" grinders":" drills")+": "+(ā?"Fwd + Bwd":"Fwd"))) ā=!ā;if(Ζ(ref Β,Δ,Α++,Ε," Work speed fwd.: "+ũ+"m/s")) Џ(ref ũ,0.5f,π.ƒ,false);
if(Ζ(ref Β,Δ,Α++,Ε," Work speed bwd.: "+Ũ+"m/s")) Џ(ref Ũ,0.5f,π.ƒ,false);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ї*100f)+"%"+(ї>0.80f?" (risky)":""))) Џ(ref ї,0.1f,π.ū,false);if(Ζ(ref Β,Δ,Α++,Ε," Width overlap: "+Ġ*100f+"%")) ΑΠ(true,0.05f);if(Ζ(ref Β,Δ,Α++,Ε," Height overlap: "+Ĉ*100f+"%")) ΑΠ(false,0.05f);
}else if(έ==Ι.Ĳ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){if(ς==Ρ.Ǔ) Φ(Ι.Ƞ);else Φ(Ι.Main);}if(Ζ(ref Β,Δ,Α++,Ε," Max load: "+к+"%")) Џ(ref к,к<=80?-10:-5,π.Ɓ,false);if(Ζ(ref Β,Δ,Α++,Ε," Unload ice: "+(Ǒ?"On":"Off"))) Ǒ=!Ǒ;if(Ζ(ref Β,Δ,Α++,Ε," Uranium: "+(Т==-1?"Ignore":"Min "+Т+"Kg"))) Џ(ref Т,(Т>5?-5:-1),π.ƃ,true);
if(Ζ(ref Β,Δ,Α++,Ε," Battery: "+(л==-1?"Ignore":"Charge up"))) л=(л==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," Hydrogen: "+(м==-1?"Ignore":"Fill up"))) м=(м==-1?1:-1);if(Ζ(ref Β,Δ,Α++,Ε," On damage: "+ΑΫ(ǎ))) ǎ=ђ(ǎ);if(Ζ(ref Β,Δ,Α++,Ε," Acceleration: "+Math.Round(ї*100f)+"%"+(ї>0.80f?" (risky)":""))) Џ(ref ї,0.1f,π.ū,false);
}else if(έ==Ι.ΑϏ){double ΕΚ=0;if(υ.Count>0) ΕΚ=Vector3.Distance(υ.Last().Τ,б);if(Ζ(ref Β,Δ,Α++,Ε," Stop path recording")) Δε();if(Ξ!=Ν.λ){if(Ζ(ref Β,Δ,Α++,Ε," Home: "+(ǒ?"Use old home":(Π.η?"Was set! ":"none ")))) ǒ=!ǒ;}else{if(Ζ(ref Β,Δ,Α++,Ε," Connector 1: "+(ǒ?"Use old connector":(Π.η?"Was set! ":"none ")))) ǒ=!ǒ;
if(Ζ(ref Β,Δ,Α++,Ε," Connector 2: "+(ĝ?"Use old connector":(Υ.η?"Was set! ":"none ")))) ĝ=!ĝ;}if(Ζ(ref Β,Δ,Α++,Ε," Path: "+(Ğ?"Use old path":(υ.Count>1?"Count: "+υ.Count:"none ")))) Ğ=!Ğ;Β+=Ȝ;Β+="Wp spacing: "+Math.Round(ΕϏ)+"m\n";}else if(έ==Ι.Ɛ){if(Ζ(ref Β,Δ,Α++,Ε," Stop")){Ǘ();Φ(Ι.Main);}
Β+=Ȝ;Β+="State: "+Αί(ł)+" \n";Β+="Speed: "+Math.Round(Ελ,1)+"m/s\n";Β+="Target dist: "+ΒЌ+"m\n";Β+="Wp count: "+υ.Count+"\n";Β+="Wp left: "+ΕЀ+"\n";}else if(έ==Ι.Αι){List<IMyTerminalBlock>Ǜ=Γν();if(Č) ћ++;if(ћ>=Ǜ.Count) ћ=0;if(Ζ(ref Β,Δ,Α++,Ε," Next")) Φ(Ι.Ακ);
Β+=Ȝ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(Ư,1)+"% "+Math.Round(ǚ,1)+" / "+Math.Round(ǥ,1)+"\n";Β+="Uranium: "+(Ƽ.Count==0?"No reactors":Math.Round(ΒБ,1)+"Kg "+ΕΞ)+"\n";Β+="Battery: "+(Ȫ==ѐ.None?"":ΒΔ+"% ")+ΑΩ(Ȫ)+"\n";Β+="Hydrogen: "+(Ċ.Count==0?"No tanks":Math.Round(Βι,1)+"% ")+"\n";
Β+="Damage: "+(Ǜ.Count==0?"None":""+(ћ+1)+"/"+Ǜ.Count+" "+Ǜ[ћ].CustomName)+"\n";}else if(έ==Ι.Ακ){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Φ(Ι.Main);Β+=Ȝ;Β+="Next scan: "+ŝ+"s\n";Β+="Drill area: "+Math.Round(ǯ,1)+"x"+Math.Round(Ǯ,1)+"m Ship: "+Math.Round(ΒЈ,1)+"x"+Math.Round(ΒЇ,1)+"m ("+Math.Round(Ț,1)+"m)\n";
Β+="Broadcast: "+(Ȭ?"Online - "+Ė:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ȋ*100f,1)+"% \n";}else if(έ==Ι.Αη){if(Ζ(ref Β,Δ,Α++,Ε," Back")) Φ(Ι.Main);Β+=Ȝ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(ǁ==2) Β="Fatal setup error\nNext scan: "+ŝ+"s\n";if(ΒЃ) Β="Recompile script now";int ΕΜ=Β.Split('\n').Length;for(int i=ΕΜ;i<=10;i++) Β+="\n";Β+=ș;Β+="Last: "+Χ+"\n";
return Β;}void ΑΠ(bool ΕΗ,float Š){Ǘ();Ƒ(true,false);if(ΕΗ) Џ(ref Ġ,Š,π.ƅ,false);else Џ(ref Ĉ,Š,π.ƅ,false);ΓΠ();Ķ(true,true,0,0);}float ΑΣ(float Ȁ,float[]ť){float Ω=ť[0];for(int i=ť.Length-1;i>=0;i--)
if(Ȁ<ť[i]) Ω=ť[i];return Ω;}String ī(ref int њ){String Ω="";if(њ>=ΒЎ.Count) њ=-1;if(њ>=0) Ω=ΒЎ[њ].CustomName;њ++;return Ω;}void ǳ(ref int value,int ȫ,int Ţ,int ǆ){if(ȫ==0) return;if(value<Ţ&&ǆ>0||value<=Ţ&&ǆ<0){value+=ǆ;return;}int Δς=Math.Abs(ȫ);int Ɠ=0;int Ƹ=1;
while(true){Ɠ+=Ƹ*Δς*10;if(ȫ<0&&value-Ţ<=Ɠ) break;if(ȫ>0&&value-Ţ<Ɠ) break;Ƹ++;}value+=Ƹ*ȫ;}void Ǽ(bool ΗΘ){Н=Math.Max(Н,1);М=Math.Max(М,1);Ъ=Math.Max(Ъ,0);Ķ(έ==Ι.ȡ,false,0,0);}void Џ(ref float value,float Š,π Ј,bool ě){Џ(ref value,value+Š,Ј,ě,true);
}void Џ(ref float value,float Ȍ,π Ј,bool ě,bool ΕЄ){float У=0,С=0;if(Ј==π.ƒ){У=0.5f;С=10f;}if(Ј==π.ū){У=0.1f;С=1f;}if(Ј==π.Ż){У=50f;С=100f;}if(Ј==π.Ατ){У=5f;С=30f;}if(Ј==π.ƃ){У=1f;С=25f;}if(Ј==π.Ɓ){У=10f;С=95f;}if(Ј==π.Αυ){У=10f;С=90f;}
if(Ј==π.Δθ){У=10f;С=1800;}if(Ј==π.ƅ){У=0.0f;С=0.75f;}if(Ȍ==-1&&ě){value=-1;return;}if(value==-1) ě=false;bool Βό=Ȍ<У||Ȍ>С;if(Βό&&ΕЄ){if(Ȍ<value) value=С;else if(Ȍ>value) value=У;}else value=Ȍ;if(Βό&&ě) value=-1;else value=Math.Max(У,Math.Min(value,С));
value=(float) Math.Round(value,2);}void ΓΓ(bool Εί){if(Εί) ж=Math.Max(0,ж-1);else ж++;Γβ();Γα();for(int k=0;k<Ʃ.Count&&k<Ь.Length;k++) ΔΫ(Ʃ[k],k<ж);ǧ=-1;ΑЏ(true);}T ђ<T>(T Ȁ){int Α=Array.IndexOf(Enum.GetValues(Ȁ.GetType()),Ȁ);Α++;if(Α>=Γγ(Ȁ)) Α=0;return(T) Enum.GetValues(Ȁ.GetType()).GetValue(Α);
}int Γγ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Δα(){ŧ.Clear();for(int i=0;i<υ.Count;i++) ŧ.Add(υ[i]);υ.Clear();Βκ=true;Βθ=new Ψ(Π);Βμ=new Ψ(Υ);Π.η=false;if(Ξ==Ν.λ) Υ.η=false;for(int i=0;i<ю.Count;i++)
if(!я.Contains(ю.Keys.ElementAt(i)))
я.Add(ю.Keys.ElementAt(i));ǒ=false;ĝ=false;Ğ=false;Φ(Ι.ΑϏ);}void Δε(){if(ǒ) Π=Βθ;if(ĝ) Υ=Βμ;if(Ğ){υ.Clear();for(int i=0;i<ŧ.Count;i++) υ.Add(ŧ[i]);}Βκ=false;Ǘ();Φ(Ι.Main);}void İ(Ψ П,Ρ ύ){ΔЂ=П;if(ς==Ρ.Ǔ) џ=ύ;}bool Αά(Ψ П,float љ,bool Εϋ,out Vector3 ι){if(Εϋ){Vector3I ΔЍ=new Vector3I((int) П.О.X,(int) П.О.Y,(int) П.О.Z);
IMySlimBlock Ϋ=Me.CubeGrid.GetCubeBlock(ΔЍ);if(Ϋ==null||!(Ϋ.FatBlock is IMyShipConnector)){ι=new Vector3();return false;}Vector3 Δώ=Ī(Ї,Ϋ.FatBlock.GetPosition()-б);Vector3 Δύ=Ī(Ї,Ϋ.FatBlock.WorldMatrix.Forward);ι=П.Τ-ΑЌ(П.Ѓ,П.ώ*-1,Δώ)-ΑЌ(П.Ѓ,П.ώ*-1,Δύ)*љ;return true;}
else{ι=П.Τ;return true;}}Vector3 Γτ(int Δϋ,int ȕ,bool Εΰ){if(!Εΰ&&Ββ) return ƚ;float Δό=((Я-1f)/2f)-Δϋ;float Εβ=((Ю-1f)/2f)-ȕ;ƚ=Υ.Τ+Υ.Е*Δό*ǯ+ȋ*-1*Εβ*Ǯ;Ββ=true;return ƚ;}Σ Ŵ(){float Л=-1;Σ Ω=Σ.г;if(Ξ!=Ν.λ){ΓЉ();if(ς!=Ρ.ф){Vector3 ω=ΑЍ(đ,ȋ*-1,б-Υ.Τ);
if(Math.Abs(ω.X)<=(float)(Я*ǯ)/2f&&Math.Abs(ω.Y)<=(float)(Ю*Ǯ)/2f){if(ω.Z<=-1&&ω.Z>=-ƿ*2) return Σ.ģ;if(ω.Z>-1&&ω.Z<Ț*2) return Σ.г;}if(ё(Υ.Τ,ref Л)) Ω=Σ.г;}if(Π.η){if(ё(Π.Τ,ref Л)) Ω=Σ.в;for(int i=0;i<υ.Count;i++)
if(ё(υ[i].Τ,ref Л)) Ω=Σ.х;if(Vector3.Distance(б,Π.Τ)<dockDist*Ț) Ω=Σ.в;if(Ƕ(MyShipConnectorStatus.Connectable)!=null||Ƕ(MyShipConnectorStatus.Connected)!=null)
Ω=Σ.в;}}else{Vector3 ι=new Vector3();IMyShipConnector Ľ=Ƕ(MyShipConnectorStatus.Connected);if(Π.η){if(ё(Π.Τ,ref Л)) Ω=Σ.в;if(Αά(Π,dockDist,true,out ι))
if(ё(ι,ref Л)) Ω=Σ.в;if(Ľ!=null&&Vector3.Distance(Ľ.GetPosition(),Π.Τ)<5)
return Σ.ŭ;}for(int i=0;i<υ.Count;i++)
if(Vector3.Distance(υ[i].Τ,Π.Τ)>dockDist*Ț&&Vector3.Distance(υ[i].Τ,Υ.Τ)>dockDist*Ț)
if(ё(υ[i].Τ,ref Л)) Ω=Σ.х;if(Υ.η){if(ё(Υ.Τ,ref Л)) Ω=Σ.г;if(Αά(Υ,dockDist,true,out ι))
if(ё(ι,ref Л)) Ω=Σ.г;if(Ľ!=null&&Vector3.Distance(Ľ.GetPosition(),Υ.Τ)<5)
return Σ.Ů;}}return Ω;}bool ё(Vector3 ι,ref float Л){float љ=Vector3.Distance(ι,б);if(љ<Л||Л==-1){Л=љ;return true;}return false;}void ΑЈ(){if(ǁ>0){Χ="Setup error! Can't start";return;}if(Ξ==Ν.λ){ΑΡ();return;}Υ.Τ=б;Υ.μ=Ї.GetNaturalGravity();Υ.Ѓ=ΔЊ;Υ.ώ=ΔЄ;Υ.Е=ΕΛ;
đ=Ǐ.WorldMatrix.Forward;ȋ=Υ.ώ;if(đ==Ї.WorldMatrix.Down) ȋ=Ї.WorldMatrix.Backward;Ƒ(true,true);ί(Λ.ŷ);Ə();}void Ƒ(bool Р,bool Εα){if(ς==Ρ.ф&&!Р) return;bool ΕΦ=Р||ς==Ρ.ΑΥ||Я!=Н||Ю!=М||ȇ!=Ǭ;if(ΕΦ){if(ς!=Ρ.ф){ς=Ρ.ĥ;Γτ(ŋ,Ō,Εα);Χ="Job changed, lost progress";}ȇ=Ǭ;
Я=Н;Ю=М;Ō=0;ŋ=0;Ь="";ǧ=-1;ƴ=0;ď=0;ΒΣ=0;ж=0;ΑЏ(true);}}void Ƈ(){ΔΩ(б,0);į(Ě,true);}void Ə(){ΑІ(ѕ.ΑЂ);Ƈ();ƌ(Ƭ,false);ƍ(ǈ);ς=Ρ.Ǔ;ΓΟ(true);џ=ς;Φ(Ι.Ƞ);ΔΡ();ΒΤ=true;ƪ=0;for(int i=Ȩ.Count-1;i>=0;i--)
if(Αμ(Ȩ[i],false)) ƪ++;if(ƪ>0) Χ="Started with damage";}void Ǘ(){if(ς==Ρ.Ǔ){ς=Ρ.Ɔ;Χ="Job paused";}ί(Λ.ǔ);џ=ς;ΔΪ(false,0,0,0,0);Δγ();ΓΖ(new Vector3(),false);Δδ();ΔΦ(ChargeMode.Auto);Δά(false);ΔΨ(true);ǖ(Λ.ǔ);Ķ(false,false,0,0);į(Ч,false);į(Ȱ,true);ƍ(true);ΒЄ=false;ΓΤ();
ΔЃ=false;ΔΛ();ΒΤ=false;Ǆ=false;ΒА=false;if(έ!=Ι.Main&&έ!=Ι.Ĥ&&έ!=Ι.Ŭ&&έ!=Ι.Ĳ)
Φ(Ι.Main);}void ΑΡ(){Σ ъ=Ŵ();if(Ξ==Ν.λ){if(!Υ.η||!Π.η) return;Ə();bool Ʀ=Vector3.Distance(б,Π.Τ)<Vector3.Distance(б,Υ.Τ);if(ǀ==Ρ.ġ) Ʀ=true;if(ǀ==Ρ.Ģ) Ʀ=false;if(Ʀ){İ(Π,Ρ.ġ);switch(ъ){case Σ.ŭ:ί(Λ.ȧ);break;case Σ.х:ί(Λ.є);break;case Σ.в:ί(Λ.Ǵ);break;default:ί(Λ.ȥ);break;
}}else{İ(Υ,Ρ.Ģ);switch(ъ){case Σ.Ů:ί(Λ.ȧ);break;case Σ.г:ί(Λ.Ǵ);break;case Σ.х:ί(Λ.є);break;default:ί(Λ.ȥ);break;}}}else{if(ς!=Ρ.Ɔ&&ς!=Ρ.ĥ) return;bool Εώ=ς==Ρ.ĥ;Ə();bool ƨ=ΓЅ(false)&&Π.η;switch(ъ){case Σ.г:ί(ƨ?Λ.Ŷ:Λ.ŷ);break;case Σ.х:ί(ƨ?Λ.ȟ:Λ.є);break;case Σ.в:ί(ƨ?Λ.Ǵ:Λ.Ȧ);break;
case Σ.ģ:{if(ď!=ж||Εώ) ί(Λ.Ɖ);else ί(Λ.Αφ);}break;default:break;}}}void Γκ(){if(ς==Ρ.ф&&!Π.η) return;if(Ξ==Ν.λ&&(!Υ.η||!Π.η)) return;Χ="Move to job";Σ ъ=Ŵ();if(Ξ==Ν.λ){İ(Υ,Ρ.Ģ);switch(ъ){case Σ.г:ί(Λ.Ǵ);break;case Σ.х:ί(Λ.є);break;case Σ.Ů:return;default:ί(Λ.ȥ);break;
}ǖ(Λ.ȧ);}else{switch(ъ){case Σ.г:ί(Λ.є);break;case Σ.х:ί(Λ.є);break;case Σ.в:ί(Λ.Ȧ);break;case Σ.ģ:ί(Λ.Ɖ);break;default:break;}if(ς==Ρ.ф) ǖ(Λ.є);else ǖ(Λ.ΑΜ);ΒЄ=true;}Ƈ();Φ(Ι.Ɛ);ƌ(Ƭ,false);џ=Ρ.Αψ;}void Γη(){if(!Π.η) return;Χ="Move home";Σ ъ=Ŵ();if(Ξ==Ν.λ){İ(Π,Ρ.ġ);switch(ъ)
{case Σ.х:ί(Λ.ȟ);break;case Σ.в:ί(Λ.Ǵ);break;case Σ.ŭ:return;default:ί(Λ.ȥ);break;}ǖ(Λ.ȧ);}else{if(Ƕ(MyShipConnectorStatus.Connected)!=null) return;if(Ƕ(MyShipConnectorStatus.Connectable)!=null){ί(Λ.ΑΤ);ǖ(Λ.Ȧ);return;}switch(ъ){case Σ.г:ί(Λ.Ŷ);break;case Σ.х:ί(Λ.ȟ);break;case Σ.в:ί(Λ.ȟ);break;
case Σ.ģ:ί(Λ.ΑЁ);break;default:break;}ǖ(Λ.Ȧ);}Ƈ();Φ(Ι.Ɛ);ƌ(Ƭ,false);џ=Ρ.Αχ;}void ί(Λ ƹ){if(ƹ==Λ.ǔ) Ę=Λ.ǔ;if(Ę!=Λ.ǔ&&ł==Ę&&ƹ!=Ę){Ǘ();return;}Εο=true;ł=ƹ;}void ǖ(Λ ύ){this.Ę=ύ;}void ΑЏ(bool ƽ){if(ƽ){ő=0;return;}if(Ξ==Ν.λ) return;float ψ=ж*Math.Max(1,Ъ);if(ď==ж) ψ+=Math.Min(Ъ,ΒΣ);
float Εω=Я*Ю*Math.Max(1,Ъ);ő=Math.Max(ő,(float) Math.Min(ψ/Εω*100.0,100));}List<IMyTerminalBlock>Γν(){List<IMyTerminalBlock>Ǜ=new List<IMyTerminalBlock>();for(int i=0;i<Ȩ.Count;i++){IMyTerminalBlock Ϋ=Ȩ[i];if(Αμ(Ϋ,false)) Ǜ.Add(Ϋ);}return Ǜ;}bool Αμ(IMyTerminalBlock Ϋ,bool ǋ)
{return(!Αν(Ϋ,ǋ)||!Ϋ.IsFunctional);}bool Αν(IMyTerminalBlock Ϋ,bool ǋ){if(Ϋ==null) return false;try{IMyCubeBlock Βα=Me.CubeGrid.GetCubeBlock(Ϋ.Position).FatBlock;if(ǋ) return Βα==Ϋ;else return Βα.GetType()==Ϋ.GetType();}catch{return false;}}bool ΓЅ(bool Ζό){if(ǲ&&Ξ!=Ν.λ)
if(Ϗ!=-1&&ΒЉ>=Ϗ){Χ="Ship too heavy";return true;}if(Ư>=к||Ǆ){Ǆ=false;Χ="Ship is full";return true;}return false;}static String Γϊ(IMyThrust о){return о.BlockDefinition.SubtypeId;}Vector3 ƀ(Vector3 В,float[,]ȉ){return new Vector3(
В.X>=0?ȉ[0,0]:ȉ[0,1],
В.Y>=0?ȉ[1,0]:ȉ[1,1],
В.Z>=0?ȉ[2,0]:ȉ[2,1]);}bool Αγ(Ψ Ϊ,String ǰ,out float Ƣ){Ƣ=0;int Α=я.IndexOf(ǰ);if(Α==-1||Ϊ.Ц==null||Α>=Ϊ.Ц.Length)
return false;Ƣ=Ϊ.Ц[Α];if(Ƣ==-1) return false;return true;}Vector3 Αβ(Vector3 В,Ψ Ϊ){if(Ϊ!=null){Vector3 Ω=new Vector3();for(int i=0;i<ю.Keys.Count;i++){String ǰ=ю.Keys.ElementAt(i);float ȳ=0;if(!Αγ(Ϊ,ǰ,out ȳ))
return ƀ(В,Ĝ);Ω+=ƀ(В,ю.Values.ElementAt(i))*ȳ;}return Ω;}return ƀ(В,Ĝ);}float ź(Vector3 В,Ψ Ϊ){return ź(В,new Vector3(),Ϊ);}float ź(Vector3 В,Vector3 μ,Ψ Ϊ){Vector3 Εσ=Αβ(В,Ϊ);Vector3 ΔІ=Εσ+μ*ΒЉ;float ǩ=(ΔІ/В).AbsMin();return(float)(В*ǩ).Length();}float Ĩ(Vector3 ą,Vector3 Ѐ,Vector3 μ,Ψ Ϊ)
{if(μ.Length()==0f) return 0;Vector3 ΕΝ=ΑЍ(ą,Ѐ,Vector3.Normalize(μ));float Р=ź(-ΕΝ,Ϊ);return Р/μ.Length();}void ΓΟ(bool ΕΓ){float З=0;float Ƥ=0.9f;if(ΕΓ){Ϗ=-1;Ǳ=0;Ş=null;if(ς!=Ρ.ф&&Υ.μ.Length()!=0){З=Ƥ*Ĩ(Υ.Ѓ,Υ.ώ*-1,Υ.μ,null);if(З<Ϗ||Ϗ==-1) Ϗ=З;}if(Π.η&&Π.μ.Length()!=0)
{З=Ƥ*Ĩ(Π.Ѓ,Π.ώ*-1,Π.μ,null);if(З<Ϗ||Ϗ==-1) Ϗ=З;}return;}if(Ǳ==-1) return;if(Ǳ>=0){int ΒЀ=0;while(Ǳ<υ.Count){if(ΒЀ>100) return;ΒЀ++;Ψ Ϊ=υ[Ǳ];if(Ϊ.μ.Length()!=0f){З=Ƥ*Math.Min(
Ĩ(Ϊ.Ѓ,Ϊ.ώ*-1,Ϊ.μ,Ϊ),
Ĩ(Ϊ.Ѓ*-1,Ϊ.ώ*-1,Ϊ.μ,Ϊ));if(З<Ϗ||Ϗ==-1) Ϗ=З;}else Ş=Ϊ;Ǳ++;}Ǳ=-1;}bool Ƨ=true;float Ǎ=0;if(Ş!=null){for(int i=0;i<ю.Count;i++){String ǰ=ю.Keys.ElementAt(i);float[,]ΑΔ=ю.Values.ElementAt(i);float ȳ=0;if(!Αγ(Ş,ǰ,out ȳ)){Ƨ=false;break;}for(int a=0;a<ΑΔ.GetLength(0);a++)
for(int b=0;b<ΑΔ.GetLength(1);b++){float ǘ=Math.Abs(ΑΔ[a,b]*ȳ);if(ǘ==0) continue;Ƨ=true;if(Ǎ==0||ǘ<Ǎ) Ǎ=ǘ;}}}if(!Ƨ){for(int a=0;a<Ĝ.GetLength(0);a++)
for(int b=0;b<Ĝ.GetLength(1);b++){float ǘ=Math.Abs(Ĝ[a,b]);if(ǘ==0) continue;if(Ǎ==0||ǘ<Ǎ) Ǎ=ǘ;}}if(Ǎ>0){float ΕΡ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;З=И(Ǎ,ΕΡ);if(З>0)
if(З<Ϗ||Ϗ==-1) Ϗ=З;}}void ΓΖ(Vector3 Р,bool ό){if(!ό){for(int i=0;i<Ě.Count;i++)
Ě[i].SetValueFloat("Override",0.0f);return;}Vector3 ƕ=Αβ(Р,null);float ΕЅ=Math.Min(1,Math.Abs(И(Р.X,ƕ.X)));float ΕІ=Math.Min(1,Math.Abs(И(Р.Y,ƕ.Y)));float ΕЇ=Math.Min(1,Math.Abs(И(Р.Z,ƕ.Z)));for(int i=0;i<Ě.Count;i++){IMyThrust о=Ě[i];Vector3 ȁ=ΔΟ(Ī(Ї,о.WorldMatrix.Backward),1);
if(ȁ.X!=0&&Math.Sign(ȁ.X)==Math.Sign(Р.X))
о.SetValueFloat("Override",о.MaxThrust*ΕЅ);else if(ȁ.Y!=0&&Math.Sign(ȁ.Y)==Math.Sign(Р.Y))
о.SetValueFloat("Override",о.MaxThrust*ΕІ);else if(ȁ.Z!=0&&Math.Sign(ȁ.Z)==Math.Sign(Р.Z))
о.SetValueFloat("Override",о.MaxThrust*ΕЇ);else
о.SetValueFloat("Override",0.0f);}}void ΔΪ(bool ό,float Εή,float Ȑ,float Ȕ,float Ȟ){for(int i=0;i<Βδ.Count;i++){IMyGyro ν=Βδ[i];ν.GyroOverride=ό;if(!ό) ν.GyroPower=100;else ν.GyroPower=Εή;if(!ό) continue;Vector3 Г=Ї.WorldMatrix.Forward;Vector3 ȓ=Ї.WorldMatrix.Right;Vector3 Ѐ=Ї.WorldMatrix.Up;
Vector3 Ȃ=ν.WorldMatrix.Forward;Vector3 Ȅ=ν.WorldMatrix.Up;Vector3 ȃ=ν.WorldMatrix.Left*-1;if(Ȃ==Г) ν.SetValueFloat("Roll",Ȟ);else if(Ȃ==(Г*-1)) ν.SetValueFloat("Roll",Ȟ*-1);else if(Ȅ==(Г*-1)) ν.SetValueFloat("Yaw",Ȟ);else if(Ȅ==Г) ν.SetValueFloat("Yaw",Ȟ*-1);else if(ȃ==Г) ν.SetValueFloat("Pitch",Ȟ);
else if(ȃ==(Г*-1)) ν.SetValueFloat("Pitch",Ȟ*-1);if(ȃ==(ȓ*-1)) ν.SetValueFloat("Pitch",Ȑ);else if(ȃ==ȓ) ν.SetValueFloat("Pitch",Ȑ*-1);else if(Ȅ==ȓ) ν.SetValueFloat("Yaw",Ȑ);else if(Ȅ==(ȓ*-1)) ν.SetValueFloat("Yaw",Ȑ*-1);else if(Ȃ==(ȓ*-1)) ν.SetValueFloat("Roll",Ȑ);
else if(Ȃ==ȓ) ν.SetValueFloat("Roll",Ȑ*-1);if(Ȅ==(Ѐ*-1)) ν.SetValueFloat("Yaw",Ȕ);else if(Ȅ==Ѐ) ν.SetValueFloat("Yaw",Ȕ*-1);else if(ȃ==Ѐ) ν.SetValueFloat("Pitch",Ȕ);else if(ȃ==(Ѐ*-1)) ν.SetValueFloat("Pitch",Ȕ*-1);else if(Ȃ==Ѐ) ν.SetValueFloat("Roll",Ȕ);else if(Ȃ==(Ѐ*-1)) ν.SetValueFloat("Roll",Ȕ*-1);
}}void Δδ(){this.ΔЎ=false;}void Δγ(){ı(false,false,false,Βϊ,0);ΒЍ=false;}void ΔΩ(Vector3 ή,float ė){ı(true,false,false,ή,ė);}void ı(bool ό,bool Ǉ,bool ΑΕ,Vector3 ή,float ė){ı(ό,Ǉ,ΑΕ,ή,ή-б,0.0f,ė);}void ı(bool ό,bool Ǉ,bool ΑΕ,Vector3 ή,Vector3 Εη,float Εθ,float ė){ΒЍ=true;
this.Ετ=ό;Βϊ=ή;this.ΕΟ=ė;this.Εά=Εθ;this.Εν=Ǉ;this.Εμ=ΑΕ;this.ΔЈ=Εη;ΒΩ=Vector3.Distance(ή,б);}void ΔΡ(){Д.GetBlocksOfType(Ȩ,ſ);}void ΓΠ(){if(Ї==null) return;Ǐ=null;float Ř=0,Ŕ=0,ř=0,ŕ=0,Ś=0,Ŗ=0;List<IMyTerminalBlock>ǌ=ΑΧ(Ч,pamTag,true);bool ΕΨ=ǌ.Count==0;
if(ǌ.Count>0) Ǐ=ǌ[0];else if(Ч.Count>0) Ǐ=Ч[0];int Ł=0;float Ȓ=0;for(int i=0;i<Ч.Count;i++){if(Ч[i].WorldMatrix.Forward!=Ǐ.WorldMatrix.Forward){if(ΕΨ){ǁ=2;Χ="Mining direction is unclear!";return;}continue;}Ł++;Ȓ=Math.Max(Ȓ,Δι(Ч[i]));Vector3 ω=Ź(Ї,Ч[i].GetPosition());
if(i==0){Ř=ω.X;Ŕ=ω.X;ř=ω.Y;ŕ=ω.Y;Ś=ω.Z;Ŗ=ω.Z;}Ŕ=Math.Max(ω.X,Ŕ);Ř=Math.Min(ω.X,Ř);ŕ=Math.Max(ω.Y,ŕ);ř=Math.Min(ω.Y,ř);Ŗ=Math.Max(ω.Z,Ŗ);Ś=Math.Min(ω.Z,Ś);}if(Ȓ==0) Ȓ=drillRadius;bool Βά=Ǐ!=null&&Ǐ.WorldMatrix.Forward==Ї.WorldMatrix.Down;ǯ=(Ŕ-Ř)*(1-Ġ)+Ȓ*2;
Ǯ=(Βά?Ŗ-Ś:ŕ-ř)*(1-Ĉ)+Ȓ*2;IMyCubeGrid g=Ї.CubeGrid;float ΑΘ=0,ΑΙ=0,ΑΚ=0,ΑΛ=0;for(int c=0;c<8;c++){Vector3 p=Ź(Ї,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Βά?p.Z:p.Y;if(c==0||p.X<ΑΘ) ΑΘ=p.X;if(c==0||p.X>ΑΙ) ΑΙ=p.X;if(c==0||y<ΑΚ) ΑΚ=y;if(c==0||y>ΑΛ) ΑΛ=y;}ΒЈ=ΑΙ-ΑΘ+g.GridSize;ΒЇ=ΑΛ-ΑΚ+g.GridSize;}float Δι(IMyTerminalBlock b)
{if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ΑΧ<T>(List<T>Є,String Х,bool ō){List<T>Ω=new List<T>();
for(int i=0;i<Є.Count;i++)
if(Αζ(Є[i],Х,ō)) Ω.Add(Є[i]);return Ω;}bool Αζ<T>(T Ϋ,String Х,bool ō){IMyTerminalBlock b=(IMyTerminalBlock) Ϋ;if(ō&&b.CustomName.ToUpper().Contains(Х.ToUpper())) return true;if(!ō&&b.BlockDefinition.ToString().ToUpper().Contains(Х.ToUpper())) return true;return false;}void į<T>(List<T>Є,bool ό)
{for(int i=0;i<Є.Count;i++)
ΔΧ((IMyTerminalBlock) Є[i],ό);}void ΔΧ(IMyTerminalBlock Ϋ,bool ό){if(Ϋ==null) return;String Δσ=ό?"OnOff_On":"OnOff_Off";var a=Ϋ.GetActionWithName(Δσ);a.Apply(Ϋ);}void ƍ(bool ό){ǈ=ό;if(!ǭ) return;į(Εξ,ό);}void ΔΦ(ChargeMode ε){for(int i=0;i<ΒΓ.Count;i++) ΒΓ[i].ChargeMode=ε;}void Δά(bool Ερ)
{for(int i=0;i<Ċ.Count;i++) Ċ[i].Stockpile=Ερ;}void ƌ(List<IMyLandingGear>ƥ,bool Βτ){for(int i=0;i<ƥ.Count;i++){if(Βτ) ƥ[i].Lock();if(!Βτ) ƥ[i].Unlock();}}void ΔΨ(bool ό){if(ΒΥ==ό) return;List<IMyShipController>ŀ=new List<IMyShipController>();Д.GetBlocksOfType(ŀ,ſ);if(ŀ.Count==0) return;
for(int i=0;i<ŀ.Count;i++)
ŀ[i].DampenersOverride=ό;ΒΥ=ό;}IMyShipConnector Ƕ(MyShipConnectorStatus Επ){for(int i=0;i<Ȱ.Count;i++){if(!Αν(Ȱ[i],true)) continue;if(Ȱ[i].Status==Επ) return Ȱ[i];}return null;}void Ķ(bool ǂ,bool Ń,float ΔЋ,float Δφ){if(ϊ==null||Ч.Count==0) return;Vector3 Ļ=new Vector3();int Ł=0;
for(int i=0;i<Ч.Count;i++){if(Ч[i].WorldMatrix.Forward!=Ǐ.WorldMatrix.Forward) continue;Ł++;Ļ+=Ч[i].GetPosition();}Ļ=Ļ/Ł;Vector3 ȍ=Ź(ϊ,Ļ);ϊ.Enabled=true;ϊ.ShowOnHUD=ǂ;ϊ.LeftExtend=(Ń?1:Н)/2f*ǯ-ȍ.X;ϊ.RightExtend=(Ń?1:Н)/2f*ǯ+ȍ.X;ϊ.TopExtend=(Ń?1:М)/2f*Ǯ+ȍ.Y;
ϊ.BottomExtend=(Ń?1:М)/2f*Ǯ-ȍ.Y;ϊ.FrontExtend=(ǂ?Ъ:ΔЋ)-ȍ.Z;ϊ.BackExtend=ǂ?0:Δφ+Ț*0.75f+ȍ.Z;ϊ.DetectFloatingObjects=true;ϊ.DetectAsteroids=false;ϊ.DetectLargeShips=true;ϊ.DetectSmallShips=true;ϊ.DetectStations=true;ϊ.DetectOwner=true;ϊ.DetectSubgrids=false;
ϊ.DetectPlayers=false;ϊ.DetectEnemy=true;ϊ.DetectFriendly=true;ϊ.DetectNeutral=true;}void ȣ<T>(List<T>Є,bool Εκ,float ΔЉ,bool ΕΖ){for(int i=0;i<Є.Count;i++){IMyTextSurface ę=null;if(Є[i]is IMyTextSurface) ę=(IMyTextSurface) Є[i];if(ę!=null){ę.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Εκ) continue;ę.Font="Debug";if(ΕΖ) continue;ę.FontSize=ΔЉ;}}}static float И(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΔΟ(Vector3 v,int ƞ){return new Vector3(Math.Round(v.X,ƞ),Math.Round(v.Y,ƞ),Math.Round(v.Z,ƞ));}Vector3 Ź(IMyTerminalBlock Ϋ,Vector3 ΕЃ){return Ī(Ϋ,ΕЃ-Ϋ.GetPosition());
}Vector3 Ī(IMyTerminalBlock Ϋ,Vector3 ΑΗ){return Vector3D.TransformNormal(ΑΗ,MatrixD.Transpose(Ϋ.WorldMatrix));}Vector3 ΑЍ(Vector3 ą,Vector3 Ѐ,Vector3 ΑΗ){MatrixD Ƴ=MatrixD.CreateFromDir(ą,Ѐ);return Vector3D.TransformNormal(ΑΗ,MatrixD.Transpose(Ƴ));}Vector3 ΑЌ(Vector3 ą,Vector3 Ѐ,Vector3 ω)
{MatrixD Ƴ=MatrixD.CreateFromDir(ą,Ѐ);return Vector3D.Transform(ω,Ƴ);}String ά(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 β(String s){String[]ΰ=s.Split('|');return new Vector3(float.Parse(Θ(ΰ,0)),float.Parse(Θ(ΰ,1)),float.Parse(Θ(ΰ,2)));}String Δρ(Ψ Ϊ){String ǫ=":";String Ω=ά(Ϊ.Τ)+ǫ+ά(Ϊ.Ѓ)+ǫ+
ά(Ϊ.ώ)+ǫ+ά(Ϊ.Е)+ǫ+ά(Ϊ.μ);for(int i=0;i<Ϊ.Ц.Length;i++)
Ω+=ǫ+Math.Round(Ϊ.Ц[i],3);return Ω;}Ψ Δζ(String s){String[]ΰ=s.Split(':');Ψ Ϊ=new Ψ(
β(Θ(ΰ,0)),β(Θ(ΰ,1)),
β(Θ(ΰ,2)),β(Θ(ΰ,3)),
β(Θ(ΰ,4)));int Α=5;List<float>Βή=new List<float>();while(Α<ΰ.Length){String Ť=Θ(ΰ,Α);float f=0;if(!float.TryParse(Ť,out f)) break;Βή.Add(f);Α++;}Ϊ.Ц=Βή.ToArray();return Ϊ;}System.Text.StringBuilder ȗ=new System.Text.StringBuilder();void Κ<T>(T value,bool ΕΧ){if(ΕΧ) ȗ.Append('\n');ȗ.Append(value);}
void Κ<T>(T value){Κ(value,true);}String Ħ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ĵ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Θ(String[]Δτ,int Α){String Ť=Δτ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Ť)) return "";
return Ť;}void Save(){if(ΒЃ||Ξ==Ν.А){Storage="";return;}Storage=ΓΛ();}String ΓΛ(){ȗ.Clear();ȗ.Append(DATAREV+";");Κ(ά(ΒЂ),false);Κ(ά(Π.Ѓ));Κ(ά(Π.Е));Κ(ά(Π.ώ));Κ(ά(Π.μ));Κ(ά(Π.Τ));Κ(ά(Π.О));Κ(Π.η);Κ(ά(Υ.Τ));Κ(ά(Υ.μ));Κ(ά(Υ.Ѓ));Κ(ά(Υ.ώ));Κ(ά(Υ.Е));Κ(ά(Υ.О));Κ(Υ.η);Κ(ά(đ));Κ(ά(ȋ));Κ(";");
Κ((int) Ξ,false);Κ((int) ς);Κ((int) џ);Κ(к);Κ(л);Κ(Т);Κ(м);Κ(ї);Κ(ǲ);Κ(Ǒ);if(Ξ==Ν.λ){Κ((int) σ.Ж);Κ(σ.Ы);Κ(σ.Ǟ);Κ(Ħ(σ.з));Κ(Ħ(σ.й));Κ((int) τ.Ж);Κ(τ.Ы);Κ(τ.Ǟ);Κ(Ħ(τ.з));Κ(Ħ(τ.й));}else{Κ((int) Ǭ);Κ((int) ǎ);Κ((int) Ā);Κ((int) ш);Κ((int) ȇ);Κ(ğ);Κ(ǭ);Κ(ċ);Κ(ā);Κ(Н);Κ(М);Κ(Ъ);Κ(ũ);Κ(Ũ);Κ(Ġ);Κ(Ĉ);
Κ(Я);Κ(Ю);Κ(ŋ);Κ(Ō);Κ(ď);Κ(ж);Κ(ƿ);Κ(ƴ);}Κ(";");for(int i=0;i<я.Count;i++)
Κ((i>0?"|":"")+я[i],false);Κ(";");for(int i=0;i<υ.Count;i++)
Κ(Δρ(υ[i]),i>0);Κ(";",false);Κ(Ь+"|"+ǧ,false);Κ(";",false);Κ(Γέ(),false);String Ο=ȗ.ToString();ȗ.Clear();return Ο;}д ΔΜ(){return ΔΝ(Storage);}д ΔΝ(String Ο){if(Ο=="") return д.ΓЍ;String[]Ǫ=Ο.Split(';');if(Θ(Ǫ,0)!=DATAREV) return д.Αω;int Α=0;try{String[]Η=Θ(Ǫ,1).Split('\n');
ΒЂ=β(Θ(Η,Α++));Π.Ѓ=β(Θ(Η,Α++));Π.Е=β(Θ(Η,Α++));Π.ώ=β(Θ(Η,Α++));Π.μ=β(Θ(Η,Α++));Π.Τ=β(Θ(Η,Α++));Π.О=β(Θ(Η,Α++));Π.η=bool.Parse(Θ(Η,Α++));Υ.Τ=β(Θ(Η,Α++));Υ.μ=β(Θ(Η,Α++));Υ.Ѓ=β(Θ(Η,Α++));Υ.ώ=β(Θ(Η,Α++));Υ.Е=β(Θ(Η,Α++));Υ.О=β(Θ(Η,Α++));Υ.η=bool.Parse(Θ(Η,Α++));
đ=β(Θ(Η,Α++));ȋ=β(Θ(Η,Α++));Η=Θ(Ǫ,2).Split('\n');Α=0;Ξ=(Ν) int.Parse(Θ(Η,Α++));ς=(Ρ) int.Parse(Θ(Η,Α++));џ=(Ρ) int.Parse(Θ(Η,Α++));к=int.Parse(Θ(Η,Α++));л=int.Parse(Θ(Η,Α++));Т=int.Parse(Θ(Η,Α++));м=int.Parse(Θ(Η,Α++));ї=float.Parse(Θ(Η,Α++));ǲ=bool.Parse(Θ(Η,Α++));
Ǒ=bool.Parse(Θ(Η,Α++));if(Ξ==Ν.λ){σ.Ж=(Ђ) int.Parse(Θ(Η,Α++));σ.Ы=float.Parse(Θ(Η,Α++));σ.Ǟ=float.Parse(Θ(Η,Α++));σ.з=ĵ(Θ(Η,Α++));σ.й=ĵ(Θ(Η,Α++));τ.Ж=(Ђ) int.Parse(Θ(Η,Α++));τ.Ы=float.Parse(Θ(Η,Α++));τ.Ǟ=float.Parse(Θ(Η,Α++));τ.з=ĵ(Θ(Η,Α++));τ.й=ĵ(Θ(Η,Α++));}else{Ǭ=(К) int.Parse(Θ(Η,Α++));
ǎ=(ѓ) int.Parse(Θ(Η,Α++));Ā=(Ш) int.Parse(Θ(Η,Α++));ш=(р) int.Parse(Θ(Η,Α++));ȇ=(К) int.Parse(Θ(Η,Α++));ğ=bool.Parse(Θ(Η,Α++));ǭ=bool.Parse(Θ(Η,Α++));ċ=bool.Parse(Θ(Η,Α++));ā=bool.Parse(Θ(Η,Α++));Н=int.Parse(Θ(Η,Α++));М=int.Parse(Θ(Η,Α++));Ъ=int.Parse(Θ(Η,Α++));ũ=float.Parse(Θ(Η,Α++));
Ũ=float.Parse(Θ(Η,Α++));Ġ=float.Parse(Θ(Η,Α++));Ĉ=float.Parse(Θ(Η,Α++));Я=int.Parse(Θ(Η,Α++));Ю=int.Parse(Θ(Η,Α++));ŋ=int.Parse(Θ(Η,Α++));Ō=int.Parse(Θ(Η,Α++));ď=int.Parse(Θ(Η,Α++));ж=int.Parse(Θ(Η,Α++));ƿ=int.Parse(Θ(Η,Α++));ƴ=float.Parse(Θ(Η,Α++));}Η=Θ(Ǫ,3).Replace("\n","").Split('|');
я=Η.ToList();Η=Θ(Ǫ,4).Split('\n');υ.Clear();if(Η.Count()>=1&&Η[0]!="")
for(int i=0;i<Η.Length;i++)
υ.Add(Δζ(Θ(Η,i)));Η=Θ(Ǫ,5).Split('|');Ь=Θ(Η,0);if(!int.TryParse(Θ(Η,1),out ǧ)) ǧ=-1;if(Ь.Length!=Я*Ю){Ь="";ǧ=-1;}Γά(Θ(Ǫ,6));}catch{return д.ΑΦ;}ǀ=џ;Ǘ();return д.ΑЊ;}const int ΕЉ=64000;void Δέ(){ǜ.Clear();for(int i=І.Count-1;i>=0;i--){String Ο=І[i].CustomData.ToUpper();
bool ƫ=false;if(Ο=="INSTRUCTIONS"){ƫ=true;ΒΦ=true;}if(Ο=="DEBUG") ƫ=true;if(ƫ){ǜ.Add(І[i]);І.RemoveAt(i);}}ȣ(ǜ,false,1,false);}void Δή(List<IMyTerminalBlock>ΒΖ){н.Clear();for(int i=0;i<ΒΖ.Count;i++){IMyTerminalBlock Ϋ=ΒΖ[i];try{String ΒϏ=pamTag.Substring(0,pamTag.Length-1)+":";
int Α=Ϋ.CustomName.IndexOf(ΒϏ);int ț=-1;if(Α<0||!int.TryParse(Ϋ.CustomName.Substring(Α+ΒϏ.Length,1),out ț)) continue;if(ț==-1) continue;ț--;IMyTextSurfaceProvider ΒЁ=(IMyTextSurfaceProvider) Ϋ;if(ț<ΒЁ.SurfaceCount&&ț>=0)
н.Add(ΒЁ.GetSurface(ț));}catch{}}}String ƶ="";void ΔΙ(){String Ʋ="";String Ȳ="";ƶ=ΓΚ(false);Ʋ+=ƶ;Ȳ+=ƶ;Ȳ+=Γς();for(int i=0;i<І.Count;i++) І[i].WriteText(Ʋ);for(int i=0;i<н.Count;i++) н[i].WriteText(Ʋ);Echo(Ȳ);for(int i=0;i<ǜ.Count;i++){IMyTextPanel Њ=ǜ[i];String Ο=Њ.CustomData.ToUpper();
if(Ο=="DEBUG") Њ.WriteText(""+"\n"+"");if(Ο=="INSTRUCTIONS") Њ.WriteText(Αέ());}}string Αέ(){String Β="";try{float Βυ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(Ǿ*Βυ)+" / "+(int)(Ȋ*Βυ)+"\n";
Β+="Inst. avg/max: "+Math.Round(Ǿ*100f,1)+"% / "+Math.Round(Ȋ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(ȩ,2)+"ms / "+Math.Round(Ƶ,2)+"ms \n";}catch{}for(int i=0;i<Ŝ.Count;i++)
Β+=""+Ŝ.Keys.ElementAt(i)+" = "+Math.Round(Ŝ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ŝ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void Γύ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ȣ(І,setLCDFontAndSize,0.9f,false);ȣ(н,setLCDFontAndSize,0.9f,true);for(int i=0;i<І.Count;i++) І[i].WriteText(Β+Ȉ.ToString());for(int i=0;i<н.Count;i++) н[i].WriteText(Β+Ȉ.ToString());}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";
const Char BROADCAST_SEP=';';void ΑЃ(String θ,String ή,String ǟ){try{if(!Ȭ) return;θ=BROADCAST_TAG+BROADCAST_SEP+ΑΪ()+BROADCAST_SEP+ή+BROADCAST_SEP+Ƙ+BROADCAST_SEP+ǟ+BROADCAST_SEP+θ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,θ);}catch(Exception e){Ȉ=e;}}String ΑΪ(){
if(Ξ!=Ν.А) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Αξ(String θ){return θ.StartsWith(BROADCAST_TAG);}bool Ё(ref String Ο,out string ă,Char ǫ){int Α=Ο.IndexOf(ǫ);ă="";if(Α<0) return false;ă=Ο.Substring(0,Α);Ο=Ο.Remove(0,Α+1);return true;}bool ΔΕ(ref String θ,out String ǐ,out String ǟ)
{ǐ="";ǟ="";if(!Ȭ) return false;String Х="";if(!Ё(ref θ,out Х,BROADCAST_SEP)||!Αξ(Х)) return false;if(!Ё(ref θ,out ǐ,BROADCAST_SEP)) return false;if(!Ё(ref θ,out Х,BROADCAST_SEP)||(Х!=ΑΪ()&&(Х!="*"&&Ξ!=Ν.А))) return false;if(!Ё(ref θ,out Х,BROADCAST_SEP)||(Х!=Ƙ)) return false;
if(!Ё(ref θ,out ǟ,BROADCAST_SEP)) return false;return true;}void ΓЃ(){bool ȯ=true;if(Βί){Βί=false;if(Me.CustomData.Contains("Antenna_Name")){Χ="Update custom data";Me.CustomData="";}}String ć=(Ξ!=Ν.А?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ć)
Αλ(ć);String[]Η=Me.CustomData.Split('\n');Ȭ=bool.Parse(ĭ(ħ(Η,"Enable_Broadcast",ref ȯ)));bool ΑΖ=false;bool ΒΨ=true;if(Ȭ){if(Ξ!=Ν.А)
Ė=ĭ(ħ(Η,"Ship_Name",ref ȯ)).Replace(BROADCAST_SELF_ID,"");Ƙ=ĭ(ħ(Η,"Broadcast_Channel",ref ȯ)).ToLower();ΒΨ=false;if(ц==null){ц=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ц.SetMessageCallback("");}List<IMyRadioAntenna>ǽ=new List<IMyRadioAntenna>();Д.GetBlocksOfType(ǽ);bool Ɣ=false;
for(int i=0;i<ǽ.Count;i++)
if(ǽ[i].EnableBroadcasting&&ǽ[i].Enabled){Ɣ=true;break;}if(ǽ.Count==0) Χ="No Antenna found";else if(!Ɣ) Χ="Antenna not ready";ΑΖ=ǽ.Count==0||!Ɣ;if(ΑА&&!ΑΖ&&Ξ!=Ν.А) Χ="Antenna ok";}else if(Ξ==Ν.А)
Χ="Offline - Enable in PB custom data";ΑА=ΑΖ;if(ΒΨ){if(ц!=null) this.IGC.DisableBroadcastListener(ц);ц=null;}}catch{ȯ=false;}if(!ȯ){Χ="Reset custom data";Αλ(ć);}}void Αλ(String ć){String Ο=ć+"\n\n"+"Enable_Broadcast="+(Ξ==Ν.А?"true":"false")+" \n";Ο+=Ξ!=Ν.А?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Ο+"Broadcast_Channel=#default";}String ĭ(String Ǥ){int Α=Ǥ.IndexOf("//");if(Α!=-1) Ǥ=Ǥ.Substring(0,Α);String[]ΰ=Ǥ.Split('=');if(ΰ.Length<=1) return "";return ΰ[1].Trim();}String ħ(String[]Η,String ѝ,ref bool Ȏ){foreach(String Ǥ in Η)
if(Ǥ.StartsWith(ѝ)) return Ǥ;Ȏ=false;return "";}void ΓΧ(string Ќ){if(Ą) ϋ=new List<α>();String ǐ="";String ǟ="";if(!Αξ(Ќ)) ΔΚ(Ќ);bool ΒЊ=false;int ΕΣ=50;while(ц!=null&&ц.HasPendingMessage&&ΕΣ-->0){MyIGCMessage θ=ц.AcceptMessage();String Ο=θ.Data as String;if(Ο==null) continue;
if(ΔΕ(ref Ο,out ǐ,out ǟ)&&ǐ!=""&&ǐ!=BROADCAST_SELF_ID){α Γ=ŵ(ǐ,"");if(Γ==null){Γ=new α(ǐ);ϋ.Add(Γ);}Γ.Ǖ(ǟ,false,false,true);Δν(Γ,Ο);ΒЊ=true;}}if(ΒЊ)
ϋ.Sort(delegate(α a,α b){if(a.ζ==null&&b.ζ==null) return 0;else if(a.ζ==null) return-1;else if(b.ζ==null) return 1;else return a.ζ.CompareTo(b.ζ);});if(Č||Ą){if(ŝ<=0||Ą){Χ="";Ą=false;ΔΣ();ΓЃ();Βπ=DateTime.Now;}}if(č) Δλ();if(č) Δμ();Δκ();}void Δν(α Γ,String Ο){Γ.ƭ=DateTime.Now;
String ă="";Ё(ref Ο,out Γ.Ŧ,BROADCAST_SEP);Ё(ref Ο,out Γ.ζ,BROADCAST_SEP);if(Γ.Ŧ!=VERSION) return;String ē="";String[]ȑ=new string[3];Ё(ref Ο,out ē,BROADCAST_SEP);Ё(ref Ο,out ȑ[0],BROADCAST_SEP);Ё(ref Ο,out ȑ[1],BROADCAST_SEP);Ё(ref Ο,out ȑ[2],BROADCAST_SEP);Ё(ref Ο,out Γ.Ǌ,BROADCAST_SEP);
Ё(ref Ο,out Γ.Χ,BROADCAST_SEP);Ё(ref Ο,out ă,BROADCAST_SEP);Ё(ref Ο,out Γ.ŗ,BROADCAST_SEP);String ΓΑ="",Βχ="";Ё(ref Ο,out ΓΑ,BROADCAST_SEP);Ё(ref Ο,out Βχ,BROADCAST_SEP);Γ.щ.Clear();while(true){String Βλ;if(!Ё(ref Ο,out Βλ,BROADCAST_SEP)) break;String[]Ő=Βλ.Split('/');if(Ő.Count()<3) continue;
int Ѕ=0;if(!int.TryParse(Ő[2],out Ѕ)) continue;Γ.щ.Add(new т(Ő[0],Ő[1],Ѕ,ĩ.ΓЀ));}int Βω=0;if(!int.TryParse(ă,out Γ.ξ)) Γ.ξ=0;if(!int.TryParse(ē,out Βω)) Γ.Ξ=Ν.і;if(!float.TryParse(ȑ[0],out Γ.Τ.X)) Γ.Τ.X=0;if(!float.TryParse(ȑ[1],out Γ.Τ.Y)) Γ.Τ.Y=0;if(!float.TryParse(ȑ[2],out Γ.Τ.Z)) Γ.Τ.Z=0;
if(!float.TryParse(ΓΑ,out Γ.ǚ)) Γ.ǚ=0;if(!float.TryParse(Βχ,out Γ.ǥ)) Γ.ǥ=0;Γ.Ξ=(Ν) Βω;Γ.Ɵ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Γ.Τ));Γ.ΑΓ=0;for(int i=0;i<Γ.щ.Count();i++) Γ.ΑΓ+=Γ.щ[i].Ѕ;}α ŵ(String γ,String ζ){γ=γ.ToUpper();ζ=ζ.ToUpper();for(int i=0;i<ϋ.Count;i++)
{if(γ!=""&&ϋ[i].γ.ToUpper()==γ) return ϋ[i];if(ζ!=""&&ϋ[i].ζ!=null&&ϋ[i].ζ.ToUpper()==ζ) return ϋ[i];}return null;}String ǿ="";α ΒΚ=null;DateTime ΒΛ=DateTime.MinValue;List<α>Ǚ=new List<α>();const float ΓΞ=20f;bool ΑЅ(α ή,String ρ){return ρ==ǿ&&ή==ΒΚ&&(DateTime.Now-ΒΛ).TotalSeconds<ΓΞ;
}String Γλ(α ή){if(ǿ==""||!ΑЅ(ή,ǿ)) return "";String s="";int Ȏ=0,ǃ=0;for(int i=0;i<Ǚ.Count;i++){δ ş=Ǚ[i].Ǖ(ǿ,false,false,false);if(ş==δ.Ȣ) Ȏ++;if(ǃ++<8) s+="  "+Ǚ[i].ζ+": "+(ş==δ.Ȣ?"received":ş==δ.ǹ?"pending...":ş==δ.Ĭ?"no answer!":"-")+"\n";
}if(ǃ>8) s+="  +"+(ǃ-8)+" more\n";return "--------------------------------------------\n"+ǿ+": "+Ȏ+"/"+Ǚ.Count+" received\n"+s;}void Й(α Γ,String ρ){ǿ=ρ;ΒΚ=Γ;ΒΛ=DateTime.Now;Ǚ.Clear();if(Γ==null) Ǚ.AddRange(ϋ);else Ǚ.Add(Γ);if(Γ==null){for(int i=0;i<ϋ.Count;i++)
ϋ[i].Ǖ(ρ,true,false,false);ΑЃ(ρ,"*",ρ);}else{Γ.Ǖ(ρ,true,false,false);ΑЃ(ρ,Γ.γ,ρ);}}void ΔΚ(string Ќ){if(Ќ=="") return;var ΰ=Ќ.ToUpper().Split(' ');ΰ.DefaultIfEmpty("");var ρ=ΰ.ElementAtOrDefault(0);var ΒΑ=ΰ.ElementAtOrDefault(1);String ΕΕ="Invalid argument: "+Ќ;if(έ==Ι.Ȥ){if(ο!=null)
{switch(ρ){case "UP":{if(ξ<2) break;if(ο.ξ==0){ξ=1;return;}Й(ο,"UP");return;}case "DOWN":{if(ξ<1) break;if(ξ==1){Й(ο,"MRES");break;}Й(ο,"DOWN");break;}case "APPLY":{if(ξ<2) break;Й(ο,"APPLY");return;}}}}switch(ρ){case "UP":this.ΓЋ(false);return;case "DOWN":this.ΓЊ(false);return;
case "APPLY":this.ǵ(true);return;}switch(ρ){case "CLEAR":ΓΥ();return;case "SENDALL":
if(String.IsNullOrEmpty(ΒΑ)){Χ="Usage: SENDALL <cmd>";return;}Й(null,ΒΑ);return;case "SEND":
if(Ќ.Length<="SEND".Length+1){Χ="Usage: SEND <ship>:<cmd>";return;}ΔΥ(Ќ.Remove(0,"SEND".Length+1));return;}Χ=ΕΕ;}void ΔΥ(String ΑБ){if(ΑБ=="") return;var ΰ=ΑБ.Split(':');if(ΰ.Length!=2){Χ="Missing separator \":\"";return;}ΰ.DefaultIfEmpty("");String Ė=ΰ.ElementAtOrDefault(0).Trim();
α Γ=ŵ("",Ė);if(Γ!=null) Й(Γ,ΰ.ElementAtOrDefault(1).Trim());else Χ="Unknown ship: "+Ė;}void ΓΥ(){ϋ.Clear();ο=null;Φ(Ι.Main);ξ=0;}void ΔΣ(){List<IMyTerminalBlock>ΒЋ=new List<IMyTerminalBlock>();Д.GetBlocksOfType(І,ſ);Д.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΒЋ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);І=ΑΧ(І,pamTag,true);Δή(ΒЋ);Δέ();ΔΤ();ȣ(І,setLCDFontAndSize,1.15f,false);ȣ(н,setLCDFontAndSize,1.15f,true);String ΕΑ=Αή(false);String Βϋ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Њ in І){if(Њ.CustomData==""){Њ.CustomData=Αή(true);continue;}
if(!Њ.CustomData.Contains(ΕΑ))
if(!Њ.CustomData.Contains(Βϋ)) Њ.CustomData=Βϋ+Њ.CustomData;}}void Δκ(){String Ȳ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<н.Count;i++)
н[i].WriteText(ΑΞ(φ.Αρ,"0",false));for(int i=0;i<І.Count;i++){φ ε=φ.і;String Ф="";ΔΖ(І[i],out ε,out Ф);І[i].WriteText(ΑΞ(ε,Ф,i==0));}Echo(Ȳ);for(int i=0;i<ǜ.Count;i++){IMyTextPanel Њ=ǜ[i];String Ο=Њ.CustomData.ToUpper();if(Ο=="DEBUG") Њ.WriteText(""+"\n"+"");if(Ο=="INSTRUCTIONS") Њ.WriteText(Αέ());
}}String ǵ(bool Ε){int ΔЅ=0;return ǵ(Ε,0,ref ΔЅ,false,1);}String ǵ(bool Ε,int ΕΪ,ref int Εζ,bool ΒΞ,int Ĕ){String Β="";String ș="——————————————————\n";String Ες="--------------------------------------------\n";if(ο==null||έ==Ι.Main||έ==Ι.Ƌ)
Β+="[PAM]-Controller | "+ϋ.Count+" Connected ships"+"\n"+Γο()+Γυ();else Β+=Αα(ο)+"\n";Β+=ș;int Α=0;int Δ=ξ;Ǧ=0;if(Χ!=""){Ζ(ref Β,Δ,Α++,Ε,Χ);}else if(έ==Ι.Main){if(ο==null&&ϋ.Count>=1) ο=ϋ[0];String Є="";if(Ζ(ref Є,Δ,Α++,Ε," Send command to all")) Φ(Ι.Ƌ);
int Ș=0;for(int s=0;s<ϋ.Count;s++){if(ΒΞ) Є+="\n";α Γ=ϋ[s];if(Δ==Α||Δ==Α+1) ο=Γ;if(Α==Δ) Ș=Є.Split('\n').Length-1;if(Ζ(ref Є,Δ,Α++,Ε," "+Αα(Γ))){ο=Γ;Φ(Ι.Ȥ);ο.Ǖ("",false,true,false);}if(Α==Δ) Ș=Є.Split('\n').Length-1;if(Ζ(ref Є,Δ,Α++,Ε," "+(ΒΞ?"":"  ")+Γχ(Γ)))
{ο=Γ;Φ(Ι.ΑЄ);}}int ȝ=Ĕ-2;ȝ+=Math.Max(0,(ΕΪ-1))*Ĕ;Β+=Ɗ(ȝ,Є,Ș,ref Εζ);}else if(έ==Ι.Į||έ==Ι.Ƌ){Ι Δυ=έ==Ι.Į?Ι.Ȥ:Ι.Main;α ή=null;if(έ==Ι.Į) ή=ο;if(Ζ(ref Β,Δ,Α++,Ε," Back")){Φ(Δυ);}if(Ζ(ref Β,Δ,Α++,Ε," [Stop] "+Ƿ(ή,"STOP"))){Й(ή,"STOP");}if(Ζ(ref Β,Δ,Α++,Ε," [Continue] "+Ƿ(ή,"CONT"))){Й(ή,"CONT");}
if(Ζ(ref Β,Δ,Α++,Ε," [Move home] "+Ƿ(ή,"HOMEPOS"))){Й(ή,"HOMEPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Move to job] "+Ƿ(ή,"JOBPOS"))){Й(ή,"JOBPOS");}if(Ζ(ref Β,Δ,Α++,Ε," [Full simulation] "+Ƿ(ή,"FULL"))){Й(ή,"FULL");}if(ή!=null&&ή.Ξ==Ν.λ)
if(Ζ(ref Β,Δ,Α++,Ε," [Undock] "+Ƿ(ή,"UNDOCK"))){Й(ή,"UNDOCK");}Β+=Γλ(ή);}else if(έ==Ι.Ȥ){String Ɩ="";if(Ζ(ref Ɩ,Δ,Α++,Ε," Back")){Φ(Ι.Main);}Β+=Ɩ.Substring(0,Ɩ.Length-1).PadRight(36,' ');if(Ζ(ref Β,Δ,Α++,Ε," Send cmd")){Φ(Ι.Į);}if(!ο.Απ()&&!ο.Ž()) Ǧ++;if(ο.Ž()) Β+="No answer received...";
else Β+=ǻ(ο,ο.ŗ);}else if(έ==Ι.ΑЄ){if(Ζ(ref Β,Δ,Α++,Ε," Back")){Φ(Ι.Main);}Β+=Ες;Β+=ǻ(ο,Αΰ(ο));}if(!Ʒ) ξ=Math.Min(Ǧ-1,ξ);return Β;}String Αα(α Γ){if(Γ.Ŧ!=VERSION) return Γ.ζ+": Different version!";return Γ.ζ+": "+ǻ(Γ,Γ.Ǌ)+" "+Γ.Ɵ+"m";}String Γχ(α Γ){String s=ƈ("",И(Γ.ǚ,Γ.ǥ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Γ.щ.Count;i++){if(i>=5) break;т и=Γ.щ[i];s+=ΑЎ(ů(Γσ(и)),3)+" "+ΑΨ(и.Ѕ)+" ";}return s;}String Γσ(т и){if(и.Ј.ToUpper()=="ORE"||и.Ј.ToUpper()=="INGOT"){String ΒΜ=Γρ(и.ζ.ToUpper());if(ΒΜ!="") return ΒΜ;}return и.ζ;}String ǻ(α Γ,String Β){if(Γ.Απ()) return "No signal...("+Ÿ((int)(DateTime.Now-Γ.ƭ).TotalSeconds)+")";
return Β;}String Αή(bool ΕЂ){String Β="";if(ΕЂ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΔΖ(IMyTerminalBlock Њ,out φ ε,out String Ф){bool Ȏ=true;String ē=ĭ(ħ(Њ.CustomData.Split('\n'),"mode",ref Ȏ)).ToUpper();String[]ΰ=ē.Split(':');String Ē=ΰ.Length>0?ΰ[0].Trim():"";Ф=ΰ.Length>1?ΰ[1].Trim():"";
ε=φ.і;if(Ē=="MAIN") ε=φ.Ας;else if(Ē=="MAINX") ε=φ.Ασ;else if(Ē=="MENU") ε=φ.Ƃ;else if(Ē=="INVENTORY") ε=φ.ż;else if(Ē=="DEBUG") ε=φ.Ų;if(ε==φ.і) Ф=ē;}String ΑΞ(φ ε,String Ф,bool ΔЇ){int Đ=15;if(ΔЇ){ΒΟ=ľ;ľ=0;ΒΠ=Ŀ;Ŀ=0;}String Β="";if(ε==φ.Ας){int Љ=0;
Β=ǵ(false,ΒΟ,ref ΔϏ,true,Đ);if(!int.TryParse(Ф,out Љ)) return Β;ľ=Math.Max(Љ,ľ);Љ--;return Αώ(Љ,Đ,Β);}else if(ε==φ.Ασ){int Љ=0;Β=ǵ(false,ΒΠ,ref ΔЀ,false,Đ);if(!int.TryParse(Ф,out Љ)) return Β;Ŀ=Math.Max(Љ,Ŀ);Љ--;return Αώ(Љ,Đ,Β);}if(ε==φ.Αρ){return ǵ(false,0,ref ΔЁ,true,Đ);
}else if(ε==φ.ż||ε==φ.Ƃ||ε==φ.Ų){α Γ=ο;if(Ф!=""){Γ=ŵ("",Ф);if(Γ==null) return "Unknown ship: "+Ф;}else{if(Γ==null) return "No ship on main screen selected.";Β="Selected: ";}String ș="—————————————————————\n";String ΑΒ="";String Ɨ="";if(ε==φ.Ƃ){ΑΒ="Menu";Ɨ=ǻ(Γ,Γ.ŗ);}
else if(ε==φ.ż){ΑΒ="Inventory";Ɨ=ǻ(Γ,Αΰ(Γ));}else if(ε==φ.Ų){return ǻ(Γ,Γψ(Γ));}Β+=Γ.ζ+" - "+Γ.Ɵ+"m | "+ů(""+ΑΒ)+"\n"+ș;Β+=Ɨ;return Β;}return "Unknown command: "+Ф;}string Γψ(α Γ){String s="";s+=Γ.Ŧ+"\n";s+=Γ.ζ+"\n";s+=Γ.Τ.X+"\n";s+=Γ.Τ.Y+"\n";
s+=Γ.Τ.Z+"\n";s+=Γ.Ǌ+"\n";s+=Γ.Χ+"\n";s+=Γ.ξ+"\n";s+=Γ.ŗ.Length+"\n";s+=Γ.ǚ+"\n";s+=Γ.ǥ+"\n";s+=Γ.щ.Count()+"\n";return s;}string Αΰ(α Γ){String s="";s+=ƈ("All",И(Γ.ǚ,Γ.ǥ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Γ.щ.Count();i++){т и=Γ.щ[i];
s+=ƈ(и.ζ,и.Ѕ,Γ.ΑΓ,30,8,10)+"\n";}return s;}δ Γμ(α Γ,bool Ȯ,string ρ){δ Ω=δ.None;if(Γ==null){for(int i=0;i<ϋ.Count;i++){δ ύ=ϋ[i].Ǖ(ρ,false,Ȯ,false);if(ύ==δ.Ȣ) Ω=ύ;if(ύ==δ.Ĭ) return ύ;if(ύ==δ.ǹ) return ύ;}return Ω;}else return Γ.Ǖ(ρ,false,Ȯ,false);}String Ƿ(α Γ,String ρ)
{if(!ΑЅ(Γ,ρ)) return "";δ ύ=Γμ(Γ,false,ρ);if(ύ==δ.Ȣ) return "received!";if(ύ==δ.ǹ) return "pending...";if(ύ==δ.Ĭ) return "no answer!";return "";}String Ɗ(int ȝ,String Β,int Ș,ref int ы){String[]Η=Β.Split('\n');if(Ș>=ы+ȝ-1) ы++;ы=Math.Min(Η.Count()-1-ȝ,ы);if(Ș<ы+1) ы--;
ы=Math.Max(0,ы);String Ω="";for(int i=0;i<ȝ;i++){int Βς=i+ы;if(Βς>=Η.Count()) break;Ω+=Η[Βς]+"\n";}return Ω;}String Αώ(int Љ,int Ĕ,String Β){String[]Η=Β.Split('\n');int ǉ=Љ*Ĕ;int ŉ=(Љ+1)*Ĕ;String Ω="";for(int i=ǉ;i<ŉ;i++){if(i>=Η.Count()) break;Ω+=Η[i]+"\n";
}return Ω;}String ů(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΑЎ(String s,int Βφ){if(s=="") return s;if(s.Length>Βφ) s=s.Substring(0,Βφ-1)+".";return s;}string ƈ(String ǣ,float value,float œ,int ΒΒ,int Βξ,int ΕΩ){float ǩ=И(value,œ)*ΒΒ;
String Ĺ="[";for(int i=0;i<ΒΒ;i++){if(i<=ǩ) Ĺ+="|";else Ĺ+="'";}Ĺ+="]";return Ĺ+" "+ΑЎ(ů(ǣ),Βξ).PadRight(Βξ)+"".PadRight(ΕΩ)+ΑΨ(value);}String ΑΨ(float Ѕ){if(Ѕ>=1000000) return Math.Round(Ѕ/1000000f,Ѕ/1000000f<100?1:0)+"M";if(Ѕ>=1000) return Math.Round(Ѕ/1000f,Ѕ/1000f<100?1:0)+"K";
return ""+Math.Round(Ѕ);}String Ÿ(int ĕ){if(ĕ>=60*60) return Math.Round(ĕ/(60f*60f),1)+" h";if(ĕ>=60) return Math.Round(ĕ/60f,1)+" min";return ""+ĕ+" s";}List<IMyCameraBlock>ΖΟ=new List<IMyCameraBlock>();List<long>Ζτ=new List<long>();bool ΕΫ=false;int Δϊ=0;int ΖΞ=0;
DateTime ΖΜ=DateTime.Now;String Δψ="";void ΓΤ(){ΕΫ=false;Δϊ=0;Δψ="";}const String Δη="[PAMTFC]";const float ΖΒ=5f;const float ΕЌ=2f;class ΑЋ{public long γ;public Vector3 ι,ΗΛ;public Vector3 Ζζ;public float ΖΪ;public String Μ="";public int ȅ=-1;public bool Ζϊ;public long П=0;public int Ζί=0;
public DateTime ΖЌ;}IMyBroadcastListener ΖБ=null;long ΕΤ=0;List<ΑЋ>ΖЍ=new List<ΑЋ>();Vector3 ΗΒ=new Vector3();float ΗΓ=-1;Vector3 ΖЎ=new Vector3();bool ΖА=false;DateTime ΖЏ=DateTime.Now;const float ΖΑ=15f;const float ΕБ=10f;int ΗΔ=0;String Ь="";int ǧ=-1;bool ΖΣ=false;Vector3 ΖΡ=new Vector3();
Vector3 ΗΑ=new Vector3();List<int>Ʃ=new List<int>();String Βη="";void Γβ(){String ѝ=Я+"|"+Ю+"|"+(int) ȇ;if(ѝ==Βη) return;Βη=ѝ;ΑΟ(Я,Ю,ȇ,Ʃ);}void ΑΟ(int w,int h,К ǉ,List<int>ƛ){ƛ.Clear();if(ǉ==К.ĳ){for(int k=0;k<w*h;k++){int ȕ=k/w;ƛ.Add(ȕ*w+(ȕ%2==0?k-ȕ*w:w-1-(k-ȕ*w)));
}return;}int Βζ=(int) Math.Ceiling(w/2f),Βε=(int) Math.Ceiling(h/2f);int ΔА=w/2,ΔЏ=h/2;int x=0,y=0,Ơ=0,ơ=-1,œ=Math.Max(w,h);for(int i=0;i<œ*œ;i++){if(-Βζ<x&&x<=ΔА&&-Βε<y&&y<=ΔЏ) ƛ.Add((y-1+Βε)*w+(x-1+Βζ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Ơ;Ơ=-ơ;ơ=t;}
x+=Ơ;y+=ơ;}}bool ΓϏ(int a,int b,int w,float ţ,float š,float Ć,float Ă){return a==b||(Math.Abs(a%w-b%w)*ţ<Ć+0.5f&&Math.Abs(a/w-b/w)*š<Ă+0.5f);}bool Γα(){int n=Math.Max(0,Я*Ю);if(Ь.Length==n) return false;Ь=new String('0',n);ǧ=-1;return true;}void ΔΫ(int Ѝ,bool ψ)
{if(Ѝ<0||Ѝ>=Ь.Length) return;char[]h=Ь.ToCharArray();h[Ѝ]=ψ?'1':'0';Ь=new String(h);}const String ΓЈ="[PAMJOB]";IMyBroadcastListener ΖΥ=null;DateTime ΖΦ=DateTime.MinValue;DateTime ΖΧ=DateTime.Now;int ΖΤ=-1;bool Ζη=false;class Ǻ{public long γ;public Vector3 ι,Г,Ѐ;}List<Ǻ>Ў=new List<Ǻ>();
bool ȱ=false;Vector3 ņ,Ņ,Ň,ň;long ĉ=0;long Βώ=0;Ψ Βύ=null;bool ΔЃ=false;Ψ Γή(Vector3 ι,Vector3 Г,Vector3 Ѐ){MatrixD m=MatrixD.CreateWorld(ι,Г,Ѐ);Ψ w=new Ψ(Π);w.Ѓ=Vector3D.TransformNormal(ņ,m);w.ώ=Vector3D.TransformNormal(Ņ,m);w.Е=Vector3D.TransformNormal(Ň,m);w.Τ=ι+(Vector3) Vector3D.TransformNormal(ň,m);
w.η=true;return w;}void ΓЉ(){if(Ξ==Ν.λ||!Π.η) return;IMyShipConnector ч=Ƕ(MyShipConnectorStatus.Connected);if(ч==null||ч.OtherConnector==null) return;IMyShipConnector Б=ч.OtherConnector;Vector3I ΕΒ=new Vector3I((int) Π.О.X,(int) Π.О.Y,(int) Π.О.Z);if(ч.Position==ΕΒ){MatrixD ŏ=MatrixD.Transpose(Б.WorldMatrix);
ņ=Vector3D.TransformNormal(Ї.WorldMatrix.Forward,ŏ);Ņ=Vector3D.TransformNormal(Ї.WorldMatrix.Down,ŏ);Ň=Vector3D.TransformNormal(Ї.WorldMatrix.Left,ŏ);ň=Vector3D.TransformNormal(ч.GetPosition()-Б.GetPosition(),ŏ);ȱ=true;}if(Vector3.Distance(ч.GetPosition(),Π.Τ)<5) ĉ=Б.EntityId;List<IMyShipConnector>ǡ=new List<IMyShipConnector>();
Д.GetBlocksOfType(ǡ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));if(ǡ.Count>0){Ў.Clear();for(int i=0;i<ǡ.Count;i++){Ǻ p=new Ǻ();p.γ=ǡ[i].EntityId;p.ι=ǡ[i].GetPosition();p.Г=ǡ[i].WorldMatrix.Forward;p.Ѐ=ǡ[i].WorldMatrix.Up;Ў.Add(p);}}Βύ=null;Βώ=0;
if(Б.EntityId==ĉ||!ȱ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].γ==Б.EntityId){Βώ=Ў[i].γ;Βύ=Γή(Ў[i].ι,Ў[i].Г,Ў[i].Ѐ);}}String Γέ(){String s=(ȱ?"1":"0")+":"+ά(ņ)+":"+ά(Ņ)+":"+
ά(Ň)+":"+ά(ň)+":"+ĉ;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].γ+","+ά(Ў[i].ι)+","+ά(Ў[i].Г)+","+ά(Ў[i].Ѐ);return s;}void Γά(String s){Ў.Clear();ȱ=false;ĉ=0;String[]f=s.Split(':');if(f.Length<6) return;ȱ=f[0]=="1";ņ=β(f[1]);Ņ=β(f[2]);Ň=β(f[3]);ň=β(f[4]);long.TryParse(f[5],out ĉ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǻ p=new Ǻ();if(!long.TryParse(e[0],out p.γ)) continue;p.ι=β(e[1]);p.Г=β(e[2]);p.Ѐ=β(e[3]);Ў.Add(p);}}const String ű="[PAMDCK]";const float ΓΝ=120f;class с{public long Γ;public int ў;public long Э;public bool ΒΗ;public DateTime Ǡ,Βο;
}IMyBroadcastListener ń=null;List<с>χ=new List<с>();List<IMyShipConnector>ь=new List<IMyShipConnector>();List<IMySensorBlock>п=new List<IMySensorBlock>();List<IMyCameraBlock>Ď=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ū=new List<MyDetectedEntityInfo>();long Щ=0;int Ɯ=0;
long ΒΘ=0;DateTime ΒΙ=DateTime.Now;String ǝ="";int ΒΫ=0;IMyCubeGrid э=null;void ΔΤ(){э=Me.CubeGrid;Д.GetBlocksOfType(ь,q=>q.CubeGrid.IsSameConstructAs(э)&&q.CustomName.Contains(dockPoolTag));Д.GetBlocksOfType(п,q=>q.CubeGrid.IsSameConstructAs(э)&&q.CustomName.Contains(zoneTag));
Д.GetBlocksOfType(Ď,q=>q.CubeGrid.IsSameConstructAs(э)&&q.CustomName.Contains(laneTag));for(int i=0;i<п.Count;i++){п[i].DetectSmallShips=true;п[i].DetectLargeShips=true;}for(int i=0;i<Ď.Count;i++) Ď[i].EnableRaycast=true;}IMyShipConnector ΑЉ(long γ){if(γ==0) return null;
IMyShipConnector c=Д.GetBlockWithId(γ) as IMyShipConnector;return c!=null&&э!=null&&c.CubeGrid.IsSameConstructAs(э)?c:null;}bool ž(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool Αο(IMyShipConnector c,long Γ){for(int i=0;i<Ď.Count;i++)
{IMyCameraBlock ȭ=Ď[i];if(!ȭ.IsWorking||Vector3D.Distance(ȭ.GetPosition(),c.GetPosition())>10) continue;Vector3D ŉ=ȭ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ȭ.CanScan(ŉ)) continue;MyDetectedEntityInfo Ŋ=ȭ.Raycast(ŉ);if(!Ŋ.IsEmpty()&&Ŋ.EntityId!=э.EntityId&&Ŋ.EntityId!=Γ&&Ŋ.Type!=MyDetectedEntityType.FloatingObject)
{ǝ="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΓІ(){for(int s=0;s<п.Count;s++){if(!п[s].IsWorking) continue;Ū.Clear();п[s].DetectedEntities(Ū);for(int i=0;i<Ū.Count;i++){MyDetectedEntityInfo e=Ū[i];bool ΔЌ=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(ΔЌ&&e.EntityId!=Щ&&e.Velocity.Length()>0.5f){ǝ="Ship moving in dock area: "+e.Name;return true;}}}return false;}long ΔΘ(с r){IMyShipConnector Э=ΑЉ(r.Э);if(ž(Э)&&Αο(Э,r.Γ)) return Э.EntityId;if(!r.ΒΗ||Э==null) return 0;IMyShipConnector ĺ=null;double ΒΕ=0;for(int i=0;i<ь.Count;i++)
{IMyShipConnector c=ь[i];if(!ž(c)) continue;double d=Vector3D.Distance(c.GetPosition(),Э.GetPosition());if(ĺ==null||d<ΒΕ){if(Αο(c,r.Γ)){ĺ=c;ΒΕ=d;}}}return ĺ==null?0:ĺ.EntityId;}void Γϋ(){с ȏ=null;long ч=0;for(int ś=2;ś>=1&&ȏ==null;ś--){с Ǩ=null;for(int i=0;i<χ.Count;i++)
{с r=χ[i];if(r.ў!=ś) continue;if(ś==1){long c=ΔΘ(r);if(c==0) continue;if(Ǩ==null||r.Ǡ<Ǩ.Ǡ){Ǩ=r;ч=c;}}else if(Ǩ==null||r.Ǡ<Ǩ.Ǡ) Ǩ=r;}ȏ=Ǩ;}if(ȏ==null){if(χ.Count>0&&ǝ=="") ǝ="No free dock";return;}Щ=ȏ.Γ;Ɯ=ȏ.ў;ΒΘ=ȏ.ў==1?ч:0;ΒΙ=DateTime.Now;
}void Δλ(){if(э==null) return;if(ń==null) ń=IGC.RegisterBroadcastListener(ű);int ƙ=30;while(ń.HasPendingMessage&&ƙ-->0){String Ο=ń.AcceptMessage().Data as String;if(Ο!=null) Γό(Ο);}ΔΠ();if(++ΒΫ<3) return;ΒΫ=0;IGC.SendBroadcastMessage(ű,ΓΜ());}void Γό(String Ο){try{String[]f=Ο.Split(';');
long Γ=long.Parse(f[1]);if(f[0]=="D"){for(int i=χ.Count-1;i>=0;i--) if(χ[i].Γ==Γ) χ.RemoveAt(i);if(Щ==Γ) Щ=0;}else if(f[0]=="R"&&f.Length>=5){long Э=long.Parse(f[3]);if(ΑЉ(Э)==null) return;с r=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Γ) r=χ[i];if(r==null){r=new с();r.Γ=Γ;r.Ǡ=DateTime.Now;χ.Add(r);}
int ў=int.Parse(f[2]);if(r.ў!=ў) r.Ǡ=DateTime.Now;r.ў=ў;r.Э=Э;r.ΒΗ=f[4]=="1";r.Βο=DateTime.Now;}}catch{}}void ΔΠ(){for(int i=χ.Count-1;i>=0;i--)
if((DateTime.Now-χ[i].Βο).TotalSeconds>3) χ.RemoveAt(i);с ļ=null;for(int i=0;i<χ.Count;i++) if(χ[i].Γ==Щ) ļ=χ[i];if(Щ!=0&&ļ==null) Щ=0;if(ļ!=null&&(DateTime.Now-ΒΙ).TotalSeconds>ΓΝ){Щ=0;ļ.Ǡ=DateTime.Now;}ǝ="";if(Щ==0&&χ.Count>0&&!ΓІ()) Γϋ();}String ΓΜ()
{String θ="S;"+э.EntityId+";"+Щ+";"+Ɯ+";"+ΒΘ+";";for(int i=0;i<χ.Count;i++) θ+=(i>0?",":"")+χ[i].Γ;θ+=";";for(int i=0;i<ь.Count;i++){IMyShipConnector c=ь[i];θ+=(i>0?":":"")+c.EntityId+","+ά(c.GetPosition())+","+ά(c.WorldMatrix.Forward)+","+ά(c.WorldMatrix.Up);
}θ+=";";for(int i=0;i<п.Count;i++){IMySensorBlock z=п[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();θ+=(i>0?":":"")+ά(z.GetPosition())+","+Math.Round(r,1);}return θ;}String Γο()
{if(ь.Count==0&&п.Count==0) return "";int Βΰ=0;for(int i=0;i<ь.Count;i++) if(ž(ь[i])) Βΰ++;return "Docks: "+Βΰ+"/"+ь.Count+" free"+(Щ!=0?(Ɯ==1?" | docking":" | undocking"):"")+
(χ.Count>1?" | "+(χ.Count-1)+" waiting":"")+(ǝ!=""?"\n"+ǝ:"")+"\n";}IMyBroadcastListener ΖЂ=null;int ΒΪ=0;DateTime Ζα=DateTime.Now;DateTime Ζώ=DateTime.MinValue;long ΖϏ=0;int ΖЀ=0;long ΖЁ=0;bool Εό=false;int Ζβ=0;List<Vector3>ΗΞ=new List<Vector3>();List<float>ΗΟ=new List<float>();
bool Ζΰ=false;bool Ζή=false;Vector3 Ζέ=new Vector3();void ΔΛ(){if(ΒΪ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ű,"D;"+ΕΤ);ΒΪ=0;Εό=false;}const float ΓЇ=600f;class у{public long γ;public int Ѝ=-1;public bool ΓΒ;public float Ć,Ă;public DateTime ΒЅ;}class е{public String ѝ;public int w,h;
public float ţ,š;public String ψ;public List<int>ƺ=new List<int>();public List<у>Ћ=new List<у>();}List<е>а=new List<е>();IMyBroadcastListener Œ=null;int Βν=0;void Δμ(){if(Œ==null) Œ=IGC.RegisterBroadcastListener(Δη);int ƙ=50;while(Œ.HasPendingMessage&&ƙ-->0){String Ο=Œ.AcceptMessage().Data as String;
if(Ο!=null) try{Γώ(Ο);}catch{}}for(int j=а.Count-1;j>=0;j--){е Μ=а[j];for(int i=Μ.Ћ.Count-1;i>=0;i--)
if((DateTime.Now-Μ.Ћ[i].ΒЅ).TotalSeconds>ΓЇ) Μ.Ћ.RemoveAt(i);if(Μ.Ћ.Count==0){а.RemoveAt(j);continue;}ΓΗ(Μ);}if(++Βν<3) return;Βν=0;for(int j=0;j<а.Count;j++) IGC.SendBroadcastMessage(ΓЈ,ΓΙ(а[j]));}void ΓΗ(е Μ){for(int i=0;i<Μ.Ћ.Count;i++){у s=Μ.Ћ[i];if(s.ΓΒ&&(s.Ѝ<0||Μ.ψ[s.Ѝ]=='1')) s.Ѝ=ΓЌ(Μ,s);
}}String ΓΙ(е Μ){String θ="J;"+Μ.ѝ+";"+Μ.ψ+";";for(int i=0;i<Μ.Ћ.Count;i++) θ+=(i>0?",":"")+Μ.Ћ[i].γ+":"+Μ.Ћ[i].Ѝ;return θ;}void Γώ(String Ο){String[]f=Ο.Split(';');if(f.Length<11||f[4]=="") return;String[]k=f[4].Split('|');if(k.Length<8) return;е Μ=null;
for(int i=0;i<а.Count;i++) if(а[i].ѝ==f[4]) Μ=а[i];if(Μ==null){Μ=new е();Μ.ѝ=f[4];Μ.w=int.Parse(k[3]);Μ.h=int.Parse(k[4]);Μ.ţ=float.Parse(k[6]);Μ.š=float.Parse(k[7]);if(Μ.w<1||Μ.h<1) return;ΑΟ(Μ.w,Μ.h,(К) int.Parse(k[5]),Μ.ƺ);Μ.ψ=new String('0',Μ.w*Μ.h);а.Add(Μ);
}String d=f[6];if(d.Length==Μ.ψ.Length){char[]c=Μ.ψ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Μ.ψ=new String(c);}long γ=long.Parse(f[0]);у s=null;for(int i=0;i<Μ.Ћ.Count;i++) if(Μ.Ћ[i].γ==γ) s=Μ.Ћ[i];if(s==null){s=new у();s.γ=γ;Μ.Ћ.Add(s);}
s.ΒЅ=DateTime.Now;String[]w=f[10].Split(',');s.ΓΒ=w[0]=="1";if(w.Length>=3){s.Ć=float.Parse(w[1]);s.Ă=float.Parse(w[2]);}int ȅ=int.Parse(f[5]);if(ȅ>=0&&ȅ<Μ.ψ.Length&&(s.Ѝ<0||Μ.ψ[s.Ѝ]=='1')&&Μ.ψ[ȅ]!='1'&&Αθ(Μ,ȅ,s)==null)
s.Ѝ=ȅ;}у Αθ(е Μ,int Ѝ,у ƣ){for(int i=0;i<Μ.Ћ.Count;i++){у o=Μ.Ћ[i];if(o!=ƣ&&o.Ѝ>=0&&ΓϏ(Ѝ,o.Ѝ,Μ.w,Μ.ţ,Μ.š,Math.Max(o.Ć,ƣ.Ć),Math.Max(o.Ă,ƣ.Ă)))
return o;}return null;}int ΓЌ(е Μ,у s){for(int k=0;k<Μ.ƺ.Count;k++){int c=Μ.ƺ[k];if(Μ.ψ[c]!='1'&&Αθ(Μ,c,s)==null) return c;}return-2;}String Γυ(){String s="";for(int j=0;j<а.Count;j++){е Μ=а[j];int ψ=0;for(int i=0;i<Μ.ψ.Length;i++) if(Μ.ψ[i]=='1') ψ++;s+="Job "+(j+1)+": "+ψ+"/"+Μ.ψ.Length+" holes, "+Μ.Ћ.Count+" ships\n";
}return s;}void Δί(string Ќ){Echo("[PAM] This is the controller script.\nPaste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB's name.");}