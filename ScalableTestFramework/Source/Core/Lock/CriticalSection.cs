using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Implementation of <see cref="ICriticalSection" /> that uses an <see cref="ILockManager" /> to handle concurrency.
    /// </summary>
    public sealed class CriticalSection : ICriticalSection
    {
        private readonly ILockManager _lockManager;
        private readonly string _requestName = string.Format("{0}-{1}", Environment.MachineName, Environment.UserName);

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalSection" /> class.
        /// </summary>
        /// <param name="lockManager">The <see cref="ILockManager" /> to use for acquiring resource locks.</param>
        /// <exception cref="ArgumentNullException"><paramref name="lockManager" /> is null.</exception>
        public CriticalSection(ILockManager lockManager)
        {
            _lockManager = lockManager ?? throw new ArgumentNullException(nameof(lockManager));
        }

        /// <summary>
        /// Obtains an exclusive lock according to the specified <see cref="LockToken" />,
        /// then executes the specified <see cref="Action" /> before releasing the lock.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by <paramref name="token" />.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by <paramref name="token" />.</exception>
        public void Run(LockToken token, Action action)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            LockTicket lockTicket = _lockManager.AcquireLock(token.Key, token.AcquireTimeout, 1, _requestName);
            try
            {
                RunAction(token, action);
            }
            finally
            {
                _lockManager.ReleaseLock(lockTicket);
            }
        }

        /// <summary>
        /// Selects a <see cref="LockToken" /> from the provided list, based on current
        /// availability of the associated resources, then obtains an exlusive lock
        /// and executes the specified <see cref="Action{LockToken}" /> before releasing the lock.
        /// </summary>
        /// <param name="tokens">The collection of tokens to choose from.</param>
        /// <param name="action">The action to execute; takes the acquired token as a parameter.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tokens" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="tokens" /> contains no elements.</exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by the selected token.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by the selected token.</exception>
        public void Run(IEnumerable<LockToken> tokens, Action<LockToken> action)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (!tokens.Any())
            {
                throw new ArgumentException("No lock tokens were specified.", nameof(tokens));
            }

            // Send the lock tokens in a random order to prevent picking the same one every time if they are all available
            Random random = new Random();
            IEnumerable<string> tokenIds = tokens.Select(n => n.Key).OrderBy(n => random.Next());
            LockTicket lockTicket = _lockManager.AcquireLock(tokenIds, tokens.First().AcquireTimeout, 1, _requestName);
            try
            {
                LockToken acquiredToken = tokens.First(n => n.Key == lockTicket.ResourceId);
                RunAction(acquiredToken, () => action(acquiredToken));
            }
            finally
            {
                _lockManager.ReleaseLock(lockTicket);
            }
        }

        /// <summary>
        /// Obtains a semaphore-based lock according to the specified <see cref="LockToken" />,
        /// then executes the specified <see cref="Action" /> before releasing the lock.
        /// The number of clients allowed to simultaneously hold the lock is defined by the concurrency count.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="concurrencyCount">The number of clients that are allowed to execute concurrently.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by <paramref name="token" />.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by <paramref name="token" />.</exception>
        public void RunConcurrent(LockToken token, Action action, int concurrencyCount)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            LockTicket lockTicket = _lockManager.AcquireLock(token.Key, token.AcquireTimeout, concurrencyCount, _requestName);
            try
            {
                RunAction(token, action);
            }
            finally
            {
                _lockManager.ReleaseLock(lockTicket);
            }
        }

        private static void RunAction(LockToken token, Action action)
        {
            bool finished = RunAction(action, token.HoldTimeout);
            if (!finished)
            {
                throw new HoldLockTimeoutException($"Action did not complete within {token.HoldTimeout}.");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is rethrown via ExceptionDispatchInfo.")]
        private static bool RunAction(Action action, TimeSpan timeout)
        {
            ExceptionDispatchInfo exceptionInfo = null;
            void runAction()
            {
                try
                {
                    action();
                }
                catch (ThreadAbortException)
                {
                    // Reset the abort so that the thread will exit cleanly.
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    // This will preserve the stack trace from the exception thrown inside the action.
                    exceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
            }

            // Start a background thread to perform the specified action.
            Thread myActionThread = new Thread(runAction);
            myActionThread.IsBackground = true;
            myActionThread.Start();

            bool result = myActionThread.Join(timeout);
            if (!result)
            {
                // The action did not finish in time.  Signal the thread to abort
                // and then wait for it to finish.
                myActionThread.Abort();
                myActionThread.Join();
            }
            else if (exceptionInfo != null)
            {
                // The action threw an exception, which will have been captured.
                // Rethrow the exception - stack trace will be preserved by ExceptionDispatchInfo.
                exceptionInfo.Throw();
            }

            return result;
        }
    }
}
