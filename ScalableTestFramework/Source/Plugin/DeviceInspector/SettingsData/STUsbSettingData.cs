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

        public bool GetFields(JediDevice device)
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

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableScanToUsb":
                        result &= SetUsbDefaultValues("UsbServiceEnabled", "Enable ScanToUsb", (DataPair<string>)item.Value,
                             device);
                        break;

                    case "OriginalSize":
                        result &= SetUsbDefaultJobValues("ScanMediaSize", "Scan Media Size", (DataPair<string>)item.Value, device);
                        break;

                    case "OriginalSides":
                        result &= SetUsbDefaultJobValues("ScanPlexMode", "Original Side", (DataPair<string>)item.Value, device);
                        break;

                    case "Optimize":
                        result &= SetUsbDefaultJobValues("ScanMode", "Optimize", (DataPair<string>)item.Value, device);
                        break;

                    case "Orientation":
                        result &= SetUsbDefaultJobValues("OriginalContentOrientation", "Orientation", (DataPair<string>)item.Value, device);
                        break;

                    case "ImagePreview":
                        result &= SetUsbDefaultJobValues("ImagePreview", "Image Preview", (DataPair<string>)item.Value, device);
                        break;

                    case "Cleanup":
                        result &= SetUsbDefaultJobValues("BackgroundRemoval", "Cleanup", (DataPair<string>)item.Value, device);
                        break;

                    case "Sharpness":
                        result &= SetUsbDefaultJobValues("Sharpness", "Sharpness", (DataPair<string>)item.Value, device);
                        break;

                    case "Darkness":
                        result &= SetUsbDefaultJobValues("Exposure", "Darkness", (DataPair<string>)item.Value, device);
                        break;

                    case "Contrast":
                        result &= SetUsbDefaultJobValues("Contrast", "Contrast", (DataPair<string>)item.Value, device);
                        break;

                    case "FileName":
                        result &= SetUsbDefaultJobValues("FileName", "FileName", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNamePrefix":
                        result &= SetUsbDefaultJobValues("FileNamePrefix", "FileName Prefix", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNameSuffix":
                        result &= SetUsbDefaultJobValues("FileNameSuffix", "FileName Suffix", (DataPair<string>)item.Value, device);
                        break;

                    case "FileType":
                        result &= SetUsbDefaultJobValues("DSFileType", "File Type", (DataPair<string>)item.Value, device);
                        break;

                    case "Resolution":
                        result &= SetUsbDefaultJobValues("DSImageResolution", "File Resolution", (DataPair<string>)item.Value, device);
                        break;

                    case "FileColor":
                        result &= SetUsbDefaultJobValues("DSColorPreference", "Color", (DataPair<string>)item.Value, device);
                        break;

                    case "FileSize":
                        result &= SetUsbDefaultJobValues("DSAttachmentSize", "File Size", (DataPair<string>)item.Value, device);
                        break;

                    case "FileNumbering":
                        result &= SetUsbDefaultJobValues("AttachmentFileNameFormat", "File Size", (DataPair<string>)item.Value, device);
                        break;
                }
            }

            return result;
        }

        private bool SetUsbDefaultValues(string elementName, string propertyName, DataPair<string> itemValue, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService";
            string endpoint = "usb";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase); 

            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, propertyName);
        }

        private bool SetUsbDefaultJobValues(string elementName, string propertyName, DataPair<string> itemValue, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:usb:UsbService:DefaultJob";
            string endpoint = "usb";

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