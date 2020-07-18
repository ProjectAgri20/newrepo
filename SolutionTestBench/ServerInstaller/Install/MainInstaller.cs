using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using HP.ScalableTest;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;

namespace HP.SolutionTest.Install
{
    internal class MainInstaller : InstallerBase
    {
        private const string BackupFolderName = "STBBackup";
        private readonly DatabaseScripts _databaseScripts = null;
        private readonly SchemaTicket _ticket = null;
        private string _dnsDomainName = string.Empty;
        private string _fileSharePath = string.Empty;
        private static readonly string[] _delimiter = new string[] { "GO{0}".FormatWith(Environment.NewLine) };

        public MainInstaller(SchemaTicket ticket)
        {
            _ticket = ticket;
            _databaseScripts = new DatabaseScripts();
            _dnsDomainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }

        /// <summary>
        /// Gets the database schema version.
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Version GetSchemaVersion(string hostname)
        {
            using (SqlTextAdapter adapter = new SqlTextAdapter(hostname, "Master"))
            {
                return SelectSchemaVersion(adapter);
            }
        }

        private static Version SelectSchemaVersion(SqlTextAdapter adapter)
        {
            StringBuilder stringVersion = null;
            Version version = new Version(0, 0, 0);

            try
            {
                // Select the current schema version from the Extended Property of the database         
                string sqlText = "SELECT value FROM fn_listextendedproperty(N'STF Revision', NULL, NULL, NULL, NULL, NULL, NULL)";
                var reader = adapter.ExecuteReader(sqlText);
                if (reader != null && reader.Read())
                {
                    stringVersion = new StringBuilder((string)reader["value"]);
                    Match match = Regex.Match(stringVersion.ToString(), @"([0-9]+)\.([0-9]+)\.*([0-9]*)", RegexOptions.IgnoreCase);
                    stringVersion.Clear();
                    stringVersion.Append(match.Groups[1].Value);
                    stringVersion.Append(".");
                    stringVersion.Append(match.Groups[2].Length == 0 ? "0" : match.Groups[2].Value);
                    stringVersion.Append(".");
                    stringVersion.Append(match.Groups[3].Length == 0 ? "0" : match.Groups[3].Value);
                    version = new Version(stringVersion.ToString());
                }
            }
            catch (Exception ex)
            {
                SystemTrace.Instance.Error(ex.ToString());
            }

            return version;
        }

        /// <summary>
        /// Run the installation.
        /// Exceptions are handled as follows: 
        /// New Install: All databases are dropped and fileshares removed.
        /// Update: All databases are restored to pre-update state.
        /// </summary>
        /// <param name="mainServer"></param>
        public void Run(string mainServer)
        {
            try
            {
                Worker.WorkerSupportsCancellation = true;

                if (string.IsNullOrEmpty(mainServer))
                {
                    // This path is used for STB installation.  All databases are
                    // local to one machine so the mainServer value is not used.
                    _ticket.CurrentVersion = GetSchemaVersion(Environment.MachineName);

                    if (!Directory.Exists(_ticket.DatabaseFilesPath))
                    {
                        Directory.CreateDirectory(_ticket.DatabaseFilesPath);
                    }

                    Worker.DoWork += InstallSTB;
                }
                else
                {
                    // This path is used for STF database update.  It will only update
                    // the database servers with any new update scripts.  In this
                    // case the mainServer value is important.
                    _ticket.CurrentVersion = GetSchemaVersion(mainServer);

                    Worker.DoWork += UpdateSTF;
                }

                _ticket.Log();
                Worker.RunWorkerAsync(mainServer);
            }
            catch (Exception ex)
            {
                if (_ticket.IsNewInstall)
                {
                    RollBackSTBInstall();
                }
                else
                {
                    RollBackSTBUpdate();
                }

                SendError(ex);
                return;
            }
        }

        /// <summary>
        /// Update an existing installation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateSTF(object sender, DoWorkEventArgs e)
        {
            string mainServer = (string)e.Argument;
            if (!PerformDatabaseUpdates(mainServer))
            {
                UpdateStatus("Installation canceled.");
                return;
            }
            SendComplete();
        }

