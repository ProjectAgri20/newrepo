/*
* © Copyright 2016 HP Development Company, L.P.
*/
using System;
using System.Windows.Forms;
using HP.ScalableTest.Development;
using HP.ScalableTest.Development.UI;
using HP.ScalableTest.Framework.Assets;
using Plugin.SdkGeneralExample;
using Plugin.SdkPullPrintExample;

namespace STFScratch
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var simulator = new PluginFrameworkSimulator();

            // Uncomment the following lines to run the plugin general example
            simulator.ConfigurationControlType = typeof(SdkGeneralExampleConfigControl);
            simulator.ExecutionEngineType = typeof(SdkGeneralExampleExecutionControl);

            // Uncomment the following lines to run the pull print plugin example
            //simulator.ConfigurationControlType = typeof(SdkPullPrintExampleConfigurationControl);
            //simulator.ExecutionEngineType = typeof(SdkPullPrintExampleExecutionControl);

            // Add mock credentials
            simulator.SessionId = "TEST";
            simulator.UserName = "u00001";
            simulator.UserPassword = "someUserPassword";
            simulator.UserDomain = null;
            simulator.UserDnsDomain = null;

            // Add mock plugin settings
            simulator.PluginSettings["sampleKey"] = "sampleValue";
            
            // Add sample printer to asset inventory mock
            // change ip address, admin password, etc. as necessary
            var printer = new PrintDeviceInfo("samplePrinter", (AssetAttributes.ControlPanel | AssetAttributes.Printer), "MFP", "192.168.1.1", "someAdminPassword", "productName", 9100, true);
            simulator.AssetInventory.AddAsset(printer);

            // Add sample documents to test document library mock
            DocumentBuilder documentBuilder = new DocumentBuilder();
            documentBuilder.Group = "All Documents";
            simulator.DocumentLibrary.AddDocuments(documentBuilder.BuildDocuments("foo.doc", 10));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginFrameworkSimulatorForm(simulator) { Size = new System.Drawing.Size(1024, 800) });
        }
    }
}
