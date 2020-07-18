using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    public class SafeComDataConverter1_1 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.2";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            // Create Proxy object, deserialize what we can.
            SafeComActivityData1_0 proxyData = Serializer.Deserialize<SafeComActivityData1_0>(xml, Maps);

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // Create a new ActivityData
            SafeComActivityData resultData = new SafeComActivityData()
            {
                LockTimeouts = proxyData.LockTimeouts,
                DeviceMemoryProfilerConfig = proxyData.DeviceMemoryProfilerConfig,
                SafeComAuthentication = UseSolutionAuth(rootNS, xml)
            };

            // Set the DocumentProcessAction property
            SetAction(proxyData.DocumentProcessAction, resultData);
            ConvertPrintingTask(rootNS, xml, resultData);

            return Serializer.Serialize(resultData);
        }

        private void SetAction(PullPrintDocumentProcessActions oldAction, SafeComActivityData newActivitytData)
        {
            switch (oldAction)
            {
                case PullPrintDocumentProcessActions.PrintSingleDocument:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.Print;
                    break;
                case PullPrintDocumentProcessActions.PrintAllDocuments:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.PrintAll;
                    break;
                case PullPrintDocumentProcessActions.PrintKeepSingleDocument:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.PrintRetain;
                    break;
                case PullPrintDocumentProcessActions.PrintKeepAllDocuments:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.PrintRetainAll;
                    newActivitytData.SelectAll = true;
                    break;
                case PullPrintDocumentProcessActions.DeleteSingleDocument:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.Delete;
                    break;
                case PullPrintDocumentProcessActions.DeleteAllDocuments:
                    newActivitytData.DocumentProcessAction = SafeComPullPrintAction.DeleteAll;
                    newActivitytData.SelectAll = true;
                    break;
            }
        }

        private bool UseSolutionAuth(XNamespace rootNS, XElement xml)
        {
            XElement authElement = xml.Element(rootNS + "AuthenticatorData");
            XNamespace authNS = GetNamespace(authElement);

            // Get the name of the button to push for Auth operation
            XElement buttonElement = authElement.Element(authNS + "InitiationButton");

            return buttonElement != null ? (buttonElement.Value == "Pull Print") : false;
        }

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
                yield return new SerializerProxyMap(typeof(SafeComActivityData1_0), "STF.Plugin.CommonWorkflowCandidates", "HP.DeviceAutomation.FeatureCandidate.PullPrinting", "PullPrintingActivityData");
                yield return new SerializerProxyMap(typeof(DeviceMemoryProfilerConfig), "STF.Plugin.CommonWorkflowCandidates", "HP.DeviceAutomation.FeatureCandidate", "DeviceMemoryProfilerConfig");
            }
        }
    }

    /// <summary>
    /// The Proxy class that receives the serialization of the current outdated metadata.
    /// </summary>
    [DataContract]
    internal class SafeComActivityData1_0
    {
        [DataMember]
        public PullPrintDocumentProcessActions DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        //[DataMember]
        //public PrintingTaskConfigData PrintingTaskData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComActivityData1_0"/> class.
        /// </summary>
        public SafeComActivityData1_0()
        {
            DocumentProcessAction = PullPrintDocumentProcessActions.PrintSingleDocument;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            //PrintingTaskData = new PrintingTaskConfigData();
        }
    }

}
