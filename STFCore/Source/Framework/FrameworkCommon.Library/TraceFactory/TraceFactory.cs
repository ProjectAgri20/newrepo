using System;
using HP.ScalableTest.Core;

namespace HP.ScalableTest
{
    /// <summary>
    /// Static class used to trace out log statements using the Log4Net library. 
    /// </summary>
    public static class TraceFactory
    {
        private static ScalableTestLogger _logger = null;

        /// <summary>
        /// Sets a new thread context property that can be used in the log file.
        /// </summary>
        /// <remarks>
        /// The application configuration file must leverage this data by including 
        /// the property name in the path or name of the file.  For example,
        /// in STF the username is included in the log file for the worker console.
        /// </remarks>
        /// <example>
        /// Below is an example of the call to set the variable and what should be set in the
        /// application configuration file.
        /// <code>
        /// TraceFactory.SetThreadContextProperty("SessionId", manifest.SessionId, false);
        /// 
        /// &lt;appender name="LogFile" type="log4net.Appender.RollingFileAppender">
        ///     &lt;file type="log4net.Util.PatternString" value="Logs/%property{SessionId}.log"/>
        /// </code>
        /// </example>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set for the property.</param>
        public static void SetThreadContextProperty(string name, string value, bool throwExceptionIfAlreadyInitialized = true)
        {
            if (throwExceptionIfAlreadyInitialized)
            {
                if (_logger != null)
                {
                    throw new InvalidOperationException("The Trace Factory is already initialized, new properties can't be added");
                }
            }
            try
            {
                SystemTraceLogger.SetGlobalProperty(name, value);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to set log4net thread context {0} to {1}: {2}".FormatWith(name, value, ex));
            }
        }

        /// <summary>
        /// Gets the logger for this trace factory.
        /// </summary>
        public static ScalableTestLogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new ScalableTestLogger();
                    _logger.Initialize();
                }
                return _logger;
            }
        }

        /// <summary>
        /// Sets the session Id in as a thread context property..
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public static void SetSessionContext(string sessionId)
        {
            SetThreadContextProperty("SessionId", sessionId, false);
        }
    }
}
