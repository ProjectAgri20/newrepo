using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DriverInstallUserControl : FieldValidatedControl
    {
        private QueueManager _manager = null;
        private PrintDriverComboBox driverModel_ComboBox = null;
        private bool _includeAllArchitectures = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverInstallUserControl"/> class.
        /// </summary>
        public DriverInstallUserControl(QueueManager manager)
        {
            InitializeComponent();
            InitializeInlineComponent();

            _manager = manager;
        }

        private void InitializeInlineComponent()
        {
            driverModel_ComboBox = new PrintDriverComboBox(new Point(134, 32));
            driverModel_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            driverModel_ComboBox.SelectedIndexChanged += new EventHandler(driverModel_ComboBox_SelectedIndexChanged);
            driverModel_ComboBox.Validating += new CancelEventHandler(driverModel_ComboBox_Validating);
            Controls.Add(driverModel_ComboBox);
        }

        private void driverModel_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(driverModel_ComboBox.Text, "Print Driver Model", model_Label, e);
        }

        private void driverModel_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = driverModel_ComboBox.SelectedData;
            ProcessorArchitecture arch = EnumUtil.Parse<ProcessorArchitecture>(data["Architecture"]);

            DriverVersion version = new DriverVersion(data["Version"]);

            int index = _manager.PrintDrivers.IndexOf(data["Name"], arch.ToDriverArchitecture(), version, data["InfFile"]);

            if (index != -1)
            {
                _manager.CurrentDriver = _manager.PrintDrivers[index];
            }
        }

        /// <summary>
        /// Gets the queue manager.
        /// </summary>
        public QueueManager QueueManager
        {
            get { return _manager; }
        }

        /// <summary>
        /// Validates the package loaded.
        /// </summary>
        /// <returns></returns>
        public bool ValidatePackageLoaded()
        {
            if (!_manager.PrintDriverSelected)
            {
                MessageBox.Show("First select a Print Driver above", "Select Print Driver", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                driverPackagePath_ComboBox.Focus();
                return false;
            }

            return true;
        }

        private void browse_Button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder that contains your print drivers";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = _manager.DriverPackagePaths.Add(dialog.SelectedPath);
                    UpdateDriverPathsComboBox();
                    driverPackagePath_ComboBox.SelectedItem = path;
                }
            }
        }

        private void UpdateDriverPathsComboBox()
        {
            driverPackagePath_ComboBox.Items.Clear();
            foreach (string item in _manager.DriverPackagePaths.Items)
            {
                driverPackagePath_ComboBox.Items.Add(item);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool LoadDriverPackage(string path)
        {
            bool loaded = false;
            try
            {
                _manager.LoadDrivers(path, _includeAllArchitectures);
                loaded = true;
                UpdateComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to find Driver: {0}".FormatWith(ex.Message), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return loaded;
        }

        private void UpdateComboBox()
        {
            driverModel_ComboBox.Items.Clear();
            driverModel_ComboBox.AddDrivers(_manager.PrintDrivers);
            if (driverModel_ComboBox.Items.Count > 0)
            {
                driverModel_ComboBox.SelectedIndex = 0;
            }
        }

        private void downloadDriver_Button_Click(object sender, EventArgs e)
        {
            using (PrintDriverLocalCopyForm form = new PrintDriverLocalCopyForm())
            {
                form.ShowDialog();

                if (form.DriverPaths.Count > 0)
                {
                    string firstItem = form.DriverPaths[0];

                    _manager.DriverPackagePaths.AddRange(form.DriverPaths);
                    UpdateDriverPathsComboBox();

                    driverPackagePath_ComboBox.SelectedItem = firstItem;
                }
            }
        }

        private void driverPackagePath_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDriverPackage(driverPackagePath_ComboBox.Text);
        }

        private void driverPackagePath_ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.Handled = LoadDriverPackage(driverPackagePath_ComboBox.Text);
            }
        }

        private void DriverInstallUserControl_Load(object sender, EventArgs e)
        {
            UpdateDriverPathsComboBox();

            // If there is a current package loaded into the manager, then
            // select the folder for that path and display it in the combobox
            string currentPackagePath = _manager.FindCurrentPackagePath();
            if (!string.IsNullOrEmpty(currentPackagePath))
            {
                driverPackagePath_ComboBox.SelectedItem = currentPackagePath;
            }

            includeAllArchitectures_CheckBox.Checked = _includeAllArchitectures;
        }

        private void SelectComboBoxItem(string name, DriverArchitecture architecture, DriverVersion version, string path)
        {
            int index = _manager.PrintDrivers.IndexOf(name, architecture, version, Path.GetFileName(path));

            TraceFactory.Logger.Debug("INDEX: {0}".FormatWith(index));

            if (index >= 0)
            {
                driverModel_ComboBox.SelectedIndex = index;
            }

            TraceFactory.Logger.Debug("Selected Index Updated");

            driverModel_ComboBox.SelectedText = string.Empty;
        }

        private void inboxDriver_Button_Click(object sender, EventArgs e)
        {
            using (InboxDriverSelectionForm form = new InboxDriverSelectionForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PrintDeviceDriver driver = form.PrintDriver;

                    if (driver != null)
                    {
                        string path = _manager.DriverPackagePaths.Add(Path.GetDirectoryName(driver.InfPath));
                        UpdateDriverPathsComboBox();

                        _manager.AddDriver(driver);

                        driverPackagePath_ComboBox.SelectedItem = path;

                        SelectComboBoxItem
                            (
                                driver.Name,
                                driver.Architecture,
                                driver.Version,
                                Path.GetFileName(driver.InfPath)
                            );
                    }
                }
            }
        }

        private void includeAllArchitectures_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _includeAllArchitectures = includeAllArchitectures_CheckBox.Checked;
        }
    }
}
