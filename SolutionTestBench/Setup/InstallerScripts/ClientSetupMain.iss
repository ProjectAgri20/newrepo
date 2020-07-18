
; STB USER CONSOLE INSTALLER
; Note: this installer uses a proprocessor macro called "VERSION" to define
; which the version for this release.  The way the program is compiled
; from the command line is:
;    iscc.exe /dVERSION="<version> ClientSetupMain.iss

; Here for DEBUG, remove when building for release
;#define VERSION "3.13"

#define ExeName = "SolutionTestBench.exe"

#define AppVer GetFileVersion("..\..\Binaries\STB User Console\" + ExeName)

[Setup]
AppName=Solution Test Bench User Console
AppVersion={#AppVer}
AppCopyright=Copyright (C) 2017 HP Inc.
AppPublisher=HP Inc.
AllowCancelDuringInstall=yes
DefaultDirName={sd}\VirtualResource\Distribution
DefaultGroupName=Solution Test Bench
PrivilegesRequired=admin
Compression=lzma2
SolidCompression=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
RestartApplications=no
CloseApplications=no
SetupLogging=yes
UninstallDisplayName=HP Solution Test Bench
UninstallDisplayIcon={app}\STBUserConsole\SolutionTestBench.exe
UninstallLogMode=new
;INSERTOUTPUTDIR
;INSERTOUTPUTBASEFILENAME

[Registry]
Root: HKLM; Subkey: "SOFTWARE\HP\STB User Console\Settings"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}\STBUserConsole"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\"; ValueType: String; ValueName: "{app}\STBUserConsole\SolutionTestBench.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 6.1

[Icons]
Name: "{group}\STB User Console"; Filename: "{app}\STBUserConsole\{#ExeName}"; WorkingDir: "{app}"
Name: "{group}\Uninstall STB User Console"; Filename: "{uninstallexe}"

#include <idp.iss>

[Files]
Source: "..\..\Binaries\Plugin\*.exe"; DestDir: "{app}\Plugin"; Flags: ignoreversion
;Source: "..\..\Binaries\Plugin\*.pdb"; DestDir: "{app}\Plugin"; Flags: ignoreversion skipifsourcedoesntexist
;Source: "..\..\Binaries\Plugin\*.dll"; DestDir: "{app}\Plugin"; Flags: ignoreversion
;INSERTPLUGINSHERE

Source: "..\..\Binaries\Solution Tester Console\*.dll"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion
Source: "..\..\Binaries\Solution Tester Console\*.config"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion
Source: "..\..\Binaries\Solution Tester Console\*.pdb"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\..\Binaries\Solution Tester Console\*.exe"; DestDir: "{app}\SolutionTesterConsole"; Flags: ignoreversion

Source: "..\..\Binaries\STB User Console\*.dll"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion
Source: "..\..\Binaries\STB User Console\SolutionTestBench.exe.config"; DestDir: "{app}\STBUserConsole"; Flags: onlyifdoesntexist
Source: "..\..\Binaries\STB User Console\*.pdb"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\..\Binaries\STB User Console\SolutionTestBench.exe"; DestDir: "{app}\STBUserConsole"; Flags: ignoreversion; AfterInstall: ProcessClientInstallation()

Source: "..\..\Documentation\*.pdf"; DestDir: "{app}\Documentation"; Flags: ignoreversion

[Run]
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12668/DataGateway/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:12672/SessionClient/ user=Everyone"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http add urlacl url=http://+:10101/ user=Everyone"; Flags: runhidden
;Filename: "{app}\STBUserConsole\SolutionTestBench.exe"; Description: "Launch Solution Test Bench User Console"; Flags: postinstall nowait skipifsilent

[UninstallRun]
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12668/DataGateway/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:12672/SessionClient/"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters: "http delete urlacl url=http://+:10101/ user=Everyone"; Flags: runhidden


[Code]
#include "NTServices.iss"
#include "StbCommon.iss"

var
   StbUserConsoleAddressPage: TInputQueryWizardPage;

// #######################################################################################
procedure ProcessClientInstallation();
begin
  if IsNewInstall then
    UpdateAppConfig(ExpandConstant('{app}') + '\STBUserConsole\SolutionTestBench.exe.config', StbUserConsoleAddressPage.Values[0]);

  if FrameworkInstallRequired and not InstallFramework() then
    begin
      CancelWithoutPrompt := True;
      WizardForm.Close();
      Exit;
    end;
end;

// ############################################################################################
procedure InitializeWizard();
var
  FrameworkDownloadPrompt: String;

