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
public enum Κ{ĩ,ƹ,ǫ,ј,ή}public enum Α{ь,Ć,ŵ,Ƞ,ħ,И,у,
Ħ,Ű,ȁ,Ń,Ŀ,Ģ,
ƻ,Ȥ,Ǯ,ł,ΒΛ,Αύ,
ΔΘ,ΒΚ,Ʊ,ΑϏ,Ī}public enum Π{э,Ŷ,В,Γά,Αψ,ΑЊ,ΑЋ,ū,Ŭ}public enum ї{None,ł,ь,ΕЈ}public enum Σ{Main,Ζδ,Ƹ,Ʒ,ΔΒ,Αφ,
ΓΛ,ΖΝ,ΖΞ,ΖΙ,ƽ,Ζι,
ΒΓ,ΓЊ,ΓЎ,Ζλ,ΒΕ}public enum х{ΒΒ,Ư,Ιο,ΖΡ,ή}public enum ю{ƿ,ΕЁ}public enum ъ{ΒΑ,Γβ,Ζξ,Ƴ}public enum Г{Γϊ,ΕЅ,ΓΧ,Γέ,Αϋ,Γμ,ΑЅ}public enum Ǖ{ΓΩ,ΓΠ,ΓΡ}public enum Щ{ų,ΙΝ,ŀ}public enum ρ{Γό,ΓЀ,ΓЁ,Ζί,Ζή,
Γϋ,ΑЏ,ΑЎ,Γώ,
ΓϏ,Γύ}public enum ε{ț,ĉ,Ț,Αυ,ΓΞ,ΓΟ}public enum ǭ{ΑЂ,Ατ,ΘЅ}public enum љ{ΒΖ,ΕЊ,ΙΨ,Γω}public enum Њ{None,ŷ,ƺ,ΑЍ}public enum ϋ{ǁ,Ασ,ΑЈ,Ƴ,Γχ,ΑЉ,Γψ,Ιφ,ΓЂ}public enum С{ĩ,ΖΫ,ΖΪ,Γτ,Γξ,ΓΨ,Ζά}class ά{public bool λ=false;public Vector3 Ε=new Vector3();
public Vector3 θ=new Vector3();public Vector3 ι=new Vector3();public Vector3 Ё=new Vector3();public Vector3 φ=new Vector3();public Vector3 о=new Vector3();public float Ч=0;public float Λκ=0;public float[]Ǿ=null;public ά(){}public ά(ά ζ){λ=ζ.λ;Ε=ζ.Ε;θ=ζ.θ;ι=ζ.ι;Ё=ζ.Ё;φ=ζ.φ;
о=ζ.о;Ǿ=ζ.Ǿ;}public ά(Vector3 Ε,Vector3 ι,Vector3 θ,Vector3 Ё,Vector3 φ){this.Ε=Ε;this.θ=θ;this.ι=ι;this.Ё=Ё;this.Ч=0;this.φ=φ;}public void Θπ(List<IMyThrust>ΒΞ,List<string>Ā){Ǿ=new float[Ā.Count];for(int i=0;i<Ǿ.Length;i++)
Ǿ[i]=-1;for(int i=0;i<ΒΞ.Count;i++){string Ǔ=ΖΗ(ΒΞ[i]);int Β=Ā.IndexOf(Ǔ);if(Β!=-1)
Ǿ[Β]=Й(ΒΞ[i].MaxEffectiveThrust,ΒΞ[i].MaxThrust);}}}class ΓЃ{public ά η=null;public List<Vector3>ƣ=new List<Vector3>();public float ΑΑ=0;public float Εο=0;public float ž=0;public float Δρ=0;public Vector3 Βζ=new Vector3();}class ź{public ρ Р=ρ.Γό;public float ǲ=0;public float ı=0;
public string ǳ="";public string Ƿ="";DateTime Ηο;public bool ΕΝ=false;private bool Δγ=false;public bool ΓΥ(bool ǻ){if(ǻ){ı=0;Δγ=false;return false;}Δγ=true;return ı>ǲ;}public void Ιΰ(){ΓΥ(true);ΕΝ=false;}public void ΙІ(){if(Δγ)
if((DateTime.Now-Ηο).TotalSeconds>1){ı++;Ηο=DateTime.Now;}}public bool Γι(){switch(Р){case ρ.ΓЀ:return true;case ρ.ΓЁ:return true;}return false;}}class м{public String ΰ="";public String υ="";public int Ћ=0;public Щ ƍ=Щ.ŀ;public м(String ΰ,String υ,int Ћ,Щ ƍ){this.ΰ=ΰ;this.υ=υ;
this.Ћ=Ћ;this.ƍ=ƍ;}}class ΘЀ{public ΘЀ(Vector3 κ,float ϊ){this.κ=κ;this.ϊ=ϊ;}public Vector3 κ;public float ϊ;}class Ѕ{public DateTime ΔЄ;public DateTime ΖЍ;public String Φ="";public String ΰ="";public String ΓΘ="";public String Λ="";public String Εή="";public String Βύ="";public String ΕΜ="";
public Vector3 Ε=new Vector3();public List<м>Ќ=new List<м>();public Κ Μ=Κ.ĩ;public Њ Ĭ;public float ȑ;public float Ȋ;public int Ў;public int Δι=0;public int Εκ=0;public bool ΖΦ(){return(DateTime.Now-ΔЄ).TotalSeconds>10;}public bool Γο(){if(Ĭ!=Њ.ŷ) return false;return(DateTime.Now-ΖЍ).TotalSeconds>=4;
}public Ѕ(String Φ){this.Φ=Φ;}public Њ ņ(String І,bool ΚЏ,bool Ъ,bool ΚΕ){if(І==""&&!Ъ) return Њ.None;if(Ĭ==Њ.ŷ&&Γο())
Ĭ=Њ.ΑЍ;if(ΚЏ){ΕΜ=І;Ĭ=Њ.ŷ;ΖЍ=DateTime.Now;return Ĭ;}else if(Ъ){ΕΜ="";Ĭ=Њ.None;}else if(ΕΜ==І){if(ΚΕ) Ĭ=Њ.ƺ;return Ĭ;}return Њ.None;}}public String ΘЍ(String Κθ){switch(Κθ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem χ;Vector3 ΒЄ=new Vector3();Vector3 δ=new Vector3();Vector3 Βι=new Vector3();Vector3 ΑΥ=new Vector3();Vector3 ΑΔ=new Vector3();
DateTime ΔЇ=new DateTime();bool ď=true;int Εδ=0;bool ĕ=false;bool ŗ=false;bool ΑΫ=true;bool Θδ=false;bool Ƌ=false;bool Βξ=false;float Ĝ=0;float ť=0;int ĺ=0;int Αμ=0;int Αί=0;int ΔΪ=0;float Ɛ=0;float ſ=0;double ΕΑ=0;float ǆ=0;List<int>ƈ=new List<int>();List<int>ƛ=new List<int>();
Κ Μ=Κ.ĩ;int А=0;float ǿ=0;float ș=0;float ν=0;float Ť=0,Αα=0;IMyRemoteControl Ϋ;IMySensorBlock ψ;List<IMyTimerBlock>Εζ=new List<IMyTimerBlock>();List<IMyShipConnector>њ=new List<IMyShipConnector>();List<IMyThrust>ā=new List<IMyThrust>();List<IMyGyro>Βκ=new List<IMyGyro>();List<IMyTerminalBlock>ω=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ȱ=new List<IMyLandingGear>();List<IMyReactor>Ĺ=new List<IMyReactor>();List<IMyConveyorSorter>ΒЉ=new List<IMyConveyorSorter>();List<IMyGasTank>ǜ=new List<IMyGasTank>();List<IMyTerminalBlock>ǂ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ƃ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ō=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ī=new List<IMyBatteryBlock>();List<IMyTextPanel>Ц=new List<IMyTextPanel>();List<IMyTextSurface>Ȗ=new List<IMyTextSurface>();List<IMyTextPanel>Ő=new List<IMyTextPanel>();IMyTerminalBlock ǡ=null;ά Ο=new ά();ά Δώ=new ά();
ά ΔЁ=new ά();bool Ă=false;bool ƨ=false;bool Ʃ=false;bool Βπ=false;List<String>Ā=new List<string>();List<ά>Ψ=new List<ά>();List<ά>Αρ=new List<ά>();int Δκ=0;double Θε=0;ά Ζ=new ά();Vector3 ǟ;Vector3 ę;Π γ=Π.э;ю Ř=ю.ƿ;int Ї=0;int Ю=0;double ΑΡ=0;bool Βά=false;ź Ј=new ź();
ź Љ=new ź();ъ ѓ=ъ.ΒΑ;bool Ľ=true;ю Ȕ=ю.ƿ;int ѐ=3;int я=3;int н=30;Ǖ ћ=Ǖ.ΓΩ;bool ġ=true;bool Ɗ=true;bool Ƅ=true;bool Ŧ=false;Г д=Г.Γϊ;bool Ġ=true;float Ȑ=90;float џ=20;float п=5;float Ǎ=20;float Ȅ=0.70f;float ƭ=1.50f;float Ƭ=2.50f;float Ū=0f;float Ŕ=0f;Α Ϊ;
Π ў;Α Αε;ΓЃ ξ=null;int Ѐ=0;int ύ=0;int б=0;int ė=0;int Ţ=30;int к=0;bool Е=true;Vector3 ǎ;double в=0;bool ΒІ=false;int Ες=0;int Ж=0;int ΔΟ=0;int Ȓ=0;int ΗЇ=0;Vector3 Αέ=new Vector3();float ђ=0;float т=0;int ũ=0;ά ΒЊ=null;float ΔЅ=0;float ΒΠ=0;float й=0;float ś=0;
bool ΔϏ=false;bool ŧ=false;bool Αν=false;bool ē=false;bool Č=false;DateTime ΚΧ=new DateTime();ά Ŏ=null;ά ΗΔ=null;float Ȧ=0;ї ȅ;float Ĕ=0;float Ũ=0;String ΔЍ="";float Βτ=0;float Ȋ=0;float ȑ=0;List<м>Ƕ=new List<м>();float е=0;bool Αβ=false;bool ΘΩ=false;bool ΘΓ=false;
bool ΒА=false;bool ΘΔ=false;float ΔА=0;float ΕΛ=0;Vector3 ΗΨ=new Vector3();Vector3 Ђ=new Vector3();Vector3 ΒБ=new Vector3(1,1,1);float Εγ=1;Vector3 Αι=new Vector3();bool Βο=false;bool Δχ=false;bool ΔЊ=false;bool Ζω=false;float ΒΪ=0;float ΒΝ=2;Vector3 ΘΥ;Vector3 Εβ;Vector3 Εα;
float[,]Ȁ=new float[3,2];Dictionary<String,float[,]>Ǧ=new Dictionary<string,float[,]>();Dictionary<String,float>Θβ=new Dictionary<String,float>();int ΔΤ=0;Dictionary<String,float[]>Ě=new Dictionary<string,float[]>();float ΕΞ;bool Βέ=false;int ΒΟ=0;int Ķ=0;int Βμ=0;int ŕ=0;
String Λ="";int[]Ηύ=new int[Enum.GetValues(typeof(Σ)).Length];bool ΕΓ=false;int Ŝ=0;int Ў=0;Σ ό=Σ.Main;int ΛΜ=0,ΛΝ=0,ΛΞ=0,ΛΟ=0;int ΚЊ=0,ΚЋ=0;bool Ɓ=false;bool Ζϊ=false;String Ơ="";String ΔΩ="";IMyBroadcastListener ǰ=null;bool ΗΧ=true;List<Ѕ>Ь=null;Ѕ Л=null;
int ΒΥ=0,ΒΦ=0;int ΖА=0,ΖБ=0;int ΚΛ=0,ΚΜ=0,ΚΝ=0;String Κκ="";Exception Ė=null;bool ΒЅ=false;Π Ɲ=Π.э;bool ΗΗ=false;bool Ην=true;bool ΕΤ=false;String Θγ="";bool Αλ=false;bool Εέ=true;int Βν=0;int[]Ǥ=null;Vector3 ΔΫ=new Vector3();bool Ηέ=false;bool ǖ(IMyTerminalBlock Ω)=>Ω.CubeGrid==Me.CubeGrid;
Program(){χ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Γκ(Me,controllerTag,true))
Μ=Κ.ј;μ(Σ.Main);if(Μ!=Κ.ј){Λ="Welcome to [PAM]!";љ ΕΣ=Ια();if(Μ==Κ.ĩ){List<IMyShipDrill>ΗΤ=new List<IMyShipDrill>();List<IMyShipGrinder>Ηή=new List<IMyShipGrinder>();χ.GetBlocksOfType(ΗΤ,q=>q.CubeGrid==Me.CubeGrid);χ.GetBlocksOfType(Ηή,q=>q.CubeGrid==Me.CubeGrid);if(ΗΤ.Count>0)
{Μ=Κ.ƹ;Λ="Miner mode enabled!";}else if(Ηή.Count>0){Μ=Κ.ǫ;Λ="Grinder mode enabled!";}else{Μ=Κ.ή;ȡ(х.ή);}}if(ΕΣ==љ.ΒΖ) ΑΫ=false;if(ΕΣ==љ.ΕЊ) Λ="Data restore failed!";if(ΕΣ==љ.Γω) Λ="New version, wipe data";}}void Main(string Т,UpdateType ΛΨ){try{if(Ė!=null){if(Т.Trim().ToUpper()=="RETRY")
{Ė=null;ď=true;ĺ=0;if(Μ!=Κ.ј) Ѓ();Λ="Restarted after error";}else{ΙΗ();Ζβ();return;}}ŗ=(ΛΨ&UpdateType.Update10)!=0;if(ŗ) Εδ++;ĕ=Εδ>=10;if(ĕ) Εδ=0;if(ŗ){ΒΟ++;if(ΒΟ>4) ΒΟ=0;Αί=(int) Math.Max(0,10-(DateTime.Now-ΔЇ).TotalSeconds);}if(Т!="") Κκ=Т;if(Μ!=Κ.ј)
Ιξ(Т);else
Θώ(Т);ΕΓ=false;try{int ΗΕ=Runtime.CurrentInstructionCount;float š=Й(ΗΕ,Runtime.MaxInstructionCount);if(š>0.90) Λ="Max. instructions >90%";if(š>Ɛ) Ɛ=š;if(Βέ){ƈ.Add(ΗΕ);while(ƈ.Count>10) ƈ.RemoveAt(0);ſ=0;for(int i=0;i<ƈ.Count;i++) ſ+=ƈ[i];ſ=Й(Й(ſ,ƈ.Count),Runtime.MaxInstructionCount);
double ΔІ=Runtime.LastRunTimeMs;if(Βξ&&ΔІ>ΕΑ) ΕΑ=ΔІ;ƛ.Add((int)(ΔІ*1000f));while(ƛ.Count>10) ƛ.RemoveAt(0);ǆ=0;for(int i=0;i<ƛ.Count;i++) ǆ+=ƛ[i];ǆ=Й(ǆ,ƛ.Count)/1000f;}}catch{Ɛ=0;}}catch(Exception e){Ė=e;}}void Ιξ(string Т){bool ΒЇ=false;String ΖЄ="";if(А<=1&&!Γς(Т))
Ζγ(Т);int ΕΔ=50;while(ǰ!=null&&ǰ.HasPendingMessage&&ΕΔ-->0){MyIGCMessage β=ǰ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;String ȓ="";if(А<=1&&Ζα(ref Θ,out ȓ,out ΖЄ)&&ȓ==BROADCAST_SELF_ID){Ζγ(Θ);ΒЇ=true;break;}}bool Ηη=Βξ&&Ϊ==Α.ь&&!Ƌ&&!ΒЇ&&ĺ==0&&!Βπ;
if(ĕ&&Ϊ!=Α.ь) ΒЇ=true;if((ŗ&&!Ηη)||(ĕ&&Ηη)){if(ĺ==0&&(Αί<=0||ď)){Θδ=А>0;А=0;ĺ=1;Ȣ();Ιε();ΖΟ();Ȝ("Scan 1");}else if(ĺ==1){ĺ=2;Ȣ();Ιζ();Ȝ("Scan 2");}else if(ĺ==2){ĺ=0;Ȣ();Ιη();Ιθ();if(А<=1) ΖΩ();Ȝ("Scan 3");ΔЇ=DateTime.Now;if(А<=1&&ΑΫ)
ΒЄ=ΑЃ(Ϋ,Ϋ.CenterOfMass);ΑΫ=false;if(ď){ď=false;bool Λά=γ==Π.В;Ѓ();if(Ην&&resumeAfterRestart&&Λά&&А<=1) ΕΤ=true;}Ην=false;if(Θδ&&А==0) Λ="Setup complete";}else{if(γ==Π.В&&Μ!=Κ.ή){Ȣ();Θι();Ȝ("Inv balance");}Ȣ();switch(Αμ){case 0:Ιϊ();break;case 1:ΙЀ();break;case 2:Ιω();break;
case 3:ΙЊ();break;case 4:ΙϏ();break;case 5:Θσ();break;case 6:Θς(Ϋ);break;}Ȝ("Update: "+Αμ);Αμ++;if(Αμ>6){Αμ=0;Βξ=true;if(Ɲ!=Π.э){switch(Ɲ){case Π.ū:Ĥ();break;case Π.Ŭ:Ĥ();break;case Π.В:Ĥ();break;case Π.ΑЊ:ΑЀ();break;case Π.ΑЋ:ń();break;}Ɲ=Π.э;}}}if(!ď){if(!Ȟ(Ϋ,true))
{Ϋ=null;ΑΫ=true;А=2;}if(А>=2&&Ϊ!=Α.ь)
Ѓ();if(А<=1){ť=Ϋ.CalculateShipMass().PhysicalMass;Ĝ=(float) Ϋ.GetShipSpeed();δ=Ιψ(Ϋ,ΒЄ);Βι=Ϋ.WorldMatrix.Forward;ΑΥ=Ϋ.WorldMatrix.Left;ΑΔ=Ϋ.WorldMatrix.Down;ΙЄ();ΙЇ();Ιό();if(ΕΤ){ΕΤ=false;Ĥ();if(Ϊ!=Α.ь) Λ="Job resumed after restart";}if(Ϊ!=Α.ь){Ƌ=false;Ζζ(false);ΕϏ(false);
String ΛΓ=Γε(Ϊ)+" "+(int) Ϊ;Ȣ();ΙЂ();ΔΗ(false);Ȝ(ΛΓ);ΙЈ();ΙЃ();Ȣ();Ιώ();Ȝ("Thruster");Ȣ();Ζυ();Ȝ("Gyroscope");}else{if(Ƌ){if(ΑЇ()){л(ΑΔ,Βι,ΑΥ,0.25f,true);Ζυ();Λ="Aligning to planet: "+Math.Round(ΒΪ-0.25f,2)+"°";if(Βο) Γθ(true,true);}else Γθ(true,true);}}Αλ=false;}}}Ȣ();Ζβ();Ȝ("Print");
if(ΒЇ||ΔΪ<=0){Ȣ();Θκ(ΖЄ);Ȝ("Broadcast");ΔΪ=4;}else if(ĕ) ΔΪ--;}void Ζγ(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȥ=τ.ElementAtOrDefault(1);var ǅ=τ.ElementAtOrDefault(2);var Ζό=τ.ElementAtOrDefault(3);String Ȯ="Invalid argument: "+Т;
bool Εξ=false;switch(І){case "UP":this.Γφ(false);break;case "DOWN":this.Γυ(false);break;case "UPLOOP":this.Γφ(true);break;case "DOWNLOOP":this.Γυ(true);break;case "APPLY":this.Εύ(true);break;case "MRES":Ў=0;break;case "STOP":this.Ѓ();break;case "PATHHOME":{this.Ѓ();this.ΔΑ();}break;
case "PATH":{this.Ѓ();this.ΔΑ();Ο.λ=true;}break;case "START":{this.Ѓ();ΓБ();}break;case "ALIGN":{Γθ(!Ƌ,false);}break;case "CONT":{this.Ѓ();this.Ĥ();}break;case "JOBPOS":{this.Ѓ();this.ń();}break;case "HOMEPOS":{this.Ѓ();this.ΑЀ();}break;case "FULL":{Αβ=true;}break;
case "RESET":{ΒЅ=true;А=2;}break;case "EXPORT":{ΘІ();}break;case "IMPORT":{ΙΛ(ȥ=="JOB");}break;default:Εξ=true;break;}if(Μ!=Κ.ή){switch(І){case "SHUTTLE":{ΘЃ();}break;case "CFGS":{if(!Θό(ȥ,ǅ,Ζό)) Λ=Ȯ;}break;case "CFGB":{if(!Θϋ(ȥ,ǅ)) Λ=Ȯ;}break;case "CFGL":
{if(!Ņ(ref Ȑ,true,ϋ.ΑЈ,ȥ,"")||!Θύ(ǅ))
Λ=Ȯ;}break;case "CFGE":{if(!Ņ(ref п,true,ϋ.ΑЉ,ȥ,"IG")||
!Ņ(ref џ,true,ϋ.Γχ,ǅ,"IG")||
!Ņ(ref Ǎ,true,ϋ.Γψ,Ζό,"IG"))
Λ=Ȯ;}break;case "CFGA":{if(!Ņ(ref Ȅ,false,ϋ.Ασ,ȥ,"")) Λ=Ȯ;}break;case "CFGW":{if(!Ņ(ref ƭ,false,ϋ.ǁ,ȥ,"")||
!Ņ(ref Ƭ,false,ϋ.ǁ,ǅ,""))
Λ=Ȯ;}break;case "NEXT":{ΓΚ(false);}break;case "PREV":{ΓΚ(true);}break;default:if(Εξ) Λ=Ȯ;break;}}else{switch(І){case "UNDOCK":{Αλ=true;}break;default:if(Εξ) Λ=Ȯ;break;}}}String ΘЎ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Μ!=Κ.ή)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΘЃ(){Ѓ();Μ=Κ.ή;ȡ(х.ή);Ζ.λ=false;Ο.λ=false;ψ=null;ω.Clear();γ=Π.э;}void Γθ(bool М,bool Ϗ){if(!М) Λ="Aligning canceled";if(Ϗ) Λ="Aligning done";if(Ϗ||!М){Ƌ=false;Ζο();ΒΔ(false,0,0,0,0);return;}if(ΑЇ()) Ƌ=true;}bool Θό(String Λή,String Κα,String ΚΡ)
{bool ΛΪ=γ==Π.В;int w,h,d;if(int.TryParse(Λή,out w)&&int.TryParse(Κα,out h)&&int.TryParse(ΚΡ,out d)){this.Ѓ();ѐ=w;я=h;н=d;Ŋ(false);ǀ(false,false);if(ΛΪ) Ĥ();return true;}return false;}bool Θύ(String Ǆ){if(Ǆ=="ON"){ġ=true;return true;}if(Ǆ=="OFF"){ġ=false;return true;}
return false;}bool Θϋ(String ΗΣ,String ΒΫ){bool ȱ=true;if(ΗΣ=="HOME") Ľ=true;else if(ΗΣ=="STOP") Ľ=false;else ȱ=false;if(ΒΫ=="HOME") ѓ=ъ.ΒΑ;else if(ΒΫ=="STOP") ѓ=ъ.Ζξ;else if(ΒΫ=="JOB") ѓ=ъ.Γβ;else if(ΒΫ=="IG") ѓ=ъ.Ƴ;else ȱ=false;return ȱ;}void μ(Σ ц){Ηύ[(int) ό]=Ў;
Ў=Ηύ[(int) ц];if(ц==Σ.ƽ) Ў=0;ό=ц;if(Μ!=Κ.ј)
ŉ(ό==Σ.Ƹ,false,0,0);ΕΓ=true;}void Γφ(bool ΔЋ){if(Ў>0) Ў--;else if(ΔЋ) Ў=Ŝ-1;}void Γυ(bool ΔЋ){if(Ў<Ŝ-1) Ў++;else if(ΔЋ) Ў=0;}String ȡ(х β){switch(β){case х.ΒΒ:Λ="Job is running";break;case х.Ư:Λ="Connector not ready!";break;case х.Ιο:Λ="Ship modified, path outdated!";break;
case х.ΖΡ:Λ="Interrupted by player!";break;case х.ή:Λ="Shuttle mode enabled!";break;}return "";}String ΙΓ(ю κ){switch(κ){case ю.ƿ:return "Top-Left";case ю.ΕЁ:return "Center";default:return "";}}String ΘЊ(Ǖ Џ){switch(Џ){case Ǖ.ΓΠ:return "Auto"+(Μ==Κ.ƹ?" (Ore)":"");case Ǖ.ΓΡ:return "Auto (+Stone)";
case Ǖ.ΓΩ:return "Default";default:return "";}}String Γγ(Π Є){switch(Є){case Π.э:return "No job";case Π.Ŷ:return "Job paused";case Π.В:return "Job active";case Π.ū:return "Job active";case Π.Ŭ:return "Job active";case Π.Γά:return "Job done";case Π.Αψ:return "Job changed";case Π.ΑЊ:return "Move home";
case Π.ΑЋ:return "Move to job";}return "";}String ΕЏ(ъ ΚΔ){switch(ΚΔ){case ъ.ΒΑ:return "Return home";case ъ.Γβ:return "Fly to job pos";case ъ.Ζξ:return "Stop";case ъ.Ƴ:return "Ignore";}return "";}String ΘЌ(Г Џ){switch(Џ){case Г.Γϊ:return "Off";case Г.Γέ:return "Drop pos (Stone) ";case Г.Αϋ:return "Drop pos (Sto.+Ice)";
case Г.ΕЅ:return "Cur. pos (Stone)";case Г.ΓΧ:return "Cur. pos (Sto.+Ice)";case Г.Γμ:return "In motion (Stone)";case Г.ΑЅ:return "In motion (Sto.+Ice)";}return "";}String ΕЍ(ї Є){switch(Є){case ї.None:return "No batteries";case ї.ł:return "Charging";case ї.ΕЈ:return "Discharging";}return "";}
String Γε(Α Є){String Λθ=Μ==Κ.ή?"target":"job";switch(Є){case Α.ь:return "Idle";case Α.Ć:return "Flying to XY position";case Α.ŵ:return Μ==Κ.ǫ?"Grinding":"Mining";case Α.Ƞ:return "Returning";case Α.Ʊ:return "Flying to drop pos";case Α.ħ:return "Returning to dock";case Α.И:return "Flying to dock area";
case Α.у:return "Flying to job area";case Α.Ħ:return "Flying to path";case Α.Ű:return "Flying to job position";case Α.ȁ:return "Approaching dock";case Α.Ń:return "Docking";case Α.Ŀ:return "Aligning";case Α.Ģ:return "Aligning";case Α.ƻ:return "Retry docking";case Α.Ȥ:return "Unloading";case Α.Ī:return Θγ;
case Α.Ǯ:return "Undocking";case Α.ł:return "Charging batteries";case Α.ΒΛ:return "Waiting for uranium";case Α.Αύ:return "Filling up hydrogen";case Α.ΔΘ:return "Waiting for ejection";case Α.ΒΚ:return "Waiting for ejection";case Α.ΑϏ:return "Flying to drop pos";}return "";}String ΖΘ(ρ Р){switch(Р)
{case ρ.Γό:return "On \"Undock\" command";case ρ.ΓЀ:return "On player entered cockpit";case ρ.Ζί:return "On ship is full";case ρ.Ζή:return "On ship is empty";case ρ.ΓЁ:return "On time delay";case ρ.ΑЏ:return "On batteries empty(<25%)";case ρ.ΑЎ:return "On batteries empty(=0%)";case ρ.Γϋ:return "On batteries full";
case ρ.ΓϏ:return "On hydrogen empty(<25%)";case ρ.Γύ:return "On hydrogen empty(=0%)";case ρ.Γώ:return "On hydrogen full";}return "";}bool Ν(ref String Γ,int ΗА,int ĵ,bool Ι,String ř){Ŝ+=1;if(ΗА==ĵ) ř=">"+ř+(ΒΟ>=2?" .":"");else ř=" "+ř;Γ+=ř+"\n";return ΗА==ĵ&&Ι;}String Εύ(bool Ι)
{int Β=0;int Η=Ў;Ŝ=0;String Ɵ="———————————————\n";String ƥ="--------------------------------------------\n";String Γ="";Γ+=Γγ(γ)+" | "+(Ο.λ?"Ready to dock":"No dock")+"\n";Γ+=Ɵ;double ȕ=Math.Max(Math.Round(this.в),0);if(ό==Σ.Main){bool ķ=Μ==Κ.ή;if(Ν(ref Γ,Η,Β++,Ι," Record path & set home")) ΔΑ();
if(Μ==Κ.ƹ)
if(Ν(ref Γ,Η,Β++,Ι," Setup mining job")) μ(Σ.Ƹ);if(Μ==Κ.ǫ)
if(Ν(ref Γ,Η,Β++,Ι," Setup grinding job")) μ(Σ.Ƹ);if(Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Setup shuttle job")) μ(Σ.ΓЎ);if(Ν(ref Γ,Η,Β++,Ι," Continue job")) Ĥ();if(Ν(ref Γ,Η,Β++,Ι," Fly to home position")) ΑЀ();if(Ν(ref Γ,Η,Β++,Ι," Fly to job position")) ń();if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(ķ) μ(Σ.ΒΕ);else μ(Σ.Αφ);if(Ν(ref Γ,Η,Β++,Ι," Info")) μ(Σ.ΖΝ);if(Μ!=Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," Help")) μ(Σ.ΖΙ);}else if(ό==Σ.Ƹ){double Λέ=Math.Round(ѐ*ǿ,1);double Κΰ=Math.Round(я*ș,1);String З="";if(Ν(ref З,Η,Β++,Ι," Start new job!")) ΓБ();if(Ν(ref З,Η,Β++,Ι," Change current job")){ǀ(false,false);μ(Σ.Main);}if(Ν(ref З,Η,Β++,Ι," Width + (Width: "+ѐ+" = "+Λέ+"m)")){ŭ(ref ѐ,5,20,1);Ŋ(true);}
if(Ν(ref З,Η,Β++,Ι," Width -")){ŭ(ref ѐ,-5,20,-1);Ŋ(true);}if(Ν(ref З,Η,Β++,Ι," Height + (Height: "+я+" = "+Κΰ+"m)")){ŭ(ref я,5,20,1);Ŋ(true);}if(Ν(ref З,Η,Β++,Ι," Height -")){ŭ(ref я,-5,20,-1);Ŋ(true);}if(Ν(ref З,Η,Β++,Ι," Depth + ("+(ћ==Ǖ.ΓΩ?"Depth":"Min")+": "+н+"m)")){ŭ(ref н,5,50,2);Ŋ(true);}
if(Ν(ref З,Η,Β++,Ι," Depth -")){ŭ(ref н,-5,50,-2);Ŋ(true);}if(Ν(ref З,Η,Β++,Ι," Depth mode: "+ΘЊ(ћ))){ћ=ĥ(ћ);}if(Ν(ref З,Η,Β++,Ι," Start pos: "+ΙΓ(Ȕ))){Ȕ=ĥ(Ȕ);}if(Μ==Κ.ǫ&&ћ==Ǖ.ΓΡ) ћ=ĥ(ћ);Γ+=ΓЈ(8,З,Η,ref ΚЊ);}else if(ό==Σ.ΓЎ){float[]ΗΙ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.Ζλ);if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=" Leave connector 1:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΘ(Ј.Р))) Ј.Р=ĥ(Ј.Р);if(!Ј.Γι()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЁ((int) Ј.ǲ))) Ј.ǲ=ΕІ(Ј.ǲ,ΗΙ);Γ+=" Leave connector 2:\n";if(Ν(ref Γ,Η,Β++,Ι," - "+ΖΘ(Љ.Р))) Љ.Р=ĥ(Љ.Р);
if(!Љ.Γι()) Γ+="\n";else if(Ν(ref Γ,Η,Β++,Ι," - Delay: "+ΑЁ((int) Љ.ǲ))) Љ.ǲ=ΕІ(Љ.ǲ,ΗΙ);}else if(ό==Σ.Ζλ){if(Ν(ref Γ,Η,Β++,Ι," Start job!")) ΓБ();if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.ΓЎ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.ǳ!=""?Ј.ǳ:"-"))) Ј.ǳ=ΑЌ(ref ΛΜ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Ј.Ƿ!=""?Ј.Ƿ:"-"))) Ј.Ƿ=ΑЌ(ref ΛΞ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Љ.ǳ!=""?Љ.ǳ:"-"))) Љ.ǳ=ΑЌ(ref ΛΝ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ν(ref Γ,Η,Β++,Ι," = "+(Љ.Ƿ!=""?Љ.Ƿ:"-"))) Љ.Ƿ=ΑЌ(ref ΛΟ);
}else if(ό==Σ.Ʒ){String Ητ=Ȑ+" %";if(ĕ) Ķ++;if(Ķ>1){Ķ=0;Βμ++;if(Βμ>1) Βμ=0;}bool[]ΕΪ=new bool[]{Ĺ.Count==0,ȅ==ї.None,ǜ.Count==0};int Θή=0;while(true){Θή++;ŕ++;if(ŕ>ΕΪ.Length-1) ŕ=0;if(Θή>=ΕΪ.Length) break;if(!ΕΪ[ŕ]) break;}bool ķ=Μ==Κ.ή;if(!ķ&&ġ&&е!=-1&&Βμ==0)
Ητ=е<1000000?Math.Round(е)+" Kg":Math.Round(е/1000)+" t";if(Ν(ref Γ,Η,Β++,Ι," Stop!")){Ѓ();μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Behavior settings"))
if(!ķ) μ(Σ.Αφ);else μ(Σ.ΒΕ);if(!ķ){if(Ν(ref Γ,Η,Β++,Ι," Next hole")) ΓΚ(false);}else if(Ν(ref Γ,Η,Β++,Ι," Undock")) Αλ=true;Γ+=ƥ;if(!ķ) Γ+="Progress: "+Math.Round(ΑΡ,1)+" %\n";Γ+="State: "+Γε(Ϊ)+" "+ȕ+"m \n";Γ+="Load: "+Βτ+" % Max: "+Ητ+" \n";if(ŕ==0) Γ+="Uranium: "+(Ĺ.Count==0?"No reactors":Math.Round(Ũ,1)+"Kg "+(п==-1?"":" Min: "+п+" Kg"))+"\n";
if(ŕ==1) Γ+="Battery: "+(ȅ==ї.None?ΕЍ(ȅ):Ȧ+"% "+(џ==-1||ķ?"":" Min: "+џ+" %"))+"\n";if(ŕ==2) Γ+="Hydrogen: "+(ǜ.Count==0?"No tanks":Math.Round(Ĕ,1)+"% "+(Ǎ==-1||ķ?"":" Min: "+Ǎ+" %"))+"\n";}else if(ό==Σ.Αφ){String З="";if(Ν(ref З,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}
if(Ν(ref З,Η,Β++,Ι," Max load: "+Ȑ+"%")) щ(ref Ȑ,Ȑ<=80?-10:-5,ϋ.ΑЈ,false);if(Ν(ref З,Η,Β++,Ι," Weight limit: "+(ġ?"On":"Off"))) ġ=!ġ;if(Ν(ref З,Η,Β++,Ι," Ejection: "+ΘЌ(д))) д=ĥ(д);if(Ν(ref З,Η,Β++,Ι," Toggle sorters: "+(Ŧ?"On":"Off"))){Ŧ=!Ŧ;if(Ŧ) ȃ(Εέ);}
if(Ν(ref З,Η,Β++,Ι," Unload ice: "+(Ġ?"On":"Off"))) Ġ=!Ġ;if(Ν(ref З,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЉ,true);if(Ν(ref З,Η,Β++,Ι," Battery: "+(џ==-1?"Ignore":"Min "+џ+"%"))) щ(ref џ,-5,ϋ.Γχ,true);if(Ν(ref З,Η,Β++,Ι," Hydrogen: "+(Ǎ==-1?"Ignore":"Min "+Ǎ+"%"))) щ(ref Ǎ,-10,ϋ.Γψ,true);
if(Ν(ref З,Η,Β++,Ι," When done: "+(Ľ?"Return home":"Stop"))) Ľ=!Ľ;if(Ν(ref З,Η,Β++,Ι," On damage: "+ΕЏ(ѓ))) ѓ=ĥ(ѓ);if(Ν(ref З,Η,Β++,Ι," Advanced...")) μ(Σ.ΓΛ);Γ+=ΓЈ(8,З,Η,ref ΚЋ);}else if(ό==Σ.ΓΛ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}
if(Ν(ref Γ,Η,Β++,Ι,(Μ==Κ.ǫ?" Grinder":" Drill")+" inv. balancing: "+(Ɗ?"On":"Off"))) Ɗ=!Ɗ;if(Ν(ref Γ,Η,Β++,Ι," Enable"+(Μ==Κ.ǫ?" grinders":" drills")+": "+(Ƅ?"Fwd + Bwd":"Fwd"))) Ƅ=!Ƅ;if(Ν(ref Γ,Η,Β++,Ι," Work speed fwd.: "+ƭ+"m/s")) щ(ref ƭ,0.5f,ϋ.ǁ,false);
if(Ν(ref Γ,Η,Β++,Ι," Work speed bwd.: "+Ƭ+"m/s")) щ(ref Ƭ,0.5f,ϋ.ǁ,false);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȅ*100f)+"%"+(Ȅ>0.80f?" (risky)":""))) щ(ref Ȅ,0.1f,ϋ.Ασ,false);if(Ν(ref Γ,Η,Β++,Ι," Width overlap: "+Ū*100f+"%")) ΕЂ(true,0.05f);if(Ν(ref Γ,Η,Β++,Ι," Height overlap: "+Ŕ*100f+"%")) ΕЂ(false,0.05f);
}else if(ό==Σ.ΒΕ){if(Ν(ref Γ,Η,Β++,Ι," Back")){if(γ==Π.В) μ(Σ.Ʒ);else μ(Σ.Main);}if(Ν(ref Γ,Η,Β++,Ι," Max load: "+Ȑ+"%")) щ(ref Ȑ,Ȑ<=80?-10:-5,ϋ.ΑЈ,false);if(Ν(ref Γ,Η,Β++,Ι," Unload ice: "+(Ġ?"On":"Off"))) Ġ=!Ġ;if(Ν(ref Γ,Η,Β++,Ι," Uranium: "+(п==-1?"Ignore":"Min "+п+"Kg"))) щ(ref п,(п>5?-5:-1),ϋ.ΑЉ,true);
if(Ν(ref Γ,Η,Β++,Ι," Battery: "+(џ==-1?"Ignore":"Charge up"))) џ=(џ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," Hydrogen: "+(Ǎ==-1?"Ignore":"Fill up"))) Ǎ=(Ǎ==-1?1:-1);if(Ν(ref Γ,Η,Β++,Ι," On damage: "+ΕЏ(ѓ))) ѓ=ĥ(ѓ);if(Ν(ref Γ,Η,Β++,Ι," Acceleration: "+Math.Round(Ȅ*100f)+"%"+(Ȅ>0.80f?" (risky)":""))) щ(ref Ȅ,0.1f,ϋ.Ασ,false);
}else if(ό==Σ.Ζδ){double Κλ=0;if(Ψ.Count>0) Κλ=Vector3.Distance(Ψ.Last().Ε,δ);if(Ν(ref Γ,Η,Β++,Ι," Stop path recording")) Ζπ();if(Μ!=Κ.ή){if(Ν(ref Γ,Η,Β++,Ι," Home: "+(Ă?"Use old home":(Ο.λ?"Was set! ":"none ")))) Ă=!Ă;}else{if(Ν(ref Γ,Η,Β++,Ι," Connector 1: "+(Ă?"Use old connector":(Ο.λ?"Was set! ":"none ")))) Ă=!Ă;
if(Ν(ref Γ,Η,Β++,Ι," Connector 2: "+(ƨ?"Use old connector":(Ζ.λ?"Was set! ":"none ")))) ƨ=!ƨ;}if(Ν(ref Γ,Η,Β++,Ι," Path: "+(Ʃ?"Use old path":(Ψ.Count>1?"Count: "+Ψ.Count:"none ")))) Ʃ=!Ʃ;Γ+=ƥ;Γ+="Wp spacing: "+Math.Round(Θε)+"m\n";}else if(ό==Σ.ΔΒ){if(Ν(ref Γ,Η,Β++,Ι," Stop")){Ѓ();μ(Σ.Main);}
Γ+=ƥ;Γ+="State: "+Γε(Ϊ)+" \n";Γ+="Speed: "+Math.Round(Ĝ,1)+"m/s\n";Γ+="Target dist: "+ȕ+"m\n";Γ+="Wp count: "+Ψ.Count+"\n";Γ+="Wp left: "+Ες+"\n";}else if(ό==Σ.ΖΝ){List<IMyTerminalBlock>ŏ=ΕА();if(ĕ) Ķ++;if(Ķ>=ŏ.Count) Ķ=0;if(Ν(ref Γ,Η,Β++,Ι," Next")) μ(Σ.ΖΞ);
Γ+=ƥ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Βτ,1)+"% "+Math.Round(Ȋ,1)+" / "+Math.Round(ȑ,1)+"\n";Γ+="Uranium: "+(Ĺ.Count==0?"No reactors":Math.Round(Ũ,1)+"Kg "+ΔЍ)+"\n";Γ+="Battery: "+(ȅ==ї.None?"":Ȧ+"% ")+ΕЍ(ȅ)+"\n";Γ+="Hydrogen: "+(ǜ.Count==0?"No tanks":Math.Round(Ĕ,1)+"% ")+"\n";
Γ+="Damage: "+(ŏ.Count==0?"None":""+(Ķ+1)+"/"+ŏ.Count+" "+ŏ[Ķ].CustomName)+"\n";}else if(ό==Σ.ΖΞ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=ƥ;Γ+="Next scan: "+Αί+"s\n";Γ+="Drill area: "+Math.Round(ǿ,1)+"x"+Math.Round(ș,1)+"m Ship: "+Math.Round(Ť,1)+"x"+Math.Round(Αα,1)+"m ("+Math.Round(ν,1)+"m)\n";
Γ+="Broadcast: "+(Ɓ?"Online - "+Ơ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ɛ*100f,1)+"% \n";}else if(ό==Σ.ΖΙ){if(Ν(ref Γ,Η,Β++,Ι," Back")) μ(Σ.Main);Γ+=ƥ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(А==2) Γ="Fatal setup error\nNext scan: "+Αί+"s\n";if(ΒЅ) Γ="Recompile script now";int Κμ=Γ.Split('\n').Length;for(int i=Κμ;i<=10;i++) Γ+="\n";Γ+=Ɵ;Γ+="Last: "+Λ+"\n";
return Γ;}void ΕЂ(bool Κη,float ǥ){Ѓ();ǀ(true,false);if(Κη) щ(ref Ū,ǥ,ϋ.ΓЂ,false);else щ(ref Ŕ,ǥ,ϋ.ΓЂ,false);ΕЀ();ŉ(true,true,0,0);}float ΕІ(float ο,float[]ΓΗ){float ί=ΓΗ[0];for(int i=ΓΗ.Length-1;i>=0;i--)
if(ο<ΓΗ[i]) ί=ΓΗ[i];return ί;}String ΑЌ(ref int ĵ){String ί="";if(ĵ>=Εζ.Count) ĵ=-1;if(ĵ>=0) ί=Εζ[ĵ].CustomName;ĵ++;return ί;}void ΒΘ(string ΰ){if(γ!=Π.В) return;if(ΰ=="") return;IMyTerminalBlock Ω=χ.GetBlockWithName(ΰ);if(Ω==null||!(Ω is IMyTimerBlock)){Λ="Timerblock "+ΰ+" not found!";
return;}((IMyTimerBlock) Ω).Trigger();}void ŭ(ref int value,int ǉ,int ğ,int ΕΫ){if(ǉ==0) return;if(value<ğ&&ΕΫ>0||value<=ğ&&ΕΫ<0){value+=ΕΫ;return;}int ΙЌ=Math.Abs(ǉ);int ΔΙ=0;int ΕΕ=1;while(true){ΔΙ+=ΕΕ*ΙЌ*10;if(ǉ<0&&value-ğ<=ΔΙ) break;if(ǉ>0&&value-ğ<ΔΙ) break;
ΕΕ++;}value+=ΕΕ*ǉ;}void Ŋ(bool Λλ){ѐ=Math.Max(ѐ,1);я=Math.Max(я,1);н=Math.Max(н,0);ŉ(ό==Σ.Ƹ,false,0,0);}bool Ņ(ref float value,bool ΚЇ,ϋ υ,String Ŗ,String Κδ){if(Ŗ=="") return false;float Ɣ=-1;bool Ηδ=false;if(Ŗ.ToUpper()==Κδ) Ηδ=true;else if(!float.TryParse(Ŗ,out Ɣ)) return false;
else Ɣ=Math.Max(0,Ɣ);if(ΚЇ) Ɣ=(float) Math.Round(Ɣ);щ(ref value,Ɣ,υ,Ηδ,false);return true;}void щ(ref float value,float ǥ,ϋ υ,bool Ακ){щ(ref value,value+ǥ,υ,Ακ,true);}void щ(ref float value,float Ƒ,ϋ υ,bool Ακ,bool Λβ){float ǹ=0,Ǹ=0;if(υ==ϋ.ǁ){ǹ=0.5f;Ǹ=10f;}if(υ==ϋ.Ασ){ǹ=0.1f;Ǹ=1f;}
if(υ==ϋ.Ƴ){ǹ=50f;Ǹ=100f;}if(υ==ϋ.Γχ){ǹ=5f;Ǹ=30f;}if(υ==ϋ.ΑЉ){ǹ=1f;Ǹ=25f;}if(υ==ϋ.ΑЈ){ǹ=10f;Ǹ=95f;}if(υ==ϋ.Γψ){ǹ=10f;Ǹ=90f;}if(υ==ϋ.Ιφ){ǹ=10f;Ǹ=1800;}if(υ==ϋ.ΓЂ){ǹ=0.0f;Ǹ=0.75f;}if(Ƒ==-1&&Ακ){value=-1;return;}if(value==-1) Ακ=false;
bool ΗЉ=Ƒ<ǹ||Ƒ>Ǹ;if(ΗЉ&&Λβ){if(Ƒ<value) value=Ǹ;else if(Ƒ>value) value=ǹ;}else value=Ƒ;if(ΗЉ&&Ακ) value=-1;else value=Math.Max(ǹ,Math.Min(value,Ǹ));value=(float) Math.Round(value,2);}void ΓΚ(bool ΚЂ){if(ΚЂ) б=Math.Max(0,б-1);else б++;Γί();Γή();for(int k=0;k<ȭ.Count&&k<г.Length;k++) ΓЌ(ȭ[k],k<б);
Д=-1;ΔΗ(true);}T ĥ<T>(T ο){int Β=Array.IndexOf(Enum.GetValues(ο.GetType()),ο);Β++;if(Β>=ΘЄ(ο)) Β=0;return(T) Enum.GetValues(ο.GetType()).GetValue(Β);}int ΘЄ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΔΑ(){Αρ.Clear();for(int i=0;i<Ψ.Count;i++) Αρ.Add(Ψ[i]);Ψ.Clear();
Βπ=true;Δώ=new ά(Ο);ΔЁ=new ά(Ζ);Ο.λ=false;if(Μ==Κ.ή) Ζ.λ=false;for(int i=0;i<Ǧ.Count;i++)
if(!Ā.Contains(Ǧ.Keys.ElementAt(i)))
Ā.Add(Ǧ.Keys.ElementAt(i));Ă=false;ƨ=false;Ʃ=false;μ(Σ.Ζδ);}void Ζπ(){if(Ă) Ο=Δώ;if(ƨ) Ζ=ΔЁ;if(Ʃ){Ψ.Clear();for(int i=0;i<Αρ.Count;i++) Ψ.Add(Αρ[i]);}Βπ=false;Ѓ();μ(Σ.Main);}void ΙЄ(){if(!Βπ) return;if(Ϊ!=Α.ь){Ζπ();return;}if(!Δώ.λ) Ă=false;if(!ΔЁ.λ) ƨ=false;if(Αρ.Count<=1) Ʃ=false;
IMyShipConnector О=з(MyShipConnectorStatus.Connectable);if(О==null) О=з(MyShipConnectorStatus.Connected);if(О!=null){if(Math.Round(Ĝ,2)<=0.20) Δκ++;else Δκ=0;if(Δκ>=5){if(Μ==Κ.ή&&(Ο.λ||Ă)&&Vector3.Distance(Ο.Ε,О.GetPosition())>5){Ζ.ι=Ϋ.WorldMatrix.Forward;Ζ.Ё=Ϋ.WorldMatrix.Left;
Ζ.θ=Ϋ.WorldMatrix.Down;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.Ε=О.GetPosition();Ζ.λ=true;Ζ.о=О.Position;}else{Ο.ι=Ϋ.WorldMatrix.Forward;Ο.Ё=Ϋ.WorldMatrix.Left;Ο.θ=Ϋ.WorldMatrix.Down;Ο.φ=Ϋ.GetNaturalGravity();Ο.Ε=О.GetPosition();Ο.λ=true;Ο.о=О.Position;}}}double ΔЃ=-1;if(Ψ.Count>0) ΔЃ=Vector3.Distance(δ,Ψ.Last().Ε);
double ΛΑ=Math.Max(1.5,Math.Pow(Ĝ/100.0,2));double Ηώ=Math.Max(Ĝ*ΛΑ,2);Θε=Ηώ;if((ΔЃ==-1)||ΔЃ>=Ηώ){ά έ=new ά(δ,Βι,ΑΔ,ΑΥ,Ϋ.GetNaturalGravity());έ.Θπ(ā,Ā);Ψ.Add(έ);}}ά ы(){if(Μ!=Κ.ή) return Ǡ!=null?Ǡ:Ο;return ΗΔ;}void Ÿ(ά α,Π Є){ΗΔ=α;if(γ==Π.В) ў=Є;}bool ȝ(ά α,float ϊ,bool ΛΩ,out Vector3 κ)
{if(ΛΩ){Vector3I Δφ=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock Ω=Me.CubeGrid.GetCubeBlock(Δφ);if(Ω==null||!(Ω.FatBlock is IMyShipConnector)){κ=new Vector3();return false;}Vector3 ΚΚ=ф(Ϋ,Ω.FatBlock.GetPosition()-δ);Vector3 ΚΙ=ф(Ϋ,Ω.FatBlock.WorldMatrix.Forward);κ=α.Ε-ΔΖ(α.ι,α.θ*-1,ΚΚ)-ΔΖ(α.ι,α.θ*-1,ΚΙ)*ϊ;
return true;}else{κ=α.Ε;return true;}}Vector3 ć(int ΚΗ,int ƚ,bool ΚЅ){if(!ΚЅ&&Ηέ) return ΔΫ;float ΚΘ=((Ї-1f)/2f)-ΚΗ;float ΚЈ=((Ю-1f)/2f)-ƚ;ΔΫ=Ζ.Ε+Ζ.Ё*ΚΘ*ǿ+ę*-1*ΚЈ*ș;Ηέ=true;return ΔΫ;}Vector3 Γδ(Vector3 ΛΕ,float Δε){return ΛΕ+(ǟ*Δε);}ε ΓΪ(){float Ы=-1;ε ί=ε.ț;
if(Μ!=Κ.ή){ΖΩ();if(γ!=Π.э){Vector3 Н=ň(ǟ,ę*-1,δ-Ζ.Ε);if(Math.Abs(Н.X)<=(float)(Ї*ǿ)/2f&&Math.Abs(Н.Y)<=(float)(Ю*ș)/2f){if(Н.Z<=-1&&Н.Z>=-Ţ*2) return ε.Αυ;if(Н.Z>-1&&Н.Z<ν*2) return ε.ț;}if(ģ(Ζ.Ε,ref Ы)) ί=ε.ț;}if(Ο.λ){if(ģ(Ο.Ε,ref Ы)) ί=ε.Ț;for(int i=0;i<Ψ.Count;i++)
if(ģ(Ψ[i].Ε,ref Ы)) ί=ε.ĉ;if(Vector3.Distance(δ,Ο.Ε)<dockDist*ν) ί=ε.Ț;if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null)
ί=ε.Ț;}}else{Vector3 κ=new Vector3();IMyShipConnector ȩ=з(MyShipConnectorStatus.Connected);if(Ο.λ){if(ģ(Ο.Ε,ref Ы)) ί=ε.Ț;if(ȝ(Ο,dockDist,true,out κ))
if(ģ(κ,ref Ы)) ί=ε.Ț;if(ȩ!=null&&Vector3.Distance(ȩ.GetPosition(),Ο.Ε)<5)
return ε.ΓΞ;}for(int i=0;i<Ψ.Count;i++)
if(Vector3.Distance(Ψ[i].Ε,Ο.Ε)>dockDist*ν&&Vector3.Distance(Ψ[i].Ε,Ζ.Ε)>dockDist*ν)
if(ģ(Ψ[i].Ε,ref Ы)) ί=ε.ĉ;if(Ζ.λ){if(ģ(Ζ.Ε,ref Ы)) ί=ε.ț;if(ȝ(Ζ,dockDist,true,out κ))
if(ģ(κ,ref Ы)) ί=ε.ț;if(ȩ!=null&&Vector3.Distance(ȩ.GetPosition(),Ζ.Ε)<5)
return ε.ΓΟ;}}return ί;}bool ģ(Vector3 κ,ref float Ы){float ϊ=Vector3.Distance(κ,δ);if(ϊ<Ы||Ы==-1){Ы=ϊ;return true;}return false;}void ΓБ(){if(А>0){Λ="Setup error! Can't start";return;}if(Μ==Κ.ή){Ĥ();return;}Ζ.Ε=δ;Ζ.φ=Ϋ.GetNaturalGravity();Ζ.ι=Βι;Ζ.θ=ΑΔ;Ζ.Ё=ΑΥ;
ǟ=ǡ.WorldMatrix.Forward;ę=Ζ.θ;if(ǟ==Ϋ.WorldMatrix.Down) ę=Ϋ.WorldMatrix.Backward;ǀ(true,true);Τ(Α.Ć);ΓА();}void ǀ(bool ǵ,bool ΚІ){if(γ==Π.э&&!ǵ) return;bool Κτ=ǵ||γ==Π.Γά||Ї!=ѐ||Ю!=я||Ř!=Ȕ;if(Κτ){if(γ!=Π.э){γ=Π.Αψ;ć(Ѐ,ύ,ΚІ);Λ="Job changed, lost progress";}Ř=Ȕ;
Ї=ѐ;Ю=я;ύ=0;Ѐ=0;г="";Д=-1;ś=0;ė=0;й=0;б=0;ΔΗ(true);}}void ΓЄ(){и(δ,0);ǘ(ā,true);}void ΓА(){ȡ(х.ΒΒ);ΓЄ();Ƽ(Ȱ,false);ȃ(Εέ);γ=Π.В;ΕϏ(true);ў=γ;μ(Σ.Ʒ);Ιδ();Βά=true;Βν=0;for(int i=ǂ.Count-1;i>=0;i--)
if(ƴ(ǂ[i],false)) Βν++;if(Βν>0) Λ="Started with damage";}void Ѓ(){if(γ==Π.В){γ=Π.Ŷ;Λ="Job paused";}Τ(Α.ь);ў=γ;ΒΔ(false,0,0,0,0);ΔΓ();Εϊ(new Vector3(),false);Ζο();ΓЋ(ChargeMode.Auto);ΓЍ(false);Ζζ(true);Ċ(Α.ь);ŉ(false,false,0,0);ǘ(ω,false);ǘ(њ,true);ȃ(true);ΒІ=false;Αϊ();
ΑΓ=false;ΓІ();Βά=false;Αβ=false;Αλ=false;if(ό!=Σ.Main&&ό!=Σ.Αφ&&ό!=Σ.ΓΛ&&ό!=Σ.ΒΕ)
μ(Σ.Main);}void Ĥ(){ε Я=ΓΪ();if(Μ==Κ.ή){if(!Ζ.λ||!Ο.λ) return;ΓА();bool Δτ=Vector3.Distance(δ,Ο.Ε)<Vector3.Distance(δ,Ζ.Ε);if(Ɲ==Π.ū) Δτ=true;if(Ɲ==Π.Ŭ) Δτ=false;if(Δτ){Ÿ(Ο,Π.ū);switch(Я){case ε.ΓΞ:Τ(Α.Ī);break;case ε.ĉ:Τ(Α.у);break;case ε.Ț:Τ(Α.ȁ);break;default:Τ(Α.Ǯ);break;
}}else{Ÿ(Ζ,Π.Ŭ);switch(Я){case ε.ΓΟ:Τ(Α.Ī);break;case ε.ț:Τ(Α.ȁ);break;case ε.ĉ:Τ(Α.у);break;default:Τ(Α.Ǯ);break;}}}else{if(γ!=Π.Ŷ&&γ!=Π.Αψ) return;bool ΛΫ=γ==Π.Αψ;ΓА();bool Δϋ=Ƶ(false)&&Ο.λ;switch(Я){case ε.ț:Τ(Δϋ?Α.Ħ:Α.Ć);break;case ε.ĉ:Τ(Δϋ?Α.И:Α.у);break;case ε.Ț:Τ(Δϋ?Α.ȁ:Α.Ȥ);break;
case ε.Αυ:{if(ė!=б||ΛΫ) Τ(Α.Ƞ);else Τ(Α.ŵ);}break;default:break;}}}void ń(){if(γ==Π.э&&!Ο.λ) return;if(Μ==Κ.ή&&(!Ζ.λ||!Ο.λ)) return;Λ="Move to job";ε Я=ΓΪ();if(Μ==Κ.ή){Ÿ(Ζ,Π.Ŭ);switch(Я){case ε.ț:Τ(Α.ȁ);break;case ε.ĉ:Τ(Α.у);break;case ε.ΓΟ:return;default:Τ(Α.Ǯ);break;
}Ċ(Α.Ī);}else{switch(Я){case ε.ț:Τ(Α.у);break;case ε.ĉ:Τ(Α.у);break;case ε.Ț:Τ(Α.Ȥ);break;case ε.Αυ:Τ(Α.Ƞ);break;default:break;}if(γ==Π.э) Ċ(Α.у);else Ċ(Α.Ģ);ΒІ=true;}ΓЄ();μ(Σ.ΔΒ);Ƽ(Ȱ,false);ў=Π.ΑЋ;}void ΑЀ(){if(!Ο.λ) return;Λ="Move home";ε Я=ΓΪ();if(Μ==Κ.ή){Ÿ(Ο,Π.ū);switch(Я)
{case ε.ĉ:Τ(Α.И);break;case ε.Ț:Τ(Α.ȁ);break;case ε.ΓΞ:return;default:Τ(Α.Ǯ);break;}Ċ(Α.Ī);}else{if(з(MyShipConnectorStatus.Connected)!=null) return;if(з(MyShipConnectorStatus.Connectable)!=null){Τ(Α.Ń);Ċ(Α.Ȥ);return;}switch(Я){case ε.ț:Τ(Α.Ħ);break;case ε.ĉ:Τ(Α.И);break;case ε.Ț:Τ(Α.И);break;
case ε.Αυ:Τ(Α.ħ);break;default:break;}Ċ(Α.Ȥ);}ΓЄ();μ(Σ.ΔΒ);Ƽ(Ȱ,false);ў=Π.ΑЊ;}void Τ(Α ΕΖ){if(ΕΖ==Α.ь) Αε=Α.ь;if(Αε!=Α.ь&&Ϊ==Αε&&ΕΖ!=Αε){Ѓ();return;}Е=true;Ϊ=ΕΖ;}void Ċ(Α Є){this.Αε=Є;}ǭ Λη(int ΑΟ,bool ǻ){if(ǻ){Ǥ=null;Ѐ=0;ύ=0;}if(Ȕ==ю.ƿ){int ŝ=ΑΟ+1;
ύ=(int) Math.Floor(Й(ΑΟ,Ї));if(ύ%2==0) Ѐ=ΑΟ-(ύ*Ї);else Ѐ=Ї-1-(ΑΟ-(ύ*Ї));if(ύ>=Ю) return ǭ.Ατ;else return ǭ.ΑЂ;}else if(Ȕ==ю.ΕЁ){if(Ǥ==null) Ǥ=new int[]{0,-1,0,0};int ΑΜ=(int) Math.Ceiling(Ї/2f);int ΑΛ=(int) Math.Ceiling(Ю/2f);int Δω=(int) Math.Floor(Ї/2f);
int Δψ=(int) Math.Floor(Ю/2f);int ΑΠ=0;while(Ǥ[2]<Math.Pow(Math.Max(Ї,Ю),2)){if(ΑΠ>200) return ǭ.ΘЅ;ΑΠ++;Ǥ[2]++;if(-ΑΜ<Ѐ&&Ѐ<=Δω&&-ΑΛ<ύ&&ύ<=Δψ){if(Ǥ[3]==ΑΟ){this.Ѐ=Ѐ-1+ΑΜ;this.ύ=ύ-1+ΑΛ;return ǭ.ΑЂ;}Ǥ[3]++;}if(Ѐ==ύ||(Ѐ<0&&Ѐ==-ύ)||(Ѐ>0&&Ѐ==1-ύ))
{int ΛΘ=Ǥ[0];Ǥ[0]=-Ǥ[1];Ǥ[1]=ΛΘ;}Ѐ+=Ǥ[0];ύ+=Ǥ[1];}}return ǭ.Ατ;}void ΙЂ(){if(Ϊ==Α.Ć){if(Е){Ж=0;if(ė!=б) ś=0;ė=б;}if(Ж==0){int Ǌ=Θφ(б);if(Ǌ<0){if(!Αω()){Τ(Α.ħ);return;}if(!Δβ||(Ǌ==-2)!=ΒΨ){Δβ=true;ΒΨ=Ǌ==-2;ΗΑ=!ΒΨ?δ:Ζ.Ε-ǟ*ν+
Ζ.Ё*(Ї*ǿ/2+Ť/2+ν/2);}и(ΗΑ,10);Λ=ΒΨ?"Waiting for a free hole":"Waiting for the controller";return;}Δβ=false;if(Ǌ!=б){б=Ǌ;ś=0;ė=б;}ǭ ί=Ιν(б);if(ί==ǭ.Ατ){γ=Π.Γά;Λ="Job done";if(Ľ&&Ο.λ){Τ(Α.Ħ);Ċ(Α.Ȥ);ў=Π.ΑЊ;}else{Τ(Α.Ű);Ċ(Α.Ģ);ў=Π.ΑЋ;}return;
}if(ί==ǭ.ΑЂ){Ж=1;ǘ(ω,true);ǎ=ć(Ѐ,ύ,true);и(ǎ,10);л(Ζ.θ,Ζ.ι,Ζ.Ё,false);}}else{if(Д>=0&&(ΖΨ()?ȉ>=0&&ȉ!=Д:Γλ(Д,true))){Ж=0;return;}if(в<wpReachedDist){Τ(Α.ŵ);return;}}}if(Ϊ==Α.ŵ){if(Е){ǘ(ω,true);ȃ(false);ǎ=ć(Ѐ,ύ,false);и(Γδ(ǎ,0),0);л(Ζ.θ,Ζ.ι,Ζ.Ё,false);
Ж=1;ђ=0;т=0;ΒΠ=0;ΔЅ=-1;Ţ=н;ΔϏ=true;}if(!Αω()){Τ(Α.ħ);return;}if(Ƶ(true)){ΗЇ=Ą("","ORE",Щ.ŀ);if((д==Г.Γέ||д==Г.Αϋ||
д==Г.Γμ||д==Г.ΑЅ)&&Μ!=Κ.ǫ)
Τ(Α.Ʊ);else if((д==Г.ΕЅ||д==Г.ΓΧ)&&Μ!=Κ.ǫ)
Τ(Α.ΔΘ);else
Τ(Α.ħ);return;}й=Vector3.Distance(δ,ǎ);if(й>ś){ś=й;ΔϏ=false;}if(Μ==Κ.ǫ&&ΕБ()==MyDetectedEntityType.SmallGrid)
т+=2;else т-=2;т=Math.Max(100,Math.Min(400,т));if(Ж>0&&Ж<т){if(й>ђ){if(т>150) ђ=й;else ђ=(float) Math.Ceiling(й);Ж=1;}else Ж++;}else{if(Ж>0){Λ="Ship stuck! Retrying";ђ=й;Ж=0;ŉ(false,true,0,ν*sensorRange);}и(Γδ(ǎ,Math.Max(0,ђ-ν)),Γη(false));if(в<=wpReachedDist/2){Ж=1;ђ=0;}
return;}ŉ(false,true,ν*sensorRange,0);Vector3 ΙЎ=ǎ+ǟ*й;bool Ηθ=false;if(Vector3.Distance(ΙЎ,δ)>0.3f){Vector3 ΚΞ=ǎ+ǟ*(й+0.1f);и(ΚΞ,4);Ηθ=true;}else{float Ǽ=Γη(true);Vector3 ΗΘ=Γδ(ǎ,Math.Max(н+1,й+1));Ȃ(true,false,false,ΗΘ,ΗΘ-ǎ,Ǽ,Ǽ);}bool Δζ=false;if(ћ==Ǖ.ΓΡ||ћ==Ǖ.ΓΠ)
{if(!Ηθ){float ΕΙ=0;foreach(IMyTerminalBlock Ș in ω)
ΕΙ+=ΘϏ(Ș,"","",ћ==Ǖ.ΓΠ?new string[]{"STONE"}:null);if(ΕΙ>ΔЅ||й<н||ΔϏ){ΔΟ=0;ΒΠ=й;Ţ=(int)(Math.Max(Ţ,ΒΠ)+ν/2);}else{Δζ=й-ΒΠ>2&&ΔΟ>=20;ΔΟ++;}ΔЅ=ΕΙ;}}else Δζ=й>=Ţ;if(ė!=б){Τ(Α.Ƞ);й=0;return;}if(Δζ){б++;Τ(Α.Ƞ);й=0;return;}}if(Ϊ==Α.Ƞ||Ϊ==Α.ħ||Ϊ==Α.Ʊ)
{if(Е){ǎ=ć(Ѐ,ύ,false);л(Ζ.θ,Ζ.ι,Ζ.Ё,false);ǘ(ω,Ƅ);ȃ(false);ђ=Vector3.Distance(δ,ǎ);ŉ(false,true,0,ν*sensorRange);}и(ǎ,Γη(false));if(Vector3.Distance(δ,ǎ)>=ђ+5){ǘ(ω,false);ȃ(true);Λ="Can´t return!";}if(в<wpReachedDist){if(Ϊ==Α.Ƞ&&ΒІ) Τ(Α.Ű);if(Ϊ==Α.Ƞ) Τ(Α.Ć);
if(Ϊ==Α.Ʊ) Τ(Α.ΑϏ);if(Ϊ==Α.ħ){if(Ο.λ) Τ(Α.Ħ);else{Ѓ();ń();Λ="Can´t return, no dock found";}}return;}}if(Ϊ==Α.И||Ϊ==Α.у){if(Ϊ==Α.у&&γ==Π.В&&Μ!=Κ.ή){if(!Αω()||Ƶ(true)){Τ(Α.И);return;}}bool ΕΠ=false;bool ΓΕ=false;bool Ζύ=false;float ž=0;bool ΗІ=false;ά έ=null;
if(Е){if(Ϊ==Α.И||Μ==Κ.ή){if(Μ!=Κ.ή){if(ċ(1)) ΓΝ();else{Ǡ=null;Ȳ=0;ΑΓ=!ΓΦ(1);}}ά α=ы();ξ=new ΓЃ();ξ.η=α;ξ.ΑΑ=followPathDock*ν;ξ.Εο=useDockDirectionDist*ν;ξ.ž=10;ξ.ƣ.Add(α.Ε);Vector3 Ž=new Vector3();if(ȝ(α,dockDist*ν,true,out Ž))
ξ.ƣ.Add(Ž);else ξ.ΑΑ*=1.5f;if(Μ==Κ.ή){if(α==Ο) ξ.Βζ=Ζ.Ε;if(α==Ζ) ξ.Βζ=Ο.Ε;ξ.Δρ=dockDist*ν*1.1f;}}else if(Ϊ==Α.у){ξ=new ΓЃ();ξ.η=Ζ;ξ.ΑΑ=followPathJob*ν;ξ.Εο=useJobDirectionDist*ν;ξ.ž=10;ξ.Βζ=Ο.Ε;ξ.Δρ=dockDist*ν*1.1f;ξ.ƣ.Add(Ζ.Ε);if(γ==Π.э){if(!Ο.λ||Ψ.Count==0){Ѓ();return;}
float ΚΥ=Vector3.Distance(Ψ.First().Ε,Ο.Ε);float ΚΦ=Vector3.Distance(Ψ.Last().Ε,Ο.Ε);if(ΚΥ<ΚΦ) ξ.η=Ψ.Last();else ξ.η=Ψ.First();}}Αέ=new Vector3();ΗІ=!ΙΟ(δ);ǘ(ω,false);ȃ(true);к=-1;double Ȉ=-1;for(int i=Ψ.Count-1;i>=0;i--){if(Vector3.Distance(Ψ[i].Ε,ξ.Βζ)<=ξ.Δρ) continue;
double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȉ==-1||ϊ<Ȉ){к=i;Ȉ=ϊ;}}Ȓ=ΘБ(ξ.η.Ε,к);Ŏ=null;}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&!ƾ()){ά ΔΥ=ы();ΑΓ=!ΓΦ(1);if(ы()!=ΔΥ){Τ(Α.И);return;}}if(Ϊ==Α.И&&Μ!=Κ.ή&&!Е&&ċ(1)&&ΓΝ()){Τ(Α.И);return;}bool Ηζ=false;if(Ϊ==Α.И&&!Е&&к>=0&&к<Ψ.Count)
{Vector3 ǌ=Ψ[к].Ε+ΖΓ(к,Ȓ);int ΕΗ=к+Ȓ;Vector3 ŝ=Ȓ!=0&&ΕΗ>=0&&ΕΗ<Ψ.Count?Ψ[ΕΗ].Ε:ξ.η.Ε;if(ΙΚ(ǌ,ŝ)){if(!Βί){Βί=true;ΗΝ=Ŵ(ǌ)?δ:ǌ;}и(ΗΝ,0);л(ξ.η.θ,ξ.η.ι,ξ.η.Ё,false);Λ="Waiting for dock clearance";return;}Βί=false;Ηζ=ċ(1)&&(Ŵ(δ)||Ŵ(ŝ));}Θρ(Ψ,Ȓ,ξ.ƣ,ξ.ΑΑ,Е,ref Ж);
for(int i=0;i<ξ.ƣ.Count;i++){float ϊ=Vector3.Distance(δ,ξ.ƣ[i]);if(ϊ<=ξ.ΑΑ) ΕΠ=true;if(ϊ<=ξ.Εο) ΓΕ=true;}if(Ηζ) ΓΕ=true;if(ΓΕ) ž=ξ.ž;float Λμ=Ŏ!=null?Ŏ.Ч:Ĝ;float Λα=(float) Math.Max(Ĝ*0.1f*ν,wpReachedDist);if((в<Λα)||Е){if(!Е) к+=Ȓ;if(Ȓ==0||к>Ψ.Count-1||к<0)
ΕΠ=true;else{Ες=Ȓ>0?Ψ.Count-1-к:к;έ=Ψ[к];Ŏ=έ;if(к>=1&&к<Ψ.Count-1)
Αέ=έ.Ε-Ψ[к-Ȓ].Ε;else Ŏ=null;Ђ=έ.Ε+ΖΓ(к,Ȓ);Ζύ=true;}}if(ΓΕ)
л(ξ.η.θ,ξ.η.ι,ξ.η.Ё,false);else if(ΗІ)
ΓΜ(ξ.η.θ,10,true);else if(Ζύ&&έ!=null){if(Ȓ>0) л(έ.θ,έ.ι,έ.Ё,90,false);else л(έ.θ,-έ.ι,-έ.Ё,90,false);}Ȃ(true,false,true,Ђ,Αέ,Ŏ==null?0:Ŏ.Ч,ž);if(ΕΠ){Ες=0;if(Ϊ==Α.И&&!Θτ()){и(Ђ,0);return;}if(Ϊ==Α.И&&ΑΓ){и(Ђ,0);Λ="Waiting for a free dock";return;}
if(Ϊ==Α.И||Μ==Κ.ή){Τ(Α.ȁ);return;}if(Ϊ==Α.у&&ΒІ){Τ(Α.Ű);return;}if(Ϊ==Α.у){Τ(Α.Ć);return;}}}if(Ϊ==Α.ȁ||Ϊ==Α.ƻ){if(Μ!=Κ.ή&&ƾ()){if(!ċ(1)){Τ(Α.И);return;}}else if(Μ!=Κ.ή){ά ΔΥ=ы();if(!ΓΦ(2)||ы()!=ΔΥ){Τ(Α.И);return;}}ά α=ы();if(Е){if(!ȝ(α,dockDist*ν,true,out Ђ))
{ȡ(х.Ư);Ѓ();return;}и(Ђ,0);ΓΜ(α.θ,90,true);}if(в<followPathDock*ν&&в!=-1){и(Ђ,10);л(α.θ,α.ι,α.Ё,false);}if(з(MyShipConnectorStatus.Connectable)!=null||з(MyShipConnectorStatus.Connected)!=null){Τ(Α.Ń);return;}if(в<wpReachedDist/2&&в!=-1){Τ(Α.Ŀ);return;}}if(Ϊ==Α.Ŀ||Ϊ==Α.Ģ)
{if(Е){if(Ϊ==Α.Ŀ){ά α=ы();if(!ȝ(α,dockDist*ν,true,out Ђ)){ȡ(х.Ư);Ѓ();return;}Ȃ(true,true,false,Ђ,0);л(α.θ,α.ι,α.Ё,10,false);}if(Ϊ==Α.Ģ){л(Ζ.θ,Ζ.ι,Ζ.Ё,0.5f,false);Ђ=Ζ.Ε;Ȃ(true,true,false,Ђ,0);}}if(Βο){ΒΔ(false,0,0,0,0);if(Ϊ==Α.Ŀ) Τ(Α.Ń);if(Ϊ==Α.Ģ) Ѓ();return;
}}if(Ϊ==Α.Ń){if(з(MyShipConnectorStatus.Connected)!=null){if(Μ==Κ.ή) Τ(Α.Ī);else Τ(Α.Ȥ);return;}ά α=ы();if(Е){т=0;ΚΧ=DateTime.Now;Ж=0;л(α.θ,α.ι,α.Ё,false);}Vector3I Δφ=new Vector3I((int) α.о.X,(int) α.о.Y,(int) α.о.Z);IMySlimBlock ΖЏ=Me.CubeGrid.GetCubeBlock(Δφ);float ΗΠ=dockingSpeed;
float Κο=dockingSpeed*5;float Ζϋ=Math.Max(1.5f,Math.Min(5f,ν*0.15f));if(!ȝ(α,0,true,out Ђ)||!ȝ(α,Ζϋ,true,out Αέ)||
ΖЏ==null||!ΖЏ.FatBlock.IsFunctional){ȡ(х.Ư);Ѓ();return;}if(т==1||(Vector3.Distance(δ,Ђ)<=Ζϋ*1.1f&&!Е))
т=1;else{Vector3 ΛΠ=ф(Ϋ,Αέ-δ);Vector3 Δυ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΚЉ=Αχ(ΛΠ,Δυ,null);ΗΠ=Math.Min(Κο,ΚЉ);}Ȃ(true,false,false,Ђ,Ђ-δ,dockingSpeed,ΗΠ);if(Е) ђ=(float) в;IMyShipConnector Δα=з(MyShipConnectorStatus.Connectable);if(Δα!=null){Ȃ(false,false,false,Ђ,0);
if(Ж>0) Ж=0;Ж--;if(Ж<-5){Δα.Connect();if(Δα.Status==MyShipConnectorStatus.Connected){if(Μ==Κ.ή) Τ(Α.Ī);else Τ(Α.Ȥ);ΔΓ();Ƽ(Ȱ,true);return;}}}else{float ΗΜ=(float) Math.Round(в,1);if(ΗΜ<ђ){Ж=-1;ђ=ΗΜ;}else Ж++;if(Ж>20){Τ(Α.ƻ);return;}}}if(Ϊ==Α.Ȥ||Ϊ==Α.Ī||
Ϊ==Α.ΒΛ||Ϊ==Α.Αύ||Ϊ==Α.ł){bool ΔΝ=false,ΔΞ=false;if(Μ==Κ.ή){if(ы()==Ο) ΔΝ=true;else if(ы()==Ζ) ΔΞ=true;}if(Е){Αβ=false;if(з(MyShipConnectorStatus.Connected)==null){Τ(Α.Ǯ);return;}ΔΓ();if(ΔΝ) ΒΘ(Ј.ǳ);if(ΔΞ) ΒΘ(Љ.ǳ);ŧ=false;Č=false;ē=false;Αν=false;}if(з(MyShipConnectorStatus.Connected)==null){Ѓ();ȡ(х.ΖΡ);return;}
if(γ!=Π.В||џ==-1||ȅ==ї.None) Č=true;else if(Ȧ>=100f) Č=true;else if(Ȧ<=99f) Č=false;if(γ!=Π.В||Ǎ==-1||ǜ.Count==0) ē=true;else if(Ĕ>=100f) ē=true;else if(Ĕ<=99) ē=false;if(γ!=Π.В||п==-1||Ĺ.Count==0) Αν=true;else Αν=Ũ>=п;ź і=null;if(ΔΝ) і=Ј;
if(ΔΞ) і=Љ;if(і!=null&&(і.Р==ρ.ΑЏ||і.Р==ρ.ΑЎ)) Č=true;if(і!=null&&(і.Р==ρ.Γϋ)) if(!ŧ) Č=false;if(і!=null&&(і.Р==ρ.ΓϏ||і.Р==ρ.Γύ)) ē=true;if(і!=null&&(і.Р==ρ.Γώ)) if(!ŧ) ē=false;if(ĕ){ChargeMode ΖЇ=Č?ChargeMode.Auto:ChargeMode.Recharge;if(і!=null&&(і.Р==ρ.ΑЎ||і.Р==ρ.ΑЏ))
ΖЇ=ChargeMode.Discharge;ΓЋ(ΖЇ);ΓЍ(!ē);}if(!ŧ){if(Μ==Κ.ή)
ŧ=γ!=Π.В||Θυ(Е,true)||Αλ;else
ŧ=γ!=Π.В||ΖΣ();}else{if(!Č) Τ(Α.ł);if(!ē) Τ(Α.Αύ);if(!Αν) Τ(Α.ΒΛ);Е=false;}if(ŧ&&Č&&ē&&Αν){ΓЋ(ChargeMode.Auto);ΓЍ(false);if(γ==Π.В){if(Μ==Κ.ή){if(ы()==Ο) ΒΘ(Ј.Ƿ);else if(ы()==Ζ) ΒΘ(Љ.Ƿ);if(ы()==Ο) Ÿ(Ζ,Π.Ŭ);else Ÿ(Ο,Π.ū);}}Τ(Α.Ǯ);return;}}if(Ϊ==Α.Ǯ){if(Απ){if(!ΕЃ()) return;
Απ=false;Τ(Α.Ǯ);return;}if(Е){if(!ΕЃ()){Απ=true;return;}IMyShipConnector ȩ=з(MyShipConnectorStatus.Connected);if(ȩ==null){Τ(Α.у);return;}IMyShipConnector ζ=ȩ.OtherConnector;Ζε(ȩ,false);Ƽ(Ȱ,false);ά α=null;ά ќ=Μ==Κ.ή?Ο:ы();if(Vector3.Distance(ȩ.GetPosition(),ќ.Ε)<5f&&Ο.λ) α=ќ;
if(Vector3.Distance(ȩ.GetPosition(),Ζ.Ε)<5f&&Ζ.λ) α=Ζ;if(α!=null){if(!ȝ(α,dockDist*ν,true,out Ђ)){ȡ(х.Ư);Ѓ();return;}и(Ђ,5);л(α.θ,α.ι,α.Ё,false);}else и(δ+ζ.WorldMatrix.Forward*dockDist*ν,5);if(γ==Π.В) ȡ(х.ΒΒ);}if(в<wpReachedDist){ǘ(њ,true);Τ(Α.у);return;}}if(Ϊ==Α.Ű)
{if(Е){ȃ(true);ǘ(ω,false);Ђ=Ζ.Ε;и(Ђ,20);л(Ζ.θ,Ζ.ι,Ζ.Ё,false);}if(в<wpReachedDist/2){Τ(Α.Ģ);return;}}if(Ϊ==Α.Ħ){if(Е){ȃ(true);ǘ(ω,false);int Ы=-1;double Ȉ=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,δ);if(Ȉ==-1||ϊ<Ȉ){Ы=i;Ȉ=ϊ;}}
if(Ы==-1){Τ(Α.И);return;}Ђ=Ψ[Ы].Ε;и(Ђ,10);л(Ζ.θ,Ζ.ι,Ζ.Ё,false);}if(в<wpReachedDist){Τ(Α.И);return;}}if(Ϊ==Α.ΑϏ){bool Ϗ=false;if(Е){ȃ(true);if((д==Г.Γέ||д==Г.Αϋ)&&ΑЇ()&&
Ů(ǟ,Ϋ.GetNaturalGravity())<25&&Ї>=2&&Ю>=2){Vector3 ΑΕ=δ;if(Ѐ>0&&ύ<Ю-1) ΑΕ=ć(Ѐ-1,ύ+1,true);else if(Ѐ<Ї-1&&ύ<Ю-1) ΑΕ=ć(Ѐ+1,ύ+1,true);else if(Ѐ<Ї-1&&ύ>0) ΑΕ=ć(Ѐ+1,ύ-1,true);else if(Ѐ>0&&ύ>0) ΑΕ=ć(Ѐ-1,ύ-1,true);else Ϗ=true;
if(!Ϗ) и(ΑΕ,10);}else Ϗ=true;}if(в<wpReachedDist/2) Ϗ=true;if(Ϗ){Τ(Α.ΒΚ);return;}}if(Ϊ==Α.ΔΘ||Ϊ==Α.ΒΚ){if(Е){Ȃ(true,true,false,δ,0);ǘ(ω,false);ȃ(true);Ж=-1;т=д==Г.Γμ||д==Г.ΑЅ?0:-1;}bool Œ=!Αω();int ΒЍ=Ą("STONE","ORE",Щ.ŀ);if(д==Г.ΓΧ||д==Г.ΑЅ||д==Г.Αϋ)
ΒЍ+=Ą("ICE","ORE",Щ.ŀ);bool Κί=ΒЍ>0;bool Δο=false;if(т>=0){float Ż=(float) Math.Sin(ΕЇ(т))*ǿ/3f;float ΙА=(float) Math.Cos(ΕЇ(т))*ș/3f;Vector3 ΚΖ=ć(Ѐ,ύ,true)+ΔΖ(ǟ,ę*-1,new Vector3(Ż,ΙА,0));и(ΚΖ,0.3f);if(в<Math.Min(ǿ,ș)/10f) т+=5f;if(т>=360) т=0;}if(Ж==-1||ΒЍ<Ж){Ж=ΒЍ;ђ=0;}
else{ђ++;if(ђ>50) Δο=true;}if(!Κί||Œ||Δο){if(!Œ){int ΛΦ=Ą("","ORE",Щ.ŀ);if(Ƶ(true)) Œ=true;else if(100-(Й(ΛΦ,ΗЇ)*100)<minEjection) Œ=true;else ȡ(х.ΒΒ);}if(Δο&&Œ) Λ="Ejection failed";if(Ϊ==Α.ΒΚ){if(Œ){if(Ο.λ) Τ(Α.Ħ);else{Ѓ();ń();Λ="Can´t return, no dock found";}}
else Τ(Α.Ć);}else if(Œ) Τ(Α.ħ);else Τ(Α.ŵ);return;}}Е=false;}void Θρ(List<ά>ľ,int ς,List<Vector3>Αθ,float ȕ,bool ǻ,ref int Ǳ){if(ǻ){for(int i=0;i<ľ.Count;i++) ľ[i].Ч=0;Ǳ=-1;return;}if(ς==0) return;int ǥ=ς*-1;if(Ǳ==-1) Ǳ=ǥ>0?1:ľ.Count-2;int ΑΠ=0;while(Ǳ>=1&&Ǳ<ľ.Count-1)
{if(ΑΠ>50) return;ΑΠ++;try{if((ǥ<0&&Ǳ>=1)||(ǥ>0&&Ǳ<=ľ.Count-2)){ά ο=ľ[Ǳ];bool ΗЃ=false;for(int j=0;j<Αθ.Count;j++)
if(Vector3.Distance(ο.Ε,Αθ[j])<=ȕ){ΗЃ=true;break;}if(!ΗЃ){ά ΒЂ=ľ[Ǳ-ǥ];ά ŝ=ľ[Ǳ+ǥ];Vector3 ΛΡ=ο.Ε-ŝ.Ε;Vector3 Εθ=ΒЂ.Ε-ο.Ε;Vector3 ΚЃ=ο.Ε+Vector3.Normalize(ΛΡ)*Εθ.Length();Vector3 ΚΤ=ΒЂ.Ε-ΚЃ;Vector3 Βυ=ň(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,ΚΤ);Vector3 Κξ=ň(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,Εθ);
Vector3 Βχ=ň(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,ο.φ);ο.Ч=(float) Math.Sqrt(Math.Pow(ΒЂ.Ч,2)+Math.Pow(Αχ(-Κξ,Βχ,ο),2));for(int j=0;j<Αθ.Count;j++)
if(Vector3.Distance(ΒЂ.Ε,Αθ[j])<=ȕ){Vector3 Βψ=ň(ς>0?ο.ι:ο.ι*-1,ο.θ*-1,Αθ[j]-ο.Ε);float ΛΗ=Αχ(-Βψ,Βχ,ο);ο.Ч=Math.Min(ο.Ч,ΛΗ)/2f;}if(Βυ.Length()==0) Βυ=new Vector3(0,0,1);Vector3 Κν=ň(ο.ι,ο.θ*-1,ο.φ);float ΛΛ=Ʋ(-Βυ,Κν,ο);float ΒΜ=Й(ΛΛ,ť);float Εε=(float) Math.Sqrt(Й(Βυ.Length()*1.0f,0.5f*ΒΜ));
ο.Ч=Math.Min(ο.Ч,(Εθ.Length()/Εε)*Ȅ);}}}catch{return;}Ǳ+=ǥ;}Ǳ=-1;}void ΔΗ(bool ǻ){if(ǻ){ΑΡ=0;return;}if(Μ==Κ.ή) return;float Ϗ=б*Math.Max(1,н);if(ė==б) Ϗ+=Math.Min(н,й);float ΛΥ=Ї*Ю*Math.Max(1,н);ΑΡ=Math.Max(ΑΡ,(float) Math.Min(Ϗ/ΛΥ*100.0,100));}MyDetectedEntityType ΕБ()
{try{if(Ȟ(ψ,true)&&!ψ.LastDetectedEntity.IsEmpty()) return ψ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Γη(bool ȍ){if(Μ==Κ.ǫ&&ΕБ()==MyDetectedEntityType.None&&!ƴ(ψ,true))
return fastSpeed;else return ȍ?ƭ:Ƭ;}void Ιω(){float Ηϋ=0,ΗΖ=0,Ŗ=0,ΕΚ=0;for(int i=0;i<ī.Count;i++){IMyBatteryBlock Ǉ=ī[i];if(!Ȟ(Ǉ,true)) continue;Ηϋ+=Ǉ.MaxStoredPower;ΗΖ+=Ǉ.CurrentStoredPower;Ŗ+=Ǉ.CurrentInput;ΕΚ+=Ǉ.CurrentOutput;}Ȧ=(float) Math.Round(Й(ΗΖ,Ηϋ)*100,1);
if(Ŗ>=ΕΚ) ȅ=ї.ł;else ȅ=ї.ΕЈ;if(Ŗ==0&&ΕΚ==0||Ȧ==100.0) ȅ=ї.ь;if(ī.Count==0) ȅ=ї.None;}void ΙϏ(){float ΗΦ=0;for(int i=0;i<ǜ.Count;i++){IMyGasTank ΘΣ=ǜ[i];if(!Ȟ(ΘΣ,true)) continue;ΗΦ+=(float) ΘΣ.FilledRatio;}Ĕ=Й(ΗΦ,ǜ.Count)*100f;}void ΙЊ(){Ũ=0;try{for(int i=0;i<Ĺ.Count;i++)
{IMyReactor ΕΡ=Ĺ[i];if(!Ȟ(ΕΡ,true)) continue;List<MyInventoryItem>Ќ=new List<MyInventoryItem>();ΕΡ.GetInventory(0).GetItems(Ќ);float Ћ=0;for(int j=0;j<Ќ.Count;j++){MyInventoryItem ώ=Ќ[j];if(ώ.Type.SubtypeId.ToUpper()=="URANIUM"&&ώ.Type.TypeId.ToUpper().Contains("_INGOT"))
Ћ+=(float) ώ.Amount;}if(Ћ<Ũ||i==0){Ũ=Ћ;ΔЍ=ΕΡ.CustomName;}}}catch(Exception e){Ė=e;}}void Ιϊ(){ȑ=0;Ȋ=0;try{for(int i=0;i<Ƃ.Count;i++){IMyTerminalBlock Ω=Ƃ[i];if(!Ȟ(Ω,true)) continue;Ȋ+=(float) Ω.GetInventory(0).CurrentVolume;ȑ+=(float) Ω.GetInventory(0).MaxVolume;
}Βτ=(float) Math.Min(Math.Round(Й(Ȋ,ȑ)*100,1),100.0);}catch(Exception e){Ė=e;}}void ΙЀ(){try{Ƕ.Clear();for(int i=0;i<Ƃ.Count;i++){IMyTerminalBlock Ω=Ƃ[i];if(!Ȟ(Ω,true)) continue;Εω(Ω,Щ.ų);}if(д!=Г.Γϊ){for(int i=0;i<Ō.Count;i++){IMyTerminalBlock Ω=Ō[i];if(!Ȟ(Ω,true)) continue;
Εω(Ω,Щ.ΙΝ);}}Ιπ(Ƕ);}catch(Exception e){Ė=e;}}void Θσ(){if(Βά){if(ΕА().Count>Βν){Βά=false;if(ѓ!=ъ.Ƴ){Ѓ();if(ѓ==ъ.Γβ) ń();if(ѓ==ъ.ΒΑ)
if(Ο.λ) ΑЀ();else ń();}Λ="Damage detected";}}}bool Αω(){if(!Βξ) return true;if(γ==Π.В){if(џ>0&&ȅ!=ї.None)
if(Ȧ<=џ){Λ="Low energy! Move home";return false;}if(п>0&&Ĺ.Count>0)
if(Ũ<=п){Λ="Low fuel: "+ΔЍ;return false;}if(Ǎ>0&&ǜ.Count>0)
if(Ĕ<=Ǎ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΕА(){List<IMyTerminalBlock>ŏ=new List<IMyTerminalBlock>();for(int i=0;i<ǂ.Count;i++){IMyTerminalBlock Ω=ǂ[i];if(ƴ(Ω,false)) ŏ.Add(Ω);}return ŏ;}bool ƴ(IMyTerminalBlock Ω,bool Εί){return(!Ȟ(Ω,Εί)||!Ω.IsFunctional);
}bool Ȟ(IMyTerminalBlock Ω,bool Εί){if(Ω==null) return false;try{IMyCubeBlock Ηά=Me.CubeGrid.GetCubeBlock(Ω.Position).FatBlock;if(Εί) return Ηά==Ω;else return Ηά.GetType()==Ω.GetType();}catch{return false;}}float ΘϏ(IMyTerminalBlock Ω,String Ǔ,String ΘΙ,String[]ȋ){float Ȫ=0;for(int i=0;i<Ω.InventoryCount;i++)
{IMyInventory Ɖ=Ω.GetInventory(i);List<MyInventoryItem>Ќ=new List<MyInventoryItem>();Ɖ.GetItems(Ќ);foreach(MyInventoryItem ώ in Ќ){if(ȋ!=null&&(ȋ.Contains(ώ.Type.TypeId.ToUpper())||ȋ.Contains(ώ.Type.SubtypeId.ToUpper())))
continue;if((Ǔ==""||ώ.Type.TypeId.ToUpper()==Ǔ)&&(ΘΙ==""||ώ.Type.SubtypeId.ToUpper()==ΘΙ))
Ȫ+=(float) ώ.Amount;}}return Ȫ;}м ΘЇ(String ΰ,String υ,Щ Я,bool ΚΟ){ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ƕ.Count;i++){м ώ=Ƕ[i];if(ώ.ΰ.ToUpper()==ΰ&&ώ.υ.ToUpper()==υ&&(ώ.ƍ==Я||Я==Щ.ŀ))
return ώ;}м ί=null;if(ΚΟ){ί=new м(ΰ,υ,0,Я);Ƕ.Add(ί);}return ί;}int Ą(String ΰ,String υ,Щ Я){return Ą(ΰ,υ,Я,null);}int Ą(String ΰ,String υ,Щ Я,String[]ȋ){int Ȫ=0;ΰ=ΰ.ToUpper();υ=υ.ToUpper();for(int i=0;i<Ƕ.Count;i++){м ώ=Ƕ[i];if(ȋ!=null&&ȋ.Contains(ώ.ΰ.ToUpper())) continue;
if((ΰ==""||ώ.ΰ.ToUpper()==ΰ)&&(υ==""||ώ.υ.ToUpper()==υ)&&(ώ.ƍ==Я||Я==Щ.ŀ))
Ȫ+=ώ.Ћ;}return Ȫ;}void Εω(IMyTerminalBlock Ω,Щ Я){for(int i=0;i<Ω.InventoryCount;i++){List<MyInventoryItem>Ќ=new List<MyInventoryItem>();Ω.GetInventory(i).GetItems(Ќ);for(int j=0;j<Ќ.Count;j++)
ΘЇ(Ќ[j].Type.SubtypeId,Ќ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Я,true).Ћ+=(int) Ќ[j].Amount;}}void Ιπ(List<м>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){м a=Ѝ[j];м b=Ѝ[j+1];if(a.Ћ<b.Ћ) Ѝ.Move(j,j+1);}}bool Ƶ(bool Λι){if(ġ&&Μ!=Κ.ή)
if(е!=-1&&ť>=е){Λ="Ship too heavy";return true;}if(Βτ>=Ȑ||Αβ){Αβ=false;Λ="Ship is full";return true;}return false;}bool ΖΣ(){String[]ȋ=null;if(!Ġ) ȋ=new string[]{"ICE"};if(Μ==Κ.ƹ) return Ą("","ORE",Щ.ų,ȋ)==0;if(Μ==Κ.ǫ) return Ą("","COMPONENT",Щ.ų,ȋ)==0;else return Ą("","",Щ.ų,ȋ)==0;
}bool ΙΠ(){List<IMyCockpit>Δΰ=new List<IMyCockpit>();χ.GetBlocksOfType(Δΰ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Δΰ.Count;i++)
if(Δΰ[i].IsUnderControl) return true;return false;}bool Θυ(bool ǻ,bool ΒЈ){IMyShipConnector О=з(MyShipConnectorStatus.Connected);if(О==null) return false;if(Vector3.Distance(Ο.Ε,О.GetPosition())<5)
return ΕЄ(Ј,ǻ,ΒЈ);if(Vector3.Distance(Ζ.Ε,О.GetPosition())<5)
return ΕЄ(Љ,ǻ,ΒЈ);return false;}bool ΕЄ(ź Ȩ,bool ǻ,bool ΒЈ){if(ǻ) Ȩ.Ιΰ();Ȩ.ΙІ();bool ѕ=false;String Ǒ="";switch(Ȩ.Р){case ρ.Γό:{Ǒ="Waiting for command";ѕ=false;break;}case ρ.Ζί:{Ǒ="Waiting for cargo";ѕ=Ƶ(true);break;}case ρ.Ζή:{Ǒ="Unloading";ѕ=ΖΣ();break;}case ρ.ΓЁ:{ѕ=true;break;}
case ρ.Γϋ:{Ǒ="Charging batteries";ѕ=Ȧ>=100f;break;}case ρ.ΑЏ:{Ǒ="Discharging batteries";ѕ=Ȧ<=25f;break;}case ρ.ΑЎ:{Ǒ="Discharging batteries";ѕ=Ȧ<=0f;break;}case ρ.Γώ:{Ǒ="Filling up hydrogen";ѕ=Ĕ>=100f;break;}case ρ.ΓϏ:{Ǒ="Unloading hydrogen";ѕ=Ĕ<=25f;break;}
case ρ.Γύ:{Ǒ="Unloading hydrogen";ѕ=Ĕ<=0f;break;}case ρ.ΓЀ:{bool Ηε=ΙΠ();if(!Ηε) Ȩ.ΕΝ=true;ѕ=Ȩ.ΕΝ&&Ηε;Ǒ="Waiting for passengers";break;}}if(!ѕ) Ȩ.ΓΥ(true);if(ѕ&&Ȩ.Γι()){ѕ=Ȩ.ΓΥ(false);Ǒ="Undocking in: "+ΑЁ((int) Math.Max(0,Ȩ.ǲ-Ȩ.ı));}if(ΒЈ) Θγ=Ǒ;return ѕ;}void Θι()
{if(!Ɗ) return;if(ω.Count<=1) return;float ΘΫ=0,ΘΪ=0;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ș=ω[i];if(ƴ(Ș,true)) continue;ΘΫ+=(float) Ș.GetInventory(0).MaxVolume;ΘΪ+=(float) Ș.GetInventory(0).CurrentVolume;}float Ζώ=(float) Math.Round(Й(ΘΪ,ΘΫ),5);for(int ş=0;ş<Math.Max(1,Math.Floor(ω.Count/10f));ş++)
{float Δύ=0,ΔЌ=0;float Ηβ=0,Ηψ=0;IMyTerminalBlock Đ=null,Ĳ=null;for(int i=0;i<ω.Count;i++){IMyTerminalBlock Ș=ω[i];if(ƴ(Ș,true)) continue;float ΕΒ=(float) Ș.GetInventory(0).MaxVolume;float Βη=Й((float) Ș.GetInventory(0).CurrentVolume,ΕΒ);if(Đ==null||Βη>Δύ){Đ=Ș;Δύ=Βη;Ηβ=ΕΒ;}
if(Ĳ==null||Βη<ΔЌ){Ĳ=Ș;ΔЌ=Βη;Ηψ=ΕΒ;}}if(Đ==null||Ĳ==null||Đ==Ĳ) return;if(checkConveyorSystem&&!Đ.GetInventory(0).IsConnectedTo(Ĳ.GetInventory(0))){if(ΔΤ>20) Λ="Inventory balancing failed";else ΔΤ++;return;}ΔΤ=0;List<MyInventoryItem>Ќ=new List<MyInventoryItem>();
Đ.GetInventory(0).GetItems(Ќ);float Αο=0;if(Ќ.Count==0) continue;MyInventoryItem Βθ=Ќ[0];String Ηι=Βθ.Type.TypeId+Βθ.Type.SubtypeId;if(!Θβ.TryGetValue(Ηι,out Αο)){if(ΙΦ(Đ.GetInventory(0),0,Ĳ.GetInventory(0),out Αο))
Θβ.Add(Ηι,Αο);else return;}float ΛΣ=((Δύ-Ζώ)*Ηβ/Αο);float ΛΤ=((Ζώ-ΔЌ)*Ηψ/Αο);int Εν=(int) Math.Min(ΛΤ,ΛΣ);if(Εν<=0) return;if((float) Βθ.Amount<Εν)
Đ.GetInventory(0).TransferItemTo(Ĳ.GetInventory(0),0,null,null,Βθ.Amount);else
Đ.GetInventory(0).TransferItemTo(Ĳ.GetInventory(0),0,null,null,Εν);}}bool ΙΦ(IMyInventory Αγ,int Β,IMyInventory ΚΣ,out float Θα){Θα=0;float ΚΓ=(float) Αγ.CurrentVolume;List<MyInventoryItem>ΔΦ=new List<MyInventoryItem>();Αγ.GetItems(ΔΦ);float ΖЂ=0;for(int i=0;i<ΔΦ.Count;i++) ΖЂ+=(float) ΔΦ[i].Amount;
Αγ.TransferItemTo(ΚΣ,Β,null,null,1);float Θί=ΚΓ-(float) Αγ.CurrentVolume;List<MyInventoryItem>ΔΚ=new List<MyInventoryItem>();Αγ.GetItems(ΔΚ);float Ζχ=0;for(int i=0;i<ΔΚ.Count;i++) Ζχ+=(float) ΔΚ[i].Amount;if(Θί==0f||!Γν(0.9999,ΖЂ-Ζχ,1.0001))
return false;Θα=Θί;return true;}void Ιώ(){Vector3 ΑΙ=Ђ+ļ;Vector3 ȗ=ΑΙ-δ;if(ȗ.Length()==0) ȗ=new Vector3(0,0,-1);Vector3 Βψ=ф(Ϋ,ȗ);Vector3 ΘΤ=Vector3.Normalize(Βψ);Vector3 ƌ=ф(Ϋ,Ϋ.GetNaturalGravity());float ΛΒ=ΕΛ>0?Math.Max(0,1-Ů(ȗ,ΗΨ)/5):0;float Ч=(float) Math.Min((ΔА>0?ΔА:1000f),Math.Max(Αχ(-Βψ,ƌ,null),ΕΛ*ΛΒ));
if(!ΘΩ) Ч=0;if(ΘΓ) Ч=Math.Max(0,1-ΒΪ/ΒΝ)*Ч;if(generalSpeedLimit>0) Ч=Math.Min(generalSpeedLimit,Ч);if(ƕ) Ч=0;if(ǒ>=0) Ч=Math.Min(Ч,ǒ);if(ΘΔ) Ч*=(float) Math.Min(1,Й(ȗ.Length(),wpReachedDist)/2);Vector3 Ηυ=ф(Ϋ,Ϋ.GetShipVelocities().LinearVelocity);float ΙЏ=(float)(Math.Max(0,15-Ů(-ΘΤ,-Ηυ))/15)*0.85f+0.15f;
Εγ+=Math.Sign(ΙЏ-Εγ)/10f;Vector3 ĭ=ΘΤ*Ч*Εγ-(Ηυ);Vector3 ΔΡ=Γζ(ĭ,null);if(ΒА&&в>0.1f){ĭ.X*=ΓЏ(ĭ.X,ref ΒБ.X,1f,ΔΡ.X,20);ĭ.Y*=ΓЏ(ĭ.Y,ref ΒБ.Y,1f,ΔΡ.Y,20);ĭ.Z*=ΓЏ(ĭ.Z,ref ΒБ.Z,1f,ΔΡ.Z,20);}else ΒБ=new Vector3(1,1,1);Αι=ť*ĭ-ƌ*ť;Εϊ(Αι,ΒА);в=Vector3.Distance(δ,ΑΙ);
}float ΓЏ(float value,ref float Є,float ǥ,float Ę,float ΔБ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Є)) Є+=Math.Sign(Є)*ǥ;else Є=value;if(value==0) Є=1;float ί=Math.Abs(Є);if(ί<ΔБ||Ę==0) return ί;Є=Math.Min(ΔБ,Math.Max(-ΔБ,Є));ί=Math.Abs(Ę);return ί;
}void Ζυ(){float ǐ=90,ǩ=90,ǣ=90;float Ǽ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ȍ,Ǟ,ΑΧ,ΔЉ;if(ΔЊ){ȍ=Vector3.Normalize(Ђ-δ);ΑΧ=ф(Ϋ,ȍ);Ǟ=ф(Ϋ,Εα);ǐ=Ů(ΑΧ,new Vector3(0,-1,0))-90;ǩ=Ł(Ǟ,new Vector3(-1,0,0),Ǟ.Y);
ǣ=Ł(ΑΧ,new Vector3(-1,0,0),ΑΧ.Z);}else{ȍ=Εβ;Ǟ=ф(Ϋ,Εα);ΑΧ=ф(Ϋ,Εβ);ΔЉ=ф(Ϋ,ΘΥ);ǐ=Ł(Ǟ,new Vector3(0,0,1),Ǟ.Y);ǩ=Ł(Ǟ,new Vector3(-1,0,0),Ǟ.Y);ǣ=Ł(ΔЉ,new Vector3(0,0,1),ΔЉ.X);}if(Ζω&&ΑЇ()){Vector3 Δυ=Ϋ.GetNaturalGravity();Ǟ=ф(Ϋ,Δυ);ǐ=Ł(Ǟ,new Vector3(0,0,1),Ǟ.Y);
ǩ=Ł(Ǟ,new Vector3(-1,0,0),Ǟ.Y);}if(!Γν(-45,ǩ,45)){ǐ=0;ǣ=0;}if(!Γν(-45,ǣ,45)) ǐ=0;ΒΔ(Δχ,1,(-ǐ)*Ǽ,(-ǣ)*Ǽ,(-ǩ)*Ǽ);ΒΪ=Math.Max(Math.Abs(ǐ),Math.Max(Math.Abs(ǩ),Math.Abs(ǣ)));Βο=ΒΪ<=ΒΝ;}void Θς(IMyTerminalBlock ΒЃ){if(ΒЃ==null) return;Ȁ=new float[3,2];Ǧ=new Dictionary<string,float[,]>();
for(int i=0;i<ā.Count;i++){IMyThrust ш=ā[i];if(!ш.IsFunctional) continue;Vector3 ς=ф(ΒЃ,ш.WorldMatrix.Backward);float İ=ш.MaxEffectiveThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) Ȁ[0,0]+=İ;else Ȁ[0,1]-=İ;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) Ȁ[1,0]+=İ;else Ȁ[1,1]-=İ;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) Ȁ[2,0]+=İ;else Ȁ[2,1]-=İ;String Ǔ=ΖΗ(ш);float[,]ǧ=null;if(Ǧ.ContainsKey(Ǔ)) ǧ=Ǧ[Ǔ];else{ǧ=new float[3,2];Ǧ.Add(Ǔ,ǧ);}float Ę=ш.MaxThrust;if(Math.Round(ς.X,2)!=0.0)
if(ς.X>=0) ǧ[0,0]+=Ę;else ǧ[0,1]-=Ę;if(Math.Round(ς.Y,2)!=0.0)
if(ς.Y>=0) ǧ[1,0]+=Ę;else ǧ[1,1]-=Ę;if(Math.Round(ς.Z,2)!=0.0)
if(ς.Z>=0) ǧ[2,0]+=Ę;else ǧ[2,1]-=Ę;}}static String ΖΗ(IMyThrust ш){return ш.BlockDefinition.SubtypeId;}Vector3 Γσ(Vector3 ǚ,float[,]Ǝ){return new Vector3(
ǚ.X>=0?Ǝ[0,0]:Ǝ[0,1],
ǚ.Y>=0?Ǝ[1,0]:Ǝ[1,1],
ǚ.Z>=0?Ǝ[2,0]:Ǝ[2,1]);}bool ΖΖ(ά έ,String Ǔ,out float Δξ){Δξ=0;int Β=Ā.IndexOf(Ǔ);if(Β==-1||έ.Ǿ==null||Β>=έ.Ǿ.Length)
return false;Δξ=έ.Ǿ[Β];if(Δξ==-1) return false;return true;}Vector3 Γζ(Vector3 ǚ,ά έ){if(έ!=null){Vector3 ί=new Vector3();for(int i=0;i<Ǧ.Keys.Count;i++){String Ǔ=Ǧ.Keys.ElementAt(i);float ΑΗ=0;if(!ΖΖ(έ,Ǔ,out ΑΗ))
return Γσ(ǚ,Ȁ);ί+=Γσ(ǚ,Ǧ.Values.ElementAt(i))*ΑΗ;}return ί;}return Γσ(ǚ,Ȁ);}float Ʋ(Vector3 ǚ,ά έ){return Ʋ(ǚ,new Vector3(),έ);}float Ʋ(Vector3 ǚ,Vector3 φ,ά έ){Vector3 ΛΚ=Γζ(ǚ,έ);Vector3 İ=ΛΚ+φ*ť;float š=(İ/ǚ).AbsMin();return(float)(ǚ*š).Length();}float Αχ(Vector3 Βφ,Vector3 ƌ,ά έ)
{if(Βφ.Length()==0) return 0;float Ζψ=1;if(ƌ.Length()>0) Ζψ=Math.Min(1,Ů(-ƌ,Βφ)/90)*0.4f+0.6f;float Αι=Ʋ(Βφ,ƌ,έ);if(Αι==0) return 0.1f;float ΒΜ=Й(Αι,ť);float Εε=(float) Math.Sqrt(Й(Βφ.Length(),ΒΜ*0.5f));return ΒΜ*Εε*Ζψ*Ȅ;}float ΑЄ(Vector3 ȍ,Vector3 Б,Vector3 φ,ά έ)
{if(φ.Length()==0f) return 0;Vector3 Βχ=ň(ȍ,Б,Vector3.Normalize(φ));float ǵ=Ʋ(-Βχ,έ);return ǵ/φ.Length();}void ΕϏ(bool Κε){float Ǩ=0;float Δς=0.9f;if(Κε){е=-1;ũ=0;ΒЊ=null;if(γ!=Π.э&&Ζ.φ.Length()!=0){Ǩ=Δς*ΑЄ(Ζ.ι,Ζ.θ*-1,Ζ.φ,null);if(Ǩ<е||е==-1) е=Ǩ;}if(Ο.λ&&Ο.φ.Length()!=0)
{Ǩ=Δς*ΑЄ(Ο.ι,Ο.θ*-1,Ο.φ,null);if(Ǩ<е||е==-1) е=Ǩ;}return;}if(ũ==-1) return;if(ũ>=0){int ΗЌ=0;while(ũ<Ψ.Count){if(ΗЌ>100) return;ΗЌ++;ά έ=Ψ[ũ];if(έ.φ.Length()!=0f){Ǩ=Δς*Math.Min(
ΑЄ(έ.ι,έ.θ*-1,έ.φ,έ),
ΑЄ(έ.ι*-1,έ.θ*-1,έ.φ,έ));if(Ǩ<е||е==-1) е=Ǩ;}else ΒЊ=έ;ũ++;}ũ=-1;}bool Δϊ=true;float ĸ=0;if(ΒЊ!=null){for(int i=0;i<Ǧ.Count;i++){String Ǔ=Ǧ.Keys.ElementAt(i);float[,]ǧ=Ǧ.Values.ElementAt(i);float ΑΗ=0;if(!ΖΖ(ΒЊ,Ǔ,out ΑΗ)){Δϊ=false;break;}for(int a=0;a<ǧ.GetLength(0);a++)
for(int b=0;b<ǧ.GetLength(1);b++){float ŋ=Math.Abs(ǧ[a,b]*ΑΗ);if(ŋ==0) continue;Δϊ=true;if(ĸ==0||ŋ<ĸ) ĸ=ŋ;}}}if(!Δϊ){for(int a=0;a<Ȁ.GetLength(0);a++)
for(int b=0;b<Ȁ.GetLength(1);b++){float ŋ=Math.Abs(Ȁ[a,b]);if(ŋ==0) continue;if(ĸ==0||ŋ<ĸ) ĸ=ŋ;}}if(ĸ>0){float Κρ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǩ=Й(ĸ,Κρ);if(Ǩ>0)
if(Ǩ<е||е==-1) е=Ǩ;}}void Εϊ(Vector3 ǵ,bool М){if(!М){for(int i=0;i<ā.Count;i++)
ā[i].SetValueFloat("Override",0.0f);return;}Vector3 ΔΠ=Γζ(ǵ,null);float Λγ=Math.Min(1,Math.Abs(Й(ǵ.X,ΔΠ.X)));float Λδ=Math.Min(1,Math.Abs(Й(ǵ.Y,ΔΠ.Y)));float Λε=Math.Min(1,Math.Abs(Й(ǵ.Z,ΔΠ.Z)));for(int i=0;i<ā.Count;i++){IMyThrust ш=ā[i];Vector3 ς=Ιβ(ф(Ϋ,ш.WorldMatrix.Backward),1);
if(ς.X!=0&&Math.Sign(ς.X)==Math.Sign(ǵ.X))
ш.SetValueFloat("Override",ш.MaxThrust*Λγ);else if(ς.Y!=0&&Math.Sign(ς.Y)==Math.Sign(ǵ.Y))
ш.SetValueFloat("Override",ш.MaxThrust*Λδ);else if(ς.Z!=0&&Math.Sign(ς.Z)==Math.Sign(ǵ.Z))
ш.SetValueFloat("Override",ш.MaxThrust*Λε);else
ш.SetValueFloat("Override",0.0f);}}void ΒΔ(bool М,float ΚЀ,float ǐ,float ǣ,float ǩ){for(int i=0;i<Βκ.Count;i++){IMyGyro К=Βκ[i];К.GyroOverride=М;if(!М) К.GyroPower=100;else К.GyroPower=ΚЀ;if(!М) continue;Vector3 Э=Ϋ.WorldMatrix.Forward;Vector3 р=Ϋ.WorldMatrix.Right;Vector3 Б=Ϋ.WorldMatrix.Up;
Vector3 ƅ=К.WorldMatrix.Forward;Vector3 Ƈ=К.WorldMatrix.Up;Vector3 Ɔ=К.WorldMatrix.Left*-1;if(ƅ==Э) К.SetValueFloat("Roll",ǩ);else if(ƅ==(Э*-1)) К.SetValueFloat("Roll",ǩ*-1);else if(Ƈ==(Э*-1)) К.SetValueFloat("Yaw",ǩ);else if(Ƈ==Э) К.SetValueFloat("Yaw",ǩ*-1);else if(Ɔ==Э) К.SetValueFloat("Pitch",ǩ);
else if(Ɔ==(Э*-1)) К.SetValueFloat("Pitch",ǩ*-1);if(Ɔ==(р*-1)) К.SetValueFloat("Pitch",ǐ);else if(Ɔ==р) К.SetValueFloat("Pitch",ǐ*-1);else if(Ƈ==р) К.SetValueFloat("Yaw",ǐ);else if(Ƈ==(р*-1)) К.SetValueFloat("Yaw",ǐ*-1);else if(ƅ==(р*-1)) К.SetValueFloat("Roll",ǐ);
else if(ƅ==р) К.SetValueFloat("Roll",ǐ*-1);if(Ƈ==(Б*-1)) К.SetValueFloat("Yaw",ǣ);else if(Ƈ==Б) К.SetValueFloat("Yaw",ǣ*-1);else if(Ɔ==Б) К.SetValueFloat("Pitch",ǣ);else if(Ɔ==(Б*-1)) К.SetValueFloat("Pitch",ǣ*-1);else if(ƅ==Б) К.SetValueFloat("Roll",ǣ);else if(ƅ==(Б*-1)) К.SetValueFloat("Roll",ǣ*-1);
}}void Ζο(){this.Δχ=false;}void л(Vector3 į,Vector3 Э,Vector3 ΔЈ,float ğ,bool Αξ){ΓΜ(į,ğ,Αξ);ΒΝ=ğ;ΔЊ=false;this.Εβ=Э;this.ΘΥ=ΔЈ;}void л(Vector3 į,Vector3 Э,Vector3 ΔЈ,bool Αξ){л(į,Э,ΔЈ,2f,Αξ);}void ΓΜ(Vector3 į,float ğ,bool Αξ){ΒΝ=ğ;this.Δχ=true;this.Ζω=Αξ;ΔЊ=true;
Βο=false;this.Εα=į;}void ΔΓ(){Ȃ(false,false,false,Ђ,0);ΒА=false;}void и(Vector3 η,float Ǽ){Ȃ(true,false,false,η,Ǽ);}void Ȃ(bool М,bool Εά,bool Επ,Vector3 η,float Ǽ){Ȃ(М,Εά,Επ,η,η-δ,0.0f,Ǽ);}void Ȃ(bool М,bool Εά,bool Επ,Vector3 η,Vector3 ΚЍ,float ΚЎ,float Ǽ){ΒА=true;
this.ΘΩ=М;Ђ=η;this.ΔА=Ǽ;this.ΕΛ=ΚЎ;this.ΘΔ=Εά;this.ΘΓ=Επ;this.ΗΨ=ΚЍ;в=Vector3.Distance(η,δ);}void Ιδ(){χ.GetBlocksOfType(ǂ,ǖ);}void Ιε(){if(ΒЅ){А=2;return;}List<IMyRemoteControl>Αή=new List<IMyRemoteControl>();List<IMySensorBlock>ΗБ=new List<IMySensorBlock>();List<IMyTerminalBlock>Αη=new List<IMyTerminalBlock>();
χ.GetBlocksOfType(Αή,ǖ);χ.GetBlocksOfType(Ц,ǖ);χ.GetBlocksOfType(ΗБ,ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αη,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǫ(Ц,pamTag,true);Ζη();Ζθ(Αη);Ň(Ц,setLCDFontAndSize,1.4f,false);Ň(Ȗ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΘΟ=Ǫ(ΗБ,pamTag,true);if(ΘΟ.Count>0) ψ=ΘΟ[0];else ψ=null;if(Μ==Κ.ƹ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ω.Count==0){А=1;Λ="Drills are missing";}}else if(Μ==Κ.ǫ){χ.GetBlocksOfType(ω,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ω.Count==0){А=1;Λ="Grinders are missing";}if(ψ==null){А=1;Λ="Sensor is missing";}}else if(Μ==Κ.ή){χ.GetBlocksOfType(Εζ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΘΞ=Ǫ(Αή,pamTag,true);if(ΘΞ.Count>0) Αή=ΘΞ;if(Αή.Count>0) Ϋ=Αή[0];else{Ϋ=null;А=2;Λ="Remote is missing";return;}ν=(float) Ϋ.CubeGrid.WorldVolume.Radius*2;ǡ=null;if(Μ!=Κ.ή){ΕЀ();if(ω.Count>0&&ǡ!=null){if(ψ!=null&&(ǡ.WorldMatrix.Forward!=ψ.WorldMatrix.Forward||
!(Ϋ.WorldMatrix.Forward==ψ.WorldMatrix.Up||Ϋ.WorldMatrix.Down==ψ.WorldMatrix.Down))){А=1;Λ="Wrong sensor direction";}if(ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Forward&&
ǡ.WorldMatrix.Forward!=Ϋ.WorldMatrix.Down){А=2;Λ="Wrong remote direction";}}}}void Ιζ(){χ.GetBlocksOfType(Ȱ,ǖ);χ.GetBlocksOfType(њ,ǖ);χ.GetBlocksOfType(ā,ǖ);χ.GetBlocksOfType(Βκ,ǖ);χ.GetBlocksOfType(ī,ǖ);χ.GetBlocksOfType(Ĺ,ǖ);χ.GetBlocksOfType(ǜ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));χ.GetBlocksOfType(ΒЉ,ǖ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
њ=Ǫ(њ,"ConnectorMedium",false);else
њ=Ǫ(њ,"Connector",false);List<IMyShipConnector>ΘΜ=Ǫ(њ,pamTag,true);if(ΘΜ.Count>0) њ=ΘΜ;if(А<=1){if(њ.Count==0){А=1;Λ="Connector is missing";}if(Βκ.Count==0){А=1;Λ="Gyros are missing";}if(ā.Count==0){А=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>ΘΠ=Ǫ(ΒЉ,pamTag,true);
if(ΘΠ.Count>0) ΒЉ=ΘΠ;List<IMyLandingGear>ΘΝ=Ǫ(Ȱ,pamTag,true);if(ΘΝ.Count>0) Ȱ=ΘΝ;for(int i=0;i<Ȱ.Count;i++) Ȱ[i].AutoLock=false;List<IMyBatteryBlock>ΘΛ=Ǫ(ī,pamTag,true);if(ΘΛ.Count>0) ī=ΘΛ;List<IMyGasTank>ΘΡ=Ǫ(ǜ,pamTag,true);if(ΘΡ.Count>0) ǜ=ΘΡ;}void Ιη(){χ.GetBlocksOfType(Ō,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ƃ.Clear();for(int i=Ō.Count-1;i>=0;i--){if(ΙΞ(Ō[i])){Ƃ.Add(Ō[i]);Ō.RemoveAt(i);}}}bool ΙΞ(IMyTerminalBlock Ω){if(Ω is IMyCargoContainer) return true;if(Ω is IMyShipDrill) return true;if(Ω is IMyShipGrinder) return true;if(Ω is IMyShipConnector){if(((IMyShipConnector) Ω).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Γκ(Ω,"ConnectorSmall",false))
return false;else return true;}return false;}void ΕЀ(){if(Ϋ==null) return;ǡ=null;float Βώ=0,Βω=0,ΒϏ=0,Βϊ=0,ΒЀ=0,Βϋ=0;List<IMyTerminalBlock>Εΰ=Ǫ(ω,pamTag,true);bool Κχ=Εΰ.Count==0;if(Εΰ.Count>0) ǡ=Εΰ[0];else if(ω.Count>0) ǡ=ω[0];int Ȫ=0;float ƙ=0;for(int i=0;i<ω.Count;i++)
{if(ω[i].WorldMatrix.Forward!=ǡ.WorldMatrix.Forward){if(Κχ){А=2;Λ="Mining direction is unclear!";return;}continue;}Ȫ++;ƙ=Math.Max(ƙ,Ιχ(ω[i]));Vector3 Н=ΑЃ(Ϋ,ω[i].GetPosition());if(i==0){Βώ=Н.X;Βω=Н.X;ΒϏ=Н.Y;Βϊ=Н.Y;ΒЀ=Н.Z;Βϋ=Н.Z;}Βω=Math.Max(Н.X,Βω);Βώ=Math.Min(Н.X,Βώ);
Βϊ=Math.Max(Н.Y,Βϊ);ΒϏ=Math.Min(Н.Y,ΒϏ);Βϋ=Math.Max(Н.Z,Βϋ);ΒЀ=Math.Min(Н.Z,ΒЀ);}if(ƙ==0) ƙ=drillRadius;bool į=ǡ!=null&&ǡ.WorldMatrix.Forward==Ϋ.WorldMatrix.Down;ǿ=(Βω-Βώ)*(1-Ū)+ƙ*2;ș=(į?Βϋ-ΒЀ:Βϊ-ΒϏ)*(1-Ŕ)+ƙ*2;IMyCubeGrid g=Ϋ.CubeGrid;float Ετ=0,Ευ=0,Εφ=0,Εχ=0;
for(int c=0;c<8;c++){Vector3 p=ΑЃ(Ϋ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=į?p.Z:p.Y;if(c==0||p.X<Ετ) Ετ=p.X;if(c==0||p.X>Ευ) Ευ=p.X;if(c==0||y<Εφ) Εφ=y;if(c==0||y>Εχ) Εχ=y;}Ť=Ευ-Ετ+g.GridSize;Αα=Εχ-Εφ+g.GridSize;}float Ιχ(IMyTerminalBlock b){if(!(b is IMyShipDrill)) return drillRadius;
String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǫ<T>(List<T>Ѝ,String ǽ,bool Βλ){List<T>ί=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(Γκ(Ѝ[i],ǽ,Βλ)) ί.Add(Ѝ[i]);return ί;}bool Γκ<T>(T Ω,String ǽ,bool Βλ){IMyTerminalBlock b=(IMyTerminalBlock) Ω;if(Βλ&&b.CustomName.ToUpper().Contains(ǽ.ToUpper())) return true;if(!Βλ&&b.BlockDefinition.ToString().ToUpper().Contains(ǽ.ToUpper())) return true;return false;}void ǘ<T>(List<T>Ѝ,bool М)
{for(int i=0;i<Ѝ.Count;i++)
Ζε((IMyTerminalBlock) Ѝ[i],М);}void Ζε(IMyTerminalBlock Ω,bool М){if(Ω==null) return;String ΙЍ=М?"OnOff_On":"OnOff_Off";var a=Ω.GetActionWithName(ΙЍ);a.Apply(Ω);}void ȃ(bool М){Εέ=М;if(!Ŧ) return;ǘ(ΒЉ,М);}void ΓЋ(ChargeMode Џ){for(int i=0;i<ī.Count;i++) ī[i].ChargeMode=Џ;}void ΓЍ(bool ΛΔ)
{for(int i=0;i<ǜ.Count;i++) ǜ[i].Stockpile=ΛΔ;}void Ƽ(List<IMyLandingGear>Δσ,bool Ηφ){for(int i=0;i<Δσ.Count;i++){if(Ηφ) Δσ[i].Lock();if(!Ηφ) Δσ[i].Unlock();}}void Ζζ(bool М){if(ΗΗ==М) return;List<IMyShipController>ΒΧ=new List<IMyShipController>();χ.GetBlocksOfType(ΒΧ,ǖ);if(ΒΧ.Count==0) return;
for(int i=0;i<ΒΧ.Count;i++)
ΒΧ[i].DampenersOverride=М;ΗΗ=М;}IMyShipConnector з(MyShipConnectorStatus Ǒ){for(int i=0;i<њ.Count;i++){if(!Ȟ(њ[i],true)) continue;if(њ[i].Status==Ǒ) return њ[i];}return null;}void ŉ(bool ΕΨ,bool Βή,float Κέ,float ΚΒ){if(ψ==null||ω.Count==0) return;Vector3 ΒΣ=new Vector3();
int Ȫ=0;for(int i=0;i<ω.Count;i++){if(ω[i].WorldMatrix.Forward!=ǡ.WorldMatrix.Forward) continue;Ȫ++;ΒΣ+=ω[i].GetPosition();}ΒΣ=ΒΣ/Ȫ;Vector3 ƒ=ΑЃ(ψ,ΒΣ);ψ.Enabled=true;ψ.ShowOnHUD=ΕΨ;ψ.LeftExtend=(Βή?1:ѐ)/2f*ǿ-ƒ.X;ψ.RightExtend=(Βή?1:ѐ)/2f*ǿ+ƒ.X;ψ.TopExtend=(Βή?1:я)/2f*ș+ƒ.Y;
ψ.BottomExtend=(Βή?1:я)/2f*ș-ƒ.Y;ψ.FrontExtend=(ΕΨ?н:Κέ)-ƒ.Z;ψ.BackExtend=ΕΨ?0:ΚΒ+ν*0.75f+ƒ.Z;ψ.DetectFloatingObjects=true;ψ.DetectAsteroids=false;ψ.DetectLargeShips=true;ψ.DetectSmallShips=true;ψ.DetectStations=true;ψ.DetectOwner=true;ψ.DetectSubgrids=false;
ψ.DetectPlayers=false;ψ.DetectEnemy=true;ψ.DetectFriendly=true;ψ.DetectNeutral=true;}void Ň<T>(List<T>Ѝ,bool ΚА,float ΚΫ,bool Κζ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Αζ=null;if(Ѝ[i]is IMyTextSurface) Αζ=(IMyTextSurface) Ѝ[i];if(Αζ!=null){Αζ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΚА) continue;Αζ.Font="Debug";if(Κζ) continue;Αζ.FontSize=ΚΫ;}}}bool ΑЇ(){Vector3D κ;return this.Ϋ.TryGetPlanetPosition(out κ);}int ΘБ(Vector3 ΛΖ,int ΘΕ){if(ΘΕ==-1) return 0;double Ы=-1;int č=-1;for(int i=Ψ.Count-1;i>=0;i--){double ϊ=Vector3.Distance(Ψ[i].Ε,ΛΖ);if(Ы==-1||ϊ<Ы){č=i;Ы=ϊ;}
}return Math.Sign(č-ΘΕ);}bool ΙΟ(Vector3 κ){List<Vector3>а=new List<Vector3>();for(int i=0;i<Ψ.Count;i++) а.Add(Ψ[i].Ε);if(Ο.λ&&Ψ.Count>=1){Vector3 Ž=new Vector3();ȝ(Ο,dockDist*ν,false,out Ž);if(Vector3.Distance(Ο.Ε,Ψ.First().Ε)<Vector3.Distance(Ο.Ε,Ψ.Last().Ε)){а.Insert(0,Ž);а.Insert(0,Ο.Ε);}
else{а.Add(Ž);а.Add(Ο.Ε);}}if(Μ==Κ.ή){if(Ζ.λ&&Ψ.Count>=1){Vector3 ΔΜ=new Vector3();ȝ(Ζ,dockDist*ν,false,out ΔΜ);if(Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε)){а.Insert(0,ΔΜ);а.Insert(0,Ζ.Ε);}else{а.Add(ΔΜ);а.Add(Ζ.Ε);}}}else{if(γ!=Π.э)
if(Ψ.Count>0&&Vector3.Distance(Ζ.Ε,Ψ.First().Ε)<Vector3.Distance(Ζ.Ε,Ψ.Last().Ε))
а.Insert(0,Ζ.Ε);else а.Add(Ζ.Ε);}int č=-1;double Ȉ=-1;for(int i=0;i<а.Count;i++){double ϊ=Vector3.Distance(а[i],κ);if(ϊ<Ȉ||Ȉ==-1){Ȉ=ϊ;č=i;}}if(а.Count==0) return false;double Δθ=Vector3.Distance(а[č],κ);double ΚЁ=Vector3.Distance(а[Math.Max(0,č-1)],а[č])*1.5f;
double Κφ=Vector3.Distance(а[Math.Min(а.Count-1,č+1)],а[č])*1.5f;return Δθ<ΚЁ||Δθ<Κφ||Δθ<laneOffset*ν*1.5f;}static float Й(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ιβ(Vector3 v,int Δδ){return new Vector3(Math.Round(v.X,Δδ),Math.Round(v.Y,Δδ),Math.Round(v.Z,Δδ));
}float Ů(Vector3 a,Vector3 b){if(a==b) return 0;float ΚΨ=(a*b).Sum;float Ηπ=a.Length();float Ηρ=b.Length();if(Ηπ==0||Ηρ==0) return 0;float ί=(float)((180.0f/Math.PI)*Math.Acos(ΚΨ/(Ηπ*Ηρ)));if(float.IsNaN(ί)) return 0;return ί;}float Ł(Vector3 v,Vector3 ΒЃ,float ΚБ){float Ż=Ů(v,ΒЃ);
if(ΚБ>0f) Ż*=-1;if(Ż>-90f) return Ż-90f;else return 180f-(-Ż-90f);}double ΕЇ(float ΚΠ){return(Math.PI/180)*ΚΠ;}bool Γν(double Κπ,double value,double Ə){return(value>=Κπ&&value<=Ə);}Vector3 Ιψ(IMyTerminalBlock Ω,Vector3 Н){return Vector3D.Transform(Н,Ω.WorldMatrix);
}Vector3 ΑЃ(IMyTerminalBlock Ω,Vector3 Λΰ){return ф(Ω,Λΰ-Ω.GetPosition());}Vector3 ф(IMyTerminalBlock Ω,Vector3 Εσ){return Vector3D.TransformNormal(Εσ,MatrixD.Transpose(Ω.WorldMatrix));}Vector3 ň(Vector3 ȍ,Vector3 Б,Vector3 Εσ){MatrixD ΔЏ=MatrixD.CreateFromDir(ȍ,Б);return Vector3D.TransformNormal(Εσ,MatrixD.Transpose(ΔЏ));
}Vector3 ΔΖ(Vector3 ȍ,Vector3 Б,Vector3 Н){MatrixD ΔЏ=MatrixD.CreateFromDir(ȍ,Б);return Vector3D.Transform(Н,ΔЏ);}String σ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 π(String s){String[]τ=s.Split('|');return new Vector3(float.Parse(Ρ(τ,0)),float.Parse(Ρ(τ,1)),float.Parse(Ρ(τ,2)));
}String ΙЋ(ά έ){String П=":";String ί=σ(έ.Ε)+П+σ(έ.ι)+П+
σ(έ.θ)+П+σ(έ.Ё)+П+σ(έ.φ);for(int i=0;i<έ.Ǿ.Length;i++)
ί+=П+Math.Round(έ.Ǿ[i],3);return ί;}ά Ιρ(String s){String[]τ=s.Split(':');ά έ=new ά(
π(Ρ(τ,0)),π(Ρ(τ,1)),
π(Ρ(τ,2)),π(Ρ(τ,3)),
π(Ρ(τ,4)));int Β=5;List<float>ΗΥ=new List<float>();while(Β<τ.Length){String ΓΖ=Ρ(τ,Β);float f=0;if(!float.TryParse(ΓΖ,out f)) break;ΗΥ.Add(f);Β++;}έ.Ǿ=ΗΥ.ToArray();return έ;}System.Text.StringBuilder Ɯ=new System.Text.StringBuilder();void Χ<T>(T value,bool Κυ){if(Κυ) Ɯ.Append('\n');Ɯ.Append(value);}
void Χ<T>(T value){Χ(value,true);}String Αό(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΒΙ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ρ(String[]ΙБ,int Β){String ΓΖ=ΙБ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΓΖ)) return "";
return ΓΖ;}void Save(){if(ΒЅ||Μ==Κ.ј){Storage="";return;}Storage=ΓΣ();}String ΓΣ(){Ɯ.Clear();Ɯ.Append(DATAREV+";");Χ(σ(ΒЄ),false);Χ(σ(Ο.ι));Χ(σ(Ο.Ё));Χ(σ(Ο.θ));Χ(σ(Ο.φ));Χ(σ(Ο.Ε));Χ(σ(Ο.о));Χ(Ο.λ);Χ(σ(Ζ.Ε));Χ(σ(Ζ.φ));Χ(σ(Ζ.ι));Χ(σ(Ζ.θ));Χ(σ(Ζ.Ё));Χ(σ(Ζ.о));Χ(Ζ.λ);Χ(σ(ǟ));Χ(σ(ę));Χ(";");
Χ((int) Μ,false);Χ((int) γ);Χ((int) ў);Χ(Ȑ);Χ(џ);Χ(п);Χ(Ǎ);Χ(Ȅ);Χ(ġ);Χ(Ġ);if(Μ==Κ.ή){Χ((int) Ј.Р);Χ(Ј.ǲ);Χ(Ј.ı);Χ(Αό(Ј.ǳ));Χ(Αό(Ј.Ƿ));Χ((int) Љ.Р);Χ(Љ.ǲ);Χ(Љ.ı);Χ(Αό(Љ.ǳ));Χ(Αό(Љ.Ƿ));}else{Χ((int) Ȕ);Χ((int) ѓ);Χ((int) д);Χ((int) ћ);Χ((int) Ř);Χ(Ľ);Χ(Ŧ);Χ(Ɗ);Χ(Ƅ);Χ(ѐ);Χ(я);Χ(н);Χ(ƭ);Χ(Ƭ);
Χ(Ū);Χ(Ŕ);Χ(Ї);Χ(Ю);Χ(Ѐ);Χ(ύ);Χ(ė);Χ(б);Χ(Ţ);Χ(ś);}Χ(";");for(int i=0;i<Ā.Count;i++)
Χ((i>0?"|":"")+Ā[i],false);Χ(";");for(int i=0;i<Ψ.Count;i++)
Χ(ΙЋ(Ψ[i]),i>0);Χ(";",false);Χ(г+"|"+Д,false);Χ(";",false);Χ(ΘЂ(),false);String Θ=Ɯ.ToString();Ɯ.Clear();return Θ;}љ Ια(){return ΑБ(Storage);}љ ΑБ(String Θ){if(Θ=="") return љ.ΙΨ;String[]ţ=Θ.Split(';');if(Ρ(ţ,0)!=DATAREV) return љ.Γω;int Β=0;try{String[]Ξ=Ρ(ţ,1).Split('\n');
ΒЄ=π(Ρ(Ξ,Β++));Ο.ι=π(Ρ(Ξ,Β++));Ο.Ё=π(Ρ(Ξ,Β++));Ο.θ=π(Ρ(Ξ,Β++));Ο.φ=π(Ρ(Ξ,Β++));Ο.Ε=π(Ρ(Ξ,Β++));Ο.о=π(Ρ(Ξ,Β++));Ο.λ=bool.Parse(Ρ(Ξ,Β++));Ζ.Ε=π(Ρ(Ξ,Β++));Ζ.φ=π(Ρ(Ξ,Β++));Ζ.ι=π(Ρ(Ξ,Β++));Ζ.θ=π(Ρ(Ξ,Β++));Ζ.Ё=π(Ρ(Ξ,Β++));Ζ.о=π(Ρ(Ξ,Β++));Ζ.λ=bool.Parse(Ρ(Ξ,Β++));
ǟ=π(Ρ(Ξ,Β++));ę=π(Ρ(Ξ,Β++));Ξ=Ρ(ţ,2).Split('\n');Β=0;Μ=(Κ) int.Parse(Ρ(Ξ,Β++));γ=(Π) int.Parse(Ρ(Ξ,Β++));ў=(Π) int.Parse(Ρ(Ξ,Β++));Ȑ=int.Parse(Ρ(Ξ,Β++));џ=int.Parse(Ρ(Ξ,Β++));п=int.Parse(Ρ(Ξ,Β++));Ǎ=int.Parse(Ρ(Ξ,Β++));Ȅ=float.Parse(Ρ(Ξ,Β++));ġ=bool.Parse(Ρ(Ξ,Β++));
Ġ=bool.Parse(Ρ(Ξ,Β++));if(Μ==Κ.ή){Ј.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Ј.ǲ=float.Parse(Ρ(Ξ,Β++));Ј.ı=float.Parse(Ρ(Ξ,Β++));Ј.ǳ=ΒΙ(Ρ(Ξ,Β++));Ј.Ƿ=ΒΙ(Ρ(Ξ,Β++));Љ.Р=(ρ) int.Parse(Ρ(Ξ,Β++));Љ.ǲ=float.Parse(Ρ(Ξ,Β++));Љ.ı=float.Parse(Ρ(Ξ,Β++));Љ.ǳ=ΒΙ(Ρ(Ξ,Β++));Љ.Ƿ=ΒΙ(Ρ(Ξ,Β++));}else
{Ȕ=(ю) int.Parse(Ρ(Ξ,Β++));ѓ=(ъ) int.Parse(Ρ(Ξ,Β++));д=(Г) int.Parse(Ρ(Ξ,Β++));ћ=(Ǖ) int.Parse(Ρ(Ξ,Β++));Ř=(ю) int.Parse(Ρ(Ξ,Β++));Ľ=bool.Parse(Ρ(Ξ,Β++));Ŧ=bool.Parse(Ρ(Ξ,Β++));Ɗ=bool.Parse(Ρ(Ξ,Β++));Ƅ=bool.Parse(Ρ(Ξ,Β++));ѐ=int.Parse(Ρ(Ξ,Β++));я=int.Parse(Ρ(Ξ,Β++));н=int.Parse(Ρ(Ξ,Β++));
ƭ=float.Parse(Ρ(Ξ,Β++));Ƭ=float.Parse(Ρ(Ξ,Β++));Ū=float.Parse(Ρ(Ξ,Β++));Ŕ=float.Parse(Ρ(Ξ,Β++));Ї=int.Parse(Ρ(Ξ,Β++));Ю=int.Parse(Ρ(Ξ,Β++));Ѐ=int.Parse(Ρ(Ξ,Β++));ύ=int.Parse(Ρ(Ξ,Β++));ė=int.Parse(Ρ(Ξ,Β++));б=int.Parse(Ρ(Ξ,Β++));Ţ=int.Parse(Ρ(Ξ,Β++));ś=float.Parse(Ρ(Ξ,Β++));
}Ξ=Ρ(ţ,3).Replace("\n","").Split('|');Ā=Ξ.ToList();Ξ=Ρ(ţ,4).Split('\n');Ψ.Clear();if(Ξ.Count()>=1&&Ξ[0]!="")
for(int i=0;i<Ξ.Length;i++)
Ψ.Add(Ιρ(Ρ(Ξ,i)));Ξ=Ρ(ţ,5).Split('|');г=Ρ(Ξ,0);if(!int.TryParse(Ρ(Ξ,1),out Д)) Д=-1;if(г.Length!=Ї*Ю){г="";Д=-1;}ΘЁ(Ρ(ţ,6));}catch{return љ.ΕЊ;}Ɲ=ў;Ѓ();return љ.ΒΖ;}const int Εώ=64000;IMyTerminalBlock ΕЋ(){List<IMyTerminalBlock>ƀ=new List<IMyTerminalBlock>();χ.GetBlocksOfType(ƀ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ƀ.Count>0?ƀ[0]:null;}void ΘІ(){IMyTerminalBlock Ω=ΕЋ();if(Ω==null){Λ="Export: name a block "+dataTag;return;}String Θ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Ψ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΓΣ();if(Θ.Length>Εώ){Λ="Export too large: "+Θ.Length+" chars, max "+Εώ;return;}Ω.CustomData=Θ;Λ="Exported "+Ψ.Count+" waypoints";}void ΙΛ(bool ΔЂ){IMyTerminalBlock Ω=ΕЋ();if(Ω==null){Λ="Import: name a block "+dataTag;return;}
ΙΜ(Ω.CustomData,ΔЂ);}void ΙΜ(String Θ,bool ΔЂ){Θ=Θ.Replace("\r","");if(Θ.StartsWith("//")){int ΗЄ=Θ.IndexOf('\n');Θ=ΗЄ<0?"":Θ.Substring(ΗЄ+1);}Θ=Θ.Trim();if(Θ==""){Λ="Import: no PAM data in "+dataTag;return;}Ѓ();String ΖЀ=ΓΣ();љ ί=ΑБ(Θ);if(ί==љ.ΒΖ&&ΔЂ){ά Υ=new ά(Ζ);
Vector3 Κσ=ǟ,Κς=ę;int w=ѐ,h=я,d=н;ю Αδ=Ȕ;Ǖ Δε=ћ;float Κϊ=Ū,Κω=Ŕ;ΑБ(ΖЀ);Ζ=Υ;ǟ=Κσ;ę=Κς;ѐ=w;я=h;н=d;Ȕ=Αδ;ћ=Δε;Ū=Κϊ;Ŕ=Κω;γ=Π.Ŷ;ǀ(true,true);γ=Π.Ŷ;ў=Π.Ŷ;Λ="Imported job, run CONT";return;}if(ί==љ.ΒΖ){ΑΫ=false;Λ="Imported "+Ψ.Count+" waypoints";
return;}ΑБ(ΖЀ);Λ=ί==љ.Γω?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ζη(){Ő.Clear();for(int i=Ц.Count-1;i>=0;i--){String Θ=Ц[i].CustomData.ToUpper();bool ΔЀ=false;if(Θ=="INSTRUCTIONS"){ΔЀ=true;Βέ=true;}if(Θ=="DEBUG") ΔЀ=true;if(ΔЀ){Ő.Add(Ц[i]);Ц.RemoveAt(i);}
}Ň(Ő,false,1,false);}void Ζθ(List<IMyTerminalBlock>ƀ){Ȗ.Clear();for(int i=0;i<ƀ.Count;i++){IMyTerminalBlock Ω=ƀ[i];try{String ΗЋ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Ω.CustomName.IndexOf(ΗЋ);int Ƣ=-1;if(Β<0||!int.TryParse(Ω.CustomName.Substring(Β+ΗЋ.Length,1),out Ƣ)) continue;
if(Ƣ==-1) continue;Ƣ--;IMyTextSurfaceProvider ΗЍ=(IMyTextSurfaceProvider) Ω;if(Ƣ<ΗЍ.SurfaceCount&&Ƣ>=0)
Ȗ.Add(ΗЍ.GetSurface(Ƣ));}catch{}}}String Βό="";void Ζβ(){String ΔЎ="";String ΑΖ="";Βό=Εύ(false);ΔЎ+=Βό;ΑΖ+=Βό;ΑΖ+=ΘЎ();for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(ΔЎ);for(int i=0;i<Ȗ.Count;i++) Ȗ[i].WriteText(ΔЎ);Echo(ΑΖ);for(int i=0;i<Ő.Count;i++){IMyTextPanel ч=Ő[i];
String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΖΑ());}}string ΖΑ(){String Γ="";try{float Ηω=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ſ*Ηω)+" / "+(int)(Ɛ*Ηω)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ſ*100f,1)+"% / "+Math.Round(Ɛ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ǆ,2)+"ms / "+Math.Round(ΕΑ,2)+"ms \n";}catch{}for(int i=0;i<Ě.Count;i++)
Γ+=""+Ě.Keys.ElementAt(i)+" = "+Math.Round(Ě.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ě.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΙΗ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ň(Ц,setLCDFontAndSize,0.9f,false);Ň(Ȗ,setLCDFontAndSize,0.9f,true);for(int i=0;i<Ц.Count;i++) Ц[i].WriteText(Γ+Ė.ToString());for(int i=0;i<Ȗ.Count;i++) Ȗ[i].WriteText(Γ+Ė.ToString());}void Ȣ(){if(!Βέ) return;try{ΕΞ=Runtime.CurrentInstructionCount;}catch{}
}void Ȝ(String ΰ){if(!Βέ) return;if(ΕΞ==0) return;try{float ΓΔ=(Runtime.CurrentInstructionCount-ΕΞ)/Runtime.MaxInstructionCount*100;if(!Ě.ContainsKey(ΰ))
Ě.Add(ΰ,new float[]{ΓΔ,ΓΔ});else{Ě[ΰ][0]=ΓΔ;Ě[ΰ][1]=Math.Max(ΓΔ,Ě[ΰ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Θκ(String Ȍ){if(!Ɓ) return;String П=""+BROADCAST_SEP;String β=VERSION+П;β+=Ʈ(Ơ)+П;
β+=(int) Μ+П;β+=ű(δ.X)+""+П;β+=ű(δ.Y)+П;β+=ű(δ.Z)+П;if(Μ!=Κ.ή)
β+=Γγ(ў)+(ў==Π.В?" "+Math.Round(ΑΡ,1)+"%":"")+П;else β+=Γγ(ў)+П;β+=Ʈ(Λ)+П;β+=Ў+""+П;β+=Ʈ(Βό)+П;β+=Ȋ+П;β+=ȑ+П;for(int i=0;i<Ƕ.Count;i++){if(Ƕ[i].ƍ!=Щ.ų) continue;β+=Ʈ(Ƕ[i].ΰ)+"/"+Ʈ(Ƕ[i].υ)+"/"+Ƕ[i].Ћ+П;}Ιλ(β,Ȍ);}String Ʈ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ιλ(String β,String Ȍ){ΓЉ(β,BROADCAST_SELF_ID,Ȍ);}void ΓЉ(String β,String η,String Ȍ){try{if(!Ɓ) return;β=BROADCAST_TAG+BROADCAST_SEP+ΕЎ()+BROADCAST_SEP+η+BROADCAST_SEP+ΔΩ+BROADCAST_SEP+Ȍ+BROADCAST_SEP+β;this.IGC.SendBroadcastMessage(BROADCAST_TAG,β);}catch(Exception e){Ė=e;}
}String ΕЎ(){if(Μ!=Κ.ј) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γς(String β){return β.StartsWith(BROADCAST_TAG);}bool ж(ref String Θ,out string ΑΘ,Char П){int Β=Θ.IndexOf(П);ΑΘ="";if(Β<0) return false;ΑΘ=Θ.Substring(0,Β);Θ=Θ.Remove(0,Β+1);return true;}bool Ζα(ref String β,out String ȓ,out String Ȍ)
{ȓ="";Ȍ="";if(!Ɓ) return false;String ǽ="";if(!ж(ref β,out ǽ,BROADCAST_SEP)||!Γς(ǽ)) return false;if(!ж(ref β,out ȓ,BROADCAST_SEP)) return false;if(!ж(ref β,out ǽ,BROADCAST_SEP)||(ǽ!=ΕЎ()&&(ǽ!="*"&&Μ!=Κ.ј))) return false;if(!ж(ref β,out ǽ,BROADCAST_SEP)||(ǽ!=ΔΩ)) return false;
if(!ж(ref β,out Ȍ,BROADCAST_SEP)) return false;return true;}void ΖΟ(){bool ǋ=true;if(ΗΧ){ΗΧ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String ΑΝ=(Μ!=Κ.ј?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ΑΝ)
ΖΠ(ΑΝ);String[]Ξ=Me.CustomData.Split('\n');Ɓ=bool.Parse(ΑА(Αώ(Ξ,"Enable_Broadcast",ref ǋ)));bool Ερ=false;bool ΗΛ=true;if(Ɓ){if(Μ!=Κ.ј)
Ơ=ΑА(Αώ(Ξ,"Ship_Name",ref ǋ)).Replace(BROADCAST_SELF_ID,"");ΔΩ=ΑА(Αώ(Ξ,"Broadcast_Channel",ref ǋ)).ToLower();ΗΛ=false;if(ǰ==null){ǰ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ǰ.SetMessageCallback("");}List<IMyRadioAntenna>ż=new List<IMyRadioAntenna>();χ.GetBlocksOfType(ż);bool ΔΛ=false;
for(int i=0;i<ż.Count;i++)
if(ż[i].EnableBroadcasting&&ż[i].Enabled){ΔΛ=true;break;}if(ż.Count==0) Λ="No Antenna found";else if(!ΔΛ) Λ="Antenna not ready";Ερ=ż.Count==0||!ΔΛ;if(Ζϊ&&!Ερ&&Μ!=Κ.ј) Λ="Antenna ok";}else if(Μ==Κ.ј)
Λ="Offline - Enable in PB custom data";Ζϊ=Ερ;if(ΗΛ){if(ǰ!=null) this.IGC.DisableBroadcastListener(ǰ);ǰ=null;}}catch{ǋ=false;}if(!ǋ){Λ="Reset custom data";ΖΠ(ΑΝ);}}void ΖΠ(String ΑΝ){String Θ=ΑΝ+"\n\n"+"Enable_Broadcast="+(Μ==Κ.ј?"true":"false")+" \n";Θ+=Μ!=Κ.ј?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Θ+"Broadcast_Channel=#default";}String ΑА(String Ś){int Β=Ś.IndexOf("//");if(Β!=-1) Ś=Ś.Substring(0,Β);String[]τ=Ś.Split('=');if(τ.Length<=1) return "";return τ[1].Trim();}String Αώ(String[]Ξ,String ȯ,ref bool ȱ){foreach(String Ś in Ξ)
if(Ś.StartsWith(ȯ)) return Ś;ȱ=false;return "";}void Θώ(string Т){if(ď) Ь=new List<Ѕ>();String ȓ="";String Ȍ="";if(!Γς(Т)) Ιί(Т);bool ΘΑ=false;int ΕΔ=50;while(ǰ!=null&&ǰ.HasPendingMessage&&ΕΔ-->0){MyIGCMessage β=ǰ.AcceptMessage();String Θ=β.Data as String;if(Θ==null) continue;
if(Ζα(ref Θ,out ȓ,out Ȍ)&&ȓ!=""&&ȓ!=BROADCAST_SELF_ID){Ѕ Δ=Γα(ȓ,"");if(Δ==null){Δ=new Ѕ(ȓ);Ь.Add(Δ);}Δ.ņ(Ȍ,false,false,true);ΙЅ(Δ,Θ);ΘΑ=true;}}if(ΘΑ)
Ь.Sort(delegate(Ѕ a,Ѕ b){if(a.ΰ==null&&b.ΰ==null) return 0;else if(a.ΰ==null) return-1;else if(b.ΰ==null) return 1;else return a.ΰ.CompareTo(b.ΰ);});if(ĕ||ď){if(Αί<=0||ď){Λ="";ď=false;Ιι();ΖΟ();ΔЇ=DateTime.Now;}}if(ŗ) Ιύ();if(ŗ) ΙЁ();if(ŗ) ΙЉ();Ιϋ();}void ΙЅ(Ѕ Δ,String Θ)
{Δ.ΔЄ=DateTime.Now;String ΑΘ="";ж(ref Θ,out Δ.ΓΘ,BROADCAST_SEP);ж(ref Θ,out Δ.ΰ,BROADCAST_SEP);if(Δ.ΓΘ!=VERSION) return;String ΑΪ="";String[]Ƙ=new string[3];ж(ref Θ,out ΑΪ,BROADCAST_SEP);ж(ref Θ,out Ƙ[0],BROADCAST_SEP);ж(ref Θ,out Ƙ[1],BROADCAST_SEP);ж(ref Θ,out Ƙ[2],BROADCAST_SEP);
ж(ref Θ,out Δ.Εή,BROADCAST_SEP);ж(ref Θ,out Δ.Λ,BROADCAST_SEP);ж(ref Θ,out ΑΘ,BROADCAST_SEP);ж(ref Θ,out Δ.Βύ,BROADCAST_SEP);String Θΰ="",Ηό="";ж(ref Θ,out Θΰ,BROADCAST_SEP);ж(ref Θ,out Ηό,BROADCAST_SEP);Δ.Ќ.Clear();while(true){String Ηκ;if(!ж(ref Θ,out Ηκ,BROADCAST_SEP)) break;String[]Βρ=Ηκ.Split('/');
if(Βρ.Count()<3) continue;int Ћ=0;if(!int.TryParse(Βρ[2],out Ћ)) continue;Δ.Ќ.Add(new м(Βρ[0],Βρ[1],Ћ,Щ.ų));}int ΗЀ=0;if(!int.TryParse(ΑΘ,out Δ.Ў)) Δ.Ў=0;if(!int.TryParse(ΑΪ,out ΗЀ)) Δ.Μ=Κ.ĩ;if(!float.TryParse(Ƙ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ƙ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;
if(!float.TryParse(Ƙ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;if(!float.TryParse(Θΰ,out Δ.Ȋ)) Δ.Ȋ=0;if(!float.TryParse(Ηό,out Δ.ȑ)) Δ.ȑ=0;Δ.Μ=(Κ) ΗЀ;Δ.Δι=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Εκ=0;for(int i=0;i<Δ.Ќ.Count();i++) Δ.Εκ+=Δ.Ќ[i].Ћ;}Ѕ Γα(String Φ,String ΰ){Φ=Φ.ToUpper();ΰ=ΰ.ToUpper();
for(int i=0;i<Ь.Count;i++){if(Φ!=""&&Ь[i].Φ.ToUpper()==Φ) return Ь[i];if(ΰ!=""&&Ь[i].ΰ!=null&&Ь[i].ΰ.ToUpper()==ΰ) return Ь[i];}return null;}String ƃ="";Ѕ ΖЊ=null;DateTime ΖЋ=DateTime.MinValue;List<Ѕ>ō=new List<Ѕ>();const float Θο=20f;bool Ζκ(Ѕ η,String І){return І==ƃ&&η==ΖЊ&&(DateTime.Now-ΖЋ).TotalSeconds<Θο;
}String ΘЈ(Ѕ η){if(ƃ==""||!Ζκ(η,ƃ)) return "";String s="";int ȱ=0,ΕΩ=0;for(int i=0;i<ō.Count;i++){Њ ΒЋ=ō[i].ņ(ƃ,false,false,false);if(ΒЋ==Њ.ƺ) ȱ++;if(ΕΩ++<8) s+="  "+ō[i].ΰ+": "+(ΒЋ==Њ.ƺ?"received":ΒЋ==Њ.ŷ?"pending...":ΒЋ==Њ.ΑЍ?"no answer!":"-")+"\n";
}if(ΕΩ>8) s+="  +"+(ΕΩ-8)+" more\n";return "--------------------------------------------\n"+ƃ+": "+ȱ+"/"+ō.Count+" received\n"+s;}void Ǭ(Ѕ Δ,String І){ƃ=І;ΖЊ=Δ;ΖЋ=DateTime.Now;ō.Clear();if(Δ==null) ō.AddRange(Ь);else ō.Add(Δ);if(Δ==null){for(int i=0;i<Ь.Count;i++)
Ь[i].ņ(І,true,false,false);ΓЉ(І,"*",І);}else{Δ.ņ(І,true,false,false);ΓЉ(І,Δ.Φ,І);}}void Ιί(string Т){if(Т=="") return;var τ=Т.ToUpper().Split(' ');τ.DefaultIfEmpty("");var І=τ.ElementAtOrDefault(0);var ȥ=τ.ElementAtOrDefault(1);String Ȯ="Invalid argument: "+Т;if(ό==Σ.ƽ){if(Л!=null)
{switch(І){case "UP":{if(Ў<2) break;if(Л.Ў==0){Ў=1;return;}Ǭ(Л,"UP");return;}case "DOWN":{if(Ў<1) break;if(Ў==1){Ǭ(Л,"MRES");break;}Ǭ(Л,"DOWN");break;}case "APPLY":{if(Ў<2) break;Ǭ(Л,"APPLY");return;}}}}switch(І){case "UP":this.Γφ(false);return;case "DOWN":this.Γυ(false);return;
case "APPLY":this.ů(true);return;}switch(І){case "CLEAR":Θψ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȥ)){Λ="Usage: SENDALL <cmd>";return;}Ǭ(null,ȥ);return;case "SEND":
if(Т.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}Ιμ(Т.Remove(0,"SEND".Length+1));return;}Λ=Ȯ;}void Ιμ(String Ǆ){if(Ǆ=="") return;var τ=Ǆ.Split(':');if(τ.Length!=2){Λ="Missing separator \":\"";return;}τ.DefaultIfEmpty("");String Ơ=τ.ElementAtOrDefault(0).Trim();
Ѕ Δ=Γα("",Ơ);if(Δ!=null) Ǭ(Δ,τ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ơ;}void Θψ(){Ь.Clear();Л=null;μ(Σ.Main);Ў=0;}void Ιι(){List<IMyTerminalBlock>Αη=new List<IMyTerminalBlock>();χ.GetBlocksOfType(Ц,ǖ);χ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Αη,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);Ц=Ǫ(Ц,pamTag,true);Ζθ(Αη);Ζη();Ικ();Ň(Ц,setLCDFontAndSize,1.15f,false);Ň(Ȗ,setLCDFontAndSize,1.15f,true);String Κβ=ΖΒ(false);String ΗЅ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock ч in Ц){if(ч.CustomData==""){ч.CustomData=ΖΒ(true);continue;}
if(!ч.CustomData.Contains(Κβ))
if(!ч.CustomData.Contains(ΗЅ)) ч.CustomData=ΗЅ+ч.CustomData;}}void Ιϋ(){String ΑΖ="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ȗ.Count;i++)
Ȗ[i].WriteText(Εϋ(С.ΖΪ,"0",false));for(int i=0;i<Ц.Count;i++){С Џ=С.ĩ;String Ǻ="";ΙΪ(Ц[i],out Џ,out Ǻ);Ц[i].WriteText(Εϋ(Џ,Ǻ,i==0));}Echo(ΑΖ);for(int i=0;i<Ő.Count;i++){IMyTextPanel ч=Ő[i];String Θ=ч.CustomData.ToUpper();if(Θ=="DEBUG") ч.WriteText(""+"\n"+"");if(Θ=="INSTRUCTIONS") ч.WriteText(ΖΑ());
}}String ů(bool Ι){int ΚΩ=0;return ů(Ι,0,ref ΚΩ,false,1);}String ů(bool Ι,int Κύ,ref int ΚЌ,bool ΖЎ,int Αά){String Γ="";String Ɵ="——————————————————\n";String ΛΙ="--------------------------------------------\n";if(Л==null||ό==Σ.Main||ό==Σ.ΓЊ)
Γ+="[PAM]-Controller | "+Ь.Count+" Connected ships"+"\n"+ΘЋ()+ΘА()+ΙΔ();else Γ+=ΖΕ(Л)+"\n";Γ+=Ɵ;int Β=0;int Η=Ў;Ŝ=0;if(Λ!=""){Ν(ref Γ,Η,Β++,Ι,Λ);}else if(ό==Σ.Main){if(Л==null&&Ь.Count>=1) Л=Ь[0];String Ѝ="";if(Ν(ref Ѝ,Η,Β++,Ι," Send command to all")) μ(Σ.ΓЊ);
int ƞ=0;for(int s=0;s<Ь.Count;s++){if(ΖЎ) Ѝ+="\n";Ѕ Δ=Ь[s];if(Η==Β||Η==Β+1) Л=Δ;if(Β==Η) ƞ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+ΖΕ(Δ))){Л=Δ;μ(Σ.ƽ);Л.ņ("",false,true,false);}if(Β==Η) ƞ=Ѝ.Split('\n').Length-1;if(Ν(ref Ѝ,Η,Β++,Ι," "+(ΖЎ?"":"  ")+ΙΑ(Δ)))
{Л=Δ;μ(Σ.Ζι);}}int ƪ=Αά-2;ƪ+=Math.Max(0,(Κύ-1))*Αά;Γ+=ΓЈ(ƪ,Ѝ,ƞ,ref ΚЌ);}else if(ό==Σ.ΒΓ||ό==Σ.ΓЊ){Σ ΚΑ=ό==Σ.ΒΓ?Σ.ƽ:Σ.Main;Ѕ η=null;if(ό==Σ.ΒΓ) η=Л;if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(ΚΑ);}if(Ν(ref Γ,Η,Β++,Ι," [Stop] "+Ų(η,"STOP"))){Ǭ(η,"STOP");}if(Ν(ref Γ,Η,Β++,Ι," [Continue] "+Ų(η,"CONT"))){Ǭ(η,"CONT");}
if(Ν(ref Γ,Η,Β++,Ι," [Move home] "+Ų(η,"HOMEPOS"))){Ǭ(η,"HOMEPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Move to job] "+Ų(η,"JOBPOS"))){Ǭ(η,"JOBPOS");}if(Ν(ref Γ,Η,Β++,Ι," [Full simulation] "+Ų(η,"FULL"))){Ǭ(η,"FULL");}if(η!=null&&η.Μ==Κ.ή)
if(Ν(ref Γ,Η,Β++,Ι," [Undock] "+Ų(η,"UNDOCK"))){Ǭ(η,"UNDOCK");}Γ+=ΘЈ(η);}else if(ό==Σ.ƽ){String ΔΣ="";if(Ν(ref ΔΣ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΔΣ.Substring(0,ΔΣ.Length-1).PadRight(36,' ');if(Ν(ref Γ,Η,Β++,Ι," Send cmd")){μ(Σ.ΒΓ);}if(!Л.ΖΦ()&&!Л.Γο()) Ŝ++;
if(Л.Γο()) Γ+="No answer received...";else Γ+=Ź(Л,Л.Βύ);}else if(ό==Σ.Ζι){if(Ν(ref Γ,Η,Β++,Ι," Back")){μ(Σ.Main);}Γ+=ΛΙ;Γ+=Ź(Л,ΖΔ(Л));}if(!ΕΓ) Ў=Math.Min(Ŝ-1,Ў);return Γ;}String ΖΕ(Ѕ Δ){if(Δ.ΓΘ!=VERSION) return Δ.ΰ+": Different version!";return Δ.ΰ+": "+Ź(Δ,Δ.Εή)+" "+Δ.Δι+"m";
}String ΙΑ(Ѕ Δ){String s=ΓЅ("",Й(Δ.Ȋ,Δ.ȑ)*100f,100f,8,0,0)+"% ";for(int i=0;i<Δ.Ќ.Count;i++){if(i>=5) break;м ώ=Δ.Ќ[i];s+=Ζτ(ΓΤ(ΘЏ(ώ)),3)+" "+ΕЌ(ώ.Ћ)+" ";}return s;}String ΘЏ(м ώ){if(ώ.υ.ToUpper()=="ORE"||ώ.υ.ToUpper()=="INGOT"){String ΖЌ=ΘЍ(ώ.ΰ.ToUpper());if(ΖЌ!="") return ΖЌ;
}return ώ.ΰ;}String Ź(Ѕ Δ,String Γ){if(Δ.ΖΦ()) return "No signal...("+ΑЁ((int)(DateTime.Now-Δ.ΔЄ).TotalSeconds)+")";return Γ;}String ΖΒ(bool Λί){String Γ="";if(Λί) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΙΪ(IMyTerminalBlock ч,out С Џ,out String Ǻ){bool ȱ=true;String ΑΪ=ΑА(Αώ(ч.CustomData.Split('\n'),"mode",ref ȱ)).ToUpper();String[]τ=ΑΪ.Split(':');String ΑΩ=τ.Length>0?τ[0].Trim():"";Ǻ=τ.Length>1?τ[1].Trim():"";
Џ=С.ĩ;if(ΑΩ=="MAIN") Џ=С.ΖΫ;else if(ΑΩ=="MAINX") Џ=С.Ζά;else if(ΑΩ=="MENU") Џ=С.Γτ;else if(ΑΩ=="INVENTORY") Џ=С.Γξ;else if(ΑΩ=="DEBUG") Џ=С.ΓΨ;if(Џ==С.ĩ) Ǻ=ΑΪ;}String Εϋ(С Џ,String Ǻ,bool ΚΪ){int ΑΦ=15;if(ΚΪ){ΖА=ΒΥ;ΒΥ=0;ΖБ=ΒΦ;ΒΦ=0;}String Γ="";if(Џ==С.ΖΫ)
{int ц=0;Γ=ů(false,ΖА,ref ΚΛ,true,ΑΦ);if(!int.TryParse(Ǻ,out ц)) return Γ;ΒΥ=Math.Max(ц,ΒΥ);ц--;return Ζΰ(ц,ΑΦ,Γ);}else if(Џ==С.Ζά){int ц=0;Γ=ů(false,ΖБ,ref ΚΜ,false,ΑΦ);if(!int.TryParse(Ǻ,out ц)) return Γ;ΒΦ=Math.Max(ц,ΒΦ);ц--;return Ζΰ(ц,ΑΦ,Γ);}if(Џ==С.ΖΪ){return ů(false,0,ref ΚΝ,true,ΑΦ);
}else if(Џ==С.Γξ||Џ==С.Γτ||Џ==С.ΓΨ){Ѕ Δ=Л;if(Ǻ!=""){Δ=Γα("",Ǻ);if(Δ==null) return "Unknown ship: "+Ǻ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String Ɵ="—————————————————————\n";String Εη="";String ΔΨ="";if(Џ==С.Γτ){Εη="Menu";ΔΨ=Ź(Δ,Δ.Βύ);}
else if(Џ==С.Γξ){Εη="Inventory";ΔΨ=Ź(Δ,ΖΔ(Δ));}else if(Џ==С.ΓΨ){return Ź(Δ,ΙΒ(Δ));}Γ+=Δ.ΰ+" - "+Δ.Δι+"m | "+ΓΤ(""+Εη)+"\n"+Ɵ;Γ+=ΔΨ;return Γ;}return "Unknown command: "+Ǻ;}string ΙΒ(Ѕ Δ){String s="";s+=Δ.ΓΘ+"\n";s+=Δ.ΰ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Εή+"\n";s+=Δ.Λ+"\n";s+=Δ.Ў+"\n";s+=Δ.Βύ.Length+"\n";s+=Δ.Ȋ+"\n";s+=Δ.ȑ+"\n";s+=Δ.Ќ.Count()+"\n";return s;}string ΖΔ(Ѕ Δ){String s="";s+=ΓЅ("All",Й(Δ.Ȋ,Δ.ȑ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ќ.Count();i++){м ώ=Δ.Ќ[i];
s+=ΓЅ(ώ.ΰ,ώ.Ћ,Δ.Εκ,30,8,10)+"\n";}return s;}Њ ΘЉ(Ѕ Δ,bool Ъ,string І){Њ ί=Њ.None;if(Δ==null){for(int i=0;i<Ь.Count;i++){Њ Є=Ь[i].ņ(І,false,Ъ,false);if(Є==Њ.ƺ) ί=Є;if(Є==Њ.ΑЍ) return Є;if(Є==Њ.ŷ) return Є;}return ί;}else return Δ.ņ(І,false,Ъ,false);}String Ų(Ѕ Δ,String І)
{if(!Ζκ(Δ,І)) return "";Њ Є=ΘЉ(Δ,false,І);if(Є==Њ.ƺ) return "received!";if(Є==Њ.ŷ) return "pending...";if(Є==Њ.ΑЍ) return "no answer!";return "";}String ΓЈ(int ƪ,String Γ,int ƞ,ref int ě){String[]Ξ=Γ.Split('\n');if(ƞ>=ě+ƪ-1) ě++;ě=Math.Min(Ξ.Count()-1-ƪ,ě);if(ƞ<ě+1) ě--;
ě=Math.Max(0,ě);String ί="";for(int i=0;i<ƪ;i++){int Ησ=i+ě;if(Ησ>=Ξ.Count()) break;ί+=Ξ[Ησ]+"\n";}return ί;}String Ζΰ(int ц,int Αά,String Γ){String[]Ξ=Γ.Split('\n');int Αδ=ц*Αά;int Ď=(ц+1)*Αά;String ί="";for(int i=Αδ;i<Ď;i++){if(i>=Ξ.Count()) break;ί+=Ξ[i]+"\n";
}return ί;}String ΓΤ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String Ζτ(String s,int Ηϊ){if(s=="") return s;if(s.Length>Ηϊ) s=s.Substring(0,Ηϊ-1)+".";return s;}string ΓЅ(String ř,float value,float Ə,int ΖЁ,int Ηξ,int Κό){float š=Й(value,Ə)*ΖЁ;
String ΒΡ="[";for(int i=0;i<ΖЁ;i++){if(i<=š) ΒΡ+="|";else ΒΡ+="'";}ΒΡ+="]";return ΒΡ+" "+Ζτ(ΓΤ(ř),Ηξ).PadRight(Ηξ)+"".PadRight(Κό)+ΕЌ(value);}String ΕЌ(float Ћ){if(Ћ>=1000000) return Math.Round(Ћ/1000000f,Ћ/1000000f<100?1:0)+"M";if(Ћ>=1000) return Math.Round(Ћ/1000f,Ћ/1000f<100?1:0)+"K";
return ""+Math.Round(Ћ);}String ΑЁ(int Αΰ){if(Αΰ>=60*60) return Math.Round(Αΰ/(60f*60f),1)+" h";if(Αΰ>=60) return Math.Round(Αΰ/60f,1)+" min";return ""+Αΰ+" s";}string ű(float v){return Math.Round(v,2)+" ";}string Λζ(Vector3 v){return "X"+ű(v.X)+"Y"+ű(v.Y)+"Z"+ű(v.Z);}
List<IMyCameraBlock>ȧ=new List<IMyCameraBlock>();List<long>Ɠ=new List<long>();bool ƕ=false;int Δή=0;int Δά=0;DateTime ΖЃ=DateTime.Now;String ΔΧ="";void Ιθ(){ȧ.Clear();Ɠ.Clear();if(!obstacleBrake) return;χ.GetBlocksOfType(ȧ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȧ.Count;i++){ȧ[i].EnableRaycast=true;if(!Ɠ.Contains(ȧ[i].CubeGrid.EntityId)) Ɠ.Add(ȧ[i].CubeGrid.EntityId);}if(!Ɠ.Contains(Me.CubeGrid.EntityId)) Ɠ.Add(Me.CubeGrid.EntityId);}bool ΖΤ(Α s){return s==Α.Ć||s==Α.у||s==Α.И||
s==Α.Ħ||s==Α.Ű||s==Α.Ʊ||s==Α.ΑϏ||
(s==Α.ȁ&&Ǡ!=null);}void Αϊ(){ƕ=false;Δή=0;ΔΧ="";}void ΙЃ(){if(!obstacleBrake||ȧ.Count==0||!ΖΤ(Ϊ)){Αϊ();return;}Vector3 ΑΙ=Ђ+ļ;Vector3 ȗ=ΑΙ-δ;float ȕ=ȗ.Length();if(ȕ<1f){Αϊ();return;}Vector3 ς=ȗ/ȕ;float Ηχ=Ĝ*obstacleLookAhead+ν;Vector3 ΚЄ=ΑΙ+ς*(ν*0.25f);
for(int n=0;n<ȧ.Count;n++){Δά=(Δά+1)%ȧ.Count;IMyCameraBlock ȇ=ȧ[Δά];if(!ȇ.IsFunctional||!ȇ.IsWorking) continue;Vector3 ΖЅ=ȇ.GetPosition();Vector3 Ď=ΚЄ;if(Vector3.Distance(ΖЅ,Ď)>Ηχ) Ď=ΖЅ+ς*Ηχ;if(!ȇ.CanScan(Ď)) continue;MyDetectedEntityInfo Ȭ=ȇ.Raycast(Ď);bool Κψ=!Ȭ.IsEmpty()&&!Ɠ.Contains(Ȭ.EntityId)&&!ΙΣ(Ȭ.EntityId)&&
Ȭ.Type!=MyDetectedEntityType.FloatingObject;if(Κψ){if(!ƕ) ΖЃ=DateTime.Now;ƕ=true;Δή=0;ΔΧ=Ȭ.Type.ToString();}else if(ƕ&&++Δή>=3) Αϊ();break;}if(ƕ&&(DateTime.Now-ΖЃ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΔΧ;}const String ΔΕ="[PAMTFC]";const float Ιυ=5f;const float Γΰ=2f;class Ĩ{public long Φ;public Vector3 κ,ǔ;public Vector3 đ;public float ΗΚ;public String Υ="";public int Ē=-1;public bool ΕΟ;public bool Ĵ;public float ё,Ǵ;public long α=0;public int ΑΒ=0;public DateTime Ļ;
}IMyBroadcastListener ΓΒ=null;long Ǐ=0;List<Ĩ>є=new List<Ĩ>();Vector3 ļ=new Vector3();float ǒ=-1;Vector3 Ελ=new Vector3();bool ΓΑ=false;DateTime Εμ=DateTime.Now;const float Ζσ=15f;const float Ιτ=10f;int Θέ=0;String г="";int Д=-1;bool Δβ=false,ΒΨ=false;Vector3 ΗΑ=new Vector3();
String ƶ(){if(γ==Π.э||Μ==Κ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ї+"|"+Ю+"|"+(int) Ř+"|"+Math.Round(ǿ,1)+"|"+Math.Round(ș,1);}void ΙЇ(){if(!trafficEnabled) return;if(ΓΒ==null) ΓΒ=IGC.RegisterBroadcastListener(ΔΕ);Ǐ=Me.CubeGrid.EntityId;int Ȇ=30;while(ΓΒ.HasPendingMessage&&Ȇ-->0){String Θ=ΓΒ.AcceptMessage().Data as String;if(Θ!=null) Ιά(Θ);
}for(int i=є.Count-1;i>=0;i--)
if((DateTime.Now-є[i].Ļ).TotalSeconds>Ιυ) є.RemoveAt(i);if(ΒЎ==null) ΒЎ=IGC.RegisterBroadcastListener(Ζρ);Ȇ=5;while(ΒЎ.HasPendingMessage&&Ȇ-->0){String Θ=ΒЎ.AcceptMessage().Data as String;if(Θ!=null) try{Ιέ(Θ);}catch{}}if(ΒΩ==null) ΒΩ=IGC.RegisterBroadcastListener(ΖΧ);
Ȇ=10;while(ΒΩ.HasPendingMessage&&Ȇ-->0){String Θ=ΒΩ.AcceptMessage().Data as String;if(Θ!=null) try{ΙΩ(Θ);}catch{}}if(++Θέ<3) return;Θέ=0;IGC.SendBroadcastMessage(ΔΕ,Ǐ+";"+σ(δ)+";"+
σ(Ϋ.GetShipVelocities().LinearVelocity)+";"+ν+";"+ƶ()+";"+
(γ==Π.В?Д:-1)+";"+г+";"+Θχ()+","+ΕЉ()+";"+σ(Ελ)+
";"+(Βα?1:0)+";"+(ĳ&&γ==Π.В?1:0)+","+Ť+","+Αα+","+(ΖΜ()?1:0));}void Ιά(String Θ){try{String[]f=Θ.Split(';');if(f.Length<7) return;long Φ=long.Parse(f[0]);if(Φ==Ǐ) return;Ĩ Δ=null;for(int i=0;i<є.Count;i++) if(є[i].Φ==Φ) Δ=є[i];if(Δ==null){Δ=new Ĩ();Δ.Φ=Φ;є.Add(Δ);}
Δ.κ=π(f[1]);Δ.ǔ=π(f[2]);Δ.ΗΚ=float.Parse(f[3]);Δ.Υ=f[4];Δ.Ē=int.Parse(f[5]);Δ.Ļ=DateTime.Now;if(Δ.Υ!=""&&Δ.Υ==ƶ()) Ζέ(f[6]);Δ.α=0;Δ.ΑΒ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.α=long.Parse(d[0]);Δ.ΑΒ=int.Parse(d[1]);}Δ.đ=f.Length>=9?π(f[8]):new Vector3();
Δ.ΕΟ=f.Length>=10&&f[9]=="1";Δ.Ĵ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Δ.ё=float.Parse(w[1]);Δ.Ǵ=float.Parse(w[2]);Δ.Ĵ=w[3]=="1";}}}catch{}}bool ΙΣ(long Φ){for(int i=0;i<є.Count;i++) if(є[i].Φ==Φ) return true;return false;}Vector3 ΖΓ(int Β,int Κώ)
{if(laneOffset<=0||Ψ.Count<2||Β<0||Β>=Ψ.Count) return new Vector3();ά έ=Ψ[Β];Vector3 ΛΧ=(Ψ[Math.Min(Ψ.Count-1,Β+1)].Ε-Ψ[Math.Max(0,Β-1)].Ε)*Κώ;Vector3 Б=έ.φ.LengthSquared()>0.01f?-Vector3.Normalize(έ.φ):-έ.θ;Vector3 р=Vector3.Cross(ΛΧ,Б);if(р.LengthSquared()<0.0001f) return new Vector3();
float ϊ=laneOffset*ν;float Κά=Math.Min(Vector3.Distance(έ.Ε,Ψ[0].Ε),
Vector3.Distance(έ.Ε,Ψ[Ψ.Count-1].Ε));return Vector3.Normalize(р)*ϊ*Math.Min(1,Κά/(ϊ*4));}Vector3 Θά=new Vector3();bool ΖΜ(){return γ==Π.В&&(Ϊ==Α.Ć||Ϊ==Α.ŵ||Ϊ==Α.Ƞ);}bool ΙΡ(Α s){return s==Α.ȁ||s==Α.Ŀ||s==Α.Ń||s==Α.ƻ||
s==Α.Ǯ||s==Α.Ƞ||s==Α.Ģ;}void ΙЈ(){ļ=new Vector3();ǒ=-1;Ελ=new Vector3();bool ΑΚ=ΖΤ(Ϊ);if(!trafficEnabled||(!ΑΚ&&!ΙΡ(Ϊ))){ΓΑ=false;return;}Θά=Ϋ.GetShipVelocities().LinearVelocity;Θω(Ϋ.GetNaturalGravity(),!ΑΚ);Θη();}IMyBroadcastListener ΒЎ=null;float Ƥ=-1;DateTime ΘΨ=DateTime.MinValue;
void Ιέ(String Θ){if(!Θ.StartsWith("C;")) return;ΘΨ=DateTime.Now;Ƥ=-1;String ΑΨ=Ǐ+":";String[]e=Θ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΨ)) Ƥ=float.Parse(e[i].Substring(ΑΨ.Length));}void Θη(){if(Ƥ<0||(DateTime.Now-ΘΨ).TotalSeconds>1.5) return;
ǒ=ǒ<0?Ƥ:Math.Min(ǒ,Ƥ);if(Ƥ<0.5f) Λ="Traffic: giving way (controller)";}void Θω(Vector3 φ,bool ΗЊ=false){ļ=new Vector3();ǒ=-1;Vector3 ȗ=Ђ-δ;float ȕ=ȗ.Length();bool Δό=ȕ>=1f;Vector3 ς=Δό?ȗ/ȕ:new Vector3();Ελ=ς;Vector3 Б=φ.LengthSquared()>0.01f?-Vector3.Normalize(φ):-ΑΔ;
Vector3 р=Vector3.Cross(ς,Б);р=р.LengthSquared()<0.0001f?-ΑΥ:Vector3.Normalize(р);Ĩ Βσ=null;bool ΗΫ=Ϊ==Α.И&&!ċ(1);for(int i=0;i<є.Count;i++){Ĩ ζ=є[i];if(ζ.ΑΒ==3&&ζ.ǔ.LengthSquared()<0.25f) continue;Vector3 κ=ζ.κ+ζ.ǔ*(float)(DateTime.Now-ζ.Ļ).TotalSeconds;Vector3 Ǣ=κ-δ;
float Ъ=(ν+ζ.ΗΚ)*0.5f;bool ΘΒ=ζ.Ĵ&&ΖΜ()&&ζ.ё>0&&Ť>0&&ζ.Υ==ƶ();if(ΘΒ) Ъ=Math.Max(Ť+ζ.ё,Αα+ζ.Ǵ)*0.5f;float ǃ=Δό?Vector3.Dot(Ǣ,ς):0;if(ǃ>0&&(Ǣ-ς*ǃ).Length()<=Ъ*1.2f){float ΒЁ=Vector3.Dot(ζ.ǔ,ς);bool Ηί=ζ.đ.LengthSquared()>0.25f;bool Ηΰ=ΒЁ<-1f||(Ηί&&Vector3.Dot(ζ.đ,ς)<-0.5f);
if(!ΗЊ){if(ζ.ΕΟ){if(!ΗΫ){ļ=р*Ъ*1.3f;continue;}float ΗϏ=Vector3.Distance(δ,Ђ),Ηλ=Vector3.Distance(κ,Ђ);if(ΗϏ<Ηλ||(ΗϏ==Ηλ&&Ǐ<ζ.Φ)) continue;}if(Ηΰ){if(ǃ<(Ĝ-ΒЁ)*6f+Ъ*4f) ļ=р*Ъ*1.3f;continue;}if(Ηί&&Ǐ<ζ.Φ){Vector3 ΖϏ=-Ǣ;float ΗЈ=Vector3.Dot(ΖϏ,ζ.đ);
if(ΗЈ>0&&(ΖϏ-ζ.đ*ΗЈ).Length()<=Ъ*1.2f){ļ=р*Ъ*1.3f;continue;}}}float ΗΪ=ǃ-Ъ-(ΘΒ?1f:followGap*ν);float ȏ=Math.Max(0,ΒЁ)+Math.Max(0,ΗΪ)/Γΰ;if(Ηΰ) ȏ=Math.Max(0,ΗΪ+Math.Min(0,ΒЁ)*Γΰ*2)/Γΰ;if(ǒ<0||ȏ<ǒ){ǒ=ȏ;Βσ=ζ;}continue;}if(ζ.ǔ.LengthSquared()<1f) continue;
if(Δό&&(Ǣ-ς*ǃ).Length()<=Ъ*1.2f) continue;Vector3 ΕΦ=ζ.ǔ-Θά;float ΗЏ=ΕΦ.LengthSquared();if(ΗЏ<0.25f) continue;float t=-Vector3.Dot(Ǣ,ΕΦ)/ΗЏ;if(t<=0||t>5f||(Ǣ+ΕΦ*t).Length()>=Ъ*1.1f) continue;if(Ǐ>ζ.Φ||t<2f){ǒ=0;Βσ=ζ;}}bool ΑΞ=ǒ>=0&&ǒ<0.5f;
if(!ΑΞ||ΗЊ||(Βσ!=null&&Βσ.ΕΟ&&ΗΫ)){ΓΑ=false;return;}if(!ΓΑ){ΓΑ=true;Εμ=DateTime.Now;}double Ηα=(DateTime.Now-Εμ).TotalSeconds;if(Ηα>Ζσ+Ιτ) Εμ=DateTime.Now;else if(Ηα>Ζσ){ǒ=-1;ļ=р*ν*1.3f;Λ="Traffic: passing a stuck ship";}}List<int>ȭ=new List<int>();
String Ηγ="";void Γί(){String ȯ=Ї+"|"+Ю+"|"+(int) Ř;if(ȯ==Ηγ) return;Ηγ=ȯ;Εό(Ї,Ю,Ř,ȭ);}void Εό(int w,int h,ю Αδ,List<int>Δέ){Δέ.Clear();if(Αδ==ю.ƿ){for(int k=0;k<w*h;k++){int ƚ=k/w;Δέ.Add(ƚ*w+(ƚ%2==0?k-ƚ*w:w-1-(k-ƚ*w)));}return;}int ΑΜ=(int) Math.Ceiling(w/2f),ΑΛ=(int) Math.Ceiling(h/2f);
int Δω=w/2,Δψ=h/2;int x=0,y=0,Δμ=0,Δν=-1,Ə=Math.Max(w,h);for(int i=0;i<Ə*Ə;i++){if(-ΑΜ<x&&x<=Δω&&-ΑΛ<y&&y<=Δψ) Δέ.Add((y-1+ΑΛ)*w+(x-1+ΑΜ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=Δμ;Δμ=-Δν;Δν=t;}x+=Δμ;y+=Δν;
}}bool ΖΛ(int a,int b,int w,float ΓΓ,float ΒЏ,float ё,float Ǵ){return a==b||(Math.Abs(a%w-b%w)*ΓΓ<ё+0.5f&&Math.Abs(a/w-b/w)*ΒЏ<Ǵ+0.5f);}bool Γή(){int n=Math.Max(0,Ї*Ю);if(г.Length==n) return false;г=new String('0',n);Д=-1;return true;}void Ζέ(String ζ)
{Γή();if(ζ.Length!=г.Length) return;char[]h=г.ToCharArray();for(int i=0;i<h.Length;i++) if(ζ[i]=='1') h[i]='1';г=new String(h);}bool ΑІ(int У){return У>=0&&У<г.Length&&г[У]=='1';}void ΓЌ(int У,bool Ϗ){if(У<0||У>=г.Length) return;char[]h=г.ToCharArray();h[У]=Ϗ?'1':'0';
г=new String(h);}bool Γλ(int У,bool Κι){if(!trafficEnabled) return false;String ȯ=ƶ();for(int i=0;i<є.Count;i++){Ĩ t=є[i];if(t.Υ!=ȯ||t.Ē<0||(Κι&&t.Φ>Ǐ)) continue;if(ΖΛ(У,t.Ē,Ї,ǿ,ș,Ť,Αα)) return true;}return false;}int Θφ(int Β){int n=Ї*Ю;if(n<1) return Β;Γί();
if(Γή()) for(int k=0;k<Β&&k<n;k++) ΓЌ(ȭ[k],true);if(Д>=0&&(Β>=n||ȭ[Β]!=Д)){ΓЌ(Д,true);Д=-1;}if(ΖΨ()){if(ȉ>=0&&!ΑІ(ȉ)){ĳ=false;Д=ȉ;return ȭ.IndexOf(ȉ);}if(ȉ==-2){Д=-1;ĳ=г.IndexOf('0')>=0;return ĳ?-2:n;}if(ȉ==-1&&Д>=0&&!ΑІ(Д)){ĳ=false;return Β;}
Д=-1;if(!ĳ){ĳ=true;ΗΓ=DateTime.Now;}if((DateTime.Now-ΗΓ).TotalSeconds<5) return-1;}ĳ=false;if(Д>=0&&!ΑІ(Д)&&!Γλ(Д,true)) return Β;Д=-1;for(int k=0;k<n;k++){int c=ȭ[k];if(ΑІ(c)||Γλ(c,false)) continue;Д=c;return k;}return г.IndexOf('0')>=0?-2:n;}const String ΖΧ="[PAMJOB]";
const String Ζρ="[PAMTCC]";IMyBroadcastListener ΒΩ=null;DateTime ΗΒ=DateTime.MinValue;DateTime ΗΓ=DateTime.Now;int ȉ=-1;bool ĳ=false;bool ΖΨ(){return trafficEnabled&&ȉ!=-3&&(DateTime.Now-ΗΒ).TotalSeconds<3;}void ΙΩ(String Θ){String[]f=Θ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ƶ()) return;
ΗΒ=DateTime.Now;Ζέ(f[2]);ȉ=-3;String ΑΨ=Ǐ+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ΑΨ)) ȉ=int.Parse(e[i].Substring(ΑΨ.Length));}ǭ Ιν(int Β){Γί();if(Β<0||Β>=ȭ.Count) return ǭ.Ατ;Ѐ=ȭ[Β]%Ї;ύ=ȭ[Β]/Ї;return ǭ.ΑЂ;}class Ǘ{public long Φ;
public Vector3 κ,Э,Б;}List<Ǘ>Ф=new List<Ǘ>();bool Į=false;Vector3 Βγ,Ββ,Βδ,Βε;long ѝ=0;long Ȳ=0;ά Ǡ=null;bool ΑΓ=false;ά ΓΫ(Vector3 κ,Vector3 Э,Vector3 Б){MatrixD m=MatrixD.CreateWorld(κ,Э,Б);ά w=new ά(Ο);w.ι=Vector3D.TransformNormal(Βγ,m);w.θ=Vector3D.TransformNormal(Ββ,m);
w.Ё=Vector3D.TransformNormal(Βδ,m);w.Ε=κ+(Vector3) Vector3D.TransformNormal(Βε,m);w.λ=true;return w;}int ΕЉ(){Α s=Ϊ;if(s==Α.ь) return з(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ń||s==Α.Ȥ||s==Α.ł||s==Α.ΒΛ||
s==Α.Αύ||s==Α.Ǯ) return 3;if(s==Α.ȁ||s==Α.Ŀ||s==Α.ƻ) return 2;if(s==Α.И) return 1;return 0;}long Θχ(){if(ΕЉ()==0) return 0;if(ċ(1)&&ơ!=0) return ơ;return Ǡ!=null?Ȳ:ѝ;}void ΖΩ(){if(Μ==Κ.ή||!Ο.λ) return;IMyShipConnector О=з(MyShipConnectorStatus.Connected);if(О==null||О.OtherConnector==null) return;
IMyShipConnector ζ=О.OtherConnector;Vector3I Κγ=new Vector3I((int) Ο.о.X,(int) Ο.о.Y,(int) Ο.о.Z);if(О.Position==Κγ){MatrixD Ɖ=MatrixD.Transpose(ζ.WorldMatrix);Βγ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Forward,Ɖ);Ββ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Down,Ɖ);Βδ=Vector3D.TransformNormal(Ϋ.WorldMatrix.Left,Ɖ);
Βε=Vector3D.TransformNormal(О.GetPosition()-ζ.GetPosition(),Ɖ);Į=true;}if(Vector3.Distance(О.GetPosition(),Ο.Ε)<5) ѝ=ζ.EntityId;List<IMyShipConnector>œ=new List<IMyShipConnector>();χ.GetBlocksOfType(œ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(œ.Count>0){Ф.Clear();for(int i=0;i<œ.Count;i++){Ǘ p=new Ǘ();p.Φ=œ[i].EntityId;p.κ=œ[i].GetPosition();p.Э=œ[i].WorldMatrix.Forward;p.Б=œ[i].WorldMatrix.Up;Ф.Add(p);}}Ǡ=null;Ȳ=0;if(ζ.EntityId==ѝ||!Į) return;for(int i=0;i<Ф.Count;i++)
if(Ф[i].Φ==ζ.EntityId){Ȳ=Ф[i].Φ;Ǡ=ΓΫ(Ф[i].κ,Ф[i].Э,Ф[i].Б);}}bool Γρ(long Φ,int ΗЂ){if(Φ==0) return false;for(int i=0;i<є.Count;i++){Ĩ t=є[i];if(t.α==Φ&&(t.ΑΒ>ΗЂ||(t.ΑΒ==ΗЂ&&t.Φ<Ǐ))) return true;}return false;}bool ΓΦ(int Ɩ){if(Μ==Κ.ή||!trafficEnabled){Ǡ=null;Ȳ=0;return true;}
long ο=Ǡ!=null?Ȳ:ѝ;if(!Γρ(ο,Ɩ)) return true;if(!Γρ(ѝ,Ɩ)){Ǡ=null;Ȳ=0;return true;}if(!Į) return false;Ǘ ǯ=null;float ǈ=0;for(int i=0;i<Ф.Count;i++){Ǘ p=Ф[i];if(p.Φ==ѝ||Γρ(p.Φ,Ɩ)) continue;float d=Vector3.Distance(p.κ,Ο.Ε);if(ǯ==null||d<ǈ){ǯ=p;ǈ=d;}
}if(ǯ==null) return false;Ȳ=ǯ.Φ;Ǡ=ΓΫ(ǯ.κ,ǯ.Э,ǯ.Б);return true;}String ΘЂ(){String s=(Į?"1":"0")+":"+σ(Βγ)+":"+σ(Ββ)+":"+
σ(Βδ)+":"+σ(Βε)+":"+ѝ;for(int i=0;i<Ф.Count;i++)
s+=":"+Ф[i].Φ+","+σ(Ф[i].κ)+","+σ(Ф[i].Э)+","+σ(Ф[i].Б);return s;}void ΘЁ(String s){Ф.Clear();Į=false;ѝ=0;String[]f=s.Split(':');if(f.Length<6) return;Į=f[0]=="1";Βγ=π(f[1]);Ββ=π(f[2]);Βδ=π(f[3]);Βε=π(f[4]);long.TryParse(f[5],out ѝ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ǘ p=new Ǘ();if(!long.TryParse(e[0],out p.Φ)) continue;p.κ=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);Ф.Add(p);}}const String ư="[PAMDCK]";const float Θξ=120f;class ą{public long Δ;public int ǝ;public long ќ;public bool ΖІ;public DateTime Ǜ,ΑΤ;
}IMyBroadcastListener Βΰ=null;List<ą>Х=new List<ą>();List<IMyShipConnector>ĝ=new List<IMyShipConnector>();List<IMySensorBlock>ă=new List<IMySensorBlock>();List<IMyCameraBlock>ΑΣ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>ΓΙ=new List<MyDetectedEntityInfo>();long Ǚ=0;int Δί=0;
long ΖЈ=0;DateTime ΖЉ=DateTime.Now;String ő="";int ΗΟ=0;IMyCubeGrid Ğ=null;void Ικ(){Ğ=Me.CubeGrid;χ.GetBlocksOfType(ĝ,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(dockPoolTag));χ.GetBlocksOfType(ă,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(zoneTag));
χ.GetBlocksOfType(ΑΣ,q=>q.CubeGrid.IsSameConstructAs(Ğ)&&q.CustomName.Contains(laneTag));for(int i=0;i<ă.Count;i++){ă[i].DetectSmallShips=true;ă[i].DetectLargeShips=true;}for(int i=0;i<ΑΣ.Count;i++) ΑΣ[i].EnableRaycast=true;}IMyShipConnector Ζμ(long Φ){if(Φ==0) return null;
IMyShipConnector c=χ.GetBlockWithId(Φ) as IMyShipConnector;return c!=null&&Ğ!=null&&c.CubeGrid.IsSameConstructAs(Ğ)?c:null;}bool Γπ(IMyShipConnector c){return c!=null&&c.IsFunctional&&c.Enabled&&c.Status==MyShipConnectorStatus.Unconnected;}bool ΖΥ(IMyShipConnector c,long Δ){for(int i=0;i<ΑΣ.Count;i++)
{IMyCameraBlock ȇ=ΑΣ[i];if(!ȇ.IsWorking||Vector3D.Distance(ȇ.GetPosition(),c.GetPosition())>10) continue;Vector3D Ď=ȇ.GetPosition()+c.WorldMatrix.Forward*dockLaneLength;if(!ȇ.CanScan(Ď)) continue;MyDetectedEntityInfo Ȭ=ȇ.Raycast(Ď);if(!Ȭ.IsEmpty()&&Ȭ.EntityId!=Ğ.EntityId&&Ȭ.EntityId!=Δ&&Ȭ.Type!=MyDetectedEntityType.FloatingObject)
{ő="Lane blocked: "+c.CustomName;return false;}}return true;}bool ΙΤ(){for(int s=0;s<ă.Count;s++){if(!ă[s].IsWorking) continue;ΓΙ.Clear();ă[s].DetectedEntities(ΓΙ);for(int i=0;i<ΓΙ.Count;i++){MyDetectedEntityInfo e=ΓΙ[i];bool Κή=e.Type==MyDetectedEntityType.SmallGrid||e.Type==MyDetectedEntityType.LargeGrid;
if(Κή&&e.EntityId!=Ǚ&&e.Velocity.Length()>0.5f){ő="Ship moving in dock area: "+e.Name;return true;}}}return false;}long Ιή(ą r){IMyShipConnector ќ=Ζμ(r.ќ);if(Γπ(ќ)&&ΖΥ(ќ,r.Δ)) return ќ.EntityId;if(!r.ΖІ||ќ==null) return 0;IMyShipConnector ǯ=null;double ǈ=0;for(int i=0;i<ĝ.Count;i++)
{IMyShipConnector c=ĝ[i];if(!Γπ(c)) continue;double d=Vector3D.Distance(c.GetPosition(),ќ.GetPosition());if(ǯ==null||d<ǈ){if(ΖΥ(c,r.Δ)){ǯ=c;ǈ=d;}}}return ǯ==null?0:ǯ.EntityId;}void ΙΕ(){ą Ɨ=null;long О=0;for(int ş=2;ş>=1&&Ɨ==null;ş--){ą Ş=null;for(int i=0;i<Х.Count;i++)
{ą r=Х[i];if(r.ǝ!=ş) continue;if(ş==1){long c=Ιή(r);if(c==0) continue;if(Ş==null||r.Ǜ<Ş.Ǜ){Ş=r;О=c;}}else if(Ş==null||r.Ǜ<Ş.Ǜ) Ş=r;}Ɨ=Ş;}if(Ɨ==null){if(Х.Count>0&&ő=="") ő="No free dock";return;}Ǚ=Ɨ.Δ;Δί=Ɨ.ǝ;ΖЈ=Ɨ.ǝ==1?О:0;ΖЉ=DateTime.Now;
}void Ιύ(){if(Ğ==null) return;if(Βΰ==null) Βΰ=IGC.RegisterBroadcastListener(ư);int Ȇ=30;while(Βΰ.HasPendingMessage&&Ȇ-->0){String Θ=Βΰ.AcceptMessage().Data as String;if(Θ!=null) ΙΖ(Θ);}Ιγ();if(++ΗΟ<3) return;ΗΟ=0;IGC.SendBroadcastMessage(ư,Θμ());}void ΙΖ(String Θ){try{String[]f=Θ.Split(';');
long Δ=long.Parse(f[1]);if(f[0]=="D"){for(int i=Х.Count-1;i>=0;i--) if(Х[i].Δ==Δ) Х.RemoveAt(i);if(Ǚ==Δ) Ǚ=0;}else if(f[0]=="R"&&f.Length>=5){long ќ=long.Parse(f[3]);if(Ζμ(ќ)==null) return;ą r=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Δ) r=Х[i];if(r==null){r=new ą();r.Δ=Δ;r.Ǜ=DateTime.Now;Х.Add(r);}
int ǝ=int.Parse(f[2]);if(r.ǝ!=ǝ) r.Ǜ=DateTime.Now;r.ǝ=ǝ;r.ќ=ќ;r.ΖІ=f[4]=="1";r.ΑΤ=DateTime.Now;}}catch{}}void Ιγ(){for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].ΑΤ).TotalSeconds>3) Х.RemoveAt(i);ą ΒΤ=null;for(int i=0;i<Х.Count;i++) if(Х[i].Δ==Ǚ) ΒΤ=Х[i];if(Ǚ!=0&&ΒΤ==null) Ǚ=0;if(ΒΤ!=null&&(DateTime.Now-ΖЉ).TotalSeconds>Θξ){Ǚ=0;ΒΤ.Ǜ=DateTime.Now;}ő="";if(Ǚ==0&&Х.Count>0&&!ΙΤ()) ΙΕ();
}String Θμ(){String β="S;"+Ğ.EntityId+";"+Ǚ+";"+Δί+";"+ΖЈ+";";for(int i=0;i<Х.Count;i++) β+=(i>0?",":"")+Х[i].Δ;β+=";";for(int i=0;i<ĝ.Count;i++){IMyShipConnector c=ĝ[i];β+=(i>0?":":"")+c.EntityId+","+σ(c.GetPosition())+","+σ(c.WorldMatrix.Forward)+","+σ(c.WorldMatrix.Up);
}β+=";";for(int i=0;i<ă.Count;i++){IMySensorBlock z=ă[i];double r=new Vector3D(Math.Max(z.LeftExtend,z.RightExtend),Math.Max(z.TopExtend,z.BottomExtend),Math.Max(z.FrontExtend,z.BackExtend)).Length();β+=(i>0?":":"")+σ(z.GetPosition())+","+Math.Round(r,1);}return β;}String ΘЋ()
{if(ĝ.Count==0&&ă.Count==0) return "";int ΑΚ=0;for(int i=0;i<ĝ.Count;i++) if(Γπ(ĝ[i])) ΑΚ++;return "Docks: "+ΑΚ+"/"+ĝ.Count+" free"+(Ǚ!=0?(Δί==1?" | docking":" | undocking"):"")+
(Х.Count>1?" | "+(Х.Count-1)+" waiting":"")+(ő!=""?"\n"+ő:"")+"\n";}IMyBroadcastListener ΒЌ=null;int ȫ=0;DateTime ΗΞ=DateTime.Now;DateTime ΘΖ=DateTime.MinValue;long ΘΗ=0;int ΘΘ=0;long ơ=0;bool Απ=false;int Δλ=0;List<Vector3>Ας=new List<Vector3>();List<float>Εψ=new List<float>();
bool Βα=false;bool Βί=false;Vector3 ΗΝ=new Vector3();bool ƾ(){return ȫ!=0&&(DateTime.Now-ΘΖ).TotalSeconds<3;}bool ċ(int ǝ){return ƾ()&&ΘΗ==Ǐ&&ΘΘ==ǝ;}bool Ζν(){return ƾ()||(DateTime.Now-ΗΞ).TotalSeconds<3;}void ΓЇ(int ǝ){if(ȫ==ǝ) return;ȫ=ǝ;ΗΞ=DateTime.Now;Δλ=3;
}void ΓІ(){if(ȫ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ư,"D;"+Ǐ);ȫ=0;Απ=false;}void Ιό(){if(!trafficEnabled||Μ==Κ.ή) return;if(ΒЌ==null) ΒЌ=IGC.RegisterBroadcastListener(ư);int Ȇ=20;while(ΒЌ.HasPendingMessage&&Ȇ-->0){String Θ=ΒЌ.AcceptMessage().Data as String;if(Θ==null||!Θ.StartsWith("S;")) continue;
try{ΙΫ(Θ);}catch{}}if(Ϊ!=Α.И){Βα=false;Βί=false;}if(ȫ==1&&з(MyShipConnectorStatus.Connected)!=null) ΓІ();if(ȫ==2&&!Απ&&Ϊ!=Α.Ǯ&&
Vector3.Distance(δ,ы().Ε)>ν*3) ΓІ();if(ȫ!=0&&++Δλ>=3){Δλ=0;IGC.SendBroadcastMessage(ư,"R;"+Ǐ+";"+ȫ+";"+ѝ+";"+(Į?1:0));}}void ΙΫ(String Θ){String[]f=Θ.Split(';');if(f.Length<7) return;bool ΗΩ=false;String[]Ζφ=f[5].Split(',');for(int i=0;i<Ζφ.Length;i++) if(Ζφ[i]==Ǐ.ToString()) ΗΩ=true;
if(!ΗΩ) return;ΘΖ=DateTime.Now;ΘΗ=long.Parse(f[2]);ΘΘ=int.Parse(f[3]);ơ=long.Parse(f[4]);Ας.Clear();Εψ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ας.Add(π(e[0]));Εψ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]ΗΡ=f[6].Split(':');List<Ǘ>Ѝ=new List<Ǘ>();for(int i=0;i<ΗΡ.Length;i++){String[]e=ΗΡ[i].Split(',');if(e.Length<4) continue;Ǘ p=new Ǘ();p.Φ=long.Parse(e[0]);p.κ=π(e[1]);p.Э=π(e[2]);p.Б=π(e[3]);Ѝ.Add(p);}Ф=Ѝ;}bool Θτ(){if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return true;
ΓЇ(1);if(!Ζν()) return true;if(!ċ(1)){Λ="Waiting for dock clearance";return false;}ΓΝ();return true;}bool ΓΝ(){ΑΓ=false;Ǘ Š=null;if(ơ!=0&&ơ!=ѝ&&Į)
for(int i=0;i<Ф.Count;i++) if(Ф[i].Φ==ơ) Š=Ф[i];long ƫ=Š==null?0:Š.Φ;if(ƫ==Ȳ&&(ƫ==0)==(Ǡ==null)) return false;Ȳ=ƫ;Ǡ=Š==null?null:ΓΫ(Š.κ,Š.Э,Š.Б);return true;}bool Ŵ(Vector3 p){for(int i=0;i<Ας.Count;i++) if(Vector3.Distance(p,Ας[i])<Εψ[i]+ν*0.75f) return true;
return false;}bool ΙΚ(Vector3 ǌ,Vector3 ŝ){Βα=false;if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return false;if(ȫ!=1&&Vector3.Distance(δ,Ο.Ε)>dockRequestDistance) return false;ΓЇ(1);if(!ƾ()||ċ(1)||Ας.Count==0) return false;if(!Ŵ(ǌ)&&!Ŵ(ŝ)&&!Ŵ(δ)) return false;Βα=true;return true;
}bool ΕЃ(){if(!trafficEnabled||Μ==Κ.ή||ѝ==0) return true;ΓЇ(2);if(!Ζν()) return true;if(!ċ(2)){Λ="Waiting for undock clearance";return false;}return true;}const float ΙΥ=600f;class Ĉ{public long Φ;public int У=-1;public bool ƫ;public float ё,Ǵ;public DateTime ΕΧ;}class ȟ{public String ȯ;
public int w,h;public float ΓΓ,ΒЏ;public String Ϗ;public List<int>ΕΘ=new List<int>();public List<Ĉ>с=new List<Ĉ>();}List<ȟ>Ȏ=new List<ȟ>();IMyBroadcastListener Βς=null;int Ημ=0;void ΙЁ(){if(Βς==null) Βς=IGC.RegisterBroadcastListener(ΔΕ);int Ȇ=50;while(Βς.HasPendingMessage&&Ȇ-->0)
{String Θ=Βς.AcceptMessage().Data as String;if(Θ==null) continue;try{ΙΘ(Θ);}catch{}try{ΙΙ(Θ);}catch{}}for(int j=Ȏ.Count-1;j>=0;j--){ȟ Υ=Ȏ[j];for(int i=Υ.с.Count-1;i>=0;i--)
if((DateTime.Now-Υ.с[i].ΕΧ).TotalSeconds>ΙΥ) Υ.с.RemoveAt(i);if(Υ.с.Count==0){Ȏ.RemoveAt(j);continue;}Θθ(Υ);}if(++Ημ<3) return;Ημ=0;for(int j=0;j<Ȏ.Count;j++) IGC.SendBroadcastMessage(ΖΧ,Θλ(Ȏ[j]));}void Θθ(ȟ Υ){for(int i=0;i<Υ.с.Count;i++){Ĉ s=Υ.с[i];if(s.ƫ&&(s.У<0||Υ.Ϗ[s.У]=='1')) s.У=ΙΧ(Υ,s);
}}String Θλ(ȟ Υ){String β="J;"+Υ.ȯ+";"+Υ.Ϗ+";";bool Ǜ=true;for(int i=0;i<Υ.с.Count;i++){if((DateTime.Now-Υ.с[i].ΕΧ).TotalSeconds>5) continue;β+=(Ǜ?"":",")+Υ.с[i].Φ+":"+Υ.с[i].У;Ǜ=false;}return β;}void ΙΘ(String Θ){String[]f=Θ.Split(';');if(f.Length<11||f[4]=="") return;
String[]k=f[4].Split('|');if(k.Length<8) return;ȟ Υ=null;for(int i=0;i<Ȏ.Count;i++) if(Ȏ[i].ȯ==f[4]) Υ=Ȏ[i];if(Υ==null){Υ=new ȟ();Υ.ȯ=f[4];Υ.w=int.Parse(k[3]);Υ.h=int.Parse(k[4]);Υ.ΓΓ=float.Parse(k[6]);Υ.ΒЏ=float.Parse(k[7]);if(Υ.w<1||Υ.h<1) return;Εό(Υ.w,Υ.h,(ю) int.Parse(k[5]),Υ.ΕΘ);
Υ.Ϗ=new String('0',Υ.w*Υ.h);Ȏ.Add(Υ);}String d=f[6];if(d.Length==Υ.Ϗ.Length){char[]c=Υ.Ϗ.ToCharArray();for(int i=0;i<c.Length;i++) if(d[i]=='1') c[i]='1';Υ.Ϗ=new String(c);}long Φ=long.Parse(f[0]);Ĉ s=null;for(int i=0;i<Υ.с.Count;i++) if(Υ.с[i].Φ==Φ) s=Υ.с[i];
if(s==null){s=new Ĉ();s.Φ=Φ;Υ.с.Add(s);}s.ΕΧ=DateTime.Now;String[]w=f[10].Split(',');s.ƫ=w[0]=="1";if(w.Length>=3){s.ё=float.Parse(w[1]);s.Ǵ=float.Parse(w[2]);}int Ē=int.Parse(f[5]);if(Ē>=0&&Ē<Υ.Ϗ.Length&&(s.У<0||Υ.Ϗ[s.У]=='1')&&Υ.Ϗ[Ē]!='1'&&ΖΚ(Υ,Ē,s)==null)
s.У=Ē;}Ĉ ΖΚ(ȟ Υ,int У,Ĉ Δπ){for(int i=0;i<Υ.с.Count;i++){Ĉ o=Υ.с[i];if(o!=Δπ&&o.У>=0&&ΖΛ(У,o.У,Υ.w,Υ.ΓΓ,Υ.ΒЏ,Math.Max(o.ё,Δπ.ё),Math.Max(o.Ǵ,Δπ.Ǵ)))
return o;}return null;}int ΙΧ(ȟ Υ,Ĉ s){for(int k=0;k<Υ.ΕΘ.Count;k++){int c=Υ.ΕΘ[k];if(Υ.Ϗ[c]!='1'&&ΖΚ(Υ,c,s)==null) return c;}return-2;}String ΘА(){String s="";for(int j=0;j<Ȏ.Count;j++){ȟ Υ=Ȏ[j];int Ϗ=0;for(int i=0;i<Υ.Ϗ.Length;i++) if(Υ.Ϗ[i]=='1') Ϗ++;s+="Job "+(j+1)+": "+Ϗ+"/"+Υ.Ϗ.Length+" holes, "+Υ.с.Count+" ships\n";
}return s;}const float Ις=8f;class ȣ{public long Φ;public Vector3 κ,ǔ,đ;public float Δη,ё,Ǵ,ȏ=-1;public bool Ĵ;public String Υ="";public int Ɩ;public DateTime Ļ;}class ΒΗ{public long a,b,Θζ;public DateTime ΑΤ;}List<ȣ>Ш=new List<ȣ>();List<ΒΗ>ȳ=new List<ΒΗ>();int ΘΧ=0;void ΙΙ(String Θ)
{String[]f=Θ.Split(';');if(f.Length<9) return;long Φ=long.Parse(f[0]);ȣ s=null;for(int i=0;i<Ш.Count;i++) if(Ш[i].Φ==Φ) s=Ш[i];if(s==null){s=new ȣ();s.Φ=Φ;Ш.Add(s);}s.κ=π(f[1]);s.ǔ=π(f[2]);s.Δη=float.Parse(f[3]);s.Υ=f[4];s.Ɩ=int.Parse(f[7].Split(',')[1]);s.đ=π(f[8]);
s.Ĵ=false;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){s.ё=float.Parse(w[1]);s.Ǵ=float.Parse(w[2]);s.Ĵ=w[3]=="1";}}s.Ļ=DateTime.Now;}bool ΔΔ(ȣ s){return s.ǔ.LengthSquared()<0.25f&&(s.Ɩ==3||s.đ.LengthSquared()<0.25f);}Vector3 Ζς(ȣ s){return s.ǔ.LengthSquared()>=1f?s.ǔ:s.đ*2f;}
void ΙЉ(){Θϊ();if(++ΘΧ<3) return;ΘΧ=0;IGC.SendBroadcastMessage(Ζρ,Θν());}void Θϊ(){for(int i=Ш.Count-1;i>=0;i--)
if((DateTime.Now-Ш[i].Ļ).TotalSeconds>3) Ш.RemoveAt(i);for(int i=ȳ.Count-1;i>=0;i--)
if((DateTime.Now-ȳ[i].ΑΤ).TotalSeconds>2) ȳ.RemoveAt(i);for(int i=0;i<Ш.Count;i++) Ш[i].ȏ=-1;for(int i=0;i<Ш.Count;i++)
for(int j=i+1;j<Ш.Count;j++){ȣ a=Ш[i],b=Ш[j];if(ΔΔ(a)||ΔΔ(b)) continue;Vector3 Κϋ=a.κ+a.ǔ*(float)(DateTime.Now-a.Ļ).TotalSeconds;Vector3 ΚϏ=b.κ+b.ǔ*(float)(DateTime.Now-b.Ļ).TotalSeconds;Vector3 Ʀ=Ζς(a),Ƨ=Ζς(b);Vector3 Ǣ=ΚϏ-Κϋ;float ϊ=Ǣ.Length();float Ъ=(a.Δη+b.Δη)*0.55f;
if(a.Ĵ&&b.Ĵ&&a.Υ!=""&&a.Υ==b.Υ&&a.ё>0&&b.ё>0)
Ъ=Math.Max(a.ё+b.ё,a.Ǵ+b.Ǵ)*0.5f+0.5f;Vector3 ΕΥ=Ƨ-Ʀ;float ΗЎ=ΕΥ.LengthSquared();float t=ΗЎ>0.01f?-Vector3.Dot(Ǣ,ΕΥ)/ΗЎ:0;bool Ει=ϊ<Ъ;if(!Ει&&(t<=0||t>Ις||(Ǣ+ΕΥ*t).Length()>=Ъ)) continue;if(!Ει&&Ʀ.LengthSquared()>0.25f&&Ƨ.LengthSquared()>0.25f&&Vector3.Dot(Vector3.Normalize(Ʀ),Vector3.Normalize(Ƨ))<-0.7f)
continue;ΒΗ p=null;for(int k=0;k<ȳ.Count;k++)
if((ȳ[k].a==a.Φ&&ȳ[k].b==b.Φ)||(ȳ[k].a==b.Φ&&ȳ[k].b==a.Φ)) p=ȳ[k];if(p==null){p=new ΒΗ();p.a=a.Φ;p.b=b.Φ;p.Θζ=Ισ(a,b,Ʀ,Ƨ,Ǣ,ϊ);ȳ.Add(p);}p.ΑΤ=DateTime.Now;ȣ y=p.Θζ==a.Φ?a:b;float Ης=Ει||t<4?0:Math.Max(1,y.ǔ.Length()*0.5f);if(y.ȏ<0||Ης<y.ȏ) y.ȏ=Ης;
}}long Ισ(ȣ a,ȣ b,Vector3 Ʀ,Vector3 Ƨ,Vector3 Ǣ,float ϊ){if(a.Φ==Ǚ) return b.Φ;if(b.Φ==Ǚ) return a.Φ;Vector3 n=ϊ>0.01f?Ǣ/ϊ:new Vector3(1,0,0);float ΘΚ=Vector3.Dot(Ʀ,n),ΘΦ=Vector3.Dot(Ƨ,-n);if(ΘΚ-ΘΦ>1) return a.Φ;if(ΘΦ-ΘΚ>1) return b.Φ;return a.Φ>b.Φ?a.Φ:b.Φ;
}String Θν(){String β="C;";bool Ǜ=true;for(int i=0;i<Ш.Count;i++){if(Ш[i].ȏ<0) continue;β+=(Ǜ?"":",")+Ш[i].Φ+":"+Math.Round(Ш[i].ȏ,1);Ǜ=false;}return β;}String ΙΔ(){int ΗЁ=0,ΑΞ=0;for(int i=0;i<Ш.Count;i++){if(!ΔΔ(Ш[i])) ΗЁ++;if(Ш[i].ȏ>=0) ΑΞ++;}if(Ш.Count==0) return "";
return "Traffic: "+ΗЁ+" moving"+(ΑΞ>0?", "+ΑΞ+" giving way":"")+"\n";}