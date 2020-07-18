using System;
using System.Configuration;
using System.Windows.Forms;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Development.UI;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;

namespace PluginSimulator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Prompt User for Plugin to load
            SelectPluginForm selectForm = new SelectPluginForm();
            if (selectForm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //Create the Framework Simulator
            PluginFactory.PluginRelativePath = ConfigurationManager.AppSettings["PluginRelativeLocation"];
            PluginAssembly pluginAssembly = PluginFactory.GetPluginByAssemblyName(selectForm.PluginAssemblyName);
            InternalFrameworkSimulator simulator = new InternalFrameworkSimulator(ConfigurationManager.AppSettings["GlobalDatabase"], pluginAssembly);

            // Paperless mode
            simulator.PaperlessMode = JobMediaMode.Paperless;

            // Common environment values
            simulator.SessionId = ConfigurationManager.AppSettings["SessionId"];
            simulator.UserName = ConfigurationManager.AppSettings["UserName"];
            simulator.UserPassword = ConfigurationManager.AppSettings["UserPassword"];
            simulator.UserDomain = ConfigurationManager.AppSettings["UserDomain"];
            simulator.UserDnsDomain = ConfigurationManager.AppSettings["UserDnsDomain"];
            simulator.FileRepository.DocumentShare = ConfigurationManager.AppSettings["DocumentShareLocation"];

            // Less common values used by some plugins
            simulator.PluginSettings["sampleKey"] = "sampleValue";
            simulator.EnvironmentConfiguration.OutputMonitorDestinations.Add("dss100");
            simulator.EnvironmentConfiguration.ServerServices.Add("spooler");
            simulator.SessionRuntime.OfficeWorkerEmailAddresses.Add("u00001@etl.boi.rd.hpicorp.net");

            Application.Run(new PluginFrameworkSimulatorForm(simulator));
        }
    }
}
