using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Management Protocol Settings Data container
    /// </summary>
    public class ProtocolSettingsData : IComponentData
    {
        /// <summary>
        /// 9100 Printing
        /// </summary>
        [DataMember]
        public DataPair<string> P9100 { get; set; }

        /// <summary>
        /// LPD Printing
        /// </summary>
        [DataMember]
        public DataPair<string> Lpd { get; set; }

        /// <summary>
        /// Web Services Printing
        /// </summary>
        [DataMember]
        public DataPair<string> WSPrint { get; set; }

        /// <summary>
        /// FTP Printing
        /// </summary>
        [DataMember]
        public DataPair<string> Ftp { get; set; }

        /// <summary>
        /// SLP discovery protocol
        /// </summary>
        [DataMember]
        public DataPair<string> Slp { get; set; }

        /// <summary>
        /// Bonjour discovery protocol
        /// </summary>
        [DataMember]
        public DataPair<string> Bonjour { get; set; }

        /// <summary>
        /// Multicast IPV4 discovery protocol
        /// </summary>
        [DataMember]
        public DataPair<string> MultiCast { get; set; }

        /// <summary>
        /// WS Discovery protocol
        /// </summary>
        [DataMember]
        public DataPair<string> WSDiscovery { get; set; }

        /// <summary>
        /// LLMNR Name Resolution
        /// </summary>
        [DataMember]
        public DataPair<string> Llmnr { get; set; }

        /// <summary>
        /// Enables WINS Port Name Resolution
        /// </summary>
        [DataMember]
        public DataPair<string> WinsPort { get; set; }

        /// <summary>
        /// Enables WINS Registration Name Resolution
        /// </summary>
        [DataMember]
        public DataPair<string> WinsRegistration { get; set; }

        /// <summary>
        /// Enables Telnet
        /// </summary>
        [DataMember]
        public DataPair<string> Telnet { get; set; }

        /// <summary>
        /// Enables TFTP Configuration File
        /// </summary>
        [DataMember]
        public DataPair<string> Tftp { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public ProtocolSettingsData()
        {
            P9100 = new DataPair<string>() { Key = string.Empty };
            Lpd = new DataPair<string>() { Key = string.Empty };
            WSPrint = new DataPair<string>() { Key = string.Empty };
            Ftp = new DataPair<string>() { Key = string.Empty };
            Slp = new DataPair<string>() { Key = string.Empty };
            Bonjour = new DataPair<string>() { Key = string.Empty };
            MultiCast = new DataPair<string>() { Key = string.Empty };
            WSDiscovery = new DataPair<string>() { Key = string.Empty };
            Llmnr = new DataPair<string>() { Key = string.Empty };
            WinsPort = new DataPair<string>() { Key = string.Empty };
            WinsRegistration = new DataPair<string>() { Key = string.Empty };
            Telnet = new DataPair<string>() { Key = string.Empty };
            Tftp = new DataPair<string>() { Key = string.Empty };
        }

        /// <summary>
        /// Execution Entry point
        /// Individual function differences separated into delagate methods.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData pluginExecutionData)
        {
            Type type = typeof(ProtocolSettingsData);
            bool result = true;
            Dictionary<string, object> properties = new Dictionary<string, object>();

            //we have to set SnmpV1V2 to read/write else this won't work.
            SetSnmpReadWrite(device);

            foreach (PropertyInfo prop in type.GetProperties().Where(x => x.PropertyType == typeof(DataPair<string>)))
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "P9100":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.24.0", (DataPair<string>)item.Value, device, assetInfo, "P9100", pluginExecutionData);
                        }
                        break;

                    case "Lpd":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.22.0", (DataPair<string>)item.Value, device, assetInfo, "Lpd", pluginExecutionData);
                        }
                        break;

                    case "WSPrint":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.37.0", (DataPair<string>)item.Value, device, assetInfo, "WSPrint", pluginExecutionData);
                        }
                        break;

                    case "Ftp":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.20.0", (DataPair<string>)item.Value, device, assetInfo, "Ftp", pluginExecutionData);
                        }
                        break;

                    case "Slp":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.21.0", (DataPair<string>)item.Value, device, assetInfo, "Slp", pluginExecutionData);
                        }
                        break;

                    case "Bonjour":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.29.0", (DataPair<string>)item.Value, device, assetInfo, "Bonjour", pluginExecutionData);
                        }
                        break;

                    case "MultiCast":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.30.0", (DataPair<string>)item.Value, device, assetInfo, "MultiCast", pluginExecutionData);
                        }
                        break;

                    case "WSDiscovery":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.36.0", (DataPair<string>)item.Value, device, assetInfo, "WSDiscovery", pluginExecutionData);
                        }
                        break;

                    case "Llmnr":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.38.0", (DataPair<string>)item.Value, device, assetInfo, "Llmnr", pluginExecutionData);
                        }
                        break;

                    case "WinsPort":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.41.0", (DataPair<string>)item.Value, device, assetInfo, "WinsPort", pluginExecutionData);
                        }
                        break;

                    case "WinsRegistration":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.42.0", (DataPair<string>)item.Value, device, assetInfo, "WinsRegistration", pluginExecutionData);
                        }
                        break;

                    case "Telnet":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.19.0", (DataPair<string>)item.Value, device, assetInfo, "Telnet", pluginExecutionData);
                        }
                        break;

                    case "Tftp":
                        {
                            result &= SetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.44.0", (DataPair<string>)item.Value, device, assetInfo, "Tftp", pluginExecutionData);
                        }
                        break;
                }
            }

            return result;
        }

        private static void SetSnmpReadWrite(JediDevice device)
        {
            string authorization = string.Empty;
            string snmpInterfaceString =
                "https://{0}/hp/network/ioConfig/v1/networkInterfaces/wired1/snmpv1v2Config";
            var getSnmpConfigRequest = (HttpWebRequest)WebRequest.Create(string.Format(snmpInterfaceString, device.Address));

            if (!string.IsNullOrEmpty(device.AdminPassword))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes("admin:" + device.AdminPassword);
                authorization = Convert.ToBase64String(plainTextBytes);
                getSnmpConfigRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
            }

            try
            {
                string snmpConfig;
                using (var response = getSnmpConfigRequest.GetResponse())
                {
                    var streamReader = new StreamReader(response.GetResponseStream());
                    snmpConfig = streamReader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(snmpConfig))
                {
                    var snmpKeys = JObject.Parse(snmpConfig);
                    if (snmpKeys["snmpv1v2Enabled"].ToString() == "true" && snmpKeys["accessOption"].ToString() == "readOnly")
                    {
                        var setSnmpConfigRequest = (HttpWebRequest)WebRequest.Create(string.Format(snmpInterfaceString, device.Address));
                        setSnmpConfigRequest.Method = "PUT";
                        setSnmpConfigRequest.ContentType = "application/json";

                        if (!string.IsNullOrEmpty(device.AdminPassword))
                            setSnmpConfigRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");

                        snmpKeys["accessOption"] = "readWrite";
                        using (var responseStream = setSnmpConfigRequest.GetRequestStream())
                        {
                            var buffer = Encoding.ASCII.GetBytes(snmpKeys.ToString());
                            responseStream.Write(buffer, 0, buffer.Length);
                        }

                        setSnmpConfigRequest.GetResponse();
                    }
                }
            }
            catch (WebException)
            {
                //in windjammer, this interface might not be present, let's ignore as SNMP read/write is set by default
            }
        }

        private bool SetProtocolDefaultValues(string oidName, DataPair<string> itemValue, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            //i don't plan to use web service ticket so this is dummy to conform with the interface
            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                return n;
            };

            //send the oid Name disguised as endpoint
            return UpdateField(change, device, itemValue, string.Empty, oidName, assetInfo, fieldChanged, data);
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> changeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo,
            string activity, PluginExecutionData pluginExecutionData)
        {
            bool success;
            if (data.Value)
            {
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginExecutionData, assetInfo.AssetId);
                try
                {
                    int oidValue = data.Key.ToString() == "on" ? 1 : 0;
                    device.Snmp.Set(endpoint, oidValue);
                    success = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to set field {activity}, {ex.Message}");
                    _failedSettings.AppendLine($"Failed to set field {activity}, {ex.Message}");
                    success = false;
                }
                log.FieldChanged = activity;
                log.Result = success ? "Passed" : "Failed";
                log.Value = data.Key.ToString();
                log.ControlChanged = "Protocol Default";

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