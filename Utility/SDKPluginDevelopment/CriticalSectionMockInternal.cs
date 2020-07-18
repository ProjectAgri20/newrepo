using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework.Synchronization;

namespace SDKPluginDevelopment
{
    /// <summary>
    /// Wrapper around <see cref="CriticalSectionMock" /> that also implements <see cref="ICriticalSectionInternal" />.
    /// </summary>
    public sealed class CriticalSectionMockInternal : ICriticalSectionInternal, ICriticalSectionMock
    {
        private readonly Random _random = new Random();
        private readonly CriticalSectionMock _criticalSectionMock;

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
        /// Initializes a new instance of the <see cref="CriticalSectionMockInternal" /> class.
        /// </summary>
        public CriticalSectionMockInternal()
        {
            _criticalSectionMock = new CriticalSectionMock();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalSectionMockInternal" /> class with the specified <see cref="CriticalSectionMockBehavior" />.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public CriticalSectionMockInternal(CriticalSectionMockBehavior behavior)
        {
            _criticalSectionMock = new CriticalSectionMock(behavior);
        }

        // Reroute all ICriticalSection calls to the mock
        public void Run(LockToken token, Action action) => _criticalSectionMock.Run(token, action);
        public void RunConcurrent(LockToken token, Action action, int concurrencyCount) => _criticalSectionMock.RunConcurrent(token, action, concurrencyCount);

        // Implement the ICriticalSectionInternal extensions
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
