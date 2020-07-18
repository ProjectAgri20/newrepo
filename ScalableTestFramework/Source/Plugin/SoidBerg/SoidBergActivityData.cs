using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.SoidBerg
{
    public enum SnmpCommandTypes
    {
        Get,
        GetNext,
        Set

    }

    [DataContract]
    internal class SoidBergActivityData
    {
        [DataMember]
        public string SnmpOid { get; set; }

        [DataMember]
        public SnmpCommandTypes SnmpCommand { get; set; }


        [DataMember]
        public string SnmpSetValue { get; set; }
    }

    public class Oids
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsInteger { get; set; }

        public SnmpCommandTypes CommandType { get; set; }

        public string Comment { get; set; }
    }
}