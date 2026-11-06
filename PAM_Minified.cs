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

//Server restart / world load
const bool resumeAfterRestart = true;//continue a job that was running when the world was saved (false = stay paused, like the original)

//Check if blocks are connected with conveyors
const bool checkConveyorSystem = false;//temporarily disabled because of a SE bug

// ===== End of settings: don't edit below this line =====
public enum Μ{ȡ,Š,ћ,к,ΰ}public enum Γ{л,ă,š,ȟ,ą,ǝ,г,
Ă,ř,ț,Ř,ŕ,Ȳ,
ΒΠ,ȉ,Ƞ,ĥ,Ββ,Αχ,
Βα,Ƴ,Ɯ,Αω,Ȣ}public enum Ρ{м,ΒΛ,А,Αυ,Ƙ,Ƥ,ƥ,Ġ,ġ}public enum й{None,ĥ,л,ΓЀ}public enum Ξ{Main,ΔΠ,ş,Ş,Βά,Ɩ,
Αζ,ΓБ,ΔΑ,ΓА,Ť,ΔΦ,
Ƭ,ΒΤ,ΒΧ,ΔΧ,Ʈ}public enum Ь{ƫ,ŗ,ΖΦ,ΔΔ,ΰ}public enum Ǡ{Ư,Γϊ}public enum б{ƪ,Αϊ,ΔΨ,ś}public enum Ђ{ΒΑ,Γύ,Αρ,Αφ,ƙ,ΑЅ,Ơ}public enum љ{Ασ,Αλ,Αμ}public enum Й{Ĩ,ΖΒ,ȳ}public enum ζ{ΒΓ,ΒΗ,ΒΘ,ΔΛ,ΔΚ,
ΒΒ,ƨ,Ƨ,ΒΕ,
ΒΖ,ΒΔ}public enum ί{ǯ,Ȇ,Ǯ,ƕ,Αι,Ακ}public enum Ȉ{Αϋ,Αθ,ΕЃ}public enum ш{Βή,ΓЁ,ΖΗ,ΑА}public enum Д{None,Ţ,ΒΞ,ΑБ}public enum π{ť,Ɣ,Ƣ,ś,ΑЎ,ƣ,ΑЏ,ΖΩ,ΒΙ}public enum Е{ȡ,ΔΘ,ΔΗ,ΑЋ,ΑЇ,Ας,ΔΙ}class έ{public bool δ=false;public Vector3 Ε=new Vector3();
public Vector3 γ=new Vector3();public Vector3 α=new Vector3();public Vector3 ϊ=new Vector3();public Vector3 χ=new Vector3();public Vector3 о=new Vector3();public float Х=0;public float ΗЏ=0;public float[]ǚ=null;public έ(){}public έ(έ Ǿ){δ=Ǿ.δ;Ε=Ǿ.Ε;γ=Ǿ.γ;α=Ǿ.α;ϊ=Ǿ.ϊ;χ=Ǿ.χ;
о=Ǿ.о;ǚ=Ǿ.ǚ;}public έ(Vector3 Ε,Vector3 α,Vector3 γ,Vector3 ϊ,Vector3 χ){this.Ε=Ε;this.γ=γ;this.α=α;this.ϊ=ϊ;this.Х=0;this.χ=χ;}public void Ευ(List<IMyThrust>ƶ,List<string>ǿ){ǚ=new float[ǿ.Count];for(int i=0;i<ǚ.Length;i++)
ǚ[i]=-1;for(int i=0;i<ƶ.Count;i++){string ч=ΓЎ(ƶ[i]);int Α=ǿ.IndexOf(ч);if(Α!=-1)
ǚ[Α]=І(ƶ[i].MaxEffectiveThrust,ƶ[i].MaxThrust);}}}class ΒΚ{public έ ι=null;public List<Vector3>Ŏ=new List<Vector3>();public float Ů=0;public float Γξ=0;public float İ=0;public float Βό=0;public Vector3 Ǆ=new Vector3();}class Ĭ{public ζ Г=ζ.ΒΓ;public float Ǎ=0;public float ȧ=0;
public string Ǐ="";public string ǒ="";DateTime ΔЍ;public bool ΓΤ=false;private bool Βυ=false;public bool Αο(bool Ǘ){if(Ǘ){ȧ=0;Βυ=false;return false;}Βυ=true;return ȧ>Ǎ;}public void ΖΚ(){Αο(true);ΓΤ=false;}public void Ζδ(){if(Βυ)
if((DateTime.Now-ΔЍ).TotalSeconds>1){ȧ++;ΔЍ=DateTime.Now;}}public bool ΑЃ(){switch(Г){case ζ.ΒΗ:return true;case ζ.ΒΘ:return true;}return false;}}class Ц{public String Ϊ="";public String ν="";public int ύ=0;public Й ľ=Й.ȳ;public Ц(String Ϊ,String ν,int ύ,Й ľ){this.Ϊ=Ϊ;this.ν=ν;
this.ύ=ύ;this.ľ=ľ;}}class ΕЀ{public ΕЀ(Vector3 И,float Ћ){this.И=И;this.Ћ=Ћ;}public Vector3 И;public float Ћ;}class Б{public DateTime ΒЋ;public DateTime Δκ;public String Ǻ="";public String Ϊ="";public String Αδ="";public String Λ="";public String Γί="";public String ΑΠ="";public String ΓΣ="";
public Vector3 Ε=new Vector3();public List<Ц>Ϗ=new List<Ц>();public Μ Π=Μ.ȡ;public Д Ȥ;public float Ǫ;public float Ǥ;public int Ѐ;public int Βψ=0;public int Γκ=0;public bool ΔΖ(){return(DateTime.Now-ΒЋ).TotalSeconds>10;}public bool ΑЈ(){if(Ȥ!=Д.Ţ) return false;return(DateTime.Now-Δκ).TotalSeconds>=4;
}public Б(String Ǻ){this.Ǻ=Ǻ;}public Д ĩ(String Њ,bool Ηε,bool Ŭ,bool Ζς){if(Њ==""&&!Ŭ) return Д.None;if(Ȥ==Д.Ţ&&ΑЈ())
Ȥ=Д.ΑБ;if(Ηε){ΓΣ=Њ;Ȥ=Д.Ţ;Δκ=DateTime.Now;return Ȥ;}else if(Ŭ){ΓΣ="";Ȥ=Д.None;}else if(ΓΣ==Њ){if(Ζς) Ȥ=Д.ΒΞ;return Ȥ;}return Д.None;}}public String ΕЉ(String ΗΕ){switch(ΗΕ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem ω;Vector3 ΑΧ=new Vector3();Vector3 μ=new Vector3();Vector3 Ǉ=new Vector3();Vector3 ΑΖ=new Vector3();Vector3 ǃ=new Vector3();
DateTime ΒЎ=new DateTime();bool ȍ=true;int Γε=0;bool ȑ=false;bool ΑΓ=false;bool ż=true;bool Ερ=false;bool ļ=false;bool ǌ=false;float ě=0;float Ě=0;int ȯ=0;int Ǝ=0;int ƀ=0;int Βρ=0;float ŀ=0;float ı=0;double ΓΙ=0;float ũ=0;List<int>ĺ=new List<int>();List<int>Ň=new List<int>();
Μ Π=Μ.ȡ;int Є=0;float Ș=0;float ȁ=0;float ψ=0;IMyRemoteControl Ψ;IMySensorBlock ξ;List<IMyTimerBlock>Γη=new List<IMyTimerBlock>();List<IMyShipConnector>п=new List<IMyShipConnector>();List<IMyThrust>Ȁ=new List<IMyThrust>();List<IMyGyro>ǈ=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>Ǽ=new List<IMyLandingGear>();List<IMyReactor>Ȯ=new List<IMyReactor>();List<IMyConveyorSorter>Αά=new List<IMyConveyorSorter>();List<IMyGasTank>ю=new List<IMyGasTank>();List<IMyTerminalBlock>Ŧ=new List<IMyTerminalBlock>();List<IMyTerminalBlock>Ĵ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ċ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȣ=new List<IMyBatteryBlock>();List<IMyTextPanel>З=new List<IMyTextPanel>();List<IMyTextSurface>Ǭ=new List<IMyTextSurface>();List<IMyTextPanel>Ď=new List<IMyTextPanel>();IMyTerminalBlock ё=null;έ Ν=new έ();έ ΒІ=new έ();
έ ΒЉ=new έ();bool Ȃ=false;bool Ő=false;bool ő=false;bool ΑΒ=false;List<String>ǿ=new List<string>();List<έ>Φ=new List<έ>();List<έ>ƒ=new List<έ>();int Βω=0;double Ες=0;έ Θ=new έ();Vector3 ǽ;Vector3 ĕ;Ρ ε=Ρ.м;Ǡ ΑΕ=Ǡ.Ư;int Ч=0;int с=0;double ŷ=0;bool ǀ=false;Ĭ ϋ=new Ĭ();
Ĭ ό=new Ĭ();б ж=б.ƪ;bool Ȱ=true;Ǡ Ȗ=Ǡ.Ư;int ь=3;int ы=3;int д=30;љ ǎ=љ.Ασ;bool Ț=true;bool Ļ=true;bool ĵ=true;bool Ĝ=false;Ђ С=Ђ.ΒΑ;bool ș=true;float ǩ=90;float т=20;float Ш=5;float у=20;float ǡ=0.70f;float Ŕ=1.50f;float œ=2.50f;float Ɠ=0f;float Ĺ=0f;Γ ά;
Ρ я;Γ ƅ;ΒΚ θ=null;int υ=0;int σ=0;int р=0;int Ȭ=0;int ę=30;int ǖ=0;bool Џ=true;Vector3 ф;double Р=0;bool ΑΩ=false;int Γρ=0;int Љ=0;int Βι=0;int ė=0;int ΕΠ=0;Vector3 ž=new Vector3();float е=0;float Щ=0;int ğ=0;έ Αέ=null;float ΒЌ=0;float Ƹ=0;float Ф=0;float Ł=0;
bool ΒЇ=false;bool ĝ=false;bool Ə=false;bool ȏ=false;bool ȋ=false;DateTime ΖЄ=new DateTime();έ Č=null;έ Δο=null;float ǵ=0;й Ǣ;float Ȑ=0;float Ğ=0;String ΓΓ="";float ΑΗ=0;float Ǥ=0;float Ǫ=0;List<Ц>Ǒ=new List<Ц>();float Т=0;bool ƃ=false;bool Εθ=false;bool ΕΪ=false;
bool Αί=false;bool ΕΫ=false;float ΓΗ=0;float ΓΡ=0;Vector3 Δώ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 Αΰ=new Vector3(1,1,1);float Γδ=1;Vector3 Ɗ=new Vector3();bool ΑΑ=false;bool ΒЂ=false;bool ΒБ=false;bool Δί=false;float ƾ=0;float Ƶ=2;Vector3 Εη;Vector3 Γγ;Vector3 Γβ;
float[,]Ǜ=new float[3,2];Dictionary<String,float[,]>ѕ=new Dictionary<string,float[,]>();Dictionary<String,float>Εο=new Dictionary<String,float>();int Βν=0;Dictionary<String,float[]>Ȕ=new Dictionary<string,float[]>();float ΓΥ;bool ǁ=false;int Ʒ=0;int Ȫ=0;int Ǌ=0;int Đ=0;
String Λ="";int[]ΕΘ=new int[Enum.GetValues(typeof(Ξ)).Length];bool ΓΛ=false;int Ĕ=0;int Ѐ=0;Ξ ρ=Ξ.Main;int Ητ=0,Ηυ=0,Ηφ=0,Ηχ=0;int Ηΰ=0,Ηα=0;bool ĳ=false;bool Δΰ=false;String Ō="";String Βπ="";IMyBroadcastListener ѝ=null;bool Δύ=true;List<Б>П=null;Б Ј=null;
int ƻ=0,Ƽ=0;int Δν=0,Δξ=0;int Ζψ=0,Ζω=0,Ζϊ=0;String ΗΗ="";Exception Ȓ=null;bool ΑΨ=false;Ρ ŉ=Ρ.м;bool Δς=false;bool ΔЋ=true;bool ΓΩ=false;String Επ="";bool ƍ=false;bool Γή=true;int ǋ=0;int[]ѓ=null;Vector3 Βς=new Vector3();bool ΔЀ=false;bool щ(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;
Program(){ω=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΑЄ(Me,controllerTag,true))
Π=Μ.к;β(Ξ.Main);if(Π!=Μ.к){Λ="Welcome to [PAM]!";ш ΓΨ=ΖΛ();if(Π==Μ.ȡ){List<IMyShipDrill>Δϊ=new List<IMyShipDrill>();List<IMyShipGrinder>ΔЁ=new List<IMyShipGrinder>();ω.GetBlocksOfType(Δϊ,q=>q.CubeGrid==Me.CubeGrid);ω.GetBlocksOfType(ΔЁ,q=>q.CubeGrid==Me.CubeGrid);if(Δϊ.Count>0)
{Π=Μ.Š;Λ="Miner mode enabled!";}else if(ΔЁ.Count>0){Π=Μ.ћ;Λ="Grinder mode enabled!";}else{Π=Μ.ΰ;ǳ(Ь.ΰ);}}if(ΓΨ==ш.Βή) ż=false;if(ΓΨ==ш.ΓЁ) Λ="Data restore failed!";if(ΓΨ==ш.ΑА) Λ="New version, wipe data";}}void Main(string Ж,UpdateType Ηώ){try{if(Ȓ!=null){if(Ж.Trim().ToUpper()=="RETRY")
{Ȓ=null;ȍ=true;ȯ=0;if(Π!=Μ.к) τ();Λ="Restarted after error";}else{ΕБ();ΔΞ();return;}}ΑΓ=(Ηώ&UpdateType.Update10)!=0;if(ΑΓ) Γε++;ȑ=Γε>=10;if(ȑ) Γε=0;if(ΑΓ){Ʒ++;if(Ʒ>4) Ʒ=0;ƀ=(int) Math.Max(0,10-(DateTime.Now-ΒЎ).TotalSeconds);}if(Ж!="") ΗΗ=Ж;if(Π!=Μ.к)
ΖΥ(Ж);else
Εώ(Ж);ΓΛ=false;try{int Δπ=Runtime.CurrentInstructionCount;float Ę=І(Δπ,Runtime.MaxInstructionCount);if(Ę>0.90) Λ="Max. instructions >90%";if(Ę>ŀ) ŀ=Ę;if(ǁ){ĺ.Add(Δπ);while(ĺ.Count>10) ĺ.RemoveAt(0);ı=0;for(int i=0;i<ĺ.Count;i++) ı+=ĺ[i];ı=І(І(ı,ĺ.Count),Runtime.MaxInstructionCount);
double ΒЍ=Runtime.LastRunTimeMs;if(ǌ&&ΒЍ>ΓΙ) ΓΙ=ΒЍ;Ň.Add((int)(ΒЍ*1000f));while(Ň.Count>10) Ň.RemoveAt(0);ũ=0;for(int i=0;i<Ň.Count;i++) ũ+=Ň[i];ũ=І(ũ,Ň.Count)/1000f;}}catch{ŀ=0;}}catch(Exception e){Ȓ=e;}}void ΖΥ(string Ж){bool ΑΪ=false;String Δη="";if(Є<=1&&!ΑЉ(Ж))
ΔΟ(Ж);int ΓΜ=50;while(ѝ!=null&&ѝ.HasPendingMessage&&ΓΜ-->0){MyIGCMessage κ=ѝ.AcceptMessage();String Χ=κ.Data as String;if(Χ==null) continue;String ǫ="";if(Є<=1&&ΔΝ(ref Χ,out ǫ,out Δη)&&ǫ==BROADCAST_SELF_ID){ΔΟ(Χ);ΑΪ=true;break;}}bool ΔЇ=ǌ&&ά==Γ.л&&!ļ&&!ΑΪ&&ȯ==0&&!ΑΒ;
if(ȑ&&ά!=Γ.л) ΑΪ=true;if((ΑΓ&&!ΔЇ)||(ȑ&&ΔЇ)){if(ȯ==0&&(ƀ<=0||ȍ)){Ερ=Є>0;Є=0;ȯ=1;Ǵ();ΖΞ();ΔΒ();ǰ("Scan 1");}else if(ȯ==1){ȯ=2;Ǵ();ΖΟ();ǰ("Scan 2");}else if(ȯ==2){ȯ=0;Ǵ();ΖΠ();ǰ("Scan 3");ΒЎ=DateTime.Now;if(Є<=1&&ż)
ΑΧ=Αύ(Ψ,Ψ.CenterOfMass);ż=false;if(ȍ){ȍ=false;bool ΗЂ=ε==Ρ.А;τ();if(ΔЋ&&resumeAfterRestart&&ΗЂ&&Є<=1) ΓΩ=true;}ΔЋ=false;if(Ερ&&Є==0) Λ="Setup complete";}else{if(ε==Ρ.А&&Π!=Μ.ΰ){Ǵ();Εσ();ǰ("Inv balance");}Ǵ();switch(Ǝ){case 0:Ζά();break;case 1:Ζΰ();break;case 2:ΖΫ();break;
case 3:Ζε();break;case 4:Ζί();break;case 5:Εψ();break;case 6:Εχ(Ψ);break;}ǰ("Update: "+Ǝ);Ǝ++;if(Ǝ>6){Ǝ=0;ǌ=true;if(ŉ!=Ρ.м){switch(ŉ){case Ρ.Ġ:ȝ();break;case Ρ.ġ:ȝ();break;case Ρ.А:ȝ();break;case Ρ.Ƥ:Ɲ();break;case Ρ.ƥ:ā();break;}ŉ=Ρ.м;}}}if(!ȍ){if(!ǲ(Ψ,true)){Ψ=null;
ż=true;Є=2;}if(Є>=2&&ά!=Γ.л)
τ();if(Є<=1){Ě=Ψ.CalculateShipMass().PhysicalMass;ě=(float) Ψ.GetShipSpeed();μ=ΖΪ(Ψ,ΑΧ);Ǉ=Ψ.WorldMatrix.Forward;ΑΖ=Ψ.WorldMatrix.Left;ǃ=Ψ.WorldMatrix.Down;Ζβ();if(ΓΩ){ΓΩ=false;ȝ();if(ά!=Γ.л) Λ="Job resumed after restart";}if(ά!=Γ.л){ļ=false;ΔΣ(false);Γψ(false);String Ηλ=ΑϏ(ά)+" "+(int) ά;
Ǵ();Ζα();Βΰ(false);ǰ(Ηλ);Ǵ();Ζή();ǰ("Thruster");Ǵ();Δά();ǰ("Gyroscope");}else{if(ļ){if(ơ()){Ъ(ǃ,Ǉ,ΑΖ,0.25f,true);Δά();Λ="Aligning to planet: "+Math.Round(ƾ-0.25f,2)+"°";if(ΑΑ) ΑЂ(true,true);}else ΑЂ(true,true);}}ƍ=false;}}}Ǵ();ΔΞ();ǰ("Print");if(ΑΪ||Βρ<=0){Ǵ();Ετ(Δη);ǰ("Broadcast");
Βρ=4;}else if(ȑ) Βρ--;}void ΔΟ(string Ж){if(Ж=="") return;var λ=Ж.ToUpper().Split(' ');λ.DefaultIfEmpty("");var Њ=λ.ElementAtOrDefault(0);var Ȋ=λ.ElementAtOrDefault(1);var Ũ=λ.ElementAtOrDefault(2);var Δβ=λ.ElementAtOrDefault(3);String ǻ="Invalid argument: "+Ж;bool Γμ=false;switch(Њ)
{case "UP":this.ΑЍ(false);break;case "DOWN":this.ΑЌ(false);break;case "UPLOOP":this.ΑЍ(true);break;case "DOWNLOOP":this.ΑЌ(true);break;case "APPLY":this.Γφ(true);break;case "MRES":Ѐ=0;break;case "STOP":this.τ();break;case "PATHHOME":{this.τ();this.ΒΫ();}break;case "PATH":{this.τ();this.ΒΫ();Ν.δ=true;}break;
case "START":{this.τ();ΒΪ();}break;case "ALIGN":{ΑЂ(!ļ,false);}break;case "CONT":{this.τ();this.ȝ();}break;case "JOBPOS":{this.τ();this.ā();}break;case "HOMEPOS":{this.τ();this.Ɲ();}break;case "FULL":{ƃ=true;}break;case "RESET":{ΑΨ=true;Є=2;}break;case "EXPORT":{ΕЄ();}break;
case "IMPORT":{ΖΑ();}break;default:Γμ=true;break;}if(Π!=Μ.ΰ){switch(Њ){case "SHUTTLE":{ΕЁ();}break;case "CFGS":{if(!Εό(Ȋ,Ũ,Δβ)) Λ=ǻ;}break;case "CFGB":{if(!Εϋ(Ȋ,Ũ)) Λ=ǻ;}break;case "CFGL":{if(!Ą(ref ǩ,true,π.Ƣ,Ȋ,"")||!Εύ(Ũ))
Λ=ǻ;}break;case "CFGE":{if(!Ą(ref Ш,true,π.ƣ,Ȋ,"IG")||
!Ą(ref т,true,π.ΑЎ,Ũ,"IG")||
!Ą(ref у,true,π.ΑЏ,Δβ,"IG"))
Λ=ǻ;}break;case "CFGA":{if(!Ą(ref ǡ,false,π.Ɣ,Ȋ,"")) Λ=ǻ;}break;case "CFGW":{if(!Ą(ref Ŕ,false,π.ť,Ȋ,"")||
!Ą(ref œ,false,π.ť,Ũ,""))
Λ=ǻ;}break;case "NEXT":{Αε(false);}break;case "PREV":{Αε(true);}break;default:if(Γμ) Λ=ǻ;break;}}else{switch(Њ){case "UNDOCK":{ƍ=true;}break;default:if(Γμ) Λ=ǻ;break;}}}String ΕЊ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n"+
"[EXPORT] Save path+job to "+dataTag+"\n"+"[IMPORT] Load path+job from "+dataTag+"\n";if(Π!=Μ.ΰ)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void ΕЁ(){τ();Π=Μ.ΰ;ǳ(Ь.ΰ);Θ.δ=false;Ν.δ=false;ξ=null;ο.Clear();ε=Ρ.м;}void ΑЂ(bool Ќ,bool ǥ){if(!Ќ) Λ="Aligning canceled";if(ǥ) Λ="Aligning done";if(ǥ||!Ќ){ļ=false;ΔΩ();ƭ(false,0,0,0,0);return;}if(ơ()) ļ=true;}bool Εό(String ΗЄ,String ΖЏ,String ΖϏ)
{bool ΗЀ=ε==Ρ.А;int w,h,d;if(int.TryParse(ΗЄ,out w)&&int.TryParse(ΖЏ,out h)&&int.TryParse(ΖϏ,out d)){this.τ();ь=w;ы=h;д=d;ĉ(false);Ʋ(false,false);if(ΗЀ) ȝ();return true;}return false;}bool Εύ(String ŧ){if(ŧ=="ON"){Ț=true;return true;}if(ŧ=="OFF"){Ț=false;return true;}
return false;}bool Εϋ(String Δω,String ƿ){bool Ė=true;if(Δω=="HOME") Ȱ=true;else if(Δω=="STOP") Ȱ=false;else Ė=false;if(ƿ=="HOME") ж=б.ƪ;else if(ƿ=="STOP") ж=б.ΔΨ;else if(ƿ=="JOB") ж=б.Αϊ;else if(ƿ=="IG") ж=б.ś;else Ė=false;return Ė;}void β(Ξ Э){ΕΘ[(int) ρ]=Ѐ;Ѐ=ΕΘ[(int) Э];
if(Э==Ξ.Ť) Ѐ=0;ρ=Э;if(Π!=Μ.к)
Ĉ(ρ==Ξ.ş,false,0,0);ΓΛ=true;}void ΑЍ(bool ΓΑ){if(Ѐ>0) Ѐ--;else if(ΓΑ) Ѐ=Ĕ-1;}void ΑЌ(bool ΓΑ){if(Ѐ<Ĕ-1) Ѐ++;else if(ΓΑ) Ѐ=0;}String ǳ(Ь κ){switch(κ){case Ь.ƫ:Λ="Job is running";break;case Ь.ŗ:Λ="Connector not ready!";break;case Ь.ΖΦ:Λ="Ship modified, path outdated!";break;
case Ь.ΔΔ:Λ="Interrupted by player!";break;case Ь.ΰ:Λ="Shuttle mode enabled!";break;}return "";}String ΕА(Ǡ И){switch(И){case Ǡ.Ư:return "Top-Left";case Ǡ.Γϊ:return "Center";default:return "";}}String ΕЇ(љ Ё){switch(Ё){case љ.Αλ:return "Auto"+(Π==Μ.Š?" (Ore)":"");case љ.Αμ:return "Auto (+Stone)";
case љ.Ασ:return "Default";default:return "";}}String Αό(Ρ φ){switch(φ){case Ρ.м:return "No job";case Ρ.ΒΛ:return "Job paused";case Ρ.А:return "Job active";case Ρ.Ġ:return "Job active";case Ρ.ġ:return "Job active";case Ρ.Αυ:return "Job done";case Ρ.Ƙ:return "Job changed";case Ρ.Ƥ:return "Move home";
case Ρ.ƥ:return "Move to job";}return "";}String ΓІ(б Ζρ){switch(Ζρ){case б.ƪ:return "Return home";case б.Αϊ:return "Fly to job pos";case б.ΔΨ:return "Stop";case б.ś:return "Ignore";}return "";}String ΕЈ(Ђ Ё){switch(Ё){case Ђ.ΒΑ:return "Off";case Ђ.Αφ:return "Drop pos (Stone) ";case Ђ.ƙ:return "Drop pos (Sto.+Ice)";
case Ђ.Γύ:return "Cur. pos (Stone)";case Ђ.Αρ:return "Cur. pos (Sto.+Ice)";case Ђ.ΑЅ:return "In motion (Stone)";case Ђ.Ơ:return "In motion (Sto.+Ice)";}return "";}String ΓЄ(й φ){switch(φ){case й.None:return "No batteries";case й.ĥ:return "Charging";case й.ΓЀ:return "Discharging";}return "";}String ΑϏ(Γ φ)
{String ΗЍ=Π==Μ.ΰ?"target":"job";switch(φ){case Γ.л:return "Idle";case Γ.ă:return "Flying to XY position";case Γ.š:return Π==Μ.ћ?"Grinding":"Mining";case Γ.ȟ:return "Returning";case Γ.Ɯ:return "Flying to drop pos";case Γ.ą:return "Returning to dock";case Γ.ǝ:return "Flying to dock area";
case Γ.г:return "Flying to job area";case Γ.Ă:return "Flying to path";case Γ.ř:return "Flying to job position";case Γ.ț:return "Approaching dock";case Γ.Ř:return "Docking";case Γ.ŕ:return "Aligning";case Γ.Ȳ:return "Aligning";case Γ.ΒΠ:return "Retry docking";case Γ.ȉ:return "Unloading";case Γ.Ȣ:return Επ;
case Γ.Ƞ:return "Undocking";case Γ.ĥ:return "Charging batteries";case Γ.Ββ:return "Waiting for uranium";case Γ.Αχ:return "Filling up hydrogen";case Γ.Βα:return "Waiting for ejection";case Γ.Ƴ:return "Waiting for ejection";case Γ.Αω:return "Flying to drop pos";}return "";}String ΓЏ(ζ Г){switch(Г)
{case ζ.ΒΓ:return "On \"Undock\" command";case ζ.ΒΗ:return "On player entered cockpit";case ζ.ΔΛ:return "On ship is full";case ζ.ΔΚ:return "On ship is empty";case ζ.ΒΘ:return "On time delay";case ζ.ƨ:return "On batteries empty(<25%)";case ζ.Ƨ:return "On batteries empty(=0%)";case ζ.ΒΒ:return "On batteries full";
case ζ.ΒΖ:return "On hydrogen empty(<25%)";case ζ.ΒΔ:return "On hydrogen empty(=0%)";case ζ.ΒΕ:return "On hydrogen full";}return "";}bool Ι(ref String Β,int ΕΧ,int ȩ,bool Η,String Ē){Ĕ+=1;if(ΕΧ==ȩ) Ē=">"+Ē+(Ʒ>=2?" .":"");else Ē=" "+Ē;Β+=Ē+"\n";return ΕΧ==ȩ&&Η;}String Γφ(bool Η)
{int Α=0;int Ζ=Ѐ;Ĕ=0;String ŋ="———————————————\n";String ŏ="--------------------------------------------\n";String Β="";Β+=Αό(ε)+" | "+(Ν.δ?"Ready to dock":"No dock")+"\n";Β+=ŋ;double ƈ=Math.Max(Math.Round(this.Р),0);if(ρ==Ξ.Main){bool ȫ=Π==Μ.ΰ;if(Ι(ref Β,Ζ,Α++,Η," Record path & set home")) ΒΫ();
if(Π==Μ.Š)
if(Ι(ref Β,Ζ,Α++,Η," Setup mining job")) β(Ξ.ş);if(Π==Μ.ћ)
if(Ι(ref Β,Ζ,Α++,Η," Setup grinding job")) β(Ξ.ş);if(Π==Μ.ΰ)
if(Ι(ref Β,Ζ,Α++,Η," Setup shuttle job")) β(Ξ.ΒΧ);if(Ι(ref Β,Ζ,Α++,Η," Continue job")) ȝ();if(Ι(ref Β,Ζ,Α++,Η," Fly to home position")) Ɲ();if(Ι(ref Β,Ζ,Α++,Η," Fly to job position")) ā();if(Ι(ref Β,Ζ,Α++,Η," Behavior settings"))
if(ȫ) β(Ξ.Ʈ);else β(Ξ.Ɩ);if(Ι(ref Β,Ζ,Α++,Η," Info")) β(Ξ.ΓБ);if(Π!=Μ.ΰ)
if(Ι(ref Β,Ζ,Α++,Η," Help")) β(Ξ.ΓА);}else if(ρ==Ξ.ş){double ΗЃ=Math.Round(ь*Ș,1);double ΖЎ=Math.Round(ы*ȁ,1);String Ѕ="";if(Ι(ref Ѕ,Ζ,Α++,Η," Start new job!")) ΒΪ();if(Ι(ref Ѕ,Ζ,Α++,Η," Change current job")){Ʋ(false,false);β(Ξ.Main);}if(Ι(ref Ѕ,Ζ,Α++,Η," Width + (Width: "+ь+" = "+ΗЃ+"m)")){Ģ(ref ь,5,20,1);ĉ(true);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Width -")){Ģ(ref ь,-5,20,-1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Height + (Height: "+ы+" = "+ΖЎ+"m)")){Ģ(ref ы,5,20,1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Height -")){Ģ(ref ы,-5,20,-1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Depth + ("+(ǎ==љ.Ασ?"Depth":"Min")+": "+д+"m)")){Ģ(ref д,5,50,2);ĉ(true);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Depth -")){Ģ(ref д,-5,50,-2);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Depth mode: "+ΕЇ(ǎ))){ǎ=Ȟ(ǎ);}if(Ι(ref Ѕ,Ζ,Α++,Η," Start pos: "+ΕА(Ȗ))){Ȗ=Ȟ(Ȗ);}if(Π==Μ.ћ&&ǎ==љ.Αμ) ǎ=Ȟ(ǎ);Β+=ΒΡ(8,Ѕ,Ζ,ref Ηΰ);}else if(ρ==Ξ.ΒΧ){float[]Δτ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ι(ref Β,Ζ,Α++,Η," Next")) β(Ξ.ΔΧ);if(Ι(ref Β,Ζ,Α++,Η," Back")) β(Ξ.Main);Β+=" Leave connector 1:\n";if(Ι(ref Β,Ζ,Α++,Η," - "+ΓЏ(ϋ.Г))) ϋ.Г=Ȟ(ϋ.Г);if(!ϋ.ΑЃ()) Β+="\n";else if(Ι(ref Β,Ζ,Α++,Η," - Delay: "+ƞ((int) ϋ.Ǎ))) ϋ.Ǎ=Γώ(ϋ.Ǎ,Δτ);Β+=" Leave connector 2:\n";if(Ι(ref Β,Ζ,Α++,Η," - "+ΓЏ(ό.Г))) ό.Г=Ȟ(ό.Г);
if(!ό.ΑЃ()) Β+="\n";else if(Ι(ref Β,Ζ,Α++,Η," - Delay: "+ƞ((int) ό.Ǎ))) ό.Ǎ=Γώ(ό.Ǎ,Δτ);}else if(ρ==Ξ.ΔΧ){if(Ι(ref Β,Ζ,Α++,Η," Start job!")) ΒΪ();if(Ι(ref Β,Ζ,Α++,Η," Back")) β(Ξ.ΒΧ);Β+=" Timer: \"Docking connector 1\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϋ.Ǐ!=""?ϋ.Ǐ:"-"))) ϋ.Ǐ=Ʀ(ref Ητ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϋ.ǒ!=""?ϋ.ǒ:"-"))) ϋ.ǒ=Ʀ(ref Ηφ);Β+=" Timer: \"Docking connector 2\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ό.Ǐ!=""?ό.Ǐ:"-"))) ό.Ǐ=Ʀ(ref Ηυ);Β+=" Timer: \"Leaving connector 2\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ό.ǒ!=""?ό.ǒ:"-"))) ό.ǒ=Ʀ(ref Ηχ);
}else if(ρ==Ξ.Ş){String ΔБ=ǩ+" %";if(ȑ) Ȫ++;if(Ȫ>1){Ȫ=0;Ǌ++;if(Ǌ>1) Ǌ=0;}bool[]ΓΫ=new bool[]{Ȯ.Count==0,Ǣ==й.None,ю.Count==0};int Ελ=0;while(true){Ελ++;Đ++;if(Đ>ΓΫ.Length-1) Đ=0;if(Ελ>=ΓΫ.Length) break;if(!ΓΫ[Đ]) break;}bool ȫ=Π==Μ.ΰ;if(!ȫ&&Ț&&Т!=-1&&Ǌ==0)
ΔБ=Т<1000000?Math.Round(Т)+" Kg":Math.Round(Т/1000)+" t";if(Ι(ref Β,Ζ,Α++,Η," Stop!")){τ();β(Ξ.Main);}if(Ι(ref Β,Ζ,Α++,Η," Behavior settings"))
if(!ȫ) β(Ξ.Ɩ);else β(Ξ.Ʈ);if(!ȫ){if(Ι(ref Β,Ζ,Α++,Η," Next hole")) Αε(false);}else if(Ι(ref Β,Ζ,Α++,Η," Undock")) ƍ=true;Β+=ŏ;if(!ȫ) Β+="Progress: "+Math.Round(ŷ,1)+" %\n";Β+="State: "+ΑϏ(ά)+" "+ƈ+"m \n";Β+="Load: "+ΑΗ+" % Max: "+ΔБ+" \n";if(Đ==0) Β+="Uranium: "+(Ȯ.Count==0?"No reactors":Math.Round(Ğ,1)+"Kg "+(Ш==-1?"":" Min: "+Ш+" Kg"))+"\n";
if(Đ==1) Β+="Battery: "+(Ǣ==й.None?ΓЄ(Ǣ):ǵ+"% "+(т==-1||ȫ?"":" Min: "+т+" %"))+"\n";if(Đ==2) Β+="Hydrogen: "+(ю.Count==0?"No tanks":Math.Round(Ȑ,1)+"% "+(у==-1||ȫ?"":" Min: "+у+" %"))+"\n";}else if(ρ==Ξ.Ɩ){String Ѕ="";if(Ι(ref Ѕ,Ζ,Α++,Η," Back")){if(ε==Ρ.А) β(Ξ.Ş);else β(Ξ.Main);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Max load: "+ǩ+"%")) а(ref ǩ,ǩ<=80?-10:-5,π.Ƣ,false);if(Ι(ref Ѕ,Ζ,Α++,Η," Weight limit: "+(Ț?"On":"Off"))) Ț=!Ț;if(Ι(ref Ѕ,Ζ,Α++,Η," Ejection: "+ΕЈ(С))) С=Ȟ(С);if(Ι(ref Ѕ,Ζ,Α++,Η," Toggle sorters: "+(Ĝ?"On":"Off"))){Ĝ=!Ĝ;if(Ĝ) ǟ(Γή);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Unload ice: "+(ș?"On":"Off"))) ș=!ș;if(Ι(ref Ѕ,Ζ,Α++,Η," Uranium: "+(Ш==-1?"Ignore":"Min "+Ш+"Kg"))) а(ref Ш,(Ш>5?-5:-1),π.ƣ,true);if(Ι(ref Ѕ,Ζ,Α++,Η," Battery: "+(т==-1?"Ignore":"Min "+т+"%"))) а(ref т,-5,π.ΑЎ,true);if(Ι(ref Ѕ,Ζ,Α++,Η," Hydrogen: "+(у==-1?"Ignore":"Min "+у+"%"))) а(ref у,-10,π.ΑЏ,true);
if(Ι(ref Ѕ,Ζ,Α++,Η," When done: "+(Ȱ?"Return home":"Stop"))) Ȱ=!Ȱ;if(Ι(ref Ѕ,Ζ,Α++,Η," On damage: "+ΓІ(ж))) ж=Ȟ(ж);if(Ι(ref Ѕ,Ζ,Α++,Η," Advanced...")) β(Ξ.Αζ);Β+=ΒΡ(8,Ѕ,Ζ,ref Ηα);}else if(ρ==Ξ.Αζ){if(Ι(ref Β,Ζ,Α++,Η," Back")){if(ε==Ρ.А) β(Ξ.Ş);else β(Ξ.Main);}
if(Ι(ref Β,Ζ,Α++,Η,(Π==Μ.ћ?" Grinder":" Drill")+" inv. balancing: "+(Ļ?"On":"Off"))) Ļ=!Ļ;if(Ι(ref Β,Ζ,Α++,Η," Enable"+(Π==Μ.ћ?" grinders":" drills")+": "+(ĵ?"Fwd + Bwd":"Fwd"))) ĵ=!ĵ;if(Ι(ref Β,Ζ,Α++,Η," Work speed fwd.: "+Ŕ+"m/s")) а(ref Ŕ,0.5f,π.ť,false);
if(Ι(ref Β,Ζ,Α++,Η," Work speed bwd.: "+œ+"m/s")) а(ref œ,0.5f,π.ť,false);if(Ι(ref Β,Ζ,Α++,Η," Acceleration: "+Math.Round(ǡ*100f)+"%"+(ǡ>0.80f?" (risky)":""))) а(ref ǡ,0.1f,π.Ɣ,false);if(Ι(ref Β,Ζ,Α++,Η," Width overlap: "+Ɠ*100f+"%")) Γϋ(true,0.05f);if(Ι(ref Β,Ζ,Α++,Η," Height overlap: "+Ĺ*100f+"%")) Γϋ(false,0.05f);
}else if(ρ==Ξ.Ʈ){if(Ι(ref Β,Ζ,Α++,Η," Back")){if(ε==Ρ.А) β(Ξ.Ş);else β(Ξ.Main);}if(Ι(ref Β,Ζ,Α++,Η," Max load: "+ǩ+"%")) а(ref ǩ,ǩ<=80?-10:-5,π.Ƣ,false);if(Ι(ref Β,Ζ,Α++,Η," Unload ice: "+(ș?"On":"Off"))) ș=!ș;if(Ι(ref Β,Ζ,Α++,Η," Uranium: "+(Ш==-1?"Ignore":"Min "+Ш+"Kg"))) а(ref Ш,(Ш>5?-5:-1),π.ƣ,true);
if(Ι(ref Β,Ζ,Α++,Η," Battery: "+(т==-1?"Ignore":"Charge up"))) т=(т==-1?1:-1);if(Ι(ref Β,Ζ,Α++,Η," Hydrogen: "+(у==-1?"Ignore":"Fill up"))) у=(у==-1?1:-1);if(Ι(ref Β,Ζ,Α++,Η," On damage: "+ΓІ(ж))) ж=Ȟ(ж);if(Ι(ref Β,Ζ,Α++,Η," Acceleration: "+Math.Round(ǡ*100f)+"%"+(ǡ>0.80f?" (risky)":""))) а(ref ǡ,0.1f,π.Ɣ,false);
}else if(ρ==Ξ.ΔΠ){double ΗΘ=0;if(Φ.Count>0) ΗΘ=Vector3.Distance(Φ.Last().Ε,μ);if(Ι(ref Β,Ζ,Α++,Η," Stop path recording")) ΔΪ();if(Π!=Μ.ΰ){if(Ι(ref Β,Ζ,Α++,Η," Home: "+(Ȃ?"Use old home":(Ν.δ?"Was set! ":"none ")))) Ȃ=!Ȃ;}else{if(Ι(ref Β,Ζ,Α++,Η," Connector 1: "+(Ȃ?"Use old connector":(Ν.δ?"Was set! ":"none ")))) Ȃ=!Ȃ;
if(Ι(ref Β,Ζ,Α++,Η," Connector 2: "+(Ő?"Use old connector":(Θ.δ?"Was set! ":"none ")))) Ő=!Ő;}if(Ι(ref Β,Ζ,Α++,Η," Path: "+(ő?"Use old path":(Φ.Count>1?"Count: "+Φ.Count:"none ")))) ő=!ő;Β+=ŏ;Β+="Wp spacing: "+Math.Round(Ες)+"m\n";}else if(ρ==Ξ.Βά){if(Ι(ref Β,Ζ,Α++,Η," Stop")){τ();β(Ξ.Main);}
Β+=ŏ;Β+="State: "+ΑϏ(ά)+" \n";Β+="Speed: "+Math.Round(ě,1)+"m/s\n";Β+="Target dist: "+ƈ+"m\n";Β+="Wp count: "+Φ.Count+"\n";Β+="Wp left: "+Γρ+"\n";}else if(ρ==Ξ.ΓБ){List<IMyTerminalBlock>č=ΓЇ();if(ȑ) Ȫ++;if(Ȫ>=č.Count) Ȫ=0;if(Ι(ref Β,Ζ,Α++,Η," Next")) β(Ξ.ΔΑ);
Β+=ŏ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΗ,1)+"% "+Math.Round(Ǥ,1)+" / "+Math.Round(Ǫ,1)+"\n";Β+="Uranium: "+(Ȯ.Count==0?"No reactors":Math.Round(Ğ,1)+"Kg "+ΓΓ)+"\n";Β+="Battery: "+(Ǣ==й.None?"":ǵ+"% ")+ΓЄ(Ǣ)+"\n";Β+="Hydrogen: "+(ю.Count==0?"No tanks":Math.Round(Ȑ,1)+"% ")+"\n";
Β+="Damage: "+(č.Count==0?"None":""+(Ȫ+1)+"/"+č.Count+" "+č[Ȫ].CustomName)+"\n";}else if(ρ==Ξ.ΔΑ){if(Ι(ref Β,Ζ,Α++,Η," Back")) β(Ξ.Main);Β+=ŏ;Β+="Next scan: "+ƀ+"s\n";Β+="Ship size: "+Math.Round(Ș,1)+"m "+Math.Round(ȁ,1)+"m "+Math.Round(ψ,1)+"m \n";
Β+="Broadcast: "+(ĳ?"Online - "+Ō:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(ŀ*100f,1)+"% \n";}else if(ρ==Ξ.ΓА){if(Ι(ref Β,Ζ,Α++,Η," Back")) β(Ξ.Main);Β+=ŏ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Є==2) Β="Fatal setup error\nNext scan: "+ƀ+"s\n";if(ΑΨ) Β="Recompile script now";int ΗΙ=Β.Split('\n').Length;for(int i=ΗΙ;i<=10;i++) Β+="\n";Β+=ŋ;Β+="Last: "+Λ+"\n";
return Β;}void Γϋ(bool ΗΔ,float є){τ();Ʋ(true,false);if(ΗΔ) а(ref Ɠ,є,π.ΒΙ,false);else а(ref Ĺ,є,π.ΒΙ,false);Γω();Ĉ(true,true,0,0);}float Γώ(float η,float[]Αγ){float Ω=Αγ[0];for(int i=Αγ.Length-1;i>=0;i--)
if(η<Αγ[i]) Ω=Αγ[i];return Ω;}String Ʀ(ref int ȩ){String Ω="";if(ȩ>=Γη.Count) ȩ=-1;if(ȩ>=0) Ω=Γη[ȩ].CustomName;ȩ++;return Ω;}void ư(string Ϊ){if(ε!=Ρ.А) return;if(Ϊ=="") return;IMyTerminalBlock Υ=ω.GetBlockWithName(Ϊ);if(Υ==null||!(Υ is IMyTimerBlock)){Λ="Timerblock "+Ϊ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void Ģ(ref int value,int ū,int ȗ,int Γά){if(ū==0) return;if(value<ȗ&&Γά>0||value<=ȗ&&Γά<0){value+=Γά;return;}int Ζη=Math.Abs(ū);int Βγ=0;int ΓΝ=1;while(true){Βγ+=ΓΝ*Ζη*10;if(ū<0&&value-ȗ<=Βγ) break;if(ū>0&&value-ȗ<Βγ) break;
ΓΝ++;}value+=ΓΝ*ū;}void ĉ(bool ΗА){ь=Math.Max(ь,1);ы=Math.Max(ы,1);д=Math.Max(д,0);Ĉ(ρ==Ξ.ş,false,0,0);}bool Ą(ref float value,bool Ηά,π ν,String đ,String ΖБ){if(đ=="") return false;float ń=-1;bool ΔЅ=false;if(đ.ToUpper()==ΖБ) ΔЅ=true;else if(!float.TryParse(đ,out ń)) return false;
else ń=Math.Max(0,ń);if(Ηά) ń=(float) Math.Round(ń);а(ref value,ń,ν,ΔЅ,false);return true;}void а(ref float value,float є,π ν,bool ƌ){а(ref value,value+є,ν,ƌ,true);}void а(ref float value,float ł,π ν,bool ƌ,bool ΗЈ){float ǔ=0,Ǔ=0;if(ν==π.ť){ǔ=0.5f;Ǔ=10f;}if(ν==π.Ɣ){ǔ=0.1f;Ǔ=1f;}
if(ν==π.ś){ǔ=50f;Ǔ=100f;}if(ν==π.ΑЎ){ǔ=5f;Ǔ=30f;}if(ν==π.ƣ){ǔ=1f;Ǔ=25f;}if(ν==π.Ƣ){ǔ=10f;Ǔ=95f;}if(ν==π.ΑЏ){ǔ=10f;Ǔ=90f;}if(ν==π.ΖΩ){ǔ=10f;Ǔ=1800;}if(ν==π.ΒΙ){ǔ=0.0f;Ǔ=0.75f;}if(ł==-1&&ƌ){value=-1;return;}if(value==-1) ƌ=false;
bool ΕΡ=ł<ǔ||ł>Ǔ;if(ΕΡ&&ΗЈ){if(ł<value) value=Ǔ;else if(ł>value) value=ǔ;}else value=ł;if(ΕΡ&&ƌ) value=-1;else value=Math.Max(ǔ,Math.Min(value,Ǔ));value=(float) Math.Round(value,2);}void Αε(bool ΗΨ){if(ΗΨ) р=Math.Max(0,р-1);else р++;Βΰ(true);}T Ȟ<T>(T η){int Α=Array.IndexOf(Enum.GetValues(η.GetType()),η);
Α++;if(Α>=ΕЂ(η)) Α=0;return(T) Enum.GetValues(η.GetType()).GetValue(Α);}int ΕЂ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒΫ(){ƒ.Clear();for(int i=0;i<Φ.Count;i++) ƒ.Add(Φ[i]);Φ.Clear();ΑΒ=true;ΒІ=new έ(Ν);ΒЉ=new έ(Θ);Ν.δ=false;if(Π==Μ.ΰ) Θ.δ=false;for(int i=0;i<ѕ.Count;i++)
if(!ǿ.Contains(ѕ.Keys.ElementAt(i)))
ǿ.Add(ѕ.Keys.ElementAt(i));Ȃ=false;Ő=false;ő=false;β(Ξ.ΔΠ);}void ΔΪ(){if(Ȃ) Ν=ΒІ;if(Ő) Θ=ΒЉ;if(ő){Φ.Clear();for(int i=0;i<ƒ.Count;i++) Φ.Add(ƒ[i]);}ΑΒ=false;τ();β(Ξ.Main);}void Ζβ(){if(!ΑΒ) return;if(ά!=Γ.л){ΔΪ();return;}if(!ΒІ.δ) Ȃ=false;if(!ΒЉ.δ) Ő=false;if(ƒ.Count<=1) ő=false;
IMyShipConnector џ=в(MyShipConnectorStatus.Connectable);if(џ==null) џ=в(MyShipConnectorStatus.Connected);if(џ!=null){if(Math.Round(ě,2)<=0.20) Βω++;else Βω=0;if(Βω>=5){if(Π==Μ.ΰ&&(Ν.δ||Ȃ)&&Vector3.Distance(Ν.Ε,џ.GetPosition())>5){Θ.α=Ψ.WorldMatrix.Forward;Θ.ϊ=Ψ.WorldMatrix.Left;
Θ.γ=Ψ.WorldMatrix.Down;Θ.χ=Ψ.GetNaturalGravity();Θ.Ε=џ.GetPosition();Θ.δ=true;Θ.о=џ.Position;}else{Ν.α=Ψ.WorldMatrix.Forward;Ν.ϊ=Ψ.WorldMatrix.Left;Ν.γ=Ψ.WorldMatrix.Down;Ν.χ=Ψ.GetNaturalGravity();Ν.Ε=џ.GetPosition();Ν.δ=true;Ν.о=џ.Position;}}}double ΒЊ=-1;if(Φ.Count>0) ΒЊ=Vector3.Distance(μ,Φ.Last().Ε);
double Ηθ=Math.Max(1.5,Math.Pow(ě/100.0,2));double ΕΙ=Math.Max(ě*Ηθ,2);Ες=ΕΙ;if((ΒЊ==-1)||ΒЊ>=ΕΙ){έ Ϋ=new έ(μ,Ǉ,ǃ,ΑΖ,Ψ.GetNaturalGravity());Ϋ.Ευ(Ȁ,ǿ);Φ.Add(Ϋ);}}έ Ȅ(){if(Π!=Μ.ΰ) return Ν;return Δο;}void Ī(έ ή,Ρ φ){Δο=ή;if(ε==Ρ.А) я=φ;}bool Ǳ(έ ή,float Ћ,bool ΗϏ,out Vector3 И)
{if(ΗϏ){Vector3I ΒЁ=new Vector3I((int) ή.о.X,(int) ή.о.Y,(int) ή.о.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(ΒЁ);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){И=new Vector3();return false;}Vector3 Ζχ=Ы(Ψ,Υ.FatBlock.GetPosition()-μ);Vector3 Ζφ=Ы(Ψ,Υ.FatBlock.WorldMatrix.Forward);И=ή.Ε-Βί(ή.α,ή.γ*-1,Ζχ)-Βί(ή.α,ή.γ*-1,Ζφ)*Ћ;
return true;}else{И=ή.Ε;return true;}}Vector3 ȅ(int Ζτ,int Ηέ,bool ΗΪ){if(!ΗΪ&&ΔЀ) return Βς;float Ζυ=((Ч-1f)/2f)-Ζτ;float Ηή=((с-1f)/2f)-Ηέ;Βς=Θ.Ε+Θ.ϊ*Ζυ*Ș+ĕ*-1*Ηή*ȁ;ΔЀ=true;return Βς;}Vector3 Αώ(Vector3 Ην,float Ζώ){return Ην+(ǽ*Ζώ);}ί Ατ(){float М=-1;
ί Ω=ί.ǯ;if(Π!=Μ.ΰ){if(ε!=Ρ.м){Vector3 Ў=ć(ǽ,ĕ*-1,μ-Θ.Ε);if(Math.Abs(Ў.X)<=(float)(Ч*Ș)/2f&&Math.Abs(Ў.Y)<=(float)(с*ȁ)/2f){if(Ў.Z<=-1&&Ў.Z>=-ę*2) return ί.ƕ;if(Ў.Z>-1&&Ў.Z<ψ*2) return ί.ǯ;}if(Ȝ(Θ.Ε,ref М)) Ω=ί.ǯ;}if(Ν.δ){if(Ȝ(Ν.Ε,ref М)) Ω=ί.Ǯ;
for(int i=0;i<Φ.Count;i++)
if(Ȝ(Φ[i].Ε,ref М)) Ω=ί.Ȇ;if(Vector3.Distance(μ,Ν.Ε)<dockDist*ψ) Ω=ί.Ǯ;if(в(MyShipConnectorStatus.Connectable)!=null||в(MyShipConnectorStatus.Connected)!=null)
Ω=ί.Ǯ;}}else{Vector3 И=new Vector3();IMyShipConnector Ƿ=в(MyShipConnectorStatus.Connected);if(Ν.δ){if(Ȝ(Ν.Ε,ref М)) Ω=ί.Ǯ;if(Ǳ(Ν,dockDist,true,out И))
if(Ȝ(И,ref М)) Ω=ί.Ǯ;if(Ƿ!=null&&Vector3.Distance(Ƿ.GetPosition(),Ν.Ε)<5)
return ί.Αι;}for(int i=0;i<Φ.Count;i++)
if(Vector3.Distance(Φ[i].Ε,Ν.Ε)>dockDist*ψ&&Vector3.Distance(Φ[i].Ε,Θ.Ε)>dockDist*ψ)
if(Ȝ(Φ[i].Ε,ref М)) Ω=ί.Ȇ;if(Θ.δ){if(Ȝ(Θ.Ε,ref М)) Ω=ί.ǯ;if(Ǳ(Θ,dockDist,true,out И))
if(Ȝ(И,ref М)) Ω=ί.ǯ;if(Ƿ!=null&&Vector3.Distance(Ƿ.GetPosition(),Θ.Ε)<5)
return ί.Ακ;}}return Ω;}bool Ȝ(Vector3 И,ref float М){float Ћ=Vector3.Distance(И,μ);if(Ћ<М||М==-1){М=Ћ;return true;}return false;}void ΒΪ(){if(Є>0){Λ="Setup error! Can't start";return;}if(Π==Μ.ΰ){ȝ();return;}Θ.Ε=μ;Θ.χ=Ψ.GetNaturalGravity();Θ.α=Ǉ;Θ.γ=ǃ;Θ.ϊ=ΑΖ;
ǽ=ё.WorldMatrix.Forward;ĕ=Θ.γ;if(ǽ==Ψ.WorldMatrix.Down) ĕ=Ψ.WorldMatrix.Backward;Ʋ(true,true);Σ(Γ.ă);ΒΩ();}void Ʋ(bool ǐ,bool ΗΫ){if(ε==Ρ.м&&!ǐ) return;bool ΗΟ=ǐ||ε==Ρ.Αυ||Ч!=ь||с!=ы||ΑΕ!=Ȗ;if(ΗΟ){if(ε!=Ρ.м){ε=Ρ.Ƙ;ȅ(υ,σ,ΗΫ);Λ="Job changed, lost progress";}ΑΕ=Ȗ;
Ч=ь;с=ы;σ=0;υ=0;Ł=0;Ȭ=0;Ф=0;р=0;Βΰ(true);}}void ΒΜ(){н(μ,0);ъ(Ȁ,true);}void ΒΩ(){ǳ(Ь.ƫ);ΒΜ();ţ(Ǽ,false);ǟ(Γή);ε=Ρ.А;Γψ(true);я=ε;β(Ξ.Ş);ΖΝ();ǀ=true;ǋ=0;for(int i=Ŧ.Count-1;i>=0;i--)
if(Ŝ(Ŧ[i],false)) ǋ++;if(ǋ>0) Λ="Started with damage";}void τ(){if(ε==Ρ.А){ε=Ρ.ΒΛ;Λ="Job paused";}Σ(Γ.л);я=ε;ƭ(false,0,0,0,0);Βέ();Γτ(new Vector3(),false);ΔΩ();ΒΥ(ChargeMode.Auto);ΒΦ(false);ΔΣ(true);ȇ(Γ.л);Ĉ(false,false,0,0);ъ(ο,false);ъ(п,true);ǟ(true);ΑΩ=false;ǀ=false;
ƃ=false;ƍ=false;if(ρ!=Ξ.Main&&ρ!=Ξ.Ɩ&&ρ!=Ξ.Αζ&&ρ!=Ξ.Ʈ)
β(Ξ.Main);}void ȝ(){ί Н=Ατ();if(Π==Μ.ΰ){if(!Θ.δ||!Ν.δ) return;ΒΩ();bool ΒϏ=Vector3.Distance(μ,Ν.Ε)<Vector3.Distance(μ,Θ.Ε);if(ŉ==Ρ.Ġ) ΒϏ=true;if(ŉ==Ρ.ġ) ΒϏ=false;if(ΒϏ){Ī(Ν,Ρ.Ġ);switch(Н){case ί.Αι:Σ(Γ.Ȣ);break;case ί.Ȇ:Σ(Γ.г);break;case ί.Ǯ:Σ(Γ.ț);break;default:Σ(Γ.Ƞ);break;
}}else{Ī(Θ,Ρ.ġ);switch(Н){case ί.Ακ:Σ(Γ.Ȣ);break;case ί.ǯ:Σ(Γ.ț);break;case ί.Ȇ:Σ(Γ.г);break;default:Σ(Γ.Ƞ);break;}}}else{if(ε!=Ρ.ΒΛ&&ε!=Ρ.Ƙ) return;bool ΗЁ=ε==Ρ.Ƙ;ΒΩ();bool ΒЄ=ŝ(false)&&Ν.δ;switch(Н){case ί.ǯ:Σ(ΒЄ?Γ.Ă:Γ.ă);break;case ί.Ȇ:Σ(ΒЄ?Γ.ǝ:Γ.г);break;case ί.Ǯ:Σ(ΒЄ?Γ.ț:Γ.ȉ);break;
case ί.ƕ:{if(Ȭ!=р||ΗЁ) Σ(Γ.ȟ);else Σ(Γ.š);}break;default:break;}}}void ā(){if(ε==Ρ.м&&!Ν.δ) return;if(Π==Μ.ΰ&&(!Θ.δ||!Ν.δ)) return;Λ="Move to job";ί Н=Ατ();if(Π==Μ.ΰ){Ī(Θ,Ρ.ġ);switch(Н){case ί.ǯ:Σ(Γ.ț);break;case ί.Ȇ:Σ(Γ.г);break;case ί.Ακ:return;default:Σ(Γ.Ƞ);break;
}ȇ(Γ.Ȣ);}else{switch(Н){case ί.ǯ:Σ(Γ.г);break;case ί.Ȇ:Σ(Γ.г);break;case ί.Ǯ:Σ(Γ.ȉ);break;case ί.ƕ:Σ(Γ.ȟ);break;default:break;}if(ε==Ρ.м) ȇ(Γ.г);else ȇ(Γ.Ȳ);ΑΩ=true;}ΒΜ();β(Ξ.Βά);ţ(Ǽ,false);я=Ρ.ƥ;}void Ɲ(){if(!Ν.δ) return;Λ="Move home";ί Н=Ατ();if(Π==Μ.ΰ){Ī(Ν,Ρ.Ġ);switch(Н)
{case ί.Ȇ:Σ(Γ.ǝ);break;case ί.Ǯ:Σ(Γ.ț);break;case ί.Αι:return;default:Σ(Γ.Ƞ);break;}ȇ(Γ.Ȣ);}else{if(в(MyShipConnectorStatus.Connected)!=null) return;if(в(MyShipConnectorStatus.Connectable)!=null){Σ(Γ.Ř);ȇ(Γ.ȉ);return;}switch(Н){case ί.ǯ:Σ(Γ.Ă);break;case ί.Ȇ:Σ(Γ.ǝ);break;case ί.Ǯ:Σ(Γ.ǝ);break;
case ί.ƕ:Σ(Γ.ą);break;default:break;}ȇ(Γ.ȉ);}ΒΜ();β(Ξ.Βά);ţ(Ǽ,false);я=Ρ.Ƥ;}void Σ(Γ ΓΞ){if(ΓΞ==Γ.л) ƅ=Γ.л;if(ƅ!=Γ.л&&ά==ƅ&&ΓΞ!=ƅ){τ();return;}Џ=true;ά=ΓΞ;}void ȇ(Γ φ){this.ƅ=φ;}Ȉ ΕЌ(int ŵ,bool Ǘ){if(Ǘ){ѓ=null;υ=0;σ=0;}if(Ȗ==Ǡ.Ư){int ΕΜ=ŵ+1;σ=(int) Math.Floor(І(ŵ,Ч));
if(σ%2==0) υ=ŵ-(σ*Ч);else υ=Ч-1-(ŵ-(σ*Ч));if(σ>=с) return Ȉ.Αθ;else return Ȉ.Αϋ;}else if(Ȗ==Ǡ.Γϊ){if(ѓ==null) ѓ=new int[]{0,-1,0,0};int ΔЃ=(int) Math.Ceiling(Ч/2f);int ΔЂ=(int) Math.Ceiling(с/2f);int ΖЌ=(int) Math.Floor(Ч/2f);int ΖЋ=(int) Math.Floor(с/2f);
int Ŷ=0;while(ѓ[2]<Math.Pow(Math.Max(Ч,с),2)){if(Ŷ>200) return Ȉ.ΕЃ;Ŷ++;ѓ[2]++;if(-ΔЃ<υ&&υ<=ΖЌ&&-ΔЂ<σ&&σ<=ΖЋ){if(ѓ[3]==ŵ){this.υ=υ-1+ΔЃ;this.σ=σ-1+ΔЂ;return Ȉ.Αϋ;}ѓ[3]++;}if(υ==σ||(υ<0&&υ==-σ)||(υ>0&&υ==1-σ)){int Ηπ=ѓ[0];ѓ[0]=-ѓ[1];ѓ[1]=Ηπ;
}υ+=ѓ[0];σ+=ѓ[1];}}return Ȉ.Αθ;}void Ζα(){if(ά==Γ.ă){if(Џ){Љ=0;if(Ȭ!=р) Ł=0;Ȭ=р;}if(Љ==0){Ȉ Ω=ΕЌ(р,Џ);if(Ω==Ȉ.Αθ){ε=Ρ.Αυ;Λ="Job done";if(Ȱ&&Ν.δ){Σ(Γ.Ă);ȇ(Γ.ȉ);я=Ρ.Ƥ;}else{Σ(Γ.ř);ȇ(Γ.Ȳ);я=Ρ.ƥ;}return;}if(Ω==Ȉ.Αϋ){Љ=1;ъ(ο,true);ф=ȅ(υ,σ,true);н(ф,10);
Ъ(Θ.γ,Θ.α,Θ.ϊ,false);}}else{if(Р<wpReachedDist){Σ(Γ.š);return;}}}if(ά==Γ.š){if(Џ){ъ(ο,true);ǟ(false);ф=ȅ(υ,σ,false);н(Αώ(ф,0),0);Ъ(Θ.γ,Θ.α,Θ.ϊ,false);Љ=1;е=0;Щ=0;Ƹ=0;ΒЌ=-1;ę=д;ΒЇ=true;}if(!Απ()){Σ(Γ.ą);return;}if(ŝ(true)){ΕΠ=ȃ("","ORE",Й.ȳ);if((С==Ђ.Αφ||С==Ђ.ƙ||
С==Ђ.ΑЅ||С==Ђ.Ơ)&&Π!=Μ.ћ)
Σ(Γ.Ɯ);else if((С==Ђ.Γύ||С==Ђ.Αρ)&&Π!=Μ.ћ)
Σ(Γ.Βα);else
Σ(Γ.ą);return;}Ф=Vector3.Distance(μ,ф);if(Ф>Ł){Ł=Ф;ΒЇ=false;}if(Π==Μ.ћ&&ΓЈ()==MyDetectedEntityType.SmallGrid)
Щ+=2;else Щ-=2;Щ=Math.Max(100,Math.Min(400,Щ));if(Љ>0&&Љ<Щ){if(Ф>е){if(Щ>150) е=Ф;else е=(float) Math.Ceiling(Ф);Љ=1;}else Љ++;}else{if(Љ>0){Λ="Ship stuck! Retrying";е=Ф;Љ=0;Ĉ(false,true,0,ψ*sensorRange);}н(Αώ(ф,Math.Max(0,е-ψ)),ΑЁ(false));if(Р<=wpReachedDist/2){Љ=1;е=0;}
return;}Ĉ(false,true,ψ*sensorRange,0);Vector3 Ζι=ф+ǽ*Ф;bool ΔЈ=false;if(Vector3.Distance(Ζι,μ)>0.3f){Vector3 Ζϋ=ф+ǽ*(Ф+0.1f);н(Ζϋ,4);ΔЈ=true;}else{float ǘ=ΑЁ(true);Vector3 Δσ=Αώ(ф,Math.Max(д+1,Ф+1));Ǟ(true,false,false,Δσ,Δσ-ф,ǘ,ǘ);}bool Βχ=false;if(ǎ==љ.Αμ||ǎ==љ.Αλ)
{if(!ΔЈ){float ΓΟ=0;foreach(IMyTerminalBlock ǭ in ο)
ΓΟ+=ΕϏ(ǭ,"","",ǎ==љ.Αλ?new string[]{"STONE"}:null);if(ΓΟ>ΒЌ||Ф<д||ΒЇ){Βι=0;Ƹ=Ф;ę=(int)(Math.Max(ę,Ƹ)+ψ/2);}else{Βχ=Ф-Ƹ>2&&Βι>=20;Βι++;}ΒЌ=ΓΟ;}}else Βχ=Ф>=ę;if(Ȭ!=р){Σ(Γ.ȟ);Ф=0;return;}if(Βχ){р++;Σ(Γ.ȟ);Ф=0;return;}}if(ά==Γ.ȟ||ά==Γ.ą||ά==Γ.Ɯ)
{if(Џ){ф=ȅ(υ,σ,false);Ъ(Θ.γ,Θ.α,Θ.ϊ,false);ъ(ο,ĵ);ǟ(false);е=Vector3.Distance(μ,ф);Ĉ(false,true,0,ψ*sensorRange);}н(ф,ΑЁ(false));if(Vector3.Distance(μ,ф)>=е+5){ъ(ο,false);ǟ(true);Λ="Can´t return!";}if(Р<wpReachedDist){if(ά==Γ.ȟ&&ΑΩ) Σ(Γ.ř);if(ά==Γ.ȟ) Σ(Γ.ă);
if(ά==Γ.Ɯ) Σ(Γ.Αω);if(ά==Γ.ą){if(Ν.δ) Σ(Γ.Ă);else{τ();ā();Λ="Can´t return, no dock found";}}return;}}if(ά==Γ.ǝ||ά==Γ.г){if(ά==Γ.г&&ε==Ρ.А&&Π!=Μ.ΰ){if(!Απ()||ŝ(true)){Σ(Γ.ǝ);return;}}bool ΓΦ=false;bool Γν=false;bool Δγ=false;float İ=0;bool ΕΟ=false;έ Ϋ=null;
if(Џ){if(ά==Γ.ǝ||Π==Μ.ΰ){έ ή=Ȅ();θ=new ΒΚ();θ.ι=ή;θ.Ů=followPathDock*ψ;θ.Γξ=useDockDirectionDist*ψ;θ.İ=10;θ.Ŏ.Add(ή.Ε);Vector3 į=new Vector3();if(Ǳ(ή,dockDist*ψ,true,out į))
θ.Ŏ.Add(į);else θ.Ů*=1.5f;if(Π==Μ.ΰ){if(ή==Ν) θ.Ǆ=Θ.Ε;if(ή==Θ) θ.Ǆ=Ν.Ε;θ.Βό=dockDist*ψ*1.1f;}}else if(ά==Γ.г){θ=new ΒΚ();θ.ι=Θ;θ.Ů=followPathJob*ψ;θ.Γξ=useJobDirectionDist*ψ;θ.İ=10;θ.Ǆ=Ν.Ε;θ.Βό=dockDist*ψ*1.1f;θ.Ŏ.Add(Θ.Ε);if(ε==Ρ.м){if(!Ν.δ||Φ.Count==0){τ();return;}
float ΖЂ=Vector3.Distance(Φ.First().Ε,Ν.Ε);float ΖЃ=Vector3.Distance(Φ.Last().Ε,Ν.Ε);if(ΖЂ<ΖЃ) θ.ι=Φ.Last();else θ.ι=Φ.First();}}ž=new Vector3();ΕΟ=!ΖΔ(μ);ъ(ο,false);ǟ(true);ǖ=-1;double ǣ=-1;for(int i=Φ.Count-1;i>=0;i--){if(Vector3.Distance(Φ[i].Ε,θ.Ǆ)<=θ.Βό) continue;
double Ћ=Vector3.Distance(Φ[i].Ε,μ);if(ǣ==-1||Ћ<ǣ){ǖ=i;ǣ=Ћ;}}ė=ΕЍ(θ.ι.Ε,ǖ);Č=null;}Εφ(Φ,ė,θ.Ŏ,θ.Ů,Џ,ref Љ);for(int i=0;i<θ.Ŏ.Count;i++){float Ћ=Vector3.Distance(μ,θ.Ŏ[i]);if(Ћ<=θ.Ů) ΓΦ=true;if(Ћ<=θ.Γξ) Γν=true;}if(Γν) İ=θ.İ;float ΗБ=Č!=null?Č.Х:ě;
float ΗЇ=(float) Math.Max(ě*0.1f*ψ,wpReachedDist);if((Р<ΗЇ)||Џ){if(!Џ) ǖ+=ė;if(ė==0||ǖ>Φ.Count-1||ǖ<0)
ΓΦ=true;else{Γρ=ė>0?Φ.Count-1-ǖ:ǖ;Ϋ=Φ[ǖ];Č=Ϋ;if(ǖ>=1&&ǖ<Φ.Count-1)
ž=Ϋ.Ε-Φ[ǖ-ė].Ε;else Č=null;Ѓ=Ϋ.Ε;Δγ=true;}}if(Γν)
Ъ(θ.ι.γ,θ.ι.α,θ.ι.ϊ,false);else if(ΕΟ)
Αη(θ.ι.γ,10,true);else if(Δγ&&Ϋ!=null){if(ė>0) Ъ(Ϋ.γ,Ϋ.α,Ϋ.ϊ,90,false);else Ъ(Ϋ.γ,-Ϋ.α,-Ϋ.ϊ,90,false);}Ǟ(true,false,true,Ѓ,ž,Č==null?0:Č.Х,İ);if(ΓΦ){Γρ=0;if(ά==Γ.ǝ||Π==Μ.ΰ){Σ(Γ.ț);return;}if(ά==Γ.г&&ΑΩ){Σ(Γ.ř);return;}if(ά==Γ.г){Σ(Γ.ă);return;}
}}if(ά==Γ.ț||ά==Γ.ΒΠ){έ ή=Ȅ();if(Џ){if(!Ǳ(ή,dockDist*ψ,true,out Ѓ)){ǳ(Ь.ŗ);τ();return;}н(Ѓ,0);Αη(ή.γ,90,true);}if(Р<followPathDock*ψ&&Р!=-1){н(Ѓ,10);Ъ(ή.γ,ή.α,ή.ϊ,false);}if(в(MyShipConnectorStatus.Connectable)!=null||в(MyShipConnectorStatus.Connected)!=null)
{Σ(Γ.Ř);return;}if(Р<wpReachedDist/2&&Р!=-1){Σ(Γ.ŕ);return;}}if(ά==Γ.ŕ||ά==Γ.Ȳ){if(Џ){if(ά==Γ.ŕ){έ ή=Ȅ();if(!Ǳ(ή,dockDist*ψ,true,out Ѓ)){ǳ(Ь.ŗ);τ();return;}Ǟ(true,true,false,Ѓ,0);Ъ(ή.γ,ή.α,ή.ϊ,10,false);}if(ά==Γ.Ȳ){Ъ(Θ.γ,Θ.α,Θ.ϊ,0.5f,false);Ѓ=Θ.Ε;
Ǟ(true,true,false,Ѓ,0);}}if(ΑΑ){ƭ(false,0,0,0,0);if(ά==Γ.ŕ) Σ(Γ.Ř);if(ά==Γ.Ȳ) τ();return;}}if(ά==Γ.Ř){if(в(MyShipConnectorStatus.Connected)!=null){if(Π==Μ.ΰ) Σ(Γ.Ȣ);else Σ(Γ.ȉ);return;}έ ή=Ȅ();if(Џ){Щ=0;ΖЄ=DateTime.Now;Љ=0;Ъ(ή.γ,ή.α,ή.ϊ,false);}Vector3I ΒЁ=new Vector3I((int) ή.о.X,(int) ή.о.Y,(int) ή.о.Z);
IMySlimBlock Δμ=Me.CubeGrid.GetCubeBlock(ΒЁ);float Δψ=dockingSpeed;float ΗΜ=dockingSpeed*5;float Δα=Math.Max(1.5f,Math.Min(5f,ψ*0.15f));if(!Ǳ(ή,0,true,out Ѓ)||!Ǳ(ή,Δα,true,out ž)||
Δμ==null||!Δμ.FatBlock.IsFunctional){ǳ(Ь.ŗ);τ();return;}if(Щ==1||(Vector3.Distance(μ,Ѓ)<=Δα*1.1f&&!Џ))
Щ=1;else{Vector3 Ηψ=Ы(Ψ,ž-μ);Vector3 ΒЀ=Ы(Ψ,Ψ.GetNaturalGravity());float Ηί=Ɨ(Ηψ,ΒЀ,null);Δψ=Math.Min(ΗΜ,Ηί);}Ǟ(true,false,false,Ѓ,Ѓ-μ,dockingSpeed,Δψ);if(Џ) е=(float) Р;IMyShipConnector Βτ=в(MyShipConnectorStatus.Connectable);if(Βτ!=null){Ǟ(false,false,false,Ѓ,0);
if(Љ>0) Љ=0;Љ--;if(Љ<-5){Βτ.Connect();if(Βτ.Status==MyShipConnectorStatus.Connected){if(Π==Μ.ΰ) Σ(Γ.Ȣ);else Σ(Γ.ȉ);Βέ();ţ(Ǽ,true);return;}}}else{float Δφ=(float) Math.Round(Р,1);if(Δφ<е){Љ=-1;е=Δφ;}else Љ++;if(Љ>20){Σ(Γ.ΒΠ);return;}}}if(ά==Γ.ȉ||ά==Γ.Ȣ||
ά==Γ.Ββ||ά==Γ.Αχ||ά==Γ.ĥ){bool Βη=false,Βθ=false;if(Π==Μ.ΰ){if(Ȅ()==Ν) Βη=true;else if(Ȅ()==Θ) Βθ=true;}if(Џ){ƃ=false;if(в(MyShipConnectorStatus.Connected)==null){Σ(Γ.Ƞ);return;}Βέ();if(Βη) ư(ϋ.Ǐ);if(Βθ) ư(ό.Ǐ);ĝ=false;ȋ=false;ȏ=false;Ə=false;}if(в(MyShipConnectorStatus.Connected)==null){τ();ǳ(Ь.ΔΔ);return;}
if(ε!=Ρ.А||т==-1||Ǣ==й.None) ȋ=true;else if(ǵ>=100f) ȋ=true;else if(ǵ<=99f) ȋ=false;if(ε!=Ρ.А||у==-1||ю.Count==0) ȏ=true;else if(Ȑ>=100f) ȏ=true;else if(Ȑ<=99) ȏ=false;if(ε!=Ρ.А||Ш==-1||Ȯ.Count==0) Ə=true;else Ə=Ğ>=Ш;Ĭ и=null;if(Βη) и=ϋ;
if(Βθ) и=ό;if(и!=null&&(и.Г==ζ.ƨ||и.Г==ζ.Ƨ)) ȋ=true;if(и!=null&&(и.Г==ζ.ΒΒ)) if(!ĝ) ȋ=false;if(и!=null&&(и.Г==ζ.ΒΖ||и.Г==ζ.ΒΔ)) ȏ=true;if(и!=null&&(и.Г==ζ.ΒΕ)) if(!ĝ) ȏ=false;if(ȑ){ChargeMode Δθ=ȋ?ChargeMode.Auto:ChargeMode.Recharge;if(и!=null&&(и.Г==ζ.Ƨ||и.Г==ζ.ƨ))
Δθ=ChargeMode.Discharge;ΒΥ(Δθ);ΒΦ(!ȏ);}if(!ĝ){if(Π==Μ.ΰ)
ĝ=ε!=Ρ.А||Εω(Џ,true)||ƍ;else
ĝ=ε!=Ρ.А||ΔΕ();}else{if(!ȋ) Σ(Γ.ĥ);if(!ȏ) Σ(Γ.Αχ);if(!Ə) Σ(Γ.Ββ);Џ=false;}if(ĝ&&ȋ&&ȏ&&Ə){ΒΥ(ChargeMode.Auto);ΒΦ(false);if(ε==Ρ.А){if(Π==Μ.ΰ){if(Ȅ()==Ν) ư(ϋ.ǒ);else if(Ȅ()==Θ) ư(ό.ǒ);if(Ȅ()==Ν) Ī(Θ,Ρ.ġ);else Ī(Ν,Ρ.Ġ);}}Σ(Γ.Ƞ);return;}}if(ά==Γ.Ƞ){if(Џ){IMyShipConnector Ƿ=в(MyShipConnectorStatus.Connected);
if(Ƿ==null){Σ(Γ.г);return;}IMyShipConnector Ǿ=Ƿ.OtherConnector;ΔΡ(Ƿ,false);ţ(Ǽ,false);έ ή=null;if(Vector3.Distance(Ƿ.GetPosition(),Ν.Ε)<5f&&Ν.δ) ή=Ν;if(Vector3.Distance(Ƿ.GetPosition(),Θ.Ε)<5f&&Θ.δ) ή=Θ;if(ή!=null){if(!Ǳ(ή,dockDist*ψ,true,out Ѓ)){ǳ(Ь.ŗ);τ();return;}
н(Ѓ,5);Ъ(ή.γ,ή.α,ή.ϊ,false);}else н(μ+Ǿ.WorldMatrix.Forward*dockDist*ψ,5);if(ε==Ρ.А) ǳ(Ь.ƫ);}if(Р<wpReachedDist){ъ(п,true);Σ(Γ.г);return;}}if(ά==Γ.ř){if(Џ){ǟ(true);ъ(ο,false);Ѓ=Θ.Ε;н(Ѓ,20);Ъ(Θ.γ,Θ.α,Θ.ϊ,false);}if(Р<wpReachedDist/2){Σ(Γ.Ȳ);return;}}if(ά==Γ.Ă){
if(Џ){ǟ(true);ъ(ο,false);int М=-1;double ǣ=-1;for(int i=Φ.Count-1;i>=0;i--){double Ћ=Vector3.Distance(Φ[i].Ε,μ);if(ǣ==-1||Ћ<ǣ){М=i;ǣ=Ћ;}}if(М==-1){Σ(Γ.ǝ);return;}Ѓ=Φ[М].Ε;н(Ѓ,10);Ъ(Θ.γ,Θ.α,Θ.ϊ,false);}if(Р<wpReachedDist){Σ(Γ.ǝ);return;}}if(ά==Γ.Αω)
{bool ǥ=false;if(Џ){ǟ(true);if((С==Ђ.Αφ||С==Ђ.ƙ)&&ơ()&&
ģ(ǽ,Ψ.GetNaturalGravity())<25&&Ч>=2&&с>=2){Vector3 Ű=μ;if(υ>0&&σ<с-1) Ű=ȅ(υ-1,σ+1,true);else if(υ<Ч-1&&σ<с-1) Ű=ȅ(υ+1,σ+1,true);else if(υ<Ч-1&&σ>0) Ű=ȅ(υ+1,σ-1,true);else if(υ>0&&σ>0) Ű=ȅ(υ-1,σ-1,true);else ǥ=true;if(!ǥ) н(Ű,10);
}else ǥ=true;}if(Р<wpReachedDist/2) ǥ=true;if(ǥ){Σ(Γ.Ƴ);return;}}if(ά==Γ.Βα||ά==Γ.Ƴ){if(Џ){Ǟ(true,true,false,μ,0);ъ(ο,false);ǟ(true);Љ=-1;Щ=С==Ђ.ΑЅ||С==Ђ.Ơ?0:-1;}bool ď=!Απ();int Αή=ȃ("STONE","ORE",Й.ȳ);if(С==Ђ.Αρ||С==Ђ.Ơ||С==Ђ.ƙ)
Αή+=ȃ("ICE","ORE",Й.ȳ);bool ΖЍ=Αή>0;bool Βϋ=false;if(Щ>=0){float ĭ=(float) Math.Sin(ΓϏ(Щ))*Ș/3f;float Ζλ=(float) Math.Cos(ΓϏ(Щ))*ȁ/3f;Vector3 Ζσ=ȅ(υ,σ,true)+Βί(ǽ,ĕ*-1,new Vector3(ĭ,Ζλ,0));н(Ζσ,0.3f);if(Р<Math.Min(Ș,ȁ)/10f) Щ+=5f;if(Щ>=360) Щ=0;}if(Љ==-1||Αή<Љ){Љ=Αή;е=0;}
else{е++;if(е>50) Βϋ=true;}if(!ΖЍ||ď||Βϋ){if(!ď){int Ηύ=ȃ("","ORE",Й.ȳ);if(ŝ(true)) ď=true;else if(100-(І(Ηύ,ΕΠ)*100)<minEjection) ď=true;else ǳ(Ь.ƫ);}if(Βϋ&&ď) Λ="Ejection failed";if(ά==Γ.Ƴ){if(ď){if(Ν.δ) Σ(Γ.Ă);else{τ();ā();Λ="Can´t return, no dock found";}}
else Σ(Γ.ă);}else if(ď) Σ(Γ.ą);else Σ(Γ.š);return;}}Џ=false;}void Εφ(List<έ>ȱ,int ώ,List<Vector3>Ɖ,float ƈ,bool Ǘ,ref int ў){if(Ǘ){for(int i=0;i<ȱ.Count;i++) ȱ[i].Х=0;ў=-1;return;}if(ώ==0) return;int є=ώ*-1;if(ў==-1) ў=є>0?1:ȱ.Count-2;int Ŷ=0;while(ў>=1&&ў<ȱ.Count-1)
{if(Ŷ>50) return;Ŷ++;try{if((є<0&&ў>=1)||(є>0&&ў<=ȱ.Count-2)){έ η=ȱ[ў];bool ΕΛ=false;for(int j=0;j<Ɖ.Count;j++)
if(Vector3.Distance(η.Ε,Ɖ[j])<=ƈ){ΕΛ=true;break;}if(!ΕΛ){έ ΑΥ=ȱ[ў-є];έ ΕΜ=ȱ[ў+є];Vector3 Ηω=η.Ε-ΕΜ.Ε;Vector3 Γι=ΑΥ.Ε-η.Ε;Vector3 ΗΩ=η.Ε+Vector3.Normalize(Ηω)*Γι.Length();Vector3 ΖЁ=ΑΥ.Ε-ΗΩ;Vector3 ΑΘ=ć(ώ>0?η.α:η.α*-1,η.γ*-1,ΖЁ);Vector3 ΗΛ=ć(ώ>0?η.α:η.α*-1,η.γ*-1,Γι);
Vector3 ΑΚ=ć(ώ>0?η.α:η.α*-1,η.γ*-1,η.χ);η.Х=(float) Math.Sqrt(Math.Pow(ΑΥ.Х,2)+Math.Pow(Ɨ(-ΗΛ,ΑΚ,η),2));for(int j=0;j<Ɖ.Count;j++)
if(Vector3.Distance(ΑΥ.Ε,Ɖ[j])<=ƈ){Vector3 ΑΛ=ć(ώ>0?η.α:η.α*-1,η.γ*-1,Ɖ[j]-η.Ε);float Ηο=Ɨ(-ΑΛ,ΑΚ,η);η.Х=Math.Min(η.Х,Ηο)/2f;}if(ΑΘ.Length()==0) ΑΘ=new Vector3(0,0,1);Vector3 ΗΚ=ć(η.α,η.γ*-1,η.χ);float Ησ=Ś(-ΑΘ,ΗΚ,η);float ƴ=І(Ησ,Ě);float Γζ=(float) Math.Sqrt(І(ΑΘ.Length()*1.0f,0.5f*ƴ));
η.Х=Math.Min(η.Х,(Γι.Length()/Γζ)*ǡ);}}}catch{return;}ў+=є;}ў=-1;}void Βΰ(bool Ǘ){if(Ǘ){ŷ=0;return;}if(Π==Μ.ΰ) return;float ǥ=р*Math.Max(1,д);if(Ȭ==р) ǥ+=Math.Min(д,Ф);float Ηό=Ч*с*Math.Max(1,д);ŷ=Math.Max(ŷ,(float) Math.Min(ǥ/Ηό*100.0,100));}MyDetectedEntityType ΓЈ()
{try{if(ǲ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΑЁ(bool Ǩ){if(Π==Μ.ћ&&ΓЈ()==MyDetectedEntityType.None&&!Ŝ(ξ,true))
return fastSpeed;else return Ǩ?Ŕ:œ;}void ΖΫ(){float ΕΖ=0,Δρ=0,đ=0,ΓΠ=0;for(int i=0;i<ȣ.Count;i++){IMyBatteryBlock Ū=ȣ[i];if(!ǲ(Ū,true)) continue;ΕΖ+=Ū.MaxStoredPower;Δρ+=Ū.CurrentStoredPower;đ+=Ū.CurrentInput;ΓΠ+=Ū.CurrentOutput;}ǵ=(float) Math.Round(І(Δρ,ΕΖ)*100,1);
if(đ>=ΓΠ) Ǣ=й.ĥ;else Ǣ=й.ΓЀ;if(đ==0&&ΓΠ==0||ǵ==100.0) Ǣ=й.л;if(ȣ.Count==0) Ǣ=й.None;}void Ζί(){float Δό=0;for(int i=0;i<ю.Count;i++){IMyGasTank Εε=ю[i];if(!ǲ(Εε,true)) continue;Δό+=(float) Εε.FilledRatio;}Ȑ=І(Δό,ю.Count)*100f;}void Ζε(){Ğ=0;try{for(int i=0;i<Ȯ.Count;i++)
{IMyReactor ΓΧ=Ȯ[i];if(!ǲ(ΓΧ,true)) continue;List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();ΓΧ.GetInventory(0).GetItems(Ϗ);float ύ=0;for(int j=0;j<Ϗ.Count;j++){MyInventoryItem ς=Ϗ[j];if(ς.Type.SubtypeId.ToUpper()=="URANIUM"&&ς.Type.TypeId.ToUpper().Contains("_INGOT"))
ύ+=(float) ς.Amount;}if(ύ<Ğ||i==0){Ğ=ύ;ΓΓ=ΓΧ.CustomName;}}}catch(Exception e){Ȓ=e;}}void Ζά(){Ǫ=0;Ǥ=0;try{for(int i=0;i<Ĵ.Count;i++){IMyTerminalBlock Υ=Ĵ[i];if(!ǲ(Υ,true)) continue;Ǥ+=(float) Υ.GetInventory(0).CurrentVolume;Ǫ+=(float) Υ.GetInventory(0).MaxVolume;
}ΑΗ=(float) Math.Min(Math.Round(І(Ǥ,Ǫ)*100,1),100.0);}catch(Exception e){Ȓ=e;}}void Ζΰ(){try{Ǒ.Clear();for(int i=0;i<Ĵ.Count;i++){IMyTerminalBlock Υ=Ĵ[i];if(!ǲ(Υ,true)) continue;Γσ(Υ,Й.Ĩ);}if(С!=Ђ.ΒΑ){for(int i=0;i<ċ.Count;i++){IMyTerminalBlock Υ=ċ[i];if(!ǲ(Υ,true)) continue;
Γσ(Υ,Й.ΖΒ);}}ΖΧ(Ǒ);}catch(Exception e){Ȓ=e;}}void Εψ(){if(ǀ){if(ΓЇ().Count>ǋ){ǀ=false;if(ж!=б.ś){τ();if(ж==б.Αϊ) ā();if(ж==б.ƪ)
if(Ν.δ) Ɲ();else ā();}Λ="Damage detected";}}}bool Απ(){if(!ǌ) return true;if(ε==Ρ.А){if(т>0&&Ǣ!=й.None)
if(ǵ<=т){Λ="Low energy! Move home";return false;}if(Ш>0&&Ȯ.Count>0)
if(Ğ<=Ш){Λ="Low fuel: "+ΓΓ;return false;}if(у>0&&ю.Count>0)
if(Ȑ<=у){Λ="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΓЇ(){List<IMyTerminalBlock>č=new List<IMyTerminalBlock>();for(int i=0;i<Ŧ.Count;i++){IMyTerminalBlock Υ=Ŧ[i];if(Ŝ(Υ,false)) č.Add(Υ);}return č;}bool Ŝ(IMyTerminalBlock Υ,bool Γΰ){return(!ǲ(Υ,Γΰ)||!Υ.IsFunctional);
}bool ǲ(IMyTerminalBlock Υ,bool Γΰ){if(Υ==null) return false;try{IMyCubeBlock ΔϏ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(Γΰ) return ΔϏ==Υ;else return ΔϏ.GetType()==Υ.GetType();}catch{return false;}}float ΕϏ(IMyTerminalBlock Υ,String ч,String Εέ,String[]Ǧ){float Ǹ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory ΗΒ=Υ.GetInventory(i);List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();ΗΒ.GetItems(Ϗ);foreach(MyInventoryItem ς in Ϗ){if(Ǧ!=null&&(Ǧ.Contains(ς.Type.TypeId.ToUpper())||Ǧ.Contains(ς.Type.SubtypeId.ToUpper())))
continue;if((ч==""||ς.Type.TypeId.ToUpper()==ч)&&(Εέ==""||ς.Type.SubtypeId.ToUpper()==Εέ))
Ǹ+=(float) ς.Amount;}}return Ǹ;}Ц ΕЅ(String Ϊ,String ν,Й Н,bool Ζό){Ϊ=Ϊ.ToUpper();ν=ν.ToUpper();for(int i=0;i<Ǒ.Count;i++){Ц ς=Ǒ[i];if(ς.Ϊ.ToUpper()==Ϊ&&ς.ν.ToUpper()==ν&&(ς.ľ==Н||Н==Й.ȳ))
return ς;}Ц Ω=null;if(Ζό){Ω=new Ц(Ϊ,ν,0,Н);Ǒ.Add(Ω);}return Ω;}int ȃ(String Ϊ,String ν,Й Н){return ȃ(Ϊ,ν,Н,null);}int ȃ(String Ϊ,String ν,Й Н,String[]Ǧ){int Ǹ=0;Ϊ=Ϊ.ToUpper();ν=ν.ToUpper();for(int i=0;i<Ǒ.Count;i++){Ц ς=Ǒ[i];if(Ǧ!=null&&Ǧ.Contains(ς.Ϊ.ToUpper())) continue;
if((Ϊ==""||ς.Ϊ.ToUpper()==Ϊ)&&(ν==""||ς.ν.ToUpper()==ν)&&(ς.ľ==Н||Н==Й.ȳ))
Ǹ+=ς.ύ;}return Ǹ;}void Γσ(IMyTerminalBlock Υ,Й Н){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(Ϗ);for(int j=0;j<Ϗ.Count;j++)
ΕЅ(Ϗ[j].Type.SubtypeId,Ϗ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Н,true).ύ+=(int) Ϗ[j].Amount;}}void ΖΧ(List<Ц>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ц a=Ѝ[j];Ц b=Ѝ[j+1];if(a.ύ<b.ύ) Ѝ.Move(j,j+1);}}bool ŝ(bool ΗЎ){if(Ț&&Π!=Μ.ΰ)
if(Т!=-1&&Ě>=Т){Λ="Ship too heavy";return true;}if(ΑΗ>=ǩ||ƃ){ƃ=false;Λ="Ship is full";return true;}return false;}bool ΔΕ(){String[]Ǧ=null;if(!ș) Ǧ=new string[]{"ICE"};if(Π==Μ.Š) return ȃ("","ORE",Й.Ĩ,Ǧ)==0;if(Π==Μ.ћ) return ȃ("","COMPONENT",Й.Ĩ,Ǧ)==0;else return ȃ("","",Й.Ĩ,Ǧ)==0;
}bool ΖΕ(){List<IMyCockpit>Βσ=new List<IMyCockpit>();ω.GetBlocksOfType(Βσ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Βσ.Count;i++)
if(Βσ[i].IsUnderControl) return true;return false;}bool Εω(bool Ǘ,bool ΑΫ){IMyShipConnector џ=в(MyShipConnectorStatus.Connected);if(џ==null) return false;if(Vector3.Distance(Ν.Ε,џ.GetPosition())<5)
return Γό(ϋ,Ǘ,ΑΫ);if(Vector3.Distance(Θ.Ε,џ.GetPosition())<5)
return Γό(ό,Ǘ,ΑΫ);return false;}bool Γό(Ĭ Ƕ,bool Ǘ,bool ΑΫ){if(Ǘ) Ƕ.ΖΚ();Ƕ.Ζδ();bool з=false;String ц="";switch(Ƕ.Г){case ζ.ΒΓ:{ц="Waiting for command";з=false;break;}case ζ.ΔΛ:{ц="Waiting for cargo";з=ŝ(true);break;}case ζ.ΔΚ:{ц="Unloading";з=ΔΕ();break;}case ζ.ΒΘ:{з=true;break;}
case ζ.ΒΒ:{ц="Charging batteries";з=ǵ>=100f;break;}case ζ.ƨ:{ц="Discharging batteries";з=ǵ<=25f;break;}case ζ.Ƨ:{ц="Discharging batteries";з=ǵ<=0f;break;}case ζ.ΒΕ:{ц="Filling up hydrogen";з=Ȑ>=100f;break;}case ζ.ΒΖ:{ц="Unloading hydrogen";з=Ȑ<=25f;break;}
case ζ.ΒΔ:{ц="Unloading hydrogen";з=Ȑ<=0f;break;}case ζ.ΒΗ:{bool ΔІ=ΖΕ();if(!ΔІ) Ƕ.ΓΤ=true;з=Ƕ.ΓΤ&&ΔІ;ц="Waiting for passengers";break;}}if(!з) Ƕ.Αο(true);if(з&&Ƕ.ΑЃ()){з=Ƕ.Αο(false);ц="Undocking in: "+ƞ((int) Math.Max(0,Ƕ.Ǎ-Ƕ.ȧ));}if(ΑΫ) Επ=ц;return з;}void Εσ(){
if(!Ļ) return;if(ο.Count<=1) return;float Εκ=0,Ει=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǭ=ο[i];if(Ŝ(ǭ,true)) continue;Εκ+=(float) ǭ.GetInventory(0).MaxVolume;Ει+=(float) ǭ.GetInventory(0).CurrentVolume;}float Δδ=(float) Math.Round(І(Ει,Εκ),5);for(int ΕΣ=0;ΕΣ<Math.Max(1,Math.Floor(ο.Count/10f));ΕΣ++)
{float ΒЅ=0,ΓΒ=0;float ΔЄ=0,ΕΓ=0;IMyTerminalBlock Ȏ=null,Ȩ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock ǭ=ο[i];if(Ŝ(ǭ,true)) continue;float ΓΚ=(float) ǭ.GetInventory(0).MaxVolume;float ǅ=І((float) ǭ.GetInventory(0).CurrentVolume,ΓΚ);if(Ȏ==null||ǅ>ΒЅ){Ȏ=ǭ;ΒЅ=ǅ;ΔЄ=ΓΚ;}
if(Ȩ==null||ǅ<ΓΒ){Ȩ=ǭ;ΓΒ=ǅ;ΕΓ=ΓΚ;}}if(Ȏ==null||Ȩ==null||Ȏ==Ȩ) return;if(checkConveyorSystem&&!Ȏ.GetInventory(0).IsConnectedTo(Ȩ.GetInventory(0))){if(Βν>20) Λ="Inventory balancing failed";else Βν++;return;}Βν=0;List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();
Ȏ.GetInventory(0).GetItems(Ϗ);float Ƒ=0;if(Ϗ.Count==0) continue;MyInventoryItem ǆ=Ϗ[0];String ΔЉ=ǆ.Type.TypeId+ǆ.Type.SubtypeId;if(!Εο.TryGetValue(ΔЉ,out Ƒ)){if(ΖΖ(Ȏ.GetInventory(0),0,Ȩ.GetInventory(0),out Ƒ))
Εο.Add(ΔЉ,Ƒ);else return;}float Ηϊ=((ΒЅ-Δδ)*ΔЄ/Ƒ);float Ηϋ=((Δδ-ΓΒ)*ΕΓ/Ƒ);int Γλ=(int) Math.Min(Ηϋ,Ηϊ);if(Γλ<=0) return;if((float) ǆ.Amount<Γλ)
Ȏ.GetInventory(0).TransferItemTo(Ȩ.GetInventory(0),0,null,null,ǆ.Amount);else
Ȏ.GetInventory(0).TransferItemTo(Ȩ.GetInventory(0),0,null,null,Γλ);}}bool ΖΖ(IMyInventory Ƅ,int Α,IMyInventory ΖЀ,out float Εξ){Εξ=0;float Ζπ=(float) Ƅ.CurrentVolume;List<MyInventoryItem>Βξ=new List<MyInventoryItem>();Ƅ.GetItems(Βξ);float Δζ=0;for(int i=0;i<Βξ.Count;i++) Δζ+=(float) Βξ[i].Amount;
Ƅ.TransferItemTo(ΖЀ,Α,null,null,1);float Εμ=Ζπ-(float) Ƅ.CurrentVolume;List<MyInventoryItem>Βδ=new List<MyInventoryItem>();Ƅ.GetItems(Βδ);float Δέ=0;for(int i=0;i<Βδ.Count;i++) Δέ+=(float) Βδ[i].Amount;if(Εμ==0f||!ΑІ(0.9999,Δζ-Δέ,1.0001))
return false;Εξ=Εμ;return true;}void Ζή(){Vector3 Ƌ=Ѓ-μ;if(Ƌ.Length()==0) Ƌ=new Vector3(0,0,-1);Vector3 ΑΛ=Ы(Ψ,Ƌ);Vector3 Εζ=Vector3.Normalize(ΑΛ);Vector3 Ľ=Ы(Ψ,Ψ.GetNaturalGravity());float Ηι=ΓΡ>0?Math.Max(0,1-ģ(Ƌ,Δώ)/5):0;float Х=(float) Math.Min((ΓΗ>0?ΓΗ:1000f),Math.Max(Ɨ(-ΑΛ,Ľ,null),ΓΡ*Ηι));
if(!Εθ) Х=0;if(ΕΪ) Х=Math.Max(0,1-ƾ/Ƶ)*Х;if(generalSpeedLimit>0) Х=Math.Min(generalSpeedLimit,Х);if(ΕΫ) Х*=(float) Math.Min(1,І(Ƌ.Length(),wpReachedDist)/2);Vector3 ΕΑ=Ы(Ψ,Ψ.GetShipVelocities().LinearVelocity);float Ζκ=(float)(Math.Max(0,15-ģ(-Εζ,-ΕΑ))/15)*0.85f+0.15f;
Γδ+=Math.Sign(Ζκ-Γδ)/10f;Vector3 ȥ=Εζ*Х*Γδ-(ΕΑ);Vector3 Βλ=ΑЀ(ȥ,null);if(Αί&&Р>0.1f){ȥ.X*=ΒΨ(ȥ.X,ref Αΰ.X,1f,Βλ.X,20);ȥ.Y*=ΒΨ(ȥ.Y,ref Αΰ.Y,1f,Βλ.Y,20);ȥ.Z*=ΒΨ(ȥ.Z,ref Αΰ.Z,1f,Βλ.Z,20);}else Αΰ=new Vector3(1,1,1);Ɗ=Ě*ȥ-Ľ*Ě;Γτ(Ɗ,Αί);Р=Vector3.Distance(μ,Ѓ);
}float ΒΨ(float value,ref float φ,float є,float ȓ,float ΓΘ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(φ)) φ+=Math.Sign(φ)*є;else φ=value;if(value==0) φ=1;float Ω=Math.Abs(φ);if(Ω<ΓΘ||ȓ==0) return Ω;φ=Math.Min(ΓΘ,Math.Max(-ΓΘ,φ));Ω=Math.Abs(ȓ);return Ω;
}void Δά(){float х=90,ј=90,ђ=90;float ǘ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 Ǩ,ѐ,Ź,ΒА;if(ΒБ){Ǩ=Vector3.Normalize(Ѓ-μ);Ź=Ы(Ψ,Ǩ);ѐ=Ы(Ψ,Γβ);х=ģ(Ź,new Vector3(0,-1,0))-90;ј=Ā(ѐ,new Vector3(-1,0,0),ѐ.Y);
ђ=Ā(Ź,new Vector3(-1,0,0),Ź.Z);}else{Ǩ=Γγ;ѐ=Ы(Ψ,Γβ);Ź=Ы(Ψ,Γγ);ΒА=Ы(Ψ,Εη);х=Ā(ѐ,new Vector3(0,0,1),ѐ.Y);ј=Ā(ѐ,new Vector3(-1,0,0),ѐ.Y);ђ=Ā(ΒА,new Vector3(0,0,1),ΒА.X);}if(Δί&&ơ()){Vector3 ΒЀ=Ψ.GetNaturalGravity();ѐ=Ы(Ψ,ΒЀ);х=Ā(ѐ,new Vector3(0,0,1),ѐ.Y);
ј=Ā(ѐ,new Vector3(-1,0,0),ѐ.Y);}if(!ΑІ(-45,ј,45)){х=0;ђ=0;}if(!ΑІ(-45,ђ,45)) х=0;ƭ(ΒЂ,1,(-х)*ǘ,(-ђ)*ǘ,(-ј)*ǘ);ƾ=Math.Max(Math.Abs(х),Math.Max(Math.Abs(ј),Math.Abs(ђ)));ΑΑ=ƾ<=Ƶ;}void Εχ(IMyTerminalBlock ΑΦ){if(ΑΦ==null) return;Ǜ=new float[3,2];ѕ=new Dictionary<string,float[,]>();
for(int i=0;i<Ȁ.Count;i++){IMyThrust Я=Ȁ[i];if(!Я.IsFunctional) continue;Vector3 ώ=Ы(ΑΦ,Я.WorldMatrix.Backward);float Ȧ=Я.MaxEffectiveThrust;if(Math.Round(ώ.X,2)!=0.0)
if(ώ.X>=0) Ǜ[0,0]+=Ȧ;else Ǜ[0,1]-=Ȧ;if(Math.Round(ώ.Y,2)!=0.0)
if(ώ.Y>=0) Ǜ[1,0]+=Ȧ;else Ǜ[1,1]-=Ȧ;if(Math.Round(ώ.Z,2)!=0.0)
if(ώ.Z>=0) Ǜ[2,0]+=Ȧ;else Ǜ[2,1]-=Ȧ;String ч=ΓЎ(Я);float[,]і=null;if(ѕ.ContainsKey(ч)) і=ѕ[ч];else{і=new float[3,2];ѕ.Add(ч,і);}float ȓ=Я.MaxThrust;if(Math.Round(ώ.X,2)!=0.0)
if(ώ.X>=0) і[0,0]+=ȓ;else і[0,1]-=ȓ;if(Math.Round(ώ.Y,2)!=0.0)
if(ώ.Y>=0) і[1,0]+=ȓ;else і[1,1]-=ȓ;if(Math.Round(ώ.Z,2)!=0.0)
if(ώ.Z>=0) і[2,0]+=ȓ;else і[2,1]-=ȓ;}}static String ΓЎ(IMyThrust Я){return Я.BlockDefinition.SubtypeId;}Vector3 ΑЊ(Vector3 э,float[,]Ŀ){return new Vector3(
э.X>=0?Ŀ[0,0]:Ŀ[0,1],
э.Y>=0?Ŀ[1,0]:Ŀ[1,1],
э.Z>=0?Ŀ[2,0]:Ŀ[2,1]);}bool ΓЍ(έ Ϋ,String ч,out float Βϊ){Βϊ=0;int Α=ǿ.IndexOf(ч);if(Α==-1||Ϋ.ǚ==null||Α>=Ϋ.ǚ.Length)
return false;Βϊ=Ϋ.ǚ[Α];if(Βϊ==-1) return false;return true;}Vector3 ΑЀ(Vector3 э,έ Ϋ){if(Ϋ!=null){Vector3 Ω=new Vector3();for(int i=0;i<ѕ.Keys.Count;i++){String ч=ѕ.Keys.ElementAt(i);float Ų=0;if(!ΓЍ(Ϋ,ч,out Ų))
return ΑЊ(э,Ǜ);Ω+=ΑЊ(э,ѕ.Values.ElementAt(i))*Ų;}return Ω;}return ΑЊ(э,Ǜ);}float Ś(Vector3 э,έ Ϋ){return Ś(э,new Vector3(),Ϋ);}float Ś(Vector3 э,Vector3 χ,έ Ϋ){Vector3 Ης=ΑЀ(э,Ϋ);Vector3 Ȧ=Ης+χ*Ě;float Ę=(Ȧ/э).AbsMin();return(float)(э*Ę).Length();}float Ɨ(Vector3 ΑΙ,Vector3 Ľ,έ Ϋ)
{if(ΑΙ.Length()==0) return 0;float Δή=1;if(Ľ.Length()>0) Δή=Math.Min(1,ģ(-Ľ,ΑΙ)/90)*0.4f+0.6f;float Ɗ=Ś(ΑΙ,Ľ,Ϋ);if(Ɗ==0) return 0.1f;float ƴ=І(Ɗ,Ě);float Γζ=(float) Math.Sqrt(І(ΑΙ.Length(),ƴ*0.5f));return ƴ*Γζ*Δή*ǡ;}float Ɵ(Vector3 Ǩ,Vector3 ǜ,Vector3 χ,έ Ϋ){if(χ.Length()==0f) return 0;
Vector3 ΑΚ=ć(Ǩ,ǜ,Vector3.Normalize(χ));float ǐ=Ś(-ΑΚ,Ϋ);return ǐ/χ.Length();}void Γψ(bool ΗΑ){float ї=0;float Βύ=0.9f;if(ΗΑ){Т=-1;ğ=0;Αέ=null;if(ε!=Ρ.м&&Θ.χ.Length()!=0){ї=Βύ*Ɵ(Θ.α,Θ.γ*-1,Θ.χ,null);if(ї<Т||Т==-1) Т=ї;}if(Ν.δ&&Ν.χ.Length()!=0){ї=Βύ*Ɵ(Ν.α,Ν.γ*-1,Ν.χ,null);
if(ї<Т||Т==-1) Т=ї;}return;}if(ğ==-1) return;if(ğ>=0){int ΕΥ=0;while(ğ<Φ.Count){if(ΕΥ>100) return;ΕΥ++;έ Ϋ=Φ[ğ];if(Ϋ.χ.Length()!=0f){ї=Βύ*Math.Min(
Ɵ(Ϋ.α,Ϋ.γ*-1,Ϋ.χ,Ϋ),
Ɵ(Ϋ.α*-1,Ϋ.γ*-1,Ϋ.χ,Ϋ));if(ї<Т||Т==-1) Т=ї;}else Αέ=Ϋ;ğ++;}ğ=-1;}bool ΒЃ=true;float ȭ=0;if(Αέ!=null){for(int i=0;i<ѕ.Count;i++){String ч=ѕ.Keys.ElementAt(i);float[,]і=ѕ.Values.ElementAt(i);float Ų=0;if(!ΓЍ(Αέ,ч,out Ų)){ΒЃ=false;break;}for(int a=0;a<і.GetLength(0);a++)
for(int b=0;b<і.GetLength(1);b++){float Ċ=Math.Abs(і[a,b]*Ų);if(Ċ==0) continue;ΒЃ=true;if(ȭ==0||Ċ<ȭ) ȭ=Ċ;}}}if(!ΒЃ){for(int a=0;a<Ǜ.GetLength(0);a++)
for(int b=0;b<Ǜ.GetLength(1);b++){float Ċ=Math.Abs(Ǜ[a,b]);if(Ċ==0) continue;if(ȭ==0||Ċ<ȭ) ȭ=Ċ;}}if(ȭ>0){float ΗΞ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;ї=І(ȭ,ΗΞ);if(ї>0)
if(ї<Т||Т==-1) Т=ї;}}void Γτ(Vector3 ǐ,bool Ќ){if(!Ќ){for(int i=0;i<Ȁ.Count;i++)
Ȁ[i].SetValueFloat("Override",0.0f);return;}Vector3 Βκ=ΑЀ(ǐ,null);float ΗЉ=Math.Min(1,Math.Abs(І(ǐ.X,Βκ.X)));float ΗЊ=Math.Min(1,Math.Abs(І(ǐ.Y,Βκ.Y)));float ΗЋ=Math.Min(1,Math.Abs(І(ǐ.Z,Βκ.Z)));for(int i=0;i<Ȁ.Count;i++){IMyThrust Я=Ȁ[i];Vector3 ώ=ΖΜ(Ы(Ψ,Я.WorldMatrix.Backward),1);
if(ώ.X!=0&&Math.Sign(ώ.X)==Math.Sign(ǐ.X))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЉ);else if(ώ.Y!=0&&Math.Sign(ώ.Y)==Math.Sign(ǐ.Y))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЊ);else if(ώ.Z!=0&&Math.Sign(ώ.Z)==Math.Sign(ǐ.Z))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЋ);else
Я.SetValueFloat("Override",0.0f);}}void ƭ(bool Ќ,float ΗΦ,float х,float ђ,float ј){for(int i=0;i<ǈ.Count;i++){IMyGyro Ї=ǈ[i];Ї.GyroOverride=Ќ;if(!Ќ) Ї.GyroPower=100;else Ї.GyroPower=ΗΦ;if(!Ќ) continue;Vector3 ǹ=Ψ.WorldMatrix.Forward;Vector3 ņ=Ψ.WorldMatrix.Right;Vector3 ǜ=Ψ.WorldMatrix.Up;
Vector3 Ķ=Ї.WorldMatrix.Forward;Vector3 ĸ=Ї.WorldMatrix.Up;Vector3 ķ=Ї.WorldMatrix.Left*-1;if(Ķ==ǹ) Ї.SetValueFloat("Roll",ј);else if(Ķ==(ǹ*-1)) Ї.SetValueFloat("Roll",ј*-1);else if(ĸ==(ǹ*-1)) Ї.SetValueFloat("Yaw",ј);else if(ĸ==ǹ) Ї.SetValueFloat("Yaw",ј*-1);else if(ķ==ǹ) Ї.SetValueFloat("Pitch",ј);
else if(ķ==(ǹ*-1)) Ї.SetValueFloat("Pitch",ј*-1);if(ķ==(ņ*-1)) Ї.SetValueFloat("Pitch",х);else if(ķ==ņ) Ї.SetValueFloat("Pitch",х*-1);else if(ĸ==ņ) Ї.SetValueFloat("Yaw",х);else if(ĸ==(ņ*-1)) Ї.SetValueFloat("Yaw",х*-1);else if(Ķ==(ņ*-1)) Ї.SetValueFloat("Roll",х);
else if(Ķ==ņ) Ї.SetValueFloat("Roll",х*-1);if(ĸ==(ǜ*-1)) Ї.SetValueFloat("Yaw",ђ);else if(ĸ==ǜ) Ї.SetValueFloat("Yaw",ђ*-1);else if(ķ==ǜ) Ї.SetValueFloat("Pitch",ђ);else if(ķ==(ǜ*-1)) Ї.SetValueFloat("Pitch",ђ*-1);else if(Ķ==ǜ) Ї.SetValueFloat("Roll",ђ);else if(Ķ==(ǜ*-1)) Ї.SetValueFloat("Roll",ђ*-1);
}}void ΔΩ(){this.ΒЂ=false;}void Ъ(Vector3 ů,Vector3 ǹ,Vector3 ΒЏ,float ȗ,bool Ɛ){Αη(ů,ȗ,Ɛ);Ƶ=ȗ;ΒБ=false;this.Γγ=ǹ;this.Εη=ΒЏ;}void Ъ(Vector3 ů,Vector3 ǹ,Vector3 ΒЏ,bool Ɛ){Ъ(ů,ǹ,ΒЏ,2f,Ɛ);}void Αη(Vector3 ů,float ȗ,bool Ɛ){Ƶ=ȗ;this.ΒЂ=true;this.Δί=Ɛ;ΒБ=true;ΑΑ=false;
this.Γβ=ů;}void Βέ(){Ǟ(false,false,false,Ѓ,0);Αί=false;}void н(Vector3 ι,float ǘ){Ǟ(true,false,false,ι,ǘ);}void Ǟ(bool Ќ,bool Γέ,bool Γο,Vector3 ι,float ǘ){Ǟ(Ќ,Γέ,Γο,ι,ι-μ,0.0f,ǘ);}void Ǟ(bool Ќ,bool Γέ,bool Γο,Vector3 ι,Vector3 Ηγ,float Ηδ,float ǘ){Αί=true;this.Εθ=Ќ;
Ѓ=ι;this.ΓΗ=ǘ;this.ΓΡ=Ηδ;this.ΕΫ=Γέ;this.ΕΪ=Γο;this.Δώ=Ηγ;Р=Vector3.Distance(ι,μ);}void ΖΝ(){ω.GetBlocksOfType(Ŧ,щ);}void ΖΞ(){if(ΑΨ){Є=2;return;}List<IMyRemoteControl>ſ=new List<IMyRemoteControl>();List<IMySensorBlock>ΕΨ=new List<IMySensorBlock>();List<IMyTerminalBlock>Ƈ=new List<IMyTerminalBlock>();
ω.GetBlocksOfType(ſ,щ);ω.GetBlocksOfType(З,щ);ω.GetBlocksOfType(ΕΨ,щ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ƈ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);З=њ(З,pamTag,true);ΔΤ();ΔΥ(Ƈ);Ć(З,setLCDFontAndSize,1.4f,false);Ć(Ǭ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Εβ=њ(ΕΨ,pamTag,true);if(Εβ.Count>0) ξ=Εβ[0];else ξ=null;if(Π==Μ.Š){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){Є=1;Λ="Drills are missing";}}else if(Π==Μ.ћ){ω.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){Є=1;Λ="Grinders are missing";}if(ξ==null){Є=1;Λ="Sensor is missing";}}else if(Π==Μ.ΰ){ω.GetBlocksOfType(Γη,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Εα=њ(ſ,pamTag,true);if(Εα.Count>0) ſ=Εα;if(ſ.Count>0) Ψ=ſ[0];else{Ψ=null;Є=2;Λ="Remote is missing";return;}ψ=(float) Ψ.CubeGrid.WorldVolume.Radius*2;ё=null;if(Π!=Μ.ΰ){Γω();if(ο.Count>0&&ё!=null){if(ξ!=null&&(ё.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Ψ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Ψ.WorldMatrix.Down==ξ.WorldMatrix.Down))){Є=1;Λ="Wrong sensor direction";}if(ё.WorldMatrix.Forward!=Ψ.WorldMatrix.Forward&&
ё.WorldMatrix.Forward!=Ψ.WorldMatrix.Down){Є=2;Λ="Wrong remote direction";}}}}void ΖΟ(){ω.GetBlocksOfType(Ǽ,щ);ω.GetBlocksOfType(п,щ);ω.GetBlocksOfType(Ȁ,щ);ω.GetBlocksOfType(ǈ,щ);ω.GetBlocksOfType(ȣ,щ);ω.GetBlocksOfType(Ȯ,щ);ω.GetBlocksOfType(ю,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ω.GetBlocksOfType(Αά,щ);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
п=њ(п,"ConnectorMedium",false);else
п=њ(п,"Connector",false);List<IMyShipConnector>Εί=њ(п,pamTag,true);if(Εί.Count>0) п=Εί;if(Є<=1){if(п.Count==0){Є=1;Λ="Connector is missing";}if(ǈ.Count==0){Є=1;Λ="Gyros are missing";}if(Ȁ.Count==0){Є=1;Λ="Thrusters are missing";}}List<IMyConveyorSorter>Εγ=њ(Αά,pamTag,true);
if(Εγ.Count>0) Αά=Εγ;List<IMyLandingGear>Εΰ=њ(Ǽ,pamTag,true);if(Εΰ.Count>0) Ǽ=Εΰ;for(int i=0;i<Ǽ.Count;i++) Ǽ[i].AutoLock=false;List<IMyBatteryBlock>Εή=њ(ȣ,pamTag,true);if(Εή.Count>0) ȣ=Εή;List<IMyGasTank>Εδ=њ(ю,pamTag,true);if(Εδ.Count>0) ю=Εδ;}void ΖΠ(){ω.GetBlocksOfType(ċ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
Ĵ.Clear();for(int i=ċ.Count-1;i>=0;i--){if(ΖΓ(ċ[i])){Ĵ.Add(ċ[i]);ċ.RemoveAt(i);}}}bool ΖΓ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΑЄ(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Γω(){if(Ψ==null) return;ё=null;float ΑΡ=0,ΑΜ=0,ΑΣ=0,ΑΝ=0,ΑΤ=0,ΑΞ=0;List<IMyTerminalBlock>Γα=њ(ο,pamTag,true);bool ΗΣ=Γα.Count==0;if(Γα.Count>0) ё=Γα[0];else if(ο.Count>0) ё=ο[0];int Ǹ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ё.WorldMatrix.Forward){if(ΗΣ){Є=2;Λ="Mining direction is unclear!";return;}continue;}Ǹ++;Vector3 Ў=Αύ(Ψ,ο[i].GetPosition());if(i==0){ΑΡ=Ў.X;ΑΜ=Ў.X;ΑΣ=Ў.Y;ΑΝ=Ў.Y;ΑΤ=Ў.Z;ΑΞ=Ў.Z;}ΑΜ=Math.Max(Ў.X,ΑΜ);ΑΡ=Math.Min(Ў.X,ΑΡ);ΑΝ=Math.Max(Ў.Y,ΑΝ);ΑΣ=Math.Min(Ў.Y,ΑΣ);
ΑΞ=Math.Max(Ў.Z,ΑΞ);ΑΤ=Math.Min(Ў.Z,ΑΤ);}Ș=(ΑΜ-ΑΡ)*(1-Ɠ)+drillRadius*2;ȁ=(ΑΝ-ΑΣ)*(1-Ĺ)+drillRadius*2;if(ё!=null&&ё.WorldMatrix.Forward==Ψ.WorldMatrix.Down)
ȁ=(ΑΞ-ΑΤ)*(1-Ĺ)+drillRadius*2;}List<T>њ<T>(List<T>Ѝ,String Ǚ,bool ǉ){List<T>Ω=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(ΑЄ(Ѝ[i],Ǚ,ǉ)) Ω.Add(Ѝ[i]);return Ω;}bool ΑЄ<T>(T Υ,String Ǚ,bool ǉ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(ǉ&&b.CustomName.ToUpper().Contains(Ǚ.ToUpper())) return true;if(!ǉ&&b.BlockDefinition.ToString().ToUpper().Contains(Ǚ.ToUpper())) return true;return false;}void ъ<T>(List<T>Ѝ,bool Ќ)
{for(int i=0;i<Ѝ.Count;i++)
ΔΡ((IMyTerminalBlock) Ѝ[i],Ќ);}void ΔΡ(IMyTerminalBlock Υ,bool Ќ){if(Υ==null) return;String Ζθ=Ќ?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(Ζθ);a.Apply(Υ);}void ǟ(bool Ќ){Γή=Ќ;if(!Ĝ) return;ъ(Αά,Ќ);}void ΒΥ(ChargeMode Ё){for(int i=0;i<ȣ.Count;i++) ȣ[i].ChargeMode=Ё;}void ΒΦ(bool Ημ)
{for(int i=0;i<ю.Count;i++) ю[i].Stockpile=Ημ;}void ţ(List<IMyLandingGear>Βώ,bool ΕΒ){for(int i=0;i<Βώ.Count;i++){if(ΕΒ) Βώ[i].Lock();if(!ΕΒ) Βώ[i].Unlock();}}void ΔΣ(bool Ќ){if(Δς==Ќ) return;List<IMyShipController>ƽ=new List<IMyShipController>();ω.GetBlocksOfType(ƽ,щ);if(ƽ.Count==0) return;
for(int i=0;i<ƽ.Count;i++)
ƽ[i].DampenersOverride=Ќ;Δς=Ќ;}IMyShipConnector в(MyShipConnectorStatus ц){for(int i=0;i<п.Count;i++){if(!ǲ(п[i],true)) continue;if(п[i].Status==ц) return п[i];}return null;}void Ĉ(bool ΓΪ,bool ǂ,float ΖЊ,float Ζξ){if(ξ==null||ο.Count==0) return;Vector3 ƺ=new Vector3();int Ǹ=0;
for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ё.WorldMatrix.Forward) continue;Ǹ++;ƺ+=ο[i].GetPosition();}ƺ=ƺ/Ǹ;Vector3 Ń=Αύ(ξ,ƺ);ξ.Enabled=true;ξ.ShowOnHUD=ΓΪ;ξ.LeftExtend=(ǂ?1:ь)/2f*Ș-Ń.X;ξ.RightExtend=(ǂ?1:ь)/2f*Ș+Ń.X;ξ.TopExtend=(ǂ?1:ы)/2f*ȁ+Ń.Y;
ξ.BottomExtend=(ǂ?1:ы)/2f*ȁ-Ń.Y;ξ.FrontExtend=(ΓΪ?д:ΖЊ)-Ń.Z;ξ.BackExtend=ΓΪ?0:Ζξ+ψ*0.75f+Ń.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ć<T>(List<T>Ѝ,bool Ηζ,float ΖЉ,bool ΗΓ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Ɔ=null;if(Ѝ[i]is IMyTextSurface) Ɔ=(IMyTextSurface) Ѝ[i];if(Ɔ!=null){Ɔ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ηζ) continue;Ɔ.Font="Debug";if(ΗΓ) continue;Ɔ.FontSize=ΖЉ;}}}bool ơ(){Vector3D И;return this.Ψ.TryGetPlanetPosition(out И);}int ΕЍ(Vector3 Ηξ,int Εά){if(Εά==-1) return 0;double М=-1;int Ȍ=-1;for(int i=Φ.Count-1;i>=0;i--){double Ћ=Vector3.Distance(Φ[i].Ε,Ηξ);if(М==-1||Ћ<М){Ȍ=i;М=Ћ;}
}return Math.Sign(Ȍ-Εά);}bool ΖΔ(Vector3 И){List<Vector3>О=new List<Vector3>();for(int i=0;i<Φ.Count;i++) О.Add(Φ[i].Ε);if(Ν.δ&&Φ.Count>=1){Vector3 į=new Vector3();Ǳ(Ν,dockDist*ψ,false,out į);if(Vector3.Distance(Ν.Ε,Φ.First().Ε)<Vector3.Distance(Ν.Ε,Φ.Last().Ε)){О.Insert(0,į);О.Insert(0,Ν.Ε);}
else{О.Add(į);О.Add(Ν.Ε);}}if(Π==Μ.ΰ){if(Θ.δ&&Φ.Count>=1){Vector3 Βζ=new Vector3();Ǳ(Θ,dockDist*ψ,false,out Βζ);if(Vector3.Distance(Θ.Ε,Φ.First().Ε)<Vector3.Distance(Θ.Ε,Φ.Last().Ε)){О.Insert(0,Βζ);О.Insert(0,Θ.Ε);}else{О.Add(Βζ);О.Add(Θ.Ε);}}}else{if(ε!=Ρ.м)
if(Φ.Count>0&&Vector3.Distance(Θ.Ε,Φ.First().Ε)<Vector3.Distance(Θ.Ε,Φ.Last().Ε))
О.Insert(0,Θ.Ε);else О.Add(Θ.Ε);}int Ȍ=-1;double ǣ=-1;for(int i=0;i<О.Count;i++){double Ћ=Vector3.Distance(О[i],И);if(Ћ<ǣ||ǣ==-1){ǣ=Ћ;Ȍ=i;}}if(О.Count==0) return false;double Δχ=Vector3.Distance(О[Ȍ],И);double ΗΧ=Vector3.Distance(О[Math.Max(0,Ȍ-1)],О[Ȍ])*1.5f;
double ΗΡ=Vector3.Distance(О[Math.Min(О.Count-1,Ȍ+1)],О[Ȍ])*1.5f;return Δχ<ΗΧ||Δχ<ΗΡ;}static float І(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΖΜ(Vector3 v,int Βφ){return new Vector3(Math.Round(v.X,Βφ),Math.Round(v.Y,Βφ),Math.Round(v.Z,Βφ));}float ģ(Vector3 a,Vector3 b)
{if(a==b) return 0;float ΖЅ=(a*b).Sum;float ΔЎ=a.Length();float ΔЏ=b.Length();if(ΔЎ==0||ΔЏ==0) return 0;float Ω=(float)((180.0f/Math.PI)*Math.Acos(ΖЅ/(ΔЎ*ΔЏ)));if(float.IsNaN(Ω)) return 0;return Ω;}float Ā(Vector3 v,Vector3 ΑΦ,float Ηη){float ĭ=ģ(v,ΑΦ);if(Ηη>0f) ĭ*=-1;
if(ĭ>-90f) return ĭ-90f;else return 180f-(-ĭ-90f);}double ΓϏ(float Ζύ){return(Math.PI/180)*Ζύ;}bool ΑІ(double ΗΝ,double value,double ΓΖ){return(value>=ΗΝ&&value<=ΓΖ);}Vector3 ΖΪ(IMyTerminalBlock Υ,Vector3 Ў){return Vector3D.Transform(Ў,Υ.WorldMatrix);}Vector3 Αύ(IMyTerminalBlock Υ,Vector3 ΗІ)
{return Ы(Υ,ΗІ-Υ.GetPosition());}Vector3 Ы(IMyTerminalBlock Υ,Vector3 Γς){return Vector3D.TransformNormal(Γς,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 ć(Vector3 Ǩ,Vector3 ǜ,Vector3 Γς){MatrixD ΓΕ=MatrixD.CreateFromDir(Ǩ,ǜ);return Vector3D.TransformNormal(Γς,MatrixD.Transpose(ΓΕ));}Vector3 Βί(Vector3 Ǩ,Vector3 ǜ,Vector3 Ў)
{MatrixD ΓΕ=MatrixD.CreateFromDir(Ǩ,ǜ);return Vector3D.Transform(Ў,ΓΕ);}String Л(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 К(String s){String[]λ=s.Split('|');return new Vector3(float.Parse(Ο(λ,0)),float.Parse(Ο(λ,1)),float.Parse(Ο(λ,2)));}String Ζζ(έ Ϋ){String В=":";
String Ω=Л(Ϋ.Ε)+В+Л(Ϋ.α)+В+
Л(Ϋ.γ)+В+Л(Ϋ.ϊ)+В+Л(Ϋ.χ);for(int i=0;i<Ϋ.ǚ.Length;i++)
Ω+=В+Math.Round(Ϋ.ǚ[i],3);return Ω;}έ ΖΨ(String s){String[]λ=s.Split(':');έ Ϋ=new έ(
К(Ο(λ,0)),К(Ο(λ,1)),
К(Ο(λ,2)),К(Ο(λ,3)),
К(Ο(λ,4)));int Α=5;List<float>Δϋ=new List<float>();while(Α<λ.Length){String Αβ=Ο(λ,Α);float f=0;if(!float.TryParse(Αβ,out f)) break;Δϋ.Add(f);Α++;}Ϋ.ǚ=Δϋ.ToArray();return Ϋ;}System.Text.StringBuilder ň=new System.Text.StringBuilder();void Τ<T>(T value,bool ΗΠ){if(ΗΠ) ň.Append('\n');ň.Append(value);}
void Τ<T>(T value){Τ(value,true);}String ƚ(String s){return s.Replace("%","%25").Replace(";","%3B").Replace("\n","%0A");}String Ʊ(String s){return s.Replace("%0A","\n").Replace("%3B",";").Replace("%25","%");}String Ο(String[]Ζμ,int Α){String Αβ=Ζμ.ElementAtOrDefault(Α);if(String.IsNullOrEmpty(Αβ)) return "";
return Αβ;}void Save(){if(ΑΨ||Π==Μ.к){Storage="";return;}Storage=Αν();}String Αν(){ň.Clear();ň.Append(DATAREV+";");Τ(Л(ΑΧ),false);Τ(Л(Ν.α));Τ(Л(Ν.ϊ));Τ(Л(Ν.γ));Τ(Л(Ν.χ));Τ(Л(Ν.Ε));Τ(Л(Ν.о));Τ(Ν.δ);Τ(Л(Θ.Ε));Τ(Л(Θ.χ));Τ(Л(Θ.α));Τ(Л(Θ.γ));Τ(Л(Θ.ϊ));Τ(Л(Θ.о));Τ(Θ.δ);Τ(Л(ǽ));Τ(Л(ĕ));Τ(";");
Τ((int) Π,false);Τ((int) ε);Τ((int) я);Τ(ǩ);Τ(т);Τ(Ш);Τ(у);Τ(ǡ);Τ(Ț);Τ(ș);if(Π==Μ.ΰ){Τ((int) ϋ.Г);Τ(ϋ.Ǎ);Τ(ϋ.ȧ);Τ(ƚ(ϋ.Ǐ));Τ(ƚ(ϋ.ǒ));Τ((int) ό.Г);Τ(ό.Ǎ);Τ(ό.ȧ);Τ(ƚ(ό.Ǐ));Τ(ƚ(ό.ǒ));}else{Τ((int) Ȗ);Τ((int) ж);Τ((int) С);Τ((int) ǎ);Τ((int) ΑΕ);Τ(Ȱ);Τ(Ĝ);Τ(Ļ);Τ(ĵ);Τ(ь);Τ(ы);Τ(д);Τ(Ŕ);Τ(œ);Τ(Ɠ);Τ(Ĺ);
Τ(Ч);Τ(с);Τ(υ);Τ(σ);Τ(Ȭ);Τ(р);Τ(ę);Τ(Ł);}Τ(";");for(int i=0;i<ǿ.Count;i++)
Τ((i>0?"|":"")+ǿ[i],false);Τ(";");for(int i=0;i<Φ.Count;i++)
Τ(Ζζ(Φ[i]),i>0);String Χ=ň.ToString();ň.Clear();return Χ;}ш ΖΛ(){return ΒΟ(Storage);}ш ΒΟ(String Χ){if(Χ=="") return ш.ΖΗ;String[]Ƃ=Χ.Split(';');if(Ο(Ƃ,0)!=DATAREV) return ш.ΑА;int Α=0;try{String[]Κ=Ο(Ƃ,1).Split('\n');ΑΧ=К(Ο(Κ,Α++));Ν.α=К(Ο(Κ,Α++));Ν.ϊ=К(Ο(Κ,Α++));Ν.γ=К(Ο(Κ,Α++));
Ν.χ=К(Ο(Κ,Α++));Ν.Ε=К(Ο(Κ,Α++));Ν.о=К(Ο(Κ,Α++));Ν.δ=bool.Parse(Ο(Κ,Α++));Θ.Ε=К(Ο(Κ,Α++));Θ.χ=К(Ο(Κ,Α++));Θ.α=К(Ο(Κ,Α++));Θ.γ=К(Ο(Κ,Α++));Θ.ϊ=К(Ο(Κ,Α++));Θ.о=К(Ο(Κ,Α++));Θ.δ=bool.Parse(Ο(Κ,Α++));ǽ=К(Ο(Κ,Α++));ĕ=К(Ο(Κ,Α++));Κ=Ο(Ƃ,2).Split('\n');Α=0;Π=(Μ) int.Parse(Ο(Κ,Α++));
ε=(Ρ) int.Parse(Ο(Κ,Α++));я=(Ρ) int.Parse(Ο(Κ,Α++));ǩ=int.Parse(Ο(Κ,Α++));т=int.Parse(Ο(Κ,Α++));Ш=int.Parse(Ο(Κ,Α++));у=int.Parse(Ο(Κ,Α++));ǡ=float.Parse(Ο(Κ,Α++));Ț=bool.Parse(Ο(Κ,Α++));ș=bool.Parse(Ο(Κ,Α++));if(Π==Μ.ΰ){ϋ.Г=(ζ) int.Parse(Ο(Κ,Α++));ϋ.Ǎ=float.Parse(Ο(Κ,Α++));
ϋ.ȧ=float.Parse(Ο(Κ,Α++));ϋ.Ǐ=Ʊ(Ο(Κ,Α++));ϋ.ǒ=Ʊ(Ο(Κ,Α++));ό.Г=(ζ) int.Parse(Ο(Κ,Α++));ό.Ǎ=float.Parse(Ο(Κ,Α++));ό.ȧ=float.Parse(Ο(Κ,Α++));ό.Ǐ=Ʊ(Ο(Κ,Α++));ό.ǒ=Ʊ(Ο(Κ,Α++));}else{Ȗ=(Ǡ) int.Parse(Ο(Κ,Α++));ж=(б) int.Parse(Ο(Κ,Α++));С=(Ђ) int.Parse(Ο(Κ,Α++));ǎ=(љ) int.Parse(Ο(Κ,Α++));
ΑΕ=(Ǡ) int.Parse(Ο(Κ,Α++));Ȱ=bool.Parse(Ο(Κ,Α++));Ĝ=bool.Parse(Ο(Κ,Α++));Ļ=bool.Parse(Ο(Κ,Α++));ĵ=bool.Parse(Ο(Κ,Α++));ь=int.Parse(Ο(Κ,Α++));ы=int.Parse(Ο(Κ,Α++));д=int.Parse(Ο(Κ,Α++));Ŕ=float.Parse(Ο(Κ,Α++));œ=float.Parse(Ο(Κ,Α++));Ɠ=float.Parse(Ο(Κ,Α++));Ĺ=float.Parse(Ο(Κ,Α++));
Ч=int.Parse(Ο(Κ,Α++));с=int.Parse(Ο(Κ,Α++));υ=int.Parse(Ο(Κ,Α++));σ=int.Parse(Ο(Κ,Α++));Ȭ=int.Parse(Ο(Κ,Α++));р=int.Parse(Ο(Κ,Α++));ę=int.Parse(Ο(Κ,Α++));Ł=float.Parse(Ο(Κ,Α++));}Κ=Ο(Ƃ,3).Replace("\n","").Split('|');ǿ=Κ.ToList();Κ=Ο(Ƃ,4).Split('\n');Φ.Clear();if(Κ.Count()>=1&&Κ[0]!="")
for(int i=0;i<Κ.Length;i++)
Φ.Add(ΖΨ(Ο(Κ,i)));}catch{return ш.ΓЁ;}ŉ=я;τ();return ш.Βή;}const int Γχ=64000;IMyTerminalBlock ΓЂ(){List<IMyTerminalBlock>Ĳ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(Ĳ,q=>q.CubeGrid==Me.CubeGrid&&q.CustomName.Contains(dataTag));return Ĳ.Count>0?Ĳ[0]:null;}void ΕЄ(){IMyTerminalBlock Υ=ΓЂ();
if(Υ==null){Λ="Export: name a block "+dataTag;return;}String Χ="// PAM export "+VERSION+" | "+Me.CubeGrid.CustomName+" | "+Φ.Count+
" waypoints | "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n"+Αν();if(Χ.Length>Γχ){Λ="Export too large: "+Χ.Length+" chars, max "+Γχ;return;}Υ.CustomData=Χ;Λ="Exported "+Φ.Count+" waypoints";}void ΖΑ(){IMyTerminalBlock Υ=ΓЂ();if(Υ==null){Λ="Import: name a block "+dataTag;return;}
String Χ=Υ.CustomData.Replace("\r","");if(Χ.StartsWith("//")){int ΕΝ=Χ.IndexOf('\n');Χ=ΕΝ<0?"":Χ.Substring(ΕΝ+1);}Χ=Χ.Trim();if(Χ==""){Λ="Import: no PAM data in "+dataTag;return;}τ();String Ζο=Αν();ш Ω=ΒΟ(Χ);if(Ω==ш.Βή){ż=false;Λ="Imported "+Φ.Count+" waypoints";
return;}ΒΟ(Ζο);Λ=Ω==ш.ΑА?"Import: data is from another PAM version":"Import failed, nothing changed";}void ΔΤ(){Ď.Clear();for(int i=З.Count-1;i>=0;i--){String Χ=З[i].CustomData.ToUpper();bool ΒЈ=false;if(Χ=="INSTRUCTIONS"){ΒЈ=true;ǁ=true;}if(Χ=="DEBUG") ΒЈ=true;if(ΒЈ){Ď.Add(З[i]);З.RemoveAt(i);}
}Ć(Ď,false,1,false);}void ΔΥ(List<IMyTerminalBlock>Ĳ){Ǭ.Clear();for(int i=0;i<Ĳ.Count;i++){IMyTerminalBlock Υ=Ĳ[i];try{String ΕΤ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Υ.CustomName.IndexOf(ΕΤ);int ō=-1;if(Α<0||!int.TryParse(Υ.CustomName.Substring(Α+ΕΤ.Length,1),out ō)) continue;
if(ō==-1) continue;ō--;IMyTextSurfaceProvider ΕΦ=(IMyTextSurfaceProvider) Υ;if(ō<ΕΦ.SurfaceCount&&ō>=0)
Ǭ.Add(ΕΦ.GetSurface(ō));}catch{}}}String ΑΟ="";void ΔΞ(){String ΓΔ="";String ű="";ΑΟ=Γφ(false);ΓΔ+=ΑΟ;ű+=ΑΟ;ű+=ΕЊ();for(int i=0;i<З.Count;i++) З[i].WriteText(ΓΔ);for(int i=0;i<Ǭ.Count;i++) Ǭ[i].WriteText(ΓΔ);Echo(ű);for(int i=0;i<Ď.Count;i++){IMyTextPanel Ю=Ď[i];
String Χ=Ю.CustomData.ToUpper();if(Χ=="DEBUG") Ю.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") Ю.WriteText(ΓЉ());}}string ΓЉ(){String Β="";try{float ΕΔ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ı*ΕΔ)+" / "+(int)(ŀ*ΕΔ)+"\n";
Β+="Inst. avg/max: "+Math.Round(ı*100f,1)+"% / "+Math.Round(ŀ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(ũ,2)+"ms / "+Math.Round(ΓΙ,2)+"ms \n";}catch{}for(int i=0;i<Ȕ.Count;i++)
Β+=""+Ȕ.Keys.ElementAt(i)+" = "+Math.Round(Ȕ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȕ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΕБ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ć(З,setLCDFontAndSize,0.9f,false);Ć(Ǭ,setLCDFontAndSize,0.9f,true);for(int i=0;i<З.Count;i++) З[i].WriteText(Β+Ȓ.ToString());for(int i=0;i<Ǭ.Count;i++) Ǭ[i].WriteText(Β+Ȓ.ToString());}void Ǵ(){if(!ǁ) return;try{ΓΥ=Runtime.CurrentInstructionCount;}catch{}
}void ǰ(String Ϊ){if(!ǁ) return;if(ΓΥ==0) return;try{float Αα=(Runtime.CurrentInstructionCount-ΓΥ)/Runtime.MaxInstructionCount*100;if(!Ȕ.ContainsKey(Ϊ))
Ȕ.Add(Ϊ,new float[]{Αα,Αα});else{Ȕ[Ϊ][0]=Αα;Ȕ[Ϊ][1]=Math.Max(Αα,Ȕ[Ϊ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Ετ(String ǧ){if(!ĳ) return;String В=""+BROADCAST_SEP;String κ=VERSION+В;κ+=Ŗ(Ō)+В;
κ+=(int) Π+В;κ+=Ħ(μ.X)+""+В;κ+=Ħ(μ.Y)+В;κ+=Ħ(μ.Z)+В;if(Π!=Μ.ΰ)
κ+=Αό(я)+(я==Ρ.А?" "+Math.Round(ŷ,1)+"%":"")+В;else κ+=Αό(я)+В;κ+=Ŗ(Λ)+В;κ+=Ѐ+""+В;κ+=Ŗ(ΑΟ)+В;κ+=Ǥ+В;κ+=Ǫ+В;for(int i=0;i<Ǒ.Count;i++){if(Ǒ[i].ľ!=Й.Ĩ) continue;κ+=Ŗ(Ǒ[i].Ϊ)+"/"+Ŗ(Ǒ[i].ν)+"/"+Ǒ[i].ύ+В;}ΖΣ(κ,ǧ);}String Ŗ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΖΣ(String κ,String ǧ){ΒΣ(κ,BROADCAST_SELF_ID,ǧ);}void ΒΣ(String κ,String ι,String ǧ){try{if(!ĳ) return;κ=BROADCAST_TAG+BROADCAST_SEP+ΓЅ()+BROADCAST_SEP+ι+BROADCAST_SEP+Βπ+BROADCAST_SEP+ǧ+BROADCAST_SEP+κ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,κ);}catch(Exception e){Ȓ=e;}
}String ΓЅ(){if(Π!=Μ.к) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΑЉ(String κ){return κ.StartsWith(BROADCAST_TAG);}bool У(ref String Χ,out string ų,Char В){int Α=Χ.IndexOf(В);ų="";if(Α<0) return false;ų=Χ.Substring(0,Α);Χ=Χ.Remove(0,Α+1);return true;}bool ΔΝ(ref String κ,out String ǫ,out String ǧ)
{ǫ="";ǧ="";if(!ĳ) return false;String Ǚ="";if(!У(ref κ,out Ǚ,BROADCAST_SEP)||!ΑЉ(Ǚ)) return false;if(!У(ref κ,out ǫ,BROADCAST_SEP)) return false;if(!У(ref κ,out Ǚ,BROADCAST_SEP)||(Ǚ!=ΓЅ()&&(Ǚ!="*"&&Π!=Μ.к))) return false;if(!У(ref κ,out Ǚ,BROADCAST_SEP)||(Ǚ!=Βπ)) return false;
if(!У(ref κ,out ǧ,BROADCAST_SEP)) return false;return true;}void ΔΒ(){bool ŭ=true;if(Δύ){Δύ=false;if(Me.CustomData.Contains("Antenna_Name")){Λ="Update custom data";Me.CustomData="";}}String Ŵ=(Π!=Μ.к?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=Ŵ)
ΔΓ(Ŵ);String[]Κ=Me.CustomData.Split('\n');ĳ=bool.Parse(Ʃ(ƛ(Κ,"Enable_Broadcast",ref ŭ)));bool Γπ=false;bool Δυ=true;if(ĳ){if(Π!=Μ.к)
Ō=Ʃ(ƛ(Κ,"Ship_Name",ref ŭ)).Replace(BROADCAST_SELF_ID,"");Βπ=Ʃ(ƛ(Κ,"Broadcast_Channel",ref ŭ)).ToLower();Δυ=false;if(ѝ==null){ѝ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ѝ.SetMessageCallback("");}List<IMyRadioAntenna>Į=new List<IMyRadioAntenna>();ω.GetBlocksOfType(Į);bool Βε=false;
for(int i=0;i<Į.Count;i++)
if(Į[i].EnableBroadcasting&&Į[i].Enabled){Βε=true;break;}if(Į.Count==0) Λ="No Antenna found";else if(!Βε) Λ="Antenna not ready";Γπ=Į.Count==0||!Βε;if(Δΰ&&!Γπ&&Π!=Μ.к) Λ="Antenna ok";}else if(Π==Μ.к)
Λ="Offline - Enable in PB custom data";Δΰ=Γπ;if(Δυ){if(ѝ!=null) this.IGC.DisableBroadcastListener(ѝ);ѝ=null;}}catch{ŭ=false;}if(!ŭ){Λ="Reset custom data";ΔΓ(Ŵ);}}void ΔΓ(String Ŵ){String Χ=Ŵ+"\n\n"+"Enable_Broadcast="+(Π==Μ.к?"true":"false")+" \n";Χ+=Π!=Μ.к?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=Χ+"Broadcast_Channel=#default";}String Ʃ(String ē){int Α=ē.IndexOf("//");if(Α!=-1) ē=ē.Substring(0,Α);String[]λ=ē.Split('=');if(λ.Length<=1) return "";return λ[1].Trim();}String ƛ(String[]Κ,String ΗΖ,ref bool Ė){foreach(String ē in Κ)
if(ē.StartsWith(ΗΖ)) return ē;Ė=false;return "";}void Εώ(string Ж){if(ȍ) П=new List<Б>();String ǫ="";String ǧ="";if(!ΑЉ(Ж)) ΖΙ(Ж);bool ΕΩ=false;int ΓΜ=50;while(ѝ!=null&&ѝ.HasPendingMessage&&ΓΜ-->0){MyIGCMessage κ=ѝ.AcceptMessage();String Χ=κ.Data as String;if(Χ==null) continue;
if(ΔΝ(ref Χ,out ǫ,out ǧ)&&ǫ!=""&&ǫ!=BROADCAST_SELF_ID){Б Δ=Αψ(ǫ,"");if(Δ==null){Δ=new Б(ǫ);П.Add(Δ);}Δ.ĩ(ǧ,false,false,true);Ζγ(Δ,Χ);ΕΩ=true;}}if(ΕΩ)
П.Sort(delegate(Б a,Б b){if(a.Ϊ==null&&b.Ϊ==null) return 0;else if(a.Ϊ==null) return-1;else if(b.Ϊ==null) return 1;else return a.Ϊ.CompareTo(b.Ϊ);});if(ȑ||ȍ){if(ƀ<=0||ȍ){Λ="";ȍ=false;ΖΡ();ΔΒ();ΒЎ=DateTime.Now;}}Ζέ();}void Ζγ(Б Δ,String Χ){Δ.ΒЋ=DateTime.Now;String ų="";
У(ref Χ,out Δ.Αδ,BROADCAST_SEP);У(ref Χ,out Δ.Ϊ,BROADCAST_SEP);if(Δ.Αδ!=VERSION) return;String Ż="";String[]Ņ=new string[3];У(ref Χ,out Ż,BROADCAST_SEP);У(ref Χ,out Ņ[0],BROADCAST_SEP);У(ref Χ,out Ņ[1],BROADCAST_SEP);У(ref Χ,out Ņ[2],BROADCAST_SEP);У(ref Χ,out Δ.Γί,BROADCAST_SEP);
У(ref Χ,out Δ.Λ,BROADCAST_SEP);У(ref Χ,out ų,BROADCAST_SEP);У(ref Χ,out Δ.ΑΠ,BROADCAST_SEP);String Εν="",ΕΗ="";У(ref Χ,out Εν,BROADCAST_SEP);У(ref Χ,out ΕΗ,BROADCAST_SEP);Δ.Ϗ.Clear();while(true){String ΔЊ;if(!У(ref Χ,out ΔЊ,BROADCAST_SEP)) break;String[]ΑΔ=ΔЊ.Split('/');if(ΑΔ.Count()<3) continue;
int ύ=0;if(!int.TryParse(ΑΔ[2],out ύ)) continue;Δ.Ϗ.Add(new Ц(ΑΔ[0],ΑΔ[1],ύ,Й.Ĩ));}int ΕΚ=0;if(!int.TryParse(ų,out Δ.Ѐ)) Δ.Ѐ=0;if(!int.TryParse(Ż,out ΕΚ)) Δ.Π=Μ.ȡ;if(!float.TryParse(Ņ[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(Ņ[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(Ņ[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(Εν,out Δ.Ǥ)) Δ.Ǥ=0;if(!float.TryParse(ΕΗ,out Δ.Ǫ)) Δ.Ǫ=0;Δ.Π=(Μ) ΕΚ;Δ.Βψ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Γκ=0;for(int i=0;i<Δ.Ϗ.Count();i++) Δ.Γκ+=Δ.Ϗ[i].ύ;}Б Αψ(String Ǻ,String Ϊ){Ǻ=Ǻ.ToUpper();Ϊ=Ϊ.ToUpper();for(int i=0;i<П.Count;i++)
{if(Ǻ!=""&&П[i].Ǻ.ToUpper()==Ǻ) return П[i];if(Ϊ!=""&&П[i].Ϊ!=null&&П[i].Ϊ.ToUpper()==Ϊ) return П[i];}return null;}void ќ(Б Δ,String Њ){if(Δ==null){for(int i=0;i<П.Count;i++)
П[i].ĩ(Њ,true,false,false);ΒΣ(Њ,"*",Њ);}else{Δ.ĩ(Њ,true,false,false);ΒΣ(Њ,Δ.Ǻ,Њ);}}void ΖΙ(string Ж){if(Ж=="") return;var λ=Ж.ToUpper().Split(' ');λ.DefaultIfEmpty("");var Њ=λ.ElementAtOrDefault(0);var Ȋ=λ.ElementAtOrDefault(1);String ǻ="Invalid argument: "+Ж;if(ρ==Ξ.Ť){if(Ј!=null)
{switch(Њ){case "UP":{if(Ѐ<2) break;if(Ј.Ѐ==0){Ѐ=1;return;}ќ(Ј,"UP");return;}case "DOWN":{if(Ѐ<1) break;if(Ѐ==1){ќ(Ј,"MRES");break;}ќ(Ј,"DOWN");break;}case "APPLY":{if(Ѐ<2) break;ќ(Ј,"APPLY");return;}}}}switch(Њ){case "UP":this.ΑЍ(false);return;case "DOWN":this.ΑЌ(false);return;
case "APPLY":this.Ĥ(true);return;}switch(Њ){case "CLEAR":Εϊ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ȋ)){Λ="Usage: SENDALL <cmd>";return;}ќ(null,Ȋ);return;case "SEND":
if(Ж.Length<="SEND".Length+1){Λ="Usage: SEND <ship>:<cmd>";return;}ΖΤ(Ж.Remove(0,"SEND".Length+1));return;}Λ=ǻ;}void ΖΤ(String ŧ){if(ŧ=="") return;var λ=ŧ.Split(':');if(λ.Length!=2){Λ="Missing separator \":\"";return;}λ.DefaultIfEmpty("");String Ō=λ.ElementAtOrDefault(0).Trim();
Б Δ=Αψ("",Ō);if(Δ!=null) ќ(Δ,λ.ElementAtOrDefault(1).Trim());else Λ="Unknown ship: "+Ō;}void Εϊ(){П.Clear();Ј=null;β(Ξ.Main);Ѐ=0;}void ΖΡ(){List<IMyTerminalBlock>Ƈ=new List<IMyTerminalBlock>();ω.GetBlocksOfType(З,щ);ω.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",Ƈ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);З=њ(З,pamTag,true);ΔΥ(Ƈ);ΔΤ();Ć(З,setLCDFontAndSize,1.15f,false);Ć(Ǭ,setLCDFontAndSize,1.15f,true);String ΖА=ΓЊ(false);String ΕΞ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ю in З){if(Ю.CustomData==""){Ю.CustomData=ΓЊ(true);continue;}
if(!Ю.CustomData.Contains(ΖА))
if(!Ю.CustomData.Contains(ΕΞ)) Ю.CustomData=ΕΞ+Ю.CustomData;}}void Ζέ(){String ű="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<Ǭ.Count;i++)
Ǭ[i].WriteText(Γυ(Е.ΔΗ,"0",false));for(int i=0;i<З.Count;i++){Е Ё=Е.ȡ;String Ǖ="";ΖΘ(З[i],out Ё,out Ǖ);З[i].WriteText(Γυ(Ё,Ǖ,i==0));}Echo(ű);for(int i=0;i<Ď.Count;i++){IMyTextPanel Ю=Ď[i];String Χ=Ю.CustomData.ToUpper();if(Χ=="DEBUG") Ю.WriteText(""+"\n"+"");if(Χ=="INSTRUCTIONS") Ю.WriteText(ΓЉ());
}}String Ĥ(bool Η){int ΖІ=0;return Ĥ(Η,0,ref ΖІ,false,1);}String Ĥ(bool Η,int ΗΥ,ref int Ηβ,bool Δλ,int Ž){String Β="";String ŋ="——————————————————\n";String Ηρ="--------------------------------------------\n";if(Ј==null||ρ==Ξ.Main||ρ==Ξ.ΒΤ)
Β+="[PAM]-Controller | "+П.Count+" Connected ships"+"\n";else Β+=ΓЌ(Ј)+"\n";Β+=ŋ;int Α=0;int Ζ=Ѐ;Ĕ=0;if(Λ!=""){Ι(ref Β,Ζ,Α++,Η,Λ);}else if(ρ==Ξ.Main){if(Ј==null&&П.Count>=1) Ј=П[0];String Ѝ="";if(Ι(ref Ѝ,Ζ,Α++,Η," Send command to all")) β(Ξ.ΒΤ);int Ŋ=0;
for(int s=0;s<П.Count;s++){if(Δλ) Ѝ+="\n";Б Δ=П[s];if(Ζ==Α||Ζ==Α+1) Ј=Δ;if(Α==Ζ) Ŋ=Ѝ.Split('\n').Length-1;if(Ι(ref Ѝ,Ζ,Α++,Η," "+ΓЌ(Δ))){Ј=Δ;β(Ξ.Ť);Ј.ĩ("",false,true,false);}if(Α==Ζ) Ŋ=Ѝ.Split('\n').Length-1;if(Ι(ref Ѝ,Ζ,Α++,Η," "+(Δλ?"":"  ")+ΕЎ(Δ)))
{Ј=Δ;β(Ξ.ΔΦ);}}int Œ=Ž-2;Œ+=Math.Max(0,(ΗΥ-1))*Ž;Β+=ΒΡ(Œ,Ѝ,Ŋ,ref Ηβ);}else if(ρ==Ξ.Ƭ||ρ==Ξ.ΒΤ){Ξ Ζν=ρ==Ξ.Ƭ?Ξ.Ť:Ξ.Main;Б ι=null;if(ρ==Ξ.Ƭ) ι=Ј;if(Ι(ref Β,Ζ,Α++,Η," Back")){β(Ζν);}if(Ι(ref Β,Ζ,Α++,Η," [Stop] "+ħ(ι,"STOP"))){ќ(ι,"STOP");}if(Ι(ref Β,Ζ,Α++,Η," [Continue] "+ħ(ι,"CONT"))){ќ(ι,"CONT");}
if(Ι(ref Β,Ζ,Α++,Η," [Move home] "+ħ(ι,"HOMEPOS"))){ќ(ι,"HOMEPOS");}if(Ι(ref Β,Ζ,Α++,Η," [Move to job] "+ħ(ι,"JOBPOS"))){ќ(ι,"JOBPOS");}if(Ι(ref Β,Ζ,Α++,Η," [Full simulation] "+ħ(ι,"FULL"))){ќ(ι,"FULL");}if(ι!=null&&ι.Π==Μ.ΰ)
if(Ι(ref Β,Ζ,Α++,Η," [Undock] "+ħ(ι,"UNDOCK"))){ќ(ι,"UNDOCK");}}else if(ρ==Ξ.Ť){String Βμ="";if(Ι(ref Βμ,Ζ,Α++,Η," Back")){β(Ξ.Main);}Β+=Βμ.Substring(0,Βμ.Length-1).PadRight(36,' ');if(Ι(ref Β,Ζ,Α++,Η," Send cmd")){β(Ξ.Ƭ);}if(!Ј.ΔΖ()&&!Ј.ΑЈ()) Ĕ++;if(Ј.ΑЈ()) Β+="No answer received...";
else Β+=ī(Ј,Ј.ΑΠ);}else if(ρ==Ξ.ΔΦ){if(Ι(ref Β,Ζ,Α++,Η," Back")){β(Ξ.Main);}Β+=Ηρ;Β+=ī(Ј,ΓЋ(Ј));}if(!ΓΛ) Ѐ=Math.Min(Ĕ-1,Ѐ);return Β;}String ΓЌ(Б Δ){if(Δ.Αδ!=VERSION) return Δ.Ϊ+": Different version!";return Δ.Ϊ+": "+ī(Δ,Δ.Γί)+" "+Δ.Βψ+"m";}String ΕЎ(Б Δ){String s=ΒΝ("",І(Δ.Ǥ,Δ.Ǫ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ϗ.Count;i++){if(i>=5) break;Ц ς=Δ.Ϗ[i];s+=ΔΫ(Αξ(ΕЋ(ς)),3)+" "+ΓЃ(ς.ύ)+" ";}return s;}String ΕЋ(Ц ς){if(ς.ν.ToUpper()=="ORE"||ς.ν.ToUpper()=="INGOT"){String Δι=ΕЉ(ς.Ϊ.ToUpper());if(Δι!="") return Δι;}return ς.Ϊ;}String ī(Б Δ,String Β){if(Δ.ΔΖ()) return "No signal...("+ƞ((int)(DateTime.Now-Δ.ΒЋ).TotalSeconds)+")";
return Β;}String ΓЊ(bool ΗЅ){String Β="";if(ΗЅ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΖΘ(IMyTerminalBlock Ю,out Е Ё,out String Ǖ){bool Ė=true;String Ż=Ʃ(ƛ(Ю.CustomData.Split('\n'),"mode",ref Ė)).ToUpper();String[]λ=Ż.Split(':');String ź=λ.Length>0?λ[0].Trim():"";Ǖ=λ.Length>1?λ[1].Trim():"";
Ё=Е.ȡ;if(ź=="MAIN") Ё=Е.ΔΘ;else if(ź=="MAINX") Ё=Е.ΔΙ;else if(ź=="MENU") Ё=Е.ΑЋ;else if(ź=="INVENTORY") Ё=Е.ΑЇ;else if(ź=="DEBUG") Ё=Е.Ας;if(Ё==Е.ȡ) Ǖ=Ż;}String Γυ(Е Ё,String Ǖ,bool ΖЈ){int Ÿ=15;if(ΖЈ){Δν=ƻ;ƻ=0;Δξ=Ƽ;Ƽ=0;}String Β="";if(Ё==Е.ΔΘ){int Э=0;
Β=Ĥ(false,Δν,ref Ζψ,true,Ÿ);if(!int.TryParse(Ǖ,out Э)) return Β;ƻ=Math.Max(Э,ƻ);Э--;return ΔΜ(Э,Ÿ,Β);}else if(Ё==Е.ΔΙ){int Э=0;Β=Ĥ(false,Δξ,ref Ζω,false,Ÿ);if(!int.TryParse(Ǖ,out Э)) return Β;Ƽ=Math.Max(Э,Ƽ);Э--;return ΔΜ(Э,Ÿ,Β);}if(Ё==Е.ΔΗ){return Ĥ(false,0,ref Ζϊ,true,Ÿ);
}else if(Ё==Е.ΑЇ||Ё==Е.ΑЋ||Ё==Е.Ας){Б Δ=Ј;if(Ǖ!=""){Δ=Αψ("",Ǖ);if(Δ==null) return "Unknown ship: "+Ǖ;}else{if(Δ==null) return "No ship on main screen selected.";Β="Selected: ";}String ŋ="—————————————————————\n";String Γθ="";String Βο="";if(Ё==Е.ΑЋ){Γθ="Menu";Βο=ī(Δ,Δ.ΑΠ);}
else if(Ё==Е.ΑЇ){Γθ="Inventory";Βο=ī(Δ,ΓЋ(Δ));}else if(Ё==Е.Ας){return ī(Δ,ΕЏ(Δ));}Β+=Δ.Ϊ+" - "+Δ.Βψ+"m | "+Αξ(""+Γθ)+"\n"+ŋ;Β+=Βο;return Β;}return "Unknown command: "+Ǖ;}string ΕЏ(Б Δ){String s="";s+=Δ.Αδ+"\n";s+=Δ.Ϊ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.Γί+"\n";s+=Δ.Λ+"\n";s+=Δ.Ѐ+"\n";s+=Δ.ΑΠ.Length+"\n";s+=Δ.Ǥ+"\n";s+=Δ.Ǫ+"\n";s+=Δ.Ϗ.Count()+"\n";return s;}string ΓЋ(Б Δ){String s="";s+=ΒΝ("All",І(Δ.Ǥ,Δ.Ǫ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ϗ.Count();i++){Ц ς=Δ.Ϗ[i];
s+=ΒΝ(ς.Ϊ,ς.ύ,Δ.Γκ,30,8,10)+"\n";}return s;}Д ΕІ(Б Δ,bool Ŭ,string Њ){Д Ω=Д.None;if(Δ==null){for(int i=0;i<П.Count;i++){Д φ=П[i].ĩ(Њ,false,Ŭ,false);if(φ==Д.ΒΞ) Ω=φ;if(φ==Д.ΑБ) return φ;if(φ==Д.Ţ) return φ;}return Ω;}else return Δ.ĩ(Њ,false,Ŭ,false);}String ħ(Б Δ,String Њ)
{Д φ=ΕІ(Δ,false,Њ);if(φ==Д.ΒΞ) return "received!";if(φ==Д.Ţ) return "pending...";if(φ==Д.ΑБ) return "no answer!";return "";}String ΒΡ(int Œ,String Β,int Ŋ,ref int ȕ){String[]Κ=Β.Split('\n');if(Ŋ>=ȕ+Œ-1) ȕ++;ȕ=Math.Min(Κ.Count()-1-Œ,ȕ);if(Ŋ<ȕ+1) ȕ--;ȕ=Math.Max(0,ȕ);
String Ω="";for(int i=0;i<Œ;i++){int ΔА=i+ȕ;if(ΔА>=Κ.Count()) break;Ω+=Κ[ΔА]+"\n";}return Ω;}String ΔΜ(int Э,int Ž,String Β){String[]Κ=Β.Split('\n');int Ηκ=Э*Ž;int ΖЇ=(Э+1)*Ž;String Ω="";for(int i=Ηκ;i<ΖЇ;i++){if(i>=Κ.Count()) break;Ω+=Κ[i]+"\n";}return Ω;
}String Αξ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΔΫ(String s,int ΕΕ){if(s=="") return s;if(s.Length>ΕΕ) s=s.Substring(0,ΕΕ-1)+".";return s;}string ΒΝ(String Ē,float value,float ΓΖ,int Δε,int ΔЌ,int ΗΤ){float Ę=І(value,ΓΖ)*Δε;
String ƹ="[";for(int i=0;i<Δε;i++){if(i<=Ę) ƹ+="|";else ƹ+="'";}ƹ+="]";return ƹ+" "+ΔΫ(Αξ(Ē),ΔЌ).PadRight(ΔЌ)+"".PadRight(ΗΤ)+ΓЃ(value);}String ΓЃ(float ύ){if(ύ>=1000000) return Math.Round(ύ/1000000f,ύ/1000000f<100?1:0)+"M";if(ύ>=1000) return Math.Round(ύ/1000f,ύ/1000f<100?1:0)+"K";
return ""+Math.Round(ύ);}String ƞ(int Ɓ){if(Ɓ>=60*60) return Math.Round(Ɓ/(60f*60f),1)+" h";if(Ɓ>=60) return Math.Round(Ɓ/60f,1)+" min";return ""+Ɓ+" s";}string Ħ(float v){return Math.Round(v,2)+" ";}string ΗЌ(Vector3 v){return "X"+Ħ(v.X)+"Y"+Ħ(v.Y)+"Z"+Ħ(v.Z);}