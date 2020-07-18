
namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Describes startup transition states of the dispatcher.
    /// </summary>
    public enum SessionStartupTransition
    {
        /// <summary>
        /// No state provided
        /// </summary>
        None,

        /// <summary>
        /// Ready to start a new session
        /// </summary>
        ReadyToStart,

        /// <summary>
        /// Ready to stage session
        /// </summary>
        ReadyToStage,

        /// <summary>
        /// Dispatcher is ready to validate
        /// </summary>
        ReadyToValidate,

        /// <summary>
        /// Dispatcher failed validation and is ready to revalidate
        /// </summary>
        ReadyToRevalidate,

        /// <summary>
        /// Ready to power up
        /// </summary>
        ReadyToPowerUp,

        /// <summary>
        /// Ready to run
        /// </summary>
        ReadyToRun,

        /// <summary>
        /// The session is running and can be shutdown
        /// </summary>
        StartupComplete,
    }
}
