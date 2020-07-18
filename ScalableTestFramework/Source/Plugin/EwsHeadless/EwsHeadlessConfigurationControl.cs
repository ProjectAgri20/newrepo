using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;
using Size = System.Drawing.Size;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Edit control for a EWS DAT Activity Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class EwsHeadlessConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private EwsHeadlessActivityData _activityData;
        private Collection<string> _payloads = new Collection<string>();
        private Dictionary<string, string> _defaultValues = new Dictionary<string, string>();
        private Dictionary<string, string> _configurationDictionary = new Dictionary<string, string>();
        private string _payloadRepository;

        //private string payloadRepository = @"C:\EWSDATPayloads";
        /// <summary>
        /// Initializes a new instance of the <see cref="EwsHeadlessConfigurationControl"/> class.
        /// </summary>
        public EwsHeadlessConfigurationControl()
        {
            InitializeComponent();
            AddFieldValidation();

            ews_assetSelectionControl.SelectionChanged += ConfigurationChanged;
            deviceType_comboBox.SelectedIndexChanged += ConfigurationChanged;
            operation_comboBox.SelectedIndexChanged += ConfigurationChanged;
        }

        private void AddFieldValidation()
        {
            ews_fieldValidator.RequireAssetSelection(ews_assetSelectionControl);
            ews_fieldValidator.RequireSelection(operation_comboBox, operation_label);
            ews_fieldValidator.RequireSelection(deviceType_comboBox, deviceType_label);
        }

        /// <summary>
        /// Configuration chnage event handler
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initialises new UI
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            if (environment == null)
                return;

            _activityData = new EwsHeadlessActivityData();
            _payloadRepository = Path.Combine(environment.PluginSettings["DATPayLoadRepository"], "EWSPayLoads");
            LoadUi();
        }

        /// <summary>
        /// Initialise UI with existing data
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            if (configuration == null || environment == null)
                return;

            _payloadRepository = Path.Combine(environment.PluginSettings["DATPayLoadRepository"], "EWSPayLoads");
            _activityData = configuration.GetMetadata<EwsHeadlessActivityData>();
            ews_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            LoadUi();
        }

        /// <summary>
        /// Saves the Configurations
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.ConfigurationValues = _configurationDictionary;
            _activityData.Operation = operation_comboBox.SelectedItem.ToString();
            AddDynamicControlstoMetadata();

            if (device_TabControl.SelectedTab.Equals(Sirius_Tab))
            {
                if (siriusOperation_comboBox.SelectedItem != null)
                {
                    _activityData.DeviceSpecificOperation = siriusOperation_comboBox.SelectedItem.ToString();
                }
            }
            else if (device_TabControl.SelectedTab.Equals(Phoenix_Tab))
            {
                if (phoenixOperation_comboBox.SelectedItem != null)
                {
                    _activityData.DeviceSpecificOperation = phoenixOperation_comboBox.SelectedItem.ToString();
                }
            }
            else if (device_TabControl.SelectedTab.Equals(Jedi_Tab))
            {
                if (jediOperation_comboBox.SelectedItem != null)
                {
                    _activityData.DeviceSpecificOperation = jediOperation_comboBox.SelectedItem.ToString();
                }
            }
            else if (device_TabControl.SelectedTab.Equals(Oz_Tab))
            {
                if (ozOperation_comboBox.SelectedItem != null)
                {
                    _activityData.DeviceSpecificOperation = ozOperation_comboBox.SelectedItem.ToString();
                }
            }
            else if (device_TabControl.SelectedTab.Equals(Omni_Tab))
            {
                if (omniOperation_comboBox.SelectedItem != null)
                {
                    _activityData.DeviceSpecificOperation = omniOperation_comboBox.SelectedItem.ToString();
                }
            }
            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = ews_assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Validates the UI
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(ews_fieldValidator.ValidateAll());
        }

        //Adds the dynamically created controls to the metadata using Serializable Dictionary
        private void AddDynamicControlstoMetadata()
        {
            _configurationDictionary.Clear();
            foreach (Control controlInTab in GetAll(this, typeof(GroupBox)))
            {
                if (controlInTab.Name.Equals($"{deviceType_comboBox.Text}_groupBox"))
                {
                    foreach (Control controlInGroupBox in controlInTab.Controls)
                    {
                        if (controlInGroupBox is TextBox)
                        {
                            _configurationDictionary.Add(controlInGroupBox.Name, controlInGroupBox.Text);
                        }
                    }
                }
            }
        }

        private void LoadUi()
        {
            //var resourceSet = Payloads.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            //List<string> xmlFiles = (from DictionaryEntry entry in resourceSet select entry.Key.ToString()).ToList();
            List<string> xmlFiles = new List<string>();
            if (Directory.Exists(_payloadRepository))
            {
                xmlFiles.AddRange(Directory.GetFiles(_payloadRepository, "*.xml", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).Where(x => !xmlFiles.Contains(x)));
            }
            if (xmlFiles.Any())
            {
                xmlFiles = xmlFiles.OrderBy(x => x).ToList();
                operation_comboBox.DataSource = xmlFiles;

                deviceType_comboBox.DataSource = Enum.GetNames(typeof(DeviceType));
                if (ews_assetSelectionControl.HasSelection)
                {
                    operation_comboBox.SelectedIndex = operation_comboBox.Items.IndexOf(_activityData.Operation);

                    deviceType_comboBox.SelectedIndex = GetDeviceTypeIndex();
                }
                if (_activityData.ConfigurationValues.Count > 0)
                {
                    foreach (var configurationValue in _activityData.ConfigurationValues)
                    {
                        foreach (Control inputData in GetAll(this, typeof(TextBox)).Where(inputData => inputData.Name.Equals(configurationValue.Key)))
                        {
                            inputData.Text = configurationValue.Value;
                        }
                    }
                }

                _configurationDictionary = _activityData.ConfigurationValues;
            }
            else
            {
                MessageBox.Show(@"Payload repository does not exist. Please check and try again", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private int GetDeviceTypeIndex()
        {
            var selectedAsset = ConfigurationServices.AssetInventory.GetAsset(ews_assetSelectionControl.AssetSelectionData.SelectedAssets.First());
            var printDeviceInfo = (PrintDeviceInfo)selectedAsset;
            try
            {
                using (
                    var device = DeviceConstructor.Create(printDeviceInfo))
                {
                    if (device is JediWindjammerDevice)
                    {
                        return deviceType_comboBox.Items.IndexOf("Jedi");
                    }
                    if (device is JediOmniDevice)
                    {
                        return deviceType_comboBox.Items.IndexOf("Omni");
                    }
                    if (device is PhoenixDevice)
                    {
                        return deviceType_comboBox.Items.IndexOf("Phoenix");
                    }
                    if (device is SiriusDevice)
                    {
                        return deviceType_comboBox.Items.IndexOf("Sirius");
                    }
                    if (device is OzDevice)
                    {
                        return deviceType_comboBox.Items.IndexOf("Oz");
                    }
                    return deviceType_comboBox.Items.IndexOf("None");
                }
            }
            catch (DeviceCommunicationException ex)
            {
                MessageBox.Show($"An error occurred. Please resolve before executing. {ex.Message} ");
                return -1;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred. Please resolve before executing. {exc.Message} ");
                return -1;
            }
        }

        /// <summary>
        /// Gets all the controls including children,grandchildren etc
        /// </summary>
        private IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>().ToList();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        //Clears the controls before dynamically populating for the next operation selected
        private void ClearControls()
        {
            //Reset the combobox
            siriusOperation_comboBox.Items.Clear();
            phoenixOperation_comboBox.Items.Clear();
            ozOperation_comboBox.Items.Clear();
            jediOperation_comboBox.Items.Clear();
            omniOperation_comboBox.Items.Clear();
            siriusOperation_comboBox.ResetText();
            phoenixOperation_comboBox.ResetText();
            ozOperation_comboBox.ResetText();
            jediOperation_comboBox.ResetText();
            omniOperation_comboBox.ResetText();

            //clearing the groupboxes

            foreach (GroupBox currentControl in Controls.Cast<Control>().SelectMany(control => control.Controls.OfType<TabPage>().SelectMany(child => child.Controls.OfType<GroupBox>())))
            {
                currentControl.Controls.Clear();
            }
        }

        private void operation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControls();
            string operation = operation_comboBox.SelectedItem.ToString();

            //lets look in the payload repository first
            string filePath = Path.Combine(_payloadRepository, $"{operation}.xml");
            if (!File.Exists(filePath))
                return;
            var xDoc = XDocument.Load(filePath);
            IEnumerable<XElement> xmlNodeList = from element in xDoc.Descendants("Printer") select element;
            foreach (XElement node in xmlNodeList)
            {
                string subFilterType = node.Attribute("SubFilterType")?.Value;
                if (string.IsNullOrEmpty(subFilterType))
                {
                    subFilterType = operation_comboBox.SelectedItem.ToString();
                }

                //poulate the respective comboboxes with the types
                foreach (string deviceType in deviceType_comboBox.Items)
                {
                    if (node.Attribute("Type").Value.Equals(deviceType.ToUpper(CultureInfo.CurrentCulture)))
                    {
                        switch ((DeviceType)Enum.Parse(typeof(DeviceType), deviceType))
                        {
                            case DeviceType.Sirius:
                                {
                                    siriusOperation_comboBox.Items.Add(subFilterType);
                                }
                                break;

                            case DeviceType.Phoenix:
                                {
                                    phoenixOperation_comboBox.Items.Add(subFilterType);
                                }
                                break;

                            case DeviceType.Jedi:
                                {
                                    jediOperation_comboBox.Items.Add(subFilterType);
                                }
                                break;

                            case DeviceType.Oz:
                                {
                                    ozOperation_comboBox.Items.Add(subFilterType);
                                }
                                break;

                            case DeviceType.Omni:
                                {
                                    omniOperation_comboBox.Items.Add(subFilterType);
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Poulates controls dynamically based on the selected operation
        /// </summary>
        private void PopulateControls(DeviceType deviceType, string deviceSpecificOperation)
        {
            var textBoxX = 294;
            var textBoxY = 39;
            var labelX = 6;
            var labelY = 45;
            const int controlAlignmentSpace = 25;

            if (deviceSpecificOperation.Equals(operation_comboBox.SelectedItem.ToString()))
            {
                deviceSpecificOperation = string.Empty; //setting it to empty to match the subfilter type in the XML payloads
            }
            _payloads = GetPayloads(deviceType.ToString(), deviceSpecificOperation);//Retrieve corresponding payloads
            for (int i = 0; i < _payloads.Count; i++)
            {
                string tempPayload = _payloads.ElementAt(i);
                var pattern = new Regex(@"(?<=\{)[^}]*(?=\})");
                var payloadMatches = pattern.Matches(tempPayload); //Retrieve all attributes to be passed from payload(ie. XML)
                foreach (Control control in Controls)
                {
                    foreach (Control childControl in control.Controls)
                    {
                        if (!childControl.Name.Equals($"{deviceType}_Tab"))
                            continue;
                        foreach (Control controlInTab in childControl.Controls)
                        {
                            if (!(controlInTab is GroupBox))
                                continue;

                            foreach (var attribute in payloadMatches)
                            {
                                if ((attribute.ToString().EqualsIgnoreCase("HideId")) ||
                                    (attribute.ToString().EqualsIgnoreCase("WizardId")))
                                    continue;

                                //check if this parameter is already present
                                if (controlInTab.Controls.ContainsKey(attribute.ToString()))
                                    continue;
                                Label label = new Label
                                {
                                    AutoSize = true,
                                    Location = new Point(labelX, labelY),
                                    Name = $"{attribute}_label",
                                    Size = new Size(87, 13),
                                    TabIndex = 67,
                                    TextAlign = ContentAlignment.MiddleRight,
                                    Text = attribute.ToString()
                                };
                                controlInTab.Controls.Add(label);

                                TextBox textBox = new TextBox
                                {
                                    Name = attribute.ToString(),
                                    Location = new Point(textBoxX, textBoxY),
                                    Size = new Size(181, 20),

                                };
                                textBox.Text = _defaultValues.FirstOrDefault(defaultNode => defaultNode.Key.Equals(textBox.Name)).Value;
                                controlInTab.Controls.Add(textBox);

                                labelY += controlAlignmentSpace;
                                textBoxY += controlAlignmentSpace;
                            }
                        }
                    }
                }
            }
        }

        private void siriusOperation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sirius_groupBox.Controls.Clear();
            PopulateControls(DeviceType.Sirius, siriusOperation_comboBox.SelectedItem.ToString());
        }

        /// <summary>
        ///return payloads and populate default values
        /// </summary>
        private Collection<string> GetPayloads(string type, string subFilterType)
        {
            _defaultValues = new Dictionary<string, string>();
            string operation = operation_comboBox.SelectedItem.ToString();
            string filePath = Path.Combine(_payloadRepository, $"{operation}.xml");
            if (!File.Exists(filePath))
                return new Collection<string>();
            var xDoc = XDocument.Load(filePath);


            Collection<string> tempPayloads = new Collection<string>();
            XElement xmlNode = xDoc.Descendants("Printer").First(xmlNodes => xmlNodes.Attribute("Type")?.Value == type.ToUpper(CultureInfo.CurrentCulture) && xmlNodes.Attribute("SubFilterType")?.Value == subFilterType);

            var xmlPayloads = xmlNode.Descendants("Payload").Select(xmlElements => xmlElements);

            foreach (XElement node in xmlPayloads)
            {
                tempPayloads.Add(node.Value);
            }
            IEnumerable<XElement> xmlDefaultValues = from xmlElements in xmlNode.Descendants("Default") select xmlElements;

            foreach (XElement node in xmlDefaultValues)
            {
                _defaultValues.Add(node.Attribute("item")?.Value, node.Value);
            }
            return tempPayloads;
        }

        private void jediOperation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Jedi_groupBox.Controls.Clear();
            PopulateControls(DeviceType.Jedi, jediOperation_comboBox.SelectedItem.ToString());
        }

        private void omniOperation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Omni_groupBox.Controls.Clear();
            PopulateControls(DeviceType.Omni, omniOperation_comboBox.SelectedItem.ToString());
        }

        private void ozOperation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Oz_groupBox.Controls.Clear();
            PopulateControls(DeviceType.Oz, ozOperation_comboBox.SelectedItem.ToString());
        }

        private void phoenixOperation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Phoenix_groupBox.Controls.Clear();
            PopulateControls(DeviceType.Phoenix, phoenixOperation_comboBox.SelectedItem.ToString());
        }

        private void deviceType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ews_fieldValidator.Clear();
            AddFieldValidation();
            DeviceType selectedDeviceType = (DeviceType)Enum.Parse(typeof(DeviceType), deviceType_comboBox.SelectedItem.ToString());
            switch (selectedDeviceType)
            {
                case DeviceType.Sirius:
                    {
                        device_TabControl.SelectedTab = Sirius_Tab;
                        Phoenix_Tab.Enabled = false;
                        Jedi_Tab.Enabled = false;
                        Oz_Tab.Enabled = false;
                        Sirius_Tab.Enabled = true;
                        Omni_Tab.Enabled = false;
                        ews_fieldValidator.RequireSelection(siriusOperation_comboBox, siriusOperation_label, ValidationCondition.IfEnabled);
                        if (!string.IsNullOrEmpty(_activityData.DeviceSpecificOperation))
                        {
                            siriusOperation_comboBox.SelectedIndex = siriusOperation_comboBox.Items.IndexOf(_activityData.DeviceSpecificOperation);
                        }
                    }
                    break;

                case DeviceType.Phoenix:
                    {
                        device_TabControl.SelectedTab = Phoenix_Tab;
                        Sirius_Tab.Enabled = false;
                        Jedi_Tab.Enabled = false;
                        Oz_Tab.Enabled = false;
                        Phoenix_Tab.Enabled = true;
                        Omni_Tab.Enabled = false;
                        ews_fieldValidator.RequireSelection(phoenixOperation_comboBox, phoenixOperation_label, ValidationCondition.IfEnabled);
                        if (!string.IsNullOrEmpty(_activityData.DeviceSpecificOperation))
                        {
                            phoenixOperation_comboBox.SelectedIndex = phoenixOperation_comboBox.Items.IndexOf(_activityData.DeviceSpecificOperation);
                        }
                    }
                    break;

                case DeviceType.Jedi:
                    {
                        device_TabControl.SelectedTab = Jedi_Tab;
                        Phoenix_Tab.Enabled = false;
                        Sirius_Tab.Enabled = false;
                        Oz_Tab.Enabled = false;
                        Jedi_Tab.Enabled = true;
                        Omni_Tab.Enabled = false;
                        ews_fieldValidator.RequireSelection(jediOperation_comboBox, jediOperation_label, ValidationCondition.IfEnabled);
                        if (!string.IsNullOrEmpty(_activityData.DeviceSpecificOperation))
                        {
                            jediOperation_comboBox.SelectedIndex = jediOperation_comboBox.Items.IndexOf(_activityData.DeviceSpecificOperation);
                        }
                    }
                    break;

                case DeviceType.Oz:
                    {
                        device_TabControl.SelectedTab = Oz_Tab;
                        Phoenix_Tab.Enabled = false;
                        Jedi_Tab.Enabled = false;
                        Sirius_Tab.Enabled = false;
                        Oz_Tab.Enabled = true;
                        Omni_Tab.Enabled = false;
                        ews_fieldValidator.RequireSelection(ozOperation_comboBox, ozOperation_label, ValidationCondition.IfEnabled);
                        if (!string.IsNullOrEmpty(_activityData.DeviceSpecificOperation))
                        {
                            ozOperation_comboBox.SelectedIndex = ozOperation_comboBox.Items.IndexOf(_activityData.DeviceSpecificOperation);
                        }
                    }
                    break;

                case DeviceType.Omni:
                    {
                        device_TabControl.SelectedTab = Omni_Tab;
                        Phoenix_Tab.Enabled = false;
                        Jedi_Tab.Enabled = false;
                        Sirius_Tab.Enabled = false;
                        Oz_Tab.Enabled = false;
                        Omni_Tab.Enabled = true;
                        ews_fieldValidator.RequireSelection(omniOperation_comboBox, omniOperation_label, ValidationCondition.IfEnabled);
                        if (!string.IsNullOrEmpty(_activityData.DeviceSpecificOperation))
                        {
                            omniOperation_comboBox.SelectedIndex = omniOperation_comboBox.Items.IndexOf(_activityData.DeviceSpecificOperation);
                        }
                    }
                    break;

                case DeviceType.None:
                    {
                        Phoenix_Tab.Enabled = false;
                        Jedi_Tab.Enabled = false;
                        Sirius_Tab.Enabled = false;
                        Oz_Tab.Enabled = false;
                        Omni_Tab.Enabled = false;
                    }
                    break;
            }
        }
    }
}