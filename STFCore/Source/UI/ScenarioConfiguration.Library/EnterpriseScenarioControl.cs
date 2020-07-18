using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring an enterprise scenario.
    /// </summary>
    public partial class EnterpriseScenarioControl : ScenarioConfigurationControlBase
    {
        private EnterpriseScenario _scenario;
        private Collection<UserGroup> _availableGroups = new Collection<UserGroup>();
        private string _owner = string.Empty;
        private List<AssociatedProduct> _associatedProducts;
        private List<AssociatedProductVersion> _productVersions;
        private IEnumerable<ScenarioProduct> _scenarioProducts;
        private Dictionary<string, string> _scenarioCustomDictionary = new Dictionary<string, string>();

        private ScenarioSettings _settings;

        private bool _warningShown;
        private bool _fullyPainted;
        private bool _modified;
        //private string _newCellValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseScenarioControl"/> class.
        /// </summary>
        public EnterpriseScenarioControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(resource_GridView, GridViewStyle.Display);

            //using (EnterpriseTestContext context = new EnterpriseTestContext())
            //{
            //    _associatedProducts = AssociatedProduct.GetProductsByScenario(context, _scenario).ToList();
            //}
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _scenario; }
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Scenario Configuration";
            }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            var scenario = new EnterpriseScenario();
            scenario.Owner = UserManager.CurrentUserName;

            // Find all groups the user is a member of and add them to the new instance
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                scenario.AddGroups(context, UserManager.CurrentUserName);
            }

            Initialize(scenario);
        }

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        ///   </exception>
        public override void Initialize(object entity)
        {
            _scenario = entity as EnterpriseScenario;

            if (_scenario == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(EnterpriseScenario));
            }

            _owner = _scenario.Owner;

            // Set data source for the grid view and resize the columns
            resource_GridView.DataSource = _scenario.VirtualResources;
            resource_GridView.BestFitColumns();

            // Set data sources for combo boxes
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                string scenarioTags = GlobalSettings.Items[Setting.ScenarioTags];
                vertical_ComboBox.Items.AddRange(scenarioTags.Split(','));
                category_ComboBox.DataSource = EnterpriseScenario.SelectDistinctCompany(context).ToList();

                if (UserManager.CurrentUser.HasPrivilege(UserRole.Manager) || (!string.IsNullOrEmpty(_scenario.Owner) && _scenario.Owner.Equals(UserManager.CurrentUserName)))
                {
                    owner_ComboBox.Items.Add("Unknown");
                    foreach (var name in context.Users.Select(x => x.UserName))
                    {
                        owner_ComboBox.Items.Add(name);
                    }
                    owner_ComboBox.SelectedItem = _scenario.Owner;
                    editorGroups_CheckedListBox.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
                else
                {
                    owner_ComboBox.Items.Add(_scenario.Owner);
                    editorGroups_CheckedListBox.SelectionMode = SelectionMode.None;
                    editorGroups_CheckedListBox.BackColor = Color.FromKnownColor(KnownColor.Control);
                }

                LoadGroups(context);

                foreach (var group in context.UserGroups)
                {
                    _availableGroups.Add(group);
                }

                sessionCycle_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionCycle.ToString());
                sessionCycle_ComboBox.SelectedIndex = -1;

                if (!string.IsNullOrEmpty(_scenario.ScenarioSettings))
                {
                    _settings = LegacySerializer.DeserializeDataContract<ScenarioSettings>(_scenario.ScenarioSettings);
                    sessionCycle_ComboBox.SelectedIndex = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionCycle.ToString()).Select(x => x.Name).ToList().IndexOf(_settings.TargetCycle);
                }

                List<string> metadatas = new List<string>();
                foreach (VirtualResource vr in _scenario.VirtualResources.Where(n => n.Enabled))
                {
                    foreach (var vrms in vr.VirtualResourceMetadataSet.Where(n => n.Enabled))
                    {
                        metadatas.Add(vrms.MetadataType);
                    }
                }
                _associatedProducts = context.AssociatedProducts.Where(n => n.MetadataTypes.Any(m => metadatas.Contains(m.Name))).ToList();
                var productIds = _associatedProducts.Select(x => x.AssociatedProductId);
                _productVersions = AssociatedProductVersion.SelectVersions(context, productIds, _scenario.EnterpriseScenarioId).ToList();

                _scenarioProducts = from productInfo in _associatedProducts
                                    join versionInfo in _productVersions
                                    on productInfo.AssociatedProductId equals versionInfo.AssociatedProductId
                                    select new ScenarioProduct
                                    {
                                        ProductId = productInfo.AssociatedProductId,
                                        Version = versionInfo.Version,
                                        ScenarioId = _scenario.EnterpriseScenarioId,
                                        Name = productInfo.Name,
                                        Vendor = productInfo.Vendor,
                                        Active = versionInfo.Active
                                    };

                var scenarioProducts = _scenarioProducts.ToList();
                if (scenarioProducts.Count != 0)
                {
                    scenarioProductBindingSource.Clear();

                    scenarioProductBindingSource.DataSource = scenarioProducts.Distinct(new ScenarioProductEqualityComparer());
                    scenarioProductBindingSource.ResetBindings(true);
                }
            }

            // Set up data bindings
            name_TextBox.DataBindings.Add("Text", _scenario, "Name");
            description_TextBox.DataBindings.Add("Text", _scenario, "Description");
            category_ComboBox.DataBindings.Add("Text", _scenario, "Company");
            vertical_ComboBox.DataBindings.Add("Text", _scenario, "Vertical");
            //owner_ComboBox.DataBindings.Add("Text", _scenario, "Owner");

            CreateResourceDropDownMenu();

            //_associateProductHandler.CopyToUI(_associateProductHandler.CopyFromScenario(_scenario), this.associatedProducts_DataGrid);
            List<TimeSpan> failTimes = new List<TimeSpan>();
            Dictionary<string, int> failureItems = new Dictionary<string, int>();
            failureItems.Add("1 Failure", 1);
            failureItems.Add("2 Failures", 2);
            failureItems.Add("5 Failures", 5);
            failureItems.Add("10 Failures", 10);
            failureItems.Add("15 Failures", 15);
            failureItems.Add("20 Failures", 20);

            failTimes.Add(TimeSpan.FromMinutes(15));
            failTimes.Add(TimeSpan.FromMinutes(30));
            failTimes.Add(TimeSpan.FromHours(1));
            failTimes.Add(TimeSpan.FromHours(2));
            failTimes.Add(TimeSpan.FromHours(6));
            failTimes.Add(TimeSpan.FromHours(12));

            failureTime_comboBox.DataSource = new BindingSource(failTimes, null);
            threshold_comboBox.DataSource = new BindingSource(failureItems, null);
            threshold_comboBox.DisplayMember = "Key";
            threshold_comboBox.ValueMember = "Value";

            //if a new scenario, get empty, otherwise gather saved info from enterprise test.
            if (!string.IsNullOrEmpty(_scenario.ScenarioSettings))
            {
                _settings = LegacySerializer.DeserializeDataContract<ScenarioSettings>(_scenario.ScenarioSettings);
                //Populate boxes from selected settings
                dartLog_CheckBox.Checked = _settings.NotificationSettings.CollectDartLogs;
                email_textBox.Text = _settings.NotificationSettings.Emails;

                failureTime_comboBox.SelectedIndex = failTimes.FindIndex(x => x == _settings.NotificationSettings.FailureTime);
                threshold_comboBox.SelectedIndex = failureItems.ToList().FindIndex(x => x.Value == _settings.NotificationSettings.FailureCount);

                triggerList_TextBox.Lines = _settings.NotificationSettings.TriggerList;
                //runtime_NumericUpDown.Value =  _settings.EstimatedRunTime;

                runtime_NumericUpDown.Value = Math.Min(_settings.EstimatedRunTime, runtime_NumericUpDown.Maximum);
                logLocation_TextBox.Text = _settings.LogLocation;
                eventLog_CheckBox.Checked = _settings.CollectEventLogs;
                _scenarioCustomDictionary = _settings.ScenarioCustomDictionary;

                if (_scenarioCustomDictionary != null && _scenarioCustomDictionary.Count != 0)
                {
                    customDictionary_listBox.DataSource = new BindingSource(_scenarioCustomDictionary, null);
                    customDictionary_listBox.DisplayMember = "Key";
                    customDictionary_listBox.ValueMember = "Value";
                }
            }
            else
            {
                //Populate combo boxes
                try
                {
                    dartLog_CheckBox.Checked = _settings.NotificationSettings.CollectDartLogs;
                }
                catch
                {
                    dartLog_CheckBox.Checked = false;
                }

                email_textBox.Text = "";

                //Log settings
                logLocation_TextBox.Text = "";
                eventLog_CheckBox.Checked = false;

                runtime_NumericUpDown.Value = Math.Min(_scenario.EstimatedRuntime, runtime_NumericUpDown.Maximum);
            }
            if (string.IsNullOrEmpty(logLocation_TextBox.Text))
            {
                logLocation_TextBox.Text = GlobalSettings.WcfHosts[WcfService.DataLog];
            }
            AddEventHandlers();
        }

        private void ContractChanged(object sender, EventArgs e)
        {
            _modified = true;

            if (dartLog_CheckBox.Checked && string.IsNullOrEmpty(email_textBox.Text))
            {
                MessageBox.Show(@"For Dart Logs to be collected. Enter an email address", @"Dart Log Collection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddEventHandlers()
        {
            dartLog_CheckBox.CheckedChanged += ContractChanged;
            email_textBox.TextChanged += ContractChanged;
            failureTime_comboBox.SelectedIndexChanged += ContractChanged;
            threshold_comboBox.SelectedIndexChanged += ContractChanged;
            triggerList_TextBox.TextChanged += ContractChanged;
            runtime_NumericUpDown.ValueChanged += ContractChanged;
            logLocation_TextBox.TextChanged += ContractChanged;
            eventLog_CheckBox.CheckedChanged += ContractChanged;
            sessionCycle_ComboBox.SelectedIndexChanged += ContractChanged;
            customDictionary_listBox.SelectedIndexChanged += ContractChanged;
            //associatedProducts_DataGrid.EditingControlShowing += LeaveCell;
            //owner_ComboBox.SelectedIndexChanged += owner_ComboBox_SelectedIndexChanged;
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_Label.Focus();

            foreach (UserGroup item in editorGroups_CheckedListBox.CheckedItems.Cast<UserGroup>())
            {
                if (!_scenario.UserGroups.Any(x => x.GroupName.Equals(item.GroupName)))
                {
                    var group = Context.UserGroups.FirstOrDefault(x => x.GroupName.Equals(item.GroupName));
                    if (group != null)
                    {
                        _scenario.UserGroups.Add(group);
                    }
                }
            }

            Collection<UserGroup> removeList = new Collection<UserGroup>();
            foreach (UserGroup item in _scenario.UserGroups)
            {
                if (!editorGroups_CheckedListBox.CheckedItems.Cast<UserGroup>().Any(x => x.GroupName.Equals(item.GroupName)))
                {
                    removeList.Add(item);
                }
            }

            foreach (var group in removeList)
            {
                _scenario.UserGroups.Remove(group);
            }

            if (string.IsNullOrEmpty(_scenario.ScenarioSettings))
            {
                _settings = new ScenarioSettings();
            }

            if (_modified)
            {
                //Grab data from combo boxes, create settings, serialize
                _settings.EstimatedRunTime = (int)runtime_NumericUpDown.Value;
                _settings.TargetCycle = sessionCycle_ComboBox.Text;
                _settings.NotificationSettings.CollectDartLogs = dartLog_CheckBox.Checked;
                _settings.NotificationSettings.Emails = email_textBox.Text;
                _settings.NotificationSettings.FailureCount = (int)threshold_comboBox.SelectedValue;
                _settings.NotificationSettings.TriggerList = triggerList_TextBox.Lines;
                _settings.NotificationSettings.FailureTime = (TimeSpan)failureTime_comboBox.SelectedValue;
                _settings.LogLocation = logLocation_TextBox.Text;
                _settings.CollectEventLogs = eventLog_CheckBox.Checked;
                _settings.ScenarioCustomDictionary = _scenarioCustomDictionary;
                _scenario.ScenarioSettings = LegacySerializer.SerializeDataContract(_settings).ToString();
            }

            //_scenarioProducts = scenarioProductBindingSource.DataSource as IEnumerable<ScenarioProduct>;

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                for (int i = 0; i < associatedProducts_DataGrid.Rows.Count; i++)
                {
                    ScenarioProduct prod = associatedProducts_DataGrid.Rows[i].DataBoundItem as ScenarioProduct;
                    prod.Update(context);
                }

                //foreach (var product in _scenarioProducts)
                //{
                //    product.Update(context);
                //}
            }

            //_associateProductHandler.SaveData(_scenario.EnterpriseScenarioId, this.associatedProducts_DataGrid);
        }

        /// <summary>
        /// The owned drawing process
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 15 && !_fullyPainted)
            {
                // When this control fully paints, then enable the owner combobox.
                // This will avoid a bogus message on initial load by subscribing
                // to the SelectedIndexChanged event too early.
                owner_ComboBox.SelectedIndexChanged += owner_ComboBox_SelectedIndexChanged;
                _fullyPainted = true;
            }
        }

        /// <summary>
        /// Displays the platform usage
        /// </summary>
        public void DisplayPlatformUsage()
        {
            ScenarioPlatformUsageSet usages = new ScenarioPlatformUsageSet(_scenario);
            usages.Load(UserManager.CurrentUser);

            using (ScenarioPlatformUsageForm form = new ScenarioPlatformUsageForm(usages))
            {
                form.ShowDialog();
            }
        }

        private void LoadGroups(EnterpriseTestEntities entities)
        {
            foreach (UserGroup group in entities.UserGroups.OrderBy(x => x.GroupName))
            {
                var selected = _scenario.UserGroups.Any(x => x.GroupName.Equals(group.GroupName));
                editorGroups_CheckedListBox.Items.Add(group, selected);
            }
        }

        private void CreateResourceDropDownMenu()
        {
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            foreach (string resourceType in Context.ResourceTypes.Select(n => n.Name))
            {
                ToolStripMenuItem menuItem = null;
                try
                {
                    menuItem = new ToolStripMenuItem(resourceType)
                    {
                        Name = resourceType + "_menuItem",
                        Image = IconManager.Instance.VirtualResourceIcons.Images[resourceType],
                        Tag = ConfigurationObjectTag.Create(EnumUtil.Parse<VirtualResourceType>(resourceType))
                    };
                    menuItem.Click += newResource_MenuItem_Click;
                    items.Add(menuItem);
                }
                catch
                {
                    menuItem?.Dispose();
                    throw;
                }
            }

            foreach (ToolStripMenuItem item in items.OrderBy(n => n.Name))
            {
                addResource_ToolStripDropDownButton.DropDownItems.Add(item);
            }
        }

        private void newResource_MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            ConfigurationObjectTag tag = menuItem.Tag as ConfigurationObjectTag;
            using (var form = new ScenarioConfigurationControlForm(tag, Context))
            {
                virtualResources_ToolStrip.Refresh();
                virtualResources_ToolStrip.Update();

                form.Text = @"Virtual Resource Configuration";
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    AddResource(form.EntityObject as VirtualResource);
                }
            }
        }

        private void copyResource_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                using (VirtualResourceListForm form = new VirtualResourceListForm())
                {
                    form.Initialize(context.VirtualResources);
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (VirtualResource resource in form.SelectedResources)
                        {
                            AddResource(resource.Copy());
                        }
                    }
                }
            }
        }

        private void AddResource(VirtualResource resource)
        {
            resource.EnterpriseScenarioId = _scenario.EnterpriseScenarioId;
            resource.Enabled = true;
            _scenario.VirtualResources.Add(resource);
        }

        private void deleteResource_ToolStripButton_Click(object sender, EventArgs e)
        {
            // Check to see if there is anything selected
            if (!resource_GridView.SelectedRows.Any())
            {
                MessageBox.Show(@"No resources selected.", @"Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = resource_GridView.SelectedRows.Select(n => n.DataBoundItem).Cast<VirtualResource>().ToList();
            string resourceText = selected.Count > 1 ?
                                  "the {0} selected resources".FormatWith(selected.Count) :
                                  "'{0}'".FormatWith(selected.First().Name);

            DialogResult result = MessageBox.Show($@"Are you sure you want to delete {resourceText}?", @"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (VirtualResource resource in selected)
                {
                    _scenario.VirtualResources.Remove(resource);
                    // Set this so that the context knows to delete this object
                    resource.EnterpriseScenarioId = Guid.Empty;
                }
            }
        }

        private void name_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(name_TextBox.Text, "Name", name_Label, e);
        }
        private void logLocation_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(logLocation_TextBox.Text, "Monitor Service Log Location", logLocation_Label, e);
            if (string.IsNullOrEmpty(logLocation_TextBox.Text))
            {
                settings_TabControl.SelectedIndex = 2;
            }
        }
        private void scenarioSummary_ToolStripButton_Click(object sender, EventArgs e)
        {
            ShowScenarioSummaryForm(0);
        }

        /// <summary>
        /// Shows the scenario summary form.
        /// </summary>
        /// <param name="pageViewIndex">Index of the page view.</param>
        private void ShowScenarioSummaryForm(int pageViewIndex)
        {
            using (ScenarioSummaryForm form = new ScenarioSummaryForm(_scenario.EnterpriseScenarioId, pageViewIndex))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (var resource in _scenario.VirtualResources)
                    {
                        resource.CopyResourceProperties(form.ModifiedScenario.VirtualResources.Where(r => r.VirtualResourceId == resource.VirtualResourceId).FirstOrDefault());
                    }
                }
            }
        }

        private void DeviceBulkEdit_toolStripButton_Click(object sender, EventArgs e)
        {
            ShowScenarioSummaryForm(1);
        }

        private void owner_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;

            if (!_owner.Equals("Unknown") && !owner_ComboBox.SelectedItem.Equals(_owner) && !UserManager.CurrentUser.HasPrivilege(UserRole.Administrator) && !_warningShown)
            {
                string msg = "Changing the owner of this Scenario may lock out the current owner (which could be you).  Are you sure you want to continue?";
                result = MessageBox.Show(msg, @"Change Scenario Owner", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                _warningShown = true;
            }

            if (result == DialogResult.No)
            {
                owner_ComboBox.SelectedItem = _owner;
                _warningShown = false;
            }
            else
            {
                _owner = owner_ComboBox.SelectedItem.ToString();
            }
        }

        private void DisableOrEnableSelectedRows(bool enable)
        {
            var selected = resource_GridView.SelectedRows.Select(r => r.DataBoundItem).Cast<VirtualResource>().ToList();

            foreach (VirtualResource resource in selected)
            {
                resource.Enabled = enable;
            }
        }

        private void enableSelected_toolStripButton_Click(object sender, EventArgs e)
        {
            DisableOrEnableSelectedRows(true);
        }

        private void disableSelected_toolStripButton_Click(object sender, EventArgs e)
        {
            DisableOrEnableSelectedRows(false);
        }

        //private void AssociatedProductsDataGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    var dgv = (DataGridView)sender;
        //    var cell = dgv[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
        //    if (cell != null && _newCellValue != null)
        //    {
        //        cell.Value = _newCellValue;
        //        //MessageBox.Show($"Cell [{e.ColumnIndex}, {e.RowIndex}].  Setting cell.Value = _newCellValue; Old: {cell.Value}; New: {_newCellValue}");
        //    }
        //}

        //private void AssociatedProductsDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        //{
        //    var dgv = (DataGridView)sender;

        //    // Don't try to validate the 'new row' until finished
        //    // editing since there
        //    // is not any point in validating its initial value.
        //    //if (dgv.Rows[e.RowIndex].IsNewRow) { return; }

        //    var cell = dgv[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
        //    if (cell != null)
        //    {
        //        var selectedValue = (string)e.FormattedValue;
        //        _newCellValue = selectedValue;
        //        if (cell.Items.Contains(selectedValue))
        //        {
        //            //MessageBox.Show($"Cell [{e.ColumnIndex}, {e.RowIndex}] value {selectedValue} already an item.  Setting cell.Value = selectedValue; Old: {cell.Value}; New: {selectedValue}");
        //            cell.Value = selectedValue;
        //        }
        //        else
        //        {
        //            //MessageBox.Show($"Cell [{e.ColumnIndex}, {e.RowIndex}] value {selectedValue} NOT an item.  Adding and setting cell.Value = selectedValue; Old: {cell.Value}; New: {selectedValue}");
        //            cell.Items.Add(selectedValue);
        //            cell.Value = selectedValue;
        //        }
        //    }
        //}

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
                associatedProducts_DataGrid.PointToScreen(e.Location);
                //this.VendorsProductsVersionsContextMenuStrip.Show(location);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_name.Text) || string.IsNullOrEmpty(textBox_description.Text))
                return;

            if (_scenarioCustomDictionary != null && _scenarioCustomDictionary.ContainsKey(textBox_name.Text))
                return;

            if (_scenarioCustomDictionary == null)
                _scenarioCustomDictionary = new Dictionary<string, string>();

            customDictionary_listBox.DataSource = null;
            _scenarioCustomDictionary.Add(textBox_name.Text, textBox_description.Text);
            customDictionary_listBox.DataSource = new BindingSource(_scenarioCustomDictionary, null);
            customDictionary_listBox.DisplayMember = "Key";
            customDictionary_listBox.ValueMember = "Value";
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (customDictionary_listBox.SelectedIndex == -1)
                return;
            int selectedIndex = customDictionary_listBox.SelectedIndex;

            customDictionary_listBox.DataSource = null;
            var key = _scenarioCustomDictionary.ElementAt(selectedIndex);
            _scenarioCustomDictionary.Remove(key.Key);
            customDictionary_listBox.DataSource = new BindingSource(_scenarioCustomDictionary, null);
            customDictionary_listBox.DisplayMember = "Key";
            customDictionary_listBox.ValueMember = "Value";
        }

    }
}