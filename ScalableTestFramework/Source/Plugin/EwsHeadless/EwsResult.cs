using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Result of an EWS operation.  Comprises a list of responses and, if any failed, the exception received.
    /// </summary>
    public class EwsResult
    {
        private readonly List<HttpResponse> _responses;

        /// <summary>
        /// Gets the responses.
        /// </summary>
        /// <value>The responses.</value>
        public IEnumerable<HttpResponse> Responses => _responses;

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsResult"/> class.
        /// </summary>
        /// <param name="responses">The responses.</param>
        public EwsResult(IEnumerable<HttpResponse> responses)
        {
            _responses = responses.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsResult"/> class.
        /// </summary>
        /// <param name="responses">The responses.</param>
        /// <param name="ex">The ex.</param>
        public EwsResult(IEnumerable<HttpResponse> responses, Exception ex)
            : this(responses)
        {
            Exception = ex;
        }
    }
}
