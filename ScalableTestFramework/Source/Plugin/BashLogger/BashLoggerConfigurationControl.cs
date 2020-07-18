using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Plugin.BashLogger.BashLog;

namespace HP.ScalableTest.Plugin.BashLogger
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
    public partial class BashLoggerConfigurationControl : UserControl, IPluginConfigurationControl
    {
        /// <summary>
        /// Create the definition of the data that will be used by this activity.  It will be
        /// instantiated later when the plug-in is started up.
        /// </summary>
        private BashLoggerActivityData _data = null;

        private string _bashCollectorServiceHost;

        /// <summary>
        /// 
        /// </summary>
        public BashLoggerConfigurationControl()
        {
            InitializeComponent();
           
            fieldValidator.RequireAssetSelection(assetSelectionControl_bashLog);
            fieldValidator.RequireValue(textBoxLoggerDirectory, label_logLocation, ValidationCondition.IfEnabled);
            fieldValidator.RequireCustom(comboBoxLoggerAction,()=>comboBoxLoggerAction.Enabled, "BashLogCollectorServiceHost Setting is required");
            comboBoxLoggerAction.DataSource = Enum.GetValues(typeof(LoggerAction));
            
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
            _data.FolderPath = textBoxLoggerDirectory.Text;
            _data.FileSplitSize =(int) numericUpDownFileSplitSize.Value;
            _data.LoggerAction = (LoggerAction)comboBoxLoggerAction.SelectedItem;
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl_bashLog.AssetSelectionData
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
            _data = new BashLoggerActivityData();
            assetSelectionControl_bashLog.Initialize(AssetAttributes.Printer);
           BashCollectorServiceHostCheck(environment);
        }

        /// <summary>
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of the
            // configuration information.
            _data = configuration.GetMetadata<BashLoggerActivityData>();
            assetSelectionControl_bashLog.Initialize(configuration.Assets,AssetAttributes.Printer);
            BashCollectorServiceHostCheck(environment);
            if(comboBoxLoggerAction.Enabled)
                comboBoxLoggerAction.SelectedIndex = comboBoxLoggerAction.Items.IndexOf(_data.LoggerAction);

            if (Directory.Exists(_data.FolderPath))
            {
                textBoxLoggerDirectory.Text = _data.FolderPath;
            }

           
        }

        private void BashCollectorServiceHostCheck(PluginEnvironment environment)
        {
            if (environment.PluginSettings.ContainsKey("BashLogCollectorServiceHost"))
            {
                _bashCollectorServiceHost = environment.PluginSettings["BashLogCollectorServiceHost"];
                comboBoxLoggerAction.Enabled = true;

            }
            else
            {
                MessageBox.Show(
                    "Please define the setting BashLogCollectorServiceHost in PluginSettings to enable BashLog Collection",
                    "Missing Plugin Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxLoggerAction.Enabled = false;

            }
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

        #endregion

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

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the save path for log files...";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _data.FolderPath = fbd.SelectedPath;
                    textBoxLoggerDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void comboBoxLoggerAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxLoggerDirectory.Enabled = buttonBrowse.Enabled = numericUpDownFileSplitSize.Enabled = comboBoxLoggerAction.SelectedIndex  == 2;
        }
    }
}
