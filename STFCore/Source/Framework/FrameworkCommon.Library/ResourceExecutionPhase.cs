
namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Represents the phases of Admin Worker activity execution.
    /// </summary>
    /// <remarks>
    /// Each metadata activity associated with an AdminWorker
    /// is assigned an integer execution order.  Certain ranges of integers are associated
    /// with a particular execution phase.  This enumeration defines the first integer
    /// that is associated with the range for each execution phase.
    /// </remarks>
    public enum ResourceExecutionPhase
    {
        /// <summary>
        /// Activity executes during the main scenario run
        /// </summary>
        Main = 0,

        /// <summary>
        /// Activity executes during the preprocess transition
        /// </summary>
        Setup = 1000,

        /// <summary>
        /// Activity executes during the postprocess step
        /// </summary>
        Teardown = 2000
    }

    /// <summary>
    /// Helper class to provide extension methods for <see cref="ResourceExecutionPhase"/>.
    /// </summary>
    public static class ResourceExecutionPhaseHelper
    {
        /// <summary>
        /// Determines whether the specified execution order falls within the specified phase.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <param name="executionOrder">The execution order.</param>
        /// <returns>
        ///   <c>true</c> if the specified execution order falls within the specified phase; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this ResourceExecutionPhase phase, int executionOrder)
        {
            int phaseMin = (int)phase;
            int phaseMax = phaseMin + 999;
            return (executionOrder >= phaseMin && executionOrder < phaseMax);
        }
    }
}
