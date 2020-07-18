using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    class EquitracDataConverter1_3 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.2";
        public string NewVersion => "1.3";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            EquitracActivityData data = Serializer.Deserialize<EquitracActivityData>(xml);

            if (data.AuthProvider.Equals(default(AuthenticationProvider)))
            {
                data.AuthProvider = AuthenticationProvider.Equitrac;
            }

            return Serializer.Serialize(data);
        }

    }

}
