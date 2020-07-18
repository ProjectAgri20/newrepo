using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    [DataContract]
    public class HpacSimulationActivityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HpacSimulationActivityData"/> class.
        /// </summary>
        public HpacSimulationActivityData()
        {
            HpacAuthenticationMode = HpacAuthenticationMode.DomainCredentials;
            DeleteAfterPull = true; // 6/10/2014 - The option to retain print jobs has been taken out in release 3.6 because it is seemingly impossible to validate if the jobs have actually been printed
        }

        /// <summary>
        /// Gets or sets the hpac authentication mode.
        /// </summary>
        /// <value>
        /// The hpac authentication mode.
        /// </value>
        [DataMember]
        public HpacAuthenticationMode HpacAuthenticationMode { get; set; }

        /// <summary>
        /// Gets or sets whether or not the jobs should be removed from the queue after being pulled
        /// 6/10/2014 - The option to retain print jobs has been taken out in release 3.6 because it is seemingly impossible to validate if the jobs have actually been printed
        /// </summary>
        [DataMember]
        public bool DeleteAfterPull { get; set; }

        /// <summary>
        /// Gets or sets whether or not all of the print jobs should be pulled from the queue during each activity. Jobs are not retained after being printed.
        /// </summary>
        [DataMember]
        public bool PullAllDocuments { get; set; }


    }
}
