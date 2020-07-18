<#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#
.SYNOPSIS
This script copies the STB/STF post-build files to their appropriate locations.

.DESCRIPTION

.PARAMETER ReleaseType
This parameter specifies if this is a build for the Beta or Production
environment.

#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#>

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet('Beta', 'Production')]
    [string]$ReleaseType
)

#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

# 1. Copy the VirtualResource\Deployment and VirtualResource\Distribution folders.

    if($ReleaseType -eq 'Beta')
    {
        if(Test-Path C:\STFBuilds\latest_beta)
        {
            Remove-Item C:\STFBuilds\latest_beta -Recurse
        }
        md C:\STFBuilds\latest_beta\VirtualResource\Deployment -Verbose
        Copy $env:WORKSPACE\VirtualResource\Deployment -Destination C:\STFBuilds\latest_beta\VirtualResource -Recurse -Verbose
        Copy $env:WORKSPACE\VirtualResource\Distribution -Destination C:\STFBuilds\latest_beta\VirtualResource -Recurse -Verbose
        Copy-Item $env:WORKSPACE\STBBuild\ -Destination C:\STFBuilds\latest_beta\STBBuild -Recurse -Force -Verbose
    }
    else
    {
        if(Test-Path C:\STFBuilds\latest_production)
        {
            Remove-Item C:\STFBuilds\latest_production -Recurse
        }
        md C:\STFBuilds\latest_production\VirtualResource\Deployment -Verbose
        Copy $env:WORKSPACE\VirtualResource\Deployment -Destination C:\STFBuilds\latest_production\VirtualResource -Recurse -Verbose
        Copy $env:WORKSPACE\VirtualResource\Distribution -Destination C:\STFBuilds\latest_production\VirtualResource -Recurse -Verbose
        Copy-Item $env:WORKSPACE\STBBuild\ -Destination C:\STFBuilds\latest_production\STBBuild -Recurse -Force -Verbose
    }

# 2. Get the build's version string.

    $versionString = "1.0.0.0"

    Write-Host "Read the CommonAssemblyInfo.cs file and locate the line that contains the assembly version string"
    (Get-Content -Path $env:WORKSPACE\ScalableTestFramework\Source\CommonAssemblyInfo.cs) | ForEach-Object {
        If($_ -match '^\[assembly: AssemblyVersion\("(.*)"\)\]')
        {
            $assemblyVersion = [version]$Matches[1]
            $versionString = "{0}.{1}.{2}.{3}" -f $assemblyVersion.Major, $assemblyVersion.Minor, $assemblyVersion.Build, $assemblyVersion.Revision
            Write-Host "Version = $versionString"
        }
    }

# 3. Copy the STFAdminConsoleSetup executable.

    New-Item -ItemType "directory" -Force -Path \\rdlfs.rdl.boi.rd.hpicorp.net\STF-STB_Build_Archive\$versionString -Verbose
    Move $env:WORKSPACE\VirtualResource\Setup\*.exe \\rdlfs.rdl.boi.rd.hpicorp.net\STF-STB_Build_Archive\$versionString -Force -Verbose

# 4. Copy the STB installer executables.

    Move $env:WORKSPACE\STBBuild\Installers\*.exe \\rdlfs.rdl.boi.rd.hpicorp.net\STF-STB_Build_Archive\$versionString -Force -Verbose

# 5. Copy the Plugin libraries.
    New-Item -ItemType "directory" -Force -Path \\rdlfs.rdl.boi.rd.hpicorp.net\STF-STB_Build_Archive\$versionString\Plugins -Verbose
    Move $env:WORKSPACE\VirtualResource\Distribution\Plugin\Plugin.*.dll \\rdlfs.rdl.boi.rd.hpicorp.net\STF-STB_Build_Archive\$versionString\Plugins -Force -Verbose
