using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Vpn
{
    [DataContract]
    public class VpnActivityData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Connect { get; set; }

        public VpnActivityData()
        {
            Connect = true;
        }
    }

    public class VpnConfiguration
    {
        public string Name { get; set; }
        public string ServerIp { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }


}
