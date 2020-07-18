using HP.ScalableTest.PluginSupport.MobilePrintApp;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{

    public class PrinterOnJobOptions : MobilePrintJobOptions
    {

        public PrinterOnJobOptions() : base()
        {
            Copies = -1;
            Page = null;
            Orientation = null;
            Duplex = null;
            Color = null;
            PaperSize = null;
        }

        public int Copies { get; set; }
        public string Page { get; set; }
        public Option_Orientation? Orientation { get; set; }
        public Option_Duplex? Duplex { get; set; }
        public Option_Color? Color { get; set; }
        public Option_PaperSize? PaperSize { get; set; }
        
    }
}
