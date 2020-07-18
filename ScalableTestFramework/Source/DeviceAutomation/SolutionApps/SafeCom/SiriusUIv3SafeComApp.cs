using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// Runs SafeCom automation on the Control Panel of a SiriusUIv3 device.
    /// </summary>
    public class SiriusUIv3SafeComApp : DeviceWorkflowLogSource, ISafeComApp
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly TimeSpan _mediumDelay = TimeSpan.FromSeconds(20);
        private readonly TimeSpan _pause = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3SafeComApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3SafeComApp(SiriusUIv3Device device)
        {
            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Launches SafeCom with the specified authenticator using eager AuthenticationMode.
        /// SafeCom only allows AuthenticationMode.Eager for SiriusUIv3SafeComApp.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">Timed out waiting for Sign In screen.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            //There is no Lazy auth mode for SafeCom in Triptane.  It's all eager because that is the only option presented by the device.
            PressSignInButton();
            if (_controlPanel.WaitForScreenId("flow_auth::st_auth_alternate_login_selection"))
            {
                authenticator.Authenticate();
                PressSafeComSolutionButton(SafeComAppBase.SolutionButtonTitle);
            }
            else
            {
                throw new DeviceWorkflowException("Timed out waiting for Sign In screen.");
            }
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public void SignInReleaseAll(IAuthenticator authenticator)
        {
            PressSignInButton();
            authenticator.Authenticate();
            PrintAll();
        }

        /// <summary>
        /// Selects all documents and presses the Print button.
        /// </summary>
        public void PrintAll()
        {
            SelectAllDocuments();
            Thread.Sleep(_pause);

            //Press the Print Button
            _controlPanel.Press("fb_footerRight");
            Thread.Sleep(_pause);
        }

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public void PrintDelete()
        {
            _controlPanel.Press("fb_footerRight");
            Thread.Sleep(_pause);
        }

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public void PrintKeep()
        {
            _controlPanel.Press("fb_footerRight");
            Thread.Sleep(_pause);
        }

        /// <summary>
        /// There is no Refresh button on the control panel for SiriusUIv3..
        /// </summary>
        /// <exception cref="System.NotImplementedException">No 'Refresh' button exists for SiriusUIv3.</exception>
        public void Refresh()
        {
            throw new NotImplementedException("No 'Refresh' button exists for SiriusUIv3.");
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            //Press the menu button
            _controlPanel.WaitForWidget("fb_footerCenter");
            _controlPanel.Press("fb_footerCenter");
            Thread.Sleep(_pause);
            //Press delete button
            Widget deleteWidget = _controlPanel.WaitForWidgetByValue("Delete");
            _controlPanel.Press(deleteWidget);
            Thread.Sleep(_pause);
        }

        /// <summary>
        /// Determines whether the device is printing by checking for the presence of an alert with the text "Printing..."
        /// </summary>
        /// <param name="checkBy">The check by.</param>
        /// <returns><c>true</c> if the "Printing" alert is present; <c>false</c> otherwise.</returns>
        public bool IsPrinting(JobStatusCheckBy checkBy)
        {
            ScreenInfo screenInfo = _controlPanel.GetScreenInfo();

            try
            {
                Widget widget = screenInfo.Widgets.FindByValue("Printing", StringMatch.StartsWith);
                return (widget != null);
            }
            catch (ElementNotFoundException)
            {
                // "Printing" Screen not found.  Allow to return.
            }

            return false;
        }

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        public void SelectFirstDocument()
        {
            string firstDocId = GetFirstDocumentId();
            _controlPanel.WaitForWidget(firstDocId);

            _controlPanel.PerformAction(WidgetAction.Check, firstDocId);
            Thread.Sleep(200);
            if (!_controlPanel.WaitForWidget(firstDocId).HasValue("SELECTED", StringMatch.Exact))
            {
                Thread.Sleep(1000);
                _controlPanel.PerformAction(WidgetAction.Check, firstDocId);
            }
        }

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        public void SelectAllDocuments()
        {
            _controlPanel.WaitForWidget("fb_footerLeft");
            _controlPanel.Press("fb_footerLeft");
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {
            _controlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(10));
            int index = -1;
            ScreenInfo screenInfo = ScrollToBottom();

            Widget widget = screenInfo.Widgets.LastOrDefault(w => w.Values.Keys.Contains("index"));
            if (widget != null)
            {
                index = int.Parse(widget.Values["index"]);
            }

            return (index + 1);
        }

        /// <summary>
        /// Gets the first document identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetFirstDocumentId()
        {
            ScreenInfo screenInfo = ScrollToTop();
            Widget widget = screenInfo.Widgets.FirstOrDefault(w => w.Values.Keys.Contains("index") && w.Values["index"] == "0");

            if (widget != null)
            {
                return widget.Id;
            }

            // No docs
            return string.Empty;
        }

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetDocumentIds()
        {
            List<string> result = new List<string>();
            int lastIndex = -1;

            ScreenInfo screenInfo = ScrollToTop();
            while (true)
            {
                try
                {
                    IEnumerable<Widget> widgets = screenInfo.Widgets.Where(w => w.Values.Keys.Contains("index"));
                    foreach (Widget widget in widgets)
                    {
                        var currentIndex = int.Parse(widget.Values["index"]);
                        if (currentIndex > lastIndex)
                        {
                            result.Add(widget.Id);
                            //System.Diagnostics.Debug.WriteLine($"Adding: {widget.Id}" );
                            lastIndex = currentIndex;
                        }
                    }

                    if (screenInfo.Widgets.Any(w => w.HasAction(WidgetAction.ScrollNext)))
                    {
                        _controlPanel.PerformAction(WidgetAction.ScrollNext, "vertical_item_list");
                        screenInfo = _controlPanel.GetScreenInfo();
                    }
                    else
                    {
                        // We're at the bottom
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Checks to see if the processing of work has started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="checkInterval">Time to continue checking</param>
        /// <returns></returns>
        public bool StartedProcessingWork(TimeSpan checkInterval)
        {
            bool startProcessing = false;

            if (checkInterval.Equals(default(TimeSpan)))
            {
                checkInterval = TimeSpan.FromSeconds(6);
            }
            DateTime expiration = DateTime.Now.Add(checkInterval);

            while (startProcessing == false && DateTime.Now < expiration)
            {
                Thread.Sleep(250);
                startProcessing = IsPrinting(JobStatusCheckBy.ControlPanel);
            }

            return startProcessing;
        }

        /// <summary>
        /// Checks for the absence of the "Printing" screen for up to 30 seconds.
        /// </summary>
        /// <returns>bool</returns>
        public bool FinishedProcessingWork()
        {
            bool isPrinting = true; //Assumes that the "Printing" dialog was detected before this method was called.
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(30));

            while (isPrinting && DateTime.Now < expiration)
            {
                Thread.Sleep(250);
                isPrinting = IsPrinting(JobStatusCheckBy.ControlPanel);
            }

            return !isPrinting;
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        public void SetCopyCount(int numCopies)
        {
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns><c>true</c> if the device is in an error state, <c>false</c> otherwise.</returns>
        public static bool BannerErrorState()
        {
            return false;
        }

        private void PressSignInButton()
        {
            var signInWidget = _controlPanel.WaitForWidgetByValue("Sign In", TimeSpan.FromSeconds(5));
            if (signInWidget == null)
            {
                //we mostly are still signed in
                var signOutWidget = _controlPanel.WaitForWidgetByValue("Sign Out", TimeSpan.FromSeconds(5));
                if (signOutWidget != null)
                {
                    _controlPanel.Press(signOutWidget);
                    //Confirm on sign out
                    _controlPanel.WaitForScreenId("flow_auth::st_auth_signout", TimeSpan.FromSeconds(5));
                    _controlPanel.Press("mdlg_action_button");
                    Thread.Sleep(_pause);
                    signInWidget = _controlPanel.WaitForWidgetByValue("Sign In", TimeSpan.FromSeconds(5));
                }
            }

            _controlPanel.Press(signInWidget);
        }

        private ScreenInfo ScrollToTop()
        {
            ScreenInfo screenInfo;

            while (true)
            {
                screenInfo = _controlPanel.GetScreenInfo();
                if (screenInfo.Widgets.Any(w => w.HasAction(WidgetAction.ScrollPrev)))
                {
                    _controlPanel.PerformAction(WidgetAction.ScrollPrev, "vertical_item_list");
                }
                else
                {
                    // We're at the top
                    break;
                }
            }

            return screenInfo;
        }

        private ScreenInfo ScrollToBottom()
        {
            ScreenInfo screenInfo;
            Widget lastScreenItem = null;

            while (true)
            {
                screenInfo = _controlPanel.GetScreenInfo();

                if (!screenInfo.Widgets.Any(w => w.HasAction(WidgetAction.ScrollNext)))
                {
                    // No ScrollNext, we're at the bottom, or there's no docs
                    break;
                }
                Widget bottomItem = screenInfo.Widgets.LastOrDefault(w => w.Values.Keys.Contains("index"));

                // In the case where there are 3 or less items in the list, the "ScrollNext" WidgetAction is present, even though there is no scrollbar on the screen.
                // So, we have to scrollNext, then compare the Ids of the last widgets on the screen.  If they are the same, we have hit the bottom.  If not, we keep
                // scrolling.

                if (bottomItem != null && (lastScreenItem == null || lastScreenItem.Id != bottomItem.Id))
                {
                    _controlPanel.PerformAction(WidgetAction.ScrollNext, "vertical_item_list");
                    lastScreenItem = bottomItem;
                }
                else
                {
                    // We're at the bottom
                    break;
                }
            }

            return screenInfo;
        }

        private void PressSafeComSolutionButton(string waitForm)
        {
            if (_controlPanel.WaitForScreenLabel("Home", _mediumDelay))
            {
                //Scroll to Apps
                _controlPanel.ScrollToItemByValue("sfolderview_p", "Apps");
                //Press App button
                _controlPanel.PressByValue("Apps");

                _controlPanel.WaitForWidgetByValue(waitForm, _mediumDelay);

                _controlPanel.ScrollToItemByValue("sfolderview_p", waitForm);
                Thread.Sleep(_pause);

                //Press SafeCom
                _controlPanel.PressByValue(waitForm);
                Thread.Sleep(_pause);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to navigate to Home Screen.");
            }
        }

        /// <summary>
        /// Gets the document name by identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>System.String.</returns>
        public string GetDocumentNameById(string documentId)
        {
            return documentId;
        }

        /// <summary>
        /// Determines whether the SafeCom solution is the new Omni-fication [is new version].
        /// </summary>
        /// <returns><c>true</c> if [is new version]; otherwise, <c>false</c>.</returns>
        public bool IsNewVersion()
        {
            return false;
        }

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SelectFirstDocument(string documentValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects the documents in the given list
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SelectDocuments(IEnumerable<string> documentIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether this instance is an Omni device.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is Omni; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOmni()
        {
            return false;
        }
    }
}