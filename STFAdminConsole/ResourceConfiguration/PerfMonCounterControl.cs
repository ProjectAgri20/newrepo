using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Control for selecting PerfMon counters on local or remote hosts.
    /// </summary>
    public partial class PerfMonCounterControl : FieldValidatedControl
    {
        /// <summary>
        /// ItemSelected event.
        /// </summary>
        public event EventHandler OnItemSelected;

        /// <summary>
        /// Initializes an instance of PerfMonCounterControl
        /// </summary>
        public PerfMonCounterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the control with the list of servers to connect to.
        /// </summary>
        /// <param name="serverList">The list of server names to connect to.</param>
        public void Initialize(IEnumerable<string> serverList)
        {
            if (serverList == null)
            {
                throw new ArgumentNullException("serverList");
            }

            // Note: Unable to use data binding here because of the need to dynamically add to the list of items. (See LoadCategories)
            server_ListBox.Items.Clear();
            foreach (string item in serverList)
            {
                server_ListBox.Items.Add(item);
            }

            server_ListBox.SelectedIndex = -1;
            server_ListBox.SelectedIndexChanged += new EventHandler(server_ListBox_SelectedIndexChanged);
        }

        /// <summary>
        /// Returns the Selected Item. Host, Category, Instance and Counter.
        /// </summary>
        public PerfMonCounterData SelectedItem
        {
            get
            {
                if (ValidateSelection())
                {
                    PerfMonCounterData result = new PerfMonCounterData();
                    result.TargetHost = SelectedServer;

                    PerformanceCounterCategory category = SelectedCategory;
                    PerformanceCounter counter = SelectedCounter;
                    result.InstanceName = counter.InstanceName;
                    if (category != null)
                    {
                        result.Category = category.CategoryName;
                    }
                    if (counter != null)
                    {
                        result.Counter = counter.CounterName;
                    }

                    return result;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the Selected Server name.
        /// </summary>
        private string SelectedServer
        {
            get { return server_ListBox.SelectedItem as string; }
        }

        /// <summary>
        /// Returns the Selected Category.
        /// </summary>
        private PerformanceCounterCategory SelectedCategory
        {
            get { return category_ListBox.SelectedItem as PerformanceCounterCategory; }
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
        private PerformanceCounter SelectedCounter
        {
            get { return counter_ListBox.SelectedItem as PerformanceCounter; }
        }

        private void server_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clear out existing values
            category_ListBox.DataSource = null;
            instance_ListBox.DataSource = null;
            counter_ListBox.DataSource = null;

            if (server_ListBox.SelectedIndex > -1)
            {
                category_ListBox.SelectedIndexChanged -= category_ListBox_SelectedIndexChanged;

                Cursor = Cursors.WaitCursor;
                LoadCategories(SelectedServer);
                category_ListBox.SelectedIndexChanged += category_ListBox_SelectedIndexChanged;
                Cursor = Cursors.Default;
            }
        }

        private void LoadCategories(string machineName)
        {
            Collection<PerformanceCounterCategory> categories = null;

            try
            {
                categories = PerfMonController.GetCategories(machineName);

                category_ListBox.DataSource = categories;
                category_ListBox.DisplayMember = "CategoryName";
                category_ListBox.SelectedIndex = -1;
            }
            catch (Win32Exception w32Exception)
            {
                TraceFactory.Logger.Error(w32Exception);
                DialogResult dialogResult = MessageBox.Show("Could not load categories for {0}. \nLoad from local machine?".FormatWith(machineName),
                    "Unreachable Host", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    //This will cause the SelectedIndex value to change which will indirectly call LoadCategories again.
                    LoadLocalHost();
                }
            }
            catch (UnauthorizedAccessException unauthException)
            {
                TraceFactory.Logger.Error(unauthException);
                MessageBox.Show("Failed to login to {0}. \nPlease check the user credentials being used.".FormatWith(machineName), "Authorization Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void category_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            instance_ListBox.DataSource = null;
            counter_ListBox.DataSource = null;

            if (category_ListBox.SelectedIndex > -1)
            {
                instance_ListBox.SelectedIndexChanged -= instance_ListBox_SelectedIndexChanged;
                LoadInstances((category_ListBox.SelectedItem as PerformanceCounterCategory));
                instance_ListBox.SelectedIndexChanged += instance_ListBox_SelectedIndexChanged;
            }
        }

        private void LoadInstances(PerformanceCounterCategory category)
        {
            Collection<string> instances = PerfMonController.GetInstances(category);

            instance_ListBox.DataSource = instances;

            //If there's only one instance, automatically select it and load the counters
            if (instances.Count == 1)
            {
                instance_ListBox.SelectedIndex = 0;
                instance_ListBox_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                instance_ListBox.SelectedIndex = -1;
            }
        }

        private void instance_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            counter_ListBox.DataSource = null;

            if (instance_ListBox.SelectedIndex > -1)
            {
                LoadCounters();
            }
        }

        private void LoadCounters()
        {
            PerformanceCounterCategory category = category_ListBox.SelectedItem as PerformanceCounterCategory;
            Collection<PerformanceCounter> counters = PerfMonController.GetCounters(category, SelectedInstance);

            counter_ListBox.DataSource = counters;
            counter_ListBox.DisplayMember = "CounterName";
            counter_ListBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Checks each listbox to ensure a selection in each.
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateSelection()
        {
            CancelEventArgs cancelEventArg = new CancelEventArgs();
            bool isValid = true;

            HasSelected(server_ListBox.Text, server_Label.Text, server_Label, cancelEventArg);
            isValid = isValid && !cancelEventArg.Cancel;
            HasSelected(category_ListBox.Text, category_Label.Text, category_Label, cancelEventArg);
            isValid = isValid && !cancelEventArg.Cancel;
            HasSelected(instance_ListBox.Text, instance_Label.Text, instance_Label, cancelEventArg);
            isValid = isValid && !cancelEventArg.Cancel;
            HasSelected(counter_ListBox.Text, counter_Label.Text, counter_Label, cancelEventArg);
            isValid = isValid && !cancelEventArg.Cancel;

            return isValid;
        }

        /// <summary>
        /// Loads the local host machine name into the server's listbox, then selects it.
        /// If it already exists, just selects it.
        /// </summary>
        private void LoadLocalHost()
        {
            string localHostName = Environment.MachineName;

            if (!server_ListBox.Items.Contains(localHostName))
            {
                server_ListBox.Items.Add(localHostName);
            }

            server_ListBox.SelectedItem = localHostName;
        }

        private void counter_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnItemSelected != null)
            {
                OnItemSelected(this, new EventArgs());
            }
        }
    }
}