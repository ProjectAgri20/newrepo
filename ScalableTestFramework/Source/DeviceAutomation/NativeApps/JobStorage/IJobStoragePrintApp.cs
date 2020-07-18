using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Interface for an embedded Save To Job Storage application.
    /// </summary>
    public interface IJobStoragePrintApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Print application on the device.
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches the Print application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Selects the specified folder in the list when left empty it treats that as “Untitled”(Default folder).
        /// </summary>
        /// <param name="folderName">Name of the folder where file exists.</param>
        void SelectFolder(string folderName);

        /// <summary>
        /// Selects first Job.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="numberCopies">Number of copies to Print.</param>
        /// <param name="folderName">Folder containing the stored job.</param>
        void SelectFirstJob(string pin, int numberCopies, string folderName);

        /// <summary>
        /// Selects all Jobs with Password in the list.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="folderName">Folder containing the stored job.</param>     
        bool SelectAllJobs(string pin, string folderName);

        /// <summary>
        /// Selects the all Jobs without Password in the list.
        /// </summary>            
        bool SelectAllJobs();

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        void ExecutePrintJob();

        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IJobStorageJobOptions Options { get; }

        /// <summary>
        /// Deletes the Printed job and runs it to completion" />.
        /// </summary>
        void DeletePrintedJob();
    }
}