        /// <summary>
        /// Install STB for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallSTB(object sender, DoWorkEventArgs e)
        {
            UpdateStatus("Creating file share...");

            CreateFileShare();

            // If this is a new installation, ie the STB databases are not installed
            // then build the databases and install the initial data.
            if (_ticket.IsNewInstall)
            {
                SystemTrace.Instance.Debug("This is a new installation");
                if (!CreateDatabases())
                {
                    UpdateStatus("Installation canceled.");
                    return;
                }
            }
            else
            {
                SystemTrace.Instance.Debug("There is an existing installation at V{0}".FormatWith(_ticket.CurrentVersion));
            }

            // Check for any database updates added to this installation
            if (!PerformDatabaseUpdates())
            {
                UpdateStatus("Installation canceled.");
                return;
            }

            // Now that the database is created, load the global settings and insert
            // the plugin metadata which determines which plugins are available.
            UpdateStatus("Loading settings from {0}...".FormatWith(_ticket.ServerHostname));
            try
            {
                GlobalSettings.Load(_ticket.ServerHostname);
                UpdateStatus("Settings loaded");
            }
            catch (Exception ex)
            {
                UpdateStatus("Failed to load settings: " + ex.ToString());
                throw;
            }

            GlobalSettings.IsDistributedSystem = false;

            // Insert any new plugin types that are added to this installation            
            if (!InsertPluginTypes()) return;

            ApplyBuildSpecificSettings();

            // Import any configurations that have be added to this installation
            if (!ImportConfigurationFiles()) return;

            SendComplete();
            UpdateStatus("Database Configuration Complete");
        }