begin
  // Installer Overview Wizard Page
  FrameworkDownloadPage := CreateOutputMsgPage(wpWelcome, 'MS .NET Framework v4.5.2', 'MS .NET Framework v4.5.2 installation', '');
  FrameworkDownloadCheckList := TNewCheckListBox.Create(FrameworkDownloadPage);
  FrameworkDownloadCheckList.Top := ScaleY(10);
  FrameworkDownloadCheckList.Width := FrameworkDownloadPage.SurfaceWidth;
  FrameworkDownloadCheckList.Height := ScaleY(200);
  FrameworkDownloadCheckList.BorderStyle := bsNone;
  FrameworkDownloadCheckList.ParentColor := True;
  FrameworkDownloadCheckList.MinItemHeight := WizardForm.TasksList.MinItemHeight;
  FrameworkDownloadCheckList.ShowLines := False;
  FrameworkDownloadCheckList.WantTabs := True;
  FrameworkDownloadCheckList.Parent := FrameworkDownloadPage.Surface;

  FrameworkDownloadPrompt := 
    'MS .NET Framework v4.6 is not installed on this server and is required for the Solution Test Bench.'#13#13 +
    'Make a selection below based on your need.  If you already have a local copy of the installer you may use it, ' +
    'otherwise you can indicate below that you would like Setup to download the installer from Microsoft.' +
    'Once you make your choice, click Next to continue.'#13;
  FrameworkDownloadCheckList.AddGroup(FrameworkDownloadPrompt, '', 0, nil);
  FrameworkDownloadCheckList.AddRadioButton('I already have my own local copy of the Installer and will use it', '', 0, True, True, nil);
  FrameworkDownloadCheckList.AddRadioButton('Download .NET Framework v4.6 Installer from the Microsoft site', '', 0, False, True, nil);

    // SQL Installer File Location Wizard Page
  FrameworkInstallerLocationPage := CreateInputFilePage(FrameworkDownloadPage.ID,
  'Select .NET Framework v4.6 Installer', 'Where is the .NET Framework v4.6 Installer Located?',
  'Select where the .NET Framework v4.6 install file is located on your system.  If ' +
  'you used the download option on the previous page it should be in your Downloads folder.'#13#13 +
  'To continue, click Next.  If the field is blank or you would like to select a different file, click Browse.');
  FrameworkInstallerLocationPage.Add('Location of .NET Framework v4.6 Installer',
    'Executable files|*.exe|All files|*.*', '.exe');
   
   StbUserConsoleAddressPage := CreateInputQueryPage(FrameworkInstallerLocationPage.ID,
   'STB Server Location', 'Provide the location of the STB Server',
   'Please provide the hostname or network address of your STB server.');
    
  StbUserConsoleAddressPage.Add('STB Server Address', False);

  // Set initial values (optional)
  //StbUserConsoleAddressPage.Values[0] := '';

  idpSetOption('AllowContinue',  '1');
  idpSetOption('DetailedMode',   '0');
  idpSetOption('DetailsButton',  '1');
  idpSetOption('RetryButton',    '1');
  idpSetOption('ConnectTimeout', '60000');

  WizardForm.DiskSpaceLabel.Visible := False;
end;

// #######################################################################################
procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  Confirm := not CancelWithoutPrompt;
end;

// ############################################################################################
function InitializeSetup(): Boolean;
begin  
  Result := True;
  IsNewInstall := not PreviousSTBClientInstallExists();

  //if IsAdminLoggedOn() = False then
  //  begin
  //    MsgBox('You must be an Administrator to run this Setup', mbError, mb_Ok);
  //    Result := False;
  //  end
end;      

// ############################################################################################
function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;

  if PageID = FrameworkDownloadPage.ID then
    begin
      Result := FrameworkInstalled();
      FrameworkInstallRequired := not Result;
    end
  else if PageID = FrameworkInstallerLocationPage.ID then
    begin
      Result := not FrameworkInstallRequired or FrameworkDownloadRequested
    end
end;

// ############################################################################################
function NextButtonClick(CurPageID: Integer): Boolean;
var
  FrameworkInstallerUrl: String;

begin
  Result := True;

  if CurPageID = FrameworkDownloadPage.ID then
    begin
      FrameworkDownloadRequested := FrameworkDownloadCheckList.Checked[2];
      if (FrameworkDownloadRequested) then 
        begin
          // This will kick off the download process using the download plugin
          // Once completed, path will be set in the installer download page field.

          FrameworkInstallerUrl := 'https://download.microsoft.com/download/C/3/A/C3A5200B-D33C-47E9-9D70-2F7C65DAAD94/NDP46-KB3045557-x86-x64-AllOS-ENU.exe';
          //FrameworkInstallerUrl := 'http://stfweb/download/Framework/NDP452-KB2901907-x86-x64-AllOS-ENU.exe';
          FrameworkInstallerLocation := GetTempDir() + 'NDP46-KB3045557-x86-x64-AllOS-ENU.exe';
          Log('Installer will download to ' + FrameworkInstallerLocation);
          idpClearFiles();
          idpAddFile(FrameworkInstallerUrl, FrameworkInstallerLocation); 
          idpDownloadAfter(FrameworkDownloadPage.ID);          
          FrameworkInstallerLocationPage.Values[0] := FrameworkInstallerLocation;
        end
    end
  else if CurPageID = FrameworkInstallerLocationPage.ID then
    begin
      if FrameworkInstallRequired and not FileExists(FrameworkInstallerLocationPage.Values[0]) then
        begin
          MsgBox('Select a valid installer file before continuing', mbError, mb_Ok);
          Result := False;
        end
    end
  else if CurPageID = StbUserConsoleAddressPage.ID then
    begin
      if StbUserConsoleAddressPage.Values[0] = '' then
        begin
          MsgBox('The STB Server Address field must have a value', mbError, mb_Ok);
          Result := False;
        end
      else if not IsStbSchemaCreated(StbUserConsoleAddressPage.Values[0], 'enterprise_admin', 'enterprise_admin') then
        begin
          MsgBox('Unable to connect to an STB database on the host ' + StbUserConsoleAddressPage.Values[0] + '.  Please contact your administrator if you feel the host you entered is correct.', mbError, mb_Ok);
          //Result := False;
        end
    end
end;

// ############################################################################################
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  S := '';
  
  S := S + MemoDirInfo + NewLine;
  S := S + NewLine;

  Result := S;
end;



