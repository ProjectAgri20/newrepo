using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    public class PrintSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> PrintFromUsb { get; set; }

        [DataMember]
        public DataPair<string> Copies { get; set; }

        [DataMember]
        public DataPair<string> OriginalSize { get; set; }

        [DataMember]
        public DataPair<string> PaperType { get; set; }

        [DataMember]
        public DataPair<string> PaperTray { get; set; }

        [DataMember]
        public DataPair<string> OutputBin { get; set; }

        [DataMember]
        public DataPair<string> OutputSides { get; set; }

        [DataMember]
        public DataPair<string> Resolution { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public PrintSettingsData()
        {
            PrintFromUsb = new DataPair<string> { Key = string.Empty };
            Copies = new DataPair<string> { Key = String.Empty };
            OriginalSize = new DataPair<string> { Key = String.Empty };

            PaperType = new DataPair<string> { Key = String.Empty };
            PaperTray = new DataPair<string> { Key = String.Empty };

            OutputSides = new DataPair<string> { Key = String.Empty };
            OutputBin = new DataPair<string> { Key = String.Empty };

            Resolution = new DataPair<string> { Key = String.Empty };
        }
        /// <summary>
        /// Execution Entry point
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            Type type = typeof(PrintSettingsData);
            bool result = true;
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Printer))
            {
                _failedSettings.AppendLine("Device has no Print capability, skipping Print Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Print Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Print Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "PrintFromUsb":
                        result &= SetUsbDefaultValues("PrintFromUSBMassStorageEnabled", (DataPair<string>)item.Value, device, assetInfo, "Enable Print From USB", data);
                        break;

                    case "Copies":
                        result &= SetPrintDefaultValues("DefaultPrintCopies", (DataPair<string>)item.Value, device, assetInfo, "Default Print Copies", data);
                        break;

                    case "OriginalSize":
                        result &= SetPrintDefaultValues("MediaSizeID", (DataPair<string>)item.Value, device, assetInfo, "Original Size", data);
                        break;

                    case "PaperType":
                        result &= SetPrintDefaultValues("MediaType", (DataPair<string>)item.Value, device, assetInfo, "Paper Type", data);
                        break;

                    case "PaperTray":
                        result &= SetPrintDefaultValues("ManualFeed", (DataPair<string>)item.Value, device, assetInfo, "Paper Tray", data);
                        break;

                    case "OutputBin":
                        result &= SetPrintDefaultValues("DefaultOutputBin", (DataPair<string>)item.Value, device, assetInfo, "Output Bin", data);
                        break;

                    case "OutputSides":
                        result &= SetPrintDefaultValues("Plex", (DataPair<string>)item.Value, device, assetInfo, "Output Sides", data);
                        break;

                    case "Resolution":
                        result &= SetPrintDefaultValues("ResolutionSetting", (DataPair<string>)item.Value, device, assetInfo, "Resolution", data);
                        break;
                }
            }

            return result;
        }

        private bool SetPrintDefaultValues(string elementName, DataPair<string> dataPair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:print:PrintService:DefaultJob";
            string endpoint = "print";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement(elementName).SetValue(dataPair.Key);
                return n;
            };

            return UpdateField(change, device, dataPair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetUsbDefaultValues(string elementName, DataPair<string> dataPair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:openfromusb:OpenFromUsbService";
            string endpoint = "openfromusb";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement(elementName).SetValue(dataPair.Key);
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
                catch(Exception ex)
                {
                    Logger.LogError($"Failed to set field {fieldChanged}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {fieldChanged}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = fieldChanged;
                log.Result = success ? "Passed" : "Failed";
                log.Value = data.Key.ToString();
                log.ControlChanged = "Print Default";


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