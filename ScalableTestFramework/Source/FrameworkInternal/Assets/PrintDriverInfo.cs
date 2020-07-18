using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a print driver.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{PackageName,nq} [{DriverName,nq}]")]
    public class PrintDriverInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _printDriverId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _driverName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _packageName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _printProcessor;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _infX86;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _infX64;

        /// <summary>
        /// Gets the unique identifier for the print driver.
        /// </summary>
        public Guid PrintDriverId => _printDriverId;

        /// <summary>
        /// Gets the print driver name.
        /// </summary>
        public string DriverName => _driverName;

        /// <summary>
        /// Gets the print driver package name.
        /// </summary>
        public string PackageName => _packageName;

        /// <summary>
        /// Gets the print processor.
        /// </summary>
        public string PrintProcessor => _printProcessor;

        /// <summary>
        /// Gets the location for the x86 INF.
        /// </summary>
        public string InfX86 => _infX86;

        /// <summary>
        /// Gets the location for the x64 INF.
        /// </summary>
        public string InfX64 => _infX64;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverInfo" /> class.
        /// </summary>
        /// <param name="printDriverId">The print driver identifier.</param>
        /// <param name="driverName">The driver name.</param>
        /// <param name="packageName">The package name.</param>
        /// <param name="printProcessor">The print processor.</param>
        /// <param name="infX86">The location of the x86 INF file.</param>
        /// <param name="infX64">The location of the x64 INF file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="driverName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="driverName" /> is an empty string.</exception>
        public PrintDriverInfo(Guid printDriverId, string driverName, string packageName, string printProcessor, string infX86, string infX64)
        {
            if (driverName == null)
            {
                throw new ArgumentNullException(nameof(driverName));
            }

            if (string.IsNullOrWhiteSpace(driverName))
            {
                throw new ArgumentException("Driver name cannot be an empty string.", nameof(driverName));
            }

            _printDriverId = printDriverId;
            _driverName = driverName;
            _packageName = packageName;
            _printProcessor = printProcessor;
            _infX86 = infX86;
            _infX64 = infX64;
        }
    }
}
