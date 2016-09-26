pushd $args[0]

$nuspec = ".\ZenSharp.nuspec"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((gi .\bin\Release\ZenSharp.Core.dll).FullName).FileVersion
write-host "Version = $version"

$packages = @{
	"ZenSharp_R91" = @{
		'PackageId' = 'Ulex.ZenSharp';
        'PackageVersion' = $version;
        'DependencyId' = 'Wave';
        'DependencyVersion' = '[7.0, 8.0)';
        'IntegrationDll' = 'bin\Release\ZenSharp.Integration.dll';
        'TargetDir' = 'DotFiles\';
    };
}
foreach ($p in $packages.Values){
    $properties = [String]::Join(";" ,($p.GetEnumerator() | % {("{0}={1}" -f @($_.Key, $_.Value))}))
    write-host $properties
#.\ilmerge.bat $p['IntegrationDll']
    nuget.exe pack $nuspec -Properties $properties
}

Move-Item *.nupkg .\bin\Release -Force

popd
