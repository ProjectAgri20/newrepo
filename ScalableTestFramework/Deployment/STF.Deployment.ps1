function Update-StfComponent()
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string] $sourceDirectory,

        [Parameter(Mandatory = $true)]
        [string] $sourceServer,

        [Parameter(Mandatory = $true)]
        [string] $sourceSharePath,

        [Parameter(Mandatory = $true)]
        [string] $destinationPath
    )

    $source = "\\$sourceServer\$sourceSharePath\$sourceDirectory"
    $destination= "$destinationPath\$sourceDirectory"

    ROBOCOPY $source $destination /S /R:5 /W:10 /MIR
}

function Update-StfService()
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string] $sourceDirectory,

        [Parameter(Mandatory = $true)]
        [string] $sourceServer,

        [Parameter(Mandatory = $true)]
        [string] $sourceSharePath,

        [Parameter(Mandatory = $true)]
        [string] $destinationPath,
        
        [Parameter()]
        [string] $serviceExeName = "$sourceDirectory.exe"
    )

    $serviceExePath = "$destinationPath\$sourceDirectory\$serviceExeName"

    if (Test-Path $serviceExePath)
    {
        Uninstall-StfService $serviceExePath
    }

    Update-StfComponent $sourceDirectory $sourceServer $sourceSharePath $destinationPath
    Install-StfService $serviceExePath
}

function Install-StfService()
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string] $serviceExe
    )

    Start-Process -NoNewWindow -Wait -FilePath $serviceExe -ArgumentList '-install /username=etl\jawa /password=!QAZ2wsx'
}

function Uninstall-StfService()
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string] $serviceExe
    )

    Start-Process -NoNewWindow -Wait -FilePath $serviceExe -ArgumentList '-uninstall'
}

function Start-ServiceWithArgs()
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string] $serviceName,

        [Parameter(Mandatory = $true, ValueFromRemainingArguments = $true)]
        [string[]] $serviceStartArgs
    )

    $service = Get-Service $serviceName
    $service.Start($serviceStartArgs);
}
