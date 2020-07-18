using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Virtualization;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Provides functionality for operations on a Jedi Simulator.
    /// </summary>
    public static class JediSimulatorManager
    {
        private const string _cpbLogLocation = @"C:\jedi-simulator\System\Simulator\ScanSimulator\Cpb\Output";

        /// <summary>
        /// Launches the simulator.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="token">The token.</param>
        public static void LaunchSimulator(string machineName, string productName, string hostAddress, CancellationToken token = default(CancellationToken))
        {
            string command = @"C:\jedi-simulator\System\Tools\bin\Xp\HP.Tools.Build.Packaging.Config.exe";
            string commandArguments = "-os XP -cpu x86 -p {0} -h BaseProduct -f Standard -mode Development -Release -sim -package -deploy -start".FormatWith(productName);

            try
            {
                RunGuestProcess(machineName, command, commandArguments, waitForExit: false);
                TraceFactory.Logger.Debug("Guest process run to start simulator");

                // Wait for the port to be available, then give things another minute to settle down
                if (WaitForPortAvailable(hostAddress, 65102, token: token))
                {
                    TraceFactory.Logger.Info("Simulator launched successfully, sleeping a bit before returning");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    return;
                }

                if (token.IsCancellationRequested)
                {
                    TraceFactory.Logger.Debug("Cancellation received, returning");
                    return;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("FAILED TO LAUNCH SIMULATOR");
                TraceFactory.Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Shuts down the simulator.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public static void ShutdownSimulator(string machineName)
        {
            string command = @"C:\jedi-simulator\System\Tools\bin\Xp\HP.Tools.Build.Clean.exe";
            string commandArguments = "-KillJustSimProcesses -os XP";

            RunGuestProcess(machineName, command, commandArguments, waitForExit: true);
            TraceFactory.Logger.Info("Simulator shut down successfully.");
        }

        /// <summary>
        /// Determines whether the simulator is up and ready.
        /// </summary>
        /// <param name="hostAddress">The machine name.</param>
        /// <returns><c>true</c> if the simulator is ready; otherwise, <c>false</c>.</returns>
        public static bool IsSimulatorReady(string hostAddress)
        {
            JediDevice simulator = new JediDevice(hostAddress);
            return simulator.GetDeviceStatus() == DeviceStatus.Running &&
                   simulator.GetPrinterStatus() >= PrinterStatus.Idle;
        }

        private static bool WaitForPortAvailable(string hostname, int port, CancellationToken token)
        {
            IPAddress address;
            if (!IPAddress.TryParse(hostname, out address))
            {
                address = Dns.GetHostEntry(hostname).AddressList.First(i => i.AddressFamily == AddressFamily.InterNetwork);
            }

            var endpoint = new IPEndPoint(address, port);

            TraceFactory.Logger.Info("Waiting for {0}:{1} to be available".FormatWith(hostname, port));
            var timeout = TimeSpan.FromMinutes(4);
            var startTime = DateTime.Now;
            while (DateTime.Now - startTime < timeout)
            {
                if (token.IsCancellationRequested)
                {
                    TraceFactory.Logger.Debug("Cancellation received, returning");
                    return false;
                }

                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect(endpoint);
                        TraceFactory.Logger.Info("{0}:{1} is now available".FormatWith(hostname, port));
                        return true;
                    }
                }
                catch (SocketException ex)
                {
                    TraceFactory.Logger.Info("Ignoring exception: " + ex.Message);
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }

            throw new TimeoutException("Timed out waiting for {0}:{1} to become available".FormatWith(hostname, port));
        }

        /// <summary>
        /// Copies the CPB logs to the specified destination.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyLogs(string machineName, string destination)
        {
            string cmdArguments = @"/C XCOPY {0} {1} /I".FormatWith(_cpbLogLocation, destination);

            RunGuestProcess(machineName, "cmd", cmdArguments, waitForExit: true);
        }

        private static bool RunGuestProcess(string hostName, string command, string arguments, bool waitForExit = false)
        {
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, hostName);
                var pid = vSphereController.RunGuestProcess(machine, command, arguments, GlobalSettings.Items.DomainAdminCredential, waitForExit);
                return pid > 0;
            }
        }

        private static VSphereVMController GetVSphereController()
        {
            string serverUri = GlobalSettings.Items[Setting.VMWareServerUri];
            return new VSphereVMController
            (
                new Uri(serverUri), UserManager.CurrentUser.ToNetworkCredential()
            );
        }

        private static VSphereVirtualMachine GetVirtualMachine(VSphereVMController vSphereController, string hostName)
        {
            return vSphereController.GetVirtualMachines().First(n => n.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
