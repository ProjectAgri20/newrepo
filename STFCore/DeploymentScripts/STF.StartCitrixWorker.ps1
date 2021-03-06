# This script is used on a Citrix server to launch the Citrix worker under
# a XenDesktop session.  The script will reside in the Deployment directory
# and a command script used to launch this script will reside in the 
# global startup folder of the system.  The script checks to make sure the
# XenDesktop user is an STF User (based on User naming conventions) and
# then launches the script.
function Test-RegistryValue 
{
param( 
[parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]$Path,
[parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]$Value)
try
{
Get-ItemProperty -Path $Path | Select-Object -ExpandProperty $Value -ErrorAction Stop | Out-Null
return $true
}
catch
{
return $false
}
}

if($args.Length)
{
$startupKey = 'Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run'
if( -Not(Test-RegistryValue -Path $startupKey -Value 'STFCitrixWorker'))
{
    New-ItemProperty $startupKey -Name "STFCitrixWorker" -PropertyType String
    Set-ItemProperty $startupKey STFCitrixWorker -Value "powershell C:\virtualresource\Deployment\STF.StartCitrixWorker.PS1"
}
}
$a = get-content env:Username
if ($a -match "[Uu]{1}[0-9]+")
{
	cd C:\VirtualResource\Distribution\OfficeWorkerConsole
    .\OfficeWorkerConsole.exe
    exit
}


