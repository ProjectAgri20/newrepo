using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.UserIntervention
{
    /// <summary>
    /// Holds the Metadata  between Configuration and Execution Control
    /// </summary>
    [DataContract]
    public class UserInterventionActivityData
    {
        [DataMember]
        public string AlertMessage { get; set; }

        /// <summary>
        /// Intialize the new instance of the UserInterventionActivityData class
        /// </summary>
        public UserInterventionActivityData()
        {
            AlertMessage = string.Empty;
        }
    }
}
