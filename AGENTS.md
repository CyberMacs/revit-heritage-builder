# Revit Heritage Builder agent notes

- Target only Autodesk Revit 2025 unless the user explicitly asks for another release.
- Do not commit Autodesk Revit API DLLs; the build references the locally installed API.
- Keep packaged family assets under src/RevitHeritageBuilder/Resources/Families.
- Validate source changes with dotnet build using Revit 2025 installed locally when available.
- Do not claim a runtime Revit test unless the command was actually run inside Revit.
- Keep the add-in portable: no developer-specific paths in source, manifest templates, or documentation.

