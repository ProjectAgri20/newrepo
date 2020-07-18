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
    public class JobSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> EnableJobStorage { get; set; }

        [DataMember]
        public DataPair<string> MinLengthPin { get; set; }

        [DataMember]
        public DataPair<string> JobsPinRequired { get; set; }

        [DataMember]
        public DataPair<string> DefaultFolderName { get; set; }

        [DataMember]
        public DataPair<string> RetainJobs { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public JobSettingsData()
        {
            EnableJobStorage = new DataPair<string> { Key = string.Empty };
            MinLengthPin = new DataPair<string> { Key = string.Empty };
            JobsPinRequired = new DataPair<string> { Key = string.Empty };
            DefaultFolderName = new DataPair<string> { Key = string.Empty };
            RetainJobs = new DataPair<string> { Key = string.Empty };
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
            Type type = typeof(JobSettingsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Printer))
            {
                _failedSettings.AppendLine("Device has no Print capability, skipping Job Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Job Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Job Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableJobStorage":
                        result &= SetJobStorageDefaultValues("StoredJobsEnabled", (DataPair<string>)item.Value, device, assetInfo, "Enable JobStorage", data);
                        break;

                    case "MinLengthPin":
                        result &= SetJobStorageDefaultValues("PINLength", (DataPair<string>)item.Value, device, assetInfo, "MinLengthPin", data);
                        break;

                    case "JobsPinRequired":
                        result &= SetJobStorageDefaultValues("SaveToDeviceMemoryJobsPinRequired", (DataPair<string>)item.Value, device, assetInfo, "Jobs Pin Requirement", data);
                        break;

                    case "DefaultFolderName":
                        result &= SetJobStorageDefaultValues("PublicFolderName", (DataPair<string>)item.Value, device, assetInfo, "Folder Name", data);
                        break;

                    case "RetainJobs":
                        result &= SetJobStorageDefaultValues("RetainTemporaryJobsMode", (DataPair<string>)item.Value, device, assetInfo, "Job Retention", data);
                        break;
                }
            }

            return result;
        }

        private bool SetJobStorageDefaultValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:jobmanager:JobManagerService";
            string endpoint = "jobmanager";

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
                log.ControlChanged = "Job Storage";


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