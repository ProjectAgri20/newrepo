<#
.SYNOPSIS
Deploys all necessary STF files to a Citrix server used for hosting virtual resources.

.DESCRIPTION
This script deploys an STF component installation from a remote build directory.  

.PARAMETER sourceServer
Specifies the remote server to download the binaries from.

.PARAMETER databaseServer
Specifies the EnterpriseTest database to connect to for configuration data.

.PARAMETER sourceSharePath
Specifies the share path on the remote server.  Defaults to 'VirtualResource\Distribution'.

.PARAMETER destinationPath
Specifies the location to deploy the files on the local machine.  Defaults to '..\Distribution'.
#>

param
(
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $sourceServer,

    [Parameter(Mandatory = $true, Position = 1)]
    [string] $databaseServer,

    [Parameter()]
    [string] $sourceSharePath = 'VirtualResource\Distribution',

    [Parameter()]
    [string] $destinationPath = '..\Distribution'
)

# Load common script module
. (Join-Path (Split-Path $MyInvocation.MyCommand.Path) STF.Deployment.ps1)

Update-StfService   CitrixQueueMonitorService $sourceServer $sourceSharePath $destinationPath
Update-StfService   PrintMonitorService       $sourceServer $sourceSharePath $destinationPath
Update-StfComponent ClientFactoryService      $sourceServer $sourceSharePath $destinationPath
Update-StfComponent OfficeWorkerConsole       $sourceServer $sourceSharePath $destinationPath
Update-StfComponent PerfMonCollector          $sourceServer $sourceSharePath $destinationPath
Update-StfComponent Plugin                    $sourceServer $sourceSharePath $destinationPath

Start-ServiceWithArgs CitrixQueueMonitorService "/database=$databaseServer"
Start-ServiceWithArgs PrintMonitorService "/database=$databaseServer"
