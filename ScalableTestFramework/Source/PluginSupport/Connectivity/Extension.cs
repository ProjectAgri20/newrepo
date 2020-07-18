using System;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public static class Extension
    {
        /// <summary>
        /// Formats the string.
        /// </summary>
        /// <param name="value">Format string</param>
        /// <param name="args">arguments to place into the formatted string.</param>
        /// <returns>Completed string.</returns>
        /// <example>
        /// This is a simple helper, but not much different from string.Format()
        /// <code>
        /// Logger.LogDebug("STDOUT: {0}".FormatWith(stdOut));
        /// </code>
        /// </example>
        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, value, args);
        }

        /// <summary>
        /// Get the <see cref="Exception.Message"/>s of the supplied <paramref name="exception"/> and all <see cref="Exception.InnerException"/>s.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/>, or <c>null</c>.</param>
        /// <returns>A string with all exceptions down through each inner exception concatenated together</returns>
        public static string JoinAllErrorMessages(this Exception exception)
        {
            if (exception != null)
            {
                if (exception.InnerException != null)
                {
                    return exception.Message + Environment.NewLine + "InnerException: " + exception.InnerException.JoinAllErrorMessages();
                }
                return exception.Message;
            }
            return string.Empty;
        }
    }
}
