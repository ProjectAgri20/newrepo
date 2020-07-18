using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
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
            ActivityTimeout = new DataPair<string> { Key = string.Empty };
            SleepTimer = new DataPair<string> { Key = string.Empty };
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

            ServerAddress = new DataPair<string> { Key = string.Empty };
            Port = new DataPair<string> { Key = string.Empty };
            SyncTime = new DataPair<string> { Key = string.Empty };
            TimeZone = new DataPair<TimeZoneInfo> { Key = TimeZoneInfo.Local };
            CurrentDate = new DataPair<DateTime> { Key = DateTime.Now };
            CurrentTime = new DataPair<DateTime> { Key = DateTime.Now };
        }

        /// <summary>
        /// Individual function differences separated into delagate methods.
        /// </summary>
        public bool GetFields(JediDevice device)
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
                        result &= SetActivityTimeout((DataPair<string>)item.Value, device);
                        break;

                    case "SleepTimer":
                        result &= SetSleepTimer((DataPair<string>)item.Value, device);
                        break;

                    case "DefaultToEnglish":
                        result &= SetEnglishDefault((DataPair<bool>)item.Value, device);
                        break;

                    case "SyncWithServer":
                        result &= SetSyncWithServer((DataPair<bool>)item.Value, device);
                        break;

                    case "ServerAddress":
                        result &= SetServerAddress((DataPair<string>)item.Value, device);
                        break;

                    case "Port":
                        result &= SetPort((DataPair<string>)item.Value, device);
                        break;

                    case "SyncTime":
                        result &= SetSyncTime((DataPair<string>)item.Value, device);
                        break;

                    case "TimeZone":
                        result &= SetTimeZone((DataPair<TimeZoneInfo>)item.Value, device);
                        break;

                    case "CurrentDate":
                        result &= SetCurrentDate((DataPair<DateTime>)item.Value, device);
                        break;

                    case "CurrentTime":
                        result &= SetCurrentTime((DataPair<DateTime>)item.Value, device);
                        break;

                    default:
                        throw new Exception("Field Property Error");
                }
            }

            return result;
        }

        private bool SetActivityTimeout(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:uiconfiguration:UIConfigurationService";
            string endpoint = "uiconfiguration";
            string name = "ActivityTimeout";

            Func<WebServiceTicket, bool> change = n => n.FindElement("InactivityTimeoutInSeconds").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetSleepTimer(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService:AutoOffDelay";
            string endpoint = "time";
            string name = "SleepTimer";

            Func<WebServiceTicket, bool> change = n => n.FindElement("AutoOffDelayAfterTime").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetEnglishDefault(DataPair<bool> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:uiconfiguration:UIConfigurationService:ServiceDefaults";
            string endpoint = "uiconfiguration";
            string name = "DefaultToEnglish";

            Func<WebServiceTicket, bool> change = n => n.FindElement("DefaultLanguageSetting").Value == "en-US";
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetSyncWithServer(DataPair<bool> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "SyncWithServer";

            Func<WebServiceTicket, bool> change = n => n.FindElement("EnableDisable").Value == pair.Key.ToString();
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetPort(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "Port";

            Func<WebServiceTicket, bool> change = n => n.FindElement("Port").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase);
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetServerAddress(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "ServerAddress";

            Func<WebServiceTicket, bool> change = n => n.FindElement("SNTPServerAddress").Value.Equals(pair.Key, StringComparison.OrdinalIgnoreCase); 
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetSyncTime(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService:SNTPTimeSynchronization";
            string endpoint = "time";
            string name = "SyncTime";

            Func<WebServiceTicket, bool> change = n =>
            {
                string[] valueString = pair.Key.Split(' ');
                int totalHours = int.Parse(valueString[0]);
                int days = totalHours / 24;
                int hours = totalHours % 24;
                string finalString = BuildSync(days, hours);
                return n.FindElement("Frequency").Value == finalString;
            };

            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetTimeZone(DataPair<TimeZoneInfo> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "TimeZone";

            Func<WebServiceTicket, bool> change = n =>
            {
                var temp = n.FindElements("TimeZoneName").First();
                return n.FindElements("Name").First().Value.Equals(pair.Key.StandardName, StringComparison.OrdinalIgnoreCase);
            };

            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetCurrentDate(DataPair<DateTime> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "CurrentDate";

            Func<WebServiceTicket, bool> change = n =>
            {
                var temp = n.FindElements("DateTime").First();
                DateTime originalTime = DateTime.Parse(temp.Value);

                DateTime finalTime = new DateTime(pair.Key.Year, pair.Key.Month, pair.Key.Day, originalTime.Hour, originalTime.Minute, originalTime.Second, originalTime.Millisecond, originalTime.Kind);

                return DateTime.Parse(n.FindElements("DateTime").First().Value) == finalTime;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, name);
        }

        private bool SetCurrentTime(DataPair<DateTime> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:time:TimeService";
            string endpoint = "time";
            string name = "CurrentTime";

            Func<WebServiceTicket, bool> change = n =>
            {
                var temp = n.FindElements("DateTime").First();
                //it doesn't make sense to verify the current time to stored time, there will be a small millisecond different before checking and during checking this will always fail
                //better to compare Time now
                DateTime originalTime = DateTime.Now;

                DateTime finalTime = new DateTime(originalTime.Year, originalTime.Month, originalTime.Day, originalTime.Hour, originalTime.Minute, originalTime.Second, originalTime.Kind);

                return DateTime.Parse(n.FindElements("DateTime").First().Value) == finalTime;
            };
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
                _failedSettings.Append($"{activityName}, ");
            return success;
        }

        private string BuildSync(int days, int hours)
        {
            return $"P{days}DT{hours}H0M0S";
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> {Key = _failedSettings.ToString(), Value = result};
        }
    }
}