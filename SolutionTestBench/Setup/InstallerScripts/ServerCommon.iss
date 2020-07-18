[Code]

var
  SqlExpressDownloadPage: TOutputMsgWizardPage;
  SqlExpressInstallRequired: Boolean;
  SqlExpressDownloadRequested: Boolean;

  SqlInstallerLocation: String;
  SqlInstallerExtracted: Boolean;
  SqlInstallerTempPath: String;

  GeneralInputPage: TInputQueryWizardPage;

  StbDatabaseLocationPage: TInputDirWizardPage;
  SqlServerConnectionPage: TOutputMsgWizardPage;
  FileShareLocationPage: TInputDirWizardPage;
  FileShareCreationRequired: Boolean;

  StbDataServiceName: String;
  IsInstallSetup: Boolean;
  CurrentUser: String;


// ############################################################################################
function FileShareExists(): Boolean;
var
  SharePath: String;

begin
  Result := False;
  FileShareCreationRequired := True;

  //Log('Looking for \\LOCALHOST\STB');

  if DirExists('\\LOCALHOST\STBShare') then
    begin
      if RegQueryStringValue(GetHKLM(), 'Software\HP\Solution Test Bench\Settings', 'FileShare', SharePath) then
        Log('File share exists at ' + SharePath)
      else
        Log('Unable to determine File share location');
      FileShareCreationRequired := False;
      Result := True;
      FileShareLocationPage.Values[0] := SharePath;
    end
  else
    Log('File share does not exist');


    Log('FileShareCreationRequired: ' + BoolToStr(FileShareCreationRequired));
end;

// ############################################################################################
function ExtractSqlExpressInstaller(Installer: String) : Boolean;
var
  ResCode : Integer;
  Args: String;
  StatusText: string;

begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Extracting SQL Server Express Installer...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  
  Result := False;
  try
    //SqlInstallerTempPath := GetTempDir() + '230A4C5E-FC93-4FA2-B915-B6ACE8576E4A';
    SqlInstallerTempPath := ExpandConstant('{sd}') + '\STBSQL';

    Log('ExtractSqlExpressInstaller: Installer: ' + Installer);
    Log('ExtractSqlExpressInstaller: SqlInstallerTempPath: ' + SqlInstallerTempPath);

    if DirExists(SqlInstallerTempPath) then 
      begin
        if not DelTree(SqlInstallerTempPath, True, True, True) then
          Log('ExtractSqlExpressInstaller: Unable to delete ' + SqlInstallerTempPath);
        Log('ExtractSqlExpressInstaller: Deleted ' + SqlInstallerTempPath);
      end;

    Args := '/u /x:"' + SqlInstallerTempPath + '"';  
    if ShellExecAsOriginalUser('open', Installer, Args, SqlInstallerTempPath, SW_SHOWNORMAL, ewWaitUntilTerminated, ResCode) then
      begin
        Log('ExtractSqlExpressInstaller: Installer successfully extracted');
        Result := True;
      end
    else
      begin
        Log('ExtractSqlExpressInstaller: Error extracting SQL Server installer. Error Message: ' + SysErrorMessage(ResCode));
        Result := False;
      end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

// ############################################################################################
function CopyConfigurationFile() : Boolean;
var
  Source: String;
  Destination: String;

begin
  Result := True;

  Source := ExpandConstant('{app}') + '\Installer\SQLEXPR_CONFIGURATION.ini';
  Log('CopyConfigurationFile:Source: ' + Source);

  Destination := SqlInstallerTempPath + '\Configuration.ini';
  Log('CopyConfigurationFile: Destination: ' + Destination);

  if not FileCopy(Source, Destination, False) then
    begin
      Log('CopyConfigurationFile: Unable to move Configuration.ini file into place');
      MsgBox('An error occured in setting up the SQL Server Express installation. Unable to copy Configuration file', mbInformation, mb_Ok);
      Result := False;
    end
end;

// ############################################################################################
function InstallSqlExpress() : Boolean;
var
  ResCode : Integer;
  Args: String;
  Setup: String;
  ConfigFile: String;
  StatusText: string;

