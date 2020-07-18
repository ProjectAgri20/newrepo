using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
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

        private StringBuilder _failedSettings= new StringBuilder();
        public JobSettingsData()
        {
            EnableJobStorage = new DataPair<string> { Key = string.Empty };
            MinLengthPin = new DataPair<string> { Key = string.Empty };
            JobsPinRequired = new DataPair<string> { Key = string.Empty };
            DefaultFolderName = new DataPair<string> { Key = string.Empty };
            RetainJobs = new DataPair<string> { Key = string.Empty };
        }

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(JobSettingsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableJobStorage":
                        result &= SetJobStorageDefaultValues("StoredJobsEnabled", "Enable JobStorage", (DataPair<string>)item.Value, device);
                        break;

                    case "MinLengthPin":
                        result &= SetJobStorageDefaultValues("PINLength", "MinLengthPin", (DataPair<string>)item.Value, device);
                        break;

                    case "JobsPinRequired":
                        result &= SetJobStorageDefaultValues("SaveToDeviceMemoryJobsPinRequired", "Jobs Pin Requirement", (DataPair<string>)item.Value, device);
                        break;

                    case "DefaultFolderName":
                        result &= SetJobStorageDefaultValues("PublicFolderName", "Folder Name", (DataPair<string>)item.Value, device);
                        break;

                    case "RetainJobs":
                        result &= SetJobStorageDefaultValues("RetainTemporaryJobsMode", "Job Retention", (DataPair<string>)item.Value, device);
                        break;
                }
            }

            return result;
        }

        private bool SetJobStorageDefaultValues(string elementName, string propertyName, DataPair<string> itemValue, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:jobmanager:JobManagerService";
            string endpoint = "jobmanager";

            Func<WebServiceTicket, bool> change = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(change, device, itemValue, activityUrn, endpoint, propertyName);
        }

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName)
        {
            bool success;
            if (data.Value)
            {
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    success =getProperty(tic);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {activityName}, {ex.Message}");
                    success = false;
                }
            }
            else
            {
                success = true;
            }
            if(!success)
                _failedSettings.Append($"{activityName}, ");

            return success;
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> {Key = _failedSettings.ToString(), Value = result};
        }
    }
}