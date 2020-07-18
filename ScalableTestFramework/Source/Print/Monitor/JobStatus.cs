using System;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// States that can be applied to a print job.
    /// </summary>
    /// <remarks>
    /// Descriptions pulled from MSDN documentation for JOB_INFO_2 structure.
    /// </remarks>
    [Flags]
    public enum JobStatus
    {
        /// <summary>
        /// No state specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Job is paused.
        /// </summary>
        Paused = 0x00000001,

        /// <summary>
        /// An error is associated with the job.
        /// </summary>
        Error = 0x00000002,

        /// <summary>
        /// Job is being deleted.
        /// </summary>
        Deleting = 0x00000004,

        /// <summary>
        /// Job is spooling.
        /// </summary>
        Spooling = 0x00000008,

        /// <summary>
        /// Job is printing.
        /// </summary>
        Printing = 0x00000010,

        /// <summary>
        /// Printer is offline.
        /// </summary>
        Offline = 0x00000020,

        /// <summary>
        /// Printer is out of paper.
        /// </summary>
        PaperOut = 0x00000040,

        /// <summary>
        /// Job has printed.
        /// </summary>
        Printed = 0x00000080,

        /// <summary>
        /// Job has been deleted.
        /// </summary>
        Deleted = 0x00000100,

        /// <summary>
        /// The driver cannot print the job.
        /// </summary>
        BlockedDevQ = 0x00000200,

        /// <summary>
        /// Printer has an error that requires the user to do something.
        /// </summary>
        UserIntervention = 0x00000400,

        /// <summary>
        /// Job has been restarted.
        /// </summary>
        Restart = 0x00000800,

        /// <summary>
        /// The job is sent to the printer, but may not be printed yet. See Remarks for more information.
        /// </summary>
        Complete = 0x00001000,

        /// <summary>
        /// The job has been retained in the print queue following printing.
        /// </summary>
        Retained = 0x00002000,

        /// <summary>
        /// Job is rendering locally.
        /// </summary>
        RenderingLocally = 0x00004000
    }
}
