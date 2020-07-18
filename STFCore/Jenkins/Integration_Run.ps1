<#
.SYNOPSIS
This script will run the night or weekend STF integration test scenario.

.PARAMETER IntegrationType
This parameter will specify which integration scenario to use. The options are 'Nightly' or 'Weekend'.
#>

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("Nightly", "Weekend")]
    [string]$IntegrationType
    ,
    [Parameter(Mandatory = $true)]
    [ValidateRange(1, 72)]
    [int]$DurationHours
)

# Global variables
$stfDistribution = "STFDistribution.etl.boi.rd.hpicorp.net"
$dispatcherServiceLocation = "C:\VirtualResource\Distribution\DispatcherService\DispatcherService.exe"

#####  Install-STFComponent  ##################################################
function Install-STFComponent()
{
    [CmdletBinding()]
    param (
        [string]$ComponentName
    )

    Write-Verbose "Trying to install STF component: $ComponentName"

    $distributionLocation = "\\$stfDistribution\workspace\Git Develop Build\VirtualResource\Distribution"
    $componentLocation = Join-Path -Path $distributionLocation -ChildPath $ComponentName

    Write-Verbose "Copying component files from $componentLocation"
    RoboCopy $componentLocation "C:\VirtualResource\Distribution\$ComponentName" /XD .svn .vs /S /R:5 /W:10 /NP
}

#####  Modify-AdminConsoleConfig  #############################################
function Modify-AdminConsoleConfig()
{
    [CmdletBinding()]
    param (
        [string]$TestType
        ,
        [int]$Duration
    )

    # Set the location of the Admin Console config file.
    $configFile = "C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe.config"
    $searchString = '<!--<UnattendedExecutionConfig>([^\)]+)<\/UnattendedExecutionConfig>-->'
    $replaceString = '<UnattendedExecutionConfig>
        <add key="scenarios" value="' + $TestType + ' Integration;" />
        <add key="database" value="STFData02" />
        <add key="sessionName" value="' + $TestType + ' Integration" />
        <add key="dispatcher" value="STFIntegration"/>
        <add key="owner" value="$stfjenkins001"/>
        <add key="reservation" value="" />
        <add key="password" value="dog.boy-19" />
        <add key="domain" value="AUTH.HPICORP.NET" />
	    <add key="durationHours" value="' + $Duration + '" />
    </UnattendedExecutionConfig>'

    Write-Verbose "Changing the STF Admin Console config file"
    $config = [string](Get-Content -Path $configFile)
    $config = $config -replace $searchString, $replaceString

    Set-Content -Path $configFile -Value $config

    $asXML = [Xml](Get-Content $configFile)
    $asXML.Save((Resolve-Path $configFile))

    # Uncomment the following line to view the config xml data on the console.
    #Get-Content $configFile
}

#####  Stop-InterferringProcess  ##############################################
function Stop-InterferringProcess()
{
    [CmdletBinding()]
    param (
        [string]$ProcessName
    )

    Write-Verbose "Trying to stop all interferring processes: $ProcessName"
    $processes = Get-WmiObject -Class Win32_Process -Filter "Name LIKE '%$ProcessName%'"
    foreach ($process in $processes)
    {
        Write-Host "Trying to stop $process.Name"
        $result = $process.Terminate()
        if ($result.ReturnValue -ne 0) { throw "Could not terminate process $process.Name" }
    }
}

#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

Write-Host "`n***** Running: $PSCommandPath *****`n"

Write-Verbose "IntegrationType = $IntegrationType"
Write-Verbose "DurationHours   = $DurationHours"

try
{
    # 1. Make sure we can contact the VM that contains the binary files.
    Write-Host "Testing the connection to STFDistribution."
    if (!(Test-Connection -ComputerName $stfDistribution -Count 1 -Quiet))
    {
        throw "Cannot establish a connection to STFDistribution"
    }

    # 2. Stop any STF processes that were left over from an existing session.
    Write-Host "Stopping any process instances of SessionProxy or AdminConsole."
    Stop-InterferringProcess "SessionProxy"
    Stop-InterferringProcess "AdminConsole"

    # 3. Stop the DispatcherService.
    Write-Host "Stopping the STF Dispatcher Service."
    Stop-Service -Name DispatcherService -WarningAction SilentlyContinue

    # 4. Uninstall the DispatcherService.
    Write-Host "Removing the STF Dispatcher Service"
    Start-Process -NoNewWindow -Wait -FilePath $dispatcherServiceLocation -ArgumentList '-uninstall'

    # 5. Install the necessary binary files.
    Install-STFComponent -ComponentName "ClientFactoryService"
    Install-STFComponent -ComponentName "DispatcherService"
    Install-STFComponent -ComponentName "OfficeWorkerConsole"
    Install-STFComponent -ComponentName "PerfMonCollector"
    Install-STFComponent -ComponentName "Plugin"
    Install-STFComponent -ComponentName "PrintMonitorService"
    Install-STFComponent -ComponentName "SessionProxy"
    Install-STFComponent -ComponentName "STFAdminConsole"

    # 6. Install and start the DispatcherService.
    Write-Host "Installing and starting the STF Dispatcher Service."
    Start-Process -NoNewWindow -Wait -FilePath $dispatcherServiceLocation -ArgumentList '-install /username=etl\jawa /password=!QAZ2wsx'
    Start-Service -Name DispatcherService -WarningAction SilentlyContinue

    # 7. Modify the STF Admin Console config file.
    Modify-AdminConsoleConfig $IntegrationType $DurationHours

    # 8. Start the integration tests.
    C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe
}
catch
{
    Write-Host -ForegroundColor Red "Error: $_"
    exit 1
}
finally
{
    Write-Host "`n***** Script $PSCommandPath complete *****`n"
}
