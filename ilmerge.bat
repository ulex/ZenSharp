SET INTEGRATIONDLL=%1
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft\ILMerge
SET NETDIR=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
set LIBS=/lib:bin\Release.R90 /lib:%NETDIR% /lib:%NETDIR%\WPF
SET ILMERGED=bin\Release\NLog.dll bin\Release\Nemerle.dll bin\Release\Nemerle.Peg.dll bin\Release\ZenSharp.Core.dll %INTEGRATIONDLL%
SET TARGETPL=/targetplatform:v4,%WINDIR%\Microsoft.NET\Framework\v4.0.30319
ilmerge.exe %LIBS% /out:ZenSharp.dll %TARGETPL% %ILMERGED% 

