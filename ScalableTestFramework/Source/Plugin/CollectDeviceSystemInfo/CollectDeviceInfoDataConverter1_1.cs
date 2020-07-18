using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;


namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// Changed the namespace in CollectDeviceSystemInfoActivityData.cs.  This converter handles the update from the old namespace to the new one.
    /// No other changes were made to the Activity Data (metadata).
    /// </summary>
    public class CollectDeviceInfoDataConverter1_1 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        /// <summary>
        /// Converts the metadata from version 1.0 to 1.1.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public XElement Convert(XElement xml)
        {
            SerializerProxyMap proxyMap = new SerializerProxyMap(typeof(CollectDeviceSystemInfoActivityData),
                                                  "Plugin.CollectDeviceSystemInfo",
                                                  "Plugin.CollectDeviceSystemInfo",
                                                  "CollectDeviceSystemInfoActivityData");

            CollectDeviceSystemInfoActivityData converted = Serializer.Deserialize<CollectDeviceSystemInfoActivityData>(xml, new[] { proxyMap });
            return Serializer.Serialize(converted);
        }

    }
}
