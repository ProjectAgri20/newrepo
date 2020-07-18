


<# Command Line Usage: AdminConsole.exe -dispatcher <DispatcherName> -database <DatabaseName> -scenario <ScenarioName> -owner <Session Owner Username> [-reservation <ReservationKey>] -password <auth pw> -domain <auth domain>;

#>

param (
    [Parameter(Mandatory = $true)]
    [string]$DispatcherName

   ,[Parameter(Mandatory = $true)]
    [ValidateSet("STFSystem03", "STFData03")]
    [string]$STFDatabase

   ,[Parameter(Mandatory = $true)]
    [string]$STFScenario
	
   ,[string]$STFSessionOwner = '$stfjenkins001'
	
	,[Parameter(Mandatory = $true)]
    [string]$STFSessionPassword
	
	,[Parameter(Mandatory = $true)]
    [string]$STFSessionDomain
	
	
)


#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# START OF MAIN SCRIPT                                                        #
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

Write-Host "`n***** Running: $PSCommandPath *****`n"

Write-Verbose "DispatcherName  = $DispatcherName"
Write-Verbose "STFDatabase  = $STFDatabase"
Write-Verbose "STFScenario = $STFScenario"
Write-Verbose "STFSessionOwner = $STFSessionOwner"
$scriptpath = "C:\Program Files (x86)\Jenkins\workspace\STF_Integration_Test\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe"

try
{
#Start-Process -FilePath "C:\Program Files (x86)\Jenkins\workspace\STF_Integration_Test\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe" -ArgumentList $DispatcherName, $STFDatabase, $STFScenario, $STFSessionOwner
#Start-Job -ScriptBlock{C:\Program Files (x86)\Jenkins\workspace\STF_Integration_Test\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe -dispatcher $args[0] -database $args[1] -scenario $args[2] -owner $args[3]} -ArgumentList $DispatcherName, $STFDatabase, $STFScenario, $STFSessionOwner
Invoke-Expression "& '$scriptpath' -dispatcher $DispatcherName -database $STFDatabase -scenario '$STFScenario' -owner '$STFSessionOwner' -password $STFSessionPassword -domain $STFSessionDomain /wait /run /SilentMode" 
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