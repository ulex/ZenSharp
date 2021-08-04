@echo off
REM SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319
SET PATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin;%PATH%
SET HostFullIdentifier=

if not defined version set /P VERSION=Version:


powershell -ExecutionPolicy bypass .\patchAssemblyInfo.ps1 %VERSION% || goto :error
msbuild /t:Build /p:Configuration=Release ZenSharp.sln               || goto :error
powershell.exe -ExecutionPolicy ByPass  -File  ".\buildNuPack.ps1"   || goto :error
powershell -ExecutionPolicy bypass .\patchAssemblyInfo.ps1 1.0.*   || goto :error
pause
goto :EOF

:error
echo "Failded with error #%errorlevel%.
exit /b %errorlevel%
pause
