using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.SPS.SES;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Implementation of <see cref="ICloudLoginManager" /> for a <see cref="GoogleLoginManager" />.
    /// </summary>
    public sealed class GoogleLoginManager : DeviceWorkflowLogSource, ICloudLoginManager
    {
        private JetAdvantageLinkUI _linkUI;
        
        /// <summary>
        /// Create Google Login Manager
        /// </summary>
        /// <param name="linkUI"><see cref="JetAdvantageLinkUI"/> to control</param>
        public GoogleLoginManager(JetAdvantageLinkUI linkUI)
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
            using (WebviewObject login = _linkUI.Controller.GetWebView())
            {
                result = SignIn(linkApp_ID, linkApp_PWD, login);
            }
                        
            return result;
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="linkApp_ID">ID of the Link application for sign in on AA</param>
        /// <param name="linkApp_PWD">PW of the Link application for sign in on AA</param>
        /// <param name="login">Webview of the Link application for signin on AA</param>
        /// </summary> 
        public bool SignIn(string linkApp_ID, string linkApp_PWD, WebviewObject login)
        {
            bool result = true;
            
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
            result = login.SetText("//*[@id=\"Email\"]", linkApp_ID, false);
            // Insert the ID

            if (result)
            {
                result = login.Click("//*[@id=\"next\"]");
            }
            // Click "Next" Button

            if (result && login.IsExist("//*[@id=\"Passwd\"]", TimeSpan.FromSeconds(10)))
            {
                result = login.SetText("//*[@id=\"Passwd\"]", linkApp_PWD, false);
            }
            // Insert the PW
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);

            if (result)
            {
                result = login.Click("//*[@id=\"signIn\"]");
            }
            // Click SignIn Button

            if (result && login.IsExist("//*[@id=\"submit_approve_access\"]", TimeSpan.FromSeconds(10)))
            {
                result = login.Click("//*[@id=\"submit_approve_access\"]");
            }
            // Check that Approve Button is displayed            

            return result;
        }
    }
}