using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.IO;
using HP.ScalableTest.Framework;
using System.Text;
using System.Reflection;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using System.Collections.Generic;
using HP.ScalableTest.Plugin.LocksmithConfiguration.ActivityData;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    internal class LockSmithConfigurationActivityManager
    {
        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Gets the <see cref="LockSmithConfigurationActivityData" /> for this activity.
        /// </summary>
        private LockSmithConfigurationActivityData _activityData;

        /// <summary>
        /// Creates the instance of <see cref="SeleniumHelper" /> class.
        /// </summary>
        private SeleniumHelper _seleniumHelper = new SeleniumHelper();

        /// <summary>
        /// Gets the <see cref="PluginExecutionData" /> for this activity.
        /// </summary>
        private PluginExecutionData _executionData = null;

        /// <summary>
        /// Creates the instace of selenium Iwebdriver
        /// </summary>
        private IWebDriver _driver;

        private const int ShortWaitDelay = 2000;
        private const int WaitDelay = 4000;
        private const int PopupWaitDelay = 9000;
        private const int ElementWaitDelay = 30;
        private const int PageLoadDelay = 200;

        public PluginExecutionResult ApplyConfiguration(PluginExecutionData executionData)
        {
            _executionData = executionData;
            _activityData = executionData.GetMetadata<LockSmithConfigurationActivityData>();
            try
            {
                CopyDriverFiles(_activityData);
                _driver.Manage().Window.Maximize();
                PerformLogin();
                SettingEWSAdminPassword();
                CreateGroup();
                switch (_activityData.Adddevice)
                {
                    case PrinterDiscovery.ManualIPAddress:
                        PrinterDiscoveryManualIPAddress();
                        break;

                    case PrinterDiscovery.DeviceListFile:
                        PrinterDiscoveryDeviceListFile();
                        break;

                    case PrinterDiscovery.AutomaticHops:
                        PrinterDiscoveryAutomaticHops();
                        break;

                    case PrinterDiscovery.AutomaticRange:
                        PrinterDiscoveryAutomaticRange();
                        break;

                    case PrinterDiscovery.AssetInventory:
                        PrinterDiscoveryAssetInventory();
                        break;
                }

                if (_activityData.PolicyConfiguration)
                {
                    ImportPolicy();
                }

                if (_activityData.ReportExtraction)
                {
                    GenerateReports();
                }

                UpdateStatus("Activities completed successfully.");

                return new PluginExecutionResult(PluginResult.Passed);
            }
            catch (IOException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Selenium Web driver is already in use. Please close all diver instances and re-run.");
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Selenium Web element does not exist.");
            }
            catch (NotFoundException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Unable to find Selenium web element.");
            }
            catch (StaleElementReferenceException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Selenium element reference does not exist.");
            }
            catch (WebDriverTimeoutException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Selenium webDriver timed out.");
            }
            catch (WebDriverException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Lost communication with the Selenium webdriver.");
            }
            catch (LocksmithConfigurationException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Locksmith configuration operation failed while performing a selenium action.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return new PluginExecutionResult(PluginResult.Failed, "Locksmith configuration operation failed.");
            }
            finally
            {
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver.Dispose();
                }
            }
        }

        private void PerformLogin()
        {
            string lockSmithServerIp = string.Empty;

            lockSmithServerIp = _executionData.Servers.First().Address;
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(PageLoadDelay);
            _driver.Navigate().GoToUrl($"https://{ lockSmithServerIp }:7637/#/Login");
            //To Ignore Self signed certificate warning
            string title = "Security Manager";
            if (_driver.Title != title)
            {
                _driver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
            }
            By userNameTextBox = By.Id("id_txtbox_userName");
            By passwordTextBox = By.Id("id_txtbox_password");
            By loginbutton = By.Id("id_btn_login");
            _seleniumHelper.WaitUntilVisible(_driver, userNameTextBox);
            if (_seleniumHelper.ElementExists(userNameTextBox, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, userNameTextBox);
            }
            _seleniumHelper.PassData(_driver, userNameTextBox, _activityData.LockSmithUser);
            UpdateStatus("Username entered");
            _seleniumHelper.WaitUntilVisible(_driver, passwordTextBox);
            _seleniumHelper.FindElementandClick(_driver, passwordTextBox);
            _seleniumHelper.PassData(_driver, passwordTextBox, _activityData.LockSmithPassword);
            UpdateStatus("Password entered");
            _seleniumHelper.WaitUntilVisible(_driver, loginbutton);
            _seleniumHelper.FindElementandClick(_driver, loginbutton);
            UpdateStatus("Clicked on Login button");
            By DevicesTab = By.LinkText("Devices");
            if (!_seleniumHelper.ElementExists(DevicesTab, _driver))
            {
                UpdateStatus("Unable to login to the locksmith server");
                ExecutionServices.SystemTrace.LogError("Unable to login to the locksmith server");
                throw new LocksmithConfigurationException("Unable to login to the Locksmith server due to incorrect username or password.");
            }
            else
            {
                _seleniumHelper.WaitUntilClickable(_driver, DevicesTab);
                UpdateStatus("Successfully logged into locksmith server");
            }
        }

        private void SettingEWSAdminPassword()
        {
            IWebElement selectedContainerElement;
            By settingsMenuButton = By.XPath("/html/body/div[1]/header/div/div/div[3]/ul/li[1]/div");
            _seleniumHelper.WaitUntilClickable(_driver, settingsMenuButton);
            if (_activityData.EWSAdminPassword == string.Empty)
            {
                UpdateStatus("Global Password not provided");
            }
            else
            {
                _seleniumHelper.FindElementandClick(_driver, settingsMenuButton);
                By settingsDropdown = By.XPath("/html/body/div[1]/header/div/div/div[3]/ul/li[1]/div/ul");
                _seleniumHelper.WaitUntilVisible(_driver, settingsDropdown);
                By settingsOption = By.Id("id_btn_Appsettings_Option_Settings");
                if (_seleniumHelper.ElementExists(settingsOption, _driver))
                {
                    _seleniumHelper.FindElementandClick(_driver, settingsOption);
                }
                By globalCredentialsOption = By.Id("id_span_tabs_globalcredentials");
                _seleniumHelper.WaitUntilVisible(_driver, globalCredentialsOption);
                _seleniumHelper.FindElementandClick(_driver, globalCredentialsOption);
                By adminPasswordCheckbox = By.Id("id_chkbox_setAdminPassword");
                selectedContainerElement = _driver.FindElement(adminPasswordCheckbox);
                _seleniumHelper.MoveToElement(_driver, adminPasswordCheckbox);
                if (selectedContainerElement.Selected)
                {
                    UpdateStatus("Password already exist updating with the provided password");
                }
                else
                {
                    _seleniumHelper.FindElementandClick(_driver, adminPasswordCheckbox);
                }
                _seleniumHelper.FindElementandClick(_driver, By.Id("id_txtbox_adminPassword"));
                _seleniumHelper.ClearContent(_driver, By.Id("id_txtbox_adminPassword"));
                _seleniumHelper.PassData(_driver, By.Id("id_txtbox_adminPassword"), _activityData.EWSAdminPassword);
                _seleniumHelper.FindElementandClick(_driver, By.Id("id_txtbox_confirmAdminPassword"));
                _seleniumHelper.ClearContent(_driver, By.Id("id_txtbox_confirmAdminPassword"));
                _seleniumHelper.PassData(_driver, By.Id("id_txtbox_confirmAdminPassword"), _activityData.EWSAdminPassword);
                _seleniumHelper.FindElementandClick(_driver, By.Id("id_btn_saveCredentials"));
                UpdateStatus("EWS admin password saved sucessfully");
            }
        }

        private void CreateGroup()
        {
            By devicesTab = By.Id("id_lst_menuitem_devices");
            if (_seleniumHelper.ElementExists(devicesTab, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, devicesTab);
            }
            if (string.IsNullOrEmpty(_activityData.GroupName))
            {
                UpdateStatus("No group name mentioned, selecting default group");
                _activityData.GroupName = "AllDevices";
            }
            else
            {
                By groupName = By.Id($"id_span_tickIconName-{_activityData.GroupName.ToLower()}");
                if (_seleniumHelper.ElementExists(groupName, _driver))
                {
                    UpdateStatus("Group name already exists");
                }
                else
                {
                    _seleniumHelper.FindElementandClick(_driver, By.Id("id_span_icon_managegroups"));
                    By newGroupNameTextBox = By.Id("id_txtbox_newGroupName");
                    _seleniumHelper.WaitUntilClickable(_driver, newGroupNameTextBox);
                    _seleniumHelper.FindElementandClick(_driver, newGroupNameTextBox);
                    _seleniumHelper.PassData(_driver, newGroupNameTextBox, _activityData.GroupName);
                    _seleniumHelper.MoveToElement(_driver, By.XPath("//*[@id='id_dropdown_addGroup']/button"));
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, By.XPath("//*[@id='id_dropdown_addGroup']/button"));
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_div_manageGrpTick_allgroups"));
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_addGroup"));
                    By newGroupName = By.Id($"id_span_nodeName_{_activityData.GroupName.ToLower()}");
                    _seleniumHelper.WaitUntilClickable(_driver, newGroupName);
                    if (_seleniumHelper.ElementExists(newGroupName, _driver))
                    {
                        _seleniumHelper.FindElementandClick(_driver, By.Id("id_btn_done"));
                        UpdateStatus("New Group created");
                    }
                }
            }
        }

        private void DiscoverDevices()
        {
            By addDeviceElement = By.Id("id_span_icon_discoverdevices");
            _seleniumHelper.WaitUntilClickable(_driver, addDeviceElement);
            if (_seleniumHelper.ElementExists(addDeviceElement, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, addDeviceElement);
            }
            UpdateStatus("Navigated to Printer discovery window");
            _driver.SwitchTo().ActiveElement().Click();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.XPath("//*[@id='id_dropdown_addGroup']/button/span[2]"));
            By groupName = By.Id($"id_div_groupSelect_{_activityData.GroupName.ToLower()}");
            if (_seleniumHelper.ElementExists(groupName, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, groupName);
                UpdateStatus("Selected the provided group");
            }
        }

        private void PrinterDiscoveryManualIPAddress()
        {
            DiscoverDevices();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_dropdown_discoveryType"));
            By manualDiscoveryType = By.Id("id_div_discoveryType_manual");
            _seleniumHelper.MoveToElement(_driver, manualDiscoveryType);
            _seleniumHelper.FindElementandClick(_driver, manualDiscoveryType);
            _seleniumHelper.FindElementandClick(_driver, By.Id("id_txtbox_ip_hostname"));
            SendKeys.SendWait(_activityData.ManualIPAddress);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_ip_hostname"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_discoverDevice"));
            ScheduleTask();
            UpdateStatus("Manual discovery - IPAddress configured");
        }

        private void PrinterDiscoveryDeviceListFile()
        {
            DiscoverDevices();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_dropdown_discoveryType"));
            By manualDiscoveryType = By.Id("id_div_discoveryType_manual");
            _seleniumHelper.MoveToElement(_driver, manualDiscoveryType);
            _seleniumHelper.FindElementandClick(_driver, manualDiscoveryType);
            Thread.Sleep(WaitDelay);
            IWebElement uploadFile = _driver.FindElement(By.Id("id_inputfile_file_upload"));
            uploadFile.SendKeys(_activityData.DeviceListPath);
            _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
            _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("progress"));
            Thread.Sleep(30);
            By discoveryDevice = By.Id("id_btn_discoverDevice");
            if (_seleniumHelper.ElementExists(discoveryDevice, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, discoveryDevice);
            }
            ScheduleTask();
            UpdateStatus("Printer Discovery - IPAddress File configured");
        }

        private void PrinterDiscoveryAutomaticHops()
        {
            DiscoverDevices();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_dropdown_discoveryType"));
            By automaticDiscoveryType = By.Id("id_div_discoveryType_automatic");
            _seleniumHelper.MoveToElement(_driver, automaticDiscoveryType);
            _seleniumHelper.FindElementandClick(_driver, automaticDiscoveryType);
            By networkHops = By.Id("id_txtbox_netHops");
            _seleniumHelper.FindElementandClick(_driver, networkHops);
            _driver.FindElement(networkHops).SendKeys(OpenQA.Selenium.Keys.LeftShift + OpenQA.Selenium.Keys.Home);
            _seleniumHelper.PassData(_driver, networkHops, _activityData.NumberOfHops.ToString());
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_discoverDevice"));
            ScheduleTask();
            UpdateStatus("Automatic discovery - network hops configured");
        }

        private void PrinterDiscoveryAutomaticRange()
        {
            DiscoverDevices();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_dropdown_discoveryType"));
            By automaticDiscoveryType = By.Id("id_div_discoveryType_automatic");
            _seleniumHelper.MoveToElement(_driver, automaticDiscoveryType);
            _seleniumHelper.FindElementandClick(_driver, automaticDiscoveryType);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_lbl_rangeIP"));
            By startIPAddressTextBox = By.Id("id_txtbox_startIP");
            By endIPAddressTextBox = By.Id("id_txtbox_endIP");
            _seleniumHelper.WaitUntilClickableAndClick(_driver, startIPAddressTextBox);
            _seleniumHelper.FindElementandClick(_driver, startIPAddressTextBox);
            _seleniumHelper.PassData(_driver, startIPAddressTextBox, _activityData.StartIPAddress);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, endIPAddressTextBox);
            _seleniumHelper.FindElementandClick(_driver, endIPAddressTextBox);
            _seleniumHelper.PassData(_driver, endIPAddressTextBox, _activityData.EndIPAddress);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_ip_range"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_discoverDevice"));
            ScheduleTask();
            UpdateStatus("Automatic discovery - IP range configured");
        }

        private void PrinterDiscoveryAssetInventory()
        {
            DiscoverDevices();
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_dropdown_discoveryType"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_div_discoveryType_manual"));
            List<IDeviceInfo> devices = _executionData.Assets.OfType<IDeviceInfo>().ToList();
            List<string> assetinfo = devices.Select(c => c.Address).ToList();
            By ipTextBox = By.Id("id_txtbox_ip_hostname");
            By addDeviceButton = By.Id("id_btn_ip_hostname");
            foreach (string device in assetinfo)
            {
                _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
                _seleniumHelper.WaitUntilClickableAndClick(_driver, ipTextBox);
                _seleniumHelper.FindElementandClick(_driver, ipTextBox);
                _seleniumHelper.PassData(_driver, ipTextBox, device);
                _seleniumHelper.WaitUntilVisible(_driver, addDeviceButton);
                if (_seleniumHelper.ElementExists(addDeviceButton, _driver))
                {
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, addDeviceButton);
                }
            }
            _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
            By DiscoverDeviceButton = By.Id("id_btn_discoverDevice");
            if (_seleniumHelper.ElementExists(DiscoverDeviceButton, _driver))
            {
                _seleniumHelper.WaitUntilClickableAndClick(_driver, DiscoverDeviceButton);
            }

            ScheduleTask();
            UpdateStatus("Manual discovery - Devices from AssetInverntory configured");
        }

        private void GenerateReports()
        {
            By reportsTab = By.Id("id_lst_menuitem_reports");
            _seleniumHelper.WaitUntilVisible(_driver, reportsTab);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, reportsTab);
            _seleniumHelper.MoveToElement(_driver, By.Id("id_span_icon_reports"));
            //Moving cursor back to the menu to get the focus of the form
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_span_icon_exportreports"));
            _seleniumHelper.MoveToElement(_driver, By.LinkText("Reports"));
            _seleniumHelper.FindElementandClick(_driver, By.Id("id_btn_filter"));
            _seleniumHelper.FindElementandClick(_driver, By.Id($"id_div_groupSelect_{ _activityData.GroupName.ToLower()}"));
            _seleniumHelper.FindElementandClick(_driver, By.Id("id_chkbox_rpt_all"));
            _seleniumHelper.FindElementandClick(_driver, By.Id("id_span_flatReprotFormatView"));
            _seleniumHelper.FindElementandClick(_driver, By.XPath("(//*[@id='id_span_flatReprotFormatView'])[2]"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_exportnow"));
            UpdateStatus("Reports generated");
        }

        private void ImportPolicy()
        {
            string policyFileName = string.Empty;
            policyFileName = Path.GetFileNameWithoutExtension(_activityData.PolicyFilePath);
            IWebElement selectedContainerElement;
            IWebElement gridElementsContainer;
            By policyMenuLinkLocator = By.LinkText("Policies");
            if (_seleniumHelper.ElementExists(policyMenuLinkLocator, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, policyMenuLinkLocator);
            }
            new WebDriverWait(_driver, TimeSpan.FromSeconds(ElementWaitDelay));
            if (!string.IsNullOrEmpty(_activityData.ExistingPolicyName))
            {
                By policyName = By.LinkText(_activityData.ExistingPolicyName);
                _seleniumHelper.WaitUntilExists(_driver, policyName);
                if (_seleniumHelper.ElementExists(policyName, _driver))
                {
                    policyFileName = _activityData.ExistingPolicyName.ToString();
                }
                PushPolicy(policyFileName);
            }
            else
            {
                _seleniumHelper.MoveToElement(_driver, By.Id("id_span_icon_policies"));
                _driver.FindElement(By.Id("id_span_icon_importpolicies")).Click();
                Thread.Sleep(WaitDelay);
                SendKeys.SendWait(_activityData.PolicyFilePath);
                SendKeys.SendWait("{Enter}");
                if ((!_seleniumHelper.IsVisible(_driver, By.Id("id_p_passPhrase"))) && _activityData.ValidatePolicyPath == true)
                {
                    Thread.Sleep(WaitDelay);
                    SendKeys.SendWait($"{_executionData.Credential.Domain }\\{ _executionData.Credential.UserName }");
                    Thread.Sleep(ShortWaitDelay);
                    SendKeys.SendWait("{Tab}");
                    Thread.Sleep(ShortWaitDelay);
                    SendKeys.SendWait(_executionData.Credential.Password);
                    Thread.Sleep(ShortWaitDelay);
                    SendKeys.SendWait("{Enter}");
                    Thread.Sleep(WaitDelay);
                }
                if (!_seleniumHelper.IsVisible(_driver, By.Id("id_txtbox_enterpassphrase")))
                {
                    _seleniumHelper.WaitUntilVisible(_driver, By.Id("id_txtbox_enterpassphrase"));
                }
                _seleniumHelper.FindElementandClick(_driver, By.Id("id_txtbox_enterpassphrase"));
                _seleniumHelper.PassData(_driver, By.Id("id_txtbox_enterpassphrase"), _activityData.PolicyPassword);
                _seleniumHelper.FindElementandClick(_driver, By.Id("id_btn_discoverDevice"));
                Thread.Sleep(2000);
                By fileName = By.Id("id_h3_importErrorTitle");
                if (_seleniumHelper.ElementExists(fileName, _driver))
                {
                    Thread.Sleep(WaitDelay);
                    ///Below IF condition is True if the password provided is incorrect. The IDs for the Import error pop-up have different IDs
                    if (_seleniumHelper.IsVisible(_driver, By.Id("id_txtbox_correctPassphrase")))
                    {
                        UpdateStatus("Policy password mismatch. Terminating the execution");
                        throw new LocksmithConfigurationException("Execution failed due to incorrect policy password provided.");
                    }
                    else
                    {
                        UpdateStatus("A policy with the same name already exists, Selecting the existing policy");
                        _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_cancelPopup"));
                        PushPolicy(policyFileName);
                    }
                }
                else
                {
                    //Waiting for Policy grid to load the list of policies
                    Thread.Sleep(WaitDelay);
                    //Copying all grid elements into temp
                    gridElementsContainer = _driver.FindElement(By.Id("id_grid_policyList"));
                    string policyName = Path.GetFileNameWithoutExtension(_activityData.PolicyFilePath);
                    selectedContainerElement = gridElementsContainer.FindElement(By.XPath($".//div/a[text()='{policyName}']"));
                    selectedContainerElement.Click();
                    Thread.Sleep(WaitDelay);
                    if (_seleniumHelper.ElementExists(By.XPath("//*[@id='id_btn_Close']"), _driver))
                    {
                        _seleniumHelper.FindElementandClick(_driver, By.XPath("//*[@id='id_btn_Close']"));
                    }

                    _seleniumHelper.FindElementandClick(_driver, By.Id("id_p_editPolicy"));
                    By validatebutton = By.Id("id_btn_validate");
                    _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
                    if (_seleniumHelper.ElementExists(validatebutton, _driver))
                    {
                        _seleniumHelper.FindElementandClick(_driver, validatebutton);
                    }
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_confirmDialog"));
                    By savebutton = By.Id("id_btn_save");
                    if (_seleniumHelper.ElementExists(savebutton, _driver))
                    {
                        _seleniumHelper.FindElementandClick(_driver, savebutton);
                    }
                    //Waiting 9 seconds for automatically close of the dynamic alert message
                    Thread.Sleep(PopupWaitDelay);
                    _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_closePopUp"));

                    UpdateStatus("Policy imported");
                    //To Assess and Remediate the task
                    PushPolicy(policyFileName);
                }
            }
        }

        private void PushPolicy(string selectionName)
        {
            By newTask = By.Id("id_p_newTasks");
            if (_seleniumHelper.ElementExists(newTask, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, newTask);
            }
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("selectedItem_Select Group(s)"));
            //Providing timeout to load devices in a list
            //Thread.Sleep(TimeSpan.FromSeconds(10)); ////Not too confident to remove this code but still keeping it commented .
            By groupName = By.Id($"id_chkbox_groupName_{ _activityData.GroupName }");
            if (_seleniumHelper.ElementExists(groupName, _driver))
            {
                _seleniumHelper.MoveToElement(_driver, groupName);
                _seleniumHelper.FindElementandClick(_driver, groupName);
            }
            By okButton = By.Id("id_btn_submitGroupSelection");
            _seleniumHelper.MoveToElement(_driver, okButton);
            _seleniumHelper.FindElementandClick(_driver, okButton);
            By assesandRemediate = By.Id("id_lbl_assessmentRemediateTypeSelection");
            By assesonly = By.Id("id_lbl_accessTypeSelection");
            _seleniumHelper.WaitUntilClickable(_driver, assesandRemediate);
            if (_activityData.AssessandRemediate)
            {
                _seleniumHelper.FindElementandClick(_driver, assesandRemediate);
                UpdateStatus("Assess and Remediate option is selected");
            }
            _seleniumHelper.WaitUntilClickable(_driver, assesonly);
            if (_activityData.AssessOnly)
            {
                _seleniumHelper.FindElementandClick(_driver, assesonly);
                UpdateStatus("Assess only option is selected");
            }
            _seleniumHelper.FindElementandClick(_driver, By.Id($"id_txtbox_taskName_{ _activityData.GroupName }"));
            _seleniumHelper.PassData(_driver, By.Id($"id_txtbox_taskName_{ _activityData.GroupName }"), $"{ _executionData.SessionId }_policytask");
            By popupExists = By.Id($"id_img_taskNameError_{ _activityData.GroupName}");
            if (_seleniumHelper.ElementExists(popupExists, _driver))
            {
                _seleniumHelper.FindElementandClick(_driver, By.Id($"id_txtbox_taskName_{ _activityData.GroupName }"));
                _seleniumHelper.PassData(_driver, By.Id($"id_txtbox_taskName_{ _activityData.GroupName }"), Guid.NewGuid().ToString("N").Substring(0, 8));
            }
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.ClassName("ls-select-icon"));
            By policyExists = By.Id($"id_img_taskNameError_{ _activityData.GroupName}");
            if (!_seleniumHelper.ElementExists(policyExists, _driver))
            {
                UpdateStatus("Unable to find the policy name in the dropdown list");
            }
            _seleniumHelper.FindElementandClick(_driver, By.Id($"id_div_policySelectionChange_{Regex.Replace(selectionName, @"\s+", "")}"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_btn_startNewTask"));
            //Waiting 9 seconds for automatically close of the dynamic alert message
            Thread.Sleep(PopupWaitDelay);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_p_refresh"));
            ScheduleTask();
            UpdateStatus("Successfully Pushed the policy");
        }

        private void ScheduleTask()
        {
            //Waiting 9 seconds for automatically close of the dynamic alert message
            Thread.Sleep(PopupWaitDelay);
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_lst_menuitem_tasks"));
            //waiting for the elements of the grid
            Thread.Sleep(WaitDelay);
            //Capturing grid elements to the temp
            IWebElement gridElementsContainer = _driver.FindElement(By.Id("id_grid_taskList"));
            //Finding the checkbox of recently 'sheduled'checkbox
            By Scheduledtask = By.XPath(".//span[text()='Scheduled']");
            if (_seleniumHelper.ElementExists(Scheduledtask, _driver))
            {
                IWebElement selectedContainerElement = gridElementsContainer.FindElement(Scheduledtask);
                selectedContainerElement.Click();
                _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_p_rerun"));
                //Waiting 9 seconds for automatically close of the dynamic alert message
                Thread.Sleep(PopupWaitDelay);
            }
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_p_refresh"));
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ElementWaitDelay);
            _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
            _seleniumHelper.WaitUntilClickableAndClick(_driver, By.Id("id_p_refresh"));
            _seleniumHelper.WaitUntilInvisible(_driver, By.ClassName("spinner"));
        }

        private void CopyDriverFiles(LockSmithConfigurationActivityData _activitydata)
        {
            ExecutionServices.SystemTrace.LogDebug("Copying Selenium _driver files...");
            StringBuilder exePath = new StringBuilder(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            if (_activityData.Browser == BrowserType.InternetExplorer)
            {
                exePath.Append("\\IEDriverServer.exe");
                if (!File.Exists(exePath.ToString()))
                {
                    UpdateStatus("Copying Internet explorer web driver");
                    File.WriteAllBytes(exePath.ToString(), Resource.IEDriverServer);
                }
                InternetExplorerOptions options = new InternetExplorerOptions();
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                options.IgnoreZoomLevel = true;
                _driver = new InternetExplorerDriver(options);
                UpdateStatus("Launching Internet explore");
            }
            else if (_activityData.Browser == BrowserType.GoogleChrome)
            {
                exePath.Append("\\chromedriver.exe");
                if (!File.Exists(exePath.ToString()))
                {
                    UpdateStatus("Copying Google chrome web driver");
                    File.WriteAllBytes(exePath.ToString(), Resource.chromedriver);
                }
                _driver = new ChromeDriver();
                UpdateStatus("Launching google chrome");
            }
            ExecutionServices.SystemTrace.LogDebug("Files copied...");
        }

        private void UpdateStatus(string status)
        {
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
