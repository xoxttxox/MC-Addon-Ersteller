@echo off
setlocal
cd /d "%~dp0.."

echo Restoring...
dotnet restore "src\MCAddonCreator\MCAddonCreator.csproj"

if %ERRORLEVEL% neq 0 (
  echo.
  echo Restore failed.
  pause
  exit /b %ERRORLEVEL%
)

echo Building MCAddonCreator Release...
dotnet build "src\MCAddonCreator\MCAddonCreator.csproj" -c Release --no-restore

if %ERRORLEVEL% neq 0 (
  echo.
  echo Build failed.
  pause
  exit /b %ERRORLEVEL%
)

echo.
echo Build complete.
pause