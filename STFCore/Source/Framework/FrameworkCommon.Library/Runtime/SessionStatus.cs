namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Enumeration describing different states for a session.
    /// </summary>
    public enum SessionStatus
    {
        /// <summary>
        /// Starting
        /// </summary>
        Starting,

        /// <summary>
        /// Running
        /// </summary>
        Running,

        /// <summary>
        /// Finished
        /// </summary>
        Complete,

        /// <summary>
        /// Aborted
        /// </summary>
        Aborted,

        /// <summary>
        /// Error
        /// </summary>
        Error
    }

    /// <summary>
    /// Various states of the session shutting down.
    /// </summary>
    public enum MachineShutdownState
    {
        /// <summary>
        /// This should never be used.
        /// </summary>
        Unknown,

        /// <summary>
        /// Scenario has not been shut down (it's still running, or the dispatcher crashed while it was still running)
        /// </summary>
        NotStarted,

        /// <summary>
        /// An attempt was made to shut down, but the dispatcher failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The machine or session is in the middle of shutting down.
        /// </summary>
        Pending,

        /// <summary>
        /// Shutdown succeeded.
        /// </summary>
        Complete,

        /// <summary>
        /// Was in the Shutdown state, but the scenario was reset.
        /// </summary>
        ManualReset,

        /// <summary>
        /// The shutdown was partial, meaning something went wrong during the process.
        /// </summary>
        Partial,
    }

    /// <summary>
    /// Various states for the session archive status.
    /// </summary>
    public enum SessionArchiveState
    {
        /// <summary>
        /// The session is has not yet reached its expiration date.
        /// </summary>
        Active,

        /// <summary>
        /// The session has exceeded its expiration date and is ready to be placed in long term storage.
        /// </summary>
        Archive,

        /// <summary>
        /// The session is ready to be deleted from the DataLog database.
        /// </summary>
        Delete
    }
}
