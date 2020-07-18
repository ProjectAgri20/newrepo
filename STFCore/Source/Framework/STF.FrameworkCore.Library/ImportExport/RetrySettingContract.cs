using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) of Retry Settings.
    /// </summary>
    [DataContract]
    public class RetrySettingContract
    {
        /// <summary>
        /// The MetadataType Name.
        /// </summary>
        public RetrySettingContract(Guid virtualResourceMetadataId)
        {
            VirtualResourceMetadataId = virtualResourceMetadataId;
        }

        /// <summary>
        /// The MetadataType Name.
        /// </summary>
        [DataMember]
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// The Retry State.
        /// </summary>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        /// The Retry Action.
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// The number of retries to execute before aborting.
        /// </summary>
        [DataMember]
        public int RetryLimit { get; set; }

        /// <summary>
        /// The delay between retries.
        /// </summary>
        [DataMember]
        public int RetryDelay { get; set; }

        /// <summary>
        /// The Action to take when RetryLimit has been exceeded.
        /// </summary>
        [DataMember]
        public string LimitExceededAction { get; set; }
    }

}
