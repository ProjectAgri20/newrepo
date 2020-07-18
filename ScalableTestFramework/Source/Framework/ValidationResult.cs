namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// The result of a call to validate status or user input.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets a <see cref="ValidationResult" /> representing successful validation with no message.
        /// </summary>
        public static ValidationResult Success { get; } = new ValidationResult(true);

        /// <summary>
        /// Gets a value indicating whether validation succeeded.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Gets a message indicating any problems that happend during validation.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult" /> class.
        /// </summary>
        /// <param name="success">Indicates whether validation was successful.</param>
        public ValidationResult(bool success)
        {
            Succeeded = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult" /> class.
        /// </summary>
        /// <param name="success">Indicates whether validation was successful.</param>
        /// <param name="message">A message indicating the result of validation.</param>
        public ValidationResult(bool success, string message)
            : this(success)
        {
            Message = message;
        }
    }
}
