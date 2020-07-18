using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.TopCat;

namespace HP.ScalableTest.Plugin.TopCat
{
    [ToolboxItem(false)]
    public partial class TopCatConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private TopCatActivityData _activityData;
        private TopCatScript _selectedScript;
        private SettingsDictionary _settings;
        private Dictionary<string, string> _properties;

        public TopCatConfigurationControl()
        {
            InitializeComponent();
            _properties = new Dictionary<string, string>();

            topcat_fieldValidator.RequireCustom(scripts_listBox, ValidateScriptSelection, "Please select a TopCat script");
            topcat_fieldValidator.RequireCustom(topcattests_listBox, ValidateTestSelection, "Please select a test to execute");

            scripts_listBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #region IPluginConfigurationControl

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new TopCatActivityData();
            _settings = environment.PluginSettings;
            if (ValidateSettings())
            {
                InitializeTopCatUI();
            }
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of configuration information.
            _activityData = configuration.GetMetadata<TopCatActivityData>(new[] { new TopCatActivityDataConverter() });
            _settings = environment.PluginSettings;
            if (ValidateSettings())
            {
                InitializeTopCatUI();
            }
        }

        public PluginConfigurationData GetConfiguration()
        {
            if (_selectedScript != null)
            {
                _activityData.Script = _selectedScript;
                if (alltests_checkBox.Checked || topcattests_listBox.SelectedIndices.Count == 0)
                {
                    _activityData.Script.SelectedTests.Clear();
                }
                else
                {
                    _activityData.Script.SelectedTests.Clear();
                    foreach (var selectedTest in topcattests_listBox.SelectedItems)
                    {
                        _activityData.Script.SelectedTests.Add(selectedTest.ToString());
                    }
                }
                _activityData.Script.Properties.Clear();
                foreach (var property in _properties.Keys)
                {
                    _activityData.Script.Properties[property] = _properties[property];
                }
                _activityData.SetupFileName = setupPath_textBox.Text;
                _activityData.CopyDirectory = copydir_checkBox.Checked;
                _activityData.RunOnce = runonce_checkBox.Checked;
            }

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(topcat_fieldValidator.ValidateAll());
        }

        #endregion IPluginConfigurationControl

