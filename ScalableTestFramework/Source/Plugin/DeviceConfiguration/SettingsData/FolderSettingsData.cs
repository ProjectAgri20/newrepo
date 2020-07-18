using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
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
        /// <summary>
        /// Execution entry point
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
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

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                _failedSettings.AppendLine("Device has no Scanner capability, skipping Folder Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Folder Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Folder Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableScanToFolder":
                        result &= SetFolderDefaultValues("FolderServiceEnabled", (DataPair<string>)item.Value, device, assetInfo, "Enable ScanToFolder", data);
                        break;

                    case "Folder":
                        result &= SetFolderDefaultValues("SendToFolderWithoutReadAccess", (DataPair<string>)item.Value, device, assetInfo, "Folder Access Type", data);
                        break;

                    case "OriginalSize":
                        result &= SetFolderDefaultJobValues("ScanMediaSize", (DataPair<string>)item.Value, device, assetInfo, "Scan Media Size", data);
                        break;

                    case "OriginalSides":
                        result &= SetFolderDefaultJobValues("ScanPlexMode", (DataPair<string>)item.Value, device, assetInfo, "Original Side", data);
                        break;

                    case "Optimize":
                        result &= SetFolderDefaultJobValues("ScanMode", (DataPair<string>)item.Value, device, assetInfo, "Optimize", data);
                        break;

                    case "Orientation":
                        result &= SetFolderDefaultJobValues("OriginalContentOrientation", (DataPair<string>)item.Value, device, assetInfo, "Orientation", data);
                        break;

                    case "ImagePreview":
                        result &= SetFolderDefaultJobValues("ImagePreview", (DataPair<string>)item.Value, device, assetInfo, "Image Preview", data);
                        break;

                    case "Cleanup":
                        result &= SetFolderDefaultJobValues("BackgroundRemoval", (DataPair<string>)item.Value, device, assetInfo, "Cleanup", data);
                        break;

                    case "Sharpness":
                        result &= SetFolderDefaultJobValues("Sharpness", (DataPair<string>)item.Value, device, assetInfo, "Sharpness", data);
                        break;

                    case "Darkness":
                        result &= SetFolderDefaultJobValues("Exposure", (DataPair<string>)item.Value, device, assetInfo, "Darkness", data);
                        break;

                    case "Contrast":
                        result &= SetFolderDefaultJobValues("Contrast", (DataPair<string>)item.Value, device, assetInfo, "Contrast", data);
                        break;

                    case "CroppingOption":
                        {
                            DataPair<string> offDataPair = new DataPair<string> { Key = "off", Value = true };
                            DataPair<string> onDataPair = new DataPair<string> { Key = "on", Value = true };
                            switch (CroppingOption.Key)
                            {
                                case "0":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", offDataPair, device, assetInfo, "Cropping Option", data);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", offDataPair, device, assetInfo, "Cropping Option", data);
                                    }
                                    break;

                                case "1":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", offDataPair, device, assetInfo, "Cropping Option", data);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", onDataPair, device, assetInfo, "Cropping Option", data);
                                    }
                                    break;

                                case "2":
                                    {
                                        result &= SetFolderDefaultJobValues("AutoCrop", onDataPair, device, assetInfo, "Cropping Option", data);
                                        result &= SetFolderDefaultJobValues("AutoPageCrop", offDataPair, device, assetInfo, "Cropping Option", data);
                                    }
                                    break;
                            }
                        }
                        break;

                    case "FileName":
                        result &= SetFolderDefaultJobValues("FileName", (DataPair<string>)item.Value, device, assetInfo, "FileName", data);
                        break;

                    case "FileNamePrefix":
                        result &= SetFolderDefaultJobValues("FileNamePrefix", (DataPair<string>)item.Value, device, assetInfo, "FileName Prefix", data);
                        break;

                    case "FileNameSuffix":
                        result &= SetFolderDefaultJobValues("FileNameSuffix", (DataPair<string>)item.Value, device, assetInfo, "FileName Suffix", data);
                        break;

                    case "FileType":
                        result &= SetFolderDefaultJobValues("DSFileType", (DataPair<string>)item.Value, device, assetInfo, "File Type", data);
                        break;

                    case "Resolution":
                        result &= SetFolderDefaultJobValues("DSImageResolution", (DataPair<string>)item.Value, device, assetInfo, "Resolution", data);
                        break;

                    case "FileColor":
                        result &= SetFolderDefaultJobValues("DSColorPreference", (DataPair<string>)item.Value, device, assetInfo, "Color", data);
                        break;

                    case "FileSize":
                        result &= SetFolderDefaultJobValues("DSAttachmentSize", (DataPair<string>)item.Value, device, assetInfo, "File Size", data);
                        break;

                    case "FileNumbering":
                        result &= SetFolderDefaultJobValues("AttachmentFileNameFormat", (DataPair<string>)item.Value, device, assetInfo, "File Numbering Format", data);
                        break;
                }
            }

            return result;
        }

        private bool SetFolderDefaultValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService";
            string endpoint = "folder";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElements(elementName).First().SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetFolderDefaultJobValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:folder:FolderService:DefaultJob";
            string endpoint = "folder";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement(elementName).SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> changeValue, JediDevice device, DataPair<T> data, string urn, string endpoint,  AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            bool success;
            if (data.Value)
            {
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    changeValue(tic);
                    device.WebServices.PutDeviceTicket(endpoint, urn, tic);
                    success = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {fieldChanged}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {fieldChanged}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = fieldChanged;
                log.Result = success ? "Passed" : "Failed";
                log.Value = data.Key.ToString();
                log.ControlChanged = "Folder Default";

                ExecutionServices.DataLogger.Submit(log);
            }
            else
            {
                success = true;
            }
            return success;
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