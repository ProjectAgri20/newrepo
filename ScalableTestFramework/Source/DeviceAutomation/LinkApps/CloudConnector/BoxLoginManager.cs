using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.SPS.SES;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Implementation of <see cref="ICloudLoginManager" /> for a <see cref="BoxLoginManager" />.
    /// </summary>
    public sealed class BoxLoginManager : DeviceWorkflowLogSource, ICloudLoginManager
    {
        private JetAdvantageLinkUI _linkUI;

        /// <summary>
        /// Create Box Login Manager
        /// </summary>
        /// <param name="linkUI"><see cref="JetAdvantageLinkUI"/> to control</param>
        public BoxLoginManager(JetAdvantageLinkUI linkUI)
        {
            _linkUI = linkUI;

        }
        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="linkApp_ID">ID of the Link application for sign in on AA</param>
        /// <param name="linkApp_PWD">PW of the Link application for sign in on AA</param>
        /// </summary> 
        public bool SignIn(string linkApp_ID, string linkApp_PWD)
        {
            bool result = true;
            using (WebviewObject loginPanel = _linkUI.Controller.GetWebView())
            {
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
                result = loginPanel.SetText("//*[@id=\"login\"]", linkApp_ID, false);
                // Insert the ID
                if (result)
                {
                    result = loginPanel.SetText("//*[@id=\"password\"]", linkApp_PWD, false);
                }
                // Insert the PW
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
                if (result)
                {
                    result = loginPanel.Click("/html/body/div[3]/div/div[1]/div[2]/div/div[1]/form/div[1]/div[2]/input");
                }
                // Click SignIn Button
                loginPanel.Click("//*[@id=\"consent_accept_button\"]/span[1]", TimeSpan.FromSeconds(5));
            }            
            return result;
        }
    }
}