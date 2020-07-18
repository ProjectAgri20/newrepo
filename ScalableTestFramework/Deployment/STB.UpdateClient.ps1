<#
.SYNOPSIS
Deploys all necessary STB files to a client used to execute STB tests.

.DESCRIPTION
This script deploys an STB client installation from a remote build directory.  

.PARAMETER sourceServer
Specifies the remote server to download the binaries from.

.PARAMETER sourceSharePath
Specifies the share path on the remote server.  Defaults to 'SolutionTestBench\VirtualResource\Distribution'.

.PARAMETER destinationPath
Specifies the location to deploy the files on the local machine.  Defaults to '..\Distribution'.
#>

param
(
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $sourceServer,

    [Parameter()]
    [string] $sourceSharePath = 'SolutionTestBench\VirtualResource\Distribution',

    [Parameter()]
    [string] $destinationPath = '..\Distribution'
)

# Load common script module
. (Join-Path (Split-Path $MyInvocation.MyCommand.Path) STF.Deployment.ps1)

Update-StfComponent Plugin                $sourceServer $sourceSharePath $destinationPath
Update-StfComponent SolutionTesterConsole $sourceServer $sourceSharePath $destinationPath
Update-StfComponent STBUserConsole        $sourceServer $sourceSharePath $destinationPath

# Open the WCF service urls for all clients to access
netsh http add urlacl url=http://+:12668/DataGateway/ user=\Everyone
netsh http add urlacl url=http://+:12672/SessionClient/ user=\Everyone
netsh http add urlacl url=http://+:10101/ user=\Everyone
