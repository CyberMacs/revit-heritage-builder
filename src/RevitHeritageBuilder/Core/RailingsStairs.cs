using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
namespace RevitHeritageBuilder;
public static partial class Builder {
 static partial void MakeRailings(){Tx(D,"Native balcony railings",()=>{
  var orig=new FilteredElementCollector(D).OfClass(typeof(RailingType)).Cast<RailingType>().First();
  var type=(RailingType)orig.Duplicate("AM Balcony railing 950");
  var hp=type.get_Parameter(BuiltInParameter.RAILING_SYSTEM_TOP_RAIL_HEIGHT_PARAM);if(hp!=null&&!hp.IsReadOnly)hp.Set(M(.95));
  foreach(var r in Data.GetProperty("records").GetProperty("rail").EnumerateArray()){
   var f=r.GetProperty("F");double a=N(r,"a"),b=N(r,"b"),z=N(r,"z"),v=N(r,"v");
   var loop=new CurveLoop();loop.Append(Line.CreateBound(At(f,a,v,0),At(f,b,v,0)));
   var rail=Railing.Create(D,loop,type.Id,Ground.Id);
   rail.get_Parameter(BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET)?.Set(M(z));
   rail.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("AM native railing; editable path and type. Historic X detailing separately modelled.");
  }
 });}
 static partial void MakeStairs(){
  foreach(var s in new[]{(name:"Front entry",height:.85,width:2.15,start:P(0,-9.2),end:P(0,-7.65)),(name:"Side veranda",height:1.35,width:2.35,start:P(10.75,.7),end:P(8.7,.7))}){
   Level landing=null;Tx(D,"Stair landing datum",()=>{landing=Level.Create(D,M(s.height));landing.Name="AM "+s.name+" landing";});
   using var scope=new StairsEditScope(D,"AM "+s.name+" stairs");var id=scope.Start(Ground.Id,landing.Id);
   Tx(D,"Native straight stair run",()=>{
    var stair=(Stairs)D.GetElement(id);int risers=(int)Math.Ceiling(s.height/.18);stair.DesiredRisersNumber=risers;stair.ActualTreadDepth=s.start.DistanceTo(s.end)/(risers-1);D.Regenerate();var run=StairsRun.CreateStraightRun(D,id,Line.CreateBound(s.start,s.end),StairsRunJustification.Center);
    run.ActualRunWidth=M(s.width);
    ((Stairs)D.GetElement(id)).get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("AM v1.0.0 | native stair, approximate photographed proportions");
   });
   scope.Commit(new Warn());
  }
 }
}

