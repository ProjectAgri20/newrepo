using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpacScan
{
    /// <summary>
    /// Base methods for all HpacScan classes
    /// </summary>
    public interface IHpacScanApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches SafeQ with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authenticationMode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Presses the Option button
        /// </summary>
        void SelectOption();

        /// <summary>
        /// Presses the Scan button
        /// </summary>
        /// <param name="numberOfcopies">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        void ScanSimplex(int numberOfcopies, bool IsJobBuildChecked);

        /// <summary>
        /// Presses the Scan button
        /// </summary>
        /// <param name="numberOfcopies">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        void ScanDuplex(int numberOfcopies, bool IsJobBuildChecked);

        /// <summary>
        /// Checks to see if the processing of work has started.
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns>bool</returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished procesing the current work.
        /// </summary>
        /// <returns></returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Checkes error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

        /// <summary>
        /// Sets the paperSupplyType.
        /// </summary>
        /// <param name="paperSupplyType">The number copies.</param>
        void SetPaperSupply(PaperSupplyType paperSupplyType);

        /// <summary>
        /// Sets the colorModeType.
        /// </summary>
        /// <param name="colorModeType">The number copies.</param>
        void SetColorMode(ColorModeType colorModeType);

        /// <summary>
        /// Sets the qualityType.
        /// </summary>
        /// <param name="qualityType">The number copies.</param>
        void SetQuality(QualityType qualityType);

        /// <summary>
        /// Sets job build.
        /// </summary>
        void SetJobBuild();
    }
}
