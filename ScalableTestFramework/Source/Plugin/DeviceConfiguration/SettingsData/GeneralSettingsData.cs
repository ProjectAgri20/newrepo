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
    public class GeneralSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> ActivityTimeout { get; set; }

        [DataMember]
        public DataPair<string> SleepTimer { get; set; }

        [DataMember]
        public DataPair<bool> DefaultToEnglish { get; set; }

        [DataMember]
        public DataPair<bool> SyncWithServer { get; set; }

        [DataMember]
        public DataPair<string> ServerAddress { get; set; }

        [DataMember]
        public DataPair<string> Port { get; set; }

        [DataMember]
        public DataPair<string> SyncTime { get; set; }

        [DataMember]
        public DataPair<TimeZoneInfo> TimeZone { get; set; }

        [DataMember]
        public DataPair<DateTime> CurrentDate { get; set; }

        [DataMember]
        public DataPair<DateTime> CurrentTime { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public GeneralSettingsData()
        {
            ActivityTimeout = new DataPair<string> {Key = string.Empty};
            SleepTimer = new DataPair<string> {Key = string.Empty};
            DefaultToEnglish = new DataPair<bool>
            {
                Key = false,
                Value = true
            };

            SyncWithServer = new DataPair<bool>
            {
                Key = false,
                Value = false
            };

            ServerAddress = new DataPair<string> {Key = string.Empty};
            Port = new DataPair<string> {Key = string.Empty};
            SyncTime = new DataPair<string> {Key = string.Empty};
            TimeZone = new DataPair<TimeZoneInfo> {Key = TimeZoneInfo.Local};
            CurrentDate = new DataPair<DateTime> {Key = DateTime.Now};
            CurrentTime = new DataPair<DateTime> {Key = DateTime.Now};

        }

        /// <summary>
        /// Individual function differences separated into delagate methods.
        /// </summary>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            Type type = typeof(GeneralSettingsData);
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
                    case "ActivityTimeout":
                        result &= SetActivityTimeout((DataPair<string>)item.Value, device, assetInfo, "ActivityTimeout", data);
                        break;
                    case "SleepTimer":
                        result &= SetSleepTimer((DataPair<string>)item.Value, device, assetInfo, "SleepTimer", data);
                        break;
                    case "DefaultToEnglish":
                        result &= SetEnglishDefault((DataPair<bool>)item.Value, device, assetInfo, "DefaultToEnglish", data);
                        break;
                    case "SyncWithServer":
                        result &= SetSyncWithServer((DataPair<bool>)item.Value, device, assetInfo, "SyncWithServer", data);
                        break;
                    case "ServerAddress":
                        result &= SetServerAddress((DataPair<string>)item.Value, device, assetInfo, "ServerAddress", data);
                        break;
                    case "Port":
                        result &= SetPort((DataPair<string>)item.Value, device, assetInfo, "Port", data);
                        break;
                    case "SyncTime":
                        result &= SetSyncTime((DataPair<string>)item.Value, device, assetInfo, "SyncTime", data);
                        break;
                    case "TimeZone":
                        result &= SetTimeZone((DataPair<TimeZoneInfo>)item.Value, device, assetInfo, "TimeZone", data);
                        break;
                    case "CurrentDate":
                        result &= SetCurrentDate((DataPair<DateTime>)item.Value, device, assetInfo, "CurrentDate", data);
                        break;
                    case "CurrentTime":
                        result &= SetCurrentTime((DataPair<DateTime>)item.Value, device, assetInfo, "CurrentTime", data);
                        break;
                    default:
                        throw new Exception("Field Property Error");
                }
            }

            return result;
        }

        private bool SetActivityTimeout(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {

            string activityUrn = "urn:hp:imaging:con:service:uiconfiguration:UIConfigurationService";
            string endpoint = "uiconfiguration";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("InactivityTimeoutInSeconds").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);

        }

        private bool SetSleepTimer(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService:AutoOffDelay";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("AutoOffDelayAfterTime").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetEnglishDefault(DataPair<bool> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:uiconfiguration:UIConfigurationService:ServiceDefaults";
            string endpoint = "uiconfiguration";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("DefaultLanguageSetting").SetValue("en-US");
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetSyncWithServer(DataPair<bool> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("EnableDisable").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetPort(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("Port").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetServerAddress(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                //[Veda]: For some weird reason, this element isn't present on first try, adding the element to fix exception
                if (n.FindElement("SNTPServerAddress") == null)
                {
                    AddNtsServer(n);
                }
                n.FindElement("SNTPServerAddress").SetValue(pair.Key);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private void AddNtsServer(WebServiceTicket webTicket)
        {
            var dsd = webTicket.GetNamespaceOfPrefix("dsd");
            var dd = webTicket.GetNamespaceOfPrefix("dd");
            XNamespace timeNs = XNamespace.Get("http://www.hp.com/schemas/imaging/con/service/time/2009/02/20");
            webTicket.Element(timeNs + "SNTPTimeSynchronization").Add(new XElement(dd + "SNTPServerAddress"));
        }

        private bool SetSyncTime(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService:SNTPTimeSynchronization";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                string[] valueString = pair.Key.Split(' ');
                int totalHours = int.Parse(valueString[0]);
                int days = totalHours / 24;
                int hours = totalHours % 24;
                string finalString = BuildSync(days, hours);
                n.FindElement("Frequency").SetValue(finalString);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetTimeZone(DataPair<TimeZoneInfo> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var temp = n.FindElements("TimeZoneName").First();
                n.FindElements("Name").First().SetValue(pair.Key.StandardName);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetCurrentDate(DataPair<DateTime> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var temp = n.FindElements("DateTime").First();
                DateTime originalTime = DateTime.Parse(temp.Value);

                DateTime finalTime = new DateTime(pair.Key.Year, pair.Key.Month, pair.Key.Day, originalTime.Hour, originalTime.Minute, originalTime.Second, originalTime.Millisecond, originalTime.Kind);

                n.FindElements("DateTime").First().SetValue(finalTime);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        private bool SetCurrentTime(DataPair<DateTime> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                var temp = n.FindElements("DateTime").First();
                DateTime originalTime = DateTime.Parse(temp.Value);

                DateTime finalTime = new DateTime(originalTime.Year, originalTime.Month, originalTime.Day, pair.Key.Hour, pair.Key.Minute, pair.Key.Second, pair.Key.Millisecond, originalTime.Kind);


                n.FindElements("DateTime").First().SetValue(finalTime);
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
                log.ControlChanged = @"Time/Language Settings";


                ExecutionServices.DataLogger.Submit(log);

            }
            else
            {
                success = true;
            }
            return success;
        }


        private string BuildSync(int days, int hours)
        {
            return $"P{days}DT{hours}H0M0S";
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
