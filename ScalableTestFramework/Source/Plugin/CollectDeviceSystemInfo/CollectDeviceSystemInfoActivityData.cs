using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// Contains data needed to execute the PluginCollectDeviceSystemInfo activity.
    /// </summary>
    [DataContract]
    internal class CollectDeviceSystemInfoActivityData
    {
        /// <summary>
        /// Initializes a new instance of the PluginCollectDeviceSystemInfoActivityData class.
        /// </summary>
        public CollectDeviceSystemInfoActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

    }
}
