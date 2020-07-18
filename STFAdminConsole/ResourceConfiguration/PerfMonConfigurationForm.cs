using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Utility for adding PerfMon counters to the EnterpriseTest database.
    /// </summary>
    public partial class PerfMonConfigurationForm : Form
    {
        PerfMonCounterControl _perfMonControl = null;
        SortableBindingList<PerfMonCounterData> _selectedCounters = new SortableBindingList<PerfMonCounterData>();
        
        /// <summary>
        /// Creates a new instance of PerfMonCongigurationForm.
        /// </summary>
        public PerfMonConfigurationForm()
        {
            InitializeComponent();
            InitializeControl();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            selectedCounters_DataGridView.AutoGenerateColumns = false;
            selectedCounters_DataGridView.DataSource = _selectedCounters;
        }

        /// <summary>
        /// Initialized the PerfMonCounterControl.
        /// </summary>
        private void InitializeControl()
        {
            HP.ScalableTest.Data.EnterpriseTest.FrameworkServerController _controller = new HP.ScalableTest.Data.EnterpriseTest.FrameworkServerController();
            IQueryable<FrameworkServer> servers = _controller.GetServersByType(ServerType.PerfMon).Where(p => p.Active == true);
            IQueryable<string> serverNames = from s in servers select s.HostName;

            _perfMonControl = new PerfMonCounterControl();
            _perfMonControl.Location = new Point(4, 4);
            _perfMonControl.Initialize(serverNames);
            //_perfMonControl.OnItemSelected += new EventHandler(_perfMonControl_OnItemSelected);
            this.Controls.Add(_perfMonControl);

        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            PerfMonCounterData selectedItem = _perfMonControl.SelectedItem;

            if (selectedItem != null && CanAdd(selectedItem))
            {
                _selectedCounters.Add(selectedItem);
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            if (_selectedCounters.Count > 0)
            {
                if (selectedCounters_DataGridView.SelectedRows.Count > 0)
                {
                    PerfMonCounterData dataItem = selectedCounters_DataGridView.SelectedRows[0].DataBoundItem as PerfMonCounterData;
                    if (dataItem != null)
                    {
                        _selectedCounters.Remove(dataItem);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a counter to remove.", "Remove Selected Counter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void insert_Button_Click(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (PerfMonCounterData counter in _selectedCounters)
                {
                    ResourceWindowsCategory.AddPerfMon(context, counter.Category, counter.InstanceName, counter.Counter, ResourceWindowsCategoryType.PerfMon);
                }
                context.SaveChanges();
            }
            MessageBox.Show(this, "Selected counters have been added to the database.", "Insert Counters", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearSelectedItems();
        }

        /// <summary>
        /// Determines whether a counter has already been added to the Seleted List.
        /// </summary>
        /// <param name="searchItem"></param>
        /// <returns></returns>
        private bool CanAdd(PerfMonCounterData searchItem)
        {
            return (_selectedCounters.Where(c => c.TargetHost == searchItem.TargetHost && 
                c.Category == searchItem.Category && 
                c.InstanceName == searchItem.InstanceName && 
                c.Counter == searchItem.Counter).FirstOrDefault() == null);
        }

        /// <summary>
        /// Clears the list of selected counters.
        /// </summary>
        private void ClearSelectedItems()
        {
            _selectedCounters.Clear();
        }
    }
}
