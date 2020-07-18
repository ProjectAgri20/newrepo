:: Builds a special ILMerged build of STF.Framework.dll, then copies all files necessary for STB
:: into a staging location in preparation for running the STB Insaller creation utility
:: To ILMerge the Telerik .dlls with STF.Framework.dll, add "/m" as a second parameter.

@ECHO OFF
IF "%1" == "" goto USAGE
IF "%2" == "" goto MAIN

:: ILMerge Framework.dll for PluginSdk
::--------------------------------------------------------------------------------------------------
IF NOT EXIST "..\ScalableTestFramework\Binaries\PluginSdk\" MKDIR ..\ScalableTestFramework\Binaries\PluginSdk\

ILMerge.exe /ndebug /wildcards /internalize /targetplatform:"v4, C:\Windows\Microsoft.NET\Framework\v4.0.30319" ^
                     /out:%~dp0..\ScalableTestFramework\Binaries\PluginSdk\STF.Framework.dll ^
                     %~dp0..\ScalableTestFramework\Binaries\STF.Common\STF.Framework.dll ^
                     %~dp0..\ScalableTestFramework\Binaries\STF.Common\Telerik*.dll

:: Pause so user can check for errors in the previous operation
set /p=Press ENTER to continue...

::--------------------------------------------------------------------------------------------------

del ..\ScalableTestFramework\Binaries\PluginSdk\Telerik*.dll
XCOPY /S /Y ..\ScalableTestFramework\Binaries\PluginSdk\STF.Framework.dll ..\ScalableTestFramework\Binaries\STF.Common\

:MAIN

:: STB Control Panel
XCOPY /S /Y VirtualResource\Distribution\ControlPanel\hpstbcp.exe "%1\Binaries\STB Control Panel\"
XCOPY /S /Y VirtualResource\Distribution\ControlPanel\hpstbcp.exe.config "%1\Binaries\STB Control Panel\"
XCOPY /S /Y VirtualResource\Distribution\ControlPanel\*.dll "%1\Binaries\STB Control Panel"

:: STB Admin Console
XCOPY /S /Y VirtualResource\Distribution\STBUserConsole\SolutionTestBench.exe "%1\Binaries\STB User Console\"
XCOPY /S /Y VirtualResource\Distribution\STBUserConsole\SolutionTestBench.exe.config "%1\Binaries\STB User Console\"
XCOPY /S /Y VirtualResource\Distribution\STBUserConsole\*.dll "%1\Binaries\STB User Console\"

:: Plugins
XCOPY /S /Y VirtualResource\Distribution\Plugin\*.dll %1\Binaries\Plugin\
XCOPY /S /Y VirtualResource\Distribution\Plugin\*.exe %1\Binaries\Plugin\

:: Solution Tester Console
XCOPY /S /Y VirtualResource\Distribution\SolutionTesterConsole\*.exe "%1\Binaries\Solution Tester Console\"
XCOPY /S /Y VirtualResource\Distribution\SolutionTesterConsole\*.dll "%1\Binaries\Solution Tester Console\"
XCOPY /S /Y VirtualResource\Distribution\SolutionTesterConsole\SolutionTesterConsole.exe.config "%1\Binaries\Solution Tester Console\"

:: SDK Components
XCOPY /S /Y /I ..\PluginSdk\Examples\* %1\Examples\
XCOPY /S /Y ..\ScalableTestFramework\Binaries\STF.Common\STF.Development.dll %1\Examples\References\
XCOPY /S /Y ..\ScalableTestFramework\Binaries\STF.Common\STF.Framework.dll %1\Examples\References\

:: STB Data Service
XCOPY /S /Y VirtualResource\Distribution\DataService\hpstbds.exe "%1\Binaries\STB Data Service\"
XCOPY /S /Y VirtualResource\Distribution\DataService\hpstbds.exe.config "%1\Binaries\STB Data Service\"
XCOPY /S /Y VirtualResource\Distribution\DataService\*.dll "%1\Binaries\STB Data Service"

:: Database Installer
XCOPY /S /Y ServerInstaller\bin\Debug\StbInstaller.exe "%1\Binaries\Database Installer\"
XCOPY /S /Y ServerInstaller\bin\Debug\StbInstaller.exe.config "%1\Binaries\Database Installer\"
XCOPY /S /Y ServerInstaller\SqlExpressInstallation\SQLEXPR_CONFIGURATION.ini "%1\Binaries\Database Installer\"
XCOPY /S /Y ServerInstaller\bin\Debug\*.dll "%1\Binaries\Database Installer\"
XCOPY /S /Y ServerInstaller\bin\Debug\*.pdb "%1\Binaries\Database Installer\"

:: Misc Tools
XCOPY /S /Y VirtualResource\Distribution\UacChecker\UacChecker.exe %1\Binaries\
XCOPY /S /Y CreateInstaller\bin\Debug\CreateInstaller.exe %1\CreateInstaller\
XCopy /S /Y CreateInstaller\bin\Debug\*.dll %1\CreateInstaller\
MKLINK %1\CreateInstaller.exe %1\CreateInstaller\CreateInstaller.exe

:: Supporting Files and Documentation
XCOPY /S /Y ServerInstaller\PluginList\Readme.txt %1\Configuration\AllPlugins\
XCOPY /S /Y ServerInstaller\PluginList\PluginList.txt %1\Configuration\AllPlugins\
XCOPY /S /Y ServerInstaller\PluginList\Readme.txt %1\Configuration\Base\
COPY /Y ServerInstaller\PluginList\BasePluginList.txt %1\Configuration\Base\PluginList.txt
XCOPY /S /Y Documentation\Readme.html %1\
XCOPY /S /Y Documentation\*.pdf %1\Documentation\
XCOPY /S /Y Documentation\*.zip %1\Documentation\PluginSdk\
XCOPY /S /Y "Documentation\*Open Source*.pdf" %1\Documentation\PluginSdk\
XCOPY /S /Y Documentation\*.chm %1\Documentation\PluginSdk\
MOVE /Y "%1\Documentation\*Developers Guide.pdf" %1\Documentation\PluginSdk\
MOVE /Y "%1\Documentation\*Open Source*.pdf" %1\Documentation\PluginSdk\

:: Inno Installer files
XCOPY /S /Y Inno\*.exe %1\Setup\InnoInstallers\
XCOPY /S /Y Setup\InstallerScripts\*.iss %1\Setup\InstallerScripts\

:: Create Build Destinations
MD %1\Installers
MD %1\Packages

goto DONE

:USAGE
echo.
echo Usage: PackageDeploymentSetup.cmd [DestinationRoot]
echo   
echo   PackageDeploymentSetup.cmd C:\STBBuilds\4.4
echo.  PackageDeploymentSetup.cmd C:\STBBuilds\4.4 /m
echo.
goto DONE

:DONE
