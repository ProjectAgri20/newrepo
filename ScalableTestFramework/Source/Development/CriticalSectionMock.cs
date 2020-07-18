using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock implementation of <see cref="ICriticalSection" /> for development.
    /// </summary>
    public sealed class CriticalSectionMock : ICriticalSection, ICriticalSectionMock
    {
        /// <summary>
        /// Gets or sets the <see cref="CriticalSectionMockBehavior" /> used when methods on this instance are called.
        /// </summary>
        public CriticalSectionMockBehavior Behavior { get; set; } = CriticalSectionMockBehavior.RunAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalSectionMock" /> class.
        /// </summary>
        public CriticalSectionMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalSectionMock" /> class with the specified <see cref="CriticalSectionMockBehavior" />.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public CriticalSectionMock(CriticalSectionMockBehavior behavior)
        {
            Behavior = behavior;
        }

        /// <summary>
        /// Mocks the critical section service by performing the action designated by <see cref="Behavior" />.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution (ignored).</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowAcquireTimeoutException" />.</exception>
        /// <exception cref="HoldLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowHoldTimeoutException" />.</exception>
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

            Run(action);
        }

        /// <summary>
        /// Mocks the critical section service by selecting a token and performing the action designated by <see cref="Behavior" />.
        /// </summary>
        /// <param name="tokens">The collection of tokens to choose from.</param>
        /// <param name="action">The action to execute; takes the acquired token as a parameter.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tokens" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="tokens" /> contains no elements.</exception>
        /// <exception cref="AcquireLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowAcquireTimeoutException" />.</exception>
        /// <exception cref="HoldLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowHoldTimeoutException" />.</exception>
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

            Random random = new Random();
            LockToken token = tokens.OrderBy(n => random.Next()).First();
            Run(() => action(token));
        }

        /// <summary>
        /// Mocks the critical section service by performing the action designated by <see cref="Behavior" />.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution (ignored).</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="concurrencyCount">The number of clients that are allowed to execute concurrently (ignored).</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowAcquireTimeoutException" />.</exception>
        /// <exception cref="HoldLockTimeoutException">This instance is configured with <see cref="CriticalSectionMockBehavior.ThrowHoldTimeoutException" />.</exception>
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

            Run(action);
        }

        private void Run(Action action)
        {
            switch (Behavior)
            {
                case CriticalSectionMockBehavior.ThrowAcquireTimeoutException:
                    throw new AcquireLockTimeoutException("The lock could not be acquired within the designated time.");

                case CriticalSectionMockBehavior.ThrowHoldTimeoutException:
                    throw new HoldLockTimeoutException("The specified action did not complete within the designated time.");
            }

            action();
        }
    }
}
