using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Form that displays options for configuring a dynamic local print queue.
    /// </summary>
    public partial class DynamicLocalPrintQueueForm : Form
    {
        private readonly List<IPrinterInfo> _selectedPrintDevices = new List<IPrinterInfo>();
        private readonly bool _allowMultipleDevices;

        private DynamicLocalPrintQueueForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            portType_ComboBox.Items.Add(PrinterPortProtocol.Raw);
            portType_ComboBox.Items.Add(PrinterPortProtocol.Lpr);
            portType_ComboBox.SelectedIndex = 0;

            fieldValidator.RequireCustom(printDevices_TextBox, () => _selectedPrintDevices.Any(), "A print device must be selected.");
            fieldValidator.RequireCustom(printDriverSelectionControl, () => printDriverSelectionControl.HasSelection, "A print driver must be selected.");
            fieldValidator.RequireValue(queueName_TextBox, queueName_Label, ValidationCondition.IfEnabled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueForm" /> class.
        /// </summary>
        /// <param name="allowMultipleDevices">if set to <c>true</c> allow multiple devices to be selected.</param>
        public DynamicLocalPrintQueueForm(bool allowMultipleDevices)
            : this()
        {
            _allowMultipleDevices = allowMultipleDevices;
            printDriverSelectionControl.Initialize();
            LoadCfmFiles();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueForm"/> class
        /// using information from the specified print queue.
        /// </summary>
        /// <param name="printQueue">The print queue whose information should be populated into the form.</param>
        /// <param name="allowMultipleDevices">if set to <c>true</c> allow multiple devices to be selected.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is null.</exception>
        public DynamicLocalPrintQueueForm(DynamicLocalPrintQueueInfo printQueue, bool allowMultipleDevices)
            : this()
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            _allowMultipleDevices = allowMultipleDevices;
            if (!string.IsNullOrEmpty(printQueue.AssociatedAssetId))
            {
                if (ConfigurationServices.AssetInventory.GetAsset(printQueue.AssociatedAssetId) is IPrinterInfo printDevice)
                {
                    _selectedPrintDevices.Add(printDevice);
                    printDevices_TextBox.Text = printQueue.AssociatedAssetId;
                }
            }
            printDriverSelectionControl.Initialize(printQueue.PrintDriver.PrintDriverId);

            if (printQueue.PrinterPort is LprPrinterPortInfo lprPrinterPort)
            {
                portType_ComboBox.SelectedItem = PrinterPortProtocol.Lpr;
                queueName_TextBox.Text = lprPrinterPort.QueueName;
            }
            else
            {
                portType_ComboBox.SelectedItem = PrinterPortProtocol.Raw;
            }

            LoadCfmFiles();
            if (printQueue.PrintDriverConfiguration != null && printQueue.PrintDriverConfiguration.ConfigurationFile != null)
            {
                cfmFile_ComboBox.SelectedItem = printQueue.PrintDriverConfiguration.ConfigurationFile;
                shortcut_ComboBox.SelectedItem = printQueue.PrintDriverConfiguration.DefaultShortcut;
            }
        }

        /// <summary>
        /// Gets the print queues configured in this form.
        /// </summary>
        public IEnumerable<DynamicLocalPrintQueueInfo> PrintQueues
        {
            get
            {
                PrintDriverInfo printDriver = printDriverSelectionControl.SelectedPrintDriver;
                PrinterPortInfo printerPort = null;
                if ((PrinterPortProtocol)portType_ComboBox.SelectedItem == PrinterPortProtocol.Lpr)
                {
                    printerPort = new LprPrinterPortInfo(queueName_TextBox.Text);
                }
                else
                {
                    printerPort = new RawPrinterPortInfo();
                }

                PrintDriverConfiguration configuration = new PrintDriverConfiguration();
                if (cfmFile_ComboBox.SelectedIndex > 0)
                {
                    configuration = new PrintDriverConfiguration(cfmFile_ComboBox.SelectedItem.ToString(), shortcut_ComboBox.SelectedItem.ToString());
                }

                foreach (IPrinterInfo device in _selectedPrintDevices)
                {
                    yield return new DynamicLocalPrintQueueInfo(device, printDriver, printerPort, configuration);
                }
            }
        }

        private void LoadCfmFiles()
        {
            cfmFile_ComboBox.Items.Clear();
            cfmFile_ComboBox.Items.Add(string.Empty);

            IEnumerable<string> configFiles = null;
            if (filterCfmList_CheckBox.Checked && _selectedPrintDevices.OfType<IDeviceInfo>().Any() && printDriverSelectionControl.HasSelection)
            {
                configFiles = ConfigurationServices.AssetInventory.AsInternal().GetPrintDriverConfigurations(_selectedPrintDevices.OfType<IDeviceInfo>().First(), printDriverSelectionControl.SelectedPrintDriver);
            }
            else
            {
                configFiles = ConfigurationServices.AssetInventory.AsInternal().GetPrintDriverConfigurations();
            }

            foreach (string configFile in configFiles)
            {
                cfmFile_ComboBox.Items.Add(configFile);
            }
            cfmFile_ComboBox.SelectedIndex = 0;
        }

        private void UpdatePrintingShortcuts()
        {
            shortcut_ComboBox.DataSource = null;

            if (cfmFile_ComboBox.SelectedIndex > 0)
            {
                shortcut_ComboBox.DataSource = GetShortCutsFromCfm(cfmFile_ComboBox.SelectedItem.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions should be caught to prevent the application from crashing.")]
        private static List<string> GetShortCutsFromCfm(string driverConfigurationFile)
        {
            var result = new List<string>() { string.Empty };
            if (!string.IsNullOrEmpty(driverConfigurationFile))
            {
                try
                {
                    var xRoot = XElement.Load(driverConfigurationFile);
                    var xCurrent = xRoot.Descendants("current").First().Element("printing");
                    var xFeatures = xCurrent.Elements("feature");
                    var shortcuts = xFeatures.Where(x => x.Attribute("resource_id").Value.Equals("3250"));
                    result.AddRange(shortcuts.Elements("option").Select(x => x.FirstNode.ToString().Trim()));
                }
                catch (Exception ex)
                {
                    ConfigurationServices.SystemTrace.LogDebug($"No printing shortcuts found in configuration file {driverConfigurationFile}: {ex}");
                }
            }
            return result;
        }

        private void selectPrinters_Button_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm form = new AssetSelectionForm(AssetAttributes.Printer, _allowMultipleDevices))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _selectedPrintDevices.Clear();
                    _selectedPrintDevices.AddRange(form.SelectedAssets.OfType<IPrinterInfo>());
                    printDevices_TextBox.Text = string.Join("; ", _selectedPrintDevices.Select(n => n.AssetId));

                    LoadCfmFiles();
                }
            }
        }

        private void portType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProtocol = (PrinterPortProtocol)portType_ComboBox.SelectedItem;

            switch (selectedProtocol)
            {
                case PrinterPortProtocol.Lpr:
                    queueName_Label.Enabled = true;
                    queueName_TextBox.Enabled = true;
                    break;

                default:
                    queueName_Label.Enabled = false;
                    queueName_TextBox.Enabled = false;
                    queueName_TextBox.Clear();
                    break;
            }
        }

        private void printDriverSelectionControl_SelectionChanged(object sender, EventArgs e)
        {
            LoadCfmFiles();
        }

        private void filterCfmList_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadCfmFiles();
        }

        private void cfmFile_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePrintingShortcuts();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            var results = fieldValidator.ValidateAll();
            if (results.All(n => n.Succeeded))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                var messages = results.Where(n => !n.Succeeded).Select(n => n.Message);
                MessageBox.Show(string.Join("\n", messages), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private enum PrinterPortProtocol
        {
            Raw,
            Lpr
        }
    }
}
