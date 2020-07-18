namespace HP.ScalableTest.FileAnalysis
{
    /// <summary>
    /// The result of a file validation, consisting of a pass/fail indication and a message.
    /// </summary>
    public sealed class FileValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether the file passed validation.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets a message indicating the reason for the validation result.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileValidationResult" /> class.
        /// </summary>
        /// <param name="success">A value indicating whether the file passed validation.</param>
        /// <param name="message">A message indicating the reason for the validation result.</param>
        private FileValidationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Gets a <see cref="FileValidationResult" /> representing a successful validation.
        /// </summary>
        internal static FileValidationResult Pass { get; } = new FileValidationResult(true, null);

        /// <summary>
        /// Gets a <see cref="FileValidationResult" /> representing a failed validation.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="FileValidationResult" />.</returns>
        internal static FileValidationResult Fail(string message) => new FileValidationResult(false, message);
    }
}
