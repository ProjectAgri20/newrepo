
namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// <FolderInfo>
    ///   <dd:UNCPath>/Customer/NSEData/6c2a5544-334a-420d-8f6e-47dda8815a5a/2014.09.25_12.59.56.348_SID-61258472-6530-4bab-9984-663fb879a353</dd:UNCPath>
    ///    <jasi:DeliveredFilesInfo>
    ///    <jasi:Files>
    ///      <jasi:File>
    ///        <dd:FileName>Filename-01.pdf</dd:FileName>
    ///        <dd:DataSize>246266</dd:DataSize>
    ///      </jasi:File>
    ///    </jasi:Files>
    ///    <jasi:FileSummaries>
    ///      <jasi:TotalDataSize>246266</jasi:TotalDataSize>
    ///      <jasi:FilenameBase>Filename</jasi:FilenameBase>
    ///      <jasi:FilenameExtension>.pdf</jasi:FilenameExtension>
    ///    </jasi:FileSummaries>
    ///  </jasi:DeliveredFilesInfo>
    ///  <jasi:CompletionStatus>Succeeded</jasi:CompletionStatus>
    /// </FolderInfo>
    /// </summary>
    public class FolderLogItem
    {
        public string UNCPath { get; set; }
        public string  Filename { get; set; }
        public string DataSize { get; set; }
        public string CompletionStatus { get; set; }

        public FolderLogItem()
        {
            UNCPath = string.Empty;
            Filename = string.Empty;
            DataSize = string.Empty;
            CompletionStatus = string.Empty;
        }
        public override string ToString()
        {
            return "FileName={0}, UNCPath={1}".FormatWith(Filename, UNCPath);
        }
    }
}
