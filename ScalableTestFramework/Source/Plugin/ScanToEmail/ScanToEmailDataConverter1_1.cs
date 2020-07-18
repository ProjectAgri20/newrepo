using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    public class ScanToEmailDataConverter1_1 : PluginDataConverter, IPluginMetadataConverter
    {

        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            Version v4_10 = new Version("4.10.0.0");
            Version v4_12 = new Version("4.12.0.0");

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            //Determine what data version we are working with.
            Version version = ParseVersionFromMetadata(xml);

            ScanToEmailData resultData = null;

            if (version < v4_10)
            {
                //Convert old metadata to new schema.
                resultData = new ScanToEmailData()
                {
                    AddressSource = GetValue(xml, "AddressSource"),
                    AutomationPause = GetTimeSpan(xml, "AutomationPause"),
                    DigitalSendServer = GetValue(xml, "DigitalSendServer"),
                    EmailAddress = GetValue(xml, "EmailAddress"),
                    ImagePreviewOptions = GetInt(xml, "ImagePreviewOptions"),
                    LaunchQuicksetFromApp = GetBool(xml, "LaunchQuicksetFromApp"),
                    QuickSetName = GetValue(xml, "QuickSetName"),
                    UseOcr = GetBool(xml, "UseOcr"),
                    UseQuickset = GetBool(xml, "UseQuickset")
                };
            }
            else if (version >= v4_10 && version < v4_12)
            {
                //Deserialize what is there.  It's possible Some ScanOptions were saved.
                resultData = Serializer.Deserialize<ScanToEmailData>(xml);
            }
            else
            {
                //No Conversion necessary
                return xml;
            }

            //Only update these next properties if they are found in the metadata root.
            if (Exists(xml, "LockTimeouts", true))
            {
                resultData.ScanOptions.LockTimeouts = GetLockTimeoutData(rootNS, xml);
            }
            if (Exists(xml, "PageCount", true))
            {
                resultData.ScanOptions.PageCount = GetInt(xml, "PageCount");
            }
            if (Exists(xml, "UseAdf", true))
            {
                resultData.ScanOptions.UseAdf = GetBool(xml, "UseAdf");
            }

            return Serializer.Serialize(resultData);
        }

        /// <summary>
        /// Assembly attribute value should look something like this:
        /// "Plugin.ScanToEmail, Version=4.9.1.0, Culture=neutral, PublicKeyToken=null"
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Version ParseVersionFromMetadata(XElement element)
        {
            XAttribute attribute = GetAttribute(element, "Assembly");
            if (attribute != null)
            {
                string[] parsed = attribute.Value.Split(new char[] { ',', '=' });
                return new Version(parsed[2]);
            }

            return Environment.Version;
        }

    }

}
