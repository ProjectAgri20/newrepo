using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.SPS.SES;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Implementation of <see cref="ICloudLoginManager" /> for a <see cref="DropBoxLoginManager" />.
    /// </summary>
    public sealed class DropBoxLoginManager : DeviceWorkflowLogSource, ICloudLoginManager
    {
        private JetAdvantageLinkUI _linkUI;
        private GoogleLoginManager GoogleSignIn;
        /// <summary>
        /// Create Dropbox Login Manager
        /// </summary>
        /// <param name="linkUI"><see cref="JetAdvantageLinkUI"/> to control</param>
        public DropBoxLoginManager(JetAdvantageLinkUI linkUI)
        {
            _linkUI = linkUI;
            GoogleSignIn = new GoogleLoginManager(_linkUI);            
        }
        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="linkApp_ID">ID of the Link application for sign in on AA</param>
        /// <param name="linkApp_PWD">PW of the Link application for sign in on AA</param>
        /// </summary> 
        public bool SignIn(string linkApp_ID, string linkApp_PWD)
        {
            bool result = true;
            bool googleAuth = false;
            
            googleAuth = linkApp_ID.Contains("@gmail.com");
            // Check that ID uses google auth

            if (googleAuth)
            {
                using (WebviewObject loginPanel = _linkUI.Controller.GetWebView())
                {
                    if (loginPanel.IsExist("//*[@id=\"regular-login-forms\"]/div/div/div/button/div", TimeSpan.FromSeconds(60)))
                    {
                        result = loginPanel.Click("//*[@id=\"regular-login-forms\"]/div/div/div/button/div");

                        if (result)
                        {
                            if(WorkflowLogger != null)
                            {
                                GoogleSignIn.WorkflowLogger = WorkflowLogger;
                            }                            
                            result = GoogleSignIn.SignIn(linkApp_ID, linkApp_PWD, loginPanel);
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                 //ID is using google AA                                 
                using (WebviewObject loginPanel = _linkUI.Controller.GetWebView())
                {
                    RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);

                    if (loginPanel.IsExist("//*[@class='text-input-input'][@type='email'][@name='login_email']", TimeSpan.FromSeconds(60)))
                    {
                        result = loginPanel.SetText("//*[@class='text-input-input'][@type='email'][@name='login_email']", linkApp_ID, false);
                    }
                    else
                    {
                        result = false;
                    }
                        
                    // Insert the ID
                    if (result)
                    {
                        result = loginPanel.SetText("//*[@class='password-input text-input-input'][@type='password'][@name='login_password']", linkApp_PWD, false);
                    }
                    // Insert the PW
                    RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
                    if (result)
                    {
                        result = loginPanel.Click("//*[@id=\"regular-login-forms\"]/div/form/div[3]/button/div[1]");
                        // Submit Button
                    }
                    // Click SignIn Button
                    loginPanel.Click("//*[@id=\"authorize-form\"]/button[2]", TimeSpan.FromSeconds(5));
                }                    
            }            
            return result;
        }
    }
}
