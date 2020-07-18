using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    [ObjectFactory(ResourceUsageType.PrintDriver)]
    public partial class PrintDriverUsageResolverForm : UsageResolverForm
    {
        public PrintDriverUsageResolverForm(UsageResolverData data)
            : base(data)
        {
            InitializeComponent();
        }

        private void PrintDriverUsageResolverForm_Load(object sender, EventArgs e)
        {
            var usage = Data.Agent.ExportData as PrintDriverUsage;

            exportNameValue.Text = usage.Name;
            exportVersionValue.Text = usage.Version;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var package = printDriverSelectionControl.SelectedPackage;
            if (package == null)
            {
                MessageBox.Show("You must select a Driver Package", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var driver = printDriverSelectionControl.SelectedDriver;
            if (driver == null)
            {
                MessageBox.Show("You must select a Driver Model", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var usage = Data.Agent.ImportData as PrintDriverUsage;
            usage.Version = package.Version;
            usage.Name = driver.Name;
            usage.ResourceId = driver.PrintDriverId.ToString();
            Data.Replacement = usage.Version;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
