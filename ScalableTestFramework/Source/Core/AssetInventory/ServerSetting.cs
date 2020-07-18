using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A configuration setting for a <see cref="FrameworkServer" />.
    /// </summary>
    [DebuggerDisplay("{Name,nq} = {Value}")]
    public sealed class ServerSetting
    {
        /// <summary>
        /// Gets or sets the ID of the server.
        /// </summary>
        public Guid FrameworkServerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the setting.
        /// </summary>
        public string Description { get; set; }
    }
}
