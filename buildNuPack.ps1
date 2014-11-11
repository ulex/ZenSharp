pushd $args[0]

$nuspec = ".\ZenSharp.nuspec"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((gi .\bin\Release\ZenSharp.Core.dll).FullName)
write-host "Version = $version"
$xml = [xml] (gc $nuspec)
$node = $xml.SelectSingleNode("package/metadata/version").InnerText = $version.FileVersion

$xml.OuterXml > out.nuspec

nuget.exe pack out.nuspec

Move-Item *.nupkg .\bin\Release -Force
ri .\out.nuspec

popd
