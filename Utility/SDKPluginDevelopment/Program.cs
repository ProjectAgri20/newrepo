using System;
using System.Windows.Forms;
using HP.ScalableTest.Development.UI;
using HP.ScalableTest.Plugin.SafeComPullPrinting;
using HP.ScalableTest.Plugin.HpacPullPrinting;
using HP.ScalableTest.Plugin.ScanToEmail;
using HP.ScalableTest.Plugin.ScanToFolder;
using HP.ScalableTest.Plugin.Hpec;
using HP.ScalableTest.Plugin.EquitracPullPrinting;
using HP.ScalableTest.Plugin.mPrint;
using HP.ScalableTest.Plugin.Authentication;
using HP.ScalableTest.Plugin.ScanToHpcr;
using HP.ScalableTest.Plugin.ScanToWorkflow;
using HP.ScalableTest.Plugin.DirtyDevice;

namespace SDKPluginDevelopment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate the plugin simulator with the appropriate types
            InternalFrameworkSimulator simulator = new InternalFrameworkSimulator("STFData03.etl.boi.rd.hpicorp.net");

            //simulator.ConfigurationControlType = typeof(SafeComPullPrintingConfigurationControl);
            //simulator.ExecutionEngineType = typeof(SafeComPullPrintingExecutionControl);

            //simulator.ConfigurationControlType = typeof(HpcrSimulationConfigControl);
            //simulator.ExecutionEngineType = typeof(HpcrSimulationExecController);

            //simulator.ConfigurationControlType = typeof(ScanToHpcrConfigControl);
            //simulator.ExecutionEngineType = typeof(ScanToHpcrExecControl);

            //simulator.ConfigurationControlType = typeof(JetAdvantageConfigControl);
            //simulator.ExecutionEngineType = typeof(JetAdvantageExecControl);

            //simulator.ConfigurationControlType = typeof(JetAdvantageUploadConfigControl);
            //simulator.ExecutionEngineType = typeof(JetAdvantageUploadExecControl);

            //simulator.ConfigurationControlType = typeof(ScanToEmailConfigControl);
            //simulator.ExecutionEngineType = typeof(ScanToEmailExecControl);

            //simulator.ConfigurationControlType = typeof(ScanToFolderConfigControl);
            //simulator.ExecutionEngineType = typeof(ScanToFolderExecControl);

            //simulator.ConfigurationControlType = typeof(ScanToWorkflowConfigControl);
            //simulator.ExecutionEngineType = typeof(ScanToWorkflowExecControl);

            //simulator.ConfigurationControlType = typeof(HpacPullPrintingConfigurationControl);
            //simulator.ExecutionEngineType = typeof(HpacPullPrintingExecutionControl);

            //simulator.ConfigurationControlType = typeof(HpecConfigControl);
            //simulator.ExecutionEngineType = typeof(HpecExecControl);

            //simulator.ConfigurationControlType = typeof(SandboxConfigControl);
            //simulator.ExecutionEngineType = typeof(SandboxExecControl);

            //simulator.ConfigurationControlType = typeof(EquitracPullPrintingConfigurationControl);
            //simulator.ExecutionEngineType = typeof(EquitracPullPrintingExecutionControl);

            //simulator.ConfigurationControlType = typeof(mPrintConfigurationControl);
            //simulator.ExecutionEngineType = typeof(mPrintExecutionControl);

            simulator.ConfigurationControlType = typeof(AuthenticationConfigControl);
            simulator.ExecutionEngineType = typeof(AuthenticationExecutionControl);

            //simulator.ConfigurationControlType = typeof(DirtyDeviceConfigurationControl);
            //simulator.ExecutionEngineType = typeof(DirtyDeviceExecutionControl);


            simulator.SessionId = "TEST";
            simulator.UserName = "u00105";
            simulator.UserPassword = "CatchTwenty2";
            simulator.UserDomain = "SMA";
            simulator.UserDnsDomain = "sma.boi.rd.hpicorp.net";
            simulator.PluginSettings["samplekey"] = "samplevalue";
            //simulator.PluginSettings["HpcrProxy"] = "";

            // The mock does not download files from a remote server.
            // Any files you will be using must be downloaded to your local machine in the path specified below.
            simulator.FileRepository.LocalFilePath = @"C:\Temp\TestDocuments";

            // If your plugin uses the SessionRuntime service to get email destination(s), add them here
            simulator.SessionRuntime.OfficeWorkerEmailAddresses.Add("u00001@etl.boi.rd.hpicorp.net");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginFrameworkSimulatorForm(simulator));
        }
    }
}
