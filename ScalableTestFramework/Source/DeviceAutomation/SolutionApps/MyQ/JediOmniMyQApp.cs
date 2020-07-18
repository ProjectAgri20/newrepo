using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ
{
    /// <summary>
    /// JediOmniMyQApp runs MyQ automation of the Control Panel
    /// </summary>
    public class JediOmniMyQApp : MyQAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Keyboard Id
        /// </summary>
        protected const string KeyboardId = "#hpid-keyboard";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniMyQApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniMyQApp(JediOmniDevice device) : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _masthead = new JediOmniMasthead(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Launches MyQ with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _controlPanel.SignalUserActivity();
            _launchHelper.WorkflowLogger = WorkflowLogger;

            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }

                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
            }
            else // AuthenticationMode.Lazy
            {
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
            _controlPanel.SignalUserActivity();
            _controlPanel.WaitForAvailable("#hpid-button-oxpd-home", TimeSpan.FromSeconds(5));

           RecordEvent(DeviceWorkflowMarker.AppShown);
        }


        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            _controlPanel.SignalUserActivity();
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
        }

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        public override void PrintAll()
        {
            PressButton("mqC3");
        }

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public override void Print()
        {
            PressButton("mqC10");
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public override void Delete()
        {
            //Press delete button
            PressButton("mqC7");

            //Press OK
            if(_controlPanel.WaitForState("#mq-dialog", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(3)))
            {
                PressButtonByClassIndex("dialog-btn", 1);
            }
        }

        /// <summary>
        /// Press the PullPrinting
        /// </summary>
        public override void PressPullPrinting()
        {
            PressButton("mqC4");
        }

        /// <summary>
        /// Press back to go to main page
        /// </summary>
        public override void GotoMainPage()
        {
            if(_controlPanel.WaitForState("#tab-bar", OmniElementState.Useable, TimeSpan.FromSeconds(3)))
            {
                PressButtonByClassIndex("back-btn lg", 0);
            }
        }

        /// <summary>
        /// Selects first documents.
        /// </summary>
        public override void SelectFirstDocument()
        {
            PressButton("mqC11");
        }

        /// <summary>
        /// Selects all documents.
        /// </summary>   
        public override void SelectAllDocuments()
        {
            PressButton("mqC2");
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>Document count</returns>
        public override int GetDocumentCount()
        {
            string text = ExecuteFunction("getDocumentCount");
            string count = "";

            foreach (var word in text)
            {
                if (!word.Equals('\"'))
                {
                    count = count + word;
                }
            }
            return int.Parse(count);
        }

        /// <summary>
        /// Press Easy Scan Folder
        /// </summary>
        public override void PressEasyScanFolder()
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            PressButton("mqC6");
        }

        /// <summary>
        /// Press Easy Scan Email
        /// </summary>
        public override void PressEasyScanEmail()
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            PressButton("mqC5");
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns>bool - true if error statement exists</returns>
        public override bool BannerErrorState()
        {
            _controlPanel.SignalUserActivity();
            string bannerText = _controlPanel.GetValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property);
            return bannerText.Contains("Runtime Error");
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts)
        {
            _controlPanel.SignalUserActivity();
            bool printing = true;
            if (_controlPanel.GetScreenSize().Width > 480)
            {
                printing = _masthead.WaitForActiveJobsButtonState(true, ts);
            }
            return printing;
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);

    }
}
