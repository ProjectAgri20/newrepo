using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    public class FolderSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> Folder { get; set; }

        [DataMember]
        public DataPair<string> EnableScanToFolder { get; set; }

        #region ScanSettings

        [DataMember]
        public ScanSettings ScanSettingsData { get; set; }

        [DataMember]
        public DataPair<string> CroppingOption { get; set; }

        #endregion ScanSettings

        #region FileSettings

        [DataMember]
        public FileSettings FileSettingsData { get; set; }

        #endregion FileSettings

        private StringBuilder _failedSettings = new StringBuilder();

        public FolderSettingsData()
        {
            Folder = new DataPair<string> { Key = String.Empty };
            EnableScanToFolder = new DataPair<string> { Key = String.Empty };
            CroppingOption = new DataPair<string> { Key = String.Empty };

            ScanSettingsData = new ScanSettings();
            FileSettingsData = new FileSettings();
        }

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(FolderSettingsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties().Where(x => x.PropertyType == typeof(DataPair<string>)))
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }
            foreach (var propertyInfo in typeof(ScanSettings).GetProperties())
            {
                properties.Add(propertyInfo.Name, propertyInfo.GetValue(ScanSettingsData));
            }
            foreach (var propertyInfo in typeof(FileSettings).GetProperties())
            {
                properties.Add(propertyInfo.Name, propertyInfo.GetValue(FileSettingsData));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableScanToFolder":
                        result &= SetFolderDefaultValues("FolderServiceEnabled", "Enable ScanToFolder", (DataPair<string>)item.Value, device);
                        break;

                    case "Folder":
                        result &= SetFolderDefaultValues("SendToFolderWithoutReadAccess", "Folder Access Type", (DataPair<string>)item.Value, device);
                        break;

                    case "OriginalSize":
                        result &= SetFolderDefaultJobValues("ScanMediaSize", "Scan Media Size", (DataPair<string>)item.Value, device);
                        break;

                    case "OriginalSides":
                        result &= SetFolderDefaultJobValues("ScanPlexMode", "Original Side", (DataPair<string>)item.Value, device);
                        break;

                    case "Optimize":
                        result &= SetFolderDefaultJobValues("ScanMode", "Optimize", (DataPair<string>)item.Value, device);
                        break;

                    case "Orientation":
                        result &= SetFolderDefaultJobValues("OriginalContentOrientation", "Orientation", (DataPair<string>)item.Value, device);
                        break;

                    case "ImagePreview":
                        result &= SetFolderDefaultJobValues("ImagePreview", "Image Preview", (DataPair<string>)item.Value, device);
                        break;

                    case "Cleanup":
                        result &= SetFolderDefaultJobValues("BackgroundRemoval", "Cleanup", (DataPair<string>)item.Value, device);
                        break;

                    case "Sharpness":
                        result &= SetFolderDefaultJobValues("Sharpness", "Sharpness", (DataPair<string>)item.Value, device);
                        break;

                    case "Darkness":
                        result &= SetFolderDefaultJobValues("Exposure", "Darkness", (DataPair<string>)item.Value, device);
                        break;

                    case "Contrast":
                        result &= SetFolderDefaultJobValues("Contrast", "Contrast", (DataPair<string>)item.Value, device);
                        break;

                    case "CroppingOption":
                        {
                            DataPair<string> offDataPair = new DataPair<string> { Key = "off", Value = true };
                            DataPair<string> onDataPair = new DataPair<string> { Key = "on", Value = true };
                            switch (CroppingOption.Key)
                            {
                                case "0":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", "Cropping Option", offDataPair, device);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", "Cropping Option", offDataPair, device);
                                    }
                                    break;

                                case "1":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", "Cropping Option", offDataPair, device);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", "Cropping Option", onDataPair, device);
                                    }
                                    break;

                                case "2":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", "Cropping Option", onDataPair, device);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", "Cropping Option", offDataPair, device);
                                    }
                                    break;
                            }
                        }
                        break;

                    case "FileName":
                        result &= SetFolderDefaultJobValues("FileName", "FileName", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNamePrefix":
                        result &= SetFolderDefaultJobValues("FileNamePrefix", "FileName Prefix", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNameSuffix":
                        result &= SetFolderDefaultJobValues("FileNameSuffix", "FileName Suffix", (DataPair<string>)item.Value, device);
                        break;

                    case "FileType":
                        result &= SetFolderDefaultJobValues("DSFileType", "File Type", (DataPair<string>)item.Value, device);
                        break;

                    case "Resolution":
                        result &= SetFolderDefaultJobValues("DSImageResolution", "File Resolution", (DataPair<string>)item.Value, device);
                        break;

                    case "FileColor":
                        result &= SetFolderDefaultJobValues("DSColorPreference", "Color", (DataPair<string>)item.Value, device);
                        break;

                    case "FileSize":
                        result &= SetFolderDefaultJobValues("DSAttachmentSize", "File Size", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNumbering":
                        result &= SetFolderDefaultJobValues("AttachmentFileNameFormat", "File Size", (DataPair<string>)item.Value, device);
                        break;
                }
            }

            return result;
        }

        private bool SetFolderDefaultValues(string elementName, string propertyName, DataPair<string> itemValue, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService";
            string endpoint = "folder";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, propertyName);
        }

        private bool SetFolderDefaultJobValues(string elementName, string propertyName, DataPair<string> itemValue, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:DefaultJob";
            string endpoint = "folder";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, propertyName);
        }

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName)
        {
            bool success;
            if (data.Value)
            {
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    success = getProperty(tic);
                }
                catch
                {
                    Logger.LogError($"Failed to set field {activityName}");
                    success = false;
                }
            }
            else
            {
                success = true;
            }
            if (!success)
            {
                _failedSettings.Append($"{activityName}, ");
            }
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