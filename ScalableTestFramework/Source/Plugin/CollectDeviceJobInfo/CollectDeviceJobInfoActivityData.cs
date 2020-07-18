using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.CollectDeviceJobInfo
{
    /// <summary>
    /// Contains data needed to execute the PluginCollectDeviceJobInfo activity.
    /// </summary>
    [DataContract]
    internal class CollectDeviceJobInfoActivityData
    {
        /// <summary>
        /// Initializes a new instance of the PluginCollectDeviceJobInfoActivityData class.
        /// </summary>
        public CollectDeviceJobInfoActivityData()
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
