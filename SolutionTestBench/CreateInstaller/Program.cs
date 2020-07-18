using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace CreateInstaller
{
    public class Program
    {
        private static ConcurrentStack<CommandLineBuilder> _processes = new ConcurrentStack<CommandLineBuilder>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            try
            {
                if (args != null && args.Length > 0)
                {
                    //-workingPath, -installerTypes (Client|Server), -buildConfig (AllPlugins|Base)
                    CommandLineArguments arguments = new CommandLineArguments(args);

                    RunCommandLine(arguments);
                }
                else
                {
                    //Run the UI
                    Application.Run(new MainForm());
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteErrorLog(e.ExceptionObject.ToString());
        }

        private static void RunCommandLine(CommandLineArguments arguments)
        {
            List<Task> tasks = new List<Task>();

            // Build each configuration specified
            foreach (string buildConfig in arguments.GetParameterValue("buildConfig").Split('|'))
            {
                foreach (string installType in arguments.GetParameterValue("installerTypes").Split('|'))
                {
                    tasks.Add(Task.Factory.StartNew(() => BuildInstaller(arguments.GetParameterValue("workingPath"), installType, buildConfig)));
                }
            }

            if (tasks.Any())
            {
                // Wait until we have forms and none of them are processing
                while (_processes.Any() && _processes.Any(x => x.Status != InstallerStatus.Completed))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            _processes.Clear();
            tasks.Clear();
        }

        private void WaitForProcessComplete()
        {
        }

        private static void BuildInstaller(string workingPath, string installerType, string configuration)
        {
            Installer installerBuilder = new Installer(workingPath, installerType, Installer.GetPackageLabel(workingPath), configuration);
            CommandLineBuilder cmdBuilder = new CommandLineBuilder(installerBuilder, workingPath);

            // Add the builder to the list so that we can monitor it's progress
            _processes.Push(cmdBuilder);
            cmdBuilder.Execute();
        }

        private static void CommandLine_MessageUpdate(object sender, InstallEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void WriteErrorLog(string logText)
        {
            Assembly exe = Assembly.GetExecutingAssembly();

            StringBuilder filePath = new StringBuilder(Path.GetDirectoryName(exe.Location));
            filePath.Append("\\");
            filePath.Append(exe.GetName().Name);
            filePath.Append(".log");

            File.WriteAllText(filePath.ToString(), logText);
        }

    }
}
