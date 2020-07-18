using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class QuickSetSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<Collection<AQuickSet>> QuickSetData { get; set; }

        private Dictionary<string, XNamespace> _nameSpaceDictionary;
        private int _priority;
        private XNamespace _dsd, _dd, _dd2, _dd3, _emailNs, _copyNs, _finishingNs, _folderNs, _securityNs, _sharepointNs, _usbNs, _faxdd, _faxNs;

        private StringBuilder _failedSettings = new StringBuilder();
        public QuickSetSettingsData()
        {
            QuickSetData = new DataPair<Collection<AQuickSet>>
            {
                Key = new Collection<AQuickSet>(),
                Value = true
            };
        }

        //Do things by type of unique QuickSetCollection
        /// <summary>
        /// Execution Entry point
        /// Individual function differences separated into delagate methods.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            bool result = true;
            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                _failedSettings.AppendLine("Device has no Scanner capability, skipping QuickSet Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "QuickSet Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "QuickSet Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }
            InitialisePrivateMembers();
            if (QuickSetData.Value)
            {
                var grouping = QuickSetData.Key.GroupBy(x => x.GetType());

                foreach (var group in grouping)
                {
                    switch (group.Key.Name)
                    {
                        case "EmailQuickSetData":
                            result &= SetEmailQuickSetData(group.OfType<EmailQuickSetData>(), device, assetInfo, "EmailQuickSetData", data);
                            break;

                        case "CopyQuickSetData":
                            result &= SetCopyQuickSetData(group.OfType<CopyQuickSetData>(), device, assetInfo, "CopyQuickSetData", data);
                            break;

                        case "STNFQuickSetData":
                            result &= SetSTNFQuickSetData(group.OfType<STNFQuickSetData>(), device, assetInfo, "NetworkFolderQuickSetData", data);
                            break;

                        case "STSharePointQuickSetData":
                            result &= SetSTSharePointQuickSetData(group.OfType<STSharePointQuickSetData>(), device, assetInfo, "SharePointQuickSetData", data);
                            break;

                        case "STUSBQuickSetData":                           
                            result &= SetSTUSBQuickSetData(group.OfType<STUSBQuickSetData>(), device, assetInfo, "ScanToUSBQuickSetData", data);
                            break;

                        case "ScanFaxQuickSetData":
                            result &= SetScanFaxQuickSetData(group.OfType<ScanFaxQuickSetData>(), device, assetInfo, "ScanFaxQuickSetData", data);
                            break;
                        case "FTPQuickSetData":
                            result &= SetFTPQuickSetData(group.OfType<FTPQuickSetData>(), device, assetInfo, "STPQuickSetData", data);
                            break;
                        default:
                            throw new Exception("QuickSet Setting Property Error");
                    }
                }
            }
            return result;
        }

        private void InitialisePrivateMembers()
        {
            _priority = 615;

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

        private bool SetEmailQuickSetData(IEnumerable<EmailQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            const string activityUrn = "urn:hp:imaging:con:service:email:EmailService:PredefinedJobs";
            const string endpoint = "email";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "email", _emailNs } };

            var emailQuickSetDatas = group as IList<EmailQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<EmailQuickSetData>> quick = new DataPair<IEnumerable<EmailQuickSetData>>
            {
                Value = true,
                Key = emailQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var emailPredefinedJobs = new XElement(_emailNs + "PredefinedJobs",
                    new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "email", _emailNs.NamespaceName));
                foreach (var emailQuickSetData in emailQuickSetDatas)
                {
                    string setFrom = emailQuickSetData.DefaultFrom.Contains("Default") ? "disabled" : "enabled";
                    string scanSettings = string.Format(Resources.scanSettings,
                        emailQuickSetData.ScanSetData.ContentOrientation.Key, emailQuickSetData.ScanSetData.OriginalSize.Key,
                        emailQuickSetData.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                        emailQuickSetData.FileSetData.FileType.Key, emailQuickSetData.FileSetData.Resolution.Key);
                    string emailSettings = string.Format(Resources.emailSettings, setFrom);
                    string displaySettings = string.Format(Resources.displaySettings, emailQuickSetData.Name, _priority++);
                    string emailPredefinedJob = string.Format(Resources.emailPredefinedJob, emailQuickSetData.Name,
                        Resources.notificationSettings, scanSettings, emailQuickSetData.ScanSetData.ImagePreview.Key,
                        attachmentSettings, emailSettings, displaySettings);
                    emailPredefinedJobs.Add(ParseFragment("email:PredefinedJob", emailPredefinedJob, _nameSpaceDictionary));
                }
                n = new WebServiceTicket(emailPredefinedJobs);
                return n;
            };

            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetCopyQuickSetData(IEnumerable<CopyQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:copy:CopyService:PredefinedJobs";
            string endpoint = "copy";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "copy", _copyNs }, { "finishing", _finishingNs } };

            var copyQuickSetDatas = group as IList<CopyQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<CopyQuickSetData>> quick = new DataPair<IEnumerable<CopyQuickSetData>>
            {
                Value = true,
                Key = copyQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var copyPredefinedJobs = new XElement(_copyNs + "PredefinedJobs",
                  new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "copy", _copyNs.NamespaceName));

                foreach (CopyQuickSetData data in copyQuickSetDatas)
                {
                    string copyScanSettings = string.Format(Resources.copyScanSettings,
                        data.ScanSetData.ContentOrientation.Key, data.ScanSetData.OriginalSize.Key,
                        data.ScanSetData.OriginalSides.Key);

                    string copySettings = string.Format(Resources.copySettings, data.Copies);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++);
                    string copyPredefinedJob = string.Format(Resources.copyPredefinedJob, data.Name, copyScanSettings,
                        copySettings, data.ScanSetData.ImagePreview.Key, displaySettings);

                    copyPredefinedJobs.Add(ParseFragment("copy:PredefinedJob", copyPredefinedJob, _nameSpaceDictionary));
                }
                n = new WebServiceTicket(copyPredefinedJobs);
                return n;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
        }

        private bool SetSTNFQuickSetData(IEnumerable<STNFQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:PredefinedJobs";
            string endpoint = "folder";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "folder", _folderNs }, { "security", _securityNs }, { "dd2", _dd2 }, { "dd3", _dd3 } };
            var stnfQuickSetDatas = group as IList<STNFQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STNFQuickSetData>> quick = new DataPair<IEnumerable<STNFQuickSetData>>
            {
                Value = true,
                Key = stnfQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var folderPredefinedJobs = new XElement(_folderNs + "PredefinedJobs",
                  new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd2", _dd2.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd3", _dd3.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "folder", _folderNs.NamespaceName));
                //              var folderPredefinedJobs = new XElement("PredefinedJobs",
                //new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                //new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                //new XAttribute(XNamespace.Xmlns + "dd2", _dd2.NamespaceName),
                //new XAttribute(XNamespace.Xmlns + "dd3", _dd3.NamespaceName),
                //new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                //new XAttribute(XNamespace.Xmlns + "folder", _folderNs.NamespaceName));


                foreach (STNFQuickSetData data in stnfQuickSetDatas)
                {
                    string scanSettings = string.Format(Resources.scanSettings, data.ScanSetData.ContentOrientation.Key,
                        data.ScanSetData.OriginalSize.Key, data.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                      data.FileSetData.FileType.Key, data.FileSetData.Resolution.Key);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++);

                    List<string> sendFolderDestinations = new List<string>();

                    foreach (var path in data.FolderPaths)
                    {
                        sendFolderDestinations.Add(string.Format(Resources.sendFolderDestination, path));
                    }

                    var sendFolderDestination = String.Join(String.Empty, sendFolderDestinations);
                    //string sendFolderDestination = string.Format(Resources.sendFolderDestination, data.FolderPath);

                    string extraJob = string.Format(Resources.sendFolderDestination, data.FolderPaths.FirstOrDefault());
                    string folderPredefinedJob = string.Format(Resources.sendFolderPredefinedJob, data.Name,
                        Resources.notificationSettings, scanSettings, data.ScanSetData.ImagePreview.Key,
                        attachmentSettings, sendFolderDestination, displaySettings, 
                        extraJob);

                    folderPredefinedJobs.Add(ParseFragment("folder:PredefinedJob", folderPredefinedJob, _nameSpaceDictionary));
                }

                n = new WebServiceTicket(folderPredefinedJobs);

                return n;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
        }

        private bool SetSTSharePointQuickSetData(IEnumerable<STSharePointQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:sharepoint:SharePointService:PredefinedJobs";
            string endpoint = "sharepoint";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "folder", _folderNs }, { "security", _securityNs }, { "dd2", _dd2 }, { "dd3", _dd3 }, { "sharepoint", _sharepointNs } };
            var stSharePointQuickSetDatas = group as IList<STSharePointQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STSharePointQuickSetData>> quick =
                new DataPair<IEnumerable<STSharePointQuickSetData>>
                {
                    Value = true,
                    Key = stSharePointQuickSetDatas
                };

           

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var sharepointPredefinedJobs = new XElement(_sharepointNs + "PredefinedJobs",
                 new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "folder", _folderNs.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "dd2", _dd2.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "dd3", _dd3.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "sharepoint", _sharepointNs.NamespaceName)
                 );

                foreach (STSharePointQuickSetData data in stSharePointQuickSetDatas)
                {
                    string scanSettings = string.Format(Resources.scanSettings, data.ScanSetData.ContentOrientation.Key,
                        data.ScanSetData.OriginalSize.Key, data.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                      data.FileSetData.FileType.Key, data.FileSetData.Resolution.Key);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++.ToString());
                    string sendSharePointDestination = string.Format(Resources.sendSharePointDestination, data.FolderPath);

                    string sharePointPredefinedJob = string.Format(Resources.sendSharePointPredefinedJob, data.Name,
                        Resources.notificationSettings, scanSettings, data.ScanSetData.ImagePreview.Key,
                        attachmentSettings, displaySettings, sendSharePointDestination);

                    sharepointPredefinedJobs.Add(ParseFragment("folder:PredefinedJob", sharePointPredefinedJob, _nameSpaceDictionary));
                }

                n = new WebServiceTicket(sharepointPredefinedJobs);
                return n;
            };
            bool quickSetAdded = UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
            if (quickSetAdded)
            {
                var sharePointTicket = device.WebServices.GetDeviceTicket("sharepoint", "urn:hp:imaging:con:service:sharepoint:SharePointService");
                var sharePointEnabled = sharePointTicket.FindElement("FolderServiceEnabled");
                if (sharePointEnabled.Value == "disabled")
                {
                    //enable sharepoint
                    sharePointEnabled.Value = "enabled";
                    device.WebServices.PutDeviceTicket("sharepoint",
                        "urn:hp:imaging:con:service:sharepoint:SharePointService", sharePointTicket);
                }
            }

            return quickSetAdded;

        }

        private bool SetSTUSBQuickSetData(IEnumerable<STUSBQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService:PredefinedJobs";
            string endpoint = "usb";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "usb", _usbNs }, { "finishing", _finishingNs }, { "security", _securityNs } };

            var stusbQuickSetDatas = group as IList<STUSBQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<STUSBQuickSetData>> quick = new DataPair<IEnumerable<STUSBQuickSetData>>
            {
                Value = true,
                Key = stusbQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var usbPredefinedJobs = new XElement(_usbNs + "PredefinedJobs",
                    new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "finishing", _finishingNs.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                 new XAttribute(XNamespace.Xmlns + "usb", _usbNs.NamespaceName)
                 );

                foreach (STUSBQuickSetData data in stusbQuickSetDatas)
                {
                    string scanSettings = string.Format(Resources.scanSettings, data.ScanSetData.ContentOrientation.Key,
                      data.ScanSetData.OriginalSize.Key, data.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                      data.FileSetData.FileType.Key, data.FileSetData.Resolution.Key);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++.ToString());

                    string usbPredefinedJob = string.Format(Resources.sendUsbPredefinedJob, data.Name,
                        Resources.notificationSettings, scanSettings, attachmentSettings,
                        data.ScanSetData.ImagePreview.Key, displaySettings);

                    usbPredefinedJobs.Add(ParseFragment("usb:PredefinedJob", usbPredefinedJob, _nameSpaceDictionary));
                }

                n = new WebServiceTicket(usbPredefinedJobs);
                return n;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
        }

        private bool SetScanFaxQuickSetData(IEnumerable<ScanFaxQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:PredefinedJobs";
            string endpoint = "fax";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "faxdd", _faxdd }, { "dsd", _dsd }, { "fax", _faxNs }, { "dd3", _dd3 }, { "security", _securityNs }, { "dd", _dd } };

            var scanFaxQuickSetDatas = group as IList<ScanFaxQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<ScanFaxQuickSetData>> quick = new DataPair<IEnumerable<ScanFaxQuickSetData>>
            {
                Value = true,
                Key = scanFaxQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var usbPredefinedJobs = new XElement(_faxNs + "PredefinedJobs",
                    new XAttribute(XNamespace.Xmlns + "faxdd", _faxdd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd3", _dd3.NamespaceName),
               new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
               new XAttribute(XNamespace.Xmlns + "fax", _faxNs.NamespaceName)
               );

                foreach (ScanFaxQuickSetData data in scanFaxQuickSetDatas)
                {
                    string scanSettings = string.Format(Resources.scanSettings, data.ScanSetData.ContentOrientation.Key,
                    data.ScanSetData.OriginalSize.Key, data.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                      data.FileSetData.FileType.Key, data.FileSetData.Resolution.Key);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++.ToString());

                    string faxSendDestination = string.Format(Resources.sendFaxDestination, data.Number);

                    string faxPredefinedJob = string.Format(Resources.sendFaxPredefinedJob,
                        Resources.notificationSettings, scanSettings, data.ScanSetData.ImagePreview.Key,
                        faxSendDestination, attachmentSettings, displaySettings);

                    usbPredefinedJobs.Add(ParseFragment("fax:PredefinedSendJob", faxPredefinedJob, _nameSpaceDictionary));
                }

                n = new WebServiceTicket(usbPredefinedJobs);
                return n;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
        }

        private bool SetFTPQuickSetData(IEnumerable<FTPQuickSetData> group, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData datas)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:PredefinedJobs";
            string endpoint = "folder";

            _nameSpaceDictionary = new Dictionary<string, XNamespace> { { "dsd", _dsd }, { "dd", _dd }, { "folder", _folderNs }, { "security", _securityNs }, { "dd2", _dd2 }, { "dd3", _dd3 } };
            var stnfQuickSetDatas = group as IList<FTPQuickSetData> ?? group.ToList();
            DataPair<IEnumerable<FTPQuickSetData>> quick = new DataPair<IEnumerable<FTPQuickSetData>>
            {
                Value = true,
                Key = stnfQuickSetDatas
            };

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var folderPredefinedJobs = new XElement(_folderNs + "PredefinedJobs",
                  new XAttribute(XNamespace.Xmlns + "dsd", _dsd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "security", _securityNs.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd2", _dd2.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd3", _dd3.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "dd", _dd.NamespaceName),
                  new XAttribute(XNamespace.Xmlns + "folder", _folderNs.NamespaceName));

                foreach (FTPQuickSetData data in stnfQuickSetDatas)
                {

                    string scanSettings = string.Format(Resources.scanSettings, data.ScanSetData.ContentOrientation.Key,
                        data.ScanSetData.OriginalSize.Key, data.ScanSetData.OriginalSides.Key);
                    string attachmentSettings = string.Format(Resources.attachmentSettings,
                      data.FileSetData.FileType.Key, data.FileSetData.Resolution.Key);
                    string displaySettings = string.Format(Resources.displaySettings, data.Name, _priority++);


                    List<string> sendFTPDestinations = new List<string>();

                    sendFTPDestinations.Add(string.Format(Resources.sendFTPDestination, data.FTPServer, data.PortNumber, data.DirectoryPath, data.UserName, data.FTPProtocol));


                    var sendFolderDestination = String.Join(String.Empty, sendFTPDestinations);

                    //string extraJob = string.Format(Resources.sendFTPDestination, data.FTPServer.FirstOrDefault());
                    string sendFTPDestination = string.Format(Resources.sendFTPDestination, data.FTPServer, data.PortNumber, data.DirectoryPath, data.UserName, data.FTPProtocol);
                    string folderPredefinedJob = string.Format(Resources.sendFolderPredefinedJob, data.Name,
                        Resources.notificationSettings, scanSettings, data.ScanSetData.ImagePreview.Key,
                        attachmentSettings, sendFTPDestination, displaySettings, sendFTPDestination);

                    folderPredefinedJobs.Add(ParseFragment("folder:PredefinedJob", folderPredefinedJob, _nameSpaceDictionary));
                }

                n = new WebServiceTicket(folderPredefinedJobs);

                return n;
            };
            return UpdateField(change, device, quick, activityUrn, endpoint, assetInfo, fieldChanged, datas);
        }
        /// <summary>
        /// Interface function to update and log device fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ChangeValue"></param>
        /// <param name="device"></param>
        /// <param name="data"></param>
        /// <param name="urn"></param>
        /// <param name="endpoint"></param>
        /// <param name="assetInfo"></param>
        /// <param name="activity"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>Success bool</returns>
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> changeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            bool success = true;
            if (data.Value)
            {
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    tic = changeValue(tic);

                    device.WebServices.PutDeviceTicket(endpoint, urn, tic, false);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {fieldChanged}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {fieldChanged}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = fieldChanged;
                log.Result = success ? "Passed" : "Failed";
                log.Value = "Quickset Values";
                log.ControlChanged = $@"Quicksets:{fieldChanged}";

                ExecutionServices.DataLogger.Submit(log);
            }
            return success;
        }

        private static XElement ParseFragment(string rootElementName, string fragment, IDictionary<string, XNamespace> namespaces)
        {
            var namespaceDefs = namespaces
                .Select(kvp => String.Format("xmlns:{0}=\"{1}\"", kvp.Key, kvp.Value.NamespaceName));
            var xml = $"<{rootElementName} {String.Join(" ", namespaceDefs)}>{fragment}</{rootElementName}>";
            var root = XElement.Parse(xml);
            root.RemoveAttributes();
            return root;
        }

        /// <summary>
        /// Interface function to drive setting of data fields and return results upstream
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>result</returns>
        public DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            _failedSettings = new StringBuilder();
            var result = ExecuteJob(device, assetInfo, data);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}