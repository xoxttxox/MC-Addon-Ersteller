$ErrorActionPreference = "Stop"
$root = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $root

$project = "src\MCAddonCreator\MCAddonCreator.csproj"
$publishDir = "src\MCAddonCreator\bin\Release\net10.0-windows\win-x64\publish"
$releaseDir = "release"
$finalExe = Join-Path $releaseDir "MCAddonCreator.exe"

Write-Host "Publishing MCAddonCreator as single EXE for win-x64..."

dotnet publish $project -c Release -r win-x64 --self-contained true `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:EnableCompressionInSingleFile=true `
  /p:DebugType=None `
  /p:DebugSymbols=false

if (!(Test-Path $releaseDir)) {
  New-Item -ItemType Directory -Path $releaseDir | Out-Null
}

Copy-Item -Force (Join-Path $publishDir "MCAddonCreator.exe") $finalExe

Write-Host ""
Write-Host "Done. The EXE is located here:"
Write-Host (Join-Path $root $finalExe)
