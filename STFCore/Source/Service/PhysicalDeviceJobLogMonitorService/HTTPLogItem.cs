
      //<jasi:HTTPInfo>
      //  <dd:UserName></dd:UserName>
      //  <dd:URI>http://15.198.216.104/DeviceClient/DeviceJobPost.aspx?Group=14N3+MOAT&amp;FilenameBase=15.198.212.248_85ebad3f-88f9-4d3d-a202-2bb6993b56f1_1</dd:URI>
      //  <jasi:DeliveredFilesInfo>
      //    <jasi:Files>
      //      <jasi:File>
      //        <dd:FileName>15.198.212.248_85ebad3f-88f9-4d3d-a202-2bb6993b56f1_1-01.pdf</dd:FileName>
      //        <dd:DataSize>617482</dd:DataSize>
      //      </jasi:File>
      //    </jasi:Files>
      //    <jasi:MetadataFile>
      //      <dd:FileName>15.198.212.248_85ebad3f-88f9-4d3d-a202-2bb6993b56f1_1.xml</dd:FileName>
      //      <dd:DataSize>620</dd:DataSize>
      //    </jasi:MetadataFile>
      //    <jasi:FileSummaries>
      //      <jasi:TotalDataSize>618102</jasi:TotalDataSize>
      //      <jasi:FilenameBase>15.198.212.248_85ebad3f-88f9-4d3d-a202-2bb6993b56f1_1</jasi:FilenameBase>
      //      <jasi:FilenameExtension>.pdf</jasi:FilenameExtension>
      //      <jasi:MetadataFilenameBase>15.198.212.248_85ebad3f-88f9-4d3d-a202-2bb6993b56f1_1.xml</jasi:MetadataFilenameBase>
      //      <jasi:MetadataFilenameExtension>.xml</jasi:MetadataFilenameExtension>
      //    </jasi:FileSummaries>
      //  </jasi:DeliveredFilesInfo>
      //  <dd:UserDomain></dd:UserDomain>
      //  <jasi:CompletionStatus>Succeeded</jasi:CompletionStatus>
      //</jasi:HTTPInfo>

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class HTTPLogItem
    {
        public string URI { get; set; }
        public string FileNameBase { get; set; }
        public string TotalDataSize { get; set; }
        public string FileNameExtension { get; set; }
        public string CompletionStatus { get; set; }

        public HTTPLogItem()
        {
            URI = string.Empty;
            FileNameBase = string.Empty;
            TotalDataSize = string.Empty;
            FileNameExtension = string.Empty;
            CompletionStatus = string.Empty;
        }
        public override string ToString()
        {
            return "URI={0}, Plex={1}".FormatWith(URI, FileNameBase);
        }
    }
}
