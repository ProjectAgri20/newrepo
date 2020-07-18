using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;


namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class EmailSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> EnableScanToEmail { get; set; }
        [DataMember]
        public DataPair<string> SMTPServer { get; set; }
        [DataMember]
        public DataPair<string> FromUser { get; set; }
        [DataMember]
        public DataPair<string> DefaultFrom { get; set; }
        [DataMember]
        public DataPair<string> To { get; set; }
        [DataMember]
        public DataPair<string> OriginalSize { get; set; }
        [DataMember]
        public DataPair<string> OriginalSides { get; set; }
        [DataMember]
        public DataPair<string> ImagePreview { get; set; }
        [DataMember]
        public DataPair<string> FileType { get; set; }
        [DataMember]
        public DataPair<string> Resolution { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();
        public EmailSettingsData()
        {
            EnableScanToEmail = new DataPair<string> { Key = string.Empty };
            SMTPServer = new DataPair<string> { Key = string.Empty };
            FromUser = new DataPair<string> { Key = string.Empty };
            DefaultFrom = new DataPair<string> { Key = string.Empty };
            To = new DataPair<string> { Key = string.Empty };
            OriginalSize = new DataPair<string> { Key = string.Empty };
            OriginalSides = new DataPair<string> { Key = string.Empty };
            ImagePreview = new DataPair<string> { Key = string.Empty };
            FileType = new DataPair<string> { Key = string.Empty };
            Resolution = new DataPair<string> { Key = string.Empty };
        }

        /// <summary>
        /// Sets fields for EmailSettingsData
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool GetFields(JediDevice device)
        {
            Type type = typeof(EmailSettingsData);
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
                    case "EnableScanToEmail":
                        result &= SetEnableScanToEmail((DataPair<string>)item.Value, device);
                        break;
                    case "SMTPServer":
                        result &= SetSMTPServer((DataPair<string>)item.Value, device);
                        break;
                    case "FromUser":
                        result &= SetFromUser((DataPair<string>)item.Value, device);
                        break;
                    case "DefaultFrom":
                        result &= SetDefaultFrom((DataPair<string>)item.Value, device);
                        break;
                    case "To":
                        result &= SetTo((DataPair<string>)item.Value, device);
                        break;
                    case "OriginalSize":
                        result &= SetOriginalSize((DataPair<string>)item.Value, device);
                        break;
                    case "OriginalSides":
                        result &= SetOriginalSides((DataPair<string>)item.Value, device);
                        break;
                    case "ImagePreview":
                        result &= SetImagePreview((DataPair<string>)item.Value, device);
                        break;
                    case "FileType":
                        result &= SetFileType((DataPair<string>)item.Value, device);
                        break;
                    case "Resolution":
                        result &= SetResolution((DataPair<string>)item.Value, device);
                        break;
                    default:
                        throw new Exception("Field Property Error");
                }
            }

            return result;
        }

        private bool SetEnableScanToEmail(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService";
            string endpoint = "email";
            string name = "EnableScanToEmail";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement("SendToEmailEnabled").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
            
        }
        private bool SetSMTPServer(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService";
            string endpoint = "email";
            string name = "SMTPServer";

            Func<WebServiceTicket, bool> getProperty = n =>
            {
                bool result = true;
                bool useSsl;
                var values = pair.Key.Split(' ').ToList();
                if (values.Count == 3)
                    values.Add("false");
              

                bool.TryParse(values[3], out useSsl);
                result &= n.FindElements("NetworkID").Any(x=>x.Value == values[0]);
                result &= n.FindElements("Port").Any(x=>x.Value == values[1]);
                result &= n.FindElements("MaxAttachmentSize").Any(x => x.Value == values[2]);
                result &= n.FindElements("UseSSL").Any(x => x.Value.Equals(useSsl.ToString(), StringComparison.OrdinalIgnoreCase));
                return result;
            };
            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
        }
        private bool SetFromUser(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "FromUser";

            Func<WebServiceTicket, bool> getProperty = n =>
            {
                bool result = true;
                result &= n.FindElement("EmailAddress").Value ==pair.Key;
                result &= n.FindElement("DisplayName").Value==pair.Key;
                return result;
            };
            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
        }
        private bool SetDefaultFrom(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "DefaultFrom";

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                if (pair.Key.Contains("Default"))
                {
                    result &= n.FindElement("UseSignedInUserAsSender").Value=="disabled";
                    result &= n.FindElement("AddSenderAsToRecipient").Value=="disabled";
                }
                else
                {
                    result &= n.FindElement("UseSignedInUserAsSender").Value =="enabled";
                    result &= n.FindElement("AddSenderAsToRecipient").Value=="enabled";
                }
                return result;

            };
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetTo(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "To";

            Func<WebServiceTicket, bool> change = n =>
            {
                bool result = true;
                if (pair.Key.Contains("Blank"))
                {
                    result &= n.FindElement("UseSignedInUserAsSender").Value=="disabled";
                    result &= n.FindElement("AddSenderAsToRecipient").Value=="disabled";
                }
                else
                {
                    result &= n.FindElement("UseSignedInUserAsSender").Value =="enabled";
                    result &= n.FindElement("AddSenderAsToRecipient").Value =="enabled";
                }
                return result;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetOriginalSize(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "OriginalSize";

            Func<WebServiceTicket, bool> change = n => n.FindElement("ScanMediaSize").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetOriginalSides(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "OriginalSides";

            Func<WebServiceTicket, bool> change = n => n.FindElement("ScanPlexMode").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetImagePreview(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "ImagePreview";

            Func<WebServiceTicket, bool> change = n => n.FindElement("ImagePreview").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetFileType(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "FileType";
           
            Func<WebServiceTicket, bool> change = n => n.FindElement("DSFileType").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }
        private bool SetResolution(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";
            string name = "Resolution";

            
            Func<WebServiceTicket, bool> change = n => n.FindElement("DSImageResolution").Value.Replace(" ","").Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
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
            return new DataPair<string> {Key = _failedSettings.ToString(), Value = result};
        }
    }
}
