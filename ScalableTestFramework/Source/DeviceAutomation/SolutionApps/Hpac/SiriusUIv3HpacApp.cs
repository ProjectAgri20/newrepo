using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;


namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpac
{
    /// <summary>
    /// Runs HPAC automation on the Control Panel of a SiriusUIv3 device.
    /// </summary>
    public class SiriusUIv3HpacApp : DeviceWorkflowLogSource, IHpacApp
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly TimeSpan _mediumDelay = TimeSpan.FromSeconds(20);
        private readonly TimeSpan _pause = TimeSpan.FromSeconds(2);

        private const string HpacAppLabel = "vw_sips_apps_state";

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3HpacApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3HpacApp(SiriusUIv3Device device)
        {
            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Launches HPAC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">Timed out waiting for Sign In screen.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                PressSignInButton();
                if (_controlPanel.WaitForScreenId("flow_auth::st_auth_alternate_login_selection"))
                {
                    authenticator.Authenticate();
                    PressHpacSolutionButton(HpacAppBase.SolutionButtonTitle);
                }
                else
                {
                    throw new DeviceWorkflowException("Timed out waiting for Sign In screen.");
                }
            }
            else // AuthenticationMode.Lazy
            {
                PressHpacSolutionButton(HpacAppBase.SolutionButtonTitle);
                Authenticate(authenticator);
            }
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public bool SignInReleaseAll(IAuthenticator authenticator)
        {
            PressSignInButton();
            authenticator.Authenticate();
            PrintAll();
            return true;
        }

        /// <summary>
        /// Selects all documents and presses the Print button.
        /// </summary>
        public void PrintAll()
        {
            SelectAllDocuments(string.Empty);
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
            //Press Print Button
            _controlPanel.Press("fb_footerRight");
            Thread.Sleep(_pause);

        }

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public void PrintKeep()
        {
            //Press Print Button
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
            //Press delete button
            _controlPanel.Press("fb_footerCenter");
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
        /// <param name="checkboxId">The checkbox identifier.</param>
        /// <param name="onchangeValue">The onchange value.</param>
        public void SelectFirstDocument(string checkboxId, string onchangeValue)
        {

            var firstDocumentWidget = _controlPanel.WaitForWidget(checkboxId, _mediumDelay);
            if(firstDocumentWidget != null)
                _controlPanel.PerformAction(WidgetAction.Check, checkboxId);

            Thread.Sleep(_pause);
        }

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        /// <param name="documentId"></param>
        public void SelectFirstDocument(string documentId)
        {
            SelectFirstDocument(documentId, string.Empty);
        }

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        /// <param name="onchangeValue">The onchange value.</param>
        public void SelectAllDocuments(string onchangeValue)
        {
            _controlPanel.Press("fb_footerLeft");
        }

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        /// <param name="documentIds"></param>
        public void SelectAllDocuments(IEnumerable<string> documentIds)
        {
            SelectAllDocuments(string.Empty);
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {
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
            int currentIndex = -1;

            ScreenInfo screenInfo = ScrollToTop();
            while (true)
            {
                try
                {
                    IEnumerable<Widget> widgets = screenInfo.Widgets.Where(w => w.Values.Keys.Contains("index"));
                    foreach (Widget widget in widgets)
                    {
                        currentIndex = int.Parse(widget.Values["index"]);
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
        /// Gets the checkbox onchange value.
        /// </summary>
        /// <returns>HpacInput.</returns>
        public HpacInput GetCheckboxOnchangeValue()
        {
            // This is not used by SiriusUIv3.
            return new HpacInput();
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns><c>true</c> if the device is in an error state, <c>false</c> otherwise.</returns>
        public bool BannerErrorState()
        {
            return false;
        }

        private void PressSignInButton()
        {
            var signInWidget = _controlPanel.WaitForWidgetByValue("Sign In", _mediumDelay);
            Thread.Sleep(_pause);
            if (signInWidget != null)
                _controlPanel.PressByValue("Sign In");

        }

        private ScreenInfo ScrollToTop()
        {
            ScreenInfo screenInfo = null;

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
            ScreenInfo screenInfo = null;
            Widget lastScreenItem = null;

            while (true)
            {
                screenInfo = _controlPanel.GetScreenInfo();

                if (!screenInfo.Widgets.Any(w => w.HasAction(WidgetAction.ScrollNext)))
                {
                    // No ScrollNext, we're at the bottom, or there's no docs
                    break;
                }
                Widget bottomItem = screenInfo.Widgets.Where(w => w.Values.Keys.Contains("index")).LastOrDefault();

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

        /// <summary>
        /// Authenticates using the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        private void Authenticate(IAuthenticator authenticator)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForScreenLabel(HpacAppLabel, TimeSpan.FromSeconds(40));
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private void PressHpacSolutionButton(string waitForm)
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
                //Press HP AC
                _controlPanel.PressByValue(waitForm);
                Thread.Sleep(_pause);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to navigate to Home Screen.");
            }
        }

        /// <summary>
        /// Determines whether the HPAC solution is the new Omni-fication [is new version].
        /// </summary>
        /// <returns><c>true</c> if [is new version]; otherwise, <c>false</c>.</returns>
        public bool IsNewVersion()
        {
            return false;
        }

        /// <summary>
        /// Checks for the absence of the "Printing" screen.
        /// </summary>
        /// <returns><c>true</c> if "Printing" screen is not detected, <c>false</c> otherwise.</returns>
        public bool FinishedProcessingWork()
        {
            bool isPrinting = true; //Assumes that the "Printing" dialog was detected before this method was called.
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(30));

            while (isPrinting && DateTime.Now < expiration)
            {
                Thread.Sleep(500);
                isPrinting = IsPrinting(JobStatusCheckBy.ControlPanel);
            }

            return !isPrinting;
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="checkInterval">Time to continue checking</param>
        /// <returns><c>true</c> if "Printing" screen detected, <c>false</c> otherwise.</returns>
        public bool StartedProcessingWork(TimeSpan checkInterval)
        {
            bool startProcessing = false;

            if (checkInterval.Equals((TimeSpan)default(TimeSpan)))
            {
                checkInterval = TimeSpan.FromSeconds(6);
            }
            DateTime expiration = DateTime.Now.Add(checkInterval);

            while (startProcessing == false && DateTime.Now < expiration)
            {
                Thread.Sleep(500);
                startProcessing = IsPrinting(JobStatusCheckBy.ControlPanel);
            }

            return startProcessing;
        }

    }
}
