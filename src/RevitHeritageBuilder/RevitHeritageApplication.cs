using Autodesk.Revit.UI;

namespace RevitHeritageBuilder;

public sealed class RevitHeritageApplication : IExternalApplication
{
    public Result OnStartup(UIControlledApplication app)
    {
        const string tab = "Heritage Builder";
        try { app.CreateRibbonTab(tab); } catch { }

        RibbonPanel panel = app.CreateRibbonPanel(tab, "Ayse Mayda");
        string assembly = typeof(RevitHeritageApplication).Assembly.Location;

        panel.AddItem(new PushButtonData(
            "CreateAyseMaydaHouse",
            "Create\nAyse Mayda House",
            assembly,
            "RevitHeritageBuilder.CreateAyseMaydaHouseCommand"));

        panel.AddItem(new PushButtonData(
            "AboutHeritageBuilder",
            "About /\nHelp",
            assembly,
            "RevitHeritageBuilder.AboutHeritageBuilderCommand"));

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication app) => Result.Succeeded;
}

