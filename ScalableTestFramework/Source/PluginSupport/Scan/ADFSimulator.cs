using System.Globalization;
using System.Net.Sockets;
using HP.DeviceAutomation;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Controller for the ADF simulator on a Jedi simulator VM.
    /// </summary>
    public static class AdfSimulator
    {
        private const string _commandAdd = "adf add simplex letter short \"{0}\" 1";
        private const string _commandClear = "adf clear";
        private const string _commandPageCount = "adf getpagecount";

        /// <summary>
        /// Loads the specified document into the simulated ADF.
        /// </summary>
        /// <param name="hostName">Hostname of the simulator.</param>
        /// <param name="filePath">The file path.</param>
        public static void LoadPage(string hostName, string filePath)
        {
            string loadFile = string.Format(_commandAdd, filePath);
            int initialPageCount = GetPageCount(hostName);
            int currentPageCount = initialPageCount;

            while (currentPageCount <= initialPageCount)
            {
                LogDebug($"Sending ADF command: {loadFile}");
                RunCommand(hostName, loadFile);
                currentPageCount = GetPageCount(hostName);
            }
        }

        /// <summary>
        /// Clears all documents from the simulated ADF.
        /// </summary>
        /// <param name="hostName">Hostname of the simulator.</param>
        public static void Clear(string hostName)
        {
            LogDebug("Clearing ADF simulator.");
            RunCommand(hostName, _commandClear);
        }

        /// <summary>
        /// Gets the number of pages currently loaded into the simulated ADF.
        /// </summary>
        /// <param name="hostName">Hostname of the simulator.</param>
        /// <returns>The number of pages in the simulated ADF.</returns>
        public static int GetPageCount(string hostName)
        {
            int pageCount = int.Parse(RunCommand(hostName, _commandPageCount), CultureInfo.InvariantCulture);
            LogDebug($"{pageCount} page(s) found in ADF simulator.");
            return pageCount;
        }

        private static string RunCommand(string hostName, string command)
        {
            try
            {
                using (Telnet telnet = new Telnet(hostName, 1952))
                {
                    string prompt = telnet.ReceiveUntilMatch(">", string.Empty);
                    telnet.SendLine(command);
                    return telnet.ReceiveUntilMatch(">", prompt);
                }
            }
            catch (SocketException ex)
            {
                throw new DeviceCommunicationException($"ADF simulator at {hostName} did not respond.", ex);
            }
        }
    }
}
