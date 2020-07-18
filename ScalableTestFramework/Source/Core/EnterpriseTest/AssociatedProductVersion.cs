using System;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A association between an <see cref="EnterpriseScenario" /> and a specific version of an <see cref="AssociatedProduct" />.
    /// </summary>
    public class AssociatedProductVersion
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated product.
        /// </summary>
        public Guid AssociatedProductId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the enterprise scenario.
        /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the version of the associated product.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AssociatedProductVersion" /> is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EnterpriseTest.AssociatedProduct" />.
        /// </summary>
        public virtual AssociatedProduct AssociatedProduct { get; set; }
    }
}
