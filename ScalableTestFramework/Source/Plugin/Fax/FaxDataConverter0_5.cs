using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.Fax
{
    internal class FaxDataConverter0_5 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "0.5";
        public string NewVersion => "1.1";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // Create the latest ActivityData
            FaxActivityData resultData = new FaxActivityData()
            {
                AutomationPause = GetTimeSpan(xml, "AutomationPause"),
                DigitalSendServer = GetValue(xml, "DigitalSendServer"),
                EnableNotification = GetBool(xml, "EnableNotification"),
                FaxType = FaxConfiguration.LANFax,
                FaxOperation = FaxTask.SendFax,
                NotificationEmail = GetValue(xml, "NotificationEmail")
            };

            resultData.ScanOptions.LockTimeouts = GetLockTimeoutData(rootNS, xml);
            resultData.ScanOptions.PageCount = GetInt(xml, "PageCount");
            resultData.ScanOptions.UseAdf = GetBool(xml, "UseAdf");

            return Serializer.Serialize(resultData);
        }

    }
}
