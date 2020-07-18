using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HP.GFriend.Core;
using HP.GFriend.Core.Execution;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    [ToolboxItem(false)]
    public partial class GFriendExecutionConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private static bool _libraryWritten = false;
        private GFriendExecutionActivityData _data;
        private const AssetAttributes DeviceAttributes = AssetAttributes.ControlPanel;

        private BindingList<GFriendFile> _gFriendFiles;
        private List<string> _requiredDevices;
        
        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GFriendExecutionConfigurationControl" /> class.
        /// </summary>
        public GFriendExecutionConfigurationControl()
        {
            InitializeComponent();
            _data = new GFriendExecutionActivityData();
            _requiredDevices = new List<string>();
            InitializeConfigurationControl();
            
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new GFriendExecutionActivityData();
            _requiredDevices = new List<string>();
            InitializeConfigurationControl();
            assetSelectionControl.Initialize(DeviceAttributes);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<GFriendExecutionActivityData>();
            InitializeConfigurationControl();
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.LockTimeouts = lockTimeoutControl.Value;
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData
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
            ConfigurationChanged?.Invoke(this, e);
        }

        private void InitializeConfigurationControl()
        {
            // Datagridview
            _gFriendFiles = new BindingList<GFriendFile>(_data.GFriendFiles);
            scripts_DataGridView.DataSource = _gFriendFiles;
            scripts_DataGridView.Columns[0].Width = 200;
            scripts_DataGridView.Columns[1].Width = 100;
            scripts_DataGridView.Columns[2].Width = 460;

            // Field Validator
            fieldValidator.RequireCustom(assetSelectionControl, ValidateAsset);
            fieldValidator.RequireCustom(scripts_DataGridView, ValidateFiles);

            // Lock Timeout
            lockTimeoutControl.Initialize(_data.LockTimeouts);

            // GFriend Initialize
            if(!_libraryWritten)
            {
                string gfLibraryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GFriend", "libs");
                GFriendPreparationManager.PrepareLibrary(gfLibraryPath);
                LibraryUtils.LibraryPath = gfLibraryPath;
                _libraryWritten = true;
            }
            
        }

        private ValidationResult ValidateAsset()
        {
            List<string> selectedAsset = assetSelectionControl.AssetSelectionData.SelectedAssets.ToList();
            List<string> missingAsset = new List<string>();
            foreach(string required in _requiredDevices)
            {
                if(!selectedAsset.Contains(required))
                {
                    missingAsset.Add(required);
                }
            }

            if(missingAsset.Contains("__DEFUALT__DUT__") && selectedAsset.Count > 0)
            {
                missingAsset.Remove("__DEFUALT__DUT__");
            }
            else if(missingAsset.Contains("__DEFUALT__DUT__") && selectedAsset.Count == 0)
            {
                string message = "At least one asset need to be added";
                return new ValidationResult(false, message);
            }

            if (missingAsset.Count > 0)
            {
                string message = "Need to add asset(s) with following names : " + string.Join(", ", missingAsset);
                return new ValidationResult(false, message);
            }

            return new ValidationResult(true);
        }

        private ValidationResult ValidateFiles()
        {
            if(scripts_DataGridView.Rows.Count <= 0)
            {
                return new ValidationResult(false, "Select GFriend Test Script to execute");
            }
            return new ValidationResult(true);
        }

        private void Add_ToolStripButton_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "GF Script |*.txt;*.gfscript;";
                fileDialog.Multiselect = false;
                fileDialog.ShowDialog();

                filePath = fileDialog.FileName;
            }
                
            if(!string.IsNullOrEmpty(filePath))
            {
                AddScriptFile(filePath);
            }
        }

        private void AddScriptFile(string filePath)
        {
            _data.GFriendFiles.Clear();
            _gFriendFiles.Clear();
            TestDataManager testDataManager = null;
            try
            {
                testDataManager = Parser.ParseTestSuite(filePath);
                testDataManager.PostActionAfterParsingUsings();

                if(testDataManager.TargetTestSuite.TcCount == 0)
                {
                    MessageBox.Show("Testcase is not exist in the script. Please check script", "Add GFriend File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                // Script check logic will be added if GFriend returns error details.
                MessageBox.Show("There is some problem in the script. Please check script.", "Add GFriend File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            GFriendFile script = new GFriendFile(filePath, GFFileTypes.GFScript);
            AddToDataGridView(script);

            foreach (string usedFile in testDataManager.FilesToUse)
            {
                string targetFilePath = Path.Combine(Path.GetDirectoryName(filePath), usedFile);
                GFFileTypes fileType;
                if (usedFile.EndsWith("gfvar"))
                {
                    fileType = GFFileTypes.GFVariable;
                }
                else if (usedFile.EndsWith("gflib"))
                {
                    fileType = GFFileTypes.GFLibrary;
                }
                else
                {
                    fileType = GFFileTypes.Unknown;
                }
                AddToDataGridView(new GFriendFile(targetFilePath, fileType));

                _requiredDevices = testDataManager.DeviceLibraryMapping.Keys.Where(s => !s.Equals(TestDataManager.NO_DUT)).ToList();

            }
        }

        
        private void AddToDataGridView(GFriendFile file)
        {
            _gFriendFiles.Add(file);
        }

        
        private void Edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (scripts_DataGridView.CurrentRow == null)
            {
                MessageBox.Show("Please select file to edit", "Edit GFriend File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            EditScriptForm editForm = new EditScriptForm((GFriendFile)scripts_DataGridView.CurrentRow.DataBoundItem);
            editForm.FormClosed += EditForm_FormClosed;
            editForm.Show();
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            scripts_DataGridView.Refresh();
        }

        private void Scripts_DataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit_ToolStripButton_Click(this, null);
        }

        private void Remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            _data.GFriendFiles.Clear();
            _gFriendFiles.Clear();
        }
    }
}
