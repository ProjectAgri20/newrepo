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
    public class ScanUsbSettingData : IComponentData
    {
        [DataMember]
        public DataPair<string> EnableScanToUsb { get; set; }

        [DataMember]
        public ScanSettings ScanSettingsData { get; set; }

        [DataMember]
        public FileSettings FileSettingsData { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public ScanUsbSettingData()
        {
            EnableScanToUsb = new DataPair<string> { Key = string.Empty };
            ScanSettingsData = new ScanSettings();
            FileSettingsData = new FileSettings();
        }
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
            Type type = typeof(ScanUsbSettingData);
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
                _failedSettings.AppendLine("Device has no Scanner capability, skipping Scan USB Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Scan USB Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Scan USB Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableScanToUsb":
                        result &= SetUsbDefaultValues("UsbServiceEnabled", (DataPair<string>)item.Value, device, assetInfo, "Enable Scan to USB", data);
                        break;

                    case "OriginalSize":
                        result &= SetUsbDefaultJobValues("ScanMediaSize", (DataPair<string>)item.Value, device, assetInfo, "Scan Media Size", data);
                        break;

                    case "OriginalSides":
                        result &= SetUsbDefaultJobValues("ScanPlexMode", (DataPair<string>)item.Value, device, assetInfo, "Original Side", data);
                        break;

                    case "Optimize":
                        result &= SetUsbDefaultJobValues("ScanMode", (DataPair<string>)item.Value, device, assetInfo, "Optimize", data);
                        break;

                    case "Orientation":
                        result &= SetUsbDefaultJobValues("OriginalContentOrientation", (DataPair<string>)item.Value, device, assetInfo, "Orientation", data);
                        break;

                    case "ImagePreview":
                        result &= SetUsbDefaultJobValues("ImagePreview", (DataPair<string>)item.Value, device, assetInfo, "Image Preview", data);
                        break;

                    case "Cleanup":
                        result &= SetUsbDefaultJobValues("BackgroundRemoval", (DataPair<string>)item.Value, device, assetInfo, "Cleanup", data);
                        break;

                    case "Sharpness":
                        result &= SetUsbDefaultJobValues("Sharpness", (DataPair<string>)item.Value, device, assetInfo, "Sharpness", data);
                        break;

                    case "Darkness":
                        result &= SetUsbDefaultJobValues("Exposure", (DataPair<string>)item.Value, device, assetInfo, "Darkness", data);
                        break;

                    case "Contrast":
                        result &= SetUsbDefaultJobValues("Contrast", (DataPair<string>)item.Value, device, assetInfo, "Contrast", data);
                        break;

                    case "FileName":
                        result &= SetUsbDefaultJobValues("FileName", (DataPair<string>)item.Value, device, assetInfo, "File Name", data);
                        break;

                    case "FileNamePrefix":
                        result &= SetUsbDefaultJobValues("FileNamePrefix", (DataPair<string>)item.Value, device, assetInfo, "File Name Prefix", data);
                        break;

                    case "FileNameSuffix":
                        result &= SetUsbDefaultJobValues("FileNameSuffix", (DataPair<string>)item.Value, device, assetInfo, "File Name Suffix", data);
                        break;

                    case "FileType":
                        result &= SetUsbDefaultJobValues("DSFileType", (DataPair<string>)item.Value, device, assetInfo, "File Type", data);
                        break;

                    case "Resolution":
                        result &= SetUsbDefaultJobValues("DSImageResolution", (DataPair<string>)item.Value, device, assetInfo, "File Resolution", data);
                        break;

                    case "FileColor":
                        result &= SetUsbDefaultJobValues("DSColorPreference", (DataPair<string>)item.Value, device, assetInfo, "Color", data);
                        break;

                    case "FileSize":
                        result &= SetUsbDefaultJobValues("DSAttachmentSize", (DataPair<string>)item.Value, device, assetInfo, "File Size", data);
                        break;

                    case "FileNumbering":
                        result &= SetUsbDefaultJobValues("AttachmentFileNameFormat", (DataPair<string>)item.Value, device, assetInfo, "File Numbering", data);
                        break;
                }
            }

            return result;
        }

        private bool SetUsbDefaultValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService";
            string endpoint = "usb";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement(elementName).SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetUsbDefaultJobValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService:DefaultJob";
            string endpoint = "usb";

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
                log.ControlChanged = "Scan To USB Default";


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