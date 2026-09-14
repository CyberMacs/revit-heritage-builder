$ErrorActionPreference = "Stop"
$root = Join-Path $env:APPDATA "Autodesk\Revit\Addins\2025"
Remove-Item -LiteralPath (Join-Path $root "RevitHeritageBuilder.addin") -Force -ErrorAction SilentlyContinue
Remove-Item -LiteralPath (Join-Path $root "RevitHeritageBuilder") -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "Revit Heritage Builder was removed for the current Windows user."

