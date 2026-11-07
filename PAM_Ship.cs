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
 //sensor(s) covering the docking area: no clearance while another ship moves inside
 //camera next to a connector, facing out along its approach: lane must be clear
//[m] how far the lane cameras check in front of their connector
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

//Speed limit near the job area (moving between holes, backing out, arriving and leaving), so ships
//working next to each other don't change direction fast or bump into each other.
const float jobAreaSpeed = 10f;//[m/s]

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ζ{ΕΜ,ű,Ǐ,у,Ϋ}public enum Α{й,ǎ,Ė,ǧ,Ǒ,Ё,Ш,
ȍ,Ķ,Ǥ,Ē,Ď,Ȧ,
Ų,Ǽ,Ǔ,đ,ΑΒ,Ƶ,
ΒЇ,ΑΑ,ē,ƶ,ȫ}public enum Λ{в,ĸ,І,ΒΣ,ư,ƿ,ǀ,Ĳ,ĳ}public enum т{None,đ,й,Δπ}public enum Ϊ{Main,ΔБ,Ű,ů,ΒЃ,Ʈ,
ΒΓ,ΔЀ,ΔЁ,Δώ,Ηφ,Ια,
Ιί,Ιΰ,Βώ,ΕΔ,ǈ}public enum г{ǆ,Ũ,Ηψ,ΔЃ,Ϋ}public enum ǒ{Ǌ,ΖЌ}public enum з{ǅ,ΒΩ,ΕΖ,ū}public enum Ѐ{Βν,Δν,ΒΝ,ΒΤ,Ƴ,Βε,ƺ}public enum я{ΒΟ,ΒΘ,ΒΙ}public enum Й{Ŭ,ΗΨ,ď}public enum ι{Βο,Βσ,Βτ,ΔЎ,ΔЍ,
Βξ,ǃ,ǂ,Βρ,
Βς,Βπ}public enum β{Ƕ,ȏ,ǵ,ƭ,ΒΖ,ΒΗ}public enum ĺ{Δτ,Δγ,Ιή}public enum ф{ǉ,Δς,Ηβ,Βμ}public enum τ{ŷ,Ƭ,ƽ,ū,Βι,ƾ,Βκ,Ηώ,Βυ}class Ψ{public bool ε=false;public Vector3 Δ=new Vector3();public Vector3 γ=new Vector3();public Vector3 δ=new Vector3();public Vector3 υ=new Vector3();
public Vector3 ν=new Vector3();public Vector3 Ъ=new Vector3();public float Њ=0;public float Ιδ=0;public float[]ǡ=null;public Ψ(){}public Ψ(Ψ ΰ){ε=ΰ.ε;Δ=ΰ.Δ;γ=ΰ.γ;δ=ΰ.δ;υ=ΰ.υ;ν=ΰ.ν;Ъ=ΰ.Ъ;ǡ=ΰ.ǡ;}public Ψ(Vector3 Δ,Vector3 δ,Vector3 γ,Vector3 υ,Vector3 ν){this.Δ=Δ;this.γ=γ;
this.δ=δ;this.υ=υ;this.Њ=0;this.ν=ν;}public void ΖЈ(List<IMyThrust>ΑΖ,List<string>ȉ){ǡ=new float[ȉ.Count];for(int i=0;i<ǡ.Length;i++)
ǡ[i]=-1;for(int i=0;i<ΑΖ.Count;i++){string ю=Δό(ΑΖ[i]);int Β=ȉ.IndexOf(ю);if(Β!=-1)
ǡ[Β]=В(ΑΖ[i].MaxEffectiveThrust,ΑΖ[i].MaxThrust);}}}class Βχ{public Ψ Ћ=null;public List<Vector3>Š=new List<Vector3>();public float Ƃ=0;public float ΔΦ=0;public float Ŀ=0;public float Γέ=0;public Vector3 Αά=new Vector3();}class Ļ{public ι Ѝ=ι.Βο;public float ǖ=0;public float Ȳ=0;
public string Ǘ="";public string ǚ="";DateTime ΖΒ;public bool ΓЈ=false;private bool ΓΠ=false;public bool ΒΛ(bool ȇ){if(ȇ){Ȳ=0;ΓΠ=false;return false;}ΓΠ=true;return Ȳ>ǖ;}public void Ηθ(){ΒΛ(true);ΓЈ=false;}public void ΗЊ(){if(ΓΠ)
if((DateTime.Now-ΖΒ).TotalSeconds>1){Ȳ++;ΖΒ=DateTime.Now;}}public bool Ββ(){switch(Ѝ){case ι.Βσ:return true;case ι.Βτ:return true;}return false;}}class Ǧ{public String ψ="";public String σ="";public int ǔ=0;public Й ŏ=Й.ď;public Ǧ(String ψ,String σ,int ǔ,Й ŏ){this.ψ=ψ;this.σ=σ;
this.ǔ=ǔ;this.ŏ=ŏ;}}IMyGridTerminalSystem ϊ;Vector3 Αύ=new Vector3();Vector3 ά=new Vector3();Vector3 Αί=new Vector3();Vector3 Ə=new Vector3();Vector3 ƅ=new Vector3();DateTime ΖΑ=new DateTime();bool Ɖ=true;int ΔΜ=0;bool Ć=false;bool Αη=false;bool Ɣ=true;bool ΖЂ=false;bool Ō=false;
bool Αδ=false;float Ȉ=0;float Ĭ=0;int Ċ=0;int ƥ=0;int ΑЀ=0;int ΓΖ=0;float ő=0;float ŀ=0;double Γό=0;float ź=0;List<int>ŉ=new List<int>();List<int>ś=new List<int>();Ζ Η=Ζ.ΕΜ;int ώ=0;float ѝ=0;float Ǣ=0;float α=0;float ΑЁ=0,ΓА=0;IMyRemoteControl Τ;IMySensorBlock ξ;
List<IMyTimerBlock>ΔΟ=new List<IMyTimerBlock>();List<IMyShipConnector>х=new List<IMyShipConnector>();List<IMyThrust>Ȋ=new List<IMyThrust>();List<IMyGyro>Αΰ=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>ĉ=new List<IMyReactor>();
List<IMyConveyorSorter>ΑІ=new List<IMyConveyorSorter>();List<IMyGasTank>ѕ=new List<IMyGasTank>();List<IMyTerminalBlock>Ÿ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ń=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ĝ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȭ=new List<IMyBatteryBlock>();
List<IMyTextPanel>і=new List<IMyTextPanel>();List<IMyTextSurface>ċ=new List<IMyTextSurface>();List<IMyTextPanel>Ɓ=new List<IMyTextPanel>();IMyTerminalBlock љ=null;Ψ Θ=new Ψ();Ψ Γι=new Ψ();Ψ Γμ=new Ψ();bool ȋ=false;bool ţ=false;bool Ť=false;bool Αζ=false;List<String>ȉ=new List<string>();
List<Ψ>Σ=new List<Ψ>();List<Ψ>ƪ=new List<Ψ>();int ΓΦ=0;double ΖЃ=0;Ψ Ε=new Ψ();Vector3 С;Vector3 ȅ;Λ ί=Λ.в;ǒ Ħ=ǒ.Ǌ;int Е=0;int Э=0;double ƍ=0;bool ΑΡ=false;Ļ ό=new Ļ();Ļ ύ=new Ļ();з о=з.ǅ;bool Č=true;ǒ Ƞ=ǒ.Ǌ;int л=3;int к=3;int Щ=30;я ц=я.ΒΟ;bool ȥ=true;bool ŋ=true;
bool ń=true;bool ĭ=false;Ѐ П=Ѐ.Βν;bool ȣ=true;float Ǳ=90;float щ=20;float Ю=5;float ъ=20;float Ǫ=0.70f;float Ŧ=1.50f;float ť=2.50f;float ı=0f;float ģ=0f;Α Φ;Λ ш;Α Ɵ;Βχ η=null;int Ы=0;int Ь=0;int М=0;int Ȝ=0;int Ī=30;int Х=0;bool Ϗ=true;Vector3 Б;double Н=0;
bool ΑϏ=false;int ΔΫ=0;int ϋ=0;int ΒЎ=0;int ǲ=0;int ΖΦ=0;Vector3 Ɨ=new Vector3();float м=0;float а=0;int İ=0;Ψ ΑЇ=null;float Γο=0;float ΑΘ=0;float У=0;float Ĩ=0;bool Γκ=false;bool Į=false;bool Ʀ=false;bool ș=false;bool Ȓ=false;DateTime ΘΦ=new DateTime();Ψ ğ=null;
Ψ Εζ=null;float ǽ=0;т ǫ;float Ț=0;float į=0;String Γχ="";float Αλ=0;float ƀ=0;float Ƒ=0;List<Ǧ>Ǚ=new List<Ǧ>();float Р=0;bool Ɲ=false;bool Ζψ=false;bool Ζε=false;bool ΑЋ=false;bool Ζζ=false;float Γϊ=0;float ΓІ=0;Vector3 Εω=new Vector3();Vector3 ς=new Vector3();Vector3 ΑЌ=new Vector3(1,1,1);
float ΔΛ=1;Vector3 Ƣ=new Vector3();bool Αε=false;bool Γγ=false;bool Γτ=false;bool ΕΡ=false;float ΑΟ=0;float ΑΔ=2;Vector3 Ζφ;Vector3 ΔΚ;Vector3 ΔΙ;float[,]ǣ=new float[3,2];Dictionary<String,float[,]>ќ=new Dictionary<string,float[,]>();Dictionary<String,float>ΖЀ=new Dictionary<String,float>();
int ΒБ=0;Dictionary<String,float[]>Ȟ=new Dictionary<string,float[]>();float ΓЉ;bool ΑΣ=false;int ΑΗ=0;int ă=0;int Αβ=0;int Ĥ=0;String Κ="";int[]ΖΜ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΝ=false;int Αυ=0;int ǜ=0;Ϊ Џ=Ϊ.Main;int ΙΔ=0,ΙΕ=0,ΙΖ=0,ΙΗ=0;int ΘІ=0,ΘЇ=0;
bool ł=false;bool ΕΣ=false;String ΓБ="";String ΓΕ="";IMyBroadcastListener ȓ=null;bool Εψ=true;String Θι="";Exception ț=null;bool Αώ=false;Λ ŝ=Λ.в;bool Ει=false;bool ΕБ=true;bool ΓЎ=false;String ΖЁ="";bool Ƥ=false;bool ΔΕ=true;int Αγ=0;Vector3 ΑΙ=new Vector3();bool ΕϏ=false;
bool ǐ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βγ(Me,controllerTag,true))
Η=Ζ.у;π(Ϊ.Main);if(Η!=Ζ.у){Κ="Welcome to [PAM]!";ф ΓЍ=Ηι();if(Η==Ζ.ΕΜ){List<IMyShipDrill>Ετ=new List<IMyShipDrill>();List<IMyShipGrinder>ΕЀ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(Ετ,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(ΕЀ,q=>q.CubeGrid==Me.CubeGrid);if(Ετ.Count>0)
{Η=Ζ.ű;Κ="Miner mode enabled!";}else if(ΕЀ.Count>0){Η=Ζ.Ǐ;Κ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;Ǻ(г.Ϋ);}}if(ΓЍ==ф.ǉ) Ɣ=false;if(ΓЍ==ф.Δς) Κ="Data restore failed!";if(ΓЍ==ф.Βμ) Κ="New version, wipe data";}}void Main(string ђ,UpdateType ΙΟ){try{if(ț!=null){if(ђ.Trim().ToUpper()=="RETRY")
{ț=null;Ɖ=true;Ċ=0;if(Η!=Ζ.у) φ();Κ="Restarted after error";}else{ΗΤ();ΔЏ();return;}}Αη=(ΙΟ&UpdateType.Update10)!=0;if(Αη) ΔΜ++;Ć=ΔΜ>=10;if(Ć) ΔΜ=0;if(Αη){ΑΗ++;if(ΑΗ>4) ΑΗ=0;ΑЀ=(int) Math.Max(0,10-(DateTime.Now-ΖΑ).TotalSeconds);}if(ђ!="") Θι=ђ;if(Η!=Ζ.у)
Ηχ(ђ);else
ΗΕ(ђ);ΖΝ=false;try{int Εη=Runtime.CurrentInstructionCount;float ƙ=В(Εη,Runtime.MaxInstructionCount);if(ƙ>0.90) Κ="Max. instructions >90%";if(ƙ>ő) ő=ƙ;if(ΑΣ){ŉ.Add(Εη);while(ŉ.Count>10) ŉ.RemoveAt(0);ŀ=0;for(int i=0;i<ŉ.Count;i++) ŀ+=ŉ[i];ŀ=В(В(ŀ,ŉ.Count),Runtime.MaxInstructionCount);
double Γπ=Runtime.LastRunTimeMs;if(Αδ&&Γπ>Γό) Γό=Γπ;ś.Add((int)(Γπ*1000f));while(ś.Count>10) ś.RemoveAt(0);ź=0;for(int i=0;i<ś.Count;i++) ź+=ś[i];ź=В(ź,ś.Count)/1000f;}}catch{ő=0;}}catch(Exception e){ț=e;}}void Ηχ(string ђ){bool ΑЂ=false;String Εί="";if(ώ<=1&&!ΔІ(ђ))
ΔА(ђ);int Θσ=50;while(ȓ!=null&&ȓ.HasPendingMessage&&Θσ-->0){MyIGCMessage ρ=ȓ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String Ɯ="";if(ώ<=1&&Ηγ(ref Χ,out Ɯ,out Εί)&&Ɯ==BROADCAST_SELF_ID){ΔА(Χ);ΑЂ=true;break;}}bool ΕЌ=Αδ&&Φ==Α.й&&!Ō&&!ΑЂ&&Ċ==0&&!Αζ;
if(Ć&&Φ!=Α.й) ΑЂ=true;if((Αη&&!ΕЌ)||(Ć&&ΕЌ)){if(Ċ==0&&(ΑЀ<=0||Ɖ)){ΖЂ=ώ>0;ώ=0;Ċ=1;ǻ();Ην();ΗΩ();Ƿ("Scan 1");}else if(Ċ==1){Ċ=2;ǻ();Ηξ();Ƿ("Scan 2");}else if(Ċ==2){Ċ=0;ǻ();Ηο();Ηπ();if(ώ<=1) ΔЉ();Ƿ("Scan 3");ΖΑ=DateTime.Now;if(ώ<=1&&Ɣ)
Αύ=Ƹ(Τ,Τ.CenterOfMass);Ɣ=false;if(Ɖ){Ɖ=false;bool ΙΥ=ί==Λ.І;φ();if(ΕБ&&resumeAfterRestart&&ΙΥ&&ώ<=1) ΓЎ=true;}ΕБ=false;if(ΖЂ&&ώ==0) Κ="Setup complete";}else{if(ί==Λ.І&&Η!=Ζ.Ϋ){ǻ();ΖЅ();Ƿ("Inv balance");}ǻ();switch(ƥ){case 0:ΗЂ();break;case 1:ΗІ();break;case 2:ΗЁ();break;
case 3:ΗЍ();break;case 4:ΗЅ();break;case 5:ΖЍ();break;case 6:ΖЊ(Τ);break;}Ƿ("Update: "+ƥ);ƥ++;if(ƥ>6){ƥ=0;Αδ=true;if(ŝ!=Λ.в){switch(ŝ){case Λ.Ĳ:Ȩ();break;case Λ.ĳ:Ȩ();break;case Λ.І:Ȩ();break;case Λ.ƿ:Ʒ();break;case Λ.ǀ:Ĕ();break;}ŝ=Λ.в;}}}if(!Ɖ){if(!ǹ(Τ,true)){Τ=null;
Ɣ=true;ώ=2;}if(ώ>=2&&Φ!=Α.й)
φ();if(ώ<=1){Ĭ=Τ.CalculateShipMass().PhysicalMass;Ȉ=(float) Τ.GetShipSpeed();ά=ΗЀ(Τ,Αύ);Αί=Τ.WorldMatrix.Forward;Ə=Τ.WorldMatrix.Left;ƅ=Τ.WorldMatrix.Down;ΗЉ();ΗЋ();ΗЃ();if(ΓЎ){ΓЎ=false;Ȩ();if(Φ!=Α.й) Κ="Job resumed after restart";}if(Φ!=Α.й){Ō=false;ΕΓ(false);Δη(false);String ΘЎ=Βή(Φ)+" "+(int) Φ;
ǻ();ΗЇ();ΒІ(false);Ƿ(ΘЎ);ΗЌ();ΗЈ();ǻ();ΗЄ();Ƿ("Thruster");ǻ();ΕΝ();Ƿ("Gyroscope");}else{if(Ō){if(Ƽ()){Ч(ƅ,Αί,Ə,0.25f,true);ΕΝ();Κ="Aligning to planet: "+Math.Round(ΑΟ-0.25f,2)+"°";if(Αε) Βα(true,true);}else Βα(true,true);}}Ƥ=false;}}}ǻ();ΔЏ();Ƿ("Print");if(ΑЂ||ΓΖ<=0){ǻ();ΖІ(Εί);
Ƿ("Broadcast");ΓΖ=4;}else if(Ć) ΓΖ--;}void ΔА(string ђ){if(ђ=="") return;var З=ђ.ToUpper().Split(' ');З.DefaultIfEmpty("");var ΓΜ=З.ElementAtOrDefault(0);var ĝ=З.ElementAtOrDefault(1);var Ź=З.ElementAtOrDefault(2);var ΕΦ=З.ElementAtOrDefault(3);String Ą="Invalid argument: "+ђ;bool ΔΥ=false;
switch(ΓΜ){case "UP":this.ΔЋ(false);break;case "DOWN":this.ΔЊ(false);break;case "UPLOOP":this.ΔЋ(true);break;case "DOWNLOOP":this.ΔЊ(true);break;case "APPLY":this.Δε(true);break;case "MRES":ǜ=0;break;case "STOP":this.φ();break;case "PATHHOME":{this.φ();this.ΒЂ();}break;case "PATH":{this.φ();this.ΒЂ();Θ.ε=true;}break;
case "START":{this.φ();ΒЁ();}break;case "ALIGN":{Βα(!Ō,false);}break;case "CONT":{this.φ();this.Ȩ();}break;case "JOBPOS":{this.φ();this.Ĕ();}break;case "HOMEPOS":{this.φ();this.Ʒ();}break;case "FULL":{Ɲ=true;}break;case "RESET":{Αώ=true;ώ=2;}break;case "EXPORT":{ΗΛ();}break;
case "IMPORT":{ΗΦ(ĝ=="JOB");}break;default:ΔΥ=true;break;}if(Η!=Ζ.Ϋ){switch(ΓΜ){case "SHUTTLE":{ΗΙ();}break;case "CFGS":{if(!ΗΓ(ĝ,Ź,ΕΦ)) Κ=Ą;}break;case "CFGB":{if(!ΗΒ(ĝ,Ź)) Κ=Ą;}break;case "CFGL":{if(!ė(ref Ǳ,true,τ.ƽ,ĝ,"")||!ΗΔ(Ź))
Κ=Ą;}break;case "CFGE":{if(!ė(ref Ю,true,τ.ƾ,ĝ,"IG")||
!ė(ref щ,true,τ.Βι,Ź,"IG")||
!ė(ref ъ,true,τ.Βκ,ΕΦ,"IG"))
Κ=Ą;}break;case "CFGA":{if(!ė(ref Ǫ,false,τ.Ƭ,ĝ,"")) Κ=Ą;}break;case "CFGW":{if(!ė(ref Ŧ,false,τ.ŷ,ĝ,"")||
!ė(ref ť,false,τ.ŷ,Ź,""))
Κ=Ą;}break;case "NEXT":{ΒΒ(false);}break;case "PREV":{ΒΒ(true);}break;default:if(ΔΥ) Κ=Ą;break;}}else{switch(ΓΜ){case "UNDOCK":{Ƥ=true;}break;default:if(ΔΥ) Κ=Ą;break;}}}String ΗΟ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΙ(){φ();Η=Ζ.Ϋ;Ǻ(г.Ϋ);Ε.ε=false;Θ.ε=false;ξ=null;ο.Clear();ί=Λ.в;}void Βα(bool Ј,bool є){if(!Ј) Κ="Aligning canceled";if(є) Κ="Aligning done";if(є||!Ј){Ō=false;ΕΗ();Ǉ(false,0,0,0,0);return;}if(Ƽ()) Ō=true;}bool ΗΓ(String ΙΧ,String Θα,String ΘΟ)
{bool ΙΣ=ί==Λ.І;int w,h,d;if(int.TryParse(ΙΧ,out w)&&int.TryParse(Θα,out h)&&int.TryParse(ΘΟ,out d)){this.φ();л=w;к=h;Щ=d;Ě(false);Ŷ(false,false);if(ΙΣ) Ȩ();return true;}return false;}bool ΗΔ(String ΕΥ){if(ΕΥ=="ON"){ȥ=true;return true;}if(ΕΥ=="OFF"){ȥ=false;return true;}
return false;}bool ΗΒ(String Εσ,String ΑΠ){bool ƕ=true;if(Εσ=="HOME") Č=true;else if(Εσ=="STOP") Č=false;else ƕ=false;if(ΑΠ=="HOME") о=з.ǅ;else if(ΑΠ=="STOP") о=з.ΕΖ;else if(ΑΠ=="JOB") о=з.ΒΩ;else if(ΑΠ=="IG") о=з.ū;else ƕ=false;return ƕ;}void π(Ϊ ΓЄ){ΖΜ[(int) Џ]=ǜ;
ǜ=ΖΜ[(int) ΓЄ];if(ΓЄ==Ϊ.Ηφ) ǜ=0;Џ=ΓЄ;if(Η!=Ζ.у)
ę(Џ==Ϊ.Ű,false,0,0);ΖΝ=true;}void ΔЋ(bool Γυ){if(ǜ>0) ǜ--;else if(Γυ) ǜ=Αυ-1;}void ΔЊ(bool Γυ){if(ǜ<Αυ-1) ǜ++;else if(Γυ) ǜ=0;}String Ǻ(г ρ){switch(ρ){case г.ǆ:Κ="Job is running";break;case г.Ũ:Κ="Connector not ready!";break;case г.Ηψ:Κ="Ship modified, path outdated!";break;
case г.ΔЃ:Κ="Interrupted by player!";break;case г.Ϋ:Κ="Shuttle mode enabled!";break;}return "";}String ΗΣ(ǒ λ){switch(λ){case ǒ.Ǌ:return "Top-Left";case ǒ.ΖЌ:return "Center";default:return "";}}String ΗΝ(я Ɠ){switch(Ɠ){case я.ΒΘ:return "Auto"+(Η==Ζ.ű?" (Ore)":"");case я.ΒΙ:return "Auto (+Stone)";
case я.ΒΟ:return "Default";default:return "";}}String Βά(Λ Ц){switch(Ц){case Λ.в:return "No job";case Λ.ĸ:return "Job paused";case Λ.І:return "Job active";case Λ.Ĳ:return "Job active";case Λ.ĳ:return "Job active";case Λ.ΒΣ:return "Job done";case Λ.ư:return "Job changed";case Λ.ƿ:return "Move home";
case Λ.ǀ:return "Move to job";}return "";}String Δχ(з ΘΖ){switch(ΘΖ){case з.ǅ:return "Return home";case з.ΒΩ:return "Fly to job pos";case з.ΕΖ:return "Stop";case з.ū:return "Ignore";}return "";}String ΗΞ(Ѐ Ɠ){switch(Ɠ){case Ѐ.Βν:return "Off";case Ѐ.ΒΤ:return "Drop pos (Stone) ";case Ѐ.Ƴ:return "Drop pos (Sto.+Ice)";
case Ѐ.Δν:return "Cur. pos (Stone)";case Ѐ.ΒΝ:return "Cur. pos (Sto.+Ice)";case Ѐ.Βε:return "In motion (Stone)";case Ѐ.ƺ:return "In motion (Sto.+Ice)";}return "";}String Δυ(т Ц){switch(Ц){case т.None:return "No batteries";case т.đ:return "Charging";case т.Δπ:return "Discharging";}return "";}String Βή(Α Ц)
{String Ιβ=Η==Ζ.Ϋ?"target":"job";switch(Ц){case Α.й:return "Idle";case Α.ǎ:return "Flying to XY position";case Α.Ė:return Η==Ζ.Ǐ?"Grinding":"Mining";case Α.ǧ:return "Returning";case Α.ē:return "Flying to drop pos";case Α.Ǒ:return "Returning to dock";case Α.Ё:return "Flying to dock area";
case Α.Ш:return "Flying to job area";case Α.ȍ:return "Flying to path";case Α.Ķ:return "Flying to job position";case Α.Ǥ:return "Approaching dock";case Α.Ē:return "Docking";case Α.Ď:return "Aligning";case Α.Ȧ:return "Aligning";case Α.Ų:return "Retry docking";case Α.Ǽ:return "Unloading";case Α.ȫ:return ΖЁ;
case Α.Ǔ:return "Undocking";case Α.đ:return "Charging batteries";case Α.ΑΒ:return "Waiting for uranium";case Α.Ƶ:return "Filling up hydrogen";case Α.ΒЇ:return "Waiting for ejection";case Α.ΑΑ:return "Waiting for ejection";case Α.ƶ:return "Flying to drop pos";}return "";}String Δύ(ι Ѝ){switch(Ѝ)
{case ι.Βο:return "On \"Undock\" command";case ι.Βσ:return "On player entered cockpit";case ι.ΔЎ:return "On ship is full";case ι.ΔЍ:return "On ship is empty";case ι.Βτ:return "On time delay";case ι.ǃ:return "On batteries empty(<25%)";case ι.ǂ:return "On batteries empty(=0%)";case ι.Βξ:return "On batteries full";
case ι.Βς:return "On hydrogen empty(<25%)";case ι.Βπ:return "On hydrogen empty(=0%)";case ι.Βρ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int Ζΰ,int Ă,bool Ξ,String Ǝ){Αυ+=1;if(Ζΰ==Ă) Ǝ=">"+Ǝ+(ΑΗ>=2?" .":"");else Ǝ=" "+Ǝ;Γ+=Ǝ+"\n";return Ζΰ==Ă&&Ξ;}String Δε(bool Ξ)
{int Β=0;int Ο=ǜ;Αυ=0;String Ζγ="———————————————\n";String Ţ="--------------------------------------------\n";String Γ="";Γ+=Βά(ί)+" | "+(Θ.ε?"Ready to dock":"No dock")+"\n";Γ+=Ζγ;double Ǡ=Math.Max(Math.Round(this.Н),0);if(Џ==Ϊ.Main){bool ą=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) ΒЂ();
if(Η==Ζ.ű)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.Ű);if(Η==Ζ.Ǐ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.Ű);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.Βώ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) Ȩ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) Ʒ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ĕ();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(ą) π(Ϊ.ǈ);else π(Ϊ.Ʈ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.ΔЀ);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δώ);}else if(Џ==Ϊ.Ű){double ΙΦ=Math.Round(л*ѝ,1);double Θΰ=Math.Round(к*Ǣ,1);String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Start new job!")) ΒЁ();if(Π(ref Ѕ,Ο,Β++,Ξ," Change current job")){Ŷ(false,false);π(Ϊ.Main);}if(Π(ref Ѕ,Ο,Β++,Ξ," Width + (Width: "+л+" = "+ΙΦ+"m)")){Ĵ(ref л,5,20,1);Ě(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Width -")){Ĵ(ref л,-5,20,-1);Ě(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height + (Height: "+к+" = "+Θΰ+"m)")){Ĵ(ref к,5,20,1);Ě(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Height -")){Ĵ(ref к,-5,20,-1);Ě(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth + ("+(ц==я.ΒΟ?"Depth":"Min")+": "+Щ+"m)")){Ĵ(ref Щ,5,50,2);Ě(true);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Depth -")){Ĵ(ref Щ,-5,50,-2);Ě(true);}if(Π(ref Ѕ,Ο,Β++,Ξ," Depth mode: "+ΗΝ(ц))){ц=ȩ(ц);}if(Π(ref Ѕ,Ο,Β++,Ξ," Start pos: "+ΗΣ(Ƞ))){Ƞ=ȩ(Ƞ);}if(Η==Ζ.Ǐ&&ц==я.ΒΙ) ц=ȩ(ц);Γ+=ΕΑ(8,Ѕ,Ο,ref ΘІ);}else if(Џ==Ϊ.Βώ){float[]Ελ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΕΔ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δύ(ό.Ѝ))) ό.Ѝ=ȩ(ό.Ѝ);if(!ό.Ββ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΫ((int) ό.ǖ))) ό.ǖ=Δξ(ό.ǖ,Ελ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δύ(ύ.Ѝ))) ύ.Ѝ=ȩ(ύ.Ѝ);
if(!ύ.Ββ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΫ((int) ύ.ǖ))) ύ.ǖ=Δξ(ύ.ǖ,Ελ);}else if(Џ==Ϊ.ΕΔ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) ΒЁ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Βώ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǘ!=""?ό.Ǘ:"-"))) ό.Ǘ=ǁ(ref ΙΔ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǚ!=""?ό.ǚ:"-"))) ό.ǚ=ǁ(ref ΙΖ);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.Ǘ!=""?ύ.Ǘ:"-"))) ύ.Ǘ=ǁ(ref ΙΕ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ύ.ǚ!=""?ύ.ǚ:"-"))) ύ.ǚ=ǁ(ref ΙΗ);
}else if(Џ==Ϊ.ů){String ΖΖ=Ǳ+" %";if(Ć) ă++;if(ă>1){ă=0;Αβ++;if(Αβ>1) Αβ=0;}bool[]ΔΒ=new bool[]{ĉ.Count==0,ǫ==т.None,ѕ.Count==0};int Ζύ=0;while(true){Ζύ++;Ĥ++;if(Ĥ>ΔΒ.Length-1) Ĥ=0;if(Ζύ>=ΔΒ.Length) break;if(!ΔΒ[Ĥ]) break;}bool ą=Η==Ζ.Ϋ;if(!ą&&ȥ&&Р!=-1&&Αβ==0)
ΖΖ=Р<1000000?Math.Round(Р)+" Kg":Math.Round(Р/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){φ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!ą) π(Ϊ.Ʈ);else π(Ϊ.ǈ);if(!ą){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΒΒ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) Ƥ=true;Γ+=Ţ;if(!ą) Γ+="Progress: "+Math.Round(ƍ,1)+" %\n";Γ+="State: "+Βή(Φ)+" "+Ǡ+"m \n";Γ+="Load: "+Αλ+" % Max: "+ΖΖ+" \n";if(Ĥ==0) Γ+="Uranium: "+(ĉ.Count==0?"No reactors":Math.Round(į,1)+"Kg "+(Ю==-1?"":" Min: "+Ю+" Kg"))+"\n";
if(Ĥ==1) Γ+="Battery: "+(ǫ==т.None?Δυ(ǫ):ǽ+"% "+(щ==-1||ą?"":" Min: "+щ+" %"))+"\n";if(Ĥ==2) Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(Ț,1)+"% "+(ъ==-1||ą?"":" Min: "+ъ+" %"))+"\n";}else if(Џ==Ϊ.Ʈ){String Ѕ="";if(Π(ref Ѕ,Ο,Β++,Ξ," Back")){if(ί==Λ.І) π(Ϊ.ů);else π(Ϊ.Main);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Max load: "+Ǳ+"%")) ж(ref Ǳ,Ǳ<=80?-10:-5,τ.ƽ,false);if(Π(ref Ѕ,Ο,Β++,Ξ," Weight limit: "+(ȥ?"On":"Off"))) ȥ=!ȥ;if(Π(ref Ѕ,Ο,Β++,Ξ," Ejection: "+ΗΞ(П))) П=ȩ(П);if(Π(ref Ѕ,Ο,Β++,Ξ," Toggle sorters: "+(ĭ?"On":"Off"))){ĭ=!ĭ;if(ĭ) ǩ(ΔΕ);}
if(Π(ref Ѕ,Ο,Β++,Ξ," Unload ice: "+(ȣ?"On":"Off"))) ȣ=!ȣ;if(Π(ref Ѕ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) ж(ref Ю,(Ю>5?-5:-1),τ.ƾ,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Battery: "+(щ==-1?"Ignore":"Min "+щ+"%"))) ж(ref щ,-5,τ.Βι,true);if(Π(ref Ѕ,Ο,Β++,Ξ," Hydrogen: "+(ъ==-1?"Ignore":"Min "+ъ+"%"))) ж(ref ъ,-10,τ.Βκ,true);
if(Π(ref Ѕ,Ο,Β++,Ξ," When done: "+(Č?"Return home":"Stop"))) Č=!Č;if(Π(ref Ѕ,Ο,Β++,Ξ," On damage: "+Δχ(о))) о=ȩ(о);if(Π(ref Ѕ,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΒΓ);Γ+=ΕΑ(8,Ѕ,Ο,ref ΘЇ);}else if(Џ==Ϊ.ΒΓ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.І) π(Ϊ.ů);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǐ?" Grinder":" Drill")+" inv. balancing: "+(ŋ?"On":"Off"))) ŋ=!ŋ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǐ?" grinders":" drills")+": "+(ń?"Fwd + Bwd":"Fwd"))) ń=!ń;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+Ŧ+"m/s")) ж(ref Ŧ,0.5f,τ.ŷ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+ť+"m/s")) ж(ref ť,0.5f,τ.ŷ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǫ*100f)+"%"+(Ǫ>0.80f?" (risky)":""))) ж(ref Ǫ,0.1f,τ.Ƭ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ı*100f+"%")) Δι(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ģ*100f+"%")) Δι(false,0.05f);
}else if(Џ==Ϊ.ǈ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.І) π(Ϊ.ů);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǳ+"%")) ж(ref Ǳ,Ǳ<=80?-10:-5,τ.ƽ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(ȣ?"On":"Off"))) ȣ=!ȣ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ю==-1?"Ignore":"Min "+Ю+"Kg"))) ж(ref Ю,(Ю>5?-5:-1),τ.ƾ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(щ==-1?"Ignore":"Charge up"))) щ=(щ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(ъ==-1?"Ignore":"Fill up"))) ъ=(ъ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δχ(о))) о=ȩ(о);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(Ǫ*100f)+"%"+(Ǫ>0.80f?" (risky)":""))) ж(ref Ǫ,0.1f,τ.Ƭ,false);
}else if(Џ==Ϊ.ΔБ){double Θκ=0;if(Σ.Count>0) Θκ=Vector3.Distance(Σ.Last().Δ,ά);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΘ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(ȋ?"Use old home":(Θ.ε?"Was set! ":"none ")))) ȋ=!ȋ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(ȋ?"Use old connector":(Θ.ε?"Was set! ":"none ")))) ȋ=!ȋ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(ţ?"Use old connector":(Ε.ε?"Was set! ":"none ")))) ţ=!ţ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(Ť?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) Ť=!Ť;Γ+=Ţ;Γ+="Wp spacing: "+Math.Round(ΖЃ)+"m\n";}else if(Џ==Ϊ.ΒЃ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){φ();π(Ϊ.Main);}
Γ+=Ţ;Γ+="State: "+Βή(Φ)+" \n";Γ+="Speed: "+Math.Round(Ȉ,1)+"m/s\n";Γ+="Target dist: "+Ǡ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΫ+"\n";}else if(Џ==Ϊ.ΔЀ){List<IMyTerminalBlock>Ġ=Δψ();if(Ć) ă++;if(ă>=Ġ.Count) ă=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔЁ);
Γ+=Ţ;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αλ,1)+"% "+Math.Round(ƀ,1)+" / "+Math.Round(Ƒ,1)+"\n";Γ+="Uranium: "+(ĉ.Count==0?"No reactors":Math.Round(į,1)+"Kg "+Γχ)+"\n";Γ+="Battery: "+(ǫ==т.None?"":ǽ+"% ")+Δυ(ǫ)+"\n";Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(Ț,1)+"% ")+"\n";
Γ+="Damage: "+(Ġ.Count==0?"None":""+(ă+1)+"/"+Ġ.Count+" "+Ġ[ă].CustomName)+"\n";}else if(Џ==Ϊ.ΔЁ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ţ;Γ+="Next scan: "+ΑЀ+"s\n";Γ+="Drill area: "+Math.Round(ѝ,1)+"x"+Math.Round(Ǣ,1)+"m Ship: "+Math.Round(ΑЁ,1)+"x"+Math.Round(ΓА,1)+"m ("+Math.Round(α,1)+"m)\n"+(Ǭ.Count>0?"Cameras: "+ΓΘ+"\n":"");
Γ+="Broadcast: "+(ł?"Online - "+ΓБ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(ő*100f,1)+"% \n";}else if(Џ==Ϊ.Δώ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=Ţ;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ώ==2) Γ="Fatal setup error\nNext scan: "+ΑЀ+"s\n";if(Αώ) Γ="Recompile script now";int Θλ=Γ.Split('\n').Length;for(int i=Θλ;i<=10;i++) Γ+="\n";Γ+=Ζγ;Γ+="Last: "+Κ+"\n";
return Γ;}void Δι(bool Θη,float ћ){φ();Ŷ(true,false);if(Θη) ж(ref ı,ћ,τ.Βυ,false);else ж(ref ģ,ћ,τ.Βυ,false);Δθ();ę(true,true,0,0);}float Δξ(float θ,float[]ΒΑ){float ή=ΒΑ[0];for(int i=ΒΑ.Length-1;i>=0;i--)
if(θ<ΒΑ[i]) ή=ΒΑ[i];return ή;}String ǁ(ref int Ă){String ή="";if(Ă>=ΔΟ.Count) Ă=-1;if(Ă>=0) ή=ΔΟ[Ă].CustomName;Ă++;return ή;}void ǋ(string ψ){if(ί!=Λ.І) return;if(ψ=="") return;IMyTerminalBlock Υ=ϊ.GetBlockWithName(ψ);if(Υ==null||!(Υ is IMyTimerBlock)){Κ="Timerblock "+ψ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void Ĵ(ref int value,int ż,int ȡ,int ΔΓ){if(ż==0) return;if(value<ȡ&&ΔΓ>0||value<=ȡ&&ΔΓ<0){value+=ΔΓ;return;}int ΗЏ=Math.Abs(ż);int ΒЈ=0;int Γώ=1;while(true){ΒЈ+=Γώ*ΗЏ*10;if(ż<0&&value-ȡ<=ΒЈ) break;if(ż>0&&value-ȡ<ΒЈ) break;
Γώ++;}value+=Γώ*ż;}void Ě(bool Ιε){л=Math.Max(л,1);к=Math.Max(к,1);Щ=Math.Max(Щ,0);ę(Џ==Ϊ.Ű,false,0,0);}bool ė(ref float value,bool ΘЂ,τ σ,String ĥ,String Θδ){if(ĥ=="") return false;float Ŗ=-1;bool ΕЉ=false;if(ĥ.ToUpper()==Θδ) ΕЉ=true;else if(!float.TryParse(ĥ,out Ŗ)) return false;
else Ŗ=Math.Max(0,Ŗ);if(ΘЂ) Ŗ=(float) Math.Round(Ŗ);ж(ref value,Ŗ,σ,ΕЉ,false);return true;}void ж(ref float value,float ћ,τ σ,bool ƣ){ж(ref value,value+ћ,σ,ƣ,true);}void ж(ref float value,float Œ,τ σ,bool ƣ,bool ΙΪ){float ǝ=0,Ǜ=0;if(σ==τ.ŷ){ǝ=0.5f;Ǜ=10f;}if(σ==τ.Ƭ){ǝ=0.1f;Ǜ=1f;}
if(σ==τ.ū){ǝ=50f;Ǜ=100f;}if(σ==τ.Βι){ǝ=5f;Ǜ=30f;}if(σ==τ.ƾ){ǝ=1f;Ǜ=25f;}if(σ==τ.ƽ){ǝ=10f;Ǜ=95f;}if(σ==τ.Βκ){ǝ=10f;Ǜ=90f;}if(σ==τ.Ηώ){ǝ=10f;Ǜ=1800;}if(σ==τ.Βυ){ǝ=0.0f;Ǜ=0.75f;}if(Œ==-1&&ƣ){value=-1;return;}if(value==-1) ƣ=false;
bool ΖΨ=Œ<ǝ||Œ>Ǜ;if(ΖΨ&&ΙΪ){if(Œ<value) value=Ǜ;else if(Œ>value) value=ǝ;}else value=Œ;if(ΖΨ&&ƣ) value=-1;else value=Math.Max(ǝ,Math.Min(value,Ǜ));value=(float) Math.Round(value,2);}void ΒΒ(bool Θώ){if(Θώ) М=Math.Max(0,М-1);else М++;ΒΦ();ΒΥ();for(int k=0;k<ȃ.Count&&k<О.Length;k++) Βό(ȃ[k],k<М);
Ѓ=-1;ΒІ(true);}T ȩ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΚ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΚ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒЂ(){ƪ.Clear();for(int i=0;i<Σ.Count;i++) ƪ.Add(Σ[i]);Σ.Clear();
Αζ=true;Γι=new Ψ(Θ);Γμ=new Ψ(Ε);Θ.ε=false;if(Η==Ζ.Ϋ) Ε.ε=false;for(int i=0;i<ќ.Count;i++)
if(!ȉ.Contains(ќ.Keys.ElementAt(i)))
ȉ.Add(ќ.Keys.ElementAt(i));ȋ=false;ţ=false;Ť=false;π(Ϊ.ΔБ);}void ΕΘ(){if(ȋ) Θ=Γι;if(ţ) Ε=Γμ;if(Ť){Σ.Clear();for(int i=0;i<ƪ.Count;i++) Σ.Add(ƪ[i]);}Αζ=false;φ();π(Ϊ.Main);}void ΗЉ(){if(!Αζ) return;if(Φ!=Α.й){ΕΘ();return;}if(!Γι.ε) ȋ=false;if(!Γμ.ε) ţ=false;if(ƪ.Count<=1) Ť=false;
IMyShipConnector Л=Т(MyShipConnectorStatus.Connectable);if(Л==null) Л=Т(MyShipConnectorStatus.Connected);if(Л!=null){if(Math.Round(Ȉ,2)<=0.20) ΓΦ++;else ΓΦ=0;if(ΓΦ>=5){if(Η==Ζ.Ϋ&&(Θ.ε||ȋ)&&Vector3.Distance(Θ.Δ,Л.GetPosition())>5){Ε.δ=Τ.WorldMatrix.Forward;Ε.υ=Τ.WorldMatrix.Left;
Ε.γ=Τ.WorldMatrix.Down;Ε.ν=Τ.GetNaturalGravity();Ε.Δ=Л.GetPosition();Ε.ε=true;Ε.Ъ=Л.Position;}else{Θ.δ=Τ.WorldMatrix.Forward;Θ.υ=Τ.WorldMatrix.Left;Θ.γ=Τ.WorldMatrix.Down;Θ.ν=Τ.GetNaturalGravity();Θ.Δ=Л.GetPosition();Θ.ε=true;Θ.Ъ=Л.Position;}}}double Γξ=-1;if(Σ.Count>0) Γξ=Vector3.Distance(ά,Σ.Last().Δ);
double ΘЌ=Math.Max(1.5,Math.Pow(Ȉ/100.0,2));double ΖΞ=Math.Max(Ȉ*ΘЌ,2);ΖЃ=ΖΞ;if((Γξ==-1)||Γξ>=ΖΞ){Ψ Ω=new Ψ(ά,Αί,ƅ,Ə,Τ.GetNaturalGravity());Ω.ΖЈ(Ȋ,ȉ);Σ.Add(Ω);}}Ψ и(){if(Η!=Ζ.Ϋ) return ј!=null?ј:Θ;return Εζ;}void Ĺ(Ψ έ,Λ Ц){Εζ=έ;if(ί==Λ.І) ш=Ц;}bool Ǹ(Ψ έ,float Ђ,bool ΙΠ,out Vector3 λ)
{if(ΙΠ){Vector3I Γβ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(Γβ);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){λ=new Vector3();return false;}Vector3 ΘΛ=б(Τ,Υ.FatBlock.GetPosition()-ά);Vector3 ΘΚ=б(Τ,Υ.FatBlock.WorldMatrix.Forward);λ=έ.Δ-ΒЅ(έ.δ,έ.γ*-1,ΘΛ)-ΒЅ(έ.δ,έ.γ*-1,ΘΚ)*Ђ;
return true;}else{λ=έ.Δ;return true;}}Vector3 Ȏ(int ΘΘ,int Ś,bool ΘЀ){if(!ΘЀ&&ΕϏ) return ΑΙ;float ΘΙ=((Е-1f)/2f)-ΘΘ;float ΘЃ=((Э-1f)/2f)-Ś;ΑΙ=Ε.Δ+Ε.υ*ΘΙ*ѝ+ȅ*-1*ΘЃ*Ǣ;ΕϏ=true;return ΑΙ;}Vector3 Βέ(Vector3 ΘА,float ΓΣ){return ΘА+(С*ΓΣ);}β ΒΠ(){float Ў=-1;β ή=β.Ƕ;
if(Η!=Ζ.Ϋ){ΔЉ();if(ί!=Λ.в){Vector3 Љ=Ę(С,ȅ*-1,ά-Ε.Δ);if(Math.Abs(Љ.X)<=(float)(Е*ѝ)/2f&&Math.Abs(Љ.Y)<=(float)(Э*Ǣ)/2f){if(Љ.Z<=-1&&Љ.Z>=-Ī*2) return β.ƭ;if(Љ.Z>-1&&Љ.Z<α*2) return β.Ƕ;}if(ȧ(Ε.Δ,ref Ў)) ή=β.Ƕ;}if(Θ.ε){if(ȧ(Θ.Δ,ref Ў)) ή=β.ǵ;for(int i=0;i<Σ.Count;i++)
if(ȧ(Σ[i].Δ,ref Ў)) ή=β.ȏ;if(Vector3.Distance(ά,Θ.Δ)<dockDist*α) ή=β.ǵ;if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null)
ή=β.ǵ;}}else{Vector3 λ=new Vector3();IMyShipConnector Ȁ=Т(MyShipConnectorStatus.Connected);if(Θ.ε){if(ȧ(Θ.Δ,ref Ў)) ή=β.ǵ;if(Ǹ(Θ,dockDist,true,out λ))
if(ȧ(λ,ref Ў)) ή=β.ǵ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return β.ΒΖ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*α&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*α)
if(ȧ(Σ[i].Δ,ref Ў)) ή=β.ȏ;if(Ε.ε){if(ȧ(Ε.Δ,ref Ў)) ή=β.Ƕ;if(Ǹ(Ε,dockDist,true,out λ))
if(ȧ(λ,ref Ў)) ή=β.Ƕ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return β.ΒΗ;}}return ή;}bool ȧ(Vector3 λ,ref float Ў){float Ђ=Vector3.Distance(λ,ά);if(Ђ<Ў||Ў==-1){Ў=Ђ;return true;}return false;}void ΒЁ(){if(ώ>0){Κ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){Ȩ();return;}Ε.Δ=ά;Ε.ν=Τ.GetNaturalGravity();Ε.δ=Αί;Ε.γ=ƅ;Ε.υ=Ə;
С=љ.WorldMatrix.Forward;ȅ=Ε.γ;if(С==Τ.WorldMatrix.Down) ȅ=Τ.WorldMatrix.Backward;Ŷ(true,true);Ν(Α.ǎ);ΒЀ();}void Ŷ(bool ǘ,bool ΘЁ){if(ί==Λ.в&&!ǘ) return;bool Θυ=ǘ||ί==Λ.ΒΣ||Е!=л||Э!=к||Ħ!=Ƞ;if(Θυ){if(ί!=Λ.в){ί=Λ.ư;Ȏ(Ы,Ь,ΘЁ);Κ="Job changed, lost progress";}Ħ=Ƞ;
Е=л;Э=к;Ь=0;Ы=0;О="";Ѓ=-1;Ĩ=0;Ȝ=0;У=0;М=0;ΒІ(true);}}void Βψ(){К(ά,0);ё(Ȋ,true);}void ΒЀ(){Ǻ(г.ǆ);Βψ();Ŵ(Ȅ,false);ǩ(ΔΕ);ί=Λ.І;Δη(true);ш=ί;π(Ϊ.ů);Ημ();ΑΡ=true;Αγ=0;for(int i=Ÿ.Count-1;i>=0;i--)
if(ŭ(Ÿ[i],false)) Αγ++;if(Αγ>0) Κ="Started with damage";}void φ(){if(ί==Λ.І){ί=Λ.ĸ;Κ="Job paused";}Ν(Α.й);ш=ί;Ǉ(false,0,0,0,0);ΒЄ();Δδ(new Vector3(),false);ΕΗ();Βϋ(ChargeMode.Auto);Βύ(false);ΕΓ(true);Ȑ(Α.й);ę(false,false,0,0);ё(ο,false);ё(х,true);ǩ(true);ΑϏ=false;Ʋ();Ƅ=false;
Βω();ΑΡ=false;Ɲ=false;Ƥ=false;if(Џ!=Ϊ.Main&&Џ!=Ϊ.Ʈ&&Џ!=Ϊ.ΒΓ&&Џ!=Ϊ.ǈ)
π(Ϊ.Main);}void Ȩ(){β Ж=ΒΠ();if(Η==Ζ.Ϋ){if(!Ε.ε||!Θ.ε) return;ΒЀ();bool Γΰ=Vector3.Distance(ά,Θ.Δ)<Vector3.Distance(ά,Ε.Δ);if(ŝ==Λ.Ĳ) Γΰ=true;if(ŝ==Λ.ĳ) Γΰ=false;if(Γΰ){Ĺ(Θ,Λ.Ĳ);switch(Ж){case β.ΒΖ:Ν(Α.ȫ);break;case β.ȏ:Ν(Α.Ш);break;case β.ǵ:Ν(Α.Ǥ);break;default:Ν(Α.Ǔ);break;
}}else{Ĺ(Ε,Λ.ĳ);switch(Ж){case β.ΒΗ:Ν(Α.ȫ);break;case β.Ƕ:Ν(Α.Ǥ);break;case β.ȏ:Ν(Α.Ш);break;default:Ν(Α.Ǔ);break;}}}else{if(ί!=Λ.ĸ&&ί!=Λ.ư) return;bool ΙΤ=ί==Λ.ư;ΒЀ();bool Γε=Ů(false)&&Θ.ε;switch(Ж){case β.Ƕ:Ν(Γε?Α.ȍ:Α.ǎ);break;case β.ȏ:Ν(Γε?Α.Ё:Α.Ш);break;case β.ǵ:Ν(Γε?Α.Ǥ:Α.Ǽ);break;
case β.ƭ:{if(Ȝ!=М||ΙΤ) Ν(Α.ǧ);else Ν(Α.Ė);}break;default:break;}}}void Ĕ(){if(ί==Λ.в&&!Θ.ε) return;if(Η==Ζ.Ϋ&&(!Ε.ε||!Θ.ε)) return;Κ="Move to job";β Ж=ΒΠ();if(Η==Ζ.Ϋ){Ĺ(Ε,Λ.ĳ);switch(Ж){case β.Ƕ:Ν(Α.Ǥ);break;case β.ȏ:Ν(Α.Ш);break;case β.ΒΗ:return;default:Ν(Α.Ǔ);break;
}Ȑ(Α.ȫ);}else{switch(Ж){case β.Ƕ:Ν(Α.Ш);break;case β.ȏ:Ν(Α.Ш);break;case β.ǵ:Ν(Α.Ǽ);break;case β.ƭ:Ν(Α.ǧ);break;default:break;}if(ί==Λ.в) Ȑ(Α.Ш);else Ȑ(Α.Ȧ);ΑϏ=true;}Βψ();π(Ϊ.ΒЃ);Ŵ(Ȅ,false);ш=Λ.ǀ;}void Ʒ(){if(!Θ.ε) return;Κ="Move home";β Ж=ΒΠ();if(Η==Ζ.Ϋ){Ĺ(Θ,Λ.Ĳ);switch(Ж)
{case β.ȏ:Ν(Α.Ё);break;case β.ǵ:Ν(Α.Ǥ);break;case β.ΒΖ:return;default:Ν(Α.Ǔ);break;}Ȑ(Α.ȫ);}else{if(Т(MyShipConnectorStatus.Connected)!=null) return;if(Т(MyShipConnectorStatus.Connectable)!=null){Ν(Α.Ē);Ȑ(Α.Ǽ);return;}switch(Ж){case β.Ƕ:Ν(Α.ȍ);break;case β.ȏ:Ν(Α.Ё);break;case β.ǵ:Ν(Α.Ё);break;
case β.ƭ:Ν(Α.Ǒ);break;default:break;}Ȑ(Α.Ǽ);}Βψ();π(Ϊ.ΒЃ);Ŵ(Ȅ,false);ш=Λ.ƿ;}void Ν(Α ΓЀ){if(ΓЀ==Α.й) Ɵ=Α.й;if(Ɵ!=Α.й&&Φ==Ɵ&&ΓЀ!=Ɵ){φ();return;}Ϗ=true;Φ=ΓЀ;}void Ȑ(Α Ц){this.Ɵ=Ц;}void ΗЇ(){if(Φ==Α.ǎ){if(Ϗ){ϋ=0;if(Ȝ!=М) Ĩ=0;Ȝ=М;}if(ϋ==0){int Ž=ΖА(М);if(Ž<0)
{if(!Ʊ()){Ν(Α.Ǒ);return;}if(!ΑΝ||(Ž==-2)!=ΑΜ){ΑΝ=true;ΑΜ=Ž==-2;Εγ=!ΑΜ?ά:Ε.Δ-С*α+
Ε.υ*(Е*ѝ/2+ΑЁ/2+α/2);}К(Εγ,10);Κ=ΑΜ?"Waiting for a free hole":"Waiting for the controller";return;}if(ΑΝ) Κ="";ΑΝ=false;if(Ž!=М){М=Ž;Ĩ=0;Ȝ=М;}ĺ ή=Ησ(М);if(ή==ĺ.Δγ){ί=Λ.ΒΣ;Κ="Job done";if(Č&&Θ.ε){Ν(Α.ȍ);Ȑ(Α.Ǽ);ш=Λ.ƿ;}else{Ν(Α.Ķ);Ȑ(Α.Ȧ);ш=Λ.ǀ;}
return;}if(ή==ĺ.Δτ){ϋ=1;ё(ο,true);Б=Ȏ(Ы,Ь,true);float h=Vector3.Dot(ά-Б,С);К(h<-1?Б+С*h:Б,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}}else{if(Ѓ>=0&&(ΔЇ()?Ǯ>=0&&Ǯ!=Ѓ:Βδ(Ѓ,true))){ϋ=0;return;}if(Н<wpReachedDist){if(Vector3.Distance(ς,Б)>1){К(Б,10);return;}Ν(Α.Ė);
return;}Vector3 Αω=Б-ά;float Ȭ=Vector3.Dot(Αω,С);if(Ȭ>0&&(Αω-С*Ȭ).Length()<wpReachedDist&&Ȉ<0.3f){if(++ϋ>30){ΑΙ=ά;Ν(Α.Ė);}return;}ϋ=1;}}if(Φ==Α.Ė){if(Ϗ){ё(ο,true);ǩ(false);Б=Ȏ(Ы,Ь,false);К(Βέ(Б,0),0);Ч(Ε.γ,Ε.δ,Ε.υ,false);ϋ=1;м=0;а=0;ΑΘ=0;Γο=-1;
Ī=Щ;Γκ=true;}if(!Ʊ()){Ν(Α.Ǒ);return;}if(Ů(true)){ΖΦ=Ȍ("","ORE",Й.ď);if((П==Ѐ.ΒΤ||П==Ѐ.Ƴ||
П==Ѐ.Βε||П==Ѐ.ƺ)&&Η!=Ζ.Ǐ)
Ν(Α.ē);else if((П==Ѐ.Δν||П==Ѐ.ΒΝ)&&Η!=Ζ.Ǐ)
Ν(Α.ΒЇ);else
Ν(Α.Ǒ);return;}У=Vector3.Distance(ά,Б);if(У>Ĩ){Ĩ=У;Γκ=false;}if(Η==Ζ.Ǐ&&Δω()==MyDetectedEntityType.SmallGrid)
а+=2;else а-=2;а=Math.Max(100,Math.Min(400,а));if(ϋ>0&&ϋ<а){if(У>м){if(а>150) м=У;else м=(float) Math.Ceiling(У);ϋ=1;}else ϋ++;}else{if(ϋ>0){Κ="Ship stuck! Retrying";м=У;ϋ=0;ę(false,true,0,α*sensorRange);}К(Βέ(Б,Math.Max(0,м-α)),Βΰ(false));if(Н<=wpReachedDist/2){ϋ=1;м=0;}
return;}ę(false,true,α*sensorRange,0);Vector3 ΗБ=Б+С*У;bool ΕЍ=false;if(Vector3.Distance(ΗБ,ά)>0.3f){Vector3 ΘΜ=Б+С*(У+0.1f);К(ΘΜ,4);ΕЍ=true;}else{float Ǟ=Βΰ(true);Vector3 Εκ=Βέ(Б,Math.Max(Щ+1,У+1));Ǩ(true,false,false,Εκ,Εκ-Б,Ǟ,Ǟ);}bool ΓΤ=false;if(ц==я.ΒΙ||ц==я.ΒΘ)
{if(!ΕЍ){float ΓЂ=0;foreach(IMyTerminalBlock Ǵ in ο)
ΓЂ+=ΗΖ(Ǵ,"","",ц==я.ΒΘ?new string[]{"STONE"}:null);if(ΓЂ>Γο||У<Щ||Γκ){ΒЎ=0;ΑΘ=У;Ī=(int)(Math.Max(Ī,ΑΘ)+α/2);}else{ΓΤ=У-ΑΘ>2&&ΒЎ>=20;ΒЎ++;}Γο=ΓЂ;}}else ΓΤ=У>=Ī;if(Ȝ!=М){Ν(Α.ǧ);У=0;return;}if(ΓΤ){М++;Ν(Α.ǧ);У=0;return;}}if(Φ==Α.ǧ||Φ==Α.Ǒ||Φ==Α.ē)
{if(Ϗ){Б=Ȏ(Ы,Ь,false);Ч(Ε.γ,Ε.δ,Ε.υ,false);ё(ο,ń);ǩ(false);м=Vector3.Distance(ά,Б);ę(false,true,0,α*sensorRange);}if(Ϗ) ƈ=false;К(ƈ?Б-С*α:Б,Βΰ(false));if(!ƈ&&Vector3.Distance(ά,Б)>=м+5){ё(ο,false);ǩ(true);Κ="Can´t return!";}if(Н<wpReachedDist){if(Φ==Α.Ǒ&&!ƈ){ƈ=true;return;}
if(Φ==Α.ǧ&&ΑϏ) Ν(Α.Ķ);if(Φ==Α.ǧ) Ν(Α.ǎ);if(Φ==Α.ē) Ν(Α.ƶ);if(Φ==Α.Ǒ){if(Θ.ε) Ν(Α.ȍ);else{φ();Ĕ();Κ="Can´t return, no dock found";}}return;}}if(Φ==Α.Ё||Φ==Α.Ш){if(Φ==Α.Ш&&ί==Λ.І&&Η!=Ζ.Ϋ){if(!Ʊ()||Ů(true)){Ν(Α.Ё);return;}}bool ΓЋ=false;bool ΑА=false;bool ΕΧ=false;
float Ŀ=0;bool ΖΥ=false;Ψ Ω=null;if(Ϗ){if(Φ==Α.Ё||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȑ(1)) ΒΕ();else{ј=null;Ȇ=0;Ƅ=!ΒΜ(1);}}Ψ έ=и();η=new Βχ();η.Ћ=έ;η.Ƃ=followPathDock*α;η.ΔΦ=useDockDirectionDist*α;η.Ŀ=10;η.Š.Add(έ.Δ);Vector3 ľ=new Vector3();if(Ǹ(έ,dockDist*α,true,out ľ))
η.Š.Add(ľ);else η.Ƃ*=1.5f;if(Η==Ζ.Ϋ){if(έ==Θ) η.Αά=Ε.Δ;if(έ==Ε) η.Αά=Θ.Δ;η.Γέ=dockDist*α*1.1f;}}else if(Φ==Α.Ш){η=new Βχ();η.Ћ=Ε;η.Ƃ=followPathJob*α;η.ΔΦ=useJobDirectionDist*α;η.Ŀ=10;η.Αά=Θ.Δ;η.Γέ=dockDist*α*1.1f;η.Š.Add(Ε.Δ);if(ί==Λ.в){if(!Θ.ε||Σ.Count==0){φ();return;}
float ΘΤ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΥ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΤ<ΘΥ) η.Ћ=Σ.Last();else η.Ћ=Σ.First();}}Ɨ=new Vector3();ΖΥ=!ΗΫ(ά);ё(ο,false);ǩ(true);Х=-1;double ǭ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.Αά)<=η.Γέ) continue;
double Ђ=Vector3.Distance(Σ[i].Δ,ά);if(ǭ==-1||Ђ<ǭ){Х=i;ǭ=Ђ;}}ǲ=ΗΡ(η.Ћ.Δ,Х);ğ=null;}if(Φ==Α.Ё&&Η!=Ζ.Ϋ&&!Ϗ&&!ŵ()){Ψ ΓΑ=и();Ƅ=!ΒΜ(1);if(и()!=ΓΑ){Ν(Α.Ё);return;}}if(Φ==Α.Ё&&Η!=Ζ.Ϋ&&!Ϗ&&ȑ(1)&&ΒΕ()){Ν(Α.Ё);return;}bool ΕЋ=false;if(Φ==Α.Ё&&!Ϗ&&Х>=0&&Х<Σ.Count)
{Vector3 ſ=Σ[Х].Δ+Δϊ(Х,ǲ);int ΓЁ=Х+ǲ;Vector3 œ=ǲ!=0&&ΓЁ>=0&&ΓЁ<Σ.Count?Σ[ΓЁ].Δ:η.Ћ.Δ;if(ΗΥ(ſ,œ)){if(!ΑΦ){ΑΦ=true;Εο=ķ(ſ)?ά:ſ;}К(Εο,0);Ч(η.Ћ.γ,η.Ћ.δ,η.Ћ.υ,false);Κ="Waiting for dock clearance";return;}ΑΦ=false;ΕЋ=ȑ(1)&&(ķ(ά)||ķ(œ));}ΖЉ(Σ,ǲ,η.Š,η.Ƃ,Ϗ,ref ϋ);
for(int i=0;i<η.Š.Count;i++){float Ђ=Vector3.Distance(ά,η.Š[i]);if(Ђ<=η.Ƃ) ΓЋ=true;if(Ђ<=η.ΔΦ) ΑА=true;}if(ΕЋ) ΑА=true;if(ΑА) Ŀ=η.Ŀ;float Ιζ=ğ!=null?ğ.Њ:Ȉ;float ΙΩ=(float) Math.Max(Ȉ*0.1f*α,wpReachedDist);if((Н<ΙΩ)||Ϗ){if(!Ϗ) Х+=ǲ;if(ǲ==0||Х>Σ.Count-1||Х<0)
ΓЋ=true;else{ΔΫ=ǲ>0?Σ.Count-1-Х:Х;Ω=Σ[Х];ğ=Ω;if(Х>=1&&Х<Σ.Count-1)
Ɨ=Ω.Δ-Σ[Х-ǲ].Δ;else ğ=null;ς=Ω.Δ+Δϊ(Х,ǲ);ΕΧ=true;}}if(ΑА)
Ч(η.Ћ.γ,η.Ћ.δ,η.Ћ.υ,false);else if(ΖΥ)
ΒΔ(η.Ћ.γ,10,true);else if(ΕΧ&&Ω!=null){if(ǲ>0) Ч(Ω.γ,Ω.δ,Ω.υ,90,false);else Ч(Ω.γ,-Ω.δ,-Ω.υ,90,false);}Ǩ(true,false,true,ς,Ɨ,ğ==null?0:ğ.Њ,Ŀ);if(ΓЋ){ΔΫ=0;if(Φ==Α.Ё&&!ΖЎ()){К(ς,0);return;}if(Φ==Α.Ё&&Ƅ){К(ς,0);Κ="Waiting for a free dock";return;}if(Φ==Α.Ё||Η==Ζ.Ϋ){Ν(Α.Ǥ);return;}
if(Φ==Α.Ш&&ΑϏ){Ν(Α.Ķ);return;}if(Φ==Α.Ш){Ν(Α.ǎ);return;}}}if(Φ==Α.Ǥ||Φ==Α.Ų){if(Η!=Ζ.Ϋ&&ŵ()){if(!ȑ(1)){Ν(Α.Ё);return;}}else if(Η!=Ζ.Ϋ){Ψ ΓΑ=и();if(!ΒΜ(2)||и()!=ΓΑ){Ν(Α.Ё);return;}}Ψ έ=и();if(Ϗ){if(!Ǹ(έ,dockDist*α,true,out ς)){Ǻ(г.Ũ);φ();return;}
К(ς,0);ΒΔ(έ.γ,90,true);}if(Н<followPathDock*α&&Н!=-1){К(ς,10);Ч(έ.γ,έ.δ,έ.υ,false);}if(Т(MyShipConnectorStatus.Connectable)!=null||Т(MyShipConnectorStatus.Connected)!=null){Ν(Α.Ē);return;}if(Н<wpReachedDist/2&&Н!=-1){Ν(Α.Ď);return;}}if(Φ==Α.Ď||Φ==Α.Ȧ){if(Ϗ)
{if(Φ==Α.Ď){Ψ έ=и();if(!Ǹ(έ,dockDist*α,true,out ς)){Ǻ(г.Ũ);φ();return;}Ǩ(true,true,false,ς,0);Ч(έ.γ,έ.δ,έ.υ,10,false);}if(Φ==Α.Ȧ){Ч(Ε.γ,Ε.δ,Ε.υ,0.5f,false);ς=Ε.Δ;Ǩ(true,true,false,ς,0);}}if(Αε){Ǉ(false,0,0,0,0);if(Φ==Α.Ď) Ν(Α.Ē);if(Φ==Α.Ȧ) φ();return;}}if(Φ==Α.Ē)
{if(Т(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.ȫ);else Ν(Α.Ǽ);return;}Ψ έ=и();if(Ϗ){а=0;ΘΦ=DateTime.Now;ϋ=0;Ч(έ.γ,έ.δ,έ.υ,false);}Vector3I Γβ=new Vector3I((int) έ.Ъ.X,(int) έ.Ъ.Y,(int) έ.Ъ.Z);IMySlimBlock Εβ=Me.CubeGrid.GetCubeBlock(Γβ);float Ερ=dockingSpeed;float Θξ=dockingSpeed*5;
float ΕΤ=Math.Max(1.5f,Math.Min(5f,α*0.15f));if(!Ǹ(έ,0,true,out ς)||!Ǹ(έ,ΕΤ,true,out Ɨ)||
Εβ==null||!Εβ.FatBlock.IsFunctional){Ǻ(г.Ũ);φ();return;}if(а==1||(Vector3.Distance(ά,ς)<=ΕΤ*1.1f&&!Ϗ))
а=1;else{Vector3 ΙΘ=б(Τ,Ɨ-ά);Vector3 Γα=б(Τ,Τ.GetNaturalGravity());float ΘЄ=Ư(ΙΘ,Γα,null);Ερ=Math.Min(Θξ,ΘЄ);}Ǩ(true,false,false,ς,ς-ά,dockingSpeed,Ερ);if(Ϗ) м=(float) Н;IMyShipConnector ΓΞ=Т(MyShipConnectorStatus.Connectable);if(ΓΞ!=null){Ǩ(false,false,false,ς,0);
if(ϋ>0) ϋ=0;ϋ--;if(ϋ<-5){ΓΞ.Connect();if(ΓΞ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.ȫ);else Ν(Α.Ǽ);ΒЄ();Ŵ(Ȅ,true);return;}}}else{float Εξ=(float) Math.Round(Н,1);if(Εξ<м){ϋ=-1;м=Εξ;}else ϋ++;if(ϋ>20){Ν(Α.Ų);return;}}}if(Φ==Α.Ǽ||Φ==Α.ȫ||
Φ==Α.ΑΒ||Φ==Α.Ƶ||Φ==Α.đ){bool ΒЌ=false,ΒЍ=false;if(Η==Ζ.Ϋ){if(и()==Θ) ΒЌ=true;else if(и()==Ε) ΒЍ=true;}if(Ϗ){Ɲ=false;if(Т(MyShipConnectorStatus.Connected)==null){Ν(Α.Ǔ);return;}ΒЄ();if(ΒЌ) ǋ(ό.Ǘ);if(ΒЍ) ǋ(ύ.Ǘ);Į=false;Ȓ=false;ș=false;Ʀ=false;}if(Т(MyShipConnectorStatus.Connected)==null){φ();Ǻ(г.ΔЃ);return;}
if(ί!=Λ.І||щ==-1||ǫ==т.None) Ȓ=true;else if(ǽ>=100f) Ȓ=true;else if(ǽ<=99f) Ȓ=false;if(ί!=Λ.І||ъ==-1||ѕ.Count==0) ș=true;else if(Ț>=100f) ș=true;else if(Ț<=99) ș=false;if(ί!=Λ.І||Ю==-1||ĉ.Count==0) Ʀ=true;else Ʀ=į>=Ю;Ļ с=null;if(ΒЌ) с=ό;
if(ΒЍ) с=ύ;if(с!=null&&(с.Ѝ==ι.ǃ||с.Ѝ==ι.ǂ)) Ȓ=true;if(с!=null&&(с.Ѝ==ι.Βξ)) if(!Į) Ȓ=false;if(с!=null&&(с.Ѝ==ι.Βς||с.Ѝ==ι.Βπ)) ș=true;if(с!=null&&(с.Ѝ==ι.Βρ)) if(!Į) ș=false;if(Ć){ChargeMode Εα=Ȓ?ChargeMode.Auto:ChargeMode.Recharge;if(с!=null&&(с.Ѝ==ι.ǂ||с.Ѝ==ι.ǃ))
Εα=ChargeMode.Discharge;Βϋ(Εα);Βύ(!ș);}if(!Į){if(Η==Ζ.Ϋ)
Į=ί!=Λ.І||ΖЏ(Ϗ,true)||Ƥ;else
Į=ί!=Λ.І||ΔЄ();}else{if(!Ȓ) Ν(Α.đ);if(!ș) Ν(Α.Ƶ);if(!Ʀ) Ν(Α.ΑΒ);Ϗ=false;}if(Į&&Ȓ&&ș&&Ʀ){Βϋ(ChargeMode.Auto);Βύ(false);if(ί==Λ.І){if(Η==Ζ.Ϋ){if(и()==Θ) ǋ(ό.ǚ);else if(и()==Ε) ǋ(ύ.ǚ);if(и()==Θ) Ĺ(Ε,Λ.ĳ);else Ĺ(Θ,Λ.Ĳ);}}Ν(Α.Ǔ);return;}}if(Φ==Α.Ǔ){if(Ʃ){if(!Δκ()) return;
Ʃ=false;Ν(Α.Ǔ);return;}if(Ϗ){if(!Δκ()){Ʃ=true;return;}IMyShipConnector Ȁ=Т(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Ш);return;}IMyShipConnector ΰ=Ȁ.OtherConnector;ΕΒ(Ȁ,false);Ŵ(Ȅ,false);Ψ έ=null;Ψ ΕЈ=Η==Ζ.Ϋ?Θ:и();if(Vector3.Distance(Ȁ.GetPosition(),ΕЈ.Δ)<5f&&Θ.ε) έ=ΕЈ;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.ε) έ=Ε;if(έ!=null){if(!Ǹ(έ,dockDist*α,true,out ς)){Ǻ(г.Ũ);φ();return;}К(ς,5);Ч(έ.γ,έ.δ,έ.υ,false);}else К(ά+ΰ.WorldMatrix.Forward*dockDist*α,5);if(ί==Λ.І) Ǻ(г.ǆ);}if(Н<wpReachedDist){ё(х,true);Ν(Α.Ш);return;}}if(Φ==Α.Ķ)
{if(Ϗ){ǩ(true);ё(ο,false);ς=Ε.Δ;К(ς,20);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(Н<wpReachedDist/2){Ν(Α.Ȧ);return;}}if(Φ==Α.ȍ){if(Ϗ){ǩ(true);ё(ο,false);int Ў=-1;double ǭ=-1;bool Ζδ=Βλ();for(int Ɩ=0;Ɩ<2&&Ў==-1;Ɩ++,Ζδ=false)
for(int i=Σ.Count-1;i>=0;i--){if(Ζδ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)<ΔЈ()+α) continue;double Ђ=Vector3.Distance(Σ[i].Δ,ά);if(ǭ==-1||Ђ<ǭ){Ў=i;ǭ=Ђ;}}if(Ў==-1){Ν(Α.Ё);return;}ς=Σ[Ў].Δ;К(ς,10);Ч(Ε.γ,Ε.δ,Ε.υ,false);}if(Н<wpReachedDist){Ν(Α.Ё);return;}}if(Φ==Α.ƶ)
{bool є=false;if(Ϗ){ǩ(true);if((П==Ѐ.ΒΤ||П==Ѐ.Ƴ)&&Ƽ()&&
ĵ(С,Τ.GetNaturalGravity())<25&&Е>=2&&Э>=2){Vector3 Ɔ=ά;if(Ы>0&&Ь<Э-1) Ɔ=Ȏ(Ы-1,Ь+1,true);else if(Ы<Е-1&&Ь<Э-1) Ɔ=Ȏ(Ы+1,Ь+1,true);else if(Ы<Е-1&&Ь>0) Ɔ=Ȏ(Ы+1,Ь-1,true);else if(Ы>0&&Ь>0) Ɔ=Ȏ(Ы-1,Ь-1,true);else є=true;if(!є) К(Ɔ,10);
}else є=true;}if(Н<wpReachedDist/2) є=true;if(є){Ν(Α.ΑΑ);return;}}if(Φ==Α.ΒЇ||Φ==Α.ΑΑ){if(Ϗ){Ǩ(true,true,false,ά,0);ё(ο,false);ǩ(true);ϋ=-1;а=П==Ѐ.Βε||П==Ѐ.ƺ?0:-1;}bool ġ=!Ʊ();int ΑЉ=Ȍ("STONE","ORE",Й.ď);if(П==Ѐ.ΒΝ||П==Ѐ.ƺ||П==Ѐ.Ƴ)
ΑЉ+=Ȍ("ICE","ORE",Й.ď);bool Θί=ΑЉ>0;bool Γά=false;if(а>=0){float ļ=(float) Math.Sin(Δο(а))*ѝ/3f;float ΘΒ=(float) Math.Cos(Δο(а))*Ǣ/3f;Vector3 ΘΗ=Ȏ(Ы,Ь,true)+ΒЅ(С,ȅ*-1,new Vector3(ļ,ΘΒ,0));К(ΘΗ,0.3f);if(Н<Math.Min(ѝ,Ǣ)/10f) а+=5f;if(а>=360) а=0;}if(ϋ==-1||ΑЉ<ϋ){ϋ=ΑЉ;м=0;}
else{м++;if(м>50) Γά=true;}if(!Θί||ġ||Γά){if(!ġ){int ΙΝ=Ȍ("","ORE",Й.ď);if(Ů(true)) ġ=true;else if(100-(В(ΙΝ,ΖΦ)*100)<minEjection) ġ=true;else Ǻ(г.ǆ);}if(Γά&&ġ) Κ="Ejection failed";if(Φ==Α.ΑΑ){if(ġ){if(Θ.ε) Ν(Α.ȍ);else{φ();Ĕ();Κ="Can´t return, no dock found";}}
else Ν(Α.ǎ);}else if(ġ) Ν(Α.Ǒ);else Ν(Α.Ė);return;}}Ϗ=false;}void ΖЉ(List<Ψ>č,int ζ,List<Vector3>ơ,float Ǡ,bool ȇ,ref int Ǖ){if(ȇ){for(int i=0;i<č.Count;i++) č[i].Њ=0;Ǖ=-1;return;}if(ζ==0) return;int ћ=ζ*-1;if(Ǖ==-1) Ǖ=ћ>0?1:č.Count-2;int ΕЏ=0;while(Ǖ>=1&&Ǖ<č.Count-1)
{if(ΕЏ>50) return;ΕЏ++;try{if((ћ<0&&Ǖ>=1)||(ћ>0&&Ǖ<=č.Count-2)){Ψ θ=č[Ǖ];bool ΖΡ=false;for(int j=0;j<ơ.Count;j++)
if(Vector3.Distance(θ.Δ,ơ[j])<=Ǡ){ΖΡ=true;break;}if(!ΖΡ){Ψ Αϋ=č[Ǖ-ћ];Ψ œ=č[Ǖ+ћ];Vector3 ΙΙ=θ.Δ-œ.Δ;Vector3 ΔΠ=Αϋ.Δ-θ.Δ;Vector3 ΘϏ=θ.Δ+Vector3.Normalize(ΙΙ)*ΔΠ.Length();Vector3 ΘΡ=Αϋ.Δ-ΘϏ;Vector3 Αμ=Ę(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ΘΡ);Vector3 Θν=Ę(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ΔΠ);
Vector3 Αξ=Ę(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,θ.ν);θ.Њ=(float) Math.Sqrt(Math.Pow(Αϋ.Њ,2)+Math.Pow(Ư(-Θν,Αξ,θ),2));for(int j=0;j<ơ.Count;j++)
if(Vector3.Distance(Αϋ.Δ,ơ[j])<=Ǡ){Vector3 Αο=Ę(ζ>0?θ.δ:θ.δ*-1,θ.γ*-1,ơ[j]-θ.Δ);float ΙΑ=Ư(-Αο,Αξ,θ);θ.Њ=Math.Min(θ.Њ,ΙΑ)/2f;}if(Αμ.Length()==0) Αμ=new Vector3(0,0,1);Vector3 Θμ=Ę(θ.δ,θ.γ*-1,θ.ν);float ΙΓ=Ū(-Αμ,Θμ,θ);float ΑΓ=В(ΙΓ,Ĭ);float ΔΞ=(float) Math.Sqrt(В(Αμ.Length()*1.0f,0.5f*ΑΓ));
θ.Њ=Math.Min(θ.Њ,(ΔΠ.Length()/ΔΞ)*Ǫ);}}}catch{return;}Ǖ+=ћ;}Ǖ=-1;}void ΒІ(bool ȇ){if(ȇ){ƍ=0;return;}if(Η==Ζ.Ϋ) return;float є=М*Math.Max(1,Щ);if(Ȝ==М) є+=Math.Min(Щ,У);float ΙΜ=Е*Э*Math.Max(1,Щ);ƍ=Math.Max(ƍ,(float) Math.Min(є/ΙΜ*100.0,100));}MyDetectedEntityType Δω()
{try{if(ǹ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float Βΰ(bool ǰ){if(Η==Ζ.Ǐ&&Δω()==MyDetectedEntityType.None&&!ŭ(ξ,true))
return fastSpeed;else return ǰ?Ŧ:ť;}void ΗЁ(){float ΖΛ=0,Εθ=0,ĥ=0,ΓЃ=0;for(int i=0;i<ȭ.Count;i++){IMyBatteryBlock Ż=ȭ[i];if(!ǹ(Ż,true)) continue;ΖΛ+=Ż.MaxStoredPower;Εθ+=Ż.CurrentStoredPower;ĥ+=Ż.CurrentInput;ΓЃ+=Ż.CurrentOutput;}ǽ=(float) Math.Round(В(Εθ,ΖΛ)*100,1);
if(ĥ>=ΓЃ) ǫ=т.đ;else ǫ=т.Δπ;if(ĥ==0&&ΓЃ==0||ǽ==100.0) ǫ=т.й;if(ȭ.Count==0) ǫ=т.None;}void ΗЅ(){float Εχ=0;for(int i=0;i<ѕ.Count;i++){IMyGasTank Ζτ=ѕ[i];if(!ǹ(Ζτ,true)) continue;Εχ+=(float) Ζτ.FilledRatio;}Ț=В(Εχ,ѕ.Count)*100f;}void ΗЍ(){į=0;try{for(int i=0;i<ĉ.Count;i++)
{IMyReactor ΓЌ=ĉ[i];if(!ǹ(ΓЌ,true)) continue;List<MyInventoryItem>д=new List<MyInventoryItem>();ΓЌ.GetInventory(0).GetItems(д);float ǔ=0;for(int j=0;j<д.Count;j++){MyInventoryItem Д=д[j];if(Д.Type.SubtypeId.ToUpper()=="URANIUM"&&Д.Type.TypeId.ToUpper().Contains("_INGOT"))
ǔ+=(float) Д.Amount;}if(ǔ<į||i==0){į=ǔ;Γχ=ΓЌ.CustomName;}}}catch(Exception e){ț=e;}}void ΗЂ(){Ƒ=0;ƀ=0;try{for(int i=0;i<Ń.Count;i++){IMyTerminalBlock Υ=Ń[i];if(!ǹ(Υ,true)) continue;ƀ+=(float) Υ.GetInventory(0).CurrentVolume;Ƒ+=(float) Υ.GetInventory(0).MaxVolume;
}Αλ=(float) Math.Min(Math.Round(В(ƀ,Ƒ)*100,1),100.0);}catch(Exception e){ț=e;}}void ΗІ(){try{Ǚ.Clear();for(int i=0;i<Ń.Count;i++){IMyTerminalBlock Υ=Ń[i];if(!ǹ(Υ,true)) continue;Δβ(Υ,Й.Ŭ);}if(П!=Ѐ.Βν){for(int i=0;i<Ĝ.Count;i++){IMyTerminalBlock Υ=Ĝ[i];if(!ǹ(Υ,true)) continue;
Δβ(Υ,Й.ΗΨ);}}Ηω(Ǚ);}catch(Exception e){ț=e;}}void ΖЍ(){if(ΑΡ){if(Δψ().Count>Αγ){ΑΡ=false;if(о!=з.ū){φ();if(о==з.ΒΩ) Ĕ();if(о==з.ǅ)
if(Θ.ε) Ʒ();else Ĕ();}Κ="Damage detected";}}}bool Ʊ(){if(!Αδ) return true;if(ί==Λ.І){if(щ>0&&ǫ!=т.None)
if(ǽ<=щ){Κ="Low energy! Move home";return false;}if(Ю>0&&ĉ.Count>0)
if(į<=Ю){Κ="Low fuel: "+Γχ;return false;}if(ъ>0&&ѕ.Count>0)
if(Ț<=ъ){Κ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δψ(){List<IMyTerminalBlock>Ġ=new List<IMyTerminalBlock>();for(int i=0;i<Ÿ.Count;i++){IMyTerminalBlock Υ=Ÿ[i];if(ŭ(Υ,false)) Ġ.Add(Υ);}return Ġ;}bool ŭ(IMyTerminalBlock Υ,bool ΔΗ){return(!ǹ(Υ,ΔΗ)||!Υ.IsFunctional);
}bool ǹ(IMyTerminalBlock Υ,bool ΔΗ){if(Υ==null) return false;try{IMyCubeBlock Εώ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΔΗ) return Εώ==Υ;else return Εώ.GetType()==Υ.GetType();}catch{return false;}}float ΗΖ(IMyTerminalBlock Υ,String ю,String Ζλ,String[]ǯ){float ȁ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory Ŋ=Υ.GetInventory(i);List<MyInventoryItem>д=new List<MyInventoryItem>();Ŋ.GetItems(д);foreach(MyInventoryItem Д in д){if(ǯ!=null&&(ǯ.Contains(Д.Type.TypeId.ToUpper())||ǯ.Contains(Д.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Д.Type.TypeId.ToUpper()==ю)&&(Ζλ==""||Д.Type.SubtypeId.ToUpper()==Ζλ))
ȁ+=(float) Д.Amount;}}return ȁ;}Ǧ ΗΜ(String ψ,String σ,Й Ж,bool ΘΝ){ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<Ǚ.Count;i++){Ǧ Д=Ǚ[i];if(Д.ψ.ToUpper()==ψ&&Д.σ.ToUpper()==σ&&(Д.ŏ==Ж||Ж==Й.ď))
return Д;}Ǧ ή=null;if(ΘΝ){ή=new Ǧ(ψ,σ,0,Ж);Ǚ.Add(ή);}return ή;}int Ȍ(String ψ,String σ,Й Ж){return Ȍ(ψ,σ,Ж,null);}int Ȍ(String ψ,String σ,Й Ж,String[]ǯ){int ȁ=0;ψ=ψ.ToUpper();σ=σ.ToUpper();for(int i=0;i<Ǚ.Count;i++){Ǧ Д=Ǚ[i];if(ǯ!=null&&ǯ.Contains(Д.ψ.ToUpper())) continue;
if((ψ==""||Д.ψ.ToUpper()==ψ)&&(σ==""||Д.σ.ToUpper()==σ)&&(Д.ŏ==Ж||Ж==Й.ď))
ȁ+=Д.ǔ;}return ȁ;}void Δβ(IMyTerminalBlock Υ,Й Ж){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>д=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(д);for(int j=0;j<д.Count;j++)
ΗΜ(д[j].Type.SubtypeId,д[j].Type.TypeId.Replace("MyObjectBuilder_",""),Ж,true).ǔ+=(int) д[j].Amount;}}void Ηω(List<Ǧ>Ф){for(int i=Ф.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ǧ a=Ф[j];Ǧ b=Ф[j+1];if(a.ǔ<b.ǔ) Ф.Move(j,j+1);}}bool Ů(bool Ιγ){if(ȥ&&Η!=Ζ.Ϋ)
if(Р!=-1&&Ĭ>=Р){Κ="Ship too heavy";return true;}if(Αλ>=Ǳ||Ɲ){Ɲ=false;Κ="Ship is full";return true;}return false;}bool ΔЄ(){String[]ǯ=null;if(!ȣ) ǯ=new string[]{"ICE"};if(Η==Ζ.ű) return Ȍ("","ORE",Й.Ŭ,ǯ)==0;if(Η==Ζ.Ǐ) return Ȍ("","COMPONENT",Й.Ŭ,ǯ)==0;else return Ȍ("","",Й.Ŭ,ǯ)==0;
}bool Ηά(){List<IMyCockpit>ΓΝ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΓΝ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΝ.Count;i++)
if(ΓΝ[i].IsUnderControl) return true;return false;}bool ΖЏ(bool ȇ,bool ΑЃ){IMyShipConnector Л=Т(MyShipConnectorStatus.Connected);if(Л==null) return false;if(Vector3.Distance(Θ.Δ,Л.GetPosition())<5)
return Δλ(ό,ȇ,ΑЃ);if(Vector3.Distance(Ε.Δ,Л.GetPosition())<5)
return Δλ(ύ,ȇ,ΑЃ);return false;}bool Δλ(Ļ ǿ,bool ȇ,bool ΑЃ){if(ȇ) ǿ.Ηθ();ǿ.ΗЊ();bool р=false;String ь="";switch(ǿ.Ѝ){case ι.Βο:{ь="Waiting for command";р=false;break;}case ι.ΔЎ:{ь="Waiting for cargo";р=Ů(true);break;}case ι.ΔЍ:{ь="Unloading";р=ΔЄ();break;}case ι.Βτ:{р=true;break;}
case ι.Βξ:{ь="Charging batteries";р=ǽ>=100f;break;}case ι.ǃ:{ь="Discharging batteries";р=ǽ<=25f;break;}case ι.ǂ:{ь="Discharging batteries";р=ǽ<=0f;break;}case ι.Βρ:{ь="Filling up hydrogen";р=Ț>=100f;break;}case ι.Βς:{ь="Unloading hydrogen";р=Ț<=25f;break;}
case ι.Βπ:{ь="Unloading hydrogen";р=Ț<=0f;break;}case ι.Βσ:{bool ΕЊ=Ηά();if(!ΕЊ) ǿ.ΓЈ=true;р=ǿ.ΓЈ&&ΕЊ;ь="Waiting for passengers";break;}}if(!р) ǿ.ΒΛ(true);if(р&&ǿ.Ββ()){р=ǿ.ΒΛ(false);ь="Undocking in: "+ΒΫ((int) Math.Max(0,ǿ.ǖ-ǿ.Ȳ));}if(ΑЃ) ΖЁ=ь;return р;}void ΖЅ()
{if(!ŋ) return;if(ο.Count<=1) return;float Ζϊ=0,Ζω=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǵ=ο[i];if(ŭ(Ǵ,true)) continue;Ζϊ+=(float) Ǵ.GetInventory(0).MaxVolume;Ζω+=(float) Ǵ.GetInventory(0).CurrentVolume;}float ΕΨ=(float) Math.Round(В(Ζω,Ζϊ),5);for(int Ɩ=0;Ɩ<Math.Max(1,Math.Floor(ο.Count/10f));Ɩ++)
{float Γη=0,Γφ=0;float ΕІ=0,ΖΙ=0;IMyTerminalBlock ȗ=null,ȳ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǵ=ο[i];if(ŭ(Ǵ,true)) continue;float Γύ=(float) Ǵ.GetInventory(0).MaxVolume;float Αέ=В((float) Ǵ.GetInventory(0).CurrentVolume,Γύ);if(ȗ==null||Αέ>Γη){ȗ=Ǵ;Γη=Αέ;ΕІ=Γύ;}
if(ȳ==null||Αέ<Γφ){ȳ=Ǵ;Γφ=Αέ;ΖΙ=Γύ;}}if(ȗ==null||ȳ==null||ȗ==ȳ) return;if(checkConveyorSystem&&!ȗ.GetInventory(0).IsConnectedTo(ȳ.GetInventory(0))){if(ΒБ>20) Κ="Inventory balancing failed";else ΒБ++;return;}ΒБ=0;List<MyInventoryItem>д=new List<MyInventoryItem>();
ȗ.GetInventory(0).GetItems(д);float ƨ=0;if(д.Count==0) continue;MyInventoryItem Αή=д[0];String ΕЎ=Αή.Type.TypeId+Αή.Type.SubtypeId;if(!ΖЀ.TryGetValue(ΕЎ,out ƨ)){if(Ηα(ȗ.GetInventory(0),0,ȳ.GetInventory(0),out ƨ))
ΖЀ.Add(ΕЎ,ƨ);else return;}float ΙΚ=((Γη-ΕΨ)*ΕІ/ƨ);float ΙΛ=((ΕΨ-Γφ)*ΖΙ/ƨ);int ΔΤ=(int) Math.Min(ΙΛ,ΙΚ);if(ΔΤ<=0) return;if((float) Αή.Amount<ΔΤ)
ȗ.GetInventory(0).TransferItemTo(ȳ.GetInventory(0),0,null,null,Αή.Amount);else
ȗ.GetInventory(0).TransferItemTo(ȳ.GetInventory(0),0,null,null,ΔΤ);}}bool Ηα(IMyInventory ƞ,int Β,IMyInventory ΘΠ,out float ΖϏ){ΖϏ=0;float ΘΕ=(float) ƞ.CurrentVolume;List<MyInventoryItem>ΓΒ=new List<MyInventoryItem>();ƞ.GetItems(ΓΒ);float ΕΫ=0;for(int i=0;i<ΓΒ.Count;i++) ΕΫ+=(float) ΓΒ[i].Amount;
ƞ.TransferItemTo(ΘΠ,Β,null,null,1);float Ζώ=ΘΕ-(float) ƞ.CurrentVolume;List<MyInventoryItem>ΒЉ=new List<MyInventoryItem>();ƞ.GetItems(ΒЉ);float ΕΟ=0;for(int i=0;i<ΒЉ.Count;i++) ΕΟ+=(float) ΒЉ[i].Amount;if(Ζώ==0f||!Βζ(0.9999,ΕΫ-ΕΟ,1.0001))
return false;ΖϏ=Ζώ;return true;}void ΗЄ(){Vector3 Ɗ=ς+Ȣ;Vector3 ǳ=Ɗ-ά;if(ǳ.Length()==0) ǳ=new Vector3(0,0,-1);Vector3 Αο=б(Τ,ǳ);Vector3 Ζυ=Vector3.Normalize(Αο);Vector3 Ŏ=б(Τ,Τ.GetNaturalGravity());float ΘЍ=ΓІ>0?Math.Max(0,1-ĵ(ǳ,Εω)/5):0;float Њ=(float) Math.Min((Γϊ>0?Γϊ:1000f),Math.Max(Ư(-Αο,Ŏ,null),ΓІ*ΘЍ));
if(!Ζψ) Њ=0;if(Ζε) Њ=Math.Max(0,1-ΑΟ/ΑΔ)*Њ;if(generalSpeedLimit>0) Њ=Math.Min(generalSpeedLimit,Њ);if(ŗ) Њ=0;if(э>=0) Њ=Math.Min(Њ,э);if(Βλ()) Њ=Math.Min(Њ,jobAreaSpeed);if(Ζζ) Њ*=(float) Math.Min(1,В(ǳ.Length(),wpReachedDist)/2);Vector3 ΖΗ=б(Τ,Τ.GetShipVelocities().LinearVelocity);
float ΘΑ=(float)(Math.Max(0,15-ĵ(-Ζυ,-ΖΗ))/15)*0.85f+0.15f;ΔΛ+=Math.Sign(ΘΑ-ΔΛ)/10f;Vector3 Ȯ=Ζυ*Њ*ΔΛ-(ΖΗ);Vector3 ΒА=Βί(Ȯ,null);if(ΑЋ&&Н>0.1f){Ȯ.X*=ΒϏ(Ȯ.X,ref ΑЌ.X,1f,ΒА.X,20);Ȯ.Y*=ΒϏ(Ȯ.Y,ref ΑЌ.Y,1f,ΒА.Y,20);Ȯ.Z*=ΒϏ(Ȯ.Z,ref ΑЌ.Z,1f,ΒА.Z,20);}else ΑЌ=new Vector3(1,1,1);
Ƣ=Ĭ*Ȯ-Ŏ*Ĭ;Δδ(Ƣ,ΑЋ);Н=Vector3.Distance(ά,Ɗ);}float ΒϏ(float value,ref float Ц,float ћ,float ȝ,float Γϋ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ц)) Ц+=Math.Sign(Ц)*ћ;else Ц=value;if(value==0) Ц=1;float ή=Math.Abs(Ц);if(ή<Γϋ||ȝ==0) return ή;Ц=Math.Min(Γϋ,Math.Max(-Γϋ,Ц));
ή=Math.Abs(ȝ);return ή;}void ΕΝ(){float ы=90,Ǎ=90,њ=90;float Ǟ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǰ,ї,Ɛ,Γσ;if(Γτ){ǰ=Vector3.Normalize(ς-ά);Ɛ=б(Τ,ǰ);ї=б(Τ,ΔΙ);ы=ĵ(Ɛ,new Vector3(0,-1,0))-90;Ǎ=Đ(ї,new Vector3(-1,0,0),ї.Y);
њ=Đ(Ɛ,new Vector3(-1,0,0),Ɛ.Z);}else{ǰ=ΔΚ;ї=б(Τ,ΔΙ);Ɛ=б(Τ,ΔΚ);Γσ=б(Τ,Ζφ);ы=Đ(ї,new Vector3(0,0,1),ї.Y);Ǎ=Đ(ї,new Vector3(-1,0,0),ї.Y);њ=Đ(Γσ,new Vector3(0,0,1),Γσ.X);}if(ΕΡ&&Ƽ()){Vector3 Γα=Τ.GetNaturalGravity();ї=б(Τ,Γα);ы=Đ(ї,new Vector3(0,0,1),ї.Y);
Ǎ=Đ(ї,new Vector3(-1,0,0),ї.Y);}if(!Βζ(-45,Ǎ,45)){ы=0;њ=0;}if(!Βζ(-45,њ,45)) ы=0;Ǉ(Γγ,1,(-ы)*Ǟ,(-њ)*Ǟ,(-Ǎ)*Ǟ);ΑΟ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(Ǎ),Math.Abs(њ)));Αε=ΑΟ<=ΑΔ;}void ΖЊ(IMyTerminalBlock Αό){if(Αό==null) return;ǣ=new float[3,2];ќ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȋ.Count;i++){IMyThrust е=Ȋ[i];if(!е.IsFunctional) continue;Vector3 ζ=б(Αό,е.WorldMatrix.Backward);float ȱ=е.MaxEffectiveThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) ǣ[0,0]+=ȱ;else ǣ[0,1]-=ȱ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) ǣ[1,0]+=ȱ;else ǣ[1,1]-=ȱ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) ǣ[2,0]+=ȱ;else ǣ[2,1]-=ȱ;String ю=Δό(е);float[,]ў=null;if(ќ.ContainsKey(ю)) ў=ќ[ю];else{ў=new float[3,2];ќ.Add(ю,ў);}float ȝ=е.MaxThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) ў[0,0]+=ȝ;else ў[0,1]-=ȝ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) ў[1,0]+=ȝ;else ў[1,1]-=ȝ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) ў[2,0]+=ȝ;else ў[2,1]-=ȝ;}}static String Δό(IMyThrust е){return е.BlockDefinition.SubtypeId;}Vector3 Βθ(Vector3 ѓ,float[,]Ő){return new Vector3(
ѓ.X>=0?Ő[0,0]:Ő[0,1],
ѓ.Y>=0?Ő[1,0]:Ő[1,1],
ѓ.Z>=0?Ő[2,0]:Ő[2,1]);}bool Δϋ(Ψ Ω,String ю,out float ΓΫ){ΓΫ=0;int Β=ȉ.IndexOf(ю);if(Β==-1||Ω.ǡ==null||Β>=Ω.ǡ.Length)
return false;ΓΫ=Ω.ǡ[Β];if(ΓΫ==-1) return false;return true;}Vector3 Βί(Vector3 ѓ,Ψ Ω){if(Ω!=null){Vector3 ή=new Vector3();for(int i=0;i<ќ.Keys.Count;i++){String ю=ќ.Keys.ElementAt(i);float Ƈ=0;if(!Δϋ(Ω,ю,out Ƈ))
return Βθ(ѓ,ǣ);ή+=Βθ(ѓ,ќ.Values.ElementAt(i))*Ƈ;}return ή;}return Βθ(ѓ,ǣ);}float Ū(Vector3 ѓ,Ψ Ω){return Ū(ѓ,new Vector3(),Ω);}float Ū(Vector3 ѓ,Vector3 ν,Ψ Ω){Vector3 ΙΒ=Βί(ѓ,Ω);Vector3 ȱ=ΙΒ+ν*Ĭ;float ƙ=(ȱ/ѓ).AbsMin();return(float)(ѓ*ƙ).Length();}float Ư(Vector3 Αν,Vector3 Ŏ,Ψ Ω)
{if(Αν.Length()==0) return 0;float ΕΠ=1;if(Ŏ.Length()>0) ΕΠ=Math.Min(1,ĵ(-Ŏ,Αν)/90)*0.4f+0.6f;float Ƣ=Ū(Αν,Ŏ,Ω);if(Ƣ==0) return 0.1f;float ΑΓ=В(Ƣ,Ĭ);float ΔΞ=(float) Math.Sqrt(В(Αν.Length(),ΑΓ*0.5f));return ΑΓ*ΔΞ*ΕΠ*Ǫ;}float ƹ(Vector3 ǰ,Vector3 ω,Vector3 ν,Ψ Ω)
{if(ν.Length()==0f) return 0;Vector3 Αξ=Ę(ǰ,ω,Vector3.Normalize(ν));float ǘ=Ū(-Αξ,Ω);return ǘ/ν.Length();}void Δη(bool Θε){float џ=0;float Γή=0.9f;if(Θε){Р=-1;İ=0;ΑЇ=null;if(ί!=Λ.в&&Ε.ν.Length()!=0){џ=Γή*ƹ(Ε.δ,Ε.γ*-1,Ε.ν,null);if(џ<Р||Р==-1) Р=џ;}if(Θ.ε&&Θ.ν.Length()!=0)
{џ=Γή*ƹ(Θ.δ,Θ.γ*-1,Θ.ν,null);if(џ<Р||Р==-1) Р=џ;}return;}if(İ==-1) return;if(İ>=0){int ΖΫ=0;while(İ<Σ.Count){if(ΖΫ>100) return;ΖΫ++;Ψ Ω=Σ[İ];if(Ω.ν.Length()!=0f){џ=Γή*Math.Min(
ƹ(Ω.δ,Ω.γ*-1,Ω.ν,Ω),
ƹ(Ω.δ*-1,Ω.γ*-1,Ω.ν,Ω));if(џ<Р||Р==-1) Р=џ;}else ΑЇ=Ω;İ++;}İ=-1;}bool Γδ=true;float Ĉ=0;if(ΑЇ!=null){for(int i=0;i<ќ.Count;i++){String ю=ќ.Keys.ElementAt(i);float[,]ў=ќ.Values.ElementAt(i);float Ƈ=0;if(!Δϋ(ΑЇ,ю,out Ƈ)){Γδ=false;break;}for(int a=0;a<ў.GetLength(0);a++)
for(int b=0;b<ў.GetLength(1);b++){float ě=Math.Abs(ў[a,b]*Ƈ);if(ě==0) continue;Γδ=true;if(Ĉ==0||ě<Ĉ) Ĉ=ě;}}}if(!Γδ){for(int a=0;a<ǣ.GetLength(0);a++)
for(int b=0;b<ǣ.GetLength(1);b++){float ě=Math.Abs(ǣ[a,b]);if(ě==0) continue;if(Ĉ==0||ě<Ĉ) Ĉ=ě;}}if(Ĉ>0){float Θπ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;џ=В(Ĉ,Θπ);if(џ>0)
if(џ<Р||Р==-1) Р=џ;}}void Δδ(Vector3 ǘ,bool Ј){if(!Ј){for(int i=0;i<Ȋ.Count;i++)
Ȋ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЏ=Βί(ǘ,null);float ΙΫ=Math.Min(1,Math.Abs(В(ǘ.X,ΒЏ.X)));float Ιά=Math.Min(1,Math.Abs(В(ǘ.Y,ΒЏ.Y)));float Ιέ=Math.Min(1,Math.Abs(В(ǘ.Z,ΒЏ.Z)));for(int i=0;i<Ȋ.Count;i++){IMyThrust е=Ȋ[i];Vector3 ζ=Ηκ(б(Τ,е.WorldMatrix.Backward),1);
if(ζ.X!=0&&Math.Sign(ζ.X)==Math.Sign(ǘ.X))
е.SetValueFloat("Override",е.MaxThrust*ΙΫ);else if(ζ.Y!=0&&Math.Sign(ζ.Y)==Math.Sign(ǘ.Y))
е.SetValueFloat("Override",е.MaxThrust*Ιά);else if(ζ.Z!=0&&Math.Sign(ζ.Z)==Math.Sign(ǘ.Z))
е.SetValueFloat("Override",е.MaxThrust*Ιέ);else
е.SetValueFloat("Override",0.0f);}}void Ǉ(bool Ј,float Θό,float ы,float њ,float Ǎ){for(int i=0;i<Αΰ.Count;i++){IMyGyro Ї=Αΰ[i];Ї.GyroOverride=Ј;if(!Ј) Ї.GyroPower=100;else Ї.GyroPower=Θό;if(!Ј) continue;Vector3 Г=Τ.WorldMatrix.Forward;Vector3 Я=Τ.WorldMatrix.Right;Vector3 ω=Τ.WorldMatrix.Up;
Vector3 ņ=Ї.WorldMatrix.Forward;Vector3 ň=Ї.WorldMatrix.Up;Vector3 Ň=Ї.WorldMatrix.Left*-1;if(ņ==Г) Ї.SetValueFloat("Roll",Ǎ);else if(ņ==(Г*-1)) Ї.SetValueFloat("Roll",Ǎ*-1);else if(ň==(Г*-1)) Ї.SetValueFloat("Yaw",Ǎ);else if(ň==Г) Ї.SetValueFloat("Yaw",Ǎ*-1);else if(Ň==Г) Ї.SetValueFloat("Pitch",Ǎ);
else if(Ň==(Г*-1)) Ї.SetValueFloat("Pitch",Ǎ*-1);if(Ň==(Я*-1)) Ї.SetValueFloat("Pitch",ы);else if(Ň==Я) Ї.SetValueFloat("Pitch",ы*-1);else if(ň==Я) Ї.SetValueFloat("Yaw",ы);else if(ň==(Я*-1)) Ї.SetValueFloat("Yaw",ы*-1);else if(ņ==(Я*-1)) Ї.SetValueFloat("Roll",ы);
else if(ņ==Я) Ї.SetValueFloat("Roll",ы*-1);if(ň==(ω*-1)) Ї.SetValueFloat("Yaw",њ);else if(ň==ω) Ї.SetValueFloat("Yaw",њ*-1);else if(Ň==ω) Ї.SetValueFloat("Pitch",њ);else if(Ň==(ω*-1)) Ї.SetValueFloat("Pitch",њ*-1);else if(ņ==ω) Ї.SetValueFloat("Roll",њ);else if(ņ==(ω*-1)) Ї.SetValueFloat("Roll",њ*-1);
}}void ΕΗ(){this.Γγ=false;}void Ч(Vector3 Ȱ,Vector3 Г,Vector3 Γρ,float ȡ,bool Ƨ){ΒΔ(Ȱ,ȡ,Ƨ);ΑΔ=ȡ;Γτ=false;this.ΔΚ=Г;this.Ζφ=Γρ;}void Ч(Vector3 Ȱ,Vector3 Г,Vector3 Γρ,bool Ƨ){Ч(Ȱ,Г,Γρ,2f,Ƨ);}void ΒΔ(Vector3 Ȱ,float ȡ,bool Ƨ){ΑΔ=ȡ;this.Γγ=true;this.ΕΡ=Ƨ;Γτ=true;Αε=false;
this.ΔΙ=Ȱ;}void ΒЄ(){Ǩ(false,false,false,ς,0);ΑЋ=false;}void К(Vector3 Ћ,float Ǟ){Ǩ(true,false,false,Ћ,Ǟ);}void Ǩ(bool Ј,bool ΔΔ,bool ΔΧ,Vector3 Ћ,float Ǟ){Ǩ(Ј,ΔΔ,ΔΧ,Ћ,Ћ-ά,0.0f,Ǟ);}void Ǩ(bool Ј,bool ΔΔ,bool ΔΧ,Vector3 Ћ,Vector3 ΘЈ,float ΘЉ,float Ǟ){ΑЋ=true;this.Ζψ=Ј;
ς=Ћ;this.Γϊ=Ǟ;this.ΓІ=ΘЉ;this.Ζζ=ΔΔ;this.Ζε=ΔΧ;this.Εω=ΘЈ;Н=Vector3.Distance(Ћ,ά);}void Ημ(){ϊ.GetBlocksOfType(Ÿ,ǐ);}void Ην(){if(Αώ){ώ=2;return;}List<IMyRemoteControl>ƚ=new List<IMyRemoteControl>();List<IMySensorBlock>Ζβ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζν=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(ƚ,ǐ);ϊ.GetBlocksOfType(і,ǐ);ϊ.GetBlocksOfType(Ζβ,ǐ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζν,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);і=ǥ(і,pamTag,true);Ητ();Ηυ(Ζν);ų(і,setLCDFontAndSize,1.4f,false);ų(ċ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζρ=ǥ(Ζβ,pamTag,true);if(Ζρ.Count>0) ξ=Ζρ[0];else ξ=null;if(Η==Ζ.ű){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ώ=1;Κ="Drills are missing";}}else if(Η==Ζ.Ǐ){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ώ=1;Κ="Grinders are missing";}if(ξ==null){ώ=1;Κ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ϊ.GetBlocksOfType(ΔΟ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζπ=ǥ(ƚ,pamTag,true);if(Ζπ.Count>0) ƚ=Ζπ;if(ƚ.Count>0) Τ=ƚ[0];else{Τ=null;ώ=2;Κ="Remote is missing";return;}α=(float) Τ.CubeGrid.WorldVolume.Radius*2;љ=null;if(Η!=Ζ.Ϋ){Δθ();if(ο.Count>0&&љ!=null){if(ξ!=null&&(љ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Τ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Τ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ώ=1;Κ="Wrong sensor direction";}if(љ.WorldMatrix.Forward!=Τ.WorldMatrix.Forward&&
љ.WorldMatrix.Forward!=Τ.WorldMatrix.Down){ώ=2;Κ="Wrong remote direction";}}}}void Ηξ(){ϊ.GetBlocksOfType(Ȅ,ǐ);ϊ.GetBlocksOfType(х,ǐ);ϊ.GetBlocksOfType(Ȋ,ǐ);ϊ.GetBlocksOfType(Αΰ,ǐ);ϊ.GetBlocksOfType(ȭ,ǐ);ϊ.GetBlocksOfType(ĉ,ǐ);ϊ.GetBlocksOfType(ѕ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(ΑІ,ǐ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
х=ǥ(х,"ConnectorMedium",false);else
х=ǥ(х,"Connector",false);List<IMyShipConnector>Ζξ=ǥ(х,pamTag,true);if(Ζξ.Count>0) х=Ζξ;if(ώ<=1){if(х.Count==0){ώ=1;Κ="Connector is missing";}if(Αΰ.Count==0){ώ=1;Κ="Gyros are missing";}if(Ȋ.Count==0){ώ=1;Κ="Thrusters are missing";}}List<IMyConveyorSorter>Ζς=ǥ(ΑІ,pamTag,true);
if(Ζς.Count>0) ΑІ=Ζς;List<IMyLandingGear>Ζο=ǥ(Ȅ,pamTag,true);if(Ζο.Count>0) Ȅ=Ζο;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζμ=ǥ(ȭ,pamTag,true);if(Ζμ.Count>0) ȭ=Ζμ;List<IMyGasTank>Ζσ=ǥ(ѕ,pamTag,true);if(Ζσ.Count>0) ѕ=Ζσ;}void Ηο(){ϊ.GetBlocksOfType(Ĝ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ń.Clear();for(int i=Ĝ.Count-1;i>=0;i--){if(ΗΪ(Ĝ[i])){Ń.Add(Ĝ[i]);Ĝ.RemoveAt(i);}}}bool ΗΪ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βγ(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δθ(){if(Τ==null) return;љ=null;float Αφ=0,Αρ=0,Αχ=0,Ας=0,Αψ=0,Ασ=0;List<IMyTerminalBlock>ΔΘ=ǥ(ο,pamTag,true);bool Θψ=ΔΘ.Count==0;if(ΔΘ.Count>0) љ=ΔΘ[0];else if(ο.Count>0) љ=ο[0];int ȁ=0;float Ř=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward){if(Θψ){ώ=2;Κ="Mining direction is unclear!";return;}continue;}ȁ++;Ř=Math.Max(Ř,ΗϏ(ο[i]));Vector3 Љ=Ƹ(Τ,ο[i].GetPosition());if(i==0){Αφ=Љ.X;Αρ=Љ.X;Αχ=Љ.Y;Ας=Љ.Y;Αψ=Љ.Z;Ασ=Љ.Z;}Αρ=Math.Max(Љ.X,Αρ);Αφ=Math.Min(Љ.X,Αφ);
Ας=Math.Max(Љ.Y,Ας);Αχ=Math.Min(Љ.Y,Αχ);Ασ=Math.Max(Љ.Z,Ασ);Αψ=Math.Min(Љ.Z,Αψ);}if(Ř==0) Ř=drillRadius;bool Ȱ=љ!=null&&љ.WorldMatrix.Forward==Τ.WorldMatrix.Down;ѝ=(Αρ-Αφ)*(1-ı)+Ř*2;Ǣ=(Ȱ?Ασ-Αψ:Ας-Αχ)*(1-ģ)+Ř*2;IMyCubeGrid g=Τ.CubeGrid;float Δέ=0,Δή=0,Δί=0,Δΰ=0;
for(int c=0;c<8;c++){Vector3 p=Ƹ(Τ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=Ȱ?p.Z:p.Y;if(c==0||p.X<Δέ) Δέ=p.X;if(c==0||p.X>Δή) Δή=p.X;if(c==0||y<Δί) Δί=y;if(c==0||y>Δΰ) Δΰ=y;}ΑЁ=Δή-Δέ+g.GridSize;ΓА=Δΰ-Δί+g.GridSize;}float ΗϏ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>ǥ<T>(List<T>Ф,String ǟ,bool Αα){List<T>ή=new List<T>();for(int i=0;i<Ф.Count;i++)
if(Βγ(Ф[i],ǟ,Αα)) ή.Add(Ф[i]);return ή;}bool Βγ<T>(T Υ,String ǟ,bool Αα){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(Αα&&b.CustomName.ToUpper().Contains(ǟ.ToUpper())) return true;if(!Αα&&b.BlockDefinition.ToString().ToUpper().Contains(ǟ.ToUpper())) return true;return false;}void ё<T>(List<T>Ф,bool Ј)
{for(int i=0;i<Ф.Count;i++)
ΕΒ((IMyTerminalBlock) Ф[i],Ј);}void ΕΒ(IMyTerminalBlock Υ,bool Ј){if(Υ==null) return;String ΗА=Ј?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(ΗА);a.Apply(Υ);}void ǩ(bool Ј){ΔΕ=Ј;if(!ĭ) return;ё(ΑІ,Ј);}void Βϋ(ChargeMode Ɠ){for(int i=0;i<ȭ.Count;i++) ȭ[i].ChargeMode=Ɠ;}void Βύ(bool ΘЏ)
{for(int i=0;i<ѕ.Count;i++) ѕ[i].Stockpile=ΘЏ;}void Ŵ(List<IMyLandingGear>Γί,bool ΖΘ){for(int i=0;i<Γί.Count;i++){if(ΖΘ) Γί[i].Lock();if(!ΖΘ) Γί[i].Unlock();}}void ΕΓ(bool Ј){if(Ει==Ј) return;List<IMyShipController>ΑΛ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΛ,ǐ);if(ΑΛ.Count==0) return;
for(int i=0;i<ΑΛ.Count;i++)
ΑΛ[i].DampenersOverride=Ј;Ει=Ј;}IMyShipConnector Т(MyShipConnectorStatus ь){for(int i=0;i<х.Count;i++){if(!ǹ(х[i],true)) continue;if(х[i].Status==ь) return х[i];}return null;}void ę(bool ΔΑ,bool ΑΥ,float ΘΫ,float ΘΔ){if(ξ==null||ο.Count==0) return;Vector3 ΑΚ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward) continue;ȁ++;ΑΚ+=ο[i].GetPosition();}ΑΚ=ΑΚ/ȁ;Vector3 Ŕ=Ƹ(ξ,ΑΚ);ξ.Enabled=true;ξ.ShowOnHUD=ΔΑ;ξ.LeftExtend=(ΑΥ?1:л)/2f*ѝ-Ŕ.X;ξ.RightExtend=(ΑΥ?1:л)/2f*ѝ+Ŕ.X;ξ.TopExtend=(ΑΥ?1:к)/2f*Ǣ+Ŕ.Y;
ξ.BottomExtend=(ΑΥ?1:к)/2f*Ǣ-Ŕ.Y;ξ.FrontExtend=(ΔΑ?Щ:ΘΫ)-Ŕ.Z;ξ.BackExtend=ΔΑ?0:ΘΔ+α*0.75f+Ŕ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void ų<T>(List<T>Ф,bool ΘЊ,float ΘΩ,bool Θζ){for(int i=0;i<Ф.Count;i++){IMyTextSurface Ơ=null;if(Ф[i]is IMyTextSurface) Ơ=(IMyTextSurface) Ф[i];if(Ơ!=null){Ơ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЊ) continue;Ơ.Font="Debug";if(Θζ) continue;Ơ.FontSize=ΘΩ;}}}bool Ƽ(){Vector3D λ;return this.Τ.TryGetPlanetPosition(out λ);}int ΗΡ(Vector3 ΘБ,int Ζη){if(Ζη==-1) return 0;double Ў=-1;int Ȗ=-1;for(int i=Σ.Count-1;i>=0;i--){double Ђ=Vector3.Distance(Σ[i].Δ,ΘБ);if(Ў==-1||Ђ<Ў){Ȗ=i;Ў=Ђ;}
}return Math.Sign(Ȗ-Ζη);}bool ΗΫ(Vector3 λ){List<Vector3>И=new List<Vector3>();for(int i=0;i<Σ.Count;i++) И.Add(Σ[i].Δ);if(Θ.ε&&Σ.Count>=1){Vector3 ľ=new Vector3();Ǹ(Θ,dockDist*α,false,out ľ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){И.Insert(0,ľ);И.Insert(0,Θ.Δ);}
else{И.Add(ľ);И.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.ε&&Σ.Count>=1){Vector3 ΒЋ=new Vector3();Ǹ(Ε,dockDist*α,false,out ΒЋ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){И.Insert(0,ΒЋ);И.Insert(0,Ε.Δ);}else{И.Add(ΒЋ);И.Add(Ε.Δ);}}}else{if(ί!=Λ.в)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
И.Insert(0,Ε.Δ);else И.Add(Ε.Δ);}int Ȗ=-1;double ǭ=-1;for(int i=0;i<И.Count;i++){double Ђ=Vector3.Distance(И[i],λ);if(Ђ<ǭ||ǭ==-1){ǭ=Ђ;Ȗ=i;}}if(И.Count==0) return false;double ΓΥ=Vector3.Distance(И[Ȗ],λ);double Θύ=Vector3.Distance(И[Math.Max(0,Ȗ-1)],И[Ȗ])*1.5f;
double Θχ=Vector3.Distance(И[Math.Min(И.Count-1,Ȗ+1)],И[Ȗ])*1.5f;return ΓΥ<Θύ||ΓΥ<Θχ||ΓΥ<laneOffset*α*1.5f;}static float В(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηκ(Vector3 v,int ΓΡ){return new Vector3(Math.Round(v.X,ΓΡ),Math.Round(v.Y,ΓΡ),Math.Round(v.Z,ΓΡ));
}float ĵ(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΧ=(a*b).Sum;float ΖΓ=a.Length();float ΖΔ=b.Length();if(ΖΓ==0||ΖΔ==0) return 0;float ή=(float)((180.0f/Math.PI)*Math.Acos(ΘΧ/(ΖΓ*ΖΔ)));if(float.IsNaN(ή)) return 0;return ή;}float Đ(Vector3 v,Vector3 Αό,float ΘЋ){float ļ=ĵ(v,Αό);
if(ΘЋ>0f) ļ*=-1;if(ļ>-90f) return ļ-90f;else return 180f-(-ļ-90f);}double Δο(float ΘΞ){return(Math.PI/180)*ΘΞ;}bool Βζ(double Θο,double value,double Απ){return(value>=Θο&&value<=Απ);}Vector3 ΗЀ(IMyTerminalBlock Υ,Vector3 Љ){return Vector3D.Transform(Љ,Υ.WorldMatrix);
}Vector3 Ƹ(IMyTerminalBlock Υ,Vector3 ΙΨ){return б(Υ,ΙΨ-Υ.GetPosition());}Vector3 б(IMyTerminalBlock Υ,Vector3 Δά){return Vector3D.TransformNormal(Δά,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 Ę(Vector3 ǰ,Vector3 ω,Vector3 Δά){MatrixD Γω=MatrixD.CreateFromDir(ǰ,ω);return Vector3D.TransformNormal(Δά,MatrixD.Transpose(Γω));
}Vector3 ΒЅ(Vector3 ǰ,Vector3 ω,Vector3 Љ){MatrixD Γω=MatrixD.CreateFromDir(ǰ,ω);return Vector3D.Transform(Љ,Γω);}String κ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]З=s.Split('|');return new Vector3(float.Parse(Μ(З,0)),float.Parse(Μ(З,1)),float.Parse(Μ(З,2)));
}String ΗЎ(Ψ Ω){String Ќ=":";String ή=κ(Ω.Δ)+Ќ+κ(Ω.δ)+Ќ+
κ(Ω.γ)+Ќ+κ(Ω.υ)+Ќ+κ(Ω.ν);for(int i=0;i<Ω.ǡ.Length;i++)
ή+=Ќ+Math.Round(Ω.ǡ[i],3);return ή;}Ψ Ηϊ(String s){String[]З=s.Split(':');Ψ Ω=new Ψ(
μ(Μ(З,0)),μ(Μ(З,1)),
μ(Μ(З,2)),μ(Μ(З,3)),
μ(Μ(З,4)));int Β=5;List<float>Ευ=new List<float>();while(Β<З.Length){String ΑБ=Μ(З,Β);float f=0;if(!float.TryParse(ΑБ,out f)) break;Ευ.Add(f);Β++;}Ω.ǡ=Ευ.ToArray();return Ω;}System.Text.StringBuilder Ŝ=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θφ){if(Θφ) Ŝ.Append('\n');Ŝ.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String ƴ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ǌ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Μ(String[]ΘΓ,int Β){String ΑБ=ΘΓ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑБ)) return "";
return ΑБ;}void Save(){if(Αώ||Η==Ζ.у){Storage="";return;}Storage=ΒΚ();}String ΒΚ(){Ŝ.Clear();Ŝ.Append(DATAREV+";");Ρ(κ(Αύ),false);Ρ(κ(Θ.δ));Ρ(κ(Θ.υ));Ρ(κ(Θ.γ));Ρ(κ(Θ.ν));Ρ(κ(Θ.Δ));Ρ(κ(Θ.Ъ));Ρ(Θ.ε);Ρ(κ(Ε.Δ));Ρ(κ(Ε.ν));Ρ(κ(Ε.δ));Ρ(κ(Ε.γ));Ρ(κ(Ε.υ));Ρ(κ(Ε.Ъ));Ρ(Ε.ε);Ρ(κ(С));Ρ(κ(ȅ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) ш);Ρ(Ǳ);Ρ(щ);Ρ(Ю);Ρ(ъ);Ρ(Ǫ);Ρ(ȥ);Ρ(ȣ);if(Η==Ζ.Ϋ){Ρ((int) ό.Ѝ);Ρ(ό.ǖ);Ρ(ό.Ȳ);Ρ(ƴ(ό.Ǘ));Ρ(ƴ(ό.ǚ));Ρ((int) ύ.Ѝ);Ρ(ύ.ǖ);Ρ(ύ.Ȳ);Ρ(ƴ(ύ.Ǘ));Ρ(ƴ(ύ.ǚ));}else{Ρ((int) Ƞ);Ρ((int) о);Ρ((int) П);Ρ((int) ц);Ρ((int) Ħ);Ρ(Č);Ρ(ĭ);Ρ(ŋ);Ρ(ń);Ρ(л);Ρ(к);Ρ(Щ);Ρ(Ŧ);Ρ(ť);Ρ(ı);Ρ(ģ);
Ρ(Е);Ρ(Э);Ρ(Ы);Ρ(Ь);Ρ(Ȝ);Ρ(М);Ρ(Ī);Ρ(Ĩ);}Ρ(";");for(int i=0;i<ȉ.Count;i++)
Ρ((i>0?"|":"")+ȉ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗЎ(Σ[i]),i>0);Ρ(";",false);Ρ(О+"|"+Ѓ,false);Ρ(";",false);Ρ(ΗΘ(),false);String Χ=Ŝ.ToString();Ŝ.Clear();return Χ;}ф Ηι(){return Ǆ(Storage);}ф Ǆ(String Χ){if(Χ=="") return ф.Ηβ;String[]ī=Χ.Split(';');if(Μ(ī,0)!=DATAREV) return ф.Βμ;int Β=0;try{String[]Ι=Μ(ī,1).Split('\n');
Αύ=μ(Μ(Ι,Β++));Θ.δ=μ(Μ(Ι,Β++));Θ.υ=μ(Μ(Ι,Β++));Θ.γ=μ(Μ(Ι,Β++));Θ.ν=μ(Μ(Ι,Β++));Θ.Δ=μ(Μ(Ι,Β++));Θ.Ъ=μ(Μ(Ι,Β++));Θ.ε=bool.Parse(Μ(Ι,Β++));Ε.Δ=μ(Μ(Ι,Β++));Ε.ν=μ(Μ(Ι,Β++));Ε.δ=μ(Μ(Ι,Β++));Ε.γ=μ(Μ(Ι,Β++));Ε.υ=μ(Μ(Ι,Β++));Ε.Ъ=μ(Μ(Ι,Β++));Ε.ε=bool.Parse(Μ(Ι,Β++));
С=μ(Μ(Ι,Β++));ȅ=μ(Μ(Ι,Β++));Ι=Μ(ī,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Μ(Ι,Β++));ί=(Λ) int.Parse(Μ(Ι,Β++));ш=(Λ) int.Parse(Μ(Ι,Β++));Ǳ=int.Parse(Μ(Ι,Β++));щ=int.Parse(Μ(Ι,Β++));Ю=int.Parse(Μ(Ι,Β++));ъ=int.Parse(Μ(Ι,Β++));Ǫ=float.Parse(Μ(Ι,Β++));ȥ=bool.Parse(Μ(Ι,Β++));
ȣ=bool.Parse(Μ(Ι,Β++));if(Η==Ζ.Ϋ){ό.Ѝ=(ι) int.Parse(Μ(Ι,Β++));ό.ǖ=float.Parse(Μ(Ι,Β++));ό.Ȳ=float.Parse(Μ(Ι,Β++));ό.Ǘ=ǌ(Μ(Ι,Β++));ό.ǚ=ǌ(Μ(Ι,Β++));ύ.Ѝ=(ι) int.Parse(Μ(Ι,Β++));ύ.ǖ=float.Parse(Μ(Ι,Β++));ύ.Ȳ=float.Parse(Μ(Ι,Β++));ύ.Ǘ=ǌ(Μ(Ι,Β++));ύ.ǚ=ǌ(Μ(Ι,Β++));}else{Ƞ=(ǒ) int.Parse(Μ(Ι,Β++));
о=(з) int.Parse(Μ(Ι,Β++));П=(Ѐ) int.Parse(Μ(Ι,Β++));ц=(я) int.Parse(Μ(Ι,Β++));Ħ=(ǒ) int.Parse(Μ(Ι,Β++));Č=bool.Parse(Μ(Ι,Β++));ĭ=bool.Parse(Μ(Ι,Β++));ŋ=bool.Parse(Μ(Ι,Β++));ń=bool.Parse(Μ(Ι,Β++));л=int.Parse(Μ(Ι,Β++));к=int.Parse(Μ(Ι,Β++));Щ=int.Parse(Μ(Ι,Β++));Ŧ=float.Parse(Μ(Ι,Β++));
ť=float.Parse(Μ(Ι,Β++));ı=float.Parse(Μ(Ι,Β++));ģ=float.Parse(Μ(Ι,Β++));Е=int.Parse(Μ(Ι,Β++));Э=int.Parse(Μ(Ι,Β++));Ы=int.Parse(Μ(Ι,Β++));Ь=int.Parse(Μ(Ι,Β++));Ȝ=int.Parse(Μ(Ι,Β++));М=int.Parse(Μ(Ι,Β++));Ī=int.Parse(Μ(Ι,Β++));Ĩ=float.Parse(Μ(Ι,Β++));}Ι=Μ(ī,3).Replace("\n","").Split('|');
ȉ=Ι.ToList();Ι=Μ(ī,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ηϊ(Μ(Ι,i)));Ι=Μ(ī,5).Split('|');О=Μ(Ι,0);if(!int.TryParse(Μ(Ι,1),out Ѓ)) Ѓ=-1;if(О.Length!=Е*Э){О="";Ѓ=-1;}ΗΗ(Μ(ī,6));}catch{return ф.Δς;}ŝ=ш;φ();return ф.ǉ;}const int Δζ=64000;IMyTerminalBlock Δσ(){List<IMyTerminalBlock>Ł=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(Ł,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return Ł.Count>0?Ł[0]:null;}void ΗΛ(){IMyTerminalBlock Υ=Δσ();if(Υ==null){Κ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΚ();if(Χ.Length>Δζ){Κ="Export too large: "+Χ.Length+" chars, max "+Δζ;return;}Υ.CustomData=Χ;Κ="Exported "+Σ.Count+" waypoints";}void ΗΦ(bool Γν){IMyTerminalBlock Υ=Δσ();if(Υ==null){Κ="Import: name a block "+dataTag;return;}
ΗΧ(Υ.CustomData,Γν);}void ΗΧ(String Χ,bool Γν){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΣ=Χ.IndexOf('\n');Χ=ΖΣ<0?"":Χ.Substring(ΖΣ+1);}Χ=Χ.Trim();if(Χ==""){Κ="Import: no PAM data in "+dataTag;return;}φ();String ΕΪ=ΒΚ();ф ή=Ǆ(Χ);if(ή==ф.ǉ&&Γν){Ψ ć=new Ψ(Ε);
Vector3 Θς=С,Θρ=ȅ;int w=л,h=к,d=Щ;ǒ ΔΖ=Ƞ;я ΓΣ=ц;float Θϊ=ı,Θω=ģ;Ǆ(ΕΪ);Ε=ć;С=Θς;ȅ=Θρ;л=w;к=h;Щ=d;Ƞ=ΔΖ;ц=ΓΣ;ı=Θϊ;ģ=Θω;ί=Λ.ĸ;Ŷ(true,true);ί=Λ.ĸ;ш=Λ.ĸ;Κ="Imported job, run CONT";return;}if(ή==ф.ǉ){Ɣ=false;Κ="Imported "+Σ.Count+" waypoints";
return;}Ǆ(ΕΪ);Κ=ή==ф.Βμ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ητ(){Ɓ.Clear();for(int i=і.Count-1;i>=0;i--){String Χ=і[i].CustomData.ToUpper();bool Γλ=false;if(Χ=="INSTRUCTIONS"){Γλ=true;ΑΣ=true;}if(Χ=="DEBUG") Γλ=true;if(Γλ){Ɓ.Add(і[i]);і.RemoveAt(i);}
}ų(Ɓ,false,1,false);}void Ηυ(List<IMyTerminalBlock>Ł){ċ.Clear();for(int i=0;i<Ł.Count;i++){IMyTerminalBlock Υ=Ł[i];try{String ΖΪ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Υ.CustomName.IndexOf(ΖΪ);int ş=-1;if(Β<0||!int.TryParse(Υ.CustomName.Substring(Β+ΖΪ.Length,1),out ş)) continue;
if(ş==-1) continue;ş--;IMyTextSurfaceProvider Ζά=(IMyTextSurfaceProvider) Υ;if(ş<Ζά.SurfaceCount&&ş>=0)
ċ.Add(Ζά.GetSurface(ş));}catch{}}}String Ατ="";void ΔЏ(){String Γψ="";String ΓΪ="";Ατ=Δε(false);Γψ+=Ατ;ΓΪ+=Ατ;ΓΪ+=ΗΟ();for(int i=0;i<і.Count;i++) і[i].WriteText(Γψ);for(int i=0;i<ċ.Count;i++) ċ[i].WriteText(Γψ);Echo(ΓΪ);for(int i=0;i<Ɓ.Count;i++){IMyTextPanel ΓЅ=Ɓ[i];
String Χ=ΓЅ.CustomData.ToUpper();if(Χ=="DEBUG") ΓЅ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓЅ.WriteText(ΗΠ());}}string ΗΠ(){String Γ="";try{float ΖΚ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(ŀ*ΖΚ)+" / "+(int)(ő*ΖΚ)+"\n";Γ+="Inst. avg/max: "+Math.Round(ŀ*100f,1)+"% / "+Math.Round(ő*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ź,2)+"ms / "+Math.Round(Γό,2)+"ms \n";}catch{}for(int i=0;i<Ȟ.Count;i++)
Γ+=""+Ȟ.Keys.ElementAt(i)+" = "+Math.Round(Ȟ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȟ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΤ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";ų(і,setLCDFontAndSize,0.9f,false);ų(ċ,setLCDFontAndSize,0.9f,true);for(int i=0;i<і.Count;i++) і[i].WriteText(Γ+ț.ToString());for(int i=0;i<ċ.Count;i++) ċ[i].WriteText(Γ+ț.ToString());}void ǻ(){if(!ΑΣ) return;try{ΓЉ=Runtime.CurrentInstructionCount;}catch{}
}void Ƿ(String ψ){if(!ΑΣ) return;if(ΓЉ==0) return;try{float ΑЏ=(Runtime.CurrentInstructionCount-ΓЉ)/Runtime.MaxInstructionCount*100;if(!Ȟ.ContainsKey(ψ))
Ȟ.Add(ψ,new float[]{ΑЏ,ΑЏ});else{Ȟ[ψ][0]=ΑЏ;Ȟ[ψ][1]=Math.Max(ΑЏ,Ȟ[ψ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void ΖІ(String Ā){if(!ł) return;String Ќ=""+BROADCAST_SEP;String ρ=VERSION+Ќ;ρ+=ŧ(ΓБ)+Ќ;
ρ+=(int) Η+Ќ;ρ+=ΒΪ(ά.X)+""+Ќ;ρ+=ΒΪ(ά.Y)+Ќ;ρ+=ΒΪ(ά.Z)+Ќ;if(Η!=Ζ.Ϋ)
ρ+=Βά(ш)+(ш==Λ.І?" "+Math.Round(ƍ,1)+"%":"")+Ќ;else ρ+=Βά(ш)+Ќ;ρ+=ŧ(Κ)+Ќ;ρ+=ǜ+""+Ќ;ρ+=ŧ(Ατ)+Ќ;ρ+=ƀ+Ќ;ρ+=Ƒ+Ќ;for(int i=0;i<Ǚ.Count;i++){if(Ǚ[i].ŏ!=Й.Ŭ) continue;ρ+=ŧ(Ǚ[i].ψ)+"/"+ŧ(Ǚ[i].σ)+"/"+Ǚ[i].ǔ+Ќ;}Ηρ(ρ,Ā);}String ŧ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηρ(String ρ,String Ā){Ης(ρ,BROADCAST_SELF_ID,Ā);}void Ης(String ρ,String Ћ,String Ā){try{if(!ł) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δφ()+BROADCAST_SEP+Ћ+BROADCAST_SEP+ΓΕ+BROADCAST_SEP+Ā+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){ț=e;}
}String Δφ(){if(Η!=Ζ.у) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔІ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool ũ(ref String Χ,out string Εφ,Char Ќ){int Β=Χ.IndexOf(Ќ);Εφ="";if(Β<0) return false;Εφ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool Ηγ(ref String ρ,out String Ɯ,out String Ā)
{Ɯ="";Ā="";if(!ł) return false;String ǟ="";if(!ũ(ref ρ,out ǟ,BROADCAST_SEP)||!ΔІ(ǟ)) return false;if(!ũ(ref ρ,out Ɯ,BROADCAST_SEP)) return false;if(!ũ(ref ρ,out ǟ,BROADCAST_SEP)||(ǟ!=Δφ()&&(ǟ!="*"&&Η!=Ζ.у))) return false;if(!ũ(ref ρ,out ǟ,BROADCAST_SEP)||(ǟ!=ΓΕ)) return false;
if(!ũ(ref ρ,out Ā,BROADCAST_SEP)) return false;return true;}void ΗΩ(){bool ž=true;if(Εψ){Εψ=false;if(Me.CustomData.Contains("Antenna_Name")){Κ="Update custom data";Me.CustomData="";}}String Ƌ=(Η!=Ζ.у?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ƌ)
ΔЂ(Ƌ);String[]Ι=Me.CustomData.Split('\n');ł=bool.Parse(Βφ(ΒΨ(Ι,"Enable_Broadcast",ref ž)));bool ΔΪ=false;bool Εν=true;if(ł){if(Η!=Ζ.у)
ΓБ=Βφ(ΒΨ(Ι,"Ship_Name",ref ž)).Replace(BROADCAST_SELF_ID,"");ΓΕ=Βφ(ΒΨ(Ι,"Broadcast_Channel",ref ž)).ToLower();Εν=false;if(ȓ==null){ȓ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȓ.SetMessageCallback("");}List<IMyRadioAntenna>Ľ=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(Ľ);bool ΒЊ=false;
for(int i=0;i<Ľ.Count;i++)
if(Ľ[i].EnableBroadcasting&&Ľ[i].Enabled){ΒЊ=true;break;}if(Ľ.Count==0) Κ="No Antenna found";else if(!ΒЊ) Κ="Antenna not ready";ΔΪ=Ľ.Count==0||!ΒЊ;if(ΕΣ&&!ΔΪ&&Η!=Ζ.у) Κ="Antenna ok";}else if(Η==Ζ.у)
Κ="Offline - Enable in PB custom data";ΕΣ=ΔΪ;if(Εν){if(ȓ!=null) this.IGC.DisableBroadcastListener(ȓ);ȓ=null;}}catch{ž=false;}if(!ž){Κ="Reset custom data";ΔЂ(Ƌ);}}void ΔЂ(String Ƌ){String Χ=Ƌ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.у?"true":"false")+" \n";Χ+=Η!=Ζ.у?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βφ(String ħ){int Β=ħ.IndexOf("//");if(Β!=-1) ħ=ħ.Substring(0,Β);String[]З=ħ.Split('=');if(З.Length<=1) return "";return З[1].Trim();}String ΒΨ(String[]Ι,String ō,ref bool ƕ){foreach(String ħ in Ι)
if(ħ.StartsWith(ō)) return ħ;ƕ=false;return "";}String ΕΑ(int ΔΨ,String Γ,int Ζα,ref int ȟ){String[]Ι=Γ.Split('\n');if(Ζα>=ȟ+ΔΨ-1) ȟ++;ȟ=Math.Min(Ι.Count()-1-ΔΨ,ȟ);if(Ζα<ȟ+1) ȟ--;ȟ=Math.Max(0,ȟ);String ή="";for(int i=0;i<ΔΨ;i++){int ΖΕ=i+ȟ;if(ΖΕ>=Ι.Count()) break;
ή+=Ι[ΖΕ]+"\n";}return ή;}String ΒΫ(int ƛ){if(ƛ>=60*60) return Math.Round(ƛ/(60f*60f),1)+" h";if(ƛ>=60) return Math.Round(ƛ/60f,1)+" min";return ""+ƛ+" s";}string ΒΪ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>Ǭ=new List<IMyCameraBlock>();List<long>ŕ=new List<long>();
bool ŗ=false;int ΓΛ=0,ΘЅ=0,ΓΓ=0;const int Ηλ=16;int ΓΙ=0;DateTime Εή=DateTime.Now;String ΓΔ="";String ΓΘ="";void Ηπ(){Ǭ.Clear();ŕ.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(Ǭ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<Ǭ.Count;i++) Ǭ[i].EnableRaycast=true;
List<IMyTerminalBlock>ΑΕ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ΑΕ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΑΕ.Count;i++) if(!ŕ.Contains(ΑΕ[i].CubeGrid.EntityId)) ŕ.Add(ΑΕ[i].CubeGrid.EntityId);if(!ŕ.Contains(Me.CubeGrid.EntityId)) ŕ.Add(Me.CubeGrid.EntityId);
ΓΘ="";if(Τ==null) return;MatrixD m=Τ.WorldMatrix;Vector3[]ΘΣ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Θτ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<Ǭ.Count;i++)
if(Vector3.Dot(Ǭ[i].WorldMatrix.Forward,ΘΣ[d])>0.7f){ΓΘ+=Θτ[d]+" ";break;}}bool ΔЅ(Α s){return s==Α.ǎ||s==Α.Ш||s==Α.Ё||
s==Α.ȍ||s==Α.Ķ||s==Α.ƶ||
(s==Α.Ǥ&&ј!=null);}void Ʋ(){ŗ=false;ΓΛ=0;ΓΔ="";}void ΗЈ(){if(!obstacleBrake||Ǭ.Count==0||!ΔЅ(Φ)){Ʋ();return;}Vector3 Ɗ=ς+Ȣ;Vector3 ǳ=Ɗ-ά;float Ǡ=ǳ.Length();if(Ǡ<1f){Ʋ();return;}Vector3 ζ=ǳ/Ǡ;Vector3 Ȥ=Τ!=null?(Vector3) Τ.GetShipVelocities().LinearVelocity:new Vector3();
if(Ȥ.Length()>2) ζ=Vector3.Normalize(Ȥ);Vector3 ΑЄ=Vector3.Cross(ζ,Math.Abs(ζ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΑЄ.Normalize();Vector3 ω=Vector3.Cross(ΑЄ,ζ);float Ђ=Math.Min(Ǡ+α*0.25f,Ȉ*obstacleLookAhead+α);int Εΰ=0;for(int k=0;k<2;k++){int p=k==0?0:ΘЅ++%8,e=p%4;
Vector3 Αω=k==0?new Vector3():(e==0?ΑЄ:e==1?-ΑЄ:e==2?ω:-ω)*α*0.5f;Vector3 Ņ=ά+ζ*(p>=4?Ђ*0.5f:Ђ)+Αω;IMyCameraBlock ΓΗ=ΖЋ(Ņ);if(ΓΗ==null){Δμ();continue;}Εΰ++;MyDetectedEntityInfo Ș=ΓΗ.Raycast(Ņ);bool ΙΡ=Ș.Type==MyDetectedEntityType.Planet||Ș.Type==MyDetectedEntityType.Asteroid;
bool ΖΤ=!Ș.IsEmpty()&&!ŕ.Contains(Ș.EntityId)&&!Ηή(Ș.EntityId)&&
Ș.Type!=MyDetectedEntityType.FloatingObject&&!(ΙΡ&&(k>0||Φ==Α.ǎ||
(Ș.HitPosition.HasValue&&Vector3D.Distance(Ș.HitPosition.Value,Ɗ)<α)));if(ΖΤ){if(!ŗ) Εή=DateTime.Now;ŗ=true;ΓΛ=0;ΓΔ=Ș.Type.ToString();}else Δμ();if(debugDraw&&ΑΤ.Count<16) ΑΤ.Add(κ(ΓΗ.GetPosition())+","+κ(Ņ)+","+(ΖΤ?1:0));}ΓΓ=Εΰ>0?0:ΓΓ+1;if(ΓΓ>12) Κ="Cameras can't see ahead";
if(ŗ&&(DateTime.Now-Εή).TotalSeconds>obstacleTimeout)
Κ="Path blocked: "+ΓΔ;}void Δμ(){if(ŗ&&++ΓΛ>=Ηλ) Ʋ();}IMyCameraBlock ΖЋ(Vector3 Ņ){IMyCameraBlock Ǿ=null;float Εέ=-2;for(int n=0;n<Ǭ.Count;n++){ΓΙ=(ΓΙ+1)%Ǭ.Count;IMyCameraBlock c=Ǭ[ΓΙ];if(!c.IsFunctional||!c.IsWorking) continue;float Ζί=Vector3.Dot(Vector3.Normalize(Ņ-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(Ζί>Εέ+0.05f&&c.CanScan(Ņ)){Ǿ=c;Εέ=Ζί;}}return Ǿ;}const String ΕΚ="[PAMTFC]";const float Ηύ=5f;const float ΒΧ=2f;class Ȫ{public long χ;public Vector3 λ,Ȥ;public Vector3 ƌ;public float Εμ;public String ć="";public int Γθ=-1;public bool ΓЊ;public int Αθ;public int Ƙ;public float Θά,ΘΨ;
public long έ=0;public int ƃ=0;public DateTime ΔΝ;}IMyBroadcastListener ΑЎ=null;long н=0;List<Ȫ>п=new List<Ȫ>();Vector3 Ȣ=new Vector3();float э=-1;Vector3 ΔΡ=new Vector3();bool ΑЍ=false;DateTime ΔΣ=DateTime.Now;const float ΕΙ=15f;const float Ηό=10f;int Ζό=0;String О="";int Ѓ=-1;
bool ΑΝ=false,ΑΜ=false,ƈ=false;Vector3 Εγ=new Vector3();String ĕ(){if(ί==Λ.в||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Е+"|"+Э+"|"+(int) Ħ+"|"+Math.Round(ѝ,1)+"|"+Math.Round(Ǣ,1);}void ΗЋ(){if(!trafficEnabled) return;if(ΑЎ==null) ΑЎ=IGC.RegisterBroadcastListener(ΕΚ);н=Me.CubeGrid.EntityId;int Ğ=30;while(ΑЎ.HasPendingMessage&&Ğ-->0){String Χ=ΑЎ.AcceptMessage().Data as String;if(Χ!=null) Ηζ(Χ);
}for(int i=п.Count-1;i>=0;i--)
if((DateTime.Now-п[i].ΔΝ).TotalSeconds>Ηύ) п.RemoveAt(i);if(ΑЊ==null) ΑЊ=IGC.RegisterBroadcastListener(Ηϋ);Ğ=5;while(ΑЊ.HasPendingMessage&&Ğ-->0){String Χ=ΑЊ.AcceptMessage().Data as String;if(Χ!=null) try{Ηη(Χ);}catch{}}if(ΑΞ==null) ΑΞ=IGC.RegisterBroadcastListener(Ηί);
Ğ=10;while(ΑΞ.HasPendingMessage&&Ğ-->0){String Χ=ΑΞ.AcceptMessage().Data as String;if(Χ!=null) try{Ηδ(Χ);}catch{}}if(++Ζό<3) return;Ζό=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",н+";"+κ(Ε.Δ)+";"+
κ(ί==Λ.в?new Vector3():Ε.υ)+";"+κ(ȅ)+";"+
κ(ς+Ȣ)+";"+String.Join(":",ΑΤ));ΑΤ.Clear();}IGC.SendBroadcastMessage(ΕΚ,н+";"+κ(ά)+";"+
κ(Τ.GetShipVelocities().LinearVelocity)+";"+α+";"+ĕ()+";"+
(ί==Λ.І?Ѓ:-1)+";"+О+";"+ΖБ()+","+Δρ()+";"+κ(ΔΡ)+
";"+(ΑΧ?1:0)+";"+(ā&&ί==Λ.І?1:0)+","+ΑЁ+","+ΓА+","+Ηΰ()+","+ΕΛ());}void Ηζ(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long χ=long.Parse(f[0]);if(χ==н) return;Ȫ Є=null;for(int i=0;i<п.Count;i++) if(п[i].χ==χ) Є=п[i];if(Є==null){Є=new Ȫ();Є.χ=χ;п.Add(Є);}
Є.λ=μ(f[1]);Є.Ȥ=μ(f[2]);Є.Εμ=float.Parse(f[3]);Є.ć=f[4];Є.Γθ=int.Parse(f[5]);Є.ΔΝ=DateTime.Now;if(Є.ć!=""&&Є.ć==ĕ()) ΔЌ(f[6]);Є.έ=0;Є.ƃ=0;if(f.Length>=8){String[]d=f[7].Split(',');Є.έ=long.Parse(d[0]);Є.ƃ=int.Parse(d[1]);}Є.ƌ=f.Length>=9?μ(f[8]):new Vector3();
Є.ΓЊ=f.Length>=10&&f[9]=="1";Є.Αθ=0;Є.Ƙ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Є.Θά=float.Parse(w[1]);Є.ΘΨ=float.Parse(w[2]);Є.Αθ=int.Parse(w[3]);if(w.Length>=5) Є.Ƙ=int.Parse(w[4]);}}}catch{}}bool Ηή(long χ){for(int i=0;i<п.Count;i++) if(п[i].χ==χ) return true;
return false;}Vector3 Δϊ(int Β,int Θϋ){if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΞ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θϋ;Vector3 ω=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.γ;Vector3 Я=Vector3.Cross(ΙΞ,ω);
if(Я.LengthSquared()<0.0001f) return new Vector3();float Ђ=laneOffset*α;float ΘΪ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Я)*Ђ*Math.Min(1,ΘΪ/(Ђ*4));}Vector3 Ζϋ=new Vector3();List<String>ΑΤ=new List<String>();bool ΔϏ(){Α s=Φ;return ĕ()!=""&&(s==Α.ǎ||s==Α.Ė||s==Α.ǧ||
s==Α.Ǒ||s==Α.ē);}float ΔЈ(){return 0.5f*(float) Math.Sqrt(Math.Pow(Е*ѝ,2)+Math.Pow(Э*Ǣ,2));}bool Βλ(){return ĕ()!=""&&Vector3.Distance(ά,Ε.Δ)<ΔЈ()+α*2;}int ΕΛ(){Α s=Φ;if(s==Α.Ǒ||s==Α.ȍ||(s==Α.Ё&&Βλ())) return 2;return s==Α.ǎ||s==Α.ǧ||s==Α.ē||s==Α.Ш?1:0;
}int Ηΰ(){if(!ΔϏ()) return 0;return Φ!=Α.ǎ&&Vector3.Dot(ά-Б,С)>α*0.5f?2:1;}bool Ηέ(Α s){return s==Α.Ǥ||s==Α.Ď||s==Α.Ē||s==Α.Ų||
s==Α.Ǔ||s==Α.ǧ||s==Α.Ǒ||s==Α.ē||s==Α.Ȧ;}void ΗЌ(){Ȣ=new Vector3();э=-1;ΔΡ=new Vector3();bool Εϋ=ΔЅ(Φ);if(!trafficEnabled||(!Εϋ&&!Ηέ(Φ))){ΑЍ=false;return;}Ζϋ=Τ.GetShipVelocities().LinearVelocity;ΗΑ(Τ.GetNaturalGravity(),!Εϋ);ΖЄ();}IMyBroadcastListener ΑЊ=null;
float š=-1;DateTime Ζχ=DateTime.MinValue;void Ηη(String Χ){if(!Χ.StartsWith("C;")) return;Ζχ=DateTime.Now;š=-1;String ƒ=н+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƒ)) š=float.Parse(e[i].Substring(ƒ.Length));}void ΖЄ(){if(š<0||(DateTime.Now-Ζχ).TotalSeconds>1.5) return;
э=э<0?š:Math.Min(э,š);if(š<0.5f) Κ="Traffic: giving way (controller)";}void ΗΑ(Vector3 ν,bool ΖΩ=false){Ȣ=new Vector3();э=-1;Vector3 ǳ=ς-ά;float Ǡ=ǳ.Length();bool Γζ=Ǡ>=1f;Vector3 ζ=Γζ?ǳ/Ǡ:new Vector3();ΔΡ=ζ;Vector3 ω=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-ƅ;
Vector3 Я=Vector3.Cross(ζ,ω);Я=Я.LengthSquared()<0.0001f?-Ə:Vector3.Normalize(Я);Ȫ Ακ=null;bool Εύ=Φ==Α.Ё&&!ȑ(1);int ΓϏ=ΕΛ();for(int i=0;i<п.Count;i++){Ȫ ΰ=п[i];if(ΰ.ƃ==3&&ΰ.Ȥ.LengthSquared()<0.25f) continue;Vector3 λ=ΰ.λ+ΰ.Ȥ*(float)(DateTime.Now-ΰ.ΔΝ).TotalSeconds;
Vector3 ř=λ-ά;float ȕ=(α+ΰ.Εμ)*0.5f;bool Ζή=ΰ.ć!=""&&ΰ.ć==ĕ();if(Ζή&&ΰ.Αθ==2) continue;bool ΑЅ=Ζή&&ΰ.Αθ==1&&ΔϏ();if(ΑЅ) ȕ=Math.Min(ѝ,Ǣ)*0.9f;float ΓΟ=ȕ*(ΑЅ?1f:1.2f);float Ȭ=Γζ?Vector3.Dot(ř,ζ):0;if(Ȭ>0&&(ř-ζ*Ȭ).Length()<=ΓΟ){float Αϊ=Vector3.Dot(ΰ.Ȥ,ζ);
bool ΕЃ=ΰ.ƌ.LengthSquared()>0.25f;bool ΕЄ=Αϊ<-1f||(ΕЃ&&Vector3.Dot(ΰ.ƌ,ζ)<-0.5f);if(!ΖΩ&&ΰ.Ƙ<=ΓϏ){if(ΰ.ΓЊ){if(!Εύ){Ȣ=Я*ȕ*1.3f;continue;}float ΖΟ=Vector3.Distance(ά,ς),ΕА=Vector3.Distance(λ,ς);if(ΖΟ<ΕА||(ΖΟ==ΕА&&н<ΰ.χ)) continue;}if(ΕЄ){if(Ȭ<(Ȉ-Αϊ)*6f+ȕ*4f) Ȣ=Я*ȕ*1.3f;
continue;}if(ΕЃ&&н<ΰ.χ){Vector3 ΕΩ=-ř;float ΖΧ=Vector3.Dot(ΕΩ,ΰ.ƌ);if(ΖΧ>0&&(ΕΩ-ΰ.ƌ*ΖΧ).Length()<=ΓΟ){Ȣ=Я*ȕ*1.3f;continue;}}}float Εό=Ȭ-ȕ-(ΑЅ?1f:followGap*α);float Γς=Math.Max(0,Αϊ)+Math.Max(0,Εό)/ΒΧ;if(ΕЄ) Γς=Math.Max(0,Εό+Math.Min(0,Αϊ)*ΒΧ*2)/ΒΧ;
if(э<0||Γς<э){э=Γς;Ακ=ΰ;}continue;}if(ΰ.Ȥ.LengthSquared()<1f) continue;if(Γζ&&(ř-ζ*Ȭ).Length()<=ΓΟ) continue;Vector3 ΓЏ=ΰ.Ȥ-Ζϋ;float Ζέ=ΓЏ.LengthSquared();if(Ζέ<0.25f) continue;float t=-Vector3.Dot(ř,ΓЏ)/Ζέ;if(t<=0||t>5f||(ř+ΓЏ*t).Length()>=(ΑЅ?ȕ:ȕ*1.1f)) continue;
if(ΰ.Ƙ>ΓϏ||(ΰ.Ƙ==ΓϏ&&н>ΰ.χ)||t<2f){э=0;Ακ=ΰ;}}bool Θβ=э>=0&&э<0.5f;if(!Θβ||ΖΩ||(Ακ!=null&&Ακ.ΓЊ&&Εύ)){ΑЍ=false;return;}if(!ΑЍ){ΑЍ=true;ΔΣ=DateTime.Now;}double ΕЅ=(DateTime.Now-ΔΣ).TotalSeconds;if(ΕЅ>ΕΙ+Ηό) ΔΣ=DateTime.Now;else if(ΕЅ>ΕΙ){э=-1;Ȣ=Я*α*1.3f;Κ="Traffic: passing a stuck ship";}
}List<int>ȃ=new List<int>();String ΕЇ="";void ΒΦ(){String ō=Е+"|"+Э+"|"+(int) Ħ;if(ō==ΕЇ) return;ΕЇ=ō;ΖЇ(Е,Э,Ħ,ȃ);}void ΖЇ(int w,int h,ǒ ΔΖ,List<int>ΓΚ){ΓΚ.Clear();if(ΔΖ==ǒ.Ǌ){for(int k=0;k<w*h;k++){int Ś=k/w;ΓΚ.Add(Ś*w+(Ś%2==0?k-Ś*w:w-1-(k-Ś*w)));
}return;}int ΕЂ=(int) Math.Ceiling(w/2f),ΕЁ=(int) Math.Ceiling(h/2f);int Θή=w/2,Θέ=h/2;int x=0,y=0,ΓΨ=0,ΓΩ=-1,Απ=Math.Max(w,h);for(int i=0;i<Απ*Απ;i++){if(-ΕЂ<x&&x<=Θή&&-ΕЁ<y&&y<=Θέ) ΓΚ.Add((y-1+ΕЁ)*w+(x-1+ΕЂ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΨ;ΓΨ=-ΓΩ;ΓΩ=t;}
x+=ΓΨ;y+=ΓΩ;}}bool ΒΥ(){int n=Math.Max(0,Е*Э);if(О.Length==n) return false;О=new String('0',n);Ѓ=-1;return true;}void ΔЌ(String ΰ){ΒΥ();if(ΰ.Length!=О.Length) return;char[]h=О.ToCharArray();for(int i=0;i<h.Length;i++) if(ΰ[i]=='1') h[i]='1';О=new String(h);}bool ƻ(int Ȕ){return Ȕ>=0&&Ȕ<О.Length&&О[Ȕ]=='1';}
void Βό(int Ȕ,bool є){if(Ȕ<0||Ȕ>=О.Length) return;char[]h=О.ToCharArray();h[Ȕ]=є?'1':'0';О=new String(h);}bool Βδ(int Ȕ,bool Θθ){if(!trafficEnabled) return false;String ō=ĕ();for(int i=0;i<п.Count;i++){Ȫ t=п[i];if(t.ć!=ō||t.Γθ<0||(Θθ&&t.χ>н)) continue;if(t.Γθ==Ȕ) return true;
}return false;}int ΖА(int Β){int n=Е*Э;if(n<1) return Β;ΒΦ();if(ΒΥ()) for(int k=0;k<Β&&k<n;k++) Βό(ȃ[k],true);if(Ѓ>=0&&(Β>=n||ȃ[Β]!=Ѓ)){Βό(Ѓ,true);Ѓ=-1;}if(ΔЇ()){if(Ǯ>=0&&!ƻ(Ǯ)){ā=false;Ѓ=Ǯ;return ȃ.IndexOf(Ǯ);}if(Ǯ==-2){Ѓ=-1;ā=О.IndexOf('0')>=0;return ā?-2:n;}
if(Ǯ==-1&&Ѓ>=0&&!ƻ(Ѓ)){ā=false;return Β;}Ѓ=-1;if(!ā){ā=true;Εε=DateTime.Now;}if((DateTime.Now-Εε).TotalSeconds<5) return-1;}ā=false;if(Ѓ>=0&&!ƻ(Ѓ)&&!Βδ(Ѓ,true)) return Β;Ѓ=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(ƻ(c)||Βδ(c,false)) continue;Ѓ=c;
return k;}return О.IndexOf('0')>=0?-2:n;}const String Ηί="[PAMJOB]";const String Ηϋ="[PAMTCC]";IMyBroadcastListener ΑΞ=null;DateTime Εδ=DateTime.MinValue;DateTime Εε=DateTime.Now;int Ǯ=-1;bool ā=false;bool ΔЇ(){return trafficEnabled&&Ǯ!=-3&&(DateTime.Now-Εδ).TotalSeconds<3;}
void Ηδ(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ĕ()) return;Εδ=DateTime.Now;ΔЌ(f[2]);Ǯ=-3;String ƒ=н+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(ƒ)) Ǯ=int.Parse(e[i].Substring(ƒ.Length));}ĺ Ησ(int Β)
{ΒΦ();if(Β<0||Β>=ȃ.Count) return ĺ.Δγ;Ы=ȃ[Β]%Е;Ь=ȃ[Β]/Е;return ĺ.Δτ;}class ѐ{public long χ;public Vector3 λ,Г,ω;}List<ѐ>А=new List<ѐ>();bool ȯ=false;Vector3 ΑΩ,ΑΨ,ΑΪ,ΑΫ;long ч=0;long Ȇ=0;Ψ ј=null;bool Ƅ=false;Ψ ΒΡ(Vector3 λ,Vector3 Г,Vector3 ω){MatrixD m=MatrixD.CreateWorld(λ,Г,ω);
Ψ w=new Ψ(Θ);w.δ=Vector3D.TransformNormal(ΑΩ,m);w.γ=Vector3D.TransformNormal(ΑΨ,m);w.υ=Vector3D.TransformNormal(ΑΪ,m);w.Δ=λ+(Vector3) Vector3D.TransformNormal(ΑΫ,m);w.ε=true;return w;}int Δρ(){Α s=Φ;if(s==Α.й) return Т(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.Ē||s==Α.Ǽ||s==Α.đ||s==Α.ΑΒ||
s==Α.Ƶ||s==Α.Ǔ) return 3;if(s==Α.Ǥ||s==Α.Ď||s==Α.Ų) return 2;if(s==Α.Ё) return 1;return 0;}long ΖБ(){if(Δρ()==0) return 0;if(ȑ(1)&&Ş!=0) return Ş;return ј!=null?Ȇ:ч;}void ΔЉ(){if(Η==Ζ.Ϋ||!Θ.ε) return;IMyShipConnector Л=Т(MyShipConnectorStatus.Connected);if(Л==null||Л.OtherConnector==null) return;
IMyShipConnector ΰ=Л.OtherConnector;Vector3I Θγ=new Vector3I((int) Θ.Ъ.X,(int) Θ.Ъ.Y,(int) Θ.Ъ.Z);if(Л.Position==Θγ){MatrixD Ŋ=MatrixD.Transpose(ΰ.WorldMatrix);ΑΩ=Vector3D.TransformNormal(Τ.WorldMatrix.Forward,Ŋ);ΑΨ=Vector3D.TransformNormal(Τ.WorldMatrix.Down,Ŋ);ΑΪ=Vector3D.TransformNormal(Τ.WorldMatrix.Left,Ŋ);
ΑΫ=Vector3D.TransformNormal(Л.GetPosition()-ΰ.GetPosition(),Ŋ);ȯ=true;}if(Vector3.Distance(Л.GetPosition(),Θ.Δ)<5) ч=ΰ.EntityId;List<IMyShipConnector>Ģ=new List<IMyShipConnector>();ϊ.GetBlocksOfType(Ģ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ģ.Count>0){А.Clear();for(int i=0;i<Ģ.Count;i++){ѐ p=new ѐ();p.χ=Ģ[i].EntityId;p.λ=Ģ[i].GetPosition();p.Г=Ģ[i].WorldMatrix.Forward;p.ω=Ģ[i].WorldMatrix.Up;А.Add(p);}}ј=null;Ȇ=0;if(ΰ.EntityId==ч||!ȯ) return;for(int i=0;i<А.Count;i++)
if(А[i].χ==ΰ.EntityId){Ȇ=А[i].χ;ј=ΒΡ(А[i].λ,А[i].Г,А[i].ω);}}bool Βη(long χ,int ΖΠ){if(χ==0) return false;for(int i=0;i<п.Count;i++){Ȫ t=п[i];if(t.έ==χ&&(t.ƃ>ΖΠ||(t.ƃ==ΖΠ&&t.χ<н))) return true;}return false;}bool ΒΜ(int ΓЇ){if(Η==Ζ.Ϋ||!trafficEnabled){ј=null;Ȇ=0;return true;}
long θ=ј!=null?Ȇ:ч;if(!Βη(θ,ΓЇ)) return true;if(!Βη(ч,ΓЇ)){ј=null;Ȇ=0;return true;}if(!ȯ) return false;ѐ Ǿ=null;float Εά=0;for(int i=0;i<А.Count;i++){ѐ p=А[i];if(p.χ==ч||Βη(p.χ,ΓЇ)) continue;float d=Vector3.Distance(p.λ,Θ.Δ);if(Ǿ==null||d<Εά){Ǿ=p;Εά=d;}
}if(Ǿ==null) return false;Ȇ=Ǿ.χ;ј=ΒΡ(Ǿ.λ,Ǿ.Г,Ǿ.ω);return true;}String ΗΘ(){String s=(ȯ?"1":"0")+":"+κ(ΑΩ)+":"+κ(ΑΨ)+":"+
κ(ΑΪ)+":"+κ(ΑΫ)+":"+ч;for(int i=0;i<А.Count;i++)
s+=":"+А[i].χ+","+κ(А[i].λ)+","+κ(А[i].Г)+","+κ(А[i].ω);return s;}void ΗΗ(String s){А.Clear();ȯ=false;ч=0;String[]f=s.Split(':');if(f.Length<6) return;ȯ=f[0]=="1";ΑΩ=μ(f[1]);ΑΨ=μ(f[2]);ΑΪ=μ(f[3]);ΑΫ=μ(f[4]);long.TryParse(f[5],out ч);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();if(!long.TryParse(e[0],out p.χ)) continue;p.λ=μ(e[1]);p.Г=μ(e[2]);p.ω=μ(e[3]);А.Add(p);}}const String ΒΞ="[PAMDCK]";IMyBroadcastListener ΑЈ=null;int Ȃ=0;DateTime Επ=DateTime.Now;DateTime Ζθ=DateTime.MinValue;long Ζι=0;
int Ζκ=0;long Ş=0;bool Ʃ=false;int ΓΧ=0;List<Vector3>ƫ=new List<Vector3>();List<float>Δα=new List<float>();bool ΑΧ=false;bool ΑΦ=false;Vector3 Εο=new Vector3();bool ŵ(){return Ȃ!=0&&(DateTime.Now-Ζθ).TotalSeconds<3;}bool ȑ(int Αι){return ŵ()&&Ζι==н&&Ζκ==Αι;}bool ΕΕ(){return ŵ()||(DateTime.Now-Επ).TotalSeconds<3;}
void Βϊ(int Αι){if(Ȃ==Αι) return;Ȃ=Αι;Επ=DateTime.Now;ΓΧ=3;}void Βω(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΞ,"D;"+н);Ȃ=0;Ʃ=false;}void ΗЃ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑЈ==null) ΑЈ=IGC.RegisterBroadcastListener(ΒΞ);int Ğ=20;while(ΑЈ.HasPendingMessage&&Ğ-->0)
{String Χ=ΑЈ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;try{Ηε(Χ);}catch{}}if(Φ!=Α.Ё){ΑΧ=false;ΑΦ=false;}if(Ȃ==1&&Т(MyShipConnectorStatus.Connected)!=null) Βω();if(Ȃ==2&&!Ʃ&&Φ!=Α.Ǔ&&
Vector3.Distance(ά,и().Δ)>α*3) Βω();if(Ȃ!=0&&++ΓΧ>=3){ΓΧ=0;IGC.SendBroadcastMessage(ΒΞ,"R;"+н+";"+Ȃ+";"+ч+";"+(ȯ?1:0));}}void Ηε(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Εϊ=false;String[]ΕΞ=f[5].Split(',');for(int i=0;i<ΕΞ.Length;i++) if(ΕΞ[i]==н.ToString()) Εϊ=true;
if(!Εϊ) return;Ζθ=DateTime.Now;Ζι=long.Parse(f[2]);Ζκ=int.Parse(f[3]);Ş=long.Parse(f[4]);ƫ.Clear();Δα.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ƫ.Add(μ(e[0]));Δα.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Ες=f[6].Split(':');List<ѐ>Ф=new List<ѐ>();for(int i=0;i<Ες.Length;i++){String[]e=Ες[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();p.χ=long.Parse(e[0]);p.λ=μ(e[1]);p.Г=μ(e[2]);p.ω=μ(e[3]);Ф.Add(p);}А=Ф;}bool ΖЎ(){if(!trafficEnabled||Η==Ζ.Ϋ||ч==0) return true;
Βϊ(1);if(!ΕΕ()) return true;if(!ȑ(1)){Κ="Waiting for dock clearance";return false;}ΒΕ();return true;}bool ΒΕ(){Ƅ=false;ѐ ĩ=null;if(Ş!=0&&Ş!=ч&&ȯ)
for(int i=0;i<А.Count;i++) if(А[i].χ==Ş) ĩ=А[i];long ΔΩ=ĩ==null?0:ĩ.χ;if(ΔΩ==Ȇ&&(ΔΩ==0)==(ј==null)) return false;Ȇ=ΔΩ;ј=ĩ==null?null:ΒΡ(ĩ.λ,ĩ.Г,ĩ.ω);return true;}bool ķ(Vector3 p){for(int i=0;i<ƫ.Count;i++) if(Vector3.Distance(p,ƫ[i])<Δα[i]+α*0.75f) return true;
return false;}bool ΗΥ(Vector3 ſ,Vector3 œ){ΑΧ=false;if(!trafficEnabled||Η==Ζ.Ϋ||ч==0) return false;if(Ȃ!=1&&Vector3.Distance(ά,Θ.Δ)>dockRequestDistance) return false;Βϊ(1);if(!ŵ()||ȑ(1)||ƫ.Count==0) return false;if(!ķ(ſ)&&!ķ(œ)&&!ķ(ά)) return false;ΑΧ=true;return true;
}bool Δκ(){if(!trafficEnabled||Η==Ζ.Ϋ||ч==0) return true;Βϊ(2);if(!ΕΕ()) return true;if(!ȑ(2)){Κ="Waiting for undock clearance";return false;}return true;}void ΗΕ(string ђ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}