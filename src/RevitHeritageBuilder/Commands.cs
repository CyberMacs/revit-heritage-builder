using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace RevitHeritageBuilder;

[Transaction(TransactionMode.Manual)]
public sealed class CreateAyseMaydaHouseCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData data, ref string message, Autodesk.Revit.DB.ElementSet elements)
    {
        string text =
            "This creates a NEW Revit project containing the Ayse Mayda house sample.\n\n" +
            "It does not modify the currently open project. The result is saved under your local " +
            "RevitHeritageBuilder folder and then opened in Revit.\n\n" +
            "The house is a photo-based reconstruction; hidden parts and dimensions are estimates.";

        if (TaskDialog.Show("Heritage Builder", text, TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel) != TaskDialogResult.Ok)
            return Result.Cancelled;

        try
        {
            string installRoot = Path.GetDirectoryName(typeof(RevitHeritageApplication).Assembly.Location)!;
            string output = Builder.Run(data.Application, installRoot);
            TaskDialog.Show("Heritage Builder", "Native Revit model created and opened.\n\nSaved copy:\n" + output);
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = "The sample could not be generated. See the local RevitHeritageBuilder Logs folder.\n" + ex.Message;
            return Result.Failed;
        }
    }
}

[Transaction(TransactionMode.ReadOnly)]
public sealed class AboutHeritageBuilderCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData data, ref string message, Autodesk.Revit.DB.ElementSet elements)
    {
        TaskDialog.Show(
            "Revit Heritage Builder v0.1.0",
            "Revit 2025 add-in.\n\n" +
            "Create Ayse Mayda House builds a new, native BIM sample: Walls, hosted Windows and Door, Floors, Roofs, Railings, Stairs, Curtain Walls and editable detail families.\n\n" +
            "It does not edit the active project. The output is a photo-based reconstruction, not a survey or construction document.");
        return Result.Succeeded;
    }
}

