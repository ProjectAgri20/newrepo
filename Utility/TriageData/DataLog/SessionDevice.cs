using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{


    [Table("SessionDevice")]
    public partial class SessionDevice
    {
        public Guid SessionDeviceId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceId { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(255)]
        public string DeviceName { get; set; }

        [StringLength(50)]
        public string FirmwareRevision { get; set; }

        [StringLength(10)]
        public string FirmwareDatecode { get; set; }

        [StringLength(50)]
        public string FirmwareType { get; set; }

        [StringLength(50)]
        public string ModelNumber { get; set; }

        [StringLength(50)]
        public string NetworkCardModel { get; set; }

        public string NetworkInterfaceVersion { get; set; }

        public virtual SessionSummary SessionSummary { get; set; }
        public string IpAddress { get; set; }
        public static SessionDevice GetBySessionDeviceId(DataLogContext dlc, string sessionId, string deviceId) => (from sd in dlc.SessionDevices where sd.SessionId.Equals(sessionId) && sd.DeviceId.Equals(deviceId) select sd).FirstOrDefault();

        public static List<string> GetProductsBySessionId(DataLogContext dlc, string sessionId) => (from sd in dlc.SessionDevices where sd.SessionId.Equals(sessionId) select sd.ProductName).Distinct().ToList();
    }
}
