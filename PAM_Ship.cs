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

//Debug drawing in 3D (offline worlds, needs Digi's "(DevTool) Programmable Block DebugAPI" mod):
//on the [PAM-Controller] PB it draws ships, traffic, docking area and hole grids; on the ships it
//sends their flight target, camera rays and job grid to the controller to draw. Off on a server.
const bool debugDraw = false;

// ===== End of settings: don't edit below this line =====
public enum Ζ{ΕΔ,Ű,Ǎ,т,Ϋ}public enum Α{з,ǣ,Ķ,ǹ,Ǹ,Ѕ,Э,
ȩ,ĳ,ǡ,đ,č,ȥ,
ű,Ǽ,ǐ,Đ,ǋ,Ʋ,
ΒЁ,Ǌ,Ĳ,Ƴ,ȫ}public enum Λ{Я,ķ,Є,ΒΝ,ƭ,Ƽ,ƽ,Į,į}public enum с{None,Đ,з,Δκ}public enum Ϊ{Main,ΔЊ,ů,Ů,Βύ,ƫ,
ΑЏ,Δϊ,Δϋ,Δψ,Ηξ,ΙΪ,
ΙΨ,ΙΩ,Βψ,ΔЎ,ǅ}public enum а{ǃ,ŧ,Ηπ,Δύ,Ϋ}public enum Ǐ{Ǉ,ΖЄ}public enum е{ǂ,ΒΤ,ΔА,Ū}public enum ώ{Βη,Δη,ΒΘ,ΒΞ,ư,Βΰ,Ʒ}public enum я{ΒΚ,ΒΓ,ΒΔ}public enum З{ū,ΗΟ,Ď}public enum ι{Βι,Βν,Βξ,ΔЇ,ΔІ,
Βθ,ǀ,ƿ,Βλ,
Βμ,Βκ}public enum ΰ{Ǵ,ȍ,ǳ,ƪ,ΒΑ,ΒΒ}public enum Ĺ{Δξ,Δέ,ΙΧ}public enum у{ǆ,Δμ,ΗΪ,Βζ}public enum σ{Ŷ,Ʃ,ƺ,Ū,Βδ,ƻ,Βε,Ηφ,Βο}class Ψ{public bool ε=false;public Vector3 Δ=new Vector3();public Vector3 α=new Vector3();public Vector3 β=new Vector3();public Vector3 τ=new Vector3();
public Vector3 ν=new Vector3();public Vector3 Ч=new Vector3();public float Џ=0;public float Ιέ=0;public float[]Ǟ=null;public Ψ(){}public Ψ(Ψ γ){ε=γ.ε;Δ=γ.Δ;α=γ.α;β=γ.β;τ=γ.τ;ν=γ.ν;Ч=γ.Ч;Ǟ=γ.Ǟ;}public Ψ(Vector3 Δ,Vector3 β,Vector3 α,Vector3 τ,Vector3 ν){this.Δ=Δ;this.α=α;
this.β=β;this.τ=τ;this.Џ=0;this.ν=ν;}public void ΖЀ(List<IMyThrust>ΑΓ,List<string>Ȉ){Ǟ=new float[Ȉ.Count];for(int i=0;i<Ǟ.Length;i++)
Ǟ[i]=-1;for(int i=0;i<ΑΓ.Count;i++){string ю=Δφ(ΑΓ[i]);int Β=Ȉ.IndexOf(ю);if(Β!=-1)
Ǟ[Β]=А(ΑΓ[i].MaxEffectiveThrust,ΑΓ[i].MaxThrust);}}}class Βρ{public Ψ Њ=null;public List<Vector3>ş=new List<Vector3>();public float Ƃ=0;public float ΔΟ=0;public float ľ=0;public float ΓΨ=0;public Vector3 ΑΨ=new Vector3();}class ĺ{public ι Ќ=ι.Βι;public float Ǔ=0;public float ȱ=0;
public string ǔ="";public string Ǘ="";DateTime ΕЋ;public bool ΓЂ=false;private bool ΓΛ=false;public bool ΒΖ(bool ȇ){if(ȇ){ȱ=0;ΓΛ=false;return false;}ΓΛ=true;return ȱ>Ǔ;}public void Ηΰ(){ΒΖ(true);ΓЂ=false;}public void ΗЂ(){if(ΓΛ)
if((DateTime.Now-ΕЋ).TotalSeconds>1){ȱ++;ΕЋ=DateTime.Now;}}public bool Βέ(){switch(Ќ){case ι.Βν:return true;case ι.Βξ:return true;}return false;}}class Ǥ{public String ψ="";public String ς="";public int Ǒ=0;public З Ŏ=З.Ď;public Ǥ(String ψ,String ς,int Ǒ,З Ŏ){this.ψ=ψ;this.ς=ς;
this.Ǒ=Ǒ;this.Ŏ=Ŏ;}}IMyGridTerminalSystem ϊ;Vector3 Αψ=new Vector3();Vector3 ή=new Vector3();Vector3 ΑΫ=new Vector3();Vector3 Ǝ=new Vector3();Vector3 ƅ=new Vector3();DateTime ΕЊ=new DateTime();bool ƈ=true;int ΔΖ=0;bool ą=false;bool Αγ=false;bool Ɠ=true;bool Ζϊ=false;bool ŋ=false;
bool Αΰ=false;float Ȟ=0;float Ĩ=0;int ĉ=0;int Ƣ=0;int Αϋ=0;int ΒБ=0;float Ő=0;float Ŀ=0;double Γχ=0;float ź=0;List<int>ň=new List<int>();List<int>Ś=new List<int>();Ζ Η=Ζ.ΕΔ;int ύ=0;float ǟ=0;float ǲ=0;float δ=0;float Αό=0,ΓЊ=0;IMyRemoteControl Τ;IMySensorBlock ξ;
List<IMyTimerBlock>ΔΙ=new List<IMyTimerBlock>();List<IMyShipConnector>ф=new List<IMyShipConnector>();List<IMyThrust>ȉ=new List<IMyThrust>();List<IMyGyro>Αά=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();List<IMyLandingGear>Ȅ=new List<IMyLandingGear>();List<IMyReactor>Ĉ=new List<IMyReactor>();
List<IMyConveyorSorter>ΑЁ=new List<IMyConveyorSorter>();List<IMyGasTank>ѕ=new List<IMyGasTank>();List<IMyTerminalBlock>ŷ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ł=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ę=new List<IMyTerminalBlock>();List<IMyBatteryBlock>Ȭ=new List<IMyBatteryBlock>();
List<IMyTextPanel>і=new List<IMyTextPanel>();List<IMyTextSurface>Ċ=new List<IMyTextSurface>();List<IMyTextPanel>Ɓ=new List<IMyTextPanel>();IMyTerminalBlock љ=null;Ψ Θ=new Ψ();Ψ Γδ=new Ψ();Ψ Γη=new Ψ();bool Ȋ=false;bool Ţ=false;bool ţ=false;bool Αβ=false;List<String>Ȉ=new List<string>();
List<Ψ>Σ=new List<Ψ>();List<Ψ>Ƨ=new List<Ψ>();int ΓΠ=0;double Ζϋ=0;Ψ Ε=new Ψ();Vector3 ъ;Vector3 ȅ;Λ ί=Λ.Я;Ǐ Ģ=Ǐ.Ǉ;int Н=0;int в=0;double ƌ=0;bool ΑΝ=false;ĺ ϋ=new ĺ();ĺ ό=new ĺ();е н=е.ǂ;bool ċ=true;Ǐ ȟ=Ǐ.Ǉ;int й=3;int и=3;int Ц=30;я х=я.ΒΚ;bool Ȥ=true;bool Ŋ=true;
bool Ń=true;bool ĩ=false;ώ М=ώ.Βη;bool Ȣ=true;float Ǯ=90;float ш=20;float Ъ=5;float щ=20;float ǧ=0.70f;float ť=1.50f;float Ť=2.50f;float ĭ=0f;float ğ=0f;Α Φ;Λ ч;Α Ɯ;Βρ η=null;int Ш=0;int Щ=0;int Й=0;int Ț=0;int Ħ=30;int У=0;bool Ё=true;Vector3 л;double К=0;
bool Αϊ=false;int ΔΥ=0;int Ђ=0;int ΒЈ=0;int ǯ=0;int ΖΝ=0;Vector3 ƕ=new Vector3();float к=0;float Ь=0;int Ĭ=0;Ψ ΑЂ=null;float Γκ=0;float ΑΕ=0;float С=0;float Ĥ=0;bool Γε=false;bool Ī=false;bool ƣ=false;bool ȗ=false;bool Ȑ=false;DateTime ΘΝ=new DateTime();Ψ ě=null;
Ψ Εή=null;float ǽ=0;с Ǩ;float Ș=0;float ī=0;String Γς="";float Αη=0;float ƀ=0;float Ɛ=0;List<Ǥ>ǖ=new List<Ǥ>();float О=0;bool ƚ=false;bool Ζπ=false;bool Ζέ=false;bool ΑІ=false;bool Ζή=false;float Γυ=0;float ΓЀ=0;Vector3 Ερ=new Vector3();Vector3 υ=new Vector3();Vector3 ΑЇ=new Vector3(1,1,1);
float ΔΕ=1;Vector3 Ɵ=new Vector3();bool Αα=false;bool Γή=false;bool Γο=false;bool ΕΙ=false;float ΑΛ=0;float ΑΑ=2;Vector3 Ζξ;Vector3 ΔΔ;Vector3 ΔΓ;float[,]Ǡ=new float[3,2];Dictionary<String,float[,]>ќ=new Dictionary<string,float[,]>();Dictionary<String,float>Ζψ=new Dictionary<String,float>();
int ΒЋ=0;Dictionary<String,float[]>Ȝ=new Dictionary<string,float[]>();float ΓЃ;bool ΑΞ=false;int ΑΔ=0;int Ă=0;int Αή=0;int Ġ=0;String Κ="";int[]ΖΔ=new int[Enum.GetValues(typeof(Ϊ)).Length];bool ΖΕ=false;int Αρ=0;int Ǚ=0;Ϊ Ѝ=Ϊ.Main;int ΘЎ=0,ΘЏ=0,ΘА=0,ΘБ=0;int ΘϏ=0,ΘЀ=0;
bool Ł=false;bool ΕΚ=false;String ΓЋ="";String ΒА="";IMyBroadcastListener ȑ=null;bool Επ=true;String Θα="";Exception ș=null;bool Αω=false;Λ Ŝ=Λ.Я;bool Εα=false;bool ΕЉ=true;bool ΓЈ=false;String Ζω="";bool ơ=false;bool ΓА=true;int Αί=0;Vector3 ΓΑ=new Vector3();bool Εχ=false;
bool ǎ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;Program(){ϊ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(Βή(Me,controllerTag,true))
Η=Ζ.т;π(Ϊ.Main);if(Η!=Ζ.т){Κ="Welcome to [PAM]!";у ΓЇ=Ηα();if(Η==Ζ.ΕΔ){List<IMyShipDrill>Εμ=new List<IMyShipDrill>();List<IMyShipGrinder>Εψ=new List<IMyShipGrinder>();ϊ.GetBlocksOfType(Εμ,q=>q.CubeGrid==Me.CubeGrid);ϊ.GetBlocksOfType(Εψ,q=>q.CubeGrid==Me.CubeGrid);if(Εμ.Count>0)
{Η=Ζ.Ű;Κ="Miner mode enabled!";}else if(Εψ.Count>0){Η=Ζ.Ǎ;Κ="Grinder mode enabled!";}else{Η=Ζ.Ϋ;Ǻ(а.Ϋ);}}if(ΓЇ==у.ǆ) Ɠ=false;if(ΓЇ==у.Δμ) Κ="Data restore failed!";if(ΓЇ==у.Βζ) Κ="New version, wipe data";}}void Main(string ђ,UpdateType ΙΘ){try{if(ș!=null){if(ђ.Trim().ToUpper()=="RETRY")
{ș=null;ƈ=true;ĉ=0;if(Η!=Ζ.т) φ();Κ="Restarted after error";}else{ΗΛ();ΔЈ();return;}}Αγ=(ΙΘ&UpdateType.Update10)!=0;if(Αγ) ΔΖ++;ą=ΔΖ>=10;if(ą) ΔΖ=0;if(Αγ){ΑΔ++;if(ΑΔ>4) ΑΔ=0;Αϋ=(int) Math.Max(0,10-(DateTime.Now-ΕЊ).TotalSeconds);}if(ђ!="") Θα=ђ;if(Η!=Ζ.т)
Ηο(ђ);else
ΖЎ(ђ);ΖΕ=false;try{int Εί=Runtime.CurrentInstructionCount;float Ɩ=А(Εί,Runtime.MaxInstructionCount);if(Ɩ>0.90) Κ="Max. instructions >90%";if(Ɩ>Ő) Ő=Ɩ;if(ΑΞ){ň.Add(Εί);while(ň.Count>10) ň.RemoveAt(0);Ŀ=0;for(int i=0;i<ň.Count;i++) Ŀ+=ň[i];Ŀ=А(А(Ŀ,ň.Count),Runtime.MaxInstructionCount);
double Γλ=Runtime.LastRunTimeMs;if(Αΰ&&Γλ>Γχ) Γχ=Γλ;Ś.Add((int)(Γλ*1000f));while(Ś.Count>10) Ś.RemoveAt(0);ź=0;for(int i=0;i<Ś.Count;i++) ź+=Ś[i];ź=А(ź,Ś.Count)/1000f;}}catch{Ő=0;}}catch(Exception e){ș=e;}}void Ηο(string ђ){bool Αύ=false;String ΕΧ="";if(ύ<=1&&!ΔЀ(ђ))
ΔЉ(ђ);int Θλ=50;while(ȑ!=null&&ȑ.HasPendingMessage&&Θλ-->0){MyIGCMessage ρ=ȑ.AcceptMessage();String Χ=ρ.Data as String;if(Χ==null) continue;String ƙ="";if(ύ<=1&&ΗΫ(ref Χ,out ƙ,out ΕΧ)&&ƙ==BROADCAST_SELF_ID){ΔЉ(Χ);Αύ=true;break;}}bool ΕЄ=Αΰ&&Φ==Α.з&&!ŋ&&!Αύ&&ĉ==0&&!Αβ;
if(ą&&Φ!=Α.з) Αύ=true;if((Αγ&&!ΕЄ)||(ą&&ΕЄ)){if(ĉ==0&&(Αϋ<=0||ƈ)){Ζϊ=ύ>0;ύ=0;ĉ=1;ǻ();Ηε();ΗΠ();ǵ("Scan 1");}else if(ĉ==1){ĉ=2;ǻ();Ηζ();ǵ("Scan 2");}else if(ĉ==2){ĉ=0;ǻ();Ηη();Ηθ();if(ύ<=1) ΔЂ();ǵ("Scan 3");ΕЊ=DateTime.Now;if(ύ<=1&&Ɠ)
Αψ=Ƶ(Τ,Τ.CenterOfMass);Ɠ=false;if(ƈ){ƈ=false;bool ΙΝ=ί==Λ.Є;φ();if(ΕЉ&&resumeAfterRestart&&ΙΝ&&ύ<=1) ΓЈ=true;}ΕЉ=false;if(Ζϊ&&ύ==0) Κ="Setup complete";}else{if(ί==Λ.Є&&Η!=Ζ.Ϋ){ǻ();Ζύ();ǵ("Inv balance");}ǻ();switch(Ƣ){case 0:Ηϊ();break;case 1:Ηώ();break;case 2:Ηω();break;
case 3:ΗЅ();break;case 4:Ηύ();break;case 5:ΖЅ();break;case 6:ΖЂ(Τ);break;}ǵ("Update: "+Ƣ);Ƣ++;if(Ƣ>6){Ƣ=0;Αΰ=true;if(Ŝ!=Λ.Я){switch(Ŝ){case Λ.Į:ȧ();break;case Λ.į:ȧ();break;case Λ.Є:ȧ();break;case Λ.Ƽ:ƴ();break;case Λ.ƽ:Ē();break;}Ŝ=Λ.Я;}}}if(!ƈ){if(!Ƿ(Τ,true)){Τ=null;
Ɠ=true;ύ=2;}if(ύ>=2&&Φ!=Α.з)
φ();if(ύ<=1){Ĩ=Τ.CalculateShipMass().PhysicalMass;Ȟ=(float) Τ.GetShipSpeed();ή=Ηψ(Τ,Αψ);ΑΫ=Τ.WorldMatrix.Forward;Ǝ=Τ.WorldMatrix.Left;ƅ=Τ.WorldMatrix.Down;ΗЁ();ΗЃ();Ηϋ();if(ΓЈ){ΓЈ=false;ȧ();if(Φ!=Α.з) Κ="Job resumed after restart";}if(Φ!=Α.з){ŋ=false;ΔЍ(false);Δα(false);String ΘЇ=ΒΩ(Φ)+" "+(int) Φ;
ǻ();ΗϏ();ΒЀ(false);ǵ(ΘЇ);ΗЄ();ΗЀ();ǻ();Ηό();ǵ("Thruster");ǻ();ΕΕ();ǵ("Gyroscope");}else{if(ŋ){if(ƹ()){Х(ƅ,ΑΫ,Ǝ,0.25f,true);ΕΕ();Κ="Aligning to planet: "+Math.Round(ΑΛ-0.25f,2)+"°";if(Αα) Βά(true,true);}else Βά(true,true);}}ơ=false;}}}ǻ();ΔЈ();ǵ("Print");if(Αύ||ΒБ<=0){ǻ();Ζώ(ΕΧ);
ǵ("Broadcast");ΒБ=4;}else if(ą) ΒБ--;}void ΔЉ(string ђ){if(ђ=="") return;var Е=ђ.ToUpper().Split(' ');Е.DefaultIfEmpty("");var ΓΗ=Е.ElementAtOrDefault(0);var ę=Е.ElementAtOrDefault(1);var Ź=Е.ElementAtOrDefault(2);var ΕΝ=Е.ElementAtOrDefault(3);String ă="Invalid argument: "+ђ;bool ΔΞ=false;
switch(ΓΗ){case "UP":this.ΔЄ(false);break;case "DOWN":this.ΔЃ(false);break;case "UPLOOP":this.ΔЄ(true);break;case "DOWNLOOP":this.ΔЃ(true);break;case "APPLY":this.Δί(true);break;case "MRES":Ǚ=0;break;case "STOP":this.φ();break;case "PATHHOME":{this.φ();this.Βό();}break;case "PATH":{this.φ();this.Βό();Θ.ε=true;}break;
case "START":{this.φ();Βϋ();}break;case "ALIGN":{Βά(!ŋ,false);}break;case "CONT":{this.φ();this.ȧ();}break;case "JOBPOS":{this.φ();this.Ē();}break;case "HOMEPOS":{this.φ();this.ƴ();}break;case "FULL":{ƚ=true;}break;case "RESET":{Αω=true;ύ=2;}break;case "EXPORT":{ΗΓ();}break;
case "IMPORT":{ΗΝ(ę=="JOB");}break;default:ΔΞ=true;break;}if(Η!=Ζ.Ϋ){switch(ΓΗ){case "SHUTTLE":{ΗΑ();}break;case "CFGS":{if(!ΖЌ(ę,Ź,ΕΝ)) Κ=ă;}break;case "CFGB":{if(!ΖЋ(ę,Ź)) Κ=ă;}break;case "CFGL":{if(!ē(ref Ǯ,true,σ.ƺ,ę,"")||!ΖЍ(Ź))
Κ=ă;}break;case "CFGE":{if(!ē(ref Ъ,true,σ.ƻ,ę,"IG")||
!ē(ref ш,true,σ.Βδ,Ź,"IG")||
!ē(ref щ,true,σ.Βε,ΕΝ,"IG"))
Κ=ă;}break;case "CFGA":{if(!ē(ref ǧ,false,σ.Ʃ,ę,"")) Κ=ă;}break;case "CFGW":{if(!ē(ref ť,false,σ.Ŷ,ę,"")||
!ē(ref Ť,false,σ.Ŷ,Ź,""))
Κ=ă;}break;case "NEXT":{ΑЎ(false);}break;case "PREV":{ΑЎ(true);}break;default:if(ΔΞ) Κ=ă;break;}}else{switch(ΓΗ){case "UNDOCK":{ơ=true;}break;default:if(ΔΞ) Κ=ă;break;}}}String ΗΗ(){String Γ="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
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
Γ+="[UNDOCK] Leave current connector\n\n";return Γ;}void ΗΑ(){φ();Η=Ζ.Ϋ;Ǻ(а.Ϋ);Ε.ε=false;Θ.ε=false;ξ=null;ο.Clear();ί=Λ.Я;}void Βά(bool Ї,bool є){if(!Ї) Κ="Aligning canceled";if(є) Κ="Aligning done";if(є||!Ї){ŋ=false;ΔБ();Ǆ(false,0,0,0,0);return;}if(ƹ()) ŋ=true;}bool ΖЌ(String ΙΟ,String ΘΩ,String ΘΗ)
{bool ΙΛ=ί==Λ.Є;int w,h,d;if(int.TryParse(ΙΟ,out w)&&int.TryParse(ΘΩ,out h)&&int.TryParse(ΘΗ,out d)){this.φ();й=w;и=h;Ц=d;Ė(false);ŵ(false,false);if(ΙΛ) ȧ();return true;}return false;}bool ΖЍ(String ΕΜ){if(ΕΜ=="ON"){Ȥ=true;return true;}if(ΕΜ=="OFF"){Ȥ=false;return true;}
return false;}bool ΖЋ(String Ελ,String ΑΜ){bool Ɣ=true;if(Ελ=="HOME") ċ=true;else if(Ελ=="STOP") ċ=false;else Ɣ=false;if(ΑΜ=="HOME") н=е.ǂ;else if(ΑΜ=="STOP") н=е.ΔА;else if(ΑΜ=="JOB") н=е.ΒΤ;else if(ΑΜ=="IG") н=е.Ū;else Ɣ=false;return Ɣ;}void π(Ϊ Γώ){ΖΔ[(int) Ѝ]=Ǚ;
Ǚ=ΖΔ[(int) Γώ];if(Γώ==Ϊ.Ηξ) Ǚ=0;Ѝ=Γώ;if(Η!=Ζ.т)
ĕ(Ѝ==Ϊ.ů,false,0,0);ΖΕ=true;}void ΔЄ(bool Γπ){if(Ǚ>0) Ǚ--;else if(Γπ) Ǚ=Αρ-1;}void ΔЃ(bool Γπ){if(Ǚ<Αρ-1) Ǚ++;else if(Γπ) Ǚ=0;}String Ǻ(а ρ){switch(ρ){case а.ǃ:Κ="Job is running";break;case а.ŧ:Κ="Connector not ready!";break;case а.Ηπ:Κ="Ship modified, path outdated!";break;
case а.Δύ:Κ="Interrupted by player!";break;case а.Ϋ:Κ="Shuttle mode enabled!";break;}return "";}String ΗΚ(Ǐ λ){switch(λ){case Ǐ.Ǉ:return "Top-Left";case Ǐ.ΖЄ:return "Center";default:return "";}}String ΗΕ(я ƒ){switch(ƒ){case я.ΒΓ:return "Auto"+(Η==Ζ.Ű?" (Ore)":"");case я.ΒΔ:return "Auto (+Stone)";
case я.ΒΚ:return "Default";default:return "";}}String ΒΧ(Λ Ф){switch(Ф){case Λ.Я:return "No job";case Λ.ķ:return "Job paused";case Λ.Є:return "Job active";case Λ.Į:return "Job active";case Λ.į:return "Job active";case Λ.ΒΝ:return "Job done";case Λ.ƭ:return "Job changed";case Λ.Ƽ:return "Move home";
case Λ.ƽ:return "Move to job";}return "";}String Δρ(е ΗЏ){switch(ΗЏ){case е.ǂ:return "Return home";case е.ΒΤ:return "Fly to job pos";case е.ΔА:return "Stop";case е.Ū:return "Ignore";}return "";}String ΗΖ(ώ ƒ){switch(ƒ){case ώ.Βη:return "Off";case ώ.ΒΞ:return "Drop pos (Stone) ";case ώ.ư:return "Drop pos (Sto.+Ice)";
case ώ.Δη:return "Cur. pos (Stone)";case ώ.ΒΘ:return "Cur. pos (Sto.+Ice)";case ώ.Βΰ:return "In motion (Stone)";case ώ.Ʒ:return "In motion (Sto.+Ice)";}return "";}String Δο(с Ф){switch(Ф){case с.None:return "No batteries";case с.Đ:return "Charging";case с.Δκ:return "Discharging";}return "";}String ΒΩ(Α Ф)
{String ΙΫ=Η==Ζ.Ϋ?"target":"job";switch(Ф){case Α.з:return "Idle";case Α.ǣ:return "Flying to XY position";case Α.Ķ:return Η==Ζ.Ǎ?"Grinding":"Mining";case Α.ǹ:return "Returning";case Α.Ĳ:return "Flying to drop pos";case Α.Ǹ:return "Returning to dock";case Α.Ѕ:return "Flying to dock area";
case Α.Э:return "Flying to job area";case Α.ȩ:return "Flying to path";case Α.ĳ:return "Flying to job position";case Α.ǡ:return "Approaching dock";case Α.đ:return "Docking";case Α.č:return "Aligning";case Α.ȥ:return "Aligning";case Α.ű:return "Retry docking";case Α.Ǽ:return "Unloading";case Α.ȫ:return Ζω;
case Α.ǐ:return "Undocking";case Α.Đ:return "Charging batteries";case Α.ǋ:return "Waiting for uranium";case Α.Ʋ:return "Filling up hydrogen";case Α.ΒЁ:return "Waiting for ejection";case Α.Ǌ:return "Waiting for ejection";case Α.Ƴ:return "Flying to drop pos";}return "";}String Δχ(ι Ќ){switch(Ќ)
{case ι.Βι:return "On \"Undock\" command";case ι.Βν:return "On player entered cockpit";case ι.ΔЇ:return "On ship is full";case ι.ΔІ:return "On ship is empty";case ι.Βξ:return "On time delay";case ι.ǀ:return "On batteries empty(<25%)";case ι.ƿ:return "On batteries empty(=0%)";case ι.Βθ:return "On batteries full";
case ι.Βμ:return "On hydrogen empty(<25%)";case ι.Βκ:return "On hydrogen empty(=0%)";case ι.Βλ:return "On hydrogen full";}return "";}bool Π(ref String Γ,int ΖΩ,int ā,bool Ξ,String ƍ){Αρ+=1;if(ΖΩ==ā) ƍ=">"+ƍ+(ΑΔ>=2?" .":"");else ƍ=" "+ƍ;Γ+=ƍ+"\n";return ΖΩ==ā&&Ξ;}String Δί(bool Ξ)
{int Β=0;int Ο=Ǚ;Αρ=0;String Ζά="———————————————\n";String š="--------------------------------------------\n";String Γ="";Γ+=ΒΧ(ί)+" | "+(Θ.ε?"Ready to dock":"No dock")+"\n";Γ+=Ζά;double ǝ=Math.Max(Math.Round(this.К),0);if(Ѝ==Ϊ.Main){bool Ą=Η==Ζ.Ϋ;if(Π(ref Γ,Ο,Β++,Ξ," Record path & set home")) Βό();
if(Η==Ζ.Ű)
if(Π(ref Γ,Ο,Β++,Ξ," Setup mining job")) π(Ϊ.ů);if(Η==Ζ.Ǎ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup grinding job")) π(Ϊ.ů);if(Η==Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Setup shuttle job")) π(Ϊ.Βψ);if(Π(ref Γ,Ο,Β++,Ξ," Continue job")) ȧ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to home position")) ƴ();if(Π(ref Γ,Ο,Β++,Ξ," Fly to job position")) Ē();if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(Ą) π(Ϊ.ǅ);else π(Ϊ.ƫ);if(Π(ref Γ,Ο,Β++,Ξ," Info")) π(Ϊ.Δϊ);if(Η!=Ζ.Ϋ)
if(Π(ref Γ,Ο,Β++,Ξ," Help")) π(Ϊ.Δψ);}else if(Ѝ==Ϊ.ů){double ΙΞ=Math.Round(й*ǟ,1);double ΘΨ=Math.Round(и*ǲ,1);String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Start new job!")) Βϋ();if(Π(ref Ѓ,Ο,Β++,Ξ," Change current job")){ŵ(false,false);π(Ϊ.Main);}if(Π(ref Ѓ,Ο,Β++,Ξ," Width + (Width: "+й+" = "+ΙΞ+"m)")){İ(ref й,5,20,1);Ė(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Width -")){İ(ref й,-5,20,-1);Ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height + (Height: "+и+" = "+ΘΨ+"m)")){İ(ref и,5,20,1);Ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Height -")){İ(ref и,-5,20,-1);Ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth + ("+(х==я.ΒΚ?"Depth":"Min")+": "+Ц+"m)")){İ(ref Ц,5,50,2);Ė(true);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Depth -")){İ(ref Ц,-5,50,-2);Ė(true);}if(Π(ref Ѓ,Ο,Β++,Ξ," Depth mode: "+ΗΕ(х))){х=Ȩ(х);}if(Π(ref Ѓ,Ο,Β++,Ξ," Start pos: "+ΗΚ(ȟ))){ȟ=Ȩ(ȟ);}if(Η==Ζ.Ǎ&&х==я.ΒΔ) х=Ȩ(х);Γ+=ΔЋ(8,Ѓ,Ο,ref ΘϏ);}else if(Ѝ==Ϊ.Βψ){float[]Εγ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.ΔЎ);if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=" Leave connector 1:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δχ(ϋ.Ќ))) ϋ.Ќ=Ȩ(ϋ.Ќ);if(!ϋ.Βέ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΦ((int) ϋ.Ǔ))) ϋ.Ǔ=Δθ(ϋ.Ǔ,Εγ);Γ+=" Leave connector 2:\n";if(Π(ref Γ,Ο,Β++,Ξ," - "+Δχ(ό.Ќ))) ό.Ќ=Ȩ(ό.Ќ);
if(!ό.Βέ()) Γ+="\n";else if(Π(ref Γ,Ο,Β++,Ξ," - Delay: "+ΒΦ((int) ό.Ǔ))) ό.Ǔ=Δθ(ό.Ǔ,Εγ);}else if(Ѝ==Ϊ.ΔЎ){if(Π(ref Γ,Ο,Β++,Ξ," Start job!")) Βϋ();if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Βψ);Γ+=" Timer: \"Docking connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.ǔ!=""?ϋ.ǔ:"-"))) ϋ.ǔ=ƾ(ref ΘЎ);
Γ+=" Timer: \"Leaving connector 1\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ϋ.Ǘ!=""?ϋ.Ǘ:"-"))) ϋ.Ǘ=ƾ(ref ΘА);Γ+=" Timer: \"Docking connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.ǔ!=""?ό.ǔ:"-"))) ό.ǔ=ƾ(ref ΘЏ);Γ+=" Timer: \"Leaving connector 2\":\n";if(Π(ref Γ,Ο,Β++,Ξ," = "+(ό.Ǘ!=""?ό.Ǘ:"-"))) ό.Ǘ=ƾ(ref ΘБ);
}else if(Ѝ==Ϊ.Ů){String ΕЏ=Ǯ+" %";if(ą) Ă++;if(Ă>1){Ă=0;Αή++;if(Αή>1) Αή=0;}bool[]ΓЍ=new bool[]{Ĉ.Count==0,Ǩ==с.None,ѕ.Count==0};int Ζυ=0;while(true){Ζυ++;Ġ++;if(Ġ>ΓЍ.Length-1) Ġ=0;if(Ζυ>=ΓЍ.Length) break;if(!ΓЍ[Ġ]) break;}bool Ą=Η==Ζ.Ϋ;if(!Ą&&Ȥ&&О!=-1&&Αή==0)
ΕЏ=О<1000000?Math.Round(О)+" Kg":Math.Round(О/1000)+" t";if(Π(ref Γ,Ο,Β++,Ξ," Stop!")){φ();π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Behavior settings"))
if(!Ą) π(Ϊ.ƫ);else π(Ϊ.ǅ);if(!Ą){if(Π(ref Γ,Ο,Β++,Ξ," Next hole")) ΑЎ(false);}else if(Π(ref Γ,Ο,Β++,Ξ," Undock")) ơ=true;Γ+=š;if(!Ą) Γ+="Progress: "+Math.Round(ƌ,1)+" %\n";Γ+="State: "+ΒΩ(Φ)+" "+ǝ+"m \n";Γ+="Load: "+Αη+" % Max: "+ΕЏ+" \n";if(Ġ==0) Γ+="Uranium: "+(Ĉ.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+(Ъ==-1?"":" Min: "+Ъ+" Kg"))+"\n";
if(Ġ==1) Γ+="Battery: "+(Ǩ==с.None?Δο(Ǩ):ǽ+"% "+(ш==-1||Ą?"":" Min: "+ш+" %"))+"\n";if(Ġ==2) Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(Ș,1)+"% "+(щ==-1||Ą?"":" Min: "+щ+" %"))+"\n";}else if(Ѝ==Ϊ.ƫ){String Ѓ="";if(Π(ref Ѓ,Ο,Β++,Ξ," Back")){if(ί==Λ.Є) π(Ϊ.Ů);else π(Ϊ.Main);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Max load: "+Ǯ+"%")) д(ref Ǯ,Ǯ<=80?-10:-5,σ.ƺ,false);if(Π(ref Ѓ,Ο,Β++,Ξ," Weight limit: "+(Ȥ?"On":"Off"))) Ȥ=!Ȥ;if(Π(ref Ѓ,Ο,Β++,Ξ," Ejection: "+ΗΖ(М))) М=Ȩ(М);if(Π(ref Ѓ,Ο,Β++,Ξ," Toggle sorters: "+(ĩ?"On":"Off"))){ĩ=!ĩ;if(ĩ) Ǧ(ΓА);}
if(Π(ref Ѓ,Ο,Β++,Ξ," Unload ice: "+(Ȣ?"On":"Off"))) Ȣ=!Ȣ;if(Π(ref Ѓ,Ο,Β++,Ξ," Uranium: "+(Ъ==-1?"Ignore":"Min "+Ъ+"Kg"))) д(ref Ъ,(Ъ>5?-5:-1),σ.ƻ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Min "+ш+"%"))) д(ref ш,-5,σ.Βδ,true);if(Π(ref Ѓ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Min "+щ+"%"))) д(ref щ,-10,σ.Βε,true);
if(Π(ref Ѓ,Ο,Β++,Ξ," When done: "+(ċ?"Return home":"Stop"))) ċ=!ċ;if(Π(ref Ѓ,Ο,Β++,Ξ," On damage: "+Δρ(н))) н=Ȩ(н);if(Π(ref Ѓ,Ο,Β++,Ξ," Advanced...")) π(Ϊ.ΑЏ);Γ+=ΔЋ(8,Ѓ,Ο,ref ΘЀ);}else if(Ѝ==Ϊ.ΑЏ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.Є) π(Ϊ.Ů);else π(Ϊ.Main);}
if(Π(ref Γ,Ο,Β++,Ξ,(Η==Ζ.Ǎ?" Grinder":" Drill")+" inv. balancing: "+(Ŋ?"On":"Off"))) Ŋ=!Ŋ;if(Π(ref Γ,Ο,Β++,Ξ," Enable"+(Η==Ζ.Ǎ?" grinders":" drills")+": "+(Ń?"Fwd + Bwd":"Fwd"))) Ń=!Ń;if(Π(ref Γ,Ο,Β++,Ξ," Work speed fwd.: "+ť+"m/s")) д(ref ť,0.5f,σ.Ŷ,false);
if(Π(ref Γ,Ο,Β++,Ξ," Work speed bwd.: "+Ť+"m/s")) д(ref Ť,0.5f,σ.Ŷ,false);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǧ*100f)+"%"+(ǧ>0.80f?" (risky)":""))) д(ref ǧ,0.1f,σ.Ʃ,false);if(Π(ref Γ,Ο,Β++,Ξ," Width overlap: "+ĭ*100f+"%")) Δγ(true,0.05f);if(Π(ref Γ,Ο,Β++,Ξ," Height overlap: "+ğ*100f+"%")) Δγ(false,0.05f);
}else if(Ѝ==Ϊ.ǅ){if(Π(ref Γ,Ο,Β++,Ξ," Back")){if(ί==Λ.Є) π(Ϊ.Ů);else π(Ϊ.Main);}if(Π(ref Γ,Ο,Β++,Ξ," Max load: "+Ǯ+"%")) д(ref Ǯ,Ǯ<=80?-10:-5,σ.ƺ,false);if(Π(ref Γ,Ο,Β++,Ξ," Unload ice: "+(Ȣ?"On":"Off"))) Ȣ=!Ȣ;if(Π(ref Γ,Ο,Β++,Ξ," Uranium: "+(Ъ==-1?"Ignore":"Min "+Ъ+"Kg"))) д(ref Ъ,(Ъ>5?-5:-1),σ.ƻ,true);
if(Π(ref Γ,Ο,Β++,Ξ," Battery: "+(ш==-1?"Ignore":"Charge up"))) ш=(ш==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," Hydrogen: "+(щ==-1?"Ignore":"Fill up"))) щ=(щ==-1?1:-1);if(Π(ref Γ,Ο,Β++,Ξ," On damage: "+Δρ(н))) н=Ȩ(н);if(Π(ref Γ,Ο,Β++,Ξ," Acceleration: "+Math.Round(ǧ*100f)+"%"+(ǧ>0.80f?" (risky)":""))) д(ref ǧ,0.1f,σ.Ʃ,false);
}else if(Ѝ==Ϊ.ΔЊ){double Θβ=0;if(Σ.Count>0) Θβ=Vector3.Distance(Σ.Last().Δ,ή);if(Π(ref Γ,Ο,Β++,Ξ," Stop path recording")) ΕΑ();if(Η!=Ζ.Ϋ){if(Π(ref Γ,Ο,Β++,Ξ," Home: "+(Ȋ?"Use old home":(Θ.ε?"Was set! ":"none ")))) Ȋ=!Ȋ;}else{if(Π(ref Γ,Ο,Β++,Ξ," Connector 1: "+(Ȋ?"Use old connector":(Θ.ε?"Was set! ":"none ")))) Ȋ=!Ȋ;
if(Π(ref Γ,Ο,Β++,Ξ," Connector 2: "+(Ţ?"Use old connector":(Ε.ε?"Was set! ":"none ")))) Ţ=!Ţ;}if(Π(ref Γ,Ο,Β++,Ξ," Path: "+(ţ?"Use old path":(Σ.Count>1?"Count: "+Σ.Count:"none ")))) ţ=!ţ;Γ+=š;Γ+="Wp spacing: "+Math.Round(Ζϋ)+"m\n";}else if(Ѝ==Ϊ.Βύ){if(Π(ref Γ,Ο,Β++,Ξ," Stop")){φ();π(Ϊ.Main);}
Γ+=š;Γ+="State: "+ΒΩ(Φ)+" \n";Γ+="Speed: "+Math.Round(Ȟ,1)+"m/s\n";Γ+="Target dist: "+ǝ+"m\n";Γ+="Wp count: "+Σ.Count+"\n";Γ+="Wp left: "+ΔΥ+"\n";}else if(Ѝ==Ϊ.Δϊ){List<IMyTerminalBlock>Ĝ=Δς();if(ą) Ă++;if(Ă>=Ĝ.Count) Ă=0;if(Π(ref Γ,Ο,Β++,Ξ," Next")) π(Ϊ.Δϋ);
Γ+=š;Γ+="Version: "+VERSION+"\n";Γ+="Ship load: "+Math.Round(Αη,1)+"% "+Math.Round(ƀ,1)+" / "+Math.Round(Ɛ,1)+"\n";Γ+="Uranium: "+(Ĉ.Count==0?"No reactors":Math.Round(ī,1)+"Kg "+Γς)+"\n";Γ+="Battery: "+(Ǩ==с.None?"":ǽ+"% ")+Δο(Ǩ)+"\n";Γ+="Hydrogen: "+(ѕ.Count==0?"No tanks":Math.Round(Ș,1)+"% ")+"\n";
Γ+="Damage: "+(Ĝ.Count==0?"None":""+(Ă+1)+"/"+Ĝ.Count+" "+Ĝ[Ă].CustomName)+"\n";}else if(Ѝ==Ϊ.Δϋ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=š;Γ+="Next scan: "+Αϋ+"s\n";Γ+="Drill area: "+Math.Round(ǟ,1)+"x"+Math.Round(ǲ,1)+"m Ship: "+Math.Round(Αό,1)+"x"+Math.Round(ΓЊ,1)+"m ("+Math.Round(δ,1)+"m)\n"+(ǩ.Count>0?"Cameras: "+ΓΓ+"\n":"");
Γ+="Broadcast: "+(Ł?"Online - "+ΓЋ:"Offline")+"\n";Γ+="Max Instructions: "+Math.Round(Ő*100f,1)+"% \n";}else if(Ѝ==Ϊ.Δψ){if(Π(ref Γ,Ο,Β++,Ξ," Back")) π(Ϊ.Main);Γ+=š;Γ+="1. Dock to your docking station\n";Γ+="2. Select Record path & set home\n";Γ+="3. Fly the path to the ores\n";
Γ+="4. Select stop path recording\n";Γ+="5. Align ship in mining direction\n";Γ+="6. Select Setup job and start\n";}if(ύ==2) Γ="Fatal setup error\nNext scan: "+Αϋ+"s\n";if(Αω) Γ="Recompile script now";int Θγ=Γ.Split('\n').Length;for(int i=Θγ;i<=10;i++) Γ+="\n";Γ+=Ζά;Γ+="Last: "+Κ+"\n";
return Γ;}void Δγ(bool Θί,float ћ){φ();ŵ(true,false);if(Θί) д(ref ĭ,ћ,σ.Βο,false);else д(ref ğ,ћ,σ.Βο,false);Δβ();ĕ(true,true,0,0);}float Δθ(float θ,float[]ΑЍ){float έ=ΑЍ[0];for(int i=ΑЍ.Length-1;i>=0;i--)
if(θ<ΑЍ[i]) έ=ΑЍ[i];return έ;}String ƾ(ref int ā){String έ="";if(ā>=ΔΙ.Count) ā=-1;if(ā>=0) έ=ΔΙ[ā].CustomName;ā++;return έ;}void ǈ(string ψ){if(ί!=Λ.Є) return;if(ψ=="") return;IMyTerminalBlock Υ=ϊ.GetBlockWithName(ψ);if(Υ==null||!(Υ is IMyTimerBlock)){Κ="Timerblock "+ψ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void İ(ref int value,int ż,int Ƞ,int ΓЎ){if(ż==0) return;if(value<Ƞ&&ΓЎ>0||value<=Ƞ&&ΓЎ<0){value+=ΓЎ;return;}int ΗЇ=Math.Abs(ż);int ΒЂ=0;int Γω=1;while(true){ΒЂ+=Γω*ΗЇ*10;if(ż<0&&value-Ƞ<=ΒЂ) break;if(ż>0&&value-Ƞ<ΒЂ) break;
Γω++;}value+=Γω*ż;}void Ė(bool Ιή){й=Math.Max(й,1);и=Math.Max(и,1);Ц=Math.Max(Ц,0);ĕ(Ѝ==Ϊ.ů,false,0,0);}bool ē(ref float value,bool Θϋ,σ ς,String ġ,String Θά){if(ġ=="") return false;float ŕ=-1;bool ΕЁ=false;if(ġ.ToUpper()==Θά) ΕЁ=true;else if(!float.TryParse(ġ,out ŕ)) return false;
else ŕ=Math.Max(0,ŕ);if(Θϋ) ŕ=(float) Math.Round(ŕ);д(ref value,ŕ,ς,ΕЁ,false);return true;}void д(ref float value,float ћ,σ ς,bool Ơ){д(ref value,value+ћ,ς,Ơ,true);}void д(ref float value,float ő,σ ς,bool Ơ,bool ΙΣ){float ǚ=0,ǘ=0;if(ς==σ.Ŷ){ǚ=0.5f;ǘ=10f;}if(ς==σ.Ʃ){ǚ=0.1f;ǘ=1f;}
if(ς==σ.Ū){ǚ=50f;ǘ=100f;}if(ς==σ.Βδ){ǚ=5f;ǘ=30f;}if(ς==σ.ƻ){ǚ=1f;ǘ=25f;}if(ς==σ.ƺ){ǚ=10f;ǘ=95f;}if(ς==σ.Βε){ǚ=10f;ǘ=90f;}if(ς==σ.Ηφ){ǚ=10f;ǘ=1800;}if(ς==σ.Βο){ǚ=0.0f;ǘ=0.75f;}if(ő==-1&&Ơ){value=-1;return;}if(value==-1) Ơ=false;
bool ΖΟ=ő<ǚ||ő>ǘ;if(ΖΟ&&ΙΣ){if(ő<value) value=ǘ;else if(ő>value) value=ǚ;}else value=ő;if(ΖΟ&&Ơ) value=-1;else value=Math.Max(ǚ,Math.Min(value,ǘ));value=(float) Math.Round(value,2);}void ΑЎ(bool Θχ){if(Θχ) Й=Math.Max(0,Й-1);else Й++;ΒΠ();ΒΟ();for(int k=0;k<ȃ.Count&&k<Л.Length;k++) Βφ(ȃ[k],k<Й);
Ѐ=-1;ΒЀ(true);}T Ȩ<T>(T θ){int Β=Array.IndexOf(Enum.GetValues(θ.GetType()),θ);Β++;if(Β>=ΗΒ(θ)) Β=0;return(T) Enum.GetValues(θ.GetType()).GetValue(Β);}int ΗΒ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void Βό(){Ƨ.Clear();for(int i=0;i<Σ.Count;i++) Ƨ.Add(Σ[i]);Σ.Clear();
Αβ=true;Γδ=new Ψ(Θ);Γη=new Ψ(Ε);Θ.ε=false;if(Η==Ζ.Ϋ) Ε.ε=false;for(int i=0;i<ќ.Count;i++)
if(!Ȉ.Contains(ќ.Keys.ElementAt(i)))
Ȉ.Add(ќ.Keys.ElementAt(i));Ȋ=false;Ţ=false;ţ=false;π(Ϊ.ΔЊ);}void ΕΑ(){if(Ȋ) Θ=Γδ;if(Ţ) Ε=Γη;if(ţ){Σ.Clear();for(int i=0;i<Ƨ.Count;i++) Σ.Add(Ƨ[i]);}Αβ=false;φ();π(Ϊ.Main);}void ΗЁ(){if(!Αβ) return;if(Φ!=Α.з){ΕΑ();return;}if(!Γδ.ε) Ȋ=false;if(!Γη.ε) Ţ=false;if(Ƨ.Count<=1) ţ=false;
IMyShipConnector И=П(MyShipConnectorStatus.Connectable);if(И==null) И=П(MyShipConnectorStatus.Connected);if(И!=null){if(Math.Round(Ȟ,2)<=0.20) ΓΠ++;else ΓΠ=0;if(ΓΠ>=5){if(Η==Ζ.Ϋ&&(Θ.ε||Ȋ)&&Vector3.Distance(Θ.Δ,И.GetPosition())>5){Ε.β=Τ.WorldMatrix.Forward;Ε.τ=Τ.WorldMatrix.Left;
Ε.α=Τ.WorldMatrix.Down;Ε.ν=Τ.GetNaturalGravity();Ε.Δ=И.GetPosition();Ε.ε=true;Ε.Ч=И.Position;}else{Θ.β=Τ.WorldMatrix.Forward;Θ.τ=Τ.WorldMatrix.Left;Θ.α=Τ.WorldMatrix.Down;Θ.ν=Τ.GetNaturalGravity();Θ.Δ=И.GetPosition();Θ.ε=true;Θ.Ч=И.Position;}}}double Γι=-1;if(Σ.Count>0) Γι=Vector3.Distance(ή,Σ.Last().Δ);
double ΘЅ=Math.Max(1.5,Math.Pow(Ȟ/100.0,2));double ΖΖ=Math.Max(Ȟ*ΘЅ,2);Ζϋ=ΖΖ;if((Γι==-1)||Γι>=ΖΖ){Ψ Ω=new Ψ(ή,ΑΫ,ƅ,Ǝ,Τ.GetNaturalGravity());Ω.ΖЀ(ȉ,Ȉ);Σ.Add(Ω);}}Ψ ж(){if(Η!=Ζ.Ϋ) return ј!=null?ј:Θ;return Εή;}void ĸ(Ψ ά,Λ Ф){Εή=ά;if(ί==Λ.Є) ч=Ф;}bool Ƕ(Ψ ά,float Ϗ,bool ΙΙ,out Vector3 λ)
{if(ΙΙ){Vector3I Γέ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(Γέ);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){λ=new Vector3();return false;}Vector3 ΘΓ=Ю(Τ,Υ.FatBlock.GetPosition()-ή);Vector3 ΘΒ=Ю(Τ,Υ.FatBlock.WorldMatrix.Forward);λ=ά.Δ-ΒϏ(ά.β,ά.α*-1,ΘΓ)-ΒϏ(ά.β,ά.α*-1,ΘΒ)*Ϗ;
return true;}else{λ=ά.Δ;return true;}}Vector3 Ȍ(int ΗБ,int ř,bool Θω){if(!Θω&&Εχ) return ΓΑ;float ΘΑ=((Н-1f)/2f)-ΗБ;float Θό=((в-1f)/2f)-ř;ΓΑ=Ε.Δ+Ε.τ*ΘΑ*ǟ+ȅ*-1*Θό*ǲ;Εχ=true;return ΓΑ;}Vector3 ΒΨ(Vector3 ΘЉ,float ΓΝ){return ΘЉ+(ъ*ΓΝ);}ΰ ΒΛ(){float Б=-1;ΰ έ=ΰ.Ǵ;
if(Η!=Ζ.Ϋ){ΔЂ();if(ί!=Λ.Я){Vector3 Ј=Ĕ(ъ,ȅ*-1,ή-Ε.Δ);if(Math.Abs(Ј.X)<=(float)(Н*ǟ)/2f&&Math.Abs(Ј.Y)<=(float)(в*ǲ)/2f){if(Ј.Z<=-1&&Ј.Z>=-Ħ*2) return ΰ.ƪ;if(Ј.Z>-1&&Ј.Z<δ*2) return ΰ.Ǵ;}if(Ȧ(Ε.Δ,ref Б)) έ=ΰ.Ǵ;}if(Θ.ε){if(Ȧ(Θ.Δ,ref Б)) έ=ΰ.ǳ;for(int i=0;i<Σ.Count;i++)
if(Ȧ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Vector3.Distance(ή,Θ.Δ)<dockDist*δ) έ=ΰ.ǳ;if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null)
έ=ΰ.ǳ;}}else{Vector3 λ=new Vector3();IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Θ.ε){if(Ȧ(Θ.Δ,ref Б)) έ=ΰ.ǳ;if(Ƕ(Θ,dockDist,true,out λ))
if(Ȧ(λ,ref Б)) έ=ΰ.ǳ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Θ.Δ)<5)
return ΰ.ΒΑ;}for(int i=0;i<Σ.Count;i++)
if(Vector3.Distance(Σ[i].Δ,Θ.Δ)>dockDist*δ&&Vector3.Distance(Σ[i].Δ,Ε.Δ)>dockDist*δ)
if(Ȧ(Σ[i].Δ,ref Б)) έ=ΰ.ȍ;if(Ε.ε){if(Ȧ(Ε.Δ,ref Б)) έ=ΰ.Ǵ;if(Ƕ(Ε,dockDist,true,out λ))
if(Ȧ(λ,ref Б)) έ=ΰ.Ǵ;if(Ȁ!=null&&Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5)
return ΰ.ΒΒ;}}return έ;}bool Ȧ(Vector3 λ,ref float Б){float Ϗ=Vector3.Distance(λ,ή);if(Ϗ<Б||Б==-1){Б=Ϗ;return true;}return false;}void Βϋ(){if(ύ>0){Κ="Setup error! Can't start";return;}if(Η==Ζ.Ϋ){ȧ();return;}Ε.Δ=ή;Ε.ν=Τ.GetNaturalGravity();Ε.β=ΑΫ;Ε.α=ƅ;Ε.τ=Ǝ;
ъ=љ.WorldMatrix.Forward;ȅ=Ε.α;if(ъ==Τ.WorldMatrix.Down) ȅ=Τ.WorldMatrix.Backward;ŵ(true,true);Ν(Α.ǣ);Βϊ();}void ŵ(bool Ǖ,bool Θϊ){if(ί==Λ.Я&&!Ǖ) return;bool Θν=Ǖ||ί==Λ.ΒΝ||Н!=й||в!=и||Ģ!=ȟ;if(Θν){if(ί!=Λ.Я){ί=Λ.ƭ;Ȍ(Ш,Щ,Θϊ);Κ="Job changed, lost progress";}Ģ=ȟ;
Н=й;в=и;Щ=0;Ш=0;Л="";Ѐ=-1;Ĥ=0;Ț=0;С=0;Й=0;ΒЀ(true);}}void Βς(){Р(ή,0);ё(ȉ,true);}void Βϊ(){Ǻ(а.ǃ);Βς();ų(Ȅ,false);Ǧ(ΓА);ί=Λ.Є;Δα(true);ч=ί;π(Ϊ.Ů);Ηδ();ΑΝ=true;Αί=0;for(int i=ŷ.Count-1;i>=0;i--)
if(Ŭ(ŷ[i],false)) Αί++;if(Αί>0) Κ="Started with damage";}void φ(){if(ί==Λ.Є){ί=Λ.ķ;Κ="Job paused";}Ν(Α.з);ч=ί;Ǆ(false,0,0,0,0);Βώ();Δή(new Vector3(),false);ΔБ();Βυ(ChargeMode.Auto);Βχ(false);ΔЍ(true);Ȏ(Α.з);ĕ(false,false,0,0);ё(ο,false);ё(ф,true);Ǧ(true);Αϊ=false;Ư();Ƅ=false;
Βσ();ΑΝ=false;ƚ=false;ơ=false;if(Ѝ!=Ϊ.Main&&Ѝ!=Ϊ.ƫ&&Ѝ!=Ϊ.ΑЏ&&Ѝ!=Ϊ.ǅ)
π(Ϊ.Main);}void ȧ(){ΰ Д=ΒΛ();if(Η==Ζ.Ϋ){if(!Ε.ε||!Θ.ε) return;Βϊ();bool ΓΫ=Vector3.Distance(ή,Θ.Δ)<Vector3.Distance(ή,Ε.Δ);if(Ŝ==Λ.Į) ΓΫ=true;if(Ŝ==Λ.į) ΓΫ=false;if(ΓΫ){ĸ(Θ,Λ.Į);switch(Д){case ΰ.ΒΑ:Ν(Α.ȫ);break;case ΰ.ȍ:Ν(Α.Э);break;case ΰ.ǳ:Ν(Α.ǡ);break;default:Ν(Α.ǐ);break;
}}else{ĸ(Ε,Λ.į);switch(Д){case ΰ.ΒΒ:Ν(Α.ȫ);break;case ΰ.Ǵ:Ν(Α.ǡ);break;case ΰ.ȍ:Ν(Α.Э);break;default:Ν(Α.ǐ);break;}}}else{if(ί!=Λ.ķ&&ί!=Λ.ƭ) return;bool ΙΜ=ί==Λ.ƭ;Βϊ();bool Γΰ=ŭ(false)&&Θ.ε;switch(Д){case ΰ.Ǵ:Ν(Γΰ?Α.ȩ:Α.ǣ);break;case ΰ.ȍ:Ν(Γΰ?Α.Ѕ:Α.Э);break;case ΰ.ǳ:Ν(Γΰ?Α.ǡ:Α.Ǽ);break;
case ΰ.ƪ:{if(Ț!=Й||ΙΜ) Ν(Α.ǹ);else Ν(Α.Ķ);}break;default:break;}}}void Ē(){if(ί==Λ.Я&&!Θ.ε) return;if(Η==Ζ.Ϋ&&(!Ε.ε||!Θ.ε)) return;Κ="Move to job";ΰ Д=ΒΛ();if(Η==Ζ.Ϋ){ĸ(Ε,Λ.į);switch(Д){case ΰ.Ǵ:Ν(Α.ǡ);break;case ΰ.ȍ:Ν(Α.Э);break;case ΰ.ΒΒ:return;default:Ν(Α.ǐ);break;
}Ȏ(Α.ȫ);}else{switch(Д){case ΰ.Ǵ:Ν(Α.Э);break;case ΰ.ȍ:Ν(Α.Э);break;case ΰ.ǳ:Ν(Α.Ǽ);break;case ΰ.ƪ:Ν(Α.ǹ);break;default:break;}if(ί==Λ.Я) Ȏ(Α.Э);else Ȏ(Α.ȥ);Αϊ=true;}Βς();π(Ϊ.Βύ);ų(Ȅ,false);ч=Λ.ƽ;}void ƴ(){if(!Θ.ε) return;Κ="Move home";ΰ Д=ΒΛ();if(Η==Ζ.Ϋ){ĸ(Θ,Λ.Į);switch(Д)
{case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǳ:Ν(Α.ǡ);break;case ΰ.ΒΑ:return;default:Ν(Α.ǐ);break;}Ȏ(Α.ȫ);}else{if(П(MyShipConnectorStatus.Connected)!=null) return;if(П(MyShipConnectorStatus.Connectable)!=null){Ν(Α.đ);Ȏ(Α.Ǽ);return;}switch(Д){case ΰ.Ǵ:Ν(Α.ȩ);break;case ΰ.ȍ:Ν(Α.Ѕ);break;case ΰ.ǳ:Ν(Α.Ѕ);break;
case ΰ.ƪ:Ν(Α.Ǹ);break;default:break;}Ȏ(Α.Ǽ);}Βς();π(Ϊ.Βύ);ų(Ȅ,false);ч=Λ.Ƽ;}void Ν(Α Γϊ){if(Γϊ==Α.з) Ɯ=Α.з;if(Ɯ!=Α.з&&Φ==Ɯ&&Γϊ!=Ɯ){φ();return;}Ё=true;Φ=Γϊ;}void Ȏ(Α Ф){this.Ɯ=Ф;}void ΗϏ(){if(Φ==Α.ǣ){if(Ё){Ђ=0;if(Ț!=Й) Ĥ=0;Ț=Й;}if(Ђ==0){int Ž=ΖЈ(Й);if(Ž<0)
{if(!Ʈ()){Ν(Α.Ǹ);return;}if(!ΑΙ||(Ž==-2)!=ΑΘ){ΑΙ=true;ΑΘ=Ž==-2;ΕΫ=!ΑΘ?ή:Ε.Δ-ъ*δ+
Ε.τ*(Н*ǟ/2+Αό/2+δ/2);}Р(ΕΫ,10);Κ=ΑΘ?"Waiting for a free hole":"Waiting for the controller";return;}if(ΑΙ) Κ="";ΑΙ=false;if(Ž!=Й){Й=Ž;Ĥ=0;Ț=Й;}Ĺ έ=Ηλ(Й);if(έ==Ĺ.Δέ){ί=Λ.ΒΝ;Κ="Job done";if(ċ&&Θ.ε){Ν(Α.ȩ);Ȏ(Α.Ǽ);ч=Λ.Ƽ;}else{Ν(Α.ĳ);Ȏ(Α.ȥ);ч=Λ.ƽ;}
return;}if(έ==Ĺ.Δξ){Ђ=1;ё(ο,true);л=Ȍ(Ш,Щ,true);Р(л,10);Х(Ε.α,Ε.β,Ε.τ,false);}}else{if(Ѐ>=0&&(ΔЁ()?ǫ>=0&&ǫ!=Ѐ:Βί(Ѐ,true))){Ђ=0;return;}if(К<wpReachedDist){Ν(Α.Ķ);return;}}}if(Φ==Α.Ķ){if(Ё){ё(ο,true);Ǧ(false);л=Ȍ(Ш,Щ,false);Р(ΒΨ(л,0),0);Х(Ε.α,Ε.β,Ε.τ,false);
Ђ=1;к=0;Ь=0;ΑΕ=0;Γκ=-1;Ħ=Ц;Γε=true;}if(!Ʈ()){Ν(Α.Ǹ);return;}if(ŭ(true)){ΖΝ=ȋ("","ORE",З.Ď);if((М==ώ.ΒΞ||М==ώ.ư||
М==ώ.Βΰ||М==ώ.Ʒ)&&Η!=Ζ.Ǎ)
Ν(Α.Ĳ);else if((М==ώ.Δη||М==ώ.ΒΘ)&&Η!=Ζ.Ǎ)
Ν(Α.ΒЁ);else
Ν(Α.Ǹ);return;}С=Vector3.Distance(ή,л);if(С>Ĥ){Ĥ=С;Γε=false;}if(Η==Ζ.Ǎ&&Δσ()==MyDetectedEntityType.SmallGrid)
Ь+=2;else Ь-=2;Ь=Math.Max(100,Math.Min(400,Ь));if(Ђ>0&&Ђ<Ь){if(С>к){if(Ь>150) к=С;else к=(float) Math.Ceiling(С);Ђ=1;}else Ђ++;}else{if(Ђ>0){Κ="Ship stuck! Retrying";к=С;Ђ=0;ĕ(false,true,0,δ*sensorRange);}Р(ΒΨ(л,Math.Max(0,к-δ)),ΒΫ(false));if(К<=wpReachedDist/2){Ђ=1;к=0;}
return;}ĕ(false,true,δ*sensorRange,0);Vector3 ΗЉ=л+ъ*С;bool ΕЅ=false;if(Vector3.Distance(ΗЉ,ή)>0.3f){Vector3 ΘΔ=л+ъ*(С+0.1f);Р(ΘΔ,4);ΕЅ=true;}else{float Ǜ=ΒΫ(true);Vector3 Εβ=ΒΨ(л,Math.Max(Ц+1,С+1));ǥ(true,false,false,Εβ,Εβ-л,Ǜ,Ǜ);}bool ΓΞ=false;if(х==я.ΒΔ||х==я.ΒΓ)
{if(!ΕЅ){float Γό=0;foreach(IMyTerminalBlock Ǳ in ο)
Γό+=ΖЏ(Ǳ,"","",х==я.ΒΓ?new string[]{"STONE"}:null);if(Γό>Γκ||С<Ц||Γε){ΒЈ=0;ΑΕ=С;Ħ=(int)(Math.Max(Ħ,ΑΕ)+δ/2);}else{ΓΞ=С-ΑΕ>2&&ΒЈ>=20;ΒЈ++;}Γκ=Γό;}}else ΓΞ=С>=Ħ;if(Ț!=Й){Ν(Α.ǹ);С=0;return;}if(ΓΞ){Й++;Ν(Α.ǹ);С=0;return;}}if(Φ==Α.ǹ||Φ==Α.Ǹ||Φ==Α.Ĳ)
{if(Ё){л=Ȍ(Ш,Щ,false);Х(Ε.α,Ε.β,Ε.τ,false);ё(ο,Ń);Ǧ(false);к=Vector3.Distance(ή,л);ĕ(false,true,0,δ*sensorRange);}Р(л,ΒΫ(false));if(Vector3.Distance(ή,л)>=к+5){ё(ο,false);Ǧ(true);Κ="Can´t return!";}if(К<wpReachedDist){if(Φ==Α.ǹ&&Αϊ) Ν(Α.ĳ);if(Φ==Α.ǹ) Ν(Α.ǣ);
if(Φ==Α.Ĳ) Ν(Α.Ƴ);if(Φ==Α.Ǹ){if(Θ.ε) Ν(Α.ȩ);else{φ();Ē();Κ="Can´t return, no dock found";}}return;}}if(Φ==Α.Ѕ||Φ==Α.Э){if(Φ==Α.Э&&ί==Λ.Є&&Η!=Ζ.Ϋ){if(!Ʈ()||ŭ(true)){Ν(Α.Ѕ);return;}}bool ΓЅ=false;bool ΑЋ=false;bool ΕΞ=false;float ľ=0;bool ΖΜ=false;Ψ Ω=null;
if(Ё){if(Φ==Α.Ѕ||Η==Ζ.Ϋ){if(Η!=Ζ.Ϋ){if(ȏ(1)) ΑБ();else{ј=null;Ȇ=0;Ƅ=!ΒΗ(1);}}Ψ ά=ж();η=new Βρ();η.Њ=ά;η.Ƃ=followPathDock*δ;η.ΔΟ=useDockDirectionDist*δ;η.ľ=10;η.ş.Add(ά.Δ);Vector3 Ľ=new Vector3();if(Ƕ(ά,dockDist*δ,true,out Ľ))
η.ş.Add(Ľ);else η.Ƃ*=1.5f;if(Η==Ζ.Ϋ){if(ά==Θ) η.ΑΨ=Ε.Δ;if(ά==Ε) η.ΑΨ=Θ.Δ;η.ΓΨ=dockDist*δ*1.1f;}}else if(Φ==Α.Э){η=new Βρ();η.Њ=Ε;η.Ƃ=followPathJob*δ;η.ΔΟ=useJobDirectionDist*δ;η.ľ=10;η.ΑΨ=Θ.Δ;η.ΓΨ=dockDist*δ*1.1f;η.ş.Add(Ε.Δ);if(ί==Λ.Я){if(!Θ.ε||Σ.Count==0){φ();return;}
float ΘΛ=Vector3.Distance(Σ.First().Δ,Θ.Δ);float ΘΜ=Vector3.Distance(Σ.Last().Δ,Θ.Δ);if(ΘΛ<ΘΜ) η.Њ=Σ.Last();else η.Њ=Σ.First();}}ƕ=new Vector3();ΖΜ=!ΗΣ(ή);ё(ο,false);Ǧ(true);У=-1;double Ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){if(Vector3.Distance(Σ[i].Δ,η.ΑΨ)<=η.ΓΨ) continue;
double Ϗ=Vector3.Distance(Σ[i].Δ,ή);if(Ǫ==-1||Ϗ<Ǫ){У=i;Ǫ=Ϗ;}}ǯ=ΗΙ(η.Њ.Δ,У);ě=null;}if(Φ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&!Ŵ()){Ψ ΒЌ=ж();Ƅ=!ΒΗ(1);if(ж()!=ΒЌ){Ν(Α.Ѕ);return;}}if(Φ==Α.Ѕ&&Η!=Ζ.Ϋ&&!Ё&&ȏ(1)&&ΑБ()){Ν(Α.Ѕ);return;}bool ΕЃ=false;if(Φ==Α.Ѕ&&!Ё&&У>=0&&У<Σ.Count)
{Vector3 ſ=Σ[У].Δ+Δτ(У,ǯ);int Γϋ=У+ǯ;Vector3 Œ=ǯ!=0&&Γϋ>=0&&Γϋ<Σ.Count?Σ[Γϋ].Δ:η.Њ.Δ;if(ΗΜ(ſ,Œ)){if(!ΑΡ){ΑΡ=true;Εη=Ĵ(ſ)?ή:ſ;}Р(Εη,0);Х(η.Њ.α,η.Њ.β,η.Њ.τ,false);Κ="Waiting for dock clearance";return;}ΑΡ=false;ΕЃ=ȏ(1)&&(Ĵ(ή)||Ĵ(Œ));}ΖЁ(Σ,ǯ,η.ş,η.Ƃ,Ё,ref Ђ);
for(int i=0;i<η.ş.Count;i++){float Ϗ=Vector3.Distance(ή,η.ş[i]);if(Ϗ<=η.Ƃ) ΓЅ=true;if(Ϗ<=η.ΔΟ) ΑЋ=true;}if(ΕЃ) ΑЋ=true;if(ΑЋ) ľ=η.ľ;float Ιί=ě!=null?ě.Џ:Ȟ;float ΙΡ=(float) Math.Max(Ȟ*0.1f*δ,wpReachedDist);if((К<ΙΡ)||Ё){if(!Ё) У+=ǯ;if(ǯ==0||У>Σ.Count-1||У<0)
ΓЅ=true;else{ΔΥ=ǯ>0?Σ.Count-1-У:У;Ω=Σ[У];ě=Ω;if(У>=1&&У<Σ.Count-1)
ƕ=Ω.Δ-Σ[У-ǯ].Δ;else ě=null;υ=Ω.Δ+Δτ(У,ǯ);ΕΞ=true;}}if(ΑЋ)
Х(η.Њ.α,η.Њ.β,η.Њ.τ,false);else if(ΖΜ)
ΑА(η.Њ.α,10,true);else if(ΕΞ&&Ω!=null){if(ǯ>0) Х(Ω.α,Ω.β,Ω.τ,90,false);else Х(Ω.α,-Ω.β,-Ω.τ,90,false);}ǥ(true,false,true,υ,ƕ,ě==null?0:ě.Џ,ľ);if(ΓЅ){ΔΥ=0;if(Φ==Α.Ѕ&&!ΖІ()){Р(υ,0);return;}if(Φ==Α.Ѕ&&Ƅ){Р(υ,0);Κ="Waiting for a free dock";return;}if(Φ==Α.Ѕ||Η==Ζ.Ϋ){Ν(Α.ǡ);return;}
if(Φ==Α.Э&&Αϊ){Ν(Α.ĳ);return;}if(Φ==Α.Э){Ν(Α.ǣ);return;}}}if(Φ==Α.ǡ||Φ==Α.ű){if(Η!=Ζ.Ϋ&&Ŵ()){if(!ȏ(1)){Ν(Α.Ѕ);return;}}else if(Η!=Ζ.Ϋ){Ψ ΒЌ=ж();if(!ΒΗ(2)||ж()!=ΒЌ){Ν(Α.Ѕ);return;}}Ψ ά=ж();if(Ё){if(!Ƕ(ά,dockDist*δ,true,out υ)){Ǻ(а.ŧ);φ();return;}
Р(υ,0);ΑА(ά.α,90,true);}if(К<followPathDock*δ&&К!=-1){Р(υ,10);Х(ά.α,ά.β,ά.τ,false);}if(П(MyShipConnectorStatus.Connectable)!=null||П(MyShipConnectorStatus.Connected)!=null){Ν(Α.đ);return;}if(К<wpReachedDist/2&&К!=-1){Ν(Α.č);return;}}if(Φ==Α.č||Φ==Α.ȥ){if(Ё)
{if(Φ==Α.č){Ψ ά=ж();if(!Ƕ(ά,dockDist*δ,true,out υ)){Ǻ(а.ŧ);φ();return;}ǥ(true,true,false,υ,0);Х(ά.α,ά.β,ά.τ,10,false);}if(Φ==Α.ȥ){Х(Ε.α,Ε.β,Ε.τ,0.5f,false);υ=Ε.Δ;ǥ(true,true,false,υ,0);}}if(Αα){Ǆ(false,0,0,0,0);if(Φ==Α.č) Ν(Α.đ);if(Φ==Α.ȥ) φ();return;}}if(Φ==Α.đ)
{if(П(MyShipConnectorStatus.Connected)!=null){if(Η==Ζ.Ϋ) Ν(Α.ȫ);else Ν(Α.Ǽ);return;}Ψ ά=ж();if(Ё){Ь=0;ΘΝ=DateTime.Now;Ђ=0;Х(ά.α,ά.β,ά.τ,false);}Vector3I Γέ=new Vector3I((int) ά.Ч.X,(int) ά.Ч.Y,(int) ά.Ч.Z);IMySlimBlock ΕΪ=Me.CubeGrid.GetCubeBlock(Γέ);float Ει=dockingSpeed;float Θζ=dockingSpeed*5;
float ΕΛ=Math.Max(1.5f,Math.Min(5f,δ*0.15f));if(!Ƕ(ά,0,true,out υ)||!Ƕ(ά,ΕΛ,true,out ƕ)||
ΕΪ==null||!ΕΪ.FatBlock.IsFunctional){Ǻ(а.ŧ);φ();return;}if(Ь==1||(Vector3.Distance(ή,υ)<=ΕΛ*1.1f&&!Ё))
Ь=1;else{Vector3 ΙΑ=Ю(Τ,ƕ-ή);Vector3 Γά=Ю(Τ,Τ.GetNaturalGravity());float Θύ=Ƭ(ΙΑ,Γά,null);Ει=Math.Min(Θζ,Θύ);}ǥ(true,false,false,υ,υ-ή,dockingSpeed,Ει);if(Ё) к=(float) К;IMyShipConnector ΓΙ=П(MyShipConnectorStatus.Connectable);if(ΓΙ!=null){ǥ(false,false,false,υ,0);
if(Ђ>0) Ђ=0;Ђ--;if(Ђ<-5){ΓΙ.Connect();if(ΓΙ.Status==MyShipConnectorStatus.Connected){if(Η==Ζ.Ϋ) Ν(Α.ȫ);else Ν(Α.Ǽ);Βώ();ų(Ȅ,true);return;}}}else{float Εζ=(float) Math.Round(К,1);if(Εζ<к){Ђ=-1;к=Εζ;}else Ђ++;if(Ђ>20){Ν(Α.ű);return;}}}if(Φ==Α.Ǽ||Φ==Α.ȫ||
Φ==Α.ǋ||Φ==Α.Ʋ||Φ==Α.Đ){bool ΒІ=false,ΒЇ=false;if(Η==Ζ.Ϋ){if(ж()==Θ) ΒІ=true;else if(ж()==Ε) ΒЇ=true;}if(Ё){ƚ=false;if(П(MyShipConnectorStatus.Connected)==null){Ν(Α.ǐ);return;}Βώ();if(ΒІ) ǈ(ϋ.ǔ);if(ΒЇ) ǈ(ό.ǔ);Ī=false;Ȑ=false;ȗ=false;ƣ=false;}if(П(MyShipConnectorStatus.Connected)==null){φ();Ǻ(а.Δύ);return;}
if(ί!=Λ.Є||ш==-1||Ǩ==с.None) Ȑ=true;else if(ǽ>=100f) Ȑ=true;else if(ǽ<=99f) Ȑ=false;if(ί!=Λ.Є||щ==-1||ѕ.Count==0) ȗ=true;else if(Ș>=100f) ȗ=true;else if(Ș<=99) ȗ=false;if(ί!=Λ.Є||Ъ==-1||Ĉ.Count==0) ƣ=true;else ƣ=ī>=Ъ;ĺ р=null;if(ΒІ) р=ϋ;
if(ΒЇ) р=ό;if(р!=null&&(р.Ќ==ι.ǀ||р.Ќ==ι.ƿ)) Ȑ=true;if(р!=null&&(р.Ќ==ι.Βθ)) if(!Ī) Ȑ=false;if(р!=null&&(р.Ќ==ι.Βμ||р.Ќ==ι.Βκ)) ȗ=true;if(р!=null&&(р.Ќ==ι.Βλ)) if(!Ī) ȗ=false;if(ą){ChargeMode ΕΩ=Ȑ?ChargeMode.Auto:ChargeMode.Recharge;if(р!=null&&(р.Ќ==ι.ƿ||р.Ќ==ι.ǀ))
ΕΩ=ChargeMode.Discharge;Βυ(ΕΩ);Βχ(!ȗ);}if(!Ī){if(Η==Ζ.Ϋ)
Ī=ί!=Λ.Є||ΖЇ(Ё,true)||ơ;else
Ī=ί!=Λ.Є||Δώ();}else{if(!Ȑ) Ν(Α.Đ);if(!ȗ) Ν(Α.Ʋ);if(!ƣ) Ν(Α.ǋ);Ё=false;}if(Ī&&Ȑ&&ȗ&&ƣ){Βυ(ChargeMode.Auto);Βχ(false);if(ί==Λ.Є){if(Η==Ζ.Ϋ){if(ж()==Θ) ǈ(ϋ.Ǘ);else if(ж()==Ε) ǈ(ό.Ǘ);if(ж()==Θ) ĸ(Ε,Λ.į);else ĸ(Θ,Λ.Į);}}Ν(Α.ǐ);return;}}if(Φ==Α.ǐ){if(Ʀ){if(!Δδ()) return;
Ʀ=false;Ν(Α.ǐ);return;}if(Ё){if(!Δδ()){Ʀ=true;return;}IMyShipConnector Ȁ=П(MyShipConnectorStatus.Connected);if(Ȁ==null){Ν(Α.Э);return;}IMyShipConnector γ=Ȁ.OtherConnector;ΔЌ(Ȁ,false);ų(Ȅ,false);Ψ ά=null;Ψ ΕЀ=Η==Ζ.Ϋ?Θ:ж();if(Vector3.Distance(Ȁ.GetPosition(),ΕЀ.Δ)<5f&&Θ.ε) ά=ΕЀ;
if(Vector3.Distance(Ȁ.GetPosition(),Ε.Δ)<5f&&Ε.ε) ά=Ε;if(ά!=null){if(!Ƕ(ά,dockDist*δ,true,out υ)){Ǻ(а.ŧ);φ();return;}Р(υ,5);Х(ά.α,ά.β,ά.τ,false);}else Р(ή+γ.WorldMatrix.Forward*dockDist*δ,5);if(ί==Λ.Є) Ǻ(а.ǃ);}if(К<wpReachedDist){ё(ф,true);Ν(Α.Э);return;}}if(Φ==Α.ĳ)
{if(Ё){Ǧ(true);ё(ο,false);υ=Ε.Δ;Р(υ,20);Х(Ε.α,Ε.β,Ε.τ,false);}if(К<wpReachedDist/2){Ν(Α.ȥ);return;}}if(Φ==Α.ȩ){if(Ё){Ǧ(true);ё(ο,false);int Б=-1;double Ǫ=-1;for(int i=Σ.Count-1;i>=0;i--){double Ϗ=Vector3.Distance(Σ[i].Δ,ή);if(Ǫ==-1||Ϗ<Ǫ){Б=i;Ǫ=Ϗ;}}
if(Б==-1){Ν(Α.Ѕ);return;}υ=Σ[Б].Δ;Р(υ,10);Х(Ε.α,Ε.β,Ε.τ,false);}if(К<wpReachedDist){Ν(Α.Ѕ);return;}}if(Φ==Α.Ƴ){bool є=false;if(Ё){Ǧ(true);if((М==ώ.ΒΞ||М==ώ.ư)&&ƹ()&&
ı(ъ,Τ.GetNaturalGravity())<25&&Н>=2&&в>=2){Vector3 Ɔ=ή;if(Ш>0&&Щ<в-1) Ɔ=Ȍ(Ш-1,Щ+1,true);else if(Ш<Н-1&&Щ<в-1) Ɔ=Ȍ(Ш+1,Щ+1,true);else if(Ш<Н-1&&Щ>0) Ɔ=Ȍ(Ш+1,Щ-1,true);else if(Ш>0&&Щ>0) Ɔ=Ȍ(Ш-1,Щ-1,true);else є=true;if(!є) Р(Ɔ,10);
}else є=true;}if(К<wpReachedDist/2) є=true;if(є){Ν(Α.Ǌ);return;}}if(Φ==Α.ΒЁ||Φ==Α.Ǌ){if(Ё){ǥ(true,true,false,ή,0);ё(ο,false);Ǧ(true);Ђ=-1;Ь=М==ώ.Βΰ||М==ώ.Ʒ?0:-1;}bool ĝ=!Ʈ();int ΑЄ=ȋ("STONE","ORE",З.Ď);if(М==ώ.ΒΘ||М==ώ.Ʒ||М==ώ.ư)
ΑЄ+=ȋ("ICE","ORE",З.Ď);bool ΘΧ=ΑЄ>0;bool ΓΧ=false;if(Ь>=0){float Ļ=(float) Math.Sin(Δι(Ь))*ǟ/3f;float ΗЋ=(float) Math.Cos(Δι(Ь))*ǲ/3f;Vector3 ΗА=Ȍ(Ш,Щ,true)+ΒϏ(ъ,ȅ*-1,new Vector3(Ļ,ΗЋ,0));Р(ΗА,0.3f);if(К<Math.Min(ǟ,ǲ)/10f) Ь+=5f;if(Ь>=360) Ь=0;}if(Ђ==-1||ΑЄ<Ђ){Ђ=ΑЄ;к=0;}
else{к++;if(к>50) ΓΧ=true;}if(!ΘΧ||ĝ||ΓΧ){if(!ĝ){int ΙΖ=ȋ("","ORE",З.Ď);if(ŭ(true)) ĝ=true;else if(100-(А(ΙΖ,ΖΝ)*100)<minEjection) ĝ=true;else Ǻ(а.ǃ);}if(ΓΧ&&ĝ) Κ="Ejection failed";if(Φ==Α.Ǌ){if(ĝ){if(Θ.ε) Ν(Α.ȩ);else{φ();Ē();Κ="Can´t return, no dock found";}}
else Ν(Α.ǣ);}else if(ĝ) Ν(Α.Ǹ);else Ν(Α.Ķ);return;}}Ё=false;}void ΖЁ(List<Ψ>Č,int ζ,List<Vector3>ƞ,float ǝ,bool ȇ,ref int ǒ){if(ȇ){for(int i=0;i<Č.Count;i++) Č[i].Џ=0;ǒ=-1;return;}if(ζ==0) return;int ћ=ζ*-1;if(ǒ==-1) ǒ=ћ>0?1:Č.Count-2;int ΕЇ=0;while(ǒ>=1&&ǒ<Č.Count-1)
{if(ΕЇ>50) return;ΕЇ++;try{if((ћ<0&&ǒ>=1)||(ћ>0&&ǒ<=Č.Count-2)){Ψ θ=Č[ǒ];bool ΖΙ=false;for(int j=0;j<ƞ.Count;j++)
if(Vector3.Distance(θ.Δ,ƞ[j])<=ǝ){ΖΙ=true;break;}if(!ΖΙ){Ψ Αφ=Č[ǒ-ћ];Ψ Œ=Č[ǒ+ћ];Vector3 ΙΒ=θ.Δ-Œ.Δ;Vector3 ΔΚ=Αφ.Δ-θ.Δ;Vector3 Θψ=θ.Δ+Vector3.Normalize(ΙΒ)*ΔΚ.Length();Vector3 ΘΙ=Αφ.Δ-Θψ;Vector3 Αθ=Ĕ(ζ>0?θ.β:θ.β*-1,θ.α*-1,ΘΙ);Vector3 Θε=Ĕ(ζ>0?θ.β:θ.β*-1,θ.α*-1,ΔΚ);
Vector3 Ακ=Ĕ(ζ>0?θ.β:θ.β*-1,θ.α*-1,θ.ν);θ.Џ=(float) Math.Sqrt(Math.Pow(Αφ.Џ,2)+Math.Pow(Ƭ(-Θε,Ακ,θ),2));for(int j=0;j<ƞ.Count;j++)
if(Vector3.Distance(Αφ.Δ,ƞ[j])<=ǝ){Vector3 Αλ=Ĕ(ζ>0?θ.β:θ.β*-1,θ.α*-1,ƞ[j]-θ.Δ);float ΘЋ=Ƭ(-Αλ,Ακ,θ);θ.Џ=Math.Min(θ.Џ,ΘЋ)/2f;}if(Αθ.Length()==0) Αθ=new Vector3(0,0,1);Vector3 Θδ=Ĕ(θ.β,θ.α*-1,θ.ν);float ΘЍ=ũ(-Αθ,Θδ,θ);float ǌ=А(ΘЍ,Ĩ);float ΔΘ=(float) Math.Sqrt(А(Αθ.Length()*1.0f,0.5f*ǌ));
θ.Џ=Math.Min(θ.Џ,(ΔΚ.Length()/ΔΘ)*ǧ);}}}catch{return;}ǒ+=ћ;}ǒ=-1;}void ΒЀ(bool ȇ){if(ȇ){ƌ=0;return;}if(Η==Ζ.Ϋ) return;float є=Й*Math.Max(1,Ц);if(Ț==Й) є+=Math.Min(Ц,С);float ΙΕ=Н*в*Math.Max(1,Ц);ƌ=Math.Max(ƌ,(float) Math.Min(є/ΙΕ*100.0,100));}MyDetectedEntityType Δσ()
{try{if(Ƿ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΒΫ(bool ǭ){if(Η==Ζ.Ǎ&&Δσ()==MyDetectedEntityType.None&&!Ŭ(ξ,true))
return fastSpeed;else return ǭ?ť:Ť;}void Ηω(){float ΖΓ=0,Εΰ=0,ġ=0,Γύ=0;for(int i=0;i<Ȭ.Count;i++){IMyBatteryBlock Ż=Ȭ[i];if(!Ƿ(Ż,true)) continue;ΖΓ+=Ż.MaxStoredPower;Εΰ+=Ż.CurrentStoredPower;ġ+=Ż.CurrentInput;Γύ+=Ż.CurrentOutput;}ǽ=(float) Math.Round(А(Εΰ,ΖΓ)*100,1);
if(ġ>=Γύ) Ǩ=с.Đ;else Ǩ=с.Δκ;if(ġ==0&&Γύ==0||ǽ==100.0) Ǩ=с.з;if(Ȭ.Count==0) Ǩ=с.None;}void Ηύ(){float Εο=0;for(int i=0;i<ѕ.Count;i++){IMyGasTank Ζμ=ѕ[i];if(!Ƿ(Ζμ,true)) continue;Εο+=(float) Ζμ.FilledRatio;}Ș=А(Εο,ѕ.Count)*100f;}void ΗЅ(){ī=0;try{for(int i=0;i<Ĉ.Count;i++)
{IMyReactor ΓІ=Ĉ[i];if(!Ƿ(ΓІ,true)) continue;List<MyInventoryItem>б=new List<MyInventoryItem>();ΓІ.GetInventory(0).GetItems(б);float Ǒ=0;for(int j=0;j<б.Count;j++){MyInventoryItem Г=б[j];if(Г.Type.SubtypeId.ToUpper()=="URANIUM"&&Г.Type.TypeId.ToUpper().Contains("_INGOT"))
Ǒ+=(float) Г.Amount;}if(Ǒ<ī||i==0){ī=Ǒ;Γς=ΓІ.CustomName;}}}catch(Exception e){ș=e;}}void Ηϊ(){Ɛ=0;ƀ=0;try{for(int i=0;i<ł.Count;i++){IMyTerminalBlock Υ=ł[i];if(!Ƿ(Υ,true)) continue;ƀ+=(float) Υ.GetInventory(0).CurrentVolume;Ɛ+=(float) Υ.GetInventory(0).MaxVolume;
}Αη=(float) Math.Min(Math.Round(А(ƀ,Ɛ)*100,1),100.0);}catch(Exception e){ș=e;}}void Ηώ(){try{ǖ.Clear();for(int i=0;i<ł.Count;i++){IMyTerminalBlock Υ=ł[i];if(!Ƿ(Υ,true)) continue;Δά(Υ,З.ū);}if(М!=ώ.Βη){for(int i=0;i<Ę.Count;i++){IMyTerminalBlock Υ=Ę[i];if(!Ƿ(Υ,true)) continue;
Δά(Υ,З.ΗΟ);}}Ηρ(ǖ);}catch(Exception e){ș=e;}}void ΖЅ(){if(ΑΝ){if(Δς().Count>Αί){ΑΝ=false;if(н!=е.Ū){φ();if(н==е.ΒΤ) Ē();if(н==е.ǂ)
if(Θ.ε) ƴ();else Ē();}Κ="Damage detected";}}}bool Ʈ(){if(!Αΰ) return true;if(ί==Λ.Є){if(ш>0&&Ǩ!=с.None)
if(ǽ<=ш){Κ="Low energy! Move home";return false;}if(Ъ>0&&Ĉ.Count>0)
if(ī<=Ъ){Κ="Low fuel: "+Γς;return false;}if(щ>0&&ѕ.Count>0)
if(Ș<=щ){Κ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>Δς(){List<IMyTerminalBlock>Ĝ=new List<IMyTerminalBlock>();for(int i=0;i<ŷ.Count;i++){IMyTerminalBlock Υ=ŷ[i];if(Ŭ(Υ,false)) Ĝ.Add(Υ);}return Ĝ;}bool Ŭ(IMyTerminalBlock Υ,bool ΔΑ){return(!Ƿ(Υ,ΔΑ)||!Υ.IsFunctional);
}bool Ƿ(IMyTerminalBlock Υ,bool ΔΑ){if(Υ==null) return false;try{IMyCubeBlock Εφ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΔΑ) return Εφ==Υ;else return Εφ.GetType()==Υ.GetType();}catch{return false;}}float ΖЏ(IMyTerminalBlock Υ,String ю,String Ζγ,String[]Ǭ){float ȁ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory ŉ=Υ.GetInventory(i);List<MyInventoryItem>б=new List<MyInventoryItem>();ŉ.GetItems(б);foreach(MyInventoryItem Г in б){if(Ǭ!=null&&(Ǭ.Contains(Г.Type.TypeId.ToUpper())||Ǭ.Contains(Г.Type.SubtypeId.ToUpper())))
continue;if((ю==""||Г.Type.TypeId.ToUpper()==ю)&&(Ζγ==""||Г.Type.SubtypeId.ToUpper()==Ζγ))
ȁ+=(float) Г.Amount;}}return ȁ;}Ǥ ΗΔ(String ψ,String ς,З Д,bool ΘΕ){ψ=ψ.ToUpper();ς=ς.ToUpper();for(int i=0;i<ǖ.Count;i++){Ǥ Г=ǖ[i];if(Г.ψ.ToUpper()==ψ&&Г.ς.ToUpper()==ς&&(Г.Ŏ==Д||Д==З.Ď))
return Г;}Ǥ έ=null;if(ΘΕ){έ=new Ǥ(ψ,ς,0,Д);ǖ.Add(έ);}return έ;}int ȋ(String ψ,String ς,З Д){return ȋ(ψ,ς,Д,null);}int ȋ(String ψ,String ς,З Д,String[]Ǭ){int ȁ=0;ψ=ψ.ToUpper();ς=ς.ToUpper();for(int i=0;i<ǖ.Count;i++){Ǥ Г=ǖ[i];if(Ǭ!=null&&Ǭ.Contains(Г.ψ.ToUpper())) continue;
if((ψ==""||Г.ψ.ToUpper()==ψ)&&(ς==""||Г.ς.ToUpper()==ς)&&(Г.Ŏ==Д||Д==З.Ď))
ȁ+=Г.Ǒ;}return ȁ;}void Δά(IMyTerminalBlock Υ,З Д){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>б=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(б);for(int j=0;j<б.Count;j++)
ΗΔ(б[j].Type.SubtypeId,б[j].Type.TypeId.Replace("MyObjectBuilder_",""),Д,true).Ǒ+=(int) б[j].Amount;}}void Ηρ(List<Ǥ>Т){for(int i=Т.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ǥ a=Т[j];Ǥ b=Т[j+1];if(a.Ǒ<b.Ǒ) Т.Move(j,j+1);}}bool ŭ(bool Ιά){if(Ȥ&&Η!=Ζ.Ϋ)
if(О!=-1&&Ĩ>=О){Κ="Ship too heavy";return true;}if(Αη>=Ǯ||ƚ){ƚ=false;Κ="Ship is full";return true;}return false;}bool Δώ(){String[]Ǭ=null;if(!Ȣ) Ǭ=new string[]{"ICE"};if(Η==Ζ.Ű) return ȋ("","ORE",З.ū,Ǭ)==0;if(Η==Ζ.Ǎ) return ȋ("","COMPONENT",З.ū,Ǭ)==0;else return ȋ("","",З.ū,Ǭ)==0;
}bool ΗΤ(){List<IMyCockpit>ΓΘ=new List<IMyCockpit>();ϊ.GetBlocksOfType(ΓΘ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<ΓΘ.Count;i++)
if(ΓΘ[i].IsUnderControl) return true;return false;}bool ΖЇ(bool ȇ,bool Αώ){IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null) return false;if(Vector3.Distance(Θ.Δ,И.GetPosition())<5)
return Δε(ϋ,ȇ,Αώ);if(Vector3.Distance(Ε.Δ,И.GetPosition())<5)
return Δε(ό,ȇ,Αώ);return false;}bool Δε(ĺ ǿ,bool ȇ,bool Αώ){if(ȇ) ǿ.Ηΰ();ǿ.ΗЂ();bool п=false;String ь="";switch(ǿ.Ќ){case ι.Βι:{ь="Waiting for command";п=false;break;}case ι.ΔЇ:{ь="Waiting for cargo";п=ŭ(true);break;}case ι.ΔІ:{ь="Unloading";п=Δώ();break;}case ι.Βξ:{п=true;break;}
case ι.Βθ:{ь="Charging batteries";п=ǽ>=100f;break;}case ι.ǀ:{ь="Discharging batteries";п=ǽ<=25f;break;}case ι.ƿ:{ь="Discharging batteries";п=ǽ<=0f;break;}case ι.Βλ:{ь="Filling up hydrogen";п=Ș>=100f;break;}case ι.Βμ:{ь="Unloading hydrogen";п=Ș<=25f;break;}
case ι.Βκ:{ь="Unloading hydrogen";п=Ș<=0f;break;}case ι.Βν:{bool ΕЂ=ΗΤ();if(!ΕЂ) ǿ.ΓЂ=true;п=ǿ.ΓЂ&&ΕЂ;ь="Waiting for passengers";break;}}if(!п) ǿ.ΒΖ(true);if(п&&ǿ.Βέ()){п=ǿ.ΒΖ(false);ь="Undocking in: "+ΒΦ((int) Math.Max(0,ǿ.Ǔ-ǿ.ȱ));}if(Αώ) Ζω=ь;return п;}void Ζύ()
{if(!Ŋ) return;if(ο.Count<=1) return;float Ζς=0,Ζρ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǳ=ο[i];if(Ŭ(Ǳ,true)) continue;Ζς+=(float) Ǳ.GetInventory(0).MaxVolume;Ζρ+=(float) Ǳ.GetInventory(0).CurrentVolume;}float ΕΟ=(float) Math.Round(А(Ζρ,Ζς),5);for(int ΖΠ=0;ΖΠ<Math.Max(1,Math.Floor(ο.Count/10f));ΖΠ++)
{float Γβ=0,Γρ=0;float Εώ=0,ΖΑ=0;IMyTerminalBlock ȕ=null,Ȳ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǳ=ο[i];if(Ŭ(Ǳ,true)) continue;float Γψ=(float) Ǳ.GetInventory(0).MaxVolume;float ΑΩ=А((float) Ǳ.GetInventory(0).CurrentVolume,Γψ);if(ȕ==null||ΑΩ>Γβ){ȕ=Ǳ;Γβ=ΑΩ;Εώ=Γψ;}
if(Ȳ==null||ΑΩ<Γρ){Ȳ=Ǳ;Γρ=ΑΩ;ΖΑ=Γψ;}}if(ȕ==null||Ȳ==null||ȕ==Ȳ) return;if(checkConveyorSystem&&!ȕ.GetInventory(0).IsConnectedTo(Ȳ.GetInventory(0))){if(ΒЋ>20) Κ="Inventory balancing failed";else ΒЋ++;return;}ΒЋ=0;List<MyInventoryItem>б=new List<MyInventoryItem>();
ȕ.GetInventory(0).GetItems(б);float ƥ=0;if(б.Count==0) continue;MyInventoryItem ΑΪ=б[0];String ΕІ=ΑΪ.Type.TypeId+ΑΪ.Type.SubtypeId;if(!Ζψ.TryGetValue(ΕІ,out ƥ)){if(ΗΩ(ȕ.GetInventory(0),0,Ȳ.GetInventory(0),out ƥ))
Ζψ.Add(ΕІ,ƥ);else return;}float ΙΓ=((Γβ-ΕΟ)*Εώ/ƥ);float ΙΔ=((ΕΟ-Γρ)*ΖΑ/ƥ);int ΔΝ=(int) Math.Min(ΙΔ,ΙΓ);if(ΔΝ<=0) return;if((float) ΑΪ.Amount<ΔΝ)
ȕ.GetInventory(0).TransferItemTo(Ȳ.GetInventory(0),0,null,null,ΑΪ.Amount);else
ȕ.GetInventory(0).TransferItemTo(Ȳ.GetInventory(0),0,null,null,ΔΝ);}}bool ΗΩ(IMyInventory ƛ,int Β,IMyInventory ΘΘ,out float Ζχ){Ζχ=0;float ΗЎ=(float) ƛ.CurrentVolume;List<MyInventoryItem>ΒЍ=new List<MyInventoryItem>();ƛ.GetItems(ΒЍ);float ΕΣ=0;for(int i=0;i<ΒЍ.Count;i++) ΕΣ+=(float) ΒЍ[i].Amount;
ƛ.TransferItemTo(ΘΘ,Β,null,null,1);float Ζφ=ΗЎ-(float) ƛ.CurrentVolume;List<MyInventoryItem>ΒЃ=new List<MyInventoryItem>();ƛ.GetItems(ΒЃ);float ΕΗ=0;for(int i=0;i<ΒЃ.Count;i++) ΕΗ+=(float) ΒЃ[i].Amount;if(Ζφ==0f||!Βα(0.9999,ΕΣ-ΕΗ,1.0001))
return false;Ζχ=Ζφ;return true;}void Ηό(){Vector3 Ɖ=υ+ȡ;Vector3 ǰ=Ɖ-ή;if(ǰ.Length()==0) ǰ=new Vector3(0,0,-1);Vector3 Αλ=Ю(Τ,ǰ);Vector3 Ζν=Vector3.Normalize(Αλ);Vector3 ō=Ю(Τ,Τ.GetNaturalGravity());float ΘІ=ΓЀ>0?Math.Max(0,1-ı(ǰ,Ερ)/5):0;float Џ=(float) Math.Min((Γυ>0?Γυ:1000f),Math.Max(Ƭ(-Αλ,ō,null),ΓЀ*ΘІ));
if(!Ζπ) Џ=0;if(Ζέ) Џ=Math.Max(0,1-ΑΛ/ΑΑ)*Џ;if(generalSpeedLimit>0) Џ=Math.Min(generalSpeedLimit,Џ);if(Ŗ) Џ=0;if(э>=0) Џ=Math.Min(Џ,э);if(Ζή) Џ*=(float) Math.Min(1,А(ǰ.Length(),wpReachedDist)/2);Vector3 ΕА=Ю(Τ,Τ.GetShipVelocities().LinearVelocity);float ΗЊ=(float)(Math.Max(0,15-ı(-Ζν,-ΕА))/15)*0.85f+0.15f;
ΔΕ+=Math.Sign(ΗЊ-ΔΕ)/10f;Vector3 ȭ=Ζν*Џ*ΔΕ-(ΕА);Vector3 ΒЊ=ΒΪ(ȭ,null);if(ΑІ&&К>0.1f){ȭ.X*=Βω(ȭ.X,ref ΑЇ.X,1f,ΒЊ.X,20);ȭ.Y*=Βω(ȭ.Y,ref ΑЇ.Y,1f,ΒЊ.Y,20);ȭ.Z*=Βω(ȭ.Z,ref ΑЇ.Z,1f,ΒЊ.Z,20);}else ΑЇ=new Vector3(1,1,1);Ɵ=Ĩ*ȭ-ō*Ĩ;Δή(Ɵ,ΑІ);К=Vector3.Distance(ή,Ɖ);
}float Βω(float value,ref float Ф,float ћ,float ț,float Γφ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(Ф)) Ф+=Math.Sign(Ф)*ћ;else Ф=value;if(value==0) Ф=1;float έ=Math.Abs(Ф);if(έ<Γφ||ț==0) return έ;Ф=Math.Min(Γφ,Math.Max(-Γφ,Ф));έ=Math.Abs(ț);return έ;
}void ΕΕ(){float ы=90,џ=90,њ=90;float Ǜ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǭ,ї,Ə,Γξ;if(Γο){ǭ=Vector3.Normalize(υ-ή);Ə=Ю(Τ,ǭ);ї=Ю(Τ,ΔΓ);ы=ı(Ə,new Vector3(0,-1,0))-90;џ=ď(ї,new Vector3(-1,0,0),ї.Y);
њ=ď(Ə,new Vector3(-1,0,0),Ə.Z);}else{ǭ=ΔΔ;ї=Ю(Τ,ΔΓ);Ə=Ю(Τ,ΔΔ);Γξ=Ю(Τ,Ζξ);ы=ď(ї,new Vector3(0,0,1),ї.Y);џ=ď(ї,new Vector3(-1,0,0),ї.Y);њ=ď(Γξ,new Vector3(0,0,1),Γξ.X);}if(ΕΙ&&ƹ()){Vector3 Γά=Τ.GetNaturalGravity();ї=Ю(Τ,Γά);ы=ď(ї,new Vector3(0,0,1),ї.Y);
џ=ď(ї,new Vector3(-1,0,0),ї.Y);}if(!Βα(-45,џ,45)){ы=0;њ=0;}if(!Βα(-45,њ,45)) ы=0;Ǆ(Γή,1,(-ы)*Ǜ,(-њ)*Ǜ,(-џ)*Ǜ);ΑΛ=Math.Max(Math.Abs(ы),Math.Max(Math.Abs(џ),Math.Abs(њ)));Αα=ΑΛ<=ΑΑ;}void ΖЂ(IMyTerminalBlock Αχ){if(Αχ==null) return;Ǡ=new float[3,2];ќ=new Dictionary<string,float[,]>();
for(int i=0;i<ȉ.Count;i++){IMyThrust г=ȉ[i];if(!г.IsFunctional) continue;Vector3 ζ=Ю(Αχ,г.WorldMatrix.Backward);float Ȱ=г.MaxEffectiveThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) Ǡ[0,0]+=Ȱ;else Ǡ[0,1]-=Ȱ;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) Ǡ[1,0]+=Ȱ;else Ǡ[1,1]-=Ȱ;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) Ǡ[2,0]+=Ȱ;else Ǡ[2,1]-=Ȱ;String ю=Δφ(г);float[,]ѝ=null;if(ќ.ContainsKey(ю)) ѝ=ќ[ю];else{ѝ=new float[3,2];ќ.Add(ю,ѝ);}float ț=г.MaxThrust;if(Math.Round(ζ.X,2)!=0.0)
if(ζ.X>=0) ѝ[0,0]+=ț;else ѝ[0,1]-=ț;if(Math.Round(ζ.Y,2)!=0.0)
if(ζ.Y>=0) ѝ[1,0]+=ț;else ѝ[1,1]-=ț;if(Math.Round(ζ.Z,2)!=0.0)
if(ζ.Z>=0) ѝ[2,0]+=ț;else ѝ[2,1]-=ț;}}static String Δφ(IMyThrust г){return г.BlockDefinition.SubtypeId;}Vector3 Βγ(Vector3 ѓ,float[,]ŏ){return new Vector3(
ѓ.X>=0?ŏ[0,0]:ŏ[0,1],
ѓ.Y>=0?ŏ[1,0]:ŏ[1,1],
ѓ.Z>=0?ŏ[2,0]:ŏ[2,1]);}bool Δυ(Ψ Ω,String ю,out float ΓΦ){ΓΦ=0;int Β=Ȉ.IndexOf(ю);if(Β==-1||Ω.Ǟ==null||Β>=Ω.Ǟ.Length)
return false;ΓΦ=Ω.Ǟ[Β];if(ΓΦ==-1) return false;return true;}Vector3 ΒΪ(Vector3 ѓ,Ψ Ω){if(Ω!=null){Vector3 έ=new Vector3();for(int i=0;i<ќ.Keys.Count;i++){String ю=ќ.Keys.ElementAt(i);float Ƈ=0;if(!Δυ(Ω,ю,out Ƈ))
return Βγ(ѓ,Ǡ);έ+=Βγ(ѓ,ќ.Values.ElementAt(i))*Ƈ;}return έ;}return Βγ(ѓ,Ǡ);}float ũ(Vector3 ѓ,Ψ Ω){return ũ(ѓ,new Vector3(),Ω);}float ũ(Vector3 ѓ,Vector3 ν,Ψ Ω){Vector3 ΘЌ=ΒΪ(ѓ,Ω);Vector3 Ȱ=ΘЌ+ν*Ĩ;float Ɩ=(Ȱ/ѓ).AbsMin();return(float)(ѓ*Ɩ).Length();}float Ƭ(Vector3 Αι,Vector3 ō,Ψ Ω)
{if(Αι.Length()==0) return 0;float ΕΘ=1;if(ō.Length()>0) ΕΘ=Math.Min(1,ı(-ō,Αι)/90)*0.4f+0.6f;float Ɵ=ũ(Αι,ō,Ω);if(Ɵ==0) return 0.1f;float ǌ=А(Ɵ,Ĩ);float ΔΘ=(float) Math.Sqrt(А(Αι.Length(),ǌ*0.5f));return ǌ*ΔΘ*ΕΘ*ǧ;}float ƶ(Vector3 ǭ,Vector3 ω,Vector3 ν,Ψ Ω){if(ν.Length()==0f) return 0;
Vector3 Ακ=Ĕ(ǭ,ω,Vector3.Normalize(ν));float Ǖ=ũ(-Ακ,Ω);return Ǖ/ν.Length();}void Δα(bool Θέ){float ў=0;float ΓΩ=0.9f;if(Θέ){О=-1;Ĭ=0;ΑЂ=null;if(ί!=Λ.Я&&Ε.ν.Length()!=0){ў=ΓΩ*ƶ(Ε.β,Ε.α*-1,Ε.ν,null);if(ў<О||О==-1) О=ў;}if(Θ.ε&&Θ.ν.Length()!=0){ў=ΓΩ*ƶ(Θ.β,Θ.α*-1,Θ.ν,null);
if(ў<О||О==-1) О=ў;}return;}if(Ĭ==-1) return;if(Ĭ>=0){int ΖΤ=0;while(Ĭ<Σ.Count){if(ΖΤ>100) return;ΖΤ++;Ψ Ω=Σ[Ĭ];if(Ω.ν.Length()!=0f){ў=ΓΩ*Math.Min(
ƶ(Ω.β,Ω.α*-1,Ω.ν,Ω),
ƶ(Ω.β*-1,Ω.α*-1,Ω.ν,Ω));if(ў<О||О==-1) О=ў;}else ΑЂ=Ω;Ĭ++;}Ĭ=-1;}bool Γί=true;float ć=0;if(ΑЂ!=null){for(int i=0;i<ќ.Count;i++){String ю=ќ.Keys.ElementAt(i);float[,]ѝ=ќ.Values.ElementAt(i);float Ƈ=0;if(!Δυ(ΑЂ,ю,out Ƈ)){Γί=false;break;}for(int a=0;a<ѝ.GetLength(0);a++)
for(int b=0;b<ѝ.GetLength(1);b++){float ė=Math.Abs(ѝ[a,b]*Ƈ);if(ė==0) continue;Γί=true;if(ć==0||ė<ć) ć=ė;}}}if(!Γί){for(int a=0;a<Ǡ.GetLength(0);a++)
for(int b=0;b<Ǡ.GetLength(1);b++){float ė=Math.Abs(Ǡ[a,b]);if(ė==0) continue;if(ć==0||ė<ć) ć=ė;}}if(ć>0){float Θθ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ў=А(ć,Θθ);if(ў>0)
if(ў<О||О==-1) О=ў;}}void Δή(Vector3 Ǖ,bool Ї){if(!Ї){for(int i=0;i<ȉ.Count;i++)
ȉ[i].SetValueFloat("Override",0.0f);return;}Vector3 ΒЉ=ΒΪ(Ǖ,null);float ΙΤ=Math.Min(1,Math.Abs(А(Ǖ.X,ΒЉ.X)));float ΙΥ=Math.Min(1,Math.Abs(А(Ǖ.Y,ΒЉ.Y)));float ΙΦ=Math.Min(1,Math.Abs(А(Ǖ.Z,ΒЉ.Z)));for(int i=0;i<ȉ.Count;i++){IMyThrust г=ȉ[i];Vector3 ζ=Ηβ(Ю(Τ,г.WorldMatrix.Backward),1);
if(ζ.X!=0&&Math.Sign(ζ.X)==Math.Sign(Ǖ.X))
г.SetValueFloat("Override",г.MaxThrust*ΙΤ);else if(ζ.Y!=0&&Math.Sign(ζ.Y)==Math.Sign(Ǖ.Y))
г.SetValueFloat("Override",г.MaxThrust*ΙΥ);else if(ζ.Z!=0&&Math.Sign(ζ.Z)==Math.Sign(Ǖ.Z))
г.SetValueFloat("Override",г.MaxThrust*ΙΦ);else
г.SetValueFloat("Override",0.0f);}}void Ǆ(bool Ї,float Θυ,float ы,float њ,float џ){for(int i=0;i<Αά.Count;i++){IMyGyro І=Αά[i];І.GyroOverride=Ї;if(!Ї) І.GyroPower=100;else І.GyroPower=Θυ;if(!Ї) continue;Vector3 В=Τ.WorldMatrix.Forward;Vector3 Ы=Τ.WorldMatrix.Right;Vector3 ω=Τ.WorldMatrix.Up;
Vector3 Ņ=І.WorldMatrix.Forward;Vector3 Ň=І.WorldMatrix.Up;Vector3 ņ=І.WorldMatrix.Left*-1;if(Ņ==В) І.SetValueFloat("Roll",џ);else if(Ņ==(В*-1)) І.SetValueFloat("Roll",џ*-1);else if(Ň==(В*-1)) І.SetValueFloat("Yaw",џ);else if(Ň==В) І.SetValueFloat("Yaw",џ*-1);else if(ņ==В) І.SetValueFloat("Pitch",џ);
else if(ņ==(В*-1)) І.SetValueFloat("Pitch",џ*-1);if(ņ==(Ы*-1)) І.SetValueFloat("Pitch",ы);else if(ņ==Ы) І.SetValueFloat("Pitch",ы*-1);else if(Ň==Ы) І.SetValueFloat("Yaw",ы);else if(Ň==(Ы*-1)) І.SetValueFloat("Yaw",ы*-1);else if(Ņ==(Ы*-1)) І.SetValueFloat("Roll",ы);
else if(Ņ==Ы) І.SetValueFloat("Roll",ы*-1);if(Ň==(ω*-1)) І.SetValueFloat("Yaw",њ);else if(Ň==ω) І.SetValueFloat("Yaw",њ*-1);else if(ņ==ω) І.SetValueFloat("Pitch",њ);else if(ņ==(ω*-1)) І.SetValueFloat("Pitch",њ*-1);else if(Ņ==ω) І.SetValueFloat("Roll",њ);else if(Ņ==(ω*-1)) І.SetValueFloat("Roll",њ*-1);
}}void ΔБ(){this.Γή=false;}void Х(Vector3 ȯ,Vector3 В,Vector3 Γμ,float Ƞ,bool Ƥ){ΑА(ȯ,Ƞ,Ƥ);ΑΑ=Ƞ;Γο=false;this.ΔΔ=В;this.Ζξ=Γμ;}void Х(Vector3 ȯ,Vector3 В,Vector3 Γμ,bool Ƥ){Х(ȯ,В,Γμ,2f,Ƥ);}void ΑА(Vector3 ȯ,float Ƞ,bool Ƥ){ΑΑ=Ƞ;this.Γή=true;this.ΕΙ=Ƥ;Γο=true;Αα=false;
this.ΔΓ=ȯ;}void Βώ(){ǥ(false,false,false,υ,0);ΑІ=false;}void Р(Vector3 Њ,float Ǜ){ǥ(true,false,false,Њ,Ǜ);}void ǥ(bool Ї,bool ΓЏ,bool ΔΠ,Vector3 Њ,float Ǜ){ǥ(Ї,ΓЏ,ΔΠ,Њ,Њ-ή,0.0f,Ǜ);}void ǥ(bool Ї,bool ΓЏ,bool ΔΠ,Vector3 Њ,Vector3 ΘЁ,float ΘЂ,float Ǜ){ΑІ=true;this.Ζπ=Ї;
υ=Њ;this.Γυ=Ǜ;this.ΓЀ=ΘЂ;this.Ζή=ΓЏ;this.Ζέ=ΔΠ;this.Ερ=ΘЁ;К=Vector3.Distance(Њ,ή);}void Ηδ(){ϊ.GetBlocksOfType(ŷ,ǎ);}void Ηε(){if(Αω){ύ=2;return;}List<IMyRemoteControl>Ɨ=new List<IMyRemoteControl>();List<IMySensorBlock>ΖΫ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ζε=new List<IMyTerminalBlock>();
ϊ.GetBlocksOfType(Ɨ,ǎ);ϊ.GetBlocksOfType(і,ǎ);ϊ.GetBlocksOfType(ΖΫ,ǎ);ϊ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ζε,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);і=Ǣ(і,pamTag,true);Ημ();Ην(Ζε);Ų(і,setLCDFontAndSize,1.4f,false);Ų(Ċ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Ζι=Ǣ(ΖΫ,pamTag,true);if(Ζι.Count>0) ξ=Ζι[0];else ξ=null;if(Η==Ζ.Ű){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){ύ=1;Κ="Drills are missing";}}else if(Η==Ζ.Ǎ){ϊ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){ύ=1;Κ="Grinders are missing";}if(ξ==null){ύ=1;Κ="Sensor is missing";}}else if(Η==Ζ.Ϋ){ϊ.GetBlocksOfType(ΔΙ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Ζθ=Ǣ(Ɨ,pamTag,true);if(Ζθ.Count>0) Ɨ=Ζθ;if(Ɨ.Count>0) Τ=Ɨ[0];else{Τ=null;ύ=2;Κ="Remote is missing";return;}δ=(float) Τ.CubeGrid.WorldVolume.Radius*2;љ=null;if(Η!=Ζ.Ϋ){Δβ();if(ο.Count>0&&љ!=null){if(ξ!=null&&(љ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Τ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Τ.WorldMatrix.Down==ξ.WorldMatrix.Down))){ύ=1;Κ="Wrong sensor direction";}if(љ.WorldMatrix.Forward!=Τ.WorldMatrix.Forward&&
љ.WorldMatrix.Forward!=Τ.WorldMatrix.Down){ύ=2;Κ="Wrong remote direction";}}}}void Ηζ(){ϊ.GetBlocksOfType(Ȅ,ǎ);ϊ.GetBlocksOfType(ф,ǎ);ϊ.GetBlocksOfType(ȉ,ǎ);ϊ.GetBlocksOfType(Αά,ǎ);ϊ.GetBlocksOfType(Ȭ,ǎ);ϊ.GetBlocksOfType(Ĉ,ǎ);ϊ.GetBlocksOfType(ѕ,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ϊ.GetBlocksOfType(ΑЁ,ǎ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
ф=Ǣ(ф,"ConnectorMedium",false);else
ф=Ǣ(ф,"Connector",false);List<IMyShipConnector>Ζζ=Ǣ(ф,pamTag,true);if(Ζζ.Count>0) ф=Ζζ;if(ύ<=1){if(ф.Count==0){ύ=1;Κ="Connector is missing";}if(Αά.Count==0){ύ=1;Κ="Gyros are missing";}if(ȉ.Count==0){ύ=1;Κ="Thrusters are missing";}}List<IMyConveyorSorter>Ζκ=Ǣ(ΑЁ,pamTag,true);
if(Ζκ.Count>0) ΑЁ=Ζκ;List<IMyLandingGear>Ζη=Ǣ(Ȅ,pamTag,true);if(Ζη.Count>0) Ȅ=Ζη;for(int i=0;i<Ȅ.Count;i++) Ȅ[i].AutoLock=false;List<IMyBatteryBlock>Ζδ=Ǣ(Ȭ,pamTag,true);if(Ζδ.Count>0) Ȭ=Ζδ;List<IMyGasTank>Ζλ=Ǣ(ѕ,pamTag,true);if(Ζλ.Count>0) ѕ=Ζλ;}void Ηη(){ϊ.GetBlocksOfType(Ę,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ł.Clear();for(int i=Ę.Count-1;i>=0;i--){if(ΗΡ(Ę[i])){ł.Add(Ę[i]);Ę.RemoveAt(i);}}}bool ΗΡ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&Βή(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Δβ(){if(Τ==null) return;љ=null;float Ας=0,Αν=0,Ασ=0,Αξ=0,Ατ=0,Αο=0;List<IMyTerminalBlock>ΔΒ=Ǣ(ο,pamTag,true);bool Θπ=ΔΒ.Count==0;if(ΔΒ.Count>0) љ=ΔΒ[0];else if(ο.Count>0) љ=ο[0];int ȁ=0;float ŗ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward){if(Θπ){ύ=2;Κ="Mining direction is unclear!";return;}continue;}ȁ++;ŗ=Math.Max(ŗ,Ηχ(ο[i]));Vector3 Ј=Ƶ(Τ,ο[i].GetPosition());if(i==0){Ας=Ј.X;Αν=Ј.X;Ασ=Ј.Y;Αξ=Ј.Y;Ατ=Ј.Z;Αο=Ј.Z;}Αν=Math.Max(Ј.X,Αν);Ας=Math.Min(Ј.X,Ας);
Αξ=Math.Max(Ј.Y,Αξ);Ασ=Math.Min(Ј.Y,Ασ);Αο=Math.Max(Ј.Z,Αο);Ατ=Math.Min(Ј.Z,Ατ);}if(ŗ==0) ŗ=drillRadius;bool ȯ=љ!=null&&љ.WorldMatrix.Forward==Τ.WorldMatrix.Down;ǟ=(Αν-Ας)*(1-ĭ)+ŗ*2;ǲ=(ȯ?Αο-Ατ:Αξ-Ασ)*(1-ğ)+ŗ*2;IMyCubeGrid g=Τ.CubeGrid;float ΔΧ=0,ΔΨ=0,ΔΩ=0,ΔΪ=0;
for(int c=0;c<8;c++){Vector3 p=Ƶ(Τ,g.GridIntegerToWorld(new Vector3I((c&1)==0?g.Min.X:g.Max.X,
(c&2)==0?g.Min.Y:g.Max.Y,(c&4)==0?g.Min.Z:g.Max.Z)));float y=ȯ?p.Z:p.Y;if(c==0||p.X<ΔΧ) ΔΧ=p.X;if(c==0||p.X>ΔΨ) ΔΨ=p.X;if(c==0||y<ΔΩ) ΔΩ=y;if(c==0||y>ΔΪ) ΔΪ=y;}Αό=ΔΨ-ΔΧ+g.GridSize;ΓЊ=ΔΪ-ΔΩ+g.GridSize;}float Ηχ(IMyTerminalBlock b){
if(!(b is IMyShipDrill)) return drillRadius;String s=b.BlockDefinition.SubtypeId;if(s=="SmallBlockDrill") return 1.3f;if(s=="LargeBlockDrill") return 1.9f;if(s=="LargeBlockPrototechDrill") return 6f;return drillRadius;}List<T>Ǣ<T>(List<T>Т,String ǜ,bool Αέ){List<T>έ=new List<T>();for(int i=0;i<Т.Count;i++)
if(Βή(Т[i],ǜ,Αέ)) έ.Add(Т[i]);return έ;}bool Βή<T>(T Υ,String ǜ,bool Αέ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(Αέ&&b.CustomName.ToUpper().Contains(ǜ.ToUpper())) return true;if(!Αέ&&b.BlockDefinition.ToString().ToUpper().Contains(ǜ.ToUpper())) return true;return false;}void ё<T>(List<T>Т,bool Ї)
{for(int i=0;i<Т.Count;i++)
ΔЌ((IMyTerminalBlock) Т[i],Ї);}void ΔЌ(IMyTerminalBlock Υ,bool Ї){if(Υ==null) return;String ΗЈ=Ї?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(ΗЈ);a.Apply(Υ);}void Ǧ(bool Ї){ΓА=Ї;if(!ĩ) return;ё(ΑЁ,Ї);}void Βυ(ChargeMode ƒ){for(int i=0;i<Ȭ.Count;i++) Ȭ[i].ChargeMode=ƒ;}void Βχ(bool ΘЈ)
{for(int i=0;i<ѕ.Count;i++) ѕ[i].Stockpile=ΘЈ;}void ų(List<IMyLandingGear>ΓΪ,bool ΕБ){for(int i=0;i<ΓΪ.Count;i++){if(ΕБ) ΓΪ[i].Lock();if(!ΕБ) ΓΪ[i].Unlock();}}void ΔЍ(bool Ї){if(Εα==Ї) return;List<IMyShipController>ΑΗ=new List<IMyShipController>();ϊ.GetBlocksOfType(ΑΗ,ǎ);if(ΑΗ.Count==0) return;
for(int i=0;i<ΑΗ.Count;i++)
ΑΗ[i].DampenersOverride=Ї;Εα=Ї;}IMyShipConnector П(MyShipConnectorStatus ь){for(int i=0;i<ф.Count;i++){if(!Ƿ(ф[i],true)) continue;if(ф[i].Status==ь) return ф[i];}return null;}void ĕ(bool ΓЌ,bool ΑΠ,float ΘΣ,float ΗЍ){if(ξ==null||ο.Count==0) return;Vector3 ΑΖ=new Vector3();
int ȁ=0;for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=љ.WorldMatrix.Forward) continue;ȁ++;ΑΖ+=ο[i].GetPosition();}ΑΖ=ΑΖ/ȁ;Vector3 œ=Ƶ(ξ,ΑΖ);ξ.Enabled=true;ξ.ShowOnHUD=ΓЌ;ξ.LeftExtend=(ΑΠ?1:й)/2f*ǟ-œ.X;ξ.RightExtend=(ΑΠ?1:й)/2f*ǟ+œ.X;ξ.TopExtend=(ΑΠ?1:и)/2f*ǲ+œ.Y;
ξ.BottomExtend=(ΑΠ?1:и)/2f*ǲ-œ.Y;ξ.FrontExtend=(ΓЌ?Ц:ΘΣ)-œ.Z;ξ.BackExtend=ΓЌ?0:ΗЍ+δ*0.75f+œ.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ų<T>(List<T>Т,bool ΘЃ,float ΘΠ,bool Θή){for(int i=0;i<Т.Count;i++){IMyTextSurface Ɲ=null;if(Т[i]is IMyTextSurface) Ɲ=(IMyTextSurface) Т[i];if(Ɲ!=null){Ɲ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!ΘЃ) continue;Ɲ.Font="Debug";if(Θή) continue;Ɲ.FontSize=ΘΠ;}}}bool ƹ(){Vector3D λ;return this.Τ.TryGetPlanetPosition(out λ);}int ΗΙ(Vector3 ΘЊ,int Ζί){if(Ζί==-1) return 0;double Б=-1;int Ȕ=-1;for(int i=Σ.Count-1;i>=0;i--){double Ϗ=Vector3.Distance(Σ[i].Δ,ΘЊ);if(Б==-1||Ϗ<Б){Ȕ=i;Б=Ϗ;}
}return Math.Sign(Ȕ-Ζί);}bool ΗΣ(Vector3 λ){List<Vector3>Ж=new List<Vector3>();for(int i=0;i<Σ.Count;i++) Ж.Add(Σ[i].Δ);if(Θ.ε&&Σ.Count>=1){Vector3 Ľ=new Vector3();Ƕ(Θ,dockDist*δ,false,out Ľ);if(Vector3.Distance(Θ.Δ,Σ.First().Δ)<Vector3.Distance(Θ.Δ,Σ.Last().Δ)){Ж.Insert(0,Ľ);Ж.Insert(0,Θ.Δ);}
else{Ж.Add(Ľ);Ж.Add(Θ.Δ);}}if(Η==Ζ.Ϋ){if(Ε.ε&&Σ.Count>=1){Vector3 ΒЅ=new Vector3();Ƕ(Ε,dockDist*δ,false,out ΒЅ);if(Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ)){Ж.Insert(0,ΒЅ);Ж.Insert(0,Ε.Δ);}else{Ж.Add(ΒЅ);Ж.Add(Ε.Δ);}}}else{if(ί!=Λ.Я)
if(Σ.Count>0&&Vector3.Distance(Ε.Δ,Σ.First().Δ)<Vector3.Distance(Ε.Δ,Σ.Last().Δ))
Ж.Insert(0,Ε.Δ);else Ж.Add(Ε.Δ);}int Ȕ=-1;double Ǫ=-1;for(int i=0;i<Ж.Count;i++){double Ϗ=Vector3.Distance(Ж[i],λ);if(Ϗ<Ǫ||Ǫ==-1){Ǫ=Ϗ;Ȕ=i;}}if(Ж.Count==0) return false;double ΓΟ=Vector3.Distance(Ж[Ȕ],λ);double Θφ=Vector3.Distance(Ж[Math.Max(0,Ȕ-1)],Ж[Ȕ])*1.5f;
double Θο=Vector3.Distance(Ж[Math.Min(Ж.Count-1,Ȕ+1)],Ж[Ȕ])*1.5f;return ΓΟ<Θφ||ΓΟ<Θο||ΓΟ<laneOffset*δ*1.5f;}static float А(float a,float b){if(b==0) return 0;return a/b;}Vector3 Ηβ(Vector3 v,int ΓΜ){return new Vector3(Math.Round(v.X,ΓΜ),Math.Round(v.Y,ΓΜ),Math.Round(v.Z,ΓΜ));
}float ı(Vector3 a,Vector3 b){if(a==b) return 0;float ΘΞ=(a*b).Sum;float ΕЌ=a.Length();float ΕЍ=b.Length();if(ΕЌ==0||ΕЍ==0) return 0;float έ=(float)((180.0f/Math.PI)*Math.Acos(ΘΞ/(ΕЌ*ΕЍ)));if(float.IsNaN(έ)) return 0;return έ;}float ď(Vector3 v,Vector3 Αχ,float ΘЄ){float Ļ=ı(v,Αχ);
if(ΘЄ>0f) Ļ*=-1;if(Ļ>-90f) return Ļ-90f;else return 180f-(-Ļ-90f);}double Δι(float ΘΖ){return(Math.PI/180)*ΘΖ;}bool Βα(double Θη,double value,double Αμ){return(value>=Θη&&value<=Αμ);}Vector3 Ηψ(IMyTerminalBlock Υ,Vector3 Ј){return Vector3D.Transform(Ј,Υ.WorldMatrix);
}Vector3 Ƶ(IMyTerminalBlock Υ,Vector3 ΙΠ){return Ю(Υ,ΙΠ-Υ.GetPosition());}Vector3 Ю(IMyTerminalBlock Υ,Vector3 ΔΦ){return Vector3D.TransformNormal(ΔΦ,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 Ĕ(Vector3 ǭ,Vector3 ω,Vector3 ΔΦ){MatrixD Γτ=MatrixD.CreateFromDir(ǭ,ω);return Vector3D.TransformNormal(ΔΦ,MatrixD.Transpose(Γτ));
}Vector3 ΒϏ(Vector3 ǭ,Vector3 ω,Vector3 Ј){MatrixD Γτ=MatrixD.CreateFromDir(ǭ,ω);return Vector3D.Transform(Ј,Γτ);}String κ(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 μ(String s){String[]Е=s.Split('|');return new Vector3(float.Parse(Μ(Е,0)),float.Parse(Μ(Е,1)),float.Parse(Μ(Е,2)));
}String ΗІ(Ψ Ω){String Ћ=":";String έ=κ(Ω.Δ)+Ћ+κ(Ω.β)+Ћ+
κ(Ω.α)+Ћ+κ(Ω.τ)+Ћ+κ(Ω.ν);for(int i=0;i<Ω.Ǟ.Length;i++)
έ+=Ћ+Math.Round(Ω.Ǟ[i],3);return έ;}Ψ Ης(String s){String[]Е=s.Split(':');Ψ Ω=new Ψ(
μ(Μ(Е,0)),μ(Μ(Е,1)),
μ(Μ(Е,2)),μ(Μ(Е,3)),
μ(Μ(Е,4)));int Β=5;List<float>Εν=new List<float>();while(Β<Е.Length){String ΑЌ=Μ(Е,Β);float f=0;if(!float.TryParse(ΑЌ,out f)) break;Εν.Add(f);Β++;}Ω.Ǟ=Εν.ToArray();return Ω;}System.Text.StringBuilder ś=new System.Text.StringBuilder();void Ρ<T>(T value,bool Θξ){if(Θξ) ś.Append('\n');ś.Append(value);}
void Ρ<T>(T value){Ρ(value,true);}String Ʊ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String ǉ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Μ(String[]ΗЌ,int Β){String ΑЌ=ΗЌ.ElementAtOrDefault(Β);if(String.IsNullOrEmpty(ΑЌ)) return "";
return ΑЌ;}void Save(){if(Αω||Η==Ζ.т){Storage="";return;}Storage=ΒΕ();}String ΒΕ(){ś.Clear();ś.Append(DATAREV+";");Ρ(κ(Αψ),false);Ρ(κ(Θ.β));Ρ(κ(Θ.τ));Ρ(κ(Θ.α));Ρ(κ(Θ.ν));Ρ(κ(Θ.Δ));Ρ(κ(Θ.Ч));Ρ(Θ.ε);Ρ(κ(Ε.Δ));Ρ(κ(Ε.ν));Ρ(κ(Ε.β));Ρ(κ(Ε.α));Ρ(κ(Ε.τ));Ρ(κ(Ε.Ч));Ρ(Ε.ε);Ρ(κ(ъ));Ρ(κ(ȅ));Ρ(";");
Ρ((int) Η,false);Ρ((int) ί);Ρ((int) ч);Ρ(Ǯ);Ρ(ш);Ρ(Ъ);Ρ(щ);Ρ(ǧ);Ρ(Ȥ);Ρ(Ȣ);if(Η==Ζ.Ϋ){Ρ((int) ϋ.Ќ);Ρ(ϋ.Ǔ);Ρ(ϋ.ȱ);Ρ(Ʊ(ϋ.ǔ));Ρ(Ʊ(ϋ.Ǘ));Ρ((int) ό.Ќ);Ρ(ό.Ǔ);Ρ(ό.ȱ);Ρ(Ʊ(ό.ǔ));Ρ(Ʊ(ό.Ǘ));}else{Ρ((int) ȟ);Ρ((int) н);Ρ((int) М);Ρ((int) х);Ρ((int) Ģ);Ρ(ċ);Ρ(ĩ);Ρ(Ŋ);Ρ(Ń);Ρ(й);Ρ(и);Ρ(Ц);Ρ(ť);Ρ(Ť);Ρ(ĭ);Ρ(ğ);
Ρ(Н);Ρ(в);Ρ(Ш);Ρ(Щ);Ρ(Ț);Ρ(Й);Ρ(Ħ);Ρ(Ĥ);}Ρ(";");for(int i=0;i<Ȉ.Count;i++)
Ρ((i>0?"|":"")+Ȉ[i],false);Ρ(";");for(int i=0;i<Σ.Count;i++)
Ρ(ΗІ(Σ[i]),i>0);Ρ(";",false);Ρ(Л+"|"+Ѐ,false);Ρ(";",false);Ρ(ΖБ(),false);String Χ=ś.ToString();ś.Clear();return Χ;}у Ηα(){return ǁ(Storage);}у ǁ(String Χ){if(Χ=="") return у.ΗΪ;String[]ħ=Χ.Split(';');if(Μ(ħ,0)!=DATAREV) return у.Βζ;int Β=0;try{String[]Ι=Μ(ħ,1).Split('\n');
Αψ=μ(Μ(Ι,Β++));Θ.β=μ(Μ(Ι,Β++));Θ.τ=μ(Μ(Ι,Β++));Θ.α=μ(Μ(Ι,Β++));Θ.ν=μ(Μ(Ι,Β++));Θ.Δ=μ(Μ(Ι,Β++));Θ.Ч=μ(Μ(Ι,Β++));Θ.ε=bool.Parse(Μ(Ι,Β++));Ε.Δ=μ(Μ(Ι,Β++));Ε.ν=μ(Μ(Ι,Β++));Ε.β=μ(Μ(Ι,Β++));Ε.α=μ(Μ(Ι,Β++));Ε.τ=μ(Μ(Ι,Β++));Ε.Ч=μ(Μ(Ι,Β++));Ε.ε=bool.Parse(Μ(Ι,Β++));
ъ=μ(Μ(Ι,Β++));ȅ=μ(Μ(Ι,Β++));Ι=Μ(ħ,2).Split('\n');Β=0;Η=(Ζ) int.Parse(Μ(Ι,Β++));ί=(Λ) int.Parse(Μ(Ι,Β++));ч=(Λ) int.Parse(Μ(Ι,Β++));Ǯ=int.Parse(Μ(Ι,Β++));ш=int.Parse(Μ(Ι,Β++));Ъ=int.Parse(Μ(Ι,Β++));щ=int.Parse(Μ(Ι,Β++));ǧ=float.Parse(Μ(Ι,Β++));Ȥ=bool.Parse(Μ(Ι,Β++));
Ȣ=bool.Parse(Μ(Ι,Β++));if(Η==Ζ.Ϋ){ϋ.Ќ=(ι) int.Parse(Μ(Ι,Β++));ϋ.Ǔ=float.Parse(Μ(Ι,Β++));ϋ.ȱ=float.Parse(Μ(Ι,Β++));ϋ.ǔ=ǉ(Μ(Ι,Β++));ϋ.Ǘ=ǉ(Μ(Ι,Β++));ό.Ќ=(ι) int.Parse(Μ(Ι,Β++));ό.Ǔ=float.Parse(Μ(Ι,Β++));ό.ȱ=float.Parse(Μ(Ι,Β++));ό.ǔ=ǉ(Μ(Ι,Β++));ό.Ǘ=ǉ(Μ(Ι,Β++));}else{ȟ=(Ǐ) int.Parse(Μ(Ι,Β++));
н=(е) int.Parse(Μ(Ι,Β++));М=(ώ) int.Parse(Μ(Ι,Β++));х=(я) int.Parse(Μ(Ι,Β++));Ģ=(Ǐ) int.Parse(Μ(Ι,Β++));ċ=bool.Parse(Μ(Ι,Β++));ĩ=bool.Parse(Μ(Ι,Β++));Ŋ=bool.Parse(Μ(Ι,Β++));Ń=bool.Parse(Μ(Ι,Β++));й=int.Parse(Μ(Ι,Β++));и=int.Parse(Μ(Ι,Β++));Ц=int.Parse(Μ(Ι,Β++));ť=float.Parse(Μ(Ι,Β++));
Ť=float.Parse(Μ(Ι,Β++));ĭ=float.Parse(Μ(Ι,Β++));ğ=float.Parse(Μ(Ι,Β++));Н=int.Parse(Μ(Ι,Β++));в=int.Parse(Μ(Ι,Β++));Ш=int.Parse(Μ(Ι,Β++));Щ=int.Parse(Μ(Ι,Β++));Ț=int.Parse(Μ(Ι,Β++));Й=int.Parse(Μ(Ι,Β++));Ħ=int.Parse(Μ(Ι,Β++));Ĥ=float.Parse(Μ(Ι,Β++));}Ι=Μ(ħ,3).Replace("\n","").Split('|');
Ȉ=Ι.ToList();Ι=Μ(ħ,4).Split('\n');Σ.Clear();if(Ι.Count()>=1&&Ι[0]!="")
for(int i=0;i<Ι.Length;i++)
Σ.Add(Ης(Μ(Ι,i)));Ι=Μ(ħ,5).Split('|');Л=Μ(Ι,0);if(!int.TryParse(Μ(Ι,1),out Ѐ)) Ѐ=-1;if(Л.Length!=Н*в){Л="";Ѐ=-1;}ΖА(Μ(ħ,6));}catch{return у.Δμ;}Ŝ=ч;φ();return у.ǆ;}const int Δΰ=64000;IMyTerminalBlock Δν(){List<IMyTerminalBlock>ŀ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ŀ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));
return ŀ.Count>0?ŀ[0]:null;}void ΗΓ(){IMyTerminalBlock Υ=Δν();if(Υ==null){Κ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Σ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+ΒΕ();if(Χ.Length>Δΰ){Κ="Export too large: "+Χ.Length+" chars, max "+Δΰ;return;}Υ.CustomData=Χ;Κ="Exported "+Σ.Count+" waypoints";}void ΗΝ(bool Γθ){IMyTerminalBlock Υ=Δν();if(Υ==null){Κ="Import: name a block "+dataTag;return;}
ΗΞ(Υ.CustomData,Γθ);}void ΗΞ(String Χ,bool Γθ){Χ=Χ.Replace("\r","");if(Χ.StartsWith("//")){int ΖΚ=Χ.IndexOf('\n');Χ=ΖΚ<0?"":Χ.Substring(ΖΚ+1);}Χ=Χ.Trim();if(Χ==""){Κ="Import: no PAM data in "+dataTag;return;}φ();String ΕΡ=ΒΕ();у έ=ǁ(Χ);if(έ==у.ǆ&&Γθ){Ψ Ć=new Ψ(Ε);
Vector3 Θκ=ъ,Θι=ȅ;int w=й,h=и,d=Ц;Ǐ ΓБ=ȟ;я ΓΝ=х;float Θσ=ĭ,Θς=ğ;ǁ(ΕΡ);Ε=Ć;ъ=Θκ;ȅ=Θι;й=w;и=h;Ц=d;ȟ=ΓБ;х=ΓΝ;ĭ=Θσ;ğ=Θς;ί=Λ.ķ;ŵ(true,true);ί=Λ.ķ;ч=Λ.ķ;Κ="Imported job, run CONT";return;}if(έ==у.ǆ){Ɠ=false;Κ="Imported "+Σ.Count+" waypoints";
return;}ǁ(ΕΡ);Κ=έ==у.Βζ?"Import: data is from another PAM version":"Import failed, nothing changed";}void Ημ(){Ɓ.Clear();for(int i=і.Count-1;i>=0;i--){String Χ=і[i].CustomData.ToUpper();bool Γζ=false;if(Χ=="INSTRUCTIONS"){Γζ=true;ΑΞ=true;}if(Χ=="DEBUG") Γζ=true;if(Γζ){Ɓ.Add(і[i]);і.RemoveAt(i);}
}Ų(Ɓ,false,1,false);}void Ην(List<IMyTerminalBlock>ŀ){Ċ.Clear();for(int i=0;i<ŀ.Count;i++){IMyTerminalBlock Υ=ŀ[i];try{String ΖΣ=pamTag.Substring(0,pamTag.Length-1)+":";int Β=Υ.CustomName.IndexOf(ΖΣ);int Ş=-1;if(Β<0||!int.TryParse(Υ.CustomName.Substring(Β+ΖΣ.Length,1),out Ş)) continue;
if(Ş==-1) continue;Ş--;IMyTextSurfaceProvider ΖΥ=(IMyTextSurfaceProvider) Υ;if(Ş<ΖΥ.SurfaceCount&&Ş>=0)
Ċ.Add(ΖΥ.GetSurface(Ş));}catch{}}}String Απ="";void ΔЈ(){String Γσ="";String ΓΥ="";Απ=Δί(false);Γσ+=Απ;ΓΥ+=Απ;ΓΥ+=ΗΗ();for(int i=0;i<і.Count;i++) і[i].WriteText(Γσ);for(int i=0;i<Ċ.Count;i++) Ċ[i].WriteText(Γσ);Echo(ΓΥ);for(int i=0;i<Ɓ.Count;i++){IMyTextPanel ΓϏ=Ɓ[i];
String Χ=ΓϏ.CustomData.ToUpper();if(Χ=="DEBUG") ΓϏ.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") ΓϏ.WriteText(ΗΘ());}}string ΗΘ(){String Γ="";try{float ΖΒ=Runtime.MaxInstructionCount;Γ+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";
Γ+="Inst. avg/max: "+(int)(Ŀ*ΖΒ)+" / "+(int)(Ő*ΖΒ)+"\n";Γ+="Inst. avg/max: "+Math.Round(Ŀ*100f,1)+"% / "+Math.Round(Ő*100f,1)+"% \n";Γ+="Time avg/max: "+Math.Round(ź,2)+"ms / "+Math.Round(Γχ,2)+"ms \n";}catch{}for(int i=0;i<Ȝ.Count;i++)
Γ+=""+Ȝ.Keys.ElementAt(i)+" = "+Math.Round(Ȝ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȝ.Values.ElementAt(i)[1],2)+"%\n";return Γ;}void ΗΛ(){String Γ="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ų(і,setLCDFontAndSize,0.9f,false);Ų(Ċ,setLCDFontAndSize,0.9f,true);for(int i=0;i<і.Count;i++) і[i].WriteText(Γ+ș.ToString());for(int i=0;i<Ċ.Count;i++) Ċ[i].WriteText(Γ+ș.ToString());}void ǻ(){if(!ΑΞ) return;try{ΓЃ=Runtime.CurrentInstructionCount;}catch{}
}void ǵ(String ψ){if(!ΑΞ) return;if(ΓЃ==0) return;try{float ΑЊ=(Runtime.CurrentInstructionCount-ΓЃ)/Runtime.MaxInstructionCount*100;if(!Ȝ.ContainsKey(ψ))
Ȝ.Add(ψ,new float[]{ΑЊ,ΑЊ});else{Ȝ[ψ][0]=ΑЊ;Ȝ[ψ][1]=Math.Max(ΑЊ,Ȝ[ψ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ζώ(String ȳ){if(!Ł) return;String Ћ=""+BROADCAST_SEP;String ρ=VERSION+Ћ;ρ+=Ŧ(ΓЋ)+Ћ;
ρ+=(int) Η+Ћ;ρ+=ΒΥ(ή.X)+""+Ћ;ρ+=ΒΥ(ή.Y)+Ћ;ρ+=ΒΥ(ή.Z)+Ћ;if(Η!=Ζ.Ϋ)
ρ+=ΒΧ(ч)+(ч==Λ.Є?" "+Math.Round(ƌ,1)+"%":"")+Ћ;else ρ+=ΒΧ(ч)+Ћ;ρ+=Ŧ(Κ)+Ћ;ρ+=Ǚ+""+Ћ;ρ+=Ŧ(Απ)+Ћ;ρ+=ƀ+Ћ;ρ+=Ɛ+Ћ;for(int i=0;i<ǖ.Count;i++){if(ǖ[i].Ŏ!=З.ū) continue;ρ+=Ŧ(ǖ[i].ψ)+"/"+Ŧ(ǖ[i].ς)+"/"+ǖ[i].Ǒ+Ћ;}Ηι(ρ,ȳ);}String Ŧ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void Ηι(String ρ,String ȳ){Ηκ(ρ,BROADCAST_SELF_ID,ȳ);}void Ηκ(String ρ,String Њ,String ȳ){try{if(!Ł) return;ρ=BROADCAST_TAG+BROADCAST_SEP+Δπ()+BROADCAST_SEP+Њ+BROADCAST_SEP+ΒА+BROADCAST_SEP+ȳ+BROADCAST_SEP+ρ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,ρ);}catch(Exception e){ș=e;}
}String Δπ(){if(Η!=Ζ.т) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΔЀ(String ρ){return ρ.StartsWith(BROADCAST_TAG);}bool Ũ(ref String Χ,out string Εξ,Char Ћ){int Β=Χ.IndexOf(Ћ);Εξ="";if(Β<0) return false;Εξ=Χ.Substring(0,Β);Χ=Χ.Remove(0,Β+1);return true;}bool ΗΫ(ref String ρ,out String ƙ,out String ȳ)
{ƙ="";ȳ="";if(!Ł) return false;String ǜ="";if(!Ũ(ref ρ,out ǜ,BROADCAST_SEP)||!ΔЀ(ǜ)) return false;if(!Ũ(ref ρ,out ƙ,BROADCAST_SEP)) return false;if(!Ũ(ref ρ,out ǜ,BROADCAST_SEP)||(ǜ!=Δπ()&&(ǜ!="*"&&Η!=Ζ.т))) return false;if(!Ũ(ref ρ,out ǜ,BROADCAST_SEP)||(ǜ!=ΒА)) return false;
if(!Ũ(ref ρ,out ȳ,BROADCAST_SEP)) return false;return true;}void ΗΠ(){bool ž=true;if(Επ){Επ=false;if(Me.CustomData.Contains("Antenna_Name")){Κ="Update custom data";Me.CustomData="";}}String Ɗ=(Η!=Ζ.т?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ɗ)
Δό(Ɗ);String[]Ι=Me.CustomData.Split('\n');Ł=bool.Parse(Βπ(ΒΣ(Ι,"Enable_Broadcast",ref ž)));bool ΔΤ=false;bool Εε=true;if(Ł){if(Η!=Ζ.т)
ΓЋ=Βπ(ΒΣ(Ι,"Ship_Name",ref ž)).Replace(BROADCAST_SELF_ID,"");ΒА=Βπ(ΒΣ(Ι,"Broadcast_Channel",ref ž)).ToLower();Εε=false;if(ȑ==null){ȑ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ȑ.SetMessageCallback("");}List<IMyRadioAntenna>ļ=new List<IMyRadioAntenna>();ϊ.GetBlocksOfType(ļ);bool ΒЄ=false;
for(int i=0;i<ļ.Count;i++)
if(ļ[i].EnableBroadcasting&&ļ[i].Enabled){ΒЄ=true;break;}if(ļ.Count==0) Κ="No Antenna found";else if(!ΒЄ) Κ="Antenna not ready";ΔΤ=ļ.Count==0||!ΒЄ;if(ΕΚ&&!ΔΤ&&Η!=Ζ.т) Κ="Antenna ok";}else if(Η==Ζ.т)
Κ="Offline - Enable in PB custom data";ΕΚ=ΔΤ;if(Εε){if(ȑ!=null) this.IGC.DisableBroadcastListener(ȑ);ȑ=null;}}catch{ž=false;}if(!ž){Κ="Reset custom data";Δό(Ɗ);}}void Δό(String Ɗ){String Χ=Ɗ+"\n\n"+"Enable_Broadcast="+(Η==Ζ.т?"true":"false")+" \n";Χ+=Η!=Ζ.т?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Βπ(String ģ){int Β=ģ.IndexOf("//");if(Β!=-1) ģ=ģ.Substring(0,Β);String[]Е=ģ.Split('=');if(Е.Length<=1) return "";return Е[1].Trim();}String ΒΣ(String[]Ι,String Ō,ref bool Ɣ){foreach(String ģ in Ι)
if(ģ.StartsWith(Ō)) return ģ;Ɣ=false;return "";}String ΔЋ(int ΔΡ,String Γ,int ΖΪ,ref int ȝ){String[]Ι=Γ.Split('\n');if(ΖΪ>=ȝ+ΔΡ-1) ȝ++;ȝ=Math.Min(Ι.Count()-1-ΔΡ,ȝ);if(ΖΪ<ȝ+1) ȝ--;ȝ=Math.Max(0,ȝ);String έ="";for(int i=0;i<ΔΡ;i++){int ΕЎ=i+ȝ;if(ΕЎ>=Ι.Count()) break;
έ+=Ι[ΕЎ]+"\n";}return έ;}String ΒΦ(int Ƙ){if(Ƙ>=60*60) return Math.Round(Ƙ/(60f*60f),1)+" h";if(Ƙ>=60) return Math.Round(Ƙ/60f,1)+" min";return ""+Ƙ+" s";}string ΒΥ(float v){return Math.Round(v,2)+" ";}List<IMyCameraBlock>ǩ=new List<IMyCameraBlock>();List<long>Ŕ=new List<long>();
bool Ŗ=false;int ΓΖ=0,Θώ=0,ΒЎ=0;const int Ηγ=16;int ΓΔ=0;DateTime ΕΦ=DateTime.Now;String ΒЏ="";String ΓΓ="";void Ηθ(){ǩ.Clear();Ŕ.Clear();if(!obstacleBrake) return;ϊ.GetBlocksOfType(ǩ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid)&&q.IsFunctional);for(int i=0;i<ǩ.Count;i++) ǩ[i].EnableRaycast=true;
List<IMyTerminalBlock>ΑΒ=new List<IMyTerminalBlock>();ϊ.GetBlocksOfType(ΑΒ,q=>q.CubeGrid.IsSameConstructAs(Me.CubeGrid));for(int i=0;i<ΑΒ.Count;i++) if(!Ŕ.Contains(ΑΒ[i].CubeGrid.EntityId)) Ŕ.Add(ΑΒ[i].CubeGrid.EntityId);if(!Ŕ.Contains(Me.CubeGrid.EntityId)) Ŕ.Add(Me.CubeGrid.EntityId);
ΓΓ="";if(Τ==null) return;MatrixD m=Τ.WorldMatrix;Vector3[]ΘΚ={m.Forward,m.Backward,m.Left,m.Right,m.Up,m.Down};String[]Θμ={"Fwd","Back","Left","Right","Up","Down"};for(int d=0;d<6;d++)
for(int i=0;i<ǩ.Count;i++)
if(Vector3.Dot(ǩ[i].WorldMatrix.Forward,ΘΚ[d])>0.7f){ΓΓ+=Θμ[d]+" ";break;}}bool ΔϏ(Α s){return s==Α.ǣ||s==Α.Э||s==Α.Ѕ||
s==Α.ȩ||s==Α.ĳ||s==Α.Ƴ||
(s==Α.ǡ&&ј!=null);}void Ư(){Ŗ=false;ΓΖ=0;ΒЏ="";}void ΗЀ(){if(!obstacleBrake||ǩ.Count==0||!ΔϏ(Φ)){Ư();return;}Vector3 Ɖ=υ+ȡ;Vector3 ǰ=Ɖ-ή;float ǝ=ǰ.Length();if(ǝ<1f){Ư();return;}Vector3 ζ=ǰ/ǝ;Vector3 ȣ=Τ!=null?(Vector3) Τ.GetShipVelocities().LinearVelocity:new Vector3();
if(ȣ.Length()>2) ζ=Vector3.Normalize(ȣ);Vector3 ΑϏ=Vector3.Cross(ζ,Math.Abs(ζ.Y)<0.9f?new Vector3(0,1,0):new Vector3(1,0,0));ΑϏ.Normalize();Vector3 ω=Vector3.Cross(ΑϏ,ζ);float Ϗ=Math.Min(ǝ+δ*0.25f,Ȟ*obstacleLookAhead+δ);int ΕΨ=0;for(int k=0;k<2;k++){int p=k==0?0:Θώ++%8,e=p%4;
Vector3 Θρ=k==0?new Vector3():(e==0?ΑϏ:e==1?-ΑϏ:e==2?ω:-ω)*δ*0.5f;Vector3 ń=ή+ζ*(p>=4?Ϗ*0.5f:Ϗ)+Θρ;IMyCameraBlock ΓΒ=ΖЃ(ń);if(ΓΒ==null){Δζ();continue;}ΕΨ++;MyDetectedEntityInfo Ȗ=ΓΒ.Raycast(ń);bool ΙΚ=Ȗ.Type==MyDetectedEntityType.Planet||Ȗ.Type==MyDetectedEntityType.Asteroid;
bool ΖΛ=!Ȗ.IsEmpty()&&!Ŕ.Contains(Ȗ.EntityId)&&!ΗΦ(Ȗ.EntityId)&&
Ȗ.Type!=MyDetectedEntityType.FloatingObject&&!(ΙΚ&&(k>0||Φ==Α.ǣ||
(Ȗ.HitPosition.HasValue&&Vector3D.Distance(Ȗ.HitPosition.Value,Ɖ)<δ)));if(ΖΛ){if(!Ŗ) ΕΦ=DateTime.Now;Ŗ=true;ΓΖ=0;ΒЏ=Ȗ.Type.ToString();}else Δζ();if(debugDraw&&ΑΟ.Count<16) ΑΟ.Add(κ(ΓΒ.GetPosition())+","+κ(ń)+","+(ΖΛ?1:0));}ΒЎ=ΕΨ>0?0:ΒЎ+1;if(ΒЎ>12) Κ="Cameras can't see ahead";
if(Ŗ&&(DateTime.Now-ΕΦ).TotalSeconds>obstacleTimeout)
Κ="Path blocked: "+ΒЏ;}void Δζ(){if(Ŗ&&++ΓΖ>=Ηγ) Ư();}IMyCameraBlock ΖЃ(Vector3 ń){IMyCameraBlock Ǿ=null;float ΕΥ=-2;for(int n=0;n<ǩ.Count;n++){ΓΔ=(ΓΔ+1)%ǩ.Count;IMyCameraBlock c=ǩ[ΓΔ];if(!c.IsFunctional||!c.IsWorking) continue;float ΖΨ=Vector3.Dot(Vector3.Normalize(ń-(Vector3) c.GetPosition()),c.WorldMatrix.Forward);
if(ΖΨ>ΕΥ+0.05f&&c.CanScan(ń)){Ǿ=c;ΕΥ=ΖΨ;}}return Ǿ;}const String ΕΓ="[PAMTFC]";const float Ηυ=5f;const float ΒΡ=2f;class Ȫ{public long χ;public Vector3 λ,ȣ;public Vector3 Ƌ;public float Εδ;public String Ć="";public int Γγ=-1;public bool ΓЄ;public int Αδ;public float ΘΤ,ΘΟ;public long ά=0;
public int ƃ=0;public DateTime ΔΗ;}IMyBroadcastListener ΑЉ=null;long м=0;List<Ȫ>о=new List<Ȫ>();Vector3 ȡ=new Vector3();float э=-1;Vector3 ΔΛ=new Vector3();bool ΑЈ=false;DateTime ΔΜ=DateTime.Now;const float ΕΒ=15f;const float Ητ=10f;int Ζτ=0;String Л="";int Ѐ=-1;bool ΑΙ=false,ΑΘ=false;
Vector3 ΕΫ=new Vector3();String ĵ(){if(ί==Λ.Я||Η==Ζ.Ϋ) return "";return Math.Round(Ε.Δ.X)+"|"+Math.Round(Ε.Δ.Y)+"|"+Math.Round(Ε.Δ.Z)+
"|"+Н+"|"+в+"|"+(int) Ģ+"|"+Math.Round(ǟ,1)+"|"+Math.Round(ǲ,1);}void ΗЃ(){if(!trafficEnabled) return;if(ΑЉ==null) ΑЉ=IGC.RegisterBroadcastListener(ΕΓ);м=Me.CubeGrid.EntityId;int Ě=30;while(ΑЉ.HasPendingMessage&&Ě-->0){String Χ=ΑЉ.AcceptMessage().Data as String;if(Χ!=null) Ηή(Χ);
}for(int i=о.Count-1;i>=0;i--)
if((DateTime.Now-о[i].ΔΗ).TotalSeconds>Ηυ) о.RemoveAt(i);if(ΑЅ==null) ΑЅ=IGC.RegisterBroadcastListener(Ησ);Ě=5;while(ΑЅ.HasPendingMessage&&Ě-->0){String Χ=ΑЅ.AcceptMessage().Data as String;if(Χ!=null) try{Ηί(Χ);}catch{}}if(ΑΚ==null) ΑΚ=IGC.RegisterBroadcastListener(ΗΧ);
Ě=10;while(ΑΚ.HasPendingMessage&&Ě-->0){String Χ=ΑΚ.AcceptMessage().Data as String;if(Χ!=null) try{Ηά(Χ);}catch{}}if(++Ζτ<3) return;Ζτ=0;if(debugDraw){IGC.SendBroadcastMessage("[PAMDBG]",м+";"+κ(Ε.Δ)+";"+
κ(ί==Λ.Я?new Vector3():Ε.τ)+";"+κ(ȅ)+";"+
κ(υ+ȡ)+";"+String.Join(":",ΑΟ));ΑΟ.Clear();}IGC.SendBroadcastMessage(ΕΓ,м+";"+κ(ή)+";"+
κ(Τ.GetShipVelocities().LinearVelocity)+";"+δ+";"+ĵ()+";"+
(ί==Λ.Є?Ѐ:-1)+";"+Л+";"+ΖЉ()+","+Δλ()+";"+κ(ΔΛ)+
";"+(ΑΣ?1:0)+";"+(Ā&&ί==Λ.Є?1:0)+","+Αό+","+ΓЊ+","+ΗΨ());}void Ηή(String Χ){try{String[]f=Χ.Split(';');if(f.Length<7) return;long χ=long.Parse(f[0]);if(χ==м) return;Ȫ Љ=null;for(int i=0;i<о.Count;i++) if(о[i].χ==χ) Љ=о[i];if(Љ==null){Љ=new Ȫ();Љ.χ=χ;о.Add(Љ);}
Љ.λ=μ(f[1]);Љ.ȣ=μ(f[2]);Љ.Εδ=float.Parse(f[3]);Љ.Ć=f[4];Љ.Γγ=int.Parse(f[5]);Љ.ΔΗ=DateTime.Now;if(Љ.Ć!=""&&Љ.Ć==ĵ()) ΔЅ(f[6]);Љ.ά=0;Љ.ƃ=0;if(f.Length>=8){String[]d=f[7].Split(',');Љ.ά=long.Parse(d[0]);Љ.ƃ=int.Parse(d[1]);}Љ.Ƌ=f.Length>=9?μ(f[8]):new Vector3();
Љ.ΓЄ=f.Length>=10&&f[9]=="1";Љ.Αδ=0;if(f.Length>=11){String[]w=f[10].Split(',');if(w.Length>=4){Љ.ΘΤ=float.Parse(w[1]);Љ.ΘΟ=float.Parse(w[2]);Љ.Αδ=int.Parse(w[3]);}}}catch{}}bool ΗΦ(long χ){for(int i=0;i<о.Count;i++) if(о[i].χ==χ) return true;return false;}Vector3 Δτ(int Β,int Θτ)
{if(laneOffset<=0||Σ.Count<2||Β<0||Β>=Σ.Count) return new Vector3();Ψ Ω=Σ[Β];Vector3 ΙΗ=(Σ[Math.Min(Σ.Count-1,Β+1)].Δ-Σ[Math.Max(0,Β-1)].Δ)*Θτ;Vector3 ω=Ω.ν.LengthSquared()>0.01f?-Vector3.Normalize(Ω.ν):-Ω.α;Vector3 Ы=Vector3.Cross(ΙΗ,ω);if(Ы.LengthSquared()<0.0001f) return new Vector3();
float Ϗ=laneOffset*δ;float ΘΡ=Math.Min(Vector3.Distance(Ω.Δ,Σ[0].Δ),
Vector3.Distance(Ω.Δ,Σ[Σ.Count-1].Δ));return Vector3.Normalize(Ы)*Ϗ*Math.Min(1,ΘΡ/(Ϗ*4));}Vector3 Ζσ=new Vector3();List<String>ΑΟ=new List<String>();bool Δω(){Α s=Φ;return ĵ()!=""&&(s==Α.ǣ||s==Α.Ķ||s==Α.ǹ||
s==Α.Ǹ||s==Α.Ĳ);}int ΗΨ(){if(!Δω()) return 0;return Φ!=Α.ǣ&&Vector3.Dot(ή-л,ъ)>δ*0.5f?2:1;}bool ΗΥ(Α s){return s==Α.ǡ||s==Α.č||s==Α.đ||s==Α.ű||
s==Α.ǐ||s==Α.ǹ||s==Α.Ǹ||s==Α.Ĳ||s==Α.ȥ;}void ΗЄ(){ȡ=new Vector3();э=-1;ΔΛ=new Vector3();bool Εσ=ΔϏ(Φ);if(!trafficEnabled||(!Εσ&&!ΗΥ(Φ))){ΑЈ=false;return;}Ζσ=Τ.GetShipVelocities().LinearVelocity;ΖЊ(Τ.GetNaturalGravity(),!Εσ);Ζό();}IMyBroadcastListener ΑЅ=null;
float Š=-1;DateTime Ζο=DateTime.MinValue;void Ηί(String Χ){if(!Χ.StartsWith("C;")) return;Ζο=DateTime.Now;Š=-1;String Ƒ=м+":";String[]e=Χ.Substring(2).Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ƒ)) Š=float.Parse(e[i].Substring(Ƒ.Length));}void Ζό(){if(Š<0||(DateTime.Now-Ζο).TotalSeconds>1.5) return;
э=э<0?Š:Math.Min(э,Š);if(Š<0.5f) Κ="Traffic: giving way (controller)";}void ΖЊ(Vector3 ν,bool ΖΡ=false){ȡ=new Vector3();э=-1;Vector3 ǰ=υ-ή;float ǝ=ǰ.Length();bool Γα=ǝ>=1f;Vector3 ζ=Γα?ǰ/ǝ:new Vector3();ΔΛ=ζ;Vector3 ω=ν.LengthSquared()>0.01f?-Vector3.Normalize(ν):-ƅ;
Vector3 Ы=Vector3.Cross(ζ,ω);Ы=Ы.LengthSquared()<0.0001f?-Ǝ:Vector3.Normalize(Ы);Ȫ Αζ=null;bool Ευ=Φ==Α.Ѕ&&!ȏ(1);for(int i=0;i<о.Count;i++){Ȫ γ=о[i];if(γ.ƃ==3&&γ.ȣ.LengthSquared()<0.25f) continue;Vector3 λ=γ.λ+γ.ȣ*(float)(DateTime.Now-γ.ΔΗ).TotalSeconds;Vector3 Ř=λ-ή;
float ȓ=(δ+γ.Εδ)*0.5f;bool ΖΧ=γ.Ć!=""&&γ.Ć==ĵ();if(ΖΧ&&γ.Αδ==2) continue;bool ΑЀ=ΖΧ&&γ.Αδ==1&&Δω();if(ΑЀ) ȓ=Math.Min(ǟ,ǲ)*0.9f;float ΓΚ=ȓ*(ΑЀ?1f:1.2f);float Ÿ=Γα?Vector3.Dot(Ř,ζ):0;if(Ÿ>0&&(Ř-ζ*Ÿ).Length()<=ΓΚ){float Αυ=Vector3.Dot(γ.ȣ,ζ);
bool Εϋ=γ.Ƌ.LengthSquared()>0.25f;bool Εό=Αυ<-1f||(Εϋ&&Vector3.Dot(γ.Ƌ,ζ)<-0.5f);if(!ΖΡ){if(γ.ΓЄ){if(!Ευ){ȡ=Ы*ȓ*1.3f;continue;}float ΖΗ=Vector3.Distance(ή,υ),ΕЈ=Vector3.Distance(λ,υ);if(ΖΗ<ΕЈ||(ΖΗ==ΕЈ&&м<γ.χ)) continue;}if(Εό){if(Ÿ<(Ȟ-Αυ)*6f+ȓ*4f) ȡ=Ы*ȓ*1.3f;
continue;}if(Εϋ&&м<γ.χ){Vector3 ΕΠ=-Ř;float ΖΞ=Vector3.Dot(ΕΠ,γ.Ƌ);if(ΖΞ>0&&(ΕΠ-γ.Ƌ*ΖΞ).Length()<=ΓΚ){ȡ=Ы*ȓ*1.3f;continue;}}}float Ετ=Ÿ-ȓ-(ΑЀ?1f:followGap*δ);float Γν=Math.Max(0,Αυ)+Math.Max(0,Ετ)/ΒΡ;if(Εό) Γν=Math.Max(0,Ετ+Math.Min(0,Αυ)*ΒΡ*2)/ΒΡ;
if(э<0||Γν<э){э=Γν;Αζ=γ;}continue;}if(γ.ȣ.LengthSquared()<1f) continue;if(Γα&&(Ř-ζ*Ÿ).Length()<=ΓΚ) continue;Vector3 ΓЉ=γ.ȣ-Ζσ;float ΖΦ=ΓЉ.LengthSquared();if(ΖΦ<0.25f) continue;float t=-Vector3.Dot(Ř,ΓЉ)/ΖΦ;if(t<=0||t>5f||(Ř+ΓЉ*t).Length()>=(ΑЀ?ȓ:ȓ*1.1f)) continue;
if(м>γ.χ||t<2f){э=0;Αζ=γ;}}bool ΘΪ=э>=0&&э<0.5f;if(!ΘΪ||ΖΡ||(Αζ!=null&&Αζ.ΓЄ&&Ευ)){ΑЈ=false;return;}if(!ΑЈ){ΑЈ=true;ΔΜ=DateTime.Now;}double Εύ=(DateTime.Now-ΔΜ).TotalSeconds;if(Εύ>ΕΒ+Ητ) ΔΜ=DateTime.Now;else if(Εύ>ΕΒ){э=-1;ȡ=Ы*δ*1.3f;Κ="Traffic: passing a stuck ship";}
}List<int>ȃ=new List<int>();String ΕϏ="";void ΒΠ(){String Ō=Н+"|"+в+"|"+(int) Ģ;if(Ō==ΕϏ) return;ΕϏ=Ō;ΖϏ(Н,в,Ģ,ȃ);}void ΖϏ(int w,int h,Ǐ ΓБ,List<int>ΓΕ){ΓΕ.Clear();if(ΓБ==Ǐ.Ǉ){for(int k=0;k<w*h;k++){int ř=k/w;ΓΕ.Add(ř*w+(ř%2==0?k-ř*w:w-1-(k-ř*w)));
}return;}int Εϊ=(int) Math.Ceiling(w/2f),Εω=(int) Math.Ceiling(h/2f);int ΘΦ=w/2,ΘΥ=h/2;int x=0,y=0,ΓΣ=0,ΓΤ=-1,Αμ=Math.Max(w,h);for(int i=0;i<Αμ*Αμ;i++){if(-Εϊ<x&&x<=ΘΦ&&-Εω<y&&y<=ΘΥ) ΓΕ.Add((y-1+Εω)*w+(x-1+Εϊ));if(x==y||(x<0&&x==-y)||(x>0&&x==1-y)){int t=ΓΣ;ΓΣ=-ΓΤ;ΓΤ=t;}
x+=ΓΣ;y+=ΓΤ;}}bool ΒΟ(){int n=Math.Max(0,Н*в);if(Л.Length==n) return false;Л=new String('0',n);Ѐ=-1;return true;}void ΔЅ(String γ){ΒΟ();if(γ.Length!=Л.Length) return;char[]h=Л.ToCharArray();for(int i=0;i<h.Length;i++) if(γ[i]=='1') h[i]='1';Л=new String(h);}bool Ƹ(int Ȓ){return Ȓ>=0&&Ȓ<Л.Length&&Л[Ȓ]=='1';}
void Βφ(int Ȓ,bool є){if(Ȓ<0||Ȓ>=Л.Length) return;char[]h=Л.ToCharArray();h[Ȓ]=є?'1':'0';Л=new String(h);}bool Βί(int Ȓ,bool Θΰ){if(!trafficEnabled) return false;String Ō=ĵ();for(int i=0;i<о.Count;i++){Ȫ t=о[i];if(t.Ć!=Ō||t.Γγ<0||(Θΰ&&t.χ>м)) continue;if(t.Γγ==Ȓ) return true;
}return false;}int ΖЈ(int Β){int n=Н*в;if(n<1) return Β;ΒΠ();if(ΒΟ()) for(int k=0;k<Β&&k<n;k++) Βφ(ȃ[k],true);if(Ѐ>=0&&(Β>=n||ȃ[Β]!=Ѐ)){Βφ(Ѐ,true);Ѐ=-1;}if(ΔЁ()){if(ǫ>=0&&!Ƹ(ǫ)){Ā=false;Ѐ=ǫ;return ȃ.IndexOf(ǫ);}if(ǫ==-2){Ѐ=-1;Ā=Л.IndexOf('0')>=0;return Ā?-2:n;}
if(ǫ==-1&&Ѐ>=0&&!Ƹ(Ѐ)){Ā=false;return Β;}Ѐ=-1;if(!Ā){Ā=true;Εέ=DateTime.Now;}if((DateTime.Now-Εέ).TotalSeconds<5) return-1;}Ā=false;if(Ѐ>=0&&!Ƹ(Ѐ)&&!Βί(Ѐ,true)) return Β;Ѐ=-1;for(int k=0;k<n;k++){int c=ȃ[k];if(Ƹ(c)||Βί(c,false)) continue;Ѐ=c;
return k;}return Л.IndexOf('0')>=0?-2:n;}const String ΗΧ="[PAMJOB]";const String Ησ="[PAMTCC]";IMyBroadcastListener ΑΚ=null;DateTime Εά=DateTime.MinValue;DateTime Εέ=DateTime.Now;int ǫ=-1;bool Ā=false;bool ΔЁ(){return trafficEnabled&&ǫ!=-3&&(DateTime.Now-Εά).TotalSeconds<3;}
void Ηά(String Χ){String[]f=Χ.Split(';');if(f.Length<4||f[0]!="J"||f[1]==""||f[1]!=ĵ()) return;Εά=DateTime.Now;ΔЅ(f[2]);ǫ=-3;String Ƒ=м+":";String[]e=f[3].Split(',');for(int i=0;i<e.Length;i++) if(e[i].StartsWith(Ƒ)) ǫ=int.Parse(e[i].Substring(Ƒ.Length));}Ĺ Ηλ(int Β)
{ΒΠ();if(Β<0||Β>=ȃ.Count) return Ĺ.Δέ;Ш=ȃ[Β]%Н;Щ=ȃ[Β]/Н;return Ĺ.Δξ;}class ѐ{public long χ;public Vector3 λ,В,ω;}List<ѐ>Ў=new List<ѐ>();bool Ȯ=false;Vector3 ΑΥ,ΑΤ,ΑΦ,ΑΧ;long ц=0;long Ȇ=0;Ψ ј=null;bool Ƅ=false;Ψ ΒΜ(Vector3 λ,Vector3 В,Vector3 ω){MatrixD m=MatrixD.CreateWorld(λ,В,ω);
Ψ w=new Ψ(Θ);w.β=Vector3D.TransformNormal(ΑΥ,m);w.α=Vector3D.TransformNormal(ΑΤ,m);w.τ=Vector3D.TransformNormal(ΑΦ,m);w.Δ=λ+(Vector3) Vector3D.TransformNormal(ΑΧ,m);w.ε=true;return w;}int Δλ(){Α s=Φ;if(s==Α.з) return П(MyShipConnectorStatus.Connected)!=null?3:0;if(s==Α.đ||s==Α.Ǽ||s==Α.Đ||s==Α.ǋ||
s==Α.Ʋ||s==Α.ǐ) return 3;if(s==Α.ǡ||s==Α.č||s==Α.ű) return 2;if(s==Α.Ѕ) return 1;return 0;}long ΖЉ(){if(Δλ()==0) return 0;if(ȏ(1)&&ŝ!=0) return ŝ;return ј!=null?Ȇ:ц;}void ΔЂ(){if(Η==Ζ.Ϋ||!Θ.ε) return;IMyShipConnector И=П(MyShipConnectorStatus.Connected);if(И==null||И.OtherConnector==null) return;
IMyShipConnector γ=И.OtherConnector;Vector3I ΘΫ=new Vector3I((int) Θ.Ч.X,(int) Θ.Ч.Y,(int) Θ.Ч.Z);if(И.Position==ΘΫ){MatrixD ŉ=MatrixD.Transpose(γ.WorldMatrix);ΑΥ=Vector3D.TransformNormal(Τ.WorldMatrix.Forward,ŉ);ΑΤ=Vector3D.TransformNormal(Τ.WorldMatrix.Down,ŉ);ΑΦ=Vector3D.TransformNormal(Τ.WorldMatrix.Left,ŉ);
ΑΧ=Vector3D.TransformNormal(И.GetPosition()-γ.GetPosition(),ŉ);Ȯ=true;}if(Vector3.Distance(И.GetPosition(),Θ.Δ)<5) ц=γ.EntityId;List<IMyShipConnector>Ğ=new List<IMyShipConnector>();ϊ.GetBlocksOfType(Ğ,q=>q.CustomName.Contains(dockPoolTag)&&!q.CubeGrid.IsSameConstructAs(Me.CubeGrid));
if(Ğ.Count>0){Ў.Clear();for(int i=0;i<Ğ.Count;i++){ѐ p=new ѐ();p.χ=Ğ[i].EntityId;p.λ=Ğ[i].GetPosition();p.В=Ğ[i].WorldMatrix.Forward;p.ω=Ğ[i].WorldMatrix.Up;Ў.Add(p);}}ј=null;Ȇ=0;if(γ.EntityId==ц||!Ȯ) return;for(int i=0;i<Ў.Count;i++)
if(Ў[i].χ==γ.EntityId){Ȇ=Ў[i].χ;ј=ΒΜ(Ў[i].λ,Ў[i].В,Ў[i].ω);}}bool Ββ(long χ,int ΖΘ){if(χ==0) return false;for(int i=0;i<о.Count;i++){Ȫ t=о[i];if(t.ά==χ&&(t.ƃ>ΖΘ||(t.ƃ==ΖΘ&&t.χ<м))) return true;}return false;}bool ΒΗ(int ΓЁ){if(Η==Ζ.Ϋ||!trafficEnabled){ј=null;Ȇ=0;return true;}
long θ=ј!=null?Ȇ:ц;if(!Ββ(θ,ΓЁ)) return true;if(!Ββ(ц,ΓЁ)){ј=null;Ȇ=0;return true;}if(!Ȯ) return false;ѐ Ǿ=null;float ΕΤ=0;for(int i=0;i<Ў.Count;i++){ѐ p=Ў[i];if(p.χ==ц||Ββ(p.χ,ΓЁ)) continue;float d=Vector3.Distance(p.λ,Θ.Δ);if(Ǿ==null||d<ΕΤ){Ǿ=p;ΕΤ=d;}
}if(Ǿ==null) return false;Ȇ=Ǿ.χ;ј=ΒΜ(Ǿ.λ,Ǿ.В,Ǿ.ω);return true;}String ΖБ(){String s=(Ȯ?"1":"0")+":"+κ(ΑΥ)+":"+κ(ΑΤ)+":"+
κ(ΑΦ)+":"+κ(ΑΧ)+":"+ц;for(int i=0;i<Ў.Count;i++)
s+=":"+Ў[i].χ+","+κ(Ў[i].λ)+","+κ(Ў[i].В)+","+κ(Ў[i].ω);return s;}void ΖА(String s){Ў.Clear();Ȯ=false;ц=0;String[]f=s.Split(':');if(f.Length<6) return;Ȯ=f[0]=="1";ΑΥ=μ(f[1]);ΑΤ=μ(f[2]);ΑΦ=μ(f[3]);ΑΧ=μ(f[4]);long.TryParse(f[5],out ц);for(int i=6;i<f.Length;i++)
{String[]e=f[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();if(!long.TryParse(e[0],out p.χ)) continue;p.λ=μ(e[1]);p.В=μ(e[2]);p.ω=μ(e[3]);Ў.Add(p);}}const String ΒΙ="[PAMDCK]";IMyBroadcastListener ΑЃ=null;int Ȃ=0;DateTime Εθ=DateTime.Now;DateTime Ζΰ=DateTime.MinValue;long Ζα=0;
int Ζβ=0;long ŝ=0;bool Ʀ=false;int ΓΡ=0;List<Vector3>ƨ=new List<Vector3>();List<float>ΔΫ=new List<float>();bool ΑΣ=false;bool ΑΡ=false;Vector3 Εη=new Vector3();bool Ŵ(){return Ȃ!=0&&(DateTime.Now-Ζΰ).TotalSeconds<3;}bool ȏ(int Αε){return Ŵ()&&Ζα==м&&Ζβ==Αε;}bool ΔЏ(){return Ŵ()||(DateTime.Now-Εθ).TotalSeconds<3;}
void Βτ(int Αε){if(Ȃ==Αε) return;Ȃ=Αε;Εθ=DateTime.Now;ΓΡ=3;}void Βσ(){if(Ȃ!=0&&trafficEnabled) IGC.SendBroadcastMessage(ΒΙ,"D;"+м);Ȃ=0;Ʀ=false;}void Ηϋ(){if(!trafficEnabled||Η==Ζ.Ϋ) return;if(ΑЃ==null) ΑЃ=IGC.RegisterBroadcastListener(ΒΙ);int Ě=20;while(ΑЃ.HasPendingMessage&&Ě-->0)
{String Χ=ΑЃ.AcceptMessage().Data as String;if(Χ==null||!Χ.StartsWith("S;")) continue;try{Ηέ(Χ);}catch{}}if(Φ!=Α.Ѕ){ΑΣ=false;ΑΡ=false;}if(Ȃ==1&&П(MyShipConnectorStatus.Connected)!=null) Βσ();if(Ȃ==2&&!Ʀ&&Φ!=Α.ǐ&&
Vector3.Distance(ή,ж().Δ)>δ*3) Βσ();if(Ȃ!=0&&++ΓΡ>=3){ΓΡ=0;IGC.SendBroadcastMessage(ΒΙ,"R;"+м+";"+Ȃ+";"+ц+";"+(Ȯ?1:0));}}void Ηέ(String Χ){String[]f=Χ.Split(';');if(f.Length<7) return;bool Ες=false;String[]ΕΖ=f[5].Split(',');for(int i=0;i<ΕΖ.Length;i++) if(ΕΖ[i]==м.ToString()) Ες=true;
if(!Ες) return;Ζΰ=DateTime.Now;Ζα=long.Parse(f[2]);Ζβ=int.Parse(f[3]);ŝ=long.Parse(f[4]);ƨ.Clear();ΔΫ.Clear();if(f.Length>=8&&f[7]!=""){String[]z=f[7].Split(':');for(int i=0;i<z.Length;i++){String[]e=z[i].Split(',');if(e.Length<2) continue;ƨ.Add(μ(e[0]));ΔΫ.Add(float.Parse(e[1]));
}}if(f[6]=="") return;String[]Εκ=f[6].Split(':');List<ѐ>Т=new List<ѐ>();for(int i=0;i<Εκ.Length;i++){String[]e=Εκ[i].Split(',');if(e.Length<4) continue;ѐ p=new ѐ();p.χ=long.Parse(e[0]);p.λ=μ(e[1]);p.В=μ(e[2]);p.ω=μ(e[3]);Т.Add(p);}Ў=Т;}bool ΖІ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;
Βτ(1);if(!ΔЏ()) return true;if(!ȏ(1)){Κ="Waiting for dock clearance";return false;}ΑБ();return true;}bool ΑБ(){Ƅ=false;ѐ ĥ=null;if(ŝ!=0&&ŝ!=ц&&Ȯ)
for(int i=0;i<Ў.Count;i++) if(Ў[i].χ==ŝ) ĥ=Ў[i];long ΔΣ=ĥ==null?0:ĥ.χ;if(ΔΣ==Ȇ&&(ΔΣ==0)==(ј==null)) return false;Ȇ=ΔΣ;ј=ĥ==null?null:ΒΜ(ĥ.λ,ĥ.В,ĥ.ω);return true;}bool Ĵ(Vector3 p){for(int i=0;i<ƨ.Count;i++) if(Vector3.Distance(p,ƨ[i])<ΔΫ[i]+δ*0.75f) return true;
return false;}bool ΗΜ(Vector3 ſ,Vector3 Œ){ΑΣ=false;if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return false;if(Ȃ!=1&&Vector3.Distance(ή,Θ.Δ)>dockRequestDistance) return false;Βτ(1);if(!Ŵ()||ȏ(1)||ƨ.Count==0) return false;if(!Ĵ(ſ)&&!Ĵ(Œ)&&!Ĵ(ή)) return false;ΑΣ=true;return true;
}bool Δδ(){if(!trafficEnabled||Η==Ζ.Ϋ||ц==0) return true;Βτ(2);if(!ΔЏ()) return true;if(!ȏ(2)){Κ="Waiting for undock clearance";return false;}return true;}void ΖЎ(string ђ){Echo("[PAM] This is the ship script.\nPaste PAM_Controller.cs into the [PAM-Controller] PB.");}