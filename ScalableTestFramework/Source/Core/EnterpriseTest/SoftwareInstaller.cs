using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A software installer that can be configured for execution before a test run.
    /// </summary>
    [DebuggerDisplay("{Description,nq}")]
    public sealed class SoftwareInstaller
    {
        /// <summary>
        /// Gets or sets the unique identifier for the software installer.
        /// </summary>
        public Guid SoftwareInstallerId { get; set; }

        /// <summary>
        /// Gets or sets the installer description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the file path where the installer is located.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the command-line arguments to pass to the installer.
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets the reboot setting.
        /// </summary>
        public string RebootSetting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer directory should be copied locally for execution.
        /// </summary>
        public bool CopyDirectory { get; set; }
    }
}
