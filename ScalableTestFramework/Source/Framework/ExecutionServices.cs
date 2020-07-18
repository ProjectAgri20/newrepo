using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides access to framework services available to an <see cref="IPluginExecutionEngine" />.
    /// </summary>
    public static class ExecutionServices
    {
        private static ICriticalSection _criticalSection;
        private static IDataLogger _dataLogger;
        private static IFileRepository _fileRepository;
        private static ISessionRuntime _sessionRuntime;
        private static ISystemTrace _systemTrace;

        /// <summary>
        /// Provides a mechanism for executing code sections serially (or with limited concurrency) across multiple processes.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The execution service is not available in the current context.</exception>
        public static ICriticalSection CriticalSection
        {
            get => _criticalSection ?? throw new FrameworkServiceUnavailableException("Execution service Critical Section is not available in the current context.");
            internal set => _criticalSection = value;
        }

        /// <summary>
        /// Provides methods for submitting and updating <see cref="ActivityDataLog" /> records.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The execution service is not available in the current context.</exception>
        public static IDataLogger DataLogger
        {
            get => _dataLogger ?? throw new FrameworkServiceUnavailableException("Execution service Data Logger is not available in the current context.");
            internal set => _dataLogger = value;
        }

        /// <summary>
        /// Manages local copies of files downloaded from a central repository.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The execution service is not available in the current context.</exception>
        public static IFileRepository FileRepository
        {
            get => _fileRepository ?? throw new FrameworkServiceUnavailableException("Execution service File Repository is not available in the current context.");
            internal set => _fileRepository = value;
        }

        /// <summary>
        /// Provides access to runtime components available when executing a test session.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The execution service is not available in the current context.</exception>
        public static ISessionRuntime SessionRuntime
        {
            get => _sessionRuntime ?? throw new FrameworkServiceUnavailableException("Execution service Session Runtime is not available in the current context.");
            internal set => _sessionRuntime = value;
        }

        /// <summary>
        /// Provides trace logging capability for debugging purposes.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The execution service is not available in the current context.</exception>
        public static ISystemTrace SystemTrace
        {
            get => _systemTrace ?? throw new FrameworkServiceUnavailableException("Execution service System Trace is not available in the current context.");
            internal set => _systemTrace = value;
        }
    }
}
