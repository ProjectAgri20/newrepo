using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class KiwiSyslogApplication : IKiwiSyslogApplication
    {
        public KiwiSyslogApplication()
        {
            Logger.LogInfo("Kiwi Syslog application started...");
        }

        public bool StartKiwiSyslogServer()
        {
            return PluginSupport.Connectivity.KiwiSyslog.KiwiSyslogApplication.StartSyslogServer();
        }

        public bool StopKiwiSyslogServer()
        {
            return PluginSupport.Connectivity.KiwiSyslog.KiwiSyslogApplication.StopSyslogServer();
        }

        public bool ValidateEntry(DateTime startTime, DateTime endTime, string entry)
        {
            return PluginSupport.Connectivity.KiwiSyslog.KiwiSyslogApplication.ValidateEntry(startTime, endTime, entry);
        }
    }
}
