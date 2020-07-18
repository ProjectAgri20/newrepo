using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a mobile device used in an enterprise test.
    /// </summary>
    [DataContract]
    [AssetHost("MobileDeviceHost")]
    public class MobileDeviceDetail : AssetDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceDetail" /> class.
        /// </summary>
        public MobileDeviceDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceDetail" /> class.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="availabilityStartTime"></param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        public MobileDeviceDetail(string assetId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(assetId, availabilityStartTime, availabilityEndTime)
        {
        }
    }
}