begin
  MsgBox('SQL Server Express will now be installed.  You can accept the defaults by simply clicking "Next" at each screen.  The only places you are required to provide input is to agree to the license and to enter an "sa" password.', mbInformation, mb_Ok);

  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing SQL Server Express...';
  WizardForm.ProgressGauge.Style := npbstMarquee;

  Result := True;
  try
    Setup := SqlInstallerTempPath + '\Setup.exe';
    ConfigFile := SqlInstallerTempPath + '\Configuration.ini';
    Log('InstallSqlExpress:Setup: ' + Setup);

    Args := '/UIMode=AutoAdvance /IACCEPTSQLSERVERLICENSETERMS /UpdateEnabled=0 /CONFIGURATIONFILE=' + ConfigFile;
    Log('InstallSqlExpress: Args: ' + Args);

    if ShellExecAsOriginalUser('open', Setup, Args, SqlInstallerTempPath, SW_SHOWNORMAL, ewWaitUntilTerminated, ResCode) then
      begin
        if ResCode <> 0 then
          begin
            Log('SysErrorMessage: ' + SysErrorMessage(ResCode));
            Log('InstallSqlExpress: SQL Installation was cancelled by the user.  Setup will exit.');
            MsgBox('The SQL Express install has been cancelled.  Setup will be aborted', mbInformation, mb_Ok);
            Result := False;
          end    
      end
    else
      begin
        Log('InstallSqlExpress: SQL Server Express Setup Error: ' + SysErrorMessage(ResCode));
        MsgBox('Failed to install SQL Server Express.  Refer to installation log file for details.', mbError, mb_Ok);
        Result := False;
      end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

// #######################################################################################
procedure StopStbService(ServiceName: string);
begin
  try
    SimpleStopService(ServiceName, True, True);
    Log('StopStbService: ' + ServiceName + ' successfully stopped');
  except
    Log('Error stopping ' + ServiceName + ': ' + GetExceptionMessage());
  end;
end;

// #######################################################################################
function StartStbService(ServiceExePath, ServiceName: string): Boolean;
var
  ComputerName: String;
  ResultCode: Integer;

