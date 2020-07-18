using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework.Synchronization;

namespace PluginSimulator
{
    internal sealed class CriticalSectionMockInternal : ICriticalSection, ICriticalSectionMock
    {
        private readonly Random _random = new Random();
        private readonly CriticalSectionMock _criticalSectionMock = new CriticalSectionMock();

        /// <summary>
        /// Gets or sets the <see cref="CriticalSectionMockBehavior" /> used when methods on this instance are called.
        /// </summary>
        /// <value>The behavior.</value>
        public CriticalSectionMockBehavior Behavior
        {
            get { return _criticalSectionMock.Behavior; }
            set { _criticalSectionMock.Behavior = value; }
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
            _criticalSectionMock.Run(token, action);
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
            _criticalSectionMock.RunConcurrent(token, action, concurrencyCount);
        }

        /// <summary>
        /// Selects a <see cref="LockToken" /> from the provided list, based on current
        /// availability of the associated resources, then mocks the critical section service
        /// by performing the action designated by <see cref="Behavior" />.
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

            if (!tokens.Any())
            {
                throw new ArgumentException("At least one token must be provided.", nameof(tokens));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            switch (Behavior)
            {
                case CriticalSectionMockBehavior.ThrowAcquireTimeoutException:
                    throw new AcquireLockTimeoutException("The lock could not be acquired within the designated time.");

                case CriticalSectionMockBehavior.ThrowHoldTimeoutException:
                    throw new HoldLockTimeoutException("The specified action did not complete within the designated time.");
            }

            List<LockToken> tokenList = tokens.ToList();
            LockToken selectedToken = tokenList[_random.Next(tokenList.Count)];
            action(selectedToken);
        }
    }
}
