using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RevitHeritageBuilder;

public static partial class Builder
{
    static partial void MakeDetails()
    {
        Tx(D, "Load packaged ornament families", () =>
        {
            string families = Path.Combine(InstallRoot, "Resources", "Families");
            foreach (string detail in Directory.EnumerateFiles(families, "AM_Detail_*.rfa"))
                PlacePackagedFamily(detail, XYZ.Zero);

            PlacePlanting(families, "AM_Garden_Tree.rfa", "Tree trunk");
            PlacePlanting(families, "AM_Garden_Shrub.rfa", "Shrub");
        });

        Notes.Add("Ornaments are packaged editable Generic Model families. Main walls, windows, doors, roofs, floors, stairs, railings and curtain walls are native Revit elements.");
    }

    static void PlacePackagedFamily(string path, XYZ point)
    {
        if (!D.LoadFamily(path, out Family family))
            family = new FilteredElementCollector(D).OfClass(typeof(Family)).Cast<Family>()
                .FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(path));

        if (family == null) throw new InvalidOperationException("Could not load packaged family: " + path);

        FamilySymbol symbol = (FamilySymbol)D.GetElement(family.GetFamilySymbolIds().First());
        if (!symbol.IsActive) { symbol.Activate(); D.Regenerate(); }
        D.Create.NewFamilyInstance(point, symbol, Ground, StructuralType.NonStructural);
    }

    static void PlacePlanting(string folder, string fileName, string sourcePrefix)
    {
        string path = Path.Combine(folder, fileName);
        if (!D.LoadFamily(path, out Family family))
            family = new FilteredElementCollector(D).OfClass(typeof(Family)).Cast<Family>()
                .FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(path));

        if (family == null) throw new InvalidOperationException("Could not load packaged family: " + path);
        FamilySymbol symbol = (FamilySymbol)D.GetElement(family.GetFamilySymbolIds().First());
        if (!symbol.IsActive) { symbol.Activate(); D.Regenerate(); }

        foreach (var obj in Data.GetProperty("objects").EnumerateArray()
            .Where(o => o.GetProperty("name").GetString().StartsWith(sourcePrefix)))
        {
            var vertices = obj.GetProperty("vertices").EnumerateArray().Select(V).ToArray();
            D.Create.NewFamilyInstance(P(vertices.Average(v => v.X), vertices.Average(v => v.Y)), symbol, Ground, StructuralType.NonStructural);
        }
    }
}