        private bool ImportConfigurationFiles()
        {
            try
            {
                SystemTrace.Instance.Debug("Checking for import files");

                string[] importFiles = Directory.GetFiles("Import", "*.stbs");

                if (importFiles.Count() > 0)
                {
                    // Import any exported scenario files that were included in the installation
                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        UpdateStatus("Importing Scenarios");
                        ContractFactory.OnStatusChanged += ContractFactory_OnStatusChanged;

                        string folderName = $"V{_ticket.CurrentVersion} Imports";

                        ConfigurationTreeFolder folder = context.ConfigurationTreeFolders.FirstOrDefault(x => x.Name.Equals(folderName));
                        if (folder == null)
                        {
                            folder = new ConfigurationTreeFolder(SequentialGuid.NewGuid(), folderName, ConfigurationObjectType.ScenarioFolder.ToString(), null);
                            context.AddToConfigurationTreeFolders(folder);
                            context.SaveChanges();
                        }

                        //Ensure the path exists for import files in the shared folder location
                        StringBuilder importDestination = new StringBuilder(_fileSharePath);
                        importDestination.Append("\\Import\\V");
                        importDestination.Append(_ticket.CurrentVersion);

                        Directory.CreateDirectory(importDestination.ToString());

                        importDestination.Append("\\");
                        int destinationPathIndex = importDestination.Length;

                        foreach (string fileName in importFiles)
                        {
                            SystemTrace.Instance.Debug($"Importing {fileName}");
                            EnterpriseScenario enterpriseScenario = null;

                            try
                            {
                                XElement fileData = XElement.Parse(File.ReadAllText(fileName));

                                // If this is a composite contract file it may contain printer and document
                                // information in addition to the base scenario data. 
                                if (fileData.Name.LocalName == "Composite")
                                {
                                    var compositeContract = Serializer.Deserialize<EnterpriseScenarioCompositeContract>(fileData);

                                    if (!ImportExportUtil.ProcessCompositeContractFile(compositeContract))
                                    {
                                        SystemTrace.Instance.Error($"Failed to process composite contract: {fileName}.");
                                    }

                                    enterpriseScenario = ContractFactory.Create(compositeContract.Scenario);
                                }
                                else
                                {
                                    var scenarioContract = Serializer.Deserialize<EnterpriseScenarioContract>(fileData);
                                    enterpriseScenario = ContractFactory.Create(scenarioContract);
                                }

                                enterpriseScenario.FolderId = folder.ConfigurationTreeFolderId;
                                SystemTrace.Instance.Debug($"Adding Scenario '{enterpriseScenario.Name}'");
                                context.AddToEnterpriseScenarios(enterpriseScenario);

                                // Copy the import file to the shared folder location
                                importDestination.Append(Path.GetFileName(fileName));
                                try
                                {
                                    File.Copy(fileName, importDestination.ToString(), true);
                                }
                                catch (Exception ex)
                                {
                                    SystemTrace.Instance.Error($"Failed to copy '{fileName}' to '{importDestination.ToString()}'.", ex);
                                }
                                importDestination.Remove(destinationPathIndex, importDestination.Length - destinationPathIndex);
                            }
                            catch (Exception ex)
                            {
                                // Log an error for the current file, but keep going
                                SystemTrace.Instance.Error($"Failed to import: {fileName}", ex);
                            }
                        }

                        context.SaveChanges();
                        ContractFactory.OnStatusChanged -= ContractFactory_OnStatusChanged;

                        UpdateStatus("Scenario Import Complete");
                    }
                }

            }
            catch (Exception ex)
            {
                SendError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cancel the installation.
        /// </summary>
        public override void Cancel()
        {
            if (Worker.IsBusy)
            {
                Worker.CancelAsync();
            }

            if (_ticket.IsNewInstall)
            {
                RollBackSTBInstall();
            }
            else
            {
                RollBackSTBUpdate();
            }
        }

        private void ContractFactory_OnStatusChanged(object sender, ScalableTest.Utility.StatusChangedEventArgs e)
        {
            SystemTrace.Instance.Debug($"ContractFactory.Create::{e.StatusMessage}");
        }

        private bool CreateDatabases()
        {
            SystemTrace.Instance.Debug("Creating databases...");
            string creationSQL = string.Empty; //Cache this in case we need to roll back.
            StringBuilder sqlText = new StringBuilder();

            // Create tables, insert data, etc. for each database
            using (SqlTextAdapter adapter = new SqlTextAdapter(Environment.MachineName, "Master"))
            {
                try
                {
                    UpdateStatus("Creating Solution Test Bench database instances...");
                    EmbeddedResource stream = new EmbeddedResource("HP.SolutionTest.DatabaseCreation");
                    creationSQL = stream.Read("create_DatabaseInstances.sql");
                    sqlText.Append(creationSQL);
                    sqlText.Replace("{DATABASE_PATH}", _ticket.DatabaseFilesPath);
                    if (!ExecuteSql(sqlText.ToString(), adapter, false))
                    {
                        return false;
                    }

                    UpdateStatus("Creating Solution Test Bench database users...");
                    if (!ExecuteSql(stream.Read("create_DatabaseUsers.sql"), adapter, true))
                    {
                        return false;
                    }

                    UpdateStatus("Creating Solution Test Bench primary configuration database schema...");
                    if (!ExecuteSql(stream.Read("create_SchemaEnterpriseTest.sql"), adapter, true))
                    {
                        return false;
                    }

                    UpdateStatus("Creating Solution Test Bench asset inventory database schema...");
                    if (!ExecuteSql(stream.Read("create_SchemaAssetInventory.sql"), adapter, true))
                    {
                        return false;
                    }

                    UpdateStatus("Creating Solution Test Bench test document database schema...");
                    if (!ExecuteSql(stream.Read("create_SchemaTestDocumentLibrary.sql"), adapter, true))
                    {
                        return false;
                    }

                    UpdateStatus("Creating Solution Test Bench data log database schema...");
                    if (!ExecuteSql(stream.Read("create_SchemaScalableTestDatalog.sql"), adapter, true))
                    {
                        return false;
                    }

                    UpdateStatus("Populating Solution Test Bench databases with base configuration data...");
                    sqlText.Clear();
                    sqlText.Append(stream.Read("ins_BaseConfigurationData.sql"));
                    sqlText.Replace("{FILE_SHARE}", _ticket.FileShareName);
                    sqlText.Replace("{SERVER_ADDRESS}", _ticket.ServerHostname);
                    sqlText.Replace("{ADMIN_USER}", _ticket.AdminUserName);
                    sqlText.Replace("{ADMIN_DOMAIN}", _ticket.AdminDomain);
                    sqlText.Replace("{ORGANIZATION}", _ticket.OrganizationName);
                    sqlText.Replace("{ADMIN_EMAIL}", _ticket.AdminEmail);
                    sqlText.Replace("{DNS_DOMAIN}", _dnsDomainName);
                    sqlText.Replace("{AD_DOMAIN}", Environment.UserDomainName);
                    if (!ExecuteSql(sqlText.ToString(), adapter, true))
                    {
                        return false;
                    }

                    Version version = new Version(stream.Read("SchemaVersion.txt"));
                    SystemTrace.Instance.Debug($"Schema version: {version}");

                    UpdateStatus("Updating Solution Test Bench database to initial version");
                    sqlText.Clear();
                    sqlText.Append(stream.Read("ins_SchemaVersion.sql"));
                    sqlText = sqlText.Replace("{0}", version.ToString());
                    if (!ExecuteSql(sqlText.ToString(), adapter, true))
                    {
                        return false;
                    }

                    // Update the ticket to the current version
                    _ticket.CurrentVersion = version;
                }
                catch (Exception ex)
                {
                    SendError(ex);
                    DropDatabaseInstances(GetSTBDatabaseNames(ref creationSQL), adapter);
                    return false;
                }
            }
            return true;
        }

        private bool InsertPluginTypes()
        {
            UpdateStatus("Enabling new plugins for this installation");

            try
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    List<MetadataType> currentPlugins = context.MetadataTypes.ToList();

                    string[] lines = File.ReadAllLines("PluginList.txt");
                    SendProgressStart(lines.Count());

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(lines[i]) && !lines[i].StartsWith("//") && !lines[i].StartsWith("*"))
                        {
                            string[] items = lines[i].Split(',');
                            if (items.Length < 1)
                            {
                                SystemTrace.Instance.Error("Need at least 1 plugin name per line");
                            }
                            else
                            {
                                string name = items[0].Trim();

                                // default the title to the name if not specified
                                string title = (items.Length > 1 && !string.IsNullOrEmpty(items[1]) ? items[1].Trim() : name);

                                // default the group to blank if not specified
                                string group = (items.Length > 2 && !string.IsNullOrEmpty(items[2]) ? items[2].Trim() : string.Empty);

                                // default the icon to null if not specified
                                byte[] icon = (items.Length > 3 && !string.IsNullOrEmpty(items[3]) ? Convert.FromBase64String(items[3].Trim()) : null);

                                MetadataType type = new MetadataType()
                                {
                                    Name = name,
                                    Title = title,
                                    Group = group,
                                    AssemblyName = "Plugin.{0}.dll".FormatWith(name),
                                    Icon = icon
                                };

                                // Only add plugins if they are new
                                if (!currentPlugins.Any(x => x.Name.Equals(name)))
                                {
                                    if (!context.MetadataTypes.Any(x => x.Name.Equals(type.Name)))
                                    {
                                        foreach (var resourceType in context.ResourceTypes)
                                        {
                                            SystemTrace.Instance.Debug("Enabling {0}".FormatWith(type.Name));
                                            type.ResourceTypes.Add(resourceType);
                                        }

                                        SendProgressUpdate(i + 1);
                                        if (!context.MetadataTypes.Any(x => x.Name.Equals(type.Name)))
                                        {
                                            context.AddToMetadataTypes(type);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    context.SaveChanges();
                    SendProgressEnd();
                }
            }
            catch (Exception ex)
            {
                SendError(ex);
                return false;
            }

            return true;
        }

        private void ApplyBuildSpecificSettings()
        {
            try
            {
                switch (_ticket.BuildConfig)
                {
                    case BuildConfiguration.AllPlugins:
                        InsertSystemSetting("FrameworkSetting", Setting.EnterpriseEnabled.ToString(), "True", "Enables Enterprise-level Features");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        }

        private void InsertSystemSetting(string subType, string settingName, string settingValue, string description)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                if (!context.SystemSettings.Any(s => s.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SystemSetting newSetting = new SystemSetting()
                    {
                        Type = SettingType.SystemSetting.ToString(),
                        SubType = subType,
                        Name = settingName,
                        Value = settingValue,
                        Description = description
                    };
                    context.AddToSystemSettings(newSetting);
                    context.SaveChanges();
                }
            }
        }

        private List<SqlUpdateScript> GetScripts()
        {
            // Grab each database update script from the embedded resource set and then sort them by the
            // version number in ascending order, weeding out any less than the current version being upgraded
            List<SqlUpdateScript> scripts = new EmbeddedResource("HP.SolutionTest.DatabaseUpdates")
                                    .GetScripts()
                                    .Where(x => x.Version > _ticket.CurrentVersion)
                                    .OrderBy(x => x.Version)
                                    .ToList();

            SystemTrace.Instance.Debug("Update script count: {0}".FormatWith(scripts.Count));
            return scripts;
        }

        /// <summary>
        /// Runs the specified update script against the database.
        /// Note: Assumes transactions are being handled outside of this method.
        /// </summary>
        /// <param name="adapter">The SQL adapter</param>
        /// <param name="script">The databse update script</param>
        /// <returns>True if the script was executed successfully.  False otherwise.</returns>
        private bool RunDatabaseUpdate(string databaseHost, SqlUpdateScript script)
        {
            // Execute the SQL update script against the defined database host
            using (SqlTextAdapter adapter = new SqlTextAdapter(databaseHost, "Master"))
            {
                UpdateStatus("Updating {0} to version {1}".FormatWith(script.Database, script.Version));
                if (!ExecuteSql(script.SqlText, adapter, true))
                {
                    return false;
                }
            }
            return true;
        }

        private bool UpdateVersion(string databaseHost, Version version)
        {
            // Update the STB/STF version on the defined database host.
            string updateVersionSql = new EmbeddedResource("HP.SolutionTest.DatabaseCreation").Read("ins_SchemaVersion.sql");

            using (var adapter = new SqlTextAdapter(databaseHost, "Master"))
            {
                adapter.BeginTransaction();
                if (!ExecuteSql(updateVersionSql.FormatWith(version.ToString()), adapter, false))
                {
                    adapter.RollbackTransaction();
                    return false;
                }
                adapter.CommitTransaction();
                _ticket.CurrentVersion = version;
            }

            return true;
        }

        private string GetDatabaseHostName(string primaryServer, string databaseName)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext(primaryServer))
            {
                var setting = context.SystemSettings.FirstOrDefault(x => x.Name.Equals(databaseName));
                if (setting != null)
                {
                    return setting.Value;
                }
                else
                {
                    var msg = "Unable to connect to primary database {0}".FormatWith(primaryServer);
                    SystemTrace.Instance.Error(msg);
                    throw new InvalidOperationException(msg);
                }
            }
        }

        /// <summary>
        /// Run updates on new install.
        /// </summary>
        /// <returns></returns>
        private bool PerformDatabaseUpdates()
        {
            UpdateStatus("Performing database updates...");

            List<SqlUpdateScript> scripts = GetScripts();
            scripts.Sort();

            foreach (SqlUpdateScript script in scripts)
            {
                if (CancelPending())
                {
                    return false;
                }

                if (!RunDatabaseUpdate(Environment.MachineName, script))
                {
                    return false;
                }
            }

            if (scripts.Count > 0)
            {
                Version version = scripts.Max(x => x.Version);
                if (!UpdateVersion(Environment.MachineName, version))
                {
                    return false;
                }

                _ticket.CurrentVersion = version;
                SystemTrace.Instance.Debug("Updated schema to v{0}".FormatWith(version.ToString()));
            }

            return true;
        }


        /// <summary>
        /// Run updates on existing install.
        /// </summary>
        /// <param name="primaryServer"></param>
        /// <returns></returns>
        private bool PerformDatabaseUpdates(string primaryServer)
        {
            UpdateStatus("Checking for database updates...");

            Dictionary<string, string> databaseHosts = new Dictionary<string, string>();
            databaseHosts.Add("EnterpriseTest", GetDatabaseHostName(primaryServer, "EnterpriseTestDatabase"));
            databaseHosts.Add("DocumentLibrary", GetDatabaseHostName(primaryServer, "DocumentLibraryDatabase"));
            databaseHosts.Add("AssetInventory", GetDatabaseHostName(primaryServer, "AssetInventoryDatabase"));
            databaseHosts.Add("DataLog", GetDatabaseHostName(primaryServer, "DataLogDatabase"));

            List<SqlUpdateScript> scripts = GetScripts();

            // Backup database files before attempting to apply updates.
            BackupDbFiles();

            if (CancelPending())
            {
                return false;
            }

            foreach (SqlUpdateScript script in scripts)
            {
                if (CancelPending())
                {
                    return false;
                }

                string databaseServer = null;
                if (!databaseHosts.TryGetValue(script.Database, out databaseServer))
                {
                    throw new InvalidOperationException("Missing server entry for {0}".FormatWith(script.Database));
                }

                if (!RunDatabaseUpdate(databaseServer, script))
                {
                    return false;
                }
            }

            if (CancelPending()) return false;
            if (scripts.Count > 0)
            {
                Version version = scripts.Max(x => x.Version);
                foreach (var key in databaseHosts.Keys)
                {
                    UpdateStatus("Finalizing update of {0}".FormatWith(key));
                    if (!UpdateVersion(databaseHosts[key], version)) return false;
                }

                _ticket.CurrentVersion = version;
                SystemTrace.Instance.Debug("Updated schema to v{0}".FormatWith(version));
            }
            else
            {
                UpdateStatus("All database are up to date.");
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }

            return true;
        }

        /// <summary>
        /// Creates a file share if one doesn't already exist.
        /// It may also perform some modifications in the file share if needed.
        /// </summary>
        private void CreateFileShare()
        {
            _fileSharePath = _ticket.FileSharePath;

            // Create a share for the system.  It will be used for test documents and other
            // shared resources used by the system.  
            if (SharedDrive.IsShared(_ticket.FileShareName))
            {
                // If there is already a share in place, then find it's path
                _fileSharePath = WindowsManagementInstrumentation
                    .GetFileShares(Environment.MachineName)
                    .First(x => x.Key.Equals(_ticket.FileShareName))
                    .Value;

                SystemTrace.Instance.Debug("Share location: {0}".FormatWith(_fileSharePath));
            }
            else
            {
                try
                {
                    // Create the file share path if it doesn't already exist
                    if (!Directory.Exists(_fileSharePath))
                    {
                        Directory.CreateDirectory(_fileSharePath);
                    }

                    SystemTrace.Instance.Debug("Share will be created on {0}".FormatWith(_fileSharePath));
                    SystemTrace.Instance.Debug("Share name: {0}".FormatWith(_ticket.FileShareName));
                    string desc = "Solution Test Bench System Share";
                    SharedDrive drive = new SharedDrive(_ticket.FileShareName);

                    // Create NT Level access control for the shared directory
                    DirectorySecurity accessControl = Directory.GetAccessControl(_fileSharePath);
                    IdentityReference everybody = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                    FileSystemAccessRule accessRule = new FileSystemAccessRule
                    (
                        everybody,
                        FileSystemRights.FullControl,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow
                    );
                    accessControl.AddAccessRule(accessRule);
                    Directory.SetAccessControl(_fileSharePath, accessControl);

                    // Now create the share, and then add user rights for Everyone.
                    FileSystemRights[] rights = new FileSystemRights[] { FileSystemRights.FullControl };
                    var result = drive.Create(_fileSharePath, desc, "Everyone", rights);
                    if (result == SharedDrive.ShareResult.Success)
                    {
                        // If the share was successfully created, then you need to explicitly 
                        // add user rights to the share as well.
                        drive.SetUserRights("Everyone", rights);
                    }
                    else
                    {
                        SendError($"Unable to create shared folder on {_fileSharePath} : {result}");
                    }
                }
                catch (Exception ex)
                {
                    SendError($"Unable to create shared folder on {_fileSharePath}: {ex.Message}", ex);
                }
            }

            // Read in all the shared folder paths from the resource file.  Create a subdirectory
            // for each of these locations in the shared folder space.
            StringReader reader = new StringReader(new EmbeddedResource("HP.SolutionTest.FileShare").Read("FolderList.txt"));
            string subdirectory = reader.ReadLine();
            while (subdirectory != null)
            {
                string path = Path.Combine(_fileSharePath, subdirectory);
                if (!Directory.Exists(path))
                {
                    SystemTrace.Instance.Debug("Created file share location: {0}".FormatWith(path));
                    Directory.CreateDirectory(path);
                }
                subdirectory = reader.ReadLine();
            }

            // In case of an STB upgrade, if the file share contains a folder named 'TestLibrary\Documents\JPEG',
            // move all its contents to the 'TestLibrary\Documents\Image' folder, and then delete the 'JPEG' folder.
            string jpegPath = Path.Combine(_fileSharePath, @"TestLibrary\Documents\JPEG");
            string imageFilePath = Path.Combine(_fileSharePath, @"TestLibrary\Documents\Image");
            try
            {
                if (Directory.Exists(jpegPath) && Directory.Exists(imageFilePath))
                {
                    foreach (var file in new DirectoryInfo(jpegPath).GetFiles())
                    {
                        file.MoveTo($@"{imageFilePath}\{file.Name}");
                    }
                    SystemTrace.Instance.Debug($"Files in share location \'{jpegPath}\' moved to \'{imageFilePath}\'");
                    Directory.Delete(jpegPath);
                    SystemTrace.Instance.Debug("Deleted file share location: {0}".FormatWith(jpegPath));
                }
            }
            catch (Exception ex)
            {
                SendError($"Unable to rename shared folder on {jpegPath}: {ex.Message}", ex);
            }

            // For any initial test documents provided, save those to the appropriate shared folder location.
            EmbeddedResource resource = new EmbeddedResource("HP.SolutionTest.FileShare.Content");
            foreach (var name in resource.SubNames)
            {
                SystemTrace.Instance.Debug("Saving resource: {0}".FormatWith(name));

                // Convert the sub name into a file path (ie replace '.' with '\')
                string filePath = Path.Combine(_fileSharePath, Path.Combine(name.Split('.')));

                // Fix the last '.' before the file extension that was converted into a '\' above.
                int index = filePath.LastIndexOf('\\');
                filePath = string.Concat(filePath.Select((c, i) => i == index ? '.' : c));

                SystemTrace.Instance.Debug("FilePath: {0}".FormatWith(filePath));

                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    SystemTrace.Instance.Debug("Created file share folder: {0}".FormatWith(directory));
                }

                resource.Save(filePath, name);
            }

        }

        private bool ExecuteSql(string sqlText, SqlTextAdapter adapter, bool useTransaction = true)
        {
            if (CancelPending()) return false;

            string[] sqlCommands = sqlText.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
            int count = 1;
            int length = sqlCommands.Length;
            string statement = string.Empty;

            SendProgressStart(sqlCommands.Length);
            foreach (string command in sqlCommands)
            {
                try
                {
                    SendProgressUpdate(count++);

                    statement = command.Trim();
                    if (!string.IsNullOrEmpty(statement))
                    {
                        if (useTransaction)
                        {
                            adapter.BeginTransaction();
                            adapter.ExecuteSql(statement);
                            adapter.CommitTransaction();
                        }
                        else
                        {
                            adapter.ExecuteSql(statement);
                        }
                    }

                    if (CancelPending()) return false;
                }
                catch (Exception ex)
                {
                    if (useTransaction)
                    {
                        adapter.RollbackTransaction();
                    }

                    SendError(ex);
                    SystemTrace.Instance.Error(statement);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.WriteLine(statement);
                    throw;
                }
            }

            Thread.Sleep(1000);
            SendProgressEnd();

            return true;
        }

        /// <summary>
        /// Drop all database instances that were created during this install.
        /// </summary>
        /// <param name="creationSQL">The SQL used to create the databases.</param>
        /// <param name="adapter">The SQL Text Adapter.</param>
        private void DropDatabaseInstances(IEnumerable<string> databaseNames, SqlTextAdapter adapter)
        {
            try
            {
                foreach (string databaseName in databaseNames)
                {
                    UpdateStatus(string.Format("Killing {0} database session.", databaseName));
                    //UpdateStatus(statement);
                    adapter.ExecuteNonQuery(Resources.UseMaster);
                    adapter.ExecuteSql(Resources.KillDatabaseProcessSql.FormatWith(databaseName));

                    UpdateStatus(string.Format("Dropping {0}.", databaseName));
                    adapter.ExecuteSql(Resources.UseMaster);
                    adapter.ExecuteSql(Resources.CloseConnectionSql.FormatWith(databaseName));
                    adapter.ExecuteSql(Resources.DropDatabaseSql.FormatWith(databaseName));
                }
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.Message);
                Thread.Sleep(5000);
                SendError(ex);
            }
        }

