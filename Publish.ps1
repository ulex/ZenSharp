$items = ls .\bin\Release\*.nupkg
$package = $items | Out-GridView -OutputMode Single
if ($package -eq $null){
    throw "Nothing selected"
}
#nuget setApiKey <key guid> -Source https://resharper-plugins.jetbrains.com 

nuget push "$package" -Source https://resharper-plugins.jetbrains.com
pause
