using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;


namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    public class SafeComDataConverter1_2 : PluginDataConverter, IPluginMetadataConverter
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
            SafeComActivityData1_1 proxyData = Serializer.Deserialize<SafeComActivityData1_1>(xml, Maps);

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // Create the latest ActivityData
            SafeComActivityData resultData = new SafeComActivityData()
            {
                DocumentProcessAction = proxyData.DocumentProcessAction,
                LockTimeouts = proxyData.LockTimeouts,
                DeviceMemoryProfilerConfig = proxyData.DeviceMemoryProfilerConfig,
                SafeComAuthentication = proxyData.SafeComAuthentication,
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
        /// <param name="newActivityData"></param>
        private void ConvertPrintingTask(XNamespace rootNS, XElement xml, SafeComActivityData newActivityData)
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
                yield return new SerializerProxyMap(typeof(SafeComActivityData1_1), "Plugin.SafeComPullPrinting", "HP.ScalableTest.Plugin.SafeComPullPrinting", "SafeComActivityData");
            }
        }
    }

    [DataContract]
    internal class SafeComActivityData1_1
    {
        private int _numOfCopies = 1;

        [DataMember]
        public SafeComPullPrintAction DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public bool SafeComAuthentication { get; set; }

        [DataMember]
        public DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        [DataMember]
        public int NumberOfCopies
        {
            get { return _numOfCopies; }
            set { _numOfCopies = Math.Max(value, 1); }
        }
        [DataMember]
        public bool SelectAll { get; set; }

        //[DataMember]
        //public PrintingTaskConfigData PrintingTaskData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComActivityData1_1"/> class.
        /// </summary>
        public SafeComActivityData1_1()
        {
            DocumentProcessAction = SafeComPullPrintAction.Print;
            SafeComAuthentication = false;
            SelectAll = false;
            NumberOfCopies = 1; //Default to 1.  Will throw if 0.
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            //PrintingTaskData = new PrintingTaskConfigData();
        }
    }
}
