using System.Collections.ObjectModel;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Information read from a driver INF file.
    /// </summary>
    public class DriverInf
    {
        /// <summary>
        /// Gets the INF file location.
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// Gets or sets the INF driver class.
        /// </summary>
        public string DriverClass { get; set; }

        /// <summary>
        /// Gets the set of drivers contained in this INF.
        /// </summary>
        public Collection<DriverDetails> Drivers { get; } = new Collection<DriverDetails>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverInf" /> class.
        /// </summary>
        /// <param name="location">The INF file location.</param>
        public DriverInf(string location)
        {
            Location = location;
        }
    }
}
