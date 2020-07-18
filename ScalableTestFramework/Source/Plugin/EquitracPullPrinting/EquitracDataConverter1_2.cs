using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    public class EquitracDataConverter1_2 : PluginDataConverter, IPluginMetadataConverter
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
            // Create Proxy object
            EquitracActivityData1_1 proxyData = Serializer.Deserialize<EquitracActivityData1_1>(xml, Maps);

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // Create the latest ActivityData
            EquitracActivityData resultData = new EquitracActivityData()
            {
                DocumentProcessAction = proxyData.DocumentProcessAction,
                LockTimeouts = proxyData.LockTimeouts,
                DeviceMemoryProfilerConfig = proxyData.DeviceMemoryProfilerConfig,
                EquitracAuthentication = proxyData.EquitracAuthentication,
                EquitracServer = proxyData.EquitracServer,
                NumberOfCopies = proxyData.NumberOfCopies,
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
        /// <param name="targetConfig"></param>
        private void ConvertPrintingTask(XNamespace rootNS, XElement xml, EquitracActivityData newActivityData)
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
                yield return new SerializerProxyMap(typeof(EquitracActivityData1_1), "Plugin.EquitracPullPrinting", "HP.ScalableTest.Plugin.EquitracPullPrinting", "EquitracActivityData");
            }
        }
    }

    /// <summary>
    /// Proxy class for the ActivityData structure used in the previous build.
    /// </summary>
    public class EquitracActivityData1_1
    {
        [DataMember]
        public EquitracPullPrintAction DocumentProcessAction { get; set; }

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
        public bool EquitracAuthentication { get; set; }
        [DataMember]
        public string EquitracServer { get; set; }

        [DataMember]
        public int NumberOfCopies { get; set; }

        [DataMember]
        public bool SelectAll { get; set; }

        public EquitracActivityData1_1()
        {
            EquitracAuthentication = false;
            EquitracServer = string.Empty;
            NumberOfCopies = 1;
            SelectAll = false;
            DocumentProcessAction = EquitracPullPrintAction.Print;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            //PrintingTaskData = new PrintingTaskConfigData();
        }

    }
}
