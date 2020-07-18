using System.Diagnostics;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Detailed information about a print driver extracted from an INF file or the registry.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public class DriverDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverDetails" /> class.
        /// </summary>
        public DriverDetails()
        {
            // Sometimes the .inf file does not have the PrintProcessor property.
            // When that is the case, using WinPrint as the default.
            PrintProcessor = "WinPrint";
        }

        /// <summary>
        /// Gets or sets the name of the driver.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DriverArchitecture" /> associated with this driver.
        /// </summary>
        public DriverArchitecture Architecture { get; set; }

        /// <summary>
        /// Gets or sets the driver version.
        /// </summary>
        public DriverVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the driver INF file path.
        /// </summary>
        public string InfPath { get; set; }

        /// <summary>
        /// Gets or sets the driver file name.
        /// </summary>
        public string DriverFile { get; set; }

        /// <summary>
        /// Gets or sets the driver config file name.
        /// </summary>
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets the driver help file name.
        /// </summary>
        public string HelpFile { get; set; }

        /// <summary>
        /// Gets or sets the driver data file name.
        /// </summary>
        public string DataFile { get; set; }

        /// <summary>
        /// Gets or sets the print processor for this driver.
        /// </summary>
        public string PrintProcessor { get; set; }

        /// <summary>
        /// Gets or sets the language monitor for the driver.
        /// </summary>
        public string Monitor { get; set; }

        /// <summary>
        /// Gets or sets the provider for the driver.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the driver date.
        /// </summary>
        public string DriverDate { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Name}, {Architecture}";
        }
    }
}
