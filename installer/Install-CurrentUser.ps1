[CmdletBinding()]
param(
    [string]$Source = (Split-Path -Parent $MyInvocation.MyCommand.Path)
)

$ErrorActionPreference = "Stop"
$target = Join-Path $env:APPDATA "Autodesk\Revit\Addins\2025\RevitHeritageBuilder"
$addinDirectory = Split-Path -Parent $target
New-Item -ItemType Directory -Force -Path $target, $addinDirectory | Out-Null

Copy-Item -Path (Join-Path $Source "RevitHeritageBuilder.dll") -Destination $target -Force
Copy-Item -Path (Join-Path $Source "Resources") -Destination $target -Recurse -Force

$manifest = Get-Content (Join-Path $Source "RevitHeritageBuilder.addin.template") -Raw
$assembly = Join-Path $target "RevitHeritageBuilder.dll"
$manifest.Replace("__ASSEMBLY_PATH__", $assembly) |
    Set-Content (Join-Path $addinDirectory "RevitHeritageBuilder.addin") -Encoding utf8

Write-Host "Installed for the current Windows user. Restart Revit 2025."

