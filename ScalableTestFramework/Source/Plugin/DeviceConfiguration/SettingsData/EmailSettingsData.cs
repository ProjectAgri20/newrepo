using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System.Text;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class EmailSettingsData : IComponentData
    {
        //[Veda]:shuffled the order of properties, in WJ, the SMTP Server and From User needs to be configured before enabling ScanToEmail, just changing the order of
        //properties works for execution. this shouldn't affect any existing metadata
        [DataMember]
        public DataPair<string> SMTPServer { get; set; }

        [DataMember]
        public DataPair<string> FromUser { get; set; }
        [DataMember]
        public DataPair<string> DefaultFrom { get; set; }

        [DataMember]
        public DataPair<string> EnableScanToEmail { get; set; }
       
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
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            Type type = typeof(EmailSettingsData);
            bool result = true;
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            if (!assetInfo.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                _failedSettings.AppendLine("Device has no Scanner capability, skipping Email Settings");

                DeviceConfigResultLog log =
                    new DeviceConfigResultLog(data, assetInfo.AssetId)
                    {
                        FieldChanged = "Email Settings",
                        Result = "Skipped",
                        Value = "NA",
                        ControlChanged = "Email Default"
                    };

                ExecutionServices.DataLogger.Submit(log);
                return false;
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "EnableScanToEmail":
                        result &= SetEnableScanToEmail((DataPair<string>)item.Value, device, assetInfo, "EnableScanToEmail", data);
                        break;
                    case "SMTPServer":
                        result &= SetSMTPServer((DataPair<string>)item.Value, device, assetInfo, "SMTPServer", data);
                        break;
                    case "FromUser":
                        result &= SetFromUser((DataPair<string>)item.Value, device, assetInfo, "FromUser", data);
                        break;
                    case "DefaultFrom":
                        result &= SetDefaultFrom((DataPair<string>)item.Value, device, assetInfo, "DefaultFrom", data);
                        break;
                    case "To":
                        result &= SetTo((DataPair<string>)item.Value, device, assetInfo, "To", data);
                        break;
                    case "OriginalSize":
                        result &= SetOriginalSize((DataPair<string>)item.Value, device, assetInfo, "OriginalSize", data);
                        break;
                    case "OriginalSides":
                        result &= SetOriginalSides((DataPair<string>)item.Value, device, assetInfo, "OriginalSides", data);
                        break;
                    case "ImagePreview":
                        result &= SetImagePreview((DataPair<string>)item.Value, device, assetInfo, "ImagePreview", data);
                        break;
                    case "FileType":
                        result &= SetFileType((DataPair<string>)item.Value, device, assetInfo, "FileType", data);
                        break;
                    case "Resolution":
                        result &= SetResolution((DataPair<string>)item.Value, device, assetInfo, "Resolution", data);
                        break;
                    default:
                        throw new Exception("Field Property Error");
                }

            }

            return result;
        }

        private bool SetEnableScanToEmail(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("SendToEmailEnabled").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetSMTPServer(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService";
            string endpoint = "email";

            //for the initial option, there wouldn't be any smtp node, and the following code will crash 

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var values = pair.Key.Split(' ').ToList();
                if (values.Count == 3)
                    values.Add("false");

                if (n.Element("NetworkID") == null)
                {
                    AddSmtpServer(n, values);
                }
                else
                {
                    bool useSsl;
                    bool.TryParse(values[3], out useSsl);
                    n.FindElement("NetworkID").SetValue(values[0]);
                    n.FindElement("Port").SetValue(values[1]);
                    n.FindElement("MaxAttachmentSize").SetValue(values[2]);
                    n.FindElement("UseSSL").SetValue(useSsl.ToString().ToLower());
                    n.FindElement("ValidateServerCertificate").SetValue("disabled");
                }
               
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private void AddSmtpServer(WebServiceTicket webTicket, List<string> dataStrings)
        {
            bool useSsl;
            bool.TryParse(dataStrings[3], out useSsl);
            var dsd = webTicket.GetNamespaceOfPrefix("dsd");
            var dd = webTicket.GetNamespaceOfPrefix("dd");
            XNamespace securityns = XNamespace.Get("http://www.hp.com/schemas/imaging/con/security/2009/02/11");
            XElement smtpserverSettings = new XElement(dsd + "SMTPServerSettings",
                new XElement(dd + "NetworkID", dataStrings[0]),
                new XElement(dd + "Port", dataStrings[1]),
                new XElement(dd + "MaxAttachmentSize", dataStrings[2]),
                new XElement(securityns + "AuthenticationSettings", new XElement("CredentialType", "hp_simple_v1"), new XElement("SimpleAuthenticationSettings", new XElement(dd + "PasswordStatus", "notSet"))),
                new XElement(dd + "UseMFPUsersCredentials", "true"),
                new XElement(dd + "UseSSL", useSsl.ToString().ToLower()),
                new XElement(dsd + "ValidateServerCertificate", "disabled"));
            webTicket.Element(dsd + "SMTPServersWithSettings").Add(smtpserverSettings);

        }
        private bool SetFromUser(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("EmailAddress").SetValue(pair.Key);
                n.FindElement("DisplayName").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetDefaultFrom(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                if (pair.Key.Contains("Default"))
                {
                    n.FindElement("UseSignedInUserAsSender").SetValue("disabled");
                    n.FindElement("AddSenderAsToRecipient").SetValue("disabled");
                }
                else
                {
                    n.FindElement("UseSignedInUserAsSender").SetValue("enabled");
                    n.FindElement("AddSenderAsToRecipient").SetValue("enabled");
                }
                return n;

            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetTo(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                if (pair.Key.Contains("Blank"))
                {
                    n.FindElement("UseSignedInUserAsSender").SetValue("disabled");
                    n.FindElement("AddSenderAsToRecipient").SetValue("disabled");
                }
                else
                {
                    n.FindElement("UseSignedInUserAsSender").SetValue("enabled");
                    n.FindElement("AddSenderAsToRecipient").SetValue("enabled");
                }
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetOriginalSize(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("ScanMediaSize").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetOriginalSides(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("ScanPlexMode").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetImagePreview(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("ImagePreview").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetFileType(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("DSFileType").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetResolution(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:email:EmailService:DefaultJob";
            string endpoint = "email";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                string temp = pair.Key.Replace(" ", "");
                n.FindElement("DSImageResolution").SetValue(temp);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
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
                catch(Exception ex)
                {
                    Logger.LogError($"Failed to set field {fieldChanged}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {fieldChanged}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = fieldChanged;
                log.Result = success ? "Passed" : "Failed";
                log.Value = data.Key.ToString();
                log.ControlChanged = "Email Default";

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
