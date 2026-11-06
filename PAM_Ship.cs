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
public enum Κ{Ʀ,Ŗ,ј,е,ή}public enum Β{Ъ,Ǽ,ĝ,Ȁ,Ā,ї,С,
Ș,Ĝ,Ȕ,Ō,ŉ,ȯ,
ΑЊ,ȃ,ș,ě,ΒΗ,Αη,
ΒΖ,Ƨ,Ŏ,Ƒ,Ț}public enum Θ{Ы,Ğ,Є,Αε,ƍ,Ƙ,ƙ,ė,Ę}public enum д{None,ě,Ъ,Γλ}public enum Χ{Main,ΓІ,ŕ,Ŕ,ΒΒ,Ƌ,
ΑΨ,Γψ,Γω,Γχ,ΖΕ,ΗЈ,
ΗІ,ΗЇ,ΑЎ,ΓЋ,ơ}public enum У{Ɵ,ŋ,ΖΗ,Γϋ,ή}public enum њ{ƣ,Γε}public enum Ш{ƞ,Αι,ΓЌ,Ő}public enum ϋ{Αύ,Γθ,Αβ,Αζ,Ə,Αφ,Ɣ}public enum у{Αγ,Αέ,Αή}public enum В{ő,Εϋ,Ȱ}public enum δ{ΑϏ,ΑЃ,ΑЄ,ΓЃ,ΓЂ,
Αώ,Ɯ,ƛ,ΑЁ,
ΑЂ,ΑЀ}public enum ά{ǩ,ǿ,Ǩ,Ɗ,ΑΫ,Αά}public enum Ȃ{Αμ,ΑΪ,Εμ}public enum ж{Ƣ,Γν,ΕІ,Αό}public enum Ǫ{None,ΑЈ,ΕЉ,ΕЅ}public enum ο{ś,Ɖ,Ɩ,Ő,Αϊ,Ɨ,Αϋ,ΖΛ,ΑЅ}public enum ΗЀ{Ʀ,ΗЂ,ΗЁ,ΗЄ,Ηώ,Ηύ,ΗЃ}class Ω{public bool β=false;public Vector3 Δ=new Vector3();
public Vector3 ΰ=new Vector3();public Vector3 α=new Vector3();public Vector3 τ=new Vector3();public Vector3 ι=new Vector3();public Vector3 з=new Vector3();public float Ќ=0;public float ΘΗ=0;public float[]Ǘ=null;public Ω(){}public Ω(Ω Ж){β=Ж.β;Δ=Ж.Δ;ΰ=Ж.ΰ;α=Ж.α;τ=Ж.τ;ι=Ж.ι;
з=Ж.з;Ǘ=Ж.Ǘ;}public Ω(Vector3 Δ,Vector3 α,Vector3 ΰ,Vector3 τ,Vector3 ι){this.Δ=Δ;this.ΰ=ΰ;this.α=α;this.τ=τ;this.Ќ=0;this.ι=ι;}public void Εέ(List<IMyThrust>ƪ,List<string>Ƿ){Ǘ=new float[Ƿ.Count];for(int i=0;i<Ǘ.Length;i++)
Ǘ[i]=-1;for(int i=0;i<ƪ.Count;i++){string т=Γυ(ƪ[i]);int Α=Ƿ.IndexOf(т);if(Α!=-1)
Ǘ[Α]=Љ(ƪ[i].MaxEffectiveThrust,ƪ[i].MaxThrust);}}}class ΑЇ{public Ω И=null;public List<Vector3>ł=new List<Vector3>();public float Ť=0;public float ΓΨ=0;public float Ĥ=0;public float Βζ=0;public Vector3 Ʒ=new Vector3();}class Ġ{public δ Ј=δ.ΑϏ;public float ў=0;public float ȟ=0;
public string џ="";public string Ǐ="";DateTime Δσ;public bool ΒЏ=false;private bool Βέ=false;public bool Αΰ(bool ǔ){if(ǔ){ȟ=0;Βέ=false;return false;}Βέ=true;return ȟ>ў;}public void ΕЊ(){Αΰ(true);ΒЏ=false;}public void ΖΦ(){if(Βέ)
if((DateTime.Now-Δσ).TotalSeconds>1){ȟ++;Δσ=DateTime.Now;}}public bool Ατ(){switch(Ј){case δ.ΑЃ:return true;case δ.ΑЄ:return true;}return false;}}class ф{public String ρ="";public String ν="";public int ћ=0;public В ĳ=В.Ȱ;public ф(String ρ,String ν,int ћ,В ĳ){this.ρ=ρ;this.ν=ν;
this.ћ=ћ;this.ĳ=ĳ;}}class Ει{public Ει(Vector3 ϊ,float χ){this.ϊ=ϊ;this.χ=χ;}public Vector3 ϊ;public float χ;}class ΑЍ{public DateTime ΖЅ;public DateTime ΔΠ;public String щ="";public String ρ="";public String ΘΙ="";public String Ξ="";public String ΘΕ="";public String ΘΒ="";public String ΒЎ="";
public Vector3 Δ=new Vector3();public List<ф>Н=new List<ф>();public Κ Ν=Κ.Ʀ;public Ǫ Ȝ;public float Ķ;public float ĩ;public int ь;public int ΗБ=0;public int ΘΖ=0;public bool ΗϏ(){return(DateTime.Now-ΖЅ).TotalSeconds>10;}public bool Εώ(){if(Ȝ!=Ǫ.ΑЈ) return false;return(DateTime.Now-ΔΠ).TotalSeconds>=4;
}public ΑЍ(String щ){this.щ=щ;}public Ǫ ΗЅ(String Ĉ,bool ΗΧ,bool ć,bool Ζε){if(Ĉ==""&&!ć) return Ǫ.None;if(Ȝ==Ǫ.ΑЈ&&Εώ())
Ȝ=Ǫ.ΕЅ;if(ΗΧ){ΒЎ=Ĉ;Ȝ=Ǫ.ΑЈ;ΔΠ=DateTime.Now;return Ȝ;}else if(ć){ΒЎ="";Ȝ=Ǫ.None;}else if(ΒЎ==Ĉ){if(Ζε) Ȝ=Ǫ.ΕЉ;return Ȝ;}return Ǫ.None;}}IMyGridTerminalSystem ψ;Vector3 ΑΖ=new Vector3();Vector3 γ=new Vector3();Vector3 ƺ=new Vector3();Vector3 Ų=new Vector3();Vector3 Ŧ=new Vector3();
DateTime Δς=new DateTime();bool ũ=true;int ΓΟ=0;bool ȧ=false;bool ǂ=false;bool ŵ=true;bool ΕΩ=false;bool İ=false;bool ƿ=false;float Ȑ=0;float đ=0;int ȫ=0;int Ƅ=0;int ΑΙ=0;int ΒΦ=0;float ĵ=0;float ĥ=0;double ΒЃ=0;float Ş=0;List<int>Į=new List<int>();List<int>Ľ=new List<int>();
Κ Ν=Κ.Ʀ;int ό=0;float ǹ=0;float ǧ=0;float η=0;IMyRemoteControl Υ;IMySensorBlock κ;List<IMyTimerBlock>ΓΣ=new List<IMyTimerBlock>();List<IMyShipConnector>и=new List<IMyShipConnector>();List<IMyThrust>Ǹ=new List<IMyThrust>();List<IMyGyro>ƻ=new List<IMyGyro>();List<IMyTerminalBlock>ξ=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ƕ=new List<IMyLandingGear>();List<IMyReactor>Ȫ=new List<IMyReactor>();List<IMyConveyorSorter>ΑΜ=new List<IMyConveyorSorter>();List<IMyGasTank>ш=new List<IMyGasTank>();List<IMyTerminalBlock>Ŝ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ĩ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ą=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ț=new List<IMyBatteryBlock>();List<IMyTextPanel>ъ=new List<IMyTextPanel>();List<IMyTextSurface>Ȭ=new List<IMyTextSurface>();List<IMyTextPanel>ţ=new List<IMyTextPanel>();IMyTerminalBlock ю=null;Ω Ι=new Ω();Ω Βπ=new Ω();
Ω Βσ=new Ω();bool Ǻ=false;bool Ņ=false;bool ņ=false;bool ǁ=false;List<String>Ƿ=new List<string>();List<Ω>Τ=new List<Ω>();List<Ω>ƈ=new List<Ω>();int Ββ=0;double ΕΪ=0;Ω Ε=new Ω();Vector3 ǒ;Vector3 ȍ;Θ έ=Θ.Ы;њ Ű=њ.ƣ;int Ϗ=0;int Ћ=0;double ů=0;bool Ƴ=false;Ġ υ=new Ġ();
Ġ φ=new Ġ();Ш б=Ш.ƞ;bool ȭ=true;њ ǣ=њ.ƣ;int Ю=3;int Э=3;int М=30;у й=у.Αγ;bool ȓ=true;bool į=true;bool Ī=true;bool Ē=false;ϋ Д=ϋ.Αύ;bool Ȓ=true;float Ǣ=90;float м=20;float О=5;float н=20;float ǜ=0.70f;float ň=1.50f;float Ň=2.50f;float Ė=0f;float Ȣ=0f;Β Ψ;
Θ л;Β ž;ΑЇ ζ=null;int σ=0;int π=0;int Й=0;int ȋ=0;int Đ=30;int э=0;bool Ѓ=true;Vector3 о;double Г=0;bool ΑΘ=false;int Γέ=0;int Ѐ=0;int ΒΞ=0;int ȩ=0;int ΔІ=0;Vector3 ŷ=new Vector3();float Я=0;float П=0;int ĕ=0;Ω ΑΝ=null;float Βχ=0;float ƭ=0;float К=0;float ď=0;
bool Βρ=false;bool ē=false;bool ƅ=false;bool Ȉ=false;bool Ȅ=false;DateTime Ζσ=new DateTime();Ω ĉ=null;Ω ΔΤ=null;float ǰ=0;д ǝ;float ȉ=0;float Ĕ=0;String Βώ="";float ǃ=0;float ĩ=0;float Ķ=0;List<ф>ǎ=new List<ф>();float Е=0;bool ż=false;bool ΕΟ=false;bool ΔБ=false;
bool ΑΟ=false;bool ΕΑ=false;float ΒЁ=0;float ΒЍ=0;Vector3 Δδ=new Vector3();Vector3 ω=new Vector3();Vector3 ΑΠ=new Vector3(1,1,1);float ΓΞ=1;Vector3 Ɓ=new Vector3();bool ǀ=false;bool Βμ=false;bool Βϋ=false;bool ΔΒ=false;float Ʊ=0;float Ʃ=2;Vector3 ΕΞ;Vector3 ΓΝ;Vector3 ΓΜ;
float[,]ǘ=new float[3,2];Dictionary<String,float[,]>ѓ=new Dictionary<string,float[,]>();Dictionary<String,float>ΕΧ=new Dictionary<String,float>();int ΒΡ=0;Dictionary<String,float[]>Ȏ=new Dictionary<string,float[]>();float ΒА;bool ƴ=false;int Ƭ=0;int Ȥ=0;int ƽ=0;int Č=0;
String Ξ="";int[]ΔϏ=new int[Enum.GetValues(typeof(Χ)).Length];bool ΔЀ=false;int ǌ=0;int ь=0;Χ Њ=Χ.Main;int Ηε=0,Ηζ=0,Ηη=0,Ηθ=0;int ΗΣ=0,ΗΤ=0;bool ħ=false;bool ΔΓ=false;String ΓΔ="";String ΒΥ="";IMyBroadcastListener ȅ=null;bool Δγ=true;List<ΑЍ>ΗЉ=null;ΑЍ ΘΓ=null;
int ΗЊ=0,ΗЋ=0;int ΗЌ=0,ΗЍ=0;int ΗЎ=0,ΗЏ=0,ΗА=0;String ΖЄ="";Exception Ȋ=null;bool ΑΗ=false;Θ Ŀ=Θ.Ы;bool ΔΧ=false;bool Δρ=true;bool ΓΓ=false;String ΕΨ="";bool ƃ=false;bool ΓΙ=true;int ƾ=0;int[]ё=null;Vector3 ΒΧ=new Vector3();bool Δζ=false;bool љ(IMyTerminalBlock Σ)=>Σ.CubeGrid==Me.CubeGrid;
Program(){ψ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Αυ(Me,controllerTag,true))
Ν=Κ.е;λ(Χ.Main);if(Ν!=Κ.е){Ξ="Welcome to [PAM]!";ж ΓΒ=ΕЋ();if(Ν==Κ.Ʀ){List<IMyShipDrill>Δί=new List<IMyShipDrill>();List<IMyShipGrinder>Δη=new List<IMyShipGrinder>();ψ.GetBlocksOfType(Δί,q=>q.CubeGrid==Me.CubeGrid);ψ.GetBlocksOfType(Δη,q=>q.CubeGrid==Me.CubeGrid);if(Δί.Count>0)
{Ν=Κ.Ŗ;Ξ="Miner mode enabled!";}else if(Δη.Count>0){Ν=Κ.ј;Ξ="Grinder mode enabled!";}else{Ν=Κ.ή;Ǯ(У.ή);}}if(ΓΒ==ж.Ƣ) ŵ=false;if(ΓΒ==ж.Γν) Ξ="Data restore failed!";if(ΓΒ==ж.Αό) Ξ="New version, wipe data";}}void Main(string ц,UpdateType Ηπ){try{if(Ȋ!=null){if(ц.Trim().ToUpper()=="RETRY")
{Ȋ=null;ũ=true;ȫ=0;if(Ν!=Κ.е) ς();Ξ="Restarted after error";}else{Εψ();ΓЄ();return;}}ǂ=(Ηπ&UpdateType.Update10)!=0;if(ǂ) ΓΟ++;ȧ=ΓΟ>=10;if(ȧ) ΓΟ=0;if(ǂ){Ƭ++;if(Ƭ>4) Ƭ=0;ΑΙ=(int) Math.Max(0,10-(DateTime.Now-Δς).TotalSeconds);}if(ц!="") ΖЄ=ц;if(Ν!=Κ.е)
ΖΖ(ц);else
Εη(ц);ΔЀ=false;try{int ΔΥ=Runtime.CurrentInstructionCount;float Ÿ=Љ(ΔΥ,Runtime.MaxInstructionCount);if(Ÿ>0.90) Ξ="Max. instructions >90%";if(Ÿ>ĵ) ĵ=Ÿ;if(ƴ){Į.Add(ΔΥ);while(Į.Count>10) Į.RemoveAt(0);ĥ=0;for(int i=0;i<Į.Count;i++) ĥ+=Į[i];ĥ=Љ(Љ(ĥ,Į.Count),Runtime.MaxInstructionCount);
double Βψ=Runtime.LastRunTimeMs;if(ƿ&&Βψ>ΒЃ) ΒЃ=Βψ;Ľ.Add((int)(Βψ*1000f));while(Ľ.Count>10) Ľ.RemoveAt(0);Ş=0;for(int i=0;i<Ľ.Count;i++) Ş+=Ľ[i];Ş=Љ(Ş,Ľ.Count)/1000f;}}catch{ĵ=0;}}catch(Exception e){Ȋ=e;}}void ΖΖ(string ц){bool ΑΚ=false;String ΔΝ="";if(ό<=1&&!Γώ(ц))
ΓЅ(ц);int ΖА=50;while(ȅ!=null&&ȅ.HasPendingMessage&&ΖА-->0){MyIGCMessage μ=ȅ.AcceptMessage();String ί=μ.Data as String;if(ί==null) continue;String Ż="";if(ό<=1&&ΕЇ(ref ί,out Ż,out ΔΝ)&&Ż==BROADCAST_SELF_ID){ΓЅ(ί);ΑΚ=true;break;}}bool Δν=ƿ&&Ψ==Β.Ъ&&!İ&&!ΑΚ&&ȫ==0&&!ǁ;
if(ȧ&&Ψ!=Β.Ъ) ΑΚ=true;if((ǂ&&!Δν)||(ȧ&&Δν)){if(ȫ==0&&(ΑΙ<=0||ũ)){ΕΩ=ό>0;ό=0;ȫ=1;ǯ();ΕЎ();Εό();ǫ("Scan 1");}else if(ȫ==1){ȫ=2;ǯ();ΕЏ();ǫ("Scan 2");}else if(ȫ==2){ȫ=0;ǯ();ΕА();ΕБ();ǫ("Scan 3");Δς=DateTime.Now;if(ό<=1&&ŵ)
ΑΖ=Αξ(Υ,Υ.CenterOfMass);ŵ=false;if(ũ){ũ=false;bool Ητ=έ==Θ.Є;ς();if(Δρ&&resumeAfterRestart&&Ητ&&ό<=1) ΓΓ=true;}Δρ=false;if(ΕΩ&&ό==0) Ξ="Setup complete";}else{if(έ==Θ.Є&&Ν!=Κ.ή){ǯ();ΕΫ();ǫ("Inv balance");}ǯ();switch(Ƅ){case 0:ΖΞ();break;case 1:ΖΡ();break;case 2:ΖΝ();break;
case 3:ΖΩ();break;case 4:ΖΠ();break;case 5:Εΰ();break;case 6:Εί(Υ);break;}ǫ("Update: "+Ƅ);Ƅ++;if(Ƅ>6){Ƅ=0;ƿ=true;if(Ŀ!=Θ.Ы){switch(Ŀ){case Θ.ė:Ȗ();break;case Θ.Ę:Ȗ();break;case Θ.Є:Ȗ();break;case Θ.Ƙ:ƒ();break;case Θ.ƙ:Ȳ();break;}Ŀ=Θ.Ы;}}}if(!ũ){if(!ǭ(Υ,true)){Υ=null;
ŵ=true;ό=2;}if(ό>=2&&Ψ!=Β.Ъ)
ς();if(ό<=1){đ=Υ.CalculateShipMass().PhysicalMass;Ȑ=(float) Υ.GetShipSpeed();γ=ΖΜ(Υ,ΑΖ);ƺ=Υ.WorldMatrix.Forward;Ų=Υ.WorldMatrix.Left;Ŧ=Υ.WorldMatrix.Down;ΖΥ();ΖΧ();if(ΓΓ){ΓΓ=false;Ȗ();if(Ψ!=Β.Ъ) Ξ="Job resumed after restart";}if(Ψ!=Β.Ъ){İ=false;ΓЉ(false);Γγ(false);String Ηέ=Απ(Ψ)+" "+(int) Ψ;
ǯ();ΖΣ();ΒΕ(false);ǫ(Ηέ);ΖΨ();ΖΤ();ǯ();ΖΟ();ǫ("Thruster");ǯ();ΓА();ǫ("Gyroscope");}else{if(İ){if(ƕ()){Р(Ŧ,ƺ,Ų,0.25f,true);ΓА();Ξ="Aligning to planet: "+Math.Round(Ʊ-0.25f,2)+"°";if(ǀ) Ασ(true,true);}else Ασ(true,true);}}ƃ=false;}}}ǯ();ΓЄ();ǫ("Print");if(ΑΚ||ΒΦ<=0){ǯ();Εά(ΔΝ);
ǫ("Broadcast");ΒΦ=4;}else if(ȧ) ΒΦ--;}void ΓЅ(string ц){if(ц=="") return;var А=ц.ToUpper().Split(' ');А.DefaultIfEmpty("");var Ĉ=А.ElementAtOrDefault(0);var Ć=А.ElementAtOrDefault(1);var ŝ=А.ElementAtOrDefault(2);var ΔΗ=А.ElementAtOrDefault(3);String ȥ="Invalid argument: "+ц;bool ΓΦ=false;
switch(Ĉ){case "UP":this.ΓЁ(false);break;case "DOWN":this.ΓЀ(false);break;case "UPLOOP":this.ΓЁ(true);break;case "DOWNLOOP":this.ΓЀ(true);break;case "APPLY":this.Γα(true);break;case "MRES":ь=0;break;case "STOP":this.ς();break;case "PATHHOME":{this.ς();this.ΒΑ();}break;case "PATH":{this.ς();this.ΒΑ();Ι.β=true;}break;
case "START":{this.ς();ΑБ();}break;case "ALIGN":{Ασ(!İ,false);}break;case "CONT":{this.ς();this.Ȗ();}break;case "JOBPOS":{this.ς();this.Ȳ();}break;case "HOMEPOS":{this.ς();this.ƒ();}break;case "FULL":{ż=true;}break;case "RESET":{ΑΗ=true;ό=2;}break;case "EXPORT":{Εν();}break;
case "IMPORT":{Εω(Ć=="JOB");}break;default:ΓΦ=true;break;}if(Ν!=Κ.ή){switch(Ĉ){case "SHUTTLE":{Εκ();}break;case "CFGS":{if(!Εε(Ć,ŝ,ΔΗ)) Ξ=ȥ;}break;case "CFGB":{if(!Εδ(Ć,ŝ)) Ξ=ȥ;}break;case "CFGL":{if(!ȳ(ref Ǣ,true,ο.Ɩ,Ć,"")||!Εζ(ŝ))
Ξ=ȥ;}break;case "CFGE":{if(!ȳ(ref О,true,ο.Ɨ,Ć,"IG")||
!ȳ(ref м,true,ο.Αϊ,ŝ,"IG")||
!ȳ(ref н,true,ο.Αϋ,ΔΗ,"IG"))
Ξ=ȥ;}break;case "CFGA":{if(!ȳ(ref ǜ,false,ο.Ɖ,Ć,"")) Ξ=ȥ;}break;case "CFGW":{if(!ȳ(ref ň,false,ο.ś,Ć,"")||
!ȳ(ref Ň,false,ο.ś,ŝ,""))
Ξ=ȥ;}break;case "NEXT":{ΑΧ(false);}break;case "PREV":{ΑΧ(true);}break;default:if(ΓΦ) Ξ=ȥ;break;}}else{switch(Ĉ){case "UNDOCK":{ƃ=true;}break;default:if(ΓΦ) Ξ=ȥ;break;}}}String Ες(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Ν!=Κ.ή)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void Εκ(){ς();Ν=Κ.ή;Ǯ(У.ή);Ε.β=false;Ι.β=false;κ=null;ξ.Clear();έ=Θ.Ы;}void Ασ(bool Ё,bool ǟ){if(!Ё) Ξ="Aligning canceled";if(ǟ) Ξ="Aligning done";if(ǟ||!Ё){İ=false;ΓЍ();Ơ(false,0,0,0,0);return;}if(ƕ()) İ=true;}bool Εε(String Ηφ,String Ζύ,String Ζξ)
{bool Ης=έ==Θ.Є;int w,h,d;if(int.TryParse(Ηφ,out w)&&int.TryParse(Ζύ,out h)&&int.TryParse(Ζξ,out d)){this.ς();Ю=w;Э=h;М=d;ă(false);Ś(false,false);if(Ης) Ȗ();return true;}return false;}bool Εζ(String ΔΖ){if(ΔΖ=="ON"){ȓ=true;return true;}if(ΔΖ=="OFF"){ȓ=false;return true;}
return false;}bool Εδ(String Δή,String Ʋ){bool Ŷ=true;if(Δή=="HOME") ȭ=true;else if(Δή=="STOP") ȭ=false;else Ŷ=false;if(Ʋ=="HOME") б=Ш.ƞ;else if(Ʋ=="STOP") б=Ш.ΓЌ;else if(Ʋ=="JOB") б=Ш.Αι;else if(Ʋ=="IG") б=Ш.Ő;else Ŷ=false;return Ŷ;}void λ(Χ ΒЋ){ΔϏ[(int) Њ]=ь;ь=ΔϏ[(int) ΒЋ];
if(ΒЋ==Χ.ΖΕ) ь=0;Њ=ΒЋ;if(Ν!=Κ.е)
Ă(Њ==Χ.ŕ,false,0,0);ΔЀ=true;}void ΓЁ(bool Βό){if(ь>0) ь--;else if(Βό) ь=ǌ-1;}void ΓЀ(bool Βό){if(ь<ǌ-1) ь++;else if(Βό) ь=0;}String Ǯ(У μ){switch(μ){case У.Ɵ:Ξ="Job is running";break;case У.ŋ:Ξ="Connector not ready!";break;case У.ΖΗ:Ξ="Ship modified, path outdated!";break;
case У.Γϋ:Ξ="Interrupted by player!";break;case У.ή:Ξ="Shuttle mode enabled!";break;}return "";}String Εχ(њ ϊ){switch(ϊ){case њ.ƣ:return "Top-Left";case њ.Γε:return "Center";default:return "";}}String Επ(у Ŵ){switch(Ŵ){case у.Αέ:return "Auto"+(Ν==Κ.Ŗ?" (Ore)":"");case у.Αή:return "Auto (+Stone)";
case у.Αγ:return "Default";default:return "";}}String Αν(Θ Л){switch(Л){case Θ.Ы:return "No job";case Θ.Ğ:return "Job paused";case Θ.Є:return "Job active";case Θ.ė:return "Job active";case Θ.Ę:return "Job active";case Θ.Αε:return "Job done";case Θ.ƍ:return "Job changed";case Θ.Ƙ:return "Move home";
case Θ.ƙ:return "Move to job";}return "";}String Γρ(Ш Ζγ){switch(Ζγ){case Ш.ƞ:return "Return home";case Ш.Αι:return "Fly to job pos";case Ш.ΓЌ:return "Stop";case Ш.Ő:return "Ignore";}return "";}String Ερ(ϋ Ŵ){switch(Ŵ){case ϋ.Αύ:return "Off";case ϋ.Αζ:return "Drop pos (Stone) ";case ϋ.Ə:return "Drop pos (Sto.+Ice)";
case ϋ.Γθ:return "Cur. pos (Stone)";case ϋ.Αβ:return "Cur. pos (Sto.+Ice)";case ϋ.Αφ:return "In motion (Stone)";case ϋ.Ɣ:return "In motion (Sto.+Ice)";}return "";}String Γο(д Л){switch(Л){case д.None:return "No batteries";case д.ě:return "Charging";case д.Γλ:return "Discharging";}return "";}String Απ(Β Л)
{String ΘΑ=Ν==Κ.ή?"target":"job";switch(Л){case Β.Ъ:return "Idle";case Β.Ǽ:return "Flying to XY position";case Β.ĝ:return Ν==Κ.ј?"Grinding":"Mining";case Β.Ȁ:return "Returning";case Β.Ŏ:return "Flying to drop pos";case Β.Ā:return "Returning to dock";case Β.ї:return "Flying to dock area";
case Β.С:return "Flying to job area";case Β.Ș:return "Flying to path";case Β.Ĝ:return "Flying to job position";case Β.Ȕ:return "Approaching dock";case Β.Ō:return "Docking";case Β.ŉ:return "Aligning";case Β.ȯ:return "Aligning";case Β.ΑЊ:return "Retry docking";case Β.ȃ:return "Unloading";case Β.Ț:return ΕΨ;
case Β.ș:return "Undocking";case Β.ě:return "Charging batteries";case Β.ΒΗ:return "Waiting for uranium";case Β.Αη:return "Filling up hydrogen";case Β.ΒΖ:return "Waiting for ejection";case Β.Ƨ:return "Waiting for ejection";case Β.Ƒ:return "Flying to drop pos";}return "";}String Γφ(δ Ј){switch(Ј)
{case δ.ΑϏ:return "On \"Undock\" command";case δ.ΑЃ:return "On player entered cockpit";case δ.ΓЃ:return "On ship is full";case δ.ΓЂ:return "On ship is empty";case δ.ΑЄ:return "On time delay";case δ.Ɯ:return "On batteries empty(<25%)";case δ.ƛ:return "On batteries empty(=0%)";case δ.Αώ:return "On batteries full";
case δ.ΑЂ:return "On hydrogen empty(<25%)";case δ.ΑЀ:return "On hydrogen empty(=0%)";case δ.ΑЁ:return "On hydrogen full";}return "";}bool Ο(ref String Γ,int ΔЍ,int ȣ,bool Λ,String ű){ǌ+=1;if(ΔЍ==ȣ) ű=">"+ű+(Ƭ>=2?" .":"");else ű=" "+ű;Γ+=ű+"\n";return ΔЍ==ȣ&&Λ;}String Γα(bool Λ)
{int Α=0;int Μ=ь;ǌ=0;String ΔА="———————————————\n";String Ń="--------------------------------------------\n";String Γ="";Γ+=Αν(έ)+" | "+(Ι.β?"Ready to dock":"No dock")+"\n";Γ+=ΔА;double Ǥ=Math.Max(Math.Round(this.Г),0);if(Њ==Χ.Main){bool Ȧ=Ν==Κ.ή;if(Ο(ref Γ,Μ,Α++,Λ," Record path & set home")) ΒΑ();
if(Ν==Κ.Ŗ)
if(Ο(ref Γ,Μ,Α++,Λ," Setup mining job")) λ(Χ.ŕ);if(Ν==Κ.ј)
if(Ο(ref Γ,Μ,Α++,Λ," Setup grinding job")) λ(Χ.ŕ);if(Ν==Κ.ή)
if(Ο(ref Γ,Μ,Α++,Λ," Setup shuttle job")) λ(Χ.ΑЎ);if(Ο(ref Γ,Μ,Α++,Λ," Continue job")) Ȗ();if(Ο(ref Γ,Μ,Α++,Λ," Fly to home position")) ƒ();if(Ο(ref Γ,Μ,Α++,Λ," Fly to job position")) Ȳ();if(Ο(ref Γ,Μ,Α++,Λ," Behavior settings"))
if(Ȧ) λ(Χ.ơ);else λ(Χ.Ƌ);if(Ο(ref Γ,Μ,Α++,Λ," Info")) λ(Χ.Γψ);if(Ν!=Κ.ή)
if(Ο(ref Γ,Μ,Α++,Λ," Help")) λ(Χ.Γχ);}else if(Њ==Χ.ŕ){double Ηυ=Math.Round(Ю*ǹ,1);double Ζό=Math.Round(Э*ǧ,1);String ύ="";if(Ο(ref ύ,Μ,Α++,Λ," Start new job!")) ΑБ();if(Ο(ref ύ,Μ,Α++,Λ," Change current job")){Ś(false,false);λ(Χ.Main);}if(Ο(ref ύ,Μ,Α++,Λ," Width + (Width: "+Ю+" = "+Ηυ+"m)")){ę(ref Ю,5,20,1);ă(true);}
if(Ο(ref ύ,Μ,Α++,Λ," Width -")){ę(ref Ю,-5,20,-1);ă(true);}if(Ο(ref ύ,Μ,Α++,Λ," Height + (Height: "+Э+" = "+Ζό+"m)")){ę(ref Э,5,20,1);ă(true);}if(Ο(ref ύ,Μ,Α++,Λ," Height -")){ę(ref Э,-5,20,-1);ă(true);}if(Ο(ref ύ,Μ,Α++,Λ," Depth + ("+(й==у.Αγ?"Depth":"Min")+": "+М+"m)")){ę(ref М,5,50,2);ă(true);}
if(Ο(ref ύ,Μ,Α++,Λ," Depth -")){ę(ref М,-5,50,-2);ă(true);}if(Ο(ref ύ,Μ,Α++,Λ," Depth mode: "+Επ(й))){й=ȗ(й);}if(Ο(ref ύ,Μ,Α++,Λ," Start pos: "+Εχ(ǣ))){ǣ=ȗ(ǣ);}if(Ν==Κ.ј&&й==у.Αή) й=ȗ(й);Γ+=ΓЇ(8,ύ,Μ,ref ΗΣ);}else if(Њ==Χ.ΑЎ){float[]ΔΩ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ο(ref Γ,Μ,Α++,Λ," Next")) λ(Χ.ΓЋ);if(Ο(ref Γ,Μ,Α++,Λ," Back")) λ(Χ.Main);Γ+=" Leave connector 1:\n";if(Ο(ref Γ,Μ,Α++,Λ," - "+Γφ(υ.Ј))) υ.Ј=ȗ(υ.Ј);if(!υ.Ατ()) Γ+="\n";else if(Ο(ref Γ,Μ,Α++,Λ," - Delay: "+Αλ((int) υ.ў))) υ.ў=Γι(υ.ў,ΔΩ);Γ+=" Leave connector 2:\n";if(Ο(ref Γ,Μ,Α++,Λ," - "+Γφ(φ.Ј))) φ.Ј=ȗ(φ.Ј);
if(!φ.Ατ()) Γ+="\n";else if(Ο(ref Γ,Μ,Α++,Λ," - Delay: "+Αλ((int) φ.ў))) φ.ў=Γι(φ.ў,ΔΩ);}else if(Њ==Χ.ΓЋ){if(Ο(ref Γ,Μ,Α++,Λ," Start job!")) ΑБ();if(Ο(ref Γ,Μ,Α++,Λ," Back")) λ(Χ.ΑЎ);Γ+=" Timer: \"Docking connector 1\":\n";if(Ο(ref Γ,Μ,Α++,Λ," = "+(υ.џ!=""?υ.џ:"-"))) υ.џ=ƚ(ref Ηε);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Ο(ref Γ,Μ,Α++,Λ," = "+(υ.Ǐ!=""?υ.Ǐ:"-"))) υ.Ǐ=ƚ(ref Ηη);Γ+=" Timer: \"Docking connector 2\":\n";if(Ο(ref Γ,Μ,Α++,Λ," = "+(φ.џ!=""?φ.џ:"-"))) φ.џ=ƚ(ref Ηζ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Ο(ref Γ,Μ,Α++,Λ," = "+(φ.Ǐ!=""?φ.Ǐ:"-"))) φ.Ǐ=ƚ(ref Ηθ);
}else if(Њ==Χ.Ŕ){String Δψ=Ǣ+" %";if(ȧ) Ȥ++;if(Ȥ>1){Ȥ=0;ƽ++;if(ƽ>1) ƽ=0;}bool[]ΓΖ=new bool[]{Ȫ.Count==0,ǝ==д.None,ш.Count==0};int ΕΤ=0;while(true){ΕΤ++;Č++;if(Č>ΓΖ.Length-1) Č=0;if(ΕΤ>=ΓΖ.Length) break;if(!ΓΖ[Č]) break;}bool Ȧ=Ν==Κ.ή;if(!Ȧ&&ȓ&&Е!=-1&&ƽ==0)
Δψ=Е<1000000?Math.Round(Е)+" Kg":Math.Round(Е/1000)+" t";if(Ο(ref Γ,Μ,Α++,Λ," Stop!")){ς();λ(Χ.Main);}if(Ο(ref Γ,Μ,Α++,Λ," Behavior settings"))
if(!Ȧ) λ(Χ.Ƌ);else λ(Χ.ơ);if(!Ȧ){if(Ο(ref Γ,Μ,Α++,Λ," Next hole")) ΑΧ(false);}else if(Ο(ref Γ,Μ,Α++,Λ," Undock")) ƃ=true;Γ+=Ń;if(!Ȧ) Γ+="Progress: "+Math.Round(ů,1)+" %\n";Γ+="State: "+Απ(Ψ)+" "+Ǥ+"m \n";Γ+="Load: "+ǃ+" % Max: "+Δψ+" \n";if(Č==0) Γ+="Uranium: "+(Ȫ.Count==0?"No reactors":Math.Round(Ĕ,1)+"Kg "+(О==-1?"":" Min: "+О+" Kg"))+"\n";
if(Č==1) Γ+="Battery: "+(ǝ==д.None?Γο(ǝ):ǰ+"% "+(м==-1||Ȧ?"":" Min: "+м+" %"))+"\n";if(Č==2) Γ+="Hydrogen: "+(ш.Count==0?"No tanks":Math.Round(ȉ,1)+"% "+(н==-1||Ȧ?"":" Min: "+н+" %"))+"\n";}else if(Њ==Χ.Ƌ){String ύ="";if(Ο(ref ύ,Μ,Α++,Λ," Back")){if(έ==Θ.Є) λ(Χ.Ŕ);else λ(Χ.Main);}
if(Ο(ref ύ,Μ,Α++,Λ," Max load: "+Ǣ+"%")) Ч(ref Ǣ,Ǣ<=80?-10:-5,ο.Ɩ,false);if(Ο(ref ύ,Μ,Α++,Λ," Weight limit: "+(ȓ?"On":"Off"))) ȓ=!ȓ;if(Ο(ref ύ,Μ,Α++,Λ," Ejection: "+Ερ(Д))) Д=ȗ(Д);if(Ο(ref ύ,Μ,Α++,Λ," Toggle sorters: "+(Ē?"On":"Off"))){Ē=!Ē;if(Ē) Ǜ(ΓΙ);}
if(Ο(ref ύ,Μ,Α++,Λ," Unload ice: "+(Ȓ?"On":"Off"))) Ȓ=!Ȓ;if(Ο(ref ύ,Μ,Α++,Λ," Uranium: "+(О==-1?"Ignore":"Min "+О+"Kg"))) Ч(ref О,(О>5?-5:-1),ο.Ɨ,true);if(Ο(ref ύ,Μ,Α++,Λ," Battery: "+(м==-1?"Ignore":"Min "+м+"%"))) Ч(ref м,-5,ο.Αϊ,true);if(Ο(ref ύ,Μ,Α++,Λ," Hydrogen: "+(н==-1?"Ignore":"Min "+н+"%"))) Ч(ref н,-10,ο.Αϋ,true);
if(Ο(ref ύ,Μ,Α++,Λ," When done: "+(ȭ?"Return home":"Stop"))) ȭ=!ȭ;if(Ο(ref ύ,Μ,Α++,Λ," On damage: "+Γρ(б))) б=ȗ(б);if(Ο(ref ύ,Μ,Α++,Λ," Advanced...")) λ(Χ.ΑΨ);Γ+=ΓЇ(8,ύ,Μ,ref ΗΤ);}else if(Њ==Χ.ΑΨ){if(Ο(ref Γ,Μ,Α++,Λ," Back")){if(έ==Θ.Є) λ(Χ.Ŕ);else λ(Χ.Main);}
if(Ο(ref Γ,Μ,Α++,Λ,(Ν==Κ.ј?" Grinder":" Drill")+" inv. balancing: "+(į?"On":"Off"))) į=!į;if(Ο(ref Γ,Μ,Α++,Λ," Enable"+(Ν==Κ.ј?" grinders":" drills")+": "+(Ī?"Fwd + Bwd":"Fwd"))) Ī=!Ī;if(Ο(ref Γ,Μ,Α++,Λ," Work speed fwd.: "+ň+"m/s")) Ч(ref ň,0.5f,ο.ś,false);
if(Ο(ref Γ,Μ,Α++,Λ," Work speed bwd.: "+Ň+"m/s")) Ч(ref Ň,0.5f,ο.ś,false);if(Ο(ref Γ,Μ,Α++,Λ," Acceleration: "+Math.Round(ǜ*100f)+"%"+(ǜ>0.80f?" (risky)":""))) Ч(ref ǜ,0.1f,ο.Ɖ,false);if(Ο(ref Γ,Μ,Α++,Λ," Width overlap: "+Ė*100f+"%")) Γζ(true,0.05f);if(Ο(ref Γ,Μ,Α++,Λ," Height overlap: "+Ȣ*100f+"%")) Γζ(false,0.05f);
}else if(Њ==Χ.ơ){if(Ο(ref Γ,Μ,Α++,Λ," Back")){if(έ==Θ.Є) λ(Χ.Ŕ);else λ(Χ.Main);}if(Ο(ref Γ,Μ,Α++,Λ," Max load: "+Ǣ+"%")) Ч(ref Ǣ,Ǣ<=80?-10:-5,ο.Ɩ,false);if(Ο(ref Γ,Μ,Α++,Λ," Unload ice: "+(Ȓ?"On":"Off"))) Ȓ=!Ȓ;if(Ο(ref Γ,Μ,Α++,Λ," Uranium: "+(О==-1?"Ignore":"Min "+О+"Kg"))) Ч(ref О,(О>5?-5:-1),ο.Ɨ,true);
if(Ο(ref Γ,Μ,Α++,Λ," Battery: "+(м==-1?"Ignore":"Charge up"))) м=(м==-1?1:-1);if(Ο(ref Γ,Μ,Α++,Λ," Hydrogen: "+(н==-1?"Ignore":"Fill up"))) н=(н==-1?1:-1);if(Ο(ref Γ,Μ,Α++,Λ," On damage: "+Γρ(б))) б=ȗ(б);if(Ο(ref Γ,Μ,Α++,Λ," Acceleration: "+Math.Round(ǜ*100f)+"%"+(ǜ>0.80f?" (risky)":""))) Ч(ref ǜ,0.1f,ο.Ɖ,false);
}else if(Њ==Χ.ΓІ){double ΖІ=0;if(Τ.Count>0) ΖІ=Vector3.Distance(Τ.Last().Δ,γ);if(Ο(ref Γ,Μ,Α++,Λ," Stop path recording")) ΓЎ();if(Ν!=Κ.ή){if(Ο(ref Γ,Μ,Α++,Λ," Home: "+(Ǻ?"Use old home":(Ι.β?"Was set! ":"none ")))) Ǻ=!Ǻ;}else{if(Ο(ref Γ,Μ,Α++,Λ," Connector 1: "+(Ǻ?"Use old connector":(Ι.β?"Was set! ":"none ")))) Ǻ=!Ǻ;
if(Ο(ref Γ,Μ,Α++,Λ," Connector 2: "+(Ņ?"Use old connector":(Ε.β?"Was set! ":"none ")))) Ņ=!Ņ;}if(Ο(ref Γ,Μ,Α++,Λ," Path: "+(ņ?"Use old path":(Τ.Count>1?"Count: "+Τ.Count:"none ")))) ņ=!ņ;Γ+=Ń;Γ+="Wp spacing: "+Math.Round(ΕΪ)+"m\n";}else if(Њ==Χ.ΒΒ){if(Ο(ref Γ,Μ,Α++,Λ," Stop")){ς();λ(Χ.Main);}
Γ+=Ń;Γ+="State: "+Απ(Ψ)+" \n";Γ+="Speed: "+Math.Round(Ȑ,1)+"m/s\n";Γ+="Target dist: "+Ǥ+"m\n";Γ+="Wp count: "+Τ.Count+"\n";Γ+="Wp left: "+Γέ+"\n";}else if(Њ==Χ.Γψ){List<IMyTerminalBlock>Ċ=Γς();if(ȧ) Ȥ++;if(Ȥ>=Ċ.Count) Ȥ=0;if(Ο(ref Γ,Μ,Α++,Λ," Next")) λ(Χ.Γω);
Γ+=Ń;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(ǃ,1)+"% "+Math.Round(ĩ,1)+" / "+Math.Round(Ķ,1)+"\n";Γ+="Uranium: "+(Ȫ.Count==0?"No reactors":Math.Round(Ĕ,1)+"Kg "+Βώ)+"\n";Γ+="Battery: "+(ǝ==д.None?"":ǰ+"% ")+Γο(ǝ)+"\n";Γ+="Hydrogen: "+(ш.Count==0?"No tanks":Math.Round(ȉ,1)+"% ")+"\n";
Γ+="Damage: "+(Ċ.Count==0?"None":""+(Ȥ+1)+"/"+Ċ.Count+" "+Ċ[Ȥ].CustomName)+"\n";}else if(Њ==Χ.Γω){if(Ο(ref Γ,Μ,Α++,Λ," Back")) λ(Χ.Main);Γ+=Ń;Γ+="Next scan: "+ΑΙ+"s\n";Γ+="Ship size: "+Math.Round(ǹ,1)+"m "+Math.Round(ǧ,1)+"m "+Math.Round(η,1)+"m \n";
Γ+="Broadcast: "+(ħ?"Online - "+ΓΔ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ĵ*100f,1)+"% \n";}else if(Њ==Χ.Γχ){if(Ο(ref Γ,Μ,Α++,Λ," Back")) λ(Χ.Main);Γ+=Ń;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ό==2) Γ="Fatal setup error\nNext scan: "+ΑΙ+"s\n";if(ΑΗ) Γ="Recompile script now";int ΖЇ=Γ.Split('\n').Length;for(int i=ΖЇ;i<=10;i++) Γ+="\n";Γ+=ΔА;Γ+="Last: "+Ξ+"\n";
return Γ;}void Γζ(bool ΖЃ,float ђ){ς();Ś(true,false);if(ΖЃ) Ч(ref Ė,ђ,ο.ΑЅ,false);else Ч(ref Ȣ,ђ,ο.ΑЅ,false);Γδ();Ă(true,true,0,0);}float Γι(float ε,float[]ΑΦ){float Ϊ=ΑΦ[0];for(int i=ΑΦ.Length-1;i>=0;i--)
if(ε<ΑΦ[i]) Ϊ=ΑΦ[i];return Ϊ;}String ƚ(ref int ȣ){String Ϊ="";if(ȣ>=ΓΣ.Count) ȣ=-1;if(ȣ>=0) Ϊ=ΓΣ[ȣ].CustomName;ȣ++;return Ϊ;}void Ƥ(string ρ){if(έ!=Θ.Є) return;if(ρ=="") return;IMyTerminalBlock Σ=ψ.GetBlockWithName(ρ);if(Σ==null||!(Σ is IMyTimerBlock)){Ξ="Timerblock "+ρ+" not found!";
return;}((IMyTimerBlock) Σ).Trigger();}void ę(ref int value,int Š,int ȑ,int ΓΗ){if(Š==0) return;if(value<ȑ&&ΓΗ>0||value<=ȑ&&ΓΗ<0){value+=ΓΗ;return;}int ΖΫ=Math.Abs(Š);int ΒΘ=0;int ΒЅ=1;while(true){ΒΘ+=ΒЅ*ΖΫ*10;if(Š<0&&value-ȑ<=ΒΘ) break;if(Š>0&&value-ȑ<ΒΘ) break;
ΒЅ++;}value+=ΒЅ*Š;}void ă(bool ΘΘ){Ю=Math.Max(Ю,1);Э=Math.Max(Э,1);М=Math.Max(М,0);Ă(Њ==Χ.ŕ,false,0,0);}bool ȳ(ref float value,bool ΗΟ,ο ν,String č,String Ζώ){if(č=="") return false;float ĺ=-1;bool Δλ=false;if(č.ToUpper()==Ζώ) Δλ=true;else if(!float.TryParse(č,out ĺ)) return false;
else ĺ=Math.Max(0,ĺ);if(ΗΟ) ĺ=(float) Math.Round(ĺ);Ч(ref value,ĺ,ν,Δλ,false);return true;}void Ч(ref float value,float ђ,ο ν,bool Ƃ){Ч(ref value,value+ђ,ν,Ƃ,true);}void Ч(ref float value,float ķ,ο ν,bool Ƃ,bool Ηω){float Ǒ=0,ǐ=0;if(ν==ο.ś){Ǒ=0.5f;ǐ=10f;}if(ν==ο.Ɖ){Ǒ=0.1f;ǐ=1f;}
if(ν==ο.Ő){Ǒ=50f;ǐ=100f;}if(ν==ο.Αϊ){Ǒ=5f;ǐ=30f;}if(ν==ο.Ɨ){Ǒ=1f;ǐ=25f;}if(ν==ο.Ɩ){Ǒ=10f;ǐ=95f;}if(ν==ο.Αϋ){Ǒ=10f;ǐ=90f;}if(ν==ο.ΖΛ){Ǒ=10f;ǐ=1800;}if(ν==ο.ΑЅ){Ǒ=0.0f;ǐ=0.75f;}if(ķ==-1&&Ƃ){value=-1;return;}if(value==-1) Ƃ=false;
bool ΔЇ=ķ<Ǒ||ķ>ǐ;if(ΔЇ&&Ηω){if(ķ<value) value=ǐ;else if(ķ>value) value=Ǒ;}else value=ķ;if(ΔЇ&&Ƃ) value=-1;else value=Math.Max(Ǒ,Math.Min(value,ǐ));value=(float) Math.Round(value,2);}void ΑΧ(bool ΗΚ){if(ΗΚ) Й=Math.Max(0,Й-1);else Й++;ΒΕ(true);}T ȗ<T>(T ε){int Α=Array.IndexOf(Enum.GetValues(ε.GetType()),ε);
Α++;if(Α>=Ελ(ε)) Α=0;return(T) Enum.GetValues(ε.GetType()).GetValue(Α);}int Ελ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒΑ(){ƈ.Clear();for(int i=0;i<Τ.Count;i++) ƈ.Add(Τ[i]);Τ.Clear();ǁ=true;Βπ=new Ω(Ι);Βσ=new Ω(Ε);Ι.β=false;if(Ν==Κ.ή) Ε.β=false;for(int i=0;i<ѓ.Count;i++)
if(!Ƿ.Contains(ѓ.Keys.ElementAt(i)))
Ƿ.Add(ѓ.Keys.ElementAt(i));Ǻ=false;Ņ=false;ņ=false;λ(Χ.ΓІ);}void ΓЎ(){if(Ǻ) Ι=Βπ;if(Ņ) Ε=Βσ;if(ņ){Τ.Clear();for(int i=0;i<ƈ.Count;i++) Τ.Add(ƈ[i]);}ǁ=false;ς();λ(Χ.Main);}void ΖΥ(){if(!ǁ) return;if(Ψ!=Β.Ъ){ΓЎ();return;}if(!Βπ.β) Ǻ=false;if(!Βσ.β) Ņ=false;if(ƈ.Count<=1) ņ=false;
IMyShipConnector ѝ=Щ(MyShipConnectorStatus.Connectable);if(ѝ==null) ѝ=Щ(MyShipConnectorStatus.Connected);if(ѝ!=null){if(Math.Round(Ȑ,2)<=0.20) Ββ++;else Ββ=0;if(Ββ>=5){if(Ν==Κ.ή&&(Ι.β||Ǻ)&&Vector3.Distance(Ι.Δ,ѝ.GetPosition())>5){Ε.α=Υ.WorldMatrix.Forward;Ε.τ=Υ.WorldMatrix.Left;
Ε.ΰ=Υ.WorldMatrix.Down;Ε.ι=Υ.GetNaturalGravity();Ε.Δ=ѝ.GetPosition();Ε.β=true;Ε.з=ѝ.Position;}else{Ι.α=Υ.WorldMatrix.Forward;Ι.τ=Υ.WorldMatrix.Left;Ι.ΰ=Υ.WorldMatrix.Down;Ι.ι=Υ.GetNaturalGravity();Ι.Δ=ѝ.GetPosition();Ι.β=true;Ι.з=ѝ.Position;}}}double Βφ=-1;if(Τ.Count>0) Βφ=Vector3.Distance(γ,Τ.Last().Δ);
double ΗΪ=Math.Max(1.5,Math.Pow(Ȑ/100.0,2));double ΔЁ=Math.Max(Ȑ*ΗΪ,2);ΕΪ=ΔЁ;if((Βφ==-1)||Βφ>=ΔЁ){Ω Φ=new Ω(γ,ƺ,Ŧ,Ų,Υ.GetNaturalGravity());Φ.Εέ(Ǹ,Ƿ);Τ.Add(Φ);}}Ω ǽ(){if(Ν!=Κ.ή) return Ι;return ΔΤ;}void ğ(Ω Ϋ,Θ Л){ΔΤ=Ϋ;if(έ==Θ.Є) л=Л;}bool Ǭ(Ω Ϋ,float χ,bool Ηρ,out Vector3 ϊ)
{if(Ηρ){Vector3I Βλ=new Vector3I((int) Ϋ.з.X,(int) Ϋ.з.Y,(int) Ϋ.з.Z);IMySlimBlock Σ=Me.CubeGrid.GetCubeBlock(Βλ);if(Σ==null||!(Σ.FatBlock is IMyShipConnector)){ϊ=new Vector3();return false;}Vector3 Ζκ=Т(Υ,Σ.FatBlock.GetPosition()-γ);Vector3 Ζι=Т(Υ,Σ.FatBlock.WorldMatrix.Forward);ϊ=Ϋ.Δ-ΒΔ(Ϋ.α,Ϋ.ΰ*-1,Ζκ)-ΒΔ(Ϋ.α,Ϋ.ΰ*-1,Ζι)*χ;
return true;}else{ϊ=Ϋ.Δ;return true;}}Vector3 Ǿ(int Ζη,int З,bool ΗΝ){if(!ΗΝ&&Δζ) return ΒΧ;float Ζθ=((Ϗ-1f)/2f)-Ζη;float ΗΠ=((Ћ-1f)/2f)-З;ΒΧ=Ε.Δ+Ε.τ*Ζθ*ǹ+ȍ*-1*ΗΠ*ǧ;Δζ=true;return ΒΧ;}Vector3 Αο(Vector3 Ηί,float Βί){return Ηί+(ǒ*Βί);}ά Αδ(){float Ѝ=-1;ά Ϊ=ά.ǩ;
if(Ν!=Κ.ή){if(έ!=Θ.Ы){Vector3 Ђ=ā(ǒ,ȍ*-1,γ-Ε.Δ);if(Math.Abs(Ђ.X)<=(float)(Ϗ*ǹ)/2f&&Math.Abs(Ђ.Y)<=(float)(Ћ*ǧ)/2f){if(Ђ.Z<=-1&&Ђ.Z>=-Đ*2) return ά.Ɗ;if(Ђ.Z>-1&&Ђ.Z<η*2) return ά.ǩ;}if(ȕ(Ε.Δ,ref Ѝ)) Ϊ=ά.ǩ;}if(Ι.β){if(ȕ(Ι.Δ,ref Ѝ)) Ϊ=ά.Ǩ;for(int i=0;i<Τ.Count;i++)
if(ȕ(Τ[i].Δ,ref Ѝ)) Ϊ=ά.ǿ;if(Vector3.Distance(γ,Ι.Δ)<dockDist*η) Ϊ=ά.Ǩ;if(Щ(MyShipConnectorStatus.Connectable)!=null||Щ(MyShipConnectorStatus.Connected)!=null)
Ϊ=ά.Ǩ;}}else{Vector3 ϊ=new Vector3();IMyShipConnector ǳ=Щ(MyShipConnectorStatus.Connected);if(Ι.β){if(ȕ(Ι.Δ,ref Ѝ)) Ϊ=ά.Ǩ;if(Ǭ(Ι,dockDist,true,out ϊ))
if(ȕ(ϊ,ref Ѝ)) Ϊ=ά.Ǩ;if(ǳ!=null&&Vector3.Distance(ǳ.GetPosition(),Ι.Δ)<5)
return ά.ΑΫ;}for(int i=0;i<Τ.Count;i++)
if(Vector3.Distance(Τ[i].Δ,Ι.Δ)>dockDist*η&&Vector3.Distance(Τ[i].Δ,Ε.Δ)>dockDist*η)
if(ȕ(Τ[i].Δ,ref Ѝ)) Ϊ=ά.ǿ;if(Ε.β){if(ȕ(Ε.Δ,ref Ѝ)) Ϊ=ά.ǩ;if(Ǭ(Ε,dockDist,true,out ϊ))
if(ȕ(ϊ,ref Ѝ)) Ϊ=ά.ǩ;if(ǳ!=null&&Vector3.Distance(ǳ.GetPosition(),Ε.Δ)<5)
return ά.Αά;}}return Ϊ;}bool ȕ(Vector3 ϊ,ref float Ѝ){float χ=Vector3.Distance(ϊ,γ);if(χ<Ѝ||Ѝ==-1){Ѝ=χ;return true;}return false;}void ΑБ(){if(ό>0){Ξ="Setup error! Can't start";return;}if(Ν==Κ.ή){Ȗ();return;}Ε.Δ=γ;Ε.ι=Υ.GetNaturalGravity();Ε.α=ƺ;Ε.ΰ=Ŧ;Ε.τ=Ų;
ǒ=ю.WorldMatrix.Forward;ȍ=Ε.ΰ;if(ǒ==Υ.WorldMatrix.Down) ȍ=Υ.WorldMatrix.Backward;Ś(true,true);Ρ(Β.Ǽ);ΑА();}void Ś(bool Ǎ,bool ΗΞ){if(έ==Θ.Ы&&!Ǎ) return;bool ΖБ=Ǎ||έ==Θ.Αε||Ϗ!=Ю||Ћ!=Э||Ű!=ǣ;if(ΖБ){if(έ!=Θ.Ы){έ=Θ.ƍ;Ǿ(σ,π,ΗΞ);Ξ="Job changed, lost progress";}Ű=ǣ;
Ϗ=Ю;Ћ=Э;π=0;σ=0;к="";Ǔ=-1;ď=0;ȋ=0;К=0;Й=0;ΒΕ(true);}}void ΑЉ(){Ь(γ,0);х(Ǹ,true);}void ΑА(){Ǯ(У.Ɵ);ΑЉ();Ř(Ƕ,false);Ǜ(ΓΙ);έ=Θ.Є;Γγ(true);л=έ;λ(Χ.Ŕ);ΕЍ();Ƴ=true;ƾ=0;for(int i=Ŝ.Count-1;i>=0;i--)
if(Œ(Ŝ[i],false)) ƾ++;if(ƾ>0) Ξ="Started with damage";}void ς(){if(έ==Θ.Є){έ=Θ.Ğ;Ξ="Job paused";}Ρ(Β.Ъ);л=έ;Ơ(false,0,0,0,0);ΒΓ();Γΰ(new Vector3(),false);ΓЍ();ΑЋ(ChargeMode.Auto);ΑЌ(false);ΓЉ(true);ȁ(Β.Ъ);Ă(false,false,0,0);х(ξ,false);х(и,true);Ǜ(true);ΑΘ=false;Ǝ();ư=false;
Ƴ=false;ż=false;ƃ=false;if(Њ!=Χ.Main&&Њ!=Χ.Ƌ&&Њ!=Χ.ΑΨ&&Њ!=Χ.ơ)
λ(Χ.Main);}void Ȗ(){ά Џ=Αδ();if(Ν==Κ.ή){if(!Ε.β||!Ι.β) return;ΑА();bool Βι=Vector3.Distance(γ,Ι.Δ)<Vector3.Distance(γ,Ε.Δ);if(Ŀ==Θ.ė) Βι=true;if(Ŀ==Θ.Ę) Βι=false;if(Βι){ğ(Ι,Θ.ė);switch(Џ){case ά.ΑΫ:Ρ(Β.Ț);break;case ά.ǿ:Ρ(Β.С);break;case ά.Ǩ:Ρ(Β.Ȕ);break;default:Ρ(Β.ș);break;
}}else{ğ(Ε,Θ.Ę);switch(Џ){case ά.Αά:Ρ(Β.Ț);break;case ά.ǩ:Ρ(Β.Ȕ);break;case ά.ǿ:Ρ(Β.С);break;default:Ρ(Β.ș);break;}}}else{if(έ!=Θ.Ğ&&έ!=Θ.ƍ) return;bool Ησ=έ==Θ.ƍ;ΑА();bool Βξ=œ(false)&&Ι.β;switch(Џ){case ά.ǩ:Ρ(Βξ?Β.Ș:Β.Ǽ);break;case ά.ǿ:Ρ(Βξ?Β.ї:Β.С);break;case ά.Ǩ:Ρ(Βξ?Β.Ȕ:Β.ȃ);break;
case ά.Ɗ:{if(ȋ!=Й||Ησ) Ρ(Β.Ȁ);else Ρ(Β.ĝ);}break;default:break;}}}void Ȳ(){if(έ==Θ.Ы&&!Ι.β) return;if(Ν==Κ.ή&&(!Ε.β||!Ι.β)) return;Ξ="Move to job";ά Џ=Αδ();if(Ν==Κ.ή){ğ(Ε,Θ.Ę);switch(Џ){case ά.ǩ:Ρ(Β.Ȕ);break;case ά.ǿ:Ρ(Β.С);break;case ά.Αά:return;default:Ρ(Β.ș);break;
}ȁ(Β.Ț);}else{switch(Џ){case ά.ǩ:Ρ(Β.С);break;case ά.ǿ:Ρ(Β.С);break;case ά.Ǩ:Ρ(Β.ȃ);break;case ά.Ɗ:Ρ(Β.Ȁ);break;default:break;}if(έ==Θ.Ы) ȁ(Β.С);else ȁ(Β.ȯ);ΑΘ=true;}ΑЉ();λ(Χ.ΒΒ);Ř(Ƕ,false);л=Θ.ƙ;}void ƒ(){if(!Ι.β) return;Ξ="Move home";ά Џ=Αδ();if(Ν==Κ.ή){ğ(Ι,Θ.ė);switch(Џ)
{case ά.ǿ:Ρ(Β.ї);break;case ά.Ǩ:Ρ(Β.Ȕ);break;case ά.ΑΫ:return;default:Ρ(Β.ș);break;}ȁ(Β.Ț);}else{if(Щ(MyShipConnectorStatus.Connected)!=null) return;if(Щ(MyShipConnectorStatus.Connectable)!=null){Ρ(Β.Ō);ȁ(Β.ȃ);return;}switch(Џ){case ά.ǩ:Ρ(Β.Ș);break;case ά.ǿ:Ρ(Β.ї);break;case ά.Ǩ:Ρ(Β.ї);break;
case ά.Ɗ:Ρ(Β.Ā);break;default:break;}ȁ(Β.ȃ);}ΑЉ();λ(Χ.ΒΒ);Ř(Ƕ,false);л=Θ.Ƙ;}void Ρ(Β ΒЇ){if(ΒЇ==Β.Ъ) ž=Β.Ъ;if(ž!=Β.Ъ&&Ψ==ž&&ΒЇ!=ž){ς();return;}Ѓ=true;Ψ=ΒЇ;}void ȁ(Β Л){this.ž=Л;}Ȃ Ευ(int ŭ,bool ǔ){if(ǔ){ё=null;σ=0;π=0;}if(ǣ==њ.ƣ){int ΔЃ=ŭ+1;π=(int) Math.Floor(Љ(ŭ,Ϗ));
if(π%2==0) σ=ŭ-(π*Ϗ);else σ=Ϗ-1-(ŭ-(π*Ϗ));if(π>=Ћ) return Ȃ.ΑΪ;else return Ȃ.Αμ;}else if(ǣ==њ.Γε){if(ё==null) ё=new int[]{0,-1,0,0};int Δι=(int) Math.Ceiling(Ϗ/2f);int Δθ=(int) Math.Ceiling(Ћ/2f);int Ζϊ=(int) Math.Floor(Ϗ/2f);int Ζω=(int) Math.Floor(Ћ/2f);
int Ů=0;while(ё[2]<Math.Pow(Math.Max(Ϗ,Ћ),2)){if(Ů>200) return Ȃ.Εμ;Ů++;ё[2]++;if(-Δι<σ&&σ<=Ζϊ&&-Δθ<π&&π<=Ζω){if(ё[3]==ŭ){this.σ=σ-1+Δι;this.π=π-1+Δθ;return Ȃ.Αμ;}ё[3]++;}if(σ==π||(σ<0&&σ==-π)||(σ>0&&σ==1-π)){int Ηβ=ё[0];ё[0]=-ё[1];ё[1]=Ηβ;
}σ+=ё[0];π+=ё[1];}}return Ȃ.ΑΪ;}void ΖΣ(){if(Ψ==Β.Ǽ){if(Ѓ){Ѐ=0;if(ȋ!=Й) ď=0;ȋ=Й;}if(Ѐ==0){int ΒΩ=Εβ(Й);if(ΒΩ<0){if(!ư){ư=true;ΔΣ=γ;}Ь(ΔΣ,0);Ξ="Waiting for a free row";return;}ư=false;bool Δπ=ΒΩ!=Й;if(Δπ){Й=ΒΩ;ď=0;ȋ=Й;}Ȃ Ϊ=Ευ(Й,Ѓ||Δπ);if(Ϊ==Ȃ.ΑΪ)
{έ=Θ.Αε;Ξ="Job done";if(ȭ&&Ι.β){Ρ(Β.Ș);ȁ(Β.ȃ);л=Θ.Ƙ;}else{Ρ(Β.Ĝ);ȁ(Β.ȯ);л=Θ.ƙ;}return;}if(Ϊ==Ȃ.Αμ){Ѐ=1;х(ξ,true);о=Ǿ(σ,π,true);Ь(о,10);Р(Ε.ΰ,Ε.α,Ε.τ,false);}}else{if(Г<wpReachedDist){Ρ(Β.ĝ);return;}}}if(Ψ==Β.ĝ){if(Ѓ){х(ξ,true);Ǜ(false);о=Ǿ(σ,π,false);
Ь(Αο(о,0),0);Р(Ε.ΰ,Ε.α,Ε.τ,false);Ѐ=1;Я=0;П=0;ƭ=0;Βχ=-1;Đ=М;Βρ=true;}if(!Αα()){Ρ(Β.Ā);return;}if(œ(true)){ΔІ=ǻ("","ORE",В.Ȱ);if((Д==ϋ.Αζ||Д==ϋ.Ə||
Д==ϋ.Αφ||Д==ϋ.Ɣ)&&Ν!=Κ.ј)
Ρ(Β.Ŏ);else if((Д==ϋ.Γθ||Д==ϋ.Αβ)&&Ν!=Κ.ј)
Ρ(Β.ΒΖ);else
Ρ(Β.Ā);return;}К=Vector3.Distance(γ,о);if(К>ď){ď=К;Βρ=false;}if(Ν==Κ.ј&&Γσ()==MyDetectedEntityType.SmallGrid)
П+=2;else П-=2;П=Math.Max(100,Math.Min(400,П));if(Ѐ>0&&Ѐ<П){if(К>Я){if(П>150) Я=К;else Я=(float) Math.Ceiling(К);Ѐ=1;}else Ѐ++;}else{if(Ѐ>0){Ξ="Ship stuck! Retrying";Я=К;Ѐ=0;Ă(false,true,0,η*sensorRange);}Ь(Αο(о,Math.Max(0,Я-η)),Ας(false));if(Г<=wpReachedDist/2){Ѐ=1;Я=0;}
return;}Ă(false,true,η*sensorRange,0);Vector3 Ζέ=о+ǒ*К;bool Δξ=false;if(Vector3.Distance(Ζέ,γ)>0.3f){Vector3 Ζλ=о+ǒ*(К+0.1f);Ь(Ζλ,4);Δξ=true;}else{float Ǖ=Ας(true);Vector3 ΔΨ=Αο(о,Math.Max(М+1,К+1));ǚ(true,false,false,ΔΨ,ΔΨ-о,Ǖ,Ǖ);}bool Βΰ=false;if(й==у.Αή||й==у.Αέ)
{if(!Δξ){float ΒЈ=0;foreach(IMyTerminalBlock Ǧ in ξ)
ΒЈ+=Εθ(Ǧ,"","",й==у.Αέ?new string[]{"STONE"}:null);if(ΒЈ>Βχ||К<М||Βρ){ΒΞ=0;ƭ=К;Đ=(int)(Math.Max(Đ,ƭ)+η/2);}else{Βΰ=К-ƭ>2&&ΒΞ>=20;ΒΞ++;}Βχ=ΒЈ;}}else Βΰ=К>=Đ;if(ȋ!=Й){Ρ(Β.Ȁ);К=0;return;}if(Βΰ){Й++;Ρ(Β.Ȁ);К=0;return;}}if(Ψ==Β.Ȁ||Ψ==Β.Ā||Ψ==Β.Ŏ)
{if(Ѓ){о=Ǿ(σ,π,false);Р(Ε.ΰ,Ε.α,Ε.τ,false);х(ξ,Ī);Ǜ(false);Я=Vector3.Distance(γ,о);Ă(false,true,0,η*sensorRange);}Ь(о,Ας(false));if(Vector3.Distance(γ,о)>=Я+5){х(ξ,false);Ǜ(true);Ξ="Can´t return!";}if(Г<wpReachedDist){if(Ψ==Β.Ȁ&&ΑΘ) Ρ(Β.Ĝ);if(Ψ==Β.Ȁ) Ρ(Β.Ǽ);
if(Ψ==Β.Ŏ) Ρ(Β.Ƒ);if(Ψ==Β.Ā){if(Ι.β) Ρ(Β.Ș);else{ς();Ȳ();Ξ="Can´t return, no dock found";}}return;}}if(Ψ==Β.ї||Ψ==Β.С){if(Ψ==Β.С&&έ==Θ.Є&&Ν!=Κ.ή){if(!Αα()||œ(true)){Ρ(Β.ї);return;}}bool ΒБ=false;bool ΓΧ=false;bool ΔΘ=false;float Ĥ=0;bool ΔЅ=false;Ω Φ=null;
if(Ѓ){if(Ψ==Β.ї||Ν==Κ.ή){Ω Ϋ=ǽ();ζ=new ΑЇ();ζ.И=Ϋ;ζ.Ť=followPathDock*η;ζ.ΓΨ=useDockDirectionDist*η;ζ.Ĥ=10;ζ.ł.Add(Ϋ.Δ);Vector3 ģ=new Vector3();if(Ǭ(Ϋ,dockDist*η,true,out ģ))
ζ.ł.Add(ģ);else ζ.Ť*=1.5f;if(Ν==Κ.ή){if(Ϋ==Ι) ζ.Ʒ=Ε.Δ;if(Ϋ==Ε) ζ.Ʒ=Ι.Δ;ζ.Βζ=dockDist*η*1.1f;}}else if(Ψ==Β.С){ζ=new ΑЇ();ζ.И=Ε;ζ.Ť=followPathJob*η;ζ.ΓΨ=useJobDirectionDist*η;ζ.Ĥ=10;ζ.Ʒ=Ι.Δ;ζ.Βζ=dockDist*η*1.1f;ζ.ł.Add(Ε.Δ);if(έ==Θ.Ы){if(!Ι.β||Τ.Count==0){ς();return;}
float Ζρ=Vector3.Distance(Τ.First().Δ,Ι.Δ);float Ζς=Vector3.Distance(Τ.Last().Δ,Ι.Δ);if(Ζρ<Ζς) ζ.И=Τ.Last();else ζ.И=Τ.First();}}ŷ=new Vector3();ΔЅ=!ΕϏ(γ);х(ξ,false);Ǜ(true);э=-1;double Ǟ=-1;for(int i=Τ.Count-1;i>=0;i--){if(Vector3.Distance(Τ[i].Δ,ζ.Ʒ)<=ζ.Βζ) continue;
double χ=Vector3.Distance(Τ[i].Δ,γ);if(Ǟ==-1||χ<Ǟ){э=i;Ǟ=χ;}}ȩ=Εφ(ζ.И.Δ,э);ĉ=null;}Εή(Τ,ȩ,ζ.ł,ζ.Ť,Ѓ,ref Ѐ);for(int i=0;i<ζ.ł.Count;i++){float χ=Vector3.Distance(γ,ζ.ł[i]);if(χ<=ζ.Ť) ΒБ=true;if(χ<=ζ.ΓΨ) ΓΧ=true;}if(ΓΧ) Ĥ=ζ.Ĥ;float ΘΚ=ĉ!=null?ĉ.Ќ:Ȑ;
float Ηψ=(float) Math.Max(Ȑ*0.1f*η,wpReachedDist);if((Г<Ηψ)||Ѓ){if(!Ѓ) э+=ȩ;if(ȩ==0||э>Τ.Count-1||э<0)
ΒБ=true;else{Γέ=ȩ>0?Τ.Count-1-э:э;Φ=Τ[э];ĉ=Φ;if(э>=1&&э<Τ.Count-1)
ŷ=Φ.Δ-Τ[э-ȩ].Δ;else ĉ=null;ω=Φ.Δ+Ετ(э,ȩ);ΔΘ=true;}}if(ΓΧ)
Р(ζ.И.ΰ,ζ.И.α,ζ.И.τ,false);else if(ΔЅ)
ΑΩ(ζ.И.ΰ,10,true);else if(ΔΘ&&Φ!=null){if(ȩ>0) Р(Φ.ΰ,Φ.α,Φ.τ,90,false);else Р(Φ.ΰ,-Φ.α,-Φ.τ,90,false);}ǚ(true,false,true,ω,ŷ,ĉ==null?0:ĉ.Ќ,Ĥ);if(ΒБ){Γέ=0;if(Ψ==Β.ї||Ν==Κ.ή){Ρ(Β.Ȕ);return;}if(Ψ==Β.С&&ΑΘ){Ρ(Β.Ĝ);return;}if(Ψ==Β.С){Ρ(Β.Ǽ);return;}
}}if(Ψ==Β.Ȕ||Ψ==Β.ΑЊ){Ω Ϋ=ǽ();if(Ѓ){if(!Ǭ(Ϋ,dockDist*η,true,out ω)){Ǯ(У.ŋ);ς();return;}Ь(ω,0);ΑΩ(Ϋ.ΰ,90,true);}if(Г<followPathDock*η&&Г!=-1){Ь(ω,10);Р(Ϋ.ΰ,Ϋ.α,Ϋ.τ,false);}if(Щ(MyShipConnectorStatus.Connectable)!=null||Щ(MyShipConnectorStatus.Connected)!=null)
{Ρ(Β.Ō);return;}if(Г<wpReachedDist/2&&Г!=-1){Ρ(Β.ŉ);return;}}if(Ψ==Β.ŉ||Ψ==Β.ȯ){if(Ѓ){if(Ψ==Β.ŉ){Ω Ϋ=ǽ();if(!Ǭ(Ϋ,dockDist*η,true,out ω)){Ǯ(У.ŋ);ς();return;}ǚ(true,true,false,ω,0);Р(Ϋ.ΰ,Ϋ.α,Ϋ.τ,10,false);}if(Ψ==Β.ȯ){Р(Ε.ΰ,Ε.α,Ε.τ,0.5f,false);ω=Ε.Δ;
ǚ(true,true,false,ω,0);}}if(ǀ){Ơ(false,0,0,0,0);if(Ψ==Β.ŉ) Ρ(Β.Ō);if(Ψ==Β.ȯ) ς();return;}}if(Ψ==Β.Ō){if(Щ(MyShipConnectorStatus.Connected)!=null){if(Ν==Κ.ή) Ρ(Β.Ț);else Ρ(Β.ȃ);return;}Ω Ϋ=ǽ();if(Ѓ){П=0;Ζσ=DateTime.Now;Ѐ=0;Р(Ϋ.ΰ,Ϋ.α,Ϋ.τ,false);}Vector3I Βλ=new Vector3I((int) Ϋ.з.X,(int) Ϋ.з.Y,(int) Ϋ.з.Z);
IMySlimBlock ΔΡ=Me.CubeGrid.GetCubeBlock(Βλ);float Δέ=dockingSpeed;float ΖЋ=dockingSpeed*5;float ΔΕ=Math.Max(1.5f,Math.Min(5f,η*0.15f));if(!Ǭ(Ϋ,0,true,out ω)||!Ǭ(Ϋ,ΔΕ,true,out ŷ)||
ΔΡ==null||!ΔΡ.FatBlock.IsFunctional){Ǯ(У.ŋ);ς();return;}if(П==1||(Vector3.Distance(γ,ω)<=ΔΕ*1.1f&&!Ѓ))
П=1;else{Vector3 Ηι=Т(Υ,ŷ-γ);Vector3 Βκ=Т(Υ,Υ.GetNaturalGravity());float ΗΡ=ƌ(Ηι,Βκ,null);Δέ=Math.Min(ΖЋ,ΗΡ);}ǚ(true,false,false,ω,ω-γ,dockingSpeed,Δέ);if(Ѓ) Я=(float) Г;IMyShipConnector Βά=Щ(MyShipConnectorStatus.Connectable);if(Βά!=null){ǚ(false,false,false,ω,0);
if(Ѐ>0) Ѐ=0;Ѐ--;if(Ѐ<-5){Βά.Connect();if(Βά.Status==MyShipConnectorStatus.Connected){if(Ν==Κ.ή) Ρ(Β.Ț);else Ρ(Β.ȃ);ΒΓ();Ř(Ƕ,true);return;}}}else{float Δά=(float) Math.Round(Г,1);if(Δά<Я){Ѐ=-1;Я=Δά;}else Ѐ++;if(Ѐ>20){Ρ(Β.ΑЊ);return;}}}if(Ψ==Β.ȃ||Ψ==Β.Ț||
Ψ==Β.ΒΗ||Ψ==Β.Αη||Ψ==Β.ě){bool ΒΜ=false,ΒΝ=false;if(Ν==Κ.ή){if(ǽ()==Ι) ΒΜ=true;else if(ǽ()==Ε) ΒΝ=true;}if(Ѓ){ż=false;if(Щ(MyShipConnectorStatus.Connected)==null){Ρ(Β.ș);return;}ΒΓ();if(ΒΜ) Ƥ(υ.џ);if(ΒΝ) Ƥ(φ.џ);ē=false;Ȅ=false;Ȉ=false;ƅ=false;}if(Щ(MyShipConnectorStatus.Connected)==null){ς();Ǯ(У.Γϋ);return;}
if(έ!=Θ.Є||м==-1||ǝ==д.None) Ȅ=true;else if(ǰ>=100f) Ȅ=true;else if(ǰ<=99f) Ȅ=false;if(έ!=Θ.Є||н==-1||ш.Count==0) Ȉ=true;else if(ȉ>=100f) Ȉ=true;else if(ȉ<=99) Ȉ=false;if(έ!=Θ.Є||О==-1||Ȫ.Count==0) ƅ=true;else ƅ=Ĕ>=О;Ġ г=null;if(ΒΜ) г=υ;
if(ΒΝ) г=φ;if(г!=null&&(г.Ј==δ.Ɯ||г.Ј==δ.ƛ)) Ȅ=true;if(г!=null&&(г.Ј==δ.Αώ)) if(!ē) Ȅ=false;if(г!=null&&(г.Ј==δ.ΑЂ||г.Ј==δ.ΑЀ)) Ȉ=true;if(г!=null&&(г.Ј==δ.ΑЁ)) if(!ē) Ȉ=false;if(ȧ){ChargeMode ΔΟ=Ȅ?ChargeMode.Auto:ChargeMode.Recharge;if(г!=null&&(г.Ј==δ.ƛ||г.Ј==δ.Ɯ))
ΔΟ=ChargeMode.Discharge;ΑЋ(ΔΟ);ΑЌ(!Ȉ);}if(!ē){if(Ν==Κ.ή)
ē=έ!=Θ.Є||Εα(Ѓ,true)||ƃ;else
ē=έ!=Θ.Є||Γό();}else{if(!Ȅ) Ρ(Β.ě);if(!Ȉ) Ρ(Β.Αη);if(!ƅ) Ρ(Β.ΒΗ);Ѓ=false;}if(ē&&Ȅ&&Ȉ&&ƅ){ΑЋ(ChargeMode.Auto);ΑЌ(false);if(έ==Θ.Є){if(Ν==Κ.ή){if(ǽ()==Ι) Ƥ(υ.Ǐ);else if(ǽ()==Ε) Ƥ(φ.Ǐ);if(ǽ()==Ι) ğ(Ε,Θ.Ę);else ğ(Ι,Θ.ė);}}Ρ(Β.ș);return;}}if(Ψ==Β.ș){if(Ѓ){IMyShipConnector ǳ=Щ(MyShipConnectorStatus.Connected);
if(ǳ==null){Ρ(Β.С);return;}IMyShipConnector Ж=ǳ.OtherConnector;ΓЈ(ǳ,false);Ř(Ƕ,false);Ω Ϋ=null;if(Vector3.Distance(ǳ.GetPosition(),Ι.Δ)<5f&&Ι.β) Ϋ=Ι;if(Vector3.Distance(ǳ.GetPosition(),Ε.Δ)<5f&&Ε.β) Ϋ=Ε;if(Ϋ!=null){if(!Ǭ(Ϋ,dockDist*η,true,out ω)){Ǯ(У.ŋ);ς();return;}
Ь(ω,5);Р(Ϋ.ΰ,Ϋ.α,Ϋ.τ,false);}else Ь(γ+Ж.WorldMatrix.Forward*dockDist*η,5);if(έ==Θ.Є) Ǯ(У.Ɵ);}if(Г<wpReachedDist){х(и,true);Ρ(Β.С);return;}}if(Ψ==Β.Ĝ){if(Ѓ){Ǜ(true);х(ξ,false);ω=Ε.Δ;Ь(ω,20);Р(Ε.ΰ,Ε.α,Ε.τ,false);}if(Г<wpReachedDist/2){Ρ(Β.ȯ);return;}}if(Ψ==Β.Ș){
if(Ѓ){Ǜ(true);х(ξ,false);int Ѝ=-1;double Ǟ=-1;for(int i=Τ.Count-1;i>=0;i--){double χ=Vector3.Distance(Τ[i].Δ,γ);if(Ǟ==-1||χ<Ǟ){Ѝ=i;Ǟ=χ;}}if(Ѝ==-1){Ρ(Β.ї);return;}ω=Τ[Ѝ].Δ;Ь(ω,10);Р(Ε.ΰ,Ε.α,Ε.τ,false);}if(Г<wpReachedDist){Ρ(Β.ї);return;}}if(Ψ==Β.Ƒ)
{bool ǟ=false;if(Ѓ){Ǜ(true);if((Д==ϋ.Αζ||Д==ϋ.Ə)&&ƕ()&&
Ě(ǒ,Υ.GetNaturalGravity())<25&&Ϗ>=2&&Ћ>=2){Vector3 ŧ=γ;if(σ>0&&π<Ћ-1) ŧ=Ǿ(σ-1,π+1,true);else if(σ<Ϗ-1&&π<Ћ-1) ŧ=Ǿ(σ+1,π+1,true);else if(σ<Ϗ-1&&π>0) ŧ=Ǿ(σ+1,π-1,true);else if(σ>0&&π>0) ŧ=Ǿ(σ-1,π-1,true);else ǟ=true;if(!ǟ) Ь(ŧ,10);
}else ǟ=true;}if(Г<wpReachedDist/2) ǟ=true;if(ǟ){Ρ(Β.Ƨ);return;}}if(Ψ==Β.ΒΖ||Ψ==Β.Ƨ){if(Ѓ){ǚ(true,true,false,γ,0);х(ξ,false);Ǜ(true);Ѐ=-1;П=Д==ϋ.Αφ||Д==ϋ.Ɣ?0:-1;}bool ċ=!Αα();int ΑΞ=ǻ("STONE","ORE",В.Ȱ);if(Д==ϋ.Αβ||Д==ϋ.Ɣ||Д==ϋ.Ə)
ΑΞ+=ǻ("ICE","ORE",В.Ȱ);bool Ζϋ=ΑΞ>0;bool Βε=false;if(П>=0){float ġ=(float) Math.Sin(Γκ(П))*ǹ/3f;float Ζί=(float) Math.Cos(Γκ(П))*ǧ/3f;Vector3 Ζζ=Ǿ(σ,π,true)+ΒΔ(ǒ,ȍ*-1,new Vector3(ġ,Ζί,0));Ь(Ζζ,0.3f);if(Г<Math.Min(ǹ,ǧ)/10f) П+=5f;if(П>=360) П=0;}if(Ѐ==-1||ΑΞ<Ѐ){Ѐ=ΑΞ;Я=0;}
else{Я++;if(Я>50) Βε=true;}if(!Ζϋ||ċ||Βε){if(!ċ){int Ηξ=ǻ("","ORE",В.Ȱ);if(œ(true)) ċ=true;else if(100-(Љ(Ηξ,ΔІ)*100)<minEjection) ċ=true;else Ǯ(У.Ɵ);}if(Βε&&ċ) Ξ="Ejection failed";if(Ψ==Β.Ƨ){if(ċ){if(Ι.β) Ρ(Β.Ș);else{ς();Ȳ();Ξ="Can´t return, no dock found";}}
else Ρ(Β.Ǽ);}else if(ċ) Ρ(Β.Ā);else Ρ(Β.ĝ);return;}}Ѓ=false;}void Εή(List<Ω>Ȯ,int θ,List<Vector3>ƀ,float Ǥ,bool ǔ,ref int ќ){if(ǔ){for(int i=0;i<Ȯ.Count;i++) Ȯ[i].Ќ=0;ќ=-1;return;}if(θ==0) return;int ђ=θ*-1;if(ќ==-1) ќ=ђ>0?1:Ȯ.Count-2;int Ů=0;while(ќ>=1&&ќ<Ȯ.Count-1)
{if(Ů>50) return;Ů++;try{if((ђ<0&&ќ>=1)||(ђ>0&&ќ<=Ȯ.Count-2)){Ω ε=Ȯ[ќ];bool ΔЂ=false;for(int j=0;j<ƀ.Count;j++)
if(Vector3.Distance(ε.Δ,ƀ[j])<=Ǥ){ΔЂ=true;break;}if(!ΔЂ){Ω ΑΔ=Ȯ[ќ-ђ];Ω ΔЃ=Ȯ[ќ+ђ];Vector3 Ηκ=ε.Δ-ΔЃ.Δ;Vector3 ΓΤ=ΑΔ.Δ-ε.Δ;Vector3 ΗΛ=ε.Δ+Vector3.Normalize(Ηκ)*ΓΤ.Length();Vector3 Ζπ=ΑΔ.Δ-ΗΛ;Vector3 Ǆ=ā(θ>0?ε.α:ε.α*-1,ε.ΰ*-1,Ζπ);Vector3 ΖЉ=ā(θ>0?ε.α:ε.α*-1,ε.ΰ*-1,ΓΤ);
Vector3 ǆ=ā(θ>0?ε.α:ε.α*-1,ε.ΰ*-1,ε.ι);ε.Ќ=(float) Math.Sqrt(Math.Pow(ΑΔ.Ќ,2)+Math.Pow(ƌ(-ΖЉ,ǆ,ε),2));for(int j=0;j<ƀ.Count;j++)
if(Vector3.Distance(ΑΔ.Δ,ƀ[j])<=Ǥ){Vector3 Ǉ=ā(θ>0?ε.α:ε.α*-1,ε.ΰ*-1,ƀ[j]-ε.Δ);float Ηα=ƌ(-Ǉ,ǆ,ε);ε.Ќ=Math.Min(ε.Ќ,Ηα)/2f;}if(Ǆ.Length()==0) Ǆ=new Vector3(0,0,1);Vector3 ΖЈ=ā(ε.α,ε.ΰ*-1,ε.ι);float Ηδ=ŏ(-Ǆ,ΖЈ,ε);float ƨ=Љ(Ηδ,đ);float ΓΡ=(float) Math.Sqrt(Љ(Ǆ.Length()*1.0f,0.5f*ƨ));
ε.Ќ=Math.Min(ε.Ќ,(ΓΤ.Length()/ΓΡ)*ǜ);}}}catch{return;}ќ+=ђ;}ќ=-1;}void ΒΕ(bool ǔ){if(ǔ){ů=0;return;}if(Ν==Κ.ή) return;float ǟ=Й*Math.Max(1,М);if(ȋ==Й) ǟ+=Math.Min(М,К);float Ην=Ϗ*Ћ*Math.Max(1,М);ů=Math.Max(ů,(float) Math.Min(ǟ/Ην*100.0,100));}MyDetectedEntityType Γσ()
{try{if(ǭ(κ,true)&&!κ.LastDetectedEntity.IsEmpty()) return κ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Ας(bool ǡ){if(Ν==Κ.ј&&Γσ()==MyDetectedEntityType.None&&!Œ(κ,true))
return fastSpeed;else return ǡ?ň:Ň;}void ΖΝ(){float Δώ=0,ΔΦ=0,č=0,ΒЊ=0;for(int i=0;i<ț.Count;i++){IMyBatteryBlock ş=ț[i];if(!ǭ(ş,true)) continue;Δώ+=ş.MaxStoredPower;ΔΦ+=ş.CurrentStoredPower;č+=ş.CurrentInput;ΒЊ+=ş.CurrentOutput;}ǰ=(float) Math.Round(Љ(ΔΦ,Δώ)*100,1);
if(č>=ΒЊ) ǝ=д.ě;else ǝ=д.Γλ;if(č==0&&ΒЊ==0||ǰ==100.0) ǝ=д.Ъ;if(ț.Count==0) ǝ=д.None;}void ΖΠ(){float Δβ=0;for(int i=0;i<ш.Count;i++){IMyGasTank ΕΜ=ш[i];if(!ǭ(ΕΜ,true)) continue;Δβ+=(float) ΕΜ.FilledRatio;}ȉ=Љ(Δβ,ш.Count)*100f;}void ΖΩ(){Ĕ=0;try{for(int i=0;i<Ȫ.Count;i++)
{IMyReactor ΓΑ=Ȫ[i];if(!ǭ(ΓΑ,true)) continue;List<MyInventoryItem>Н=new List<MyInventoryItem>();ΓΑ.GetInventory(0).GetItems(Н);float ћ=0;for(int j=0;j<Н.Count;j++){MyInventoryItem Ў=Н[j];if(Ў.Type.SubtypeId.ToUpper()=="URANIUM"&&Ў.Type.TypeId.ToUpper().Contains("_INGOT"))
ћ+=(float) Ў.Amount;}if(ћ<Ĕ||i==0){Ĕ=ћ;Βώ=ΓΑ.CustomName;}}}catch(Exception e){Ȋ=e;}}void ΖΞ(){Ķ=0;ĩ=0;try{for(int i=0;i<Ĩ.Count;i++){IMyTerminalBlock Σ=Ĩ[i];if(!ǭ(Σ,true)) continue;ĩ+=(float) Σ.GetInventory(0).CurrentVolume;Ķ+=(float) Σ.GetInventory(0).MaxVolume;
}ǃ=(float) Math.Min(Math.Round(Љ(ĩ,Ķ)*100,1),100.0);}catch(Exception e){Ȋ=e;}}void ΖΡ(){try{ǎ.Clear();for(int i=0;i<Ĩ.Count;i++){IMyTerminalBlock Σ=Ĩ[i];if(!ǭ(Σ,true)) continue;Γί(Σ,В.ő);}if(Д!=ϋ.Αύ){for(int i=0;i<ą.Count;i++){IMyTerminalBlock Σ=ą[i];if(!ǭ(Σ,true)) continue;
Γί(Σ,В.Εϋ);}}ΖΘ(ǎ);}catch(Exception e){Ȋ=e;}}void Εΰ(){if(Ƴ){if(Γς().Count>ƾ){Ƴ=false;if(б!=Ш.Ő){ς();if(б==Ш.Αι) Ȳ();if(б==Ш.ƞ)
if(Ι.β) ƒ();else Ȳ();}Ξ="Damage detected";}}}bool Αα(){if(!ƿ) return true;if(έ==Θ.Є){if(м>0&&ǝ!=д.None)
if(ǰ<=м){Ξ="Low energy! Move home";return false;}if(О>0&&Ȫ.Count>0)
if(Ĕ<=О){Ξ="Low fuel: "+Βώ;return false;}if(н>0&&ш.Count>0)
if(ȉ<=н){Ξ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Γς(){List<IMyTerminalBlock>Ċ=new List<IMyTerminalBlock>();for(int i=0;i<Ŝ.Count;i++){IMyTerminalBlock Σ=Ŝ[i];if(Œ(Σ,false)) Ċ.Add(Σ);}return Ċ;}bool Œ(IMyTerminalBlock Σ,bool ΓΚ){return(!ǭ(Σ,ΓΚ)||!Σ.IsFunctional);
}bool ǭ(IMyTerminalBlock Σ,bool ΓΚ){if(Σ==null) return false;try{IMyCubeBlock Δε=Me.CubeGrid.GetCubeBlock(Σ.Position).FatBlock;if(ΓΚ) return Δε==Σ;else return Δε.GetType()==Σ.GetType();}catch{return false;}}float Εθ(IMyTerminalBlock Σ,String т,String ΕΓ,String[]Ǡ){float Ǵ=0;for(int i=0;i<Σ.InventoryCount;i++)
{IMyInventory ΖЁ=Σ.GetInventory(i);List<MyInventoryItem>Н=new List<MyInventoryItem>();ΖЁ.GetItems(Н);foreach(MyInventoryItem Ў in Н){if(Ǡ!=null&&(Ǡ.Contains(Ў.Type.TypeId.ToUpper())||Ǡ.Contains(Ў.Type.SubtypeId.ToUpper())))
continue;if((т==""||Ў.Type.TypeId.ToUpper()==т)&&(ΕΓ==""||Ў.Type.SubtypeId.ToUpper()==ΕΓ))
Ǵ+=(float) Ў.Amount;}}return Ǵ;}ф Εο(String ρ,String ν,В Џ,bool Ζμ){ρ=ρ.ToUpper();ν=ν.ToUpper();for(int i=0;i<ǎ.Count;i++){ф Ў=ǎ[i];if(Ў.ρ.ToUpper()==ρ&&Ў.ν.ToUpper()==ν&&(Ў.ĳ==Џ||Џ==В.Ȱ))
return Ў;}ф Ϊ=null;if(Ζμ){Ϊ=new ф(ρ,ν,0,Џ);ǎ.Add(Ϊ);}return Ϊ;}int ǻ(String ρ,String ν,В Џ){return ǻ(ρ,ν,Џ,null);}int ǻ(String ρ,String ν,В Џ,String[]Ǡ){int Ǵ=0;ρ=ρ.ToUpper();ν=ν.ToUpper();for(int i=0;i<ǎ.Count;i++){ф Ў=ǎ[i];if(Ǡ!=null&&Ǡ.Contains(Ў.ρ.ToUpper())) continue;
if((ρ==""||Ў.ρ.ToUpper()==ρ)&&(ν==""||Ў.ν.ToUpper()==ν)&&(Ў.ĳ==Џ||Џ==В.Ȱ))
Ǵ+=Ў.ћ;}return Ǵ;}void Γί(IMyTerminalBlock Σ,В Џ){for(int i=0;i<Σ.InventoryCount;i++){List<MyInventoryItem>Н=new List<MyInventoryItem>();Σ.GetInventory(i).GetItems(Н);for(int j=0;j<Н.Count;j++)
Εο(Н[j].Type.SubtypeId,Н[j].Type.TypeId.Replace("MyObjectBuilder_",""),Џ,true).ћ+=(int) Н[j].Amount;}}void ΖΘ(List<ф>а){for(int i=а.Count-1;i>0;i--)
for(int j=0;j<i;j++){ф a=а[j];ф b=а[j+1];if(a.ћ<b.ћ) а.Move(j,j+1);}}bool œ(bool ΘΔ){if(ȓ&&Ν!=Κ.ή)
if(Е!=-1&&đ>=Е){Ξ="Ship too heavy";return true;}if(ǃ>=Ǣ||ż){ż=false;Ξ="Ship is full";return true;}return false;}bool Γό(){String[]Ǡ=null;if(!Ȓ) Ǡ=new string[]{"ICE"};if(Ν==Κ.Ŗ) return ǻ("","ORE",В.ő,Ǡ)==0;if(Ν==Κ.ј) return ǻ("","COMPONENT",В.ő,Ǡ)==0;else return ǻ("","",В.ő,Ǡ)==0;
}bool ΕЀ(){List<IMyCockpit>ΒΫ=new List<IMyCockpit>();ψ.GetBlocksOfType(ΒΫ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΒΫ.Count;i++)
if(ΒΫ[i].IsUnderControl) return true;return false;}bool Εα(bool ǔ,bool ΑΛ){IMyShipConnector ѝ=Щ(MyShipConnectorStatus.Connected);if(ѝ==null) return false;if(Vector3.Distance(Ι.Δ,ѝ.GetPosition())<5)
return Γη(υ,ǔ,ΑΛ);if(Vector3.Distance(Ε.Δ,ѝ.GetPosition())<5)
return Γη(φ,ǔ,ΑΛ);return false;}bool Γη(Ġ ǲ,bool ǔ,bool ΑΛ){if(ǔ) ǲ.ΕЊ();ǲ.ΖΦ();bool в=false;String с="";switch(ǲ.Ј){case δ.ΑϏ:{с="Waiting for command";в=false;break;}case δ.ΓЃ:{с="Waiting for cargo";в=œ(true);break;}case δ.ΓЂ:{с="Unloading";в=Γό();break;}case δ.ΑЄ:{в=true;break;}
case δ.Αώ:{с="Charging batteries";в=ǰ>=100f;break;}case δ.Ɯ:{с="Discharging batteries";в=ǰ<=25f;break;}case δ.ƛ:{с="Discharging batteries";в=ǰ<=0f;break;}case δ.ΑЁ:{с="Filling up hydrogen";в=ȉ>=100f;break;}case δ.ΑЂ:{с="Unloading hydrogen";в=ȉ<=25f;break;}
case δ.ΑЀ:{с="Unloading hydrogen";в=ȉ<=0f;break;}case δ.ΑЃ:{bool Δμ=ΕЀ();if(!Δμ) ǲ.ΒЏ=true;в=ǲ.ΒЏ&&Δμ;с="Waiting for passengers";break;}}if(!в) ǲ.Αΰ(true);if(в&&ǲ.Ατ()){в=ǲ.Αΰ(false);с="Undocking in: "+Αλ((int) Math.Max(0,ǲ.ў-ǲ.ȟ));}if(ΑΛ) ΕΨ=с;return в;}void ΕΫ()
{if(!į) return;if(ξ.Count<=1) return;float ΕΡ=0,ΕΠ=0;for(int i=0;i<ξ.Count;i++){IMyTerminalBlock Ǧ=ξ[i];if(Œ(Ǧ,true)) continue;ΕΡ+=(float) Ǧ.GetInventory(0).MaxVolume;ΕΠ+=(float) Ǧ.GetInventory(0).CurrentVolume;}float ΔΙ=(float) Math.Round(Љ(ΕΠ,ΕΡ),5);for(int ΔЈ=0;ΔЈ<Math.Max(1,Math.Floor(ξ.Count/10f));ΔЈ++)
{float Βο=0,Βύ=0;float Δκ=0,Δό=0;IMyTerminalBlock ȇ=null,Ƞ=null;for(int i=0;i<ξ.Count;i++){IMyTerminalBlock Ǧ=ξ[i];if(Œ(Ǧ,true)) continue;float ΒЄ=(float) Ǧ.GetInventory(0).MaxVolume;float Ƹ=Љ((float) Ǧ.GetInventory(0).CurrentVolume,ΒЄ);if(ȇ==null||Ƹ>Βο){ȇ=Ǧ;Βο=Ƹ;Δκ=ΒЄ;}
if(Ƞ==null||Ƹ<Βύ){Ƞ=Ǧ;Βύ=Ƹ;Δό=ΒЄ;}}if(ȇ==null||Ƞ==null||ȇ==Ƞ) return;if(checkConveyorSystem&&!ȇ.GetInventory(0).IsConnectedTo(Ƞ.GetInventory(0))){if(ΒΡ>20) Ξ="Inventory balancing failed";else ΒΡ++;return;}ΒΡ=0;List<MyInventoryItem>Н=new List<MyInventoryItem>();
ȇ.GetInventory(0).GetItems(Н);float Ƈ=0;if(Н.Count==0) continue;MyInventoryItem ƹ=Н[0];String Δο=ƹ.Type.TypeId+ƹ.Type.SubtypeId;if(!ΕΧ.TryGetValue(Δο,out Ƈ)){if(ΕЃ(ȇ.GetInventory(0),0,Ƞ.GetInventory(0),out Ƈ))
ΕΧ.Add(Δο,Ƈ);else return;}float Ηλ=((Βο-ΔΙ)*Δκ/Ƈ);float Ημ=((ΔΙ-Βύ)*Δό/Ƈ);int ΓΥ=(int) Math.Min(Ημ,Ηλ);if(ΓΥ<=0) return;if((float) ƹ.Amount<ΓΥ)
ȇ.GetInventory(0).TransferItemTo(Ƞ.GetInventory(0),0,null,null,ƹ.Amount);else
ȇ.GetInventory(0).TransferItemTo(Ƞ.GetInventory(0),0,null,null,ΓΥ);}}bool ΕЃ(IMyInventory Ž,int Α,IMyInventory Ζο,out float ΕΦ){ΕΦ=0;float Ζβ=(float) Ž.CurrentVolume;List<MyInventoryItem>ΒΣ=new List<MyInventoryItem>();Ž.GetItems(ΒΣ);float ΔΛ=0;for(int i=0;i<ΒΣ.Count;i++) ΔΛ+=(float) ΒΣ[i].Amount;
Ž.TransferItemTo(Ζο,Α,null,null,1);float ΕΥ=Ζβ-(float) Ž.CurrentVolume;List<MyInventoryItem>ΒΙ=new List<MyInventoryItem>();Ž.GetItems(ΒΙ);float ΓБ=0;for(int i=0;i<ΒΙ.Count;i++) ΓБ+=(float) ΒΙ[i].Amount;if(ΕΥ==0f||!Αχ(0.9999,ΔΛ-ΓБ,1.0001))
return false;ΕΦ=ΕΥ;return true;}void ΖΟ(){Vector3 Ū=ω+ΑΣ;Vector3 ǥ=Ū-γ;if(ǥ.Length()==0) ǥ=new Vector3(0,0,-1);Vector3 Ǉ=Т(Υ,ǥ);Vector3 ΕΝ=Vector3.Normalize(Ǉ);Vector3 Ĳ=Т(Υ,Υ.GetNaturalGravity());float ΗΫ=ΒЍ>0?Math.Max(0,1-Ě(ǥ,Δδ)/5):0;float Ќ=(float) Math.Min((ΒЁ>0?ΒЁ:1000f),Math.Max(ƌ(-Ǉ,Ĳ,null),ΒЍ*ΗΫ));
if(!ΕΟ) Ќ=0;if(ΔБ) Ќ=Math.Max(0,1-Ʊ/Ʃ)*Ќ;if(generalSpeedLimit>0) Ќ=Math.Min(generalSpeedLimit,Ќ);if(Ļ) Ќ=0;if(ń>=0) Ќ=Math.Min(Ќ,ń);if(ΕΑ) Ќ*=(float) Math.Min(1,Љ(ǥ.Length(),wpReachedDist)/2);Vector3 Δω=Т(Υ,Υ.GetShipVelocities().LinearVelocity);float Ζή=(float)(Math.Max(0,15-Ě(-ΕΝ,-Δω))/15)*0.85f+0.15f;
ΓΞ+=Math.Sign(Ζή-ΓΞ)/10f;Vector3 ȝ=ΕΝ*Ќ*ΓΞ-(Δω);Vector3 ΒΠ=Αρ(ȝ,null);if(ΑΟ&&Г>0.1f){ȝ.X*=ΑЏ(ȝ.X,ref ΑΠ.X,1f,ΒΠ.X,20);ȝ.Y*=ΑЏ(ȝ.Y,ref ΑΠ.Y,1f,ΒΠ.Y,20);ȝ.Z*=ΑЏ(ȝ.Z,ref ΑΠ.Z,1f,ΒΠ.Z,20);}else ΑΠ=new Vector3(1,1,1);Ɓ=đ*ȝ-Ĳ*đ;Γΰ(Ɓ,ΑΟ);Г=Vector3.Distance(γ,Ū);
}float ΑЏ(float value,ref float Л,float ђ,float Ȍ,float ΒЂ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Л)) Л+=Math.Sign(Л)*ђ;else Л=value;if(value==0) Л=1;float Ϊ=Math.Abs(Л);if(Ϊ<ΒЂ||Ȍ==0) return Ϊ;Л=Math.Min(ΒЂ,Math.Max(-ΒЂ,Л));Ϊ=Math.Abs(Ȍ);return Ϊ;
}void ΓА(){float п=90,і=90,я=90;float Ǖ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǡ,ы,ų,Βϊ;if(Βϋ){ǡ=Vector3.Normalize(ω-γ);ų=Т(Υ,ǡ);ы=Т(Υ,ΓΜ);п=Ě(ų,new Vector3(0,-1,0))-90;і=ȱ(ы,new Vector3(-1,0,0),ы.Y);
я=ȱ(ų,new Vector3(-1,0,0),ų.Z);}else{ǡ=ΓΝ;ы=Т(Υ,ΓΜ);ų=Т(Υ,ΓΝ);Βϊ=Т(Υ,ΕΞ);п=ȱ(ы,new Vector3(0,0,1),ы.Y);і=ȱ(ы,new Vector3(-1,0,0),ы.Y);я=ȱ(Βϊ,new Vector3(0,0,1),Βϊ.X);}if(ΔΒ&&ƕ()){Vector3 Βκ=Υ.GetNaturalGravity();ы=Т(Υ,Βκ);п=ȱ(ы,new Vector3(0,0,1),ы.Y);
і=ȱ(ы,new Vector3(-1,0,0),ы.Y);}if(!Αχ(-45,і,45)){п=0;я=0;}if(!Αχ(-45,я,45)) п=0;Ơ(Βμ,1,(-п)*Ǖ,(-я)*Ǖ,(-і)*Ǖ);Ʊ=Math.Max(Math.Abs(п),Math.Max(Math.Abs(і),Math.Abs(я)));ǀ=Ʊ<=Ʃ;}void Εί(IMyTerminalBlock ΑΕ){if(ΑΕ==null) return;ǘ=new float[3,2];ѓ=new Dictionary<string,float[,]>();
for(int i=0;i<Ǹ.Count;i++){IMyThrust Ф=Ǹ[i];if(!Ф.IsFunctional) continue;Vector3 θ=Т(ΑΕ,Ф.WorldMatrix.Backward);float Ȟ=Ф.MaxEffectiveThrust;if(Math.Round(θ.X,2)!=0.0)
if(θ.X>=0) ǘ[0,0]+=Ȟ;else ǘ[0,1]-=Ȟ;if(Math.Round(θ.Y,2)!=0.0)
if(θ.Y>=0) ǘ[1,0]+=Ȟ;else ǘ[1,1]-=Ȟ;if(Math.Round(θ.Z,2)!=0.0)
if(θ.Z>=0) ǘ[2,0]+=Ȟ;else ǘ[2,1]-=Ȟ;String т=Γυ(Ф);float[,]є=null;if(ѓ.ContainsKey(т)) є=ѓ[т];else{є=new float[3,2];ѓ.Add(т,є);}float Ȍ=Ф.MaxThrust;if(Math.Round(θ.X,2)!=0.0)
if(θ.X>=0) є[0,0]+=Ȍ;else є[0,1]-=Ȍ;if(Math.Round(θ.Y,2)!=0.0)
if(θ.Y>=0) є[1,0]+=Ȍ;else є[1,1]-=Ȍ;if(Math.Round(θ.Z,2)!=0.0)
if(θ.Z>=0) є[2,0]+=Ȍ;else є[2,1]-=Ȍ;}}static String Γυ(IMyThrust Ф){return Ф.BlockDefinition.SubtypeId;}Vector3 Αω(Vector3 ч,float[,]Ĵ){return new Vector3(
ч.X>=0?Ĵ[0,0]:Ĵ[0,1],
ч.Y>=0?Ĵ[1,0]:Ĵ[1,1],
ч.Z>=0?Ĵ[2,0]:Ĵ[2,1]);}bool Γτ(Ω Φ,String т,out float Βδ){Βδ=0;int Α=Ƿ.IndexOf(т);if(Α==-1||Φ.Ǘ==null||Α>=Φ.Ǘ.Length)
return false;Βδ=Φ.Ǘ[Α];if(Βδ==-1) return false;return true;}Vector3 Αρ(Vector3 ч,Ω Φ){if(Φ!=null){Vector3 Ϊ=new Vector3();for(int i=0;i<ѓ.Keys.Count;i++){String т=ѓ.Keys.ElementAt(i);float Ũ=0;if(!Γτ(Φ,т,out Ũ))
return Αω(ч,ǘ);Ϊ+=Αω(ч,ѓ.Values.ElementAt(i))*Ũ;}return Ϊ;}return Αω(ч,ǘ);}float ŏ(Vector3 ч,Ω Φ){return ŏ(ч,new Vector3(),Φ);}float ŏ(Vector3 ч,Vector3 ι,Ω Φ){Vector3 Ηγ=Αρ(ч,Φ);Vector3 Ȟ=Ηγ+ι*đ;float Ÿ=(Ȟ/ч).AbsMin();return(float)(ч*Ÿ).Length();}float ƌ(Vector3 ǅ,Vector3 Ĳ,Ω Φ)
{if(ǅ.Length()==0) return 0;float ΔΑ=1;if(Ĳ.Length()>0) ΔΑ=Math.Min(1,Ě(-Ĳ,ǅ)/90)*0.4f+0.6f;float Ɓ=ŏ(ǅ,Ĳ,Φ);if(Ɓ==0) return 0.1f;float ƨ=Љ(Ɓ,đ);float ΓΡ=(float) Math.Sqrt(Љ(ǅ.Length(),ƨ*0.5f));return ƨ*ΓΡ*ΔΑ*ǜ;}float Ɠ(Vector3 ǡ,Vector3 Ц,Vector3 ι,Ω Φ){if(ι.Length()==0f) return 0;
Vector3 ǆ=ā(ǡ,Ц,Vector3.Normalize(ι));float Ǎ=ŏ(-ǆ,Φ);return Ǎ/ι.Length();}void Γγ(bool ΖЀ){float ѕ=0;float Βη=0.9f;if(ΖЀ){Е=-1;ĕ=0;ΑΝ=null;if(έ!=Θ.Ы&&Ε.ι.Length()!=0){ѕ=Βη*Ɠ(Ε.α,Ε.ΰ*-1,Ε.ι,null);if(ѕ<Е||Е==-1) Е=ѕ;}if(Ι.β&&Ι.ι.Length()!=0){ѕ=Βη*Ɠ(Ι.α,Ι.ΰ*-1,Ι.ι,null);
if(ѕ<Е||Е==-1) Е=ѕ;}return;}if(ĕ==-1) return;if(ĕ>=0){int ΔЊ=0;while(ĕ<Τ.Count){if(ΔЊ>100) return;ΔЊ++;Ω Φ=Τ[ĕ];if(Φ.ι.Length()!=0f){ѕ=Βη*Math.Min(
Ɠ(Φ.α,Φ.ΰ*-1,Φ.ι,Φ),
Ɠ(Φ.α*-1,Φ.ΰ*-1,Φ.ι,Φ));if(ѕ<Е||Е==-1) Е=ѕ;}else ΑΝ=Φ;ĕ++;}ĕ=-1;}bool Βν=true;float Ȩ=0;if(ΑΝ!=null){for(int i=0;i<ѓ.Count;i++){String т=ѓ.Keys.ElementAt(i);float[,]є=ѓ.Values.ElementAt(i);float Ũ=0;if(!Γτ(ΑΝ,т,out Ũ)){Βν=false;break;}for(int a=0;a<є.GetLength(0);a++)
for(int b=0;b<є.GetLength(1);b++){float Ą=Math.Abs(є[a,b]*Ũ);if(Ą==0) continue;Βν=true;if(Ȩ==0||Ą<Ȩ) Ȩ=Ą;}}}if(!Βν){for(int a=0;a<ǘ.GetLength(0);a++)
for(int b=0;b<ǘ.GetLength(1);b++){float Ą=Math.Abs(ǘ[a,b]);if(Ą==0) continue;if(Ȩ==0||Ą<Ȩ) Ȩ=Ą;}}if(Ȩ>0){float ΖЍ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ѕ=Љ(Ȩ,ΖЍ);if(ѕ>0)
if(ѕ<Е||Е==-1) Е=ѕ;}}void Γΰ(Vector3 Ǎ,bool Ё){if(!Ё){for(int i=0;i<Ǹ.Count;i++)
Ǹ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒΟ=Αρ(Ǎ,null);float Ηϊ=Math.Min(1,Math.Abs(Љ(Ǎ.X,ΒΟ.X)));float Ηϋ=Math.Min(1,Math.Abs(Љ(Ǎ.Y,ΒΟ.Y)));float Ηό=Math.Min(1,Math.Abs(Љ(Ǎ.Z,ΒΟ.Z)));for(int i=0;i<Ǹ.Count;i++){IMyThrust Ф=Ǹ[i];Vector3 θ=ΕЌ(Т(Υ,Ф.WorldMatrix.Backward),1);
if(θ.X!=0&&Math.Sign(θ.X)==Math.Sign(Ǎ.X))
Ф.SetValueFloat("Override",Ф.MaxThrust*Ηϊ);else if(θ.Y!=0&&Math.Sign(θ.Y)==Math.Sign(Ǎ.Y))
Ф.SetValueFloat("Override",Ф.MaxThrust*Ηϋ);else if(θ.Z!=0&&Math.Sign(θ.Z)==Math.Sign(Ǎ.Z))
Ф.SetValueFloat("Override",Ф.MaxThrust*Ηό);else
Ф.SetValueFloat("Override",0.0f);}}void Ơ(bool Ё,float ΗΘ,float п,float я,float і){for(int i=0;i<ƻ.Count;i++){IMyGyro ώ=ƻ[i];ώ.GyroOverride=Ё;if(!Ё) ώ.GyroPower=100;else ώ.GyroPower=ΗΘ;if(!Ё) continue;Vector3 ǵ=Υ.WorldMatrix.Forward;Vector3 р=Υ.WorldMatrix.Right;Vector3 Ц=Υ.WorldMatrix.Up;
Vector3 ī=ώ.WorldMatrix.Forward;Vector3 ĭ=ώ.WorldMatrix.Up;Vector3 Ĭ=ώ.WorldMatrix.Left*-1;if(ī==ǵ) ώ.SetValueFloat("Roll",і);else if(ī==(ǵ*-1)) ώ.SetValueFloat("Roll",і*-1);else if(ĭ==(ǵ*-1)) ώ.SetValueFloat("Yaw",і);else if(ĭ==ǵ) ώ.SetValueFloat("Yaw",і*-1);else if(Ĭ==ǵ) ώ.SetValueFloat("Pitch",і);
else if(Ĭ==(ǵ*-1)) ώ.SetValueFloat("Pitch",і*-1);if(Ĭ==(р*-1)) ώ.SetValueFloat("Pitch",п);else if(Ĭ==р) ώ.SetValueFloat("Pitch",п*-1);else if(ĭ==р) ώ.SetValueFloat("Yaw",п);else if(ĭ==(р*-1)) ώ.SetValueFloat("Yaw",п*-1);else if(ī==(р*-1)) ώ.SetValueFloat("Roll",п);
else if(ī==р) ώ.SetValueFloat("Roll",п*-1);if(ĭ==(Ц*-1)) ώ.SetValueFloat("Yaw",я);else if(ĭ==Ц) ώ.SetValueFloat("Yaw",я*-1);else if(Ĭ==Ц) ώ.SetValueFloat("Pitch",я);else if(Ĭ==(Ц*-1)) ώ.SetValueFloat("Pitch",я*-1);else if(ī==Ц) ώ.SetValueFloat("Roll",я);else if(ī==(Ц*-1)) ώ.SetValueFloat("Roll",я*-1);
}}void ΓЍ(){this.Βμ=false;}void Р(Vector3 ť,Vector3 ǵ,Vector3 Βω,float ȑ,bool Ɔ){ΑΩ(ť,ȑ,Ɔ);Ʃ=ȑ;Βϋ=false;this.ΓΝ=ǵ;this.ΕΞ=Βω;}void Р(Vector3 ť,Vector3 ǵ,Vector3 Βω,bool Ɔ){Р(ť,ǵ,Βω,2f,Ɔ);}void ΑΩ(Vector3 ť,float ȑ,bool Ɔ){Ʃ=ȑ;this.Βμ=true;this.ΔΒ=Ɔ;Βϋ=true;ǀ=false;
this.ΓΜ=ť;}void ΒΓ(){ǚ(false,false,false,ω,0);ΑΟ=false;}void Ь(Vector3 И,float Ǖ){ǚ(true,false,false,И,Ǖ);}void ǚ(bool Ё,bool ΓΘ,bool ΓΩ,Vector3 И,float Ǖ){ǚ(Ё,ΓΘ,ΓΩ,И,И-γ,0.0f,Ǖ);}void ǚ(bool Ё,bool ΓΘ,bool ΓΩ,Vector3 И,Vector3 ΗΥ,float ΗΦ,float Ǖ){ΑΟ=true;this.ΕΟ=Ё;
ω=И;this.ΒЁ=Ǖ;this.ΒЍ=ΗΦ;this.ΕΑ=ΓΘ;this.ΔБ=ΓΩ;this.Δδ=ΗΥ;Г=Vector3.Distance(И,γ);}void ΕЍ(){ψ.GetBlocksOfType(Ŝ,љ);}void ΕЎ(){if(ΑΗ){ό=2;return;}List<IMyRemoteControl>Ź=new List<IMyRemoteControl>();List<IMySensorBlock>ΔЏ=new List<IMySensorBlock>();List<IMyTerminalBlock>ΕΕ=new List<IMyTerminalBlock>();
ψ.GetBlocksOfType(Ź,љ);ψ.GetBlocksOfType(ъ,љ);ψ.GetBlocksOfType(ΔЏ,љ);ψ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ΕΕ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ъ=Ǚ(ъ,pamTag,true);ΖΓ();ΖΔ(ΕΕ);ŗ(ъ,setLCDFontAndSize,1.4f,false);ŗ(Ȭ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>ΕΙ=Ǚ(ΔЏ,pamTag,true);if(ΕΙ.Count>0) κ=ΕΙ[0];else κ=null;if(Ν==Κ.Ŗ){ψ.GetBlocksOfType(ξ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ξ.Count==0){ό=1;Ξ="Drills are missing";}}else if(Ν==Κ.ј){ψ.GetBlocksOfType(ξ,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ξ.Count==0){ό=1;Ξ="Grinders are missing";}if(κ==null){ό=1;Ξ="Sensor is missing";}}else if(Ν==Κ.ή){ψ.GetBlocksOfType(ΓΣ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>ΕΘ=Ǚ(Ź,pamTag,true);if(ΕΘ.Count>0) Ź=ΕΘ;if(Ź.Count>0) Υ=Ź[0];else{Υ=null;ό=2;Ξ="Remote is missing";return;}η=(float) Υ.CubeGrid.WorldVolume.Radius*2;ю=null;if(Ν!=Κ.ή){Γδ();if(ξ.Count>0&&ю!=null){if(κ!=null&&(ю.WorldMatrix.Forward!=κ.WorldMatrix.Forward||
!(Υ.WorldMatrix.Forward==κ.WorldMatrix.Up||Υ.WorldMatrix.Down==κ.WorldMatrix.Down))){ό=1;Ξ="Wrong sensor direction";}if(ю.WorldMatrix.Forward!=Υ.WorldMatrix.Forward&&
ю.WorldMatrix.Forward!=Υ.WorldMatrix.Down){ό=2;Ξ="Wrong remote direction";}}}}void ΕЏ(){ψ.GetBlocksOfType(Ƕ,љ);ψ.GetBlocksOfType(и,љ);ψ.GetBlocksOfType(Ǹ,љ);ψ.GetBlocksOfType(ƻ,љ);ψ.GetBlocksOfType(ț,љ);ψ.GetBlocksOfType(Ȫ,љ);ψ.GetBlocksOfType(ш,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ψ.GetBlocksOfType(ΑΜ,љ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
и=Ǚ(и,"ConnectorMedium",false);else
и=Ǚ(и,"Connector",false);List<IMyShipConnector>ΕΖ=Ǚ(и,pamTag,true);if(ΕΖ.Count>0) и=ΕΖ;if(ό<=1){if(и.Count==0){ό=1;Ξ="Connector is missing";}if(ƻ.Count==0){ό=1;Ξ="Gyros are missing";}if(Ǹ.Count==0){ό=1;Ξ="Thrusters are missing";}}List<IMyConveyorSorter>ΕΚ=Ǚ(ΑΜ,pamTag,true);
if(ΕΚ.Count>0) ΑΜ=ΕΚ;List<IMyLandingGear>ΕΗ=Ǚ(Ƕ,pamTag,true);if(ΕΗ.Count>0) Ƕ=ΕΗ;for(int i=0;i<Ƕ.Count;i++) Ƕ[i].AutoLock=false;List<IMyBatteryBlock>ΕΔ=Ǚ(ț,pamTag,true);if(ΕΔ.Count>0) ț=ΕΔ;List<IMyGasTank>ΕΛ=Ǚ(ш,pamTag,true);if(ΕΛ.Count>0) ш=ΕΛ;}void ΕА(){ψ.GetBlocksOfType(ą,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ĩ.Clear();for(int i=ą.Count-1;i>=0;i--){if(Εύ(ą[i])){Ĩ.Add(ą[i]);ą.RemoveAt(i);}}}bool Εύ(IMyTerminalBlock Σ){if(Σ is IMyCargoContainer) return true;if(Σ is IMyShipDrill) return true;if(Σ is IMyShipGrinder) return true;if(Σ is IMyShipConnector){if(((IMyShipConnector) Σ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Αυ(Σ,"ConnectorSmall",false))
return false;else return true;}return false;}void Γδ(){if(Υ==null) return;ю=null;float ΑΑ=0,ǈ=0,ΑΒ=0,ǉ=0,ΑΓ=0,Ǌ=0;List<IMyTerminalBlock>ΓΛ=Ǚ(ξ,pamTag,true);bool ΗΓ=ΓΛ.Count==0;if(ΓΛ.Count>0) ю=ΓΛ[0];else if(ξ.Count>0) ю=ξ[0];int Ǵ=0;for(int i=0;i<ξ.Count;i++)
{if(ξ[i].WorldMatrix.Forward!=ю.WorldMatrix.Forward){if(ΗΓ){ό=2;Ξ="Mining direction is unclear!";return;}continue;}Ǵ++;Vector3 Ђ=Αξ(Υ,ξ[i].GetPosition());if(i==0){ΑΑ=Ђ.X;ǈ=Ђ.X;ΑΒ=Ђ.Y;ǉ=Ђ.Y;ΑΓ=Ђ.Z;Ǌ=Ђ.Z;}ǈ=Math.Max(Ђ.X,ǈ);ΑΑ=Math.Min(Ђ.X,ΑΑ);ǉ=Math.Max(Ђ.Y,ǉ);ΑΒ=Math.Min(Ђ.Y,ΑΒ);
Ǌ=Math.Max(Ђ.Z,Ǌ);ΑΓ=Math.Min(Ђ.Z,ΑΓ);}ǹ=(ǈ-ΑΑ)*(1-Ė)+drillRadius*2;ǧ=(ǉ-ΑΒ)*(1-Ȣ)+drillRadius*2;if(ю!=null&&ю.WorldMatrix.Forward==Υ.WorldMatrix.Down)
ǧ=(Ǌ-ΑΓ)*(1-Ȣ)+drillRadius*2;}List<T>Ǚ<T>(List<T>а,String ǖ,bool Ƽ){List<T>Ϊ=new List<T>();for(int i=0;i<а.Count;i++)
if(Αυ(а[i],ǖ,Ƽ)) Ϊ.Add(а[i]);return Ϊ;}bool Αυ<T>(T Σ,String ǖ,bool Ƽ){IMyTerminalBlock b=(IMyTerminalBlock) Σ;if(Ƽ&&b.CustomName.ToUpper().Contains(ǖ.ToUpper())) return true;if(!Ƽ&&b.BlockDefinition.ToString().ToUpper().Contains(ǖ.ToUpper())) return true;return false;}void х<T>(List<T>а,bool Ё)
{for(int i=0;i<а.Count;i++)
ΓЈ((IMyTerminalBlock) а[i],Ё);}void ΓЈ(IMyTerminalBlock Σ,bool Ё){if(Σ==null) return;String Ζά=Ё?"OnOff_On":"OnOff_Off";var a=Σ.GetActionWithName(Ζά);a.Apply(Σ);}void Ǜ(bool Ё){ΓΙ=Ё;if(!Ē) return;х(ΑΜ,Ё);}void ΑЋ(ChargeMode Ŵ){for(int i=0;i<ț.Count;i++) ț[i].ChargeMode=Ŵ;}void ΑЌ(bool Ηή)
{for(int i=0;i<ш.Count;i++) ш[i].Stockpile=Ηή;}void Ř(List<IMyLandingGear>Βθ,bool Δϊ){for(int i=0;i<Βθ.Count;i++){if(Δϊ) Βθ[i].Lock();if(!Δϊ) Βθ[i].Unlock();}}void ΓЉ(bool Ё){if(ΔΧ==Ё) return;List<IMyShipController>Ư=new List<IMyShipController>();ψ.GetBlocksOfType(Ư,љ);if(Ư.Count==0) return;
for(int i=0;i<Ư.Count;i++)
Ư[i].DampenersOverride=Ё;ΔΧ=Ё;}IMyShipConnector Щ(MyShipConnectorStatus с){for(int i=0;i<и.Count;i++){if(!ǭ(и[i],true)) continue;if(и[i].Status==с) return и[i];}return null;}void Ă(bool ΓΕ,bool Ƶ,float Ζχ,float Ζα){if(κ==null||ξ.Count==0) return;Vector3 Ʈ=new Vector3();int Ǵ=0;
for(int i=0;i<ξ.Count;i++){if(ξ[i].WorldMatrix.Forward!=ю.WorldMatrix.Forward) continue;Ǵ++;Ʈ+=ξ[i].GetPosition();}Ʈ=Ʈ/Ǵ;Vector3 ĸ=Αξ(κ,Ʈ);κ.Enabled=true;κ.ShowOnHUD=ΓΕ;κ.LeftExtend=(Ƶ?1:Ю)/2f*ǹ-ĸ.X;κ.RightExtend=(Ƶ?1:Ю)/2f*ǹ+ĸ.X;κ.TopExtend=(Ƶ?1:Э)/2f*ǧ+ĸ.Y;
κ.BottomExtend=(Ƶ?1:Э)/2f*ǧ-ĸ.Y;κ.FrontExtend=(ΓΕ?М:Ζχ)-ĸ.Z;κ.BackExtend=ΓΕ?0:Ζα+η*0.75f+ĸ.Z;κ.DetectFloatingObjects=true;κ.DetectAsteroids=false;κ.DetectLargeShips=true;κ.DetectSmallShips=true;κ.DetectStations=true;κ.DetectOwner=true;κ.DetectSubgrids=false;
κ.DetectPlayers=false;κ.DetectEnemy=true;κ.DetectFriendly=true;κ.DetectNeutral=true;}void ŗ<T>(List<T>а,bool ΗΨ,float Ζυ,bool ΖЂ){for(int i=0;i<а.Count;i++){IMyTextSurface ſ=null;if(а[i]is IMyTextSurface) ſ=(IMyTextSurface) а[i];if(ſ!=null){ſ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΗΨ) continue;ſ.Font="Debug";if(ΖЂ) continue;ſ.FontSize=Ζυ;}}}bool ƕ(){Vector3D ϊ;return this.Υ.TryGetPlanetPosition(out ϊ);}int Εφ(Vector3 Ηΰ,int ΕΒ){if(ΕΒ==-1) return 0;double Ѝ=-1;int Ȇ=-1;for(int i=Τ.Count-1;i>=0;i--){double χ=Vector3.Distance(Τ[i].Δ,Ηΰ);if(Ѝ==-1||χ<Ѝ){Ȇ=i;Ѝ=χ;}
}return Math.Sign(Ȇ-ΕΒ);}bool ΕϏ(Vector3 ϊ){List<Vector3>Б=new List<Vector3>();for(int i=0;i<Τ.Count;i++) Б.Add(Τ[i].Δ);if(Ι.β&&Τ.Count>=1){Vector3 ģ=new Vector3();Ǭ(Ι,dockDist*η,false,out ģ);if(Vector3.Distance(Ι.Δ,Τ.First().Δ)<Vector3.Distance(Ι.Δ,Τ.Last().Δ)){Б.Insert(0,ģ);Б.Insert(0,Ι.Δ);}
else{Б.Add(ģ);Б.Add(Ι.Δ);}}if(Ν==Κ.ή){if(Ε.β&&Τ.Count>=1){Vector3 ΒΛ=new Vector3();Ǭ(Ε,dockDist*η,false,out ΒΛ);if(Vector3.Distance(Ε.Δ,Τ.First().Δ)<Vector3.Distance(Ε.Δ,Τ.Last().Δ)){Б.Insert(0,ΒΛ);Б.Insert(0,Ε.Δ);}else{Б.Add(ΒΛ);Б.Add(Ε.Δ);}}}else{if(έ!=Θ.Ы)
if(Τ.Count>0&&Vector3.Distance(Ε.Δ,Τ.First().Δ)<Vector3.Distance(Ε.Δ,Τ.Last().Δ))
Б.Insert(0,Ε.Δ);else Б.Add(Ε.Δ);}int Ȇ=-1;double Ǟ=-1;for(int i=0;i<Б.Count;i++){double χ=Vector3.Distance(Б[i],ϊ);if(χ<Ǟ||Ǟ==-1){Ǟ=χ;Ȇ=i;}}if(Б.Count==0) return false;double Βα=Vector3.Distance(Б[Ȇ],ϊ);double ΗΙ=Vector3.Distance(Б[Math.Max(0,Ȇ-1)],Б[Ȇ])*1.5f;
double ΗΒ=Vector3.Distance(Б[Math.Min(Б.Count-1,Ȇ+1)],Б[Ȇ])*1.5f;return Βα<ΗΙ||Βα<ΗΒ||Βα<laneOffset*η*1.5f;}static float Љ(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΕЌ(Vector3 v,int Βή){return new Vector3(Math.Round(v.X,Βή),Math.Round(v.Y,Βή),Math.Round(v.Z,Βή));
}float Ě(Vector3 a,Vector3 b){if(a==b) return 0;float Ζτ=(a*b).Sum;float Δτ=a.Length();float Δυ=b.Length();if(Δτ==0||Δυ==0) return 0;float Ϊ=(float)((180.0f/Math.PI)*Math.Acos(Ζτ/(Δτ*Δυ)));if(float.IsNaN(Ϊ)) return 0;return Ϊ;}float ȱ(Vector3 v,Vector3 ΑΕ,float ΗΩ){float ġ=Ě(v,ΑΕ);
if(ΗΩ>0f) ġ*=-1;if(ġ>-90f) return ġ-90f;else return 180f-(-ġ-90f);}double Γκ(float Ζν){return(Math.PI/180)*Ζν;}bool Αχ(double ΖЌ,double value,double ΖЊ){return(value>=ΖЌ&&value<=ΖЊ);}Vector3 ΖΜ(IMyTerminalBlock Σ,Vector3 Ђ){return Vector3D.Transform(Ђ,Σ.WorldMatrix);
}Vector3 Αξ(IMyTerminalBlock Σ,Vector3 Ηχ){return Т(Σ,Ηχ-Σ.GetPosition());}Vector3 Т(IMyTerminalBlock Σ,Vector3 Γή){return Vector3D.TransformNormal(Γή,MatrixD.Transpose(Σ.WorldMatrix));}Vector3 ā(Vector3 ǡ,Vector3 Ц,Vector3 Γή){MatrixD ΒЀ=MatrixD.CreateFromDir(ǡ,Ц);return Vector3D.TransformNormal(Γή,MatrixD.Transpose(ΒЀ));
}Vector3 ΒΔ(Vector3 ǡ,Vector3 Ц,Vector3 Ђ){MatrixD ΒЀ=MatrixD.CreateFromDir(ǡ,Ц);return Vector3D.Transform(Ђ,ΒЀ);}String І(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 Ѕ(String s){String[]А=s.Split('|');return new Vector3(float.Parse(Η(А,0)),float.Parse(Η(А,1)),float.Parse(Η(А,2)));
}String ΖΪ(Ω Φ){String Ї=":";String Ϊ=І(Φ.Δ)+Ї+І(Φ.α)+Ї+
І(Φ.ΰ)+Ї+І(Φ.τ)+Ї+І(Φ.ι);for(int i=0;i<Φ.Ǘ.Length;i++)
Ϊ+=Ї+Math.Round(Φ.Ǘ[i],3);return Ϊ;}Ω ΖΙ(String s){String[]А=s.Split(':');Ω Φ=new Ω(
Ѕ(Η(А,0)),Ѕ(Η(А,1)),
Ѕ(Η(А,2)),Ѕ(Η(А,3)),
Ѕ(Η(А,4)));int Α=5;List<float>Δΰ=new List<float>();while(Α<А.Length){String ΑΥ=Η(А,Α);float f=0;if(!float.TryParse(ΑΥ,out f)) break;Δΰ.Add(f);Α++;}Φ.Ǘ=Δΰ.ToArray();return Φ;}System.Text.StringBuilder ľ=new System.Text.StringBuilder();void Π<T>(T value,bool ΗΑ){if(ΗΑ) ľ.Append('\n');ľ.Append(value);}
void Π<T>(T value){Π(value,true);}String Ɛ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ƥ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Η(String[]Ζΰ,int Α){String ΑΥ=Ζΰ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(ΑΥ)) return "";
return ΑΥ;}void Save(){if(ΑΗ||Ν==Κ.е){Storage="";return;}Storage=Αί();}String Αί(){ľ.Clear();ľ.Append(DATAREV+";");Π(І(ΑΖ),false);Π(І(Ι.α));Π(І(Ι.τ));Π(І(Ι.ΰ));Π(І(Ι.ι));Π(І(Ι.Δ));Π(І(Ι.з));Π(Ι.β);Π(І(Ε.Δ));Π(І(Ε.ι));Π(І(Ε.α));Π(І(Ε.ΰ));Π(І(Ε.τ));Π(І(Ε.з));Π(Ε.β);Π(І(ǒ));Π(І(ȍ));Π(";");
Π((int) Ν,false);Π((int) έ);Π((int) л);Π(Ǣ);Π(м);Π(О);Π(н);Π(ǜ);Π(ȓ);Π(Ȓ);if(Ν==Κ.ή){Π((int) υ.Ј);Π(υ.ў);Π(υ.ȟ);Π(Ɛ(υ.џ));Π(Ɛ(υ.Ǐ));Π((int) φ.Ј);Π(φ.ў);Π(φ.ȟ);Π(Ɛ(φ.џ));Π(Ɛ(φ.Ǐ));}else{Π((int) ǣ);Π((int) б);Π((int) Д);Π((int) й);Π((int) Ű);Π(ȭ);Π(Ē);Π(į);Π(Ī);Π(Ю);Π(Э);Π(М);Π(ň);Π(Ň);Π(Ė);Π(Ȣ);
Π(Ϗ);Π(Ћ);Π(σ);Π(π);Π(ȋ);Π(Й);Π(Đ);Π(ď);}Π(";");for(int i=0;i<Ƿ.Count;i++)
Π((i>0?"|":"")+Ƿ[i],false);Π(";");for(int i=0;i<Τ.Count;i++)
Π(ΖΪ(Τ[i]),i>0);Π(";",false);Π(к+"|"+Ǔ,false);String ί=ľ.ToString();ľ.Clear();return ί;}ж ΕЋ(){return Ɲ(Storage);}ж Ɲ(String ί){if(ί=="") return ж.ΕІ;String[]ŀ=ί.Split(';');if(Η(ŀ,0)!=DATAREV) return ж.Αό;int Α=0;try{String[]Ζ=Η(ŀ,1).Split('\n');ΑΖ=Ѕ(Η(Ζ,Α++));Ι.α=Ѕ(Η(Ζ,Α++));
Ι.τ=Ѕ(Η(Ζ,Α++));Ι.ΰ=Ѕ(Η(Ζ,Α++));Ι.ι=Ѕ(Η(Ζ,Α++));Ι.Δ=Ѕ(Η(Ζ,Α++));Ι.з=Ѕ(Η(Ζ,Α++));Ι.β=bool.Parse(Η(Ζ,Α++));Ε.Δ=Ѕ(Η(Ζ,Α++));Ε.ι=Ѕ(Η(Ζ,Α++));Ε.α=Ѕ(Η(Ζ,Α++));Ε.ΰ=Ѕ(Η(Ζ,Α++));Ε.τ=Ѕ(Η(Ζ,Α++));Ε.з=Ѕ(Η(Ζ,Α++));Ε.β=bool.Parse(Η(Ζ,Α++));ǒ=Ѕ(Η(Ζ,Α++));ȍ=Ѕ(Η(Ζ,Α++));Ζ=Η(ŀ,2).Split('\n');Α=0;
Ν=(Κ) int.Parse(Η(Ζ,Α++));έ=(Θ) int.Parse(Η(Ζ,Α++));л=(Θ) int.Parse(Η(Ζ,Α++));Ǣ=int.Parse(Η(Ζ,Α++));м=int.Parse(Η(Ζ,Α++));О=int.Parse(Η(Ζ,Α++));н=int.Parse(Η(Ζ,Α++));ǜ=float.Parse(Η(Ζ,Α++));ȓ=bool.Parse(Η(Ζ,Α++));Ȓ=bool.Parse(Η(Ζ,Α++));if(Ν==Κ.ή){υ.Ј=(δ) int.Parse(Η(Ζ,Α++));
υ.ў=float.Parse(Η(Ζ,Α++));υ.ȟ=float.Parse(Η(Ζ,Α++));υ.џ=ƥ(Η(Ζ,Α++));υ.Ǐ=ƥ(Η(Ζ,Α++));φ.Ј=(δ) int.Parse(Η(Ζ,Α++));φ.ў=float.Parse(Η(Ζ,Α++));φ.ȟ=float.Parse(Η(Ζ,Α++));φ.џ=ƥ(Η(Ζ,Α++));φ.Ǐ=ƥ(Η(Ζ,Α++));}else{ǣ=(њ) int.Parse(Η(Ζ,Α++));б=(Ш) int.Parse(Η(Ζ,Α++));Д=(ϋ) int.Parse(Η(Ζ,Α++));
й=(у) int.Parse(Η(Ζ,Α++));Ű=(њ) int.Parse(Η(Ζ,Α++));ȭ=bool.Parse(Η(Ζ,Α++));Ē=bool.Parse(Η(Ζ,Α++));į=bool.Parse(Η(Ζ,Α++));Ī=bool.Parse(Η(Ζ,Α++));Ю=int.Parse(Η(Ζ,Α++));Э=int.Parse(Η(Ζ,Α++));М=int.Parse(Η(Ζ,Α++));ň=float.Parse(Η(Ζ,Α++));Ň=float.Parse(Η(Ζ,Α++));Ė=float.Parse(Η(Ζ,Α++));
Ȣ=float.Parse(Η(Ζ,Α++));Ϗ=int.Parse(Η(Ζ,Α++));Ћ=int.Parse(Η(Ζ,Α++));σ=int.Parse(Η(Ζ,Α++));π=int.Parse(Η(Ζ,Α++));ȋ=int.Parse(Η(Ζ,Α++));Й=int.Parse(Η(Ζ,Α++));Đ=int.Parse(Η(Ζ,Α++));ď=float.Parse(Η(Ζ,Α++));}Ζ=Η(ŀ,3).Replace("\n","").Split('|');Ƿ=Ζ.ToList();Ζ=Η(ŀ,4).Split('\n');
Τ.Clear();if(Ζ.Count()>=1&&Ζ[0]!="")
for(int i=0;i<Ζ.Length;i++)
Τ.Add(ΖΙ(Η(Ζ,i)));Ζ=Η(ŀ,5).Split('|');к=Η(Ζ,0);if(!int.TryParse(Η(Ζ,1),out Ǔ)) Ǔ=-1;}catch{return ж.Γν;}Ŀ=л;ς();return ж.Ƣ;}const int Γβ=64000;IMyTerminalBlock Γξ(){List<IMyTerminalBlock>Ħ=new List<IMyTerminalBlock>();ψ.GetBlocksOfType(Ħ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ħ.Count>0?Ħ[0]:null;}void Εν(){IMyTerminalBlock Σ=Γξ();if(Σ==null){Ξ="Export: name a block "+dataTag;return;}String ί="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Τ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Αί();if(ί.Length>Γβ){Ξ="Export too large: "+ί.Length+" chars, max "+Γβ;return;}Σ.CustomData=ί;Ξ="Exported "+Τ.Count+" waypoints";}void Εω(bool Βτ){IMyTerminalBlock Σ=Γξ();if(Σ==null){Ξ="Import: name a block "+dataTag;return;}
Εϊ(Σ.CustomData,Βτ);}void Εϊ(String ί,bool Βτ){ί=ί.Replace("\r","");if(ί.StartsWith("//")){int ΔЄ=ί.IndexOf('\n');ί=ΔЄ<0?"":ί.Substring(ΔЄ+1);}ί=ί.Trim();if(ί==""){Ξ="Import: no PAM data in "+dataTag;return;}ς();String ΔΚ=Αί();ж Ϊ=Ɲ(ί);if(Ϊ==ж.Ƣ&&Βτ){Ω ı=new Ω(Ε);
Vector3 ΖЏ=ǒ,ΖЎ=ȍ;int w=Ю,h=Э,d=М;њ Ηά=ǣ;у Βί=й;float ΗΖ=Ė,ΗΕ=Ȣ;Ɲ(ΔΚ);Ε=ı;ǒ=ΖЏ;ȍ=ΖЎ;Ю=w;Э=h;М=d;ǣ=Ηά;й=Βί;Ė=ΗΖ;Ȣ=ΗΕ;έ=Θ.Ğ;Ś(true,true);έ=Θ.Ğ;л=Θ.Ğ;Ξ="Imported job, run CONT";return;}if(Ϊ==ж.Ƣ){ŵ=false;Ξ="Imported "+Τ.Count+" waypoints";
return;}Ɲ(ΔΚ);Ξ=Ϊ==ж.Αό?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΖΓ(){ţ.Clear();for(int i=ъ.Count-1;i>=0;i--){String ί=ъ[i].CustomData.ToUpper();bool Βς=false;if(ί=="INSTRUCTIONS"){Βς=true;ƴ=true;}if(ί=="DEBUG") Βς=true;if(Βς){ţ.Add(ъ[i]);ъ.RemoveAt(i);}
}ŗ(ţ,false,1,false);}void ΖΔ(List<IMyTerminalBlock>Ħ){Ȭ.Clear();for(int i=0;i<Ħ.Count;i++){IMyTerminalBlock Σ=Ħ[i];try{String ΔЉ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Σ.CustomName.IndexOf(ΔЉ);int Ł=-1;if(Α<0||!int.TryParse(Σ.CustomName.Substring(Α+ΔЉ.Length,1),out Ł)) continue;
if(Ł==-1) continue;Ł--;IMyTextSurfaceProvider ΔЋ=(IMyTextSurfaceProvider) Σ;if(Ł<ΔЋ.SurfaceCount&&Ł>=0)
Ȭ.Add(ΔЋ.GetSurface(Ł));}catch{}}}String ǋ="";void ΓЄ(){String ΒϏ="";String Βγ="";ǋ=Γα(false);ΒϏ+=ǋ;Βγ+=ǋ;Βγ+=Ες();for(int i=0;i<ъ.Count;i++) ъ[i].WriteText(ΒϏ);for(int i=0;i<Ȭ.Count;i++) Ȭ[i].WriteText(ΒϏ);Echo(Βγ);for(int i=0;i<ţ.Count;i++){IMyTextPanel ΒЌ=ţ[i];
String ί=ΒЌ.CustomData.ToUpper();if(ί=="DEBUG") ΒЌ.WriteText(""+"\n"+"");if(ί=="INSTRUCTIONS") ΒЌ.WriteText(Εσ());}}string Εσ(){String Γ="";try{float Δύ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(ĥ*Δύ)+" / "+(int)(ĵ*Δύ)+"\n";Γ+="Inst. avg/max: "+Math.Round(ĥ*100f,1)+"% / "+Math.Round(ĵ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(Ş,2)+"ms / "+Math.Round(ΒЃ,2)+"ms \n";}catch{}for(int i=0;i<Ȏ.Count;i++)
Γ+=""+Ȏ.Keys.ElementAt(i)+" = "+Math.Round(Ȏ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȏ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void Εψ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ŗ(ъ,setLCDFontAndSize,0.9f,false);ŗ(Ȭ,setLCDFontAndSize,0.9f,true);for(int i=0;i<ъ.Count;i++) ъ[i].WriteText(Γ+Ȋ.ToString());for(int i=0;i<Ȭ.Count;i++) Ȭ[i].WriteText(Γ+Ȋ.ToString());}void ǯ(){if(!ƴ) return;try{ΒА=Runtime.CurrentInstructionCount;}catch{}
}void ǫ(String ρ){if(!ƴ) return;if(ΒА==0) return;try{float ΑΤ=(Runtime.CurrentInstructionCount-ΒА)/Runtime.MaxInstructionCount*100;if(!Ȏ.ContainsKey(ρ))
Ȏ.Add(ρ,new float[]{ΑΤ,ΑΤ});else{Ȏ[ρ][0]=ΑΤ;Ȏ[ρ][1]=Math.Max(ΑΤ,Ȏ[ρ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Εά(String ȡ){if(!ħ) return;String Ї=""+BROADCAST_SEP;String μ=VERSION+Ї;μ+=Ŋ(ΓΔ)+Ї;
μ+=(int) Ν+Ї;μ+=Ακ(γ.X)+""+Ї;μ+=Ακ(γ.Y)+Ї;μ+=Ακ(γ.Z)+Ї;if(Ν!=Κ.ή)
μ+=Αν(л)+(л==Θ.Є?" "+Math.Round(ů,1)+"%":"")+Ї;else μ+=Αν(л)+Ї;μ+=Ŋ(Ξ)+Ї;μ+=ь+""+Ї;μ+=Ŋ(ǋ)+Ї;μ+=ĩ+Ї;μ+=Ķ+Ї;for(int i=0;i<ǎ.Count;i++){if(ǎ[i].ĳ!=В.ő) continue;μ+=Ŋ(ǎ[i].ρ)+"/"+Ŋ(ǎ[i].ν)+"/"+ǎ[i].ћ+Ї;}ΖΑ(μ,ȡ);}String Ŋ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΖΑ(String μ,String ȡ){ΖΒ(μ,BROADCAST_SELF_ID,ȡ);}void ΖΒ(String μ,String И,String ȡ){try{if(!ħ) return;μ=BROADCAST_TAG+BROADCAST_SEP+Γπ()+BROADCAST_SEP+И+BROADCAST_SEP+ΒΥ+BROADCAST_SEP+ȡ+BROADCAST_SEP+μ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,μ);}catch(Exception e){Ȋ=e;}
}String Γπ(){if(Ν!=Κ.е) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool Γώ(String μ){return μ.StartsWith(BROADCAST_TAG);}bool ō(ref String ί,out string Δα,Char Ї){int Α=ί.IndexOf(Ї);Δα="";if(Α<0) return false;Δα=ί.Substring(0,Α);ί=ί.Remove(0,Α+1);return true;}bool ΕЇ(ref String μ,out String Ż,out String ȡ)
{Ż="";ȡ="";if(!ħ) return false;String ǖ="";if(!ō(ref μ,out ǖ,BROADCAST_SEP)||!Γώ(ǖ)) return false;if(!ō(ref μ,out Ż,BROADCAST_SEP)) return false;if(!ō(ref μ,out ǖ,BROADCAST_SEP)||(ǖ!=Γπ()&&(ǖ!="*"&&Ν!=Κ.е))) return false;if(!ō(ref μ,out ǖ,BROADCAST_SEP)||(ǖ!=ΒΥ)) return false;
if(!ō(ref μ,out ȡ,BROADCAST_SEP)) return false;return true;}void Εό(){bool Ţ=true;if(Δγ){Δγ=false;if(Me.CustomData.Contains("Antenna_Name")){Ξ="Update custom data";Me.CustomData="";}}String ū=(Ν!=Κ.е?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ū)
Γϊ(ū);String[]Ζ=Me.CustomData.Split('\n');ħ=bool.Parse(ΑІ(Αθ(Ζ,"Enable_Broadcast",ref Ţ)));bool Γά=false;bool ΔΫ=true;if(ħ){if(Ν!=Κ.е)
ΓΔ=ΑІ(Αθ(Ζ,"Ship_Name",ref Ţ)).Replace(BROADCAST_SELF_ID,"");ΒΥ=ΑІ(Αθ(Ζ,"Broadcast_Channel",ref Ţ)).ToLower();ΔΫ=false;if(ȅ==null){ȅ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȅ.SetMessageCallback("");}List<IMyRadioAntenna>Ģ=new List<IMyRadioAntenna>();ψ.GetBlocksOfType(Ģ);bool ΒΚ=false;
for(int i=0;i<Ģ.Count;i++)
if(Ģ[i].EnableBroadcasting&&Ģ[i].Enabled){ΒΚ=true;break;}if(Ģ.Count==0) Ξ="No Antenna found";else if(!ΒΚ) Ξ="Antenna not ready";Γά=Ģ.Count==0||!ΒΚ;if(ΔΓ&&!Γά&&Ν!=Κ.е) Ξ="Antenna ok";}else if(Ν==Κ.е)
Ξ="Offline - Enable in PB custom data";ΔΓ=Γά;if(ΔΫ){if(ȅ!=null) this.IGC.DisableBroadcastListener(ȅ);ȅ=null;}}catch{Ţ=false;}if(!Ţ){Ξ="Reset custom data";Γϊ(ū);}}void Γϊ(String ū){String ί=ū+"\n\n"+"Enable_Broadcast="+(Ν==Κ.е?"true":"false")+" \n";ί+=Ν!=Κ.е?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=ί+"Broadcast_Channel=#default";}String ΑІ(String Ď){int Α=Ď.IndexOf("//");if(Α!=-1) Ď=Ď.Substring(0,Α);String[]А=Ď.Split('=');if(А.Length<=1) return "";return А[1].Trim();}String Αθ(String[]Ζ,String Βυ,ref bool Ŷ){foreach(String Ď in Ζ)
if(Ď.StartsWith(Βυ)) return Ď;Ŷ=false;return "";}String ΓЇ(int ΓΫ,String Γ,int ΔЎ,ref int ȏ){String[]Ζ=Γ.Split('\n');if(ΔЎ>=ȏ+ΓΫ-1) ȏ++;ȏ=Math.Min(Ζ.Count()-1-ΓΫ,ȏ);if(ΔЎ<ȏ+1) ȏ--;ȏ=Math.Max(0,ȏ);String Ϊ="";for(int i=0;i<ΓΫ;i++){int Δχ=i+ȏ;if(Δχ>=Ζ.Count()) break;
Ϊ+=Ζ[Δχ]+"\n";}return Ϊ;}String Αλ(int ź){if(ź>=60*60) return Math.Round(ź/(60f*60f),1)+" h";if(ź>=60) return Math.Round(ź/60f,1)+" min";return ""+ź+" s";}string Ακ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>Ǳ=new List<IMyCameraBlock>();List<long>Ĺ=new List<long>();
bool Ļ=false;int ΒΪ=0;int ΒΨ=0;DateTime ΔΜ=DateTime.Now;String ΒΤ="";void ΕБ(){Ǳ.Clear();Ĺ.Clear();if(!obstacleBrake) return;ψ.GetBlocksOfType(Ǳ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<Ǳ.Count;i++){Ǳ[i].EnableRaycast=true;if(!Ĺ.Contains(Ǳ[i].CubeGrid.EntityId)) Ĺ.Add(Ǳ[i].CubeGrid.EntityId);
}if(!Ĺ.Contains(Me.CubeGrid.EntityId)) Ĺ.Add(Me.CubeGrid.EntityId);}bool Γύ(Β s){return s==Β.Ǽ||s==Β.С||s==Β.ї||
s==Β.Ș||s==Β.Ĝ||s==Β.Ŏ||s==Β.Ƒ;}void Ǝ(){Ļ=false;ΒΪ=0;ΒΤ="";}void ΖΤ(){if(!obstacleBrake||Ǳ.Count==0||!Γύ(Ψ)){Ǝ();return;}Vector3 Ū=ω+ΑΣ;Vector3 ǥ=Ū-γ;float Ǥ=ǥ.Length();if(Ǥ<1f){Ǝ();return;}Vector3 θ=ǥ/Ǥ;float Δϋ=Ȑ*obstacleLookAhead+η;Vector3 ΗΜ=Ū+θ*(η*0.25f);
for(int n=0;n<Ǳ.Count;n++){ΒΨ=(ΒΨ+1)%Ǳ.Count;IMyCameraBlock š=Ǳ[ΒΨ];if(!š.IsFunctional||!š.IsWorking) continue;Vector3 ΔΞ=š.GetPosition();Vector3 ƶ=ΗΜ;if(Vector3.Distance(ΔΞ,ƶ)>Δϋ) ƶ=ΔΞ+θ*Δϋ;if(!š.CanScan(ƶ)) continue;MyDetectedEntityInfo Ŭ=š.Raycast(ƶ);bool ΗΔ=!Ŭ.IsEmpty()&&!Ĺ.Contains(Ŭ.EntityId)&&!ΕЂ(Ŭ.EntityId)&&
Ŭ.Type!=MyDetectedEntityType.FloatingObject;if(ΗΔ){if(!Ļ) ΔΜ=DateTime.Now;Ļ=true;ΒΪ=0;ΒΤ=Ŭ.Type.ToString();}else if(Ļ&&++ΒΪ>=3) Ǝ();break;}if(Ļ&&(DateTime.Now-ΔΜ).TotalSeconds>obstacleTimeout)
Ξ="Path blocked: "+ΒΤ;}const String ΓЏ="[PAMTFC]";const float ΖΚ=5f;const float Εξ=2f;class ř{public long щ;public Vector3 ϊ,ΓΪ;public float ΔΪ;public String ı="";public int З=-1;public DateTime ΓΠ;}IMyBroadcastListener ΑΡ=null;long ΒІ=0;List<ř>Х=new List<ř>();Vector3 ΑΣ=new Vector3();
float ń=-1;int ΕΣ=0;String к="";int Ǔ=-1;bool ư=false;Vector3 ΔΣ=new Vector3();String Αψ(){if(έ==Θ.Ы||Ν==Κ.ή) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Ϗ+"|"+Ћ+"|"+(int) Ű+"|"+Math.Round(ǹ,1)+"|"+Math.Round(ǧ,1);}void ΖΧ(){if(!trafficEnabled) return;if(ΑΡ==null) ΑΡ=IGC.RegisterBroadcastListener(ΓЏ);ΒІ=Me.CubeGrid.EntityId;int Ζδ=30;while(ΑΡ.HasPendingMessage&&Ζδ-->0){String ί=ΑΡ.AcceptMessage().Data as String;
if(ί!=null) ΕЈ(ί);}for(int i=Х.Count-1;i>=0;i--)
if((DateTime.Now-Х[i].ΓΠ).TotalSeconds>ΖΚ) Х.RemoveAt(i);if(Ψ==Β.Ъ||++ΕΣ<3) return;ΕΣ=0;bool ΖϏ=Ψ==Β.Ǽ||Ψ==Β.ĝ||Ψ==Β.Ȁ;IGC.SendBroadcastMessage(ΓЏ,ΒІ+";"+І(γ)+";"+
І(Υ.GetShipVelocities().LinearVelocity)+";"+η+";"+Αψ()+";"+
(ΖϏ?Ǔ:-1)+";"+к);}void ΕЈ(String ί){try{String[]f=ί.Split(';');if(f.Length<7) return;long щ=long.Parse(f[0]);if(щ==ΒІ) return;ř ѐ=null;for(int i=0;i<Х.Count;i++) if(Х[i].щ==щ) ѐ=Х[i];if(ѐ==null){ѐ=new ř();ѐ.щ=щ;Х.Add(ѐ);}ѐ.ϊ=Ѕ(f[1]);ѐ.ΓΪ=Ѕ(f[2]);ѐ.ΔΪ=float.Parse(f[3]);
ѐ.ı=f[4];ѐ.З=int.Parse(f[5]);ѐ.ΓΠ=DateTime.Now;if(ѐ.ı!=""&&ѐ.ı==Αψ()) ΕЄ(f[6]);}catch{}}bool ΕЂ(long щ){for(int i=0;i<Х.Count;i++) if(Х[i].щ==щ) return true;return false;}Vector3 Ετ(int Α,int ΗΗ){if(laneOffset<=0||Τ.Count<2||Α<0||Α>=Τ.Count) return new Vector3();
Ω Φ=Τ[Α];Vector3 Ηο=(Τ[Math.Min(Τ.Count-1,Α+1)].Δ-Τ[Math.Max(0,Α-1)].Δ)*ΗΗ;Vector3 Ц=Φ.ι.LengthSquared()>0.01f?-Vector3.Normalize(Φ.ι):-Φ.ΰ;Vector3 р=Vector3.Cross(Ηο,Ц);if(р.LengthSquared()<0.0001f) return new Vector3();float χ=laneOffset*η;float Ζφ=Math.Min(Vector3.Distance(Φ.Δ,Τ[0].Δ),
Vector3.Distance(Φ.Δ,Τ[Τ.Count-1].Δ));return Vector3.Normalize(р)*χ*Math.Min(1,Ζφ/(χ*4));}void ΖΨ(){ΑΣ=new Vector3();ń=-1;if(!trafficEnabled||Х.Count==0||!Γύ(Ψ)) return;Εγ(Υ.GetNaturalGravity());}void Εγ(Vector3 ι){Vector3 ǥ=ω-γ;float Ǥ=ǥ.Length();if(Ǥ<1f) return;Vector3 θ=ǥ/Ǥ;
Vector3 Ц=ι.LengthSquared()>0.01f?-Vector3.Normalize(ι):-Ŧ;Vector3 р=Vector3.Cross(θ,Ц);р=р.LengthSquared()<0.0001f?-Ų:Vector3.Normalize(р);for(int i=0;i<Х.Count;i++){ř Ж=Х[i];Vector3 ϊ=Ж.ϊ+Ж.ΓΪ*(float)(DateTime.Now-Ж.ΓΠ).TotalSeconds;Vector3 ΔЌ=ϊ-γ;float ƫ=Vector3.Dot(ΔЌ,θ);
if(ƫ<=0) continue;float ć=(η+Ж.ΔΪ)*0.5f;if((ΔЌ-θ*ƫ).Length()>ć*1.2f) continue;float ΒЉ=Vector3.Dot(Ж.ΓΪ,θ);if(ΒЉ<-1f){if(ƫ<(Ȑ-ΒЉ)*6f+ć*4f) ΑΣ=р*ć*1.3f;}else{float Ζψ=ƫ-ć-followGap*η;float Δφ=Math.Max(0,ΒЉ)+Math.Max(0,Ζψ)/Εξ;if(ń<0||Δφ<ń) ń=Δφ;
}}}bool Γμ(){if(к.Length==Math.Max(0,Ћ)) return false;к=new String('0',Math.Max(0,Ћ));Ǔ=-1;return true;}void ΕЄ(String Ж){Γμ();if(Ж.Length!=к.Length) return;char[]ļ=к.ToCharArray();for(int i=0;i<ļ.Length;i++) if(Ж[i]=='1') ļ[i]='1';к=new String(ļ);}bool ΓϏ(int З){return З>=0&&З<к.Length&&к[З]=='1';}
void ΓЊ(int З){if(З<0||З>=к.Length) return;char[]ļ=к.ToCharArray();ļ[З]='1';к=new String(ļ);}bool ΕЁ(int З){String Βυ=Αψ();for(int i=0;i<Х.Count;i++)
if(Х[i].ı==Βυ&&Х[i].З>=0&&Math.Abs(Х[i].З-З)<=1) return true;return false;}int Εβ(int Α){if(Ϗ<1||Ћ<1) return Α;if(Γμ()) for(int r=0;r<Α/Ϗ&&r<Ћ;r++) ΓЊ(r);if(Ǔ>=0&&Α>=(Ǔ+1)*Ϗ){ΓЊ(Ǔ);Ǔ=-1;}int З=Α/Ϗ;if(З==Ǔ&&!ΓϏ(З)) return Α;bool ΔΔ=false;for(int r=0;r<Ћ;r++)
{if(ΓϏ(r)) continue;ΔΔ=true;if(trafficEnabled&&ΕЁ(r)) continue;Ǔ=r;return r==З?Α:r*Ϗ;}return ΔΔ?-1:Ϗ*Ћ;}void Εη(string ц){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}