begin
  Result := True;

  // Create the ServiceStartArgs.dat file in the service directory that will be used
  // by the service to connect to the local database instance.
  ComputerName := ExpandConstant('{computername}');
  Log('StartStbService: ' + ComputerName);

  SaveStringToFile(ExpandConstant('{app}') +  '\DataService\ServiceStartArgs.dat', '/database=' + ComputerName, false);

  Log('StartStbService: Uninstalling service if it exists')
  try
    Exec(ServiceExePath, '-uninstall', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
  except
    Log('Service was not installed or did not uninstall successfully.')
  end

  Log('StartStbService: Installing: ' + ServiceExePath);
  try
    if not Exec(ServiceExePath, '-install', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    //if not SimpleCreateService(ServiceName, ServiceName, ServiceExePath, SERVICE_AUTO_START, '', '', True, True) then
    //if SimpleInstallService(ServiceExe, StbDataServiceName, StbDataServiceName, 'STB Data Service', SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START) = False then
      begin
        Result := False;
        Log('StartStbService: Unable to install ' + ServiceName);
        Exit;
      end
    else
      Log('StartStbService: ' + ServiceName + ' successfully installed');
  except
    Log('Error installing service: ' + GetExceptionMessage());
    Result := False;
    Exit;
  end;

  Log('StartStbService: Attempting to start or restart ' + ServiceName);
  try
    Exec('cmd', '/c "NET START ' + ServiceName + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  except
    Log('Error starting service: ' + GetExceptionMessage());
    Result := False;
  end;
end;

// #######################################################################################
procedure ProcessServerInstallation();
var
  InstallData: String;
  FileName: String;
  SchemaInstaller: String;
  ResCode : Integer;

begin
  CancelWithoutPrompt := false;

  if FrameworkInstallRequired and not InstallFramework() then
    begin
      CancelWithoutPrompt := True;
      WizardForm.Close();
      Exit;
    end;

  if SqlExpressInstallRequired then
    begin
      // If SQL Server Express installation is required, then first extract the
      // installer into a temp location, and return an error if there is a problem.
      if not SqlInstallerExtracted then
        begin
          Log('ProcessInstallation: Extracting SQL Server Express installer');
          SqlInstallerExtracted := ExtractSqlExpressInstaller(SqlInstallerLocation);
          if not SqlInstallerExtracted then
            begin
              CancelWithoutPrompt := True;
              Log('ProcessInstallation: Failed to install SQL Server Express, unable to extract installer');
              MsgBox('Failed to install SQL Server Express.  Refer to installation log file for details.', mbError, mb_Ok);
              WizardForm.Close();
              Exit;
            end
          else
            Log('ProcessInstallation: SQL Server Express installer successfully extracted');
        end;
      // Copy the Configuration.ini file from the Installer location into the appropriate 
      // temp location.  Return if there is an error with this.
      if not CopyConfigurationFile() then
        begin
          CancelWithoutPrompt := True;
          WizardForm.Close();
          Exit;
        end
      else
        Log('ProcessInstallation: Configuration.ini copied into temp location sucessfully.');

      // Finally attempt to install SQL Server Express and return an error
      // if there is a problem.
      if not InstallSqlExpress() then
        begin
          CancelWithoutPrompt := True;
          WizardForm.Close();
          Exit;
        end
    end
  else
    Log('ProcessInstallation: SQL Server Express already installed on system');
 
  // Drop a data file out in temp that will be picked up the the STB Schema
  // installer (C# code base) and used to define how to create the tables
  // and add initial data to STF.

  InstallData := GeneralInputPage.Values[0];
  InstallData := InstallData + #13#10;
  InstallData := InstallData + GeneralInputPage.Values[1];
  InstallData := InstallData + #13#10;
  InstallData := InstallData + GeneralInputPage.Values[2];
  InstallData := InstallData + #13#10;
  InstallData := InstallData + StbDatabaseLocationPage.Values[0];
  InstallData := InstallData + #13#10;
  InstallData := InstallData + '{#TARGET}';
  InstallData := InstallData + #13#10;
  if FileShareCreationRequired then
    InstallData := InstallData + FileShareLocationPage.Values[0]
  else
    InstallData := InstallData + 'NONE';

  Log('ProcessInstallation: Installation parameters: ' + InstallData);

  FileName := GetTempDir() + 'stb-setup.dat';
  Log('ProcessInstallation: Setup Filename: ' + FileName);

  if not SaveStringToFile(FileName, InstallData, False) then
    begin
      CancelWithoutPrompt := True;
      Log('ProcessInstallation: Failed to write temp file for installation');
      MsgBox('Failed to write temp file for installation', mbError, mb_Ok);
      WizardForm.Close(); 
      Exit;              
    end;

  SchemaInstaller :=  ExpandConstant('{app}') + '\Installer\StbInstaller.exe';
  Log('ProcessInstallation: SchemaInstaller: ' + SchemaInstaller);

  if ShellExecAsOriginalUser('open', SchemaInstaller, FileName, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResCode) then
    begin
      if ResCode <> 0 then
        begin
          Log('ProcessInstallation: SchemaInstaller failed.  Exit code: ' + IntToStr(ResCode));
          CancelWithoutPrompt := True;
          MsgBox('The STB install has been cancelled.  Setup will be aborted', mbInformation, mb_Ok);
          WizardForm.Close();
          Exit;
        end 
      else
        Log('Schema installer return: ' + IntToStr(ResCode));
    end
  else
    begin
      Log('ProcessInstallation: STB Setup Error: ' + SysErrorMessage(ResCode));
      CancelWithoutPrompt := True;
      MsgBox('Unable to install database entries.  Setup will exit. Error Message: ' + SysErrorMessage(ResCode), mbError, mb_Ok);
      WizardForm.Close();
      Exit;
    end;  

  if not StartStbService(ExpandConstant('{app}') +  '\DataService\hpstbds.exe', StbDataServiceName) then
    MsgBox('Failed to start the STB data service.  Refer to installation log file for details.', mbError, mb_Ok);

  // Write out the file share information so there is record that it has been
  // created.  Subsequent runs of setup should skip this share setup step
  // because it has already been created.
  if FileShareCreationRequired then
    RegWriteStringValue(GetHKLM(), 'Software\HP\Solution Test Bench\Settings', 'FileShare', FileShareLocationPage.Values[0]);

  if IsNewInstall then
  begin
    UpdateAppConfig(ExpandConstant('{app}') + '\ControlPanel\hpstbcp.exe.config', GetComputerNameString());
    UpdateAppConfig(ExpandConstant('{app}') + '\STBUserConsole\SolutionTestBench.exe.config', GetComputerNameString());
  end

end;
