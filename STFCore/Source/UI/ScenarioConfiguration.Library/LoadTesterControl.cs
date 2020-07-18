using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring an Load Tester virtual resource.
    /// </summary>
    public partial class LoadTesterControl : ScenarioConfigurationControlBase
    {
        private LoadTester _loadTester = null;
        private SortableBindingList<LoadTesterConfiguration> _configurations = null;
        private LoadTesterConfiguration _currentConfig = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadTesterControl"/> class.
        /// </summary>
        public LoadTesterControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(activity_GridView, GridViewStyle.Display);

            this.activity_GridView.TableElement.AlternatingRowColor = System.Drawing.Color.FromArgb(240, 240, 240);

            _configurations = new SortableBindingList<LoadTesterConfiguration>();
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Load Tester Configuration";
            }
        }

        /// <summary>
        /// Gets the type of the worker.
        /// </summary>
        protected virtual VirtualResourceType WorkerType
        {
            get { return VirtualResourceType.LoadTester; }
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _loadTester; }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            Initialize(new LoadTester());
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
            _loadTester = entity as LoadTester;
            if (_loadTester == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(LoadTester));
            }

            // Load the configurations into the helper class
            LoadTesterConfiguration config = null;
            foreach (var item in _loadTester.VirtualResourceMetadataSet)
            {
                config = new LoadTesterConfiguration();
                config.Metadata = item;

                if (item.ExecutionPlan == null)
                {
                    config.ExecutionPlan = new LoadTesterExecutionPlan();
                }
                else
                {
                    config.ExecutionPlan = LegacySerializer.DeserializeDataContract<LoadTesterExecutionPlan>(item.ExecutionPlan);
                }

                _configurations.Add(config);
            }

            // Load the activities into the binding list
            activity_GridView.DataSource = _configurations;
            activity_GridView.BestFitColumns();

            if (!GlobalSettings.IsDistributedSystem)
            {
                virtualMachinePlatform_ComboBox.Visible = false;
                platform_Label.Visible = false;
                _loadTester.Platform = "LOCAL";
            }
            else
            {
                virtualMachinePlatform_ComboBox.SetPlatform(_loadTester.Platform, WorkerType);
            }

            name_TextBox.DataBindings.Add("Text", _loadTester, "Name");
            description_TextBox.DataBindings.Add("Text", _loadTester, "Description");

            // Set up execution thread count for the current plan
            maxThreadsPerVM_NumericUpDown.ValueChanged -= maxThreadsPerVM_NumericUpDown_ValueChanged;
            maxThreadsPerVM_NumericUpDown.Maximum = 500;
            maxThreadsPerVM_NumericUpDown.Minimum = 1;
            maxThreadsPerVM_NumericUpDown.Value = _loadTester.ThreadsPerVM;
            maxThreadsPerVM_NumericUpDown.ValueChanged += maxThreadsPerVM_NumericUpDown_ValueChanged;

            CreateActivityDropDownMenu();

            if (_configurations.Count > 0)
            {
                // Select the first item in the list and then setup the databindings for that list.
                activity_GridView.Rows[0].IsSelected = true;
                var configuration = activity_GridView.Rows[0].DataBoundItem as LoadTesterConfiguration;
                SetupExecutionPlanBindings(configuration);
            }
        }

        private void CreateActivityDropDownMenu()
        {
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            foreach (MetadataType type in GetMetadataTypes(VirtualResourceType.LoadTester))
            {
                string typeName = type.Name;

                ToolStripMenuItem menuItem = new ToolStripMenuItem(type.Title);
                menuItem.Name = typeName + "_menuItem";
                menuItem.Image = IconManager.Instance.PluginIcons.Images[typeName];
                menuItem.Tag = ConfigurationObjectTag.Create(VirtualResourceType.LoadTester, typeName);
                menuItem.Click += new EventHandler(newActivity_MenuItem_Click);

                // Determine whether this item should be added to a sub-menu
                if (string.IsNullOrEmpty(type.Group))
                {
                    items.Add(menuItem);
                }
                else
                {
                    // See if the sub-menu alrady exists
                    ToolStripMenuItem subMenu = items.FirstOrDefault(n => n.Text == type.Group);
                    if (subMenu == null)
                    {
                        subMenu = new ToolStripMenuItem(type.Group);
                        items.Add(subMenu);
                    }
                    subMenu.DropDownItems.Add(menuItem);
                }
            }

            newActivity_ToolStripDropDownButton.DropDownItems.AddRange(items.OrderBy(n => n.Text).ToArray());
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
            foreach (var item in _loadTester.VirtualResourceMetadataSet)
            {
                if (string.IsNullOrEmpty(item.ExecutionPlan))
                {
                    var configItem = _configurations.FirstOrDefault(e => e.Metadata.VirtualResourceMetadataId == item.VirtualResourceMetadataId);
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

                    var configItem = _configurations.FirstOrDefault(e => e.Metadata.VirtualResourceMetadataId == item.VirtualResourceMetadataId);
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

            if (GlobalSettings.IsDistributedSystem)
            {
                // Need to grab the platform directly from the combobox
                _loadTester.Platform = ((FrameworkClientPlatform)virtualMachinePlatform_ComboBox.SelectedItem).FrameworkClientPlatformId;
            }
        }

        private void SetupExecutionPlanBindings(LoadTesterConfiguration configuration)
        {
            _currentConfig = configuration;

            SuspendLayout();

            currentPlan_Label.Text = "Execution Plan for: '{0}'".FormatWith(configuration.Name);
            currentActivity_Panel.Enabled = true;

            SelectTab(_currentConfig.ExecutionPlan.Mode);
            SelectTab(_currentConfig.ExecutionPlan.RampUpMode);

            // Set up execution thread count for the current plan
            threadCount_NumericUpDown.ValueChanged -= threadCount_NumericUpDown_ValueChanged;
            threadCount_NumericUpDown.Maximum = 500;
            threadCount_NumericUpDown.Minimum = 1;
            threadCount_NumericUpDown.Value = _currentConfig.ThreadCount;
            threadCount_NumericUpDown.ValueChanged += threadCount_NumericUpDown_ValueChanged;

            // Set up the number of threads to start if the ramp up is in a rate based mode
            rampUpThreadCount_NumericUpDown.ValueChanged -= rampUpThreadCount_NumericUpDown_ValueChanged;
            rampUpThreadCount_NumericUpDown.Maximum = _currentConfig.ThreadCount;
            rampUpThreadCount_NumericUpDown.Minimum = 1;
            rampUpThreadCount_NumericUpDown.Value = (_currentConfig.ExecutionPlan.RampUpPacingThreads < _currentConfig.ThreadCount)
                                                  ? _currentConfig.ExecutionPlan.RampUpPacingThreads
                                                  : _currentConfig.ThreadCount;
            rampUpThreadCount_NumericUpDown.ValueChanged += rampUpThreadCount_NumericUpDown_ValueChanged;

            // Set the initial value and update it through the ValueChanged event
            rampUpThreads_TimeSpanControl.Value = _currentConfig.ExecutionPlan.RampUpPacingDelay;

            // Set the initial value and update it through the ValueChanged event
            rampUp_TimeDelayControl.Randomize = _currentConfig.ExecutionPlan.RandomizeRampUpDelay;
            rampUp_TimeDelayControl.MinDelay = TimeSpan.FromSeconds(_currentConfig.ExecutionPlan.MinRampUpDelay);
            rampUp_TimeDelayControl.MaxDelay = TimeSpan.FromSeconds(_currentConfig.ExecutionPlan.MaxRampUpDelay);

            // Clear, then reset the data bindings for the activity duration value
            duration_TextBox.DataBindings.Clear();
            Binding binding = new Binding("Text", _currentConfig.ExecutionPlan, "DurationTime");
            binding.Format += new ConvertEventHandler(ConvertTotalMinutesToHourMinute);
            binding.Parse += new ConvertEventHandler(ConvertHourMinuteToTotalMinutes);
            duration_TextBox.DataBindings.Add(binding);

            delayInterval_CheckBox.DataBindings.Clear();
            delayInterval_CheckBox.DataBindings.Add("Checked", _currentConfig.ExecutionPlan, "DelayOneInterval");

            // Set the initial value and update it through the ValueChanged event
            pacing_TimeDelayControl.Randomize = _currentConfig.ExecutionPlan.RandomizeActivityDelay;
            pacing_TimeDelayControl.MinDelay = TimeSpan.FromSeconds(_currentConfig.ExecutionPlan.MinActivityDelay);
            pacing_TimeDelayControl.MaxDelay = TimeSpan.FromSeconds(_currentConfig.ExecutionPlan.MaxActivityDelay);

            // Clear and rebind the repeat count for the activity.
            repeatCount_TextBox.DataBindings.Clear();
            repeatCount_TextBox.DataBindings.Add("Text", _currentConfig.ExecutionPlan, "RepeatCount");

            ResumeLayout();
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
                    form.Initialize(VirtualResourceMetadata.Select(context, VirtualResourceType.LoadTester));
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
            metadata.VirtualResourceId = _loadTester.VirtualResourceId;
            metadata.Enabled = true;
            //metadata.ExecutionOrder = _loadTester.VirtualResourceMetadataSet.Count + 1;
            _loadTester.VirtualResourceMetadataSet.Add(metadata);

            LoadTesterConfiguration config = new LoadTesterConfiguration();
            //config.Name = metadata.Name;
            //config.MetadataType = metadata.MetadataType;
            //config.ThreadCount = 1;
            config.Metadata = metadata;
            config.ExecutionPlan = new LoadTesterExecutionPlan();
            _configurations.Add(config);

            // Set the current plan to this new entry
            _currentConfig = config;
            activity_GridView.Rows[_configurations.Count - 1].IsSelected = true;

            switch (config.ExecutionPlan.Mode)
            {
                case ExecutionMode.RateBased:
                    execution_TabControl.SelectedTab = rateBased_TabPage;
                    break;
                case ExecutionMode.Duration:
                    execution_TabControl.SelectedTab = timeBased_TabPage;
                    break;
                case ExecutionMode.Poisson:
                    execution_TabControl.SelectedTab = poisson_TabPage;
                    break;
            }

            switch (config.ExecutionPlan.RampUpMode)
            {
                case RampUpMode.RateBased:
                    rampUp_TabControl.SelectedTab = timeBasedRampUp_TabPage;
                    break;
                case RampUpMode.TimeBased:
                    rampUp_TabControl.SelectedTab = rateBasedRampUp_TabPage;
                    break;
            }

            SetupExecutionPlanBindings(config);
        }

        private void ConvertHourMinuteToTotalMinutes(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.Parse(((string)e.Value)).TotalMinutes;
        }

        private void ConvertTotalMinutesToHourMinute(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.ToTimeSpanString((int)e.Value);
        }


        #region Validation Event Handlers

        private void name_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(name_TextBox.Text, "Name", name_Label, e);
        }

        private void virtualMachinePlatform_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                HasValue(virtualMachinePlatform_ComboBox.Text, "Platform", platform_Label, e);
            }
        }

        private void repeatCount_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (SelectedExecutionMode == ExecutionMode.Iteration)
            {
                ValidatePositiveInt(repeatCount_TextBox.Text, "Repeat Count", repeatCount_TextBox, e);
            }
        }

        private void duration_TextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateRegex(duration_TextBox.Text, "Duration", "^\\d+:\\d+$", "h:mm", duration_TextBox, e);
        }

        #endregion


        private void LoadTesterControl_Load(object sender, EventArgs e)
        {
        }

        private void deleteActivity_ToolStripButton_Click(object sender, EventArgs e)
        {
            // Check to see if there is anything selected
            if (!activity_GridView.SelectedRows.Any())
            {
                MessageBox.Show("No activities selected.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = activity_GridView.SelectedRows[0];
            var selected = selectedRow.DataBoundItem as LoadTesterConfiguration;

            DialogResult result = MessageBox.Show
                ("Are you sure you want to delete {0}?".FormatWith(selected.Name),
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                activity_GridView.Rows.Remove(selectedRow);

                // Set this so that the context knows to delete this object
                selected.Metadata.VirtualResourceId = Guid.Empty;

                //activity_GridView.Refresh();
                //activity_GridView.Parent.Refresh();
            }
        }

        private void SelectTab(ExecutionMode mode)
        {
            switch (mode)
            {
                case ExecutionMode.RateBased:
                    execution_TabControl.SelectedTab = rateBased_TabPage;
                    break;
                case ExecutionMode.Duration:
                    execution_TabControl.SelectedTab = timeBased_TabPage;
                    break;
                case ExecutionMode.Poisson:
                    execution_TabControl.SelectedTab = poisson_TabPage;
                    break;
            }

            DisplayControls(mode);
        }

        private void SelectTab(RampUpMode mode)
        {
            switch (mode)
            {
                case RampUpMode.TimeBased:
                    rampUp_TabControl.SelectedTab = timeBasedRampUp_TabPage;
                    break;
                case RampUpMode.RateBased:
                    rampUp_TabControl.SelectedTab = rateBasedRampUp_TabPage;
                    break;
            }
        }

        private void execution_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = SelectedExecutionMode;
            if (_currentConfig != null)
            {
                _currentConfig.ExecutionPlan.Mode = mode;
            }
            DisplayControls(mode);
        }

        private ExecutionMode SelectedExecutionMode
        {
            get
            {
                string tag = execution_TabControl.SelectedTab.Tag.ToString();
                return EnumUtil.Parse<ExecutionMode>(tag);
            }
        }

        private void rampUp_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = SelectedRampUpMode;
            if (_currentConfig != null)
            {
                _currentConfig.ExecutionPlan.RampUpMode = mode;
            }
        }

        private RampUpMode SelectedRampUpMode
        {
            get
            {
                string tag = rampUp_TabControl.SelectedTab.Tag.ToString();
                return EnumUtil.Parse<RampUpMode>(tag);
            }
        }

        private void DisplayControls(ExecutionMode mode)
        {
            // Change parent controls for some of the children based on the
            // tab page that is to be displayed.
            switch (mode)
            {
                case ExecutionMode.Duration:
                    duration_TextBox.Parent = timeBased_TabPage;
                    pacing_TimeDelayControl.Parent = timeBased_TabPage;
                    threadCount_Label.Text = "Thread Count";
                    rampUp_TabControl.Visible = true;
                    poisson_Label.Visible = false;
                    poissonSample_LinkLabel.Visible = false;
                    break;
                case ExecutionMode.RateBased:
                    duration_TextBox.Parent = rateBased_TabPage;
                    threadCount_Label.Text = "Thread Count";
                    rampUp_TabControl.Visible = true;
                    poisson_Label.Visible = false;
                    poissonSample_LinkLabel.Visible = false;
                    break;
                case ExecutionMode.Poisson:
                    duration_TextBox.Parent = poisson_TabPage;
                    pacing_TimeDelayControl.Parent = poisson_TabPage;
                    threadCount_Label.Text = "Max Poisson Thread Count";
                    rampUp_TabControl.Visible = false;
                    poisson_Label.Visible = true;
                    poissonSample_LinkLabel.Visible = true;
                    break;
            }
        }

        private void activity_GridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var configuration = activity_GridView.Rows[e.RowIndex].DataBoundItem as LoadTesterConfiguration;

                if (e.ColumnIndex == 3)
                {
                    configuration.Enabled = !configuration.Enabled;
                }

                SetupExecutionPlanBindings(configuration);

                //currentActivity_Panel.Enabled = true;
            }
        }

        private void rampUpThreads_TimeSpanControl_ValueChanged(object sender, EventArgs e)
        {
            _currentConfig.ExecutionPlan.RampUpPacingDelay = rampUpThreads_TimeSpanControl.Value;
        }

        private void rampUp_TimeDelayControl_ValueChanged(object sender, EventArgs e)
        {
            if (_currentConfig != null)
            {
                _currentConfig.ExecutionPlan.RandomizeRampUpDelay = rampUp_TimeDelayControl.Randomize;
                _currentConfig.ExecutionPlan.MinRampUpDelay = (int)rampUp_TimeDelayControl.MinDelay.TotalSeconds;
                _currentConfig.ExecutionPlan.MaxRampUpDelay = (int)rampUp_TimeDelayControl.MaxDelay.TotalSeconds;
            }
        }

        private void pacing_TimeDelayControl_ValueChanged(object sender, EventArgs e)
        {
            if (_currentConfig != null)
            {
                _currentConfig.ExecutionPlan.RandomizeActivityDelay = pacing_TimeDelayControl.Randomize;
                _currentConfig.ExecutionPlan.MinActivityDelay = (int)pacing_TimeDelayControl.MinDelay.TotalSeconds;
                _currentConfig.ExecutionPlan.MaxActivityDelay = (int)pacing_TimeDelayControl.MaxDelay.TotalSeconds;
            }
        }

        private void threadCount_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_currentConfig != null)
            {
                _currentConfig.ThreadCount = (int)threadCount_NumericUpDown.Value;
                rampUpThreadCount_NumericUpDown.Maximum = _currentConfig.ThreadCount;

                BindingContext[activity_GridView.DataSource].EndCurrentEdit();
                BindingContext[activity_GridView.DataSource, "ThreadCount"].EndCurrentEdit();
                activity_GridView.Refresh();
                activity_GridView.Parent.Refresh();
                //activity_GridView.Refresh();
            }
        }

        private void maxThreadsPerVM_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_loadTester != null)
            {
                _loadTester.ThreadsPerVM = (int)maxThreadsPerVM_NumericUpDown.Value;
            }
        }

        private void rampUpThreadCount_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_currentConfig != null)
            {
                _currentConfig.ExecutionPlan.RampUpPacingThreads = (int)rampUpThreadCount_NumericUpDown.Value;
            }
        }

        private void copyActivity_ToolStripButton_Click_1(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                using (VirtualResourceMetadataListForm form = new VirtualResourceMetadataListForm())
                {
                    form.Initialize(VirtualResourceMetadata.Select(context, VirtualResourceType.LoadTester));
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

        private void poissonSample_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var duration = TimeSpan.FromMinutes(_currentConfig.ExecutionPlan.DurationTime);

            using (PoissonDistributionViewForm form = new PoissonDistributionViewForm(_currentConfig.ThreadCount, duration))
            {
                form.ShowDialog();
            }
        }
    }
}
