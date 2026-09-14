using Autodesk.Revit.DB;
namespace RevitHeritageBuilder;
public static partial class Builder {
 static RoofType TileRoof,ZincRoof;
 static FootPrintRoof Roof(XYZ[] points,RoofType type,double baseZ,Dictionary<int,double> slopes,string label) {
  var curves=new CurveArray();for(int i=0;i<points.Length;i++)curves.Append(Line.CreateBound(new XYZ(points[i].X,points[i].Y,0),new XYZ(points[(i+1)%points.Length].X,points[(i+1)%points.Length].Y,0)));
  ModelCurveArray edges=new ModelCurveArray();var roof=D.Create.NewFootPrintRoof(curves,Ground,type,out edges);
  roof.get_Parameter(BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM).Set(M(baseZ));
  int k=0;foreach(ModelCurve edge in edges){roof.set_DefinesSlope(edge,slopes.ContainsKey(k));if(slopes.TryGetValue(k,out var slope))roof.set_SlopeAngle(edge,Math.Atan(slope));k++;}
  roof.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(label+" | native footprint roof, v1.0.0");return roof;
 }
 static FootPrintRoof RoofFace(XYZ[] points,RoofType type,string label) {
  double baseZ=points[0].Z*.3048;
  var edge=points[1]-points[0];var inward=new XYZ(-edge.Y,edge.X,0).Normalize();
  double dz=points[2].Z-points[0].Z,run=Math.Abs((points[2]-points[0]).DotProduct(inward));
  return Roof(points,type,baseZ,new Dictionary<int,double>{{0,Math.Abs(dz/run)}},label);
 }
 static partial void MakeRoofs() {Tx(D,"Native roofs: hip, gables and zinc spire",()=>{
  var original=new FilteredElementCollector(D).OfClass(typeof(RoofType)).Cast<RoofType>().First();
  TileRoof=(RoofType)original.Duplicate("AM Terracotta roof 160 mm");var tc=TileRoof.GetCompoundStructure();tc.SetLayers(new List<CompoundStructureLayer>{new(M(.16),MaterialFunctionAssignment.Structure,Mats["Clay tile 3"])});TileRoof.SetCompoundStructure(tc);
  ZincRoof=(RoofType)original.Duplicate("AM Zinc roof 40 mm");var zc=ZincRoof.GetCompoundStructure();zc.SetLayers(new List<CompoundStructureLayer>{new(M(.04),MaterialFunctionAssignment.Structure,Mats["Weathered blue grey zinc"])});ZincRoof.SetCompoundStructure(zc);
  Roof(new[]{P(-7.55,-6.55),P(7.55,-6.55),P(7.55,6.55),P(-7.55,6.55)},TileRoof,10.27,new(){{0,2.08/5.45},{1,2.08/7.55},{2,2.08/5.45},{3,2.08/7.55}},"Main hipped roof");
  Roof(new[]{P(-3.5,1.9),P(3.5,1.9),P(3.5,6.75),P(-3.5,6.75)},TileRoof,10.28,new(){{1,1.69/3.5},{3,1.69/3.5}},"Rear gable roof");
  Roof(new[]{P(1.2,-1.65),P(7.75,-1.65),P(7.75,3.05),P(1.2,3.05)},TileRoof,10.28,new(){{0,1.69/2.35},{2,1.69/2.35}},"Side gable roof");
  var ring=new[]{(-2.6,-5.8),(-2.6,-6.85),(-1.5,-7.95),(1.5,-7.95),(2.6,-6.85),(2.6,-5.8),(1.5,-4.7),(-1.5,-4.7)};
  var levels=new[]{(10.28,1.0),(10.48,.95),(10.9,.76),(11.65,.57),(12.55,.38),(13.5,.18),(13.83,.13)};
  for(int row=0;row<levels.Length-1;row++)for(int i=0;i<8;i++){
   var a=ring[i];var b=ring[(i+1)%8];var low=levels[row];var high=levels[row+1];
   XYZ Q((double,double) p,(double,double) l)=>P(p.Item1*l.Item2,-6.35+(p.Item2+6.35)*l.Item2,l.Item1);
   RoofFace(new[]{Q(a,low),Q(b,low),Q(b,high),Q(a,high)},ZincRoof,"Tower zinc facet "+row+"-"+i);
  }
  Roof(ring.Select(p=>P(p.Item1*.13,-6.35+(p.Item2+6.35)*.13)).ToArray(),ZincRoof,13.83,new(),"Tower cap");
  RoofFace(new[]{P(-1.45,-8.44,8.62),P(1.45,-8.44,8.62),P(1.45,-7.53,9.05),P(-1.45,-7.53,9.05)},ZincRoof,"Oriel hood");
  Roof(new[]{P(-10,1.4),P(-7,1.4),P(-7,5),P(-10,5)},TileRoof,2.55,new(){{0,.4},{2,.4}},"Service annex roof; position inferred");
 });Log("Roofs created");}
 static partial void MakeVerandas(){Tx(D,"Native curtain walls and veranda floors",()=>{
  var original=new FilteredElementCollector(D).OfClass(typeof(WallType)).Cast<WallType>().First(t=>t.Kind==WallKind.Curtain);
  var type=(WallType)original.Duplicate("AM Timber glazed veranda");
  foreach(var item in Data.GetProperty("records").GetProperty("conservatory").EnumerateArray()){
   var f=item.GetProperty("F");double a=N(item,"a"),b=N(item,"b"),bottom=N(item,"bottom"),top=N(item,"top"),depth=N(item,"depth");
   var points=new[]{At(f,a,0,bottom),At(f,a,depth,bottom),At(f,b,depth,bottom),At(f,b,0,bottom)};
   Slab(points,Ground,.16,"Veranda floor",Mats["Warm limestone | fine pores"],bottom);
   Roof(points,ZincRoof,top+.1,new(),"Veranda canopy");
   for(int i=0;i<3;i++){
    var line=Line.CreateBound(points[i],points[i+1]);var cw=Wall.Create(D,line,type.Id,Ground.Id,M(top-bottom),M(bottom),false,false);
    cw.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("AM native curtain wall; timber frame details separate");
   }
  }
  var annex=new[]{P(-9.8,1.6),P(-7.2,1.6),P(-7.2,4.8),P(-9.8,4.8)};
  for(int i=0;i<4;i++)Wall.Create(D,Line.CreateBound(annex[i],annex[(i+1)%4]),Masonry.Id,Ground.Id,M(2.5),0,false,false);
 });}
}


