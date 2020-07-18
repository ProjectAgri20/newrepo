using System.Collections.Generic;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public static class CtcMetadataConverter
    {
        private static List<IPluginMetadataConverter> _converters = new List<IPluginMetadataConverter>
        {
            new CtcMetadataConverter1_1()
        };

        public static IEnumerable<IPluginMetadataConverter> Converters => _converters;

        private class CtcMetadataConverter1_1 : IPluginMetadataConverter
        {
            public string OldVersion => "1.0";

            public string NewVersion => "1.1";

            public XElement Convert(XElement xml)
            {
                string rawXml = xml.ToString();
                string newXml = rawXml.Replace("Plugin.CtcBase", "PluginSupport.Connectivity");
                return XElement.Parse(newXml);
            }
        }
    }
}
