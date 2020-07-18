using System;
using System.IO;
using System.Windows.Forms;
using HP.ScalableTest;
using HP.ScalableTest.Framework.Settings;

namespace HP.SolutionTest.Install
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                SystemTrace.Instance.Debug("Starting installer");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SchemaInstallerForm form = null;
                if (args.Length == 1)
                {
                    SystemTrace.Instance.Debug("Reading {0}".FormatWith(args[0]));

                    var lines = File.ReadAllLines(args[0]);
                    var items = lines[0].Split('\\');

                    SchemaTicket ticket = new SchemaTicket()
                    {
                        AdminDomain = items[0],
                        AdminUserName = items[1],
                        AdminEmail = lines[1],
                        OrganizationName = lines[2],
                        DatabaseFilesPath = lines[3],
                        BuildConfig = GetBuildConfig(lines[4]),
                        FileSharePath = lines[5]
                    };

                    form = new SchemaInstallerForm(ticket);
                    SystemTrace.Instance.Debug("Created ticket");
                }
                else if (args.Length == 2 && args[0].Equals("STFUPDATE", StringComparison.OrdinalIgnoreCase))
                {
                    form = new SchemaInstallerForm(new SchemaTicket());

                    // This will set the hostname for the Primary (EnterpriseTest) hostname, which
                    // will prompt the installer to just perform a database update using the hostnames
                    // listed in the SystemSetting table of the primary database.  This will be used
                    // to manually update a STF system where each database may reside on a different server.
                    form.EnterpriseTestHostname = args[1];
                }
                Application.Run(form);
            }
            catch (Exception ex)
            {
                SystemTrace.Instance.Error("Failed to start installer", ex);
            }
        }

        private static BuildConfiguration GetBuildConfig(string configParam)
        {
            BuildConfiguration result = BuildConfiguration.Solution;
            try
            {
                result = (BuildConfiguration)Enum.Parse(typeof(BuildConfiguration), configParam);
            }
            catch { } //Couldn't parse to BuildConfiguration enum.  Assuming Solution configuration.

            return result;
        }
    }
}