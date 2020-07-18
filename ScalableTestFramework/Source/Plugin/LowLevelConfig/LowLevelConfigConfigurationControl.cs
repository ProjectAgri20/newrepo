using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    /// <summary>
    /// Provides the control to configure the LowLevelConfig activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class LowLevelConfigConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private LowLevelConfigActivityData _data = null;
        private NVRAMOptions _NVRAMOptions = new NVRAMOptions();

        public const string Version = "1.0";

        /// <summary>
        /// Initializes a new instance of the LowLevelConfigConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public LowLevelConfigConfigurationControl()
        {
            InitializeComponent();

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");


            assetSelectionControl.SelectionChanged += OnConfigurationChanged;


            AddEventHandlers();
            //commandType_Checkbox.CheckedChanged += ToggleOptions;
            //commandType_Checkbox.CheckStateChanged += ToggleFolder;
            //ToggleOptions(null, null);
            //ToggleFolder(null, null);

            //fileLocation_TextBox.TextChanged += DisableClearNVRAMOptions;

        }
        //Code commented as we may use the fimclient to update FW in the future.
        //private void ToggleFolder(object sender, EventArgs e)
        //{
        //    if (commandType_Checkbox.CheckState == CheckState.Checked)
        //    {
        //        fileLocation_TextBox.Enabled = true;
        //        selectFile_Button.Enabled = false;
        //    }
        //    else if (commandType_Checkbox.CheckState == CheckState.Indeterminate)
        //    {
        //        fileLocation_TextBox.Text = string.Empty;
        //        fileLocation_TextBox.Enabled = false;
        //        selectFile_Button.Enabled = true;
        //    }
        //    else if (commandType_Checkbox.CheckState == CheckState.Unchecked)
        //    {
        //        fileLocation_TextBox.Text = string.Empty;
        //        fileLocation_TextBox.Enabled = false;
        //        selectFile_Button.Enabled = false;
        //    }
        //}

        //private void ToggleOptions(object sender, EventArgs e)
        //{
        //    if (commandType_Checkbox.Checked)
        //    {
        //        selectFile_Button.Enabled = true;

        //        RemoveEventHandlers();
        //        partialClean_ComboBox.SelectedIndex = -1;
        //        executionMode_ComboBox.SelectedIndex = -1;
        //        jdimfgReset_ComboBox.SelectedIndex = -1;
        //        rebootCrashMode_ComboBox.SelectedIndex = -1;
        //        saveRecoverFlags_ComboBox.SelectedIndex = -1;
        //        language_ComboBox.SelectedIndex = -1;
        //        suppressUsed_ComboBox.SelectedIndex = -1;
        //        crDefSleep_ComboBox.SelectedIndex = -1;
                

        //        fileLocation_TextBox.Text = string.Empty;
        //        modelNumber_CheckBox.Checked = false;
        //        serialNumber_CheckBox.Checked = false;


        //        partialClean_ComboBox.Enabled = false;
        //        executionMode_ComboBox.Enabled = false;
        //        jdimfgReset_ComboBox.Enabled = false;
        //        rebootCrashMode_ComboBox.Enabled = false;
        //        saveRecoverFlags_ComboBox.Enabled = false;
        //        language_ComboBox.Enabled = false;
        //        suppressUsed_ComboBox.Enabled = false;
        //        crDefSleep_ComboBox.Enabled = false;

        //        modelNumber_CheckBox.Enabled = false;
        //        serialNumber_CheckBox.Enabled = false;


        //        AddEventHandlers();
        //    }
        //    else if(!commandType_Checkbox.Checked)
        //    {
        //        RemoveEventHandlers();
        //        fileLocation_TextBox.Text = string.Empty;
        //        selectFile_Button.Enabled = false;

        //        partialClean_ComboBox.Enabled = true;
        //        executionMode_ComboBox.Enabled = true;
        //        jdimfgReset_ComboBox.Enabled = true;
        //        rebootCrashMode_ComboBox.Enabled = true;
        //        saveRecoverFlags_ComboBox.Enabled = true;
        //        language_ComboBox.Enabled = true;
        //        suppressUsed_ComboBox.Enabled = true;
        //        crDefSleep_ComboBox.Enabled = true;

        //        modelNumber_CheckBox.Enabled = true;
        //        serialNumber_CheckBox.Enabled = true;

        //        AddEventHandlers();
        //    }
        //}

        public event EventHandler ConfigurationChanged;



        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            //_data.EnableFW = commandType_Checkbox.Checked;
            //_data.FimBundle = fileLocation_TextBox.Text;
            _data.JDIMfgReset = jdimfgReset_ComboBox.SelectedIndex ==-1 ? "" :  jdimfgReset_ComboBox.SelectedValue.ToString();
            _data.SerialNumberBool = serialNumber_CheckBox.Checked.ToString();
            _data.ModelNumberBool = modelNumber_CheckBox.Checked.ToString();
            _data.ExecutionMode = executionMode_ComboBox.SelectedIndex == -1 ? "" : executionMode_ComboBox.SelectedValue.ToString();
            _data.SaveRecoverFlags = saveRecoverFlags_ComboBox.SelectedIndex == -1 ? "" : saveRecoverFlags_ComboBox.SelectedValue.ToString();
            _data.RebootCrashMode = rebootCrashMode_ComboBox.SelectedIndex == -1 ? "" : rebootCrashMode_ComboBox.SelectedValue.ToString();
            _data.CRDefSleep = crDefSleep_ComboBox.SelectedIndex == -1 ? "" : crDefSleep_ComboBox.SelectedValue.ToString();
            _data.Language = language_ComboBox.SelectedIndex == -1 ? "" : language_ComboBox.SelectedValue.ToString();
            _data.SuppressUsed = suppressUsed_ComboBox.SelectedIndex == -1 ? "" : suppressUsed_ComboBox.SelectedValue.ToString();
            _data.PartialClean = partialClean_ComboBox.SelectedIndex == -1 ? "" : partialClean_ComboBox.SelectedValue.ToString();

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
            _data = new LowLevelConfigActivityData();

            assetSelectionControl.Initialize(AssetAttributes.None);

            partialClean_ComboBox.DataSource = _NVRAMOptions.PartialCleanOptions;
            executionMode_ComboBox.DataSource = _NVRAMOptions.ExecutionModeOptions;
            jdimfgReset_ComboBox.DataSource = _NVRAMOptions.JDIOptions;
            rebootCrashMode_ComboBox.DataSource = _NVRAMOptions.RebootCrashModeOptions;
            saveRecoverFlags_ComboBox.DataSource = _NVRAMOptions.SaveRecoverFlagsOptions;
            language_ComboBox.DataSource = _NVRAMOptions.LanguageOptions;
            suppressUsed_ComboBox.DataSource = _NVRAMOptions.SuppressUsedOptions;
            crDefSleep_ComboBox.DataSource = _NVRAMOptions.CRDefSleepOptions;

        }

        public void AddEventHandlers()
        {
            //fileLocation_TextBox.TextChanged += OnConfigurationChanged;
            partialClean_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            executionMode_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            jdimfgReset_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            rebootCrashMode_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            saveRecoverFlags_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            language_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            suppressUsed_ComboBox.SelectedValueChanged += OnConfigurationChanged;
            crDefSleep_ComboBox.SelectedValueChanged += OnConfigurationChanged;
        }

        public void RemoveEventHandlers()
        {
            //fileLocation_TextBox.TextChanged -= OnConfigurationChanged;
            partialClean_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            executionMode_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            jdimfgReset_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            rebootCrashMode_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            saveRecoverFlags_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            language_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            suppressUsed_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
            crDefSleep_ComboBox.SelectedValueChanged -= OnConfigurationChanged;
        }




        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            Initialize(environment);
            _data = configuration.GetMetadata<LowLevelConfigActivityData>();

            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);

            //commandType_Checkbox.Checked = _data.EnableFW;


            partialClean_ComboBox.SelectedIndex = _NVRAMOptions.PartialCleanOptions.FindIndex(x => x == _data.PartialClean);
            executionMode_ComboBox.SelectedIndex = _NVRAMOptions.ExecutionModeOptions.FindIndex(x => x == _data.ExecutionMode);
            jdimfgReset_ComboBox.SelectedIndex = _NVRAMOptions.JDIOptions.FindIndex(x => x == _data.JDIMfgReset);
            rebootCrashMode_ComboBox.SelectedIndex = _NVRAMOptions.RebootCrashModeOptions.FindIndex(x => x == _data.RebootCrashMode);
            saveRecoverFlags_ComboBox.SelectedIndex = _NVRAMOptions.SaveRecoverFlagsOptions.FindIndex(x => x == _data.SaveRecoverFlags);
            language_ComboBox.SelectedIndex = _NVRAMOptions.LanguageOptions.FindIndex(x => x == _data.Language);
            suppressUsed_ComboBox.SelectedIndex = _NVRAMOptions.SuppressUsedOptions.FindIndex(x => x == _data.SuppressUsed);
            crDefSleep_ComboBox.SelectedIndex = _NVRAMOptions.CRDefSleepOptions.FindIndex(x => x == _data.CRDefSleep);

            //fileLocation_TextBox.Text = _data.FimBundle;
            modelNumber_CheckBox.Checked = "True" == _data.ModelNumberBool;
            serialNumber_CheckBox.Checked = "True" == _data.SerialNumberBool;
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        /// <summary>
        /// Event handler to be called whenever the activity's configuration data changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        //We convert to UNC path
        //private void selectFile_Button_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog dia = new OpenFileDialog();
        //    dia.Filter = "Firmware|*.bdl";
        //    dia.Title = "Select a Firmware Bundle";
        //    dia.Multiselect = false;

        //    if (dia.ShowDialog() == DialogResult.OK)
        //    {
        //        Uri ur = new Uri(dia.FileName);
        //        var temp = System.Net.Dns.GetHostEntry(ur.Authority.ToString());
        //        string file = ur.ToString().Replace($@"file://{ur.Host}", $@"\\{temp.HostName}").Replace('/', '\\');
        //        fileLocation_TextBox.Text = file;

        //    }
        //    else if (dia.ShowDialog() == DialogResult.Cancel)
        //    {
        //        fileLocation_TextBox.Text = string.Empty;
        //    }
        //}


        //private void selectFile_Button_Click(object sender, EventArgs e)
        //{
        //    if (commandType_Checkbox.CheckState == CheckState.Checked)
        //    {
        //        FolderBrowserDialog dia = new FolderBrowserDialog();
        //        dia.Container.Add()

        //        DialogResult result = dia.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            Uri ur = new Uri(dia.SelectedPath);
        //            var temp = System.Net.Dns.GetHostEntry(ur.Authority.ToString());
        //            string path = ur.ToString().Replace($@"file://{ur.Host}", $@"\\{temp.HostName}").Replace('/', '\\');

        //            fileLocation_TextBox.Text = path;
        //        }
        //        else if (result == DialogResult.Cancel)
        //        {
        //            fileLocation_TextBox.Text = string.Empty;
        //        }

        //    }
        //    else if(commandType_Checkbox.CheckState == CheckState.Indeterminate)
        //    {
        //        OpenFileDialog dia = new OpenFileDialog();
        //        dia.Filter = "Firmware|*.bdl";
        //        dia.Title = "Select a Firmware Bundle";
        //        dia.Multiselect = false;

        //        DialogResult result = dia.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            Uri ur = new Uri(dia.FileName);
        //            var temp = System.Net.Dns.GetHostEntry(ur.Authority.ToString());
        //            string file = ur.ToString().Replace($@"file://{ur.Host}", $@"\\{temp.HostName}").Replace('/', '\\');
        //            fileLocation_TextBox.Text = file;

        //        }
        //        else if (result == DialogResult.Cancel)
        //        {
        //            fileLocation_TextBox.Text = string.Empty;
        //        }


        //    }
        //}
    }
}
