using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using ComboBox = System.Windows.Forms.ComboBox;

namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    /// <summary>
    /// Provides the control to configure the DSSConfiguration activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DssConfigurationConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private DssConfigurationActivityData _data;
        private int _textBoxY = 20;
        private int _labelCounter;
        private const int ControlAlignmentSpace = 10;

        /// <summary>
        /// Initializes a new instance of the DSSConfigurationConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public DssConfigurationConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(tasksComboBox, tasknameLabel);
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.TaskName = tasksComboBox.SelectedItem.ToString();
            _data.ParameterValueDictionary.Clear();
            GetTaskParameters(_data.TaskName);
            return new PluginConfigurationData(_data, "1.0");
        }

        private void GetTaskParameters(string taskName)
        {
            var parameters = typeof(DssConfigurationTask).GetMethod(taskName).GetParameters();
            if (!parameters.Any())
                return;
            foreach (var parameterInfo in parameters)
            {
                if (parameterInfo.ParameterType.IsClass)
                {
                    var className = Activator.CreateInstance(parameterInfo.ParameterType.GetTypeInfo());
                    var properties = parameterInfo.ParameterType.GetTypeInfo().DeclaredProperties.ToList();
                    GetParameterInfo(className, properties);
                    _data.ParameterValueDictionary.Add(parameterInfo.ParameterType.Name, className);
                }
                else
                {
                    // get the control with this parameter name.
                    var paramControl = parametersGroupBox.Controls.Find(parameterInfo.Name, true).First();
                    var box = paramControl as CheckBox;
                    var propertyValue = box?.Checked ?? TypeDescriptor.GetConverter(parameterInfo.ParameterType).ConvertFromInvariantString(paramControl.Text);
                    _data.ParameterValueDictionary.Add(parameterInfo.ParameterType.Name, propertyValue);
                }
            }
        }

        private void GetParameterInfo(object classname, List<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.Namespace != null && (propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase)))
                {
                    var className = Activator.CreateInstance(propertyInfo.PropertyType.GetTypeInfo());
                    var propertiesInfo = propertyInfo.PropertyType.GetTypeInfo().DeclaredProperties.ToList();

                    GetParameterInfo(className, propertiesInfo);
                    propertyInfo.SetValue(classname, className);
                }
                else
                {
                    var property = classname.GetType().GetProperty(propertyInfo.Name);
                    // get the value from UI
                    var uiControl = parametersGroupBox.Controls.Find(propertyInfo.Name, true).First();
                    if (property != null && (property.PropertyType.GetInterface("IEnumerable") != null &&
                                             property.PropertyType != typeof(string)))
                    {
                        if (uiControl is ListBox)
                        {
                            var enumType = property.PropertyType.GetGenericArguments().First();
                            var selectedItemList = (IList)typeof(List<>).MakeGenericType(enumType).GetConstructor(new Type[0])?.Invoke(new object[0]);
                            var uiListBox = uiControl as ListBox;
                            foreach (var selectedItem in uiListBox.SelectedItems)
                            {
                                selectedItemList?.Add(Enum.Parse(enumType, selectedItem.ToString()));
                            }
                            property.SetValue(classname, selectedItemList);
                        }
                    }
                    else
                    {
                        var box = uiControl as CheckBox;
                        if (box != null)
                        {
                            if (property != null) property.SetValue(classname, box.Checked);
                        }
                        else
                        {
                            if (uiControl.Text.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                            {
                                if (property != null) property.SetValue(classname, string.Empty);
                            }
                            else
                            {
                                if (property != null)
                                {
                                    var propertyValue = TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(uiControl.Text);
                                    property.SetValue(classname, propertyValue);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new DssConfigurationActivityData();
        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<DssConfigurationActivityData>();
            if (string.IsNullOrEmpty(_data.TaskName)) return;
            tasksComboBox.SelectedIndex = -1;
            tasksComboBox.SelectedItem = _data.TaskName;
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void DSSConfigurationConfigurationControl_Load(object sender, EventArgs e)
        {
            tasksComboBox.DataSource =
                typeof(DssConfigurationTask).GetMethods()
                    .Where(x => x.ReturnType.Name == "Void")
                    .Select(x => x.Name)
                    .ToList();
            if (string.IsNullOrEmpty(_data.TaskName)) return;
            tasksComboBox.SelectedIndex = -1;
            tasksComboBox.SelectedItem = _data.TaskName;
        }

        private void TasksComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tasksComboBox.SelectedIndex == -1)
                return;

            _labelCounter = 0;
            int groupBoxY = 20;
            _textBoxY = 20;
            parametersGroupBox.Controls.Clear();
            fieldValidator.Clear();
            var methodName = tasksComboBox.SelectedItem.ToString();
            parametersGroupBox.Name = methodName;
            parametersGroupBox.Text = methodName;
            parametersGroupBox.AutoSize = true;
            var parameters = typeof(DssConfigurationTask).GetMethod(methodName).GetParameters();
            var attributes = typeof(DssConfigurationTask).GetMethod(methodName).GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault();
            description_textBox.Text = attributes != null ? ((DescriptionAttribute)attributes).Description : string.Empty;
            foreach (var parameterInfo in parameters)
            {
                var defaultobject = Activator.CreateInstance(parameterInfo.ParameterType.GetTypeInfo());
                if (parameterInfo.ParameterType.IsClass)
                {
                    var properties = parameterInfo.ParameterType.GetTypeInfo().DeclaredProperties;
                    GroupBox groupBoxClass = new GroupBox
                    {
                        AutoSize = true,
                        Name = parameterInfo.ParameterType.Name,
                        Text = parameterInfo.Name,
                        Location = new Point(10, groupBoxY)
                    };
                    object dssdata;
                    _data.ParameterValueDictionary.TryGetValue(parameterInfo.ParameterType.Name, out dssdata);
                    if (dssdata != null)
                    {
                        groupBoxClass.Tag = dssdata;
                    }
                    _textBoxY += PopulateParameters(properties, groupBoxClass, _textBoxY, defaultobject);
                    groupBoxY += groupBoxClass.PreferredSize.Height;
                    parametersGroupBox.Controls.Add(groupBoxClass);
                }
                else
                {
                    string description = string.Empty;
                    var descriptionAttribute = parameterInfo.GetCustomAttribute(typeof(DescriptionAttribute));
                    if (descriptionAttribute != null)
                    {
                        description = ((DescriptionAttribute)descriptionAttribute).Description;
                    }
                    _textBoxY = PopulateUi(parameterInfo.Name, _textBoxY, parameterInfo.ParameterType, parametersGroupBox, defaultobject, description);
                    _textBoxY += ControlAlignmentSpace;
                    _labelCounter++;
                }
            }
        }



        private int PopulateParameters(IEnumerable<PropertyInfo> properties, GroupBox groupBoxClass, int y, object defaultObject)
        {
            foreach (var propertyInfo in properties)
            {
                object defaultvalue = null;
                if (null != defaultObject)
                {
                    var memberInfo = defaultObject.GetType().GetProperty(propertyInfo.Name);
                    if (memberInfo != null)
                        defaultvalue = memberInfo
                            .GetValue(defaultObject, null);
                }
                if (propertyInfo.PropertyType.Namespace != null && (propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase)))
                {
                    var declaredProperties = propertyInfo.PropertyType.GetTypeInfo().DeclaredProperties;
                    GroupBox groupboxSubClass = new GroupBox
                    {
                        AutoSize = true,
                        Name = propertyInfo.PropertyType.Name,
                        Text = propertyInfo.Name,
                        Location = new Point(10, y)
                    };
                    if (groupBoxClass.Tag != null)
                    {
                        groupboxSubClass.Tag = propertyInfo.GetValue(groupBoxClass.Tag);
                    }

                    y += PopulateParameters(declaredProperties, groupboxSubClass, 20, defaultvalue);
                    groupBoxClass.Controls.Add(groupboxSubClass);
                }
                else
                {
                    string description = string.Empty;
                    var descriptionAttribute = propertyInfo.GetCustomAttribute(typeof(DescriptionAttribute));
                    if (descriptionAttribute != null)
                    {
                        description = ((DescriptionAttribute)descriptionAttribute).Description;
                    }
                    y = PopulateUi(propertyInfo.Name, y, propertyInfo.PropertyType, groupBoxClass, defaultvalue, description) + ControlAlignmentSpace;
                    _labelCounter++;
                }
            }

            return groupBoxClass.PreferredSize.Height + 20;
        }

        private int PopulateUi(string name, int y, Type type, GroupBox groupbox, object defaultValue, string description = "")
        {
            const int textBoxX = 200;
            const int labelX = 10;
            TextBox textBox = new TextBox();

            Label label = new Label
            {
                AutoSize = true,
                Location = new Point(labelX, y),
                Name = $"{_labelCounter}_label",
                Size = new Size(87, 13),
                TabIndex = 67,
                Text = name,
                TextAlign = ContentAlignment.MiddleRight
            };

            groupbox.Controls.Add(label);

            if (type.IsEnum)
            {
                ComboBox comboBox = new ComboBox
                {
                    Location = new Point(textBoxX, y),
                    Name = name,
                    Size = new Size(150, 20),
                    BindingContext = new BindingContext(),
                    DataSource = type.GetEnumValues()
                };
                workflowToolTip.SetToolTip(comboBox, description);
                if (groupbox.Tag != null)
                {
                    var propertyInfo = groupbox.Tag.GetType().GetProperty(name);
                    if (propertyInfo != null)
                        comboBox.SelectedIndex =
                            comboBox.Items.IndexOf(propertyInfo.GetValue(groupbox.Tag));
                }

                fieldValidator.RequireCustom(comboBox, () => ValidateInput(comboBox.Text, type), $"Input Parameter of { (object)comboBox.Name} is of incorrect type");
                y += 20;
                groupbox.Controls.Add(comboBox);
            }
            else if (type.GetInterface("IEnumerable") != null && type != typeof(string))
            {
                var firstOrDefault = type.GetGenericArguments().FirstOrDefault();
                if (firstOrDefault != null && firstOrDefault.IsEnum)
                {
                    ListBox listBox = new ListBox
                    {
                        Name = name,
                        Location = new Point(textBoxX, y),
                        Size = new Size(150, 50),
                        BindingContext = new BindingContext(),
                        SelectionMode = SelectionMode.MultiSimple,
                        DataSource = type.GetGenericArguments().First().GetEnumNames()
                    };
                    workflowToolTip.SetToolTip(listBox, description);
                    if (groupbox.Tag != null)
                    {
                        listBox.ClearSelected();
                        var propertyInfo = groupbox.Tag.GetType().GetProperty(name);
                        if (propertyInfo != null)
                        {
                            var selectedItemList = propertyInfo.GetValue(groupbox.Tag) as IEnumerable;
                            if (selectedItemList != null)
                                foreach (int index in selectedItemList.Cast<object>().Select(selectedItem => listBox.Items.IndexOf(selectedItem.ToString())).Where(index => index != -1))
                                {
                                    listBox.SetSelected(index, true);
                                }
                        }
                    }

                    groupbox.Controls.Add(listBox);
                    y += 50;
                }
            }
            else if (type == typeof(bool))
            {
                CheckBox checkBox = new CheckBox
                {
                    Location = new Point(textBoxX, y),
                    Name = name,
                    Size = new Size(150, 20)
                };
                if (groupbox.Tag != null)
                {
                    checkBox.DataBindings.Add("Checked", groupbox.Tag, name);
                }
                workflowToolTip.SetToolTip(checkBox, description);
                if (null != defaultValue)
                {
                    checkBox.Checked = (bool)defaultValue;
                }
                groupbox.Controls.Add(checkBox);
                y += 20;
            }
            else
            {
                textBox.Name = name;
                textBox.Size = new Size(150, 20);
                textBox.Location = new Point(textBoxX, y);
                if (null != defaultValue)
                {
                    textBox.Text = defaultValue.ToString();
                }
                if (groupbox.Tag != null)
                {
                    textBox.DataBindings.Add("Text", groupbox.Tag, name);
                }
                workflowToolTip.SetToolTip(textBox, description);
                groupbox.Controls.Add(textBox);
                fieldValidator.RequireCustom(textBox, () => ValidateInput(textBox.Text, type), $"Input Parameter of {textBox.Name} is of incorrect type");

                y += 20;
            }
            groupbox.Refresh();
            return y;
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