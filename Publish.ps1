$package = (ls .\bin\Release\*.nupkg | sort LastWriteTIme -Descending | select -first 1).FullName

#nuget setApiKey <key guid> -Source https://resharper-plugins.jetbrains.com 

nuget push "$package" -Source https://resharper-plugins.jetbrains.com
pause