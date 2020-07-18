using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.Kiosk.Controls;
using HP.ScalableTest.Plugin.Kiosk.Options;
using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Kiosk
{
    [DataContract]
    public class KioskActivityData
    {
        /// <summary>
        /// Gets or sets the Auth type: Login, Card
        /// </summary>
        [DataMember]
        public KioskAuthType AuthType { get; set; }

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
        /// Gets or sets the Copy Options
        /// </summary>
        [DataMember]
        public KioskCopyOptions CopyOptions { get; set; }

        /// <summary>
        /// Gets or sets the Print Options
        /// </summary>
        [DataMember]
        public KioskPrintOptions PrintOptions { get; set; }

        /// <summary>
        /// Gets or sets the Scan Options
        /// </summary>
        [DataMember]
        public KioskScanOptions ScanOptions { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the Job type: copy, print, scan
        /// </summary>
        public KioskJobType JobType { get; set; }

        public KioskActivityData()
        {
            //값 초기화 부분
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            CopyOptions = new KioskCopyOptions();
            PrintOptions = new KioskPrintOptions();
            ScanOptions = new KioskScanOptions();            
        }
    }
}
