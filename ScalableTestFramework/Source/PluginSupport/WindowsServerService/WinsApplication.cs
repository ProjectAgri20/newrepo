using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Wins;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class WinsApplication : IWinsApplication
    {
        public WinsApplication()
        {
            Logger.LogInfo("Wins application started...");
        }

        public bool ValidateWinServerLog(string serverIP, string hostName, string recordType, string printerIP)
        {
            return PluginSupport.Connectivity.Wins.WinsApplication.ValidateWinServerLog(serverIP, hostName, recordType, printerIP);
        }

        /// <summary>
        /// Starts the WINS service.
        /// </summary>
        /// <returns>True if the WINS service is started, else false.</returns>
        public bool StartWinsService()
        {
            return PluginSupport.Connectivity.Wins.WinsApplication.StartWinsService();
        }

        /// <summary>
        /// Stops the WINS service
        /// </summary>
        /// <returns>True if the WINS service is stopped, else false.</returns>
        public bool StopWinsService()
        {
            return PluginSupport.Connectivity.Wins.WinsApplication.StopWinsService();
        }

        public bool IsWinsServiceRunning()
        {
            return PluginSupport.Connectivity.Wins.WinsApplication.IsWinsServiceRunning();
        }

        public bool DeleteAllRecords(string serverIP)
        {
            return PluginSupport.Connectivity.Wins.WinsApplication.DeleteAllRecords(serverIP);
        }
    }
}
