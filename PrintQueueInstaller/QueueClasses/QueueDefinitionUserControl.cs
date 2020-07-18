using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// User control that allows users to select drivers to install.
    /// </summary>
    public partial class QueueDefinitionUserControl : FieldValidatedControl
    {
        private QueueManager _manager = null;
        private DriverInstallUserControl _control = null;
        private AdHocQueueData _adhocDevices = new AdHocQueueData();
        private Collection<string> _selectedDevices = new Collection<string>();

        internal class AdHocQueueData
        {
            public int StartingIPValue { get; set; }
            public int EndingIPValue { get; set; }
            public string VirtualPrinterAddress { get; set; }
            public int NumberOfQueues { get; set; }
            public string AddressCode { get; set; }
            public bool IncrementIPValue { get; set; }
            public bool EnableSnmp { get; set; }
            public bool RenderOnClient { get; set; }
            public bool ShareQueues { get; set; }

            public AdHocQueueData()
            {
                NumberOfQueues = 0;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="QueueDefinitionUserControl"/> class from being created.
        /// </summary>
        QueueDefinitionUserControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueDefinitionUserControl"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public QueueDefinitionUserControl(QueueManager manager)
        {
            InitializeComponent();

            _control = new DriverInstallUserControl(manager);
            _manager = manager;
        }

        /// <summary>
        /// Gets the installation timeout.
        /// </summary>
        public TimeSpan InstallationTimeout
        {
            get
            {
                return TimeSpan.FromMinutes((int)installTimeout_NumericUpDown.Value);
            }
        }

        /// <summary>
        /// Builds the defined by the user.
        /// </summary>
        public void CreateDefinitions()
        {
            if (_selectedDevices.Count > 0)
            {
                int queueCount = int.Parse(queueCount_TextBox.Text, CultureInfo.InvariantCulture);
                _manager.ConstructQueueDefinitions(_selectedDevices, queueCount, fullName_CheckBox.Checked);
            }

            if (_adhocDevices.NumberOfQueues > 0)
            {
                _manager.ConstructQueueDefinitions
                    (
                        _adhocDevices.StartingIPValue,
                        _adhocDevices.EndingIPValue,
                        _adhocDevices.VirtualPrinterAddress,
                        _adhocDevices.NumberOfQueues,
                        _adhocDevices.AddressCode,
                        _adhocDevices.IncrementIPValue,
                        _adhocDevices.EnableSnmp,
                        _adhocDevices.RenderOnClient,
                        _adhocDevices.ShareQueues
                    );
            }
        }

        private void QueueDefinitionControl_Load(object sender, EventArgs e)
        {
            driver_GroupBox.Controls.Add(_control);
            _control.Dock = DockStyle.Fill;

            additionalDescriptionText_TextBox.DataBindings.Add("Text", _manager, "AdditionalDescription");

            // Update the configuration file section
            useConfigurationFile_CheckBox.Checked = _manager.UseConfigurationFile;
            configFileName_TextBox.Text = _manager.ConfigurationFile;
            additionalDescriptionText_TextBox.Text = _manager.AdditionalDescription;
            UpdateConfigurationFileSection();
        }

        private void definePhysicalQueues_Button_Click(object sender, EventArgs e)
        {
            if (!_control.ValidatePackageLoaded())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            using (AssetSelectionForm printerSelectionForm = new AssetSelectionForm(AssetAttributes.Printer, true))
            {
                Cursor.Current = Cursors.Default;
                printerSelectionForm.ShowDialog(this);
                if (printerSelectionForm.DialogResult == DialogResult.OK)
                {
                    _selectedDevices.Clear();
                    foreach (string printerId in printerSelectionForm.SelectedAssets.Select(n => n.AssetId))
                    {
                        _selectedDevices.Add(printerId);
                    }
                    int queueCount = int.Parse(queueCount_TextBox.Text, CultureInfo.InvariantCulture);
                    //_manager.ConstructQueueDefinitions(printerSelectionForm.SelectedPrinterIds, queueCount);

                    pendingPhysicalQueues_TextBox.Text = "Queue definitions pending: {0}".FormatWith(_selectedDevices.Count * queueCount);
                    pendingPhysicalQueues_TextBox.Update();
                }
            }
        }

        private void dcuSetup_Button_Click(object sender, EventArgs e)
        {
            QueueManager.RunDriverConfigurationUtility();
        }

        private void useConfigurationFile_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _manager.UseConfigurationFile = useConfigurationFile_CheckBox.Checked;
            UpdateConfigurationFileSection();
        }

        private void UpdateConfigurationFileSection()
        {
            if (_manager.UseConfigurationFile)
            {
                configFileBrowse_Button.Enabled = true;
                configFileName_TextBox.Enabled = true;
            }
            else
            {
                configFileBrowse_Button.Enabled = false;
                configFileName_TextBox.Enabled = false;
            }
        }

        private void adHocQueue_Button_Click(object sender, EventArgs e)
        {
            if (!_control.ValidatePackageLoaded())
            {
                return;
            }

            using (VirtualQueueNameForm form = new VirtualQueueNameForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.DataControl.StartingIPValue > form.DataControl.EndingIPValue)
                    {
                        MessageBox.Show("Starting IP value must be less than the ending IP value", "IP value Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    _adhocDevices.NumberOfQueues = 0;
                    _adhocDevices.StartingIPValue = form.DataControl.StartingIPValue;
                    _adhocDevices.EndingIPValue = form.DataControl.EndingIPValue;
                    _adhocDevices.VirtualPrinterAddress = form.DataControl.VirtualPrinterAddress;
                    _adhocDevices.NumberOfQueues = form.DataControl.NumberOfQueues;
                    _adhocDevices.AddressCode = form.DataControl.AddressCode;
                    _adhocDevices.IncrementIPValue = form.DataControl.IncrementIPValue;
                    _adhocDevices.EnableSnmp = form.DataControl.EnableSnmp;
                    _adhocDevices.RenderOnClient = form.DataControl.RenderOnClient;
                    _adhocDevices.ShareQueues = form.DataControl.ShareQueues;

                    pendingVirtualQueues_TextBox.Text = "Queue definitions pending: {0}".FormatWith(_adhocDevices.NumberOfQueues);
                    pendingVirtualQueues_TextBox.Update();
                }
            }
        }

        private void configFileBrowse_Button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.AddExtension = true;
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "cfm";
                dialog.Multiselect = false;
                dialog.Title = "Select CFM File";
                dialog.Filter = "CFM files (*.cfm)|*.cfm";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    configFileName_TextBox.Text = dialog.FileName;
                    _manager.ConfigurationFile = dialog.FileName;
                }
            }
        }

        private void additionalDescriptionText_TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _manager.AdditionalDescription = additionalDescriptionText_TextBox.Text;
        }

        private void configFileName_TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMessage = null;

            bool valid = useConfigurationFile_CheckBox.Checked && string.IsNullOrEmpty(configFileName_TextBox.Text.Trim());
            
            if (valid)
            {
                errorMessage = "Configuration File name must have a value.";
            }
            else
            {
                _manager.ConfigurationFile = configFileName_TextBox.Text;
            }

            fieldValidator.SetError(configFileName_TextBox, errorMessage);
            e.Cancel = valid;
        }

        private void queueCount_TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            base.ValidatePositiveInt(queueCount_TextBox.Text, "Queue Count", queueCount_Label, e);
        }

        private void queueCount_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (Validate())
            {
                int queueCount = int.Parse(queueCount_TextBox.Text, CultureInfo.InvariantCulture);
                pendingPhysicalQueues_TextBox.Text = "Queue definitions pending: {0}".FormatWith(_selectedDevices.Count * queueCount);
            }
        }

        private void queueCount_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (Validate())
                {
                    int queueCount = int.Parse(queueCount_TextBox.Text, CultureInfo.InvariantCulture);
                    pendingPhysicalQueues_TextBox.Text = "Queue definitions pending: {0}".FormatWith(_selectedDevices.Count * queueCount);
                }
            }
            e.Handled = true;
        }

        private void queueCount_TextBox_Leave(object sender, EventArgs e)
        {
            if (Validate())
            {
                int queueCount = int.Parse(queueCount_TextBox.Text, CultureInfo.InvariantCulture);
                pendingPhysicalQueues_TextBox.Text = "Queue definitions pending: {0}".FormatWith(_selectedDevices.Count * queueCount);
            }
        }
    }
}
