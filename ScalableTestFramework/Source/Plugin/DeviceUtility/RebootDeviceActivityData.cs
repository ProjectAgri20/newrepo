using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.DeviceUtility
{
    [DataContract]
    public class RebootDeviceActivityData
    {
        [DataMember]
        public DeviceUtilityAction DeviceUtilityAction { get; set; }

        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public JobMediaModeDesired JobMediaMode { get; set; }

        [DataMember]
        public bool ShouldWaitForReady { get; set; }

        public RebootDeviceActivityData()
        {
            DeviceUtilityAction = DeviceUtilityAction.Reboot;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            ShouldWaitForReady = true;
            JobMediaMode = JobMediaModeDesired.Preserve;
        }

        public string ToActivityName() => "RebootDevice";
    }

    public enum JobMediaModeDesired
    {
        Preserve,
        Paper,
        Paperless,
    }

    public enum DeviceUtilityAction
    {
        Reboot,
    }
}