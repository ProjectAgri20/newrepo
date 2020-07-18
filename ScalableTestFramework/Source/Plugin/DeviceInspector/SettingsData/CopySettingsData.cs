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
    public class CopySettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> Copies { get; set; }

        [DataMember]
        public DataPair<string> ReduceEnlarge { get; set; }

        [DataMember]
        public DataPair<string> CopySides { get; set; }

        [DataMember]
        public DataPair<string> Color { get; set; }

        [DataMember]
        public DataPair<string> ScanMode { get; set; }

        [DataMember]
        public DataPair<string> Collate { get; set; }

        [DataMember]
        public DataPair<string> PagesPerSheet { get; set; }

        [DataMember]
        public ScanSettings ScanSettingsData { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public CopySettingsData()
        {
            Copies = new DataPair<string> { Key = String.Empty };
            CopySides = new DataPair<string> { Key = String.Empty };

            Color = new DataPair<string> { Key = String.Empty };
            ScanMode = new DataPair<string> { Key = String.Empty };

            ReduceEnlarge = new DataPair<string> { Key = String.Empty };
            Collate = new DataPair<string> { Key = String.Empty };

            PagesPerSheet = new DataPair<string> { Key = string.Empty };

            ScanSettingsData = new ScanSettings();
        }

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(CopySettingsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties().Where(x => x.PropertyType == typeof(DataPair<string>)))
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (PropertyInfo prop in typeof(ScanSettings).GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(ScanSettingsData));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "Copies":
                        result &= GetCopyDefaultValues("DefaultPrintCopies", "Default Print Copies", (DataPair<string>)item.Value, device);
                        break;

                    case "ReduceEnlarge":
                        {
                            //skip this if the field isn't enabled
                            if (((DataPair<string>) item.Value).Value)
                            {
                                var dataPair = new DataPair<string>
                                {
                                    Key = "false",
                                    Value = true
                                };
                                if (Convert.ToInt32(((DataPair<string>) item.Value).Key) == 100)
                                {
                                    dataPair.Key = "true";
                                }

                                result &= GetCopyDefaultValues("AutoScale", "Reduce Enlarge", dataPair, device);
                                result &= GetCopyDefaultValues("Scaling", "Reduce Enlarge",
                                    (DataPair<string>) item.Value, device);
                            }
                        }
                        break;

                    case "Color":
                        result &= GetCopyDefaultValues("ChromaticMode", "Color", (DataPair<string>)item.Value, device);
                        break;

                    case "ScanMode":
                        result &= GetCopyDefaultValues("CaptureMode", "Scan Mode", (DataPair<string>)item.Value, device);
                        break;

                    case "Collate":
                        result &= GetCopyDefaultValues("SheetCollate", "Collate", (DataPair<string>)item.Value, device);
                        break;

                    case "PagesPerSheet":
                        result &= GetCopyDefaultValues("CopyOutputNumberUpCount", "Pages per Sheet", (DataPair<string>)item.Value, device);
                        break;

                    case "OriginalSize":
                        result &= GetCopyDefaultValues("ScanMediaSize", "Original Size", (DataPair<string>)item.Value, device);
                        break;

                    case "CopySides":
                        result &= GetCopyDefaultValues("CopySides", "Copy Sides", (DataPair<string>)item.Value, device);
                        break;

                    case "Optimize":
                        result &= GetCopyDefaultValues("ScanMode", "Optimize Mode", (DataPair<string>)item.Value, device);
                        break;

                    case "ContentOrientation":
                        result &= GetCopyDefaultValues("OriginalContentOrientation", "Orientation", (DataPair<string>)item.Value, device);
                        break;

                    case "Cleanup":
                        result &= GetCopyDefaultValues("BackgroundRemoval", "Cleanup", (DataPair<string>)item.Value, device);
                        break;

                    case "Sharpness":
                        result &= GetCopyDefaultValues("Sharpness", "Sharpness", (DataPair<string>)item.Value, device);
                        break;

                    case "Darkness":
                        result &= GetCopyDefaultValues("Exposure", "Darkness", (DataPair<string>)item.Value, device);
                        break;

                    case "Contrast":
                        result &= GetCopyDefaultValues("Contrast", "Contrast", (DataPair<string>)item.Value, device);
                        break;

                    case "ImagePreview":
                        result &= GetCopyDefaultValues("ImagePreview", "Image Preview", (DataPair<string>)item.Value, device);
                        break;
                }
            }

            return result;
        }

        private bool GetCopyDefaultValues(string elementName, string activityName, DataPair<string> dataPair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:copy:CopyService:DefaultJob";
            string endpoint = "copy";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(dataPair.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, dataPair, activityUrn, endpoint, activityName);
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