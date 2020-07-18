using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
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
        public bool GetFields(JediDevice device)
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

            //if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            //{
            //    _failedSettings.AppendLine("Device has no Scanner capability, skipping Fax Settings");

            //    DeviceConfigResultLog log =
            //        new DeviceConfigResultLog(data, assetInfo.AssetId)
            //        {
            //            FieldChanged = "Fax Settings",
            //            Result = "Skipped",
            //            Value = "NA",
            //            ControlChanged = "Fax Default"
            //        };

            //    ExecutionServices.DataLogger.Submit(log);
            //    return false;
            //}

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
                        result &= GetFaxDefaultValues("FaxServiceEnabled", (DataPair<string>)item.Value, device, "Enable Fax");
                        break;

                    case "FaxMethod":
                        result &= GetFaxDefaultValues("FaxMethod", (DataPair<string>)item.Value, device,  "Fax Send Method");
                        break;

                    case "ThirdPartyProduct":
                        result &= GetFaxDefaultValues("FaxVendorProductName", (DataPair<string>)item.Value, device, "Third Party Vendor");
                        break;

                    case "FileFormat":
                        result &= GetFaxDefaultValues("FaxFileFormat", (DataPair<string>)item.Value, device, "File Format");
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
                                result &= GetLanFaxDefaultValues(unc, domain, user, password, device, "Lan Fax Default Values");
                            }
                        }
                        break;

                    case "Location":
                        result &= GetAnalogFaxSettings("AnalogFaxCountry", (DataPair<string>)item.Value, device,
                             "Location");
                        break;

                    case "CompanyName":
                        result &= GetAnalogFaxSettings("CompanyName", (DataPair<string>)item.Value, device,  "Company Name" );
                        break;

                    case "FaxNumber":
                        result &= GetAnalogFaxSettings("FaxNumber", (DataPair<string>)item.Value, device,  "Fax Number");
                        break;

                    case "OriginalSize":
                        result &= GetFaxDefaultJobValues("ScanMediaSize", (DataPair<string>)item.Value, device,  "Scan Media Size");
                        break;

                    case "OriginalSides":
                        result &= GetFaxDefaultJobValues("ScanPlexMode", (DataPair<string>)item.Value, device,  "Original Sides");
                        break;

                    case "Optimize":
                        result &= GetFaxDefaultJobValues("ScanMode", (DataPair<string>)item.Value, device,  "Optimize");
                        break;

                    case "ContentOrientation":
                        result &= GetFaxDefaultJobValues("OriginalContentOrientation", (DataPair<string>)item.Value, device,  "Orientation");
                        break;

                    case "ImagePreview":
                        result &= GetFaxDefaultJobValues("ImagePreview", (DataPair<string>)item.Value, device,  "Image Preview");
                        break;

                    case "Cleanup":
                        result &= GetFaxDefaultJobValues("BackgroundRemoval", (DataPair<string>)item.Value, device,  "Cleanup");
                        break;

                    case "Sharpness":
                        result &= GetFaxDefaultJobValues("Sharpness", (DataPair<string>)item.Value, device,  "Sharpness");
                        break;

                    case "Darkness":
                        result &= GetFaxDefaultJobValues("Exposure", (DataPair<string>)item.Value, device,  "Darkness");
                        break;

                    case "Contrast":
                        result &= GetFaxDefaultJobValues("Contrast", (DataPair<string>)item.Value, device,  "Contrast");
                        break;

                    case "FaxResolution":
                        result &= GetFaxDefaultJobValues("FaxSendResolution", (DataPair<string>)item.Value, device,  "Fax Resolution");
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        private bool GetAnalogFaxSettings(string elementName, DataPair<string> itemValue, JediDevice device,string fieldChanged )
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:AnalogFaxSettings";
            string endpoint = "fax";

           
            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, fieldChanged);
        }

        private bool GetFaxDefaultValues(string elementName, DataPair<string> itemValue, JediDevice device, string fieldChanged)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService";
            string endpoint = "fax";
           

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, fieldChanged);
        }

        private bool GetFaxDefaultJobValues(string elementName, DataPair<string> itemValue, JediDevice device,  string fieldChanged)
        {
            string activityUrn = "urn:hp:imaging:con:service:fax:FaxService:DefaultJobs";
            string endpoint = "fax";
            Func<WebServiceTicket, bool> getProperty = n =>n.FindElement(elementName).Value.Equals(itemValue.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, itemValue, activityUrn, endpoint, fieldChanged);
        }

        private bool GetLanFaxDefaultValues(string unc, string domain, string username, string password, JediDevice device, string fieldChanged)
        {
            bool success= true;
            string endpoint = "fax";
            string urn = "urn:hp:imaging:con:service:fax:FaxService:ServiceDefaults";
            
            try
            {
                WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                //Change values

                success &= tic.FindElement("FaxMethod").Value.Equals("lanFaxService");
                success &= tic.FindElement("UserName").Value.Equals(username, StringComparison.OrdinalIgnoreCase);
                success &= tic.FindElement("UNCPath").Value.Equals(unc, StringComparison.OrdinalIgnoreCase);
                success &= tic.FindElement("DomainName").Value.Equals(domain, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to set field {fieldChanged}, {ex.Message}");
                success = false;
            }
           
            if(!success)
                _failedSettings.AppendLine($"Failed to set field {fieldChanged}");

            return success;
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
       

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName)
        {
            bool success;

            if (data != null && data.Value)
            {
                
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    getProperty(tic);
                    device.WebServices.PutDeviceTicket(endpoint, urn, tic);
                    success = true;
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
            if (!success)
            {
                _failedSettings.Append($"{activityName}, ");
            }

            return success;
        }
    }
}