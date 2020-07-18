using System;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    class JediWindjammerJobStoragePrintApp : DeviceWorkflowLogSource, IJobStoragePrintApp
    {
        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IJobStorageJobOptions Options
        {
            get
            {
                throw new NotImplementedException("Job option is not implemented on Windjammer devices.");
            }
        }

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper
        {
            get
            {
                throw new NotImplementedException("Pacekeeper is not implemented on Windjammer devices.");
            }

            set
            {
                throw new NotImplementedException("Pacekeeper is not implemented on Windjammer devices.");
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        public void ExecutePrintJob()
        {
            throw new NotImplementedException("ExecutePrintJob is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Deletes the printed job and runs it to completion" />.
        /// </summary>
        public void DeletePrintedJob()
        {
            throw new NotImplementedException("DeletePrintedJob is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Launches the Print application on the device.
        /// </summary>
        public void Launch()
        {
            throw new NotImplementedException("Launch is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Launches the Print application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            throw new NotImplementedException("Launch is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Selects the all Jobs without Password in the list.
        /// </summary> 
        public bool SelectAllJobs()
        {
            throw new NotImplementedException("SelectAllJobs is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Selects all Jobs with Password in the list.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="folderName">Folder containing the stored job.</param>>
        public bool SelectAllJobs(string pin, string folderName)
        {
            throw new NotImplementedException("SelectAllJobs with pin is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Selects first Job.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="numberCopies">Number of copies to Print.</param>
        /// <param name="folderName">Folder containing the stored job.</param>
        public void SelectFirstJob(string pin, int numberCopies, string folderName)
        {
            throw new NotImplementedException("SelectFirstJob with pin is not implemented on Windjammer devices.");
        }

        /// <summary>
        /// Selects the specified folder in the list when left empty it treats that as “Untitled”(Default folder).
        /// </summary>
        /// <param name="folderName">Name of the folder where file exists.</param>
        public void SelectFolder(string folderName)
        {
            throw new NotImplementedException("SelectFolder is not implemented on Windjammer devices.");
        }
    }
}
