using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using Size = System.Drawing.Size;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    /// <summary>
    /// Edit control for a the Activity Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class ControlPanelConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ControlPanelActivityData _activityData;

        private Type _deviceType;

        //private string PayloadPath = @"C:\Projects\Temp\Payloads.xml";
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPanelConfigurationControl"/> class.
        /// </summary>
        public ControlPanelConfigurationControl()
        {
            InitializeComponent();
            controlpanel_fieldValidator.RequireAssetSelection(controlpanel_assetSelectionControl);
            controlpanel_fieldValidator.RequireSelection(controlPanelOptions_comboBox, controlpaneloptions_label);
            controlpanel_fieldValidator.RequireSelection(controlpaneltypes_comboBox, controlpaneltype_label);

            controlpanel_assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged?.Invoke(s, e);
            controlPanelOptions_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged?.Invoke(s, e);
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new ControlPanelActivityData();
            controlpanel_assetSelectionControl.Initialize(AssetAttributes.ControlPanel);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<ControlPanelActivityData>();

            controlpanel_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);

            if (string.IsNullOrEmpty(_activityData.ControlPanelType))
            {
                try
                {
                    var deviceInfo = (DeviceInfo)ConfigurationServices.AssetInventory.GetAsset(configuration.Assets.SelectedAssets.First());
                    var device = DeviceConstructor.Create(deviceInfo);

                    if (device is JediWindjammerDevice)
                    {
                        controlpaneltypes_comboBox.SelectedItem = "Jedi";
                    }
                    else if (device is JediOmniDevice)
                    {
                        controlpaneltypes_comboBox.SelectedItem = "Omni";
                    }
                    else if (device is SiriusUIv2Device)
                    {
                        controlpaneltypes_comboBox.SelectedItem = "SiriusPentane";
                    }
                    else if (device is SiriusUIv3Device)
                    {
                        controlpaneltypes_comboBox.SelectedItem = "SiriusTriptane";
                    }
                    else
                    {
                        controlpaneltypes_comboBox.SelectedItem = "Phoenix";
                    }
                }
                catch (DeviceCommunicationException)
                {
                    controlpaneltypes_comboBox.SelectedIndex = -1;
                }

            }
            else
            {
                controlpaneltypes_comboBox.SelectedItem = _activityData.ControlPanelType;
            }
            controlPanelOptions_comboBox.Text = _activityData.ControlPanelAction;
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.ControlPanelAction = controlPanelOptions_comboBox.Text;
            _activityData.ControlPanelType = controlpaneltypes_comboBox.Text;
            WriteParameterValues();

            return new PluginConfigurationData(_activityData, "1.1")
            {
                Assets = controlpanel_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(controlpanel_fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Set Dictionary Values
        /// </summary>
        private void WriteParameterValues()
        {
            _activityData.ParameterValues.Clear();
            foreach (Control controlInGroupBox in settings_groupBox.Controls)
            {
                if (controlInGroupBox is TextBox)
                {
                    _activityData.ParameterValues.Add(controlInGroupBox.Name, controlInGroupBox.Text);
                }
                if (controlInGroupBox is ComboBox)
                {
                    _activityData.ParameterValues.Add(controlInGroupBox.Name, controlInGroupBox.Text);
                }
            }
        }

        private void controlpaneltypes_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlpaneltypes_comboBox.SelectedIndex == -1)
                return;

            var deviceType = controlpaneltypes_comboBox.SelectedItem.ToString();

            switch (deviceType)
            {
                case "Jedi":
                    {
                        _deviceType = typeof(JediWorkflow);
                    }
                    break;

                case "Omni":
                    {
                        _deviceType = typeof(OmniWorkflow);
                    }
                    break;

                case "Phoenix":
                    {
                        _deviceType = typeof(PhoenixWorkflow);
                    }
                    break;

                case "SiriusPentane":
                    {
                        _deviceType = typeof(SiriusPentaneWorkflow);
                    }
                    break;

                case "SiriusTriptane":
                    {
                        _deviceType = typeof(SiriusTriptaneWorkFlow);
                    }
                    break;
            }

            controlPanelOptions_comboBox.DataSource = GetMethodNames(_deviceType);
        }

        private static List<string> GetMethodNames(Type type)
        {
            return type.GetMethods().Where(x => x.ReturnType.Name == "Void").OrderBy(x => x.Name).Select(x => x.Name).ToList();
        }

        private void controlPanelOptions_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlPanelOptions_comboBox.SelectedIndex == -1)
                return;

            var methodName = controlPanelOptions_comboBox.SelectedItem.ToString();
            var parameters = _deviceType.GetMethod(methodName).GetParameters();
            var attributes = _deviceType.GetMethod(methodName).GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault();
            description_textBox.Text = attributes != null ? ((DescriptionAttribute)attributes).Description : string.Empty;
            PopulateParameterInfo(parameters);
        }

        private void PopulateParameterInfo(ParameterInfo[] parameters)
        {
            const int textBoxX = 200;
            var textBoxY = 39;
            const int labelX = 10;
            var labelY = 45;
            const int controlAlignmentSpace = 25;
            int labelCounter = 0;

            controlpanel_fieldValidator.Clear();
            controlpanel_fieldValidator.RequireAssetSelection(controlpanel_assetSelectionControl);
            controlpanel_fieldValidator.RequireSelection(controlPanelOptions_comboBox, controlpaneloptions_label);
            controlpanel_fieldValidator.RequireSelection(controlpaneltypes_comboBox, controlpaneltype_label);

            settings_groupBox.Controls.Clear();

            foreach (var parameter in parameters)
            {
                TextBox textBox = new TextBox();
                Label label = new Label
                {
                    AutoSize = true,
                    Location = new Point(labelX, labelY),
                    Name = $"{ (object)labelCounter}_label",
                    Size = new Size(87, 13),
                    TabIndex = 67,
                    Text = parameter.Name,
                    TextAlign = ContentAlignment.MiddleRight
                };

                label.AutoSize = true;
                settings_groupBox.Controls.Add(label);
                labelY += controlAlignmentSpace;
                if (parameter.ParameterType.IsEnum)
                {
                    ComboBox comboBox = new ComboBox
                    {
                        Location = new Point(textBoxX, textBoxY),
                        Name = parameter.Name,
                        Size = new Size(150, 20),
                        BindingContext = new BindingContext(),
                        DataSource = parameter.ParameterType.GetEnumNames()
                    };

                    comboBox.Refresh();
                    if (_activityData.ParameterValues.ContainsKey(comboBox.Name))
                    {
                        string temp;
                        _activityData.ParameterValues.TryGetValue(comboBox.Name, out temp);

                        if (!string.IsNullOrEmpty(temp))
                            comboBox.SelectedIndex = comboBox.Items.IndexOf(temp);

                    }

                    settings_groupBox.Controls.Add(comboBox);

                    Label labelParam = new Label
                    {
                        AutoSize = true,
                        Location = new Point(comboBox.Right + 10, comboBox.Top + 5),
                        Name = $"{ (object)labelCounter}_labelparam",
                        Size = new Size(50, 13),
                        TabIndex = 90,
                        Text = @"please select value from the list"
                    };

                    settings_groupBox.Controls.Add(labelParam);
                }
                else
                {
                    textBox.Name = parameter.Name;
                    textBox.Location = new Point(textBoxX, textBoxY);
                    if (_activityData.ParameterValues.ContainsKey(textBox.Name))
                    {
                        string temp;
                        _activityData.ParameterValues.TryGetValue(textBox.Name, out temp);
                        textBox.Text = temp;
                    }
                    else
                    {
                        textBox.Text = parameter.DefaultValue.ToString();
                    }
                    textBox.Size = new Size(150, 20);
                    controlpanel_fieldValidator.RequireCustom(textBox, () => ValidateInput(textBox.Text, parameter.ParameterType), $"Input Parameter of { (object)textBox.Name} is of incorrect type");

                    settings_groupBox.Controls.Add(textBox);

                    Label labelParam = new Label
                    {
                        AutoSize = true,
                        Location = new Point(textBox.Right + 10, textBox.Top + 5),
                        Name = $"{ (object)labelCounter}_labelparam",
                        Size = new Size(50, 13),
                        TabIndex = 90,
                        Text = $"please enter value of type {parameter.ParameterType.Name}"
                    };

                    settings_groupBox.Controls.Add(labelParam);
                }

                textBoxY += controlAlignmentSpace;
                labelCounter++;
            }
        }

        private static bool ValidateInput(string input, Type parameterType)
        {
            try
            {
                TypeDescriptor.GetConverter(parameterType).ConvertFromInvariantString(input);
            }
            catch (ArgumentNullException)
            {

                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }

            return !string.IsNullOrEmpty(input);

        }
    }
}