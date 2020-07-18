using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportWelcomeControl : UserControl
    {
        private string _directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public string ImportFile
        {
            get { return importFileTextBox.Text; }
            set { importFileTextBox.Text = value; }
        }

        public bool IsFileReloaded { get; set; }

        public ImportWelcomeControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void fileOpenButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportOpenFileDialog(_directory, "Open Scenario Export File", ImportExportType.Scenario))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    importFileTextBox.Text = dialog.Base.FileName;
                    _directory = Path.GetDirectoryName(ImportFile);
                    IsFileReloaded = true;
                }
            }
        }

        private void OpenFileControl_Load(object sender, EventArgs e)
        {
            welcomeRichTextBox.Rtf = Resource.ImportWelcome;
        }
    }
}
