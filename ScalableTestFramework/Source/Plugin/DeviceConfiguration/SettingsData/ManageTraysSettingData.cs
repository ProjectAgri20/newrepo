using System;
using System.Text;
using System.Runtime.Serialization;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings;
using System.Net;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class ManageTraysSettingData : IComponentData
    {
        [DataMember]
        public DataPair<string> UseRequestedTray { get; set; }

        [DataMember]
        public DataPair<string> ManualFeedPrompt { get; set; }

        [DataMember]
        public DataPair<string> SizeTypePrompt { get; set; }

        [DataMember]
        public DataPair<string> UseAnotherTray { get; set; }

        [DataMember]
        public DataPair<string> AlternativeLetterHeadMode { get; set; }

        [DataMember]
        public DataPair<string> DuplexBlankPages { get; set; }

        [DataMember]
        public DataPair<string> ImageRotation { get; set; }

        [DataMember]
        public DataPair<string> OverrideA4Letter { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        [DataMember]
        public TraySettings _traySettings { get; set; }

        public ManageTraysSettingData()
        {
            UseRequestedTray = new DataPair<string> { Key = string.Empty };
            ManualFeedPrompt = new DataPair<string> { Key = string.Empty };
            SizeTypePrompt = new DataPair<string> { Key = string.Empty };
            UseAnotherTray = new DataPair<string> { Key = string.Empty };
            AlternativeLetterHeadMode = new DataPair<string> { Key = string.Empty };
            DuplexBlankPages = new DataPair<string> { Key = string.Empty };
            ImageRotation = new DataPair<string> { Key = string.Empty };
            OverrideA4Letter = new DataPair<string> { Key = string.Empty };
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
            ITraySettingsApp traySettingsApp = TraySettingsAppFactory.Create(device);
            try
            {
                NetworkCredential cred = new NetworkCredential("admin", device.AdminPassword);
                AuthenticationCredential auhcred = new AuthenticationCredential(cred);
                IAuthenticator auth = AuthenticatorFactory.Create(device, auhcred, AuthenticationProvider.Auto);
                if (CheckExecuteStatus(_traySettings))
                {
                    traySettingsApp.Launch(auth, AuthenticationMode.Lazy);
                    traySettingsApp.ManageTraySettings(this._traySettings);
                }
                return true;
            }
            catch (Exception ex)
            {
                _failedSettings.AppendLine($"Failed to set field speed Dial: {ex.Message}");
                throw new DeviceWorkflowException("Manage Trays cannot be created.");
            }
        }
        /// <summary>
        /// ManageTraysSettings utilizes the UI and not available web settings.
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
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> changeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string fieldChanged, Framework.Plugin.PluginExecutionData pluginData)
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
        public bool CheckExecuteStatus(TraySettings traySettings)
        {
            bool checkStatus = traySettings.IsUseRequesetedTraySet||traySettings.IsManualFeedPromptSet|| traySettings.IsSizeTypePromptSet|| traySettings.IsUseAnotherTraySet || traySettings.IsAlternativeLetterheadModeSet|| traySettings.IsDuplexBlankPagesSet|| traySettings.IsImageRotationSet|| traySettings.IsOverrideA4LetterSet;
            return checkStatus;
        }
    }
}
