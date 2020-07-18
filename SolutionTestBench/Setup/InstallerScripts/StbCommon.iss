[Code]

var
  FrameworkDownloadPage: TOutputMsgWizardPage;
  FrameworkInstallRequired: Boolean;
  FrameworkDownloadCheckList: TNewCheckListBox;
  FrameworkDownloadRequested: Boolean;

  FrameworkInstallerLocationPage: TInputFileWizardPage;
  FrameworkInstallerLocation: String;
  IsNewInstall: Boolean;
  CancelWithoutPrompt: Boolean;

 const
  SqlCmdUnspecified = $FFFFFFFF;
  SqlCmdUnknown = $00000008;
  SqlCmdText = $00000001;
  SqlCmdTable = $00000002;
  SqlCmdStoredProc = $00000004;
  SqlCmdFile = $00000100;
  SqlCmdTableDirect = $00000200;
  SqlOptionUnspecified = $FFFFFFFF;
  SqlAsyncExecute = $00000010;
  SqlAsyncFetch = $00000020;
  SqlAsyncFetchNonBlocking = $00000040;
  SqlExecuteNoRecords = $00000080;
  SqlExecuteStream = $00000400;
  SqlExecuteRecord = $00000800;

// ############################################################################################
function GetHKLM: Integer;
begin
  if IsWin64 then
    Result := HKLM64
  else
    Result := HKLM32;
end;

// #######################################################################################
function BoolToStr(Value: Boolean): String;
begin
  Result := 'False';
  if Value then
    Result := 'True';
end;

// #######################################################################################
function ValidateUsernameDomain(Value : String) : Boolean;
var
  Slash: Integer;

