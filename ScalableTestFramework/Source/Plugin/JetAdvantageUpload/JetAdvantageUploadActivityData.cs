using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    /// <summary>
    /// Contains data needed to execute the activity through the plugin.
    /// </summary>

    [DataContract]
    public class JetAdvantageUploadActivityData
    {
        /// <summary>
        /// Gets or sets whether the list of documents to upload is shuffled prior to each run.
        /// </summary>
        [DataMember]
        public bool Shuffle { get; set; }

        [DataMember]
        public string LoginId { get; set; }

        [DataMember]
        public string LoginPassword { get; set; }

        [DataMember]
        public string StackURL { get; set; }

        [DataMember]
        public string StackProxy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageUploadActivityData"/> class.
        /// </summary>
        public JetAdvantageUploadActivityData()
        {
            Shuffle = true;

            LoginId = string.Empty;
            LoginPassword = string.Empty;
            StackURL = "https://pp.hpondemand.com";
            StackProxy = "https://mfp.hpbizapps.com";
        }
    }
}
