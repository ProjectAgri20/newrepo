using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.TopCat;

namespace HP.ScalableTest.Plugin.TopCat
{
    [DataContract]
    public class TopCatActivityData
    {
        [DataMember]
        public TopCatScript Script { get; set; }

        [DataMember]
        public string SetupFileName { get; set; }

        [DataMember]
        public bool CopyDirectory { get; set; }

        [DataMember]
        public bool RunOnce { get; set; }

        public TopCatActivityData()
        {
            SetupFileName = string.Empty;
        }
    }
}