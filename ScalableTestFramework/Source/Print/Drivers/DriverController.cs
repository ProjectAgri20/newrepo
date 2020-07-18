using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32;
using static HP.ScalableTest.Framework.Logger;
using ComFileTime = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Provides information about print drivers on the local machine.
    /// </summary>
    public static class DriverController
    {
        private static readonly Lazy<string> _driverDirectory = new Lazy<string>(NativeMethods.GetPrinterDriverDirectory);

        /// <summary>
        /// Gets the <see cref="DriverArchitecture" /> for the local machine.
        /// </summary>
        public static DriverArchitecture LocalArchitecture
        {
            get { return Environment.Is64BitOperatingSystem ? DriverArchitecture.NTAMD64 : DriverArchitecture.NTx86; }
        }

        /// <summary>
        /// Gets the path to the system driver directory for V3 drivers.
        /// </summary>
        public static string SystemVersion3DriverDirectory
        {
            get { return Path.Combine(_driverDirectory.Value, "3"); }
        }

        /// <summary>
        /// Gets a collection of available print processors on the local machine.
        /// </summary>
        /// <returns>A collection of strings representing the available print processors.</returns>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public static Collection<string> GetInstalledPrintProcessors()
        {
            LogDebug("Retrieving installed print processors.");
            Collection<string> processors = new Collection<string>();
            foreach (string printProcessor in NativeMethods.EnumeratePrintProcessors())
            {
                processors.Add(printProcessor);
            }
            return processors;
        }

        /// <summary>
        /// Gets a collection of available print drivers on the local machine.
        /// </summary>
        /// <returns>A collection of <see cref="DriverDetails" /> representing the available print drivers.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public static Collection<DriverDetails> GetInstalledPrintDrivers()
        {
            LogDebug("Retrieving installed print drivers.");
            Collection<DriverDetails> drivers = new Collection<DriverDetails>();
            foreach (DriverDetails driver in LoadFromRegistry())
            {
                drivers.Add(driver);
            }
            return drivers;
        }

        /// <summary>
        /// Creates a collection of <see cref="DriverDetails" /> for drivers in the inbox driver store.
        /// </summary>
        /// <param name="includeAllArchitectures">if set to <c>true</c> include drivers with architectures other than <see cref="LocalArchitecture" />.</param>
        /// <returns>A collection of <see cref="DriverDetails" /> representing all drivers in the inbox driver store.</returns>
        public static IEnumerable<DriverDetails> LoadFromDriverStore(bool includeAllArchitectures)
        {
            string inboxPath = string.Empty;

            // Assume that for Vista or greater the DriverStore is used...
            if (Environment.OSVersion.Version.Major >= 6)
            {
                inboxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"DriverStore\FileRepository");
            }
            else
            {
                inboxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "inf");
            }

            if (Directory.Exists(inboxPath))
            {
                return LoadFromDirectory(inboxPath, includeAllArchitectures);
            }
            else
            {
                return Enumerable.Empty<DriverDetails>();
            }
        }

        /// <summary>
        /// Creates a collection of <see cref="DriverDetails" /> for drivers in the specified directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="includeAllArchitectures">if set to <c>true</c> include drivers with architectures other than <see cref="LocalArchitecture" />.</param>
        /// <returns>A collection of <see cref="DriverDetails" /> representing all drivers in the specified directory.</returns>
        public static IEnumerable<DriverDetails> LoadFromDirectory(string path, bool includeAllArchitectures)
        {
            return LoadFromDirectory(path, includeAllArchitectures, SearchOption.AllDirectories);
        }

        /// <summary>
        /// Creates a collection of <see cref="DriverDetails" /> for drivers in the specified directory, optionally ignoring subdirectories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="includeAllArchitectures">if set to <c>true</c> include drivers with architectures other than <see cref="LocalArchitecture" />.</param>
        /// <param name="searchOption">Specifies whether to search subdirectories for drivers.</param>
        /// <returns>A collection of <see cref="DriverDetails" /> representing all drivers in the specified directory.</returns>
        public static IEnumerable<DriverDetails> LoadFromDirectory(string path, bool includeAllArchitectures, SearchOption searchOption)
        {
            LogDebug($"Loading drivers from {path}");

            List<DriverDetails> drivers = new List<DriverDetails>();
            foreach (string fileName in Directory.GetFiles(path, "*.inf", searchOption))
            {
                using (DriverInfReader reader = new DriverInfReader(fileName))
                {
                    DriverInfParser parser = new DriverInfParser(reader);
                    DriverInf inf = parser.BuildInf();
                    if (inf.Drivers.Any())
                    {
                        if (includeAllArchitectures)
                        {
                            drivers.AddRange(inf.Drivers);
                        }
                        else
                        {
                            drivers.AddRange(inf.Drivers.Where(n => n.Architecture == LocalArchitecture));
                        }
                    }
                }
            }

            LogDebug($"{drivers.Count} drivers loaded.");
            return drivers;
        }

        /// <summary>
        /// Creates a collection of <see cref="DriverDetails" /> for drivers from the registry.
        /// </summary>
        /// <returns>A collection of <see cref="DriverDetails" /> representing all drivers in the registry.</returns>
        public static IEnumerable<DriverDetails> LoadFromRegistry()
        {
            LogDebug("Loading drivers from the registry.");

            var drivers = LoadDrivers("Windows NT x86", DriverArchitecture.NTx86)
                   .Union(LoadDrivers("Windows x64", DriverArchitecture.NTAMD64)).ToList();

            if (drivers.Any(n => n.InfPath == null))
            {
                UpdateEmptyInfFromCache(drivers);
            }

            LogDebug($"{drivers.Count} drivers loaded.");

            return drivers;
        }

        private static IEnumerable<DriverDetails> LoadDrivers(string environment, DriverArchitecture architecture)
        {
            string driversRegistryPath = @"SYSTEM\CurrentControlSet\Control\Print\Environments\{0}\Drivers\Version-3";

            List<DriverDetails> drivers = new List<DriverDetails>();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(string.Format(driversRegistryPath, environment)))
            {
                if (key != null)
                {
                    foreach (string driverName in key.GetSubKeyNames())
                    {
                        using (RegistryKey driverKey = key.OpenSubKey(driverName))
                        {
                            // The registry entry for the driver must have the DriverVersion.  If not,
                            // we will consider this entry NOT a driver...
                            if (driverKey?.GetValue("DriverVersion") != null)
                            {
                                DriverDetails driver = CreateDriverDetails(driverKey, driverName);
                                driver.Architecture = architecture;
                                drivers.Add(driver);
                            }
                        }
                    }
                }
            }
            return drivers;
        }

        private static DriverDetails CreateDriverDetails(RegistryKey key, string driverName)
        {
            DriverDetails driver = new DriverDetails()
            {
                Name = driverName,
                InfPath = (string)key.GetValue("InfPath"),
                DriverFile = (string)key.GetValue("Driver"),
                ConfigurationFile = (string)key.GetValue("Configuration File"),
                HelpFile = (string)key.GetValue("Help File"),
                DataFile = (string)key.GetValue("Data File"),
                PrintProcessor = (string)key.GetValue("Print Processor"),
                Monitor = (string)key.GetValue("Monitor"),
                Provider = (string)key.GetValue("Provider")
            };

            if (key.GetValueKind("DriverVersion") == RegistryValueKind.Binary)
            {
                byte[] versionValue = (byte[])key.GetValue("DriverVersion");
                driver.Version = new DriverVersion(ParseRegistryDriverVersion(versionValue));
            }
            else
            {
                driver.Version = new DriverVersion((string)key.GetValue("DriverVersion"));
            }

            if (key.GetValueKind("DriverDate") == RegistryValueKind.Binary)
            {
                byte[] dateValue = (byte[])key.GetValue("DriverDate");
                driver.DriverDate = ParseRegistryDriverDate(dateValue);
            }
            else
            {
                driver.DriverDate = (string)key.GetValue("DriverDate");
            }

            return driver;
        }

        private static void UpdateEmptyInfFromCache(IEnumerable<DriverDetails> drivers)
        {
            List<DriverDetails> inboxCache = LoadFromDriverStore(true).ToList();

            foreach (DriverDetails driver in drivers.Where(n => n.InfPath == null))
            {
                driver.InfPath = (from n in inboxCache
                                  where n.Name.Equals(driver.Name, StringComparison.OrdinalIgnoreCase)
                                     && n.Architecture == driver.Architecture
                                     && n.Version == driver.Version
                                  select n.InfPath).FirstOrDefault();
            }
        }

        private static string ParseRegistryDriverDate(byte[] data)
        {
            DateTime systemTime = NativeMethods.ParseDriverDate(data);
            return string.Format("{0:D2}/{1:D2}/{2:D4}", systemTime.Month, systemTime.Day, systemTime.Year);
        }

        private static string ParseRegistryDriverVersion(byte[] data)
        {
            uint major = BitConverter.ToUInt16(data, 6);
            uint minor = BitConverter.ToUInt16(data, 4);
            uint rev = BitConverter.ToUInt16(data, 2);
            uint build = BitConverter.ToUInt16(data, 0);

            return string.Format("{0}.{1}.{2}.{3}", major, minor, rev, build);
        }

        private static class NativeMethods
        {
            internal static IEnumerable<string> EnumeratePrintProcessors()
            {
                uint needed = 0;
                uint returned = 0;

                // As per MSDN, call EnumPrintProcessors with an empty buffer.
                // This call will fail, but will populate 'needed' with the required size of the buffer.
                using (SafeUnmanagedMemoryHandle nullBuffer = new SafeUnmanagedMemoryHandle(0))
                {
                    EnumPrintProcessors(null, null, 1, nullBuffer, needed, ref needed, ref returned);
                }

                // Allocate the required memory and retrieve the print processor data
                using (SafeUnmanagedMemoryHandle buffer = new SafeUnmanagedMemoryHandle((int)needed))
                {
                    if (EnumPrintProcessors(null, null, 1, buffer, needed, ref needed, ref returned))
                    {
                        PrintProcessorInfo[] printProcessors = new PrintProcessorInfo[returned];
                        IntPtr currentPrintProcessor = buffer.DangerousGetHandle();
                        for (int i = 0; i < returned; i++)
                        {
                            printProcessors[i] = Marshal.PtrToStructure<PrintProcessorInfo>(currentPrintProcessor);
                            currentPrintProcessor = IntPtr.Add(currentPrintProcessor, Marshal.SizeOf<PrintProcessorInfo>());
                        }
                        return printProcessors.Select(n => n.Name);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }
                }
            }

            internal static string GetPrinterDriverDirectory()
            {
                StringBuilder buffer = new StringBuilder(1024);
                if (GetPrinterDriverDirectory(null, null, 1, buffer, 500, out _))
                {
                    return buffer.ToString();
                }
                else
                {
                    throw new Win32Exception();
                }
            }

            internal static DateTime ParseDriverDate(byte[] data)
            {
                SystemTime systemTime = new SystemTime();
                ComFileTime fileTime = new ComFileTime
                {
                    dwLowDateTime = BitConverter.ToInt32(data, 0),
                    dwHighDateTime = BitConverter.ToInt32(data, 4)
                };
                FileTimeToSystemTime(ref fileTime, ref systemTime);
                return new DateTime(systemTime.Year, systemTime.Month, systemTime.Day, systemTime.Hour, systemTime.Minute, systemTime.Second, systemTime.Milliseconds, DateTimeKind.Utc);
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct PrintProcessorInfo
            {
                public string Name;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct SystemTime
            {
                public ushort Year;
                public ushort Month;
                public ushort DayOfWeek;
                public ushort Day;
                public ushort Hour;
                public ushort Minute;
                public ushort Second;
                public ushort Milliseconds;
            }

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumPrintProcessors(string pName, string pEnvironment, uint level, SafeUnmanagedMemoryHandle pPrintProcessorInfo, uint cbBuf, ref uint cbNeeded, ref uint cReturned);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetPrinterDriverDirectory(string pName, string pEnvironment, uint level, StringBuilder pDriverDirectory, uint cbBuf, out uint pcbNeeded);

            [DllImport("kernel32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FileTimeToSystemTime(ref ComFileTime fileTime, ref SystemTime systemTime);
        }
    }
}
