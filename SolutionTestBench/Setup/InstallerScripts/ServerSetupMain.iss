; STB INSTALLER
; Note: this installer uses a proprocessor macro called "PARTNER" to define
; which metadata type data file to load.  It also has a macro called "VERSION"
; to define the version of this release. The way the program is compile 
; from the command line is:
;    iscc.exe /dPARTNER="<PartnerName>" /dVERSION="<Version>" StbSetupMain.iss

; Here for DEBUG, remove when building for release
;#define VERSION "3.13"
;#define TARGET "Default"

#define ExeName = "hpstbcp.exe"

#define AppVer GetFileVersion("..\..\Binaries\STB Control Panel\" + ExeName)

[Setup]
AppName=Solution Test Bench
AppVerName=The Solution Test Bench Server {#AppVer}
AppVersion={#AppVer}
AppCopyright=Copyright (C) 2019 HP Inc.
AppPublisher=HP Inc.
AllowCancelDuringInstall=yes
DefaultDirName={sd}\VirtualResource\Distribution
DefaultGroupName=Solution Test Bench
Compression=lzma2
SolidCompression=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
RestartApplications=no
CloseApplications=no
SetupLogging=yes
UninstallDisplayName=HP Solution Test Bench
UninstallDisplayIcon={app}\ControlPanel\hpstbcp.exe
PrivilegesRequired=admin
;INSERTOUTPUTDIR
;INSERTOUTPUTBASEFILENAME

[Registry]
Root: HKLM; Subkey: "Software\HP\Solution Test Bench\Settings"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\"; ValueType: String; ValueName: "{app}\Installer\StbInstaller.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 6.1
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\"; ValueType: String; ValueName: "{app}\ControlPanel\hpstbcp.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 6.1
;Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\"; ValueType: String; ValueName: "{app}\UserConsole\SolutionTestBench.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 6.1

[Icons]
Name: "{group}\Admin Control Panel"; Filename: "{app}\ControlPanel\{#ExeName}"; WorkingDir: "{app}"
Name: "{group}\STB User Console"; Filename: "{app}\STBUserConsole\SolutionTestBench.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall STB"; Filename: "{uninstallexe}"

#include <idp.iss>

[Files]
Source: "..\..\\Binaries\UACChecker.exe"; DestDir: {app}

Source: "..\..\Binaries\STB Control Panel\*.dll"; DestDir: "{app}\ControlPanel"; Flags: ignoreversion
Source: "..\..\Binaries\STB Control Panel\hpstbcp.exe.config"; DestDir: "{app}\ControlPanel"; Flags: onlyifdoesntexist
;Source: "..\..\Binaries\STB Control Panel\*.pdb"; DestDir: "{app}\ControlPanel"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\..\Binaries\STB Control Panel\hpstbcp.exe"; DestDir: "{app}\ControlPanel"; Flags: ignoreversion

Source: "..\..\Binaries\STB User Console\*.dll"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion
Source: "..\..\Binaries\STB User Console\SolutionTestBench.exe.config"; DestDir: "{app}\STBUserConsole"; Flags: onlyifdoesntexist
;Source: "..\..\Binaries\STB User Console\*.pdb"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\..\Binaries\STB User Console\SolutionTestBench.exe"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion

Source: "..\..\Binaries\Plugin\*.exe"; DestDir: "{app}\Plugin"; Flags: ignoreversion
;Source: "..\..\Binaries\Plugin\*.pdb"; DestDir: "{app}\Plugin"; Flags: ignoreversion skipifsourcedoesntexist
;Source: "..\..\Binaries\Plugin\*.dll"; DestDir: "{app}\Plugin"; Flags: ignoreversion
;INSERTPLUGINSHERE

Source: "..\..\Binaries\Solution Tester Console\*.dll"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion
Source: "..\..\Binaries\Solution Tester Console\*.config"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion
;Source: "..\..\Binaries\Solution Tester Console\*.pdb"; DestDir: "{app}\SolutionTesterConsole"; Flags: skipifsourcedoesntexist ignoreversion
Source: "..\..\Binaries\Solution Tester Console\*.exe"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion

Source: "..\..\Binaries\STB Data Service\hpstbds.exe"; DestDir: "{app}\DataService"; Flags: ignoreversion; BeforeInstall: StopStbService('hpstbds');
Source: "..\..\Binaries\STB Data Service\*.dll"; DestDir: "{app}\DataService"; Flags: ignoreversion
Source: "..\..\Binaries\STB Data Service\hpstbds.exe.config"; DestDir: "{app}\DataService"; Flags: ignoreversion
;Source: "..\..\Binaries\STB Data Service\*.pdb"; DestDir: "{app}\DataService"; Flags: skipifsourcedoesntexist ignoreversion

Source: "..\..\Configuration\{#TARGET}\*.stbs"; DestDir: "{app}\Installer\Import"; Flags: skipifsourcedoesntexist
Source: "..\..\Configuration\{#TARGET}\PluginList.txt"; DestDir: "{app}\Installer";

Source: "..\..\Documentation\*.pdf"; DestDir: "{app}\Documentation"; Flags: ignoreversion
Source: "..\..\Documentation\PluginSdk\*.*"; DestDir: "{app}\Documentation\PluginSdk"; Flags: ignoreversion

Source: "..\..\Binaries\Database Installer\SQLEXPR_CONFIGURATION.ini"; DestDir: "{app}\Installer"; Flags: ignoreversion

Source: "..\..\Binaries\Database Installer\*.dll"; DestDir: "{app}\Installer"; Flags: ignoreversion
Source: "..\..\Binaries\Database Installer\StbInstaller.exe.config"; DestDir: "{app}\Installer"; Flags: ignoreversion
;Source: "..\..\Binaries\Database Installer\*.pdb"; DestDir: "{app}\Installer"; Flags: skipifsourcedoesntexist ignoreversion
Source: "..\..\Binaries\Database Installer\StbInstaller.exe"; DestDir: "{app}\Installer"; Flags: ignoreversion; AfterInstall: ProcessServerInstallation()

[Dirs]
Name: "{app}\Installer\Import"

[Run]
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12664/GlobalLock/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12668/DataGateway/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12671/TestDocument/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12672/SessionClient/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12673/AssetInventory/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12678/DataLog/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:10101/ user=Everyone"; Flags: runhidden
;Filename: "{app}\ControlPanel\hpstbcp.exe"; Description: "Launch STB Administrator Control Panel"; Flags: postinstall nowait skipifsilent unchecked
;Filename: "{app}\UserConsole\SolutionTestBench.exe"; Description: "Launch Solution Test Bench User Console"; Flags: postinstall nowait skipifsilent unchecked

[UninstallRun]
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12664/GlobalLock/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12668/DataGateway/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12671/TestDocument/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12672/SessionClient/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12673/AssetInventory/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12678/DataLog/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:10101/ user=Everyone"; Flags: runhidden

[Code]
#include "NTServices.iss"
#include "StbCommon.iss"
#include "ServerCommon.iss"
#include "ServerSetupEventHandlers.iss"

               
