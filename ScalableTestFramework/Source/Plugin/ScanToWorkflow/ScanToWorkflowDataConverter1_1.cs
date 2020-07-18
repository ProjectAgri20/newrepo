using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    internal class ScanToWorkflowDataConverter1_1 : IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        public XElement Convert(XElement xml)
        {
            ScanToWorkflowData data = Serializer.Deserialize<ScanToWorkflowData>(xml);
            data.ApplicationAuthentication = true;
            data.AuthProvider = AuthenticationProvider.Auto;
            return Serializer.Serialize(data);
        }
    }
}
