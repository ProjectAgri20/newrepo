using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.Executor
{
    public partial class ExecutorManagementForm : Form
    {
        private readonly Executable _executable;
        public ExecutorManagementForm()
        {
            InitializeComponent();
        }

        public ExecutorManagementForm(Executable executable)
        {
            InitializeComponent();
            _executable = executable;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.exe";
                dialog.Filter = @"All Executables |*.exe;*.bat;*.msi;*.cmd";
                dialog.Multiselect = false;
                dialog.Title = @"Select the Executable";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    executableFileName_textBox.Text = dialog.FileName;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(executableFileName_textBox.Text))
            {
                return;
            }

            _executable.FilePath = executableFileName_textBox.Text;
            _executable.Arguments = arguments_textBox.Text;
            _executable.CopyDirectory = copydirectory_checkBox.Checked;
            _executable.PassSessionId = sessionid_checkBox.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ExecutorManagementForm_Load(object sender, EventArgs e)
        {
            executableFileName_textBox.Text = _executable.FilePath;
            arguments_textBox.Text = _executable.Arguments;
            copydirectory_checkBox.Checked = _executable.CopyDirectory;
            sessionid_checkBox.Checked = _executable.PassSessionId;

        }
    }
}
