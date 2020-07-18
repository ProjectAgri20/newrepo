using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Installs and upgrades print drivers.
    /// </summary>
    public static class DriverInstaller
    {
        private static readonly Dictionary<DriverArchitecture, string> _printUIArchitectures = new Dictionary<DriverArchitecture, string>
        {
            [DriverArchitecture.NTx86] = "x86",
            [DriverArchitecture.NTAMD64] = "x64"
        };

        /// <summary>
        /// Determines whether the specified driver is installed.
        /// </summary>
        /// <param name="driver">The <see cref="DriverDetails" /> describing the driver to check.</param>
        /// <returns><c>true</c> if the specified driver is installed; otherwise, <c>false</c>.</returns>
        public static bool IsInstalled(DriverDetails driver)
        {
            bool driverFound = (from n in DriverController.GetInstalledPrintDrivers()
                                where n.Name.EqualsIgnoreCase(driver.Name)
                                   && n.Architecture == driver.Architecture
                                   && n.Version == driver.Version
                                   && Path.GetFileName(n.InfPath).EqualsIgnoreCase(Path.GetFileName(driver.InfPath))
                                select n).Any();

            bool processorFound = DriverController.GetInstalledPrintProcessors().Contains(driver.PrintProcessor, StringComparer.OrdinalIgnoreCase);

            return driverFound && processorFound;
        }

        /// <summary>
        /// Installs the specified driver.
        /// </summary>
        /// <param name="driver">The <see cref="DriverDetails" /> describing the driver to install.</param>
        public static void Install(DriverDetails driver)
        {
            Install(driver, false);
        }

        /// <summary>
        /// Installs the specified driver.
        /// </summary>
        /// <param name="driver">The <see cref="DriverDetails" /> describing the driver to install.</param>
        /// <param name="forceInstall">if set to <c>true</c> install the driver even if it is already installed.</param>
        public static void Install(DriverDetails driver, bool forceInstall)
        {
            if (!forceInstall && IsInstalled(driver))
            {
                LogInfo($"Driver {driver} already installed.");
            }
            else
            {
                InstallDriver(driver);
            }
        }

        private static void InstallDriver(DriverDetails driver)
        {
            if (string.IsNullOrEmpty(driver.Name))
            {
                throw new ArgumentException("Driver name must be provided.", nameof(driver));
            }

            if (string.IsNullOrEmpty(driver.InfPath))
            {
                throw new ArgumentException("Driver INF path must be provided.", nameof(driver));
            }

            LogInfo($"Installing driver {driver}");

            string architecture = _printUIArchitectures[driver.Architecture];
            string printUICommand = $"/ia /f \"{driver.InfPath}\" /m \"{driver.Name}\" /h \"{architecture}\"";

            LogDebug($"Invoking PrintUI command: {printUICommand}");
            NativeMethods.InvokePrintUI(printUICommand);
            LogInfo($"Driver '{driver}' installed.");
        }

        /// <summary>
        /// Upgrades the driver for the specified print queue.
        /// </summary>
        /// <param name="driver">The <see cref="DriverDetails" /> describing the driver to upgrade to.</param>
        /// <param name="queueName">The queue name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="driver" /> is null.</exception>
        public static void Upgrade(DriverDetails driver, string queueName)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            LogInfo($"Upgrading driver of {queueName} to {driver}");

            string printUICommand = $"/Xs /n \"{queueName}\" DriverName \"{driver.Name}\"";

            LogDebug($"Invoking PrintUI command: {printUICommand}");
            NativeMethods.InvokePrintUI(printUICommand);
            LogInfo($"Driver '{driver}' installed.");
        }

        private static class NativeMethods
        {
            internal static void InvokePrintUI(string command)
            {
                int result = PrintUIEntry(IntPtr.Zero, IntPtr.Zero, command, 5);
                if (result != 0)
                {
                    throw new Win32Exception();
                }
            }
            [DllImport("printui", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int PrintUIEntry(IntPtr hwnd, IntPtr hinst, string lpszCmdLine, int nCmdShow);
        }
    }
}
