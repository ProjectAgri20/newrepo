<#
.SYNOPSIS
This script will change the STF environment and optionally push new binary files to a set of STF servers.

.DESCRIPTION
This script is designed to be called by a job that is running on the STF Jenkins server. The Jenkins job is further designed to be called from the web server that is displaying the STF Asset Inventory web site.

The web site will present the UI to a user and allow them to select one or more servers that have been designed to work with the STF system. When the user clicks the 'Update' button on the web site, the web server will send a command to the STF Jenkins server which will in turn, call this PowerShell script to perform the requested action.

.PARAMETER STFServerList
This parameter contains a comma-delimited list of servers that will be processed by this script. The servers will be processed in the order they are listed in the parameter.

.PARAMETER STFEnvironment
This parameter will specify the name of the database server where the STF components will read their environment settings.

To point the components to the production environment, use a value of "STFSystem01". To point the components to the beta environment, use a value of "STFSystem02".

.PARAMETER STFUpdate
This parameter will only accept a value of "True" or "False". If it is set to "False" the the components installed on the servers will be switched to the environment specified by the STFEnvironment parameter.

If this parameter is set to a value of "True" then the components will be updated to the latest version of either production or beta code before they are switched to the requested environment.

.INPUTS
None

.OUTPUTS
None

.EXAMPLE
  .\STF_AI_InstallServers.ps1 -STFServerList "Dispatcher38,FileServer01" -STFEnvironment STFSystem01 -STFUpdate True

  The above example will update the components on the two servers named "Dispatcher38" and "FileServer01" to the latest version of production code and ensure that the components installed on both servers are switched to use the production environment.

.LINK
https://stfdistribution.etl.boi.rd.hpicorp.net:8443/

#>

param (
    [Parameter(Mandatory = $true)]
    [string]$STFServerList

   ,[Parameter(Mandatory = $true)]
    [ValidateSet("Production", "Beta", "Development")] #Using environment name rather than server names
    [string]$STFEnvironment

   ,[Parameter(Mandatory = $true)]
    [ValidateScript({$_ -eq [bool]::FalseString -or $_ -eq [bool]::TrueString})]
    [string]$STFUpdate
)

#####  Get-ServerTypesTable  ##################################################
function Get-ServerTypesArray ()
{
    [CmdletBinding()]
    param (
        [string]$ServerName
    )

    Write-Verbose "Get-ServerTypesArray $ServerName"
    #using hub's environment
    $dbConnection = New-Object -TypeName System.Data.SqlClient.SqlConnection
    $dbConnection.ConnectionString = "Data Source=STFGlobal01.etl.psr.rd.hpicorp.net;Persist Security Info=True;User ID=asset_admin;Password=asset_admin"
    Write-Verbose "STFGlobal01 connection string: $dbConnection.ConnectionString"

    $dbCommand = $dbConnection.CreateCommand();
    $dbCommand.CommandText = "SELECT fst.Name FROM FrameworkServer fs INNER JOIN FrameworkServerTypeAssoc fsta ON fs.ServerId = fsta.ServerId INNER JOIN FrameworkServerType fst ON fsta.TypeId = fst.TypeId WHERE fs.HostName = '" + $ServerName + "' AND fst.Name IN ('Citrix','Dispatcher','DSS','ePrint','FileServer','HPAC','HPCR','Print','VPrint')"
    Write-Verbose "SQL query: $dbCommand.CommandText"

    $dbAdapter = New-Object -TypeName System.Data.SqlClient.SqlDataAdapter $dbCommand
    $dbTypeTable = New-Object -TypeName System.Data.DataTable

    $dbAdapter.Fill($dbTypeTable) | Out-Null

    $serverTypes = @($dbTypeTable | select -ExpandProperty Name)
    Write-Verbose "Server types result: `n$serverTypes"

    return @($serverTypes)
}

