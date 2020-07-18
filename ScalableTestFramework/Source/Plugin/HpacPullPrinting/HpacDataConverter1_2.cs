using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    public class HpacDataConverter1_2 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.1";
        public string NewVersion => "1.2";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            // Create Proxy object, serialize what we can
            HpacActivityData1_1 proxyData = Serializer.Deserialize<HpacActivityData1_1>(xml, Maps);

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // Create the latest ActivityData
            HpacActivityData resultData = new HpacActivityData()
            {
                AuthProvider = AuthenticationProvider.HpacIrm,
                DocumentProcessAction = proxyData.DocumentProcessAction,
                LockTimeouts = proxyData.LockTimeouts,
                DeviceMemoryProfilerConfig = proxyData.DeviceMemoryProfilerConfig,
                HpacAuthentication = proxyData.HpacAuthentication,
                SelectAll = proxyData.SelectAll
            };

            ConvertPrintingTask(rootNS, xml, resultData);

            return Serializer.Serialize(resultData);
        }

        /// <summary>
        /// Grab values out of PrintingTaskData without the need for the class to serialize/de-serialize.
        /// </summary>
        /// <param name="rootNS"></param>
        /// <param name="xml"></param>
        /// <param name="newActivityData"></param>
        private void ConvertPrintingTask(XNamespace rootNS, XElement xml, HpacActivityData newActivityData)
        {
            XElement prinTaskElement = xml.Element(rootNS + "PrintingTaskData");
            XNamespace prinTaskNS = GetNamespace(prinTaskElement);

            try
            {
                // Set native properties
                newActivityData.ShuffleDocuments = GetBool(prinTaskElement, "ShuffleDocuments");

                // Only set if a real value is there.
                int delayVal = GetInt(prinTaskElement, "DelayAfterPrint");
                if (delayVal > 0)
                {
                    newActivityData.DelayAfterPrint = delayVal;
                }
            }
            catch (ArgumentNullException)
            { }
        }

        private static IEnumerable<SerializerProxyMap> Maps
        {
            get
            {
                yield return new SerializerProxyMap(typeof(HpacActivityData1_1), "Plugin.HpacPullPrinting", "HP.ScalableTest.Plugin.HpacPullPrinting", "HpacActivityData");
            }
        }
    }

    /// <summary>
    /// The Proxy class that receives the serialization of the current outdated metadata.
    /// </summary>
    [DataContract]
    internal class HpacActivityData1_1
    {
        [DataMember]
        public HpacPullPrintAction DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public virtual DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        //[DataMember]
        //public PrintingTaskConfigData PrintingTaskData { get; set; }

        [DataMember]
        public bool HpacAuthentication { get; set; }

        [DataMember]
        public bool SelectAll { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacActivityData1_1"/> class.
        /// </summary>
        public HpacActivityData1_1()
        {
            DocumentProcessAction = HpacPullPrintAction.PrintAll;
            HpacAuthentication = false;
            SelectAll = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            //PrintingTaskData = new PrintingTaskConfigData();
        }
    }

}
