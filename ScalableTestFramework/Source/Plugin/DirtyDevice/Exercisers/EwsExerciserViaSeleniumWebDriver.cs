using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Automation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class EwsExerciserViaSeleniumWebDriver
    {
        private readonly DirtyDeviceManager _owner;
        private readonly JediDevice _device;
        protected Exception _exception = null;
        protected DirtyDeviceActivityData _activityData;
        protected NetworkCredential _userCredential;
        protected PluginEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsExerciserViaSeleniumWebDriver" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal EwsExerciserViaSeleniumWebDriver(DirtyDeviceManager owner, JediDevice device)
        {
            _owner = owner;
            _device = device;
        }

        internal void Exercise(DirtyDeviceActivityData activityData, NetworkCredential userCredential, PluginEnvironment environment, AssetAttributes deviceAttribute)
        {
            _activityData = activityData;
            _userCredential = userCredential;
            _environment = environment;
            StartEws(deviceAttribute);
        }

        public void StartEws(AssetAttributes deviceAttribute)
        {
            InternetExplorerDriver browser = null;

            try
            {
                CopyDriverFiles();
                try
                {
                    InternetExplorerOptions options = new InternetExplorerOptions();
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    browser = new InternetExplorerDriver(options);
                    browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    browser.Manage().Window.Maximize();

                    // Check to see if there are any popups, such as the first run popup, and close them:
                    HandleSetupUpIeDialog();
                }
                catch (Exception ex)
                {
                    throw new EwsAutomationException("There was an error creating a selenium IE driver: " + ex.Message);
                }

                EwsInit(browser);
                EwsWalk(browser);
                ConfigureNetworking(browser);
                if (deviceAttribute.HasFlag(AssetAttributes.Scanner))
                {
                    ConfigureDigitalSend(browser);
                }

                By signoutLinkLocator = By.Id("EwsLogoff");
                if (ElementExists(signoutLinkLocator, browser))
                {
                    browser.FindElement(signoutLinkLocator).Click();
                }
                else
                {
                    throw new EwsAutomationException("Could not find the signout link.");
                }
            }
            finally
            {
                if (browser != null)
                {
                    browser.Quit();
                }
            }
        }

        /// <summary>
        /// Initializes the browser and attempts to login.
        /// </summary>
        /// <param name="browser">The IE Browser instance to use.</param>
        public void EwsInit(InternetExplorerDriver browser)
        {
            _owner.OnUpdateStatus(this, "Navigating to the devices EWS Homepage");
            browser.Navigate().GoToUrl(_device.Address);

            if (ElementExists(By.Id("overridelink"), browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the link to get past the security warning.");
                browser.FindElement(By.Id("overridelink")).Click();
            }

            By loginLinkLocator = By.Id("EwsLogin");
            if (ElementExists(loginLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the EWS Login link");
                browser.FindElement(loginLinkLocator).Click();

                if (ElementExists(By.Id("PinDropDown"), browser))
                {
                    _owner.OnUpdateStatus(this, "Typing in the password");
                    browser.FindElement(By.Id("PasswordTextBox")).SendKeys(_device.AdminPassword);
                }
                else
                {
                    _owner.OnUpdateStatus(this, "Typing in the username and password.");
                    browser.FindElement(By.Id("UserNameTextBox")).SendKeys("admin");
                    browser.FindElement(By.Id("PasswordTextBox")).SendKeys(_device.AdminPassword);
                }

                _owner.OnUpdateStatus(this, "Logging into the device.");
                browser.FindElement(By.Id("signInOk")).Click();
            }
            else
            {
                throw new EwsAutomationException("No password has been set on the device.  Please set the admin password on the device.");
            }
        }

        /// <summary>
        /// Walks the EWS and touches minor settings present.
        /// </summary>
        /// <param name="browser">The IE Browser instance to use.</param>
        public void EwsWalk(InternetExplorerDriver browser)
        {
            // Click the 'Job Log' link.
            By jobLogReportLinkLocator = By.Id("JobLogReport");
            if (ElementExists(jobLogReportLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Job Log Report' link.");
                ClickElement(browser, browser.FindElement(jobLogReportLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Job Log Report' link.");
            }

            // Click the 'Configuration' link.
            By configurationPageLocator = By.Id("InternalPages_Index_ConfigurationPage");
            if (ElementExists(configurationPageLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Configuration' link.");
                ClickElement(browser, browser.FindElement(configurationPageLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Configuration' link.");
            }

            // Click on the 'General' Tab.
            By settingsPageLinkLocator = By.Id("SettingsPages");
            if (ElementExists(settingsPageLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'General Tab' link.");
                ClickElement(browser, browser.FindElement(settingsPageLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'General' tab link.");
            }

            // Click on the 'QuickSets' link.
            By quickSetsLinkLocator = By.Id("QuickSets");
            if (ElementExists(quickSetsLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'QuickSets' link.");
                ClickElement(browser, browser.FindElement(quickSetsLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Quick Sets' link.");
            }

            // Click on the 'Alerts' link.
            By alertsLinkLocator = By.Id("Alerts");
            if (ElementExists(alertsLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Alerts' link.");
                ClickElement(browser, browser.FindElement(alertsLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Alerts' link.");
            }
        }

        /// <summary>
        /// Configures the 'Networking' tab settings
        /// </summary>
        /// <param name="browser">The IE Browser instance to use.</param>
        public void ConfigureNetworking(InternetExplorerDriver browser)
        {
            By networkingLinkLocator = By.Id("Networking");
            if (ElementExists(networkingLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking on the 'Networking' link.");
                ClickElement(browser, browser.FindElement(networkingLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Networking' link.");
            }

            By tcpIpSettingLinkLocator = By.LinkText("TCP/IP Settings");
            if (ElementExists(tcpIpSettingLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Moving to the TCP/IP Settings page.");
                ClickElement(browser, browser.FindElement(tcpIpSettingLinkLocator));
            }
            else
            {
                ////throw new EwsAutomationException("Could not find the 'TCP/IP Settings' link.");
                // For some reason Selenium is unable to see the text for this link at all.  It manages to get a collection of the links but the
                // text property of them is all empty.  In that case we're just going to manually navigate.
                // This only occurs on smaller window sizers with the navigation menu is hidden.
                browser.Navigate().GoToUrl($"http://{_device.Address}/tcp_summary.htm");
            }

            By ipv6LinkLocator = By.LinkText("TCP/IP(v6)");
            if (ElementExists(ipv6LinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'TCP/IP(v6)' link.");
                ClickElement(browser, browser.FindElement(ipv6LinkLocator));
            }
            else
            {
                ////throw new EwsAutomationException("Could not find the 'TCP/IP(v6)' link.");
                // For some reason Selenium is unable to see the text for this link at all.  It manages to get a collection of the links but the
                // text property of them is all empty.  In that case we're just going to manually navigate.
                // This only occurs on smaller window sizes where the navigation menu is hidden.
                browser.Navigate().GoToUrl($"http://{_device.Address}/tcpipv6.htm");
            }

            _owner.OnUpdateStatus(this, "Unchecking the IP6 Enable checkbox.");
            if (browser.FindElement(By.Id("IP6_Enable")).Selected)
            {
                ClickElement(browser, browser.FindElement(By.Id("IP6_Enable")));
            }

            _owner.OnUpdateStatus(this, "Clicking the 'Apply' link.");
            browser.FindElement(By.Id("Apply")).Click();

            if (ElementExists(By.Name("ok"), browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'OK' link.");
                ClickElement(browser, browser.FindElement(By.Name("ok")));
            }

            By authenticationLinkLocator = By.PartialLinkText("802.1X Authentication");
            if (ElementExists(authenticationLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the '802.1X Authentication' link.");
                ClickElement(browser, browser.FindElement(authenticationLinkLocator));
            }
            else
            {
                ////throw new EwsAutomationException("Could not find the '802.1X Authentication' link.");
                // For some reason Selenium is unable to see the text for this link at all.  It manages to get a collection of the links but the
                // text property of them is all empty.  In that case we're just going to manually navigate.
                // This only occurs on smaller window sizes where the navigation sidebar is hidden.
                _owner.OnUpdateStatus(this, "Navigating to the '802.1X Authentication' page.");
                browser.Navigate().GoToUrl($"http://{_device.Address}/dot1x_config.htm");
            }

            By managementProtocolsLinkLocator = By.LinkText("Mgmt. Protocols");
            if (ElementExists(managementProtocolsLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Mgmt. Protocols' link.");
                ClickElement(browser, browser.FindElement(managementProtocolsLinkLocator));

                // Sometimes clicks don't navigate, still investigating and hope to remove this soon.
                if (browser.Url != $"http://{ (object)_device.Address}/websecurity/http_mgmt.html")
                {
                    browser.Navigate().GoToUrl($"http://{ (object)_device.Address}/websecurity/http_mgmt.html");
                }
            }
            else
            {
                ////throw new EwsAutomationException("Could not find the 'Mgmt. Protocols' link.");
                // For some reason Selenium is unable to see the text for this link at all.  It manages to get a collection of the links but the
                // text property of them is all empty.  In that case we're just going to manually navigate.
                // This only occurs on smaller window sizes where the navigation sidebar is hidden.
                _owner.OnUpdateStatus(this, "Navigating to the 'Mgmt. Protocols' page.");
                browser.Navigate().GoToUrl($"http://{_device.Address}/websecurity/http_mgmt.html");
            }

            _owner.OnUpdateStatus(this, "Unchecking the encrypt all checkbox.");
            var encryptAllCheckbox = browser.FindElement(By.Id("encryptall"));
            if (encryptAllCheckbox.Selected)
            {
                encryptAllCheckbox.Click();
            }

            _owner.OnUpdateStatus(this, "Clicking the 'Apply' link.");
            browser.FindElement(By.Name("Apply")).Click();

            if (ElementExists(By.Name("ok"), browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'OK' link.");
                browser.FindElement(By.Name("ok")).Click();
            }
        }

        /// <summary>
        /// Configures a quick set button for the digital scan function on the device
        /// </summary>
        /// <param name="browser">The IE Browser instance to use.</param>
        public void ConfigureDigitalSend(InternetExplorerDriver browser)
        {
            By scanDigitalSendLinkLocator = By.LinkText("Scan/Digital Send");
            if (ElementExists(scanDigitalSendLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Scan/Digital Send' link.");
                browser.FindElement(scanDigitalSendLinkLocator).Click();

                // Sometimes clicks don't navigate, still investigating and hope to remove this soon.
                if (browser.Url != $"http://{ (object)_device.Address}/hp/device/BasicSend/Index")
                {
                    browser.Navigate().GoToUrl($"http://{ (object)_device.Address}/hp/device/BasicSend/Index");
                }
            }
            else
            {
                ////throw new EwsAutomationException("Could not find the 'Scan/Digital Send' link.");
                // For some reason Selenium is unable to see the text for this link at all.  It manages to get a collection of the links but the
                // text property of them is all empty.  In that case we're just going to manually navigate.
                // This only occurs on smaller window sizes where the navigation menu is hidden.
                browser.Navigate().GoToUrl($"http://{_device.Address}/hp/device/BasicSend/Index");
            }

            By quickSetsLinkLocator = By.Id("QuickSetsFolder");
            if (ElementExists(quickSetsLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the 'Quick Sets' link");
                ClickElement(browser, browser.FindElement(quickSetsLinkLocator));
            }
            else
            {
                throw new EwsAutomationException("Could not find the Quick Set Folder link.");
            }

            By selectAllQuickSetsLocator = By.Id("QuickSetAll");
            if (ElementExists(selectAllQuickSetsLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Found the select all checkbox in the quickset list.  Attempting to clear the quickset list.");
                ClickElement(browser, browser.FindElement(selectAllQuickSetsLocator));

                ClickElement(browser, browser.FindElement(By.Id("RemoveQuickSetButton")));

                By confirmRemoveButton = By.Id("DialogButtonYes");
                if (ElementExists(confirmRemoveButton, browser))
                {
                    browser.FindElement(confirmRemoveButton).Click();
                }
            }

            By addQuickSetButtonLocator = By.Id("AddQuickSetButton");
            if (ElementExists(addQuickSetButtonLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the add QuickSet button.");
                browser.FindElement(addQuickSetButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Add QuickSet' button");
            }

            By quickSetTitleBoxLocator = By.Id("QuickSetTitle");
            if (ElementExists(quickSetsLinkLocator, browser))
            {
                _owner.OnUpdateStatus(this, $"Typing '{_activityData.Ews.QuickSetTitle}' into the quick set title box.");
                browser.FindElement(quickSetTitleBoxLocator).SendKeys(_activityData.Ews.QuickSetTitle);
            }
            else
            {
                throw new EwsAutomationException("Could not find the Quick Set Title textbox.");
            }

            By nextButtonLocator = By.Id("FormButtonNext");
            if (ElementExists(nextButtonLocator, browser))
            {
                browser.FindElement(nextButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Next' Button.");
            }

            By selectAllSharedNetworkFolderLocator = By.Id("SharedFolderAll");
            if (ElementExists(selectAllQuickSetsLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Found the select all checkbox in the network folder list.  Attempting to clear the network folder list.");
                ClickElement(browser, browser.FindElement(selectAllSharedNetworkFolderLocator));

                ClickElement(browser, browser.FindElement(By.Id("SharedFolderRemove")));

                By confirmRemoveButton = By.Id("DialogButtonYes");
                if (ElementExists(confirmRemoveButton, browser))
                {
                    browser.FindElement(confirmRemoveButton).Click();
                }
            }

            By addSharedFolderButtonLocator = By.Id("SharedFolderAdd");
            if (ElementExists(addSharedFolderButtonLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the Add Shared Folder button");
                browser.FindElement(addSharedFolderButtonLocator).Click();
            }

            By uncFolderPathBoxLocator = By.Id("UncPath");
            if (ElementExists(uncFolderPathBoxLocator, browser))
            {
                _owner.OnUpdateStatus(this, $"Typing '{_activityData.DigitalSend.OutputFolder.ToFQDNUncPath(_environment)}' into the UNC Path box.");
                browser.FindElement(uncFolderPathBoxLocator).SendKeys(_activityData.DigitalSend.OutputFolder.ToFQDNUncPath(_environment));
            }
            else
            {
                throw new EwsAutomationException("Could not find the UNC Path text box.");
            }

            By authenticationSettingsLocator = By.Id("UseMfpSignInCredentials");
            if (ElementExists(authenticationSettingsLocator, browser))
            {
                SelectElement authenticationSettingsSelect = new SelectElement(browser.FindElement(authenticationSettingsLocator));
                authenticationSettingsSelect.SelectByValue(true.ToString());
            }
            else
            {
                throw new EwsAutomationException("Could not find the authentication settings drop down list.");
            }

            By uncDomainBoxLocator = By.Id("UncDomain");
            if (ElementExists(uncDomainBoxLocator, browser))
            {
                _owner.OnUpdateStatus(this, $"Typing '{_userCredential.Domain}' into the UNC Domain box.");
                browser.FindElement(uncDomainBoxLocator).SendKeys(_userCredential.Domain);
            }
            else
            {
                throw new EwsAutomationException("Could not find the UNC Domain text box.");
            }

            By okButtonLocator = By.Id("FormButtonSubmit");
            if (ElementExists(okButtonLocator, browser))
            {
                browser.FindElement(okButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Ok' button.");
            }

            if (ElementExists(nextButtonLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the Next button.");
                browser.FindElement(nextButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Next' Button.");
            }

            if (ElementExists(nextButtonLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the Next button.");
                browser.FindElement(nextButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Next' Button.");
            }

            By finishButtonLocator = By.Id("FormButtonFinish");
            if (ElementExists(finishButtonLocator, browser))
            {
                _owner.OnUpdateStatus(this, "Clicking the Exit Wizard button.");
                browser.FindElement(finishButtonLocator).Click();
            }
            else
            {
                throw new EwsAutomationException("Could not find the 'Finish' Button.");
            }
        }

        private bool ElementExists(By elementSelector, IWebDriver browser)
        {
            try
            {
                browser.FindElement(elementSelector);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        private void ClickElement(IWebDriver browser, IWebElement element)
        {
            if (element.Displayed)
            {
                element.Click();
            }
            else
            {
                string js = "arguments[0].click()";
                ((IJavaScriptExecutor)browser).ExecuteScript(js, element);
            }
        }

        private void HandleSetupUpIeDialog()
        {
            AutomationElement aeDesktop = AutomationElement.RootElement;
            AutomationElementCollection collection = aeDesktop.FindAll(TreeScope.Children, Condition.TrueCondition);
            if (collection != null)
            {
                List<AutomationElement> aeList = new List<AutomationElement>(collection.Cast<AutomationElement>());
                var internetExplorer = aeList.FirstOrDefault(i => i.Current.Name.Contains("WebDriver"));
                var childCollection = internetExplorer.FindAll(TreeScope.Children, Condition.TrueCondition);
                List<AutomationElement> childList = new List<AutomationElement>(childCollection.Cast<AutomationElement>());
                if (childList != null)
                {
                    var dialog = childList.FirstOrDefault(i => i.Current.Name.Contains("Internet Explorer"));
                    if (dialog != null)
                    {
                        var okButton = dialog.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "54314"));
                        if (okButton != null)
                        {
                            var clickOkButton = (InvokePattern)okButton.GetCurrentPattern(InvokePattern.Pattern);
                            clickOkButton.Invoke();
                        }
                    }
                }
            }
        }

        private static void CopyDriverFiles()
        {
            ExecutionServices.SystemTrace.LogDebug("Copying Selenium driver files...");
            StringBuilder exePath = new StringBuilder(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            exePath.Append("\\IEDriverServer.exe");

            File.WriteAllBytes(exePath.ToString(), Resource.IEDriverServer);
            ExecutionServices.SystemTrace.LogDebug("Files copied...");

        }

    }
}
