@echo off
REM SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319
SET VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
for /f "usebackq tokens=*" %%i in (`%VSWHERE% -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do SET MSBUILD=%%i
if not defined MSBUILD echo "MSBuild not found" && goto :error
SET HostFullIdentifier=

if not defined version set /P VERSION=Version:


powershell -ExecutionPolicy bypass .\patchAssemblyInfo.ps1 %VERSION%             || goto :error
"%MSBUILD%" /t:Build /p:Configuration=Release /p:Version=%VERSION% ZenSharp.sln  || goto :error
powershell.exe -ExecutionPolicy ByPass  -File  ".\buildNuPack.ps1"              || goto :error
powershell -ExecutionPolicy bypass .\patchAssemblyInfo.ps1 1.0.*   || goto :error
pause
goto :EOF

:error
echo "Failded with error #%errorlevel%.
exit /b %errorlevel%
pause
