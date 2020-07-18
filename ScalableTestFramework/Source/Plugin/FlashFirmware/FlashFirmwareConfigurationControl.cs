using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.FlashFirmware.BashLog;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.FlashFirmware
{
    /// <summary>
    /// Provides the control to configure the FlashFirmware activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class FlashFirmwareConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private FlashFirmwareActivityData _data;
        private List<string> _portList;
        private string _bashCollectorServiceHost;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the FlashFirmwareConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public FlashFirmwareConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireAssetSelection(firmware_assetSelectionControl);
            fieldValidator.RequireValue(textBoxFirmwareFile, "Firmware file input");

            validatetimeSpanControl.DataBindings.Add("Enabled", checkBoxValidate, "Checked");
            fieldValidator.RequireSelection(port_comboBox, port_label, bios_radioButton);
            fieldValidator.RequireCustom(validatetimeSpanControl, ValidateTimeout, "Please enter sufficient time for validation");

            textBoxFirmwareFile.TextChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.FirmwareFileName = textBoxFirmwareFile.Text;
            _data.AutoBackup = checkBoxAutoBackup.Checked;
            _data.ValidateFlash = checkBoxValidate.Checked;
            _data.ValidateTimeOut = validatetimeSpanControl.Value;
            _data.IsDowngrade = radioButtonDowngrade.Checked;

            if (ews_radioButton.Checked)
            {
                _data.FlashMethod = FlashMethod.Ews;
            }
            else if (controlPanel_radioButton.Checked)
            {
                _data.FlashMethod = FlashMethod.ControlPanel;
            }
            else
            {
                _data.FlashMethod = FlashMethod.Bios;
                _data.ComPort = port_comboBox.Text;
            }

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = firmware_assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new FlashFirmwareActivityData();
            validatetimeSpanControl.Value = TimeSpan.FromMinutes(5.0);
            BashCollectorServiceHostCheck(environment);
        }

        private void BashCollectorServiceHostCheck(PluginEnvironment environment)
        {
            if (environment.PluginSettings.ContainsKey("BashLogCollectorServiceHost"))
            {
                _bashCollectorServiceHost = environment.PluginSettings["BashLogCollectorServiceHost"];

                try
                {
                    using (TcpClient client = new TcpClient(_bashCollectorServiceHost,
                        (int) WcfService.BashLogcollectorService))
                    {
                        try
                        {
                            client.Connect(_bashCollectorServiceHost, (int)WcfService.BashLogcollectorService);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(
                                $@"Please verify the setting BashLogCollectorServiceHost in PluginSettings to enable Firmware Flash through Bios method, {e.Message}",
                                @"Incorrect Plugin Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            port_comboBox.Enabled = bios_radioButton.Enabled = false;
                            return;
                        }

                       
                    }
                    using (BashLogCollectorClient client = new BashLogCollectorClient(_bashCollectorServiceHost))
                    {
                        _portList = client.GetSerialPorts();
                        if (_portList.Count > 0)
                        {
                            port_comboBox.DataSource = _portList;
                            port_comboBox.DataBindings.Add("Enabled", bios_radioButton, "Checked");
                        }
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        $@"Please verify the setting BashLogCollectorServiceHost in PluginSettings to enable Firmware Flash through Bios method, {e.Message}",
                        @"Incorrect Plugin Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    port_comboBox.Enabled = bios_radioButton.Enabled = false;
                }
               
            }
            else
            {
                MessageBox.Show(
                    @"Please define the setting BashLogCollectorServiceHost in PluginSettings to enable Firmware Flash through Bios method",
                    @"Missing Plugin Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                port_comboBox.Enabled = bios_radioButton.Enabled = false;
            }
        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<FlashFirmwareActivityData>();
            firmware_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            if (!File.Exists(_data.FirmwareFileName))
            {
                MessageBox.Show(@"The firmware file is no longer available and might have been moved. Please add the firmware file detail again.",
                    @"Firmware File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                textBoxFirmwareFile.Text = _data.FirmwareFileName;
            }

            checkBoxAutoBackup.Checked = _data.AutoBackup;
                checkBoxValidate.Checked = _data.ValidateFlash;
                validatetimeSpanControl.Value = _data.ValidateTimeOut == TimeSpan.FromMinutes(0) ? TimeSpan.FromMinutes(1) : _data.ValidateTimeOut;
                BashCollectorServiceHostCheck(environment);
                radioButtonDowngrade.Checked = _data.IsDowngrade;

                switch (_data.FlashMethod)
                {
                    default:
                        ews_radioButton.Checked = true;
                        break;

                    case FlashMethod.ControlPanel:
                        controlPanel_radioButton.Checked = true;
                        break;

                    case FlashMethod.Bios:
                        {
                           
                            int comPortIndex = port_comboBox.Items.IndexOf(_data.ComPort);
                            if (comPortIndex == -1)
                            {
                                //check if the comport mentioned is telnet ip or COM
                                if (_data.ComPort.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show(
                                        $@"The serial port {_data.ComPort} is not available currently. Please check the BashLogCollectorHost and try again!", @"Port Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                port_comboBox.DataSource = null;
                                _portList.Add(_data.ComPort);
                                port_comboBox.DataSource = _portList;
                                port_comboBox.SelectedItem = _data.ComPort;
                               
                            }
                            else
                            {
                                port_comboBox.SelectedIndex = comPortIndex;
                            }
                            bios_radioButton.Checked = true;
                        }
                        break;
                }
            
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        /// <summary>
        /// Event handler to be called whenever the activity's configuration data changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void textBoxFirmwareFile_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFirmwareFile.Text))
                return;

            if (!File.Exists(textBoxFirmwareFile.Text))
            {
                MessageBox.Show(@"The firmware file is no longer available and might have been moved. Please add the firmware file detail again",
                    @"Firmware File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            ReadFirmwareDump();
        }

        private void ReadFirmwareDump()
        {
            string[] separator = { Environment.NewLine };
            string firmwareErrorString =
                @"An error occurred while reading firmware {0} information. Please check the firmware file again and try";
            //extract the file;
            var tempDumpDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Dump"));
            var dumpUtilityFileName = Path.Combine(tempDumpDirectory.FullName, "FimDumpUtility.exe");
            File.WriteAllBytes(dumpUtilityFileName, ResourceDump.FimDumpUtility);

            var result = ProcessUtil.Execute(dumpUtilityFileName,
                $"-o {tempDumpDirectory.FullName} \"{textBoxFirmwareFile.Text}\"");
            var outputLines = result.StandardOutput.Split(separator, StringSplitOptions.None);

            var revision = outputLines.FirstOrDefault(x => x.Contains("Version"));
            if (string.IsNullOrEmpty(revision))
            {
                MessageBox.Show(string.Format(firmwareErrorString, "Version"),@"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            revision = revision.Substring(revision.IndexOf(':') + 1).Trim();
            textBoxRevision.Text = revision.Split(' ').First();

            var version = outputLines.FirstOrDefault(x => x.Contains("Description"))?.Trim();
            if (string.IsNullOrEmpty(version))
            {
                MessageBox.Show(string.Format(firmwareErrorString, "Description"),@"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            version = version.Substring(version.IndexOf(':') + 1);
            textBoxVersion.Text = version;

            var dateCode = revision.Substring(revision.IndexOf('(') + 1, revision.LastIndexOf(')') - (revision.IndexOf('(') + 1));
            textBoxDate.Text = dateCode;

            var name = outputLines.FirstOrDefault(x => x.Contains("Name"));
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(string.Format(firmwareErrorString, "Name"),@"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            name = name.Substring(name.IndexOf(':') + 1).Trim();
            textBoxModelName.Text = name;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.bdl";
                dialog.Filter = @"Firmware Bundle File (*.bdl)|*.bdl";
                dialog.Multiselect = false;
                dialog.Title = @"Select the Firmware Bundle File";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    textBoxFirmwareFile.Text = dialog.FileName;
                }
            }
        }

        private bool ValidateTimeout()
        {
            if (!checkBoxValidate.Checked)
            {
                return true;
            }
            return validatetimeSpanControl.Value > TimeSpan.FromMinutes(1);
        }
    }
}