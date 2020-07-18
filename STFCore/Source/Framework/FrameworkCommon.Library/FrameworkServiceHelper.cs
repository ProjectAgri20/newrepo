using System;
using System.Linq;
using System.Reflection;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework
{
    public static class FrameworkServiceHelper
    {
        /// <summary>
        /// Loads the global settings, using either a local cache or passed-in args.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" />.</param>
        /// <param name="autoRefresh">if set to <c>true</c> automatically refresh from the database.</param>
        public static void LoadSettings(CommandLineArguments args, bool autoRefresh = false)
        {
            string systemSettingsDatabase = args.GetParameterValue("database");
            GlobalSettings.Load(systemSettingsDatabase, autoRefresh: autoRefresh);
        }

        /// <summary>
        /// Updates Environment, version and database host name in global database.
        /// </summary>
        public static void UpdateServerInfo()
        {
            try
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    FrameworkServer server = context.FrameworkServers.FirstOrDefault(n => n.HostName == Environment.MachineName);
                    if (server != null)
                    {
                        server.Environment = GlobalSettings.Items[Setting.Environment];
                        server.StfServiceVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex.ToString());
            }
        }
    }
}
