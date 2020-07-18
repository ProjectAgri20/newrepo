using System;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ExportOptionsForm : Form
    {
        private string _scenarioName = string.Empty;

        public ExportOptionsForm(string scenarioName)
        {
            InitializeComponent();

            _scenarioName = scenarioName;
        }

        private void ExportOptionsForm_Load(object sender, EventArgs e)
        {
            scenarioNameLabel.Text = _scenarioName;
        }

        public bool IncludePrinters
        {
            get { return includePrintersCheckbox.Checked; }
        }

        public bool IncludeDocuments
        {
            get { return includeDocumentsCheckbox.Checked; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
