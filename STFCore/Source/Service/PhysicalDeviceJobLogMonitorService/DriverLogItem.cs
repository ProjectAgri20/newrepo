using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    //<jasi:DriverInfo>
    //  <jasi:JobUUID>5d89f107-c42e-4cb3-855a-6924a6d3e2ca</jasi:JobUUID>
    //  <jasi:ApplicationInfo>
    //    <dd:ApplicationName>Microsoft Word</dd:ApplicationName>
    //    <dd:FileName>WINWORD.EXE</dd:FileName>
    //  </jasi:ApplicationInfo>
    //  <jasi:ClientInfo>
    //    <dd:IPAddress>15.1.1.1</dd:IPAddress>
    //    <dd:Hostname>W7X64-001-004</dd:Hostname>
    //  </jasi:ClientInfo>
    //  <dd:JobAcct13></dd:JobAcct13>
    //  <dd:JobAcct14></dd:JobAcct14>
    //  <dd:JobAcct15></dd:JobAcct15>
    //  <dd:JobAcct16></dd:JobAcct16>
    //  <dd:KeyValuePairs>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct1</dd:Key>
    //      <dd:ValueString>u00042</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct2</dd:Key>
    //      <dd:ValueString>W7X64-001-004</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct3</dd:Key>
    //      <dd:ValueString>ETL</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct4</dd:Key>
    //      <dd:ValueString>20140919111416</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct5</dd:Key>
    //      <dd:ValueString>5d89f107-c42e-4cb3-855a-6924a6d3e2ca</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct6</dd:Key>
    //      <dd:ValueString>Microsoft Word</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct7</dd:Key>
    //      <dd:ValueString>WINWORD.EXE</dd:ValueString>
    //    </dd:KeyValuePair>
    //    <dd:KeyValuePair>
    //      <dd:Key>JobAcct8</dd:Key>
    //      <dd:ValueString>u00042</dd:ValueString>
    //    </dd:KeyValuePair>
    //  </dd:KeyValuePairs>
    //</jasi:DriverInfo>


namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class DriverLogItem
    {
        public string JobUUID { get; set; }
        public string ApplicationName { get; set; }
        public string FileName { get; set; }
        public string ClientInfo_IPAddress { get; set; }
        public string ClientInfo_Hostname { get; set; }

        public DriverLogItem()
        {
            JobUUID = string.Empty;
            ApplicationName = string.Empty;
            FileName = string.Empty;
            ClientInfo_Hostname = string.Empty;
            ClientInfo_IPAddress = string.Empty;
        }
        public override string ToString()
        {
            return "JobUUID={0}, ApplicationName={1}".FormatWith(JobUUID, ApplicationName);
        }
    }
}
