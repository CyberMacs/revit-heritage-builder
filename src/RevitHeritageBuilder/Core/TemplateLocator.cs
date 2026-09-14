namespace RevitHeritageBuilder;

internal static class TemplateLocator
{
    internal static string FindProjectTemplate()
    {
        string root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk", "RVT 2025", "Templates");

        if (!Directory.Exists(root))
            throw new FileNotFoundException("Revit 2025 project templates were not found.", root);

        string template = Directory.EnumerateFiles(root, "DefaultMetric.rte", SearchOption.AllDirectories).FirstOrDefault()
            ?? Directory.EnumerateFiles(root, "*.rte", SearchOption.AllDirectories).FirstOrDefault();

        if (template == null)
            throw new FileNotFoundException("No Revit 2025 project template (.rte) was found.", root);

        return template;
    }
}

