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
public enum Ζ{ΑΔ,Ŷ,Ǐ,с,Ϋ}public enum Α{ж,ǥ,Ļ,ǻ,Ǻ,Ѕ,Ю,
ȫ,ĸ,ǣ,Ĕ,Đ,ȧ,
ŷ,Ǿ,ǒ,ē,ΑΖ,Ƹ,
ΒЊ,ΑΕ,ķ,ƹ,ȭ}public enum Μ{з,ļ,Є,ΒΦ,Ƴ,ǂ,ǃ,ĳ,Ĵ}public enum р{None,ē,ж,Δρ}public enum Ϊ{Main,ΕΓ,ŵ,Ŵ,ΒІ,Ʊ,
ΒΖ,ΔЂ,ΔЃ,ΔЀ,Ηύ,Ιψ,
Ιφ,Ιχ,ΒЁ,ΕΗ,ǋ}public enum а{ǉ,ŭ,ΗϏ,ΔЅ,Ϋ}public enum Ǒ{ΑΑ,ΖЏ}public enum д{ǈ,Βά,ΕΙ,Ű}public enum Ϗ{Βο,Δξ,ΒΠ,ΒΧ,ƶ,Βθ,ƽ}public enum ѐ{ΒΣ,ΒΛ,ΒΜ}public enum З{ű,Ηά,đ}public enum κ{Βρ,Βυ,Βφ,ΔБ,ΔА,
Βπ,ǆ,ǅ,Βσ,
Βτ,Βς}public enum ΰ{ǵ,Ȑ,Ǵ,ư,ΒΙ,ΒΚ}public enum ľ{Δφ,Δδ,Ιμ}public enum т{ǌ,Δτ,Ηθ,Βξ}public enum Ƕ{None,Βϊ,Ηξ,Ηη}public enum σ{ż,Ư,ǀ,Ű,Βμ,ǁ,Βν,ΗЅ,Βχ}public enum Ιπ{ΑΔ,Ις,Ιρ,Ιτ,Ιν,Ιλ,Ισ}class Ψ{public bool ε=false;public Vector3 Δ=new Vector3();
public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 υ=new Vector3();public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float Κέ=0;public float[]Ǡ=null;public Ψ(){}public Ψ(Ψ γ){ε=γ.ε;Δ=γ.Δ;α=γ.α;β=γ.β;υ=γ.υ;ν=γ.ν;
Ч=γ.Ч;Ǡ=γ.Ǡ;}public Ψ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 υ,Vector3 ν){this.Δ=Δ;this.α=α;this.β=β;this.υ=υ;this.Џ=0;this.ν=ν;}public void ΖЋ(List<IMyThrust>ΑΚ,List<string>Ȋ){Ǡ=new float[Ȋ.Count];for(int i=0;i<Ǡ.Length;i++)
Ǡ[i]=-1;for(int i=0;i<ΑΚ.Count;i++){string я=Δώ(ΑΚ[i]);int Β=Ȋ.IndexOf(я);if(Β!=-1)
Ǡ[Β]=А(ΑΚ[i].MaxEffectiveThrust,ΑΚ[i].MaxThrust);}}}class Βω{public Ψ Њ=null;public List<Vector3>ť=new List<Vector3>();public float Ƈ=0;public float ΔΨ=0;public float Ń=0;public float Γί=0;public Vector3 Αή=new Vector3();}class Ŀ{public κ Ќ=κ.Βρ;public float ǖ=0;public float Ā=0;
public string Ǘ="";public string ǚ="";DateTime ΖΖ;public bool ΓЋ=false;private bool ΓΣ=false;public bool ΒΞ(bool ȉ){if(ȉ){Ā=0;ΓΣ=false;return false;}ΓΣ=true;return Ā>ǖ;}public void Ηο(){ΒΞ(true);ΓЋ=false;}public void ΗБ(){if(ΓΣ)
if((DateTime.Now-ΖΖ).TotalSeconds>1){Ā++;ΖΖ=DateTime.Now;}}public bool Βε(){switch(Ќ){case κ.Βυ:return true;case κ.Βφ:return true;}return false;}}class ё{public String φ="";public String ς="";public int Ǔ=0;public З œ=З.đ;public ё(String φ,String ς,int Ǔ,З œ){this.φ=φ;this.ς=ς;
this.Ǔ=Ǔ;this.œ=œ;}}class ΗΚ{public ΗΚ(Vector3 η,float ω){this.η=η;this.ω=ω;}public Vector3 η;public float ω;}class Ȓ{public DateTime Θρ;public DateTime Εζ;public String λ="";public String φ="";public String Κί="";public String Κ="";public String ΚΤ="";public String ΚΜ="";public String ΓЊ="";
public Vector3 Δ=new Vector3();public List<ё>Ъ=new List<ё>();public Ζ Η=Ζ.ΑΔ;public Ƕ ȯ;public float Ŗ;public float ň;public int љ;public int ΙЌ=0;public int ΚΫ=0;public bool Ιξ(){return(DateTime.Now-Θρ).TotalSeconds>10;}public bool Ηί(){if(ȯ!=Ƕ.Βϊ) return false;return(DateTime.Now-Εζ).TotalSeconds>=4;
}public Ȓ(String λ){this.λ=λ;}public Ƕ Ιυ(String Ğ,bool ΙΓ,bool Ǖ,bool ΘΞ){if(Ğ==""&&!Ǖ) return Ƕ.None;if(ȯ==Ƕ.Βϊ&&Ηί())
ȯ=Ƕ.Ηη;if(ΙΓ){ΓЊ=Ğ;ȯ=Ƕ.Βϊ;Εζ=DateTime.Now;return ȯ;}else if(Ǖ){ΓЊ="";ȯ=Ƕ.None;}else if(ΓЊ==Ğ){if(ΘΞ) ȯ=Ƕ.Ηξ;return ȯ;}return Ƕ.None;}}IMyGridTerminalSystem ϋ;Vector3 Αώ=new Vector3();Vector3 ή=new Vector3();Vector3 Αα=new Vector3();Vector3 ƕ=new Vector3();Vector3 Ɗ=new Vector3();
DateTime ΖΕ=new DateTime();bool Ǝ=true;int ΔΟ=0;bool Ĉ=false;bool Αι=false;bool ƙ=true;bool ΖЅ=false;bool ő=false;bool Αζ=false;float Ȣ=0;float ĭ=0;int ċ=0;int ƨ=0;int ΑЁ=0;int ΓΙ=0;float ŕ=0;float ń=0;double ΓЀ=0;float ƀ=0;List<int>Ŏ=new List<int>();List<int>Š=new List<int>();
Ζ Η=Ζ.ΑΔ;int ώ=0;float ǡ=0;float ǳ=0;float δ=0;float ΑЂ=0,ΔΒ=0;IMyRemoteControl Τ;IMySensorBlock ξ;List<IMyTimerBlock>ΔΡ=new List<IMyTimerBlock>();List<IMyShipConnector>у=new List<IMyShipConnector>();List<IMyThrust>ȋ=new List<IMyThrust>();List<IMyGyro>Αβ=new List<IMyGyro>();
List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>ȇ=new List<IMyLandingGear>();List<IMyReactor>Ċ=new List<IMyReactor>();List<IMyConveyorSorter>ΑЇ=new List<IMyConveyorSorter>();List<IMyGasTank>і=new List<IMyGasTank>();List<IMyTerminalBlock>Ž=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>Ň=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ě=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȯ=new List<IMyBatteryBlock>();List<IMyTextPanel>ї=new List<IMyTextPanel>();List<IMyTextSurface>Č=new List<IMyTextSurface>();List<IMyTextPanel>Ɔ=new List<IMyTextPanel>();
IMyTerminalBlock ћ=null;Ψ Θ=new Ψ();Ψ Γξ=new Ψ();Ψ Γρ=new Ψ();bool Ȍ=false;bool Ũ=false;bool ũ=false;bool Αθ=false;List<String>Ȋ=new List<string>();List<Ψ>Σ=new List<Ψ>();List<Ψ>ƭ=new List<Ψ>();int ΓΨ=0;double ΖІ=0;Ψ Ε=new Ψ();Vector3 ъ;Vector3 ȟ;Μ ί=Μ.з;Ǒ Ħ=Ǒ.ΑΑ;int Н=0;
int б=0;double ƒ=0;bool ΑΥ=false;Ŀ ό=new Ŀ();Ŀ ύ=new Ŀ();д м=д.ǈ;bool Ď=true;Ǒ ȣ=Ǒ.ΑΑ;int й=3;int и=3;int Ц=30;ѐ ф=ѐ.ΒΣ;bool Ȧ=true;bool Ő=true;bool ŉ=true;bool Į=false;Ϗ М=Ϗ.Βο;bool ȥ=true;float ǯ=90;float ш=20;float Ы=5;float щ=20;float Ǩ=0.70f;float ū=1.50f;
float Ū=2.50f;float Ĳ=0f;float ģ=0f;Α Φ;Μ ч;Α Ƣ;Βω θ=null;int Ш=0;int Щ=0;int Й=0;int ȝ=0;int ī=30;int У=0;bool Ё=true;Vector3 л;double К=0;bool ΑЀ=false;int Δά=0;int Ђ=0;int ΒБ=0;int ǰ=0;int ΖΩ=0;Vector3 ƛ=new Vector3();float к=0;float Э=0;int ı=0;Ψ ΑЈ=null;
float Γτ=0;float ΑΜ=0;float С=0;float ĩ=0;bool Γο=false;bool į=false;bool Ʃ=false;bool ș=false;bool Ȕ=false;DateTime Θή=new DateTime();Ψ ğ=null;Ψ Ελ=null;float ǿ=0;р ǩ;float Ț=0;float İ=0;String Γϋ="";float Αμ=0;float ň=0;float Ŗ=0;List<ё>Ǚ=new List<ё>();float О=0;
bool Ơ=false;bool Ζϋ=false;bool Ζθ=false;bool ΑЌ=false;bool Ζι=false;float Γώ=0;float ΓЉ=0;Vector3 Εώ=new Vector3();Vector3 χ=new Vector3();Vector3 ΑЍ=new Vector3(1,1,1);float ΔΞ=1;Vector3 ƥ=new Vector3();bool Αη=false;bool Γη=false;bool Γψ=false;bool ΕΥ=false;float ΑΣ=0;
float ΑΘ=2;Vector3 Ζω;Vector3 ΔΝ;Vector3 ΔΜ;float[,]Ǣ=new float[3,2];Dictionary<String,float[,]>ў=new Dictionary<string,float[,]>();Dictionary<String,float>ΖЃ=new Dictionary<String,float>();int ΓΓ=0;Dictionary<String,float[]>Ƞ=new Dictionary<string,float[]>();float ΓЌ;bool ΑΦ=false;
int ΑΛ=0;int ą=0;int Αδ=0;int Ĥ=0;String Κ="";int[]ΖΠ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΡ=false;int Αφ=0;int љ=0;Ϊ Ѝ=Ϊ.Main;int ΙΟ=0,ΙΠ=0,ΙΡ=0,ΙΣ=0;int ΘА=0,ΘБ=0;bool ņ=false;bool ΕΦ=false;String ΔΓ="";String ΓΘ="";IMyBroadcastListener ȕ=null;
bool Εύ=true;List<Ȓ>ΙЃ=null;Ȓ ΚΟ=null;int ΙЄ=0,ΙЅ=0;int ΙІ=0,ΙЇ=0;int ΙЈ=0,ΙЉ=0,ΙЊ=0;String Θπ="";Exception Ȝ=null;bool ΑϏ=false;Μ Ţ=Μ.з;bool Εξ=false;bool ΖΔ=true;bool ΓБ=false;String ΖЄ="";bool Ƨ=false;bool ΔΘ=true;int Αε=0;int[]ΚΣ=null;Vector3 ΓΚ=new Vector3();
bool ΕЄ=false;bool ǐ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;Program(){ϋ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βζ(Me,controllerTag,true))
Η=Ζ.с;π(Ϊ.Main);if(Η!=Ζ.с){Κ="Welcome to [PAM]!";т ΓА=Ηπ();if(Η==Ζ.ΑΔ){List<IMyShipDrill>Εω=new List<IMyShipDrill>();List<IMyShipGrinder>ΕЅ=new List<IMyShipGrinder>();ϋ.GetBlocksOfType(Εω,q=>q.CubeGrid==Me.CubeGrid);ϋ.GetBlocksOfType(ΕЅ,q=>q.CubeGrid==Me.CubeGrid);if(Εω.Count>0)
{Η=Ζ.Ŷ;Κ="Miner mode enabled!";}else if(ΕЅ.Count>0){Η=Ζ.Ǐ;Κ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;Ǽ(а.Ϋ);}}if(ΓА==т.ǌ) ƙ=false;if(ΓА==т.Δτ) Κ="Data restore failed!";if(ΓА==т.Βξ) Κ="New version, wipe data";}}void Main(string є,UpdateType ΙΫ){try{if(Ȝ!=null){if(є.Trim().ToUpper()=="RETRY")
{Ȝ=null;Ǝ=true;ċ=0;if(Η!=Ζ.с) ψ();Κ="Restarted after error";}else{ΗΨ();ΕΑ();return;}}Αι=(ΙΫ&UpdateType.Update10)!=0;if(Αι) ΔΟ++;Ĉ=ΔΟ>=10;if(Ĉ) ΔΟ=0;if(Αι){ΑΛ++;if(ΑΛ>4) ΑΛ=0;ΑЁ=(int) Math.Max(0,10-(DateTime.Now-ΖΕ).TotalSeconds);}if(є!="") Θπ=є;if(Η!=Ζ.с)
Ηώ(є);else
ΗΘ(є);ΖΡ=false;try{int Εμ=Runtime.CurrentInstructionCount;float Ɯ=А(Εμ,Runtime.MaxInstructionCount);if(Ɯ>0.90) Κ="Max. instructions >90%";if(Ɯ>ŕ) ŕ=Ɯ;if(ΑΦ){Ŏ.Add(Εμ);while(Ŏ.Count>10) Ŏ.RemoveAt(0);ń=0;for(int i=0;i<Ŏ.Count;i++) ń+=Ŏ[i];ń=А(А(ń,Ŏ.Count),Runtime.MaxInstructionCount);
double Γυ=Runtime.LastRunTimeMs;if(Αζ&&Γυ>ΓЀ) ΓЀ=Γυ;Š.Add((int)(Γυ*1000f));while(Š.Count>10) Š.RemoveAt(0);ƀ=0;for(int i=0;i<Š.Count;i++) ƀ+=Š[i];ƀ=А(ƀ,Š.Count)/1000f;}}catch{ŕ=0;}}catch(Exception e){Ȝ=e;}}void Ηώ(string є){bool ΑЃ=false;String Εβ="";if(ώ<=1&&!ΔЈ(є))
ΕΒ(є);int Θϋ=50;while(ȕ!=null&&ȕ.HasPendingMessage&&Θϋ-->0){MyIGCMessage ρ=ȕ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String Ɵ="";if(ώ<=1&&Ηι(ref Χ,out Ɵ,out Εβ)&&Ɵ==BROADCAST_SELF_ID){ΕΒ(Χ);ΑЃ=true;break;}}bool ΕА=Αζ&&Φ==Α.ж&&!ő&&!ΑЃ&&ċ==0&&!Αθ;
if(Ĉ&&Φ!=Α.ж) ΑЃ=true;if((Αι&&!ΕА)||(Ĉ&&ΕА)){if(ċ==0&&(ΑЁ<=0||Ǝ)){ΖЅ=ώ>0;ώ=0;ċ=1;ǽ();Ητ();Ηέ();Ƿ("Scan 1");}else if(ċ==1){ċ=2;ǽ();Ηυ();Ƿ("Scan 2");}else if(ċ==2){ċ=0;ǽ();Ηφ();Ηχ();if(ώ<=1) ΔЌ();Ƿ("Scan 3");ΖΕ=DateTime.Now;if(ώ<=1&&ƙ)
Αώ=ƻ(Τ,Τ.CenterOfMass);ƙ=false;if(Ǝ){Ǝ=false;bool Ιΰ=ί==Μ.Є;ψ();if(ΖΔ&&resumeAfterRestart&&Ιΰ&&ώ<=1) ΓБ=true;}ΖΔ=false;if(ΖЅ&&ώ==0) Κ="Setup complete";}else{if(ί==Μ.Є&&Η!=Ζ.Ϋ){ǽ();ΖЈ();Ƿ("Inv balance");}ǽ();switch(ƨ){case 0:ΗЉ();break;case 1:ΗЍ();break;case 2:ΗЈ();break;
case 3:ΘΓ();break;case 4:ΗЌ();break;case 5:ΖА();break;case 6:ΖЍ(Τ);break;}Ƿ("Update: "+ƨ);ƨ++;if(ƨ>6){ƨ=0;Αζ=true;if(Ţ!=Μ.з){switch(Ţ){case Μ.ĳ:ȩ();break;case Μ.Ĵ:ȩ();break;case Μ.Є:ȩ();break;case Μ.ǂ:ƺ();break;case Μ.ǃ:ĕ();break;}Ţ=Μ.з;}}}if(!Ǝ){if(!ǹ(Τ,true)){Τ=null;
ƙ=true;ώ=2;}if(ώ>=2&&Φ!=Α.ж)
ψ();if(ώ<=1){ĭ=Τ.CalculateShipMass().PhysicalMass;Ȣ=(float) Τ.GetShipSpeed();ή=ΗЇ(Τ,Αώ);Αα=Τ.WorldMatrix.Forward;ƕ=Τ.WorldMatrix.Left;Ɗ=Τ.WorldMatrix.Down;ΗА();ΘΑ();ΗЊ();if(ΓБ){ΓБ=false;ȩ();if(Φ!=Α.ж) Κ="Job resumed after restart";}if(Φ!=Α.ж){ő=false;ΕΖ(false);Δθ(false);String ΙΘ=Βα(Φ)+" "+(int) Φ;
ǽ();ΗЎ();ΒЉ(false);Ƿ(ΙΘ);ΘΒ();ΗЏ();ǽ();ΗЋ();Ƿ("Thruster");ǽ();ΕΠ();Ƿ("Gyroscope");}else{if(ő){if(ƿ()){Х(Ɗ,Αα,ƕ,0.25f,true);ΕΠ();Κ="Aligning to planet: "+Math.Round(ΑΣ-0.25f,2)+"°";if(Αη) Βδ(true,true);}else Βδ(true,true);}}Ƨ=false;}}}ǽ();ΕΑ();Ƿ("Print");if(ΑЃ||ΓΙ<=0){ǽ();ΖЉ(Εβ);
Ƿ("Broadcast");ΓΙ=4;}else if(Ĉ) ΓΙ--;}void ΕΒ(string є){if(є=="") return;var Е=є.ToUpper().Split(' ');Е.DefaultIfEmpty("");var Ğ=Е.ElementAtOrDefault(0);var Ĝ=Е.ElementAtOrDefault(1);var ſ=Е.ElementAtOrDefault(2);var ΕΩ=Е.ElementAtOrDefault(3);String Ć="Invalid argument: "+є;bool ΔΧ=false;
switch(Ğ){case "UP":this.ΔЎ(false);break;case "DOWN":this.ΔЍ(false);break;case "UPLOOP":this.ΔЎ(true);break;case "DOWNLOOP":this.ΔЍ(true);break;case "APPLY":this.Δζ(true);break;case "MRES":љ=0;break;case "STOP":this.ψ();break;case "PATHHOME":{this.ψ();this.ΒЅ();}break;case "PATH":{this.ψ();this.ΒЅ();Θ.ε=true;}break;
case "START":{this.ψ();ΒЄ();}break;case "ALIGN":{Βδ(!ő,false);}break;case "CONT":{this.ψ();this.ȩ();}break;case "JOBPOS":{this.ψ();this.ĕ();}break;case "HOMEPOS":{this.ψ();this.ƺ();}break;case "FULL":{Ơ=true;}break;case "RESET":{ΑϏ=true;ώ=2;}break;case "EXPORT":{ΗΟ();}break;
case "IMPORT":{ΗΪ(Ĝ=="JOB");}break;default:ΔΧ=true;break;}if(Η!=Ζ.Ϋ){switch(Ğ){case "SHUTTLE":{ΗΝ();}break;case "CFGS":{if(!ΗΖ(Ĝ,ſ,ΕΩ)) Κ=Ć;}break;case "CFGB":{if(!ΗΕ(Ĝ,ſ)) Κ=Ć;}break;case "CFGL":{if(!Ė(ref ǯ,true,σ.ǀ,Ĝ,"")||!ΗΗ(ſ))
Κ=Ć;}break;case "CFGE":{if(!Ė(ref Ы,true,σ.ǁ,Ĝ,"IG")||
!Ė(ref ш,true,σ.Βμ,ſ,"IG")||
!Ė(ref щ,true,σ.Βν,ΕΩ,"IG"))
Κ=Ć;}break;case "CFGA":{if(!Ė(ref Ǩ,false,σ.Ư,Ĝ,"")) Κ=Ć;}break;case "CFGW":{if(!Ė(ref ū,false,σ.ż,Ĝ,"")||
!Ė(ref Ū,false,σ.ż,ſ,""))
Κ=Ć;}break;case "NEXT":{ΒΕ(false);}break;case "PREV":{ΒΕ(true);}break;default:if(ΔΧ) Κ=Ć;break;}}else{switch(Ğ){case "UNDOCK":{Ƨ=true;}break;default:if(ΔΧ) Κ=Ć;break;}}}String ΗΤ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n"+"[IMPORT JOB] Load only the job (co-op)\n";if(Η!=Ζ.Ϋ)
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΝ(){ψ();Η=Ζ.Ϋ;Ǽ(а.Ϋ);Ε.ε=false;Θ.ε=false;ξ=null;ο.Clear();ί=Μ.з;}void Βδ(bool Ј,bool х){if(!Ј) Κ="Aligning canceled";if(х) Κ="Aligning done";if(х||!Ј){ő=false;ΕΚ();Ǌ(false,0,0,0,0);return;}if(ƿ()) ő=true;}bool ΗΖ(String Ιβ,String Θη,String ΘΨ)
{bool Ιή=ί==Μ.Є;int w,h,d;if(int.TryParse(Ιβ,out w)&&int.TryParse(Θη,out h)&&int.TryParse(ΘΨ,out d)){this.ψ();й=w;и=h;Ц=d;ę(false);Ż(false,false);if(Ιή) ȩ();return true;}return false;}bool ΗΗ(String ΕΨ){if(ΕΨ=="ON"){Ȧ=true;return true;}if(ΕΨ=="OFF"){Ȧ=false;return true;}
return false;}bool ΗΕ(String Εψ,String ΑΤ){bool ƚ=true;if(Εψ=="HOME") Ď=true;else if(Εψ=="STOP") Ď=false;else ƚ=false;if(ΑΤ=="HOME") м=д.ǈ;else if(ΑΤ=="STOP") м=д.ΕΙ;else if(ΑΤ=="JOB") м=д.Βά;else if(ΑΤ=="IG") м=д.Ű;else ƚ=false;return ƚ;}void π(Ϊ ΓЇ){ΖΠ[(int) Ѝ]=љ;
љ=ΖΠ[(int) ΓЇ];if(ΓЇ==Ϊ.Ηύ) љ=0;Ѝ=ΓЇ;if(Η!=Ζ.с)
Ę(Ѝ==Ϊ.ŵ,false,0,0);ΖΡ=true;}void ΔЎ(bool Γω){if(љ>0) љ--;else if(Γω) љ=Αφ-1;}void ΔЍ(bool Γω){if(љ<Αφ-1) љ++;else if(Γω) љ=0;}String Ǽ(а ρ){switch(ρ){case а.ǉ:Κ="Job is running";break;case а.ŭ:Κ="Connector not ready!";break;case а.ΗϏ:Κ="Ship modified, path outdated!";break;
case а.ΔЅ:Κ="Interrupted by player!";break;case а.Ϋ:Κ="Shuttle mode enabled!";break;}return "";}String ΗΧ(Ǒ η){switch(η){case Ǒ.ΑΑ:return "Top-Left";case Ǒ.ΖЏ:return "Center";default:return "";}}String ΗΡ(ѐ Ƙ){switch(Ƙ){case ѐ.ΒΛ:return "Auto"+(Η==Ζ.Ŷ?" (Ore)":"");case ѐ.ΒΜ:return "Auto (+Stone)";
case ѐ.ΒΣ:return "Default";default:return "";}}String Βί(Μ Ф){switch(Ф){case Μ.з:return "No job";case Μ.ļ:return "Job paused";case Μ.Є:return "Job active";case Μ.ĳ:return "Job active";case Μ.Ĵ:return "Job active";case Μ.ΒΦ:return "Job done";case Μ.Ƴ:return "Job changed";case Μ.ǂ:return "Move home";
case Μ.ǃ:return "Move to job";}return "";}String Δω(д ΘΝ){switch(ΘΝ){case д.ǈ:return "Return home";case д.Βά:return "Fly to job pos";case д.ΕΙ:return "Stop";case д.Ű:return "Ignore";}return "";}String ΗΣ(Ϗ Ƙ){switch(Ƙ){case Ϗ.Βο:return "Off";case Ϗ.ΒΧ:return "Drop pos (Stone) ";case Ϗ.ƶ:return "Drop pos (Sto.+Ice)";
case Ϗ.Δξ:return "Cur. pos (Stone)";case Ϗ.ΒΠ:return "Cur. pos (Sto.+Ice)";case Ϗ.Βθ:return "In motion (Stone)";case Ϗ.ƽ:return "In motion (Sto.+Ice)";}return "";}String Δχ(р Ф){switch(Ф){case р.None:return "No batteries";case р.ē:return "Charging";case р.Δρ:return "Discharging";}return "";}String Βα(Α Ф)
{String ΚΗ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.ж:return "Idle";case Α.ǥ:return "Flying to XY position";case Α.Ļ:return Η==Ζ.Ǐ?"Grinding":"Mining";case Α.ǻ:return "Returning";case Α.ķ:return "Flying to drop pos";case Α.Ǻ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Ю:return "Flying to job area";case Α.ȫ:return "Flying to path";case Α.ĸ:return "Flying to job position";case Α.ǣ:return "Approaching dock";case Α.Ĕ:return "Docking";case Α.Đ:return "Aligning";case Α.ȧ:return "Aligning";case Α.ŷ:return "Retry docking";case Α.Ǿ:return "Unloading";case Α.ȭ:return ΖЄ;
case Α.ǒ:return "Undocking";case Α.ē:return "Charging batteries";case Α.ΑΖ:return "Waiting for uranium";case Α.Ƹ:return "Filling up hydrogen";case Α.ΒЊ:return "Waiting for ejection";case Α.ΑΕ:return "Waiting for ejection";case Α.ƹ:return "Flying to drop pos";}return "";}String ΔϏ(κ Ќ){switch(Ќ)
{case κ.Βρ:return "On \"Undock\" command";case κ.Βυ:return "On player entered cockpit";case κ.ΔБ:return "On ship is full";case κ.ΔА:return "On ship is empty";case κ.Βφ:return "On time delay";case κ.ǆ:return "On batteries empty(<25%)";case κ.ǅ:return "On batteries empty(=0%)";case κ.Βπ:return "On batteries full";
case κ.Βτ:return "On hydrogen empty(<25%)";case κ.Βς:return "On hydrogen empty(=0%)";case κ.Βσ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Ζδ,int Ą,bool Ξ,String Ɣ){Αφ+=1;if(Ζδ==Ą) Ɣ=">"+Ɣ+(ΑΛ>=2?" .":"");else Ɣ=" "+Ɣ;Γ+=Ɣ+"\n";return Ζδ==Ą&&Ξ;}String Δζ(bool Ξ)
{int Β=0;int Ο=љ;Αφ=0;String Ζη="———————————————\n";String ŧ="--------------------------------------------\n";String Γ="";Γ+=Βί(ί)+" | "+(Θ.ε?"Ready to dock":"No dock")+"\n";Γ+=Ζη;double ǟ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ϊ.Main){bool ć=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЅ();
if(Η==Ζ.Ŷ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.ŵ);if(Η==Ζ.Ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.ŵ);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.ΒЁ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȩ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƺ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) ĕ();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(ć) π(Ϊ.ǋ);else π(Ϊ.Ʊ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.ΔЂ);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.ΔЀ);}else if(Ѝ==Ϊ.ŵ){double Ια=Math.Round(й*ǡ,1);double Θζ=Math.Round(и*ǳ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) ΒЄ();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){Ż(false,false);π(Ϊ.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+Ια+"m)")){ĵ(ref й,5,20,1);ę(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){ĵ(ref й,-5,20,-1);ę(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+Θζ+"m)")){ĵ(ref и,5,20,1);ę(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){ĵ(ref и,-5,20,-1);ę(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(ф==ѐ.ΒΣ?"Depth":"Min")+": "+Ц+"m)")){ĵ(ref Ц,5,50,2);ę(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){ĵ(ref Ц,-5,50,-2);ę(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+ΗΡ(ф))){ф=Ȫ(ф);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΗΧ(ȣ))){ȣ=Ȫ(ȣ);}if(Η==Ζ.Ǐ&&ф==ѐ.ΒΜ) ф=Ȫ(ф);Γ+=ΕΔ(8,Ѓ,Ο,ref ΘА);}else if(Ѝ==Ϊ.ΒЁ){float[]Επ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΗ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔϏ(ό.Ќ))) ό.Ќ=Ȫ(ό.Ќ);if(!ό.Βε()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βή((int) ό.ǖ))) ό.ǖ=Δο(ό.ǖ,Επ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+ΔϏ(ύ.Ќ))) ύ.Ќ=Ȫ(ύ.Ќ);
if(!ύ.Βε()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+Βή((int) ύ.ǖ))) ύ.ǖ=Δο(ύ.ǖ,Επ);}else if(Ѝ==Ϊ.ΕΗ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЄ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.ΒЁ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǘ!=""?ό.Ǘ:"-"))) ό.Ǘ=Ǆ(ref ΙΟ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǚ!=""?ό.ǚ:"-"))) ό.ǚ=Ǆ(ref ΙΡ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.Ǘ!=""?ύ.Ǘ:"-"))) ύ.Ǘ=Ǆ(ref ΙΠ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.ǚ!=""?ύ.ǚ:"-"))) ύ.ǚ=Ǆ(ref ΙΣ);
}else if(Ѝ==Ϊ.Ŵ){String ΖΚ=ǯ+" %";if(Ĉ) ą++;if(ą>1){ą=0;Αδ++;if(Αδ>1) Αδ=0;}bool[]ΔΕ=new bool[]{Ċ.Count==0,ǩ==р.None,і.Count==0};int ΖЀ=0;while(true){ΖЀ++;Ĥ++;if(Ĥ>ΔΕ.Length-1) Ĥ=0;if(ΖЀ>=ΔΕ.Length) break;if(!ΔΕ[Ĥ]) break;}bool ć=Η==Ζ.Ϋ;if(!ć&&Ȧ&&О!=-1&&Αδ==0)
ΖΚ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){ψ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!ć) π(Ϊ.Ʊ);else π(Ϊ.ǋ);if(!ć){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΕ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƨ=true;Γ+=ŧ;if(!ć) Γ+="Progress: "+Math.Round(ƒ,1)+" %\n";Γ+="State: "+Βα(Φ)+" "+ǟ+"m \n";Γ+="Load: "+Αμ+" % Max: "+ΖΚ+" \n";if(Ĥ==0) Γ+="Uranium: "+(Ċ.Count==0?"No reactors":Math.Round(İ,1)+"Kg "+(Ы==-1?"":" Min: "+Ы+" Kg"))+"\n";
if(Ĥ==1) Γ+="Battery: "+(ǩ==р.None?Δχ(ǩ):ǿ+"% "+(ш==-1||ć?"":" Min: "+ш+" %"))+"\n";if(Ĥ==2) Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(Ț,1)+"% "+(щ==-1||ć?"":" Min: "+щ+" %"))+"\n";}else if(Ѝ==Ϊ.Ʊ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.Ŵ);else π(Ϊ.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) г(ref ǯ,ǯ<=80?-10:-5,σ.ǀ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(Ȧ?"On":"Off"))) Ȧ=!Ȧ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+ΗΣ(М))) М=Ȫ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(Į?"On":"Off"))){Į=!Į;if(Į) ǧ(ΔΘ);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(ȥ?"On":"Off"))) ȥ=!ȥ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǁ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Min "+ш+"%"))) г(ref ш,-5,σ.Βμ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Min "+щ+"%"))) г(ref щ,-10,σ.Βν,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(Ď?"Return home":"Stop"))) Ď=!Ď;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δω(м))) м=Ȫ(м);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΖ);Γ+=ΕΔ(8,Ѓ,Ο,ref ΘБ);}else if(Ѝ==Ϊ.ΒΖ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.Ŵ);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǐ?" Grinder":" Drill")+" inv. balancing: "+(Ő?"On":"Off"))) Ő=!Ő;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǐ?" grinders":" drills")+": "+(ŉ?"Fwd + Bwd":"Fwd"))) ŉ=!ŉ;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ū+"m/s")) г(ref ū,0.5f,σ.ż,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ū+"m/s")) г(ref Ū,0.5f,σ.ż,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) г(ref Ǩ,0.1f,σ.Ư,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+Ĳ*100f+"%")) Δκ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ģ*100f+"%")) Δκ(false,0.05f);
}else if(Ѝ==Ϊ.ǋ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Μ.Є) π(Ϊ.Ŵ);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+ǯ+"%")) г(ref ǯ,ǯ<=80?-10:-5,σ.ǀ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȥ?"On":"Off"))) ȥ=!ȥ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ы==-1?"Ignore":"Min "+Ы+"Kg"))) г(ref Ы,(Ы>5?-5:-1),σ.ǁ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Charge up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Fill up"))) щ=(щ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δω(м))) м=Ȫ(м);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǩ*100f)+"%"+(Ǩ>0.80f?" (risky)":""))) г(ref Ǩ,0.1f,σ.Ư,false);
}else if(Ѝ==Ϊ.ΕΓ){double Θς=0;if(Σ.Count>0) Θς=Vector3.Distance(Σ.Last().Δ,ή);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΛ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȍ?"Use old home":(Θ.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȍ?"Use old connector":(Θ.ε?"Was set! ":"none ")))) Ȍ=!Ȍ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ũ?"Use old connector":(Ε.ε?"Was set! ":"none ")))) Ũ=!Ũ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ũ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ũ=!ũ;Γ+=ŧ;Γ+="Wp spacing: "+Math.Round(ΖІ)+"m\n";}else if(Ѝ==Ϊ.ΒІ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){ψ();π(Ϊ.Main);}
Γ+=ŧ;Γ+="State: "+Βα(Φ)+" \n";Γ+="Speed: "+Math.Round(Ȣ,1)+"m/s\n";Γ+="Target dist: "+ǟ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+Δά+"\n";}else if(Ѝ==Ϊ.ΔЂ){List<IMyTerminalBlock>Ġ=Δϊ();if(Ĉ) ą++;if(ą>=Ġ.Count) ą=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔЃ);
Γ+=ŧ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αμ,1)+"% "+Math.Round(ň,1)+" / "+Math.Round(Ŗ,1)+"\n";Γ+="Uranium: "+(Ċ.Count==0?"No reactors":Math.Round(İ,1)+"Kg "+Γϋ)+"\n";Γ+="Battery: "+(ǩ==р.None?"":ǿ+"% ")+Δχ(ǩ)+"\n";Γ+="Hydrogen: "+(і.Count==0?"No tanks":Math.Round(Ț,1)+"% ")+"\n";
Γ+="Damage: "+(Ġ.Count==0?"None":""+(ą+1)+"/"+Ġ.Count+" "+Ġ[ą].CustomName)+"\n";}else if(Ѝ==Ϊ.ΔЃ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="Next scan: "+ΑЁ+"s\n";Γ+="Drill area: "+Math.Round(ǡ,1)+"x"+Math.Round(ǳ,1)+"m Ship: "+Math.Round(ΑЂ,1)+"x"+Math.Round(ΔΒ,1)+"m ("+Math.Round(δ,1)+"m)\n"+(Ǫ.Count>0?"Cameras: "+ΓΛ+"\n":"");
Γ+="Broadcast: "+(ņ?"Online - "+ΔΓ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ŕ*100f,1)+"% \n";}else if(Ѝ==Ϊ.ΔЀ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=ŧ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+ΑЁ+"s\n";if(ΑϏ) Γ="Recompile script now";int Θσ=Γ.Split('\n').Length;for(int i=Θσ;i<=10;i++) Γ+="\n";Γ+=Ζη;Γ+="Last: "+Κ+"\n";
return Γ;}void Δκ(bool Θν,float ѝ){ψ();Ż(true,false);if(Θν) г(ref Ĳ,ѝ,σ.Βχ,false);else г(ref ģ,ѝ,σ.Βχ,false);Δι();Ę(true,true,0,0);}float Δο(float ι,float[]ΒΓ){float έ=ΒΓ[0];for(int i=ΒΓ.Length-1;i>=0;i--)
if(ι<ΒΓ[i]) έ=ΒΓ[i];return έ;}String Ǆ(ref int Ą){String έ="";if(Ą>=ΔΡ.Count) Ą=-1;if(Ą>=0) έ=ΔΡ[Ą].CustomName;Ą++;return έ;}void ΑΒ(string φ){if(ί!=Μ.Є) return;if(φ=="") return;IMyTerminalBlock Υ=ϋ.GetBlockWithName(φ);if(Υ==null||!(Υ is IMyTimerBlock)){Κ="Timerblock "+φ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void ĵ(ref int value,int Ƃ,int Ȥ,int ΔΖ){if(Ƃ==0) return;if(value<Ȥ&&ΔΖ>0||value<=Ȥ&&ΔΖ<0){value+=ΔΖ;return;}int ΘΕ=Math.Abs(Ƃ);int ΒЋ=0;int ΓЂ=1;while(true){ΒЋ+=ΓЂ*ΘΕ*10;if(Ƃ<0&&value-Ȥ<=ΒЋ) break;if(Ƃ>0&&value-Ȥ<ΒЋ) break;
ΓЂ++;}value+=ΓЂ*Ƃ;}void ę(bool Κή){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);Ę(Ѝ==Ϊ.ŵ,false,0,0);}bool Ė(ref float value,bool ΘЌ,σ ς,String ĥ,String Θκ){if(ĥ=="") return false;float ś=-1;bool ΕЍ=false;if(ĥ.ToUpper()==Θκ) ΕЍ=true;else if(!float.TryParse(ĥ,out ś)) return false;
else ś=Math.Max(0,ś);if(ΘЌ) ś=(float) Math.Round(ś);г(ref value,ś,ς,ΕЍ,false);return true;}void г(ref float value,float ѝ,σ ς,bool Ʀ){г(ref value,value+ѝ,ς,Ʀ,true);}void г(ref float value,float ŗ,σ ς,bool Ʀ,bool Ιε){float ǜ=0,Ǜ=0;if(ς==σ.ż){ǜ=0.5f;Ǜ=10f;}if(ς==σ.Ư){ǜ=0.1f;Ǜ=1f;}
if(ς==σ.Ű){ǜ=50f;Ǜ=100f;}if(ς==σ.Βμ){ǜ=5f;Ǜ=30f;}if(ς==σ.ǁ){ǜ=1f;Ǜ=25f;}if(ς==σ.ǀ){ǜ=10f;Ǜ=95f;}if(ς==σ.Βν){ǜ=10f;Ǜ=90f;}if(ς==σ.ΗЅ){ǜ=10f;Ǜ=1800;}if(ς==σ.Βχ){ǜ=0.0f;Ǜ=0.75f;}if(ŗ==-1&&Ʀ){value=-1;return;}if(value==-1) Ʀ=false;
bool ΖΫ=ŗ<ǜ||ŗ>Ǜ;if(ΖΫ&&Ιε){if(ŗ<value) value=Ǜ;else if(ŗ>value) value=ǜ;}else value=ŗ;if(ΖΫ&&Ʀ) value=-1;else value=Math.Max(ǜ,Math.Min(value,Ǜ));value=(float) Math.Round(value,2);}void ΒΕ(bool ΘЈ){if(ΘЈ) Й=Math.Max(0,Й-1);else Й++;ΒΩ();ΒΨ();for(int k=0;k<Ȇ.Count&&k<Л.Length;k++) ΒϏ(Ȇ[k],k<Й);
Ѐ=-1;ΒЉ(true);}T Ȫ<T>(T ι){int Β=Array.IndexOf(Enum.GetValues(ι.GetType()),ι);Β++;if(Β>=ΗΞ(ι)) Β=0;return(T) Enum.GetValues(ι.GetType()).GetValue(Β);}int ΗΞ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЅ(){ƭ.Clear();for(int i=0;i<Σ.Count;i++) ƭ.Add(Σ[i]);Σ.Clear();
Αθ=true;Γξ=new Ψ(Θ);Γρ=new Ψ(Ε);Θ.ε=false;if(Η==Ζ.Ϋ) Ε.ε=false;for(int i=0;i<ў.Count;i++)
if(!Ȋ.Contains(ў.Keys.ElementAt(i)))
Ȋ.Add(ў.Keys.ElementAt(i));Ȍ=false;Ũ=false;ũ=false;π(Ϊ.ΕΓ);}void ΕΛ(){if(Ȍ) Θ=Γξ;if(Ũ) Ε=Γρ;if(ũ){Σ.Clear();for(int i=0;i<ƭ.Count;i++) Σ.Add(ƭ[i]);}Αθ=false;ψ();π(Ϊ.Main);}void ΗА(){if(!Αθ) return;if(Φ!=Α.ж){ΕΛ();return;}if(!Γξ.ε) Ȍ=false;if(!Γρ.ε) Ũ=false;if(ƭ.Count<=1) ũ=false;
IMyShipConnector И=П(MyShipConnectorStatus.Connectable);if(И==null) И=П(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(Ȣ,2)<=0.20) ΓΨ++;else ΓΨ=0;if(ΓΨ>=5){if(Η==Ζ.Ϋ&&(Θ.ε||Ȍ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.β=Τ.WorldMatrix.Forward;Ε.υ=Τ.WorldMatrix.Left;
Ε.α=Τ.WorldMatrix.Down;Ε.ν=Τ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.ε=true;Ε.Ч=И.Position;}else{Θ.β=Τ.WorldMatrix.Forward;Θ.υ=Τ.WorldMatrix.Left;Θ.α=Τ.WorldMatrix.Down;Θ.ν=Τ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.ε=true;Θ.Ч=И.Position;}}}double Γσ=-1;if(Σ.Count>0) Γσ=Vector3.Distance(ή,Σ.Last().Δ);
double ΙΖ=Math.Max(1.5,Math.Pow(Ȣ/100.0,2));double ΖΣ=Math.Max(Ȣ*ΙΖ,2);ΖІ=ΖΣ;if((Γσ==-1)||Γσ>=ΖΣ){Ψ Ω=new Ψ(ή,Αα,Ɗ,ƕ,Τ.GetNaturalGravity());Ω.ΖЋ(ȋ,Ȋ);Σ.Add(Ω);}}Ψ е(){if(Η!=Ζ.Ϋ) return њ!=null?њ:Θ;return Ελ;}void Ľ(Ψ ά,Μ Ф){Ελ=ά;if(ί==Μ.Є) ч=Ф;}bool Ǹ(Ψ ά,float ω,bool Ιά,out Vector3 η)
{if(Ιά){Vector3I Γζ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(Γζ);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){η=new Vector3();return false;}Vector3 ΘΤ=Я(Τ,Υ.FatBlock.GetPosition()-ή);Vector3 ΘΣ=Я(Τ,Υ.FatBlock.WorldMatrix.Forward);η=ά.Δ-ΒЈ(ά.β,ά.α*-1,ΘΤ)-ΒЈ(ά.β,ά.α*-1,ΘΣ)*ω;
return true;}else{η=ά.Δ;return true;}}Vector3 ȏ(int ΘΠ,int ş,bool ΘЊ){if(!ΘЊ&&ΕЄ) return ΓΚ;float ΘΡ=((Н-1f)/2f)-ΘΠ;float ΘЍ=((б-1f)/2f)-ş;ΓΚ=Ε.Δ+Ε.υ*ΘΡ*ǡ+ȟ*-1*ΘЍ*ǳ;ΕЄ=true;return ΓΚ;}Vector3 Βΰ(Vector3 ΙΚ,float ΓΥ){return ΙΚ+(ъ*ΓΥ);}ΰ ΒΤ(){float Б=-1;ΰ έ=ΰ.ǵ;
if(Η!=Ζ.Ϋ){ΔЌ();if(ί!=Μ.з){Vector3 Љ=ė(ъ,ȟ*-1,ή-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(Н*ǡ)/2f&&Math.Abs(Љ.Y)<=(float)(б*ǳ)/2f){if(Љ.Z<=-1&&Љ.Z>=-ī*2) return ΰ.ư;if(Љ.Z>-1&&Љ.Z<δ*2) return ΰ.ǵ;}if(Ȩ(Ε.Δ,ref Б)) έ=ΰ.ǵ;}if(Θ.ε){if(Ȩ(Θ.Δ,ref Б)) έ=ΰ.Ǵ;for(int i=0;i<Σ.Count;i++)
if(Ȩ(Σ[i].Δ,ref Б)) έ=ΰ.Ȑ;if(Vector3.Distance(ή,Θ.Δ)<dockDist*δ) έ=ΰ.Ǵ;if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.Ǵ;}}else{Vector3 η=new Vector3();IMyShipConnector ȃ=П(MyShipConnectorStatus.Connected);if(Θ.ε){if(Ȩ(Θ.Δ,ref Б)) έ=ΰ.Ǵ;if(Ǹ(Θ,dockDist,true,out η))
if(Ȩ(η,ref Б)) έ=ΰ.Ǵ;if(ȃ!=null&&Vector3.Distance(ȃ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΙ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*δ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*δ)
if(Ȩ(Σ[i].Δ,ref Б)) έ=ΰ.Ȑ;if(Ε.ε){if(Ȩ(Ε.Δ,ref Б)) έ=ΰ.ǵ;if(Ǹ(Ε,dockDist,true,out η))
if(Ȩ(η,ref Б)) έ=ΰ.ǵ;if(ȃ!=null&&Vector3.Distance(ȃ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΚ;}}return έ;}bool Ȩ(Vector3 η,ref float Б){float ω=Vector3.Distance(η,ή);if(ω<Б||Б==-1){Б=ω;return true;}return false;}void ΒЄ(){if(ώ>0){Κ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){ȩ();return;}Ε.Δ=ή;Ε.ν=Τ.GetNaturalGravity();Ε.β=Αα;Ε.α=Ɗ;Ε.υ=ƕ;
ъ=ћ.WorldMatrix.Forward;ȟ=Ε.α;if(ъ==Τ.WorldMatrix.Down) ȟ=Τ.WorldMatrix.Backward;Ż(true,true);Ν(Α.ǥ);ΒЃ();}void Ż(bool ǘ,bool ΘЋ){if(ί==Μ.з&&!ǘ) return;bool Θύ=ǘ||ί==Μ.ΒΦ||Н!=й||б!=и||Ħ!=ȣ;if(Θύ){if(ί!=Μ.з){ί=Μ.Ƴ;ȏ(Ш,Щ,ΘЋ);Κ="Job changed, lost progress";}Ħ=ȣ;
Н=й;б=и;Щ=0;Ш=0;Л="";Ѐ=-1;ĩ=0;ȝ=0;С=0;Й=0;ΒЉ(true);}}void Βϋ(){Р(ή,0);ѓ(ȋ,true);}void ΒЃ(){Ǽ(а.ǉ);Βϋ();Ź(ȇ,false);ǧ(ΔΘ);ί=Μ.Є;Δθ(true);ч=ί;π(Ϊ.Ŵ);Ησ();ΑΥ=true;Αε=0;for(int i=Ž.Count-1;i>=0;i--)
if(Ų(Ž[i],false)) Αε++;if(Αε>0) Κ="Started with damage";}void ψ(){if(ί==Μ.Є){ί=Μ.ļ;Κ="Job paused";}Ν(Α.ж);ч=ί;Ǌ(false,0,0,0,0);ΒЇ();Δε(new Vector3(),false);ΕΚ();Βώ(ChargeMode.Auto);ΒЀ(false);ΕΖ(true);ȑ(Α.ж);Ę(false,false,0,0);ѓ(ο,false);ѓ(у,true);ǧ(true);ΑЀ=false;Ƶ();Ɖ=false;
Βό();ΑΥ=false;Ơ=false;Ƨ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.Ʊ&&Ѝ!=Ϊ.ΒΖ&&Ѝ!=Ϊ.ǋ)
π(Ϊ.Main);}void ȩ(){ΰ Д=ΒΤ();if(Η==Ζ.Ϋ){if(!Ε.ε||!Θ.ε) return;ΒЃ();bool Γδ=Vector3.Distance(ή,Θ.Δ)<Vector3.Distance(ή,Ε.Δ);if(Ţ==Μ.ĳ) Γδ=true;if(Ţ==Μ.Ĵ) Γδ=false;if(Γδ){Ľ(Θ,Μ.ĳ);switch(Д){case ΰ.ΒΙ:Ν(Α.ȭ);break;case ΰ.Ȑ:Ν(Α.Ю);break;case ΰ.Ǵ:Ν(Α.ǣ);break;default:Ν(Α.ǒ);break;
}}else{Ľ(Ε,Μ.Ĵ);switch(Д){case ΰ.ΒΚ:Ν(Α.ȭ);break;case ΰ.ǵ:Ν(Α.ǣ);break;case ΰ.Ȑ:Ν(Α.Ю);break;default:Ν(Α.ǒ);break;}}}else{if(ί!=Μ.ļ&&ί!=Μ.Ƴ) return;bool Ιί=ί==Μ.Ƴ;ΒЃ();bool Γι=ų(false)&&Θ.ε;switch(Д){case ΰ.ǵ:Ν(Γι?Α.ȫ:Α.ǥ);break;case ΰ.Ȑ:Ν(Γι?Α.Ѕ:Α.Ю);break;case ΰ.Ǵ:Ν(Γι?Α.ǣ:Α.Ǿ);break;
case ΰ.ư:{if(ȝ!=Й||Ιί) Ν(Α.ǻ);else Ν(Α.Ļ);}break;default:break;}}}void ĕ(){if(ί==Μ.з&&!Θ.ε) return;if(Η==Ζ.Ϋ&&(!Ε.ε||!Θ.ε)) return;Κ="Move to job";ΰ Д=ΒΤ();if(Η==Ζ.Ϋ){Ľ(Ε,Μ.Ĵ);switch(Д){case ΰ.ǵ:Ν(Α.ǣ);break;case ΰ.Ȑ:Ν(Α.Ю);break;case ΰ.ΒΚ:return;default:Ν(Α.ǒ);break;
}ȑ(Α.ȭ);}else{switch(Д){case ΰ.ǵ:Ν(Α.Ю);break;case ΰ.Ȑ:Ν(Α.Ю);break;case ΰ.Ǵ:Ν(Α.Ǿ);break;case ΰ.ư:Ν(Α.ǻ);break;default:break;}if(ί==Μ.з) ȑ(Α.Ю);else ȑ(Α.ȧ);ΑЀ=true;}Βϋ();π(Ϊ.ΒІ);Ź(ȇ,false);ч=Μ.ǃ;}void ƺ(){if(!Θ.ε) return;Κ="Move home";ΰ Д=ΒΤ();if(Η==Ζ.Ϋ){Ľ(Θ,Μ.ĳ);switch(Д)
{case ΰ.Ȑ:Ν(Α.Ѕ);break;case ΰ.Ǵ:Ν(Α.ǣ);break;case ΰ.ΒΙ:return;default:Ν(Α.ǒ);break;}ȑ(Α.ȭ);}else{if(П(MyShipConnectorStatus.Connected)!=null) return;if(П(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Ĕ);ȑ(Α.Ǿ);return;}switch(Д){case ΰ.ǵ:Ν(Α.ȫ);break;case ΰ.Ȑ:Ν(Α.Ѕ);break;case ΰ.Ǵ:Ν(Α.Ѕ);break;
case ΰ.ư:Ν(Α.Ǻ);break;default:break;}ȑ(Α.Ǿ);}Βϋ();π(Ϊ.ΒІ);Ź(ȇ,false);ч=Μ.ǂ;}void Ν(Α ΓЃ){if(ΓЃ==Α.ж) Ƣ=Α.ж;if(Ƣ!=Α.ж&&Φ==Ƣ&&ΓЃ!=Ƣ){ψ();return;}Ё=true;Φ=ΓЃ;}void ȑ(Α Ф){this.Ƣ=Ф;}void ΗЎ(){if(Φ==Α.ǥ){if(Ё){Ђ=0;if(ȝ!=Й) ĩ=0;ȝ=Й;}if(Ђ==0){int ƃ=ΗΒ(Й);if(ƃ<0)
{if(!ƴ()){Ν(Α.Ǻ);return;}if(!ΑΠ||(ƃ==-2)!=ΑΟ){ΑΠ=true;ΑΟ=ƃ==-2;Εθ=!ΑΟ?ή:Ε.Δ-ъ*δ+
Ε.υ*(Н*ǡ/2+ΑЂ/2+δ/2);}Р(Εθ,10);Κ=ΑΟ?"Waiting for a free hole":"Waiting for the controller";return;}if(ΑΠ) Κ="";ΑΠ=false;if(ƃ!=Й){Й=ƃ;ĩ=0;ȝ=Й;}ľ έ=Ηϊ(Й);if(έ==ľ.Δδ){ί=Μ.ΒΦ;Κ="Job done";if(Ď&&Θ.ε){Ν(Α.ȫ);ȑ(Α.Ǿ);ч=Μ.ǂ;}else{Ν(Α.ĸ);ȑ(Α.ȧ);ч=Μ.ǃ;}
return;}if(έ==ľ.Δφ){Ђ=1;ѓ(ο,true);л=ȏ(Ш,Щ,true);Р(л,10);Х(Ε.α,Ε.β,Ε.υ,false);}}else{if(Ѐ>=0&&(ΔЉ()?Ǭ>=0&&Ǭ!=Ѐ:Βη(Ѐ,true))){Ђ=0;return;}if(К<wpReachedDist){Ν(Α.Ļ);return;}}}if(Φ==Α.Ļ){if(Ё){ѓ(ο,true);ǧ(false);л=ȏ(Ш,Щ,false);Р(Βΰ(л,0),0);Х(Ε.α,Ε.β,Ε.υ,false);
Ђ=1;к=0;Э=0;ΑΜ=0;Γτ=-1;ī=Ц;Γο=true;}if(!ƴ()){Ν(Α.Ǻ);return;}if(ų(true)){ΖΩ=Ȏ("","ORE",З.đ);if((М==Ϗ.ΒΧ||М==Ϗ.ƶ||
М==Ϗ.Βθ||М==Ϗ.ƽ)&&Η!=Ζ.Ǐ)
Ν(Α.ķ);else if((М==Ϗ.Δξ||М==Ϗ.ΒΠ)&&Η!=Ζ.Ǐ)
Ν(Α.ΒЊ);else
Ν(Α.Ǻ);return;}С=Vector3.Distance(ή,л);if(С>ĩ){ĩ=С;Γο=false;}if(Η==Ζ.Ǐ&&Δϋ()==MyDetectedEntityType.SmallGrid)
Э+=2;else Э-=2;Э=Math.Max(100,Math.Min(400,Э));if(Ђ>0&&Ђ<Э){if(С>к){if(Э>150) к=С;else к=(float) Math.Ceiling(С);Ђ=1;}else Ђ++;}else{if(Ђ>0){Κ="Ship stuck! Retrying";к=С;Ђ=0;Ę(false,true,0,δ*sensorRange);}Р(Βΰ(л,Math.Max(0,к-δ)),Βγ(false));if(К<=wpReachedDist/2){Ђ=1;к=0;}
return;}Ę(false,true,δ*sensorRange,0);Vector3 ΘΗ=л+ъ*С;bool ΕБ=false;if(Vector3.Distance(ΘΗ,ή)>0.3f){Vector3 ΘΥ=л+ъ*(С+0.1f);Р(ΘΥ,4);ΕБ=true;}else{float ǝ=Βγ(true);Vector3 Εο=Βΰ(л,Math.Max(Ц+1,С+1));Ǧ(true,false,false,Εο,Εο-л,ǝ,ǝ);}bool ΓΦ=false;if(ф==ѐ.ΒΜ||ф==ѐ.ΒΛ)
{if(!ΕБ){float ΓЅ=0;foreach(IMyTerminalBlock ǲ in ο)
ΓЅ+=ΗΙ(ǲ,"","",ф==ѐ.ΒΛ?new string[]{"STONE"}:null);if(ΓЅ>Γτ||С<Ц||Γο){ΒБ=0;ΑΜ=С;ī=(int)(Math.Max(ī,ΑΜ)+δ/2);}else{ΓΦ=С-ΑΜ>2&&ΒБ>=20;ΒБ++;}Γτ=ΓЅ;}}else ΓΦ=С>=ī;if(ȝ!=Й){Ν(Α.ǻ);С=0;return;}if(ΓΦ){Й++;Ν(Α.ǻ);С=0;return;}}if(Φ==Α.ǻ||Φ==Α.Ǻ||Φ==Α.ķ)
{if(Ё){л=ȏ(Ш,Щ,false);Х(Ε.α,Ε.β,Ε.υ,false);ѓ(ο,ŉ);ǧ(false);к=Vector3.Distance(ή,л);Ę(false,true,0,δ*sensorRange);}Р(л,Βγ(false));if(Vector3.Distance(ή,л)>=к+5){ѓ(ο,false);ǧ(true);Κ="Can´t return!";}if(К<wpReachedDist){if(Φ==Α.ǻ&&ΑЀ) Ν(Α.ĸ);if(Φ==Α.ǻ) Ν(Α.ǥ);
if(Φ==Α.ķ) Ν(Α.ƹ);if(Φ==Α.Ǻ){if(Θ.ε) Ν(Α.ȫ);else{ψ();ĕ();Κ="Can´t return, no dock found";}}return;}}if(Φ==Α.Ѕ||Φ==Α.Ю){if(Φ==Α.Ю&&ί==Μ.Є&&Η!=Ζ.Ϋ){if(!ƴ()||ų(true)){Ν(Α.Ѕ);return;}}bool ΓЎ=false;bool ΒΑ=false;bool ΕΪ=false;float Ń=0;bool ΖΨ=false;Ψ Ω=null;
if(Ё){if(Φ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȓ(1)) ΒΘ();else{њ=null;Ȉ=0;Ɖ=!ΒΟ(1);}}Ψ ά=е();θ=new Βω();θ.Њ=ά;θ.Ƈ=followPathDock*δ;θ.ΔΨ=useDockDirectionDist*δ;θ.Ń=10;θ.ť.Add(ά.Δ);Vector3 ł=new Vector3();if(Ǹ(ά,dockDist*δ,true,out ł))
θ.ť.Add(ł);else θ.Ƈ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) θ.Αή=Ε.Δ;if(ά==Ε) θ.Αή=Θ.Δ;θ.Γί=dockDist*δ*1.1f;}}else if(Φ==Α.Ю){θ=new Βω();θ.Њ=Ε;θ.Ƈ=followPathJob*δ;θ.ΔΨ=useJobDirectionDist*δ;θ.Ń=10;θ.Αή=Θ.Δ;θ.Γί=dockDist*δ*1.1f;θ.ť.Add(Ε.Δ);if(ί==Μ.з){if(!Θ.ε||Σ.Count==0){ψ();return;}
float Θά=Vector3.Distance(Σ.First().Δ,Θ.Δ);float Θέ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(Θά<Θέ) θ.Њ=Σ.Last();else θ.Њ=Σ.First();}}ƛ=new Vector3();ΖΨ=!Ηΰ(ή);ѓ(ο,false);ǧ(true);У=-1;double ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,θ.Αή)<=θ.Γί) continue;
double ω=Vector3.Distance(Σ[i].Δ,ή);if(ǫ==-1||ω<ǫ){У=i;ǫ=ω;}}ǰ=ΗΦ(θ.Њ.Δ,У);ğ=null;}if(Φ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&!ź()){Ψ ΓΔ=е();Ɖ=!ΒΟ(1);if(е()!=ΓΔ){Ν(Α.Ѕ);return;}}if(Φ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&ȓ(1)&&ΒΘ()){Ν(Α.Ѕ);return;}bool ΕЏ=false;if(Φ==Α.Ѕ&&!Ё&&У>=0&&У<Σ.Count)
{Vector3 ƅ=Σ[У].Δ+Δό(У,ǰ);int ΓЄ=У+ǰ;Vector3 Ř=ǰ!=0&&ΓЄ>=0&&ΓЄ<Σ.Count?Σ[ΓЄ].Δ:θ.Њ.Δ;if(ΗΩ(ƅ,Ř)){if(!ΑΨ){ΑΨ=true;Ετ=Ĺ(ƅ)?ή:ƅ;}Р(Ετ,0);Х(θ.Њ.α,θ.Њ.β,θ.Њ.υ,false);Κ="Waiting for dock clearance";return;}ΑΨ=false;ΕЏ=ȓ(1)&&(Ĺ(ή)||Ĺ(Ř));}ΖЌ(Σ,ǰ,θ.ť,θ.Ƈ,Ё,ref Ђ);
for(int i=0;i<θ.ť.Count;i++){float ω=Vector3.Distance(ή,θ.ť[i]);if(ω<=θ.Ƈ) ΓЎ=true;if(ω<=θ.ΔΨ) ΒΑ=true;}if(ΕЏ) ΒΑ=true;if(ΒΑ) Ń=θ.Ń;float Κΰ=ğ!=null?ğ.Џ:Ȣ;float Ιδ=(float) Math.Max(Ȣ*0.1f*δ,wpReachedDist);if((К<Ιδ)||Ё){if(!Ё) У+=ǰ;if(ǰ==0||У>Σ.Count-1||У<0)
ΓЎ=true;else{Δά=ǰ>0?Σ.Count-1-У:У;Ω=Σ[У];ğ=Ω;if(У>=1&&У<Σ.Count-1)
ƛ=Ω.Δ-Σ[У-ǰ].Δ;else ğ=null;χ=Ω.Δ+Δό(У,ǰ);ΕΪ=true;}}if(ΒΑ)
Х(θ.Њ.α,θ.Њ.β,θ.Њ.υ,false);else if(ΖΨ)
ΒΗ(θ.Њ.α,10,true);else if(ΕΪ&&Ω!=null){if(ǰ>0) Х(Ω.α,Ω.β,Ω.υ,90,false);else Х(Ω.α,-Ω.β,-Ω.υ,90,false);}Ǧ(true,false,true,χ,ƛ,ğ==null?0:ğ.Џ,Ń);if(ΓЎ){Δά=0;if(Φ==Α.Ѕ&&!ΖБ()){Р(χ,0);return;}if(Φ==Α.Ѕ&&Ɖ){Р(χ,0);Κ="Waiting for a free dock";return;}if(Φ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.ǣ);return;}
if(Φ==Α.Ю&&ΑЀ){Ν(Α.ĸ);return;}if(Φ==Α.Ю){Ν(Α.ǥ);return;}}}if(Φ==Α.ǣ||Φ==Α.ŷ){if(Η!=Ζ.Ϋ&&ź()){if(!ȓ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΔ=е();if(!ΒΟ(2)||е()!=ΓΔ){Ν(Α.Ѕ);return;}}Ψ ά=е();if(Ё){if(!Ǹ(ά,dockDist*δ,true,out χ)){Ǽ(а.ŭ);ψ();return;}
Р(χ,0);ΒΗ(ά.α,90,true);}if(К<followPathDock*δ&&К!=-1){Р(χ,10);Х(ά.α,ά.β,ά.υ,false);}if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null){Ν(Α.Ĕ);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.Đ);return;}}if(Φ==Α.Đ||Φ==Α.ȧ){if(Ё)
{if(Φ==Α.Đ){Ψ ά=е();if(!Ǹ(ά,dockDist*δ,true,out χ)){Ǽ(а.ŭ);ψ();return;}Ǧ(true,true,false,χ,0);Х(ά.α,ά.β,ά.υ,10,false);}if(Φ==Α.ȧ){Х(Ε.α,Ε.β,Ε.υ,0.5f,false);χ=Ε.Δ;Ǧ(true,true,false,χ,0);}}if(Αη){Ǌ(false,0,0,0,0);if(Φ==Α.Đ) Ν(Α.Ĕ);if(Φ==Α.ȧ) ψ();return;}}if(Φ==Α.Ĕ)
{if(П(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.ȭ);else Ν(Α.Ǿ);return;}Ψ ά=е();if(Ё){Э=0;Θή=DateTime.Now;Ђ=0;Х(ά.α,ά.β,ά.υ,false);}Vector3I Γζ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Εη=Me.CubeGrid.GetCubeBlock(Γζ);float Εφ=dockingSpeed;float Θφ=dockingSpeed*5;
float ΕΧ=Math.Max(1.5f,Math.Min(5f,δ*0.15f));if(!Ǹ(ά,0,true,out χ)||!Ǹ(ά,ΕΧ,true,out ƛ)||
Εη==null||!Εη.FatBlock.IsFunctional){Ǽ(а.ŭ);ψ();return;}if(Э==1||(Vector3.Distance(ή,χ)<=ΕΧ*1.1f&&!Ё))
Э=1;else{Vector3 ΙΤ=Я(Τ,ƛ-ή);Vector3 Γε=Я(Τ,Τ.GetNaturalGravity());float ΘЎ=Ʋ(ΙΤ,Γε,null);Εφ=Math.Min(Θφ,ΘЎ);}Ǧ(true,false,false,χ,χ-ή,dockingSpeed,Εφ);if(Ё) к=(float) К;IMyShipConnector ΓΠ=П(MyShipConnectorStatus.Connectable);if(ΓΠ!=null){Ǧ(false,false,false,χ,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΓΠ.Connect();if(ΓΠ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.ȭ);else Ν(Α.Ǿ);ΒЇ();Ź(ȇ,true);return;}}}else{float Εσ=(float) Math.Round(К,1);if(Εσ<к){Ђ=-1;к=Εσ;}else Ђ++;if(Ђ>20){Ν(Α.ŷ);return;}}}if(Φ==Α.Ǿ||Φ==Α.ȭ||
Φ==Α.ΑΖ||Φ==Α.Ƹ||Φ==Α.ē){bool ΒЏ=false,ΒА=false;if(Η==Ζ.Ϋ){if(е()==Θ) ΒЏ=true;else if(е()==Ε) ΒА=true;}if(Ё){Ơ=false;if(П(MyShipConnectorStatus.Connected)==null){Ν(Α.ǒ);return;}ΒЇ();if(ΒЏ) ΑΒ(ό.Ǘ);if(ΒА) ΑΒ(ύ.Ǘ);į=false;Ȕ=false;ș=false;Ʃ=false;}if(П(MyShipConnectorStatus.Connected)==null){ψ();Ǽ(а.ΔЅ);return;}
if(ί!=Μ.Є||ш==-1||ǩ==р.None) Ȕ=true;else if(ǿ>=100f) Ȕ=true;else if(ǿ<=99f) Ȕ=false;if(ί!=Μ.Є||щ==-1||і.Count==0) ș=true;else if(Ț>=100f) ș=true;else if(Ț<=99) ș=false;if(ί!=Μ.Є||Ы==-1||Ċ.Count==0) Ʃ=true;else Ʃ=İ>=Ы;Ŀ п=null;if(ΒЏ) п=ό;
if(ΒА) п=ύ;if(п!=null&&(п.Ќ==κ.ǆ||п.Ќ==κ.ǅ)) Ȕ=true;if(п!=null&&(п.Ќ==κ.Βπ)) if(!į) Ȕ=false;if(п!=null&&(п.Ќ==κ.Βτ||п.Ќ==κ.Βς)) ș=true;if(п!=null&&(п.Ќ==κ.Βσ)) if(!į) ș=false;if(Ĉ){ChargeMode Εε=Ȕ?ChargeMode.Auto:ChargeMode.Recharge;if(п!=null&&(п.Ќ==κ.ǅ||п.Ќ==κ.ǆ))
Εε=ChargeMode.Discharge;Βώ(Εε);ΒЀ(!ș);}if(!į){if(Η==Ζ.Ϋ)
į=ί!=Μ.Є||ΗΑ(Ё,true)||Ƨ;else
į=ί!=Μ.Є||ΔІ();}else{if(!Ȕ) Ν(Α.ē);if(!ș) Ν(Α.Ƹ);if(!Ʃ) Ν(Α.ΑΖ);Ё=false;}if(į&&Ȕ&&ș&&Ʃ){Βώ(ChargeMode.Auto);ΒЀ(false);if(ί==Μ.Є){if(Η==Ζ.Ϋ){if(е()==Θ) ΑΒ(ό.ǚ);else if(е()==Ε) ΑΒ(ύ.ǚ);if(е()==Θ) Ľ(Ε,Μ.Ĵ);else Ľ(Θ,Μ.ĳ);}}Ν(Α.ǒ);return;}}if(Φ==Α.ǒ){if(Ƭ){if(!Δλ()) return;
Ƭ=false;Ν(Α.ǒ);return;}if(Ё){if(!Δλ()){Ƭ=true;return;}IMyShipConnector ȃ=П(MyShipConnectorStatus.Connected);if(ȃ==null){Ν(Α.Ю);return;}IMyShipConnector γ=ȃ.OtherConnector;ΕΕ(ȃ,false);Ź(ȇ,false);Ψ ά=null;Ψ Γν=Η==Ζ.Ϋ?Θ:е();if(Vector3.Distance(ȃ.GetPosition(),Γν.Δ)<5f&&Θ.ε) ά=Γν;
if(Vector3.Distance(ȃ.GetPosition(),Ε.Δ)<5f&&Ε.ε) ά=Ε;if(ά!=null){if(!Ǹ(ά,dockDist*δ,true,out χ)){Ǽ(а.ŭ);ψ();return;}Р(χ,5);Х(ά.α,ά.β,ά.υ,false);}else Р(ή+γ.WorldMatrix.Forward*dockDist*δ,5);if(ί==Μ.Є) Ǽ(а.ǉ);}if(К<wpReachedDist){ѓ(у,true);Ν(Α.Ю);return;}}if(Φ==Α.ĸ)
{if(Ё){ǧ(true);ѓ(ο,false);χ=Ε.Δ;Р(χ,20);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist/2){Ν(Α.ȧ);return;}}if(Φ==Α.ȫ){if(Ё){ǧ(true);ѓ(ο,false);int Б=-1;double ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){double ω=Vector3.Distance(Σ[i].Δ,ή);if(ǫ==-1||ω<ǫ){Б=i;ǫ=ω;}}
if(Б==-1){Ν(Α.Ѕ);return;}χ=Σ[Б].Δ;Р(χ,10);Х(Ε.α,Ε.β,Ε.υ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Φ==Α.ƹ){bool х=false;if(Ё){ǧ(true);if((М==Ϗ.ΒΧ||М==Ϗ.ƶ)&&ƿ()&&
Ķ(ъ,Τ.GetNaturalGravity())<25&&Н>=2&&б>=2){Vector3 Ƌ=ή;if(Ш>0&&Щ<б-1) Ƌ=ȏ(Ш-1,Щ+1,true);else if(Ш<Н-1&&Щ<б-1) Ƌ=ȏ(Ш+1,Щ+1,true);else if(Ш<Н-1&&Щ>0) Ƌ=ȏ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ƌ=ȏ(Ш-1,Щ-1,true);else х=true;if(!х) Р(Ƌ,10);
}else х=true;}if(К<wpReachedDist/2) х=true;if(х){Ν(Α.ΑΕ);return;}}if(Φ==Α.ΒЊ||Φ==Α.ΑΕ){if(Ё){Ǧ(true,true,false,ή,0);ѓ(ο,false);ǧ(true);Ђ=-1;Э=М==Ϗ.Βθ||М==Ϗ.ƽ?0:-1;}bool ġ=!ƴ();int ΑЊ=Ȏ("STONE","ORE",З.đ);if(М==Ϗ.ΒΠ||М==Ϗ.ƽ||М==Ϗ.ƶ)
ΑЊ+=Ȏ("ICE","ORE",З.đ);bool Θε=ΑЊ>0;bool Γή=false;if(Э>=0){float ŀ=(float) Math.Sin(Δπ(Э))*ǡ/3f;float ΘΙ=(float) Math.Cos(Δπ(Э))*ǳ/3f;Vector3 ΘΟ=ȏ(Ш,Щ,true)+ΒЈ(ъ,ȟ*-1,new Vector3(ŀ,ΘΙ,0));Р(ΘΟ,0.3f);if(К<Math.Min(ǡ,ǳ)/10f) Э+=5f;if(Э>=360) Э=0;}if(Ђ==-1||ΑЊ<Ђ){Ђ=ΑЊ;к=0;}
else{к++;if(к>50) Γή=true;}if(!Θε||ġ||Γή){if(!ġ){int ΙΩ=Ȏ("","ORE",З.đ);if(ų(true)) ġ=true;else if(100-(А(ΙΩ,ΖΩ)*100)<minEjection) ġ=true;else Ǽ(а.ǉ);}if(Γή&&ġ) Κ="Ejection failed";if(Φ==Α.ΑΕ){if(ġ){if(Θ.ε) Ν(Α.ȫ);else{ψ();ĕ();Κ="Can´t return, no dock found";}}
else Ν(Α.ǥ);}else if(ġ) Ν(Α.Ǻ);else Ν(Α.Ļ);return;}}Ё=false;}void ΖЌ(List<Ψ>ď,int ζ,List<Vector3>Ƥ,float ǟ,bool ȉ,ref int ǔ){if(ȉ){for(int i=0;i<ď.Count;i++) ď[i].Џ=0;ǔ=-1;return;}if(ζ==0) return;int ѝ=ζ*-1;if(ǔ==-1) ǔ=ѝ>0?1:ď.Count-2;int ΖΒ=0;while(ǔ>=1&&ǔ<ď.Count-1)
{if(ΖΒ>50) return;ΖΒ++;try{if((ѝ<0&&ǔ>=1)||(ѝ>0&&ǔ<=ď.Count-2)){Ψ ι=ď[ǔ];bool ΖΦ=false;for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(ι.Δ,Ƥ[j])<=ǟ){ΖΦ=true;break;}if(!ΖΦ){Ψ Αό=ď[ǔ-ѝ];Ψ Ř=ď[ǔ+ѝ];Vector3 ΙΥ=ι.Δ-Ř.Δ;Vector3 ΔΣ=Αό.Δ-ι.Δ;Vector3 ΘЉ=ι.Δ+Vector3.Normalize(ΙΥ)*ΔΣ.Length();Vector3 ΘΪ=Αό.Δ-ΘЉ;Vector3 Αν=ė(ζ>0?ι.β:ι.β*-1,ι.α*-1,ΘΪ);Vector3 Θυ=ė(ζ>0?ι.β:ι.β*-1,ι.α*-1,ΔΣ);
Vector3 Αο=ė(ζ>0?ι.β:ι.β*-1,ι.α*-1,ι.ν);ι.Џ=(float) Math.Sqrt(Math.Pow(Αό.Џ,2)+Math.Pow(Ʋ(-Θυ,Αο,ι),2));for(int j=0;j<Ƥ.Count;j++)
if(Vector3.Distance(Αό.Δ,Ƥ[j])<=ǟ){Vector3 Απ=ė(ζ>0?ι.β:ι.β*-1,ι.α*-1,Ƥ[j]-ι.Δ);float ΙΜ=Ʋ(-Απ,Αο,ι);ι.Џ=Math.Min(ι.Џ,ΙΜ)/2f;}if(Αν.Length()==0) Αν=new Vector3(0,0,1);Vector3 Θτ=ė(ι.β,ι.α*-1,ι.ν);float ΙΞ=ů(-Αν,Θτ,ι);float ΑΗ=А(ΙΞ,ĭ);float ΔΠ=(float) Math.Sqrt(А(Αν.Length()*1.0f,0.5f*ΑΗ));
ι.Џ=Math.Min(ι.Џ,(ΔΣ.Length()/ΔΠ)*Ǩ);}}}catch{return;}ǔ+=ѝ;}ǔ=-1;}void ΒЉ(bool ȉ){if(ȉ){ƒ=0;return;}if(Η==Ζ.Ϋ) return;float х=Й*Math.Max(1,Ц);if(ȝ==Й) х+=Math.Min(Ц,С);float ΙΨ=Н*б*Math.Max(1,Ц);ƒ=Math.Max(ƒ,(float) Math.Min(х/ΙΨ*100.0,100));}MyDetectedEntityType Δϋ()
{try{if(ǹ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βγ(bool Ǯ){if(Η==Ζ.Ǐ&&Δϋ()==MyDetectedEntityType.None&&!Ų(ξ,true))
return fastSpeed;else return Ǯ?ū:Ū;}void ΗЈ(){float ΖΟ=0,Εν=0,ĥ=0,ΓІ=0;for(int i=0;i<Ȯ.Count;i++){IMyBatteryBlock Ɓ=Ȯ[i];if(!ǹ(Ɓ,true)) continue;ΖΟ+=Ɓ.MaxStoredPower;Εν+=Ɓ.CurrentStoredPower;ĥ+=Ɓ.CurrentInput;ΓІ+=Ɓ.CurrentOutput;}ǿ=(float) Math.Round(А(Εν,ΖΟ)*100,1);
if(ĥ>=ΓІ) ǩ=р.ē;else ǩ=р.Δρ;if(ĥ==0&&ΓІ==0||ǿ==100.0) ǩ=р.ж;if(Ȯ.Count==0) ǩ=р.None;}void ΗЌ(){float Εό=0;for(int i=0;i<і.Count;i++){IMyGasTank Ζχ=і[i];if(!ǹ(Ζχ,true)) continue;Εό+=(float) Ζχ.FilledRatio;}Ț=А(Εό,і.Count)*100f;}void ΘΓ(){İ=0;try{for(int i=0;i<Ċ.Count;i++)
{IMyReactor ΓЏ=Ċ[i];if(!ǹ(ΓЏ,true)) continue;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ΓЏ.GetInventory(0).GetItems(Ъ);float Ǔ=0;for(int j=0;j<Ъ.Count;j++){MyInventoryItem Г=Ъ[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǔ+=(float) Г.Amount;}if(Ǔ<İ||i==0){İ=Ǔ;Γϋ=ΓЏ.CustomName;}}}catch(Exception e){Ȝ=e;}}void ΗЉ(){Ŗ=0;ň=0;try{for(int i=0;i<Ň.Count;i++){IMyTerminalBlock Υ=Ň[i];if(!ǹ(Υ,true)) continue;ň+=(float) Υ.GetInventory(0).CurrentVolume;Ŗ+=(float) Υ.GetInventory(0).MaxVolume;
}Αμ=(float) Math.Min(Math.Round(А(ň,Ŗ)*100,1),100.0);}catch(Exception e){Ȝ=e;}}void ΗЍ(){try{Ǚ.Clear();for(int i=0;i<Ň.Count;i++){IMyTerminalBlock Υ=Ň[i];if(!ǹ(Υ,true)) continue;Δγ(Υ,З.ű);}if(М!=Ϗ.Βο){for(int i=0;i<ě.Count;i++){IMyTerminalBlock Υ=ě[i];if(!ǹ(Υ,true)) continue;
Δγ(Υ,З.Ηά);}}ΗЀ(Ǚ);}catch(Exception e){Ȝ=e;}}void ΖА(){if(ΑΥ){if(Δϊ().Count>Αε){ΑΥ=false;if(м!=д.Ű){ψ();if(м==д.Βά) ĕ();if(м==д.ǈ)
if(Θ.ε) ƺ();else ĕ();}Κ="Damage detected";}}}bool ƴ(){if(!Αζ) return true;if(ί==Μ.Є){if(ш>0&&ǩ!=р.None)
if(ǿ<=ш){Κ="Low energy! Move home";return false;}if(Ы>0&&Ċ.Count>0)
if(İ<=Ы){Κ="Low fuel: "+Γϋ;return false;}if(щ>0&&і.Count>0)
if(Ț<=щ){Κ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δϊ(){List<IMyTerminalBlock>Ġ=new List<IMyTerminalBlock>();for(int i=0;i<Ž.Count;i++){IMyTerminalBlock Υ=Ž[i];if(Ų(Υ,false)) Ġ.Add(Υ);}return Ġ;}bool Ų(IMyTerminalBlock Υ,bool ΔΚ){return(!ǹ(Υ,ΔΚ)||!Υ.IsFunctional);
}bool ǹ(IMyTerminalBlock Υ,bool ΔΚ){if(Υ==null) return false;try{IMyCubeBlock ΕЃ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΔΚ) return ΕЃ==Υ;else return ΕЃ.GetType()==Υ.GetType();}catch{return false;}}float ΗΙ(IMyTerminalBlock Υ,String я,String Ζξ,String[]ǭ){float Ȅ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory ŏ=Υ.GetInventory(i);List<MyInventoryItem>Ъ=new List<MyInventoryItem>();ŏ.GetItems(Ъ);foreach(MyInventoryItem Г in Ъ){if(ǭ!=null&&(ǭ.Contains(Г.Type.TypeId.ToUpper())||ǭ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((я==""||Г.Type.TypeId.ToUpper()==я)&&(Ζξ==""||Г.Type.SubtypeId.ToUpper()==Ζξ))
Ȅ+=(float) Г.Amount;}}return Ȅ;}ё ΗΠ(String φ,String ς,З Д,bool ΘΦ){φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ё Г=Ǚ[i];if(Г.φ.ToUpper()==φ&&Г.ς.ToUpper()==ς&&(Г.œ==Д||Д==З.đ))
return Г;}ё έ=null;if(ΘΦ){έ=new ё(φ,ς,0,Д);Ǚ.Add(έ);}return έ;}int Ȏ(String φ,String ς,З Д){return Ȏ(φ,ς,Д,null);}int Ȏ(String φ,String ς,З Д,String[]ǭ){int Ȅ=0;φ=φ.ToUpper();ς=ς.ToUpper();for(int i=0;i<Ǚ.Count;i++){ё Г=Ǚ[i];if(ǭ!=null&&ǭ.Contains(Г.φ.ToUpper())) continue;
if((φ==""||Г.φ.ToUpper()==φ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.œ==Д||Д==З.đ))
Ȅ+=Г.Ǔ;}return Ȅ;}void Δγ(IMyTerminalBlock Υ,З Д){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>Ъ=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(Ъ);for(int j=0;j<Ъ.Count;j++)
ΗΠ(Ъ[j].Type.SubtypeId,Ъ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).Ǔ+=(int) Ъ[j].Amount;}}void ΗЀ(List<ё>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){ё a=Т[j];ё b=Т[j+1];if(a.Ǔ<b.Ǔ) Т.Move(j,j+1);}}bool ų(bool ΚΡ){if(Ȧ&&Η!=Ζ.Ϋ)
if(О!=-1&&ĭ>=О){Κ="Ship too heavy";return true;}if(Αμ>=ǯ||Ơ){Ơ=false;Κ="Ship is full";return true;}return false;}bool ΔІ(){String[]ǭ=null;if(!ȥ) ǭ=new string[]{"ICE"};if(Η==Ζ.Ŷ) return Ȏ("","ORE",З.ű,ǭ)==0;if(Η==Ζ.Ǐ) return Ȏ("","COMPONENT",З.ű,ǭ)==0;else return Ȏ("","",З.ű,ǭ)==0;
}bool Ηα(){List<IMyCockpit>ΓΟ=new List<IMyCockpit>();ϋ.GetBlocksOfType(ΓΟ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΟ.Count;i++)
if(ΓΟ[i].IsUnderControl) return true;return false;}bool ΗΑ(bool ȉ,bool ΑЄ){IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δμ(ό,ȉ,ΑЄ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δμ(ύ,ȉ,ΑЄ);return false;}bool Δμ(Ŀ Ȃ,bool ȉ,bool ΑЄ){if(ȉ) Ȃ.Ηο();Ȃ.ΗБ();bool о=false;String э="";switch(Ȃ.Ќ){case κ.Βρ:{э="Waiting for command";о=false;break;}case κ.ΔБ:{э="Waiting for cargo";о=ų(true);break;}case κ.ΔА:{э="Unloading";о=ΔІ();break;}case κ.Βφ:{о=true;break;}
case κ.Βπ:{э="Charging batteries";о=ǿ>=100f;break;}case κ.ǆ:{э="Discharging batteries";о=ǿ<=25f;break;}case κ.ǅ:{э="Discharging batteries";о=ǿ<=0f;break;}case κ.Βσ:{э="Filling up hydrogen";о=Ț>=100f;break;}case κ.Βτ:{э="Unloading hydrogen";о=Ț<=25f;break;}
case κ.Βς:{э="Unloading hydrogen";о=Ț<=0f;break;}case κ.Βυ:{bool ΕЎ=Ηα();if(!ΕЎ) Ȃ.ΓЋ=true;о=Ȃ.ΓЋ&&ΕЎ;э="Waiting for passengers";break;}}if(!о) Ȃ.ΒΞ(true);if(о&&Ȃ.Βε()){о=Ȃ.ΒΞ(false);э="Undocking in: "+Βή((int) Math.Max(0,Ȃ.ǖ-Ȃ.Ā));}if(ΑЄ) ΖЄ=э;return о;}void ΖЈ()
{if(!Ő) return;if(ο.Count<=1) return;float Ζύ=0,Ζό=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǲ=ο[i];if(Ų(ǲ,true)) continue;Ζύ+=(float) ǲ.GetInventory(0).MaxVolume;Ζό+=(float) ǲ.GetInventory(0).CurrentVolume;}float ΕΫ=(float) Math.Round(А(Ζό,Ζύ),5);for(int Ζά=0;Ζά<Math.Max(1,Math.Floor(ο.Count/10f));Ζά++)
{float Γλ=0,Γϊ=0;float ΕЋ=0,ΖΝ=0;IMyTerminalBlock ȗ=null,ā=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǲ=ο[i];if(Ų(ǲ,true)) continue;float ΓЁ=(float) ǲ.GetInventory(0).MaxVolume;float Αί=А((float) ǲ.GetInventory(0).CurrentVolume,ΓЁ);if(ȗ==null||Αί>Γλ){ȗ=ǲ;Γλ=Αί;ΕЋ=ΓЁ;}
if(ā==null||Αί<Γϊ){ā=ǲ;Γϊ=Αί;ΖΝ=ΓЁ;}}if(ȗ==null||ā==null||ȗ==ā) return;if(checkConveyorSystem&&!ȗ.GetInventory(0).IsConnectedTo(ā.GetInventory(0))){if(ΓΓ>20) Κ="Inventory balancing failed";else ΓΓ++;return;}ΓΓ=0;List<MyInventoryItem>Ъ=new List<MyInventoryItem>();
ȗ.GetInventory(0).GetItems(Ъ);float ƫ=0;if(Ъ.Count==0) continue;MyInventoryItem Αΰ=Ъ[0];String ΖΑ=Αΰ.Type.TypeId+Αΰ.Type.SubtypeId;if(!ΖЃ.TryGetValue(ΖΑ,out ƫ)){if(Ηζ(ȗ.GetInventory(0),0,ā.GetInventory(0),out ƫ))
ΖЃ.Add(ΖΑ,ƫ);else return;}float ΙΦ=((Γλ-ΕΫ)*ΕЋ/ƫ);float ΙΧ=((ΕΫ-Γϊ)*ΖΝ/ƫ);int ΔΦ=(int) Math.Min(ΙΧ,ΙΦ);if(ΔΦ<=0) return;if((float) Αΰ.Amount<ΔΦ)
ȗ.GetInventory(0).TransferItemTo(ā.GetInventory(0),0,null,null,Αΰ.Amount);else
ȗ.GetInventory(0).TransferItemTo(ā.GetInventory(0),0,null,null,ΔΦ);}}bool Ηζ(IMyInventory ơ,int Β,IMyInventory ΘΩ,out float ΖЂ){ΖЂ=0;float ΘΜ=(float) ơ.CurrentVolume;List<MyInventoryItem>ΓΕ=new List<MyInventoryItem>();ơ.GetItems(ΓΕ);float Εή=0;for(int i=0;i<ΓΕ.Count;i++) Εή+=(float) ΓΕ[i].Amount;
ơ.TransferItemTo(ΘΩ,Β,null,null,1);float ΖЁ=ΘΜ-(float) ơ.CurrentVolume;List<MyInventoryItem>ΒЌ=new List<MyInventoryItem>();ơ.GetItems(ΒЌ);float ΕΣ=0;for(int i=0;i<ΒЌ.Count;i++) ΕΣ+=(float) ΒЌ[i].Amount;if(ΖЁ==0f||!Βι(0.9999,Εή-ΕΣ,1.0001))
return false;ΖЂ=ΖЁ;return true;}void ΗЋ(){Vector3 Ə=χ+č;Vector3 Ǳ=Ə-ή;if(Ǳ.Length()==0) Ǳ=new Vector3(0,0,-1);Vector3 Απ=Я(Τ,Ǳ);Vector3 Ζψ=Vector3.Normalize(Απ);Vector3 Œ=Я(Τ,Τ.GetNaturalGravity());float ΙΗ=ΓЉ>0?Math.Max(0,1-Ķ(Ǳ,Εώ)/5):0;float Џ=(float) Math.Min((Γώ>0?Γώ:1000f),Math.Max(Ʋ(-Απ,Œ,null),ΓЉ*ΙΗ));
if(!Ζϋ) Џ=0;if(Ζθ) Џ=Math.Max(0,1-ΑΣ/ΑΘ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(Ŝ) Џ=0;if(ю>=0) Џ=Math.Min(Џ,ю);if(Ζι) Џ*=(float) Math.Min(1,А(Ǳ.Length(),wpReachedDist)/2);Vector3 ΖΛ=Я(Τ,Τ.GetShipVelocities().LinearVelocity);float ΘΘ=(float)(Math.Max(0,15-Ķ(-Ζψ,-ΖΛ))/15)*0.85f+0.15f;
ΔΞ+=Math.Sign(ΘΘ-ΔΞ)/10f;Vector3 Ȱ=Ζψ*Џ*ΔΞ-(ΖΛ);Vector3 ΓΒ=Ββ(Ȱ,null);if(ΑЌ&&К>0.1f){Ȱ.X*=ΒЂ(Ȱ.X,ref ΑЍ.X,1f,ΓΒ.X,20);Ȱ.Y*=ΒЂ(Ȱ.Y,ref ΑЍ.Y,1f,ΓΒ.Y,20);Ȱ.Z*=ΒЂ(Ȱ.Z,ref ΑЍ.Z,1f,ΓΒ.Z,20);}else ΑЍ=new Vector3(1,1,1);ƥ=ĭ*Ȱ-Œ*ĭ;Δε(ƥ,ΑЌ);К=Vector3.Distance(ή,Ə);
}float ΒЂ(float value,ref float Ф,float ѝ,float Ȟ,float ΓϏ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ѝ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<ΓϏ||Ȟ==0) return έ;Ф=Math.Min(ΓϏ,Math.Max(-ΓϏ,Ф));έ=Math.Abs(Ȟ);return έ;
}void ΕΠ(){float ь=90,ǎ=90,ќ=90;float ǝ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǯ,ј,Ɩ,Γχ;if(Γψ){Ǯ=Vector3.Normalize(χ-ή);Ɩ=Я(Τ,Ǯ);ј=Я(Τ,ΔΜ);ь=Ķ(Ɩ,new Vector3(0,-1,0))-90;ǎ=Ē(ј,new Vector3(-1,0,0),ј.Y);
ќ=Ē(Ɩ,new Vector3(-1,0,0),Ɩ.Z);}else{Ǯ=ΔΝ;ј=Я(Τ,ΔΜ);Ɩ=Я(Τ,ΔΝ);Γχ=Я(Τ,Ζω);ь=Ē(ј,new Vector3(0,0,1),ј.Y);ǎ=Ē(ј,new Vector3(-1,0,0),ј.Y);ќ=Ē(Γχ,new Vector3(0,0,1),Γχ.X);}if(ΕΥ&&ƿ()){Vector3 Γε=Τ.GetNaturalGravity();ј=Я(Τ,Γε);ь=Ē(ј,new Vector3(0,0,1),ј.Y);
ǎ=Ē(ј,new Vector3(-1,0,0),ј.Y);}if(!Βι(-45,ǎ,45)){ь=0;ќ=0;}if(!Βι(-45,ќ,45)) ь=0;Ǌ(Γη,1,(-ь)*ǝ,(-ќ)*ǝ,(-ǎ)*ǝ);ΑΣ=Math.Max(Math.Abs(ь),Math.Max(Math.Abs(ǎ),Math.Abs(ќ)));Αη=ΑΣ<=ΑΘ;}void ΖЍ(IMyTerminalBlock Αύ){if(Αύ==null) return;Ǣ=new float[3,2];ў=new Dictionary<string,float[,]>();
for(int i=0;i<ȋ.Count;i++){IMyThrust в=ȋ[i];if(!в.IsFunctional) continue;Vector3 ζ=Я(Αύ,в.WorldMatrix.Backward);float ȳ=в.MaxEffectiveThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) Ǣ[0,0]+=ȳ;else Ǣ[0,1]-=ȳ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) Ǣ[1,0]+=ȳ;else Ǣ[1,1]-=ȳ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) Ǣ[2,0]+=ȳ;else Ǣ[2,1]-=ȳ;String я=Δώ(в);float[,]џ=null;if(ў.ContainsKey(я)) џ=ў[я];else{џ=new float[3,2];ў.Add(я,џ);}float Ȟ=в.MaxThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) џ[0,0]+=Ȟ;else џ[0,1]-=Ȟ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) џ[1,0]+=Ȟ;else џ[1,1]-=Ȟ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) џ[2,0]+=Ȟ;else џ[2,1]-=Ȟ;}}static String Δώ(IMyThrust в){return в.BlockDefinition.SubtypeId;}Vector3 Βλ(Vector3 ѕ,float[,]Ŕ){return new Vector3(
ѕ.X>=0?Ŕ[0,0]:Ŕ[0,1],
ѕ.Y>=0?Ŕ[1,0]:Ŕ[1,1],
ѕ.Z>=0?Ŕ[2,0]:Ŕ[2,1]);}bool Δύ(Ψ Ω,String я,out float Γέ){Γέ=0;int Β=Ȋ.IndexOf(я);if(Β==-1||Ω.Ǡ==null||Β>=Ω.Ǡ.Length)
return false;Γέ=Ω.Ǡ[Β];if(Γέ==-1) return false;return true;}Vector3 Ββ(Vector3 ѕ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<ў.Keys.Count;i++){String я=ў.Keys.ElementAt(i);float ƌ=0;if(!Δύ(Ω,я,out ƌ))
return Βλ(ѕ,Ǣ);έ+=Βλ(ѕ,ў.Values.ElementAt(i))*ƌ;}return έ;}return Βλ(ѕ,Ǣ);}float ů(Vector3 ѕ,Ψ Ω){return ů(ѕ,new Vector3(),Ω);}float ů(Vector3 ѕ,Vector3 ν,Ψ Ω){Vector3 ΙΝ=Ββ(ѕ,Ω);Vector3 ȳ=ΙΝ+ν*ĭ;float Ɯ=(ȳ/ѕ).AbsMin();return(float)(ѕ*Ɯ).Length();}float Ʋ(Vector3 Αξ,Vector3 Œ,Ψ Ω)
{if(Αξ.Length()==0) return 0;float ΕΤ=1;if(Œ.Length()>0) ΕΤ=Math.Min(1,Ķ(-Œ,Αξ)/90)*0.4f+0.6f;float ƥ=ů(Αξ,Œ,Ω);if(ƥ==0) return 0.1f;float ΑΗ=А(ƥ,ĭ);float ΔΠ=(float) Math.Sqrt(А(Αξ.Length(),ΑΗ*0.5f));return ΑΗ*ΔΠ*ΕΤ*Ǩ;}float Ƽ(Vector3 Ǯ,Vector3 ϊ,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Αο=ė(Ǯ,ϊ,Vector3.Normalize(ν));float ǘ=ů(-Αο,Ω);return ǘ/ν.Length();}void Δθ(bool Θλ){float Ǎ=0;float Γΰ=0.9f;if(Θλ){О=-1;ı=0;ΑЈ=null;if(ί!=Μ.з&&Ε.ν.Length()!=0){Ǎ=Γΰ*Ƽ(Ε.β,Ε.α*-1,Ε.ν,null);if(Ǎ<О||О==-1) О=Ǎ;}if(Θ.ε&&Θ.ν.Length()!=0)
{Ǎ=Γΰ*Ƽ(Θ.β,Θ.α*-1,Θ.ν,null);if(Ǎ<О||О==-1) О=Ǎ;}return;}if(ı==-1) return;if(ı>=0){int Ζί=0;while(ı<Σ.Count){if(Ζί>100) return;Ζί++;Ψ Ω=Σ[ı];if(Ω.ν.Length()!=0f){Ǎ=Γΰ*Math.Min(
Ƽ(Ω.β,Ω.α*-1,Ω.ν,Ω),
Ƽ(Ω.β*-1,Ω.α*-1,Ω.ν,Ω));if(Ǎ<О||О==-1) О=Ǎ;}else ΑЈ=Ω;ı++;}ı=-1;}bool Γθ=true;float ĉ=0;if(ΑЈ!=null){for(int i=0;i<ў.Count;i++){String я=ў.Keys.ElementAt(i);float[,]џ=ў.Values.ElementAt(i);float ƌ=0;if(!Δύ(ΑЈ,я,out ƌ)){Γθ=false;break;}for(int a=0;a<џ.GetLength(0);a++)
for(int b=0;b<џ.GetLength(1);b++){float Ě=Math.Abs(џ[a,b]*ƌ);if(Ě==0) continue;Γθ=true;if(ĉ==0||Ě<ĉ) ĉ=Ě;}}}if(!Γθ){for(int a=0;a<Ǣ.GetLength(0);a++)
for(int b=0;b<Ǣ.GetLength(1);b++){float Ě=Math.Abs(Ǣ[a,b]);if(Ě==0) continue;if(ĉ==0||Ě<ĉ) ĉ=Ě;}}if(ĉ>0){float Θψ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;Ǎ=А(ĉ,Θψ);if(Ǎ>0)
if(Ǎ<О||О==-1) О=Ǎ;}}void Δε(Vector3 ǘ,bool Ј){if(!Ј){for(int i=0;i<ȋ.Count;i++)
ȋ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΓΑ=Ββ(ǘ,null);float Ιζ=Math.Min(1,Math.Abs(А(ǘ.X,ΓΑ.X)));float Ιη=Math.Min(1,Math.Abs(А(ǘ.Y,ΓΑ.Y)));float Ιθ=Math.Min(1,Math.Abs(А(ǘ.Z,ΓΑ.Z)));for(int i=0;i<ȋ.Count;i++){IMyThrust в=ȋ[i];Vector3 ζ=Ηρ(Я(Τ,в.WorldMatrix.Backward),1);
if(ζ.X!=0&&Math.Sign(ζ.X)==Math.Sign(ǘ.X))
в.SetValueFloat("Override",в.MaxThrust*Ιζ);else if(ζ.Y!=0&&Math.Sign(ζ.Y)==Math.Sign(ǘ.Y))
в.SetValueFloat("Override",в.MaxThrust*Ιη);else if(ζ.Z!=0&&Math.Sign(ζ.Z)==Math.Sign(ǘ.Z))
в.SetValueFloat("Override",в.MaxThrust*Ιθ);else
в.SetValueFloat("Override",0.0f);}}void Ǌ(bool Ј,float ΘІ,float ь,float ќ,float ǎ){for(int i=0;i<Αβ.Count;i++){IMyGyro І=Αβ[i];І.GyroOverride=Ј;if(!Ј) І.GyroPower=100;else І.GyroPower=ΘІ;if(!Ј) continue;Vector3 В=Τ.WorldMatrix.Forward;Vector3 Ь=Τ.WorldMatrix.Right;Vector3 ϊ=Τ.WorldMatrix.Up;
Vector3 Ŋ=І.WorldMatrix.Forward;Vector3 Ō=І.WorldMatrix.Up;Vector3 ŋ=І.WorldMatrix.Left*-1;if(Ŋ==В) І.SetValueFloat("Roll",ǎ);else if(Ŋ==(В*-1)) І.SetValueFloat("Roll",ǎ*-1);else if(Ō==(В*-1)) І.SetValueFloat("Yaw",ǎ);else if(Ō==В) І.SetValueFloat("Yaw",ǎ*-1);else if(ŋ==В) І.SetValueFloat("Pitch",ǎ);
else if(ŋ==(В*-1)) І.SetValueFloat("Pitch",ǎ*-1);if(ŋ==(Ь*-1)) І.SetValueFloat("Pitch",ь);else if(ŋ==Ь) І.SetValueFloat("Pitch",ь*-1);else if(Ō==Ь) І.SetValueFloat("Yaw",ь);else if(Ō==(Ь*-1)) І.SetValueFloat("Yaw",ь*-1);else if(Ŋ==(Ь*-1)) І.SetValueFloat("Roll",ь);
else if(Ŋ==Ь) І.SetValueFloat("Roll",ь*-1);if(Ō==(ϊ*-1)) І.SetValueFloat("Yaw",ќ);else if(Ō==ϊ) І.SetValueFloat("Yaw",ќ*-1);else if(ŋ==ϊ) І.SetValueFloat("Pitch",ќ);else if(ŋ==(ϊ*-1)) І.SetValueFloat("Pitch",ќ*-1);else if(Ŋ==ϊ) І.SetValueFloat("Roll",ќ);else if(Ŋ==(ϊ*-1)) І.SetValueFloat("Roll",ќ*-1);
}}void ΕΚ(){this.Γη=false;}void Х(Vector3 Ȳ,Vector3 В,Vector3 Γφ,float Ȥ,bool ƪ){ΒΗ(Ȳ,Ȥ,ƪ);ΑΘ=Ȥ;Γψ=false;this.ΔΝ=В;this.Ζω=Γφ;}void Х(Vector3 Ȳ,Vector3 В,Vector3 Γφ,bool ƪ){Х(Ȳ,В,Γφ,2f,ƪ);}void ΒΗ(Vector3 Ȳ,float Ȥ,bool ƪ){ΑΘ=Ȥ;this.Γη=true;this.ΕΥ=ƪ;Γψ=true;Αη=false;
this.ΔΜ=Ȳ;}void ΒЇ(){Ǧ(false,false,false,χ,0);ΑЌ=false;}void Р(Vector3 Њ,float ǝ){Ǧ(true,false,false,Њ,ǝ);}void Ǧ(bool Ј,bool ΔΗ,bool ΔΩ,Vector3 Њ,float ǝ){Ǧ(Ј,ΔΗ,ΔΩ,Њ,Њ-ή,0.0f,ǝ);}void Ǧ(bool Ј,bool ΔΗ,bool ΔΩ,Vector3 Њ,Vector3 ΙΑ,float ΙΒ,float ǝ){ΑЌ=true;this.Ζϋ=Ј;
χ=Њ;this.Γώ=ǝ;this.ΓЉ=ΙΒ;this.Ζι=ΔΗ;this.Ζθ=ΔΩ;this.Εώ=ΙΑ;К=Vector3.Distance(Њ,ή);}void Ησ(){ϋ.GetBlocksOfType(Ž,ǐ);}void Ητ(){if(ΑϏ){ώ=2;return;}List<IMyRemoteControl>Ɲ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζζ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζπ=new List<IMyTerminalBlock>();
ϋ.GetBlocksOfType(Ɲ,ǐ);ϋ.GetBlocksOfType(ї,ǐ);ϋ.GetBlocksOfType(Ζζ,ǐ);ϋ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζπ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);ї=Ǥ(ї,pamTag,true);Ηϋ();Ηό(Ζπ);Ÿ(ї,setLCDFontAndSize,1.4f,false);Ÿ(Č,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζτ=Ǥ(Ζζ,pamTag,true);if(Ζτ.Count>0) ξ=Ζτ[0];else ξ=null;if(Η==Ζ.Ŷ){ϋ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ώ=1;Κ="Drills are missing";}}else if(Η==Ζ.Ǐ){ϋ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ώ=1;Κ="Grinders are missing";}if(ξ==null){ώ=1;Κ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ϋ.GetBlocksOfType(ΔΡ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζσ=Ǥ(Ɲ,pamTag,true);if(Ζσ.Count>0) Ɲ=Ζσ;if(Ɲ.Count>0) Τ=Ɲ[0];else{Τ=null;ώ=2;Κ="Remote is missing";return;}δ=(float) Τ.CubeGrid.WorldVolume.Radius*2;ћ=null;if(Η!=Ζ.Ϋ){Δι();if(ο.Count>0&&ћ!=null){if(ξ!=null&&(ћ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Τ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Τ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ώ=1;Κ="Wrong sensor direction";}if(ћ.WorldMatrix.Forward!=Τ.WorldMatrix.Forward&&
ћ.WorldMatrix.Forward!=Τ.WorldMatrix.Down){ώ=2;Κ="Wrong remote direction";}}}}void Ηυ(){ϋ.GetBlocksOfType(ȇ,ǐ);ϋ.GetBlocksOfType(у,ǐ);ϋ.GetBlocksOfType(ȋ,ǐ);ϋ.GetBlocksOfType(Αβ,ǐ);ϋ.GetBlocksOfType(Ȯ,ǐ);ϋ.GetBlocksOfType(Ċ,ǐ);ϋ.GetBlocksOfType(і,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϋ.GetBlocksOfType(ΑЇ,ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
у=Ǥ(у,"ConnectorMedium",false);else
у=Ǥ(у,"Connector",false);List<IMyShipConnector>Ζρ=Ǥ(у,pamTag,true);if(Ζρ.Count>0) у=Ζρ;if(ώ<=1){if(у.Count==0){ώ=1;Κ="Connector is missing";}if(Αβ.Count==0){ώ=1;Κ="Gyros are missing";}if(ȋ.Count==0){ώ=1;Κ="Thrusters are missing";}}List<IMyConveyorSorter>Ζυ=Ǥ(ΑЇ,pamTag,true);
if(Ζυ.Count>0) ΑЇ=Ζυ;List<IMyLandingGear>Ζς=Ǥ(ȇ,pamTag,true);if(Ζς.Count>0) ȇ=Ζς;for(int i=0;i<ȇ.Count;i++) ȇ[i].AutoLock=false;List<IMyBatteryBlock>Ζο=Ǥ(Ȯ,pamTag,true);if(Ζο.Count>0) Ȯ=Ζο;List<IMyGasTank>Ζφ=Ǥ(і,pamTag,true);if(Ζφ.Count>0) і=Ζφ;}void Ηφ(){ϋ.GetBlocksOfType(ě,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ň.Clear();for(int i=ě.Count-1;i>=0;i--){if(Ηή(ě[i])){Ň.Add(ě[i]);ě.RemoveAt(i);}}}bool Ηή(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βζ(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δι(){if(Τ==null) return;ћ=null;float Αχ=0,Ας=0,Αψ=0,Ασ=0,Αω=0,Ατ=0;List<IMyTerminalBlock>ΔΛ=Ǥ(ο,pamTag,true);bool ΘЀ=ΔΛ.Count==0;if(ΔΛ.Count>0) ћ=ΔΛ[0];else if(ο.Count>0) ћ=ο[0];int Ȅ=0;float ŝ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward){if(ΘЀ){ώ=2;Κ="Mining direction is unclear!";return;}continue;}Ȅ++;ŝ=Math.Max(ŝ,ΗІ(ο[i]));Vector3 Љ=ƻ(Τ,ο[i].GetPosition());if(i==0){Αχ=Љ.X;Ας=Љ.X;Αψ=Љ.Y;Ασ=Љ.Y;Αω=Љ.Z;Ατ=Љ.Z;}Ας=Math.Max(Љ.X,Ας);Αχ=Math.Min(Љ.X,Αχ);
Ασ=Math.Max(Љ.Y,Ασ);Αψ=Math.Min(Љ.Y,Αψ);Ατ=Math.Max(Љ.Z,Ατ);Αω=Math.Min(Љ.Z,Αω);}if(ŝ==0) ŝ=drillRadius;bool Ȳ=ћ!=null&&ћ.WorldMatrix.Forward==Τ.WorldMatrix.Down;ǡ=(Ας-Αχ)*(1-Ĳ)+ŝ*2;ǳ=(Ȳ?Ατ-Αω:Ασ-Αψ)*(1-ģ)+ŝ*2;IMyCubeGrid g=Τ.CubeGrid;float Δή=0,Δί=0,Δΰ=0,Δα=0;
for(int c=0;c<8;c++){Vector3 p=ƻ(Τ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȳ?p.Z:p.Y;if(c==0||p.X<Δή) Δή=p.X;if(c==0||p.X>Δί) Δί=p.X;if(c==0||y<Δΰ) Δΰ=y;if(c==0||y>Δα) Δα=y;}ΑЂ=Δί-Δή+g.GridSize;ΔΒ=Δα-Δΰ+g.GridSize;}float ΗІ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǥ<T>(List<T>Т,String Ǟ,bool Αγ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βζ(Т[i],Ǟ,Αγ)) έ.Add(Т[i]);return έ;}bool Βζ<T>(T Υ,String Ǟ,bool Αγ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(Αγ&&b.CustomName.ToUpper().Contains(Ǟ.ToUpper())) return true;if(!Αγ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǟ.ToUpper())) return true;return false;}void ѓ<T>(List<T>Т,bool Ј)
{for(int i=0;i<Т.Count;i++)
ΕΕ((IMyTerminalBlock) Т[i],Ј);}void ΕΕ(IMyTerminalBlock Υ,bool Ј){if(Υ==null) return;String ΘΖ=Ј?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(ΘΖ);a.Apply(Υ);}void ǧ(bool Ј){ΔΘ=Ј;if(!Į) return;ѓ(ΑЇ,Ј);}void Βώ(ChargeMode Ƙ){for(int i=0;i<Ȯ.Count;i++) Ȯ[i].ChargeMode=Ƙ;}void ΒЀ(bool ΙΙ)
{for(int i=0;i<і.Count;i++) і[i].Stockpile=ΙΙ;}void Ź(List<IMyLandingGear>Γγ,bool ΖΜ){for(int i=0;i<Γγ.Count;i++){if(ΖΜ) Γγ[i].Lock();if(!ΖΜ) Γγ[i].Unlock();}}void ΕΖ(bool Ј){if(Εξ==Ј) return;List<IMyShipController>ΑΞ=new List<IMyShipController>();ϋ.GetBlocksOfType(ΑΞ,ǐ);if(ΑΞ.Count==0) return;
for(int i=0;i<ΑΞ.Count;i++)
ΑΞ[i].DampenersOverride=Ј;Εξ=Ј;}IMyShipConnector П(MyShipConnectorStatus э){for(int i=0;i<у.Count;i++){if(!ǹ(у[i],true)) continue;if(у[i].Status==э) return у[i];}return null;}void Ę(bool ΔΔ,bool ΑΧ,float Θβ,float ΘΛ){if(ξ==null||ο.Count==0) return;Vector3 ΑΝ=new Vector3();
int Ȅ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ћ.WorldMatrix.Forward) continue;Ȅ++;ΑΝ+=ο[i].GetPosition();}ΑΝ=ΑΝ/Ȅ;Vector3 ř=ƻ(ξ,ΑΝ);ξ.Enabled=true;ξ.ShowOnHUD=ΔΔ;ξ.LeftExtend=(ΑΧ?1:й)/2f*ǡ-ř.X;ξ.RightExtend=(ΑΧ?1:й)/2f*ǡ+ř.X;ξ.TopExtend=(ΑΧ?1:и)/2f*ǳ+ř.Y;
ξ.BottomExtend=(ΑΧ?1:и)/2f*ǳ-ř.Y;ξ.FrontExtend=(ΔΔ?Ц:Θβ)-ř.Z;ξ.BackExtend=ΔΔ?0:ΘΛ+δ*0.75f+ř.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ÿ<T>(List<T>Т,bool ΙΔ,float Θΰ,bool Θμ){for(int i=0;i<Т.Count;i++){IMyTextSurface ƣ=null;if(Т[i]is IMyTextSurface) ƣ=(IMyTextSurface) Т[i];if(ƣ!=null){ƣ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΙΔ) continue;ƣ.Font="Debug";if(Θμ) continue;ƣ.FontSize=Θΰ;}}}bool ƿ(){Vector3D η;return this.Τ.TryGetPlanetPosition(out η);}int ΗΦ(Vector3 ΙΛ,int Ζκ){if(Ζκ==-1) return 0;double Б=-1;int Ȗ=-1;for(int i=Σ.Count-1;i>=0;i--){double ω=Vector3.Distance(Σ[i].Δ,ΙΛ);if(Б==-1||ω<Б){Ȗ=i;Б=ω;}
}return Math.Sign(Ȗ-Ζκ);}bool Ηΰ(Vector3 η){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.ε&&Σ.Count>=1){Vector3 ł=new Vector3();Ǹ(Θ,dockDist*δ,false,out ł);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,ł);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(ł);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.ε&&Σ.Count>=1){Vector3 ΒЎ=new Vector3();Ǹ(Ε,dockDist*δ,false,out ΒЎ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЎ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЎ);Ж.Add(Ε.Δ);}}}else{if(ί!=Μ.з)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȗ=-1;double ǫ=-1;for(int i=0;i<Ж.Count;i++){double ω=Vector3.Distance(Ж[i],η);if(ω<ǫ||ǫ==-1){ǫ=ω;Ȗ=i;}}if(Ж.Count==0) return false;double ΓΧ=Vector3.Distance(Ж[Ȗ],η);double ΘЇ=Vector3.Distance(Ж[Math.Max(0,Ȗ-1)],Ж[Ȗ])*1.5f;
double ΘϏ=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȗ+1)],Ж[Ȗ])*1.5f;return ΓΧ<ΘЇ||ΓΧ<ΘϏ||ΓΧ<laneOffset*δ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηρ(Vector3 v,int ΓΤ){return new Vector3(Math.Round(v.X,ΓΤ),Math.Round(v.Y,ΓΤ),Math.Round(v.Z,ΓΤ));
}float Ķ(Vector3 a,Vector3 b){if(a==b) return 0;float Θί=(a*b).Sum;float ΖΗ=a.Length();float ΖΘ=b.Length();if(ΖΗ==0||ΖΘ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(Θί/(ΖΗ*ΖΘ)));if(float.IsNaN(έ)) return 0;return έ;}float Ē(Vector3 v,Vector3 Αύ,float ΙΕ){float ŀ=Ķ(v,Αύ);
if(ΙΕ>0f) ŀ*=-1;if(ŀ>-90f) return ŀ-90f;else return 180f-(-ŀ-90f);}double Δπ(float ΘΧ){return(Math.PI/180)*ΘΧ;}bool Βι(double Θχ,double value,double Αρ){return(value>=Θχ&&value<=Αρ);}Vector3 ΗЇ(IMyTerminalBlock Υ,Vector3 Љ){return Vector3D.Transform(Љ,Υ.WorldMatrix);
}Vector3 ƻ(IMyTerminalBlock Υ,Vector3 Ιγ){return Я(Υ,Ιγ-Υ.GetPosition());}Vector3 Я(IMyTerminalBlock Υ,Vector3 Δέ){return Vector3D.TransformNormal(Δέ,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 ė(Vector3 Ǯ,Vector3 ϊ,Vector3 Δέ){MatrixD Γύ=MatrixD.CreateFromDir(Ǯ,ϊ);return Vector3D.TransformNormal(Δέ,MatrixD.Transpose(Γύ));
}Vector3 ΒЈ(Vector3 Ǯ,Vector3 ϊ,Vector3 Љ){MatrixD Γύ=MatrixD.CreateFromDir(Ǯ,ϊ);return Vector3D.Transform(Љ,Γύ);}String τ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Λ(Е,0)),float.Parse(Λ(Е,1)),float.Parse(Λ(Е,2)));
}String ΘΔ(Ψ Ω){String Ћ=":";String έ=τ(Ω.Δ)+Ћ+τ(Ω.β)+Ћ+
τ(Ω.α)+Ћ+τ(Ω.υ)+Ћ+τ(Ω.ν);for(int i=0;i<Ω.Ǡ.Length;i++)
έ+=Ћ+Math.Round(Ω.Ǡ[i],3);return έ;}Ψ ΗЁ(String s){String[]Е=s.Split(':');Ψ Ω=new Ψ(
μ(Λ(Е,0)),μ(Λ(Е,1)),
μ(Λ(Е,2)),μ(Λ(Е,3)),
μ(Λ(Е,4)));int Β=5;List<float>Εϊ=new List<float>();while(Β<Е.Length){String ΒΒ=Λ(Е,Β);float f=0;if(!float.TryParse(ΒΒ,out f)) break;Εϊ.Add(f);Β++;}Ω.Ǡ=Εϊ.ToArray();return Ω;}System.Text.StringBuilder š=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θώ){if(Θώ) š.Append('\n');š.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʒ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ΑΓ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Λ(String[]ΘΚ,int Β){String ΒΒ=ΘΚ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΒΒ)) return "";
return ΒΒ;}void Save(){if(ΑϏ||Η==Ζ.с){Storage="";return;}Storage=ΒΝ();}String ΒΝ(){š.Clear();š.Append(DATAREV+";");Ρ(τ(Αώ),false);Ρ(τ(Θ.β));Ρ(τ(Θ.υ));Ρ(τ(Θ.α));Ρ(τ(Θ.ν));Ρ(τ(Θ.Δ));Ρ(τ(Θ.Ч));Ρ(Θ.ε);Ρ(τ(Ε.Δ));Ρ(τ(Ε.ν));Ρ(τ(Ε.β));Ρ(τ(Ε.α));Ρ(τ(Ε.υ));Ρ(τ(Ε.Ч));Ρ(Ε.ε);Ρ(τ(ъ));Ρ(τ(ȟ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) ч);Ρ(ǯ);Ρ(ш);Ρ(Ы);Ρ(щ);Ρ(Ǩ);Ρ(Ȧ);Ρ(ȥ);if(Η==Ζ.Ϋ){Ρ((int) ό.Ќ);Ρ(ό.ǖ);Ρ(ό.Ā);Ρ(Ʒ(ό.Ǘ));Ρ(Ʒ(ό.ǚ));Ρ((int) ύ.Ќ);Ρ(ύ.ǖ);Ρ(ύ.Ā);Ρ(Ʒ(ύ.Ǘ));Ρ(Ʒ(ύ.ǚ));}else{Ρ((int) ȣ);Ρ((int) м);Ρ((int) М);Ρ((int) ф);Ρ((int) Ħ);Ρ(Ď);Ρ(Į);Ρ(Ő);Ρ(ŉ);Ρ(й);Ρ(и);Ρ(Ц);Ρ(ū);Ρ(Ū);Ρ(Ĳ);Ρ(ģ);
Ρ(Н);Ρ(б);Ρ(Ш);Ρ(Щ);Ρ(ȝ);Ρ(Й);Ρ(ī);Ρ(ĩ);}Ρ(";");for(int i=0;i<Ȋ.Count;i++)
Ρ((i>0?"|":"")+Ȋ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΘΔ(Σ[i]),i>0);Ρ(";",false);Ρ(Л+"|"+Ѐ,false);Ρ(";",false);Ρ(ΗΜ(),false);String Χ=š.ToString();š.Clear();return Χ;}т Ηπ(){return Ǉ(Storage);}т Ǉ(String Χ){if(Χ=="") return т.Ηθ;String[]Ĭ=Χ.Split(';');if(Λ(Ĭ,0)!=DATAREV) return т.Βξ;int Β=0;try{String[]Ι=Λ(Ĭ,1).Split('\n');
Αώ=μ(Λ(Ι,Β++));Θ.β=μ(Λ(Ι,Β++));Θ.υ=μ(Λ(Ι,Β++));Θ.α=μ(Λ(Ι,Β++));Θ.ν=μ(Λ(Ι,Β++));Θ.Δ=μ(Λ(Ι,Β++));Θ.Ч=μ(Λ(Ι,Β++));Θ.ε=bool.Parse(Λ(Ι,Β++));Ε.Δ=μ(Λ(Ι,Β++));Ε.ν=μ(Λ(Ι,Β++));Ε.β=μ(Λ(Ι,Β++));Ε.α=μ(Λ(Ι,Β++));Ε.υ=μ(Λ(Ι,Β++));Ε.Ч=μ(Λ(Ι,Β++));Ε.ε=bool.Parse(Λ(Ι,Β++));
ъ=μ(Λ(Ι,Β++));ȟ=μ(Λ(Ι,Β++));Ι=Λ(Ĭ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Λ(Ι,Β++));ί=(Μ) int.Parse(Λ(Ι,Β++));ч=(Μ) int.Parse(Λ(Ι,Β++));ǯ=int.Parse(Λ(Ι,Β++));ш=int.Parse(Λ(Ι,Β++));Ы=int.Parse(Λ(Ι,Β++));щ=int.Parse(Λ(Ι,Β++));Ǩ=float.Parse(Λ(Ι,Β++));Ȧ=bool.Parse(Λ(Ι,Β++));
ȥ=bool.Parse(Λ(Ι,Β++));if(Η==Ζ.Ϋ){ό.Ќ=(κ) int.Parse(Λ(Ι,Β++));ό.ǖ=float.Parse(Λ(Ι,Β++));ό.Ā=float.Parse(Λ(Ι,Β++));ό.Ǘ=ΑΓ(Λ(Ι,Β++));ό.ǚ=ΑΓ(Λ(Ι,Β++));ύ.Ќ=(κ) int.Parse(Λ(Ι,Β++));ύ.ǖ=float.Parse(Λ(Ι,Β++));ύ.Ā=float.Parse(Λ(Ι,Β++));ύ.Ǘ=ΑΓ(Λ(Ι,Β++));ύ.ǚ=ΑΓ(Λ(Ι,Β++));}else
{ȣ=(Ǒ) int.Parse(Λ(Ι,Β++));м=(д) int.Parse(Λ(Ι,Β++));М=(Ϗ) int.Parse(Λ(Ι,Β++));ф=(ѐ) int.Parse(Λ(Ι,Β++));Ħ=(Ǒ) int.Parse(Λ(Ι,Β++));Ď=bool.Parse(Λ(Ι,Β++));Į=bool.Parse(Λ(Ι,Β++));Ő=bool.Parse(Λ(Ι,Β++));ŉ=bool.Parse(Λ(Ι,Β++));й=int.Parse(Λ(Ι,Β++));и=int.Parse(Λ(Ι,Β++));Ц=int.Parse(Λ(Ι,Β++));
ū=float.Parse(Λ(Ι,Β++));Ū=float.Parse(Λ(Ι,Β++));Ĳ=float.Parse(Λ(Ι,Β++));ģ=float.Parse(Λ(Ι,Β++));Н=int.Parse(Λ(Ι,Β++));б=int.Parse(Λ(Ι,Β++));Ш=int.Parse(Λ(Ι,Β++));Щ=int.Parse(Λ(Ι,Β++));ȝ=int.Parse(Λ(Ι,Β++));Й=int.Parse(Λ(Ι,Β++));ī=int.Parse(Λ(Ι,Β++));ĩ=float.Parse(Λ(Ι,Β++));
}Ι=Λ(Ĭ,3).Replace("\n","").Split('|');Ȋ=Ι.ToList();Ι=Λ(Ĭ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(ΗЁ(Λ(Ι,i)));Ι=Λ(Ĭ,5).Split('|');Л=Λ(Ι,0);if(!int.TryParse(Λ(Ι,1),out Ѐ)) Ѐ=-1;if(Л.Length!=Н*б){Л="";Ѐ=-1;}ΗΛ(Λ(Ĭ,6));}catch{return т.Δτ;}Ţ=ч;ψ();return т.ǌ;}const int Δη=64000;IMyTerminalBlock Δυ(){List<IMyTerminalBlock>Ņ=new List<IMyTerminalBlock>();ϋ.GetBlocksOfType(Ņ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ņ.Count>0?Ņ[0]:null;}void ΗΟ(){IMyTerminalBlock Υ=Δυ();if(Υ==null){Κ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΝ();if(Χ.Length>Δη){Κ="Export too large: "+Χ.Length+" chars, max "+Δη;return;}Υ.CustomData=Χ;Κ="Exported "+Σ.Count+" waypoints";}void ΗΪ(bool Γς){IMyTerminalBlock Υ=Δυ();if(Υ==null){Κ="Import: name a block "+dataTag;return;}
ΗΫ(Υ.CustomData,Γς);}void ΗΫ(String Χ,bool Γς){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΧ=Χ.IndexOf('\n');Χ=ΖΧ<0?"":Χ.Substring(ΖΧ+1);}Χ=Χ.Trim();if(Χ==""){Κ="Import: no PAM data in "+dataTag;return;}ψ();String Εέ=ΒΝ();т έ=Ǉ(Χ);if(έ==т.ǌ&&Γς){Ψ ț=new Ψ(Ε);
Vector3 Θϊ=ъ,Θω=ȟ;int w=й,h=и,d=Ц;Ǒ ΔΙ=ȣ;ѐ ΓΥ=ф;float ΘЄ=Ĳ,ΘЃ=ģ;Ǉ(Εέ);Ε=ț;ъ=Θϊ;ȟ=Θω;й=w;и=h;Ц=d;ȣ=ΔΙ;ф=ΓΥ;Ĳ=ΘЄ;ģ=ΘЃ;ί=Μ.ļ;Ż(true,true);ί=Μ.ļ;ч=Μ.ļ;Κ="Imported job, run CONT";return;}if(έ==т.ǌ){ƙ=false;Κ="Imported "+Σ.Count+" waypoints";
return;}Ǉ(Εέ);Κ=έ==т.Βξ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ηϋ(){Ɔ.Clear();for(int i=ї.Count-1;i>=0;i--){String Χ=ї[i].CustomData.ToUpper();bool Γπ=false;if(Χ=="INSTRUCTIONS"){Γπ=true;ΑΦ=true;}if(Χ=="DEBUG") Γπ=true;if(Γπ){Ɔ.Add(ї[i]);ї.RemoveAt(i);}
}Ÿ(Ɔ,false,1,false);}void Ηό(List<IMyTerminalBlock>Ņ){Č.Clear();for(int i=0;i<Ņ.Count;i++){IMyTerminalBlock Υ=Ņ[i];try{String Ζή=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Υ.CustomName.IndexOf(Ζή);int Ť=-1;if(Β<0||!int.TryParse(Υ.CustomName.Substring(Β+Ζή.Length,1),out Ť)) continue;
if(Ť==-1) continue;Ť--;IMyTextSurfaceProvider Ζΰ=(IMyTextSurfaceProvider) Υ;if(Ť<Ζΰ.SurfaceCount&&Ť>=0)
Č.Add(Ζΰ.GetSurface(Ť));}catch{}}}String Αυ="";void ΕΑ(){String Γό="";String Γά="";Αυ=Δζ(false);Γό+=Αυ;Γά+=Αυ;Γά+=ΗΤ();for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γό);for(int i=0;i<Č.Count;i++) Č[i].WriteText(Γό);Echo(Γά);for(int i=0;i<Ɔ.Count;i++){IMyTextPanel ΓЈ=Ɔ[i];
String Χ=ΓЈ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЈ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЈ.WriteText(ΗΥ());}}string ΗΥ(){String Γ="";try{float ΖΞ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(ń*ΖΞ)+" / "+(int)(ŕ*ΖΞ)+"\n";Γ+="Inst. avg/max: "+Math.Round(ń*100f,1)+"% / "+Math.Round(ŕ*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ƀ,2)+"ms / "+Math.Round(ΓЀ,2)+"ms \n";}catch{}for(int i=0;i<Ƞ.Count;i++)
Γ+=""+Ƞ.Keys.ElementAt(i)+" = "+Math.Round(Ƞ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ƞ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΨ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ÿ(ї,setLCDFontAndSize,0.9f,false);Ÿ(Č,setLCDFontAndSize,0.9f,true);for(int i=0;i<ї.Count;i++) ї[i].WriteText(Γ+Ȝ.ToString());for(int i=0;i<Č.Count;i++) Č[i].WriteText(Γ+Ȝ.ToString());}void ǽ(){if(!ΑΦ) return;try{ΓЌ=Runtime.CurrentInstructionCount;}catch{}
}void Ƿ(String φ){if(!ΑΦ) return;if(ΓЌ==0) return;try{float ΑБ=(Runtime.CurrentInstructionCount-ΓЌ)/Runtime.MaxInstructionCount*100;if(!Ƞ.ContainsKey(φ))
Ƞ.Add(φ,new float[]{ΑБ,ΑБ});else{Ƞ[φ][0]=ΑБ;Ƞ[φ][1]=Math.Max(ΑБ,Ƞ[φ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖЉ(String Ă){if(!ņ) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=Ŭ(ΔΓ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=Βέ(ή.X)+""+Ћ;ρ+=Βέ(ή.Y)+Ћ;ρ+=Βέ(ή.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=Βί(ч)+(ч==Μ.Є?" "+Math.Round(ƒ,1)+"%":"")+Ћ;else ρ+=Βί(ч)+Ћ;ρ+=Ŭ(Κ)+Ћ;ρ+=љ+""+Ћ;ρ+=Ŭ(Αυ)+Ћ;ρ+=ň+Ћ;ρ+=Ŗ+Ћ;for(int i=0;i<Ǚ.Count;i++){if(Ǚ[i].œ!=З.ű) continue;ρ+=Ŭ(Ǚ[i].φ)+"/"+Ŭ(Ǚ[i].ς)+"/"+Ǚ[i].Ǔ+Ћ;}Ηψ(ρ,Ă);}String Ŭ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηψ(String ρ,String Ă){Ηω(ρ,BROADCAST_SELF_ID,Ă);}void Ηω(String ρ,String Њ,String Ă){try{if(!ņ) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δψ()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΓΘ+BROADCAST_SEP+Ă+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){Ȝ=e;}
}String Δψ(){if(Η!=Ζ.с) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЈ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ů(ref String Χ,out string Εϋ,Char Ћ){int Β=Χ.IndexOf(Ћ);Εϋ="";if(Β<0) return false;Εϋ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηι(ref String ρ,out String Ɵ,out String Ă)
{Ɵ="";Ă="";if(!ņ) return false;String Ǟ="";if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||!ΔЈ(Ǟ)) return false;if(!Ů(ref ρ,out Ɵ,BROADCAST_SEP)) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=Δψ()&&(Ǟ!="*"&&Η!=Ζ.с))) return false;if(!Ů(ref ρ,out Ǟ,BROADCAST_SEP)||(Ǟ!=ΓΘ)) return false;
if(!Ů(ref ρ,out Ă,BROADCAST_SEP)) return false;return true;}void Ηέ(){bool Ƅ=true;if(Εύ){Εύ=false;if(Me.CustomData.Contains("Antenna_Name")){Κ="Update custom data";Me.CustomData="";}}String Ɛ=(Η!=Ζ.с?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɛ)
ΔЄ(Ɛ);String[]Ι=Me.CustomData.Split('\n');ņ=bool.Parse(Βψ(ΒΫ(Ι,"Enable_Broadcast",ref Ƅ)));bool ΔΫ=false;bool Ες=true;if(ņ){if(Η!=Ζ.с)
ΔΓ=Βψ(ΒΫ(Ι,"Ship_Name",ref Ƅ)).Replace(BROADCAST_SELF_ID,"");ΓΘ=Βψ(ΒΫ(Ι,"Broadcast_Channel",ref Ƅ)).ToLower();Ες=false;if(ȕ==null){ȕ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȕ.SetMessageCallback("");}List<IMyRadioAntenna>Ł=new List<IMyRadioAntenna>();ϋ.GetBlocksOfType(Ł);bool ΒЍ=false;
for(int i=0;i<Ł.Count;i++)
if(Ł[i].EnableBroadcasting&&Ł[i].Enabled){ΒЍ=true;break;}if(Ł.Count==0) Κ="No Antenna found";else if(!ΒЍ) Κ="Antenna not ready";ΔΫ=Ł.Count==0||!ΒЍ;if(ΕΦ&&!ΔΫ&&Η!=Ζ.с) Κ="Antenna ok";}else if(Η==Ζ.с)
Κ="Offline - Enable in PB custom data";ΕΦ=ΔΫ;if(Ες){if(ȕ!=null) this.IGC.DisableBroadcastListener(ȕ);ȕ=null;}}catch{Ƅ=false;}if(!Ƅ){Κ="Reset custom data";ΔЄ(Ɛ);}}void ΔЄ(String Ɛ){String Χ=Ɛ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.с?"true":"false")+" \n";Χ+=Η!=Ζ.с?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βψ(String Ĩ){int Β=Ĩ.IndexOf("//");if(Β!=-1) Ĩ=Ĩ.Substring(0,Β);String[]Е=Ĩ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΫ(String[]Ι,String ħ,ref bool ƚ){foreach(String Ĩ in Ι)
if(Ĩ.StartsWith(ħ)) return Ĩ;ƚ=false;return "";}String ΙϏ="";Ȓ ΙЁ=null;DateTime ΙЂ=DateTime.MinValue;List<Ȓ>ΙЀ=new List<Ȓ>();const float Ικ=20f;String ΕΔ(int ΔΪ,String Γ,int Ζε,ref int ȡ){String[]Ι=Γ.Split('\n');if(Ζε>=ȡ+ΔΪ-1) ȡ++;ȡ=Math.Min(Ι.Count()-1-ΔΪ,ȡ);if(Ζε<ȡ+1) ȡ--;
ȡ=Math.Max(0,ȡ);String έ="";for(int i=0;i<ΔΪ;i++){int ΖΙ=i+ȡ;if(ΖΙ>=Ι.Count()) break;έ+=Ι[ΖΙ]+"\n";}return έ;}String Βή(int ƞ){if(ƞ>=60*60) return Math.Round(ƞ/(60f*60f),1)+" h";if(ƞ>=60) return Math.Round(ƞ/60f,1)+" min";return ""+ƞ+" s";}string Βέ(float v){return Math.Round(v,2)+" ";}
List<IMyCameraBlock>Ǫ=new List<IMyCameraBlock>();List<long>Ś=new List<long>();bool Ŝ=false;int ΓΞ=0,ΘЏ=0,ΓΖ=0;const int Ης=16;int ΓΜ=0;DateTime Εα=DateTime.Now;String ΓΗ="";String ΓΛ="";void Ηχ(){Ǫ.Clear();Ś.Clear();if(!obstacleBrake) return;ϋ.GetBlocksOfType(Ǫ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);
for(int i=0;i<Ǫ.Count;i++) Ǫ[i].EnableRaycast=true;List<IMyTerminalBlock>ΑΙ=new List<IMyTerminalBlock>();ϋ.GetBlocksOfType(ΑΙ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΑΙ.Count;i++) if(!Ś.Contains(ΑΙ[i].CubeGrid.EntityId)) Ś.Add(ΑΙ[i].CubeGrid.EntityId);if(!Ś.Contains(Me.CubeGrid.EntityId)) Ś.Add(Me.CubeGrid.EntityId);
ΓΛ="";if(Τ==null) return;MatrixD m=Τ.WorldMatrix;Vector3[]ΘΫ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Θό={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ǫ.Count;i++)
if(Vector3.Dot(Ǫ[i].WorldMatrix.Forward,ΘΫ[d])>0.7f){ΓΛ+=Θό[d]+" ";break;}}bool ΔЇ(Α s){return s==Α.ǥ||s==Α.Ю||s==Α.Ѕ||
s==Α.ȫ||s==Α.ĸ||s==Α.ƹ||
(s==Α.ǣ&&њ!=null);}void Ƶ(){Ŝ=false;ΓΞ=0;ΓΗ="";}void ΗЏ(){if(!obstacleBrake||Ǫ.Count==0||!ΔЇ(Φ)){Ƶ();return;}Vector3 Ə=χ+č;Vector3 Ǳ=Ə-ή;float ǟ=Ǳ.Length();if(ǟ<1f){Ƶ();return;}Vector3 ζ=Ǳ/ǟ;Vector3 ȍ=Τ!=null?(Vector3) Τ.GetShipVelocities().LinearVelocity:new Vector3();
if(ȍ.Length()>2) ζ=Vector3.Normalize(ȍ);Vector3 ΑЅ=Vector3.Cross(ζ,Math.Abs(ζ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΑЅ.Normalize();Vector3 ϊ=Vector3.Cross(ΑЅ,ζ);float ω=Math.Min(ǟ+δ*0.25f,Ȣ*obstacleLookAhead+δ);int Εδ=0;for(int k=0;k<2;k++){int p=k==0?0:ΘЏ++%8,e=p%4;
Vector3 ΘЂ=k==0?new Vector3():(e==0?ΑЅ:e==1?-ΑЅ:e==2?ϊ:-ϊ)*δ*0.5f;Vector3 ƍ=ή+ζ*(p>=4?ω*0.5f:ω)+ΘЂ;IMyCameraBlock Εγ=ΖЎ(ƍ);if(Εγ==null){Δν();continue;}Εδ++;MyDetectedEntityInfo Ș=Εγ.Raycast(ƍ);bool Ιέ=Ș.Type==MyDetectedEntityType.Planet||Ș.Type==MyDetectedEntityType.Asteroid;
bool ΘЁ=!Ș.IsEmpty()&&!Ś.Contains(Ș.EntityId)&&!Ηγ(Ș.EntityId)&&
Ș.Type!=MyDetectedEntityType.FloatingObject&&!(Ιέ&&(k>0||Φ==Α.ǥ||
(Ș.HitPosition.HasValue&&Vector3D.Distance(Ș.HitPosition.Value,Ə)<δ)));if(ΘЁ){if(!Ŝ) Εα=DateTime.Now;Ŝ=true;ΓΞ=0;ΓΗ=Ș.Type.ToString();}else Δν();}ΓΖ=Εδ>0?0:ΓΖ+1;if(ΓΖ>12) Κ="Cameras can't see ahead";if(Ŝ&&(DateTime.Now-Εα).TotalSeconds>obstacleTimeout)
Κ="Path blocked: "+ΓΗ;}void Δν(){if(Ŝ&&++ΓΞ>=Ης) Ƶ();}IMyCameraBlock ΖЎ(Vector3 ƍ){IMyCameraBlock Ȁ=null;float Εΰ=-2;for(int n=0;n<Ǫ.Count;n++){ΓΜ=(ΓΜ+1)%Ǫ.Count;IMyCameraBlock c=Ǫ[ΓΜ];if(!c.IsFunctional||!c.IsWorking) continue;float Ζγ=Vector3.Dot(Vector3.Normalize(ƍ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Ζγ>Εΰ+0.05f&&c.CanScan(ƍ)){Ȁ=c;Εΰ=Ζγ;}}return Ȁ;}const String ΕΟ="[PAMTFC]";const float ΗЄ=5f;const float ΒΪ=2f;class Ȭ{public long λ;public Vector3 η,ȍ;public Vector3 ō;public float Ερ;public String ț="";public int Γμ=-1;public bool ΓЍ;public int Ƒ;public float Γβ,Γα;public long ά=0;
public int ƈ=0;public DateTime ΑЎ;}IMyBroadcastListener ΑА=null;long ы=0;List<Ȭ>н=new List<Ȭ>();Vector3 č=new Vector3();float ю=-1;Vector3 ΔΤ=new Vector3();bool ΑЏ=false;DateTime ΔΥ=DateTime.Now;const float ΕΞ=15f;const float ΗЃ=10f;int ΖϏ=0;String Л="";int Ѐ=-1;bool ΑΠ=false,ΑΟ=false;
Vector3 Εθ=new Vector3();String ĺ(){if(ί==Μ.з||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Н+"|"+б+"|"+(int) Ħ+"|"+Math.Round(ǡ,1)+"|"+Math.Round(ǳ,1);}void ΘΑ(){if(!trafficEnabled) return;if(ΑА==null) ΑА=IGC.RegisterBroadcastListener(ΕΟ);ы=Me.CubeGrid.EntityId;int ĝ=30;while(ΑА.HasPendingMessage&&ĝ-->0){String Χ=ΑА.AcceptMessage().Data as String;if(Χ!=null) Ημ(Χ);
}for(int i=н.Count-1;i>=0;i--)
if((DateTime.Now-н[i].ΑЎ).TotalSeconds>ΗЄ) н.RemoveAt(i);if(ΑЋ==null) ΑЋ=IGC.RegisterBroadcastListener(ΗЂ);ĝ=5;while(ΑЋ.HasPendingMessage&&ĝ-->0){String Χ=ΑЋ.AcceptMessage().Data as String;if(Χ!=null) try{Ην(Χ);}catch{}}if(ΑΡ==null) ΑΡ=IGC.RegisterBroadcastListener(Ηδ);
ĝ=10;while(ΑΡ.HasPendingMessage&&ĝ-->0){String Χ=ΑΡ.AcceptMessage().Data as String;if(Χ!=null) try{Ηκ(Χ);}catch{}}if(++ΖϏ<3) return;ΖϏ=0;IGC.SendBroadcastMessage(ΕΟ,ы+";"+τ(ή)+";"+
τ(Τ.GetShipVelocities().LinearVelocity)+";"+δ+";"+ĺ()+";"+
(ί==Μ.Є?Ѐ:-1)+";"+Л+";"+ΗΓ()+","+Δς()+";"+τ(ΔΤ)+
";"+(ΑΩ?1:0)+";"+(ă&&ί==Μ.Є?1:0)+","+ΑЂ+","+ΔΒ+","+Ηε());}void Ημ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long λ=long.Parse(f[0]);if(λ==ы) return;Ȭ Ї=null;for(int i=0;i<н.Count;i++) if(н[i].λ==λ) Ї=н[i];if(Ї==null){Ї=new Ȭ();Ї.λ=λ;н.Add(Ї);}
Ї.η=μ(f[1]);Ї.ȍ=μ(f[2]);Ї.Ερ=float.Parse(f[3]);Ї.ț=f[4];Ї.Γμ=int.Parse(f[5]);Ї.ΑЎ=DateTime.Now;if(Ї.ț!=""&&Ї.ț==ĺ()) ΔЏ(f[6]);Ї.ά=0;Ї.ƈ=0;if(f.Length>=8){String[]d=f[7].Split(',');Ї.ά=long.Parse(d[0]);Ї.ƈ=int.Parse(d[1]);}Ї.ō=f.Length>=9?μ(f[8]):new Vector3();
Ї.ΓЍ=f.Length>=10&&f[9]=="1";Ї.Ƒ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Ї.Γβ=float.Parse(w[1]);Ї.Γα=float.Parse(w[2]);Ї.Ƒ=int.Parse(w[3]);}}}catch{}}bool Ηγ(long λ){for(int i=0;i<н.Count;i++) if(н[i].λ==λ) return true;return false;}Vector3 Δό(int Β,int ΘЅ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΪ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*ΘЅ;Vector3 ϊ=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.α;Vector3 Ь=Vector3.Cross(ΙΪ,ϊ);if(Ь.LengthSquared()<0.0001f) return new Vector3();
float ω=laneOffset*δ;float Θα=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ь)*ω*Math.Min(1,Θα/(ω*4));}Vector3 Ζώ=new Vector3();bool ΔЁ(){Α s=Φ;return ĺ()!=""&&(s==Α.ǥ||s==Α.Ļ||s==Α.ǻ||
s==Α.Ǻ||s==Α.ķ);}int Ηε(){if(!ΔЁ()) return 0;return Φ!=Α.ǥ&&Vector3.Dot(ή-л,ъ)>δ*0.5f?2:1;}bool Ηβ(Α s){return s==Α.ǣ||s==Α.Đ||s==Α.Ĕ||s==Α.ŷ||
s==Α.ǒ||s==Α.ǻ||s==Α.Ǻ||s==Α.ķ||s==Α.ȧ;}void ΘΒ(){č=new Vector3();ю=-1;ΔΤ=new Vector3();bool ΕЀ=ΔЇ(Φ);if(!trafficEnabled||(!ΕЀ&&!Ηβ(Φ))){ΑЏ=false;return;}Ζώ=Τ.GetShipVelocities().LinearVelocity;ΗΔ(Τ.GetNaturalGravity(),!ΕЀ);ΖЇ();}IMyBroadcastListener ΑЋ=null;
float Ŧ=-1;DateTime Ζϊ=DateTime.MinValue;void Ην(String Χ){if(!Χ.StartsWith("C;")) return;Ζϊ=DateTime.Now;Ŧ=-1;String Ɨ=ы+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) Ŧ=float.Parse(e[i].Substring(Ɨ.Length));}void ΖЇ(){if(Ŧ<0||(DateTime.Now-Ζϊ).TotalSeconds>1.5) return;
ю=ю<0?Ŧ:Math.Min(ю,Ŧ);if(Ŧ<0.5f) Κ="Traffic: giving way (controller)";}void ΗΔ(Vector3 ν,bool Ζέ=false){č=new Vector3();ю=-1;Vector3 Ǳ=χ-ή;float ǟ=Ǳ.Length();bool Γκ=ǟ>=1f;Vector3 ζ=Γκ?Ǳ/ǟ:new Vector3();ΔΤ=ζ;Vector3 ϊ=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-Ɗ;
Vector3 Ь=Vector3.Cross(ζ,ϊ);Ь=Ь.LengthSquared()<0.0001f?-ƕ:Vector3.Normalize(Ь);Ȭ Αλ=null;bool ΕЂ=Φ==Α.Ѕ&&!ȓ(1);for(int i=0;i<н.Count;i++){Ȭ γ=н[i];if(γ.ƈ==3&&γ.ȍ.LengthSquared()<0.25f) continue;Vector3 η=γ.η+γ.ȍ*(float)(DateTime.Now-γ.ΑЎ).TotalSeconds;Vector3 Ş=η-ή;
float Ǖ=(δ+γ.Ερ)*0.5f;bool Ζβ=γ.ț!=""&&γ.ț==ĺ();if(Ζβ&&γ.Ƒ==2) continue;bool ΑІ=Ζβ&&γ.Ƒ==1&&ΔЁ();if(ΑІ) Ǖ=Math.Min(ǡ,ǳ)*0.9f;float ΓΡ=Ǖ*(ΑІ?1f:1.2f);float ž=Γκ?Vector3.Dot(Ş,ζ):0;if(ž>0&&(Ş-ζ*ž).Length()<=ΓΡ){float Αϊ=Vector3.Dot(γ.ȍ,ζ);bool ΕЈ=γ.ō.LengthSquared()>0.25f;
bool ΕЉ=Αϊ<-1f||(ΕЈ&&Vector3.Dot(γ.ō,ζ)<-0.5f);if(!Ζέ){if(γ.ΓЍ){if(!ΕЂ){č=Ь*Ǖ*1.3f;continue;}float ΖΤ=Vector3.Distance(ή,χ),ΖΓ=Vector3.Distance(η,χ);if(ΖΤ<ΖΓ||(ΖΤ==ΖΓ&&ы<γ.λ)) continue;}if(ΕЉ){if(ž<(Ȣ-Αϊ)*6f+Ǖ*4f) č=Ь*Ǖ*1.3f;continue;}if(ΕЈ&&ы<γ.λ)
{Vector3 Εά=-Ş;float ΖΪ=Vector3.Dot(Εά,γ.ō);if(ΖΪ>0&&(Εά-γ.ō*ΖΪ).Length()<=ΓΡ){č=Ь*Ǖ*1.3f;continue;}}}float ΕЁ=ž-Ǖ-(ΑІ?1f:followGap*δ);float Ακ=Math.Max(0,Αϊ)+Math.Max(0,ΕЁ)/ΒΪ;if(ΕЉ) Ακ=Math.Max(0,ΕЁ+Math.Min(0,Αϊ)*ΒΪ*2)/ΒΪ;if(ю<0||Ακ<ю){ю=Ακ;Αλ=γ;}
continue;}if(γ.ȍ.LengthSquared()<1f) continue;if(Γκ&&(Ş-ζ*ž).Length()<=ΓΡ) continue;Vector3 ΔΑ=γ.ȍ-Ζώ;float Ζα=ΔΑ.LengthSquared();if(Ζα<0.25f) continue;float t=-Vector3.Dot(Ş,ΔΑ)/Ζα;if(t<=0||t>5f||(Ş+ΔΑ*t).Length()>=(ΑІ?Ǖ:Ǖ*1.1f)) continue;if(ы>γ.λ||t<2f){ю=0;Αλ=γ;}
}bool Θθ=ю>=0&&ю<0.5f;if(!Θθ||Ζέ||(Αλ!=null&&Αλ.ΓЍ&&ΕЂ)){ΑЏ=false;return;}if(!ΑЏ){ΑЏ=true;ΔΥ=DateTime.Now;}double ΕЊ=(DateTime.Now-ΔΥ).TotalSeconds;if(ΕЊ>ΕΞ+ΗЃ) ΔΥ=DateTime.Now;else if(ΕЊ>ΕΞ){ю=-1;č=Ь*δ*1.3f;Κ="Traffic: passing a stuck ship";}
}List<int>Ȇ=new List<int>();String ΕЌ="";void ΒΩ(){String ħ=Н+"|"+б+"|"+(int) Ħ;if(ħ==ΕЌ) return;ΕЌ=ħ;ΖЊ(Н,б,Ħ,Ȇ);}void ΖЊ(int w,int h,Ǒ ΔΙ,List<int>ΓΝ){ΓΝ.Clear();if(ΔΙ==Ǒ.ΑΑ){for(int k=0;k<w*h;k++){int ş=k/w;ΓΝ.Add(ş*w+(ş%2==0?k-ş*w:w-1-(k-ş*w)));
}return;}int ΕЇ=(int) Math.Ceiling(w/2f),ΕІ=(int) Math.Ceiling(h/2f);int Θδ=w/2,Θγ=h/2;int x=0,y=0,ΓΪ=0,ΓΫ=-1,Αρ=Math.Max(w,h);for(int i=0;i<Αρ*Αρ;i++){if(-ΕЇ<x&&x<=Θδ&&-ΕІ<y&&y<=Θγ) ΓΝ.Add((y-1+ΕІ)*w+(x-1+ΕЇ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΪ;ΓΪ=-ΓΫ;ΓΫ=t;}
x+=ΓΪ;y+=ΓΫ;}}bool ΒΨ(){int n=Math.Max(0,Н*б);if(Л.Length==n) return false;Л=new String('0',n);Ѐ=-1;return true;}void ΔЏ(String γ){ΒΨ();if(γ.Length!=Л.Length) return;char[]h=Л.ToCharArray();for(int i=0;i<h.Length;i++) if(γ[i]=='1') h[i]='1';Л=new String(h);}bool ƾ(int ȁ){return ȁ>=0&&ȁ<Л.Length&&Л[ȁ]=='1';}
void ΒϏ(int ȁ,bool х){if(ȁ<0||ȁ>=Л.Length) return;char[]h=Л.ToCharArray();h[ȁ]=х?'1':'0';Л=new String(h);}bool Βη(int ȁ,bool Θξ){if(!trafficEnabled) return false;String ħ=ĺ();for(int i=0;i<н.Count;i++){Ȭ t=н[i];if(t.ț!=ħ||t.Γμ<0||(Θξ&&t.λ>ы)) continue;if(t.Γμ==ȁ) return true;
}return false;}int ΗΒ(int Β){int n=Н*б;if(n<1) return Β;ΒΩ();if(ΒΨ()) for(int k=0;k<Β&&k<n;k++) ΒϏ(Ȇ[k],true);if(Ѐ>=0&&(Β>=n||Ȇ[Β]!=Ѐ)){ΒϏ(Ѐ,true);Ѐ=-1;}if(ΔЉ()){if(Ǭ>=0&&!ƾ(Ǭ)){ă=false;Ѐ=Ǭ;return Ȇ.IndexOf(Ǭ);}if(Ǭ==-2){Ѐ=-1;ă=Л.IndexOf('0')>=0;return ă?-2:n;}
if(Ǭ==-1&&Ѐ>=0&&!ƾ(Ѐ)){ă=false;return Β;}Ѐ=-1;if(!ă){ă=true;Εκ=DateTime.Now;}if((DateTime.Now-Εκ).TotalSeconds<5) return-1;}ă=false;if(Ѐ>=0&&!ƾ(Ѐ)&&!Βη(Ѐ,true)) return Β;Ѐ=-1;for(int k=0;k<n;k++){int c=Ȇ[k];if(ƾ(c)||Βη(c,false)) continue;Ѐ=c;
return k;}return Л.IndexOf('0')>=0?-2:n;}const String Ηδ="[PAMJOB]";const String ΗЂ="[PAMTCC]";IMyBroadcastListener ΑΡ=null;DateTime Ει=DateTime.MinValue;DateTime Εκ=DateTime.Now;int Ǭ=-1;bool ă=false;bool ΔЉ(){return trafficEnabled&&Ǭ!=-3&&(DateTime.Now-Ει).TotalSeconds<3;}
void Ηκ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ĺ()) return;Ει=DateTime.Now;ΔЏ(f[2]);Ǭ=-3;String Ɨ=ы+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ɨ)) Ǭ=int.Parse(e[i].Substring(Ɨ.Length));}ľ Ηϊ(int Β)
{ΒΩ();if(Β<0||Β>=Ȇ.Count) return ľ.Δδ;Ш=Ȇ[Β]%Н;Щ=Ȇ[Β]/Н;return ľ.Δφ;}class ђ{public long λ;public Vector3 η,В,ϊ;}List<ђ>Ў=new List<ђ>();bool ȱ=false;Vector3 ΑΫ,ΑΪ,Αά,Αέ;long ц=0;long Ȉ=0;Ψ њ=null;bool Ɖ=false;Ψ ΒΥ(Vector3 η,Vector3 В,Vector3 ϊ){MatrixD m=MatrixD.CreateWorld(η,В,ϊ);
Ψ w=new Ψ(Θ);w.β=Vector3D.TransformNormal(ΑΫ,m);w.α=Vector3D.TransformNormal(ΑΪ,m);w.υ=Vector3D.TransformNormal(Αά,m);w.Δ=η+(Vector3) Vector3D.TransformNormal(Αέ,m);w.ε=true;return w;}int Δς(){Α s=Φ;if(s==Α.ж) return П(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ĕ||s==Α.Ǿ||s==Α.ē||s==Α.ΑΖ||
s==Α.Ƹ||s==Α.ǒ) return 3;if(s==Α.ǣ||s==Α.Đ||s==Α.ŷ) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΗΓ(){if(Δς()==0) return 0;if(ȓ(1)&&ţ!=0) return ţ;return њ!=null?Ȉ:ц;}void ΔЌ(){if(Η==Ζ.Ϋ||!Θ.ε) return;IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector γ=И.OtherConnector;Vector3I Θι=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==Θι){MatrixD ŏ=MatrixD.Transpose(γ.WorldMatrix);ΑΫ=Vector3D.TransformNormal(Τ.WorldMatrix.Forward,ŏ);ΑΪ=Vector3D.TransformNormal(Τ.WorldMatrix.Down,ŏ);Αά=Vector3D.TransformNormal(Τ.WorldMatrix.Left,ŏ);
Αέ=Vector3D.TransformNormal(И.GetPosition()-γ.GetPosition(),ŏ);ȱ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) ц=γ.EntityId;List<IMyShipConnector>Ģ=new List<IMyShipConnector>();ϋ.GetBlocksOfType(Ģ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ģ.Count>0){Ў.Clear();for(int i=0;i<Ģ.Count;i++){ђ p=new ђ();p.λ=Ģ[i].EntityId;p.η=Ģ[i].GetPosition();p.В=Ģ[i].WorldMatrix.Forward;p.ϊ=Ģ[i].WorldMatrix.Up;Ў.Add(p);}}њ=null;Ȉ=0;if(γ.EntityId==ц||!ȱ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].λ==γ.EntityId){Ȉ=Ў[i].λ;њ=ΒΥ(Ў[i].η,Ў[i].В,Ў[i].ϊ);}}bool Βκ(long λ,int ΖΥ){if(λ==0) return false;for(int i=0;i<н.Count;i++){Ȭ t=н[i];if(t.ά==λ&&(t.ƈ>ΖΥ||(t.ƈ==ΖΥ&&t.λ<ы))) return true;}return false;}bool ΒΟ(int Αϋ){if(Η==Ζ.Ϋ||!trafficEnabled){њ=null;Ȉ=0;return true;}
long ι=њ!=null?Ȉ:ц;if(!Βκ(ι,Αϋ)) return true;if(!Βκ(ц,Αϋ)){њ=null;Ȉ=0;return true;}if(!ȱ) return false;ђ Ȁ=null;float Εί=0;for(int i=0;i<Ў.Count;i++){ђ p=Ў[i];if(p.λ==ц||Βκ(p.λ,Αϋ)) continue;float d=Vector3.Distance(p.η,Θ.Δ);if(Ȁ==null||d<Εί){Ȁ=p;Εί=d;}
}if(Ȁ==null) return false;Ȉ=Ȁ.λ;њ=ΒΥ(Ȁ.η,Ȁ.В,Ȁ.ϊ);return true;}String ΗΜ(){String s=(ȱ?"1":"0")+":"+τ(ΑΫ)+":"+τ(ΑΪ)+":"+
τ(Αά)+":"+τ(Αέ)+":"+ц;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].λ+","+τ(Ў[i].η)+","+τ(Ў[i].В)+","+τ(Ў[i].ϊ);return s;}void ΗΛ(String s){Ў.Clear();ȱ=false;ц=0;String[]f=s.Split(':');if(f.Length<6) return;ȱ=f[0]=="1";ΑΫ=μ(f[1]);ΑΪ=μ(f[2]);Αά=μ(f[3]);Αέ=μ(f[4]);long.TryParse(f[5],out ц);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ђ p=new ђ();if(!long.TryParse(e[0],out p.λ)) continue;p.η=μ(e[1]);p.В=μ(e[2]);p.ϊ=μ(e[3]);Ў.Add(p);}}const String ΒΡ="[PAMDCK]";const float Ιι=120f;class Δσ{public long Ї;public int Ɠ;public long Γν;public bool Ιϊ;public DateTime ΙБ,Θο;
}IMyBroadcastListener ΙЎ=null;List<Δσ>ΙЏ=new List<Δσ>();List<IMyShipConnector>ΚΥ=new List<IMyShipConnector>();List<IMySensorBlock>Κγ=new List<IMySensorBlock>();List<IMyCameraBlock>ΚΛ=new List<IMyCameraBlock>();List<MyDetectedEntityInfo>Κβ=new List<MyDetectedEntityInfo>();long Ιύ=0;
int Ιό=0;long Ιϋ=0;DateTime Ιώ=DateTime.Now;String ΙЍ="";int ΙА=0;IMyCubeGrid ΚΦ=null;IMyBroadcastListener ΑЉ=null;int ȅ=0;DateTime Ευ=DateTime.Now;DateTime Ζλ=DateTime.MinValue;long Ζμ=0;int Ζν=0;long ţ=0;bool Ƭ=false;int ΓΩ=0;List<Vector3>Ʈ=new List<Vector3>();List<float>Δβ=new List<float>();
bool ΑΩ=false;bool ΑΨ=false;Vector3 Ετ=new Vector3();bool ź(){return ȅ!=0&&(DateTime.Now-Ζλ).TotalSeconds<3;}bool ȓ(int Ɠ){return ź()&&Ζμ==ы&&Ζν==Ɠ;}bool ΕΘ(){return ź()||(DateTime.Now-Ευ).TotalSeconds<3;}void Βύ(int Ɠ){if(ȅ==Ɠ) return;ȅ=Ɠ;Ευ=DateTime.Now;ΓΩ=3;
}void Βό(){if(ȅ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΡ,"D;"+ы);ȅ=0;Ƭ=false;}void ΗЊ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑЉ==null) ΑЉ=IGC.RegisterBroadcastListener(ΒΡ);int ĝ=20;while(ΑЉ.HasPendingMessage&&ĝ-->0){String Χ=ΑЉ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;
try{Ηλ(Χ);}catch{}}if(Φ!=Α.Ѕ){ΑΩ=false;ΑΨ=false;}if(ȅ==1&&П(MyShipConnectorStatus.Connected)!=null) Βό();if(ȅ==2&&!Ƭ&&Φ!=Α.ǒ&&
Vector3.Distance(ή,е().Δ)>δ*3) Βό();if(ȅ!=0&&++ΓΩ>=3){ΓΩ=0;IGC.SendBroadcastMessage(ΒΡ,"R;"+ы+";"+ȅ+";"+ц+";"+(ȱ?1:0));}}void Ηλ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool ΕϏ=false;String[]ΕΡ=f[5].Split(',');for(int i=0;i<ΕΡ.Length;i++) if(ΕΡ[i]==ы.ToString()) ΕϏ=true;
if(!ΕϏ) return;Ζλ=DateTime.Now;Ζμ=long.Parse(f[2]);Ζν=int.Parse(f[3]);ţ=long.Parse(f[4]);Ʈ.Clear();Δβ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;Ʈ.Add(μ(e[0]));Δβ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εχ=f[6].Split(':');List<ђ>Т=new List<ђ>();for(int i=0;i<Εχ.Length;i++){String[]e=Εχ[i].Split(',');if(e.Length<4) continue;ђ p=new ђ();p.λ=long.Parse(e[0]);p.η=μ(e[1]);p.В=μ(e[2]);p.ϊ=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖБ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;
Βύ(1);if(!ΕΘ()) return true;if(!ȓ(1)){Κ="Waiting for dock clearance";return false;}ΒΘ();return true;}bool ΒΘ(){Ɖ=false;ђ Ī=null;if(ţ!=0&&ţ!=ц&&ȱ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].λ==ţ) Ī=Ў[i];long ΒΔ=Ī==null?0:Ī.λ;if(ΒΔ==Ȉ&&(ΒΔ==0)==(њ==null)) return false;Ȉ=ΒΔ;њ=Ī==null?null:ΒΥ(Ī.η,Ī.В,Ī.ϊ);return true;}bool Ĺ(Vector3 p){for(int i=0;i<Ʈ.Count;i++) if(Vector3.Distance(p,Ʈ[i])<Δβ[i]+δ*0.75f) return true;
return false;}bool ΗΩ(Vector3 ƅ,Vector3 Ř){ΑΩ=false;if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return false;if(ȅ!=1&&Vector3.Distance(ή,Θ.Δ)>dockRequestDistance) return false;Βύ(1);if(!ź()||ȓ(1)||Ʈ.Count==0) return false;if(!Ĺ(ƅ)&&!Ĺ(Ř)&&!Ĺ(ή)) return false;ΑΩ=true;return true;
}bool Δλ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;Βύ(2);if(!ΕΘ()) return true;if(!ȓ(2)){Κ="Waiting for undock clearance";return false;}return true;}const float Ιο=600f;class ΔЋ{public long λ;public int ȁ=-1;public bool ΒΔ;public float Γβ,Γα;public DateTime ΚΞ;}class ΔЊ{public String ħ;
public int w,h;public float Κά,ΚΪ;public String х;public List<int>ΚΝ=new List<int>();public List<ΔЋ>ΚΠ=new List<ΔЋ>();}List<ΔЊ>ΚΚ=new List<ΔЊ>();IMyBroadcastListener ΚΙ=null;int ΚΘ=0;const float Ιω=8f;class ΕΝ{public long λ;public Vector3 η,ȍ,ō;public float ΙЋ,Γβ,Γα,Ακ=-1;public int Ƒ;
public String ț="";public int Αϋ;public DateTime ΑЎ;}class ΕΜ{public long a,b,Κα;public DateTime Θο;}List<ΕΝ>ΚΩ=new List<ΕΝ>();List<ΕΜ>ΚΧ=new List<ΕΜ>();int ΚΨ=0;List<Ȓ>ΚΓ=new List<Ȓ>();Ȓ ΚΑ=null;Vector3 ΚΖ;DateTime ΚΕ;float ΚΔ=40;bool ΚΒ=false;void ΗΘ(string є){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}