        private IEnumerable<string> GetSTBDatabaseNames()
        {
            EmbeddedResource stream = new EmbeddedResource("HP.SolutionTest.DatabaseCreation");
            string creationSql = stream.Read("create_DatabaseInstances.sql");
            return GetSTBDatabaseNames(ref creationSql);
        }

        private IEnumerable<string> GetSTBDatabaseNames(ref string creationSql)
        {
            List<string> result = new List<string>();

            string pattern = @"\[(.*?)\]";     //Get database names within braces
            MatchCollection databaseNames = Regex.Matches(creationSql, pattern);

            foreach (string databaseName in databaseNames.OfType<Match>().Select(m => m.Value).Distinct())
            {
                result.Add(databaseName.Substring(1, databaseName.Length - 2)); //Shave off the braces.
            }

            return result;
        }

        private void RollBackSTBInstall()
        {
            using (SqlTextAdapter adapter = new SqlTextAdapter(Environment.MachineName, "Master"))
            {
                DropDatabaseInstances(GetSTBDatabaseNames(), adapter);
            }

            CleanUpFileShare();
        }

        /// <summary>
        /// Restores backed up database files from the backup location.  Cleans up the backups.
        /// </summary>
        private void RollBackSTBUpdate()
        {
            string backupDirectory = Path.Combine(_ticket.DatabaseFilesPath, BackupFolderName);

            if (Directory.Exists(backupDirectory))
            {
                try
                {
                    SqlServerService(SqlServerServiceOperation.Stop);

                    foreach (string databaseName in GetSTBDatabaseNames())
                    {
                        UpdateStatus("Restoring {0}...".FormatWith(databaseName));
                        CopyDbFiles(databaseName, backupDirectory, _ticket.DatabaseFilesPath, true);
                    }

                    Directory.Delete(backupDirectory);
                }
                finally
                {
                    SqlServerService(SqlServerServiceOperation.Start);
                }
            }
        }

