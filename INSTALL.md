# Installation

## End users

1. Download and extract RevitHeritageBuilder-v0.1.0.zip.
2. Close Autodesk Revit 2025.
3. Right-click Install-CurrentUser.ps1 and choose Run with PowerShell. If Windows asks for permission, allow the script to write only to your current user's Revit add-in folder.
4. Start Revit 2025.
5. Open the Heritage Builder tab and choose Create Ayse Mayda House.
6. Confirm the dialog. The add-in creates and opens a separate RVT.

The installer writes only to:

%APPDATA%\Autodesk\Revit\Addins\2025\

It does not modify the Revit installation folder or the current project.

## Uninstall

Close Revit and run Uninstall-CurrentUser.ps1 from the extracted package.

## Troubleshooting

- The tab is missing: restart Revit after installation. Check that RevitHeritageBuilder.addin exists under %APPDATA%\Autodesk\Revit\Addins\2025.
- Revit reports that the add-in cannot load: confirm that you use Revit 2025 and that all files from the ZIP remain together in the installed RevitHeritageBuilder folder.
- No metric template is found: repair or install Revit 2025 metric templates. The add-in searches the installed Revit 2025 Templates folder.
- Output folder: review %LOCALAPPDATA%\RevitHeritageBuilder\Logs\build.log if generation fails.

