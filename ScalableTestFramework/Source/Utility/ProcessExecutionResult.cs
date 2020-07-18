namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Represents the result of a remote process execution invoked by <see cref="ProcessUtil" />.
    /// </summary>
    public sealed class ProcessExecutionResult
    {
        /// <summary>
        /// Gets a value indicating whether the process executed successfully.
        /// </summary>
        public bool SuccessfulExit { get; }

        /// <summary>
        /// Gets the exit code of the executed process.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the textual output of the executed process.
        /// </summary>
        public string StandardOutput { get; }

        /// <summary>
        /// Gets the error output of the executed process.
        /// </summary>
        public string StandardError { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExecutionResult" /> class.
        /// </summary>
        /// <param name="successfulExit">if set to <c>true</c> the process exited successfully.</param>
        /// <param name="exitCode">The exit code.</param>
        /// <param name="output">The textual output.</param>
        /// <param name="error">The error output.</param>
        public ProcessExecutionResult(bool successfulExit, int exitCode, string output, string error)
        {
            SuccessfulExit = successfulExit;
            ExitCode = exitCode;
            StandardOutput = output;
            StandardError = error;
        }
    }
}
