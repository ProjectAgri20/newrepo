using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Information about a type of <see cref="VirtualResourceMetadata" /> that can be created by the framework.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class MetadataType
    {
        /// <summary>
        /// Gets or sets the virtual resource metadata name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the plugin assembly for this metadata type.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the title to use when displaying the virtual resource metadata.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the group to use when displaying the virtual resource metadata.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the icon to use for this metadata type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] Icon { get; set; }
    }
}
