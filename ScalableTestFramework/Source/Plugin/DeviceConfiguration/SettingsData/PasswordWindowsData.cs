using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System.Text;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
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
            Type type = typeof(PasswordWindowsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach(PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "MinPWLength":
                        result &= SetMinPWLength((DataPair<string>)item.Value, device, assetInfo, "MinPassword Length", data);// != true ? false : true;
                        break;
                    case "WindowsDomains":
                        result &= SetWindowsDomains((DataPair<string>)item.Value, device,  assetInfo, "Windows Domains", data);// != true ? false : true;
                        break;
                    case "DefaultDomain":
                        result &= SetDefaultDomain((DataPair<string>)item.Value, device, assetInfo, "Default Domain", data);// != true ? false : true;
                        break;
                    case "EnablePasswordComplexity":
                        result &= SetPasswordComplexity((DataPair<bool>)item.Value, device, assetInfo, "Password Complexity", data);// != true ? false : true;
                        break;
                    case "EnableWindowsAuthentication":
                        result &= SetWindowsAuthentication((DataPair<bool>)item.Value, device, assetInfo, "Enable Windows Auth", data);// != true ? false : true;
                        break;
                    default:
                        throw new Exception("Field Set Property Error");
                }
            }

            return result;
        }

        private bool SetMinPWLength(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                if (pair.Key != "-1")
                {
                    n.FindElement("MinLength").SetValue(pair.Key);
                }
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);

        }

        private bool SetWindowsDomains(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:DomainNames";
            string endpoint = "windowsauthenticationagent";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                string[] domains = pair.Key.Split(';');
                if (domains.Length != 0)
                {
                    try
                    {
                        string ticketString = n.ToString();
                        string addDomain = ">";
                        ticketString = ticketString.Remove(ticketString.Length - 2);

                        foreach (string domain in domains)
                        {
                            addDomain += $@"<dd3:DomainName>{domain}</dd3:DomainName>";
                        }
                        addDomain += @"</dd3:DomainNames>";
                        n = new WebServiceTicket(ticketString + addDomain);
                        return n;
                    }
                    catch (Exception e)
                    {
                        ExecutionServices.SystemTrace.LogDebug($@"Device {device.Address} failed to set domains {e.Message}");
                        throw;
                    }
                }
                return n;

            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }

        //Validation takes care of making sure default is a subset of domains?
        private bool SetDefaultDomain(DataPair<string> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:AgentSettings";
            string endpoint = "windowsauthenticationagent";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                n.FindElement("DefaultDomainName").SetValue($"{pair.Key}");
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);



        }
        private bool SetPasswordComplexity(DataPair<bool> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                string val = pair.Key ? "true" : "false";
                n.FindElement("IsPasswordComplexityEnabled").SetValue(val);
                return n;
            };
            return UpdateField(change, device, pair, activityUrn, endpoint, assetInfo, fieldChanged, data);
        }
        private bool SetWindowsAuthentication(DataPair<bool> pair, JediDevice device, AssetInfo assetInfo, string fieldChanged, PluginExecutionData data)
        {
            string activityUrn = "urn:hp:imaging:con:service:windowsauthenticationagent:WindowsAuthenticationAgentService:AgentSettings";
            string endpoint = "windowsauthenticationagent";

            Func<WebServiceTicket, WebServiceTicket> change = n =>
            {
                if (pair.Key)
                {
                    n.FindElement("EnableDisable").SetValue("enabled");
                }
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> ChangeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string fieldChanged, PluginExecutionData pluginData)
        {
            bool success = false;
            if (data.Value)
            {
                DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    tic = ChangeValue(tic);
                    device.WebServices.PutDeviceTicket(endpoint, urn, tic, false);
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
                log.ControlChanged = "Usage Settings";


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