#####  Install-Citrix  ########################################################
function Install-Citrix ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$StartArgs
    )

    Write-Verbose "Install-Citrix $ComputerName $StartArgs"

    Install-GenericService -ComputerName $ComputerName -ServiceName "CitrixQueueMonitorService" -StartArgs $StartArgs
    Start-Sleep -Seconds 2 #for some weird reason, it was not able to install without a sleep
    Install-GenericService -ComputerName $ComputerName -ServiceName "PrintMonitorService" -StartArgs $StartArgs

    Write-Host "'$ComputerName': Installing remaining citrix components"
    Install-Component -ComputerName $ComputerName -ServerEnvironment $StartArgs -ComponentName "OfficeWorkerConsole"
    Install-Component -ComputerName $ComputerName -ServerEnvironment $StartArgs -ComponentName "ClientFactoryService"
    Install-Component -ComputerName $ComputerName -ServerEnvironment $StartArgs -ComponentName "Plugin"
    Install-Component -ComputerName $ComputerName -ServerEnvironment $StartArgs -ComponentName "PerfMonCollector"

    Write-Host "'$ComputerName': Open the Lock service url for all clients to access"
    Invoke-Command -ComputerName $ComputerName -ScriptBlock { netsh http add urlacl url=http://+:10632/LockClient/ user=\Everyone }
}

#####  Install-Component  #####################################################
function Install-Component ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServerEnvironment
       ,[string]$ComponentName
    )

    Write-Verbose "Install-Component $ComputerName $ServerEnvironment $ComponentName"

    $currentDir = ""

    Write-Host "'$ComputerName': Validate source directories"
    switch ($ServerEnvironment)
    {
        "STFSystem01" { $currentDir = "C:\STF\latest_production" } #using Hub's location
        "STFSystem02" { $currentDir = "C:\Users\wookie.ETLBLR\.jenkins\workspace\STF_Trunk" } #using Hub's location
        "STFSystem03" {  $currentDir = "C:\STF\latest_beta" } #using Hub's location
        DEFAULT { throw "Unknown environment string: $ServerEnvironment" }
    }
    $virtualResourceDir = Join-Path -Path $currentDir -ChildPath "VirtualResource"
    if (!(Test-Path $virtualResourceDir)) { throw "'$ComputerName': Virtual Resource directory '$virtualResourceDir' does not exist" }

    $distributionDir = Join-Path -Path $virtualResourceDir -ChildPath "Distribution"
    if (!(Test-Path $distributionDir)) { throw "'$ComputerName': Distribution directory '$distributionDir' does not exist" }

    $componentDir = Join-Path -Path $distributionDir -ChildPath $ComponentName
    if (!(Test-Path $componentDir)) { throw "'$ComputerName': Component directory '$componentDir' does not exist" }

    Write-Host "'$ComputerName': Pushing files for component $ComponentName"
    RoboCopy $componentDir \\$ComputerName\c$\VirtualResource\Distribution\$ComponentName /XD .svn .vs /S /R:5 /W:10 /NP
}

