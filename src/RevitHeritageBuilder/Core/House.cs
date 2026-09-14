using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Text.Json;
namespace RevitHeritageBuilder;
public static partial class Builder {
 static Level Ground,First,Upper,Eaves;static WallType Masonry;
 static List<(string face,double a,double b,double bottom,double top,Wall wall)> Hosts=new();
 static Dictionary<string,FamilySymbol> WindowTypes=new();
 static partial void BuildHouse() {
  Log("BEGIN native house v1.0.0");Hosts=new();WindowTypes=new();Mats=new();
  Data=JsonDocument.Parse(File.ReadAllText(Path.Combine(InstallRoot,"Resources","house_semantic.json"))).RootElement;
  var windowPath=Path.Combine(InstallRoot,"Resources","Families","AM_Heritage_Window.rfa");var doorPath=Path.Combine(InstallRoot,"Resources","Families","AM_Heritage_Door.rfa");
  D=UI.Application.NewProjectDocument(TemplateLocator.FindProjectTemplate());
  Tx(D,"Project levels and materials",()=>{
   D.ProjectInformation.Name="Ayse Mayda | Native Revit 2025 v1.0.0";D.ProjectInformation.Number="AM-1.0.0";
   foreach(var j in Data.GetProperty("materials").EnumerateArray()){
    string name=j.GetProperty("name").GetString();var c=j.GetProperty("color");
    byte C(int i)=>(byte)Math.Clamp(Math.Round(Math.Pow(c[i].GetDouble(),1/2.2)*255),0,255);
    Mats[name]=Mat(D,"AM "+name,C(0),C(1),C(2),name.Contains("glass")?65:0);
   }
   Ground=Level.Create(D,0);Ground.Name="AM 00 Site";
   First=Level.Create(D,M(1.22));First.Name="AM 01 Ground floor";
   Upper=Level.Create(D,M(5.47));Upper.Name="AM 02 Upper floor";
   Eaves=Level.Create(D,M(10.2));Eaves.Name="AM 03 Eaves";
  });
  Tx(D,"Native masonry walls",()=>{
   var original=new FilteredElementCollector(D).OfClass(typeof(WallType)).Cast<WallType>().First(t=>t.Kind==WallKind.Basic);
   Masonry=(WallType)original.Duplicate("AM Masonry 400 mm");
   Masonry.SetCompoundStructure(CompoundStructure.CreateSimpleCompoundStructure(new List<CompoundStructureLayer>{
    new(M(.03),MaterialFunctionAssignment.Finish1,Mats["Lime brick bond X"]),new(M(.34),MaterialFunctionAssignment.Structure,Mats["Warm limestone | fine pores"]),new(M(.03),MaterialFunctionAssignment.Finish2,Mats["Ivory carved stone"])}));
   foreach(var item in Data.GetProperty("records").GetProperty("wall").EnumerateArray()){
    var f=item.GetProperty("F");string name=f.GetProperty("name").GetString();double a=N(item,"a"),b=N(item,"b"),top=N(item,"top");
    var spans=name=="Tower face 2"?new[]{(0.0,5.47,Ground),(5.47,top,Upper)}:new[]{(0.0,1.22,Ground),(1.22,5.47,First),(5.47,top,Upper)};
    foreach(var s in spans){
     var line=Line.CreateBound(At(f,a,-.2,s.Item1),At(f,b,-.2,s.Item1));
     var w=Wall.Create(D,line,Masonry.Id,s.Item3.Id,M(s.Item2-s.Item1),0,false,false);
     D.Regenerate();if(w.Orientation.DotProduct(V(f.GetProperty("normal")))<0)w.Flip();
     w.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(name+" | "+(name.Contains("Inferred")?"HYPOTHESIS":"photo-based; dimensions estimated"));
     Hosts.Add((name,a,b,s.Item1,s.Item2,w));
    }
   }
  });Log("Walls: "+Hosts.Count);
  Family window=null,door=null;
  Tx(D,"Load hosted families",()=>{D.LoadFamily(windowPath,out window);D.LoadFamily(doorPath,out door);});
  Tx(D,"Hosted native windows and doors",()=>{
   var winbase=(FamilySymbol)D.GetElement(window.GetFamilySymbolIds().First());
   foreach(var item in Data.GetProperty("records").GetProperty("window").EnumerateArray()){
    var f=item.GetProperty("F");double u=N(item,"u"),z=N(item,"bottom"),w=N(item,"w"),h=N(item,"h");
    var host=Hosts.First(x=>x.face==f.GetProperty("name").GetString()&&u>=x.a-.001&&u<=x.b+.001&&z>=x.bottom-.001&&z+h<=x.top+.001);
    PlaceWindow(winbase,host.wall,At(f,u,-.2,z),w,h,z,host.bottom,item.GetProperty("arched").GetBoolean()?"Arched casing approximated":"Rectangular");
   }
   var tower=Hosts.First(x=>x.face=="Tower face 2"&&x.bottom==0);
   var doorbase=(FamilySymbol)D.GetElement(door.GetFamilySymbolIds().First());doorbase.get_Parameter(BuiltInParameter.FAMILY_WIDTH_PARAM).Set(M(1.35));doorbase.get_Parameter(BuiltInParameter.FAMILY_HEIGHT_PARAM).Set(M(2.8));doorbase.Activate();D.Regenerate();
   var doorinst=D.Create.NewFamilyInstance(P(0,-7.45,.85),doorbase,tower.wall,Ground,StructuralType.NonStructural);
   doorinst.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM)?.Set(M(.85));
   var towerup=Hosts.First(x=>x.face=="Tower face 2"&&x.bottom>0);
   PlaceWindow(winbase,towerup.wall,P(0,-7.45,6.05),2.05,2.8,6.05,5.47,"Behind oriel");
  });
  MakeFloors();MakeRoofs();MakeVerandas();MakeRailings();MakeStairs();MakeDetails();Finish();
 }
 static void PlaceWindow(FamilySymbol basic,Wall wall,XYZ pos,double width,double height,double z,double baseZ,string comment){
  string name=$"{width*1000:0} x {height*1000:0}";
  if(!WindowTypes.TryGetValue(name,out var type)){
   type=(FamilySymbol)basic.Duplicate(name);type.get_Parameter(BuiltInParameter.FAMILY_WIDTH_PARAM).Set(M(width));type.get_Parameter(BuiltInParameter.FAMILY_HEIGHT_PARAM).Set(M(height));type.Activate();WindowTypes[name]=type;D.Regenerate();
  }
  var level=(Level)D.GetElement(wall.LevelId);var fi=D.Create.NewFamilyInstance(pos,type,wall,level,StructuralType.NonStructural);
  fi.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM)?.Set(M(z-baseZ));
  fi.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(comment+" | AM v1.0.0");
 }
 static Floor Slab(XYZ[] points,Level level,double thick,string name,ElementId mat,double offset=0){
  var original=new FilteredElementCollector(D).OfClass(typeof(FloorType)).Cast<FloorType>().First();
  var ft=(FloorType)original.Duplicate("AM "+name+" "+Guid.NewGuid().ToString()[..4]);
  var cs=ft.GetCompoundStructure();cs.SetLayers(new List<CompoundStructureLayer>{new(M(thick),MaterialFunctionAssignment.Structure,mat)});ft.SetCompoundStructure(cs);
  var loop=new CurveLoop();for(int i=0;i<points.Length;i++)loop.Append(Line.CreateBound(new XYZ(points[i].X,points[i].Y,0),new XYZ(points[(i+1)%points.Length].X,points[(i+1)%points.Length].Y,0)));
  var floor=Floor.Create(D,new List<CurveLoop>{loop},ft.Id,level.Id);floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(M(offset));return floor;
 }
 static void MakeFloors(){Tx(D,"Native floors and balconies",()=>{
  foreach(var level in new[]{First,Upper})Slab(new[]{P(-7,-6),P(7,-6),P(7,6),P(-7,6)},level,.2,"Timber floor",Mats["Aged chestnut timber"]);
  Slab(new[]{P(-2.48,-6),P(-2.48,-6.85),P(-1.6,-8.08),P(1.6,-8.08),P(2.48,-6.85),P(2.48,-6)},Upper,.19,"Tower balcony",Mats["Warm limestone | fine pores"],.38);
  Slab(new[]{P(-3.25,5.85),P(3.25,5.85),P(3.25,7.35),P(-3.25,7.35)},Upper,.2,"Rear balcony",Mats["Warm limestone | fine pores"],.25);
  Slab(new[]{P(-15,-14),P(15,-14),P(15,14),P(-15,14)},Ground,.25,"Site base",Mats["Garden gravel"]);
 });}
}


