using System;
using System.Text;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Adapter class for <see cref="Logger" />.
    /// Introduced to avoid code turmoil for extensive existing log statements.
    /// </summary>
    public static class TraceFactory
    {
        public static TraceFactoryLogger Logger { get; } = new TraceFactoryLogger();

        public class TraceFactoryLogger
        {
            public void Debug(object message) => Framework.Logger.LogDebug(message);
            public void Info(object message) => Framework.Logger.LogInfo(message);
            public void Fatal(object message) => Framework.Logger.LogError(message);
            public void Error(object message) => Framework.Logger.LogError(message);
            public void Error(object message, Exception ex) => Framework.Logger.LogError(message, ex);
        }

        /// <summary>
        /// Gets the thread context property value for the given property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>System.String value for the specified property.</returns>
        public static string GetThreadContextProperty(string name)
        {
            string result = null;
            try
            {
                var property = log4net.ThreadContext.Properties[name];
                if (property == null)
                {
                    property = log4net.GlobalContext.Properties[name];
                }
                result = (property == null ? string.Empty : property.ToString());

                if (result == "(null)")
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to get log4net thread context for {0}: {1}".FormatWith(name, ex));
            }
            return result;
        }

        /// <summary>
        /// Logs the debug result.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        public static void LogDebugResult(string title, string body)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(title);
            if (!string.IsNullOrEmpty(body))
            {
                builder.Append(Environment.NewLine);
                builder.Append(body);
            }
            Logger.Debug(builder.ToString());
        }
    }
}