#####  Install-Dispatcher  ####################################################
function Install-Dispatcher ()
{
    [CmdletBinding()]
    param (
        [string]$DispatcherName
       ,[string]$StartArgs
    )

    Write-Verbose "Install-Dispatcher $DispatcherName $StartArgs"

    $service = "DispatcherService"

    Write-Host "'$DispatcherName': Kill interferring processes"
    Stop-InterferringProcesses -ComputerName $DispatcherName

    Write-Host "'$DispatcherName': Stop the DispatcherService"
    Stop-RemoteService -ComputerName $DispatcherName -ServiceName $service

    Write-Host "'$DispatcherName': Uninstall the DispatcherService"
    Uninstall-RemoteService -ComputerName $DispatcherName -ServiceName $service

    Write-Host "'$DispatcherName': Installing dispatcher component files"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "ClientFactoryService"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName $service
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "OfficeWorkerConsole"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "PerfMonCollector"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "Plugin"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "PrintMonitorService"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "SessionProxy"
    Install-Component -ComputerName $DispatcherName -ServerEnvironment $StartArgs -ComponentName "STFAdminConsole"

    Write-Host "'$DispatcherName': Validate deployment directory" #this doesn't make sense, we should be validating remote machine location not local!!!
    $currentDir = ""
    switch ($StartArgs)
    {
       "STFSystem01" { $currentDir = "C:\STF\latest_production" } 
        "STFSystem02" { $currentDir = "C:\Users\wookie.ETLBLR\.jenkins\workspace\STF_Trunk" }
        "STFSystem03" {  $currentDir = "C:\STF\latest_beta" }
        DEFAULT { throw "Unknown environment string: $ServerEnvironment" }
    }
    $virtualResourceDir = Join-Path -Path $currentDir -ChildPath "VirtualResource"
    #this is local path testing, it should be remote
    if (!(Test-Path $virtualResourceDir)) { throw "'$DispatcherName': Virtual Resource directory '$virtualResourceDir' does not exist" }

    $deploymentDir = Join-Path -Path $virtualResourceDir -ChildPath "Deployment"
    if (!(Test-Path $deploymentDir)) { throw "'$DispatcherName': Deployment directory '$deploymentDir' does not exist" }

    Write-Host "'$DispatcherName': Push deployment files"
    RoboCopy $deploymentDir \\$DispatcherName\c$\VirtualResource\Deployment /XD .svn .vs /S /R:5 /W:10 /NP

    Write-Host "'$DispatcherName': Install & start the DispatcherService with start args: $StartArgs"
    Start-RemoteService -ComputerName $DispatcherName -ServiceName $service -StartArgs $StartArgs
}

#####  Install-GenericService  ################################################
function Install-GenericService ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServiceName
       ,[string]$StartArgs
       ,[string]$AddlStartArgs
    )

    Write-Verbose "Install-GenericService $ComputerName $ServiceName $StartArgs"

    Write-Host "'$ComputerName': Stopping $ServiceName"
    Stop-RemoteService -ComputerName $ComputerName -ServiceName $ServiceName
    Start-Sleep -Seconds 2 #without the sleep, it would try to perform the operation so fast that the OS is not ready
    Write-Host "'$ComputerName': Uninstalling $ServiceName"
    Uninstall-RemoteService -ComputerName $ComputerName -ServiceName $ServiceName
    Start-Sleep -Seconds 2
    Write-Host "'$ComputerName': Installing $ServiceName component files"
    Install-Component -ComputerName $ComputerName -ServerEnvironment $StartArgs -ComponentName $ServiceName
    Start-Sleep -Seconds 2
    $delimitedStartArgs = $StartArgs + $AddlStartArgs
    $startArgsArray = $delimitedStartArgs.Split(",")
    Write-Host "'$ComputerName': Starting $ServiceName service with start args: $startArgsArray"
    Start-RemoteService -ComputerName $ComputerName -ServiceName $ServiceName -StartArgs $startArgsArray
    Start-Sleep -Seconds 2
}

#####  Install-PrintServer  ###################################################
function Install-PrintServer ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$StartArgs
    )

    Write-Verbose "Install-PrintServer $ComputerName $StartArgs"

    Install-GenericService -ComputerName $ComputerName -ServiceName "PrintMonitorService" -StartArgs $StartArgs

    if (($server -notlike "*SafeCom*") -and ($server -notlike "*HPAC*") -and ($server -notlike "*HPCR*") -and ($server -notlike "*EquiTrac*"))
    {
    #this wouldn't start in HUB
      #  Install-GenericService -ComputerName $ComputerName -ServiceName "PhysicalDeviceJobLogMonitorService" -StartArgs $StartArgs
    }
}

