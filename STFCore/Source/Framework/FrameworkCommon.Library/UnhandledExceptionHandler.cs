using System;
using System.Security;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Used to manage any unhandled exceptions that may occur in the program.
    /// </summary>
    /// <example>
    /// <para>
    /// How to use the event handler in a service or non-interactive program:
    /// <code>
    /// class Program
    /// {
    ///     static void Main(string[] args)
    ///     {
    ///         UnhandledExceptionHandler.Attach();
    ///         // more code here
    ///     }
    /// }
    /// </code>
    /// </para>
    /// </example>
    public static class UnhandledExceptionHandler
    {
        /// <summary>
        /// Adds the <see cref="UnhandledExceptionEventHandler"/> <see cref="UnhandledExceptionMethod"/>
        /// to the current <see cref="AppDomain.UnhandledException"/> event.
        /// </summary>
        [SecurityCritical]
        public static void Attach()
        {
            TraceFactory.Logger.Debug("Attaching unhandled exception handler.");
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionMethod;
        }

        /// <summary>
        /// Method that to handle the event raised by an exception
        /// that is not handled by the application domain.
        /// Logs the error and silently kills the application.
        /// </summary>
        public static UnhandledExceptionEventHandler UnhandledExceptionMethod
        {
            get
            {
                return OnUnhandledException;
            }
        }

        /// <summary>
        /// Called when an unhandled exception occurs.
        /// Logs the error and silently kills the application.
        /// </summary>
        /// <param name="sender">The source of the unhandled exception event. Ignored.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            TraceFactory.Logger.Fatal(error);
            Environment.Exit(1);
        }
    }
}
