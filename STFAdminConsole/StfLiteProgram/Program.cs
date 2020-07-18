using System;
using System.Windows.Forms;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using HP.ScalableTest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.LabConsole;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using System.Runtime.InteropServices;

namespace HP.SolutionTest.WorkBench
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
        /// The arguments need not be passed for running commandline execution of STB . Same can be achieved by modifying the APP Config XML 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            NameValueCollection _args = new NameValueCollection();
            GlobalSettings.IsDistributedSystem = false;
            
            if (args != null && args.Length > 0)
            {
                CommandLineMessage();
                Console.ReadLine();
            }
            else
            {                
                if (ConfigurationManager.GetSection("UnattendedExecutionConfig") != null)
                    _args.Add(ConfigurationManager.GetSection("UnattendedExecutionConfig") as NameValueCollection);
                           
                if (CommandLineExec.GetAppConfigCount(_args) > 0)
                {
                    SetDispatcherArg(ref _args);
                    using (CommandLineExec commandLine = new CommandLineExec(_args))
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

                    TraceFactory.Logger.Debug("Starting STB User Console UI.");

                    using (Form mainForm = new MainForm())
                    {
                        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        ApplicationExceptionHandler.Attach(mainForm);
                        Application.Run(mainForm);
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
            string messagePrompt = "To run Solution Test Bench unattended, utilize the 'UnattendedExecutionConfig' section in the SolutionTestBench.exe.config XML ";
            Console.WriteLine(messagePrompt);
            Console.WriteLine("Press any key to continue...");
            TraceFactory.Logger.Debug(messagePrompt);
            Console.WriteLine();
        }

        /// <summary>
        /// Creates or Sets the Dispatcher arguement to the App Configurations .        
        /// This method sets the dispatcher tag in the App Config to Machine name for STB . If not present in the APP config then it needs to be handle to avoid exceptions       
        /// </summary>                
        private static void SetDispatcherArg(ref NameValueCollection _args)
        {
            if (_args["dispatcher"] != null)
            {
                _args["dispatcher"] = Environment.MachineName;
            }
            else
            {
                _args.Add("dispatcher", Environment.MachineName);
            }
        }     
    }
}
