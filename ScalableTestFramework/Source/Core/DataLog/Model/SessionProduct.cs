using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A product associated with a test session.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class SessionProduct
    {
        /// <summary>
        /// Gets or sets the identifier for the session associated with the product.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated product.
        /// </summary>
        public Guid EnterpriseTestAssociatedProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product vendor.
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or sets the product version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this association has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
