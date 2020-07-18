using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    /// <summary>
    /// Edit control for an Authentication plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class AuthenticationConfigControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.1";

        private const AssetAttributes _deviceAttributes = AssetAttributes.ControlPanel;
        private AuthenticationData _activityData = null;
        private Dictionary<string, List<string>> _providerMap = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _methodMap = new Dictionary<string, List<string>>();

        private List<string> _initializationMethods = new List<string>();

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationConfigControl"/> class.
        /// </summary>
        public AuthenticationConfigControl()
        {
            InitializeComponent();
            InitializeProviderMap();

            AuthInitMethod aim = new AuthInitMethod();
            comboBox_InitiatorButton.Items.Clear();
            comboBox_InitiatorButton.DataSource = _initializationMethods; 

            SignOutMethod signOutMethods = new SignOutMethod();
            comboBox_Unauthenticate.Items.Clear();
            comboBox_Unauthenticate.DataSource = signOutMethods.SignOutMethodValues;

            fieldValidator.RequireValue(comboBox_InitiatorButton, label_InitiatingButton);
            fieldValidator.RequireValue(comboBox_Unauthenticate, label_UnAuthenticateMethod);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            comboBox_InitiatorButton.SelectedValueChanged += OnConfigurationChanged;
            comboBox_AuthMethod.SelectedValueChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedValueChanged += OnConfigurationChanged;

            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            comboBox_Unauthenticate.SelectedValueChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            assetSelectionControl.Initialize(_deviceAttributes);
            _activityData = new AuthenticationData();
            BindControls();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="T:HP.ScalableTest.Framework.Plugin.PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            _activityData = configuration.GetMetadata<AuthenticationData>(ConverterProvider.GetMetadataConverters());
            BindControls();
        }

        /// <summary>
        /// Creates and returns a <see cref="T:HP.ScalableTest.Framework.Plugin.PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.InitiationButton = comboBox_InitiatorButton.Text;
            _activityData.AuthProvider = EnumUtil.GetByDescription<AuthenticationProvider>(comboBox_AuthProvider.Text);

            _activityData.AuthMethod = EnumUtil.GetByDescription<AuthenticationMethod>(comboBox_AuthMethod.Text);

            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.UnAuthenticateMethod = comboBox_Unauthenticate.Text;

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="T:HP.ScalableTest.Framework.Plugin.PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Binds the controls to the data object.
        /// </summary>
        private void BindControls()
        {
            // Set up data bindings
            comboBox_InitiatorButton.Text = _activityData.InitiationButton;
            comboBox_AuthMethod.Text = _activityData.AuthMethod.GetDescription();
            comboBox_AuthProvider.Text = _activityData.AuthProvider.GetDescription();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            comboBox_Unauthenticate.Text = _activityData.UnAuthenticateMethod;
        }

        /// <summary>
        /// Initializes the provider map which maps supported provider values to Initiator buttons.
        /// </summary>
        /// <param name="initiationValues">The initiation values.</param>
        private void InitializeProviderMap()
        {
            _initializationMethods = EnumUtil.GetDescriptions<InitializationMethod>().ToList();

            _methodMap.Add(InitializationMethod.SignIn.GetDescription(), new List<string>
            {
                AuthenticationMethod.Auto.GetDescription(), AuthenticationMethod.UserNamePWD.GetDescription(), AuthenticationMethod.Pin.GetDescription(),
                AuthenticationMethod.Badge.GetDescription(), AuthenticationMethod.BadgePin.GetDescription()
            });

            _methodMap.Add(InitializationMethod.Badge.GetDescription(), new List<string>
            {
                AuthenticationMethod.Badge.GetDescription(), AuthenticationMethod.BadgePin.GetDescription()
            });

            _providerMap.Add(AuthenticationMethod.Auto.GetDescription(), new List<string>
            {
                AuthenticationProvider.Auto.GetDescription(), AuthenticationProvider.HpacAgentLess.GetDescription(), AuthenticationProvider.EquitracWindows.GetDescription(),
                AuthenticationProvider.HpacWindows.GetDescription(), AuthenticationProvider.Windows.GetDescription()
            });

            _providerMap.Add(AuthenticationMethod.Badge.GetDescription(), new List<string>
            {
                AuthenticationProvider.Blueprint.GetDescription(), AuthenticationProvider.HpacAgentLess.GetDescription(), AuthenticationProvider.HpacDra.GetDescription(),
                AuthenticationProvider.HpacIrm.GetDescription(), AuthenticationProvider.HpRoamPin.GetDescription(),
                AuthenticationProvider.PaperCut.GetDescription(), AuthenticationProvider.PaperCutAgentless.GetDescription(), AuthenticationProvider.SafeCom.GetDescription()

            });

            _providerMap.Add(AuthenticationMethod.BadgePin.GetDescription(), new List<string>
            {
                AuthenticationProvider.Blueprint.GetDescription(), AuthenticationProvider.HpacAgentLess.GetDescription(), AuthenticationProvider.HpacDra.GetDescription(),
                AuthenticationProvider.HpacIrm.GetDescription(), AuthenticationProvider.HpRoamPin.GetDescription(),
                AuthenticationProvider.PaperCut.GetDescription(), AuthenticationProvider.PaperCutAgentless.GetDescription(), AuthenticationProvider.SafeCom.GetDescription()
            });

            _providerMap.Add(InitializationMethod.Equitrac.GetDescription(), new List<string>
            {
                AuthenticationProvider.Equitrac.GetDescription(),AuthenticationProvider.EquitracWindows.GetDescription(), AuthenticationProvider.Windows.GetDescription()
            });

            _providerMap.Add(AuthInitMethod.HpacSecurePullPrint, new List<string>
            {
                AuthenticationProvider.HpacDra.GetDescription(), AuthenticationProvider.HpacIrm.GetDescription(),AuthenticationProvider.HpacWindows.GetDescription(),
                AuthenticationProvider.Windows.GetDescription(), AuthenticationProvider.DSS.GetDescription()
            });

            _providerMap.Add(AuthInitMethod.SafeCom, new List<string>
            {
                AuthenticationProvider.SafeCom.GetDescription(), AuthenticationProvider.Windows.GetDescription(), AuthenticationProvider.DSS.GetDescription()
            });

            _providerMap.Add(AuthInitMethod.HpRoam, new List<string>
            {
                AuthenticationProvider.HpRoamPin.GetDescription()
            });

            _providerMap.Add("Default", new List<string>());
            _methodMap.Add("Default", new List<string>());

            foreach (var key in _methodMap.Keys)
            {
                if (key != "Auto-detect" && !key.Contains("Badge"))
                {
                    _methodMap[key].Add(AuthenticationProvider.Auto.GetDescription());
                }
            }

            // Add Auto to Everything that might use it
            foreach (var key in _providerMap.Keys)
            {
                if (key != "Auto-detect" && !key.Contains("Swipe"))
                {
                    _providerMap[key].Add(AuthenticationProvider.Auto.GetDescription());
                }
            }

        }

        private void comboBox_InitiatorButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_AuthMethod.DataSource = null;

            if (_methodMap.ContainsKey(comboBox_InitiatorButton.Text))
            {
                comboBox_AuthMethod.DataSource = _methodMap[comboBox_InitiatorButton.Text];
            }
            else if (_providerMap.ContainsKey(comboBox_InitiatorButton.Text))
            {
                comboBox_AuthMethod.DataSource = _methodMap["Default"];
                comboBox_AuthProvider.DataSource = _providerMap[comboBox_InitiatorButton.Text];
            }
            else
            {
                comboBox_AuthMethod.DataSource = _methodMap["Default"];
            }

            if(comboBox_InitiatorButton.Text.Equals("Do Not Sign In"))
            {
                comboBox_AuthMethod.Enabled = false;
                comboBox_AuthProvider.Enabled = false;
            }
            else
            {
                comboBox_AuthMethod.Enabled = true;
                comboBox_AuthProvider.Enabled = true;
            }
        }

        private void comboBox_AuthMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_AuthProvider.DataSource = null;
            string initMethod = comboBox_InitiatorButton.Text;

            if (_providerMap.ContainsKey(comboBox_AuthMethod.Text) && (initMethod.Equals("Sign In") || initMethod.Equals("Badge")))
            {
                comboBox_AuthProvider.DataSource = _providerMap[comboBox_AuthMethod.Text];
            }
            else
            {
                comboBox_AuthProvider.DataSource = _providerMap["Default"];
            }

        }
    }
}
