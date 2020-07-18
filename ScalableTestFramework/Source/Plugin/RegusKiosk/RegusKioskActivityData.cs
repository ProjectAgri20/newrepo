using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using HP.ScalableTest.Plugin.RegusKiosk.Options;
using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.RegusKiosk
{
    [DataContract]
    public class RegusKioskActivityData
    {
        /// <summary>
        /// Gets or sets the Auth type: Login, Card
        /// </summary>
        [DataMember]
        public RegusKioskAuthType AuthType { get; set; }

        /// <summary>
        /// Gets or sets the LockTimeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the World key PIN
        /// </summary>
        [DataMember]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the Copy Options
        /// </summary>
        [DataMember]
        public RegusKioskCopyOptions CopyOptions { get; set; }

        /// <summary>
        /// Gets or sets the Print Options
        /// </summary>
        [DataMember]
        public RegusKioskPrintOptions PrintOptions { get; set; }

        /// <summary>
        /// Gets or sets the Scan Options
        /// </summary>
        [DataMember]
        public RegusKioskScanOptions ScanOptions { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the Job type: copy, print, scan
        /// </summary>
        public RegusKioskJobType JobType { get; set; }

        public RegusKioskActivityData()
        {
            //값 초기화 부분
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            CopyOptions = new RegusKioskCopyOptions();
            PrintOptions = new RegusKioskPrintOptions();
            ScanOptions = new RegusKioskScanOptions();            
        }
    }
}
