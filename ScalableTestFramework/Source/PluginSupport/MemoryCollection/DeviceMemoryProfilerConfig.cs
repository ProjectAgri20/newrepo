using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.MemoryCollection
{
    /// <summary>
    /// Memory profiler configuration data
    /// </summary>
    [DataContract]
    public class DeviceMemoryProfilerConfig
    {
        /// <summary>
        /// Gets or sets the target sample time.
        /// </summary>
        /// <value>
        /// The target sample time.
        /// </value>
        [DataMember]
        public TimeSpan TargetSampleTime { get; set; }

        /// <summary>
        /// Gets or sets the target sample count.
        /// </summary>
        /// <value>
        /// The target sample count.
        /// </value>
        [DataMember]
        public int TargetSampleCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [sample at time intervals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sample at time intervals]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool SampleAtTimeIntervals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [sample at count intervals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sample at count intervals]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool SampleAtCountIntervals { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMemoryProfilerConfig"/> class.
        /// </summary>
        public DeviceMemoryProfilerConfig()
        {
            SampleAtCountIntervals = false;
            SampleAtCountIntervals = false;

            TargetSampleTime = new TimeSpan(0, 30, 0);
            TargetSampleCount = 100;
        }
    }
}
