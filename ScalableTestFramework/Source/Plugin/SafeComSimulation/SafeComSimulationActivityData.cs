using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    [DataContract]
    public class SafeComSimulationActivityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComSimulationActivityData"/> class.
        /// </summary>
        public SafeComSimulationActivityData()
        {
            SafeComAuthenticationMode = SafeComAuthenticationMode.NameAndPassword;
            PullAllDocuments = false;
        }

        /// <summary>
        /// Gets or sets whether or not all of the print jobs should be pulled from the queue during each activity. Jobs are not retained after being pulled.
        /// </summary>
        [DataMember]
        public bool PullAllDocuments { get; set; }

        /// <summary>
        /// Gets or sets the SafeCom authentication mode.
        /// </summary>
        [DataMember]
        public SafeComAuthenticationMode SafeComAuthenticationMode { get; set; }

        /// <summary>
        /// The number from the proximity card.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the asset MAC address.
        /// </summary>
        [DataMember]
        public string AssetMacAddress { get; set; }
    }
}