#####  Install-Servers  #######################################################
function Install-Servers ()
{
    [CmdletBinding()]
    param (
        [string[]]$Servers
       ,[string]$ServerEnvironment
    )

    Write-Verbose "Install-Servers $Servers $ServerEnvironment"

    foreach ($server in $Servers)
    {
        Write-Host "'$server': Getting server types"
        $serverTypes = Get-ServerTypesArray($server)

        $fqServerName = "$server.etl.psr.rd.hpicorp.net"

        foreach ($serverType in $serverTypes)
        {
            Write-Host "'$fqServerName': Server type = $serverType"
            switch ($serverType)
            {
                "Citrix" {
                        Write-Host "'$fqServerName': Install Citrix server"
                        Install-Citrix -ComputerName $fqServerName -StartArgs $ServerEnvironment
                    }
                "Dispatcher" {
                        Write-Host "'$fqServerName': Install Dispatcher server"
                        Install-Dispatcher -DispatcherName $fqServerName -StartArgs $ServerEnvironment
                    }
                "DSS" {
                        Write-Host "'$fqServerName': Install DSS server"
                        Install-GenericService `
                            -ComputerName $fqServerName `
                            -ServiceName "DigitalSendJobMonitorService" `
                            -StartArgs $ServerEnvironment `
                            -AddlStartArgs ",/localhost,/hpdss2008,/default"
                    }
                "ePrint" {
                        Write-Host "'$fqServerName': Install ePrint server"
                        Install-GenericService `
                            -ComputerName $fqServerName `
                            -ServiceName "EPrintJobMonitorService" `
                            -StartArgs $ServerEnvironment
                    }
                "FileServer" {
                        Write-Host "'$fqServerName': Install FileServer server"
                        Install-GenericService `
                            -ComputerName $fqServerName `
                            -ServiceName "DigitalSendOutputMonitorService" `
                            -StartArgs $ServerEnvironment
                    }
                "HPAC" { Write-Host "'$fqServerName': Install HPAC server" }
                "HPCR" { Write-Host "'$fqServerName': Install HPCR server" }
                "Print" {
                        Write-Host "'$fqServerName': Install Print server"
                        Install-PrintServer -ComputerName $fqServerName -StartArgs $ServerEnvironment
                    }
                "VPrint" {
                        Write-Host "'$fqServerName': Install VPrint server"
                        Install-VPrintServer -ComputerName $fqServerName
                    }
                    
            }
            Start-Sleep -Seconds 2 #i have citrix servers tagged as print as well, it was reinstalling printmonitor service again and failed without a sleep
        }
    }
}

#####  Install-VPrintServer  ##################################################
#this wouldn't work in HUB, not used
function Install-VPrintServer ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
    )

    Write-Verbose "Install-VPrintServer $ComputerName"

    Write-Host "'$ComputerName': Stopping all virtual print devices"
    Stop-RemoteProcess -ComputerName $ComputerName -ProcessName "VirtualPrintDevice"

    Write-Host "'$ComputerName': Installing VPrint component files"
    Install-Component -ComputerName $ComputerName -ComponentName "VirtualPrintDevice"

    Write-Host "'$ComputerName': Starting virtual print devices"
    Invoke-Command -ComputerName $ComputerName -ScriptBlock { param ( $ps, $delay, $nod, $ls ) C:\VirtualResource\Distribution\VirtualPrintDevice\StartPrinters.cmd $ps $delay $nod $ls } -ArgumentList 100000, 2, 255, STFSystem01
}

#####  Restart-RemoteService  #################################################
function Restart-RemoteService ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServiceName
       ,[string]$StartArgs
    )

    Write-Verbose "'$ComputerName': Restarting service $ServiceName with start args: $StartArgs"

    Stop-RemoteService -ComputerName $ComputerName -ServiceName $ServiceName

    Start-RemoteService -ComputerName $ComputerName -ServiceName $ServiceName -StartArgs $StartArgs
}

