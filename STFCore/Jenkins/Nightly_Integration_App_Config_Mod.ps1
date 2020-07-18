cd 'C:\'
$doc = [string](Get-Content "C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe.config")
$doc = $doc -replace '<!--<UnattendedExecutionConfig>([^\)]+)<\/UnattendedExecutionConfig>-->',
	'<UnattendedExecutionConfig>
        <add key="scenarios" value="Nightly Integration;" />
        <add key="database" value="STFData02" />
        <add key="sessionName" value="Nightly Integration" />
        <add key="dispatcher" value="STFIntegration"/>
        <add key="owner" value="$stfjenkins001"/>
        <add key="reservation" value="" />
       <add key="password" value="car.hat-25" />
       <add key="domain" value="AUTH.HPICORP.NET" />
	   <add key="durationHours" value="2" />
    </UnattendedExecutionConfig>'
	
(Set-Content -Path "C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe.config" -Value $doc)
$asXML = [Xml](Get-Content "C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe.config")
$asXML.save((Resolve-Path "C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe.config"))
echo $asXML