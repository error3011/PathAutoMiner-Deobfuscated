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
public enum Μ{ȳ,Ž,ǖ,ы,ή}public enum Β{т,ȕ,ŀ,Ș,Ę,а,й,
ȱ,ļ,Ǿ,Ļ,ĸ,Ē,
ǋ,Ȇ,ț,ĕ,ΑΚ,ƺ,
ΒЉ,ΑΙ,Ŷ,Ƽ,Ā}public enum Π{у,Ł,З,ΒΠ,ƶ,Ǆ,ǅ,ĵ,Ķ}public enum ъ{None,ĕ,т,Δε}public enum Ρ{Main,ΔЌ,ż,Ż,ΒЅ,ƴ,
ΑБ,Δω,Δϊ,Δψ,ƀ,ΕΑ,
ΑΒ,Βύ,ΒЀ,ΕΒ,ΑΔ}public enum м{ΑΑ,ŵ,Ηέ,Δύ,ή}public enum ǘ{ΑΖ,Δί}public enum с{ǌ,ΒΤ,ΕΓ,Ÿ}public enum Љ{Βξ,Δβ,ΒΜ,ΒΡ,Ƹ,Βί,ǀ}public enum љ{ΒΞ,ΒΕ,ΒΖ}public enum С{Ŀ,ΗΔ,ē}public enum θ{Βπ,Βτ,Βυ,ΔЇ,ΔІ,
Βο,ǈ,Ǉ,Βς,
Βσ,Βρ}public enum ΰ{Ȁ,ȗ,ǿ,Ƴ,ΒΓ,ΒΔ}public enum Ț{ΒΥ,ΒΒ,ΖЂ}public enum ь{ΑΕ,Δι,ΗΜ,Βμ}public enum Л{None,ž,Βϊ,Βν}public enum υ{Ƃ,Ʋ,ǂ,Ÿ,Βκ,ǃ,Βλ,Ηα,Βφ}public enum М{ȳ,ΔЄ,ΔЃ,Βη,Βα,ΒΝ,ΔЅ}class Ϊ{public bool δ=false;public Vector3 Ε=new Vector3();
public Vector3 β=new Vector3();public Vector3 γ=new Vector3();public Vector3 Ϗ=new Vector3();public Vector3 ρ=new Vector3();public Vector3 г=new Vector3();public float Р=0;public float ΙΟ=0;public float[]Ǩ=null;public Ϊ(){}public Ϊ(Ϊ Њ){δ=Њ.δ;Ε=Њ.Ε;β=Њ.β;γ=Њ.γ;Ϗ=Њ.Ϗ;ρ=Њ.ρ;
г=Њ.г;Ǩ=Њ.Ǩ;}public Ϊ(Vector3 Ε,Vector3 γ,Vector3 β,Vector3 Ϗ,Vector3 ρ){this.Ε=Ε;this.β=β;this.γ=γ;this.Ϗ=Ϗ;this.Р=0;this.ρ=ρ;}public void Ζο(List<IMyThrust>ΑΝ,List<string>Ȑ){Ǩ=new float[Ȑ.Count];for(int i=0;i<Ǩ.Length;i++)
Ǩ[i]=-1;for(int i=0;i<ΑΝ.Count;i++){string ј=Δφ(ΑΝ[i]);int Α=Ȑ.IndexOf(ј);if(Α!=-1)
Ǩ[Α]=Ќ(ΑΝ[i].MaxEffectiveThrust,ΑΝ[i].MaxThrust);}}}class Βχ{public Ϊ μ=null;public List<Vector3>Ŭ=new List<Vector3>();public float Ƌ=0;public float ΔΡ=0;public float Ŋ=0;public float ΓΪ=0;public Vector3 Αγ=new Vector3();}class ņ{public θ К=θ.Βπ;public float Ǜ=0;public float ą=0;
public string ǜ="";public string ǟ="";DateTime ΕЂ;public bool ΓІ=false;private bool ΓΠ=false;public bool ΒΙ(bool ǥ){if(ǥ){ą=0;ΓΠ=false;return false;}ΓΠ=true;return ą>Ǜ;}public void ΗΠ(){ΒΙ(true);ΓІ=false;}public void Ην(){if(ΓΠ)
if((DateTime.Now-ΕЂ).TotalSeconds>1){ą++;ΕЂ=DateTime.Now;}}public bool Βέ(){switch(К){case θ.Βτ:return true;case θ.Βυ:return true;}return false;}}class б{public String έ="";public String π="";public int Ѓ=0;public С ś=С.ē;public б(String έ,String π,int Ѓ,С ś){this.έ=έ;this.π=π;
this.Ѓ=Ѓ;this.ś=ś;}}class Ζύ{public Ζύ(Vector3 ι,float Є){this.ι=ι;this.Є=Є;}public Vector3 ι;public float Є;}class И{public DateTime Γλ;public DateTime Εά;public String ς="";public String έ="";public String ΑЏ="";public String Λ="";public String ΔΑ="";public String Αω="";public String ΓЄ="";
public Vector3 Ε=new Vector3();public List<б>Ѕ=new List<б>();public Μ Ξ=Μ.ȳ;public Л Ă;public float ǵ;public float ǯ;public int І;public int ΓΦ=0;public int ΔΝ=0;public bool ΔЁ(){return(DateTime.Now-Γλ).TotalSeconds>10;}public bool Ββ(){if(Ă!=Л.ž) return false;return(DateTime.Now-Εά).TotalSeconds>=4;
}public И(String ς){this.ς=ς;}public Л ł(String Г,bool Θφ,bool ȋ,bool Ηύ){if(Г==""&&!ȋ) return Л.None;if(Ă==Л.ž&&Ββ())
Ă=Л.Βν;if(Θφ){ΓЄ=Г;Ă=Л.ž;Εά=DateTime.Now;return Ă;}else if(ȋ){ΓЄ="";Ă=Л.None;}else if(ΓЄ==Г){if(Ηύ) Ă=Л.Βϊ;return Ă;}return Л.None;}}public String ΖЉ(String ΘΡ){switch(ΘΡ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem ό;Vector3 ΑЀ=new Vector3();Vector3 ζ=new Vector3();Vector3 Αζ=new Vector3();Vector3 ƙ=new Vector3();Vector3 ƍ=new Vector3();
DateTime Γξ=new DateTime();bool Ȟ=true;int ΔΗ=0;bool ȣ=false;bool Αξ=false;bool ƞ=true;bool Ζλ=false;bool Ř=false;bool Αλ=false;float Ȫ=0;float į=0;int ď=0;int ƭ=0;int Ƣ=0;int ΓΙ=0;float ŝ=0;float ŋ=0;double Γϊ=0;float Ɔ=0;List<int>ŕ=new List<int>();List<int>ť=new List<int>();
Μ Ξ=Μ.ȳ;int Ј=0;float Ȓ=0;float ǽ=0;float λ=0;IMyRemoteControl Ψ;IMySensorBlock σ;List<IMyTimerBlock>ΔΚ=new List<IMyTimerBlock>();List<IMyShipConnector>ю=new List<IMyShipConnector>();List<IMyThrust>ȑ=new List<IMyThrust>();List<IMyGyro>Αη=new List<IMyGyro>();List<IMyTerminalBlock>τ=new List<IMyTerminalBlock>();
List<IMyLandingGear>ȏ=new List<IMyLandingGear>();List<IMyReactor>Ď=new List<IMyReactor>();List<IMyConveyorSorter>ΑЅ=new List<IMyConveyorSorter>();List<IMyGasTank>ѝ=new List<IMyGasTank>();List<IMyTerminalBlock>ƃ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ŏ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ğ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ā=new List<IMyBatteryBlock>();List<IMyTextPanel>П=new List<IMyTextPanel>();List<IMyTextSurface>Ǻ=new List<IMyTextSurface>();List<IMyTextPanel>Ģ=new List<IMyTextPanel>();IMyTerminalBlock Ǎ=null;Ϊ Ν=new Ϊ();Ϊ Γδ=new Ϊ();
Ϊ Γη=new Ϊ();bool ȓ=false;bool ů=false;bool Ű=false;bool Αν=false;List<String>Ȑ=new List<string>();List<Ϊ>Φ=new List<Ϊ>();List<Ϊ>Ʊ=new List<Ϊ>();int ΓΧ=0;double Ζμ=0;Ϊ Ζ=new Ϊ();Vector3 Ǣ;Vector3 ȧ;Π α=Π.у;ǘ Ƙ=ǘ.ΑΖ;int Ў=0;int О=0;double Ɨ=0;bool ΑΪ=false;ņ Ё=new ņ();
ņ Ђ=new ņ();с ч=с.ǌ;bool Đ=true;ǘ Ǹ=ǘ.ΑΖ;int х=3;int ф=3;int в=30;љ я=љ.ΒΞ;bool ȭ=true;bool ŗ=true;bool Ő=true;bool İ=false;Љ Ш=Љ.Βξ;bool Ȭ=true;float Ǵ=90;float ђ=20;float е=5;float ѓ=20;float Ǭ=0.70f;float ų=1.50f;float Ų=2.50f;float Ĵ=0f;float ć=0f;Β Ω;
Π ё;Β Ʀ;Βχ κ=null;int ύ=0;int ψ=0;int Ь=0;int ȥ=0;int ĭ=30;int џ=0;bool А=true;Vector3 є;double Ч=0;bool ΑЂ=false;int ΔΦ=0;int Б=0;int ΒА=0;int Č=0;int ΖΘ=0;Vector3 Ơ=new Vector3();float ц=0;float ж=0;int ĳ=0;Ϊ ΑІ=null;float Γμ=0;float ΑΠ=0;float Э=0;float ĩ=0;
bool Γε=false;bool ı=false;bool Ʈ=false;bool ȡ=false;bool Ȝ=false;DateTime ΗЎ=new DateTime();Ϊ Ġ=null;Ϊ Εβ=null;float Ȉ=0;ъ ǭ;float Ȣ=0;float Ĳ=0;String Γτ="";float Απ=0;float ǯ=0;float ǵ=0;List<б>Ǟ=new List<б>();float Щ=0;bool Ƥ=false;bool Ζα=false;bool ΖΣ=false;
bool ΑЈ=false;bool ΖΤ=false;float Γψ=0;float ΓЃ=0;Vector3 Ερ=new Vector3();Vector3 Ѐ=new Vector3();Vector3 ΑЉ=new Vector3(1,1,1);float ΔΖ=1;Vector3 ƪ=new Vector3();bool Αμ=false;bool Γΰ=false;bool Γρ=false;bool ΕΛ=false;float ΑΨ=0;float ΑΜ=2;Vector3 Ζΰ;Vector3 ΔΕ;Vector3 ΔΔ;
float[,]ǩ=new float[3,2];Dictionary<String,float[,]>Ǒ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζι=new Dictionary<String,float>();int ΓΓ=0;Dictionary<String,float[]>Ȩ=new Dictionary<string,float[]>();float ΓЇ;bool ΑΫ=false;int ΑΟ=0;int ĉ=0;int Αι=0;int ĥ=0;
String Λ="";int[]ΕА=new int[Enum.GetValues(typeof(Ρ)).Length];bool Γό=false;int Ī=0;int І=0;Ρ φ=Ρ.Main;int ΘЄ=0,ΘЅ=0,ΘІ=0,ΘЇ=0;int Θρ=0,Θς=0;bool ō=false;bool ΕΜ=false;String Ū="";String ΓΘ="";IMyBroadcastListener Ǚ=null;bool Επ=true;List<И>Ц=null;И Џ=null;
int ΑΤ=0,ΑΥ=0;int Εί=0,Εΰ=0;int ΗЃ=0,ΗЄ=0,ΗЅ=0;String ΘΣ="";Exception Ȥ=null;bool ΑЁ=false;Π ŧ=Π.у;bool Εε=false;bool ΕЀ=true;bool ΓЋ=false;String Ζκ="";bool Ƭ=false;bool ΓА=true;int Ακ=0;int[]Ǐ=null;Vector3 ΓΚ=new Vector3();bool Εσ=false;bool њ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;
Program(){ό=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βή(Me,controllerTag,true))
Ξ=Μ.ы;ε(Ρ.Main);if(Ξ!=Μ.ы){Λ="Welcome to [PAM]!";ь ΓЊ=ΗΡ();if(Ξ==Μ.ȳ){List<IMyShipDrill>Εν=new List<IMyShipDrill>();List<IMyShipGrinder>Ετ=new List<IMyShipGrinder>();ό.GetBlocksOfType(Εν,q=>q.CubeGrid==Me.CubeGrid);ό.GetBlocksOfType(Ετ,q=>q.CubeGrid==Me.CubeGrid);if(Εν.Count>0)
{Ξ=Μ.Ž;Λ="Miner mode enabled!";}else if(Ετ.Count>0){Ξ=Μ.ǖ;Λ="Grinder mode enabled!";}else{Ξ=Μ.ή;Ȅ(м.ή);}}if(ΓЊ==ь.ΑΕ) ƞ=false;if(ΓЊ==ь.Δι) Λ="Data restore failed!";if(ΓЊ==ь.Βμ) Λ="New version, wipe data";}}void Main(string Н,UpdateType ΘЏ){try{if(Ȥ!=null){if(Н.Trim().ToUpper()=="RETRY")
{Ȥ=null;Ȟ=true;ď=0;if(Ξ!=Μ.ы) ω();Λ="Restarted after error";}else{ΗΑ();ΔЊ();return;}}Αξ=(ΘЏ&UpdateType.Update10)!=0;if(Αξ) ΔΗ++;ȣ=ΔΗ>=10;if(ȣ) ΔΗ=0;if(Αξ){ΑΟ++;if(ΑΟ>4) ΑΟ=0;Ƣ=(int) Math.Max(0,10-(DateTime.Now-Γξ).TotalSeconds);}if(Н!="") ΘΣ=Н;if(Ξ!=Μ.ы)
Ηά(Н);else
Ζϋ(Н);Γό=false;try{int Εγ=Runtime.CurrentInstructionCount;float Ĭ=Ќ(Εγ,Runtime.MaxInstructionCount);if(Ĭ>0.90) Λ="Max. instructions >90%";if(Ĭ>ŝ) ŝ=Ĭ;if(ΑΫ){ŕ.Add(Εγ);while(ŕ.Count>10) ŕ.RemoveAt(0);ŋ=0;for(int i=0;i<ŕ.Count;i++) ŋ+=ŕ[i];ŋ=Ќ(Ќ(ŋ,ŕ.Count),Runtime.MaxInstructionCount);
double Γν=Runtime.LastRunTimeMs;if(Αλ&&Γν>Γϊ) Γϊ=Γν;ť.Add((int)(Γν*1000f));while(ť.Count>10) ť.RemoveAt(0);Ɔ=0;for(int i=0;i<ť.Count;i++) Ɔ+=ť[i];Ɔ=Ќ(Ɔ,ť.Count)/1000f;}}catch{ŝ=0;}}catch(Exception e){Ȥ=e;}}void Ηά(string Н){bool ΑЃ=false;String ΕΨ="";if(Ј<=1&&!Βδ(Н))
ΔЋ(Н);int Γύ=50;while(Ǚ!=null&&Ǚ.HasPendingMessage&&Γύ-->0){MyIGCMessage ξ=Ǚ.AcceptMessage();String Χ=ξ.Data as String;if(Χ==null) continue;String Ƿ="";if(Ј<=1&&ΔЉ(ref Χ,out Ƿ,out ΕΨ)&&Ƿ==BROADCAST_SELF_ID){ΔЋ(Χ);ΑЃ=true;break;}}bool Εϋ=Αλ&&Ω==Β.т&&!Ř&&!ΑЃ&&ď==0&&!Αν;
if(ȣ&&Ω!=Β.т) ΑЃ=true;if((Αξ&&!Εϋ)||(ȣ&&Εϋ)){if(ď==0&&(Ƣ<=0||Ȟ)){Ζλ=Ј>0;Ј=0;ď=1;ȅ();ΗΥ();Δϋ();ȁ("Scan 1");}else if(ď==1){ď=2;ȅ();ΗΦ();ȁ("Scan 2");}else if(ď==2){ď=0;ȅ();ΗΧ();ΗΨ();if(Ј<=1) ΔЂ();ȁ("Scan 3");Γξ=DateTime.Now;if(Ј<=1&&ƞ)
ΑЀ=ΒΧ(Ψ,Ψ.CenterOfMass);ƞ=false;if(Ȟ){Ȟ=false;bool ΙΒ=α==Π.З;ω();if(ΕЀ&&resumeAfterRestart&&ΙΒ&&Ј<=1) ΓЋ=true;}ΕЀ=false;if(Ζλ&&Ј==0) Λ="Setup complete";}else{if(α==Π.З&&Ξ!=Μ.ή){ȅ();Ζν();ȁ("Inv balance");}ȅ();switch(ƭ){case 0:Ηδ();break;case 1:Ηθ();break;case 2:Ηγ();break;
case 3:Ηπ();break;case 4:Ηη();break;case 5:Ζς();break;case 6:Ζρ(Ψ);break;}ȁ("Update: "+ƭ);ƭ++;if(ƭ>6){ƭ=0;Αλ=true;if(ŧ!=Π.у){switch(ŧ){case Π.ĵ:ȯ();break;case Π.Ķ:ȯ();break;case Π.З:ȯ();break;case Π.Ǆ:ƽ();break;case Π.ǅ:Ė();break;}ŧ=Π.у;}}}if(!Ȟ){if(!ȃ(Ψ,true)){Ψ=null;
ƞ=true;Ј=2;}if(Ј>=2&&Ω!=Β.т)
ω();if(Ј<=1){į=Ψ.CalculateShipMass().PhysicalMass;Ȫ=(float) Ψ.GetShipSpeed();ζ=Ηβ(Ψ,ΑЀ);Αζ=Ψ.WorldMatrix.Forward;ƙ=Ψ.WorldMatrix.Left;ƍ=Ψ.WorldMatrix.Down;Ηλ();Ηξ();if(ΓЋ){ΓЋ=false;ȯ();if(Ω!=Β.т) Λ="Job resumed after restart";}if(Ω!=Β.т){Ř=false;ΔЎ(false);Δέ(false);String Θϋ=ΒΩ(Ω)+" "+(int) Ω;
ȅ();Ηι();ΒЈ(false);ȁ(Θϋ);Ηο();Ηκ();ȅ();Ηζ();ȁ("Thruster");ȅ();ΕΘ();ȁ("Gyroscope");}else{if(Ř){if(ǁ()){з(ƍ,Αζ,ƙ,0.25f,true);ΕΘ();Λ="Aligning to planet: "+Math.Round(ΑΨ-0.25f,2)+"°";if(Αμ) Βά(true,true);}else Βά(true,true);}}Ƭ=false;}}}ȅ();ΔЊ();ȁ("Print");if(ΑЃ||ΓΙ<=0){ȅ();Ζξ(ΕΨ);
ȁ("Broadcast");ΓΙ=4;}else if(ȣ) ΓΙ--;}void ΔЋ(string Н){if(Н=="") return;var ο=Н.ToUpper().Split(' ');ο.DefaultIfEmpty("");var Г=ο.ElementAtOrDefault(0);var ȇ=ο.ElementAtOrDefault(1);var ƅ=ο.ElementAtOrDefault(2);var ΕΟ=ο.ElementAtOrDefault(3);String Ȏ="Invalid argument: "+Н;bool ΔΟ=false;
switch(Г){case "UP":this.Βι(false);break;case "DOWN":this.Βθ(false);break;case "UPLOOP":this.Βι(true);break;case "DOWNLOOP":this.Βθ(true);break;case "APPLY":this.ΔΫ(true);break;case "MRES":І=0;break;case "STOP":this.ω();break;case "PATHHOME":{this.ω();this.ΒЄ();}break;case "PATH":{this.ω();this.ΒЄ();Ν.δ=true;}break;
case "START":{this.ω();ΒЃ();}break;case "ALIGN":{Βά(!Ř,false);}break;case "CONT":{this.ω();this.ȯ();}break;case "JOBPOS":{this.ω();this.Ė();}break;case "HOMEPOS":{this.ω();this.ƽ();}break;case "FULL":{Ƥ=true;}break;case "RESET":{ΑЁ=true;Ј=2;}break;case "EXPORT":{ΖЃ();}break;
case "IMPORT":{ΗΒ(ȇ=="JOB");}break;default:ΔΟ=true;break;}if(Ξ!=Μ.ή){switch(Г){case "SHUTTLE":{ΖЀ();}break;case "CFGS":{if(!Ζω(ȇ,ƅ,ΕΟ)) Λ=Ȏ;}break;case "CFGB":{if(!Ζψ(ȇ,ƅ)) Λ=Ȏ;}break;case "CFGL":{if(!ė(ref Ǵ,true,υ.ǂ,ȇ,"")||!Ζϊ(ƅ))
Λ=Ȏ;}break;case "CFGE":{if(!ė(ref е,true,υ.ǃ,ȇ,"IG")||
!ė(ref ђ,true,υ.Βκ,ƅ,"IG")||
!ė(ref ѓ,true,υ.Βλ,ΕΟ,"IG"))
Λ=Ȏ;}break;case "CFGA":{if(!ė(ref Ǭ,false,υ.Ʋ,ȇ,"")) Λ=Ȏ;}break;case "CFGW":{if(!ė(ref ų,false,υ.Ƃ,ȇ,"")||
!ė(ref Ų,false,υ.Ƃ,ƅ,""))
Λ=Ȏ;}break;case "NEXT":{ΑА(false);}break;case "PREV":{ΑА(true);}break;default:if(ΔΟ) Λ=Ȏ;break;}}else{switch(Г){case "UNDOCK":{Ƭ=true;}break;default:if(ΔΟ) Λ=Ȏ;break;}}}String ΖЊ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ξ!=Μ.ή)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΖЀ(){ω();Ξ=Μ.ή;Ȅ(м.ή);Ζ.δ=false;Ν.δ=false;σ=null;τ.Clear();α=Π.у;}void Βά(bool Д,bool ǰ){if(!Д) Λ="Aligning canceled";if(ǰ) Λ="Aligning done";if(ǰ||!Д){Ř=false;ΕΔ();ΑΓ(false,0,0,0,0);return;}if(ǁ()) Ř=true;}bool Ζω(String ΙΔ,String ΘΙ,String ΗЉ)
{bool ΘБ=α==Π.З;int w,h,d;if(int.TryParse(ΙΔ,out w)&&int.TryParse(ΘΙ,out h)&&int.TryParse(ΗЉ,out d)){this.ω();х=w;ф=h;в=d;Ĝ(false);Ɓ(false,false);if(ΘБ) ȯ();return true;}return false;}bool Ζϊ(String Ƅ){if(Ƅ=="ON"){ȭ=true;return true;}if(Ƅ=="OFF"){ȭ=false;return true;}
return false;}bool Ζψ(String Εμ,String ΑΩ){bool ī=true;if(Εμ=="HOME") Đ=true;else if(Εμ=="STOP") Đ=false;else ī=false;if(ΑΩ=="HOME") ч=с.ǌ;else if(ΑΩ=="STOP") ч=с.ΕΓ;else if(ΑΩ=="JOB") ч=с.ΒΤ;else if(ΑΩ=="IG") ч=с.Ÿ;else ī=false;return ī;}void ε(Ρ н){ΕА[(int) φ]=І;
І=ΕА[(int) н];if(н==Ρ.ƀ) І=0;φ=н;if(Ξ!=Μ.ы)
ě(φ==Ρ.ż,false,0,0);Γό=true;}void Βι(bool Γς){if(І>0) І--;else if(Γς) І=Ī-1;}void Βθ(bool Γς){if(І<Ī-1) І++;else if(Γς) І=0;}String Ȅ(м ξ){switch(ξ){case м.ΑΑ:Λ="Job is running";break;case м.ŵ:Λ="Connector not ready!";break;case м.Ηέ:Λ="Ship modified, path outdated!";break;
case м.Δύ:Λ="Interrupted by player!";break;case м.ή:Λ="Shuttle mode enabled!";break;}return "";}String ΖБ(ǘ ι){switch(ι){case ǘ.ΑΖ:return "Top-Left";case ǘ.Δί:return "Center";default:return "";}}String ΖЇ(љ Ї){switch(Ї){case љ.ΒΕ:return "Auto"+(Ξ==Μ.Ž?" (Ore)":"");case љ.ΒΖ:return "Auto (+Stone)";
case љ.ΒΞ:return "Default";default:return "";}}String ΒΦ(Π ώ){switch(ώ){case Π.у:return "No job";case Π.Ł:return "Job paused";case Π.З:return "Job active";case Π.ĵ:return "Job active";case Π.Ķ:return "Job active";case Π.ΒΠ:return "Job done";case Π.ƶ:return "Job changed";case Π.Ǆ:return "Move home";
case Π.ǅ:return "Move to job";}return "";}String Δξ(с Ηϋ){switch(Ηϋ){case с.ǌ:return "Return home";case с.ΒΤ:return "Fly to job pos";case с.ΕΓ:return "Stop";case с.Ÿ:return "Ignore";}return "";}String ΖЈ(Љ Ї){switch(Ї){case Љ.Βξ:return "Off";case Љ.ΒΡ:return "Drop pos (Stone) ";case Љ.Ƹ:return "Drop pos (Sto.+Ice)";
case Љ.Δβ:return "Cur. pos (Stone)";case Љ.ΒΜ:return "Cur. pos (Sto.+Ice)";case Љ.Βί:return "In motion (Stone)";case Љ.ǀ:return "In motion (Sto.+Ice)";}return "";}String Δμ(ъ ώ){switch(ώ){case ъ.None:return "No batteries";case ъ.ĕ:return "Charging";case ъ.Δε:return "Discharging";}return "";}String ΒΩ(Β ώ)
{String ΙΝ=Ξ==Μ.ή?"target":"job";switch(ώ){case Β.т:return "Idle";case Β.ȕ:return "Flying to XY position";case Β.ŀ:return Ξ==Μ.ǖ?"Grinding":"Mining";case Β.Ș:return "Returning";case Β.Ŷ:return "Flying to drop pos";case Β.Ę:return "Returning to dock";case Β.а:return "Flying to dock area";
case Β.й:return "Flying to job area";case Β.ȱ:return "Flying to path";case Β.ļ:return "Flying to job position";case Β.Ǿ:return "Approaching dock";case Β.Ļ:return "Docking";case Β.ĸ:return "Aligning";case Β.Ē:return "Aligning";case Β.ǋ:return "Retry docking";case Β.Ȇ:return "Unloading";case Β.Ā:return Ζκ;
case Β.ț:return "Undocking";case Β.ĕ:return "Charging batteries";case Β.ΑΚ:return "Waiting for uranium";case Β.ƺ:return "Filling up hydrogen";case Β.ΒЉ:return "Waiting for ejection";case Β.ΑΙ:return "Waiting for ejection";case Β.Ƽ:return "Flying to drop pos";}return "";}String Δχ(θ К){switch(К)
{case θ.Βπ:return "On \"Undock\" command";case θ.Βτ:return "On player entered cockpit";case θ.ΔЇ:return "On ship is full";case θ.ΔІ:return "On ship is empty";case θ.Βυ:return "On time delay";case θ.ǈ:return "On batteries empty(<25%)";case θ.Ǉ:return "On batteries empty(=0%)";case θ.Βο:return "On batteries full";
case θ.Βσ:return "On hydrogen empty(<25%)";case θ.Βρ:return "On hydrogen empty(=0%)";case θ.Βς:return "On hydrogen full";}return "";}bool Ι(ref String Γ,int ΖΟ,int Ĉ,bool Θ,String ħ){Ī+=1;if(ΖΟ==Ĉ) ħ=">"+ħ+(ΑΟ>=2?" .":"");else ħ=" "+ħ;Γ+=ħ+"\n";return ΖΟ==Ĉ&&Θ;}String ΔΫ(bool Θ)
{int Α=0;int Η=І;Ī=0;String ũ="———————————————\n";String ŭ="--------------------------------------------\n";String Γ="";Γ+=ΒΦ(α)+" | "+(Ν.δ?"Ready to dock":"No dock")+"\n";Γ+=ũ;double ǹ=Math.Max(Math.Round(this.Ч),0);if(φ==Ρ.Main){bool Ċ=Ξ==Μ.ή;if(Ι(ref Γ,Η,Α++,Θ," Record path & set home")) ΒЄ();
if(Ξ==Μ.Ž)
if(Ι(ref Γ,Η,Α++,Θ," Setup mining job")) ε(Ρ.ż);if(Ξ==Μ.ǖ)
if(Ι(ref Γ,Η,Α++,Θ," Setup grinding job")) ε(Ρ.ż);if(Ξ==Μ.ή)
if(Ι(ref Γ,Η,Α++,Θ," Setup shuttle job")) ε(Ρ.ΒЀ);if(Ι(ref Γ,Η,Α++,Θ," Continue job")) ȯ();if(Ι(ref Γ,Η,Α++,Θ," Fly to home position")) ƽ();if(Ι(ref Γ,Η,Α++,Θ," Fly to job position")) Ė();if(Ι(ref Γ,Η,Α++,Θ," Behavior settings"))
if(Ċ) ε(Ρ.ΑΔ);else ε(Ρ.ƴ);if(Ι(ref Γ,Η,Α++,Θ," Info")) ε(Ρ.Δω);if(Ξ!=Μ.ή)
if(Ι(ref Γ,Η,Α++,Θ," Help")) ε(Ρ.Δψ);}else if(φ==Ρ.ż){double ΙΓ=Math.Round(х*Ȓ,1);double ΘΘ=Math.Round(ф*ǽ,1);String Ћ="";if(Ι(ref Ћ,Η,Α++,Θ," Start new job!")) ΒЃ();if(Ι(ref Ћ,Η,Α++,Θ," Change current job")){Ɓ(false,false);ε(Ρ.Main);}if(Ι(ref Ћ,Η,Α++,Θ," Width + (Width: "+х+" = "+ΙΓ+"m)")){ķ(ref х,5,20,1);Ĝ(true);}
if(Ι(ref Ћ,Η,Α++,Θ," Width -")){ķ(ref х,-5,20,-1);Ĝ(true);}if(Ι(ref Ћ,Η,Α++,Θ," Height + (Height: "+ф+" = "+ΘΘ+"m)")){ķ(ref ф,5,20,1);Ĝ(true);}if(Ι(ref Ћ,Η,Α++,Θ," Height -")){ķ(ref ф,-5,20,-1);Ĝ(true);}if(Ι(ref Ћ,Η,Α++,Θ," Depth + ("+(я==љ.ΒΞ?"Depth":"Min")+": "+в+"m)")){ķ(ref в,5,50,2);Ĝ(true);}
if(Ι(ref Ћ,Η,Α++,Θ," Depth -")){ķ(ref в,-5,50,-2);Ĝ(true);}if(Ι(ref Ћ,Η,Α++,Θ," Depth mode: "+ΖЇ(я))){я=Ȱ(я);}if(Ι(ref Ћ,Η,Α++,Θ," Start pos: "+ΖБ(Ǹ))){Ǹ=Ȱ(Ǹ);}if(Ξ==Μ.ǖ&&я==љ.ΒΖ) я=Ȱ(я);Γ+=Βϋ(8,Ћ,Η,ref Θρ);}else if(φ==Ρ.ΒЀ){float[]Εη=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ι(ref Γ,Η,Α++,Θ," Next")) ε(Ρ.ΕΒ);if(Ι(ref Γ,Η,Α++,Θ," Back")) ε(Ρ.Main);Γ+=" Leave connector 1:\n";if(Ι(ref Γ,Η,Α++,Θ," - "+Δχ(Ё.К))) Ё.К=Ȱ(Ё.К);if(!Ё.Βέ()) Γ+="\n";else if(Ι(ref Γ,Η,Α++,Θ," - Delay: "+ƾ((int) Ё.Ǜ))) Ё.Ǜ=Δγ(Ё.Ǜ,Εη);Γ+=" Leave connector 2:\n";if(Ι(ref Γ,Η,Α++,Θ," - "+Δχ(Ђ.К))) Ђ.К=Ȱ(Ђ.К);
if(!Ђ.Βέ()) Γ+="\n";else if(Ι(ref Γ,Η,Α++,Θ," - Delay: "+ƾ((int) Ђ.Ǜ))) Ђ.Ǜ=Δγ(Ђ.Ǜ,Εη);}else if(φ==Ρ.ΕΒ){if(Ι(ref Γ,Η,Α++,Θ," Start job!")) ΒЃ();if(Ι(ref Γ,Η,Α++,Θ," Back")) ε(Ρ.ΒЀ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(Ё.ǜ!=""?Ё.ǜ:"-"))) Ё.ǜ=ǆ(ref ΘЄ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(Ё.ǟ!=""?Ё.ǟ:"-"))) Ё.ǟ=ǆ(ref ΘІ);Γ+=" Timer: \"Docking connector 2\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(Ђ.ǜ!=""?Ђ.ǜ:"-"))) Ђ.ǜ=ǆ(ref ΘЅ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ι(ref Γ,Η,Α++,Θ," = "+(Ђ.ǟ!=""?Ђ.ǟ:"-"))) Ђ.ǟ=ǆ(ref ΘЇ);
}else if(φ==Ρ.Ż){String ΕЇ=Ǵ+" %";if(ȣ) ĉ++;if(ĉ>1){ĉ=0;Αι++;if(Αι>1) Αι=0;}bool[]ΓЍ=new bool[]{Ď.Count==0,ǭ==ъ.None,ѝ.Count==0};int Ζε=0;while(true){Ζε++;ĥ++;if(ĥ>ΓЍ.Length-1) ĥ=0;if(Ζε>=ΓЍ.Length) break;if(!ΓЍ[ĥ]) break;}bool Ċ=Ξ==Μ.ή;if(!Ċ&&ȭ&&Щ!=-1&&Αι==0)
ΕЇ=Щ<1000000?Math.Round(Щ)+" Kg":Math.Round(Щ/1000)+" t";if(Ι(ref Γ,Η,Α++,Θ," Stop!")){ω();ε(Ρ.Main);}if(Ι(ref Γ,Η,Α++,Θ," Behavior settings"))
if(!Ċ) ε(Ρ.ƴ);else ε(Ρ.ΑΔ);if(!Ċ){if(Ι(ref Γ,Η,Α++,Θ," Next hole")) ΑА(false);}else if(Ι(ref Γ,Η,Α++,Θ," Undock")) Ƭ=true;Γ+=ŭ;if(!Ċ) Γ+="Progress: "+Math.Round(Ɨ,1)+" %\n";Γ+="State: "+ΒΩ(Ω)+" "+ǹ+"m \n";Γ+="Load: "+Απ+" % Max: "+ΕЇ+" \n";if(ĥ==0) Γ+="Uranium: "+(Ď.Count==0?"No reactors":Math.Round(Ĳ,1)+"Kg "+(е==-1?"":" Min: "+е+" Kg"))+"\n";
if(ĥ==1) Γ+="Battery: "+(ǭ==ъ.None?Δμ(ǭ):Ȉ+"% "+(ђ==-1||Ċ?"":" Min: "+ђ+" %"))+"\n";if(ĥ==2) Γ+="Hydrogen: "+(ѝ.Count==0?"No tanks":Math.Round(Ȣ,1)+"% "+(ѓ==-1||Ċ?"":" Min: "+ѓ+" %"))+"\n";}else if(φ==Ρ.ƴ){String Ћ="";if(Ι(ref Ћ,Η,Α++,Θ," Back")){if(α==Π.З) ε(Ρ.Ż);else ε(Ρ.Main);}
if(Ι(ref Ћ,Η,Α++,Θ," Max load: "+Ǵ+"%")) р(ref Ǵ,Ǵ<=80?-10:-5,υ.ǂ,false);if(Ι(ref Ћ,Η,Α++,Θ," Weight limit: "+(ȭ?"On":"Off"))) ȭ=!ȭ;if(Ι(ref Ћ,Η,Α++,Θ," Ejection: "+ΖЈ(Ш))) Ш=Ȱ(Ш);if(Ι(ref Ћ,Η,Α++,Θ," Toggle sorters: "+(İ?"On":"Off"))){İ=!İ;if(İ) ǫ(ΓА);}
if(Ι(ref Ћ,Η,Α++,Θ," Unload ice: "+(Ȭ?"On":"Off"))) Ȭ=!Ȭ;if(Ι(ref Ћ,Η,Α++,Θ," Uranium: "+(е==-1?"Ignore":"Min "+е+"Kg"))) р(ref е,(е>5?-5:-1),υ.ǃ,true);if(Ι(ref Ћ,Η,Α++,Θ," Battery: "+(ђ==-1?"Ignore":"Min "+ђ+"%"))) р(ref ђ,-5,υ.Βκ,true);if(Ι(ref Ћ,Η,Α++,Θ," Hydrogen: "+(ѓ==-1?"Ignore":"Min "+ѓ+"%"))) р(ref ѓ,-10,υ.Βλ,true);
if(Ι(ref Ћ,Η,Α++,Θ," When done: "+(Đ?"Return home":"Stop"))) Đ=!Đ;if(Ι(ref Ћ,Η,Α++,Θ," On damage: "+Δξ(ч))) ч=Ȱ(ч);if(Ι(ref Ћ,Η,Α++,Θ," Advanced...")) ε(Ρ.ΑБ);Γ+=Βϋ(8,Ћ,Η,ref Θς);}else if(φ==Ρ.ΑБ){if(Ι(ref Γ,Η,Α++,Θ," Back")){if(α==Π.З) ε(Ρ.Ż);else ε(Ρ.Main);}
if(Ι(ref Γ,Η,Α++,Θ,(Ξ==Μ.ǖ?" Grinder":" Drill")+" inv. balancing: "+(ŗ?"On":"Off"))) ŗ=!ŗ;if(Ι(ref Γ,Η,Α++,Θ," Enable"+(Ξ==Μ.ǖ?" grinders":" drills")+": "+(Ő?"Fwd + Bwd":"Fwd"))) Ő=!Ő;if(Ι(ref Γ,Η,Α++,Θ," Work speed fwd.: "+ų+"m/s")) р(ref ų,0.5f,υ.Ƃ,false);
if(Ι(ref Γ,Η,Α++,Θ," Work speed bwd.: "+Ų+"m/s")) р(ref Ų,0.5f,υ.Ƃ,false);if(Ι(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(Ǭ*100f)+"%"+(Ǭ>0.80f?" (risky)":""))) р(ref Ǭ,0.1f,υ.Ʋ,false);if(Ι(ref Γ,Η,Α++,Θ," Width overlap: "+Ĵ*100f+"%")) Δΰ(true,0.05f);if(Ι(ref Γ,Η,Α++,Θ," Height overlap: "+ć*100f+"%")) Δΰ(false,0.05f);
}else if(φ==Ρ.ΑΔ){if(Ι(ref Γ,Η,Α++,Θ," Back")){if(α==Π.З) ε(Ρ.Ż);else ε(Ρ.Main);}if(Ι(ref Γ,Η,Α++,Θ," Max load: "+Ǵ+"%")) р(ref Ǵ,Ǵ<=80?-10:-5,υ.ǂ,false);if(Ι(ref Γ,Η,Α++,Θ," Unload ice: "+(Ȭ?"On":"Off"))) Ȭ=!Ȭ;if(Ι(ref Γ,Η,Α++,Θ," Uranium: "+(е==-1?"Ignore":"Min "+е+"Kg"))) р(ref е,(е>5?-5:-1),υ.ǃ,true);
if(Ι(ref Γ,Η,Α++,Θ," Battery: "+(ђ==-1?"Ignore":"Charge up"))) ђ=(ђ==-1?1:-1);if(Ι(ref Γ,Η,Α++,Θ," Hydrogen: "+(ѓ==-1?"Ignore":"Fill up"))) ѓ=(ѓ==-1?1:-1);if(Ι(ref Γ,Η,Α++,Θ," On damage: "+Δξ(ч))) ч=Ȱ(ч);if(Ι(ref Γ,Η,Α++,Θ," Acceleration: "+Math.Round(Ǭ*100f)+"%"+(Ǭ>0.80f?" (risky)":""))) р(ref Ǭ,0.1f,υ.Ʋ,false);
}else if(φ==Ρ.ΔЌ){double ΘΤ=0;if(Φ.Count>0) ΘΤ=Vector3.Distance(Φ.Last().Ε,ζ);if(Ι(ref Γ,Η,Α++,Θ," Stop path recording")) ΕΕ();if(Ξ!=Μ.ή){if(Ι(ref Γ,Η,Α++,Θ," Home: "+(ȓ?"Use old home":(Ν.δ?"Was set! ":"none ")))) ȓ=!ȓ;}else{if(Ι(ref Γ,Η,Α++,Θ," Connector 1: "+(ȓ?"Use old connector":(Ν.δ?"Was set! ":"none ")))) ȓ=!ȓ;
if(Ι(ref Γ,Η,Α++,Θ," Connector 2: "+(ů?"Use old connector":(Ζ.δ?"Was set! ":"none ")))) ů=!ů;}if(Ι(ref Γ,Η,Α++,Θ," Path: "+(Ű?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) Ű=!Ű;Γ+=ŭ;Γ+="Wp spacing: "+Math.Round(Ζμ)+"m\n";}else if(φ==Ρ.ΒЅ){if(Ι(ref Γ,Η,Α++,Θ," Stop")){ω();ε(Ρ.Main);}
Γ+=ŭ;Γ+="State: "+ΒΩ(Ω)+" \n";Γ+="Speed: "+Math.Round(Ȫ,1)+"m/s\n";Γ+="Target dist: "+ǹ+"m\n";Γ+="Wp count: "+Φ.Count+"\n";Γ+="Wp left: "+ΔΦ+"\n";}else if(φ==Ρ.Δω){List<IMyTerminalBlock>ġ=Δο();if(ȣ) ĉ++;if(ĉ>=ġ.Count) ĉ=0;if(Ι(ref Γ,Η,Α++,Θ," Next")) ε(Ρ.Δϊ);
Γ+=ŭ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Απ,1)+"% "+Math.Round(ǯ,1)+" / "+Math.Round(ǵ,1)+"\n";Γ+="Uranium: "+(Ď.Count==0?"No reactors":Math.Round(Ĳ,1)+"Kg "+Γτ)+"\n";Γ+="Battery: "+(ǭ==ъ.None?"":Ȉ+"% ")+Δμ(ǭ)+"\n";Γ+="Hydrogen: "+(ѝ.Count==0?"No tanks":Math.Round(Ȣ,1)+"% ")+"\n";
Γ+="Damage: "+(ġ.Count==0?"None":""+(ĉ+1)+"/"+ġ.Count+" "+ġ[ĉ].CustomName)+"\n";}else if(φ==Ρ.Δϊ){if(Ι(ref Γ,Η,Α++,Θ," Back")) ε(Ρ.Main);Γ+=ŭ;Γ+="Next scan: "+Ƣ+"s\n";Γ+="Ship size: "+Math.Round(Ȓ,1)+"m "+Math.Round(ǽ,1)+"m "+Math.Round(λ,1)+"m \n";
Γ+="Broadcast: "+(ō?"Online - "+Ū:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŝ*100f,1)+"% \n";}else if(φ==Ρ.Δψ){if(Ι(ref Γ,Η,Α++,Θ," Back")) ε(Ρ.Main);Γ+=ŭ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(Ј==2) Γ="Fatal setup error\nNext scan: "+Ƣ+"s\n";if(ΑЁ) Γ="Recompile script now";int ΘΥ=Γ.Split('\n').Length;for(int i=ΘΥ;i<=10;i++) Γ+="\n";Γ+=ũ;Γ+="Last: "+Λ+"\n";
return Γ;}void Δΰ(bool ΘΠ,float ǐ){ω();Ɓ(true,false);if(ΘΠ) р(ref Ĵ,ǐ,υ.Βφ,false);else р(ref ć,ǐ,υ.Βφ,false);Δή();ě(true,true,0,0);}float Δγ(float η,float[]ΑЎ){float ά=ΑЎ[0];for(int i=ΑЎ.Length-1;i>=0;i--)
if(η<ΑЎ[i]) ά=ΑЎ[i];return ά;}String ǆ(ref int Ĉ){String ά="";if(Ĉ>=ΔΚ.Count) Ĉ=-1;if(Ĉ>=0) ά=ΔΚ[Ĉ].CustomName;Ĉ++;return ά;}void ΑΗ(string έ){if(α!=Π.З) return;if(έ=="") return;IMyTerminalBlock Υ=ό.GetBlockWithName(έ);if(Υ==null||!(Υ is IMyTimerBlock)){Λ="Timerblock "+έ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void ķ(ref int value,int ƈ,int ȫ,int ΓЎ){if(ƈ==0) return;if(value<ȫ&&ΓЎ>0||value<=ȫ&&ΓЎ<0){value+=ΓЎ;return;}int Ης=Math.Abs(ƈ);int ΒЊ=0;int Γώ=1;while(true){ΒЊ+=Γώ*Ης*10;if(ƈ<0&&value-ȫ<=ΒЊ) break;if(ƈ>0&&value-ȫ<ΒЊ) break;
Γώ++;}value+=Γώ*ƈ;}void Ĝ(bool ΙΠ){х=Math.Max(х,1);ф=Math.Max(ф,1);в=Math.Max(в,0);ě(φ==Ρ.ż,false,0,0);}bool ė(ref float value,bool Θξ,υ π,String Ħ,String ΘΜ){if(Ħ=="") return false;float š=-1;bool Εω=false;if(Ħ.ToUpper()==ΘΜ) Εω=true;else if(!float.TryParse(Ħ,out š)) return false;
else š=Math.Max(0,š);if(Θξ) š=(float) Math.Round(š);р(ref value,š,π,Εω,false);return true;}void р(ref float value,float ǐ,υ π,bool ƫ){р(ref value,value+ǐ,π,ƫ,true);}void р(ref float value,float Ş,υ π,bool ƫ,bool ΙΘ){float ǡ=0,Ǡ=0;if(π==υ.Ƃ){ǡ=0.5f;Ǡ=10f;}if(π==υ.Ʋ){ǡ=0.1f;Ǡ=1f;}
if(π==υ.Ÿ){ǡ=50f;Ǡ=100f;}if(π==υ.Βκ){ǡ=5f;Ǡ=30f;}if(π==υ.ǃ){ǡ=1f;Ǡ=25f;}if(π==υ.ǂ){ǡ=10f;Ǡ=95f;}if(π==υ.Βλ){ǡ=10f;Ǡ=90f;}if(π==υ.Ηα){ǡ=10f;Ǡ=1800;}if(π==υ.Βφ){ǡ=0.0f;Ǡ=0.75f;}if(Ş==-1&&ƫ){value=-1;return;}if(value==-1) ƫ=false;
bool ΖΙ=Ş<ǡ||Ş>Ǡ;if(ΖΙ&&ΙΘ){if(Ş<value) value=Ǡ;else if(Ş>value) value=ǡ;}else value=Ş;if(ΖΙ&&ƫ) value=-1;else value=Math.Max(ǡ,Math.Min(value,Ǡ));value=(float) Math.Round(value,2);}void ΑА(bool Θι){if(Θι) Ь=Math.Max(0,Ь-1);else Ь++;ΒЈ(true);}T Ȱ<T>(T η){int Α=Array.IndexOf(Enum.GetValues(η.GetType()),η);
Α++;if(Α>=ΖЁ(η)) Α=0;return(T) Enum.GetValues(η.GetType()).GetValue(Α);}int ΖЁ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЄ(){Ʊ.Clear();for(int i=0;i<Φ.Count;i++) Ʊ.Add(Φ[i]);Φ.Clear();Αν=true;Γδ=new Ϊ(Ν);Γη=new Ϊ(Ζ);Ν.δ=false;if(Ξ==Μ.ή) Ζ.δ=false;for(int i=0;i<Ǒ.Count;i++)
if(!Ȑ.Contains(Ǒ.Keys.ElementAt(i)))
Ȑ.Add(Ǒ.Keys.ElementAt(i));ȓ=false;ů=false;Ű=false;ε(Ρ.ΔЌ);}void ΕΕ(){if(ȓ) Ν=Γδ;if(ů) Ζ=Γη;if(Ű){Φ.Clear();for(int i=0;i<Ʊ.Count;i++) Φ.Add(Ʊ[i]);}Αν=false;ω();ε(Ρ.Main);}void Ηλ(){if(!Αν) return;if(Ω!=Β.т){ΕΕ();return;}if(!Γδ.δ) ȓ=false;if(!Γη.δ) ů=false;if(Ʊ.Count<=1) Ű=false;
IMyShipConnector Х=и(MyShipConnectorStatus.Connectable);if(Х==null) Х=и(MyShipConnectorStatus.Connected);if(Х!=null){if(Math.Round(Ȫ,2)<=0.20) ΓΧ++;else ΓΧ=0;if(ΓΧ>=5){if(Ξ==Μ.ή&&(Ν.δ||ȓ)&&Vector3.Distance(Ν.Ε,Х.GetPosition())>5){Ζ.γ=Ψ.WorldMatrix.Forward;Ζ.Ϗ=Ψ.WorldMatrix.Left;
Ζ.β=Ψ.WorldMatrix.Down;Ζ.ρ=Ψ.GetNaturalGravity();Ζ.Ε=Х.GetPosition();Ζ.δ=true;Ζ.г=Х.Position;}else{Ν.γ=Ψ.WorldMatrix.Forward;Ν.Ϗ=Ψ.WorldMatrix.Left;Ν.β=Ψ.WorldMatrix.Down;Ν.ρ=Ψ.GetNaturalGravity();Ν.Ε=Х.GetPosition();Ν.δ=true;Ν.г=Х.Position;}}}double Γκ=-1;if(Φ.Count>0) Γκ=Vector3.Distance(ζ,Φ.Last().Ε);
double Θω=Math.Max(1.5,Math.Pow(Ȫ/100.0,2));double ΕБ=Math.Max(Ȫ*Θω,2);Ζμ=ΕБ;if((Γκ==-1)||Γκ>=ΕБ){Ϊ Ϋ=new Ϊ(ζ,Αζ,ƍ,ƙ,Ψ.GetNaturalGravity());Ϋ.Ζο(ȑ,Ȑ);Φ.Add(Ϋ);}}Ϊ э(){if(Ξ!=Μ.ή) return Ƕ!=null?Ƕ:Ν;return Εβ;}void Ń(Ϊ ί,Π ώ){Εβ=ί;if(α==Π.З) ё=ώ;}bool Ȃ(Ϊ ί,float Є,bool ΘА,out Vector3 ι)
{if(ΘА){Vector3I Γί=new Vector3I((int) ί.г.X,(int) ί.г.Y,(int) ί.г.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(Γί);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){ι=new Vector3();return false;}Vector3 ΗЂ=к(Ψ,Υ.FatBlock.GetPosition()-ζ);Vector3 ΗЁ=к(Ψ,Υ.FatBlock.WorldMatrix.Forward);ι=ί.Ε-ΒЇ(ί.γ,ί.β*-1,ΗЂ)-ΒЇ(ί.γ,ί.β*-1,ΗЁ)*Є;
return true;}else{ι=ί.Ε;return true;}}Vector3 Ȗ(int ΗϏ,int Ъ,bool Θμ){if(!Θμ&&Εσ) return ΓΚ;float ΗЀ=((Ў-1f)/2f)-ΗϏ;float Θο=((О-1f)/2f)-Ъ;ΓΚ=Ζ.Ε+Ζ.Ϗ*ΗЀ*Ȓ+ȧ*-1*Θο*ǽ;Εσ=true;return ΓΚ;}Vector3 ΒΨ(Vector3 Θύ,float ΓΣ){return Θύ+(Ǣ*ΓΣ);}ΰ ΒΟ(){float Т=-1;ΰ ά=ΰ.Ȁ;
if(Ξ!=Μ.ή){ΔЂ();if(α!=Π.у){Vector3 Ж=Ě(Ǣ,ȧ*-1,ζ-Ζ.Ε);if(Math.Abs(Ж.X)<=(float)(Ў*Ȓ)/2f&&Math.Abs(Ж.Y)<=(float)(О*ǽ)/2f){if(Ж.Z<=-1&&Ж.Z>=-ĭ*2) return ΰ.Ƴ;if(Ж.Z>-1&&Ж.Z<λ*2) return ΰ.Ȁ;}if(Ȯ(Ζ.Ε,ref Т)) ά=ΰ.Ȁ;}if(Ν.δ){if(Ȯ(Ν.Ε,ref Т)) ά=ΰ.ǿ;for(int i=0;i<Φ.Count;i++)
if(Ȯ(Φ[i].Ε,ref Т)) ά=ΰ.ȗ;if(Vector3.Distance(ζ,Ν.Ε)<dockDist*λ) ά=ΰ.ǿ;if(и(MyShipConnectorStatus.Connectable)!=null||и(MyShipConnectorStatus.Connected)!=null)
ά=ΰ.ǿ;}}else{Vector3 ι=new Vector3();IMyShipConnector Ȍ=и(MyShipConnectorStatus.Connected);if(Ν.δ){if(Ȯ(Ν.Ε,ref Т)) ά=ΰ.ǿ;if(Ȃ(Ν,dockDist,true,out ι))
if(Ȯ(ι,ref Т)) ά=ΰ.ǿ;if(Ȍ!=null&&Vector3.Distance(Ȍ.GetPosition(),Ν.Ε)<5)
return ΰ.ΒΓ;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ν.Ε)>dockDist*λ&&Vector3.Distance(Φ[i].Ε,Ζ.Ε)>dockDist*λ)
if(Ȯ(Φ[i].Ε,ref Т)) ά=ΰ.ȗ;if(Ζ.δ){if(Ȯ(Ζ.Ε,ref Т)) ά=ΰ.Ȁ;if(Ȃ(Ζ,dockDist,true,out ι))
if(Ȯ(ι,ref Т)) ά=ΰ.Ȁ;if(Ȍ!=null&&Vector3.Distance(Ȍ.GetPosition(),Ζ.Ε)<5)
return ΰ.ΒΔ;}}return ά;}bool Ȯ(Vector3 ι,ref float Т){float Є=Vector3.Distance(ι,ζ);if(Є<Т||Т==-1){Т=Є;return true;}return false;}void ΒЃ(){if(Ј>0){Λ="Setup error! Can't start";return;}if(Ξ==Μ.ή){ȯ();return;}Ζ.Ε=ζ;Ζ.ρ=Ψ.GetNaturalGravity();Ζ.γ=Αζ;Ζ.β=ƍ;Ζ.Ϗ=ƙ;
Ǣ=Ǎ.WorldMatrix.Forward;ȧ=Ζ.β;if(Ǣ==Ψ.WorldMatrix.Down) ȧ=Ψ.WorldMatrix.Backward;Ɓ(true,true);Τ(Β.ȕ);ΒЂ();}void Ɓ(bool ǝ,bool Θν){if(α==Π.у&&!ǝ) return;bool Θέ=ǝ||α==Π.ΒΠ||Ў!=х||О!=ф||Ƙ!=Ǹ;if(Θέ){if(α!=Π.у){α=Π.ƶ;Ȗ(ύ,ψ,Θν);Λ="Job changed, lost progress";}Ƙ=Ǹ;
Ў=х;О=ф;ψ=0;ύ=0;ѐ="";ǣ=-1;ĩ=0;ȥ=0;Э=0;Ь=0;ΒЈ(true);}}void Βψ(){л(ζ,0);ћ(ȑ,true);}void ΒЂ(){Ȅ(м.ΑΑ);Βψ();ſ(ȏ,false);ǫ(ΓА);α=Π.З;Δέ(true);ё=α;ε(Ρ.Ż);ΗΤ();ΑΪ=true;Ακ=0;for(int i=ƃ.Count-1;i>=0;i--)
if(Ź(ƃ[i],false)) Ακ++;if(Ακ>0) Λ="Started with damage";}void ω(){if(α==Π.З){α=Π.Ł;Λ="Job paused";}Τ(Β.т);ё=α;ΑΓ(false,0,0,0,0);ΒІ();ΔΩ(new Vector3(),false);ΕΔ();Βώ(ChargeMode.Auto);ΒϏ(false);ΔЎ(true);ș(Β.т);ě(false,false,0,0);ћ(τ,false);ћ(ю,true);ǫ(true);ΑЂ=false;Ʒ();
ΑΧ=false;Αβ=false;ΑΪ=false;Ƥ=false;Ƭ=false;if(φ!=Ρ.Main&&φ!=Ρ.ƴ&&φ!=Ρ.ΑБ&&φ!=Ρ.ΑΔ)
ε(Ρ.Main);}void ȯ(){ΰ У=ΒΟ();if(Ξ==Μ.ή){if(!Ζ.δ||!Ν.δ) return;ΒЂ();bool Γέ=Vector3.Distance(ζ,Ν.Ε)<Vector3.Distance(ζ,Ζ.Ε);if(ŧ==Π.ĵ) Γέ=true;if(ŧ==Π.Ķ) Γέ=false;if(Γέ){Ń(Ν,Π.ĵ);switch(У){case ΰ.ΒΓ:Τ(Β.Ā);break;case ΰ.ȗ:Τ(Β.й);break;case ΰ.ǿ:Τ(Β.Ǿ);break;default:Τ(Β.ț);break;
}}else{Ń(Ζ,Π.Ķ);switch(У){case ΰ.ΒΔ:Τ(Β.Ā);break;case ΰ.Ȁ:Τ(Β.Ǿ);break;case ΰ.ȗ:Τ(Β.й);break;default:Τ(Β.ț);break;}}}else{if(α!=Π.Ł&&α!=Π.ƶ) return;bool ΙΑ=α==Π.ƶ;ΒЂ();bool Γβ=ź(false)&&Ν.δ;switch(У){case ΰ.Ȁ:Τ(Γβ?Β.ȱ:Β.ȕ);break;case ΰ.ȗ:Τ(Γβ?Β.а:Β.й);break;case ΰ.ǿ:Τ(Γβ?Β.Ǿ:Β.Ȇ);break;
case ΰ.Ƴ:{if(ȥ!=Ь||ΙΑ) Τ(Β.Ș);else Τ(Β.ŀ);}break;default:break;}}}void Ė(){if(α==Π.у&&!Ν.δ) return;if(Ξ==Μ.ή&&(!Ζ.δ||!Ν.δ)) return;Λ="Move to job";ΰ У=ΒΟ();if(Ξ==Μ.ή){Ń(Ζ,Π.Ķ);switch(У){case ΰ.Ȁ:Τ(Β.Ǿ);break;case ΰ.ȗ:Τ(Β.й);break;case ΰ.ΒΔ:return;default:Τ(Β.ț);break;
}ș(Β.Ā);}else{switch(У){case ΰ.Ȁ:Τ(Β.й);break;case ΰ.ȗ:Τ(Β.й);break;case ΰ.ǿ:Τ(Β.Ȇ);break;case ΰ.Ƴ:Τ(Β.Ș);break;default:break;}if(α==Π.у) ș(Β.й);else ș(Β.Ē);ΑЂ=true;}Βψ();ε(Ρ.ΒЅ);ſ(ȏ,false);ё=Π.ǅ;}void ƽ(){if(!Ν.δ) return;Λ="Move home";ΰ У=ΒΟ();if(Ξ==Μ.ή){Ń(Ν,Π.ĵ);switch(У)
{case ΰ.ȗ:Τ(Β.а);break;case ΰ.ǿ:Τ(Β.Ǿ);break;case ΰ.ΒΓ:return;default:Τ(Β.ț);break;}ș(Β.Ā);}else{if(и(MyShipConnectorStatus.Connected)!=null) return;if(и(MyShipConnectorStatus.Connectable)!=null){Τ(Β.Ļ);ș(Β.Ȇ);return;}switch(У){case ΰ.Ȁ:Τ(Β.ȱ);break;case ΰ.ȗ:Τ(Β.а);break;case ΰ.ǿ:Τ(Β.а);break;
case ΰ.Ƴ:Τ(Β.Ę);break;default:break;}ș(Β.Ȇ);}Βψ();ε(Ρ.ΒЅ);ſ(ȏ,false);ё=Π.Ǆ;}void Τ(Β ΓϏ){if(ΓϏ==Β.т) Ʀ=Β.т;if(Ʀ!=Β.т&&Ω==Ʀ&&ΓϏ!=Ʀ){ω();return;}А=true;Ω=ΓϏ;}void ș(Β ώ){this.Ʀ=ώ;}Ț ΖЍ(int ƕ,bool ǥ){if(ǥ){Ǐ=null;ύ=0;ψ=0;}if(Ǹ==ǘ.ΑΖ){int ΖΔ=ƕ+1;ψ=(int) Math.Floor(Ќ(ƕ,Ў));
if(ψ%2==0) ύ=ƕ-(ψ*Ў);else ύ=Ў-1-(ƕ-(ψ*Ў));if(ψ>=О) return Ț.ΒΒ;else return Ț.ΒΥ;}else if(Ǹ==ǘ.Δί){if(Ǐ==null) Ǐ=new int[]{0,-1,0,0};int Εφ=(int) Math.Ceiling(Ў/2f);int Ευ=(int) Math.Ceiling(О/2f);int ΘΖ=(int) Math.Floor(Ў/2f);int ΘΕ=(int) Math.Floor(О/2f);
int Ɩ=0;while(Ǐ[2]<Math.Pow(Math.Max(Ў,О),2)){if(Ɩ>200) return Ț.ΖЂ;Ɩ++;Ǐ[2]++;if(-Εφ<ύ&&ύ<=ΘΖ&&-Ευ<ψ&&ψ<=ΘΕ){if(Ǐ[3]==ƕ){this.ύ=ύ-1+Εφ;this.ψ=ψ-1+Ευ;return Ț.ΒΥ;}Ǐ[3]++;}if(ύ==ψ||(ύ<0&&ύ==-ψ)||(ύ>0&&ύ==1-ψ)){int ΘЀ=Ǐ[0];Ǐ[0]=-Ǐ[1];Ǐ[1]=ΘЀ;
}ύ+=Ǐ[0];ψ+=Ǐ[1];}}return Ț.ΒΒ;}void Ηι(){if(Ω==Β.ȕ){if(А){Б=0;if(ȥ!=Ь) ĩ=0;ȥ=Ь;}if(Б==0){int ΓΜ=Ζτ(Ь);if(ΓΜ<0){if(!ΑΧ){ΑΧ=true;Εα=ζ;}л(Εα,0);Λ="Waiting for a free row";return;}ΑΧ=false;bool ΕϏ=ΓΜ!=Ь;if(ΕϏ){Ь=ΓΜ;ĩ=0;ȥ=Ь;}Ț ά=ΖЍ(Ь,А||ΕϏ);if(ά==Ț.ΒΒ)
{α=Π.ΒΠ;Λ="Job done";if(Đ&&Ν.δ){Τ(Β.ȱ);ș(Β.Ȇ);ё=Π.Ǆ;}else{Τ(Β.ļ);ș(Β.Ē);ё=Π.ǅ;}return;}if(ά==Ț.ΒΥ){Б=1;ћ(τ,true);є=Ȗ(ύ,ψ,true);л(є,10);з(Ζ.β,Ζ.γ,Ζ.Ϗ,false);}}else{if(Ч<wpReachedDist){Τ(Β.ŀ);return;}}}if(Ω==Β.ŀ){if(А){ћ(τ,true);ǫ(false);є=Ȗ(ύ,ψ,false);
л(ΒΨ(є,0),0);з(Ζ.β,Ζ.γ,Ζ.Ϗ,false);Б=1;ц=0;ж=0;ΑΠ=0;Γμ=-1;ĭ=в;Γε=true;}if(!ΒΚ()){Τ(Β.Ę);return;}if(ź(true)){ΖΘ=Ȕ("","ORE",С.ē);if((Ш==Љ.ΒΡ||Ш==Љ.Ƹ||
Ш==Љ.Βί||Ш==Љ.ǀ)&&Ξ!=Μ.ǖ)
Τ(Β.Ŷ);else if((Ш==Љ.Δβ||Ш==Љ.ΒΜ)&&Ξ!=Μ.ǖ)
Τ(Β.ΒЉ);else
Τ(Β.Ę);return;}Э=Vector3.Distance(ζ,є);if(Э>ĩ){ĩ=Э;Γε=false;}if(Ξ==Μ.ǖ&&Δπ()==MyDetectedEntityType.SmallGrid)
ж+=2;else ж-=2;ж=Math.Max(100,Math.Min(400,ж));if(Б>0&&Б<ж){if(Э>ц){if(ж>150) ц=Э;else ц=(float) Math.Ceiling(Э);Б=1;}else Б++;}else{if(Б>0){Λ="Ship stuck! Retrying";ц=Э;Б=0;ě(false,true,0,λ*sensorRange);}л(ΒΨ(є,Math.Max(0,ц-λ)),ΒΫ(false));if(Ч<=wpReachedDist/2){Б=1;ц=0;}
return;}ě(false,true,λ*sensorRange,0);Vector3 Ητ=є+Ǣ*Э;bool Εό=false;if(Vector3.Distance(Ητ,ζ)>0.3f){Vector3 ΗІ=є+Ǣ*(Э+0.1f);л(ΗІ,4);Εό=true;}else{float Ǧ=ΒΫ(true);Vector3 Εζ=ΒΨ(є,Math.Max(в+1,Э+1));Ǫ(true,false,false,Εζ,Εζ-є,Ǧ,Ǧ);}bool ΓΤ=false;if(я==љ.ΒΖ||я==љ.ΒΕ)
{if(!Εό){float ΓЀ=0;foreach(IMyTerminalBlock Ǽ in τ)
ΓЀ+=Ζό(Ǽ,"","",я==љ.ΒΕ?new string[]{"STONE"}:null);if(ΓЀ>Γμ||Э<в||Γε){ΒА=0;ΑΠ=Э;ĭ=(int)(Math.Max(ĭ,ΑΠ)+λ/2);}else{ΓΤ=Э-ΑΠ>2&&ΒА>=20;ΒА++;}Γμ=ΓЀ;}}else ΓΤ=Э>=ĭ;if(ȥ!=Ь){Τ(Β.Ș);Э=0;return;}if(ΓΤ){Ь++;Τ(Β.Ș);Э=0;return;}}if(Ω==Β.Ș||Ω==Β.Ę||Ω==Β.Ŷ)
{if(А){є=Ȗ(ύ,ψ,false);з(Ζ.β,Ζ.γ,Ζ.Ϗ,false);ћ(τ,Ő);ǫ(false);ц=Vector3.Distance(ζ,є);ě(false,true,0,λ*sensorRange);}л(є,ΒΫ(false));if(Vector3.Distance(ζ,є)>=ц+5){ћ(τ,false);ǫ(true);Λ="Can´t return!";}if(Ч<wpReachedDist){if(Ω==Β.Ș&&ΑЂ) Τ(Β.ļ);if(Ω==Β.Ș) Τ(Β.ȕ);
if(Ω==Β.Ŷ) Τ(Β.Ƽ);if(Ω==Β.Ę){if(Ν.δ) Τ(Β.ȱ);else{ω();Ė();Λ="Can´t return, no dock found";}}return;}}if(Ω==Β.а||Ω==Β.й){if(Ω==Β.й&&α==Π.З&&Ξ!=Μ.ή){if(!ΒΚ()||ź(true)){Τ(Β.а);return;}}bool ΓЈ=false;bool ΔΠ=false;bool ΕΠ=false;float Ŋ=0;bool ΖΗ=false;Ϊ Ϋ=null;
if(А){if(Ω==Β.а||Ξ==Μ.ή){if(Ξ!=Μ.ή){Ƕ=null;č=0;Αβ=!ΒΛ(1);}Ϊ ί=э();κ=new Βχ();κ.μ=ί;κ.Ƌ=followPathDock*λ;κ.ΔΡ=useDockDirectionDist*λ;κ.Ŋ=10;κ.Ŭ.Add(ί.Ε);Vector3 ŉ=new Vector3();if(Ȃ(ί,dockDist*λ,true,out ŉ))
κ.Ŭ.Add(ŉ);else κ.Ƌ*=1.5f;if(Ξ==Μ.ή){if(ί==Ν) κ.Αγ=Ζ.Ε;if(ί==Ζ) κ.Αγ=Ν.Ε;κ.ΓΪ=dockDist*λ*1.1f;}}else if(Ω==Β.й){κ=new Βχ();κ.μ=Ζ;κ.Ƌ=followPathJob*λ;κ.ΔΡ=useJobDirectionDist*λ;κ.Ŋ=10;κ.Αγ=Ν.Ε;κ.ΓΪ=dockDist*λ*1.1f;κ.Ŭ.Add(Ζ.Ε);if(α==Π.у){if(!Ν.δ||Φ.Count==0){ω();return;}
float ΗЌ=Vector3.Distance(Φ.First().Ε,Ν.Ε);float ΗЍ=Vector3.Distance(Φ.Last().Ε,Ν.Ε);if(ΗЌ<ΗЍ) κ.μ=Φ.Last();else κ.μ=Φ.First();}}Ơ=new Vector3();ΖΗ=!ΗΖ(ζ);ћ(τ,false);ǫ(true);џ=-1;double Ǯ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,κ.Αγ)<=κ.ΓΪ) continue;
double Є=Vector3.Distance(Φ[i].Ε,ζ);if(Ǯ==-1||Є<Ǯ){џ=i;Ǯ=Є;}}Č=ΖЎ(κ.μ.Ε,џ);Ġ=null;}if(Ω==Β.а&&Ξ!=Μ.ή&&!А){Ϊ ΓΔ=э();Αβ=!ΒΛ(1);if(э()!=ΓΔ){Τ(Β.а);return;}}Ζπ(Φ,Č,κ.Ŭ,κ.Ƌ,А,ref Б);for(int i=0;i<κ.Ŭ.Count;i++){float Є=Vector3.Distance(ζ,κ.Ŭ[i]);if(Є<=κ.Ƌ) ΓЈ=true;
if(Є<=κ.ΔΡ) ΔΠ=true;}if(ΔΠ) Ŋ=κ.Ŋ;float ΙΡ=Ġ!=null?Ġ.Р:Ȫ;float ΙΗ=(float) Math.Max(Ȫ*0.1f*λ,wpReachedDist);if((Ч<ΙΗ)||А){if(!А) џ+=Č;if(Č==0||џ>Φ.Count-1||џ<0)
ΓЈ=true;else{ΔΦ=Č>0?Φ.Count-1-џ:џ;Ϋ=Φ[џ];Ġ=Ϋ;if(џ>=1&&џ<Φ.Count-1)
Ơ=Ϋ.Ε-Φ[џ-Č].Ε;else Ġ=null;Ѐ=Ϋ.Ε+ΖЌ(џ,Č);ΕΠ=true;}}if(ΔΠ)
з(κ.μ.β,κ.μ.γ,κ.μ.Ϗ,false);else if(ΖΗ)
ΒΑ(κ.μ.β,10,true);else if(ΕΠ&&Ϋ!=null){if(Č>0) з(Ϋ.β,Ϋ.γ,Ϋ.Ϗ,90,false);else з(Ϋ.β,-Ϋ.γ,-Ϋ.Ϗ,90,false);}Ǫ(true,false,true,Ѐ,Ơ,Ġ==null?0:Ġ.Р,Ŋ);if(ΓЈ){ΔΦ=0;if(Ω==Β.а&&Αβ){л(Ѐ,0);Λ="Waiting for a free dock";return;}if(Ω==Β.а||Ξ==Μ.ή){Τ(Β.Ǿ);return;}
if(Ω==Β.й&&ΑЂ){Τ(Β.ļ);return;}if(Ω==Β.й){Τ(Β.ȕ);return;}}}if(Ω==Β.Ǿ||Ω==Β.ǋ){if(Ξ!=Μ.ή){Ϊ ΓΔ=э();if(!ΒΛ(2)||э()!=ΓΔ){Τ(Β.а);return;}}Ϊ ί=э();if(А){if(!Ȃ(ί,dockDist*λ,true,out Ѐ)){Ȅ(м.ŵ);ω();return;}л(Ѐ,0);ΒΑ(ί.β,90,true);}if(Ч<followPathDock*λ&&Ч!=-1)
{л(Ѐ,10);з(ί.β,ί.γ,ί.Ϗ,false);}if(и(MyShipConnectorStatus.Connectable)!=null||и(MyShipConnectorStatus.Connected)!=null){Τ(Β.Ļ);return;}if(Ч<wpReachedDist/2&&Ч!=-1){Τ(Β.ĸ);return;}}if(Ω==Β.ĸ||Ω==Β.Ē){if(А){if(Ω==Β.ĸ){Ϊ ί=э();if(!Ȃ(ί,dockDist*λ,true,out Ѐ)){Ȅ(м.ŵ);ω();return;}
Ǫ(true,true,false,Ѐ,0);з(ί.β,ί.γ,ί.Ϗ,10,false);}if(Ω==Β.Ē){з(Ζ.β,Ζ.γ,Ζ.Ϗ,0.5f,false);Ѐ=Ζ.Ε;Ǫ(true,true,false,Ѐ,0);}}if(Αμ){ΑΓ(false,0,0,0,0);if(Ω==Β.ĸ) Τ(Β.Ļ);if(Ω==Β.Ē) ω();return;}}if(Ω==Β.Ļ){if(и(MyShipConnectorStatus.Connected)!=null){if(Ξ==Μ.ή) Τ(Β.Ā);else Τ(Β.Ȇ);
return;}Ϊ ί=э();if(А){ж=0;ΗЎ=DateTime.Now;Б=0;з(ί.β,ί.γ,ί.Ϗ,false);}Vector3I Γί=new Vector3I((int) ί.г.X,(int) ί.г.Y,(int) ί.г.Z);IMySlimBlock Εή=Me.CubeGrid.GetCubeBlock(Γί);float Ελ=dockingSpeed;float ΘΨ=dockingSpeed*5;float ΕΞ=Math.Max(1.5f,Math.Min(5f,λ*0.15f));if(!Ȃ(ί,0,true,out Ѐ)||!Ȃ(ί,ΕΞ,true,out Ơ)||
Εή==null||!Εή.FatBlock.IsFunctional){Ȅ(м.ŵ);ω();return;}if(ж==1||(Vector3.Distance(ζ,Ѐ)<=ΕΞ*1.1f&&!А))
ж=1;else{Vector3 ΘЈ=к(Ψ,Ơ-ζ);Vector3 Γή=к(Ψ,Ψ.GetNaturalGravity());float Θπ=Ƶ(ΘЈ,Γή,null);Ελ=Math.Min(ΘΨ,Θπ);}Ǫ(true,false,false,Ѐ,Ѐ-ζ,dockingSpeed,Ελ);if(А) ц=(float) Ч;IMyShipConnector ΓΟ=и(MyShipConnectorStatus.Connectable);if(ΓΟ!=null){Ǫ(false,false,false,Ѐ,0);
if(Б>0) Б=0;Б--;if(Б<-5){ΓΟ.Connect();if(ΓΟ.Status==MyShipConnectorStatus.Connected){if(Ξ==Μ.ή) Τ(Β.Ā);else Τ(Β.Ȇ);ΒІ();ſ(ȏ,true);return;}}}else{float Εκ=(float) Math.Round(Ч,1);if(Εκ<ц){Б=-1;ц=Εκ;}else Б++;if(Б>20){Τ(Β.ǋ);return;}}}if(Ω==Β.Ȇ||Ω==Β.Ā||
Ω==Β.ΑΚ||Ω==Β.ƺ||Ω==Β.ĕ){bool ΒЎ=false,ΒЏ=false;if(Ξ==Μ.ή){if(э()==Ν) ΒЎ=true;else if(э()==Ζ) ΒЏ=true;}if(А){Ƥ=false;if(и(MyShipConnectorStatus.Connected)==null){Τ(Β.ț);return;}ΒІ();if(ΒЎ) ΑΗ(Ё.ǜ);if(ΒЏ) ΑΗ(Ђ.ǜ);ı=false;Ȝ=false;ȡ=false;Ʈ=false;}if(и(MyShipConnectorStatus.Connected)==null){ω();Ȅ(м.Δύ);return;}
if(α!=Π.З||ђ==-1||ǭ==ъ.None) Ȝ=true;else if(Ȉ>=100f) Ȝ=true;else if(Ȉ<=99f) Ȝ=false;if(α!=Π.З||ѓ==-1||ѝ.Count==0) ȡ=true;else if(Ȣ>=100f) ȡ=true;else if(Ȣ<=99) ȡ=false;if(α!=Π.З||е==-1||Ď.Count==0) Ʈ=true;else Ʈ=Ĳ>=е;ņ щ=null;if(ΒЎ) щ=Ё;
if(ΒЏ) щ=Ђ;if(щ!=null&&(щ.К==θ.ǈ||щ.К==θ.Ǉ)) Ȝ=true;if(щ!=null&&(щ.К==θ.Βο)) if(!ı) Ȝ=false;if(щ!=null&&(щ.К==θ.Βσ||щ.К==θ.Βρ)) ȡ=true;if(щ!=null&&(щ.К==θ.Βς)) if(!ı) ȡ=false;if(ȣ){ChargeMode ΕΪ=Ȝ?ChargeMode.Auto:ChargeMode.Recharge;if(щ!=null&&(щ.К==θ.Ǉ||щ.К==θ.ǈ))
ΕΪ=ChargeMode.Discharge;Βώ(ΕΪ);ΒϏ(!ȡ);}if(!ı){if(Ξ==Μ.ή)
ı=α!=Π.З||Ζσ(А,true)||Ƭ;else
ı=α!=Π.З||Δώ();}else{if(!Ȝ) Τ(Β.ĕ);if(!ȡ) Τ(Β.ƺ);if(!Ʈ) Τ(Β.ΑΚ);А=false;}if(ı&&Ȝ&&ȡ&&Ʈ){Βώ(ChargeMode.Auto);ΒϏ(false);if(α==Π.З){if(Ξ==Μ.ή){if(э()==Ν) ΑΗ(Ё.ǟ);else if(э()==Ζ) ΑΗ(Ђ.ǟ);if(э()==Ν) Ń(Ζ,Π.Ķ);else Ń(Ν,Π.ĵ);}}Τ(Β.ț);return;}}if(Ω==Β.ț){if(А){IMyShipConnector Ȍ=и(MyShipConnectorStatus.Connected);
if(Ȍ==null){Τ(Β.й);return;}IMyShipConnector Њ=Ȍ.OtherConnector;ΔЍ(Ȍ,false);ſ(ȏ,false);Ϊ ί=null;Ϊ Εψ=Ξ==Μ.ή?Ν:э();if(Vector3.Distance(Ȍ.GetPosition(),Εψ.Ε)<5f&&Ν.δ) ί=Εψ;if(Vector3.Distance(Ȍ.GetPosition(),Ζ.Ε)<5f&&Ζ.δ) ί=Ζ;if(ί!=null){if(!Ȃ(ί,dockDist*λ,true,out Ѐ))
{Ȅ(м.ŵ);ω();return;}л(Ѐ,5);з(ί.β,ί.γ,ί.Ϗ,false);}else л(ζ+Њ.WorldMatrix.Forward*dockDist*λ,5);if(α==Π.З) Ȅ(м.ΑΑ);}if(Ч<wpReachedDist){ћ(ю,true);Τ(Β.й);return;}}if(Ω==Β.ļ){if(А){ǫ(true);ћ(τ,false);Ѐ=Ζ.Ε;л(Ѐ,20);з(Ζ.β,Ζ.γ,Ζ.Ϗ,false);}if(Ч<wpReachedDist/2){Τ(Β.Ē);return;}
}if(Ω==Β.ȱ){if(А){ǫ(true);ћ(τ,false);int Т=-1;double Ǯ=-1;for(int i=Φ.Count-1;i>=0;i--){double Є=Vector3.Distance(Φ[i].Ε,ζ);if(Ǯ==-1||Є<Ǯ){Т=i;Ǯ=Є;}}if(Т==-1){Τ(Β.а);return;}Ѐ=Φ[Т].Ε;л(Ѐ,10);з(Ζ.β,Ζ.γ,Ζ.Ϗ,false);}if(Ч<wpReachedDist){Τ(Β.а);return;}
}if(Ω==Β.Ƽ){bool ǰ=false;if(А){ǫ(true);if((Ш==Љ.ΒΡ||Ш==Љ.Ƹ)&&ǁ()&&
Ĺ(Ǣ,Ψ.GetNaturalGravity())<25&&Ў>=2&&О>=2){Vector3 Ǝ=ζ;if(ύ>0&&ψ<О-1) Ǝ=Ȗ(ύ-1,ψ+1,true);else if(ύ<Ў-1&&ψ<О-1) Ǝ=Ȗ(ύ+1,ψ+1,true);else if(ύ<Ў-1&&ψ>0) Ǝ=Ȗ(ύ+1,ψ-1,true);else if(ύ>0&&ψ>0) Ǝ=Ȗ(ύ-1,ψ-1,true);else ǰ=true;if(!ǰ) л(Ǝ,10);
}else ǰ=true;}if(Ч<wpReachedDist/2) ǰ=true;if(ǰ){Τ(Β.ΑΙ);return;}}if(Ω==Β.ΒЉ||Ω==Β.ΑΙ){if(А){Ǫ(true,true,false,ζ,0);ћ(τ,false);ǫ(true);Б=-1;ж=Ш==Љ.Βί||Ш==Љ.ǀ?0:-1;}bool ģ=!ΒΚ();int ΑЇ=Ȕ("STONE","ORE",С.ē);if(Ш==Љ.ΒΜ||Ш==Љ.ǀ||Ш==Љ.Ƹ)
ΑЇ+=Ȕ("ICE","ORE",С.ē);bool ΘΗ=ΑЇ>0;bool ΓΩ=false;if(ж>=0){float Ň=(float) Math.Sin(Δδ(ж))*Ȓ/3f;float Ηφ=(float) Math.Cos(Δδ(ж))*ǽ/3f;Vector3 Ηώ=Ȗ(ύ,ψ,true)+ΒЇ(Ǣ,ȧ*-1,new Vector3(Ň,Ηφ,0));л(Ηώ,0.3f);if(Ч<Math.Min(Ȓ,ǽ)/10f) ж+=5f;if(ж>=360) ж=0;}if(Б==-1||ΑЇ<Б){Б=ΑЇ;ц=0;}
else{ц++;if(ц>50) ΓΩ=true;}if(!ΘΗ||ģ||ΓΩ){if(!ģ){int ΘЍ=Ȕ("","ORE",С.ē);if(ź(true)) ģ=true;else if(100-(Ќ(ΘЍ,ΖΘ)*100)<minEjection) ģ=true;else Ȅ(м.ΑΑ);}if(ΓΩ&&ģ) Λ="Ejection failed";if(Ω==Β.ΑΙ){if(ģ){if(Ν.δ) Τ(Β.ȱ);else{ω();Ė();Λ="Can´t return, no dock found";}}
else Τ(Β.ȕ);}else if(ģ) Τ(Β.Ę);else Τ(Β.ŀ);return;}}А=false;}void Ζπ(List<Ϊ>đ,int ν,List<Vector3>Ʃ,float ǹ,bool ǥ,ref int ǚ){if(ǥ){for(int i=0;i<đ.Count;i++) đ[i].Р=0;ǚ=-1;return;}if(ν==0) return;int ǐ=ν*-1;if(ǚ==-1) ǚ=ǐ>0?1:đ.Count-2;int Ɩ=0;while(ǚ>=1&&ǚ<đ.Count-1)
{if(Ɩ>50) return;Ɩ++;try{if((ǐ<0&&ǚ>=1)||(ǐ>0&&ǚ<=đ.Count-2)){Ϊ η=đ[ǚ];bool ΖΓ=false;for(int j=0;j<Ʃ.Count;j++)
if(Vector3.Distance(η.Ε,Ʃ[j])<=ǹ){ΖΓ=true;break;}if(!ΖΓ){Ϊ Αώ=đ[ǚ-ǐ];Ϊ ΖΔ=đ[ǚ+ǐ];Vector3 ΘЉ=η.Ε-ΖΔ.Ε;Vector3 ΔΜ=Αώ.Ε-η.Ε;Vector3 Θκ=η.Ε+Vector3.Normalize(ΘЉ)*ΔΜ.Length();Vector3 ΗЋ=Αώ.Ε-Θκ;Vector3 Αρ=Ě(ν>0?η.γ:η.γ*-1,η.β*-1,ΗЋ);Vector3 ΘΧ=Ě(ν>0?η.γ:η.γ*-1,η.β*-1,ΔΜ);
Vector3 Ασ=Ě(ν>0?η.γ:η.γ*-1,η.β*-1,η.ρ);η.Р=(float) Math.Sqrt(Math.Pow(Αώ.Р,2)+Math.Pow(Ƶ(-ΘΧ,Ασ,η),2));for(int j=0;j<Ʃ.Count;j++)
if(Vector3.Distance(Αώ.Ε,Ʃ[j])<=ǹ){Vector3 Ατ=Ě(ν>0?η.γ:η.γ*-1,η.β*-1,Ʃ[j]-η.Ε);float ΘϏ=Ƶ(-Ατ,Ασ,η);η.Р=Math.Min(η.Р,ΘϏ)/2f;}if(Αρ.Length()==0) Αρ=new Vector3(0,0,1);Vector3 ΘΦ=Ě(η.γ,η.β*-1,η.ρ);float ΘЃ=ŷ(-Αρ,ΘΦ,η);float ΑΛ=Ќ(ΘЃ,į);float ΔΙ=(float) Math.Sqrt(Ќ(Αρ.Length()*1.0f,0.5f*ΑΛ));
η.Р=Math.Min(η.Р,(ΔΜ.Length()/ΔΙ)*Ǭ);}}}catch{return;}ǚ+=ǐ;}ǚ=-1;}void ΒЈ(bool ǥ){if(ǥ){Ɨ=0;return;}if(Ξ==Μ.ή) return;float ǰ=Ь*Math.Max(1,в);if(ȥ==Ь) ǰ+=Math.Min(в,Э);float ΘЌ=Ў*О*Math.Max(1,в);Ɨ=Math.Max(Ɨ,(float) Math.Min(ǰ/ΘЌ*100.0,100));}MyDetectedEntityType Δπ()
{try{if(ȃ(σ,true)&&!σ.LastDetectedEntity.IsEmpty()) return σ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΫ(bool ǳ){if(Ξ==Μ.ǖ&&Δπ()==MyDetectedEntityType.None&&!Ź(σ,true))
return fastSpeed;else return ǳ?ų:Ų;}void Ηγ(){float ΕЎ=0,Εδ=0,Ħ=0,ΓЂ=0;for(int i=0;i<ā.Count;i++){IMyBatteryBlock Ƈ=ā[i];if(!ȃ(Ƈ,true)) continue;ΕЎ+=Ƈ.MaxStoredPower;Εδ+=Ƈ.CurrentStoredPower;Ħ+=Ƈ.CurrentInput;ΓЂ+=Ƈ.CurrentOutput;}Ȉ=(float) Math.Round(Ќ(Εδ,ΕЎ)*100,1);
if(Ħ>=ΓЂ) ǭ=ъ.ĕ;else ǭ=ъ.Δε;if(Ħ==0&&ΓЂ==0||Ȉ==100.0) ǭ=ъ.т;if(ā.Count==0) ǭ=ъ.None;}void Ηη(){float Εο=0;for(int i=0;i<ѝ.Count;i++){IMyGasTank Ζή=ѝ[i];if(!ȃ(Ζή,true)) continue;Εο+=(float) Ζή.FilledRatio;}Ȣ=Ќ(Εο,ѝ.Count)*100f;}void Ηπ(){Ĳ=0;try{for(int i=0;i<Ď.Count;i++)
{IMyReactor ΓЉ=Ď[i];if(!ȃ(ΓЉ,true)) continue;List<MyInventoryItem>Ѕ=new List<MyInventoryItem>();ΓЉ.GetInventory(0).GetItems(Ѕ);float Ѓ=0;for(int j=0;j<Ѕ.Count;j++){MyInventoryItem χ=Ѕ[j];if(χ.Type.SubtypeId.ToUpper()=="URANIUM"&&χ.Type.TypeId.ToUpper().Contains("_INGOT"))
Ѓ+=(float) χ.Amount;}if(Ѓ<Ĳ||i==0){Ĳ=Ѓ;Γτ=ΓЉ.CustomName;}}}catch(Exception e){Ȥ=e;}}void Ηδ(){ǵ=0;ǯ=0;try{for(int i=0;i<Ŏ.Count;i++){IMyTerminalBlock Υ=Ŏ[i];if(!ȃ(Υ,true)) continue;ǯ+=(float) Υ.GetInventory(0).CurrentVolume;ǵ+=(float) Υ.GetInventory(0).MaxVolume;
}Απ=(float) Math.Min(Math.Round(Ќ(ǯ,ǵ)*100,1),100.0);}catch(Exception e){Ȥ=e;}}void Ηθ(){try{Ǟ.Clear();for(int i=0;i<Ŏ.Count;i++){IMyTerminalBlock Υ=Ŏ[i];if(!ȃ(Υ,true)) continue;ΔΨ(Υ,С.Ŀ);}if(Ш!=Љ.Βξ){for(int i=0;i<Ğ.Count;i++){IMyTerminalBlock Υ=Ğ[i];if(!ȃ(Υ,true)) continue;
ΔΨ(Υ,С.ΗΔ);}}Ηή(Ǟ);}catch(Exception e){Ȥ=e;}}void Ζς(){if(ΑΪ){if(Δο().Count>Ακ){ΑΪ=false;if(ч!=с.Ÿ){ω();if(ч==с.ΒΤ) Ė();if(ч==с.ǌ)
if(Ν.δ) ƽ();else Ė();}Λ="Damage detected";}}}bool ΒΚ(){if(!Αλ) return true;if(α==Π.З){if(ђ>0&&ǭ!=ъ.None)
if(Ȉ<=ђ){Λ="Low energy! Move home";return false;}if(е>0&&Ď.Count>0)
if(Ĳ<=е){Λ="Low fuel: "+Γτ;return false;}if(ѓ>0&&ѝ.Count>0)
if(Ȣ<=ѓ){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δο(){List<IMyTerminalBlock>ġ=new List<IMyTerminalBlock>();for(int i=0;i<ƃ.Count;i++){IMyTerminalBlock Υ=ƃ[i];if(Ź(Υ,false)) ġ.Add(Υ);}return ġ;}bool Ź(IMyTerminalBlock Υ,bool ΔΒ){return(!ȃ(Υ,ΔΒ)||!Υ.IsFunctional);
}bool ȃ(IMyTerminalBlock Υ,bool ΔΒ){if(Υ==null) return false;try{IMyCubeBlock Ες=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΔΒ) return Ες==Υ;else return Ες.GetType()==Υ.GetType();}catch{return false;}}float Ζό(IMyTerminalBlock Υ,String ј,String ΖΦ,String[]Ǳ){float ȍ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory Ŗ=Υ.GetInventory(i);List<MyInventoryItem>Ѕ=new List<MyInventoryItem>();Ŗ.GetItems(Ѕ);foreach(MyInventoryItem χ in Ѕ){if(Ǳ!=null&&(Ǳ.Contains(χ.Type.TypeId.ToUpper())||Ǳ.Contains(χ.Type.SubtypeId.ToUpper())))
continue;if((ј==""||χ.Type.TypeId.ToUpper()==ј)&&(ΖΦ==""||χ.Type.SubtypeId.ToUpper()==ΖΦ))
ȍ+=(float) χ.Amount;}}return ȍ;}б ΖЅ(String έ,String π,С У,bool ΗЇ){έ=έ.ToUpper();π=π.ToUpper();for(int i=0;i<Ǟ.Count;i++){б χ=Ǟ[i];if(χ.έ.ToUpper()==έ&&χ.π.ToUpper()==π&&(χ.ś==У||У==С.ē))
return χ;}б ά=null;if(ΗЇ){ά=new б(έ,π,0,У);Ǟ.Add(ά);}return ά;}int Ȕ(String έ,String π,С У){return Ȕ(έ,π,У,null);}int Ȕ(String έ,String π,С У,String[]Ǳ){int ȍ=0;έ=έ.ToUpper();π=π.ToUpper();for(int i=0;i<Ǟ.Count;i++){б χ=Ǟ[i];if(Ǳ!=null&&Ǳ.Contains(χ.έ.ToUpper())) continue;
if((έ==""||χ.έ.ToUpper()==έ)&&(π==""||χ.π.ToUpper()==π)&&(χ.ś==У||У==С.ē))
ȍ+=χ.Ѓ;}return ȍ;}void ΔΨ(IMyTerminalBlock Υ,С У){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>Ѕ=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(Ѕ);for(int j=0;j<Ѕ.Count;j++)
ΖЅ(Ѕ[j].Type.SubtypeId,Ѕ[j].Type.TypeId.Replace("MyObjectBuilder_",""),У,true).Ѓ+=(int) Ѕ[j].Amount;}}void Ηή(List<б>Е){for(int i=Е.Count-1;i>0;i--)
for(int j=0;j<i;j++){б a=Е[j];б b=Е[j+1];if(a.Ѓ<b.Ѓ) Е.Move(j,j+1);}}bool ź(bool ΙΞ){if(ȭ&&Ξ!=Μ.ή)
if(Щ!=-1&&į>=Щ){Λ="Ship too heavy";return true;}if(Απ>=Ǵ||Ƥ){Ƥ=false;Λ="Ship is full";return true;}return false;}bool Δώ(){String[]Ǳ=null;if(!Ȭ) Ǳ=new string[]{"ICE"};if(Ξ==Μ.Ž) return Ȕ("","ORE",С.Ŀ,Ǳ)==0;if(Ξ==Μ.ǖ) return Ȕ("","COMPONENT",С.Ŀ,Ǳ)==0;else return Ȕ("","",С.Ŀ,Ǳ)==0;
}bool ΗΗ(){List<IMyCockpit>ΓΞ=new List<IMyCockpit>();ό.GetBlocksOfType(ΓΞ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΞ.Count;i++)
if(ΓΞ[i].IsUnderControl) return true;return false;}bool Ζσ(bool ǥ,bool ΑЄ){IMyShipConnector Х=и(MyShipConnectorStatus.Connected);if(Х==null) return false;if(Vector3.Distance(Ν.Ε,Х.GetPosition())<5)
return Δα(Ё,ǥ,ΑЄ);if(Vector3.Distance(Ζ.Ε,Х.GetPosition())<5)
return Δα(Ђ,ǥ,ΑЄ);return false;}bool Δα(ņ Ȋ,bool ǥ,bool ΑЄ){if(ǥ) Ȋ.ΗΠ();Ȋ.Ην();bool ш=false;String ї="";switch(Ȋ.К){case θ.Βπ:{ї="Waiting for command";ш=false;break;}case θ.ΔЇ:{ї="Waiting for cargo";ш=ź(true);break;}case θ.ΔІ:{ї="Unloading";ш=Δώ();break;}case θ.Βυ:{ш=true;break;}
case θ.Βο:{ї="Charging batteries";ш=Ȉ>=100f;break;}case θ.ǈ:{ї="Discharging batteries";ш=Ȉ<=25f;break;}case θ.Ǉ:{ї="Discharging batteries";ш=Ȉ<=0f;break;}case θ.Βς:{ї="Filling up hydrogen";ш=Ȣ>=100f;break;}case θ.Βσ:{ї="Unloading hydrogen";ш=Ȣ<=25f;break;}
case θ.Βρ:{ї="Unloading hydrogen";ш=Ȣ<=0f;break;}case θ.Βτ:{bool Εϊ=ΗΗ();if(!Εϊ) Ȋ.ΓІ=true;ш=Ȋ.ΓІ&&Εϊ;ї="Waiting for passengers";break;}}if(!ш) Ȋ.ΒΙ(true);if(ш&&Ȋ.Βέ()){ш=Ȋ.ΒΙ(false);ї="Undocking in: "+ƾ((int) Math.Max(0,Ȋ.Ǜ-Ȋ.ą));}if(ΑЄ) Ζκ=ї;return ш;}void Ζν(){
if(!ŗ) return;if(τ.Count<=1) return;float Ζγ=0,Ζβ=0;for(int i=0;i<τ.Count;i++){IMyTerminalBlock Ǽ=τ[i];if(Ź(Ǽ,true)) continue;Ζγ+=(float) Ǽ.GetInventory(0).MaxVolume;Ζβ+=(float) Ǽ.GetInventory(0).CurrentVolume;}float ΕΡ=(float) Math.Round(Ќ(Ζβ,Ζγ),5);for(int ΖΚ=0;ΖΚ<Math.Max(1,Math.Floor(τ.Count/10f));ΖΚ++)
{float Γγ=0,Γσ=0;float Εχ=0,ΕЋ=0;IMyTerminalBlock ȟ=null,Ć=null;for(int i=0;i<τ.Count;i++){IMyTerminalBlock Ǽ=τ[i];if(Ź(Ǽ,true)) continue;float Γϋ=(float) Ǽ.GetInventory(0).MaxVolume;float Αδ=Ќ((float) Ǽ.GetInventory(0).CurrentVolume,Γϋ);if(ȟ==null||Αδ>Γγ){ȟ=Ǽ;Γγ=Αδ;Εχ=Γϋ;}
if(Ć==null||Αδ<Γσ){Ć=Ǽ;Γσ=Αδ;ΕЋ=Γϋ;}}if(ȟ==null||Ć==null||ȟ==Ć) return;if(checkConveyorSystem&&!ȟ.GetInventory(0).IsConnectedTo(Ć.GetInventory(0))){if(ΓΓ>20) Λ="Inventory balancing failed";else ΓΓ++;return;}ΓΓ=0;List<MyInventoryItem>Ѕ=new List<MyInventoryItem>();
ȟ.GetInventory(0).GetItems(Ѕ);float ư=0;if(Ѕ.Count==0) continue;MyInventoryItem Αε=Ѕ[0];String Εύ=Αε.Type.TypeId+Αε.Type.SubtypeId;if(!Ζι.TryGetValue(Εύ,out ư)){if(ΗΚ(ȟ.GetInventory(0),0,Ć.GetInventory(0),out ư))
Ζι.Add(Εύ,ư);else return;}float ΘЊ=((Γγ-ΕΡ)*Εχ/ư);float ΘЋ=((ΕΡ-Γσ)*ΕЋ/ư);int ΔΞ=(int) Math.Min(ΘЋ,ΘЊ);if(ΔΞ<=0) return;if((float) Αε.Amount<ΔΞ)
ȟ.GetInventory(0).TransferItemTo(Ć.GetInventory(0),0,null,null,Αε.Amount);else
ȟ.GetInventory(0).TransferItemTo(Ć.GetInventory(0),0,null,null,ΔΞ);}}bool ΗΚ(IMyInventory ƥ,int Α,IMyInventory ΗЊ,out float Ζθ){Ζθ=0;float Ηϊ=(float) ƥ.CurrentVolume;List<MyInventoryItem>ΓΕ=new List<MyInventoryItem>();ƥ.GetItems(ΓΕ);float ΕΥ=0;for(int i=0;i<ΓΕ.Count;i++) ΕΥ+=(float) ΓΕ[i].Amount;
ƥ.TransferItemTo(ΗЊ,Α,null,null,1);float Ζζ=Ηϊ-(float) ƥ.CurrentVolume;List<MyInventoryItem>ΒЋ=new List<MyInventoryItem>();ƥ.GetItems(ΒЋ);float ΕΙ=0;for(int i=0;i<ΒЋ.Count;i++) ΕΙ+=(float) ΒЋ[i].Amount;if(Ζζ==0f||!Βΰ(0.9999,ΕΥ-ΕΙ,1.0001))
return false;Ζθ=Ζζ;return true;}void Ηζ(){Vector3 ƒ=Ѐ+ΑЋ;Vector3 ǻ=ƒ-ζ;if(ǻ.Length()==0) ǻ=new Vector3(0,0,-1);Vector3 Ατ=к(Ψ,ǻ);Vector3 Ζί=Vector3.Normalize(Ατ);Vector3 Ś=к(Ψ,Ψ.GetNaturalGravity());float Θϊ=ΓЃ>0?Math.Max(0,1-Ĺ(ǻ,Ερ)/5):0;float Р=(float) Math.Min((Γψ>0?Γψ:1000f),Math.Max(Ƶ(-Ατ,Ś,null),ΓЃ*Θϊ));
if(!Ζα) Р=0;if(ΖΣ) Р=Math.Max(0,1-ΑΨ/ΑΜ)*Р;if(generalSpeedLimit>0) Р=Math.Min(generalSpeedLimit,Р);if(Ţ) Р=0;if(Ů>=0) Р=Math.Min(Р,Ů);if(ΖΤ) Р*=(float) Math.Min(1,Ќ(ǻ.Length(),wpReachedDist)/2);Vector3 ΕЈ=к(Ψ,Ψ.GetShipVelocities().LinearVelocity);float Ηυ=(float)(Math.Max(0,15-Ĺ(-Ζί,-ΕЈ))/15)*0.85f+0.15f;
ΔΖ+=Math.Sign(Ηυ-ΔΖ)/10f;Vector3 ă=Ζί*Р*ΔΖ-(ΕЈ);Vector3 ΓΑ=ΒΪ(ă,null);if(ΑЈ&&Ч>0.1f){ă.X*=ΒЁ(ă.X,ref ΑЉ.X,1f,ΓΑ.X,20);ă.Y*=ΒЁ(ă.Y,ref ΑЉ.Y,1f,ΓΑ.Y,20);ă.Z*=ΒЁ(ă.Z,ref ΑЉ.Z,1f,ΓΑ.Z,20);}else ΑЉ=new Vector3(1,1,1);ƪ=į*ă-Ś*į;ΔΩ(ƪ,ΑЈ);Ч=Vector3.Distance(ζ,ƒ);
}float ΒЁ(float value,ref float ώ,float ǐ,float Ȧ,float Γω){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(ώ)) ώ+=Math.Sign(ώ)*ǐ;else ώ=value;if(value==0) ώ=1;float ά=Math.Abs(ώ);if(ά<Γω||Ȧ==0) return ά;ώ=Math.Min(Γω,Math.Max(-Γω,ώ));ά=Math.Abs(Ȧ);return ά;
}void ΕΘ(){float ѕ=90,ǔ=90,ǎ=90;float Ǧ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǳ,ў,ƛ,Γπ;if(Γρ){ǳ=Vector3.Normalize(Ѐ-ζ);ƛ=к(Ψ,ǳ);ў=к(Ψ,ΔΔ);ѕ=Ĺ(ƛ,new Vector3(0,-1,0))-90;ǔ=Ĕ(ў,new Vector3(-1,0,0),ў.Y);
ǎ=Ĕ(ƛ,new Vector3(-1,0,0),ƛ.Z);}else{ǳ=ΔΕ;ў=к(Ψ,ΔΔ);ƛ=к(Ψ,ΔΕ);Γπ=к(Ψ,Ζΰ);ѕ=Ĕ(ў,new Vector3(0,0,1),ў.Y);ǔ=Ĕ(ў,new Vector3(-1,0,0),ў.Y);ǎ=Ĕ(Γπ,new Vector3(0,0,1),Γπ.X);}if(ΕΛ&&ǁ()){Vector3 Γή=Ψ.GetNaturalGravity();ў=к(Ψ,Γή);ѕ=Ĕ(ў,new Vector3(0,0,1),ў.Y);
ǔ=Ĕ(ў,new Vector3(-1,0,0),ў.Y);}if(!Βΰ(-45,ǔ,45)){ѕ=0;ǎ=0;}if(!Βΰ(-45,ǎ,45)) ѕ=0;ΑΓ(Γΰ,1,(-ѕ)*Ǧ,(-ǎ)*Ǧ,(-ǔ)*Ǧ);ΑΨ=Math.Max(Math.Abs(ѕ),Math.Max(Math.Abs(ǔ),Math.Abs(ǎ)));Αμ=ΑΨ<=ΑΜ;}void Ζρ(IMyTerminalBlock ΑϏ){if(ΑϏ==null) return;ǩ=new float[3,2];Ǒ=new Dictionary<string,float[,]>();
for(int i=0;i<ȑ.Count;i++){IMyThrust п=ȑ[i];if(!п.IsFunctional) continue;Vector3 ν=к(ΑϏ,п.WorldMatrix.Backward);float Ą=п.MaxEffectiveThrust;if(Math.Round(ν.X,2)!=0.0)
if(ν.X>=0) ǩ[0,0]+=Ą;else ǩ[0,1]-=Ą;if(Math.Round(ν.Y,2)!=0.0)
if(ν.Y>=0) ǩ[1,0]+=Ą;else ǩ[1,1]-=Ą;if(Math.Round(ν.Z,2)!=0.0)
if(ν.Z>=0) ǩ[2,0]+=Ą;else ǩ[2,1]-=Ą;String ј=Δφ(п);float[,]ǒ=null;if(Ǒ.ContainsKey(ј)) ǒ=Ǒ[ј];else{ǒ=new float[3,2];Ǒ.Add(ј,ǒ);}float Ȧ=п.MaxThrust;if(Math.Round(ν.X,2)!=0.0)
if(ν.X>=0) ǒ[0,0]+=Ȧ;else ǒ[0,1]-=Ȧ;if(Math.Round(ν.Y,2)!=0.0)
if(ν.Y>=0) ǒ[1,0]+=Ȧ;else ǒ[1,1]-=Ȧ;if(Math.Round(ν.Z,2)!=0.0)
if(ν.Z>=0) ǒ[2,0]+=Ȧ;else ǒ[2,1]-=Ȧ;}}static String Δφ(IMyThrust п){return п.BlockDefinition.SubtypeId;}Vector3 Βζ(Vector3 ќ,float[,]Ŝ){return new Vector3(
ќ.X>=0?Ŝ[0,0]:Ŝ[0,1],
ќ.Y>=0?Ŝ[1,0]:Ŝ[1,1],
ќ.Z>=0?Ŝ[2,0]:Ŝ[2,1]);}bool Δυ(Ϊ Ϋ,String ј,out float ΓΨ){ΓΨ=0;int Α=Ȑ.IndexOf(ј);if(Α==-1||Ϋ.Ǩ==null||Α>=Ϋ.Ǩ.Length)
return false;ΓΨ=Ϋ.Ǩ[Α];if(ΓΨ==-1) return false;return true;}Vector3 ΒΪ(Vector3 ќ,Ϊ Ϋ){if(Ϋ!=null){Vector3 ά=new Vector3();for(int i=0;i<Ǒ.Keys.Count;i++){String ј=Ǒ.Keys.ElementAt(i);float Ɛ=0;if(!Δυ(Ϋ,ј,out Ɛ))
return Βζ(ќ,ǩ);ά+=Βζ(ќ,Ǒ.Values.ElementAt(i))*Ɛ;}return ά;}return Βζ(ќ,ǩ);}float ŷ(Vector3 ќ,Ϊ Ϋ){return ŷ(ќ,new Vector3(),Ϋ);}float ŷ(Vector3 ќ,Vector3 ρ,Ϊ Ϋ){Vector3 ΘЂ=ΒΪ(ќ,Ϋ);Vector3 Ą=ΘЂ+ρ*į;float Ĭ=(Ą/ќ).AbsMin();return(float)(ќ*Ĭ).Length();}float Ƶ(Vector3 Ας,Vector3 Ś,Ϊ Ϋ)
{if(Ας.Length()==0) return 0;float ΕΚ=1;if(Ś.Length()>0) ΕΚ=Math.Min(1,Ĺ(-Ś,Ας)/90)*0.4f+0.6f;float ƪ=ŷ(Ας,Ś,Ϋ);if(ƪ==0) return 0.1f;float ΑΛ=Ќ(ƪ,į);float ΔΙ=(float) Math.Sqrt(Ќ(Ας.Length(),ΑΛ*0.5f));return ΑΛ*ΔΙ*ΕΚ*Ǭ;}float ƿ(Vector3 ǳ,Vector3 В,Vector3 ρ,Ϊ Ϋ)
{if(ρ.Length()==0f) return 0;Vector3 Ασ=Ě(ǳ,В,Vector3.Normalize(ρ));float ǝ=ŷ(-Ασ,Ϋ);return ǝ/ρ.Length();}void Δέ(bool ΘΞ){float Ǔ=0;float ΓΫ=0.9f;if(ΘΞ){Щ=-1;ĳ=0;ΑІ=null;if(α!=Π.у&&Ζ.ρ.Length()!=0){Ǔ=ΓΫ*ƿ(Ζ.γ,Ζ.β*-1,Ζ.ρ,null);if(Ǔ<Щ||Щ==-1) Щ=Ǔ;}if(Ν.δ&&Ν.ρ.Length()!=0)
{Ǔ=ΓΫ*ƿ(Ν.γ,Ν.β*-1,Ν.ρ,null);if(Ǔ<Щ||Щ==-1) Щ=Ǔ;}return;}if(ĳ==-1) return;if(ĳ>=0){int ΖΜ=0;while(ĳ<Φ.Count){if(ΖΜ>100) return;ΖΜ++;Ϊ Ϋ=Φ[ĳ];if(Ϋ.ρ.Length()!=0f){Ǔ=ΓΫ*Math.Min(
ƿ(Ϋ.γ,Ϋ.β*-1,Ϋ.ρ,Ϋ),
ƿ(Ϋ.γ*-1,Ϋ.β*-1,Ϋ.ρ,Ϋ));if(Ǔ<Щ||Щ==-1) Щ=Ǔ;}else ΑІ=Ϋ;ĳ++;}ĳ=-1;}bool Γα=true;float ċ=0;if(ΑІ!=null){for(int i=0;i<Ǒ.Count;i++){String ј=Ǒ.Keys.ElementAt(i);float[,]ǒ=Ǒ.Values.ElementAt(i);float Ɛ=0;if(!Δυ(ΑІ,ј,out Ɛ)){Γα=false;break;}for(int a=0;a<ǒ.GetLength(0);a++)
for(int b=0;b<ǒ.GetLength(1);b++){float ĝ=Math.Abs(ǒ[a,b]*Ɛ);if(ĝ==0) continue;Γα=true;if(ċ==0||ĝ<ċ) ċ=ĝ;}}}if(!Γα){for(int a=0;a<ǩ.GetLength(0);a++)
for(int b=0;b<ǩ.GetLength(1);b++){float ĝ=Math.Abs(ǩ[a,b]);if(ĝ==0) continue;if(ċ==0||ĝ<ċ) ċ=ĝ;}}if(ċ>0){float ΘΪ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǔ=Ќ(ċ,ΘΪ);if(Ǔ>0)
if(Ǔ<Щ||Щ==-1) Щ=Ǔ;}}void ΔΩ(Vector3 ǝ,bool Д){if(!Д){for(int i=0;i<ȑ.Count;i++)
ȑ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒБ=ΒΪ(ǝ,null);float ΙΙ=Math.Min(1,Math.Abs(Ќ(ǝ.X,ΒБ.X)));float ΙΚ=Math.Min(1,Math.Abs(Ќ(ǝ.Y,ΒБ.Y)));float ΙΛ=Math.Min(1,Math.Abs(Ќ(ǝ.Z,ΒБ.Z)));for(int i=0;i<ȑ.Count;i++){IMyThrust п=ȑ[i];Vector3 ν=ΗΣ(к(Ψ,п.WorldMatrix.Backward),1);
if(ν.X!=0&&Math.Sign(ν.X)==Math.Sign(ǝ.X))
п.SetValueFloat("Override",п.MaxThrust*ΙΙ);else if(ν.Y!=0&&Math.Sign(ν.Y)==Math.Sign(ǝ.Y))
п.SetValueFloat("Override",п.MaxThrust*ΙΚ);else if(ν.Z!=0&&Math.Sign(ν.Z)==Math.Sign(ǝ.Z))
п.SetValueFloat("Override",п.MaxThrust*ΙΛ);else
п.SetValueFloat("Override",0.0f);}}void ΑΓ(bool Д,float Θη,float ѕ,float ǎ,float ǔ){for(int i=0;i<Αη.Count;i++){IMyGyro Ѝ=Αη[i];Ѝ.GyroOverride=Д;if(!Д) Ѝ.GyroPower=100;else Ѝ.GyroPower=Θη;if(!Д) continue;Vector3 Ю=Ψ.WorldMatrix.Forward;Vector3 і=Ψ.WorldMatrix.Right;Vector3 В=Ψ.WorldMatrix.Up;
Vector3 Œ=Ѝ.WorldMatrix.Forward;Vector3 Ŕ=Ѝ.WorldMatrix.Up;Vector3 œ=Ѝ.WorldMatrix.Left*-1;if(Œ==Ю) Ѝ.SetValueFloat("Roll",ǔ);else if(Œ==(Ю*-1)) Ѝ.SetValueFloat("Roll",ǔ*-1);else if(Ŕ==(Ю*-1)) Ѝ.SetValueFloat("Yaw",ǔ);else if(Ŕ==Ю) Ѝ.SetValueFloat("Yaw",ǔ*-1);else if(œ==Ю) Ѝ.SetValueFloat("Pitch",ǔ);
else if(œ==(Ю*-1)) Ѝ.SetValueFloat("Pitch",ǔ*-1);if(œ==(і*-1)) Ѝ.SetValueFloat("Pitch",ѕ);else if(œ==і) Ѝ.SetValueFloat("Pitch",ѕ*-1);else if(Ŕ==і) Ѝ.SetValueFloat("Yaw",ѕ);else if(Ŕ==(і*-1)) Ѝ.SetValueFloat("Yaw",ѕ*-1);else if(Œ==(і*-1)) Ѝ.SetValueFloat("Roll",ѕ);
else if(Œ==і) Ѝ.SetValueFloat("Roll",ѕ*-1);if(Ŕ==(В*-1)) Ѝ.SetValueFloat("Yaw",ǎ);else if(Ŕ==В) Ѝ.SetValueFloat("Yaw",ǎ*-1);else if(œ==В) Ѝ.SetValueFloat("Pitch",ǎ);else if(œ==(В*-1)) Ѝ.SetValueFloat("Pitch",ǎ*-1);else if(Œ==В) Ѝ.SetValueFloat("Roll",ǎ);else if(Œ==(В*-1)) Ѝ.SetValueFloat("Roll",ǎ*-1);
}}void ΕΔ(){this.Γΰ=false;}void з(Vector3 ƌ,Vector3 Ю,Vector3 Γο,float ȫ,bool Ư){ΒΑ(ƌ,ȫ,Ư);ΑΜ=ȫ;Γρ=false;this.ΔΕ=Ю;this.Ζΰ=Γο;}void з(Vector3 ƌ,Vector3 Ю,Vector3 Γο,bool Ư){з(ƌ,Ю,Γο,2f,Ư);}void ΒΑ(Vector3 ƌ,float ȫ,bool Ư){ΑΜ=ȫ;this.Γΰ=true;this.ΕΛ=Ư;Γρ=true;Αμ=false;
this.ΔΔ=ƌ;}void ΒІ(){Ǫ(false,false,false,Ѐ,0);ΑЈ=false;}void л(Vector3 μ,float Ǧ){Ǫ(true,false,false,μ,Ǧ);}void Ǫ(bool Д,bool ΓЏ,bool ΔΣ,Vector3 μ,float Ǧ){Ǫ(Д,ΓЏ,ΔΣ,μ,μ-ζ,0.0f,Ǧ);}void Ǫ(bool Д,bool ΓЏ,bool ΔΣ,Vector3 μ,Vector3 Θτ,float Θυ,float Ǧ){ΑЈ=true;this.Ζα=Д;
Ѐ=μ;this.Γψ=Ǧ;this.ΓЃ=Θυ;this.ΖΤ=ΓЏ;this.ΖΣ=ΔΣ;this.Ερ=Θτ;Ч=Vector3.Distance(μ,ζ);}void ΗΤ(){ό.GetBlocksOfType(ƃ,њ);}void ΗΥ(){if(ΑЁ){Ј=2;return;}List<IMyRemoteControl>ơ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΠ=new List<IMySensorBlock>();List<IMyTerminalBlock>ƨ=new List<IMyTerminalBlock>();
ό.GetBlocksOfType(ơ,њ);ό.GetBlocksOfType(П,њ);ό.GetBlocksOfType(ΖΠ,њ);ό.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ƨ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);П=Ǖ(П,pamTag,true);ΔА();ΔБ(ƨ);ę(П,setLCDFontAndSize,1.4f,false);ę(Ǻ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΖΫ=Ǖ(ΖΠ,pamTag,true);if(ΖΫ.Count>0) σ=ΖΫ[0];else σ=null;if(Ξ==Μ.Ž){ό.GetBlocksOfType(τ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(τ.Count==0){Ј=1;Λ="Drills are missing";}}else if(Ξ==Μ.ǖ){ό.GetBlocksOfType(τ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(τ.Count==0){Ј=1;Λ="Grinders are missing";}if(σ==null){Ј=1;Λ="Sensor is missing";}}else if(Ξ==Μ.ή){ό.GetBlocksOfType(ΔΚ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΖΪ=Ǖ(ơ,pamTag,true);if(ΖΪ.Count>0) ơ=ΖΪ;if(ơ.Count>0) Ψ=ơ[0];else{Ψ=null;Ј=2;Λ="Remote is missing";return;}λ=(float) Ψ.CubeGrid.WorldVolume.Radius*2;Ǎ=null;if(Ξ!=Μ.ή){Δή();if(τ.Count>0&&Ǎ!=null){if(σ!=null&&(Ǎ.WorldMatrix.Forward!=σ.WorldMatrix.Forward||
!(Ψ.WorldMatrix.Forward==σ.WorldMatrix.Up||Ψ.WorldMatrix.Down==σ.WorldMatrix.Down))){Ј=1;Λ="Wrong sensor direction";}if(Ǎ.WorldMatrix.Forward!=Ψ.WorldMatrix.Forward&&
Ǎ.WorldMatrix.Forward!=Ψ.WorldMatrix.Down){Ј=2;Λ="Wrong remote direction";}}}}void ΗΦ(){ό.GetBlocksOfType(ȏ,њ);ό.GetBlocksOfType(ю,њ);ό.GetBlocksOfType(ȑ,њ);ό.GetBlocksOfType(Αη,њ);ό.GetBlocksOfType(ā,њ);ό.GetBlocksOfType(Ď,њ);ό.GetBlocksOfType(ѝ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ό.GetBlocksOfType(ΑЅ,њ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ю=Ǖ(ю,"ConnectorMedium",false);else
ю=Ǖ(ю,"Connector",false);List<IMyShipConnector>ΖΨ=Ǖ(ю,pamTag,true);if(ΖΨ.Count>0) ю=ΖΨ;if(Ј<=1){if(ю.Count==0){Ј=1;Λ="Connector is missing";}if(Αη.Count==0){Ј=1;Λ="Gyros are missing";}if(ȑ.Count==0){Ј=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Ζά=Ǖ(ΑЅ,pamTag,true);
if(Ζά.Count>0) ΑЅ=Ζά;List<IMyLandingGear>ΖΩ=Ǖ(ȏ,pamTag,true);if(ΖΩ.Count>0) ȏ=ΖΩ;for(int i=0;i<ȏ.Count;i++) ȏ[i].AutoLock=false;List<IMyBatteryBlock>ΖΧ=Ǖ(ā,pamTag,true);if(ΖΧ.Count>0) ā=ΖΧ;List<IMyGasTank>Ζέ=Ǖ(ѝ,pamTag,true);if(Ζέ.Count>0) ѝ=Ζέ;}void ΗΧ(){ό.GetBlocksOfType(Ğ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ŏ.Clear();for(int i=Ğ.Count-1;i>=0;i--){if(ΗΕ(Ğ[i])){Ŏ.Add(Ğ[i]);Ğ.RemoveAt(i);}}}bool ΗΕ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βή(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δή(){if(Ψ==null) return;Ǎ=null;float Αϊ=0,Αυ=0,Αϋ=0,Αφ=0,Αό=0,Αχ=0;List<IMyTerminalBlock>ΔΓ=Ǖ(τ,pamTag,true);bool Θΰ=ΔΓ.Count==0;if(ΔΓ.Count>0) Ǎ=ΔΓ[0];else if(τ.Count>0) Ǎ=τ[0];int ȍ=0;for(int i=0;i<τ.Count;i++)
{if(τ[i].WorldMatrix.Forward!=Ǎ.WorldMatrix.Forward){if(Θΰ){Ј=2;Λ="Mining direction is unclear!";return;}continue;}ȍ++;Vector3 Ж=ΒΧ(Ψ,τ[i].GetPosition());if(i==0){Αϊ=Ж.X;Αυ=Ж.X;Αϋ=Ж.Y;Αφ=Ж.Y;Αό=Ж.Z;Αχ=Ж.Z;}Αυ=Math.Max(Ж.X,Αυ);Αϊ=Math.Min(Ж.X,Αϊ);Αφ=Math.Max(Ж.Y,Αφ);Αϋ=Math.Min(Ж.Y,Αϋ);
Αχ=Math.Max(Ж.Z,Αχ);Αό=Math.Min(Ж.Z,Αό);}Ȓ=(Αυ-Αϊ)*(1-Ĵ)+drillRadius*2;ǽ=(Αφ-Αϋ)*(1-ć)+drillRadius*2;if(Ǎ!=null&&Ǎ.WorldMatrix.Forward==Ψ.WorldMatrix.Down)
ǽ=(Αχ-Αό)*(1-ć)+drillRadius*2;}List<T>Ǖ<T>(List<T>Е,String ǧ,bool Αθ){List<T>ά=new List<T>();for(int i=0;i<Е.Count;i++)
if(Βή(Е[i],ǧ,Αθ)) ά.Add(Е[i]);return ά;}bool Βή<T>(T Υ,String ǧ,bool Αθ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(Αθ&&b.CustomName.ToUpper().Contains(ǧ.ToUpper())) return true;if(!Αθ&&b.BlockDefinition.ToString().ToUpper().Contains(ǧ.ToUpper())) return true;return false;}void ћ<T>(List<T>Е,bool Д)
{for(int i=0;i<Е.Count;i++)
ΔЍ((IMyTerminalBlock) Е[i],Д);}void ΔЍ(IMyTerminalBlock Υ,bool Д){if(Υ==null) return;String Ησ=Д?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(Ησ);a.Apply(Υ);}void ǫ(bool Д){ΓА=Д;if(!İ) return;ћ(ΑЅ,Д);}void Βώ(ChargeMode Ї){for(int i=0;i<ā.Count;i++) ā[i].ChargeMode=Ї;}void ΒϏ(bool Θό)
{for(int i=0;i<ѝ.Count;i++) ѝ[i].Stockpile=Θό;}void ſ(List<IMyLandingGear>Γά,bool ΕЉ){for(int i=0;i<Γά.Count;i++){if(ΕЉ) Γά[i].Lock();if(!ΕЉ) Γά[i].Unlock();}}void ΔЎ(bool Д){if(Εε==Д) return;List<IMyShipController>ΑΦ=new List<IMyShipController>();ό.GetBlocksOfType(ΑΦ,њ);if(ΑΦ.Count==0) return;
for(int i=0;i<ΑΦ.Count;i++)
ΑΦ[i].DampenersOverride=Д;Εε=Д;}IMyShipConnector и(MyShipConnectorStatus ї){for(int i=0;i<ю.Count;i++){if(!ȃ(ю[i],true)) continue;if(ю[i].Status==ї) return ю[i];}return null;}void ě(bool ΓЌ,bool Αά,float ΘΓ,float Ηω){if(σ==null||τ.Count==0) return;Vector3 ΑΣ=new Vector3();
int ȍ=0;for(int i=0;i<τ.Count;i++){if(τ[i].WorldMatrix.Forward!=Ǎ.WorldMatrix.Forward) continue;ȍ++;ΑΣ+=τ[i].GetPosition();}ΑΣ=ΑΣ/ȍ;Vector3 ş=ΒΧ(σ,ΑΣ);σ.Enabled=true;σ.ShowOnHUD=ΓЌ;σ.LeftExtend=(Αά?1:х)/2f*Ȓ-ş.X;σ.RightExtend=(Αά?1:х)/2f*Ȓ+ş.X;σ.TopExtend=(Αά?1:ф)/2f*ǽ+ş.Y;
σ.BottomExtend=(Αά?1:ф)/2f*ǽ-ş.Y;σ.FrontExtend=(ΓЌ?в:ΘΓ)-ş.Z;σ.BackExtend=ΓЌ?0:Ηω+λ*0.75f+ş.Z;σ.DetectFloatingObjects=true;σ.DetectAsteroids=false;σ.DetectLargeShips=true;σ.DetectSmallShips=true;σ.DetectStations=true;σ.DetectOwner=true;σ.DetectSubgrids=false;
σ.DetectPlayers=false;σ.DetectEnemy=true;σ.DetectFriendly=true;σ.DetectNeutral=true;}void ę<T>(List<T>Е,bool Θχ,float ΘΑ,bool ΘΟ){for(int i=0;i<Е.Count;i++){IMyTextSurface Ƨ=null;if(Е[i]is IMyTextSurface) Ƨ=(IMyTextSurface) Е[i];if(Ƨ!=null){Ƨ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Θχ) continue;Ƨ.Font="Debug";if(ΘΟ) continue;Ƨ.FontSize=ΘΑ;}}}bool ǁ(){Vector3D ι;return this.Ψ.TryGetPlanetPosition(out ι);}int ΖЎ(Vector3 Θώ,int ΖΥ){if(ΖΥ==-1) return 0;double Т=-1;int ȝ=-1;for(int i=Φ.Count-1;i>=0;i--){double Є=Vector3.Distance(Φ[i].Ε,Θώ);if(Т==-1||Є<Т){ȝ=i;Т=Є;}
}return Math.Sign(ȝ-ΖΥ);}bool ΗΖ(Vector3 ι){List<Vector3>Ф=new List<Vector3>();for(int i=0;i<Φ.Count;i++) Ф.Add(Φ[i].Ε);if(Ν.δ&&Φ.Count>=1){Vector3 ŉ=new Vector3();Ȃ(Ν,dockDist*λ,false,out ŉ);if(Vector3.Distance(Ν.Ε,Φ.First().Ε)<Vector3.Distance(Ν.Ε,Φ.Last().Ε)){Ф.Insert(0,ŉ);Ф.Insert(0,Ν.Ε);}
else{Ф.Add(ŉ);Ф.Add(Ν.Ε);}}if(Ξ==Μ.ή){if(Ζ.δ&&Φ.Count>=1){Vector3 ΒЍ=new Vector3();Ȃ(Ζ,dockDist*λ,false,out ΒЍ);if(Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε)){Ф.Insert(0,ΒЍ);Ф.Insert(0,Ζ.Ε);}else{Ф.Add(ΒЍ);Ф.Add(Ζ.Ε);}}}else{if(α!=Π.у)
if(Φ.Count>0&&Vector3.Distance(Ζ.Ε,Φ.First().Ε)<Vector3.Distance(Ζ.Ε,Φ.Last().Ε))
Ф.Insert(0,Ζ.Ε);else Ф.Add(Ζ.Ε);}int ȝ=-1;double Ǯ=-1;for(int i=0;i<Ф.Count;i++){double Є=Vector3.Distance(Ф[i],ι);if(Є<Ǯ||Ǯ==-1){Ǯ=Є;ȝ=i;}}if(Ф.Count==0) return false;double ΓΥ=Vector3.Distance(Ф[ȝ],ι);double Θθ=Vector3.Distance(Ф[Math.Max(0,ȝ-1)],Ф[ȝ])*1.5f;
double Θί=Vector3.Distance(Ф[Math.Min(Ф.Count-1,ȝ+1)],Ф[ȝ])*1.5f;return ΓΥ<Θθ||ΓΥ<Θί||ΓΥ<laneOffset*λ*1.5f;}static float Ќ(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΗΣ(Vector3 v,int ΓΡ){return new Vector3(Math.Round(v.X,ΓΡ),Math.Round(v.Y,ΓΡ),Math.Round(v.Z,ΓΡ));
}float Ĺ(Vector3 a,Vector3 b){if(a==b) return 0;float ΗЏ=(a*b).Sum;float ΕЃ=a.Length();float ΕЄ=b.Length();if(ΕЃ==0||ΕЄ==0) return 0;float ά=(float)((180.0f/Math.PI)*Math.Acos(ΗЏ/(ΕЃ*ΕЄ)));if(float.IsNaN(ά)) return 0;return ά;}float Ĕ(Vector3 v,Vector3 ΑϏ,float Θψ){float Ň=Ĺ(v,ΑϏ);
if(Θψ>0f) Ň*=-1;if(Ň>-90f) return Ň-90f;else return 180f-(-Ň-90f);}double Δδ(float ΗЈ){return(Math.PI/180)*ΗЈ;}bool Βΰ(double ΘΩ,double value,double Γχ){return(value>=ΘΩ&&value<=Γχ);}Vector3 Ηβ(IMyTerminalBlock Υ,Vector3 Ж){return Vector3D.Transform(Ж,Υ.WorldMatrix);
}Vector3 ΒΧ(IMyTerminalBlock Υ,Vector3 ΙΖ){return к(Υ,ΙΖ-Υ.GetPosition());}Vector3 к(IMyTerminalBlock Υ,Vector3 ΔΧ){return Vector3D.TransformNormal(ΔΧ,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 Ě(Vector3 ǳ,Vector3 В,Vector3 ΔΧ){MatrixD Γφ=MatrixD.CreateFromDir(ǳ,В);return Vector3D.TransformNormal(ΔΧ,MatrixD.Transpose(Γφ));
}Vector3 ΒЇ(Vector3 ǳ,Vector3 В,Vector3 Ж){MatrixD Γφ=MatrixD.CreateFromDir(ǳ,В);return Vector3D.Transform(Ж,Γφ);}String ϋ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 ϊ(String s){String[]ο=s.Split('|');return new Vector3(float.Parse(Ο(ο,0)),float.Parse(Ο(ο,1)),float.Parse(Ο(ο,2)));
}String Ηρ(Ϊ Ϋ){String Й=":";String ά=ϋ(Ϋ.Ε)+Й+ϋ(Ϋ.γ)+Й+
ϋ(Ϋ.β)+Й+ϋ(Ϋ.Ϗ)+Й+ϋ(Ϋ.ρ);for(int i=0;i<Ϋ.Ǩ.Length;i++)
ά+=Й+Math.Round(Ϋ.Ǩ[i],3);return ά;}Ϊ Ηί(String s){String[]ο=s.Split(':');Ϊ Ϋ=new Ϊ(
ϊ(Ο(ο,0)),ϊ(Ο(ο,1)),
ϊ(Ο(ο,2)),ϊ(Ο(ο,3)),
ϊ(Ο(ο,4)));int Α=5;List<float>Εξ=new List<float>();while(Α<ο.Length){String ΑЍ=Ο(ο,Α);float f=0;if(!float.TryParse(ΑЍ,out f)) break;Εξ.Add(f);Α++;}Ϋ.Ǩ=Εξ.ToArray();return Ϋ;}System.Text.StringBuilder Ŧ=new System.Text.StringBuilder();void Σ<T>(T value,bool Θή){if(Θή) Ŧ.Append('\n');Ŧ.Append(value);}
void Σ<T>(T value){Σ(value,true);}String ƹ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΘ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ο(String[]Ηχ,int Α){String ΑЍ=Ηχ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ΑЍ)) return "";
return ΑЍ;}void Save(){if(ΑЁ||Ξ==Μ.ы){Storage="";return;}Storage=ΒΗ();}String ΒΗ(){Ŧ.Clear();Ŧ.Append(DATAREV+";");Σ(ϋ(ΑЀ),false);Σ(ϋ(Ν.γ));Σ(ϋ(Ν.Ϗ));Σ(ϋ(Ν.β));Σ(ϋ(Ν.ρ));Σ(ϋ(Ν.Ε));Σ(ϋ(Ν.г));Σ(Ν.δ);Σ(ϋ(Ζ.Ε));Σ(ϋ(Ζ.ρ));Σ(ϋ(Ζ.γ));Σ(ϋ(Ζ.β));Σ(ϋ(Ζ.Ϗ));Σ(ϋ(Ζ.г));Σ(Ζ.δ);Σ(ϋ(Ǣ));Σ(ϋ(ȧ));Σ(";");
Σ((int) Ξ,false);Σ((int) α);Σ((int) ё);Σ(Ǵ);Σ(ђ);Σ(е);Σ(ѓ);Σ(Ǭ);Σ(ȭ);Σ(Ȭ);if(Ξ==Μ.ή){Σ((int) Ё.К);Σ(Ё.Ǜ);Σ(Ё.ą);Σ(ƹ(Ё.ǜ));Σ(ƹ(Ё.ǟ));Σ((int) Ђ.К);Σ(Ђ.Ǜ);Σ(Ђ.ą);Σ(ƹ(Ђ.ǜ));Σ(ƹ(Ђ.ǟ));}else{Σ((int) Ǹ);Σ((int) ч);Σ((int) Ш);Σ((int) я);Σ((int) Ƙ);Σ(Đ);Σ(İ);Σ(ŗ);Σ(Ő);Σ(х);Σ(ф);Σ(в);Σ(ų);Σ(Ų);Σ(Ĵ);Σ(ć);
Σ(Ў);Σ(О);Σ(ύ);Σ(ψ);Σ(ȥ);Σ(Ь);Σ(ĭ);Σ(ĩ);}Σ(";");for(int i=0;i<Ȑ.Count;i++)
Σ((i>0?"|":"")+Ȑ[i],false);Σ(";");for(int i=0;i<Φ.Count;i++)
Σ(Ηρ(Φ[i]),i>0);Σ(";",false);Σ(ѐ+"|"+ǣ,false);Σ(";",false);Σ(ΖϏ(),false);String Χ=Ŧ.ToString();Ŧ.Clear();return Χ;}ь ΗΡ(){return Ǌ(Storage);}ь Ǌ(String Χ){if(Χ=="") return ь.ΗΜ;String[]Į=Χ.Split(';');if(Ο(Į,0)!=DATAREV) return ь.Βμ;int Α=0;try{String[]Κ=Ο(Į,1).Split('\n');
ΑЀ=ϊ(Ο(Κ,Α++));Ν.γ=ϊ(Ο(Κ,Α++));Ν.Ϗ=ϊ(Ο(Κ,Α++));Ν.β=ϊ(Ο(Κ,Α++));Ν.ρ=ϊ(Ο(Κ,Α++));Ν.Ε=ϊ(Ο(Κ,Α++));Ν.г=ϊ(Ο(Κ,Α++));Ν.δ=bool.Parse(Ο(Κ,Α++));Ζ.Ε=ϊ(Ο(Κ,Α++));Ζ.ρ=ϊ(Ο(Κ,Α++));Ζ.γ=ϊ(Ο(Κ,Α++));Ζ.β=ϊ(Ο(Κ,Α++));Ζ.Ϗ=ϊ(Ο(Κ,Α++));Ζ.г=ϊ(Ο(Κ,Α++));Ζ.δ=bool.Parse(Ο(Κ,Α++));
Ǣ=ϊ(Ο(Κ,Α++));ȧ=ϊ(Ο(Κ,Α++));Κ=Ο(Į,2).Split('\n');Α=0;Ξ=(Μ) int.Parse(Ο(Κ,Α++));α=(Π) int.Parse(Ο(Κ,Α++));ё=(Π) int.Parse(Ο(Κ,Α++));Ǵ=int.Parse(Ο(Κ,Α++));ђ=int.Parse(Ο(Κ,Α++));е=int.Parse(Ο(Κ,Α++));ѓ=int.Parse(Ο(Κ,Α++));Ǭ=float.Parse(Ο(Κ,Α++));ȭ=bool.Parse(Ο(Κ,Α++));
Ȭ=bool.Parse(Ο(Κ,Α++));if(Ξ==Μ.ή){Ё.К=(θ) int.Parse(Ο(Κ,Α++));Ё.Ǜ=float.Parse(Ο(Κ,Α++));Ё.ą=float.Parse(Ο(Κ,Α++));Ё.ǜ=ΑΘ(Ο(Κ,Α++));Ё.ǟ=ΑΘ(Ο(Κ,Α++));Ђ.К=(θ) int.Parse(Ο(Κ,Α++));Ђ.Ǜ=float.Parse(Ο(Κ,Α++));Ђ.ą=float.Parse(Ο(Κ,Α++));Ђ.ǜ=ΑΘ(Ο(Κ,Α++));Ђ.ǟ=ΑΘ(Ο(Κ,Α++));}else
{Ǹ=(ǘ) int.Parse(Ο(Κ,Α++));ч=(с) int.Parse(Ο(Κ,Α++));Ш=(Љ) int.Parse(Ο(Κ,Α++));я=(љ) int.Parse(Ο(Κ,Α++));Ƙ=(ǘ) int.Parse(Ο(Κ,Α++));Đ=bool.Parse(Ο(Κ,Α++));İ=bool.Parse(Ο(Κ,Α++));ŗ=bool.Parse(Ο(Κ,Α++));Ő=bool.Parse(Ο(Κ,Α++));х=int.Parse(Ο(Κ,Α++));ф=int.Parse(Ο(Κ,Α++));в=int.Parse(Ο(Κ,Α++));
ų=float.Parse(Ο(Κ,Α++));Ų=float.Parse(Ο(Κ,Α++));Ĵ=float.Parse(Ο(Κ,Α++));ć=float.Parse(Ο(Κ,Α++));Ў=int.Parse(Ο(Κ,Α++));О=int.Parse(Ο(Κ,Α++));ύ=int.Parse(Ο(Κ,Α++));ψ=int.Parse(Ο(Κ,Α++));ȥ=int.Parse(Ο(Κ,Α++));Ь=int.Parse(Ο(Κ,Α++));ĭ=int.Parse(Ο(Κ,Α++));ĩ=float.Parse(Ο(Κ,Α++));
}Κ=Ο(Į,3).Replace("\n","").Split('|');Ȑ=Κ.ToList();Κ=Ο(Į,4).Split('\n');Φ.Clear();if(Κ.Count()>=1&&Κ[0]!="")
for(int i=0;i<Κ.Length;i++)
Φ.Add(Ηί(Ο(Κ,i)));Κ=Ο(Į,5).Split('|');ѐ=Ο(Κ,0);if(!int.TryParse(Ο(Κ,1),out ǣ)) ǣ=-1;Ζώ(Ο(Į,6));}catch{return ь.Δι;}ŧ=ё;ω();return ь.ΑΕ;}const int Δά=64000;IMyTerminalBlock Δκ(){List<IMyTerminalBlock>Ō=new List<IMyTerminalBlock>();ό.GetBlocksOfType(Ō,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ō.Count>0?Ō[0]:null;}void ΖЃ(){IMyTerminalBlock Υ=Δκ();if(Υ==null){Λ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΗ();if(Χ.Length>Δά){Λ="Export too large: "+Χ.Length+" chars, max "+Δά;return;}Υ.CustomData=Χ;Λ="Exported "+Φ.Count+" waypoints";}void ΗΒ(bool Γθ){IMyTerminalBlock Υ=Δκ();if(Υ==null){Λ="Import: name a block "+dataTag;return;}
ΗΓ(Υ.CustomData,Γθ);}void ΗΓ(String Χ,bool Γθ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΕ=Χ.IndexOf('\n');Χ=ΖΕ<0?"":Χ.Substring(ΖΕ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}ω();String ΕΣ=ΒΗ();ь ά=Ǌ(Χ);if(ά==ь.ΑΕ&&Γθ){Ϊ ř=new Ϊ(Ζ);
Vector3 Θά=Ǣ,ΘΫ=ȧ;int w=х,h=ф,d=в;ǘ ΓБ=Ǹ;љ ΓΣ=я;float Θγ=Ĵ,Θβ=ć;Ǌ(ΕΣ);Ζ=ř;Ǣ=Θά;ȧ=ΘΫ;х=w;ф=h;в=d;Ǹ=ΓБ;я=ΓΣ;Ĵ=Θγ;ć=Θβ;α=Π.Ł;Ɓ(true,true);α=Π.Ł;ё=Π.Ł;Λ="Imported job, run CONT";return;}if(ά==ь.ΑΕ){ƞ=false;Λ="Imported "+Φ.Count+" waypoints";
return;}Ǌ(ΕΣ);Λ=ά==ь.Βμ?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΔА(){Ģ.Clear();for(int i=П.Count-1;i>=0;i--){String Χ=П[i].CustomData.ToUpper();bool Γζ=false;if(Χ=="INSTRUCTIONS"){Γζ=true;ΑΫ=true;}if(Χ=="DEBUG") Γζ=true;if(Γζ){Ģ.Add(П[i]);П.RemoveAt(i);}
}ę(Ģ,false,1,false);}void ΔБ(List<IMyTerminalBlock>Ō){Ǻ.Clear();for(int i=0;i<Ō.Count;i++){IMyTerminalBlock Υ=Ō[i];try{String ΖΛ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Υ.CustomName.IndexOf(ΖΛ);int ū=-1;if(Α<0||!int.TryParse(Υ.CustomName.Substring(Α+ΖΛ.Length,1),out ū)) continue;
if(ū==-1) continue;ū--;IMyTextSurfaceProvider ΖΝ=(IMyTextSurfaceProvider) Υ;if(ū<ΖΝ.SurfaceCount&&ū>=0)
Ǻ.Add(ΖΝ.GetSurface(ū));}catch{}}}String Αψ="";void ΔЊ(){String Γυ="";String Ə="";Αψ=ΔΫ(false);Γυ+=Αψ;Ə+=Αψ;Ə+=ΖЊ();for(int i=0;i<П.Count;i++) П[i].WriteText(Γυ);for(int i=0;i<Ǻ.Count;i++) Ǻ[i].WriteText(Γυ);Echo(Ə);for(int i=0;i<Ģ.Count;i++){IMyTextPanel о=Ģ[i];
String Χ=о.CustomData.ToUpper();if(Χ=="DEBUG") о.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") о.WriteText(Δρ());}}string Δρ(){String Γ="";try{float ΕЌ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Γ+="Inst. avg/max: "+(int)(ŋ*ΕЌ)+" / "+(int)(ŝ*ΕЌ)+"\n";
Γ+="Inst. avg/max: "+Math.Round(ŋ*100f,1)+"% / "+Math.Round(ŝ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ɔ,2)+"ms / "+Math.Round(Γϊ,2)+"ms \n";}catch{}for(int i=0;i<Ȩ.Count;i++)
Γ+=""+Ȩ.Keys.ElementAt(i)+" = "+Math.Round(Ȩ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȩ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΑ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ę(П,setLCDFontAndSize,0.9f,false);ę(Ǻ,setLCDFontAndSize,0.9f,true);for(int i=0;i<П.Count;i++) П[i].WriteText(Γ+Ȥ.ToString());for(int i=0;i<Ǻ.Count;i++) Ǻ[i].WriteText(Γ+Ȥ.ToString());}void ȅ(){if(!ΑΫ) return;try{ΓЇ=Runtime.CurrentInstructionCount;}catch{}
}void ȁ(String έ){if(!ΑΫ) return;if(ΓЇ==0) return;try{float ΑЌ=(Runtime.CurrentInstructionCount-ΓЇ)/Runtime.MaxInstructionCount*100;if(!Ȩ.ContainsKey(έ))
Ȩ.Add(έ,new float[]{ΑЌ,ΑЌ});else{Ȩ[έ][0]=ΑЌ;Ȩ[έ][1]=Math.Max(ΑЌ,Ȩ[έ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζξ(String ǲ){if(!ō) return;String Й=""+BROADCAST_SEP;String ξ=VERSION+Й;ξ+=Ŵ(Ū)+Й;
ξ+=(int) Ξ+Й;ξ+=Ľ(ζ.X)+""+Й;ξ+=Ľ(ζ.Y)+Й;ξ+=Ľ(ζ.Z)+Й;if(Ξ!=Μ.ή)
ξ+=ΒΦ(ё)+(ё==Π.З?" "+Math.Round(Ɨ,1)+"%":"")+Й;else ξ+=ΒΦ(ё)+Й;ξ+=Ŵ(Λ)+Й;ξ+=І+""+Й;ξ+=Ŵ(Αψ)+Й;ξ+=ǯ+Й;ξ+=ǵ+Й;for(int i=0;i<Ǟ.Count;i++){if(Ǟ[i].ś!=С.Ŀ) continue;ξ+=Ŵ(Ǟ[i].έ)+"/"+Ŵ(Ǟ[i].π)+"/"+Ǟ[i].Ѓ+Й;}ΗΪ(ξ,ǲ);}String Ŵ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΗΪ(String ξ,String ǲ){Βό(ξ,BROADCAST_SELF_ID,ǲ);}void Βό(String ξ,String μ,String ǲ){try{if(!ō) return;ξ=BROADCAST_TAG+BROADCAST_SEP+Δν()+BROADCAST_SEP+μ+BROADCAST_SEP+ΓΘ+BROADCAST_SEP+ǲ+BROADCAST_SEP+ξ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ξ);}catch(Exception e){Ȥ=e;}
}String Δν(){if(Ξ!=Μ.ы) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Βδ(String ξ){return ξ.StartsWith(BROADCAST_TAG);}bool Ы(ref String Χ,out string Ƒ,Char Й){int Α=Χ.IndexOf(Й);Ƒ="";if(Α<0) return false;Ƒ=Χ.Substring(0,Α);Χ=Χ.Remove(0,Α+1);return true;}bool ΔЉ(ref String ξ,out String Ƿ,out String ǲ)
{Ƿ="";ǲ="";if(!ō) return false;String ǧ="";if(!Ы(ref ξ,out ǧ,BROADCAST_SEP)||!Βδ(ǧ)) return false;if(!Ы(ref ξ,out Ƿ,BROADCAST_SEP)) return false;if(!Ы(ref ξ,out ǧ,BROADCAST_SEP)||(ǧ!=Δν()&&(ǧ!="*"&&Ξ!=Μ.ы))) return false;if(!Ы(ref ξ,out ǧ,BROADCAST_SEP)||(ǧ!=ΓΘ)) return false;
if(!Ы(ref ξ,out ǲ,BROADCAST_SEP)) return false;return true;}void Δϋ(){bool Ɗ=true;if(Επ){Επ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ɠ=(Ξ!=Μ.ы?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɠ)
Δό(Ɠ);String[]Κ=Me.CustomData.Split('\n');ō=bool.Parse(ǉ(ƻ(Κ,"Enable_Broadcast",ref Ɗ)));bool ΔΥ=false;bool Ει=true;if(ō){if(Ξ!=Μ.ы)
Ū=ǉ(ƻ(Κ,"Ship_Name",ref Ɗ)).Replace(BROADCAST_SELF_ID,"");ΓΘ=ǉ(ƻ(Κ,"Broadcast_Channel",ref Ɗ)).ToLower();Ει=false;if(Ǚ==null){Ǚ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);Ǚ.SetMessageCallback("");}List<IMyRadioAntenna>ň=new List<IMyRadioAntenna>();ό.GetBlocksOfType(ň);bool ΒЌ=false;
for(int i=0;i<ň.Count;i++)
if(ň[i].EnableBroadcasting&&ň[i].Enabled){ΒЌ=true;break;}if(ň.Count==0) Λ="No Antenna found";else if(!ΒЌ) Λ="Antenna not ready";ΔΥ=ň.Count==0||!ΒЌ;if(ΕΜ&&!ΔΥ&&Ξ!=Μ.ы) Λ="Antenna ok";}else if(Ξ==Μ.ы)
Λ="Offline - Enable in PB custom data";ΕΜ=ΔΥ;if(Ει){if(Ǚ!=null) this.IGC.DisableBroadcastListener(Ǚ);Ǚ=null;}}catch{Ɗ=false;}if(!Ɗ){Λ="Reset custom data";Δό(Ɠ);}}void Δό(String Ɠ){String Χ=Ɠ+"\n\n"+"Enable_Broadcast="+(Ξ==Μ.ы?"true":"false")+" \n";Χ+=Ξ!=Μ.ы?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String ǉ(String Ĩ){int Α=Ĩ.IndexOf("//");if(Α!=-1) Ĩ=Ĩ.Substring(0,Α);String[]ο=Ĩ.Split('=');if(ο.Length<=1) return "";return ο[1].Trim();}String ƻ(String[]Κ,String Γι,ref bool ī){foreach(String Ĩ in Κ)
if(Ĩ.StartsWith(Γι)) return Ĩ;ī=false;return "";}void Ζϋ(string Н){if(Ȟ) Ц=new List<И>();String Ƿ="";String ǲ="";if(!Βδ(Н)) ΗΟ(Н);bool ΖΡ=false;int Γύ=50;while(Ǚ!=null&&Ǚ.HasPendingMessage&&Γύ-->0){MyIGCMessage ξ=Ǚ.AcceptMessage();String Χ=ξ.Data as String;if(Χ==null) continue;
if(ΔЉ(ref Χ,out Ƿ,out ǲ)&&Ƿ!=""&&Ƿ!=BROADCAST_SELF_ID){И Δ=ΒΣ(Ƿ,"");if(Δ==null){Δ=new И(Ƿ);Ц.Add(Δ);}Δ.ł(ǲ,false,false,true);Ημ(Δ,Χ);ΖΡ=true;}}if(ΖΡ)
Ц.Sort(delegate(И a,И b){if(a.έ==null&&b.έ==null) return 0;else if(a.έ==null) return-1;else if(b.έ==null) return 1;else return a.έ.CompareTo(b.έ);});if(ȣ||Ȟ){if(Ƣ<=0||Ȟ){Λ="";Ȟ=false;ΗΩ();Δϋ();Γξ=DateTime.Now;}}Ηε();}void Ημ(И Δ,String Χ){Δ.Γλ=DateTime.Now;String Ƒ="";
Ы(ref Χ,out Δ.ΑЏ,BROADCAST_SEP);Ы(ref Χ,out Δ.έ,BROADCAST_SEP);if(Δ.ΑЏ!=VERSION) return;String Ɲ="";String[]ţ=new string[3];Ы(ref Χ,out Ɲ,BROADCAST_SEP);Ы(ref Χ,out ţ[0],BROADCAST_SEP);Ы(ref Χ,out ţ[1],BROADCAST_SEP);Ы(ref Χ,out ţ[2],BROADCAST_SEP);Ы(ref Χ,out Δ.ΔΑ,BROADCAST_SEP);
Ы(ref Χ,out Δ.Λ,BROADCAST_SEP);Ы(ref Χ,out Ƒ,BROADCAST_SEP);Ы(ref Χ,out Δ.Αω,BROADCAST_SEP);String Ζη="",ΕЏ="";Ы(ref Χ,out Ζη,BROADCAST_SEP);Ы(ref Χ,out ΕЏ,BROADCAST_SEP);Δ.Ѕ.Clear();while(true){String Εώ;if(!Ы(ref Χ,out Εώ,BROADCAST_SEP)) break;String[]Αο=Εώ.Split('/');if(Αο.Count()<3) continue;
int Ѓ=0;if(!int.TryParse(Αο[2],out Ѓ)) continue;Δ.Ѕ.Add(new б(Αο[0],Αο[1],Ѓ,С.Ŀ));}int ΖΑ=0;if(!int.TryParse(Ƒ,out Δ.І)) Δ.І=0;if(!int.TryParse(Ɲ,out ΖΑ)) Δ.Ξ=Μ.ȳ;if(!float.TryParse(ţ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ţ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(ţ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(Ζη,out Δ.ǯ)) Δ.ǯ=0;if(!float.TryParse(ΕЏ,out Δ.ǵ)) Δ.ǵ=0;Δ.Ξ=(Μ) ΖΑ;Δ.ΓΦ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.ΔΝ=0;for(int i=0;i<Δ.Ѕ.Count();i++) Δ.ΔΝ+=Δ.Ѕ[i].Ѓ;}И ΒΣ(String ς,String έ){ς=ς.ToUpper();έ=έ.ToUpper();for(int i=0;i<Ц.Count;i++)
{if(ς!=""&&Ц[i].ς.ToUpper()==ς) return Ц[i];if(έ!=""&&Ц[i].έ!=null&&Ц[i].έ.ToUpper()==έ) return Ц[i];}return null;}void Ǘ(И Δ,String Г){if(Δ==null){for(int i=0;i<Ц.Count;i++)
Ц[i].ł(Г,true,false,false);Βό(Г,"*",Г);}else{Δ.ł(Г,true,false,false);Βό(Г,Δ.ς,Г);}}void ΗΟ(string Н){if(Н=="") return;var ο=Н.ToUpper().Split(' ');ο.DefaultIfEmpty("");var Г=ο.ElementAtOrDefault(0);var ȇ=ο.ElementAtOrDefault(1);String Ȏ="Invalid argument: "+Н;if(φ==Ρ.ƀ){if(Џ!=null)
{switch(Г){case "UP":{if(І<2) break;if(Џ.І==0){І=1;return;}Ǘ(Џ,"UP");return;}case "DOWN":{if(І<1) break;if(І==1){Ǘ(Џ,"MRES");break;}Ǘ(Џ,"DOWN");break;}case "APPLY":{if(І<2) break;Ǘ(Џ,"APPLY");return;}}}}switch(Г){case "UP":this.Βι(false);return;case "DOWN":this.Βθ(false);return;
case "APPLY":this.ĺ(true);return;}switch(Г){case "CLEAR":Ζφ();return;case "SENDALL":
if(String.IsNullOrEmpty(ȇ)){Λ="Usage: SENDALL <cmd>";return;}Ǘ(null,ȇ);return;case "SEND":
if(Н.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΗΫ(Н.Remove(0,"SEND".Length+1));return;}Λ=Ȏ;}void ΗΫ(String Ƅ){if(Ƅ=="") return;var ο=Ƅ.Split(':');if(ο.Length!=2){Λ="Missing separator \":\"";return;}ο.DefaultIfEmpty("");String Ū=ο.ElementAtOrDefault(0).Trim();
И Δ=ΒΣ("",Ū);if(Δ!=null) Ǘ(Δ,ο.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ū;}void Ζφ(){Ц.Clear();Џ=null;ε(Ρ.Main);І=0;}void ΗΩ(){List<IMyTerminalBlock>ƨ=new List<IMyTerminalBlock>();ό.GetBlocksOfType(П,њ);ό.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ƨ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);П=Ǖ(П,pamTag,true);ΔБ(ƨ);ΔА();ę(П,setLCDFontAndSize,1.15f,false);ę(Ǻ,setLCDFontAndSize,1.15f,true);String ΘΚ=Δς(false);String ΖΖ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock о in П){if(о.CustomData==""){о.CustomData=Δς(true);continue;}
if(!о.CustomData.Contains(ΘΚ))
if(!о.CustomData.Contains(ΖΖ)) о.CustomData=ΖΖ+о.CustomData;}}void Ηε(){String Ə="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ǻ.Count;i++)
Ǻ[i].WriteText(ΔΪ(М.ΔЃ,"0",false));for(int i=0;i<П.Count;i++){М Ї=М.ȳ;String Ǥ="";ΗΝ(П[i],out Ї,out Ǥ);П[i].WriteText(ΔΪ(Ї,Ǥ,i==0));}Echo(Ə);for(int i=0;i<Ģ.Count;i++){IMyTextPanel о=Ģ[i];String Χ=о.CustomData.ToUpper();if(Χ=="DEBUG") о.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") о.WriteText(Δρ());
}}String ĺ(bool Θ){int ΗА=0;return ĺ(Θ,0,ref ΗА,false,1);}String ĺ(bool Θ,int Θε,ref int Θσ,bool Εέ,int Ɵ){String Γ="";String ũ="——————————————————\n";String ΘЁ="--------------------------------------------\n";if(Џ==null||φ==Ρ.Main||φ==Ρ.Βύ)
Γ+="[PAM]-Controller | "+Ц.Count+" Connected ships"+"\n";else Γ+=Δτ(Џ)+"\n";Γ+=ũ;int Α=0;int Η=І;Ī=0;if(Λ!=""){Ι(ref Γ,Η,Α++,Θ,Λ);}else if(φ==Ρ.Main){if(Џ==null&&Ц.Count>=1) Џ=Ц[0];String Е="";if(Ι(ref Е,Η,Α++,Θ," Send command to all")) ε(Ρ.Βύ);int Ũ=0;
for(int s=0;s<Ц.Count;s++){if(Εέ) Е+="\n";И Δ=Ц[s];if(Η==Α||Η==Α+1) Џ=Δ;if(Α==Η) Ũ=Е.Split('\n').Length-1;if(Ι(ref Е,Η,Α++,Θ," "+Δτ(Δ))){Џ=Δ;ε(Ρ.ƀ);Џ.ł("",false,true,false);}if(Α==Η) Ũ=Е.Split('\n').Length-1;if(Ι(ref Е,Η,Α++,Θ," "+(Εέ?"":"  ")+ΖЏ(Δ)))
{Џ=Δ;ε(Ρ.ΕΑ);}}int ű=Ɵ-2;ű+=Math.Max(0,(Θε-1))*Ɵ;Γ+=Βϋ(ű,Е,Ũ,ref Θσ);}else if(φ==Ρ.ΑΒ||φ==Ρ.Βύ){Ρ Ηψ=φ==Ρ.ΑΒ?Ρ.ƀ:Ρ.Main;И μ=null;if(φ==Ρ.ΑΒ) μ=Џ;if(Ι(ref Γ,Η,Α++,Θ," Back")){ε(Ηψ);}if(Ι(ref Γ,Η,Α++,Θ," [Stop] "+ľ(μ,"STOP"))){Ǘ(μ,"STOP");}if(Ι(ref Γ,Η,Α++,Θ," [Continue] "+ľ(μ,"CONT"))){Ǘ(μ,"CONT");}
if(Ι(ref Γ,Η,Α++,Θ," [Move home] "+ľ(μ,"HOMEPOS"))){Ǘ(μ,"HOMEPOS");}if(Ι(ref Γ,Η,Α++,Θ," [Move to job] "+ľ(μ,"JOBPOS"))){Ǘ(μ,"JOBPOS");}if(Ι(ref Γ,Η,Α++,Θ," [Full simulation] "+ľ(μ,"FULL"))){Ǘ(μ,"FULL");}if(μ!=null&&μ.Ξ==Μ.ή)
if(Ι(ref Γ,Η,Α++,Θ," [Undock] "+ľ(μ,"UNDOCK"))){Ǘ(μ,"UNDOCK");}}else if(φ==Ρ.ƀ){String ΓΒ="";if(Ι(ref ΓΒ,Η,Α++,Θ," Back")){ε(Ρ.Main);}Γ+=ΓΒ.Substring(0,ΓΒ.Length-1).PadRight(36,' ');if(Ι(ref Γ,Η,Α++,Θ," Send cmd")){ε(Ρ.ΑΒ);}if(!Џ.ΔЁ()&&!Џ.Ββ()) Ī++;if(Џ.Ββ()) Γ+="No answer received...";
else Γ+=ń(Џ,Џ.Αω);}else if(φ==Ρ.ΕΑ){if(Ι(ref Γ,Η,Α++,Θ," Back")){ε(Ρ.Main);}Γ+=ΘЁ;Γ+=ń(Џ,Δσ(Џ));}if(!Γό) І=Math.Min(Ī-1,І);return Γ;}String Δτ(И Δ){if(Δ.ΑЏ!=VERSION) return Δ.έ+": Different version!";return Δ.έ+": "+ń(Δ,Δ.ΔΑ)+" "+Δ.ΓΦ+"m";}String ΖЏ(И Δ){String s=Βω("",Ќ(Δ.ǯ,Δ.ǵ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ѕ.Count;i++){if(i>=5) break;б χ=Δ.Ѕ[i];s+=ΕΗ(ΒΘ(ΖЋ(χ)),3)+" "+Δλ(χ.Ѓ)+" ";}return s;}String ΖЋ(б χ){if(χ.π.ToUpper()=="ORE"||χ.π.ToUpper()=="INGOT"){String ΕΫ=ΖЉ(χ.έ.ToUpper());if(ΕΫ!="") return ΕΫ;}return χ.έ;}String ń(И Δ,String Γ){if(Δ.ΔЁ()) return "No signal...("+ƾ((int)(DateTime.Now-Δ.Γλ).TotalSeconds)+")";
return Γ;}String Δς(bool ΙΕ){String Γ="";if(ΙΕ) Γ+="mode=main:1\n\n";Γ+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Γ;}void ΗΝ(IMyTerminalBlock о,out М Ї,out String Ǥ){bool ī=true;String Ɲ=ǉ(ƻ(о.CustomData.Split('\n'),"mode",ref ī)).ToUpper();String[]ο=Ɲ.Split(':');String Ɯ=ο.Length>0?ο[0].Trim():"";Ǥ=ο.Length>1?ο[1].Trim():"";
Ї=М.ȳ;if(Ɯ=="MAIN") Ї=М.ΔЄ;else if(Ɯ=="MAINX") Ї=М.ΔЅ;else if(Ɯ=="MENU") Ї=М.Βη;else if(Ɯ=="INVENTORY") Ї=М.Βα;else if(Ɯ=="DEBUG") Ї=М.ΒΝ;if(Ї==М.ȳ) Ǥ=Ɲ;}String ΔΪ(М Ї,String Ǥ,bool ΗБ){int ƚ=15;if(ΗБ){Εί=ΑΤ;ΑΤ=0;Εΰ=ΑΥ;ΑΥ=0;}String Γ="";if(Ї==М.ΔЄ){int н=0;
Γ=ĺ(false,Εί,ref ΗЃ,true,ƚ);if(!int.TryParse(Ǥ,out н)) return Γ;ΑΤ=Math.Max(н,ΑΤ);н--;return ΔЈ(н,ƚ,Γ);}else if(Ї==М.ΔЅ){int н=0;Γ=ĺ(false,Εΰ,ref ΗЄ,false,ƚ);if(!int.TryParse(Ǥ,out н)) return Γ;ΑΥ=Math.Max(н,ΑΥ);н--;return ΔЈ(н,ƚ,Γ);}if(Ї==М.ΔЃ){return ĺ(false,0,ref ΗЅ,true,ƚ);
}else if(Ї==М.Βα||Ї==М.Βη||Ї==М.ΒΝ){И Δ=Џ;if(Ǥ!=""){Δ=ΒΣ("",Ǥ);if(Δ==null) return "Unknown ship: "+Ǥ;}else{if(Δ==null) return "No ship on main screen selected.";Γ="Selected: ";}String ũ="—————————————————————\n";String ΔΛ="";String ΓΗ="";if(Ї==М.Βη){ΔΛ="Menu";ΓΗ=ń(Δ,Δ.Αω);}
else if(Ї==М.Βα){ΔΛ="Inventory";ΓΗ=ń(Δ,Δσ(Δ));}else if(Ї==М.ΒΝ){return ń(Δ,ΖА(Δ));}Γ+=Δ.έ+" - "+Δ.ΓΦ+"m | "+ΒΘ(""+ΔΛ)+"\n"+ũ;Γ+=ΓΗ;return Γ;}return "Unknown command: "+Ǥ;}string ΖА(И Δ){String s="";s+=Δ.ΑЏ+"\n";s+=Δ.έ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΔΑ+"\n";s+=Δ.Λ+"\n";s+=Δ.І+"\n";s+=Δ.Αω.Length+"\n";s+=Δ.ǯ+"\n";s+=Δ.ǵ+"\n";s+=Δ.Ѕ.Count()+"\n";return s;}string Δσ(И Δ){String s="";s+=Βω("All",Ќ(Δ.ǯ,Δ.ǵ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ѕ.Count();i++){б χ=Δ.Ѕ[i];
s+=Βω(χ.έ,χ.Ѓ,Δ.ΔΝ,30,8,10)+"\n";}return s;}Л ΖІ(И Δ,bool ȋ,string Г){Л ά=Л.None;if(Δ==null){for(int i=0;i<Ц.Count;i++){Л ώ=Ц[i].ł(Г,false,ȋ,false);if(ώ==Л.Βϊ) ά=ώ;if(ώ==Л.Βν) return ώ;if(ώ==Л.ž) return ώ;}return ά;}else return Δ.ł(Г,false,ȋ,false);}String ľ(И Δ,String Г)
{Л ώ=ΖІ(Δ,false,Г);if(ώ==Л.Βϊ) return "received!";if(ώ==Л.ž) return "pending...";if(ώ==Л.Βν) return "no answer!";return "";}String Βϋ(int ű,String Γ,int Ũ,ref int ȩ){String[]Κ=Γ.Split('\n');if(Ũ>=ȩ+ű-1) ȩ++;ȩ=Math.Min(Κ.Count()-1-ű,ȩ);if(Ũ<ȩ+1) ȩ--;ȩ=Math.Max(0,ȩ);
String ά="";for(int i=0;i<ű;i++){int ΕІ=i+ȩ;if(ΕІ>=Κ.Count()) break;ά+=Κ[ΕІ]+"\n";}return ά;}String ΔЈ(int н,int Ɵ,String Γ){String[]Κ=Γ.Split('\n');int ΓБ=н*Ɵ;int ő=(н+1)*Ɵ;String ά="";for(int i=ΓБ;i<ő;i++){if(i>=Κ.Count()) break;ά+=Κ[i]+"\n";}return ά;
}String ΒΘ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΕΗ(String s,int ΕЍ){if(s=="") return s;if(s.Length>ΕЍ) s=s.Substring(0,ΕЍ-1)+".";return s;}string Βω(String ħ,float value,float Γχ,int ΕΤ,int ΕЁ,int Θδ){float Ĭ=Ќ(value,Γχ)*ΕΤ;
String ΑΡ="[";for(int i=0;i<ΕΤ;i++){if(i<=Ĭ) ΑΡ+="|";else ΑΡ+="'";}ΑΡ+="]";return ΑΡ+" "+ΕΗ(ΒΘ(ħ),ΕЁ).PadRight(ΕЁ)+"".PadRight(Θδ)+Δλ(value);}String Δλ(float Ѓ){if(Ѓ>=1000000) return Math.Round(Ѓ/1000000f,Ѓ/1000000f<100?1:0)+"M";if(Ѓ>=1000) return Math.Round(Ѓ/1000f,Ѓ/1000f<100?1:0)+"K";
return ""+Math.Round(Ѓ);}String ƾ(int ƣ){if(ƣ>=60*60) return Math.Round(ƣ/(60f*60f),1)+" h";if(ƣ>=60) return Math.Round(ƣ/60f,1)+" min";return ""+ƣ+" s";}string Ľ(float v){return Math.Round(v,2)+" ";}string ΙΜ(Vector3 v){return "X"+Ľ(v.X)+"Y"+Ľ(v.Y)+"Z"+Ľ(v.Z);}
List<IMyCameraBlock>ȉ=new List<IMyCameraBlock>();List<long>Š=new List<long>();bool Ţ=false;int ΓΝ=0;int ΓΛ=0;DateTime ΕΧ=DateTime.Now;String ΓΖ="";void ΗΨ(){ȉ.Clear();Š.Clear();if(!obstacleBrake) return;ό.GetBlocksOfType(ȉ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<ȉ.Count;i++){ȉ[i].EnableRaycast=true;if(!Š.Contains(ȉ[i].CubeGrid.EntityId)) Š.Add(ȉ[i].CubeGrid.EntityId);}if(!Š.Contains(Me.CubeGrid.EntityId)) Š.Add(Me.CubeGrid.EntityId);}bool ΔϏ(Β s){return s==Β.ȕ||s==Β.й||s==Β.а||
s==Β.ȱ||s==Β.ļ||s==Β.Ŷ||s==Β.Ƽ||
(s==Β.Ǿ&&Ƕ!=null);}void Ʒ(){Ţ=false;ΓΝ=0;ΓΖ="";}void Ηκ(){if(!obstacleBrake||ȉ.Count==0||!ΔϏ(Ω)){Ʒ();return;}Vector3 ƒ=Ѐ+ΑЋ;Vector3 ǻ=ƒ-ζ;float ǹ=ǻ.Length();if(ǹ<1f){Ʒ();return;}Vector3 ν=ǻ/ǹ;float ΕЊ=Ȫ*obstacleLookAhead+λ;Vector3 Θλ=ƒ+ν*(λ*0.25f);
for(int n=0;n<ȉ.Count;n++){ΓΛ=(ΓΛ+1)%ȉ.Count;IMyCameraBlock Ɖ=ȉ[ΓΛ];if(!Ɖ.IsFunctional||!Ɖ.IsWorking) continue;Vector3 ΕΩ=Ɖ.GetPosition();Vector3 ő=Θλ;if(Vector3.Distance(ΕΩ,ő)>ΕЊ) ő=ΕΩ+ν*ΕЊ;if(!Ɖ.CanScan(ő)) continue;MyDetectedEntityInfo Ɣ=Ɖ.Raycast(ő);bool Θα=!Ɣ.IsEmpty()&&!Š.Contains(Ɣ.EntityId)&&!ΗΙ(Ɣ.EntityId)&&
Ɣ.Type!=MyDetectedEntityType.FloatingObject;if(Θα){if(!Ţ) ΕΧ=DateTime.Now;Ţ=true;ΓΝ=0;ΓΖ=Ɣ.Type.ToString();}else if(Ţ&&++ΓΝ>=3) Ʒ();break;}if(Ţ&&(DateTime.Now-ΕΧ).TotalSeconds>obstacleTimeout)
Λ="Path blocked: "+ΓΖ;}const String ΕΖ="[PAMTFC]";const float Ηΰ=5f;const float ΖЄ=2f;class Ņ{public long ς;public Vector3 ι,ΔΤ;public float Εθ;public String ř="";public int Ъ=-1;public long ί=0;public int Αέ=0;public DateTime ΔΘ;}IMyBroadcastListener ΑЊ=null;long Αύ=0;List<Ņ>Я=new List<Ņ>();
Vector3 ΑЋ=new Vector3();float Ů=-1;int Ζδ=0;String ѐ="";int ǣ=-1;bool ΑΧ=false;Vector3 Εα=new Vector3();String Βε(){if(α==Π.у||Ξ==Μ.ή) return "";return Math.Round(Ζ.Ε.X)+"|"+Math.Round(Ζ.Ε.Y)+"|"+Math.Round(Ζ.Ε.Z)+
"|"+Ў+"|"+О+"|"+(int) Ƙ+"|"+Math.Round(Ȓ,1)+"|"+Math.Round(ǽ,1);}void Ηξ(){if(!trafficEnabled) return;if(ΑЊ==null) ΑЊ=IGC.RegisterBroadcastListener(ΕΖ);Αύ=Me.CubeGrid.EntityId;int Ηό=30;while(ΑЊ.HasPendingMessage&&Ηό-->0){String Χ=ΑЊ.AcceptMessage().Data as String;
if(Χ!=null) ΗΞ(Χ);}for(int i=Я.Count-1;i>=0;i--)
if((DateTime.Now-Я[i].ΔΘ).TotalSeconds>Ηΰ) Я.RemoveAt(i);if(Ω==Β.т||++Ζδ<3) return;Ζδ=0;bool ΘΝ=Ω==Β.ȕ||Ω==Β.ŀ||Ω==Β.Ș;IGC.SendBroadcastMessage(ΕΖ,Αύ+";"+ϋ(ζ)+";"+
ϋ(Ψ.GetShipVelocities().LinearVelocity)+";"+λ+";"+Βε()+";"+
(ΘΝ?ǣ:-1)+";"+ѐ+";"+Ζυ()+","+Δζ());}void ΗΞ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long ς=long.Parse(f[0]);if(ς==Αύ) return;Ņ Δ=null;for(int i=0;i<Я.Count;i++) if(Я[i].ς==ς) Δ=Я[i];if(Δ==null){Δ=new Ņ();Δ.ς=ς;Я.Add(Δ);}Δ.ι=ϊ(f[1]);
Δ.ΔΤ=ϊ(f[2]);Δ.Εθ=float.Parse(f[3]);Δ.ř=f[4];Δ.Ъ=int.Parse(f[5]);Δ.ΔΘ=DateTime.Now;if(Δ.ř!=""&&Δ.ř==Βε()) ΗΛ(f[6]);Δ.ί=0;Δ.Αέ=0;if(f.Length>=8){String[]d=f[7].Split(',');Δ.ί=long.Parse(d[0]);Δ.Αέ=int.Parse(d[1]);}}catch{}}bool ΗΙ(long ς){for(int i=0;i<Я.Count;i++) if(Я[i].ς==ς) return true;
return false;}Vector3 ΖЌ(int Α,int Θζ){if(laneOffset<=0||Φ.Count<2||Α<0||Α>=Φ.Count) return new Vector3();Ϊ Ϋ=Φ[Α];Vector3 ΘЎ=(Φ[Math.Min(Φ.Count-1,Α+1)].Ε-Φ[Math.Max(0,Α-1)].Ε)*Θζ;Vector3 В=Ϋ.ρ.LengthSquared()>0.01f?-Vector3.Normalize(Ϋ.ρ):-Ϋ.β;Vector3 і=Vector3.Cross(ΘЎ,В);
if(і.LengthSquared()<0.0001f) return new Vector3();float Є=laneOffset*λ;float ΘΒ=Math.Min(Vector3.Distance(Ϋ.Ε,Φ[0].Ε),
Vector3.Distance(Ϋ.Ε,Φ[Φ.Count-1].Ε));return Vector3.Normalize(і)*Є*Math.Min(1,ΘΒ/(Є*4));}void Ηο(){ΑЋ=new Vector3();Ů=-1;if(!trafficEnabled||Я.Count==0||!ΔϏ(Ω)) return;Ζχ(Ψ.GetNaturalGravity());}void Ζχ(Vector3 ρ){Vector3 ǻ=Ѐ-ζ;float ǹ=ǻ.Length();if(ǹ<1f) return;Vector3 ν=ǻ/ǹ;
Vector3 В=ρ.LengthSquared()>0.01f?-Vector3.Normalize(ρ):-ƍ;Vector3 і=Vector3.Cross(ν,В);і=і.LengthSquared()<0.0001f?-ƙ:Vector3.Normalize(і);for(int i=0;i<Я.Count;i++){Ņ Њ=Я[i];Vector3 ι=Њ.ι+Њ.ΔΤ*(float)(DateTime.Now-Њ.ΔΘ).TotalSeconds;Vector3 ΖΞ=ι-ζ;float ΑΞ=Vector3.Dot(ΖΞ,ν);
if(ΑΞ<=0) continue;float ȋ=(λ+Њ.Εθ)*0.5f;if((ΖΞ-ν*ΑΞ).Length()>ȋ*1.2f) continue;float ΓЁ=Vector3.Dot(Њ.ΔΤ,ν);if(ΓЁ<-1f){if(ΑΞ<(Ȫ-ΓЁ)*6f+ȋ*4f) ΑЋ=і*ȋ*1.3f;}else{float ΘΔ=ΑΞ-ȋ-followGap*λ;float ΕЅ=Math.Max(0,ΓЁ)+Math.Max(0,ΘΔ)/ΖЄ;if(Ů<0||ΕЅ<Ů) Ů=ΕЅ;
}}}bool Δθ(){if(ѐ.Length==Math.Max(0,О)) return false;ѐ=new String('0',Math.Max(0,О));ǣ=-1;return true;}void ΗΛ(String Њ){Δθ();if(Њ.Length!=ѐ.Length) return;char[]Ť=ѐ.ToCharArray();for(int i=0;i<Ť.Length;i++) if(Њ[i]=='1') Ť[i]='1';ѐ=new String(Ť);}bool ΔЀ(int Ъ){return Ъ>=0&&Ъ<ѐ.Length&&ѐ[Ъ]=='1';}
void ΔЏ(int Ъ){if(Ъ<0||Ъ>=ѐ.Length) return;char[]Ť=ѐ.ToCharArray();Ť[Ъ]='1';ѐ=new String(Ť);}bool ΗΘ(int Ъ){String Γι=Βε();for(int i=0;i<Я.Count;i++)
if(Я[i].ř==Γι&&Я[i].Ъ>=0&&Math.Abs(Я[i].Ъ-Ъ)<=1) return true;return false;}int Ζτ(int Α){if(Ў<1||О<1) return Α;if(Δθ()) for(int r=0;r<Α/Ў&&r<О;r++) ΔЏ(r);if(ǣ>=0&&Α>=(ǣ+1)*Ў){ΔЏ(ǣ);ǣ=-1;}int Ъ=Α/Ў;if(Ъ==ǣ&&!ΔЀ(Ъ)) return Α;bool ΕΝ=false;for(int r=0;r<О;r++)
{if(ΔЀ(r)) continue;ΕΝ=true;if(trafficEnabled&&ΗΘ(r)) continue;ǣ=r;return r==Ъ?Α:r*Ў;}return ΕΝ?-1:Ў*О;}class Ȳ{public long ς;public Vector3 ι,Ю,В;}List<Ȳ>д=new List<Ȳ>();bool ŏ=false;Vector3 Αί,Αή,Αΰ,Αα;long Ƞ=0;long č=0;Ϊ Ƕ=null;bool Αβ=false;Ϊ Δη(Vector3 ι,Vector3 Ю,Vector3 В)
{MatrixD m=MatrixD.CreateWorld(ι,Ю,В);Ϊ w=new Ϊ(Ν);w.γ=Vector3D.TransformNormal(Αί,m);w.β=Vector3D.TransformNormal(Αή,m);w.Ϗ=Vector3D.TransformNormal(Αΰ,m);w.Ε=ι+(Vector3) Vector3D.TransformNormal(Αα,m);w.δ=true;return w;}int Δζ(){Β s=Ω;if(s==Β.Ļ||s==Β.Ȇ||s==Β.ĕ||s==Β.ΑΚ||
s==Β.ƺ||s==Β.ț) return 3;if(s==Β.Ǿ||s==Β.ĸ||s==Β.ǋ) return 2;if(s==Β.а) return 1;return 0;}long Ζυ(){return Δζ()==0?0:(Ƕ!=null?č:Ƞ);}void ΔЂ(){if(Ξ==Μ.ή||!Ν.δ) return;IMyShipConnector Х=и(MyShipConnectorStatus.Connected);if(Х==null||Х.OtherConnector==null) return;
IMyShipConnector Њ=Х.OtherConnector;Vector3I ΘΛ=new Vector3I((int) Ν.г.X,(int) Ν.г.Y,(int) Ν.г.Z);if(Х.Position==ΘΛ){MatrixD Ŗ=MatrixD.Transpose(Њ.WorldMatrix);Αί=Vector3D.TransformNormal(Ψ.WorldMatrix.Forward,Ŗ);Αή=Vector3D.TransformNormal(Ψ.WorldMatrix.Down,Ŗ);Αΰ=Vector3D.TransformNormal(Ψ.WorldMatrix.Left,Ŗ);
Αα=Vector3D.TransformNormal(Х.GetPosition()-Њ.GetPosition(),Ŗ);ŏ=true;}if(Vector3.Distance(Х.GetPosition(),Ν.Ε)<5) Ƞ=Њ.EntityId;List<IMyShipConnector>Ĥ=new List<IMyShipConnector>();ό.GetBlocksOfType(Ĥ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ĥ.Count>0){д.Clear();for(int i=0;i<Ĥ.Count;i++){Ȳ p=new Ȳ();p.ς=Ĥ[i].EntityId;p.ι=Ĥ[i].GetPosition();p.Ю=Ĥ[i].WorldMatrix.Forward;p.В=Ĥ[i].WorldMatrix.Up;д.Add(p);}}Ƕ=null;č=0;if(Њ.EntityId==Ƞ||!ŏ) return;for(int i=0;i<д.Count;i++)
if(д[i].ς==Њ.EntityId){č=д[i].ς;Ƕ=Δη(д[i].ι,д[i].Ю,д[i].В);}}bool Βγ(long ς,int ΖΒ){if(ς==0) return false;for(int i=0;i<Я.Count;i++){Ņ t=Я[i];if(t.ί==ς&&(t.Αέ>ΖΒ||(t.Αέ==ΖΒ&&t.ς<Αύ))) return true;}return false;}bool ΒΛ(int ΓЅ){if(Ξ==Μ.ή||!trafficEnabled){Ƕ=null;č=0;return true;}
long η=Ƕ!=null?č:Ƞ;if(!Βγ(η,ΓЅ)) return true;if(!Βγ(Ƞ,ΓЅ)){Ƕ=null;č=0;return true;}if(!ŏ) return false;Ȳ ğ=null;float ΕΦ=0;for(int i=0;i<д.Count;i++){Ȳ p=д[i];if(p.ς==Ƞ||Βγ(p.ς,ΓЅ)) continue;float d=Vector3.Distance(p.ι,Ν.Ε);if(ğ==null||d<ΕΦ){ğ=p;ΕΦ=d;}
}if(ğ==null) return false;č=ğ.ς;Ƕ=Δη(ğ.ι,ğ.Ю,ğ.В);return true;}String ΖϏ(){String s=(ŏ?"1":"0")+":"+ϋ(Αί)+":"+ϋ(Αή)+":"+
ϋ(Αΰ)+":"+ϋ(Αα)+":"+Ƞ;for(int i=0;i<д.Count;i++)
s+=":"+д[i].ς+","+ϋ(д[i].ι)+","+ϋ(д[i].Ю)+","+ϋ(д[i].В);return s;}void Ζώ(String s){д.Clear();ŏ=false;Ƞ=0;String[]f=s.Split(':');if(f.Length<6) return;ŏ=f[0]=="1";Αί=ϊ(f[1]);Αή=ϊ(f[2]);Αΰ=ϊ(f[3]);Αα=ϊ(f[4]);long.TryParse(f[5],out Ƞ);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;Ȳ p=new Ȳ();if(!long.TryParse(e[0],out p.ς)) continue;p.ι=ϊ(e[1]);p.Ю=ϊ(e[2]);p.В=ϊ(e[3]);д.Add(p);}}