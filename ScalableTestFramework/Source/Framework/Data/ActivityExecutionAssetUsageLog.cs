using System;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Logs usage of an asset by a plugin activity.
    /// </summary>
    public sealed class ActivityExecutionAssetUsageLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "ActivityExecutionAssetUsage";

        /// <summary>
        /// Gets the asset identifier.
        /// </summary>
        [DataLogProperty]
        public string AssetId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionAssetUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="assetInfo">The <see cref="IAssetInfo" /> for the asset used by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetInfo" /> is null.</exception>
        public ActivityExecutionAssetUsageLog(PluginExecutionData executionData, IAssetInfo assetInfo)
            : base(executionData)
        {
            if (assetInfo == null)
            {
                throw new ArgumentNullException(nameof(assetInfo));
            }

            AssetId = assetInfo.AssetId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionAssetUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="assetId">The unique identifier for the asset used by the activity.</param>
        public ActivityExecutionAssetUsageLog(PluginExecutionData executionData, string assetId)
            : base(executionData)
        {
            AssetId = assetId;
        }
    }
}
