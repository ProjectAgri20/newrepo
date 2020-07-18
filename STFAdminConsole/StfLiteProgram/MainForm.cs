using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using HP.ScalableTest;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.LabConsole;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.Reporting;
using HP.ScalableTest.UI.ScenarioConfiguration;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.LabConsole.ResourceConfiguration;
using Telerik.WinControls.UI;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Core.Configuration;


namespace HP.SolutionTest.WorkBench
{
    internal partial class MainForm : Form
    {
        private EnterpriseTestUIController _enterpriseTestUIController;
        private SessionExecutionManager _sessionManager;
        private ConfigurationTagEventArgs _refreshEvent = new ConfigurationTagEventArgs(ConfigurationObjectTag.Create(VirtualResourceType.None));

        private Guid? _currentlyDisplayedItemId;

        static MainForm()
        {
            HP.ScalableTest.Framework.UI.UserInterfaceStyler.Initialize();
        }

        public MainForm()
        {
            InitializeComponent();
            SetDialogMenuItemHandlers();
            AddDocumentationToHelpMenu();
            version_StatusLabel.Text = "Version " + AssemblyProperties.Version;

            // Set up Scenario Configuration tab
            _enterpriseTestUIController = new EnterpriseTestUIController();
            _enterpriseTestUIController.NodeRemoving += _enterpriseTestUIController_NodeRemoving;
            scenarioConfigurationTreeView.Initialize(_enterpriseTestUIController);
            resource_ToolStrip.Visible = false;

            // Set up Scenario Execution tab
            _sessionManager = connectedSessionsControl.SessionManager;
            _sessionManager.PropertyChanged += _sessionManager_PropertyChanged;

            // Subscribe to dispatcher updates
            SessionClient.Instance.DispatcherExceptionReceived += new EventHandler<ExceptionDetailEventArgs>(DispatcherErrorReceived);

            // Subscribe to this event so that the connection dialog will pop up when the app is finished loading
            Application.Idle += new EventHandler(OnLoaded);

            // Subscribe to events that occur when the connection to a dispatcher changes.
            STFDispatcherManager.DispatcherChanged += STFDispatcherManager_DispatcherChanged;

            scenarioConfigurationTreeView.OnStartBatch += ScenarioConfigurationTreeView_OnStartBatch;

        }

