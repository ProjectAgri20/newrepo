@ECHO OFF
:: This script will create an STB Installer for the given STB version number

IF "%1" == "" goto USAGE
IF "%2"=="" ( SET "Target=Default" ) ELSE ( SET "Target=%2" )

IF NOT EXIST "C:\Program Files (x86)\Inno Setup 5\iscc.exe" (
ECHO.
ECHO ERROR: Inno Setup does not appear to be installed.
ECHO        It must be present before building an installer.
ECHO        Refer to the Readme.html file for details.
GOTO:DONE
)

"C:\Program Files (x86)\Inno Setup 5\iscc.exe"  /dVERSION="%1" /dTARGET="%Target%" "%~dp0Setup\ServerSetup\ServerSetupMain.iss"
if %errorlevel% NEQ 0 (
    exit /b %errorlevel%
)

COPY /Y "%~dp0Setup\ServerSetup\Output\Setup.exe" "%~dp0Installers\STBServerSetup-%Target%-%1.exe" > nul

:: Display the time so it shows when this script was last run.
Time /T

GOTO:DONE

:USAGE
echo.
echo Usage: CreateServerInstaller [Version] [Target]
echo   ex: CreateServerInstaller 3.13
echo.
echo Note that [Version] must be supplied, but if [Target] is not provided, then the Default target is used, which comes with the intial installation of the STB Deployment Package.
echo.
goto DONE

:DONE
