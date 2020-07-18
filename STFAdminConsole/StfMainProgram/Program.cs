using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.UI.Framework;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.LabConsole
{
    static class Program
    {

        /// <summary>
        /// Allocate and attach a new command console window to this instance.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// Command Line Usage:         
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            if (args != null && args.Length > 0)
            {
                CommandLineMessage();
                Console.ReadLine();
            }
            else
            {
                NameValueCollection appConfig = ConfigurationManager.GetSection("UnattendedExecutionConfig") as NameValueCollection;
                if (CommandLineExec.GetAppConfigCount(appConfig) > 0)
                {
                    using (CommandLineExec commandLine = new CommandLineExec(appConfig))
                    {
                        try
                        {
                            commandLine.StatusChanged += CommandLine_StatusChanged;
                            FrameworkServicesInitializer.InitializeExecution();
                            commandLine.StartSession();
                        }
                        catch (Exception ex)
                        {
                            TraceFactory.Logger.Debug(ex.ToString());
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    try
                    {
                        using (Form mainForm = new MainForm())
                        {
                            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            ApplicationExceptionHandler.Attach(mainForm);
                            Application.Run(mainForm);
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error(ex);
                        Application.Exit();
                    }
                }
            }
        }

        private static void CommandLine_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(e.StatusMessage);
        }

        private static void CommandLineMessage()
        {
            //We need to invoke the console in order to show the below message. Otherwise the running application simply terminates since its a windows based Applcation
            AllocConsole();
            string messagePrompt = "To run Solution Test Enterprise unattended, modify the 'UnattendedExecutionConfig' section in the App Config XML ";
            Console.WriteLine(messagePrompt);
            Console.WriteLine("Press any key to continue...");
            TraceFactory.Logger.Debug(messagePrompt);
            Console.WriteLine();            
        }

    }
}