        private static void SqlServerService(SqlServerServiceOperation operation)
        {
            string command = operation == SqlServerServiceOperation.Start ? "/C net start MSSQLSERVER" : " /C net stop MSSQLSERVER";

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;   //hide window
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = command;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Creates a backup of STB database files to a temp location.
        /// </summary>
        private void BackupDbFiles()
        {
            try
            {
                SqlServerService(SqlServerServiceOperation.Stop);
                string backupDirectory = Path.Combine(_ticket.DatabaseFilesPath, BackupFolderName);

                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                foreach (string databaseName in GetSTBDatabaseNames())
                {
                    UpdateStatus("Backing up {0}...".FormatWith(databaseName));
                    CopyDbFiles(databaseName, _ticket.DatabaseFilesPath, backupDirectory);
                }
            }
            finally
            {
                SqlServerService(SqlServerServiceOperation.Start);
            }
        }

        private void CopyDbFiles(string databaseName, string sourceDirectory, string destinationDirectory, bool cleanUpSourceFiles = false)
        {
            StringBuilder sourcePath = new StringBuilder(sourceDirectory);
            StringBuilder destinationPath = new StringBuilder(destinationDirectory);

            // Copy database file
            sourcePath.Append("\\").Append(databaseName).Append(".mdf");
            destinationPath.Append("\\").Append(databaseName).Append(".mdf");

            File.Copy(sourcePath.ToString(), destinationPath.ToString(), true);

            // Copy database log file
            sourcePath.Replace('m', 'l', sourcePath.Length - 4, 1);
            destinationPath.Replace('m', 'l', sourcePath.Length - 4, 1);

            File.Copy(sourcePath.ToString(), destinationPath.ToString(), true);

            if (cleanUpSourceFiles)
            {
                File.Delete(sourcePath.ToString());
            }
        }

        /// <summary>
        /// Removes the file share created by the installer
        /// </summary>
        private void CleanUpFileShare()
        {
            if (Directory.Exists(_fileSharePath))
            {
                if (SharedDrive.IsShared(_ticket.FileShareName))
                {
                    SharedDrive drive = SharedDrive.GetNamedShare(_ticket.FileShareName);
                    drive.Delete();
                    Directory.Delete(_ticket.FileSharePath, true);
                }
            }
        }

        private enum SqlServerServiceOperation
        {
            Start,
            Stop
        }
    }
}
