using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    [DataContract]
    internal class PrintQueueResponse
    {
        [DataMember]
        public Parent parent { get; set; }

        [DataMember]
        public TitanDocument[] data { get; set; }

        [DataMember]
        public string overflow { get; set; }
    }
}