# Revit Heritage Builder

A Revit 2025 add-in that creates a new, native BIM reconstruction of the Ayse Mayda house in Izmir.

It adds a Heritage Builder ribbon tab with two buttons:

- Create Ayse Mayda House creates a new RVT, then opens it in Revit.
- About / Help explains the scope and constraints.

The generated house uses native Revit categories: Walls, hosted Windows, a hosted Door, Floors, Roofs, Curtain Walls, Railings, Stairs, Planting, and editable Generic Model ornament families. It does not import a triangulated Blender mesh or use DirectShape.

> The model is a photo-based reconstruction. Dimensions, hidden sides, and interior layout are estimates. It is not a survey or construction document.

## Requirements

- Autodesk Revit 2025
- Windows
- The installer package includes the compiled add-in, family assets, and geometry data.
- .NET 8 SDK is required only to build the source.

## Install

Download the versioned ZIP from Releases, extract it, and run Install-CurrentUser.ps1. Restart Revit. See INSTALL.md.

## Output

The command creates a new project without editing the currently open RVT. The generated file is saved to:

%LOCALAPPDATA%\RevitHeritageBuilder\Final\Ayse_Mayda_Revit2025_v1.0.0.rvt

The original RVT stays open; the generated house opens as a separate Revit document.

## Build from source

Set REVIT2025_API to C:\Program Files\Autodesk\Revit 2025, then run dotnet build src\RevitHeritageBuilder\RevitHeritageBuilder.csproj -c Release and installer\Package.ps1.

## Repository contents

- src/RevitHeritageBuilder — add-in source and packaged assets.
- installer — per-user installer, uninstaller, and package script.
- docs — release and development notes.
- !Manual — Hungarian project documentation.
- dist — generated local release package; excluded from Git.

## Known limitations

The add-in targets Revit 2025. It intentionally creates the supplied Ayse Mayda sample, rather than converting arbitrary photographs into a building. It uses simplified Revit materials; it does not reproduce the earlier Blender PBR shader network exactly.

No license has been selected yet. Do not redistribute this source as open source until the repository owner adds a license.

