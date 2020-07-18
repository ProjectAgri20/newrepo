using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.JetAdvantage
{
    /// <summary>
    /// Contains data needed to execute a JetAdvantage (Titan) activity.
    /// </summary>
    [DataContract]
    public class JetAdvantageActivityData
    {
        /// <summary>
        /// Gets or sets CkBox indicating whether to delete after printing the document
        /// </summary>
        /// <value><c>true</c>if [Delete document after print]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool DeleteAfterPrint { get; set; }

        /// <summary>
        /// Gets or sets the JetAdvantage solution.
        /// </summary>
        /// <value>The JetAdvantage solution</value>
        // [DataMember]
        // public JetAdvantageSolution JetAdvantageSolution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to print all documents.
        /// </summary>
        /// <value><c>true</c> if [print all documents]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool PrintAllDocuments { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>The admin password.</value>
        [DataMember]
        public string AdminPassword { get; set; }

        /// <summary>
        /// Gets or sets the JetAdvantage login Id. (Email address)
        /// </summary>
        /// <value>The JetAdvantage Login ID</value>
        [DataMember]
        public string JetAdvantageLoginId { get; set; }

        /// <summary>
        /// Gets or sets the JetAdvantage password
        /// </summary>
        /// <value>The JetAdvantage password</value>
        [DataMember]
        public string JetAdvantagePassword { get; set; }

        /// <summary>
        /// Gets or sets the JetAdvantage Login PIN
        /// </summary>
        /// <value>The JetAdvantage Login PIN</value>
        [DataMember]
        public string JetAdvantageLoginPin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use login PIN.
        /// </summary>
        /// <value><c>true</c> if [authenticate with Login Pin]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseLoginPin { get; set; }

        /// <summary>
        /// Initializes class members
        /// </summary>
        public JetAdvantageActivityData()
        {
            DeleteAfterPrint = true;
            //JetAdvantageSolution = JetAdvantageSolution.AtlasPullPrint;
            PrintAllDocuments = true;
            AdminPassword = "admin";
            JetAdvantageLoginId = "";
            JetAdvantagePassword = "";
            UseLoginPin = false;
            JetAdvantageLoginPin = "";
        }
    }
}
