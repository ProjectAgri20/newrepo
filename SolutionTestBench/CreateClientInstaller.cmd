@ECHO OFF
:: This script will create an STB Installer based on the provided partner name and STB version number

IF "%1" == "" goto USAGE

IF NOT EXIST "C:\Program Files (x86)\Inno Setup 5\iscc.exe" (
ECHO.
ECHO ERROR: Inno Setup does not appear to be installed.
ECHO        It must be present before building an installer.
ECHO        Refer to the Readme.html file for details.
GOTO:DONE
)

"C:\Program Files (x86)\Inno Setup 5\iscc.exe"  /dVERSION="%1" "%~dp0Setup\ClientSetup\ClientSetupMain.iss"
if %errorlevel% NEQ 0 (
    exit /b %errorlevel%
)

COPY /Y "%~dp0Setup\ClientSetup\Output\Setup.exe" "%~dp0Installers\STBClientSetup.exe" > nul

:: Display the time so it shows when this script was last run.
Time /T

GOTO:DONE

:USAGE
echo.
echo Usage: CreateClientInstaller [Version]
echo   ex: CreateClientInstaller 3.13
echo.
goto DONE

:DONE
