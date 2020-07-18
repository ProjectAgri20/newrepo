using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    [DataContract]
    public class FirmwareData //: IComparer<FirmwareData>
    {
        [DataMember]
        public string FWModelName { get; set; }

        [DataMember]
        public string FWBundleVersion { get; set; }

        [DataMember]
        public string FirmwareRevision { get; set; }

        [DataMember]
        public string FirmwareDateCode { get; set; }
        [DataMember]
        public string ProductFamily { get; set; }
        [DataMember]
        public int FlashTimeOutPeriod { get; set; }


        public FirmwareData()
        {
            FWModelName = string.Empty;
            FWBundleVersion = string.Empty;
            FirmwareRevision = string.Empty;
            FirmwareDateCode = string.Empty;
            ProductFamily = string.Empty;
            FlashTimeOutPeriod = int.MinValue;
        }

        //public int Compare(FirmwareData x, FirmwareData y)
        //{
        //    bool equal = true;

        //    equal &= x.FirmwareDateCode == y.FirmwareDateCode;
        //    equal &= x.FirmwareRevision == y.FirmwareRevision;
        //    equal &= x.FWModelName == y.FWModelName;
        //    equal &= x.FWBundleVersion == y.FWBundleVersion;


        //    return equal ? 0 : -1;
        //}
    }
}
