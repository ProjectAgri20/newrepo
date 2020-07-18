using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.GeniusBytesPullPrinting
{
    [ToolboxItem(false)]
    public partial class GeniusBytesPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private GeniusBytesPullPrintingActivityData _activityData;
        private PluginConfigurationData _pluginConfigurationData;
        private PluginEnvironment _environment;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesPullPrintingConfigurationControl" /> class.
        /// </summary>
        public GeniusBytesPullPrintingConfigurationControl()
        {
            InitializeComponent();

            //set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            //set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            radioButton_GuestLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_ManualLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_PINLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_ProximityCardLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_PrintAll.CheckedChanged += OnConfigurationChanged;
            radioButton_PrintAllandDelete.CheckedChanged += OnConfigurationChanged;
            radioButton_Print.CheckedChanged += OnConfigurationChanged;
            radioButton_PrintandDelete.CheckedChanged += OnConfigurationChanged;
            radioButton_Delete.CheckedChanged += OnConfigurationChanged;
            radioButton_DeleteAll.CheckedChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            checkBox_ColorModeNotification.CheckedChanged += OnConfigurationChanged;
            checkBox_DeletionNotification.CheckedChanged += OnConfigurationChanged;

            //Set Enums on Radio Button
            radioButton_PrintAll.Tag = GeniusBytesPullPrintAction.PrintAll;
            radioButton_PrintAllandDelete.Tag = GeniusBytesPullPrintAction.PrintAllandDelete;
            radioButton_Print.Tag = GeniusBytesPullPrintAction.Print;
            radioButton_PrintandDelete.Tag = GeniusBytesPullPrintAction.PrintandDelete;
            radioButton_Delete.Tag = GeniusBytesPullPrintAction.Delete;
            radioButton_DeleteAll.Tag = GeniusBytesPullPrintAction.DeleteAll;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _environment = environment;
            _activityData = new GeniusBytesPullPrintingActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetPullPrintAction();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<GeniusBytesPullPrintingActivityData>();
            _pluginConfigurationData = configuration;
            _environment = environment;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
            if (radioButton_GuestLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesGuest;
            }
            else if (radioButton_ManualLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesManual;
            }
            else if(radioButton_PINLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesPin;
            }
            else//radioButton_ProximityCardLogin.Checked
            {
                _activityData.AuthProvider = AuthenticationProvider.Card;
            }
            Console.WriteLine($"authprovider : {_activityData.AuthProvider}");
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.ShuffleDocuments = printingConfigurationControl.GetShuffle();
            _activityData.DelayAfterPrint = printingConfigurationControl.GetDelay();
            _activityData.UsePrintServerNotification = printingConfigurationControl.GetPrintServerNotification();
            _activityData.UseColorModeNotification = checkBox_ColorModeNotification.Checked;
            _activityData.UseDeletionNotification = checkBox_DeletionNotification.Checked;

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                PrintQueues = printingConfigurationControl.GetPrintQueues(),
                Documents = printingConfigurationControl.GetDocuments(),
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBox_ReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;
            checkBox_ColorModeNotification.Checked = _activityData.UseColorModeNotification;
            checkBox_DeletionNotification.Checked = _activityData.UseDeletionNotification;

            switch (_activityData.AuthProvider)
            {
                case AuthenticationProvider.GeniusBytesGuest:
                    radioButton_GuestLogin.Checked = true;
                    break;
                case AuthenticationProvider.GeniusBytesManual:
                    radioButton_ManualLogin.Checked = true;
                    break;
                case AuthenticationProvider.GeniusBytesPin:
                    radioButton_PINLogin.Checked = true;
                    break;
                case AuthenticationProvider.Card:
                    radioButton_ProximityCardLogin.Checked = true;
                    break;
            }
            SetPullPrintAction();
         }
        
        /// <summary>
        /// Set pull print action: Print or Delete
        /// </summary>
        private void SetPullPrintAction()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case GeniusBytesPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case GeniusBytesPullPrintAction.PrintAllandDelete:
                    radioButton_PrintAllandDelete.Checked = true;
                    break;
                case GeniusBytesPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case GeniusBytesPullPrintAction.PrintandDelete:
                    radioButton_PrintandDelete.Checked = true;
                    break;
                case GeniusBytesPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
                case GeniusBytesPullPrintAction.DeleteAll:
                    radioButton_DeleteAll.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Get pull print action: Print or Delete
        /// </summary>
        private GeniusBytesPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrint.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (GeniusBytesPullPrintAction)selected.Tag;
        }

        private void radioButton_PullPrinting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_PrintAll.Checked || radioButton_PrintAllandDelete.Checked)
            {
                _activityData.PrintAll = true;
            }
            else
            {
                _activityData.PrintAll = false;
            }
        }
    }
}
