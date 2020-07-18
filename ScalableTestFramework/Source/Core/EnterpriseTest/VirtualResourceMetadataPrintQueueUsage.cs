using System;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Contains print queue usage data for a <see cref="VirtualResourceMetadata" /> object.
    /// </summary>
    public sealed class VirtualResourceMetadataPrintQueueUsage
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated <see cref="VirtualResourceMetadata" />.
        /// </summary>
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// Gets or sets an XML representation of the <see cref="Framework.Assets.PrintQueueSelectionData" />.
        /// </summary>
        public string PrintQueueSelectionData { get; set; }
    }
}
