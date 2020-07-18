<#
.SYNOPSIS
Deploys all necessary STB files to a server used for an STB installation.

.DESCRIPTION
This script deploys an STB server installation from a remote build directory.
Note: To make this script work correctly when installing from your dev box,
you will need to share your 'SolutionTestBench' directory.

.PARAMETER sourceServer
Specifies the remote server to download the binaries from.
Example: MyDevBox.auth.hpicorp.net\SolutionTestBench

.PARAMETER sourceSharePath
Specifies the share path on the remote server.  Defaults to 'VirtualResource\Distribution'.

.PARAMETER destinationPath
Specifies the location to deploy the files on the local machine.  Defaults to '..\Distribution'.
#>

param
(
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $sourceServer,

    [Parameter()]
    [string] $sourceSharePath = 'VirtualResource\Distribution',

    [Parameter()]
    [string] $destinationPath = '..\Distribution'
)

# Load common script module
. (Join-Path (Split-Path $MyInvocation.MyCommand.Path) STF.Deployment.ps1)

Update-StfService   'DataService'           $sourceServer $sourceSharePath $destinationPath -serviceExeName hpstbds.exe
Update-StfComponent 'ControlPanel'          $sourceServer $sourceSharePath $destinationPath
Update-StfComponent Plugin                $sourceServer $sourceSharePath $destinationPath
Update-StfComponent 'SolutionTesterConsole' $sourceServer $sourceSharePath $destinationPath
Update-StfComponent 'STBUserConsole'        $sourceServer $sourceSharePath $destinationPath
Update-StfComponent UacChecker            $sourceServer $sourceSharePath $destinationPath

Start-ServiceWithArgs hpstbds '/database=localhost'
