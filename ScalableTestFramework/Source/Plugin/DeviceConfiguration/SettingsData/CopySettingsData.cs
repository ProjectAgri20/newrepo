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
    public class CopySettingsData : IComponentData
    {
        /// <summary>
        /// Number of Copies to Set.
        /// </summary>
        [DataMember]
        public DataPair<string> Copies { get; set; }
        /// <summary>
        /// Scaling Selection.
        /// </summary>
        [DataMember]
        public DataPair<string> ReduceEnlarge { get; set; }
        /// <summary>
        /// Copy Sides
        /// </summary>
        [DataMember]
        public DataPair<string> CopySides { get; set; }
        /// <summary>
        /// Mono/Color Selection.
        /// </summary>
        [DataMember]
        public DataPair<string> Color { get; set; }
        /// <summary>
        /// Scan Mode Default Selection.
        /// </summary>
        [DataMember]
        public DataPair<string> ScanMode { get; set; }
        /// <summary>
        /// Collation Default Selection.
        /// </summary>
        [DataMember]
        public DataPair<string> Collate { get; set; }
        /// <summary>
        /// Pages per Sheet Default Selection.
        /// </summary>
        [DataMember]
        public DataPair<string> PagesPerSheet { get; set; }

        [DataMember]
        public ScanSettings ScanSettingsData { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();
        /// <summary>
        /// Initializes an instance of CopySettings Data.
        /// </summary>
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
        /// <summary>
        /// Execution point
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            Type type = typeof(CopySettingsData);
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

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                _failedSettings.AppendLine("Device has no Scanner capability, skipping Copy Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Copy Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Copy Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "Copies":                  
                        result &= SetCopyDefaultValues("DefaultPrintCopies", (DataPair<string>)item.Value, device, assetInfo, "Default Print Copies", data);
                        break;

                    case "ReduceEnlarge":
                        {
                            var dataPair = new DataPair<string>
                            {
                                Key = "false",
                                Value = true
                            };
                            if (Convert.ToInt32(((DataPair<string>)item.Value).Key) == 100)
                            {
                                dataPair.Key = "true";
                            }
                           
                            result &= SetCopyDefaultValues("AutoScale",  dataPair, device, assetInfo, "Reduce Enlarge", data);
                            result &= SetCopyDefaultValues("Scaling", (DataPair<string>)item.Value, device, assetInfo, "Reduce Enlarge", data);
                        }
                        break;

                    case "CopySides":                    
                        result &= SetCopyDefaultValues("CopySides", (DataPair<string>)item.Value, device, assetInfo, "Copy Sides", data);
                        break;

                    case "Color":
                        result &= SetCopyDefaultValues("ChromaticMode", (DataPair<string>)item.Value, device, assetInfo, "Color", data);
                        break;

                    case "ScanMode":
                        result &= SetCopyDefaultValues("CaptureMode", (DataPair<string>)item.Value, device, assetInfo, "Scan Mode", data);
                        break;

                    case "Collate":
                        result &= SetCopyDefaultValues("SheetCollate", (DataPair<string>)item.Value, device, assetInfo, "Collate", data);
                        break;

                    case "PagesPerSheet":
                        result &= SetCopyDefaultValues("CopyOutputNumberUpCount", (DataPair<string>)item.Value, device, assetInfo, "Pages per Sheet", data);
                        break;

                    case "OriginalSize":
                        result &= SetCopyDefaultValues("ScanMediaSize", (DataPair<string>)item.Value, device, assetInfo, "Original Size", data);
                        break;

                    case "OriginalSides":
                        result &= SetCopyDefaultValues("ScanPlexMode", (DataPair<string>)item.Value, device, assetInfo, "Original Sides", data);
                        break;

                    case "Optimize":
                        result &= SetCopyDefaultValues("ScanMode", (DataPair<string>)item.Value, device, assetInfo, "Optimize Mode", data);
                        break;

                    case "ContentOrientation":
                        result &= SetCopyDefaultValues("OriginalContentOrientation", (DataPair<string>)item.Value, device, assetInfo, "Orientation", data);
                        break;

                    case "Cleanup":
                        result &= SetCopyDefaultValues("BackgroundRemoval", (DataPair<string>)item.Value, device, assetInfo, "Cleanup", data);
                        break;

                    case "Sharpness":
                        result &= SetCopyDefaultValues("Sharpness", (DataPair<string>)item.Value, device, assetInfo, "Sharpness", data);
                        break;

                    case "Darkness":
                        result &= SetCopyDefaultValues("Exposure", (DataPair<string>)item.Value, device, assetInfo, "Darkness", data);
                        break;

                    case "Contrast":
                        result &= SetCopyDefaultValues("Contrast", (DataPair<string>)item.Value, device, assetInfo, "Contrast", data);
                        break;

                    case "ImagePreview":
                        result &= SetCopyDefaultValues("ImagePreview", (DataPair<string>)item.Value, device, assetInfo, "Image Preview", data);
                        break;
                }
            }

            return result;
        }
        /// <summary>
        /// Sets Copy Default Values via WebTicket.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="dataPair"></param>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="fieldChanged"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SetCopyDefaultValues(string elementName,  DataPair<string> dataPair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:copy:CopyService:DefaultJob";
            string endpoint = "copy";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElements(elementName).FirstOrDefault().SetValue(dataPair.Key);
                return n;
            };

            return UpdateField(change, device, dataPair, activityUrn, endpoint, assetInfo, fieldChanged, data);
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
                log.ControlChanged = "Copy Default";

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
        public DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, PluginExecutionData pluginExecutionData)
        {
            _failedSettings = new StringBuilder();
            var result = ExecuteJob(device, assetInfo, pluginExecutionData);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}