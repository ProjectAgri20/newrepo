using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.WindowsAutomation;
using HP.ScalableTest.Xml;
using System.Net;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// the PerfMon Collector UI
    /// </summary>
    public partial class PerfMonCollectorControl : ScenarioConfigurationControlBase
    {
        private PerfMonCollector _perfMonCollector = null;
        private SortableBindingList<PerfMonCounterData> _selectedCountersDataList = new SortableBindingList<PerfMonCounterData>();
        private FrameworkServer _selectedServer;
        private bool _loadFromMachine = true;
        private const string PrintQueueCategory = "Print Queue";

        /// <summary>
        /// constructor
        /// </summary>
        public PerfMonCollectorControl()
        {
            InitializeComponent();
            selectedCounters_DataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "PerfMon Collector Configuration";
            }
        }

        public override EntityObject EntityObject
        {
            get
            {
                return _perfMonCollector;
            }
        }

        /// <summary>
        /// Initialise a new form, and set the virtual resource name
        /// </summary>
        public override void Initialize()
        {
            PerfMonCollector newEntity = new PerfMonCollector();
            newEntity.Name = "PerfMonCollector_" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.CurrentCulture);
            Initialize(newEntity);
        }

        /// <summary>
        /// Initialise using the saved data
        /// </summary>
        /// <param name="entity"></param>
        public override void Initialize(object entity)
        {
            _perfMonCollector = entity as PerfMonCollector;
            if (_perfMonCollector == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(PerfMonCollector));
            }

            //Bind to the controls that make sense
            name_TextBox.DataBindings.Add("Text", _perfMonCollector, "Name");

            PopulateServerList();

            //browse throught the virtual resource metadata and add them to our listbox
            foreach (VirtualResourceMetadata vmdata in _perfMonCollector.VirtualResourceMetadataSet)
            {
                PerfMonCounterData tempCounter = LegacySerializer.DeserializeXml<PerfMonCounterData>(vmdata.Metadata);
                tempCounter.VirtualResourceMetadataId = vmdata.VirtualResourceMetadataId;
                _selectedCountersDataList.Add(tempCounter);
            }

            platform_ComboBox.SetPlatform(_perfMonCollector.Platform, VirtualResourceType.PerfMonCollector);

            LoadSystemSetting();

            selectedCounters_DataGridView.DataSource = _selectedCountersDataList;
        }

        /// <summary>
        /// to update the data bindings
        /// </summary>
        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_Label.Focus();

            // Need to grab the platform directly from the combobox
            if (platform_ComboBox.SelectedIndex > -1)
            {
                _perfMonCollector.Platform = platform_ComboBox.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Returns the Selected Category.
        /// </summary>
        private object SelectedCategory
        {
            get { return category_ListBox.SelectedItem; }
        }

        /// <summary>
        /// Returns the selected Instance name.
        /// </summary>
        private string SelectedInstance
        {
            get { return instance_ListBox.SelectedItem.ToString(); }
        }

        /// <summary>
        /// Returns the Selected Counter
        /// </summary>
        private object SelectedCounter
        {
            get { return counter_ListBox.SelectedItem; }
        }

        private void PopulateServerList()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                string serverType = ServerType.PerfMon.ToString();
                var printservers = context.FrameworkServers.Where(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active).OrderBy(n => n.HostName);
                server_ListBox.DataSource = printservers.ToList();
                server_ListBox.SelectedIndex = -1;
                server_ListBox.SelectedIndexChanged += server_ListBox_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// Retrieve the setting from DB, check if the user wants to load data real time or from DB
        /// </summary>
        private void LoadSystemSetting()
        {
            string counterSetting = GlobalSettings.Items["PerfMonCountersDB"];
            _loadFromMachine = (String.Equals(counterSetting, "false", StringComparison.OrdinalIgnoreCase));
        }

        private void addCounter_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                PerfMonCounterData tempCounterData = new PerfMonCounterData();

                if (_loadFromMachine)
                {
                    PerformanceCounter selectedCounter = SelectedCounter as PerformanceCounter;
                    tempCounterData.Category = selectedCounter.CategoryName;
                    tempCounterData.Counter = selectedCounter.CounterName;
                    tempCounterData.InstanceName = selectedCounter.InstanceName;
                }
                else
                {
                    string selectedInstance = SelectedInstance;
                    tempCounterData.Category = SelectedCategory.ToString();
                    tempCounterData.Counter = ((ResourceWindowsCategory)SelectedCounter).Name;
                    /// Don't insert "N/A" into the counter data, use empty string instead.
                    tempCounterData.InstanceName = (selectedInstance == PerfMonController.InstanceDoesNotApply) ? string.Empty : selectedInstance;
                }

                tempCounterData.TargetHost = _selectedServer.HostName;

                //if the user has entered the username and password then use it
                if (!string.IsNullOrEmpty(userName_textBox.Text) || !string.IsNullOrEmpty(password_textBox.Text))
                {
                    tempCounterData.Credentials = new PerfMonCounterCredential(userName_textBox.Text, password_textBox.Text, string.IsNullOrEmpty(domain_textBox.Text) ? "." : domain_textBox.Text);
                }
                else
                {
                    tempCounterData.Credentials = new PerfMonCounterCredential();
                }

                tempCounterData.Interval = interval_TimeSpanControl.Value.TotalMilliseconds;

                VirtualResourceMetadata tempMetaData = new VirtualResourceMetadata(VirtualResourceType.PerfMonCollector.ToString(), "PerfMonCounter");

                //associate the GUID of the tempMetaData to the perfmoncounterdata item
                tempCounterData.VirtualResourceMetadataId = tempMetaData.VirtualResourceMetadataId;

                tempMetaData.VirtualResourceId = _perfMonCollector.VirtualResourceId;
                tempMetaData.Name = tempCounterData.TargetHost + "-" + tempCounterData.Category + "/" + tempCounterData.InstanceName + "/" + tempCounterData.Counter;
                string metadataxml = LegacySerializer.SerializeXml(tempCounterData).ToString();
                tempMetaData.Metadata = metadataxml;

                //we are not currently connected to DB or using the existing data, so add this to the virtual resource metadata collection
                _perfMonCollector.VirtualResourceMetadataSet.Add(tempMetaData);
                _perfMonCollector.Platform = (string)platform_ComboBox.SelectedValue;

                //populate the listview in the newly added item
                _selectedCountersDataList.Add(tempCounterData);
            }
        }

        /// <summary>
        /// Checks each listbox to ensure a selection in each.
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateSelection()
        {
            bool isValid = false;

            //validate the timespan interval
            if (interval_TimeSpanControl.Value.TotalSeconds < 5)
            {
                fieldValidator.SetError(interval_groupBox, "Please select an interval of at least 5 seconds");
            }
            else
            {
                fieldValidator.SetError(interval_groupBox, string.Empty);
                isValid = true;
            }
            return isValid;
        }

        private void deleteCounter_TooStripButton_Click(object sender, EventArgs e)
        {
            if (selectedCounters_DataGridView.SelectedRows.Count < 1)
            {
                fieldValidator.SetError(counters_ToolStrip, "Please select the counter to remove from the below list");
                return;
            }
            else
            {
                fieldValidator.SetError(counters_ToolStrip, string.Empty);
            }

            var selectedRow = selectedCounters_DataGridView.SelectedRows[0].DataBoundItem as PerfMonCounterData;

            var delMetaData = _perfMonCollector.VirtualResourceMetadataSet.Single(c => c.VirtualResourceMetadataId == selectedRow.VirtualResourceMetadataId);
            if (_perfMonCollector.VirtualResourceMetadataSet.Remove(delMetaData))
            {
                delMetaData.VirtualResourceId = Guid.Empty;
                _selectedCountersDataList.Remove(selectedRow);
            }
        }

        private void category_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (category_ListBox.SelectedIndex > -1)
            {
                instance_ListBox.SelectedIndexChanged -= instance_ListBox_SelectedIndexChanged;
                instance_ListBox.DataSource = null;

                if (_loadFromMachine)
                {
                    LoadInstances(SelectedCategory as PerformanceCounterCategory);
                }
                else
                {
                    LoadInstances(SelectedCategory.ToString(), _selectedServer.HostName);
                }
                instance_ListBox.SelectedIndexChanged += instance_ListBox_SelectedIndexChanged;
            }
        }

        private void instance_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCounter_ToolStripButton.Enabled = false;

            if (instance_ListBox.SelectedIndex < 0)
            {
                return;
            }

            counter_ListBox.DataSource = null;
            counter_ListBox.Items.Clear();
            counter_ListBox.SelectedIndexChanged -= counter_ListBox_SelectedIndexChanged;
            addCounter_ToolStripButton.Enabled = false;

            if (_loadFromMachine)
            {
                LoadCounters(_selectedServer.HostName);
            }
            else
            {
                if (SelectedCategory.ToString() == PrintQueueCategory)
                {
                    LoadCounters(PrintQueueCategory);
                }
                else
                {
                    LoadCounters(SelectedInstance);
                }
            }
            counter_ListBox.SelectedIndexChanged += counter_ListBox_SelectedIndexChanged;
        }

        private void server_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (server_ListBox.SelectedIndex > -1)
            {
                //clear out existing values
                category_ListBox.SelectedIndexChanged -= category_ListBox_SelectedIndexChanged;
                category_ListBox.DataSource = null;
                instance_ListBox.DataSource = null;
                counter_ListBox.DataSource = null;

                category_ListBox.Items.Clear();
                instance_ListBox.Items.Clear();
                counter_ListBox.Items.Clear();

                addCounter_ToolStripButton.Enabled = false;

                _selectedServer = server_ListBox.SelectedItem as FrameworkServer;

                Cursor = Cursors.WaitCursor;
                LoadCategories(_selectedServer.HostName);
                Cursor = Cursors.Default;
            }
        }

        private void LoadCategories(string machineName)
        {
            if (_loadFromMachine)
            {
                try
                {
                    if (!string.IsNullOrEmpty(userName_textBox.Text))
                    {
                        NetworkCredential networkCredential = new NetworkCredential(userName_textBox.Text, password_textBox.Text, string.IsNullOrEmpty(domain_textBox.Text) ? "." : domain_textBox.Text);
                        UserImpersonator.Execute(() => LoadCategoriesImpl(machineName), networkCredential);
                    }
                    else
                    {
                        LoadCategoriesImpl(machineName);
                    }
                }
                catch (Win32Exception w32Exception)
                {
                    TraceFactory.Logger.Error(w32Exception);
                    MessageBox.Show("Could not access counters.  Please ensure the following services are running on remote machine: " + Environment.NewLine +
                                  "1. Performance Logs & Alerts" + Environment.NewLine +
                                  "2. Remote Registry. " + w32Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    server_ListBox.SelectedIndex = -1;
                    return;
                }
                catch (UnauthorizedAccessException unauthException)
                {
                    TraceFactory.Logger.Error(unauthException);
                    MessageBox.Show("Unable to access host, please check the user credentials provided.");
                    server_ListBox.SelectedIndex = -1;
                    return;
                }
                catch (ArgumentException argException)
                {
                    TraceFactory.Logger.Error(argException);
                    MessageBox.Show("Could not access counters.  Please ensure the following services are running on remote machine: " + Environment.NewLine +
                                    "1. Performance Logs & Alerts" + Environment.NewLine +
                                    "2. Remote Registry. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    server_ListBox.SelectedIndex = -1;
                    return;
                }
            }
            else
            {
                //Load categories from the database.
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    category_ListBox.Items.Clear();
                    var categoryTop = ResourceWindowsCategory.Select(context, "PerfMon").FirstOrDefault(c => string.IsNullOrEmpty(c.Name));

                    foreach (var counter in categoryTop.Children)
                    {
                        category_ListBox.Items.Add(counter.Name);
                    }

                    category_ListBox.SelectedIndex = -1;
                    category_ListBox.SelectedIndexChanged += category_ListBox_SelectedIndexChanged;
                }
            }
        }

        private void LoadCategoriesImpl(string machineName)
        {
            Collection<PerformanceCounterCategory> categories = PerfMonController.GetCategories(machineName);
            category_ListBox.DataSource = categories;
            category_ListBox.DisplayMember = "CategoryName";
            category_ListBox.SelectedIndex = -1;
            category_ListBox.SelectedIndexChanged += category_ListBox_SelectedIndexChanged;
        }

        private void LoadInstances(PerformanceCounterCategory category)
        {
            Collection<string> instances = PerfMonController.GetInstances(category);

            instance_ListBox.DataSource = instances;
            instance_ListBox.SelectedIndex = -1;

            counter_ListBox.DataSource = null;
            addCounter_ToolStripButton.Enabled = false;
        }

        private void LoadInstances(string categoryName, string machineName)
        {
            instance_ListBox.Items.Clear();

            if (categoryName == PrintQueueCategory)
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var queues = context.RemotePrintQueues.Where(n => n.PrintServer.HostName.Equals(machineName, StringComparison.OrdinalIgnoreCase)).OrderBy(n => n.Name);
                    foreach (RemotePrintQueue queue in queues)
                    {
                        instance_ListBox.Items.Add(queue.Name);
                    }
                }
            }
            else
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    var category = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.PerfMon.ToString()).Where(c => c.Name == categoryName).First();

                    foreach (ResourceWindowsCategory instance in category.Children)
                    {
                        instance_ListBox.Items.Add(instance.Name);
                    }
                }
            }


            //If there's only one instance, automatically select it and load the counters
            if (instance_ListBox.Items.Count == 1)
            {
                instance_ListBox.SelectedIndex = 0;
                instance_ListBox_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                instance_ListBox.SelectedIndex = -1;
                counter_ListBox.DataSource = null;
            }

            addCounter_ToolStripButton.Enabled = false;
        }

        private void LoadCounters(string machineName)
        {
            if (_loadFromMachine)
            {
                string instanceName = SelectedInstance;
                PerformanceCounterCategory category = SelectedCategory as PerformanceCounterCategory;
                Collection<PerformanceCounter> counters = PerfMonController.GetCounters(category, instanceName);
                counter_ListBox.DataSource = counters;
            }
            else
            {
                ResourceWindowsCategory instance = null;
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    instance = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.PerfMon.ToString()).Where(c => c.Name == machineName).FirstOrDefault();
                }
                counter_ListBox.DataSource = instance.Children;
            }

            counter_ListBox.DisplayMember = "Name";
            counter_ListBox.SelectedIndex = -1;
        }

        private void counter_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (counter_ListBox.SelectedIndex > -1)
                addCounter_ToolStripButton.Enabled = true;
        }

        private void server_ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = server_ListBox.IndexFromPoint(e.Location);

            if (index >= 0 && index < server_ListBox.Items.Count)
            {
                if (!toolTipServer.GetToolTip(server_ListBox).Contains(server_ListBox.Items[index].ToString()))
                {
                    FrameworkServer hoveredItem = server_ListBox.Items[index] as FrameworkServer;
                    toolTipServer.SetToolTip(server_ListBox, hoveredItem.HostName + ", " +
                                                             hoveredItem.Architecture + ", " +
                                                             hoveredItem.Cores.ToString(CultureInfo.CurrentCulture) + "x" +
                                                             hoveredItem.Processors.ToString(CultureInfo.CurrentCulture) + ", " +
                                                             hoveredItem.Memory.ToString(CultureInfo.CurrentCulture) + ", " +
                                                             hoveredItem.OperatingSystem);
                }
            }
        }

        private void platform_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            HasSelected(platform_ComboBox.Text, platform_Label.Text, platform_Label, e);
        }
    }
}