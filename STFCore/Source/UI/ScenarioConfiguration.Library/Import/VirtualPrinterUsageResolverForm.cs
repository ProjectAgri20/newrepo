using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    [ObjectFactory(ResourceUsageType.VirtualPrinter)]
    public partial class VirtualPrinterUsageResolverForm : PrinterUsageResolverForm
    {
        protected VirtualPrinterUsageResolverForm()
        { }

        public VirtualPrinterUsageResolverForm(UsageResolverData data)
            : base(data)
        {
        }

        protected override void DisplayValues()
        {
            var printer = Data.Agent.ExportData as VirtualPrinterUsage;
            oldNameValueLabel.Text = printer.Name;
            oldModelValueLabel.Text = printer.HostName;
        }
    }
}
