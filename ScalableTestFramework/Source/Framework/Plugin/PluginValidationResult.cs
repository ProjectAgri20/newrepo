using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// The result of a call to <see cref="IPluginConfigurationControl.ValidateConfiguration" />.
    /// </summary>
    public sealed class PluginValidationResult
    {
        private readonly List<string> _errorMessages = new List<string>();

        /// <summary>
        /// Gets a value indicating whether validation succeeded.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Gets the error message(s) describing the issues that prevented successful validation (if any).
        /// </summary>
        public IEnumerable<string> ErrorMessages => _errorMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidationResult" /> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> validation succeeded.</param>
        public PluginValidationResult(bool success)
        {
            Succeeded = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidationResult" /> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> validation succeeded.</param>
        /// <param name="errorMessage">The error message.</param>
        public PluginValidationResult(bool success, string errorMessage)
            : this(success)
        {
            Succeeded = success;
            _errorMessages.Add(errorMessage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidationResult" /> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> validation succeeded.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessages" /> is null.</exception>
        public PluginValidationResult(bool success, IEnumerable<string> errorMessages)
            : this(success)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            _errorMessages.AddRange(errorMessages);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidationResult" /> class
        /// based on the specified <see cref="ValidationResult" />.
        /// </summary>
        /// <param name="validationResult">The <see cref="ValidationResult" /> to base this instance on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="validationResult" /> is null.</exception>
        public PluginValidationResult(ValidationResult validationResult)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException(nameof(validationResult));
            }

            Succeeded = validationResult.Succeeded;
            _errorMessages.Add(validationResult.Message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidationResult" /> class
        /// based on the specified <see cref="ValidationResult" /> list.
        /// </summary>
        /// <param name="validationResults">The list of <see cref="ValidationResult" /> to base this instance on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="validationResults" /> is null.</exception>
        public PluginValidationResult(IEnumerable<ValidationResult> validationResults)
        {
            if (validationResults == null)
            {
                throw new ArgumentNullException(nameof(validationResults));
            }

            List<ValidationResult> failed = validationResults.Where(n => !n.Succeeded).ToList();
            if (failed.Any())
            {
                Succeeded = false;
                _errorMessages.AddRange(failed.Select(n => n.Message));
            }
            else
            {
                Succeeded = true;
            }
        }
    }
}
