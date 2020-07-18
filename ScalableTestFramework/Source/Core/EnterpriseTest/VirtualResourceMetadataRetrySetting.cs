using System;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Defines retry behavior for a <see cref="VirtualResourceMetadata" /> object.
    /// </summary>
    public sealed class VirtualResourceMetadataRetrySetting
    {
        /// <summary>
        /// Gets or sets the unique identifier for the setting.
        /// </summary>
        public Guid VirtualResourceMetadataRetrySettingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated <see cref="VirtualResourceMetadata" />.
        /// </summary>
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// Gets or sets the state this retry setting applies to.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the action defined for this retry setting.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of retries.
        /// </summary>
        public int RetryLimit { get; set; }

        /// <summary>
        /// Gets or sets the delay between retries, in seconds.
        /// </summary>
        public int RetryDelay { get; set; }

        /// <summary>
        /// Gets or sets the action to take when the retry limit is exceeded.
        /// </summary>
        public string LimitExceededAction { get; set; }
    }
}
