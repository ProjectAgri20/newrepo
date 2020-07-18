@ECHO OFF
echo.  
set logServer=%4
set packSize=%1
set delay=%2
set numberOfDevices=%3
set counter=%5

IF "%1" == "" goto USAGE
IF "%2" == "" goto USAGE
IF "%3" == "" goto USAGE

IF "%4" == "/i" set logServer=

IF "%5" == "" set counter=1
IF %counter% GTR 255 goto USAGE

REM Add the starting index to the number of indexes.
set /a numberOfDevices += counter

echo Creating Virtual Printers...
echo.
 
:LOOP
if %counter% GEQ %numberOfDevices% goto COMPLETED
echo StartPrinter.cmd %packSize% %delay% %counter% %logServer%
start StartPrinter.cmd %packSize% %delay% %counter% %logServer%
set /a counter += 1
if %counter% GTR 255 goto COMPLETED
goto LOOP

:USAGE
echo.
echo   Usage: StartPrinters ^<packetSize^> ^<delayBetweenPackets^> ^<NumberOfDevices^> [^<SettingsServer^>] [^<StartIndex^>]
echo   Note that SettingsServer is optional.  If no settings server is provided, logging will be written to the TraceFile.
echo   Note that the StartIndex is optional, but if the SettingsServer is not included, the /i option will have to be used to set the StartIndex.
echo   The StartIndex indicates the starting point for the last octet of the IP addresses that will be created.
echo   Ex: StartPrinters 100000 2 15 STFSystem02 5
echo   OR
echo   Ex: StartPrinters 100000 2 15 /i 5
echo.
goto DONE

:COMPLETED
echo.
echo All printers should have started.
echo.

:DONE