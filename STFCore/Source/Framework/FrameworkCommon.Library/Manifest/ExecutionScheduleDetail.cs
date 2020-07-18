using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Defines details for the scheduled execution of a defined resource
    /// </summary>
    [DataContract]
    public class ExecutionScheduleDetail
    {
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        [DataMember]
        public ExecutionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        [DataMember]
        public int Hours { get; set; }

        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        [DataMember]
        public int Minutes { get; set; }
    }
}