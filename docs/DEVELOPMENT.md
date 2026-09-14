# Development notes

The add-in is a Revit API command, not screen automation. It creates a separate project document, constructs category-native elements, saves the result, and opens it in Revit.

## Data and assets

Resources/house_semantic.json is a geometry-oriented export from the earlier Blender reconstruction. It contains no photographs. The packaged RFA files provide hosted window/door and editable ornament/planting families.

## Portability

The add-in gets its install root from Assembly.Location. It writes output, logs, validation data, and backups to %LOCALAPPDATA%\RevitHeritageBuilder, avoiding write permissions in the add-in installation directory.

## Runtime test scope

The predecessor generator was validated in Revit 2025 while creating the delivered Ayse Mayda RVT. This distributable wrapper is compiled against the same Revit 2025 API. Before publishing a release, run the installed ribbon command in a clean Revit 2025 session and confirm that the output file opens and contains native elements.

