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
public enum Λ{ȡ,ş,њ,к,ί}public enum Γ{л,ă,Š,ȟ,ą,ǜ,г,
Ă,Ř,ț,ŗ,Ŕ,Ȳ,
ΒΜ,ȉ,Ƞ,ĥ,Βέ,Αυ,
Βά,Ʊ,ƚ,Αχ,Ȣ}public enum Ρ{м,ΒΘ,А,Ασ,Ɩ,Ƣ,ƣ,Ġ,ġ}public enum й{None,ĥ,л,Γϊ}public enum Ν{Main,ΔΚ,Ş,ŝ,ΒΨ,Ɣ,
Αε,ΓЊ,ΓЋ,ΓЉ,ţ,ΔΟ,
ƪ,ΒΟ,ΒΣ,ΔΠ,Ƭ}public enum Ь{Ʃ,Ŗ,ΖΞ,ΓЎ,ί}public enum ǟ{ƭ,Γτ}public enum б{ƨ,Αψ,ΔΡ,Ś}public enum Ђ{ΑЏ,Γχ,Αο,Ατ,Ɨ,ΑЃ,ƞ}public enum ј{Αρ,Ακ,Αλ}public enum Й{Ĩ,ΕЌ,ȳ}public enum ζ{ΑБ,ΒΔ,ΒΕ,ΔΕ,ΔΔ,
ΑА,Ʀ,ƥ,ΒΒ,
ΒΓ,ΒΑ}public enum ή{Ǯ,Ȇ,ǭ,Ɠ,Αθ,Αι}public enum Ȉ{Αω,Αη,Εώ}public enum ȃ{ΔΥ,Γϋ,ΕБ,ΔΓ}public enum Д{None,š,ΒΛ,ΑЎ}public enum π{Ť,ƒ,Ơ,Ś,ΑЌ,ơ,ΑЍ,ΖΡ,ΒΖ}public enum Е{ȡ,ΔΑ,ΓБ,ΑЉ,ΑЅ,Απ,ΔΒ}class ά{public bool γ=false;public Vector3 Ε=new Vector3();
public Vector3 β=new Vector3();public Vector3 ΰ=new Vector3();public Vector3 ω=new Vector3();public Vector3 χ=new Vector3();public Vector3 о=new Vector3();public float Х=0;public float ΗЇ=0;public float[]Ǚ=null;public ά(){}public ά(ά ǽ){γ=ǽ.γ;Ε=ǽ.Ε;β=ǽ.β;ΰ=ǽ.ΰ;ω=ǽ.ω;χ=ǽ.χ;
о=ǽ.о;Ǚ=ǽ.Ǚ;}public ά(Vector3 Ε,Vector3 ΰ,Vector3 β,Vector3 ω,Vector3 χ){this.Ε=Ε;this.β=β;this.ΰ=ΰ;this.ω=ω;this.Х=0;this.χ=χ;}public void Επ(List<IMyThrust>ƴ,List<string>Ǿ){Ǚ=new float[Ǿ.Count];for(int i=0;i<Ǚ.Length;i++)
Ǚ[i]=-1;for(int i=0;i<ƴ.Count;i++){string ч=ΓЇ(ƴ[i]);int Α=Ǿ.IndexOf(ч);if(Α!=-1)
Ǚ[Α]=І(ƴ[i].MaxEffectiveThrust,ƴ[i].MaxThrust);}}}class ΒΗ{public ά ι=null;public List<Vector3>ō=new List<Vector3>();public float ŭ=0;public float Γι=0;public float İ=0;public float Βχ=0;public Vector3 ǂ=new Vector3();}class Ĭ{public ζ Г=ζ.ΑБ;public float џ=0;public float ȧ=0;
public string ǎ="";public string Ǒ="";DateTime ΔЉ;public bool ΓΞ=false;private bool Βπ=false;public bool Αν(bool ǖ){if(ǖ){ȧ=0;Βπ=false;return false;}Βπ=true;return ȧ>џ;}public void ΖΓ(){Αν(true);ΓΞ=false;}public void Ζέ(){if(Βπ)
if((DateTime.Now-ΔЉ).TotalSeconds>1){ȧ++;ΔЉ=DateTime.Now;}}public bool ΑЁ(){switch(Г){case ζ.ΒΔ:return true;case ζ.ΒΕ:return true;}return false;}}class Ц{public String Ψ="";public String ν="";public int ό=0;public Й Ľ=Й.ȳ;public Ц(String Ψ,String ν,int ό,Й Ľ){this.Ψ=Ψ;this.ν=ν;
this.ό=ό;this.Ľ=Ľ;}}class Εϋ{public Εϋ(Vector3 И,float Ћ){this.И=И;this.Ћ=Ћ;}public Vector3 И;public float Ћ;}class Б{public DateTime ΒІ;public DateTime Δζ;public String ǹ="";public String Ψ="";public String Αγ="";public String Π="";public String ΓΪ="";public String ΑΞ="";public String ΓΝ="";
public Vector3 Ε=new Vector3();public List<Ц>Ϗ=new List<Ц>();public Λ Ο=Λ.ȡ;public Д Ȥ;public float ǩ;public float ǣ;public int Ѐ;public int Βσ=0;public int Γε=0;public bool ΓА(){return(DateTime.Now-ΒІ).TotalSeconds>10;}public bool ΑІ(){if(Ȥ!=Д.š) return false;return(DateTime.Now-Δζ).TotalSeconds>=4;
}public Б(String ǹ){this.ǹ=ǹ;}public Д ĩ(String Њ,bool Ηέ,bool ū,bool Ζκ){if(Њ==""&&!ū) return Д.None;if(Ȥ==Д.š&&ΑІ())
Ȥ=Д.ΑЎ;if(Ηέ){ΓΝ=Њ;Ȥ=Д.š;Δζ=DateTime.Now;return Ȥ;}else if(ū){ΓΝ="";Ȥ=Д.None;}else if(ΓΝ==Њ){if(Ζκ) Ȥ=Д.ΒΛ;return Ȥ;}return Д.None;}}public String ΕЃ(String ΖЎ){switch(ΖЎ){case "IRON":return "Fe";case "NICKEL":return "Ni";case "COBALT":return "Co";case "MAGNESIUM":return "Mg";
case "SILICON":return "Si";case "SILVER":return "Ag";case "GOLD":return "Au";case "PLATINUM":return "Pt";case "URANIUM":return "U";default:return "";}}IMyGridTerminalSystem ώ;Vector3 ΑΦ=new Vector3();Vector3 μ=new Vector3();Vector3 ǅ=new Vector3();Vector3 ΑΔ=new Vector3();Vector3 ǁ=new Vector3();
DateTime ΒЉ=new DateTime();bool ȍ=true;int Γΰ=0;bool ȑ=false;bool ΑΑ=false;bool ΑΣ=true;bool Εμ=false;bool Ļ=false;bool Ǌ=false;float ě=0;float Ě=0;int ȯ=0;int ƌ=0;int ž=0;int Βμ=0;float Ŀ=0;float ı=0;double ΓΔ=0;float Ũ=0;List<int>Ĺ=new List<int>();List<int>ņ=new List<int>();
Λ Ο=Λ.ȡ;int Є=0;float Ș=0;float Ȁ=0;float ψ=0;IMyRemoteControl Φ;IMySensorBlock ξ;List<IMyTimerBlock>Γβ=new List<IMyTimerBlock>();List<IMyShipConnector>п=new List<IMyShipConnector>();List<IMyThrust>ǿ=new List<IMyThrust>();List<IMyGyro>ǆ=new List<IMyGyro>();List<IMyTerminalBlock>ο=new List<IMyTerminalBlock>();
List<IMyLandingGear>ǻ=new List<IMyLandingGear>();List<IMyReactor>Ȯ=new List<IMyReactor>();List<IMyConveyorSorter>ΑΫ=new List<IMyConveyorSorter>();List<IMyGasTank>э=new List<IMyGasTank>();List<IMyTerminalBlock>ť=new List<IMyTerminalBlock>();List<IMyTerminalBlock>ĳ=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ċ=new List<IMyTerminalBlock>();List<IMyBatteryBlock>ȣ=new List<IMyBatteryBlock>();List<IMyTextPanel>З=new List<IMyTextPanel>();List<IMyTextSurface>ǫ=new List<IMyTextSurface>();List<IMyTextPanel>Ď=new List<IMyTextPanel>();IMyTerminalBlock ѐ=null;ά Μ=new ά();ά ΒЁ=new ά();
ά ΒЄ=new ά();bool ȁ=false;bool ŏ=false;bool Ő=false;bool ǌ=false;List<String>Ǿ=new List<string>();List<ά>Χ=new List<ά>();List<ά>Ɛ=new List<ά>();int Βτ=0;double Εν=0;ά Θ=new ά();Vector3 Ǽ;Vector3 ĕ;Ρ δ=Ρ.м;ǟ ΑΓ=ǟ.ƭ;int Ч=0;int с=0;double Ŷ=0;bool ƾ=false;Ĭ ϊ=new Ĭ();
Ĭ ϋ=new Ĭ();б ж=б.ƨ;bool Ȱ=true;ǟ Ȗ=ǟ.ƭ;int ы=3;int ъ=3;int д=30;ј Ǎ=ј.Αρ;bool Ț=true;bool ĺ=true;bool Ĵ=true;bool Ĝ=false;Ђ С=Ђ.ΑЏ;bool ș=true;float Ǩ=90;float т=20;float Ш=5;float у=20;float Ǡ=0.70f;float œ=1.50f;float Œ=2.50f;float Ƒ=0f;float ĸ=0f;Γ Ϊ;
Ρ ю;Γ ƃ;ΒΗ θ=null;int τ=0;int σ=0;int р=0;int Ȭ=0;int ę=30;int Ǖ=0;bool Џ=true;Vector3 ф;double Р=0;bool ΑΨ=false;int Γμ=0;int Љ=0;int Βδ=0;int ė=0;int ΕΛ=0;Vector3 ż=new Vector3();float е=0;float Щ=0;int ğ=0;ά Αά=null;float ΒЇ=0;float ƶ=0;float Ф=0;float ŀ=0;
bool ΒЂ=false;bool ĝ=false;bool ƍ=false;bool ȏ=false;bool ȋ=false;DateTime Ζό=new DateTime();ά Č=null;ά Δλ=null;float Ǵ=0;й ǡ;float Ȑ=0;float Ğ=0;String ΒЏ="";float ΑΕ=0;float ǣ=0;float ǩ=0;List<Ц>ǐ=new List<Ц>();float Т=0;bool Ɓ=false;bool Εγ=false;bool ΕΥ=false;
bool Αή=false;bool ΕΦ=false;float ΓΒ=0;float ΓΜ=0;Vector3 Δϊ=new Vector3();Vector3 Ѓ=new Vector3();Vector3 Αί=new Vector3(1,1,1);float Γί=1;Vector3 ƈ=new Vector3();bool ǋ=false;bool Βύ=false;bool ΒЌ=false;bool ΔΪ=false;float Ƽ=0;float Ƴ=2;Vector3 Εβ;Vector3 Γή;Vector3 Γέ;
float[,]ǚ=new float[3,2];Dictionary<String,float[,]>є=new Dictionary<string,float[,]>();Dictionary<String,float>Εκ=new Dictionary<String,float>();int Βθ=0;Dictionary<String,float[]>Ȕ=new Dictionary<string,float[]>();float ΓΟ;bool ƿ=false;int Ƶ=0;int Ȫ=0;int ǈ=0;int Đ=0;
String Π="";int[]ΕΔ=new int[Enum.GetValues(typeof(Ν)).Length];bool ΓΖ=false;int Ĕ=0;int Ѐ=0;Ν ρ=Ν.Main;int Ημ=0,Ην=0,Ηξ=0,Ηο=0;int ΗΨ=0,ΗΩ=0;bool Ĳ=false;bool ΔΫ=false;String ŋ="";String Βλ="";IMyBroadcastListener ќ=null;bool Δω=true;List<Б>П=null;Б Ј=null;
int ƹ=0,ƺ=0;int Δι=0,Δκ=0;int Ζπ=0,Ζρ=0,Ζς=0;String ΖА="";Exception Ȓ=null;bool ΑΧ=false;Ρ ň=Ρ.м;bool Δξ=false;bool ΔЇ=true;bool ΓΤ=false;String Ελ="";bool Ƌ=false;bool ΓΩ=true;int ǉ=0;int[]ђ=null;Vector3 Βν=new Vector3();bool Δό=false;bool ш(IMyTerminalBlock Υ)=>Υ.CubeGrid==Me.CubeGrid;
Program(){ώ=GridTerminalSystem;Runtime.UpdateFrequency=UpdateFrequency.Update10;if(ΑЂ(Me,controllerTag,true))
Ο=Λ.к;α(Ν.Main);if(Ο!=Λ.к){Π="Welcome to [PAM]!";ȃ ΓΣ=ΖΔ();if(Ο==Λ.ȡ){List<IMyShipDrill>Δφ=new List<IMyShipDrill>();List<IMyShipGrinder>Δύ=new List<IMyShipGrinder>();ώ.GetBlocksOfType(Δφ,q=>q.CubeGrid==Me.CubeGrid);ώ.GetBlocksOfType(Δύ,q=>q.CubeGrid==Me.CubeGrid);if(Δφ.Count>0)
{Ο=Λ.ş;Π="Miner mode enabled!";}else if(Δύ.Count>0){Ο=Λ.њ;Π="Grinder mode enabled!";}else{Ο=Λ.ί;ǲ(Ь.ί);}}if(ΓΣ==ȃ.ΔΥ) ΑΣ=false;if(ΓΣ==ȃ.Γϋ) Π="Data restore failed!";if(ΓΣ==ȃ.ΔΓ) Π="New version, wipe data";}}void Main(string Ж,UpdateType Ηφ){try{if(Ȓ!=null){if(Ж.Trim().ToUpper()=="RETRY")
{Ȓ=null;ȍ=true;ȯ=0;if(Ο!=Λ.к) φ();Π="Restarted after error";}else{ΕЋ();ΔΘ();return;}}ΑΑ=(Ηφ&UpdateType.Update10)!=0;if(ΑΑ) Γΰ++;ȑ=Γΰ>=10;if(ȑ) Γΰ=0;if(ΑΑ){Ƶ++;if(Ƶ>4) Ƶ=0;ž=(int) Math.Max(0,10-(DateTime.Now-ΒЉ).TotalSeconds);}if(Ж!="") ΖА=Ж;if(Ο!=Λ.к)
ΖΝ(Ж);else
Εω(Ж);ΓΖ=false;try{int Δμ=Runtime.CurrentInstructionCount;float Ę=І(Δμ,Runtime.MaxInstructionCount);if(Ę>0.90) Π="Max. instructions >90%";if(Ę>Ŀ) Ŀ=Ę;if(ƿ){Ĺ.Add(Δμ);while(Ĺ.Count>10) Ĺ.RemoveAt(0);ı=0;for(int i=0;i<Ĺ.Count;i++) ı+=Ĺ[i];ı=І(І(ı,Ĺ.Count),Runtime.MaxInstructionCount);
double ΒЈ=Runtime.LastRunTimeMs;if(Ǌ&&ΒЈ>ΓΔ) ΓΔ=ΒЈ;ņ.Add((int)(ΒЈ*1000f));while(ņ.Count>10) ņ.RemoveAt(0);Ũ=0;for(int i=0;i<ņ.Count;i++) Ũ+=ņ[i];Ũ=І(Ũ,ņ.Count)/1000f;}}catch{Ŀ=0;}}catch(Exception e){Ȓ=e;}}void ΖΝ(string Ж){bool ΑΩ=false;String Δγ="";if(Є<=1&&!ΑЇ(Ж))
ΔΙ(Ж);int ΓΗ=50;while(ќ!=null&&ќ.HasPendingMessage&&ΓΗ-->0){MyIGCMessage κ=ќ.AcceptMessage();String ε=κ.Data as String;if(ε==null) continue;String Ǫ="";if(Є<=1&&ΔΗ(ref ε,out Ǫ,out Δγ)&&Ǫ==BROADCAST_SELF_ID){ΔΙ(ε);ΑΩ=true;break;}}bool ΔЃ=Ǌ&&Ϊ==Γ.л&&!Ļ&&!ΑΩ&&ȯ==0&&!ǌ;
if(ȑ&&Ϊ!=Γ.л) ΑΩ=true;if((ΑΑ&&!ΔЃ)||(ȑ&&ΔЃ)){if(ȯ==0&&(ž<=0||ȍ)){Εμ=Є>0;Є=0;ȯ=1;ǳ();ΖΗ();ΓЌ();ǯ("Scan 1");}else if(ȯ==1){ȯ=2;ǳ();ΖΘ();ǯ("Scan 2");}else if(ȯ==2){ȯ=0;ǳ();ΖΙ();ǯ("Scan 3");ΒЉ=DateTime.Now;if(Є<=1&&ΑΣ)
ΑΦ=Αϋ(Φ,Φ.CenterOfMass);ΑΣ=false;if(ȍ){ȍ=false;bool Ηϊ=δ==Ρ.А;φ();if(ΔЇ&&resumeAfterRestart&&Ηϊ&&Є<=1) ΓΤ=true;}ΔЇ=false;if(Εμ&&Є==0) Π="Setup complete";}else{if(δ==Ρ.А&&Ο!=Λ.ί){ǳ();Εξ();ǯ("Inv balance");}ǳ();switch(ƌ){case 0:ΖΥ();break;case 1:ΖΩ();break;case 2:ΖΤ();break;
case 3:Ζή();break;case 4:ΖΨ();break;case 5:Εσ();break;case 6:Ες(Φ);break;}ǯ("Update: "+ƌ);ƌ++;if(ƌ>6){ƌ=0;Ǌ=true;if(ň!=Ρ.м){switch(ň){case Ρ.Ġ:ȝ();break;case Ρ.ġ:ȝ();break;case Ρ.А:ȝ();break;case Ρ.Ƣ:ƛ();break;case Ρ.ƣ:ā();break;}ň=Ρ.м;}}}if(!ȍ){if(!Ǳ(Φ,true)){Φ=null;
ΑΣ=true;Є=2;}if(Є>=2&&Ϊ!=Γ.л)
φ();if(Є<=1){Ě=Φ.CalculateShipMass().PhysicalMass;ě=(float) Φ.GetShipSpeed();μ=ΖΣ(Φ,ΑΦ);ǅ=Φ.WorldMatrix.Forward;ΑΔ=Φ.WorldMatrix.Left;ǁ=Φ.WorldMatrix.Down;ΖΫ();if(ΓΤ){ΓΤ=false;ȝ();if(Ϊ!=Γ.л) Π="Job resumed after restart";}if(Ϊ!=Γ.л){Ļ=false;ΔΜ(false);Γς(false);String Ηγ=Αύ(Ϊ)+" "+(int) Ϊ;
ǳ();ΖΪ();ΒΫ(false);ǯ(Ηγ);ǳ();ΖΧ();ǯ("Thruster");ǳ();ΔΧ();ǯ("Gyroscope");}else{if(Ļ){if(Ɵ()){Ъ(ǁ,ǅ,ΑΔ,0.25f,true);ΔΧ();Π="Aligning to planet: "+Math.Round(Ƽ-0.25f,2)+"°";if(ǋ) ΑЀ(true,true);}else ΑЀ(true,true);}}Ƌ=false;}}}ǳ();ΔΘ();ǯ("Print");if(ΑΩ||Βμ<=0){ǳ();Εο(Δγ);ǯ("Broadcast");
Βμ=4;}else if(ȑ) Βμ--;}void ΔΙ(string Ж){if(Ж=="") return;var λ=Ж.ToUpper().Split(' ');λ.DefaultIfEmpty("");var Њ=λ.ElementAtOrDefault(0);var Ȋ=λ.ElementAtOrDefault(1);var ŧ=λ.ElementAtOrDefault(2);var Δέ=λ.ElementAtOrDefault(3);String Ǻ="Invalid argument: "+Ж;bool Γη=false;switch(Њ)
{case "UP":this.ΑЋ(false);break;case "DOWN":this.ΑЊ(false);break;case "UPLOOP":this.ΑЋ(true);break;case "DOWNLOOP":this.ΑЊ(true);break;case "APPLY":this.Γρ(true);break;case "MRES":Ѐ=0;break;case "STOP":this.φ();break;case "PATHHOME":{this.φ();this.ΒΧ();}break;case "PATH":{this.φ();this.ΒΧ();Μ.γ=true;}break;
case "START":{this.φ();ΒΦ();}break;case "ALIGN":{ΑЀ(!Ļ,false);}break;case "CONT":{this.φ();this.ȝ();}break;case "JOBPOS":{this.φ();this.ā();}break;case "HOMEPOS":{this.φ();this.ƛ();}break;case "FULL":{Ɓ=true;}break;case "RESET":{ΑΧ=true;Є=2;}break;default:Γη=true;break;
}if(Ο!=Λ.ί){switch(Њ){case "SHUTTLE":{Εό();}break;case "CFGS":{if(!Εχ(Ȋ,ŧ,Δέ)) Π=Ǻ;}break;case "CFGB":{if(!Εφ(Ȋ,ŧ)) Π=Ǻ;}break;case "CFGL":{if(!Ą(ref Ǩ,true,π.Ơ,Ȋ,"")||!Εψ(ŧ))
Π=Ǻ;}break;case "CFGE":{if(!Ą(ref Ш,true,π.ơ,Ȋ,"IG")||
!Ą(ref т,true,π.ΑЌ,ŧ,"IG")||
!Ą(ref у,true,π.ΑЍ,Δέ,"IG"))
Π=Ǻ;}break;case "CFGA":{if(!Ą(ref Ǡ,false,π.ƒ,Ȋ,"")) Π=Ǻ;}break;case "CFGW":{if(!Ą(ref œ,false,π.Ť,Ȋ,"")||
!Ą(ref Œ,false,π.Ť,ŧ,""))
Π=Ǻ;}break;case "NEXT":{Αδ(false);}break;case "PREV":{Αδ(true);}break;default:if(Γη) Π=Ǻ;break;}}else{switch(Њ){case "UNDOCK":{Ƌ=true;}break;default:if(Γη) Π=Ǻ;break;}}}String ΕЄ(){String Β="\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n\n"+
"[UPLOOP] \"UP\" + looping\n"+"[DOWNLOOP] \"DOWN\" + looping\n"+
"[PATHHOME] Record path, set home\n"+"[PATH] Record path, use old home\n"+
"[START] Start job\n"+"[STOP] Stop every process\n"+"[CONT] Continue last job\n"+
"[JOBPOS] Move to job position\n"+"[HOMEPOS] Move to home position\n\n"+
"[FULL] Simulate ship is full\n"+"[ALIGN] Align the ship to planet\n"+"[RESET] Reset all data\n";if(Ο!=Λ.ί)
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
Β+="[UNDOCK] Leave current connector\n\n";return Β;}void Εό(){φ();Ο=Λ.ί;ǲ(Ь.ί);Θ.γ=false;Μ.γ=false;ξ=null;ο.Clear();δ=Ρ.м;}void ΑЀ(bool Ќ,bool Ǥ){if(!Ќ) Π="Aligning canceled";if(Ǥ) Π="Aligning done";if(Ǥ||!Ќ){Ļ=false;ΔΣ();ƫ(false,0,0,0,0);return;}if(Ɵ()) Ļ=true;}bool Εχ(String Ηό,String ΖЇ,String Ζχ)
{bool Ηψ=δ==Ρ.А;int w,h,d;if(int.TryParse(Ηό,out w)&&int.TryParse(ΖЇ,out h)&&int.TryParse(Ζχ,out d)){this.φ();ы=w;ъ=h;д=d;ĉ(false);ư(false,false);if(Ηψ) ȝ();return true;}return false;}bool Εψ(String Ŧ){if(Ŧ=="ON"){Ț=true;return true;}if(Ŧ=="OFF"){Ț=false;return true;}
return false;}bool Εφ(String Δυ,String ƽ){bool Ė=true;if(Δυ=="HOME") Ȱ=true;else if(Δυ=="STOP") Ȱ=false;else Ė=false;if(ƽ=="HOME") ж=б.ƨ;else if(ƽ=="STOP") ж=б.ΔΡ;else if(ƽ=="JOB") ж=б.Αψ;else if(ƽ=="IG") ж=б.Ś;else Ė=false;return Ė;}void α(Ν Э){ΕΔ[(int) ρ]=Ѐ;Ѐ=ΕΔ[(int) Э];
if(Э==Ν.ţ) Ѐ=0;ρ=Э;if(Ο!=Λ.к)
Ĉ(ρ==Ν.Ş,false,0,0);ΓΖ=true;}void ΑЋ(bool ΒЍ){if(Ѐ>0) Ѐ--;else if(ΒЍ) Ѐ=Ĕ-1;}void ΑЊ(bool ΒЍ){if(Ѐ<Ĕ-1) Ѐ++;else if(ΒЍ) Ѐ=0;}String ǲ(Ь κ){switch(κ){case Ь.Ʃ:Π="Job is running";break;case Ь.Ŗ:Π="Connector not ready!";break;case Ь.ΖΞ:Π="Ship modified, path outdated!";break;
case Ь.ΓЎ:Π="Interrupted by player!";break;case Ь.ί:Π="Shuttle mode enabled!";break;}return "";}String ΕЊ(ǟ И){switch(И){case ǟ.ƭ:return "Top-Left";case ǟ.Γτ:return "Center";default:return "";}}String ΕЁ(ј Ё){switch(Ё){case ј.Ακ:return "Auto"+(Ο==Λ.ş?" (Ore)":"");case ј.Αλ:return "Auto (+Stone)";
case ј.Αρ:return "Default";default:return "";}}String Αϊ(Ρ υ){switch(υ){case Ρ.м:return "No job";case Ρ.ΒΘ:return "Job paused";case Ρ.А:return "Job active";case Ρ.Ġ:return "Job active";case Ρ.ġ:return "Job active";case Ρ.Ασ:return "Job done";case Ρ.Ɩ:return "Job changed";case Ρ.Ƣ:return "Move home";
case Ρ.ƣ:return "Move to job";}return "";}String ΓϏ(б Ζι){switch(Ζι){case б.ƨ:return "Return home";case б.Αψ:return "Fly to job pos";case б.ΔΡ:return "Stop";case б.Ś:return "Ignore";}return "";}String ΕЂ(Ђ Ё){switch(Ё){case Ђ.ΑЏ:return "Off";case Ђ.Ατ:return "Drop pos (Stone) ";case Ђ.Ɨ:return "Drop pos (Sto.+Ice)";
case Ђ.Γχ:return "Cur. pos (Stone)";case Ђ.Αο:return "Cur. pos (Sto.+Ice)";case Ђ.ΑЃ:return "In motion (Stone)";case Ђ.ƞ:return "In motion (Sto.+Ice)";}return "";}String Γύ(й υ){switch(υ){case й.None:return "No batteries";case й.ĥ:return "Charging";case й.Γϊ:return "Discharging";}return "";}String Αύ(Γ υ)
{String ΗЅ=Ο==Λ.ί?"target":"job";switch(υ){case Γ.л:return "Idle";case Γ.ă:return "Flying to XY position";case Γ.Š:return Ο==Λ.њ?"Grinding":"Mining";case Γ.ȟ:return "Returning";case Γ.ƚ:return "Flying to drop pos";case Γ.ą:return "Returning to dock";case Γ.ǜ:return "Flying to dock area";
case Γ.г:return "Flying to job area";case Γ.Ă:return "Flying to path";case Γ.Ř:return "Flying to job position";case Γ.ț:return "Approaching dock";case Γ.ŗ:return "Docking";case Γ.Ŕ:return "Aligning";case Γ.Ȳ:return "Aligning";case Γ.ΒΜ:return "Retry docking";case Γ.ȉ:return "Unloading";case Γ.Ȣ:return Ελ;
case Γ.Ƞ:return "Undocking";case Γ.ĥ:return "Charging batteries";case Γ.Βέ:return "Waiting for uranium";case Γ.Αυ:return "Filling up hydrogen";case Γ.Βά:return "Waiting for ejection";case Γ.Ʊ:return "Waiting for ejection";case Γ.Αχ:return "Flying to drop pos";}return "";}String ΓЈ(ζ Г){switch(Г)
{case ζ.ΑБ:return "On \"Undock\" command";case ζ.ΒΔ:return "On player entered cockpit";case ζ.ΔΕ:return "On ship is full";case ζ.ΔΔ:return "On ship is empty";case ζ.ΒΕ:return "On time delay";case ζ.Ʀ:return "On batteries empty(<25%)";case ζ.ƥ:return "On batteries empty(=0%)";case ζ.ΑА:return "On batteries full";
case ζ.ΒΓ:return "On hydrogen empty(<25%)";case ζ.ΒΑ:return "On hydrogen empty(=0%)";case ζ.ΒΒ:return "On hydrogen full";}return "";}bool Ι(ref String Β,int ΕΡ,int ȩ,bool Η,String Ē){Ĕ+=1;if(ΕΡ==ȩ) Ē=">"+Ē+(Ƶ>=2?" .":"");else Ē=" "+Ē;Β+=Ē+"\n";return ΕΡ==ȩ&&Η;}String Γρ(bool Η)
{int Α=0;int Ζ=Ѐ;Ĕ=0;String Ŋ="———————————————\n";String Ŏ="--------------------------------------------\n";String Β="";Β+=Αϊ(δ)+" | "+(Μ.γ?"Ready to dock":"No dock")+"\n";Β+=Ŋ;double Ɔ=Math.Max(Math.Round(this.Р),0);if(ρ==Ν.Main){bool ȫ=Ο==Λ.ί;if(Ι(ref Β,Ζ,Α++,Η," Record path & set home")) ΒΧ();
if(Ο==Λ.ş)
if(Ι(ref Β,Ζ,Α++,Η," Setup mining job")) α(Ν.Ş);if(Ο==Λ.њ)
if(Ι(ref Β,Ζ,Α++,Η," Setup grinding job")) α(Ν.Ş);if(Ο==Λ.ί)
if(Ι(ref Β,Ζ,Α++,Η," Setup shuttle job")) α(Ν.ΒΣ);if(Ι(ref Β,Ζ,Α++,Η," Continue job")) ȝ();if(Ι(ref Β,Ζ,Α++,Η," Fly to home position")) ƛ();if(Ι(ref Β,Ζ,Α++,Η," Fly to job position")) ā();if(Ι(ref Β,Ζ,Α++,Η," Behavior settings"))
if(ȫ) α(Ν.Ƭ);else α(Ν.Ɣ);if(Ι(ref Β,Ζ,Α++,Η," Info")) α(Ν.ΓЊ);if(Ο!=Λ.ί)
if(Ι(ref Β,Ζ,Α++,Η," Help")) α(Ν.ΓЉ);}else if(ρ==Ν.Ş){double Ηϋ=Math.Round(ы*Ș,1);double ΖІ=Math.Round(ъ*Ȁ,1);String Ѕ="";if(Ι(ref Ѕ,Ζ,Α++,Η," Start new job!")) ΒΦ();if(Ι(ref Ѕ,Ζ,Α++,Η," Change current job")){ư(false,false);α(Ν.Main);}if(Ι(ref Ѕ,Ζ,Α++,Η," Width + (Width: "+ы+" = "+Ηϋ+"m)")){Ģ(ref ы,5,20,1);ĉ(true);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Width -")){Ģ(ref ы,-5,20,-1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Height + (Height: "+ъ+" = "+ΖІ+"m)")){Ģ(ref ъ,5,20,1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Height -")){Ģ(ref ъ,-5,20,-1);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Depth + ("+(Ǎ==ј.Αρ?"Depth":"Min")+": "+д+"m)")){Ģ(ref д,5,50,2);ĉ(true);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Depth -")){Ģ(ref д,-5,50,-2);ĉ(true);}if(Ι(ref Ѕ,Ζ,Α++,Η," Depth mode: "+ΕЁ(Ǎ))){Ǎ=Ȟ(Ǎ);}if(Ι(ref Ѕ,Ζ,Α++,Η," Start pos: "+ΕЊ(Ȗ))){Ȗ=Ȟ(Ȗ);}if(Ο==Λ.њ&&Ǎ==ј.Αλ) Ǎ=Ȟ(Ǎ);Β+=ΒΝ(8,Ѕ,Ζ,ref ΗΨ);}else if(ρ==Ν.ΒΣ){float[]Δπ=new float[]{0,3,10,30,60,300,600,1200,1800};
if(Ι(ref Β,Ζ,Α++,Η," Next")) α(Ν.ΔΠ);if(Ι(ref Β,Ζ,Α++,Η," Back")) α(Ν.Main);Β+=" Leave connector 1:\n";if(Ι(ref Β,Ζ,Α++,Η," - "+ΓЈ(ϊ.Г))) ϊ.Г=Ȟ(ϊ.Г);if(!ϊ.ΑЁ()) Β+="\n";else if(Ι(ref Β,Ζ,Α++,Η," - Delay: "+Ɯ((int) ϊ.џ))) ϊ.џ=Γψ(ϊ.џ,Δπ);Β+=" Leave connector 2:\n";if(Ι(ref Β,Ζ,Α++,Η," - "+ΓЈ(ϋ.Г))) ϋ.Г=Ȟ(ϋ.Г);
if(!ϋ.ΑЁ()) Β+="\n";else if(Ι(ref Β,Ζ,Α++,Η," - Delay: "+Ɯ((int) ϋ.џ))) ϋ.џ=Γψ(ϋ.џ,Δπ);}else if(ρ==Ν.ΔΠ){if(Ι(ref Β,Ζ,Α++,Η," Start job!")) ΒΦ();if(Ι(ref Β,Ζ,Α++,Η," Back")) α(Ν.ΒΣ);Β+=" Timer: \"Docking connector 1\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϊ.ǎ!=""?ϊ.ǎ:"-"))) ϊ.ǎ=Ƥ(ref Ημ);
Β+=" Timer: \"Leaving connector 1\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϊ.Ǒ!=""?ϊ.Ǒ:"-"))) ϊ.Ǒ=Ƥ(ref Ηξ);Β+=" Timer: \"Docking connector 2\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϋ.ǎ!=""?ϋ.ǎ:"-"))) ϋ.ǎ=Ƥ(ref Ην);Β+=" Timer: \"Leaving connector 2\":\n";if(Ι(ref Β,Ζ,Α++,Η," = "+(ϋ.Ǒ!=""?ϋ.Ǒ:"-"))) ϋ.Ǒ=Ƥ(ref Ηο);
}else if(ρ==Ν.ŝ){String ΔЍ=Ǩ+" %";if(ȑ) Ȫ++;if(Ȫ>1){Ȫ=0;ǈ++;if(ǈ>1) ǈ=0;}bool[]ΓΦ=new bool[]{Ȯ.Count==0,ǡ==й.None,э.Count==0};int Εζ=0;while(true){Εζ++;Đ++;if(Đ>ΓΦ.Length-1) Đ=0;if(Εζ>=ΓΦ.Length) break;if(!ΓΦ[Đ]) break;}bool ȫ=Ο==Λ.ί;if(!ȫ&&Ț&&Т!=-1&&ǈ==0)
ΔЍ=Т<1000000?Math.Round(Т)+" Kg":Math.Round(Т/1000)+" t";if(Ι(ref Β,Ζ,Α++,Η," Stop!")){φ();α(Ν.Main);}if(Ι(ref Β,Ζ,Α++,Η," Behavior settings"))
if(!ȫ) α(Ν.Ɣ);else α(Ν.Ƭ);if(!ȫ){if(Ι(ref Β,Ζ,Α++,Η," Next hole")) Αδ(false);}else if(Ι(ref Β,Ζ,Α++,Η," Undock")) Ƌ=true;Β+=Ŏ;if(!ȫ) Β+="Progress: "+Math.Round(Ŷ,1)+" %\n";Β+="State: "+Αύ(Ϊ)+" "+Ɔ+"m \n";Β+="Load: "+ΑΕ+" % Max: "+ΔЍ+" \n";if(Đ==0) Β+="Uranium: "+(Ȯ.Count==0?"No reactors":Math.Round(Ğ,1)+"Kg "+(Ш==-1?"":" Min: "+Ш+" Kg"))+"\n";
if(Đ==1) Β+="Battery: "+(ǡ==й.None?Γύ(ǡ):Ǵ+"% "+(т==-1||ȫ?"":" Min: "+т+" %"))+"\n";if(Đ==2) Β+="Hydrogen: "+(э.Count==0?"No tanks":Math.Round(Ȑ,1)+"% "+(у==-1||ȫ?"":" Min: "+у+" %"))+"\n";}else if(ρ==Ν.Ɣ){String Ѕ="";if(Ι(ref Ѕ,Ζ,Α++,Η," Back")){if(δ==Ρ.А) α(Ν.ŝ);else α(Ν.Main);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Max load: "+Ǩ+"%")) а(ref Ǩ,Ǩ<=80?-10:-5,π.Ơ,false);if(Ι(ref Ѕ,Ζ,Α++,Η," Weight limit: "+(Ț?"On":"Off"))) Ț=!Ț;if(Ι(ref Ѕ,Ζ,Α++,Η," Ejection: "+ΕЂ(С))) С=Ȟ(С);if(Ι(ref Ѕ,Ζ,Α++,Η," Toggle sorters: "+(Ĝ?"On":"Off"))){Ĝ=!Ĝ;if(Ĝ) Ǟ(ΓΩ);}
if(Ι(ref Ѕ,Ζ,Α++,Η," Unload ice: "+(ș?"On":"Off"))) ș=!ș;if(Ι(ref Ѕ,Ζ,Α++,Η," Uranium: "+(Ш==-1?"Ignore":"Min "+Ш+"Kg"))) а(ref Ш,(Ш>5?-5:-1),π.ơ,true);if(Ι(ref Ѕ,Ζ,Α++,Η," Battery: "+(т==-1?"Ignore":"Min "+т+"%"))) а(ref т,-5,π.ΑЌ,true);if(Ι(ref Ѕ,Ζ,Α++,Η," Hydrogen: "+(у==-1?"Ignore":"Min "+у+"%"))) а(ref у,-10,π.ΑЍ,true);
if(Ι(ref Ѕ,Ζ,Α++,Η," When done: "+(Ȱ?"Return home":"Stop"))) Ȱ=!Ȱ;if(Ι(ref Ѕ,Ζ,Α++,Η," On damage: "+ΓϏ(ж))) ж=Ȟ(ж);if(Ι(ref Ѕ,Ζ,Α++,Η," Advanced...")) α(Ν.Αε);Β+=ΒΝ(8,Ѕ,Ζ,ref ΗΩ);}else if(ρ==Ν.Αε){if(Ι(ref Β,Ζ,Α++,Η," Back")){if(δ==Ρ.А) α(Ν.ŝ);else α(Ν.Main);}
if(Ι(ref Β,Ζ,Α++,Η,(Ο==Λ.њ?" Grinder":" Drill")+" inv. balancing: "+(ĺ?"On":"Off"))) ĺ=!ĺ;if(Ι(ref Β,Ζ,Α++,Η," Enable"+(Ο==Λ.њ?" grinders":" drills")+": "+(Ĵ?"Fwd + Bwd":"Fwd"))) Ĵ=!Ĵ;if(Ι(ref Β,Ζ,Α++,Η," Work speed fwd.: "+œ+"m/s")) а(ref œ,0.5f,π.Ť,false);
if(Ι(ref Β,Ζ,Α++,Η," Work speed bwd.: "+Œ+"m/s")) а(ref Œ,0.5f,π.Ť,false);if(Ι(ref Β,Ζ,Α++,Η," Acceleration: "+Math.Round(Ǡ*100f)+"%"+(Ǡ>0.80f?" (risky)":""))) а(ref Ǡ,0.1f,π.ƒ,false);if(Ι(ref Β,Ζ,Α++,Η," Width overlap: "+Ƒ*100f+"%")) Γυ(true,0.05f);if(Ι(ref Β,Ζ,Α++,Η," Height overlap: "+ĸ*100f+"%")) Γυ(false,0.05f);
}else if(ρ==Ν.Ƭ){if(Ι(ref Β,Ζ,Α++,Η," Back")){if(δ==Ρ.А) α(Ν.ŝ);else α(Ν.Main);}if(Ι(ref Β,Ζ,Α++,Η," Max load: "+Ǩ+"%")) а(ref Ǩ,Ǩ<=80?-10:-5,π.Ơ,false);if(Ι(ref Β,Ζ,Α++,Η," Unload ice: "+(ș?"On":"Off"))) ș=!ș;if(Ι(ref Β,Ζ,Α++,Η," Uranium: "+(Ш==-1?"Ignore":"Min "+Ш+"Kg"))) а(ref Ш,(Ш>5?-5:-1),π.ơ,true);
if(Ι(ref Β,Ζ,Α++,Η," Battery: "+(т==-1?"Ignore":"Charge up"))) т=(т==-1?1:-1);if(Ι(ref Β,Ζ,Α++,Η," Hydrogen: "+(у==-1?"Ignore":"Fill up"))) у=(у==-1?1:-1);if(Ι(ref Β,Ζ,Α++,Η," On damage: "+ΓϏ(ж))) ж=Ȟ(ж);if(Ι(ref Β,Ζ,Α++,Η," Acceleration: "+Math.Round(Ǡ*100f)+"%"+(Ǡ>0.80f?" (risky)":""))) а(ref Ǡ,0.1f,π.ƒ,false);
}else if(ρ==Ν.ΔΚ){double ΖБ=0;if(Χ.Count>0) ΖБ=Vector3.Distance(Χ.Last().Ε,μ);if(Ι(ref Β,Ζ,Α++,Η," Stop path recording")) ΔΤ();if(Ο!=Λ.ί){if(Ι(ref Β,Ζ,Α++,Η," Home: "+(ȁ?"Use old home":(Μ.γ?"Was set! ":"none ")))) ȁ=!ȁ;}else{if(Ι(ref Β,Ζ,Α++,Η," Connector 1: "+(ȁ?"Use old connector":(Μ.γ?"Was set! ":"none ")))) ȁ=!ȁ;
if(Ι(ref Β,Ζ,Α++,Η," Connector 2: "+(ŏ?"Use old connector":(Θ.γ?"Was set! ":"none ")))) ŏ=!ŏ;}if(Ι(ref Β,Ζ,Α++,Η," Path: "+(Ő?"Use old path":(Χ.Count>1?"Count: "+Χ.Count:"none ")))) Ő=!Ő;Β+=Ŏ;Β+="Wp spacing: "+Math.Round(Εν)+"m\n";}else if(ρ==Ν.ΒΨ){if(Ι(ref Β,Ζ,Α++,Η," Stop")){φ();α(Ν.Main);}
Β+=Ŏ;Β+="State: "+Αύ(Ϊ)+" \n";Β+="Speed: "+Math.Round(ě,1)+"m/s\n";Β+="Target dist: "+Ɔ+"m\n";Β+="Wp count: "+Χ.Count+"\n";Β+="Wp left: "+Γμ+"\n";}else if(ρ==Ν.ΓЊ){List<IMyTerminalBlock>č=ΓЀ();if(ȑ) Ȫ++;if(Ȫ>=č.Count) Ȫ=0;if(Ι(ref Β,Ζ,Α++,Η," Next")) α(Ν.ΓЋ);
Β+=Ŏ;Β+="Version: "+VERSION+"\n";Β+="Ship load: "+Math.Round(ΑΕ,1)+"% "+Math.Round(ǣ,1)+" / "+Math.Round(ǩ,1)+"\n";Β+="Uranium: "+(Ȯ.Count==0?"No reactors":Math.Round(Ğ,1)+"Kg "+ΒЏ)+"\n";Β+="Battery: "+(ǡ==й.None?"":Ǵ+"% ")+Γύ(ǡ)+"\n";Β+="Hydrogen: "+(э.Count==0?"No tanks":Math.Round(Ȑ,1)+"% ")+"\n";
Β+="Damage: "+(č.Count==0?"None":""+(Ȫ+1)+"/"+č.Count+" "+č[Ȫ].CustomName)+"\n";}else if(ρ==Ν.ΓЋ){if(Ι(ref Β,Ζ,Α++,Η," Back")) α(Ν.Main);Β+=Ŏ;Β+="Next scan: "+ž+"s\n";Β+="Ship size: "+Math.Round(Ș,1)+"m "+Math.Round(Ȁ,1)+"m "+Math.Round(ψ,1)+"m \n";
Β+="Broadcast: "+(Ĳ?"Online - "+ŋ:"Offline")+"\n";Β+="Max Instructions: "+Math.Round(Ŀ*100f,1)+"% \n";}else if(ρ==Ν.ΓЉ){if(Ι(ref Β,Ζ,Α++,Η," Back")) α(Ν.Main);Β+=Ŏ;Β+="1. Dock to your docking station\n";Β+="2. Select Record path & set home\n";Β+="3. Fly the path to the ores\n";
Β+="4. Select stop path recording\n";Β+="5. Align ship in mining direction\n";Β+="6. Select Setup job and start\n";}if(Є==2) Β="Fatal setup error\nNext scan: "+ž+"s\n";if(ΑΧ) Β="Recompile script now";int ΗΑ=Β.Split('\n').Length;for(int i=ΗΑ;i<=10;i++) Β+="\n";Β+=Ŋ;Β+="Last: "+Π+"\n";
return Β;}void Γυ(bool ΖЍ,float ѓ){φ();ư(true,false);if(ΖЍ) а(ref Ƒ,ѓ,π.ΒΖ,false);else а(ref ĸ,ѓ,π.ΒΖ,false);Γσ();Ĉ(true,true,0,0);}float Γψ(float η,float[]Αβ){float Ϋ=Αβ[0];for(int i=Αβ.Length-1;i>=0;i--)
if(η<Αβ[i]) Ϋ=Αβ[i];return Ϋ;}String Ƥ(ref int ȩ){String Ϋ="";if(ȩ>=Γβ.Count) ȩ=-1;if(ȩ>=0) Ϋ=Γβ[ȩ].CustomName;ȩ++;return Ϋ;}void Ʈ(string Ψ){if(δ!=Ρ.А) return;if(Ψ=="") return;IMyTerminalBlock Υ=ώ.GetBlockWithName(Ψ);if(Υ==null||!(Υ is IMyTimerBlock)){Π="Timerblock "+Ψ+" not found!";
return;}((IMyTimerBlock) Υ).Trigger();}void Ģ(ref int value,int Ū,int ȗ,int ΓΧ){if(Ū==0) return;if(value<ȗ&&ΓΧ>0||value<=ȗ&&ΓΧ<0){value+=ΓΧ;return;}int Ζΰ=Math.Abs(Ū);int Βή=0;int ΓΘ=1;while(true){Βή+=ΓΘ*Ζΰ*10;if(Ū<0&&value-ȗ<=Βή) break;if(Ū>0&&value-ȗ<Βή) break;
ΓΘ++;}value+=ΓΘ*Ū;}void ĉ(bool ΗЈ){ы=Math.Max(ы,1);ъ=Math.Max(ъ,1);д=Math.Max(д,0);Ĉ(ρ==Ν.Ş,false,0,0);}bool Ą(ref float value,bool ΗΤ,π ν,String đ,String ΖЉ){if(đ=="") return false;float Ń=-1;bool ΔЁ=false;if(đ.ToUpper()==ΖЉ) ΔЁ=true;else if(!float.TryParse(đ,out Ń)) return false;
else Ń=Math.Max(0,Ń);if(ΗΤ) Ń=(float) Math.Round(Ń);а(ref value,Ń,ν,ΔЁ,false);return true;}void а(ref float value,float ѓ,π ν,bool Ɗ){а(ref value,value+ѓ,ν,Ɗ,true);}void а(ref float value,float Ł,π ν,bool Ɗ,bool ΗЀ){float Ǔ=0,ǒ=0;if(ν==π.Ť){Ǔ=0.5f;ǒ=10f;}if(ν==π.ƒ){Ǔ=0.1f;ǒ=1f;}
if(ν==π.Ś){Ǔ=50f;ǒ=100f;}if(ν==π.ΑЌ){Ǔ=5f;ǒ=30f;}if(ν==π.ơ){Ǔ=1f;ǒ=25f;}if(ν==π.Ơ){Ǔ=10f;ǒ=95f;}if(ν==π.ΑЍ){Ǔ=10f;ǒ=90f;}if(ν==π.ΖΡ){Ǔ=10f;ǒ=1800;}if(ν==π.ΒΖ){Ǔ=0.0f;ǒ=0.75f;}if(Ł==-1&&Ɗ){value=-1;return;}if(value==-1) Ɗ=false;
bool ΕΜ=Ł<Ǔ||Ł>ǒ;if(ΕΜ&&ΗЀ){if(Ł<value) value=ǒ;else if(Ł>value) value=Ǔ;}else value=Ł;if(ΕΜ&&Ɗ) value=-1;else value=Math.Max(Ǔ,Math.Min(value,ǒ));value=(float) Math.Round(value,2);}void Αδ(bool ΗΟ){if(ΗΟ) р=Math.Max(0,р-1);else р++;ΒΫ(true);}T Ȟ<T>(T η){int Α=Array.IndexOf(Enum.GetValues(η.GetType()),η);
Α++;if(Α>=Εύ(η)) Α=0;return(T) Enum.GetValues(η.GetType()).GetValue(Α);}int Εύ<T>(T value){return Enum.GetValues(value.GetType()).Length;}void ΒΧ(){Ɛ.Clear();for(int i=0;i<Χ.Count;i++) Ɛ.Add(Χ[i]);Χ.Clear();ǌ=true;ΒЁ=new ά(Μ);ΒЄ=new ά(Θ);Μ.γ=false;if(Ο==Λ.ί) Θ.γ=false;for(int i=0;i<є.Count;i++)
if(!Ǿ.Contains(є.Keys.ElementAt(i)))
Ǿ.Add(є.Keys.ElementAt(i));ȁ=false;ŏ=false;Ő=false;α(Ν.ΔΚ);}void ΔΤ(){if(ȁ) Μ=ΒЁ;if(ŏ) Θ=ΒЄ;if(Ő){Χ.Clear();for(int i=0;i<Ɛ.Count;i++) Χ.Add(Ɛ[i]);}ǌ=false;φ();α(Ν.Main);}void ΖΫ(){if(!ǌ) return;if(Ϊ!=Γ.л){ΔΤ();return;}if(!ΒЁ.γ) ȁ=false;if(!ΒЄ.γ) ŏ=false;if(Ɛ.Count<=1) Ő=false;
IMyShipConnector ў=в(MyShipConnectorStatus.Connectable);if(ў==null) ў=в(MyShipConnectorStatus.Connected);if(ў!=null){if(Math.Round(ě,2)<=0.20) Βτ++;else Βτ=0;if(Βτ>=5){if(Ο==Λ.ί&&(Μ.γ||ȁ)&&Vector3.Distance(Μ.Ε,ў.GetPosition())>5){Θ.ΰ=Φ.WorldMatrix.Forward;Θ.ω=Φ.WorldMatrix.Left;
Θ.β=Φ.WorldMatrix.Down;Θ.χ=Φ.GetNaturalGravity();Θ.Ε=ў.GetPosition();Θ.γ=true;Θ.о=ў.Position;}else{Μ.ΰ=Φ.WorldMatrix.Forward;Μ.ω=Φ.WorldMatrix.Left;Μ.β=Φ.WorldMatrix.Down;Μ.χ=Φ.GetNaturalGravity();Μ.Ε=ў.GetPosition();Μ.γ=true;Μ.о=ў.Position;}}}double ΒЅ=-1;if(Χ.Count>0) ΒЅ=Vector3.Distance(μ,Χ.Last().Ε);
double Ηΰ=Math.Max(1.5,Math.Pow(ě/100.0,2));double ΕΕ=Math.Max(ě*Ηΰ,2);Εν=ΕΕ;if((ΒЅ==-1)||ΒЅ>=ΕΕ){ά Ω=new ά(μ,ǅ,ǁ,ΑΔ,Φ.GetNaturalGravity());Ω.Επ(ǿ,Ǿ);Χ.Add(Ω);}}ά Ȅ(){if(Ο!=Λ.ί) return Μ;return Δλ;}void Ī(ά έ,Ρ υ){Δλ=έ;if(δ==Ρ.А) ю=υ;}bool ǰ(ά έ,float Ћ,bool Ηχ,out Vector3 И)
{if(Ηχ){Vector3I Βό=new Vector3I((int) έ.о.X,(int) έ.о.Y,(int) έ.о.Z);IMySlimBlock Υ=Me.CubeGrid.GetCubeBlock(Βό);if(Υ==null||!(Υ.FatBlock is IMyShipConnector)){И=new Vector3();return false;}Vector3 Ζο=Ы(Φ,Υ.FatBlock.GetPosition()-μ);Vector3 Ζξ=Ы(Φ,Υ.FatBlock.WorldMatrix.Forward);И=έ.Ε-ΒΪ(έ.ΰ,έ.β*-1,Ζο)-ΒΪ(έ.ΰ,έ.β*-1,Ζξ)*Ћ;
return true;}else{И=έ.Ε;return true;}}Vector3 ȅ(int Ζμ,int ΗΥ,bool ΗΡ){if(!ΗΡ&&Δό) return Βν;float Ζν=((Ч-1f)/2f)-Ζμ;float ΗΦ=((с-1f)/2f)-ΗΥ;Βν=Θ.Ε+Θ.ω*Ζν*Ș+ĕ*-1*ΗΦ*Ȁ;Δό=true;return Βν;}Vector3 Αό(Vector3 Ηε,float Ζφ){return Ηε+(Ǽ*Ζφ);}ή Ας(){float М=-1;
ή Ϋ=ή.Ǯ;if(Ο!=Λ.ί){if(δ!=Ρ.м){Vector3 Ў=ć(Ǽ,ĕ*-1,μ-Θ.Ε);if(Math.Abs(Ў.X)<=(float)(Ч*Ș)/2f&&Math.Abs(Ў.Y)<=(float)(с*Ȁ)/2f){if(Ў.Z<=-1&&Ў.Z>=-ę*2) return ή.Ɠ;if(Ў.Z>-1&&Ў.Z<ψ*2) return ή.Ǯ;}if(Ȝ(Θ.Ε,ref М)) Ϋ=ή.Ǯ;}if(Μ.γ){if(Ȝ(Μ.Ε,ref М)) Ϋ=ή.ǭ;
for(int i=0;i<Χ.Count;i++)
if(Ȝ(Χ[i].Ε,ref М)) Ϋ=ή.Ȇ;if(Vector3.Distance(μ,Μ.Ε)<dockDist*ψ) Ϋ=ή.ǭ;if(в(MyShipConnectorStatus.Connectable)!=null||в(MyShipConnectorStatus.Connected)!=null)
Ϋ=ή.ǭ;}}else{Vector3 И=new Vector3();IMyShipConnector Ƕ=в(MyShipConnectorStatus.Connected);if(Μ.γ){if(Ȝ(Μ.Ε,ref М)) Ϋ=ή.ǭ;if(ǰ(Μ,dockDist,true,out И))
if(Ȝ(И,ref М)) Ϋ=ή.ǭ;if(Ƕ!=null&&Vector3.Distance(Ƕ.GetPosition(),Μ.Ε)<5)
return ή.Αθ;}for(int i=0;i<Χ.Count;i++)
if(Vector3.Distance(Χ[i].Ε,Μ.Ε)>dockDist*ψ&&Vector3.Distance(Χ[i].Ε,Θ.Ε)>dockDist*ψ)
if(Ȝ(Χ[i].Ε,ref М)) Ϋ=ή.Ȇ;if(Θ.γ){if(Ȝ(Θ.Ε,ref М)) Ϋ=ή.Ǯ;if(ǰ(Θ,dockDist,true,out И))
if(Ȝ(И,ref М)) Ϋ=ή.Ǯ;if(Ƕ!=null&&Vector3.Distance(Ƕ.GetPosition(),Θ.Ε)<5)
return ή.Αι;}}return Ϋ;}bool Ȝ(Vector3 И,ref float М){float Ћ=Vector3.Distance(И,μ);if(Ћ<М||М==-1){М=Ћ;return true;}return false;}void ΒΦ(){if(Є>0){Π="Setup error! Can't start";return;}if(Ο==Λ.ί){ȝ();return;}Θ.Ε=μ;Θ.χ=Φ.GetNaturalGravity();Θ.ΰ=ǅ;Θ.β=ǁ;Θ.ω=ΑΔ;
Ǽ=ѐ.WorldMatrix.Forward;ĕ=Θ.β;if(Ǽ==Φ.WorldMatrix.Down) ĕ=Φ.WorldMatrix.Backward;ư(true,true);Σ(Γ.ă);ΒΥ();}void ư(bool Ǐ,bool ΗΣ){if(δ==Ρ.м&&!Ǐ) return;bool ΗΗ=Ǐ||δ==Ρ.Ασ||Ч!=ы||с!=ъ||ΑΓ!=Ȗ;if(ΗΗ){if(δ!=Ρ.м){δ=Ρ.Ɩ;ȅ(τ,σ,ΗΣ);Π="Job changed, lost progress";}ΑΓ=Ȗ;
Ч=ы;с=ъ;σ=0;τ=0;ŀ=0;Ȭ=0;Ф=0;р=0;ΒΫ(true);}}void ΒΙ(){н(μ,0);щ(ǿ,true);}void ΒΥ(){ǲ(Ь.Ʃ);ΒΙ();Ţ(ǻ,false);Ǟ(ΓΩ);δ=Ρ.А;Γς(true);ю=δ;α(Ν.ŝ);ΖΖ();ƾ=true;ǉ=0;for(int i=ť.Count-1;i>=0;i--)
if(ś(ť[i],false)) ǉ++;if(ǉ>0) Π="Started with damage";}void φ(){if(δ==Ρ.А){δ=Ρ.ΒΘ;Π="Job paused";}Σ(Γ.л);ю=δ;ƫ(false,0,0,0,0);ΒΩ();Γο(new Vector3(),false);ΔΣ();ΒΠ(ChargeMode.Auto);ΒΡ(false);ΔΜ(true);ȇ(Γ.л);Ĉ(false,false,0,0);щ(ο,false);щ(п,true);Ǟ(true);ΑΨ=false;ƾ=false;
Ɓ=false;Ƌ=false;if(ρ!=Ν.Main&&ρ!=Ν.Ɣ&&ρ!=Ν.Αε&&ρ!=Ν.Ƭ)
α(Ν.Main);}void ȝ(){ή Н=Ας();if(Ο==Λ.ί){if(!Θ.γ||!Μ.γ) return;ΒΥ();bool Βϊ=Vector3.Distance(μ,Μ.Ε)<Vector3.Distance(μ,Θ.Ε);if(ň==Ρ.Ġ) Βϊ=true;if(ň==Ρ.ġ) Βϊ=false;if(Βϊ){Ī(Μ,Ρ.Ġ);switch(Н){case ή.Αθ:Σ(Γ.Ȣ);break;case ή.Ȇ:Σ(Γ.г);break;case ή.ǭ:Σ(Γ.ț);break;default:Σ(Γ.Ƞ);break;
}}else{Ī(Θ,Ρ.ġ);switch(Н){case ή.Αι:Σ(Γ.Ȣ);break;case ή.Ǯ:Σ(Γ.ț);break;case ή.Ȇ:Σ(Γ.г);break;default:Σ(Γ.Ƞ);break;}}}else{if(δ!=Ρ.ΒΘ&&δ!=Ρ.Ɩ) return;bool Ηω=δ==Ρ.Ɩ;ΒΥ();bool ΒϏ=Ŝ(false)&&Μ.γ;switch(Н){case ή.Ǯ:Σ(ΒϏ?Γ.Ă:Γ.ă);break;case ή.Ȇ:Σ(ΒϏ?Γ.ǜ:Γ.г);break;case ή.ǭ:Σ(ΒϏ?Γ.ț:Γ.ȉ);break;
case ή.Ɠ:{if(Ȭ!=р||Ηω) Σ(Γ.ȟ);else Σ(Γ.Š);}break;default:break;}}}void ā(){if(δ==Ρ.м&&!Μ.γ) return;if(Ο==Λ.ί&&(!Θ.γ||!Μ.γ)) return;Π="Move to job";ή Н=Ας();if(Ο==Λ.ί){Ī(Θ,Ρ.ġ);switch(Н){case ή.Ǯ:Σ(Γ.ț);break;case ή.Ȇ:Σ(Γ.г);break;case ή.Αι:return;default:Σ(Γ.Ƞ);break;
}ȇ(Γ.Ȣ);}else{switch(Н){case ή.Ǯ:Σ(Γ.г);break;case ή.Ȇ:Σ(Γ.г);break;case ή.ǭ:Σ(Γ.ȉ);break;case ή.Ɠ:Σ(Γ.ȟ);break;default:break;}if(δ==Ρ.м) ȇ(Γ.г);else ȇ(Γ.Ȳ);ΑΨ=true;}ΒΙ();α(Ν.ΒΨ);Ţ(ǻ,false);ю=Ρ.ƣ;}void ƛ(){if(!Μ.γ) return;Π="Move home";ή Н=Ας();if(Ο==Λ.ί){Ī(Μ,Ρ.Ġ);switch(Н)
{case ή.Ȇ:Σ(Γ.ǜ);break;case ή.ǭ:Σ(Γ.ț);break;case ή.Αθ:return;default:Σ(Γ.Ƞ);break;}ȇ(Γ.Ȣ);}else{if(в(MyShipConnectorStatus.Connected)!=null) return;if(в(MyShipConnectorStatus.Connectable)!=null){Σ(Γ.ŗ);ȇ(Γ.ȉ);return;}switch(Н){case ή.Ǯ:Σ(Γ.Ă);break;case ή.Ȇ:Σ(Γ.ǜ);break;case ή.ǭ:Σ(Γ.ǜ);break;
case ή.Ɠ:Σ(Γ.ą);break;default:break;}ȇ(Γ.ȉ);}ΒΙ();α(Ν.ΒΨ);Ţ(ǻ,false);ю=Ρ.Ƣ;}void Σ(Γ ΓΙ){if(ΓΙ==Γ.л) ƃ=Γ.л;if(ƃ!=Γ.л&&Ϊ==ƃ&&ΓΙ!=ƃ){φ();return;}Џ=true;Ϊ=ΓΙ;}void ȇ(Γ υ){this.ƃ=υ;}Ȉ ΕІ(int Ŵ,bool ǖ){if(ǖ){ђ=null;τ=0;σ=0;}if(Ȗ==ǟ.ƭ){int ΕΘ=Ŵ+1;σ=(int) Math.Floor(І(Ŵ,Ч));
if(σ%2==0) τ=Ŵ-(σ*Ч);else τ=Ч-1-(Ŵ-(σ*Ч));if(σ>=с) return Ȉ.Αη;else return Ȉ.Αω;}else if(Ȗ==ǟ.Γτ){if(ђ==null) ђ=new int[]{0,-1,0,0};int ΔϏ=(int) Math.Ceiling(Ч/2f);int Δώ=(int) Math.Ceiling(с/2f);int ΖЄ=(int) Math.Floor(Ч/2f);int ΖЃ=(int) Math.Floor(с/2f);
int ŵ=0;while(ђ[2]<Math.Pow(Math.Max(Ч,с),2)){if(ŵ>200) return Ȉ.Εώ;ŵ++;ђ[2]++;if(-ΔϏ<τ&&τ<=ΖЄ&&-Δώ<σ&&σ<=ΖЃ){if(ђ[3]==Ŵ){this.τ=τ-1+ΔϏ;this.σ=σ-1+Δώ;return Ȉ.Αω;}ђ[3]++;}if(τ==σ||(τ<0&&τ==-σ)||(τ>0&&τ==1-σ)){int Ηθ=ђ[0];ђ[0]=-ђ[1];ђ[1]=Ηθ;
}τ+=ђ[0];σ+=ђ[1];}}return Ȉ.Αη;}void ΖΪ(){if(Ϊ==Γ.ă){if(Џ){Љ=0;if(Ȭ!=р) ŀ=0;Ȭ=р;}if(Љ==0){Ȉ Ϋ=ΕІ(р,Џ);if(Ϋ==Ȉ.Αη){δ=Ρ.Ασ;Π="Job done";if(Ȱ&&Μ.γ){Σ(Γ.Ă);ȇ(Γ.ȉ);ю=Ρ.Ƣ;}else{Σ(Γ.Ř);ȇ(Γ.Ȳ);ю=Ρ.ƣ;}return;}if(Ϋ==Ȉ.Αω){Љ=1;щ(ο,true);ф=ȅ(τ,σ,true);н(ф,10);
Ъ(Θ.β,Θ.ΰ,Θ.ω,false);}}else{if(Р<wpReachedDist){Σ(Γ.Š);return;}}}if(Ϊ==Γ.Š){if(Џ){щ(ο,true);Ǟ(false);ф=ȅ(τ,σ,false);н(Αό(ф,0),0);Ъ(Θ.β,Θ.ΰ,Θ.ω,false);Љ=1;е=0;Щ=0;ƶ=0;ΒЇ=-1;ę=д;ΒЂ=true;}if(!Αξ()){Σ(Γ.ą);return;}if(Ŝ(true)){ΕΛ=Ȃ("","ORE",Й.ȳ);if((С==Ђ.Ατ||С==Ђ.Ɨ||
С==Ђ.ΑЃ||С==Ђ.ƞ)&&Ο!=Λ.њ)
Σ(Γ.ƚ);else if((С==Ђ.Γχ||С==Ђ.Αο)&&Ο!=Λ.њ)
Σ(Γ.Βά);else
Σ(Γ.ą);return;}Ф=Vector3.Distance(μ,ф);if(Ф>ŀ){ŀ=Ф;ΒЂ=false;}if(Ο==Λ.њ&&ΓЁ()==MyDetectedEntityType.SmallGrid)
Щ+=2;else Щ-=2;Щ=Math.Max(100,Math.Min(400,Щ));if(Љ>0&&Љ<Щ){if(Ф>е){if(Щ>150) е=Ф;else е=(float) Math.Ceiling(Ф);Љ=1;}else Љ++;}else{if(Љ>0){Π="Ship stuck! Retrying";е=Ф;Љ=0;Ĉ(false,true,0,ψ*sensorRange);}н(Αό(ф,Math.Max(0,е-ψ)),ΑϏ(false));if(Р<=wpReachedDist/2){Љ=1;е=0;}
return;}Ĉ(false,true,ψ*sensorRange,0);Vector3 Ζβ=ф+Ǽ*Ф;bool ΔЄ=false;if(Vector3.Distance(Ζβ,μ)>0.3f){Vector3 Ζσ=ф+Ǽ*(Ф+0.1f);н(Ζσ,4);ΔЄ=true;}else{float Ǘ=ΑϏ(true);Vector3 Δο=Αό(ф,Math.Max(д+1,Ф+1));ǝ(true,false,false,Δο,Δο-ф,Ǘ,Ǘ);}bool Βς=false;if(Ǎ==ј.Αλ||Ǎ==ј.Ακ)
{if(!ΔЄ){float ΓΚ=0;foreach(IMyTerminalBlock Ǭ in ο)
ΓΚ+=Εϊ(Ǭ,"","",Ǎ==ј.Ακ?new string[]{"STONE"}:null);if(ΓΚ>ΒЇ||Ф<д||ΒЂ){Βδ=0;ƶ=Ф;ę=(int)(Math.Max(ę,ƶ)+ψ/2);}else{Βς=Ф-ƶ>2&&Βδ>=20;Βδ++;}ΒЇ=ΓΚ;}}else Βς=Ф>=ę;if(Ȭ!=р){Σ(Γ.ȟ);Ф=0;return;}if(Βς){р++;Σ(Γ.ȟ);Ф=0;return;}}if(Ϊ==Γ.ȟ||Ϊ==Γ.ą||Ϊ==Γ.ƚ)
{if(Џ){ф=ȅ(τ,σ,false);Ъ(Θ.β,Θ.ΰ,Θ.ω,false);щ(ο,Ĵ);Ǟ(false);е=Vector3.Distance(μ,ф);Ĉ(false,true,0,ψ*sensorRange);}н(ф,ΑϏ(false));if(Vector3.Distance(μ,ф)>=е+5){щ(ο,false);Ǟ(true);Π="Can´t return!";}if(Р<wpReachedDist){if(Ϊ==Γ.ȟ&&ΑΨ) Σ(Γ.Ř);if(Ϊ==Γ.ȟ) Σ(Γ.ă);
if(Ϊ==Γ.ƚ) Σ(Γ.Αχ);if(Ϊ==Γ.ą){if(Μ.γ) Σ(Γ.Ă);else{φ();ā();Π="Can´t return, no dock found";}}return;}}if(Ϊ==Γ.ǜ||Ϊ==Γ.г){if(Ϊ==Γ.г&&δ==Ρ.А&&Ο!=Λ.ί){if(!Αξ()||Ŝ(true)){Σ(Γ.ǜ);return;}}bool ΓΠ=false;bool Γθ=false;bool Δή=false;float İ=0;bool ΕΚ=false;ά Ω=null;
if(Џ){if(Ϊ==Γ.ǜ||Ο==Λ.ί){ά έ=Ȅ();θ=new ΒΗ();θ.ι=έ;θ.ŭ=followPathDock*ψ;θ.Γι=useDockDirectionDist*ψ;θ.İ=10;θ.ō.Add(έ.Ε);Vector3 į=new Vector3();if(ǰ(έ,dockDist*ψ,true,out į))
θ.ō.Add(į);else θ.ŭ*=1.5f;if(Ο==Λ.ί){if(έ==Μ) θ.ǂ=Θ.Ε;if(έ==Θ) θ.ǂ=Μ.Ε;θ.Βχ=dockDist*ψ*1.1f;}}else if(Ϊ==Γ.г){θ=new ΒΗ();θ.ι=Θ;θ.ŭ=followPathJob*ψ;θ.Γι=useJobDirectionDist*ψ;θ.İ=10;θ.ǂ=Μ.Ε;θ.Βχ=dockDist*ψ*1.1f;θ.ō.Add(Θ.Ε);if(δ==Ρ.м){if(!Μ.γ||Χ.Count==0){φ();return;}
float Ζϊ=Vector3.Distance(Χ.First().Ε,Μ.Ε);float Ζϋ=Vector3.Distance(Χ.Last().Ε,Μ.Ε);if(Ζϊ<Ζϋ) θ.ι=Χ.Last();else θ.ι=Χ.First();}}ż=new Vector3();ΕΚ=!ΕЎ(μ);щ(ο,false);Ǟ(true);Ǖ=-1;double Ǣ=-1;for(int i=Χ.Count-1;i>=0;i--){if(Vector3.Distance(Χ[i].Ε,θ.ǂ)<=θ.Βχ) continue;
double Ћ=Vector3.Distance(Χ[i].Ε,μ);if(Ǣ==-1||Ћ<Ǣ){Ǖ=i;Ǣ=Ћ;}}ė=ΕЇ(θ.ι.Ε,Ǖ);Č=null;}Ερ(Χ,ė,θ.ō,θ.ŭ,Џ,ref Љ);for(int i=0;i<θ.ō.Count;i++){float Ћ=Vector3.Distance(μ,θ.ō[i]);if(Ћ<=θ.ŭ) ΓΠ=true;if(Ћ<=θ.Γι) Γθ=true;}if(Γθ) İ=θ.İ;float ΗЉ=Č!=null?Č.Х:ě;
float ΗϏ=(float) Math.Max(ě*0.1f*ψ,wpReachedDist);if((Р<ΗϏ)||Џ){if(!Џ) Ǖ+=ė;if(ė==0||Ǖ>Χ.Count-1||Ǖ<0)
ΓΠ=true;else{Γμ=ė>0?Χ.Count-1-Ǖ:Ǖ;Ω=Χ[Ǖ];Č=Ω;if(Ǖ>=1&&Ǖ<Χ.Count-1)
ż=Ω.Ε-Χ[Ǖ-ė].Ε;else Č=null;Ѓ=Ω.Ε;Δή=true;}}if(Γθ)
Ъ(θ.ι.β,θ.ι.ΰ,θ.ι.ω,false);else if(ΕΚ)
Αζ(θ.ι.β,10,true);else if(Δή&&Ω!=null){if(ė>0) Ъ(Ω.β,Ω.ΰ,Ω.ω,90,false);else Ъ(Ω.β,-Ω.ΰ,-Ω.ω,90,false);}ǝ(true,false,true,Ѓ,ż,Č==null?0:Č.Х,İ);if(ΓΠ){Γμ=0;if(Ϊ==Γ.ǜ||Ο==Λ.ί){Σ(Γ.ț);return;}if(Ϊ==Γ.г&&ΑΨ){Σ(Γ.Ř);return;}if(Ϊ==Γ.г){Σ(Γ.ă);return;}
}}if(Ϊ==Γ.ț||Ϊ==Γ.ΒΜ){ά έ=Ȅ();if(Џ){if(!ǰ(έ,dockDist*ψ,true,out Ѓ)){ǲ(Ь.Ŗ);φ();return;}н(Ѓ,0);Αζ(έ.β,90,true);}if(Р<followPathDock*ψ&&Р!=-1){н(Ѓ,10);Ъ(έ.β,έ.ΰ,έ.ω,false);}if(в(MyShipConnectorStatus.Connectable)!=null||в(MyShipConnectorStatus.Connected)!=null)
{Σ(Γ.ŗ);return;}if(Р<wpReachedDist/2&&Р!=-1){Σ(Γ.Ŕ);return;}}if(Ϊ==Γ.Ŕ||Ϊ==Γ.Ȳ){if(Џ){if(Ϊ==Γ.Ŕ){ά έ=Ȅ();if(!ǰ(έ,dockDist*ψ,true,out Ѓ)){ǲ(Ь.Ŗ);φ();return;}ǝ(true,true,false,Ѓ,0);Ъ(έ.β,έ.ΰ,έ.ω,10,false);}if(Ϊ==Γ.Ȳ){Ъ(Θ.β,Θ.ΰ,Θ.ω,0.5f,false);Ѓ=Θ.Ε;
ǝ(true,true,false,Ѓ,0);}}if(ǋ){ƫ(false,0,0,0,0);if(Ϊ==Γ.Ŕ) Σ(Γ.ŗ);if(Ϊ==Γ.Ȳ) φ();return;}}if(Ϊ==Γ.ŗ){if(в(MyShipConnectorStatus.Connected)!=null){if(Ο==Λ.ί) Σ(Γ.Ȣ);else Σ(Γ.ȉ);return;}ά έ=Ȅ();if(Џ){Щ=0;Ζό=DateTime.Now;Љ=0;Ъ(έ.β,έ.ΰ,έ.ω,false);}Vector3I Βό=new Vector3I((int) έ.о.X,(int) έ.о.Y,(int) έ.о.Z);
IMySlimBlock Δθ=Me.CubeGrid.GetCubeBlock(Βό);float Δτ=dockingSpeed;float ΗΔ=dockingSpeed*5;float Δά=Math.Max(1.5f,Math.Min(5f,ψ*0.15f));if(!ǰ(έ,0,true,out Ѓ)||!ǰ(έ,Δά,true,out ż)||
Δθ==null||!Δθ.FatBlock.IsFunctional){ǲ(Ь.Ŗ);φ();return;}if(Щ==1||(Vector3.Distance(μ,Ѓ)<=Δά*1.1f&&!Џ))
Щ=1;else{Vector3 Ηπ=Ы(Φ,ż-μ);Vector3 Βϋ=Ы(Φ,Φ.GetNaturalGravity());float ΗΧ=ƕ(Ηπ,Βϋ,null);Δτ=Math.Min(ΗΔ,ΗΧ);}ǝ(true,false,false,Ѓ,Ѓ-μ,dockingSpeed,Δτ);if(Џ) е=(float) Р;IMyShipConnector Βο=в(MyShipConnectorStatus.Connectable);if(Βο!=null){ǝ(false,false,false,Ѓ,0);
if(Љ>0) Љ=0;Љ--;if(Љ<-5){Βο.Connect();if(Βο.Status==MyShipConnectorStatus.Connected){if(Ο==Λ.ί) Σ(Γ.Ȣ);else Σ(Γ.ȉ);ΒΩ();Ţ(ǻ,true);return;}}}else{float Δς=(float) Math.Round(Р,1);if(Δς<е){Љ=-1;е=Δς;}else Љ++;if(Љ>20){Σ(Γ.ΒΜ);return;}}}if(Ϊ==Γ.ȉ||Ϊ==Γ.Ȣ||
Ϊ==Γ.Βέ||Ϊ==Γ.Αυ||Ϊ==Γ.ĥ){bool Ββ=false,Βγ=false;if(Ο==Λ.ί){if(Ȅ()==Μ) Ββ=true;else if(Ȅ()==Θ) Βγ=true;}if(Џ){Ɓ=false;if(в(MyShipConnectorStatus.Connected)==null){Σ(Γ.Ƞ);return;}ΒΩ();if(Ββ) Ʈ(ϊ.ǎ);if(Βγ) Ʈ(ϋ.ǎ);ĝ=false;ȋ=false;ȏ=false;ƍ=false;}if(в(MyShipConnectorStatus.Connected)==null){φ();ǲ(Ь.ΓЎ);return;}
if(δ!=Ρ.А||т==-1||ǡ==й.None) ȋ=true;else if(Ǵ>=100f) ȋ=true;else if(Ǵ<=99f) ȋ=false;if(δ!=Ρ.А||у==-1||э.Count==0) ȏ=true;else if(Ȑ>=100f) ȏ=true;else if(Ȑ<=99) ȏ=false;if(δ!=Ρ.А||Ш==-1||Ȯ.Count==0) ƍ=true;else ƍ=Ğ>=Ш;Ĭ и=null;if(Ββ) и=ϊ;
if(Βγ) и=ϋ;if(и!=null&&(и.Г==ζ.Ʀ||и.Г==ζ.ƥ)) ȋ=true;if(и!=null&&(и.Г==ζ.ΑА)) if(!ĝ) ȋ=false;if(и!=null&&(и.Г==ζ.ΒΓ||и.Г==ζ.ΒΑ)) ȏ=true;if(и!=null&&(и.Г==ζ.ΒΒ)) if(!ĝ) ȏ=false;if(ȑ){ChargeMode Δδ=ȋ?ChargeMode.Auto:ChargeMode.Recharge;if(и!=null&&(и.Г==ζ.ƥ||и.Г==ζ.Ʀ))
Δδ=ChargeMode.Discharge;ΒΠ(Δδ);ΒΡ(!ȏ);}if(!ĝ){if(Ο==Λ.ί)
ĝ=δ!=Ρ.А||Ετ(Џ,true)||Ƌ;else
ĝ=δ!=Ρ.А||ΓЏ();}else{if(!ȋ) Σ(Γ.ĥ);if(!ȏ) Σ(Γ.Αυ);if(!ƍ) Σ(Γ.Βέ);Џ=false;}if(ĝ&&ȋ&&ȏ&&ƍ){ΒΠ(ChargeMode.Auto);ΒΡ(false);if(δ==Ρ.А){if(Ο==Λ.ί){if(Ȅ()==Μ) Ʈ(ϊ.Ǒ);else if(Ȅ()==Θ) Ʈ(ϋ.Ǒ);if(Ȅ()==Μ) Ī(Θ,Ρ.ġ);else Ī(Μ,Ρ.Ġ);}}Σ(Γ.Ƞ);return;}}if(Ϊ==Γ.Ƞ){if(Џ){IMyShipConnector Ƕ=в(MyShipConnectorStatus.Connected);
if(Ƕ==null){Σ(Γ.г);return;}IMyShipConnector ǽ=Ƕ.OtherConnector;ΔΛ(Ƕ,false);Ţ(ǻ,false);ά έ=null;if(Vector3.Distance(Ƕ.GetPosition(),Μ.Ε)<5f&&Μ.γ) έ=Μ;if(Vector3.Distance(Ƕ.GetPosition(),Θ.Ε)<5f&&Θ.γ) έ=Θ;if(έ!=null){if(!ǰ(έ,dockDist*ψ,true,out Ѓ)){ǲ(Ь.Ŗ);φ();return;}
н(Ѓ,5);Ъ(έ.β,έ.ΰ,έ.ω,false);}else н(μ+ǽ.WorldMatrix.Forward*dockDist*ψ,5);if(δ==Ρ.А) ǲ(Ь.Ʃ);}if(Р<wpReachedDist){щ(п,true);Σ(Γ.г);return;}}if(Ϊ==Γ.Ř){if(Џ){Ǟ(true);щ(ο,false);Ѓ=Θ.Ε;н(Ѓ,20);Ъ(Θ.β,Θ.ΰ,Θ.ω,false);}if(Р<wpReachedDist/2){Σ(Γ.Ȳ);return;}}if(Ϊ==Γ.Ă){
if(Џ){Ǟ(true);щ(ο,false);int М=-1;double Ǣ=-1;for(int i=Χ.Count-1;i>=0;i--){double Ћ=Vector3.Distance(Χ[i].Ε,μ);if(Ǣ==-1||Ћ<Ǣ){М=i;Ǣ=Ћ;}}if(М==-1){Σ(Γ.ǜ);return;}Ѓ=Χ[М].Ε;н(Ѓ,10);Ъ(Θ.β,Θ.ΰ,Θ.ω,false);}if(Р<wpReachedDist){Σ(Γ.ǜ);return;}}if(Ϊ==Γ.Αχ)
{bool Ǥ=false;if(Џ){Ǟ(true);if((С==Ђ.Ατ||С==Ђ.Ɨ)&&Ɵ()&&
ģ(Ǽ,Φ.GetNaturalGravity())<25&&Ч>=2&&с>=2){Vector3 ů=μ;if(τ>0&&σ<с-1) ů=ȅ(τ-1,σ+1,true);else if(τ<Ч-1&&σ<с-1) ů=ȅ(τ+1,σ+1,true);else if(τ<Ч-1&&σ>0) ů=ȅ(τ+1,σ-1,true);else if(τ>0&&σ>0) ů=ȅ(τ-1,σ-1,true);else Ǥ=true;if(!Ǥ) н(ů,10);
}else Ǥ=true;}if(Р<wpReachedDist/2) Ǥ=true;if(Ǥ){Σ(Γ.Ʊ);return;}}if(Ϊ==Γ.Βά||Ϊ==Γ.Ʊ){if(Џ){ǝ(true,true,false,μ,0);щ(ο,false);Ǟ(true);Љ=-1;Щ=С==Ђ.ΑЃ||С==Ђ.ƞ?0:-1;}bool ď=!Αξ();int Αέ=Ȃ("STONE","ORE",Й.ȳ);if(С==Ђ.Αο||С==Ђ.ƞ||С==Ђ.Ɨ)
Αέ+=Ȃ("ICE","ORE",Й.ȳ);bool ΖЅ=Αέ>0;bool Βφ=false;if(Щ>=0){float ĭ=(float) Math.Sin(Γω(Щ))*Ș/3f;float Ζδ=(float) Math.Cos(Γω(Щ))*Ȁ/3f;Vector3 Ζλ=ȅ(τ,σ,true)+ΒΪ(Ǽ,ĕ*-1,new Vector3(ĭ,Ζδ,0));н(Ζλ,0.3f);if(Р<Math.Min(Ș,Ȁ)/10f) Щ+=5f;if(Щ>=360) Щ=0;}if(Љ==-1||Αέ<Љ){Љ=Αέ;е=0;}
else{е++;if(е>50) Βφ=true;}if(!ΖЅ||ď||Βφ){if(!ď){int Ηυ=Ȃ("","ORE",Й.ȳ);if(Ŝ(true)) ď=true;else if(100-(І(Ηυ,ΕΛ)*100)<minEjection) ď=true;else ǲ(Ь.Ʃ);}if(Βφ&&ď) Π="Ejection failed";if(Ϊ==Γ.Ʊ){if(ď){if(Μ.γ) Σ(Γ.Ă);else{φ();ā();Π="Can´t return, no dock found";}}
else Σ(Γ.ă);}else if(ď) Σ(Γ.ą);else Σ(Γ.Š);return;}}Џ=false;}void Ερ(List<ά>ȱ,int ύ,List<Vector3>Ƈ,float Ɔ,bool ǖ,ref int ѝ){if(ǖ){for(int i=0;i<ȱ.Count;i++) ȱ[i].Х=0;ѝ=-1;return;}if(ύ==0) return;int ѓ=ύ*-1;if(ѝ==-1) ѝ=ѓ>0?1:ȱ.Count-2;int ŵ=0;while(ѝ>=1&&ѝ<ȱ.Count-1)
{if(ŵ>50) return;ŵ++;try{if((ѓ<0&&ѝ>=1)||(ѓ>0&&ѝ<=ȱ.Count-2)){ά η=ȱ[ѝ];bool ΕΗ=false;for(int j=0;j<Ƈ.Count;j++)
if(Vector3.Distance(η.Ε,Ƈ[j])<=Ɔ){ΕΗ=true;break;}if(!ΕΗ){ά ΑΤ=ȱ[ѝ-ѓ];ά ΕΘ=ȱ[ѝ+ѓ];Vector3 Ηρ=η.Ε-ΕΘ.Ε;Vector3 Γδ=ΑΤ.Ε-η.Ε;Vector3 ΗΠ=η.Ε+Vector3.Normalize(Ηρ)*Γδ.Length();Vector3 Ζω=ΑΤ.Ε-ΗΠ;Vector3 ΑΖ=ć(ύ>0?η.ΰ:η.ΰ*-1,η.β*-1,Ζω);Vector3 ΗΓ=ć(ύ>0?η.ΰ:η.ΰ*-1,η.β*-1,Γδ);
Vector3 ΑΘ=ć(ύ>0?η.ΰ:η.ΰ*-1,η.β*-1,η.χ);η.Х=(float) Math.Sqrt(Math.Pow(ΑΤ.Х,2)+Math.Pow(ƕ(-ΗΓ,ΑΘ,η),2));for(int j=0;j<Ƈ.Count;j++)
if(Vector3.Distance(ΑΤ.Ε,Ƈ[j])<=Ɔ){Vector3 ΑΙ=ć(ύ>0?η.ΰ:η.ΰ*-1,η.β*-1,Ƈ[j]-η.Ε);float Ηη=ƕ(-ΑΙ,ΑΘ,η);η.Х=Math.Min(η.Х,Ηη)/2f;}if(ΑΖ.Length()==0) ΑΖ=new Vector3(0,0,1);Vector3 ΗΒ=ć(η.ΰ,η.β*-1,η.χ);float Ηλ=ř(-ΑΖ,ΗΒ,η);float Ʋ=І(Ηλ,Ě);float Γα=(float) Math.Sqrt(І(ΑΖ.Length()*1.0f,0.5f*Ʋ));
η.Х=Math.Min(η.Х,(Γδ.Length()/Γα)*Ǡ);}}}catch{return;}ѝ+=ѓ;}ѝ=-1;}void ΒΫ(bool ǖ){if(ǖ){Ŷ=0;return;}if(Ο==Λ.ί) return;float Ǥ=р*Math.Max(1,д);if(Ȭ==р) Ǥ+=Math.Min(д,Ф);float Ητ=Ч*с*Math.Max(1,д);Ŷ=Math.Max(Ŷ,(float) Math.Min(Ǥ/Ητ*100.0,100));}MyDetectedEntityType ΓЁ()
{try{if(Ǳ(ξ,true)&&!ξ.LastDetectedEntity.IsEmpty()) return ξ.LastDetectedEntity.Type;}catch{}return MyDetectedEntityType.None;}float ΑϏ(bool ǧ){if(Ο==Λ.њ&&ΓЁ()==MyDetectedEntityType.None&&!ś(ξ,true))
return fastSpeed;else return ǧ?œ:Œ;}void ΖΤ(){float ΕΒ=0,Δν=0,đ=0,ΓΛ=0;for(int i=0;i<ȣ.Count;i++){IMyBatteryBlock ũ=ȣ[i];if(!Ǳ(ũ,true)) continue;ΕΒ+=ũ.MaxStoredPower;Δν+=ũ.CurrentStoredPower;đ+=ũ.CurrentInput;ΓΛ+=ũ.CurrentOutput;}Ǵ=(float) Math.Round(І(Δν,ΕΒ)*100,1);
if(đ>=ΓΛ) ǡ=й.ĥ;else ǡ=й.Γϊ;if(đ==0&&ΓΛ==0||Ǵ==100.0) ǡ=й.л;if(ȣ.Count==0) ǡ=й.None;}void ΖΨ(){float Δψ=0;for(int i=0;i<э.Count;i++){IMyGasTank Εΰ=э[i];if(!Ǳ(Εΰ,true)) continue;Δψ+=(float) Εΰ.FilledRatio;}Ȑ=І(Δψ,э.Count)*100f;}void Ζή(){Ğ=0;try{for(int i=0;i<Ȯ.Count;i++)
{IMyReactor ΓΡ=Ȯ[i];if(!Ǳ(ΓΡ,true)) continue;List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();ΓΡ.GetInventory(0).GetItems(Ϗ);float ό=0;for(int j=0;j<Ϗ.Count;j++){MyInventoryItem ς=Ϗ[j];if(ς.Type.SubtypeId.ToUpper()=="URANIUM"&&ς.Type.TypeId.ToUpper().Contains("_INGOT"))
ό+=(float) ς.Amount;}if(ό<Ğ||i==0){Ğ=ό;ΒЏ=ΓΡ.CustomName;}}}catch(Exception e){Ȓ=e;}}void ΖΥ(){ǩ=0;ǣ=0;try{for(int i=0;i<ĳ.Count;i++){IMyTerminalBlock Υ=ĳ[i];if(!Ǳ(Υ,true)) continue;ǣ+=(float) Υ.GetInventory(0).CurrentVolume;ǩ+=(float) Υ.GetInventory(0).MaxVolume;
}ΑΕ=(float) Math.Min(Math.Round(І(ǣ,ǩ)*100,1),100.0);}catch(Exception e){Ȓ=e;}}void ΖΩ(){try{ǐ.Clear();for(int i=0;i<ĳ.Count;i++){IMyTerminalBlock Υ=ĳ[i];if(!Ǳ(Υ,true)) continue;Γξ(Υ,Й.Ĩ);}if(С!=Ђ.ΑЏ){for(int i=0;i<ċ.Count;i++){IMyTerminalBlock Υ=ċ[i];if(!Ǳ(Υ,true)) continue;
Γξ(Υ,Й.ΕЌ);}}ΖΟ(ǐ);}catch(Exception e){Ȓ=e;}}void Εσ(){if(ƾ){if(ΓЀ().Count>ǉ){ƾ=false;if(ж!=б.Ś){φ();if(ж==б.Αψ) ā();if(ж==б.ƨ)
if(Μ.γ) ƛ();else ā();}Π="Damage detected";}}}bool Αξ(){if(!Ǌ) return true;if(δ==Ρ.А){if(т>0&&ǡ!=й.None)
if(Ǵ<=т){Π="Low energy! Move home";return false;}if(Ш>0&&Ȯ.Count>0)
if(Ğ<=Ш){Π="Low fuel: "+ΒЏ;return false;}if(у>0&&э.Count>0)
if(Ȑ<=у){Π="Low hydrogen";return false;}}return true;}List<IMyTerminalBlock>ΓЀ(){List<IMyTerminalBlock>č=new List<IMyTerminalBlock>();for(int i=0;i<ť.Count;i++){IMyTerminalBlock Υ=ť[i];if(ś(Υ,false)) č.Add(Υ);}return č;}bool ś(IMyTerminalBlock Υ,bool ΓΫ){return(!Ǳ(Υ,ΓΫ)||!Υ.IsFunctional);
}bool Ǳ(IMyTerminalBlock Υ,bool ΓΫ){if(Υ==null) return false;try{IMyCubeBlock Δϋ=Me.CubeGrid.GetCubeBlock(Υ.Position).FatBlock;if(ΓΫ) return Δϋ==Υ;else return Δϋ.GetType()==Υ.GetType();}catch{return false;}}float Εϊ(IMyTerminalBlock Υ,String ч,String ΕΨ,String[]ǥ){float Ƿ=0;for(int i=0;i<Υ.InventoryCount;i++)
{IMyInventory ΖЋ=Υ.GetInventory(i);List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();ΖЋ.GetItems(Ϗ);foreach(MyInventoryItem ς in Ϗ){if(ǥ!=null&&(ǥ.Contains(ς.Type.TypeId.ToUpper())||ǥ.Contains(ς.Type.SubtypeId.ToUpper())))
continue;if((ч==""||ς.Type.TypeId.ToUpper()==ч)&&(ΕΨ==""||ς.Type.SubtypeId.ToUpper()==ΕΨ))
Ƿ+=(float) ς.Amount;}}return Ƿ;}Ц ΕϏ(String Ψ,String ν,Й Н,bool Ζτ){Ψ=Ψ.ToUpper();ν=ν.ToUpper();for(int i=0;i<ǐ.Count;i++){Ц ς=ǐ[i];if(ς.Ψ.ToUpper()==Ψ&&ς.ν.ToUpper()==ν&&(ς.Ľ==Н||Н==Й.ȳ))
return ς;}Ц Ϋ=null;if(Ζτ){Ϋ=new Ц(Ψ,ν,0,Н);ǐ.Add(Ϋ);}return Ϋ;}int Ȃ(String Ψ,String ν,Й Н){return Ȃ(Ψ,ν,Н,null);}int Ȃ(String Ψ,String ν,Й Н,String[]ǥ){int Ƿ=0;Ψ=Ψ.ToUpper();ν=ν.ToUpper();for(int i=0;i<ǐ.Count;i++){Ц ς=ǐ[i];if(ǥ!=null&&ǥ.Contains(ς.Ψ.ToUpper())) continue;
if((Ψ==""||ς.Ψ.ToUpper()==Ψ)&&(ν==""||ς.ν.ToUpper()==ν)&&(ς.Ľ==Н||Н==Й.ȳ))
Ƿ+=ς.ό;}return Ƿ;}void Γξ(IMyTerminalBlock Υ,Й Н){for(int i=0;i<Υ.InventoryCount;i++){List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();Υ.GetInventory(i).GetItems(Ϗ);for(int j=0;j<Ϗ.Count;j++)
ΕϏ(Ϗ[j].Type.SubtypeId,Ϗ[j].Type.TypeId.Replace("MyObjectBuilder_",""),Н,true).ό+=(int) Ϗ[j].Amount;}}void ΖΟ(List<Ц>Ѝ){for(int i=Ѝ.Count-1;i>0;i--)
for(int j=0;j<i;j++){Ц a=Ѝ[j];Ц b=Ѝ[j+1];if(a.ό<b.ό) Ѝ.Move(j,j+1);}}bool Ŝ(bool ΗІ){if(Ț&&Ο!=Λ.ί)
if(Т!=-1&&Ě>=Т){Π="Ship too heavy";return true;}if(ΑΕ>=Ǩ||Ɓ){Ɓ=false;Π="Ship is full";return true;}return false;}bool ΓЏ(){String[]ǥ=null;if(!ș) ǥ=new string[]{"ICE"};if(Ο==Λ.ş) return Ȃ("","ORE",Й.Ĩ,ǥ)==0;if(Ο==Λ.њ) return Ȃ("","COMPONENT",Й.Ĩ,ǥ)==0;else return Ȃ("","",Й.Ĩ,ǥ)==0;
}bool ΕЏ(){List<IMyCockpit>Βξ=new List<IMyCockpit>();ώ.GetBlocksOfType(Βξ,q=>q.CubeGrid==Me.CubeGrid);for(int i=0;i<Βξ.Count;i++)
if(Βξ[i].IsUnderControl) return true;return false;}bool Ετ(bool ǖ,bool ΑΪ){IMyShipConnector ў=в(MyShipConnectorStatus.Connected);if(ў==null) return false;if(Vector3.Distance(Μ.Ε,ў.GetPosition())<5)
return Γφ(ϊ,ǖ,ΑΪ);if(Vector3.Distance(Θ.Ε,ў.GetPosition())<5)
return Γφ(ϋ,ǖ,ΑΪ);return false;}bool Γφ(Ĭ ǵ,bool ǖ,bool ΑΪ){if(ǖ) ǵ.ΖΓ();ǵ.Ζέ();bool з=false;String ц="";switch(ǵ.Г){case ζ.ΑБ:{ц="Waiting for command";з=false;break;}case ζ.ΔΕ:{ц="Waiting for cargo";з=Ŝ(true);break;}case ζ.ΔΔ:{ц="Unloading";з=ΓЏ();break;}case ζ.ΒΕ:{з=true;break;}
case ζ.ΑА:{ц="Charging batteries";з=Ǵ>=100f;break;}case ζ.Ʀ:{ц="Discharging batteries";з=Ǵ<=25f;break;}case ζ.ƥ:{ц="Discharging batteries";з=Ǵ<=0f;break;}case ζ.ΒΒ:{ц="Filling up hydrogen";з=Ȑ>=100f;break;}case ζ.ΒΓ:{ц="Unloading hydrogen";з=Ȑ<=25f;break;}
case ζ.ΒΑ:{ц="Unloading hydrogen";з=Ȑ<=0f;break;}case ζ.ΒΔ:{bool ΔЂ=ΕЏ();if(!ΔЂ) ǵ.ΓΞ=true;з=ǵ.ΓΞ&&ΔЂ;ц="Waiting for passengers";break;}}if(!з) ǵ.Αν(true);if(з&&ǵ.ΑЁ()){з=ǵ.Αν(false);ц="Undocking in: "+Ɯ((int) Math.Max(0,ǵ.џ-ǵ.ȧ));}if(ΑΪ) Ελ=ц;return з;}void Εξ(){
if(!ĺ) return;if(ο.Count<=1) return;float Εε=0,Εδ=0;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǭ=ο[i];if(ś(Ǭ,true)) continue;Εε+=(float) Ǭ.GetInventory(0).MaxVolume;Εδ+=(float) Ǭ.GetInventory(0).CurrentVolume;}float Δί=(float) Math.Round(І(Εδ,Εε),5);for(int ΕΝ=0;ΕΝ<Math.Max(1,Math.Floor(ο.Count/10f));ΕΝ++)
{float ΒЀ=0,ΒЎ=0;float ΔЀ=0,ΔА=0;IMyTerminalBlock Ȏ=null,Ȩ=null;for(int i=0;i<ο.Count;i++){IMyTerminalBlock Ǭ=ο[i];if(ś(Ǭ,true)) continue;float ΓΕ=(float) Ǭ.GetInventory(0).MaxVolume;float ǃ=І((float) Ǭ.GetInventory(0).CurrentVolume,ΓΕ);if(Ȏ==null||ǃ>ΒЀ){Ȏ=Ǭ;ΒЀ=ǃ;ΔЀ=ΓΕ;}
if(Ȩ==null||ǃ<ΒЎ){Ȩ=Ǭ;ΒЎ=ǃ;ΔА=ΓΕ;}}if(Ȏ==null||Ȩ==null||Ȏ==Ȩ) return;if(checkConveyorSystem&&!Ȏ.GetInventory(0).IsConnectedTo(Ȩ.GetInventory(0))){if(Βθ>20) Π="Inventory balancing failed";else Βθ++;return;}Βθ=0;List<MyInventoryItem>Ϗ=new List<MyInventoryItem>();
Ȏ.GetInventory(0).GetItems(Ϗ);float Ə=0;if(Ϗ.Count==0) continue;MyInventoryItem Ǆ=Ϗ[0];String ΔЅ=Ǆ.Type.TypeId+Ǆ.Type.SubtypeId;if(!Εκ.TryGetValue(ΔЅ,out Ə)){if(ΕА(Ȏ.GetInventory(0),0,Ȩ.GetInventory(0),out Ə))
Εκ.Add(ΔЅ,Ə);else return;}float Ης=((ΒЀ-Δί)*ΔЀ/Ə);float Ησ=((Δί-ΒЎ)*ΔА/Ə);int Γζ=(int) Math.Min(Ησ,Ης);if(Γζ<=0) return;if((float) Ǆ.Amount<Γζ)
Ȏ.GetInventory(0).TransferItemTo(Ȩ.GetInventory(0),0,null,null,Ǆ.Amount);else
Ȏ.GetInventory(0).TransferItemTo(Ȩ.GetInventory(0),0,null,null,Γζ);}}bool ΕА(IMyInventory Ƃ,int Α,IMyInventory Ζψ,out float Ει){Ει=0;float Ζθ=(float) Ƃ.CurrentVolume;List<MyInventoryItem>Βι=new List<MyInventoryItem>();Ƃ.GetItems(Βι);float Δα=0;for(int i=0;i<Βι.Count;i++) Δα+=(float) Βι[i].Amount;
Ƃ.TransferItemTo(Ζψ,Α,null,null,1);float Εη=Ζθ-(float) Ƃ.CurrentVolume;List<MyInventoryItem>Βί=new List<MyInventoryItem>();Ƃ.GetItems(Βί);float ΔΨ=0;for(int i=0;i<Βί.Count;i++) ΔΨ+=(float) Βί[i].Amount;if(Εη==0f||!ΑЄ(0.9999,Δα-ΔΨ,1.0001))
return false;Ει=Εη;return true;}void ΖΧ(){Vector3 Ɖ=Ѓ-μ;if(Ɖ.Length()==0) Ɖ=new Vector3(0,0,-1);Vector3 ΑΙ=Ы(Φ,Ɖ);Vector3 Εα=Vector3.Normalize(ΑΙ);Vector3 ļ=Ы(Φ,Φ.GetNaturalGravity());float Ηα=ΓΜ>0?Math.Max(0,1-ģ(Ɖ,Δϊ)/5):0;float Х=(float) Math.Min((ΓΒ>0?ΓΒ:1000f),Math.Max(ƕ(-ΑΙ,ļ,null),ΓΜ*Ηα));
if(!Εγ) Х=0;if(ΕΥ) Х=Math.Max(0,1-Ƽ/Ƴ)*Х;if(generalSpeedLimit>0) Х=Math.Min(generalSpeedLimit,Х);if(ΕΦ) Х*=(float) Math.Min(1,І(Ɖ.Length(),wpReachedDist)/2);Vector3 ΔЎ=Ы(Φ,Φ.GetShipVelocities().LinearVelocity);float Ζγ=(float)(Math.Max(0,15-ģ(-Εα,-ΔЎ))/15)*0.85f+0.15f;
Γί+=Math.Sign(Ζγ-Γί)/10f;Vector3 ȥ=Εα*Х*Γί-(ΔЎ);Vector3 Βζ=Αώ(ȥ,null);if(Αή&&Р>0.1f){ȥ.X*=ΒΤ(ȥ.X,ref Αί.X,1f,Βζ.X,20);ȥ.Y*=ΒΤ(ȥ.Y,ref Αί.Y,1f,Βζ.Y,20);ȥ.Z*=ΒΤ(ȥ.Z,ref Αί.Z,1f,Βζ.Z,20);}else Αί=new Vector3(1,1,1);ƈ=Ě*ȥ-ļ*Ě;Γο(ƈ,Αή);Р=Vector3.Distance(μ,Ѓ);
}float ΒΤ(float value,ref float υ,float ѓ,float ȓ,float ΓΓ){value=Math.Sign(Math.Round(value,2));if(value==Math.Sign(υ)) υ+=Math.Sign(υ)*ѓ;else υ=value;if(value==0) υ=1;float Ϋ=Math.Abs(υ);if(Ϋ<ΓΓ||ȓ==0) return Ϋ;υ=Math.Min(ΓΓ,Math.Max(-ΓΓ,υ));Ϋ=Math.Abs(ȓ);return Ϋ;
}void ΔΧ(){float х=90,ї=90,ё=90;float Ǘ=(float)(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?gyroSpeedSmall:gyroSpeedLarge)/100f;Vector3 ǧ,я,Ÿ,ΒЋ;if(ΒЌ){ǧ=Vector3.Normalize(Ѓ-μ);Ÿ=Ы(Φ,ǧ);я=Ы(Φ,Γέ);х=ģ(Ÿ,new Vector3(0,-1,0))-90;ї=Ā(я,new Vector3(-1,0,0),я.Y);
ё=Ā(Ÿ,new Vector3(-1,0,0),Ÿ.Z);}else{ǧ=Γή;я=Ы(Φ,Γέ);Ÿ=Ы(Φ,Γή);ΒЋ=Ы(Φ,Εβ);х=Ā(я,new Vector3(0,0,1),я.Y);ї=Ā(я,new Vector3(-1,0,0),я.Y);ё=Ā(ΒЋ,new Vector3(0,0,1),ΒЋ.X);}if(ΔΪ&&Ɵ()){Vector3 Βϋ=Φ.GetNaturalGravity();я=Ы(Φ,Βϋ);х=Ā(я,new Vector3(0,0,1),я.Y);
ї=Ā(я,new Vector3(-1,0,0),я.Y);}if(!ΑЄ(-45,ї,45)){х=0;ё=0;}if(!ΑЄ(-45,ё,45)) х=0;ƫ(Βύ,1,(-х)*Ǘ,(-ё)*Ǘ,(-ї)*Ǘ);Ƽ=Math.Max(Math.Abs(х),Math.Max(Math.Abs(ї),Math.Abs(ё)));ǋ=Ƽ<=Ƴ;}void Ες(IMyTerminalBlock ΑΥ){if(ΑΥ==null) return;ǚ=new float[3,2];є=new Dictionary<string,float[,]>();
for(int i=0;i<ǿ.Count;i++){IMyThrust Я=ǿ[i];if(!Я.IsFunctional) continue;Vector3 ύ=Ы(ΑΥ,Я.WorldMatrix.Backward);float Ȧ=Я.MaxEffectiveThrust;if(Math.Round(ύ.X,2)!=0.0)
if(ύ.X>=0) ǚ[0,0]+=Ȧ;else ǚ[0,1]-=Ȧ;if(Math.Round(ύ.Y,2)!=0.0)
if(ύ.Y>=0) ǚ[1,0]+=Ȧ;else ǚ[1,1]-=Ȧ;if(Math.Round(ύ.Z,2)!=0.0)
if(ύ.Z>=0) ǚ[2,0]+=Ȧ;else ǚ[2,1]-=Ȧ;String ч=ΓЇ(Я);float[,]ѕ=null;if(є.ContainsKey(ч)) ѕ=є[ч];else{ѕ=new float[3,2];є.Add(ч,ѕ);}float ȓ=Я.MaxThrust;if(Math.Round(ύ.X,2)!=0.0)
if(ύ.X>=0) ѕ[0,0]+=ȓ;else ѕ[0,1]-=ȓ;if(Math.Round(ύ.Y,2)!=0.0)
if(ύ.Y>=0) ѕ[1,0]+=ȓ;else ѕ[1,1]-=ȓ;if(Math.Round(ύ.Z,2)!=0.0)
if(ύ.Z>=0) ѕ[2,0]+=ȓ;else ѕ[2,1]-=ȓ;}}static String ΓЇ(IMyThrust Я){return Я.BlockDefinition.SubtypeId;}Vector3 ΑЈ(Vector3 ь,float[,]ľ){return new Vector3(
ь.X>=0?ľ[0,0]:ľ[0,1],
ь.Y>=0?ľ[1,0]:ľ[1,1],
ь.Z>=0?ľ[2,0]:ľ[2,1]);}bool ΓІ(ά Ω,String ч,out float Βυ){Βυ=0;int Α=Ǿ.IndexOf(ч);if(Α==-1||Ω.Ǚ==null||Α>=Ω.Ǚ.Length)
return false;Βυ=Ω.Ǚ[Α];if(Βυ==-1) return false;return true;}Vector3 Αώ(Vector3 ь,ά Ω){if(Ω!=null){Vector3 Ϋ=new Vector3();for(int i=0;i<є.Keys.Count;i++){String ч=є.Keys.ElementAt(i);float ű=0;if(!ΓІ(Ω,ч,out ű))
return ΑЈ(ь,ǚ);Ϋ+=ΑЈ(ь,є.Values.ElementAt(i))*ű;}return Ϋ;}return ΑЈ(ь,ǚ);}float ř(Vector3 ь,ά Ω){return ř(ь,new Vector3(),Ω);}float ř(Vector3 ь,Vector3 χ,ά Ω){Vector3 Ηκ=Αώ(ь,Ω);Vector3 Ȧ=Ηκ+χ*Ě;float Ę=(Ȧ/ь).AbsMin();return(float)(ь*Ę).Length();}float ƕ(Vector3 ΑΗ,Vector3 ļ,ά Ω)
{if(ΑΗ.Length()==0) return 0;float ΔΩ=1;if(ļ.Length()>0) ΔΩ=Math.Min(1,ģ(-ļ,ΑΗ)/90)*0.4f+0.6f;float ƈ=ř(ΑΗ,ļ,Ω);if(ƈ==0) return 0.1f;float Ʋ=І(ƈ,Ě);float Γα=(float) Math.Sqrt(І(ΑΗ.Length(),Ʋ*0.5f));return Ʋ*Γα*ΔΩ*Ǡ;}float Ɲ(Vector3 ǧ,Vector3 Ǜ,Vector3 χ,ά Ω){if(χ.Length()==0f) return 0;
Vector3 ΑΘ=ć(ǧ,Ǜ,Vector3.Normalize(χ));float Ǐ=ř(-ΑΘ,Ω);return Ǐ/χ.Length();}void Γς(bool ΖЊ){float і=0;float Βψ=0.9f;if(ΖЊ){Т=-1;ğ=0;Αά=null;if(δ!=Ρ.м&&Θ.χ.Length()!=0){і=Βψ*Ɲ(Θ.ΰ,Θ.β*-1,Θ.χ,null);if(і<Т||Т==-1) Т=і;}if(Μ.γ&&Μ.χ.Length()!=0){і=Βψ*Ɲ(Μ.ΰ,Μ.β*-1,Μ.χ,null);
if(і<Т||Т==-1) Т=і;}return;}if(ğ==-1) return;if(ğ>=0){int ΕΟ=0;while(ğ<Χ.Count){if(ΕΟ>100) return;ΕΟ++;ά Ω=Χ[ğ];if(Ω.χ.Length()!=0f){і=Βψ*Math.Min(
Ɲ(Ω.ΰ,Ω.β*-1,Ω.χ,Ω),
Ɲ(Ω.ΰ*-1,Ω.β*-1,Ω.χ,Ω));if(і<Т||Т==-1) Т=і;}else Αά=Ω;ğ++;}ğ=-1;}bool Βώ=true;float ȭ=0;if(Αά!=null){for(int i=0;i<є.Count;i++){String ч=є.Keys.ElementAt(i);float[,]ѕ=є.Values.ElementAt(i);float ű=0;if(!ΓІ(Αά,ч,out ű)){Βώ=false;break;}for(int a=0;a<ѕ.GetLength(0);a++)
for(int b=0;b<ѕ.GetLength(1);b++){float Ċ=Math.Abs(ѕ[a,b]*ű);if(Ċ==0) continue;Βώ=true;if(ȭ==0||Ċ<ȭ) ȭ=Ċ;}}}if(!Βώ){for(int a=0;a<ǚ.GetLength(0);a++)
for(int b=0;b<ǚ.GetLength(1);b++){float Ċ=Math.Abs(ǚ[a,b]);if(Ċ==0) continue;if(ȭ==0||Ċ<ȭ) ȭ=Ċ;}}if(ȭ>0){float ΗΖ=Me.CubeGrid.GridSizeEnum==MyCubeSize.Small?minAccelerationSmall:minAccelerationLarge;і=І(ȭ,ΗΖ);if(і>0)
if(і<Т||Т==-1) Т=і;}}void Γο(Vector3 Ǐ,bool Ќ){if(!Ќ){for(int i=0;i<ǿ.Count;i++)
ǿ[i].SetValueFloat("Override",0.0f);return;}Vector3 Βε=Αώ(Ǐ,null);float ΗЁ=Math.Min(1,Math.Abs(І(Ǐ.X,Βε.X)));float ΗЂ=Math.Min(1,Math.Abs(І(Ǐ.Y,Βε.Y)));float ΗЃ=Math.Min(1,Math.Abs(І(Ǐ.Z,Βε.Z)));for(int i=0;i<ǿ.Count;i++){IMyThrust Я=ǿ[i];Vector3 ύ=ΖΕ(Ы(Φ,Я.WorldMatrix.Backward),1);
if(ύ.X!=0&&Math.Sign(ύ.X)==Math.Sign(Ǐ.X))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЁ);else if(ύ.Y!=0&&Math.Sign(ύ.Y)==Math.Sign(Ǐ.Y))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЂ);else if(ύ.Z!=0&&Math.Sign(ύ.Z)==Math.Sign(Ǐ.Z))
Я.SetValueFloat("Override",Я.MaxThrust*ΗЃ);else
Я.SetValueFloat("Override",0.0f);}}void ƫ(bool Ќ,float ΗΝ,float х,float ё,float ї){for(int i=0;i<ǆ.Count;i++){IMyGyro Ї=ǆ[i];Ї.GyroOverride=Ќ;if(!Ќ) Ї.GyroPower=100;else Ї.GyroPower=ΗΝ;if(!Ќ) continue;Vector3 Ǹ=Φ.WorldMatrix.Forward;Vector3 Ņ=Φ.WorldMatrix.Right;Vector3 Ǜ=Φ.WorldMatrix.Up;
Vector3 ĵ=Ї.WorldMatrix.Forward;Vector3 ķ=Ї.WorldMatrix.Up;Vector3 Ķ=Ї.WorldMatrix.Left*-1;if(ĵ==Ǹ) Ї.SetValueFloat("Roll",ї);else if(ĵ==(Ǹ*-1)) Ї.SetValueFloat("Roll",ї*-1);else if(ķ==(Ǹ*-1)) Ї.SetValueFloat("Yaw",ї);else if(ķ==Ǹ) Ї.SetValueFloat("Yaw",ї*-1);else if(Ķ==Ǹ) Ї.SetValueFloat("Pitch",ї);
else if(Ķ==(Ǹ*-1)) Ї.SetValueFloat("Pitch",ї*-1);if(Ķ==(Ņ*-1)) Ї.SetValueFloat("Pitch",х);else if(Ķ==Ņ) Ї.SetValueFloat("Pitch",х*-1);else if(ķ==Ņ) Ї.SetValueFloat("Yaw",х);else if(ķ==(Ņ*-1)) Ї.SetValueFloat("Yaw",х*-1);else if(ĵ==(Ņ*-1)) Ї.SetValueFloat("Roll",х);
else if(ĵ==Ņ) Ї.SetValueFloat("Roll",х*-1);if(ķ==(Ǜ*-1)) Ї.SetValueFloat("Yaw",ё);else if(ķ==Ǜ) Ї.SetValueFloat("Yaw",ё*-1);else if(Ķ==Ǜ) Ї.SetValueFloat("Pitch",ё);else if(Ķ==(Ǜ*-1)) Ї.SetValueFloat("Pitch",ё*-1);else if(ĵ==Ǜ) Ї.SetValueFloat("Roll",ё);else if(ĵ==(Ǜ*-1)) Ї.SetValueFloat("Roll",ё*-1);
}}void ΔΣ(){this.Βύ=false;}void Ъ(Vector3 Ů,Vector3 Ǹ,Vector3 ΒЊ,float ȗ,bool Ǝ){Αζ(Ů,ȗ,Ǝ);Ƴ=ȗ;ΒЌ=false;this.Γή=Ǹ;this.Εβ=ΒЊ;}void Ъ(Vector3 Ů,Vector3 Ǹ,Vector3 ΒЊ,bool Ǝ){Ъ(Ů,Ǹ,ΒЊ,2f,Ǝ);}void Αζ(Vector3 Ů,float ȗ,bool Ǝ){Ƴ=ȗ;this.Βύ=true;this.ΔΪ=Ǝ;ΒЌ=true;ǋ=false;
this.Γέ=Ů;}void ΒΩ(){ǝ(false,false,false,Ѓ,0);Αή=false;}void н(Vector3 ι,float Ǘ){ǝ(true,false,false,ι,Ǘ);}void ǝ(bool Ќ,bool ΓΨ,bool Γκ,Vector3 ι,float Ǘ){ǝ(Ќ,ΓΨ,Γκ,ι,ι-μ,0.0f,Ǘ);}void ǝ(bool Ќ,bool ΓΨ,bool Γκ,Vector3 ι,Vector3 ΗΫ,float Ηά,float Ǘ){Αή=true;this.Εγ=Ќ;
Ѓ=ι;this.ΓΒ=Ǘ;this.ΓΜ=Ηά;this.ΕΦ=ΓΨ;this.ΕΥ=Γκ;this.Δϊ=ΗΫ;Р=Vector3.Distance(ι,μ);}void ΖΖ(){ώ.GetBlocksOfType(ť,ш);}void ΖΗ(){if(ΑΧ){Є=2;return;}List<IMyRemoteControl>Ž=new List<IMyRemoteControl>();List<IMySensorBlock>ΕΣ=new List<IMySensorBlock>();List<IMyTerminalBlock>ƅ=new List<IMyTerminalBlock>();
ώ.GetBlocksOfType(Ž,ш);ώ.GetBlocksOfType(З,ш);ώ.GetBlocksOfType(ΕΣ,ш);ώ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ƅ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);З=љ(З,pamTag,true);ΔΝ();ΔΞ(ƅ);Ć(З,setLCDFontAndSize,1.4f,false);Ć(ǫ,setLCDFontAndSize,1.4f,true);List<IMySensorBlock>Εέ=љ(ΕΣ,pamTag,true);if(Εέ.Count>0) ξ=Εέ[0];else ξ=null;if(Ο==Λ.ş){ώ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipDrill);
if(ο.Count==0){Є=1;Π="Drills are missing";}}else if(Ο==Λ.њ){ώ.GetBlocksOfType(ο,q=>q.CubeGrid==Me.CubeGrid&&q is IMyShipGrinder);if(ο.Count==0){Є=1;Π="Grinders are missing";}if(ξ==null){Є=1;Π="Sensor is missing";}}else if(Ο==Λ.ί){ώ.GetBlocksOfType(Γβ,q=>q.CubeGrid==Me.CubeGrid);
}List<IMyRemoteControl>Εά=љ(Ž,pamTag,true);if(Εά.Count>0) Ž=Εά;if(Ž.Count>0) Φ=Ž[0];else{Φ=null;Є=2;Π="Remote is missing";return;}ψ=(float) Φ.CubeGrid.WorldVolume.Radius*2;ѐ=null;if(Ο!=Λ.ί){Γσ();if(ο.Count>0&&ѐ!=null){if(ξ!=null&&(ѐ.WorldMatrix.Forward!=ξ.WorldMatrix.Forward||
!(Φ.WorldMatrix.Forward==ξ.WorldMatrix.Up||Φ.WorldMatrix.Down==ξ.WorldMatrix.Down))){Є=1;Π="Wrong sensor direction";}if(ѐ.WorldMatrix.Forward!=Φ.WorldMatrix.Forward&&
ѐ.WorldMatrix.Forward!=Φ.WorldMatrix.Down){Є=2;Π="Wrong remote direction";}}}}void ΖΘ(){ώ.GetBlocksOfType(ǻ,ш);ώ.GetBlocksOfType(п,ш);ώ.GetBlocksOfType(ǿ,ш);ώ.GetBlocksOfType(ǆ,ш);ώ.GetBlocksOfType(ȣ,ш);ώ.GetBlocksOfType(Ȯ,ш);ώ.GetBlocksOfType(э,
q=>q.CubeGrid==Me.CubeGrid&&q.BlockDefinition.ToString().ToUpper().Contains("HYDROGEN"));ώ.GetBlocksOfType(ΑΫ,ш);if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Small)
п=љ(п,"ConnectorMedium",false);else
п=љ(п,"Connector",false);List<IMyShipConnector>ΕΪ=љ(п,pamTag,true);if(ΕΪ.Count>0) п=ΕΪ;if(Є<=1){if(п.Count==0){Є=1;Π="Connector is missing";}if(ǆ.Count==0){Є=1;Π="Gyros are missing";}if(ǿ.Count==0){Є=1;Π="Thrusters are missing";}}List<IMyConveyorSorter>Εή=љ(ΑΫ,pamTag,true);
if(Εή.Count>0) ΑΫ=Εή;List<IMyLandingGear>ΕΫ=љ(ǻ,pamTag,true);if(ΕΫ.Count>0) ǻ=ΕΫ;for(int i=0;i<ǻ.Count;i++) ǻ[i].AutoLock=false;List<IMyBatteryBlock>ΕΩ=љ(ȣ,pamTag,true);if(ΕΩ.Count>0) ȣ=ΕΩ;List<IMyGasTank>Εί=љ(э,pamTag,true);if(Εί.Count>0) э=Εί;}void ΖΙ(){ώ.GetBlocksOfType(ċ,q=>q.CubeGrid==Me.CubeGrid&&q.InventoryCount>0);
ĳ.Clear();for(int i=ċ.Count-1;i>=0;i--){if(ΕЍ(ċ[i])){ĳ.Add(ċ[i]);ċ.RemoveAt(i);}}}bool ΕЍ(IMyTerminalBlock Υ){if(Υ is IMyCargoContainer) return true;if(Υ is IMyShipDrill) return true;if(Υ is IMyShipGrinder) return true;if(Υ is IMyShipConnector){if(((IMyShipConnector) Υ).ThrowOut) return false;
if(Me.CubeGrid.GridSizeEnum!=MyCubeSize.Large&&ΑЂ(Υ,"ConnectorSmall",false))
return false;else return true;}return false;}void Γσ(){if(Φ==null) return;ѐ=null;float ΑΟ=0,ΑΚ=0,ΑΠ=0,ΑΛ=0,ΑΡ=0,ΑΜ=0;List<IMyTerminalBlock>Γά=љ(ο,pamTag,true);bool ΗΚ=Γά.Count==0;if(Γά.Count>0) ѐ=Γά[0];else if(ο.Count>0) ѐ=ο[0];int Ƿ=0;for(int i=0;i<ο.Count;i++)
{if(ο[i].WorldMatrix.Forward!=ѐ.WorldMatrix.Forward){if(ΗΚ){Є=2;Π="Mining direction is unclear!";return;}continue;}Ƿ++;Vector3 Ў=Αϋ(Φ,ο[i].GetPosition());if(i==0){ΑΟ=Ў.X;ΑΚ=Ў.X;ΑΠ=Ў.Y;ΑΛ=Ў.Y;ΑΡ=Ў.Z;ΑΜ=Ў.Z;}ΑΚ=Math.Max(Ў.X,ΑΚ);ΑΟ=Math.Min(Ў.X,ΑΟ);ΑΛ=Math.Max(Ў.Y,ΑΛ);ΑΠ=Math.Min(Ў.Y,ΑΠ);
ΑΜ=Math.Max(Ў.Z,ΑΜ);ΑΡ=Math.Min(Ў.Z,ΑΡ);}Ș=(ΑΚ-ΑΟ)*(1-Ƒ)+drillRadius*2;Ȁ=(ΑΛ-ΑΠ)*(1-ĸ)+drillRadius*2;if(ѐ!=null&&ѐ.WorldMatrix.Forward==Φ.WorldMatrix.Down)
Ȁ=(ΑΜ-ΑΡ)*(1-ĸ)+drillRadius*2;}List<T>љ<T>(List<T>Ѝ,String ǘ,bool Ǉ){List<T>Ϋ=new List<T>();for(int i=0;i<Ѝ.Count;i++)
if(ΑЂ(Ѝ[i],ǘ,Ǉ)) Ϋ.Add(Ѝ[i]);return Ϋ;}bool ΑЂ<T>(T Υ,String ǘ,bool Ǉ){IMyTerminalBlock b=(IMyTerminalBlock) Υ;if(Ǉ&&b.CustomName.ToUpper().Contains(ǘ.ToUpper())) return true;if(!Ǉ&&b.BlockDefinition.ToString().ToUpper().Contains(ǘ.ToUpper())) return true;return false;}void щ<T>(List<T>Ѝ,bool Ќ)
{for(int i=0;i<Ѝ.Count;i++)
ΔΛ((IMyTerminalBlock) Ѝ[i],Ќ);}void ΔΛ(IMyTerminalBlock Υ,bool Ќ){if(Υ==null) return;String Ζα=Ќ?"OnOff_On":"OnOff_Off";var a=Υ.GetActionWithName(Ζα);a.Apply(Υ);}void Ǟ(bool Ќ){ΓΩ=Ќ;if(!Ĝ) return;щ(ΑΫ,Ќ);}void ΒΠ(ChargeMode Ё){for(int i=0;i<ȣ.Count;i++) ȣ[i].ChargeMode=Ё;}void ΒΡ(bool Ηδ)
{for(int i=0;i<э.Count;i++) э[i].Stockpile=Ηδ;}void Ţ(List<IMyLandingGear>Βω,bool ΔЏ){for(int i=0;i<Βω.Count;i++){if(ΔЏ) Βω[i].Lock();if(!ΔЏ) Βω[i].Unlock();}}void ΔΜ(bool Ќ){if(Δξ==Ќ) return;List<IMyShipController>ƻ=new List<IMyShipController>();ώ.GetBlocksOfType(ƻ,ш);if(ƻ.Count==0) return;
for(int i=0;i<ƻ.Count;i++)
ƻ[i].DampenersOverride=Ќ;Δξ=Ќ;}IMyShipConnector в(MyShipConnectorStatus ц){for(int i=0;i<п.Count;i++){if(!Ǳ(п[i],true)) continue;if(п[i].Status==ц) return п[i];}return null;}void Ĉ(bool ΓΥ,bool ǀ,float ΖЂ,float Ζη){if(ξ==null||ο.Count==0) return;Vector3 Ƹ=new Vector3();int Ƿ=0;
for(int i=0;i<ο.Count;i++){if(ο[i].WorldMatrix.Forward!=ѐ.WorldMatrix.Forward) continue;Ƿ++;Ƹ+=ο[i].GetPosition();}Ƹ=Ƹ/Ƿ;Vector3 ł=Αϋ(ξ,Ƹ);ξ.Enabled=true;ξ.ShowOnHUD=ΓΥ;ξ.LeftExtend=(ǀ?1:ы)/2f*Ș-ł.X;ξ.RightExtend=(ǀ?1:ы)/2f*Ș+ł.X;ξ.TopExtend=(ǀ?1:ъ)/2f*Ȁ+ł.Y;
ξ.BottomExtend=(ǀ?1:ъ)/2f*Ȁ-ł.Y;ξ.FrontExtend=(ΓΥ?д:ΖЂ)-ł.Z;ξ.BackExtend=ΓΥ?0:Ζη+ψ*0.75f+ł.Z;ξ.DetectFloatingObjects=true;ξ.DetectAsteroids=false;ξ.DetectLargeShips=true;ξ.DetectSmallShips=true;ξ.DetectStations=true;ξ.DetectOwner=true;ξ.DetectSubgrids=false;
ξ.DetectPlayers=false;ξ.DetectEnemy=true;ξ.DetectFriendly=true;ξ.DetectNeutral=true;}void Ć<T>(List<T>Ѝ,bool Ηή,float ΖЁ,bool ΖЌ){for(int i=0;i<Ѝ.Count;i++){IMyTextSurface Ƅ=null;if(Ѝ[i]is IMyTextSurface) Ƅ=(IMyTextSurface) Ѝ[i];if(Ƅ!=null){Ƅ.ContentType=ContentType.TEXT_AND_IMAGE;
if(!Ηή) continue;Ƅ.Font="Debug";if(ΖЌ) continue;Ƅ.FontSize=ΖЁ;}}}bool Ɵ(){Vector3D И;return this.Φ.TryGetPlanetPosition(out И);}int ΕЇ(Vector3 Ηζ,int ΕΧ){if(ΕΧ==-1) return 0;double М=-1;int Ȍ=-1;for(int i=Χ.Count-1;i>=0;i--){double Ћ=Vector3.Distance(Χ[i].Ε,Ηζ);if(М==-1||Ћ<М){Ȍ=i;М=Ћ;}
}return Math.Sign(Ȍ-ΕΧ);}bool ΕЎ(Vector3 И){List<Vector3>О=new List<Vector3>();for(int i=0;i<Χ.Count;i++) О.Add(Χ[i].Ε);if(Μ.γ&&Χ.Count>=1){Vector3 į=new Vector3();ǰ(Μ,dockDist*ψ,false,out į);if(Vector3.Distance(Μ.Ε,Χ.First().Ε)<Vector3.Distance(Μ.Ε,Χ.Last().Ε)){О.Insert(0,į);О.Insert(0,Μ.Ε);}
else{О.Add(į);О.Add(Μ.Ε);}}if(Ο==Λ.ί){if(Θ.γ&&Χ.Count>=1){Vector3 Βα=new Vector3();ǰ(Θ,dockDist*ψ,false,out Βα);if(Vector3.Distance(Θ.Ε,Χ.First().Ε)<Vector3.Distance(Θ.Ε,Χ.Last().Ε)){О.Insert(0,Βα);О.Insert(0,Θ.Ε);}else{О.Add(Βα);О.Add(Θ.Ε);}}}else{if(δ!=Ρ.м)
if(Χ.Count>0&&Vector3.Distance(Θ.Ε,Χ.First().Ε)<Vector3.Distance(Θ.Ε,Χ.Last().Ε))
О.Insert(0,Θ.Ε);else О.Add(Θ.Ε);}int Ȍ=-1;double Ǣ=-1;for(int i=0;i<О.Count;i++){double Ћ=Vector3.Distance(О[i],И);if(Ћ<Ǣ||Ǣ==-1){Ǣ=Ћ;Ȍ=i;}}if(О.Count==0) return false;double Δσ=Vector3.Distance(О[Ȍ],И);double ΗΞ=Vector3.Distance(О[Math.Max(0,Ȍ-1)],О[Ȍ])*1.5f;
double ΗΙ=Vector3.Distance(О[Math.Min(О.Count-1,Ȍ+1)],О[Ȍ])*1.5f;return Δσ<ΗΞ||Δσ<ΗΙ;}static float І(float a,float b){if(b==0) return 0;return a/b;}Vector3 ΖΕ(Vector3 v,int Βρ){return new Vector3(Math.Round(v.X,Βρ),Math.Round(v.Y,Βρ),Math.Round(v.Z,Βρ));}float ģ(Vector3 a,Vector3 b)
{if(a==b) return 0;float Ζύ=(a*b).Sum;float ΔЊ=a.Length();float ΔЋ=b.Length();if(ΔЊ==0||ΔЋ==0) return 0;float Ϋ=(float)((180.0f/Math.PI)*Math.Acos(Ζύ/(ΔЊ*ΔЋ)));if(float.IsNaN(Ϋ)) return 0;return Ϋ;}float Ā(Vector3 v,Vector3 ΑΥ,float Ηί){float ĭ=ģ(v,ΑΥ);if(Ηί>0f) ĭ*=-1;
if(ĭ>-90f) return ĭ-90f;else return 180f-(-ĭ-90f);}double Γω(float Ζυ){return(Math.PI/180)*Ζυ;}bool ΑЄ(double ΗΕ,double value,double ΓΑ){return(value>=ΗΕ&&value<=ΓΑ);}Vector3 ΖΣ(IMyTerminalBlock Υ,Vector3 Ў){return Vector3D.Transform(Ў,Υ.WorldMatrix);}Vector3 Αϋ(IMyTerminalBlock Υ,Vector3 Ηώ)
{return Ы(Υ,Ηώ-Υ.GetPosition());}Vector3 Ы(IMyTerminalBlock Υ,Vector3 Γν){return Vector3D.TransformNormal(Γν,MatrixD.Transpose(Υ.WorldMatrix));}Vector3 ć(Vector3 ǧ,Vector3 Ǜ,Vector3 Γν){MatrixD ΒБ=MatrixD.CreateFromDir(ǧ,Ǜ);return Vector3D.TransformNormal(Γν,MatrixD.Transpose(ΒБ));}Vector3 ΒΪ(Vector3 ǧ,Vector3 Ǜ,Vector3 Ў)
{MatrixD ΒБ=MatrixD.CreateFromDir(ǧ,Ǜ);return Vector3D.Transform(Ў,ΒБ);}String Л(Vector3 v){return ""+v.X+"|"+v.Y+"|"+v.Z;}Vector3 К(String s){String[]λ=s.Split('|');return new Vector3(float.Parse(Ξ(λ,0)),float.Parse(Ξ(λ,1)),float.Parse(Ξ(λ,2)));}String Ζί(ά Ω){String В=":";
String Ϋ=Л(Ω.Ε)+В+Л(Ω.ΰ)+В+
Л(Ω.β)+В+Л(Ω.ω)+В+Л(Ω.χ);for(int i=0;i<Ω.Ǚ.Length;i++)
Ϋ+=В+Math.Round(Ω.Ǚ[i],3);return Ϋ;}ά ΖΠ(String s){String[]λ=s.Split(':');ά Ω=new ά(
К(Ξ(λ,0)),К(Ξ(λ,1)),
К(Ξ(λ,2)),К(Ξ(λ,3)),
К(Ξ(λ,4)));int Α=5;List<float>Δχ=new List<float>();while(Α<λ.Length){String Αα=Ξ(λ,Α);float f=0;if(!float.TryParse(Αα,out f)) break;Δχ.Add(f);Α++;}Ω.Ǚ=Δχ.ToArray();return Ω;}System.Text.StringBuilder Ň=new System.Text.StringBuilder();void Τ<T>(T value,bool ΗΘ){if(ΗΘ) Ň.Append('\n');Ň.Append(value);}
void Τ<T>(T value){Τ(value,true);}String Ƙ(String s){return s.Replace("&","&#38;").Replace(";","&#59;").Replace("\n","&#10;");}String Ư(String s){return s.Replace("&#10;","\n").Replace("&#59;",";").Replace("&#38;","&");}String Ξ(String[]Ζε,int Α){String Αα=Ζε.ElementAtOrDefault(Α);
if(String.IsNullOrEmpty(Αα)) return "";return Αα;}void Save(){if(ΑΧ||Ο==Λ.к){Storage="";return;}Ň.Clear();Ň.Append(DATAREV+";");Τ(Л(ΑΦ),false);Τ(Л(Μ.ΰ));Τ(Л(Μ.ω));Τ(Л(Μ.β));Τ(Л(Μ.χ));Τ(Л(Μ.Ε));Τ(Л(Μ.о));Τ(Μ.γ);Τ(Л(Θ.Ε));Τ(Л(Θ.χ));Τ(Л(Θ.ΰ));Τ(Л(Θ.β));Τ(Л(Θ.ω));Τ(Л(Θ.о));Τ(Θ.γ);Τ(Л(Ǽ));Τ(Л(ĕ));
Τ(";");Τ((int) Ο,false);Τ((int) δ);Τ((int) ю);Τ(Ǩ);Τ(т);Τ(Ш);Τ(у);Τ(Ǡ);Τ(Ț);Τ(ș);if(Ο==Λ.ί){Τ((int) ϊ.Г);Τ(ϊ.џ);Τ(ϊ.ȧ);Τ(Ƙ(ϊ.ǎ));Τ(Ƙ(ϊ.Ǒ));Τ((int) ϋ.Г);Τ(ϋ.џ);Τ(ϋ.ȧ);Τ(Ƙ(ϋ.ǎ));Τ(Ƙ(ϋ.Ǒ));}else{Τ((int) Ȗ);Τ((int) ж);Τ((int) С);Τ((int) Ǎ);Τ((int) ΑΓ);Τ(Ȱ);Τ(Ĝ);Τ(ĺ);Τ(Ĵ);Τ(ы);Τ(ъ);Τ(д);Τ(œ);Τ(Œ);
Τ(Ƒ);Τ(ĸ);Τ(Ч);Τ(с);Τ(τ);Τ(σ);Τ(Ȭ);Τ(р);Τ(ę);Τ(ŀ);}Τ(";");for(int i=0;i<Ǿ.Count;i++)
Τ((i>0?"|":"")+Ǿ[i],false);Τ(";");for(int i=0;i<Χ.Count;i++)
Τ(Ζί(Χ[i]),i>0);Storage=Ň.ToString();Ň.Clear();}ȃ ΖΔ(){if(Storage=="") return ȃ.ΕБ;String[]ƀ=Storage.Split(';');if(Ξ(ƀ,0)!=DATAREV) return ȃ.ΔΓ;int Α=0;try{String[]Κ=Ξ(ƀ,1).Split('\n');ΑΦ=К(Ξ(Κ,Α++));Μ.ΰ=К(Ξ(Κ,Α++));Μ.ω=К(Ξ(Κ,Α++));Μ.β=К(Ξ(Κ,Α++));Μ.χ=К(Ξ(Κ,Α++));Μ.Ε=К(Ξ(Κ,Α++));
Μ.о=К(Ξ(Κ,Α++));Μ.γ=bool.Parse(Ξ(Κ,Α++));Θ.Ε=К(Ξ(Κ,Α++));Θ.χ=К(Ξ(Κ,Α++));Θ.ΰ=К(Ξ(Κ,Α++));Θ.β=К(Ξ(Κ,Α++));Θ.ω=К(Ξ(Κ,Α++));Θ.о=К(Ξ(Κ,Α++));Θ.γ=bool.Parse(Ξ(Κ,Α++));Ǽ=К(Ξ(Κ,Α++));ĕ=К(Ξ(Κ,Α++));Κ=Ξ(ƀ,2).Split('\n');Α=0;Ο=(Λ) int.Parse(Ξ(Κ,Α++));δ=(Ρ) int.Parse(Ξ(Κ,Α++));
ю=(Ρ) int.Parse(Ξ(Κ,Α++));Ǩ=int.Parse(Ξ(Κ,Α++));т=int.Parse(Ξ(Κ,Α++));Ш=int.Parse(Ξ(Κ,Α++));у=int.Parse(Ξ(Κ,Α++));Ǡ=float.Parse(Ξ(Κ,Α++));Ț=bool.Parse(Ξ(Κ,Α++));ș=bool.Parse(Ξ(Κ,Α++));if(Ο==Λ.ί){ϊ.Г=(ζ) int.Parse(Ξ(Κ,Α++));ϊ.џ=float.Parse(Ξ(Κ,Α++));ϊ.ȧ=float.Parse(Ξ(Κ,Α++));
ϊ.ǎ=Ư(Ξ(Κ,Α++));ϊ.Ǒ=Ư(Ξ(Κ,Α++));ϋ.Г=(ζ) int.Parse(Ξ(Κ,Α++));ϋ.џ=float.Parse(Ξ(Κ,Α++));ϋ.ȧ=float.Parse(Ξ(Κ,Α++));ϋ.ǎ=Ư(Ξ(Κ,Α++));ϋ.Ǒ=Ư(Ξ(Κ,Α++));}else{Ȗ=(ǟ) int.Parse(Ξ(Κ,Α++));ж=(б) int.Parse(Ξ(Κ,Α++));С=(Ђ) int.Parse(Ξ(Κ,Α++));Ǎ=(ј) int.Parse(Ξ(Κ,Α++));ΑΓ=(ǟ) int.Parse(Ξ(Κ,Α++));
Ȱ=bool.Parse(Ξ(Κ,Α++));Ĝ=bool.Parse(Ξ(Κ,Α++));ĺ=bool.Parse(Ξ(Κ,Α++));Ĵ=bool.Parse(Ξ(Κ,Α++));ы=int.Parse(Ξ(Κ,Α++));ъ=int.Parse(Ξ(Κ,Α++));д=int.Parse(Ξ(Κ,Α++));œ=float.Parse(Ξ(Κ,Α++));Œ=float.Parse(Ξ(Κ,Α++));Ƒ=float.Parse(Ξ(Κ,Α++));ĸ=float.Parse(Ξ(Κ,Α++));Ч=int.Parse(Ξ(Κ,Α++));
с=int.Parse(Ξ(Κ,Α++));τ=int.Parse(Ξ(Κ,Α++));σ=int.Parse(Ξ(Κ,Α++));Ȭ=int.Parse(Ξ(Κ,Α++));р=int.Parse(Ξ(Κ,Α++));ę=int.Parse(Ξ(Κ,Α++));ŀ=float.Parse(Ξ(Κ,Α++));}Κ=Ξ(ƀ,3).Replace("\n","").Split('|');Ǿ=Κ.ToList();Κ=Ξ(ƀ,4).Split('\n');Χ.Clear();if(Κ.Count()>=1&&Κ[0]!="")
for(int i=0;i<Κ.Length;i++)
Χ.Add(ΖΠ(Ξ(Κ,i)));}catch{return ȃ.Γϋ;}ň=ю;φ();return ȃ.ΔΥ;}void ΔΝ(){Ď.Clear();for(int i=З.Count-1;i>=0;i--){String ε=З[i].CustomData.ToUpper();bool ΒЃ=false;if(ε=="INSTRUCTIONS"){ΒЃ=true;ƿ=true;}if(ε=="DEBUG") ΒЃ=true;if(ΒЃ){Ď.Add(З[i]);З.RemoveAt(i);}}Ć(Ď,false,1,false);
}void ΔΞ(List<IMyTerminalBlock>Δβ){ǫ.Clear();for(int i=0;i<Δβ.Count;i++){IMyTerminalBlock Υ=Δβ[i];try{String ΕΞ=pamTag.Substring(0,pamTag.Length-1)+":";int Α=Υ.CustomName.IndexOf(ΕΞ);int Ō=-1;if(Α<0||!int.TryParse(Υ.CustomName.Substring(Α+ΕΞ.Length,1),out Ō)) continue;if(Ō==-1) continue;
Ō--;IMyTextSurfaceProvider ΕΠ=(IMyTextSurfaceProvider) Υ;if(Ō<ΕΠ.SurfaceCount&&Ō>=0)
ǫ.Add(ΕΠ.GetSurface(Ō));}catch{}}}String ΑΝ="";void ΔΘ(){String ΒА="";String Ű="";ΑΝ=Γρ(false);ΒА+=ΑΝ;Ű+=ΑΝ;Ű+=ΕЄ();for(int i=0;i<З.Count;i++) З[i].WriteText(ΒА);for(int i=0;i<ǫ.Count;i++) ǫ[i].WriteText(ΒА);Echo(Ű);for(int i=0;i<Ď.Count;i++){IMyTextPanel Ю=Ď[i];
String ε=Ю.CustomData.ToUpper();if(ε=="DEBUG") Ю.WriteText(""+"\n"+"");if(ε=="INSTRUCTIONS") Ю.WriteText(ΓЂ());}}string ΓЂ(){String Β="";try{float ΔБ=Runtime.MaxInstructionCount;Β+="Inst: "+Runtime.CurrentInstructionCount+" Time: "+Math.Round(Runtime.LastRunTimeMs,3)+"\n";Β+="Inst. avg/max: "+(int)(ı*ΔБ)+" / "+(int)(Ŀ*ΔБ)+"\n";
Β+="Inst. avg/max: "+Math.Round(ı*100f,1)+"% / "+Math.Round(Ŀ*100f,1)+"% \n";Β+="Time avg/max: "+Math.Round(Ũ,2)+"ms / "+Math.Round(ΓΔ,2)+"ms \n";}catch{}for(int i=0;i<Ȕ.Count;i++)
Β+=""+Ȕ.Keys.ElementAt(i)+" = "+Math.Round(Ȕ.Values.ElementAt(i)[0],2)+" / "+
Math.Round(Ȕ.Values.ElementAt(i)[1],2)+"%\n";return Β;}void ΕЋ(){String Β="Error occurred! \nPlease copy this and paste it \nin the \"Bugs and issues\" discussion.\n"+
"Run with argument RETRY to restart.\n"+
"Version: "+VERSION+"\n";Ć(З,setLCDFontAndSize,0.9f,false);Ć(ǫ,setLCDFontAndSize,0.9f,true);for(int i=0;i<З.Count;i++) З[i].WriteText(Β+Ȓ.ToString());for(int i=0;i<ǫ.Count;i++) ǫ[i].WriteText(Β+Ȓ.ToString());}void ǳ(){if(!ƿ) return;try{ΓΟ=Runtime.CurrentInstructionCount;}catch{}
}void ǯ(String Ψ){if(!ƿ) return;if(ΓΟ==0) return;try{float Αΰ=(Runtime.CurrentInstructionCount-ΓΟ)/Runtime.MaxInstructionCount*100;if(!Ȕ.ContainsKey(Ψ))
Ȕ.Add(Ψ,new float[]{Αΰ,Αΰ});else{Ȕ[Ψ][0]=Αΰ;Ȕ[Ψ][1]=Math.Max(Αΰ,Ȕ[Ψ][1]);}}catch{}}const String BROADCAST_TAG="[PAMCMD]";const String BROADCAST_SELF_ID="#";const Char BROADCAST_SEP=';';void Εο(String Ǧ){if(!Ĳ) return;String В=""+BROADCAST_SEP;String κ=VERSION+В;κ+=ŕ(ŋ)+В;
κ+=(int) Ο+В;κ+=Ħ(μ.X)+""+В;κ+=Ħ(μ.Y)+В;κ+=Ħ(μ.Z)+В;if(Ο!=Λ.ί)
κ+=Αϊ(ю)+(ю==Ρ.А?" "+Math.Round(Ŷ,1)+"%":"")+В;else κ+=Αϊ(ю)+В;κ+=ŕ(Π)+В;κ+=Ѐ+""+В;κ+=ŕ(ΑΝ)+В;κ+=ǣ+В;κ+=ǩ+В;for(int i=0;i<ǐ.Count;i++){if(ǐ[i].Ľ!=Й.Ĩ) continue;κ+=ŕ(ǐ[i].Ψ)+"/"+ŕ(ǐ[i].ν)+"/"+ǐ[i].ό+В;}ΖΛ(κ,Ǧ);}String ŕ(String s){return s==null?"":s.Replace(BROADCAST_SEP,',');}
void ΖΛ(String κ,String Ǧ){ΒΞ(κ,BROADCAST_SELF_ID,Ǧ);}void ΒΞ(String κ,String ι,String Ǧ){try{if(!Ĳ) return;κ=BROADCAST_TAG+BROADCAST_SEP+Γώ()+BROADCAST_SEP+ι+BROADCAST_SEP+Βλ+BROADCAST_SEP+Ǧ+BROADCAST_SEP+κ;this.IGC.SendBroadcastMessage(BROADCAST_TAG,κ);}catch(Exception e){Ȓ=e;}
}String Γώ(){if(Ο!=Λ.к) return ""+Me.GetId();return BROADCAST_SELF_ID;}bool ΑЇ(String κ){return κ.StartsWith(BROADCAST_TAG);}bool У(ref String ε,out string Ų,Char В){int Α=ε.IndexOf(В);Ų="";if(Α<0) return false;Ų=ε.Substring(0,Α);ε=ε.Remove(0,Α+1);return true;}bool ΔΗ(ref String κ,out String Ǫ,out String Ǧ)
{Ǫ="";Ǧ="";if(!Ĳ) return false;String ǘ="";if(!У(ref κ,out ǘ,BROADCAST_SEP)||!ΑЇ(ǘ)) return false;if(!У(ref κ,out Ǫ,BROADCAST_SEP)) return false;if(!У(ref κ,out ǘ,BROADCAST_SEP)||(ǘ!=Γώ()&&(ǘ!="*"&&Ο!=Λ.к))) return false;if(!У(ref κ,out ǘ,BROADCAST_SEP)||(ǘ!=Βλ)) return false;
if(!У(ref κ,out Ǧ,BROADCAST_SEP)) return false;return true;}void ΓЌ(){bool Ŭ=true;if(Δω){Δω=false;if(Me.CustomData.Contains("Antenna_Name")){Π="Update custom data";Me.CustomData="";}}String ų=(Ο!=Λ.к?"[PAM-Ship]":"[PAM-Controller]")+" Broadcast-settings";try{if(Me.CustomData.Length==0||Me.CustomData.Split('\n')[0]!=ų)
ΓЍ(ų);String[]Κ=Me.CustomData.Split('\n');Ĳ=bool.Parse(Ƨ(ƙ(Κ,"Enable_Broadcast",ref Ŭ)));bool Γλ=false;bool Δρ=true;if(Ĳ){if(Ο!=Λ.к)
ŋ=Ƨ(ƙ(Κ,"Ship_Name",ref Ŭ)).Replace(BROADCAST_SELF_ID,"");Βλ=Ƨ(ƙ(Κ,"Broadcast_Channel",ref Ŭ)).ToLower();Δρ=false;if(ќ==null){ќ=this.IGC.RegisterBroadcastListener(BROADCAST_TAG);ќ.SetMessageCallback("");}List<IMyRadioAntenna>Į=new List<IMyRadioAntenna>();ώ.GetBlocksOfType(Į);bool Βΰ=false;
for(int i=0;i<Į.Count;i++)
if(Į[i].EnableBroadcasting&&Į[i].Enabled){Βΰ=true;break;}if(Į.Count==0) Π="No Antenna found";else if(!Βΰ) Π="Antenna not ready";Γλ=Į.Count==0||!Βΰ;if(ΔΫ&&!Γλ&&Ο!=Λ.к) Π="Antenna ok";}else if(Ο==Λ.к)
Π="Offline - Enable in PB custom data";ΔΫ=Γλ;if(Δρ){if(ќ!=null) this.IGC.DisableBroadcastListener(ќ);ќ=null;}}catch{Ŭ=false;}if(!Ŭ){Π="Reset custom data";ΓЍ(ų);}}void ΓЍ(String ų){String ε=ų+"\n\n"+"Enable_Broadcast="+(Ο==Λ.к?"true":"false")+" \n";ε+=Ο!=Λ.к?"Ship_Name=Your_ship_name_here\n":"";
Me.CustomData=ε+"Broadcast_Channel=#default";}String Ƨ(String ē){int Α=ē.IndexOf("//");if(Α!=-1) ē=ē.Substring(0,Α);String[]λ=ē.Split('=');if(λ.Length<=1) return "";return λ[1].Trim();}String ƙ(String[]Κ,String ΖЏ,ref bool Ė){foreach(String ē in Κ)
if(ē.StartsWith(ΖЏ)) return ē;Ė=false;return "";}void Εω(string Ж){if(ȍ) П=new List<Б>();String Ǫ="";String Ǧ="";if(!ΑЇ(Ж)) ΖΒ(Ж);bool ΕΤ=false;int ΓΗ=50;while(ќ!=null&&ќ.HasPendingMessage&&ΓΗ-->0){MyIGCMessage κ=ќ.AcceptMessage();String ε=κ.Data as String;if(ε==null) continue;
if(ΔΗ(ref ε,out Ǫ,out Ǧ)&&Ǫ!=""&&Ǫ!=BROADCAST_SELF_ID){Б Δ=Αφ(Ǫ,"");if(Δ==null){Δ=new Б(Ǫ);П.Add(Δ);}Δ.ĩ(Ǧ,false,false,true);Ζά(Δ,ε);ΕΤ=true;}}if(ΕΤ)
П.Sort(delegate(Б a,Б b){if(a.Ψ==null&&b.Ψ==null) return 0;else if(a.Ψ==null) return-1;else if(b.Ψ==null) return 1;else return a.Ψ.CompareTo(b.Ψ);});if(ȑ||ȍ){if(ž<=0||ȍ){Π="";ȍ=false;ΖΚ();ΓЌ();ΒЉ=DateTime.Now;}}ΖΦ();}void Ζά(Б Δ,String ε){Δ.ΒІ=DateTime.Now;String Ų="";
У(ref ε,out Δ.Αγ,BROADCAST_SEP);У(ref ε,out Δ.Ψ,BROADCAST_SEP);if(Δ.Αγ!=VERSION) return;String ź="";String[]ń=new string[3];У(ref ε,out ź,BROADCAST_SEP);У(ref ε,out ń[0],BROADCAST_SEP);У(ref ε,out ń[1],BROADCAST_SEP);У(ref ε,out ń[2],BROADCAST_SEP);У(ref ε,out Δ.ΓΪ,BROADCAST_SEP);
У(ref ε,out Δ.Π,BROADCAST_SEP);У(ref ε,out Ų,BROADCAST_SEP);У(ref ε,out Δ.ΑΞ,BROADCAST_SEP);String Εθ="",ΕΓ="";У(ref ε,out Εθ,BROADCAST_SEP);У(ref ε,out ΕΓ,BROADCAST_SEP);Δ.Ϗ.Clear();while(true){String ΔІ;if(!У(ref ε,out ΔІ,BROADCAST_SEP)) break;String[]ΑΒ=ΔІ.Split('/');if(ΑΒ.Count()<3) continue;
int ό=0;if(!int.TryParse(ΑΒ[2],out ό)) continue;Δ.Ϗ.Add(new Ц(ΑΒ[0],ΑΒ[1],ό,Й.Ĩ));}int ΕΖ=0;if(!int.TryParse(Ų,out Δ.Ѐ)) Δ.Ѐ=0;if(!int.TryParse(ź,out ΕΖ)) Δ.Ο=Λ.ȡ;if(!float.TryParse(ń[0],out Δ.Ε.X)) Δ.Ε.X=0;if(!float.TryParse(ń[1],out Δ.Ε.Y)) Δ.Ε.Y=0;if(!float.TryParse(ń[2],out Δ.Ε.Z)) Δ.Ε.Z=0;
if(!float.TryParse(Εθ,out Δ.ǣ)) Δ.ǣ=0;if(!float.TryParse(ΕΓ,out Δ.ǩ)) Δ.ǩ=0;Δ.Ο=(Λ) ΕΖ;Δ.Βσ=(int) Math.Round(Vector3.Distance(Me.GetPosition(),Δ.Ε));Δ.Γε=0;for(int i=0;i<Δ.Ϗ.Count();i++) Δ.Γε+=Δ.Ϗ[i].ό;}Б Αφ(String ǹ,String Ψ){ǹ=ǹ.ToUpper();Ψ=Ψ.ToUpper();for(int i=0;i<П.Count;i++)
{if(ǹ!=""&&П[i].ǹ.ToUpper()==ǹ) return П[i];if(Ψ!=""&&П[i].Ψ!=null&&П[i].Ψ.ToUpper()==Ψ) return П[i];}return null;}void ћ(Б Δ,String Њ){if(Δ==null){for(int i=0;i<П.Count;i++)
П[i].ĩ(Њ,true,false,false);ΒΞ(Њ,"*",Њ);}else{Δ.ĩ(Њ,true,false,false);ΒΞ(Њ,Δ.ǹ,Њ);}}void ΖΒ(string Ж){if(Ж=="") return;var λ=Ж.ToUpper().Split(' ');λ.DefaultIfEmpty("");var Њ=λ.ElementAtOrDefault(0);var Ȋ=λ.ElementAtOrDefault(1);String Ǻ="Invalid argument: "+Ж;if(ρ==Ν.ţ){if(Ј!=null)
{switch(Њ){case "UP":{if(Ѐ<2) break;if(Ј.Ѐ==0){Ѐ=1;return;}ћ(Ј,"UP");return;}case "DOWN":{if(Ѐ<1) break;if(Ѐ==1){ћ(Ј,"MRES");break;}ћ(Ј,"DOWN");break;}case "APPLY":{if(Ѐ<2) break;ћ(Ј,"APPLY");return;}}}}switch(Њ){case "UP":this.ΑЋ(false);return;case "DOWN":this.ΑЊ(false);return;
case "APPLY":this.Ĥ(true);return;}switch(Њ){case "CLEAR":Ευ();return;case "SENDALL":
if(String.IsNullOrEmpty(Ȋ)){Π="Usage: SENDALL <cmd>";return;}ћ(null,Ȋ);return;case "SEND":
if(Ж.Length<="SEND".Length+1){Π="Usage: SEND <ship>:<cmd>";return;}ΖΜ(Ж.Remove(0,"SEND".Length+1));return;}Π=Ǻ;}void ΖΜ(String Ŧ){if(Ŧ=="") return;var λ=Ŧ.Split(':');if(λ.Length!=2){Π="Missing separator \":\"";return;}λ.DefaultIfEmpty("");String ŋ=λ.ElementAtOrDefault(0).Trim();
Б Δ=Αφ("",ŋ);if(Δ!=null) ћ(Δ,λ.ElementAtOrDefault(1).Trim());else Π="Unknown ship: "+ŋ;}void Ευ(){П.Clear();Ј=null;α(Ν.Main);Ѐ=0;}void ΖΚ(){List<IMyTerminalBlock>ƅ=new List<IMyTerminalBlock>();ώ.GetBlocksOfType(З,ш);ώ.SearchBlocksOfName(pamTag.Substring(0,pamTag.Length-1)+":",ƅ,
q=>q.CubeGrid==Me.CubeGrid&&q is IMyTextSurfaceProvider);З=љ(З,pamTag,true);ΔΞ(ƅ);ΔΝ();Ć(З,setLCDFontAndSize,1.15f,false);Ć(ǫ,setLCDFontAndSize,1.15f,true);String ΖЈ=ΓЃ(false);String ΕΙ="//Custom Data is obsolete, please delete it.\n\n";foreach(IMyTerminalBlock Ю in З){if(Ю.CustomData==""){Ю.CustomData=ΓЃ(true);continue;}
if(!Ю.CustomData.Contains(ΖЈ))
if(!Ю.CustomData.Contains(ΕΙ)) Ю.CustomData=ΕΙ+Ю.CustomData;}}void ΖΦ(){String Ű="[PAM]-Controller\n\n"+"Run-arguments: (Type without:[ ])\n"+"———————————————\n"+
"[UP] Menu navigation up\n"+"[DOWN] Menu navigation down\n"+"[APPLY] Apply menu point\n"+
"[CLEAR] Clear miner list\n"+"[SEND ship:cmd] Send to a ship*\n"+"[SENDALL cmd] Send to all ships*\n"+
"———————————————\n\n"+"*[SEND] = Cmd to one ship:\n"+" e.g.: \"SEND Miner 1:homepos\"\n\n"+
"*[SENDALL] = Cmd to all ships:\n"+" e.g.: \"SENDALL homepos\"\n\n";for(int i=0;i<ǫ.Count;i++)
ǫ[i].WriteText(Γπ(Е.ΓБ,"0",false));for(int i=0;i<З.Count;i++){Е Ё=Е.ȡ;String ǔ="";ΖΑ(З[i],out Ё,out ǔ);З[i].WriteText(Γπ(Ё,ǔ,i==0));}Echo(Ű);for(int i=0;i<Ď.Count;i++){IMyTextPanel Ю=Ď[i];String ε=Ю.CustomData.ToUpper();if(ε=="DEBUG") Ю.WriteText(""+"\n"+"");if(ε=="INSTRUCTIONS") Ю.WriteText(ΓЂ());
}}String Ĥ(bool Η){int Ζώ=0;return Ĥ(Η,0,ref Ζώ,false,1);}String Ĥ(bool Η,int ΗΜ,ref int ΗΪ,bool Δη,int Ż){String Β="";String Ŋ="——————————————————\n";String Ηι="--------------------------------------------\n";if(Ј==null||ρ==Ν.Main||ρ==Ν.ΒΟ)
Β+="[PAM]-Controller | "+П.Count+" Connected ships"+"\n";else Β+=ΓЅ(Ј)+"\n";Β+=Ŋ;int Α=0;int Ζ=Ѐ;Ĕ=0;if(Π!=""){Ι(ref Β,Ζ,Α++,Η,Π);}else if(ρ==Ν.Main){if(Ј==null&&П.Count>=1) Ј=П[0];String Ѝ="";if(Ι(ref Ѝ,Ζ,Α++,Η," Send command to all")) α(Ν.ΒΟ);int ŉ=0;
for(int s=0;s<П.Count;s++){if(Δη) Ѝ+="\n";Б Δ=П[s];if(Ζ==Α||Ζ==Α+1) Ј=Δ;if(Α==Ζ) ŉ=Ѝ.Split('\n').Length-1;if(Ι(ref Ѝ,Ζ,Α++,Η," "+ΓЅ(Δ))){Ј=Δ;α(Ν.ţ);Ј.ĩ("",false,true,false);}if(Α==Ζ) ŉ=Ѝ.Split('\n').Length-1;if(Ι(ref Ѝ,Ζ,Α++,Η," "+(Δη?"":"  ")+ΕЈ(Δ)))
{Ј=Δ;α(Ν.ΔΟ);}}int ő=Ż-2;ő+=Math.Max(0,(ΗΜ-1))*Ż;Β+=ΒΝ(ő,Ѝ,ŉ,ref ΗΪ);}else if(ρ==Ν.ƪ||ρ==Ν.ΒΟ){Ν Ζζ=ρ==Ν.ƪ?Ν.ţ:Ν.Main;Б ι=null;if(ρ==Ν.ƪ) ι=Ј;if(Ι(ref Β,Ζ,Α++,Η," Back")){α(Ζζ);}if(Ι(ref Β,Ζ,Α++,Η," [Stop] "+ħ(ι,"STOP"))){ћ(ι,"STOP");}if(Ι(ref Β,Ζ,Α++,Η," [Continue] "+ħ(ι,"CONT"))){ћ(ι,"CONT");}
if(Ι(ref Β,Ζ,Α++,Η," [Move home] "+ħ(ι,"HOMEPOS"))){ћ(ι,"HOMEPOS");}if(Ι(ref Β,Ζ,Α++,Η," [Move to job] "+ħ(ι,"JOBPOS"))){ћ(ι,"JOBPOS");}if(Ι(ref Β,Ζ,Α++,Η," [Full simulation] "+ħ(ι,"FULL"))){ћ(ι,"FULL");}if(ι!=null&&ι.Ο==Λ.ί)
if(Ι(ref Β,Ζ,Α++,Η," [Undock] "+ħ(ι,"UNDOCK"))){ћ(ι,"UNDOCK");}}else if(ρ==Ν.ţ){String Βη="";if(Ι(ref Βη,Ζ,Α++,Η," Back")){α(Ν.Main);}Β+=Βη.Substring(0,Βη.Length-1).PadRight(36,' ');if(Ι(ref Β,Ζ,Α++,Η," Send cmd")){α(Ν.ƪ);}if(!Ј.ΓА()&&!Ј.ΑІ()) Ĕ++;if(Ј.ΑІ()) Β+="No answer received...";
else Β+=ī(Ј,Ј.ΑΞ);}else if(ρ==Ν.ΔΟ){if(Ι(ref Β,Ζ,Α++,Η," Back")){α(Ν.Main);}Β+=Ηι;Β+=ī(Ј,ΓЄ(Ј));}if(!ΓΖ) Ѐ=Math.Min(Ĕ-1,Ѐ);return Β;}String ΓЅ(Б Δ){if(Δ.Αγ!=VERSION) return Δ.Ψ+": Different version!";return Δ.Ψ+": "+ī(Δ,Δ.ΓΪ)+" "+Δ.Βσ+"m";}String ΕЈ(Б Δ){String s=ΒΚ("",І(Δ.ǣ,Δ.ǩ)*100f,100f,8,0,0)+"% ";
for(int i=0;i<Δ.Ϗ.Count;i++){if(i>=5) break;Ц ς=Δ.Ϗ[i];s+=ΔΦ(Αμ(ΕЅ(ς)),3)+" "+Γό(ς.ό)+" ";}return s;}String ΕЅ(Ц ς){if(ς.ν.ToUpper()=="ORE"||ς.ν.ToUpper()=="INGOT"){String Δε=ΕЃ(ς.Ψ.ToUpper());if(Δε!="") return Δε;}return ς.Ψ;}String ī(Б Δ,String Β){if(Δ.ΓА()) return "No signal...("+Ɯ((int)(DateTime.Now-Δ.ΒІ).TotalSeconds)+")";
return Β;}String ΓЃ(bool Ηύ){String Β="";if(Ηύ) Β+="mode=main:1\n\n";Β+="//Available modes:\n"+"//main:<Page>\n"+"//mainX:<Page>  (no empty lines)\n"+"//menu\n"+
"//inventory\n"+"//menu:<shipname>\n"+"//inventory:<shipname>";return Β;}void ΖΑ(IMyTerminalBlock Ю,out Е Ё,out String ǔ){bool Ė=true;String ź=Ƨ(ƙ(Ю.CustomData.Split('\n'),"mode",ref Ė)).ToUpper();String[]λ=ź.Split(':');String Ź=λ.Length>0?λ[0].Trim():"";ǔ=λ.Length>1?λ[1].Trim():"";
Ё=Е.ȡ;if(Ź=="MAIN") Ё=Е.ΔΑ;else if(Ź=="MAINX") Ё=Е.ΔΒ;else if(Ź=="MENU") Ё=Е.ΑЉ;else if(Ź=="INVENTORY") Ё=Е.ΑЅ;else if(Ź=="DEBUG") Ё=Е.Απ;if(Ё==Е.ȡ) ǔ=ź;}String Γπ(Е Ё,String ǔ,bool ΖЀ){int ŷ=15;if(ΖЀ){Δι=ƹ;ƹ=0;Δκ=ƺ;ƺ=0;}String Β="";if(Ё==Е.ΔΑ){int Э=0;
Β=Ĥ(false,Δι,ref Ζπ,true,ŷ);if(!int.TryParse(ǔ,out Э)) return Β;ƹ=Math.Max(Э,ƹ);Э--;return ΔΖ(Э,ŷ,Β);}else if(Ё==Е.ΔΒ){int Э=0;Β=Ĥ(false,Δκ,ref Ζρ,false,ŷ);if(!int.TryParse(ǔ,out Э)) return Β;ƺ=Math.Max(Э,ƺ);Э--;return ΔΖ(Э,ŷ,Β);}if(Ё==Е.ΓБ){return Ĥ(false,0,ref Ζς,true,ŷ);
}else if(Ё==Е.ΑЅ||Ё==Е.ΑЉ||Ё==Е.Απ){Б Δ=Ј;if(ǔ!=""){Δ=Αφ("",ǔ);if(Δ==null) return "Unknown ship: "+ǔ;}else{if(Δ==null) return "No ship on main screen selected.";Β="Selected: ";}String Ŋ="—————————————————————\n";String Γγ="";String Βκ="";if(Ё==Е.ΑЉ){Γγ="Menu";Βκ=ī(Δ,Δ.ΑΞ);}
else if(Ё==Е.ΑЅ){Γγ="Inventory";Βκ=ī(Δ,ΓЄ(Δ));}else if(Ё==Е.Απ){return ī(Δ,ΕЉ(Δ));}Β+=Δ.Ψ+" - "+Δ.Βσ+"m | "+Αμ(""+Γγ)+"\n"+Ŋ;Β+=Βκ;return Β;}return "Unknown command: "+ǔ;}string ΕЉ(Б Δ){String s="";s+=Δ.Αγ+"\n";s+=Δ.Ψ+"\n";s+=Δ.Ε.X+"\n";s+=Δ.Ε.Y+"\n";
s+=Δ.Ε.Z+"\n";s+=Δ.ΓΪ+"\n";s+=Δ.Π+"\n";s+=Δ.Ѐ+"\n";s+=Δ.ΑΞ.Length+"\n";s+=Δ.ǣ+"\n";s+=Δ.ǩ+"\n";s+=Δ.Ϗ.Count()+"\n";return s;}string ΓЄ(Б Δ){String s="";s+=ΒΚ("All",І(Δ.ǣ,Δ.ǩ)*100f,100,30,8,12)+"%\n";s+="\n";for(int i=0;i<Δ.Ϗ.Count();i++){Ц ς=Δ.Ϗ[i];
s+=ΒΚ(ς.Ψ,ς.ό,Δ.Γε,30,8,10)+"\n";}return s;}Д ΕЀ(Б Δ,bool ū,string Њ){Д Ϋ=Д.None;if(Δ==null){for(int i=0;i<П.Count;i++){Д υ=П[i].ĩ(Њ,false,ū,false);if(υ==Д.ΒΛ) Ϋ=υ;if(υ==Д.ΑЎ) return υ;if(υ==Д.š) return υ;}return Ϋ;}else return Δ.ĩ(Њ,false,ū,false);}String ħ(Б Δ,String Њ)
{Д υ=ΕЀ(Δ,false,Њ);if(υ==Д.ΒΛ) return "received!";if(υ==Д.š) return "pending...";if(υ==Д.ΑЎ) return "no answer!";return "";}String ΒΝ(int ő,String Β,int ŉ,ref int ȕ){String[]Κ=Β.Split('\n');if(ŉ>=ȕ+ő-1) ȕ++;ȕ=Math.Min(Κ.Count()-1-ő,ȕ);if(ŉ<ȕ+1) ȕ--;ȕ=Math.Max(0,ȕ);
String Ϋ="";for(int i=0;i<ő;i++){int ΔЌ=i+ȕ;if(ΔЌ>=Κ.Count()) break;Ϋ+=Κ[ΔЌ]+"\n";}return Ϋ;}String ΔΖ(int Э,int Ż,String Β){String[]Κ=Β.Split('\n');int Ηβ=Э*Ż;int ΖϏ=(Э+1)*Ż;String Ϋ="";for(int i=Ηβ;i<ΖϏ;i++){if(i>=Κ.Count()) break;Ϋ+=Κ[i]+"\n";}return Ϋ;
}String Αμ(String s){if(s=="") return s;return s.First().ToString().ToUpper()+s.Substring(1).ToLower();}String ΔΦ(String s,int ΕΑ){if(s=="") return s;if(s.Length>ΕΑ) s=s.Substring(0,ΕΑ-1)+".";return s;}string ΒΚ(String Ē,float value,float ΓΑ,int Δΰ,int ΔЈ,int ΗΛ){float Ę=І(value,ΓΑ)*Δΰ;
String Ʒ="[";for(int i=0;i<Δΰ;i++){if(i<=Ę) Ʒ+="|";else Ʒ+="'";}Ʒ+="]";return Ʒ+" "+ΔΦ(Αμ(Ē),ΔЈ).PadRight(ΔЈ)+"".PadRight(ΗΛ)+Γό(value);}String Γό(float ό){if(ό>=1000000) return Math.Round(ό/1000000f,ό/1000000f<100?1:0)+"M";if(ό>=1000) return Math.Round(ό/1000f,ό/1000f<100?1:0)+"K";
return ""+Math.Round(ό);}String Ɯ(int ſ){if(ſ>=60*60) return Math.Round(ſ/(60f*60f),1)+" h";if(ſ>=60) return Math.Round(ſ/60f,1)+" min";return ""+ſ+" s";}string Ħ(float v){return Math.Round(v,2)+" ";}string ΗЄ(Vector3 v){return "X"+Ħ(v.X)+"Y"+Ħ(v.Y)+"Z"+Ħ(v.Z);}