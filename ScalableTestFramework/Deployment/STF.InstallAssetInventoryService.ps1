<#
.SYNOPSIS
Installs the Asset Inventory Service on this machine.

.DESCRIPTION
This script downloads and installs an STF service from a remote build directory.  

.PARAMETER sourceServer
Specifies the remote server to download the binaries from.

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

Update-StfService AssetInventoryService $sourceServer $sourceSharePath $destinationPath
Start-Service AssetInventoryService