        private void _sessionManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName.ToLower())
            {
                case "readytostart":
                    executeScenario_ToolStripButton.Enabled = _sessionManager.AbleToStartNewSession;
                    break;

                default:
                    break;
            }
        }

        private void DispatcherErrorReceived(object sender, ExceptionDetailEventArgs e)
        {
            ApplicationExceptionHandler.UnhandledException(e.Message, e.Detail, UserManager.CurrentUserName);
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(OnLoaded);

            try
            {
                if (!ConnectToEnvironment())
                {
                    Application.Exit();
                }

                logIn_MenuItem.Visible = STFLoginManager.MultipleEnvironmentsAvailable();
            }
            catch (Exception ex)
            {
                // Inform the user of the error and allow them to send but do not kill the app unless they choose
                // For example, they may instead want to change environment.
                ApplicationExceptionHandler.UnhandledException(ex);
            }
        }

        private bool ConnectToEnvironment()
        {
            if (!STFLoginManager.Login())
            {
                return false; //User canceled the login
            }

            Cursor.Current = Cursors.WaitCursor;

            // Update status labels
            main_StatusLabel.Text = "Connecting to {0}".FormatWith(STFLoginManager.SystemDatabase);
            userName_StatusLabel.Text = UserManager.CurrentUserName;
            environment_StatusLabel.Text = "{0} {1} Environment".FormatWith(GlobalSettings.Items[Setting.Organization], GlobalSettings.Items[Setting.Environment]);
            SetLoggingContext();

            // DoEvents so the screen will refresh, and then set the wait cursor
            Application.DoEvents();

            try
            {
                // Initialize framework services
                FrameworkServicesInitializer.InitializeConfiguration();

                // Populate the config tree view
                InitializeConfigurationTreeView();

                // Apply user security settings
                ApplySecurity();

                // Refresh the status label and return true to indicate success
                main_StatusLabel.Text = "Connected to {0} ({1})".FormatWith(GlobalSettings.Environment, GlobalSettings.Database);
                return true;
            }
            catch (EnvironmentConnectionException ex)
            {
                MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private static void SetLoggingContext()
        {
            SetLoggingContext(null);
        }

        private static void SetLoggingContext(FrameworkServer dispatcher)
        {
            try
            {
                // Set the logging context
                if (dispatcher != null)
                {
                    TraceFactory.SetThreadContextProperty("Dispatcher", dispatcher.HostName, false);
                }
                string environment = GlobalSettings.Items[Setting.Environment];
                TraceFactory.SetThreadContextProperty("Environment", environment, false);
                TraceFactory.Logger.Debug("Set logging context");
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to set logging context", ex);
            }
        }

        private void InitializeConfigurationTreeView()
        {
            try
            {
                ReloadTree();

                // Apply system settings to the configuration tree view
                bool allowRootScenarios = bool.Parse(GlobalSettings.Items[Setting.AllowRootScenarioCreation]);
                scenarioConfigurationTreeView.AllowRootScenarioCreation = allowRootScenarios;
                newScenarioAtRoot_ToolStripMenuItem.Visible = allowRootScenarios;
            }
            catch (EntityCommandExecutionException ex)
            {
                TraceFactory.Logger.Error(ex);
                throw new EnvironmentConnectionException(
                    "The selected database schema is incompatible with this version of the software.", ex);
            }
        }

        private void ReloadTree()
        {
            try
            {
                scenarioConfigurationTreeView.Clear();
                dataEditPanel.Controls.Clear();

                Cursor = Cursors.WaitCursor;
                scenarioConfigurationTreeView.BeginUpdate();
                _enterpriseTestUIController.Load();
                scenarioConfigurationTreeView.EndUpdate();
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to Load Tree");
                ConfigurationServices.SystemTrace.LogError(e);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void STFDispatcherManager_DispatcherChanged(object sender, EventArgs e)
        {
            if (STFDispatcherManager.Dispatcher != null)
            {
                main_StatusLabel.Text = "Connected to {0}".FormatWith(STFDispatcherManager.Dispatcher.HostName);
            }
            else
            {
                main_StatusLabel.Text = "Disconnected from dispatcher";
            }
            SetLoggingContext(STFDispatcherManager.Dispatcher);
        }

        private void ApplySecurity()
        {
            bool isAdmin = UserManager.CurrentUser.HasPrivilege(UserRole.Administrator);
            bool isUser = UserManager.CurrentUser.HasPrivilege(UserRole.User);

            scenarioConfiguration_SplitContainer.Enabled = isUser;
            manageSessionData_MenuItem.Visible = isUser;
            configuration_MenuItem.Visible = isUser;
            administration_MenuItem.Visible = isAdmin;
            monitorConfig_MenuItem.Visible = isAdmin;
            toolStripSeparator1.Visible = isAdmin;

            ApplySettings();
        }

        /// <summary>
        /// Turn on certain features if 'EnterpriseEnabled' system setting is present and turned on
        /// </summary>
        private void ApplySettings()
        {
            try
            {
                if (GlobalSettings.Items["EnterpriseEnabled"].Equals("True", StringComparison.InvariantCultureIgnoreCase))
                {
                    badgeBoxes_MenuItem.Visible = true;
                    cameras_MenuItem.Visible = true;
                    printDrivers_MenuItem.Visible = true;
                    printDriverConfigurations_MenuItem.Visible = true;
                    installers_Separator.Visible = true;
                    installerPackages_MenuItem.Visible = true;
                    softwareInstallers_MenuItem.Visible = true;
                }
            }
            catch { } //If no setting exists, continue
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool closeForm = true;

            ///  Check if there is a running session.  
            /// If so, ask the user if they want to stop the session (and do it for them if affirmative)
            /// If they do not want to stop the session then they don't really want to exit the app            
            try
            {
                switch (e.CloseReason)
                {
                    case CloseReason.UserClosing:
                        if (_sessionManager != null && _sessionManager.GetActiveSessions().Any())
                        {
                            var response = MessageBox.Show("A test execution session is currently running - do you want to stop the session and exit?",
                                            "Attempting Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            closeForm = (response == DialogResult.Yes);

                            if (closeForm)
                            {
                                var session = _sessionManager.GetActiveSessions().FirstOrDefault();
                                var sessionId = session.SessionId;
                                if (!string.IsNullOrEmpty(sessionId))
                                {
                                    using (SessionShutdownForm form = new SessionShutdownForm(sessionId))
                                    {
                                        if (form.ShowDialog(this) == DialogResult.OK)
                                        {
                                            using (new BusyCursor())
                                            {
                                                // start shut down of session
                                                _sessionManager.Shutdown(sessionId, form.ShutdownOptions);

                                                // wait for shut down or 1 minute to complete
                                                Stopwatch s = Stopwatch.StartNew();
                                                TimeSpan maxWait = TimeSpan.FromSeconds(60);
                                                int count = 0;
                                                while (session.State != SessionState.ShutdownComplete && s.Elapsed < maxWait)
                                                {
                                                    System.Threading.Thread.Sleep(500);
                                                    main_StatusLabel.Text = "Exiting in {0} seconds (or less)..{1}".FormatWith((maxWait - s.Elapsed).Seconds, new string('.', count % 3));
                                                    Application.DoEvents();
                                                }
                                                s.Stop();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            finally
            {
                e.Cancel = !closeForm;
                if (closeForm)
                {
                    try
                    {
                        SessionClient.Instance.Stop();
                    }
                    catch { }
                }
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainTabControl.SelectedTab == reports_TabPage)
            {
                graphingUserControl.RefreshScenarioList();
            }
        }

        #region Scenario Configuration Tab

        private void scenarioConfigurationTreeView_ConfigurationObjectSelectionChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if (dataEditPanel.Controls.Count <= 0)
            {
                return;
            }

            var control = dataEditPanel.Controls[0] as IScenarioConfigurationControl;

            var metadataEditor = control as WorkerActivityMetadataControl;
            if (metadataEditor != null)
            {
                // This control tracks its own unsaved changes
                if (metadataEditor.HasUnsavedChanges)
                {
                    DialogResult result = MessageBox.Show("You have unsaved changes.  Would you like to save?",
                        "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        bool saveResult = SaveChanges();
                        if (saveResult)
                        {
                            metadataEditor.HasUnsavedChanges = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            else
            {
                control.FinalizeEdit();

                if (_enterpriseTestUIController.HasAddedOrRemovedObjects)
                {
                    DialogResult result = MessageBox.Show("One or more configuration objects have been added or removed. " +
                        "If these changes are not saved before navigating away, they will be discarded.\n\n" +
                        "Click Yes to return to the previous screen so you can save your changes, or click No to discard the changes.",
                        "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }

                // If there are unsaved edits, prompt user to save before navigating away
                else if (_enterpriseTestUIController.HasUnsavedChanges)
                {
                    DialogResult result = MessageBox.Show("You have unsaved changes.  Would you like to save?",
                        "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        bool saveResult = SaveChanges();
                        if (saveResult == false)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }


        private void scenarioConfigurationTreeView_ConfigurationObjectSelected(object sender, ConfigurationTagEventArgs e)
        {
            _refreshEvent = e;
            this.Cursor = Cursors.WaitCursor;
            resource_ToolStrip.Visible = false;
            main_StatusLabel.Text = string.Empty;
            dataEditPanel.SuspendLayout();

            try
            {
                // Remove all controls from the data edit panel
                ClearEditPanel();

                // Load the entity object from the database
                EntityObject entity = _enterpriseTestUIController.GetEntityObject(e.Tag.Id);
                if (entity == null)
                {
                    // This will be caught by the EnterpriseScenarioTreeView as a result of the ConfigurationObjectSelected event.
                    throw new InvalidOperationException("Object does not exist.");
                }
                _currentlyDisplayedItemId = e.Tag.Id;
                try
                {
                    // Create the configuration control and add to the panel
                    IScenarioConfigurationControl control = ScenarioConfigurationControlFactory.Create(e.Tag, _enterpriseTestUIController.Context);

                    if (control != null)
                    {
                        control.Initialize(entity);
                        dataEditPanel.Controls.Add(control as Control);
                        (control as Control).Dock = DockStyle.Fill;

                        // Set the title of this edit form 
                        resource_ToolStripLabel.Text = control.EditFormTitle;

                        // Configure and show the tool bar
                        resource_ToolStrip.Visible = !e.Tag.ObjectType.IsFolder();
                        executeScenario_ToolStripButton.Visible = e.Tag.ObjectType == ConfigurationObjectType.EnterpriseScenario; //(scenarioConfigurationTreeView.SelectedScenarioConfigurationObject != null);
                        executeScenario_ToolStripButton.Enabled = _sessionManager.AbleToStartNewSession;
                    }
                }
                catch (ControlTypeMismatchException ex)
                {
                    MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                dataEditPanel.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
        }

        private void _enterpriseTestUIController_NodeRemoving(object sender, EnterpriseTestUIEventArgs e)
        {
            // Check to see if we are deleting the currently displayed
            if (e.Id == _currentlyDisplayedItemId)
            {
                ClearEditPanel();
                _enterpriseTestUIController.DiscardChanges();
            }
        }

        private void ClearEditPanel()
        {
            while (dataEditPanel.Controls.Count > 0)
            {
                Control oldControl = dataEditPanel.Controls[0];
                dataEditPanel.Controls.Remove(oldControl);
                oldControl.Dispose();
            }
        }


        private void newItem_DropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedConfigurationObject;
            if (selected != null)
            {
                newFolder_ToolStripMenuItem.Enabled = _enterpriseTestUIController.CanContainFolder(selected.Id);
                newScenario_ToolStripMenuItem.Enabled = _enterpriseTestUIController.CanContainScenario(selected.Id);
            }
            else
            {
                newFolder_ToolStripMenuItem.Enabled = false;
                newScenario_ToolStripMenuItem.Enabled = false;
            }
        }

        private void newFolder_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedConfigurationObject;
            if (selected != null && _enterpriseTestUIController.CanContainFolder(selected.Id))
            {
                Guid folderId = _enterpriseTestUIController.CreateFolder(selected.Id);
                scenarioConfigurationTreeView.SelectNode(folderId);
            }
        }

        private void newScenario_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedConfigurationObject;
            if (selected != null && _enterpriseTestUIController.CanContainScenario(selected.Id))
            {
                Guid scenarioId = _enterpriseTestUIController.CreateScenario(selected.Id);
                scenarioConfigurationTreeView.SelectNode(scenarioId);
            }
        }

        private void newFolderAtRoot_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guid folderId = _enterpriseTestUIController.CreateFolder(null);
            scenarioConfigurationTreeView.SelectNode(folderId);
        }

        private void newScenarioAtRoot_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guid scenarioId = _enterpriseTestUIController.CreateScenario(null);
            scenarioConfigurationTreeView.SelectNode(scenarioId);
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (scenarioConfigurationTreeView.NumberOfSelectedTrees() > 1)
            {
                MessageBox.Show("Cannot Delete Multiple selections at the same time.",
                                "Multi-Selection Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedConfigurationObject;
            if (selected != null)
            {
                // Pop a dialog to confirm this with the user
                DialogResult result = MessageBox.Show
                    ("Are you sure you want to delete '{0}'?".FormatWith(scenarioConfigurationTreeView.SelectedNode.Text),
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _enterpriseTestUIController.Delete(selected.Id);
                    scenarioConfigurationTreeView.ClearMCSelections();
                }
            }
        }

        private void configurationTreeRefresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------------------------------------------------
            //ADD: There is a problem with the Refresh in that it rebuilds the tree view, but does not clear the
            // dataEditPanel (or remember which node was selected and restore it to the dataEditPanel).  So, if
            // the user selects a scenario, clicks Refresh and the clicks Save from the toolstrip menu - the Save
            // will generate a Null Object error. In order to prevent this, grab the selected item(s). Multiple
            // selected items are not supported here, yet.  However, they may be soon - so just go ahead and use
            // an IEnumerable, but only operate on the first one.
            //---------------------------------------------------------------------------------------------------
            var selected = scenarioConfigurationTreeView.FindNodes(n => n.Selected).Select(n => n.Tag as Guid?).ToList();
            var expanded = scenarioConfigurationTreeView.FindNodes(n => n.Expanded).Select(n => n.Tag as Guid?).ToList();
            ReloadTree();
            scenarioConfigurationTreeView.ExpandNodes(expanded);

            //---------------------------------------------------------------------------------------------------
            //ADD: If something was selected when we did the refresh, go ahead and re-select it.  This should 
            // handle keeping the dataEditPanel properly updated to the new node.
            //---------------------------------------------------------------------------------------------------
            if (selected.Count > 0)
            {
                RadTreeNode[] reSelect = scenarioConfigurationTreeView.FindNodes(n => n.Tag as Guid? == (Guid?)selected[0]);
                if (reSelect.Length > 0)
                {
                    scenarioConfigurationTreeView.SelectedNode = reSelect[0];
                    if (_refreshEvent.Tag.ObjectType != ConfigurationObjectType.Unknown)
                    {
                        scenarioConfigurationTreeView_ConfigurationObjectSelected(null, _refreshEvent);
                    }
                }
            }
        }

        private void save_ToolStripButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            var metadataEditor = dataEditPanel.Controls[0] as WorkerActivityMetadataControl;
            if (metadataEditor != null)
            {
                metadataEditor.HasUnsavedChanges = false;
            }
        }

        private bool SaveChanges()
        {
            if (dataEditPanel.Controls.Count > 0)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var editControl = dataEditPanel.Controls[0] as IScenarioConfigurationControl;

                    // The metadata control works differently than everything else (for now).
                    // This check is required to make sure that the validation is called before the configuration is retrieved,
                    // but only for metadata controls.
                    bool lateFinalize = editControl is WorkerActivityMetadataControl;
                    if (!lateFinalize)
                    {
                        editControl.FinalizeEdit();
                    }

                    ValidationResult result = editControl.Validate();
                    if (result.Succeeded)
                    {
                        if (lateFinalize)
                        {
                            editControl.FinalizeEdit();
                        }
                        bool success = _enterpriseTestUIController.Commit();
                        if (success && scenarioConfigurationTreeView.SelectedNode != null)
                        {
                            main_StatusLabel.Text = scenarioConfigurationTreeView.SelectedNode.Text + " was saved successfully.";
                        }
                        else
                        {
                            MessageBox.Show("One or more of the affected objects was modified by another user, so the operation you attempted might not have "
                                          + "had the intended result.  Please reload the tree view and the object you were editing to verify the final state.",
                                            "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("The following errors must be corrected before changes can be saved:\n" + result.Message,
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            return true;
        }

        private void analyze_ToolStripButton_Click(object sender, EventArgs e)
        {
            var control = dataEditPanel.Controls[0] as EnterpriseScenarioControl;
            control.DisplayPlatformUsage();
        }

        private void executeScenario_ToolStripButton_Click(object sender, EventArgs e)
        {

                ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedScenarioConfigurationObject;
                if (selected != null)
                {
                    mainTabControl.SelectTab(execution_TabPage);
                    StartSession(new List<Guid>() { selected.Id });
                }
            
        }

        private void ScenarioConfigurationTreeView_OnStartBatch(object sender, RadTreeViewEventArgs e)
        {

            ConfigurationObjectTag selected = scenarioConfigurationTreeView.SelectedConfigurationObject;
            if (selected != null && selected.ObjectType == ConfigurationObjectType.ScenarioFolder)
            {
                mainTabControl.SelectTab(execution_TabPage);
                List<Guid> scenarioIds = null;
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    scenarioIds = (List<Guid>)ConfigurationTreeFolder.ContainedScenarioIds(context, selected.Id);
                }
                StartSession(scenarioIds);
            }
            
        }

        #endregion

        private void StartSession(List<Guid> scenarioIds)
        {
            try
            {
                SessionConfigurationWizard wizard = new SessionConfigurationWizard(scenarioIds);
                _sessionManager.StartSession(wizard, wizard.Ticket);
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.UnhandledException(ex);
            }
        }

        #region Menu Handlers

        private void ShowDialog<T>(object sender, EventArgs e) where T : Form, new()
        {
            try
            {
                using (T form = new T())
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.UnhandledException(ex);
            }
        }

        private void ShowPrintServerDialog(object sender, EventArgs e)
        {
            ServerType serverType;
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            if (!Enum.TryParse<ServerType>(menuItem.Tag.ToString(), out serverType))
            {
                serverType = ServerType.Print;
            }

            using (PrintServerManagementForm form = new PrintServerManagementForm())
            {
                form.ServerType = serverType;
                form.ShowDialog(this);
            }
        }

        private void SetDialogMenuItemHandlers()
        {
            //importScenarioToolStripMenuItem.Click += ShowDialog<ScenarioImportWizardForm>;

            // Session menu
            manageSessionData_MenuItem.Click += ShowDialog<SessionDataForm>;
            sessionReports_MenuItem.Click += ShowSessionReports;
            //sessionLogsToolStripMenuItem.Click += ShowDialog<SessionLogForm>;

            userGroup_MenuItem.Click += ShowDialog<UserGroupsConfigForm>;

            // Configuration menu
            serverInventory_MenuItem.Click += ShowDialog<FrameworkServerListForm>;
            printServers_MenuItem.Click += ShowPrintServerDialog;
            badgeBoxes_MenuItem.Click += ShowDialog<BadgeBoxListForm>;
            cameras_MenuItem.Click += ShowDialog<CameraListForm>;
            printDrivers_MenuItem.Click += ShowDialog<PrintDriverConfigForm>;
            printDriverConfigurations_MenuItem.Click += ShowDialog<PrintDriverCfmManagementForm>;
            printDeviceInventoryMenuItem.Click += ShowDialog<PrinterListForm>;
            monitorConfig_MenuItem.Click += ShowDialog<MonitorConfigForm>;
            resourceLists_MenuItem.Click += ShowDialog<ResourceWindowsCategoryAddForm>;
            testDocumentLibraryMenuItem.Click += ShowDialog<DocumentListForm>;

            // Administration menu
            userManagement_MenuItem.Click += ShowDialog<UserManagementListForm>;
            pluginReferences_MenuItem.Click += ShowDialog<PluginMetadataListForm>;
            installerPackages_MenuItem.Click += ShowDialog<InstallerPackagesForm>;
            softwareInstallers_MenuItem.Click += ShowDialog<SoftwareInstallersForm>;
            //lockService_MenuItem.Click += ShowDialog<LockViewForm>;

            // Help menu
            about_MenuItem.Click += ShowDialog<AboutBox>;
        }

        private void exit_MenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void assetUsage_MenuItem_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm form = new AssetSelectionForm(AssetAttributes.None))
            {
                form.MultiSelect = true;
                form.SelectButtonVisible = false;
                form.ShowDialog(this);
            }
        }

        private void logIn_MenuItem_Click(object sender, EventArgs e)
        {
            ConnectToEnvironment();
            //System.Diagnostics.Debug.WriteLine(STFLoginManager.Dispatcher.HostName);
        }

        #endregion

        private void importScenarioMenuItem_Click(object sender, EventArgs e)
        {
            using (ScenarioImportWizardForm form = new ScenarioImportWizardForm())
            {
                form.ShowDialog();

                if (form.ScenarioImported)
                {
                    RefreshAfterImport();
                }
            }
        }

        private void RefreshAfterImport()
        {
            // Refresh the tree control so the new scenario shows up
            var expanded = scenarioConfigurationTreeView.FindNodes(n => n.Expanded).Select(n => n.Tag as Guid?).ToList();
            ReloadTree();
            scenarioConfigurationTreeView.ExpandNodes(expanded);
        }

        private void ShowSessionReports(object sender, EventArgs e)
        {
            try
            {
                var form = new STFReportGenerationForm();
                form.StartPosition = FormStartPosition.CenterParent;
                form.Show((IWin32Window)this);
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.UnhandledException(ex);
            }
        }

        private void userAccountPoolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new DomainAccountListForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }

        private void systemSettings_MenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new GlobalSettingsListForm(SettingType.SystemSetting))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }

        private void activityPluginSettingsMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new GlobalSettingsListForm(SettingType.PluginSetting))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }

        private void scenarioConfigurationTreeView_ImportCompleted(object sender, EventArgs e)
        {
            RefreshAfterImport();
        }

        /// <summary>
        /// Adds documentation menu items to the Help menu if the Documentation folder exists in the main STB install folder.
        /// If the Documentation folder does not exist for some reason, we won't worry about it.
        /// </summary>
        private void AddDocumentationToHelpMenu()
        {
            StringBuilder documentationPath = new StringBuilder();

            documentationPath.Append(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\")));
            documentationPath.Append("\\Documentation");
            string folderPath = documentationPath.ToString();

            if (Directory.Exists(folderPath))
            {
                string[] documentationFiles = Directory.GetFiles(folderPath, "*.pdf");

                int fileCount = 0;
                if (documentationFiles.Length > 0)
                {
                    foreach (string filePath in documentationFiles)
                    {
                        string menuText = Path.GetFileNameWithoutExtension(filePath);
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(menuText);
                        menuItem.Tag = filePath;
                        menuItem.Click += DisplayDocFile;

                        help_MenuItem.DropDownItems.Insert(fileCount, menuItem);
                        fileCount++;
                    }
                }

                if (fileCount > 0)
                {
                    ToolStripSeparator helpMenuSeparator = new ToolStripSeparator();
                    helpMenuSeparator.Name = "helpMenuSeparator1";
                    helpMenuSeparator.Size = new System.Drawing.Size(215, 6);
                    helpMenuSeparator.Visible = true;
                    help_MenuItem.DropDownItems.Insert(fileCount, helpMenuSeparator);
                }
            }
        }

        private void DisplayDocFile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string filePath = (string)menuItem.Tag;
            Process.Start(filePath);
        }
    }
}
