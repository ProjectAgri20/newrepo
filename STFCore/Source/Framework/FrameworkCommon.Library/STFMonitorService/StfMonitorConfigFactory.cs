using HP.ScalableTest.Core;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Monitor
{
    public static class StfMonitorConfigFactory
    {
        /// <summary>
        /// Creates the appropriate <see cref="StfMonitorConfig" /> object from the specified <see cref="STFMonitorType" /> using this instance's mappings.
        /// </summary>
        /// <param name="monitorType">The <see cref="STFMonitorType" /> used to determine the type of <see cref="StfMonitorConfig" /> to create.</param>
        /// <param name="xml">The xml serialization string used to deserialize the correct <see cref="StfMonitorConfig" /> object.</param>
        /// <returns>The constructed <see cref="StfMonitorConfig" /> object.</returns>
        public static StfMonitorConfig Create(STFMonitorType monitorType, string xml)
        {
            switch (monitorType)
            {
                case STFMonitorType.DigitalSendNotification:
                case STFMonitorType.OutputDirectory:
                case STFMonitorType.OutputEmail:
                case STFMonitorType.LANFax:
                case STFMonitorType.SharePoint:
                    return LegacySerializer.DeserializeXml<OutputMonitorConfig>(xml);
                case STFMonitorType.Directory:
                    return LegacySerializer.DeserializeXml<DirectoryMonitorConfig>(xml);
                case STFMonitorType.DSSServer:
                case STFMonitorType.Hpcr:
                case STFMonitorType.EPrint:
                    return LegacySerializer.DeserializeXml<DatabaseMonitorConfig>(xml);
                default:
                    // Any STFMonitorType that does not have a custom config will use the default StfMonitorConfig
                    return LegacySerializer.DeserializeXml<StfMonitorConfig>(xml);
            }
        }

        public static StfMonitorConfig Create(string monitorType, string xml)
        {
            return Create(EnumUtil.Parse<STFMonitorType>(monitorType), xml);
        }
    }
}
