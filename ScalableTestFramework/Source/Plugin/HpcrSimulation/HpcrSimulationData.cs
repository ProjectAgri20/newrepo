using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpcrSimulation
{
    /// <summary>
    /// Defines the object that represents all metadata needed for executing
    /// an HPCR (HP Capture and Route) activity within the test framework
    /// </summary>
    [DataContract]
    public class HpcrSimulationData
    {
        /// <summary>
        /// Gets or sets the type of the test.
        /// </summary>
        /// <value>The type of the test.</value>
        [DataMember]
        public HpcrTestType TestType { get; set; }

        [DataMember]
        public string EmailDomain { get; set; }

        [DataMember]
        public SendToDistributionData SendToDistribution { get; set; }

        [DataMember]
        public SendToEmailData SendToEmail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrSimulationData"/> class.
        /// </summary>
        public HpcrSimulationData()
        {
            EmailDomain = "etl.boi.rd.hpicorp.net";
            TestType = HpcrTestType.DeliverToRandomEmails;

            SendToDistribution = new SendToDistributionData();
            SendToEmail = new SendToEmailData();
        }
    }

    [DataContract]
    public class SendToDistributionData
    {
        public SendToDistributionData()
        {
            Originator = string.Empty;
            DistributionTitle = string.Empty;
        }
        [DataMember]
        public string Originator { get; set; }
        [DataMember]
        public string DistributionTitle { get; set; }
    }
    [DataContract]
    public class SendToEmailData
    {

        public SendToEmailData()
        {
            Originator = Constants.CURRENT_USER;
            NumberOfRandomRecipients = 1;
        }
        [DataMember]
        public string Originator { get; set; }
        [DataMember]
        public int NumberOfRandomRecipients { get; set; }
    }

    public enum HpcrTestType
    {
        DeliverToUserDistribution = 0,
        DeliverToRandomEmails,
        ShowUserDistributions,
        ShowUserGroupPolicy,
        ShowUserGroupMembership
    }
}