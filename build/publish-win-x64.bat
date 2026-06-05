@echo off
setlocal
cd /d "%~dp0.."

set "PROJECT=src\MCAddonCreator\MCAddonCreator.csproj"
set "RELEASE_DIR=release"

echo Publishing MCAddonCreator as single EXE for win-x64...

if exist "%RELEASE_DIR%" rmdir /s /q "%RELEASE_DIR%"
mkdir "%RELEASE_DIR%"

dotnet publish "%PROJECT%" ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -o "%RELEASE_DIR%" ^
  /p:PublishSingleFile=true ^
  /p:IncludeNativeLibrariesForSelfExtract=true ^
  /p:EnableCompressionInSingleFile=true ^
  /p:DebugType=None ^
  /p:DebugSymbols=false

if %ERRORLEVEL% neq 0 (
  echo.
  echo Publish failed.
  pause
  exit /b %ERRORLEVEL%
)

echo.
echo Done. The EXE is located here:
echo %CD%\%RELEASE_DIR%\MCAddonCreator.exe
pause