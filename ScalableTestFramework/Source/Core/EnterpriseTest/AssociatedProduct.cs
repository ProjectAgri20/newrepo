using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A product or solution that is tested or used by a virtual resource activity.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class AssociatedProduct
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public Guid AssociatedProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product vendor.
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or sets the metadata types that are associated with this product.
        /// </summary>
        public virtual ICollection<MetadataType> MetadataTypes { get; set; } = new HashSet<MetadataType>();
    }
}
