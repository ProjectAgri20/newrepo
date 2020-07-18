using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Service.Monitor.AutoStore
{
    internal struct ParentNodes
    {
        public const string JobData = "JobData";
        public const string FileData = "FileData";
        public const string ServerData = "ServerData";
        public const string DeviceData = "DeviceData";
    }
    internal struct ChildNodes
    {
        public const string ServerName = "ServerName";
        public const string FileName = "FileName";
        public const string JobId = "JobId";
        public const string FormName = "FormName";
        public const string FileSize = "FileSize";
        public const string FileExt = "FileExt";
        public const string PageCount = "PageCount";
        public const string ModelProduct = "ModelProduct";
        public const string DeviceTime = "DeviceTime";
        public const string EmailConnector = "EmailConnector";
        public const string EmailEndDt = "EmailEndDt";
        public const string FolderConnector = "FolderConnector";
        public const string FolderEndDt = "FolderEndDt";
        public const string SharePointConnector = "SharePointConnector";
        public const string SharePointEndDt = "SharePointEndDt";
        public const string LanFaxConnector = "FaxConnector";
        public const string LanFaxEndDt = "FaxEndDt";
    }
    internal struct AutoStoreJobType
    {
        public const string Email = "Email";
        public const string Folder = "Folder";
        public const string SharePoint = "SharePoint";
        public const string LanFAX = "Fax";
    }
}
