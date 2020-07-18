using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan
{
    /// <summary>
    /// UdocxScanApp Base class.
    /// </summary>
    public abstract class UdocxScanAppBase : DeviceWorkflowLogSource, IUdocxScanApp
    {
        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        /// <summary>
        /// Initializes a new instance of the <see cref="UdocxScanAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        protected UdocxScanAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, UdocxScanResource.UdocxScanJavaScript);
        }

        /// <summary>
        /// Launches The UdocxScan solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches Udocx Scan App in Udocx
        /// </summary>
        /// <param name="destination">The authenticator.</param>
        public abstract void SelectApp(string destination);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public abstract bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        public abstract void Scan(string emailAddress);

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        public abstract void Scan();

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        public void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);
        }

        /// <summary>
        /// Wait specific value for available
        /// </summary>
        /// <param name="value">Value for waiting</param>
        /// <param name="time">Waiting time</param>
        /// <returns></returns>
        public bool WaitObjectForAvailable(string value, TimeSpan time)
        {
            return _engine.WaitForHtmlContains(value, time);
        }

        /// <summary>
        /// Press Element by Text
        /// </summary>
        public void PressElementByText(string tag, string text)
        {            
            string elementWithText = $"getElementByText('{tag}','{text}')";
            BoundingBox boundingBox = _engine.GetElementBoundingArea(elementWithText);
            _engine.PressElementByBoundingArea(boundingBox);
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        public void SetCopyCount(int numCopies)
        {
            throw new NotImplementedException("SetCopyCount is not yet implemented in this version of UdocxScanApp.");
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        public void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
