<#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#
.SYNOPSIS
This script prepares the STB staging folder and files for creation of the STB
installers.

.DESCRIPTION

#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#>

#####  Build-InnoScript  ######################################################
function Build-InnoScript()
{
    [CmdletBinding()]
    param (
        [Parameter()]
        [ValidateSet('Client', 'Server')]
        [string]$Build

       ,[Parameter()]
        [ValidateSet('AllPlugins', 'Base')]
        [string]$Configuration

       ,[Parameter()]
        [string]$VersionNumber
    )

    Write-Verbose "Build-InnoScript Build = $Build, Configuration = $Configuration, VersionNumber = $VersionNumber"

    Write-Verbose "Getting the list of support executable files"
    $executableArray = dir $env:WORKSPACE\STBBuild\Binaries\Plugin\* -Include *.exe, *.dll -Exclude Plugin.* -Name
    
    [string[]] $libraryArray = @()

    # Build the Inno script strings for the list of support libraries.
    Write-Verbose "Build the list of support libraries"
    ForEach ($item in $executableArray)
    {
        $libraryArray += 'Source: "..\..\Binaries\Plugin\' + $item + '"; DestDir: "{app}\Plugin"; Flags: ignoreversion'
    }

    # Now get the list of plugins to be included.
    Write-Verbose "Build the list of plugins to be included"
    ForEach ($line in (Get-Content -Path $env:WORKSPACE\STBBuild\Configuration\$Configuration\PluginList.txt | Where {$_ -notmatch '^//.*' -and $_ -notmatch '^\*.*'}))
    {
        $line = $line | %{$_.Split(',')[0]}
        $libraryArray += 'Source: "..\..\Binaries\Plugin\Plugin.' + $line + '.dll"; DestDir: "{app}\Plugin"; Flags: ignoreversion'
    }

    # Save the Inno script with the additions.
    $innoScriptName = "{0}SetupMain-{1}.iss" -f $Build, $Configuration
    $outputBaseFilename = "STB{0}-{1}-{2}_{3}" -f $Build, $Configuration, $VersionNumber, (Get-Date -Format yyyyMMdd)
    Write-Verbose "Saving the modified Inno script: $env:WORKSPACE\STBBuild\Setup\InstallerScripts\$innoScriptName"
    $libContent = $libraryArray -join [Environment]::NewLine

    (Get-Content ("$env:WORKSPACE\STBBuild\Setup\InstallerScripts\" + $Build + "SetupMain.iss")).Replace(";INSERTPLUGINSHERE", $libContent).Replace(";INSERTOUTPUTDIR", "OutputDir=$env:WORKSPACE\STBBuild\Installers").Replace(";INSERTOUTPUTBASEFILENAME", "OutputBaseFilename=$outputBaseFilename") | Set-Content "$env:WORKSPACE\STBBuild\Setup\InstallerScripts\$innoScriptName"
}

#####  Copy-STBFiles  #########################################################
function Copy-STBFiles()
{
    # STB Control Panel
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\STB Control Panel" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\ControlPanel\hpstbcp.exe "$env:WORKSPACE\STBBuild\Binaries\STB Control Panel\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\ControlPanel\hpstbcp.exe.config "$env:WORKSPACE\STBBuild\Binaries\STB Control Panel\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\ControlPanel\*.dll "$env:WORKSPACE\STBBuild\Binaries\STB Control Panel" -Force -Recurse -Verbose

    # STB Admin Console
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\STB User Console" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\STBUserConsole\SolutionTestBench.exe "$env:WORKSPACE\STBBuild\Binaries\STB User Console\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\STBUserConsole\SolutionTestBench.exe.config "$env:WORKSPACE\STBBuild\Binaries\STB User Console\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\STBUserConsole\*.dll "$env:WORKSPACE\STBBuild\Binaries\STB User Console\" -Force -Recurse -Verbose

    # Plugins
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\Plugin" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\Plugin\*.dll $env:WORKSPACE\STBBuild\Binaries\Plugin\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\Plugin\*.exe $env:WORKSPACE\STBBuild\Binaries\Plugin\ -Force -Recurse -Verbose

    # Solution Tester Console
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\Solution Tester Console" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\SolutionTesterConsole\*.exe "$env:WORKSPACE\STBBuild\Binaries\Solution Tester Console\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\SolutionTesterConsole\*.dll "$env:WORKSPACE\STBBuild\Binaries\Solution Tester Console\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\SolutionTesterConsole\SolutionTesterConsole.exe.config "$env:WORKSPACE\STBBuild\Binaries\Solution Tester Console\" -Force -Recurse -Verbose

    # SDK Components
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Examples\References" -Verbose
    Copy $env:WORKSPACE\PluginSdk\Examples\* STBBuild\Examples\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\ScalableTestFramework\Binaries\STF.Common\STF.Development.dll $env:WORKSPACE\STBBuild\Examples\References\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\ScalableTestFramework\Binaries\STF.Common\STF.Framework.dll $env:WORKSPACE\STBBuild\Examples\References\ -Force -Recurse -Verbose

    # STB Data Service
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\STB Data Service" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\DataService\hpstbds.exe "$env:WORKSPACE\STBBuild\Binaries\STB Data Service\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\DataService\hpstbds.exe.config "$env:WORKSPACE\STBBuild\Binaries\STB Data Service\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\DataService\*.dll "$env:WORKSPACE\STBBuild\Binaries\STB Data Service" -Force -Recurse -Verbose

    # Database Installer
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Binaries\Database Installer" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\bin\Debug\StbInstaller.exe "$env:WORKSPACE\STBBuild\Binaries\Database Installer\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\bin\Debug\StbInstaller.exe.config "$env:WORKSPACE\STBBuild\Binaries\Database Installer\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\SqlExpressInstallation\SQLEXPR_CONFIGURATION.ini "$env:WORKSPACE\STBBuild\Binaries\Database Installer\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\bin\Debug\*.dll "$env:WORKSPACE\STBBuild\Binaries\Database Installer\" -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\bin\Debug\*.pdb "$env:WORKSPACE\STBBuild\Binaries\Database Installer\" -Force -Recurse -Verbose

    # Misc Tools
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\CreateInstaller" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\VirtualResource\Distribution\UacChecker\UacChecker.exe $env:WORKSPACE\STBBuild\Binaries\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\CreateInstaller\bin\Debug\CreateInstaller.exe $env:WORKSPACE\STBBuild\CreateInstaller\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\CreateInstaller\bin\Debug\*.dll $env:WORKSPACE\STBBuild\CreateInstaller\ -Force -Recurse -Verbose

    # Package Files and Documentation
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Configuration" -Verbose
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Documentation\PluginSdk" -Verbose
    Copy-Item $env:WORKSPACE\SolutionTestBench\PartnerPackages\* $env:WORKSPACE\STBBuild\Configuration\ -Force -Recurse -Verbose
    #Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\PluginList\PluginList.txt $env:WORKSPACE\STBBuild\Configuration\AllPlugins\ -Force -Recurse -Verbose
    #Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\PluginList\Readme.txt $env:WORKSPACE\STBBuild\Configuration\Base\ -Force -Recurse -Verbose
    #Copy $env:WORKSPACE\SolutionTestBench\ServerInstaller\PluginList\BasePluginList.txt $env:WORKSPACE\STBBuild\Configuration\Base\PluginList.txt -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Documentation\Readme.html $env:WORKSPACE\STBBuild\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Documentation\*.pdf $env:WORKSPACE\STBBuild\Documentation\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Documentation\*.zip $env:WORKSPACE\STBBuild\Documentation\PluginSdk\ -Force -Recurse -Verbose
    Copy "$env:WORKSPACE\SolutionTestBench\Documentation\*Open Source*.pdf" $env:WORKSPACE\STBBuild\Documentation\PluginSdk\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Documentation\*.chm $env:WORKSPACE\STBBuild\Documentation\PluginSdk\ -Force -Recurse -Verbose
    Move "$env:WORKSPACE\STBBuild\Documentation\*Developers Guide.pdf" $env:WORKSPACE\STBBuild\Documentation\PluginSdk\ -Force -Verbose
    Move "$env:WORKSPACE\STBBuild\Documentation\*Open Source*.pdf" $env:WORKSPACE\STBBuild\Documentation\PluginSdk\ -Force -Verbose

    # Inno Installer files
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Setup\InnoInstallers" -Verbose
    New-Item -ItemType Directory -Force -Path "$env:WORKSPACE\STBBuild\Setup\InstallerScripts" -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Inno\*.exe $env:WORKSPACE\STBBuild\Setup\InnoInstallers\ -Force -Recurse -Verbose
    Copy $env:WORKSPACE\SolutionTestBench\Setup\InstallerScripts\*.iss $env:WORKSPACE\STBBuild\Setup\InstallerScripts\ -Force -Recurse -Verbose
	
    # Create Build Destinations
    New-Item -ItemType Directory -Force -Path $env:WORKSPACE\STBBuild\Installers -Verbose
    New-Item -ItemType Directory -Force -Path $env:WORKSPACE\STBBuild\Packages -Verbose

    # Preserve Build Version Number
    Copy $env:WORKSPACE\ScalableTestFramework\Source\CommonAssemblyInfo.cs $env:WORKSPACE\STBBuild\Configuration\ -Force -Verbose
}

#####  Extract-BuildVersionNumber  ############################################
function Extract-BuildVersionNumber()
{
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

    # Return the version string.
    $versionString
}

#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

# 1. Clean the staging folder.

    # Create the folder if it does not already exist.
    if(!(Test-Path $env:WORKSPACE\STBBuild))
    {
        New-Item -ItemType "directory" -Force -Path $env:WORKSPACE\STBBuild -Verbose
    }

    Write-Host "Removing all files & folders from the staging folder"
    Remove-Item $env:WORKSPACE\STBBuild\* -Recurse -Force

# 2. Extract the build version number.

    Write-Host "`r`nExtracting the build version number"
    $versionString = Extract-BuildVersionNumber

# 3. Copy the STB files to the staging folder.

    Write-Host "`r`nCopying STB files to the staging folder"
    Copy-STBFiles

# 4. Create the client all-plugins Inno script.

    Write-Host "`r`nCreating the client all-plugins Inno script"
    Build-InnoScript -Build Client -Configuration AllPlugins -VersionNumber $versionString -Verbose

# 5. Create the server all-plugins Inno script.

    Write-Host "`r`nCreating the server all-plugins Inno script"
    Build-InnoScript -Build Server -Configuration AllPlugins -VersionNumber $versionString -Verbose
