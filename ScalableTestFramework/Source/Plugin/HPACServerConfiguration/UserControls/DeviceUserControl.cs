using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// User control that allows users to perform device installation or configuration.
    /// </summary>
    internal partial class DeviceUserControl : UserControl
    {
        public DeviceOperation selectedOperation;
        private const AssetAttributes _deviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel | AssetAttributes.Printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceUserControl"/> class.
        /// </summary>
        public DeviceUserControl()
        {
            InitializeComponent();
        }

        private void AdddeviceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (addDeviceRadioButton.Checked)
            {
                configuration_GroupBox.Enabled = false;
                selectedOperation = DeviceOperation.Add;
            }

        }
        private void ConfiguredeviceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (configureRadioButton.Checked)
            {
                configuration_GroupBox.Enabled = true;
                selectedOperation = DeviceOperation.Configure;
            }

        }
        private void InstalldeviceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (installRadioButton.Checked)
            {
                configuration_GroupBox.Enabled = false;
                selectedOperation = DeviceOperation.InstallHPAgent;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="DeviceTabData" /> instance containing the
        /// device tab data from this control.
        /// </summary>
        /// <returns>The device data.</returns>
        public DeviceTabData GetConfigurationData()
        {
            DeviceTabData devicedata = new DeviceTabData();
            devicedata.Configuration = new List<HpacConfiguration>();
            if (configureRadioButton.Checked)
            {
                if (pullPrinting_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.PullPrinting);
                }
                if (tracking_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.Tracking);
                }
                if (authentication_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.Authentication);
                }
                if (authorization_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.Authorization);
                }
                if (quota_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.Quota);
                }
                if (confirmationTrap_CheckBox.Checked)
                {
                    devicedata.Configuration.Add(HpacConfiguration.ConfirmationTrap);
                }
                if (localList_CheckBox.Checked)
                {
                    devicedata.Authenticators.Add(HpacAuthenticators.LocalList);
                }
                if (picService_CheckBox.Checked)
                {
                    devicedata.Authenticators.Add(HpacAuthenticators.PicServer);
                }
                if (irmService_CheckBox.Checked)
                {
                    devicedata.Authenticators.Add(HpacAuthenticators.IrmServer);
                }
                if (draService_CheckBox.Checked)
                {
                    devicedata.Authenticators.Add(HpacAuthenticators.DraServer);
                }
            }
            devicedata.DeviceOperation = selectedOperation;
            devicedata.Asset = (IDeviceInfo)ConfigurationServices.AssetInventory.GetAsset(assetSelectionControl.AssetSelectionData.SelectedAssets.FirstOrDefault());
            return devicedata;
        }

        /// <summary>
        /// Configures the controls per the device data either derived from initialization or the saved meta data.
        /// </summary>
        public void LoadConfiguration(DeviceTabData devicedata)
        {
            ClearCheckBoxes(this);

            switch (devicedata.DeviceOperation)
            {
                case DeviceOperation.Add:
                    addDeviceRadioButton.Checked = true;
                    break;
                case DeviceOperation.Configure:
                    configureRadioButton.Checked = true;
                    break;
                case DeviceOperation.InstallHPAgent:
                    installRadioButton.Checked = true;
                    break;
                default:
                    break;
            }

            foreach (var checkedItemText in devicedata.Configuration)
            {
                switch (checkedItemText)
                {
                    case HpacConfiguration.PullPrinting:
                        pullPrinting_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.Tracking:
                        tracking_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.Authentication:
                        authentication_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.Authorization:
                        authorization_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.Quota:
                        quota_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.ConfirmationTrap:
                        confirmationTrap_CheckBox.Checked = true;
                        break;
                  
                }
            }

            if (devicedata.Authenticators != null)
            {
                foreach (var devicedataAuthenticator in devicedata.Authenticators)
                {
                    switch (devicedataAuthenticator)
                    {
                        case HpacAuthenticators.LocalList:
                            localList_CheckBox.Checked = true;
                            break;
                        case HpacAuthenticators.PicServer:
                            picService_CheckBox.Checked = true;
                            break;
                        case HpacAuthenticators.IrmServer:
                            irmService_CheckBox.Checked = true;
                            break;
                        case HpacAuthenticators.DraServer:
                            draService_CheckBox.Checked = true;
                            break;
                    }
                }
            }

            if (devicedata.Asset != null)
            {
                assetSelectionControl.Initialize((new AssetSelectionData((AssetInfo)devicedata.Asset)), _deviceAttributes);
            }
            else
            {
                assetSelectionControl.Initialize(AssetAttributes.None);
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult AddValidaton()
        {
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Removes the Validation for the controls.
        /// </summary>
        public void RemoveValidation()
        {
            fieldValidator.Remove(assetSelectionControl);
        }

        private void ClearCheckBoxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                ClearCheckBoxes(ctrl);
            }
        }
    }
}
