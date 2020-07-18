<#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#
.SYNOPSIS
This script prepares the STB staging folder and files for creation of the STB
installers.

.DESCRIPTION

#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#>

#####  PackageType  ######################################################
#Receives the package type from Jenkins that was selected by the operator.

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet('Base', 'AllPlugins', 'AutoStore', 'BluePrint', 'Celiveo', 'Equitrac', 'Hpac', 'Hpcr', 'SafeCom', 'SafeQ', 'SafeComUC')]
    [string]$PackageType
)

#####  Build-InnoScript  ######################################################
function Build-InnoScript()
{
    [CmdletBinding()]
    param (
        [Parameter()]
        [ValidateSet('Client', 'Server')]
        [string]$Build

       ,[Parameter()]
        [string]$Configuration

       ,[Parameter()]
        [string]$VersionNumber
    )

    Write-Verbose "Build-InnoScript Build = $Build, Configuration = $Configuration, VersionNumber = $VersionNumber"

    Write-Verbose "Getting the list of support executable files"
    $executableArray = dir C:\STFBuilds\latest_production\STBBuild\Binaries\Plugin\* -Include *.exe, *.dll -Exclude Plugin.* -Name
    
    [string[]] $libraryArray = @()

    # Build the Inno script strings for the list of support libraries.
    Write-Verbose "Build the list of support libraries"
    ForEach ($item in $executableArray)
    {
        $libraryArray += 'Source: "..\..\Binaries\Plugin\' + $item + '"; DestDir: "{app}\Plugin"; Flags: ignoreversion'
    }

    # Now get the list of plugins to be included.
    Write-Verbose "Build the list of plugins to be included"
    ForEach ($line in (Get-Content -Path C:\STFBuilds\latest_production\STBBuild\Configuration\$Configuration\PluginList.txt | Where {$_ -notmatch '^//.*' -and $_ -notmatch '^\*.*'}))
    {
        $line = $line | %{$_.Split(',')[0]}
        $libraryArray += 'Source: "..\..\Binaries\Plugin\Plugin.' + $line + '.dll"; DestDir: "{app}\Plugin"; Flags: ignoreversion'
    }

    # Save the Inno script with the additions.
    $innoScriptName = "{0}SetupMain-{1}.iss" -f $Build, $Configuration
    $outputBaseFilename = "STB{0}-{1}-{2}_{3}" -f $Build, $Configuration, $VersionNumber, (Get-Date -Format yyyyMMdd)
    Write-Verbose "Saving the modified Inno script: C:\STFBuilds\latest_production\STBBuild\Setup\InstallerScripts\$innoScriptName"
    $libContent = $libraryArray -join [Environment]::NewLine

    (Get-Content ("C:\STFBuilds\latest_production\STBBuild\Setup\InstallerScripts\" + $Build + "SetupMain.iss")).Replace(";INSERTPLUGINSHERE", $libContent).Replace(";INSERTOUTPUTDIR", "OutputDir=C:\STFBuilds\latest_production\STBBuild\Installers").Replace(";INSERTOUTPUTBASEFILENAME", "OutputBaseFilename=$outputBaseFilename") | Set-Content "C:\STFBuilds\latest_production\STBBuild\Setup\InstallerScripts\$innoScriptName"
}


#####  Extract-BuildVersionNumber  ############################################
function Extract-BuildVersionNumber()
{
    $versionString = "1.0.0.0"

    Write-Host "Read the CommonAssemblyInfo.cs file and locate the line that contains the assembly version string"
    (Get-Content -Path C:\STFBuilds\latest_production\STBBuild\Configuration\CommonAssemblyInfo.cs) | ForEach-Object {
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

#####  ConvertPackageDocPdf  ############################################
function ConvertPackageDocPdf()
{
    [CmdletBinding()]
    param (
        [Parameter()]
        [string]$Configuration
    )

    $pdfFormat = 17
    $folder_path = 'C:\STFBuilds\latest_production\STBBuild\Configuration\' + $Configuration + '\'
    $word_app = New-Object -ComObject Word.Application

    # This filter will find .doc as well as .docx documents
    Get-ChildItem -Path $folder_path -Recurse -Filter *.doc? | ForEach-Object {

        Write-Host "Converting $($_.FullName)" 
        $document = $word_app.Documents.Open($_.FullName)
        $pdf_filename = "C:\STFBuilds\latest_production\STBBuild\Documentation\$($_.BaseName).pdf"
        $document.SaveAs($pdf_filename, $pdfFormat)
        $document.Close()
    }

    $word_app.Quit()
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($word_app)
    Remove-Variable word_app
}


#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

Write-Host "`r`nBuilding STB Package for '$PackageType'."

# 1. Convert Package Document PDF

    Write-Host "`r`nCreating PDF versions of package documentation."
    ConvertPackageDocPdf -Configuration $PackageType -Verbose

# 2. Extract the build version number.

    Write-Host "`r`nExtracting the build version number"
    $versionString = Extract-BuildVersionNumber

# 3. Create the client Inno script.

    Write-Host "`r`nCreating the client Inno script"
    Build-InnoScript -Build Client -Configuration $PackageType -VersionNumber $versionString -Verbose

# 4. Create the server Inno script.

    Write-Host "`r`nCreating the server Inno script"
    Build-InnoScript -Build Server -Configuration $PackageType -VersionNumber $versionString -Verbose
