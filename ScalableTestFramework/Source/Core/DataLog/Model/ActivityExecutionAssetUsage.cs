using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// An asset used during an <see cref="ActivityExecution" />.
    /// </summary>
    [DebuggerDisplay("{AssetId,nq}")]
    public sealed class ActivityExecutionAssetUsage
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution asset usage.
        /// </summary>
        public Guid ActivityExecutionAssetUsageId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed the activity using this asset.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ActivityExecution" /> that used this asset.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the asset.
        /// </summary>
        public string AssetId { get; set; }
    }
}
