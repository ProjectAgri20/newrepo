using System;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Contains document usage data for a <see cref="VirtualResourceMetadata" /> object.
    /// </summary>
    public sealed class VirtualResourceMetadataDocumentUsage
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated <see cref="VirtualResourceMetadata" />.
        /// </summary>
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// Gets or sets an XML representation of the <see cref="Framework.Documents.DocumentSelectionData" />.
        /// </summary>
        public string DocumentSelectionData { get; set; }
    }
}
