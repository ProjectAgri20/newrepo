using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A software license tracked by Asset Inventory.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Gets or sets the unique identifier for the license.
        /// </summary>
        public Guid LicenseId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated <see cref="FrameworkServer" />.
        /// </summary>
        public Guid FrameworkServerId { get; set; }

        /// <summary>
        /// Gets or sets the solution this license is for.
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// Gets or sets the solution version.
        /// </summary>
        public string SolutionVersion { get; set; }

        /// <summary>
        /// Gets or sets the installation key.
        /// </summary>
        public string InstallationKey { get; set; }

        /// <summary>
        /// Gets or sets the number of license seats.
        /// </summary>
        public int Seats { get; set; }

        /// <summary>
        /// Gets or sets the date when the license expires.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the number of days before expiration a notice should be sent.
        /// </summary>
        public int ExpirationNoticeDays { get; set; }

        /// <summary>
        /// Gets or sets the time when a renewal request was sent.
        /// </summary>
        public DateTime? RequestSentDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="LicenseOwner" /> objects for this license.
        /// </summary>
        public virtual ICollection<LicenseOwner> Owners { get; set; } = new HashSet<LicenseOwner>();
    }
}
