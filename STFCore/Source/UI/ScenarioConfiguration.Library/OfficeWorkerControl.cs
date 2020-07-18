using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring an Office Worker virtual resource.
    /// </summary>
    public partial class OfficeWorkerControl : ScenarioConfigurationControlBase
    {
        private OfficeWorker _officeWorker = null;
        private int _maxResourcesPerVM = 1;
        private SortableBindingList<WorkerActivityConfiguration> _mainConfigurations = null;
        private ResourceExecutionPhase _selectedPhase = ResourceExecutionPhase.Main;
        ActivityExecutionHelpForm _helpForm = null;

        private ControlSet _durationControls = new ControlSet();
        private ControlSet _iterationControls = new ControlSet();
        private ControlSet _scheduleControls = new ControlSet();
        private ControlSet _paceControls = new ControlSet();

        private class ControlSet
        {
            public List<Control> Items { get; private set; }

            public ControlSet()
            {
                Items = new List<Control>();
            }

            public void Hide()
            {
                Items.ForEach(x => x.Visible = false);
            }

            public void Show()
            {
                Items.ForEach(x => x.Visible = true);
            }

            public void AddRange(params Control[] items)
            {
                Items.AddRange(items);
            }

            public void SetY(int value)
            {
                Items.ForEach(x => x.Location = new Point(x.Location.X, value));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerControl"/> class.
        /// </summary>
        public OfficeWorkerControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(activity_GridView, GridViewStyle.Display);

            _mainConfigurations = new SortableBindingList<WorkerActivityConfiguration>();
            _helpForm = new ActivityExecutionHelpForm(ActivityExecutionHelpForm.Edge.Right);

            helpIcon.Location = new Point(226, 1);
            pacing_TimeDelayControl.Location = new Point(5, 47);
            configureSchedule_LinkLabel.Location = new Point(6, 13);
            repeatCount_Label.Location = new Point(6, 13);
            repeatCount_TextBox.Location = new Point(83, 10);
            duration_Label.Location = new Point(10, 13);
            duration_TextBox.Location = new Point(83, 10);
            durationTimeFormat_Label.Location = new Point(130, 13);
            pace_Label.Location = new Point(10, 43);
            pace_TextBox.Location = new Point(83, 100);
            paceTimeFormat_Label.Location = new Point(130, 43);
            help_toolTip.SetToolTip(userPoolConfig_PictureBox, Resource.UserPoolConfigHelp);

            _durationControls.AddRange(duration_Label, duration_TextBox, durationTimeFormat_Label);
            _iterationControls.AddRange(repeatCount_Label, repeatCount_TextBox);
            _scheduleControls.AddRange(configureSchedule_LinkLabel);
            _paceControls.AddRange(pace_Label, pace_TextBox, paceTimeFormat_Label);
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Office Worker Configuration";
            }
        }

        /// <summary>
        /// Gets the type of the worker.
        /// </summary>
        protected virtual VirtualResourceType WorkerType
        {
            get { return VirtualResourceType.OfficeWorker; }
        }

        /// <summary>
        /// Gets the office worker.
        /// </summary>
        protected OfficeWorker Worker
        {
            get { return _officeWorker; }
        }

        protected int MaxResourcesPerVM
        {
            get { return _maxResourcesPerVM; }
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _officeWorker; }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            OfficeWorker newEntity = new OfficeWorker();
            newEntity.Platform = "WINDOWS-ANY";
            Initialize(newEntity);
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
            _officeWorker = entity as OfficeWorker;
            if (_officeWorker == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(OfficeWorker));
            }

            // Load the configurations into the helper class
            foreach (var item in _officeWorker.VirtualResourceMetadataSet)
            {
                WorkerExecutionPlan plan = null;
                if (item.ExecutionPlan == null)
                {
                    plan = new WorkerExecutionPlan();
                }
                else
                {
                    plan = LegacySerializer.DeserializeDataContract<WorkerExecutionPlan>(item.ExecutionPlan);
                }

                _mainConfigurations.Add(new WorkerActivityConfiguration(item, plan));
            }

            // Load the activities into the binding list
            activity_TabControl.SelectTab(main_TabPage);
            _selectedPhase = ResourceExecutionPhase.Main;
            activity_GridView.BestFitColumns();

            InitializePlatforms();

            //testcaseid_numericUpDown.Value = _officeWorker.TestCaseId.HasValue ? Convert.ToDecimal(_officeWorker.TestCaseId.Value): 0;
            testcaseid_numericUpDown.Value = Convert.ToDecimal(_officeWorker.TestCaseId);

            LoadGroups();
            LoadUserPools();
            SetupChildControls(_officeWorker);
            SetUpDataBindings();
            ConfigureEnableAllButton();
            CreateActivityDropDownMenu();

            RefreshGrid();
        }

        protected virtual void InitializePlatforms()
        {
            virtualMachinePlatform_ComboBox.Validating += virtualMachinePlatform_ComboBox_Validating;
            virtualMachinePlatform_ComboBox.SetPlatform(_officeWorker.Platform, WorkerType);
        }

        protected virtual void ReadPlatform()
        {
            _officeWorker.Platform = ((FrameworkClientPlatform)virtualMachinePlatform_ComboBox.SelectedItem).FrameworkClientPlatformId;
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_TextBox.Focus();

            // Push the configuration data into the ExecutionPlan property of the metadata
            foreach (var item in _officeWorker.VirtualResourceMetadataSet)
            {
                if (string.IsNullOrEmpty(item.ExecutionPlan))
                {
                    var configItem = _mainConfigurations.FirstOrDefault(e => e.Metadata.VirtualResourceMetadataId == item.VirtualResourceMetadataId);
                    if (configItem != null)
                    {
                        item.ExecutionPlan = LegacySerializer.SerializeDataContract(configItem.ExecutionPlan).ToString();
                    }
                }
                else
                {
                    // Entity Framework handles all situations where no fields have changed, except serialized XML.
                    // So to ensure this form doesn't think something has changed when it hasn't, only update
                    // the serialized XML if there is actually a change.
                    XDocument oldPlan = XDocument.Parse(item.ExecutionPlan);

                    var configItem = _mainConfigurations.FirstOrDefault(e => e.Metadata.VirtualResourceMetadataId == item.VirtualResourceMetadataId);
                    if (configItem != null)
                    {
                        string serializedPlan = LegacySerializer.SerializeDataContract(configItem.ExecutionPlan).ToString();
                        XDocument newPlan = XDocument.Parse(serializedPlan);

                        if (!XmlUtil.AreEqual(oldPlan, newPlan, orderInvariant: true))
                        {
                            item.ExecutionPlan = serializedPlan;
                        }
                    }
                }
            }

            ReadPlatform();

            // If running as a SolutionTester, it's possible that there are no
            // user pools configured in STB.  So check this before making a change.
            if (userPool_ComboBox.SelectedItem != null)
            {
                _officeWorker.UserPool = ((DomainAccountPool)userPool_ComboBox.SelectedItem).DomainAccountKey;
            }

            // If any AD groups were specified, serialize those into the property, otherwise clear the property
            if (groups_AddRemoveListControl.DestinationItems.Count > 0)
            {
                Collection<ActiveDirectoryGroup> groups = new Collection<ActiveDirectoryGroup>();

                foreach (var item in groups_AddRemoveListControl.DestinationItems.Cast<ActiveDirectoryGroup>())
                {
                    groups.Add(item);
                }

                _officeWorker.SecurityGroups = LegacySerializer.SerializeDataContract(groups).ToString();
            }
            else
            {
                _officeWorker.SecurityGroups = string.Empty;
            }

            // Check to see if the resources per VM value is lower than the configured value
            // for the resource type.  If it is, then set it, if not, then set the _officeWorker
            // value for this property to Null.
            if (workersPerVM_UpDown.Value <= _maxResourcesPerVM)
            {
                _officeWorker.ResourcesPerVM = (int)workersPerVM_UpDown.Value;
            }
            else
            {
                _officeWorker.ResourcesPerVM = null;
            }

            _officeWorker.TestCaseId = Convert.ToInt32(testcaseid_numericUpDown.Value);
        }

        /// <summary>
        /// Sets up any child controls.
        /// </summary>
        /// <param name="officeWorker">The office worker.</param>
        protected virtual void SetupChildControls(OfficeWorker officeWorker)
        {
            var yOffset = 85;

            configuration_TabControl.TabPages.Remove(userAccounts_TabPage);

            // Move the bottom controls up since there are no Citrix controls
            var location = workflow_GroupBox.Location;
            workflow_GroupBox.Location = new Point(location.X, location.Y - yOffset);

            location = workerQuantity_GroupBox.Location;
            workerQuantity_GroupBox.Location = new Point(location.X, location.Y - yOffset);

            location = startup_TimeDelayControl.Location;
            startup_TimeDelayControl.Location = new Point(location.X, location.Y - yOffset);

            location = solutionTesterActivityAccountsGroupBox.Location;
            solutionTesterActivityAccountsGroupBox.Location = new Point(location.X, location.Y - yOffset);
        }

        private void LoadGroups()
        {
            groups_AddRemoveListControl.SourceLabelText = "Available AD Groups";
            groups_AddRemoveListControl.DestinationLabelText = "Assigned AD Groups";
            groups_AddRemoveListControl.SourceListDisplayMember = "Name";
            groups_AddRemoveListControl.DesinationListDisplayMember = "Name";

            List<ActiveDirectoryGroup> groups = null;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                groups = context.ActiveDirectoryGroups.ToList();
                groups_AddRemoveListControl.SourceItems = groups;

                if (!string.IsNullOrEmpty(_officeWorker.SecurityGroups))
                {
                    var selectedGroups = LegacySerializer.DeserializeDataContract<Collection<ActiveDirectoryGroup>>(_officeWorker.SecurityGroups);
                    groups_AddRemoveListControl.DestinationItems = selectedGroups;
                }
                else
                {
                    groups_AddRemoveListControl.DestinationItems = new List<ActiveDirectoryGroup>();
                }
            }
        }

        protected virtual void LoadUserPools()
        {
            userPool_ComboBox.DisplayMember = "Description";
            DomainAccountPool selectedPool = null;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var userPools = context.DomainAccountPools.OrderBy(n => n.Description).ToList();

                if (!userPools.Any(x => x.DomainAccountKey.Equals(_officeWorker.UserPool)))
                {
                    MessageBox.Show
                        (
                            "The User Pool configured for your Worker is not available.  Your worker will be set to use the default User Pool. Select the User Account tab to change.",
                            "User Pool Unavailable",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );
                    selectedPool = userPools.First(x => x.DomainAccountKey.Equals("User"));
                }
                else
                {
                    selectedPool = userPools.First(x => x.DomainAccountKey.Equals(_officeWorker.UserPool));
                }

                userPool_ComboBox.DataSource = userPools;
                userPool_ComboBox.SelectedItem = selectedPool;
            }
        }

        private void SetUpDataBindings()
        {
            name_TextBox.DataBindings.Add("Text", _officeWorker, "Name");
            description_TextBox.DataBindings.Add("Text", _officeWorker, "Description");
            instanceCount_TextBox.DataBindings.Add("Text", _officeWorker, "InstanceCount");
            random_RadioButton.DataBindings.Add("Checked", _officeWorker, "RandomizeActivities");
            repeatCount_TextBox.DataBindings.Add("Text", _officeWorker, "RepeatCount");

            startup_TimeDelayControl.ValueChanged -= startup_TimeDelayControl_ValueChanged;

            startup_TimeDelayControl.Randomize = _officeWorker.RandomizeStartupDelay;
            startup_TimeDelayControl.MinDelay = TimeSpan.FromSeconds(_officeWorker.MinStartupDelay);
            startup_TimeDelayControl.MaxDelay = TimeSpan.FromSeconds(_officeWorker.MaxStartupDelay);

            startup_TimeDelayControl.ValueChanged += startup_TimeDelayControl_ValueChanged;


            //List<ActiveDirectoryGroup> securityGroups = _officeWorker.SecurityGroups.ToObject<List<ActiveDirectoryGroup>>();
            //securityGroups_TextBox.Text = string.Join(";", securityGroups);

            // Set up the Resources Per VM control value.
            string workerType = WorkerType.ToString();
            _maxResourcesPerVM = Context.ResourceTypes.First(n => n.Name == workerType).MaxResourcesPerHost;
            workersPerVM_UpDown.Maximum = _maxResourcesPerVM;
            workersPerVM_UpDown.Minimum = 1;
            if (_officeWorker.ResourcesPerVM != null)
            {
                if (_officeWorker.ResourcesPerVM > workersPerVM_UpDown.Maximum)
                {
                    workersPerVM_UpDown.Value = workersPerVM_UpDown.Maximum;
                }
                else
                {
                    workersPerVM_UpDown.Value = (decimal)_officeWorker.ResourcesPerVM;
                }
            }

            switch (_officeWorker.ExecutionMode)
            {
                case ExecutionMode.Duration:
                    timeBased_RadioButton.Checked = true;
                    InitializeActivityDelayControl();
                    break;

                case ExecutionMode.Iteration:
                    countBased_RadioButton.Checked = true;
                    InitializeActivityDelayControl();
                    break;

                case ExecutionMode.RateBased:
                    rateBased_RadioButton.Checked = true;
                    break;

                case ExecutionMode.SetPaced:
                    setPaced_radioButton.Checked = true;
                    break;

                default:
                    scheduled_RadioButton.Checked = true;
                    InitializeActivityDelayControl();
                    break;
            }

            DisplayControls(_officeWorker.ExecutionMode);

            // Set initial value for random/ordered fields
            random_RadioButton.Checked = _officeWorker.RandomizeActivities;
            sequential_RadioButton.Checked = !_officeWorker.RandomizeActivities;

            Binding binding = new Binding("Text", _officeWorker, "DurationTime");
            binding.Format += new ConvertEventHandler(ConvertIntToHourMin);
            binding.Parse += new ConvertEventHandler(ConvertHourMinToInt);
            duration_TextBox.DataBindings.Add(binding);

            Binding binding2 = new Binding("Text", _officeWorker, "MinActivityDelay");
            binding2.Format += new ConvertEventHandler(ConvertIntToHourMin);
            binding2.Parse += new ConvertEventHandler(ConvertHourMinToInt);
            pace_TextBox.DataBindings.Add(binding2);
        }

        private void InitializeActivityDelayControl()
        {
            pacing_TimeDelayControl.ValueChanged -= pacing_TimeDelayControl_ValueChanged;
            pacing_TimeDelayControl.Randomize = _officeWorker.RandomizeActivityDelay;
            pacing_TimeDelayControl.MinDelay = TimeSpan.FromSeconds(_officeWorker.MinActivityDelay);
            pacing_TimeDelayControl.MaxDelay = TimeSpan.FromSeconds(_officeWorker.MaxActivityDelay);
            pacing_TimeDelayControl.ValueChanged += pacing_TimeDelayControl_ValueChanged;
        }

        private ExecutionMode SelectedExecutionMode
        {
            get
            {
                string tag = string.Empty;
                if (timeBased_RadioButton.Checked)
                {
                    tag = timeBased_RadioButton.Tag.ToString();
                }
                else if (countBased_RadioButton.Checked)
                {
                    tag = countBased_RadioButton.Tag.ToString();
                }
                else if (rateBased_RadioButton.Checked)
                {
                    tag = rateBased_RadioButton.Tag.ToString();
                }
                else if (setPaced_radioButton.Checked)
                {
                    tag = setPaced_radioButton.Tag.ToString();
                }
                else
                {
                    tag = scheduled_RadioButton.Tag.ToString();
                }
                return EnumUtil.Parse<ExecutionMode>(tag);
            }
        }

        protected IEnumerable<MetadataType> MetadataTypes
        {
            get { return GetMetadataTypes(WorkerType); }
        }

        protected void RemoveCitrixWorkerActivities()
        {
            // This is handled at this level because the Telerik grid
            // by default wants to be private.
            Collection<WorkerActivityConfiguration> rows = new Collection<WorkerActivityConfiguration>();
            foreach (var row in activity_GridView.Rows)
            {
                rows.Add(row.DataBoundItem as WorkerActivityConfiguration);
            }

            foreach (var row in rows)
            {
                RemoveActivity(row);
            }
        }

        private void CreateActivityDropDownMenu()
        {
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            foreach (MetadataType type in MetadataTypes)
            {
                string typeName = type.Name;
                ToolStripMenuItem menuItem = null;
                try
                {
                    menuItem = new ToolStripMenuItem(type.Title);
                    menuItem.Name = typeName + "_menuItem";

                    var image = IconManager.Instance.PluginIcons.Images[typeName];
                    if (image == null)
                    {
                        image = IconManager.Instance.PluginIcons.Images["HPLogo"];
                    }
                    menuItem.Image = image;
                    menuItem.Tag = ConfigurationObjectTag.Create(VirtualResourceType.OfficeWorker, typeName);
                    menuItem.ToolTipText = type.Title;
                    menuItem.Click += new EventHandler(newActivity_MenuItem_Click);

                    // Determine whether this item should be added to a sub-menu
                    if (string.IsNullOrEmpty(type.Group))
                    {
                        items.Add(menuItem);
                    }
                    else
                    {
                        // See if the sub-menu already exists
                        ToolStripMenuItem subMenu = items.FirstOrDefault(n => n.Text == type.Group);
                        if (subMenu == null)
                        {
                            subMenu = new ToolStripMenuItem(type.Group);
                            items.Add(subMenu);
                        }
                        subMenu.DropDownItems.Add(menuItem);
                    }
                }
                catch
                {
                    if (menuItem != null)
                    {
                        menuItem.Dispose();
                    }
                    throw;
                }
            }

            newActivity_ToolStripDropDownButton.DropDownItems.AddRange(items.OrderBy(n => n.Text).ToArray());
        }

        private void newActivity_MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            ConfigurationObjectTag tag = menuItem.Tag as ConfigurationObjectTag;
            using (var form = new ScenarioConfigurationControlForm(tag, Context))
            {
                form.Text = "Worker Activity Configuration";
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    AddActivity(form.EntityObject as VirtualResourceMetadata);
                }
            }
        }

        private void copyActivity_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                using (VirtualResourceMetadataListForm form = new VirtualResourceMetadataListForm())
                {
                    form.Initialize(VirtualResourceMetadata.Select(context, VirtualResourceType.OfficeWorker));
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (VirtualResourceMetadata metadata in form.SelectedMetadata)
                        {
                            AddActivity(metadata.Copy(false));
                        }
                    }
                }
            }
        }

        private void AddActivity(VirtualResourceMetadata metadata)
        {
            metadata.VirtualResourceId = _officeWorker.VirtualResourceId;
            metadata.Enabled = true;
            _officeWorker.VirtualResourceMetadataSet.Add(metadata);

            WorkerExecutionPlan plan = null;
            if (metadata.ExecutionPlan == null)
            {
                plan = new WorkerExecutionPlan();
            }
            else
            {
                plan = LegacySerializer.DeserializeDataContract<WorkerExecutionPlan>(metadata.ExecutionPlan);
            }

            plan.Order = ConfigurationByPhase.Count() > 0 ? ConfigurationByPhase.Max(x => x.ExecutionPlan.Order) + 1 : 1;

            var tab = activity_TabControl.SelectedTab;
            if (tab == main_TabPage)
            {
                plan.Phase = ResourceExecutionPhase.Main;
            }
            else
            {
                plan.Value = 1;
                plan.ActivityPacing.DelayOnRepeat = true;
                plan.Phase = (tab == setup_TabPage) ? ResourceExecutionPhase.Setup : ResourceExecutionPhase.Teardown;
            }

            _mainConfigurations.Add(new WorkerActivityConfiguration(metadata, plan));

            RefreshGrid();

            // Set the current plan to this new entry
            activity_GridView.ChildRows[ConfigurationByPhase.Count() - 1].IsSelected = true;
        }

        private void deleteActivity_ToolStripButton_Click(object sender, EventArgs e)
        {
            // Check to see if there is anything selected
            if (!activity_GridView.SelectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (activity_GridView.SelectedRows.Count > 1)
            {
                DialogResult result = MessageBox.Show
                    ("Are you sure you want to delete all selected rows?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                var selectedRow = activity_GridView.SelectedRows[0];
                var selected = selectedRow.DataBoundItem as WorkerActivityConfiguration;

                DialogResult result = MessageBox.Show
                    ("Are you sure you want to delete {0}?".FormatWith(selected.Name),
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            List<WorkerActivityConfiguration> deletedItems = new List<WorkerActivityConfiguration>();
            foreach (var selectedRow in activity_GridView.SelectedRows)
            {
                var selected = selectedRow.DataBoundItem as WorkerActivityConfiguration;
                deletedItems.Add(selected);
            }

            foreach (var item in deletedItems)
            {
                RemoveActivity(item);
            }
        }

        internal void RemoveActivity(WorkerActivityConfiguration item)
        {
            _officeWorker.VirtualResourceMetadataSet.Remove(item.Metadata);
            _mainConfigurations.Remove(item);

            // Set this so that the context knows to delete this object
            item.Metadata.VirtualResourceId = Guid.Empty;

            // Reset the order number for each remaining entry
            int index = 1;
            foreach (var config in ConfigurationByPhase.OrderBy(x => x.ExecutionPlan.Order))
            {
                config.Order = index++;
            }

            RefreshGrid();
        }

        private void reorder_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (WorkerActivityOrderForm form = new WorkerActivityOrderForm())
            {
                form.Initialize(ConfigurationByPhase.ToList());
                form.ShowDialog(this);
                RefreshGrid();
            }
        }

        private void enableAll_ToolStripButton_Click(object sender, EventArgs e)
        {
            // Set a flag that will tell us what to do with all the resources
            bool newState = enableAll_ToolStripButton.Text == "Enable All";

            foreach (var config in ConfigurationByPhase)
            {
                config.Metadata.Enabled = newState;
            }
            ConfigureEnableAllButton();
            RefreshGrid();
        }

        private void ConfigureEnableAllButton()
        {
            // If they are all enabled, change the button to say "Disable All"
            if (ConfigurationByPhase.All(n => n.Metadata.Enabled))
            {
                enableAll_ToolStripButton.Image = IconManager.Instance.DisableIcon;
                enableAll_ToolStripButton.Text = "Disable All";
            }
            else
            {
                enableAll_ToolStripButton.Image = IconManager.Instance.EnableIcon;
                enableAll_ToolStripButton.Text = "Enable All";
            }
        }

        private void random_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (random_RadioButton.Checked != _officeWorker.RandomizeActivities)
            {
                //Only update the data object if the value has changed
                _officeWorker.RandomizeActivities = random_RadioButton.Checked;
            }
            reorder_ToolStripButton.Visible = !random_RadioButton.Checked;
            activity_GridView.Columns["order_GridViewColumn"].IsVisible = (sequential_RadioButton.Checked);
        }

        private void ConvertHourMinToInt(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.Parse(((string)e.Value)).TotalMinutes;
        }

        private void ConvertIntToHourMin(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.ToTimeSpanString((int)e.Value);
        }

        private void startup_TimeDelayControl_ValueChanged(object sender, System.EventArgs e)
        {
            if (_officeWorker != null)
            {
                _officeWorker.RandomizeStartupDelay = startup_TimeDelayControl.Randomize;
                _officeWorker.MinStartupDelay = (int)startup_TimeDelayControl.MinDelay.TotalSeconds;
                //for the first load, the value from UI for max startup will be 0, if it is that condition, we don't want to overwrite the metadata value
                if (startup_TimeDelayControl.MaxDelay.TotalSeconds > 0)
                {
                    _officeWorker.MaxStartupDelay = (int)startup_TimeDelayControl.MaxDelay.TotalSeconds;
                }
            }
        }

        private void configureSchedule_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (WorkerScheduledExecutionForm form = new WorkerScheduledExecutionForm(_officeWorker.ExecutionSchedule))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _officeWorker.ExecutionSchedule = form.ScheduleXml;
                    _officeWorker.DurationTime = form.Schedule.UseDuration ? form.Schedule.Duration : form.Schedule.CumulativeDuration;
                }
            }
        }

        #region Validation Event Handlers

        protected void EnableWorkerValidation(bool enable)
        {
            if (enable)
            {
                instanceCount_TextBox.Validating += instanceCount_TextBox_Validating;
                repeatCount_TextBox.Validating += repeatCount_TextBox_Validating;
                duration_TextBox.Validating += duration_TextBox_Validating;
            }
            else
            {
                instanceCount_TextBox.Validating -= instanceCount_TextBox_Validating;
                repeatCount_TextBox.Validating -= repeatCount_TextBox_Validating;
                duration_TextBox.Validating -= duration_TextBox_Validating;
            }
        }

        private void name_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(name_TextBox.Text, "Name", name_Label, e);
        }

        private void virtualMachinePlatform_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(virtualMachinePlatform_ComboBox.Text, "Platform", platform_Label, e);
        }

        protected virtual void instanceCount_TextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidatePositiveInt(instanceCount_TextBox.Text, "Worker Count", instanceCount_TextBox, e);
        }

        private void repeatCount_TextBox_Validating(object sender, CancelEventArgs e)
        {
            var mode = SelectedExecutionMode;
            if (mode == ExecutionMode.Iteration || mode == ExecutionMode.RateBased)
            {
                ValidatePositiveInt(repeatCount_TextBox.Text, "Repeat Count", repeatCount_TextBox, e);
            }
        }

        private void duration_TextBox_Validating(object sender, CancelEventArgs e)
        {
            var mode = SelectedExecutionMode;
            if (mode == ExecutionMode.Duration || mode == ExecutionMode.RateBased || mode == ExecutionMode.SetPaced)
            {
                ValidateRegex(duration_TextBox.Text, "Duration", "^\\d+:\\d+$", "h:mm", duration_TextBox, e);

                if (!e.Cancel)
                {
                    ValidatePositiveInt(_officeWorker.DurationTime.ToString(), "Duration", duration_TextBox, e);
                }
            }
        }

        #endregion

        private void DisplayControls(ExecutionMode executionMode)
        {
            // Change parent controls for some of the children based on the
            // tab page that is to be displayed.
            switch (executionMode)
            {
                case ExecutionMode.Duration:
                    workflow_Panel.Controls.Clear();
                    duration_Label.SetY(13);
                    duration_TextBox.SetY(10);
                    durationTimeFormat_Label.SetY(13);
                    _durationControls.Show();
                    _paceControls.Hide();
                    _iterationControls.Hide();
                    _scheduleControls.Hide();
                    this.SuspendLayout();
                    workflow_Panel.Controls.AddRange(_durationControls.Items.ToArray());
                    workflow_Panel.Controls.Add(pacing_TimeDelayControl);
                    workflow_Panel.Controls.Add(helpIcon);
                    this.ResumeLayout();
                    break;
                case ExecutionMode.Iteration:
                    workflow_Panel.Controls.Clear();
                    _durationControls.Hide();
                    _paceControls.Hide();
                    _iterationControls.Show();
                    _scheduleControls.Hide();
                    this.SuspendLayout();
                    workflow_Panel.Controls.AddRange(_iterationControls.Items.ToArray());
                    workflow_Panel.Controls.Add(pacing_TimeDelayControl);
                    workflow_Panel.Controls.Add(helpIcon);
                    this.ResumeLayout();
                    break;
                case ExecutionMode.RateBased:
                    workflow_Panel.Controls.Clear();
                    duration_Label.SetY(46);
                    duration_TextBox.SetY(43);
                    durationTimeFormat_Label.SetY(46);
                    _durationControls.Show();
                    _paceControls.Hide();
                    _iterationControls.Show();
                    _scheduleControls.Hide();
                    this.SuspendLayout();
                    workflow_Panel.Controls.AddRange(_iterationControls.Items.ToArray());
                    workflow_Panel.Controls.AddRange(_durationControls.Items.ToArray());
                    workflow_Panel.Controls.Add(helpIcon);
                    this.ResumeLayout();
                    break;
                case ExecutionMode.Scheduled:
                    workflow_Panel.Controls.Clear();
                    _iterationControls.Hide();
                    _durationControls.Hide();
                    _paceControls.Hide();
                    _scheduleControls.Show();
                    this.SuspendLayout();
                    workflow_Panel.Controls.AddRange(_scheduleControls.Items.ToArray());
                    workflow_Panel.Controls.Add(pacing_TimeDelayControl);
                    workflow_Panel.Controls.Add(helpIcon);
                    this.ResumeLayout();
                    break;
                case ExecutionMode.SetPaced:
                    workflow_Panel.Controls.Clear();
                    duration_Label.SetY(46);
                    duration_TextBox.SetY(43);
                    durationTimeFormat_Label.SetY(46);
                    _durationControls.Show();
                    pace_Label.SetY(76);
                    pace_TextBox.SetY(73);
                    paceTimeFormat_Label.SetY(76);
                    _paceControls.Show();
                    _iterationControls.Hide();
                    _scheduleControls.Hide();
                    this.SuspendLayout();
                    workflow_Panel.Controls.AddRange(_paceControls.Items.ToArray());
                    workflow_Panel.Controls.AddRange(_durationControls.Items.ToArray());
                    workflow_Panel.Controls.Add(helpIcon);
                    this.ResumeLayout();
                    break;
            }
        }

        private void pacing_TimeDelayControl_ValueChanged(object sender, EventArgs e)
        {
            if (_officeWorker != null)
            {
                _officeWorker.RandomizeActivityDelay = pacing_TimeDelayControl.Randomize;
                _officeWorker.MinActivityDelay = (int)pacing_TimeDelayControl.MinDelay.TotalSeconds;
                //for the initial load of UI, the max value of the delay is 0, we don't want to overwrite it at this time
                if (pacing_TimeDelayControl.MaxDelay.TotalSeconds > 0)
                {
                    _officeWorker.MaxActivityDelay = (int)pacing_TimeDelayControl.MaxDelay.TotalSeconds;
                }
            }
        }


        private void activity_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectedExecutionPhase)
            {
                case ResourceExecutionPhase.Main:
                    _selectedPhase = ResourceExecutionPhase.Main;
                    activity_GridView.Parent = main_TabPage;
                    activity_ToolStrip.Parent = main_TabPage;

                    RefreshGrid();
                    break;
                case ResourceExecutionPhase.Setup:
                    _selectedPhase = ResourceExecutionPhase.Setup;
                    activity_GridView.Parent = setup_TabPage;
                    activity_ToolStrip.Parent = setup_TabPage;
                    RefreshGrid();
                    break;
                case ResourceExecutionPhase.Teardown:
                    _selectedPhase = ResourceExecutionPhase.Teardown;
                    activity_GridView.Parent = teardown_TabPage;
                    activity_ToolStrip.Parent = teardown_TabPage;
                    RefreshGrid();
                    break;
            }
        }

        private ResourceExecutionPhase SelectedExecutionPhase
        {
            get
            {
                string tag = activity_TabControl.SelectedTab.Tag.ToString();
                return (ResourceExecutionPhase)Enum.Parse(typeof(ResourceExecutionPhase), tag);
            }
        }

        private IEnumerable<WorkerActivityConfiguration> ConfigurationByPhase
        {
            get
            {
                return _mainConfigurations.Where(x => x.ExecutionPlan.Phase == _selectedPhase);
            }
        }

        private void RefreshGrid()
        {
            int index = -1;

            // Get the currently selected row
            if (activity_GridView.SelectedRows.Count() > 0)
            {
                index = activity_GridView.SelectedRows[0].Index;
            }

            activity_GridView.DataSource = null;
            activity_GridView.DataSource = ConfigurationByPhase;
            activity_GridView.CurrentRow = null;

            var count = activity_GridView.Rows.Count();
            if (count > 0 && index >= 0 && index < count)
            {
                activity_GridView.ChildRows[index].IsSelected = true;
            }
            else if (count > 0)
            {
                activity_GridView.ChildRows[0].IsSelected = true;
            }
        }

        private void helpIcon_MouseHover(object sender, EventArgs e)
        {
            switch (SelectedExecutionMode)
            {
                case ExecutionMode.Duration:
                    DisplayHelpPopup(Resource.TimeBasedHelp);
                    break;
                case ExecutionMode.Iteration:
                    DisplayHelpPopup(Resource.CountBasedHelp);
                    break;
                case ExecutionMode.RateBased:
                    DisplayHelpPopup(Resource.RateBasedHelp);
                    break;
                case ExecutionMode.SetPaced:
                    DisplayHelpPopup(Resource.SetPacedHelp);
                    break;
                default:
                    DisplayHelpPopup(Resource.ScheduledHelp);
                    break;
            }
        }

        private void userPool_PictureBox_MouseHover(object sender, EventArgs e)
        {
            DisplayHelpPopup(Resource.UserPoolHelp);
        }

        private void activeDirectory_PictureBox_MouseHover(object sender, EventArgs e)
        {
            DisplayHelpPopup(Resource.ActiveDirectoryHelp);
        }

        private void DisplayHelpPopup(string helpString)
        {
            if (!_helpForm.Visible && !_helpForm.CanFocus)
            {
                _helpForm.HelpString = helpString;
                _helpForm.Location = new Point(Cursor.Position.X - _helpForm.Width, Cursor.Position.Y);
                _helpForm.ShowDialog(helpIcon);
            }
        }

        private void activityWorkflow_CheckedChange(object sender, EventArgs e)
        {
            var executionMode = SelectedExecutionMode;

            //Only update the data object if the value has changed
            if (executionMode != _officeWorker.ExecutionMode)
            {
                _officeWorker.ExecutionMode = executionMode;
            }
            DisplayControls(executionMode);
        }

        private void activityRetryHandlingToolStripButton_Click(object sender, EventArgs e)
        {
            if (!SelectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (ActivityRetryHandlingForm form = new ActivityRetryHandlingForm(SelectedRows))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshGrid();
                    }
                }
            }
        }

        private void removeRetryHandlingToolStripButton_Click(object sender, EventArgs e)
        {
            if (!SelectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                StringBuilder builder = new StringBuilder("Are you sure you want to remove retry handling from ");

                if (SelectedRows.Count() == 1)
                {
                    builder.Append("this activity?");
                }
                else
                {
                    builder.Append("these activities?");
                }

                if (MessageBox.Show(builder.ToString(), "Remove Activity Retry Handling", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    foreach (var row in SelectedRows)
                    {
                        row.RemoveAllRetrySettings();
                    }
                    RefreshGrid();
                }
            }
        }

        private void pacingToolStripButton_Click(object sender, EventArgs e)
        {
            var selectedRows = SelectedRows;

            // Check to see if there is anything selected
            if (!selectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRows != null)
            {
                using (WorkerActivityPacingForm form = new WorkerActivityPacingForm(selectedRows))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshGrid();
                    }
                }
            }
        }

        private IEnumerable<WorkerActivityConfiguration> SelectedRows
        {
            get
            {
                return activity_GridView.SelectedRows.Select<GridViewRowInfo, WorkerActivityConfiguration>(x => x.DataBoundItem as WorkerActivityConfiguration);
            }
            set { }
        }

        private void activity_GridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //pacing_GridViewColumn
            var column = activity_GridView.Columns[e.ColumnIndex].Name;
            if (column == "pacing_GridViewColumn")
            {
                if (e.CellElement.Value != null)
                {
                    var configuration = e.Row.DataBoundItem as WorkerActivityConfiguration;

                    if (!configuration.ExecutionPlan.ActivityPacing.Enabled)
                    {
                        e.CellElement.Text = string.Empty;
                    }
                    else
                    {
                        var builder = new StringBuilder();
                        var pacing = configuration.ExecutionPlan.ActivityPacing;

                        if (pacing.Randomize)
                        {
                            builder.AppendLine("Min: {0}".FormatWith(pacing.MinDelay));
                            builder.Append("Max: {0}".FormatWith(pacing.MaxDelay));
                        }
                        else
                        {
                            builder.Append("{0}".FormatWith(pacing.MinDelay));
                        }

                        if (configuration.ExecutionPlan.Value > 1)
                        {
                            if (configuration.ExecutionPlan.ActivityPacing.DelayOnRepeat)
                            {
                                builder.AppendLine();
                                builder.Append("Delay on Each");
                            }
                            else
                            {
                                builder.AppendLine();
                                builder.Append("Delay at End");
                            }
                        }
                        e.CellElement.Text = builder.ToString();
                    }
                }
            }
        }

        private void clearPacingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRows = SelectedRows.ToArray();

            // Check to see if there is anything selected
            if (!selectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!selectedRows.Any(x => x.ExecutionPlan.ActivityPacing.Enabled))
            {
                MessageBox.Show("No activities with Pacing enabled were selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            if (selectedRows.Length > 1)
            {
                DialogResult result = MessageBox.Show
                    ("Are you sure you want to clear Pacing for all rows and revert back to the default Pacing for this worker?",
                    "Confirm Clear Pacing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                var selected = selectedRows.First();

                DialogResult result = MessageBox.Show
                    ("Are you sure you want to clear Pacing for '{0}' and revert to the default Pacing for this worker?".FormatWith(selected.Name),
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }


            foreach (var item in selectedRows)
            {
                item.ExecutionPlan.ActivityPacing.Clear();
            }

            RefreshGrid();
        }

        private bool ShowRemoveErrorHandling()
        {
            return SelectedRows.All(x => x.Metadata.VirtualResourceMetadataRetrySettings != null && x.Metadata.VirtualResourceMetadataRetrySettings.Any());
        }

        private void activityGridContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            clearPacingToolStripMenuItem.Enabled = SelectedRows.Any(x => x.ExecutionPlan.ActivityPacing.Enabled);

            setRetryHandlingToolStripMenuItem.Enabled = true;
            removeRetryHandlingToolStripMenuItem.Enabled = ShowRemoveErrorHandling();
        }

        private void retryHandling_toolStripDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            setActivityRetryHandlingToolStripMenuItem.Enabled = true;
            removeActivityRetryHandlingToolStripMenuItem.Enabled = ShowRemoveErrorHandling();
        }

        private void setActivityExecutionCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRows = SelectedRows.ToList();

            // Check to see if there is anything selected
            if (!selectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int value = 1;
            using (var dialog = new InputDialog("Enter Activity Execution Count", "Activity Execution Count", "1"))
            {
                while (true)
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (!int.TryParse(dialog.Value, out value))
                        {
                            MessageBox.Show("Value is not a number");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (selectedRows.Count > 1)
            {
                DialogResult result = MessageBox.Show
                    ("Are you sure you want to set the Execution Count to {0} for all selected Activities?".FormatWith(value),
                    "Confirm Set Activity Execution Count", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                var selected = selectedRows.First();

                DialogResult result = MessageBox.Show
                    ("Are you sure you want to set the Execution Count to {0} for '{1}'?".FormatWith(value, selected.Name),
                    "Confirm Set Activity Execution Count", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            foreach (var item in selectedRows)
            {
                item.ExecutionPlan.Value = value;
            }

            RefreshGrid();
        }

        private void clearActivityExecutionCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRows = SelectedRows.ToList();

            // Check to see if there is anything selected
            if (!selectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRows.Count() > 1)
            {
                DialogResult result = MessageBox.Show
                    ("Are you sure you want to set the Execution Count back to 1 for these Activities?",
                    "Confirm Reset Activity Execution Count", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                var selected = selectedRows.First();

                DialogResult result = MessageBox.Show
                    ("Are you sure you want to set the Execution Count back to 1 for '{0}'?".FormatWith(selected.Name),
                    "Confirm Reset Activity Execution Count", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            foreach (var item in selectedRows)
            {
                item.ExecutionPlan.Value = 1;
            }

            RefreshGrid();
        }

        private void revealPictureBox_MouseHover(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = '\0';
        }

        private void revealPictureBox_MouseLeave(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = '•';
        }
    }
}
