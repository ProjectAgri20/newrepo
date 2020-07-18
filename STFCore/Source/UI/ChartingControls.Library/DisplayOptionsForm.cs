using System;
using System.Windows.Forms;
using HP.ScalableTest.UI.Charting.Properties;

namespace HP.ScalableTest.UI.Charting
{
    internal partial class DisplayOptionsForm : Form
    {
        public DisplayOptionsForm()
        {
            InitializeComponent();
        }

        private void DisplayOptionsForm_Load(object sender, EventArgs e)
        {
            displayCompleted_CheckBox.Checked = Settings.Default.DisplayCompleted;
            displayFailed_CheckBox.Checked = Settings.Default.DisplayFailed;
            displaySkipped_CheckBox.Checked = Settings.Default.DisplaySkipped;
            displayError_CheckBox.Checked = Settings.Default.DisplayError;
            displayOther_CheckBox.Checked = Settings.Default.DisplayOthers;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Settings.Default.DisplayCompleted = displayCompleted_CheckBox.Checked;
            Settings.Default.DisplayFailed = displayFailed_CheckBox.Checked;
            Settings.Default.DisplaySkipped = displaySkipped_CheckBox.Checked;
            Settings.Default.DisplayError = displayError_CheckBox.Checked;
            Settings.Default.DisplayOthers = displayOther_CheckBox.Checked;
            Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }
    }
}
