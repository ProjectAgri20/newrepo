using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpRoam
{
    [ToolboxItem(false)]
    public partial class HpRoamConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const string Version = "1.0";
        private PluginConfigurationData _pluginConfigurationData;
        private HpRoamActivityData _activityData;
        private MobileDeviceInfo _mobileAsset = null;
        private List<KeyValuePair<AuthenticationProvider, string>> _deviceAuthProviders = null;
        private List<KeyValuePair<AuthenticationProvider, string>> _phoneAuthProviders = null;

        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpRoamConfigurationControl" /> class.
        /// </summary>
        public HpRoamConfigurationControl()
        {
            InitializeComponent();

            // Set up Authentication Provider Lists
            _deviceAuthProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            _deviceAuthProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            _deviceAuthProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpRoamPin, AuthenticationProvider.HpRoamPin.GetDescription()));
            _deviceAuthProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpId, AuthenticationProvider.HpId.GetDescription()));

            _phoneAuthProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            _phoneAuthProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpId, AuthenticationProvider.HpId.GetDescription()));

            //Set up field Validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "HP Roam Asset");
            fieldValidator.SetIconAlignment(assetSelectionControl, ErrorIconAlignment.TopLeft);
            fieldValidator.RequireDocumentSelection(documentSelectionControl, ValidationCondition.IfEnabled);
            fieldValidator.SetIconAlignment(documentSelectionControl, ErrorIconAlignment.TopLeft);

            fieldValidator.RequireValue(textBox_Description, "Mobile Device Selection", ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(textBox_PhoneDocument, label_documentToPush, ValidationCondition.IfEnabled);
            fieldValidator.SetIconAlignment(textBox_PhoneDocument, ErrorIconAlignment.MiddleLeft);

            //set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButton_RoamApp.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            documentSelectionControl.SelectionChanged += OnConfigurationChanged;
            numericUpDown_PullPrintDelay.ValueChanged += OnConfigurationChanged;

            //Set Enums on Radio Buttons
            radioButton_Print.Tag = HpRoamPullPrintAction.Print;
            radioButton_Delete.Tag = HpRoamPullPrintAction.Delete;
            radioButton_PrintAll.Tag = HpRoamPullPrintAction.PrintAll;           
        }


        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new HpRoamActivityData();
            _printQueues.Clear();
            assetSelectionControl.Initialize(AssetAttributes.None);
            var allExtensions = ConfigurationServices.DocumentLibrary.GetExtensions();
            var pdfs = allExtensions.Where(n => n.FileType.Equals("pdf", StringComparison.OrdinalIgnoreCase));

            documentSelectionControl.Initialize(pdfs);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);

            SetPhoneUse();
            SetAuthenticationOptions();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<HpRoamActivityData>();
            _pluginConfigurationData = configuration;

            _mobileAsset = ConfigurationServices.AssetInventory.GetAssets(_pluginConfigurationData.Assets.SelectedAssets).FirstOrDefault(n => n.Attributes.HasFlag(AssetAttributes.Mobile)) as MobileDeviceInfo;

            if (_mobileAsset != null)
            {
                _pluginConfigurationData.Assets.SelectedAssets.Remove(_mobileAsset.AssetId);
            }

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, AssetAttributes.None);
            documentSelectionControl.Initialize(configuration.Documents);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            printServerNotificationcheckBox.Checked = _activityData.UsePrintServerNotification;
            numericUpDown_DelayAfterPrint.Value = _activityData.DelayAfterPrint;
            numericUpDown_PullPrintDelay.Value = _activityData.DelayBeforePullPrint;
            radioButton_RoamApp.Checked = _activityData.HPRoamAuthentication;
            radioButton_SignInButton.Checked = !_activityData.HPRoamAuthentication;

            switch (_activityData.RoamDocumentSendAction)
            {
                case DocumentSendAction.Windows:
                    radioButton_PrintViaDriver.Checked = true;
                    break;
                case DocumentSendAction.Android:
                    radioButton_PrintViaPhone.Checked = true;
                    break;
                case DocumentSendAction.WebClient:
                    radioButton_PrintViaWebClient.Checked = true;
                    break;
            }

            if (_activityData.PhoneDocumentPush)
            {
                radioButton_PullPrintPhone.Checked = true;
                SetPhoneConfiguration();
            }
            else
            {
                radioButton_PullPrintOxpd.Checked = true;
                SetOxpdConfiguration();
            }
            SetAuthenticationOptions();

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;
            textBox_PhoneDocument.Text = _activityData.PhoneDocument;

        }

        private void SetOxpdConfiguration()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case HpRoamPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
                case HpRoamPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case HpRoamPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
            }
            SetPhoneUse();
        }

        private void SetPhoneConfiguration()
        {
            textBox_AssetId.Text = _mobileAsset?.AssetId;
            textBox_PhoneId.Text = _mobileAsset?.MobileEquipmentId;
            textBox_Description.Text = _mobileAsset?.Description;

            switch (_activityData.AndroidDocumentAction)
            {
                case RoamAndroidAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
                case RoamAndroidAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case RoamAndroidAction.Print:
                    radioButton_Print.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.HPRoamAuthentication = radioButton_RoamApp.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.DelayBeforePullPrint = (int)numericUpDown_PullPrintDelay.Value;

            _activityData.ShuffleDocuments = shuffle_CheckBox.Checked;
            _activityData.PhoneDocumentPush = radioButton_PullPrintPhone.Checked;
            _activityData.PhoneDocument = textBox_PhoneDocument.Text;
            _activityData.RoamDocumentSendAction = GetDocumentSendAction();

            AssetSelectionData assetSelectionData = assetSelectionControl.AssetSelectionData;

            if (radioButton_PullPrintPhone.Checked)
            {
                assetSelectionData.SelectedAssets.Add(_mobileAsset.AssetId);
                _activityData.MobileEquipmentId = _mobileAsset.MobileEquipmentId;
            }
            _activityData.AndroidDocumentAction = GetAndroidDocAction();

            LocalPrintQueueInfo info = new LocalPrintQueueInfo("HP Roam");
            _printQueues.Add(info);
            _activityData.DelayAfterPrint = GetDelayAfterPrint();
            _activityData.UsePrintServerNotification = printServerNotificationcheckBox.Checked;

            int count = documentSelectionControl.DocumentSelectionData.SelectedDocuments.Count();

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionData,
                Documents = documentSelectionControl.DocumentSelectionData,
                PrintQueues = new PrintQueueSelectionData(_printQueues)
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
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        private HpRoamPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (HpRoamPullPrintAction)selected.Tag;            
        }

        private RoamAndroidAction GetAndroidDocAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (RoamAndroidAction)selected.Tag;
        }

        private void radioButton_PullPrintPhone_CheckedChanged(object sender, EventArgs e)
        {
            SetPhoneUse();
            SetAuthenticationOptions();
        }

        private void radioButton_PullPrintOperation_CheckedChanged(object sender, EventArgs e)
        {
            label_PullPrintDelay.Enabled = !radioButton_Delete.Checked;
            numericUpDown_PullPrintDelay.Enabled = !radioButton_Delete.Checked;
            OnConfigurationChanged(sender, e);
        }

        private void SetPhoneUse()
        {
            // Set OXPd Pull Printing use
            radioButton_Delete.Enabled = radioButton_PullPrintOxpd.Checked;


            // Set AuthenticationMethod
            radioButton_SignInButton.Enabled = radioButton_PullPrintOxpd.Checked;
            radioButton_RoamApp.Enabled = radioButton_PullPrintOxpd.Checked;

            // Set Phone info controls if being used by Pull Print Or Print
            bool enabled = radioButton_PullPrintPhone.Checked || radioButton_PrintViaPhone.Checked;
            textBox_PhoneId.Enabled = enabled;
            textBox_AssetId.Enabled = enabled;
            textBox_Description.Enabled = enabled;
            button_PhoneSelect.Enabled = enabled;

            if (!enabled)
            {
                //Clear phone info if not using phone
                textBox_PhoneId.Text = string.Empty;
                textBox_AssetId.Text = string.Empty;
                textBox_Description.Text = string.Empty;
            }
        }

        private void SetAuthenticationOptions()
        {
            if (radioButton_PullPrintOxpd.Checked)
            {
                comboBox_AuthProvider.DataSource = _deviceAuthProviders;
                return;
            }

            comboBox_AuthProvider.DataSource = _phoneAuthProviders;
        }

        private DocumentSendAction GetDocumentSendAction()
        {
            DocumentSendAction dsa = DocumentSendAction.Windows;

            if (radioButton_PrintViaPhone.Checked)
            {
                dsa = DocumentSendAction.Android;
            }
            // when web client code is written will need to add the send action for it.            

            return dsa;
        }

        private void CheckedChanged_RoamDocSend(object sender, EventArgs e)
        {
            DocumentSendAction docSendAction = GetDocumentSendAction();
            switch (docSendAction)
            {
                case DocumentSendAction.Windows:
                    documentSelectionControl.Enabled = true;
                    shuffle_CheckBox.Enabled = true;
                    label_documentToPush.Enabled = false;
                    textBox_PhoneDocument.Enabled = false;
                    textBox_PhoneDocument.Text = string.Empty;
                    break;
                case DocumentSendAction.Android:
                    documentSelectionControl.Enabled = false;
                    documentSelectionControl.ClearSelection();
                    shuffle_CheckBox.Enabled = false;
                    label_documentToPush.Enabled = true;
                    textBox_PhoneDocument.Enabled = true;
                    break;
                case DocumentSendAction.WebClient:
                    documentSelectionControl.Enabled = true;
                    shuffle_CheckBox.Enabled = true;
                    label_documentToPush.Enabled = false;
                    textBox_PhoneDocument.Enabled = false;
                    textBox_PhoneDocument.Text = string.Empty;
                    break;
            }
            SetPhoneUse();
        }

        /// <summary>
        /// Gets the delay.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetDelayAfterPrint() => (int)numericUpDown_DelayAfterPrint.Value;

        private void Button_PhoneSelect_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm assetSelectionForm = new AssetSelectionForm(AssetAttributes.Mobile, textBox_PhoneId.Text, false))
            {
                assetSelectionForm.ShowDialog(this);
                if (assetSelectionForm.DialogResult == DialogResult.OK)
                {
                    _mobileAsset = (MobileDeviceInfo)assetSelectionForm.SelectedAssets.FirstOrDefault();
                    if (_mobileAsset != null)
                    {
                        textBox_AssetId.Text = _mobileAsset.AssetId;
                        textBox_Description.Text = _mobileAsset.Description;
                        textBox_PhoneId.Text = _mobileAsset.MobileEquipmentId;                        
                    }
                }
            }
        }

        private void PrintServerNotificationcheckBox_CheckedChanged(object sender, EventArgs e)
        {
                numericUpDown_DelayAfterPrint.Enabled = !printServerNotificationcheckBox.Checked;
        }

        private void comboBox_AuthProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isHpId = ((KeyValuePair<AuthenticationProvider, string>)comboBox_AuthProvider.SelectedItem).Key == AuthenticationProvider.HpId;
            radioButton_RoamApp.Checked = isHpId || radioButton_RoamApp.Checked;
            radioButton_SignInButton.Enabled = !isHpId;
        }

    }
}
