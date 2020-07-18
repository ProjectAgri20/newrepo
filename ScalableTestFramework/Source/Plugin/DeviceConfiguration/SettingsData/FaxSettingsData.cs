using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class FaxSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> EnableFax { get; set; }

        [DataMember]
        public DataPair<string> ThirdPartyProduct { get; set; }

        [DataMember]
        public DataPair<string> FaxMethod { get; set; }

        [DataMember]
        public DataPair<string> FileFormat { get; set; }

        [DataMember]
        public DataPair<string> UNCFolderPath { get; set; }

        [DataMember]
        public DataPair<string> DomainName { get; set; }

        [DataMember]
        public DataPair<string> UserName { get; set; }

        [DataMember]
        public DataPair<string> Password { get; set; }

        [DataMember]
        public DataPair<string> Location { get; set; }

        [DataMember]
        public DataPair<string> CompanyName { get; set; }

        [DataMember]
        public DataPair<string> FaxNumber { get; set; }

        #region ScanSettings

        [DataMember]
        public ScanSettings ScanSettingsData { get; set; }

        [DataMember]
        public DataPair<string> FaxResolution { get; set; }

        #endregion ScanSettings

        private StringBuilder _failedSettings = new StringBuilder();

        public FaxSettingsData()
        {
            EnableFax = new DataPair<string> { Key = String.Empty };
            ThirdPartyProduct = new DataPair<string> { Key = String.Empty };
            FaxMethod = new DataPair<string> { Key = String.Empty };
            FileFormat = new DataPair<string> { Key = String.Empty };
            UNCFolderPath = new DataPair<string> { Key = String.Empty };
            DomainName = new DataPair<string> { Key = String.Empty };
            UserName = new DataPair<string> { Key = String.Empty };
            Password = new DataPair<string> { Key = String.Empty };
            Location = new DataPair<string> { Key = string.Empty };
            CompanyName = new DataPair<string> { Key = string.Empty };
            FaxNumber = new DataPair<string> { Key = string.Empty };
            ScanSettingsData = new ScanSettings();
            FaxResolution = new DataPair<string> { Key = String.Empty };
        }

        //[OnDeserializing]
        //public void ResetData(StreamingContext context)
        //{
        //    FaxResolution = new DataPair<string> { Key = String.Empty };
        //}
        /// <summary>
        /// Execution entry point
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            Type type = typeof(FaxSettingsData);
            bool result = true;
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties().Where(x => x.PropertyType == typeof(DataPair<string>)))
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }
            foreach (PropertyInfo propertyInfo in typeof(ScanSettings).GetProperties())
            {
                properties.Add(propertyInfo.Name, propertyInfo.GetValue(ScanSettingsData));
            }

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                _failedSettings.AppendLine("Device has no Scanner capability, skipping Fax Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Fax Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Fax Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            //int fieldCount = 0;
            string unc = "";
            string domain = "";
            string user = "";
            string password = "";
            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableFax":
                        result &= SetFaxDefaultValues("FaxServiceEnabled", (DataPair<string>)item.Value, device, assetInfo, "Enable Fax", data);
                        break;

                    case "FaxMethod":
                        result &= SetFaxDefaultValues("FaxMethod", (DataPair<string>)item.Value, device, assetInfo, "Fax Send Method", data);
                        break;

                    case "ThirdPartyProduct":
                        result &= SetFaxDefaultValues("FaxVendorProductName", (DataPair<string>)item.Value, device, assetInfo, "Third Party Vendor", data);
                        break;

                    case "FileFormat":
                        result &= SetFaxDefaultValues("FaxFileFormat", (DataPair<string>)item.Value, device, assetInfo, "File Format", data);
                        break;

                    case "UNCFolderPath":
                    case "DomainName":
                    case "UserName":
                    case "Password":
                        ///Find a way to implement the 4 fields at once
                        ///
                        var datapair = (DataPair<string>)item.Value;
                        if (datapair.Value)
                        {
                            switch (item.Key)
                            {
                                case "UNCFolderPath":
                                    unc = datapair.Key;
                                    break;

                                case "DomainName":
                                    domain = datapair.Key;
                                    break;

                                case "UserName":
                                    user = datapair.Key;
                                    break;

                                case "Password":
                                    password = datapair.Key;
                                    break;
                            }

                            if (!string.IsNullOrWhiteSpace(unc) && !string.IsNullOrWhiteSpace(domain) && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                            {
                                result &= SetLanFaxDefaultValues(unc, domain, user, password, device, assetInfo, "Lan Fax Default Values", data);
                            }
                        }
                        break;

                    case "Location":
                        result &= SetAnalogFaxSettings("AnalogFaxCountry", (DataPair<string>)item.Value, device,
                            assetInfo, "Location", data);
                        break;

                    case "CompanyName":
                        result &= SetAnalogFaxSettings("CompanyName", (DataPair<string>)item.Value, device, assetInfo, "Company Name", data);
                        break;

                    case "FaxNumber":
                        result &= SetAnalogFaxSettings("FaxNumber", (DataPair<string>)item.Value, device, assetInfo, "Fax Number", data);
                        break;

                    case "OriginalSize":
                        result &= SetFaxDefaultJobValues("ScanMediaSize", (DataPair<string>)item.Value, device, assetInfo, "Scan Media Size", data);
                        break;

                    case "OriginalSides":
                        result &= SetFaxDefaultJobValues("ScanPlexMode", (DataPair<string>)item.Value, device, assetInfo, "Original Sides", data);
                        break;

                    case "Optimize":
                        result &= SetFaxDefaultJobValues("ScanMode", (DataPair<string>)item.Value, device, assetInfo, "Optimize", data);
                        break;

                    case "ContentOrientation":
                        result &= SetFaxDefaultJobValues("OriginalContentOrientation", (DataPair<string>)item.Value, device, assetInfo, "Orientation", data);
                        break;

                    case "ImagePreview":
                        result &= SetFaxDefaultJobValues("ImagePreview", (DataPair<string>)item.Value, device, assetInfo, "Image Preview", data);
                        break;

                    case "Cleanup":
                        result &= SetFaxDefaultJobValues("BackgroundRemoval", (DataPair<string>)item.Value, device, assetInfo, "Cleanup", data);
                        break;

                    case "Sharpness":
                        result &= SetFaxDefaultJobValues("Sharpness", (DataPair<string>)item.Value, device, assetInfo, "Sharpness", data);
                        break;

                    case "Darkness":
                        result &= SetFaxDefaultJobValues("Exposure", (DataPair<string>)item.Value, device, assetInfo, "Darkness", data);
                        break;

                    case "Contrast":
                        result &= SetFaxDefaultJobValues("Contrast", (DataPair<string>)item.Value, device, assetInfo, "Contrast", data);
                        break;

                    case "FaxResolution":
                        result &= SetFaxDefaultJobValues("FaxSendResolution", (DataPair<string>)item.Value, device, assetInfo, "Fax Resolution", data);
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        private bool SetAnalogFaxSettings(string elementName, DataPair<string> itemValue, JediDevice device,
            AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:AnalogFaxSettings";
            string endpoint = "fax";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                if (string.IsNullOrEmpty(n.FindElement("CompanyName").Value))
                {
                    n.FindElement("CompanyName").SetValue("PlaceHolder");
                }

                if (string.IsNullOrEmpty(n.FindElement("FaxNumber").Value))
                {
                    n.FindElement("FaxNumber").SetValue("420");
                }

                n.FindElement(elementName).SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetFaxDefaultValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService";
            string endpoint = "fax";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElements(elementName).FirstOrDefault().SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetFaxDefaultJobValues(string elementName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:DefaultJobs";
            string endpoint = "fax";
            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement(elementName).SetValue(itemValue.Key);
                return n;
            };

            return UpdateField(change, device, itemValue, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetLanFaxDefaultValues(string unc, string domain, string username, string password, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            bool success = false;
            string endpoint = "fax";
            string urn = "urn:hp:imaging:con:service:fax:FaxService:ServiceDefaults";
            DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
            try
            {
                WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                //Change values

                tic.FindElement("FaxMethod").SetValue("lanFaxService");
                tic.FindElement("UserName").SetValue(username);
                tic.FindElement("UNCPath").SetValue(unc);
                tic.FindElement("DomainName").SetValue(domain);

                string ticketString = tic.ToString();
                string insertString = $@"<dd:Password>{password}</dd:Password>";
                string searchString = @"</dd:UserName>";

                int foundLocation = ticketString.IndexOf(searchString);
                ticketString = ticketString.Insert(foundLocation + searchString.Length, Environment.NewLine + insertString);
                tic = new WebServiceTicket(ticketString);

                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
                success = true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to set field {log.FieldChanged}, {ex.Message}");
                _failedSettings.AppendLine($"Failed to set field {log.FieldChanged}, {ex.Message}");
                success = false;
            }
            log.FieldChanged = fieldChanged;
            log.Result = success ? "Passed" : "Failed";
            log.Value = $@"Method:Lanfax UserName:{username} Domain:{domain}";
            log.ControlChanged = "Fax Default";

            ExecutionServices.DataLogger.Submit(log);
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> ChangeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            bool success;

            if (data != null && data.Value)
            {
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    ChangeValue(tic);
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
                log.ControlChanged = "Fax Default";

                ExecutionServices.DataLogger.Submit(log);
            }
            else
            {
                success = true;
            }
            return success;
        }
    }
}