using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    /// <summary>
    /// Management Protocol Settings container
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

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(ProtocolSettingsData);
            bool result = true;
            Dictionary<string, object> properties = new Dictionary<string, object>();
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
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.24.0", (DataPair<string>)item.Value, device, "P9100");
                        }
                        break;

                    case "Lpd":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.22.0", (DataPair<string>)item.Value, device, "Lpd");
                        }
                        break;

                    case "WSPrint":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.37.0", (DataPair<string>)item.Value, device, "WSPrint");
                        }
                        break;

                    case "Ftp":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.20.0", (DataPair<string>)item.Value, device, "Ftp");
                        }
                        break;

                    case "Slp":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.21.0", (DataPair<string>)item.Value, device, "Slp");
                        }
                        break;

                    case "Bonjour":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.29.0", (DataPair<string>)item.Value, device, "Bonjour");
                        }
                        break;

                    case "MultiCast":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.30.0", (DataPair<string>)item.Value, device, "MultiCast");
                        }
                        break;

                    case "WSDiscovery":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.36.0", (DataPair<string>)item.Value, device, "WSDiscovery");
                        }
                        break;

                    case "Llmnr":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.38.0", (DataPair<string>)item.Value, device, "Llmnr");
                        }
                        break;

                    case "WinsPort":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.41.0", (DataPair<string>)item.Value, device, "WinsPort");
                        }
                        break;

                    case "WinsRegistration":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.42.0", (DataPair<string>)item.Value, device, "WinsRegistration");
                        }
                        break;

                    case "Telnet":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.19.0", (DataPair<string>)item.Value, device, "Telnet");
                        }
                        break;

                    case "Tftp":
                        {
                            result &= GetProtocolDefaultValues("1.3.6.1.4.1.11.2.4.3.7.44.0", (DataPair<string>)item.Value, device, "Tftp");
                        }
                        break;
                }
            }

            return result;
        }

        private bool GetProtocolDefaultValues(string oidName, DataPair<string> itemValue, JediDevice device, string activityName)
        {
            //i don't plan to use web service ticket so this is dummy to conform with the interface
            Func<WebServiceTicket, bool> change = n =>
            {
                return true;
            };

            //send the oid Name disguised as endpoint
            return UpdateField(change, device, itemValue, string.Empty, oidName, activityName);
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint,
            string activityName)
        {
            bool success;
            if (data.Value)
            {
                try
                {
                    int oidValue = data.Key.ToString() == "on" ? 1 : 0;
                    var oidResult = device.Snmp.Get(endpoint);

                    success = oidResult == oidValue.ToString();
                }
                catch (Exception ex)
                {
                    _failedSettings.AppendLine($"Failed to verify field {activityName}, {ex.Message}");
                    success = false;
                }
            }
            else
            {
                success = true;
            }

            if (!success)
                _failedSettings.Append($"{activityName}, ");

            return success;
        }
    }
}