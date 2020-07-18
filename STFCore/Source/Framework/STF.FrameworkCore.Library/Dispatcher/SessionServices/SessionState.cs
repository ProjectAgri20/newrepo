using System.ComponentModel;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Describes the various states of a session.
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// Unavailable
        /// </summary>
        [Description("Unavailable")]
        Unavailable,

        /// <summary>
        /// Unavailable
        /// </summary>
        [Description("Available")]
        Available,

        /// <summary>
        /// Unavailable
        /// </summary>
        [Description("Canceled")]
        Canceled,

        /// <summary>
        /// Reserving assets for new session
        /// </summary>
        [Description("Reserving")]
        Reserving,

        /// <summary>
        /// Building the session map
        /// </summary>
        [Description("Staging")]
        Staging,

        /// <summary>
        /// Dispatcher is validating resources
        /// </summary>
        [Description("Validating")]
        Validating,

        /// <summary>
        /// Powering up VMs
        /// </summary>
        [Description("Powering Up")]
        PowerUp,

        /// <summary>
        /// Running
        /// </summary>
        [Description("Running")]
        Running,

        /// <summary>
        /// Resetting
        /// </summary>
        [Description("Resetting")]
        Resetting,

        /// <summary>
        /// Run complete
        /// </summary>
        [Description("Complete")]
        RunComplete,

        /// <summary>
        /// Pausing
        /// </summary>
        [Description("Pausing")]
        Pausing,

        /// <summary>
        /// Pause complete
        /// </summary>
        [Description("Paused")]
        PauseComplete,

        /// <summary>
        /// Shutting down
        /// </summary>
        [Description("Shutting Down")]
        ShuttingDown,

        /// <summary>
        /// Shutdown complete
        /// </summary>
        [Description("Shut Down")]
        ShutdownComplete,

        /// <summary>
        /// Error
        /// </summary>
        [Description("Error")]
        Error,

        /// <summary>
        /// No state
        /// </summary>
        [Description("None")]
        None,
    }
}