begin
  Value := Trim(Value);
  Slash := Pos('\', Value);

  if Slash <= 1 then
    Result := False
  else if Slash = Length(Value) then
    Result := False
  else
    Result := True;
end;

// #######################################################################################
function ValidateEmail(Email : String) : Boolean;
var
  Temp  : String;
  Space   : Integer;
  At      : Integer;
  Dot     : Integer;

begin
  Email := Trim(Email);
  Space := Pos(' ', Email);
  At := Pos('@', Email);
  Temp := Copy(Email, At + 1, Length(Email) - At + 1);
  Dot := Pos('.', Temp) + At;
  Result := ((Space = 0) and (1 < At) and (At + 1 < Dot) and (Dot < Length(Email)));
end;

// ############################################################################################
function SqlServerInstalled: Boolean;
begin
  Result := False;
  if RegKeyExists(GetHKLM(), 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL') then
    Result := True
end;

//#############################################################################################
//To be ran before new registry entries are added to determine if new install or update
function PreviousSTBClientInstallExists: Boolean;
begin
  Result := False;
  if RegKeyExists(GetHKLM(), 'SOFTWARE\HP\STB User Console\Settings') then
    Result := True
end;

//#############################################################################################
//To be ran before new registry entries are added to determine if new install or update
function PreviousSTBServerInstallExists: Boolean;
begin
  Result := False;
  if RegKeyExists(GetHKLM(), 'Software\HP\Solution Test Bench\Settings') then
    Result := True
end;

// ############################################################################################
function SqlServerHasMixedAuth: Boolean;
var
  InstanceNames: TArrayOfString;
  InstanceName: String;
  Index: Integer;
  LoginMode: Cardinal;
  BasePath: String;
  Path: String;

begin
  Result := True;

  BasePath :=  'SOFTWARE\Microsoft\Microsoft SQL Server\';
  Log(BasePath);
  Path :=  BasePath + 'Instance Names\SQL';
  Log(Path);
  if RegGetValueNames(GetHKLM(), Path, InstanceNames) then
    begin
      for Index := 0 to GetArrayLength(InstanceNames) - 1 do
        begin
          RegQueryStringValue(GetHKLM(), Path, InstanceNames[Index], InstanceName);
          Log(InstanceNames[Index]);
          Log(InstanceName);
          Path :=  BasePath + InstanceName + '\MSSQLServer';
          Log(Path);
          if RegQueryDWordValue(GetHKLM(), Path, 'LoginMode', LoginMode) then
            begin
              Log('LoginMode: ' + IntToStr(LoginMode));
              if IntToStr(LoginMode) = '1' then
                begin
                  Result := False;
                  Exit;
                end;
            end;
        end;
    end;
end;

// ############################################################################################
function GetDefaultDatabasePath: String;
var
  InstanceNames: TArrayOfString;
  InstanceName: String;
  RegBasePath: String;
  RegPath: String;
  DataPath: String;

begin
  DataPath := 'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL'; //Default if registry entry is not found

  RegBasePath :=  'SOFTWARE\Microsoft\Microsoft SQL Server\';
  Log(RegBasePath);
  RegPath :=  RegBasePath + 'Instance Names\SQL';
  Log(RegPath);
  if RegGetValueNames(GetHKLM(), RegPath, InstanceNames) then
    begin
      RegQueryStringValue(GetHKLM(), RegPath, InstanceNames[0], InstanceName);
      Log(InstanceNames[0]);
      Log(InstanceName);
      RegPath :=  RegBasePath + InstanceName + '\Setup';
      Log(RegPath);
      if RegQueryStringValue(GetHKLM(), RegPath, 'SQLDataRoot', DataPath) then
        begin
          Log('Found SQLDataRoot registry entry: ' + DataPath);
        end;
    end;
  Log('Default database Path: ' + DataPath + '\Data');
  Result := DataPath + '\Data';
end;

// ############################################################################################
function FrameworkInstalled: Boolean;
var
  key: String;
  Release: Cardinal;

  // 378389 .NET Framework 4.5
  // 378675 .NET Framework 4.5.1 installed with Windows 8.1 or Windows Server 2012 R2
  // 378758 .NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2
  // 379893 .NET Framework 4.5.2
  // 393297 .NET Framework 4.6

begin
  Result := False;
  key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full';

  if RegQueryDWordValue(GetHKLM(), key, 'Release', release) then
    begin
      Log('FrameworkInstalled: Release: ' + IntToStr(Release));
      Result := (Release >= 393297);
    end
  else
    Log('FrameworkInstalled: False')
end;

// #######################################################################################
procedure UpdateAppConfig(ConfigFile, Hostname: String);
var
  XMLDoc: Variant;
  Node: Variant;
  ChildNode: Variant;

begin
  try
     XMLDoc := CreateOleObject('Msxml2.DOMDocument.6.0');
  except
    Log('Failed to create XML document object.  ' + ConfigFile);
    MsgBox('Error modifying hpstbcp.exe.config and SolutionTestBench.exe.config to add your STB server host name.  You will need to manually update these files.', mbError, mb_Ok);
    Exit;
  end;  

  XMLDoc.async := False;
  XMLDoc.resolveExternals := False;
  XMLDoc.load(ConfigFile);

  if XMLDoc.parseError.errorCode <> 0 then
    begin
      Log('Failed to open XML document.  ' + ConfigFile);
      Log('Error on line ' + IntToStr(XMLDoc.parseError.line) + ', position ' + IntToStr(XMLDoc.parseError.linepos) + ': ' + XMLDoc.parseError.reason);
      MsgBox('Error modifying hpstbcp.exe.config and SolutionTestBench.exe.config to add your STB server host name.  You will need to manually update these files.', mbError, mb_Ok);
      Exit;
    end;

  Node := XMLDoc.documentElement.selectSingleNode('//configuration/Systems');

  ChildNode := Node.childNodes.Item[0];
  ChildNode.setAttribute('key', 'Solution Test Bench Server');
  ChildNode.setAttribute('value', Hostname);

  Node.selectNodes('add').removeAll();
  Node.appendChild(ChildNode);

  XMLDoc.Save(ConfigFile); 
end;

// #######################################################################################
function InstallFramework: Boolean;
var
  StatusText: string;
  ResCode: Integer;

begin
  MsgBox('The .NET Framework will now be installed', mbInformation, mb_Ok);

  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing .NET framework...';
  WizardForm.ProgressGauge.Style := npbstMarquee;

  Result := True;
  try
    Log('Installing ' + FrameworkInstallerLocationPage.Values[0]);
    if ShellExecAsOriginalUser('open', FrameworkInstallerLocationPage.Values[0], '/norestart', '', SW_HIDE, ewWaitUntilTerminated, ResCode) then
      begin
        Log('Installation completed, exit code: ' + IntToStr(ResCode));
        if ResCode <> 0 then
          begin
            Log('InstallFramework: .NET Framework Installation was cancelled by the user.  Setup will exit.');
            MsgBox('The .NET Framework install has been cancelled.  Setup will be aborted', mbInformation, mb_Ok);
            Result := False;
          end    
      end
    else
      begin
        Log('InstallFramework: .NET Framework Installation Error: ' + SysErrorMessage(ResCode));
        MsgBox('Failed to install .NET Framework.  Refer to installation log file for details.', mbError, mb_Ok);
        Result := False;
      end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

// #######################################################################################
// Connects to the specified database via connection string and checks the extended property
// for "STF Revision".  If found, returns true.  If not found or can't connect, returns false.
function CheckSchema(ConnectionString: String): Boolean;
var
  QueryResult: String;
  ADOCommand: Variant;
  ADORecordset: Variant;
  ADOConnection: Variant;  

begin
  Result := False;

  try
    ADOConnection := CreateOleObject('ADODB.Connection');
    ADOConnection.ConnectionString := ConnectionString;

    try
      ADOConnection.Open;
      ADOCommand := CreateOleObject('ADODB.Command');
      ADOCommand.ActiveConnection := ADOConnection;
      ADOCommand.CommandText := 
        'IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N''STF Revision'', NULL, NULL, NULL, NULL, NULL, NULL)) ' +
        '   select 1 as Result ' +
        'ELSE ' +
        '   select 0 as Result';

      ADOCommand.CommandType := SqlCmdText;
      ADORecordset := ADOCommand.Execute;
      QueryResult := ADORecordset.Fields(0);

      if QueryResult = '1' then
        Result := True;    

    finally
      ADOConnection.Close;
    end;
  except
    Log('CheckSchema::Failed to connect. ConnectionString: ' + ConnectionString);
    Log(AddPeriod(GetExceptionMessage));
  end;
end;

// #######################################################################################
function IsStbSchemaCreated(Hostname, Username, Password: String): Boolean;
var
  ConnectionString: String;

begin
  ConnectionString := 
    'Provider=SQLOLEDB;' +
    'Data Source=' + Hostname + ';' +
    'Initial Catalog=master;' +
    'User Id=' + Username + ';' +
    'Password=' + Password + ';';

  Result := CheckSchema(ConnectionString);
end;

// #######################################################################################
function IsStbSchemaCreatedIntSec(Hostname: String): Boolean;
var
  ConnectionString: String;

begin
  ConnectionString :=
    'Provider=SQLOLEDB;' +
    'Data Source=' + Hostname + ';' +
    'Initial Catalog=master;' +
    'Integrated Security=SSPI;';

  Result := CheckSchema(ConnectionString);
end;

// #######################################################################################
// Connects to the local database via integrated security.
// If connection is successful, returns true.  If can't connect, returns false.
function CheckConnectionIntSec(): Boolean;
var
  ADOConnection: Variant;
  ConnectionString: String;

begin
  Result := False;

  ConnectionString := 
    'Provider=SQLOLEDB;' +
    'Data Source=' + GetComputerNameString() + ';' +
    'Initial Catalog=master;' +
    'Integrated Security=SSPI;';

  try
    ADOConnection := CreateOleObject('ADODB.Connection');
    ADOConnection.ConnectionString := ConnectionString;

    try
      ADOConnection.Open;
      Result := True;    

    finally
      ADOConnection.Close;
    end;
  except
    Log('CheckConnection::Failed to connect. ConnectionString: ' + ConnectionString);
    Log('CheckConnection::' + GetExceptionMessage);
    Result := False;
  end;
end;

// #######################################################################################
// The problem with this method is that you have to know what version of SQL Server "might" be installed
// on the target machine in order to instantiate a WMI Service object (see the line below where the FWMIService
// object is created.  The exact version of ComputerManagement has to be specified, or the operation fails.
// ComputerManagement12 = SqlServer 14.  ComputerManagement14 = SqlServer 17.  The code would have to loop
// through the known values just to notify the user that Tcp is not enabled.  For now, this method is not used.
// I'm leaving it in the code as an artifact of the research done and the decision made. -kyoungman 04/2019
function RemoteConnectionEnabled(): Boolean;
var
    FSWbemLocator: Variant;
    FWMIService: Variant;
    FWbemObjectSet: Variant;
begin
    Result := false;
    FSWbemLocator := CreateOleObject('WBEMScripting.SWBEMLocator');
    FWMIService := FSWbemLocator.ConnectServer('', 'root\Microsoft\SqlServer\ComputerManagement12', '', '');
    FWbemObjectSet := FWMIService.ExecQuery('SELECT * FROM ServerNetworkProtocol WHERE (InstanceName = "SQLEXPRESS" OR InstanceName = "MSSQLSERVER") AND ProtocolName = "Tcp"');
    Result := (FWbemObjectSet.Count > 0);

    FWbemObjectSet := Unassigned;
    FWMIService := Unassigned;
    FSWbemLocator := Unassigned;

end;

// #######################################################################################
// Sets the specified TCursor for the specified TControl.
procedure SetControlCursor(Control: TControl; Cursor: TCursor);
var 
  i: Integer;
  Component: TComponent;
begin
  Control.Cursor := Cursor;
  for i := 0 to Control.ComponentCount-1 do
  begin
    Component := Control.Components[i];
    if Component is TControl then
    begin
      SetControlCursor(TControl(Component), Cursor);
    end;
  end;
end;
