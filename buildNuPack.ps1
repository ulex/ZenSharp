pushd $args[0]

$nuspec = ".\ZenSharp.nuspec"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((gi .\bin\Release\ZenSharp.Core.dll).FullName).FileVersion
write-host "Version = $version"

$packages = @{
	"ZenSharp" = @{
		'PackageId' = 'ZenSharp';
        'PackageVersion' = $version;
        'DependencyId' = 'ReSharper';
        'DependencyVersion' = '8.2';
        'IntegrationDll' = 'bin\Release\ZenSharp.Integration.dll';
        'TargetDir' = 'ReSharper\v8.2\plugins';
    };
	"ZenSharp_R9" = @{
		'PackageId' = 'Ulex.ZenSharp';
        'PackageVersion' = $version;
        'DependencyId' = 'Wave';
        'DependencyVersion' = '[1.0]';
        'IntegrationDll' = 'bin\Release.R90\ZenSharp.Integration.dll';
        'TargetDir' = 'DotFiles\';
    };
}
foreach ($p in $packages.Values){
    $properties = [String]::Join(";" ,($p.GetEnumerator() | % {("{0}={1}" -f @($_.Key, $_.Value))}))
    write-host $properties
    nuget.exe pack $nuspec -Properties $properties
}

Move-Item *.nupkg .\bin\Release -Force

popd
