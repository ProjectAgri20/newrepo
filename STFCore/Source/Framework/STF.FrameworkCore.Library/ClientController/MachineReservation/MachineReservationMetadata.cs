using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Class used to hold the Installer Package Id for the machine reservation
    /// </summary>
    [Serializable]
    public class MachineReservationMetadata
    {
        /// <summary>
        /// Gets or sets the package unique identifier.
        /// </summary>
        public Guid PackageId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineReservationMetadata"/> class.
        /// </summary>
        public MachineReservationMetadata()
        {
            PackageId = Guid.Empty;
        }
    }
}
