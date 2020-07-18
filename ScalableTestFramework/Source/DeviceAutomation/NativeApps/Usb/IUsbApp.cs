using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.USB
{
    /// <summary>
    /// Interface for an embedded Save To Usb application.
    /// </summary>
    public interface IUsbApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Scan application on the device.
        /// </summary>
        void LaunchScanToUsb();

        /// <summary>
        /// Launches the Scan to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void LaunchScanToUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches the scan to usb by a given quick set.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quickset</param>
        void LaunchScanToUsbByQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName);

        /// <summary>
        /// Launches the Print application on the device.
        /// </summary>
        void LaunchPrintFromUsb();

        /// <summary>
        /// Launches the Print to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void LaunchPrintFromUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Adds the specified name for the job
        /// </summary>
        /// <param name="jobname">The job name.</param>
        void AddJobName(string jobname);

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        void SelectQuickset(string quicksetName);

        /// <summary>
        /// Select the Usb device.
        /// </summary>
        /// <param name="usbName">The file name.</param>
        void SelectUsbDevice(string usbName);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        bool ExecuteScanJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        bool ExecutePrintJob();

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        void SelectUsbPrint(string usbName);

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        void SelectFile();

        /// <summary>
        /// Gets the <see cref="IUsbJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IUsbJobOptions Options { get; }
    }
}
