using System;
using System.Linq;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HtmlAgilityPack;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    internal class HpacJedi
    {
        private readonly JediWindjammerDevice _jediDevice;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(5));
        private readonly TimeSpan _shortDelay = TimeSpan.FromSeconds(5);
        private readonly TimeSpan _mediumDelay = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _longDelay = TimeSpan.FromSeconds(60);
        private OxpdBrowserEngine _engine;
        private const string PrintAll = "ButtonPrintAll";
        private const string PrintAndDelete = "ButtonSelect";
        private const string PrintAndKeep = "ButtonRetain";
        private readonly NetworkCredential _credential;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="device"></param>
        /// <param name="credential"></param>
        public HpacJedi(IDevice device, NetworkCredential credential)
        {
            _jediDevice = device as JediWindjammerDevice;
            _credential = credential;
        }


        /// <summary>
        /// Navigate to Jedi HPAC
        /// </summary>
        public void NavigateToJediHpac()
        {
            _engine = new OxpdBrowserEngine(_jediDevice.ControlPanel);

            try
            {
                _jediDevice.ControlPanel.PressToNavigate("43617065-6C6C-614D-4A20-110209495001", "SignInForm", true);
                // Thread.Sleep(_shortDelay);
                _jediDevice.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.UserName.Substring(1));
                //Thread.Sleep(_mediumDelay);
                _jediDevice.ControlPanel.PressToNavigate("Enter", "OxpUIAPPMainForm800X600", true);
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogDebug(
                    $"Navigation failed with exception: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// PerformJediHpacTasksOnCp
        /// </summary>
        /// <param name="action"></param>
        public void PerformJediHpacTasksOnCp(SolutionOperation action)
        {
            try
            {
                bool success =
                    Wait.ForTrue(
                        () =>
                            _jediDevice.ControlPanel.CurrentForm().StartsWith("OXP", StringComparison.OrdinalIgnoreCase),
                        _longDelay);
                if (success)
                {
                    // Get the document id - refer to SampleHpacPrint.html for example
                    var results = GetDocumentsInfo();
                    string checkBoxId = "";
                    if (results != null)
                    {
                        checkBoxId = results.ElementAt(0).Id;
                    }
                    ExecutionServices.SystemTrace.LogDebug($"CheckBox Id of first print job: { (object)checkBoxId}");
                    //_engine.ExecuteJavaScript(HpacSelectFirstDocument); //Selects the First Document
                    if (results != null)
                    {
                        switch (action)
                        {
                            case SolutionOperation.PrintKeep:
                                {
                                    ClickById(checkBoxId, _shortDelay);
                                    ClickPrintJobsByName(PrintAndKeep);
                                    ClickById(PrintAndKeep, _shortDelay);
                                }
                                break;

                            case SolutionOperation.PrintDelete:
                                {
                                    ClickById(checkBoxId, _shortDelay);
                                    ClickPrintJobsByName(PrintAndDelete);
                                    ClickById(PrintAndDelete, _shortDelay);
                                }
                                break;

                            case SolutionOperation.Print:
                                {
                                    ClickById(checkBoxId, _shortDelay);
                                    ClickPrintJobsByName(PrintAll);
                                    ClickById(PrintAll, _shortDelay);
                                }
                                break;

                            case SolutionOperation.Cancel:
                                {
                                    ClickPrintJobsByName(PrintAll);
                                    ClickById(PrintAll, _mediumDelay);
                                    _jediDevice.ControlPanel.PressKey(JediHardKey.Menu);
                                    Thread.Sleep(_mediumDelay);

                                    _jediDevice.ControlPanel.PressToNavigate("mStopButton", "PauseDevicePopup", true);
                                    Thread.Sleep(TimeSpan.FromSeconds(15));

                                    _jediDevice.ControlPanel.PressToNavigate("m_CancelButton", "TwoButtonMessageBox", true);
                                    Thread.Sleep(TimeSpan.FromSeconds(3));

                                    _jediDevice.ControlPanel.Press("m_OKButton");
                                }
                                break;

                            case SolutionOperation.PrintAll:
                                {
                                    ClickById(PrintAll, _shortDelay);
                                }
                                break;
                            case SolutionOperation.UIValidation:
                                {
                                    string result = _engine.ExecuteJavaScript(this.CheckButtonRefreshValue);
                                    if (result.ToLower() == "\"true\"")
                                    {
                                        ClickById(checkBoxId, _shortDelay);
                                        var res = _engine.ExecuteJavaScript(this.CheckButtonDeleteValue);
                                        {
                                            if (res.ToLower() == "\"false\"")
                                            {
                                                throw new Exception("The button did not toggle to Delete");
                                            }
                                        }
                                    }
                                    else
                                    { throw new Exception("The button did not toggle to Refresh"); }

                                }
                                break;
                            default:
                                {
                                    _jediDevice.ControlPanel.PressKey(JediHardKey.Menu);
                                    throw new Exception($"{action} is not supported on Jedi devices");
                                }
                        }


                        //Press home sceeen
                        _jediDevice.ControlPanel.PressKey(JediHardKey.Menu);
                        _pacekeeper.Pause();

                        //Press on Signout
                        _jediDevice.ControlPanel.Press("mSignInButton");
                    }
                }
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogDebug(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// ClickPrintJobsByName
        /// </summary>
        /// <param name="name"></param>
        public void ClickPrintJobsByName(string name)
        {
            string script = $"this.document.form1.{name}.checked = \"checked\";setLabel(this.document.form1.{name}, 'DOCUMENT INFO', 'noGuid', 'Refresh', 'Delete');";
            try
            {
                _engine.ExecuteJavaScript(script);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogDebug(
                    $"Exception in Printing jobs by name: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ClickPrintBy
        /// </summary>
        /// <param name="id">the id of control</param>
        /// <param name="delay">the delay after the click event</param>
        public void ClickById(string id, TimeSpan delay)
        {
            string findById = $@"document.getElementById('{id}')";

            try
            {
                CreateFunction(@"function ClickByMouseEvent(element) { var event = document.createEvent('MouseEvents'); event.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null); element.dispatchEvent(event);}");
                string script = "ClickByMouseEvent(" + findById + ")";
                _engine.ExecuteJavaScript(script);
                Thread.Sleep(delay);
            }
            catch (JavaScriptExecutionException)
            {
                // Try clicking the button using a javascript event instead
                string script = findById + ".click()";
                _engine.ExecuteJavaScript(script);
            }
        }
        public string CheckButtonRefreshValue
        {
            get
            {
                return "function BoolCheckByValue(){var val = document.getElementById('ButtonDelete').value;var result =  val == 'Refresh'?'true':'false'; return result;}BoolCheckByValue();";
            }
        }
        public string CheckButtonDeleteValue
        {
            get
            {
                return "function BoolCheckByValue(){var val = document.getElementById('ButtonDelete').value;var result =  val == 'Delete'?'true':'false'; return result;}BoolCheckByValue();";
            }
        }

        /// <summary>
        /// Creates the specified JavaScript function if it does not already exist.
        /// </summary>
        /// <param name="function">The function.</param>
        public void CreateFunction(string function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            // Extract the function name
            string functionName = function.Substring(0, function.IndexOf('(')).Replace("function", string.Empty).Trim();

            // Check to see if the function is already defined
            string result = _engine.ExecuteJavaScript($"typeof({functionName})");
            if (result.Equals("\"undefined\"", StringComparison.OrdinalIgnoreCase))
            {
                // Execute the script to create the function
                _engine.ExecuteJavaScript(function);
            }
        }


        /// <summary>
        /// Gets the list of document ids currently displayed
        /// The expectation is that each document/job name coming from STF will contain at least a partial GUID.  
        /// This function parses looking for this pattern and extracting the GUIDs.
        /// </summary>
        /// <returns>List of document ids</returns>
        public HtmlNodeCollection GetDocumentsInfo(bool distinctOnly = true)
        {
            string docListXpath = "//table[@id='scrollingContent']//input[@type='checkbox']";

            string rawHtml = string.Empty;
            HtmlNodeCollection nodes;
            try
            {
                rawHtml = _engine.GetBrowserHtml();
                var doc = new HtmlDocument();
                doc.LoadHtml(rawHtml);
                nodes = doc.DocumentNode.SelectNodes(docListXpath);
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("Error getting last document id", ex);
                ExecutionServices.SystemTrace.LogDebug($"Retrieved HTML:\n{rawHtml}");
                throw;
            }
            return nodes;
        }
    }
}
