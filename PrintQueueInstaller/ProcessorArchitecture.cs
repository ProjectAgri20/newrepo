using System.Collections.Generic;
using System.ComponentModel;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Represents the various processor architectures available.
    /// </summary>
    /// <remarks>
    /// The spelling of these enumerated values were chosen based off of the supported values found in an INF file.
    /// </remarks>
    internal enum ProcessorArchitecture
    {
        /// <summary>
        /// X86 (32-bit)
        /// </summary>
        [Description("x86")]
        NTx86,

        /// <summary>
        /// AMD64 (64-bit)
        /// </summary>
        [Description("x64")]
        NTAMD64
    }

    internal static class ProcessorArchitectureHelper
    {
        private static readonly Dictionary<ProcessorArchitecture, DriverArchitecture> _map = new Dictionary<ProcessorArchitecture, DriverArchitecture>
        {
            [ProcessorArchitecture.NTAMD64] = DriverArchitecture.NTAMD64,
            [ProcessorArchitecture.NTx86] = DriverArchitecture.NTx86
        };

        public static DriverArchitecture ToDriverArchitecture(this ProcessorArchitecture architecture)
        {
            return _map[architecture];
        }
    }
}
