using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    internal sealed class DirtyDeviceDataConverter1_1 : IPluginMetadataConverter
    {
        public string OldVersion => "1.0";

        public string NewVersion => "1.1";

        public XElement Convert(XElement xml)
        {
            SerializerProxyMap map = new SerializerProxyMap(typeof(DigitalSendOutputLocation),
                                                            "HP.STF.Data.EnterpriseTest.Model",
                                                            "HP.ScalableTest.Data.EnterpriseTest.Model",
                                                            "DigitalSendOutputLocation");

            var data = Serializer.Deserialize<DirtyDeviceActivityData>(xml, new[] { map });

            return Serializer.Serialize(data);
        }
    }
}
