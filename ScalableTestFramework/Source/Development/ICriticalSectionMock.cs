using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Interface for a class that acts as a mock for the <see cref="ICriticalSection" /> service
    /// and allows consumers to set the behavior of the service.
    /// </summary>
    public interface ICriticalSectionMock
    {
        /// <summary>
        /// Gets or sets the <see cref="CriticalSectionMockBehavior" /> used when methods on this instance are called.
        /// </summary>
        CriticalSectionMockBehavior Behavior { get; set; }
    }
}