#####  Start-RemoteService  ###################################################
function Start-RemoteService ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServiceName
       ,[string[]]$StartArgs
    )

    Write-Verbose "Start-RemoteService $ComputerName $ServiceName $StartArgs"

    for ($i = 0; $i -lt $StartArgs.GetUpperBound(0); $i++)
    {
        if (!([string]::IsNullOrEmpty($StartArgs[$i])) -and ($StartArgs[$i].Substring(0, 1) -ne "/"))
        {
            $StartArgs[$i] = "/" + $StartArgs[$i]
        }
    }

    $service = Get-Service $ServiceName -ComputerName $ComputerName -ErrorAction SilentlyContinue
    if ($service -eq $null)
    {
        Write-Host -ForegroundColor Yellow "'$ComputerName': WARNING - service $ServiceName does not appear to be installed"
        $serviceExePath = "C:\VirtualResource\Distribution\{0}\{0}.exe" -f $ServiceName
        Write-Host "'$ComputerName': Installing $ServiceName service"
        Invoke-Command -ComputerName $ComputerName -ScriptBlock { param ( $sn ) C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe $sn } -ArgumentList $serviceExePath
        Write-Host "'$ComputerName': Changing properties on $ServiceName"
        sc.exe \\$ComputerName config $ServiceName obj= 'ETLBLR\Wookie' password= '1iso*help' #using Hub's domain account
    }

    Write-Host "'$ComputerName': Verifying that service $ServiceName is installed"
    $service = Get-Service $ServiceName -ComputerName $ComputerName -ErrorAction SilentlyContinue
    if ($service -eq $null) { throw "'$ComputerName': Service $ServiceName is not installed" }

    Write-Verbose "'$ComputerName': Starting service $ServiceName with start args: $StartArgs"
    $service.Start($StartArgs)
}

#####  Stop-InterferringProcesses  ############################################
function Stop-InterferringProcesses ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
    )

    Write-Verbose "Stop-InterferringProcesses $ComputerName"

    Stop-RemoteProcess -ComputerName $ComputerName -ProcessName "SessionProxy"
    Stop-RemoteProcess -ComputerName $ComputerName -ProcessName "AdminConsole"
}

#####  Stop-RemoteProcess  ####################################################
function Stop-RemoteProcess ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ProcessName
    )

    Write-Host "'$ComputerName': Getting the list of processes like $ProcessName"
    $processes = Get-WmiObject -Class Win32_Process -ComputerName $ComputerName -Filter "Name LIKE '%$ProcessName%'"

    foreach ($process in $processes)
    {
        Write-Host "'$ComputerName': Trying to stop $process.Name"
        $result = $process.Terminate()
        if ($result.ReturnValue -ne 0) { throw "'$ComputerName': Could not terminate process $process.Name" }
    }
}

#####  Stop-RemoteService  ####################################################
function Stop-RemoteService ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServiceName
    )

    Write-Verbose "Stop-RemoteService $ComputerName $ServiceName"

    $service = Get-Service $ServiceName -ComputerName $ComputerName -ErrorAction SilentlyContinue

    if (($service -ne $null) -and ($service.Status -ne "Stopped"))
    {
        Write-Verbose "'$ComputerName': Trying to stop service $ServiceName"
        $service.Stop()
        $waitTime = New-TimeSpan -Seconds 5
        $service.WaitForStatus("Stopped", $waitTime)
        if ($service.Status -ne "Stopped") { throw "'$ComputerName': Cannot stop $ServiceName" }
    }
}

#####  Uninstall-RemoteService  ###############################################
function Uninstall-RemoteService ()
{
    [CmdletBinding()]
    param (
        [string]$ComputerName
       ,[string]$ServiceName
    )

    Write-Host "'$ComputerName': Uninstalling service $ServiceName"
    $service = Get-WmiObject -Class Win32_Service -ComputerName $ComputerName -Filter "Name LIKE '%$ServiceName%'"
    if ($service -ne $null)
    {
        [int]$result = ($service.Delete()).ReturnValue
        if ($result -ne 0) { throw "'$ComputerName': Could not uninstall service $ServiceName.  Return value: $result" }
    }
    else
    {
        Write-Host -ForegroundColor Yellow "'$ComputerName': WARNING - service $ServiceName does not exist on server"
    }
}

