[Code]
// ############################################################################################
function InitializeSetup(): Boolean;
var
  UacChecker: String;
  ResCode: Integer;

begin  
  Result := True;

  ExtractTemporaryFiles('{app}\UACChecker.exe');
  UacChecker := ExpandConstant('{tmp}\')+'{app}\UACChecker.exe';
  CurrentUser := GetEnv('USERDOMAIN') + '\' +  ExpandConstant('{username}');

  Log(UacChecker);

  if ExecAsOriginalUser(UacChecker, '', '', SW_HIDE, ewWaitUntilTerminated, ResCode) then
    begin
      if ResCode = 0 then
        begin
          Log('The user is not elevated in Admin privs. Setup will exit.');
          MsgBox('You must run this installer with elevated administrator privileges.  Right click on the installer and select "Run as Administrator".', mbInformation, mb_Ok);
          Result := False;
        end    
    end;

    SqlExpressInstallRequired := not SqlServerInstalled();
    if  not SqlExpressInstallRequired and not SqlServerHasMixedAuth() then
      begin
        Log('Existing SQL Server installation is not configured for Mixed Authentication Mode.');
        MsgBox
        ('There is an existing installation of SQL Server on this system with only Windows Authentication mode enabled.  ' +
         'To install STB, Mixed Mode must be configured.  Please refer to the STB Installation and Administration Guide ' +
         'for detailed instructions about enabling Mixed Mode Authentication.  Setup will exit.', mbError, mb_Ok);
         Result := False;  
      end;
 
    IsNewInstall := not PreviousSTBServerInstallExists();

    // If this ends up being false, then the deinitializesetup will not run
    // which is good as it will give errors otherwise.
    IsInstallSetup := Result;
end;      

// ############################################################################################
procedure DeinitializeSetup();
var
  Installer: String;

begin
  // If the installer was never fully initialized then just return as there
  // is nothing to do.
  if not IsInstallSetup then
    Exit;

  Log('Removing installer');

  Installer := WizardDirValue() + '\Installer';
  if DirExists(Installer) then 
    begin
        if DelTree(Installer, True, True, True) = False then
          Log('Unable to delete: ' + Installer)
        else
          Log('Successfully deleted: ' + Installer);
    end;

  Log('DeinitializeSetup: Removing .NET Framework Installer download');
  if FileExists(FrameworkInstallerLocation) then 
    begin
        if DeleteFile(FrameworkInstallerLocation) = False then
            Log('Unable to delete ' + FrameworkInstallerLocation);
        Log('Deleted ' + FrameworkInstallerLocation);
    end;

  Log('Removing SQL Server Installer download');
  if FileExists(SqlInstallerLocation) then 
    begin
        if DeleteFile(SqlInstallerLocation) = False then
            Log('Unable to delete ' + SqlInstallerLocation);
        Log('Deleted ' + SqlInstallerLocation);
    end;

  Log('Removing SQL Server Installer extraction');
  if DirExists(SqlInstallerTempPath) then 
    begin
        if DelTree(SqlInstallerTempPath, True, True, True) = False then
            Log('Unable to delete ' + SqlInstallerTempPath);
        Log('Deleted ' + SqlInstallerTempPath);
    end;

end;

// #######################################################################################
function InitializeUninstall(): Boolean;
begin
  StbDataServiceName := 'hpstbds';

  Result := True;

  MsgBox('Note this uninstaller will only remove the STB programs and data service.  It will not remove SQL Server and the STB databases, or the STB File Share. Those must be removed manually.', mbInformation, mb_Ok);

  if ServiceExists(StbDataServiceName) then
    try
      SimpleStopService(StbDataServiceName, True, True);
      SimpleDeleteService(StbDataServiceName);
    except
      Log('Error stopping/deleting service: ' + GetExceptionMessage());
      MsgBox('Unable to remove the STB Data Service. You will have to manually remove it. Refer to the Setup log file for details', mbError, mb_Ok); 
  end;
end;

// ############################################################################################
procedure InitializeWizard();
var
  OverviewPage: TOutputMsgWizardPage;
  FrameworkDownloadPrompt: String;

begin
  StbDataServiceName := 'hpstbds';

  // Installer Overview Wizard Page
  OverviewPage := CreateOutputMsgPage(wpWelcome, 'Solution Test Bench Installation', 'Installing the Solution Test Bench Software',
  'This Setup will install the Solution Test Bench (STB) Server on this machine. ' +
  'The following components will be installed on this machine if not already present:'#13#13 +
  '- Microsoft .NET Framework 4.6, if not already installed.'#13 +
  '- Microsoft SQL Server Express 2014, if not already installed.'#13 +
  '- STB Database Instances:'#13 +
	'    - Configuration data which defines each test stored in STB for execution.'#13 +
	'    - Asset inventory which holds information on equipment that may be used in tests.'#13 +
	'    - Test Document inventory that lists documents that can be printed during a test.'#13 +
	'    - Data Log and Results that stores data gathered during test execution.'#13 +
  '- STB Management Service, used by client systems when communicating with STB.'#13 +
  '- STB User Console used by test engineers to design and execute tests.'#13 +
  '- STB Admin Console used by the STB administrator to manage and configure the system'#13 +
  '- Other general STB services and utilities');
  
  FrameworkDownloadPage := CreateOutputMsgPage(OverviewPage.ID,
    'Microsoft .NET Framework 4.6', 'Microsoft .NET Framework 4.6 installation', '');
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
    'Microsoft .NET Framework 4.6 is not installed on this server and is required for the Solution Test Bench.'#13#13 +
    'Make a selection below based on your need.  If you already have a local copy of the installer you may use it, ' +
    'otherwise you can indicate that you would like Setup to download the installer from Microsoft.' +
    'Once you make your choice, click Next to continue.'#13;
  FrameworkDownloadCheckList.AddGroup(FrameworkDownloadPrompt, '', 0, nil);
  FrameworkDownloadCheckList.AddRadioButton('I already have a local copy of the installer and will use it', '', 0, True, True, nil);
  FrameworkDownloadCheckList.AddRadioButton('Download the .NET Framework installer from the Microsoft site', '', 0, False, True, nil);

  FrameworkInstallerLocationPage := CreateInputFilePage(FrameworkDownloadPage.ID,
  'Select .NET Framework 4.6 installer', 'Where is the .NET Framework 4.6 installer located?',
  'Select where the .NET Framework 4.6 installer file is located on your system. ' +
  ' If you used the download option on the previous page it should be located in your Downloads folder.'#13#13 +
  'To continue, click Next.  If the field is blank or you would like to select a different file, click Browse.');
  FrameworkInstallerLocationPage.Add('Location of .NET Framework 4.6 Installer',
    'Executable files|*.exe|All files|*.*', '.exe');

  // SQL Installer Download Wizard Page
  SqlExpressDownloadPage := CreateOutputMsgPage(FrameworkInstallerLocationPage.ID,
    'SQL Server Express Database', 'SQL Server Express Database installation',
    'An instance of SQL Server was not detected on this server and is required for the Solution Test Bench.'#13#13 +
    'SQL Server Express 2014 will be downloaded from Microsoft and installed.  If you already have a local copy of ' +
    'the SQL Server Express installer you may use it by canceling the STB Server Setup and running the SQL Server Express ' +
    'installer manually, then re-running STB Server Setup.'#13#13 +
    '- Click Next to continue with the download'#13 +
    '- Click Cancel to manually install SQL Server Express'#13);

  // Database Files Location Wizard Page
  StbDatabaseLocationPage := CreateInputDirPage(wpSelectDir,
    'Select STB Database Files Location', 'Where should the STB database files be located?',
    'Setup will create the SQL Server Express databases to support STB data management. You can ' +
    'specify where you would like these database files to be located on your system.'#13#13 +
    'To continue, click Next.  If you would like to select a different folder, click Browse.',
    False, 'New Folder');
  StbDatabaseLocationPage.Add('');
  StbDatabaseLocationPage.Values[0] := GetDefaultDatabasePath();

  // SQL Server Database Connection Page
  SqlServerConnectionPage := CreateOutputMsgPage(StbDatabaseLocationPage.ID, 
  'SQL Server Database', 'Connect to Existing SQL Server Database',
  'An existing installation of SQL Server was detected on this system.'#13 +
  'Solution Test Bench Setup assumes that the currently logged in user has sufficient permissions to:'#13 +
  '    - Connect to the database service'#13 +
  '    - Create new databases'#13 +
  '    - Create new logins'#13 +
  '    - Create new tables'#13#13 +
  'Please ensure that [' +  CurrentUser + '] has enough database permissions to execute the above operations.'#13 +
  'Clicking Next will validate the database connection as [' + CurrentUser + '].');

  // Administrator Username Wizard Page
  GeneralInputPage := CreateInputQueryPage(SqlServerConnectionPage.ID,
  'General System Information', 'Provide Administrator and Organizational Information',
  'Please specify a domain\username combination and email address for the initial Administrator of the STB system. ' +
  'In addition please specify an organization name that will be used primarily for display ' +
  'within various STB applications.');

  GeneralInputPage.Add('Administrator Domain\Username:', False);
  GeneralINputPage.Add('Administrator Email:', False);
  GeneralInputPage.Add('Organization:', False);

  // Set initial values (optional)
  GeneralInputPage.Values[0] := CurrentUser;
  GeneralInputPage.Values[1] := 'user@company.com';
  GeneralInputPage.Values[2] := ExpandConstant('{sysuserinfoorg}');

  // File Share Location Wizard Page
  FileShareLocationPage := CreateInputDirPage(wpSelectDir,
    'Select STB File Share Location', 'Where should the STB File Share be located?',
    'Setup will create a File Share used by clients to access common artifacts like print drivers and test documents.' + 
    'Note that this file share will be open to Everyone with full control.  If this is too open for your own ' +
    'internal IT policies you will need to adjust it after the installation coompletes.'#13#13 + 
    'To continue, click Next.  If you would like to select a different folder, click Browse.',
    False, 'New Folder');

  FileShareLocationPage.Add('');
  FileShareLocationPage.Values[0] := (ExtractFileDrive(WizardDirValue()) + '\STBShare');

  idpSetOption('AllowContinue',  '1');
  idpSetOption('DetailedMode',   '0');
  idpSetOption('DetailsButton',  '1');
  idpSetOption('RetryButton',    '1');
  idpSetOption('ConnectTimeout', '60000');

  WizardForm.DiskSpaceLabel.Visible := False;
  SqlInstallerExtracted := False;
end;

// #######################################################################################
procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  Confirm := not CancelWithoutPrompt;
end;

// ############################################################################################
// Calculates whether or not the given PageID should be skipped as the pages are navigated.
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
      Result := not FrameworkInstallRequired or FrameworkDownloadRequested;
    end
  else if PageID = SqlExpressDownloadPage.ID then
    begin
      Result := SqlServerInstalled();
      SqlExpressInstallRequired := not Result;
    end
  else if PageID = StbDatabaseLocationPage.ID then
    begin
      Result := not SqlExpressInstallRequired;
    end
  else if PageID = SqlServerConnectionPage.ID then
    begin
      SetControlCursor(WizardForm, crHourGlass);
      Result := SqlExpressInstallRequired or IsStbSchemaCreatedIntSec(GetComputerNameString());
      SetControlCursor(WizardForm, crDefault);
    end
  else if PageID = GeneralInputPage.ID then
    begin
      Result := not SqlExpressInstallRequired and IsStbSchemaCreatedIntSec(GetComputerNameString());
    end
  else if PageID = FileShareLocationPage.ID then
    begin
      Result := FileShareExists();
    end
end;

// ############################################################################################
function NextButtonClick(CurPageID: Integer): Boolean;
var
  SqlInstallerUrl: String;
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
          MsgBox('Select a valid installer file before continuing.', mbError, mb_Ok);
          Result := False;
        end
    end
  else if CurPageID = SqlExpressDownloadPage.ID then
    begin
      SqlExpressDownloadRequested := True;

      // This will kick off the download process using the download plugin
      // Once completed, path will be set in the installer download page field.
      SqlInstallerUrl := 'http://download.microsoft.com/download/E/A/E/EAE6F7FC-767A-4038-A954-49B8B05D04EB/Express%2064BIT/SQLEXPR_x64_ENU.exe';
      //SqlInstallerUrl := 'http://stfweb/download/Express/SQLEXPR_x64_ENU.exe';
      SqlInstallerLocation := GetTempDir() + 'SQLEXPR_x64_ENU.exe';
      Log('SQL Server Express Installer will download to: ' + SqlInstallerLocation);
      idpClearFiles();
      idpAddFile(SqlInstallerUrl, SqlInstallerLocation); 
      idpDownloadAfter(SqlExpressDownloadPage.ID);        
    end
  else if CurPageID = SqlServerConnectionPage.ID then
    begin
      SetControlCursor(WizardForm, crHourGlass);
      // Validate the connection to the SQL Server Instance
      if not CheckConnectionIntSec() then
        begin
          MsgBox('Unable to connect to the local database as [' + CurrentUser + '].'#13 +
          'This may be caused by remote connections being disabled for TCP/IP on the database instance.  ' +
          'Please refer to the STB Installation and Administration Guide for details about troubleshooting database connections.', mbError, mb_Ok);
          Result := False;
        end
      else
        MsgBox('Database connection succeeded!', mbInformation, mb_Ok);

      SetControlCursor(WizardForm, crDefault);
    end
  else if CurPageID = FileShareLocationPage.ID then
    begin
      if not FileShareExists() and DirExists(FileShareLocationPage.Values[0]) then
        if MsgBox('The path you entered for the STB File Share already exists.'#13 + 'Do you still want to use it?', mbConfirmation, MB_YESNO or MB_DEFBUTTON1) = IDNO then
          begin
            Result := False;
          end
    end
  else if CurPageID = GeneralInputPage.ID then
    begin
      if not ValidateUsernameDomain(GeneralInputPage.Values[0]) then
        begin
          MsgBox('The Username field must have a domain\username combination', mbError, mb_Ok);
          Result := False;
        end
      else if not ValidateEmail(GeneralInputPage.Values[1]) then
        begin
          MsgBox('The Email field must have a valid email address', mbError, mb_Ok);
          Result := False;
        end
      else if GeneralInputPage.Values[2] = '' then
        begin
          MsgBox('The Organization field must have a value', mbError, mb_Ok);
          Result := False;
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
  
  if SqlExpressInstallRequired then
    S := S + 'SQL Server Express will be installed' + NewLine;
  S := S + NewLine; 
  S := S + MemoDirInfo + NewLine;
  S := S + NewLine;
  S := S + 'Solution Test Bench database files location' + NewLine;  
  S := S + Space +StbDatabaseLocationPage.Values[0] + NewLine;

  Result := S;
end;


