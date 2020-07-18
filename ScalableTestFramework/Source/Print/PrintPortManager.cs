using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using HP.ScalableTest.WindowsAutomation;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Contains methods for working with print ports on the local machine.
    /// </summary>
    public static class PrintPortManager
    {
        private const int _rawPortProtocol = 1;
        private const int _lprPortProtocol = 2;

        /// <summary>
        /// Gets a collection of available print ports on the local machine.
        /// </summary>
        /// <returns>A collection of strings representing the available print ports.</returns>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public static Collection<string> GetInstalledPorts()
        {
            LogTrace("Retrieving installed printer ports.");
            Collection<string> ports = new Collection<string>();
            foreach (string port in NativeMethods.EnumeratePorts())
            {
                ports.Add(port);
            }
            return ports;
        }

        /// <summary>
        /// Creates a RAW print port on the local machine.
        /// </summary>
        /// <param name="portName">The port name.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="address">The address.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void AddRawPort(string portName, int portNumber, string address)
        {
            AddPort(portName, _rawPortProtocol, portNumber, address, string.Empty);
        }

        /// <summary>
        /// Creates a RAW print port on the local machine.
        /// </summary>
        /// <param name="portName">The port name.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="address">The address.</param>
        /// <param name="snmpEnabled">if set to <c>true</c> enable SNMP for the port.</param>
        /// <param name="snmpCommunity">The SNMP community name.</param>
        /// <param name="snmpDevIndex">The SNMP dev index.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void AddRawPort(string portName, int portNumber, string address, bool snmpEnabled, string snmpCommunity, int snmpDevIndex)
        {
            AddPort(portName, _rawPortProtocol, portNumber, address, string.Empty, snmpEnabled, snmpCommunity, snmpDevIndex);
        }

        /// <summary>
        /// Creates an LPR print port on the local machine.
        /// </summary>
        /// <param name="portName">The port name.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="address">The address.</param>
        /// <param name="queue">The queue name.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void AddLprPort(string portName, int portNumber, string address, string queue)
        {
            AddPort(portName, _lprPortProtocol, portNumber, address, queue);
        }

        /// <summary>
        /// Creates an LPR print port on the local machine.
        /// </summary>
        /// <param name="portName">The port name.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="address">The address.</param>
        /// <param name="queue">The queue name.</param>
        /// <param name="snmpEnabled">if set to <c>true</c> enable SNMP for the port.</param>
        /// <param name="snmpCommunity">The SNMP community name.</param>
        /// <param name="snmpDevIndex">The SNMP dev index.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void AddLprPort(string portName, int portNumber, string address, string queue, bool snmpEnabled, string snmpCommunity, int snmpDevIndex)
        {
            AddPort(portName, _lprPortProtocol, portNumber, address, queue, snmpEnabled, snmpCommunity, snmpDevIndex);
        }

        private static void AddPort(string portName, int protocol, int portNumber, string address, string queue, bool snmpEnabled = false, string snmpCommunity = "public", int snmpDevIndex = 1)
        {
            if (PortExists(portName))
            {
                LogDebug($"Port '{portName}' already exists.");
                return;
            }

            LogDebug($"Creating port '{portName}'.");

            using (SafePrinterHandle xcvMonitor = NativeMethods.OpenXcvMonitor())
            {
                var portData = new NativeMethods.PortData
                {
                    PortName = portName,
                    Protocol = (uint)protocol,
                    PortNumber = (uint)portNumber,
                    IPAddress = address,
                    HostAddress = address,
                    Queue = queue,
                    SnmpEnabled = snmpEnabled ? 1U : 0U,
                    SnmpCommunity = snmpCommunity,
                    SnmpDevIndex = (uint)snmpDevIndex,
                    Version = 1,
                    Reserved = 0,
                    Size = (uint)Marshal.SizeOf<NativeMethods.PortData>()
                };

                NativeMethods.XcvPortData(xcvMonitor, "AddPort", portData);
                LogDebug($"Port '{portName}' created.");
            }
        }

        /// <summary>
        /// Deletes the specified print port.
        /// </summary>
        /// <param name="portName">The port name.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void DeletePort(string portName)
        {
            if (!PortExists(portName))
            {
                LogDebug($"Port '{portName}' does not exist.");
                return;
            }

            LogDebug($"Deleting port '{portName}'.");

            using (SafePrinterHandle xcvMonitor = NativeMethods.OpenXcvMonitor())
            {
                var portData = new NativeMethods.DeletePortData()
                {
                    PortName = portName,
                    Version = 1,
                    Reserved = 0
                };

                NativeMethods.XcvPortData(xcvMonitor, "DeletePort", portData);
                LogDebug($"Port '{portName}' deleted.");
            }
        }

        private static bool PortExists(string portName)
        {
            return GetInstalledPorts().Any(n => n.Equals(portName, StringComparison.OrdinalIgnoreCase));
        }

        private static class NativeMethods
        {
            internal static SafePrinterHandle OpenXcvMonitor()
            {
                PrinterDefaults defaults = PrinterDefaults.AllAccess;
                bool success = OpenPrinter(",XcvMonitor Standard TCP/IP Port", out SafePrinterHandle monitor, ref defaults);

                if (Marshal.GetLastWin32Error() > 0)
                {
                    throw new Win32Exception();
                }

                if (!success)
                {
                    throw new Win32Exception("Call to OpenPrinter failed.");
                }

                return monitor;
            }

            internal static void XcvPortData(SafePrinterHandle xcvMonitor, string command, object portData)
            {
                // TCPMON XCV commands are listed at https://msdn.microsoft.com/en-us/windows/hardware/drivers/print/tcpmon-xcv-commands
                int size = Marshal.SizeOf(portData);
                using (SafeUnmanagedMemoryHandle inputDataPointer = new SafeUnmanagedMemoryHandle(size))
                {
                    Marshal.StructureToPtr(portData, inputDataPointer.DangerousGetHandle(), true);
                    if (!XcvData(xcvMonitor, command, inputDataPointer, (uint)size, IntPtr.Zero, 0, out uint outputNeededSize, out uint status))
                    {
                        throw new Win32Exception();
                    }
                }
            }

            internal static IEnumerable<string> EnumeratePorts()
            {
                uint needed = 0;
                uint returned = 0;

                // As per MSDN, call EnumPorts with an empty buffer.
                // This call will fail, but will populate 'needed' with the required size of the buffer.
                using (SafeUnmanagedMemoryHandle nullBuffer = new SafeUnmanagedMemoryHandle(0))
                {
                    EnumPorts(null, 2, nullBuffer, 0, ref needed, ref returned);
                }

                // Allocate the required memory and retrieve the port data
                using (SafeUnmanagedMemoryHandle buffer = new SafeUnmanagedMemoryHandle((int)needed))
                {
                    if (EnumPorts(null, 2, buffer, needed, ref needed, ref returned))
                    {
                        PortInfo[] ports = new PortInfo[returned];
                        IntPtr currentPort = buffer.DangerousGetHandle();
                        for (int i = 0; i < returned; i++)
                        {
                            ports[i] = Marshal.PtrToStructure<PortInfo>(currentPort);
                            currentPort = IntPtr.Add(currentPort, Marshal.SizeOf<PortInfo>());
                        }
                        return ports.Select(n => n.PortName);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct PortInfo
            {
                public string PortName;
                public string MonitorName;
                public string Description;
                public int PortType;
                public int Reserved;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct PortData
            {
                private const int MAX_PORTNAME_LEN = 64;
                private const int MAX_NETWORKNAME_LEN = 49;
                private const int MAX_SNMP_COMMUNITY_STR_LEN = 33;
                private const int MAX_QUEUENAME_LEN = 33;
                private const int MAX_IPADDR_STR_LEN = 16;
                private const int RESERVED_BYTE_ARRAY_SIZE = 540;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PORTNAME_LEN)]
                public string PortName;
                public uint Version;
                public uint Protocol;
                public uint Size;
                public uint Reserved;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_NETWORKNAME_LEN)]
                public string HostAddress;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SNMP_COMMUNITY_STR_LEN)]
                public string SnmpCommunity;
                public uint DoubleSpool;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_QUEUENAME_LEN)]
                public string Queue;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_IPADDR_STR_LEN)]
                public string IPAddress;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = RESERVED_BYTE_ARRAY_SIZE)]
                public byte[] ReservedArray;
                public uint PortNumber;
                public uint SnmpEnabled;
                public uint SnmpDevIndex;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct DeletePortData
            {
                private const int MAX_PORTNAME_LEN = 64;
                private const int RESERVED_BYTE_ARRAY_SIZE = 98;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PORTNAME_LEN)]
                public string PortName;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = RESERVED_BYTE_ARRAY_SIZE)]
                public byte[] ReservedArray;
                public uint Version;
                public uint Reserved;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct PrinterDefaults
            {
                private const uint GENERIC_ALL = 0x10000000;

                public IntPtr DataType;
                public IntPtr DevMode;
                public uint DesiredAccess;

                public static PrinterDefaults AllAccess { get; } = new PrinterDefaults { DesiredAccess = GENERIC_ALL };
            }

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenPrinter(string pPrinterName, out SafePrinterHandle phPrinter, ref PrinterDefaults pDefault);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool XcvData(SafePrinterHandle hXcv, string pszDataName, SafeUnmanagedMemoryHandle pInputData, uint cbInputData, IntPtr pOutputData, uint cbOutputData, out uint pcbOutputNeeded, out uint pdwStatus);

            [DllImport("winspool.drv", CharSet = CharSet.Ansi, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumPorts([MarshalAs(UnmanagedType.LPWStr)] string pName, uint level, SafeUnmanagedMemoryHandle pPorts, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);
        }
    }
}