#####  Update-Environment  ####################################################
function Update-Environment ()
{
    [CmdletBinding()]
    param (
        [string[]]$Servers
       ,[string]$ServerEnvironment
    )

    Write-Verbose "Update-Environment $Servers $ServerEnvironment"

    foreach ($server in $Servers)
    {
        Write-Host "'$server': Getting server types"
        $serverTypes = Get-ServerTypesArray($server)

        foreach ($serverType in $serverTypes)
        {
            Write-Host "'$server': Server type = $serverType"
            switch ($serverType)
            {
                "Citrix" {
                        Write-Host "'$server': Update Citrix server environment"
                        Restart-RemoteService -ComputerName $server -ServiceName "CitrixQueueMonitorService" -StartArgs $ServerEnvironment
                        Restart-RemoteService -ComputerName $server -ServiceName "PrintMonitorService" -StartArgs $ServerEnvironment
                    }
                "Dispatcher" {
                        Write-Host "'$server': Update Dispatcher server environment"
                        Stop-InterferringProcesses -ComputerName $server
                        Restart-RemoteService -ComputerName $server -ServiceName "DispatcherService" -StartArgs $ServerEnvironment
                    }
                "DSS" {
                        Write-Host "'$server': Update DSS server environment"
                        Restart-RemoteService -ComputerName $server -ServiceName "DigitalSendJobMonitorService" -StartArgs $ServerEnvironment
                    }
                "ePrint" {
                        Write-Host "'$server': Update ePrint server environment"
                        Restart-RemoteService -ComputerName $server -ServiceName "EPrintJobMonitorService" -StartArgs $ServerEnvironment
                    }
                "FileServer" {
                        Write-Host "'$server': Update FileServer server environment"
                        Restart-RemoteService -ComputerName $server -ServiceName "DigitalSendOutputMonitorService" -StartArgs $ServerEnvironment
                    }
                "HPAC" { Write-Host "'$server': Update HPAC server environment" }
                "HPCR" { Write-Host "'$server': Update HPCR server environment" }
                "Print" {
                        Write-Host "'$server': Update Print server environment"
                        if (($server -notlike "*SafeCom*") -and ($server -notlike "*HPAC*") -and ($server -notlike "*HPCR*") -and ($server -notlike "*EquiTrac*"))
                        {
                            Restart-RemoteService -ComputerName $server -ServiceName "PhysicalDeviceJobLogMonitorService" -StartArgs $ServerEnvironment
                        }
                        Restart-RemoteService -ComputerName $server -ServiceName "PrintMonitorService" -StartArgs $ServerEnvironment
                    }
            }
        }
    }
}

#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

Write-Host "`n***** Running: $PSCommandPath *****`n"

Write-Verbose "Server List  = $STFServerList"
Write-Verbose "Environment  = $STFEnvironment"
Write-Verbose "Update Files = $STFUpdate"

try
{
    Write-Host "Converting list of server names to an array"
    $servers = $STFServerList.Split(",")
    if($STFEnvironment -eq "Production")
    {
        $STFSystem = "Stfsystem01" #using this to assign databases
    }
    elseif($STFEnvironment -eq "Development") #added support for development
    {
        $STFSystem = "Stfsystem02"
    }
    else
    {
        $STFSystem = "Stfsystem03"
    }
    #$servers = $servers | ForEach-Object { $_ = "$_.etl.psr.rd.hpicorp.net"; $_ }

    foreach ($server in $servers)
    {
        Write-Host "'$server': Testing connection to server"
        if (!(Test-Connection -ComputerName $server -Count 1 -Quiet)) { throw "Cannot establish a connection to server $server" }
    }

    if ($STFUpdate -eq [bool]::FalseString) #this should be the equivalent of not using /d option instead does nothing!
    {
        Update-Environment -Servers $servers -ServerEnvironment $STFSystem
    }
    else #this should be the equivalent of using /d option
    {
        Write-Host "Install to servers"
        Install-Servers -Servers $servers -ServerEnvironment $STFSystem
    }
}
catch
{
    Write-Host -ForegroundColor Red "ERROR: $_"
    exit 1
}
finally
{
    Write-Host "`n***** Script $PSCommandPath complete *****`n"
}
