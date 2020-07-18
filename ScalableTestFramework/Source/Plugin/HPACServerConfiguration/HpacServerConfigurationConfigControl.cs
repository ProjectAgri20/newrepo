using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Control to configure data for a HpacServer activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpacServerConfigurationConfigControl : UserControl, IPluginConfigurationControl
    {
        private HpacServerConfigurationActivityData _activityData;
        private HpacTile _selectedTile = HpacTile.Devices;
        private readonly PrintServerUserControl printServerUserControl = new PrintServerUserControl();
        private readonly IRMUserControl irmUserControl = new IRMUserControl();
        private readonly DeviceUserControl deviceUserControl = new DeviceUserControl();
        private readonly SettingsUserControl settingsUserControl = new SettingsUserControl();
        private readonly JobAccountingUserControl jobAccountingUserControl = new JobAccountingUserControl();
        private AssetSelectionData _assetSelectionData;
        /// <summary>
        /// Initializes a new instance of the <see cref="HpacServerConfigurationConfigControl"/> class.
        /// </summary> 
        public HpacServerConfigurationConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireSelection(hpac_ServerComboBox, hpacServer_Label);
            fieldValidator.RequireSelection(hpacTileCombobox, hpacTile_label);
        }

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new HpacServerConfigurationActivityData();
            hpac_ServerComboBox.Initialize("HPAC");
            hpacTileCombobox.DataSource = EnumUtil.GetValues<HpacTile>().ToList();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<HpacServerConfigurationActivityData>();
            _assetSelectionData = configuration.Assets;
            hpac_ServerComboBox.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "HPAC");
            hpacTileCombobox.DataSource = EnumUtil.GetValues<HpacTile>().ToList();
            hpacTileCombobox.SelectedItem = _activityData.HpacConfigTile.ToString();
            LoadConfiguration();
        }

        private void PopulateUi()
        {
            _selectedTile = EnumUtil.Parse<HpacTile>(hpacTileCombobox.SelectedItem.ToString(), true);
            if (_selectedTile == HpacTile.Devices)
            {
                PopulateDevicesUi();
            }
            if (_selectedTile == HpacTile.IRM)
            {
                PopulateIRMUI();
            }
            if (_selectedTile == HpacTile.Settings)
            {
                PopulateSetting();
            }
            if (_selectedTile == HpacTile.PrintServer)
            {
                PopulatePrintServerUi();
            }
            if (_selectedTile == HpacTile.JobAccounting)
            {
                PopulateJobAccountingUi();
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            PluginValidationResult result = new PluginValidationResult(fieldValidator.ValidateAll());
            _activityData.HpacConfigTile = EnumUtil.Parse<HpacTile>(hpacTileCombobox.SelectedItem.ToString(), true);

            if (_activityData.HpacConfigTile == HpacTile.Devices)
            {
                var deviceUserControlresult = deviceUserControl.AddValidaton();
                irmUserControl.RemoveValidation();
                printServerUserControl.RemoveValidation();
                settingsUserControl.RemoveValidation();
                jobAccountingUserControl.RemoveValidation();
                if (deviceUserControlresult.Succeeded & result.Succeeded)
                {
                    return new PluginValidationResult(true);
                }
                else
                {
                    return new PluginValidationResult(false);
                }
            }
            else if (_activityData.HpacConfigTile == HpacTile.IRM)
            {
                var irmUserControllresult = irmUserControl.AddValidaton();
                deviceUserControl.RemoveValidation();
                printServerUserControl.RemoveValidation();
                settingsUserControl.RemoveValidation();
                jobAccountingUserControl.RemoveValidation();
                if (irmUserControllresult.Succeeded & result.Succeeded)
                {
                    return new PluginValidationResult(true);
                }
                else
                {
                    return new PluginValidationResult(false);
                }
            }
            else if (_activityData.HpacConfigTile == HpacTile.PrintServer)
            {
                var printServerUserControlresult = printServerUserControl.AddValidaton();
                deviceUserControl.RemoveValidation();
                irmUserControl.RemoveValidation();
                settingsUserControl.RemoveValidation();
                jobAccountingUserControl.RemoveValidation();
                if (printServerUserControlresult.Succeeded & result.Succeeded)
                {
                    return new PluginValidationResult(true);
                }
                else
                {
                    return new PluginValidationResult(false);
                }
            }
            else if (_activityData.HpacConfigTile == HpacTile.Settings)
            {
                var settingsUserControlresult = settingsUserControl.AddValidaton();
                deviceUserControl.RemoveValidation();
                irmUserControl.RemoveValidation();
                printServerUserControl.RemoveValidation();
                jobAccountingUserControl.RemoveValidation();
                if (settingsUserControlresult.Succeeded & result.Succeeded)
                {
                    return new PluginValidationResult(true);
                }
                else
                {
                    return new PluginValidationResult(false);
                }
            }
            else if (_activityData.HpacConfigTile == HpacTile.JobAccounting)
            {
                var jobAccountingUserControlresult = jobAccountingUserControl.AddValidaton();
                deviceUserControl.RemoveValidation();
                irmUserControl.RemoveValidation();
                printServerUserControl.RemoveValidation();
                settingsUserControl.RemoveValidation();
                if (jobAccountingUserControlresult.Succeeded & result.Succeeded)
                {
                    return new PluginValidationResult(true);
                }
                else
                {
                    return new PluginValidationResult(false);
                }
            }
            else
            {
                return new PluginValidationResult(fieldValidator.ValidateAll());
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// tab data .
        /// </summary>
        /// <returns>The tab data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.HpacConfigTile = EnumUtil.Parse<HpacTile>(hpacTileCombobox.SelectedItem.ToString(), true);
            if (_activityData.HpacConfigTile == HpacTile.Devices)
            {
                _activityData.DeviceData = deviceUserControl.GetConfigurationData();
            }
            else if (_activityData.HpacConfigTile == HpacTile.IRM)
            {
                _activityData.IRMData = irmUserControl.GetConfigurationData();
            }
            else if (_activityData.HpacConfigTile == HpacTile.PrintServer)
            {
                _activityData.PrintServerData = printServerUserControl.GetConfigurationData();
            }
            else if (_activityData.HpacConfigTile == HpacTile.Settings)
            {
                _activityData.SettingsData = settingsUserControl.GetConfigurationData();
            }
            else if (_activityData.HpacConfigTile == HpacTile.JobAccounting)
            {
                _activityData.JobAccountingData = jobAccountingUserControl.GetConfigurationData();
            }
            return new PluginConfigurationData(_activityData, "1.0")
            {
                Servers = new ServerSelectionData(hpac_ServerComboBox.SelectedServer),
                Assets = new AssetSelectionData((AssetInfo)_activityData.DeviceData?.Asset)
            };
        }

        /// <summary>  
        /// Configures the controls data either derived from initialization or the saved meta data.   
        /// </summary>
        public void LoadConfiguration()
        {
            if (_activityData.HpacConfigTile == HpacTile.Devices)
            {
                if (_assetSelectionData != null && _assetSelectionData.SelectedAssets.Count > 0)
                {
                    var asset = ConfigurationServices.AssetInventory.GetAsset(_assetSelectionData.SelectedAssets
                        .FirstOrDefault());
                    if (asset != null)
                        _activityData.DeviceData.Asset = (IDeviceInfo)asset;
                }

                deviceUserControl.LoadConfiguration(_activityData.DeviceData);
            }
            else if (_activityData.HpacConfigTile == HpacTile.IRM)
            {
                irmUserControl.LoadConfiguration(_activityData.IRMData);
            }
            else if (_activityData.HpacConfigTile == HpacTile.PrintServer)
            {
                printServerUserControl.LoadConfiguration(_activityData.PrintServerData);
            }
            else if (_activityData.HpacConfigTile == HpacTile.Settings)
            {
                settingsUserControl.LoadConfiguration(_activityData.SettingsData);
            }
            else if (_activityData.HpacConfigTile == HpacTile.JobAccounting)
            {
                jobAccountingUserControl.LoadConfiguration(_activityData.JobAccountingData);
            }
        }

        private void PopulateIRMUI()
        {
            dynamic_groupBox.Controls.Clear();
            dynamic_groupBox.Controls.Add(irmUserControl);
        }
        private void PopulateDevicesUi()
        {
            dynamic_groupBox.Controls.Clear();
            dynamic_groupBox.Controls.Add(deviceUserControl);
        }
        private void PopulateJobAccountingUi()
        {
            dynamic_groupBox.Controls.Clear();
            dynamic_groupBox.Controls.Add(jobAccountingUserControl);
        }
        private void PopulatePrintServerUi()
        {
            dynamic_groupBox.Controls.Clear();
            dynamic_groupBox.Controls.Add(printServerUserControl);
        }
        private void PopulateSetting()
        {
            dynamic_groupBox.Controls.Clear();
            dynamic_groupBox.Controls.Add(settingsUserControl);
        }

        private void hpacTileCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            OnConfigurationChanged(sender, e);
        }

        private void hpacTileCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateUi();
        }
    }
}
