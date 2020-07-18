using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Print;

namespace HP.ScalableTest.Plugin.Printing
{
    internal class PrintingDataConverter1_1 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        public XElement Convert(XElement xml)
        {
            // Creating the latest ActivityData
            PrintingActivityData resultData = new PrintingActivityData()
            {
                ShuffleDocuments = GetBool(xml, "ShuffleDocuments"),
                PrintJobSeparator = GetBool(xml, "PrintJobSeparator"),
                JobThrottling = GetBool(xml, "JobThrottling"),
                MaxJobsInQueue = GetInt(xml, "MaxJobsInQueue")
            };

            return Serializer.Serialize(resultData);
        }
    }
}
