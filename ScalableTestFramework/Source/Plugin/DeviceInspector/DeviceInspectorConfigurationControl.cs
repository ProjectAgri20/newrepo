using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsData;

namespace HP.ScalableTest.Plugin.DeviceInspector
{
    /// <summary>
    /// Provides the control to configure the DeviceConfiguration activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DeviceInspectorConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private DeviceInspectorActivityData _data = null;
        //List<Type> _modifiedControls = null;
        public event EventHandler ConfigurationChanged;

        public const string Version = "1.0";

        /// <summary>
        /// Initializes a new instance of the DeviceConfigurationConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public DeviceInspectorConfigurationControl()
        {
            InitializeComponent();
            //_modifiedControls = new List<Type>();
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            
            generalSettingsControl.ControlComponentChanged += ModifiedControl;
            emailSettingsControl.ControlComponentChanged += ModifiedControl;
            passwordWindowsControl.ControlComponentChanged += ModifiedControl;
            quickSetControl.ControlComponentChanged += ModifiedControl;
            copyDefaultControl.ControlComponentChanged += ModifiedControl;
            printDefaultControl.ControlComponentChanged += ModifiedControl;
            folderDefaultControl.ControlComponentChanged += ModifiedControl;
            scanToUsbDefaultControl.ControlComponentChanged += ModifiedControl;
            jobStorageDefaultControl.ControlComponentChanged += ModifiedControl;
            protocolDefaultControl.ControlComponentChanged += ModifiedControl;
            faxDefaultControl.ControlComponentChanged += ModifiedControl;
        }

        private void ModifiedControl(object sender, EventArgs e)
        {
            //Type temp = sender.GetType();
            //if (!_data.ModifiedControls.ContainsKey(temp))
            //{
            //    _data.ModifiedControls.Add(temp, (IComponentData)sender);
            //}
            //_data.ComponentData.Contains(((IGetSetComponentData)sender).GetData());
            //if (!_data.ComponentData.Contains(((IGetSetComponentData)sender).GetData()))
            //{


            //SHOULD BE MADE TO WORK WITH ALL CONTROL TYPES
            //generalSettingsControl.SetData();
            //_data.ComponentData.Add(generalSettingsControl.GetData());
            //}
            int index;
            if (generalSettingsControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(GeneralSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                generalSettingsControl.SetData();
                _data.ComponentData.Add(generalSettingsControl.GetData());
            }

            if (emailSettingsControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(EmailSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                emailSettingsControl.SetData();
                _data.ComponentData.Add(emailSettingsControl.GetData());
            }

            if (passwordWindowsControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(PasswordWindowsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                passwordWindowsControl.SetData();
                _data.ComponentData.Add(passwordWindowsControl.GetData());
            }

            if (quickSetControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(QuickSetSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                //quickSetControl.SetData();
                _data.ComponentData.Add(quickSetControl.GetData());

            }

            if (printDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(PrintSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                printDefaultControl.SetData();
                _data.ComponentData.Add(printDefaultControl.GetData());
            }

            if (copyDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(CopySettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                copyDefaultControl.SetData();
                _data.ComponentData.Add(copyDefaultControl.GetData());
            }
            if (faxDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(FaxSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                faxDefaultControl.SetData();
                _data.ComponentData.Add(faxDefaultControl.GetData());
            }

            if (folderDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(FolderSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                folderDefaultControl.SetData();
                _data.ComponentData.Add(folderDefaultControl.GetData());
            }

            if (scanToUsbDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(ScanUsbSettingData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                scanToUsbDefaultControl.SetData();
                _data.ComponentData.Add(scanToUsbDefaultControl.GetData());
            }

            if (jobStorageDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(JobSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                jobStorageDefaultControl.SetData();
                _data.ComponentData.Add(jobStorageDefaultControl.GetData());
            }

            if (protocolDefaultControl.Modified)
            {
                index = _data.ComponentData.IndexOf(_data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(ProtocolSettingsData)));
                if (index != -1)
                {
                    _data.ComponentData.RemoveAt(index);
                }
                protocolDefaultControl.SetData();
                _data.ComponentData.Add(protocolDefaultControl.GetData());
            }


        }


        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.EnableDefaultPW = enablePassword_CheckBox.Checked;
            //int convertedValue = -1;
            //bool success = int.TryParse(maxPasswordLength_Textbox.Text, out convertedValue);
            //_data.MaxPWLength = success ? convertedValue : -1;
            //_data.WindowsDomains.Clear();

            ////Basic Settings Data

            //if (!string.IsNullOrWhiteSpace(winDomains_TextBox.Text))
            //{
            //    _data.WindowsDomains.AddRange(winDomains_TextBox.Text.Split(';'));
            //}
            //_data.DefaultDomain = defaultDomain_TextBox.Text;
            //_data.EnablePasswordComplexity = passwordComplexity_CheckBox.Checked;
            //_data.EnableWindowsAuthentication = windowsAuth_CheckBox.Checked;



            return new PluginConfigurationData(_data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new DeviceInspectorActivityData();

            assetSelectionControl.Initialize(Framework.Assets.AssetAttributes.None);

        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<DeviceInspectorActivityData>();

            assetSelectionControl.Initialize(configuration.Assets, Framework.Assets.AssetAttributes.None);

            //DefaultSetupData
            enablePassword_CheckBox.Checked = _data.EnableDefaultPW;
          
            //GeneralSettingsData
            var gendata = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(GeneralSettingsData));
            if (gendata != null)
            {
                generalSettingsControl.SetControl(_data.ComponentData); //Alternatively just sending gendata
            }
            var emailData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(EmailSettingsData));
            if (emailData != null)
            {
                emailSettingsControl.SetControl(_data.ComponentData);
            }
            var passWinData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(PasswordWindowsData));
            if (passWinData != null)
            {
                passwordWindowsControl.SetControl(_data.ComponentData);
            }
            var quickSetData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(QuickSetSettingsData));
            if(quickSetData != null)
            {
                quickSetControl.SetControl(_data.ComponentData);
            }

            var copyData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(CopySettingsData));
            if (copyData != null)
            {
                copyDefaultControl.SetControl(_data.ComponentData);
            }

            var printData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(PrintSettingsData));
            if (printData != null)
            {
                printDefaultControl.SetControl(_data.ComponentData);
            }

            var faxData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(FaxSettingsData));
            if (faxData != null)
            {
                faxDefaultControl.SetControl(_data.ComponentData);
            }

            var folderData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(FolderSettingsData));
            if (folderData != null)
            {
                folderDefaultControl.SetControl(_data.ComponentData);
            }

            var scanUsbData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(ScanUsbSettingData));
            if (scanUsbData != null)
            {
                scanToUsbDefaultControl.SetControl(_data.ComponentData);
            }

            var jobData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(JobSettingsData));
            if (jobData != null)
            {
                jobStorageDefaultControl.SetControl(_data.ComponentData);
            }

            var protocolData = _data.ComponentData.FirstOrDefault(x => x.GetType() == typeof(ProtocolSettingsData));
            if (protocolData != null)
            {
                protocolDefaultControl.SetControl(_data.ComponentData);
            }

        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());


    }
}
