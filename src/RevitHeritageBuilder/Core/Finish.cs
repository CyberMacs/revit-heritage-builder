using Autodesk.Revit.DB;
using System.Text.Json;
namespace RevitHeritageBuilder;
public static partial class Builder {
 static partial void Finish(){
  View3D view=null;
  Tx(D,"Views, plans and category schedules",()=>{
   var vft=new FilteredElementCollector(D).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().First(t=>t.ViewFamily==ViewFamily.ThreeDimensional);
   view=View3D.CreateIsometric(D,vft.Id);view.Name="AM 3D Native BIM v1.0.0";
   var forward=(P(0,0,5.5)-P(24,-32,23)).Normalize();var right=forward.CrossProduct(XYZ.BasisZ).Normalize();var up=right.CrossProduct(forward).Normalize();
   view.SetOrientation(new ViewOrientation3D(P(24,-32,23),up,forward));view.DetailLevel=ViewDetailLevel.Fine;view.DisplayStyle=DisplayStyle.ShadingWithEdges;
   var pft=new FilteredElementCollector(D).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().First(t=>t.ViewFamily==ViewFamily.FloorPlan);
   foreach(var level in new[]{First,Upper}){var plan=ViewPlan.Create(D,pft.Id,level.Id);plan.Name="AM Plan "+level.Name;plan.Scale=100;}
   foreach(var cat in new[]{BuiltInCategory.OST_Walls,BuiltInCategory.OST_Windows,BuiltInCategory.OST_Doors,BuiltInCategory.OST_Roofs,BuiltInCategory.OST_Floors,BuiltInCategory.OST_Stairs,BuiltInCategory.OST_StairsRailing}){
    var sched=ViewSchedule.CreateSchedule(D,new ElementId(cat));sched.Name="AM "+Category.GetCategory(D,cat).Name+" Schedule";
    var fields=sched.Definition.GetSchedulableFields();
    foreach(var field in fields.Where(f=>new[]{"Family and Type","Type","Level","Base Constraint","Width","Height","Area","Length","Sill Height","Comments"}.Contains(f.GetName(D))).Take(7))sched.Definition.AddField(field);
   }
  });
  Tx(D,"Final regeneration",()=>D.Regenerate());
  string path=Path.Combine(Root,"Final","Ayse_Mayda_Revit2025_v1.0.0.rvt");
  if(File.Exists(path))File.Copy(path,Path.Combine(Root,"BackUp","Ayse_Mayda_"+DateTime.Now.ToString("yyyyMMdd_HHmmss")+".rvt"));
  D.SaveAs(path,new SaveAsOptions{OverwriteExistingFile=true,MaximumBackups=1});
  var counts=new FilteredElementCollector(D).WhereElementIsNotElementType().Where(e=>e.Category!=null&&e.Category.CategoryType==CategoryType.Model).GroupBy(e=>e.Category.Name).ToDictionary(g=>g.Key,g=>g.Count());
  var windows=new FilteredElementCollector(D).OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().Cast<FamilyInstance>().Select(w=>new{id=w.Id.Value,host=w.Host?.Id.Value,type=w.Symbol.Name}).ToArray();
  int ds=new FilteredElementCollector(D).OfClass(typeof(DirectShape)).Count(),imports=new FilteredElementCollector(D).OfClass(typeof(ImportInstance)).Count();
  if(ds!=0||imports!=0||windows.Any(w=>w.host==null))throw new Exception("Native BIM category/host validation failed");
  File.WriteAllText(Path.Combine(Root,"Work","validation.json"),JsonSerializer.Serialize(new{version="1.0.0",counts,directShapes=ds,imports,windows,notes=Notes},new JsonSerializerOptions{WriteIndented=true}));
  var ex=new ImageExportOptions{ExportRange=ExportRange.SetOfViews,FilePath=Path.Combine(Root,"Final","Ayse_Mayda_Revit2025"),HLRandWFViewsFileType=ImageFileType.PNG,ShadowViewsFileType=ImageFileType.PNG,ImageResolution=ImageResolution.DPI_150,ZoomType=ZoomFitType.FitToPage,PixelSize=1800};
  ex.SetViewsAndSheets(new List<ElementId>{view.Id});D.ExportImage(ex);
  var viewId=view.Id;D.Close(false);var opened=UI.OpenAndActivateDocument(path);
  opened.ActiveView=opened.Document.GetElement(viewId) as View;
  Log("SAVED and reopened "+path);
 }
}


