using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class QuickSetSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<Collection<AQuickSet>> QuickSetData { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();
        private XNamespace _dsd, _dd, _dd2, _dd3, _emailNs, _copyNs, _finishingNs, _folderNs, _securityNs, _sharepointNs, _usbNs, _faxdd, _faxNs;

        public QuickSetSettingsData()
        {
            QuickSetData = new DataPair<Collection<AQuickSet>>
            {
                Key = new Collection<AQuickSet>(),
                Value = true
            };

            InitialisePrivateMembers();
        }

        private void InitialisePrivateMembers()
        {
            _dsd = "http://www.hp.com/schemas/imaging/con/digitalsending/2009/02/11";
            _dd = "http://www.hp.com/schemas/imaging/con/dictionaries/1.0/";
            _emailNs = "http://www.hp.com/schemas/imaging/con/service/email/2009/02/11";
            _copyNs = "http://www.hp.com/schemas/imaging/con/service/copy/2009/08/14";
            _finishingNs = "http://www.hp.com/schemas/imaging/con/finishing/2009/01/08";
            _folderNs = "http://www.hp.com/schemas/imaging/con/service/folder/2009/02/11";
            _securityNs = "http://www.hp.com/schemas/imaging/con/security/2009/02/11";
            _dd2 = "http://www.hp.com/schemas/imaging/con/dictionaries/2008/10/10";
            _dd3 = "http://www.hp.com/schemas/imaging/con/dictionaries/2009/04/06";
            _sharepointNs = "http://www.hp.com/schemas/imaging/con/service/sharepoint/2011/05/03";
            _usbNs = "http://www.hp.com/schemas/imaging/con/service/usb/2009/01/12";
            _faxdd = "http://www.hp.com/schemas/imaging/con/fax/2008/06/13";
            _faxNs = "http://www.hp.com/schemas/imaging/con/service/fax/2009/02/11";
        }

        //Do things by type of unique QuickSetCollection
        public bool GetFields(JediDevice device)
        {
            bool result = true;
            _failedSettings = new StringBuilder();
            InitialisePrivateMembers();
            if (QuickSetData.Value)
            {
                var grouping = QuickSetData.Key.GroupBy(x => x.GetType());

                foreach (var group in grouping)
                {
                    switch (group.Key.Name)
                    {
                        case "EmailQuickSetData":
                            result &= SetEmailQuickSetData(group.OfType<EmailQuickSetData>(), device);
                            break;

                        case "CopyQuickSetData":
                            result &= SetCopyQuickSetData(group.OfType<CopyQuickSetData>(), device);
                            break;

                        case "STNFQuickSetData":
                            result &= SetSTNFQuickSetData(group.OfType<STNFQuickSetData>(), device);
                            break;

                        case "STSharePointQuickSetData":
                            result &= SetSTSharePointQuickSetData(group.OfType<STSharePointQuickSetData>(), device);
                            break;

                        case "STUSBQuickSetData":
                            result &= SetSTUSBQuickSetData(group.OfType<STUSBQuickSetData>(), device);
                            break;

                        case "ScanFaxQuickSetData":
                            result &= SetScanFaxQuickSetData(group.OfType<ScanFaxQuickSetData>(), device);
                            break;

                        case "FTPQuickSetData":
                            result &= SetFTPQuickSetData(group.OfType<FTPQuickSetData>(), device);
                            break;

                        default:
                            throw new Exception("QuickSet Setting Property Error");
                    }
                }
            }
            return result;
        }

        private bool SetFTPQuickSetData(IEnumerable<FTPQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:PredefinedJobs";
            string endpoint = "folder";
            string name = "FTPQuickSet";
            var ftpQuickSetDatas = group as IList<FTPQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<FTPQuickSetData>> quick = new DataPair<IEnumerable<FTPQuickSetData>>
            {
                Value = true,
                Key = ftpQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;

                var jobs = n.Elements(_folderNs + "PredefinedJob").ToList();

                foreach (var data in ftpQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dd + "DSFileType").First().Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "DSImageResolution").First().Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);

                    var ftpDestinationElement = job.Element(_folderNs + "SendFolderDestinations")?.Element(_folderNs + "SendFTPDestination");
                    if (ftpDestinationElement == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= ftpDestinationElement.Element(_dd3 + "NetworkID").Value.Equals(data.FTPServer, StringComparison.OrdinalIgnoreCase);
                    result &= ftpDestinationElement.Element(_dd + "Port").Value.Equals(data.PortNumber, StringComparison.OrdinalIgnoreCase);
                    result &= ftpDestinationElement.Element(_dd + "DirectoryPath").Value.Equals(data.DirectoryPath, StringComparison.OrdinalIgnoreCase);
                    result &= ftpDestinationElement.Element(_dd + "FTPProtocol").Value.Equals(data.FTPProtocol, StringComparison.OrdinalIgnoreCase);
                    result &= ftpDestinationElement.Descendants(_dd + "UserName").First().Value.Equals(data.UserName, StringComparison.OrdinalIgnoreCase);
                }
                return result;
            };

            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetEmailQuickSetData(IEnumerable<EmailQuickSetData> group, JediDevice device)
        {
            const string activityUrn = "urn:hp:imaging:con:service:email:EmailService:PredefinedJobs";
            const string endpoint = "email";
            string name = "EmailQuickSet";

            var emailQuickSetDatas = group as IList<EmailQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<EmailQuickSetData>> quick = new DataPair<IEnumerable<EmailQuickSetData>>
            {
                Value = true,
                Key = emailQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                var jobs = n.Elements(_emailNs + "PredefinedJob").ToList();
                foreach (var data in emailQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    string setFrom = data.DefaultFrom.Contains("Default") ? "disabled" : "enabled";
                    result &= job.Descendants(_emailNs + "UseSignedInUserAsSender").First().Value == setFrom;
                    result &= job.Descendants(_emailNs + "AddSenderAsToRecipient").First().Value == setFrom;

                    result &= job.Descendants(_dd + "DSFileType").First().Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "DSImageResolution").First().Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);
                }
                return result;
            };

            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetCopyQuickSetData(IEnumerable<CopyQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:copy:CopyService:PredefinedJobs";
            string endpoint = "copy";
            string name = "CopyQuickSet";

            var copyQuickSetDatas = group as IList<CopyQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<CopyQuickSetData>> quick = new DataPair<IEnumerable<CopyQuickSetData>>
            {
                Value = true,
                Key = copyQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                var jobs = n.Elements(_copyNs + "PredefinedJob").ToList();

                foreach (CopyQuickSetData data in copyQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dd + "DefaultPrintCopies").First().Value.Equals(data.Copies, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);
                }

                return result;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetSTNFQuickSetData(IEnumerable<STNFQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:PredefinedJobs";
            string endpoint = "folder";
            string name = "NetworkFolderQuickSet";

            var stnfQuickSetDatas = group as IList<STNFQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STNFQuickSetData>> quick = new DataPair<IEnumerable<STNFQuickSetData>>
            {
                Value = true,
                Key = stnfQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;

                var jobs = n.Elements(_folderNs + "PredefinedJob").ToList();

                foreach (STNFQuickSetData data in stnfQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dd + "DSFileType").First().Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "DSImageResolution").First().Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);

                    var folderDestinationElement = job.Element(_folderNs + "SendFolderDestinations")?.Element(_folderNs + "SendFolderDestination");
                    if (folderDestinationElement == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= folderDestinationElement.Element(_dd + "UNCPath").Value.Equals(data.FolderPath, StringComparison.OrdinalIgnoreCase);
                }

                return result;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetSTSharePointQuickSetData(IEnumerable<STSharePointQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:sharepoint:SharePointService:PredefinedJobs";
            string endpoint = "sharepoint";
            string name = "SharePointQuickSet";

            var stSharePointQuickSetDatas = group as IList<STSharePointQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STSharePointQuickSetData>> quick =
                new DataPair<IEnumerable<STSharePointQuickSetData>>
                {
                    Value = true,
                    Key = stSharePointQuickSetDatas
                };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                var jobs = n.Elements(_folderNs + "PredefinedJob").ToList();

                foreach (STSharePointQuickSetData data in stSharePointQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }
                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dd + "DSFileType").First().Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "DSImageResolution").First().Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);

                    var sharePointDestinationElement = job.Element(_folderNs + "SendFolderDestinationList")?.Element(_folderNs + "SendSharePointDestination");
                    if (sharePointDestinationElement == null)
                    {
                        result = false;
                        continue;
                    }
                    result &= sharePointDestinationElement.Element(_folderNs + "SharePointPath").Value == data.FolderPath;
                }

                return result;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetSTUSBQuickSetData(IEnumerable<STUSBQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService:PredefinedJobs";
            string endpoint = "usb";
            string name = "USBQuickSet";

            var stusbQuickSetDatas = group as IList<STUSBQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STUSBQuickSetData>> quick = new DataPair<IEnumerable<STUSBQuickSetData>>
            {
                Value = true,
                Key = stusbQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                var jobs = n.Elements(_usbNs + "PredefinedJob").ToList();

                foreach (STUSBQuickSetData data in stusbQuickSetDatas)
                {
                    var job = jobs.Find(x => x.Element(_dd + "DeviceJobName").Value == data.Name);
                    if (job == null)
                    {
                        result = false;
                        continue;
                    }

                    result &= job.Descendants(_dd + "OriginalContentOrientation").First().Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanMediaSize").First().Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "ScanPlexMode").First().Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dd + "DSFileType").First().Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= job.Descendants(_dd + "DSImageResolution").First().Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= job.Descendants(_dsd + "ImagePreview").First().Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);
                }

                return result;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        private bool SetScanFaxQuickSetData(IEnumerable<ScanFaxQuickSetData> group, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:PredefinedJobs";
            string endpoint = "fax";
            string name = "ScanFaxQuickSet";

            var scanFaxQuickSetDatas = group as IList<ScanFaxQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<ScanFaxQuickSetData>> quick = new DataPair<IEnumerable<ScanFaxQuickSetData>>
            {
                Value = true,
                Key = scanFaxQuickSetDatas
            };

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;

                foreach (ScanFaxQuickSetData data in scanFaxQuickSetDatas)
                {
                    result &= n.FindElement("OriginalContentOrientation").Value.Equals(data.ScanSetData.ContentOrientation.Key, StringComparison.OrdinalIgnoreCase);
                    result &= n.FindElement("ScanMediaSize").Value.Equals(data.ScanSetData.OriginalSize.Key, StringComparison.OrdinalIgnoreCase);
                    result &= n.FindElement("ScanPlexMode").Value.Equals(data.ScanSetData.OriginalSides.Key, StringComparison.OrdinalIgnoreCase);

                    result &= n.FindElement("DSFileType").Value.Equals(data.FileSetData.FileType.Key, StringComparison.OrdinalIgnoreCase);
                    result &= n.FindElement("DSImageResolution").Value.Equals(data.FileSetData.Resolution.Key, StringComparison.OrdinalIgnoreCase);

                    result &= n.FindElement("FaxNumber").Value == data.Number;

                    result &= n.FindElement("Name").Value.Equals(data.Name, StringComparison.OrdinalIgnoreCase);
                    result &= n.FindElement("ImagePreview").Value.Equals(data.ScanSetData.ImagePreview.Key, StringComparison.OrdinalIgnoreCase);
                }

                return result;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, name);
        }

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName)
        {
            bool success = true;
            if (data.Value)
            {
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    success = getProperty(tic);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {activityName}, Exception: {ex.Message}");
                    success = false;
                }
            }
            if (!success)
                _failedSettings.Append($"{activityName}, ");

            return success;
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}