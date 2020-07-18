using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// Contains the various job account objects. There are two objects, JobInfo and UserInfo, always present. The rest are dependent on job type.
    /// <JobAccountingData>
    ///     Always present
    ///     <JobInfo></JobInfo>
    ///     <UserInfo></UserInfo>
    ///     
    ///     Job dependent (known so far)
    ///     <DriverInfo></DriverInfo>
    ///     <ScanInfo></ScanInfo>
    ///     <FolderInfo></FolderInfo>
    ///     <FolderListInfo></FolderListInfo>
    ///     <PrintInfo></PrintInfo>
    ///     <HTTPInfo></HTTPInfo>
    ///     <HTTPListInfo></HTTPListInfo>
    /// </JobAccountingData>
    /// </summary>
    public class JobAccountingLogItem
    {
        public JobLogItem JobInfo { get; set; }
        public UserLogItem UserInfo { get; set; }
        public DriverLogItem DriverInfo { get; set; }
        public HTTPLogItem HttpInfo { get; set; }
        public PrintLogItem PrintInfo { get; set; }
        public FolderLogItem FolderInfo { get; set; }
        public ScanLogItem ScanInfo { get; set; }

    }
}
