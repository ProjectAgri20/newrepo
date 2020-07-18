using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    [ToolboxItem(false)]
    public partial class EWSFirmwarePerformanceConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private EWSFirmwarePerformanceActivityData _data;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="EWSFirmwarePerformanceConfigurationControl" /> class.
        /// </summary>
        public EWSFirmwarePerformanceConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(textBoxFirmwareFolder, "Firmware file input");

            validatetimeSpanControl.DataBindings.Add("Enabled", checkBoxValidate, "Checked");
            fieldValidator.RequireCustom(validatetimeSpanControl, ValidateTimeout, "Please enter sufficient time for validation");

            textBoxFirmwareFolder.TextChanged += OnConfigurationChanged;


        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new EWSFirmwarePerformanceActivityData();
            validatetimeSpanControl.Value = TimeSpan.FromMinutes(30.0);

        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<EWSFirmwarePerformanceActivityData>();
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            if (!Directory.Exists(_data.FimBundlesLocation))
            {
                MessageBox.Show(@"The firmware location is no longer available . Please add the firmware directory detail again.",
    @"Firmware File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
            else
            {
                textBoxFirmwareFolder.Text = _data.FimBundlesLocation;
                checkBoxAutoBackup.Checked = _data.AutoBackup;
                checkBoxValidate.Checked = _data.ValidateFlash;
                validatetimeSpanControl.Value = _data.ValidateTimeOut == TimeSpan.FromMinutes(0) ? TimeSpan.FromMinutes(1) : _data.ValidateTimeOut;
                validateFW_Checkbox.Checked = _data.ValidateFWBundles;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.FimBundlesLocation = textBoxFirmwareFolder.Text;
            _data.AutoBackup = checkBoxAutoBackup.Checked;
            _data.ValidateFlash = checkBoxValidate.Checked;
            _data.ValidateFWBundles = validateFW_Checkbox.Checked;
            _data.ValidateTimeOut = validatetimeSpanControl.Value;


            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }


        private bool ValidateTimeout()
        {
            if (!checkBoxValidate.Checked)
            {
                return true;
            }
            return validatetimeSpanControl.Value > TimeSpan.FromMinutes(1);
        }

        private void textBoxFirmwareFolder_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFirmwareFolder.Text))
                return;

            if (!Directory.Exists(textBoxFirmwareFolder.Text))
            {
                MessageBox.Show(@"The firmware folder directory is no longer available and might have been removed. Please add check your path and try again.",
                    @"Firmware directory Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            string[] files = Directory.GetFiles(textBoxFirmwareFolder.Text, "*.bdl");
            if (files.Length == 0)
            {
                MessageBox.Show(@"The firmware folder directory does not appear to contain firmware bundle files (.bdl). Please add some to the directory and try again.",
    @"Firmware File(s) Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            //else
            //{
            //    ReadFirmwareDump(files);


            //    firmwareInfo_GridView.DataSource = null;
            //    firmwareInfo_GridView.DataSource = _data.FWBundleInfo;
            //    firmwareInfo_GridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //    firmwareInfo_GridView.Refresh();
            //}
        }

        private void ReadFirmwareDump(string[] bundleFiles)
        {
            string[] separator = { Environment.NewLine };
            //extract the file;
            var tempDumpDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Dump"));
            var dumpUtilityFileName = Path.Combine(tempDumpDirectory.FullName, "FimDumpUtility.exe");
            File.WriteAllBytes(dumpUtilityFileName, ResourceDump.FimDumpUtility);

            //Get .bdl files from directory
            //Extract data
            //populate datagridview
            //refresh

            _data.FWBundleInfo.Clear();

            AssetIdCollection assetIds = assetSelectionControl.AssetSelectionData.SelectedAssets;
            
            AssetInfoCollection assets =  Framework.ConfigurationServices.AssetInventory.GetAssets(assetIds);
            var col = assets.OfType<PrintDeviceInfo>();

            Dictionary<string, ModelFileMap> nameModel = new Dictionary<string, ModelFileMap>();
            string endpoint = "fim";
            string urn = "urn:hp:imaging:con:service:fim:FIMService";
            foreach (var printer in col)
            {
                SetDefaultPassword(printer.Address, printer.AdminPassword);
                ///Get way of finding the product family

                if (!nameModel.ContainsKey(printer.AssetId))
                {
                    ModelFileMap map = new ModelFileMap();

                    JediDevice device = new JediDevice(printer.Address, printer.AdminPassword);

                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    var ident = tic.FindElements("AssetIdentifier").First().Value;



                    map.ProductFamily = ident;// "6D6670-0055";// Bugatti"696D66-0015";
                    nameModel.Add(printer.AssetId, map);
                }
                
            }

            //_data.AssetMapping = nameModel;


            foreach (string firmwareFile in bundleFiles)
            {
                FirmwareData fwData = new FirmwareData();
                
                FileInfo fInfo = new FileInfo(firmwareFile);
                var fileSize = fInfo.Length / (1024 * 1024);
                var fileSizeMb = (int)((fileSize / 50.0) * 6);
                fwData.FlashTimeOutPeriod = (int)TimeSpan.FromMinutes(fileSizeMb).TotalMilliseconds;

                var result = ProcessUtil.Execute(dumpUtilityFileName,
                    $"-o {tempDumpDirectory.FullName} \"{firmwareFile}\"");

                var outputLines = result.StandardOutput.Split(separator, StringSplitOptions.None);

                var revision = outputLines.FirstOrDefault(x => x.Contains("Version"));
                if (string.IsNullOrEmpty(revision))
                {
                    MessageBox.Show(
                        $@"An error occurred while reading firmware revision information. Please check the firmware file {firmwareFile} and try again. Read Aborted",
                        @"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                revision = revision.Substring(revision.IndexOf(':') + 1).Trim();
                fwData.FirmwareRevision = revision.Split(' ').First();

                var version = outputLines.FirstOrDefault(x => x.Contains("Description"))?.Trim();
                if (string.IsNullOrEmpty(version))
                {
                    MessageBox.Show(
                        @"An error occurred while reading firmware version information. Please check the firmware file again and try",
                        @"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                version = version.Substring(version.IndexOf(':') + 1);
                fwData.FWBundleVersion = version;

                var dateCode = revision.Substring(revision.IndexOf('(') + 1, revision.LastIndexOf(')') - (revision.IndexOf('(') + 1));
                fwData.FirmwareDateCode = dateCode;

                var name = outputLines.FirstOrDefault(x => x.Contains("Name"));
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show(
                        @"An error occurred while reading firmware Name information. Please check the firmware file again and try",
                        @"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                name = name.Substring(name.IndexOf(':') + 1).Trim();
                fwData.FWModelName = name;


                var pfamily = outputLines.FirstOrDefault(x => x.Contains("Identifier"));
                pfamily = pfamily.Substring(pfamily.IndexOf(':') + 1).Trim();
                fwData.ProductFamily = pfamily;

                if (nameModel.Where(x => x.Value.ProductFamily == pfamily).Count() == 0)
                {
                    MessageBox.Show(
    $@"Failed to match the firmware bundle to an existing device. Please check the firmware files and selected assets and try again. Model: {name}",
    @"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    var devs = nameModel.Where(x => x.Value.ProductFamily == pfamily).Select(x => x.Key);
                    foreach (var dev in devs)
                    {
                        nameModel[dev].FirmwareFile = firmwareFile;
                    }
                }


                _data.FWBundleInfo.Add(fwData);
            }
            

            if (nameModel.Where(x => x.Value.FirmwareFile == string.Empty).Count() > 0)
            {
                string devices = nameModel.Where(x => x.Value.FirmwareFile == string.Empty).Select(x => x.Key).Aggregate((current, next) => current + ", " + next);
                MessageBox.Show(
$@"Failed to match the following devices with firmware: {devices}. Please check the firmware files and selected assets and try again.",
@"Firmware File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            _data.AssetMapping = nameModel;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                //dialog.DefaultExt = "*.bdl";
                //dialog.Filter = @"Firmware Bundle File (*.bdl)|*.bdl";
                dialog.Multiselect = false;
                dialog.Title = @"Select the Folder Containing the Firmware Bundle Files";
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    textBoxFirmwareFolder.Text = dialog.FileName;
                }
            }
        }

        private void mapFirmware_Button_Click(object sender, EventArgs e)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(textBoxFirmwareFolder.Text, "*.bdl");
            }
            catch
            {
                MessageBox.Show(@"Please select a valid path.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            if (files.Length == 0)
            {
                MessageBox.Show(@"The firmware folder directory does not appear to contain firmware bundle files (.bdl). Please add some to the directory and try again.",
    @"Firmware File(s) Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                ReadFirmwareDump(files);


                firmwareInfo_GridView.DataSource = null;
                firmwareInfo_GridView.DataSource = _data.FWBundleInfo;
                firmwareInfo_GridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                firmwareInfo_GridView.Refresh();
            }
        }




        public JediDevice SetDefaultPassword(string address, string password)
        {
            var defPWUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            JediDevice dev;
            try
            {
                dev = new JediDevice(address, "");
                WebServiceTicket tic = dev.WebServices.GetDeviceTicket(endpoint, defPWUrn);
                tic.FindElement("Password").SetValue(password);
                tic.FindElement("PasswordStatus").SetValue("set");
                dev.WebServices.PutDeviceTicket("security", defPWUrn, tic, false);

                dev = new JediDevice(address, password);
            }
            catch (Exception)
            {
                dev = new JediDevice(address, password);
            }


            return dev;
        }
    }
}
