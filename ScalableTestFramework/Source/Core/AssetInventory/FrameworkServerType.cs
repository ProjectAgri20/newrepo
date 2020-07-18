using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A descriptive type applied to a <see cref="FrameworkServer" />.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class FrameworkServerType
    {
        /// <summary>
        /// Gets or sets the unique identifier for the server type.
        /// </summary>
        public Guid FrameworkServerTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the server type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the server type.
        /// </summary>
        public string Description { get; set; }
    }
}
