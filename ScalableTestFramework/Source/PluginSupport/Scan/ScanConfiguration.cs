using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Configuration data used by a <see cref="ScanActivityManager" />.
    /// </summary>
    public class ScanConfiguration
    {
        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the lock timeouts.
        /// </summary>
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the ADF for simulators.
        /// </summary>
        public bool UseAdf { get; set; }
    }
}
