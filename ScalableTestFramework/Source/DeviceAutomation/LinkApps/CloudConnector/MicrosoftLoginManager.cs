using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Threading;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Implementation of <see cref="ICloudLoginManager" /> for a <see cref="MicrosoftLoginManager" />.
    /// </summary>
    public sealed class MicrosoftLoginManager : DeviceWorkflowLogSource, ICloudLoginManager
    {
        
        private JetAdvantageLinkUI _linkUI;        
        private bool _staySignedInPopup;
        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Create Microsoft Login Manager (Sharepoint and Onedrive)
        /// </summary>
        /// <param name="linkUI"><see cref="JetAdvantageLinkUI"/> to control</param>
        public MicrosoftLoginManager(JetAdvantageLinkUI linkUI)
        {
            _linkUI = linkUI;
            _staySignedInPopup = false;
        }

        /// <summary>
        /// Create Microsoft Login Manager (Sharepoint and Onedrive)
        /// </summary>
        /// <param name="linkUI"><see cref="JetAdvantageLinkUI"/> to control</param>
        /// <param name="staySignedInPopup">give true if Stay SingnedIn check box appears after login (OneDrive Business)</param>
        public MicrosoftLoginManager(JetAdvantageLinkUI linkUI, bool staySignedInPopup)
        {
            _linkUI = linkUI;
            _staySignedInPopup = staySignedInPopup;
        }
        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="linkApp_ID">ID of the Link application for sign in on AA</param>
        /// <param name="linkApp_PWD">PW of the Link application for sign in on AA</param>
        /// </summary> 
        public bool SignIn(string linkApp_ID, string linkApp_PWD)
        {
            bool result = true;

            using (WebviewObject singinPanel = _linkUI.Controller.GetWebView())
            {
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);

                //_controller.PressKey(KeyCode.KEYCODE_WAKEUP);

                // Insert the ID and Click "Next" Button
                result = singinPanel.SetText("//*[@class=\"placeholderContainer\"]/input[1]", linkApp_ID + OpenQA.Selenium.Keys.Enter, false);
                UpdateStatus($"Input ID and Click Next Button result : {result}");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Insert the PW and Click SignIn Button
                if (result)
                {
                    result = singinPanel.IsExist("//*[@class=\"placeholderContainer\"]/input[1]", TimeSpan.FromSeconds(10));
                    result = singinPanel.SetText("//*[@class=\"placeholderContainer\"]/input[1]", linkApp_PWD + OpenQA.Selenium.Keys.Enter, false);
                }
                UpdateStatus($"Input PW and Click Login Button result : {result}");
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);

                if (_staySignedInPopup)
                {
                    singinPanel.Click("//*[@id='idBtn_Back']");
                }
            }
            return result;
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

    }
}
