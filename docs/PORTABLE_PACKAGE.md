# Portable package

The local release package is generated under Final/Portable and can be copied to another Windows computer without source compilation.

It contains:

- RevitHeritageBuilder.dll;
- Resources with all required RFA families and the geometry data;
- per-user installer and uninstaller scripts;
- add-in manifest template;
- English and Hungarian installation documentation;
- MIT license and SHA-256 checksums.

The folder and its ZIP are intentionally excluded from Git because they are generated release artifacts. Build them with installer/Package.ps1, then use the documented portable assembly step from the project manual.

The installer writes only to the current Windows user's Revit 2025 add-in folder. It does not modify the Autodesk Revit installation directory.

