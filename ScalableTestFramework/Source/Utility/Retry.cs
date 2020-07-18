using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides methods for retrying actions.
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// Retries a boolean function until it returns true or the number of attempts is reached.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="maximumRetries">The maximum number of times to retry the action.</param>
        /// <param name="retryDelay">The amount of time to wait between retries.</param>
        /// <returns><c>true</c> if the function returns true within the specified number of retries, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRetries" /> is zero or fewer.
        /// <para>or</para>
        /// <paramref name="retryDelay" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public static bool UntilTrue(Func<bool> action, int maximumRetries, TimeSpan retryDelay)
        {
            ValidateParameters(action, maximumRetries, retryDelay);

            bool result = action();
            while (result == false && maximumRetries-- > 0)
            {
                LogTrace($"Action returned false.  Waiting for {retryDelay} before retrying...");
                Thread.Sleep(retryDelay);
                result = action();
            }
            return result;
        }

        /// <summary>
        /// Retries an action until it does not throw the specified exception or the number of attempts is exceeded.
        /// </summary>
        /// <typeparam name="TException">The type of <see cref="Exception" /> to catch and retry.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="maximumRetries">The maximum number of times to retry the action.</param>
        /// <param name="retryDelay">The amount of time to wait between retries.</param>
        /// <returns>
        /// The number of times the action was retried before it succeeded.
        /// (A value of zero indicates the action succeeded on the first attempt).
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRetries" /> is zero or fewer.
        /// <para>or</para>
        /// <paramref name="retryDelay" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static int WhileThrowing<TException>(Action action, int maximumRetries, TimeSpan retryDelay) where TException : Exception
        {
            ValidateParameters(action, maximumRetries, retryDelay);

            for (int i = 0; i < maximumRetries; i++)
            {
                try
                {
                    action();
                    return i;
                }
                catch (TException ex)
                {
                    LogDebug($"Action threw {ex.GetType()}: {ex.Message}");
                    LogTrace($"Complete exception details: {ex}");
                    LogDebug($"Waiting for {retryDelay} before retrying...");
                    Thread.Sleep(retryDelay);
                }
            }

            // Don't catch the exception if the action fails on the last call
            action();
            return maximumRetries;
        }

        /// <summary>
        /// Retries an action until it does not throw one of the specified exceptions or the number of attempts is exceeded.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="maximumRetries">The maximum number of times to retry the action.</param>
        /// <param name="retryDelay">The amount of time to wait between retries.</param>
        /// <param name="exceptionTypes">A list of <see cref="Exception" /> types to catch and retry.</param>
        /// <returns>
        /// The number of times the action was retried before it succeeded.
        /// (A value of zero indicates the action succeeded on the first attempt).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is null.
        /// <para>or</para>
        /// <paramref name="exceptionTypes" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRetries" /> is zero or fewer.
        /// <para>or</para>
        /// <paramref name="retryDelay" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public static int WhileThrowing(Action action, int maximumRetries, TimeSpan retryDelay, IEnumerable<Type> exceptionTypes)
        {
            ValidateParameters(action, maximumRetries, retryDelay);

            if (exceptionTypes == null)
            {
                throw new ArgumentNullException(nameof(exceptionTypes));
            }

            for (int i = 0; i < maximumRetries; i++)
            {
                try
                {
                    action();
                    return i;
                }
                catch (Exception ex) when (exceptionTypes.Any(n => ex.GetType() == n || ex.GetType().IsSubclassOf(n)))
                {
                    LogDebug($"Action threw {ex.GetType()}: {ex.Message}");
                    LogTrace($"Complete exception details: {ex}");
                    LogDebug($"Waiting for {retryDelay} before retrying...");
                    Thread.Sleep(retryDelay);
                }
            }

            // Don't catch the exception if the action fails on the last call
            action();
            return maximumRetries;
        }

        private static void ValidateParameters(Delegate action, int attempts, TimeSpan retryDelay)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (attempts < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(attempts), "Attempts must be greater than zero.");
            }

            if (retryDelay < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(retryDelay), "Retry delay must be greater than zero.");
            }
        }
    }
}
