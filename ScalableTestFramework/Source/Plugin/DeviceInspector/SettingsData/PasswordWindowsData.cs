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
    [DataContract]
    public class PasswordWindowsData : IComponentData
    {
        [DataMember]
        public DataPair<string> MinPWLength { get; set; }

        [DataMember]
        public DataPair<string> WindowsDomains { get; set; }

        [DataMember]
        public DataPair<string> DefaultDomain { get; set; }

        [DataMember]
        public DataPair<bool> EnablePasswordComplexity { get; set; }

        [DataMember]
        public DataPair<bool> EnableWindowsAuthentication { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();
        public PasswordWindowsData()
        {
            MinPWLength = new DataPair<string> {Key = "-1"};
            WindowsDomains = new DataPair<string> {Key = string.Empty};
            DefaultDomain = new DataPair<string> {Key = string.Empty};

            EnablePasswordComplexity = new DataPair<bool>
            {
                Key = false,
                Value = true
            };

            EnableWindowsAuthentication = new DataPair<bool>
            {
                Key = false,
                Value = true
            };
        }

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(PasswordWindowsData);
            bool result = false;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "MinPWLength":
                        result = GetMinPWLength((DataPair<string>)item.Value, device);
                        break;

                    case "WindowsDomains":
                       // result = SetWindowsDomains((DataPair<string>)item.Value, device);
                        break;

                    case "DefaultDomain":
                        result = GetDefaultDomain((DataPair<string>)item.Value, device);
                        break;

                    case "EnablePasswordComplexity":
                        result = GetPasswordComplexity((DataPair<bool>)item.Value, device);
                        break;

                    case "EnableWindowsAuthentication":
                        result = GetWindowsAuthentication((DataPair<bool>)item.Value, device);
                        break;

                    default:
                        throw new Exception("Field Set Property Error");
                }
            }

            return result;
        }

        private bool GetMinPWLength(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            string name = "MinPasswordLength";

            Func<WebServiceTicket, bool> getProperty = n =>
            {
                if (pair.Key != "-1")
                {
                    return n.FindElement("MinLength").Value == pair.Key;
                }
                return true;
            };

            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
        }

        //private bool SetWindowsDomains(DataPair<string> pair, JediDevice device)
        //{
        //    string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:DomainNames";
        //    string endpoint = "windowsauthenticationagent";
        //    string name = "WindowsDomains";

        //    Func<WebServiceTicket, bool> getProperty = n =>
        //    {
        //        string[] domains = pair.Key.Split(';');
        //        if (domains.Length != 0)
        //        {
        //            try
        //            {
        //                string ticketString = n.ToString();
        //                string addDomain = ">";
        //                ticketString = ticketString.Remove(ticketString.Length - 2);

        //                foreach (string domain in domains)
        //                {
        //                    addDomain += $@"<dd3:DomainName>{domain}</dd3:DomainName>";
        //                }
        //                addDomain += @"</dd3:DomainNames>";
        //                n = new WebServiceTicket(ticketString + addDomain);
        //                return n;
        //            }
        //            catch (Exception e)
        //            {
        //                ExecutionServices.SystemTrace.LogDebug($@"Device {device.Address} failed to set domains {e.Message}");
        //            }
        //        }
        //        return n;
        //    };
        //    return UpdateField(change, device, pair, activityUrn, endpoint, name);
        //}

        //Validation takes care of making sure default is a subset of domains?
        private bool GetDefaultDomain(DataPair<string> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:AgentSettings";
            string endpoint = "windowsauthenticationagent";
            string name = "DefaultDomain";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement("DefaultDomainName").Value == pair.Key;

            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
           
        }

        private bool GetPasswordComplexity(DataPair<bool> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            string name = "PasswordComplexity";

            Func<WebServiceTicket, bool> getProperty = n =>
            {
                string val = pair.Key ? "true" : "false";
                return n.FindElement("IsPasswordComplexityEnabled").Value == val;
                
            };
            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
        }

        private bool GetWindowsAuthentication(DataPair<bool> pair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:AgentSettings";
            string endpoint = "windowsauthenticationagent";
            string name = "DefaultDomain";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement("EnableDisable").Value == pair.Key.ToString();

            return UpdateField(getProperty, device, pair, activityUrn, endpoint, name);
            
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

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}