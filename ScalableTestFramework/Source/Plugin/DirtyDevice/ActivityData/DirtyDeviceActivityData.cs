using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    [DataContract]
    public class DirtyDeviceActivityData
    {
        [DataMember]
        public DirtyDeviceActionFlags DirtyDeviceActionFlags { get; set; }

        [DataMember]
        public DigitalSendOutputFolderActivityData DigitalSend { get; set; }

        [DataMember]
        public QuickSetActivityData Ews { get; set; }

        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        public DirtyDeviceActivityData()
        {
            DirtyDeviceActionFlags = DirtyDeviceActionFlags.All;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(30));
            DigitalSend = new DigitalSendOutputFolderActivityData();
            Ews = new QuickSetActivityData();
        }
    }
}
