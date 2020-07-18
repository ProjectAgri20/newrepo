using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using System.ComponentModel;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpacScan
{
    /// <summary>
    /// HpacScanAppBase class.
    /// </summary>
    public abstract class HpacScanAppBase : DeviceWorkflowLogSource, IHpacScanApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacScanAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        protected HpacScanAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, HpacScanResource.HpacScanJavaScript);
        }
        /// <summary>
        /// Launches Hpac scan with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authenticationMode</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Presses the Option button
        /// </summary>
        public abstract void SelectOption();

        /// <summary>
        /// Presses the Scan button for simplex job
        /// </summary>
        /// <param name="numberOfcopies">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        public abstract void ScanSimplex(int numberOfcopies, bool IsJobBuildChecked);

        /// <summary>
        /// Presses the Scan button for duplex job
        /// </summary>
        /// <param name="numberOfcopies">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        public abstract void ScanDuplex(int numberOfcopies, bool IsJobBuildChecked);

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        public void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);
        }

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="classId">The class ID</param>
        /// <param name="idx">The index</param>

        public void PressButtonByClass(string classId, int idx)
        {
            _engine.PressElementByClassIndex(classId, idx);
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
        /// Checks to see if the processing of work has started.
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns>bool</returns>
        public abstract bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished procesing the current work.
        /// </summary>
        /// <returns></returns>
        public abstract bool FinishedProcessingWork();

        /// <summary>
        /// Checkes error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public abstract bool BannerErrorState();

        /// <summary>
        /// Sets the paperSupplyType.
        /// </summary>
        /// <param name="paperSupplyType">The number copies.</param>
        public abstract void SetPaperSupply(PaperSupplyType paperSupplyType);

        /// <summary>
        /// Sets the colorModeType.
        /// </summary>
        /// <param name="colorModeType">The number copies.</param>
        public abstract void SetColorMode(ColorModeType colorModeType);

        /// <summary>
        /// Sets the qualityType.
        /// </summary>
        /// <param name="qualityType">The number copies.</param>
        public abstract void SetQuality(QualityType qualityType);

        /// <summary>
        /// Sets the SetJobBuild.
        /// </summary>
        public abstract void SetJobBuild();
    }
}
