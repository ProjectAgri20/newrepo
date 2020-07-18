using HP.ScalableTest;

    //<jasi:JobInfo>
    //  <dd:ApplicationName>Microsoft Word</dd:ApplicationName>
    //  <dd:UUID>8ea7c161-a7f2-4be5-beb8-4aeecb1f81a5</dd:UUID>
    //  <dd:DeviceJobName>Microsoft Word - __1pgText_-_a00c217c-9c48-49fe-adf8-13c23bdc3086__</dd:DeviceJobName>
    //  <jasi:ProcessedBy>Device</jasi:ProcessedBy>
    //  <dd:JobStartedTimestamp>2014-09-19T18:47:26.858</dd:JobStartedTimestamp>
    //  <dd:JobPaused>false</dd:JobPaused>
    //  <dd:JobDoneTimestamp>2014-09-19T18:47:34.356</dd:JobDoneTimestamp>
    //  <jasi:JobDoneStatus>Succeeded</jasi:JobDoneStatus>
    //  <jasi:DataSource>NetworkIO</jasi:DataSource>
    //  <jasi:Destinations>
    //    <jasi:Destination>PrintEngine</jasi:Destination>
    //  </jasi:Destinations>
    //  <jasi:JobCategory>Print</jasi:JobCategory>
    //  <jasi:ParentId>00000000-0000-0000-0000-000000000000</jasi:ParentId>
    //  <jasi:Ordinal>384</jasi:Ordinal>
    //</jasi:JobInfo>

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class JobLogItem
    {
        public string ApplicationName { get; set; }

        public string UUID { get; set; }

        public string DeviceJobName { get; set; }

        public string JobStartedTimestamp { get; set; }

        public string JobDoneTimestamp { get; set; }

        public string JobDoneStatus { get; set; }

        public string JobCategory { get; set; }

        public override string ToString()
        {
            return "UUID={0}, DeviceJobName={1}".FormatWith(UUID, DeviceJobName);
        }

    }
}