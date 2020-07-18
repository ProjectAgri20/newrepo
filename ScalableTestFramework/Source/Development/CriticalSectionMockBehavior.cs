using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Actions an <see cref="ICriticalSectionMock" /> can perform when one of its methods is called.
    /// </summary>
    public enum CriticalSectionMockBehavior
    {
        /// <summary>
        /// Run the specified action, as though the lock was acquired.
        /// </summary>
        RunAction,

        /// <summary>
        /// Throw an <see cref="AcquireLockTimeoutException" />,
        /// as though the lock could not be obtained within the specified time.
        /// </summary>
        ThrowAcquireTimeoutException,

        /// <summary>
        /// Throw a <see cref="HoldLockTimeoutException" />,
        /// as though the action did not complete within the specified time.
        /// </summary>
        ThrowHoldTimeoutException
    }
}
