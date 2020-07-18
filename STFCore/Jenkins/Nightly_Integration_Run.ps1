#Updates the repo
cd 'C:\Git\STF'
git pull git@github.azc.ext.hp.com:RDL/STF.git develop

#We build the projects

MSBuild MasterSolution.sln /m /p:Configuration=Debug /p:Platform=STF -t:build -restore

MSBuild .\ScalableTestFramework\Source\STF.Plugin.sln /m /p:Configuration=Debug /p:Platform="STF Plugin" -t:build -restore


#Install dispatcher service
cd C:\VirtualResource\Deployment
.'C:\VirtualResource\Deployment\STF.UpdateDispatcher.ps1' STFIntegration STFData02


#Modify App config for integration
.'C:\Git\STF\STFCore\Jenkins\Nightly_Integration_App_Config_Mod.ps1'

#Start STF

C:\VirtualResource\Distribution\STFAdminConsole\HP.STF.AdminConsole.exe

