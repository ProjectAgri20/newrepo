#define AppName = "STF Admin Console"
#define ExeName = "HP.STF.AdminConsole.exe"
#define DistName = "STFAdminConsole"
#define InstallDir = AddBackslash("{app}") + AppName

#define AppVer GetFileVersion("..\..\VirtualResource\Distribution\" + DistName + "\" + ExeName)

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{7A9171E6-9D5E-459D-BDFE-E5C6F539786C}
AppName={#AppName}
AppVersion={#AppVer}
AppVerName={#AppName} {#AppVer}
AppPublisher=HP Reliable Deployment Lab
DefaultDirName={pf}\Scalable Test Framework
DefaultGroupName=Scalable Test Framework
DirExistsWarning=no
AllowNoIcons=yes
OutputDir=..\..\VirtualResource\Setup
OutputBaseFilename={#DistName}Setup-{#AppVer}
SetupIconFile=Setup.ico
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
UninstallFilesDir={#InstallDir}
UninstallDisplayIcon={#InstallDir}\{#ExeName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\..\VirtualResource\Distribution\{#DistName}\{#ExeName}"; DestDir: "{#InstallDir}"; Flags: ignoreversion
Source: "..\..\VirtualResource\Distribution\{#DistName}\*"; DestDir: "{#InstallDir}"; Flags: ignoreversion recursesubdirs
Source: "..\..\VirtualResource\Distribution\Plugin\*"; DestDir: "{app}\Plugin"; Flags: ignoreversion recursesubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#AppName}"; Filename: "{#InstallDir}\{#ExeName}"
Name: "{commondesktop}\{#AppName}"; Filename: "{#InstallDir}\{#ExeName}"; Tasks: desktopicon

[Run]
Filename: "{#InstallDir}\{#ExeName}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

