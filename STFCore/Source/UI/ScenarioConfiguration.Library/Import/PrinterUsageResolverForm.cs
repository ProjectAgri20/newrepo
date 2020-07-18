using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Core.ImportExport;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    [ObjectFactory(ResourceUsageType.Printer)]
    public partial class PrinterUsageResolverForm : UsageResolverForm
    {
        protected PrinterUsageResolverForm()
        { }

        public PrinterUsageResolverForm(UsageResolverData data)
            : base(data)
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void PrinterUsageResolverForm_Load(object sender, EventArgs e)
        {
            DisplayValues();
        }

        protected virtual void DisplayValues()
        {
            var printer = Data.Agent.ExportData as PrinterUsage;
            oldNameValueLabel.Text = printer.Name;
            oldModelValueLabel.Text = printer.Model;
        }

        private void selectDeviceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (AssetSelectionForm printerSelectionForm = new AssetSelectionForm(DeviceCapabilities.Print))
            {
                printerSelectionForm.MultiSelect = false;

                printerSelectionForm.ShowDialog(this);
                if (printerSelectionForm.DialogResult == DialogResult.OK)
                {
                    var asset = printerSelectionForm.SelectedAssets.First();

                    Data.Replacement = asset.AssetId;
                    Data.Agent.ImportData.ResourceId = asset.AssetId;                    

                    newNameValueLabel.Text = asset.AssetId;
                    newModelValueLabel.Text = asset.Description;
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Data.Replacement = string.Empty;
            Data.Agent.ImportData.ResourceId = string.Empty;                    

            Close();
        }
    }
}