        public bool ValidateSettings()
        {
            if (_settings != null)
            {
                if (string.IsNullOrEmpty(_settings["TopCatSetup"]))
                {
                    MessageBox.Show("TopCat Setup not defined. Please contact your system administrator. UI will not be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!Directory.Exists(_settings["TopCatSetup"]))
                {
                    MessageBox.Show("TopCat Setup files not found at the defined location. Please check. UI will not be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (Directory.GetFiles(_settings["TopCatSetup"], "TopCat.exe", SearchOption.AllDirectories).Count() < 2)
                {
                    MessageBox.Show("TopCat executable not found at the defined location. Please check. UI will not be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        #region UIOperations

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.tcx";
                dialog.Filter = "TopCat Script (*.tcx) | *.tcx";
                dialog.Multiselect = false;
                dialog.Title = "Add TopCat Script File";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    scriptpath_textBox.Text = dialog.FileName;

                    buttonUpload.Enabled = true;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            UploadScriptFiles(scriptpath_textBox.Text);
            InitializeTopCatUI();
            scriptpath_textBox.Text = "";
            buttonUpload.Enabled = false;
        }

        private void UploadScriptFiles(string scriptFileName)
        {
            var repo = _settings["TopCatScripts"];

            //now create a directory having an unique ID and store the webtest file and the dataconnection file
            Guid folderId = Guid.NewGuid();
            var destinationDir = Directory.CreateDirectory(Path.Combine(repo, folderId.ToString()));

            File.Copy(scriptFileName, Path.Combine(destinationDir.FullName, Path.GetFileName(scriptFileName)), true);

            UploadDataSources(scriptFileName, destinationDir);
        }

        private static void UploadDataSources(string scriptFileName, DirectoryInfo destinationDir)
        {
            Collection<string> importFiles = new Collection<string>();
            var xml = XDocument.Load(scriptFileName);
            XNamespace ns = xml.Root.Name.Namespace;
            var imports = xml.Descendants(ns + "Imports").First();
            foreach (XElement xelement in imports.Descendants(ns + "ConfigurationFile"))
            {
                string importfile = xelement.Element(ns + "Path").Value;
                if (File.Exists(importfile))
                {
                    importFiles.Add(importfile);
                }
                else
                {
                    importFiles.Add(Path.Combine(Path.GetDirectoryName(scriptFileName), Path.GetFileName(importfile)));
                }
            }

            //now that we have the file, upload it to your repository
            foreach (var importFile in importFiles)
            {
                File.Copy(importFile, Path.Combine(destinationDir.FullName, Path.GetFileName(importFile)), true);
            }
        }

        private void InitializeTopCatUI()
        {
            int index = 0;
            int selectedIndex = 0;

            scripts_listBox.Items.Clear();
            var repo = _settings["TopCatScripts"];
            foreach (var file in Directory.GetFiles(repo, "*.tcx", SearchOption.AllDirectories))
            {
                scripts_listBox.Items.Add(new TopCatScript(file, Path.GetFileName(file)));
                if (_activityData.Script != null)
                {
                    if (!string.IsNullOrEmpty(_activityData.Script.FileName))
                    {
                        if (_activityData.Script.FileName.Equals(file))
                        {
                            selectedIndex = index;
                        }
                    }
                }
                index++;
            }
            scripts_listBox.DisplayMember = "ScriptName";
            scripts_listBox.ValueMember = "FileName";

            if (_activityData.Script != null)
            {
                _selectedScript = _activityData.Script;
                _properties = _activityData.Script.Properties;
                setupPath_textBox.Text = _activityData.SetupFileName;
                runonce_checkBox.Checked = _activityData.RunOnce;
                copydir_checkBox.Checked = _activityData.CopyDirectory;
                if (!string.IsNullOrEmpty(_activityData.Script.ScriptName))
                {
                    scripts_listBox.SelectedIndex = selectedIndex;
                }

                if (_activityData.Script.SelectedTests.Count == 0)
                {
                    alltests_checkBox.Checked = true;
                }
                else
                {
                    alltests_checkBox.Checked = false;
                    foreach (var selectedTest in _activityData.Script.SelectedTests)
                    {
                        if (topcattests_listBox.Items.IndexOf(selectedTest) > -1)
                        {
                            topcattests_listBox.SelectedIndices.Add(topcattests_listBox.Items.IndexOf(selectedTest));
                        }
                    }
                }
            }
        }

        private void LoadProperties()
        {
            if (_selectedScript != null)
            {
                properties_comboBox.DataSource = null;
                if (!_selectedScript.FileName.Equals(_activityData.Script?.FileName))
                {
                    _properties = new Dictionary<string, string>();
                    XDocument xDoc = XDocument.Load(_selectedScript.FileName);
                    XNamespace ns = xDoc.Root.Name.Namespace;

                    var properties = xDoc.Descendants(ns + "Properties").First();

                    foreach (XElement xProperty in properties.Descendants(ns + "Property"))
                    {
                        _properties.Add(xProperty.FirstAttribute.Value, xProperty.Element(ns + "Value").Value);
                    }
                }
                else
                {
                    _properties = _activityData.Script.Properties;
                    setupPath_textBox.Text = _activityData.SetupFileName;
                    copydir_checkBox.Checked = _activityData.CopyDirectory;
                    runonce_checkBox.Checked = _activityData.RunOnce;
                }
            }

            properties_comboBox.DisplayMember = "Key";
            properties_comboBox.ValueMember = "Value";
            properties_comboBox.DataSource = new BindingSource(_properties, null);
        }

        private void LoadTests()
        {
            topcattests_listBox.Items.Clear();
            if (_selectedScript != null)
            {
                XDocument xDoc = XDocument.Load(_selectedScript.FileName);
                XNamespace ns = xDoc.Root.Name.Namespace;

                var topcattest = xDoc.Descendants(ns + "Tests").First();

                foreach (XElement topcattestcase in topcattest.Descendants(ns + "TestCase"))
                {
                    topcattests_listBox.Items.Add(topcattestcase.FirstAttribute.Value);
                }
            }
        }

        private void properties_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (properties_comboBox.SelectedIndex < 0)
            {
                return;
            }

            properties_textBox.Text = properties_comboBox.SelectedValue.ToString();
        }

        private void scripts_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedScript = scripts_listBox.SelectedItem as TopCatScript;
            LoadProperties();
            LoadTests();
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            var selectedProperty = (KeyValuePair<string, string>)properties_comboBox.SelectedItem;
            _properties[selectedProperty.Key] = properties_textBox.Text;
        }

        private void browseInstaller_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.bat";
                dialog.Filter = "All Executables (*.exe)(*.bat)(*.msi)|*.exe;*.bat;*.msi";
                dialog.Multiselect = false;
                dialog.Title = "Select the software installer";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    setupPath_textBox.Text = dialog.FileName;
                }
            }
        }

        private void alltests_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (alltests_checkBox.Checked)
            {
                topcattests_listBox.Enabled = false;
                topcattests_listBox.SelectedIndex = -1;
            }
            else
            {
                topcattests_listBox.Enabled = true;
            }
        }

        #endregion UIOperations

        private bool ValidateScriptSelection()
        {
            if (scripts_listBox.SelectedIndex > -1)
            {
                return true;
            }

            return false;
        }

        private bool ValidateTestSelection()
        {
            if (alltests_checkBox.Checked)
            {
                return true;
            }
            if (topcattests_listBox.SelectedIndex > -1)
            {
                return true;
            }
            return false;
        }
    }
}