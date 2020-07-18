using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// A <see cref="System.EventArgs"/> child containing information about an exception message.
    /// </summary>
    /// <example>
    /// This class simply adds the ability to add more context around an error that occurs in code.
    /// <code>
    /// try
    /// {
    ///     Foo();
    /// }
    /// catch (TimeoutException ex)
    /// {
    ///     throw new ExceptionDetailEventArgs(ex.Message, "This provides a place to add more detail around the error");
    /// }
    /// </code>
    /// </example>
    public class ExceptionDetailEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception detail.
        /// </summary>
        /// <value>
        /// The exception detail.
        /// </value>
        public string Detail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDetailEventArgs"/> class.
        /// </summary>
        /// <param name="message"><see cref="System.String"/> value containing the message.</param>
        /// <param name="detail"><see cref="System.String"/> containing additional detail about the exception.</param>
        public ExceptionDetailEventArgs(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDetailEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> used to populate this class.</param>
        public ExceptionDetailEventArgs(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            Message = ex.Message;
            Detail = ex.ToString();
        }
    }
}
