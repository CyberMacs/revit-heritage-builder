[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$RevitApiPath = "C:\Program Files\Autodesk\Revit 2025"
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $projectRoot "src\RevitHeritageBuilder\RevitHeritageBuilder.csproj"
$dist = Join-Path $projectRoot "dist\RevitHeritageBuilder-v0.1.0"
$zip = Join-Path $projectRoot "dist\RevitHeritageBuilder-v0.1.0.zip"

$env:REVIT2025_API = $RevitApiPath
dotnet build $project -c $Configuration --nologo -p:NoWarn=MSB3277
if ($LASTEXITCODE -ne 0) { throw "Build failed." }

Remove-Item -LiteralPath $dist -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $dist | Out-Null
$bin = Join-Path $projectRoot "src\RevitHeritageBuilder\bin\$Configuration\net8.0-windows"
Copy-Item (Join-Path $bin "RevitHeritageBuilder.dll") $dist
Copy-Item (Join-Path $bin "Resources") $dist -Recurse
Copy-Item (Join-Path $PSScriptRoot "RevitHeritageBuilder.addin.template") $dist
Copy-Item (Join-Path $PSScriptRoot "Install-CurrentUser.ps1") $dist
Copy-Item (Join-Path $PSScriptRoot "Uninstall-CurrentUser.ps1") $dist
Copy-Item (Join-Path $projectRoot "README.md") $dist
Copy-Item (Join-Path $projectRoot "INSTALL.md") $dist
Copy-Item (Join-Path $projectRoot "LICENSE") $dist

Remove-Item -LiteralPath $zip -Force -ErrorAction SilentlyContinue
Compress-Archive -Path (Join-Path $dist "*") -DestinationPath $zip
Write-Host "Package created: $zip"

