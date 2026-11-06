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
public enum Ζ{ǅ,Ŭ,џ,р,Ϊ}public enum Α{а,ȣ,ŭ,ȋ,ď,І,Я,
Ȣ,Į,Ǡ,Č,Ĉ,Ȟ,
Ů,ǻ,ǎ,ċ,Ǉ,Ư,
Βψ,ǆ,Ť,ư,Ȧ}public enum Μ{з,İ,Ѕ,ΑБ,ƫ,Ʒ,Ƹ,Ī,ī}public enum п{None,ċ,а,ΔΩ}public enum Ω{Main,Δψ,ū,Ū,Βτ,Ʃ,
ΑЁ,Δι,Δκ,Δθ,ΗΟ,ΙΒ,
ΘБ,ΙΑ,Βο,Δό,ǀ}public enum в{ƾ,Ţ,ΗΡ,Δμ,Ϊ}public enum Ǥ{ǂ,Ζζ}public enum е{ƽ,ΒΖ,Δώ,Ŧ}public enum Ѐ{Βά,ΔΦ,ΑЌ,ΒΑ,ƭ,ΒΤ,Ƴ}public enum ь{ΑЎ,ΑІ,ΑЇ}public enum И{ŧ,ΖЃ,ĉ}public enum ι{Βή,Ββ,Βγ,Δυ,Δτ,
Βέ,ƻ,ƺ,Βΰ,
Βα,Βί}public enum ή{Ǵ,Ȋ,ǳ,ƨ,ΑЄ,ΑЅ}public enum Ĳ{Δή,ΔΜ,ΘЈ}public enum с{ǁ,Δά,ΖЎ,ΒΫ}public enum ǵ{None,Βη,ΗΑ,ΖЍ}public enum σ{ų,Ƨ,Ƶ,Ŧ,ΒΩ,ƶ,ΒΪ,ΗΧ,Βδ}public enum ΘЋ{ǅ,ΘЍ,ΘЌ,ΘЏ,ΘЉ,ΘЇ,ΘЎ}class Χ{public bool γ=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 ψ=new Vector3();public Vector3 μ=new Vector3();public Vector3 Ч=new Vector3();public float Ў=0;public float Ιβ=0;public float[]ǝ=null;public Χ(){}public Χ(Χ δ){γ=δ.γ;Δ=δ.Δ;α=δ.α;β=δ.β;ψ=δ.ψ;μ=δ.μ;
Ч=δ.Ч;ǝ=δ.ǝ;}public Χ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 ψ,Vector3 μ){this.Δ=Δ;this.α=α;this.β=β;this.ψ=ψ;this.Ў=0;this.μ=μ;}public void Ζγ(List<IMyThrust>Ǌ,List<string>ȅ){ǝ=new float[ȅ.Count];for(int i=0;i<ǝ.Length;i++)
ǝ[i]=-1;for(int i=0;i<Ǌ.Count;i++){string ы=Δζ(Ǌ[i]);int Β=ȅ.IndexOf(ы);if(Β!=-1)
ǝ[Β]=Џ(Ǌ[i].MaxEffectiveThrust,Ǌ[i].MaxThrust);}}}class Βζ{public Χ Љ=null;public List<Vector3>Ś=new List<Vector3>();public float ž=0;public float ΔΓ=0;public float ķ=0;public float ΓΛ=0;public Vector3 ΑΟ=new Vector3();}class ĳ{public ι Ћ=ι.Βή;public float ǒ=0;public float Ȭ=0;
public string Ǔ="";public string ǖ="";DateTime Ετ;public bool Γψ=false;private bool ΒЏ=false;public bool ΑЉ(bool Ȅ){if(Ȅ){Ȭ=0;ΒЏ=false;return false;}ΒЏ=true;return Ȭ>ǒ;}public void ΗΒ(){ΑЉ(true);Γψ=false;}public void Ηβ(){if(ΒЏ)
if((DateTime.Now-Ετ).TotalSeconds>1){Ȭ++;Ετ=DateTime.Now;}}public bool ΒΠ(){switch(Ћ){case ι.Ββ:return true;case ι.Βγ:return true;}return false;}}class э{public String φ="";public String ρ="";public int Ǐ=0;public И ŉ=И.ĉ;public э(String φ,String ρ,int Ǐ,И ŉ){this.φ=φ;this.ρ=ρ;
this.Ǐ=Ǐ;this.ŉ=ŉ;}}class Ζς{public Ζς(Vector3 θ,float ύ){this.θ=θ;this.ύ=ύ;}public Vector3 θ;public float ύ;}class Βξ{public DateTime ΗА;public DateTime ΕΖ;public String ν="";public String φ="";public String Ιδ="";public String Λ="";public String Ιή="";public String ΙΪ="";public String Γφ="";
public Vector3 Δ=new Vector3();public List<э>Ы=new List<э>();public Ζ Η=Ζ.ǅ;public ǵ Ȩ;public float Ō;public float ļ;public int і;public int ΙΠ=0;public int Ια=0;public bool ΘЊ(){return(DateTime.Now-ΗА).TotalSeconds>10;}public bool ΖІ(){if(Ȩ!=ǵ.Βη) return false;return(DateTime.Now-ΕΖ).TotalSeconds>=4;
}public Βξ(String ν){this.ν=ν;}public ǵ ΘА(String ė,bool Θα,bool Ǒ,bool Ηπ){if(ė==""&&!Ǒ) return ǵ.None;if(Ȩ==ǵ.Βη&&ΖІ())
Ȩ=ǵ.ΖЍ;if(Θα){Γφ=ė;Ȩ=ǵ.Βη;ΕΖ=DateTime.Now;return Ȩ;}else if(Ǒ){Γφ="";Ȩ=ǵ.None;}else if(Γφ==ė){if(Ηπ) Ȩ=ǵ.ΗΑ;return Ȩ;}return ǵ.None;}}IMyGridTerminalSystem ϊ;Vector3 Αξ=new Vector3();Vector3 ΰ=new Vector3();Vector3 ΑΣ=new Vector3();Vector3 Ǝ=new Vector3();Vector3 Ƃ=new Vector3();
DateTime Εσ=new DateTime();bool ƅ=true;int ΓЋ=0;bool Ā=false;bool ΑΫ=false;bool Ƒ=true;bool Ζί=false;bool Ņ=false;bool ΑΨ=false;float ș=0;float Ĥ=0;int ă=0;int Ơ=0;int Αρ=0;int ΒЇ=0;float ŋ=0;float ĸ=0;double Γμ=0;float ŷ=0;List<int>ł=new List<int>();List<int>ŕ=new List<int>();
Ζ Η=Ζ.ǅ;int ώ=0;float Ǳ=0;float Ǟ=0;float ζ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>ΓЎ=new List<IMyTimerBlock>();List<IMyShipConnector>т=new List<IMyShipConnector>();List<IMyThrust>Ȇ=new List<IMyThrust>();List<IMyGyro>ΑΤ=new List<IMyGyro>();List<IMyTerminalBlock>ς=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȃ=new List<IMyLandingGear>();List<IMyReactor>Ă=new List<IMyReactor>();List<IMyConveyorSorter>Ατ=new List<IMyConveyorSorter>();List<IMyGasTank>ѓ=new List<IMyGasTank>();List<IMyTerminalBlock>Ŵ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ļ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ĕ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȧ=new List<IMyBatteryBlock>();List<IMyTextPanel>є=new List<IMyTextPanel>();List<IMyTextSurface>Ą=new List<IMyTextSurface>();List<IMyTextPanel>Ž=new List<IMyTextPanel>();IMyTerminalBlock ј=null;Χ Θ=new Χ();Χ ΓΩ=new Χ();
Χ Γά=new Χ();bool ȇ=false;bool Ŝ=false;bool ŝ=false;bool ΑΪ=false;List<String>ȅ=new List<string>();List<Χ>Σ=new List<Χ>();List<Χ>ƥ=new List<Χ>();int ΓΔ=0;double Ζΰ=0;Χ Ε=new Χ();Vector3 Ǚ;Vector3 Ȗ;Μ ί=Μ.з;Ǥ Ğ=Ǥ.ǂ;int τ=0;int Н=0;double Ƌ=0;bool ΑΖ=false;ĳ ϋ=new ĳ();
ĳ ό=new ĳ();е л=е.ƽ;bool Ć=true;Ǥ Ț=Ǥ.ǂ;int й=3;int и=3;int Ц=30;ь у=ь.ΑЎ;bool ȝ=true;bool ń=true;bool ľ=true;bool ĥ=false;Ѐ М=Ѐ.Βά;bool Ȝ=true;float Ǭ=90;float ц=20;float Ь=5;float ч=20;float ǥ=0.70f;float Š=1.50f;float ş=2.50f;float ĩ=0f;float ȯ=0f;Α Υ;
Μ х;Α ƚ;Βζ ε=null;int Щ=0;int Ъ=0;int К=0;int Ȕ=0;int Ģ=30;int У=0;bool Ё=true;Vector3 ш;double Л=0;bool Απ=false;int ΔΘ=0;int Ђ=0;int ΒϏ=0;int ǭ=0;int ΕІ=0;Vector3 Ɠ=new Vector3();float к=0;float Ю=0;int Ĩ=0;Χ Αυ=null;float Γί=0;float ǌ=0;float С=0;float Ġ=0;
bool ΓΪ=false;bool Ħ=false;bool ơ=false;bool ȑ=false;bool ȍ=false;DateTime Ηώ=new DateTime();Χ Ę=null;Χ ΕΙ=null;float Ǽ=0;п Ǧ;float Ȓ=0;float ħ=0;String Γη="";float Αέ=0;float ļ=0;float Ō=0;List<э>Ǖ=new List<э>();float О=0;bool Ƙ=false;bool ΖΥ=false;bool ΖΒ=false;
bool Αψ=false;bool ΖΓ=false;float Γκ=0;float Γυ=0;Vector3 Εή=new Vector3();Vector3 ω=new Vector3();Vector3 Αω=new Vector3(1,1,1);float ΓЊ=1;Vector3 Ɲ=new Vector3();bool ΑΩ=false;bool ΓΡ=false;bool Γδ=false;bool ΔЇ=false;float ΑΔ=0;float ǉ=2;Vector3 ΖΤ;Vector3 ΓЉ;Vector3 ΓЈ;
float[,]ǟ=new float[3,2];Dictionary<String,float[,]>ћ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζέ=new Dictionary<String,float>();int ΒЂ=0;Dictionary<String,float[]>ȗ=new Dictionary<string,float[]>();float Γω;bool ΑΗ=false;int ǋ=0;int ȱ=0;int ΑΦ=0;int Ĝ=0;
String Λ="";int[]ΕϏ=new int[Enum.GetValues(typeof(Ω)).Length];bool ΕЀ=false;int Αη=0;int і=0;Ω Ќ=Ω.Main;int Θξ=0,Θο=0,Θπ=0,Θρ=0;int Θέ=0,Θή=0;bool ĺ=false;bool ΔЈ=false;String ΓЀ="";String ΒІ="";IMyBroadcastListener Ȏ=null;bool Εέ=true;List<Βξ>ΙΘ=null;Βξ ΙΫ=null;
int ΙΙ=0,ΙΚ=0;int ΙΛ=0,ΙΜ=0;int ΙΝ=0,ΙΞ=0,ΙΟ=0;String ΗЏ="";Exception ȓ=null;bool Αο=false;Μ ŗ=Μ.з;bool ΕΜ=false;bool Ες=true;bool Γώ=false;String Ζή="";bool Ɵ=false;bool ΓЅ=true;int ΑΧ=0;int[]Ιέ=null;Vector3 ΒЉ=new Vector3();bool Εγ=false;bool Ǎ(IMyTerminalBlock Τ)=>Τ.CubeGrid==Me.CubeGrid;
Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΒΡ(Me,controllerTag,true))
Η=Ζ.р;ο(Ω.Main);if(Η!=Ζ.р){Λ="Welcome to [PAM]!";с Γύ=ΗΓ();if(Η==Ζ.ǅ){List<IMyShipDrill>ΕΩ=new List<IMyShipDrill>();List<IMyShipGrinder>Εδ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(ΕΩ,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(Εδ,q=>q.CubeGrid==Me.CubeGrid);if(ΕΩ.Count>0)
{Η=Ζ.Ŭ;Λ="Miner mode enabled!";}else if(Εδ.Count>0){Η=Ζ.џ;Λ="Grinder mode enabled!";}else{Η=Ζ.Ϊ;ǹ(в.Ϊ);}}if(Γύ==с.ǁ) Ƒ=false;if(Γύ==с.Δά) Λ="Data restore failed!";if(Γύ==с.ΒΫ) Λ="New version, wipe data";}}void Main(string ѐ,UpdateType Θω){try{if(ȓ!=null){if(ѐ.Trim().ToUpper()=="RETRY")
{ȓ=null;ƅ=true;ă=0;if(Η!=Ζ.р) χ();Λ="Restarted after error";}else{ΖϏ();Δφ();return;}}ΑΫ=(Θω&UpdateType.Update10)!=0;if(ΑΫ) ΓЋ++;Ā=ΓЋ>=10;if(Ā) ΓЋ=0;if(ΑΫ){ǋ++;if(ǋ>4) ǋ=0;Αρ=(int) Math.Max(0,10-(DateTime.Now-Εσ).TotalSeconds);}if(ѐ!="") ΗЏ=ѐ;if(Η!=Ζ.р)
ΗΠ(ѐ);else
Ζπ(ѐ);ΕЀ=false;try{int ΕΚ=Runtime.CurrentInstructionCount;float Ɣ=Џ(ΕΚ,Runtime.MaxInstructionCount);if(Ɣ>0.90) Λ="Max. instructions >90%";if(Ɣ>ŋ) ŋ=Ɣ;if(ΑΗ){ł.Add(ΕΚ);while(ł.Count>10) ł.RemoveAt(0);ĸ=0;for(int i=0;i<ł.Count;i++) ĸ+=ł[i];ĸ=Џ(Џ(ĸ,ł.Count),Runtime.MaxInstructionCount);
double Γΰ=Runtime.LastRunTimeMs;if(ΑΨ&&Γΰ>Γμ) Γμ=Γΰ;ŕ.Add((int)(Γΰ*1000f));while(ŕ.Count>10) ŕ.RemoveAt(0);ŷ=0;for(int i=0;i<ŕ.Count;i++) ŷ+=ŕ[i];ŷ=Џ(ŷ,ŕ.Count)/1000f;}}catch{ŋ=0;}}catch(Exception e){ȓ=e;}}void ΗΠ(string ѐ){bool Ας=false;String ΕΓ="";if(ώ<=1&&!Δπ(ѐ))
Δχ(ѐ);int ΘΙ=50;while(Ȏ!=null&&Ȏ.HasPendingMessage&&ΘΙ-->0){MyIGCMessage π=Ȏ.AcceptMessage();String ά=π.Data as String;if(ά==null) continue;String Ɨ="";if(ώ<=1&&ΖЏ(ref ά,out Ɨ,out ΕΓ)&&Ɨ==BROADCAST_SELF_ID){Δχ(ά);Ας=true;break;}}bool Εξ=ΑΨ&&Υ==Α.а&&!Ņ&&!Ας&&ă==0&&!ΑΪ;
if(Ā&&Υ!=Α.а) Ας=true;if((ΑΫ&&!Εξ)||(Ā&&Εξ)){if(ă==0&&(Αρ<=0||ƅ)){Ζί=ώ>0;ώ=0;ă=1;Ǻ();ΗΖ();ΖЄ();Ƕ("Scan 1");}else if(ă==1){ă=2;Ǻ();ΗΗ();Ƕ("Scan 2");}else if(ă==2){ă=0;Ǻ();ΗΘ();ΗΙ();if(ώ<=1) Δρ();Ƕ("Scan 3");Εσ=DateTime.Now;if(ώ<=1&&Ƒ)
Αξ=ΒΚ(Φ,Φ.CenterOfMass);Ƒ=false;if(ƅ){ƅ=false;bool Θύ=ί==Μ.Ѕ;χ();if(Ες&&resumeAfterRestart&&Θύ&&ώ<=1) Γώ=true;}Ες=false;if(Ζί&&ώ==0) Λ="Setup complete";}else{if(ί==Μ.Ѕ&&Η!=Ζ.Ϊ){Ǻ();Ζα();Ƕ("Inv balance");}Ǻ();switch(Ơ){case 0:ΗΪ();break;case 1:Ηή();break;case 2:ΗΩ();break;
case 3:Ηε();break;case 4:Ηέ();break;case 5:Ζη();break;case 6:Ζε(Φ);break;}Ƕ("Update: "+Ơ);Ơ++;if(Ơ>6){Ơ=0;ΑΨ=true;if(ŗ!=Μ.з){switch(ŗ){case Μ.Ī:Ƞ();break;case Μ.ī:Ƞ();break;case Μ.Ѕ:Ƞ();break;case Μ.Ʒ:Ʊ();break;case Μ.Ƹ:č();break;}ŗ=Μ.з;}}}if(!ƅ){if(!Ǹ(Φ,true)){Φ=null;
Ƒ=true;ώ=2;}if(ώ>=2&&Υ!=Α.а)
χ();if(ώ<=1){Ĥ=Φ.CalculateShipMass().PhysicalMass;ș=(float) Φ.GetShipSpeed();ΰ=ΗΨ(Φ,Αξ);ΑΣ=Φ.WorldMatrix.Forward;Ǝ=Φ.WorldMatrix.Left;Ƃ=Φ.WorldMatrix.Down;Ηα();Ηγ();ΗΫ();if(Γώ){Γώ=false;Ƞ();if(Υ!=Α.а) Λ="Job resumed after restart";}if(Υ!=Α.а){Ņ=false;Δϋ(false);ΔΠ(false);String Θη=ΒΜ(Υ)+" "+(int) Υ;
Ǻ();Ηί();Βχ(false);Ƕ(Θη);Ηδ();Ηΰ();Ǻ();Ηά();Ƕ("Thruster");Ǻ();ΔЃ();Ƕ("Gyroscope");}else{if(Ņ){if(ƴ()){Х(Ƃ,ΑΣ,Ǝ,0.25f,true);ΔЃ();Λ="Aligning to planet: "+Math.Round(ΑΔ-0.25f,2)+"°";if(ΑΩ) ΒΟ(true,true);}else ΒΟ(true,true);}}Ɵ=false;}}}Ǻ();Δφ();Ƕ("Print");if(Ας||ΒЇ<=0){Ǻ();Ζβ(ΕΓ);
Ƕ("Broadcast");ΒЇ=4;}else if(Ā) ΒЇ--;}void Δχ(string ѐ){if(ѐ=="") return;var Д=ѐ.ToUpper().Split(' ');Д.DefaultIfEmpty("");var ė=Д.ElementAtOrDefault(0);var ĕ=Д.ElementAtOrDefault(1);var Ŷ=Д.ElementAtOrDefault(2);var ΔЌ=Д.ElementAtOrDefault(3);String Ȳ="Invalid argument: "+ѐ;bool ΔΒ=false;
switch(ė){case "UP":this.Δσ(false);break;case "DOWN":this.Δς(false);break;case "UPLOOP":this.Δσ(true);break;case "DOWNLOOP":this.Δς(true);break;case "APPLY":this.ΔΞ(true);break;case "MRES":і=0;break;case "STOP":this.χ();break;case "PATHHOME":{this.χ();this.Βσ();}break;case "PATH":{this.χ();this.Βσ();Θ.γ=true;}break;
case "START":{this.χ();Βς();}break;case "ALIGN":{ΒΟ(!Ņ,false);}break;case "CONT":{this.χ();this.Ƞ();}break;case "JOBPOS":{this.χ();this.č();}break;case "HOMEPOS":{this.χ();this.Ʊ();}break;case "FULL":{Ƙ=true;}break;case "RESET":{Αο=true;ώ=2;}break;case "EXPORT":{Ζχ();}break;
case "IMPORT":{ΖЁ(ĕ=="JOB");}break;default:ΔΒ=true;break;}if(Η!=Ζ.Ϊ){switch(ė){case "SHUTTLE":{Ζυ();}break;case "CFGS":{if(!Ζξ(ĕ,Ŷ,ΔЌ)) Λ=Ȳ;}break;case "CFGB":{if(!Ζν(ĕ,Ŷ)) Λ=Ȳ;}break;case "CFGL":{if(!Ď(ref Ǭ,true,σ.Ƶ,ĕ,"")||!Ζο(Ŷ))
Λ=Ȳ;}break;case "CFGE":{if(!Ď(ref Ь,true,σ.ƶ,ĕ,"IG")||
!Ď(ref ц,true,σ.ΒΩ,Ŷ,"IG")||
!Ď(ref ч,true,σ.ΒΪ,ΔЌ,"IG"))
Λ=Ȳ;}break;case "CFGA":{if(!Ď(ref ǥ,false,σ.Ƨ,ĕ,"")) Λ=Ȳ;}break;case "CFGW":{if(!Ď(ref Š,false,σ.ų,ĕ,"")||
!Ď(ref ş,false,σ.ų,Ŷ,""))
Λ=Ȳ;}break;case "NEXT":{ΑЀ(false);}break;case "PREV":{ΑЀ(true);}break;default:if(ΔΒ) Λ=Ȳ;break;}}else{switch(ė){case "UNDOCK":{Ɵ=true;}break;default:if(ΔΒ) Λ=Ȳ;break;}}}String Ζϋ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Ζυ(){χ();Η=Ζ.Ϊ;ǹ(в.Ϊ);Ε.γ=false;Θ.γ=false;ξ=null;ς.Clear();ί=Μ.з;}void ΒΟ(bool Ї,bool ђ){if(!Ї) Λ="Aligning canceled";if(ђ) Λ="Aligning done";if(ђ||!Ї){Ņ=false;ΔϏ();ƿ(false,0,0,0,0);return;}if(ƴ()) Ņ=true;}bool Ζξ(String ΘϏ,String ΗЇ,String Ηω)
{bool Θϋ=ί==Μ.Ѕ;int w,h,d;if(int.TryParse(ΘϏ,out w)&&int.TryParse(ΗЇ,out h)&&int.TryParse(Ηω,out d)){this.χ();й=w;и=h;Ц=d;Ē(false);Ų(false,false);if(Θϋ) Ƞ();return true;}return false;}bool Ζο(String ΔЋ){if(ΔЋ=="ON"){ȝ=true;return true;}if(ΔЋ=="OFF"){ȝ=false;return true;}
return false;}bool Ζν(String ΕΧ,String ΑΕ){bool ƒ=true;if(ΕΧ=="HOME") Ć=true;else if(ΕΧ=="STOP") Ć=false;else ƒ=false;if(ΑΕ=="HOME") л=е.ƽ;else if(ΑΕ=="STOP") л=е.Δώ;else if(ΑΕ=="JOB") л=е.ΒΖ;else if(ΑΕ=="IG") л=е.Ŧ;else ƒ=false;return ƒ;}void ο(Ω Γσ){ΕϏ[(int) Ќ]=і;
і=ΕϏ[(int) Γσ];if(Γσ==Ω.ΗΟ) і=0;Ќ=Γσ;if(Η!=Ζ.р)
đ(Ќ==Ω.ū,false,0,0);ΕЀ=true;}void Δσ(bool Γε){if(і>0) і--;else if(Γε) і=Αη-1;}void Δς(bool Γε){if(і<Αη-1) і++;else if(Γε) і=0;}String ǹ(в π){switch(π){case в.ƾ:Λ="Job is running";break;case в.Ţ:Λ="Connector not ready!";break;case в.ΗΡ:Λ="Ship modified, path outdated!";break;
case в.Δμ:Λ="Interrupted by player!";break;case в.Ϊ:Λ="Shuttle mode enabled!";break;}return "";}String Ζώ(Ǥ θ){switch(θ){case Ǥ.ǂ:return "Top-Left";case Ǥ.Ζζ:return "Center";default:return "";}}String Ζω(ь Ɛ){switch(Ɛ){case ь.ΑІ:return "Auto"+(Η==Ζ.Ŭ?" (Ore)":"");case ь.ΑЇ:return "Auto (+Stone)";
case ь.ΑЎ:return "Default";default:return "";}}String ΒΙ(Μ Ф){switch(Ф){case Μ.з:return "No job";case Μ.İ:return "Job paused";case Μ.Ѕ:return "Job active";case Μ.Ī:return "Job active";case Μ.ī:return "Job active";case Μ.ΑБ:return "Job done";case Μ.ƫ:return "Job changed";case Μ.Ʒ:return "Move home";
case Μ.Ƹ:return "Move to job";}return "";}String Δα(е Ηο){switch(Ηο){case е.ƽ:return "Return home";case е.ΒΖ:return "Fly to job pos";case е.Δώ:return "Stop";case е.Ŧ:return "Ignore";}return "";}String Ζϊ(Ѐ Ɛ){switch(Ɛ){case Ѐ.Βά:return "Off";case Ѐ.ΒΑ:return "Drop pos (Stone) ";case Ѐ.ƭ:return "Drop pos (Sto.+Ice)";
case Ѐ.ΔΦ:return "Cur. pos (Stone)";case Ѐ.ΑЌ:return "Cur. pos (Sto.+Ice)";case Ѐ.ΒΤ:return "In motion (Stone)";case Ѐ.Ƴ:return "In motion (Sto.+Ice)";}return "";}String Δί(п Ф){switch(Ф){case п.None:return "No batteries";case п.ċ:return "Charging";case п.ΔΩ:return "Discharging";}return "";}String ΒΜ(Α Ф)
{String ΙΧ=Η==Ζ.Ϊ?"target":"job";switch(Ф){case Α.а:return "Idle";case Α.ȣ:return "Flying to XY position";case Α.ŭ:return Η==Ζ.џ?"Grinding":"Mining";case Α.ȋ:return "Returning";case Α.Ť:return "Flying to drop pos";case Α.ď:return "Returning to dock";case Α.І:return "Flying to dock area";
case Α.Я:return "Flying to job area";case Α.Ȣ:return "Flying to path";case Α.Į:return "Flying to job position";case Α.Ǡ:return "Approaching dock";case Α.Č:return "Docking";case Α.Ĉ:return "Aligning";case Α.Ȟ:return "Aligning";case Α.Ů:return "Retry docking";case Α.ǻ:return "Unloading";case Α.Ȧ:return Ζή;
case Α.ǎ:return "Undocking";case Α.ċ:return "Charging batteries";case Α.Ǉ:return "Waiting for uranium";case Α.Ư:return "Filling up hydrogen";case Α.Βψ:return "Waiting for ejection";case Α.ǆ:return "Waiting for ejection";case Α.ư:return "Flying to drop pos";}return "";}String Δη(ι Ћ){switch(Ћ)
{case ι.Βή:return "On \"Undock\" command";case ι.Ββ:return "On player entered cockpit";case ι.Δυ:return "On ship is full";case ι.Δτ:return "On ship is empty";case ι.Βγ:return "On time delay";case ι.ƻ:return "On batteries empty(<25%)";case ι.ƺ:return "On batteries empty(=0%)";case ι.Βέ:return "On batteries full";
case ι.Βα:return "On hydrogen empty(<25%)";case ι.Βί:return "On hydrogen empty(=0%)";case ι.Βΰ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΕЏ,int Ȱ,bool Ξ,String ƍ){Αη+=1;if(ΕЏ==Ȱ) ƍ=">"+ƍ+(ǋ>=2?" .":"");else ƍ=" "+ƍ;Γ+=ƍ+"\n";return ΕЏ==Ȱ&&Ξ;}String ΔΞ(bool Ξ)
{int Β=0;int Ο=і;Αη=0;String ΖΑ="———————————————\n";String ś="--------------------------------------------\n";String Γ="";Γ+=ΒΙ(ί)+" | "+(Θ.γ?"Ready to dock":"No dock")+"\n";Γ+=ΖΑ;double Ǯ=Math.Max(Math.Round(this.Л),0);if(Ќ==Ω.Main){bool ȳ=Η==Ζ.Ϊ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βσ();
if(Η==Ζ.Ŭ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) ο(Ω.ū);if(Η==Ζ.џ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) ο(Ω.ū);if(Η==Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) ο(Ω.Βο);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ƞ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) Ʊ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) č();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(ȳ) ο(Ω.ǀ);else ο(Ω.Ʃ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) ο(Ω.Δι);if(Η!=Ζ.Ϊ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) ο(Ω.Δθ);}else if(Ќ==Ω.ū){double Θώ=Math.Round(й*Ǳ,1);double ΗІ=Math.Round(и*Ǟ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) Βς();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){Ų(false,false);ο(Ω.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+Θώ+"m)")){Ĭ(ref й,5,20,1);Ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){Ĭ(ref й,-5,20,-1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΗІ+"m)")){Ĭ(ref и,5,20,1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){Ĭ(ref и,-5,20,-1);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(у==ь.ΑЎ?"Depth":"Min")+": "+Ц+"m)")){Ĭ(ref Ц,5,50,2);Ē(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){Ĭ(ref Ц,-5,50,-2);Ē(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+Ζω(у))){у=ȡ(у);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+Ζώ(Ț))){Ț=ȡ(Ț);}if(Η==Ζ.џ&&у==ь.ΑЇ) у=ȡ(у);Γ+=Δω(8,Ѓ,Ο,ref Θέ);}else if(Ќ==Ω.Βο){float[]ΕΟ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) ο(Ω.Δό);if(Π(ref Γ,Ο,Β++,Ξ," Back")) ο(Ω.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δη(ϋ.Ћ))) ϋ.Ћ=ȡ(ϋ.Ћ);if(!ϋ.ΒΠ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΘ((int) ϋ.ǒ))) ϋ.ǒ=ΔΧ(ϋ.ǒ,ΕΟ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δη(ό.Ћ))) ό.Ћ=ȡ(ό.Ћ);
if(!ό.ΒΠ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΘ((int) ό.ǒ))) ό.ǒ=ΔΧ(ό.ǒ,ΕΟ);}else if(Ќ==Ω.Δό){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βς();if(Π(ref Γ,Ο,Β++,Ξ," Back")) ο(Ω.Βο);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǔ!=""?ϋ.Ǔ:"-"))) ϋ.Ǔ=ƹ(ref Θξ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǖ!=""?ϋ.ǖ:"-"))) ϋ.ǖ=ƹ(ref Θπ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǔ!=""?ό.Ǔ:"-"))) ό.Ǔ=ƹ(ref Θο);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǖ!=""?ό.ǖ:"-"))) ό.ǖ=ƹ(ref Θρ);
}else if(Ќ==Ω.Ū){String Εψ=Ǭ+" %";if(Ā) ȱ++;if(ȱ>1){ȱ=0;ΑΦ++;if(ΑΦ>1) ΑΦ=0;}bool[]ΓЂ=new bool[]{Ă.Count==0,Ǧ==п.None,ѓ.Count==0};int ΖΪ=0;while(true){ΖΪ++;Ĝ++;if(Ĝ>ΓЂ.Length-1) Ĝ=0;if(ΖΪ>=ΓЂ.Length) break;if(!ΓЂ[Ĝ]) break;}bool ȳ=Η==Ζ.Ϊ;if(!ȳ&&ȝ&&О!=-1&&ΑΦ==0)
Εψ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){χ();ο(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!ȳ) ο(Ω.Ʃ);else ο(Ω.ǀ);if(!ȳ){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЀ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ɵ=true;Γ+=ś;if(!ȳ) Γ+="Progress: "+Math.Round(Ƌ,1)+" %\n";Γ+="State: "+ΒΜ(Υ)+" "+Ǯ+"m \n";Γ+="Load: "+Αέ+" % Max: "+Εψ+" \n";if(Ĝ==0) Γ+="Uranium: "+(Ă.Count==0?"No reactors":Math.Round(ħ,1)+"Kg "+(Ь==-1?"":" Min: "+Ь+" Kg"))+"\n";
if(Ĝ==1) Γ+="Battery: "+(Ǧ==п.None?Δί(Ǧ):Ǽ+"% "+(ц==-1||ȳ?"":" Min: "+ц+" %"))+"\n";if(Ĝ==2) Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(Ȓ,1)+"% "+(ч==-1||ȳ?"":" Min: "+ч+" %"))+"\n";}else if(Ќ==Ω.Ʃ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ί==Μ.Ѕ) ο(Ω.Ū);else ο(Ω.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) д(ref Ǭ,Ǭ<=80?-10:-5,σ.Ƶ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(ȝ?"On":"Off"))) ȝ=!ȝ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+Ζϊ(М))) М=ȡ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(ĥ?"On":"Off"))){ĥ=!ĥ;if(ĥ) ǣ(ΓЅ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(Ȝ?"On":"Off"))) Ȝ=!Ȝ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ь==-1?"Ignore":"Min "+Ь+"Kg"))) д(ref Ь,(Ь>5?-5:-1),σ.ƶ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ц==-1?"Ignore":"Min "+ц+"%"))) д(ref ц,-5,σ.ΒΩ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(ч==-1?"Ignore":"Min "+ч+"%"))) д(ref ч,-10,σ.ΒΪ,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(Ć?"Return home":"Stop"))) Ć=!Ć;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δα(л))) л=ȡ(л);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) ο(Ω.ΑЁ);Γ+=Δω(8,Ѓ,Ο,ref Θή);}else if(Ќ==Ω.ΑЁ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Ѕ) ο(Ω.Ū);else ο(Ω.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.џ?" Grinder":" Drill")+" inv. balancing: "+(ń?"On":"Off"))) ń=!ń;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.џ?" grinders":" drills")+": "+(ľ?"Fwd + Bwd":"Fwd"))) ľ=!ľ;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Š+"m/s")) д(ref Š,0.5f,σ.ų,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ş+"m/s")) д(ref ş,0.5f,σ.ų,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǥ*100f)+"%"+(ǥ>0.80f?" (risky)":""))) д(ref ǥ,0.1f,σ.Ƨ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ĩ*100f+"%")) ΔΣ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ȯ*100f+"%")) ΔΣ(false,0.05f);
}else if(Ќ==Ω.ǀ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Ѕ) ο(Ω.Ū);else ο(Ω.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǭ+"%")) д(ref Ǭ,Ǭ<=80?-10:-5,σ.Ƶ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȝ?"On":"Off"))) Ȝ=!Ȝ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ь==-1?"Ignore":"Min "+Ь+"Kg"))) д(ref Ь,(Ь>5?-5:-1),σ.ƶ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ц==-1?"Ignore":"Charge up"))) ц=(ц==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ч==-1?"Ignore":"Fill up"))) ч=(ч==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δα(л))) л=ȡ(л);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǥ*100f)+"%"+(ǥ>0.80f?" (risky)":""))) д(ref ǥ,0.1f,σ.Ƨ,false);
}else if(Ќ==Ω.Δψ){double ΗБ=0;if(Σ.Count>0) ΗБ=Vector3.Distance(Σ.Last().Δ,ΰ);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΔЀ();if(Η!=Ζ.Ϊ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȇ?"Use old home":(Θ.γ?"Was set! ":"none ")))) ȇ=!ȇ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȇ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) ȇ=!ȇ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ŝ?"Use old connector":(Ε.γ?"Was set! ":"none ")))) Ŝ=!Ŝ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ŝ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ŝ=!ŝ;Γ+=ś;Γ+="Wp spacing: "+Math.Round(Ζΰ)+"m\n";}else if(Ќ==Ω.Βτ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){χ();ο(Ω.Main);}
Γ+=ś;Γ+="State: "+ΒΜ(Υ)+" \n";Γ+="Speed: "+Math.Round(ș,1)+"m/s\n";Γ+="Target dist: "+Ǯ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΘ+"\n";}else if(Ќ==Ω.Δι){List<IMyTerminalBlock>ę=Δβ();if(Ā) ȱ++;if(ȱ>=ę.Count) ȱ=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) ο(Ω.Δκ);
Γ+=ś;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αέ,1)+"% "+Math.Round(ļ,1)+" / "+Math.Round(Ō,1)+"\n";Γ+="Uranium: "+(Ă.Count==0?"No reactors":Math.Round(ħ,1)+"Kg "+Γη)+"\n";Γ+="Battery: "+(Ǧ==п.None?"":Ǽ+"% ")+Δί(Ǧ)+"\n";Γ+="Hydrogen: "+(ѓ.Count==0?"No tanks":Math.Round(Ȓ,1)+"% ")+"\n";
Γ+="Damage: "+(ę.Count==0?"None":""+(ȱ+1)+"/"+ę.Count+" "+ę[ȱ].CustomName)+"\n";}else if(Ќ==Ω.Δκ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) ο(Ω.Main);Γ+=ś;Γ+="Next scan: "+Αρ+"s\n";Γ+="Ship size: "+Math.Round(Ǳ,1)+"m "+Math.Round(Ǟ,1)+"m "+Math.Round(ζ,1)+"m \n";
Γ+="Broadcast: "+(ĺ?"Online - "+ΓЀ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŋ*100f,1)+"% \n";}else if(Ќ==Ω.Δθ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) ο(Ω.Main);Γ+=ś;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+Αρ+"s\n";if(Αο) Γ="Recompile script now";int ΘΑ=Γ.Split('\n').Length;for(int i=ΘΑ;i<=10;i++) Γ+="\n";Γ+=ΖΑ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΔΣ(bool ΗЍ,float њ){χ();Ų(true,false);if(ΗЍ) д(ref ĩ,њ,σ.Βδ,false);else д(ref ȯ,њ,σ.Βδ,false);ΔΡ();đ(true,true,0,0);}float ΔΧ(float η,float[]ΑϏ){float έ=ΑϏ[0];for(int i=ΑϏ.Length-1;i>=0;i--)
if(η<ΑϏ[i]) έ=ΑϏ[i];return έ;}String ƹ(ref int Ȱ){String έ="";if(Ȱ>=ΓЎ.Count) Ȱ=-1;if(Ȱ>=0) έ=ΓЎ[Ȱ].CustomName;Ȱ++;return έ;}void ǃ(string φ){if(ί!=Μ.Ѕ) return;if(φ=="") return;IMyTerminalBlock Τ=ϊ.GetBlockWithName(φ);if(Τ==null||!(Τ is IMyTimerBlock)){Λ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Τ).Trigger();}void Ĭ(ref int value,int Ź,int ț,int ΓЃ){if(Ź==0) return;if(value<ț&&ΓЃ>0||value<=ț&&ΓЃ<0){value+=ΓЃ;return;}int Ηη=Math.Abs(Ź);int Βω=0;int Γξ=1;while(true){Βω+=Γξ*Ηη*10;if(Ź<0&&value-ț<=Βω) break;if(Ź>0&&value-ț<Βω) break;
Γξ++;}value+=Γξ*Ź;}void Ē(bool Ιγ){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);đ(Ќ==Ω.ū,false,0,0);}bool Ď(ref float value,bool ΘΪ,σ ρ,String ĝ,String ΗЊ){if(ĝ=="") return false;float ő=-1;bool Ελ=false;if(ĝ.ToUpper()==ΗЊ) Ελ=true;else if(!float.TryParse(ĝ,out ő)) return false;
else ő=Math.Max(0,ő);if(ΘΪ) ő=(float) Math.Round(ő);д(ref value,ő,ρ,Ελ,false);return true;}void д(ref float value,float њ,σ ρ,bool ƞ){д(ref value,value+њ,ρ,ƞ,true);}void д(ref float value,float ō,σ ρ,bool ƞ,bool ΘЂ){float ǘ=0,Ǘ=0;if(ρ==σ.ų){ǘ=0.5f;Ǘ=10f;}if(ρ==σ.Ƨ){ǘ=0.1f;Ǘ=1f;}
if(ρ==σ.Ŧ){ǘ=50f;Ǘ=100f;}if(ρ==σ.ΒΩ){ǘ=5f;Ǘ=30f;}if(ρ==σ.ƶ){ǘ=1f;Ǘ=25f;}if(ρ==σ.Ƶ){ǘ=10f;Ǘ=95f;}if(ρ==σ.ΒΪ){ǘ=10f;Ǘ=90f;}if(ρ==σ.ΗΧ){ǘ=10f;Ǘ=1800;}if(ρ==σ.Βδ){ǘ=0.0f;Ǘ=0.75f;}if(ō==-1&&ƞ){value=-1;return;}if(value==-1) ƞ=false;
bool ΕЈ=ō<ǘ||ō>Ǘ;if(ΕЈ&&ΘЂ){if(ō<value) value=Ǘ;else if(ō>value) value=ǘ;}else value=ō;if(ΕЈ&&ƞ) value=-1;else value=Math.Max(ǘ,Math.Min(value,Ǘ));value=(float) Math.Round(value,2);}void ΑЀ(bool ΘΥ){if(ΘΥ) К=Math.Max(0,К-1);else К++;ΒΓ();ΒΒ();for(int k=0;k<ǫ.Count&&k<Ш.Length;k++) Βμ(ǫ[k],k<К);
П=-1;Βχ(true);}T ȡ<T>(T η){int Β=Array.IndexOf(Enum.GetValues(η.GetType()),η);Β++;if(Β>=Ζφ(η)) Β=0;return(T) Enum.GetValues(η.GetType()).GetValue(Β);}int Ζφ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βσ(){ƥ.Clear();for(int i=0;i<Σ.Count;i++) ƥ.Add(Σ[i]);Σ.Clear();
ΑΪ=true;ΓΩ=new Χ(Θ);Γά=new Χ(Ε);Θ.γ=false;if(Η==Ζ.Ϊ) Ε.γ=false;for(int i=0;i<ћ.Count;i++)
if(!ȅ.Contains(ћ.Keys.ElementAt(i)))
ȅ.Add(ћ.Keys.ElementAt(i));ȇ=false;Ŝ=false;ŝ=false;ο(Ω.Δψ);}void ΔЀ(){if(ȇ) Θ=ΓΩ;if(Ŝ) Ε=Γά;if(ŝ){Σ.Clear();for(int i=0;i<ƥ.Count;i++) Σ.Add(ƥ[i]);}ΑΪ=false;χ();ο(Ω.Main);}void Ηα(){if(!ΑΪ) return;if(Υ!=Α.а){ΔЀ();return;}if(!ΓΩ.γ) ȇ=false;if(!Γά.γ) Ŝ=false;if(ƥ.Count<=1) ŝ=false;
IMyShipConnector Й=З(MyShipConnectorStatus.Connectable);if(Й==null) Й=З(MyShipConnectorStatus.Connected);if(Й!=null){if(Math.Round(ș,2)<=0.20) ΓΔ++;else ΓΔ=0;if(ΓΔ>=5){if(Η==Ζ.Ϊ&&(Θ.γ||ȇ)&&Vector3.Distance(Θ.Δ,Й.GetPosition())>5){Ε.β=Φ.WorldMatrix.Forward;Ε.ψ=Φ.WorldMatrix.Left;
Ε.α=Φ.WorldMatrix.Down;Ε.μ=Φ.GetNaturalGravity();Ε.Δ=Й.GetPosition();Ε.γ=true;Ε.Ч=Й.Position;}else{Θ.β=Φ.WorldMatrix.Forward;Θ.ψ=Φ.WorldMatrix.Left;Θ.α=Φ.WorldMatrix.Down;Θ.μ=Φ.GetNaturalGravity();Θ.Δ=Й.GetPosition();Θ.γ=true;Θ.Ч=Й.Position;}}}double Γή=-1;if(Σ.Count>0) Γή=Vector3.Distance(ΰ,Σ.Last().Δ);
double Θδ=Math.Max(1.5,Math.Pow(ș/100.0,2));double ΕЁ=Math.Max(ș*Θδ,2);Ζΰ=ΕЁ;if((Γή==-1)||Γή>=ΕЁ){Χ Ψ=new Χ(ΰ,ΑΣ,Ƃ,Ǝ,Φ.GetNaturalGravity());Ψ.Ζγ(Ȇ,ȅ);Σ.Add(Ψ);}}Χ ж(){if(Η!=Ζ.Ϊ) return ї!=null?ї:Θ;return ΕΙ;}void ı(Χ Ϋ,Μ Ф){ΕΙ=Ϋ;if(ί==Μ.Ѕ) х=Ф;}bool Ƿ(Χ Ϋ,float ύ,bool Θϊ,out Vector3 θ)
{if(Θϊ){Vector3I ΓΠ=new Vector3I((int) Ϋ.Ч.X,(int) Ϋ.Ч.Y,(int) Ϋ.Ч.Z);IMySlimBlock Τ=Me.CubeGrid.GetCubeBlock(ΓΠ);if(Τ==null||!(Τ.FatBlock is IMyShipConnector)){θ=new Vector3();return false;}Vector3 Ηυ=б(Φ,Τ.FatBlock.GetPosition()-ΰ);Vector3 Ητ=б(Φ,Τ.FatBlock.WorldMatrix.Forward);θ=Ϋ.Δ-Βφ(Ϋ.β,Ϋ.α*-1,Ηυ)-Βφ(Ϋ.β,Ϋ.α*-1,Ητ)*ύ;
return true;}else{θ=Ϋ.Δ;return true;}}Vector3 ȉ(int Ης,int Ŕ,bool ΘΨ){if(!ΘΨ&&Εγ) return ΒЉ;float Ησ=((τ-1f)/2f)-Ης;float ΘΫ=((Н-1f)/2f)-Ŕ;ΒЉ=Ε.Δ+Ε.ψ*Ησ*Ǳ+Ȗ*-1*ΘΫ*Ǟ;Εγ=true;return ΒЉ;}Vector3 ΒΛ(Vector3 Θι,float ΒБ){return Θι+(Ǚ*ΒБ);}ή ΑЏ(){float А=-1;ή έ=ή.Ǵ;
if(Η!=Ζ.Ϊ){Δρ();if(ί!=Μ.з){Vector3 Ј=Đ(Ǚ,Ȗ*-1,ΰ-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(τ*Ǳ)/2f&&Math.Abs(Ј.Y)<=(float)(Н*Ǟ)/2f){if(Ј.Z<=-1&&Ј.Z>=-Ģ*2) return ή.ƨ;if(Ј.Z>-1&&Ј.Z<ζ*2) return ή.Ǵ;}if(ȟ(Ε.Δ,ref А)) έ=ή.Ǵ;}if(Θ.γ){if(ȟ(Θ.Δ,ref А)) έ=ή.ǳ;for(int i=0;i<Σ.Count;i++)
if(ȟ(Σ[i].Δ,ref А)) έ=ή.Ȋ;if(Vector3.Distance(ΰ,Θ.Δ)<dockDist*ζ) έ=ή.ǳ;if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null)
έ=ή.ǳ;}}else{Vector3 θ=new Vector3();IMyShipConnector ǿ=З(MyShipConnectorStatus.Connected);if(Θ.γ){if(ȟ(Θ.Δ,ref А)) έ=ή.ǳ;if(Ƿ(Θ,dockDist,true,out θ))
if(ȟ(θ,ref А)) έ=ή.ǳ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Θ.Δ)<5)
return ή.ΑЄ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*ζ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*ζ)
if(ȟ(Σ[i].Δ,ref А)) έ=ή.Ȋ;if(Ε.γ){if(ȟ(Ε.Δ,ref А)) έ=ή.Ǵ;if(Ƿ(Ε,dockDist,true,out θ))
if(ȟ(θ,ref А)) έ=ή.Ǵ;if(ǿ!=null&&Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5)
return ή.ΑЅ;}}return έ;}bool ȟ(Vector3 θ,ref float А){float ύ=Vector3.Distance(θ,ΰ);if(ύ<А||А==-1){А=ύ;return true;}return false;}void Βς(){if(ώ>0){Λ="Setup error! Can't start";return;}if(Η==Ζ.Ϊ){Ƞ();return;}Ε.Δ=ΰ;Ε.μ=Φ.GetNaturalGravity();Ε.β=ΑΣ;Ε.α=Ƃ;Ε.ψ=Ǝ;
Ǚ=ј.WorldMatrix.Forward;Ȗ=Ε.α;if(Ǚ==Φ.WorldMatrix.Down) Ȗ=Φ.WorldMatrix.Backward;Ų(true,true);Ν(Α.ȣ);Βρ();}void Ų(bool ǔ,bool ΘΩ){if(ί==Μ.з&&!ǔ) return;bool ΘΚ=ǔ||ί==Μ.ΑБ||τ!=й||Н!=и||Ğ!=Ț;if(ΘΚ){if(ί!=Μ.з){ί=Μ.ƫ;ȉ(Щ,Ъ,ΘΩ);Λ="Job changed, lost progress";}Ğ=Ț;
τ=й;Н=и;Ъ=0;Щ=0;Ш="";П=-1;Ġ=0;Ȕ=0;С=0;К=0;Βχ(true);}}void Βθ(){Р(ΰ,0);я(Ȇ,true);}void Βρ(){ǹ(в.ƾ);Βθ();Ű(Ȃ,false);ǣ(ΓЅ);ί=Μ.Ѕ;ΔΠ(true);х=ί;ο(Ω.Ū);ΗΕ();ΑΖ=true;ΑΧ=0;for(int i=Ŵ.Count-1;i>=0;i--)
if(Ũ(Ŵ[i],false)) ΑΧ++;if(ΑΧ>0) Λ="Started with damage";}void χ(){if(ί==Μ.Ѕ){ί=Μ.İ;Λ="Job paused";}Ν(Α.а);х=ί;ƿ(false,0,0,0,0);Βυ();ΔΝ(new Vector3(),false);ΔϏ();Βλ(ChargeMode.Auto);Βν(false);Δϋ(true);Ȍ(Α.а);đ(false,false,0,0);я(ς,false);я(т,true);ǣ(true);Απ=false;Ƭ();ΑΓ=false;
ƀ=false;Βι();ΑΖ=false;Ƙ=false;Ɵ=false;if(Ќ!=Ω.Main&&Ќ!=Ω.Ʃ&&Ќ!=Ω.ΑЁ&&Ќ!=Ω.ǀ)
ο(Ω.Main);}void Ƞ(){ή Г=ΑЏ();if(Η==Ζ.Ϊ){if(!Ε.γ||!Θ.γ) return;Βρ();bool ΓΞ=Vector3.Distance(ΰ,Θ.Δ)<Vector3.Distance(ΰ,Ε.Δ);if(ŗ==Μ.Ī) ΓΞ=true;if(ŗ==Μ.ī) ΓΞ=false;if(ΓΞ){ı(Θ,Μ.Ī);switch(Г){case ή.ΑЄ:Ν(Α.Ȧ);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ǳ:Ν(Α.Ǡ);break;default:Ν(Α.ǎ);break;
}}else{ı(Ε,Μ.ī);switch(Г){case ή.ΑЅ:Ν(Α.Ȧ);break;case ή.Ǵ:Ν(Α.Ǡ);break;case ή.Ȋ:Ν(Α.Я);break;default:Ν(Α.ǎ);break;}}}else{if(ί!=Μ.İ&&ί!=Μ.ƫ) return;bool Θό=ί==Μ.ƫ;Βρ();bool ΓΤ=ũ(false)&&Θ.γ;switch(Г){case ή.Ǵ:Ν(ΓΤ?Α.Ȣ:Α.ȣ);break;case ή.Ȋ:Ν(ΓΤ?Α.І:Α.Я);break;case ή.ǳ:Ν(ΓΤ?Α.Ǡ:Α.ǻ);break;
case ή.ƨ:{if(Ȕ!=К||Θό) Ν(Α.ȋ);else Ν(Α.ŭ);}break;default:break;}}}void č(){if(ί==Μ.з&&!Θ.γ) return;if(Η==Ζ.Ϊ&&(!Ε.γ||!Θ.γ)) return;Λ="Move to job";ή Г=ΑЏ();if(Η==Ζ.Ϊ){ı(Ε,Μ.ī);switch(Г){case ή.Ǵ:Ν(Α.Ǡ);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ΑЅ:return;default:Ν(Α.ǎ);break;
}Ȍ(Α.Ȧ);}else{switch(Г){case ή.Ǵ:Ν(Α.Я);break;case ή.Ȋ:Ν(Α.Я);break;case ή.ǳ:Ν(Α.ǻ);break;case ή.ƨ:Ν(Α.ȋ);break;default:break;}if(ί==Μ.з) Ȍ(Α.Я);else Ȍ(Α.Ȟ);Απ=true;}Βθ();ο(Ω.Βτ);Ű(Ȃ,false);х=Μ.Ƹ;}void Ʊ(){if(!Θ.γ) return;Λ="Move home";ή Г=ΑЏ();if(Η==Ζ.Ϊ){ı(Θ,Μ.Ī);switch(Г)
{case ή.Ȋ:Ν(Α.І);break;case ή.ǳ:Ν(Α.Ǡ);break;case ή.ΑЄ:return;default:Ν(Α.ǎ);break;}Ȍ(Α.Ȧ);}else{if(З(MyShipConnectorStatus.Connected)!=null) return;if(З(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Č);Ȍ(Α.ǻ);return;}switch(Г){case ή.Ǵ:Ν(Α.Ȣ);break;case ή.Ȋ:Ν(Α.І);break;case ή.ǳ:Ν(Α.І);break;
case ή.ƨ:Ν(Α.ď);break;default:break;}Ȍ(Α.ǻ);}Βθ();ο(Ω.Βτ);Ű(Ȃ,false);х=Μ.Ʒ;}void Ν(Α Γο){if(Γο==Α.а) ƚ=Α.а;if(ƚ!=Α.а&&Υ==ƚ&&Γο!=ƚ){χ();return;}Ё=true;Υ=Γο;}void Ȍ(Α Ф){this.ƚ=Ф;}void Ηί(){if(Υ==Α.ȣ){if(Ё){Ђ=0;if(Ȕ!=К) Ġ=0;Ȕ=К;}if(Ђ==0){int ΒЋ=Ζκ(К);if(ΒЋ<0)
{if(!ΑΓ){ΑΓ=true;ΕΘ=ΰ;}Р(ΕΘ,0);Λ="Waiting for a free hole";return;}ΑΓ=false;if(ΒЋ!=К){К=ΒЋ;Ġ=0;Ȕ=К;}Ĳ έ=ΗΜ(К);if(έ==Ĳ.ΔΜ){ί=Μ.ΑБ;Λ="Job done";if(Ć&&Θ.γ){Ν(Α.Ȣ);Ȍ(Α.ǻ);х=Μ.Ʒ;}else{Ν(Α.Į);Ȍ(Α.Ȟ);х=Μ.Ƹ;}return;}if(έ==Ĳ.Δή){Ђ=1;я(ς,true);ш=ȉ(Щ,Ъ,true);
Р(ш,10);Х(Ε.α,Ε.β,Ε.ψ,false);}}else{if(П>=0&&ΒΣ(П,true)){Ђ=0;return;}if(Л<wpReachedDist){Ν(Α.ŭ);return;}}}if(Υ==Α.ŭ){if(Ё){я(ς,true);ǣ(false);ш=ȉ(Щ,Ъ,false);Р(ΒΛ(ш,0),0);Х(Ε.α,Ε.β,Ε.ψ,false);Ђ=1;к=0;Ю=0;ǌ=0;Γί=-1;Ģ=Ц;ΓΪ=true;}if(!ΑЊ()){Ν(Α.ď);return;}
if(ũ(true)){ΕІ=Ȉ("","ORE",И.ĉ);if((М==Ѐ.ΒΑ||М==Ѐ.ƭ||
М==Ѐ.ΒΤ||М==Ѐ.Ƴ)&&Η!=Ζ.џ)
Ν(Α.Ť);else if((М==Ѐ.ΔΦ||М==Ѐ.ΑЌ)&&Η!=Ζ.џ)
Ν(Α.Βψ);else
Ν(Α.ď);return;}С=Vector3.Distance(ΰ,ш);if(С>Ġ){Ġ=С;ΓΪ=false;}if(Η==Ζ.џ&&Δγ()==MyDetectedEntityType.SmallGrid)
Ю+=2;else Ю-=2;Ю=Math.Max(100,Math.Min(400,Ю));if(Ђ>0&&Ђ<Ю){if(С>к){if(Ю>150) к=С;else к=(float) Math.Ceiling(С);Ђ=1;}else Ђ++;}else{if(Ђ>0){Λ="Ship stuck! Retrying";к=С;Ђ=0;đ(false,true,0,ζ*sensorRange);}Р(ΒΛ(ш,Math.Max(0,к-ζ)),ΒΞ(false));if(Л<=wpReachedDist/2){Ђ=1;к=0;}
return;}đ(false,true,ζ*sensorRange,0);Vector3 Ηι=ш+Ǚ*С;bool Εο=false;if(Vector3.Distance(Ηι,ΰ)>0.3f){Vector3 Ηφ=ш+Ǚ*(С+0.1f);Р(Ηφ,4);Εο=true;}else{float Ǜ=ΒΞ(true);Vector3 ΕΞ=ΒΛ(ш,Math.Max(Ц+1,С+1));Ǣ(true,false,false,ΕΞ,ΕΞ-ш,Ǜ,Ǜ);}bool ΓΑ=false;if(у==ь.ΑЇ||у==ь.ΑІ)
{if(!Εο){float Γρ=0;foreach(IMyTerminalBlock ǰ in ς)
Γρ+=Ζρ(ǰ,"","",у==ь.ΑІ?new string[]{"STONE"}:null);if(Γρ>Γί||С<Ц||ΓΪ){ΒϏ=0;ǌ=С;Ģ=(int)(Math.Max(Ģ,ǌ)+ζ/2);}else{ΓΑ=С-ǌ>2&&ΒϏ>=20;ΒϏ++;}Γί=Γρ;}}else ΓΑ=С>=Ģ;if(Ȕ!=К){Ν(Α.ȋ);С=0;return;}if(ΓΑ){К++;Ν(Α.ȋ);С=0;return;}}if(Υ==Α.ȋ||Υ==Α.ď||Υ==Α.Ť)
{if(Ё){ш=ȉ(Щ,Ъ,false);Х(Ε.α,Ε.β,Ε.ψ,false);я(ς,ľ);ǣ(false);к=Vector3.Distance(ΰ,ш);đ(false,true,0,ζ*sensorRange);}Р(ш,ΒΞ(false));if(Vector3.Distance(ΰ,ш)>=к+5){я(ς,false);ǣ(true);Λ="Can´t return!";}if(Л<wpReachedDist){if(Υ==Α.ȋ&&Απ) Ν(Α.Į);if(Υ==Α.ȋ) Ν(Α.ȣ);
if(Υ==Α.Ť) Ν(Α.ư);if(Υ==Α.ď){if(Θ.γ) Ν(Α.Ȣ);else{χ();č();Λ="Can´t return, no dock found";}}return;}}if(Υ==Α.І||Υ==Α.Я){if(Υ==Α.Я&&ί==Μ.Ѕ&&Η!=Ζ.Ϊ){if(!ΑЊ()||ũ(true)){Ν(Α.І);return;}}bool Γϋ=false;bool Αύ=false;bool ΔЍ=false;float ķ=0;bool ΕЅ=false;Χ Ψ=null;
if(Ё){if(Υ==Α.І||Η==Ζ.Ϊ){if(Η!=Ζ.Ϊ){if(Ȥ(1)) ΑЃ();else{ї=null;ȃ=0;ƀ=!ΑЋ(1);}}Χ Ϋ=ж();ε=new Βζ();ε.Љ=Ϋ;ε.ž=followPathDock*ζ;ε.ΔΓ=useDockDirectionDist*ζ;ε.ķ=10;ε.Ś.Add(Ϋ.Δ);Vector3 Ķ=new Vector3();if(Ƿ(Ϋ,dockDist*ζ,true,out Ķ))
ε.Ś.Add(Ķ);else ε.ž*=1.5f;if(Η==Ζ.Ϊ){if(Ϋ==Θ) ε.ΑΟ=Ε.Δ;if(Ϋ==Ε) ε.ΑΟ=Θ.Δ;ε.ΓΛ=dockDist*ζ*1.1f;}}else if(Υ==Α.Я){ε=new Βζ();ε.Љ=Ε;ε.ž=followPathJob*ζ;ε.ΔΓ=useJobDirectionDist*ζ;ε.ķ=10;ε.ΑΟ=Θ.Δ;ε.ΓΛ=dockDist*ζ*1.1f;ε.Ś.Add(Ε.Δ);if(ί==Μ.з){if(!Θ.γ||Σ.Count==0){χ();return;}
float Ηό=Vector3.Distance(Σ.First().Δ,Θ.Δ);float Ηύ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(Ηό<Ηύ) ε.Љ=Σ.Last();else ε.Љ=Σ.First();}}Ɠ=new Vector3();ΕЅ=!ΖЇ(ΰ);я(ς,false);ǣ(true);У=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,ε.ΑΟ)<=ε.ΓΛ) continue;
double ύ=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǩ==-1||ύ<Ǩ){У=i;Ǩ=ύ;}}ǭ=Ζύ(ε.Љ.Δ,У);Ę=null;}if(Υ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&!ű()){Χ ΒЃ=ж();ƀ=!ΑЋ(1);if(ж()!=ΒЃ){Ν(Α.І);return;}}if(Υ==Α.І&&Η!=Ζ.Ϊ&&!Ё&&Ȥ(1)&&ΑЃ()){Ν(Α.І);return;}bool Εν=false;if(Υ==Α.І&&!Ё&&У>=0&&У<Σ.Count)
{Vector3 ż=Σ[У].Δ+Δδ(У,ǭ);int Γπ=У+ǭ;Vector3 Ŏ=ǭ!=0&&Γπ>=0&&Γπ<Σ.Count?Σ[Γπ].Δ:ε.Љ.Δ;if(ΖЀ(ż,Ŏ)){if(!ΑΙ){ΑΙ=true;ΕΣ=į(ż)?ΰ:ż;}Р(ΕΣ,0);Х(ε.Љ.α,ε.Љ.β,ε.Љ.ψ,false);Λ="Waiting for dock clearance";return;}ΑΙ=false;Εν=Ȥ(1)&&(į(ΰ)||į(Ŏ));}Ζδ(Σ,ǭ,ε.Ś,ε.ž,Ё,ref Ђ);
for(int i=0;i<ε.Ś.Count;i++){float ύ=Vector3.Distance(ΰ,ε.Ś[i]);if(ύ<=ε.ž) Γϋ=true;if(ύ<=ε.ΔΓ) Αύ=true;}if(Εν) Αύ=true;if(Αύ) ķ=ε.ķ;float Ιε=Ę!=null?Ę.Ў:ș;float ΘЁ=(float) Math.Max(ș*0.1f*ζ,wpReachedDist);if((Л<ΘЁ)||Ё){if(!Ё) У+=ǭ;if(ǭ==0||У>Σ.Count-1||У<0)
Γϋ=true;else{ΔΘ=ǭ>0?Σ.Count-1-У:У;Ψ=Σ[У];Ę=Ψ;if(У>=1&&У<Σ.Count-1)
Ɠ=Ψ.Δ-Σ[У-ǭ].Δ;else Ę=null;ω=Ψ.Δ+Δδ(У,ǭ);ΔЍ=true;}}if(Αύ)
Х(ε.Љ.α,ε.Љ.β,ε.Љ.ψ,false);else if(ΕЅ)
ΑЂ(ε.Љ.α,10,true);else if(ΔЍ&&Ψ!=null){if(ǭ>0) Х(Ψ.α,Ψ.β,Ψ.ψ,90,false);else Х(Ψ.α,-Ψ.β,-Ψ.ψ,90,false);}Ǣ(true,false,true,ω,Ɠ,Ę==null?0:Ę.Ў,ķ);if(Γϋ){ΔΘ=0;if(Υ==Α.І&&!Ζθ()){Р(ω,0);return;}if(Υ==Α.І&&ƀ){Р(ω,0);Λ="Waiting for a free dock";return;}if(Υ==Α.І||Η==Ζ.Ϊ){Ν(Α.Ǡ);return;}
if(Υ==Α.Я&&Απ){Ν(Α.Į);return;}if(Υ==Α.Я){Ν(Α.ȣ);return;}}}if(Υ==Α.Ǡ||Υ==Α.Ů){if(Η!=Ζ.Ϊ&&ű()){if(!Ȥ(1)){Ν(Α.І);return;}}else if(Η!=Ζ.Ϊ){Χ ΒЃ=ж();if(!ΑЋ(2)||ж()!=ΒЃ){Ν(Α.І);return;}}Χ Ϋ=ж();if(Ё){if(!Ƿ(Ϋ,dockDist*ζ,true,out ω)){ǹ(в.Ţ);χ();return;}
Р(ω,0);ΑЂ(Ϋ.α,90,true);}if(Л<followPathDock*ζ&&Л!=-1){Р(ω,10);Х(Ϋ.α,Ϋ.β,Ϋ.ψ,false);}if(З(MyShipConnectorStatus.Connectable)!=null||З(MyShipConnectorStatus.Connected)!=null){Ν(Α.Č);return;}if(Л<wpReachedDist/2&&Л!=-1){Ν(Α.Ĉ);return;}}if(Υ==Α.Ĉ||Υ==Α.Ȟ){if(Ё)
{if(Υ==Α.Ĉ){Χ Ϋ=ж();if(!Ƿ(Ϋ,dockDist*ζ,true,out ω)){ǹ(в.Ţ);χ();return;}Ǣ(true,true,false,ω,0);Х(Ϋ.α,Ϋ.β,Ϋ.ψ,10,false);}if(Υ==Α.Ȟ){Х(Ε.α,Ε.β,Ε.ψ,0.5f,false);ω=Ε.Δ;Ǣ(true,true,false,ω,0);}}if(ΑΩ){ƿ(false,0,0,0,0);if(Υ==Α.Ĉ) Ν(Α.Č);if(Υ==Α.Ȟ) χ();return;}}if(Υ==Α.Č)
{if(З(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϊ) Ν(Α.Ȧ);else Ν(Α.ǻ);return;}Χ Ϋ=ж();if(Ё){Ю=0;Ηώ=DateTime.Now;Ђ=0;Х(Ϋ.α,Ϋ.β,Ϋ.ψ,false);}Vector3I ΓΠ=new Vector3I((int) Ϋ.Ч.X,(int) Ϋ.Ч.Y,(int) Ϋ.Ч.Z);IMySlimBlock ΕΗ=Me.CubeGrid.GetCubeBlock(ΓΠ);float ΕΥ=dockingSpeed;float ΘΔ=dockingSpeed*5;
float ΔЊ=Math.Max(1.5f,Math.Min(5f,ζ*0.15f));if(!Ƿ(Ϋ,0,true,out ω)||!Ƿ(Ϋ,ΔЊ,true,out Ɠ)||
ΕΗ==null||!ΕΗ.FatBlock.IsFunctional){ǹ(в.Ţ);χ();return;}if(Ю==1||(Vector3.Distance(ΰ,ω)<=ΔЊ*1.1f&&!Ё))
Ю=1;else{Vector3 Θς=б(Φ,Ɠ-ΰ);Vector3 ΓΟ=б(Φ,Φ.GetNaturalGravity());float Θά=ƪ(Θς,ΓΟ,null);ΕΥ=Math.Min(ΘΔ,Θά);}Ǣ(true,false,false,ω,ω-ΰ,dockingSpeed,ΕΥ);if(Ё) к=(float) Л;IMyShipConnector ΒЎ=З(MyShipConnectorStatus.Connectable);if(ΒЎ!=null){Ǣ(false,false,false,ω,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΒЎ.Connect();if(ΒЎ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϊ) Ν(Α.Ȧ);else Ν(Α.ǻ);Βυ();Ű(Ȃ,true);return;}}}else{float ΕΡ=(float) Math.Round(Л,1);if(ΕΡ<к){Ђ=-1;к=ΕΡ;}else Ђ++;if(Ђ>20){Ν(Α.Ů);return;}}}if(Υ==Α.ǻ||Υ==Α.Ȧ||
Υ==Α.Ǉ||Υ==Α.Ư||Υ==Α.ċ){bool Βύ=false,Βώ=false;if(Η==Ζ.Ϊ){if(ж()==Θ) Βύ=true;else if(ж()==Ε) Βώ=true;}if(Ё){Ƙ=false;if(З(MyShipConnectorStatus.Connected)==null){Ν(Α.ǎ);return;}Βυ();if(Βύ) ǃ(ϋ.Ǔ);if(Βώ) ǃ(ό.Ǔ);Ħ=false;ȍ=false;ȑ=false;ơ=false;}if(З(MyShipConnectorStatus.Connected)==null){χ();ǹ(в.Δμ);return;}
if(ί!=Μ.Ѕ||ц==-1||Ǧ==п.None) ȍ=true;else if(Ǽ>=100f) ȍ=true;else if(Ǽ<=99f) ȍ=false;if(ί!=Μ.Ѕ||ч==-1||ѓ.Count==0) ȑ=true;else if(Ȓ>=100f) ȑ=true;else if(Ȓ<=99) ȑ=false;if(ί!=Μ.Ѕ||Ь==-1||Ă.Count==0) ơ=true;else ơ=ħ>=Ь;ĳ о=null;if(Βύ) о=ϋ;
if(Βώ) о=ό;if(о!=null&&(о.Ћ==ι.ƻ||о.Ћ==ι.ƺ)) ȍ=true;if(о!=null&&(о.Ћ==ι.Βέ)) if(!Ħ) ȍ=false;if(о!=null&&(о.Ћ==ι.Βα||о.Ћ==ι.Βί)) ȑ=true;if(о!=null&&(о.Ћ==ι.Βΰ)) if(!Ħ) ȑ=false;if(Ā){ChargeMode ΕΕ=ȍ?ChargeMode.Auto:ChargeMode.Recharge;if(о!=null&&(о.Ћ==ι.ƺ||о.Ћ==ι.ƻ))
ΕΕ=ChargeMode.Discharge;Βλ(ΕΕ);Βν(!ȑ);}if(!Ħ){if(Η==Ζ.Ϊ)
Ħ=ί!=Μ.Ѕ||Ζι(Ё,true)||Ɵ;else
Ħ=ί!=Μ.Ѕ||Δν();}else{if(!ȍ) Ν(Α.ċ);if(!ȑ) Ν(Α.Ư);if(!ơ) Ν(Α.Ǉ);Ё=false;}if(Ħ&&ȍ&&ȑ&&ơ){Βλ(ChargeMode.Auto);Βν(false);if(ί==Μ.Ѕ){if(Η==Ζ.Ϊ){if(ж()==Θ) ǃ(ϋ.ǖ);else if(ж()==Ε) ǃ(ό.ǖ);if(ж()==Θ) ı(Ε,Μ.ī);else ı(Θ,Μ.Ī);}}Ν(Α.ǎ);return;}}if(Υ==Α.ǎ){if(Ƥ){if(!ΔΤ()) return;
Ƥ=false;Ν(Α.ǎ);return;}if(Ё){if(!ΔΤ()){Ƥ=true;return;}IMyShipConnector ǿ=З(MyShipConnectorStatus.Connected);if(ǿ==null){Ν(Α.Я);return;}IMyShipConnector δ=ǿ.OtherConnector;Δϊ(ǿ,false);Ű(Ȃ,false);Χ Ϋ=null;Χ ΓΨ=Η==Ζ.Ϊ?Θ:ж();if(Vector3.Distance(ǿ.GetPosition(),ΓΨ.Δ)<5f&&Θ.γ) Ϋ=ΓΨ;
if(Vector3.Distance(ǿ.GetPosition(),Ε.Δ)<5f&&Ε.γ) Ϋ=Ε;if(Ϋ!=null){if(!Ƿ(Ϋ,dockDist*ζ,true,out ω)){ǹ(в.Ţ);χ();return;}Р(ω,5);Х(Ϋ.α,Ϋ.β,Ϋ.ψ,false);}else Р(ΰ+δ.WorldMatrix.Forward*dockDist*ζ,5);if(ί==Μ.Ѕ) ǹ(в.ƾ);}if(Л<wpReachedDist){я(т,true);Ν(Α.Я);return;}}if(Υ==Α.Į)
{if(Ё){ǣ(true);я(ς,false);ω=Ε.Δ;Р(ω,20);Х(Ε.α,Ε.β,Ε.ψ,false);}if(Л<wpReachedDist/2){Ν(Α.Ȟ);return;}}if(Υ==Α.Ȣ){if(Ё){ǣ(true);я(ς,false);int А=-1;double Ǩ=-1;for(int i=Σ.Count-1;i>=0;i--){double ύ=Vector3.Distance(Σ[i].Δ,ΰ);if(Ǩ==-1||ύ<Ǩ){А=i;Ǩ=ύ;}}
if(А==-1){Ν(Α.І);return;}ω=Σ[А].Δ;Р(ω,10);Х(Ε.α,Ε.β,Ε.ψ,false);}if(Л<wpReachedDist){Ν(Α.І);return;}}if(Υ==Α.ư){bool ђ=false;if(Ё){ǣ(true);if((М==Ѐ.ΒΑ||М==Ѐ.ƭ)&&ƴ()&&
ĭ(Ǚ,Φ.GetNaturalGravity())<25&&τ>=2&&Н>=2){Vector3 ƃ=ΰ;if(Щ>0&&Ъ<Н-1) ƃ=ȉ(Щ-1,Ъ+1,true);else if(Щ<τ-1&&Ъ<Н-1) ƃ=ȉ(Щ+1,Ъ+1,true);else if(Щ<τ-1&&Ъ>0) ƃ=ȉ(Щ+1,Ъ-1,true);else if(Щ>0&&Ъ>0) ƃ=ȉ(Щ-1,Ъ-1,true);else ђ=true;if(!ђ) Р(ƃ,10);
}else ђ=true;}if(Л<wpReachedDist/2) ђ=true;if(ђ){Ν(Α.ǆ);return;}}if(Υ==Α.Βψ||Υ==Α.ǆ){if(Ё){Ǣ(true,true,false,ΰ,0);я(ς,false);ǣ(true);Ђ=-1;Ю=М==Ѐ.ΒΤ||М==Ѐ.Ƴ?0:-1;}bool Ě=!ΑЊ();int Αχ=Ȉ("STONE","ORE",И.ĉ);if(М==Ѐ.ΑЌ||М==Ѐ.Ƴ||М==Ѐ.ƭ)
Αχ+=Ȉ("ICE","ORE",И.ĉ);bool ΗЅ=Αχ>0;bool ΓΚ=false;if(Ю>=0){float Ĵ=(float) Math.Sin(ΔΨ(Ю))*Ǳ/3f;float Ηλ=(float) Math.Cos(ΔΨ(Ю))*Ǟ/3f;Vector3 Ηρ=ȉ(Щ,Ъ,true)+Βφ(Ǚ,Ȗ*-1,new Vector3(Ĵ,Ηλ,0));Р(Ηρ,0.3f);if(Л<Math.Min(Ǳ,Ǟ)/10f) Ю+=5f;if(Ю>=360) Ю=0;}if(Ђ==-1||Αχ<Ђ){Ђ=Αχ;к=0;}
else{к++;if(к>50) ΓΚ=true;}if(!ΗЅ||Ě||ΓΚ){if(!Ě){int Θχ=Ȉ("","ORE",И.ĉ);if(ũ(true)) Ě=true;else if(100-(Џ(Θχ,ΕІ)*100)<minEjection) Ě=true;else ǹ(в.ƾ);}if(ΓΚ&&Ě) Λ="Ejection failed";if(Υ==Α.ǆ){if(Ě){if(Θ.γ) Ν(Α.Ȣ);else{χ();č();Λ="Can´t return, no dock found";}}
else Ν(Α.ȣ);}else if(Ě) Ν(Α.ď);else Ν(Α.ŭ);return;}}Ё=false;}void Ζδ(List<Χ>ć,int κ,List<Vector3>Ɯ,float Ǯ,bool Ȅ,ref int ǐ){if(Ȅ){for(int i=0;i<ć.Count;i++) ć[i].Ў=0;ǐ=-1;return;}if(κ==0) return;int њ=κ*-1;if(ǐ==-1) ǐ=њ>0?1:ć.Count-2;int Ερ=0;while(ǐ>=1&&ǐ<ć.Count-1)
{if(Ερ>50) return;Ερ++;try{if((њ<0&&ǐ>=1)||(њ>0&&ǐ<=ć.Count-2)){Χ η=ć[ǐ];bool ΕЃ=false;for(int j=0;j<Ɯ.Count;j++)
if(Vector3.Distance(η.Δ,Ɯ[j])<=Ǯ){ΕЃ=true;break;}if(!ΕЃ){Χ Αμ=ć[ǐ-њ];Χ Ŏ=ć[ǐ+њ];Vector3 Θσ=η.Δ-Ŏ.Δ;Vector3 ΓЏ=Αμ.Δ-η.Δ;Vector3 ΘΦ=η.Δ+Vector3.Normalize(Θσ)*ΓЏ.Length();Vector3 Ηϋ=Αμ.Δ-ΘΦ;Vector3 Αή=Đ(κ>0?η.β:η.β*-1,η.α*-1,Ηϋ);Vector3 ΘΓ=Đ(κ>0?η.β:η.β*-1,η.α*-1,ΓЏ);
Vector3 Αΰ=Đ(κ>0?η.β:η.β*-1,η.α*-1,η.μ);η.Ў=(float) Math.Sqrt(Math.Pow(Αμ.Ў,2)+Math.Pow(ƪ(-ΘΓ,Αΰ,η),2));for(int j=0;j<Ɯ.Count;j++)
if(Vector3.Distance(Αμ.Δ,Ɯ[j])<=Ǯ){Vector3 Αα=Đ(κ>0?η.β:η.β*-1,η.α*-1,Ɯ[j]-η.Δ);float Θλ=ƪ(-Αα,Αΰ,η);η.Ў=Math.Min(η.Ў,Θλ)/2f;}if(Αή.Length()==0) Αή=new Vector3(0,0,1);Vector3 ΘΒ=Đ(η.β,η.α*-1,η.μ);float Θν=ť(-Αή,ΘΒ,η);float ǈ=Џ(Θν,Ĥ);float ΓЍ=(float) Math.Sqrt(Џ(Αή.Length()*1.0f,0.5f*ǈ));
η.Ў=Math.Min(η.Ў,(ΓЏ.Length()/ΓЍ)*ǥ);}}}catch{return;}ǐ+=њ;}ǐ=-1;}void Βχ(bool Ȅ){if(Ȅ){Ƌ=0;return;}if(Η==Ζ.Ϊ) return;float ђ=К*Math.Max(1,Ц);if(Ȕ==К) ђ+=Math.Min(Ц,С);float Θφ=τ*Н*Math.Max(1,Ц);Ƌ=Math.Max(Ƌ,(float) Math.Min(ђ/Θφ*100.0,100));}MyDetectedEntityType Δγ()
{try{if(Ǹ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΞ(bool Ǫ){if(Η==Ζ.џ&&Δγ()==MyDetectedEntityType.None&&!Ũ(ξ,true))
return fastSpeed;else return Ǫ?Š:ş;}void ΗΩ(){float Εώ=0,ΕΛ=0,ĝ=0,Γς=0;for(int i=0;i<ȧ.Count;i++){IMyBatteryBlock Ÿ=ȧ[i];if(!Ǹ(Ÿ,true)) continue;Εώ+=Ÿ.MaxStoredPower;ΕΛ+=Ÿ.CurrentStoredPower;ĝ+=Ÿ.CurrentInput;Γς+=Ÿ.CurrentOutput;}Ǽ=(float) Math.Round(Џ(ΕΛ,Εώ)*100,1);
if(ĝ>=Γς) Ǧ=п.ċ;else Ǧ=п.ΔΩ;if(ĝ==0&&Γς==0||Ǽ==100.0) Ǧ=п.а;if(ȧ.Count==0) Ǧ=п.None;}void Ηέ(){float Εά=0;for(int i=0;i<ѓ.Count;i++){IMyGasTank ΖΡ=ѓ[i];if(!Ǹ(ΖΡ,true)) continue;Εά+=(float) ΖΡ.FilledRatio;}Ȓ=Џ(Εά,ѓ.Count)*100f;}void Ηε(){ħ=0;try{for(int i=0;i<Ă.Count;i++)
{IMyReactor Γό=Ă[i];if(!Ǹ(Γό,true)) continue;List<MyInventoryItem>Ы=new List<MyInventoryItem>();Γό.GetInventory(0).GetItems(Ы);float Ǐ=0;for(int j=0;j<Ы.Count;j++){MyInventoryItem В=Ы[j];if(В.Type.SubtypeId.ToUpper()=="URANIUM"&&В.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǐ+=(float) В.Amount;}if(Ǐ<ħ||i==0){ħ=Ǐ;Γη=Γό.CustomName;}}}catch(Exception e){ȓ=e;}}void ΗΪ(){Ō=0;ļ=0;try{for(int i=0;i<Ļ.Count;i++){IMyTerminalBlock Τ=Ļ[i];if(!Ǹ(Τ,true)) continue;ļ+=(float) Τ.GetInventory(0).CurrentVolume;Ō+=(float) Τ.GetInventory(0).MaxVolume;
}Αέ=(float) Math.Min(Math.Round(Џ(ļ,Ō)*100,1),100.0);}catch(Exception e){ȓ=e;}}void Ηή(){try{Ǖ.Clear();for(int i=0;i<Ļ.Count;i++){IMyTerminalBlock Τ=Ļ[i];if(!Ǹ(Τ,true)) continue;ΔΛ(Τ,И.ŧ);}if(М!=Ѐ.Βά){for(int i=0;i<Ĕ.Count;i++){IMyTerminalBlock Τ=Ĕ[i];if(!Ǹ(Τ,true)) continue;
ΔΛ(Τ,И.ΖЃ);}}ΗΣ(Ǖ);}catch(Exception e){ȓ=e;}}void Ζη(){if(ΑΖ){if(Δβ().Count>ΑΧ){ΑΖ=false;if(л!=е.Ŧ){χ();if(л==е.ΒΖ) č();if(л==е.ƽ)
if(Θ.γ) Ʊ();else č();}Λ="Damage detected";}}}bool ΑЊ(){if(!ΑΨ) return true;if(ί==Μ.Ѕ){if(ц>0&&Ǧ!=п.None)
if(Ǽ<=ц){Λ="Low energy! Move home";return false;}if(Ь>0&&Ă.Count>0)
if(ħ<=Ь){Λ="Low fuel: "+Γη;return false;}if(ч>0&&ѓ.Count>0)
if(Ȓ<=ч){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δβ(){List<IMyTerminalBlock>ę=new List<IMyTerminalBlock>();for(int i=0;i<Ŵ.Count;i++){IMyTerminalBlock Τ=Ŵ[i];if(Ũ(Τ,false)) ę.Add(Τ);}return ę;}bool Ũ(IMyTerminalBlock Τ,bool ΓІ){return(!Ǹ(Τ,ΓІ)||!Τ.IsFunctional);
}bool Ǹ(IMyTerminalBlock Τ,bool ΓІ){if(Τ==null) return false;try{IMyCubeBlock Εβ=Me.CubeGrid.GetCubeBlock(Τ.Position).FatBlock;if(ΓІ) return Εβ==Τ;else return Εβ.GetType()==Τ.GetType();}catch{return false;}}float Ζρ(IMyTerminalBlock Τ,String ы,String ΖΘ,String[]ǩ){float Ȁ=0;for(int i=0;i<Τ.InventoryCount;i++)
{IMyInventory Ń=Τ.GetInventory(i);List<MyInventoryItem>Ы=new List<MyInventoryItem>();Ń.GetItems(Ы);foreach(MyInventoryItem В in Ы){if(ǩ!=null&&(ǩ.Contains(В.Type.TypeId.ToUpper())||ǩ.Contains(В.Type.SubtypeId.ToUpper())))
continue;if((ы==""||В.Type.TypeId.ToUpper()==ы)&&(ΖΘ==""||В.Type.SubtypeId.ToUpper()==ΖΘ))
Ȁ+=(float) В.Amount;}}return Ȁ;}э Ζψ(String φ,String ρ,И Г,bool Ηχ){φ=φ.ToUpper();ρ=ρ.ToUpper();for(int i=0;i<Ǖ.Count;i++){э В=Ǖ[i];if(В.φ.ToUpper()==φ&&В.ρ.ToUpper()==ρ&&(В.ŉ==Г||Г==И.ĉ))
return В;}э έ=null;if(Ηχ){έ=new э(φ,ρ,0,Г);Ǖ.Add(έ);}return έ;}int Ȉ(String φ,String ρ,И Г){return Ȉ(φ,ρ,Г,null);}int Ȉ(String φ,String ρ,И Г,String[]ǩ){int Ȁ=0;φ=φ.ToUpper();ρ=ρ.ToUpper();for(int i=0;i<Ǖ.Count;i++){э В=Ǖ[i];if(ǩ!=null&&ǩ.Contains(В.φ.ToUpper())) continue;
if((φ==""||В.φ.ToUpper()==φ)&&(ρ==""||В.ρ.ToUpper()==ρ)&&(В.ŉ==Г||Г==И.ĉ))
Ȁ+=В.Ǐ;}return Ȁ;}void ΔΛ(IMyTerminalBlock Τ,И Г){for(int i=0;i<Τ.InventoryCount;i++){List<MyInventoryItem>Ы=new List<MyInventoryItem>();Τ.GetInventory(i).GetItems(Ы);for(int j=0;j<Ы.Count;j++)
Ζψ(Ы[j].Type.SubtypeId,Ы[j].Type.TypeId.Replace("MyObjectBuilder_",""),Г,true).Ǐ+=(int) Ы[j].Amount;}}void ΗΣ(List<э>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){э a=Т[j];э b=Т[j+1];if(a.Ǐ<b.Ǐ) Т.Move(j,j+1);}}bool ũ(bool Ιά){if(ȝ&&Η!=Ζ.Ϊ)
if(О!=-1&&Ĥ>=О){Λ="Ship too heavy";return true;}if(Αέ>=Ǭ||Ƙ){Ƙ=false;Λ="Ship is full";return true;}return false;}bool Δν(){String[]ǩ=null;if(!Ȝ) ǩ=new string[]{"ICE"};if(Η==Ζ.Ŭ) return Ȉ("","ORE",И.ŧ,ǩ)==0;if(Η==Ζ.џ) return Ȉ("","COMPONENT",И.ŧ,ǩ)==0;else return Ȉ("","",И.ŧ,ǩ)==0;
}bool ΖЈ(){List<IMyCockpit>ΒЍ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΒЍ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΒЍ.Count;i++)
if(ΒЍ[i].IsUnderControl) return true;return false;}bool Ζι(bool Ȅ,bool Ασ){IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null) return false;if(Vector3.Distance(Θ.Δ,Й.GetPosition())<5)
return ΔΥ(ϋ,Ȅ,Ασ);if(Vector3.Distance(Ε.Δ,Й.GetPosition())<5)
return ΔΥ(ό,Ȅ,Ασ);return false;}bool ΔΥ(ĳ Ǿ,bool Ȅ,bool Ασ){if(Ȅ) Ǿ.ΗΒ();Ǿ.Ηβ();bool н=false;String ъ="";switch(Ǿ.Ћ){case ι.Βή:{ъ="Waiting for command";н=false;break;}case ι.Δυ:{ъ="Waiting for cargo";н=ũ(true);break;}case ι.Δτ:{ъ="Unloading";н=Δν();break;}case ι.Βγ:{н=true;break;}
case ι.Βέ:{ъ="Charging batteries";н=Ǽ>=100f;break;}case ι.ƻ:{ъ="Discharging batteries";н=Ǽ<=25f;break;}case ι.ƺ:{ъ="Discharging batteries";н=Ǽ<=0f;break;}case ι.Βΰ:{ъ="Filling up hydrogen";н=Ȓ>=100f;break;}case ι.Βα:{ъ="Unloading hydrogen";н=Ȓ<=25f;break;}
case ι.Βί:{ъ="Unloading hydrogen";н=Ȓ<=0f;break;}case ι.Ββ:{bool Εμ=ΖЈ();if(!Εμ) Ǿ.Γψ=true;н=Ǿ.Γψ&&Εμ;ъ="Waiting for passengers";break;}}if(!н) Ǿ.ΑЉ(true);if(н&&Ǿ.ΒΠ()){н=Ǿ.ΑЉ(false);ъ="Undocking in: "+ΒΘ((int) Math.Max(0,Ǿ.ǒ-Ǿ.Ȭ));}if(Ασ) Ζή=ъ;return н;}void Ζα()
{if(!ń) return;if(ς.Count<=1) return;float ΖΧ=0,ΖΦ=0;for(int i=0;i<ς.Count;i++){IMyTerminalBlock ǰ=ς[i];if(Ũ(ǰ,true)) continue;ΖΧ+=(float) ǰ.GetInventory(0).MaxVolume;ΖΦ+=(float) ǰ.GetInventory(0).CurrentVolume;}float ΔЎ=(float) Math.Round(Џ(ΖΦ,ΖΧ),5);for(int ΕЉ=0;ΕЉ<Math.Max(1,Math.Floor(ς.Count/10f));ΕЉ++)
{float ΓΧ=0,Γζ=0;float Ει=0,Εό=0;IMyTerminalBlock Ȑ=null,ȭ=null;for(int i=0;i<ς.Count;i++){IMyTerminalBlock ǰ=ς[i];if(Ũ(ǰ,true)) continue;float Γν=(float) ǰ.GetInventory(0).MaxVolume;float ΑΠ=Џ((float) ǰ.GetInventory(0).CurrentVolume,Γν);if(Ȑ==null||ΑΠ>ΓΧ){Ȑ=ǰ;ΓΧ=ΑΠ;Ει=Γν;}
if(ȭ==null||ΑΠ<Γζ){ȭ=ǰ;Γζ=ΑΠ;Εό=Γν;}}if(Ȑ==null||ȭ==null||Ȑ==ȭ) return;if(checkConveyorSystem&&!Ȑ.GetInventory(0).IsConnectedTo(ȭ.GetInventory(0))){if(ΒЂ>20) Λ="Inventory balancing failed";else ΒЂ++;return;}ΒЂ=0;List<MyInventoryItem>Ы=new List<MyInventoryItem>();
Ȑ.GetInventory(0).GetItems(Ы);float ƣ=0;if(Ы.Count==0) continue;MyInventoryItem ΑΡ=Ы[0];String Επ=ΑΡ.Type.TypeId+ΑΡ.Type.SubtypeId;if(!Ζέ.TryGetValue(Επ,out ƣ)){if(ΖЋ(Ȑ.GetInventory(0),0,ȭ.GetInventory(0),out ƣ))
Ζέ.Add(Επ,ƣ);else return;}float Θτ=((ΓΧ-ΔЎ)*Ει/ƣ);float Θυ=((ΔЎ-Γζ)*Εό/ƣ);int ΔΑ=(int) Math.Min(Θυ,Θτ);if(ΔΑ<=0) return;if((float) ΑΡ.Amount<ΔΑ)
Ȑ.GetInventory(0).TransferItemTo(ȭ.GetInventory(0),0,null,null,ΑΡ.Amount);else
Ȑ.GetInventory(0).TransferItemTo(ȭ.GetInventory(0),0,null,null,ΔΑ);}}bool ΖЋ(IMyInventory ƙ,int Β,IMyInventory Ηϊ,out float Ζά){Ζά=0;float Ηξ=(float) ƙ.CurrentVolume;List<MyInventoryItem>ΒЄ=new List<MyInventoryItem>();ƙ.GetItems(ΒЄ);float ΔБ=0;for(int i=0;i<ΒЄ.Count;i++) ΔБ+=(float) ΒЄ[i].Amount;
ƙ.TransferItemTo(Ηϊ,Β,null,null,1);float ΖΫ=Ηξ-(float) ƙ.CurrentVolume;List<MyInventoryItem>Βϊ=new List<MyInventoryItem>();ƙ.GetItems(Βϊ);float ΔЅ=0;for(int i=0;i<Βϊ.Count;i++) ΔЅ+=(float) Βϊ[i].Amount;if(ΖΫ==0f||!ΒΥ(0.9999,ΔБ-ΔЅ,1.0001))
return false;Ζά=ΖΫ;return true;}void Ηά(){Vector3 Ɔ=ω+ą;Vector3 ǯ=Ɔ-ΰ;if(ǯ.Length()==0) ǯ=new Vector3(0,0,-1);Vector3 Αα=б(Φ,ǯ);Vector3 ΖΣ=Vector3.Normalize(Αα);Vector3 ň=б(Φ,Φ.GetNaturalGravity());float Θε=Γυ>0?Math.Max(0,1-ĭ(ǯ,Εή)/5):0;float Ў=(float) Math.Min((Γκ>0?Γκ:1000f),Math.Max(ƪ(-Αα,ň,null),Γυ*Θε));
if(!ΖΥ) Ў=0;if(ΖΒ) Ў=Math.Max(0,1-ΑΔ/ǉ)*Ў;if(generalSpeedLimit>0) Ў=Math.Min(generalSpeedLimit,Ў);if(Œ) Ў=0;if(ǲ>=0) Ў=Math.Min(Ў,ǲ);if(ΖΓ) Ў*=(float) Math.Min(1,Џ(ǯ.Length(),wpReachedDist)/2);Vector3 Εω=б(Φ,Φ.GetShipVelocities().LinearVelocity);float Ηκ=(float)(Math.Max(0,15-ĭ(-ΖΣ,-Εω))/15)*0.85f+0.15f;
ΓЊ+=Math.Sign(Ηκ-ΓЊ)/10f;Vector3 ȩ=ΖΣ*Ў*ΓЊ-(Εω);Vector3 ΒЁ=ΒΝ(ȩ,null);if(Αψ&&Л>0.1f){ȩ.X*=Βπ(ȩ.X,ref Αω.X,1f,ΒЁ.X,20);ȩ.Y*=Βπ(ȩ.Y,ref Αω.Y,1f,ΒЁ.Y,20);ȩ.Z*=Βπ(ȩ.Z,ref Αω.Z,1f,ΒЁ.Z,20);}else Αω=new Vector3(1,1,1);Ɲ=Ĥ*ȩ-ň*Ĥ;ΔΝ(Ɲ,Αψ);Л=Vector3.Distance(ΰ,Ɔ);
}float Βπ(float value,ref float Ф,float њ,float ȕ,float Γλ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*њ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γλ||ȕ==0) return έ;Ф=Math.Min(Γλ,Math.Max(-Γλ,Ф));έ=Math.Abs(ȕ);return έ;
}void ΔЃ(){float щ=90,ў=90,љ=90;float Ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǫ,ѕ,Ə,Γγ;if(Γδ){Ǫ=Vector3.Normalize(ω-ΰ);Ə=б(Φ,Ǫ);ѕ=б(Φ,ΓЈ);щ=ĭ(Ə,new Vector3(0,-1,0))-90;ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);
љ=Ċ(Ə,new Vector3(-1,0,0),Ə.Z);}else{Ǫ=ΓЉ;ѕ=б(Φ,ΓЈ);Ə=б(Φ,ΓЉ);Γγ=б(Φ,ΖΤ);щ=Ċ(ѕ,new Vector3(0,0,1),ѕ.Y);ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);љ=Ċ(Γγ,new Vector3(0,0,1),Γγ.X);}if(ΔЇ&&ƴ()){Vector3 ΓΟ=Φ.GetNaturalGravity();ѕ=б(Φ,ΓΟ);щ=Ċ(ѕ,new Vector3(0,0,1),ѕ.Y);
ў=Ċ(ѕ,new Vector3(-1,0,0),ѕ.Y);}if(!ΒΥ(-45,ў,45)){щ=0;љ=0;}if(!ΒΥ(-45,љ,45)) щ=0;ƿ(ΓΡ,1,(-щ)*Ǜ,(-љ)*Ǜ,(-ў)*Ǜ);ΑΔ=Math.Max(Math.Abs(щ),Math.Max(Math.Abs(ў),Math.Abs(љ)));ΑΩ=ΑΔ<=ǉ;}void Ζε(IMyTerminalBlock Αν){if(Αν==null) return;ǟ=new float[3,2];ћ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȇ.Count;i++){IMyThrust г=Ȇ[i];if(!г.IsFunctional) continue;Vector3 κ=б(Αν,г.WorldMatrix.Backward);float ȫ=г.MaxEffectiveThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ǟ[0,0]+=ȫ;else ǟ[0,1]-=ȫ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ǟ[1,0]+=ȫ;else ǟ[1,1]-=ȫ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ǟ[2,0]+=ȫ;else ǟ[2,1]-=ȫ;String ы=Δζ(г);float[,]ќ=null;if(ћ.ContainsKey(ы)) ќ=ћ[ы];else{ќ=new float[3,2];ћ.Add(ы,ќ);}float ȕ=г.MaxThrust;if(Math.Round(κ.X,2)!=0.0)
if(κ.X>=0) ќ[0,0]+=ȕ;else ќ[0,1]-=ȕ;if(Math.Round(κ.Y,2)!=0.0)
if(κ.Y>=0) ќ[1,0]+=ȕ;else ќ[1,1]-=ȕ;if(Math.Round(κ.Z,2)!=0.0)
if(κ.Z>=0) ќ[2,0]+=ȕ;else ќ[2,1]-=ȕ;}}static String Δζ(IMyThrust г){return г.BlockDefinition.SubtypeId;}Vector3 ΒΨ(Vector3 ё,float[,]Ŋ){return new Vector3(
ё.X>=0?Ŋ[0,0]:Ŋ[0,1],
ё.Y>=0?Ŋ[1,0]:Ŋ[1,1],
ё.Z>=0?Ŋ[2,0]:Ŋ[2,1]);}bool Δε(Χ Ψ,String ы,out float ΓΙ){ΓΙ=0;int Β=ȅ.IndexOf(ы);if(Β==-1||Ψ.ǝ==null||Β>=Ψ.ǝ.Length)
return false;ΓΙ=Ψ.ǝ[Β];if(ΓΙ==-1) return false;return true;}Vector3 ΒΝ(Vector3 ё,Χ Ψ){if(Ψ!=null){Vector3 έ=new Vector3();for(int i=0;i<ћ.Keys.Count;i++){String ы=ћ.Keys.ElementAt(i);float Ƅ=0;if(!Δε(Ψ,ы,out Ƅ))
return ΒΨ(ё,ǟ);έ+=ΒΨ(ё,ћ.Values.ElementAt(i))*Ƅ;}return έ;}return ΒΨ(ё,ǟ);}float ť(Vector3 ё,Χ Ψ){return ť(ё,new Vector3(),Ψ);}float ť(Vector3 ё,Vector3 μ,Χ Ψ){Vector3 Θμ=ΒΝ(ё,Ψ);Vector3 ȫ=Θμ+μ*Ĥ;float Ɣ=(ȫ/ё).AbsMin();return(float)(ё*Ɣ).Length();}float ƪ(Vector3 Αί,Vector3 ň,Χ Ψ)
{if(Αί.Length()==0) return 0;float ΔІ=1;if(ň.Length()>0) ΔІ=Math.Min(1,ĭ(-ň,Αί)/90)*0.4f+0.6f;float Ɲ=ť(Αί,ň,Ψ);if(Ɲ==0) return 0.1f;float ǈ=Џ(Ɲ,Ĥ);float ΓЍ=(float) Math.Sqrt(Џ(Αί.Length(),ǈ*0.5f));return ǈ*ΓЍ*ΔІ*ǥ;}float Ʋ(Vector3 Ǫ,Vector3 Ϗ,Vector3 μ,Χ Ψ){if(μ.Length()==0f) return 0;
Vector3 Αΰ=Đ(Ǫ,Ϗ,Vector3.Normalize(μ));float ǔ=ť(-Αΰ,Ψ);return ǔ/μ.Length();}void ΔΠ(bool ΗЋ){float ѝ=0;float ΓΜ=0.9f;if(ΗЋ){О=-1;Ĩ=0;Αυ=null;if(ί!=Μ.з&&Ε.μ.Length()!=0){ѝ=ΓΜ*Ʋ(Ε.β,Ε.α*-1,Ε.μ,null);if(ѝ<О||О==-1) О=ѝ;}if(Θ.γ&&Θ.μ.Length()!=0){ѝ=ΓΜ*Ʋ(Θ.β,Θ.α*-1,Θ.μ,null);
if(ѝ<О||О==-1) О=ѝ;}return;}if(Ĩ==-1) return;if(Ĩ>=0){int ΕЌ=0;while(Ĩ<Σ.Count){if(ΕЌ>100) return;ΕЌ++;Χ Ψ=Σ[Ĩ];if(Ψ.μ.Length()!=0f){ѝ=ΓΜ*Math.Min(
Ʋ(Ψ.β,Ψ.α*-1,Ψ.μ,Ψ),
Ʋ(Ψ.β*-1,Ψ.α*-1,Ψ.μ,Ψ));if(ѝ<О||О==-1) О=ѝ;}else Αυ=Ψ;Ĩ++;}Ĩ=-1;}bool ΓΣ=true;float ā=0;if(Αυ!=null){for(int i=0;i<ћ.Count;i++){String ы=ћ.Keys.ElementAt(i);float[,]ќ=ћ.Values.ElementAt(i);float Ƅ=0;if(!Δε(Αυ,ы,out Ƅ)){ΓΣ=false;break;}for(int a=0;a<ќ.GetLength(0);a++)
for(int b=0;b<ќ.GetLength(1);b++){float ē=Math.Abs(ќ[a,b]*Ƅ);if(ē==0) continue;ΓΣ=true;if(ā==0||ē<ā) ā=ē;}}}if(!ΓΣ){for(int a=0;a<ǟ.GetLength(0);a++)
for(int b=0;b<ǟ.GetLength(1);b++){float ē=Math.Abs(ǟ[a,b]);if(ē==0) continue;if(ā==0||ē<ā) ā=ē;}}if(ā>0){float ΘΖ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ѝ=Џ(ā,ΘΖ);if(ѝ>0)
if(ѝ<О||О==-1) О=ѝ;}}void ΔΝ(Vector3 ǔ,bool Ї){if(!Ї){for(int i=0;i<Ȇ.Count;i++)
Ȇ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЀ=ΒΝ(ǔ,null);float ΘЃ=Math.Min(1,Math.Abs(Џ(ǔ.X,ΒЀ.X)));float ΘЄ=Math.Min(1,Math.Abs(Џ(ǔ.Y,ΒЀ.Y)));float ΘЅ=Math.Min(1,Math.Abs(Џ(ǔ.Z,ΒЀ.Z)));for(int i=0;i<Ȇ.Count;i++){IMyThrust г=Ȇ[i];Vector3 κ=ΗΔ(б(Φ,г.WorldMatrix.Backward),1);
if(κ.X!=0&&Math.Sign(κ.X)==Math.Sign(ǔ.X))
г.SetValueFloat("Override",г.MaxThrust*ΘЃ);else if(κ.Y!=0&&Math.Sign(κ.Y)==Math.Sign(ǔ.Y))
г.SetValueFloat("Override",г.MaxThrust*ΘЄ);else if(κ.Z!=0&&Math.Sign(κ.Z)==Math.Sign(ǔ.Z))
г.SetValueFloat("Override",г.MaxThrust*ΘЅ);else
г.SetValueFloat("Override",0.0f);}}void ƿ(bool Ї,float ΘΣ,float щ,float љ,float ў){for(int i=0;i<ΑΤ.Count;i++){IMyGyro Є=ΑΤ[i];Є.GyroOverride=Ї;if(!Ї) Є.GyroPower=100;else Є.GyroPower=ΘΣ;if(!Ї) continue;Vector3 Б=Φ.WorldMatrix.Forward;Vector3 Э=Φ.WorldMatrix.Right;Vector3 Ϗ=Φ.WorldMatrix.Up;
Vector3 Ŀ=Є.WorldMatrix.Forward;Vector3 Ł=Є.WorldMatrix.Up;Vector3 ŀ=Є.WorldMatrix.Left*-1;if(Ŀ==Б) Є.SetValueFloat("Roll",ў);else if(Ŀ==(Б*-1)) Є.SetValueFloat("Roll",ў*-1);else if(Ł==(Б*-1)) Є.SetValueFloat("Yaw",ў);else if(Ł==Б) Є.SetValueFloat("Yaw",ў*-1);else if(ŀ==Б) Є.SetValueFloat("Pitch",ў);
else if(ŀ==(Б*-1)) Є.SetValueFloat("Pitch",ў*-1);if(ŀ==(Э*-1)) Є.SetValueFloat("Pitch",щ);else if(ŀ==Э) Є.SetValueFloat("Pitch",щ*-1);else if(Ł==Э) Є.SetValueFloat("Yaw",щ);else if(Ł==(Э*-1)) Є.SetValueFloat("Yaw",щ*-1);else if(Ŀ==(Э*-1)) Є.SetValueFloat("Roll",щ);
else if(Ŀ==Э) Є.SetValueFloat("Roll",щ*-1);if(Ł==(Ϗ*-1)) Є.SetValueFloat("Yaw",љ);else if(Ł==Ϗ) Є.SetValueFloat("Yaw",љ*-1);else if(ŀ==Ϗ) Є.SetValueFloat("Pitch",љ);else if(ŀ==(Ϗ*-1)) Є.SetValueFloat("Pitch",љ*-1);else if(Ŀ==Ϗ) Є.SetValueFloat("Roll",љ);else if(Ŀ==(Ϗ*-1)) Є.SetValueFloat("Roll",љ*-1);
}}void ΔϏ(){this.ΓΡ=false;}void Х(Vector3 Ɓ,Vector3 Б,Vector3 Γα,float ț,bool Ƣ){ΑЂ(Ɓ,ț,Ƣ);ǉ=ț;Γδ=false;this.ΓЉ=Б;this.ΖΤ=Γα;}void Х(Vector3 Ɓ,Vector3 Б,Vector3 Γα,bool Ƣ){Х(Ɓ,Б,Γα,2f,Ƣ);}void ΑЂ(Vector3 Ɓ,float ț,bool Ƣ){ǉ=ț;this.ΓΡ=true;this.ΔЇ=Ƣ;Γδ=true;ΑΩ=false;
this.ΓЈ=Ɓ;}void Βυ(){Ǣ(false,false,false,ω,0);Αψ=false;}void Р(Vector3 Љ,float Ǜ){Ǣ(true,false,false,Љ,Ǜ);}void Ǣ(bool Ї,bool ΓЄ,bool ΔΔ,Vector3 Љ,float Ǜ){Ǣ(Ї,ΓЄ,ΔΔ,Љ,Љ-ΰ,0.0f,Ǜ);}void Ǣ(bool Ї,bool ΓЄ,bool ΔΔ,Vector3 Љ,Vector3 Θί,float Θΰ,float Ǜ){Αψ=true;this.ΖΥ=Ї;
ω=Љ;this.Γκ=Ǜ;this.Γυ=Θΰ;this.ΖΓ=ΓЄ;this.ΖΒ=ΔΔ;this.Εή=Θί;Л=Vector3.Distance(Љ,ΰ);}void ΗΕ(){ϊ.GetBlocksOfType(Ŵ,Ǎ);}void ΗΖ(){if(Αο){ώ=2;return;}List<IMyRemoteControl>ƕ=new List<IMyRemoteControl>();List<IMySensorBlock>ΕБ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΖΚ=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(ƕ,Ǎ);ϊ.GetBlocksOfType(є,Ǎ);ϊ.GetBlocksOfType(ΕБ,Ǎ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΖΚ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);є=ǡ(є,pamTag,true);ΗΝ();ΗΞ(ΖΚ);ů(є,setLCDFontAndSize,1.4f,false);ů(Ą,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΖΞ=ǡ(ΕБ,pamTag,true);if(ΖΞ.Count>0) ξ=ΖΞ[0];else ξ=null;if(Η==Ζ.Ŭ){ϊ.GetBlocksOfType(ς,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ς.Count==0){ώ=1;Λ="Drills are missing";}}else if(Η==Ζ.џ){ϊ.GetBlocksOfType(ς,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ς.Count==0){ώ=1;Λ="Grinders are missing";}if(ξ==null){ώ=1;Λ="Sensor is missing";}}else if(Η==Ζ.Ϊ){ϊ.GetBlocksOfType(ΓЎ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΖΝ=ǡ(ƕ,pamTag,true);if(ΖΝ.Count>0) ƕ=ΖΝ;if(ƕ.Count>0) Φ=ƕ[0];else{Φ=null;ώ=2;Λ="Remote is missing";return;}ζ=(float) Φ.CubeGrid.WorldVolume.Radius*2;ј=null;if(Η!=Ζ.Ϊ){ΔΡ();if(ς.Count>0&&ј!=null){if(ξ!=null&&(ј.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ώ=1;Λ="Wrong sensor direction";}if(ј.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ј.WorldMatrix.Forward!=Φ.WorldMatrix.Down){ώ=2;Λ="Wrong remote direction";}}}}void ΗΗ(){ϊ.GetBlocksOfType(Ȃ,Ǎ);ϊ.GetBlocksOfType(т,Ǎ);ϊ.GetBlocksOfType(Ȇ,Ǎ);ϊ.GetBlocksOfType(ΑΤ,Ǎ);ϊ.GetBlocksOfType(ȧ,Ǎ);ϊ.GetBlocksOfType(Ă,Ǎ);ϊ.GetBlocksOfType(ѓ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(Ατ,Ǎ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
т=ǡ(т,"ConnectorMedium",false);else
т=ǡ(т,"Connector",false);List<IMyShipConnector>ΖΛ=ǡ(т,pamTag,true);if(ΖΛ.Count>0) т=ΖΛ;if(ώ<=1){if(т.Count==0){ώ=1;Λ="Connector is missing";}if(ΑΤ.Count==0){ώ=1;Λ="Gyros are missing";}if(Ȇ.Count==0){ώ=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΖΟ=ǡ(Ατ,pamTag,true);
if(ΖΟ.Count>0) Ατ=ΖΟ;List<IMyLandingGear>ΖΜ=ǡ(Ȃ,pamTag,true);if(ΖΜ.Count>0) Ȃ=ΖΜ;for(int i=0;i<Ȃ.Count;i++) Ȃ[i].AutoLock=false;List<IMyBatteryBlock>ΖΙ=ǡ(ȧ,pamTag,true);if(ΖΙ.Count>0) ȧ=ΖΙ;List<IMyGasTank>ΖΠ=ǡ(ѓ,pamTag,true);if(ΖΠ.Count>0) ѓ=ΖΠ;}void ΗΘ(){ϊ.GetBlocksOfType(Ĕ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ļ.Clear();for(int i=Ĕ.Count-1;i>=0;i--){if(ΖЅ(Ĕ[i])){Ļ.Add(Ĕ[i]);Ĕ.RemoveAt(i);}}}bool ΖЅ(IMyTerminalBlock Τ){if(Τ is IMyCargoContainer) return true;if(Τ is IMyShipDrill) return true;if(Τ is IMyShipGrinder) return true;if(Τ is IMyShipConnector){if(((IMyShipConnector) Τ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΒΡ(Τ,"ConnectorSmall",false))
return false;else return true;}return false;}void ΔΡ(){if(Φ==null) return;ј=null;float Αθ=0,Αγ=0,Αι=0,Αδ=0,Ακ=0,Αε=0;List<IMyTerminalBlock>ΓЇ=ǡ(ς,pamTag,true);bool ΘΝ=ΓЇ.Count==0;if(ΓЇ.Count>0) ј=ΓЇ[0];else if(ς.Count>0) ј=ς[0];int Ȁ=0;for(int i=0;i<ς.Count;i++)
{if(ς[i].WorldMatrix.Forward!=ј.WorldMatrix.Forward){if(ΘΝ){ώ=2;Λ="Mining direction is unclear!";return;}continue;}Ȁ++;Vector3 Ј=ΒΚ(Φ,ς[i].GetPosition());if(i==0){Αθ=Ј.X;Αγ=Ј.X;Αι=Ј.Y;Αδ=Ј.Y;Ακ=Ј.Z;Αε=Ј.Z;}Αγ=Math.Max(Ј.X,Αγ);Αθ=Math.Min(Ј.X,Αθ);Αδ=Math.Max(Ј.Y,Αδ);Αι=Math.Min(Ј.Y,Αι);
Αε=Math.Max(Ј.Z,Αε);Ακ=Math.Min(Ј.Z,Ακ);}Ǳ=(Αγ-Αθ)*(1-ĩ)+drillRadius*2;Ǟ=(Αδ-Αι)*(1-ȯ)+drillRadius*2;if(ј!=null&&ј.WorldMatrix.Forward==Φ.WorldMatrix.Down)
Ǟ=(Αε-Ακ)*(1-ȯ)+drillRadius*2;}List<T>ǡ<T>(List<T>Т,String ǜ,bool ΑΥ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(ΒΡ(Т[i],ǜ,ΑΥ)) έ.Add(Т[i]);return έ;}bool ΒΡ<T>(T Τ,String ǜ,bool ΑΥ){IMyTerminalBlock b=(IMyTerminalBlock) Τ;if(ΑΥ&&b.CustomName.ToUpper().Contains(ǜ.ToUpper())) return true;if(!ΑΥ&&b.BlockDefinition.ToString().ToUpper().Contains(ǜ.ToUpper())) return true;return false;}void я<T>(List<T>Т,bool Ї)
{for(int i=0;i<Т.Count;i++)
Δϊ((IMyTerminalBlock) Т[i],Ї);}void Δϊ(IMyTerminalBlock Τ,bool Ї){if(Τ==null) return;String Ηθ=Ї?"OnOff_On":"OnOff_Off";var a=Τ.GetActionWithName(Ηθ);a.Apply(Τ);}void ǣ(bool Ї){ΓЅ=Ї;if(!ĥ) return;я(Ατ,Ї);}void Βλ(ChargeMode Ɛ){for(int i=0;i<ȧ.Count;i++) ȧ[i].ChargeMode=Ɛ;}void Βν(bool Θθ)
{for(int i=0;i<ѓ.Count;i++) ѓ[i].Stockpile=Θθ;}void Ű(List<IMyLandingGear>ΓΝ,bool Εϊ){for(int i=0;i<ΓΝ.Count;i++){if(Εϊ) ΓΝ[i].Lock();if(!Εϊ) ΓΝ[i].Unlock();}}void Δϋ(bool Ї){if(ΕΜ==Ї) return;List<IMyShipController>ΑΒ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΒ,Ǎ);if(ΑΒ.Count==0) return;
for(int i=0;i<ΑΒ.Count;i++)
ΑΒ[i].DampenersOverride=Ї;ΕΜ=Ї;}IMyShipConnector З(MyShipConnectorStatus ъ){for(int i=0;i<т.Count;i++){if(!Ǹ(т[i],true)) continue;if(т[i].Status==ъ) return т[i];}return null;}void đ(bool ΓЁ,bool ΑΘ,float ΗЂ,float Ην){if(ξ==null||ς.Count==0) return;Vector3 ΑΑ=new Vector3();
int Ȁ=0;for(int i=0;i<ς.Count;i++){if(ς[i].WorldMatrix.Forward!=ј.WorldMatrix.Forward) continue;Ȁ++;ΑΑ+=ς[i].GetPosition();}ΑΑ=ΑΑ/Ȁ;Vector3 ŏ=ΒΚ(ξ,ΑΑ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЁ;ξ.LeftExtend=(ΑΘ?1:й)/2f*Ǳ-ŏ.X;ξ.RightExtend=(ΑΘ?1:й)/2f*Ǳ+ŏ.X;ξ.TopExtend=(ΑΘ?1:и)/2f*Ǟ+ŏ.Y;
ξ.BottomExtend=(ΑΘ?1:и)/2f*Ǟ-ŏ.Y;ξ.FrontExtend=(ΓЁ?Ц:ΗЂ)-ŏ.Z;ξ.BackExtend=ΓЁ?0:Ην+ζ*0.75f+ŏ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ů<T>(List<T>Т,bool Θβ,float ΗЀ,bool ΗЌ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƛ=null;if(Т[i]is IMyTextSurface) ƛ=(IMyTextSurface) Т[i];if(ƛ!=null){ƛ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Θβ) continue;ƛ.Font="Debug";if(ΗЌ) continue;ƛ.FontSize=ΗЀ;}}}bool ƴ(){Vector3D θ;return this.Φ.TryGetPlanetPosition(out θ);}int Ζύ(Vector3 Θκ,int ΖΔ){if(ΖΔ==-1) return 0;double А=-1;int ȏ=-1;for(int i=Σ.Count-1;i>=0;i--){double ύ=Vector3.Distance(Σ[i].Δ,Θκ);if(А==-1||ύ<А){ȏ=i;А=ύ;}
}return Math.Sign(ȏ-ΖΔ);}bool ΖЇ(Vector3 θ){List<Vector3>Е=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Е.Add(Σ[i].Δ);if(Θ.γ&&Σ.Count>=1){Vector3 Ķ=new Vector3();Ƿ(Θ,dockDist*ζ,false,out Ķ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Е.Insert(0,Ķ);Е.Insert(0,Θ.Δ);}
else{Е.Add(Ķ);Е.Add(Θ.Δ);}}if(Η==Ζ.Ϊ){if(Ε.γ&&Σ.Count>=1){Vector3 Βό=new Vector3();Ƿ(Ε,dockDist*ζ,false,out Βό);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Е.Insert(0,Βό);Е.Insert(0,Ε.Δ);}else{Е.Add(Βό);Е.Add(Ε.Δ);}}}else{if(ί!=Μ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Е.Insert(0,Ε.Δ);else Е.Add(Ε.Δ);}int ȏ=-1;double Ǩ=-1;for(int i=0;i<Е.Count;i++){double ύ=Vector3.Distance(Е[i],θ);if(ύ<Ǩ||Ǩ==-1){Ǩ=ύ;ȏ=i;}}if(Е.Count==0) return false;double ΓΓ=Vector3.Distance(Е[ȏ],θ);double ΘΤ=Vector3.Distance(Е[Math.Max(0,ȏ-1)],Е[ȏ])*1.5f;
double ΘΜ=Vector3.Distance(Е[Math.Min(Е.Count-1,ȏ+1)],Е[ȏ])*1.5f;return ΓΓ<ΘΤ||ΓΓ<ΘΜ||ΓΓ<laneOffset*ζ*1.5f;}static float Џ(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΗΔ(Vector3 v,int ΒА){return new Vector3(Math.Round(v.X,ΒА),Math.Round(v.Y,ΒА),Math.Round(v.Z,ΒА));
}float ĭ(Vector3 a,Vector3 b){if(a==b) return 0;float ΗϏ=(a*b).Sum;float Ευ=a.Length();float Εφ=b.Length();if(Ευ==0||Εφ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΗϏ/(Ευ*Εφ)));if(float.IsNaN(έ)) return 0;return έ;}float Ċ(Vector3 v,Vector3 Αν,float Θγ){float Ĵ=ĭ(v,Αν);
if(Θγ>0f) Ĵ*=-1;if(Ĵ>-90f) return Ĵ-90f;else return 180f-(-Ĵ-90f);}double ΔΨ(float Ηψ){return(Math.PI/180)*Ηψ;}bool ΒΥ(double ΘΕ,double value,double Αβ){return(value>=ΘΕ&&value<=Αβ);}Vector3 ΗΨ(IMyTerminalBlock Τ,Vector3 Ј){return Vector3D.Transform(Ј,Τ.WorldMatrix);
}Vector3 ΒΚ(IMyTerminalBlock Τ,Vector3 ΘЀ){return б(Τ,ΘЀ-Τ.GetPosition());}Vector3 б(IMyTerminalBlock Τ,Vector3 ΔΙ){return Vector3D.TransformNormal(ΔΙ,MatrixD.Transpose(Τ.WorldMatrix));}Vector3 Đ(Vector3 Ǫ,Vector3 Ϗ,Vector3 ΔΙ){MatrixD Γι=MatrixD.CreateFromDir(Ǫ,Ϗ);return Vector3D.TransformNormal(ΔΙ,MatrixD.Transpose(Γι));
}Vector3 Βφ(Vector3 Ǫ,Vector3 Ϗ,Vector3 Ј){MatrixD Γι=MatrixD.CreateFromDir(Ǫ,Ϗ);return Vector3D.Transform(Ј,Γι);}String υ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 λ(String s){String[]Д=s.Split('|');return new Vector3(float.Parse(Κ(Д,0)),float.Parse(Κ(Д,1)),float.Parse(Κ(Д,2)));
}String Ηζ(Χ Ψ){String Њ=":";String έ=υ(Ψ.Δ)+Њ+υ(Ψ.β)+Њ+
υ(Ψ.α)+Њ+υ(Ψ.ψ)+Њ+υ(Ψ.μ);for(int i=0;i<Ψ.ǝ.Length;i++)
έ+=Њ+Math.Round(Ψ.ǝ[i],3);return έ;}Χ ΗΤ(String s){String[]Д=s.Split(':');Χ Ψ=new Χ(
λ(Κ(Д,0)),λ(Κ(Д,1)),
λ(Κ(Д,2)),λ(Κ(Д,3)),
λ(Κ(Д,4)));int Β=5;List<float>ΕΪ=new List<float>();while(Β<Д.Length){String Αώ=Κ(Д,Β);float f=0;if(!float.TryParse(Αώ,out f)) break;ΕΪ.Add(f);Β++;}Ψ.ǝ=ΕΪ.ToArray();return Ψ;}System.Text.StringBuilder Ŗ=new System.Text.StringBuilder();void Ρ<T>(T value,bool ΘΛ){if(ΘΛ) Ŗ.Append('\n');Ŗ.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʈ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ǆ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Κ(String[]Ημ,int Β){String Αώ=Ημ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(Αώ)) return "";
return Αώ;}void Save(){if(Αο||Η==Ζ.р){Storage="";return;}Storage=ΑЈ();}String ΑЈ(){Ŗ.Clear();Ŗ.Append(DATAREV+";");Ρ(υ(Αξ),false);Ρ(υ(Θ.β));Ρ(υ(Θ.ψ));Ρ(υ(Θ.α));Ρ(υ(Θ.μ));Ρ(υ(Θ.Δ));Ρ(υ(Θ.Ч));Ρ(Θ.γ);Ρ(υ(Ε.Δ));Ρ(υ(Ε.μ));Ρ(υ(Ε.β));Ρ(υ(Ε.α));Ρ(υ(Ε.ψ));Ρ(υ(Ε.Ч));Ρ(Ε.γ);Ρ(υ(Ǚ));Ρ(υ(Ȗ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) х);Ρ(Ǭ);Ρ(ц);Ρ(Ь);Ρ(ч);Ρ(ǥ);Ρ(ȝ);Ρ(Ȝ);if(Η==Ζ.Ϊ){Ρ((int) ϋ.Ћ);Ρ(ϋ.ǒ);Ρ(ϋ.Ȭ);Ρ(Ʈ(ϋ.Ǔ));Ρ(Ʈ(ϋ.ǖ));Ρ((int) ό.Ћ);Ρ(ό.ǒ);Ρ(ό.Ȭ);Ρ(Ʈ(ό.Ǔ));Ρ(Ʈ(ό.ǖ));}else{Ρ((int) Ț);Ρ((int) л);Ρ((int) М);Ρ((int) у);Ρ((int) Ğ);Ρ(Ć);Ρ(ĥ);Ρ(ń);Ρ(ľ);Ρ(й);Ρ(и);Ρ(Ц);Ρ(Š);Ρ(ş);Ρ(ĩ);Ρ(ȯ);
Ρ(τ);Ρ(Н);Ρ(Щ);Ρ(Ъ);Ρ(Ȕ);Ρ(К);Ρ(Ģ);Ρ(Ġ);}Ρ(";");for(int i=0;i<ȅ.Count;i++)
Ρ((i>0?"|":"")+ȅ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(Ηζ(Σ[i]),i>0);Ρ(";",false);Ρ(Ш+"|"+П,false);Ρ(";",false);Ρ(Ζτ(),false);String ά=Ŗ.ToString();Ŗ.Clear();return ά;}с ΗΓ(){return Ƽ(Storage);}с Ƽ(String ά){if(ά=="") return с.ΖЎ;String[]ģ=ά.Split(';');if(Κ(ģ,0)!=DATAREV) return с.ΒΫ;int Β=0;try{String[]Ι=Κ(ģ,1).Split('\n');
Αξ=λ(Κ(Ι,Β++));Θ.β=λ(Κ(Ι,Β++));Θ.ψ=λ(Κ(Ι,Β++));Θ.α=λ(Κ(Ι,Β++));Θ.μ=λ(Κ(Ι,Β++));Θ.Δ=λ(Κ(Ι,Β++));Θ.Ч=λ(Κ(Ι,Β++));Θ.γ=bool.Parse(Κ(Ι,Β++));Ε.Δ=λ(Κ(Ι,Β++));Ε.μ=λ(Κ(Ι,Β++));Ε.β=λ(Κ(Ι,Β++));Ε.α=λ(Κ(Ι,Β++));Ε.ψ=λ(Κ(Ι,Β++));Ε.Ч=λ(Κ(Ι,Β++));Ε.γ=bool.Parse(Κ(Ι,Β++));
Ǚ=λ(Κ(Ι,Β++));Ȗ=λ(Κ(Ι,Β++));Ι=Κ(ģ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Κ(Ι,Β++));ί=(Μ) int.Parse(Κ(Ι,Β++));х=(Μ) int.Parse(Κ(Ι,Β++));Ǭ=int.Parse(Κ(Ι,Β++));ц=int.Parse(Κ(Ι,Β++));Ь=int.Parse(Κ(Ι,Β++));ч=int.Parse(Κ(Ι,Β++));ǥ=float.Parse(Κ(Ι,Β++));ȝ=bool.Parse(Κ(Ι,Β++));
Ȝ=bool.Parse(Κ(Ι,Β++));if(Η==Ζ.Ϊ){ϋ.Ћ=(ι) int.Parse(Κ(Ι,Β++));ϋ.ǒ=float.Parse(Κ(Ι,Β++));ϋ.Ȭ=float.Parse(Κ(Ι,Β++));ϋ.Ǔ=Ǆ(Κ(Ι,Β++));ϋ.ǖ=Ǆ(Κ(Ι,Β++));ό.Ћ=(ι) int.Parse(Κ(Ι,Β++));ό.ǒ=float.Parse(Κ(Ι,Β++));ό.Ȭ=float.Parse(Κ(Ι,Β++));ό.Ǔ=Ǆ(Κ(Ι,Β++));ό.ǖ=Ǆ(Κ(Ι,Β++));}else{Ț=(Ǥ) int.Parse(Κ(Ι,Β++));
л=(е) int.Parse(Κ(Ι,Β++));М=(Ѐ) int.Parse(Κ(Ι,Β++));у=(ь) int.Parse(Κ(Ι,Β++));Ğ=(Ǥ) int.Parse(Κ(Ι,Β++));Ć=bool.Parse(Κ(Ι,Β++));ĥ=bool.Parse(Κ(Ι,Β++));ń=bool.Parse(Κ(Ι,Β++));ľ=bool.Parse(Κ(Ι,Β++));й=int.Parse(Κ(Ι,Β++));и=int.Parse(Κ(Ι,Β++));Ц=int.Parse(Κ(Ι,Β++));Š=float.Parse(Κ(Ι,Β++));
ş=float.Parse(Κ(Ι,Β++));ĩ=float.Parse(Κ(Ι,Β++));ȯ=float.Parse(Κ(Ι,Β++));τ=int.Parse(Κ(Ι,Β++));Н=int.Parse(Κ(Ι,Β++));Щ=int.Parse(Κ(Ι,Β++));Ъ=int.Parse(Κ(Ι,Β++));Ȕ=int.Parse(Κ(Ι,Β++));К=int.Parse(Κ(Ι,Β++));Ģ=int.Parse(Κ(Ι,Β++));Ġ=float.Parse(Κ(Ι,Β++));}Ι=Κ(ģ,3).Replace("\n","").Split('|');
ȅ=Ι.ToList();Ι=Κ(ģ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(ΗΤ(Κ(Ι,i)));Ι=Κ(ģ,5).Split('|');Ш=Κ(Ι,0);if(!int.TryParse(Κ(Ι,1),out П)) П=-1;if(Ш.Length!=τ*Н){Ш="";П=-1;}Ζσ(Κ(ģ,6));}catch{return с.Δά;}ŗ=х;χ();return с.ǁ;}const int ΔΟ=64000;IMyTerminalBlock Δέ(){List<IMyTerminalBlock>Ĺ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(Ĺ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ĺ.Count>0?Ĺ[0]:null;}void Ζχ(){IMyTerminalBlock Τ=Δέ();if(Τ==null){Λ="Export: name a block "+dataTag;return;}String ά="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΑЈ();if(ά.Length>ΔΟ){Λ="Export too large: "+ά.Length+" chars, max "+ΔΟ;return;}Τ.CustomData=ά;Λ="Exported "+Σ.Count+" waypoints";}void ΖЁ(bool Γέ){IMyTerminalBlock Τ=Δέ();if(Τ==null){Λ="Import: name a block "+dataTag;return;}
ΖЂ(Τ.CustomData,Γέ);}void ΖЂ(String ά,bool Γέ){ά=ά.Replace("\r","");if(ά.StartsWith("//")){int ΕЄ=ά.IndexOf('\n');ά=ΕЄ<0?"":ά.Substring(ΕЄ+1);}ά=ά.Trim();if(ά==""){Λ="Import: no PAM data in "+dataTag;return;}χ();String ΔА=ΑЈ();с έ=Ƽ(ά);if(έ==с.ǁ&&Γέ){Χ ņ=new Χ(Ε);
Vector3 ΘΘ=Ǚ,ΘΗ=Ȗ;int w=й,h=и,d=Ц;Ǥ Θζ=Ț;ь ΒБ=у;float ΘΠ=ĩ,ΘΟ=ȯ;Ƽ(ΔА);Ε=ņ;Ǚ=ΘΘ;Ȗ=ΘΗ;й=w;и=h;Ц=d;Ț=Θζ;у=ΒБ;ĩ=ΘΠ;ȯ=ΘΟ;ί=Μ.İ;Ų(true,true);ί=Μ.İ;х=Μ.İ;Λ="Imported job, run CONT";return;}if(έ==с.ǁ){Ƒ=false;Λ="Imported "+Σ.Count+" waypoints";
return;}Ƽ(ΔА);Λ=έ==с.ΒΫ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΗΝ(){Ž.Clear();for(int i=є.Count-1;i>=0;i--){String ά=є[i].CustomData.ToUpper();bool ΓΫ=false;if(ά=="INSTRUCTIONS"){ΓΫ=true;ΑΗ=true;}if(ά=="DEBUG") ΓΫ=true;if(ΓΫ){Ž.Add(є[i]);є.RemoveAt(i);}
}ů(Ž,false,1,false);}void ΗΞ(List<IMyTerminalBlock>Ĺ){Ą.Clear();for(int i=0;i<Ĺ.Count;i++){IMyTerminalBlock Τ=Ĺ[i];try{String ΕЋ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Τ.CustomName.IndexOf(ΕЋ);int ř=-1;if(Β<0||!int.TryParse(Τ.CustomName.Substring(Β+ΕЋ.Length,1),out ř)) continue;
if(ř==-1) continue;ř--;IMyTextSurfaceProvider ΕЍ=(IMyTextSurfaceProvider) Τ;if(ř<ΕЍ.SurfaceCount&&ř>=0)
Ą.Add(ΕЍ.GetSurface(ř));}catch{}}}String Αζ="";void Δφ(){String Γθ="";String ΓΘ="";Αζ=ΔΞ(false);Γθ+=Αζ;ΓΘ+=Αζ;ΓΘ+=Ζϋ();for(int i=0;i<є.Count;i++) є[i].WriteText(Γθ);for(int i=0;i<Ą.Count;i++) Ą[i].WriteText(Γθ);Echo(ΓΘ);for(int i=0;i<Ž.Count;i++){IMyTextPanel Γτ=Ž[i];
String ά=Γτ.CustomData.ToUpper();if(ά=="DEBUG") Γτ.WriteText(""+"\n"+"");if(ά=="INSTRUCTIONS") Γτ.WriteText(Ζό());}}string Ζό(){String Γ="";try{float Εύ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(ĸ*Εύ)+" / "+(int)(ŋ*Εύ)+"\n";Γ+="Inst. avg/max: "+Math.Round(ĸ*100f,1)+"% / "+Math.Round(ŋ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ŷ,2)+"ms / "+Math.Round(Γμ,2)+"ms \n";}catch{}for(int i=0;i<ȗ.Count;i++)
Γ+=""+ȗ.Keys.ElementAt(i)+" = "+Math.Round(ȗ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(ȗ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΖϏ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ů(є,setLCDFontAndSize,0.9f,false);ů(Ą,setLCDFontAndSize,0.9f,true);for(int i=0;i<є.Count;i++) є[i].WriteText(Γ+ȓ.ToString());for(int i=0;i<Ą.Count;i++) Ą[i].WriteText(Γ+ȓ.ToString());}void Ǻ(){if(!ΑΗ) return;try{Γω=Runtime.CurrentInstructionCount;}catch{}
}void Ƕ(String φ){if(!ΑΗ) return;if(Γω==0) return;try{float Αό=(Runtime.CurrentInstructionCount-Γω)/Runtime.MaxInstructionCount*100;if(!ȗ.ContainsKey(φ))
ȗ.Add(φ,new float[]{Αό,Αό});else{ȗ[φ][0]=Αό;ȗ[φ][1]=Math.Max(Αό,ȗ[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζβ(String Ȯ){if(!ĺ) return;String Њ=""+BROADCAST_SEP;String π=VERSION+Њ;π+=š(ΓЀ)+Њ;
π+=(int) Η+Њ;π+=ΒΗ(ΰ.X)+""+Њ;π+=ΒΗ(ΰ.Y)+Њ;π+=ΒΗ(ΰ.Z)+Њ;if(Η!=Ζ.Ϊ)
π+=ΒΙ(х)+(х==Μ.Ѕ?" "+Math.Round(Ƌ,1)+"%":"")+Њ;else π+=ΒΙ(х)+Њ;π+=š(Λ)+Њ;π+=і+""+Њ;π+=š(Αζ)+Њ;π+=ļ+Њ;π+=Ō+Њ;for(int i=0;i<Ǖ.Count;i++){if(Ǖ[i].ŉ!=И.ŧ) continue;π+=š(Ǖ[i].φ)+"/"+š(Ǖ[i].ρ)+"/"+Ǖ[i].Ǐ+Њ;}ΗΚ(π,Ȯ);}String š(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΗΚ(String π,String Ȯ){ΗΛ(π,BROADCAST_SELF_ID,Ȯ);}void ΗΛ(String π,String Љ,String Ȯ){try{if(!ĺ) return;π=BROADCAST_TAG+BROADCAST_SEP+Δΰ()+BROADCAST_SEP+Љ+BROADCAST_SEP+ΒІ+BROADCAST_SEP+Ȯ+BROADCAST_SEP+π;this.IGC.SendBroadcastMessage(BROADCAST_TAG,π);}catch(Exception e){ȓ=e;}
}String Δΰ(){if(Η!=Ζ.р) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Δπ(String π){return π.StartsWith(BROADCAST_TAG);}bool ţ(ref String ά,out string ΕΫ,Char Њ){int Β=ά.IndexOf(Њ);ΕΫ="";if(Β<0) return false;ΕΫ=ά.Substring(0,Β);ά=ά.Remove(0,Β+1);return true;}bool ΖЏ(ref String π,out String Ɨ,out String Ȯ)
{Ɨ="";Ȯ="";if(!ĺ) return false;String ǜ="";if(!ţ(ref π,out ǜ,BROADCAST_SEP)||!Δπ(ǜ)) return false;if(!ţ(ref π,out Ɨ,BROADCAST_SEP)) return false;if(!ţ(ref π,out ǜ,BROADCAST_SEP)||(ǜ!=Δΰ()&&(ǜ!="*"&&Η!=Ζ.р))) return false;if(!ţ(ref π,out ǜ,BROADCAST_SEP)||(ǜ!=ΒІ)) return false;
if(!ţ(ref π,out Ȯ,BROADCAST_SEP)) return false;return true;}void ΖЄ(){bool Ż=true;if(Εέ){Εέ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ƈ=(Η!=Ζ.р?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ƈ)
Δλ(Ƈ);String[]Ι=Me.CustomData.Split('\n');ĺ=bool.Parse(Βε(ΒΕ(Ι,"Enable_Broadcast",ref Ż)));bool ΔΗ=false;bool ΕΠ=true;if(ĺ){if(Η!=Ζ.р)
ΓЀ=Βε(ΒΕ(Ι,"Ship_Name",ref Ż)).Replace(BROADCAST_SELF_ID,"");ΒІ=Βε(ΒΕ(Ι,"Broadcast_Channel",ref Ż)).ToLower();ΕΠ=false;if(Ȏ==null){Ȏ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ȏ.SetMessageCallback("");}List<IMyRadioAntenna>ĵ=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(ĵ);bool Βϋ=false;
for(int i=0;i<ĵ.Count;i++)
if(ĵ[i].EnableBroadcasting&&ĵ[i].Enabled){Βϋ=true;break;}if(ĵ.Count==0) Λ="No Antenna found";else if(!Βϋ) Λ="Antenna not ready";ΔΗ=ĵ.Count==0||!Βϋ;if(ΔЈ&&!ΔΗ&&Η!=Ζ.р) Λ="Antenna ok";}else if(Η==Ζ.р)
Λ="Offline - Enable in PB custom data";ΔЈ=ΔΗ;if(ΕΠ){if(Ȏ!=null) this.IGC.DisableBroadcastListener(Ȏ);Ȏ=null;}}catch{Ż=false;}if(!Ż){Λ="Reset custom data";Δλ(Ƈ);}}void Δλ(String Ƈ){String ά=Ƈ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.р?"true":"false")+" \n";ά+=Η!=Ζ.р?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=ά+"Broadcast_Channel=#default";}String Βε(String ğ){int Β=ğ.IndexOf("//");if(Β!=-1) ğ=ğ.Substring(0,Β);String[]Д=ğ.Split('=');if(Д.Length<=1) return "";return Д[1].Trim();}String ΒΕ(String[]Ι,String Ň,ref bool ƒ){foreach(String ğ in Ι)
if(ğ.StartsWith(Ň)) return ğ;ƒ=false;return "";}String Δω(int ΔΕ,String Γ,int ΕА,ref int Ș){String[]Ι=Γ.Split('\n');if(ΕА>=Ș+ΔΕ-1) Ș++;Ș=Math.Min(Ι.Count()-1-ΔΕ,Ș);if(ΕА<Ș+1) Ș--;Ș=Math.Max(0,Ș);String έ="";for(int i=0;i<ΔΕ;i++){int Εχ=i+Ș;if(Εχ>=Ι.Count()) break;
έ+=Ι[Εχ]+"\n";}return έ;}String ΒΘ(int Ɩ){if(Ɩ>=60*60) return Math.Round(Ɩ/(60f*60f),1)+" h";if(Ɩ>=60) return Math.Round(Ɩ/60f,1)+" min";return ""+Ɩ+" s";}string ΒΗ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>ǽ=new List<IMyCameraBlock>();List<long>Ő=new List<long>();
bool Œ=false;int ΒЌ=0;int ΒЊ=0;DateTime ΕΒ=DateTime.Now;String ΒЅ="";void ΗΙ(){ǽ.Clear();Ő.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(ǽ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<ǽ.Count;i++){ǽ[i].EnableRaycast=true;if(!Ő.Contains(ǽ[i].CubeGrid.EntityId)) Ő.Add(ǽ[i].CubeGrid.EntityId);
}if(!Ő.Contains(Me.CubeGrid.EntityId)) Ő.Add(Me.CubeGrid.EntityId);}bool Δξ(Α s){return s==Α.ȣ||s==Α.Я||s==Α.І||
s==Α.Ȣ||s==Α.Į||s==Α.Ť||s==Α.ư||
(s==Α.Ǡ&&ї!=null);}void Ƭ(){Œ=false;ΒЌ=0;ΒЅ="";}void Ηΰ(){if(!obstacleBrake||ǽ.Count==0||!Δξ(Υ)){Ƭ();return;}Vector3 Ɔ=ω+ą;Vector3 ǯ=Ɔ-ΰ;float Ǯ=ǯ.Length();if(Ǯ<1f){Ƭ();return;}Vector3 κ=ǯ/Ǯ;float Εϋ=ș*obstacleLookAhead+ζ;Vector3 ΘΧ=Ɔ+κ*(ζ*0.25f);
for(int n=0;n<ǽ.Count;n++){ΒЊ=(ΒЊ+1)%ǽ.Count;IMyCameraBlock ź=ǽ[ΒЊ];if(!ź.IsFunctional||!ź.IsWorking) continue;Vector3 ΕΔ=ź.GetPosition();Vector3 ΑΞ=ΘΧ;if(Vector3.Distance(ΕΔ,ΑΞ)>Εϋ) ΑΞ=ΕΔ+κ*Εϋ;if(!ź.CanScan(ΑΞ)) continue;MyDetectedEntityInfo Ɖ=ź.Raycast(ΑΞ);bool ΘΞ=!Ɖ.IsEmpty()&&!Ő.Contains(Ɖ.EntityId)&&!ΖЊ(Ɖ.EntityId)&&
Ɖ.Type!=MyDetectedEntityType.FloatingObject;if(ΘΞ){if(!Œ) ΕΒ=DateTime.Now;Œ=true;ΒЌ=0;ΒЅ=Ɖ.Type.ToString();}else if(Œ&&++ΒЌ>=3) Ƭ();break;}if(Œ&&(DateTime.Now-ΕΒ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΒЅ;}const String ΔЂ="[PAMTFC]";const float ΗΦ=5f;const float ΒΔ=2f;class ȥ{public long ν;public Vector3 θ,Ş;public Vector3 ƈ;public float ΓΒ;public String ņ="";public int Ɗ=-1;public bool Γϊ;public long Ϋ=0;public int ſ=0;public DateTime ΓЌ;}IMyBroadcastListener Αϋ=null;
long ǚ=0;List<ȥ>м=new List<ȥ>();Vector3 ą=new Vector3();float ǲ=-1;Vector3 ΓА=new Vector3();bool Αϊ=false;DateTime ΓБ=DateTime.Now;const float ΔЁ=15f;const float ΗΥ=10f;int ΖΩ=0;String Ш="";int П=-1;bool ΑΓ=false;Vector3 ΕΘ=new Vector3();String ΒΧ(){if(ί==Μ.з||Η==Ζ.Ϊ) return "";
return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+τ+"|"+Н+"|"+(int) Ğ+"|"+Math.Round(Ǳ,1)+"|"+Math.Round(Ǟ,1);}void Ηγ(){if(!trafficEnabled) return;if(Αϋ==null) Αϋ=IGC.RegisterBroadcastListener(ΔЂ);ǚ=Me.CubeGrid.EntityId;int ΒЈ=30;while(Αϋ.HasPendingMessage&&ΒЈ-->0){String ά=Αϋ.AcceptMessage().Data as String;if(ά!=null) ΖБ(ά);
}for(int i=м.Count-1;i>=0;i--)
if((DateTime.Now-м[i].ΓЌ).TotalSeconds>ΗΦ) м.RemoveAt(i);if(++ΖΩ<3) return;if(Υ==Α.а&&З(MyShipConnectorStatus.Connected)==null) return;ΖΩ=0;IGC.SendBroadcastMessage(ΔЂ,ǚ+";"+υ(ΰ)+";"+
υ(Φ.GetShipVelocities().LinearVelocity)+";"+ζ+";"+ΒΧ()+";"+
(ί==Μ.Ѕ?П:-1)+";"+Ш+";"+Ζλ()+","+ΔΪ()+";"+υ(ΓА)+
";"+(Ľ?1:0));}void ΖБ(String ά){try{String[]f=ά.Split(';');if(f.Length<7) return;long ν=long.Parse(f[0]);if(ν==ǚ) return;ȥ Ж=null;for(int i=0;i<м.Count;i++) if(м[i].ν==ν) Ж=м[i];if(Ж==null){Ж=new ȥ();Ж.ν=ν;м.Add(Ж);}Ж.θ=λ(f[1]);Ж.Ş=λ(f[2]);Ж.ΓΒ=float.Parse(f[3]);
Ж.ņ=f[4];Ж.Ɗ=int.Parse(f[5]);Ж.ΓЌ=DateTime.Now;if(Ж.ņ!=""&&Ж.ņ==ΒΧ()) ΖЌ(f[6]);Ж.Ϋ=0;Ж.ſ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ж.Ϋ=long.Parse(d[0]);Ж.ſ=int.Parse(d[1]);}Ж.ƈ=f.Length>=9?λ(f[8]):new Vector3();Ж.Γϊ=f.Length>=10&&f[9]=="1";}catch{}}bool ΖЊ(long ν)
{for(int i=0;i<м.Count;i++) if(м[i].ν==ν) return true;return false;}Vector3 Δδ(int Β,int ΘΡ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Χ Ψ=Σ[Β];Vector3 Θψ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*ΘΡ;Vector3 Ϗ=Ψ.μ.LengthSquared()>0.01f?-Vector3.Normalize(Ψ.μ):-Ψ.α;
Vector3 Э=Vector3.Cross(Θψ,Ϗ);if(Э.LengthSquared()<0.0001f) return new Vector3();float ύ=laneOffset*ζ;float ΗЁ=Math.Min(Vector3.Distance(Ψ.Δ,Σ[0].Δ),
Vector3.Distance(Ψ.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Э)*ύ*Math.Min(1,ΗЁ/(ύ*4));}Vector3 ΖΨ=new Vector3();bool ΖЉ(Α s){return s==Α.Ǡ||s==Α.Ĉ||s==Α.Č||s==Α.Ů||
s==Α.ǎ||s==Α.ȋ||s==Α.Ȟ;}void Ηδ(){ą=new Vector3();ǲ=-1;ΓА=new Vector3();bool Εΰ=Δξ(Υ);if(!trafficEnabled||(!Εΰ&&!ΖЉ(Υ))){Αϊ=false;return;}ΖΨ=Φ.GetShipVelocities().LinearVelocity;Ζμ(Φ.GetNaturalGravity(),!Εΰ);}void Ζμ(Vector3 μ,bool ΕЊ=false){ą=new Vector3();ǲ=-1;
Vector3 ǯ=ω-ΰ;float Ǯ=ǯ.Length();bool ΓΥ=Ǯ>=1f;Vector3 κ=ΓΥ?ǯ/Ǯ:new Vector3();ΓА=κ;Vector3 Ϗ=μ.LengthSquared()>0.01f?-Vector3.Normalize(μ):-Ƃ;Vector3 Э=Vector3.Cross(κ,Ϗ);Э=Э.LengthSquared()<0.0001f?-Ǝ:Vector3.Normalize(Э);ȥ Αά=null;for(int i=0;i<м.Count;i++)
{ȥ δ=м[i];if(δ.ſ==3&&δ.Ş.LengthSquared()<0.25f) continue;Vector3 θ=δ.θ+δ.Ş*(float)(DateTime.Now-δ.ΓЌ).TotalSeconds;Vector3 œ=θ-ΰ;float Ǒ=(ζ+δ.ΓΒ)*0.5f;float ŵ=ΓΥ?Vector3.Dot(œ,κ):0;if(ŵ>0&&(œ-κ*ŵ).Length()<=Ǒ*1.2f){float Αλ=Vector3.Dot(δ.Ş,κ);bool Εη=δ.ƈ.LengthSquared()>0.25f;
bool ΓΦ=Αλ<-1f||(Εη&&Vector3.Dot(δ.ƈ,κ)<-0.5f);if(ΕЊ){if(ΓΦ&&ǚ<δ.ν) continue;}else{if(δ.Γϊ&&!Ľ){ą=Э*Ǒ*1.3f;continue;}if(ΓΦ){if(ŵ<(ș-Αλ)*6f+Ǒ*4f) ą=Э*Ǒ*1.3f;continue;}if(Εη&&ǚ<δ.ν){Vector3 ΔЏ=-œ;float ΕЇ=Vector3.Dot(ΔЏ,δ.ƈ);if(ΕЇ>0&&(ΔЏ-δ.ƈ*ΕЇ).Length()<=Ǒ*1.2f)
{ą=Э*Ǒ*1.3f;continue;}}}float Εα=ŵ-Ǒ-followGap*ζ;float Γβ=Math.Max(0,Αλ)+Math.Max(0,Εα)/ΒΔ;if(ΓΦ) Γβ=Math.Max(0,Εα+Math.Min(0,Αλ)*ΒΔ*2)/ΒΔ;if(ǲ<0||Γβ<ǲ){ǲ=Γβ;Αά=δ;}continue;}if(δ.Ş.LengthSquared()<1f) continue;if(ΓΥ&&(œ-κ*ŵ).Length()<=Ǒ*1.2f) continue;
Vector3 ΓϏ=δ.Ş-ΖΨ;float ΕЎ=ΓϏ.LengthSquared();if(ΕЎ<0.25f) continue;float t=-Vector3.Dot(œ,ΓϏ)/ΕЎ;if(t<=0||t>5f||(œ+ΓϏ*t).Length()>=Ǒ*1.1f) continue;if(ǚ>δ.ν||t<2f){ǲ=0;Αά=δ;}}bool ΗЈ=ǲ>=0&&ǲ<0.5f;if(!ΗЈ||ΕЊ||(Αά!=null&&Αά.Γϊ&&Ľ)){Αϊ=false;return;}
if(!Αϊ){Αϊ=true;ΓБ=DateTime.Now;}double Εθ=(DateTime.Now-ΓБ).TotalSeconds;if(Εθ>ΔЁ+ΗΥ) ΓБ=DateTime.Now;else if(Εθ>ΔЁ){ǲ=-1;ą=Э*ζ*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ǫ=new List<int>();String Εκ="";void ΒΓ(){String Ň=τ+"|"+Н+"|"+(int) Ğ;
if(Ň==Εκ) return;Εκ=Ň;ǫ.Clear();int n=τ*Н;if(Ğ==Ǥ.ǂ){for(int k=0;k<n;k++){int Ŕ=k/τ;ǫ.Add(Ŕ*τ+(Ŕ%2==0?k-Ŕ*τ:τ-1-(k-Ŕ*τ)));}return;}int Εζ=(int) Math.Ceiling(τ/2f),Εε=(int) Math.Ceiling(Н/2f);int ΗЄ=τ/2,ΗЃ=Н/2;int x=0,y=0,ΓΖ=0,ΓΗ=-1,Αβ=Math.Max(τ,Н);
for(int i=0;i<Αβ*Αβ;i++){if(-Εζ<x&&x<=ΗЄ&&-Εε<y&&y<=ΗЃ) ǫ.Add((y-1+Εε)*τ+(x-1+Εζ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΖ;ΓΖ=-ΓΗ;ΓΗ=t;}x+=ΓΖ;y+=ΓΗ;}}bool ΒΒ(){int n=Math.Max(0,τ*Н);if(Ш.Length==n) return false;Ш=new String('0',n);
П=-1;return true;}void ΖЌ(String δ){ΒΒ();if(δ.Length!=Ш.Length) return;char[]h=Ш.ToCharArray();for(int i=0;i<h.Length;i++) if(δ[i]=='1') h[i]='1';Ш=new String(h);}bool Δο(int ǧ){return ǧ>=0&&ǧ<Ш.Length&&Ш[ǧ]=='1';}void Βμ(int ǧ,bool ђ){if(ǧ<0||ǧ>=Ш.Length) return;
char[]h=Ш.ToCharArray();h[ǧ]=ђ?'1':'0';Ш=new String(h);}bool ΒΣ(int ǧ,bool ΗЎ){if(!trafficEnabled) return false;String Ň=ΒΧ();for(int i=0;i<м.Count;i++){ȥ t=м[i];if(t.ņ!=Ň||t.Ɗ<0||(ΗЎ&&t.ν>ǚ)) continue;float ΕΝ=Math.Abs(ǧ%τ-t.Ɗ%τ)*Ǳ;float ΕΨ=Math.Abs(ǧ/τ-t.Ɗ/τ)*Ǟ;
if(t.Ɗ==ǧ||Math.Sqrt(ΕΝ*ΕΝ+ΕΨ*ΕΨ)<Math.Max(ζ,t.ΓΒ)*1.1f) return true;}return false;}int Ζκ(int Β){int n=τ*Н;if(n<1) return Β;ΒΓ();if(ΒΒ()) for(int k=0;k<Β&&k<n;k++) Βμ(ǫ[k],true);if(П>=0&&(Β>=n||ǫ[Β]!=П)){Βμ(П,true);П=-1;}if(П>=0&&!Δο(П)&&!ΒΣ(П,true)) return Β;
П=-1;bool ΔЉ=false;for(int k=0;k<n;k++){int c=ǫ[k];if(Δο(c)) continue;ΔЉ=true;if(ΒΣ(c,false)) continue;П=c;return k;}return ΔЉ?-1:n;}Ĳ ΗΜ(int Β){ΒΓ();if(Β<0||Β>=ǫ.Count) return Ĳ.ΔΜ;Щ=ǫ[Β]%τ;Ъ=ǫ[Β]/τ;return Ĳ.Δή;}class ю{public long ν;public Vector3 θ,Б,Ϗ;}List<ю>Ѝ=new List<ю>();
bool Ȫ=false;Vector3 ΑΛ,ΑΚ,ΑΜ,ΑΝ;long ф=0;long ȃ=0;Χ ї=null;bool ƀ=false;Χ ΑА(Vector3 θ,Vector3 Б,Vector3 Ϗ){MatrixD m=MatrixD.CreateWorld(θ,Б,Ϗ);Χ w=new Χ(Θ);w.β=Vector3D.TransformNormal(ΑΛ,m);w.α=Vector3D.TransformNormal(ΑΚ,m);w.ψ=Vector3D.TransformNormal(ΑΜ,m);w.Δ=θ+(Vector3) Vector3D.TransformNormal(ΑΝ,m);
w.γ=true;return w;}int ΔΪ(){Α s=Υ;if(s==Α.а) return З(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Č||s==Α.ǻ||s==Α.ċ||s==Α.Ǉ||
s==Α.Ư||s==Α.ǎ) return 3;if(s==Α.Ǡ||s==Α.Ĉ||s==Α.Ů) return 2;if(s==Α.І) return 1;return 0;}long Ζλ(){if(ΔΪ()==0) return 0;if(Ȥ(1)&&Ř!=0) return Ř;return ї!=null?ȃ:ф;}void Δρ(){if(Η==Ζ.Ϊ||!Θ.γ) return;IMyShipConnector Й=З(MyShipConnectorStatus.Connected);if(Й==null||Й.OtherConnector==null) return;
IMyShipConnector δ=Й.OtherConnector;Vector3I ΗЉ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(Й.Position==ΗЉ){MatrixD Ń=MatrixD.Transpose(δ.WorldMatrix);ΑΛ=Vector3D.TransformNormal(Φ.WorldMatrix.Forward,Ń);ΑΚ=Vector3D.TransformNormal(Φ.WorldMatrix.Down,Ń);ΑΜ=Vector3D.TransformNormal(Φ.WorldMatrix.Left,Ń);
ΑΝ=Vector3D.TransformNormal(Й.GetPosition()-δ.GetPosition(),Ń);Ȫ=true;}if(Vector3.Distance(Й.GetPosition(),Θ.Δ)<5) ф=δ.EntityId;List<IMyShipConnector>ě=new List<IMyShipConnector>();ϊ.GetBlocksOfType(ě,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(ě.Count>0){Ѝ.Clear();for(int i=0;i<ě.Count;i++){ю p=new ю();p.ν=ě[i].EntityId;p.θ=ě[i].GetPosition();p.Б=ě[i].WorldMatrix.Forward;p.Ϗ=ě[i].WorldMatrix.Up;Ѝ.Add(p);}}ї=null;ȃ=0;if(δ.EntityId==ф||!Ȫ) return;for(int i=0;i<Ѝ.Count;i++)
if(Ѝ[i].ν==δ.EntityId){ȃ=Ѝ[i].ν;ї=ΑА(Ѝ[i].θ,Ѝ[i].Б,Ѝ[i].Ϗ);}}bool ΒΦ(long ν,int ΕЂ){if(ν==0) return false;for(int i=0;i<м.Count;i++){ȥ t=м[i];if(t.Ϋ==ν&&(t.ſ>ΕЂ||(t.ſ==ΕЂ&&t.ν<ǚ))) return true;}return false;}bool ΑЋ(int Γχ){if(Η==Ζ.Ϊ||!trafficEnabled){ї=null;ȃ=0;return true;}
long η=ї!=null?ȃ:ф;if(!ΒΦ(η,Γχ)) return true;if(!ΒΦ(ф,Γχ)){ї=null;ȃ=0;return true;}if(!Ȫ) return false;ю Ė=null;float ΕΑ=0;for(int i=0;i<Ѝ.Count;i++){ю p=Ѝ[i];if(p.ν==ф||ΒΦ(p.ν,Γχ)) continue;float d=Vector3.Distance(p.θ,Θ.Δ);if(Ė==null||d<ΕΑ){Ė=p;ΕΑ=d;}
}if(Ė==null) return false;ȃ=Ė.ν;ї=ΑА(Ė.θ,Ė.Б,Ė.Ϗ);return true;}String Ζτ(){String s=(Ȫ?"1":"0")+":"+υ(ΑΛ)+":"+υ(ΑΚ)+":"+
υ(ΑΜ)+":"+υ(ΑΝ)+":"+ф;for(int i=0;i<Ѝ.Count;i++)
s+=":"+Ѝ[i].ν+","+υ(Ѝ[i].θ)+","+υ(Ѝ[i].Б)+","+υ(Ѝ[i].Ϗ);return s;}void Ζσ(String s){Ѝ.Clear();Ȫ=false;ф=0;String[]f=s.Split(':');if(f.Length<6) return;Ȫ=f[0]=="1";ΑΛ=λ(f[1]);ΑΚ=λ(f[2]);ΑΜ=λ(f[3]);ΑΝ=λ(f[4]);long.TryParse(f[5],out ф);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ю p=new ю();if(!long.TryParse(e[0],out p.ν)) continue;p.θ=λ(e[1]);p.Б=λ(e[2]);p.Ϗ=λ(e[3]);Ѝ.Add(p);}}const String ΑЍ="[PAMDCK]";const float ΘІ=120f;class ΔΫ{public long Ж;public int ƌ;public long ΓΨ;public bool ΙΓ;public DateTime ΙΦ,ΙΩ;
}IMyBroadcastListener ΙΣ=null;List<ΔΫ>ΙΤ=new List<ΔΫ>();List<IMyShipConnector>Ιί=new List<IMyShipConnector>();List<IMySensorBlock>Ιη=new List<IMySensorBlock>();List<IMyCameraBlock>ΙΨ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Ιζ=new List<MyDetectedEntityInfo>();long ΙΖ=0;
int ΙΕ=0;long ΙΔ=0;DateTime ΙΗ=DateTime.Now;String ΙΡ="";int ΙΥ=0;IMyCubeGrid Ιΰ=null;IMyBroadcastListener Αφ=null;int ȁ=0;DateTime ΕΤ=DateTime.Now;DateTime ΖΕ=DateTime.MinValue;long ΖΖ=0;int ΖΗ=0;long Ř=0;bool Ƥ=false;int ΓΕ=0;List<Vector3>Ʀ=new List<Vector3>();List<float>ΔΚ=new List<float>();
bool Ľ=false;bool ΑΙ=false;Vector3 ΕΣ=new Vector3();bool ű(){return ȁ!=0&&(DateTime.Now-ΖΕ).TotalSeconds<3;}bool Ȥ(int ƌ){return ű()&&ΖΖ==ǚ&&ΖΗ==ƌ;}bool Δύ(){return ű()||(DateTime.Now-ΕΤ).TotalSeconds<3;}void Βκ(int ƌ){if(ȁ==ƌ) return;ȁ=ƌ;ΕΤ=DateTime.Now;ΓΕ=3;
}void Βι(){if(ȁ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΑЍ,"D;"+ǚ);ȁ=0;Ƥ=false;}void ΗΫ(){if(!trafficEnabled||Η==Ζ.Ϊ) return;if(Αφ==null) Αφ=IGC.RegisterBroadcastListener(ΑЍ);int ΒЈ=20;while(Αφ.HasPendingMessage&&ΒЈ-->0){String ά=Αφ.AcceptMessage().Data as String;if(ά==null||!ά.StartsWith("S;")) continue;
try{ΖА(ά);}catch{}}if(Υ!=Α.І){Ľ=false;ΑΙ=false;}if(ȁ==1&&З(MyShipConnectorStatus.Connected)!=null) Βι();if(ȁ==2&&!Ƥ&&Υ!=Α.ǎ&&
Vector3.Distance(ΰ,ж().Δ)>ζ*3) Βι();if(ȁ!=0&&++ΓΕ>=3){ΓΕ=0;IGC.SendBroadcastMessage(ΑЍ,"R;"+ǚ+";"+ȁ+";"+ф+";"+(Ȫ?1:0));}}void ΖА(String ά){String[]f=ά.Split(';');if(f.Length<7) return;bool Εί=false;String[]ΔЄ=f[5].Split(',');for(int i=0;i<ΔЄ.Length;i++) if(ΔЄ[i]==ǚ.ToString()) Εί=true;
if(!Εί) return;ΖΕ=DateTime.Now;ΖΖ=long.Parse(f[2]);ΖΗ=int.Parse(f[3]);Ř=long.Parse(f[4]);Ʀ.Clear();ΔΚ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʀ.Add(λ(e[0]));ΔΚ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΕΦ=f[6].Split(':');List<ю>Т=new List<ю>();for(int i=0;i<ΕΦ.Length;i++){String[]e=ΕΦ[i].Split(',');if(e.Length<4) continue;ю p=new ю();p.ν=long.Parse(e[0]);p.θ=λ(e[1]);p.Б=λ(e[2]);p.Ϗ=λ(e[3]);Т.Add(p);}Ѝ=Т;}bool Ζθ(){if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return true;
Βκ(1);if(!Δύ()) return true;if(!Ȥ(1)){Λ="Waiting for dock clearance";return false;}ΑЃ();return true;}bool ΑЃ(){ƀ=false;ю ġ=null;if(Ř!=0&&Ř!=ф&&Ȫ)
for(int i=0;i<Ѝ.Count;i++) if(Ѝ[i].ν==Ř) ġ=Ѝ[i];long ΔΖ=ġ==null?0:ġ.ν;if(ΔΖ==ȃ&&(ΔΖ==0)==(ї==null)) return false;ȃ=ΔΖ;ї=ġ==null?null:ΑА(ġ.θ,ġ.Б,ġ.Ϗ);return true;}bool į(Vector3 p){for(int i=0;i<Ʀ.Count;i++) if(Vector3.Distance(p,Ʀ[i])<ΔΚ[i]+ζ*0.75f) return true;
return false;}bool ΖЀ(Vector3 ż,Vector3 Ŏ){Ľ=false;if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return false;if(ȁ!=1&&Vector3.Distance(ΰ,Θ.Δ)>dockRequestDistance) return false;Βκ(1);if(!ű()||Ȥ(1)||Ʀ.Count==0) return false;if(!į(ż)&&!į(Ŏ)&&!į(ΰ)) return false;Ľ=true;return true;
}bool ΔΤ(){if(!trafficEnabled||Η==Ζ.Ϊ||ф==0) return true;Βκ(2);if(!Δύ()) return true;if(!Ȥ(2)){Λ="Waiting for undock clearance";return false;}return true;}void Ζπ(string ѐ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}