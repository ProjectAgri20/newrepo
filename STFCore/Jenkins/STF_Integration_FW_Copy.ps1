$scriptDir=(Get-Item -Path ".\").FullName
$bdlDest="\\rdlfs.etl.boi.rd.hpicorp.net\Public\STFIntegrationFW\Firmware"
$stagingbdl =  "\\rdlfs.etl.boi.rd.hpicorp.net\Public\STFIntegrationFW"
$bldNC="LOGOOEM"
$bdlExt="signed.bdl"
#if ((read-host "Clean Destination $bdlDest Y/N") -like "*y*"){Remove-Item $bdlDest\*}
$assetInv = Get-Content "\\rdlfs.etl.boi.rd.hpicorp.net\Public\STFIntegrationFW\asset-versions.htm"
$pattern = @("Cypress", "Denali", "Mercury", "Atom", "Blackbird", "Cordoba", "Bluefin", "Va", "Ruby", "Diamond", "Bugatti")
$assetInv | Select-String '"(file://jedi[^"]+)"' -AllMatches | Select-String -Pattern $pattern -SimpleMatch |
    Foreach-Object {
        $bdlPath = ($_ -replace 'href="file:','' -replace '/','\' -replace ' ','' -split '"')[0]
        Get-ChildItem -Exclude *$bldNC* -Name *.$bdlExt -Path $bdlPath |
            Foreach-Object {
                Write-Host Copying $_ from $bdlPath to $stagingbdl...
                # Write-Progress -Activity "Copying $_" -Status "$bdlPath -> $bdlDest" -PercentComplete ([int]($_.Length))
                Copy-Item "$($bdlPath)/$($_)" $stagingbdl
                Write-Host Copying of $_ complete.
            }
    }

Get-ChildItem $stagingbdl *.bdl | %{Rename-Item $_.FullName ($_.Name.Split("_")[0]+"." + $bdlExt)} 
Get-ChildItem $stagingbdl *.bdl | Move-Item -Destination $bdlDest -Force

cd $scriptDir