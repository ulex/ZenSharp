@echo off
SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319

if not defined version set /P VERSION=Version:


powershell -ExecutionPolicy bypass .\patchAssemblyInfo.ps1 %VERSION% || goto :error
msbuild /t:Build /p:Configuration=Release ZenSharp.sln               || goto :error
powershell.exe -ExecutionPolicy ByPass  -File  ".\buildNuPack.ps1"   || goto :error
goto :EOF

:error
echo "Failded with error #%errorlevel%.
popd
exit /b %errorlevel%

