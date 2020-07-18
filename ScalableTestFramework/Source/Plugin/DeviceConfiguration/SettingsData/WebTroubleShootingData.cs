using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Web ToubleShotting Data
    /// </summary>
    [DataContract]
    public class WebTroubleShootingData : IComponentData
    {
        [DataMember]
        public DataPair<string> ServerName { get; set; }

        [DataMember]
        public DataPair<string> ServerPort { get; set; }

        [DataMember]
        public DataPair<string> ProxyExceptionList { get; set; }

        [DataMember]
        public DataPair<string> AutoRecoveryMode { get; set; }

        [DataMember]
        public DataPair<string> SyslogServer { get; set; }

        [DataMember]
        public DataPair<string> SyslogPort { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public WebTroubleShootingData()
        {
            ServerName = new DataPair<string> { Key = string.Empty };
            ServerPort = new DataPair<string> { Key = string.Empty };
            ProxyExceptionList = new DataPair<string> { Key = string.Empty };
            AutoRecoveryMode = new DataPair<string> { Key = string.Empty };
            SyslogServer = new DataPair<string> { Key = string.Empty };
            SyslogPort = new DataPair<string> { Key = string.Empty };
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
            Type type = typeof(WebTroubleShootingData);
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
                    case "ServerName":
                        result &= SetServerName((DataPair<string>)item.Value, device, assetInfo, "Server Name", data);
                        break;

                    case "ServerPort":
                        result &= SetServerPort((DataPair<string>)item.Value, device, assetInfo, "Server Port", data);
                        break;

                    case "ProxyExceptionList":
                        result &= SetProxyList((DataPair<string>)item.Value, device, assetInfo, "Proxy Exception List", data);
                        break;

                    case "AutoRecoveryMode"://not sure why a string was passed for a validation when that is the only field
                        result &= SetAutoRecoveryMode((DataPair<string>)item.Value, device, assetInfo, "Auto Recovery Mode", data);
                        break;

                    case "SyslogServer":
                        result &= SetSyslogServer((DataPair<string>)item.Value, device, assetInfo, "Syslog Server", data);
                        break;

                    case "SyslogPort":
                        result &= SetSyslogPort((DataPair<string>)item.Value, device, assetInfo, "Syslog Port", data);
                        break;

                    default:
                        throw new Exception("Field Set Property Error");
                }
            }

            return result;
        }

        public bool SetSyslogServer(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string mainOID = "1.3.6.1.4.1.11.2.4.3.5.5.0";
            //string enableOID = "1.3.6.1.4.1.11.2.4.3.7.26.0";
            Action<string> change = n =>
            {
                //device.Snmp.Set(enableOID, 0);

                var snmpvalue = new HP.DeviceAutomation.SnmpOidValue(mainOID, pair.Key, 64);
                device.Snmp.Set(snmpvalue);
            };
            return UpdateWithOid(change, device, pair, assetInfo, fieldChanged, data);
        }

        public bool SetSyslogPort(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string mainOID = "1.3.6.1.4.1.11.2.4.3.5.98.0";
            Action<string> change = n =>
            {
                int number;

                int.TryParse(pair.Key, out number);

                device.Snmp.Set(mainOID, number);
            };
            return UpdateWithOid(change, device, pair, assetInfo, fieldChanged, data);
        }

        public bool SetServerName(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string oid = "1.3.6.1.4.1.11.2.4.3.18.12.0";
            Action<string> change = n =>
            {
                device.Snmp.Set(oid, pair.Key);
            };
            return UpdateWithOid(change, device, pair, assetInfo, fieldChanged, data);
        }

        public bool SetServerPort(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string oid = "1.3.6.1.4.1.11.2.4.3.18.13.0";
            Action<string> change = n =>
            {
                int number;

                int.TryParse(pair.Key, out number);
                device.Snmp.Set(oid, number);
            };
            return UpdateWithOid(change, device, pair, assetInfo, fieldChanged, data);
        }

        public bool SetProxyList(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string oid = "1.3.6.1.4.1.11.2.4.3.18.16.0";
            Action<string> change = n =>
            {
                device.Snmp.Set(oid, pair.Key);
            };
            return UpdateWithOid(change, device, pair, assetInfo, fieldChanged, data);
        }

        public bool SetAutoRecoveryMode(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:systemconfiguration:SystemConfigurationService:DeviceInformation";
            string endpoint = "systemconfiguration";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                string enabled = pair.Key == "true" ? "enabled" : "disabled";
                n.FindElement("AutoRestartAfterUnrecoverableFirmwareFault").SetValue(enabled);
                return n;
            };

            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        /// <summary>
        /// Only used for AutoRestartAfterUnrecoverableFirmwareFault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="changeValue"></param>
        /// <param name="device"></param>
        /// <param name="data"></param>
        /// <param name="urn"></param>
        /// <param name="endpoint"></param>
        /// <param name="assetInfo"></param>
        /// <param name="fieldChanged"></param>
        /// <param name="pluginData"></param>
        /// <returns></returns>
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
                log.ControlChanged = @"Web/TroubleShooting";

                ExecutionServices.DataLogger.Submit(log);
            }
            else
            {
                success = true;
            }

            return success;
        }

        public bool UpdateWithOid<T>(Action<T> oidUsed, JediDevice device, DataPair<T> pair, AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            if (pair.Value)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                bool success = true;
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
                try
                {
                    var snmpData = GetCdm(device); //incase we have issues with endpoint, it is better to fail the setting than to abruptly stop the execution of the plugin
                    if (!string.IsNullOrEmpty(snmpData))
                    {
                        var objectGraph = serializer.DeserializeObject(snmpData) as Dictionary<string, object>;
                        var snmpEnabled = (string)objectGraph["snmpv1v2Enabled"];
                        if (snmpEnabled == "true")
                        {
                            var accessOption = (string)objectGraph["accessOption"];

                            //We need to change from read only
                            if (accessOption == "readOnly")
                            {
                                string jsonContent =
                                    @"{""snmpv1v2Enabled"": ""true"",""accessOption"": ""readWrite"",""readOnlyPublicAllowed"": ""true"",""readOnlyCommunityNameSet"": ""false"",""writeOnlyCommunitryNameSet"": ""false""}";

                                PutCdm(device, jsonContent);
                            }
                        }
                        //snmpv1v2 is disabled we need to enable it
                        else
                        {
                            string jsonContent =
                                @"{""snmpv1v2Enabled"": ""true"",""accessOption"": ""readWrite"",""readOnlyPublicAllowed"": ""true"",""readOnlyCommunityNameSet"": ""false"",""writeOnlyCommunitryNameSet"": ""false""}";
                            PutCdm(device, jsonContent);
                        }
                    }

                    oidUsed(pair.Key);

                    if (!string.IsNullOrEmpty(snmpData))
                    {
                        //Restore state
                        PutCdm(device, snmpData);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {log.FieldChanged}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {log.FieldChanged}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = fieldChanged;
                log.Result = success ? "Passed" : "Failed";
                log.Value = pair.Key.ToString();
                log.ControlChanged = "Web/TroubleShooting";

                ExecutionServices.DataLogger.Submit(log);
            }
            return true;
        }

        /// <summary>
        /// Interface function to drive setting of data fields and return results upstream
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns>result</returns>
        public DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            _failedSettings = new StringBuilder();
            var result = ExecuteJob(device, assetInfo, data);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }

        public static string GetCdm(JediDevice device)
        {
            string final = $@"https://{device.Address}/hp/network/ioConfig/v1/networkInterfaces/wired1/snmpv1v2Config";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(final);
            request.Method = "GET";
            request.Credentials = new NetworkCredential("admin", device.AdminPassword);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;

                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    //assetId errorText
                    Logger.LogError(errorText);
                }

                return string.Empty;
            }
        }

        public static void PutCdm(JediDevice device, string jsonContent)
        {
            string final = $@"https://{device.Address}/hp/network/ioConfig/v1/networkInterfaces/wired1/snmpv1v2Config";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(final);
            request.Method = "PUT";
            request.Credentials = new NetworkCredential("admin", device.AdminPassword); //this was hardcoded and has been changed accordingly

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            UTF8Encoding encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //Stream receiveStream = response.GetResponseStream(); not sure why we have to get response stream
                    Console.WriteLine(response.StatusCode);
                }
            }
            catch (WebException ex)
            {
                Logger.LogError(ex.Message);
                throw ex;
                // Log exception and throw as for GET example above
            }
        }
    }
}