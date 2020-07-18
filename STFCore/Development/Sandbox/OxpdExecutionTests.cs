using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Development
{
    public class OxpdExecutionTests
    {

        public static void Go()
        {
            //JediDevice device = null;
            //IEnumerable<string> controls;
            //string html;
            Collection<string> htmls = new Collection<string>();

            //GlobalSettings.Load("stfsystem03");            
            //using (var context = new AssetInventoryContext())
            //{
            //    var assets = context.Assets.Where(x => x.AssetId.StartsWith("LTL"));
            //    var asset = assets.First();                
            //}

            try
            {
                //var ip = "15.198.215.83";
                //var ip = "15.198.212.155";
                //var ip = "15.198.212.237";
                //var ip = "15.198.212.109";
                //var ip = "15.198.212.63";
                //var ip = "15.198.212.54";  // BJ OXPD TESTING, has Pull Print (safecom) and HPCR Scan To Me
                //var ip = "15.198.212.226"; // has HP AC Secure Pull Print
                //"15.198.212.210";
                //var ip = "15.198.212.73"; // has Safecom Pull Print

                var credential = new NetworkCredential("u00001", "1qaz2wsx", "etl.boi.rd.adapps.hp.com");
                //var harness = new Harness();
                //harness.Go("LTL-12739", ip, credential);


                //PullPrint(credential, ip);
                //HpecMyWorkflow(credential, ip);
                //HpecViaActivityData(credential, ip);
                //HpacPullPrint(credential, ip);


                var results = new Dictionary<string, string>();
                var printers = new List<string>() {
                    "15.198.212.150", // Autobahn
                    "15.198.212.97", // Autobahn
                    "15.198.212.98", // Autobahn
                    "15.198.212.152", // Autobahn WF
                    "15.198.212.91", // Autobahn WF
                    "15.198.212.127", // Autobahn WF
                    "15.198.212.155", // Autobahn WF
                    "15.198.215.83",  // Denali WF
                    "15.198.212.84", //Azalea
                    "15.198.212.247", //Denali WF
                    "15.198.212.248", //Denali WF
                    "15.198.212.228", //Everest
                    "15.198.212.19", //Fiji
                };

                //var printers = new List<string>() {
                //    "15.198.215.83", 
                //    "15.198.212.84", //Azalea
                //    "15.198.212.247", //Denali WF
                //    "15.198.212.248", //Denali WF
                //    "15.198.212.228", //Everest
                //    "15.198.212.19", //Fiji
                //};


                foreach (var printer in printers)
                {
                    var result = GetDeviceStatusFromOID(printer);
                    results.Add(printer, result);
                    System.Diagnostics.Debug.WriteLine("{0}: {1}", printer, result);
                }

                //GetLatestJobStats(ip);
                //device.Home();

            }
            catch (Exception)
            {

            }
        }

        static private string GetDeviceStatusFromOID(string ip)
        {
            var _device = new JediDevice(ip);
            _device.PowerManagement.Wake();

            Thread.Sleep(5000);

            // Keep checking on device status to see if job(s) have completed

            // check if device is ready by OID
            //var foo = _device.DeviceStatus;
            //var deviceStatus = _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0");
            //return deviceStatus;
            return string.Empty;
        }


        static private bool CheckDevicePrintingStatusByOid(string ip)
        {
            bool printingFinished = false;
            var deviceStatus = GetDeviceStatusFromOID(ip);
            if (deviceStatus.StartsWith("ready", StringComparison.OrdinalIgnoreCase))
            {
                printingFinished = true;
            }
            else if (deviceStatus.Contains("processing", StringComparison.OrdinalIgnoreCase))
            {
                printingFinished = false;
            }
            else
            {
                throw new Exception("Unexpected device status: " + deviceStatus);
            }
            TraceFactory.Logger.Debug("Device printing status via OID = {0}, {1}".FormatWith(printingFinished, deviceStatus));
            return printingFinished;
        }


        /// <summary>
        /// Gets stats for the latest completed job
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        static private void GetLatestJobStats(string ip)
        {
            var device = new JediWindjammerDevice(ip, "admin");
            var engine = new JavaScriptEngine(device.ControlPanel);

            //WaitForDeviceToFinish(device);
            device.ControlPanel.PressKeyWait(JediHardKey.Reset, "HomeScreenForm");
            device.ControlPanel.ScrollPressNavigate("mAccessPointDisplay", "Title", "Job Status", "JobStatusMainForm", true);
            var cp = device.ControlPanel;
            var firstJobButtonText = cp.GetProperty("mLoggedJobsListBox_Item0", "Column1Text");
            if (firstJobButtonText == string.Empty)
            {
                TraceFactory.Logger.Error("No job history found");
            }
            else
            {
                //cp.Press("mLogTab");
                cp.PressToNavigate(firstJobButtonText, "JobStatusMainForm", true);
                cp.PressToNavigate("mDetailsButton", "JobDetailsForm", true);
                var doc = device.ControlPanel.GetProperty("m_WebBrowser", "DocumentHTML");
            }           
        }

        private void WaitForDeviceToFinish(JediWindjammerDevice device)
        {
            bool finished = false;
            DateTime end = DateTime.Now + TimeSpan.FromMinutes(1);
            while (!finished && DateTime.Now < end)
            {
                finished = DeviceJobStatusFinishedByControlPanel(device);
                if (finished)
                {
                    break;
                }
                else
                {
                    // go to sleep and try again
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }

            if (!finished)
            {
                throw new Exception("Did not finish in time");
            }
        }

        private bool DeviceJobStatusFinishedByControlPanel(JediWindjammerDevice device)
        {
            bool finished = false;
            try
            {
                device.ControlPanel.PressKeyWait(JediHardKey.Reset, "HomeScreenForm");
                device.ControlPanel.ScrollPressNavigate("mAccessPointDisplay", "Title", "Job Status", "JobStatusMainForm", true);
                var cp = device.ControlPanel;
                var activeJobButtonText = cp.GetProperty("mActiveJobListBox_Item0", "Column1Text");
                if (activeJobButtonText == string.Empty)
                {
                    finished = true;
                }
                else
                {
                    finished = false;
                    if (activeJobButtonText == null)
                    {
                        // inconclusive, could not find expected button which means unexpected screen may be up
                    }
                }
                TraceFactory.Logger.Debug("Device active job via control panel = {0}".FormatWith(activeJobButtonText));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error checking job status via control panel", ex);
            }
            return finished;
        }


        private void HpecMyWorkflow(System.Net.NetworkCredential user, string ip)
        {
//            //var device = new JediWindjammerDevice(ip, "admin");
//            //var engine = new JavaScriptEngine(device.ControlPanel);

//            //var controller = new HpecJediDeviceController(user, device);
//            //try
//            //{
//            //    var entryButtonTitle = "My workflow (FutureSmart)";
//            //    if (controller.LaunchAndWaitForHtml(entryButtonTitle, "Send to Email"))
//            //    {
//            //        var html = string.Empty;
//            //        var cp = device.ControlPanel;
//            //        var currentForm = cp.CurrentForm();

//                    //"var checklist = document.getElementsByClassName(""checkbox""); if (checklist.length > 0) { checklist[checklist.length - 1].onclick.apply(checklist[checklist.length - 1]); }"
////                    var javaButtonClick = @"
////                    var buttons = document.getElementsByClassName(""labelOXPd""); 
////                    if (buttons.length > 0) { buttons[buttons.length - 1].onclick.apply(checklist[checklist.length - 1]); }
////                    ";

//                    var javaButtonCount = @"
//                        function getItemCount(){var documents = document.getElementsByClassName(""labelOXPd""); return documents.length;}
//                        getItemCount()
//                    ";

//                    // Expecting HTML similar to:
//                    //  <table id="scrollingContent" class="scrollingContentOXPd">
//                    //  <tbody><tr>
//                    //    <td valign="top">
//                    //      <table id="list1" class="leveledBoxOXPd" style="width:100%;">
//                    //                      <tbody><tr>
//                    //          <td onmouseout="this.className='';" onmousedown="this.className='leveledTDOXPdDown'; this.onclick=RequestNewScreen('value=OnButtonClicked&amp;id=d6ae0641-0350-4e69-b61e-bf88e8cdd6f6', null, true);try{window.OXPd.beep()}catch(err){};">
//                    //            <div class="labelOXPd">
//                    //              Send to Email
//                    //            </div>
//                    //          </td>
//                    //        </tr>
//                    //                      <tr>
//                    //          <td onmouseout="this.className='';" onmousedown="this.className='leveledTDOXPdDown'; this.onclick=RequestNewScreen('value=OnButtonClicked&amp;id=b05887dd-d96d-41dd-8100-16adc6b38c99', null, true);try{window.OXPd.beep()}catch(err){};">
//                    //            <div class="labelOXPd">
//                    //              Send to Network Folder
//                    //            </div>
//                    //          </td>
//                    //        </tr>
//                    //                    </tbody></table>
//                    //    </td>
//                    //  </tr>
//                    //</tbody></table>

//                    var javaPressWorkflowButton = @"
//                        function getButtons(pClassName){var buttons = document.getElementsByClassName(pClassName); return buttons;}
//                        function getButtonByText(pText){
//                            var buttons = getButtons(""labelOXPd"");
//                            for (i=0; i < buttons.length; i++)
//                            {
//                                var button = buttons[i];
//                                if (button.innerHTML.toLowerCase().indexOf(pText.toLowerCase()) > -1 )
//                                {
//                                    return button.parentNode;
//                                    break;
//                                }
//                            }
//                        }
                        
//                        var button = getButtonByText(""{buttonText}"");                        
//                        if (typeof button.onmousedown == ""function"") {
//                            button.onmousedown.apply(button);
//                        }
//                    ";

//                    // Expecting HTML similar to:
//                    //<table align="right" style="margin-left:5px; margin-right:5px;">
//                    //  <tbody><tr>
//                    //            <td>
//                    //      <div class="button" onmousedown="try{window.OXPd.beep()}catch(err){};this.enabled=false;ProcessMetadataAction('BACK_BUTTON');">
//                    //        Back
//                    //      </div>
//                    //    </td>
//                    //            <td>
//                    //      <div class="button" onmousedown="try{window.OXPd.beep()}catch(err){};this.enabled=false;ProcessMetadataAction('NEXT_BUTTON');">
//                    //        Scan
//                    //      </div>
//                    //    </td>
//                    //          </tr>
//                    //</tbody></table>

//                    var javaPressButton = @"
//                        function getButtons(pClassName){var buttons = document.getElementsByClassName(pClassName); return buttons;}
//                        function getButtonByInnerText(pText){
//                            var buttons = getButtons(""button"");
//                            for (i=0; i < buttons.length; i++)
//                            {
//                                var button = buttons[i];
//                                if (button.innerHTML.toLowerCase().indexOf(pText.toLowerCase()) > -1 )
//                                {
//                                    return button;
//                                    break;
//                                }
//                            }
//                        }
                        
//                        var button = getButtonByInnerText(""{buttonText}"");                        
//                        if (typeof button.onmousedown == ""function"") {
//                            button.onmousedown.apply(button);
//                        }
//                    ";

//                    try
//                    {
//                        var buttonCount = engine.ExecuteJavaScript(javaButtonCount);
//                        var java = ReplaceParams(javaPressWorkflowButton, "{buttonText}","Send to email");
//                        var foo = engine.ExecuteJavaScript(java);
//                        if (controller.WaitForHtmlString(">Media size<"))
//                        {
//                            java = ReplaceParams(javaPressButton, "{buttonText}", "Scan");
//                            var bar = engine.ExecuteJavaScript(java);
//                            if (!Wait.ForEquals(cp.CurrentForm, "JobPromptDialog", TimeSpan.FromSeconds(45)))
//                            {
//                                cp.Press("m_CancelButton");
//                                if (controller.WaitForHtmlString("Scanned pages"))
//                                { 
//                                    if (controller.LastHtmlContains("Success"))
//                                    {
//                                        TraceFactory.Logger.Info("Successful activity");
//                                    }
//                                    else
//                                    {
//                                        TraceFactory.Logger.Error("Error completing activity: " + controller.LastHtml);
//                                    }
//                                }
//                            }

//                        }
//                    }
//                    catch(Exception)
//                    {
//                        var bar = string.Empty;
//                    }
//                }
//            }
//            catch (Exception)
//            {
//            }
//            finally
//            {
//                device.ControlPanel.PressKeyWait(JediHardKey.Reset, "HomeScreenForm");
//            }
        }

        private string ReplaceParams(string orig, string paramCode, string replaceValue)
        {
            return orig.Replace(paramCode, replaceValue);
        }


        //private void PullPrint(UserCredential user, string ip)
        //{
        //    var controller = new SafeComDeviceAutomationController(user, ip);
        //    try
        //    {
        //        controller.Launch();
        //        var initialJobCount = controller.GetNumberOfAvailablePrintJobs();

        //        if (initialJobCount > 0)
        //        {
        //            //controller.PullSinglePrintJob();
        //            TimedDelay.Wait(TimeSpan.FromSeconds(5));
        //            var finalJobCount = controller.GetNumberOfAvailablePrintJobs();
        //            if (finalJobCount == initialJobCount)
        //            {
        //                TraceFactory.Logger.Error("Validation of job counts failed");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        controller.Finish();
        //    }
        //}

        //private void HpacPullPrint(UserCredential user, string ip)
        //{
        //    var controller = new HpacDeviceAutomationController(user, ip, "admin");
        //    try
        //    {
        //        controller.Launch();
        //        var initialJobCount = controller.GetNumberOfAvailablePrintJobs();

        //        if (initialJobCount > 0)
        //        {
        //            //controller.PullSinglePrintJob();
        //            TimedDelay.Wait(TimeSpan.FromSeconds(5));
        //            var finalJobCount = controller.GetNumberOfAvailablePrintJobs();
        //            if (finalJobCount == initialJobCount)
        //            {
        //                TraceFactory.Logger.Error("Validation of job counts failed");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        controller.SignOut();
        //    }
        //}

        private void WaitForHtmlString(JavaScriptEngine engine, string validationRegexPattern)
        {
            // Wait for up to 2 minutes for the job to finish
            DateTime end = DateTime.Now + TimeSpan.FromMinutes(2);
            while (DateTime.Now < end)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                try
                {
                    var html = string.Empty;
                    if (Regex.IsMatch(html, validationRegexPattern, RegexOptions.IgnoreCase))
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
