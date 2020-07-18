using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Edit form to create STF Server entries
    /// </summary>
    public partial class FrameworkServerEditForm : Form
    {
        private FrameworkServerProxy _proxy = null;
        private FrameworkServerController _controller = null;
        private BackgroundWorker _worker = new BackgroundWorker();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerEditForm"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="server">The server.</param>
        public FrameworkServerEditForm(FrameworkServerController controller, FrameworkServerProxy server)
        {
            _controller = controller;
            _proxy = server;

            InitializeComponent();

            hostname_TextBox.ReadOnly = true;
            serverTypes_ListBox.DisplayMember = "Name";
        }

        private void SetArchitecture()
        {
            x86_RadioButton.Checked = (_proxy.Architecture.Equals("X86", StringComparison.OrdinalIgnoreCase));
            x64_RadioButton.Checked = !x86_RadioButton.Checked;
        }

        private void ServerInventoryEditForm_Load(object sender, EventArgs e)
        {
            var operatingSystems = _controller.OperatingSystems.ToArray();
            operatingSystem_ComboBox.Items.AddRange(operatingSystems);

            if (!string.IsNullOrEmpty(_proxy.OperatingSystem))
            {
                if (!operatingSystems.Contains(_proxy.OperatingSystem, StringComparer.OrdinalIgnoreCase))
                {
                    operatingSystem_ComboBox.Items.Add(_proxy.OperatingSystem);
                }
                operatingSystem_ComboBox.SelectedItem = _proxy.OperatingSystem;
            }


            serverTypes_ListBox.Items.AddRange(_controller.ServerTypes.ToArray());          
            status_ComboBox.DataSource = EnumUtil.GetDescriptions<FrameworkServerStatus>().ToArray();

            processors_NumericUpDown.DataBindings.Add("Value", _proxy, "Processors", true, DataSourceUpdateMode.OnPropertyChanged);
            cores_numericUpDown.DataBindings.Add("Value", _proxy, "Cores", true, DataSourceUpdateMode.OnPropertyChanged);
            memory_NumericUpDown.DataBindings.Add("Value", _proxy, "Memory", true, DataSourceUpdateMode.OnPropertyChanged);
            disk_TextBox.DataBindings.Add("Text", _proxy, "DiskSpace", true, DataSourceUpdateMode.OnPropertyChanged);
            hostname_TextBox.DataBindings.Add("Text", _proxy, "Hostname", true, DataSourceUpdateMode.OnPropertyChanged);
            contact_TextBox.DataBindings.Add("Text", _proxy, "Contact", true);
            status_ComboBox.DataBindings.Add("Text", _proxy, "Status");            
            active_CheckBox.DataBindings.Add("Checked", _proxy, "Active");
            ipAddress_textBox.DataBindings.Add("Text", _proxy, "IpAddress", true, DataSourceUpdateMode.OnPropertyChanged);
            service_TextBox.DataBindings.Add("Text", _proxy, "ServiceVersion", true, DataSourceUpdateMode.OnPropertyChanged);

            SetArchitecture();

            for (int i = 0; i < serverTypes_ListBox.Items.Count; i++)
            {
                var item = serverTypes_ListBox.Items[i];

                if (_proxy.ServerTypes.Contains(item))
                {
                    serverTypes_ListBox.SetItemChecked(i, true);
                }
                else
                {
                    serverTypes_ListBox.SetItemChecked(i, false);
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            _proxy.ServerTypes.Clear();

            for (int i = 0; i < serverTypes_ListBox.Items.Count; i++)
            {
                if (serverTypes_ListBox.GetItemChecked(i))
                {
                    _proxy.ServerTypes.Add((FrameworkServerType)serverTypes_ListBox.Items[i]);
                }
            }

            _proxy.OperatingSystem = operatingSystem_ComboBox.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
            }

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void architecture_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (x64_RadioButton.Checked)
            {
                _proxy.Architecture = WindowsSystemInfo.ArchitectureX64;
            }
            else
            {
                _proxy.Architecture = WindowsSystemInfo.ArchitectureX86;
            }
        }

        private void rescan_Button_Click(object sender, EventArgs e)
        {
            scanning_Button.Enabled = false;
            ok_Button.Enabled = false;
            scanning_PictureBox.Visible = true;
            scanning_PictureBox.Image = Properties.Resources.BusySpinner;
            scanning_Label.Visible = true;

            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _worker.RunWorkerAsync(_proxy);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            scanning_Button.Enabled = true;
            ok_Button.Enabled = true;
            scanning_PictureBox.Visible = false;
            scanning_Label.Visible = false;

            object[] resultData = e.Result as object[];

            if (!(bool)resultData[0])
            {
                MessageBox.Show
                    (
                        "Unable to scan the server. {0}".FormatWith((string)resultData[2]),
                        "Server Scan", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return;
            }
            else
            {
                var proxy = resultData[1] as FrameworkServerProxy;
                _proxy.CopyFrom(proxy);
            }

            string name = _proxy.OperatingSystem;
            if (!operatingSystem_ComboBox.Items.Contains(name))
            {
                operatingSystem_ComboBox.Items.Add(name);
            }
            operatingSystem_ComboBox.SelectedItem = name;

            SetArchitecture();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sourceProxy = e.Argument as FrameworkServerProxy;

            // Use a copy of the proxy class that is not databound to UI controls.
            // If not done this way, the _proxy will fire property changed events
            // as it gets updated and will cause the UI updates to fail as this is
            // running on a background thread.
            FrameworkServerProxy proxy = new FrameworkServerProxy();
            proxy.CopyFrom(sourceProxy);
            string error = string.Empty;
            bool result = _controller.QueryServer(proxy, out error);
            e.Result = new object[] { result, proxy, error };
        }

        private void settings_Button_Click(object sender, EventArgs e)
        {
            using (ServerSettingsListForm dialog = new ServerSettingsListForm(_proxy.HostName))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }
    }

    /// <summary>
    /// Enumeration describing the status of a Framework Server
    /// </summary>
    public enum FrameworkServerStatus
    {
        /// <summary>
        /// No usage state selected
        /// </summary>
        /// <remarks>
        /// Do not add an Enum Description - this prevents it from showing up in UI lists
        /// </remarks>
        None,

        /// <summary>
        /// Describes a server in an available state and accessible by general users
        /// </summary>
        [Description("Available")]
        Available,

        /// <summary>
        /// Describes a server that is down for maintenance and should not be used
        /// </summary>
        [Description("Maintenance")]
        Maintenance,

        /// <summary>
        /// Describes a server that is reserved for usage by a specific user
        /// </summary>
        [Description("Restricted Use")]
        Restricted
    }
}
