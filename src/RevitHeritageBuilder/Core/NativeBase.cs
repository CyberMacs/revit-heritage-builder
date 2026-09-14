using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using System.Text.Json;
namespace RevitHeritageBuilder;
public static partial class Builder {
 static UIApplication UI; static Document D; static string Root; static string InstallRoot;
 static JsonElement Data;
 static Dictionary<string,ElementId> Mats=new();
 static List<string> Notes=new();
 static double M(double v)=>v/0.3048;
 static XYZ P(double x,double y,double z=0)=>new(M(x),M(y),M(z));
 static double N(JsonElement e,string name)=>e.GetProperty(name).GetDouble();
 static XYZ V(JsonElement a)=>new(a[0].GetDouble(),a[1].GetDouble(),a[2].GetDouble());
 static XYZ At(JsonElement f,double u,double v,double z)=>(V(f.GetProperty("origin"))+V(f.GetProperty("tangent"))*u+V(f.GetProperty("normal"))*v+new XYZ(0,0,z))/0.3048;
 static void Log(string s) { Directory.CreateDirectory(Path.Combine(Root,"Logs")); File.AppendAllText(Path.Combine(Root,"Logs","build.log"),DateTime.Now+" "+s+"\n"); }
 static void Tx(Document d,string name,Action action) {using var t=new Transaction(d,name);t.Start();var opt=t.GetFailureHandlingOptions();opt.SetFailuresPreprocessor(new Warn());t.SetFailureHandlingOptions(opt);action();if(t.Commit()!=TransactionStatus.Committed)throw new Exception("Transaction failed: "+name);}
 class Warn:IFailuresPreprocessor {public FailureProcessingResult PreprocessFailures(FailuresAccessor a){bool error=false;foreach(var f in a.GetFailureMessages()){Log(f.GetSeverity()+": "+f.GetDescriptionText());if(f.GetSeverity()==FailureSeverity.Warning)a.DeleteWarning(f);else error=true;}return error?FailureProcessingResult.ProceedWithRollBack:FailureProcessingResult.Continue;}}
 static ElementId Mat(Document d,string name,byte r,byte g,byte b,int trans=0) {
  name=name.Replace("|","-");var existing=new FilteredElementCollector(d).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m=>m.Name==name);
  if(existing!=null)return existing.Id;
  var id=Material.Create(d,name);var mat=(Material)d.GetElement(id);mat.Color=new Color(r,g,b);mat.Transparency=trans;return id;
 }
 public static string Run(UIApplication ui,string installRoot) {
  UI=ui;InstallRoot=installRoot;Root=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"RevitHeritageBuilder");Notes=new();
  foreach(var folder in new[]{"Final","Logs","Work","BackUp"})Directory.CreateDirectory(Path.Combine(Root,folder));
  BuildHouse();
  return Path.Combine(Root,"Final","Ayse_Mayda_Revit2025_v1.0.0.rvt");
 }
 static partial void BuildHouse();
}





