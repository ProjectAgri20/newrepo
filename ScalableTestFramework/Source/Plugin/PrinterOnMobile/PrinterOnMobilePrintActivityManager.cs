using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.MobilePrintApp;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using System.Net;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    public class PrinterOnMobilePrintActivityManager : MobilePrintActivityManager
    {
        private PrinterOnMobileActivityData _data;

        /// <summary>
        ///  Initialize PrinterOnMobilePrintActivityManager
        /// </summary>
        /// <param name="executionData"></param>
        public PrinterOnMobilePrintActivityManager(PluginExecutionData executionData)  : base (executionData)
        {
            _data = executionData.GetMetadata<PrinterOnMobileActivityData>();
            Option = _data.Options;
            Option.SetOption("name", _data.Name);
            Option.SetOption("email", _data.Email);
            Target = new ObjectToPrint
            {
                Type = DocumentType.File
            };
            Target.Infomation.Add("FilePath", _data.FilePath);
            PrinterId = _data.PrinterId;
        }
        
        public override void SetupJob()
        {
            UpdateStatus("Setup Device");
            
            
            // Currently only support Android
            IPAddress mobileDeviceIP;
            if(!IPAddress.TryParse(DeviceIdentifier, out mobileDeviceIP))
            {
                UpdateStatus("Try to connect USB mode. Make sure mobile device is connected with STB Client PC with USB");
            }
            App = new PrinterOnAndroid(DeviceIdentifier);
            JetAdvantageLinkLogAdapter.Attach();
            UpdateStatus($"Mobile Print App Controller {App.GetAppName()} is created.");
            
        }
    }
}