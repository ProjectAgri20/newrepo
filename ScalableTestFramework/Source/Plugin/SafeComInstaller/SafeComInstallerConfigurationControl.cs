using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.SafeComInstaller
{
    /// <summary>
    /// Provides a control that is used to configure the plug-in.
    /// </summary>
    /// <remarks>
    /// This class inherits from the <see cref="UserControl"/> class and implements the
    /// <see cref="IPluginConfigurationControl"/> interface.
    ///
    /// The <see cref="UserControl"/> class provides an empty control that can be used to contain
    /// other controls that are used to configure this plug-in.
    ///
    /// <seealso cref="IPluginConfigurationControl"/>
    /// <seealso cref="System.Windows.Forms.UserControl"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class SafeComInstallerConfigurationControl : UserControl, IPluginConfigurationControl
    {
        /// <summary>
        /// Create the definition of the data that will be used by this activity.  It will be
        /// instantiated later when the plug-in is started up.
        /// </summary>
        private SafeComInstallerActivityData _data;

        /// <summary>
        ///
        /// </summary>
        public SafeComInstallerConfigurationControl()
        {
            InitializeComponent();
            fieldValidator.RequireAssetSelection(safecom_assetSelectionControl);
            fieldValidator.RequireCustom(bundleFile_textBox, () => !(string.IsNullOrEmpty(bundleFile_textBox.Text) && (SafeComAdministratorAction)tasks_comboBox.SelectedItem == SafeComAdministratorAction.AddDevice),"Bundle file required");
            fieldValidator.RequireSelection(tasks_comboBox, label2);
            fieldValidator.RequireSelection(safecom_serverComboBox, safecomserver_label);
            fieldValidator.RequireCustom(logon_textBox, ()=> !(string.IsNullOrEmpty(logon_textBox.Text) && (SafeComAdministratorAction)tasks_comboBox.SelectedItem == SafeComAdministratorAction.RegisterDevice), "Logon information required");
            fieldValidator.RequireCustom(pincode_textBox, () => !(string.IsNullOrEmpty(pincode_textBox.Text) && (SafeComAdministratorAction)tasks_comboBox.SelectedItem == SafeComAdministratorAction.RegisterDevice), "PIN information required");
            tasks_comboBox.DataSource = EnumUtil.GetValues<SafeComAdministratorAction>().ToList();
            loginType.DataSource = EnumUtil.GetDescriptions<SafeComLoginType>().ToList();
            prefilDomain.DataSource = EnumUtil.GetDescriptions<SafeComDomain>().ToList();
            //load the default values
            loginType.SelectedIndex = 2;
            prefilDomain.SelectedIndex = 0;
        }

        #region IPluginConfigurationControl implementation

        /// <summary>
        /// This event indicates to the framework that the user has changed something in the
        /// configuration; it will be used to keep track of unsaved changes so the user can be
        /// notified. This event should be raised whenever the user makes a change that modifies
        /// the configuration that the control would return.
        ///
        /// Failure to use this event will not prevent metadata from saving successfully; however,
        /// the user will not be prompted if they attempt to navigate away from the activity
        /// without saving their changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// This method should return a new <see cref="PluginConfigurationData"/> object containing
        /// all the configuration from the control. (This is the same object used in Initialize.)
        /// Custom metadata is passed into the constructor, either as XML or an object that will be
        /// serialized. The metadata version can be set to any value other than null.
        ///
        /// Selection data for assets and documents is set using the Assets and Documents
        /// properties. Plug-ins that will not make use of Assets and/or Documents can ignore these
        /// properties.
        ///
        /// <seealso cref="PluginConfigurationData"/>
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data = new SafeComInstallerActivityData
            {
                BundleFile = bundleFile_textBox.Text,
                SafeComAction = (SafeComAdministratorAction) tasks_comboBox.SelectedItem,
                SafeComConfigurationCollection = GetSafeComConfiguration()
            };
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = safecom_assetSelectionControl.AssetSelectionData,
                Servers = new ServerSelectionData(safecom_serverComboBox.SelectedServer)
            };
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        ///
        /// <seealso cref="PluginEnvironment"/>
        /// </summary>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data.
            _data = new SafeComInstallerActivityData();
            safecom_assetSelectionControl.Initialize(AssetAttributes.ControlPanel);
            bundleFile_textBox.Text = string.Empty;
            safecom_serverComboBox.Initialize("SafeCom");
        }

        /// <summary>
        /// Provides plug-in configuration data for an existing activity, including plug-in
        /// specific metadata, a metadata version, and selections of assets and documents.
        ///
        /// The custom metadata can be retrieved from the configuration using one of the
        /// <c>GetMetadata</c> methods, which return either a deserialized object or the XML. This
        /// data is then used to populate the configuration control. Asset and Document information
        /// can be retrieved using. The metadata version is included for forwards compatibility.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of the
            // configuration information.
            _data = configuration.GetMetadata<SafeComInstallerActivityData>();
            safecom_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);
            safecom_serverComboBox.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "SafeCom");
            bundleFile_textBox.Text = _data.BundleFile;
            tasks_comboBox.SelectedItem = _data.SafeComAction;
            UpdateSafeComConfigurationUi(_data.SafeComConfigurationCollection);
        }



        /// <summary>
        /// This method is used for validating the data entered on the control before saving.
        ///
        /// This method must return a <see cref="PluginValidationResult"/> indicating whether
        /// validation was successful, and if not, the error message(s) to present to the user.
        ///
        /// <seealso cref="PluginValidationResult"/>
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult"/> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data.
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion IPluginConfigurationControl implementation

        /// <summary>
        /// This method should be called when the configuration of the plug-in changes. It will
        /// raise the 'ConfigurationChanged' event that will eventually inform the user that they
        /// need to save the configuration.
        /// </summary>
        /// <param name="e">Contains any event data that should be sent with the event.</param>
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void browse_button_Click(object sender, EventArgs e)
        {
            bundle_openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = @"SafeCom Go HP software (*.b89;*.b95) | *.b89; *.b95"
            };

            if (bundle_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bundleFile_textBox.Text = bundle_openFileDialog.FileName;
            }
        }

        private NameValueCollection GetSafeComConfiguration()
        {
            NameValueCollection safeComCollection;
            if ((SafeComAdministratorAction) tasks_comboBox.SelectedItem == SafeComAdministratorAction.RegisterDevice)
            {
                safeComCollection = new NameValueCollection
                {
                    {"regUserLogin", logon_textBox.Text},
                    {"regPinCode", pincode_textBox.Text},
                    {"BtnRegisterDev", "Register"}

                };
                return safeComCollection;
            }

            safeComCollection = new NameValueCollection
            {
                {"groupName", safecom_serverComboBox.SelectedServer.Address},
                {"tcpPort", "7500"},
                {"SERVER_IPS", safecom_serverComboBox.SelectedServer.Address},
                {"NewServer", string.Empty},
                {"ASYNC_STR", "1"},
                {"SYNC_STR", "1"},
                {"devPw", string.Empty},
                {"devSNMPEngine", "SNMPv1v2Old"},
                {"devSetCommunityName", "public"},
                {"devGetCommunityName", "public"},
                {"devName", string.Empty},
                {"devModel", string.Empty},
                {"devLocation", string.Empty},
                {"devContact", string.Empty}
            };

            //add login settings
            if ((SafeComAdministratorAction) tasks_comboBox.SelectedItem ==
                SafeComAdministratorAction.UpdateConfiguration || (SafeComAdministratorAction)tasks_comboBox.SelectedItem ==
                SafeComAdministratorAction.InitialConfiguration)
            {
                safeComCollection.Add("loginType",
                    $"{(int) EnumUtil.GetByDescription<SafeComLoginType>(loginType.SelectedItem.ToString())}");
                safeComCollection.Add("devDomain", devDomain.Text);
                safeComCollection.Add("prefilDomain",
                    $"{(int) EnumUtil.GetByDescription<SafeComDomain>(prefilDomain.SelectedItem.ToString())}");


                foreach (Control control in login_groupBox.Controls)
                {
                    if (control is CheckBox)
                    {
                        if (((CheckBox) control).Checked)
                        {
                            safeComCollection.Add(control.Name, "on");
                        }
                    }
                }

                foreach (Control control in print_groupBox.Controls)
                {
                    if (control is CheckBox)
                    {
                        if (((CheckBox) control).Checked)
                        {
                            safeComCollection.Add(control.Name, "on");
                        }
                    }
                }

                foreach (Control control in tracking_groupBox.Controls)
                {
                    if (control is CheckBox)
                    {
                        if (((CheckBox) control).Checked)
                        {
                            safeComCollection.Add(control.Name, "on");
                        }
                    }
                }
            }

            safeComCollection.Add("BtnSave", "Apply");
            safeComCollection.Add("NewDriver", string.Empty);

            return safeComCollection;
        }

        private void UpdateSafeComConfigurationUi(NameValueCollection dataSafeComConfigurationCollection)
        {

            if ((SafeComAdministratorAction)tasks_comboBox.SelectedItem == SafeComAdministratorAction.RegisterDevice)
            {
                logon_textBox.Text = dataSafeComConfigurationCollection["regUserLogin"];
                pincode_textBox.Text = dataSafeComConfigurationCollection["regPinCode"];
                return;
            }
            //update the login settings
            loginType.SelectedIndex = Convert.ToInt32(dataSafeComConfigurationCollection["loginType"]) - 1;
            prefilDomain.SelectedIndex = Convert.ToInt32(dataSafeComConfigurationCollection["prefilDomain"]) - 1;
            devDomain.Text = dataSafeComConfigurationCollection["devDomain"];
            
            foreach (Control control in login_groupBox.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox) control).Checked = dataSafeComConfigurationCollection[control.Name] == "on";
                }
            }

            foreach (Control control in print_groupBox.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox) control).Checked = dataSafeComConfigurationCollection[control.Name] == "on";
                }
            }

            foreach (Control control in tracking_groupBox.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox) control).Checked = dataSafeComConfigurationCollection[control.Name] == "on";
                }
            }
        }

        private void tasks_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tasks_comboBox.SelectedIndex)
            {
                case 0:
                case 1:
                {
                    login_groupBox.Enabled = false;
                    print_groupBox.Enabled = false;
                    tracking_groupBox.Enabled = false;
                    register_groupBox.Enabled = false;
                }
                    break;

                case 2:
                case 3:
                {
                    login_groupBox.Enabled = true;
                    print_groupBox.Enabled = true;
                    tracking_groupBox.Enabled = true;
                    register_groupBox.Enabled = false;

                    }
                    break;

                case 4:
                {
                    login_groupBox.Enabled = false;
                    print_groupBox.Enabled = false;
                    tracking_groupBox.Enabled = false;
                    register_groupBox.Enabled = true;
                    }
                    break;
            }
        }
    }
}