@ECHO OFF

REM mode con lines=40
REM mode con cols=132

IF "%1" == "" goto USAGE
IF "%2" == "" goto USAGE
IF "%3" == "" goto USAGE
IF %3 GTR 255 goto USAGE

IF "%4" == "" ( 
VirtualPrintDevice.exe /s %1 /d %2 %3 
) ELSE ( 
VirtualPrintDevice.exe /s %1 /d %2 /c %4 %3 
)

goto DONE

:USAGE
echo.
echo 255 IP Addresses are statically assigned to this machine.  Pass in only the last octet of the IP Address you want to bind to, within the range of 1 to 255.
echo Note that the SettingsServer parameter is optional.  If not included, logging will be written to the TraceLog file.
echo Usage: StartPrinter ^<packetSize^> ^<delayBetweenPackets^> ^<lastOctectOfIPAddress^> [^<SettingsServer^>]
echo   StartPrinter 100000 2 51 STFSystem02
echo.
goto DONE

:DONE
