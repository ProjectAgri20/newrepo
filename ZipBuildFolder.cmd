@ ECHO OFF

REM Replace space in hour with zero if ti's less than 10
SET hr=%time:~0,2%
IF %hr% lss 10 SET hr=hr:~1,1%

REM This set the date like this: mm-dd-yr-hrminsecs1/100secs
SET TODAY=%date:~4,2%-%date:~7,2%-%date:~10,4%

REM Use 7zip to zip files in folder

ECHO.
ECHO Zipping all files in Folder

"C:\Program Files\7-Zip\7z.exe" a -tzip "C:\STFBuilds\3.16\Build\STFBuild_%TODAY%.zip" "C:\STFBuilds\3.16\VirtualResource\*" -mx5

PAUSE