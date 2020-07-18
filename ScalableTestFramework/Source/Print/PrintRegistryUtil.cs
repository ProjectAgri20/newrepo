using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Printing;
using System.Security.Principal;
using Microsoft.Win32;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Methods for working with print queue information stored in the system registry.
    /// </summary>
    public static class PrintRegistryUtil
    {
        private const string _devicesSubKey = @"Software\Microsoft\Windows NT\CurrentVersion\Devices";
        private const string _printerPortsSubKey = @"Software\Microsoft\Windows NT\CurrentVersion\PrinterPorts";
        private const string _printersSubKey = @"SYSTEM\CurrentControlSet\Control\Print\Printers";

        /// <summary>
        /// This method creates registry keys allowing printing to work in processes launched as a different user.
        /// </summary>
        /// <remarks>
        /// The first time a process is launched with a new user's credentials, the OS must create that user's profile.
        /// As part of profile creation, the default user registry hive is copied to the new user's profile folder.
        /// Specifically, C:\Users\Default\NTUSER.DAT is copied to C:\Users\{NewUser}\NTUSER.DAT.
        /// A couple of registry keys necessary to printing do not exist in the default user's hive.  A normal interactive 
        /// desktop login creates these keys, but launching a process under different credentials does not.  This is 
        /// evidenced by the fact that logging into the desktop as the new user then logging out makes it possible to
        /// start successfully printing as the new user from a different user's desktop session.
        /// </remarks>
        public static void CreateUserRegistryKeys()
        {
            foreach (string keyPath in new[] { _devicesSubKey, _printerPortsSubKey })
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(keyPath);
                }
                key.Close();
            }
        }

        /// <summary>
        /// Sets the printer attributes for the specified print queue to the specified value.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" /> to modify.</param>
        /// <param name="attributes">The <see cref="PrintQueueAttributes" />.</param>
        /// <remarks>
        /// Print queue attributes can be retrieved (read-only) from a property on the <see cref="PrintQueue" /> object.
        /// This is why there is no symmetrical "GetPrinterAttributes" method.
        /// </remarks>
        internal static void SetPrinterAttributes(PrintQueue queue, PrintQueueAttributes attributes)
        {
            RegistryKey printer = Registry.LocalMachine.OpenSubKey(_printersSubKey).OpenSubKey(queue.FullName, true);
            printer.SetValue("Attributes", (int)attributes);
            printer.Close();
        }

        /// <summary>
        /// Gets the job render location for the specified print queue.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" /> to query.</param>
        /// <returns>The <see cref="PrintJobRenderLocation" /> for the queue.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Many different exceptions could occur, but they all just mean we can't determine the render location.")]
        internal static PrintJobRenderLocation GetJobRenderLocation(PrintQueue queue)
        {
            try
            {
                RegistryKey driverData = Registry.LocalMachine.OpenSubKey(_printersSubKey).OpenSubKey(queue.FullName).OpenSubKey("PrinterDriverData");
                int value = (int?)driverData?.GetValue("EMFDespoolingSetting") ?? 0;
                return (PrintJobRenderLocation)value;
            }
            catch
            {
                return PrintJobRenderLocation.Unknown;
            }
        }

        /// <summary>
        /// Sets the job render location for the specified print queue.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" /> to modify.</param>
        /// <param name="location">The <see cref="PrintJobRenderLocation" />.</param>
        /// <exception cref="ArgumentException"><paramref name="location" /> is <see cref="PrintJobRenderLocation.Unknown" /></exception>
        internal static void SetJobRenderLocation(PrintQueue queue, PrintJobRenderLocation location)
        {
            if (location == PrintJobRenderLocation.Unknown)
            {
                throw new ArgumentException("Cannot set job render location to Unknown.", nameof(location));
            }

            RegistryKey driverData = Registry.LocalMachine.OpenSubKey(_printersSubKey).OpenSubKey(queue.FullName).OpenSubKey("PrinterDriverData", true);
            driverData?.SetValue("EMFDespoolingSetting", (int)location);
            driverData?.Close();
        }

        /// <summary>
        /// Gets a value indicating whether the specified queue has set the InstallationComplete flag.
        /// </summary>
        /// <param name="queueName">The name of the queue to query.</param>
        /// <returns><c>true</c> if the specified queue has the InstallationComplete flag; otherwise, <c>false</c>.</returns>
        internal static bool GetQueueInstallationCompleteStatus(string queueName)
        {
            RegistryKey driverData = Registry.LocalMachine.OpenSubKey(_printersSubKey).OpenSubKey(queueName).OpenSubKey("PrinterDriverData", true);
            int? value = (int?)driverData?.GetValue("InstallationComplete");
            return value == 0;
        }

        /// <summary>
        /// Gets the printer port key for the specified <see cref="PrintQueue" />.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to query.</param>
        /// <returns>The port key for the specified print queue.</returns>
        /// <remarks>
        /// NOTE: This is not the port name that is displayed when the queue properties are viewed.
        /// This port name will be something like "winspool,Ne00" for network ports.
        /// </remarks>
        internal static string GetPrinterPortValue(PrintQueue printQueue)
        {
            string sid = GetCurrentUserSid();
            return GetPrinterPortValue(printQueue, sid);
        }

        /// <summary>
        /// Sets the key that specifies the default print device.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to modify.</param>
        /// <param name="printerPortKey">The printer port key.</param>
        internal static void SetDefaultDeviceKey(PrintQueue printQueue, string printerPortKey)
        {
            RegistryKey deviceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Windows", true);
            deviceKey.SetValue("Device", $"{printQueue.FullName},{printerPortKey}", RegistryValueKind.String);
        }

        /// <summary>
        /// Gets a detailed queue name with network port for the provided <see cref="PrintQueue" />.
        /// </summary>
        /// <remarks>
        /// NOTE: This is not the port name that is displayed when the queue properties are viewed.
        /// This port name will be something like "Ne00:" or "Ne01:" for network ports, or LPT1, etc. for local ports.
        /// </remarks>
        /// <param name="printQueue">The <see cref="PrintQueue" /> associated with the current user.</param>
        /// <returns>The full name of the print queue with the port appended to the end.</returns>
        internal static string GetQueueNameWithPort(PrintQueue printQueue)
        {
            string queueName = printQueue.FullName;
            LogDebug($"Retrieving full queue name for print queue {queueName}");

            string devicePortValue = GetPrinterPortValue(printQueue);
            if (devicePortValue == null)
            {
                // We did not find the devices key in the user hive; try in the admin hive.
                foreach (string sid in GetAdminSids())
                {
                    devicePortValue = GetPrinterPortValue(printQueue, sid);
                    if (devicePortValue != null)
                    {
                        // Copy the key from the admin hive to the user hive
                        SetPrinterPortValue(printQueue, devicePortValue, GetCurrentUserSid());
                        break;
                    }
                }
            }

            if (devicePortValue != null)
            {
                // The key from the registry will be something like "winspool,Ne01:"
                // We only need the part after the comma.
                string portValue = devicePortValue.Split(',')[1];
                string fullQueueName = $"{queueName} on {portValue}";

                LogDebug($"Full queue name: {fullQueueName}");
                return fullQueueName;
            }
            else
            {
                // Did not find a registry value to load.  Return the queuename as-is.
                LogDebug($"Queue not found in registry.  Returning base queue name {queueName}");
                return queueName;
            }
        }

        private static string GetPrinterPortValue(PrintQueue printQueue, string sid)
        {
            return Registry.Users.OpenSubKey(sid)
                                ?.OpenSubKey(_devicesSubKey)
                                ?.GetValue(printQueue.FullName)
                                ?.ToString();
        }

        private static void SetPrinterPortValue(PrintQueue printQueue, string portInfo, string sid)
        {
            RegistryKey devicesKey = Registry.Users.OpenSubKey(sid).OpenSubKey(_devicesSubKey, true);
            devicesKey.SetValue(printQueue.FullName, portInfo, RegistryValueKind.String);
            devicesKey.Close();
        }

        private static string GetCurrentUserSid()
        {
            try
            {
                return UserPrincipal.Current.Sid.Value;
            }
            catch (NoMatchingPrincipalException)
            {
                // Current user is part of a workgroup
                return WindowsIdentity.GetCurrent().User.Value;
            }
            catch (InvalidOperationException)
            {
                return WindowsIdentity.GetCurrent().User.Value;
            }
        }

        private static IEnumerable<string> GetAdminSids()
        {
            return Registry.Users.GetSubKeyNames().Where(n => n.Length > 8
                                                     && !n.StartsWith(WindowsIdentity.GetCurrent().User.Value, StringComparison.OrdinalIgnoreCase)
                                                     && !n.EndsWith("Classes"));
        }
    }
}
