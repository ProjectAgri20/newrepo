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
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Xml;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring an Admin Worker virtual resource.
    /// </summary>
    public partial class AdminWorkerControl : ScenarioConfigurationControlBase
    {
        private AdminWorker _adminWorker = null;
        private SortableBindingList<WorkerActivityConfiguration> _mainConfigurations = null;
        private ResourceExecutionPhase _selectedPhase = ResourceExecutionPhase.Main;


        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorkerControl"/> class.
        /// </summary>
        public AdminWorkerControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(activity_GridView, GridViewStyle.Display);

            _mainConfigurations = new SortableBindingList<WorkerActivityConfiguration>();
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Admin Worker Configuration";
            }
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _adminWorker; }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            Initialize(new AdminWorker());
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
            _adminWorker = entity as AdminWorker;
            if (_adminWorker == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(AdminWorker));
            }

            // Load the configurations into the helper class
            foreach (var item in _adminWorker.VirtualResourceMetadataSet)
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

            // Set up data bindings
            name_TextBox.DataBindings.Add("Text", _adminWorker, "Name");
            description_TextBox.DataBindings.Add("Text", _adminWorker, "Description");

            platform_ComboBox.SetPlatform(_adminWorker.Platform, VirtualResourceType.AdminWorker);
            testcaseid_numericUpDown.Value = _adminWorker.TestCaseId;
            ConfigureEnableAllButton();
            CreateActivityDropDownMenu();

            RefreshGrid();
        }

        private void ConfigureEnableAllButton()
        {
            // If they are all enabled, change the button to say "Disable All"
            if (PhaseConfigurations.All(n => n.Metadata.Enabled))
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


        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_TextBox.Focus();

            // Push the configuration data into the ExecutionPlan property of the metadata
            foreach (var item in _adminWorker.VirtualResourceMetadataSet)
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

            // Need to grab the platform directly from the combobox
            _adminWorker.Platform = ((FrameworkClientPlatform)platform_ComboBox.SelectedItem).FrameworkClientPlatformId;
            _adminWorker.TestCaseId = Convert.ToInt32(testcaseid_numericUpDown.Value);
        }

        private void CreateActivityDropDownMenu()
        {
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            foreach (MetadataType type in GetMetadataTypes(VirtualResourceType.AdminWorker))
            {
                string typeName = type.Name;
                ToolStripMenuItem menuItem = null;
                try
                {
                    menuItem = new ToolStripMenuItem(type.Title);
                    menuItem.Name = typeName + "_menuItem";
                    menuItem.Image = IconManager.Instance.PluginIcons.Images[typeName];
                    menuItem.Tag = ConfigurationObjectTag.Create(VirtualResourceType.AdminWorker, typeName);
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
                    form.Initialize(VirtualResourceMetadata.Select(context, VirtualResourceType.AdminWorker));
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
            metadata.VirtualResourceId = _adminWorker.VirtualResourceId;
            metadata.Enabled = true;
            //metadata.ExecutionOrder = _officeWorker.VirtualResourceMetadataSet.Count + 1;
            _adminWorker.VirtualResourceMetadataSet.Add(metadata);

            WorkerActivityConfiguration config = new WorkerActivityConfiguration(metadata, new WorkerExecutionPlan());
            config.ExecutionPlan.Order = PhaseConfigurations.Count() > 0 ? PhaseConfigurations.Max(x => x.ExecutionPlan.Order) + 1 : 1;

            if (activity_TabControl.SelectedTab == main_TabPage)
            {
                config.ExecutionPlan.Phase = ResourceExecutionPhase.Main;
            }
            else if (activity_TabControl.SelectedTab == setup_TabPage)
            {
                config.ExecutionPlan.Phase = ResourceExecutionPhase.Setup;
            }
            else
            {
                config.ExecutionPlan.Phase = ResourceExecutionPhase.Teardown;
            }

            _mainConfigurations.Add(config);

            RefreshGrid();

            // Set the current plan to this new entry
            activity_GridView.ChildRows[PhaseConfigurations.Count() - 1].IsSelected = true;
        }

        private void reorder_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (WorkerActivityOrderForm form = new WorkerActivityOrderForm())
            {
                form.Initialize(PhaseConfigurations.ToList());
                form.ShowDialog(this);
                RefreshGrid();
            }
        }

        private void enableAll_ToolStripButton_Click(object sender, EventArgs e)
        {
            // Set a flag that will tell us what to do with all the resources
            bool newState = enableAll_ToolStripButton.Text == "Enable All";

            foreach (var config in PhaseConfigurations)
            {
                config.Metadata.Enabled = newState;
            }
            ConfigureEnableAllButton();
            RefreshGrid();
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
            var selected = selectedRow.DataBoundItem as WorkerActivityConfiguration;

            DialogResult result = MessageBox.Show
                ("Are you sure you want to delete {0}?".FormatWith(selected.Name),
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _adminWorker.VirtualResourceMetadataSet.Remove(selected.Metadata);
                _mainConfigurations.Remove(selected);

                // Set this so that the context knows to delete this object
                selected.Metadata.VirtualResourceId = Guid.Empty;

                // Reset the order number for each remaining entry
                int index = 1;
                foreach (var item in PhaseConfigurations.OrderBy(x => x.ExecutionPlan.Order))
                {
                    item.Order = index++;
                }

                RefreshGrid();
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

        private IEnumerable<WorkerActivityConfiguration> PhaseConfigurations
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
            activity_GridView.DataSource = PhaseConfigurations;
            activity_GridView.CurrentRow = null;

            var count = activity_GridView.ChildRows.Count();
            if (count > 0 && index >= 0 && index <= count)
            {
                activity_GridView.ChildRows[index].IsSelected = true;
            }
            else if (count > 0)
            {
                activity_GridView.ChildRows[0].IsSelected = true;
            }
        }

        private void activity_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectedExecutionPhase)
            {
                case ResourceExecutionPhase.Main:
                    _selectedPhase = ResourceExecutionPhase.Main;
                    activity_ToolStrip.Parent = main_TabPage;
                    activity_GridView.Parent = main_TabPage;

                    RefreshGrid();
                    break;
                case ResourceExecutionPhase.Setup:
                    _selectedPhase = ResourceExecutionPhase.Setup;
                    activity_ToolStrip.Parent = setup_TabPage;
                    activity_GridView.Parent = setup_TabPage;
                    RefreshGrid();
                    break;
                case ResourceExecutionPhase.Teardown:
                    _selectedPhase = ResourceExecutionPhase.Teardown;
                    activity_ToolStrip.Parent = teardown_TabPage;
                    activity_GridView.Parent = teardown_TabPage;
                    RefreshGrid();
                    break;
            }
        }

        #region Validation Event Handlers

        private void name_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(name_TextBox.Text, "Name", name_Label, e);
        }

        private void platform_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(platform_ComboBox.Text, "Platform", platform_Label, e);
        }

        #endregion

        private class ActivityListItem
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string ActivityType { get; private set; }

            public ActivityListItem(VirtualResourceMetadata metadata)
            {
                Id = metadata.VirtualResourceMetadataId;
                Name = metadata.Name;
                ActivityType = metadata.MetadataType;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
            public RadListDataItem GetDataItem()
            {
                return new RadListDataItem()
                {
                    Value = this,
                    Text = "{0} [{1}]".FormatWith(Name, ActivityType),
                    Image = IconManager.Instance.PluginIcons.Images[ActivityType]
                };
            }
        }

        private void activity_GridView_Click(object sender, EventArgs e)
        {

        }
    }
}
