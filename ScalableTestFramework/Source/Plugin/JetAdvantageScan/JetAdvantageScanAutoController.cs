using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    public class JetAdvantageScanAutoController : ScanActivityManager
    {
        private const string OXP_MAIN_FORM = "HPForm";
        private readonly JetAdvantageScanActivityData _data;

        private readonly JediWindjammerDevice _device;
        private readonly OxpdBrowserEngine _engine;

        // Confirm delete button coordinates
        private const int X_DELETE = 475;
        private const int Y_DELETE = 440;
        private const int LongDelay = 2000;

        protected override string ScanType => "JetAdvantageScan";

        /// <summary>
        /// HP JetAdvantage constructor
        /// </summary>
        public JetAdvantageScanAutoController(PluginExecutionData executionData, ScanOptions scanOptions)
            : base(executionData)
        {
            var device = (IDeviceInfo)executionData.Assets.First();
            _device = new JediWindjammerDevice(device.Address, device.AdminPassword);
            _data = executionData.GetMetadata<JetAdvantageScanActivityData>();
            _engine = new OxpdBrowserEngine(_device.ControlPanel);
            ScanOptions = scanOptions;
        }

        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            SignIn();
            RunHPJetAdvantage();
            return new PluginExecutionResult(PluginResult.Passed);
        }
        protected override void SetupJob(IDevice device)
        {
            _device.ControlPanel.PressKey(JediHardKey.Menu);
        }

        /// <summary>
        /// Signs into the HP JetAdvantage Cloud
        /// </summary>
        public virtual void SignIn()
        {
            // Press the HP JetAdvantage (Titan) button
            string controlName = "mAccessPointDisplay";
            _device.ControlPanel.WaitForControl(controlName, TimeSpan.FromSeconds(200));
            PressAppButtonByProperty(controlName, "Title", "HP JetAdvantage", OXP_MAIN_FORM, ignorePopups: true);

            // if the email textbox is available than able to access the server
            if (_engine.WaitForHtmlContains("email-textbox", TimeSpan.FromSeconds(15)))
            {
                UpdateStatus("Selecting User.");
                SelectUser("different-user");

                // set the login ID and password
                UpdateStatus("Filling out HP JetAdvantage login and password data.");
                EnterLoginPassword();

                if (_data.UseLoginPin)
                {
                    //click create pin
                    UpdateStatus("Selecting Create PIN.");
                    PressButton("create-pin-checkbox");

                    //Login to JetAdvantage
                    UpdateStatus("Pressing the HP JetAdvantage login Button");
                    PressButton("login-button");

                    _engine.WaitForHtmlContains("keypad", TimeSpan.FromSeconds(15));
                    _device.ControlPanel.Type(_data.JetAdvantageLoginPin);

                    PressButton("dialog normal show", "dialog-confirm-button button-keypad primary done");
                }
                else
                {
                    UpdateStatus("Pressing the HP JetAdvantage login Button");
                    PressButton("login-button");
                }

                _engine.WaitForHtmlContains("hp-scansend", TimeSpan.FromSeconds(15));
            }
            else
            {
                throw new Exception("Unable to sign in to HP JetAdvantage.");
            }
        }

        /// <summary>
        /// Configures the scan settings into the browser
        /// </summary>
        private void ConfigureScanSettings()
        {
            RadioButtonClickCreateFunction();

            //Configure Duplex Mode
            UpdateStatus("Configuring the Duplex mode changes");
            PressButton("plexMode-button");
            _engine.WaitForHtmlContains("dialog setting-popup normal show", TimeSpan.FromSeconds(5));
            SelectRadioButton("popupSetting", _data.Settings.DuplexMode);
            PressButton("dialog setting-popup normal show", "dialog-confirm-button primary");

            //Configure PaperSize
            PressButton("mediaSize-button");
            UpdateStatus("Configuring the Paper size changes");
            _engine.WaitForHtmlContains("dialog setting-popup normal show", TimeSpan.FromSeconds(5));
            SelectRadioButton("popupSetting", _data.Settings.PaperSize);
            PressButton("dialog setting-popup normal show", "dialog-confirm-button primary");

            //Configure Orientation
            PressButton("mediaOrientation-button");
            UpdateStatus("Configuring the Orientation changes");
            _engine.WaitForHtmlContains("dialog setting-popup normal show", TimeSpan.FromSeconds(5));
            SelectRadioButton("popupSetting", _data.Settings.Orientation);
            PressButton("dialog setting-popup normal show", "dialog-confirm-button primary");

            //Configure More Settings
            PressButton("more-settings-button");
            UpdateColumnCreateFunction();

            //Configure scanFileType
            SelectRadioButton("options", "fileType");
            UpdateStatus("Configuring the file type for saving");
            UpdateColumn("saveAs-column", "fileType-column");
            SelectRadioButton("fileType", _data.Settings.FileType);

            //return Back to Scan App
            //_engine.RetrieveHtml();
            if (_data.UseAdf)
            {
                SelectRadioButton("options", "mediaSource");
                UpdateStatus("Configuring the scan setting to pick document from ADF");
                UpdateColumn("saveAs-column", "mediaSource-column");
                SelectRadioButton("mediaSource", "Adf");
            }
            PressButton("settings-ok-button");
        }


        /// <summary>
        /// Creates a javascript function for selecting the radio button in html.
        /// </summary>
        private void RadioButtonClickCreateFunction()
        {
            _engine.ExecuteJavaScript(@" function selectRadioButton(controlName, controlValue)
                                                {
                                                    var bound = '0';
                                                    var allElements = document.getElementsByName(controlName);
                                                    for (i = 0; i < allElements.length; i++)
                                                    {   if (allElements[i].value == controlValue)
                                                         {
                                                            allElements[i].click();
                                                            //allElements[i].focus();
                                                            break;
                                                         }
                                                    }
                                                }");
        }

        /// <summary>
        /// Creates a javascript function for updating the Column with new Section.
        /// </summary>
        private void UpdateColumnCreateFunction()
        {
            _engine.ExecuteJavaScript(@"function UpdateColumn(currentSectionId, newSectionId)
                                                {
                                                    var Element;
                                                    var Element2;
                                                    Element = document.getElementById(currentSectionId);
                                                    Element.className = 'data-column hidden';
                                                    Element2 = document.getElementById(newSectionId);
                                                    Element2.className = 'data-column';
                                                }");
        }

        /// <summary>
        /// Function call for Updating the Column based on the Current and New Section ID
        /// </summary>
        /// <param name="currentSectionId"></param>
        /// <param name="newSectionId"></param>
        private void UpdateColumn(string currentSectionId, string newSectionId)
        {
            string script = "UpdateColumn('" + currentSectionId + "','" + newSectionId + "'); ";
            var rtn = _engine.ExecuteJavaScript(script);
            Thread.Sleep(200);
        }

        /// <summary>
        /// Function call for radio button selection from the dialog id and class name
        /// </summary>
        /// <param name="dialogId">string</param>
        /// <param name="objectId">string</param>
        private void SelectRadioButton(string dialogId, string objectId)
        {
            string script = "selectRadioButton('" + dialogId + "','" + objectId + "');";
            var rtn = _engine.ExecuteJavaScript(script);
            Thread.Sleep(200);
        }

        /// <summary>
        /// Presses the application button by property name/value lookup
        /// </summary>
        /// <param name="scrollControlName">Name of the scroll control.</param>
        /// <param name="propertyNameToSearch">The property name to search.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="destinationFormName">Name of the destination form.</param>
        /// <param name="ignorePopups">if set to <c>true</c> [ignore popups].</param>
        private void PressAppButtonByProperty(string scrollControlName, string propertyNameToSearch, string searchValue, string destinationFormName, bool ignorePopups)
        {
            try
            {
                // First try
                _device.ControlPanel.ScrollPressNavigate(scrollControlName, propertyNameToSearch, searchValue, destinationFormName, ignorePopups: ignorePopups);
            }
            catch (WindjammerInvalidOperationException ex)
            {
                if (HandlePossibleDuplicateControlException(ex, propertyNameToSearch, searchValue, scrollControlName, destinationFormName, ignorePopups)) return;
                if (HandleUnexpectedFormException(ex, destinationFormName)) return;
                throw ex;
            }
        }

        /// <summary>
        /// Presses application button by name.
        /// </summary>
        /// <param name="scrollControlName">Name of the scroll control.</param>
        /// <param name="buttonName">Name of the button.</param>
        /// <param name="expectedDestinationFormPatternMatch">The expected destination form pattern match.</param>
        /// <param name="ignorePopups">if set to <c>true</c> [ignore popups].</param>
        private void PressAppButtonByName(string scrollControlName, string buttonName, string expectedDestinationFormPatternMatch, bool ignorePopups)
        {
            try
            {
                // First try
                _device.ControlPanel.ScrollPressNavigate(scrollControlName, buttonName, expectedDestinationFormPatternMatch, ignorePopups: ignorePopups);
            }
            catch (WindjammerInvalidOperationException ex)
            {
                if (HandleUnexpectedFormException(ex, expectedDestinationFormPatternMatch)) return;
                // re-throw if we made it here since we don't know how to handle
                throw ex;
            }
        }

        /// <summary>
        /// Handles possible duplicate control exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="propertyNameToSearch">The property name to search.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="scrollControlName">Name of the scroll control.</param>
        /// <param name="destinationFormName">Name of the destination form.</param>
        /// <param name="ignorePopups">if set to <c>true</c> [ignore popups].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool HandlePossibleDuplicateControlException(WindjammerInvalidOperationException ex, string propertyNameToSearch, string searchValue, string scrollControlName, string destinationFormName, bool ignorePopups)
        {
            bool result = false;

            // Failure can occur if there are more than 1 control that meets the property name/value criteria
            // Check if we have that condition and if so try to press them individually.
            if (Regex.IsMatch(ex.Message, "control .* not (visible|enable)", RegexOptions.IgnoreCase))
            {
                var allControlNames = _device.ControlPanel.GetControls();
                foreach (var name in allControlNames)
                {
                    // try looking for that property name on each control
                    try
                    {
                        string propValue = _device.ControlPanel.GetProperty(name, propertyNameToSearch);
                        if (propValue.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                PressAppButtonByName(scrollControlName, name, destinationFormName, ignorePopups);

                                // if we made it to here then we pressed the intended control
                                result = true;
                            }
                            catch (Exception)
                            {
                                // keep looking since this threw 
                            }
                        }
                    }
                    catch
                    {
                        // property name was invalid for that control so ignore and move on
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Handles unexpected form exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="expectedDestinationFormPatternMatch">The expected destination form pattern match.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool HandleUnexpectedFormException(WindjammerInvalidOperationException ex, string expectedDestinationFormPatternMatch)
        {
            var result = false;

            // Failure can occur if there are more than 1 control that meets the property name/value criteria
            // Check if we have that condition and if so try to press them individually.
            if (Regex.IsMatch(ex.Message, "expected form", RegexOptions.IgnoreCase))
            {
                var actualForm = _device.ControlPanel.CurrentForm();
                if (Regex.IsMatch(actualForm, expectedDestinationFormPatternMatch, RegexOptions.IgnoreCase))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Runs the HP JetAdvantage automated process.
        /// </summary>
        private void RunHPJetAdvantage()
        {
            _engine.GetBrowserHtml();
            // press the scan to cloud button
            UpdateStatus("Preparing for the Helios Scan to Cloud.");
            PressButton("hp-scansend");
            UpdateStatus("Scan App is Launched.");
            string htm1 = _engine.GetBrowserHtml();
            if (!HasCloudStorageError(htm1))
            {
                ScanDocuments();
            }
            else
            {
                PressButton("choice-error-dialog-template-inst8", "dialog - dismiss - button tertiary");
                UpdateStatus("No Cloud Storage available, logging out.");
            }

            Logout();

        }

        /// <summary>
        /// Function for scanning the document.
        /// </summary>
        private void ScanDocuments()
        {
            _engine.WaitForHtmlContains("scan-button", TimeSpan.FromSeconds(30));
            ConfigureScanSettings();
            UpdateStatus("Scanning the document");
            //string htm = _engine.RetrieveHtml();
            PressButton("scan-button");
            //_engine.RetrieveHtml();
            _engine.WaitForHtmlContains("dialog success show", TimeSpan.FromSeconds(50));
            //_engine.RetrieveHtml();
            PressButton("dialog success show", "dialog-dismiss-button tertiary");
            UpdateStatus("Scan is completed and the dialog is closed");
        }

        /// <summary>
        /// Returns true if no cloud storage found.
        /// </summary>
        /// <param name="htmlString">string</param>
        /// <returns>bool</returns>
        private bool HasCloudStorageError(string htmlString) => htmlString.ToLower().Contains("dialog warning show");

        /// <summary>
        /// Logout from HP JetAdvantage and return to the home screen.
        /// </summary>
        private void Logout()
        {
            UpdateStatus("Pressing the Back button.");
            PressButton("back-button");

            UpdateStatus("Pressing the Log Out button.");

            PressButton("id-button-logout");

            _engine.WaitForHtmlContains("email-textbox", TimeSpan.FromSeconds(15));

            // home screen
            UpdateStatus("Returning to the home screen.");
            _device.ControlPanel.PressKey(JediHardKey.Menu);
        }

        /// <summary>
        /// Presses the given control via a coordinate system.
        /// </summary>
        /// <param name="buttonId">string</param>
        private void PressButton(string buttonId)
        {
            PressElement(buttonId);
            Thread.Sleep(LongDelay);
        }

        /// <summary>
        /// Presses the given control via a coordinate system.
        /// </summary>
        /// <param name="className">string</param>
        /// <param name="className2">string</param>
        private void PressButton(string className, string className2)
        {
            PressElement(className, className2);
            Thread.Sleep(LongDelay);
        }

        /// <summary>
        /// Process for entering JetAdvantage Login and password
        /// </summary>
        private void EnterLoginPassword()
        {
            PlayKeyBoard("email-textbox", _data.JetAdvantageLoginId);
            Thread.Sleep(LongDelay);

            PlayKeyBoard("password-textbox", _data.JetAdvantagePassword);
            Thread.Sleep(LongDelay);
        }

        /// <summary>
        /// method to select the user.
        /// </summary>
        private void SelectUser(string objectId)
        {
            PressElement(objectId);
            Thread.Sleep(LongDelay);
        }

        /// <summary>
        /// Writes the given text into the given control ID.
        /// </summary>
        /// <param name="objectId">string</param>
        /// <param name="textToType">int</param>
        private void PlayKeyBoard(string objectId, string textToType)
        {
            PressElement(objectId);
            _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", textToType);

            Thread.Sleep(LongDelay);

            _device.ControlPanel.Press("ok");
        }

        /// <summary>
        /// Presses the given control.
        /// </summary>
        /// <param name="objectId">string</param>
        private void PressElement(string objectId)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaById(objectId);
            if (boundingBox.Left == 0 && boundingBox.Right == 0)
            {
                boundingBox = _engine.GetBoundingAreaById(objectId);
            }
            _engine.PressElementByBoundingArea(boundingBox);
        }

        /// <summary>
        /// Presses a given control from Object id and Object class name
        /// </summary>
        /// <param name="className">string</param>
        /// <param name="className2">string</param>
        private void PressElement(string className, string className2)
        {
            string script = $"document.getElementsByClassName('{className}')[0].getElementsByClassName('{className2}')[0]";

            BoundingBox boundingBox = _engine.GetElementBoundingArea(script);
            if (boundingBox.Left == 0 && boundingBox.Right == 0)
            {
                boundingBox = _engine.GetElementBoundingArea(script);
            }
            _engine.PressElementByBoundingArea(boundingBox);
        }


    }
}
