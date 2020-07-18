using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Form used to define data for a driver upgrade
    /// </summary>
    public partial class DriverUpgradeForm : Form
    {
        private DriverInstallUserControl _control = null;
        private DriverUpgradeManager _upgradeManager = null;
        private SortableBindingList<DriverUpgradeData> _targetQueues = null;
        PrintDriverComboBox installedDrivers_ComboBox = null;
        private bool _upgrading = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverUpgradeForm"/> class.
        /// </summary>
        /// <param name="queueManager">The manager.</param>
        public DriverUpgradeForm(QueueManager queueManager)
        {
            InitializeComponent();
            InitializeInlineComponent();

            _control = new DriverInstallUserControl(queueManager);
            _control.Dock = DockStyle.Fill;

            _upgradeManager = new DriverUpgradeManager(queueManager);
            _upgradeManager.StatusChange += new EventHandler<StatusEventArgs>(_manager_StatusChange);
            _upgradeManager.DataUpdated += new EventHandler(_manager_DataUpdated);
            _upgradeManager.UpgradeCompleted += new EventHandler(_upgradeManager_UpgradeCompleted);

            driverToUpgrade_GridView.AutoGenerateColumns = false;
        }

        void InitializeInlineComponent()
        {
            installedDrivers_ComboBox = new PrintDriverComboBox(new Point(141, 27));
            installedDrivers_ComboBox.SelectedIndexChanged += new EventHandler(installedDrivers_ComboBox_SelectedIndexChanged);
            currentDriver_GroupBox.Controls.Add(installedDrivers_ComboBox);
            warning_Label.Text = Resource.UpgradeWarningMessage;
        }

        void installedDrivers_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string driverName = installedDrivers_ComboBox.SelectedData["Name"];
            string architecture = installedDrivers_ComboBox.SelectedData["Architecture"];

            var matchingQueues = DriverUpgradeManager.SelectQueues(driverName, architecture);

            _upgradeManager.CurrentDriverName = driverName;

            _targetQueues = new SortableBindingList<DriverUpgradeData>(matchingQueues);
            driverToUpgrade_GridView.DataSource = null;
            driverToUpgrade_GridView.DataSource = _targetQueues;
        }

        void _upgradeManager_UpgradeCompleted(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(this._upgradeManager_UpgradeCompleted), new object[] { sender, e });
            }
            else
            {
                EnableControls(true);
            }
        }

        void _manager_DataUpdated(object sender, EventArgs e)
        {
            if (driverToUpgrade_GridView.InvokeRequired)
            {
                this.Invoke(new EventHandler(this._manager_DataUpdated), new object[] { sender, e });
            }
            else
            {
                driverToUpgrade_GridView.Update();
                driverToUpgrade_GridView.Refresh();
            }
        }

        void _manager_StatusChange(object sender, StatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => _manager_StatusChange(sender, e)));
            }
            else
            {
                switch (e.EventType)
                {
                    case StatusEventType.StatusChange:
                        main_toolStripStatusLabel.Text = e.Message;
                        break;
                    case StatusEventType.AverageQueueUpgradeDuration:
                        averagePerQueue_TextBox.Text = e.Message;
                        break;
                    case StatusEventType.UpgradeStartTime:
                        upgradeStart_TextBox.Text = e.Message;
                        break;
                    case StatusEventType.UpgradeEndTime:
                        upgradeEnd_TextBox.Text = e.Message;
                        break;
                    case StatusEventType.TotalUpgradeTime:
                        totalUpgradeTime_TextBox.Text = e.Message;
                        break;
                    case StatusEventType.Reset:
                        main_toolStripStatusLabel.Text = string.Empty;
                        averagePerQueue_TextBox.Text = string.Empty;
                        upgradeStart_TextBox.Text = string.Empty;
                        upgradeEnd_TextBox.Text = string.Empty;
                        totalUpgradeTime_TextBox.Text = string.Empty;
                        break;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void upgrade_Button_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            Cursor.Current = Cursors.WaitCursor;
            if (_control.ValidatePackageLoaded())
            {
                var driverName = installedDrivers_ComboBox.SelectedData["Name"];
                var architecture = EnumUtil.Parse<ProcessorArchitecture>(installedDrivers_ComboBox.SelectedData["Architecture"]);

                if (architecture.ToDriverArchitecture() != _control.QueueManager.CurrentDriver.Architecture)
                {
                    MessageBox.Show
                        (
                            "Upgrading a {0} driver with a {1} driver is not allowed."
                                .FormatWith(architecture, _control.QueueManager.CurrentDriver.Architecture),
                            "Driver Architecture Mismatch",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );

                    EnableControls(true);
                    return;
                }

                if (driverName.Equals(_control.QueueManager.CurrentDriver.Name, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show
                        (
                            "Because the driver model is the same, all queues using this driver will be updated automatically by the spooler, "
                            + "so there will be no queue specific data available.",
                            "Upgrading all Queues",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                }

                try
                {
                    _manager_StatusChange(this, new StatusEventArgs(string.Empty, StatusEventType.Reset));
                    _upgradeManager.Upgrade(_targetQueues, _control.QueueManager.CurrentDriver);
                }
                catch (Exception ex)
                {
                    _upgradeManager.AbortUpgrade();
                    MessageBox.Show(ex.Message, "Failed to upgrade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EnableControls(true);
                }
            }
            else
            {
                EnableControls(true);
            }
            Cursor.Current = Cursors.Default;
        }

        private void EnableControls(bool enabled)
        {
            _upgrading = !enabled;

            upgrade_Button.Enabled = enabled;
            editControl_Panel.Enabled = enabled;
            installedDrivers_ComboBox.Enabled = enabled;
            abort_Button.Enabled = !enabled;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_upgrading)
            {
                var result = MessageBox.Show("This will abort the upgrade and close the window.  Continue?", "Close Upgrade", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            _upgradeManager.AbortUpgrade();
            EnableControls(true);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// Loads the drivers.
        /// </summary>
        public void LoadDrivers()
        {
            var selectedItem = installedDrivers_ComboBox.SelectedData;

            PrintDeviceDriverCollection drivers = new PrintDeviceDriverCollection();
            drivers.AddRange(DriverController.LoadFromRegistry().Select(n => new PrintDeviceDriver(n)));
            drivers.Sort();

            installedDrivers_ComboBox.Items.Clear();
            installedDrivers_ComboBox.AddDrivers(drivers);

            if (selectedItem.Count > 0)
            {
                ProcessorArchitecture arch = EnumUtil.Parse<ProcessorArchitecture>(selectedItem["Architecture"]);
                DriverVersion version = new DriverVersion(selectedItem["Version"]);
                int index = drivers.IndexOf(selectedItem["Name"], arch.ToDriverArchitecture(), version, selectedItem["InfFile"]);
                if (index != -1)
                {
                    installedDrivers_ComboBox.SelectedIndex = index;
                }
            }
            else
            {
                installedDrivers_ComboBox.SelectedIndex = 0;
            }
        }

        private void DriverUpgradeForm_Load(object sender, EventArgs e)
        {
            editControl_Panel.Controls.Add(_control);
            abort_Button.Enabled = false;
        }

        private void viewLog_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (StatusDisplayForm textbox = new StatusDisplayForm())
            {
                textbox.Text = "Driver Upgrade Status Log";

                textbox.Data = _upgradeManager.LogText;
                textbox.ShowDialog();
            }
        }

        private void abort_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will abort the upgrade.  Continue?", "Abort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _upgradeManager.AbortUpgrade();
                EnableControls(true);
            }
        }

        private void installedDriverRefresh_Button_Click(object sender, EventArgs e)
        {
            LoadDrivers();
        }
    }
}
