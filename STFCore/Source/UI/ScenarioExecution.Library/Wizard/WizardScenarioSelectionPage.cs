using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// Start page for selecting a single scenario to run.  Contains additional settings than the Batch startup page.
    /// </summary>
    public partial class WizardScenarioSelectionPage : UserControl, IWizardPage
    {
        private ErrorProvider _errorProvider = null;
        private bool _initial;
        private IEnumerable<ScenarioProduct> _scenarioProducts;
        private TabPage _debugTabPage;
        private TextBox _debugTextBox;
        private string _newCellValue;
        private EnterpriseScenario _scenario = null;

        private SessionTicket Ticket { get; set; }

        /// <summary>
        /// Notification to cancel the wizard.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Creates a new instance of <see cref="WizardScenarioSelectionPage"/>
        /// </summary>
        public WizardScenarioSelectionPage()
        {
            InitializeComponent();
            InitializeErrorProvider();
            _initial = true;
        }

        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public virtual bool Initialize(WizardConfiguration configuration)
        {
            Ticket = configuration.Ticket;
            Guid scenarioId = Ticket.ScenarioIds.FirstOrDefault();

            if (STFDispatcherManager.Dispatcher == null && STFDispatcherManager.ConnectToDispatcher() == false)
            {
                //The user canceled the connect dialog
                return false;
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Load the specified scenario (if there is one)
                if (scenarioId != Guid.Empty)
                {
                    LoadScenario(scenarioId, context);
                    LoadComboBoxes(context);
                }
            }

            // If this is not the first time we have entered this step, the user
            // must have come back from some point later in the wizard.
            // Make sure the dispatcher doesn't hang onto configuration from the previous try.
            if (!_initial)
            {
                SessionClient.Instance.Close(Ticket.SessionId);
            }

            if (_scenario != null && Ticket.SessionId != null && Ticket.AssociatedProductList.Count != 0)
            {
                foreach (var item in Ticket.AssociatedProductList)
                {
                    _scenarioProducts.Where(n => n.ProductId == item.AssociatedProductId && n.Vendor == item.Vendor && n.Version == item.Version).FirstOrDefault().Active = item.Active;
                }
                scenarioProductBindingSource.Clear();
                scenarioProductBindingSource.DataSource = _scenarioProducts;
                scenarioProductBindingSource.ResetBindings(true);
            }
            deviceOffline_CheckBox.Checked = Ticket.RemoveUnresponsiveDevices;

            environment_Label.Text = "{0} {1} Environment".FormatWith(GlobalSettings.Items[Setting.Organization], GlobalSettings.Items[Setting.Environment]);

            if (string.IsNullOrEmpty(logLocation_TextBox.Text))
            {
                logLocation_TextBox.Text = GlobalSettings.WcfHosts[WcfService.DataLog];
            }
            _initial = false;

            dispatcher_Label.Text = STFDispatcherManager.Dispatcher.HostName;

            if (!GlobalSettings.IsDistributedSystem)
            {
                this.settings_TabControl.TabPages.Remove(this.virtualMachineSelection_TabPage);
            }

            return true;
        }

        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>
        /// True if this page was successfully validated.
        /// </returns>
        public virtual bool Complete()
        {
            Guid scenarioId = Ticket.ScenarioIds.FirstOrDefault();

            if (! ValidateInput(scenarioId))
            {
                return false;
            }

            // STE-only
            if (GlobalSettings.IsDistributedSystem)
            {
                PopulateSelectedVMs();
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Perform a data integrity check on the scenario
                if (PerformDataIntegrityCheck(context, scenarioId) == false)
                {
                    Cancel?.Invoke(this, EventArgs.Empty);
                    return false;
                }

                PopulateNotificationSettings();

                SetAssociatedProducts();

                // Populate ticket data from the UI
                Ticket.CollectEventLogs = eventLog_CheckBox.Checked;
                Ticket.SessionName = string.IsNullOrEmpty(sessionName_ComboBox.Text) ? selectedScenario_TextBox.Text : sessionName_ComboBox.Text;
                Ticket.SessionType = sessionType_ComboBox.Text;
                Ticket.SessionCycle = sessionCycle_ComboBox.Text;
                Ticket.Reference = WizardPageManager.GetReferenceData(reference_TextBox);
                Ticket.SessionNotes = notes_TextBox.Text;
                Ticket.DurationHours = (int)runtime_NumericUpDown.Value;
                SessionLogRetention selected = EnumUtil.GetByDescription<SessionLogRetention>((string)retention_ComboBox.SelectedItem);
                Ticket.ExpirationDate = selected.GetExpirationDate(DateTime.Now);
                Ticket.LogLocation = logLocation_TextBox.Text;
                Ticket.RemoveUnresponsiveDevices = deviceOffline_CheckBox.Checked;

                // Save session name and selected VMs
                SaveSessionName(context, scenarioId);
                context.SaveChanges();
            }

            // Save selected scenario to settings so it can be selected next time
            Properties.Settings.Default.LastExecutedScenario = scenarioId;
            Properties.Settings.Default.Save();

            // Initiate the session with the dispatcher
            TraceFactory.Logger.Debug("Calling Initiate() on {0}".FormatWith(Ticket.SessionId));
            SessionClient.Instance.InitiateSession(Ticket);

            return true;
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // STF-only
            if (GlobalSettings.IsDistributedSystem)
            {
                if (dispatcher_Label != null)
                {
                    _errorProvider.SetIconAlignment(dispatcher_Label, ErrorIconAlignment.MiddleLeft);
                }
            }

            _errorProvider.SetIconAlignment(selectedScenario_TextBox, ErrorIconAlignment.MiddleLeft);
            _errorProvider.SetIconAlignment(sessionName_ComboBox, ErrorIconAlignment.MiddleLeft);

        }

        private void scenarioSelection_Button_Click(object sender, EventArgs e)
        {
            using (ScenarioSelectionForm selectionForm = new ScenarioSelectionForm())
            {
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        LoadScenario(selectionForm.SelectedScenarioId, context);
                    }
                }
            }
        }

        private void LoadScenario(Guid scenarioId, EnterpriseTestContext context)
        {
            EnterpriseScenario scenario = EnterpriseScenario.Select(context, scenarioId);

            if (scenario != null)
            {
                Ticket.ScenarioIds = new List<Guid>() { scenarioId };
                _scenario = scenario;
                selectedScenario_TextBox.Text = scenario.Name;
                
                runtime_NumericUpDown.Value = Math.Min(scenario.EstimatedRuntime, runtime_NumericUpDown.Maximum); //Ensure we don't exceed the NumericUpDown.Maximum.

                //Populate Associated Products
                _scenarioProducts = WizardPageManager.GetAssociatedProducts(context, scenario);
                scenarioProductBindingSource.DataSource = _scenarioProducts;
                scenarioProductBindingSource.ResetBindings(true);
            }

        }

        private bool PerformDataIntegrityCheck(EnterpriseTestContext context, Guid scenarioId)
        {
            EnterpriseScenario scenario = EnterpriseScenario.Select(context, scenarioId);

            if (platform_RadioButton.Checked)
            {
                return WizardPageManager.PerformScenarioIntegrityCheck(scenario, (FrameworkClientPlatform)platform_ComboBox.SelectedItem);
            }
            else if (holdId_RadioButton.Checked)
            {
                return WizardPageManager.PerformScenarioIntegrityCheck(scenario, (string)holdId_ComboBox.SelectedItem);
            }
            else
            {
                return WizardPageManager.PerformScenarioIntegrityCheck(scenario);
            }
        }

        private bool SetAssociatedProducts()
        {
            Ticket.AssociatedProductList.Clear();

            try
            {
                foreach (ScenarioProduct assocProduct in _scenarioProducts)
                {
                    var item = new AssociatedProductSerializable()
                    {
                        AssociatedProductId = assocProduct.ProductId,
                        Vendor = assocProduct.Vendor,
                        Name = assocProduct.Name,
                        Version = assocProduct.Version,
                        Active = assocProduct.Active
                    };
                    Ticket.AssociatedProductList.Add(item);
                }

                //Clear any previous error
                _errorProvider.SetError(associatedProducts_TabPage, string.Empty);
                return true;
            }
            catch (Exception ex)
            {
                UpdateDebug(ex.ToString());
                TraceFactory.Logger.Error("Error loading Associated Products.", ex);
                _errorProvider.SetError(settings_TabControl, ex.Message);
                return false;
            }
        }

        private void SaveSessionName(EnterpriseTestContext context, Guid scenarioId)
        {
            if (!string.IsNullOrEmpty(sessionName_ComboBox.Text))
            {
                ScenarioSession itemToSave = context.ScenarioSessions.FirstOrDefault(n => n.EnterpriseScenarioId == scenarioId && n.Name == sessionName_ComboBox.Text);

                if (itemToSave == null)
                {
                    itemToSave = ScenarioSession.CreateScenarioSession(scenarioId, sessionName_ComboBox.Text, DateTime.Now);
                    itemToSave.Notes = notes_TextBox.Text;
                    context.AddToScenarioSessions(itemToSave);
                }
                else
                {
                    if (!itemToSave.Notes.Equals(notes_TextBox.Text))
                    {
                        itemToSave.Notes = notes_TextBox.Text;
                        itemToSave.EditedDate = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// Validates user input.
        /// </summary>
        /// <returns>true if input is valid, false otherwise.</returns>
        private bool ValidateInput(Guid scenarioId)
        {
            bool result = true;

            // STF-only
            if (GlobalSettings.IsDistributedSystem)
            {
                result = result && (!SetError(dispatcher_Label, STFDispatcherManager.Dispatcher == null, "Must be connected to a dispatcher."));
            }

            result = result && (!SetError(selectedScenario_TextBox, scenarioId == Guid.Empty, "Please select a scenario to run."));
            result = result && (!SetError(sessionName_ComboBox, string.IsNullOrEmpty(sessionName_ComboBox.Text), "Please select or enter a Session Name."));
            result = result && (!SetError(logLocation_TextBox, string.IsNullOrEmpty(logLocation_TextBox.Text), "Please enter the Monitor Service Log Location."));

            if (string.IsNullOrEmpty(logLocation_TextBox.Text))
            {
                settings_TabControl.SelectedIndex = 3;
            }

            return result;
        }

        /// <summary>
        /// Sets an error in the ErrorProvider if the error condition is true;
        /// </summary>
        /// <param name="control">The control to set the error.</param>
        /// <param name="setError">true if the error text is to be set.  False to remove any error text.</param>
        /// <param name="errorText">The error text.</param>
        /// <returns>true if an error was set, false otherwise.</returns>
        private bool SetError(Control control, bool setError, string errorText)
        {
            _errorProvider.SetError(control, setError ? errorText : string.Empty);
            return setError;
        }

        #region DistributedSystem

        /// <summary>
        /// Populates Combo Box settings for Notification Emails
        /// </summary>
        public void PopulateNotificationSettings()
        {
            TraceFactory.Logger.Debug("Failure Count: {0}".FormatWith(threshold_comboBox.SelectedValue));
            Ticket.EmailAddresses = email_textBox.Text.Trim().Replace(';', ',');
            Ticket.FailureCount = (int)threshold_comboBox.SelectedValue;
            Ticket.CollectDARTLogs = dartLog_CheckBox.Checked;

            TraceFactory.Logger.Debug("Over Time: {0}".FormatWith(failureTime_comboBox.SelectedValue));
            Ticket.FailureTime = (TimeSpan)failureTime_comboBox.SelectedValue;

            Ticket.TriggerList = triggerList_TextBox.Lines;
        }

        private void LoadComboBoxes(EnterpriseTestContext context)
        {
            sessionName_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionName.ToString());
            sessionName_ComboBox.SelectedIndex = -1;
            sessionType_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionType.ToString());
            sessionType_ComboBox.SelectedIndex = -1;
            sessionCycle_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionCycle.ToString());

             
            retention_ComboBox.DataSource = SessionLogRetentionHelper.ExpirationList;
            retention_ComboBox.SelectedIndex = retention_ComboBox.FindString(EnumUtil.GetDescription(WizardPageManager.GetDefaultLogRetention()));

            Dictionary<string, int> failureItems = new Dictionary<string, int>();
            List<TimeSpan> failTimes = new List<TimeSpan>();

            using (AssetInventoryContext assetContext = DbConnect.AssetInventoryContext())
            {
                string powerState = EnumUtil.GetDescription(VMPowerState.PoweredOff);
                var availableVMs = assetContext.FrameworkClients.Where(n => n.PowerState == powerState);

                platform_ComboBox.DataSource = null;
                platform_ComboBox.Items.Clear();
                platform_ComboBox.DisplayMember = "Name";
                platform_ComboBox.ValueMember = "FrameworkClientPlatformId";
                platform_ComboBox.DataSource = assetContext.FrameworkClientPlatforms.Where(n => n.Active).OrderBy(n => n.FrameworkClientPlatformId).ToList();

                holdId_ComboBox.DataSource = null;
                holdId_ComboBox.Items.Clear();
                holdId_ComboBox.DataSource = availableVMs.Select(n => n.HoldId).Distinct().Where(n => n != null).ToList();
            }
            failureItems.Add("1 Failure", 1);
            failureItems.Add("2 Failures", 2);
            failureItems.Add("5 Failures", 5);
            failureItems.Add("10 Failures", 10);
            failureItems.Add("15 Failures", 15);
            failureItems.Add("20 Failures", 20);

            threshold_comboBox.DataSource = new BindingSource(failureItems, null);
            threshold_comboBox.DisplayMember = "Key";
            threshold_comboBox.ValueMember = "Value";

            failTimes.Add(TimeSpan.FromMinutes(15));
            failTimes.Add(TimeSpan.FromMinutes(30));
            failTimes.Add(TimeSpan.FromHours(1));
            failTimes.Add(TimeSpan.FromHours(2));
            failTimes.Add(TimeSpan.FromHours(6));
            failTimes.Add(TimeSpan.FromHours(12));
            failureTime_comboBox.DataSource = new BindingSource(failTimes, null);


            if (Ticket.SessionId != null && _scenario != null)
            {
                if (!string.IsNullOrEmpty(_scenario.ScenarioSettings))
                {
                    var scenarioSettings = LegacySerializer.DeserializeDataContract<ScenarioSettings>(_scenario.ScenarioSettings);
                    //Populate boxes from selected settings
                    dartLog_CheckBox.Checked = scenarioSettings.NotificationSettings.CollectDartLogs;
                    email_textBox.Text = scenarioSettings.NotificationSettings.Emails;
                    failureTime_comboBox.SelectedIndex = failTimes.FindIndex(x => x == scenarioSettings.NotificationSettings.FailureTime);
                    threshold_comboBox.SelectedIndex = failureItems.ToList().FindIndex(x => x.Value == scenarioSettings.NotificationSettings.FailureCount);
                    triggerList_TextBox.Lines = scenarioSettings.NotificationSettings.TriggerList;
                    runtime_NumericUpDown.Value = Math.Min(scenarioSettings.EstimatedRunTime, runtime_NumericUpDown.Maximum); // scenarioSettings.EstimatedRunTime;
                    logLocation_TextBox.Text = scenarioSettings.LogLocation;
                    eventLog_CheckBox.Checked = scenarioSettings.CollectEventLogs;
                }

                sessionName_ComboBox.Text = string.IsNullOrEmpty(Ticket.SessionName) ? _scenario.Name : Ticket.SessionName;
            }

            //TraceFactory.Logger.Debug($"initial:{_initial}");
            if (!_initial)
            {

                sessionType_ComboBox.SelectedText = Ticket.SessionType;
                sessionCycle_ComboBox.SelectedIndex = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionCycle.ToString()).Select(x => x.Name).ToList().IndexOf(Ticket.SessionCycle);


                //TraceFactory.Logger.Debug($"email:{_ticket.EmailAddresses}");
                if (!string.IsNullOrEmpty(Ticket.EmailAddresses))
                {
                    dartLog_CheckBox.Checked = Ticket.CollectDARTLogs;
                    email_textBox.Text = Ticket.EmailAddresses;
                    failureTime_comboBox.SelectedIndex = failTimes.FindIndex(x => x == Ticket.FailureTime);
                    threshold_comboBox.SelectedIndex = failureItems.ToList().FindIndex(x => x.Value == Ticket.FailureCount);
                    triggerList_TextBox.Lines = Ticket.TriggerList;
                    runtime_NumericUpDown.Value = Math.Min(Ticket.DurationHours, runtime_NumericUpDown.Maximum);                    
                    eventLog_CheckBox.Checked = Ticket.CollectEventLogs;
                    email_textBox.Text = Ticket.EmailAddresses;
                }
                if (Ticket.FailureTime != TimeSpan.MaxValue)
                {
                    failureTime_comboBox.SelectedIndex = failTimes.FindIndex(x => x == Ticket.FailureTime);
                }
                if (Ticket.FailureCount != -1)
                {
                    threshold_comboBox.SelectedIndex = failureItems.ToList().FindIndex(x => x.Value == Ticket.FailureCount);
                }
                if (!string.IsNullOrEmpty(Ticket.LogLocation))
                {
                    logLocation_TextBox.Text = Ticket.LogLocation;
                }
                if (Ticket.TriggerList != null)
                {
                    triggerList_TextBox.Lines = Ticket.TriggerList;
                }
                dartLog_CheckBox.Checked = Ticket.CollectDARTLogs;
                eventLog_CheckBox.Checked = Ticket.CollectEventLogs;
                runtime_NumericUpDown.Value = Math.Min(Ticket.DurationHours, runtime_NumericUpDown.Maximum);
            }

        }

        private IEnumerable<VirtualMachine> SelectMachinesByPlatform()
        {
            string platform = ((FrameworkClientPlatform)platform_ComboBox.SelectedItem).FrameworkClientPlatformId;
            return VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available, platform: platform);
        }

        private IEnumerable<VirtualMachine> SelectMachinesByHoldId()
        {
            string holdId = (string)holdId_ComboBox.SelectedItem;
            return VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available, holdId: holdId);
        }
        private void PopulateSelectedVMs()
        {
            Ticket.RequestedVMs.Clear();
            if (platform_RadioButton.Checked)
            {
                Ticket.RequestedVMs.Add("BY_PLATFORM", (from vm in SelectMachinesByPlatform() select vm.Name).ToList());
            }
            else if (holdId_RadioButton.Checked)
            {
                Ticket.RequestedVMs.Add("BY_HOLDID", (from vm in SelectMachinesByHoldId() select vm.Name).ToList());
            }
        }

        private void resourcePool_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                platform_ComboBox.Enabled = (sender == platform_RadioButton);
                holdId_ComboBox.Enabled = (sender == holdId_RadioButton);
            }
        }

        private void viewVMs_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<VirtualMachine> machines = null;

            if (platform_RadioButton.Checked)
            {
                machines = SelectMachinesByPlatform().ToList();
            }
            else if (holdId_RadioButton.Checked)
            {
                machines = SelectMachinesByHoldId().ToList();
            }
            else
            {
                machines = VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available, holdId: null).ToList();
            }

            using (SelectedVirtualMachinesForm form = new SelectedVirtualMachinesForm(machines))
            {
                form.ShowDialog(this);
            }
        }

        private void refreshVMs_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                LoadComboBoxes(context);
            }
        }

        #endregion

        private void AddPvp_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.associatedProducts_DataGrid.Rows.Add();
        }

        private void DeletePvp_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = associatedProducts_DataGrid.SelectedCells[0].RowIndex;
            if (rowIndex < 0 || rowIndex > associatedProducts_DataGrid.Rows.Count)
            {
                return;
            }
            if (associatedProducts_DataGrid.Rows[rowIndex].IsNewRow)
            {
                return;
            }

            associatedProducts_DataGrid.Rows.RemoveAt(rowIndex);
        }

        private void UpdateDebug(string message)
        {
            if (_debugTabPage == null)
            {
                this._debugTabPage = new System.Windows.Forms.TabPage();
                this._debugTextBox = new System.Windows.Forms.TextBox();
                this._debugTabPage.SuspendLayout();
                // 
                // DebugTabPage
                // 
                this._debugTabPage.Controls.Add(this._debugTextBox);
                this._debugTabPage.Location = new System.Drawing.Point(4, 24);
                this._debugTabPage.Name = "DebugTabPage";
                this._debugTabPage.Padding = new System.Windows.Forms.Padding(3);
                this._debugTabPage.Size = new System.Drawing.Size(647, 144);
                this._debugTabPage.TabIndex = 3;
                this._debugTabPage.Text = "Debug";
                this._debugTabPage.UseVisualStyleBackColor = true;
                // 
                // DebugTextBox
                // 
                this._debugTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this._debugTextBox.Location = new System.Drawing.Point(0, 0);
                this._debugTextBox.Multiline = true;
                this._debugTextBox.Name = "DebugTextBox";
                this._debugTextBox.Size = new System.Drawing.Size(651, 141);
                this._debugTextBox.TabIndex = 0;
                this._debugTabPage.ResumeLayout(false);
                this._debugTabPage.PerformLayout();
            }

            _debugTextBox.Text = message;
        }

        private void AssociatedProductsDataGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            var cell = dgv[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell != null  && _newCellValue != null)
            {
                cell.Value = _newCellValue;
                //MessageBox.Show($"Cell [{e.ColumnIndex}, {e.RowIndex}].  Setting cell.Value = _newCellValue; Old: {cell.Value}; New: {_newCellValue}");
            }
        }

        private void AssociatedProductsDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var dgv = (DataGridView)sender;

            // Don't try to validate the 'new row' until finished 
            // editing since there is not any point in validating its initial value.
            if (dgv.Rows[e.RowIndex].IsNewRow) { return; }

            var cell = dgv[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell != null)
            {
                var selectedValue = (string)e.FormattedValue;
                _newCellValue = selectedValue;
                if (cell.Items.Contains(selectedValue))
                {
                    cell.Value = selectedValue;
                }
                else
                {
                    cell.Items.Add(selectedValue);
                    cell.Value = selectedValue;
                }
            }
        }

        private void AssociatedProductsDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var cb = e.Control as ComboBox;
            if (cb != null)
            {
                cb.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        private void AssociatedProductsDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point location = associatedProducts_DataGrid.PointToScreen(e.Location);
                this.VendorsProductsVersionsContextMenuStrip.Show(location);
            }
        }
    }
}
