using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.Contacts;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class SpeedDialData : IComponentData
    {
        [DataMember]
        public DataPair<string> DisplayName { get; set; }
        [DataMember]
        public DataPair<string> SpeedDialNumber { get; set; }
        [DataMember]
        public DataPair<string> FaxNumbers { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public SpeedDialData()
        {
            DisplayName = new DataPair<string> { Key = String.Empty };
            SpeedDialNumber = new DataPair<string> { Key = String.Empty };
            FaxNumbers = new DataPair<string> { Key = String.Empty };
        }
        /// <summary>
        /// Execution Entry point
        /// Individual function differences separated into delagate methods.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, Framework.Plugin.PluginExecutionData data)
        {                    
            IContactsApp contactsApp = ContactsAppFactory.Create(device);

            try
            {
                if (DisplayName.Value)
                {
                    NetworkCredential cred = new NetworkCredential("admin", device.AdminPassword);
                    AuthenticationCredential auhcred = new AuthenticationCredential(cred);
                    IAuthenticator auth = AuthenticatorFactory.Create(device, auhcred, AuthenticationProvider.Auto);
                    contactsApp.Launch(auth, AuthenticationMode.Lazy);
                    int speedDialNumber;
                    speedDialNumber = contactsApp.CreateSpeedDial(DisplayName.Key, SpeedDialNumber.Key, FaxNumbers.Key);
                }
            }
            catch (Exception ex)
            {
                _failedSettings.AppendLine($"Failed to set field speed Dial: {ex.Message}");
                throw new DeviceWorkflowException("Speed dial cannot be created.",ex);
            }
            return true;
        }

        /// <summary>
        /// Throws NotImplementedException as SpeedDial Data use the UI
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> ChangeValue, JediDevice device, DataPair<T> data, string urn, string endpoint,  AssetInfo assetInfo, string fieldChanged, Framework.Plugin.PluginExecutionData pluginData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface function to drive setting of data fields and return results upstream
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>result</returns>
        public DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, Framework.Plugin.PluginExecutionData data)
        {
            _failedSettings = new StringBuilder();
            var result = ExecuteJob(device, assetInfo, data);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}