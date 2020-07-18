using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Form for adding and editing SoftwareInstallers configuration.
    /// </summary>
    public partial class SoftwareInstallerForm : Form
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private SoftwareInstaller _installer = null;

        public SoftwareInstallerForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.SetIconAlignment(description_TextBox, ErrorIconAlignment.MiddleLeft);
            _errorProvider.SetIconAlignment(filePath_TextBox, ErrorIconAlignment.MiddleLeft);
            _errorProvider.SetIconAlignment(reboot_ComboBox, ErrorIconAlignment.MiddleLeft);
        }

        public SoftwareInstallerForm(SoftwareInstaller installer)
            : this()
        {
            _installer = installer;
        }

        /// <summary>
        /// The Software Installer
        /// </summary>
        public SoftwareInstaller SoftwareInstaller
        {
            get 
            {
                return _installer; 
            }
        }

        private void SoftwareInstallerForm_Load(object sender, EventArgs e)
        {
            if (_installer == null)
            {
                _installer = new SoftwareInstaller();
                _installer.InstallerId = SequentialGuid.NewGuid();
                _installer.RebootSetting = RebootMode.NoReboot.ToString();
            }
            
            reboot_ComboBox.DataSource = EnumUtil.GetDescriptions<RebootMode>().ToArray();

            // Data Bindings
            description_TextBox.DataBindings.Add("Text", _installer, "Description");
            filePath_TextBox.DataBindings.Add("Text", _installer, "FilePath");
            arguments_TextBox.DataBindings.Add("Text", _installer, "Arguments");
            RebootMode rebootMode = EnumUtil.Parse<RebootMode>(_installer.RebootSetting);
            reboot_ComboBox.SelectedItem = EnumUtil.GetDescription(rebootMode);
            copyDirectory_CheckBox.DataBindings.Add("Checked", _installer, "CopyDirectory");
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                // Set the selected Reboot Mode
                _installer.RebootSetting = EnumUtil.GetByDescription<RebootMode>(reboot_ComboBox.Text).ToString();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void description_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(description_TextBox.Text, description_Label.Text, description_TextBox, e);
        }

        private void filePath_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(filePath_TextBox.Text, filePath_Label.Text, filePath_TextBox, e);
        }

        private void reboot_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(reboot_ComboBox.Text, reboot_Label.Text, reboot_ComboBox, e);
        }

        private void HasValue(string data, string fieldName, Control control, CancelEventArgs e)
        {
            string errorMessage = null;

            if (string.IsNullOrEmpty(data.Trim()))
            {
                errorMessage = fieldName + " must have a value.";
            }

            _errorProvider.SetError(control, errorMessage);
            e.Cancel = (errorMessage != null);
        }
    }
}
