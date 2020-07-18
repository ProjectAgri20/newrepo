using System;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// An owner of a <see cref="License" />.
    /// </summary>
    public sealed class LicenseOwner
    {
        /// <summary>
        /// Gets or sets the unique identifier for the license.
        /// </summary>
        public Guid LicenseId { get; set; }

        /// <summary>
        /// Gets or sets the email contact for the owner.
        /// </summary>
        public string Contact { get; set; }
    }
}
